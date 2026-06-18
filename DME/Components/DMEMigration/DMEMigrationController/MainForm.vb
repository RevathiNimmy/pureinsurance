Imports Pure.DMEMigration.Controller
Imports System.Threading.Tasks
Imports Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase

<Assembly: CLSCompliant(True)> 

Public Class MainForm
    Private WithEvents m_BatchController As New BatchController()
    Private m_BatchId As Integer?
    Private m_FolderNum As Integer?
    Private m_Docs() As Integer
    Private m_ShowUI As Boolean
    Delegate Sub UpdateStatusTextCallback(statusUpdate As String)

    Public Property BatchId() As Integer?
        Get
            Return m_BatchId
        End Get
        Set(ByVal value As Integer?)
            m_BatchId = value
        End Set
    End Property

    Public Property FolderNum() As Integer?
        Get
            Return m_FolderNum
        End Get
        Set(ByVal value As Integer?)
            m_FolderNum = value
        End Set
    End Property

    Public Property Docs() As Integer()
        Get
            Return m_Docs
        End Get
        Set(ByVal value As Integer())
            m_Docs = value
        End Set
    End Property

    Public Property ShowUI() As Boolean
        Get
            Return m_ShowUI
        End Get
        Set(ByVal value As Boolean)
            m_ShowUI = value
        End Set
    End Property

    Private Sub MainForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        End
    End Sub

    Public Sub MainFormLoad()
        If m_ShowUI = True Then
            Me.Opacity = 100
        End If

        If (m_BatchId.HasValue = False And m_FolderNum.HasValue = False And m_Docs Is Nothing) Then
            Me.Opacity = 100
            DisplayHelp()
        Else
            Task.Factory.StartNew(Sub() StartController())
            timer.Enabled = True
        End If
    End Sub

    ''' <summary>
    ''' Displays the help text
    ''' </summary>
    Private Sub DisplayHelp()
        UpdateStatusText("-B:batchId, Resume's processing for the given batch")
        UpdateStatusText("-S:false, Hides the UI")
    End Sub

    Private Sub StartController()
        m_BatchController.Start(m_BatchId, m_FolderNum, m_Docs)
    End Sub

    Public Sub EnqueueNextBatch(ByVal iBatchId As Integer?, ByVal iFolderNum As Integer?, ByVal iDocNums() As Integer)
        If m_ShowUI = True Then
            Me.Opacity = 100
        Else
            Me.Opacity = 0
        End If

        If (BatchId.HasValue = False And FolderNum.HasValue = False And m_Docs Is Nothing) Then
            DisplayHelp()
        Else
            Task.Factory.StartNew(Sub() ResumeController(iBatchId, iFolderNum, iDocNums))
        End If

        timer.Enabled = True
    End Sub

    Private Sub ResumeController(ByVal iBatchId As Integer?, ByVal iFolderNum As Integer?, ByVal iDocNums() As Integer)
        m_BatchController.EnqueueNextBatch(iBatchId, iFolderNum, iDocNums)
        timer.Enabled = True
    End Sub

    Private Sub TimerTick(sender As Object, e As EventArgs) Handles timer.Tick
        labelQueueDepth.Text = String.Format("Documents in queue: {0}", m_BatchController.QueueDepth)
        RunStatisticBindingSource.DataSource = m_BatchController.CurrentStatistics

        If Not m_ShowUI AndAlso m_BatchController.QueueDepth = 0 AndAlso m_BatchController.CurrentStatistics.Sum(Function(p) p.InProgress) = 0 Then
            End
        End If
    End Sub

    Private Sub BatchControllerStatusUpdate(sender As Object, e As StatusUpdateEventArgs) Handles m_BatchController.StatusUpdate
        If e.Level = StatusLevel.Fatal OrElse e.Level = StatusLevel.Information Then
            UpdateStatusText(e.Status)
        End If

    End Sub

    Private Sub UpdateStatusText(ByVal sStatusUpdate As String)

        If outputText.InvokeRequired Then
            Dim d As New UpdateStatusTextCallback(AddressOf UpdateStatusText)
            Invoke(d, New Object() {sStatusUpdate})
        Else
            outputText.SuspendLayout()
            outputText.Text = outputText.Text & vbNewLine & sStatusUpdate
            outputText.SelectionStart = outputText.Text.Length
            outputText.ScrollToCaret()
            outputText.ResumeLayout()
        End If
    End Sub


End Class
