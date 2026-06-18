Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Imports System.Data
Imports System.Data.SqlClient
Partial Class Work_Manager_Exposure_2_ViewTask
    Inherits System.Web.UI.Page
    Dim Str As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        Dim oRequest As New GetWmTaskRequestType
        Dim oResponse As New GetWmTaskResponseType
        Dim TaskInstanceKey As Integer
        Dim dtGroups As New DataTable
        Dim dtTaskGroups As New DataTable
        TaskInstanceKey = Session("TaskInstanceKey")
        '''Binding Task group to ddlTaskGroup
        'DataBind to Dropdowns- Jigar
        If Not Page.IsPostBack Then
            'Populate.PopulateUserGroups(ddlUserGroup)
            PopulateGroups()
            PopulateUser()
            'PopulateControl.PopulateUserGroups(ddlUserGroup, dtGroups)
            'PopulateControl.populateallusersWM(ddlUser)
            
            With oRequest
                .BranchCode = "HeadOff"
                .TaskInstanceKey = TaskInstanceKey '1923
            End With

            Try
                'BuildLists(oSAM, ddlTaskGroup, STSListType.PMLookup, "PMwrk_Task_Group")
                'BuildLists(oSAM, ddlDueDateTime, STSListType.PMLookup, "Analysis_code")
                'BuildLists(oSAM, ddlTask, STSListType.PMLookup, "PMwrk_Task")
                'BuildLists(oSAM, ddlUserGroup, STSListType.PMLookup, "PMUser_Group")
                'BuildLists(oSAM, ddlUser, STSListType.PMLookup, "PMUser")

                PopulateControl.PopulateTaskGroups(ddlTaskGroup, dtGroups, "TaskGroupKey")


                oResponse = oSAM.GetWmTask(oRequest)
                PopulateControl.populateTaskgroupTask(ddlTask, ddlTaskGroup, dtGroups, dtTaskGroups, oResponse.TaskGroupKey, "TaskKey")

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
                    txtDate.Text = .DueDate
                    If .UserKey = 0 Then
                        ddlUser.SelectedValue = 0
                    Else
                        ddlUser.SelectedValue = .UserKey
                    End If
                    'ddlUser.SelectedValue = 0 '.UserKey
                    ddlUserGroup.SelectedValue = 1 '.UserGroupKey
                    lblLastModifiedBy.Text = "sirius" 'ddlUser.SelectedItem.Text
                    lblLoggedBy.Text = "sirius" 'ddlUser.SelectedItem.Text
                    'txtDate.Text = .DateCreated
                    lblAtLoggedBy.Text = .DateCreated
                    lblAt.Text = .LastModified
                    If oResponse.TaskStatusKey = 3 Then
                        chkComplete.Checked = True
                    Else
                        chkComplete.Checked = False

                    End If
                    If oResponse.IsUrgent = 1 Then

                        chkUrgent.Checked = True
                    Else

                        chkUrgent.Checked = False
                    End If
                    If oResponse.IsTaskReview = 1 Then
                        chkIsTaskReview.Checked = True
                    Else
                        chkIsTaskReview.Checked = False
                    End If
                    '' For Viewing purpose
                    pnlTaskDetails.Enabled = False
                    pnlAllocation.Enabled = False
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

        End If
    End Sub

    Protected Sub btnTaskLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTaskLog.Click

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
                    objControl.DataValueField = "Key"
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

    Protected Sub mnuAddTask_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles mnuAddTask.MenuItemClick
        mvAddTask.ActiveViewIndex = Convert.ToInt32(e.Item.Value)
    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Session("Arr") = Nothing
        Session("Arr1") = Nothing
        Response.Redirect("gettask.aspx")
    End Sub
#Region "Developed new-- Combo Box filling -- Jigar"
    Public Sub PopulateGroups()
        'Str = "Data Source=localhost;Initial Catalog=Sirius_UIIC;Persist Security Info=True;User ID=sa"
        'rk updates following as part of SAM SFI Interop conversions
        Str = "Data Source=NIIT-ssp-sqlsvr;Initial Catalog=Siriusdev;Integrated Security=False; User ID=SIRIUS;Password=$1R1U5"
        'Str = "Data Source=VN-035\SQL2005;Initial Catalog=BMark;Integrated Security=False; User ID=SIRIUS;Password=$1R1U5"
        'Str = "Data Source=localhost;Initial Catalog=Sirius_UIIC;Persist Security Info=True;User ID=sa"
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
        'rk updates following as part of SAM SFI Interop conversions
        Str = "Data Source=NIIT-SSP-SQLSVR;Initial Catalog=Sirius115;Integrated Security=False; User ID=SIRIUS;Password=$1R1U5"
        'Str = "Data Source=localhost;Initial Catalog=Sirius_UIIC;Persist Security Info=True;User ID=sa"
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
