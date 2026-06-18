Imports System.Data.SqlClient
Imports System.Web.HttpContext
Imports System.Web.Security

Imports Nexus.Utils

Namespace Members

    Public Class Group

        Private iId As Integer
        Private sName As String
        Private iNoOfUsers As Integer
        Private bAccessAllowed As Integer

        Public Sub New(ByVal v_iID As Integer)

            Dim command As New SqlCommand("usp_GetGroup")

            command.Parameters.Add("@group_id", SqlDbType.Int).Value = v_iID

            Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command, "CMS")

            If sdrTmp.HasRows Then

                While sdrTmp.Read

                    iId = v_iID
                    sName = sdrTmp("group_name")
                    iNoOfUsers = sdrTmp("no_of_users")

                End While

            End If

            sdrTmp.Close()
            command.Dispose()

        End Sub

        Public Sub New(ByVal v_iId As Integer, ByVal v_sName As String, ByVal v_iNoOfUsers As Integer, _
                    ByVal v_bAccessAllowed As Boolean)

            iId = v_iId
            sName = v_sName
            iNoOfUsers = v_iNoOfUsers
            bAccessAllowed = v_bAccessAllowed

        End Sub

        Public Property Id() As Integer
            Get
                Return iId
            End Get
            Set(ByVal value As Integer)
                iId = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return sName
            End Get
            Set(ByVal value As String)
                sName = value
            End Set
        End Property

        Public Property NoOfUsers() As Integer
            Get
                Return iNoOfUsers
            End Get
            Set(ByVal value As Integer)
                iNoOfUsers = value
            End Set
        End Property

        Public Property AccessAllowed() As Boolean
            Get
                Return bAccessAllowed
            End Get
            Set(ByVal value As Boolean)
                value = bAccessAllowed
            End Set
        End Property

        Public Shared Function EditGroup(ByVal v_sName As String, ByVal v_sRole As String, _
                                            Optional ByVal v_iGroupID As Integer = 0)

            Dim command As New SqlCommand("usp_EditGroup")

            command.Parameters.Add("@group_id", SqlDbType.Int).Value = v_iGroupID
            command.Parameters.Item("@group_id").Direction = ParameterDirection.InputOutput
            command.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = v_sName
            command.Parameters.Add("@role", SqlDbType.NVarChar, 256).Value = v_sRole
            command.Parameters.Add("@return", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

            funcDB.ExecSql(command, "CMS")

            If command.Parameters.Item("@return").Value > 0 Then
                Return command.Parameters.Item("@group_id").Value
            Else
                Return 0
            End If

            command.Dispose()

        End Function

        Public Shared Sub DeleteGroup(ByVal v_iGroupID As Integer)

            Dim command As New SqlCommand("usp_DeleteGroup")

            command.Parameters.Add("@group_id", SqlDbType.Int).Value = v_iGroupID

            funcDB.ExecNonQuery(command, "CMS")

            command.Dispose()

        End Sub

        Public Shared Sub AddPortals(ByVal v_iGroupID As Integer, ByVal v_sCommaSeperatedPortalID As String)

            Dim command As New SqlCommand("usp_AddPortalsToGroup")

            command.Parameters.Add("@group_id", SqlDbType.Int).Value = v_iGroupID
            command.Parameters.Add("@portal_ids", SqlDbType.NVarChar, 256).Value = v_sCommaSeperatedPortalID

            funcDB.ExecNonQuery(command, "CMS")

            command.Dispose()

        End Sub

        Public Shared Sub AddActions(ByVal v_iGroupID As Integer, ByVal v_sCommaSeperatedActionID As String)

            Dim command As New SqlCommand("usp_AddActionsToGroup")

            command.Parameters.Add("@group_id", SqlDbType.Int).Value = v_iGroupID
            command.Parameters.Add("@action_ids", SqlDbType.NVarChar, 256).Value = v_sCommaSeperatedActionID

            funcDB.ExecNonQuery(command, "CMS")

            command.Dispose()

        End Sub

        Public Shared Function GetGroupsInRole(ByVal v_sRoleName As String) As ArrayList

            Dim oGroups As ArrayList = Nothing

            Dim command As New SqlCommand("usp_GetGroupsInRole")

            command.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = Membership.ApplicationName
            command.Parameters.Add("@RoleName", SqlDbType.NVarChar, 256).Value = v_sRoleName

            Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command, "CMS")

            If sdrTmp.HasRows Then

                oGroups = New ArrayList

                While sdrTmp.Read

                    oGroups.Add(New Group(sdrTmp("group_id"), sdrTmp("group_name"), sdrTmp("no_of_users"), _
                            sdrTmp("allowed")))

                End While

            End If

            sdrTmp.Close()
            command.Dispose()

            Return oGroups

        End Function

        Public Shared Function GetGroupsByUser(ByVal v_oUserID As Guid) As ArrayList

            Dim oGroups As ArrayList = Nothing

            Dim command As New SqlCommand("usp_GetGroupsByUser")
            command.Parameters.Add("@user_id", SqlDbType.UniqueIdentifier).Value = v_oUserID
            command.Parameters.Add("@authenticated_username", SqlDbType.NVarChar, 256).Value = Current.User.Identity.Name

            Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command, "CMS")

            If sdrTmp.HasRows Then

                oGroups = New ArrayList

                While sdrTmp.Read

                    oGroups.Add(New Group(sdrTmp("group_id"), sdrTmp("group_name"), sdrTmp("no_of_users"), _
                                        sdrTmp("allowed")))

                End While

            End If

            sdrTmp.Close()
            command.Dispose()

            Return oGroups

        End Function

        Public Shared Function GetGroupActionsByCategory(ByVal v_iGroupID As Integer, _
                                                            ByVal v_iCategoryID As Integer) As ArrayList

            Dim oActions As ArrayList = Nothing

            Dim oCommand As New SqlCommand("usp_GetGroupActionsByCategory")
            oCommand.Parameters.Add("@group_id", SqlDbType.Int).Value = v_iGroupID
            oCommand.Parameters.Add("@cat_id", SqlDbType.Int).Value = v_iCategoryID

            Dim sdrTmp As SqlDataReader = funcDB.ExecSql(oCommand, "CMS")

            If sdrTmp.HasRows Then

                oActions = New ArrayList()

                While sdrTmp.Read

                    oActions.Add(New Action(sdrTmp("action_id"), sdrTmp("name"), sdrTmp("label"), _
                                    sdrTmp("description"), sdrTmp("allowed")))

                End While

            End If

            sdrTmp.Close()
            oCommand.Dispose()

            Return oActions

        End Function

    End Class

End Namespace