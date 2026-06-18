Imports System.Globalization
Imports Sirius.BatchRenewal.Controller
Imports System.Threading.Tasks

<Assembly: CLSCompliant(True)>

Public Class MainForm
    ' Comaand Line Arguments variables.
    Private mBatchId As Integer?
    Private mShowUI As Boolean = True
    Private mXmlPath As String = Nothing
    Private mBatchResume As Boolean = False
    Private mJobCode As String
    Private WithEvents mBatchController As New BatchController()

    Delegate Sub UpdateStatusTextCallback(statusUpdate As String)

    ' Samples of calling.
    ' Parameters:
    '   -batchId, -b            : Batch ID
    '   -showUI, -ui, -s        : Displays a progress UI if True. Default is False.
    '   -xmlpath, -xml, -x      : Loads the batch XML file if a path is provided. The XSD is specified in the app.config file.
    '   -resume, -r             : Resumes an existing bath if True.
    '   -job,-j                 : Runs a spefic job code
    ' BatchRenewalWinController.exe -xml:C:\temp\SampleRenewal.xml
    ' BatchRenewalWinController.exe -b:226 -ui:false -x:C:\temp\SampleRenewal.xml
    ' BatchRenewalWinController.exe -j:RS1 -ui:true

    Private Sub MainFormLoad(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim processed As Boolean = ProcessCommandLine()

        If mShowUI Then
            Opacity = 100
        End If

        If processed Then
            Task.Factory.StartNew(Sub() StartController())
            timer.Enabled = True
        Else
            DisplayHelp()
        End If
    End Sub

    ''' <summary>
    ''' Process the command line arugments
    ''' </summary>
    Private Function ProcessCommandLine() As Boolean

        Try
            Dim info As TextInfo = CultureInfo.CurrentCulture.TextInfo

            ' Check the command line arguments.
            For Each argument As String In My.Application.CommandLineArgs
                Dim key As String = argument.Substring(1, 1).ToUpper()
                Dim value As String = String.Empty

                If argument.Length > 3 Then
                    value = argument.Substring(3)
                End If
                If Trim(value) <> String.Empty Then
                    Select Case key
                        Case "B"
                            mBatchId = CInt(value)
                        Case "S"
                            mShowUI = CBool(info.ToTitleCase(value))
                        Case "X"
                            mXmlPath = value
                        Case "J"
                            mJobCode = value
                    End Select
                End If
            Next
        Catch ex As Exception
            mShowUI = True
            UpdateStatusText(ex.ToString())
            Return False
        End Try
        mShowUI = True
        'mJobCode = "RS2"
        'Check we have to have either a job code or and xml file to continue
        Return Not String.IsNullOrEmpty(mXmlPath) OrElse Not String.IsNullOrEmpty(mJobCode)
    End Function

    ''' <summary>
    ''' Displays the help text
    ''' </summary>
    Private Sub DisplayHelp()
        UpdateStatusText("-B:batchId, Resume's processing the given batch")
        UpdateStatusText("-S:false, Hides the UI")
        UpdateStatusText("-X:path, Process the given xml file")
        UpdateStatusText("-J:path, Renewal job code")

        UpdateStatusText("BatchRenewalWinController.exe -X:C:\Temp\RenwalList.xml")
        UpdateStatusText("BatchRenewalWinController.exe -X:C:\Temp\RenwalList.xml -S:false")
        UpdateStatusText("BatchRenewalWinController.exe -J:RS5 -S:false")

    End Sub

    Private Sub StartController()
        mBatchController.Start(mBatchId, mXmlPath, mBatchResume, mJobCode)
    End Sub

    Private Sub TimerTick(sender As Object, e As EventArgs) Handles timer.Tick
        labelQueueDepth.Text = String.Format("Queue Depth: {0}", mBatchController.QueueDepth)
        RunStatisticBindingSource.DataSource = mBatchController.CurrentStatistics
        timer.Enabled = True
        If Not mShowUI AndAlso mBatchController.QueueDepth = 0 AndAlso mBatchController.CurrentStatistics.Sum(Function(p) p.InProgress) = 0 Then
            End
        End If
    End Sub

    Private Sub BatchControllerStatusUpdate(sender As Object, e As StatusUpdateEventArgs) Handles mBatchController.StatusUpdate
        If e.Level = StatusLevel.Fatal OrElse e.Level = StatusLevel.Information Then
            UpdateStatusText(e.Status)
        End If

    End Sub

    Private Sub UpdateStatusText(ByVal statusUpdate As String)

        If outputText.InvokeRequired Then
            Dim d As New UpdateStatusTextCallback(AddressOf UpdateStatusText)
            Invoke(d, New Object() {statusUpdate})
        Else
            outputText.SuspendLayout()
            outputText.Text = outputText.Text & vbNewLine & statusUpdate
            outputText.SelectionStart = outputText.Text.Length
            outputText.ScrollToCaret()
            outputText.ResumeLayout()
        End If
    End Sub


End Class
