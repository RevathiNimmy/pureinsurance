<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
	Public WithEvents cmdNext As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents pctLogo As System.Windows.Forms.PictureBox
	Public WithEvents cmdPre As System.Windows.Forms.Button
	Public WithEvents cmdSA As System.Windows.Forms.Button
	Public WithEvents cmdDME As System.Windows.Forms.Button
	Public WithEvents lblPre As System.Windows.Forms.Label
	Public WithEvents lblDME As System.Windows.Forms.Label
	Public WithEvents lblSA As System.Windows.Forms.Label
	Public WithEvents fraInstall As System.Windows.Forms.GroupBox
	Public WithEvents cmdPreClient As System.Windows.Forms.Button
	Public WithEvents cmdPreServer As System.Windows.Forms.Button
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents fraPre As System.Windows.Forms.GroupBox
	Public WithEvents cmdSAServer As System.Windows.Forms.Button
	Public WithEvents cmdSAClient As System.Windows.Forms.Button
	Public WithEvents lblSAServer As System.Windows.Forms.Label
	Public WithEvents lblSAClient As System.Windows.Forms.Label
	Public WithEvents fraQuestion As System.Windows.Forms.GroupBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdNext = New System.Windows.Forms.Button
		Me.cmdExit = New System.Windows.Forms.Button
		Me.pctLogo = New System.Windows.Forms.PictureBox
		Me.fraInstall = New System.Windows.Forms.GroupBox
		Me.cmdPre = New System.Windows.Forms.Button
		Me.cmdSA = New System.Windows.Forms.Button
		Me.cmdDME = New System.Windows.Forms.Button
		Me.lblPre = New System.Windows.Forms.Label
		Me.lblDME = New System.Windows.Forms.Label
		Me.lblSA = New System.Windows.Forms.Label
		Me.fraPre = New System.Windows.Forms.GroupBox
		Me.cmdPreClient = New System.Windows.Forms.Button
		Me.cmdPreServer = New System.Windows.Forms.Button
		Me.Label2 = New System.Windows.Forms.Label
		Me.Label1 = New System.Windows.Forms.Label
		Me.fraQuestion = New System.Windows.Forms.GroupBox
		Me.cmdSAServer = New System.Windows.Forms.Button
		Me.cmdSAClient = New System.Windows.Forms.Button
		Me.lblSAServer = New System.Windows.Forms.Label
		Me.lblSAClient = New System.Windows.Forms.Label
		Me.fraInstall.SuspendLayout()
		Me.fraPre.SuspendLayout()
		Me.fraQuestion.SuspendLayout()
		Me.SuspendLayout()
		' 
		' cmdNext
		' 
		Me.cmdNext.BackColor = System.Drawing.SystemColors.Control
		Me.cmdNext.CausesValidation = True
		Me.cmdNext.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdNext.Enabled = True
		Me.cmdNext.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdNext.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNext.Location = New System.Drawing.Point(328, 376)
		Me.cmdNext.Name = "cmdNext"
		Me.cmdNext.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNext.Size = New System.Drawing.Size(73, 22)
		Me.cmdNext.TabIndex = 3
		Me.cmdNext.TabStop = True
		Me.cmdNext.Tag = "next"
		Me.cmdNext.Text = "&Next >>"
		Me.cmdNext.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdNext.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdExit
		' 
		Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdExit.CausesValidation = True
		Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdExit.Enabled = True
		Me.cmdExit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdExit.Location = New System.Drawing.Point(8, 376)
		Me.cmdExit.Name = "cmdExit"
		Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdExit.Size = New System.Drawing.Size(73, 22)
		Me.cmdExit.TabIndex = 4
		Me.cmdExit.TabStop = True
		Me.cmdExit.Text = "&Exit"
		Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' pctLogo
		' 
		Me.pctLogo.BackColor = System.Drawing.SystemColors.Control
		Me.pctLogo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.pctLogo.CausesValidation = True
		Me.pctLogo.Cursor = System.Windows.Forms.Cursors.Default
		Me.pctLogo.Dock = System.Windows.Forms.DockStyle.None
		Me.pctLogo.Enabled = True
		Me.pctLogo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pctLogo.Image = CType(resources.GetObject("pctLogo.Image"), System.Drawing.Image)
		Me.pctLogo.Location = New System.Drawing.Point(8, 8)
		Me.pctLogo.Name = "pctLogo"
		Me.pctLogo.Size = New System.Drawing.Size(390, 198)
		Me.pctLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
		Me.pctLogo.TabIndex = 5
		Me.pctLogo.TabStop = True
		Me.pctLogo.Visible = True
		' 
		' fraInstall
		' 
		Me.fraInstall.BackColor = System.Drawing.SystemColors.Control
		Me.fraInstall.Controls.Add(Me.cmdPre)
		Me.fraInstall.Controls.Add(Me.cmdSA)
		Me.fraInstall.Controls.Add(Me.cmdDME)
		Me.fraInstall.Controls.Add(Me.lblPre)
		Me.fraInstall.Controls.Add(Me.lblDME)
		Me.fraInstall.Controls.Add(Me.lblSA)
		Me.fraInstall.Enabled = True
		Me.fraInstall.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraInstall.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraInstall.Location = New System.Drawing.Point(8, 216)
		Me.fraInstall.Name = "fraInstall"
		Me.fraInstall.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraInstall.Size = New System.Drawing.Size(393, 153)
		Me.fraInstall.TabIndex = 0
		Me.fraInstall.Text = "Please select your required installation"
		Me.fraInstall.Visible = True
		' 
		' cmdPre
		' 
		Me.cmdPre.BackColor = System.Drawing.SystemColors.Control
		Me.cmdPre.CausesValidation = True
		Me.cmdPre.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdPre.Enabled = True
		Me.cmdPre.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdPre.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdPre.Location = New System.Drawing.Point(272, 32)
		Me.cmdPre.Name = "cmdPre"
		Me.cmdPre.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdPre.Size = New System.Drawing.Size(73, 22)
		Me.cmdPre.TabIndex = 14
		Me.cmdPre.TabStop = True
		Me.cmdPre.Text = "Install"
		Me.cmdPre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdPre.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdSA
		' 
		Me.cmdSA.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSA.CausesValidation = True
		Me.cmdSA.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSA.Enabled = True
		Me.cmdSA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdSA.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSA.Location = New System.Drawing.Point(272, 72)
		Me.cmdSA.Name = "cmdSA"
		Me.cmdSA.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSA.Size = New System.Drawing.Size(73, 22)
		Me.cmdSA.TabIndex = 1
		Me.cmdSA.TabStop = True
		Me.cmdSA.Text = "Install"
		Me.cmdSA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSA.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdDME
		' 
		Me.cmdDME.BackColor = System.Drawing.SystemColors.Control
		Me.cmdDME.CausesValidation = True
		Me.cmdDME.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdDME.Enabled = True
		Me.cmdDME.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdDME.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdDME.Location = New System.Drawing.Point(272, 104)
		Me.cmdDME.Name = "cmdDME"
		Me.cmdDME.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdDME.Size = New System.Drawing.Size(73, 22)
		Me.cmdDME.TabIndex = 2
		Me.cmdDME.TabStop = True
		Me.cmdDME.Text = "Install"
		Me.cmdDME.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdDME.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lblPre
		' 
		Me.lblPre.AutoSize = True
		Me.lblPre.BackColor = System.Drawing.SystemColors.Control
		Me.lblPre.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPre.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPre.Enabled = True
		Me.lblPre.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPre.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPre.Location = New System.Drawing.Point(32, 32)
		Me.lblPre.Name = "lblPre"
		Me.lblPre.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPre.Size = New System.Drawing.Size(138, 16)
		Me.lblPre.TabIndex = 13
		Me.lblPre.Text = "Sirius Pre-Requisites v "
		Me.lblPre.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPre.UseMnemonic = True
		Me.lblPre.Visible = True
		' 
		' lblDME
		' 
		Me.lblDME.AutoSize = True
		Me.lblDME.BackColor = System.Drawing.SystemColors.Control
		Me.lblDME.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDME.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDME.Enabled = True
		Me.lblDME.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDME.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDME.Location = New System.Drawing.Point(32, 104)
		Me.lblDME.Name = "lblDME"
		Me.lblDME.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDME.Size = New System.Drawing.Size(150, 16)
		Me.lblDME.TabIndex = 7
		Me.lblDME.Text = "DocuMaster Enterprise v "
		Me.lblDME.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDME.UseMnemonic = True
		Me.lblDME.Visible = True
		' 
		' lblSA
		' 
		Me.lblSA.AutoSize = True
		Me.lblSA.BackColor = System.Drawing.SystemColors.Control
		Me.lblSA.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSA.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSA.Enabled = True
		Me.lblSA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSA.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSA.Location = New System.Drawing.Point(32, 72)
		Me.lblSA.Name = "lblSA"
		Me.lblSA.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSA.Size = New System.Drawing.Size(119, 16)
		Me.lblSA.TabIndex = 6
		Me.lblSA.Text = "Sirius Architecture v "
		Me.lblSA.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblSA.UseMnemonic = True
		Me.lblSA.Visible = True
		' 
		' fraPre
		' 
		Me.fraPre.BackColor = System.Drawing.SystemColors.Control
		Me.fraPre.Controls.Add(Me.cmdPreClient)
		Me.fraPre.Controls.Add(Me.cmdPreServer)
		Me.fraPre.Controls.Add(Me.Label2)
		Me.fraPre.Controls.Add(Me.Label1)
		Me.fraPre.Enabled = True
		Me.fraPre.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraPre.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraPre.Location = New System.Drawing.Point(8, 216)
		Me.fraPre.Name = "fraPre"
		Me.fraPre.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraPre.Size = New System.Drawing.Size(393, 153)
		Me.fraPre.TabIndex = 15
		Me.fraPre.Text = "Are you installing this software on a client, or a server?"
		Me.fraPre.Visible = True
		' 
		' cmdPreClient
		' 
		Me.cmdPreClient.BackColor = System.Drawing.SystemColors.Control
		Me.cmdPreClient.CausesValidation = True
		Me.cmdPreClient.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdPreClient.Enabled = True
		Me.cmdPreClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdPreClient.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdPreClient.Location = New System.Drawing.Point(272, 40)
		Me.cmdPreClient.Name = "cmdPreClient"
		Me.cmdPreClient.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdPreClient.Size = New System.Drawing.Size(73, 22)
		Me.cmdPreClient.TabIndex = 17
		Me.cmdPreClient.TabStop = True
		Me.cmdPreClient.Text = "Install"
		Me.cmdPreClient.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdPreClient.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdPreServer
		' 
		Me.cmdPreServer.BackColor = System.Drawing.SystemColors.Control
		Me.cmdPreServer.CausesValidation = True
		Me.cmdPreServer.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdPreServer.Enabled = True
		Me.cmdPreServer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdPreServer.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdPreServer.Location = New System.Drawing.Point(272, 72)
		Me.cmdPreServer.Name = "cmdPreServer"
		Me.cmdPreServer.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdPreServer.Size = New System.Drawing.Size(73, 22)
		Me.cmdPreServer.TabIndex = 16
		Me.cmdPreServer.TabStop = True
		Me.cmdPreServer.Text = "Install"
		Me.cmdPreServer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdPreServer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' Label2
		' 
		Me.Label2.AutoSize = True
		Me.Label2.BackColor = System.Drawing.SystemColors.Control
		Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label2.Enabled = True
		Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label2.Location = New System.Drawing.Point(32, 40)
		Me.Label2.Name = "Label2"
		Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label2.Size = New System.Drawing.Size(161, 16)
		Me.Label2.TabIndex = 19
		Me.Label2.Text = "Sirius Pre-Requisites Client"
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
		Me.Label1.Location = New System.Drawing.Point(32, 72)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(168, 16)
		Me.Label1.TabIndex = 18
		Me.Label1.Text = "Sirius Pre-Requisites Server"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' fraQuestion
		' 
		Me.fraQuestion.BackColor = System.Drawing.SystemColors.Control
		Me.fraQuestion.Controls.Add(Me.cmdSAServer)
		Me.fraQuestion.Controls.Add(Me.cmdSAClient)
		Me.fraQuestion.Controls.Add(Me.lblSAServer)
		Me.fraQuestion.Controls.Add(Me.lblSAClient)
		Me.fraQuestion.Enabled = True
		Me.fraQuestion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraQuestion.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraQuestion.Location = New System.Drawing.Point(8, 216)
		Me.fraQuestion.Name = "fraQuestion"
		Me.fraQuestion.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraQuestion.Size = New System.Drawing.Size(393, 153)
		Me.fraQuestion.TabIndex = 8
		Me.fraQuestion.Text = "Are you installing this software on a client, or a server?"
		Me.fraQuestion.Visible = True
		' 
		' cmdSAServer
		' 
		Me.cmdSAServer.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSAServer.CausesValidation = True
		Me.cmdSAServer.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSAServer.Enabled = True
		Me.cmdSAServer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdSAServer.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSAServer.Location = New System.Drawing.Point(272, 72)
		Me.cmdSAServer.Name = "cmdSAServer"
		Me.cmdSAServer.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSAServer.Size = New System.Drawing.Size(73, 22)
		Me.cmdSAServer.TabIndex = 10
		Me.cmdSAServer.TabStop = True
		Me.cmdSAServer.Text = "Install"
		Me.cmdSAServer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSAServer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdSAClient
		' 
		Me.cmdSAClient.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSAClient.CausesValidation = True
		Me.cmdSAClient.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSAClient.Enabled = True
		Me.cmdSAClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdSAClient.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSAClient.Location = New System.Drawing.Point(272, 40)
		Me.cmdSAClient.Name = "cmdSAClient"
		Me.cmdSAClient.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSAClient.Size = New System.Drawing.Size(73, 22)
		Me.cmdSAClient.TabIndex = 9
		Me.cmdSAClient.TabStop = True
		Me.cmdSAClient.Text = "Install"
		Me.cmdSAClient.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSAClient.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lblSAServer
		' 
		Me.lblSAServer.AutoSize = True
		Me.lblSAServer.BackColor = System.Drawing.SystemColors.Control
		Me.lblSAServer.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSAServer.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSAServer.Enabled = True
		Me.lblSAServer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSAServer.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSAServer.Location = New System.Drawing.Point(32, 72)
		Me.lblSAServer.Name = "lblSAServer"
		Me.lblSAServer.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSAServer.Size = New System.Drawing.Size(229, 16)
		Me.lblSAServer.TabIndex = 12
		Me.lblSAServer.Text = "Sirius Architecture Server"
		Me.lblSAServer.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblSAServer.UseMnemonic = True
		Me.lblSAServer.Visible = True
		' 
		' lblSAClient
		' 
		Me.lblSAClient.AutoSize = True
		Me.lblSAClient.BackColor = System.Drawing.SystemColors.Control
		Me.lblSAClient.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSAClient.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSAClient.Enabled = True
		Me.lblSAClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSAClient.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSAClient.Location = New System.Drawing.Point(32, 40)
		Me.lblSAClient.Name = "lblSAClient"
		Me.lblSAClient.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSAClient.Size = New System.Drawing.Size(230, 16)
		Me.lblSAClient.TabIndex = 11
		Me.lblSAClient.Text = "Sirius Architecture Client"
		Me.lblSAClient.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblSAClient.UseMnemonic = True
		Me.lblSAClient.Visible = True
		' 
		' frmMain
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(411, 410)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdNext)
		Me.Controls.Add(Me.cmdExit)
		Me.Controls.Add(Me.pctLogo)
		Me.Controls.Add(Me.fraInstall)
		Me.Controls.Add(Me.fraPre)
		Me.Controls.Add(Me.fraQuestion)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmMain.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(144, 174)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmMain"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "DocuMaster Enterprise Installation"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.fraInstall.ResumeLayout(False)
		Me.fraPre.ResumeLayout(False)
		Me.fraQuestion.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class