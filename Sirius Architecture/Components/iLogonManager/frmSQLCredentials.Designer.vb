<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSQLCredentials
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSQLCredentials))
        Me.imgLogo = New System.Windows.Forms.PictureBox()
        Me.lblPure = New System.Windows.Forms.Label()
        Me.pnlSQLCredentials = New System.Windows.Forms.Panel()
        Me.txtDataBaseServer = New System.Windows.Forms.TextBox()
        Me.optSQLServerAuthentication = New System.Windows.Forms.RadioButton()
        Me.optWindowsAuthentication = New System.Windows.Forms.RadioButton()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtDataBase = New System.Windows.Forms.TextBox()
        Me.lblLoginId = New System.Windows.Forms.Label()
        Me.txtSQLPassword = New System.Windows.Forms.TextBox()
        Me.lblPassword = New System.Windows.Forms.Label()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.txtSQLloginId = New System.Windows.Forms.TextBox()
        Me.cmdOk = New System.Windows.Forms.Button()
        Me.lblConnect = New System.Windows.Forms.Label()
        Me.lblVersion = New System.Windows.Forms.Label()
        CType(Me.imgLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlSQLCredentials.SuspendLayout()
        Me.SuspendLayout()
        '
        'imgLogo
        '
        Me.imgLogo.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgLogo.Image = CType(resources.GetObject("imgLogo.Image"), System.Drawing.Image)
        Me.imgLogo.Location = New System.Drawing.Point(4, 0)
        Me.imgLogo.Name = "imgLogo"
        Me.imgLogo.Size = New System.Drawing.Size(367, 249)
        Me.imgLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.imgLogo.TabIndex = 14
        Me.imgLogo.TabStop = False
        '
        'lblPure
        '
        Me.lblPure.AutoSize = True
        Me.lblPure.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPure.Location = New System.Drawing.Point(8, 252)
        Me.lblPure.Name = "lblPure"
        Me.lblPure.Size = New System.Drawing.Size(241, 45)
        Me.lblPure.TabIndex = 15
        Me.lblPure.Text = "Pure Insurance"
        '
        'pnlSQLCredentials
        '
        Me.pnlSQLCredentials.Controls.Add(Me.txtDataBaseServer)
        Me.pnlSQLCredentials.Controls.Add(Me.optSQLServerAuthentication)
        Me.pnlSQLCredentials.Controls.Add(Me.optWindowsAuthentication)
        Me.pnlSQLCredentials.Controls.Add(Me.Label2)
        Me.pnlSQLCredentials.Controls.Add(Me.Label1)
        Me.pnlSQLCredentials.Controls.Add(Me.txtDataBase)
        Me.pnlSQLCredentials.Controls.Add(Me.lblLoginId)
        Me.pnlSQLCredentials.Controls.Add(Me.txtSQLPassword)
        Me.pnlSQLCredentials.Controls.Add(Me.lblPassword)
        Me.pnlSQLCredentials.Controls.Add(Me.cmdCancel)
        Me.pnlSQLCredentials.Controls.Add(Me.txtSQLloginId)
        Me.pnlSQLCredentials.Controls.Add(Me.cmdOk)
        Me.pnlSQLCredentials.Controls.Add(Me.lblConnect)
        Me.pnlSQLCredentials.Location = New System.Drawing.Point(414, 0)
        Me.pnlSQLCredentials.Name = "pnlSQLCredentials"
        Me.pnlSQLCredentials.Size = New System.Drawing.Size(240, 327)
        Me.pnlSQLCredentials.TabIndex = 16
        '
        'txtDataBaseServer
        '
        Me.txtDataBaseServer.AcceptsReturn = True
        Me.txtDataBaseServer.BackColor = System.Drawing.SystemColors.Window
        Me.txtDataBaseServer.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDataBaseServer.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDataBaseServer.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDataBaseServer.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.txtDataBaseServer.Location = New System.Drawing.Point(16, 35)
        Me.txtDataBaseServer.MaxLength = 255
        Me.txtDataBaseServer.Name = "txtDataBaseServer"
        Me.txtDataBaseServer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDataBaseServer.Size = New System.Drawing.Size(197, 23)
        Me.txtDataBaseServer.TabIndex = 29
        '
        'optSQLServerAuthentication
        '
        Me.optSQLServerAuthentication.AutoSize = True
        Me.optSQLServerAuthentication.Location = New System.Drawing.Point(23, 166)
        Me.optSQLServerAuthentication.Name = "optSQLServerAuthentication"
        Me.optSQLServerAuthentication.Size = New System.Drawing.Size(151, 17)
        Me.optSQLServerAuthentication.TabIndex = 28
        Me.optSQLServerAuthentication.TabStop = True
        Me.optSQLServerAuthentication.Text = "SQL Server Authentication"
        Me.optSQLServerAuthentication.UseVisualStyleBackColor = True
        '
        'optWindowsAuthentication
        '
        Me.optWindowsAuthentication.AutoSize = True
        Me.optWindowsAuthentication.Location = New System.Drawing.Point(23, 143)
        Me.optWindowsAuthentication.Name = "optWindowsAuthentication"
        Me.optWindowsAuthentication.Size = New System.Drawing.Size(140, 17)
        Me.optWindowsAuthentication.TabIndex = 27
        Me.optWindowsAuthentication.TabStop = True
        Me.optWindowsAuthentication.Text = "Windows Authentication"
        Me.optWindowsAuthentication.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.SystemColors.Window
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(16, 61)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(55, 15)
        Me.Label2.TabIndex = 26
        Me.Label2.Text = "Database"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Window
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(16, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(90, 15)
        Me.Label1.TabIndex = 25
        Me.Label1.Text = "Database Server"
        '
        'txtDataBase
        '
        Me.txtDataBase.AcceptsReturn = True
        Me.txtDataBase.BackColor = System.Drawing.SystemColors.Window
        Me.txtDataBase.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDataBase.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDataBase.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDataBase.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.txtDataBase.Location = New System.Drawing.Point(16, 82)
        Me.txtDataBase.MaxLength = 255
        Me.txtDataBase.Name = "txtDataBase"
        Me.txtDataBase.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDataBase.Size = New System.Drawing.Size(197, 23)
        Me.txtDataBase.TabIndex = 24
        '
        'lblLoginId
        '
        Me.lblLoginId.BackColor = System.Drawing.SystemColors.Window
        Me.lblLoginId.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLoginId.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLoginId.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLoginId.Location = New System.Drawing.Point(16, 198)
        Me.lblLoginId.Name = "lblLoginId"
        Me.lblLoginId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLoginId.Size = New System.Drawing.Size(121, 17)
        Me.lblLoginId.TabIndex = 20
        Me.lblLoginId.Text = "Login ID:"
        '
        'txtSQLPassword
        '
        Me.txtSQLPassword.AcceptsReturn = True
        Me.txtSQLPassword.BackColor = System.Drawing.SystemColors.Window
        Me.txtSQLPassword.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSQLPassword.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSQLPassword.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSQLPassword.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.txtSQLPassword.Location = New System.Drawing.Point(16, 267)
        Me.txtSQLPassword.MaxLength = 255
        Me.txtSQLPassword.Name = "txtSQLPassword"
        Me.txtSQLPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtSQLPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSQLPassword.Size = New System.Drawing.Size(197, 23)
        Me.txtSQLPassword.TabIndex = 21
        '
        'lblPassword
        '
        Me.lblPassword.AutoSize = True
        Me.lblPassword.BackColor = System.Drawing.SystemColors.Window
        Me.lblPassword.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPassword.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPassword.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPassword.Location = New System.Drawing.Point(16, 244)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPassword.Size = New System.Drawing.Size(60, 15)
        Me.lblPassword.TabIndex = 22
        Me.lblPassword.Text = "Password:"
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(130, 299)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 25)
        Me.cmdCancel.TabIndex = 21
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'txtSQLloginId
        '
        Me.txtSQLloginId.AcceptsReturn = True
        Me.txtSQLloginId.BackColor = System.Drawing.SystemColors.Window
        Me.txtSQLloginId.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSQLloginId.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSQLloginId.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSQLloginId.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.txtSQLloginId.Location = New System.Drawing.Point(16, 218)
        Me.txtSQLloginId.MaxLength = 255
        Me.txtSQLloginId.Name = "txtSQLloginId"
        Me.txtSQLloginId.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtSQLloginId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSQLloginId.Size = New System.Drawing.Size(197, 23)
        Me.txtSQLloginId.TabIndex = 16
        '
        'cmdOk
        '
        Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOk.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOk.Location = New System.Drawing.Point(51, 299)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(73, 25)
        Me.cmdOk.TabIndex = 22
        Me.cmdOk.Text = "&OK"
        Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOk.UseVisualStyleBackColor = False
        '
        'lblConnect
        '
        Me.lblConnect.AutoSize = True
        Me.lblConnect.BackColor = System.Drawing.SystemColors.Window
        Me.lblConnect.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblConnect.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConnect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblConnect.Location = New System.Drawing.Point(16, 122)
        Me.lblConnect.Name = "lblConnect"
        Me.lblConnect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblConnect.Size = New System.Drawing.Size(88, 15)
        Me.lblConnect.TabIndex = 19
        Me.lblConnect.Text = "Connect Using:"
        '
        'lblVersion
        '
        Me.lblVersion.BackColor = System.Drawing.SystemColors.Window
        Me.lblVersion.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblVersion.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVersion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblVersion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblVersion.Location = New System.Drawing.Point(13, 299)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblVersion.Size = New System.Drawing.Size(285, 17)
        Me.lblVersion.TabIndex = 17
        Me.lblVersion.Text = "Client Software Version Unavailable"
        Me.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'frmSQLCredentials
        '
        Me.AcceptButton = Me.cmdOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(675, 339)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.pnlSQLCredentials)
        Me.Controls.Add(Me.lblPure)
        Me.Controls.Add(Me.imgLogo)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSQLCredentials"
        Me.Text = "Pure Insurance- SQL Login"
        CType(Me.imgLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlSQLCredentials.ResumeLayout(False)
        Me.pnlSQLCredentials.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents imgLogo As System.Windows.Forms.PictureBox
    Friend WithEvents lblPure As System.Windows.Forms.Label
    Friend WithEvents pnlSQLCredentials As System.Windows.Forms.Panel
    Public WithEvents lblLoginId As System.Windows.Forms.Label
    Public WithEvents txtSQLPassword As System.Windows.Forms.TextBox
    Public WithEvents lblPassword As System.Windows.Forms.Label
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents txtSQLloginId As System.Windows.Forms.TextBox
    Public WithEvents cmdOk As System.Windows.Forms.Button
    Public WithEvents lblConnect As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents txtDataBase As System.Windows.Forms.TextBox
    Friend WithEvents optSQLServerAuthentication As System.Windows.Forms.RadioButton
    Friend WithEvents optWindowsAuthentication As System.Windows.Forms.RadioButton
    Public WithEvents txtDataBaseServer As System.Windows.Forms.TextBox
    Public WithEvents lblVersion As System.Windows.Forms.Label
End Class
