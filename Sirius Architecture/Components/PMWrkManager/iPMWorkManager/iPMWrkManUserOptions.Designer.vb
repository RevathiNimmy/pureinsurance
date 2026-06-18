<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUserOptions
#Region "Windows Form Designer generated code "
	Public Sub New()
        MyBase.New()
        Me.KeyPreview = True
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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents chkGraphics As System.Windows.Forms.CheckBox
	Public WithEvents lblGraphics As System.Windows.Forms.Label
	Public WithEvents fraGeneral As System.Windows.Forms.GroupBox
	Public WithEvents chkGridLines As System.Windows.Forms.CheckBox
	Public WithEvents chkSplashScreen As System.Windows.Forms.CheckBox
	Public WithEvents chkStatusBar As System.Windows.Forms.CheckBox
	Public WithEvents chkAvailableTasks As System.Windows.Forms.CheckBox
	Public WithEvents chkToolbar As System.Windows.Forms.CheckBox
	Public WithEvents lblGridLines As System.Windows.Forms.Label
	Public WithEvents lblSplashScreen As System.Windows.Forms.Label
	Public WithEvents lblStatusBar As System.Windows.Forms.Label
	Public WithEvents lblAvailableTasks As System.Windows.Forms.Label
	Public WithEvents lblToolbar As System.Windows.Forms.Label
	Public WithEvents fraWorkManager As System.Windows.Forms.GroupBox
	Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents sldRefresh As System.Windows.Forms.TrackBar
	Public WithEvents lblSlow As System.Windows.Forms.Label
	Public WithEvents lblFast As System.Windows.Forms.Label
	Public WithEvents lblMinutes As System.Windows.Forms.Label
	Public WithEvents lblDsplMins As System.Windows.Forms.Label
	Public WithEvents fraRefresh As System.Windows.Forms.GroupBox
	Public WithEvents chkAutoRef As System.Windows.Forms.CheckBox
	Public WithEvents lblYN As System.Windows.Forms.Label
	Private WithEvents _SSTab1_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents SSTab1 As System.Windows.Forms.TabControl
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOk = New System.Windows.Forms.Button
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me.fraGeneral = New System.Windows.Forms.GroupBox
        Me.chkGraphics = New System.Windows.Forms.CheckBox
        Me.lblGraphics = New System.Windows.Forms.Label
        Me.fraWorkManager = New System.Windows.Forms.GroupBox
        Me.chkGridLines = New System.Windows.Forms.CheckBox
        Me.chkSplashScreen = New System.Windows.Forms.CheckBox
        Me.chkStatusBar = New System.Windows.Forms.CheckBox
        Me.chkAvailableTasks = New System.Windows.Forms.CheckBox
        Me.chkToolbar = New System.Windows.Forms.CheckBox
        Me.lblGridLines = New System.Windows.Forms.Label
        Me.lblSplashScreen = New System.Windows.Forms.Label
        Me.lblStatusBar = New System.Windows.Forms.Label
        Me.lblAvailableTasks = New System.Windows.Forms.Label
        Me.lblToolbar = New System.Windows.Forms.Label
        Me._SSTab1_TabPage1 = New System.Windows.Forms.TabPage
        Me.fraRefresh = New System.Windows.Forms.GroupBox
        Me.sldRefresh = New System.Windows.Forms.TrackBar
        Me.lblSlow = New System.Windows.Forms.Label
        Me.lblFast = New System.Windows.Forms.Label
        Me.lblMinutes = New System.Windows.Forms.Label
        Me.lblDsplMins = New System.Windows.Forms.Label
        Me.chkAutoRef = New System.Windows.Forms.CheckBox
        Me.lblYN = New System.Windows.Forms.Label
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me.fraGeneral.SuspendLayout()
        Me.fraWorkManager.SuspendLayout()
        Me._SSTab1_TabPage1.SuspendLayout()
        Me.fraRefresh.SuspendLayout()
        CType(Me.sldRefresh, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(224, 176)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOk
        '
        Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOk.Location = New System.Drawing.Point(144, 176)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(73, 22)
        Me.cmdOk.TabIndex = 1
        Me.cmdOk.Text = "&Ok"
        Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOk.UseVisualStyleBackColor = False
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage1)
        Me.SSTab1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(95, 18)
        Me.SSTab1.Location = New System.Drawing.Point(8, 8)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(293, 165)
        Me.SSTab1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.SSTab1.TabIndex = 0
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.fraGeneral)
        Me._SSTab1_TabPage0.Controls.Add(Me.fraWorkManager)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(285, 139)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "Display Options"
        '
        'fraGeneral
        '
        Me.fraGeneral.BackColor = System.Drawing.SystemColors.Control
        Me.fraGeneral.Controls.Add(Me.chkGraphics)
        Me.fraGeneral.Controls.Add(Me.lblGraphics)
        Me.fraGeneral.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraGeneral.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraGeneral.Location = New System.Drawing.Point(16, 12)
        Me.fraGeneral.Name = "fraGeneral"
        Me.fraGeneral.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraGeneral.Size = New System.Drawing.Size(257, 41)
        Me.fraGeneral.TabIndex = 3
        Me.fraGeneral.TabStop = False
        Me.fraGeneral.Text = "General"
        '
        'chkGraphics
        '
        Me.chkGraphics.BackColor = System.Drawing.SystemColors.Control
        Me.chkGraphics.Checked = True
        Me.chkGraphics.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkGraphics.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkGraphics.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkGraphics.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkGraphics.Location = New System.Drawing.Point(96, 16)
        Me.chkGraphics.Name = "chkGraphics"
        Me.chkGraphics.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkGraphics.Size = New System.Drawing.Size(17, 17)
        Me.chkGraphics.TabIndex = 4
        Me.chkGraphics.UseVisualStyleBackColor = False
        '
        'lblGraphics
        '
        Me.lblGraphics.BackColor = System.Drawing.SystemColors.Control
        Me.lblGraphics.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGraphics.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGraphics.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGraphics.Location = New System.Drawing.Point(8, 16)
        Me.lblGraphics.Name = "lblGraphics"
        Me.lblGraphics.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGraphics.Size = New System.Drawing.Size(81, 17)
        Me.lblGraphics.TabIndex = 5
        Me.lblGraphics.Text = "Graphics:"
        '
        'fraWorkManager
        '
        Me.fraWorkManager.BackColor = System.Drawing.SystemColors.Control
        Me.fraWorkManager.Controls.Add(Me.chkGridLines)
        Me.fraWorkManager.Controls.Add(Me.chkSplashScreen)
        Me.fraWorkManager.Controls.Add(Me.chkStatusBar)
        Me.fraWorkManager.Controls.Add(Me.chkAvailableTasks)
        Me.fraWorkManager.Controls.Add(Me.chkToolbar)
        Me.fraWorkManager.Controls.Add(Me.lblGridLines)
        Me.fraWorkManager.Controls.Add(Me.lblSplashScreen)
        Me.fraWorkManager.Controls.Add(Me.lblStatusBar)
        Me.fraWorkManager.Controls.Add(Me.lblAvailableTasks)
        Me.fraWorkManager.Controls.Add(Me.lblToolbar)
        Me.fraWorkManager.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraWorkManager.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraWorkManager.Location = New System.Drawing.Point(16, 60)
        Me.fraWorkManager.Name = "fraWorkManager"
        Me.fraWorkManager.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraWorkManager.Size = New System.Drawing.Size(257, 73)
        Me.fraWorkManager.TabIndex = 6
        Me.fraWorkManager.TabStop = False
        Me.fraWorkManager.Text = "Work Manager"
        '
        'chkGridLines
        '
        Me.chkGridLines.BackColor = System.Drawing.SystemColors.Control
        Me.chkGridLines.Checked = True
        Me.chkGridLines.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkGridLines.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkGridLines.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkGridLines.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkGridLines.Location = New System.Drawing.Point(96, 48)
        Me.chkGridLines.Name = "chkGridLines"
        Me.chkGridLines.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkGridLines.Size = New System.Drawing.Size(17, 17)
        Me.chkGridLines.TabIndex = 11
        Me.chkGridLines.UseVisualStyleBackColor = False
        '
        'chkSplashScreen
        '
        Me.chkSplashScreen.BackColor = System.Drawing.SystemColors.Control
        Me.chkSplashScreen.Checked = True
        Me.chkSplashScreen.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSplashScreen.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSplashScreen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSplashScreen.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSplashScreen.Location = New System.Drawing.Point(232, 32)
        Me.chkSplashScreen.Name = "chkSplashScreen"
        Me.chkSplashScreen.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSplashScreen.Size = New System.Drawing.Size(17, 17)
        Me.chkSplashScreen.TabIndex = 10
        Me.chkSplashScreen.UseVisualStyleBackColor = False
        '
        'chkStatusBar
        '
        Me.chkStatusBar.BackColor = System.Drawing.SystemColors.Control
        Me.chkStatusBar.Checked = True
        Me.chkStatusBar.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkStatusBar.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkStatusBar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkStatusBar.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkStatusBar.Location = New System.Drawing.Point(96, 32)
        Me.chkStatusBar.Name = "chkStatusBar"
        Me.chkStatusBar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkStatusBar.Size = New System.Drawing.Size(17, 17)
        Me.chkStatusBar.TabIndex = 9
        Me.chkStatusBar.UseVisualStyleBackColor = False
        '
        'chkAvailableTasks
        '
        Me.chkAvailableTasks.BackColor = System.Drawing.SystemColors.Control
        Me.chkAvailableTasks.Checked = True
        Me.chkAvailableTasks.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAvailableTasks.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAvailableTasks.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAvailableTasks.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAvailableTasks.Location = New System.Drawing.Point(232, 16)
        Me.chkAvailableTasks.Name = "chkAvailableTasks"
        Me.chkAvailableTasks.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAvailableTasks.Size = New System.Drawing.Size(17, 17)
        Me.chkAvailableTasks.TabIndex = 8
        Me.chkAvailableTasks.UseVisualStyleBackColor = False
        '
        'chkToolbar
        '
        Me.chkToolbar.BackColor = System.Drawing.SystemColors.Control
        Me.chkToolbar.Checked = True
        Me.chkToolbar.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkToolbar.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkToolbar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkToolbar.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkToolbar.Location = New System.Drawing.Point(96, 16)
        Me.chkToolbar.Name = "chkToolbar"
        Me.chkToolbar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkToolbar.Size = New System.Drawing.Size(17, 17)
        Me.chkToolbar.TabIndex = 7
        Me.chkToolbar.UseVisualStyleBackColor = False
        '
        'lblGridLines
        '
        Me.lblGridLines.BackColor = System.Drawing.SystemColors.Control
        Me.lblGridLines.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGridLines.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGridLines.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGridLines.Location = New System.Drawing.Point(8, 48)
        Me.lblGridLines.Name = "lblGridLines"
        Me.lblGridLines.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGridLines.Size = New System.Drawing.Size(81, 17)
        Me.lblGridLines.TabIndex = 16
        Me.lblGridLines.Text = "Grid Lines:"
        '
        'lblSplashScreen
        '
        Me.lblSplashScreen.BackColor = System.Drawing.SystemColors.Control
        Me.lblSplashScreen.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSplashScreen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSplashScreen.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSplashScreen.Location = New System.Drawing.Point(136, 32)
        Me.lblSplashScreen.Name = "lblSplashScreen"
        Me.lblSplashScreen.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSplashScreen.Size = New System.Drawing.Size(81, 17)
        Me.lblSplashScreen.TabIndex = 15
        Me.lblSplashScreen.Text = "Splash Screen:"
        '
        'lblStatusBar
        '
        Me.lblStatusBar.BackColor = System.Drawing.SystemColors.Control
        Me.lblStatusBar.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatusBar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatusBar.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatusBar.Location = New System.Drawing.Point(8, 32)
        Me.lblStatusBar.Name = "lblStatusBar"
        Me.lblStatusBar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatusBar.Size = New System.Drawing.Size(81, 17)
        Me.lblStatusBar.TabIndex = 14
        Me.lblStatusBar.Text = "Status Bar:"
        '
        'lblAvailableTasks
        '
        Me.lblAvailableTasks.BackColor = System.Drawing.SystemColors.Control
        Me.lblAvailableTasks.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAvailableTasks.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAvailableTasks.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAvailableTasks.Location = New System.Drawing.Point(136, 16)
        Me.lblAvailableTasks.Name = "lblAvailableTasks"
        Me.lblAvailableTasks.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAvailableTasks.Size = New System.Drawing.Size(81, 17)
        Me.lblAvailableTasks.TabIndex = 13
        Me.lblAvailableTasks.Text = "AvailableTasks:"
        '
        'lblToolbar
        '
        Me.lblToolbar.BackColor = System.Drawing.SystemColors.Control
        Me.lblToolbar.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblToolbar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblToolbar.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblToolbar.Location = New System.Drawing.Point(8, 16)
        Me.lblToolbar.Name = "lblToolbar"
        Me.lblToolbar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblToolbar.Size = New System.Drawing.Size(81, 17)
        Me.lblToolbar.TabIndex = 12
        Me.lblToolbar.Text = "Toolbar:"
        '
        '_SSTab1_TabPage1
        '
        Me._SSTab1_TabPage1.Controls.Add(Me.fraRefresh)
        Me._SSTab1_TabPage1.Controls.Add(Me.chkAutoRef)
        Me._SSTab1_TabPage1.Controls.Add(Me.lblYN)
        Me._SSTab1_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage1.Name = "_SSTab1_TabPage1"
        Me._SSTab1_TabPage1.Size = New System.Drawing.Size(285, 139)
        Me._SSTab1_TabPage1.TabIndex = 1
        Me._SSTab1_TabPage1.Text = "Refresh Settings"
        '
        'fraRefresh
        '
        Me.fraRefresh.BackColor = System.Drawing.SystemColors.Control
        Me.fraRefresh.Controls.Add(Me.sldRefresh)
        Me.fraRefresh.Controls.Add(Me.lblSlow)
        Me.fraRefresh.Controls.Add(Me.lblFast)
        Me.fraRefresh.Controls.Add(Me.lblMinutes)
        Me.fraRefresh.Controls.Add(Me.lblDsplMins)
        Me.fraRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraRefresh.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraRefresh.Location = New System.Drawing.Point(8, 36)
        Me.fraRefresh.Name = "fraRefresh"
        Me.fraRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraRefresh.Size = New System.Drawing.Size(273, 97)
        Me.fraRefresh.TabIndex = 18
        Me.fraRefresh.TabStop = False
        Me.fraRefresh.Text = "Refresh Rate"
        '
        'sldRefresh
        '
        Me.sldRefresh.LargeChange = 2
        Me.sldRefresh.Location = New System.Drawing.Point(5, 32)
        Me.sldRefresh.Maximum = 60
        Me.sldRefresh.Minimum = 1
        Me.sldRefresh.Name = "sldRefresh"
        Me.sldRefresh.Size = New System.Drawing.Size(257, 42)
        Me.sldRefresh.TabIndex = 19
        Me.sldRefresh.Value = 1
        '
        'lblSlow
        '
        Me.lblSlow.AutoSize = True
        Me.lblSlow.BackColor = System.Drawing.SystemColors.Control
        Me.lblSlow.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSlow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSlow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSlow.Location = New System.Drawing.Point(232, 72)
        Me.lblSlow.Name = "lblSlow"
        Me.lblSlow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSlow.Size = New System.Drawing.Size(30, 13)
        Me.lblSlow.TabIndex = 23
        Me.lblSlow.Text = "Slow"
        '
        'lblFast
        '
        Me.lblFast.AutoSize = True
        Me.lblFast.BackColor = System.Drawing.SystemColors.Control
        Me.lblFast.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFast.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFast.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFast.Location = New System.Drawing.Point(16, 72)
        Me.lblFast.Name = "lblFast"
        Me.lblFast.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFast.Size = New System.Drawing.Size(27, 13)
        Me.lblFast.TabIndex = 22
        Me.lblFast.Text = "Fast"
        '
        'lblMinutes
        '
        Me.lblMinutes.AutoSize = True
        Me.lblMinutes.BackColor = System.Drawing.SystemColors.Control
        Me.lblMinutes.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMinutes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMinutes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMinutes.Location = New System.Drawing.Point(152, 16)
        Me.lblMinutes.Name = "lblMinutes"
        Me.lblMinutes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMinutes.Size = New System.Drawing.Size(39, 13)
        Me.lblMinutes.TabIndex = 21
        Me.lblMinutes.Text = "Minute"
        '
        'lblDsplMins
        '
        Me.lblDsplMins.BackColor = System.Drawing.Color.White
        Me.lblDsplMins.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblDsplMins.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDsplMins.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDsplMins.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDsplMins.Location = New System.Drawing.Point(124, 16)
        Me.lblDsplMins.Name = "lblDsplMins"
        Me.lblDsplMins.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDsplMins.Size = New System.Drawing.Size(22, 17)
        Me.lblDsplMins.TabIndex = 20
        Me.lblDsplMins.Text = "1"
        Me.lblDsplMins.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'chkAutoRef
        '
        Me.chkAutoRef.BackColor = System.Drawing.SystemColors.Control
        Me.chkAutoRef.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAutoRef.Checked = True
        Me.chkAutoRef.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAutoRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAutoRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAutoRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAutoRef.Location = New System.Drawing.Point(16, 12)
        Me.chkAutoRef.Name = "chkAutoRef"
        Me.chkAutoRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAutoRef.Size = New System.Drawing.Size(89, 17)
        Me.chkAutoRef.TabIndex = 17
        Me.chkAutoRef.Text = "Auto Refresh"
        Me.chkAutoRef.UseVisualStyleBackColor = False
        '
        'lblYN
        '
        Me.lblYN.AutoSize = True
        Me.lblYN.BackColor = System.Drawing.SystemColors.Control
        Me.lblYN.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblYN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblYN.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblYN.Location = New System.Drawing.Point(112, 12)
        Me.lblYN.Name = "lblYN"
        Me.lblYN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblYN.Size = New System.Drawing.Size(25, 13)
        Me.lblYN.TabIndex = 24
        Me.lblYN.Text = "Yes"
        '
        'frmUserOptions
        '
        Me.AcceptButton = Me.cmdOk
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(305, 203)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.SSTab1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmUserOptions"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.Text = "User Options"
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me.fraGeneral.ResumeLayout(False)
        Me.fraWorkManager.ResumeLayout(False)
        Me._SSTab1_TabPage1.ResumeLayout(False)
        Me._SSTab1_TabPage1.PerformLayout()
        Me.fraRefresh.ResumeLayout(False)
        Me.fraRefresh.PerformLayout()
        CType(Me.sldRefresh, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class