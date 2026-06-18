Imports System.Web.Security
Imports Nexus.Library

Namespace Nexus
    Public Class NexusRoleProvider
        Inherits SqlRoleProvider

        Public Overrides Function GetRolesForUser(ByVal username As String) As String()
            'Get list of usergroups from back office for the current user
            'Assign user to roles equivilent to these user groups
            If HttpContext.Current.Application(username & "_rolelist") IsNot Nothing Then
                Return CType(HttpContext.Current.Application(username & "_rolelist"), ArrayList).ToArray(GetType(String))
            Else
                Dim EmptyArray As String()
                Return EmptyArray
            End If
        End Function
    End Class
End Namespace
