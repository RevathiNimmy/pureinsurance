Imports System.Globalization
Namespace My

    ' The following events are available for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend NotInheritable Class MyApplication
        Private mForm As MainForm

        Private Sub MyApplication_Startup(sender As Object, e As ApplicationServices.StartupEventArgs) Handles Me.Startup
        ' Command Line Arguments variables.
        Dim mBatchId As Integer?
        Dim mShowUI As Boolean = True
        Dim mFolderNum As Integer?
        Dim mDocs() As Integer = Nothing

        Try
            Dim info As TextInfo = CultureInfo.CurrentCulture.TextInfo
            mForm = New MainForm

            ' Check the command line arguments.
            For Each argument As String In My.Application.CommandLineArgs
                Dim key As String = argument.Substring(1, 1).ToUpper()
                Dim value As String = String.Empty

                If argument.Length > 3 Then
                    value = argument.Substring(3)
                End If

                Select Case key
                    Case "B"
                        mBatchId = CInt(value)
                    Case "S"
                        mShowUI = CBool(info.ToTitleCase(value))
                    Case "F"
                        mFolderNum = CInt(value)
                    Case "D"
                        If mDocs Is Nothing Then
                            ReDim Preserve mDocs(0)
                        Else
                            ReDim Preserve mDocs(UBound(mDocs) + 1)
                        End If

                        mDocs(UBound(mDocs)) = CInt(value)
                End Select
            Next
        Catch ex As Exception
            mShowUI = True
        End Try


        If (mBatchId.HasValue OrElse mFolderNum.HasValue OrElse mDocs IsNot Nothing) Then
            With mForm
                .BatchId = mBatchId
                .ShowUI = mShowUI
                .FolderNum = mFolderNum
                .Docs = mDocs
            End With
        End If

        mForm.Show()
        mForm.MainFormLoad()
    End Sub
    Private Sub MyApplication_StartupNextInstance(ByVal sender As Object,
            ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs) Handles Me.StartupNextInstance
        ' Command Line Arguments variables.
        Dim mBatchId As Integer?
        Dim mShowUI As Boolean = True
        Dim mFolderNum As Integer?
        Dim mDocs() As Integer = Nothing

        Try
            Dim info As TextInfo = CultureInfo.CurrentCulture.TextInfo

            ' Check the command line arguments.
            For Each argument As String In e.CommandLine
                Dim key As String = argument.Substring(1, 1).ToUpper()
                Dim value As String = String.Empty

                If argument.Length > 3 Then
                    value = argument.Substring(3)
                End If

                Select Case key
                    Case "B"
                        mBatchId = CInt(value)
                    Case "S"
                        mShowUI = CBool(info.ToTitleCase(value))
                    Case "F"
                        mFolderNum = CInt(value)
                    Case "D"
                        If mDocs Is Nothing Then
                            ReDim Preserve mDocs(0)
                        Else
                            ReDim Preserve mDocs(UBound(mDocs) + 1)
                        End If

                        mDocs(UBound(mDocs)) = CInt(value)
                End Select
            Next
        Catch ex As Exception
            mShowUI = True
        End Try

        mForm.ShowUI = mShowUI
        mForm.EnqueueNextBatch(mBatchId, mFolderNum, mDocs)
        mForm.BringToFront()

    End Sub
    End Class
End Namespace

