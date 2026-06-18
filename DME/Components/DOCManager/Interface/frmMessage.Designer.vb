<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMessage
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
	Public WithEvents tmrTimer As System.Windows.Forms.Timer
	Public WithEvents lblProgress As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMessage))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.tmrTimer = New System.Windows.Forms.Timer(components)
		Me.lblProgress = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' tmrTimer
		' 
		Me.tmrTimer.Enabled = False
		Me.tmrTimer.Interval = 1
		' 
		' lblProgress
		' 
		Me.lblProgress.AutoSize = False
		Me.lblProgress.BackColor = System.Drawing.SystemColors.Control
		Me.lblProgress.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblProgress.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblProgress.Enabled = True
		Me.lblProgress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblProgress.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblProgress.Location = New System.Drawing.Point(8, 16)
		Me.lblProgress.Name = "lblProgress"
		Me.lblProgress.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblProgress.Size = New System.Drawing.Size(313, 25)
		Me.lblProgress.TabIndex = 0
		Me.lblProgress.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me.lblProgress.UseMnemonic = True
		Me.lblProgress.Visible = True
		' 
		' frmMessage
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(333, 50)
		Me.ControlBox = True
		Me.Controls.Add(Me.lblProgress)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmMessage"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "Progress"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class