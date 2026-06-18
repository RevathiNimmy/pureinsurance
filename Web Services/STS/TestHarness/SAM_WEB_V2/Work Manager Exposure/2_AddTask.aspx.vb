Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Imports System.Data
Imports System.Data.SqlClient
Partial Class Work_Manager_Exposure_2_AddTask
    Inherits System.Web.UI.Page
    Dim str As String
    Dim dtGroups As New DataTable
    Dim dtTaskGroups As New DataTable
    Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
    Dim oSAM As New SAMForInsuranceV2
           
    Protected Sub mnuAddTask_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles mnuAddTask.MenuItemClick
        mvAddTask.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
            Dim oSAM As New SAMForInsuranceV2
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")
            PopulateControl.PopulateTaskGroups(ddlTaskGroup, dtGroups, "Code")
            Session("TaskGroup") = dtGroups
            ddlTaskGroup.SelectedIndex = 1
            
            PopulateControl.populateTaskgroupTask(ddlTask, ddlTaskGroup, dtGroups, dtTaskGroups, 1, "TaskKey")
            PopulateControl.populateallusersWM(ddlUser)



            'PopulateControl.PopulateUsers(ddlUser, ddlUserGroup.SelectedValue)

            'BuildLists(oSAM, ddlTaskGroup, STSListType.PMLookup, "PMwrk_Task_Group")
            'BuildLists(oSAM, ddlDueDateTime, STSListType.PMLookup, "Analysis_code")
            'BuildLists(oSAM, ddlTask, STSListType.PMLookup, "PMwrk_Task")
            'BuildLists(oSAM, ddlUserGroup, STSListType.PMLookup, "PMUser_Group")
            'BuildLists(oSAM, ddlUser, STSListType.PMLookup, "PMUser")



            txtDate.Text = DateTime.Now()
            'PopulateGroups()
            'PopulateUser()

        End If

    End Sub
    Public Function getKey(ByVal code As String) As Integer
        Dim dr() As DataRow = dtGroups.Select("CODE ='" & code & "'")
        Dim strKey As Integer = dr(0).Item("TaskGroupKey")
        Return strKey
    End Function

    Protected Sub btnTaskLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTaskLog.Click
        Response.Redirect("2_TaskLog.aspx")
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

                    If objControl.ID = "ddlUserGroup" Or objControl.ID = "ddlTask" Or objControl.ID = "ddlTaskGroup" Then
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
    
    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oRequest As New CreateWmTaskRequestType
        Dim oResponse As New CreateWmTaskResponseType
        If ddlUser.SelectedItem.Text = "(All Group Users)" Then
            oRequest.AllocationUser = ""
        Else
            oRequest.AllocationUser = ddlUser.SelectedItem.Text
        End If
        'ddlUser.Items(ddlUser.SelectedIndex).Value
        oRequest.AllocationUserGroup = ddlUserGroup.SelectedValue     'ddlUserGroup.Items(ddlUserGroup.SelectedIndex).Value
        oRequest.BranchCode = "HeadOff"
        oRequest.Client = txtClient.Text
        oRequest.Description = txtDescription.Text
        'txtDate.Text = ddlDueDateTime.Items(ddlDueDateTime.SelectedIndex).Value
        oRequest.DueDateTime = CDate(txtDate.Text)
        If (chkComplete.Checked) Then
            oRequest.IsComplete = True
        Else
            oRequest.IsComplete = False

        End If
        If (chkUrgent.Checked) Then
            oRequest.IsUrgent = True
        Else
            oRequest.IsUrgent = False

        End If

        If (chkIsTaskReview.Checked) Then
            oRequest.IsTaskReview = True
        Else
            oRequest.IsTaskReview = False

        End If
        oRequest.IsTaskReview = False
        oRequest.Task = ddlTask.SelectedValue 'ddlTask.Items(ddlTask.SelectedIndex).Value
        oRequest.TaskGroup = ddlTaskGroup.SelectedValue  'ddlTaskGroup.Items(ddlTaskGroup.SelectedIndex).Value
        oResponse = oSAM.CreateWmTask(oRequest)

        Response.Redirect("gettask.aspx")

    End Sub

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

        'DateCheck()
    End Sub
    'Private Sub DateCheck()
    '    Dim dat As Date = CDate(txtDate.Text).ToShortDateString
    '    Dim d As TimeSpan = dat.Subtract(Date.Now.ToShortDateString())
    '    If d.Days >= 0 Then
    '        CurrentDate.Value = "YES"
    '    Else
    '        CurrentDate.Value = "NO"
    '    End If
    'End Sub

    Protected Sub ddlUserGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUserGroup.SelectedIndexChanged
        ' PopulateUser()
        PopulateControl.PopulateUsers(ddlUser, ddlUserGroup.SelectedValue)
    End Sub

    Public Sub PopulateGroups()
        'rk updates following as part of SAM SFI Interop conversions to get the same connection used there
        'Str = "Data Source=localhost;Initial Catalog=Sirius_UIIC;Persist Security Info=True;User ID=sa"
        str = "Data Source=NIIT-SSP-SQLSVR;Initial Catalog=Sirius115;Integrated Security=False; User ID=SIRIUS;Password=$1R1U5"
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
        str = "Data Source=NIIT-SSP-SQLSVR;Initial Catalog=Sirius115;Integrated Security=False; User ID=SIRIUS;Password=$1R1U5"
        Dim cn As New SqlConnection(str)
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



    'Public Sub PopulateTaskGroups(ByRef ddlTaskGroup As DropDownList)
    '    Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
    '    'set up the proxy object
    '    Dim oSAM As New SAMForInsuranceV2
    '    oSAM.SetClientCredential(UserToken)
    '    oSAM.SetPolicy("SamClientPolicy")
    '    Dim oRequest As New GetTaskGroupsRequestType
    '    Dim oResponse As New GetTaskGroupsResponseType
    '    oRequest.BranchCode = "HeadOff"
    '    oResponse = oSAM.GetTaskGroups(oRequest)


    '    BuildTable(dtGroups)
    '    For i As Integer = 0 To UBound(oResponse.TaskGroups)
    '        Dim tr As System.Data.DataRow
    '        'tr = dsGroups.Tables(0).NewRow()
    '        tr = dtGroups.NewRow()
    '        tr("IsDeleted") = oResponse.TaskGroups(i).IsDeleted
    '        tr("Description") = oResponse.TaskGroups(i).Description
    '        tr("TaskGroupKey") = oResponse.TaskGroups(i).TaskGroupKey
    '        tr("Code") = oResponse.TaskGroups(i).Code
    '        dtGroups.Rows.Add(tr)
    '    Next

    '    Dim strFilter As String = "IsDeleted =false"
    '    dtGroups.DefaultView.RowFilter = strFilter
    '    ddlTaskGroup.DataSource = dtGroups
    '    ddlTaskGroup.DataTextField = "Description"
    '    ddlTaskGroup.DataValueField = "Code"
    '    ddlTaskGroup.DataBind()
    '    Session("TaskGroup") = dtGroups
    'End Sub
    'Public Sub BuildTable(ByRef dt As DataTable)

    '    Dim tc1 As DataColumn = New DataColumn()
    '    tc1.DataType = System.Type.GetType("System.String")
    '    tc1.ColumnName = "Description"



    '    Dim tc2 As DataColumn = New DataColumn()
    '    tc2.DataType = System.Type.GetType("System.Boolean")
    '    tc2.ColumnName = "IsDeleted"

    '    Dim tc3 As DataColumn = New DataColumn()
    '    tc3.DataType = System.Type.GetType("System.Int32")
    '    tc3.ColumnName = "TaskGroupKey"

    '    Dim tc4 As DataColumn = New DataColumn()
    '    tc4.DataType = System.Type.GetType("System.String")
    '    tc4.ColumnName = "Code"

    '    dt.Columns.Add(tc1)
    '    dt.Columns.Add(tc2)
    '    dt.Columns.Add(tc3)
    '    dt.Columns.Add(tc4)



    'End Sub
    'Public Sub populateTaskgroupTask(ByRef ddlTask As DropDownList)

    '    dtGroups = Session("TaskGroup")
    '    Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
    '    Dim dt As New DataTable


    '    'set up the proxy object
    '    Dim oSAM As New SAMForInsuranceV2


    '    oSAM.SetClientCredential(UserToken)
    '    oSAM.SetPolicy("SamClientPolicy")
    '    Dim oRequest As New GetTaskGroupTasksRequestType
    '    Dim oResponse As New GetTaskGroupTasksResponseType
    '    oRequest.BranchCode = "HeadOff"
    '    oRequest.EffectiveDate = DateTime.Now
    '    oRequest.TaskGroupKey = Convert.ToInt32(dtGroups.Rows(ddlTaskGroup.SelectedIndex)("TaskGroupKey")) 'ddlTaskGroup.SelectedValue
    '    oResponse = oSAM.GetTaskGroupTasks(oRequest)
    '    If Not oResponse Is Nothing Then

    '    End If
    '    'ddlDisplay.SelectedIndex = oResponse.TaskGroupTasks(0).TaskCategoryKey
    '    '  buidling table

    '    Dim tc1 As DataColumn = New DataColumn()
    '    tc1.DataType = System.Type.GetType("System.String")
    '    tc1.ColumnName = "Name"

    '    Dim tc2 As DataColumn = New DataColumn()
    '    tc2.DataType = System.Type.GetType("System.String")
    '    tc2.ColumnName = "Description"

    '    Dim tc3 As DataColumn = New DataColumn()
    '    tc3.DataType = System.Type.GetType("System.DateTime")
    '    tc3.ColumnName = "EffectiveDate"

    '    Dim tc4 As DataColumn = New DataColumn()
    '    tc4.DataType = System.Type.GetType("System.Boolean")
    '    tc4.ColumnName = "IsAvailable"

    '    Dim tc5 As DataColumn = New DataColumn()
    '    tc5.DataType = System.Type.GetType("System.Int32")
    '    tc5.ColumnName = "DisplayIcon"

    '    Dim tc6 As DataColumn = New DataColumn()
    '    tc6.DataType = System.Type.GetType("System.Int32")
    '    tc6.ColumnName = "TaskCategoryKey"

    '    Dim tc7 As DataColumn = New DataColumn()
    '    tc7.DataType = System.Type.GetType("System.Boolean")
    '    tc7.ColumnName = "IsDeleted"

    '    Dim tc8 As DataColumn = New DataColumn()
    '    tc8.DataType = System.Type.GetType("System.Boolean")
    '    tc8.ColumnName = "IsIncluded"

    '    Dim tc9 As DataColumn = New DataColumn()
    '    tc9.DataType = System.Type.GetType("System.Int32")
    '    tc9.ColumnName = "TaskKey"
    '    Dim tc10 As DataColumn = New DataColumn()
    '    tc10.DataType = System.Type.GetType("System.Boolean")
    '    tc10.ColumnName = "IsViewOnly"


    '    dt.Columns.Add(tc1)
    '    dt.Columns.Add(tc2)
    '    dt.Columns.Add(tc3)
    '    dt.Columns.Add(tc4)
    '    dt.Columns.Add(tc5)
    '    dt.Columns.Add(tc6)
    '    dt.Columns.Add(tc7)
    '    dt.Columns.Add(tc8)
    '    dt.Columns.Add(tc9)
    '    dt.Columns.Add(tc10)

    '    For i As Integer = 0 To UBound(oResponse.TaskGroupTasks)
    '        Dim tr As System.Data.DataRow
    '        'tr = dsGroups.Tables(0).NewRow()
    '        tr = dt.NewRow()
    '        tr("Name") = oResponse.TaskGroupTasks(i).Name
    '        tr("Description") = oResponse.TaskGroupTasks(i).Description
    '        tr("IsAvailable") = oResponse.TaskGroupTasks(i).IsAvailable
    '        tr("IsDeleted") = oResponse.TaskGroupTasks(i).IsDeleted
    '        tr("IsIncluded") = oResponse.TaskGroupTasks(i).IsIncluded
    '        tr("EffectiveDate") = oResponse.TaskGroupTasks(i).EffectiveDate
    '        tr("IsViewOnly") = oResponse.TaskGroupTasks(i).IsViewOnly
    '        tr("TaskKey") = oResponse.TaskGroupTasks(i).TaskKey
    '        tr("TaskCategoryKey") = oResponse.TaskGroupTasks(i).TaskCategoryKey
    '        dt.Rows.Add(tr)
    '    Next

    '    Dim strFilter As String = "IsIncluded = True"
    '    dt.DefaultView.RowFilter = strFilter
    '    ddlTask.DataSource = dt
    '    ddlTask.DataTextField = "Description"
    '    ddlTask.DataValueField = "Name"
    '    ddlTask.DataBind()
    'End Sub

    Protected Sub ddlTaskGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTaskGroup.SelectedIndexChanged
        dtGroups = Session("TaskGroup")
        PopulateControl.populateTaskgroupTask(ddlTask, ddlTaskGroup, dtGroups, dtTaskGroups, 0, "Name")
        'ddlTaskGroup.SelectedIndex
        PopulateUsergroupforTaskGroup()
    End Sub

    Public Sub PopulateUsergroupforTaskGroup()


        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")


        Dim orequest As New GetUserGroupsbyTaskRequestType
        Dim oreponse As New GetUserGroupsbyTaskResponseType
        orequest.BranchCode = "HeadOff"
        orequest.TaskGroupCode = ddlTaskGroup.SelectedValue
        ddlUserGroup.Items.Clear()

        oreponse = oSAM.GetUserGroupsbyTask(orequest)
        If oreponse.UserGroups IsNot Nothing AndAlso oreponse.UserGroups.Length > 0 Then
            For i As Integer = 0 To oreponse.UserGroups.Length - 1
                Dim li As New ListItem
                li.Text = oreponse.UserGroups(i).UserGroupDescription
                li.Value = oreponse.UserGroups(i).UserGroupCode
                ddlUserGroup.Items.Add(li)
                ddlUserGroup.DataBind()
            Next


        End If
       
       
        ''Dim iKey = getKey(ddlTaskGroup.SelectedValue)
        ''str = "Data Source=localhost;Initial Catalog=Sirius_UIIC;Persist Security Info=True;User ID=sa"
        ''Dim cn As New SqlConnection(str)
        ''Dim cntI As Integer
        ''Dim objDataReader As SqlDataReader
        ''Dim objCmdAllUsers As SqlCommand

        ''If (cn.State = ConnectionState.Closed) Then
        ''    cn.Open()
        ''End If

        ' ''If (ddlTaskGroup.SelectedValue > 0) Then
        ''cntI = 0
        ''objCmdAllUsers = New SqlCommand("spu_pmuser_groups_task_sel", cn)
        ''objCmdAllUsers.CommandType = CommandType.StoredProcedure
        ''objCmdAllUsers.Parameters.Add("effective_date", SqlDbType.DateTime)
        ''objCmdAllUsers.Parameters("effective_date").Value = System.DateTime.Now
        ''objCmdAllUsers.Parameters.Add("pmwrk_task_group_id", SqlDbType.Int)
        ''objCmdAllUsers.Parameters("pmwrk_task_group_id").Value = iKey 'Convert.ToInt32(ddlTaskGroup.SelectedValue)
        ''objCmdAllUsers.Parameters.Add("language_id", SqlDbType.Int)
        ''objCmdAllUsers.Parameters("language_id").Value = 1 ' Hard-Coded as if now

        ''objDataReader = objCmdAllUsers.ExecuteReader()
        ''ddlUserGroup.Items.Clear()
        ''While (objDataReader.Read())
        ''    ddlUserGroup.Items.Insert(++cntI, New ListItem(objDataReader("code"), objDataReader("pmuser_group_id")))
        ''End While
        ''ddlUserGroup.Items.Insert(0, New ListItem("(All Group Users)", "0"))

        ''objDataReader.Close()

        'Else
        '    cntI = 0
        '    objCmdAllUsers = New SqlCommand("spu_pmuser_groups_all_sel", cn)
        '    objCmdAllUsers.CommandType = CommandType.StoredProcedure
        '    objCmdAllUsers.Parameters.Add("effective_date", SqlDbType.DateTime)
        '    objCmdAllUsers.Parameters("effective_date").Value = System.DateTime.Now
        '    objCmdAllUsers.Parameters.Add("language_id", SqlDbType.Int)
        '    objCmdAllUsers.Parameters("language_id").Value = 1 ' Har

        '    objDataReader = objCmdAllUsers.ExecuteReader()
        '    ddlUser.Items.Clear()
        '    While (objDataReader.Read())
        '        ddlUser.Items.Insert(++cntI, New ListItem(objDataReader("caption"), objDataReader("code")))
        '    End While
        '    ddlUser.Items.Insert(0, New ListItem("(All Users)", "0"))

        '    objDataReader.Close()
        'End If
    End Sub
End Class
