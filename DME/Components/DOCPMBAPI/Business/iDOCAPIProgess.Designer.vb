<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
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
	Public WithEvents cmdPause As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents proProgress As System.Windows.Forms.ProgressBar
	Public WithEvents panCurrentFile As System.Windows.Forms.Panel
	Public WithEvents fraProgress As System.Windows.Forms.GroupBox
	Public WithEvents SSPanel1 As System.Windows.Forms.Panel
    Public WithEvents panProgress As System.Windows.Forms.Panel
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdPause = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.panProgress = New System.Windows.Forms.Panel
		Me.fraProgress = New System.Windows.Forms.GroupBox
		Me.proProgress = New System.Windows.Forms.ProgressBar
		Me.panCurrentFile = New System.Windows.Forms.Panel
		Me.SSPanel1 = New System.Windows.Forms.Panel
		Me.panProgress.SuspendLayout()
		Me.fraProgress.SuspendLayout()
		Me.SuspendLayout()
		' 
		' cmdPause
		' 
		Me.cmdPause.BackColor = System.Drawing.SystemColors.Control
		Me.cmdPause.CausesValidation = True
		Me.cmdPause.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdPause.Enabled = True
		Me.cmdPause.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdPause.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdPause.Location = New System.Drawing.Point(288, 120)
		Me.cmdPause.Name = "cmdPause"
		Me.cmdPause.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdPause.Size = New System.Drawing.Size(73, 22)
		Me.cmdPause.TabIndex = 4
		Me.cmdPause.TabStop = True
		Me.cmdPause.Text = "Pause"
		Me.cmdPause.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdPause.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(376, 120)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 3
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' panProgress
		' 
		Me.panProgress.BackColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.panProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.panProgress.Controls.Add(Me.fraProgress)
		Me.panProgress.Controls.Add(Me.SSPanel1)
		Me.panProgress.Dock = System.Windows.Forms.DockStyle.Top
		Me.panProgress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.panProgress.Location = New System.Drawing.Point(0, 0)
		Me.panProgress.Name = "panProgress"
		Me.panProgress.Size = New System.Drawing.Size(597, 158)
		Me.panProgress.TabIndex = 0
		' 
		' fraProgress
		' 
		Me.fraProgress.BackColor = System.Drawing.SystemColors.Control
		Me.fraProgress.Controls.Add(Me.proProgress)
		Me.fraProgress.Controls.Add(Me.panCurrentFile)
		Me.fraProgress.Enabled = True
		Me.fraProgress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraProgress.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraProgress.Location = New System.Drawing.Point(16, 32)
		Me.fraProgress.Name = "fraProgress"
		Me.fraProgress.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraProgress.Size = New System.Drawing.Size(441, 81)
		Me.fraProgress.TabIndex = 5
		Me.fraProgress.Text = "Importing Data"
		Me.fraProgress.Visible = True
		' 
		' proProgress
		' 
		Me.proProgress.Location = New System.Drawing.Point(8, 56)
		Me.proProgress.Name = "proProgress"
		Me.proProgress.Size = New System.Drawing.Size(425, 17)
		Me.proProgress.TabIndex = 7
		' 
		' panCurrentFile
		' 
		Me.panCurrentFile.BackColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.panCurrentFile.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.panCurrentFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        'To Do
        'Me.panCurrentFile.Font3D = 3
		Me.panCurrentFile.Location = New System.Drawing.Point(8, 24)
		Me.panCurrentFile.Name = "panCurrentFile"
		Me.panCurrentFile.Size = New System.Drawing.Size(353, 17)
		Me.panCurrentFile.TabIndex = 6
		' 
		' SSPanel1
		' 
		Me.SSPanel1.BackColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.SSPanel1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.SSPanel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.SSPanel1.Location = New System.Drawing.Point(24, 8)
		Me.SSPanel1.Name = "SSPanel1"
		Me.SSPanel1.Size = New System.Drawing.Size(345, 17)
		Me.SSPanel1.TabIndex = 2
		' 
		' frmInterface
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(597, 157)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdPause)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.panProgress)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmInterface.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "DocuMaster API Daemon Progress"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.panProgress.ResumeLayout(False)
		Me.fraProgress.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class