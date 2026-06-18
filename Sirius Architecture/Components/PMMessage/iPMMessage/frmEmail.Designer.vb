<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEmail
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializepicLog()
		InitializelblLog()
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
	Public WithEvents txtLogNumber As System.Windows.Forms.TextBox
	Public WithEvents chkUpdate As System.Windows.Forms.CheckBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents txtRcpt As System.Windows.Forms.TextBox
	Public WithEvents cmdSend As System.Windows.Forms.Button
	Private WithEvents _picLog_3 As System.Windows.Forms.PictureBox
	Private WithEvents _picLog_2 As System.Windows.Forms.PictureBox
	Private WithEvents _picLog_1 As System.Windows.Forms.PictureBox
	Private WithEvents _picLog_0 As System.Windows.Forms.PictureBox
	Private WithEvents _lblLog_3 As System.Windows.Forms.Label
	Private WithEvents _lblLog_0 As System.Windows.Forms.Label
	Private WithEvents _lblLog_2 As System.Windows.Forms.Label
	Private WithEvents _lblLog_1 As System.Windows.Forms.Label
	Public WithEvents picAttachments As System.Windows.Forms.PictureBox
	Public WithEvents txtMessage As System.Windows.Forms.TextBox
	Public WithEvents txtSubject As System.Windows.Forms.TextBox
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents lblMessage As System.Windows.Forms.Label
	Public WithEvents lblAttachments As System.Windows.Forms.Label
	Public WithEvents lblSubject As System.Windows.Forms.Label
	Public lblLog(3) As System.Windows.Forms.Label
	Public picLog(3) As System.Windows.Forms.PictureBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEmail))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.txtLogNumber = New System.Windows.Forms.TextBox
		Me.chkUpdate = New System.Windows.Forms.CheckBox
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.txtRcpt = New System.Windows.Forms.TextBox
		Me.cmdSend = New System.Windows.Forms.Button
		Me.picAttachments = New System.Windows.Forms.PictureBox
		Me._picLog_3 = New System.Windows.Forms.PictureBox
		Me._picLog_2 = New System.Windows.Forms.PictureBox
		Me._picLog_1 = New System.Windows.Forms.PictureBox
		Me._picLog_0 = New System.Windows.Forms.PictureBox
		Me._lblLog_3 = New System.Windows.Forms.Label
		Me._lblLog_0 = New System.Windows.Forms.Label
		Me._lblLog_2 = New System.Windows.Forms.Label
		Me._lblLog_1 = New System.Windows.Forms.Label
		Me.txtMessage = New System.Windows.Forms.TextBox
		Me.txtSubject = New System.Windows.Forms.TextBox
		Me.Label2 = New System.Windows.Forms.Label
		Me.Label1 = New System.Windows.Forms.Label
		Me.lblMessage = New System.Windows.Forms.Label
		Me.lblAttachments = New System.Windows.Forms.Label
		Me.lblSubject = New System.Windows.Forms.Label
		Me.picAttachments.SuspendLayout()
		Me.SuspendLayout()
		' 
		' txtLogNumber
		' 
		Me.txtLogNumber.AcceptsReturn = True
		Me.txtLogNumber.AutoSize = False
		Me.txtLogNumber.BackColor = System.Drawing.SystemColors.InactiveCaptionText
		Me.txtLogNumber.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtLogNumber.CausesValidation = True
		Me.txtLogNumber.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtLogNumber.Enabled = True
		Me.txtLogNumber.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtLogNumber.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtLogNumber.HideSelection = True
		Me.txtLogNumber.Location = New System.Drawing.Point(168, 60)
		Me.txtLogNumber.MaxLength = 0
		Me.txtLogNumber.Multiline = False
		Me.txtLogNumber.Name = "txtLogNumber"
		Me.txtLogNumber.ReadOnly = False
		Me.txtLogNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtLogNumber.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtLogNumber.Size = New System.Drawing.Size(129, 19)
		Me.txtLogNumber.TabIndex = 3
		Me.txtLogNumber.TabStop = True
		Me.txtLogNumber.Text = "<LogNumber>"
		Me.txtLogNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtLogNumber.Visible = True
		' 
		' chkUpdate
		' 
		Me.chkUpdate.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkUpdate.BackColor = System.Drawing.SystemColors.Control
		Me.chkUpdate.CausesValidation = True
		Me.chkUpdate.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkUpdate.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkUpdate.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkUpdate.Enabled = True
		Me.chkUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkUpdate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkUpdate.Location = New System.Drawing.Point(140, 64)
		Me.chkUpdate.Name = "chkUpdate"
		Me.chkUpdate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkUpdate.Size = New System.Drawing.Size(21, 13)
		Me.chkUpdate.TabIndex = 2
		Me.chkUpdate.TabStop = True
		Me.chkUpdate.Text = ""
		Me.chkUpdate.Visible = True
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(288, 404)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 6
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtRcpt
		' 
		Me.txtRcpt.AcceptsReturn = True
		Me.txtRcpt.AutoSize = False
		Me.txtRcpt.BackColor = System.Drawing.SystemColors.Window
		Me.txtRcpt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtRcpt.CausesValidation = True
		Me.txtRcpt.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtRcpt.Enabled = True
		Me.txtRcpt.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtRcpt.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtRcpt.HideSelection = True
		Me.txtRcpt.Location = New System.Drawing.Point(56, 8)
		Me.txtRcpt.MaxLength = 0
		Me.txtRcpt.Multiline = False
		Me.txtRcpt.Name = "txtRcpt"
		Me.txtRcpt.ReadOnly = False
		Me.txtRcpt.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtRcpt.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtRcpt.Size = New System.Drawing.Size(385, 19)
		Me.txtRcpt.TabIndex = 0
		Me.txtRcpt.TabStop = True
		Me.txtRcpt.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtRcpt.Visible = True
		' 
		' cmdSend
		' 
		Me.cmdSend.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSend.CausesValidation = True
		Me.cmdSend.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSend.Enabled = True
		Me.cmdSend.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdSend.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSend.Location = New System.Drawing.Point(368, 404)
		Me.cmdSend.Name = "cmdSend"
		Me.cmdSend.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSend.Size = New System.Drawing.Size(73, 22)
		Me.cmdSend.TabIndex = 5
		Me.cmdSend.TabStop = True
		Me.cmdSend.Text = "&Send"
		Me.cmdSend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSend.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' picAttachments
		' 
		Me.picAttachments.BackColor = System.Drawing.Color.White
		Me.picAttachments.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.picAttachments.CausesValidation = True
		Me.picAttachments.Controls.Add(Me._picLog_3)
		Me.picAttachments.Controls.Add(Me._picLog_2)
		Me.picAttachments.Controls.Add(Me._picLog_1)
		Me.picAttachments.Controls.Add(Me._picLog_0)
		Me.picAttachments.Controls.Add(Me._lblLog_3)
		Me.picAttachments.Controls.Add(Me._lblLog_0)
		Me.picAttachments.Controls.Add(Me._lblLog_2)
		Me.picAttachments.Controls.Add(Me._lblLog_1)
		Me.picAttachments.Cursor = System.Windows.Forms.Cursors.Default
		Me.picAttachments.Dock = System.Windows.Forms.DockStyle.None
		Me.picAttachments.Enabled = True
		Me.picAttachments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.picAttachments.Location = New System.Drawing.Point(8, 324)
		Me.picAttachments.Name = "picAttachments"
		Me.picAttachments.Size = New System.Drawing.Size(433, 73)
		Me.picAttachments.TabIndex = 10
		Me.picAttachments.TabStop = True
		Me.picAttachments.Visible = True
		' 
		' _picLog_3
		' 
		Me._picLog_3.BackColor = System.Drawing.Color.White
		Me._picLog_3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._picLog_3.CausesValidation = True
		Me._picLog_3.Cursor = System.Windows.Forms.Cursors.Default
		Me._picLog_3.Dock = System.Windows.Forms.DockStyle.None
		Me._picLog_3.Enabled = True
		Me._picLog_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._picLog_3.Image = CType(resources.GetObject("_picLog_3.Image"), System.Drawing.Image)
		Me._picLog_3.Location = New System.Drawing.Point(296, 8)
		Me._picLog_3.Name = "_picLog_3"
		Me._picLog_3.Size = New System.Drawing.Size(32, 32)
		Me._picLog_3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
		Me._picLog_3.TabIndex = 18
		Me._picLog_3.TabStop = True
		Me._picLog_3.Visible = True
		' 
		' _picLog_2
		' 
		Me._picLog_2.BackColor = System.Drawing.Color.White
		Me._picLog_2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._picLog_2.CausesValidation = True
		Me._picLog_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._picLog_2.Dock = System.Windows.Forms.DockStyle.None
		Me._picLog_2.Enabled = True
		Me._picLog_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._picLog_2.Image = CType(resources.GetObject("_picLog_2.Image"), System.Drawing.Image)
		Me._picLog_2.Location = New System.Drawing.Point(208, 8)
		Me._picLog_2.Name = "_picLog_2"
		Me._picLog_2.Size = New System.Drawing.Size(32, 32)
		Me._picLog_2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
		Me._picLog_2.TabIndex = 17
		Me._picLog_2.TabStop = True
		Me._picLog_2.Visible = True
		' 
		' _picLog_1
		' 
		Me._picLog_1.BackColor = System.Drawing.SystemColors.ControlLight
		Me._picLog_1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._picLog_1.CausesValidation = True
		Me._picLog_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._picLog_1.Dock = System.Windows.Forms.DockStyle.None
		Me._picLog_1.Enabled = True
		Me._picLog_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._picLog_1.Image = CType(resources.GetObject("_picLog_1.Image"), System.Drawing.Image)
		Me._picLog_1.Location = New System.Drawing.Point(112, 8)
		Me._picLog_1.Name = "_picLog_1"
		Me._picLog_1.Size = New System.Drawing.Size(32, 32)
		Me._picLog_1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
		Me._picLog_1.TabIndex = 16
		Me._picLog_1.TabStop = True
		Me._picLog_1.Visible = True
		' 
		' _picLog_0
		' 
		Me._picLog_0.BackColor = System.Drawing.SystemColors.ControlLight
		Me._picLog_0.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._picLog_0.CausesValidation = True
		Me._picLog_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._picLog_0.Dock = System.Windows.Forms.DockStyle.None
		Me._picLog_0.Enabled = True
		Me._picLog_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._picLog_0.Image = CType(resources.GetObject("_picLog_0.Image"), System.Drawing.Image)
		Me._picLog_0.Location = New System.Drawing.Point(24, 8)
		Me._picLog_0.Name = "_picLog_0"
		Me._picLog_0.Size = New System.Drawing.Size(32, 32)
		Me._picLog_0.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
		Me._picLog_0.TabIndex = 15
		Me._picLog_0.TabStop = True
		Me._picLog_0.Visible = True
		' 
		' _lblLog_3
		' 
		Me._lblLog_3.AutoSize = True
		Me._lblLog_3.BackColor = System.Drawing.Color.White
		Me._lblLog_3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLog_3.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLog_3.Enabled = True
		Me._lblLog_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._lblLog_3.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLog_3.Location = New System.Drawing.Point(293, 48)
		Me._lblLog_3.Name = "_lblLog_3"
		Me._lblLog_3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLog_3.Size = New System.Drawing.Size(38, 13)
		Me._lblLog_3.TabIndex = 19
		Me._lblLog_3.Text = "Registry"
		Me._lblLog_3.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLog_3.UseMnemonic = True
		Me._lblLog_3.Visible = True
		' 
		' _lblLog_0
		' 
		Me._lblLog_0.AutoSize = True
		Me._lblLog_0.BackColor = System.Drawing.Color.White
		Me._lblLog_0.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLog_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLog_0.Enabled = True
		Me._lblLog_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._lblLog_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLog_0.Location = New System.Drawing.Point(16, 48)
		Me._lblLog_0.Name = "_lblLog_0"
		Me._lblLog_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLog_0.Size = New System.Drawing.Size(47, 13)
		Me._lblLog_0.TabIndex = 13
		Me._lblLog_0.Text = "Client Log"
		Me._lblLog_0.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLog_0.UseMnemonic = True
		Me._lblLog_0.Visible = True
		' 
		' _lblLog_2
		' 
		Me._lblLog_2.AutoSize = True
		Me._lblLog_2.BackColor = System.Drawing.Color.White
		Me._lblLog_2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLog_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLog_2.Enabled = True
		Me._lblLog_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._lblLog_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLog_2.Location = New System.Drawing.Point(200, 48)
		Me._lblLog_2.Name = "_lblLog_2"
		Me._lblLog_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLog_2.Size = New System.Drawing.Size(48, 13)
		Me._lblLog_2.TabIndex = 11
		Me._lblLog_2.Text = "Cobol Log"
		Me._lblLog_2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLog_2.UseMnemonic = True
		Me._lblLog_2.Visible = True
		' 
		' _lblLog_1
		' 
		Me._lblLog_1.AutoSize = True
		Me._lblLog_1.BackColor = System.Drawing.Color.White
		Me._lblLog_1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLog_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLog_1.Enabled = True
		Me._lblLog_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._lblLog_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLog_1.Location = New System.Drawing.Point(104, 48)
		Me._lblLog_1.Name = "_lblLog_1"
		Me._lblLog_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLog_1.Size = New System.Drawing.Size(52, 13)
		Me._lblLog_1.TabIndex = 7
		Me._lblLog_1.Text = "Server Log"
		Me._lblLog_1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLog_1.UseMnemonic = True
		Me._lblLog_1.Visible = True
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
		Me.txtMessage.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtMessage.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtMessage.HideSelection = True
		Me.txtMessage.Location = New System.Drawing.Point(8, 108)
		Me.txtMessage.MaxLength = 0
		Me.txtMessage.Multiline = True
		Me.txtMessage.Name = "txtMessage"
		Me.txtMessage.ReadOnly = False
		Me.txtMessage.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtMessage.Size = New System.Drawing.Size(433, 193)
		Me.txtMessage.TabIndex = 4
		Me.txtMessage.TabStop = True
		Me.txtMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtMessage.Visible = True
		' 
		' txtSubject
		' 
		Me.txtSubject.AcceptsReturn = True
		Me.txtSubject.AutoSize = False
		Me.txtSubject.BackColor = System.Drawing.SystemColors.Window
		Me.txtSubject.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtSubject.CausesValidation = True
		Me.txtSubject.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtSubject.Enabled = True
		Me.txtSubject.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtSubject.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtSubject.HideSelection = True
		Me.txtSubject.Location = New System.Drawing.Point(56, 32)
		Me.txtSubject.MaxLength = 0
		Me.txtSubject.Multiline = False
		Me.txtSubject.Name = "txtSubject"
		Me.txtSubject.ReadOnly = False
		Me.txtSubject.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtSubject.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtSubject.Size = New System.Drawing.Size(385, 19)
		Me.txtSubject.TabIndex = 1
		Me.txtSubject.TabStop = True
		Me.txtSubject.Text = "An error has occurred in Sirius"
		Me.txtSubject.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtSubject.Visible = True
		' 
		' Label2
		' 
		Me.Label2.AutoSize = True
		Me.Label2.BackColor = System.Drawing.SystemColors.Control
		Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label2.Enabled = True
		Me.Label2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label2.Location = New System.Drawing.Point(8, 64)
		Me.Label2.Name = "Label2"
		Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label2.Size = New System.Drawing.Size(117, 13)
		Me.Label2.TabIndex = 20
		Me.Label2.Text = "Update to existing fault:"
		Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label2.UseMnemonic = True
		Me.Label2.Visible = True
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
		Me.Label1.Location = New System.Drawing.Point(8, 11)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(16, 13)
		Me.Label1.TabIndex = 14
		Me.Label1.Text = "To:"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' lblMessage
		' 
		Me.lblMessage.AutoSize = True
		Me.lblMessage.BackColor = System.Drawing.SystemColors.Control
		Me.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMessage.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMessage.Enabled = True
		Me.lblMessage.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMessage.Location = New System.Drawing.Point(8, 92)
		Me.lblMessage.Name = "lblMessage"
		Me.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMessage.Size = New System.Drawing.Size(297, 13)
		Me.lblMessage.TabIndex = 12
		Me.lblMessage.Text = "Please enter any extra information you think might be useful :"
		Me.lblMessage.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMessage.UseMnemonic = True
		Me.lblMessage.Visible = True
		' 
		' lblAttachments
		' 
		Me.lblAttachments.AutoSize = True
		Me.lblAttachments.BackColor = System.Drawing.SystemColors.Control
		Me.lblAttachments.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAttachments.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAttachments.Enabled = True
		Me.lblAttachments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAttachments.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAttachments.Location = New System.Drawing.Point(8, 308)
		Me.lblAttachments.Name = "lblAttachments"
		Me.lblAttachments.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAttachments.Size = New System.Drawing.Size(62, 13)
		Me.lblAttachments.TabIndex = 9
		Me.lblAttachments.Text = "Attachments:"
		Me.lblAttachments.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAttachments.UseMnemonic = True
		Me.lblAttachments.Visible = True
		' 
		' lblSubject
		' 
		Me.lblSubject.AutoSize = True
		Me.lblSubject.BackColor = System.Drawing.SystemColors.Control
		Me.lblSubject.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSubject.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSubject.Enabled = True
		Me.lblSubject.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSubject.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSubject.Location = New System.Drawing.Point(8, 36)
		Me.lblSubject.Name = "lblSubject"
		Me.lblSubject.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSubject.Size = New System.Drawing.Size(40, 13)
		Me.lblSubject.TabIndex = 8
		Me.lblSubject.Text = "Subject:"
		Me.lblSubject.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblSubject.UseMnemonic = True
		Me.lblSubject.Visible = True
		' 
		' frmEmail
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(451, 435)
		Me.ControlBox = True
		Me.Controls.Add(Me.txtLogNumber)
		Me.Controls.Add(Me.chkUpdate)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.txtRcpt)
		Me.Controls.Add(Me.cmdSend)
		Me.Controls.Add(Me.picAttachments)
		Me.Controls.Add(Me.txtMessage)
		Me.Controls.Add(Me.txtSubject)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.lblMessage)
		Me.Controls.Add(Me.lblAttachments)
		Me.Controls.Add(Me.lblSubject)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmEmail.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmEmail"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Send Support Email"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.picAttachments.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub InitializepicLog()
		Me.picLog(3) = _picLog_3
		Me.picLog(2) = _picLog_2
		Me.picLog(1) = _picLog_1
		Me.picLog(0) = _picLog_0
	End Sub
	Sub InitializelblLog()
		Me.lblLog(3) = _lblLog_3
		Me.lblLog(0) = _lblLog_0
		Me.lblLog(2) = _lblLog_2
		Me.lblLog(1) = _lblLog_1
	End Sub
#End Region 
End Class