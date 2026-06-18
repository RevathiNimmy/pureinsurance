Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Web.HttpContext
Imports System.Web.Security

Imports Nexus.Utils
Imports Nexus.Utils.Cache
Imports CMS.Library

Namespace Members

    Public Class User

        Private iRecordCount As Integer = 0

        Public Function RecordCount(ByVal v_sRoleName As String, _
                                ByVal v_sUserName As String, _
                                ByVal v_sGroups As String) As Integer

            Return iRecordCount

        End Function

        Public Shared Function GetPortals(ByVal v_sUserName As String, _
                                        Optional ByVal v_iGroupID As Integer = 0) As ArrayList

            Dim arTmp As ArrayList = Nothing

            Dim command As New SqlCommand("usp_GetAllPortals")
            command.Parameters.Add("@username", SqlDbType.NVarChar, 256).Value = v_sUserName
            command.Parameters.Add("@group_id", SqlDbType.Int).Value = v_iGroupID

            Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command, "CMS")

            If sdrTmp.HasRows Then

                arTmp = New ArrayList

                While sdrTmp.Read
                    arTmp.Add(New Portal.Portal(sdrTmp("portal_id"), sdrTmp("name"), sdrTmp("url"), _
                                sdrTmp("otherurls"), sdrTmp("culture_code"), sdrTmp("master_page"), _
                                sdrTmp("theme"), sdrTmp("allowed")))
                End While

            End If


            sdrTmp.Close()
            command.Dispose()

            Return arTmp

        End Function

        Public Function GetUsersInRole(ByVal v_sRoleName As String, _
                                            ByVal v_sUserName As String, _
                                            ByVal v_sGroups As String, _
                                            ByVal startRowIndex As Integer, _
                                            ByVal maximumRows As Integer) As MembershipUserCollection

            Dim oUsers As MembershipUserCollection = Nothing

            Dim command As New SqlCommand("usp_GetUsersInRole")
            command.CommandType = CommandType.StoredProcedure
            command.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = Membership.ApplicationName
            command.Parameters.Add("@RoleName", SqlDbType.NVarChar, 256).Value = v_sRoleName
            command.Parameters.Add("@UserName", SqlDbType.NVarChar, 256).Value = v_sUserName
            command.Parameters.Add("@group_ids", SqlDbType.NVarChar, 256).Value = v_sGroups
            command.Parameters.Add("@PageIndex", SqlDbType.Int).Value = startRowIndex
            command.Parameters.Add("@PageSize", SqlDbType.Int).Value = maximumRows
            command.Parameters.Add("@TotalRecords", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

            Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command, "CMS")

            If sdrTmp.HasRows Then

                oUsers = New MembershipUserCollection

                While sdrTmp.Read
                    oUsers.Add(New MembershipUser(Membership.Provider.Name, sdrTmp("UserName"), _
                        sdrTmp("UserId"), sdrTmp("Email"), sdrTmp("PasswordQuestion"), sdrTmp("Comment"), _
                        sdrTmp("IsApproved"), sdrTmp("IsLockedOut"), sdrTmp("CreateDate"), _
                        sdrTmp("LastLoginDate"), sdrTmp("LastActivityDate"), sdrTmp("LastPasswordChangedDate"), _
                        sdrTmp("LastLockoutDate")))
                End While

            End If

            sdrTmp.Close()

            iRecordCount = command.Parameters("@TotalRecords").Value

            command.Dispose()

            Return oUsers

        End Function

        Public Shared Sub AddGroups(ByVal v_oUserID As Guid, ByVal v_sCommaSeperatedGroupID As String)

            If Not String.IsNullOrEmpty(v_sCommaSeperatedGroupID) Then

                Dim command As New SqlCommand("usp_AddGroupsToUser")

                command.Parameters.Add("@user_id", SqlDbType.UniqueIdentifier).Value = v_oUserID
                command.Parameters.Add("@group_ids", SqlDbType.NVarChar, 256).Value = v_sCommaSeperatedGroupID

                funcDB.ExecNonQuery(command, "CMS")

                command.Dispose()

            End If

        End Sub

    End Class

End Namespace