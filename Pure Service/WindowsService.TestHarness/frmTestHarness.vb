Imports Ssp.Pure.Service.ProcessJobs

Public Class frmTestHarness
    Private _info As New BackgroundJobInfo

    Private Sub cmdProcessBackgroundJob_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdProcessBackgroundJob.Click

        InitaliseSettings()

        Dim oProcessJobs As New BackgroundJobProcess(_info)

        If oProcessJobs.ProcessAllJobs(_info.Database) Then
            MsgBox("process successfully")
        End If

    End Sub

    Private Sub InitaliseSettings()

        Dim database As New Database

        _info.Source = System.Environment.MachineName.ToString
        _info.Database = database
        _info.MessageBatchSize = 1  ' Default to 1
        _info.PollingDelay = TimeSpan.FromMilliseconds(10000)
        _info.RetryDelay = TimeSpan.FromHours(1)

    End Sub

End Class
