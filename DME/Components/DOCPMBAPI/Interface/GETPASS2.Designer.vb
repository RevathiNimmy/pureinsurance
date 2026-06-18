<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGetPassword
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
    Public WithEvents pan3Title As System.Windows.Forms.Panel
    Public WithEvents lbl3Title As System.Windows.Forms.Label
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents txtPassword As System.Windows.Forms.TextBox
	Public WithEvents pan3Password As System.Windows.Forms.Panel
	Public WithEvents fra3GetPassword As System.Windows.Forms.GroupBox
	Public WithEvents pan3GetPassword As System.Windows.Forms.Panel
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmGetPassword))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.pan3GetPassword = New System.Windows.Forms.Panel
		Me.pan3Title = New System.Windows.Forms.Panel
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.fra3GetPassword = New System.Windows.Forms.GroupBox
		Me.pan3Password = New System.Windows.Forms.Panel
		Me.txtPassword = New System.Windows.Forms.TextBox
		Me.pan3GetPassword.SuspendLayout()
		Me.fra3GetPassword.SuspendLayout()
		Me.pan3Password.SuspendLayout()
		Me.SuspendLayout()
		' 
		' pan3GetPassword
		' 
		Me.pan3GetPassword.BackColor = System.Drawing.SystemColors.Control
		Me.pan3GetPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.pan3GetPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.pan3GetPassword.Controls.Add(Me.pan3Title)
		Me.pan3GetPassword.Controls.Add(Me.cmdCancel)
		Me.pan3GetPassword.Controls.Add(Me.cmdOK)
		Me.pan3GetPassword.Controls.Add(Me.fra3GetPassword)
		Me.pan3GetPassword.Dock = System.Windows.Forms.DockStyle.Top
		Me.pan3GetPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3GetPassword.Location = New System.Drawing.Point(0, 0)
		Me.pan3GetPassword.Name = "pan3GetPassword"
		Me.pan3GetPassword.Size = New System.Drawing.Size(316, 113)
		Me.pan3GetPassword.TabIndex = 0
		' 
		' pan3Title
		' 
		Me.pan3Title.BackColor = System.Drawing.SystemColors.Control
		Me.pan3Title.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.pan3Title.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3Title.Location = New System.Drawing.Point(16, 13)
		Me.pan3Title.Name = "pan3Title"
		Me.pan3Title.Size = New System.Drawing.Size(281, 18)
        Me.pan3Title.TabIndex = 6
        Me.pan3Title.Controls.Add(lbl3Title)
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(232, 72)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(65, 25)
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
		Me.cmdOK.Enabled = False
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(232, 40)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(65, 25)
		Me.cmdOK.TabIndex = 2
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' fra3GetPassword
		' 
		Me.fra3GetPassword.Controls.Add(Me.pan3Password)
		Me.fra3GetPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        'Me.fra3GetPassword.Font3D = Threed.enumFont3DConstants._InsetLight
		Me.fra3GetPassword.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0)
		Me.fra3GetPassword.Location = New System.Drawing.Point(16, 36)
		Me.fra3GetPassword.Name = "fra3GetPassword"
		Me.fra3GetPassword.Size = New System.Drawing.Size(201, 61)
		Me.fra3GetPassword.TabIndex = 4
		Me.fra3GetPassword.Text = "Enter Password"
		' 
		' pan3Password
		' 
		Me.pan3Password.BackColor = System.Drawing.SystemColors.Control
		Me.pan3Password.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.pan3Password.Controls.Add(Me.txtPassword)
		Me.pan3Password.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3Password.Location = New System.Drawing.Point(16, 24)
		Me.pan3Password.Name = "pan3Password"
		Me.pan3Password.Size = New System.Drawing.Size(169, 23)
		Me.pan3Password.TabIndex = 5
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
		Me.txtPassword.Size = New System.Drawing.Size(167, 21)
		Me.txtPassword.TabIndex = 1
		Me.txtPassword.TabStop = True
		Me.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPassword.Visible = True
		' 
		' frmGetPassword
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
		Me.BackColor = System.Drawing.SystemColors.Window
		Me.ClientSize = New System.Drawing.Size(316, 113)
		Me.ControlBox = False
		Me.Controls.Add(Me.pan3GetPassword)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.ForeColor = System.Drawing.SystemColors.WindowText
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(185, 180)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmGetPassword"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Password Required"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.pan3GetPassword.ResumeLayout(False)
		Me.fra3GetPassword.ResumeLayout(False)
		Me.pan3Password.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class