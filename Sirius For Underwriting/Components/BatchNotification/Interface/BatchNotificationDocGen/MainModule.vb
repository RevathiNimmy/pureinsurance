Module MainModule

#Region "Fields"

    Public Const ACApp As String = "BatchNotificationDocGen"

    Private m_sSAMURL As String
    Private m_sSAMUsername As String
    Private m_sSAMPassword As String
    Private m_sBranchCode As String
    Private m_iNumberOfThreads As Integer
    Private m_sClientId As String
    Private m_sTenantId As String
    Private m_sTokenUrl As String
    Private m_lTimeOut As Integer
    Private m_Operation As DocGenOperation
    Private m_sFilePath As String
    Private m_sDocCode As String
    Private m_sBatchRef As String
#End Region

#Region "Main Method"
    Sub Main()
        Dim oInterface As DocGen = Nothing

        Try
            ' Check for parameters
            If My.Application.CommandLineArgs.Count = 0 Then
                ' No parameters so default to plain help
                OutputSyntax()
                Exit Sub
            End If

            ' Strip command line
            ProcessCommandLine()

            ProcessSettings()

            oInterface = New DocGen(m_sSAMURL, m_sSAMUsername, m_sSAMPassword,
                                    m_sBranchCode, m_iNumberOfThreads, m_Operation, m_sFilePath,
                                    m_sDocCode, m_sBatchRef, m_lTimeOut, m_sClientId, m_sTenantId, m_sTokenUrl)

            ' Process export
            oInterface.ProcessDocumentGeneration()


            OutputLine("BatchNotificationDocGen Complete")

        Catch ex As Exception

            OutputLine("BatchNotificationDocGen FAILED - " & ex.Message.ToString())
        Finally
            oInterface = Nothing
            Environment.Exit(0)
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
        Dim bOperationSpecified As Boolean

        ' Process args
        For Each sArg As String In My.Application.CommandLineArgs
            sArgValues = sArg.Split(CChar("="))
            ' Process and argument switches
            Try

                ' determine which argument we are looking at
                Select Case sArgValues(0).ToUpper

                    Case "OPERATION"
                        If sArgValues(1).ToUpper = "NEW" Then
                            m_Operation = DocGenOperation.NewRun
                        ElseIf sArgValues(1).ToUpper = "RERUN" Then
                            m_Operation = DocGenOperation.Rerun
                        ElseIf sArgValues(1).ToUpper = "PURGE" Then
                            m_Operation = DocGenOperation.Purge
                        Else
                            Throw New ArgumentException("Invalid argument value specified " + sArgValues(0).ToString)
                        End If
                        bOperationSpecified = True
                    Case "FILEPATH"
                        m_sFilePath = sArgValues(1).ToString

                    Case "DOCCODE"
                        m_sDocCode = sArgValues(1).ToString

                    Case "BATCHREF"
                        m_sBatchRef = sArgValues(1).ToString
                    Case Else
                        Throw New ArgumentException("Invalid argument specified " + sArgValues(0).ToString)
                End Select
            Catch ex As Exception

                Throw New ArgumentException("Invalid argument value " + sArgValues(0).ToString, ex)

            End Try
        Next

        If String.IsNullOrEmpty(m_sDocCode) And (m_Operation <> DocGenOperation.Purge) Then
            Dim ex As Exception = Nothing
            Throw New ApplicationException("Mandatory command line argument not found - DocCode", ex)
        ElseIf String.IsNullOrEmpty(m_sBatchRef) And (m_Operation <> DocGenOperation.Purge) Then
            Dim ex As Exception = Nothing
            Throw New ApplicationException("Mandatory command line argument not found - BatchRef", ex)
        ElseIf Not bOperationSpecified Then
            Dim ex As Exception = Nothing
            Throw New ApplicationException("Mandatory command line argument not found - Operation", ex)
        End If

        If m_Operation = DocGenOperation.NewRun Then
            If Not System.IO.File.Exists(m_sFilePath) Then
                Throw New ApplicationException("Path does not exist. " & m_sFilePath)
            End If
        End If
    End Sub

    Private Sub ProcessSettings()
        If String.IsNullOrEmpty(My.Settings.SAMURL) Then
            Throw New ApplicationException("SAMURL not set in App.Config ")
        End If
        m_sSAMURL = My.Settings.SAMURL

        If String.IsNullOrEmpty(My.Settings.SAMUsername) Then
            Throw New ApplicationException("SAMUsername not set in App.Config ")
        End If
        m_sSAMUsername = My.Settings.SAMUsername

        m_sSAMPassword = My.Settings.SAMPassword

        If String.IsNullOrEmpty(My.Settings.BranchCode) Then
            Throw New ApplicationException("BranchCode not set in App.Config ")
        End If
        m_sBranchCode = My.Settings.BranchCode
        If String.IsNullOrEmpty(My.Settings.NumberOfThreads) Then
            Throw New ApplicationException("BranchCode not set in App.Config ")
        End If
        m_iNumberOfThreads = My.Settings.NumberOfThreads
        If m_iNumberOfThreads <= 0 Or m_iNumberOfThreads > 200 Then
            Throw New ApplicationException("Number of Threads value <=0 or >200")
        End If

        m_lTimeOut = My.Settings.TimeOut

        If m_lTimeOut <= 0 Then
            Throw New ApplicationException("TimeOut should be > 0")
        End If
        If String.IsNullOrEmpty(My.Settings.ClientId) Then
            Throw New ApplicationException("ClientId not set in App.Config ")
        End If
        m_sClientId = My.Settings.ClientId

        m_sTenantId = My.Settings.TenantId
        If String.IsNullOrEmpty(My.Settings.TokenUrl) Then
            Throw New ApplicationException("Token Url not set in App.Config ")
        End If
        m_sTokenUrl = My.Settings.TokenUrl
    End Sub

    Private Sub OutputSyntax()
        ' Write basic syntax
        OutputLine("Example Call : BatchNotificationDocGen Operation=""New"" Filepath=""c:\temp\export001.xml"" DocCode=""NOTIF12"" BatchRef=""ABC123""")
        OutputLine()
        OutputLine("  Operation    - (mandatory) Either New, Rerun or Purge")
        OutputLine("  Filepath     - (optional) The full filepath of the the xml document to be processed")
        OutputLine("  DocCode      - (mandatory) The code of the document template to be generated")
        OutputLine("  BatchRef	   - (mandatory) A unique reference for batch")

    End Sub
#End Region

End Module
