Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports CMS.Library
Imports Nexus
Imports Nexus.Constants
Imports Nexus.Constants.Session
Partial Class Controls_Document
    Inherits System.Web.UI.UserControl
    Private sDocumentName As String
    Private sText As String
    Private bPreGenerate As Boolean
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Nothing
            Dim oClaim As NexusProvider.ClaimOpen
            Dim sPolicyRef As String = Nothing
            Dim sDocumentRef As String = Nothing
            Dim sClientName As String = String.Empty
            '''''
            If DocumentName.Trim.ToUpper.Contains("RECEIPT") Then

                If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                    oQuote = Session(CNClaimQuote)
                    oClaim = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                    sPolicyRef = oQuote.InsuranceFileRef
                ElseIf Session.Item(CNPolicy_Summary) IsNot Nothing Then
                    Dim oPolicySummary As NexusProvider.PolicySummary
                    oQuote = Session(CNQuote)
                    oPolicySummary = Session.Item(CNPolicy_Summary)
                    sPolicyRef = oPolicySummary.Reference
                End If

                If Session(CNProduceDocument) IsNot Nothing Then
                    Dim oCashListReceipts As NexusProvider.CashListReceipts
                    Dim oCashListReceipt As New NexusProvider.CashListReceipt
                    Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                    Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
                    If Session(CNQuoteCollectionFiles) IsNot Nothing AndAlso Session(CNPolicySummaryCollection) IsNot Nothing Then
                        Dim oPolicySummaryCollection As NexusProvider.PolicySummaryCollection
                        Dim arrQuoteCollectionFiles As ArrayList
                        oQuote = Session(CNQuote)
                        arrQuoteCollectionFiles = Session(CNQuoteCollectionFiles)
                        oPolicySummaryCollection = Session(CNPolicySummaryCollection)

                        If Session(CNPolicyNumber) IsNot Nothing Then
                            'oCashListReceipt.InsuranceFileKey = arrQuoteCollectionFiles(0)
                            oCashListReceipt.InsuranceRef = Session(CNPolicyNumber).trim()

                            'to limit the search return from SAM
                            oCashListReceipt.MaxRowsToFetch = oPortal.MaxSearchResults

                            oCashListReceipts = oWebService.FindCashListReceipts(oCashListReceipt)
                            Me.Visible = True

                            If oCashListReceipts IsNot Nothing Then
                                HyperLink1.Visible = False

                                For iCount As Integer = 0 To oCashListReceipts.Count - 1
                                    Dim oHyperlink As New HyperLink
                                    oHyperlink.Target = "_blank"
                                    oHyperlink.Text = oCashListReceipts(iCount).DocumentRef

                                    sDocumentRef += oCashListReceipts(iCount).DocumentRef.Trim + ","

                                    If PreGenerate = True Then
                                        oHyperlink.NavigateUrl = "~/secure/document.aspx?doc=" & DocumentName & "&DocRef=" & oCashListReceipts(iCount).DocumentRef.Trim & "&PreGenerate=True"
                                    Else
                                        oHyperlink.NavigateUrl = "~/secure/document.aspx?doc=" & DocumentName & "&DocRef=" & oCashListReceipts(iCount).DocumentRef.Trim & "&PreGenerate=False"
                                    End If

                                    Me.Controls.Add(oHyperlink)
                                Next
                            End If
                        End If
                    Else
                        oCashListReceipt.InsuranceFileKey = oQuote.InsuranceFileKey
                        oCashListReceipt.InsuranceRef = sPolicyRef

                        'to limit the search return from SAM
                        oCashListReceipt.MaxRowsToFetch = oPortal.MaxSearchResults

                        oCashListReceipts = oWebService.FindCashListReceipts(oCashListReceipt)
                        Me.Visible = True

                        If oCashListReceipts IsNot Nothing Then
                            HyperLink1.Visible = False

                            For iCount As Integer = 0 To oCashListReceipts.Count - 1
                                If oCashListReceipts(iCount).InsuranceFileKey = oQuote.InsuranceFileKey Then
                                    Dim oHyperlink As New HyperLink
                                    oHyperlink.Target = "_blank"
                                    oHyperlink.Text = oCashListReceipts(iCount).DocumentRef

                                    sDocumentRef += oCashListReceipts(iCount).DocumentRef.Trim + ","

                                    If PreGenerate = True Then
                                        oHyperlink.NavigateUrl = "~/secure/document.aspx?doc=" & DocumentName & "&DocRef=" & oCashListReceipts(iCount).DocumentRef.Trim & "&PreGenerate=True"
                                    Else
                                        oHyperlink.NavigateUrl = "~/secure/document.aspx?doc=" & DocumentName & "&DocRef=" & oCashListReceipts(iCount).DocumentRef.Trim & "&PreGenerate=False"
                                    End If

                                    Me.Controls.Add(oHyperlink)
                                End If
                            Next
                        End If
                    End If
                End If
            End If
            '''''

            If Me.Visible = True And PreGenerate = True Then
                Dim iPartyKey As Integer
                Dim sPolicyReference As String = Nothing
                Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                Dim oFile As String
                If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                    oQuote = Session(CNClaimQuote)
                    oClaim = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                Else
                    oQuote = Session(CNQuote)
                    If Session.Item(CNPolicy_Summary) IsNot Nothing Then
                        Dim oPolicySummary As NexusProvider.PolicySummary = Session.Item(CNPolicy_Summary)
                        sPolicyReference = oPolicySummary.Reference
                    Else
                        sPolicyReference = oQuote.InsuranceFileRef
                    End If
                End If

                Dim oDocuments As Config.Documents = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork) _
                    .Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode).Documents
                Dim sDocumentDirName As String = oDocuments.Location
                sDocumentDirName = Right(sDocumentDirName, (sDocumentDirName.Length - sDocumentDirName.LastIndexOf("\")))
                If oDocuments.DocTemplate(DocumentName) Is Nothing Then
                    Exit Sub
                End If
                Dim sDocumentCode As String = oDocuments.DocTemplate(DocumentName).Code()
                Dim sFileName As String = oDocuments.DocTemplate(DocumentName).FileName
                Dim sTagFileName As String = Nothing
                Dim sTagFileFormat As String = Nothing
                Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                Dim sGUID As String = Guid.NewGuid.ToString
                Dim sPolicyNo As String = oQuote.InsuranceFileRef
                Dim sBranchName As String = String.Empty

                'IF TagFileName is provided
                If String.IsNullOrEmpty(oDocuments.DocTemplate(DocumentName).TagFileName) = False Then
                    sTagFileName = oDocuments.DocTemplate(DocumentName).TagFileName
                End If

                'IF TagFileFormat is provided
                If String.IsNullOrEmpty(oDocuments.DocTemplate(DocumentName).TagFileFormat) = False Then
                    sTagFileFormat = oDocuments.DocTemplate(DocumentName).TagFileFormat
                End If

                'if Oparty is nothing
                If oParty Is Nothing AndAlso oQuote.PartyKey > 0 Then
                    Dim sBranchCode As String = Nothing
                    If String.IsNullOrEmpty(oQuote.BranchCode) = False Then
                        sBranchCode = oQuote.BranchCode
                    End If
                    oParty = oWebService.GetParty(oQuote.PartyKey, sBranchCode)
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
                    If sTagFileFormat IsNot Nothing Then
                        sTagFileFormat = sTagFileFormat.Replace("[!UserID!]", oUserDetails.ResolvedName)
                    End If
                    If sTagFileName IsNot Nothing Then
                        sTagFileName = sTagFileName.Replace("[!UserID!]", oUserDetails.ResolvedName)
                    End If

                    sFileName = sFileName.Replace("[!UserID!]", oUserDetails.ResolvedName)

                ElseIf oParty IsNot Nothing Then
                    sFileName = sFileName.Replace("[!UserID!]", oParty.TPIntroducer)

                    If sTagFileFormat IsNot Nothing Then
                        sTagFileFormat = sTagFileFormat.Replace("[!UserID!]", oParty.TPIntroducer)
                    End If

                    If sTagFileName IsNot Nothing Then
                        sTagFileName = sTagFileName.Replace("[!UserID!]", oParty.TPIntroducer)
                    End If

                End If

                If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                    iPartyKey = oClaim.Client.PartyKey
                    sFileName = sFileName.Replace("[!ClientCode!]", oClaim.Client.ShortName)
                    sFileName = sFileName.Replace("[!ClientName!]", oClaim.Client.ClientName)

                    If sTagFileName IsNot Nothing Then
                        sTagFileName = sTagFileName.Replace("[!ClientCode!]", oClaim.Client.ShortName)
                        sTagFileName = sTagFileName.Replace("[!ClientName!]", oClaim.Client.ClientName)
                    End If

                    If sTagFileFormat IsNot Nothing Then
                        sTagFileFormat = sTagFileFormat.Replace("[!ClientCode!]", oClaim.Client.ShortName)
                        sTagFileFormat = sTagFileFormat.Replace("[!ClientName!]", oClaim.Client.ClientName)
                    End If

                ElseIf oParty IsNot Nothing Then
                    iPartyKey = oParty.Key
                    Select Case True
                        Case TypeOf oParty Is NexusProvider.CorporateParty
                            With CType(oParty, NexusProvider.CorporateParty)
                                sClientName = DirectCast(oParty, NexusProvider.CorporateParty).ClientSharedData.ResolvedName
                            End With
                        Case TypeOf oParty Is NexusProvider.PersonalParty
                            With CType(oParty, NexusProvider.PersonalParty)
                                sClientName = DirectCast(oParty, NexusProvider.PersonalParty).ClientSharedData.ResolvedName
                            End With
                    End Select
                    sFileName = sFileName.Replace("[!ClientCode!]", If(oParty.UserName IsNot Nothing, oParty.UserName.Trim, String.Empty))
                    sFileName = sFileName.Replace("[!ClientName!]", If(oParty.ResolvedName IsNot Nothing, oParty.ResolvedName.Trim, String.Empty))


                If sTagFileName IsNot Nothing Then
                    sTagFileName = sTagFileName.Replace("[!ClientCode!]", If(oParty.UserName IsNot Nothing, oParty.UserName.Trim, String.Empty))
                    sTagFileName = sTagFileName.Replace("[!ClientName!]", If(oParty.ResolvedName IsNot Nothing, oParty.ResolvedName.Trim, String.Empty))
                End If

                If sTagFileFormat IsNot Nothing Then
                    sTagFileFormat = sTagFileFormat.Replace("[!ClientCode!]", If(oParty.UserName IsNot Nothing, oParty.UserName.Trim, String.Empty))
                    sTagFileFormat = sTagFileFormat.Replace("[!ClientName!]", If(oParty.ResolvedName IsNot Nothing, oParty.ResolvedName.Trim, String.Empty))
                End If

                Else
                    iPartyKey = oQuote.PartyKey
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

                'Tag File Format
                If sTagFileFormat IsNot Nothing Then
                    sTagFileFormat = sTagFileFormat.Replace("[!ProductName!]", oQuote.ProductCode)
                    sTagFileFormat = sTagFileFormat.Replace("[!PolicyNumber!]", oQuote.InsuranceFileRef)
                    sTagFileFormat = sTagFileFormat.Replace("[!ClaimNumber!]", oQuote.InsuranceFileRef)
                    sTagFileFormat = sTagFileFormat.Replace("[!BranchCode!]", oQuote.BranchCode)
                    sTagFileFormat = sTagFileFormat.Replace("[!BranchName!]", oQuote.BranchCode)
                    sTagFileFormat = sTagFileFormat.Replace("[!TransactionType!]", oQuote.TransactionType)
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
                                    Using strDataset As New System.IO.StringReader(oQuote.Risks(iCount).XMLDataset)
                                        Dim DataModelCode As String = Session(CNDataModelCode)
                                        Dim NavString As String = "DATA_SET/RISK_OBJECTS/" & DataModelCode & "_POLICY_BINDER/" & sTemp(iCounter)
                                        Dim Navigator As System.Xml.XPath.XPathNavigator
                                        Using trDataset As New System.Xml.XmlTextReader(strDataset)
                                            Dim Doc As System.Xml.XPath.XPathDocument = New System.Xml.XPath.XPathDocument(trDataset)
                                            Navigator = Doc.CreateNavigator()
                                            Dim NodeI As System.Xml.XPath.XPathNodeIterator = Navigator.Select(NavString)
                                            While NodeI.MoveNext
                                                sTagFileFormat = sTagFileFormat & NodeI.Current.Value
                                                sTagFileFormat = sTagFileFormat & ";"
                                            End While
                                        End Using
                                    End Using
                                End If
                            Next
                        End If
                    Next
                End If

                Select Case CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).DocumentFormat.ToUpper()
                    Case "HTML"
                        sFileName = sFileName & ".htm"
                        If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                            If Session(CNProduceDocument) IsNot Nothing Then

                                If sDocumentRef IsNot Nothing Then
                                    Dim sDocumentName() As String = sDocumentRef.Split(",")
                                    For iCount As Integer = 0 To sDocumentName.Length - 1
                                        If sDocumentName(iCount).Trim.Length <> 0 Then
                                            Dim sReceiptFileName As String = sFileName
                                            Dim sReceiptTagFileName As String = sTagFileName
                                            sReceiptFileName = sReceiptFileName.Replace("[!DocumentRef!]", sDocumentName(iCount))
                                            sReceiptTagFileName = sReceiptFileName.Replace("[!DocumentRef!]", sDocumentName(iCount))

                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey, _
                                                                                                                oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.HTML, Server.MapPath("~/" & sDocumentDirName) & "\" & sReceiptFileName, oClaim.ClaimKey, Nothing, Nothing, sDocumentName(iCount))

                                            CreateTagFile(oDocuments.Location, sReceiptTagFileName, sTagFileFormat)
                                        End If

                                    Next
                                End If
                            Else
                                oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey, _
                                                                                oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.HTML, Server.MapPath("~/" & sDocumentDirName) & "\" & sFileName, oClaim.ClaimKey)

                                CreateTagFile(oDocuments.Location, sTagFileName, sTagFileFormat)
                            End If
                        Else
                            If Session(CNProduceDocument) IsNot Nothing Then

                                If sDocumentRef IsNot Nothing Then
                                    Dim sDocumentName() As String = sDocumentRef.Split(",")

                                    For iCount As Integer = 0 To sDocumentName.Length - 1
                                        If sDocumentName(iCount).Trim.Length <> 0 Then
                                            Dim sReceiptFileName As String = sFileName
                                            Dim sReceiptTagFileName As String = sTagFileName
                                            sReceiptFileName = sReceiptFileName.Replace("[!DocumentRef!]", sDocumentName(iCount))
                                            sReceiptTagFileName = sReceiptFileName.Replace("[!DocumentRef!]", sDocumentName(iCount))

                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey, _
                                                                                                                oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.HTML, Server.MapPath("~/" & sDocumentDirName) & "\" & sReceiptFileName, Nothing, Nothing, Nothing, sDocumentName(iCount))

                                            CreateTagFile(oDocuments.Location, sReceiptTagFileName, sTagFileFormat)
                                        End If

                                    Next
                                End If
                            Else
                                oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey, _
                                   oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.HTML, Server.MapPath("~/" & sDocumentDirName) & "\" & sFileName)

                                CreateTagFile(oDocuments.Location, sTagFileName, sTagFileFormat)
                            End If
                        End If


                    Case "PDF"
                        sFileName = sFileName & ".pdf"

                        If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                            If Session(CNProduceDocument) IsNot Nothing Then
                                If sDocumentRef IsNot Nothing Then
                                    Dim sDocumentName() As String = sDocumentRef.Split(",")

                                    For iCount As Integer = 0 To sDocumentName.Length - 1

                                        If sDocumentName(iCount).Trim.Length <> 0 Then
                                            Dim sReceiptFileName As String = sFileName
                                            Dim sReceiptTagFileName As String = sTagFileName
                                            sReceiptFileName = sReceiptFileName.Replace("[!DocumentRef!]", sDocumentName(iCount))
                                            sReceiptTagFileName = sReceiptFileName.Replace("[!DocumentRef!]", sDocumentName(iCount))

                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey, _
                                                                                                                oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.PDF, oDocuments.Location & "\" & sReceiptFileName, oClaim.ClaimKey, Nothing, Nothing, sDocumentName(iCount))
                                            CreateTagFile(oDocuments.Location, sReceiptTagFileName, sTagFileFormat)
                                        End If

                                    Next
                                End If

                            Else
                                oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey, _
                                                                                oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.PDF, oDocuments.Location & "\" & sFileName, oClaim.ClaimKey)

                                CreateTagFile(oDocuments.Location, sTagFileName, sTagFileFormat)
                            End If
                        Else
                            If Session(CNProduceDocument) IsNot Nothing Then

                                If sDocumentRef IsNot Nothing Then
                                    Dim sDocumentName() As String = sDocumentRef.Split(",")

                                    For iCount As Integer = 0 To sDocumentName.Length - 1
                                        If sDocumentName(iCount).Trim.Length <> 0 Then
                                            Dim sReceiptFileName As String = sFileName
                                            Dim sReceiptTagFileName As String = sTagFileName
                                            sReceiptFileName = sReceiptFileName.Replace("[!DocumentRef!]", sDocumentName(iCount))
                                            sReceiptTagFileName = sReceiptFileName.Replace("[!DocumentRef!]", sDocumentName(iCount))

                                            oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey, _
                                                                                                                oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.PDF, oDocuments.Location & "\" & sReceiptFileName, Nothing, Nothing, Nothing, sDocumentName(iCount))

                                            CreateTagFile(oDocuments.Location, sReceiptTagFileName, sTagFileFormat)
                                        End If

                                    Next
                                End If
                            Else
                                oFile = oWebService.GenerateDocument(iPartyKey, oQuote.InsuranceFileKey, _
                                   oQuote.InsuranceFolderKey, sDocumentCode, NexusProvider.DocumentType.PDF, oDocuments.Location & "\" & sFileName)

                                CreateTagFile(oDocuments.Location, sTagFileName, sTagFileFormat)
                            End If
                        End If

                End Select
            End If

        End If

        If Me.Visible = True And PreGenerate = True Then
            HyperLink1.NavigateUrl = "~/secure/document.aspx?doc=" & DocumentName & "&PreGenerate=True"
        Else
            HyperLink1.NavigateUrl = "~/secure/document.aspx?doc=" & DocumentName & "&PreGenerate=False"
        End If
    End Sub
    Sub CreateTagFile(ByVal sDocumentDirName As String, ByVal sTagFileName As String, ByVal sTagFileFormat As String)
        'IF TagFileName is provided
        If sTagFileName IsNot Nothing And sTagFileFormat IsNot Nothing Then
            Dim v_sDocumentExtractionDirectory As String = sTagFileName
            If v_sDocumentExtractionDirectory.Contains("\") Then
                Dim sDirectoryName As String = Left(v_sDocumentExtractionDirectory, v_sDocumentExtractionDirectory.LastIndexOf("\"))

                If Not IO.Directory.Exists(sDocumentDirName & "\" & sDirectoryName) Then
                    IO.Directory.CreateDirectory(sDocumentDirName & "\" & sDirectoryName)
                End If
            End If
            Dim StreamWriter1 As System.IO.StreamWriter
                StreamWriter1 = New System.IO.StreamWriter(sDocumentDirName & "\" & sTagFileName, True)
                StreamWriter1.WriteLine(sTagFileFormat)
                StreamWriter1.Close()
            End If
    End Sub
    Public Property DocumentName() As String
        Get
            Return sDocumentName
        End Get
        Set(ByVal value As String)
            sDocumentName = value
        End Set
    End Property
    Public Property PreGenerate() As Boolean
        Get
            Return bPreGenerate
        End Get
        Set(ByVal value As Boolean)
            bPreGenerate = value
        End Set
    End Property
    Public Property Text() As String
        Get
            Return HyperLink1.Text
        End Get
        Set(ByVal value As String)
            HyperLink1.Text = value
        End Set
    End Property
End Class
