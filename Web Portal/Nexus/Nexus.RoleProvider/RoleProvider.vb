Imports System.Web.Security

Namespace Nexus.RoleProvider
    Public Class RoleProvider
        Inherits System.Web.Security.SqlRoleProvider

        Public Overrides Function IsUserInRole(ByVal username As String, ByVal roleName As String) As Boolean

        End Function
    End Class
End Namespace
