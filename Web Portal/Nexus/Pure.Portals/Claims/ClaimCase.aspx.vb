Imports CMS.Library
Imports System.Data
Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session

Imports System
Imports System.Globalization
Imports System.Threading
Imports NexusProvider.SAMForInsurance
Imports Nexus.Utils
Imports Nexus.Library

Namespace Nexus
    Partial Class Claims_ClaimCase
        Inherits Frontend.clsCMSPage

        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
        Dim bHasNumberSchemeON As Boolean

        Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oOptionSettings As NexusProvider.OptionTypeSetting
                Try
                    'If System Option for "Case number scheme" is not set then we need to enable case number
                    oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5031)
                    If oOptionSettings IsNot Nothing AndAlso oOptionSettings.OptionValue IsNot Nothing Then
                        If oOptionSettings.OptionValue(0) <> "0" Then
                            bHasNumberSchemeON = True
                        End If
                    End If
                Catch
                End Try
            If Not bHasNumberSchemeON Then
                rqdCaseNumber.Enabled = True
           
            End If
        End Sub
        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'To set the Focus
            Page.SetFocus(btnClaimCode)
            If Not IsPostBack Then

                If Request.QueryString("NewCase") = "true" Then
                    ClearCase()
                    If Not bHasNumberSchemeON Then
                        txtCaseNumber.Enabled = True
                    End If
                Else
                    txtCaseNumber.Enabled = False
                End If
                'set the current date as maximum date in range validator
                rngCaseOpenDate.MaximumValue = DateTime.Now.ToShortDateString
                txtCaseOpenDate.Text = DateTime.Now.ToShortDateString
                'Populate Dropdown
                PopulateDropdown()

                'create a unique key and add this to viewstate
                'this will be used to cache the results of the SAM call
                Dim ClaimCasepageCacheID As Guid
                ClaimCasepageCacheID = Guid.NewGuid()
                ViewState.Add("ClaimCasepageCacheID", ClaimCasepageCacheID.ToString)

                If Request.QueryString("CaseKey") IsNot Nothing Or Request.QueryString("BaseCaseKey") IsNot Nothing Then
                    btnOpen.Visible = True
                    btnLink.Visible = True
                    btnClose.Visible = True
                    liTabEvents.Visible = True
                    pnlTabEvents.Visible = True
                    btnSubmit.OnClientClick = "if (Page_IsValid) {tb_show(null , '../Modal/ClaimCaseChange.aspx?Button=Submit&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;}"
                    GetCaseDetails()

                   
                End If
            Else
                lblInformation.Visible = False
                lblInformation.Text = String.Empty
            End If

            If Request("__EVENTARGUMENT") = "LinkRefresh" Then
                Page.ClientScript.GetPostBackEventReference(Me, "")
                LinkCase()
            End If

            If Request("__EVENTARGUMENT") = "CloseCase" Then
                CloseCase()
            End If

            If Request("__EVENTARGUMENT") = "SaveCase" Then
                Page.ClientScript.GetPostBackEventReference(Me, "")
                SaveCase()
                'javascript message
                Dim sMessage As String
                sMessage = GetLocalResourceObject("UpdateCase_Msg")
                sMessage = sMessage.Replace("#CaseNumber", txtCaseNumber.Text)
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "showUpdMsg", _
                                                   "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){showUpdMsg('" & sMessage & "');});</script>")
            End If

            If Request("__EVENTARGUMENT") = "Refresh" Then
                Dim oEventDetails As NexusProvider.EventDetailsCollection = Nothing
                oEventDetails = PolulateEvent()
                FillUser(oEventDetails)
                grdvEvents.Visible = True
                grdvEvents.AllowPaging = True
                grdvEvents.DataSource = oEventDetails
                grdvEvents.DataBind()
            End If
            
        End Sub
        Protected Sub btnOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOpen.Click
            Dim iBaseCaseKey As Integer
            If hBaseCaseKey.Value > 0 Then
                iBaseCaseKey = hBaseCaseKey.Value
            End If

            If iBaseCaseKey > 0 Then
                Response.Redirect("~/Claims/FindInsuranceFile.aspx?BaseCaseKey=" & iBaseCaseKey, False)
            End If
        End Sub

        ''' <summary>
        ''' This is fired on Row Command of the Grid View
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvLinkedClaims_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvLinkedClaims.RowCommand

            Select Case e.CommandName
                Case "UnLink"
                    'Claim Unlink from Case
                    Dim sClaimNumber As String = CStr(e.CommandArgument)
                    Dim oLinkedClaimColl As NexusProvider.LinkedClaimCollection = CType(Cache.Item(ViewState("ClaimCasepageCacheID")), NexusProvider.LinkedClaimCollection)
                    For iCount As Integer = 0 To oLinkedClaimColl.Count - 1
                        If oLinkedClaimColl(iCount).ClaimNumber = sClaimNumber Then
                            UnlinkClaim(oLinkedClaimColl(iCount).ClaimKey)
                            Cache.Remove(ViewState("ClaimCasepageCacheID"))
                            GetCaseDetails()
                        End If
                    Next

                Case "Edit", "View", "Pay", "Salvage", "TPRecovery"
                    ClearQuote()
                    ClearClaims()
                    ClearHeader()
                    Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    If e.CommandName = "Edit" Then
                        Session(CNMode) = Mode.EditClaim
                    ElseIf e.CommandName = "Pay" Then
                        Session(CNMode) = Mode.PayClaim
                    ElseIf e.CommandName = "Salvage" Then
                        Session(CNMode) = Mode.SalvageClaim
                    ElseIf e.CommandName = "TPRecovery" Then
                        Session(CNMode) = Mode.TPRecovery
                    Else
                        Session(CNMode) = Mode.ViewClaim
                    End If
                    'Check for pending portfolio transfer
                    If Session(CNMode) <> Mode.ViewClaim Then
                        Dim lbtn As LinkButton = CType(e.CommandSource, LinkButton)
                        Dim gvrCurrentRow As GridViewRow = CType(lbtn.NamingContainer, GridViewRow)
                        Dim nInsuranceFilKey As Integer = Convert.ToInt32(grdvLinkedClaims.DataKeys(gvrCurrentRow.RowIndex).Value)
                        Dim bIsPendingPortfolioTransfer, bIsPendingCloneTransfer As Boolean

                        oWebservice.IsPendingTransfer(nInsuranceFilKey, bIsPendingCloneTransfer, bIsPendingPortfolioTransfer, Nothing)
                        Dim sMessage As String = ""
                        If bIsPendingCloneTransfer OrElse bIsPendingPortfolioTransfer Then
                            If bIsPendingPortfolioTransfer Then
                                sMessage = Convert.ToString(GetLocalResourceObject("msg_PendingPortfolioTransfer"))
                            ElseIf bIsPendingCloneTransfer Then
                                sMessage = Convert.ToString(GetLocalResourceObject("msg_PendingClonedTransfer"))
                            End If
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "PendingPortfolioTransfer", "alert('" + sMessage + "')", True)
                            Exit Sub
                        End If
                    End If

                    Session.Remove(CNClaim)
                    Session.Remove(CNClaimsSearchData)

                    'Find Latest Claim Version
                    If Not String.IsNullOrEmpty(e.CommandArgument) Then
                        Dim oClaimVersions As NexusProvider.VersionsCollections = Nothing
                        Dim oQuote As NexusProvider.Quote = Nothing
                        Dim oBaseParty As NexusProvider.BaseParty = Nothing
                        Dim sClaimNumber As String = CStr(e.CommandArgument)
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
                                    Session(CNClaimQuote) = oQuote
                                End If
                                Session(CNClaimVersion) = oClaimVersions
                                Session.Item(CNInsuranceFileKey) = oClaimVersions(0).InsuranceFileKey
                                Session.Item(CNPolicyNumber) = oClaimVersions(0).InsuranceRef
                                'Response.Redirect("FindClaim.aspx")
                            End If
                            For iCount As Integer = 0 To oClaimVersions.Count - 1
                                If oClaimVersions(iCount).Version = iHighest Then

                                    ' Begin - WPR VB 64 - Media Type Status 

                                    If e.CommandName = "Pay" Then
                                        Dim CheckMediatypeStatusAtPolicyRefund As String = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, _
                                                                   NexusProvider.ProductRiskOptions.CheckMediatypeStatusAtClaimPayment, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing, oQuote.BranchCode).Trim()

                                        If CheckMediatypeStatusAtPolicyRefund.Contains("1") Then
                                            Dim oMediaTypeStatus As New NexusProvider.MediaTypeStatus
                                            With oMediaTypeStatus
                                                .InsuranceFileKey = oQuote.InsuranceFileKey
                                                .LossDateSpecified = False
                                            End With
                                            oWebservice.GetPolicyStatusForMediaTypeStatus(oMediaTypeStatus)
                                            'SAM Return the False intead of True, if unclear fund exist then it retirn False or else true
                                            If Not oMediaTypeStatus.IsUnclearedCashListExists Then
                                                vldMediaTypeStatus.IsValid = False
                                                Exit Sub
                                            End If
                                        End If
                                    End If
                                    ' End - WPR VB 64 - Media Type Status 

                                    'To Check whether Payment is pending for Authorization
                                    If e.CommandName = "Pay" Then
                                        Dim sMultipleClaimsPayments As String
                                        Dim dMaxUnauthorisedClaimValue As Double
                                        Dim iMaxUnauthorisedNoClaimPayments As Integer
                                        Dim CashListItem As New NexusProvider.CashListItems
                                        Dim CountOfClaims As Integer
                                        'get the Product Risk option setting named Multiple Claims Payments(MultipleClaimsPayments)
                                        sMultipleClaimsPayments = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.MultipleClaimsPayments, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
                                        'get the Product Risk option setting named max unauthorised claim Value(MaxUnauthorisedClaimValue)
                                        dMaxUnauthorisedClaimValue = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.MaxUnauthorisedClaimValue, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
                                        'get the Product Risk option setting named max unauthorised number of claim payamnt(MaxUnauthorisedNoClaimPayments)
                                        iMaxUnauthorisedNoClaimPayments = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.MaxUnauthorisedNoClaimPayments, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)

                                        With CashListItem
                                            .ClaimNumber = oClaimVersions(iCount).ClaimNumber
                                        End With
                                        'get the Payment Referred details
                                        oCashListItem = oWebservice.GetReferredPayments(CashListItem)

                                        If oCashListItem IsNot Nothing AndAlso oCashListItem.Count > 0 Then
                                            For Each oCashList As NexusProvider.CashListItems In oCashListItem
                                                If sMultipleClaimsPayments IsNot Nothing Then
                                                    If sMultipleClaimsPayments = "1" Then
                                                        If oClaimVersions(iCount).ClaimNumber = oCashList.ClaimNumber Then
                                                            ' Count the Number of oCashList and add it up in CountOfClaims and check the same against MaxUnauthorisedNoClaimPayments
                                                            CountOfClaims = CountOfClaims + 1
                                                            If iMaxUnauthorisedNoClaimPayments <= CountOfClaims Or dMaxUnauthorisedClaimValue <= oCashList.PaymentAmount Then
                                                                AllowClaimPayment.IsValid = False
                                                                Exit Sub
                                                            End If
                                                        End If
                                                    Else
                                                        If oClaimVersions(iCount).ClaimNumber = oCashList.ClaimNumber Then
                                                            AllowClaimPayment.IsValid = False
                                                            Exit Sub
                                                        End If
                                                    End If
                                                End If
                                            Next
                                        End If
                                    End If
                                    'Retreival of claim details

                                    Try
                                        'This is expected to through an error if claim is locked in BO
                                        GetClaimDetails(oClaimVersions(iCount).ClaimKey)
                                    Catch ex As NexusProvider.NexusException
                                        'Claim locking error
                                        Select Case CType(ex.Errors(0), NexusProvider.NexusError).Code
                                            Case "200" 'Claim Locking
                                                'Show Claim locking error as alert
                                                Dim sMessage As String = "alert('" + ex.Errors(0).Description + ".\n" + ex.Errors(0).Detail + "')"
                                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage, True)
                                                Server.ClearError()
                                                ClearQuote()
                                                ClearClaims()
                                                Exit Sub
                                            Case Else
                                                'Clear all claim related sessions and throw the error
                                                ClearQuote()
                                                ClearClaims()
                                                Throw
                                        End Select
                                    End Try

                                    oOpenClaim = Session(CNClaim)
                                    oOpenClaim.ClientShortName = oClaimVersions(iCount).ClientShortName
                                    'check for closed claim
                                    If e.CommandName = "Pay" AndAlso oClaimDetails IsNot Nothing Then
                                        If Not String.IsNullOrEmpty(oClaimDetails.ClaimStatus) AndAlso oClaimDetails.ClaimStatus.Trim.ToUpper = "CLOSED" Then
                                            ChkClosedClaim.IsValid = False
                                            Exit Sub
                                        End If
                                    End If


                                    'Check Recovery Reserve
                                    Dim bAvailableReserve As Boolean = False
                                    If e.CommandName = "Salvage" Then
                                        'Check the availability of salvage reserve
                                        If oOpenClaim.ClaimPeril IsNot Nothing Then
                                            For iCounter As Integer = 0 To oOpenClaim.ClaimPeril.Count - 1
                                                If oOpenClaim.ClaimPeril(iCounter).SalvageRecovery IsNot Nothing Then
                                                    If oOpenClaim.ClaimPeril(iCounter).SalvageRecovery.Count > 0 Then
                                                        For jCounter As Integer = 0 To oOpenClaim.ClaimPeril(iCounter).SalvageRecovery.Count - 1
                                                            If oOpenClaim.ClaimPeril(iCounter).SalvageRecovery(jCounter).TotalRecovery > 0 Then
                                                                bAvailableReserve = True
                                                                Exit For
                                                            End If
                                                        Next
                                                    End If
                                                End If
                                                If bAvailableReserve = True Then
                                                    Exit For
                                                End If
                                            Next
                                            'if reserve is available then link will be shown
                                            If bAvailableReserve = False Then
                                                ChkRecoveryReserver.Enabled = True
                                                ChkRecoveryReserver.IsValid = False
                                                Exit Sub
                                            Else
                                                ChkRecoveryReserver.Enabled = False
                                                ChkRecoveryReserver.IsValid = True
                                            End If
                                        End If

                                    ElseIf e.CommandName = "TPRecovery" Then
                                        'Check the availability of TPRecovery reserve
                                        If oOpenClaim.ClaimPeril IsNot Nothing Then
                                            For iCounter As Integer = 0 To oOpenClaim.ClaimPeril.Count - 1
                                                If oOpenClaim.ClaimPeril(iCounter).TPRecovery IsNot Nothing Then
                                                    If oOpenClaim.ClaimPeril(iCounter).TPRecovery.Count > 0 Then
                                                        For jCounter As Integer = 0 To oOpenClaim.ClaimPeril(iCounter).TPRecovery.Count - 1
                                                            If oOpenClaim.ClaimPeril(iCounter).TPRecovery(jCounter).TotalRecovery > 0 Then
                                                                bAvailableReserve = True
                                                                Exit For
                                                            End If
                                                        Next
                                                    End If
                                                End If
                                                If bAvailableReserve = True Then
                                                    Exit For
                                                End If
                                            Next
                                            'if reserve is available then link will be shown
                                            If bAvailableReserve = False Then
                                                ChkRecoveryReserver.Enabled = True
                                                ChkRecoveryReserver.IsValid = False
                                                Exit Sub
                                            Else
                                                ChkRecoveryReserver.Enabled = False
                                                ChkRecoveryReserver.IsValid = True
                                            End If
                                        End If
                                    End If
                                    If e.CommandName = "Pay" Or e.CommandName = "View" Or e.CommandName = "Salvage" Or e.CommandName = "TPRecovery" Then
                                        Dim bClaimBuilder As Boolean
                                        Boolean.TryParse(Session(CNClaimBuilder), bClaimBuilder)
                                        If bClaimBuilder = True Then
                                            'Retreival of the risk related values 
                                            oClaimRisk = GetClaimRiskCall(oOpenClaim.BaseClaimKey, oOpenClaim.ClaimKey, oQuote.BranchCode)
                                            Session(CNDataSet) = oClaimRisk.XMLDataSet
                                            Session.Item(CNClaimRiskTimeStamp) = oClaimRisk.TimeStamp
                                        End If
                                    End If
                                End If
                            Next

                            Session(CNClaim) = oOpenClaim
                            Session(CNParentPage) = "CC" 'Claim Case
                            Response.Redirect("~/Claims/Overview.aspx", False)
                        Finally
                            oClaimDetails = Nothing
                            oWebservice = Nothing
                            oClaimRisk = Nothing
                        End Try
                    End If
                    ' End of Find Latest Claim Version
            End Select
        End Sub

        ''' <summary>
        ''' This is fired on the row data bound of the Grid View.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvLinkedClaims_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvLinkedClaims.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim lbUnlink As LinkButton = e.Row.FindControl("btnUnLink")
                lbUnlink.CommandArgument = CType(e.Row.DataItem, NexusProvider.CaseDetails).ClaimNumber
                Dim sMessage As String = GetLocalResourceObject("msg_UnlinkConfirm")
                sMessage = sMessage.Replace("#ClaimNumber", CType(e.Row.DataItem, NexusProvider.CaseDetails).ClaimNumber)
                sMessage = sMessage.Replace("#CaseNumber", txtCaseNumber.Text)

                lbUnlink.OnClientClick = "javascript:return UnLinkCaseConfirmation('" & sMessage & "');"

                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("ClaimKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.Claim).ClaimKey)

            End If

        End Sub

        Protected Sub grdvLinkedClaims_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdvLinkedClaims.Sorting
            'sort the Quote & Policy according to the column clicked
            'we need to store the current sort order in viewstate, and reverse it each time
            Dim oLinkedClaimColl As NexusProvider.LinkedClaimCollection = CType(Cache.Item(ViewState("ClaimCasepageCacheID")), NexusProvider.LinkedClaimCollection)
            If oLinkedClaimColl Is Nothing Then
                GetCaseDetails()
                oLinkedClaimColl = CType(Cache.Item(ViewState("ClaimCasepageCacheID")), NexusProvider.LinkedClaimCollection)
            End If
            oLinkedClaimColl.SortColumn = e.SortExpression
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
            oLinkedClaimColl.SortingOrder = _sortDirection
            oLinkedClaimColl.Sort()
            CType(sender, GridView).DataSource = oLinkedClaimColl
            CType(sender, GridView).DataBind()
        End Sub

        Protected Sub btnRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
            Me.ClientScript.RegisterStartupScript(GetType(String), "StartupScripts", "$('.nav-tabs li:eq(1) a').tab('show');", True)
            If Page.IsValid Then
                Dim oEventDetails As NexusProvider.EventDetailsCollection = Nothing
                oEventDetails = PolulateEvent()
                FillUser(oEventDetails)
                grdvEvents.Visible = True
                grdvEvents.AllowPaging = True
                grdvEvents.DataSource = oEventDetails
                grdvEvents.DataBind()
            End If
        End Sub

        Protected Sub drpEventType_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpEventType.SelectedIndexChange
            Me.ClientScript.RegisterStartupScript(GetType(String), "StartupScripts", "$('.nav-tabs li:eq(1) a').tab('show');", True)
            Dim oEventDetails As NexusProvider.EventDetailsCollection = Nothing
            oEventDetails = PolulateEvent()
            FillUser(oEventDetails)
            grdvEvents.Visible = True
            grdvEvents.AllowPaging = True
            grdvEvents.DataSource = oEventDetails
            grdvEvents.DataBind()
        End Sub

        Protected Sub drpUserName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpUserName.SelectedIndexChanged
            Me.ClientScript.RegisterStartupScript(GetType(String), "StartupScripts", "$('.nav-tabs li:eq(1) a').tab('show');", True)
            Dim oEventDetails As NexusProvider.EventDetailsCollection = Nothing
            oEventDetails = PolulateEvent()
            'FillUser(oEventDetails)
            grdvEvents.Visible = True
            grdvEvents.AllowPaging = True
            grdvEvents.DataSource = oEventDetails
            grdvEvents.DataBind()
        End Sub

#Region "Protected Methods"
        Sub CloseCase()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Try
                oWebService.CloseCase(hBaseCaseKey.Value, hCaseKey.Value, hDesc.Value)
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "showcloseerrmsg", "<script language=""JavaScript"" type=""text/javascript""></script>")
                Response.Redirect("~/Claims/FindCase.aspx", False)
            Catch ex As NexusProvider.NexusException
                If ex.Errors(0).Code = "300" Then
                    'javascript message
                    Dim sMessage As String
                    sMessage = ex.Errors(0).Detail
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "showcloseerrmsg", _
                                                       "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowCloseErrMsg('" & sMessage & "');});</script>")


                End If
            Finally
                oWebService = Nothing

            End Try


        End Sub
        Sub LinkCase()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oWarnings As NexusProvider.WarningCollection
            lblInformation.Text = String.Empty
            Try
                oWarnings = oWebService.CaseLinkUnlink(hBaseCaseKey.Value, hClaimKey.Value, False)

                If oWarnings IsNot Nothing AndAlso oWarnings.Count > 0 Then
                    Dim sMessage As String
                    sMessage = oWarnings(0).Description
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "showcloseerrmsg", _
                                                       "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowCloseErrMsg('" & sMessage & "');});</script>")
                Else
                    Cache.Remove(ViewState("ClaimCasepageCacheID"))
                    GetCaseDetails(hCaseKey.Value)
                End If
            Catch ex As System.Exception
                Dim sMessage As String
                sMessage = GetLocalResourceObject("Err_LinkClaim")
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "showcloseerrmsg", _
                                                   "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowCloseErrMsg('" & sMessage & "');});</script>")
            End Try
        End Sub
        Sub UnlinkClaim(ByVal iClaimKey As Integer)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oWarnings As NexusProvider.WarningCollection
            If iClaimKey = 0 AndAlso hClaimKey.Value > 0 Then
                iClaimKey = hClaimKey.Value
            End If
            lblInformation.Text = String.Empty
            oWarnings = oWebService.CaseLinkUnlink(hBaseCaseKey.Value, iClaimKey, True)
            If oWarnings IsNot Nothing AndAlso oWarnings.Count > 0 Then
                lblInformation.Visible = True
                lblInformation.Text = oWarnings(0).Description
            End If
        End Sub
        Sub FillUser(ByVal oEventList As NexusProvider.EventDetailsCollection)

            If oEventList IsNot Nothing AndAlso oEventList.Count > 0 Then

                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim sUserCode As String = Nothing
                Dim oUser As NexusProvider.UserCollection = Nothing
                Dim oTempUser As New NexusProvider.UserCollection

                'Fill the user dropdownlist.
                oUser = oWebService.GetUserGroupUsers(sUserCode, DateTime.Now, False, False)
                If oUser IsNot Nothing Then
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

                drpUserName.DataSource = oTempUser
                drpUserName.DataTextField = "UserName"
                drpUserName.DataValueField = "UserId"
                drpUserName.DataBind()
                drpUserName.Items.Insert(0, New ListItem(GetLocalResourceObject("drp_Default"), ""))
                drpUserName.SelectedIndex = 0
            Else
                drpUserName.Items.Clear()
                drpUserName.Items.Insert(0, New ListItem(GetLocalResourceObject("drp_Default"), ""))
                drpUserName.SelectedIndex = 0
            End If
        End Sub
        Sub SaveCase()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim iCaseKey As Integer
            Integer.TryParse(Request.QueryString("CaseKey"), iCaseKey)
            Dim oCaseSearchCriteria As New NexusProvider.CaseDetails
            Dim oNewCaseDetails As NexusProvider.CaseDetails
            Dim dCaseOpenDate As Date

            If String.IsNullOrEmpty(txtCaseOpenDate.Text) = False AndAlso IsDate(txtCaseOpenDate.Text) Then
                dCaseOpenDate = txtCaseOpenDate.Text
            End If

            'Initializing the values
            If Request.QueryString("CaseKey") IsNot Nothing Then
                oCaseSearchCriteria.CaseKey = iCaseKey
            ElseIf Request.QueryString("NewCase") = "true" AndAlso String.IsNullOrEmpty(txtCaseNumber.Text) = False AndAlso Not String.IsNullOrEmpty(hCaseKey.Value) Then
                oCaseSearchCriteria.CaseKey = hCaseKey.Value
            ElseIf Not String.IsNullOrEmpty(hCaseKey.Value) AndAlso Convert.ToDouble(hCaseKey.Value.Trim) > 0 Then
                oCaseSearchCriteria.CaseKey = hCaseKey.Value
            End If

            oCaseSearchCriteria.CaseNumber = Trim(txtCaseNumber.Text)
            oCaseSearchCriteria.CaseOpenDate = dCaseOpenDate
            oCaseSearchCriteria.ClaimNumber = Trim(txtClaimNumber.Text)
            oCaseSearchCriteria.ProgressStatusCode = drpProgressStatus.Text
            oCaseSearchCriteria.Analyst = drpAnalyst.Text
            oCaseSearchCriteria.Assistant = drpAssistant.Text
            oCaseSearchCriteria.EventDescription = Trim(hDesc.Value)

            'Sam Call for saving the Data
            oNewCaseDetails = oWebService.SaveCase(oCaseSearchCriteria.CaseKey, oCaseSearchCriteria.CaseNumber, oCaseSearchCriteria.CaseOpenDate, oCaseSearchCriteria.Assistant, oCaseSearchCriteria.Analyst, oCaseSearchCriteria.ProgressStatusCode, oCaseSearchCriteria.EventDescription)

            If oNewCaseDetails IsNot Nothing Then
                If Request.QueryString("NewCase") = "true" Then
                    btnOpen.Visible = True
                    btnLink.Visible = True
                    btnClose.visible = True
                    liTabEvents.Visible = True
                    pnlTabEvents.Visible = True
                    txtCaseNumber.Text = oNewCaseDetails.CaseNumber
                    hCaseKey.Value = oNewCaseDetails.CaseKey
                    hBaseCaseKey.Value = oNewCaseDetails.BaseCaseKey
                    txtEventCaseNumber.Text = oNewCaseDetails.CaseNumber
                    Dim oEventDetails As NexusProvider.EventDetailsCollection = Nothing
                    oEventDetails = PolulateEvent(oNewCaseDetails.CaseKey, oNewCaseDetails.BaseCaseKey)
                    FillUser(oEventDetails)
                    'Update case elated sessions
                    Session(CNCaseKey) = oNewCaseDetails.CaseKey
                    Session(CNBaseCaseKey) = oNewCaseDetails.BaseCaseKey

                    'Case Number need to pass to show in Javascript message in FindClaim.aspx page
                    btnLink.OnClientClick = "tb_show(null , '../Claims/FindClaim.aspx?Page=CC&CaseNumber=" & oNewCaseDetails.CaseNumber & "&modal=true&KeepThis=true&TB_iframe=true&height=750&width=750' , null);return false;"
                    btnSubmit.OnClientClick = "tb_show(null , '../Modal/ClaimCaseChange.aspx?Button=Submit&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    Response.Redirect("~/Claims/ClaimCase.aspx?CaseKey=" & hCaseKey.Value, False)
                Else
                    'Update sessions CNCaseKey and find case details again
                    Session(CNCaseKey) = oNewCaseDetails.CaseKey
                    'Clear cache to search new detail for claim case.So that new details can be searched
                    If ViewState("ClaimCasepageCacheID") IsNot Nothing Then
                        Cache.Remove(ViewState("ClaimCasepageCacheID"))
                    End If
                    'Get laletst case detail
                    GetCaseDetails(oNewCaseDetails.CaseKey)
                End If
            End If
        End Sub

        Function PolulateEvent(Optional ByVal iCaseKey As Integer = 0, Optional ByVal iBaseCaseKey As Integer = 0) As NexusProvider.EventDetailsCollection
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oEventCriteria As New NexusProvider.EventDetails

            If iCaseKey = 0 Then
                Integer.TryParse(Request.QueryString("CaseKey"), iCaseKey)
            End If

            Dim oEventDetails As NexusProvider.EventDetailsCollection = Nothing
            oEventCriteria.CaseKey = iCaseKey
            oEventCriteria.CaseKeySpecified = True
            If txtClaimNumber.Text.Length <> 0 Then
                oEventCriteria.ClaimNumber = txtClaimNumber.Text.Trim
            End If

            'Initializing the values
            If iCaseKey > 0 Then
                oEventCriteria.CaseKey = iCaseKey
            ElseIf hCaseKey.Value > 0 Then
                oEventCriteria.CaseKey = hCaseKey.Value
            End If

            If iBaseCaseKey > 0 Then
                oEventCriteria.BaseCaseKey = iBaseCaseKey
            ElseIf hBaseCaseKey.Value > 0 Then
                oEventCriteria.BaseCaseKey = hBaseCaseKey.Value
            End If

            If txtFromDate.Text.Trim.Length <> 0 Then
                oEventCriteria.FromDateSpecified = True
                oEventCriteria.FromDate = CDate(txtFromDate.Text.Trim)
            End If

            If txtToDate.Text.Trim.Length <> 0 Then
                oEventCriteria.DateToSpecified = True
                oEventCriteria.DateTo = CDate(txtToDate.Text.Trim)
            End If

            If drpUserName.SelectedValue <> "" Then
                If drpUserName.SelectedValue > 0 Then
                    oEventCriteria.UserId = drpUserName.SelectedValue
                End If
            End If

            If drpEventType.Value IsNot Nothing Then
                If drpEventType.Value.Trim.Length <> 0 Then
                    oEventCriteria.EventTypeKey = drpEventType.Value
                End If
            End If

            oEventDetails = oWebservice.GetEventDetails(oEventCriteria)
            Session.Item(CNEvent) = oEventDetails
            Return oEventDetails
        End Function
        Sub GetCaseDetails(Optional ByVal iCaseKeyToSearch As Integer = 0)
            Dim oLinkedClaimColl As NexusProvider.LinkedClaimCollection = CType(Cache.Item(ViewState("ClaimCasepageCacheID")), NexusProvider.LinkedClaimCollection)
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            If oLinkedClaimColl Is Nothing Then
                Dim oCaseDetails As NexusProvider.CaseDetails = Nothing
                Dim oEventColl As NexusProvider.EventDetailsCollection
                Dim iCaseKey As Integer

                If iCaseKeyToSearch = 0 Then
                    Integer.TryParse(Request.QueryString("CaseKey"), iCaseKey)
                Else
                    iCaseKey = iCaseKeyToSearch
                End If
                'Returned from claim complete page
                If Request.QueryString("BaseCaseKey") IsNot Nothing AndAlso iCaseKey = 0 Then
                    Integer.TryParse(Request.QueryString("BaseCaseKey"), iCaseKey)
                End If

                oCaseDetails = oWebservice.GetCaseDetails(iCaseKey)

                If oCaseDetails IsNot Nothing Then
                    txtEventCaseNumber.Text = oCaseDetails.CaseNumber
                    txtCaseNumber.Text = oCaseDetails.CaseNumber
                    drpAssistant.SelectedValue = oCaseDetails.Assistant
                    txtCaseOpenDate.Text = CDate(oCaseDetails.CaseOpenDate)
                    drpAnalyst.SelectedValue = oCaseDetails.Analyst
                    oLinkedClaimColl = oCaseDetails.LinkedClaims

                    If oCaseDetails.ProgressStatusCode IsNot Nothing Then
                        drpProgressStatus.SelectedValue = oCaseDetails.ProgressStatusCode

                        If oCaseDetails.ProgressStatusCode = "CLOSED" Then
                            'grdvLinkedClaims.Enabled = False
                            grdvLinkedClaims.Columns(5).Visible = False
                            'btnSubmit.Enabled = False
                            btnOpen.Enabled = False
                            btnLink.Enabled = False
                            btnClose.Enabled = False
                            drpAnalyst.Enabled = False
                            drpAssistant.Enabled = False
                            txtCaseOpenDate.Enabled = False
                            LossDateEndLimit_CalendarLookup.Enabled = False
                            If Not Page.IsPostBack Then
                                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "showUpdMsg", _
                                                       "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){showUpdMsg('This is a close Case. Please Reopen/Open the Case Progress Status for further Action');});</script>")
                            End If
                        Else
                            grdvLinkedClaims.Columns(5).Visible = True
                            btnOpen.Enabled = True
                            btnLink.Enabled = True
                            btnClose.Enabled = True
                            drpAnalyst.Enabled = True
                            drpAssistant.Enabled = True
                            txtCaseOpenDate.Enabled = True
                            LossDateEndLimit_CalendarLookup.Enabled = True
                        End If
                    End If



                    hCaseKey.Value = oCaseDetails.CaseKey
                    hBaseCaseKey.Value = oCaseDetails.BaseCaseKey
                    txtCaseVersion.Text = oCaseDetails.ClaimVersion
                    Cache.Insert(ViewState("ClaimCasepageCacheID"), oCaseDetails.LinkedClaims, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                    'Close Button functionality
                    Dim sUrl As String = Nothing
                    sUrl = "'../Modal/ClaimCaseChange.aspx?Button=CloseCase&modal=true&KeepThis=true&TB_iframe=true&height=500&width=700'"
                    Dim sMessage As String = GetLocalResourceObject("msg_CloseConfirm")
                    sMessage = sMessage.Replace("#CaseNumber", txtCaseNumber.Text)
                    btnClose.OnClientClick = "javascript:CloseCaseConfirmation('" & sMessage & "'," & hCaseKey.Value & "," & hBaseCaseKey.Value & "," & sUrl & "); return false;"
                    'Case Number need to pass to show in Javascript message in FindClaim.aspx page
                    btnLink.OnClientClick = "tb_show(null , '../Claims/FindClaim.aspx?Page=CC&CaseNumber=" & oCaseDetails.CaseNumber & "&modal=true&KeepThis=true&TB_iframe=true&height=750&width=750' , null);return false;"

                    If HttpContext.Current.Session.IsCookieless Then
                        btnClaimCode.OnClientClick = "tb_show(null ,'../Claims/FindClaim.aspx?Page=CEL&FindClaim=1&CaseNumber=" & oCaseDetails.CaseNumber & "&modal=true&KeepThis=true&TB_iframe=true&height=550&width=750' , null);return false;"
                    Else
                        btnClaimCode.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Claims/FindClaim.aspx?Page=CEL&FindClaim=1&CaseNumber=" & oCaseDetails.CaseNumber & "&modal=true&KeepThis=true&TB_iframe=true&height=550&width=750' , null);return false;"
                    End If
                End If

                'Filteration of Event Details of the  selected case
                oEventColl = PolulateEvent(oCaseDetails.CaseKey, oCaseDetails.BaseCaseKey)
                FillUser(oEventColl)
                'populate the event details
                grdvEvents.AllowPaging = True
                grdvEvents.Visible = True
                grdvEvents.DataSource = oEventColl
                grdvEvents.DataBind()
            End If

            'Populating the linked claim details Grid
            grdvLinkedClaims.Visible = True
            grdvLinkedClaims.DataSource = oLinkedClaimColl
            grdvLinkedClaims.DataBind()
        End Sub

        Sub PopulateDropdown()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oXmlElement As System.Xml.XmlElement = Nothing
            Dim oListColl As NexusProvider.LookupListCollection = Nothing
            oListColl = oWebService.GetList(NexusProvider.ListType.PMLookup, "Handler", True, False, Nothing, Nothing, Nothing, oXmlElement)
            Dim oAssistant As New NexusProvider.LookupListCollection
            Dim oAnalyst As New NexusProvider.LookupListCollection

            'Load the xml element 
            If oXmlElement IsNot Nothing Then
                Dim sXML As String = oXmlElement.OuterXml
                Dim xmlDoc As New System.Xml.XmlDocument()
                xmlDoc.LoadXml(sXML)

                If oListColl IsNot Nothing Then
                    For iListCount As Integer = 0 To oListColl.Count - 1
                        If xmlDoc.ChildNodes IsNot Nothing Then
                            For iCount As Integer = 0 To xmlDoc.ChildNodes(0).ChildNodes.Count - 1
                                Dim iAnalyst, iHandlerId As Integer
                                For iChildCount As Integer = 0 To xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes.Count - 1
                                    If xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount).Name.Trim.ToUpper = "HANDLER_ID" Then
                                        iHandlerId = CInt(xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount).InnerText)
                                    End If
                                    If xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount).Name.Trim.ToUpper = "IS_ANALYST" Then
                                        iAnalyst = CInt(xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount).InnerText)
                                        Exit For
                                    End If
                                Next
                                If oListColl(iListCount).Key = iHandlerId Then
                                    If iAnalyst > 0 Then
                                        oAnalyst.Add(oListColl(iListCount))
                                    Else
                                        oAssistant.Add(oListColl(iListCount))
                                    End If
                                    Exit For
                                End If
                            Next
                        End If
                    Next
                End If
            End If

            'Sort Analysts and assistants to ascending order of Name
            oAnalyst.Sort(NexusProvider.DataItemTypes.Description, NexusProvider.Direction.Asc)
            oAssistant.Sort(NexusProvider.DataItemTypes.Description, NexusProvider.Direction.Asc)

            'Analyst
            drpAnalyst.DataSource = oAnalyst
            drpAnalyst.DataTextField = "Description"
            drpAnalyst.DataValueField = "Code"
            drpAnalyst.DataBind()
            drpAnalyst.Items.Insert(0, New ListItem(GetLocalResourceObject("drp_Default"), ""))
            drpAnalyst.SelectedIndex = 0

            'Assistant
            drpAssistant.DataSource = oAssistant
            drpAssistant.DataTextField = "Description"
            drpAssistant.DataValueField = "Code"
            drpAssistant.DataBind()
            drpAssistant.Items.Insert(0, New ListItem(GetLocalResourceObject("drp_Default"), ""))
            drpAssistant.SelectedIndex = 0

            'Progress Code
            oListColl = oWebService.GetList(NexusProvider.ListType.PMLookup, "case_progress", True, False)
            oListColl.Sort(NexusProvider.DataItemTypes.Description, NexusProvider.Direction.Asc)
            drpProgressStatus.DataSource = oListColl
            drpProgressStatus.DataTextField = "Description"
            drpProgressStatus.DataValueField = "Code"
            drpProgressStatus.DataBind()
            drpProgressStatus.Items.Insert(0, New ListItem(GetLocalResourceObject("drp_Default"), ""))
            drpProgressStatus.SelectedIndex = 0
        End Sub

        Protected Sub CustVldDate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustVldDate.ServerValidate
            If txtFromDate.Text.Trim.Length <> 0 Then
                If IsDate(txtFromDate.Text.Trim) = False Then
                    args.IsValid = False
                    CustVldDate.ErrorMessage = GetLocalResourceObject("lbl_RqdErrMsgInvalidFromDate")
                Else
                    args.IsValid = True
                End If
            End If
        End Sub
#End Region

        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
            If Page.IsValid Then
                SaveCase()
                If Request.QueryString("CaseKey") IsNot Nothing Then
                    'javascript message
                    Dim sMessage As String
                    sMessage = GetLocalResourceObject("UpdateCase_Msg")
                    sMessage = sMessage.Replace("#CaseNumber", txtCaseNumber.Text)
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "showUpdMsg", _
                                                       "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){showUpdMsg('" & sMessage & "');});</script>")
                End If
            End If
        End Sub

        Protected Sub grdvEvents_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvEvents.PageIndexChanging
            Dim oEventColl As NexusProvider.EventDetailsCollection = Session.Item(CNEvent)
            If oEventColl IsNot Nothing Then
                'populate the event details
                grdvEvents.PageIndex = e.NewPageIndex
                grdvEvents.DataSource = oEventColl
                grdvEvents.DataBind()
            End If
        End Sub

        Protected Sub grdvEvents_PreRender(sender As Object, e As EventArgs) Handles grdvEvents.PreRender
            If grdvEvents.Rows.Count = 0 Then
                grdvEvents.AllowPaging = False
            End If
        End Sub

        Protected Sub grdvEvents_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvEvents.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim hypDetails As LinkButton = e.Row.FindControl("btnDetails")
                hypDetails.OnClientClick = "tb_show(null , '../Modal/EventDetails.aspx?PostbackTo=" & pnlEvents.ClientID.ToString & "&EventDetailID=" & CType(e.Row.DataItem, NexusProvider.EventDetails).EventKey & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"

                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("EventKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.EventDetails).EventKey)

            End If
        End Sub

        Protected Sub grdvEvents_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdvEvents.Sorting
            'sort the Quote & Policy according to the column clicked
            'we need to store the current sort order in viewstate, and reverse it each time
            Dim oEventDetails As NexusProvider.EventDetailsCollection = Session.Item(CNEvent)
            If oEventDetails Is Nothing Then
                PolulateEvent()
                oEventDetails = Session.Item(CNEvent)
            End If
            oEventDetails.SortColumn = e.SortExpression
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
            oEventDetails.SortingOrder = _sortDirection
            oEventDetails.Sort()
            CType(sender, GridView).DataSource = oEventDetails
            CType(sender, GridView).DataBind()
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If HttpContext.Current.Session.IsCookieless Then
                btnClaimCode.OnClientClick = "tb_show(null ,'../Claims/FindClaim.aspx?Page=CEL&FindClaim=1&modal=true&KeepThis=true&TB_iframe=true&height=550&width=750' , null);return false;"
            Else
                btnClaimCode.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Claims/FindClaim.aspx?Page=CEL&FindClaim=1&modal=true&KeepThis=true&TB_iframe=true&height=550&width=750' , null);return false;"
            End If
        End Sub
    End Class
End Namespace
