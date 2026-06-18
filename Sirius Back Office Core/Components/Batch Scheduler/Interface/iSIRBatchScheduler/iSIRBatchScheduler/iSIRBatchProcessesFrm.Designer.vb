<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmSelectBatchProcess
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()

    End Sub
    Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
        Dispose(True)
    End Sub
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
     Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
        If Disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(Disposing)
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents txtReportName As System.Windows.Forms.TextBox
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents lvwReportSchedulerDetail As System.Windows.Forms.ListView
    Public WithEvents lblBatchProcesses As System.Windows.Forms.Label
    Public WithEvents lblReportName As System.Windows.Forms.Label
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cboBatchProcessList = New System.Windows.Forms.ComboBox()
        Me.lblBatchProcesses = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(493, 31)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 6
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cboBatchProcessList
        '
        Me.cboBatchProcessList.FormattingEnabled = True
        Me.cboBatchProcessList.Location = New System.Drawing.Point(256, 32)
        Me.cboBatchProcessList.Name = "cboBatchProcessList"
        Me.cboBatchProcessList.Size = New System.Drawing.Size(224, 21)
        Me.cboBatchProcessList.TabIndex = 12

        '
        'lblBatchProcesses
        '
        Me.lblBatchProcesses.AutoSize = True
        Me.lblBatchProcesses.BackColor = System.Drawing.SystemColors.Control
        Me.lblBatchProcesses.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBatchProcesses.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBatchProcesses.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBatchProcesses.Location = New System.Drawing.Point(16, 32)
        Me.lblBatchProcesses.Name = "lblBatchProcesses"
        Me.lblBatchProcesses.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBatchProcesses.Size = New System.Drawing.Size(234, 13)
        Me.lblBatchProcesses.TabIndex = 9
        Me.lblBatchProcesses.Text = "Select the Process which you want to schedule:"
        '
        'frmSelectBatchProcess
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(582, 117)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cboBatchProcessList)
        Me.Controls.Add(Me.lblBatchProcesses)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSelectBatchProcess"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Select Batch Process"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents lblSeprateBy As System.Windows.Forms.Label
    Friend WithEvents cboBatchProcessList As System.Windows.Forms.ComboBox
#End Region
End Class