Imports Microsoft.Web.Services3.Security.Tokens
Imports System.Data
Imports System.Data.SqlClient
Imports SAMForInsuranceV2
Partial Class Work_Manager_Exposure_2_EditTask
    Inherits System.Web.UI.Page
    Dim timestamp As Byte()
    Dim Str As String
    Dim dtGroups As New DataTable
    Dim dtTaskGroups As New DataTable
    Protected Sub mnuAddTask_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles mnuAddTask.MenuItemClick
        mvAddTask.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")


        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        '' Need to replace with AddWMTaskRequestResponseType pattern
        Dim oRequest As New GetWmTaskRequestType
        Dim oResponse As New GetWmTaskResponseType
        '''Binding Task group to ddlTaskGroup
        'DataBind to Dropdowns- Jigar
        Dim TaskInstanceKey As Integer
        TaskInstanceKey = Session("TaskInstanceKey")

        If Not Page.IsPostBack Then
            PopulateGroups()
            PopulateUser()
            'PopulateControl.PopulateUserGroups(ddlUserGroup, dtGroups)
            'PopulateControl.populateallusersWM(ddlUser)
            With oRequest
                .BranchCode = "HeadOff"
                .TaskInstanceKey = TaskInstanceKey
            End With

            Try
                PopulateControl.PopulateTaskGroups(ddlTaskGroup, dtGroups, "TaskGroupKey")
                'BuildLists(oSAM, ddlTaskGroup, STSListType.PMLookup, "PMwrk_Task_Group")
                'BuildLists(oSAM, ddlDueDateTime, STSListType.PMLookup, "Analysis_code")
                'BuildLists(oSAM, ddlTask, STSListType.PMLookup, "PMwrk_Task")
                'BuildLists(oSAM, ddlUserGroup, STSListType.PMLookup, "PMUser_Group")
                'BuildLists(oSAM, ddlUser, STSListType.PMLookup, "PMUser")

                oResponse = oSAM.GetWmTask(oRequest)
                PopulateControl.populateTaskgroupTask(ddlTask, ddlTaskGroup, dtGroups, dtTaskGroups, oResponse.TaskGroupKey, "TaskKey")
                Session("usergroupcode") = oResponse.UserGroupCode
                With oResponse
                    txtClient.Text = .Customer
                    txtDescription.Text = .Description

                    Dim dr() As Data.DataRow = dtGroups.Select("TaskGroupKey =" & .TaskGroupKey)
                    'ddlTaskGroup.SelectedValue = .TaskGroupKey
                    ddlTaskGroup.Items.Clear()
                    ddlTaskGroup.Items.Add(dr(0)("Description"))


                    Dim drTask() As Data.DataRow = dtTaskGroups.Select("TaskKey =" & .TaskKey)
                    ddlTask.Items.Clear()
                    If drTask(0)(7) = True Then
                        ddlTask.Items.Add(drTask(0)("Description"))
                    End If
                    'ddlTask.SelectedValue = .TaskKey

                    ddlDueDateTime.SelectedValue = .DueDate
                    If .UserKey = 0 Then
                        ddlUser.SelectedValue = 0
                    Else
                        ddlUser.SelectedValue = .UserKey
                    End If
                    ddlUserGroup.SelectedValue = 1 '.UserGroupKey
                    lblLastModifiedBy.Text = ddlUser.SelectedItem.Text
                    lblLoggedBy.Text = "sirius" 'ddlUser.SelectedItem.Text
                    'txtDate.Text = .DateCreated
                    txtDate.Text = .DueDate
                    lblAtLoggedBy.Text = .DateCreated
                    lblAt.Text = .LastModified
                    'timestamp = oResponse.TimeStamp

                    Session("timestamp") = .TaskTimestamp
                    Session("task_status") = .TaskStatusKey
                    If oResponse.TaskStatusKey = 3 Then
                        chkComplete.Checked = True
                    Else
                        chkComplete.Checked = False

                    End If
                    If oResponse.IsUrgent = 1 Then

                        ChkUrgent.Checked = True
                    Else

                        ChkUrgent.Checked = False
                    End If
                    '' For Viewing purpose
                    pnlTaskDetails.Enabled = True
                    pnlAllocation.Enabled = True
                End With

            Catch os As SamResponseException
                'should do some error handling here. Just output error for now
                Response.Write("An error occured calling SAM:<br>" & os.Message)

            Catch oe As Exception
                'should do some error handling here. Just output error for now
                Response.Write("An error occured:<br>" & oe.Message)

            Finally
                'clean up any objects here
            End Try
            
            DateCheck()

        End If
        
    End Sub

    Protected Sub btnTaskLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTaskLog.Click

    End Sub

    Private Sub DateCheck()
        Dim dat As Date = CDate(txtDate.Text).ToShortDateString
        Dim d As TimeSpan = dat.Subtract(Date.Now.ToShortDateString())
        If d.Days >= 0 Then
            CurrentDate.Value = "YES"
        Else
            CurrentDate.Value = "NO"
        End If
    End Sub
    Private Sub BuildLists(ByVal oSAM As SAMForInsuranceV2, ByRef objControl As DropDownList, ByVal ESTSLookup As STSListType, ByVal ListCode As String)
        Dim oRequest As New GetListRequestType
        Dim oResponse As New GetListResponseType


        oRequest.BranchCode = "HeadOff"
        oRequest.ListType = STSListType.PMLookup
        oRequest.ListCode = ListCode

        Try
            oResponse = oSAM.GetList(oRequest)

            With oResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                Else
                    If objControl.ID = "ddlUserGroup" Then
                        objControl.DataSource = oResponse.List
                        objControl.DataTextField = "Description"
                        objControl.DataValueField = "Code"
                        objControl.DataBind()
                        objControl.Items.Insert(0, New ListItem("", ""))
                    Else
                        objControl.DataSource = oResponse.List
                        objControl.DataTextField = "Description"
                        objControl.DataValueField = "Key"
                        objControl.DataBind()
                        objControl.Items.Insert(0, New ListItem("", ""))
                    End If
                    
                End If
                If ListCode = "source" Then
                    Session("Branch") = oResponse.List
                End If
            End With

        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            Response.Write("An error occured calling SAM:<br>" & os.Message)

        Catch oe As Exception
            'should do some error handling here. Just output error for now
            Response.Write("An error occured:<br>" & oe.Message)

        Finally
            'clean up any objects here
        End Try

    End Sub
  
    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        
        If CurrentDate.Value = "YES" Then
            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
            Dim oSAM As New SAMForInsuranceV2
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")

            Dim oUpdateWmTaskRequest As New UpdateWmTaskRequestType
            Dim oUpdateWmTaskResponse As New UpdateWmTaskResponseType
            'Dim a() As Byte
            With oUpdateWmTaskRequest
                .BranchCode = "HeadOff"
                .TaskInstanceKey = Session("TaskInstanceKey")
                'txtDate.Text = DateTime.Now.ToString()
                .DueDate = txtDate.Text
                .Client = txtClient.Text
                .Description = txtDescription.Text

                .Urgent = Convert.ToInt32(ChkUrgent.Checked)

                .TaskStatusKey = Session("task_status") 'ddlTask.Text 'txtTaskStatusKey.Text

                .UserGroupCode = Session("usergroupcode") 'ddlUserGroup.SelectedItem.Text 'ddlUserGroup.Items(ddlUserGroup.SelectedIndex).Value 'txtUserGroupCode.Text
                If ddlUser.SelectedItem.Text = "(All Users)" Then
                    .UserCode = ""
                Else
                    .UserCode = ddlUser.SelectedItem.Text 'txtUserCode.Text
                End If

                .TaskTimeStamp = Session("timestamp")
            End With
            oUpdateWmTaskResponse = oSAM.UpdateWmTask(oUpdateWmTaskRequest)

            With oUpdateWmTaskResponse
                If Not (.Errors) Is Nothing Then
                    Throw New SamResponseException(.Errors)
                    'Else
                    '    getTaskValues()
                End If

            End With
            Session("Arr1") = Nothing
            Session("Arr") = Nothing
            Response.Redirect("gettask.aspx")
        End If

    End Sub
    'Private Sub getTaskValues()
    '    Dim con As New SqlConnection("Data Source=INFCH01354; Initial Catalog=sirius_uiic; user id=sa;pwd=sa")
    '    'Dim con As New SqlConnection("Persist Security Info=False;Integrated Security=SSPI;database=sirius_uiic;server=mySQLServer")
    '    Dim da As New SqlDataAdapter()
    '    Dim myCommand As New SqlCommand()
    '    Dim ds As New DataSet
    '    myCommand.CommandText = "SELECT * FROM PMWrk_Task_Instance where pmwrk_task_instance_cnt=" + txtTaskInstanceKey.Text.ToString
    '    myCommand.CommandTimeout = 15
    '    myCommand.CommandType = CommandType.Text
    '    myCommand.Connection = con
    '    da.SelectCommand = myCommand
    '    da.Fill(ds)
    '    If (ds.Tables(0).Rows.Count > 0) Then
    '        With ds.Tables(0)
    '            'txtTaskGroupCode.Text = .Rows(0)("pmwrk_task_group_id").ToString()
    '            'txtTaskCode.Text = .Rows(0)("pmwrk_task_id").ToString
    '            txtDueDate.Text = .Rows(0)("task_due_date").ToString
    '            txtClient.Text = .Rows(0)("Customer").ToString
    '            txtDescription.Text = .Rows(0)("Description").ToString
    '            txtUrgent.Text = .Rows(0)("is_urgent").ToString
    '            txtTaskStatusKey.Text = .Rows(0)("task_status").ToString
    '            txtUserGroupCode.Text = .Rows(0)("pmuser_group_id").ToString
    '            txtUserCode.Text = .Rows(0)("user_id").ToString
    '        End With
    '    End If
    'End Sub

    Protected Sub ddlDueDateTime_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDueDateTime.SelectedIndexChanged

        If ddlDueDateTime.SelectedIndex = 1 Then txtDate.Text = DateTime.Now().ToString()
        Select Case ddlDueDateTime.SelectedIndex
            Case 1 'Today
                txtDate.Text = DateTime.Now().ToString()
            Case 2 'Tomorrow
                txtDate.Text = DateTime.Now().AddDays(1)
            Case 3 'Week
                txtDate.Text = DateTime.Now.AddDays(7)
            Case 4 'month
                txtDate.Text = DateTime.Now.AddMonths(1)
            Case 5 'Quarter
                txtDate.Text = DateTime.Now.AddMonths(4)
            Case 6 'Year
                txtDate.Text = DateTime.Now.AddYears(1)
        End Select

        DateCheck()

    End Sub

#Region "Developed new-- Combo Box filling -- Jigar"
    Public Sub PopulateGroups()
        'rk updates following as part of SAM SFI Interop conversions to get the same connection used there
        'Str = "Data Source=localhost;Initial Catalog=Sirius_UIIC;Persist Security Info=True;User ID=sa"
        Str = "Data Source=NIIT-SSP-SQLSVR;Initial Catalog=Sirius115;Integrated Security=False; User ID=SIRIUS;Password=$1R1U5"
        'Dim cn As New SqlConnection(SqlDataSource2.ConnectionString)
        Dim cn As New SqlConnection(Str)
        Dim cntI As Integer
        Dim objDataReader As SqlDataReader
        Dim objCmdAllGroups As SqlCommand

        If (cn.State = ConnectionState.Closed) Then
            cn.Open()
        End If

        cntI = 0
        objCmdAllGroups = New SqlCommand("spu_pmuser_users_groups_sel", cn)
        objCmdAllGroups.CommandType = CommandType.StoredProcedure
        objCmdAllGroups.Parameters.Add("effective_date", SqlDbType.DateTime)
        objCmdAllGroups.Parameters("effective_date").Value = System.DateTime.Now
        objCmdAllGroups.Parameters.Add("user_id", SqlDbType.Int)
        objCmdAllGroups.Parameters("user_id").Value = 1 ' Hard-Coded as if now
        objCmdAllGroups.Parameters.Add("language_id", SqlDbType.Int)
        objCmdAllGroups.Parameters("language_id").Value = 1 ' Hard-Coded as if now

        objDataReader = objCmdAllGroups.ExecuteReader()
        ddlUserGroup.Items.Clear()
        While (objDataReader.Read())
            ddlUserGroup.Items.Insert(++cntI, New ListItem(objDataReader("code"), objDataReader("pmuser_group_id")))
        End While
        ddlUserGroup.Items.Insert(0, New ListItem("(All Group)", "0"))
        objDataReader.Close()

    End Sub

    Public Sub PopulateUser()
        'Dim cn As New SqlConnection(SqlDataSource2.ConnectionString)
        'rk updates following as part of SAM SFI Interop conversions to get the same connection used there
        'Str = "Data Source=localhost;Initial Catalog=Sirius_UIIC;Persist Security Info=True;User ID=sa"
        Str = "Data Source=NIIT-SSP-SQLSVR;Initial Catalog=Sirius115;Integrated Security=False; User ID=SIRIUS;Password=$1R1U5"
        Dim cn As New SqlConnection(Str)
        Dim cntI As Integer
        Dim objDataReader As SqlDataReader
        Dim objCmdAllUsers As SqlCommand

        If (cn.State = ConnectionState.Closed) Then
            cn.Open()
        End If

        If (ddlUserGroup.SelectedValue > 0) Then
            cntI = 0
            objCmdAllUsers = New SqlCommand("spu_pmuser_group_users_sel", cn)
            objCmdAllUsers.CommandType = CommandType.StoredProcedure
            objCmdAllUsers.Parameters.Add("effective_date", SqlDbType.DateTime)
            objCmdAllUsers.Parameters("effective_date").Value = System.DateTime.Now
            objCmdAllUsers.Parameters.Add("pmuser_group_id", SqlDbType.Int)
            objCmdAllUsers.Parameters("pmuser_group_id").Value = Convert.ToInt32(ddlUserGroup.SelectedValue)

            objDataReader = objCmdAllUsers.ExecuteReader()
            ddlUser.Items.Clear()
            While (objDataReader.Read())
                ddlUser.Items.Insert(++cntI, New ListItem(objDataReader("username"), objDataReader("user_id")))
            End While
            ddlUser.Items.Insert(0, New ListItem("(All Group Users)", "0"))

            objDataReader.Close()
        Else
            cntI = 0
            objCmdAllUsers = New SqlCommand("spu_pmuser_all_users_sel", cn)
            objCmdAllUsers.CommandType = CommandType.StoredProcedure
            objCmdAllUsers.Parameters.Add("effective_date", SqlDbType.DateTime)
            objCmdAllUsers.Parameters("effective_date").Value = System.DateTime.Now

            objDataReader = objCmdAllUsers.ExecuteReader()
            ddlUser.Items.Clear()
            While (objDataReader.Read())
                ddlUser.Items.Insert(++cntI, New ListItem(objDataReader("username"), objDataReader("user_id")))
            End While
            ddlUser.Items.Insert(0, New ListItem("(All Users)", "0"))

            objDataReader.Close()
        End If
    End Sub
#End Region

End Class
