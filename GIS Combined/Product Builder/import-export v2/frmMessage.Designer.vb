<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmMessage
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents optAllUsers As System.Windows.Forms.RadioButton
	Public WithEvents optSelectedUsers As System.Windows.Forms.RadioButton
	Public WithEvents cmdSend As System.Windows.Forms.Button
	Public WithEvents txtMessage As System.Windows.Forms.TextBox
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmMessage))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.cmdOk = New System.Windows.Forms.Button
		Me.Frame1 = New System.Windows.Forms.GroupBox
		Me.optAllUsers = New System.Windows.Forms.RadioButton
		Me.optSelectedUsers = New System.Windows.Forms.RadioButton
		Me.cmdSend = New System.Windows.Forms.Button
		Me.txtMessage = New System.Windows.Forms.TextBox
		Me.Label1 = New System.Windows.Forms.Label
		Me.Frame1.SuspendLayout()
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Text = "Message users"
		Me.ClientSize = New System.Drawing.Size(328, 236)
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ControlBox = True
		Me.Enabled = True
		Me.KeyPreview = False
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.HelpButton = False
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Name = "frmMessage"
		Me.cmdOk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOk.Text = "&Ok"
		Me.cmdOk.Size = New System.Drawing.Size(81, 25)
		Me.cmdOk.Location = New System.Drawing.Point(232, 208)
		Me.cmdOk.TabIndex = 4
		Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOk.CausesValidation = True
		Me.cmdOk.Enabled = True
		Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOk.TabStop = True
		Me.cmdOk.Name = "cmdOk"
		Me.Frame1.Size = New System.Drawing.Size(301, 197)
		Me.Frame1.Location = New System.Drawing.Point(8, 4)
		Me.Frame1.TabIndex = 0
		Me.Frame1.BackColor = System.Drawing.SystemColors.Control
		Me.Frame1.Enabled = True
		Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Frame1.Visible = True
		Me.Frame1.Padding = New System.Windows.Forms.Padding(0)
		Me.Frame1.Name = "Frame1"
		Me.optAllUsers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.optAllUsers.Text = "All users"
		Me.optAllUsers.Size = New System.Drawing.Size(137, 17)
		Me.optAllUsers.Location = New System.Drawing.Point(16, 176)
		Me.optAllUsers.TabIndex = 6
		Me.optAllUsers.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.optAllUsers.BackColor = System.Drawing.SystemColors.Control
		Me.optAllUsers.CausesValidation = True
		Me.optAllUsers.Enabled = True
		Me.optAllUsers.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optAllUsers.Cursor = System.Windows.Forms.Cursors.Default
		Me.optAllUsers.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.optAllUsers.Appearance = System.Windows.Forms.Appearance.Normal
		Me.optAllUsers.TabStop = True
		Me.optAllUsers.Checked = False
		Me.optAllUsers.Visible = True
		Me.optAllUsers.Name = "optAllUsers"
		Me.optSelectedUsers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.optSelectedUsers.Text = "Selected user"
		Me.optSelectedUsers.Size = New System.Drawing.Size(113, 17)
		Me.optSelectedUsers.Location = New System.Drawing.Point(16, 152)
		Me.optSelectedUsers.TabIndex = 5
		Me.optSelectedUsers.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.optSelectedUsers.BackColor = System.Drawing.SystemColors.Control
		Me.optSelectedUsers.CausesValidation = True
		Me.optSelectedUsers.Enabled = True
		Me.optSelectedUsers.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optSelectedUsers.Cursor = System.Windows.Forms.Cursors.Default
		Me.optSelectedUsers.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.optSelectedUsers.Appearance = System.Windows.Forms.Appearance.Normal
		Me.optSelectedUsers.TabStop = True
		Me.optSelectedUsers.Checked = False
		Me.optSelectedUsers.Visible = True
		Me.optSelectedUsers.Name = "optSelectedUsers"
		Me.cmdSend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSend.Text = "&Send"
		Me.cmdSend.Size = New System.Drawing.Size(65, 25)
		Me.cmdSend.Location = New System.Drawing.Point(224, 160)
		Me.cmdSend.TabIndex = 3
		Me.cmdSend.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSend.CausesValidation = True
		Me.cmdSend.Enabled = True
		Me.cmdSend.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSend.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSend.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSend.TabStop = True
		Me.cmdSend.Name = "cmdSend"
		Me.txtMessage.AutoSize = False
		Me.txtMessage.Size = New System.Drawing.Size(277, 113)
		Me.txtMessage.Location = New System.Drawing.Point(12, 32)
		Me.txtMessage.TabIndex = 2
		Me.txtMessage.AcceptsReturn = True
		Me.txtMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtMessage.BackColor = System.Drawing.SystemColors.Window
		Me.txtMessage.CausesValidation = True
		Me.txtMessage.Enabled = True
		Me.txtMessage.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtMessage.HideSelection = True
		Me.txtMessage.ReadOnly = False
		Me.txtMessage.Maxlength = 0
		Me.txtMessage.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtMessage.MultiLine = False
		Me.txtMessage.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtMessage.TabStop = True
		Me.txtMessage.Visible = True
		Me.txtMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtMessage.Name = "txtMessage"
		Me.Label1.Text = "Message"
		Me.Label1.Size = New System.Drawing.Size(85, 13)
		Me.Label1.Location = New System.Drawing.Point(12, 16)
		Me.Label1.TabIndex = 1
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.Enabled = True
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		Me.Label1.AutoSize = False
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Name = "Label1"
		Me.Controls.Add(cmdOk)
		Me.Controls.Add(Frame1)
		Me.Frame1.Controls.Add(optAllUsers)
		Me.Frame1.Controls.Add(optSelectedUsers)
		Me.Frame1.Controls.Add(cmdSend)
		Me.Frame1.Controls.Add(txtMessage)
		Me.Frame1.Controls.Add(Label1)
		Me.Frame1.ResumeLayout(False)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class