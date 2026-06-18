Imports Microsoft.Practices.EnterpriseLibrary.Logging

''' <summary>
''' Logging wrapper methods, which will log if the code is compiled as debug version
''' </summary>


Friend NotInheritable Class Log

#Region "Constructors"

    ''' <summary>
    ''' This class cannot be instantiated.
    ''' </summary>
    Private Sub New()
    End Sub

#End Region

#Region "Public Shared Methods"

    Public Shared Sub WriteDebug(ByVal message As String)

        Dim logEntry As New LogEntry
        logEntry.Message = message
        logEntry.Categories.Add("Debug")
        logEntry.Severity = TraceEventType.Verbose

        Try
            Logger.Write(logEntry)
        Catch ex As Exception

        Finally

        End Try

    End Sub

    Public Shared Sub WriteDebug(ByVal database As Database, ByVal message As String)

        Dim logEntry As New LogEntry
        logEntry.Message = message
        logEntry.Categories.Add("Debug")
        logEntry.Severity = TraceEventType.Verbose
        logEntry.ExtendedProperties.Add("Database", database)
        Try
            Logger.Write(logEntry)
        Catch ex As Exception

        Finally

        End Try

    End Sub

    Public Shared Sub WriteDebug(ByVal database As Database, ByVal format As String, ByVal ParamArray args As Object())

        WriteDebug(database, String.Format(format, args))

    End Sub

    Public Shared Sub WriteInfo(ByVal message As String)

        Dim logEntry As New LogEntry
        logEntry.Message = message
        logEntry.Categories.Add("NonDebug")
        logEntry.Severity = TraceEventType.Information

        Try
            Logger.Write(logEntry)
        Catch ex As Exception

        Finally

        End Try

    End Sub

    Public Shared Sub WriteWarning(ByVal database As Database, ByVal message As String)

        Dim logEntry As New LogEntry
        logEntry.Message = message
        logEntry.Categories.Add("NonDebug")
        logEntry.Severity = TraceEventType.Warning
        logEntry.ExtendedProperties.Add("Database", database)

        Try
            Logger.Write(logEntry)
        Catch ex As Exception

        Finally

        End Try

    End Sub

    Public Shared Sub WriteWarning(ByVal database As Database, ByVal format As String, ByVal ParamArray args As Object())

        WriteWarning(database, String.Format(format, args))

    End Sub

#End Region

End Class
