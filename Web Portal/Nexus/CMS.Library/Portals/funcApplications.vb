Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Web
Imports System.Web.HttpContext
Imports Nexus.Utils
Imports Nexus.Utils.Cache

Namespace Portal

    Public Class AppFunctions

        Public Shared Function GetApplications() As DataTable

            Dim command As New SqlCommand("spGetApplications")
            GetApplications = GetDataTable(command, "CMS")
            command.Dispose()

        End Function

    End Class

End Namespace