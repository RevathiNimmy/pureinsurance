<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        isInitializingComponent = True
        InitializeComponent()
        isInitializingComponent = False
        Form_Initialize_Renamed()
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
    Public WithEvents _staStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents staStatus As System.Windows.Forms.StatusStrip
    Public WithEvents lblVersion As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.staStatus = New System.Windows.Forms.StatusStrip()
        Me._staStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.imgLogo = New System.Windows.Forms.PictureBox()
        Me.lblPure = New System.Windows.Forms.Label()
        Me.pnlLogin = New System.Windows.Forms.Panel()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.txtUserName = New System.Windows.Forms.TextBox()
        Me.lblPassword = New System.Windows.Forms.Label()
        Me.lblUserName = New System.Windows.Forms.Label()
        Me.pnlPasswordChange = New System.Windows.Forms.Panel()
        Me.lblNewPassword = New System.Windows.Forms.Label()
        Me.txtConfirmPassword = New System.Windows.Forms.TextBox()
        Me.lblConfirmPassword = New System.Windows.Forms.Label()
        Me.cmdCancelChangePassword = New System.Windows.Forms.Button()
        Me.txtNewPassword = New System.Windows.Forms.TextBox()
        Me.cmdChangePassword = New System.Windows.Forms.Button()
        Me.txtOldPassword = New System.Windows.Forms.TextBox()
        Me.lblOldPassword = New System.Windows.Forms.Label()
        Me.staStatus.SuspendLayout()
        CType(Me.imgLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlLogin.SuspendLayout()
        Me.pnlPasswordChange.SuspendLayout()
        Me.SuspendLayout()
        '
        'staStatus
        '
        Me.staStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.staStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._staStatus_Panel1})
        Me.staStatus.Location = New System.Drawing.Point(0, 274)
        Me.staStatus.Name = "staStatus"
        Me.staStatus.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.staStatus.Size = New System.Drawing.Size(605, 22)
        Me.staStatus.TabIndex = 11
        '
        '_staStatus_Panel1
        '
        Me._staStatus_Panel1.AutoSize = False
        Me._staStatus_Panel1.BackColor = System.Drawing.SystemColors.Control
        Me._staStatus_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._staStatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._staStatus_Panel1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._staStatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._staStatus_Panel1.Name = "_staStatus_Panel1"
        Me._staStatus_Panel1.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always
        Me._staStatus_Panel1.Size = New System.Drawing.Size(590, 22)
        Me._staStatus_Panel1.Spring = True
        Me._staStatus_Panel1.Tag = ""
        Me._staStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblVersion
        '
        Me.lblVersion.BackColor = System.Drawing.SystemColors.Window
        Me.lblVersion.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblVersion.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVersion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblVersion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblVersion.Location = New System.Drawing.Point(12, 244)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblVersion.Size = New System.Drawing.Size(285, 17)
        Me.lblVersion.TabIndex = 13
        Me.lblVersion.Text = "Client Software Version Unavailable"
        Me.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'imgLogo
        '
        Me.imgLogo.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgLogo.Image = CType(resources.GetObject("imgLogo.Image"), System.Drawing.Image)
        Me.imgLogo.Location = New System.Drawing.Point(-18, -28)
        Me.imgLogo.Name = "imgLogo"
        Me.imgLogo.Size = New System.Drawing.Size(367, 249)
        Me.imgLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.imgLogo.TabIndex = 13
        Me.imgLogo.TabStop = False
        '
        'lblPure
        '
        Me.lblPure.AutoSize = True
        Me.lblPure.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPure.Location = New System.Drawing.Point(5, 199)
        Me.lblPure.Name = "lblPure"
        Me.lblPure.Size = New System.Drawing.Size(241, 45)
        Me.lblPure.TabIndex = 14
        Me.lblPure.Text = "Pure Insurance"
        '
        'pnlLogin
        '
        Me.pnlLogin.Controls.Add(Me.cmdCancel)
        Me.pnlLogin.Controls.Add(Me.txtPassword)
        Me.pnlLogin.Controls.Add(Me.cmdOK)
        Me.pnlLogin.Controls.Add(Me.txtUserName)
        Me.pnlLogin.Controls.Add(Me.lblPassword)
        Me.pnlLogin.Controls.Add(Me.lblUserName)
        Me.pnlLogin.Location = New System.Drawing.Point(364, 12)
        Me.pnlLogin.Name = "pnlLogin"
        Me.pnlLogin.Size = New System.Drawing.Size(229, 259)
        Me.pnlLogin.TabIndex = 15
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(140, 224)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 25)
        Me.cmdCancel.TabIndex = 12
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'txtPassword
        '
        Me.txtPassword.AcceptsReturn = True
        Me.txtPassword.BackColor = System.Drawing.SystemColors.Window
        Me.txtPassword.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPassword.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPassword.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPassword.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.txtPassword.Location = New System.Drawing.Point(16, 154)
        Me.txtPassword.MaxLength = 15
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPassword.Size = New System.Drawing.Size(197, 23)
        Me.txtPassword.TabIndex = 10
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Enabled = False
        Me.cmdOK.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(61, 224)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 25)
        Me.cmdOK.TabIndex = 11
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'txtUserName
        '
        Me.txtUserName.AcceptsReturn = True
        Me.txtUserName.BackColor = System.Drawing.SystemColors.Window
        Me.txtUserName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtUserName.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUserName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtUserName.Location = New System.Drawing.Point(16, 81)
        Me.txtUserName.MaxLength = 255
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtUserName.Size = New System.Drawing.Size(197, 23)
        Me.txtUserName.TabIndex = 9
        '
        'lblPassword
        '
        Me.lblPassword.BackColor = System.Drawing.SystemColors.Window
        Me.lblPassword.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPassword.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPassword.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPassword.Location = New System.Drawing.Point(13, 133)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPassword.Size = New System.Drawing.Size(62, 17)
        Me.lblPassword.TabIndex = 14
        Me.lblPassword.Text = "Password:"
        '
        'lblUserName
        '
        Me.lblUserName.BackColor = System.Drawing.SystemColors.Window
        Me.lblUserName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUserName.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUserName.Location = New System.Drawing.Point(13, 60)
        Me.lblUserName.Name = "lblUserName"
        Me.lblUserName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUserName.Size = New System.Drawing.Size(70, 18)
        Me.lblUserName.TabIndex = 13
        Me.lblUserName.Text = "User Name:"
        '
        'pnlPasswordChange
        '
        Me.pnlPasswordChange.Controls.Add(Me.lblNewPassword)
        Me.pnlPasswordChange.Controls.Add(Me.txtConfirmPassword)
        Me.pnlPasswordChange.Controls.Add(Me.lblConfirmPassword)
        Me.pnlPasswordChange.Controls.Add(Me.cmdCancelChangePassword)
        Me.pnlPasswordChange.Controls.Add(Me.txtNewPassword)
        Me.pnlPasswordChange.Controls.Add(Me.cmdChangePassword)
        Me.pnlPasswordChange.Controls.Add(Me.txtOldPassword)
        Me.pnlPasswordChange.Controls.Add(Me.lblOldPassword)
        Me.pnlPasswordChange.Location = New System.Drawing.Point(364, 12)
        Me.pnlPasswordChange.Name = "pnlPasswordChange"
        Me.pnlPasswordChange.Size = New System.Drawing.Size(229, 259)
        Me.pnlPasswordChange.TabIndex = 15
        Me.pnlPasswordChange.Visible = False
        '
        'lblNewPassword
        '
        Me.lblNewPassword.BackColor = System.Drawing.SystemColors.Window
        Me.lblNewPassword.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNewPassword.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNewPassword.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNewPassword.Location = New System.Drawing.Point(13, 86)
        Me.lblNewPassword.Name = "lblNewPassword"
        Me.lblNewPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNewPassword.Size = New System.Drawing.Size(121, 17)
        Me.lblNewPassword.TabIndex = 20
        Me.lblNewPassword.Text = "New Password:"
        '
        'txtConfirmPassword
        '
        Me.txtConfirmPassword.AcceptsReturn = True
        Me.txtConfirmPassword.BackColor = System.Drawing.SystemColors.Window
        Me.txtConfirmPassword.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtConfirmPassword.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtConfirmPassword.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtConfirmPassword.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.txtConfirmPassword.Location = New System.Drawing.Point(13, 170)
        Me.txtConfirmPassword.MaxLength = 255
        Me.txtConfirmPassword.Name = "txtConfirmPassword"
        Me.txtConfirmPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtConfirmPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtConfirmPassword.Size = New System.Drawing.Size(197, 23)
        Me.txtConfirmPassword.TabIndex = 21
        '
        'lblConfirmPassword
        '
        Me.lblConfirmPassword.AutoSize = True
        Me.lblConfirmPassword.BackColor = System.Drawing.SystemColors.Window
        Me.lblConfirmPassword.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblConfirmPassword.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConfirmPassword.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblConfirmPassword.Location = New System.Drawing.Point(13, 150)
        Me.lblConfirmPassword.Name = "lblConfirmPassword"
        Me.lblConfirmPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblConfirmPassword.Size = New System.Drawing.Size(134, 15)
        Me.lblConfirmPassword.TabIndex = 22
        Me.lblConfirmPassword.Text = "Confirm New Password:"
        '
        'cmdCancelChangePassword
        '
        Me.cmdCancelChangePassword.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancelChangePassword.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancelChangePassword.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancelChangePassword.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancelChangePassword.Location = New System.Drawing.Point(140, 221)
        Me.cmdCancelChangePassword.Name = "cmdCancelChangePassword"
        Me.cmdCancelChangePassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancelChangePassword.Size = New System.Drawing.Size(73, 25)
        Me.cmdCancelChangePassword.TabIndex = 21
        Me.cmdCancelChangePassword.Text = "&Cancel"
        Me.cmdCancelChangePassword.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancelChangePassword.UseVisualStyleBackColor = False
        '
        'txtNewPassword
        '
        Me.txtNewPassword.AcceptsReturn = True
        Me.txtNewPassword.BackColor = System.Drawing.SystemColors.Window
        Me.txtNewPassword.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNewPassword.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNewPassword.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNewPassword.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.txtNewPassword.Location = New System.Drawing.Point(13, 105)
        Me.txtNewPassword.MaxLength = 255
        Me.txtNewPassword.Name = "txtNewPassword"
        Me.txtNewPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtNewPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNewPassword.Size = New System.Drawing.Size(197, 23)
        Me.txtNewPassword.TabIndex = 16
        '
        'cmdChangePassword
        '
        Me.cmdChangePassword.BackColor = System.Drawing.SystemColors.Control
        Me.cmdChangePassword.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdChangePassword.Enabled = False
        Me.cmdChangePassword.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdChangePassword.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdChangePassword.Location = New System.Drawing.Point(61, 221)
        Me.cmdChangePassword.Name = "cmdChangePassword"
        Me.cmdChangePassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdChangePassword.Size = New System.Drawing.Size(73, 25)
        Me.cmdChangePassword.TabIndex = 22
        Me.cmdChangePassword.Text = "&OK"
        Me.cmdChangePassword.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdChangePassword.UseVisualStyleBackColor = False
        '
        'txtOldPassword
        '
        Me.txtOldPassword.AcceptsReturn = True
        Me.txtOldPassword.BackColor = System.Drawing.SystemColors.Window
        Me.txtOldPassword.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOldPassword.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOldPassword.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOldPassword.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.txtOldPassword.Location = New System.Drawing.Point(13, 40)
        Me.txtOldPassword.MaxLength = 255
        Me.txtOldPassword.Name = "txtOldPassword"
        Me.txtOldPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtOldPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOldPassword.Size = New System.Drawing.Size(197, 23)
        Me.txtOldPassword.TabIndex = 15
        '
        'lblOldPassword
        '
        Me.lblOldPassword.AutoSize = True
        Me.lblOldPassword.BackColor = System.Drawing.SystemColors.Window
        Me.lblOldPassword.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOldPassword.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOldPassword.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOldPassword.Location = New System.Drawing.Point(13, 19)
        Me.lblOldPassword.Name = "lblOldPassword"
        Me.lblOldPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOldPassword.Size = New System.Drawing.Size(82, 15)
        Me.lblOldPassword.TabIndex = 19
        Me.lblOldPassword.Text = "Old Password:"
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(605, 296)
        Me.Controls.Add(Me.pnlPasswordChange)
        Me.Controls.Add(Me.staStatus)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.lblPure)
        Me.Controls.Add(Me.imgLogo)
        Me.Controls.Add(Me.pnlLogin)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(133, 195)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Pure Insurance Logon"
        Me.staStatus.ResumeLayout(False)
        Me.staStatus.PerformLayout()
        CType(Me.imgLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlLogin.ResumeLayout(False)
        Me.pnlLogin.PerformLayout()
        Me.pnlPasswordChange.ResumeLayout(False)
        Me.pnlPasswordChange.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents imgLogo As System.Windows.Forms.PictureBox
    Friend WithEvents lblPure As System.Windows.Forms.Label
    Friend WithEvents pnlLogin As System.Windows.Forms.Panel
    Friend WithEvents pnlPasswordChange As System.Windows.Forms.Panel
    Public WithEvents txtConfirmPassword As System.Windows.Forms.TextBox
    Public WithEvents lblConfirmPassword As System.Windows.Forms.Label
    Public WithEvents cmdCancelChangePassword As System.Windows.Forms.Button
    Public WithEvents txtNewPassword As System.Windows.Forms.TextBox
    Public WithEvents cmdChangePassword As System.Windows.Forms.Button
    Public WithEvents txtOldPassword As System.Windows.Forms.TextBox
    Public WithEvents lblNewPassword As System.Windows.Forms.Label
    Public WithEvents lblOldPassword As System.Windows.Forms.Label
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents txtPassword As System.Windows.Forms.TextBox
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents txtUserName As System.Windows.Forms.TextBox
    Public WithEvents lblPassword As System.Windows.Forms.Label
    Public WithEvents lblUserName As System.Windows.Forms.Label
#End Region
End Class