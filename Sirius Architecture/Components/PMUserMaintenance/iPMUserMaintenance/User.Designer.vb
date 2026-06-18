<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUser
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
	Public WithEvents txtEffectiveDate As System.Windows.Forms.TextBox
	Public WithEvents txtUsername As System.Windows.Forms.TextBox
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents lblConfirmPassword As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUser))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.Frame1 = New System.Windows.Forms.GroupBox
		Me.txtEffectiveDate = New System.Windows.Forms.TextBox
		Me.txtUsername = New System.Windows.Forms.TextBox
		Me.Label1 = New System.Windows.Forms.Label
		Me.lblConfirmPassword = New System.Windows.Forms.Label
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.Frame1.SuspendLayout()
		Me.SuspendLayout()
		' 
		' Frame1
		' 
		Me.Frame1.BackColor = System.Drawing.SystemColors.Control
		Me.Frame1.Controls.Add(Me.txtEffectiveDate)
		Me.Frame1.Controls.Add(Me.txtUsername)
		Me.Frame1.Controls.Add(Me.Label1)
		Me.Frame1.Controls.Add(Me.lblConfirmPassword)
		Me.Frame1.Enabled = True
		Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Frame1.Location = New System.Drawing.Point(8, 4)
		Me.Frame1.Name = "Frame1"
		Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Frame1.Size = New System.Drawing.Size(289, 93)
		Me.Frame1.TabIndex = 2
		Me.Frame1.Visible = True
		' 
		' txtEffectiveDate
		' 
		Me.txtEffectiveDate.AcceptsReturn = True
		Me.txtEffectiveDate.AutoSize = False
		Me.txtEffectiveDate.BackColor = System.Drawing.SystemColors.Window
		Me.txtEffectiveDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtEffectiveDate.CausesValidation = True
		Me.txtEffectiveDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtEffectiveDate.Enabled = True
		Me.txtEffectiveDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtEffectiveDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtEffectiveDate.HideSelection = True
		Me.txtEffectiveDate.ImeMode = System.Windows.Forms.ImeMode.Disable
		Me.txtEffectiveDate.Location = New System.Drawing.Point(137, 56)
		Me.txtEffectiveDate.MaxLength = 30
		Me.txtEffectiveDate.Multiline = False
		Me.txtEffectiveDate.Name = "txtEffectiveDate"
		Me.txtEffectiveDate.ReadOnly = False
		Me.txtEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtEffectiveDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtEffectiveDate.Size = New System.Drawing.Size(136, 21)
		Me.txtEffectiveDate.TabIndex = 4
		Me.txtEffectiveDate.TabStop = True
		Me.txtEffectiveDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtEffectiveDate.Visible = True
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
		Me.txtUsername.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtUsername.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtUsername.HideSelection = True
		Me.txtUsername.ImeMode = System.Windows.Forms.ImeMode.Disable
		Me.txtUsername.Location = New System.Drawing.Point(136, 23)
		Me.txtUsername.MaxLength = 30
		Me.txtUsername.Multiline = False
		Me.txtUsername.Name = "txtUsername"
		Me.txtUsername.ReadOnly = False
		Me.txtUsername.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtUsername.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtUsername.Size = New System.Drawing.Size(136, 21)
		Me.txtUsername.TabIndex = 3
		Me.txtUsername.TabStop = True
		Me.txtUsername.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtUsername.Visible = True
		' 
		' Label1
		' 
		Me.Label1.AutoSize = False
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.Enabled = True
		Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Location = New System.Drawing.Point(16, 58)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(121, 17)
		Me.Label1.TabIndex = 6
		Me.Label1.Text = "Effective Date:"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' lblConfirmPassword
		' 
		Me.lblConfirmPassword.AutoSize = False
		Me.lblConfirmPassword.BackColor = System.Drawing.SystemColors.Control
		Me.lblConfirmPassword.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblConfirmPassword.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblConfirmPassword.Enabled = True
		Me.lblConfirmPassword.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblConfirmPassword.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblConfirmPassword.Location = New System.Drawing.Point(16, 25)
		Me.lblConfirmPassword.Name = "lblConfirmPassword"
		Me.lblConfirmPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblConfirmPassword.Size = New System.Drawing.Size(97, 17)
		Me.lblConfirmPassword.TabIndex = 5
		Me.lblConfirmPassword.Text = "User Name:"
		Me.lblConfirmPassword.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblConfirmPassword.UseMnemonic = True
		Me.lblConfirmPassword.Visible = True
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(226, 105)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 1
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(146, 105)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 0
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' frmUser
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(306, 136)
		Me.ControlBox = False
		Me.Controls.Add(Me.Frame1)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmUser.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(269, 146)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmUser"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Add New User"
		Me.Visible = False
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ToolTip1.SetToolTip(Me.txtEffectiveDate, "Effective Date")
		Me.ToolTip1.SetToolTip(Me.txtUsername, "User Name")
		Me.ToolTip1.SetToolTip(Me.cmdCancel, "Cancel changes and return to previous screen")
		Me.ToolTip1.SetToolTip(Me.cmdOK, "Accept Changes and return to previous screen")
		Me.Frame1.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class