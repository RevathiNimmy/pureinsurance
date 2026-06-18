<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAppSettings
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
	Public WithEvents cmdChange As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents txtScanDirectory As System.Windows.Forms.TextBox
    Public WithEvents Panel3D1 As System.Windows.Forms.Panel
    Public WithEvents lbl3ScanDBName As System.Windows.Forms.Label
    Public WithEvents lbl3ScanDBPath As System.Windows.Forms.Label
	Public WithEvents pan3ScanDBName As System.Windows.Forms.Panel
	Public WithEvents pan3ScanDBPath As System.Windows.Forms.Panel
	Public WithEvents Label5 As System.Windows.Forms.Label
	Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents fra3ScanStation As System.Windows.Forms.GroupBox
    Public WithEvents lbl3ViewDBName As System.Windows.Forms.Label
    Public WithEvents lbl3ViewDBPath As System.Windows.Forms.Label
	Public WithEvents pan3ViewDBName As System.Windows.Forms.Panel
	Public WithEvents pan3ViewDBPath As System.Windows.Forms.Panel
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents fra3ViewStation As System.Windows.Forms.GroupBox
	Public WithEvents txtAPIInterval As System.Windows.Forms.TextBox
    Public WithEvents Panel3D9 As System.Windows.Forms.Panel
    Public WithEvents lbl3APIRemotePath As System.Windows.Forms.Label
    Public WithEvents lbl3APIDBName As System.Windows.Forms.Label
    Public WithEvents lbl3APIDBPath As System.Windows.Forms.Label
	Public WithEvents pan3APIRemotePath As System.Windows.Forms.Panel
	Public WithEvents pan3APIDBName As System.Windows.Forms.Panel
	Public WithEvents pan3APIDBPath As System.Windows.Forms.Panel
	Public WithEvents Label10 As System.Windows.Forms.Label
	Public WithEvents Label9 As System.Windows.Forms.Label
	Public WithEvents Label8 As System.Windows.Forms.Label
	Public WithEvents Label7 As System.Windows.Forms.Label
	Public WithEvents Label6 As System.Windows.Forms.Label
	Public WithEvents fra3API As System.Windows.Forms.GroupBox
	Public cdbChangeOpen As System.Windows.Forms.OpenFileDialog
	Public cdbChangeSave As System.Windows.Forms.SaveFileDialog
	Public cdbChangeFont As System.Windows.Forms.FontDialog
	Public cdbChangeColor As System.Windows.Forms.ColorDialog
	Public cdbChangePrint As System.Windows.Forms.PrintDialog
	Public WithEvents pan3Settings As System.Windows.Forms.Panel
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAppSettings))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.pan3Settings = New System.Windows.Forms.Panel
		Me.cmdChange = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.fra3ScanStation = New System.Windows.Forms.GroupBox
		Me.Panel3D1 = New System.Windows.Forms.Panel
		Me.txtScanDirectory = New System.Windows.Forms.TextBox
		Me.pan3ScanDBName = New System.Windows.Forms.Panel
		Me.pan3ScanDBPath = New System.Windows.Forms.Panel
		Me.Label5 = New System.Windows.Forms.Label
		Me.Label4 = New System.Windows.Forms.Label
		Me.Label3 = New System.Windows.Forms.Label
		Me.fra3ViewStation = New System.Windows.Forms.GroupBox
		Me.pan3ViewDBName = New System.Windows.Forms.Panel
		Me.pan3ViewDBPath = New System.Windows.Forms.Panel
		Me.Label2 = New System.Windows.Forms.Label
		Me.Label1 = New System.Windows.Forms.Label
		Me.fra3API = New System.Windows.Forms.GroupBox
		Me.Panel3D9 = New System.Windows.Forms.Panel
		Me.txtAPIInterval = New System.Windows.Forms.TextBox
		Me.pan3APIRemotePath = New System.Windows.Forms.Panel
		Me.pan3APIDBName = New System.Windows.Forms.Panel
		Me.pan3APIDBPath = New System.Windows.Forms.Panel
		Me.Label10 = New System.Windows.Forms.Label
		Me.Label9 = New System.Windows.Forms.Label
		Me.Label8 = New System.Windows.Forms.Label
		Me.Label7 = New System.Windows.Forms.Label
		Me.Label6 = New System.Windows.Forms.Label
		Me.cdbChangeOpen = New System.Windows.Forms.OpenFileDialog
		Me.cdbChangeSave = New System.Windows.Forms.SaveFileDialog
		Me.cdbChangeFont = New System.Windows.Forms.FontDialog
		Me.cdbChangeColor = New System.Windows.Forms.ColorDialog
		Me.cdbChangePrint = New System.Windows.Forms.PrintDialog
		Me.pan3Settings.SuspendLayout()
		Me.fra3ScanStation.SuspendLayout()
		Me.Panel3D1.SuspendLayout()
		Me.fra3ViewStation.SuspendLayout()
		Me.fra3API.SuspendLayout()
		Me.Panel3D9.SuspendLayout()
		Me.SuspendLayout()
		' 
		' pan3Settings
		' 
		Me.pan3Settings.BackColor = System.Drawing.SystemColors.Control
		Me.pan3Settings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.pan3Settings.Controls.Add(Me.cmdChange)
		Me.pan3Settings.Controls.Add(Me.cmdCancel)
		Me.pan3Settings.Controls.Add(Me.cmdOK)
		Me.pan3Settings.Controls.Add(Me.fra3ScanStation)
		Me.pan3Settings.Controls.Add(Me.fra3ViewStation)
		Me.pan3Settings.Controls.Add(Me.fra3API)
		Me.pan3Settings.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3Settings.Location = New System.Drawing.Point(0, 0)
		Me.pan3Settings.Name = "pan3Settings"
		Me.pan3Settings.Size = New System.Drawing.Size(419, 189)
		Me.pan3Settings.TabIndex = 0
		' 
		' cmdChange
		' 
		Me.cmdChange.BackColor = System.Drawing.SystemColors.Control
		Me.cmdChange.CausesValidation = True
		Me.cmdChange.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdChange.Enabled = True
		Me.cmdChange.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdChange.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdChange.Location = New System.Drawing.Point(320, 84)
		Me.cmdChange.Name = "cmdChange"
		Me.cmdChange.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdChange.Size = New System.Drawing.Size(81, 25)
		Me.cmdChange.TabIndex = 26
		Me.cmdChange.TabStop = True
		Me.cmdChange.Text = "C&hange..."
		Me.cmdChange.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdChange.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(320, 44)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(81, 25)
		Me.cmdCancel.TabIndex = 7
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
		Me.cmdOK.Location = New System.Drawing.Point(320, 16)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(81, 25)
		Me.cmdOK.TabIndex = 6
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' fra3ScanStation
		' 
		Me.fra3ScanStation.Controls.Add(Me.Panel3D1)
		Me.fra3ScanStation.Controls.Add(Me.pan3ScanDBName)
		Me.fra3ScanStation.Controls.Add(Me.pan3ScanDBPath)
		Me.fra3ScanStation.Controls.Add(Me.Label5)
		Me.fra3ScanStation.Controls.Add(Me.Label4)
		Me.fra3ScanStation.Controls.Add(Me.Label3)
		Me.fra3ScanStation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        'Me.fra3ScanStation.Font3D = Threed.enumFont3DConstants._InsetLight
		Me.fra3ScanStation.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0)
		Me.fra3ScanStation.Location = New System.Drawing.Point(12, 12)
		Me.fra3ScanStation.Name = "fra3ScanStation"
		Me.fra3ScanStation.Size = New System.Drawing.Size(289, 133)
		Me.fra3ScanStation.TabIndex = 8
		Me.fra3ScanStation.Text = "DocuMaster ScanStation"
		Me.fra3ScanStation.Visible = False
		' 
		' Panel3D1
		' 
		Me.Panel3D1.BackColor = System.Drawing.SystemColors.Control
		Me.Panel3D1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.Panel3D1.Controls.Add(Me.txtScanDirectory)
		Me.Panel3D1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Panel3D1.Location = New System.Drawing.Point(96, 96)
		Me.Panel3D1.Name = "Panel3D1"
		Me.Panel3D1.Size = New System.Drawing.Size(185, 21)
		Me.Panel3D1.TabIndex = 14
		' 
		' txtScanDirectory
		' 
		Me.txtScanDirectory.AcceptsReturn = True
		Me.txtScanDirectory.AutoSize = False
		Me.txtScanDirectory.BackColor = System.Drawing.SystemColors.Window
		Me.txtScanDirectory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.txtScanDirectory.CausesValidation = True
		Me.txtScanDirectory.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtScanDirectory.Enabled = True
		Me.txtScanDirectory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtScanDirectory.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtScanDirectory.HideSelection = True
		Me.txtScanDirectory.Location = New System.Drawing.Point(1, 1)
		Me.txtScanDirectory.MaxLength = 0
		Me.txtScanDirectory.Multiline = False
		Me.txtScanDirectory.Name = "txtScanDirectory"
		Me.txtScanDirectory.ReadOnly = False
		Me.txtScanDirectory.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtScanDirectory.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtScanDirectory.Size = New System.Drawing.Size(183, 19)
		Me.txtScanDirectory.TabIndex = 27
		Me.txtScanDirectory.TabStop = True
		Me.txtScanDirectory.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtScanDirectory.Visible = True
		' 
		' pan3ScanDBName
		' 
		Me.pan3ScanDBName.BackColor = System.Drawing.SystemColors.Control
		Me.pan3ScanDBName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.pan3ScanDBName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3ScanDBName.Location = New System.Drawing.Point(96, 60)
		Me.pan3ScanDBName.Name = "pan3ScanDBName"
		Me.pan3ScanDBName.Size = New System.Drawing.Size(161, 21)
        Me.pan3ScanDBName.TabIndex = 12
        Me.pan3ScanDBName.Controls.Add(lbl3ScanDBName)
		' 
		' pan3ScanDBPath
		' 
		Me.pan3ScanDBPath.BackColor = System.Drawing.SystemColors.Control
		Me.pan3ScanDBPath.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.pan3ScanDBPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3ScanDBPath.Location = New System.Drawing.Point(96, 24)
		Me.pan3ScanDBPath.Name = "pan3ScanDBPath"
		Me.pan3ScanDBPath.Size = New System.Drawing.Size(185, 21)
        Me.pan3ScanDBPath.TabIndex = 11
        Me.pan3ScanDBName.Controls.Add(lbl3ScanDBName)
		' 
		' Label5
		' 
		Me.Label5.AutoSize = False
		Me.Label5.BackColor = System.Drawing.Color.Transparent
		Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label5.Enabled = True
		Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label5.ForeColor = System.Drawing.SystemColors.WindowText
		Me.Label5.Location = New System.Drawing.Point(8, 100)
		Me.Label5.Name = "Label5"
		Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label5.Size = New System.Drawing.Size(77, 17)
		Me.Label5.TabIndex = 13
		Me.Label5.Text = "Scan Directory"
		Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label5.UseMnemonic = True
		Me.Label5.Visible = True
		' 
		' Label4
		' 
		Me.Label4.AutoSize = False
		Me.Label4.BackColor = System.Drawing.Color.Transparent
		Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label4.Enabled = True
		Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label4.ForeColor = System.Drawing.SystemColors.WindowText
		Me.Label4.Location = New System.Drawing.Point(8, 64)
		Me.Label4.Name = "Label4"
		Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label4.Size = New System.Drawing.Size(81, 13)
		Me.Label4.TabIndex = 10
		Me.Label4.Text = "Database Name"
		Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label4.UseMnemonic = True
		Me.Label4.Visible = True
		' 
		' Label3
		' 
		Me.Label3.AutoSize = False
		Me.Label3.BackColor = System.Drawing.Color.Transparent
		Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label3.Enabled = True
		Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label3.ForeColor = System.Drawing.SystemColors.WindowText
		Me.Label3.Location = New System.Drawing.Point(8, 28)
		Me.Label3.Name = "Label3"
		Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label3.Size = New System.Drawing.Size(85, 13)
		Me.Label3.TabIndex = 9
		Me.Label3.Text = "DocuMaster Path"
		Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label3.UseMnemonic = True
		Me.Label3.Visible = True
		' 
		' fra3ViewStation
		' 
		Me.fra3ViewStation.Controls.Add(Me.pan3ViewDBName)
		Me.fra3ViewStation.Controls.Add(Me.pan3ViewDBPath)
		Me.fra3ViewStation.Controls.Add(Me.Label2)
		Me.fra3ViewStation.Controls.Add(Me.Label1)
		Me.fra3ViewStation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        'Me.fra3ViewStation.Font3D = Threed.enumFont3DConstants._InsetLight
		Me.fra3ViewStation.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0)
		Me.fra3ViewStation.Location = New System.Drawing.Point(12, 12)
		Me.fra3ViewStation.Name = "fra3ViewStation"
		Me.fra3ViewStation.Size = New System.Drawing.Size(289, 101)
		Me.fra3ViewStation.TabIndex = 1
		Me.fra3ViewStation.Text = "DocuMaster ViewStation"
		Me.fra3ViewStation.Visible = False
		' 
		' pan3ViewDBName
		' 
		Me.pan3ViewDBName.BackColor = System.Drawing.SystemColors.Control
		Me.pan3ViewDBName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.pan3ViewDBName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3ViewDBName.Location = New System.Drawing.Point(100, 60)
		Me.pan3ViewDBName.Name = "pan3ViewDBName"
		Me.pan3ViewDBName.Size = New System.Drawing.Size(169, 21)
        Me.pan3ViewDBName.TabIndex = 5
        Me.pan3ViewDBName.Controls.Add(lbl3ViewDBName)
		' 
		' pan3ViewDBPath
		' 
		Me.pan3ViewDBPath.BackColor = System.Drawing.SystemColors.Control
		Me.pan3ViewDBPath.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.pan3ViewDBPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3ViewDBPath.Location = New System.Drawing.Point(100, 24)
		Me.pan3ViewDBPath.Name = "pan3ViewDBPath"
		Me.pan3ViewDBPath.Size = New System.Drawing.Size(169, 21)
        Me.pan3ViewDBPath.TabIndex = 3
        Me.pan3ViewDBPath.Controls.Add(lbl3ViewDBPath)
		' 
		' Label2
		' 
		Me.Label2.AutoSize = False
		Me.Label2.BackColor = System.Drawing.Color.Transparent
		Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label2.Enabled = True
		Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label2.ForeColor = System.Drawing.SystemColors.WindowText
		Me.Label2.Location = New System.Drawing.Point(8, 64)
		Me.Label2.Name = "Label2"
		Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label2.Size = New System.Drawing.Size(81, 13)
		Me.Label2.TabIndex = 4
		Me.Label2.Text = "Database Name"
		Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label2.UseMnemonic = True
		Me.Label2.Visible = True
		' 
		' Label1
		' 
		Me.Label1.AutoSize = False
		Me.Label1.BackColor = System.Drawing.Color.Transparent
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.Enabled = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.Label1.Location = New System.Drawing.Point(8, 28)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(89, 13)
		Me.Label1.TabIndex = 2
		Me.Label1.Text = "DocuMaster Path"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' fra3API
		' 
		Me.fra3API.Controls.Add(Me.Panel3D9)
		Me.fra3API.Controls.Add(Me.pan3APIRemotePath)
		Me.fra3API.Controls.Add(Me.pan3APIDBName)
		Me.fra3API.Controls.Add(Me.pan3APIDBPath)
		Me.fra3API.Controls.Add(Me.Label10)
		Me.fra3API.Controls.Add(Me.Label9)
		Me.fra3API.Controls.Add(Me.Label8)
		Me.fra3API.Controls.Add(Me.Label7)
		Me.fra3API.Controls.Add(Me.Label6)
		Me.fra3API.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        'Me.fra3API.Font3D = Threed.enumFont3DConstants._InsetLight
		Me.fra3API.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0)
		Me.fra3API.Location = New System.Drawing.Point(12, 12)
		Me.fra3API.Name = "fra3API"
		Me.fra3API.Size = New System.Drawing.Size(293, 161)
		Me.fra3API.TabIndex = 15
		Me.fra3API.Text = "DocuMaster API"
		Me.fra3API.Visible = False
		' 
		' Panel3D9
		' 
		Me.Panel3D9.BackColor = System.Drawing.SystemColors.Control
		Me.Panel3D9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.Panel3D9.Controls.Add(Me.txtAPIInterval)
		Me.Panel3D9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Panel3D9.Location = New System.Drawing.Point(96, 128)
		Me.Panel3D9.Name = "Panel3D9"
		Me.Panel3D9.Size = New System.Drawing.Size(45, 21)
		Me.Panel3D9.TabIndex = 23
		' 
		' txtAPIInterval
		' 
		Me.txtAPIInterval.AcceptsReturn = True
		Me.txtAPIInterval.AutoSize = False
		Me.txtAPIInterval.BackColor = System.Drawing.SystemColors.Window
		Me.txtAPIInterval.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.txtAPIInterval.CausesValidation = True
		Me.txtAPIInterval.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAPIInterval.Enabled = True
		Me.txtAPIInterval.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtAPIInterval.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAPIInterval.HideSelection = True
		Me.txtAPIInterval.Location = New System.Drawing.Point(1, 1)
		Me.txtAPIInterval.MaxLength = 2
		Me.txtAPIInterval.Multiline = False
		Me.txtAPIInterval.Name = "txtAPIInterval"
		Me.txtAPIInterval.ReadOnly = False
		Me.txtAPIInterval.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAPIInterval.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAPIInterval.Size = New System.Drawing.Size(43, 19)
		Me.txtAPIInterval.TabIndex = 24
		Me.txtAPIInterval.TabStop = True
		Me.txtAPIInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
		Me.txtAPIInterval.Visible = True
		' 
		' pan3APIRemotePath
		' 
		Me.pan3APIRemotePath.BackColor = System.Drawing.SystemColors.Control
		Me.pan3APIRemotePath.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.pan3APIRemotePath.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3APIRemotePath.Location = New System.Drawing.Point(96, 96)
		Me.pan3APIRemotePath.Name = "pan3APIRemotePath"
		Me.pan3APIRemotePath.Size = New System.Drawing.Size(185, 21)
        Me.pan3APIRemotePath.TabIndex = 20
        Me.pan3APIRemotePath.Controls.Add(lbl3APIRemotePath)
		' 
		' pan3APIDBName
		' 
		Me.pan3APIDBName.BackColor = System.Drawing.SystemColors.Control
		Me.pan3APIDBName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.pan3APIDBName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3APIDBName.Location = New System.Drawing.Point(96, 60)
		Me.pan3APIDBName.Name = "pan3APIDBName"
		Me.pan3APIDBName.Size = New System.Drawing.Size(161, 21)
        Me.pan3APIDBName.TabIndex = 19
        Me.pan3APIDBName.Controls.Add(lbl3APIDBName)
		' 
		' pan3APIDBPath
		' 
		Me.pan3APIDBPath.BackColor = System.Drawing.SystemColors.Control
		Me.pan3APIDBPath.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.pan3APIDBPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3APIDBPath.Location = New System.Drawing.Point(96, 24)
		Me.pan3APIDBPath.Name = "pan3APIDBPath"
		Me.pan3APIDBPath.Size = New System.Drawing.Size(185, 21)
        Me.pan3APIDBPath.TabIndex = 17
        Me.pan3APIDBPath.Controls.Add(lbl3APIDBPath)
		' 
		' Label10
		' 
		Me.Label10.AutoSize = False
		Me.Label10.BackColor = System.Drawing.Color.Transparent
		Me.Label10.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label10.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label10.Enabled = True
		Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label10.ForeColor = System.Drawing.SystemColors.WindowText
		Me.Label10.Location = New System.Drawing.Point(148, 132)
		Me.Label10.Name = "Label10"
		Me.Label10.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label10.Size = New System.Drawing.Size(45, 17)
		Me.Label10.TabIndex = 25
		Me.Label10.Text = "minutes"
		Me.Label10.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label10.UseMnemonic = True
		Me.Label10.Visible = True
		' 
		' Label9
		' 
		Me.Label9.AutoSize = False
		Me.Label9.BackColor = System.Drawing.Color.Transparent
		Me.Label9.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label9.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label9.Enabled = True
		Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label9.ForeColor = System.Drawing.SystemColors.WindowText
		Me.Label9.Location = New System.Drawing.Point(8, 132)
		Me.Label9.Name = "Label9"
		Me.Label9.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label9.Size = New System.Drawing.Size(69, 13)
		Me.Label9.TabIndex = 22
		Me.Label9.Text = "Run Interval"
		Me.Label9.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label9.UseMnemonic = True
		Me.Label9.Visible = True
		' 
		' Label8
		' 
		Me.Label8.AutoSize = False
		Me.Label8.BackColor = System.Drawing.Color.Transparent
		Me.Label8.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label8.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label8.Enabled = True
		Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label8.ForeColor = System.Drawing.SystemColors.WindowText
		Me.Label8.Location = New System.Drawing.Point(8, 100)
		Me.Label8.Name = "Label8"
		Me.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label8.Size = New System.Drawing.Size(105, 13)
		Me.Label8.TabIndex = 21
		Me.Label8.Text = "Remote Directory"
		Me.Label8.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label8.UseMnemonic = True
		Me.Label8.Visible = True
		' 
		' Label7
		' 
		Me.Label7.AutoSize = False
		Me.Label7.BackColor = System.Drawing.Color.Transparent
		Me.Label7.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label7.Enabled = True
		Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label7.ForeColor = System.Drawing.SystemColors.WindowText
		Me.Label7.Location = New System.Drawing.Point(8, 64)
		Me.Label7.Name = "Label7"
		Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label7.Size = New System.Drawing.Size(81, 13)
		Me.Label7.TabIndex = 18
		Me.Label7.Text = "Database Name"
		Me.Label7.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label7.UseMnemonic = True
		Me.Label7.Visible = True
		' 
		' Label6
		' 
		Me.Label6.AutoSize = False
		Me.Label6.BackColor = System.Drawing.Color.Transparent
		Me.Label6.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label6.Enabled = True
		Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label6.ForeColor = System.Drawing.SystemColors.WindowText
		Me.Label6.Location = New System.Drawing.Point(8, 28)
		Me.Label6.Name = "Label6"
		Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label6.Size = New System.Drawing.Size(89, 13)
		Me.Label6.TabIndex = 16
		Me.Label6.Text = "DocuMaster Path"
		Me.Label6.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label6.UseMnemonic = True
		Me.Label6.Visible = True
		' 
		' frmAppSettings
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
		Me.BackColor = System.Drawing.SystemColors.Window
		Me.ClientSize = New System.Drawing.Size(419, 190)
		Me.ControlBox = False
		Me.Controls.Add(Me.pan3Settings)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.cdbChangeOpen.DefaultExt = ".MDB"
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.ForeColor = System.Drawing.SystemColors.WindowText
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(108, 125)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmAppSettings"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "                                                                                                                    m"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.pan3Settings.ResumeLayout(False)
		Me.fra3ScanStation.ResumeLayout(False)
		Me.Panel3D1.ResumeLayout(False)
		Me.fra3ViewStation.ResumeLayout(False)
		Me.fra3API.ResumeLayout(False)
		Me.Panel3D9.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class