Module MainModule

#Region "Application Constants"
    Public Const ACApp As String = "BatchNotificationExport"
#End Region

#Region "Fields"
    ' Basic command details
    Private m_bIsHelp As Boolean = False
    Private m_sProcedureName As String
    Private m_dtStartDate As DateTime
    Private m_dtEndDate As DateTime
    Private m_cArgs As System.Collections.ObjectModel.Collection(Of String) = Nothing
    Private m_sInterface As String = String.Empty
#End Region

#Region "Main Method"

    Sub Main()
        Dim oInterface As NotificationExport = Nothing

        Try
            ' Check for parameters
            If My.Application.CommandLineArgs.Count = 0 Then
                ' No parameters so default to plain help
                OutputSyntax()
                Exit Sub
            End If

            ' Strip command line
            ProcessCommandLine()

            oInterface = New NotificationExport(m_sProcedureName, m_dtStartDate, m_dtEndDate)

            ' Process export
            oInterface.ProcessExport()


            OutputLine("BatchNotificationExport Complete")

        Catch ex As Exception

            OutputLine("BatchNotificationExport FAILED" & Environment.NewLine & Environment.NewLine & ex.ToString())
        Finally

            oInterface = Nothing

        End Try
    End Sub

#End Region

#Region "Public Methods"
    ' Outputs feedback, currently to the console
    Public Sub OutputLine(ByVal message As String)
        ' Write message with carriage return
        Console.WriteLine(message)
    End Sub

    Public Sub OutputLine()
        ' Write carriage return (or new line)
        Console.WriteLine()
    End Sub

    Public Sub Output(ByVal message As String)
        ' Write message without carriage return
        Console.Write(message)
    End Sub
#End Region

#Region "Private Methods"
    Private Sub ProcessCommandLine()
        Dim sArgValues() As String

        ' Process args
        For Each sArg As String In My.Application.CommandLineArgs
            sArgValues = sArg.Split(CChar("="))
            ' Process and argument switches
            Try

                ' determine which argument we are looking at
                Select Case sArgValues(0).ToUpper

                    Case "PROCEDURE"
                        m_sProcedureName = sArgValues(1)

                    Case "STARTDATE"
                        m_dtStartDate = CDate(sArgValues(1))

                    Case "ENDDATE"
                        m_dtEndDate = CDate(sArgValues(1))
                    Case Else
                        Throw New ArgumentException("Invalid argument specified " + sArgValues(0).ToString)
                End Select
            Catch ex As Exception

                Throw New ArgumentException("Invalid argument value " + sArgValues(0).ToString, ex)
                OutputSyntax()
            End Try
        Next

        If String.IsNullOrEmpty(m_sProcedureName) Then
            Dim ex As Exception = Nothing
            ' raise an exception
            Throw New ApplicationException("Mandatory command line argument not found - Procedure", ex)
            OutputSyntax()
        End If
    End Sub

    Private Sub OutputSyntax()
        ' Write basic syntax
        OutputLine("Example call : - BatchNotificationExport Procedure=""ExportParties"" StartDate=""01/01/2008"" EndDate=""01/01/2009""")
        OutputLine()
        OutputLine("  Procedure    - (mandatory) The stored procedure that will be called for the export")
        OutputLine("  StartDate    - (optional) Start date that will be passed to the procedure")
        OutputLine("  EndDate      - (optional) End date that will be passed to the procedure")

    End Sub
#End Region
End Module
