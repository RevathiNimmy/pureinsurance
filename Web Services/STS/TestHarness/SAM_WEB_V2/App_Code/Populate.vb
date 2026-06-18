Imports Microsoft.VisualBasic
Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Imports System.Data
Imports System.Data.SqlClient
Public Class PopulateControl

    Public Shared Sub PopulateTaskGroups(ByRef ddlTaskGroup As DropDownList, ByRef dtGroups As DataTable, ByVal ColBind As String)
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        'Dim dtGroups As New DataTable
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        Dim oRequest As New GetTaskGroupsRequestType
        Dim oResponse As New GetTaskGroupsResponseType
        oRequest.BranchCode = "HeadOff"
        oResponse = oSAM.GetTaskGroups(oRequest)



        Dim tc1 As DataColumn = New DataColumn()
        tc1.DataType = System.Type.GetType("System.String")
        tc1.ColumnName = "Description"



        Dim tc2 As DataColumn = New DataColumn()
        tc2.DataType = System.Type.GetType("System.Boolean")
        tc2.ColumnName = "IsDeleted"

        Dim tc3 As DataColumn = New DataColumn()
        tc3.DataType = System.Type.GetType("System.Int32")
        tc3.ColumnName = "TaskGroupKey"

        Dim tc4 As DataColumn = New DataColumn()
        tc4.DataType = System.Type.GetType("System.String")
        tc4.ColumnName = "Code"

        dtGroups.Columns.Add(tc1)
        dtGroups.Columns.Add(tc2)
        dtGroups.Columns.Add(tc3)
        dtGroups.Columns.Add(tc4)
        For i As Integer = 0 To UBound(oResponse.TaskGroups)
            Dim tr As System.Data.DataRow
            'tr = dsGroups.Tables(0).NewRow()
            tr = dtGroups.NewRow()
            tr("IsDeleted") = oResponse.TaskGroups(i).IsDeleted
            tr("Description") = oResponse.TaskGroups(i).Description
            tr("TaskGroupKey") = oResponse.TaskGroups(i).TaskGroupKey
            tr("Code") = oResponse.TaskGroups(i).Code
            dtGroups.Rows.Add(tr)
        Next

        Dim strFilter As String = "IsDeleted =false"
        dtGroups.DefaultView.RowFilter = strFilter
        ddlTaskGroup.DataSource = dtGroups
        ddlTaskGroup.DataTextField = "Description"
        ddlTaskGroup.DataValueField = ColBind '"Code"
        ddlTaskGroup.DataBind()
        'Session("TaskGroup") = dtGroups
    End Sub

    Public Shared Sub populateTaskgroupTask(ByRef ddlTask As DropDownList, ByRef ddlTaskGroup As DropDownList, ByVal dtGroups As DataTable, ByRef dtTaskGroup As DataTable, ByVal TaskGroupKey As Integer, ByVal ColBind As String)

        'dtGroups = Session("TaskGroup")
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        'Dim dt As New DataTable


        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2


        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        Dim oRequest As New GetTaskGroupTasksRequestType
        Dim oResponse As New GetTaskGroupTasksResponseType
        oRequest.BranchCode = "HeadOff"
        oRequest.EffectiveDate = DateTime.Now

        If TaskGroupKey = 0 Then
            oRequest.TaskGroupKey = Convert.ToInt32(dtGroups.Rows(ddlTaskGroup.SelectedIndex)("TaskGroupKey")) 'ddlTaskGroup.SelectedValue
        Else
            oRequest.TaskGroupKey = TaskGroupKey
        End If


        oResponse = oSAM.GetTaskGroupTasks(oRequest)
        If Not oResponse Is Nothing Then

        End If
        'ddlDisplay.SelectedIndex = oResponse.TaskGroupTasks(0).TaskCategoryKey
        '  buidling table

        Dim tc1 As DataColumn = New DataColumn()
        tc1.DataType = System.Type.GetType("System.String")
        tc1.ColumnName = "Name"

        Dim tc2 As DataColumn = New DataColumn()
        tc2.DataType = System.Type.GetType("System.String")
        tc2.ColumnName = "Description"

        Dim tc3 As DataColumn = New DataColumn()
        tc3.DataType = System.Type.GetType("System.DateTime")
        tc3.ColumnName = "EffectiveDate"

        Dim tc4 As DataColumn = New DataColumn()
        tc4.DataType = System.Type.GetType("System.Boolean")
        tc4.ColumnName = "IsAvailable"

        Dim tc5 As DataColumn = New DataColumn()
        tc5.DataType = System.Type.GetType("System.Int32")
        tc5.ColumnName = "DisplayIcon"

        Dim tc6 As DataColumn = New DataColumn()
        tc6.DataType = System.Type.GetType("System.Int32")
        tc6.ColumnName = "TaskCategoryKey"

        Dim tc7 As DataColumn = New DataColumn()
        tc7.DataType = System.Type.GetType("System.Boolean")
        tc7.ColumnName = "IsDeleted"

        Dim tc8 As DataColumn = New DataColumn()
        tc8.DataType = System.Type.GetType("System.Boolean")
        tc8.ColumnName = "IsIncluded"

        Dim tc9 As DataColumn = New DataColumn()
        tc9.DataType = System.Type.GetType("System.Int32")
        tc9.ColumnName = "TaskKey"
        Dim tc10 As DataColumn = New DataColumn()
        tc10.DataType = System.Type.GetType("System.Boolean")
        tc10.ColumnName = "IsViewOnly"


        dtTaskGroup.Columns.Add(tc1)
        dtTaskGroup.Columns.Add(tc2)
        dtTaskGroup.Columns.Add(tc3)
        dtTaskGroup.Columns.Add(tc4)
        dtTaskGroup.Columns.Add(tc5)
        dtTaskGroup.Columns.Add(tc6)
        dtTaskGroup.Columns.Add(tc7)
        dtTaskGroup.Columns.Add(tc8)
        dtTaskGroup.Columns.Add(tc9)
        dtTaskGroup.Columns.Add(tc10)

        For i As Integer = 0 To UBound(oResponse.TaskGroupTasks)
            Dim tr As System.Data.DataRow
            'tr = dsGroups.Tables(0).NewRow()

            tr = dtTaskGroup.NewRow()
            tr("Name") = oResponse.TaskGroupTasks(i).Name
            tr("Description") = oResponse.TaskGroupTasks(i).Description
            tr("IsAvailable") = oResponse.TaskGroupTasks(i).IsAvailable
            tr("IsDeleted") = oResponse.TaskGroupTasks(i).IsDeleted
            tr("IsIncluded") = oResponse.TaskGroupTasks(i).IsIncluded
            tr("EffectiveDate") = oResponse.TaskGroupTasks(i).EffectiveDate
            tr("IsViewOnly") = oResponse.TaskGroupTasks(i).IsViewOnly
            tr("TaskKey") = oResponse.TaskGroupTasks(i).TaskKey
            tr("TaskCategoryKey") = oResponse.TaskGroupTasks(i).TaskCategoryKey
            dtTaskGroup.Rows.Add(tr)
        Next

        Dim strFilter As String = "IsIncluded = true"
        dtTaskGroup.DefaultView.RowFilter = strFilter
        ddlTask.DataSource = dtTaskGroup.DefaultView
        ddlTask.DataTextField = "Description"
        ddlTask.DataValueField = ColBind '"TaskKey"
        ddlTask.DataBind()



    End Sub

    Public Shared Sub PopulateUserGroups(ByRef ddlGroups As DropDownList, ByRef dtGroups As DataTable)
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")


        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        Dim oRequest As New GetUserGroupsRequestType
        Dim oResponse As New GetUserGroupsResponseType
        oRequest.BranchCode = "HeadOff"
        oResponse = oSAM.GetUserGroups(oRequest)


        Dim tc1 As DataColumn = New DataColumn()
        tc1.DataType = System.Type.GetType("System.String")
        tc1.ColumnName = "Description"


        Dim tc2 As DataColumn = New DataColumn()
        tc2.DataType = System.Type.GetType("System.Boolean")
        tc2.ColumnName = "IsDeleted"

        Dim tc3 As DataColumn = New DataColumn()
        tc3.DataType = System.Type.GetType("System.Int32")
        tc3.ColumnName = "UserGroupKey"

        Dim tc4 As DataColumn = New DataColumn()
        tc4.DataType = System.Type.GetType("System.String")
        tc4.ColumnName = "Code"

        dtGroups.Columns.Add(tc1)
        dtGroups.Columns.Add(tc2)
        dtGroups.Columns.Add(tc3)
        dtGroups.Columns.Add(tc4)

        For i As Integer = 0 To UBound(oResponse.UserGroups)
            Dim tr As System.Data.DataRow
            'tr = dsGroups.Tables(0).NewRow()
            tr = dtGroups.NewRow()
            tr("IsDeleted") = oResponse.UserGroups(i).IsDeleted
            tr("Description") = oResponse.UserGroups(i).Description
            tr("UserGroupKey") = oResponse.UserGroups(i).UserGroupKey
            tr("Code") = oResponse.UserGroups(i).Code
            dtGroups.Rows.Add(tr)

        Next
        Dim strFilter As String = "IsDeleted =false"
        dtGroups.DefaultView.RowFilter = strFilter
        ddlGroups.DataSource = dtGroups
        ddlGroups.DataTextField = "Description"
        ddlGroups.DataValueField = "Code"
        ddlGroups.DataBind()
        ddlGroups.Items.Insert(0, New ListItem("(All Group)"))
    End Sub
  
    Public Shared Sub PopulateUsers(ByRef ddlUsers As DropDownList, ByVal UserGroupCode As String)
        ddlUsers.Items.Clear()
        Dim oSAM As New SAMForInsuranceV2
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")


        Dim oRequest As New GetUserGroupUsersRequestType
        Dim oResponse As New GetUserGroupUsersResponseType

        oRequest.BranchCode = "HeadOff"
        oRequest.EffectiveDate = DateTime.Now
        ' oRequest.UserGroupKeySpecified = True
        oRequest.UserGroupCode = UserGroupCode 'Session("UserGroupKey")
        oRequest.RestrictUserList = False
        oRequest.RestrictUserListSpecified = False
        oResponse = oSAM.GetUserGroupUsers(oRequest)

        ddlUsers.DataSource = oResponse.UserGroupUsers

        ddlUsers.DataTextField = "Name"
        ddlUsers.DataValueField = "UserKey"
        ddlUsers.DataBind()
        ddlUsers.Items.Insert(0, New ListItem("(All Group Users)"))
    End Sub
    Public Shared Sub populateallusers(ByRef lstAllUsers As ListBox)
        lstAllUsers.Items.Clear()
        Dim oSAM As New SAMForInsuranceV2
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")


        Dim oRequest As New GetUserGroupUsersRequestType
        Dim oResponse As New GetUserGroupUsersResponseType

        oRequest.BranchCode = "HeadOff"
        oRequest.EffectiveDate = DateTime.Now
        'oRequest.UserGroupKey = 0 'Session("UserGroupKey")
        oRequest.RestrictUserList = False
        oRequest.RestrictUserListSpecified = False
        oResponse = oSAM.GetUserGroupUsers(oRequest)

        lstAllUsers.DataSource = oResponse.UserGroupUsers

        lstAllUsers.DataTextField = "Name"
        lstAllUsers.DataValueField = "UserKey"
        lstAllUsers.DataBind()

    End Sub
    Public Shared Sub populateallusersWM(ByRef ddlAllUsers As DropDownList)
        ddlAllUsers.Items.Clear()
        Dim oSAM As New SAMForInsuranceV2
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")


        Dim oRequest As New GetUserGroupUsersRequestType
        Dim oResponse As New GetUserGroupUsersResponseType

        oRequest.BranchCode = "HeadOff"
        oRequest.EffectiveDate = DateTime.Now
        'oRequest.UserGroupCode  =  'Session("UserGroupKey")
        oRequest.RestrictUserList = False
        oRequest.RestrictUserListSpecified = False
        oResponse = oSAM.GetUserGroupUsers(oRequest)

        ddlAllUsers.DataSource = oResponse.UserGroupUsers

        ddlAllUsers.DataTextField = "Name"
        ddlAllUsers.DataValueField = "UserKey"
        ddlAllUsers.DataBind()
        ddlAllUsers.Items.Insert(0, New ListItem("(All Group Users)"))
    End Sub
End Class
