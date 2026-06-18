Imports Microsoft.Web.Services3.Security.Tokens
Imports System.Data
Imports System.Data.SqlClient

Imports SAMForInsuranceV2
Imports System.Collections
Partial Class Work_Manager_Exposure_ReAssignTask
    Inherits System.Web.UI.Page
    Dim oSAM As New SAMForInsuranceV2
    Dim str As String
    Dim dtGroups As New DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object

        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        If Not Page.IsPostBack Then
            'BuildLists(oSAM, ddlUserGroup, STSListType.PMLookup, "PMUser_Group")
            'PopulateGroups()
            'PopulateUser()
            PopulateControl.PopulateUserGroups(ddlUserGroup, dtGroups)
            PopulateControl.populateallusersWM(ddlUser)

        End If


        Dim dt As New System.Data.DataTable
        Dim dtDisplay As New System.Data.DataTable
        dt = Session("Table")

        Dim ArrInstanceKey As New ArrayList()
        ArrInstanceKey = Session("Arr1")
        Dim dv As New System.Data.DataView
        Dim str As String
        str = "TaskInstanceKey in ("
        For i As Integer = 0 To ArrInstanceKey.Count - 1
            str += ArrInstanceKey(i) & ","
        Next
        str = str.TrimEnd(",")
        str += ")"

        dt.DefaultView.RowFilter = str
        dtDisplay = dv.Table()
        gvSelectedTask.DataSource = dt.DefaultView
        gvSelectedTask.DataBind()



        'Dim a() As Byte


        '    'Dim chk As New CheckBox
        '    'chk = CType(grd_Output.Rows(iCount).FindControl("chkSelect"), CheckBox)
        '    ' If (chk.Checked) Then
        '    oReAssignMultipleWmTasksRequest.Tasks(iCount) = New BaseReAssignMultipleWmTasksRequestTypeRow
        '    oReAssignMultipleWmTasksRequest.Tasks(iCount).TaskInstanceKey = Convert.ToInt32(gvSelectedTask.Rows(iCount).Cells(1).Text.ToString())
        '    oReAssignMultipleWmTasksRequest.Tasks(iCount).UserGroupCode = Convert.ToString(gvSelectedTask.Rows(iCount).Cells(7).Text.ToString())
        '    oReAssignMultipleWmTasksRequest.Tasks(iCount).UserCode = Convert.ToString(gvSelectedTask.Rows(iCount).Cells(8).Text.ToString())
        '    oReAssignMultipleWmTasksRequest.Tasks(iCount).QuoteTimeStamp = a
        '    ' End If
        'Next
        ' Dim oReAssignMultipleWmTasksResponse As New BaseReAssignMultipleWmTasksResponseType

        'oReAssignMultipleWmTasksResponse = oSAM.ReAssignMultipleWmTasks(oReAssignMultipleWmTasksRequest)

    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim oReAssignMultipleWmTasksRequest As New ReAssignMultipleWmTasksRequestType
        Dim iCount As Integer = 0

        oReAssignMultipleWmTasksRequest.BranchCode = "HEADOFF"

        ReDim Preserve oReAssignMultipleWmTasksRequest.Tasks(gvSelectedTask.Rows.Count - 1)
        For iCount = 0 To gvSelectedTask.Rows.Count - 1
            oReAssignMultipleWmTasksRequest.Tasks(iCount) = New BaseReAssignMultipleWmTasksRequestTypeRow
            oReAssignMultipleWmTasksRequest.Tasks(iCount).TaskInstanceKey = Convert.ToInt32(gvSelectedTask.Rows(iCount).Cells(8).Text.ToString())
            oReAssignMultipleWmTasksRequest.Tasks(iCount).UserGroupCode = ddlUserGroup.SelectedValue  'ddlUserGroup.Items(ddlUserGroup.SelectedIndex).Value
            If ddlUser.SelectedItem.Text = "(All Group Users)" Then
                oReAssignMultipleWmTasksRequest.Tasks(iCount).UserCode = ""
                'ddlUser.SelectedItem.Text  '"sirius"
            Else

                oReAssignMultipleWmTasksRequest.Tasks(iCount).UserCode = ddlUser.SelectedItem.Text  '"sirius"
            End If


        Next
        Dim oReAssignMultipleWmTasksResponse As New BaseReAssignMultipleWmTasksResponseType
        oReAssignMultipleWmTasksResponse = oSAM.ReAssignMultipleWmTasks(oReAssignMultipleWmTasksRequest)
        Session("Arr") = Nothing
        Session("Arr1") = Nothing
        Response.Redirect("../Work Manager Exposure/gettask.aspx")
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

    Protected Sub ddlUserGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUserGroup.SelectedIndexChanged
        'PopulateUser()
        If ddlUserGroup.SelectedValue <> "(All Group)" Then
            PopulateControl.PopulateUsers(ddlUser, ddlUserGroup.SelectedValue)
        Else
            ddlUser.Items.Clear()
            'ddlALLUser.Items.Insert(0, New ListItem("(All Group Users)"))
            PopulateControl.populateallusersWM(ddlUser)
        End If
    End Sub
End Class
