Imports SharedFiles
Module MainModule

#Region "Application Constants"
    Public Const ACApp As String = "bSIRBatchQuoteDeletion"
#End Region

#Region "Fields"
    Private m_oInterface As QuoteDeletion = Nothing
#End Region

#Region "Main Method"

    ''' <summary>
    ''' Entry point for the batch quote deletion application
    ''' </summary>
    Sub Main()
        Try
            ' Initialize the quote deletion interface
            m_oInterface = New QuoteDeletion()

            ' Process command line (reads config settings)
            ProcessCommandLine()

            ' Clean up resources
            m_oInterface.CloseDBConnection()
            m_oInterface.CleanUpInterops()

        Catch ex As Exception
            ' Fatal error handling
            OutputLine()
            OutputLine("═══════════════════════════════════════════════════════════════")
            OutputLine("    ❌ BATCH QUOTE DELETION FAILED")
            OutputLine("═══════════════════════════════════════════════════════════════")
            OutputLine($"Error: {ex.Message}")
            OutputLine()
            OutputLine("Stack Trace:")
            OutputLine(ex.StackTrace)
            OutputLine("═══════════════════════════════════════════════════════════════")
            OutputLine()
            ReadLine()
        End Try
    End Sub

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Outputs a line of text to the console
    ''' </summary>
    Public Sub OutputLine(ByVal message As String)
        Console.WriteLine(message)
    End Sub

    ''' <summary>
    ''' Outputs a blank line to the console
    ''' </summary>
    Public Sub OutputLine()
        Console.WriteLine()
    End Sub

    ''' <summary>
    ''' Outputs text to the console without a newline
    ''' </summary>
    Public Sub Output(ByVal message As String)
        Console.Write(message)
    End Sub

    ''' <summary>
    ''' Waits for user input before exiting
    ''' </summary>
    Public Sub ReadLine()
        OutputLine()
        OutputLine("Press <Enter> to exit.")
        Console.ReadLine()
    End Sub

#End Region

#Region "Private Methods"

    ''' <summary>
    ''' Processes the batch deletion using configuration settings
    ''' </summary>
    Private Sub ProcessCommandLine()
        ' Read configuration settings
        Dim sUserName As String = My.Settings.SAMUserName
        Dim sPassword As String = "" ' Not used anymore, kept for backward compatibility
        Dim lQuotesDeleted As Long = 0
        Dim lReturn As Long = gPMConstants.PMEReturnCode.PMTrue

        ' Execute the batch deletion
        lReturn = m_oInterface.QuoteDeletion(sUserName, sPassword, lQuotesDeleted)

        ' Generate CSV reports for today's run
        Dim dtToday As Date = Date.Today
        Dim sOutputPath As String = My.Settings.ReportOutputPath
        m_oInterface.GenerateCsvReports(dtToday, dtToday, sOutputPath)

        ' Wait for user acknowledgment
        ReadLine()
    End Sub

#End Region

End Module