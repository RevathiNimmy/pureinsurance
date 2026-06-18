<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmVerify
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
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
	Public WithEvents CmdHelp As System.Windows.Forms.Button
	Public WithEvents TxtPassword As System.Windows.Forms.TextBox
	Public WithEvents Image1 As System.Windows.Forms.PictureBox
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents FraVerify As System.Windows.Forms.GroupBox
	Public WithEvents CmdCancel As System.Windows.Forms.Button
	Public WithEvents CmdOK As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmVerify))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.CmdHelp = New System.Windows.Forms.Button
		Me.FraVerify = New System.Windows.Forms.GroupBox
		Me.TxtPassword = New System.Windows.Forms.TextBox
		Me.Image1 = New System.Windows.Forms.PictureBox
		Me.Label1 = New System.Windows.Forms.Label
		Me.CmdCancel = New System.Windows.Forms.Button
		Me.CmdOK = New System.Windows.Forms.Button
		Me.FraVerify.SuspendLayout()
		Me.SuspendLayout()
		' 
		' CmdHelp
		' 
		Me.CmdHelp.BackColor = System.Drawing.SystemColors.Control
		Me.CmdHelp.CausesValidation = True
		Me.CmdHelp.Cursor = System.Windows.Forms.Cursors.Default
		Me.CmdHelp.Enabled = True
		Me.CmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.CmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.CmdHelp.Location = New System.Drawing.Point(304, 80)
		Me.CmdHelp.Name = "CmdHelp"
		Me.CmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.CmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.CmdHelp.TabIndex = 5
		Me.CmdHelp.TabStop = True
		Me.CmdHelp.Text = "&Help"
		Me.CmdHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' FraVerify
		' 
		Me.FraVerify.BackColor = System.Drawing.SystemColors.Control
		Me.FraVerify.Controls.Add(Me.TxtPassword)
		Me.FraVerify.Controls.Add(Me.Image1)
		Me.FraVerify.Controls.Add(Me.Label1)
		Me.FraVerify.Enabled = True
		Me.FraVerify.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FraVerify.ForeColor = System.Drawing.SystemColors.ControlText
		Me.FraVerify.Location = New System.Drawing.Point(8, 8)
		Me.FraVerify.Name = "FraVerify"
		Me.FraVerify.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.FraVerify.Size = New System.Drawing.Size(369, 65)
		Me.FraVerify.TabIndex = 3
		Me.FraVerify.Visible = True
		' 
		' TxtPassword
		' 
		Me.TxtPassword.AcceptsReturn = True
		Me.TxtPassword.AutoSize = False
		Me.TxtPassword.BackColor = System.Drawing.SystemColors.Window
		Me.TxtPassword.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.TxtPassword.CausesValidation = True
		Me.TxtPassword.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.TxtPassword.Enabled = True
		Me.TxtPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.TxtPassword.ForeColor = System.Drawing.SystemColors.WindowText
		Me.TxtPassword.HideSelection = True
		Me.TxtPassword.ImeMode = System.Windows.Forms.ImeMode.Disable
		Me.TxtPassword.Location = New System.Drawing.Point(160, 24)
		Me.TxtPassword.MaxLength = 10
		Me.TxtPassword.Multiline = False
		Me.TxtPassword.Name = "TxtPassword"
		Me.TxtPassword.PasswordChar = ChrW(42)
		Me.TxtPassword.ReadOnly = False
		Me.TxtPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.TxtPassword.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.TxtPassword.Size = New System.Drawing.Size(113, 19)
		Me.TxtPassword.TabIndex = 0
		Me.TxtPassword.TabStop = True
		Me.TxtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.TxtPassword.Visible = True
		' 
		' Image1
		' 
		Me.Image1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Image1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Image1.Enabled = True
		Me.Image1.Image = CType(resources.GetObject("Image1.Image"), System.Drawing.Image)
		Me.Image1.Location = New System.Drawing.Point(320, 24)
		Me.Image1.Name = "Image1"
		Me.Image1.Size = New System.Drawing.Size(32, 32)
		Me.Image1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.Image1.Visible = True
		' 
		' Label1
		' 
		Me.Label1.AutoSize = True
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.Enabled = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Location = New System.Drawing.Point(16, 24)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(109, 13)
		Me.Label1.TabIndex = 4
		Me.Label1.Text = "Please Enter Password"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' CmdCancel
		' 
		Me.CmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.CmdCancel.CausesValidation = True
		Me.CmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.CmdCancel.Enabled = True
		Me.CmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.CmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.CmdCancel.Location = New System.Drawing.Point(224, 80)
		Me.CmdCancel.Name = "CmdCancel"
		Me.CmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.CmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.CmdCancel.TabIndex = 2
		Me.CmdCancel.TabStop = True
		Me.CmdCancel.Text = "&Cancel"
		Me.CmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' CmdOK
		' 
		Me.CmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.CmdOK.CausesValidation = True
		Me.CmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.CmdOK.Enabled = False
		Me.CmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.CmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.CmdOK.Location = New System.Drawing.Point(144, 80)
		Me.CmdOK.Name = "CmdOK"
		Me.CmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.CmdOK.Size = New System.Drawing.Size(73, 22)
		Me.CmdOK.TabIndex = 1
		Me.CmdOK.TabStop = True
		Me.CmdOK.Text = "&OK"
		Me.CmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' FrmVerify
		' 
		Me.AcceptButton = Me.CmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(391, 111)
		Me.ControlBox = True
		Me.Controls.Add(Me.CmdHelp)
		Me.Controls.Add(Me.FraVerify)
		Me.Controls.Add(Me.CmdCancel)
		Me.Controls.Add(Me.CmdOK)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("FrmVerify.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "FrmVerify"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Verify Password"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.FraVerify.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class