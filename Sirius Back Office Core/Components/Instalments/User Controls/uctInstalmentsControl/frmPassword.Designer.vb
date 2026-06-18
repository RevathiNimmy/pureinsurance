<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPassword
#Region "Windows Form Designer generated code "
	Friend Sub New()
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
	Public WithEvents txtBrokerID As System.Windows.Forms.TextBox
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents txtPassword As System.Windows.Forms.TextBox
	Public WithEvents txtUsername As System.Windows.Forms.TextBox
	Public WithEvents Label5 As System.Windows.Forms.Label
	Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Image1 As System.Windows.Forms.PictureBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPassword))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.txtBrokerID = New System.Windows.Forms.TextBox
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.txtPassword = New System.Windows.Forms.TextBox
		Me.txtUsername = New System.Windows.Forms.TextBox
		Me.Label5 = New System.Windows.Forms.Label
		Me.Label4 = New System.Windows.Forms.Label
		Me.Label3 = New System.Windows.Forms.Label
		Me.Label2 = New System.Windows.Forms.Label
		Me.Label1 = New System.Windows.Forms.Label
		Me.Image1 = New System.Windows.Forms.PictureBox
		Me.SuspendLayout()
		' 
		' txtBrokerID
		' 
		Me.txtBrokerID.AcceptsReturn = True
		Me.txtBrokerID.AutoSize = False
		Me.txtBrokerID.BackColor = System.Drawing.SystemColors.Window
		Me.txtBrokerID.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtBrokerID.CausesValidation = True
		Me.txtBrokerID.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtBrokerID.Enabled = True
		Me.txtBrokerID.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtBrokerID.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtBrokerID.HideSelection = True
		Me.txtBrokerID.Location = New System.Drawing.Point(128, 64)
		Me.txtBrokerID.MaxLength = 0
		Me.txtBrokerID.Multiline = False
		Me.txtBrokerID.Name = "txtBrokerID"
		Me.txtBrokerID.ReadOnly = False
		Me.txtBrokerID.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtBrokerID.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtBrokerID.Size = New System.Drawing.Size(209, 19)
		Me.txtBrokerID.TabIndex = 1
		Me.txtBrokerID.TabStop = True
		Me.txtBrokerID.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtBrokerID.Visible = True
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(168, 144)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(89, 25)
		Me.cmdOK.TabIndex = 8
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(264, 144)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(89, 25)
		Me.cmdCancel.TabIndex = 7
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtPassword
		' 
		Me.txtPassword.AcceptsReturn = True
		Me.txtPassword.AutoSize = False
		Me.txtPassword.BackColor = System.Drawing.SystemColors.Window
		Me.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPassword.CausesValidation = True
		Me.txtPassword.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPassword.Enabled = True
		Me.txtPassword.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPassword.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPassword.HideSelection = True
		Me.txtPassword.ImeMode = System.Windows.Forms.ImeMode.Disable
		Me.txtPassword.Location = New System.Drawing.Point(128, 112)
		Me.txtPassword.MaxLength = 0
		Me.txtPassword.Multiline = False
		Me.txtPassword.Name = "txtPassword"
		Me.txtPassword.PasswordChar = ChrW(42)
		Me.txtPassword.ReadOnly = False
		Me.txtPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPassword.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPassword.Size = New System.Drawing.Size(209, 19)
		Me.txtPassword.TabIndex = 3
		Me.txtPassword.TabStop = True
		Me.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPassword.Visible = True
		' 
		' txtUsername
		' 
		Me.txtUsername.AcceptsReturn = True
		Me.txtUsername.AutoSize = False
		Me.txtUsername.BackColor = System.Drawing.SystemColors.Window
		Me.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtUsername.CausesValidation = True
		Me.txtUsername.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtUsername.Enabled = True
		Me.txtUsername.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtUsername.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtUsername.HideSelection = True
		Me.txtUsername.Location = New System.Drawing.Point(128, 88)
		Me.txtUsername.MaxLength = 0
		Me.txtUsername.Multiline = False
		Me.txtUsername.Name = "txtUsername"
		Me.txtUsername.ReadOnly = False
		Me.txtUsername.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtUsername.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtUsername.Size = New System.Drawing.Size(209, 19)
		Me.txtUsername.TabIndex = 2
		Me.txtUsername.TabStop = True
		Me.txtUsername.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtUsername.Visible = True
		' 
		' Label5
		' 
		Me.Label5.AutoSize = True
		Me.Label5.BackColor = System.Drawing.Color.Transparent
		Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label5.Enabled = True
		Me.Label5.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label5.Location = New System.Drawing.Point(64, 65)
		Me.Label5.Name = "Label5"
		Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label5.Size = New System.Drawing.Size(49, 13)
		Me.Label5.TabIndex = 9
		Me.Label5.Text = "Broker ID:"
		Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label5.UseMnemonic = True
		Me.Label5.Visible = True
		' 
		' Label4
		' 
		Me.Label4.AutoSize = True
		Me.Label4.BackColor = System.Drawing.Color.Transparent
		Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label4.Enabled = True
		Me.Label4.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label4.Location = New System.Drawing.Point(64, 113)
		Me.Label4.Name = "Label4"
		Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label4.Size = New System.Drawing.Size(50, 13)
		Me.Label4.TabIndex = 6
		Me.Label4.Text = "Password:"
		Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label4.UseMnemonic = True
		Me.Label4.Visible = True
		' 
		' Label3
		' 
		Me.Label3.AutoSize = True
		Me.Label3.BackColor = System.Drawing.Color.Transparent
		Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label3.Enabled = True
		Me.Label3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label3.Location = New System.Drawing.Point(64, 89)
		Me.Label3.Name = "Label3"
		Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label3.Size = New System.Drawing.Size(52, 13)
		Me.Label3.TabIndex = 5
		Me.Label3.Text = "Username:"
		Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label3.UseMnemonic = True
		Me.Label3.Visible = True
		' 
		' Label2
		' 
		Me.Label2.AutoSize = True
		Me.Label2.BackColor = System.Drawing.Color.Transparent
		Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label2.Enabled = True
		Me.Label2.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label2.Location = New System.Drawing.Point(64, 16)
		Me.Label2.Name = "Label2"
		Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label2.Size = New System.Drawing.Size(190, 16)
		Me.Label2.TabIndex = 4
		Me.Label2.Text = "Premium finance connectivity"
		Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label2.UseMnemonic = True
		Me.Label2.Visible = True
		' 
		' Label1
		' 
		Me.Label1.AutoSize = True
		Me.Label1.BackColor = System.Drawing.Color.Transparent
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.Enabled = True
		Me.Label1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Location = New System.Drawing.Point(64, 32)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(200, 13)
		Me.Label1.TabIndex = 0
		Me.Label1.Text = "Please enter your logon credentials below"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' Image1
		' 
		Me.Image1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Image1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Image1.Enabled = True
		Me.Image1.Image = CType(resources.GetObject("Image1.Image"), System.Drawing.Image)
		Me.Image1.Location = New System.Drawing.Point(16, 16)
		Me.Image1.Name = "Image1"
		Me.Image1.Size = New System.Drawing.Size(32, 32)
		Me.Image1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.Image1.Visible = True
		' 
		' frmPassword
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(364, 179)
		Me.ControlBox = False
		Me.Controls.Add(Me.txtBrokerID)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.txtPassword)
		Me.Controls.Add(Me.txtUsername)
		Me.Controls.Add(Me.Label5)
		Me.Controls.Add(Me.Label4)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.Image1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 25)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmPassword"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Logon"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class