<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProgress
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
	Public WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
	Public WithEvents lblProcess As System.Windows.Forms.Label
	Public WithEvents lblStatus As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmProgress))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.ProgressBar1 = New System.Windows.Forms.ProgressBar
		Me.lblProcess = New System.Windows.Forms.Label
		Me.lblStatus = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' ProgressBar1
		' 
		Me.ProgressBar1.Location = New System.Drawing.Point(8, 56)
		Me.ProgressBar1.Name = "ProgressBar1"
		Me.ProgressBar1.Size = New System.Drawing.Size(353, 17)
		Me.ProgressBar1.TabIndex = 0
		' 
		' lblProcess
		' 
		Me.lblProcess.AutoSize = False
		Me.lblProcess.BackColor = System.Drawing.SystemColors.Control
		Me.lblProcess.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblProcess.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblProcess.Enabled = True
		Me.lblProcess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblProcess.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblProcess.Location = New System.Drawing.Point(8, 8)
		Me.lblProcess.Name = "lblProcess"
		Me.lblProcess.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblProcess.Size = New System.Drawing.Size(345, 17)
		Me.lblProcess.TabIndex = 2
		Me.lblProcess.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me.lblProcess.UseMnemonic = True
		Me.lblProcess.Visible = True
		' 
		' lblStatus
		' 
		Me.lblStatus.AutoSize = False
		Me.lblStatus.BackColor = System.Drawing.SystemColors.Control
		Me.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblStatus.Enabled = True
		Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblStatus.Location = New System.Drawing.Point(8, 32)
		Me.lblStatus.Name = "lblStatus"
		Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblStatus.Size = New System.Drawing.Size(345, 17)
		Me.lblStatus.TabIndex = 1
		Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me.lblStatus.UseMnemonic = True
		Me.lblStatus.Visible = True
		' 
		' frmProgress
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(366, 77)
		Me.ControlBox = True
		Me.Controls.Add(Me.ProgressBar1)
		Me.Controls.Add(Me.lblProcess)
		Me.Controls.Add(Me.lblStatus)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmProgress.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmProgress"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "DocuMaster to Sirius Briefcase download"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class