Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Imports System.Data
Imports System.Data.SqlClient
Partial Class gettask
    Inherits System.Web.UI.Page


    Dim dt As New System.Data.DataTable
    Dim dtGroups As New DataTable
    Public flgDelete As Boolean
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")


        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2


        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        btnAssignTask.Enabled = False

        ' BuildLists(oSAM, ddlALLUser, STSListType.PMLookup, "PMUser")

        Dim oRequest As New GetWorkManagerScheduledTasksRequestType
        Dim oResponse As New GetWorkManagerScheduledTasksResponseType

        oRequest.BranchCode = "HeadOff"


        If Not Page.IsPostBack Then
            ' BuildLists(oSAM, ddlGroups, STSListType.PMLookup, "PMUser_Group")

            PopulateControl.PopulateUserGroups(ddlGroups, dtGroups)
            'PopulateUser()
            ddlALLUser.Items.Insert(0, New ListItem("(All Group Users)"))
            PopulateControl.populateallusersWM(ddlALLUser)
            'PopulateControl.PopulateUsers(ddlALLUser, ddlALLUser.SelectedValue)
            ddlStatus.SelectedIndex = TaskStatus.All
            ddlGroups.SelectedIndex = 0
            oRequest.Date = DateRange.Today
            oRequest.TaskStatusKey = TaskStatus.All
            oRequest.ShowSystemKEY = ShowType.User
            oRequest.UserCODE = "(All Group Users)" '"SIRIUS"
            oRequest.UserGroupCODE = "(All Group)" '"sysadmin"
            oRequest.DateSpecified = True
            oRequest.ShowSystemKEYSpecified = False
            oRequest.TaskStatusKeySpecified = True
            SelectedDate()

        Else

            If ddlStatus.SelectedIndex = TaskStatus.All Then
                oRequest.Date = ddlDates.SelectedIndex 'ddlDates.Items(ddlDates.SelectedIndex).Value
                'oRequest.TaskStatusKey = 0
                If ddlSystem.Items(ddlSystem.SelectedIndex).Value = "All" Then
                    oRequest.ShowSystemKEY = ShowType.All
                Else
                    oRequest.ShowSystemKEY = ddlSystem.SelectedIndex
                End If

                oRequest.TaskStatusKey = TaskStatus.All
                oRequest.UserCODE = ddlALLUser.SelectedItem.Text  '""
                oRequest.UserGroupCODE = ddlGroups.SelectedValue  'ddlGroups.Items(ddlGroups.SelectedIndex).Value
                oRequest.DateSpecified = True
                oRequest.ShowSystemKEYSpecified = True
                oRequest.TaskStatusKeySpecified = True
            Else
                oRequest.Date = ddlDates.SelectedIndex 'ddlDates.Items(ddlDates.SelectedIndex).Value

                If ddlSystem.Items(ddlSystem.SelectedIndex).Value = "All" Then
                    oRequest.ShowSystemKEY = ShowType.All
                Else
                    oRequest.ShowSystemKEY = ddlSystem.SelectedIndex 'ddlSystem.SelectedIndex - 1 '0
                End If

                oRequest.UserCODE = ddlALLUser.SelectedItem.Text   '""
                oRequest.UserGroupCODE = ddlGroups.SelectedValue  'ddlGroups.Items(ddlGroups.SelectedIndex).Value
                If ddlStatus.Items(ddlStatus.SelectedIndex).Value = "(Not Complete)" Then
                    oRequest.TaskStatusKey = TaskStatus.NotComplete
                Else
                    oRequest.TaskStatusKey = ddlStatus.SelectedIndex
                End If

                oRequest.DateSpecified = True
                oRequest.ShowSystemKEYSpecified = True
                oRequest.TaskStatusKeySpecified = True
            End If

        End If

        'oRequest.TaskStatusKeySpecified = True


        oResponse = oSAM.GetWorkManagerScheduledTasks(oRequest)


        'Dim dt As New System.Data.DataTable
        'gvTasks.DataSource = oResponse.Tasks
        If Not (oResponse.Tasks Is Nothing) Then

            Dim tc1 As DataColumn = New DataColumn()
            tc1.DataType = System.Type.GetType("System.String")
            tc1.ColumnName = "Customer"

            Dim tc2 As DataColumn = New DataColumn()
            tc2.DataType = System.Type.GetType("System.String")
            tc2.ColumnName = "Description"

            Dim tc3 As DataColumn = New DataColumn()
            tc3.DataType = System.Type.GetType("System.DateTime")
            tc3.ColumnName = "DueDate"

            Dim tc4 As DataColumn = New DataColumn()
            tc4.DataType = System.Type.GetType("System.String")
            tc4.ColumnName = "TaskStatusKey"

            Dim tc5 As DataColumn = New DataColumn()
            tc5.DataType = System.Type.GetType("System.String")
            tc5.ColumnName = "Type"

            Dim tc6 As DataColumn = New DataColumn()
            tc6.DataType = System.Type.GetType("System.Int32")
            tc6.ColumnName = "Urgent"

            Dim tc7 As DataColumn = New DataColumn()
            tc7.DataType = System.Type.GetType("System.Int32")
            tc7.ColumnName = "UserGroupKey"

            Dim tc8 As DataColumn = New DataColumn()
            tc8.DataType = System.Type.GetType("System.Int32")
            tc8.ColumnName = "UserKey"

            Dim tc9 As DataColumn = New DataColumn()
            tc9.DataType = System.Type.GetType("System.Int32")
            tc9.ColumnName = "TaskInstanceKey"

            Dim tc10 As DataColumn = New DataColumn()
            tc10.DataType = System.Type.GetType("System.String")
            tc10.ColumnName = "UserGroupDescription"

            Dim tc11 As DataColumn = New DataColumn()
            tc11.DataType = System.Type.GetType("System.String")
            tc11.ColumnName = "UserGroupCode"

            Dim tc12 As DataColumn = New DataColumn()
            tc12.DataType = System.Type.GetType("System.String")
            tc12.ColumnName = "UserCode"


            dt.Columns.Add(tc1)
            dt.Columns.Add(tc2)
            dt.Columns.Add(tc3)
            dt.Columns.Add(tc4)
            dt.Columns.Add(tc5)
            dt.Columns.Add(tc6)
            dt.Columns.Add(tc7)
            dt.Columns.Add(tc8)
            dt.Columns.Add(tc9)
            dt.Columns.Add(tc10)
            dt.Columns.Add(tc11)
            dt.Columns.Add(tc12)

            For i As Integer = 0 To UBound(oResponse.Tasks)

                Dim tr As System.Data.DataRow
                'tr = dsGroups.Tables(0).NewRow()
                tr = dt.NewRow()
                tr(0) = oResponse.Tasks(i).Customer 'dsAllGroups.Tables(0).Rows(icntGroups)(0)
                tr(1) = oResponse.Tasks(i).Description
                tr(2) = oResponse.Tasks(i).DueDate
                tr(3) = oResponse.Tasks(i).TaskStatusKey.ToString()
                Select Case oResponse.Tasks(i).TaskStatusKey
                    Case 0
                        tr(3) = "New"
                    Case 1
                        tr(3) = "InProgress"
                    Case 2
                        tr(3) = "InComplete"
                    Case 3
                        tr(3) = "Complete"
                End Select

                'If (oResponse.Tasks(i).TaskStatusKey = 0) Then
                '    tr(3) = ""
                'End If

                tr(4) = oResponse.Tasks(i).Type
                tr(5) = oResponse.Tasks(i).Urgent
                tr(6) = oResponse.Tasks(i).UserGroupKey
                tr(7) = oResponse.Tasks(i).UserKey
                tr(8) = oResponse.Tasks(i).TaskInstanceKey
                tr(9) = oResponse.Tasks(i).UserGroupDescription
                tr(10) = oResponse.Tasks(i).UserGroupCode
                tr(11) = oResponse.Tasks(i).UserCode
                dt.Rows.Add(tr)
            Next
            gvTasks.DataSource = dt
            Label1.Text = dt.Rows.Count
        Else
            gvTasks.DataSource = Nothing

        End If
        Session("Table") = dt
        gvTasks.DataBind()


        Response.Write(gvTasks.Rows.Count.ToString())

    End Sub
    Dim Str As String
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
        ddlGroups.Items.Clear()
        While (objDataReader.Read())
            ddlGroups.Items.Insert(++cntI, New ListItem(objDataReader("code"), objDataReader("pmuser_group_id")))
        End While
        ddlGroups.Items.Insert(0, New ListItem("(All Group)", "0"))
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

        If (ddlGroups.SelectedValue > 0) Then
            cntI = 0
            objCmdAllUsers = New SqlCommand("spu_pmuser_group_users_sel", cn)
            objCmdAllUsers.CommandType = CommandType.StoredProcedure
            objCmdAllUsers.Parameters.Add("effective_date", SqlDbType.DateTime)
            objCmdAllUsers.Parameters("effective_date").Value = System.DateTime.Now
            objCmdAllUsers.Parameters.Add("pmuser_group_id", SqlDbType.Int)
            objCmdAllUsers.Parameters("pmuser_group_id").Value = Convert.ToInt32(ddlGroups.SelectedValue)

            objDataReader = objCmdAllUsers.ExecuteReader()
            ddlALLUser.Items.Clear()
            While (objDataReader.Read())
                ddlALLUser.Items.Insert(++cntI, New ListItem(objDataReader("username"), objDataReader("user_id")))
            End While
            ddlALLUser.Items.Insert(0, New ListItem("(All Group Users)", "0"))

            objDataReader.Close()
        Else
            cntI = 0
            objCmdAllUsers = New SqlCommand("spu_pmuser_all_users_sel", cn)
            objCmdAllUsers.CommandType = CommandType.StoredProcedure
            objCmdAllUsers.Parameters.Add("effective_date", SqlDbType.DateTime)
            objCmdAllUsers.Parameters("effective_date").Value = System.DateTime.Now

            objDataReader = objCmdAllUsers.ExecuteReader()
            ddlALLUser.Items.Clear()
            While (objDataReader.Read())
                ddlALLUser.Items.Insert(++cntI, New ListItem(objDataReader("username"), objDataReader("user_id")))
            End While
            ddlALLUser.Items.Insert(0, New ListItem("(All Users)", "0"))

            objDataReader.Close()
        End If
    End Sub
    Public Sub PopulateUserGroups1()
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")


        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        Dim oRequest As New GetUserGroupsRequestType
        Dim oResponse As New GetUserGroupsResponseType
        oRequest.BranchCode = "HeadOff"
        oResponse = oSAM.GetUserGroups(oRequest)
        ddlGroups.DataSource = oResponse.UserGroups
        ddlGroups.DataTextField = "Code"
        ddlGroups.DataValueField = "UserGroupKey"

        'gvUserGroups.Columns(4).Visible = False
        'gvUserGroups.Columns(5).Visible = False
        'gvUserGroups.Columns(6).Visible = False
        ddlGroups.DataBind()
    End Sub
#End Region

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

                    objControl.DataSource = oResponse.List
                    objControl.DataTextField = "Description"
                    objControl.DataValueField = "CODE"
                    objControl.DataBind()
                    objControl.Items.Insert(0, New ListItem("", ""))
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
    Protected Sub gvTasks_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTasks.RowDataBound



    End Sub

    Protected Sub ddlStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlStatus.SelectedIndexChanged
        If ddlStatus.SelectedIndex = 3 Then
            btnEditTask.Enabled = False
        Else
            btnEditTask.Enabled = True
        End If


        'Dim dv As New System.Data.DataView
        'dv = dt.DefaultView
        'dv.RowFilter = " TaskStatusKey =" & ddlStatus.SelectedIndex
        'gvTasks.DataSource = dv.Table
    End Sub

    Protected Sub ddlStatus_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlStatus.Load
        ' Response.Write("Test")
    End Sub

    Protected Sub ddlDates_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDates.PreRender

    End Sub

    Protected Sub ddlDates_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDates.SelectedIndexChanged


    End Sub

    Protected Sub btnViewTask_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnViewTask.Click
        Response.Redirect("2_ViewTask.aspx")
    End Sub

    Protected Sub gvTasks_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvTasks.SelectedIndexChanged
        'hid1.Value = "../Work Manager Exposure/2_ViewTask.aspx?select = " & gvTasks.SelectedRow.Cells(9).Text
        Session("TaskInstanceKey") = gvTasks.SelectedRow.Cells(9).Text
        'If btnAssignTask.Enabled = True Then


        Dim arr As New ArrayList
        Dim arr1 As New ArrayList

        If gvTasks.SelectedRow.Cells(2).Text = "Complete" Then
            btnEditTask.Enabled = False
            btnDeleteTask.Enabled = True
            btnComplete.Enabled = False
        Else
            btnEditTask.Enabled = True
            btnDeleteTask.Enabled = False
            btnComplete.Enabled = True
        End If

        If gvTasks.SelectedRow.Cells(2).Text = "InComplete" Or gvTasks.SelectedRow.Cells(2).Text = "New" Then
            btnIncomplete.Enabled = False
        Else
            btnIncomplete.Enabled = True
        End If
       
        
        'Dim ht As New Hashtable
        'ht.Add(gvTasks.SelectedRow.DataItemIndex, gvTasks.SelectedRow.Cells(9).Text)

        If Not Session("Arr") Is Nothing Then
            arr = Session("Arr")
            arr1 = Session("Arr1")
            arr1.Add(gvTasks.SelectedRow.Cells(9).Text)
            arr.Add(gvTasks.SelectedRow.DataItemIndex)
        Else
            arr.Add(gvTasks.SelectedRow.DataItemIndex)
            arr1.Add(gvTasks.SelectedRow.Cells(9).Text)
            Session("Arr") = arr
            Session("Arr1") = arr1
        End If

        For i As Integer = 0 To arr.Count - 1
            gvTasks.Rows(arr(i)).BackColor = Drawing.Color.Cornsilk
        Next

        'End If
        If arr1.Count > 1 Then
            btnAssignTask.Enabled = True
            btnAssignSingletask.Enabled = False
        Else
            btnAssignSingletask.Enabled = True
        End If
        'gvTasks.Rows(gvTasks.SelectedRow.DataItemIndex - 1).BackColor = Drawing.Color.Blue
    End Sub

    Protected Sub btnAssignTask_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAssignTask.Click
        Response.Redirect("../Work Manager Exposure/ReAssignTask.aspx")
    End Sub

    Protected Sub btnEditTask_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditTask.Click
        Response.Redirect("2_EditTask.aspx")
    End Sub

    Protected Sub btnDeleteTask_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteTask.Click

        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        '' Need to replace with AddWMTaskRequestResponseType pattern
        Dim oRequest As New GetWmTaskRequestType
        Dim oResponse As New GetWmTaskResponseType
        Dim TaskInstanceKey As Integer
        TaskInstanceKey = Session("TaskInstanceKey")


        With oRequest
            .BranchCode = "HeadOff"
            .TaskInstanceKey = TaskInstanceKey
        End With
        oResponse = oSAM.GetWmTask(oRequest)
        Dim oDeleteWmTaskRequest As New DeleteWmTaskRequestType
        Dim oDeleteWmTaskResponse As New DeleteWmTaskResponseType
        oDeleteWmTaskRequest.BranchCode = "HeadOff"
        oDeleteWmTaskRequest.TaskInstanceKey = TaskInstanceKey
        oDeleteWmTaskRequest.TaskTimeStamp = oResponse.TaskTimestamp

        oDeleteWmTaskResponse = oSAM.DeleteWmTask(oDeleteWmTaskRequest)
        Populate()

    End Sub

    Public Sub SelectedDate()
        For i As Integer = 0 To ddlDates.Items.Count - 1


            Select Case ddlDates.Items(i).Text
                Case "(All Dates)"
                    ddlDates.Items(i).Value = Nothing 'Date.Today.ToString

                Case "Today"
                    ddlDates.Items(i).Value = Date.Today.ToString
                Case "Tomorrow"
                    ddlDates.Items(i).Value = Date.Today.AddDays(1).ToString
                Case "Next 2 Days"
                    ddlDates.Items(i).Value = Date.Today.AddDays(2).ToString
                Case "Next 3 Days"
                    ddlDates.Items(i).Value = Date.Today.AddDays(3).ToString
                Case "Next 4 Days"
                    ddlDates.Items(i).Value = Date.Today.AddDays(4).ToString
                Case "Next 5 Days"
                    ddlDates.Items(i).Value = Date.Today.AddDays(5).ToString
                Case "Next 6 Days"
                    ddlDates.Items(i).Value = Date.Today.AddDays(6).ToString
                Case "Next 7 Days"
                    ddlDates.Items(i).Value = Date.Today.AddDays(7).ToString
                Case "Next 14 Days"
                    ddlDates.Items(i).Value = Date.Today.AddDays(14).ToString
                Case "Next 28 Days"
                    ddlDates.Items(i).Value = Date.Today.AddDays(28).ToString
            End Select
        Next

    End Sub

    Public Sub Populate()
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")


        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2


        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oRequest As New GetWorkManagerScheduledTasksRequestType
        Dim oResponse As New GetWorkManagerScheduledTasksResponseType
        dt.Rows.Clear()

        oRequest.BranchCode = "HeadOff"
        oRequest.Date = ddlDates.SelectedIndex
        If ddlSystem.Items(ddlSystem.SelectedIndex).Value = "All" Then
            oRequest.ShowSystemKEY = ShowType.All
        Else
            oRequest.ShowSystemKEY = ddlSystem.SelectedIndex
        End If
        oRequest.UserCODE = ddlALLUser.SelectedItem.Text 
        oRequest.UserGroupCODE = ddlGroups.SelectedValue  'ddlGroups.Items(ddlGroups.SelectedIndex).Value
        oRequest.TaskStatusKey = ddlStatus.SelectedIndex
        oRequest.DateSpecified = True
        oRequest.ShowSystemKEYSpecified = True
        oRequest.TaskStatusKeySpecified = True


        oRequest.Date = ddlDates.SelectedIndex

        oResponse = oSAM.GetWorkManagerScheduledTasks(oRequest)
        If Not oResponse.Tasks Is Nothing Then


            For i As Integer = 0 To UBound(oResponse.Tasks)

                Dim tr As System.Data.DataRow
                'tr = dsGroups.Tables(0).NewRow()
                tr = dt.NewRow()
                tr(0) = oResponse.Tasks(i).Customer 'dsAllGroups.Tables(0).Rows(icntGroups)(0)
                tr(1) = oResponse.Tasks(i).Description
                tr(2) = oResponse.Tasks(i).DueDate
                tr(3) = oResponse.Tasks(i).TaskStatusKey.ToString()
                Select Case oResponse.Tasks(i).TaskStatusKey
                    Case 0
                        tr(3) = "New"
                    Case 1
                        tr(3) = "InProgress"
                    Case 2
                        tr(3) = "InComplete"
                    Case 3
                        tr(3) = "Complete"
                End Select

                'If (oResponse.Tasks(i).TaskStatusKey = 0) Then
                '    tr(3) = ""
                'End If

                tr(4) = oResponse.Tasks(i).Type
                tr(5) = oResponse.Tasks(i).Urgent
                tr(6) = oResponse.Tasks(i).UserGroupKey
                tr(7) = oResponse.Tasks(i).UserKey
                tr(8) = oResponse.Tasks(i).TaskInstanceKey
                tr(9) = oResponse.Tasks(i).UserGroupDescription
                tr(10) = oResponse.Tasks(i).UserGroupCode
                tr(11) = oResponse.Tasks(i).UserCode
                dt.Rows.Add(tr)
            Next

            gvTasks.DataSource = dt
            gvTasks.DataBind()
            Label1.Text = dt.Rows.Count
            Session("Arr1") = Nothing
            Session("Arr") = Nothing
        Else
            gvTasks.DataSource = Nothing
            gvTasks.DataBind()
        End If
    End Sub


    Protected Sub btnAddTask_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddTask.Click
        Response.Redirect("2_AddTask.aspx")
    End Sub

    Protected Sub btnComplete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnComplete.Click
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oRequest As New GetWmTaskRequestType
        Dim oResponse As New GetWmTaskResponseType
        With oRequest
            .BranchCode = "HeadOff"
            .TaskInstanceKey = Session("TaskInstanceKey")
        End With
        oResponse = oSAM.GetWmTask(oRequest)
        'oResponse.TimeStamp


        Dim oRequestUpdate As New UpdateWmTaskRequestType
        Dim oResponseUpdate As New UpdateWmTaskResponseType
        oRequestUpdate.BranchCode = "HeadOff"
        oRequestUpdate.Client = gvTasks.SelectedRow.Cells(5).Text
        oRequestUpdate.Description = gvTasks.SelectedRow.Cells(6).Text
        oRequestUpdate.DueDate = Convert.ToDateTime(gvTasks.SelectedRow.Cells(4).Text)
        oRequestUpdate.TaskInstanceKey = Convert.ToInt32(gvTasks.SelectedRow.Cells(9).Text)
        oRequestUpdate.TaskStatusKey = TaskStatus.Complete
        oRequestUpdate.TaskTimeStamp = oResponse.TaskTimestamp
        oRequestUpdate.Urgent = Convert.ToInt32(gvTasks.SelectedRow.Cells(1).Text)

        oRequestUpdate.UserCode = oResponse.UserCode.Trim() 'ddlALLUser.SelectedValue  'gvTasks.SelectedRow.Cells(12).Text
        oRequestUpdate.UserGroupCode = gvTasks.SelectedRow.Cells(11).Text

        'If ddlALLUser.SelectedItem.Text = "(All Group Users)" Then
        '    For i As Integer = 1 To ddlALLUser.Items.Count - 1
        '        If oResponse.UserKey = 0 Then
        '            oRequestUpdate.UserCode = "0"
        '            Exit For
        '        End If
        '        If ddlALLUser.Items(i).Value = oResponse.UserKey Then
        '            oRequestUpdate.UserCode = ddlALLUser.Items(i).Text
        '            Exit For
        '        End If
        '    Next
        '    'oRequestUpdate.UserCode = ddlALLUser.Items(oResponse.UserKey).Text
        'Else
        '    oRequestUpdate.UserCode = ddlALLUser.SelectedItem.Text
        'End If

        'If ddlGroups.SelectedItem.Text = "(All Group)" Then
        '    oRequestUpdate.UserGroupCode = ddlGroups.Items(oResponse.UserGroupKey).Text
        'Else
        '    oRequestUpdate.UserGroupCode = ddlGroups.SelectedItem.Text
        'End If

        oResponseUpdate = oSAM.UpdateWmTask(oRequestUpdate)

        Populate()

    End Sub

    Protected Sub btnIncomplete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIncomplete.Click
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oRequest As New GetWmTaskRequestType
        Dim oResponse As New GetWmTaskResponseType
        With oRequest
            .BranchCode = "HeadOff"
            .TaskInstanceKey = Session("TaskInstanceKey")
        End With
        oResponse = oSAM.GetWmTask(oRequest)
        'oResponse.TimeStamp


        Dim oRequestUpdate As New UpdateWmTaskRequestType
        Dim oResponseUpdate As New UpdateWmTaskResponseType
        oRequestUpdate.BranchCode = "HeadOff"
        oRequestUpdate.Client = gvTasks.SelectedRow.Cells(5).Text
        oRequestUpdate.Description = gvTasks.SelectedRow.Cells(6).Text
        oRequestUpdate.DueDate = Convert.ToDateTime(gvTasks.SelectedRow.Cells(4).Text)
        oRequestUpdate.TaskInstanceKey = Convert.ToInt32(gvTasks.SelectedRow.Cells(9).Text)
        oRequestUpdate.TaskStatusKey = TaskStatus.InComplete
        oRequestUpdate.TaskTimeStamp = oResponse.TaskTimestamp
        oRequestUpdate.Urgent = Convert.ToInt32(gvTasks.SelectedRow.Cells(1).Text)
        oRequestUpdate.UserCode = oResponse.UserCode.Trim() 'gvTasks.SelectedRow.Cells(12).Text.Trim()
        oRequestUpdate.UserGroupCode = gvTasks.SelectedRow.Cells(11).Text 'ddlGroups.Items(Convert.ToInt32(gvTasks.SelectedRow.Cells(7).Text)).Value


        'If ddlALLUser.SelectedItem.Text = "(All Group Users)" Then
        '    For i As Integer = 1 To ddlALLUser.Items.Count - 1
        '        If oResponse.UserKey = 0 Then
        '            oRequestUpdate.UserCode = "0"
        '            Exit For
        '        End If
        '        If ddlALLUser.Items(i).Value = oResponse.UserKey Then
        '            oRequestUpdate.UserCode = ddlALLUser.Items(i).Text
        '            Exit For
        '        End If
        '    Next
        '    'oRequestUpdate.UserCode = ddlALLUser.Items(oResponse.UserKey).Text
        'Else
        '    oRequestUpdate.UserCode = ddlALLUser.SelectedItem.Text
        'End If

        'If ddlGroups.SelectedItem.Text = "(All Group)" Then
        '    oRequestUpdate.UserGroupCode = ddlGroups.Items(oResponse.UserGroupKey).Text
        'Else
        '    oRequestUpdate.UserGroupCode = ddlGroups.SelectedItem.Text
        'End If

       
        oResponseUpdate = oSAM.UpdateWmTask(oRequestUpdate)

        Populate()
    End Sub

    Protected Sub btnAssignSingletask_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAssignSingletask.Click
        Response.Redirect("AssignSingleTask.aspx")
    End Sub

    Protected Sub btnRunTask_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRunTask.Click
        If (CurrentRun.Value) Then
            btnComplete_Click(sender, e)
        Else
            btnIncomplete_Click(sender, e)
        End If
    End Sub
    Private Sub DisableCombo()
        If ddlSystem.SelectedIndex = ShowType.Sys Then
            ddlALLUser.Enabled = False
            ddlDates.Enabled = False
            ddlStatus.Enabled = False
            ddlGroups.Enabled = False
        Else
            ddlALLUser.Enabled = True
            ddlDates.Enabled = True
            ddlStatus.Enabled = True
            ddlGroups.Enabled = True
        End If

    End Sub

    Protected Sub ddlSystem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSystem.SelectedIndexChanged
        DisableCombo()
    End Sub

    Protected Sub ddlGroups_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGroups.SelectedIndexChanged
        'PopulateUser()
        If ddlGroups.SelectedValue <> "(All Group)" Then
            PopulateControl.PopulateUsers(ddlALLUser, ddlGroups.SelectedValue)
        Else
            ddlALLUser.Items.Clear()
            'ddlALLUser.Items.Insert(0, New ListItem("(All Group Users)"))
            PopulateControl.populateallusersWM(ddlALLUser)
        End If



    End Sub

    Protected Sub ddlALLUser_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlALLUser.SelectedIndexChanged

    End Sub
End Class
