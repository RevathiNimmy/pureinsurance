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
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents cmdSend As System.Windows.Forms.Button
	Public WithEvents optAllUsers As System.Windows.Forms.RadioButton
	Public WithEvents optSelectedUsers As System.Windows.Forms.RadioButton
	Public WithEvents txtMessage As System.Windows.Forms.TextBox
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMessage))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdOk = New System.Windows.Forms.Button
		Me.Frame1 = New System.Windows.Forms.GroupBox
		Me.cmdSend = New System.Windows.Forms.Button
		Me.optAllUsers = New System.Windows.Forms.RadioButton
		Me.optSelectedUsers = New System.Windows.Forms.RadioButton
		Me.txtMessage = New System.Windows.Forms.TextBox
		Me.Label1 = New System.Windows.Forms.Label
		Me.Frame1.SuspendLayout()
		Me.SuspendLayout()
		' 
		' cmdOk
		' 
		Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOk.CausesValidation = True
		Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOk.Enabled = True
		Me.cmdOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOk.Location = New System.Drawing.Point(236, 208)
		Me.cmdOk.Name = "cmdOk"
		Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOk.Size = New System.Drawing.Size(73, 22)
		Me.cmdOk.TabIndex = 4
		Me.cmdOk.TabStop = True
		Me.cmdOk.Text = "&Ok"
		Me.cmdOk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' Frame1
		' 
		Me.Frame1.BackColor = System.Drawing.SystemColors.Control
		Me.Frame1.Controls.Add(Me.cmdSend)
		Me.Frame1.Controls.Add(Me.optAllUsers)
		Me.Frame1.Controls.Add(Me.optSelectedUsers)
		Me.Frame1.Controls.Add(Me.txtMessage)
		Me.Frame1.Controls.Add(Me.Label1)
		Me.Frame1.Enabled = True
		Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Frame1.Location = New System.Drawing.Point(8, 4)
		Me.Frame1.Name = "Frame1"
		Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Frame1.Size = New System.Drawing.Size(301, 197)
		Me.Frame1.TabIndex = 0
		Me.Frame1.Visible = True
		' 
		' cmdSend
		' 
		Me.cmdSend.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSend.CausesValidation = True
		Me.cmdSend.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSend.Enabled = True
		Me.cmdSend.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdSend.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSend.Location = New System.Drawing.Point(216, 164)
		Me.cmdSend.Name = "cmdSend"
		Me.cmdSend.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSend.Size = New System.Drawing.Size(73, 22)
		Me.cmdSend.TabIndex = 5
		Me.cmdSend.TabStop = True
		Me.cmdSend.Text = "&Send"
		Me.cmdSend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSend.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' optAllUsers
		' 
		Me.optAllUsers.Appearance = System.Windows.Forms.Appearance.Normal
		Me.optAllUsers.BackColor = System.Drawing.SystemColors.Control
		Me.optAllUsers.CausesValidation = True
		Me.optAllUsers.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.optAllUsers.Checked = False
		Me.optAllUsers.Cursor = System.Windows.Forms.Cursors.Default
		Me.optAllUsers.Enabled = True
		Me.optAllUsers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.optAllUsers.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optAllUsers.Location = New System.Drawing.Point(12, 172)
		Me.optAllUsers.Name = "optAllUsers"
		Me.optAllUsers.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.optAllUsers.Size = New System.Drawing.Size(77, 13)
		Me.optAllUsers.TabIndex = 3
		Me.optAllUsers.TabStop = True
		Me.optAllUsers.Text = "All users"
		Me.optAllUsers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.optAllUsers.Visible = True
		' 
		' optSelectedUsers
		' 
		Me.optSelectedUsers.Appearance = System.Windows.Forms.Appearance.Normal
		Me.optSelectedUsers.BackColor = System.Drawing.SystemColors.Control
		Me.optSelectedUsers.CausesValidation = True
		Me.optSelectedUsers.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.optSelectedUsers.Checked = False
		Me.optSelectedUsers.Cursor = System.Windows.Forms.Cursors.Default
		Me.optSelectedUsers.Enabled = True
		Me.optSelectedUsers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.optSelectedUsers.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optSelectedUsers.Location = New System.Drawing.Point(12, 152)
		Me.optSelectedUsers.Name = "optSelectedUsers"
		Me.optSelectedUsers.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.optSelectedUsers.Size = New System.Drawing.Size(93, 13)
		Me.optSelectedUsers.TabIndex = 2
		Me.optSelectedUsers.TabStop = True
		Me.optSelectedUsers.Text = "Selected user"
		Me.optSelectedUsers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.optSelectedUsers.Visible = True
		' 
		' txtMessage
		' 
		Me.txtMessage.AcceptsReturn = True
		Me.txtMessage.AutoSize = False
		Me.txtMessage.BackColor = System.Drawing.SystemColors.Window
		Me.txtMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtMessage.CausesValidation = True
		Me.txtMessage.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtMessage.Enabled = True
		Me.txtMessage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtMessage.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtMessage.HideSelection = True
		Me.txtMessage.Location = New System.Drawing.Point(12, 32)
		Me.txtMessage.MaxLength = 0
		Me.txtMessage.Multiline = False
		Me.txtMessage.Name = "txtMessage"
		Me.txtMessage.ReadOnly = False
		Me.txtMessage.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtMessage.Size = New System.Drawing.Size(277, 113)
		Me.txtMessage.TabIndex = 1
		Me.txtMessage.TabStop = True
		Me.txtMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtMessage.Visible = True
		' 
		' Label1
		' 
		Me.Label1.AutoSize = False
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.Enabled = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Location = New System.Drawing.Point(12, 16)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(85, 13)
		Me.Label1.TabIndex = 6
		Me.Label1.Text = "Message:"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' frmMessage
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(315, 236)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdOk)
		Me.Controls.Add(Me.Frame1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmMessage.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmMessage"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Message users"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Frame1.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class