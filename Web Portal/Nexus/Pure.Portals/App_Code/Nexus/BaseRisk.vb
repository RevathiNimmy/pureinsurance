Imports System.Xml.XmlReader
Imports System.Xml.XPath
Imports System.Xml
Imports Nexus.Utils
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports CMS.Library
Imports System.Data
Imports Nexus
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports SiriusFS.SAM.Client
Imports NexusProvider.Quote
Imports System.Linq
Imports System.Text
Imports System.Xml.Linq
Imports System.IO
Imports Nexus.Constants.Constant
Imports System.Resources
Imports System.Reflection

Namespace Nexus

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class BaseRisk : Inherits CMS.Library.Frontend.clsCMSPage

        Private oMaster As ContentPlaceHolder

        Private oOI As Collections.Stack

        Private sScreenCode As String
        Private iDepth As Integer
        Private sParentTab As String

        Private sNextPage As String
        Private sPrevPage As String
        Private bIsAgentPending As Boolean
        Private bCopyQuote As Boolean = True
        Private sTabIndexControlID As String = "ctrlTabIndex"
        Private _InsuranceFileKey As Integer
        Private _ClaimKey As Integer
        Private _PartyKey As Integer

        Private oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        'This is the status of the renewal Waiting Status in Back office
        Const sAwaiting_Manual_Preview = "Awaiting Manual Review"
        Const sAwaiting_Renewal_Notice = "Awaiting Renewal Notice Print"
        Const sAwaiting_Update = "Awaiting Update"
        Public Const CNBrokerCollection As String = "BrokerCollection"
        Dim sSummaryOfRisk As String = String.Empty
        Dim sSummaryOfCover As String = String.Empty
        Dim sSummaryOfRiskURL As String = String.Empty
        Dim sSummaryOfCoverURL As String = String.Empty
        Dim sReferScreen As String = String.Empty
        Dim sDeclineScreen As String = String.Empty
        Dim sReferScreenURL As String = String.Empty
        Dim sDeclineScreenURL As String = String.Empty

        Dim sMessageBody As String = String.Empty
        Dim sEmailTo As String = String.Empty
        Dim SEmailSubject As String = String.Empty

        Protected dPremium As Double
        Protected dTax As Double
        Protected dTaxRate As Double
        Protected dTotalPremium As Double
        Protected dSumInsured As Double
        Protected dFee As Double
        ''' <summary>
        ''' Property to identify the TabIndexControl, as the TabIndex determines the location within
        ''' the risk dataset. This only needs be set if the TabIndex is not labelled 'ctrlTabIndex'
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TabIndexControlID() As String
            Get
                Return sTabIndexControlID
            End Get
            Set(ByVal value As String)
                sTabIndexControlID = value
            End Set
        End Property

        ''' <summary>
        ''' Allows the BranchCode to be returned without having an knowledge of the Nexus config
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property BranchCode() As String
            Get
                Return oNexusConfig.BranchCode
            End Get
        End Property
        ''' <summary>
        ''' Register Client Script to Show Validation And Reset Branch
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            If Not Page.IsPostBack Then
                Dim strClientScript As StringBuilder = New StringBuilder
                strClientScript.Append("function ShowValidationAndResetBranch(sBranchCode) {")
                strClientScript.Append("alert('The Agent does not have access to the selected branch');")
                strClientScript.Append("$('#<%=POLICYHEADER__BRANCH.ClientID%>').val(sBranchCode);")
                strClientScript.Append("}")

                If Not ClientScript.IsClientScriptBlockRegistered("ShowValidationAndResetBranch") Then
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowValidationAndResetBranch", strClientScript.ToString(), True)
                End If
            End If
        End Sub

        Private Shadows Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            Dim objNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim sDetailsMessage As String = GetLocalResource("lblMsgDetails", "The agent will not be able to view this quote version until you Issue it. Are you sure you wish to continue?")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "DetailsConfirmation", _
            "<script language=""javascript"" type=""text/javascript"">function showMessageDetails() {return confirm('" & sDetailsMessage & "');}</script>")
            Dim sIssuedMessage As String = GetLocalResource("lblMsgIssued", "This will change the status of the quote version to Issued and you will not subsequently be able to make any changes to this quote without creating a new Pending version. Are you sure you wish to proceed?")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "IssuedConfirmation", _
                       "<script language=""javascript"" type=""text/javascript"">function showMessageIssued() {return confirm('" & sIssuedMessage & "');}</script>")


            'Needs to be in the preinit as the controls will already have been initialized if any
            'later and they'll fail if they need any of the session values below that are being set

            If Not IsPostBack Then

                ''if user is requesting for a New Quote then we need to clear the existing quote
                ''added by SB on dated 07-Apr
                If Request("newquote") = "true" Then 'if user is creating a new quote
                    ClearClaims()
                    ClearHeader()
                    ClearQuote()
                    Session.Remove(CNTabState & "_" & sTabIndexControlID)
                    Session("SWOnThisRiskPage") = Nothing
                    Dim sProductPath() As String = CStr(Request.ApplicationPath & "/" & oNexusConfig.ProductsFolder) _
                        .Split(Regex.Split("/", ""), StringSplitOptions.RemoveEmptyEntries)
                    Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
                    Dim oProductConfig As Nexus.Library.Config.Product = oPortal.Products.GetProductByName(Server.UrlDecode( _
                      Request.Url.Segments(sProductPath.Length + 1).TrimEnd("/")))

                    If Session(CNRiskType) Is Nothing AndAlso String.IsNullOrEmpty(Request.QueryString("riskcode")) Then
                        'if risktype is not set
                        'Session(CNRiskType) Is Nothing means that user is not comming from "NewQuote" control
                        Dim oRisk As Config.RiskType = oProductConfig.RiskTypes.RiskType(0)
                        Dim oRiskType As New NexusProvider.RiskType
                        oRiskType.DataModelCode = oRisk.DataModelCode
                        oRiskType.Name = oRisk.Name
                        oRiskType.Path = oRisk.Path
                        oRiskType.RiskCode = oRisk.RiskCode
                        Session(CNRiskType) = oRiskType

                    ElseIf Session(CNRiskType) Is Nothing AndAlso String.IsNullOrEmpty(Request.QueryString("riskcode")) = False Then
                        'if risktype is set
                        'Session(CNRiskType) Is Nothing means that user is not comming from "NewQuote" control
                        Dim sRiskCode As String = Request.QueryString("riskcode")
                        Dim oRisk As Config.RiskType = oProductConfig.RiskTypes.RiskType(sRiskCode)
                        Dim oRiskType As New NexusProvider.RiskType
                        oRiskType.DataModelCode = oRisk.DataModelCode
                        oRiskType.Name = oRisk.Name
                        oRiskType.Path = oRisk.Path
                        oRiskType.RiskCode = oRisk.RiskCode
                        Session(CNRiskType) = oRiskType
                    End If

                    'To Clear the Party Session Variable
                    If Request("newclient") = "true" Then
                        Session.Remove(CNParty)
                        Session.Remove(CNPartyDataModelCode)
                        Session.Remove(CNOI)
                        Session.Remove(CNClientMode)
                    End If

                    'To Remove the newquote status
                    Dim url As String = String.Empty
                    If HttpContext.Current.Session.IsCookieless Then
                        url = Left(Request.Url.AbsoluteUri, Request.Url.AbsoluteUri.LastIndexOf("?"))
                        Dim iIndex As Integer = url.IndexOf(AppSettings("WebRoot"))
                        iIndex = iIndex + Convert.ToInt16(Convert.ToString(AppSettings("WebRoot")).Length)
                        url = url.Insert(iIndex, "(S(" & Session.SessionID.ToString() + "))/")

                    Else
                        url = Left(Request.Url.AbsoluteUri, Request.Url.AbsoluteUri.LastIndexOf("?"))
                    End If

                    Response.Redirect(url, False)
                End If

                'DH - 09-01-08 - Improved session handling and random user navigation, sesison timeouts etc .. should
                '               be able to handle more problems. This could probably be improved but it works for now.

                Dim bConfigureQuote As Boolean = True

                If Session(CNQuoteInSync) Is Nothing Or Not TypeOf Session(CNQuoteInSync) Is Boolean Then
                    'Reset Quote
                    Session.Remove(CNDataModelCode)
                    Session.Remove(CNQuote)
                    Session.Remove(CNOI)
                    Session.Remove(CNRiskProgress)
                Else
                    If CType(Session(CNQuoteInSync), Boolean) Then
                        'Everything is fine with the quote so continue
                        bConfigureQuote = False
                    Else
                        'Quote maybe invalid so check
                    End If
                End If

                If bConfigureQuote Then

                    Dim sDataModelCode As String
                    Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
                    Dim sProductPath() As String
                    Dim oRiskType As NexusProvider.RiskType = Session(CNRiskType)
                    sProductPath = CStr(Request.ApplicationPath & "/" & oNexusConfig.ProductsFolder) _
                        .Split(Regex.Split("/", ""), StringSplitOptions.RemoveEmptyEntries)

                    Dim oProduct As Nexus.Library.Config.Product = oPortal.Products.GetProductByName(Server.UrlDecode(Request.Url.Segments( _
                        sProductPath.Length + 1).TrimEnd("/")))

                    If oProduct Is Nothing Then
                        Throw New Exception("Product can NOT be found")

                    Else
                        '1.3
                        If CType(Session(CNLoginType), LoginType) = LoginType.Agent Then
                            Dim sAllowedAgent() As String
                            Dim bMatched As Boolean = False
                            Dim oUserDetails As NexusProvider.UserDetails = Session.Item(CNAgentDetails)
                            Dim iCounter As Integer = 0
                            sAllowedAgent = oPortal.Products.GetAllowedAgentByName(Server.UrlDecode( _
                                                        Request.Url.Segments(sProductPath.Length + 1).TrimEnd("/"))).Split(",")

                            If sAllowedAgent.Length = 1 And String.IsNullOrEmpty(sAllowedAgent(0)) Then
                                'no agents specified, so any allowed
                                bMatched = True
                            Else
                                'loop through array of allowed agents. If any match current user then set flag
                                For iCounter = 0 To sAllowedAgent.Length - 1
                                    If sAllowedAgent(iCounter).ToUpper() = oUserDetails.PartyName.ToUpper() Then
                                        bMatched = True
                                        Exit For
                                    End If
                                Next
                            End If

                            If bMatched = False Then
                                Throw New Exception("Access denied to product due to configuration")
                            End If
                        End If

                        'Need to select the datamodelcode of the selected risk
                        If oRiskType IsNot Nothing Then
                            sDataModelCode = oRiskType.DataModelCode
                              If Session(CNMode) = Mode.View Then
                                Session.Remove(CNOI)
                            End If 
                        End If
                    End If

                    If Not String.Equals(sDataModelCode.ToUpper, Session(CNDataModelCode)) Then

                        Session(CNDataModelCode) = sDataModelCode.ToUpper

                        'retrieved DataModelCode does not match that in session so we've changed
                        'product, so check we are on the first page as a new quote will be created

                        Dim oProductConfig As Nexus.Library.Config.Product = oPortal.Products.GetProductByName(Server.UrlDecode( _
                            Request.Url.Segments(sProductPath.Length + 1).TrimEnd("/")))
                        'oPortal.Products.Product(CType(Session(CNQuote), NexusProvider.Quote).ProductCode)

                        Dim sFolder As String = AppSettings("WebRoot") & oNexusConfig.ProductsFolder & "/" & oProductConfig.Name
                        Dim oNexusFramework As Config.NexusFrameWork
                        Dim oQuote As NexusProvider.Quote = HttpContext.Current.Session(CNQuote)
                        Dim oRisk As Config.RiskType
                        'This code will solve the purpose in case of Anonymous Agent when client is not selected
                        'Session(CnriskType) is nothing so have to repopulate this here PN 60503
                        If Session(CNParty) Is Nothing And oRiskType Is Nothing Then
                            oRisk = oProductConfig.RiskTypes.RiskType(0)
                            Dim oRiskAnonymousType As New NexusProvider.RiskType
                            oRiskAnonymousType.DataModelCode = oRisk.DataModelCode
                            oRiskAnonymousType.Name = oRisk.Name
                            oRiskAnonymousType.Path = oRisk.Path
                            oRiskAnonymousType.RiskCode = oRisk.RiskCode
                            Session(CNRiskType) = oRiskAnonymousType
                            oRiskType = Session(CNRiskType)
                        End If


                        If HttpContext.Current.Session.Item(CNQuote) IsNot Nothing Then

                            oNexusFramework = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

                            For iCount As Integer = 0 To oQuote.Risks.Count - 1
                                If HttpContext.Current.Session(CNRiskType) Is Nothing Then

                                    If oQuote.Risks(HttpContext.Current.Session(CNCurrentRiskKey)).RiskCode Is Nothing Then
                                        oRisk = oProduct.RiskTypes.RiskType(oQuote.Risks(iCount).RiskTypeCode.Trim)
                                    Else
                                        oRisk = oProduct.RiskTypes.RiskType(oQuote.Risks(iCount).RiskCode.Trim)
                                    End If

                                    Dim oRiskAnonymousType As New NexusProvider.RiskType
                                    oRiskAnonymousType.DataModelCode = oRisk.DataModelCode
                                    oRiskAnonymousType.Name = oRisk.Name
                                    oRiskAnonymousType.Path = oRisk.Path
                                    oRiskAnonymousType.RiskCode = oRisk.RiskCode
                                    HttpContext.Current.Session(CNRiskType) = oRiskAnonymousType
                                    oRiskType = HttpContext.Current.Session(CNRiskType)
                                End If
                                Exit For
                            Next
                        End If

                        Dim sXMLPath As String = Server.MapPath(sFolder & "\" & oRiskType.Path & "\")

                        If oProductConfig.QuickQuoteConfig = String.Empty Then
                            'No quickquote for product, so FullQuote
                            Session.Item(CNQuoteMode) = QuoteMode.FullQuote
                        Else
                            If Session.Item(CNQuoteMode) Is Nothing Then
                                'No QuoteMode in session and quickquote product available, so QuickQuote
                                Session.Item(CNQuoteMode) = QuoteMode.QuickQuote
                            Else
                                'Don't override QuoteMode as QQ and FQ are available for the product
                            End If
                        End If

                        Select Case CType(Session.Item(CNQuoteMode), QuoteMode)
                            Case QuoteMode.QuickQuote
                                sXMLPath = sXMLPath & oProductConfig.QuickQuoteConfig
                            Case QuoteMode.FullQuote
                                sXMLPath = sXMLPath & oProductConfig.FullQuoteConfig
                        End Select

                        Dim xmlds As New XmlDataSource
                        xmlds.DataFile = sXMLPath
                        xmlds.EnableCaching = False

                        Dim Navigator As XPathNavigator
                        Dim Doc As XPathDocument = New XPathDocument(sXMLPath)
                        Navigator = Doc.CreateNavigator()
                        Dim i As XPathNodeIterator
                        Dim sMainDetail As String = String.Empty

                        i = Navigator.Select("/screens/screen/tab[1]")

                        Dim sFirstPage As String = String.Empty

                        While (i.MoveNext)
                            sFirstPage = i.Current.GetAttribute("url", String.Empty)
                            sMainDetail = i.Current.GetAttribute("maindetails", String.Empty)
                        End While
                        If sMainDetail.ToLower = "false" Then
                            i = Navigator.Select("/screens/screen/tab[2]")
                            While (i.MoveNext)
                                sFirstPage = i.Current.GetAttribute("url", String.Empty)
                            End While
                        Else
                            If sMainDetail.ToLower = "true" And oPortal.UseCorePolicyHeader = True Then
                                i = Navigator.Select("/screens/screen/tab[2]")
                                While (i.MoveNext)
                                    sFirstPage = i.Current.GetAttribute("url", String.Empty)
                                End While
                            Else
                                i = Navigator.Select("/screens/screen/tab[1]")
                            End If


                        End If
                            If Not IsCurrentPage(sFirstPage) Then

                            'We're not on the first risk screen and we really should be

                            'stops all the processing being down again on then correct
                            'risks screen, as we've already collected all the info we need
                            Session(CNQuoteInSync) = True

                            Response.Redirect(sFolder & "/" & sFirstPage, False)

                        End If

                    Else
                        Session(CNDataModelCode) = sDataModelCode.ToUpper
                    End If

                End If

            End If

            'need to restrict in all the cases(FQ,MTA etc) except QQ  
            If Session.Item(CNQuoteMode) <> QuoteMode.QuickQuote And Not HttpContext.Current.User.Identity.IsAuthenticated Then
                Response.Redirect("~/login.aspx", False)
            End If

            '09-01-08 - DH
            'Set to false to force a correct exit point to be taken out of the risk page
            'e.g back/next, tab index, random navigation out the risk screen will cause
            'the quote to no longer be the active quote, broswer back/forward buttons
            'will still work. If still false on the next risk screen initialize the
            'quote will validated before the user can continue, if not correct we'll
            'assmume the product/client selection has changed or we really are intending
            'to start a new quote
            Session(CNQuoteInSync) = False

            'Dim oProductConfig As Config.Product = oPortal.Products.Product(Session.Item(CNDataModelCode))

            oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)

            Dim oTabIndex As TabIndex = oMaster.FindControl(sTabIndexControlID)

            If oTabIndex IsNot Nothing Then
                AddHandler oTabIndex.ValuesInitialized, AddressOf TabIndexInitialized
                AddHandler oTabIndex.TabClicked, AddressOf TabClick

                If CType(Session.Item(CNMode), Mode) = Mode.View Or CType(Session.Item(CNMode), Mode) = Mode.Review Then
                    DisableControls(oMaster)
                End If
            End If

            Response.Cache.SetCacheability(HttpCacheability.NoCache)

        End Sub

        ''' <summary>
        ''' Handles the initialization event from the TabIndex, this is
        ''' intended to return the values set within the TabIndex
        ''' </summary>
        ''' <param name="v_sScreenCode">Current screen code, need for running the Add and Edit rules via SAM</param>
        ''' <param name="v_iDepth">Current screen depth of the tabs</param>
        ''' <param name="v_sPreviousPage">Path to the previous page</param>
        ''' <param name="v_sNextPage">Path to the next page</param>
        ''' <remarks></remarks>
        Public Sub TabIndexInitialized(ByVal v_sScreenCode As String, _
                                ByVal v_iDepth As Integer, _
                                 ByVal v_sParentTab As String, _
                                ByVal v_sPreviousPage As String, _
                                ByVal v_sNextPage As String)

            sScreenCode = v_sScreenCode
            iDepth = v_iDepth
            sParentTab = v_sParentTab
            sPrevPage = v_sPreviousPage
            sNextPage = v_sNextPage

        End Sub

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Dim oQuote As NexusProvider.Quote = CType(Session(CNQuote), NexusProvider.Quote)
            Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
            Dim bIsTrueMonthlyPolicy As Boolean
            Dim bIsUnifiedRenewalDayReadOnly As Boolean
            Dim sProductPath() As String
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oRiskType As NexusProvider.RiskType = CType(Session(CNRiskType), NexusProvider.RiskType)
            Dim sUnifiedRenewalDay As String
            Dim nUnifiedRenewalDay As Integer
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oMaster As ContentPlaceHolder = GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName)
            Dim hdnIsTrueMonthlyProduct As HiddenField = CType(oMaster.FindControl("hfIsTrueMonthlyProduct"), 
                                                               HiddenField)
            Dim oFrequency As Object = oMaster.FindControl("POLICYHEADER__FREQUENCY")
            Dim hdnIsUnifiedRenewalDayReadOnly As HiddenField =
                    CType(oMaster.FindControl("hfIsUnifiedRenewalDayReadOnly"), HiddenField)

            Dim ddlUnifiedRenewalDay As DropDownList = CType(oMaster.FindControl("POLICYHEADER__UNIFIEDRENEWALDAY"), 
                                                             DropDownList)
            Dim lblUnifiedRenewalDay As Label = CType(oMaster.FindControl("lblUnifiedRenewalDay"), Label)
            Dim lblAnniversary As Label = CType(oMaster.FindControl("lblAnniversary"), Label)
            Dim txtCoverEndDate As TextBox = CType(oMaster.FindControl("POLICYHEADER__COVERENDDATE"), TextBox)
            Dim txtAnniversaryDate As TextBox = CType(oMaster.FindControl("POLICYHEADER__ANNIVERSARY"), TextBox)

            If hdnIsTrueMonthlyProduct IsNot Nothing Then
                hdnIsTrueMonthlyProduct.Value = 0.ToString()
            End If
            If hdnIsUnifiedRenewalDayReadOnly IsNot Nothing Then
                hdnIsUnifiedRenewalDayReadOnly.Value = 0.ToString()
            End If

            'Set product path from configuration
            sProductPath = CStr(Request.ApplicationPath & "/" & oNexusConfig.ProductsFolder).Split(Regex.Split("/", ""),
                                                                                                   StringSplitOptions.
                                                                                                      RemoveEmptyEntries)
            Dim oProduct As Config.Product =
                    oNexusFrameWork.Portals.Portal(Convert.ToString(Portal.GetPortalID())).Products.GetProductByName(
                        Server.UrlDecode(Request.Url.Segments(sProductPath.Length + 1).TrimEnd("/")))
            'Set Unified Renewal Day form Product Risk option.
            sUnifiedRenewalDay =
                oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance,
                                                      NexusProvider.ProductRiskOptions.UnifiedRenewalDay,
                                                      NexusProvider.RiskTypeOptions.Code, oProduct.ProductCode,
                                                      oRiskType.RiskCode)
            If sUnifiedRenewalDay Is Nothing OrElse sUnifiedRenewalDay = "" OrElse sUnifiedRenewalDay = "0" Then
                nUnifiedRenewalDay = 1
            Else
                nUnifiedRenewalDay = CType(sUnifiedRenewalDay, Integer)
            End If

            'Check True Monthly product options
            If Session(CNIsTrueMonthlyPolicy) IsNot Nothing Then
                bIsTrueMonthlyPolicy = CType(Session(CNIsUnifiedRenewalDayReadOnly), Boolean)
            Else
                bIsTrueMonthlyPolicy =
                    CType(
                        oWebService.GetProductRiskOptionValue(
                            NexusProvider.ProductConfigActionType.ProductRiskMaintenance,
                            NexusProvider.ProductRiskOptions.IsTrueMonthlyPolicy, NexusProvider.RiskTypeOptions.Code,
                            oProduct.ProductCode, oRiskType.RiskCode),
                        Boolean)
            End If

            If bIsTrueMonthlyPolicy = True Then
                'Check Product Risk Option for Unified Renewal Day ReadOnly field
                If Session(CNIsUnifiedRenewalDayReadOnly) IsNot Nothing Then
                    bIsUnifiedRenewalDayReadOnly = CType(Session(CNIsUnifiedRenewalDayReadOnly), Boolean)
                Else
                    bIsUnifiedRenewalDayReadOnly =
                        CType(
                            oWebService.GetProductRiskOptionValue(
                                NexusProvider.ProductConfigActionType.ProductRiskMaintenance,
                                NexusProvider.ProductRiskOptions.UnifiedRenewalDateIsReadOnly,
                                NexusProvider.RiskTypeOptions.Code, oProduct.ProductCode, oRiskType.RiskCode),
                            Boolean)
                End If
            End If
            If hdnIsTrueMonthlyProduct IsNot Nothing Then
                If bIsTrueMonthlyPolicy = True Then
                    'Initialise Frequency to Monthly
                    'Set product Risk option in hidden field, which is later used in Java script function
                    If hdnIsTrueMonthlyProduct IsNot Nothing Then
                        hdnIsTrueMonthlyProduct.Value = 1.ToString()
                    End If

                    If oFrequency IsNot Nothing Then
                        If TypeOf oFrequency Is DropDownList Then
                            CType(oFrequency, DropDownList).SelectedValue = "MONTH"
                        ElseIf TypeOf oFrequency Is NexusProvider.LookupList Then
                            CType(oFrequency, NexusProvider.LookupList).Value = "MONTH"
                        ElseIf TypeOf oFrequency Is NexusProvider.LookupListV2 Then
                            CType(oFrequency, NexusProvider.LookupListV2).Value = "MONTH"
                        End If
                    End If

                    'Populating the ddlUnifiedRenewalDay Drop Down List with 0 to 31
                    If ddlUnifiedRenewalDay IsNot Nothing Then
                        'existing items cleared
                        ddlUnifiedRenewalDay.Items.Clear()
                        'Fill items from 1 to 31
                        For iRenewalDay As Integer = 1 To 31
                            Dim lstRenewalDay As New ListItem
                            lstRenewalDay.Text = iRenewalDay.ToString()
                            lstRenewalDay.Value = iRenewalDay.ToString()
                            ddlUnifiedRenewalDay.Items.Add(lstRenewalDay)
                            ddlUnifiedRenewalDay.DataBind()
                        Next
                        If IsNumeric(nUnifiedRenewalDay) Then
                            ddlUnifiedRenewalDay.SelectedValue = nUnifiedRenewalDay.ToString()
                        Else
                            ddlUnifiedRenewalDay.SelectedIndex = -1
                        End If
                        If lblUnifiedRenewalDay IsNot Nothing Then
                            lblUnifiedRenewalDay.Visible = True
                        End If
                        ddlUnifiedRenewalDay.Visible = True
                    End If

                    'Set Anniversary label to visible true
                    If lblAnniversary IsNot Nothing Then
                        lblAnniversary.Visible = True
                    End If
                    'Set Anniversary text Box to visible true
                    If txtAnniversaryDate IsNot Nothing Then
                        txtAnniversaryDate.Visible = True
                    End If

                    If (bIsTrueMonthlyPolicy = True AndAlso bIsUnifiedRenewalDayReadOnly = True) Then
                        ' Set Unified Renewal Day combo to Disabled
                        If ddlUnifiedRenewalDay IsNot Nothing Then
                            ddlUnifiedRenewalDay.Enabled = False
                        End If
                        'Set Cover End Date textbox to Readonly
                        If txtCoverEndDate IsNot Nothing Then
                            txtCoverEndDate.ReadOnly = True
                        End If
                        'Set Cover End Date textbox to Disabled 
                        If txtCoverEndDate IsNot Nothing Then
                            txtCoverEndDate.Enabled = False
                        End If
                        'Set product Risk option in hidden field, which is later used in Java script function
                        If hdnIsUnifiedRenewalDayReadOnly IsNot Nothing Then
                            hdnIsUnifiedRenewalDayReadOnly.Value = 1.ToString()
                        End If
                    End If
                End If
            End If
            Select Case CType(Session.Item(CNQuoteMode), QuoteMode)
                Case QuoteMode.QuickQuote
                    SetPageProgress(1)
                Case Else
                    SetPageProgress(3)
            End Select

            If Session.Item(CNOI) IsNot Nothing AndAlso (Not (Session.Item(CNOI)).ToString().Contains("OI")) Then
                oOI = CType(Session.Item(CNOI), System.Collections.Stack)
            End If

            'Depth should be equal to number of OI's in the stack, so remove any additional ones from the end
            'gonna have problems if stack length is shorter then depth, can deal with length zero
            If oOI Is Nothing Then
                oOI = New Collections.Stack()
            Else
                While oOI.Count > iDepth + 1

                    'we've moved back up the dataset tree from child to parent, so we need to check

                    Dim _
                        srDataset As _
                            New System.IO.StringReader(oQuote.Risks(CInt(Session(CNCurrentRiskKey))).XMLDataset)
                    Dim xmlTR As New XmlTextReader(srDataset)
                    Dim xDoc As New XmlDocument

                    xDoc.Load(xmlTR)
                    xmlTR.Close()

                    Dim oNode As XmlNode = xDoc.SelectSingleNode("//*[@OI='" & oOI.Peek.ToString() & "' and @US='3']")
                    srDataset.Dispose()

                    If oNode IsNot Nothing Then
                        Dim oSAMClient As New SiriusFS.SAM.Client.DataSetControl.Application
                        oSAMClient.LoadFromXML(GetDataSetDefinition(Convert.ToString(Session(CNDataModelCode))),
                                               oQuote.Risks(CType(Session(CNCurrentRiskKey), Integer)).XMLDataset)
                        oSAMClient.DelObjectInstance(oNode.Name, oOI.Peek.ToString())
                        oSAMClient.ReturnAsXML(oQuote.Risks(CInt(Session(CNCurrentRiskKey))).XMLDataset)
                        oSAMClient.Terminate()
                    Else
                        'Remove invalid node from DB and XMl
                        RemoveInvalidNodeFromXML(oOI.Peek.ToString())
                    End If

                    Session(CNQuote) = oQuote
                    oOI.Pop()

                End While
            End If

            Session.Item(CNOI) = oOI

            GetScreens()
            oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)
            If IsPostBack Then
                'Dont load the data into the controls, just initialize them
                If oOI.Count > 0 Then
                    DataSetFunctions.ReadContainerFromXML(oMaster, Convert.ToString(oOI.Peek), Me, True, True)
                Else
                    'To Delete the child added into Dataset Abnormally
                    DataSetFunctions.DeleteElementFromXML(sScreenCode, Nothing, Nothing)
                    DataSetFunctions.ReadContainerFromXML(oMaster, String.Empty, Me, True, True)
                End If
            Else

                Dim oParty As NexusProvider.BaseParty = CType(Session.Item(CNParty), NexusProvider.BaseParty)
                Dim oPortal As Nexus.Library.Config.Portal =
                        oNexusConfig.Portals.Portal(Convert.ToString(Portal.GetPortalID()))

                If oParty Is Nothing AndAlso CType(Session.Item(CNLoginType), LoginType) = LoginType.Agent Then

                    'Added for Allowing Anonymous quote for Agent--AR
                    'Will have to be an anonymous policy, so create anonymous user


                    If String.IsNullOrEmpty(oPortal.AnnPartyID) Then
                        Throw New ArgumentException("Invalid AnnPartyID")
                    Else
                        oParty = oWebService.GetParty(CInt(oPortal.AnnPartyID))
                    End If
                    Select Case True
                        Case TypeOf oParty Is NexusProvider.CorporateParty
                            With CType(oParty, NexusProvider.CorporateParty)
                                Session(CNAnonymous) = .ClientSharedData.ShortName.Trim
                            End With
                        Case TypeOf oParty Is NexusProvider.PersonalParty
                            With CType(oParty, NexusProvider.PersonalParty)
                                Session(CNAnonymous) = .ClientSharedData.ShortName.Trim
                            End With
                        Case Else
                            'Unknown customer type, so hide control
                            Me.Visible = False
                    End Select

                    Session(CNIsAnonymous) = True
                    Session(CNParty) = oParty


                Else
                    'Remove the PartySummary cache as we are adding a new quote to the client
                    Cache.Remove("PartySummary_" & oParty.Key.ToString())
                End If


                If oQuote Is Nothing AndAlso Request("newquote") = "true" Then



                    Dim nOptionValue As Integer = Nothing

                    Dim sOptionTypeSetting As String = Nothing
                    Dim nGracePeriod As Integer = 0
                    Dim oOptionSettings As NexusProvider.OptionTypeSetting
                    Dim dCoverStartDate, dCoverEndDate As Date
                    'Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                    Dim IsTrueMonthlyPolicy As Boolean
                    Dim hUnifiedRenewalDay As Integer = Nothing
                    dCoverStartDate = GetCoverStartDate()
                    dCoverEndDate = GetCoverEndDate(dCoverStartDate)

                    oQuote = New NexusProvider.Quote(dCoverStartDate, dCoverEndDate, "Nexus Web Quote")

                    'S4B has no product codes, but S4I does so, so populate the field for S4B even though it won't
                    'be used, is a required attribute in the web.config (for S4I purposes) so leave blank for S4B
                    oQuote.ProductCode = oProduct.ProductCode

                    sOptionTypeSetting =
                        oWebService.GetProductRiskOptionValue(
                            NexusProvider.ProductConfigActionType.ProductRiskMaintenance,
                            NexusProvider.ProductRiskOptions.IsMidnightRenewal, NexusProvider.RiskTypeOptions.Code,
                            oProduct.ProductCode, oRiskType.RiskCode)
                    nOptionValue = CInt(sOptionTypeSetting.ToString)
                    'Fixed to handle conversion of String to Integer (It was throwing error when QuoteExpiryDate was set to null in BackOffice
                    nGracePeriod =
                        CInt(
                            IIf(
                                oWebService.GetProductRiskOptionValue(
                                    NexusProvider.ProductConfigActionType.ProductRiskMaintenance,
                                    NexusProvider.ProductRiskOptions.GracePeriod, NexusProvider.RiskTypeOptions.Code,
                                    oProduct.ProductCode, oRiskType.RiskCode) = "", 0,
                                oWebService.GetProductRiskOptionValue(
                                    NexusProvider.ProductConfigActionType.ProductRiskMaintenance,
                                    NexusProvider.ProductRiskOptions.GracePeriod, NexusProvider.RiskTypeOptions.Code,
                                    oProduct.ProductCode, oRiskType.RiskCode)))
                    oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 1009)
                    IsTrueMonthlyPolicy = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsTrueMonthlyPolicy, NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, oRiskType.RiskCode)
                    With oQuote
                        Select Case True
                            Case TypeOf oParty Is NexusProvider.CorporateParty
                                'if logged in user if Corporate
                                With CType(oParty, NexusProvider.CorporateParty)
                                    oQuote.InsuredName = .CompanyName
                                End With
                            Case TypeOf oParty Is NexusProvider.PersonalParty
                                'if logged in user if Personal
                                With CType(oParty, NexusProvider.PersonalParty)
                                    oQuote.InsuredName = .Title & " " & .Forename & " " & .Lastname
                                End With
                        End Select

                        oQuote.CoverStartDate = CType(DateTime.Now.ToShortDateString(), Date) _
                        'DateTime.Now.ToString("MM/dd/yyyy")
                        If (bIsTrueMonthlyPolicy) Then
                            'All the code for the mentioned steps will go here
                            nUnifiedRenewalDay =
                                CType(
                                    oWebService.GetProductRiskOptionValue(
                                        NexusProvider.ProductConfigActionType.ProductRiskMaintenance,
                                        NexusProvider.ProductRiskOptions.UnifiedRenewalDay,
                                        NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, oRiskType.RiskCode),
                                    Integer)

                            If nUnifiedRenewalDay = "0" Then
                                'when "Product Risk Maintenance"  “Unified Renewal Day” set as blank

                                Dim hSetUnifiedRenewalDay As HiddenField =
                                        CType(
                                            GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName).FindControl(
                                                "iUnifiedRenewalDay"),
                                            HiddenField)
                                If hSetUnifiedRenewalDay IsNot Nothing Then
                                    hSetUnifiedRenewalDay.Value = oQuote.CoverStartDate.Day.ToString()
                                    'iUnifiedRenewalDay = oQuote.CoverStartDate.Day
                                End If
                            End If

                            'When TRUE MONTHLY POLICY ON -  'Adding one Month to cover start date
                            Dim dtCoverEnd As Date
                            dtCoverEnd = CType(oQuote.CoverStartDate.AddMonths(1).ToShortDateString(), Date)
                            If _
                                IsDate(
                                    CInt(nUnifiedRenewalDay) & "/" & CDate(dtCoverEnd).Month & "/" &
                                    CDate(dtCoverEnd).Year) And nUnifiedRenewalDay <> "0" Then
                                If _
                                    dtCoverEnd <
                                    CDate(
                                        (CInt(nUnifiedRenewalDay) & "/" & CDate(dtCoverEnd).Month & "/" &
                                         CDate(dtCoverEnd).Year)) AndAlso
                                    CDate(oQuote.CoverStartDate).Day < CInt(nUnifiedRenewalDay) Then
                                    oQuote.CoverEndDate =
                                        CDate(
                                            CInt(nUnifiedRenewalDay) & "/" & CDate(oQuote.CoverStartDate).Month & "/" &
                                            CDate(oQuote.CoverStartDate).Year)
                                ElseIf _
                                    dtCoverEnd <
                                    CDate(
                                        (CInt(nUnifiedRenewalDay) & "/" & CDate(dtCoverEnd).Month & "/" &
                                         CDate(dtCoverEnd).Year)) AndAlso
                                    CDate(oQuote.CoverStartDate).Day > CInt(nUnifiedRenewalDay) Then
                                    oQuote.CoverEndDate =
                                        CDate(
                                            CInt(nUnifiedRenewalDay) & "/" & CDate(dtCoverEnd).Month & "/" &
                                            CDate(dtCoverEnd).Year)
                                Else
                                    oQuote.CoverEndDate =
                                        CType(
                                            CDate(
                                                CInt(nUnifiedRenewalDay) & "/" & CDate(dtCoverEnd).Month & "/" &
                                                CDate(dtCoverEnd).Year),
                                            Date)
                                End If
                            Else
                                oQuote.CoverEndDate = CType(oQuote.CoverStartDate.AddMonths(1).ToShortDateString(), Date)
                            End If
                        Else
                            oQuote.CoverEndDate = CType(oQuote.CoverStartDate.AddYears(1).ToShortDateString(), Date)
                        End If
                        oQuote.InceptionDate = CType(oQuote.CoverStartDate.ToShortDateString(), Date)
                        oQuote.InceptionTPI = CType(DateTime.Now.ToShortDateString(), Date)
                        oQuote.QuoteExpiryDate = CType(oQuote.CoverStartDate.AddDays(nGracePeriod).ToShortDateString(),
                                                       Date)
                        oQuote.ProposalDate = CType(oQuote.CoverStartDate.ToShortDateString(), Date)

                        'Checkhing the Value of Midnight Renewal Settings
                        If sOptionTypeSetting = 1 Then
                            'Adding 366 days to Renewal Date and cover to date
                            oQuote.CoverEndDate = CType(oQuote.CoverEndDate.AddDays(-1).ToShortDateString(), Date)
                            oQuote.RenewalDate = CType(oQuote.CoverEndDate.AddDays(1).ToShortDateString(), Date)
                        Else
                            'Adding 365 days to Renewal Date
                            oQuote.RenewalDate = CType(oQuote.CoverEndDate.ToShortDateString(), Date)
                        End If
                        'Checkhing the Value of UnifiedRenewalDay Settings When True Monthly Policies ON
                        If (bIsTrueMonthlyPolicy) Then
                            Dim txtAnniversary As TextBox = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__ANNIVERSARY"), TextBox)
                            If txtAnniversaryDate IsNot Nothing Then
                                txtAnniversaryDate.Text = oQuote.CoverStartDate.AddYears(1).ToShortDateString()
                            End If

                            If IsNumeric(nUnifiedRenewalDay) Then
                                If nUnifiedRenewalDay = "0" Then
                                    oQuote.RenewalDayNo = oQuote.CoverStartDate.Day
                                    If _
                                        IsDate(
                                            oQuote.RenewalDayNo & "/" & oQuote.RenewalDate.Month & "/" &
                                            oQuote.RenewalDate.AddYears(1)) = True Then
                                        If txtAnniversaryDate IsNot Nothing Then
                                            txtAnniversaryDate.Text =
                                                CDate(
                                                    oQuote.RenewalDayNo & "/" & oQuote.RenewalDate.Month & "/" &
                                                    oQuote.RenewalDate.Year).ToShortDateString()
                                        End If
                                    End If
                                Else
                                    oQuote.RenewalDayNo = nUnifiedRenewalDay
                                    If _
                                        IsDate(
                                            nUnifiedRenewalDay & "/" & oQuote.RenewalDate.Month & "/" &
                                            oQuote.RenewalDate.Year) = True Then
                                        If txtAnniversaryDate IsNot Nothing Then
                                            txtAnniversaryDate.Text =
                                                CDate(
                                                    nUnifiedRenewalDay & "/" & oQuote.RenewalDate.Month & "/" &
                                                    oQuote.RenewalDate.Year).ToShortDateString()
                                        End If
                                    End If
                                End If
                            End If
                            If txtAnniversaryDate IsNot Nothing Then
                                .AnniversaryDate = CType(txtAnniversaryDate.Text, Date)
                            End If
                        End If
                        If Not oParty Is Nothing AndAlso oProduct.Currencies.Contains(oParty.Currency) Then
                            .CurrencyCode = oParty.Currency
                        Else
                            .CurrencyCode = oProduct.Currencies.Split(",")(0)
                        End If
                        Session(CNCurrenyCode) = .CurrencyCode

                        If _
                            Session(CNBranchCode) IsNot Nothing AndAlso
                            (Not String.IsNullOrEmpty(Convert.ToString(Session(CNBranchCode)))) Then
                            .BranchCode = Convert.ToString(Session(CNBranchCode))
                            .SubBranchCode = Convert.ToString(Session(CNBranchCode))
                        Else
                            .BranchCode = oParty.BranchCode
                            .SubBranchCode = oParty.BranchCode
                        End If

                        If oUserDetails IsNot Nothing Then
                            If oUserDetails.Key > 0 Then
                                .BusinessTypeCode = "AGENCY"
                                Dim oTempParty As NexusProvider.PartyCollection
                                Dim oTempSearchCriteria As New NexusProvider.PartySearchCriteria
                                oTempSearchCriteria.AgentType = Nothing
                                oTempSearchCriteria.Name =
                                    CType(Session(CNAgentDetails), NexusProvider.UserDetails).PartyName
                                oTempSearchCriteria.ShortName =
                                    CType(Session(CNAgentDetails), NexusProvider.UserDetails).PartyCode
                                oTempSearchCriteria.PartyType =
                                    CType(Session(CNAgentDetails), NexusProvider.UserDetails).PartyType
                                oTempSearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.AG)
                                oWebService = New NexusProvider.ProviderManager().Provider
                                oTempParty = oWebService.FindParty(oTempSearchCriteria)

                                If oTempParty IsNot Nothing Then
                                    If oTempParty.Count > 0 Then
                                        .AgentCode = oTempParty(0).UserName
                                        .Agent =
                                            CType(Session(CNAgentDetails), NexusProvider.UserDetails).Key.ToString()
                                    End If
                                End If
                            Else
                                .BusinessTypeCode = "DIRECT"
                            End If
                        Else
                            .BusinessTypeCode = "DIRECT"
                        End If
                        .FrequencyCode = "ANNUAL"
                        .CorrespondenceType = "DEFAULT"
                    End With

                    'count the risk minus IsMandatory=true
                    Dim iRiskCount As Integer = 0
                    For Each oTempRisk As Nexus.Library.Config.RiskType In oProduct.RiskTypes
                        If oTempRisk.IsMandatory = False Then
                            iRiskCount += 1
                        End If
                    Next

                    sScreenCode =
                        GetScreenCode(
                            "~/" & oNexusConfig.ProductsFolder & "/" & oProduct.Name & "/" & oRiskType.Path & "/" &
                            oProduct.FullQuoteConfig)
                    Dim oRisk As New NexusProvider.Risk(sScreenCode, oRiskType.Name)
                    oRisk.DataModelCode = oRiskType.DataModelCode
                    oRisk.RiskCode = oRiskType.RiskCode

                    'if only mandatory is configured
                    If iRiskCount = 0 Then
                        oRisk.IsMandatoryRisk = True
                    End If
                    oQuote.Risks.Add(oRisk)

                    oProduct = Nothing

                    With oQuote
                        .PartyKey = oParty.Key
                    End With

                    Try
                        Session(CNCurrentRiskKey) = 0
                        Session(CNFreshPolicySW) = 1
                        oWebService.AddQuoteV2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode)


                        'need to retreive the latest risk details if only mandatory risk is configured
                        If iRiskCount = 0 Then
                            oWebService.GetHeaderAndRisksByKey(oQuote)
                        End If
                        oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock) 'Exclusive Lock
                        If oOptionSettings.OptionValue = "1" Then
                            oWebService.GetHeaderAndSummariesByKey(v_iInsuranceFileKey:=oQuote.InsuranceFileKey, v_sBranchCode:=oQuote.BranchCode, bExclusiveLock:=True)
                        End If
                        Session.Add(CNQuote, oQuote)
                        GetPreferredCorrespondenceDetails()
                    Finally
                        oWebService = Nothing
                        oRisk = Nothing
                    End Try
                Else
                GetPreferredCorrespondenceDetails()
                    'check we've got a screencode in the risk object, as a call to GetRisk won't retrieve the screencode, we
                    'only want to do this on the first risk screen though, so if we've already got a screencode don't change it
                    If String.IsNullOrEmpty(oQuote.Risks(CInt(Session(CNCurrentRiskKey))).ScreenCode) Then
                        oQuote.Risks(CInt(Session(CNCurrentRiskKey))).ScreenCode = sScreenCode
                    End If
                    Dim objNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), 
                                                                           Config.NexusFrameWork)
                    'Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)

                    If _
                        (oQuote.QuoteStatusKey = QuoteStatusType.AgentPending And oUserDetails.Key = 0 AndAlso
                         Session(CNQuoteMode) = QuoteMode.FullQuote AndAlso Session(CNRenewal) Is Nothing AndAlso
                         Session(CNMTAType) Is Nothing) Then

                        Dim btnDetails As Button =
                                CType(
                                    GetMasterPlaceHolder(Page, objNexusFrameWork.MainContainerName).FindControl(
                                        "btnDetails"), 
                                    Button)
                        If btnDetails IsNot Nothing Then
                            btnDetails.Attributes.Add("onclick", "javascript:return showMessageDetails();")
                        End If

                        Dim btnSaveQuote As Button =
                                CType(
                                    GetMasterPlaceHolder(Page, objNexusFrameWork.MainContainerName).FindControl(
                                        "btnSaveQuote"), 
                                    Button)
                        If btnSaveQuote IsNot Nothing Then
                            btnSaveQuote.Attributes.Add("onclick", "javascript:return showMessageDetails();")
                        End If

                    End If
                End If

                'load controls from XML
                If oOI.Count > 0 Then
                    DataSetFunctions.ReadContainerFromXML(oMaster, Convert.ToString(oOI.Peek), Me)
                Else
                    'To Delete the child added into Dataset Abnormally
                    DataSetFunctions.DeleteElementFromXML(sScreenCode, Nothing, Nothing)
                    DataSetFunctions.ReadContainerFromXML(oMaster, String.Empty, Me)
                End If
            End If
            'Enable/Disable Agent during MTA/MTC as per User Authority.
            If Not Page.IsPostBack Then
                If (Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Or
                    Session(CNMTAType) = MTAType.CANCELLATION) Then
                    'Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oUserAuthority As New NexusProvider.UserAuthority
                    oUserAuthority.UserCode = Convert.ToString(Session(CNLoginName))
                    'Pass Allow Edit Agent during MTA/MTC option
                    oUserAuthority.UserAuthorityOption =
                        NexusProvider.UserAuthority.UserAuthorityOptionType.AgentEditableDuringMTAMTC
                    oWebService.GetUserAuthorityValue(oUserAuthority)
                    If oUserAuthority.UserAuthorityValue = False Then
                        ' Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
                        Dim btnAgentCode As Button =
                                CType(
                                    GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName).FindControl(
                                        "POLICYHEADER__BTNAGENTCODE"), 
                                    Button)
                        Dim txtAgent As HiddenField =
                                CType(
                                    GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName).FindControl(
                                        "POLICYHEADER__AGENT"), 
                                    HiddenField)
                        Dim txtAgentCode As TextBox =
                                CType(
                                    GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName).FindControl(
                                        "POLICYHEADER__AGENTCODE"), 
                                    TextBox)
                        If btnAgentCode IsNot Nothing AndAlso txtAgentCode IsNot Nothing Then
                            btnAgentCode.Enabled = False
                            txtAgentCode.Enabled = False
                        End If
                    End If
                End If

            End If
            If Session(CNRenewal) Then
                Dim oRenewalStatus As NexusProvider.RenewalStatus

                oRenewalStatus = oWebService.GetRenewalStatus(oQuote.InsuranceFileKey)
                oWebService.GetHeaderAndRisksByKey(oQuote)
                Session(CNRenewalStatus) = oRenewalStatus
            End If
            If Request("__EVENTARGUMENT") = "Complete" Then
                CompletePolicy()
            End If
            If Request("__EVENTARGUMENT") = "Delete" Then
                DeletePolicy()
            End If
            If Request("__EVENTARGUMENT") = "RefreshAgent" Then
                FillDefaultCorrespondence()
            End If

        End Sub

        Public Function GetListDescription(ByVal sListType As String, ByVal sListCode As String, ByVal sItemCode As String) As String
            Dim sItemDescription As String = String.Empty

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oList As New NexusProvider.LookupListCollection

            ' sam call to retreive the list of items from user defined list
            If sListType = "UserDefined" Then
                oList = oWebService.GetList(NexusProvider.ListType.UserDefined, sListCode, False, False)
            Else
                oList = oWebService.GetList(NexusProvider.ListType.PMLookup, sListCode, False, False)
            End If

            ' Get code for ID
            For iListCount As Integer = 0 To oList.Count - 1
                If oList(iListCount).Code = sItemCode Then
                    sItemDescription = oList(iListCount).Description.Trim()
                    Exit For
                End If
            Next
            Return sItemDescription
        End Function
        Public Function GetListId(ByVal sListType As String, ByVal sListCode As String, ByVal sItemCode As String) As String
            Dim sItemDescription As String = String.Empty

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oList As New NexusProvider.LookupListCollection

            ' sam call to retreive the list of items from user defined list
            If sListType = "UserDefined" Then
                oList = oWebService.GetList(NexusProvider.ListType.UserDefined, sListCode, False, False)
            Else
                oList = oWebService.GetList(NexusProvider.ListType.PMLookup, sListCode, False, False)
            End If

            ' Get code for ID
            For iListCount As Integer = 0 To oList.Count - 1
                If oList(iListCount).Code = sItemCode Then
                    sItemDescription = oList(iListCount).Key
                    Exit For
                End If
            Next
            Return sItemDescription
        End Function

        Private Sub GetPreferredCorrespondenceDetails()
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oAgentSettings As NexusProvider.AgentSettings = Nothing
            If oQuote IsNot Nothing Then
                If oQuote.BusinessTypeCode = "DIRECT" Then
                    oQuote.IsAgentReceiveCorrespondence = False
                    If oQuote.CorrespondenceType = "DEFAULT" Then
                        oQuote.DefaultPreferredCorrespondence = GetClientDefaultPreferredCorrespondence()
                    End If
                Else
                    If Not String.IsNullOrEmpty(oQuote.Agent) And oQuote.Agent > 0 Then
                        oAgentSettings = oWebService.GetAgentSettings(oQuote.Agent)
                        If oAgentSettings IsNot Nothing Then
                            If oQuote.CorrespondenceType = "DEFAULT" Then
                                If oAgentSettings.IsReceiveClientCorrespondence Then
                                    oQuote.DefaultPreferredCorrespondence = oAgentSettings.CorrespondenceType.Trim.ToUpper
                                    oQuote.IsAgentReceiveCorrespondence = True
                                Else
                                    oQuote.DefaultPreferredCorrespondence = GetClientDefaultPreferredCorrespondence()
                                    oQuote.IsAgentReceiveCorrespondence = False
                                End If
                            End If
                        End If
                    Else
                        oQuote.IsAgentReceiveCorrespondence = False
                        If oQuote.CorrespondenceType = "DEFAULT" Then
                            oQuote.DefaultPreferredCorrespondence = GetClientDefaultPreferredCorrespondence()
                        End If
                    End If
                End If
            End If
        End Sub

        Private Sub FillDefaultCorrespondence()

            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oAgentSettings As NexusProvider.AgentSettings = Nothing

            Dim ddlBusinessType As NexusProvider.LookupList = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__BUSINESSTYPE"), NexusProvider.LookupList)
            Dim ddlCorrespondenceType As NexusProvider.LookupList = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__CORRESPONDENCETYPE"), NexusProvider.LookupList)
            Dim txtAgent As HiddenField = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__AGENT"), HiddenField)
            Dim txtDefaultCorrespondence As TextBox = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE"), TextBox)
            Dim lblCorrespondenceType As Label = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("lblPOLICYHEADER_CORRESPONDENCETYPE"), Label)
            Dim hdnReceiveClientCorrespondence As HiddenField = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__RECEIVESCLIENTCORRESPONDENCE"), HiddenField)
            Dim hdnDefaultCorrespondenceCode As HiddenField = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__DEFAULTCORRESPONDENCECODE"), HiddenField)

            lblCorrespondenceType.Text = "Client Correspondence"
            txtDefaultCorrespondence.Visible = False
            hdnReceiveClientCorrespondence.Value = 0
            If Not String.IsNullOrEmpty(txtAgent.Value) Then
                oAgentSettings = oWebService.GetAgentSettings(txtAgent.Value)
                If oAgentSettings IsNot Nothing Then
                    If oAgentSettings.IsReceiveClientCorrespondence Then
                        lblCorrespondenceType.Text = "Agent Correspondence"
                        hdnReceiveClientCorrespondence.Value = 1
                    Else
                        lblCorrespondenceType.Text = "Client Correspondence"
                    End If

                    If ddlCorrespondenceType IsNot Nothing AndAlso ddlCorrespondenceType.Value IsNot Nothing AndAlso ddlCorrespondenceType.Value.Trim.ToUpper = "DEFAULT" Then
                        If oAgentSettings.IsReceiveClientCorrespondence Then
                            txtDefaultCorrespondence.Visible = True
                            txtDefaultCorrespondence.Text = oAgentSettings.CorrespondenceType.Trim.ToUpper
                        Else
                            txtDefaultCorrespondence.Visible = True
                            txtDefaultCorrespondence.Text = GetClientDefaultPreferredCorrespondence()
                        End If
                    End If
                Else
                    txtDefaultCorrespondence.Text = String.Empty
                End If
            Else
                If ddlCorrespondenceType IsNot Nothing AndAlso ddlCorrespondenceType.Value IsNot Nothing AndAlso ddlCorrespondenceType.Value.Trim.ToUpper = "DEFAULT" Then
                    txtDefaultCorrespondence.Visible = True
                    txtDefaultCorrespondence.Text = GetClientDefaultPreferredCorrespondence()
                End If
                lblCorrespondenceType.Text = "Client Correspondence"
            End If
            If Not String.IsNullOrEmpty(txtDefaultCorrespondence.Text) Then
                hdnDefaultCorrespondenceCode.Value = txtDefaultCorrespondence.Text
                txtDefaultCorrespondence.Text = GetListDescription("PMLookup", "Contact_Type", txtDefaultCorrespondence.Text)
            End If
        End Sub

        Private Function GetClientDefaultPreferredCorrespondence() As String
            Dim oParty As NexusProvider.BaseParty
            Dim sDefaultPreferredCorrespondence As String = String.Empty
            If Session(CNParty) IsNot Nothing Then
                Select Case True
                    Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                        oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                        With CType(oParty, NexusProvider.CorporateParty)
                            sDefaultPreferredCorrespondence = .ClientSharedData.CorrespondenceCode
                        End With
                    Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                        oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                        With CType(oParty, NexusProvider.PersonalParty)
                            sDefaultPreferredCorrespondence = .ClientSharedData.CorrespondenceCode
                        End With
                End Select
            End If
            Return sDefaultPreferredCorrespondence
        End Function

        Private Sub CompletePolicy()
            'Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            'Dim sRedirectPath As String = String.Empty
            'Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            'Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
            'DoQuoteConfirmation(oQuote.InsuranceFileKey)
            DoQuoteConfirmation(Session("TempInsuranceFileKey"))
        End Sub

        Private Sub DeletePolicy()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As New NexusProvider.Quote
            oQuote = Session(CNQuote)
            Dim oQuoteNew As New NexusProvider.Quote
            oQuoteNew.InsuranceFileKey = Session("TempInsuranceFileKey")
            oWebService.DeletePolicy(oQuoteNew)
            CopyQuote()
            oQuote = FillSessionValues(oQuote.InsuranceFileKey)
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Session(CNQuoteMode) = QuoteMode.ReQuote
            'Dim oQuote As NexusProvider.Quote
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name
            Dim oRiskType As Config.RiskType
            Dim oRiskT As New NexusProvider.RiskType


            If oQuote.Risks(0).RiskCode Is Nothing Then
                oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(0).RiskTypeCode.Trim)
            Else
                oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(0).RiskCode)
            End If
            oRiskT.DataModelCode = oRiskType.DataModelCode
            oRiskT.Name = oRiskType.Name
            oRiskT.Path = oRiskType.Path
            oRiskT.RiskCode = oRiskType.RiskCode
            Session(CNRiskType) = oRiskT

            Dim sRiskFolderTemp As String = sProductFolder & "/" & oRiskT.Path & "/"
            Dim sRiskFolderDelete As String = sProductFolder & "/" & oRiskT.Path & "/"

            Dim Doc As XPathDocument = New XPathDocument(Server.MapPath(sRiskFolderTemp & oProduct.FullQuoteConfig))
            Dim Navigator As XPathNavigator
            Navigator = Doc.CreateNavigator()

            Dim i = Navigator.Select("/screens/screen/tab[1]")
            While i.MoveNext()
                If (i.Current.GetAttribute("maindetails", String.Empty)) Then
                    sRiskFolderDelete = sRiskFolderDelete.Substring(0, sRiskFolderDelete.LastIndexOf("/"))
                    sRiskFolderDelete = sRiskFolderDelete.Substring(0, sRiskFolderDelete.LastIndexOf("/")) & "/"
                Else
                    sRiskFolderDelete = sRiskFolderTemp
                End If
            End While


            Dim iGracePeriod As Integer
            Dim dExpiryDate As Date
            If oQuote.QuoteExpiryDate = Date.MinValue Then
                iGracePeriod = IIf(GetQuoteGracePeriod(oQuote.ProductCode.Trim()) = "", 0, GetQuoteGracePeriod(oQuote.ProductCode.Trim()))
                dExpiryDate = oQuote.CoverStartDate.AddDays(iGracePeriod).ToShortDateString()
            Else
                dExpiryDate = oQuote.QuoteExpiryDate
            End If
            Session.Remove(CNTabState & "_" & sTabIndexControlID)
            If (dExpiryDate < DateTime.Now) Then
                Session(CNMode) = Mode.View
                Response.Redirect(sRiskFolderDelete & "/" & GetFirstRiskScreen(sRiskFolderTemp & oProduct.FullQuoteConfig), False)
            ElseIf (oQuote.QuoteStatusKey = QuoteStatusType.Declined) Then
                Session(CNMode) = Mode.View
                Response.Redirect(sRiskFolderDelete & "/" & GetFirstRiskScreen(sRiskFolderTemp & oProduct.FullQuoteConfig), False)
            Else
                Response.Redirect(sRiskFolderDelete & "/" & GetFirstRiskScreen(sRiskFolderTemp & oProduct.FullQuoteConfig), False)
            End If
        End Sub

        Public Sub PreRenderSave(ByVal sender As Object, ByVal e As System.EventArgs)

            If CType(Session(CNMode), Mode) = Mode.View Or CType(Session(CNMode), Mode) = Mode.Review Then
                sender.enabled = False
            Else
                sender.enabled = True
            End If

        End Sub

        ''' <summary>
        ''' Handles the SaveButton event as specified in the risk screen. This needs to be manually
        ''' hooked up, as you could implement the save button via different controls or multiple
        ''' times on a single page.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>Save button will will save the data (XML) to DB</remarks>
        Public Sub SaveButton(ByVal sender As Object, ByVal e As System.EventArgs)
            If Page.IsValid Then
                Session(CNQuoteInSync) = True
                If Session(CNMode) <> Mode.View Then
                    WriteRisk()
                    'empty the Risl level SW session after successfull writing of Risk
                    Session(CNRiskStandardWordingsTemplate) = Nothing
                    Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                    Dim oProduct As Config.Product = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).Products.GetProductByCode(oQuote.ProductCode)
                    If oProduct.AutoSave = False Then
                        SaveQuote()
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Handles the CancelButton event as specified in the child risk screen. This needs to be manually
        ''' hooked up, as you could implement the cancel button via different controls or multiple
        ''' times on a single page.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>Will delete the current child and return to the parent</remarks>
        Public Sub CancelButton(ByVal sender As Object, ByVal e As System.EventArgs)
            If sPrevPage = sParentTab Then

                'To Delete the latest child if user presses CANCEL button
                If Session(CNMode) <> Mode.View And Session(CNMode) <> Mode.Review Then
                    oOI = Session(CNOI)
                    DataSetFunctions.DeleteElementFromXML(sScreenCode, oOI.Peek.ToString(), Nothing)
                    RemoveInvalidNodeFromXML(oOI.Peek.ToString())
                    Session(CNRiskStandardWordingsTemplate) = Nothing
                End If
            End If
            Session(CNQuoteInSync) = True
            'Call and override  this function if we want to Redirect to some other page
            CancelButtonRedirect()
            Response.Redirect(sPrevPage, False)

        End Sub

        ''' <summary>
        ''' This will acutally allow the user to move to the desired page
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub CancelButtonRedirect()

        End Sub

        ''' <summary>
        ''' Handles the BackButton event as specified in the risk screen. This needs to be manually
        ''' hooked up, as you could implement the back button via different controls or multiple
        ''' times on a single page.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>Back button will NOT save the current form data, as it meant to have similiar
        ''' effect as clicking the browser back button, which won't save the current form data.</remarks>
        Public Sub BackButton(ByVal sender As Object, ByVal e As System.EventArgs)

            If sPrevPage = sParentTab Then

                'To Delete the latest child if user presses BACK button
                If Session(CNMode) <> Mode.View And Session(CNMode) <> Mode.Review Then
                    oOI = Session(CNOI)
                    DataSetFunctions.DeleteElementFromXML(sScreenCode, oOI.Peek.ToString(), Nothing)
                    RemoveInvalidNodeFromXML(oOI.Peek.ToString())
                End If
            End If
            'empty the Risl level SW session after successfull writing of Risk
            Session(CNRiskStandardWordingsTemplate) = Nothing
            Session(CNQuoteInSync) = True
            'Call and override  this function if we want to Redirect to some other page
            If Session(CNCurrentOI) IsNot Nothing AndAlso CType(Session(CNMode), Mode) = Mode.Review Then
                Session(CNOI) = Session(CNCurrentOI)
            End If
            BackButtonRedirect()
            Response.Redirect(sPrevPage, False)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CheckDuplicateClient() As Boolean
            If Session(CNRiskDuplicateClientCheck) IsNot Nothing Then
                If CType(Session(CNRiskDuplicateClientCheck), Boolean) = True Then
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                    Session.Add(CNIsTransferQuoteRequired, "True")
                    Session.Add(CNRiskDuplicateClientCheck, "False")
                    Return True
                Else
                    Session.Remove(CNRiskDuplicateClientCheck)
                    Return False
                End If
            End If
        End Function
        ''' <summary>
        ''' Handles the Next button OnClick event from the risk page. This needs to be hooked up manually
        ''' in the risk page as you could implement the back button via different controls or multiple
        ''' times on a single page.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>Current form data will be saved.</remarks>
        Public Sub NextButton(ByVal sender As Object, ByVal e As System.EventArgs)

            If Page.IsValid Then
                Dim bDuplicateClientCheck As Boolean = False
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim sURLNew As String = String.Empty
                Session(CNQuoteInSync) = True

                If Session(CNMode) <> Mode.View And CType(Session(CNMode), Mode) <> Mode.Review Then
                    WriteRisk()
                    ' Check DuplicateClient Response
                    bDuplicateClientCheck = CheckDuplicateClient()
                End If
                'empty the Risl level SW session after successfull writing of Risk
                Session(CNRiskStandardWordingsTemplate) = Nothing
                Session.Remove(CNDeletedNode)
                If (bDuplicateClientCheck = False Or (CType(Session(CNIsTransferQuoteRequired), Boolean) = True And Session(CNPartyKey) IsNot Nothing)) Then
                    If (oQuote.BusinessTypeCode.Trim.ToUpper = "COIN LEAD" OrElse oQuote.BusinessTypeCode.Trim.ToUpper = "COIN FOLL") And Session(CNCoInsurancePage) Is Nothing Then
                        Session("NextPage") = sNextPage
                        Response.Redirect("~/secure/CoinsuranceDetails.aspx", False)
                    ElseIf Session(CNQuoteMode) = QuoteMode.ReQuote Then
                        Session(CNQuoteMode) = QuoteMode.FullQuote
                        UpdateQuote()
                    Else

                        If String.IsNullOrEmpty(sNextPage) Then
                            If CType(Session(CNMode), Mode) = Mode.Review Then
                                Session(CNQuote) = Nothing
                                Session(CNQuoteMode) = Nothing
                                Session(CNRiskType) = Nothing
                                Session(CNQuoteInSync) = Nothing
                                Session(CNOI) = Session(CNCurrentOI)
                                Session(CNMode) = Session(CNCurrentMode)
                                Response.Redirect(Session(CNReturnURL), True)
                            End If
                        End If

                        'If String.IsNullOrEmpty(sNextPage) Then
                        '    sNextPage = ""
                        'End If
                        If String.IsNullOrEmpty(sNextPage) OrElse sNextPage = sParentTab OrElse InStr(sNextPage.ToLower, "summaryofcover") > 0 OrElse InStr(sNextPage.ToLower, "summaryofrisk") > 0 OrElse Convert.ToBoolean(Session("IsFromSummary")) Then
                            Session.Item(CNTempOI) = Nothing
                            Dim sDatasetErrorMessages As String = String.Empty

                            If Session(CNMode) <> Mode.View And CType(Session(CNMode), Mode) <> Mode.Review Then
                                sDatasetErrorMessages = ValidateDataset()
                            End If

                            If sDatasetErrorMessages <> String.Empty Then
                                'create a new custom validator
                                'set it as invalid and set the error message property to the output from ValidateDataset
                                Dim cstInvalidDataset As New CustomValidator
                                cstInvalidDataset.IsValid = False
                                cstInvalidDataset.ErrorMessage = sDatasetErrorMessages
                                cstInvalidDataset.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                                Page.Validators.Add(cstInvalidDataset)
                            End If

                            If sDatasetErrorMessages = String.Empty Then

                                If iDepth > 1 Then

                                    'Removal of USed OI from Hash Table
                                    If Session(CNMode) <> Mode.View And Session(CNMode) <> Mode.Review Then
                                        Dim oUsedOI As Collections.Stack = Session(CNOI)
                                        If Session(CNNode) IsNot Nothing Then
                                            Dim hCurrentNodeColl As New Hashtable()
                                            hCurrentNodeColl = CType(Session(CNNode), Hashtable)

                                            If hCurrentNodeColl.ContainsKey(oUsedOI.Peek().ToString) Then
                                                hCurrentNodeColl.Remove(oUsedOI.Peek().ToString)
                                                Session(CNNode) = hCurrentNodeColl
                                            End If
                                        End If

                                        'Delete the usedOI from DeletedOI collection
                                        If Session(CNDeletedNode) IsNot Nothing Then
                                            Dim hDeletedNodeColl As New Hashtable()
                                            hDeletedNodeColl = CType(Session(CNDeletedNode), Hashtable)

                                            If hDeletedNodeColl.ContainsKey(oUsedOI.Peek().ToString) Then
                                                hDeletedNodeColl.Remove(oUsedOI.Peek().ToString)
                                                Session(CNDeletedNode) = hDeletedNodeColl
                                            End If
                                        End If

                                        'Remove the UC attrinbute to identify the valid/invalid node
                                        ResetUCElement(oUsedOI.Peek().ToString)
                                    End If

                                    'call PrePageRedirect before we redirect to allow this to be overridden if required
                                    PrePageRedirect()
                                    If HttpContext.Current.Session.IsCookieless Then
                                        sURLNew = sNextPage
                                        Dim iIndex As Integer = sURLNew.IndexOf(AppSettings("WebRoot"))
                                        iIndex = iIndex + Convert.ToInt16(Convert.ToString(AppSettings("WebRoot")).Length)
                                        sURLNew = sURLNew.Insert(iIndex, "(S(" & Session.SessionID.ToString() + "))/")
                                        Response.Redirect(sURLNew)
                                    Else
                                        Response.Redirect(sNextPage)
                                    End If

                                Else
                                    If Session(CNMode) <> Mode.View And CType(Session(CNMode), Mode) <> Mode.Review Then
                                        If Page.IsValid Then
                                            UpdateQuote()
                                        End If
                                    ElseIf Session(CNMode) = Mode.View Or CType(Session(CNMode), Mode) = Mode.Review Then
                                        Response.Redirect("~/secure/RiskDetails.aspx")
                                    Else
                                        If sSummaryOfCover.ToLower = "true" Then
                                            Response.Redirect(ResolveUrl(sSummaryOfCoverURL))
                                        Else
                                            Response.Redirect("~/secure/Premiumdisplay.aspx")
                                        End If
                                    End If

                                End If
                            End If
                        Else
                            If String.IsNullOrEmpty(sNextPage) Then
                                If CType(Session(CNMode), Mode) = Mode.Review Then
                                    Session(CNOI) = Session(CNCurrentOI)
                                End If
                            End If
                            'redirect the page if its required
                            PrePageRedirect()
                            If HttpContext.Current.Session.IsCookieless Then
                                sURLNew = sNextPage
                                Dim iIndex As Integer = sURLNew.IndexOf(AppSettings("WebRoot"))
                                iIndex = iIndex + Convert.ToInt16(Convert.ToString(AppSettings("WebRoot")).Length)
                                sURLNew = sURLNew.Insert(iIndex, "(S(" & Session.SessionID.ToString() + "))/")
                                Response.Redirect(sURLNew)
                            Else
                                Response.Redirect(sNextPage)
                            End If
                        End If
                    End If
                Else
                    Dim sURL As String
                    If HttpContext.Current.Session.IsCookieless Then
                        sURL = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/DuplicateClients.aspx?modal=true&Riskcheck=true&KeepThis=true&TB_iframe=true&height=500&width=750"
                    Else
                        sURL = AppSettings("WebRoot") & "/Modal/DuplicateClients.aspx?modal=true&Riskcheck=true&KeepThis=true&TB_iframe=true&height=500&width=750"
                    End If
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show", _
                    "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sURL & "' , null);});</script>")


                End If
            End If
            Session(CNNEXTBUTTON) = "TRUE"
        End Sub

        ''' <summary>
        ''' Handles the Calculate button OnClick event from the risk page. 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>return PRE rating.</remarks>

        Public Sub CalculatePreButton(ByVal sender As Object, ByVal e As System.EventArgs)
            If Page.IsValid Then
                If CType(Session(CNMode), Mode) = Mode.Review Then
                    Session(CNQuote) = Nothing
                    Session(CNQuoteMode) = Nothing
                    Session(CNRiskType) = Nothing
                    Session(CNQuoteInSync) = Nothing
                    Session(CNOI) = Session(CNCurrentOI)
                    Session(CNMode) = Session(CNCurrentMode)
                    Response.Redirect(Session(CNReturnURL), False)
                End If
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)

                Session(CNQuoteInSync) = True

                If Session(CNMode) <> Mode.View And CType(Session(CNMode), Mode) <> Mode.Review Then
                    WriteRisk()
                End If

                Dim sDatasetErrorMessages As String = String.Empty

                If Session(CNMode) <> Mode.View And CType(Session(CNMode), Mode) <> Mode.Review Then
                    sDatasetErrorMessages = ValidateDataset() 'need to validate the
                End If

                If sDatasetErrorMessages <> String.Empty Then
                    'create a new custom validator
                    'set it as invalid and set the error message property to the output from ValidateDataset
                    Dim cstInvalidDataset As New CustomValidator
                    cstInvalidDataset.IsValid = False
                    cstInvalidDataset.ErrorMessage = sDatasetErrorMessages
                    cstInvalidDataset.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                    Page.Validators.Add(cstInvalidDataset)
                End If

                CallPRE()
                Response.Redirect(Session(CNReturnURL), False)

            End If
        End Sub

        Protected Function GetTransactionType() As String
            If String.IsNullOrEmpty(Session(CNMTAType)) = False Then

                If Session(CNMTAType) = MTAType.CANCELLATION Then
                    Return "MTC"
                End If

                If Session(CNMTAType) = MTAType.REINSTATEMENT Then
                    Return "MTR"
                End If

                If Session(CNMTAType) = MTAType.TEMPORARY Then
                    Return "MTATEMP"
                End If

                Return "MTA"
            End If

            '	Renewal
            If String.IsNullOrEmpty(Session(CNRenewal)) = False Then
                Return "REN"
            End If

            Return CType(Session.Item(Nexus.Constants.Session.CNQuote), NexusProvider.Quote).TransactionType.ToString()
        End Function
        Private Sub CallPRE()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim sTransactionType As String = GetTransactionType()
            Try

                oWebService.ExecutePRERuleset(oQuote, CInt(Session(CNCurrentRiskKey)), Nothing, Nothing, sTransactionType, False, "", False, False)

                Dim doc = LoadDocument(oQuote)
                oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = doc.OuterXml.ToString

                Session(CNQuote) = oQuote

            Catch ex As System.Exception

            Finally
                oQuote = Nothing
                oWebService = Nothing
            End Try
        End Sub

        Private Function LoadDocument(ByRef quote As NexusProvider.Quote) As XmlDocument
            Dim srDataset As New System.IO.StringReader(CType(quote.Risks(Session(CNCurrentRiskKey)).XMLDataset, String))
            Dim xmlTextReader As New XmlTextReader(srDataset)
            Dim doc As New XmlDocument
            doc.Load(xmlTextReader)
            xmlTextReader.Close()
            Return doc
        End Function

        ''' <summary>
        ''' Add the UC attrinbute to identify the valid/invalid node
        ''' </summary>
        ''' <param name="v_sOI"></param>
        ''' <remarks></remarks>
        Sub AddUCElement(ByVal v_sOI As String)
            'Add the UC attrinbute to identify the valid/invalid node
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oProduct As Config.Product = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products.GetProductByCode(oQuote.ProductCode)
            Dim srDataset As New System.IO.StringReader(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
            Dim xmlTR As New XmlTextReader(srDataset)
            Dim Doc As New XmlDocument

            Doc.Load(xmlTR)
            xmlTR.Close()

            'Dim oDefaultNodes As New Hashtable()
            Dim oNode As XmlNode = Doc.SelectSingleNode("//*[@OI = '" & v_sOI & "']")

            'Add the UC attrinbute to identify the valid/invalid node
            Dim xUCAttr As XmlAttribute
            xUCAttr = oNode.Attributes("UC")
            If xUCAttr IsNot Nothing Then
                xUCAttr.Value = "1"
            Else
                xUCAttr = oNode.OwnerDocument.CreateAttribute("UC")
                oNode.Attributes.Append(xUCAttr)
                oNode.Attributes("UC").Value = "1"
            End If

            Using swContent As New System.IO.StringWriter
                Using xmlwContent As New XmlTextWriter(swContent)

                    Doc.WriteTo(xmlwContent)
                    oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = swContent.ToString()

                End Using
            End Using

            Session(CNQuote) = oQuote
        End Sub

        ''' <summary>
        ''' Remove the UC attrinbute to identify the valid/invalid node
        ''' </summary>
        ''' <param name="v_sOI"></param>
        ''' <remarks></remarks>
        Sub ResetUCElement(ByVal v_sOI As String)
            'Remove the UC attrinbute to identify the valid/invalid node
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oProduct As Config.Product = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products.GetProductByCode(oQuote.ProductCode)
            Dim srDataset As New System.IO.StringReader(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
            Dim xmlTR As New XmlTextReader(srDataset)
            Dim Doc As New XmlDocument

            Doc.Load(xmlTR)
            xmlTR.Close()

            'Dim oDefaultNodes As New Hashtable()
            Dim oNode As XmlNode = Doc.SelectSingleNode("//*[@OI = '" & v_sOI & "']")

            Dim xUCAttr As XmlAttribute
            xUCAttr = oNode.Attributes("UC")
            If xUCAttr IsNot Nothing Then
                oNode.Attributes.Remove(xUCAttr)
            End If

            Using swContent As New System.IO.StringWriter
                Using xmlwContent As New XmlTextWriter(swContent)

                    Doc.WriteTo(xmlwContent)
                    oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = swContent.ToString()
                End Using
            End Using

            Session(CNQuote) = oQuote
        End Sub
        ''' <summary>
        ''' Handles the Save button from a RiskContainer control, again this needs to be manually
        ''' hooked upto the OnClick event of the button to allow flexibility of the calling control.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Sub SaveChildButton(ByVal sender As Object, ByVal e As System.EventArgs)
            If Page.IsValid Then
                If CType(Session.Item(CNMode), Mode) <> Mode.View And _
                  CType(Session.Item(CNMode), Mode) <> Mode.Authorise And _
                  CType(Session.Item(CNMode), Mode) <> Mode.Recommend And _
                  CType(Session.Item(CNMode), Mode) <> Mode.ViewClaimPayment And _
                  CType(Session.Item(CNMode), Mode) <> Mode.DeclinePayment Then
                    Dim oRiskContainer As RiskContainer = sender.Parent
                    sScreenCode = oRiskContainer.ScreenCode
                    If oRiskContainer IsNot Nothing Then
                        Dim htOI As Collections.Stack = Session(CNOI)

                        If oRiskContainer.Mode = RiskContainer.ChildMode.Add Then

                            oRiskContainer.OI = DataSetFunctions.CreateElementFromXML(oRiskContainer.ScreenCode, _
                                oOI.Peek, oRiskContainer.ParentElement, oRiskContainer.ChildElement)

                        End If

                        'ADD CHILD CONTROL VALUES TO XML DATASET
                        WriteContainerToXML(oMaster, oRiskContainer.ScreenCode, oOI.Peek)
                        WriteContainerToXML(oRiskContainer, oRiskContainer.ScreenCode, oRiskContainer.OI)

                        'Save the Data on Click of the every Addition of the child
                        'To Save the child data of Inline itemgrid with saverisk method
                        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                        Dim srDataset As New System.IO.StringReader(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
                        Dim xmlTR As New XmlTextReader(srDataset)
                        Dim oDoc As New XmlDocument
                        Dim oProduct As Config.Product = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products.GetProductByCode(oQuote.ProductCode)

                        oDoc.Load(xmlTR)
                        xmlTR.Close()

                        Dim oNode As XmlNode = oDoc.SelectSingleNode("//*[@OI = '" & oRiskContainer.OI & "']")
                        If oNode IsNot Nothing AndAlso oNode.Attributes("US").Value = "3" Then
                            oNode.Attributes("US").Value = "1"
                        End If

                        Dim tempswContent As New System.IO.StringWriter
                        Dim tempxmlwContent As New XmlTextWriter(tempswContent)

                        oDoc.WriteTo(tempxmlwContent)
                        oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = tempswContent.ToString()

                        tempxmlwContent.Close()
                        tempswContent.Close()

                        Session(CNQuote) = oQuote

                        If oProduct.AutoSave Then
                            SaveQuote()
                        End If

                        'Start - Write Parent screen data in XML before postback but need to be saved only on Next click
                        Dim sOI As String = Nothing
                        PreDataSetWrite()
                        If oOI.Count > 0 Then
                            sOI = oOI.Peek.ToString()
                        End If
                        WriteContainerToXML(oMaster, sScreenCode, sOI, False, sParentTab)
                        PostDataSetWrite()
                        'End - Write Parent screen data in XML before postback but need to be saved only on Next click

                    End If

                    Dim sDatasetErrorMessages As String = ValidateDataset() 'need to validate the 
                    If sDatasetErrorMessages <> String.Empty Then
                        'create a new custom validator
                        'set it as invalid and set the error message property to the output from ValidateDataset
                        Dim cstInvalidDataset As New CustomValidator
                        cstInvalidDataset.IsValid = False
                        cstInvalidDataset.ErrorMessage = sDatasetErrorMessages
                        cstInvalidDataset.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                        Page.Validators.Add(cstInvalidDataset)

                        DataSetFunctions.DeleteElementFromXML(oRiskContainer.ScreenCode, oRiskContainer.OI, oRiskContainer.ChildElement)
                        oRiskContainer.Mode = RiskContainer.ChildMode.Add
                    Else
                        ResetUCElement(oRiskContainer.OI)
                        'RESET CHILD CONTROL
                        FrameWorkFunctions.ResetControls(oRiskContainer)
                        oRiskContainer.Mode = RiskContainer.ChildMode.Add
                        'RELOAD EDITED CHILD SCREEN VALUES IN ITEMGRID
                        DataSetFunctions.ReadContainerFromXML(oMaster, oOI.Peek, Me)
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Event to handle the cancelling of an edit/add on a child item with the RiskContainer control,
        ''' needs to be manually hooked upto the OnClick event of the calling control.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Sub CancelChildButton(ByVal sender As Object, ByVal e As System.EventArgs)

            Dim oRiskContainer As RiskContainer = sender.Parent

            If oRiskContainer IsNot Nothing Then

                'Remove the invalid child node
                RemoveInvalidNodeFromXML(oRiskContainer.OI)

                'RESET CHILD CONTROL
                FrameWorkFunctions.ResetControls(oRiskContainer)

                oRiskContainer.Mode = RiskContainer.ChildMode.Add
                'RELOAD EDITED CHILD SCREEN VALUES IN ITEMGRID
                DataSetFunctions.ReadContainerFromXML(oMaster, oOI.Peek, Me)
            End If

        End Sub

        ''' <summary>
        ''' Handles the OnClick event of a Finish button on the risk page, which cause the risk
        ''' process to exit and redirect to the summary of cover page. Needs to be manually
        ''' hooked up to a control on the risk page
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Sub FinishButton(ByVal sender As Object, ByVal e As System.EventArgs)
            If Page.IsValid Then
                Dim bDuplicateClientCheck As Boolean = False
                If CType(Session(CNMode), Mode) = Mode.Review Then
                    Session(CNQuote) = Nothing
                    Session(CNQuoteMode) = Nothing
                    Session(CNRiskType) = Nothing
                    Session(CNQuoteInSync) = Nothing
                    Session(CNOI) = Session(CNCurrentOI)
                    Session("FromRiskPage") = True
                    Session(CNMode) = Session(CNCurrentMode)
                    'If we are coming here from claim pages, then there can be more than 1 URLs(; separated) in return URL session 
                    'In this case we should redirect the user to last visited page only otherwise it will throw an error
                    If Session(CNReturnURL) IsNot Nothing Then
                        Dim aReturnURL() As String = Session(CNReturnURL).Split(";")
                        Dim sReturnURL As String
                        Dim sRedirectURL As String
                        If aReturnURL.Length > 1 Then
                            sReturnURL = aReturnURL(aReturnURL.Length - 1)
                            sRedirectURL = aReturnURL(aReturnURL.Length - 1)
                        Else
                            sReturnURL = aReturnURL(0)
                            sRedirectURL = aReturnURL(0)
                        End If
                        Session(CNReturnURL) = sReturnURL
                    End If
                    Response.Redirect(Session(CNReturnURL))
                End If
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)

                Session(CNQuoteInSync) = True

                If Session(CNMode) <> Mode.View And CType(Session(CNMode), Mode) <> Mode.Review Then
                    WriteRisk()
                    'Check DuplicateClient Response
                    bDuplicateClientCheck = CheckDuplicateClient()
                End If
                'empty the Risl level SW session after successfull writing of Risk
                Session(CNRiskStandardWordingsTemplate) = Nothing
                Session.Remove(CNDeletedNode)
                If (bDuplicateClientCheck = False Or (CType(Session(CNIsTransferQuoteRequired), Boolean) = True And Session(CNPartyKey) IsNot Nothing)) Then
                    Dim sDatasetErrorMessages As String = String.Empty

                    If Session(CNMode) <> Mode.View And CType(Session(CNMode), Mode) <> Mode.Review Then
                        sDatasetErrorMessages = ValidateDataset() 'need to validate the
                    End If

                    If sDatasetErrorMessages <> String.Empty Then
                        'create a new custom validator
                        'set it as invalid and set the error message property to the output from ValidateDataset
                        Dim cstInvalidDataset As New CustomValidator
                        cstInvalidDataset.IsValid = False
                        cstInvalidDataset.ErrorMessage = sDatasetErrorMessages
                        cstInvalidDataset.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                        Page.Validators.Add(cstInvalidDataset)
                    End If

                    If sDatasetErrorMessages = String.Empty Then
                        'no error messages returned so call UpdateQuote
                        If Not String.IsNullOrEmpty(sNextPage) And iDepth > 1 Then
                            Session.Item(CNTempOI) = Nothing
                            'Removal of USed OI from Hash Table
                            If Session(CNMode) <> Mode.View And Session(CNMode) <> Mode.Review Then
                                Dim oUsedOI As Collections.Stack = Session(CNOI)
                                If Session(CNNode) IsNot Nothing Then
                                    Dim hCurrentNodeColl As New Hashtable()
                                    hCurrentNodeColl = CType(Session(CNNode), Hashtable)

                                    If hCurrentNodeColl.ContainsKey(oUsedOI.Peek().ToString) Then
                                        hCurrentNodeColl.Remove(oUsedOI.Peek().ToString)
                                        Session(CNNode) = hCurrentNodeColl
                                    End If
                                End If

                                'Delete the usedOI from DeletedOI collection
                                If Session(CNDeletedNode) IsNot Nothing Then
                                    Dim hDeletedNodeColl As New Hashtable()
                                    hDeletedNodeColl = CType(Session(CNDeletedNode), Hashtable)

                                    If hDeletedNodeColl.ContainsKey(oUsedOI.Peek().ToString) Then
                                        hDeletedNodeColl.Remove(oUsedOI.Peek().ToString)
                                        Session(CNDeletedNode) = hDeletedNodeColl
                                    End If
                                End If

                                'Remove the UC attrinbute to identify the valid/invalid node
                                ResetUCElement(oUsedOI.Peek().ToString)
                            End If

                            'call PrePageRedirect before we redirect to allow this to be overridden if required
                            PrePageRedirect()
                            Response.Redirect(sParentTab, False)
                        Else
                            If Session(CNMode) <> Mode.View And CType(Session(CNMode), Mode) <> Mode.Review Then
                                UpdateQuote()
                            ElseIf sSummaryOfRisk.ToLower = "true" Then
                                Response.Redirect(ResolveUrl(sSummaryOfRiskURL), False)
                            ElseIf sSummaryOfCover.ToLower = "true" Then
                                Response.Redirect(ResolveUrl(sSummaryOfCoverURL), False)
                            Else
                                Response.Redirect("~/secure/RiskDetails.aspx", False)
                            End If
                        End If
                    End If
                Else
                    'Check Duplicate Client Exist
                    Dim sURL As String
                    If HttpContext.Current.Session.IsCookieless Then
                        sURL = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/DuplicateClients.aspx?modal=true&Riskcheck=true&KeepThis=true&TB_iframe=true&height=500&width=750"
                    Else
                        sURL = AppSettings("WebRoot") & "/Modal/DuplicateClients.aspx?modal=true&Riskcheck=true&KeepThis=true&TB_iframe=true&height=500&width=750"
                    End If

                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show",
                    "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sURL & "' , null);});</script>")
                End If
            End If
        End Sub
        ''' <summary>
        ''' This method will hide/show the MakeLive button
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Sub PreRenderMakeLive(ByVal sender As Object, ByVal e As System.EventArgs)
            'This button will be hide in case of the view mode
            If CType(Session(CNMode), Mode) = Mode.View Or CType(Session(CNMode), Mode) = Mode.ViewClaim _
            Or CType(Session(CNMode), Mode) = Mode.Review Then
                sender.enabled = False
            Else
                sender.enabled = True
            End If

        End Sub
        ''' <summary>
        ''' Handles the OnClick event of a MakeLive button on the risk page, which cause the risk
        ''' process to exit and redirect to the transaction confirmation page. Needs to be manually
        ''' hooked up to a control on the risk page
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Sub MakeLiveButton(ByVal sender As Object, ByVal e As System.EventArgs)
            If Page.IsValid Then
                'if mode is review 
                If CType(Session(CNMode), Mode) = Mode.Review Then
                    Session(CNQuote) = Nothing
                    Session(CNQuoteMode) = Nothing
                    Session(CNRiskType) = Nothing
                    Session(CNQuoteInSync) = Nothing
                    Session(CNOI) = Session(CNCurrentOI)
                    Session(CNMode) = Session(CNCurrentMode)
                    Response.Redirect(Session(CNReturnURL), False)
                End If
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)

                Session(CNQuoteInSync) = True

                If Session(CNMode) <> Mode.View Then
                    WriteRisk()
                End If

                Dim sDatasetErrorMessages As String = ValidateDataset() 'need to validate the 

                If sDatasetErrorMessages <> String.Empty Then
                    'create a new custom validator
                    'set it as invalid and set the error message property to the output from ValidateDataset
                    Dim cstInvalidDataset As New CustomValidator
                    cstInvalidDataset.IsValid = False
                    cstInvalidDataset.ErrorMessage = sDatasetErrorMessages
                    cstInvalidDataset.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                    Page.Validators.Add(cstInvalidDataset)
                End If
                'if it si empty then their is no error so fire the UpdateQuote method
                If sDatasetErrorMessages = String.Empty Then
                    If Session(CNMode) <> Mode.View Then
                        'True is passed beacuse this method is called from the MakeLive Button
                        UpdateQuote(True)
                    End If
                End If
            End If
        End Sub


        Protected Function ValidateDataset() As String
            'Code for Running XSLT Validation
            'Declaration of the Vairables used
            Dim oQuote As NexusProvider.Quote = Session(CNQuote) ' for taking the xml from session
            Dim sbOutput As New StringBuilder
            Dim xmlTR As New XmlTextReader(New System.IO.StringReader(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)) ' xml from session
            Dim xInput As New XmlDocument
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name

            Dim SValidatorPath As String = Server.MapPath(sProductFolder) & "\" & sScreenCode & ".xslt"

            'Dim SValidatorPath As String = Server.MapPath(sScreenCode & ".xslt")
            'A check for validation file exist
            If (System.IO.File.Exists(SValidatorPath)) Then

                xInput.Load(xmlTR)
                Dim xslDoc As New Xsl.XslCompiledTransform 'This should load the relevant validator file from the current product folder
                xslDoc.Load(SValidatorPath)
                Using stream As New System.IO.MemoryStream()
                    Using xwOutput As New XmlTextWriter(stream, System.Text.Encoding.UTF8)
                        'transform xInput with xslDoc to create xOutput
                        xslDoc.Transform(xInput, xwOutput)

                        Dim xOutput As New XmlDocument
                        'reset stream to begining
                        stream.Position = 0
                        xOutput.Load(stream)

                        Dim nodes As XmlNodeList = xOutput.GetElementsByTagName("ValidationFailure")

                        If nodes.Count <> 0 Then 'no failures so return an empty string

                            'create the error message
                            Dim node As XmlNode

                            For Each node In nodes
                                sbOutput.Append(node.InnerText)
                                If node.NextSibling IsNot Nothing Then
                                    ' add html tags 
                                    ' the error message will already be placed in <li></li> tags 
                                    'so we just need to close and reopen the <li> tag if there is another error message to follow
                                    sbOutput.Append("</li><li>")
                                End If

                            Next
                            'return the error message as as string
                            Return sbOutput.ToString

                        End If
                    End Using
                End Using
            End If
            xmlTR.Close()
            'Code for Running SAM Validation Rules

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim v_sXMLDataSet As String
            Dim strValidationMsg As String = String.Empty

            Dim sProductPath() As String = CStr(Request.ApplicationPath & "/" & oNexusConfig.ProductsFolder) _
                        .Split(Regex.Split("/", ""), StringSplitOptions.RemoveEmptyEntries)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
            Dim oProductConfig As Nexus.Library.Config.Product = oPortal.Products.GetProductByName(Server.UrlDecode( _
              Request.Url.Segments(sProductPath.Length + 1).TrimEnd("/")))

            v_sXMLDataSet = oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset
            'new config section added to web.config directing if validation rules exist or not.
            'avoids unnecessary SAM call if no validation rules are present.
            If oProductConfig.RiskTypes.RiskType(0).ValidationEnabled = "True" Then

                Try

                    If Session(CNMTAType) IsNot Nothing And Session(CNRenewal) Is Nothing Then
                        If Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Then
                            oWebService.RunValidationRules(sScreenCode, v_sXMLDataSet, Nothing, Nothing, Nothing, "MTA", True)
                        ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                            oWebService.RunValidationRules(sScreenCode, v_sXMLDataSet, Nothing, Nothing, Nothing, "MTC", True)
                        ElseIf (Session(CNMTAType) = MTAType.REINSTATEMENT) Then
                            oWebService.RunValidationRules(sScreenCode, v_sXMLDataSet, Nothing, Nothing, Nothing, "MTR", True)
                        End If
                    ElseIf Session(CNMTAType) Is Nothing And Session(CNRenewal) IsNot Nothing Then
                        oWebService.RunValidationRules(sScreenCode, v_sXMLDataSet, Nothing, Nothing, Nothing, "REN", True)
                    Else
                        oWebService.RunValidationRules(sScreenCode, v_sXMLDataSet, Nothing, Nothing, Nothing, "NB", True)
                    End If
                    oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = v_sXMLDataSet
                    Session(CNQuote) = oQuote
                Catch ex As Exception
                    Throw New Exception(ex.Message.ToString())
                End Try

                Dim srDataset As New System.IO.StringReader(v_sXMLDataSet)
                Dim xmlTRNew As New XmlTextReader(srDataset)
                Dim Doc As New XmlDocument

                Doc.Load(xmlTRNew)
                xmlTRNew.Close()

                Dim oNodes As XmlNodeList = Doc.SelectNodes("//" & Session.Item(CNDataModelCode).ToString() & "_OUTPUT[@REFER_REASON]")

                Dim oNode As XmlNode

                For Each oNode In oNodes
                    If oNode.Attributes("REFER_REASON").Value.Trim() <> "" Then
                        sbOutput.Append(oNode.Attributes("REFER_REASON").Value)
                        If oNode.NextSibling IsNot Nothing Then
                            sbOutput.Append("</li><li>")
                        End If
                    End If
                Next

                oNodes = Doc.SelectNodes("//" & Session.Item(CNDataModelCode).ToString() & "_OUTPUT[@DECLINE_REASON]")
                For Each oNode In oNodes
                    If oNode.Attributes("DECLINE_REASON").Value.Trim() <> "" Then
                        sbOutput.Append(oNode.Attributes("DECLINE_REASON").Value)
                        If oNode.NextSibling IsNot Nothing Then
                            sbOutput.Append("</li><li>")
                        End If
                    End If
                Next

                strValidationMsg = sbOutput.ToString()

                srDataset.Dispose()
                Return strValidationMsg
            Else
                Return ""
            End If
        End Function


        ''' <summary>
        ''' Allows the Finish button to be hidden from the user when they are entering the
        ''' details of a new risk, as at this point to must complete all pages and can not
        ''' skip to the end of the process. This event needs to manually hooked up to
        ''' the PreRender event of the Finish button.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>Using PreRender to overide any visibility set with the risk screen,
        ''' this should ensure the button can not be displayed when it shouldn't be.
        ''' we've conig option product level called "SubmitFromAnyPage".
        ''' If set to true then the finish button should not be hidden on the risk screens at any time
        ''' if the finish button is added to the page then it should always be visible</remarks>
        Public Sub PreRenderFinish(ByVal sender As Object, ByVal e As System.EventArgs)

            Dim sProductCode As String = CType(Session(CNQuote), NexusProvider.Quote).ProductCode ' get the product code from session
            Dim bSubmitFromAnyPage As Boolean = oNexusConfig.Portals.Portal(Portal.GetPortalID()).Products.Product(sProductCode).SubmitFromAnyPage
            If bSubmitFromAnyPage Then
                sender.visible = True
            Else
                sender.visible = False
            End If

        End Sub

        ''' <summary>
        ''' Writes the current dataset in session back to SAM, handles the status response of the quote
        ''' e.g referred, declined and redirects to the appropriate page.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub UpdateQuote(Optional ByVal bMakeLive As Boolean = False)

            'Update Quote
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)

            'need to update the quote if user has selected currency using Currencies Control
            If Session(CNCurrenyCode) Is Nothing Then
                Session(CNCurrenyCode) = CType(GetSection("NexusFrameWork"),  _
                        Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode).Currencies.ToString
            End If
            ' oQuote.CurrencyCode = Session(CNCurrenyCode)

            'Remove the PartySummary cache as we are adding or updating the quote for the client
            Cache.Remove("PartySummary_" & oQuote.PartyKey.ToString())

            'this should not allow to update quote when Process is MTA
            'If Session(CNMTAType) = Nothing Then 'Needs to call as we are changing details on MainDetail Screen
            Try
                Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)

                'if user has not supplied the Quote Expiry date in case of Renewal
                If (Session(CNRenewal) IsNot Nothing Or Session(CNIsBackDatedMTA) = True) AndAlso oQuote.QuoteExpiryDate = Date.MinValue Then
                    oQuote.QuoteExpiryDate = oQuote.CoverStartDate.AddDays(1)
                ElseIf Session(CNMTAType) IsNot Nothing Then
                    Dim iGracePeriod As Integer = 0
                    Dim oRiskTypes As NexusProvider.RiskType = Session(CNRiskType)
                    iGracePeriod = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.GracePeriod, NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, oRiskTypes.RiskCode)
                    oQuote.QuoteExpiryDate = CDate(DateTime.Now).AddDays(iGracePeriod).ToShortDateString() ' .QuoteExpiryDate
                End If

                If oUserDetails IsNot Nothing AndAlso oUserDetails.PartyType <> NexusProvider.PartyTypeType.OTTHIRD.ToString() Then
                    oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode, oUserDetails.Key)
                Else
                    oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode)
                End If

            Finally
                oWebService = Nothing
            End Try

            If Session(CNQuoteMode) = QuoteMode.FullQuote And IsDataSetNexusQuoteStatus(Session(CNCurrentRiskKey)) Then
                UpdateNexusQuoteStatus()
            End If

            ' START - DONE AGAINST PN 38532
            Dim oSAMClient As New SiriusFS.SAM.Client.DataSetControl.Application
            oSAMClient.LoadFromXML(GetDataSetDefinition(Session.Item(CNDataModelCode)), oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)

            If Not String.IsNullOrEmpty(oSAMClient.Risk.Item(Session.Item(CNDataModelCode) & "_POLICY_BINDER").Item(Session.Item(CNDataModelCode) _
                & "_OUTPUT").Item(Session.Item(CNDataModelCode) & "_OUTPUT_ID").Value) Then

                oSAMClient.Risk.Item(Session.Item(CNDataModelCode) & "_POLICY_BINDER").Item(Session.Item(CNDataModelCode) _
                    & "_OUTPUT").DeleteObject()

            End If

            oSAMClient.ReturnAsXML(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
            oSAMClient.Terminate()
            ' End - DONE AGAINST PN 38532


            oWebService = New NexusProvider.ProviderManager().Provider
            Dim bReturnPremiumMoreThanBilled As Boolean = False
            Try
                If Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Then
                    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), oQuote.BranchCode, oQuote.SubBranchCode, "MTA")
                    Session(CNQuote) = oQuote
                ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), oQuote.BranchCode, oQuote.SubBranchCode, "MTC")
                    Session(CNQuote) = oQuote
                ElseIf Session(CNMTAType) = MTAType.REINSTATEMENT Then
                    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), oQuote.BranchCode, oQuote.SubBranchCode, "MTR")
                    Session(CNQuote) = oQuote
                ElseIf Session(CNRenewal) Then

                    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), oQuote.BranchCode, oQuote.SubBranchCode, "REN")
                    Session(CNQuote) = oQuote
                    Dim oRenewalStatus As NexusProvider.RenewalStatus
                    oRenewalStatus = oWebService.GetRenewalStatus(oQuote.InsuranceFileKey)
                    If oRenewalStatus.RenewalStatusTypeDescription = sAwaiting_Manual_Preview Then
                        Dim sTrueMonthlyPolicy As String = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, _
                        NexusProvider.ProductRiskOptions.IsTrueMonthlyPolicy, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing, oQuote.BranchCode).Trim()

                        If (sTrueMonthlyPolicy = "1" And oQuote.AnniversaryCopy = False) Then
                            oWebService.UpdateRenewalStatus(oQuote, "Update")
                        Else
                            oWebService.UpdateRenewalStatus(oQuote, "AutoReview")
                        End If
                    End If
                    Dim oRiskTypes As NexusProvider.RiskType = Session(CNRiskType)
                    Dim sOptionTypeSetting As String
                    'Retreival of the Setting from Product Risk maintenance
                    sOptionTypeSetting = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.BindRenewalWithoutInvitation, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, oRiskTypes.RiskCode)

                    If sOptionTypeSetting = 1 Then
                        oWebService.UpdateRenewalStatus(oQuote, "Update")
                    End If
                Else
                    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey))
                    Session(CNQuote) = oQuote
                End If

                bReturnPremiumMoreThanBilled = oQuote.Risks(Session(CNCurrentRiskKey)).ReturnPremiumMoreThanBilled
            Catch ex As NexusProvider.NexusException
                If CType(Session("IsFromSummary"), Boolean) Then
                    Session("IsFromSummary") = False
                    Response.Redirect(sNextPage)
                Else
                    If ex.Errors(0).Code = "277" Or ex.Errors(0).Code = "279" Then
                        If sReferScreen.ToLower = "true" Then
                            Response.Redirect(ResolveUrl(sReferScreenURL))
                        Else
                            Response.Redirect("~/referred.aspx")
                        End If
                    ElseIf ex.Errors(0).Code = "278" Or ex.Errors(0).Code = "280" Then
                        If sDeclineScreen.ToLower = "true" Then
                            Response.Redirect(ResolveUrl(sDeclineScreenURL))
                        Else
                            Response.Redirect("~/declined.aspx")
                        End If
                    End If
                End If
            Finally
                oWebService = Nothing
            End Try

            oWebService = New NexusProvider.ProviderManager().Provider

            Dim sStatusCode As String = String.Empty

            If oQuote Is Nothing Then
                'read from risk xml
                'Catlin Performance Fix
                ' oQuote = Session(CNQuote)

                'GetHeaderAndSummaries isn't returning a statuscode at the moment,
                'so we need to retrieve from the xml dataset

                oSAMClient.LoadFromXML(GetDataSetDefinition(Session(CNDataModelCode)), oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)

                If oSAMClient.Risk.Item(Session.Item(CNDataModelCode) & "_POLICY_BINDER").Item(Session.Item(CNDataModelCode) _
                    & "_OUTPUT").Item("STATUS").Value IsNot Nothing Then

                    sStatusCode = oSAMClient.Risk.Item(Session.Item(CNDataModelCode) _
                        & "_POLICY_BINDER").Item(Session.Item(CNDataModelCode) & "_OUTPUT").Item("STATUS").Value

                Else
                    Response.Redirect(CType(WebConfigurationManager.GetSection("system.web/customErrors"), CustomErrorsSection).DefaultRedirect)
                End If

            Else
                sStatusCode = oQuote.Risks(Session(CNCurrentRiskKey)).StatusCode
            End If

            Select Case sStatusCode
                Case "DECLINED"
                    If sDeclineScreen.ToLower = "true" Then
                        Response.Redirect(ResolveUrl(sDeclineScreenURL))
                    Else
                        Response.Redirect("~/declined.aspx")
                    End If
                Case "REFERRED"
                    If sReferScreen.ToLower = "true" Then
                        Response.Redirect(ResolveUrl(sReferScreenURL))
                    Else
                        Response.Redirect("~/referred.aspx")
                    End If
                Case Else
                    'bMakeLive is false means user has not selected the Make Live button from risk screen
                    'so process would be same as it was earliar
                    If bMakeLive = False Then
                        'call PrePageRedirect before we redirect to allow this to be overridden if required
                        PrePageRedirect()
                        If sSummaryOfRisk.ToLower = "true" Or sSummaryOfCover.ToLower = "true" Then
                            oQuote = oWebService.GetHeaderAndSummariesByKey(oQuote.InsuranceFileKey)
                            'Multirisk control is showing risk in sorting order of FolderKey. 
                            'So we need to sort this here as well otherwise Session(CNCurrentRiskKey) will start to pick the incorrect risk
                            If oQuote.Risks.Count > 1 Then
                                oQuote.Risks.SortColumn = "FolderKey"
                                oQuote.Risks.SortObjectType = GetType(NexusProvider.Risk)
                                oQuote.Risks.SortingOrder = SortDirection.Ascending
                                oQuote.Risks.Sort()
                            End If

                            For iCount As Integer = 0 To oQuote.Risks.Count - 1
                                oWebService.GetRisk(oQuote.Risks(iCount).Key, iCount, oQuote)
                            Next
                            oQuote.Risks(Session(CNCurrentRiskKey)).ReturnPremiumMoreThanBilled = bReturnPremiumMoreThanBilled
                            Session(CNQuote) = oQuote
                            CalculatePremiumAndTax(dPremium, dTax, dTaxRate, dTotalPremium, dSumInsured, dFee)
                            Dim oAgentCommission As NexusProvider.EditAgentCommission
                            oAgentCommission = oWebService.GetAgentCommission(oQuote.InsuranceFileKey)
                            If oAgentCommission IsNot Nothing Then
                                Dim count As Integer
                                Dim cTotalAmount As Double = 0.0
                                Dim cTotalTax As Double = 0.0
                                For count = 0 To oAgentCommission.AgentCommission.Count - 1
                                    cTotalAmount = oAgentCommission.AgentCommission(count).CommissionValue + cTotalAmount
                                    cTotalTax = oAgentCommission.AgentCommission(count).TaxValue + cTotalTax
                                Next
                                Session(CNAgentComm) = cTotalAmount + cTotalTax
                            End If
                        End If

                        If sSummaryOfRisk.ToLower = "true" Then
                            Response.Redirect(ResolveUrl(sSummaryOfRiskURL), False)
                        ElseIf sSummaryOfCover.ToLower = "true" Then
                            Response.Redirect(ResolveUrl(sSummaryOfCoverURL), False)
                        End If
                        Select Case CType(Session.Item(CNQuoteMode), QuoteMode)
                            Case QuoteMode.FullQuote
                                Response.Redirect("~/secure/RiskDetails.aspx", False)
                            Case QuoteMode.QuickQuote
                                Session.Add(CNFinalScreenCode, sScreenCode)
                                Response.Redirect("~/QQPremium.aspx", False)
                            Case QuoteMode.MTAQuote
                                Response.Redirect("~/secure/RiskDetails.aspx", False)
                            Case QuoteMode.ReQuote
                                Response.Redirect("~/secure/RiskDetails.aspx", False)
                        End Select
                    Else
                        'User has opt to make live the policy directly from risk screen
                        'Calculate the Permium
                        Dim dTotalPremium As Decimal = CheckAndCalculateRoundOff()
                        Dim dAgentComm As Decimal
                        Dim bFound As Boolean = False
                        Dim oPolicySummary As NexusProvider.PolicySummary
                        Dim sRedirectPath As String = String.Empty
                        Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
                        Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
                        Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name
                        oWebService = New NexusProvider.ProviderManager().Provider
                        'if agent is broker then it's commission should be deducetd from premium
                        If Session(CNAgentType) IsNot Nothing And Session(CNAgentComm) IsNot Nothing Then
                            If Session(CNAgentType).ToString.Trim.ToUpper = "BROKER" Then
                                dAgentComm = CalculateAgentCommission()
                                dTotalPremium = dTotalPremium - dAgentComm
                                bFound = True
                            End If
                        ElseIf Session(CNQuote) IsNot Nothing Then
                            'Find The AgentType through SAM Call, if AgentType is not in the session
                            Dim oTempParty As NexusProvider.PartyCollection
                            Dim oTempSearchCriteria As New NexusProvider.PartySearchCriteria

                            oTempSearchCriteria.AgentType = Nothing
                            oTempSearchCriteria.ShortName = CType(Session(CNQuote), NexusProvider.Quote).AgentCode
                            oTempSearchCriteria.PartyType = CType(Session(CNAgentDetails), NexusProvider.UserDetails).PartyType
                            oTempSearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.AG)

                            oTempParty = oWebService.FindParty(oTempSearchCriteria)

                            If oTempParty IsNot Nothing Then
                                If oTempParty.Count > 0 Then
                                    Session(CNAgentType) = oTempParty(0).AgentType
                                    'Check if Agent is Broker then Agent Commission should be deducted from Total AMount
                                    If Session(CNAgentType).ToString.Trim.ToUpper = "BROKER" Then
                                        dAgentComm = CalculateAgentCommission()
                                        dTotalPremium = dTotalPremium - dAgentComm
                                        bFound = True
                                    End If
                                End If
                            End If
                        End If
                        'Initialization of the oPayment Object with Agent Collection method
                        Dim oPayment As New NexusProvider.Payment(NexusProvider.PaymentTypes.AgentCollection, dTotalPremium)
                        'Calling of the BindQuote
                        If Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Then
                            oPolicySummary = New NexusProvider.PolicySummary(oQuote.Reference)
                            oPolicySummary = oWebService.BindQuote(oQuote.InsuranceFileKey, oPayment, oQuote.TimeStamp, Nothing, Nothing, "MTA")
                        ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                            oPolicySummary = New NexusProvider.PolicySummary(oQuote.Reference)
                            oPolicySummary = oWebService.BindQuote(oQuote.InsuranceFileKey, oPayment, oQuote.TimeStamp, Nothing, Nothing, "MTC")
                        ElseIf Session(CNRenewal) IsNot Nothing Then
                            oPolicySummary = New NexusProvider.PolicySummary(oQuote.Reference)
                            oPolicySummary = oWebService.BindQuote(oQuote.InsuranceFileKey, oPayment, oQuote.TimeStamp, True, Nothing, "REN")
                        Else
                            oPolicySummary = oWebService.BindQuote(oQuote.InsuranceFileKey, oPayment, oQuote.TimeStamp, Nothing, Nothing, "NB")
                        End If
                        'In oreder to show policy number on transactionconfirmation page, so put it down in session
                        Session.Item(CNPolicy_Summary) = oPolicySummary
                        'Redirect the user to the madelive.aspx, if page is available
                        If System.IO.File.Exists(Server.MapPath(sRedirectPath)) Then
                            sRedirectPath = sProductFolder & "/madelive.aspx"
                        Else
                            'if the page is not available
                            Session(CNPaid) = False
                            Session(CNIsTransactionConfirmationVisited) = True
                            sRedirectPath = "~/secure/TransactionConfirmation.aspx"
                        End If
                        Response.Redirect(sRedirectPath, False)
                    End If
            End Select

        End Sub
        Function CalculateAgentCommission() As Decimal
            'Calculate the Agent Commision, if any
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oHeaderAndAgentCommission As NexusProvider.HeaderAndAgentCommission
            Dim oAgentCommissionCollection As NexusProvider.AgentCommissionCollection
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim cTotalAmount As Decimal
            Dim cTotalTax As Decimal

            oHeaderAndAgentCommission = oWebService.GetHeaderAndAgentCommissionByKey(oQuote.InsuranceFileKey)
            oAgentCommissionCollection = oHeaderAndAgentCommission.AgentCommission

            If oAgentCommissionCollection IsNot Nothing And oAgentCommissionCollection.Count > 0 Then
                'this is used to calculate the total Agent Commission amount in the Quote.
                Dim count As Integer
                For count = 0 To oAgentCommissionCollection.Count - 1
                    cTotalAmount = oAgentCommissionCollection(count).CommissionValue + cTotalAmount
                    cTotalTax = oAgentCommissionCollection(count).TaxValue + cTotalTax
                Next

                Return (cTotalAmount + cTotalTax)
            End If
        End Function
        ''' <summary>
        ''' Handles the TabIndex click event and redirects to the location passed
        ''' </summary>
        ''' <param name="v_sPath">The location of the page to redirect to</param>
        ''' <remarks></remarks>
        Public Sub TabClick(ByVal v_sPath As String)

            If Page.IsValid Then

                Session(CNQuoteInSync) = True
                WriteRisk()
                If Page.IsValid Then
                    If Session(CNCurrentOI) IsNot Nothing Then
                        Session(CNOI) = Session(CNCurrentOI)
                    End If
                    'Session(CNOI) = Nothing
                    'empty the Risl level SW session after successfull writing of Risk
                    Session(CNRiskStandardWordingsTemplate) = Nothing
                    Response.Redirect(v_sPath)
                End If
            End If

        End Sub

        ''' <summary>
        ''' Handles the edit child item event from the ItemGrid when the ItemGrids is not in 'inline' mode
        ''' </summary>
        ''' <param name="v_sPath">Location of the first child page to redirect to</param>
        ''' <param name="v_sOI">Dataset child item identifier</param>
        ''' <remarks>The current form data will be written to the dataset at this point.</remarks>
        Public Sub EditItem(ByVal v_sPath As String, ByVal v_sOI As String, ByVal v_sScreenCode As String)

            Session(CNQuoteInSync) = True

            WriteRisk()
            'empty the Risl level SW session after successfull writing of Risk
            Session(CNRiskStandardWordingsTemplate) = Nothing
            If Session(CNMode) <> Mode.View And Session(CNMode) <> Mode.Review Then
                StorePreviousNode(v_sOI)
                AddUCElement(v_sOI)
                'Run Default Rules Edit is called to load the data from Sub Main
                EditDefaultRule(v_sScreenCode)
            End If

            oOI.Push(v_sOI)
            Session.Item(CNOI) = oOI
            Session(CNCurrentOIItem) = v_sOI
            Response.Redirect(v_sPath, False)

        End Sub
        Sub StorePreviousNode(ByVal v_sOI As String)
            'Need to store the prevoius copy of XMLDataset
            'If user is editing the records
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim hCurrentNodeColl As New Hashtable()
            If Session(CNNode) IsNot Nothing Then
                hCurrentNodeColl = CType(Session(CNNode), Hashtable)
            End If

            Dim Doc As New XmlDocument
            Doc.LoadXml(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)

            Dim oNode As XmlNode = Doc.SelectSingleNode("//*[@OI='" & v_sOI & "']")

            'check if key already exists
            If hCurrentNodeColl.Item(v_sOI) Is Nothing Then
                'Added into collection
                hCurrentNodeColl.Add(v_sOI, oNode.OuterXml)
            Else
                hCurrentNodeColl.Item(v_sOI) = oNode.OuterXml
            End If
            Session(CNNode) = hCurrentNodeColl
        End Sub
        ''' <summary>
        ''' Handles the edit child item event from the ItemGrid when in the ItemGrid is in 'inline' mode
        ''' </summary>
        ''' <param name="v_sRiskContainer">Control Id of the RiskContainer containing the child screen controls.</param>
        ''' <param name="v_sOI">Dataset child item identifier</param>
        ''' <remarks></remarks>
        Public Sub EditItemInRiskContainer(ByVal v_sRiskContainer As String, ByVal v_sOI As String)
            Session(CNQuoteInSync) = True

            Dim oRiskContainer As RiskContainer = oMaster.FindControl(v_sRiskContainer)

            If oRiskContainer IsNot Nothing Then
                If Session(CNMode) <> Mode.View And Session(CNMode) <> Mode.Review Then
                    'Run Default Rules Edit is called to load the data from Sub Main
                    EditDefaultRule(oRiskContainer.ScreenCode)
                End If

                oRiskContainer.Mode = RiskContainer.ChildMode.Edit
                oRiskContainer.OI = v_sOI
                DataSetFunctions.ReadContainerFromXML(oRiskContainer, v_sOI, Me)

                'Make disable the control in view and review mode
                If Session(CNMode) = Mode.View Or Session(CNMode) = Mode.Review Then
                    DisableControls(oRiskContainer, True)
                End If
            End If

        End Sub
        Sub EditDefaultRule(ByVal sScreenCode As String)
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim v_bSkipSaveToDB As Boolean = True
            'Save the Data on Click of the every Addition of the child
            Dim oProduct As Config.Product = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products.GetProductByCode(oQuote.ProductCode)
            If oProduct.AutoSave Then
                v_bSkipSaveToDB = False
            End If

            If Session(CNMTAType) IsNot Nothing And Session(CNRenewal) Is Nothing Then
                If Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Then
                    oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = oWebService.RunDefaultRulesEdit(sScreenCode, oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset, Nothing, oQuote.BranchCode, "MTA", v_bSkipSaveToDB, v_dtInceptionTPI:=oQuote.InceptionTPI)
                ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                    oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = oWebService.RunDefaultRulesEdit(sScreenCode, oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset, Nothing, oQuote.BranchCode, "MTC", v_bSkipSaveToDB, v_dtInceptionTPI:=oQuote.InceptionTPI)
                ElseIf (Session(CNMTAType) = MTAType.REINSTATEMENT) Then
                    oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = oWebService.RunDefaultRulesEdit(sScreenCode, oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset, Nothing, oQuote.BranchCode, "MTR", v_bSkipSaveToDB, v_dtInceptionTPI:=oQuote.InceptionTPI)
                End If
            ElseIf Session(CNMTAType) Is Nothing And Session(CNRenewal) IsNot Nothing Then
                oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = oWebService.RunDefaultRulesEdit(sScreenCode, oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset, Nothing, oQuote.BranchCode, "REN", v_bSkipSaveToDB, v_dtInceptionTPI:=oQuote.InceptionTPI)
            Else
                oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = oWebService.RunDefaultRulesEdit(sScreenCode, oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset, Nothing, oQuote.BranchCode, "NB", v_bSkipSaveToDB, v_dtInceptionTPI:=oQuote.InceptionTPI)
            End If
        End Sub
        ''' <summary>
        ''' Handles the add child item event from the ItemGrid, when the ItemGrid is NOT in 'inline' mode
        ''' </summary>
        ''' <param name="v_sScreenCode">ScreenCode of the child item to be added</param>
        ''' <param name="v_sPath">Location of the first child page to be redirected to</param>
        ''' <param name="v_sParentElement">Parent Element of the child item, as we need to identifier where to
        ''' place the child item within the dataset</param>
        ''' <param name="v_sChildElement">Child Element name, need for creation of the element</param>
        ''' <remarks>The current risk page form data will also be written to the dataset</remarks>
        Public Sub AddItem(ByVal v_sScreenCode As String, ByVal v_sPath As String, _
                            ByVal v_sParentElement As String, ByVal v_sChildElement As String)

            Session(CNQuoteInSync) = True

            WriteRisk()
            'empty the Risl level SW session after successfull writing of Risk
            Session(CNRiskStandardWordingsTemplate) = Nothing
            'create new element in XML
            Dim sOI As String = DataSetFunctions.CreateElementFromXML(v_sScreenCode, _
                oOI.Peek.ToString(), v_sParentElement, v_sChildElement)

            oOI.Push(sOI)
            Session.Item(CNOI) = oOI
            Session(CNCurrentOIItem) = sOI

            Response.Redirect(v_sPath, False)

        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="v_sScreenCode"></param>
        ''' <param name="v_sParentElement"></param>
        ''' <param name="v_sChildElement"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function LoadChildDefaultItem(ByVal v_sScreenCode As String, ByVal v_sParentElement As String, ByVal v_sChildElement As String) As String
            Dim oRiskContainer As New RiskContainer

            If Session(CNOI) IsNot Nothing AndAlso oOI.Count = 0 Then
                oOI = Session(CNOI)
            End If

            oRiskContainer.OI = DataSetFunctions.CreateElementFromXML(v_sScreenCode, _
                    oOI.Peek, v_sParentElement, v_sChildElement)

            Return oRiskContainer.OI
        End Function
        ''' <summary>
        ''' Handles the child item deletion event
        ''' </summary>
        ''' <param name="v_sOI">Dataset identifier of the selected child item</param>
        ''' <param name="v_sChildElement">Element name within the dataset of the child item</param>
        ''' <remarks></remarks>
        Public Sub DeleteItem(ByVal v_sOI As String, ByVal v_sChildElement As String)

            DataSetFunctions.DeleteElementFromXML(sScreenCode, v_sOI, v_sChildElement)
            'Save the Risk before rendering the XML
            'To save the changed data dering Delete operation
            WriteRisk()
            'empty the Risl level SW session after successfull writing of Risk
            Session(CNRiskStandardWordingsTemplate) = Nothing
            DataSetFunctions.ReadContainerFromXML(oMaster, oOI.Peek.ToString(), Me)

        End Sub

        ''' <summary>
        ''' An overridable event executed before the form data is written to the dataset. This can
        ''' used within the product implementation in the risk page to allow any data changes to take place,
        ''' without being concerned about which controls/events cause the data to be saved.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub PreDataSetWrite()

        End Sub

        ''' <summary>
        ''' An overridable event executed after form data has been written to the dataset. This can be implemented
        ''' within the risk screens of a product and is generally sued to change the Previous/Next page redirect when
        ''' validation or entered values cause a different route through the risks pages.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub PostDataSetWrite()

        End Sub
        ''' <summary>
        ''' An overridable event executed after form data has been written to the dataset. This can be implemented
        ''' within the risk screens of a product and is generally sued to change the Previous/Next page redirect when
        ''' validation or entered values cause a different route through the risks pages.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub PrePageRedirect()

        End Sub

        ''' <summary>
        ''' This will acutally allow the user to move to the same page back from where he has come.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub BackButtonRedirect()

        End Sub


        ''' <summary>
        ''' Writes the current form data to the risk dataset
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub WriteRisk()

            If CType(Session.Item(CNMode), Mode) <> Mode.View Then
                Dim sOI As String = Nothing
                PreDataSetWrite()
                If oOI.Count > 0 Then
                    sOI = oOI.Peek.ToString()
                End If
                WriteContainerToXML(oMaster, sScreenCode, sOI, False, sParentTab)
                Session(CNCurrentOIItem) = Nothing
                PostDataSetWrite()

                'Save the Data on Click of the every Addition of the child
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim oProduct As Config.Product = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).Products.GetProductByCode(oQuote.ProductCode)
                If oProduct.AutoSave Then
                    SaveQuote()
                End If
            End If

        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

        Private Function GetStatus(ByVal v_sQuote As NexusProvider.Quote) As String
            Dim Navigator As XPathNavigator
            Dim srDataset As New System.IO.StringReader(v_sQuote.Risks(0).XMLDataset)
            Dim Doc As XPathDocument = New XPathDocument(New XmlTextReader(srDataset))
            Navigator = Doc.CreateNavigator()
            Dim i As XPathNodeIterator
            Dim sQuoteQuoted As String = Nothing

            Dim sPath As String = "/DATA_SET/RISK_OBJECTS/*/*[@STATUS]"
            i = Navigator.Select(sPath)

            While i.MoveNext()
                'If so check its status, if 1 we're doing a quickquote, else we're not
                If i.Current.GetAttribute("STATUS", String.Empty) = "QUOTED" Then
                    sQuoteQuoted = "QUOTED"
                End If
            End While

            srDataset.Dispose()

            Return sQuoteQuoted
        End Function
#Region "MainDetails"
        ''' <summary>
        ''' This methods fill the sub branches by default or based on the value passed in it.
        ''' </summary>
        ''' <param name="oBranchCode"></param>
        ''' <remarks></remarks>
        Public Sub FillSubBranches(Optional ByVal oBranchCode As String = Nothing)
            'Fill Sub Branch
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim ddlSubBranch As DropDownList = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__SUBBRANCH"), DropDownList)
            Dim oLookUP As New NexusProvider.LookupListCollection
            'Removing of the cache object
            Cache.Remove("PMLookup_Sub_Branch")

            'sam call to retreive the list of branch from table source
            oLookUP = oWebService.GetList(NexusProvider.ListType.PMLookup, "Source", False, False, "Source_ID")
            'Retreival of the Branch Key, which will latet identify the sub-branch
            'sam need barnch key to find the respective sub-branches of the selected branches
            Dim iBranchKey As Integer = 0
            For iBranchCount As Integer = 0 To oLookUP.Count - 1
                If oLookUP(iBranchCount).Code = oBranchCode Then
                    iBranchKey = oLookUP(iBranchCount).Key
                    Exit For
                End If
            Next
            'sam call to retreive the list of sub-branch from table source
            oLookUP = Nothing
            oLookUP = oWebService.GetList(NexusProvider.ListType.PMLookup, "Sub_Branch", False, False, "Source_ID", iBranchKey, Session(CNTransBranchCode))

            'Populating the sub-branch control with the retreived values
            If ddlSubBranch IsNot Nothing Then
                'existing items cleared
                ddlSubBranch.Items.Clear()
                For iSubBranchCount As Integer = 0 To oLookUP.Count - 1
                    Dim lstSubBranch As New ListItem
                    lstSubBranch.Text = oLookUP(iSubBranchCount).Description
                    lstSubBranch.Value = Trim(oLookUP(iSubBranchCount).Code)
                    ddlSubBranch.Items.Add(lstSubBranch)
                    ddlSubBranch.DataBind()
                Next
            End If
        End Sub
        ''' <summary>
        ''' This method fill the currency based on the Branch in session
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub FillCurrency()
            'Fill Currency
            Dim oCurrencyCollection As NexusProvider.CurrencyCollection
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim ddlCurrency As DropDownList = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__CURRENCY"), DropDownList)
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)

            'Sam call to find the currency based on the branch in session
            oCurrencyCollection = oWebService.GetCurrenciesByBranch(Session(CNTransBranchCode))

            'Populating the currency control bansed on results returned
            If ddlCurrency IsNot Nothing Then
                'Making collection empty
                ddlCurrency.Items.Clear()
                For i As Integer = 0 To oCurrencyCollection.Count - 1
                    Dim lstCurrency As New ListItem
                    lstCurrency.Text = oCurrencyCollection.Item(i).Description.ToString
                    lstCurrency.Value = Trim(oCurrencyCollection.Item(i).CurrencyCode.ToString)
                    ddlCurrency.Items.Add(lstCurrency)
                Next
                ddlCurrency.DataBind()

                'Setting of the values based on the oQuote.CurrencyCode
                If oQuote IsNot Nothing Then
                    If oQuote.CurrencyCode IsNot Nothing AndAlso ddlCurrency.Items.FindByValue(oQuote.CurrencyCode) IsNot Nothing Then
                        If oQuote.CurrencyCode.Trim.Length > 0 Then
                            ddlCurrency.SelectedValue = oQuote.CurrencyCode
                        End If
                    Else
                        ddlCurrency.SelectedIndex = 0
                    End If
                End If
            End If
        End Sub
        ''' <summary>
        ''' This method fill the Branches
        ''' </summary>
        ''' <param name="oBranchCode"></param>
        ''' <remarks></remarks>
        Public Sub FillBanches(Optional ByVal oBranchCode As String = Nothing)
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim ddlBranch As DropDownList = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__BRANCH"), DropDownList)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            'If branch code is available in oQuote object
            If oBranchCode Is Nothing Then
                oBranchCode = oQuote.BranchCode
            End If
            If Not IsPostBack AndAlso ddlBranch IsNot Nothing Then
                'Fill Branch for Agent/Employee login
                If Session(CNLoginType) = LoginType.Agent Then
                    If oUserDetails IsNot Nothing Then
                        Dim oListOfBranches As NexusProvider.BranchCollection = oUserDetails.AvailableUserProductsByBranch.GetProductByCode(oQuote.ProductCode.Trim).ListOfBranches
                        For i As Integer = 0 To oListOfBranches.Count - 1
                            Dim lstBranch As New ListItem
                            lstBranch.Text = oListOfBranches(i).Description.ToString
                            lstBranch.Value = oListOfBranches(i).Code.ToString.Trim
                            ddlBranch.Items.Add(lstBranch)
                            ddlBranch.DataBind()
                        Next
                    End If
                Else 'For Customer login
                    Dim lstBranch As New ListItem
                    lstBranch.Text = oNexusConfig.BranchCode
                    lstBranch.Value = oNexusConfig.BranchCode
                    ddlBranch.Items.Add(lstBranch)
                    ddlBranch.DataBind()
                End If

                'Setting the values in Session based on the value from oQuote object
                If oQuote.BranchCode IsNot Nothing Then
                    Session(CNTransBranchCode) = oQuote.BranchCode
                    ddlBranch.SelectedValue = Session(CNTransBranchCode)
                End If
            End If
        End Sub
        ''' <summary>
        ''' This method fires the selected index event of the branch dropdownlist
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Sub ddlBranchCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            If CType(sender, DropDownList).SelectedValue.Trim.Length > 0 Then
                Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim txtAgentCode As TextBox
                txtAgentCode = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__AGENTCODE"), TextBox)

                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oUserAuthority As New NexusProvider.UserAuthority
                oUserAuthority.UserCode = Session(CNLoginName)
                'Pass Allow Edit Agent during MTA/MTC option
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.AgentEditableDuringMTAMTC
                'Get User Authority Value
                oWebService.GetUserAuthorityValue(oUserAuthority)
                Dim strSelectedBranchCode As String = CType(sender, DropDownList).SelectedValue
                'Get all the branches for an agent 
                Dim oAgentSettings As NexusProvider.AgentSettings = Nothing
                Dim hftxtAgent As HiddenField = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__AGENT"), HiddenField)

                If hftxtAgent.Value.Trim.Length <> 0 AndAlso hftxtAgent.Value.Trim <> "0" Then
                    GetAgentSettingsCall(oAgentSettings, hftxtAgent.Value)
                End If
                Dim blAgentBranch As Boolean = False
                'Loop through all the branches returned from above SAM Method
                If oAgentSettings IsNot Nothing AndAlso oAgentSettings.AgentBranchCollection IsNot Nothing Then
                    For i = 0 To oAgentSettings.AgentBranchCollection.Count - 1
                        ' Check Agent branchs(Allowed branches) with selected branch
                        If strSelectedBranchCode.Trim.ToUpper = oAgentSettings.AgentBranchCollection(i).Code.Trim.ToUpper Then
                            blAgentBranch = True
                            Exit For
                        End If
                    Next
                End If
                'Agent doesnot have permission to branch
                If blAgentBranch = False Then
                    If (Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Or Session(CNMTAType) = MTAType.CANCELLATION) And oUserAuthority.UserAuthorityValue = False Then
                        'if selected branche not exist in allowed branches for the agent, then show a validation message using javascript. 
                        'and reset selected branch value for setting previous value
                        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                        'Reset Branch to be previous one in oQuote object
                        CType(sender, DropDownList).ClearSelection()
                        CType(sender, DropDownList).SelectedValue = Session(CNTransBranchCode)
                        'Show Alert Message for not to changing branch
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowValidation", "ShowValidationAndResetBranch('" + Session(CNTransBranchCode).ToString().Trim() + "');", True)
                        Exit Sub
                    Else
                        'We need to clear agent code on change of branch code as agent will be searched for the selected branch only
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "toggleBranch", "toggleBranch();", True)
                    End If
                End If

                'Populating of the sub-branches based on the currenct selection
                FillSubBranches(CType(sender, DropDownList).SelectedValue)

                'Session updation with the latest values, this session is used to find the user selected branch
                Session(CNTransBranchCode) = CType(sender, DropDownList).SelectedValue

                'Populating of the currency based on the currenct selection
                FillCurrency()

                'For Agent/Employee login Population of the cover note book
                If Session(CNLoginType) = LoginType.Agent Then
                    Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                    If oUserDetails IsNot Nothing Then
                        FillCoverNoteBook()
                    End If
                End If
            End If
        End Sub
        ''' <summary>
        ''' Custom Validator to validate the cover to date
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="args"></param>
        ''' <remarks></remarks>
        Protected Sub custToDate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oRiskTypes As NexusProvider.RiskType = Session(CNRiskType)
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim txtCoverStartDate As TextBox = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__COVERSTARTDATE"), TextBox)
            Dim txtCoverEndDate As TextBox = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__COVERENDDATE"), TextBox)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oOptionSettings As NexusProvider.OptionTypeSetting
            Dim hiddenMidnightRenewalSettings, sOptionTypeSetting, hiddenOptionSetting As String
            'Retreival of the Setting from System Option
            oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 1009)
            hiddenOptionSetting = oOptionSettings.OptionValue
            'Retreival of the Setting from Product Risk maintenance
            sOptionTypeSetting = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsMidnightRenewal, NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, oRiskTypes.RiskCode)
            hiddenMidnightRenewalSettings = Val(sOptionTypeSetting.ToString)

            If txtCoverEndDate IsNot Nothing AndAlso txtCoverStartDate IsNot Nothing AndAlso txtCoverEndDate.Text.Trim.Length <> 0 Then
                If IsDate(txtCoverEndDate.Text.Trim) = True Then
                    Dim dCovertEndDate As Date = CDate(txtCoverEndDate.Text)
                    Dim dCoverStartDate As Date = CDate(txtCoverStartDate.Text)
                    Dim dMaxDate As Date
                    'Setting of the cover to date based on the system option settings
                    If hiddenMidnightRenewalSettings = 1 Then
                        'in case of mid night renewal 1 day would be minus
                        dMaxDate = dCoverStartDate.AddMonths(Val(hiddenOptionSetting))
                        dMaxDate = dMaxDate.AddDays(-1)
                    Else
                        dMaxDate = dCoverStartDate.AddMonths(Val(hiddenOptionSetting))
                    End If
                    If hiddenMidnightRenewalSettings = 0 AndAlso dCovertEndDate = dCoverStartDate Then
                        CType(source, CustomValidator).ErrorMessage = "The effective date and expiration date are same #CoverStartDate."
                        CType(source, CustomValidator).ErrorMessage = CType(source, CustomValidator).ErrorMessage.Replace("#CoverStartDate", dCoverStartDate.ToShortDateString)
                        args.IsValid = False
                    End If
                    'Checking the valid cover end date
                    If dCovertEndDate < dCoverStartDate Then
                        CType(source, CustomValidator).ErrorMessage = "Invalid Cover End Date.. Cover End Date can not be less than cover start date #CoverStartDate"
                        CType(source, CustomValidator).ErrorMessage = CType(source, CustomValidator).ErrorMessage.Replace("#CoverStartDate", dCoverStartDate.ToShortDateString)
                        args.IsValid = False
                    ElseIf dCovertEndDate > FormatDateTime(dMaxDate, DateFormat.ShortDate) Then
                        CType(source, CustomValidator).ErrorMessage = "Invalid Cover End Date.. Cover End Date can not be greater than #CoverEndDate"
                        CType(source, CustomValidator).ErrorMessage = CType(source, CustomValidator).ErrorMessage.Replace("#CoverEndDate", dMaxDate.ToShortDateString)
                        args.IsValid = False
                    End If
                End If
            End If

        End Sub

        ''' <summary>
        ''' Custom Validator to validate the cover from date
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="args"></param>
        ''' <remarks></remarks>
        Protected Sub custFromDate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim txtCoverStartDate As TextBox = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__COVERSTARTDATE"), TextBox)

            If txtCoverStartDate IsNot Nothing AndAlso txtCoverStartDate.Text.Trim.Length <> 0 Then
                'Setting of the cover from date based on the system option settings
                If IsDate(txtCoverStartDate.Text.Trim) = True Then
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim dCovertStartDate As Date = CDate(txtCoverStartDate.Text)
                    Dim dMinDate, dMaxDate As Date
                    Dim oFrmDateOptionSettings As NexusProvider.OptionTypeSetting
                    Dim hiddenOptionSetting As String
                    'Retreival of the Setting from System Option
                    oFrmDateOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 1008)
                    hiddenOptionSetting = oFrmDateOptionSettings.OptionValue

                    dMinDate = Date.Now.AddYears(-2)
                    If Session(CNRenewal) Then
                        dMaxDate = dCovertStartDate.AddMonths(Val(hiddenOptionSetting))
                    Else
                        dMaxDate = Date.Now.AddMonths(Val(hiddenOptionSetting))
                    End If

                    If dCovertStartDate < FormatDateTime(dMinDate, DateFormat.ShortDate) Then
                        'args.IsValid = False
                        'CType(source, CustomValidator).ErrorMessage = "Invalid Cover Start Date.. Cover Start Date can not be less than 2 years back"

                    ElseIf dCovertStartDate > FormatDateTime(dMaxDate, DateFormat.ShortDate) Then
                        args.IsValid = False
                        CType(source, CustomValidator).ErrorMessage = "Invalid Cover Start Date.. Cover Start Date can not be greater than  #CoverStartDate"
                        CType(source, CustomValidator).ErrorMessage = CType(source, CustomValidator).ErrorMessage.Replace("#CoverStartDate", dMaxDate.ToShortDateString)
                    End If
                End If
            End If

        End Sub
        ''' <summary>
        ''' This method fires the selcted index of the cover note book
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub POLICYHEADER__COVERNOTEBOOKNO_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim ddlCoverNOteBookNo As DropDownList = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__COVERNOTEBOOKNO"), DropDownList)
            Dim ddlCoverNOteSheetNo As DropDownList = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__COVERNOTESHEETNO"), DropDownList)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            'if Controls are not available in the page
            If ddlCoverNOteBookNo Is Nothing Or ddlCoverNOteSheetNo Is Nothing Then
                Exit Sub
            End If

            'if selection is valid then only Not Issue status sheet should be populated
            If ddlCoverNOteBookNo.SelectedIndex > 0 Then
                Dim oCoverNote As New NexusProvider.CoverNote
                Dim oCoverNoteSheetColl As New NexusProvider.CoverNoteSheetTypeCollection
                Dim iCounter As Integer = 0

                oCoverNote.BookNumber = ddlCoverNOteBookNo.SelectedItem.Text
                oCoverNote.CoverNoteBookKey = ddlCoverNOteBookNo.SelectedValue
                oCoverNote.CoverNoteStatusCode = "NOTISS"

                oWebService.GetCoverNoteBook(oCoverNote)
                For iCounter = 0 To oCoverNote.CoverNoteSheets.Count - 1
                    If oCoverNote.CoverNoteSheets(iCounter).CoverNoteSheetStatusCode.Trim = "NOTISS" Then
                        oCoverNoteSheetColl.Add(oCoverNote.CoverNoteSheets(iCounter))
                    End If
                Next

                ddlCoverNOteSheetNo.DataSource = oCoverNoteSheetColl
                ddlCoverNOteSheetNo.DataTextField = "CoverNoteSheetNumber"
                ddlCoverNOteSheetNo.DataValueField = "CoverNoteSheetNumber"
                ddlCoverNOteSheetNo.DataBind()
                ddlCoverNOteSheetNo.Items.Insert(0, New ListItem("(Please Select)", ""))
                ddlCoverNOteSheetNo.SelectedIndex = 0
            Else
                ddlCoverNOteSheetNo.Items.Clear()
                ddlCoverNOteSheetNo.Items.Insert(0, New ListItem("(Please Select)", ""))
                ddlCoverNOteSheetNo.SelectedIndex = 0
            End If
        End Sub
        ''' <summary>
        ''' This method fill the cover note book
        ''' </summary>
        ''' <remarks></remarks>
        Sub FillCoverNoteBook()
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim txtAgent As HiddenField = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__AGENT"), HiddenField)
            Dim ddlCoverNOteBookNo As DropDownList = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__COVERNOTEBOOKNO"), DropDownList)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            'if Controls are not available in the page
            If txtAgent Is Nothing Or ddlCoverNOteBookNo Is Nothing Then
                Exit Sub
            End If

            'if valid agent key is selected/entered
            If txtAgent.Value.Trim.Length > 0 Then
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim oCoverNote As New NexusProvider.CoverNote
                Dim oCoverNoteCollection As New NexusProvider.CoverNoteCollection
                Dim oTempCoverNoteCollection As New NexusProvider.CoverNoteCollection
                Dim iCounter As Integer = 0
                Dim chkIsCoverNoteUsed As CheckBox = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("chkIsCoverNoteUsed"), CheckBox)
                'Diable the cover not panel
                Dim PnlCoverNote As Panel = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__COVERNOTEPANEL"), Panel)
                If PnlCoverNote IsNot Nothing Then
                    DisableControls(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__COVERNOTEPANEL"))
                End If
                If chkIsCoverNoteUsed IsNot Nothing Then
                    chkIsCoverNoteUsed.Enabled = True
                    chkIsCoverNoteUsed.Checked = False
                End If
                'Populating of the valid cover not book
                'based on the valid agent key and branch selected by user
                'Only Issued status cover not book should be populated
                oCoverNote.CoverNoteBranchCode = Session(CNTransBranchCode)
                oCoverNote.AgentKey = txtAgent.Value
                oCoverNote.CoverNoteBookStatusCode = "ISSUED"
                oTempCoverNoteCollection = oWebService.FindCoverNoteBooks(oCoverNote)
                For iCounter = 0 To oTempCoverNoteCollection.Count - 1
                    If oTempCoverNoteCollection(iCounter).CoverNoteStatusDescription IsNot Nothing And _
                    oTempCoverNoteCollection(iCounter).CoverNoteStatusDescription.Trim.ToUpper = "ISSUED" _
                    And CDate(oTempCoverNoteCollection(iCounter).EffectiveDate) <= CDate(Date.Now.ToShortDateString) Then
                        oCoverNoteCollection.Add(oTempCoverNoteCollection(iCounter))
                    End If
                Next

                'Set "Please select" as default for both CoverNote book and sheet No
                ddlCoverNOteBookNo.Items.Clear()
                ddlCoverNOteBookNo.DataSource = oCoverNoteCollection
                ddlCoverNOteBookNo.DataTextField = "BookNumber"
                ddlCoverNOteBookNo.DataValueField = "CoverNoteBookKey"
                ddlCoverNOteBookNo.DataBind()
                ddlCoverNOteBookNo.Items.Insert(0, (New ListItem("(Please Select)", "")))
                ddlCoverNOteBookNo.SelectedIndex = 0
                ddlCoverNOteBookNo.Enabled = False
            End If
        End Sub

        ''' <summary>
        ''' This method will Check True Monthly Policy and Set True Monthly Policy interconnected Controls
        ''' </summary>
        ''' <remarks></remarks>
        Sub CheckTrueMonthlyPolicy()
            Dim oQuote As NexusProvider.Quote = CType(Session(CNQuote), NexusProvider.Quote)
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oProducts As Config.Products =
                    CType(WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(
                        Portal.GetPortalID()).Products
            Dim oAgentSettings As NexusProvider.AgentSettings = Nothing
            Dim oRiskTypes As NexusProvider.RiskType = CType(Session(CNRiskType), NexusProvider.RiskType)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            'Main detail page
            Dim txtCoverStartDate As TextBox =
                    CType(
                        GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName).FindControl(
                            "POLICYHEADER__COVERSTARTDATE"), 
                        TextBox)
            Dim txtCoverEndDate As TextBox =
                    CType(
                        GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName).FindControl(
                            "POLICYHEADER__COVERENDDATE"), 
                        TextBox)
            Dim txtRenewalDate As TextBox =
                    CType(
                        GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName).FindControl(
                            "POLICYHEADER__RENEWAL"), 
                        TextBox)
            Dim hdnMidnightRenewalSettings As HiddenField =
                    CType(
                        GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName).FindControl(
                            "hiddenMidnightRenewalSettings"), 
                        HiddenField)
            Dim lblAnniversary As Label =
                    CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName).FindControl("lblAnniversary"), 
                          Label)
            Dim txtAnniversary As TextBox =
                    CType(
                        GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName).FindControl(
                            "POLICYHEADER__ANNIVERSARY"), 
                        TextBox)
            Dim hdnAgent As HiddenField =
                    CType(
                        GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName).FindControl("POLICYHEADER__AGENT"), 
                        HiddenField)

            'True Monthly Policy Controls 
            Dim ddlUnifiedrenewalday As DropDownList =
                    CType(
                        GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName).FindControl(
                            "POLICYHEADER__UNIFIEDRENEWALDAY"), 
                        DropDownList)
            Dim chkConsolidateLeadAgentCommission As CheckBox =
                    CType(
                        GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName).FindControl(
                            "POLICYHEADER__CONSOLIDATEDLEADAGENTCOMMISSION"), 
                        CheckBox)
            Dim chkConsolidateLeadSubAgentCommission As CheckBox =
                    CType(
                        GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName).FindControl(
                            "POLICYHEADER__CONSOLIDATEDLEADSUBAGENTCOMMISSION"), 
                        CheckBox)
            Dim hdnUnifiedRenewalDay As HiddenField =
                    CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName).FindControl("iUnifiedRenewalDay"), 
                          HiddenField)
            Dim hdnProuductLeadAllowConsolidatedCommission As HiddenField =
                    CType(
                        GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName).FindControl(
                            "HiddenProuductLeadAllowConsolidatedCommission"), 
                        HiddenField)
            Dim hdnProuductSubConsolidatedCommission As HiddenField =
                    CType(
                        GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName).FindControl(
                            "HiddenProuductSubConsolidatedCommission"), 
                        HiddenField)
            Dim lblLeadAgentCommission As Label =
                    CType(
                        GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName).FindControl(
                            "lblLeadAgentCommission"), 
                        Label)
            Dim lblSubAgentCommission As Label =
                    CType(
                        GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName).FindControl(
                            "lblSubAgentCommission"), 
                        Label)
            Dim bIsTrueMonthlyPolicy As Boolean
            Dim chkAgentLeadAllowConsolidatedCommission As Boolean = False

            'if product is configured for True Month Policies
            bIsTrueMonthlyPolicy =
                CType(oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance,
                                                            NexusProvider.ProductRiskOptions.IsTrueMonthlyPolicy,
                                                            NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode,
                                                            oRiskTypes.RiskCode),
                      Boolean)
            If _
                txtCoverStartDate IsNot Nothing AndAlso txtCoverEndDate IsNot Nothing AndAlso
                txtRenewalDate IsNot Nothing AndAlso hdnMidnightRenewalSettings IsNot Nothing AndAlso
                lblAnniversary IsNot Nothing AndAlso hdnAgent IsNot Nothing AndAlso ddlUnifiedrenewalday IsNot Nothing AndAlso
                chkConsolidateLeadAgentCommission IsNot Nothing AndAlso
                chkConsolidateLeadSubAgentCommission IsNot Nothing AndAlso hdnUnifiedRenewalDay IsNot Nothing AndAlso
                hdnProuductLeadAllowConsolidatedCommission IsNot Nothing AndAlso
                hdnProuductSubConsolidatedCommission IsNot Nothing AndAlso lblLeadAgentCommission IsNot Nothing AndAlso
                lblSubAgentCommission IsNot Nothing AndAlso txtAnniversary IsNot Nothing Then
                If Not IsPostBack Then
                    If (bIsTrueMonthlyPolicy) Then
                        'All the code for the mentioned steps will go here
                        ddlUnifiedrenewalday.Visible = True
                        'If a “Unified Renewal Day” is specified within Product Risk Maintenance the system should default Renewal date in the “Anniversary Date”
                        lblAnniversary.Text = GetLocalResource("lblAnniversary", "Anniversary Date")
                        txtRenewalDate.Attributes.Add("readonly", "readonly")
                        txtAnniversary.Attributes.Add("readonly", "readonly")

                        hdnUnifiedRenewalDay.Value = Convert.ToString(oQuote.RenewalDayNo)
                        'if product is configured for True Month Policies within Product Risk Maintenance the system Check value of  LeadAllowConsolidatedCommission
                        hdnProuductLeadAllowConsolidatedCommission.Value =
                            oWebService.GetProductRiskOptionValue(
                                NexusProvider.ProductConfigActionType.ProductRiskMaintenance,
                                NexusProvider.ProductRiskOptions.LeadAllowConsolidatedCommission,
                                NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, oRiskTypes.RiskCode)
                        'if product is configured for True Month Policies within Product Risk Maintenance the system Check value of  SubAllowConsolidatedCommission
                        hdnProuductSubConsolidatedCommission.Value =
                            oWebService.GetProductRiskOptionValue(
                                NexusProvider.ProductConfigActionType.ProductRiskMaintenance,
                                NexusProvider.ProductRiskOptions.SubAllowConsolidatedCommission,
                                NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, oRiskTypes.RiskCode)
                        If oQuote.Agent <> "" AndAlso oQuote.Agent IsNot Nothing Then
                            If oQuote.Agent <> 0 Then
                                'checking “Allow Consolidated Commission” option of selected Agent 
                                GetAgentSettingsCall(oAgentSettings, oQuote.Agent)
                                If oAgentSettings IsNot Nothing Then
                                    'getting value of “Allow Consolidated Commission” of selected Agent
                                    chkAgentLeadAllowConsolidatedCommission = oAgentSettings.AllowConsolidatedCommission
                                End If
                            End If
                        End If

                        'On Page Load of Maindetail Page, set Bydefault Agent Commission CheckBox  as 'True' and Enable as False
                        chkConsolidateLeadAgentCommission.Visible = True
                        chkConsolidateLeadAgentCommission.Enabled = False

                        chkConsolidateLeadSubAgentCommission.Visible = True
                        chkConsolidateLeadSubAgentCommission.Enabled = False

                        lblLeadAgentCommission.Visible = True
                        lblSubAgentCommission.Visible = True

                        If _
                            (hdnProuductLeadAllowConsolidatedCommission.Value = "1" And
                             chkAgentLeadAllowConsolidatedCommission) Then
                            'if "Product Risk Maintenance" LeadAllowConsolidatedCommission checkbox  and  selected "Agent"  LeadAllowConsolidatedCommission option have checked  In BO
                            chkConsolidateLeadAgentCommission.Enabled = True
                        Else
                            chkConsolidateLeadAgentCommission.Enabled = False
                        End If

                        If hdnUnifiedRenewalDay.Value = "0" Then
                            ddlUnifiedrenewalday.SelectedValue = Convert.ToString(oQuote.CoverStartDate.Day)
                        Else
                            ddlUnifiedrenewalday.SelectedValue = hdnUnifiedRenewalDay.Value
                        End If

                        Dim dtAnniversaryDate As Date
                        dtAnniversaryDate = oQuote.CoverStartDate
                        txtAnniversary.Text = dtAnniversaryDate.AddYears(1).ToString()
                        If _
                            IsDate(
                                CInt(ddlUnifiedrenewalday.SelectedValue) & "/" & CDate(oQuote.CoverStartDate).Month &
                                "/" & CDate(txtAnniversary.Text).Year) Then
                            txtAnniversary.Text =
                                CDate(
                                    CInt(ddlUnifiedrenewalday.SelectedValue) & "/" & CDate(oQuote.CoverStartDate).Month &
                                    "/" & CDate(txtAnniversary.Text).Year).ToString()
                        End If

                    End If
                Else
                    'Refreshing of agent 
                    If Request("__EVENTARGUMENT") = "RefreshAgent" Then
                        If hdnAgent.Value <> "" Then
                            'Get the Value of AgentLeadAllowConsolidatedCommission 
                            GetAgentSettingsCall(oAgentSettings, CInt(hdnAgent.Value))
                            If oAgentSettings IsNot Nothing Then

                                chkAgentLeadAllowConsolidatedCommission = oAgentSettings.AllowConsolidatedCommission
                                If _
                                    (hdnProuductLeadAllowConsolidatedCommission.Value = "1" And
                                     chkAgentLeadAllowConsolidatedCommission) Then
                                    chkConsolidateLeadAgentCommission.Enabled = True
                                Else
                                    chkConsolidateLeadAgentCommission.Enabled = False
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' This method fired the event on cover not book checkbox changes 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub chkIsCoverNoteUsed_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim chkIsCoverNoteUsed As CheckBox = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("chkIsCoverNoteUsed"), CheckBox)
            'if Controls are not available in the page
            If chkIsCoverNoteUsed Is Nothing Then
                Exit Sub
            End If

            If chkIsCoverNoteUsed.Checked = True Then
                'Enable the panel controls
                EnableControls(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__COVERNOTEPANEL"))
            Else
                'Disable the panel controls
                DisableControls(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__COVERNOTEPANEL"))
                chkIsCoverNoteUsed.Enabled = True
                chkIsCoverNoteUsed.Checked = False
            End If
        End Sub
        ''' <summary>
        ''' Custom validator to validate the valid agent
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="args"></param>
        ''' <remarks></remarks>
        Protected Sub cust_VldAgent_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim ddlBusinessType As NexusProvider.LookupList = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__BUSINESSTYPE"), NexusProvider.LookupList)
            'if Controls are not available in the page
            If ddlBusinessType Is Nothing Then
                Exit Sub
            End If
            If ddlBusinessType.Value IsNot Nothing Then
                'in case of the DIRECT business type agent is not required otherwise agent is required for rest
                If ddlBusinessType.Value.Trim.Length <> 0 Then
                    If ddlBusinessType.Value.Trim.ToUpper <> "DIRECT" _
                    And ddlBusinessType.Text.Trim.Length = 0 Then
                        args.IsValid = False
                    Else
                        args.IsValid = True
                    End If
                End If
            End If
        End Sub
        ''' <summary>
        ''' Custom validator to validate the valid Cover Sheet Number
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="args"></param>
        ''' <remarks></remarks>
        Protected Sub cust_VldCoverSheet_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim ddlCoverNOteBookNo As DropDownList = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__COVERNOTEBOOKNO"), DropDownList)
            Dim ddlCoverNOteSheetNo As DropDownList = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__COVERNOTESHEETNO"), DropDownList)
            Dim chkIsCoverNoteUsed As CheckBox = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("chkIsCoverNoteUsed"), CheckBox)

            If args.IsValid = True Then
                If chkIsCoverNoteUsed.Checked = True AndAlso String.IsNullOrEmpty(ddlCoverNOteBookNo.SelectedValue) = False Then
                    If String.IsNullOrEmpty(ddlCoverNOteSheetNo.SelectedValue) Then
                        args.IsValid = False
                    Else
                        args.IsValid = True
                    End If
                End If
            End If
        End Sub
        ''' <summary>
        ''' This method fires the lapse button click event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnLapseQuote_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            If Page.IsValid Then
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
                Dim ddlLapseCancelReason As NexusProvider.LookupList = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__LAPSECANCELREASON"), NexusProvider.LookupList)
                Dim txtLapseCancelDate As TextBox = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__LAPSECANCELDATE"), TextBox)
                Dim VldLapseCancel As CustomValidator = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("VldLapseCancel"), CustomValidator)
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                'if Controls are not available in the page
                If ddlLapseCancelReason Is Nothing Or txtLapseCancelDate Is Nothing Then
                    Exit Sub
                ElseIf String.IsNullOrEmpty(ddlLapseCancelReason.Value) = True Then
                    'Validate the Lapse/Cancel Reason
                    VldLapseCancel.ErrorMessage = "Lapse/Cancel reason not specified"
                    VldLapseCancel.IsValid = False
                    Exit Sub
                ElseIf String.IsNullOrEmpty(txtLapseCancelDate.Text) Then
                    'Validate the Lapse/Cancel Date
                    VldLapseCancel.ErrorMessage = "Lapse/Cancel date not specified"
                    VldLapseCancel.IsValid = False
                    Exit Sub
                ElseIf String.IsNullOrEmpty(txtLapseCancelDate.Text) = False AndAlso IsDate(txtLapseCancelDate.Text) = False Then
                    'Validate the Lapse/Cancel Date
                    VldLapseCancel.ErrorMessage = "Invalid Lapse/Cancellation Date"
                    VldLapseCancel.IsValid = False
                    Exit Sub
                ElseIf String.IsNullOrEmpty(txtLapseCancelDate.Text) = False AndAlso IsDate(txtLapseCancelDate.Text) = True Then
                    If CDate(txtLapseCancelDate.Text) < CDate("01/01/1900") And CDate(txtLapseCancelDate.Text) > CDate("01/12/9998") Then
                        'Validate the Lapse/Cancel Date
                        VldLapseCancel.ErrorMessage = "Please enter a date between 01/01/1900 and 01/12/9998"
                        VldLapseCancel.IsValid = False
                        Exit Sub
                    End If
                End If

                'Updation of the oQuote object 
                oQuote.LapseCancelReasonCode = ddlLapseCancelReason.Value
                oQuote.LapseCancelDate = txtLapseCancelDate.Text
                oQuote.PolicyStatusCode = "CUR"
                'sam call to update the status
                Try
                    oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode)
                Finally
                    oWebService = Nothing
                End Try
                'Agent/Employee login
                If CType(Session.Item(CNLoginType), LoginType) = LoginType.Agent Then

                    Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                    Select Case True
                        Case TypeOf oParty Is NexusProvider.PersonalParty
                            Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName, False)
                        Case TypeOf oParty Is NexusProvider.CorporateParty
                            Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName, False)
                    End Select

                    oParty = Nothing
                Else 'Customer login
                    Response.Redirect(oPortal.ClientStartPage.Trim, False)
                End If

            End If
        End Sub
        ''' <summary>
        ''' custom validator validates the cover issue date
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="args"></param>
        ''' <remarks></remarks>
        Protected Sub VldCoverNoteIsueDate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim chkIsCoverNoteUsed As CheckBox = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("chkIsCoverNoteUsed"), CheckBox)
            Dim txtCoverNoteIssuedDate As TextBox = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("txtCoverNoteIssuedDate"), TextBox)
            'if Controls are not available in the page
            If chkIsCoverNoteUsed Is Nothing Or txtCoverNoteIssuedDate Is Nothing Then
                Exit Sub
            End If

            If chkIsCoverNoteUsed.Checked = True And txtCoverNoteIssuedDate.Text.Trim.Length <> 0 Then
                If IsDate(txtCoverNoteIssuedDate.Text.Trim) = False Then
                    args.IsValid = False
                End If
            End If
        End Sub
        ''' <summary>
        ''' This method fires the event on selection of the business type
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub POLICYHEADER__BUSINESSTYPE_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs)
            OnChnageBusinessType()
        End Sub
        Sub OnChnageBusinessType()
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim ddlBusinessType As NexusProvider.LookupList = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__BUSINESSTYPE"), NexusProvider.LookupList)
            Dim btnAgentCode As LinkButton = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__BTNAGENTCODE"), LinkButton)
            Dim PnlCoverNote As Panel = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__COVERNOTEPANEL"), Panel)
            Dim txtAgent As HiddenField = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__AGENT"), HiddenField)
            Dim txtAgentCode As TextBox = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__AGENTCODE"), TextBox)
            'direct business who contact you field disabled
            Dim rfvWhoContactedYou As RequiredFieldValidator = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("rfvWhoCanContact"), RequiredFieldValidator)

            'if Controls are not available in the page

            If ddlBusinessType Is Nothing Or btnAgentCode Is Nothing Or PnlCoverNote Is Nothing _
            Or txtAgent Is Nothing Or txtAgentCode Is Nothing Then
                Exit Sub
            End If
            Dim bAllowEditAgentDuringMTAMTC As Boolean = True
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oUserAuthority As New NexusProvider.UserAuthority
            oUserAuthority.UserCode = Session(CNLoginName)

            'Pass Allow Edit Agent during MTA/MTC option
            oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.AgentEditableDuringMTAMTC
            oWebService.GetUserAuthorityValue(oUserAuthority)
            bAllowEditAgentDuringMTAMTC = oUserAuthority.UserAuthorityValue

            If ddlBusinessType.Value.Trim.Length <> 0 Then
                If ddlBusinessType.Value.Trim.ToUpper = "DIRECT" Then
                    Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                    'Check if "Allow agent change" is enabled for user
                    'Check if user is doing MTA/MTC
                    'Check if existing quote is agency business
                    If Session(CNMTAType) IsNot Nothing And bAllowEditAgentDuringMTAMTC = False And oQuote.BusinessTypeCode.ToUpper <> "DIRECT" Then
                        'Show alert message
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowValidation", "alert('Cannot change to Direct Business this will remove the Agent.\nInsufficient User Authority to remove Agent');", True)
                        'Revert the value as before
                        ddlBusinessType.Value = oQuote.BusinessTypeCode
                    Else
                        'in case of the DIRECT businesscover not panel should be disable
                        'agent panel should also be disable
                        btnAgentCode.Enabled = False
                        PnlCoverNote.Enabled = False
                        txtAgentCode.Text = String.Empty
                        txtAgentCode.Enabled = False
                        txtAgent.Value = String.Empty
                        DisableControls(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__COVERNOTEPANEL"))
                        'who contacted field disabled
                        If Not (rfvWhoContactedYou Is Nothing) Then
                            rfvWhoContactedYou.Enabled = False
                        End If

                    End If
                Else
                    If Session(CNMTAType) IsNot Nothing And bAllowEditAgentDuringMTAMTC = False Then
                        btnAgentCode.Enabled = False
                        txtAgentCode.Enabled = False
                        'who contacted field disabled
                        If Not (rfvWhoContactedYou Is Nothing) Then
                            rfvWhoContactedYou.Enabled = False
                        End If
                    Else
                        btnAgentCode.Enabled = True
                        PnlCoverNote.Enabled = True
                        txtAgentCode.Enabled = True
                    End If
                End If
            End If

        End Sub

        ''' <summary>
        ''' Enable the COver not sheet in case of view
        ''' </summary>
        ''' <param name="bStatus"></param>
        ''' <remarks></remarks>
        Sub EnableDisableCNS(ByVal bStatus As Boolean)
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim ddlCoverNOteSheetNo As DropDownList = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__COVERNOTESHEETNO"), DropDownList)
            Dim txtCoverNOteSheetNo As TextBox = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("txtCoverNoteSheetNo"), TextBox)
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim PnlCoverNote As Panel = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__COVERNOTEPANEL"), Panel)

            'if Controls are not available in the page
            If ddlCoverNOteSheetNo Is Nothing Or txtCoverNOteSheetNo Is Nothing Or PnlCoverNote Is Nothing Then
                Exit Sub
            End If

            'dropdownlist will be hidden and textbox will be show if user come back on maindetails page after allocation of the cover note sheet
            If bStatus = True Then
                ddlCoverNOteSheetNo.Visible = False
                txtCoverNOteSheetNo.Visible = True
                txtCoverNOteSheetNo.Text = oQuote.CoverNoteSheetNumber
                PnlCoverNote.Enabled = False
            Else
                ddlCoverNOteSheetNo.Visible = True
                txtCoverNOteSheetNo.Visible = False
                PnlCoverNote.Enabled = True
            End If
        End Sub
        ''' <summary>
        ''' Enable of the Cover note book in case of the view
        ''' </summary>
        ''' <param name="bStatus"></param>
        ''' <remarks></remarks>
        Sub EnableDisableCNB(ByVal bStatus As Boolean)
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim ddlCoverNOteBookNo As DropDownList = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__COVERNOTEBOOKNO"), DropDownList)
            Dim txtCoverNOteBookNo As TextBox = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("txtCoverNoteBookNo"), TextBox)
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim PnlCoverNote As Panel = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__COVERNOTEPANEL"), Panel)

            'if Controls are not available in the page
            If ddlCoverNOteBookNo Is Nothing Or txtCoverNOteBookNo Is Nothing Or PnlCoverNote Is Nothing Then
                Exit Sub
            End If

            'dropdownlist will be hidden and textbox will be show if user come back on maindetails page after allocation of the cover note book
            If bStatus = True Then
                ddlCoverNOteBookNo.Visible = False
                txtCoverNOteBookNo.Visible = True
                txtCoverNOteBookNo.Text = oQuote.CoverNoteBookNumber
                PnlCoverNote.Enabled = False
            Else
                ddlCoverNOteBookNo.Visible = True
                txtCoverNOteBookNo.Visible = False
                PnlCoverNote.Enabled = True
            End If
        End Sub
        ''' <summary>
        ''' Enable the Alternate Ref based on the agnet selection
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub EnableAlternateRef()
            If Session(CNLoginType) = LoginType.Agent Then
                Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim vldrqdAlternateRef As RequiredFieldValidator = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("vldrqdAlternateRef"), RequiredFieldValidator)
                Dim markAlternateRef As HtmlControl = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("markAlternateRef"), HtmlControl)
                Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                Dim oAgentSettings As NexusProvider.AgentSettings = Nothing
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim txtAgent As HiddenField = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__AGENT"), HiddenField)

                'if Controls are not available in the page
                If vldrqdAlternateRef Is Nothing Or markAlternateRef Is Nothing Or txtAgent Is Nothing Then
                    Exit Sub
                End If

                If txtAgent.Value.Trim.Length <> 0 AndAlso txtAgent.Value.Trim <> "0" Then
                    GetAgentSettingsCall(oAgentSettings, txtAgent.Value)
                End If
                If oAgentSettings IsNot Nothing Then
                    If oAgentSettings.IsAlternateReferenceMandatory Then
                        vldrqdAlternateRef.Enabled = True
                        markAlternateRef.Visible = True
                    Else
                        vldrqdAlternateRef.Enabled = False
                        markAlternateRef.Visible = False
                    End If
                End If
            End If
        End Sub
        ''' <summary>
        ''' This method fill the product 
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub FillProduct()
            Dim oProducts As Config.Products = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim ddlProductlst As DropDownList = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__PRODUCT"), DropDownList)
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)

            'if Controls are not available in the page
            If ddlProductlst Is Nothing Then
                Exit Sub
            End If

            For Each oProduct As Config.Product In oProducts
                ddlProductlst.Items.Add(New ListItem(oProduct.Name, oProduct.ProductCode))
            Next
            ddlProductlst.SelectedValue = oQuote.ProductCode
            ddlProductlst.Enabled = False
        End Sub
        ''' <summary>
        ''' Show/Hide of the Lapse button
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub EnableBtnLapseQuote()

            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim ddlLapseCancelReason As NexusProvider.LookupList = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__LAPSECANCELREASON"), NexusProvider.LookupList)
            Dim txtLapseCancelDate As TextBox = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__LAPSECANCELDATE"), TextBox)
            Dim calLapseDate As Object = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("calLapseDate"), Object)
            Dim btnLapseQuote As LinkButton = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("btnLapseQuote"), LinkButton)
            Dim VldLapseCancel As CustomValidator = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("VldLapseCancel"), CustomValidator)


            'if Controls are not available in the page
            If ddlLapseCancelReason Is Nothing Or txtLapseCancelDate Is Nothing _
            Or calLapseDate Is Nothing Or VldLapseCancel Is Nothing Or btnLapseQuote Is Nothing Then
                Exit Sub
            End If

            'in case of the ReQUte mode only
            If Session(CNQuoteMode) = QuoteMode.ReQuote Then
                ddlLapseCancelReason.Enabled = True
                txtLapseCancelDate.Enabled = True
                calLapseDate.Enabled = True
                btnLapseQuote.Visible = True
                VldLapseCancel.Enabled = True
            Else
                ddlLapseCancelReason.Enabled = False
                txtLapseCancelDate.Enabled = False
                calLapseDate.Enabled = False
                VldLapseCancel.Enabled = False
            End If
        End Sub

        ''' <summary>
        ''' AgencyCancelled
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub AgencyCancelled()
            Dim oAgentSettings As NexusProvider.AgentSettings = Nothing
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim hdnAgent As HiddenField = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__AGENT"), HiddenField)
            Dim txtCoverstrDate As TextBox = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__COVERSTARTDATE"), TextBox)
            Dim oPOLICYHEADER__BUSINESSTYPE As NexusProvider.LookupList = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__BUSINESSTYPE"), NexusProvider.LookupList)
            If hdnAgent Is Nothing Then
                Exit Sub
            End If

            If String.IsNullOrEmpty(hdnAgent.Value) Then
                If oQuote.Agent Is Nothing OrElse CInt(oQuote.AgentKey) = 0 Then
                    Exit Sub
                End If
                hdnAgent.Value = Trim(oQuote.Agent)
            End If
            GetAgentSettingsCall(oAgentSettings, hdnAgent.Value)
            If oAgentSettings IsNot Nothing Then
                Dim dtAgencyCancellationDate As DateTime
                Dim dtDateToValidateForAgencyCancellation As DateTime
                Dim oValidateCancelledAgentOrBroker As New NexusProvider.OptionTypeSetting
                oValidateCancelledAgentOrBroker = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 1040)

                If Not oValidateCancelledAgentOrBroker.OptionValue Is Nothing Then
                    If oValidateCancelledAgentOrBroker.OptionValue = "1" Then 'CoverStartDate
                        If Not txtCoverstrDate Is Nothing AndAlso Not String.IsNullOrEmpty(txtCoverstrDate.Text) Then
                            dtDateToValidateForAgencyCancellation = CDate(txtCoverstrDate.Text)
                        Else
                            dtDateToValidateForAgencyCancellation = oQuote.CoverStartDate
                        End If
                    ElseIf oValidateCancelledAgentOrBroker.OptionValue = "0" Then 'TransactionDate
                        dtDateToValidateForAgencyCancellation = DateTime.Now
                    End If
                Else
                    dtDateToValidateForAgencyCancellation = DateTime.Now
                End If
                Dim sScriptBuilder As New StringBuilder

                sScriptBuilder.Append("function AgencyCancellation(){")
                If oAgentSettings.AgencyCancellationDate <> DateTime.MinValue AndAlso oAgentSettings.AgencyCancellationDate.ToString("MM/dd/yyyy") <> "12/29/1899" Then
                    dtAgencyCancellationDate = oAgentSettings.AgencyCancellationDate

                    sScriptBuilder.Append("var modifiedAgent = document.getElementById('" & hdnAgent.UniqueID.Replace("$", "_") & "').value;")
                    If oValidateCancelledAgentOrBroker.OptionValue = "0" Then
                        sScriptBuilder.Append("var modifiedcovertdate = ('" & dtDateToValidateForAgencyCancellation.ToString("dd/MM/yyyy") & "');")
                    Else
                        sScriptBuilder.Append("var modifiedcovertdate = (document.getElementById('" & txtCoverstrDate.UniqueID.Replace("$", "_") & "').value);")
                    End If
                    sScriptBuilder.Append("var agentcancelleddate = ('" & dtAgencyCancellationDate.ToString("dd/MM/yyyy") & "');")

                    sScriptBuilder.Append("var businesstype = (document.getElementById('" & oPOLICYHEADER__BUSINESSTYPE.UniqueID.Replace("$", "_") & "').value);")

                    sScriptBuilder.Append("var datecompare=fn_DateDiff(agentcancelleddate,modifiedcovertdate);")

                    Select Case UCase(oQuote.InsuranceFileTypeCode)
                        Case Nothing, "QUOTE", "RENEWAL"
                            sScriptBuilder.Append("if ((datecompare>=0) && (businesstype=='AGENCY')) {")
                            sScriptBuilder.Append("alert('" & GetLocalResource("msgAgencyCancelledQuote", "Agency cancelled - No new transactions can be placed through this agent.") & "');return false;}")
                        Case Else
                            sScriptBuilder.Append("if ((datecompare>=0) && (businesstype=='AGENCY')) {")
                            sScriptBuilder.Append("return confirm('" & GetLocalResource("msgAgencyCancelled", "Agent Cancelled - do you still wish to proceed ?") & "');}")
                    End Select
                Else
                    sScriptBuilder.Append("return true;")
                End If

                sScriptBuilder.Append("}")

                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "AgencyCancellation", _
"<script language=""javascript"" type=""text/javascript"">" & sScriptBuilder.ToString() & "</script>")
                Session(CNAgentCancelled) = True
            End If
        End Sub



        ''' <summary>
        ''' validating Anniversary Date
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="args"></param>
        ''' <remarks></remarks>
        Protected Sub ValidateAnniversaryDate(ByVal source As Object,
                                              ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
            'Retrieve value from hidden field hfIsTrueMonthlyProduct and IsUnifiedRenewalDayReadOnly
            Dim hdnIsTrueMonthlyProduct As HiddenField = CType(oMaster.FindControl("hfIsTrueMonthlyProduct"), 
                                                               HiddenField)
            Dim hdnIsUnifiedRenewalDayReadOnly As HiddenField =
                    CType(oMaster.FindControl("hfIsUnifiedRenewalDayReadOnly"), HiddenField)

            If (hdnIsTrueMonthlyProduct.Value = "1" And hdnIsUnifiedRenewalDayReadOnly.Value = "1") Then
                'Retrieve selected index of Dropdown List
                Dim ddlUnifiedRenewalDay As DropDownList = CType(oMaster.FindControl("POLICYHEADER__UNIFIEDRENEWALDAY"), 
                                                                 DropDownList)
                'Retrieve selected value of Dropdown List
                Dim nUnifiedRenewalDay As Integer = CInt(ddlUnifiedRenewalDay.SelectedValue)
                'Retrieve Anniversary Date Control
                Dim txtAnniversaryDate As TextBox = CType(oMaster.FindControl("POLICYHEADER__ANNIVERSARY"), TextBox)
                'Retrieve Renewal Date Control
                Dim txtRenewalDate As TextBox = CType(oMaster.FindControl("POLICYHEADER__RENEWAL"), TextBox)

                Dim dtAnniversaryDate As Date = CDate(txtAnniversaryDate.Text)
                Dim dtRenewalDate As Date = CDate(txtRenewalDate.Text)

                Dim nAnniversaryDay As Integer = dtAnniversaryDate.Day

                If (nAnniversaryDay <> nUnifiedRenewalDay) Then
                    'Show alert message

                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowValidation",
                                                        "alert('" &
                                                        GetGlobalResourceObject("Resource", "AnniversaryDayRenewalDay") &
                                                        "');", True)
                    'On OK change day of Anniversary back to Unified Renewal Day (leave month and year of Anniversary date unchanged)
                    txtAnniversaryDate.Text =
                        (nUnifiedRenewalDay & "/" & dtAnniversaryDate.Month & "/" & dtAnniversaryDate.Year) _
                    ' DD/MM/YYYY Format.
                Else
                    If (dtAnniversaryDate < dtRenewalDate) Then
                        'Show alert message
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowValidation",
                                                            "alert('" &
                                                            GetGlobalResourceObject("Resource",
                                                                                    "AnniversaryDayGreaterRenewalDay") &
                                                            "');", True)
                        'On OK return to Policy screen and change Anniversary date back to Default
                        txtAnniversaryDate.Text =
                            (dtRenewalDate.Day & "/" & dtRenewalDate.Month & "/" & dtRenewalDate.Year)
                    End If
                End If
            End If
            args.IsValid = True
        End Sub
#End Region

#Region "Dynamic Logic Helper Methods"

        ''' <summary>
        ''' Sum the values found in child objects at a specific location in the risk data
        ''' </summary>
        ''' <param name="sPath">Path to the nodes in the xml which contain the attribute to be summed</param>
        ''' <param name="sAttribute">Attribute to be summed</param>
        ''' <returns>Returns the sum of values found in the attribute from the path specified</returns>
        ''' <remarks></remarks>
        Protected Function GetColumnTotal(ByVal sPath As String, ByVal sAttribute As String) As Double
            Return GetColumnTotal(sPath, sAttribute, Nothing, Nothing)
        End Function

        ''' <summary>
        ''' Sum the values found in child objects at a specific location in the risk data, 
        ''' Filters results by comparing an attribute to a value
        ''' </summary>
        ''' <param name="sPath">Path in the XML to the child nodes</param>
        ''' <param name="sAttribute">Attribute which is summed</param>
        ''' <param name="sCheckAttribute">Attribute to compare</param>
        ''' <param name="sCompareValue">Value to compare the check attribute to</param>
        ''' <returns>Returns the sum of values found in the attribute from the path specified</returns>
        ''' <remarks></remarks>
        Protected Function GetColumnTotal(ByVal sPath As String, ByVal sAttribute As String, ByVal sCheckAttribute As String, ByVal sCompareValue As String) As Double
            Dim iTotal As Double = 0 ' keep track of the total
            Dim dCurrent As Double

            Dim xmlXMLNodes As XmlNodeList = GetXmlNodes(sPath)
            'check that we've found something in the XML
            If xmlXMLNodes IsNot Nothing Then
                For Each xmlNode1 As XmlNode In xmlXMLNodes
                    'check the attibute is there or we'll get an error fetching it's value
                    If xmlNode1.Attributes(sAttribute) IsNot Nothing Then
                        'check the attribute contains a double
                        If Double.TryParse(xmlNode1.Attributes(sAttribute).Value, dCurrent) Then
                            'we've got a numeric in the attribute, so we can add this to the total
                            If sCompareValue IsNot Nothing Then
                                'check the  CheckAttribute exists in the node, or we'll get an error fetching the value
                                If xmlNode1.Attributes(sCheckAttribute) IsNot Nothing Then
                                    'check the attribute is equal to the compare value, only add to total if they are equal
                                    If xmlNode1.Attributes(sCheckAttribute).Value = sCompareValue Then
                                        iTotal += dCurrent
                                    End If
                                End If
                            Else
                                'no need to compare, just keep running total
                                iTotal += dCurrent
                            End If
                        End If
                    End If
                Next
            End If

            Return iTotal
        End Function

        ''' <summary>
        ''' Counts the number of child objects at a specified path
        ''' </summary>
        ''' <param name="sPath">Path to the child objects</param>
        ''' <returns>The number of child objects at a specified path</returns>
        ''' <remarks></remarks>
        Protected Function GetObjectCount(ByVal sPath As String) As Integer
            Return GetObjectCount(sPath, Nothing, Nothing)
        End Function

        ''' <summary>
        ''' Counts the number of child objects at a specified path where some attribute matches a compare value
        ''' </summary>
        ''' <param name="sPath">Path to the child objects</param>
        ''' <param name="sCheckAttribute">Attribute to check</param>
        ''' <param name="sCompareValue">Value to compare to</param>
        ''' <returns>The number of child objects at a specified path</returns>
        ''' <remarks>For example could count the number of insured travellers who are male on a travel policy</remarks>
        Protected Function GetObjectCount(ByVal sPath As String, ByVal sCheckAttribute As String, ByVal sCompareValue As String) As Integer
            Dim iTotal As Integer = 0 ' keep track of the count

            Dim xmlXMLNodes As XmlNodeList = GetXmlNodes(sPath)
            'check that we've found something in the XML
            If xmlXMLNodes IsNot Nothing Then
                For Each xmlNode1 As XmlNode In xmlXMLNodes
                    If sCompareValue IsNot Nothing Then
                        'check the attribute is equal to the compare value, only increment if they are equal
                        If xmlNode1.Attributes(sCheckAttribute).Value = sCompareValue Then
                            iTotal += 1
                        End If
                    Else
                        'no need to compare, just count the objects
                        iTotal += 1
                    End If
                Next
            End If

            Return iTotal
        End Function

        ''' <summary>
        ''' Gets a single value from the risk data
        ''' </summary>
        ''' <param name="sPath">Path in XML to the required attribute</param>
        ''' <param name="sAttribute">Attribute value to fetch</param>
        ''' <returns></returns>
        ''' <remarks>If path specified contains more than one node, then the value from the first node is returned</remarks>
        Protected Function GetValue(ByVal sPath As String, ByVal sAttribute As String) As String
            Dim xNode As XmlNodeList = GetXmlNodes(sPath)
            If xNode IsNot Nothing Then
                If xNode.Item(0) IsNot Nothing Then
                    If xNode.Item(0).Attributes(sAttribute) IsNot Nothing Then
                        Return xNode.Item(0).Attributes(sAttribute).Value.ToString
                    End If
                End If
            End If
            'there's nothing to return, so return nothing
            Return Nothing
        End Function

        ''' <summary>
        ''' Returns an XML node list from the specified path in the current risk dataset
        ''' </summary>
        ''' <param name="sPath"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function GetXmlNodes(ByVal sPath As String) As XmlNodeList

            Dim oQuote As NexusProvider.Quote = System.Web.HttpContext.Current.Session(CNQuote)
            Dim srDataset As New System.IO.StringReader(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
            Dim xmlTR As New XmlTextReader(srDataset)
            Dim Doc As New XmlDocument
            Doc.Load(xmlTR)
            xmlTR.Close()

            Try
                'return the nodes from the selected path, use current datamodel code from session
                Return Doc.SelectNodes("//" & Session.Item(CNDataModelCode) & "_POLICY_BINDER/" & sPath)
            Catch ex As Exception
                'an invalid path will result in an exception, return nothing instead
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Show specified tab ID in tab index control
        ''' </summary>
        ''' <param name="TabID">ID of tab, as found in quick / fullquote.config</param>
        ''' <remarks></remarks>
        Public Sub ShowTab(ByVal TabID As String)
            'get the hashtable of tabs from session
            Dim oTabState As Hashtable = CType(Session.Item(CNTabState & "_" & sTabIndexControlID), Hashtable)
            If oTabState Is Nothing Then
                'need to create a new hashtable
                oTabState = New Hashtable
            End If
            If oTabState.Contains(TabID) Then
                'set the value for this to true to make it visible regardless of the setting in config
                oTabState(TabID) = True
            Else
                'add an entry to the hash table to ensure this tab is visible
                oTabState.Add(TabID, True)
            End If
            'put the hashtable into session
            Session(CNTabState & "_" & sTabIndexControlID) = oTabState
        End Sub

        ''' <summary>
        ''' Show specified tab ID in tab index control
        ''' </summary>
        ''' <param name="TabID">ID of tab, as found in quick / fullquote.config</param>
        ''' <remarks></remarks>
        Public Sub HideTab(ByVal TabID As String)
            'get the hashtable of tabs from session
            Dim oTabState As Hashtable = CType(Session.Item(CNTabState & "_" & sTabIndexControlID), Hashtable)
            If oTabState Is Nothing Then
                'need to create a new hashtable
                oTabState = New Hashtable
            End If
            If oTabState.Contains(TabID) Then
                'set the value for this to false to hide tab
                oTabState(TabID) = False
            Else
                'add an entry to the hash table to ensure this tab is hidden
                oTabState.Add(TabID, False)
            End If
            'put the hashtable into session
            Session(CNTabState & "_" & sTabIndexControlID) = oTabState
        End Sub

#End Region

#Region "WPR 8_11 Changes"

        Sub SaveQuote()
            Dim oWebService As NexusProvider.ProviderBase
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)

            'call SAM method SaveRisk to save the risk to DB
            oWebService.SaveRisk(oQuote, Session(CNCurrentRiskKey), Nothing)
            If CType(Session(CNIsTransferQuoteRequired), Boolean) = True Then
                TransferQuoteToRealParty()
            End If
            'Mae changes on 27-12-2011 as SaveQuote was failing during MTA and MTC
            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
            Dim oQuoteVersionSetting As NexusProvider.OptionTypeSetting
            If (oQuote.QuoteStatusKey = QuoteStatusType.AgentPending And oUserDetails.Key = 0) Then
                If Session(CNQuoteMode) = QuoteMode.FullQuote AndAlso Session(CNRenewal) Is Nothing AndAlso Session(CNMTAType) Is Nothing Then
                    Dim sQuoteVersioning As String = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsQuoteVersioning, NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, "")
                    If (Not String.IsNullOrEmpty(sQuoteVersioning) AndAlso sQuoteVersioning.Trim = "1") Then
                        oQuote.QuoteStatusKey = QuoteStatusType.Pending
                        oWebService.UpdateQuoteStatus(oQuote)
                        Session(CNQuote) = oQuote
                    End If
                End If
            End If
        End Sub


        'Sub SaveQuote()
        '    'redirecting the user to Client details page if he clicks on Save Quote button
        '    ''need to check if the Login User is an Agent/Direct Registered Client
        '    Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        '    Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
        '    If CType(Session.Item(CNLoginType), LoginType) = LoginType.Agent Then

        '        Dim oParty As NexusProvider.BaseParty = Session(CNParty)
        '        Select Case True
        '            Case TypeOf oParty Is NexusProvider.PersonalParty
        '                Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName, False)
        '            Case TypeOf oParty Is NexusProvider.CorporateParty
        '                Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName, False)
        '        End Select

        '        oParty = Nothing
        '    Else
        '        Response.Redirect(oPortal.ClientStartPage.Trim, False)
        '    End If

        'End Sub
        Public Sub SaveQuoteButton(ByVal sender As Object, ByVal e As System.EventArgs)
            SaveQuote()
        End Sub

        Public Sub BuyButton(ByVal sender As Object, ByVal e As System.EventArgs)

            If CType(sender, Button).Text = GetLocalResource("lblIssued", "Issue") Then  'WPR63
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                oQuote.QuoteStatusKey = QuoteStatusType.Issued
                oWebService.UpdateQuoteStatus(oQuote)
                Session(CNQuote) = oQuote
                '[start]changes for WPR 73_74
                'If an underwriter (non-agency user) is logged
                Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                Dim sDesc As String = String.Empty
                Dim sTask As String = String.Empty
                Dim sTaskGroup As String = String.Empty

                If oUserDetails IsNot Nothing AndAlso oUserDetails.Key = 0 AndAlso oQuote.ContactUserName <> "" Then
                    If (Session(CNQuoteMode) = QuoteMode.FullQuote) Then 'If NB
                        sTask = "UNDERNB"
                        sTaskGroup = "UNDER"
                    ElseIf (Session(CNQuoteMode) = QuoteMode.MTAQuote) Then
                        sTask = "UNDERMTA"
                        sTaskGroup = "UNDER"
                    End If
                    sDesc = GetLocalResource("lblTaskIssued", "Your Quote with Ref No. XXXXX is Issued")
                    sDesc = sDesc.Replace("XXXXX", oQuote.InsuranceFileRef)
                    CreateTask(oQuote, sDesc, sTask, sTaskGroup)
                End If
                ''Will open the Modal/SendMail.aspx pop-up to send the mail
                'SendMail()
                'Exit Sub
                ''[end]changes for WPR 73_74
                Exit Sub
            ElseIf (CType(sender, Button).Text = GetLocalResource("lblFinish", "Finish")) Then
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                'calling the sam methods
                oQuote.QuoteStatusKey = QuoteStatusType.AgentComplete
                oWebService.UpdateQuoteStatus(oQuote)
                Session(CNQuote) = oQuote
                Exit Sub
            End If


            If Session(CNCommissionGreaterThanPremium) IsNot Nothing AndAlso Session(CNCommissionGreaterThanPremium) = True Then
                Dim cstCommissionGreaterThanPremium As New CustomValidator
                cstCommissionGreaterThanPremium.IsValid = False
                'look for a validation message in the page resources, but if there is not one defined add a default message
                cstCommissionGreaterThanPremium.ErrorMessage = GetLocalResource("lbl_CommissionGreaterThanPremium", "Total Commission is more than the Premium. Please amend before proceedings.")
                cstCommissionGreaterThanPremium.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                'add the validator to the page, this will have the effect of making the page invalid
                Page.Validators.Add(cstCommissionGreaterThanPremium)

                'vldCommissionGreaterThanPremium.Enabled = True
                'vldCommissionGreaterThanPremium.IsValid = False
            End If
            If Page.IsValid Then
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oProductConfig As Config.Product = oNexusConfig.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                If CheckRefer() = True Then
                    Session(CNQuoteMode) = QuoteMode.FullQuote
                    Response.Redirect(AppSettings("WebRoot") & "referred.aspx")
                ElseIf CheckDecline() = True Then
                    Session(CNQuoteMode) = QuoteMode.FullQuote
                    Response.Redirect(AppSettings("WebRoot") & "declined.aspx")
                Else
                    If Session(CNMTAType) <> MTAType.CANCELLATION Then
                        oWebService.GetHeaderAndRisksByKey(oQuote)
                        Dim bRiskDeleted As Boolean = False
                        Dim iTotalRiskCount As Integer = oQuote.Risks.Count
                        For iTempVar As Integer = 0 To iTotalRiskCount - 1
                            If bRiskDeleted = True Then
                                bRiskDeleted = False
                                iTempVar -= 1
                            End If
                            If iTempVar < iTotalRiskCount AndAlso oQuote.Risks(iTempVar).IsRisk = False Then
                                oWebService.DeleteRisk(oQuote, iTempVar)
                                oQuote.Risks.Remove(iTempVar)
                                bRiskDeleted = True
                                iTotalRiskCount = oQuote.Risks.Count
                            End If
                        Next
                        'Update Session(CNQuote) with only selected risk
                        Session(CNQuote) = oQuote
                    End If

                    Dim dTatalPremium As Decimal
                    If oQuote.Risks.Count > 0 Then
                        dTatalPremium = oQuote.GrossTotal
                    End If

                    If Session(CNRenewal) Is Nothing AndAlso Session(CNMTAType) IsNot Nothing AndAlso dTatalPremium < 0 Then
                        ' Begin - WPR VB 64 - Media Type Status 
                        Dim CheckMediatypeStatusAtPolicyRefund As String = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, _
                                                        NexusProvider.ProductRiskOptions.CheckMediatypeStatusAtPolicyRefund, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing, oQuote.BranchCode).Trim()

                        If CheckMediatypeStatusAtPolicyRefund.Contains("1") Then
                            Dim oMediaTypeStatus As New NexusProvider.MediaTypeStatus
                            With oMediaTypeStatus
                                .InsuranceFileKey = oQuote.InsuranceFileKey
                                .LossDateSpecified = False
                            End With
                            oWebService.GetPolicyStatusForMediaTypeStatus(oMediaTypeStatus)
                            'SAM Return the False intead of True, if unclear fund exist then it retirn False or else true
                            If Not oMediaTypeStatus.IsUnclearedCashListExists Then
                                Dim cstMediaTypeStatus As New CustomValidator
                                cstMediaTypeStatus.IsValid = False
                                'look for a validation message in the page resources, but if there is not one defined add a default message
                                cstMediaTypeStatus.ErrorMessage = GetLocalResource("lbl_MediaTypeStatus_Error", "Please refer to accounts as the status of receipts is not cleared")
                                cstMediaTypeStatus.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                                'add the validator to the page, this will have the effect of making the page invalid
                                Page.Validators.Add(cstMediaTypeStatus)


                                'vldMediaTypeStatus.IsValid = False
                                Exit Sub
                            End If
                        End If
                        ' End - WPR VB 64 - Media Type Status 
                    End If

                    If (dTatalPremium < 0.0) And (Session(CNMTAType) <> MTAType.CANCELLATION) Then
                        Session(CNMode) = Mode.Edit
                        Session(CNQuoteInSync) = False
                        Session.Remove(CNOI)

                        'this will check in case of MTA Return Premium exists
                        ' which will check statements is set to true in web.config and then will redirect to staements page
                        If CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).ShowStatements = True Then
                            Response.Redirect("~/secure/Statements.aspx", False)
                            'else will redirect to transaction confirmation page directly
                        Else
                            Session(CNPaid) = True
                            Response.Redirect("~/secure/TransactionConfirmation.aspx", False)
                            'End If
                        End If
                    End If

                    'This will allow Zero Premium to be transacted in Case of NB/MTA/Renewals
                    If (dTatalPremium = 0.0) Then
                        Session(CNMode) = Mode.Edit
                        Session(CNQuoteInSync) = False
                        Session.Remove(CNOI)
                        Session(CNPaid) = True
                        Response.Redirect("~/secure/TransactionConfirmation.aspx", False)
                    End If

                    Response.Redirect("~/secure/Statements.aspx", False)
                End If
            End If

        End Sub

        Public Sub ChangePolicyButton(ByVal sender As Object, ByVal e As System.EventArgs)

            'Setting the mode as Edit,Quote mode as Mta Quote in case of Doing MTA on the Premium display page             '
            'Session(CNQuoteMode) = QuoteMode.MTAQuote
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Session.Remove(CNOI)
            Session(CNMode) = Mode.Edit
            Session(CNRenewal) = Nothing
            Session(CNInsuranceFileKey) = oQuote.InsuranceFileKey
            Session(CNQuoteMode) = QuoteMode.FullQuote
            Session(CNQuoteInSync) = False
            Session(CNMtaReasonSelected) = Nothing
            Response.Redirect("~/secure/MTAReason.aspx", False)

        End Sub

        Public Sub CancelButton1(ByVal sender As Object, ByVal e As System.EventArgs)
            '' AddMtaQuote, UpdateMtaRisk has been already fired on the page load if CANCELLING the Policy
            Dim oWebService As NexusProvider.ProviderBase
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oMtaQuote As New NexusProvider.MTA()

            oWebService = New NexusProvider.ProviderManager().Provider

            Dim oPolicySummary As NexusProvider.PolicySummary
            oQuote = Session(CNQuote)
            Dim oPayment As NexusProvider.Payment = Nothing

            oPolicySummary = New NexusProvider.PolicySummary(oQuote.Reference)
            oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.Cash)
            oPolicySummary = oWebService.BindQuote(oQuote.InsuranceFileKey, oPayment, oQuote.TimeStamp, Nothing, Nothing, "MTC")
            '' now finally calling BindMtaQuote then redirecting to PersonalClientDetails page


            'Need to find the type of client whether personal or corporate and then redirect it to the right page
            Dim oParty As NexusProvider.BaseParty = Session(CNParty)
            Try
                'Removing the Sessions as the cancellation has been already done in this stage.
                Session.Remove(CNAmountToPay)
                Session.Remove(CNPayment)
                Session.Remove(CNOI)
                Session.Remove(CNMTAType)
                Session.Remove(CNMode)
                Session.Remove(CNQuote)
                Session.Remove(CNInsuranceFileKey)

                Select Case True
                    Case TypeOf oParty Is NexusProvider.PersonalParty
                        Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName, False)
                    Case TypeOf oParty Is NexusProvider.CorporateParty
                        Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName, False)
                End Select
            Finally
                'Removing the references of obejcts
                oWebService = Nothing
                oQuote = Nothing
                oPolicySummary = Nothing
                oPayment = Nothing
            End Try

        End Sub
        'Add a event for MarkedQuote entry as this is not been handle through SAM
        Public Sub AddEventForMarkedQuote()
            Dim oEventDetails As New NexusProvider.EventDetails
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            With oEventDetails
                .EventDate = Now()
                .InsuranceFileKey = oQuote.InsuranceFileKey
                .InsuranceFileKeySpecified = True
                .InsuranceFolderKey = oQuote.InsuranceFolderKey
                .InsuranceFolderKeySpecified = True
                .PartyKey = oQuote.PartyKey
                .RtfText = "Quote Marked For Collection"
                .UserName = Session(CNLoginName)
                .EventTypeKey = 5
                .EventLogSubjectKey = 1
            End With

            oWebService.AddEvent(oEventDetails)
        End Sub
        'This code will unmark the Quote if already marked 
        Public Sub CallUnmarkQuote()
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            If oQuote.MarkedQuoteForCollection = True Then
                oQuote.MarkedQuoteForCollection = False
                oWebService.UpdateQuotev2(oQuote)
                Session(CNQuote) = oQuote
            End If
        End Sub
        'end



        Public Sub Lapse(ByVal sender As Object, ByVal e As System.EventArgs)
            Response.Redirect("PolicyLapsed.aspx", False)
        End Sub


        Public Sub Print(ByVal sender As Object, ByVal e As System.EventArgs)


            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sDocument As String = String.Empty
            Dim oDocuments As Config.Documents = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork) _
                              .Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode) _
                              .Documents
            Dim sDocumentDirName As String = oDocuments.Location
            Dim oRenewalStatus As NexusProvider.RenewalStatus
            oRenewalStatus = oWebService.GetRenewalStatus(oQuote.InsuranceFileKey)

            Try
                If oRenewalStatus.RenewalStatusTypeDescription = sAwaiting_Manual_Preview Then
                    oWebService.UpdateRenewalStatus(oQuote, "AutoReview")
                    oRenewalStatus = oWebService.GetRenewalStatus(oQuote.InsuranceFileKey)
                End If
                If oRenewalStatus.RenewalStatusTypeDescription = sAwaiting_Renewal_Notice Then
                    sDocument = oWebService.GenerateInvite(NexusProvider.DocumentType.PDF, True, oQuote, sDocumentDirName, Nothing)
                End If
                Dim oDocumentControl As Object = oMaster.FindControl("document.ascx")
                If oDocumentControl IsNot Nothing Then
                    oDocumentControl.Visible = True
                End If




                Dim obtnPrint As Object = oMaster.FindControl("btnPrint")
                If obtnPrint IsNot Nothing Then
                    obtnPrint.Visible = False
                End If
                Dim obtnBuy As Object = oMaster.FindControl("btnBuy")
                If obtnBuy IsNot Nothing Then
                    obtnBuy.Visible = True
                End If
                Dim obtnMarkQuoteForCollection As Object = oMaster.FindControl("btnMarkQuoteForCollection")
                If obtnMarkQuoteForCollection IsNot Nothing Then
                    obtnMarkQuoteForCollection.Visible = True
                End If

                'Print_Renewaldocument.Visible = True
                'btnPrint.Visible = False
                'btnBuy.Visible = True
                'btnMarkQuoteForCollection.Visible = True

                'to update the oQuote since Quote status has been updated
                oQuote = oWebService.GetHeaderAndSummariesByKey(oQuote.InsuranceFileKey)
                For iCount As Integer = 0 To oQuote.Risks.Count - 1
                    oWebService.GetRisk(oQuote.Risks(iCount).Key, iCount, oQuote)
                Next
                oWebService.GetHeaderAndRisksByKey(oQuote)
                'TO retreive the selected status after btn Print
                For iCount As Integer = 0 To oQuote.Risks.Count - 1
                    If oQuote.Risks(iCount).IsRisk = True Then
                        oQuote.Risks(iCount).IsRisk = True
                    Else
                        oQuote.Risks(iCount).IsRisk = False
                    End If
                Next
            Catch ex As NexusProvider.NexusException
                '  If ex.Errors(0).Code <> "1000093" Then

                Dim cstChkRenwalDoc As New CustomValidator
                cstChkRenwalDoc.IsValid = False
                'look for a validation message in the page resources, but if there is not one defined add a default message
                cstChkRenwalDoc.ErrorMessage = GetLocalResource("btn_vldChkRenwalDoc", "Please configure the document for Renewal Invite like RNC1 (here, 1 is product code)")
                cstChkRenwalDoc.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                'add the validator to the page, this will have the effect of making the page invalid
                Page.Validators.Add(cstChkRenwalDoc)

                'vldChkRenwalDoc.Enabled = True
                'vldChkRenwalDoc.IsValid = False
                'End If
            Finally
                Session(CNQuote) = oQuote
            End Try

            Dim HTable As New Hashtable() 'to hold the document details
            Dim odocument As NexusProvider.DocumentCollection
            odocument = oWebService.GetDocumentList(oQuote.InsuranceFolderKey)

            Dim odocumentstr As New NexusProvider.Document

            'check if there is any object of document type returned
            If Not odocument Is Nothing Then
                If odocument.Count > 0 Then

                    'need to store the unique record into HashTable with the highest DocNum
                    Dim icount As Integer = 0
                    'run the loop till the count reaches to the total documents present in the policy
                    For icount = 0 To odocument.Count - 1
                        'if Exist, then update the data into Hash Table
                        If odocument.Item(icount).DocDescription.Contains("Renewal") Then
                            HTable.Item(odocument.Item(icount).DocDescription) = odocument.Item(icount).DocNum
                        End If
                    Next

                    'displaying the data from the Hash Table 
                    Dim HData As DictionaryEntry
                    For Each HData In HTable
                        Dim h1 As New System.Web.UI.WebControls.HyperLink
                        Session("SelectedDocId") = HData.Value
                        h1.NavigateUrl = "~/secure/document.aspx"
                        h1.Target = "_blank"
                        h1.Text = HData.Key
                        Dim tRow As New TableRow()

                        Dim otblDocs As Object = oMaster.FindControl("tblDocs")
                        If otblDocs IsNot Nothing Then
                            otblDocs.Rows.Add(tRow)
                        End If

                        'tblDocs.Rows.Add(tRow)
                        Dim tCell As New TableCell
                        tRow.Cells.Add(tCell)
                        tCell.Controls.Add(h1)
                    Next
                End If
            End If

        End Sub

        Public Sub vldChkStatus(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
            Dim cstChkStatus As New CustomValidator
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            For iTempVar As Integer = 0 To oQuote.Risks.Count - 1
                If oQuote.Risks IsNot Nothing Then
                    If oQuote.Risks(iTempVar).IsRisk = True Then
                        args.IsValid = True
                        Exit For
                    Else
                        args.IsValid = False
                    End If
                End If
            Next

            'Chekc Quote Status
            If args.IsValid = True Then
                Dim bFound As Boolean = False
                If oQuote.Risks IsNot Nothing Then
                    For iCount As Integer = 0 To oQuote.Risks.Count - 1
                        If IsDataSetNexusQuoteStatus(iCount) = True AndAlso Session(CNQuoteMode) = QuoteMode.FullQuote _
                                And NexusQuoteStatus(oQuote.Risks(iCount)) = False And oQuote.Risks(iCount).IsRisk = True _
                                And args.IsValid = True And Session(CNRenewal) Is Nothing Then
                            bFound = True
                            Exit For
                        End If
                    Next
                End If
                If bFound = True Then


                    cstChkStatus.IsValid = False
                    'look for a validation message in the page resources, but if there is not one defined add a default message
                    cstChkStatus.ErrorMessage = GetLocalResource("lbl_Please_Check", "At least one quoted risk on this policy must be selected to make it live.")
                    cstChkStatus.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                    'add the validator to the page, this will have the effect of making the page invalid
                    Page.Validators.Add(cstChkStatus)


                    'vldChkStatus.ErrorMessage = GetLocalResourceObject("Err_FullQuote")
                    args.IsValid = False
                End If
            End If

            'if Risk is UNQUOTED then Buy Now should throw a message
            If args.IsValid = True Then
                For iTempVar As Integer = 0 To oQuote.Risks.Count - 1
                    If oQuote.Risks IsNot Nothing Then
                        If oQuote.Risks(iTempVar).IsRisk = True AndAlso oQuote.Risks(iTempVar).StatusCode.Trim.ToUpper <> "QUOTED" Then

                            cstChkStatus.IsValid = False
                            'look for a validation message in the page resources, but if there is not one defined add a default message
                            cstChkStatus.ErrorMessage = GetLocalResource("lbl_Please_Check", "At least one quoted risk on this policy must be selected to make it live.")
                            cstChkStatus.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                            'add the validator to the page, this will have the effect of making the page invalid
                            Page.Validators.Add(cstChkStatus)

                            'vldChkStatus.ErrorMessage = GetLocalResourceObject("Err_UnQuoted")
                            args.IsValid = False
                            Exit For
                        End If
                    End If
                Next
            End If
        End Sub


        Public Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

            If Request.QueryString("ViewPolicy") IsNot Nothing Then
                CMS.Library.Frontend.Functions.SetTheme(Page, ConfigurationManager.AppSettings("ModalPageTemplate"))
            End If

            'Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "LapseConfirmation", _
            '    "<script language=""JavaScript"" type=""text/javascript"">function LapseConfirmation(){return confirm('" & GetLocalResourceObject("msg_ConfirmLapsePolicy").ToString() & "');}</script>")
            'Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "MarkedConfirmation", _
            '   "<script language=""JavaScript"" type=""text/javascript"">function MarkedConfirmation(){var IsConfirm; IsConfirm=confirm('" & GetLocalResourceObject("msg_MarkedConfirmation").ToString() & "');return IsConfirm;}</script>")

        End Sub

        Public Sub MarkQuoteForCollection(ByVal sender As Object, ByVal e As System.EventArgs)
            'this will Mark the Quote if Button gets visible and MarkedQuoteForCollection is false 
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            If oQuote.MarkedQuoteForCollection = False Then
                oQuote.MarkedQuoteForCollection = True
                oQuote.MarkedDateforCollection = Date.Now.Date
                oWebService.UpdateQuotev2(oQuote)
                'Add the Event for Updating the Marked Quote now as this is not handled in SAM
                AddEventForMarkedQuote()
                Session(CNQuote) = oQuote
                Dim obtnMarkQuoteForCollection As Object = oMaster.FindControl("btnMarkQuoteForCollection")
                If obtnMarkQuoteForCollection IsNot Nothing Then
                    obtnMarkQuoteForCollection.Visible = False
                End If

                'btnMarkQuoteForCollection.Visible = False
                SaveQuote()
            End If
        End Sub

        Private Sub GetScreens()
            Dim oPortal1 As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
            Dim sProductPath1() As String
            Dim oRiskType1 As NexusProvider.RiskType = Session(CNRiskType)
            sProductPath1 = CStr(Request.ApplicationPath & "/" & oNexusConfig.ProductsFolder) _
                .Split(Regex.Split("/", ""), StringSplitOptions.RemoveEmptyEntries)

            Dim oProduct1 As Nexus.Library.Config.Product = oPortal1.Products.GetProductByName(Server.UrlDecode(Request.Url.Segments( _
                sProductPath1.Length + 1).TrimEnd("/")))

            If oProduct1 Is Nothing Then
                Throw New Exception("Product can NOT be found")
            End If
            'retrieved DataModelCode does not match that in session so we've changed
            'product, so check we are on the first page as a new quote will be created

            Dim oProductConfig As Nexus.Library.Config.Product = oPortal1.Products.GetProductByName(Server.UrlDecode( _
                Request.Url.Segments(sProductPath1.Length + 1).TrimEnd("/")))
            'oPortal.Products.Product(CType(Session(CNQuote), NexusProvider.Quote).ProductCode)

            Dim sFolder As String = AppSettings("WebRoot") & oNexusConfig.ProductsFolder & "/" & oProductConfig.Name
            'This code will solve the purpose in case of Anonymous Agent when client is not selected
            'Session(CnriskType) is nothing so have to repopulate this here PN 60503
            If Session(CNParty) Is Nothing And oRiskType1 Is Nothing Then
                Dim oRisk As Config.RiskType = oProductConfig.RiskTypes.RiskType(0)
                Dim oRiskAnonymousType As New NexusProvider.RiskType
                oRiskAnonymousType.DataModelCode = oRisk.DataModelCode
                oRiskAnonymousType.Name = oRisk.Name
                oRiskAnonymousType.Path = oRisk.Path
                oRiskAnonymousType.RiskCode = oRisk.RiskCode
                Session(CNRiskType) = oRiskAnonymousType
                oRiskType1 = Session(CNRiskType)
            End If
            Dim sXMLPath As String = Server.MapPath(sFolder & "\" & oRiskType1.Path & "\")

            If oProductConfig.QuickQuoteConfig = String.Empty Then
                'No quickquote for product, so FullQuote
                Session.Item(CNQuoteMode) = QuoteMode.FullQuote
            Else
                If Session.Item(CNQuoteMode) Is Nothing Then
                    'No QuoteMode in session and quickquote product available, so QuickQuote
                    Session.Item(CNQuoteMode) = QuoteMode.QuickQuote
                Else
                    'Don't override QuoteMode as QQ and FQ are available for the product
                End If
            End If

            Select Case CType(Session.Item(CNQuoteMode), QuoteMode)
                Case QuoteMode.QuickQuote
                    sXMLPath = sXMLPath & oProductConfig.QuickQuoteConfig
                Case QuoteMode.FullQuote, QuoteMode.MTAQuote, QuoteMode.ReQuote
                    sXMLPath = sXMLPath & oProductConfig.FullQuoteConfig
            End Select

            Dim xmlds As New XmlDataSource
            xmlds.DataFile = sXMLPath
            xmlds.EnableCaching = False

            Dim Navigator As XPathNavigator
            Dim Doc1 As XPathDocument = New XPathDocument(sXMLPath)
            Navigator = Doc1.CreateNavigator()

            'check summary of risk and cover configure as risk screens
            Dim iSrc As XPathNodeIterator
            Dim iTab As XPathNodeIterator
            iSrc = Navigator.Select("/screens/screen")
            While (iSrc.MoveNext)
                iTab = Navigator.Select("/screens/screen/tab")
                While (iTab.MoveNext)
                    sSummaryOfRisk = iTab.Current.GetAttribute("summaryofrisk", String.Empty)
                    If sSummaryOfRisk.ToLower = "true" Then
                        sSummaryOfRiskURL = iTab.Current.GetAttribute("url", String.Empty)
                    End If
                    If sSummaryOfRisk.ToLower = "true" Then
                        Exit While
                    End If
                End While
                If sSummaryOfRisk.ToLower = "true" Then
                    Exit While
                End If
            End While

            iSrc = Navigator.Select("/screens/screen")
            While (iSrc.MoveNext)
                iTab = Navigator.Select("/screens/screen/tab")
                While (iTab.MoveNext)
                    sSummaryOfCover = iTab.Current.GetAttribute("summaryofcover", String.Empty)
                    If sSummaryOfCover.ToLower = "true" Then
                        sSummaryOfCoverURL = iTab.Current.GetAttribute("url", String.Empty)
                    End If
                    If sSummaryOfCover.ToLower = "true" Then
                        Exit While
                    End If
                End While
                If sSummaryOfCover.ToLower = "true" Then
                    Exit While
                End If
            End While

            iSrc = Navigator.Select("/screens/screen")
            While (iSrc.MoveNext)
                iTab = Navigator.Select("/screens/screen/tab")
                While (iTab.MoveNext)
                    sReferScreen = iTab.Current.GetAttribute("referredscreen", String.Empty)
                    If sReferScreen.ToLower = "true" Then
                        sReferScreenURL = iTab.Current.GetAttribute("url", String.Empty)
                    End If
                    If sReferScreen.ToLower = "true" Then
                        Exit While
                    End If
                End While
                If sReferScreen.ToLower = "true" Then
                    Exit While
                End If
            End While

            iSrc = Navigator.Select("/screens/screen")
            While (iSrc.MoveNext)
                iTab = Navigator.Select("/screens/screen/tab")
                While (iTab.MoveNext)
                    sDeclineScreen = iTab.Current.GetAttribute("declinedscreen", String.Empty)
                    If sDeclineScreen.ToLower = "true" Then
                        sDeclineScreenURL = iTab.Current.GetAttribute("url", String.Empty)
                    End If
                    If sDeclineScreen.ToLower = "true" Then
                        Exit While
                    End If
                End While
                If sDeclineScreen.ToLower = "true" Then
                    Exit While
                End If
            End While
        End Sub

        Public Sub RecalculateButton(ByVal sender As Object, ByVal e As System.EventArgs)
            If Page.IsValid Then
                If CType(Session(CNMode), Mode) = Mode.Review Then
                    Session(CNQuote) = Nothing
                    Session(CNQuoteMode) = Nothing
                    Session(CNRiskType) = Nothing
                    Session(CNQuoteInSync) = Nothing
                    Session(CNOI) = Session(CNCurrentOI)
                    Session(CNMode) = Session(CNCurrentMode)
                    Response.Redirect(Session(CNReturnURL), False)
                End If
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)

                Session(CNQuoteInSync) = True

                If Session(CNMode) <> Mode.View And CType(Session(CNMode), Mode) <> Mode.Review Then
                    WriteRisk()
                End If

                Dim sDatasetErrorMessages As String = String.Empty

                If Session(CNMode) <> Mode.View And CType(Session(CNMode), Mode) <> Mode.Review Then
                    sDatasetErrorMessages = ValidateDataset() 'need to validate the
                End If

                If sDatasetErrorMessages <> String.Empty Then
                    'create a new custom validator
                    'set it as invalid and set the error message property to the output from ValidateDataset
                    Dim cstInvalidDataset As New CustomValidator
                    cstInvalidDataset.IsValid = False
                    cstInvalidDataset.ErrorMessage = sDatasetErrorMessages
                    cstInvalidDataset.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                    Page.Validators.Add(cstInvalidDataset)
                End If
                If sDatasetErrorMessages = String.Empty Then
                    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey))
                    Session(CNQuote) = oQuote
                End If
                If sDatasetErrorMessages = String.Empty Then
                    'no error messages returned so call UpdateQuote
                    If Not String.IsNullOrEmpty(sNextPage) And iDepth > 1 Then
                        'Removal of USed OI from Hash Table
                        If Session(CNMode) <> Mode.View And Session(CNMode) <> Mode.Review Then
                            Dim oUsedOI As Collections.Stack = Session(CNOI)
                            If Session(CNNode) IsNot Nothing Then
                                Dim hCurrentNodeColl As New Hashtable()
                                hCurrentNodeColl = CType(Session(CNNode), Hashtable)

                                If hCurrentNodeColl.ContainsKey(oUsedOI.Peek().ToString) Then
                                    hCurrentNodeColl.Remove(oUsedOI.Peek().ToString)
                                    Session(CNNode) = hCurrentNodeColl
                                End If
                            End If

                            'Delete the usedOI from DeletedOI collection
                            If Session(CNDeletedNode) IsNot Nothing Then
                                Dim hDeletedNodeColl As New Hashtable()
                                hDeletedNodeColl = CType(Session(CNDeletedNode), Hashtable)

                                If hDeletedNodeColl.ContainsKey(oUsedOI.Peek().ToString) Then
                                    hDeletedNodeColl.Remove(oUsedOI.Peek().ToString)
                                    Session(CNDeletedNode) = hDeletedNodeColl
                                End If
                            End If

                            'Remove the UC attrinbute to identify the valid/invalid node
                            ResetUCElement(oUsedOI.Peek().ToString)
                        End If

                        ''call PrePageRedirect before we redirect to allow this to be overridden if required
                        'PrePageRedirect()
                        'Response.Redirect(sParentTab)
                    Else
                        If Session(CNMode) <> Mode.View And CType(Session(CNMode), Mode) <> Mode.Review Then
                            UpdateQuote()
                            'ElseIf sSummaryOfCover.ToLower = "false" And sSummaryOfRisk.ToLower = "false" Then
                            '    Response.Redirect("~/secure/Premiumdisplay.aspx")
                        End If
                    End If
                End If
            End If
        End Sub


#End Region

        ''' <summary>
        ''' Handles the OnClick event of Requote button on the risk page, which Calls the CopyQuote method and redirect to the summary of cover page. Needs to be manually
        ''' hooked up to a control on the risk page
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
#Region "WPR 63 Changes"
        Public Sub RequoteButton(ByVal sender As Object, ByVal e As System.EventArgs)
            ''UnMark the risk if it is already selected
            CallUnmarkQuote()
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Session(CNQuoteMode) = QuoteMode.ReQuote
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            'Session("TempInsuranceFileKey") = oQuote.InsuranceFileKey
            'Session("TempInsuranceFolderKey") = oQuote.InsuranceFolderKey
            'Dim oQuote As NexusProvider.Quote
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name
            Dim oRiskT As New NexusProvider.RiskType
            Dim bBrokerHavePendingVer As Boolean, bUWHavePendingVer As Boolean
            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
            Dim sURL As String
            If HttpContext.Current.Session.IsCookieless Then
                sURL = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/QuoteConfirmation.aspx?modal=true&Riskcheck=true&KeepThis=true&TB_iframe=true&height=300&width=750"
            Else
                sURL = AppSettings("WebRoot") & "/Modal/QuoteConfirmation.aspx?modal=true&Riskcheck=true&KeepThis=true&TB_iframe=true&height=300&width=750"
            End If

            If oUserDetails IsNot Nothing AndAlso oUserDetails.Key <> 0 Then
                bBrokerHavePendingVer = HasPendingQuote(True)
            ElseIf oUserDetails IsNot Nothing AndAlso oUserDetails.Key = 0 Then
                bUWHavePendingVer = HasPendingQuote()
            End If

            If bBrokerHavePendingVer Or bUWHavePendingVer Then
                'Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show", _
                '"<script language=""JavaScript"" type=""text/javascript"">tb_show( null,'" & sURL & "' , null);</script>")
                'Exit Sub
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show", _
                "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sURL & "' , null);});</script>")
                Exit Sub

            Else
                Dim sQuoteVersioning2 As String = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsQuoteVersioning, NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, "")
                If (Not String.IsNullOrEmpty(sQuoteVersioning2) AndAlso sQuoteVersioning2.Trim = "1" And Session(CNMTAType) Is Nothing And Session(CNRenewal) Is Nothing) Then
                    CopyQuote(bIsQuoteVersioning:=True)
                End If
                RedirectRiskPage()
            End If


            'DoQuoteConfirmation(oQuote.InsuranceFileKey, False)

        End Sub

        ''' <summary>
        ''' Handles the OnClick event of Details button on the risk page, which redirect to the summary of cover page. Needs to be manually
        ''' hooked up to a control on the risk page
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Sub DetailsButton(ByVal sender As Object, ByVal e As System.EventArgs)
            Session.Remove(CNTabState & "_" & sTabIndexControlID)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            ' Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
            Dim sProductPath() As String = CStr(Request.ApplicationPath & "/" & oNexusConfig.ProductsFolder).Split(Regex.Split("/", ""), StringSplitOptions.RemoveEmptyEntries)
            'Dim oProduct As Nexus.Library.Config.Product = oPortal.Products.GetProductByName(Server.UrlDecode(Request.Url.Segments(sProductPath.Length + 1).TrimEnd("/")))
            'Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name
            'Dim oRiskT As New NexusProvider.RiskType
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name
            Dim oRiskType As Config.RiskType
            Dim oRiskT As New NexusProvider.RiskType

            If oProduct Is Nothing Then
                Throw New Exception("Product can NOT be found")
            End If
            If oQuote.Risks(0).RiskCode Is Nothing Then
                oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(0).RiskTypeCode.Trim)
            Else
                oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(0).RiskCode)
            End If
            oRiskT.DataModelCode = oRiskType.DataModelCode
            oRiskT.Name = oRiskType.Name
            oRiskT.Path = oRiskType.Path
            oRiskT.RiskCode = oRiskType.RiskCode
            Session(CNRiskType) = oRiskT
            Dim sRiskFolder As String = sProductFolder & "/" & oRiskT.Path & "/"
            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)

            'new quote added to check if the quote is expired then will open in view-only mode
            Dim iGracePeriod As Integer
            Dim dExpiryDate As Date

            Session(CNOI) = Nothing
            If oQuote.QuoteExpiryDate = Date.MinValue Then

                iGracePeriod = IIf(GetQuoteGracePeriod(oQuote.ProductCode.Trim()) = "", 0, GetQuoteGracePeriod(oQuote.ProductCode.Trim()))
                dExpiryDate = oQuote.CoverStartDate.AddDays(iGracePeriod).ToShortDateString()
            Else
                dExpiryDate = oQuote.QuoteExpiryDate
            End If

            If (dExpiryDate < DateTime.Now) Then
                Session(CNMode) = Mode.View
                Response.Redirect(sRiskFolder & "/" & GetFirstRiskScreen(sRiskFolder & oProduct.FullQuoteConfig), False)

            ElseIf (oQuote.QuoteStatusKey = QuoteStatusType.AgentPending And oUserDetails.Key = 0) Then

                oQuote.QuoteStatusKey = QuoteStatusType.Pending
                oWebService.UpdateQuoteStatus(oQuote)
                Session(CNQuote) = oQuote

                Response.Redirect(sRiskFolder & "/" & GetFirstRiskScreen(sRiskFolder & oProduct.FullQuoteConfig), False)
            ElseIf ((oQuote.QuoteStatusKey = QuoteStatusType.Issued Or oQuote.QuoteStatusKey = QuoteStatusType.AgentComplete) And oUserDetails.Key = 0) Then
                Session(CNMode) = Mode.View
                Response.Redirect(sRiskFolder & "/" & GetFirstRiskScreen(sRiskFolder & oProduct.FullQuoteConfig), False)

            ElseIf (CType(Session(CNLoginType), LoginType) = LoginType.Agent And oUserDetails.Key > 0 And (oQuote.QuoteStatusKey = QuoteStatusType.AgentComplete Or oQuote.QuoteStatusKey = QuoteStatusType.Issued)) Then
                Session(CNMode) = Mode.View
                Response.Redirect(sRiskFolder & "/" & GetFirstRiskScreen(sRiskFolder & oProduct.FullQuoteConfig), False)
            ElseIf (oQuote.QuoteStatusKey = QuoteStatusType.Declined) Then
                Session(CNMode) = Mode.View
                Response.Redirect(sRiskFolder & "/" & GetFirstRiskScreen(sRiskFolder & oProduct.FullQuoteConfig), False)
            Else
                Response.Redirect(sRiskFolder & "/" & GetFirstRiskScreen(sRiskFolder & oProduct.FullQuoteConfig), False)
            End If
        End Sub

        ''' <summary>
        ''' Handles the OnClick event of Decline button on the risk page, which sets the quote status to decline. Needs to be manually
        ''' hooked up to a control on the risk page
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Sub DeclineButton(ByVal sender As Object, ByVal e As System.EventArgs) 'This method is not being used now as a seperate control "DeclineButton" has been created for the purpose
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            oQuote.QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.Declined
            oWebService.UpdateQuoteStatus(oQuote)
            Session(CNQuote) = oQuote
            AddEventForDeclinedQuote(oQuote)
            '[start]changes for WPR 73_74
            'If an underwriter (non-agency user) is logged
            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
            Dim sDesc As String = String.Empty
            Dim sTask As String = String.Empty
            Dim sTaskGroup As String = String.Empty

            If oUserDetails IsNot Nothing AndAlso oUserDetails.Key = 0 AndAlso oQuote.ContactUserName <> "" Then
                If (Session(CNQuoteMode) = QuoteMode.FullQuote) Then 'If NB
                    sTask = "UNDERNB"
                    sTaskGroup = "UNDER"
                ElseIf (Session(CNQuoteMode) = QuoteMode.MTAQuote) Then
                    sTask = "UNDERMTA"
                    sTaskGroup = "UNDER"
                End If
                sDesc = IIf(GetLocalResourceObject("lblTaskDecline") Is Nothing, "Your Quote with Ref No. XXXXX is Declined", GetLocalResourceObject("lblTaskDecline"))
                sDesc = sDesc.Replace("XXXXX", oQuote.InsuranceFileRef)
                CreateTask(oQuote, sDesc, sTask, sTaskGroup)
                'SendMailJob pending
            End If
            '[end]changes for WPR 73_74
        End Sub

        ''' <summary>
        ''' will add event for the declined quote
        ''' </summary>
        ''' <param name="oquote"></param>
        ''' <remarks></remarks>
        Private Sub AddEventForDeclinedQuote(ByVal oquote As NexusProvider.Quote)
            Dim oEventDetails As New NexusProvider.EventDetails
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            With oEventDetails
                .EventDate = Now()
                .InsuranceFileKey = oquote.InsuranceFileKey
                .InsuranceFileKeySpecified = True
                .InsuranceFolderKey = oquote.InsuranceFolderKey
                .InsuranceFolderKeySpecified = True
                .PartyKey = oquote.PartyKey
                .RtfText = "Quote Declined"
                .UserName = Session(CNLoginName)
                .EventTypeKey = 5
                .EventLogSubjectKey = 1
            End With

            oWebService.AddEvent(oEventDetails)
        End Sub

        ''' <summary>
        ''' Handles the OnClick event of Decline button on the risk page, which sets the quote status to decline. Needs to be manually
        ''' hooked up to a control on the risk page
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub Page_SaveStateComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SaveStateComplete
            Dim objNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim btnBuy As Button = CType(CType(GetMasterPlaceHolder(Page, objNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("btnBuy"), Button)
            If btnBuy IsNot Nothing Then
                If (btnBuy.Text = IIf(GetLocalResourceObject("lblIssued") Is Nothing, "Issue", GetLocalResourceObject("lblIssued"))) Then
                    btnBuy.Attributes.Add("onclick", "javascript:return showMessageIssued();")
                End If
            End If
        End Sub

        ''' <summary>
        ''' Will return the quote grace period for the product whose ProductCode is passed
        ''' </summary>
        ''' <param name="sProductCode"></param>
        ''' <remarks></remarks>
        Protected Function GetQuoteGracePeriod(ByVal sProductCode As String) As String
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oRiskType As NexusProvider.RiskType = Session(CNRiskType)
            Dim sProductPath() As String
            sProductPath = CStr(Request.ApplicationPath & "/" & oNexusConfig.ProductsFolder) _
                       .Split(Regex.Split("/", ""), StringSplitOptions.RemoveEmptyEntries)
            Dim oProduct As Config.Product = CType(GetSection("NexusFrameWork"),  _
             Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products.GetProductByName(Server.UrlDecode( _
             Request.Url.Segments(sProductPath.Length + 1).TrimEnd("/")))
            Dim iGracePeriod As String = ""
            iGracePeriod = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.GracePeriod, NexusProvider.RiskTypeOptions.Code, sProductCode, "")
            Return iGracePeriod
        End Function


        ''' <summary>
        ''' Perform QuoteConfirmation Task on the basis of Insurance File Key Provided. This is different to that of placed on BrokerView.aspx page
        ''' </summary>
        ''' <param name="iInsuranceFileKey"></param>
        ''' <remarks></remarks>
        Protected Sub DoQuoteConfirmation(ByVal iInsuranceFileKey As Integer)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oQuoteTemp As NexusProvider.Quote = Session(CNQuote)
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oRiskT As New NexusProvider.RiskType
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode) '(Session.Item(CNDataModelCode))
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name & "/"
            Dim sRiskFolder As String = sProductFolder & "\" & oProduct.RiskTypes.RiskType(0).Path & "/"
            Dim bIsQuoted As Boolean

            Dim sInsuranceFileKey As String
            sInsuranceFileKey = Convert.ToString(iInsuranceFileKey)
            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNBrokerCollection)

            Dim sRedirectPath As String = String.Empty

            oQuoteTemp = FillSessionValues(iInsuranceFileKey)
            Session(CNQuote) = oQuoteTemp
            bIsQuoted = IsAllRiskQuoted()
            If bIsQuoted Then

                sRedirectPath = String.Empty
                GetScreens()
                If sSummaryOfCover.ToLower = "true" Then
                    sRedirectPath = sSummaryOfCoverURL
                Else
                    sRedirectPath = "~/secure/PremiumDisplay.aspx"
                End If
                Response.Redirect(sRedirectPath, False)
                Exit Sub
            Else
                Session.Remove(CNTabState & "_" & sTabIndexControlID)
                sRedirectPath = sProductFolder & oProduct.RiskTypes.RiskType(0).Path & "/" & GetFirstRiskScreen(sRiskFolder & oProduct.FullQuoteConfig)
                If (CType(Session(CNLoginType), LoginType) = LoginType.Agent And oUserDetails.Key > 0) Then
                    Session(CNMode) = Mode.View
                    Response.Redirect(sRedirectPath, False)
                Else
                    Response.Redirect(sRedirectPath, False)
                End If
                Exit Sub
            End If

        End Sub
        ''' <summary>
        ''' Fill SessionValues 
        ''' </summary>
        ''' <param name="iInsuranceFileKey"></param>
        ''' <remarks></remarks>
        Protected Function FillSessionValues(ByVal iInsuranceFileKey As Integer) As NexusProvider.Quote
            Dim oQuote As NexusProvider.Quote
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim iCurrentRiskKey As Integer
            Try
                oQuote = oWebService.GetHeaderAndSummariesByKey(iInsuranceFileKey)
                'Put Party information into CNParty Session
                Dim oFindParty As NexusProvider.BaseParty
                oFindParty = oWebService.GetParty(oQuote.PartyKey)
                Session(CNParty) = oFindParty

                'Put highest risk key into Session
                For i As Integer = 0 To oQuote.Risks.Count - 1
                    oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote)
                    iCurrentRiskKey = oQuote.Risks(i).Key
                Next

                oWebService.GetHeaderAndRisksByKey(oQuote)

                Session(CNQuote) = oQuote


            Finally
                'oWebService = Nothing
            End Try
            Session(CNCurrenyCode) = oQuote.CurrencyCode
            'QUICK QUOTE CHECK IS REQUIRED. IF QUICK_QUOTE IS "TRUE", USER WILL BE REDIRECTED TO QUICK QUOTE ELSE TO FULL QUOTE


            'Use the GetDataSetDefinition to interogate the dataset to get the datamodelcode into session
            GetDataSetDefinition()

            'this will need to be set to nothing in case after doing MTA process user selects client
            ' and then choses to buy a Quote 
            Session(CNMTAType) = Nothing

            Session(CNRenewal) = Nothing
            Session(CNMode) = Mode.Edit
            Session(CNQuoteInSync) = False
            Session.Remove(CNOI)
            Session(CNInsuranceFileKey) = iInsuranceFileKey
            Session(CNQuoteInSync) = False


            If IsDataSetQuickQuote() = False Then
                Session(CNQuoteMode) = QuoteMode.FullQuote
            Else
                Session(CNQuoteMode) = QuoteMode.QuickQuote
            End If
            Return oQuote
        End Function

        ''' <summary>
        ''' Clear QuoteCollection SessionValues .
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub ClearQuoteCollectionSessionValues()
            Session.Remove(CNQuoteCollectionFiles)
            Session.Remove(CNTotalForQuoteCollection)
            Session.Remove(CNPolicySummaryCollection)
        End Sub

        ''' <summary>
        ''' Checks if the Quote is a Pending Quote or Agent Pending quote on the basis of logged in user
        ''' </summary>
        ''' <param name="bIsAgent"></param>
        ''' <remarks></remarks>
        Protected Function HasPendingQuote(Optional ByVal bIsAgent As Boolean = False) As Boolean
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)

            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name
            Dim oRiskT As New NexusProvider.RiskType

            'New changes as per WPR63
            Dim dStartDate As Date
            Dim dQuoteDate As Date

            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())

            Dim oPartySummary As NexusProvider.PartySummary
            oPartySummary = oWebService.GetBrokerSummary(NexusProvider.InsuranceQuoteType.ALL, "ALL", 0, oQuote.InsuranceFileRef, "", oPortal.MaxSearchResults, Nothing, dStartDate, dQuoteDate, 0)
            ViewState.Add(CNBrokerCollection, oPartySummary.Policies)

            For Each oPolicy As NexusProvider.Policy In oPartySummary.Policies
                If bIsAgent Then
                    If oPolicy.QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.AgentPending Then
                        Session("TempInsuranceFileKey") = oPolicy.InsuranceFileKey
                        Session("TempInsuranceFolderKey") = oPolicy.InsuranceFolderKey
                        Return True
                    End If
                Else
                    If oPolicy.QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.Pending Then
                        Session("TempInsuranceFileKey") = oPolicy.InsuranceFileKey
                        Session("TempInsuranceFolderKey") = oPolicy.InsuranceFolderKey
                        Return True
                    End If
                End If
            Next
        End Function

        ''' <summary>
        ''' Method to create a new copy of a quote which is currently in session
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CopyQuote(Optional ByVal bIsQuoteVersioning As Boolean = False)
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            oWebService.CopyQuote(oQuote.InsuranceFileKey, oQuote.InsuranceFolderKey, v_bIsQuoteVersioning:=bIsQuoteVersioning)
            oQuote = FillSessionValues(oQuote.InsuranceFileKey)
            If oUserDetails IsNot Nothing AndAlso oUserDetails.Key <> 0 Then 'Check if Logged in user is Agent
                Dim oUserAuthority As New NexusProvider.UserAuthority
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.AllowEditAgentCommission
                oUserAuthority.UserCode = Session(CNLoginName)
                oWebService.GetUserAuthorityValue(oUserAuthority)
                'Check the user's "Commission Override Authority"
                If (oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.AllowEditAgentCommission) Then
                    oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.AgentPending
                Else
                    oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.AgentComplete
                End If
                oWebService.UpdateQuoteStatus(oQuote)
            Else
                oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.Pending
                oWebService.UpdateQuoteStatus(oQuote)
            End If
            Session(CNQuote) = oQuote
        End Sub

        ''' <summary>
        ''' Method to check whether the risks of a quote are quoted or not
        ''' </summary>
        ''' <remarks></remarks>
        Private Function IsAllRiskQuoted() As Boolean
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim bRiskQuoted As Boolean = True
            For iCount As Integer = 0 To oQuote.Risks.Count - 1
                If (oQuote.Risks(iCount).IsRisk = True AndAlso oQuote.Risks(iCount).StatusCode.Trim.ToUpper <> "QUOTED") Then
                    bRiskQuoted = False
                    Exit For
                End If
            Next
            Return bRiskQuoted
        End Function

        ''' <summary>
        ''' Method to check whether the risks of a quote are referred or not
        ''' </summary>
        ''' <remarks></remarks>
        Private Function IsAnyRiskReffered() As Boolean
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim bRiskReferred As Boolean = False
            For iCount As Integer = 0 To oQuote.Risks.Count - 1
                If (oQuote.Risks(iCount).IsRisk = True AndAlso oQuote.Risks(iCount).StatusCode.Trim.ToUpper = "REFERRED") Then
                    bRiskReferred = True
                    Exit For
                End If
            Next
            Return bRiskReferred
        End Function

        ''' <summary>
        ''' Redirects to the specified risk page
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub RedirectRiskPage()

            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oRiskType As Config.RiskType
            Dim oRiskT As New NexusProvider.RiskType
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode) '(Session.Item(CNDataModelCode))
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name & "/"
            Dim sRiskFolder As String = sProductFolder & "\" & oProduct.RiskTypes.RiskType(0).Path & "/"
            Dim dExpiryDate As Date


            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNBrokerCollection)

            Dim sRedirectPath As String = String.Empty
            'Call UnmarkQuote Process
            CallUnmarkQuote()
            If oQuote.Risks(0).RiskCode Is Nothing Then
                oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(0).RiskTypeCode.Trim)
            Else
                oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(0).RiskCode)
            End If
            oRiskT.DataModelCode = oRiskType.DataModelCode
            oRiskT.Name = oRiskType.Name
            oRiskT.Path = oRiskType.Path
            oRiskT.RiskCode = oRiskType.RiskCode
            Session(CNRiskType) = oRiskT
            Dim iGracePeriod As Integer = 0
            sRiskFolder = sProductFolder & "/" & oRiskT.Path & "/"
            If oQuote.QuoteExpiryDate = Date.MinValue Then
                iGracePeriod = IIf(GetQuoteGracePeriod(oQuote.ProductCode.Trim()) = "", 0, GetQuoteGracePeriod(oQuote.ProductCode.Trim()))
                dExpiryDate = oQuote.CoverStartDate.AddDays(iGracePeriod).ToShortDateString()
            Else
                dExpiryDate = oQuote.QuoteExpiryDate
            End If
            Session.Remove(CNTabState & "_" & sTabIndexControlID)
            If (dExpiryDate < DateTime.Now) Then
                Session(CNMode) = Mode.View
                Response.Redirect(sRiskFolder & "/" & GetFirstRiskScreen(sRiskFolder & oProduct.FullQuoteConfig), False)
            ElseIf (oQuote.QuoteStatusKey = QuoteStatusType.Declined) Then
                Session(CNMode) = Mode.View
                Response.Redirect(sRiskFolder & "/" & GetFirstRiskScreen(sRiskFolder & oProduct.FullQuoteConfig), False)
            Else
                Response.Redirect(sRiskFolder & "/" & GetFirstRiskScreen(sRiskFolder & oProduct.FullQuoteConfig), False)
            End If

        End Sub
#End Region

#Region "WPR 73_74 Changes"
        ''' <summary>
        ''' This Method creates a background task for the work manager
        ''' </summary>
        ''' <param name="oQuote"></param>
        ''' <param name="v_sTask"></param>
        ''' <param name="v_sTaskDescription"></param>
        ''' <param name="v_sTaskGroup"></param>
        ''' <remarks></remarks>
        Protected Sub CreateTask(ByVal oQuote As NexusProvider.Quote, ByVal v_sTaskDescription As String, ByVal v_sTask As String, ByVal v_sTaskGroup As String)
            'Calling the Create Task Method of FrameWorkFunctions.vb
            FrameWorkFunctions.CreateTask(oQuote:=oQuote, v_sTaskDescription:=v_sTaskDescription, v_sTask:=v_sTask, v_sTaskGroup:=v_sTaskGroup)
        End Sub

        Public Sub SendMail()
            Dim sURL As String
            Dim oParty As NexusProvider.BaseParty = HttpContext.Current.Session(CNParty)
            Dim oQuote As NexusProvider.Quote
            Dim oClaim As NexusProvider.ClaimOpen = CType(HttpContext.Current.Session.Item(CNClaim), NexusProvider.ClaimOpen)
            If HttpContext.Current.Session(CNMode) = Mode.NewClaim Or HttpContext.Current.Session(CNMode) = Mode.EditClaim Or HttpContext.Current.Session(CNMode) = Mode.PayClaim Or HttpContext.Current.Session(CNMode) = Mode.SalvageClaim Or HttpContext.Current.Session(CNMode) = Mode.TPRecovery Then
                oQuote = HttpContext.Current.Session(CNClaimQuote)
            Else
                oQuote = HttpContext.Current.Session(CNQuote)
            End If

            If HttpContext.Current.Session.IsCookieless Then
                sURL = ConfigurationManager.AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/SendEmail.aspx?PartyKey=" & oParty.Key & "&key=Issued&InsuranceFileKey=" & oQuote.InsuranceFileKey & "&modal=true&loc=docm&n=p&Riskcheck=true&KeepThis=true&TB_iframe=true&height=300&width=750"
            Else
                sURL = ConfigurationManager.AppSettings("WebRoot") & "/Modal/SendEmail.aspx?PartyKey=" & oParty.Key & "&key=Issued&InsuranceFileKey=" & oQuote.InsuranceFileKey & "&modal=true&loc=docm&n=p&Riskcheck=true&KeepThis=true&TB_iframe=true&height=300&width=750"
            End If

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show", _
            "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sURL & "' , null);});</script>")
            Exit Sub
        End Sub

        Protected Function PrintButton(ByVal sender As Object, ByVal e As System.EventArgs) As String

            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sDocument As String = String.Empty
            Dim sFileName As String = String.Empty


            Dim oNexusFramework As Config.NexusFrameWork
            oNexusFramework = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

            Dim oRiskContainer As Object = CType(GetMasterPlaceHolder(Page, oNexusFramework.MainContainerName), ContentPlaceHolder)

            sFileName = GetRenewalDocumentName(oRiskContainer)

            If (sFileName <> "") Then
                sFileName = sFileName & "." & CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).DocumentFormat.ToLower()
            End If
            Dim oDocuments As Config.Documents = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork) _
                   .Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode) _
                   .Documents


            Dim sDocumentDirName As String = oDocuments.Location
            Dim oRenewalStatus As NexusProvider.RenewalStatus
            oRenewalStatus = oWebService.GetRenewalStatus(oQuote.InsuranceFileKey)

            Try
                If oRenewalStatus.RenewalStatusTypeDescription = sAwaiting_Manual_Preview Then
                    oWebService.UpdateRenewalStatus(oQuote, "AutoReview")
                    oRenewalStatus = oWebService.GetRenewalStatus(oQuote.InsuranceFileKey)
                End If
                If oRenewalStatus.RenewalStatusTypeDescription = sAwaiting_Renewal_Notice Then
                    sDocument = oWebService.GenerateInvite(NexusProvider.DocumentType.PDF, True, oQuote, sDocumentDirName + "\" + sFileName, Nothing)
                End If
                'Print_Renewaldocument.Visible = True
                'btnPrint.Visible = False
                'btnBuy.Visible = True
                'btnMarkQuoteForCollection.Visible = True
                'to update the oQuote since Quote status has been updated
                oQuote = oWebService.GetHeaderAndSummariesByKey(oQuote.InsuranceFileKey)
                For iCount As Integer = 0 To oQuote.Risks.Count - 1
                    oWebService.GetRisk(oQuote.Risks(iCount).Key, iCount, oQuote)
                Next
                oWebService.GetHeaderAndRisksByKey(oQuote)
                'TO retreive the selected status after btn Print
                For iCount As Integer = 0 To oQuote.Risks.Count - 1
                    If oQuote.Risks(iCount).IsRisk = True Then
                        oQuote.Risks(iCount).IsRisk = True
                    Else
                        oQuote.Risks(iCount).IsRisk = False
                    End If
                Next
            Catch ex As NexusProvider.NexusException
                '  If ex.Errors(0).Code <> "1000093" Then

                'End If
            Finally
                Session(CNQuote) = oQuote
            End Try
            'Send Mail Job required as WPR73_74
            SendMail()
            'Dim HTable As New Hashtable() 'to hold the document details
            'Dim odocument As NexusProvider.DocumentCollection
            'odocument = oWebService.GetDocumentList(oQuote.InsuranceFolderKey)

            'Dim odocumentstr As New NexusProvider.Document

            ''check if there is any object of document type returned
            'If Not odocument Is Nothing Then
            '    If odocument.Count > 0 Then

            '        'need to store the unique record into HashTable with the highest DocNum
            '        Dim icount As Integer = 0
            '        'run the loop till the count reaches to the total documents present in the policy
            '        For icount = 0 To odocument.Count - 1
            '            'if Exist, then update the data into Hash Table
            '            If odocument.Item(icount).DocDescription.Contains("Renewal") Then
            '                HTable.Item(odocument.Item(icount).DocDescription) = odocument.Item(icount).DocNum
            '            End If
            '        Next

            '        'displaying the data from the Hash Table 
            '        Dim HData As DictionaryEntry
            '        Dim objNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            '        Dim tblDocs As Table = CType(CType(GetMasterPlaceHolder(Page, objNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("tblDocs"), Table)

            '        For Each HData In HTable
            '            Dim h1 As New System.Web.UI.WebControls.HyperLink
            '            h1.NavigateUrl = "~/secure/document.aspx?doc_number=" & HData.Value & ""
            '            h1.Target = "_blank"
            '            h1.Text = HData.Key
            '            Dim tRow As New TableRow()
            '            If tblDocs IsNot Nothing Then
            '                tblDocs.Rows.Add(tRow)
            '            End If
            '            Dim tCell As New TableCell
            '            tRow.Cells.Add(tCell)
            '            tCell.Controls.Add(h1)
            '        Next
            '    End If
            'End If
            Return sDocument
        End Function

        ''' <summary>
        ''' To fetch any value from Bask Risk Resource file
        ''' </summary>
        ''' <param name="sId"></param>
        ''' <param name="sDefaultValue"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetLocalResource(ByVal sId As String, Optional ByVal sDefaultValue As String = "") As String
            Dim oResource As ResXResourceReader
            Dim en As IDictionaryEnumerator
            oResource = New ResXResourceReader(HttpContext.Current.Server.MapPath(AppSettings("WebRoot") & "App_LocalResources/BaseRisk.resx"))
            en = oResource.GetEnumerator()
            While (en.MoveNext)
                If en.Key.ToString.Trim = sId Then
                    Return en.Value
                End If
            End While
            Return sDefaultValue
        End Function

        ''' <summary>
        ''' Returns the Renewal Invite document Dynamically
        ''' </summary>
        ''' <param name="oRiskContainer"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function GetRenewalDocumentName(ByVal oRiskContainer As Object) As String
            Dim oNexusFramework As Config.NexusFrameWork
            Dim oDocumentControl As Object = Nothing
            Dim bFound As Boolean = False

            oNexusFramework = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

            For Each oControl In oRiskContainer.Controls
                If oControl.id IsNot Nothing Then
                    If (oControl.GetType.Name.ToUpper.Contains("CONTROLS_DOCUMENT_ASCX")) Then
                        oDocumentControl = CType(CType(GetMasterPlaceHolder(Page, oNexusFramework.MainContainerName), ContentPlaceHolder).FindControl(oControl.id), Object)
                        bFound = True
                        Exit For
                    End If
                End If
            Next
            If (bFound) Then
                Return oDocumentControl.documentname
            Else
                Return ""
            End If
        End Function

#End Region

#Region "WPR05"

        Public Sub QuoteAllRisk(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oRiskCollection As New NexusProvider.RiskCollection
            oRiskCollection = oQuote.Risks
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name & "/"

            Dim oRiskType As Config.RiskType

            For jCount As Integer = 0 To oQuote.Risks.Count - 1
                If oQuote.Risks(jCount).RiskCode Is Nothing Then
                    oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(jCount).RiskTypeCode.Trim)
                Else
                    oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(jCount).RiskCode.Trim)
                End If

                If oQuote.Risks(jCount).ScreenCode Is Nothing Then
                    oQuote.Risks(jCount).ScreenCode = GetScreenCode(sProductFolder & oRiskType.Path & "\" & oProduct.FullQuoteConfig)
                End If
                oWebService.UpdateRisk(oQuote, jCount, Nothing)
                Session(CNQuote) = oQuote
            Next

        End Sub
#End Region
    End Class


End Namespace
