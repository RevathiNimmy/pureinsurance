<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
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
	Public WithEvents TxtpassVer As System.Windows.Forms.TextBox
	Public WithEvents TxtPass As System.Windows.Forms.TextBox
	Public WithEvents Image1 As System.Windows.Forms.PictureBox
	Public WithEvents LblVerify As System.Windows.Forms.Label
	Public WithEvents LblPass As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents CmdOK2 As System.Windows.Forms.Button
	Public WithEvents CmdCancel As System.Windows.Forms.Button
	Public WithEvents CmdOK As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.CmdHelp = New System.Windows.Forms.Button
		Me.Frame1 = New System.Windows.Forms.GroupBox
		Me.TxtpassVer = New System.Windows.Forms.TextBox
		Me.TxtPass = New System.Windows.Forms.TextBox
		Me.Image1 = New System.Windows.Forms.PictureBox
		Me.LblVerify = New System.Windows.Forms.Label
		Me.LblPass = New System.Windows.Forms.Label
		Me.CmdOK2 = New System.Windows.Forms.Button
		Me.CmdCancel = New System.Windows.Forms.Button
		Me.CmdOK = New System.Windows.Forms.Button
		Me.Frame1.SuspendLayout()
		Me.SuspendLayout()
		' 
		' CmdHelp
		' 
		Me.CmdHelp.BackColor = System.Drawing.SystemColors.Control
		Me.CmdHelp.CausesValidation = True
		Me.CmdHelp.Cursor = System.Windows.Forms.Cursors.Default
		Me.CmdHelp.Enabled = True
		Me.CmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.CmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.CmdHelp.Location = New System.Drawing.Point(304, 72)
		Me.CmdHelp.Name = "CmdHelp"
		Me.CmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.CmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.CmdHelp.TabIndex = 8
		Me.CmdHelp.TabStop = True
		Me.CmdHelp.Text = "&Help"
		Me.CmdHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' Frame1
		' 
		Me.Frame1.BackColor = System.Drawing.SystemColors.Control
		Me.Frame1.Controls.Add(Me.TxtpassVer)
		Me.Frame1.Controls.Add(Me.TxtPass)
		Me.Frame1.Controls.Add(Me.Image1)
		Me.Frame1.Controls.Add(Me.LblVerify)
		Me.Frame1.Controls.Add(Me.LblPass)
		Me.Frame1.Enabled = True
		Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Frame1.Location = New System.Drawing.Point(8, 8)
		Me.Frame1.Name = "Frame1"
		Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Frame1.Size = New System.Drawing.Size(369, 57)
		Me.Frame1.TabIndex = 4
		Me.Frame1.Text = "Set Password"
		Me.Frame1.Visible = True
		' 
		' TxtpassVer
		' 
		Me.TxtpassVer.AcceptsReturn = True
		Me.TxtpassVer.AutoSize = False
		Me.TxtpassVer.BackColor = System.Drawing.SystemColors.Window
		Me.TxtpassVer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.TxtpassVer.CausesValidation = True
		Me.TxtpassVer.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.TxtpassVer.Enabled = True
		Me.TxtpassVer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.TxtpassVer.ForeColor = System.Drawing.SystemColors.WindowText
		Me.TxtpassVer.HideSelection = True
		Me.TxtpassVer.ImeMode = System.Windows.Forms.ImeMode.Disable
		Me.TxtpassVer.Location = New System.Drawing.Point(152, 64)
		Me.TxtpassVer.MaxLength = 10
		Me.TxtpassVer.Multiline = False
		Me.TxtpassVer.Name = "TxtpassVer"
		Me.TxtpassVer.PasswordChar = ChrW(42)
		Me.TxtpassVer.ReadOnly = False
		Me.TxtpassVer.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.TxtpassVer.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.TxtpassVer.Size = New System.Drawing.Size(113, 19)
		Me.TxtpassVer.TabIndex = 7
		Me.TxtpassVer.TabStop = True
		Me.TxtpassVer.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.TxtpassVer.Visible = True
		' 
		' TxtPass
		' 
		Me.TxtPass.AcceptsReturn = True
		Me.TxtPass.AutoSize = False
		Me.TxtPass.BackColor = System.Drawing.SystemColors.Window
		Me.TxtPass.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.TxtPass.CausesValidation = True
		Me.TxtPass.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.TxtPass.Enabled = True
		Me.TxtPass.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.TxtPass.ForeColor = System.Drawing.SystemColors.WindowText
		Me.TxtPass.HideSelection = True
		Me.TxtPass.ImeMode = System.Windows.Forms.ImeMode.Disable
		Me.TxtPass.Location = New System.Drawing.Point(152, 24)
		Me.TxtPass.MaxLength = 10
		Me.TxtPass.Multiline = False
		Me.TxtPass.Name = "TxtPass"
		Me.TxtPass.PasswordChar = ChrW(42)
		Me.TxtPass.ReadOnly = False
		Me.TxtPass.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.TxtPass.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.TxtPass.Size = New System.Drawing.Size(113, 19)
		Me.TxtPass.TabIndex = 0
		Me.TxtPass.TabStop = True
		Me.TxtPass.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.TxtPass.Visible = True
		' 
		' Image1
		' 
		Me.Image1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Image1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Image1.Enabled = True
		Me.Image1.Image = CType(resources.GetObject("Image1.Image"), System.Drawing.Image)
		Me.Image1.Location = New System.Drawing.Point(312, 16)
		Me.Image1.Name = "Image1"
		Me.Image1.Size = New System.Drawing.Size(32, 32)
		Me.Image1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.Image1.Visible = True
		' 
		' LblVerify
		' 
		Me.LblVerify.AutoSize = False
		Me.LblVerify.BackColor = System.Drawing.SystemColors.Control
		Me.LblVerify.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.LblVerify.Cursor = System.Windows.Forms.Cursors.Default
		Me.LblVerify.Enabled = True
		Me.LblVerify.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.LblVerify.ForeColor = System.Drawing.SystemColors.ControlText
		Me.LblVerify.Location = New System.Drawing.Point(24, 64)
		Me.LblVerify.Name = "LblVerify"
		Me.LblVerify.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.LblVerify.Size = New System.Drawing.Size(113, 17)
		Me.LblVerify.TabIndex = 6
		Me.LblVerify.Text = "Verify password:-"
		Me.LblVerify.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.LblVerify.UseMnemonic = True
		Me.LblVerify.Visible = True
		' 
		' LblPass
		' 
		Me.LblPass.AutoSize = False
		Me.LblPass.BackColor = System.Drawing.SystemColors.Control
		Me.LblPass.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.LblPass.Cursor = System.Windows.Forms.Cursors.Default
		Me.LblPass.Enabled = True
		Me.LblPass.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.LblPass.ForeColor = System.Drawing.SystemColors.ControlText
		Me.LblPass.Location = New System.Drawing.Point(24, 24)
		Me.LblPass.Name = "LblPass"
		Me.LblPass.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.LblPass.Size = New System.Drawing.Size(121, 25)
		Me.LblPass.TabIndex = 5
		Me.LblPass.Text = "Please Enter Password"
		Me.LblPass.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.LblPass.UseMnemonic = True
		Me.LblPass.Visible = True
		' 
		' CmdOK2
		' 
		Me.CmdOK2.BackColor = System.Drawing.SystemColors.Control
		Me.CmdOK2.CausesValidation = True
		Me.CmdOK2.Cursor = System.Windows.Forms.Cursors.Default
		Me.CmdOK2.Enabled = False
		Me.CmdOK2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.CmdOK2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.CmdOK2.Location = New System.Drawing.Point(144, 72)
		Me.CmdOK2.Name = "CmdOK2"
		Me.CmdOK2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.CmdOK2.Size = New System.Drawing.Size(73, 22)
		Me.CmdOK2.TabIndex = 1
		Me.CmdOK2.TabStop = True
		Me.CmdOK2.Text = "&OK"
		Me.CmdOK2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CmdOK2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.CmdOK2.Visible = False
		' 
		' CmdCancel
		' 
		Me.CmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.CmdCancel.CausesValidation = True
		Me.CmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.CmdCancel.Enabled = True
		Me.CmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.CmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.CmdCancel.Location = New System.Drawing.Point(224, 72)
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
		Me.CmdOK.Enabled = True
		Me.CmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.CmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.CmdOK.Location = New System.Drawing.Point(144, 72)
		Me.CmdOK.Name = "CmdOK"
		Me.CmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.CmdOK.Size = New System.Drawing.Size(73, 22)
		Me.CmdOK.TabIndex = 3
		Me.CmdOK.TabStop = True
		Me.CmdOK.Text = "&OK"
		Me.CmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' frmInterface
		' 
		Me.AcceptButton = Me.CmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(7, 16)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(396, 101)
		Me.ControlBox = True
		Me.Controls.Add(Me.CmdHelp)
		Me.Controls.Add(Me.Frame1)
		Me.Controls.Add(Me.CmdOK2)
		Me.Controls.Add(Me.CmdCancel)
		Me.Controls.Add(Me.CmdOK)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Bookman Old Style", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmInterface.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 24)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Password"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Frame1.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class