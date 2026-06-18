<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPassword
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
	Public WithEvents txtRePassword As System.Windows.Forms.TextBox
	Public WithEvents Panel3D1 As System.Windows.Forms.Panel
	Public WithEvents fra3RePassBox As System.Windows.Forms.GroupBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents txtPassword As System.Windows.Forms.TextBox
	Public WithEvents Panel3D2 As System.Windows.Forms.Panel
	Public WithEvents fra3Password As System.Windows.Forms.GroupBox
	Public WithEvents pan3FormBox As System.Windows.Forms.Panel
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPassword))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.fra3RePassBox = New System.Windows.Forms.GroupBox
		Me.Panel3D1 = New System.Windows.Forms.Panel
		Me.txtRePassword = New System.Windows.Forms.TextBox
		Me.pan3FormBox = New System.Windows.Forms.Panel
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.fra3Password = New System.Windows.Forms.GroupBox
		Me.Panel3D2 = New System.Windows.Forms.Panel
		Me.txtPassword = New System.Windows.Forms.TextBox
		Me.fra3RePassBox.SuspendLayout()
		Me.Panel3D1.SuspendLayout()
		Me.pan3FormBox.SuspendLayout()
		Me.fra3Password.SuspendLayout()
		Me.Panel3D2.SuspendLayout()
		Me.SuspendLayout()
		' 
		' fra3RePassBox
		' 
		Me.fra3RePassBox.Controls.Add(Me.Panel3D1)
		Me.fra3RePassBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        'Me.fra3RePassBox.Font3D = Threed.enumFont3DConstants._InsetLight
        Me.fra3RePassBox.Location = New System.Drawing.Point(20, 68)
		Me.fra3RePassBox.Name = "fra3RePassBox"
		Me.fra3RePassBox.Size = New System.Drawing.Size(225, 49)
		Me.fra3RePassBox.TabIndex = 7
		Me.fra3RePassBox.Text = "Re-Enter Password"
		' 
		' Panel3D1
		' 
		Me.Panel3D1.BackColor = System.Drawing.SystemColors.Control
		Me.Panel3D1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.Panel3D1.Controls.Add(Me.txtRePassword)
		Me.Panel3D1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Panel3D1.Location = New System.Drawing.Point(8, 16)
		Me.Panel3D1.Name = "Panel3D1"
		Me.Panel3D1.Size = New System.Drawing.Size(209, 25)
		Me.Panel3D1.TabIndex = 8
		' 
		' txtRePassword
		' 
		Me.txtRePassword.AcceptsReturn = True
		Me.txtRePassword.AutoSize = False
		Me.txtRePassword.BackColor = System.Drawing.SystemColors.Window
		Me.txtRePassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.txtRePassword.CausesValidation = True
		Me.txtRePassword.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtRePassword.Enabled = True
		Me.txtRePassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtRePassword.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtRePassword.HideSelection = True
		Me.txtRePassword.ImeMode = System.Windows.Forms.ImeMode.Disable
		Me.txtRePassword.Location = New System.Drawing.Point(1, 1)
		Me.txtRePassword.MaxLength = 10
		Me.txtRePassword.Multiline = False
		Me.txtRePassword.Name = "txtRePassword"
		Me.txtRePassword.PasswordChar = ChrW(42)
		Me.txtRePassword.ReadOnly = False
		Me.txtRePassword.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtRePassword.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtRePassword.Size = New System.Drawing.Size(207, 23)
		Me.txtRePassword.TabIndex = 1
		Me.txtRePassword.TabStop = True
		Me.txtRePassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtRePassword.Visible = True
		' 
		' pan3FormBox
		' 
		Me.pan3FormBox.BackColor = System.Drawing.SystemColors.Control
		Me.pan3FormBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.pan3FormBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.pan3FormBox.Controls.Add(Me.cmdCancel)
		Me.pan3FormBox.Controls.Add(Me.cmdOK)
		Me.pan3FormBox.Controls.Add(Me.fra3Password)
		Me.pan3FormBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3FormBox.Location = New System.Drawing.Point(0, 0)
		Me.pan3FormBox.Name = "pan3FormBox"
		Me.pan3FormBox.Size = New System.Drawing.Size(357, 133)
		Me.pan3FormBox.TabIndex = 4
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(260, 40)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(81, 25)
		Me.cmdCancel.TabIndex = 3
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
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(260, 12)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(81, 25)
		Me.cmdOK.TabIndex = 2
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' fra3Password
		' 
		Me.fra3Password.Controls.Add(Me.Panel3D2)
		Me.fra3Password.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        'Me.fra3Password.Font3D = Threed.enumFont3DConstants._InsetLight
		Me.fra3Password.Location = New System.Drawing.Point(20, 8)
		Me.fra3Password.Name = "fra3Password"
		Me.fra3Password.Size = New System.Drawing.Size(225, 49)
		Me.fra3Password.TabIndex = 5
		Me.fra3Password.Text = "Enter Password"
		' 
		' Panel3D2
		' 
		Me.Panel3D2.BackColor = System.Drawing.SystemColors.Control
		Me.Panel3D2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.Panel3D2.Controls.Add(Me.txtPassword)
		Me.Panel3D2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Panel3D2.Location = New System.Drawing.Point(8, 16)
		Me.Panel3D2.Name = "Panel3D2"
		Me.Panel3D2.Size = New System.Drawing.Size(209, 25)
		Me.Panel3D2.TabIndex = 6
		' 
		' txtPassword
		' 
		Me.txtPassword.AcceptsReturn = True
		Me.txtPassword.AutoSize = False
		Me.txtPassword.BackColor = System.Drawing.SystemColors.Window
		Me.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.txtPassword.CausesValidation = True
		Me.txtPassword.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPassword.Enabled = True
		Me.txtPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPassword.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPassword.HideSelection = True
		Me.txtPassword.ImeMode = System.Windows.Forms.ImeMode.Disable
		Me.txtPassword.Location = New System.Drawing.Point(1, 1)
		Me.txtPassword.MaxLength = 10
		Me.txtPassword.Multiline = False
		Me.txtPassword.Name = "txtPassword"
		Me.txtPassword.PasswordChar = ChrW(42)
		Me.txtPassword.ReadOnly = False
		Me.txtPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPassword.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPassword.Size = New System.Drawing.Size(207, 23)
		Me.txtPassword.TabIndex = 0
		Me.txtPassword.TabStop = True
		Me.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPassword.Visible = True
		' 
		' frmPassword
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
		Me.BackColor = System.Drawing.SystemColors.Window
		Me.ClientSize = New System.Drawing.Size(357, 133)
		Me.ControlBox = False
		Me.Controls.Add(Me.fra3RePassBox)
		Me.Controls.Add(Me.pan3FormBox)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.ForeColor = System.Drawing.SystemColors.WindowText
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(97, 188)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmPassword"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Password"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.fra3RePassBox.ResumeLayout(False)
		Me.Panel3D1.ResumeLayout(False)
		Me.pan3FormBox.ResumeLayout(False)
		Me.fra3Password.ResumeLayout(False)
		Me.Panel3D2.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class