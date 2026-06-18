
Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session
Namespace Nexus


    Partial Class Modal_WrmTask : Inherits CMS.Library.Frontend.clsCMSPage
        ''' <summary>
        '''  This btnOK_Click event is the Click event of the OK button in which the functionality for adding a task as well as for Modifying a task is mentioned.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
            If Page.IsValid Then

                If ddlUserGroup.SelectedIndex = -1 OrElse ddlTask.SelectedIndex = -1 Then
                    Page.ClientScript.RegisterStartupScript(GetType(String), "taskmissing", "alert('" & GetLocalResourceObject("msg_missingtask") & "')", True)
                    Exit Sub
                End If

                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oworkManagerCollection As New NexusProvider.WorkManagerCollection
                Dim oWorkManager As New NexusProvider.WorkManager
                Dim sOpen As String = Session(CNWMMode)
                If ddlUser.SelectedValue.ToString() = "(Any Group Member)" Then
                    oWorkManager.UserCode = ""
                Else
                    oWorkManager.UserCode = ddlUser.SelectedValue.ToString()
                End If
                If sOpen = WMMode.Add Then
                    'session for Add
                    oWorkManager.DueDate = Convert.ToDateTime(txtDueDate.Text & "  " & txtTime.Text)
                    oWorkManager.Client = txtClient.Text
                    oWorkManager.Task = ddlTask.SelectedValue
                    oWorkManager.TaskGroup = ddlTaskGroup.Value
                    oWorkManager.Description = txtDescription.Text
                    If ddlUser.SelectedValue.ToString() = "(Any Group Member)" Then
                        oWorkManager.AllocationUser = ""
                    Else
                        oWorkManager.AllocationUser = ddlUser.SelectedValue.ToString()
                    End If
                    oWorkManager.AllocationUserGroup = ddlUserGroup.SelectedValue.ToString()
                    If chkUrgent.Checked = True Then
                        oWorkManager.IsUrgent = True
                        oWorkManager.IsUrgentForUpdate = 1
                    Else
                        oWorkManager.IsUrgent = False
                        oWorkManager.IsUrgentForUpdate = 0
                    End If
                    If chkComplete.Checked = True Then
                        oWorkManager.IsComplete = True
                    Else
                        oWorkManager.IsComplete = False
                    End If
                    If chkReview.Checked = True Then
                        oWorkManager.IsTaskReview = True
                    Else
                        oWorkManager.IsTaskReview = False
                    End If

                    If Session(CNMTAType) IsNot Nothing Then
                        Dim oWmrk As New NexusProvider.KeyData
                        oWmrk.KeyName = "MtaType"
                        oWmrk.KeyValue = Session(CNMTAType)
                        oWorkManager.KeyData.Add(oWmrk)
                    Else
                        Dim oWmrk As New NexusProvider.KeyData
                        oWmrk.KeyName = "MtaType"
                        oWmrk.KeyValue = "None"
                        oWorkManager.KeyData.Add(oWmrk)
                    End If
                    If Session(CNRenewal) IsNot Nothing Then
                        Dim oWmrk As New NexusProvider.KeyData
                        oWmrk.KeyName = "Renewal"
                        oWmrk.KeyValue = Session(CNRenewal)
                        oWorkManager.KeyData.Add(oWmrk)
                    End If
                    If Session(CNQuote) IsNot Nothing Then
                        Dim oWmrk As New NexusProvider.KeyData
                        oWmrk.KeyName = "InsuranceFileKey"
                        oWmrk.KeyValue = CType(Session(CNQuote), NexusProvider.Quote).InsuranceFileKey
                        oWorkManager.KeyData.Add(oWmrk)
                    End If
                    If Session(CNParty) IsNot Nothing Then
                        Dim oWmrk As New NexusProvider.KeyData
                        oWmrk.KeyName = "PartyKey"
                        oWmrk.KeyValue = CType(Session(CNParty), NexusProvider.BaseParty).Key
                        oWorkManager.KeyData.Add(oWmrk)
                    End If
                    If Session(CNClaimNumber) IsNot Nothing And Session(CNMode) <> 7 Then 'If not open claim
                        Dim oWmrk As New NexusProvider.KeyData
                        oWmrk.KeyName = "ClaimNumber"
                        oWmrk.KeyValue = Session(CNClaimNumber)
                        oWorkManager.KeyData.Add(oWmrk)
                    ElseIf Session(CNPolicyNumber) IsNot Nothing Then 'In case of open claim
                        Dim oWmrk As New NexusProvider.KeyData
                        oWmrk.KeyName = "PolicyNumber"
                        oWmrk.KeyValue = Session(CNPolicyNumber)
                        oWorkManager.KeyData.Add(oWmrk)
                    End If
                    If oWorkManager.TaskGroup IsNot Nothing Then
                        oWorkManager.LockName = NexusProvider.SAMForInsurance.PureService.TaskLockName.InvalidValue
                        oWebService.CreateWmTask(oWorkManager)
                    End If
                Else
                    'session for Edit
                    If sOpen = WMMode.Edit Then
                        Dim oWorkManagerTaskfromSession As NexusProvider.WorkManager = CType(Session.Item(CNWMWorkManagerCollection), NexusProvider.WorkManagerCollection).Item(CType(Request("TaskKey"), Integer))
                        oWorkManager.TaskInstanceKey = Request.QueryString("TaskInstanceKey") 'oWorkManagerTaskfromSession.TaskInstanceKey
                        oWorkManager.DueDate = Convert.ToDateTime(txtDueDate.Text)
                        oWorkManager.Client = txtClient.Text
                        oWorkManager.Description = txtDescription.Text
                        If chkUrgent.Checked = True Then
                            oWorkManager.IsUrgent = True
                            oWorkManager.IsUrgentForUpdate = 1
                        Else
                            oWorkManager.IsUrgent = False
                            oWorkManager.IsUrgentForUpdate = 0
                        End If
                        If chkComplete.Checked = True Then
                            oWorkManager.IsComplete = True
                            oWorkManager.TaskStatusKey = 3 'Enum Value for Complete 

                        Else
                            oWorkManager.IsComplete = False
                            oWorkManager.TaskStatusKey = 0 'Enum Value for New(Default).

                        End If

                        If chkReview.Checked = True Then
                            oWorkManager.IsTaskReview = True
                        Else
                            oWorkManager.IsTaskReview = False
                        End If

                        oWorkManager.UserGroupCode = ddlUserGroup.SelectedValue.ToString()

                        oWorkManager.WmActionType = NexusProvider.WMActionType.Update

                        oWorkManager.TaskTimeStamp = CType(ViewState("WMTaskTimestamp"), Byte())

                        If oWorkManager.TaskGroup IsNot Nothing Then
                            oWorkManager.LockName = NexusProvider.SAMForInsurance.PureService.TaskLockName.InvalidValue
                            oWebService.CreateWmTask(oWorkManager)
                        Else
                            oWebService.UpdateWmTask(oWorkManager)
                        End If
                    End If
                End If
                If Request.QueryString("FromPage") IsNot Nothing Then
                    If Request.QueryString("FromPage") = "WM" Then
                        'Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "Refresh") & ";"
                        'Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
                        Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
                    End If
                Else
                    Response.Redirect("~/secure/WorkManager.aspx", False)
                End If
            End If
        End Sub
        ''' <summary>
        '''  This btnCancel_Click event is the Click event of the Cancel button  all the Textboxes are made empty.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

            If Request.QueryString("FromPage") IsNot Nothing Then
                Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
            Else
                Response.Redirect("~/secure/WorkManager.aspx", False)
            End If

        End Sub
        Sub FillUser()
            If ddlUserGroup.SelectedValue IsNot Nothing Then
                If ddlUserGroup.SelectedValue.Trim.Length > 0 Then
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oUser As NexusProvider.UserCollection
                    oUser = oWebService.GetUserGroupUsers(ddlUserGroup.SelectedValue.ToString().Trim, DateTime.Now, True, True)
                    ddlUser.DataSource = oUser
                    ddlUser.DataTextField = "UserName"
                    ddlUser.DataValueField = "UserName"
                    ddlUser.DataBind()
                    ddlUser.Items.Insert(0, "(Any Group Member)")

                    If ddlUser.Items.Count > 0 Then
                        For iCount As Integer = 0 To ddlUser.Items.Count - 1
                            If ddlUser.Items(iCount).Text.Trim.ToUpper = Session(CNLoginName).ToString.Trim.ToUpper Then
                                ddlUser.SelectedValue = ddlUser.Items(iCount).Text
                                Exit For
                            End If
                        Next
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' To fill details on audit task scree
        ''' </summary>
        ''' <remarks></remarks>
        Sub AuditDetails()

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oWorkManager As New NexusProvider.WorkManager
            Dim oworkManagerResponse As New NexusProvider.WorkManager
            Dim sOpen As String = Session.Item(CNWMMode)

            If sOpen = WMMode.Add Then
                'Logged by details at the time of add task
                txtLoggedBy.Text = Session(CNLoginName)
                txtLoggedDate.Text = Date.Today.ToShortDateString
                txtLoggedTime.Text = Date.Now.ToLongTimeString
            Else
                oWorkManager.TaskInstanceKey = Request.QueryString("TaskInstanceKey")
                oWorkManager.IsDeleted = False
                oworkManagerResponse = oWebService.GetWmTask(oWorkManager)
                'Logged By Details
                txtLoggedBy.Text = oworkManagerResponse.CreatedBy.ToString.Trim
                txtLoggedDate.Text = oworkManagerResponse.DateCreated.ToString("dd MMMM yyyy")
                txtLoggedTime.Text = oworkManagerResponse.DateCreated.ToString("H:mm:ss")
                'Modified By Details
                txtLastModifiedBy.Text = oworkManagerResponse.ModifiedBy.ToString.Trim
                txtLastModifiedDate.Text = oworkManagerResponse.LastModified.ToString("dd MMMM yyyy")
                txtLastModifiedTime.Text = oworkManagerResponse.LastModified.ToString("H:mm:ss")
            End If

        End Sub
        ''' <summary>
        ''' This Pageload Event binds all the related values to the controls on PageLoad
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oWorkManager As New NexusProvider.WorkManager
            Dim UserGroup As NexusProvider.UserGroupCollection
            Dim oUserDetails As NexusProvider.UserDetails
            Dim bChkIfSysAdmin As Boolean = False
            Dim sCallingApp As String = String.Empty
            If Not IsPostBack Then
                If Not Request.QueryString("CallingApp") Is Nothing Then
                    sCallingApp = Request.QueryString("CallingApp")
                End If
                Session(CNWMTaskInstanceKey) = Nothing
                btnDelete.Attributes.Add("onClick", "javascript:return DeleteConfirmation();")
                UserGroup = oWebService.GetUserGroups()
                ddlUserGroup.DataSource = UserGroup
                ddlUserGroup.DataTextField = "Description"
                ddlUserGroup.DataValueField = "Code"
                ddlUserGroup.DataBind()

                If Request.QueryString("TaskInstanceKey") IsNot Nothing Then
                    Session.Item(CNWMMode) = WMMode.Edit
                Else
                    Session.Item(CNWMMode) = WMMode.Add
                End If
                'To fill the details on Audit details screen.
                AuditDetails()
                Dim sOpen As String = Session.Item(CNWMMode)
                If sOpen = WMMode.Add Then
                    Dim sDefaultfocus As String
                    li_LastModifiedBy.Visible = False
                    li_LastModifiedAt.Visible = False
                    btnDelete.Visible = False
                    btnStart.Visible = False
                    btnEdit.Visible = False
                    btnNewEntry.Visible = False
                    txtClient.Enabled = True
                    txtDescription.Enabled = True
                    txtDueDate.Enabled = True
                    txtTime.Enabled = True
                    ddlDueDateTime.Enabled = True
                    ddlTask.Enabled = True
                    ddlTaskGroup.Enabled = True
                    ddlUser.Enabled = True
                    ddlUserGroup.Enabled = True
                    chkComplete.Enabled = True
                    chkUrgent.Enabled = True
                    ddlDueDateTime.SelectedIndex = 1
                    txtDueDate.Text = DateTime.Now().ToShortDateString()
                    txtTime.Text = DateTime.Now().ToLongTimeString()
                    rngvDueDate.MinimumValue = DateTime.Now().ToShortDateString()
                    rngvDueDate.ErrorMessage = GetLocalResourceObject("msg_RangeDuedate")
                    btnAssign.Visible = False
                    btnComplete.Visible = False
                    btnInComplete.Visible = False

                    If sCallingApp = "FinancePlan" Then
                        ddlTaskGroup.Value = "INSTAL"
                        ddlTask.SelectedValue = "PFPLNMAINT"
                        ddlDueDateTime.SelectedValue = "2"
                        If Session(CNInsuranceRef) IsNot Nothing And Session(CNFinancePlanStatus) IsNot Nothing Then
                            txtDescription.Text = Session(CNInsuranceRef) & " Instalment on " & Session(CNFinancePlanStatus)
                        End If
                    End If
                    If Session(CNMTAType) IsNot Nothing And Session(CNRenewal) Is Nothing And sCallingApp.Trim.Length = 0 Then

                        'To set the Focus
                        Page.SetFocus(ddlDueDateTime)
                        If ddlTaskGroup.Items.FindItemByCode("UNDER") IsNot Nothing Then
                            ddlTaskGroup.Value = "UNDER"
                        End If
                        ddlTask.SelectedValue = "UnderMTA"
                        'ddlTaskGroup.Enabled = False
                        'ddlTask.Enabled = False
                        ddlUserGroup.SelectedValue = "SAMADMIN"
                        FillUser()
                        If ddlUser.Items.Count > 0 Then
                            For iCount As Integer = 0 To ddlUser.Items.Count - 1
                                If ddlUser.Items(iCount).Text.Trim.ToUpper = Session(CNLoginName).ToString.Trim.ToUpper Then
                                    ddlUser.SelectedValue = ddlUser.Items(iCount).Text
                                    Exit For
                                End If
                            Next
                        End If
                        'ddlUser.Enabled = False
                        'ddlUserGroup.Enabled = False
                        If Session(CNParty) IsNot Nothing Then
                            Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                            Select Case True
                                Case TypeOf oParty Is NexusProvider.CorporateParty
                                    With CType(oParty, NexusProvider.CorporateParty)
                                        txtClient.Text = .CompanyName
                                    End With
                                Case TypeOf oParty Is NexusProvider.PersonalParty
                                    With CType(oParty, NexusProvider.PersonalParty)
                                        txtClient.Text = .Title & " " & .Forename & " " & .Lastname
                                    End With
                            End Select
                        End If
                        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                        txtClient.Text = Trim(oQuote.ClientCode) & " " & oQuote.InsuranceFileRef
                        txtClient.ReadOnly = True
                        txtDescription.Text = oQuote.InsuranceFileRef

                    ElseIf Session(CNQuoteMode) = QuoteMode.FullQuote And Session(CNRenewal) Is Nothing And sCallingApp.Trim.Length = 0 Then

                        'To set the Focus
                        Page.SetFocus(ddlDueDateTime)

                        If ddlTaskGroup.Items.FindItemByCode("UNDER") IsNot Nothing Then
                            ddlTaskGroup.Value = "UNDER"
                        End If
                        ddlTask.SelectedValue = "UNDERNB"
                        'ddlTaskGroup.Enabled = False
                        'ddlTask.Enabled = False
                        ddlUserGroup.SelectedValue = "SAMADMIN"
                        FillUser()
                        If ddlUser.Items.Count > 0 Then
                            For iCount As Integer = 0 To ddlUser.Items.Count - 1
                                If ddlUser.Items(iCount).Text.Trim.ToUpper = Session(CNLoginName).ToString.Trim.ToUpper Then
                                    ddlUser.SelectedValue = ddlUser.Items(iCount).Text
                                    Exit For
                                End If
                            Next
                        End If
                        'ddlUser.Enabled = False
                        'ddlUserGroup.Enabled = False
                        If Session(CNParty) IsNot Nothing Then
                            Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                            Select Case True
                                Case TypeOf oParty Is NexusProvider.CorporateParty
                                    With CType(oParty, NexusProvider.CorporateParty)
                                        txtClient.Text = .CompanyName
                                    End With
                                Case TypeOf oParty Is NexusProvider.PersonalParty
                                    With CType(oParty, NexusProvider.PersonalParty)
                                        txtClient.Text = .Title & " " & .Forename & " " & .Lastname
                                    End With
                            End Select
                        End If
                        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                        txtClient.Text = Trim(oQuote.ClientCode) & " " & oQuote.InsuranceFileRef
                        txtClient.ReadOnly = True
                        txtDescription.Text = oQuote.InsuranceFileRef

                    ElseIf Session(CNRenewal) IsNot Nothing And sCallingApp.Trim.Length = 0 Then

                        'To set the Focus
                        Page.SetFocus(ddlDueDateTime)

                        ddlTaskGroup.Value = "UWRENEWAL"
                        ddlTask.SelectedValue = "RENAMEND"
                        ddlTaskGroup.Enabled = False
                        ddlTask.Enabled = False
                        ddlUserGroup.SelectedValue = "SAMADMIN"
                        FillUser()
                        If ddlUser.Items.Count > 0 Then
                            For iCount As Integer = 0 To ddlUser.Items.Count - 1
                                If ddlUser.Items(iCount).Text.Trim.ToUpper = Session(CNLoginName).ToString.Trim.ToUpper Then
                                    ddlUser.SelectedValue = ddlUser.Items(iCount).Text
                                    Exit For
                                End If
                            Next
                        End If
                        'ddlUser.Enabled = False
                        'ddlUserGroup.Enabled = False
                        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                        txtClient.Text = Trim(oQuote.ClientCode) & " " & oQuote.InsuranceFileRef
                        txtClient.ReadOnly = True
                        txtDescription.Text = oQuote.InsuranceFileRef
                    ElseIf Session(CNClaimNumber) IsNot Nothing And sCallingApp.Trim.Length = 0 Then
                        txtDescription.Text = Session(CNClaimNumber)
                        ddlTaskGroup.Enabled = True
                        ddlTask.Enabled = True
                        ddlTaskGroup.Value = "CLAIMS"
                        ddlTask.SelectedValue = "CASEMGMT"
                        ddlUserGroup.SelectedValue = "SAMADMIN"
                        FillUser()
                    ElseIf sCallingApp = "DocumentManager" Then
                        'To set the Focus
                        Page.SetFocus(ddlTaskGroup)

                        ddlTaskGroup.Enabled = True
                        ddlTask.Enabled = True
                        ddlTaskGroup.Value = "COMMON"
                        ddlTask.SelectedValue = "MEMO"
                        ddlUserGroup.SelectedIndex = 0
                        FillUser()
                        Dim sClaimNumber As String = String.Empty
                        If Not String.IsNullOrEmpty(Request.QueryString("ClaimKey")) Then
                            Dim iClaimKey As Integer
                            sClaimNumber = Request.QueryString("ClaimNumber")
                            Integer.TryParse(Request.QueryString("ClaimKey"), iClaimKey)

                            If Not String.IsNullOrEmpty(sClaimNumber) Then
                                sClaimNumber = Trim(sClaimNumber)
                            Else
                                Dim oClaim As NexusProvider.ClaimDetails = GetClaimDetailsCall(iClaimKey)
                                sClaimNumber = Trim(oClaim.ClaimNumber)
                            End If
                        End If
                        If Session(CNClaimNumber) IsNot Nothing Then
                            sClaimNumber = Session(CNClaimNumber).ToString
                        End If
                        If sClaimNumber.Trim.Length <> 0 Then
                            txtDescription.Text = sClaimNumber
                        End If
                        Dim sPolicyNumber As String = String.Empty
                        If Not String.IsNullOrEmpty(Request.QueryString("FileKey")) Then
                            Dim iFileKey As Integer
                            Integer.TryParse(Request.QueryString("FileKey"), iFileKey)
                            Dim oQuote As NexusProvider.Quote = oWebService.GetHeaderAndSummariesByKey(iFileKey)
                            sPolicyNumber = Trim(oQuote.InsuranceFileRef)
                        End If

                        If Not Session(CNPolicy_Summary) Is Nothing Then
                            sPolicyNumber = DirectCast(Session(CNPolicy_Summary), NexusProvider.PolicySummary).Reference
                        ElseIf Not Session(CNQuote) Is Nothing Then
                            sPolicyNumber = DirectCast(Session(CNQuote), NexusProvider.Quote).InsuranceFileRef
                        End If
                        If sPolicyNumber.Trim.Length <> 0 Then
                            txtDescription.Text = sPolicyNumber
                        End If
                        If txtDescription.Text.Trim.Length > 0 Then
                            txtDescription.Text = "Document Uploaded - " + txtDescription.Text
                        End If
                    ElseIf sCallingApp = "InsurerPayment" Then
                        'To set the Focus
                        Page.SetFocus(ddlTaskGroup)

                        ddlTaskGroup.Enabled = True
                        ddlTask.Enabled = True
                        ddlTaskGroup.Value = "COMMON"
                        ddlTask.SelectedValue = "MEMO"
                        ddlUserGroup.SelectedValue = "SAMADMIN"
                        txtClient.Text = Request.QueryString("AccountCode")
                        txtClient.ReadOnly = True
                        txtDescription.Text = "Account Query - " + Request.QueryString("DocumentRef")
                        FillUser()
                    End If
                    If Session(CNParty) IsNot Nothing Then
                        Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                        Select Case True
                            Case TypeOf oParty Is NexusProvider.CorporateParty
                                With CType(oParty, NexusProvider.CorporateParty)
                                    txtClient.Text = .CompanyName
                                End With
                            Case TypeOf oParty Is NexusProvider.PersonalParty
                                With CType(oParty, NexusProvider.PersonalParty)
                                    txtClient.Text = .Title & " " & .Forename & " " & .Lastname
                                End With
                        End Select
                    End If
                ElseIf sOpen = WMMode.Edit Then

                    'To set the Focus
                    Page.SetFocus(ddlTaskGroup)

                    Dim oworkManagerCollection As New NexusProvider.WorkManagerCollection
                    Session(CNWMTaskInstanceKey) = Request.QueryString("TaskInstanceKey")
                    btnEdit.Enabled = True
                    btnStart.Visible = True
                    btnEdit.Visible = True
                    txtClient.Enabled = False
                    txtDescription.Enabled = False
                    txtDueDate.Enabled = False
                    txtTime.Enabled = False
                    ddlDueDateTime.Enabled = False
                    ddlTask.Enabled = False
                    ddlTaskGroup.Enabled = False
                    ddlUser.Enabled = False
                    ddlUserGroup.Enabled = False
                    chkComplete.Enabled = True
                    chkUrgent.Enabled = False
                    chkReview.Enabled = False
                    btnComplete.Enabled = False
                    btnInComplete.Enabled = False
                    oUserDetails = oWebService.GetUserDetails(HttpContext.Current.User.Identity.Name)
                    If Not oUserDetails.AvailableUsergroups Is Nothing Then
                        For Each oUserGroup In oUserDetails.AvailableUsergroups
                            If oUserGroup.IsSysAdmin = True Then
                                bChkIfSysAdmin = True
                                Exit For
                            End If
                        Next
                    End If

                    Dim oworkManagerResponse As New NexusProvider.WorkManager
                    oWorkManager.TaskInstanceKey = Session(CNWMTaskInstanceKey)
                    oWorkManager.IsDeleted = False

                    oworkManagerResponse = oWebService.GetWmTask(oWorkManager)
                    txtClient.Text = oworkManagerResponse.Customer
                    If oworkManagerResponse.IsUrgent = True Then
                        chkUrgent.Checked = True
                    Else
                        chkUrgent.Checked = False
                    End If
                    If oworkManagerResponse.IsTaskReview = True Then
                        chkReview.Checked = True
                    Else
                        chkReview.Checked = False
                    End If
                    If oworkManagerResponse.TaskStatusKey = 3 Then 'If the task is complete
                        chkComplete.Checked = True
                        btnEdit.Enabled = False
                        btnAssign.Enabled = False
                        btnComplete.Enabled = False
                        btnStart.Enabled = False
                        btnNewEntry.Enabled = False
                        'status can be changed to incomplete only by sysadmin user or if task is allocated to logged in user.
                        If bChkIfSysAdmin Or oworkManagerResponse.UserCode.ToUpper.Trim = Session(CNLoginName).ToString.ToUpper.Trim Then
                            btnInComplete.Enabled = True
                            btnDelete.Enabled = True
                        End If
                    Else
                        chkComplete.Checked = False
                        btnEdit.Enabled = True
                        btnAssign.Enabled = True
                        btnNewEntry.Enabled = True
                        'Only a sysadmin user can change the status to complete or if task is allocated to logged in user.
                        If bChkIfSysAdmin Or oworkManagerResponse.UserCode.ToUpper.Trim = Session(CNLoginName).ToString.ToUpper.Trim Then
                            btnComplete.Enabled = True
                        End If
                        btnInComplete.Enabled = False
                        btnDelete.Enabled = False
                    End If

                    txtDescription.Text = oworkManagerResponse.Description
                    If (oworkManagerResponse.DueDate.Date = Date.Today()) Then
                        ddlDueDateTime.SelectedIndex = 1
                    ElseIf (oworkManagerResponse.DueDate.Date = Date.Today().AddDays(1)) Then
                        ddlDueDateTime.SelectedValue = 2
                    End If
                    txtDueDate.Text = oworkManagerResponse.DueDate.Date
                    txtTime.Text = FormatDateTime(oworkManagerResponse.DueDate, DateFormat.LongTime)
                    ViewState("WMTaskTimestamp") = oworkManagerResponse.TimeStamp
                    ddlTaskGroup.Value = oworkManagerResponse.TaskGroupCode.Trim
                    ddlUserGroup.SelectedValue = oworkManagerResponse.UserGroupCode.Trim
                    If ViewState("Task") IsNot Nothing Then
                        Dim bFound As Boolean = False
                        Dim oTask As NexusProvider.TaskGroupCollection = ViewState("Task")
                        For iCount As Integer = 0 To oTask.Count - 1
                            If oTask(iCount).Name.Trim.ToUpper = oworkManagerResponse.TaskCode.Trim.ToUpper Then
                                bFound = True
                                Exit For
                            End If
                        Next
                        If bFound = True Then
                            ddlTask.SelectedValue = oworkManagerResponse.TaskCode.Trim
                        End If
                    End If
                    FillUser()
                    If oworkManagerResponse.UserCode = "" Then
                        ddlUser.SelectedValue = "(Any Group Member)"
                    Else
                        ddlUser.SelectedValue = oworkManagerResponse.UserCode
                        ddlUserGroup.SelectedValue = oworkManagerResponse.UserGroupCode.Trim
                    End If

                    'Start link will not appear for teh task which can not be started
                    'Authorize Claim Payments
                    'NB and Referal Task
                    'Renewal
                    'Cash/Cheque Payment Authorisation
                    'MTA
                    'Open Claim
                    'Maintain Claim 
                    'Pay Claim
                    If (oworkManagerResponse.TaskGroupKey = 10 And oworkManagerResponse.TaskKey = 98) _
                    Or (oworkManagerResponse.TaskGroupKey = 8 And oworkManagerResponse.TaskKey = 58) _
                    Or (oworkManagerResponse.TaskGroupKey = 11 And oworkManagerResponse.TaskKey = 69) _
                    Or (oworkManagerResponse.TaskGroupKey = 1 And oworkManagerResponse.TaskKey = 206) _
                    Or (oworkManagerResponse.TaskGroupKey = 9 And oworkManagerResponse.TaskKey = 60) _
                    Or (oworkManagerResponse.TaskGroupKey = 9 And oworkManagerResponse.TaskKey = 80) _
                    Or (oworkManagerResponse.TaskGroupKey = 9 And oworkManagerResponse.TaskKey = 81) _
                    Or (oWorkManager.TaskGroupKey = 7 And oWorkManager.TaskKey = 688) _
                    Or (oworkManagerResponse.TaskGroupKey = 33 And oworkManagerResponse.TaskKey = 97) Then
                        'OrElse ddlUserGroup.SelectedValue = oworkManagerResponse.UserGroupCode.Trim Then
                        If Not oworkManagerResponse.TaskStatusKey = 3 Then 'If task is not complete
                            btnStart.Enabled = True
                        End If
                    Else
                        btnStart.Enabled = False
                    End If
                End If

            End If

            BindData()
        End Sub
        ''' <summary>
        ''' This event is fired on the selected index change of the ddlDueDateTime Dropdownlist which assigns the values to the textboxes accordingly.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub ddlDueDateTime_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Select Case ddlDueDateTime.SelectedIndex
                Case 1 'Today
                    txtDueDate.Text = DateTime.Now().ToShortDateString()
                    txtTime.Text = DateTime.Now().ToLongTimeString()
                Case 2 'Tomorrow
                    txtDueDate.Text = DateTime.Now().AddDays(1).ToShortDateString()
                    txtTime.Text = DateTime.Now().AddDays(1).ToLongTimeString()
                Case 3 'Week
                    txtDueDate.Text = DateTime.Now().AddDays(7).ToShortDateString()
                    txtTime.Text = DateTime.Now().AddDays(7).ToLongTimeString()
                Case 4 'month
                    txtDueDate.Text = DateTime.Now().AddMonths(1).ToShortDateString()
                    txtTime.Text = DateTime.Now().AddMonths(1).ToLongTimeString()
                Case 5 'Quarter
                    txtDueDate.Text = DateTime.Now().AddMonths(4).ToShortDateString()
                    txtTime.Text = DateTime.Now().AddMonths(4).ToLongTimeString()
                Case 6 'Year
                    txtDueDate.Text = DateTime.Now().AddYears(1).ToShortDateString()
                    txtTime.Text = DateTime.Now().AddYears(1).ToLongTimeString()
            End Select

        End Sub
        ''' <summary>
        ''' This event is fired when the User Group Dropdown list selected index is changed and Binds the ddlUser accordingly.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub ddlUserGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            FillUser()
        End Sub
        Protected Sub ddlUser_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        End Sub
        ''' <summary>
        ''' This is fired on Edit Button Click.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click

            'To set the Focus
            Page.SetFocus(ddlDueDateTime)

            'all the fields should be editable.
            txtClient.Enabled = True
            txtDescription.Enabled = True
            txtDueDate.Enabled = True
            txtTime.Enabled = True
            ddlDueDateTime.Enabled = True
            chkUrgent.Enabled = True
            btnEdit.Enabled = False
            ddlTask.Enabled = False
            ddlTaskGroup.Enabled = False
            ddlUser.Enabled = False
            ddlUserGroup.Enabled = False


        End Sub

        ''' <summary>
        ''' Binds the data to the Grid View
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub BindData()
            If Not Request.QueryString("TaskInstanceKey") Is Nothing Then
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oworkManagerCollection As New NexusProvider.WorkManagerCollection
                Dim oWorkManager As New NexusProvider.WorkManager
                Dim OtaskLogCollection As NexusProvider.TaskLogCollection

                oWorkManager.TaskInstanceKey = Request.QueryString("TaskInstanceKey")
                oWorkManager.IsDeleted = False
                OtaskLogCollection = oWebService.GetWmTaskLog(oWorkManager)
                Session(CNWMTaskLogCollection) = OtaskLogCollection
                If OtaskLogCollection.Count <> 0 Then
                    gvTaskLog.DataSource = OtaskLogCollection
                    gvTaskLog.DataBind()
                End If
            End If
        End Sub
        ''' <summary>
        ''' This event is fired on Delete Button click which Deletes the Task.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oworkManagerCollection As New NexusProvider.WorkManagerCollection
            Dim oWorkManagerTaskfromSession As NexusProvider.WorkManager = CType(Session.Item(CNWMWorkManagerCollection), NexusProvider.WorkManagerCollection).Item(CType(Request("TaskKey"), Integer))
            Dim oWorkManager As New NexusProvider.WorkManager
            Dim bTimeStamp() As Byte = CType(ViewState("WMTaskTimestamp"), Byte())

            oWorkManager.TaskInstanceKey = Request.QueryString("TaskInstanceKey") 'oWorkManagerTaskfromSession.TaskInstanceKey

            If bTimeStamp IsNot Nothing Then
                oWorkManager.TimeStamp = bTimeStamp
            End If
            oWorkManager.IsDeleted = False
            oWebService.DeleteWmTask(oWorkManager)

            If Request.QueryString("FromPage") IsNot Nothing Then
                If Request.QueryString("FromPage") = "WM" Then

                    Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
                End If
            Else
                Response.Redirect("~/secure/WorkManager.aspx", False)
            End If

        End Sub

        Protected Sub gvTaskLog_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvTaskLog.Load
            If gvTaskLog.PageCount = 1 Then
                gvTaskLog.AllowPaging = False
            End If
        End Sub
        ''' <summary>
        ''' This event is fired on GridView Row DataBound
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvTaskLog_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTaskLog.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("TaskInstanceKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.TaskLog).TaskInstanceKey)

                Dim hypView As HyperLink = e.Row.Cells(3).FindControl("hypView")
                hypView.CssClass = "thickbox"
                hypView.NavigateUrl = "~/Modal/WmTaskLogEntry.aspx?TaskInstanceKey=" & e.Row.RowIndex & "&Mode=ViewTaskLog&modal=true&FromPage=WMT&KeepThis=true&TB_iframe=true&height=500&width=750"

            End If
        End Sub
        ''' <summary>
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            If Request.QueryString("FromPage") = "WM" Then
                CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
            End If

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "DeleteConfirmation", _
                        "<script language=""JavaScript"" type=""text/javascript"">function DeleteConfirmation(){return confirm('" & GetLocalResourceObject("msg_DelTask").ToString() & "');}</script>")
        End Sub

        ''' <summary>
        ''' This event is fired when the TaskGroup Dropdown list selected index is changed and Binds the ddlUserGroup accordingly.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>

        Protected Sub ddlTaskGroup_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTaskGroup.SelectedIndexChange
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oUserGroup As NexusProvider.UserGroupCollection = New NexusProvider.UserGroupCollection
            oUserGroup = oWebService.GetUserGroupsbyTask(ddlTaskGroup.Value)
            ddlUserGroup.DataSource = oUserGroup
            ddlUserGroup.DataTextField = "Description"
            ddlUserGroup.DataValueField = "Code"
            ddlUserGroup.DataBind()
            FillUser()
            'Task Group based o  Task
            Dim oTempTask As NexusProvider.TaskGroupCollection = New NexusProvider.TaskGroupCollection
            Dim oTask As NexusProvider.TaskGroupCollection = New NexusProvider.TaskGroupCollection

            Dim sTaskKey As String = GetKeyForDescription(NexusProvider.ListType.PMLookup, ddlTaskGroup.Text, "PMwrk_Task_Group")
            If CInt(sTaskKey) > 0 Then
                oTempTask = oWebService.GetTaskGroupTasks(CInt(sTaskKey), Date.Today)
            End If
            For iCount As Integer = 0 To oTempTask.Count - 1
                If oTempTask(iCount).IsIncluded = True Then
                    oTask.Add(oTempTask(iCount))
                End If
            Next
            ViewState("Task") = oTask
            ddlTask.DataSource = oTask
            ddlTask.DataTextField = "Description"
            ddlTask.DataValueField = "Name"
            ddlTask.DataBind()
        End Sub

        Protected Sub btnStart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStart.Click
            Try
                'Unlock policy for same user
                Dim nInsuranceFolderKey As Integer
                If Request.QueryString("InsuranceFolderKey") IsNot Nothing Then
                    nInsuranceFolderKey = CInt(Request.QueryString("InsuranceFolderKey"))
                End If
                UnlockPolicy(nInsuranceFolderKey, Session(CNBranchCode).ToString)
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
                        oWebService.GetRisk(oQuote.Risks(jCount).Key, jCount, oQuote)
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
            ElseIf oWorkManager.TaskGroupKey = 7 And oWorkManager.TaskKey = 688 Then  'AuthorizeManualJournal
                Response.Redirect("~/secure/ManualJournal.aspx?ManualJournalKey=" & CType(Session(CNAuthoriseManualJournalTransactions), String) & "&Mode=WM", False)

            ElseIf oWorkManager.TaskGroupKey = 1 And oWorkManager.TaskKey = 206 Then 'Cash/Cheque Payment Authorisation
                Dim iCashListItemKey As Integer
                For iCount = 0 To oWorkManager.KeyData.Count - 1
                    If oWorkManager.KeyData(iCount).KeyName IsNot Nothing Then
                        If oWorkManager.KeyData(iCount).KeyName.Trim.ToUpper = "CASHLISTITEM_ID" Then
                            iCashListItemKey = oWorkManager.KeyData(iCount).KeyValue
                        End If
                    End If
                Next
                Session.Remove(CNSearchPaymentAuthorization)
                Response.Redirect("~/secure/AuthorizePayments.aspx?Type=Task&CashListItemKey=" & iCashListItemKey & "", False)
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
                            'In case WMTask is created while doing open claim
                        ElseIf oWorkManager.KeyData(iCount).KeyName.Trim.ToUpper() = "POLICYNUMBER" Then
                            iPolicyNumber = oWorkManager.KeyData(iCount).KeyValue
                            Response.Redirect("~/Claims/FindInsuranceFile.aspx?Policyno=" & iPolicyNumber, False)
                            'In case new WMTask is created for open claim from the work manager page
                        ElseIf oWorkManager.TaskKey = 60 Then
                            Response.Redirect("~/Claims/FindInsuranceFile.aspx?", False)
                            'In case new WMTask is created for  maintain claim or pay claim from the work manager page
                        ElseIf oWorkManager.TaskKey = 80 Or oWorkManager.TaskKey = 81 Then
                            Response.Redirect("~/Claims/FindClaim.aspx?Claimno=", False)
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
            oWebService.GetHeaderAndRisksByKey(oQuote)
            Session(CNQuote) = oQuote
            Session(CNCurrenyCode) = oQuote.CurrencyCode
            GetDataSetDefinition()
            Session.Remove(CNOI)
            Session(CNInsuranceFileKey) = oQuote.InsuranceFileKey
            Session(CNQuoteInSync) = False
            Session.Item(CNMode) = Mode.Buy
            Session(CNQuoteMode) = QuoteMode.FullQuote
            Response.Redirect("~/secure/premiumdisplay.aspx", False)
        End Sub

        ''' <summary>
        ''' This is fired on Assign Button Click.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnAssign_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAssign.Click
            'To set the Focus
            Page.SetFocus(ddlDueDateTime)
            ddlUserGroup.Enabled = True
            ddlUser.Enabled = True
            ddlTask.Enabled = False
            ddlTaskGroup.Enabled = False
            btnEdit.Enabled = False
            btnAssign.Enabled = False
            btnStart.Enabled = False
            btnComplete.Enabled = False
        End Sub

        ''' <summary>
        ''' This is fired on Complete Button Click.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnComplete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnComplete.Click
            'To set the Focus
            Page.SetFocus(ddlDueDateTime)
            chkComplete.Checked = True
            ddlTask.Enabled = False
            ddlTaskGroup.Enabled = False
            ddlUser.Enabled = False
            ddlUserGroup.Enabled = False
            btnEdit.Enabled = False
            btnComplete.Enabled = False
            btnAssign.Enabled = False
            btnStart.Enabled = False
        End Sub

        ''' <summary>
        ''' This is fired on InComplete Button Click.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnInComplete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInComplete.Click
            'To set the Focus
            Page.SetFocus(ddlDueDateTime)
            chkComplete.Checked = False
            ddlTask.Enabled = False
            ddlTaskGroup.Enabled = False
            ddlUser.Enabled = False
            ddlUserGroup.Enabled = False
            btnComplete.Enabled = False
            btnEdit.Enabled = False
            btnAssign.Enabled = False
            btnStart.Enabled = False
            btnDelete.Enabled = False
        End Sub
       
    End Class
End Namespace
