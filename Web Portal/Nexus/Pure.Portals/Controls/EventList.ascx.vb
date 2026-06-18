Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Utils
Imports Nexus.Library
Imports CMS.Library
Imports System
Imports System.Data
Imports Nexus.Constants
Imports Nexus.Constants.Session


Namespace Nexus
    Partial Class secure_Controls_EventList : Inherits System.Web.UI.UserControl

        Private v_iPartyKey As Integer
        Dim arr As New ArrayList
        Dim bDisplayCaseOption As Boolean = False
        Private oNexusConfig As Config.NexusFrameWork
        Private oPortal As Nexus.Library.Config.Portal

#Region " Page Init "
        ''' <summary>
        ''' Set the Nexus Config informations.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oOptionSettings As NexusProvider.OptionTypeSetting

            oNexusConfig = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            oPortal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
            If Session(CNClaim) IsNot Nothing And Request.QueryString("modal") = "true" Then
                btnBack.Visible = True
            Else
                btnBack.Visible = False
                If (Request.QueryString("RequestType") Is Nothing OrElse (Request.QueryString("RequestType") IsNot Nothing AndAlso Request.QueryString("RequestType") <> "MTA")) Or  Session(CNDoNotClearSession) Is Nothing Or Session(CNDoNotClearSession) <> "true" Then
                    ClearClaims()
                    Session.Remove(CNMode)
                End If
            End If
            'If System Option for "Enhanced Case Search" is ON then we need to visible case related search criteria and grid column
            oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5099)
            If oOptionSettings IsNot Nothing AndAlso oOptionSettings.OptionValue IsNot Nothing AndAlso Not String.IsNullOrEmpty(oOptionSettings.OptionValue) Then
                If oOptionSettings.OptionValue(0) <> "0" Then
                    liCaseNumber.Visible = True
                    bDisplayCaseOption = True
                Else
                    liCaseNumber.Visible = False
                    bDisplayCaseOption = False
                End If
            Else
                liCaseNumber.Visible = False
                bDisplayCaseOption = False
            End If

            If Request.QueryString("modal") = "true" Then
                gvEventList.PageSize = "10"
                btnBack.Attributes.Add("onclick", "self.parent.tb_remove();")

            End If
        End Sub
#End Region

#Region " Page Load "
        ''' <summary>
        ''' In Page load fill the grid with the even list.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'To set the Focus
            Page.SetFocus(btnPolicyCode)
            'Catlin Performance Fix
            If Not IsPostBack AndAlso Me.Visible = True Then

                ViewState("SortExpression") = "EventDate"
                ViewState("SortDirection") = SortDirection.Descending

                'Populate the EventList 
                Session.Remove(CNPolicyNumber)
                ' Session.Remove(CNInsuranceFolderKey)
                PopulateEventListGrid()

                FillEventType()

                '  'Populate the User
                Dim oEventDetails As NexusProvider.EventDetailsCollection = Session(CNEvent)
                If oEventDetails IsNot Nothing AndAlso oEventDetails.Count > 0 Then
                    FillUser(oEventDetails)
                End If
            End If

            If Request("__EVENTARGUMENT") = "Refresh" Then
                PopulateEventListGrid()

                '  'Populate the User
                Dim oEventDetails As NexusProvider.EventDetailsCollection = Session(CNEvent)
                If oEventDetails IsNot Nothing AndAlso oEventDetails.Count > 0 Then
                    FillUser(oEventDetails)
                End If
            End If

        End Sub
#End Region
        Sub FillUser(Optional ByVal oEventList As NexusProvider.EventDetailsCollection = Nothing, Optional ByVal Index As Integer = 0)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sUserCode As String = Nothing
            Dim oUser As NexusProvider.UserCollection = Nothing
            Dim oTempUser As New NexusProvider.UserCollection
            'Fill the user dropdownlist.
            oUser = oWebService.GetUserGroupUsers(sUserCode, DateTime.Now, False, False)
            If oUser IsNot Nothing AndAlso oEventList IsNot Nothing Then
                If oUser.Count > 0 Then
                    For iCount As Integer = 0 To oUser.Count - 1
                        For jCount As Integer = 0 To oEventList.Count - 1
                            If oUser(iCount).UserName.Trim.ToUpper = oEventList(jCount).UserName.Trim.ToUpper Then
                                oTempUser.Add(oUser(iCount))
                                Exit For
                            End If
                        Next
                    Next
                End If
            End If

            ddlUserName.DataSource = oTempUser
            ddlUserName.DataTextField = "UserName"
            ddlUserName.DataValueField = "UserId"
            ddlUserName.DataBind()
            ddlUserName.Items.Insert(0, New ListItem("(all)", 0))
            ddlUserName.SelectedIndex = 0

        End Sub
        Sub FillEventType()
            Cache.Remove("PMLookup_event_type_group")
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim olist As New NexusProvider.LookupListCollection
            olist = oWebService.GetList(NexusProvider.ListType.PMLookup, "event_type_group", True, False)
            ddlEventType.DataSource = olist
            ddlEventType.DataTextField = "Description"
            ddlEventType.DataValueField = "Key"
            ddlEventType.DataBind()
            ddlEventType.Items.Insert(0, New ListItem("(all)", ""))

            ddlEventType.SelectedIndex = 0
        End Sub
#Region " Public Properties "
        ''' <summary>
        ''' Build Event Search Criteria.
        ''' </summary>
        ''' <param name="v_oEvent"></param>
        ''' <remarks></remarks>
        Private Sub EventSearchCriteria(ByRef v_oEvent As NexusProvider.EventDetails)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oInsuranceFileDetailsCollection As NexusProvider.InsuranceFileDetailsCollection
            Dim oClaims As NexusProvider.ClaimCollection
            Dim sURL As String
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)

            With (v_oEvent)
                .PartyKey = v_iPartyKey
                .AccountKeySpecified = False
                .BaseCaseKeySpecified = False
                .BaseCaseKeySpecified = False

                If (txtClaimCode.Text <> String.Empty) Then
                    Dim oClaimSearchCriteria As New NexusProvider.ClaimSearchCriteria
                    oClaimSearchCriteria.ClaimNumber = UCase(Trim(txtClaimCode.Text))

                    oClaims = oWebService.FindClaim(oClaimSearchCriteria)
                    If oClaims.Count > 0 Then
                        'As per the BA's advice we need to show all the events associated with Claim
                        'So need to pass the Base Claim Key
                        .BaseClaimKey = oClaims(0).BaseClaimKey
                        .BaseClaimKeySpecified = True
                        .ClaimNumber = txtClaimCode.Text.Trim()
                    End If
                End If

                If (txtPolicyCode.Text <> String.Empty) Then
                    Dim sShortName As String = String.Empty
                    If Session(CNParty) IsNot Nothing Then
                        Select Case True
                            Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                                With CType(Session(CNParty), NexusProvider.CorporateParty)
                                    sShortName = .ClientSharedData.ShortName.Trim()
                                End With
                            Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                                With CType(Session(CNParty), NexusProvider.PersonalParty)
                                    sShortName = .ClientSharedData.ShortName.Trim()
                                End With
                        End Select
                    End If
                    oInsuranceFileDetailsCollection = oWebService.FindPolicy(UCase(Trim(txtPolicyCode.Text)), "", sShortName, NexusProvider.InsuranceFileTypes.POLICY, False, Nothing)
                    .PolicyCode = txtPolicyCode.Text.Trim()
                    If oInsuranceFileDetailsCollection IsNot Nothing AndAlso oInsuranceFileDetailsCollection.Count > 0 Then
                        .InsuranceFileKeySpecified = True
                        .InsuranceFileKey = IIf(oInsuranceFileDetailsCollection IsNot Nothing, oInsuranceFileDetailsCollection(0).InsuranceFileKey, 0)
                        .InsuranceFolderKey = IIf(oInsuranceFileDetailsCollection IsNot Nothing, oInsuranceFileDetailsCollection(0).InsuranceFolderKey, 0)
                        .InsuranceFolderKeySpecified = True
                        Session("InsuranceFolderKey") = .InsuranceFolderKey
                    End If
                Else
                    .InsuranceFileKeySpecified = False
                End If

                If txtCaseNumber.Text.Length <> 0 Then
                    v_oEvent.CaseNumber = txtCaseNumber.Text.Trim
                    v_oEvent.CaseKey = IIf(hfCaseKey.Value.Trim = "", 0, hfCaseKey.Value.Trim)

                    If v_oEvent.CaseKey = 0 Or v_oEvent.BaseCaseKey = 0 Then
                        Dim oCaseCol As NexusProvider.CaseCollection = Nothing
                        Dim oCaseDetail As New NexusProvider.CaseDetails
                        oCaseDetail.CaseNumber = txtCaseNumber.Text.Trim
                        oCaseCol = oWebService.FindCase(oCaseDetail, Nothing)
                        If oCaseCol IsNot Nothing Then
                            If oCaseCol.Count = 1 Then
                                v_oEvent.CaseKey = oCaseCol(0).CaseKey
                                v_oEvent.CaseKeySpecified = True
                                v_oEvent.BaseCaseKey = oCaseCol(0).BaseCaseKey
                                v_oEvent.BaseCaseKeySpecified = True
                            Else

                                'Register a startup script script for opening findcase modal screen with CaseRef as querystring
                                If HttpContext.Current.Session.IsCookieless Then
                                    sURL = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Claims/FindCase.aspx?modal=true&KeepThis=true&Page=EL&FindCase=1&CaseRef=" & Server.UrlEncode(txtCaseNumber.Text.Trim) & "&TB_iframe=true&height=550&width=750"
                                Else
                                    sURL = AppSettings("WebRoot") & "/Claims/FindCase.aspx?modal=true&KeepThis=true&Page=EL&FindCase=1&CaseRef=" & Server.UrlEncode(txtCaseNumber.Text.Trim) & "&TB_iframe=true&height=550&width=750"
                                End If
                                sURL = "tb_show( null,'" & sURL & "' , null);"
                                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "openFindCase", sURL, True)
                                Exit Sub
                            End If
                        End If
                    End If
                End If

                If (txtEventFromDate.Text <> String.Empty) Then
                    .FromDate = CDate(txtEventFromDate.Text)
                    .FromDateSpecified = True
                Else
                    .FromDateSpecified = False
                End If

                If (txtEventToDate.Text <> String.Empty) Then
                    .DateTo = CDate(txtEventToDate.Text)
                    .DateToSpecified = True
                Else
                    .DateToSpecified = False
                End If

                If oQuote IsNot Nothing AndAlso oQuote.InsuranceFolderKey > 0 Then
                    .InsuranceFolderKey = oQuote.InsuranceFolderKey
                    .InsuranceFolderKeySpecified = True
                    Session.Remove(CNInsuranceFolderKey)
                Else
                    .InsuranceFolderKeySpecified = False
                End If

            End With
        End Sub
#End Region

#Region " DataGrid Events "
        ''' <summary>
        ''' Populate the Event List Gridview.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub PopulateEventListGrid(Optional ByVal bUserSelection As Boolean = True)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oEventParams As New NexusProvider.EventDetails
            Dim oEventList As New NexusProvider.EventDetailsCollection
            Dim oTempEventList As New NexusProvider.EventDetailsCollection
            Dim oParty As NexusProvider.BaseParty = Nothing

            If String.IsNullOrEmpty(HttpContext.Current.Request.QueryString("ReturnUrl")) Then
                HttpContext.Current.Session.Remove("InsuranceFolderKey")
                If txtPolicyCode.Text.Length <> 0 Then
                    oEventParams.PolicyCode = txtPolicyCode.Text.Trim
                    'oEventParams.InsuranceFileKey = IIf(hInsuranceFileKey.Value.Trim = "", 0, hInsuranceFileKey.Value.Trim)

                    If oEventParams.InsuranceFileKey <= 0 Then
                        Dim oPolicyCol As NexusProvider.InsuranceFileDetailsCollection = Nothing
                        oPolicyCol = oWebService.FindPolicy(oEventParams.PolicyCode, Nothing, Nothing, NexusProvider.InsuranceFileTypes.POLICY, False, v_bShowCancelledForEvents:=True)
                        If oPolicyCol IsNot Nothing AndAlso oPolicyCol.Count > 0 Then
                            oEventParams.InsuranceFolderKey = oPolicyCol(0).InsuranceFolderKey
                        End If
                    End If
                    'End If
                End If

                If Session(CNQuote) IsNot Nothing AndAlso bUserSelection = True Then
                    Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                    Session(CNInsuranceFolderKey) = oQuote.InsuranceFolderKey
                End If
                If txtClaimCode.Text.Length <> 0 Then
                    oEventParams.ClaimNumber = txtClaimCode.Text.Trim
                    oEventParams.ClaimKey = IIf(hiddenClaimCode.Value.Trim = "", 0, hiddenClaimCode.Value.Trim)
                    If oEventParams.ClaimKey <= 0 Then
                        Dim oClaimSearchCriteria As New NexusProvider.ClaimSearchCriteria
                        Dim oClaimColl As NexusProvider.ClaimCollection = Nothing
                        oClaimSearchCriteria.ClaimNumber = oEventParams.ClaimNumber
                        oClaimSearchCriteria.IncludeClosedClaim = True
                        oClaimColl = oWebService.FindClaim(oClaimSearchCriteria)

                        If oClaimColl IsNot Nothing AndAlso oClaimColl.Count > 0 Then
                            oEventParams.ClaimKey = oClaimColl(0).BaseClaimKey
                        End If
                    End If
                End If


                If txtEventFromDate.Text.Trim.Length <> 0 Then
                    oEventParams.EventFromDate = CDate(txtEventFromDate.Text.Trim)
                End If

                If txtEventToDate.Text.Trim.Length <> 0 Then
                    oEventParams.EventToDate = CDate(txtEventToDate.Text.Trim)
                End If

                If ddlEventType.SelectedValue IsNot Nothing Then
                    If ddlEventType.SelectedValue.Trim.Length <> 0 Then
                        oEventParams.EventTypeKey = ddlEventType.SelectedValue
                    End If
                End If

                If bUserSelection = True AndAlso ddlUserName.SelectedValue <> "" Then
                    If ddlUserName.SelectedValue > 0 Then
                        oEventParams.UserId = ddlUserName.SelectedValue
                    End If
                End If

                'Get the Party from the session.
                oParty = Session.Item(CNParty)
                If oParty IsNot Nothing AndAlso oParty.Key > 0 Then
                    v_iPartyKey = oParty.Key
                Else

                    If Request.QueryString("mode") IsNot Nothing AndAlso Request.QueryString("mode").ToUpper = "ADD" Then
                        Return
                    Else
                        Response.Redirect("~/secure/agent/FindClient.aspx?RequestPage=Event")
                    End If

                End If

                'Build the Request Parameters and get the EventListCollection from SAM.
                EventSearchCriteria(oEventParams)
                If String.IsNullOrEmpty(txtPolicyCode.Text) = False And oEventParams.InsuranceFolderKey = 0 Then
                    oEventList = Nothing
                ElseIf String.IsNullOrEmpty(txtClaimCode.Text) = False And oEventParams.BaseClaimKey = 0 Then
                    oEventList = Nothing
                    oEventParams.InsuranceFileKey = 0
                    oEventList = oWebService.GetEventDetails(oEventParams)
                ElseIf String.IsNullOrEmpty(txtCaseNumber.Text) = False And oEventParams.CaseKey = 0 Then
                    oEventList = Nothing
                Else
                    'TFS #10322 : Insurance file key must not be sent to sql query in order to get all the event details
                    oEventParams.InsuranceFileKey = 0
                    oEventList = oWebService.GetEventDetails(oEventParams)
                End If

                'Session(CNEvent) = oEventList
            Else
                If Session(CNClaim) IsNot Nothing And String.IsNullOrEmpty(HttpContext.Current.Request.QueryString("ReturnUrl")) = False Then
                    Dim oOpenClaim As NexusProvider.ClaimOpen = Session(CNClaim)
                    btnBack.Visible = True
                    Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)

                    If String.IsNullOrEmpty(oOpenClaim.ClaimNumber) = True Or oOpenClaim.ClaimNumber.Trim.ToUpper = "TBA" Then
                        txtPolicyCode.Text = oQuote.InsuranceFileRef
                        'oEventParams.InsuranceFileKey = oQuote.InsuranceFileKey
                        oEventParams.InsuranceFolderKey = oQuote.InsuranceFolderKey
                        txtPolicyCode.ReadOnly = True
                        btnPolicyCode.Enabled = False
                    ElseIf oOpenClaim.ClaimNumber.Trim.ToUpper <> "TBA" Then
                        If Session(CNMode) = Mode.NewClaim Then
                            oEventParams.ClaimNumber = oOpenClaim.ClaimNumber
                            txtClaimCode.Text = oOpenClaim.ClaimNumber
                            oEventParams.BaseClaimKey = oOpenClaim.BaseClaimKey
                            txtClaimCode.ReadOnly = True
                            btnClaimCode.Enabled = False
                        Else
                            oEventParams.ClaimNumber = oOpenClaim.ClaimNumber
                            txtClaimCode.Text = oOpenClaim.ClaimNumber
                            oEventParams.BaseClaimKey = oOpenClaim.BaseClaimKey
                            txtClaimCode.ReadOnly = True
                            btnClaimCode.Enabled = False
                            'To Make Policy Code read Only
                            txtPolicyCode.Text = oQuote.InsuranceFileRef
                            'oEventParams.InsuranceFileKey = oQuote.InsuranceFileKey
                            oEventParams.InsuranceFolderKey = oQuote.InsuranceFolderKey
                            txtPolicyCode.ReadOnly = True
                            btnPolicyCode.Enabled = False
                        End If
                    End If

                    If oOpenClaim.Client.PartyKey = 0 And Session(CNParty) IsNot Nothing Then
                        oParty = Session.Item(CNParty)
                        oEventParams.PartyKey = oParty.Key
                    Else
                        oEventParams.PartyKey = oOpenClaim.Client.PartyKey
                    End If

                    'oEventParams.ClaimNumber = oOpenClaim.ClaimNumber


                    If txtEventFromDate.Text.Trim.Length <> 0 Then
                        oEventParams.EventFromDate = CDate(txtEventFromDate.Text.Trim)
                        oEventParams.FromDate = oEventParams.EventFromDate
                        oEventParams.FromDateSpecified = True
                    End If

                    If txtEventToDate.Text.Trim.Length <> 0 Then
                        oEventParams.EventToDate = CDate(txtEventToDate.Text.Trim)
                        oEventParams.DateTo = oEventParams.EventToDate
                        oEventParams.DateToSpecified = True
                    End If

                    If ddlEventType.SelectedValue IsNot Nothing Then
                        If ddlEventType.SelectedValue.Trim.Length <> 0 Then
                            oEventParams.EventTypeKey = ddlEventType.SelectedValue
                        End If
                    End If

                    If ddlUserName.SelectedValue <> "" Then
                        If ddlUserName.SelectedValue > 0 Then
                            oEventParams.UserId = ddlUserName.SelectedValue
                        End If
                    End If

                    If String.IsNullOrEmpty(txtPolicyCode.Text) = False And oEventParams.InsuranceFolderKey = 0 Then
                        oEventList = Nothing
                    ElseIf String.IsNullOrEmpty(txtClaimCode.Text) = False And oEventParams.BaseClaimKey = 0 Then
                        oEventList = Nothing
                    Else
                        oTempEventList = oWebService.GetEventDetails(oEventParams)
                    End If


                    For i As Integer = 0 To oTempEventList.Count - 1

                        oEventList.Add(oTempEventList(i))

                    Next

                    'Session(CNEvent) = oEventList
                ElseIf Session(CNQuote) IsNot Nothing And String.IsNullOrEmpty(HttpContext.Current.Request.QueryString("ReturnUrl")) = False Then
                    Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                    btnBack.Visible = True

                    txtPolicyCode.Text = oQuote.InsuranceFileRef
                    'oEventParams.InsuranceFileKey = oQuote.InsuranceFileKey
                    oEventParams.InsuranceFolderKey = oQuote.InsuranceFolderKey
                    txtPolicyCode.ReadOnly = True
                    btnPolicyCode.Enabled = False

                    oParty = Session.Item(CNParty)
                    oEventParams.PartyKey = oParty.Key

                    If (txtClaimCode.Text <> String.Empty) Then
                        Dim oClaimSearchCriteria As New NexusProvider.ClaimSearchCriteria
                        Dim oClaims As NexusProvider.ClaimCollection
                        oClaimSearchCriteria.ClaimNumber = UCase(Trim(txtClaimCode.Text))

                        oClaims = oWebService.FindClaim(oClaimSearchCriteria)
                        If oClaims.Count > 0 Then
                            oEventParams.BaseClaimKey = oClaims(0).BaseClaimKey
                            oEventParams.BaseClaimKeySpecified = True
                            oEventParams.ClaimNumber = txtClaimCode.Text.Trim()
                        End If
                    End If

                    If txtEventFromDate.Text.Trim.Length <> 0 Then
                        oEventParams.EventFromDate = CDate(txtEventFromDate.Text.Trim)
                        oEventParams.FromDate = oEventParams.EventFromDate
                        oEventParams.FromDateSpecified = True
                    End If

                    If txtEventToDate.Text.Trim.Length <> 0 Then
                        oEventParams.EventToDate = CDate(txtEventToDate.Text.Trim)
                        oEventParams.DateTo = oEventParams.EventToDate
                        oEventParams.DateToSpecified = True
                    End If

                    If ddlEventType.SelectedValue IsNot Nothing Then
                        If ddlEventType.SelectedValue.Trim.Length <> 0 Then
                            oEventParams.EventTypeKey = ddlEventType.SelectedValue
                        End If
                    End If

                    If ddlUserName.SelectedValue <> "" Then
                        If ddlUserName.SelectedValue > 0 Then
                            oEventParams.UserId = ddlUserName.SelectedValue
                        End If
                    End If

                    If String.IsNullOrEmpty(txtPolicyCode.Text) = False And oEventParams.InsuranceFolderKey = 0 Then
                        oEventList = Nothing
                    ElseIf String.IsNullOrEmpty(txtClaimCode.Text) = False And oEventParams.BaseClaimKey = 0 Then
                        oEventList = Nothing
                    Else
                        oEventList = oWebService.GetEventDetails(oEventParams)
                    End If

                    'Session(CNEvent) = oEventList
                End If
            End If
            Dim oEventListTemp As New NexusProvider.EventDetailsCollection

            If txtPolicyCode.Text.Length <> 0 AndAlso Not oEventList Is Nothing Then
                For count As Integer = 0 To oEventList.Count - 1
                    If oEventList(count).PolicyCode = txtPolicyCode.Text.Trim() Then
                        oEventListTemp.Add(oEventList(count))
                    End If
                Next
            Else
                oEventListTemp = oEventList
            End If

            Session(CNEvent) = oEventListTemp

            If oEventListTemp IsNot Nothing AndAlso oEventListTemp.Count > 0 Then
                Dim sortExpression As String = If(ViewState("SortExpression") IsNot Nothing, ViewState("SortExpression").ToString(), "EventDate")
                Dim sortDirection As SortDirection = If(ViewState("SortDirection") IsNot Nothing, CType(ViewState("SortDirection"), SortDirection), SortDirection.Descending)

                oEventListTemp.SortColumn = sortExpression
                oEventListTemp.SortingOrder = sortDirection
                oEventListTemp.Sort()
            End If

            'Bind the Grid with the EvenList Collection object.
            gvEventList.AllowPaging = True
            gvEventList.AllowSorting = True
            gvEventList.DataSource = oEventListTemp
            gvEventList.DataBind()

            oWebService = Nothing
            oEventParams = Nothing
            oEventList = Nothing
            oParty = Nothing
        End Sub

        Protected Sub gvEventList_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvEventList.DataBound
            If gvEventList.Rows.Count = 0 Or gvEventList.PageCount = 1 Then
                gvEventList.AllowPaging = False
            End If
            If bDisplayCaseOption Then
                gvEventList.Columns(3).Visible = True
            Else
                gvEventList.Columns(3).Visible = False
            End If
        End Sub
        ''' <summary>
        ''' Page navigation in the Gridview.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvEventList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvEventList.PageIndexChanging

            gvEventList.PageIndex = e.NewPageIndex
            gvEventList.DataSource = Session(CNEvent)
            gvEventList.DataBind()
        End Sub
        ''' <summary>
        ''' Row Data bound Event in Gridview
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvEventList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvEventList.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim hypDetails As LinkButton = e.Row.FindControl("hypEventDetails")
                If HttpContext.Current.Session.IsCookieless Then
                    hypDetails.OnClientClick = "tb_show(null , '" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/EventDetails.aspx?PostbackTo=" & updEventList.ClientID.ToString & "&EventDetailID=" & CType(e.Row.DataItem, NexusProvider.EventDetails).EventKey & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                Else
                    hypDetails.OnClientClick = "tb_show(null , '" & AppSettings("WebRoot") & "/Modal/EventDetails.aspx?PostbackTo=" & updEventList.ClientID.ToString & "&EventDetailID=" & CType(e.Row.DataItem, NexusProvider.EventDetails).EventKey & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                End If
                Dim rowindex As Integer = Convert.ToInt32(e.Row.RowIndex) + ((gvEventList.PageIndex) * (gvEventList.PageSize))

                If e.Row.FindControl("lblEventDescription") IsNot Nothing Then
                    Dim oLabelDesc As Label = e.Row.FindControl("lblEventDescription")

                    If DataBinder.Eval(e.Row.DataItem, "EventDescription") = "" _
                    And DataBinder.Eval(e.Row.DataItem, "Description") = "" _
                    And DataBinder.Eval(e.Row.DataItem, "EventType") = "New Policy" Then
                        oLabelDesc.Text = "Added Policy " + DataBinder.Eval(e.Row.DataItem, "PolicyCode")
                    ElseIf DataBinder.Eval(e.Row.DataItem, "EventType") = "Notes - Customer" Then
                        oLabelDesc.Text = "Note:" & DataBinder.Eval(e.Row.DataItem, "EventDescription")
                    ElseIf DataBinder.Eval(e.Row.DataItem, "EventType") = "New Client" _
                    And DataBinder.Eval(e.Row.DataItem, "EventDescription") = "" _
                    And DataBinder.Eval(e.Row.DataItem, "Description") = "" Then
                        oLabelDesc.Text = "Client created"
                    End If
                End If

                'To show the status
                If CType(e.Row.DataItem, NexusProvider.EventDetails).EventType = "Notes - Customer Warning" _
                And String.IsNullOrEmpty(CType(e.Row.DataItem, NexusProvider.EventDetails).Priority) = False Then
                    If e.Row.FindControl("lblStatus") IsNot Nothing Then
                        Dim olblStatus As Label = e.Row.FindControl("lblStatus")
                        If CType(e.Row.DataItem, NexusProvider.EventDetails).StatusKey = 0 Then
                            olblStatus.Text = "Outstanding"
                        ElseIf CType(e.Row.DataItem, NexusProvider.EventDetails).StatusKey = 1 Then
                            olblStatus.Text = "Completed"
                        End If
                    End If
                End If
            End If
        End Sub
#End Region

#Region " Event Type SelectedIndexChange event "
        ''' <summary>
        ''' Filter the Gridview by Event Type.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub ddlEventType_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlEventType.SelectedIndexChanged
            PopulateEventListGrid()
        End Sub
#End Region

#Region " Username SelectedIndexChange event "
        ''' <summary>
        ''' Filter the Gridview by USername.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub ddlUserName_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUserName.SelectedIndexChanged
            PopulateEventListGrid()
        End Sub
#End Region

#Region " Refresh Click "
        Protected Sub btnRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
            If Page.IsValid Then
                Dim oEventDetails As NexusProvider.EventDetailsCollection = Nothing
                ddlEventType.SelectedIndex = 0
                PopulateEventListGrid(True)
                oEventDetails = Session(CNEvent)
                FillUser(oEventDetails, 0)
            End If
        End Sub
#End Region

        Protected Sub CustVldDate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustVldDate.ServerValidate
            If txtEventFromDate.Text.Trim.Length <> 0 Then
                If IsDate(txtEventFromDate.Text.Trim) = False Then
                    args.IsValid = False
                    CustVldDate.ErrorMessage = GetLocalResourceObject("lbl_RqdErrMsgInvalidFromDate")
                Else
                    args.IsValid = True
                End If
            End If
        End Sub
        Protected Sub btnNewSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSearch.Click
            txtClaimCode.Text = String.Empty
            hiddenClaimCode.Value = String.Empty
            hfClaimKey.Value = String.Empty
            hfinsurancekey.Value = String.Empty
            txtEventFromDate.Text = String.Empty
            txtEventToDate.Text = String.Empty
            txtPolicyCode.Text = String.Empty
            hInsuranceFileKey.Value = String.Empty
            ddlEventType.SelectedIndex = 0
            txtCaseNumber.Text = String.Empty
            hfCaseKey.Value = String.Empty
            Session(CNPolicyNumber) = Nothing
            Session("InsuranceFolderKey") = Nothing
            If Session(CNQuote) IsNot Nothing Then
                Session(CNQuote).InsuranceFolderKey = Nothing
            End If
            PopulateEventListGrid(False)
            '  'Populate the User
            Dim oEventDetails As NexusProvider.EventDetailsCollection = Session(CNEvent)
            If oEventDetails IsNot Nothing AndAlso oEventDetails.Count > 0 Then
                FillUser(oEventDetails)
            End If
        End Sub

        Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
            If String.IsNullOrEmpty(HttpContext.Current.Request.QueryString("ReturnUrl")) = False Then
                Response.Redirect(HttpContext.Current.Request.QueryString("ReturnUrl"))
            End If
        End Sub
        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If HttpContext.Current.Session.IsCookieless Then
                btnPolicyCode.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/FindInsuranceFile.aspx?Page=EL&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=550&width=750' , null);return false;"
                btnClaimCode.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Claims/FindClaim.aspx?Page=EL&FindClaim=1&modal=true&KeepThis=true&TB_iframe=true&height=550&width=750' , null);return false;"
                btnCaseNumber.OnClientClick = "tb_show(null ,'../Claims/FindCase.aspx?modal=true&KeepThis=true&Page=EL&FindCase=1&TB_iframe=true&height=550&width=750' , null);return false;"
                btnAddEvent.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/AddEvent.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"

            Else
                btnPolicyCode.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Modal/FindInsuranceFile.aspx?Page=EL&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=550&width=750' , null);return false;"
                btnClaimCode.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Claims/FindClaim.aspx?Page=EL&FindClaim=1&modal=true&KeepThis=true&TB_iframe=true&height=550&width=750' , null);return false;"
                btnCaseNumber.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Claims/FindCase.aspx?modal=true&KeepThis=true&Page=EL&FindCase=1&TB_iframe=true&height=550&width=750' , null);return false;"
                btnAddEvent.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Modal/AddEvent.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            End If
        End Sub

        ''' <summary>
        ''' For sorting events grid
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvEventList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvEventList.Sorting
            'sort the Quote & Policy according to the column clicked
            'we need to store the current sort order in viewstate, and reverse it each time
            Dim oEventCollection As NexusProvider.EventDetailsCollection = Session(CNEvent)
            oEventCollection.SortColumn = e.SortExpression
            'check that the sort expression is the same as stored in viewstate as we should start again if reordering by a new column
            Dim _sortDirection As New SortDirection
            If ViewState("SortDirection") = SortDirection.Ascending And ViewState("SortExpression") = e.SortExpression Then
                _sortDirection = SortDirection.Descending
            Else
                _sortDirection = SortDirection.Ascending
            End If
            'store the current sortdirection for comparison on the next sort
            ViewState("SortDirection") = _sortDirection
            'store the SortExpression in viewstate so that we can check if we are sorting by a new column on the next sort
            ViewState("SortExpression") = e.SortExpression
            oEventCollection.SortingOrder = _sortDirection
            oEventCollection.Sort()
            CType(sender, GridView).DataSource = oEventCollection
            CType(sender, GridView).DataBind()
        End Sub
    End Class
End Namespace

