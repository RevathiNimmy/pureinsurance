<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmDMETransfer
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
	Public WithEvents CmdExit As System.Windows.Forms.Button
	Public WithEvents CmdRun As System.Windows.Forms.Button
	Public WithEvents ChkClaim As System.Windows.Forms.CheckBox
	Public WithEvents ChkPolicies As System.Windows.Forms.CheckBox
	Public WithEvents ChkClient As System.Windows.Forms.CheckBox
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmDMETransfer))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.ProgressBar1 = New System.Windows.Forms.ProgressBar
		Me.CmdExit = New System.Windows.Forms.Button
		Me.CmdRun = New System.Windows.Forms.Button
		Me.ChkClaim = New System.Windows.Forms.CheckBox
		Me.ChkPolicies = New System.Windows.Forms.CheckBox
		Me.ChkClient = New System.Windows.Forms.CheckBox
		Me.Label3 = New System.Windows.Forms.Label
		Me.Label2 = New System.Windows.Forms.Label
		Me.Label1 = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' ProgressBar1
		' 
		Me.ProgressBar1.Location = New System.Drawing.Point(16, 144)
		Me.ProgressBar1.Name = "ProgressBar1"
		Me.ProgressBar1.Size = New System.Drawing.Size(313, 17)
		Me.ProgressBar1.TabIndex = 8
		' 
		' CmdExit
		' 
		Me.CmdExit.BackColor = System.Drawing.SystemColors.Control
		Me.CmdExit.CausesValidation = True
		Me.CmdExit.Cursor = System.Windows.Forms.Cursors.Default
		Me.CmdExit.Enabled = True
		Me.CmdExit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.CmdExit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.CmdExit.Location = New System.Drawing.Point(256, 64)
		Me.CmdExit.Name = "CmdExit"
		Me.CmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.CmdExit.Size = New System.Drawing.Size(81, 33)
		Me.CmdExit.TabIndex = 6
		Me.CmdExit.TabStop = True
		Me.CmdExit.Text = "E&xit"
		Me.CmdExit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CmdExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' CmdRun
		' 
		Me.CmdRun.BackColor = System.Drawing.SystemColors.Control
		Me.CmdRun.CausesValidation = True
		Me.CmdRun.Cursor = System.Windows.Forms.Cursors.Default
		Me.CmdRun.Enabled = True
		Me.CmdRun.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.CmdRun.ForeColor = System.Drawing.SystemColors.ControlText
		Me.CmdRun.Location = New System.Drawing.Point(256, 16)
		Me.CmdRun.Name = "CmdRun"
		Me.CmdRun.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.CmdRun.Size = New System.Drawing.Size(81, 33)
		Me.CmdRun.TabIndex = 5
		Me.CmdRun.TabStop = True
		Me.CmdRun.Text = "&Run"
		Me.CmdRun.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CmdRun.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' ChkClaim
		' 
		Me.ChkClaim.Appearance = System.Windows.Forms.Appearance.Normal
		Me.ChkClaim.BackColor = System.Drawing.SystemColors.Control
		Me.ChkClaim.CausesValidation = True
		Me.ChkClaim.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.ChkClaim.CheckState = System.Windows.Forms.CheckState.Checked
		Me.ChkClaim.Cursor = System.Windows.Forms.Cursors.Default
		Me.ChkClaim.Enabled = True
		Me.ChkClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.ChkClaim.ForeColor = System.Drawing.SystemColors.ControlText
		Me.ChkClaim.Location = New System.Drawing.Point(16, 72)
		Me.ChkClaim.Name = "ChkClaim"
		Me.ChkClaim.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ChkClaim.Size = New System.Drawing.Size(161, 33)
		Me.ChkClaim.TabIndex = 4
		Me.ChkClaim.TabStop = True
		Me.ChkClaim.Text = "Update Claims"
		Me.ChkClaim.Visible = True
		' 
		' ChkPolicies
		' 
		Me.ChkPolicies.Appearance = System.Windows.Forms.Appearance.Normal
		Me.ChkPolicies.BackColor = System.Drawing.SystemColors.Control
		Me.ChkPolicies.CausesValidation = True
		Me.ChkPolicies.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.ChkPolicies.CheckState = System.Windows.Forms.CheckState.Checked
		Me.ChkPolicies.Cursor = System.Windows.Forms.Cursors.Default
		Me.ChkPolicies.Enabled = True
		Me.ChkPolicies.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.ChkPolicies.ForeColor = System.Drawing.SystemColors.ControlText
		Me.ChkPolicies.Location = New System.Drawing.Point(16, 40)
		Me.ChkPolicies.Name = "ChkPolicies"
		Me.ChkPolicies.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ChkPolicies.Size = New System.Drawing.Size(161, 33)
		Me.ChkPolicies.TabIndex = 3
		Me.ChkPolicies.TabStop = True
		Me.ChkPolicies.Text = "Update Policies"
		Me.ChkPolicies.Visible = True
		' 
		' ChkClient
		' 
		Me.ChkClient.Appearance = System.Windows.Forms.Appearance.Normal
		Me.ChkClient.BackColor = System.Drawing.SystemColors.Control
		Me.ChkClient.CausesValidation = True
		Me.ChkClient.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.ChkClient.CheckState = System.Windows.Forms.CheckState.Checked
		Me.ChkClient.Cursor = System.Windows.Forms.Cursors.Default
		Me.ChkClient.Enabled = True
		Me.ChkClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.ChkClient.ForeColor = System.Drawing.SystemColors.ControlText
		Me.ChkClient.Location = New System.Drawing.Point(16, 8)
		Me.ChkClient.Name = "ChkClient"
		Me.ChkClient.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ChkClient.Size = New System.Drawing.Size(161, 33)
		Me.ChkClient.TabIndex = 2
		Me.ChkClient.TabStop = True
		Me.ChkClient.Text = "Update Clients"
		Me.ChkClient.Visible = True
		' 
		' Label3
		' 
		Me.Label3.AutoSize = False
		Me.Label3.BackColor = System.Drawing.SystemColors.Control
		Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label3.Enabled = True
		Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label3.Location = New System.Drawing.Point(168, 80)
		Me.Label3.Name = "Label3"
		Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label3.Size = New System.Drawing.Size(113, 25)
		Me.Label3.TabIndex = 7
		Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label3.UseMnemonic = True
		Me.Label3.Visible = True
		' 
		' Label2
		' 
		Me.Label2.AutoSize = False
		Me.Label2.BackColor = System.Drawing.SystemColors.Control
		Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label2.Enabled = True
		Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label2.Location = New System.Drawing.Point(128, 112)
		Me.Label2.Name = "Label2"
		Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label2.Size = New System.Drawing.Size(209, 25)
		Me.Label2.TabIndex = 1
		Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label2.UseMnemonic = True
		Me.Label2.Visible = True
		' 
		' Label1
		' 
		Me.Label1.AutoSize = False
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.Enabled = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Location = New System.Drawing.Point(16, 112)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(113, 25)
		Me.Label1.TabIndex = 0
		Me.Label1.Text = "Updating Client :"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' FrmDMETransfer
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(344, 169)
		Me.ControlBox = True
		Me.Controls.Add(Me.ProgressBar1)
		Me.Controls.Add(Me.CmdExit)
		Me.Controls.Add(Me.CmdRun)
		Me.Controls.Add(Me.ChkClaim)
		Me.Controls.Add(Me.ChkPolicies)
		Me.Controls.Add(Me.ChkClient)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.Label1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 19)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "FrmDMETransfer"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "DME Transfer"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class