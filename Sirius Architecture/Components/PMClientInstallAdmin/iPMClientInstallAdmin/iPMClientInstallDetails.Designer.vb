<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDetails
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
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents panClientVersion As System.Windows.Forms.Panel
	Public WithEvents panClientDate As System.Windows.Forms.Panel
	Public WithEvents panMandatory As System.Windows.Forms.Panel
	Public WithEvents panAutoInstallable As System.Windows.Forms.Panel
	Public WithEvents panDescription As System.Windows.Forms.Panel
	Public WithEvents panPath As System.Windows.Forms.Panel
	Public WithEvents panProgram As System.Windows.Forms.Panel
	Public WithEvents panRebootLevel As System.Windows.Forms.Panel
	Public WithEvents lblRebootLevel As System.Windows.Forms.Label
	Public WithEvents lblProgram As System.Windows.Forms.Label
	Public WithEvents lblPath As System.Windows.Forms.Label
	Public WithEvents lblClientDescription As System.Windows.Forms.Label
	Public WithEvents lblAutoInstallable As System.Windows.Forms.Label
	Public WithEvents lblClientMandatory As System.Windows.Forms.Label
	Public WithEvents lblClientDate As System.Windows.Forms.Label
	Public WithEvents lblClientVersion As System.Windows.Forms.Label
	Public WithEvents fraClient As System.Windows.Forms.GroupBox
	Public WithEvents panServerVersion As System.Windows.Forms.Panel
	Public WithEvents panServerDate As System.Windows.Forms.Panel
	Public WithEvents lblServerVersion As System.Windows.Forms.Label
	Public WithEvents lblServerDate As System.Windows.Forms.Label
	Public WithEvents fraServer As System.Windows.Forms.GroupBox
	Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMain As System.Windows.Forms.TabControl
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.tabMain = New System.Windows.Forms.TabControl
        Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage
        Me.fraClient = New System.Windows.Forms.GroupBox
        Me.panClientVersion = New System.Windows.Forms.Panel
        Me.lblClientVersionLabel = New System.Windows.Forms.Label
        Me.panClientDate = New System.Windows.Forms.Panel
        Me.panMandatory = New System.Windows.Forms.Panel
        Me.lblMandatory = New System.Windows.Forms.Label
        Me.panAutoInstallable = New System.Windows.Forms.Panel
        Me.panDescription = New System.Windows.Forms.Panel
        Me.panPath = New System.Windows.Forms.Panel
        Me.panProgram = New System.Windows.Forms.Panel
        Me.panRebootLevel = New System.Windows.Forms.Panel
        Me.lblRebootLevel = New System.Windows.Forms.Label
        Me.lblProgram = New System.Windows.Forms.Label
        Me.lblPath = New System.Windows.Forms.Label
        Me.lblClientDescription = New System.Windows.Forms.Label
        Me.lblAutoInstallable = New System.Windows.Forms.Label
        Me.lblClientMandatory = New System.Windows.Forms.Label
        Me.lblClientDate = New System.Windows.Forms.Label
        Me.lblClientVersion = New System.Windows.Forms.Label
        Me.fraServer = New System.Windows.Forms.GroupBox
        Me.panServerVersion = New System.Windows.Forms.Panel
        Me.panServerDate = New System.Windows.Forms.Panel
        Me.lblServerVersion = New System.Windows.Forms.Label
        Me.lblServerDate = New System.Windows.Forms.Label
        Me.lblProgramlabel = New System.Windows.Forms.Label
        Me.lblpathlabel = New System.Windows.Forms.Label
        Me.lbldescriptionlabel = New System.Windows.Forms.Label
        Me.lblClientDatelabel = New System.Windows.Forms.Label
        Me.lblAutoInstallablelabel = New System.Windows.Forms.Label
        Me.lblRebootLevellabel = New System.Windows.Forms.Label
        Me.lblServerVersionlabel = New System.Windows.Forms.Label
        Me.lblServerDatelabel = New System.Windows.Forms.Label
        Me.tabMain.SuspendLayout()
        Me._tabMain_TabPage0.SuspendLayout()
        Me.fraClient.SuspendLayout()
        Me.panClientVersion.SuspendLayout()
        Me.panClientDate.SuspendLayout()
        Me.panMandatory.SuspendLayout()
        Me.panAutoInstallable.SuspendLayout()
        Me.panDescription.SuspendLayout()
        Me.panPath.SuspendLayout()
        Me.panProgram.SuspendLayout()
        Me.panRebootLevel.SuspendLayout()
        Me.fraServer.SuspendLayout()
        Me.panServerVersion.SuspendLayout()
        Me.panServerDate.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(384, 376)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 24
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(464, 376)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me._tabMain_TabPage0)
        Me.tabMain.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMain.ItemSize = New System.Drawing.Size(528, 18)
        Me.tabMain.Location = New System.Drawing.Point(8, 8)
        Me.tabMain.Multiline = True
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(533, 365)
        Me.tabMain.TabIndex = 0
        Me.tabMain.TabStop = False
        '
        '_tabMain_TabPage0
        '
        Me._tabMain_TabPage0.Controls.Add(Me.fraClient)
        Me._tabMain_TabPage0.Controls.Add(Me.fraServer)
        Me._tabMain_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage0.Name = "_tabMain_TabPage0"
        Me._tabMain_TabPage0.Size = New System.Drawing.Size(525, 339)
        Me._tabMain_TabPage0.TabIndex = 0
        Me._tabMain_TabPage0.Text = "Product"
        '
        'fraClient
        '
        Me.fraClient.BackColor = System.Drawing.SystemColors.Control
        Me.fraClient.Controls.Add(Me.panClientVersion)
        Me.fraClient.Controls.Add(Me.panClientDate)
        Me.fraClient.Controls.Add(Me.panMandatory)
        Me.fraClient.Controls.Add(Me.panAutoInstallable)
        Me.fraClient.Controls.Add(Me.panDescription)
        Me.fraClient.Controls.Add(Me.panPath)
        Me.fraClient.Controls.Add(Me.panProgram)
        Me.fraClient.Controls.Add(Me.panRebootLevel)
        Me.fraClient.Controls.Add(Me.lblRebootLevel)
        Me.fraClient.Controls.Add(Me.lblProgram)
        Me.fraClient.Controls.Add(Me.lblPath)
        Me.fraClient.Controls.Add(Me.lblClientDescription)
        Me.fraClient.Controls.Add(Me.lblAutoInstallable)
        Me.fraClient.Controls.Add(Me.lblClientMandatory)
        Me.fraClient.Controls.Add(Me.lblClientDate)
        Me.fraClient.Controls.Add(Me.lblClientVersion)
        Me.fraClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraClient.Location = New System.Drawing.Point(8, 12)
        Me.fraClient.Name = "fraClient"
        Me.fraClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraClient.Size = New System.Drawing.Size(513, 257)
        Me.fraClient.TabIndex = 2
        Me.fraClient.TabStop = False
        Me.fraClient.Text = "Client"
        '
        'panClientVersion
        '
        Me.panClientVersion.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.panClientVersion.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panClientVersion.Controls.Add(Me.lblClientVersionLabel)
        Me.panClientVersion.Font = New System.Drawing.Font("Verdana", 8.24!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panClientVersion.Location = New System.Drawing.Point(104, 16)
        Me.panClientVersion.Name = "panClientVersion"
        Me.panClientVersion.Size = New System.Drawing.Size(129, 25)
        Me.panClientVersion.TabIndex = 4
        '
        'lblClientVersionLabel
        '
        Me.lblClientVersionLabel.AutoSize = True
        Me.lblClientVersionLabel.Location = New System.Drawing.Point(3, 2)
        Me.lblClientVersionLabel.Name = "lblClientVersionLabel"
        Me.lblClientVersionLabel.Size = New System.Drawing.Size(0, 13)
        Me.lblClientVersionLabel.TabIndex = 0
        '
        'panClientDate
        '
        Me.panClientDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.panClientDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panClientDate.Controls.Add(Me.lblClientDatelabel)
        Me.panClientDate.Font = New System.Drawing.Font("Verdana", 8.24!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panClientDate.Location = New System.Drawing.Point(376, 16)
        Me.panClientDate.Name = "panClientDate"
        Me.panClientDate.Size = New System.Drawing.Size(129, 25)
        Me.panClientDate.TabIndex = 5
        '
        'panMandatory
        '
        Me.panMandatory.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.panMandatory.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panMandatory.Controls.Add(Me.lblMandatory)
        Me.panMandatory.Font = New System.Drawing.Font("Verdana", 8.24!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panMandatory.Location = New System.Drawing.Point(104, 48)
        Me.panMandatory.Name = "panMandatory"
        Me.panMandatory.Size = New System.Drawing.Size(129, 25)
        Me.panMandatory.TabIndex = 7
        '
        'lblMandatory
        '
        Me.lblMandatory.AutoSize = True
        Me.lblMandatory.Location = New System.Drawing.Point(3, 6)
        Me.lblMandatory.Name = "lblMandatory"
        Me.lblMandatory.Size = New System.Drawing.Size(0, 13)
        Me.lblMandatory.TabIndex = 0
        '
        'panAutoInstallable
        '
        Me.panAutoInstallable.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.panAutoInstallable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panAutoInstallable.Controls.Add(Me.lblAutoInstallablelabel)
        Me.panAutoInstallable.Font = New System.Drawing.Font("Verdana", 8.24!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panAutoInstallable.Location = New System.Drawing.Point(376, 48)
        Me.panAutoInstallable.Name = "panAutoInstallable"
        Me.panAutoInstallable.Size = New System.Drawing.Size(129, 25)
        Me.panAutoInstallable.TabIndex = 9
        '
        'panDescription
        '
        Me.panDescription.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.panDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panDescription.Controls.Add(Me.lbldescriptionlabel)
        Me.panDescription.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panDescription.Location = New System.Drawing.Point(104, 144)
        Me.panDescription.Name = "panDescription"
        Me.panDescription.Size = New System.Drawing.Size(401, 105)
        Me.panDescription.TabIndex = 11
        '
        'panPath
        '
        Me.panPath.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.panPath.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panPath.Controls.Add(Me.lblpathlabel)
        Me.panPath.Font = New System.Drawing.Font("Verdana", 8.24!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panPath.Location = New System.Drawing.Point(104, 112)
        Me.panPath.Name = "panPath"
        Me.panPath.Size = New System.Drawing.Size(401, 25)
        Me.panPath.TabIndex = 13
        '
        'panProgram
        '
        Me.panProgram.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.panProgram.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panProgram.Controls.Add(Me.lblProgramlabel)
        Me.panProgram.Font = New System.Drawing.Font("Verdana", 8.24!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panProgram.Location = New System.Drawing.Point(104, 80)
        Me.panProgram.Name = "panProgram"
        Me.panProgram.Size = New System.Drawing.Size(129, 25)
        Me.panProgram.TabIndex = 15
        '
        'panRebootLevel
        '
        Me.panRebootLevel.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.panRebootLevel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panRebootLevel.Controls.Add(Me.lblRebootLevellabel)
        Me.panRebootLevel.Font = New System.Drawing.Font("Verdana", 8.24!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panRebootLevel.Location = New System.Drawing.Point(376, 80)
        Me.panRebootLevel.Name = "panRebootLevel"
        Me.panRebootLevel.Size = New System.Drawing.Size(129, 25)
        Me.panRebootLevel.TabIndex = 17
        '
        'lblRebootLevel
        '
        Me.lblRebootLevel.BackColor = System.Drawing.SystemColors.Control
        Me.lblRebootLevel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRebootLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRebootLevel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRebootLevel.Location = New System.Drawing.Point(267, 84)
        Me.lblRebootLevel.Name = "lblRebootLevel"
        Me.lblRebootLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRebootLevel.Size = New System.Drawing.Size(105, 17)
        Me.lblRebootLevel.TabIndex = 18
        Me.lblRebootLevel.Text = "Reboot Level:"
        '
        'lblProgram
        '
        Me.lblProgram.BackColor = System.Drawing.SystemColors.Control
        Me.lblProgram.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProgram.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProgram.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProgram.Location = New System.Drawing.Point(8, 84)
        Me.lblProgram.Name = "lblProgram"
        Me.lblProgram.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProgram.Size = New System.Drawing.Size(81, 17)
        Me.lblProgram.TabIndex = 16
        Me.lblProgram.Text = "Program:"
        '
        'lblPath
        '
        Me.lblPath.BackColor = System.Drawing.SystemColors.Control
        Me.lblPath.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPath.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPath.Location = New System.Drawing.Point(8, 116)
        Me.lblPath.Name = "lblPath"
        Me.lblPath.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPath.Size = New System.Drawing.Size(81, 17)
        Me.lblPath.TabIndex = 14
        Me.lblPath.Text = "Path:"
        '
        'lblClientDescription
        '
        Me.lblClientDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientDescription.Location = New System.Drawing.Point(8, 144)
        Me.lblClientDescription.Name = "lblClientDescription"
        Me.lblClientDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientDescription.Size = New System.Drawing.Size(65, 17)
        Me.lblClientDescription.TabIndex = 12
        Me.lblClientDescription.Text = "Description:"
        '
        'lblAutoInstallable
        '
        Me.lblAutoInstallable.BackColor = System.Drawing.SystemColors.Control
        Me.lblAutoInstallable.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAutoInstallable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAutoInstallable.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAutoInstallable.Location = New System.Drawing.Point(267, 52)
        Me.lblAutoInstallable.Name = "lblAutoInstallable"
        Me.lblAutoInstallable.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAutoInstallable.Size = New System.Drawing.Size(105, 17)
        Me.lblAutoInstallable.TabIndex = 10
        Me.lblAutoInstallable.Text = "Auto Installable:"
        '
        'lblClientMandatory
        '
        Me.lblClientMandatory.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientMandatory.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientMandatory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientMandatory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientMandatory.Location = New System.Drawing.Point(8, 52)
        Me.lblClientMandatory.Name = "lblClientMandatory"
        Me.lblClientMandatory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientMandatory.Size = New System.Drawing.Size(81, 17)
        Me.lblClientMandatory.TabIndex = 8
        Me.lblClientMandatory.Text = "Mandatory:"
        '
        'lblClientDate
        '
        Me.lblClientDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientDate.Location = New System.Drawing.Point(267, 20)
        Me.lblClientDate.Name = "lblClientDate"
        Me.lblClientDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientDate.Size = New System.Drawing.Size(105, 17)
        Me.lblClientDate.TabIndex = 6
        Me.lblClientDate.Text = "Software Date:"
        '
        'lblClientVersion
        '
        Me.lblClientVersion.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientVersion.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientVersion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientVersion.Location = New System.Drawing.Point(8, 20)
        Me.lblClientVersion.Name = "lblClientVersion"
        Me.lblClientVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientVersion.Size = New System.Drawing.Size(81, 17)
        Me.lblClientVersion.TabIndex = 3
        Me.lblClientVersion.Text = "Version:"
        '
        'fraServer
        '
        Me.fraServer.BackColor = System.Drawing.SystemColors.Control
        Me.fraServer.Controls.Add(Me.panServerVersion)
        Me.fraServer.Controls.Add(Me.panServerDate)
        Me.fraServer.Controls.Add(Me.lblServerVersion)
        Me.fraServer.Controls.Add(Me.lblServerDate)
        Me.fraServer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraServer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraServer.Location = New System.Drawing.Point(8, 276)
        Me.fraServer.Name = "fraServer"
        Me.fraServer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraServer.Size = New System.Drawing.Size(513, 49)
        Me.fraServer.TabIndex = 19
        Me.fraServer.TabStop = False
        Me.fraServer.Text = "Server"
        '
        'panServerVersion
        '
        Me.panServerVersion.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.panServerVersion.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panServerVersion.Controls.Add(Me.lblServerVersionlabel)
        Me.panServerVersion.Font = New System.Drawing.Font("Verdana", 8.24!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panServerVersion.Location = New System.Drawing.Point(120, 16)
        Me.panServerVersion.Name = "panServerVersion"
        Me.panServerVersion.Size = New System.Drawing.Size(129, 25)
        Me.panServerVersion.TabIndex = 20
        '
        'panServerDate
        '
        Me.panServerDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.panServerDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panServerDate.Controls.Add(Me.lblServerDatelabel)
        Me.panServerDate.Font = New System.Drawing.Font("Verdana", 8.24!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panServerDate.Location = New System.Drawing.Point(376, 16)
        Me.panServerDate.Name = "panServerDate"
        Me.panServerDate.Size = New System.Drawing.Size(129, 25)
        Me.panServerDate.TabIndex = 21
        '
        'lblServerVersion
        '
        Me.lblServerVersion.BackColor = System.Drawing.SystemColors.Control
        Me.lblServerVersion.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblServerVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblServerVersion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblServerVersion.Location = New System.Drawing.Point(8, 20)
        Me.lblServerVersion.Name = "lblServerVersion"
        Me.lblServerVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblServerVersion.Size = New System.Drawing.Size(105, 17)
        Me.lblServerVersion.TabIndex = 23
        Me.lblServerVersion.Text = "Required Version:"
        '
        'lblServerDate
        '
        Me.lblServerDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblServerDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblServerDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblServerDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblServerDate.Location = New System.Drawing.Point(267, 20)
        Me.lblServerDate.Name = "lblServerDate"
        Me.lblServerDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblServerDate.Size = New System.Drawing.Size(97, 17)
        Me.lblServerDate.TabIndex = 22
        Me.lblServerDate.Text = "Software Date:"
        '
        'lblProgramlabel
        '
        Me.lblProgramlabel.AutoSize = True
        Me.lblProgramlabel.Location = New System.Drawing.Point(3, 6)
        Me.lblProgramlabel.Name = "lblProgramlabel"
        Me.lblProgramlabel.Size = New System.Drawing.Size(0, 13)
        Me.lblProgramlabel.TabIndex = 0
        '
        'lblpathlabel
        '
        Me.lblpathlabel.AutoSize = True
        Me.lblpathlabel.Location = New System.Drawing.Point(3, 6)
        Me.lblpathlabel.Name = "lblpathlabel"
        Me.lblpathlabel.Size = New System.Drawing.Size(0, 13)
        Me.lblpathlabel.TabIndex = 0
        '
        'lbldescriptionlabel
        '
        Me.lbldescriptionlabel.AutoSize = True
        Me.lbldescriptionlabel.Location = New System.Drawing.Point(3, 2)
        Me.lbldescriptionlabel.Name = "lbldescriptionlabel"
        Me.lbldescriptionlabel.Size = New System.Drawing.Size(0, 13)
        Me.lbldescriptionlabel.TabIndex = 0
        '
        'lblClientDatelabel
        '
        Me.lblClientDatelabel.AutoSize = True
        Me.lblClientDatelabel.Location = New System.Drawing.Point(3, 6)
        Me.lblClientDatelabel.Name = "lblClientDatelabel"
        Me.lblClientDatelabel.Size = New System.Drawing.Size(11, 13)
        Me.lblClientDatelabel.TabIndex = 0
        Me.lblClientDatelabel.Text = " "
        '
        'lblAutoInstallablelabel
        '
        Me.lblAutoInstallablelabel.AutoSize = True
        Me.lblAutoInstallablelabel.Location = New System.Drawing.Point(3, 6)
        Me.lblAutoInstallablelabel.Name = "lblAutoInstallablelabel"
        Me.lblAutoInstallablelabel.Size = New System.Drawing.Size(0, 13)
        Me.lblAutoInstallablelabel.TabIndex = 0
        '
        'lblRebootLevellabel
        '
        Me.lblRebootLevellabel.AutoSize = True
        Me.lblRebootLevellabel.Location = New System.Drawing.Point(3, 6)
        Me.lblRebootLevellabel.Name = "lblRebootLevellabel"
        Me.lblRebootLevellabel.Size = New System.Drawing.Size(15, 13)
        Me.lblRebootLevellabel.TabIndex = 0
        Me.lblRebootLevellabel.Text = "  "
        '
        'lblServerVersionlabel
        '
        Me.lblServerVersionlabel.AutoSize = True
        Me.lblServerVersionlabel.Location = New System.Drawing.Point(3, 6)
        Me.lblServerVersionlabel.Name = "lblServerVersionlabel"
        Me.lblServerVersionlabel.Size = New System.Drawing.Size(15, 13)
        Me.lblServerVersionlabel.TabIndex = 0
        Me.lblServerVersionlabel.Text = "  "
        '
        'lblServerDatelabel
        '
        Me.lblServerDatelabel.AutoSize = True
        Me.lblServerDatelabel.Location = New System.Drawing.Point(3, 6)
        Me.lblServerDatelabel.Name = "lblServerDatelabel"
        Me.lblServerDatelabel.Size = New System.Drawing.Size(44, 13)
        Me.lblServerDatelabel.TabIndex = 0
        Me.lblServerDatelabel.Text = "Label1"
        '
        'frmDetails
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(546, 404)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.tabMain)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDetails"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.tabMain.ResumeLayout(False)
        Me._tabMain_TabPage0.ResumeLayout(False)
        Me.fraClient.ResumeLayout(False)
        Me.panClientVersion.ResumeLayout(False)
        Me.panClientVersion.PerformLayout()
        Me.panClientDate.ResumeLayout(False)
        Me.panClientDate.PerformLayout()
        Me.panMandatory.ResumeLayout(False)
        Me.panMandatory.PerformLayout()
        Me.panAutoInstallable.ResumeLayout(False)
        Me.panAutoInstallable.PerformLayout()
        Me.panDescription.ResumeLayout(False)
        Me.panDescription.PerformLayout()
        Me.panPath.ResumeLayout(False)
        Me.panPath.PerformLayout()
        Me.panProgram.ResumeLayout(False)
        Me.panProgram.PerformLayout()
        Me.panRebootLevel.ResumeLayout(False)
        Me.panRebootLevel.PerformLayout()
        Me.fraServer.ResumeLayout(False)
        Me.panServerVersion.ResumeLayout(False)
        Me.panServerVersion.PerformLayout()
        Me.panServerDate.ResumeLayout(False)
        Me.panServerDate.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblClientVersionLabel As System.Windows.Forms.Label
    Friend WithEvents lblMandatory As System.Windows.Forms.Label
    Friend WithEvents lblProgramlabel As System.Windows.Forms.Label
    Friend WithEvents lblpathlabel As System.Windows.Forms.Label
    Friend WithEvents lbldescriptionlabel As System.Windows.Forms.Label
    Friend WithEvents lblClientDatelabel As System.Windows.Forms.Label
    Friend WithEvents lblAutoInstallablelabel As System.Windows.Forms.Label
    Friend WithEvents lblRebootLevellabel As System.Windows.Forms.Label
    Friend WithEvents lblServerVersionlabel As System.Windows.Forms.Label
    Friend WithEvents lblServerDatelabel As System.Windows.Forms.Label
#End Region 
End Class