Imports System.Web
Imports System.Data.SqlClient
Imports System.Web.Configuration.WebConfigurationManager


    Public Module funcDB

        Function ExecSql(ByRef SQLCommand As SqlCommand, Optional ByVal sConnectionName As String = Nothing) As SqlDataReader
            'Execute Stored Procedure, with parameters and return datareader

            Dim dtrResponse As SqlDataReader
            Dim sqlConnection As SqlConnection

            If sConnectionName Is Nothing Then
                sqlConnection = New SqlConnection(AppSettings("SQLConnection"))
            Else
                sqlConnection = New SqlConnection(ConnectionStrings(sConnectionName).ConnectionString)
            End If

            SQLCommand.Connection = sqlConnection
            SQLCommand.CommandType = CommandType.StoredProcedure
            sqlConnection.Open()
            dtrResponse = SQLCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Return dtrResponse

        End Function

        Sub ExecNonQuery(ByRef SQLCommand As SqlCommand, Optional ByVal sConnectionName As String = Nothing)
            'Execute Stored Procedure, with parameters

            Dim sqlConnection As SqlConnection

            If sConnectionName Is Nothing Then
                sqlConnection = New SqlConnection(AppSettings("SQLConnection"))
            Else
                sqlConnection = New SqlConnection(ConnectionStrings(sConnectionName).ConnectionString)
            End If

            SQLCommand.Connection = sqlConnection
            SQLCommand.CommandType = CommandType.StoredProcedure
            sqlConnection.Open()
            SQLCommand.ExecuteNonQuery()
            'close the connection to free resources
            sqlConnection.Close()

        End Sub
        '*******************************************************************************************************************
        ' PURPOSE: RETURNS A SINGLE STRING VALUE FROM THE DATABASE.
        ' AUTHOR:  MANISH BATRA                 DATE: JUNE 20, 2006
        '*******************************************************************************************************************
        Function ExecScalar(ByRef SQLCommand As SqlCommand, Optional ByVal sConnectionName As String = Nothing) As String

            Dim sValue As String = ""
            Dim sqlConnection As SqlConnection

            If sConnectionName Is Nothing Then
                sqlConnection = New SqlConnection(AppSettings("SQLConnection"))
            Else
                sqlConnection = New SqlConnection(ConnectionStrings(sConnectionName).ConnectionString)
            End If

            SQLCommand.Connection = sqlConnection
            SQLCommand.CommandType = CommandType.StoredProcedure
            sqlConnection.Open()
            sValue = CType(SQLCommand.ExecuteScalar(), String)
            sqlConnection.Close()

            Return sValue

        End Function

        '*******************************************************************************************************************
        ' PURPOSE: EXECUTE THE STORED PROCEDURE, PUT RESULT INTO DATA TABLE AND RETURN THE SAME
        ' AUTHOR:  MANISH BATRA                 DATE: JUNE 20, 2006
        '*******************************************************************************************************************
        Function GetDataTable(ByRef SQLCommand As SqlCommand, Optional ByVal sConnectionName As String = Nothing) As DataTable

            Dim tbTemp As New DataTable
            Dim sqlConnection As SqlConnection

            If sConnectionName Is Nothing Then
                sqlConnection = New SqlConnection(AppSettings("SQLConnection"))
            Else
                sqlConnection = New SqlConnection(ConnectionStrings(sConnectionName).ConnectionString)
            End If

            SQLCommand.Connection = sqlConnection
            SQLCommand.CommandType = CommandType.StoredProcedure
            sqlConnection.Open()
            Dim SQLAdapter As New SqlDataAdapter(SQLCommand)
            SQLAdapter.Fill(tbTemp)

            Return tbTemp

        End Function
    End Module
