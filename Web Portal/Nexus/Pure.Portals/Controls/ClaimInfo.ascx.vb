Imports System.Resources
Imports System.Xml
Imports System.Xml.XPath
Imports System.Xml.XmlReader
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Utils

Namespace Nexus

    Partial Class Controls_ClaimInfo : Inherits System.Web.UI.UserControl
        Dim m_sPolicyNumber As String
        Dim m_sCurrency As String
        Dim m_sDates As String
        Dim m_sInsuredName As String
        Dim m_sStatus As String
        Dim m_sClaimNumber As String
        Dim sReturnUrl As String = String.Empty
        Dim sFolder As String = String.Empty
        Dim sFirstPage As String = String.Empty
        Private oMaster As ContentPlaceHolder

        Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
            Dim bolIsReturn As Boolean = False
            Select Case CType(Session(CNMode), Mode)
                Case Mode.ViewClaim, Mode.EditClaim, Mode.NewClaim, Mode.PayClaim, Mode.SalvageClaim, Mode.TPRecovery, Mode.ViewClaimPayment, Mode.Authorise, Mode.DeclinePayment, Mode.Recommend

                    Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oDocumentArchiveOptionSettings As NexusProvider.OptionTypeSetting
                    oDocumentArchiveOptionSettings = oWebservice.GetOptionSetting(NexusProvider.OptionType.SystemOption, 10)

                    'Get values to use out of session

                    If Request.QueryString("ReturnUrl") IsNot Nothing Then
                        If Request.QueryString("ReturnUrl").ToUpper.Contains("~/CLAIMS/") Then
                            bolIsReturn = True
                        End If
                    End If

                    Dim AllowFolderName As String = "/CLAIMS/"
                    If Not Request.CurrentExecutionFilePath.ToUpper.Contains(AllowFolderName.ToUpper) And Not bolIsReturn Then

                        'claim control should be hidden
                        Me.Visible = False
                    Else
                        Dim PolicyNumber As String = Session(CNPolicyNumber)
                        Dim Currency As String = GetCurrencyForCode(Session(CNCurrenyCode))
                        Dim Dates As String = Session(CNDate_Header)
                        Dim InsuredName As String = Session(CNInsurer_Header)
                        Dim Status As String = Session(CNStatus)
                        Dim ClaimNumber As String = Session(CNClaimNumber)
                        Dim EventMode As Mode = CType(Session(CNMode), Mode)
                        Dim oParty As NexusProvider.BaseParty = Session.Item(CNParty)
                        If (oParty Is Nothing) Then
                            Dim oQuote As NexusProvider.Quote = Nothing
                            Dim oBaseParty As NexusProvider.BaseParty = Nothing
                            oQuote = oWebservice.GetHeaderAndSummariesByKey(Session(CNInsuranceFileKey))
                            If oQuote IsNot Nothing Then
                                oBaseParty = oWebservice.GetParty(oQuote.PartyKey)
                                Session.Item(CNParty) = oBaseParty
                                oParty = oBaseParty
                            End If
                            oQuote = Nothing
                        End If
                        lblDates.Text = Dates
                        lblStatus.Text = Status
                        Select Case EventMode
                            Case Mode.Add
                                lblInsuredNameTitle.Visible = False
                                hypInsured.Visible = False
                            Case Mode.Edit, Mode.View
                                lblInsuredNameTitle.Visible = True
                                hypInsured.Visible = True
                        End Select
                        hypInsured.Text = InsuredName
                        lblCurrency.Text = Currency

                        If Not String.IsNullOrEmpty(ClaimNumber) Then
                            'set the claim number
                            lblClaimNumber.Text = ClaimNumber.Trim.ToUpper
                        If (Not String.IsNullOrEmpty(ClaimNumber)) AndAlso ClaimNumber.Trim.ToUpper = "TBA" Then
                                'new claim so we cannot show the 
                                hypClaimDocs.Visible = False
                            End If
                        Else
                            hypClaimDocs.Visible = False
                        End If

                        Dim oClaim As NexusProvider.Claim = CType(Session(CNClaim), NexusProvider.Claim)
                        Dim oInsurerDetails As NexusProvider.ClaimOpen
                        If oClaim IsNot Nothing Then
                            oInsurerDetails = CType(oClaim, NexusProvider.ClaimOpen)
                        Else
                            oInsurerDetails = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                        End If
                        'Dim oInsurerDetails As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                        If oInsurerDetails IsNot Nothing Then
                            'set up documents hyperlink to launch modal, passing the file key in the query 
                            If oDocumentArchiveOptionSettings.OptionValue = "1" Then
                                If Session("Clientname") IsNot Nothing And oInsurerDetails.ClientShortName Is Nothing Then
                                    oInsurerDetails.ClientShortName = Session("ClientName").ToString()
                                End If
                                If (oInsurerDetails.ClientShortName IsNot Nothing) Then
                                    If (oInsurerDetails.ClientShortName.ToString() <> "") Then
                                        Dim oDMESearchCriteria As New NexusProvider.DMESearchCriteria
                                        Dim oDME As NexusProvider.DME
                                        Dim Server_Name As String
                                        Dim HTTPS_Protocol As String
                                        Dim Requset_Protocol As String = "http://"
                                        Server_Name = Request.ServerVariables("SERVER_NAME").ToString()
                                        HTTPS_Protocol = Request.ServerVariables("HTTPS").ToString()
                                        If HTTPS_Protocol.ToLower() = "on" Then
                                            Requset_Protocol = "https://"
                                        End If
                                        'Initializing the values
                                        Session("Clientname") = oInsurerDetails.ClientShortName.ToString()
                                        oDMESearchCriteria.PartyCode = Trim(oInsurerDetails.ClientShortName.ToString())
                                        oDMESearchCriteria.IncludeFiles = False
                                        ''Sam Call with Search criteria
                                        Try
                                            oDME = oWebservice.FindDMEDocuments(oDMESearchCriteria)
                                            If oDME IsNot Nothing And oDME.SubFolder IsNot Nothing And oDME.SubFolder.Count > 0 Then
                                                hypClaimDocs.NavigateUrl = "~/Modal/DMEDocumentManager.aspx" & "?modal=true&KeepThis=true&fromlink=claim&TB_iframe=true&height=550&width=750&BranchCode=" & CType(Session(CNClaimQuote), NexusProvider.Quote).BranchCode & "&FolderName=" & oDME.SubFolder(0).Name & "&FolderNum=" & oDME.SubFolder(0).FolderNum & "',null)"
                                                Session("claimno") = lblClaimNumber.Text

                                            End If
                                        Catch ex As System.Exception

                                        End Try


                                    End If
                                End If
                            Else
                                hypClaimDocs.NavigateUrl = "~/Modal/DocumentManager.aspx" & "?ClaimKey=" & oInsurerDetails.ClaimKey & "&PartyKey=" & oParty.Key & "&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=550&width=750', null);return false;"
                            End If
                            'hypClaimDocs.NavigateUrl = "~/Modal/DocumentManager.aspx" & "?ClaimKey=" & oInsurerDetails.ClaimKey & "&PartyCode=" & Trim(oInsurerDetails.ClientShortName) & "&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=550&width=750', null);return false;"

                            If oInsurerDetails.Insurer IsNot Nothing Then
                                hypAgentDetails.Text = oInsurerDetails.Insurer.InsurerName
                            End If
                        End If

                        Dim WebRoot As String = AppSettings("WebRoot")

                        'Creating return URL
                        Dim ReturnURL As String = Session(CNReturnURL)
                        If ReturnURL Is Nothing Then
                            ReturnURL = ""
                        End If
                        If Session(CNReturnURL) IsNot Nothing Then
                            If ReturnURL.Contains(Request.Path.Replace(WebRoot, "")) = False Then
                                If ReturnURL = "" Then
                                    sReturnUrl = Request.Path.Replace(WebRoot, "~/")
                                Else
                                    sReturnUrl = ReturnURL & ";" & Request.Path.Replace(WebRoot, "~/")
                                End If
                            Else
                                Dim sOldReturn As String = Request.Path.Replace(WebRoot, "~/") & ";"
                                ReturnURL = ReturnURL.Replace(sOldReturn, "")
                                sOldReturn = ";" & Request.Path.Replace(WebRoot, "~/")
                                ReturnURL = ReturnURL.Replace(sOldReturn, "")
                                sOldReturn = Request.Path.Replace(WebRoot, "~/")
                                ReturnURL = ReturnURL.Replace(sOldReturn, "")
                                If ReturnURL = "" Then
                                    sReturnUrl = Request.Path.Replace(WebRoot, "~/")
                                Else
                                    sReturnUrl = ReturnURL & ";" & Request.Path.Replace(WebRoot, "~/")
                                End If
                            End If
                        Else
                            sReturnUrl = Request.Path.Replace(WebRoot, "~/")
                        End If


                        'sReturnUrl = Request.Path.Replace(WebRoot, "~/")

                        Session(CNReturnURL) = sReturnUrl & "?" & Request.QueryString.ToString()

                        hypFinancialdetails.NavigateUrl = "~/Claims/FinancialDetails.aspx?modal=true&ReturnUrl=" + sReturnUrl
                        hypEvents.NavigateUrl = "~/secure/EventList.aspx?modal=true&ReturnUrl=" + sReturnUrl

                        If (Session(CNClaimNumber) IsNot Nothing AndAlso Session(CNClaimNumber).ToString.Trim.ToUpper <> "TBA") Or Session(CNMode) = Mode.EditClaim Then
                            Me.liFinancialDetails.Visible = True
                        End If

                        Dim oOpenClaim As NexusProvider.ClaimOpen
                        If oClaim IsNot Nothing Then
                            oOpenClaim = CType(oClaim, NexusProvider.ClaimOpen)
                        Else
                            oOpenClaim = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                        End If

                        Dim oNexusConfig As Config.NexusFrameWork
                        Dim oPortal As Nexus.Library.Config.Portal
                        Dim oClaimQuote As NexusProvider.Quote = Session(CNClaimQuote)

                        If oClaimQuote IsNot Nothing Then
                            hypPolicyNumber.Text = oClaimQuote.InsuranceFileRef

                            If oClaimQuote.BusinessTypeCode = "DIRECT" Then
                                liAgent.Visible = False
                            Else
                                liAgent.Visible = True
                            End If
                        End If

                        oNexusConfig = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                        oPortal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
                        If ((Session(CNClaimNumber) IsNot Nothing AndAlso Session(CNClaimNumber).Trim.ToUpper <> "TBA") Or Session(CNMode) = Mode.EditClaim) AndAlso oOpenClaim IsNot Nothing AndAlso String.IsNullOrEmpty(oOpenClaim.RiskType) = False Then
                            Dim sFirstPage As String = Nothing
                            liRisk.Visible = True
                            hypRisk.Text = oOpenClaim.RiskTypeDescription
                            If Session(CNClaimNumber) IsNot Nothing Then
                                If (Session(CNClaimNumber).ToString.Trim.ToUpper <> "TBA" Or Session(CNMode) = Mode.EditClaim) AndAlso CheckValidProduct(oClaimQuote.ProductCode) = True Then
                                    hypRisk.Visible = True
                                End If
                            End If
                        End If
                        If oPortal.EnableMasterClientAssociate = True Then
                            Dim oGetPolicyAssociateCollection As NexusProvider.PolicyAssociateCollection = New NexusProvider.PolicyAssociateCollection


                            oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)
                            Dim hLnk As HyperLink = oMaster.FindControl("hypPolicyAssociate")
                            oGetPolicyAssociateCollection = oWebservice.GetPolicyAssociates(oClaimQuote.InsuranceFileKey, oClaimQuote.InsuranceFolderKey, Nothing)
                            rptrPolicyAssociate.DataSource = oGetPolicyAssociateCollection
                            rptrPolicyAssociate.DataBind()
                        Else
                            liPolicyAssociate.Visible = False
                        End If
                    End If

                Case Else
                    'claim control should be hidden
                    Me.Visible = False
            End Select
        End Sub

        ''' <summary>
        ''' This function is to get Claim Details if not any
        ''' </summary>
        ''' <param name="sender"></param>        
        ''' <remarks></remarks>
        Private Sub GetCNClaimSession(ClaimNumber As String)

            Dim oClaims1 As NexusProvider.ClaimCollection
            oClaims1 = CType(Session(CNClaimsSearchData), NexusProvider.ClaimCollection)
            Dim iRowIndex As Integer = 0
            Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
            Dim sBranchCode As String = oUserDetails.ListOfBranches(0).Code
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            ClearClaims()
            'Find Latest Claim Version
            If Not String.IsNullOrEmpty(ClaimNumber) Then
                Dim oClaimVersions As NexusProvider.VersionsCollections = Nothing
                Dim oQuote As NexusProvider.Quote = Nothing
                Dim oBaseParty As NexusProvider.BaseParty = Nothing
                Dim sClaimNumber As String = CStr(ClaimNumber)
                Dim iHighest As Integer = 0
                Dim oOpenClaim As New NexusProvider.ClaimOpen
                Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
                Dim oCashListItem As NexusProvider.CashListItemsCollection = Nothing
                Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing

                Try
                    oClaimVersions = oWebservice.GetVersionsForClaim(sClaimNumber)
                    If oClaimVersions IsNot Nothing Then
                        'Find Highest Version

                        For iCount As Integer = 0 To oClaimVersions.Count - 1
                            If oClaimVersions(iCount).Version > iHighest Then
                                iHighest = oClaimVersions(iCount).Version
                            End If
                        Next

                        'Updating of claim quote oQuote
                        oQuote = oWebservice.GetHeaderAndSummariesByKey(oClaimVersions(0).InsuranceFileKey)
                        If oQuote IsNot Nothing Then
                            oBaseParty = oWebservice.GetParty(oQuote.PartyKey)
                            Session.Item(CNParty) = oBaseParty
                            Session.Item(CNRisks) = oQuote.Risks
                            Session.Item(CNRenewalDate) = oQuote.RenewalDate
                            Session.Item(CNAddress) = oBaseParty.Addresses(0).Address1 & ", " & oBaseParty.Addresses(0).Address4
                            Session.Item(CNDate_Header) = oQuote.CoverStartDate.ToShortDateString & " - " & oQuote.CoverEndDate.ToShortDateString
                            Session(CNInsurer_Header) = oQuote.InsuredName
                            Session(CNProductCode) = oQuote.ProductCode
                            Session("ProductCode") = oQuote.ProductCode
                            Session(CNClaimQuote) = oQuote
                        End If
                        Session(CNClaimVersion) = oClaimVersions
                        Session.Item(CNInsuranceFileKey) = oClaimVersions(0).InsuranceFileKey
                        Session.Item(CNPolicyNumber) = oClaimVersions(0).InsuranceRef
                    End If
                    For iCount As Integer = 0 To oClaimVersions.Count - 1
                        If oClaimVersions(iCount).Version = iHighest Then
                            'Retreival of claim details                            
                            Try
                                'This is expected to through an error if claim is locked in BO
                                oClaimDetails = GetClaimDetailsCall(oClaimVersions(iCount).ClaimKey, sBranchCode)
                            Catch ex As NexusProvider.NexusException
                                'Claim locking error
                                Select Case CType(ex.Errors(0), NexusProvider.NexusError).Code
                                    Case "200" 'Claim Locking
                                        'Show Claim locking error as alert
                                        Dim sMessage As String = "alert('" + ex.Errors(0).Description + ".\n" + ex.Errors(0).Detail + "')"
                                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "claimlocked", sMessage, True)
                                        Server.ClearError()
                                        ClearQuote()
                                        ClearClaims()
                                        Exit Sub
                                    Case Else
                                        'Clear all claim related sessions and throw the error
                                        ClearQuote()
                                        ClearClaims()
                                        Throw ex
                                End Select
                            End Try
                            'check for closed claim
                            'If e.CommandName = "Pay" AndAlso oClaimDetails IsNot Nothing Then
                            If oClaimDetails IsNot Nothing Then
                                If Not String.IsNullOrEmpty(oClaimDetails.ClaimStatus) AndAlso oClaimDetails.ClaimStatus.Trim.ToUpper = "CLOSED" Then
                                    Exit Sub
                                End If
                            End If

                            'Updation of session with claim details
                            With oClaimDetails
                                oOpenClaim.BaseCaseKey = .BaseCaseKey
                                oOpenClaim.CatastropheCode = .CatastropheCode
                                oOpenClaim.BaseClaimKey = .BaseClaimKey
                                oOpenClaim.Claim = .Claim
                                oOpenClaim.ClaimCoInsurer = .ClaimCoInsurer
                                oOpenClaim.ClaimDescription = .ClaimDescription
                                oOpenClaim.ClaimHandlerDescription = .ClaimHandlerDescription
                                oOpenClaim.ClaimKey = .ClaimKey
                                oOpenClaim.ClaimNumber = .ClaimNumber
                                oOpenClaim.ClaimPeril = .ClaimPeril
                                oOpenClaim.ClaimStatus = .ClaimStatus
                                oOpenClaim.ClaimStatusDate = .ClaimStatusDate
                                oOpenClaim.ClaimStatusID = .ClaimStatusID
                                oOpenClaim.ClaimVersion = .ClaimVersion
                                oOpenClaim.ClaimVersionDescription = .ClaimVersionDescription
                                oOpenClaim.ClientClaimNumber = .ClientClaimNumber
                                oOpenClaim.ClientEmail = .ClientEmail
                                oOpenClaim.ClientFaxNo = .ClientFaxNo
                                oOpenClaim.ClientMobileNo = .ClientMobileNo
                                oOpenClaim.ClientName = .ClientName
                                oOpenClaim.ClientShortName = oClaimVersions(0).ClientShortName 'IIf(.ClientShortName <> String.Empty, .ClientShortName, Trim(lblClientCode.Text))
                                oOpenClaim.ClientTelNo = .ClientTelNo
                                oOpenClaim.ClientTelNoOff = .ClientTelNoOff
                                oOpenClaim.CloseClaimOnZeroReserveRecoveryBalance = .CloseClaimOnZeroReserveRecoveryBalance
                                oOpenClaim.Comments = .Comments
                                oOpenClaim.Contact = .Contact
                                oOpenClaim.CurrencyISOCode = .CurrencyCode
                                oOpenClaim.Description = .Description
                                oOpenClaim.ExternalHandler = .ExternalHandler
                                oOpenClaim.HandlerCode = .HandlerCode
                                oOpenClaim.IgnoreClaimMaintain = .IgnoreClaimMaintain
                                oOpenClaim.InfoOnly = .InfoOnly
                                oOpenClaim.InsuranceFileKey = .InsuranceFileKey
                                oOpenClaim.InsuranceRef = .InsuranceRef
                                oOpenClaim.InsurerClaimNumber = .InsurerClaimNumber
                                oOpenClaim.IsAllowedClosedClaims = .IsAllowedClosedClaims
                                oOpenClaim.IsDeleted = .IsDeleted
                                oOpenClaim.LastModifiedDate = .LastModifiedDate
                                oOpenClaim.LikelyClaim = .LikelyClaim
                                oOpenClaim.Location = .Location
                                oOpenClaim.LossDate = .LossDate
                                oOpenClaim.LossDateFrom = .LossDateFrom
                                oOpenClaim.LossFromDate = .LossFromDate
                                oOpenClaim.LossToDate = .LossToDate
                                oOpenClaim.LossToDateSpecified = .LossToDateSpecified
                                oOpenClaim.Payments = .Payments
                                oOpenClaim.PolicyNumber = .PolicyNumber
                                oOpenClaim.PolicyType = .PolicyType
                                oOpenClaim.PrimaryCause = .PrimaryCause
                                oOpenClaim.PrimaryCauseCode = .PrimaryCauseCode
                                oOpenClaim.PrimaryCauseDescription = .PrimaryCauseDescription
                                oOpenClaim.ProductDescription = .ProductDescription
                                oOpenClaim.ProgressStatusCode = .ProgressStatusCode
                                oOpenClaim.ProgressStatusDescription = .ProgressStatusDescription
                                oOpenClaim.ReportedDate = .ReportedDate
                                oOpenClaim.Reserve = .Reserve
                                oOpenClaim.RiskKey = .RiskKey
                                oOpenClaim.RiskType = CType(Session(CNClaimQuote), NexusProvider.Quote).Risks.FindItemByRiskKey(.RiskKey).RiskTypeCode
                                oOpenClaim.RiskTypeDescription = CType(Session(CNClaimQuote), NexusProvider.Quote).Risks.FindItemByRiskKey(.RiskKey).Description
                                oOpenClaim.SecondaryCause = .SecondaryCause
                                oOpenClaim.SecondaryCauseCode = .SecondaryCauseCode
                                oOpenClaim.SecondaryCauseDescription = .SecondaryCauseDescription
                                oOpenClaim.TotalCurrentShareValue = .TotalCurrentShareValue
                                oOpenClaim.TotalShare = .TotalShare
                                oOpenClaim.Town = .Town
                                oOpenClaim.TownCode = .TownCode
                                oOpenClaim.UnderwritingYearCode = .UnderwritingYearCode
                                oOpenClaim.UserDefFldACode = .UserDefFldACode
                                oOpenClaim.UserDefFldBCode = .UserDefFldBCode
                                oOpenClaim.UserDefFldCCode = .UserDefFldCCode
                                oOpenClaim.UserDefFldDCode = .UserDefFldDCode
                                oOpenClaim.UserDefFldECode = .UserDefFldECode
                                If Session(CNMode) = Mode.ViewClaim Then
                                    oOpenClaim.IsRecovery = .IsRecovery
                                End If
                                oOpenClaim.TPA = .TPA
                                'Added for Insurer
                                oOpenClaim.Insurer = .Insurer
                                Session.Item(CNClaimTimeStamp) = .TimeStamp
                                oOpenClaim.CurrencyISOCode = .CurrencyCode
                                Session.Item(CNCurrenyCode) = Trim(.CurrencyCode) 'Changed
                                oOpenClaim.Client = .Client
                                Session.Item(CNBaseCaseKey) = .BaseCaseKey
                                'this needs to be removed after SAM issue is resolved
                                If oOpenClaim.Client.PartyKey = 0 Then
                                    oOpenClaim.Client.PartyKey = oQuote.PartyKey
                                End If
                                'Session(CNInsurer_Header) = .ClientName
                                Session(CNClaimNumber) = .ClaimNumber
                                Session(CNStatus) = .ClaimStatus

                            End With
                        End If
                    Next
                    Session(CNClaim) = oOpenClaim
                Finally
                    oClaimDetails = Nothing
                    oWebservice = Nothing
                    oClaimRisk = Nothing
                End Try
            End If
        End Sub


        Protected Sub hypRisk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hypRisk.Click

            Dim oNexusConfig As Config.NexusFrameWork
            Dim oPortal As Nexus.Library.Config.Portal
            Dim oOpenClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            oNexusConfig = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            oPortal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
            If oOpenClaim.RiskType.Trim() <> String.Empty Then
                Dim sFirstPage As String = Nothing
                oNexusConfig = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                oPortal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
                Dim oClaimQuote As NexusProvider.Quote = Session(CNClaimQuote)
                Dim oRiskType As New NexusProvider.RiskType
                Dim oProductConfig As Nexus.Library.Config.Product = oPortal.Products.Product(oClaimQuote.ProductCode)
                Dim oRisk As Config.RiskType = oProductConfig.RiskTypes.RiskType(oOpenClaim.RiskType.Trim())
                Dim sFolder As String = AppSettings("WebRoot") & oNexusConfig.ProductsFolder & "/" & oProductConfig.Name & "/" & oRisk.Path & "/"

                If IO.File.Exists(Server.MapPath(sFolder & "/fullquote.config")) Then

                    Dim sMainDetails As String = Nothing
                    Dim sXMLPath As String = Server.MapPath(sFolder & "/fullquote.config")

                    Dim xmlds As New XmlDataSource
                    xmlds.DataFile = sXMLPath
                    xmlds.EnableCaching = False

                    Dim Navigator As XPathNavigator
                    Dim Doc As XPathDocument = New XPathDocument(sXMLPath)
                    Navigator = Doc.CreateNavigator()
                    Dim i As XPathNodeIterator

                    i = Navigator.Select("/screens/screen/tab[1]")

                    While (i.MoveNext)
                        sFirstPage = i.Current.GetAttribute("url", String.Empty)
                        sMainDetails = i.Current.GetAttribute("maindetails", String.Empty)
                    End While

                    If Session(CNClaimNumber) IsNot Nothing Then
                        If String.IsNullOrEmpty(Session(CNClaimNumber).ToString) = False Then
                            hypRisk.Visible = True
                            Session(CNCurrentOI) = Session(CNOI)
                            Session(CNCurrentMode) = Session(CNMode)
                            Session(CNMode) = Mode.Review
                            Dim oQuote As NexusProvider.Quote = Nothing
                            oQuote = oWebservice.GetHeaderAndSummariesByKey(oClaimQuote.InsuranceFileKey)

                            'get the risk associated to that claim only
                            oWebservice.GetRisk(oOpenClaim.RiskKey, 0, oQuote)

                            oWebservice.GetHeaderAndRisksByKey(oQuote)

                            Session(CNQuote) = oQuote

                            Session(CNCurrenyCode) = oQuote.CurrencyCode

                            'Use the GetDataSetDefinition to interogate the dataset to get the datamodelcode into session
                            GetDataSetDefinition()
                            Session(CNMTAType) = Nothing
                            Session(CNQuoteInSync) = False
                            Session.Remove(CNOI)
                            Session(CNQuoteMode) = QuoteMode.FullQuote

                            'set up the risk type object from the details in config
                            oRiskType.DataModelCode = oRisk.DataModelCode
                            oRiskType.Name = oRisk.Name
                            oRiskType.RiskCode = oRisk.RiskCode
                            oRiskType.Path = oRisk.Path
                            Session(CNRiskType) = oRiskType

                            Dim sFirstRiskPage As String = String.Empty
                            'Get maindetails.aspx page path
                            Dim sFirstPagePath As String = AppSettings("WebRoot") & oNexusConfig.ProductsFolder & "/" & oProductConfig.Name & "/"
                            'checked maindetails attribute from fullquote.config file
                            If Session(CNQuoteMode) = QuoteMode.FullQuote Then
                                If sMainDetails.ToLower = "true" Then
                                    sMainDetails = "false"
                                End If
                            End If
                            If sMainDetails.ToLower = "false" Then
                                'If maindetails attribute is false go to tab[2] in fullquote.config
                                sFirstPagePath = AppSettings("WebRoot") & oNexusConfig.ProductsFolder & "/" & oProductConfig.Name & "/" & oRisk.Path & "/"
                                i = Navigator.Select("/screens/screen/tab[2]")
                                While i.MoveNext()
                                    sFirstPage = i.Current.GetAttribute("url", String.Empty)
                                    Response.Redirect(sFolder & "/" & sFirstPage)
                                End While
                            Else
                                i = Navigator.Select("/screens/screen/tab[1]")
                                While i.MoveNext()
                                    sFirstPage = i.Current.GetAttribute("url", String.Empty)
                                    Response.Redirect(sFolder & "/" & sFirstPage)
                                End While
                            End If

                        End If
                    End If
                End If
            End If
        End Sub
        Function CheckValidProduct(ByVal v_sProductCode As String) As Boolean
            'Check the product where it is configurent in Nexus or not
            Dim bReturn As Boolean = False
            Dim oProducts As Config.Products = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products
            For Each oProduct As Config.Product In oProducts
                If v_sProductCode.Trim.ToUpper = oProduct.ProductCode.Trim.ToUpper Then
                    bReturn = True
                End If
            Next
            Return bReturn
        End Function

        Protected Sub rptrPolicyAssociate_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptrPolicyAssociate.ItemDataBound
            Dim btnSelectButton As HyperLink = CType(e.Item.FindControl("hypPolicyAssociate"), HyperLink)
            If (e.Item.ItemType.ToString().ToLower() = "item" OrElse e.Item.ItemType.ToString().ToLower() = "alternatingitem") Then
                If HttpContext.Current.Session.IsCookieless Then
                    btnSelectButton.NavigateUrl = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/ClientDetailsForClaims.aspx?InsuranceFileAssociateKey=" & CType(e.Item.DataItem, NexusProvider.PolicyAssociate).InsuranceFileAssociatesKey & "&modal=true&KeepThis=true&TB_iframe=true&height=600&width=750' , null);return false;"
                Else
                    btnSelectButton.NavigateUrl = AppSettings("WebRoot") & "/Modal/ClientDetailsForClaims.aspx?InsuranceFileAssociateKey=" & CType(e.Item.DataItem, NexusProvider.PolicyAssociate).InsuranceFileAssociatesKey & "&modal=true&KeepThis=true&TB_iframe=true&height=600&width=750' , null);return false;"
                End If

                If DirectCast(e.Item.DataItem, NexusProvider.PolicyAssociate).IsDeleted Then
                    btnSelectButton.Visible = False
                End If
            End If
        End Sub

    End Class

End Namespace
