Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class secure_document : Inherits Frontend.clsCMSPage

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim oQuote As NexusProvider.Quote
            Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            Dim iPartyKey As Integer
            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
            Dim oParty As NexusProvider.BaseParty = Session(CNParty)
            Dim sDocumentRef As String = Request.QueryString("DocRef")
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sClientName As String = String.Empty
            Dim sClientCode As String = String.Empty
            Dim iDoc_Number As Integer = 0
            If Not String.IsNullOrEmpty(Session(CNDocumentRef)) Then
                iDoc_Number = CInt(Session(CNDocumentRef))
            End If
            If Not Session("SelectedDocId") Then
                iDoc_Number = CInt(Session("SelectedDocId"))
            End If
            If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                oQuote = Session(CNClaimQuote)

                If oClaim.Client.PartyKey = 0 AndAlso oParty IsNot Nothing Then
                    iPartyKey = oParty.Key
                Else
                    iPartyKey = oClaim.Client.PartyKey
                End If

            Else
                oQuote = Session(CNQuote)
            End If

            'if Oparty is nothing
            If oQuote IsNot Nothing Then
                If oParty Is Nothing AndAlso oQuote.PartyKey > 0 Then
                    Dim sBranchCode As String = Nothing
                    If String.IsNullOrEmpty(oQuote.BranchCode) = False Then
                        sBranchCode = oQuote.BranchCode
                    End If
                    oParty = oWebService.GetParty(oQuote.PartyKey, sBranchCode)
                End If
            End If


            If Not String.IsNullOrEmpty(Request.QueryString("doc")) Then
                ' this section will be called from the Transaction Confirmation page
                Dim oDocuments As Config.Documents = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork) _
                    .Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode).Documents
                If oDocuments.DocTemplate(Request.QueryString("doc")) Is Nothing Then
                    Response.StatusCode = 404
                    Response.End()
                    Response.Flush()
                Else
                    Dim sDocumentDirName As String = oDocuments.Location
                    sDocumentDirName = Right(sDocumentDirName, (sDocumentDirName.Length - sDocumentDirName.LastIndexOf("\")))
                    Dim sDocumentCode As String = oDocuments.DocTemplate(Request.QueryString("doc")).Code()

                    Dim sFileName As String = oDocuments.DocTemplate(Request.QueryString("doc")).FileName
                    Dim sTagFileName As String = Nothing
                    Dim sTagFileFormat As String = Nothing
                    Dim sGUID As String = Guid.NewGuid.ToString
                    Dim sPolicyNo As String = oQuote.InsuranceFileRef.Trim()
                    Dim sDocumentName As String = oDocuments.DocTemplate(Request.QueryString("doc")).Name

                    'If TagFileName is Provided
                    If String.IsNullOrEmpty(oDocuments.DocTemplate(Request.QueryString("doc")).TagFileName) = False Then
                        sTagFileName = oDocuments.DocTemplate(Request.QueryString("doc")).TagFileName
                    End If

                    'If TagFileFormat is Provided
                    If String.IsNullOrEmpty(oDocuments.DocTemplate(Request.QueryString("doc")).TagFileFormat) = False Then
                        sTagFileFormat = oDocuments.DocTemplate(Request.QueryString("doc")).TagFileFormat
                    End If

                    sPolicyNo = sPolicyNo.Replace("/", "-")
                    sPolicyNo = sPolicyNo.Replace("\", "-")
                    Dim sClaimNo As String = Nothing

                    If Session(CNClaimNumber) IsNot Nothing Then
                        sClaimNo = Session(CNClaimNumber)
                        sClaimNo = sClaimNo.Replace("/", "-")
                        sClaimNo = sClaimNo.Replace("\", "-")
                    End If

                    If oUserDetails IsNot Nothing Then

                        sFileName = sFileName.Replace("[!UserID!]", oUserDetails.ResolvedName)

                        If sTagFileName IsNot Nothing Then
                            sTagFileName = sTagFileName.Replace("[!UserID!]", oUserDetails.ResolvedName)
                        End If

                        If sTagFileFormat IsNot Nothing Then
                            sTagFileFormat = sTagFileFormat.Replace("[!UserID!]", oUserDetails.ResolvedName)
                        End If
                    ElseIf oParty IsNot Nothing Then
                        sFileName = sFileName.Replace("[!UserID!]", oParty.TPIntroducer)

                        If sTagFileName IsNot Nothing Then
                            sTagFileName = sTagFileName.Replace("[!UserID!]", oParty.TPIntroducer)
                        End If

                        If sTagFileFormat IsNot Nothing Then
                            sTagFileFormat = sTagFileFormat.Replace("[!UserID!]", oParty.TPIntroducer)
                        End If

                    End If

                    If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                        sFileName = sFileName.Replace("[!ClientCode!]", oClaim.Client.ShortName.Trim())
                        sFileName = sFileName.Replace("[!ClientName!]", oClaim.Client.ClientName.Trim())

                        If sTagFileName IsNot Nothing Then
                            sTagFileName = sTagFileName.Replace("[!ClientCode!]", oClaim.Client.ShortName.Trim())
                            sTagFileName = sTagFileName.Replace("[!ClientName!]", oClaim.Client.ClientName.Trim())
                        End If

                        If sTagFileFormat IsNot Nothing Then
                            sTagFileFormat = sTagFileFormat.Replace("[!ClientCode!]", oClaim.Client.ShortName.Trim())
                            sTagFileFormat = sTagFileFormat.Replace("[!ClientName!]", oClaim.Client.ClientName.Trim())
                        End If

                    ElseIf oParty IsNot Nothing Then
                        iPartyKey = oParty.Key
                        Select Case True
                            Case TypeOf oParty Is NexusProvider.CorporateParty
                                With CType(oParty, NexusProvider.CorporateParty)
                                    sClientName = DirectCast(oParty, NexusProvider.CorporateParty).ClientSharedData.ResolvedName
                                    sClientCode = DirectCast(oParty, NexusProvider.CorporateParty).ClientSharedData.ShortName
                                End With
                            Case TypeOf oParty Is NexusProvider.PersonalParty
                                With CType(oParty, NexusProvider.PersonalParty)
                                    sClientName = DirectCast(oParty, NexusProvider.PersonalParty).ClientSharedData.ResolvedName
                                    sClientCode = DirectCast(oParty, NexusProvider.PersonalParty).ClientSharedData.ShortName
                                End With
                        End Select

                        If (String.IsNullOrWhiteSpace(sClientCode)) Then
                            sFileName = sFileName.Replace("[!ClientCode!]", sClientCode)
                        End If
                        If (sClientName <> "") Then
                            sFileName = sFileName.Replace("[!ClientName!]", sClientName.Trim())
                        End If



                        If sTagFileName IsNot Nothing Then
                            If (String.IsNullOrWhiteSpace(sClientCode)) Then
                                sTagFileName = sTagFileName.Replace("[!ClientCode!]", sClientCode)
                            End If
                            If (sClientName <> "") Then
                                sTagFileName = sFileName.Replace("[!ClientName!]", sClientName.Trim())
                            End If
                        End If

                        If sTagFileFormat IsNot Nothing Then
                            If (String.IsNullOrWhiteSpace(sClientCode)) Then
                                sTagFileFormat = sTagFileFormat.Replace("[!ClientCode!]", sClientCode)
                            End If
                            If (sClientName <> "") Then
                                sTagFileFormat = sFileName.Replace("[!ClientName!]", sClientName.Trim())
                            End If
                        End If

                    End If

                    ' File Name concatenation
                    sFileName = sFileName.Replace("[!ProductName!]", oQuote.ProductCode)
                    sFileName = sFileName.Replace("[!PolicyNumber!]", sPolicyNo)

                    If sClaimNo IsNot Nothing Then
                        sFileName = sFileName.Replace("[!ClaimNumber!]", sClaimNo)
                    Else
                        sFileName = sFileName.Replace("[!ClaimNumber!]", "")
                    End If

                    sFileName = sFileName.Replace("[!BranchCode!]", oQuote.BranchCode)
                    sFileName = sFileName.Replace("[!BranchName!]", oQuote.BranchCode)
                    sFileName = sFileName.Replace("[!TransactionType!]", oQuote.TransactionType.ToString())
                    sFileName = sFileName.Replace("[!GUID!]", sGUID)
                    sFileName = sFileName.Replace("[!InsuranceFileKey!]", oQuote.InsuranceFileKey)
                    sFileName = sFileName.Replace("[!CurrentDate!]", Date.Now().ToString("dd-MMMM-yyyy"))
                    'Tag File Name concatenation
                    If sTagFileName IsNot Nothing Then
                        sTagFileName = sTagFileName.Replace("[!ProductName!]", oQuote.ProductCode)
                        sTagFileName = sTagFileName.Replace("[!PolicyNumber!]", sPolicyNo)
                        If sClaimNo IsNot Nothing Then
                            sTagFileName = sTagFileName.Replace("[!ClaimNumber!]", sClaimNo)
                        Else
                            sTagFileName = sTagFileName.Replace("[!ClaimNumber!]", "")
                        End If

                        sTagFileName = sTagFileName.Replace("[!BranchCode!]", oQuote.BranchCode)
                        sTagFileName = sTagFileName.Replace("[!BranchName!]", oQuote.BranchName)
                        sTagFileName = sTagFileName.Replace("[!TransactionType!]", oQuote.TransactionType.ToString())
                        sTagFileName = sTagFileName.Replace("[!GUID!]", sGUID)
                        sTagFileName = sTagFileName.Replace("[!InsuranceFileKey!]", oQuote.InsuranceFileKey)
                        sTagFileName = sTagFileName.Replace("[!CurrentDate!]", Date.Now().ToString("dd-MMMM-yyyy"))
                    End If

                    If Session(CNProduceDocument) IsNot Nothing Then
                        sFileName = sFileName.Replace("[!DocumentRef!]", sDocumentRef)
                    End If
                    'Tag File Format
                    If sTagFileFormat IsNot Nothing Then
                        sTagFileFormat = sTagFileFormat.Replace("[!ProductName!]", oQuote.ProductCode)
                        sTagFileFormat = sTagFileFormat.Replace("[!PolicyNumber!]", oQuote.InsuranceFileRef)
                        If sClaimNo IsNot Nothing Then
                            sTagFileFormat = sTagFileFormat.Replace("[!ClaimNumber!]", sClaimNo)
                        Else
                            sTagFileFormat = sTagFileFormat.Replace("[!ClaimNumber!]", "NA")
                        End If
                        'sTagFileFormat = sTagFileFormat.Replace("[!ClaimNumber!]", oQuote.InsuranceFileRef)
                        sTagFileFormat = sTagFileFormat.Replace("[!BranchCode!]", oQuote.BranchCode)
                        sTagFileFormat = sTagFileFormat.Replace("[!BranchName!]", oQuote.BranchName)
                        sTagFileFormat = sTagFileFormat.Replace("[!TransactionType!]", oQuote.TransactionType.ToString())
                        sTagFileFormat = sTagFileFormat.Replace("[!GUID!]", sGUID)
                        'TO DO RISKDATA (NEED TO IMPROVE LOGIC)
                        Dim sTemp() As String = sTagFileFormat.Split(",")
                        Dim iCounter As Integer = 0

                        For iCounter = 0 To sTemp.Length - 1
                            If sTemp(iCounter).ToUpper.Contains("RISKDATA") = True Then
                                sTagFileFormat = sTagFileFormat.Replace(sTemp(iCounter), "")
                                sTemp(iCounter) = sTemp(iCounter).Replace("[", "")
                                sTemp(iCounter) = sTemp(iCounter).Replace("]", "")
                                sTemp(iCounter) = sTemp(iCounter).Replace("!", "")
                                sTemp(iCounter) = sTemp(iCounter).Replace("(", "")
                                sTemp(iCounter) = sTemp(iCounter).Replace(")", "")
                                sTemp(iCounter) = sTemp(iCounter).Replace("RiskData", "")
                                sTemp(iCounter) = sTemp(iCounter).Replace("'", "")

                                Dim iCount As Integer = 0
                                For iCount = 0 To oQuote.Risks.Count - 1
                                    If oQuote.Risks(iCount).XMLDataset IsNot Nothing Then
                                        Dim strDataset As New System.IO.StringReader(oQuote.Risks(iCount).XMLDataset)
                                        Dim DataModelCode As String = Session(CNDataModelCode)
                                        Dim NavString As String = "DATA_SET/RISK_OBJECTS/" & DataModelCode & "_POLICY_BINDER/" & sTemp(iCounter)
                                        Dim Navigator As System.Xml.XPath.XPathNavigator
                                        Dim trDataset As New System.Xml.XmlTextReader(strDataset)
                                        Dim Doc As System.Xml.XPath.XPathDocument = New System.Xml.XPath.XPathDocument(trDataset)
                                        Navigator = Doc.CreateNavigator()
                                        Dim NodeI As System.Xml.XPath.XPathNodeIterator = Navigator.Select(NavString)
                                        While NodeI.MoveNext
                                            sTagFileFormat = sTagFileFormat & NodeI.Current.Value
                                            sTagFileFormat = sTagFileFormat & ";"
                                        End While
                                    End If
                                Next
                            End If
                        Next
                    End If

                    Try
                        Dim oFinancePlan As New NexusProvider.FinancePlan
                        oFinancePlan = CType(Session(CNFinancePlan), NexusProvider.FinancePlan)
                        Dim oPaymentOptions As Nexus.Library.Config.PaymentTypes = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).PaymentTypes

                        Dim paymentType As Config.PaymentType = oPaymentOptions.PaymentType(oQuote.PaymentMethodCode.Trim)
                        Dim sPaymentTypeType As String = If((Session(CNSelectedPaymentIndex) Is Nothing AndAlso paymentType IsNot Nothing), paymentType.Type, oPaymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).Type)

                        If oFinancePlan IsNot Nothing AndAlso oFinancePlan.FinancePlanKey <> 0 Then
                            If Session(CNMode) <> Nexus.Constants.Mode.View AndAlso sPaymentTypeType.Trim() = "PremiumFinance" Then
                                If Session(CNInstalmentDatesUpdated) = "1" Then
                                    Dim oPayment As NexusProvider.Payment = Nothing
                                    oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.None, CDec(HttpContext.Current.Session(CNAmountToPay)))
                                    oPayment = HttpContext.Current.Session(CNPayment)
                                    oWebService.SavePremiumFinanceDetails(oPayment, oQuote.InsuranceFileKey, oQuote.CoverStartDate)
                                End If
                            End If
                        End If
                        'write the file to the browser then close the response
                        Select Case CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).DocumentFormat.ToUpper()
                            Case "HTML"
                                'sFileName = sFileName & ".htm"
                                sFileName = sFileName & ".doc"
                                If Request.QueryString("PreGenerate").ToLower = "true" And Not IO.File.Exists(Server.MapPath("~/" & sDocumentDirName) & "\" & sFileName) Then
                                    Dim oFile As String
                                    If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                                        If Session(CNProduceDocument) IsNot Nothing Then

                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey,
                                                                                 oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.HTML, Server.MapPath("~/" & sDocumentDirName) & "\" & sFileName, oClaim.ClaimKey, Nothing, Nothing, sDocumentRef)
                                        Else
                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey,
                                                                                            oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.HTML, Server.MapPath("~/" & sDocumentDirName) & "\" & sFileName, oClaim.ClaimKey)
                                        End If

                                    Else
                                        If Session(CNProduceDocument) IsNot Nothing Then

                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey,
                                                oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.HTML, Server.MapPath("~/" & sDocumentDirName) & "\" & sFileName, Nothing, Nothing, Nothing, sDocumentRef, sDocumentName:=sDocumentName)
                                        Else
                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey,
                                               oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.HTML, Server.MapPath("~/" & sDocumentDirName) & "\" & sFileName, sDocumentName:=sDocumentName)
                                        End If
                                    End If

                                    CreateTagFile(oDocuments.Location, sTagFileName, sTagFileFormat)

                                    Response.Redirect("~/" & sDocumentDirName & "\" & sFileName, False)
                                ElseIf Request.QueryString("PreGenerate").ToLower = "true" And IO.File.Exists(Server.MapPath("~/" & sDocumentDirName) & "\" & sFileName) = True Then
                                    Response.Redirect("~/" & sDocumentDirName & "\" & sFileName, False)
                                ElseIf Request.QueryString("PreGenerate").ToLower = "false" Then
                                    Dim oFile As String
                                    If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                                        If Session(CNProduceDocument) IsNot Nothing Then

                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey,
                                                                                 oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.HTML, Server.MapPath("~/" & sDocumentDirName) & "\" & sFileName, oClaim.ClaimKey, Nothing, Nothing, sDocumentRef)
                                        Else
                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey,
                                                                                            oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.HTML, Server.MapPath("~/" & sDocumentDirName) & "\" & sFileName, oClaim.ClaimKey)
                                        End If
                                    Else
                                        If Session(CNProduceDocument) IsNot Nothing Then

                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey,
                                                oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.HTML, Server.MapPath("~/" & sDocumentDirName) & "\" & sFileName, Nothing, Nothing, Nothing, sDocumentRef, sDocumentName:=sDocumentName)
                                        Else
                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey,
                                               oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.HTML, Server.MapPath("~/" & sDocumentDirName) & "\" & sFileName, sDocumentName:=sDocumentName)
                                        End If
                                    End If

                                    CreateTagFile(oDocuments.Location, sTagFileName, sTagFileFormat)

                                    Response.Redirect("~/" & sDocumentDirName & "\" & sFileName, False)
                                End If

                            Case "PDF"

                                sFileName = sFileName & ".pdf"

                                If Request.QueryString("PreGenerate").ToString.ToLower = "true" And Not IO.File.Exists(oDocuments.Location & "\" & sFileName) Then
                                    Dim oFile As String

                                    If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                                        If Session(CNProduceDocument) IsNot Nothing Then

                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey,
                                                                                 oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.PDF, oDocuments.Location & "\" & sFileName, oClaim.ClaimKey, Nothing, Nothing, sDocumentRef)
                                        Else
                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey,
                                                                                            oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.PDF, oDocuments.Location & "\" & sFileName, oClaim.ClaimKey)
                                        End If
                                    Else
                                        If Session(CNProduceDocument) IsNot Nothing Then

                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey,
                                                oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.PDF, oDocuments.Location & "\" & sFileName, Nothing, Nothing, Nothing, sDocumentRef, sDocumentName:=sDocumentName)
                                        Else
                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey,
                                               oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.PDF, oDocuments.Location & "\" & sFileName, sDocumentName:=sDocumentName)
                                        End If
                                    End If

                                    CreateTagFile(oDocuments.Location, sTagFileName, sTagFileFormat)

                                    Response.ClearHeaders()
                                    Response.ContentType = "Application/pdf"
                                    Response.AddHeader("Content-Disposition", "inline; filename=document.pdf")
                                    Response.WriteFile(oDocuments.Location & "\" & sFileName)
                                ElseIf Request.QueryString("PreGenerate").ToString.ToLower = "true" And IO.File.Exists(oDocuments.Location & "\" & sFileName) Then
                                    Response.ClearHeaders()
                                    Response.ContentType = "Application/pdf"
                                    Response.AddHeader("Content-Disposition", "inline; filename=document.pdf")
                                    Response.WriteFile(oDocuments.Location & "\" & sFileName)
                                ElseIf Request.QueryString("PreGenerate").ToString.ToLower = "false" Then
                                    Dim oFile As String

                                    If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                                        If Session(CNProduceDocument) IsNot Nothing Then

                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey,
                                                                                 oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.PDF, oDocuments.Location & "\" & sFileName, oClaim.ClaimKey, Nothing, Nothing, sDocumentRef)
                                        Else
                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey,
                                                                                            oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.PDF, oDocuments.Location & "\" & sFileName, oClaim.ClaimKey)
                                        End If
                                    Else
                                        If Session(CNProduceDocument) IsNot Nothing Then

                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey,
                                                oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.PDF, oDocuments.Location & "\" & sFileName, Nothing, Nothing, Nothing, sDocumentRef, sDocumentName:=sDocumentName)
                                        Else
                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey,
                                               oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.PDF, oDocuments.Location & "\" & sFileName, sDocumentName:=sDocumentName)
                                        End If
                                    End If

                                    CreateTagFile(oDocuments.Location, sTagFileName, sTagFileFormat)

                                    Response.ClearHeaders()
                                    Response.ContentType = "Application/pdf"
                                    Response.AddHeader("Content-Disposition", "inline; filename=document.pdf")
                                    Response.WriteFile(oDocuments.Location & "\" & sFileName)
                                End If

                            Case "DOCX"

                                sFileName = sFileName & ".docx"

                                If Request.QueryString("PreGenerate").ToString.ToLower = "true" And Not IO.File.Exists(oDocuments.Location & "\" & sFileName) Then
                                    Dim oFile As String

                                    If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                                        If Session(CNProduceDocument) IsNot Nothing Then

                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey,
                                                                                 oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.DOCX, oDocuments.Location & "\" & sFileName, oClaim.ClaimKey, Nothing, Nothing, sDocumentRef)
                                        Else
                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey,
                                                                                            oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.DOCX, oDocuments.Location & "\" & sFileName, oClaim.ClaimKey)
                                        End If
                                    Else
                                        If Session(CNProduceDocument) IsNot Nothing Then

                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey,
                                                oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.DOCX, oDocuments.Location & "\" & sFileName, Nothing, Nothing, Nothing, sDocumentRef, sDocumentName:=sDocumentName)
                                        Else
                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey,
                                               oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.DOCX, oDocuments.Location & "\" & sFileName, sDocumentName:=sDocumentName)
                                        End If
                                    End If

                                    CreateTagFile(oDocuments.Location, sTagFileName, sTagFileFormat)

                                    Response.ClearHeaders()
                                    Response.ContentType = "Application/msword"
                                    Response.AddHeader("Content-Disposition", "inline; filename=document.DOCX")
                                    Response.WriteFile(oDocuments.Location & "\" & sFileName)
                                ElseIf Request.QueryString("PreGenerate").ToString.ToLower = "true" And IO.File.Exists(oDocuments.Location & "\" & sFileName) Then
                                    Response.ClearHeaders()
                                    Response.ContentType = "Application/msword"
                                    Response.AddHeader("Content-Disposition", "inline; filename=document.DOCX")
                                    Response.WriteFile(oDocuments.Location & "\" & sFileName)
                                ElseIf Request.QueryString("PreGenerate").ToString.ToLower = "false" Then
                                    Dim oFile As String

                                    If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                                        If Session(CNProduceDocument) IsNot Nothing Then

                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey,
                                                                                 oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.DOCX, oDocuments.Location & "\" & sFileName, oClaim.ClaimKey, Nothing, Nothing, sDocumentRef)
                                        Else
                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey,
                                                                                            oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.DOCX, oDocuments.Location & "\" & sFileName, oClaim.ClaimKey)
                                        End If
                                    Else
                                        If Session(CNProduceDocument) IsNot Nothing Then

                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey,
                                                oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.DOCX, oDocuments.Location & "\" & sFileName, Nothing, Nothing, Nothing, sDocumentRef, sDocumentName:=sDocumentName)
                                        Else
                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey,
                                               oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.DOCX, oDocuments.Location & "\" & sFileName, sDocumentName:=sDocumentName)
                                        End If
                                    End If

                                    CreateTagFile(oDocuments.Location, sTagFileName, sTagFileFormat)

                                    Response.ClearHeaders()
                                    Response.ContentType = "Application/msword"
                                    Response.AddHeader("Content-Disposition", "inline; filename=document.docx")
                                    Response.WriteFile(oDocuments.Location & "\" & sFileName)

                                End If

                        End Select
                        Response.Flush()
                        Response.End()
                    Finally
                        oWebService = Nothing
                    End Try
                End If
            ElseIf Not iDoc_Number = 0 Then
                'Get the document from archive via the doc number
                Dim oDocuments As Config.Documents = Nothing
                Dim sDocumentDirName As String = Nothing
                Dim sExtensionType As String = Nothing

                Dim odocumentstr As New NexusProvider.Document
                Dim sContentType As String = Nothing
                Dim sDocumentType As String = Nothing


                Dim oFileTypes As Config.FileTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork) _
                .Portals.Portal(Portal.GetPortalID()).FileTypes

                If oQuote IsNot Nothing Then
                    oDocuments = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork) _
                         .Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode).Documents
                    sDocumentDirName = oDocuments.Location
                End If

                odocumentstr.DocNum = iDoc_Number
                'Call GetDocument SAM Method , will returned File Extension
                oWebService.GetDocument(odocumentstr)
                If Not (String.IsNullOrEmpty(Request.QueryString("doctype"))) Then
                    If UCase(Request.QueryString("doctype")) = "EMAIL" Then
                        sExtensionType = odocumentstr.FileExtension
                    Else
                        sExtensionType = oFileTypes.FileType(UCase(Request.QueryString("doctype"))).Extension
                    End If
                End If

                odocumentstr.FileExtension = sExtensionType
                'clear any headers so that we control what goes out
                Response.ClearHeaders()

                'Get the doctype from querystring 
                If Not (String.IsNullOrEmpty(Request.QueryString("doctype"))) Then
                    'used to specify the content type based on selected doctype of web.config
                    sContentType = oFileTypes.FileType(UCase(Request.QueryString("doctype"))).ContentType()
                    If String.IsNullOrEmpty(sContentType) Then
                        sDocumentType = oFileTypes.FileType(UCase(Request.QueryString("doctype"))).DocType()
                        Response.ContentType = sDocumentType
                    Else
                        Response.ContentType = sContentType
                    End If
                Else
                    'set content type so that browser knows what to do with the binary data
                    Response.ContentType = "Application/pdf"
                End If

                'tell browser to download the document
                If Not (String.IsNullOrEmpty(Request.QueryString("filename"))) Then
                    Dim safeFilename As String = Request.QueryString("filename")
                    If String.IsNullOrEmpty(odocumentstr.FileExtension) Then
                        Response.AddHeader("Content-Disposition", "attachment; filename=""" & safeFilename & """")
                    Else
                        If odocumentstr.FileExtension.StartsWith(".") Then
                            Response.AddHeader("Content-Disposition", "attachment; filename=""" & safeFilename & odocumentstr.FileExtension & """")
                        Else
                            Response.AddHeader("Content-Disposition", "attachment; filename=""" & safeFilename & "." & odocumentstr.FileExtension & """")
                        End If
                    End If
                Else
                    'tell browser to open the document inline, i.e. in the browser window
                    Response.AddHeader("Content-Disposition", "inline; filename=document.pdf")
                End If
                'write the file out
                Response.BinaryWrite(odocumentstr.PdfDocument)
                'flush and end the response
                Response.Flush()
                Response.End()
            ElseIf Not String.IsNullOrEmpty(Request.QueryString("docOpenClaim")) Then
                ' this section will be called from the Claims
                oQuote = Session(CNClaimQuote)
                Dim oDocuments As Config.Documents = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork) _
                           .Portals.Portal(Portal.GetPortalID()).Products.Product(Session(CNDataModelCode).ToString()) _
                           .Documents
                Dim sDocumentCode As String = oDocuments.DocTemplate(Request.QueryString("docOpenClaim")).Code()
                Dim oClaimDocument As New NexusProvider.ClaimDocument
                oClaimDocument.ClaimKey = Session(CNClaimKey)
                oClaimDocument.Mode = "1"
                oClaimDocument.TransactionType = sDocumentCode

                Select Case CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).DocumentFormat.ToUpper()
                    Case "HTML"
                        oWebService.GenerateClaimsDocuments(oClaimDocument, NexusProvider.DocumentType.HTML, oQuote.BranchCode)
                    Case "DOCX"
                        oWebService.GenerateClaimsDocuments(oClaimDocument, NexusProvider.DocumentType.DOCX, oQuote.BranchCode)

                    Case "PDF"
                        oWebService.GenerateClaimsDocuments(oClaimDocument, NexusProvider.DocumentType.PDF, oQuote.BranchCode)
                End Select
            ElseIf Not String.IsNullOrEmpty(Request.QueryString("docClaimPaymentCode")) Then

                oQuote = Session(CNClaimQuote)
                Dim oFile As String
                Dim sDocumentCode As String = Request.QueryString("docClaimPaymentCode").ToString.Trim
                Dim sFileName As String
                Dim oDocuments As Config.Documents = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork) _
                    .Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode).Documents
                Dim sDocumentDirName As String = oDocuments.Location

                sDocumentDirName = Right(sDocumentDirName, (sDocumentDirName.Length - sDocumentDirName.LastIndexOf("\")))

                Select Case CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).DocumentFormat.ToUpper()
                    Case "HTML"
                        'sFileName = oQuote.InsuranceFileKey & "_" & Session(CNClaimPaymentKey) & "_" & sDocumentCode & ".htm"
                        ' ''generate the document
                        'oFile = oWebService.GenerateDocument(oQuote.PartyKey, oQuote.InsuranceFileKey, oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.HTML, oDocuments.Location & "\" & sFileName, Session(CNClaimKey), Nothing, Nothing)
                        'Response.Redirect("~/" & sDocumentDirName & "\" & sFileName)

                        'As SAM not support DOC type but we need to generate doc type through HTML tag
                        sFileName = oQuote.InsuranceFileKey & "_" & Session(CNClaimPaymentKey) & "_" & sDocumentCode & ".doc"
                        'generate the document
                        oFile = oWebService.GenerateDocument(oQuote.PartyKey, oQuote.InsuranceFileKey, oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.HTML, oDocuments.Location & "\" & sFileName, Session(CNClaimKey), Nothing, Nothing)

                        Response.ClearHeaders()

                        Response.ContentType = "Application/msword"
                        Response.AddHeader("Content-Disposition", "inline; filename=document.doc")

                        Response.WriteFile(oDocuments.Location & "\" & sFileName)
                        'flush and end the response
                        Response.Flush()
                        Response.End()


                    Case "PDF"
                        sFileName = oQuote.InsuranceFileKey & "_" & Session(CNClaimPaymentKey) & "_" & sDocumentCode & ".pdf"
                        'generate the document
                        oFile = oWebService.GenerateDocument(oQuote.PartyKey, oQuote.InsuranceFileKey, oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.PDF, oDocuments.Location & "\" & sFileName, Session(CNClaimKey), Nothing, Nothing)

                        Response.ClearHeaders()

                        Response.ContentType = "Application/pdf"
                        Response.AddHeader("Content-Disposition", "inline; filename=document.pdf")
                        'Response.Write("<script>")
                        'Response.Write("window.open('page.html','_blank')")
                        'Response.Write("</script>")
                        Response.WriteFile(oDocuments.Location & "\" & sFileName)
                    Case "DOCX"
                        sFileName = oQuote.InsuranceFileKey & "_" & Session(CNClaimPaymentKey) & "_" & sDocumentCode & ".pdf"
                        'generate the document
                        oFile = oWebService.GenerateDocument(oQuote.PartyKey, oQuote.InsuranceFileKey, oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.PDF, oDocuments.Location & "\" & sFileName, Session(CNClaimKey), Nothing, Nothing)

                        Response.ClearHeaders()

                        Response.ContentType = "Application/msword"
                        Response.AddHeader("Content-Disposition", "inline; filename=document.docx")
                        'Response.Write("<script>")
                        'Response.Write("window.open('page.html','_blank')")
                        'Response.Write("</script>")
                        Response.WriteFile(oDocuments.Location & "\" & sFileName)
                End Select
            End If
        End Sub
        Sub CreateTagFile(ByVal sDocumentDirName As String, ByVal sTagFileName As String, ByVal sTagFileFormat As String)
            If sTagFileName IsNot Nothing And sTagFileFormat IsNot Nothing Then
                Dim v_sDocumentExtractionDirectory As String = sTagFileName
                Dim sDirectoryName As String
                If sTagFileName.Contains("\") Then
                    sDirectoryName = Left(v_sDocumentExtractionDirectory, v_sDocumentExtractionDirectory.LastIndexOf("\"))
                Else
                    sDirectoryName = sTagFileName
                End If

                If Not IO.Directory.Exists(sDocumentDirName & "\" & sDirectoryName) Then
                    IO.Directory.CreateDirectory(sDocumentDirName & "\" & sDirectoryName)
                End If

                Dim StreamWriter1 As System.IO.StreamWriter
                StreamWriter1 = New System.IO.StreamWriter(sDocumentDirName & "\" & sTagFileName, True)
                StreamWriter1.WriteLine(sTagFileFormat)
                StreamWriter1.Close()
            End If

        End Sub
    End Class

End Namespace
