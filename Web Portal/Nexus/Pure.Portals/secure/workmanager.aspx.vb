Imports CMS.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Configuration.ConfigurationManager

Namespace Nexus

    Partial Class WorkManager : Inherits Frontend.clsCMSPage
        Private oWorkManagerFilter As New NexusProvider.WorkManagerFilter
        Private bChkIfSysAdmin As Boolean = False

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Request("__EVENTARGUMENT") = "StartTask" Then
                StartTask()
            ElseIf Not IsPostBack Then
                ClearWorkManager()
                ClearQuote()
                ClearClaims()
                ClearHeader()

                If Not IsPostBack Then
                    SetFilters()

                    If Not Session(kWorkManagerFilter) Is Nothing Then
                        oWorkManagerFilter = Session(kWorkManagerFilter)
                        ddlShowType.SelectedValue = oWorkManagerFilter.DropDownShowTypeValue
                        ddlUserGroups.SelectedValue = oWorkManagerFilter.DropDownUserGroupsValue
                        ddlUsers.SelectedValue = oWorkManagerFilter.DropDownUsersValue
                        ddlTaskStatus.SelectedValue = oWorkManagerFilter.DropDownTaskStatusValue
                        ddlDate.SelectedValue = oWorkManagerFilter.DropDownDateValue
                        ddlParty.SelectedValue = oWorkManagerFilter.DropDownPartyValue
                    End If
                End If
                BindData()
            Else
                If Not Session(kWorkManagerFilter) Is Nothing Then
                    oWorkManagerFilter = Session(kWorkManagerFilter)
                End If
            End If
        End Sub
        ''' <summary>
        ''' This event is fired when the Selected index of User Group DropDown List is changed and the User DropDown List is binded accordingly.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub ddlUserGroups_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUserGroups.SelectedIndexChanged
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oUserGroup As NexusProvider.UserCollection = New NexusProvider.UserCollection
            Dim oNewUser As New NexusProvider.User()
            Dim sUserGroupcode As String = Nothing
            Dim oWorkManager As NexusProvider.WorkManager = Nothing
            Dim bChkIfSysAdmin As Boolean = False
            If ddlUserGroups.SelectedValue = "(All Group)" Or ddlUserGroups.SelectedValue.ToString.Trim = GetLocalResourceObject("llb_UserGroupDefaultText").ToString.Trim Then  '(All Group) is the Hard coded value with reference to SAM.
                sUserGroupcode = Nothing
            Else
                sUserGroupcode = ddlUserGroups.SelectedValue.ToString()
            End If
            oUserGroup = oWebService.GetUserGroupUsers(sUserGroupcode, DateTime.Now, False, False)
            ddlUsers.DataSource = oUserGroup
            ddlUsers.DataTextField = "UserName"
            ddlUsers.DataValueField = "UserName"
            ddlUsers.DataBind()
            ddlUsers.Items.Insert(0, New ListItem(GetLocalResourceObject("llb_UsersDefaultText"), GetLocalResourceObject("llb_UsersDefaultText")))
            If ddlUsers.Items.Count > 0 Then
                For iCount As Integer = 0 To ddlUsers.Items.Count - 1
                    If ddlUsers.Items(iCount).Text.Trim.ToUpper = Session(CNLoginName).ToString.Trim.ToUpper Then
                        ddlUsers.SelectedValue = ddlUsers.Items(iCount).Text
                        Exit For
                    End If
                Next
            End If
            'arch issue -Supervisor for User Groups should able to see other user task
            Dim oUserDetails As NexusProvider.UserDetails = oWebService.GetUserDetails(HttpContext.Current.User.Identity.Name)
            Dim UserGroups As NexusProvider.UserGroupCollection = oUserDetails.AvailableUsergroups
            If Not oUserDetails.AvailableUsergroups Is Nothing Then
                For Each oAvailableUsergroups In oUserDetails.AvailableUsergroups
                    If oAvailableUsergroups.IsSysAdmin = True Then
                        bChkIfSysAdmin = True
                        Exit For
                    End If
                Next
            End If
            If bChkIfSysAdmin Then
                ddlUsers.Enabled = True
            Else
                ddlUsers.Enabled = False
            End If
            'check user is Supervisor of selected user group  
            For iCount As Integer = 0 To UserGroups.Count - 1
                If UserGroups(iCount).Code = ddlUserGroups.SelectedValue AndAlso UserGroups(iCount).IsSupervisor = True Then
                    ddlUsers.Enabled = True
                End If
            Next

        End Sub

        ''' <summary>
        ''' This method is used for Binding the gridview.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub BindData()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oWorkManager As New NexusProvider.WorkManager
            Dim oworkManagerCollection As New NexusProvider.WorkManagerCollection

            oWorkManager.UserCode = ddlUsers.SelectedValue.ToString()
            oWorkManager.PartyKey = ddlParty.SelectedValue 'WPR29
            If ddlUserGroups.SelectedValue.ToString.Trim = GetLocalResourceObject("llb_UserGroupDefaultText").ToString.Trim Then
                oWorkManager.UserGroupCode = "(All Group)"
            Else
                oWorkManager.UserGroupCode = ddlUserGroups.SelectedValue.ToString()
            End If
            oWorkManager.IsDeleted = False
            'Passing the selected DateRange value to the object.
            Dim oDateRange As NexusProvider.DateRange = DirectCast([Enum].Parse(GetType(NexusProvider.DateRange), ddlDate.SelectedValue), NexusProvider.DateRange)
            oWorkManager.DateRange = oDateRange
            'Passing the selected TaskStatus value to the object.
            Dim oTaskStatus As NexusProvider.TaskStatus = DirectCast([Enum].Parse(GetType(NexusProvider.TaskStatus), ddlTaskStatus.SelectedValue), NexusProvider.TaskStatus)
            oWorkManager.TaskStatus = oTaskStatus
            'Passing the selected TaskStatus value to the object.
            Dim oShowType As NexusProvider.ShowType = DirectCast([Enum].Parse(GetType(NexusProvider.ShowType), ddlShowType.SelectedValue), NexusProvider.ShowType)
            oWorkManager.ShowType = oShowType

            '' CR24
            If Not String.IsNullOrEmpty(txtRefNumber.Text) AndAlso txtRefNumber.Text.Trim.Length > 0 Then
                oWorkManager.ReferenceNumber = txtRefNumber.Text.Trim()
                oWorkManager.UserGroupCode = "(All Group)"
                If ddlUsers.Items.Count > 0 Then
                    For nCount As Integer = 0 To ddlUsers.Items.Count - 1
                        If ddlUsers.Items(nCount).Text.Trim.ToUpper = Session(CNLoginName).ToString.Trim.ToUpper Then
                            oWorkManager.UserCode = ddlUsers.Items(nCount).Text
                            Exit For
                        End If
                    Next
                End If
            Else
                oWorkManager.ReferenceNumber = String.Empty
            End If

            oworkManagerCollection = oWebService.GetWorkManagerScheduledTasks(oWorkManager)
            Session(CNWMWorkManagerCollection) = oworkManagerCollection
            Dim iCount As Integer
            For iCount = 0 To oworkManagerCollection.Count - 1   'This For Loop is to go through the enetire collection for the respective TaskStatus Key
                Select Case oworkManagerCollection(iCount).TaskStatusKey 'This is the Select Case for Enum values of the TaskStatus.
                    Case 0
                        oworkManagerCollection(iCount).TaskStatus = NexusProvider.TaskStatus.[New]
                    Case 1
                        oworkManagerCollection(iCount).TaskStatus = NexusProvider.TaskStatus.InProgress
                    Case 2
                        oworkManagerCollection(iCount).TaskStatus = NexusProvider.TaskStatus.InComplete
                    Case 3
                        oworkManagerCollection(iCount).TaskStatus = NexusProvider.TaskStatus.Complete
                    Case 4
                        oworkManagerCollection(iCount).TaskStatus = NexusProvider.TaskStatus.All
                    Case 5
                        oworkManagerCollection(iCount).TaskStatus = NexusProvider.TaskStatus.NotComplete

                End Select
            Next



            gvWorkManager.Visible = False
            If oworkManagerCollection.Count > 0 Then
                gvWorkManager.AllowPaging = True
                '' CR24 - customize page size of grid view
                If ConfigurationManager.AppSettings("WorkManagerPageSize") IsNot Nothing Then
                    gvWorkManager.PageSize = CInt(ConfigurationManager.AppSettings("WorkManagerPageSize"))
                End If
                If Not String.IsNullOrEmpty(oWorkManagerFilter.PageIndex) Then
                    gvWorkManager.PageIndex = CInt(oWorkManagerFilter.PageIndex)
                End If
                If oWorkManagerFilter.SortDirection Is Nothing Then
                    oWorkManagerFilter.SortDirection = SortDirection.Descending
                    oWorkManagerFilter.SortExpression = "UserCode"
                Else
                    oworkManagerCollection.SortColumn = oWorkManagerFilter.SortExpression
                    oworkManagerCollection.SortingOrder = oWorkManagerFilter.SortDirection
                    oworkManagerCollection.Sort()
                End If

                gvWorkManager.DataSource = oworkManagerCollection
                gvWorkManager.DataBind()
                gvWorkManager.Visible = True 'TODO:If no data is there, need to display a message.
                ViewState("SortDirection") = SortDirection.Ascending
                ViewState("SortExpression") = "DueDate"
            End If
            oWorkManagerFilter.DropDownDateValue = ddlDate.SelectedValue
            oWorkManagerFilter.DropDownTaskStatusValue = ddlTaskStatus.SelectedValue
            oWorkManagerFilter.DropDownShowTypeValue = ddlShowType.SelectedValue
            oWorkManagerFilter.DropDownUserGroupsValue = ddlUserGroups.SelectedValue.ToString()
            oWorkManagerFilter.DropDownUsersValue = ddlUsers.SelectedValue.ToString()
            oWorkManagerFilter.DropDownPartyValue = ddlParty.SelectedValue
            oWorkManagerFilter.ReferenceNumber = txtRefNumber.Text
            Session(kWorkManagerFilter) = oWorkManagerFilter
        End Sub

        Protected Sub gvWorkManager_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvWorkManager.Load
            If gvWorkManager.PageCount = 1 Then
                gvWorkManager.AllowPaging = False
            End If
        End Sub

        Protected Sub gvWorkManager_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvWorkManager.PageIndexChanging
            CType(sender, GridView).PageIndex = e.NewPageIndex
            oWorkManagerFilter.PageIndex = e.NewPageIndex
            CType(sender, GridView).DataSource = Session(CNWMWorkManagerCollection)
            CType(sender, GridView).DataBind()
        End Sub

        Protected Sub gvWorkManager_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvWorkManager.RowCommand
            If e.CommandName = "Select" Then
                Session(CNWMTaskInstanceKey) = e.CommandArgument
                Try
                    'Unlock policy for same user
                    Dim nInsuranceFolderKey As Integer
                    If Not LCase(e.CommandName).Equals("page") And Not LCase(e.CommandName).Equals("sort") Then
                        Dim GridRow As GridViewRow
                        GridRow = CType((e.CommandSource).NamingContainer, GridViewRow)
                        Dim lblInsuranceFolderKey As Label = GridRow.FindControl("lblInsuranceFolderKey")
                        nInsuranceFolderKey = CInt(lblInsuranceFolderKey.Text)
                    End If
                    UnlockPolicy(nInsuranceFolderKey)
                    StartTask()
                Catch ex As NexusProvider.NexusException
                    Select Case CType(ex.Errors(0), NexusProvider.NexusError).Code
                        Case "1000148" 'Policy Locking
                            'Show policy locking error as alert
                            Dim sMessage As String = "alert('" + Replace(GetLocalResourceObject("lbl_policylocked_error"), "{1}", (ex.Errors(0).Detail.Split(":"))(1) + ".") + "')"
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage, True)
                        Case Else
                            Throw ex
                    End Select
                End Try
            End If
        End Sub
        ''' <summary>
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvWorkManager_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvWorkManager.RowDataBound

            If e.Row.RowType = DataControlRowType.DataRow Then
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("TaskInstanceKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.WorkManager).TaskInstanceKey)

                e.Row.Cells(3).Text = Convert.ToDateTime(e.Row.Cells(3).Text).Date
                If e.Row.Cells(2).Text.Trim = "Complete" Then
                    e.Row.Cells(0).Enabled = False
                End If

                Select Case e.Row.Cells(1).Text
                    Case "True"
                        e.Row.Cells(1).Text = GetLocalResourceObject("True").ToString()
                    Case "False"
                        e.Row.Cells(1).Text = GetLocalResourceObject("False").ToString()
                End Select

                'Start link will not appear for teh task which can not be started
                Dim ohyplink As LinkButton = e.Row.Cells(0).FindControl("lnkStart")
                ohyplink.CommandArgument = CType(e.Row.DataItem, NexusProvider.WorkManager).TaskInstanceKey
                'Authorize Claim Payments
                'NB and Referal Task
                'Renewal
                'Cash/Cheque Payment Authorisation
                'MTA
                'Open Claim
                'Maintain Claim 
                'Pay Claim
                'Authorize Manual Journal Payments
                If (CType(e.Row.DataItem, NexusProvider.WorkManager).TaskGroupKey = 10 And CType(e.Row.DataItem, NexusProvider.WorkManager).TaskKey = 98) _
                Or (CType(e.Row.DataItem, NexusProvider.WorkManager).TaskGroupKey = 8 And CType(e.Row.DataItem, NexusProvider.WorkManager).TaskKey = 58) _
                Or (CType(e.Row.DataItem, NexusProvider.WorkManager).TaskGroupKey = 11 And CType(e.Row.DataItem, NexusProvider.WorkManager).TaskKey = 69) _
                Or (CType(e.Row.DataItem, NexusProvider.WorkManager).TaskGroupKey = 1 And CType(e.Row.DataItem, NexusProvider.WorkManager).TaskKey = 206) _
                Or (CType(e.Row.DataItem, NexusProvider.WorkManager).TaskGroupKey = 8 And CType(e.Row.DataItem, NexusProvider.WorkManager).TaskKey = 59) _
                Or (CType(e.Row.DataItem, NexusProvider.WorkManager).TaskGroupKey = 8 And CType(e.Row.DataItem, NexusProvider.WorkManager).TaskKey = 622) _
                Or (CType(e.Row.DataItem, NexusProvider.WorkManager).TaskGroupKey = 8 And CType(e.Row.DataItem, NexusProvider.WorkManager).TaskKey = 208) _
                Or (CType(e.Row.DataItem, NexusProvider.WorkManager).TaskGroupKey = 11 And CType(e.Row.DataItem, NexusProvider.WorkManager).TaskKey = 216) _
                Or (CType(e.Row.DataItem, NexusProvider.WorkManager).TaskGroupKey = 9 And CType(e.Row.DataItem, NexusProvider.WorkManager).TaskKey = 60) _
                Or (CType(e.Row.DataItem, NexusProvider.WorkManager).TaskGroupKey = 9 And CType(e.Row.DataItem, NexusProvider.WorkManager).TaskKey = 80) _
                Or (CType(e.Row.DataItem, NexusProvider.WorkManager).TaskGroupKey = 9 And CType(e.Row.DataItem, NexusProvider.WorkManager).TaskKey = 81) _
                Or (CType(e.Row.DataItem, NexusProvider.WorkManager).TaskGroupKey = 11 And CType(e.Row.DataItem, NexusProvider.WorkManager).TaskKey = 216) _
                Or (CType(e.Row.DataItem, NexusProvider.WorkManager).TaskGroupKey = 33 And CType(e.Row.DataItem, NexusProvider.WorkManager).TaskKey = 97) _
                Or (CType(e.Row.DataItem, NexusProvider.WorkManager).TaskGroupKey = 1 And CType(e.Row.DataItem, NexusProvider.WorkManager).TaskKey = 4) _
                    Or (CType(e.Row.DataItem, NexusProvider.WorkManager).TaskGroupKey = 7 And CType(e.Row.DataItem, NexusProvider.WorkManager).TaskKey = 688) _
                Or (CType(e.Row.DataItem, NexusProvider.WorkManager).TaskGroupKey = 1 And CType(e.Row.DataItem, NexusProvider.WorkManager).TaskKey = 21) Then
                    ohyplink.Enabled = True
                Else
                    ohyplink.Enabled = False
                End If

                If e.Row.Cells(5).Text.ToUpper = "NONE" Then
                    e.Row.Visible = False
                End If

            End If
        End Sub

        Sub StartTask()
            'Clear The session
            Session(CNMTAType) = Nothing
            Session(CNRenewal) = Nothing
            Session(CNInsuranceFileKey) = Nothing
            Session(CNPartyKey) = Nothing
            Session(CNClaimKey) = Nothing
            Session(CNClaimNumber) = Nothing
            Session(CNQuoteMode) = Nothing
            Session(CNClientMode) = Nothing
            Session(CNMode) = Nothing
            Session(CNRiskType) = Nothing

            'Variale Declaration
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oWorkManager As New NexusProvider.WorkManager
            Dim iInsuranceFileKey As Integer = 0
            Dim iPartyKey As Integer = 0
            Dim sMtaType As String = Nothing
            Dim sRenewal As String = Nothing
            Dim iCount As Integer = 0

            oWorkManager.TaskInstanceKey = Session(CNWMTaskInstanceKey)
            oWebService.GetWmTask(oWorkManager)

            For iCount = 0 To oWorkManager.KeyData.Count - 1
                If oWorkManager.KeyData(iCount).KeyName IsNot Nothing Then
                    If oWorkManager.KeyData(iCount).KeyName.Trim = "MtaType" Then
                        Session(CNMTAType) = oWorkManager.KeyData(iCount).KeyValue
                    ElseIf oWorkManager.KeyData(iCount).KeyName.Trim = "Renewal" Then
                        Session(CNRenewal) = oWorkManager.KeyData(iCount).KeyValue
                    ElseIf oWorkManager.KeyData(iCount).KeyName.Trim.ToUpper = "INSURANCEFILEKEY" Or oWorkManager.KeyData(iCount).KeyName.Trim.ToUpper = "INSURANCE_FILE_CNT" Then
                        Session(CNInsuranceFileKey) = oWorkManager.KeyData(iCount).KeyValue
                    ElseIf oWorkManager.KeyData(iCount).KeyName.Trim = "PartyKey" Then
                        Session(CNPartyKey) = oWorkManager.KeyData(iCount).KeyValue
                    ElseIf oWorkManager.KeyData(iCount).KeyName.Trim = "claim_id" Then
                        Session(CNClaimKey) = oWorkManager.KeyData(iCount).KeyValue
                    ElseIf oWorkManager.KeyData(iCount).KeyName.Trim = "claim_number" Then
                        Session(CNClaimNumber) = oWorkManager.KeyData(iCount).KeyValue
                    ElseIf oWorkManager.KeyData(iCount).KeyName.Trim = "ManualJournalId" Then
                        Session(CNAuthoriseManualJournalTransactions) = oWorkManager.KeyData(iCount).KeyValue
                    End If
                End If
            Next
            'Reset The Valid Values in Session
            If Session(CNMTAType) = "None" Then
                Session(CNMTAType) = Nothing
            End If

            Dim bExclusiveLock As Boolean = True
            If oWorkManager.TaskGroupKey = 10 And oWorkManager.TaskKey = 98 Then
                'Authorize Claim Payments
                Response.Redirect("~/secure/AuthoriseClaimPayments.aspx?claim_key=" & CType(Session(CNClaimKey), String) & "&claim_number=" & CType(Session(CNClaimNumber), String))

            ElseIf oWorkManager.TaskGroupKey = 8 And oWorkManager.TaskKey = 58 Then 'NB and Referal Task
                Dim oQuote As NexusProvider.Quote = Nothing
                If Session(CNInsuranceFileKey) IsNot Nothing Then
                    If CInt(Session(CNInsuranceFileKey)) > 0 Then
                        oQuote = oWebService.GetHeaderAndSummariesByKey(CInt(Session(CNInsuranceFileKey)))
                        Session(CNQuote) = oQuote
                    End If
                End If
                If oQuote IsNot Nothing Then
                    'Check whether task has been completed
                    If oQuote.InsuranceFileTypeCode IsNot Nothing Then
                        If oQuote.InsuranceFileTypeCode.Trim.ToUpper = "POLICY" Then
                            CstVldData.IsValid = False
                            Exit Sub
                        End If
                    End If

                    'Populating the CNParty Object
                    Dim oParty As NexusProvider.BaseParty
                    oParty = oWebService.GetParty(oQuote.PartyKey)
                    Session(CNParty) = oParty

                    Session(CNClientMode) = Mode.View
                    Session(CNQuoteInSync) = False
                    'to update the oQuote since Quote status has been updated
                    oQuote = oWebService.GetHeaderAndSummariesByKey(oQuote.InsuranceFileKey, bExclusiveLock:=bExclusiveLock)
                    For jCount As Integer = 0 To oQuote.Risks.Count - 1
                        oWebService.GetRisk(oQuote.Risks(jCount).Key, jCount, oQuote, oQuote.BranchCode)
                    Next
                    Session(CNRenewal) = Nothing
                    Session(CNMTAType) = Nothing
                    LoadQuote(oQuote)
                End If

            ElseIf oWorkManager.TaskGroupKey = 8 And oWorkManager.TaskKey = 59 Then 'MTA
                Dim oQuote As NexusProvider.Quote = Nothing
                If Session(CNInsuranceFileKey) IsNot Nothing Then
                    If CInt(Session(CNInsuranceFileKey)) > 0 Then
                        oQuote = oWebService.GetHeaderAndSummariesByKey(CInt(Session(CNInsuranceFileKey)))
                        Session(CNQuote) = oQuote
                    End If
                End If
                If oQuote IsNot Nothing Then
                    'Check whether task has been completed
                    If oQuote.InsuranceFileTypeCode IsNot Nothing Then
                        If oQuote.InsuranceFileTypeCode.Trim.ToUpper = "MTA PERM" Or oQuote.InsuranceFileTypeCode.Trim.ToUpper = "MTA TEMP" Then
                            CstVldData.IsValid = False
                            Exit Sub
                        End If
                    End If
                    'Populating the CNParty Object
                    Dim oParty As NexusProvider.BaseParty
                    oParty = oWebService.GetParty(oQuote.PartyKey)
                    Session(CNParty) = oParty

                    Session(CNClientMode) = Mode.View
                    Session(CNQuoteInSync) = False
                    'to update the oQuote since Quote status has been updated
                    oQuote = oWebService.GetHeaderAndSummariesByKey(oQuote.InsuranceFileKey, bExclusiveLock:=bExclusiveLock)
                    For jCount As Integer = 0 To oQuote.Risks.Count - 1
                        oWebService.GetRisk(oQuote.Risks(jCount).Key, jCount, oQuote, oQuote.BranchCode)
                    Next

                    Session(CNRenewal) = Nothing
                    'before proceding BUY MTAQUOTE we need to check if the policy already have existing MTA
                    Session(CNMtaReasonSelected) = Nothing
                    Dim oPolicy As NexusProvider.PolicyCollection
                    Dim TempVar As Integer
                    Dim SelMTAQuoteStartDate, ExistingMTAStartDate As Date
                    oWebService = New NexusProvider.ProviderManager().Provider
                    oPolicy = oWebService.GetAllPolicyVersions(oQuote.InsuranceFolderKey)
                    For TempVar = 0 To oPolicy.Count - 1
                        If oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "MTA PERM" Then
                            SelMTAQuoteStartDate = oQuote.CoverStartDate
                            ExistingMTAStartDate = oPolicy.Item(TempVar).CoverStartDate
                            If SelMTAQuoteStartDate < ExistingMTAStartDate Then
                                'if yes then User cant proceed
                                CstVldMTA.Enabled = True
                                CstVldMTA.IsValid = False
                                Exit Sub
                            Else
                                CstVldMTA.Enabled = False
                                CstVldMTA.IsValid = True
                            End If
                        End If
                    Next

                    Session(CNQuote) = oQuote
                    If oQuote.InsuranceFileTypeCode.Trim() = "MTAQCAN" Then
                        Session(CNMTAType) = MTAType.CANCELLATION
                    Else
                        Session(CNMTAType) = MTAType.PERMANENT
                    End If
                    Session(CNQuoteMode) = QuoteMode.FullQuote
                    LoadQuote(oQuote)
                End If

            ElseIf oWorkManager.TaskGroupKey = 11 And oWorkManager.TaskKey = 69 Then 'Renewal

                Dim oQuote As NexusProvider.Quote = Nothing
                If Session(CNInsuranceFileKey) IsNot Nothing Then
                    If CInt(Session(CNInsuranceFileKey)) > 0 Then
                        oQuote = oWebService.GetHeaderAndSummariesByKey(CInt(Session(CNInsuranceFileKey)))
                        Session(CNQuote) = oQuote
                    End If
                End If
                If oQuote IsNot Nothing Then
                    'Check whether task has been completed
                    If oQuote.InsuranceFileTypeCode IsNot Nothing Then
                        If oQuote.InsuranceFileTypeCode.Trim.ToUpper = "POLICY" Then
                            CstVldData.IsValid = False
                            Exit Sub
                        End If
                    End If
                    'Populating the CNParty Object
                    Dim oParty As NexusProvider.BaseParty
                    oParty = oWebService.GetParty(oQuote.PartyKey)
                    Session(CNParty) = oParty

                    Session(CNClientMode) = Mode.View

                    'to update the oQuote since Quote status has been updated
                    oQuote = oWebService.GetHeaderAndSummariesByKey(oQuote.InsuranceFileKey, bExclusiveLock:=bExclusiveLock)
                    For jCount As Integer = 0 To oQuote.Risks.Count - 1
                        oWebService.GetRisk(oQuote.Risks(jCount).Key, jCount, oQuote, oQuote.BranchCode)
                    Next
                    ResetTransactionInSession()
                    Session(CNMTAType) = Nothing
                    Session(CNRenewal) = True
                    LoadQuote(oQuote)
                End If
            ElseIf oWorkManager.TaskGroupKey = 1 And oWorkManager.TaskKey = 206 Then  'Cash/Cheque Payment Authorisation
                Dim iCashListItemKey As Integer
                For iCount = 0 To oWorkManager.KeyData.Count - 1
                    If oWorkManager.KeyData(iCount).KeyName IsNot Nothing Then
                        If oWorkManager.KeyData(iCount).KeyName.Trim.ToUpper = "CASHLISTITEM_ID" Then
                            iCashListItemKey = oWorkManager.KeyData(iCount).KeyValue
                        End If
                    End If
                Next
                'Response.Redirect("~/secure/payment/cashListItem.aspx?Type=Task&CashListItemKey=" & iCashListItemKey & "", False)
                Session.Remove(CNSearchPaymentAuthorization)
                Response.Redirect("~/secure/AuthorizePayments.aspx?Type=Task&CashListItemKey=" & iCashListItemKey & "", False)
            ElseIf oWorkManager.TaskGroupKey = 1 And oWorkManager.TaskKey = 4 Then
                Response.Redirect("~/secure/SearchTransactions.aspx", False)
            ElseIf oWorkManager.TaskGroupKey = 7 And oWorkManager.TaskKey = 688 Then  'AuthorizeManualJournal
                Response.Redirect("~/secure/ManualJournal.aspx?ManualJournalKey=" & CType(Session(CNAuthoriseManualJournalTransactions), String) & "&Mode=WM", False)
            ElseIf oWorkManager.TaskGroupKey = 1 And oWorkManager.TaskKey = 21 Then  'Cash/Cheque Receipt
                Response.Redirect("~/secure/payment/cashList.aspx?Mode=Receipt", False)
            ElseIf oWorkManager.TaskGroupKey = 8 And oWorkManager.TaskCode.Trim = "SUMCOV" Then
                Dim oQuote As NexusProvider.Quote = Nothing
                If Session(CNInsuranceFileKey) IsNot Nothing Then
                    If CInt(Session(CNInsuranceFileKey)) > 0 Then
                        oQuote = oWebService.GetHeaderAndSummariesByKey(CInt(Session(CNInsuranceFileKey)))
                        Session(CNQuote) = oQuote
                    End If
                End If
                If oQuote IsNot Nothing Then
                    'Check whether task has been completed
                    If oQuote.InsuranceFileTypeCode IsNot Nothing Then
                        If oQuote.InsuranceFileTypeCode.Trim.ToUpper = "POLICY" Then
                            CstVldData.IsValid = False
                            Exit Sub
                        End If
                    End If

                    'Populating the CNParty Object
                    Dim oParty As NexusProvider.BaseParty
                    oParty = oWebService.GetParty(oQuote.PartyKey)
                    Session(CNParty) = oParty

                    Session(CNClientMode) = Mode.View
                    Session(CNQuoteInSync) = False
                    'to update the oQuote since Quote status has been updated
                    oQuote = oWebService.GetHeaderAndSummariesByKey(oQuote.InsuranceFileKey, bExclusiveLock:=bExclusiveLock)
                    For jCount As Integer = 0 To oQuote.Risks.Count - 1
                        oWebService.GetRisk(oQuote.Risks(jCount).Key, jCount, oQuote)
                    Next
                    Session(CNRenewal) = Nothing
                    Session(CNMTAType) = Nothing
                    LoadQuote(oQuote)
                End If
            ElseIf oWorkManager.TaskGroupKey = 8 And oWorkManager.TaskKey = 208 Then 'Re-instatement
                Dim oQuote As NexusProvider.Quote = Nothing
                If Session(CNInsuranceFileKey) IsNot Nothing Then
                    If CInt(Session(CNInsuranceFileKey)) > 0 Then
                        oQuote = oWebService.GetHeaderAndSummariesByKey(CInt(Session(CNInsuranceFileKey)))
                        Session(CNQuote) = oQuote
                    End If
                End If
                If oQuote IsNot Nothing Then
                    'Check whether task has been completed
                    If oQuote.InsuranceFileTypeCode IsNot Nothing Then
                        If oQuote.InsuranceFileTypeCode.Trim.ToUpper = "MTA PERM" Or oQuote.InsuranceFileTypeCode.Trim.ToUpper = "MTA TEMP" Then
                            CstVldData.IsValid = False
                            Exit Sub
                        End If
                    End If
                    'Populating the CNParty Object
                    Dim oParty As NexusProvider.BaseParty
                    oParty = oWebService.GetParty(oQuote.PartyKey)
                    Session(CNParty) = oParty

                    Session(CNClientMode) = Mode.View
                    Session(CNQuoteInSync) = False
                    'to update the oQuote since Quote status has been updated
                    oQuote = oWebService.GetHeaderAndSummariesByKey(oQuote.InsuranceFileKey, bExclusiveLock:=bExclusiveLock)
                    For jCount As Integer = 0 To oQuote.Risks.Count - 1
                        oWebService.GetRisk(oQuote.Risks(jCount).Key, jCount, oQuote)
                    Next

                    Session(CNRenewal) = Nothing
                    'before proceding BUY MTAQUOTE we need to check if the policy already have existing MTA
                    Session(CNMtaReasonSelected) = Nothing
                    Dim oPolicy As NexusProvider.PolicyCollection
                    Dim TempVar As Integer
                    Dim SelMTAQuoteStartDate, ExistingMTAStartDate As Date
                    oWebService = New NexusProvider.ProviderManager().Provider
                    oPolicy = oWebService.GetAllPolicyVersions(oQuote.InsuranceFolderKey)
                    For TempVar = 0 To oPolicy.Count - 1
                        If oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "MTA PERM" Then
                            SelMTAQuoteStartDate = oQuote.CoverStartDate
                            ExistingMTAStartDate = oPolicy.Item(TempVar).CoverStartDate
                            If SelMTAQuoteStartDate < ExistingMTAStartDate Then
                                'if yes then User cant proceed
                                CstVldMTA.Enabled = True
                                CstVldMTA.IsValid = False
                                Exit Sub
                            Else
                                CstVldMTA.Enabled = False
                                CstVldMTA.IsValid = True
                            End If
                        End If
                    Next

                    Session(CNQuote) = oQuote
                    Session(CNMTAType) = MTAType.PERMANENT
                    Session(CNQuoteMode) = QuoteMode.FullQuote
                    LoadQuote(oQuote)
                End If
            ElseIf oWorkManager.TaskGroupKey = 11 And oWorkManager.TaskKey = 216 Then 'Renewal Acceptance
                Dim oQuote As NexusProvider.Quote = Nothing
                If Session(CNInsuranceFileKey) IsNot Nothing Then
                    If CInt(Session(CNInsuranceFileKey)) > 0 Then
                        oQuote = oWebService.GetHeaderAndSummariesByKey(CInt(Session(CNInsuranceFileKey)))
                        Session(CNQuote) = oQuote
                    End If
                End If
                If oQuote IsNot Nothing Then
                    'Check whether task has been completed
                    If oQuote.InsuranceFileTypeCode IsNot Nothing Then
                        If oQuote.InsuranceFileTypeCode.Trim.ToUpper = "MTA PERM" Or oQuote.InsuranceFileTypeCode.Trim.ToUpper = "MTA TEMP" Then
                            CstVldData.IsValid = False
                            Exit Sub
                        End If
                    End If
                    'Populating the CNParty Object
                    Dim oParty As NexusProvider.BaseParty
                    oParty = oWebService.GetParty(oQuote.PartyKey)
                    Session(CNParty) = oParty

                    Session(CNClientMode) = Mode.View
                    Session(CNQuoteInSync) = False
                    'to update the oQuote since Quote status has been updated
                    oQuote = oWebService.GetHeaderAndSummariesByKey(oQuote.InsuranceFileKey, bExclusiveLock:=bExclusiveLock)
                    For jCount As Integer = 0 To oQuote.Risks.Count - 1
                        oWebService.GetRisk(oQuote.Risks(jCount).Key, jCount, oQuote)
                    Next

                    Session(CNRenewal) = Nothing
                    'before proceding BUY MTAQUOTE we need to check if the policy already have existing MTA
                    Session(CNMtaReasonSelected) = Nothing
                    Dim oPolicy As NexusProvider.PolicyCollection
                    Dim TempVar As Integer
                    Dim SelMTAQuoteStartDate, ExistingMTAStartDate As Date
                    oWebService = New NexusProvider.ProviderManager().Provider
                    oPolicy = oWebService.GetAllPolicyVersions(oQuote.InsuranceFolderKey)
                    For TempVar = 0 To oPolicy.Count - 1
                        If oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "MTA PERM" Then
                            SelMTAQuoteStartDate = oQuote.CoverStartDate
                            ExistingMTAStartDate = oPolicy.Item(TempVar).CoverStartDate
                            If SelMTAQuoteStartDate < ExistingMTAStartDate Then
                                'if yes then User cant proceed
                                CstVldMTA.Enabled = True
                                CstVldMTA.IsValid = False
                                Exit Sub
                            Else
                                CstVldMTA.Enabled = False
                                CstVldMTA.IsValid = True
                            End If
                        End If
                    Next

                    Session(CNQuote) = oQuote
                    Session(CNMTAType) = MTAType.PERMANENT
                    Session(CNQuoteMode) = QuoteMode.FullQuote
                    LoadQuote(oQuote)
                End If

            ElseIf oWorkManager.TaskGroupKey = 9 OrElse oWorkManager.TaskKey = 60 _
            Or oWorkManager.TaskGroupKey = 9 OrElse oWorkManager.TaskKey = 80 _
            Or oWorkManager.TaskGroupKey = 9 And oWorkManager.TaskKey = 81 Then 'open, maintain and pay claim
                Dim iClaimNumber As String
                Dim iPolicyNumber As String
                For iCount = 0 To oWorkManager.KeyData.Count - 1
                    If oWorkManager.KeyData(iCount).KeyName IsNot Nothing Then
                        'In case WMTask is created while doing maintain claim or pay claim
                        If oWorkManager.KeyData(iCount).KeyName.Trim.ToUpper() = "CLAIMNUMBER" Then
                            iClaimNumber = oWorkManager.KeyData(iCount).KeyValue
                            Response.Redirect("~/Claims/FindClaim.aspx?Claimno=" & iClaimNumber, False)
                            Exit For
                            'In case WMTask is created while doing open claim
                        ElseIf oWorkManager.KeyData(iCount).KeyName.Trim.ToUpper() = "POLICYNUMBER" Then
                            iPolicyNumber = oWorkManager.KeyData(iCount).KeyValue
                            Response.Redirect("~/Claims/FindInsuranceFile.aspx?Policyno=" & iPolicyNumber, False)
                            Exit For
                            'In case new WMTask is created for open claim from the work manager page
                        ElseIf oWorkManager.TaskKey = 60 Then
                            Response.Redirect("~/Claims/FindInsuranceFile.aspx?", False)
                            Exit For
                            'In case new WMTask is created for  maintain claim or pay claim from the work manager page
                        ElseIf oWorkManager.TaskKey = 80 Or oWorkManager.TaskKey = 81 Then
                            Response.Redirect("~/Claims/FindClaim.aspx?Claimno=", False)
                            Exit For
                        End If
                    End If
                Next

            End If
        End Sub
        Private Sub ResetTransactionInSession()
            Session.Remove(CNMTAType)
            Session.Remove(CNMTATypeDesc)
            Session.Remove(CNRenewal)
            Session.Remove(CNRenewalShowPremium)
        End Sub
        Sub LoadQuote(ByVal oQuote As NexusProvider.Quote)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sRedirectPath As String
            oWebService.GetHeaderAndRisksByKey(oQuote)
            Session(CNQuote) = oQuote
            Session(CNCurrenyCode) = oQuote.CurrencyCode
            GetDataSetDefinition()
            Session.Remove(CNOI)
            Session(CNInsuranceFileKey) = oQuote.InsuranceFileKey
            Session(CNQuoteInSync) = False
            Session.Item(CNMode) = Mode.Buy
            Session(CNQuoteMode) = QuoteMode.FullQuote
            '[start] Changes as per WPR 73_74
            DataSetFunctions.GetScreens()
            If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                sRedirectPath = DataSetFunctions.sSummaryOfCoverURL
                If Session(CNMTAType) Is Nothing And Session(CNRenewal) Is Nothing Then
                    If (oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.Issued) Then
                        Session.Item(CNMode) = Mode.Buy
                        If Not UserCanDoTask("NewBusiness") Then
                            Session.Item(CNMode) = Mode.View
                        End If
                        Session(CNQuoteMode) = QuoteMode.FullQuote
                        Response.Redirect(sRedirectPath, False)
                    Else
                        'Session(CNMode) = Mode.View 
                        Session.Item(CNMode) = Mode.Buy 'Above line has been commented and new code has been added to make the quote status as buyable. This is implemented as a workaround for now (as per discussion with Gaurav)
                        If Not UserCanDoTask("NewBusiness") Then
                            Session.Item(CNMode) = Mode.View
                        End If
                        Response.Redirect(sRedirectPath)
                    End If
                Else
                    Session.Item(CNMode) = Mode.Buy
                    If Session(CNMTAType) IsNot Nothing Or Session(CNRenewal) IsNot Nothing Then
                        If Session(CNMTAType) = MTAType.PERMANENT And Not UserCanDoTask("MidTermAdjustment") Then
                            Session.Item(CNMode) = Mode.View
                        ElseIf Session(CNMTAType) = MTAType.CANCELLATION And Not UserCanDoTask("MidTermCancellation") Then
                            Session.Item(CNMode) = Mode.View
                        ElseIf Session(CNMTAType) = MTAType.REINSTATEMENT And Not UserCanDoTask("MidTermReinstatement") Then
                            Session.Item(CNMode) = Mode.View
                        ElseIf Session(CNMTAType) = MTAType.TEMPORARY And Not UserCanDoTask("MidTermAdjustmentTemp") Then
                            Session.Item(CNMode) = Mode.View
                        ElseIf Session(CNRenewal) IsNot Nothing And Not UserCanDoTask("Renewals") Then
                            Session.Item(CNMode) = Mode.View
                        End If
                    End If
                    Response.Redirect(sRedirectPath)
                End If
            Else
                Session.Item(CNMode) = Mode.Buy
                If Session(CNMTAType) IsNot Nothing Or Session(CNRenewal) IsNot Nothing Then
                    If Session(CNMTAType) = MTAType.PERMANENT And Not UserCanDoTask("MidTermAdjustment") Then
                        Session.Item(CNMode) = Mode.View
                    ElseIf Session(CNMTAType) = MTAType.CANCELLATION And Not UserCanDoTask("MidTermCancellation") Then
                        Session.Item(CNMode) = Mode.View
                    ElseIf Session(CNMTAType) = MTAType.REINSTATEMENT And Not UserCanDoTask("MidTermReinstatement") Then
                        Session.Item(CNMode) = Mode.View
                    ElseIf Session(CNMTAType) = MTAType.TEMPORARY And Not UserCanDoTask("MidTermAdjustmentTemp") Then
                        Session.Item(CNMode) = Mode.View
                    ElseIf Session(CNRenewal) IsNot Nothing And Not UserCanDoTask("Renewals") Then
                        Session.Item(CNMode) = Mode.View
                    End If
                End If

                If Session(CNMTAType) Is Nothing And Session(CNRenewal) Is Nothing And Not UserCanDoTask("NewBusiness") Then
                    Session.Item(CNMode) = Mode.View
                End If

                sRedirectPath = "~/secure/PremiumDisplay.aspx"
                Response.Redirect(sRedirectPath, False)
            End If
            '[end] Changes as per WPR 73_74
        End Sub

        Protected Sub ddlShowType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlShowType.SelectedIndexChanged
            If ddlShowType.SelectedValue.Trim = "Sys" Then
                ddlUserGroups.Enabled = False
                ddlTaskStatus.Enabled = False
                ddlUsers.Enabled = False
            Else
                ddlUserGroups.Enabled = True
                ddlTaskStatus.Enabled = True
                ddlUsers.Enabled = True
            End If
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If Not IsPostBack Then
                Session(CNWMMode) = WMMode.Edit
            End If
        End Sub

        Protected Sub gvWorkManager_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvWorkManager.Sorting
            'sort the work manager entries according to the column clicked
            Dim oWorkManagerCollection As NexusProvider.WorkManagerCollection = Session(CNWMWorkManagerCollection)
            oWorkManagerCollection.SortColumn = e.SortExpression

            'we need to store the current sort order in viewstate, and reverse it each time
            'check that the sort expression is the same as stored in viewstate as we should start again if reordering by a new column
            Dim _sortDirection As New SortDirection
            If oWorkManagerFilter.SortDirection = SortDirection.Ascending And oWorkManagerFilter.SortExpression = e.SortExpression Then
                _sortDirection = SortDirection.Descending
            Else
                _sortDirection = SortDirection.Ascending
            End If
            'store the current sortdirection for comparison on the next sort
            oWorkManagerFilter.SortDirection = _sortDirection
            ViewState("SortDirection") = _sortDirection
            'store the SortExpression in viewstate so that we can check if we are sorting by a new column on the next sort
            oWorkManagerFilter.SortExpression = e.SortExpression
            ViewState("SortExpression") = e.SortExpression
            Session(kWorkManagerFilter) = oWorkManagerFilter
            oWorkManagerCollection.SortingOrder = _sortDirection
            oWorkManagerCollection.Sort()

            CType(sender, GridView).DataSource = oWorkManagerCollection
            CType(sender, GridView).DataBind()
        End Sub

        Private Sub UnlockPolicy(ByVal nInsuranceFolderCnt As Integer)
            Dim oLockCollection As NexusProvider.LockCollection
            Dim oWebService As NexusProvider.ProviderBase = Nothing
            Dim sUserName As String = String.Empty
            Dim bMaintainedSuccess As Boolean = False
            Dim bLogout As Boolean = False
            Dim bAllClear As Boolean = False
            Dim oLock As New NexusProvider.Locks
            oWebService = New NexusProvider.ProviderManager().Provider
            oLockCollection = oWebService.GetLockDetails(Session(CNBranchCode).ToString())

            For Each oLockItem As NexusProvider.Locks In oLockCollection
                If HttpContext.Current.User.Identity.Name.ToLower().Trim() = oLockItem.LockUserName.ToLower().Trim() AndAlso oLockItem.LockName.Trim() = "insurance_folder_cnt" AndAlso oLockItem.LockValue = nInsuranceFolderCnt Then
                    oLock.LockName = oLockItem.LockName
                    oLock.LockValue = oLockItem.LockValue
                    oLockCollection.Add(oLock)
                    bMaintainedSuccess = oWebService.MaintainLock(oLockCollection, bAllClear, bLogout, Session(CNBranchCode).ToString())
                    Exit For
                End If
            Next
        End Sub
        ''' <summary>
        ''' This event is fired on Update Button Click and based on the selected values of the dropdown list the SAM method is fired with the specified input.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
            Session(CNWMMode) = WMMode.Edit
            If Not String.IsNullOrEmpty(oWorkManagerFilter.PageIndex) Then
                oWorkManagerFilter.PageIndex = 0
            End If
            BindData()
        End Sub

        ''' <summary>
        ''' This event is fired on Clear Button Click for clearing the filter settings.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
            ClearWorkManager()
            ClearQuote()
            ClearClaims()
            ClearHeader()
            HttpContext.Current.Session.Remove(kWorkManagerFilter)
            txtRefNumber.Text = String.Empty
            gvWorkManager.Visible = False
            SetFilters()
        End Sub

        Private Sub SetFilters()
            Page.SetFocus(ddlTaskStatus)

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oWorkManager As NexusProvider.WorkManager = Nothing
            Dim UserGroup As NexusProvider.UserGroupCollection = Nothing
            Dim oUser As NexusProvider.UserCollection = Nothing
            Dim NewUser As New NexusProvider.UserGroup()
            Dim oNewUser As New NexusProvider.User()
            Dim oUserDetails As NexusProvider.UserDetails
            Dim oPartySearchCriteria As NexusProvider.PartySearchCriteria
            Dim oPartyCollection As NexusProvider.PartyCollection
            Dim oNewAgent As New NexusProvider.BaseParty()
            oPartySearchCriteria = New NexusProvider.PartySearchCriteria()
            'Binding Data to TaskStatus DropDownList using Enum Value from the Nexus Provider
            ddlTaskStatus.DataSource = [Enum].GetNames(GetType(NexusProvider.TaskStatus))
            ddlTaskStatus.DataBind()
            ddlTaskStatus.SelectedIndex = ddlTaskStatus.Items.IndexOf _
                    (ddlTaskStatus.Items.FindByText(NexusProvider.TaskStatus.InComplete.ToString()))
            'Binding Data to UserGroup DropDownList
            'UserGroup = oWebService.GetUserGroups()
            oUserDetails = oWebService.GetUserDetails(HttpContext.Current.User.Identity.Name)
            If oUserDetails IsNot Nothing AndAlso oUserDetails.PartyType = "OTTHIRD" Then
                oPartySearchCriteria.PartyType = NexusProvider.PartyTypeType.OTTHIRD
                oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTTHIRD)
            Else
                oPartySearchCriteria.PartyType = NexusProvider.PartyTypeType.AG
                oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.AG)
            End If
            If Not oUserDetails.AvailableUsergroups Is Nothing Then
                For Each oUserGroup In oUserDetails.AvailableUsergroups
                    If oUserGroup.IsSysAdmin = True Then
                        bChkIfSysAdmin = True
                        Exit For
                    End If
                Next
            End If
            If bChkIfSysAdmin Then
                UserGroup = oWebService.GetUserGroups()
                NewUser.Code = "(All Group)"
                NewUser.Description = "(All Group)"
                NewUser.EffectiveDate = DateTime.Now
                NewUser.IsDeleted = False
                NewUser.IsSysAdmin = True
                NewUser.UserGroupKey = 0
                UserGroup.Add(NewUser)
            Else
                UserGroup = oUserDetails.AvailableUsergroups
                ddlUsers.Enabled = False
                ddlShowType.Visible = False

            End If
            Dim oUserGroupCollection As NexusProvider.UserGroupCollection = New NexusProvider.UserGroupCollection()
            For Each oUserGroup As NexusProvider.UserGroup In UserGroup
                If Not oUserGroup.IsDeleted Then
                    oUserGroupCollection.Add(oUserGroup)
                End If
            Next

            ddlUserGroups.DataSource = oUserGroupCollection
            ddlUserGroups.DataTextField = "Description"
            ddlUserGroups.DataValueField = "Code"
            ddlUserGroups.DataBind()
            If Not bChkIfSysAdmin Then
                ddlUserGroups.Items.Insert(ddlUserGroups.Items.Count, New ListItem(GetLocalResourceObject("llb_UserGroupDefaultText"), GetLocalResourceObject("llb_UserGroupDefaultText")))
            End If
            ddlUserGroups.SelectedIndex = ddlUserGroups.Items.Count - 1
            'If ddlUserGroups.Items.Count > 0 And ddlUserGroups.SelectedIndex = 0 Then
            If ddlUserGroups.Items.Count > 0 Then
                'Bimding data to Users DropDownList
                Dim sUserCode As String = Nothing
                If ddlUserGroups.SelectedValue = "(All Group)" Or ddlUserGroups.SelectedValue.ToString.Trim = GetLocalResourceObject("llb_UserGroupDefaultText").ToString.Trim Then
                    sUserCode = Nothing
                Else
                    sUserCode = ddlUserGroups.SelectedValue.ToString()
                End If
                oUser = oWebService.GetUserGroupUsers(sUserCode, DateTime.Now, False, False)
                ddlUsers.DataSource = oUser
                ddlUsers.DataTextField = "UserName"
                ddlUsers.DataValueField = "UserName"
                ddlUsers.DataBind()
                ddlUsers.Items.Insert(0, New ListItem(GetLocalResourceObject("llb_UsersDefaultText"), GetLocalResourceObject("llb_UsersDefaultText")))

                If ddlUsers.Items.Count > 0 Then
                    For iCount As Integer = 0 To ddlUsers.Items.Count - 1
                        If ddlUsers.Items(iCount).Text.Trim.ToUpper = Session(CNLoginName).ToString.Trim.ToUpper Then
                            ddlUsers.SelectedValue = ddlUsers.Items(iCount).Text
                            Exit For
                        End If
                    Next
                End If
            End If
            'Binding data to Date DropDownlist using Enum Value from the Nexus Provider
            ddlDate.DataSource = [Enum].GetNames(GetType(NexusProvider.DateRange))
            ddlDate.DataBind()

            'Binding data to Party DropDownlist
            oPartyCollection = oWebService.FindParty(oPartySearchCriteria)
            oNewAgent.ShortName = "(All)"
            oNewAgent.Key = 0
            oPartyCollection.Add(oNewAgent)
            ddlParty.DataSource = oPartyCollection
            ddlParty.DataTextField = "ShortName"
            ddlParty.DataValueField = "Key"
            ddlParty.DataBind()
            'If logged in user is agent then set the ddlParty’s value to that Agent 
            'and disable the control else set to (All) and keep it enabled.
            If oUserDetails.PartyType = "AG" Or oUserDetails.PartyType = "OTTHIRD" Then
                ddlParty.Enabled = False
                ddlParty.SelectedValue = oUserDetails.PartyKey
            Else
                ddlParty.Enabled = True
                ddlParty.SelectedValue = 0
            End If
        End Sub
    End Class
End Namespace
