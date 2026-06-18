Imports System.Data
Imports System.Data.SqlClient

'Imports Nexus.Utils.Nexus

Imports Nexus.Utils

Imports System.Web
Imports System.Configuration

Public Class ErrorLog

    Public Function GetErrorLog() As DataTable

        Dim command As New SqlCommand("usp_GetErrorLog")

        With command.Parameters
            .Add("@PortalID", SqlDbType.Int).Value = HttpContext.Current.Session("PortalID")
        End With

        GetErrorLog = funcDB.GetDataTable(command, "CMS")
        command.Dispose()

    End Function

    Public Function GetSessionDetails(ByVal SessionID As String) As DataTable

        'TEMP should this go in a 'SessionTracking' project? & use a Sproc

        Dim cn As New SqlConnection(ConfigurationManager.ConnectionStrings("CMS").ConnectionString)
        cn.Open()
        Dim dad As New SqlDataAdapter("Select * From tbl_session_tracker Where SessionTrackerID = @SessionID", cn)
        With dad.SelectCommand
            .Parameters.Add("@SessionID", SqlDbType.Int).Value = SessionID
        End With
        Dim dt As New DataTable
        dad.Fill(dt)
        cn.Close()
        Return dt

    End Function

    Public Function GetSessionPages(ByVal SessionID As String) As DataTable

        'TEMP should this go in a 'SessionTracking' project? & use a Sproc

        Dim cn As New SqlConnection(ConfigurationManager.ConnectionStrings("CMS").ConnectionString)
        cn.Open()
        Dim dad As New SqlDataAdapter("Select * From tbl_session_tracker_pages Where SessionTrackerID = @SessionID Order by PageTime Desc", cn)
        With dad.SelectCommand
            .Parameters.Add("@SessionID", SqlDbType.Int).Value = SessionID
        End With
        Dim dt As New DataTable
        dad.Fill(dt)
        cn.Close()
        Return dt

    End Function

End Class
