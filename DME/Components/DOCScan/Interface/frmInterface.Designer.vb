<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializemnuTab()
		InitializecmdView()
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
    Public WithEvents mnuFileScan As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFileReScan As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFileSep2 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuFileNextDocument As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFileSep1 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuFileExit As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuTab_0 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuTab_1 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuTab_2 As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuView As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuOptionsSettings As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuOptionsSep1 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuOptionsMultiDoc As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuOptions As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuHelpContents As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuHelpSep1 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuHelpAbout As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
    Public WithEvents cmdViewBatch As System.Windows.Forms.Button
    Public WithEvents cmdScanNext As System.Windows.Forms.Button
    Public WithEvents cmdReScan As System.Windows.Forms.Button
    Public WithEvents cmdScan As System.Windows.Forms.Button
    Public WithEvents cmdClose As System.Windows.Forms.Button
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Private WithEvents _statMainbar_Panel1 As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _statMainbar_Panel2 As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _statMainbar_Panel3 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents statMainbar As System.Windows.Forms.StatusStrip
    Public WithEvents imgMain As System.Windows.Forms.PictureBox
    'Public WithEvents IkScan1 As AxIMGKIT6Lib.AxIkScan
    Public WithEvents IkScan1 As Newtone.ImageKit.Scan
    Public WithEvents cboAccessLevel As System.Windows.Forms.ComboBox
    Public WithEvents txtScanUser As System.Windows.Forms.TextBox
    Public WithEvents lblUser As System.Windows.Forms.Label
    Public WithEvents lblAccess As System.Windows.Forms.Label
    Public WithEvents fraUser As System.Windows.Forms.GroupBox
    Public WithEvents cmdSelectDestination As System.Windows.Forms.Button
    Public WithEvents txtScanFolder As System.Windows.Forms.TextBox
    Public WithEvents txtParentFolderName As System.Windows.Forms.TextBox
    Public WithEvents txtPagesScanned As System.Windows.Forms.TextBox
    Public WithEvents txtDocumentsScanned As System.Windows.Forms.TextBox
    Public WithEvents cboDocumentName As System.Windows.Forms.ComboBox
    Public WithEvents lblDocName As System.Windows.Forms.Label
    Public WithEvents lblParent As System.Windows.Forms.Label
    Public WithEvents lblDestination As System.Windows.Forms.Label
    Public WithEvents lblDocScanned As System.Windows.Forms.Label
    Public WithEvents lblPages As System.Windows.Forms.Label
    Public WithEvents fraDocInfo As System.Windows.Forms.GroupBox
    Public WithEvents cmdPassword As System.Windows.Forms.Button
    Public WithEvents cmdKeywords As System.Windows.Forms.Button
    Public WithEvents cmdAnnotations As System.Windows.Forms.Button
    Public WithEvents tmrDisplay As System.Windows.Forms.Timer
    'Public WithEvents IkCommon1 As Newtone.ImageKit
    Public WithEvents IkFile1 As Newtone.ImageKit.IkFile

    Public WithEvents cboScanner As System.Windows.Forms.ComboBox
    Public WithEvents Frame7 As System.Windows.Forms.GroupBox
    Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents imgOptions As System.Windows.Forms.PictureBox
    Public WithEvents cmdOptimise As System.Windows.Forms.Button
    Public WithEvents txtCreateDate As System.Windows.Forms.TextBox
    Public WithEvents txtDocumentDate As System.Windows.Forms.TextBox
    Public WithEvents txtExpiryDate As System.Windows.Forms.TextBox
    Public WithEvents cmdDocReset As System.Windows.Forms.Button
    Public WithEvents cmdExpiryReset As System.Windows.Forms.Button
    Public WithEvents lblDocDate As System.Windows.Forms.Label
    Public WithEvents lblExpiryDate As System.Windows.Forms.Label
    Public WithEvents lblCreate As System.Windows.Forms.Label
    Public WithEvents fraDateInformation As System.Windows.Forms.GroupBox
    Public WithEvents optManual As System.Windows.Forms.RadioButton
    Public WithEvents optMultiYes As System.Windows.Forms.RadioButton
    Public WithEvents optMultiNo As System.Windows.Forms.RadioButton
    Public WithEvents cboDisplay As System.Windows.Forms.ComboBox
	Public WithEvents txtDelay As System.Windows.Forms.TextBox
	Public WithEvents UpDown1 As System.Windows.Forms.PictureBox
	Public WithEvents lblDisplayScanned As System.Windows.Forms.Label
	Public WithEvents lblMultipage As System.Windows.Forms.Label
	Public WithEvents lblDocNaming As System.Windows.Forms.Label
	Public WithEvents lblSeconds As System.Windows.Forms.Label
	Public WithEvents fraAdvOptions As System.Windows.Forms.GroupBox
	Private WithEvents _tabMain_TabPage1 As System.Windows.Forms.TabPage
	Private WithEvents _cmdView_2 As System.Windows.Forms.Button
	Private WithEvents _cmdView_1 As System.Windows.Forms.Button
	Private WithEvents _cmdView_0 As System.Windows.Forms.Button
    'Public WithEvents IkDisp1 As AxIMGKIT6Lib.AxIkDisp
	Public WithEvents picBorder As System.Windows.Forms.PictureBox
	Public WithEvents fraView As System.Windows.Forms.GroupBox
	Private WithEvents _tabMain_TabPage2 As System.Windows.Forms.TabPage
	Public WithEvents imgSirius As System.Windows.Forms.PictureBox
	Public WithEvents cmdTaskDDReset As System.Windows.Forms.Button
	Public WithEvents txtTaskDD As System.Windows.Forms.TextBox
	Public WithEvents cboCustomer As System.Windows.Forms.ComboBox
	Public WithEvents chkUrgent As System.Windows.Forms.CheckBox
    'Public WithEvents cboPMUserGroupByTask As AxPMUGroupLookupCtrl.AxcboPMUserGroupByTask
    Public WithEvents cboPMUserGroupByTask As PMUGroupLookupCtrl.cboPMUserGroupByTask
    'Public WithEvents cboPMUserLookup As AxPMUserLookupControl.AxcboPMUserLookup
    Public WithEvents cboPMUserLookup As PMUserLookupControl.cboPMUserLookup
    Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents FraRecipient As System.Windows.Forms.GroupBox
	Public WithEvents chkEnableTask As System.Windows.Forms.CheckBox
	Private WithEvents _tabMain_TabPage3 As System.Windows.Forms.TabPage
	Public WithEvents tabMain As System.Windows.Forms.TabControl
	Public cmdView(2) As System.Windows.Forms.Button
	Public mnuTab(2) As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents commandButtonHelper1 As Artinsoft.VB6.Gui.CommandButtonHelper
    Private WithEvents listBoxComboBoxHelper1 As Artinsoft.VB6.Gui.ListControlHelper
    Friend WithEvents IkDisp1 As Newtone.ImageKit.Win.ImageKit
    Friend WithEvents fraMultiPage As System.Windows.Forms.GroupBox
    Friend WithEvents fraDocName As System.Windows.Forms.GroupBox
    Public WithEvents optAutomatic As System.Windows.Forms.RadioButton
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdViewBatch = New System.Windows.Forms.Button
        Me.cmdScanNext = New System.Windows.Forms.Button
        Me.cmdReScan = New System.Windows.Forms.Button
        Me.cmdScan = New System.Windows.Forms.Button
        Me.cmdSelectDestination = New System.Windows.Forms.Button
        Me.cmdPassword = New System.Windows.Forms.Button
        Me.cmdKeywords = New System.Windows.Forms.Button
        Me.cmdAnnotations = New System.Windows.Forms.Button
        Me.cmdOptimise = New System.Windows.Forms.Button
        Me._cmdView_2 = New System.Windows.Forms.Button
        Me._cmdView_1 = New System.Windows.Forms.Button
        Me._cmdView_0 = New System.Windows.Forms.Button
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFileScan = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFileReScan = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFileSep2 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuFileNextDocument = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFileSep1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuFileExit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuView = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuTab_0 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuTab_1 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuTab_2 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuOptions = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuOptionsSettings = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuOptionsSep1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuOptionsMultiDoc = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelpContents = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelpSep1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuHelpAbout = New System.Windows.Forms.ToolStripMenuItem
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.statMainbar = New System.Windows.Forms.StatusStrip
        Me._statMainbar_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me._statMainbar_Panel2 = New System.Windows.Forms.ToolStripStatusLabel
        Me._statMainbar_Panel3 = New System.Windows.Forms.ToolStripStatusLabel
        Me.tabMain = New System.Windows.Forms.TabControl
        Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage
        Me.imgMain = New System.Windows.Forms.PictureBox
        Me.fraUser = New System.Windows.Forms.GroupBox
        Me.cboAccessLevel = New System.Windows.Forms.ComboBox
        Me.txtScanUser = New System.Windows.Forms.TextBox
        Me.lblUser = New System.Windows.Forms.Label
        Me.lblAccess = New System.Windows.Forms.Label
        Me.fraDocInfo = New System.Windows.Forms.GroupBox
        Me.txtScanFolder = New System.Windows.Forms.TextBox
        Me.txtParentFolderName = New System.Windows.Forms.TextBox
        Me.txtPagesScanned = New System.Windows.Forms.TextBox
        Me.txtDocumentsScanned = New System.Windows.Forms.TextBox
        Me.cboDocumentName = New System.Windows.Forms.ComboBox
        Me.lblDocName = New System.Windows.Forms.Label
        Me.lblParent = New System.Windows.Forms.Label
        Me.lblDestination = New System.Windows.Forms.Label
        Me.lblDocScanned = New System.Windows.Forms.Label
        Me.lblPages = New System.Windows.Forms.Label
        Me.Frame7 = New System.Windows.Forms.GroupBox
        Me.cboScanner = New System.Windows.Forms.ComboBox
        Me._tabMain_TabPage1 = New System.Windows.Forms.TabPage
        Me.imgOptions = New System.Windows.Forms.PictureBox
        Me.fraDateInformation = New System.Windows.Forms.GroupBox
        Me.txtCreateDate = New System.Windows.Forms.TextBox
        Me.txtDocumentDate = New System.Windows.Forms.TextBox
        Me.txtExpiryDate = New System.Windows.Forms.TextBox
        Me.cmdDocReset = New System.Windows.Forms.Button
        Me.cmdExpiryReset = New System.Windows.Forms.Button
        Me.lblDocDate = New System.Windows.Forms.Label
        Me.lblExpiryDate = New System.Windows.Forms.Label
        Me.lblCreate = New System.Windows.Forms.Label
        Me.fraAdvOptions = New System.Windows.Forms.GroupBox
        Me.fraMultiPage = New System.Windows.Forms.GroupBox
        Me.optMultiYes = New System.Windows.Forms.RadioButton
        Me.optMultiNo = New System.Windows.Forms.RadioButton
        Me.fraDocName = New System.Windows.Forms.GroupBox
        Me.optManual = New System.Windows.Forms.RadioButton
        Me.optAutomatic = New System.Windows.Forms.RadioButton
        Me.cboDisplay = New System.Windows.Forms.ComboBox
        Me.txtDelay = New System.Windows.Forms.TextBox
        Me.UpDown1 = New System.Windows.Forms.PictureBox
        Me.lblDisplayScanned = New System.Windows.Forms.Label
        Me.lblMultipage = New System.Windows.Forms.Label
        Me.lblDocNaming = New System.Windows.Forms.Label
        Me.lblSeconds = New System.Windows.Forms.Label
        Me._tabMain_TabPage2 = New System.Windows.Forms.TabPage
        Me.fraView = New System.Windows.Forms.GroupBox
        Me.IkDisp1 = New Newtone.ImageKit.Win.ImageKit
        Me.picBorder = New System.Windows.Forms.PictureBox
        Me._tabMain_TabPage3 = New System.Windows.Forms.TabPage
        Me.imgSirius = New System.Windows.Forms.PictureBox
        Me.FraRecipient = New System.Windows.Forms.GroupBox
        Me.cmdTaskDDReset = New System.Windows.Forms.Button
        Me.txtTaskDD = New System.Windows.Forms.TextBox
        Me.cboCustomer = New System.Windows.Forms.ComboBox
        Me.chkUrgent = New System.Windows.Forms.CheckBox
        Me.cboPMUserGroupByTask = New PMUGroupLookupCtrl.cboPMUserGroupByTask
        Me.cboPMUserLookup = New PMUserLookupControl.cboPMUserLookup
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.chkEnableTask = New System.Windows.Forms.CheckBox
        Me.tmrDisplay = New System.Windows.Forms.Timer(Me.components)
        Me.commandButtonHelper1 = New Artinsoft.VB6.Gui.CommandButtonHelper(Me.components)
        Me.listBoxComboBoxHelper1 = New Artinsoft.VB6.Gui.ListControlHelper(Me.components)
        Me.MainMenu1.SuspendLayout()
        Me.statMainbar.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me._tabMain_TabPage0.SuspendLayout()
        CType(Me.imgMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraUser.SuspendLayout()
        Me.fraDocInfo.SuspendLayout()
        Me.Frame7.SuspendLayout()
        Me._tabMain_TabPage1.SuspendLayout()
        CType(Me.imgOptions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraDateInformation.SuspendLayout()
        Me.fraAdvOptions.SuspendLayout()
        Me.fraMultiPage.SuspendLayout()
        Me.fraDocName.SuspendLayout()
        CType(Me.UpDown1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMain_TabPage2.SuspendLayout()
        Me.fraView.SuspendLayout()
        CType(Me.picBorder, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMain_TabPage3.SuspendLayout()
        CType(Me.imgSirius, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FraRecipient.SuspendLayout()
        CType(Me.commandButtonHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdViewBatch
        '
        Me.cmdViewBatch.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdViewBatch, True)
        Me.cmdViewBatch.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdViewBatch, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdViewBatch, Nothing)
        Me.cmdViewBatch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdViewBatch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdViewBatch.Location = New System.Drawing.Point(267, 424)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdViewBatch, System.Drawing.Color.Silver)
        Me.cmdViewBatch.Name = "cmdViewBatch"
        Me.cmdViewBatch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdViewBatch.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdViewBatch, 0)
        Me.cmdViewBatch.TabIndex = 65
        Me.cmdViewBatch.Text = "View &Batch"
        Me.cmdViewBatch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdViewBatch, "View the current batch of scans")
        Me.cmdViewBatch.UseVisualStyleBackColor = False
        '
        'cmdScanNext
        '
        Me.cmdScanNext.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdScanNext, True)
        Me.cmdScanNext.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdScanNext, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdScanNext, Nothing)
        Me.cmdScanNext.Enabled = False
        Me.cmdScanNext.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdScanNext.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdScanNext.Location = New System.Drawing.Point(187, 424)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdScanNext, System.Drawing.Color.Silver)
        Me.cmdScanNext.Name = "cmdScanNext"
        Me.cmdScanNext.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdScanNext.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdScanNext, 0)
        Me.cmdScanNext.TabIndex = 64
        Me.cmdScanNext.Text = "&Next Doc"
        Me.cmdScanNext.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdScanNext, "Scan the next document")
        Me.cmdScanNext.UseVisualStyleBackColor = False
        '
        'cmdReScan
        '
        Me.cmdReScan.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdReScan, True)
        Me.cmdReScan.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdReScan, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdReScan, Nothing)
        Me.cmdReScan.Enabled = False
        Me.cmdReScan.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReScan.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReScan.Location = New System.Drawing.Point(107, 424)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdReScan, System.Drawing.Color.Silver)
        Me.cmdReScan.Name = "cmdReScan"
        Me.cmdReScan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReScan.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdReScan, 0)
        Me.cmdReScan.TabIndex = 63
        Me.cmdReScan.Text = "&Re-Scan"
        Me.cmdReScan.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdReScan, "Rescan the current document")
        Me.cmdReScan.UseVisualStyleBackColor = False
        '
        'cmdScan
        '
        Me.cmdScan.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdScan, True)
        Me.cmdScan.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdScan, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdScan, Nothing)
        Me.cmdScan.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdScan.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdScan.Location = New System.Drawing.Point(27, 424)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdScan, System.Drawing.Color.Silver)
        Me.cmdScan.Name = "cmdScan"
        Me.cmdScan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdScan.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdScan, 0)
        Me.cmdScan.TabIndex = 62
        Me.cmdScan.Text = "&Scan"
        Me.cmdScan.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdScan, "Scans the next page")
        Me.cmdScan.UseVisualStyleBackColor = False
        '
        'cmdSelectDestination
        '
        Me.cmdSelectDestination.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdSelectDestination, True)
        Me.cmdSelectDestination.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdSelectDestination, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdSelectDestination, Nothing)
        Me.cmdSelectDestination.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSelectDestination.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelectDestination.Location = New System.Drawing.Point(277, 24)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdSelectDestination, System.Drawing.Color.Silver)
        Me.cmdSelectDestination.Name = "cmdSelectDestination"
        Me.cmdSelectDestination.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelectDestination.Size = New System.Drawing.Size(19, 20)
        Me.commandButtonHelper1.SetStyle(Me.cmdSelectDestination, 0)
        Me.cmdSelectDestination.TabIndex = 60
        Me.cmdSelectDestination.Text = "..."
        Me.cmdSelectDestination.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdSelectDestination, "Select a different folder to scan into.")
        Me.cmdSelectDestination.UseVisualStyleBackColor = False
        '
        'cmdPassword
        '
        Me.cmdPassword.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdPassword, True)
        Me.cmdPassword.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdPassword, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdPassword, Nothing)
        Me.cmdPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPassword.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPassword.Location = New System.Drawing.Point(512, 292)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdPassword, System.Drawing.Color.Silver)
        Me.cmdPassword.Name = "cmdPassword"
        Me.cmdPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPassword.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdPassword, 0)
        Me.cmdPassword.TabIndex = 45
        Me.cmdPassword.Text = "&Password"
        Me.cmdPassword.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdPassword, "Set a password for the document")
        Me.cmdPassword.UseVisualStyleBackColor = False
        '
        'cmdKeywords
        '
        Me.cmdKeywords.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdKeywords, True)
        Me.cmdKeywords.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdKeywords, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdKeywords, Nothing)
        Me.cmdKeywords.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdKeywords.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdKeywords.Location = New System.Drawing.Point(512, 260)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdKeywords, System.Drawing.Color.Silver)
        Me.cmdKeywords.Name = "cmdKeywords"
        Me.cmdKeywords.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdKeywords.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdKeywords, 0)
        Me.cmdKeywords.TabIndex = 46
        Me.cmdKeywords.Text = "&Keywords"
        Me.cmdKeywords.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdKeywords, "Add keywords to the document")
        Me.cmdKeywords.UseVisualStyleBackColor = False
        '
        'cmdAnnotations
        '
        Me.cmdAnnotations.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdAnnotations, True)
        Me.cmdAnnotations.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdAnnotations, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdAnnotations, Nothing)
        Me.cmdAnnotations.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAnnotations.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAnnotations.Location = New System.Drawing.Point(512, 324)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdAnnotations, System.Drawing.Color.Silver)
        Me.cmdAnnotations.Name = "cmdAnnotations"
        Me.cmdAnnotations.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAnnotations.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdAnnotations, 0)
        Me.cmdAnnotations.TabIndex = 47
        Me.cmdAnnotations.Text = "&Annotations"
        Me.cmdAnnotations.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdAnnotations, "Add annotations to the document")
        Me.cmdAnnotations.UseVisualStyleBackColor = False
        '
        'cmdOptimise
        '
        Me.cmdOptimise.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdOptimise, True)
        Me.cmdOptimise.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdOptimise, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdOptimise, Nothing)
        Me.cmdOptimise.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOptimise.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOptimise.Location = New System.Drawing.Point(512, 324)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdOptimise, System.Drawing.Color.Silver)
        Me.cmdOptimise.Name = "cmdOptimise"
        Me.cmdOptimise.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOptimise.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdOptimise, 0)
        Me.cmdOptimise.TabIndex = 3
        Me.cmdOptimise.Text = "Optimi&se"
        Me.cmdOptimise.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdOptimise, "Repair and optimise the database")
        Me.cmdOptimise.UseVisualStyleBackColor = False
        '
        '_cmdView_2
        '
        Me._cmdView_2.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me._cmdView_2, True)
        Me._cmdView_2.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me._cmdView_2, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me._cmdView_2, Nothing)
        Me._cmdView_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdView_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdView_2.Image = CType(resources.GetObject("_cmdView_2.Image"), System.Drawing.Image)
        Me._cmdView_2.Location = New System.Drawing.Point(520, 96)
        Me.commandButtonHelper1.SetMaskColor(Me._cmdView_2, System.Drawing.Color.Silver)
        Me._cmdView_2.Name = "_cmdView_2"
        Me._cmdView_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdView_2.Size = New System.Drawing.Size(41, 41)
        Me.commandButtonHelper1.SetStyle(Me._cmdView_2, 1)
        Me._cmdView_2.TabIndex = 74
        Me._cmdView_2.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me._cmdView_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me._cmdView_2, "Fit to Screen")
        Me._cmdView_2.UseVisualStyleBackColor = False
        '
        '_cmdView_1
        '
        Me._cmdView_1.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me._cmdView_1, True)
        Me._cmdView_1.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me._cmdView_1, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me._cmdView_1, Nothing)
        Me._cmdView_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdView_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdView_1.Image = CType(resources.GetObject("_cmdView_1.Image"), System.Drawing.Image)
        Me._cmdView_1.Location = New System.Drawing.Point(520, 56)
        Me.commandButtonHelper1.SetMaskColor(Me._cmdView_1, System.Drawing.Color.Silver)
        Me._cmdView_1.Name = "_cmdView_1"
        Me._cmdView_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdView_1.Size = New System.Drawing.Size(41, 41)
        Me.commandButtonHelper1.SetStyle(Me._cmdView_1, 1)
        Me._cmdView_1.TabIndex = 73
        Me._cmdView_1.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me._cmdView_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me._cmdView_1, "Fit to Width")
        Me._cmdView_1.UseVisualStyleBackColor = False
        '
        '_cmdView_0
        '
        Me._cmdView_0.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me._cmdView_0, True)
        Me._cmdView_0.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me._cmdView_0, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me._cmdView_0, Nothing)
        Me._cmdView_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdView_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdView_0.Image = CType(resources.GetObject("_cmdView_0.Image"), System.Drawing.Image)
        Me._cmdView_0.Location = New System.Drawing.Point(520, 16)
        Me.commandButtonHelper1.SetMaskColor(Me._cmdView_0, System.Drawing.Color.Silver)
        Me._cmdView_0.Name = "_cmdView_0"
        Me._cmdView_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdView_0.Size = New System.Drawing.Size(41, 41)
        Me.commandButtonHelper1.SetStyle(Me._cmdView_0, 1)
        Me._cmdView_0.TabIndex = 72
        Me._cmdView_0.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me._cmdView_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me._cmdView_0, "Fit to Height")
        Me._cmdView_0.UseVisualStyleBackColor = False
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuView, Me.mnuOptions, Me.mnuHelp})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(619, 24)
        Me.MainMenu1.TabIndex = 66
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFileScan, Me.mnuFileReScan, Me.mnuFileSep2, Me.mnuFileNextDocument, Me.mnuFileSep1, Me.mnuFileExit})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(35, 20)
        Me.mnuFile.Text = "&File"
        '
        'mnuFileScan
        '
        Me.mnuFileScan.Name = "mnuFileScan"
        Me.mnuFileScan.Size = New System.Drawing.Size(148, 22)
        Me.mnuFileScan.Text = "&Scan"
        '
        'mnuFileReScan
        '
        Me.mnuFileReScan.Enabled = False
        Me.mnuFileReScan.Name = "mnuFileReScan"
        Me.mnuFileReScan.Size = New System.Drawing.Size(148, 22)
        Me.mnuFileReScan.Text = "&Re-Scan"
        '
        'mnuFileSep2
        '
        Me.mnuFileSep2.Name = "mnuFileSep2"
        Me.mnuFileSep2.Size = New System.Drawing.Size(145, 6)
        '
        'mnuFileNextDocument
        '
        Me.mnuFileNextDocument.Enabled = False
        Me.mnuFileNextDocument.Name = "mnuFileNextDocument"
        Me.mnuFileNextDocument.Size = New System.Drawing.Size(148, 22)
        Me.mnuFileNextDocument.Text = "&Next Document"
        '
        'mnuFileSep1
        '
        Me.mnuFileSep1.Name = "mnuFileSep1"
        Me.mnuFileSep1.Size = New System.Drawing.Size(145, 6)
        '
        'mnuFileExit
        '
        Me.mnuFileExit.Name = "mnuFileExit"
        Me.mnuFileExit.Size = New System.Drawing.Size(148, 22)
        Me.mnuFileExit.Text = "E&xit"
        '
        'mnuView
        '
        Me.mnuView.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._mnuTab_0, Me._mnuTab_1, Me._mnuTab_2})
        Me.mnuView.Enabled = False
        Me.mnuView.Name = "mnuView"
        Me.mnuView.Size = New System.Drawing.Size(41, 20)
        Me.mnuView.Text = "&View"
        Me.mnuView.Visible = False
        '
        '_mnuTab_0
        '
        Me._mnuTab_0.Enabled = False
        Me._mnuTab_0.Name = "_mnuTab_0"
        Me._mnuTab_0.Size = New System.Drawing.Size(162, 22)
        Me._mnuTab_0.Text = "&Main"
        '
        '_mnuTab_1
        '
        Me._mnuTab_1.Enabled = False
        Me._mnuTab_1.Name = "_mnuTab_1"
        Me._mnuTab_1.Size = New System.Drawing.Size(162, 22)
        Me._mnuTab_1.Text = "&Advanced Options"
        '
        '_mnuTab_2
        '
        Me._mnuTab_2.Name = "_mnuTab_2"
        Me._mnuTab_2.Size = New System.Drawing.Size(162, 22)
        Me._mnuTab_2.Text = "&Document View"
        '
        'mnuOptions
        '
        Me.mnuOptions.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuOptionsSettings, Me.mnuOptionsSep1, Me.mnuOptionsMultiDoc})
        Me.mnuOptions.Name = "mnuOptions"
        Me.mnuOptions.Size = New System.Drawing.Size(56, 20)
        Me.mnuOptions.Text = "&Options"
        '
        'mnuOptionsSettings
        '
        Me.mnuOptionsSettings.Name = "mnuOptionsSettings"
        Me.mnuOptionsSettings.Size = New System.Drawing.Size(190, 22)
        Me.mnuOptionsSettings.Text = "&Settings..."
        '
        'mnuOptionsSep1
        '
        Me.mnuOptionsSep1.Name = "mnuOptionsSep1"
        Me.mnuOptionsSep1.Size = New System.Drawing.Size(187, 6)
        '
        'mnuOptionsMultiDoc
        '
        Me.mnuOptionsMultiDoc.Name = "mnuOptionsMultiDoc"
        Me.mnuOptionsMultiDoc.Size = New System.Drawing.Size(190, 22)
        Me.mnuOptionsMultiDoc.Text = "&Multiple Document Mode"
        '
        'mnuHelp
        '
        Me.mnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuHelpContents, Me.mnuHelpSep1, Me.mnuHelpAbout})
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(40, 20)
        Me.mnuHelp.Text = "&Help"
        '
        'mnuHelpContents
        '
        Me.mnuHelpContents.Name = "mnuHelpContents"
        Me.mnuHelpContents.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.mnuHelpContents.Size = New System.Drawing.Size(180, 22)
        Me.mnuHelpContents.Text = "&Contents"
        Me.mnuHelpContents.Visible = False
        '
        'mnuHelpSep1
        '
        Me.mnuHelpSep1.Name = "mnuHelpSep1"
        Me.mnuHelpSep1.Size = New System.Drawing.Size(177, 6)
        '
        'mnuHelpAbout
        '
        Me.mnuHelpAbout.Name = "mnuHelpAbout"
        Me.mnuHelpAbout.Size = New System.Drawing.Size(180, 22)
        Me.mnuHelpAbout.Text = "&About Document Scan"
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdClose, True)
        Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdClose, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdClose, Nothing)
        Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClose.Location = New System.Drawing.Point(440, 424)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdClose, System.Drawing.Color.Silver)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClose.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdClose, 0)
        Me.cmdClose.TabIndex = 48
        Me.cmdClose.Text = "E&xit"
        Me.cmdClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClose.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdHelp, True)
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdHelp, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdHelp, Nothing)
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(520, 424)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdHelp, System.Drawing.Color.Silver)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdHelp, 0)
        Me.cmdHelp.TabIndex = 1
        Me.cmdHelp.Text = "Hel&p"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'statMainbar
        '
        Me.statMainbar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.statMainbar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.statMainbar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._statMainbar_Panel1, Me._statMainbar_Panel2, Me._statMainbar_Panel3})
        Me.statMainbar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow
        Me.statMainbar.Location = New System.Drawing.Point(0, 466)
        Me.statMainbar.Name = "statMainbar"
        Me.statMainbar.ShowItemToolTips = True
        Me.statMainbar.Size = New System.Drawing.Size(619, 22)
        Me.statMainbar.SizingGrip = False
        Me.statMainbar.TabIndex = 0
        '
        '_statMainbar_Panel1
        '
        Me._statMainbar_Panel1.AutoSize = False
        Me._statMainbar_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._statMainbar_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._statMainbar_Panel1.DoubleClickEnabled = True
        Me._statMainbar_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._statMainbar_Panel1.Name = "_statMainbar_Panel1"
        Me._statMainbar_Panel1.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me._statMainbar_Panel1.Size = New System.Drawing.Size(167, 22)
        Me._statMainbar_Panel1.Tag = ""
        Me._statMainbar_Panel1.Text = "Ready ..."
        Me._statMainbar_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_statMainbar_Panel2
        '
        Me._statMainbar_Panel2.AutoSize = False
        Me._statMainbar_Panel2.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._statMainbar_Panel2.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._statMainbar_Panel2.DoubleClickEnabled = True
        Me._statMainbar_Panel2.Margin = New System.Windows.Forms.Padding(0)
        Me._statMainbar_Panel2.Name = "_statMainbar_Panel2"
        Me._statMainbar_Panel2.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me._statMainbar_Panel2.Size = New System.Drawing.Size(300, 22)
        Me._statMainbar_Panel2.Tag = ""
        Me._statMainbar_Panel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_statMainbar_Panel3
        '
        Me._statMainbar_Panel3.AutoSize = False
        Me._statMainbar_Panel3.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._statMainbar_Panel3.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._statMainbar_Panel3.DoubleClickEnabled = True
        Me._statMainbar_Panel3.Margin = New System.Windows.Forms.Padding(0)
        Me._statMainbar_Panel3.Name = "_statMainbar_Panel3"
        Me._statMainbar_Panel3.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me._statMainbar_Panel3.Size = New System.Drawing.Size(121, 22)
        Me._statMainbar_Panel3.Tag = ""
        Me._statMainbar_Panel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me._tabMain_TabPage0)
        Me.tabMain.Controls.Add(Me._tabMain_TabPage1)
        Me.tabMain.Controls.Add(Me._tabMain_TabPage2)
        Me.tabMain.Controls.Add(Me._tabMain_TabPage3)
        Me.tabMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMain.ItemSize = New System.Drawing.Size(149, 18)
        Me.tabMain.Location = New System.Drawing.Point(8, 32)
        Me.tabMain.Multiline = True
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(605, 389)
        Me.tabMain.TabIndex = 2
        '
        '_tabMain_TabPage0
        '
        Me._tabMain_TabPage0.Controls.Add(Me.imgMain)
        Me._tabMain_TabPage0.Controls.Add(Me.fraUser)
        Me._tabMain_TabPage0.Controls.Add(Me.fraDocInfo)
        Me._tabMain_TabPage0.Controls.Add(Me.cmdPassword)
        Me._tabMain_TabPage0.Controls.Add(Me.cmdKeywords)
        Me._tabMain_TabPage0.Controls.Add(Me.cmdAnnotations)
        Me._tabMain_TabPage0.Controls.Add(Me.Frame7)
        Me._tabMain_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage0.Name = "_tabMain_TabPage0"
        Me._tabMain_TabPage0.Size = New System.Drawing.Size(597, 363)
        Me._tabMain_TabPage0.TabIndex = 0
        Me._tabMain_TabPage0.Text = "1 - Main"
        Me._tabMain_TabPage0.UseVisualStyleBackColor = True
        '
        'imgMain
        '
        Me.imgMain.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgMain.Image = CType(resources.GetObject("imgMain.Image"), System.Drawing.Image)
        Me.imgMain.Location = New System.Drawing.Point(533, 20)
        Me.imgMain.Name = "imgMain"
        Me.imgMain.Size = New System.Drawing.Size(32, 32)
        Me.imgMain.TabIndex = 0
        Me.imgMain.TabStop = False
        '
        'fraUser
        '
        Me.fraUser.BackColor = System.Drawing.SystemColors.Control
        Me.fraUser.Controls.Add(Me.cboAccessLevel)
        Me.fraUser.Controls.Add(Me.txtScanUser)
        Me.fraUser.Controls.Add(Me.lblUser)
        Me.fraUser.Controls.Add(Me.lblAccess)
        Me.fraUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraUser.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraUser.Location = New System.Drawing.Point(336, 84)
        Me.fraUser.Name = "fraUser"
        Me.fraUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraUser.Size = New System.Drawing.Size(161, 81)
        Me.fraUser.TabIndex = 13
        Me.fraUser.TabStop = False
        Me.fraUser.Text = "User"
        '
        'cboAccessLevel
        '
        Me.cboAccessLevel.BackColor = System.Drawing.SystemColors.Window
        Me.cboAccessLevel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboAccessLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAccessLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboAccessLevel.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboAccessLevel, New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0})
        Me.cboAccessLevel.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "6", "7", "8", "9"})
        Me.cboAccessLevel.Location = New System.Drawing.Point(80, 48)
        Me.cboAccessLevel.Name = "cboAccessLevel"
        Me.cboAccessLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboAccessLevel.Size = New System.Drawing.Size(73, 21)
        Me.cboAccessLevel.TabIndex = 15
        '
        'txtScanUser
        '
        Me.txtScanUser.AcceptsReturn = True
        Me.txtScanUser.BackColor = System.Drawing.SystemColors.Menu
        Me.txtScanUser.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtScanUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtScanUser.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtScanUser.Location = New System.Drawing.Point(80, 24)
        Me.txtScanUser.MaxLength = 0
        Me.txtScanUser.Name = "txtScanUser"
        Me.txtScanUser.ReadOnly = True
        Me.txtScanUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtScanUser.Size = New System.Drawing.Size(73, 20)
        Me.txtScanUser.TabIndex = 14
        '
        'lblUser
        '
        Me.lblUser.AutoSize = True
        Me.lblUser.BackColor = System.Drawing.SystemColors.Control
        Me.lblUser.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUser.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUser.Location = New System.Drawing.Point(8, 25)
        Me.lblUser.Name = "lblUser"
        Me.lblUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUser.Size = New System.Drawing.Size(57, 13)
        Me.lblUser.TabIndex = 17
        Me.lblUser.Text = "Scan User"
        '
        'lblAccess
        '
        Me.lblAccess.AutoSize = True
        Me.lblAccess.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccess.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccess.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccess.Location = New System.Drawing.Point(8, 50)
        Me.lblAccess.Name = "lblAccess"
        Me.lblAccess.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccess.Size = New System.Drawing.Size(71, 13)
        Me.lblAccess.TabIndex = 16
        Me.lblAccess.Text = "Access Level"
        '
        'fraDocInfo
        '
        Me.fraDocInfo.BackColor = System.Drawing.SystemColors.Control
        Me.fraDocInfo.Controls.Add(Me.cmdSelectDestination)
        Me.fraDocInfo.Controls.Add(Me.txtScanFolder)
        Me.fraDocInfo.Controls.Add(Me.txtParentFolderName)
        Me.fraDocInfo.Controls.Add(Me.txtPagesScanned)
        Me.fraDocInfo.Controls.Add(Me.txtDocumentsScanned)
        Me.fraDocInfo.Controls.Add(Me.cboDocumentName)
        Me.fraDocInfo.Controls.Add(Me.lblDocName)
        Me.fraDocInfo.Controls.Add(Me.lblParent)
        Me.fraDocInfo.Controls.Add(Me.lblDestination)
        Me.fraDocInfo.Controls.Add(Me.lblDocScanned)
        Me.fraDocInfo.Controls.Add(Me.lblPages)
        Me.fraDocInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDocInfo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDocInfo.Location = New System.Drawing.Point(16, 12)
        Me.fraDocInfo.Name = "fraDocInfo"
        Me.fraDocInfo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDocInfo.Size = New System.Drawing.Size(313, 153)
        Me.fraDocInfo.TabIndex = 32
        Me.fraDocInfo.TabStop = False
        Me.fraDocInfo.Text = "Document Information"
        '
        'txtScanFolder
        '
        Me.txtScanFolder.AcceptsReturn = True
        Me.txtScanFolder.BackColor = System.Drawing.SystemColors.InactiveBorder
        Me.txtScanFolder.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtScanFolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtScanFolder.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtScanFolder.Location = New System.Drawing.Point(144, 24)
        Me.txtScanFolder.MaxLength = 0
        Me.txtScanFolder.Name = "txtScanFolder"
        Me.txtScanFolder.ReadOnly = True
        Me.txtScanFolder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtScanFolder.Size = New System.Drawing.Size(130, 20)
        Me.txtScanFolder.TabIndex = 37
        '
        'txtParentFolderName
        '
        Me.txtParentFolderName.AcceptsReturn = True
        Me.txtParentFolderName.BackColor = System.Drawing.SystemColors.InactiveBorder
        Me.txtParentFolderName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtParentFolderName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtParentFolderName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtParentFolderName.Location = New System.Drawing.Point(144, 48)
        Me.txtParentFolderName.MaxLength = 0
        Me.txtParentFolderName.Name = "txtParentFolderName"
        Me.txtParentFolderName.ReadOnly = True
        Me.txtParentFolderName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtParentFolderName.Size = New System.Drawing.Size(153, 20)
        Me.txtParentFolderName.TabIndex = 36
        '
        'txtPagesScanned
        '
        Me.txtPagesScanned.AcceptsReturn = True
        Me.txtPagesScanned.BackColor = System.Drawing.SystemColors.InactiveBorder
        Me.txtPagesScanned.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPagesScanned.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPagesScanned.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPagesScanned.Location = New System.Drawing.Point(144, 120)
        Me.txtPagesScanned.MaxLength = 0
        Me.txtPagesScanned.Name = "txtPagesScanned"
        Me.txtPagesScanned.ReadOnly = True
        Me.txtPagesScanned.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPagesScanned.Size = New System.Drawing.Size(41, 20)
        Me.txtPagesScanned.TabIndex = 35
        '
        'txtDocumentsScanned
        '
        Me.txtDocumentsScanned.AcceptsReturn = True
        Me.txtDocumentsScanned.BackColor = System.Drawing.SystemColors.InactiveBorder
        Me.txtDocumentsScanned.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDocumentsScanned.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDocumentsScanned.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDocumentsScanned.Location = New System.Drawing.Point(144, 96)
        Me.txtDocumentsScanned.MaxLength = 0
        Me.txtDocumentsScanned.Name = "txtDocumentsScanned"
        Me.txtDocumentsScanned.ReadOnly = True
        Me.txtDocumentsScanned.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDocumentsScanned.Size = New System.Drawing.Size(41, 20)
        Me.txtDocumentsScanned.TabIndex = 34
        '
        'cboDocumentName
        '
        Me.cboDocumentName.BackColor = System.Drawing.SystemColors.Window
        Me.cboDocumentName.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboDocumentName.Enabled = False
        Me.cboDocumentName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboDocumentName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboDocumentName, New Integer(-1) {})
        Me.cboDocumentName.Location = New System.Drawing.Point(144, 72)
        Me.cboDocumentName.Name = "cboDocumentName"
        Me.cboDocumentName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboDocumentName.Size = New System.Drawing.Size(153, 21)
        Me.cboDocumentName.Sorted = True
        Me.cboDocumentName.TabIndex = 33
        '
        'lblDocName
        '
        Me.lblDocName.AutoSize = True
        Me.lblDocName.BackColor = System.Drawing.SystemColors.Control
        Me.lblDocName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDocName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDocName.Location = New System.Drawing.Point(16, 74)
        Me.lblDocName.Name = "lblDocName"
        Me.lblDocName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDocName.Size = New System.Drawing.Size(87, 13)
        Me.lblDocName.TabIndex = 42
        Me.lblDocName.Text = "Document Name"
        '
        'lblParent
        '
        Me.lblParent.AutoSize = True
        Me.lblParent.BackColor = System.Drawing.SystemColors.Control
        Me.lblParent.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblParent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblParent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblParent.Location = New System.Drawing.Point(16, 49)
        Me.lblParent.Name = "lblParent"
        Me.lblParent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblParent.Size = New System.Drawing.Size(101, 13)
        Me.lblParent.TabIndex = 41
        Me.lblParent.Text = "Parent Folder Name"
        '
        'lblDestination
        '
        Me.lblDestination.AutoSize = True
        Me.lblDestination.BackColor = System.Drawing.SystemColors.Control
        Me.lblDestination.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDestination.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDestination.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDestination.Location = New System.Drawing.Point(16, 25)
        Me.lblDestination.Name = "lblDestination"
        Me.lblDestination.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDestination.Size = New System.Drawing.Size(123, 13)
        Me.lblDestination.TabIndex = 40
        Me.lblDestination.Text = "Destination Folder Name"
        '
        'lblDocScanned
        '
        Me.lblDocScanned.AutoSize = True
        Me.lblDocScanned.BackColor = System.Drawing.SystemColors.Control
        Me.lblDocScanned.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDocScanned.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocScanned.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDocScanned.Location = New System.Drawing.Point(16, 97)
        Me.lblDocScanned.Name = "lblDocScanned"
        Me.lblDocScanned.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDocScanned.Size = New System.Drawing.Size(107, 13)
        Me.lblDocScanned.TabIndex = 39
        Me.lblDocScanned.Text = "Documents Scanned"
        '
        'lblPages
        '
        Me.lblPages.AutoSize = True
        Me.lblPages.BackColor = System.Drawing.SystemColors.Control
        Me.lblPages.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPages.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPages.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPages.Location = New System.Drawing.Point(16, 121)
        Me.lblPages.Name = "lblPages"
        Me.lblPages.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPages.Size = New System.Drawing.Size(83, 13)
        Me.lblPages.TabIndex = 38
        Me.lblPages.Text = "Pages Scanned"
        '
        'Frame7
        '
        Me.Frame7.BackColor = System.Drawing.SystemColors.Control
        Me.Frame7.Controls.Add(Me.cboScanner)
        Me.Frame7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame7.Location = New System.Drawing.Point(336, 20)
        Me.Frame7.Name = "Frame7"
        Me.Frame7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame7.Size = New System.Drawing.Size(161, 65)
        Me.Frame7.TabIndex = 70
        Me.Frame7.TabStop = False
        Me.Frame7.Text = "Scanner"
        '
        'cboScanner
        '
        Me.cboScanner.BackColor = System.Drawing.SystemColors.Window
        Me.cboScanner.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboScanner.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboScanner.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboScanner, New Integer(-1) {})
        Me.cboScanner.Location = New System.Drawing.Point(8, 24)
        Me.cboScanner.Name = "cboScanner"
        Me.cboScanner.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboScanner.Size = New System.Drawing.Size(145, 21)
        Me.cboScanner.Sorted = True
        Me.cboScanner.TabIndex = 71
        '
        '_tabMain_TabPage1
        '
        Me._tabMain_TabPage1.Controls.Add(Me.imgOptions)
        Me._tabMain_TabPage1.Controls.Add(Me.cmdOptimise)
        Me._tabMain_TabPage1.Controls.Add(Me.fraDateInformation)
        Me._tabMain_TabPage1.Controls.Add(Me.fraAdvOptions)
        Me._tabMain_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage1.Name = "_tabMain_TabPage1"
        Me._tabMain_TabPage1.Size = New System.Drawing.Size(597, 363)
        Me._tabMain_TabPage1.TabIndex = 1
        Me._tabMain_TabPage1.Text = "2 - Advanced"
        Me._tabMain_TabPage1.UseVisualStyleBackColor = True
        '
        'imgOptions
        '
        Me.imgOptions.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgOptions.Image = CType(resources.GetObject("imgOptions.Image"), System.Drawing.Image)
        Me.imgOptions.Location = New System.Drawing.Point(533, 20)
        Me.imgOptions.Name = "imgOptions"
        Me.imgOptions.Size = New System.Drawing.Size(32, 32)
        Me.imgOptions.TabIndex = 0
        Me.imgOptions.TabStop = False
        '
        'fraDateInformation
        '
        Me.fraDateInformation.BackColor = System.Drawing.SystemColors.Control
        Me.fraDateInformation.Controls.Add(Me.txtCreateDate)
        Me.fraDateInformation.Controls.Add(Me.txtDocumentDate)
        Me.fraDateInformation.Controls.Add(Me.txtExpiryDate)
        Me.fraDateInformation.Controls.Add(Me.cmdDocReset)
        Me.fraDateInformation.Controls.Add(Me.cmdExpiryReset)
        Me.fraDateInformation.Controls.Add(Me.lblDocDate)
        Me.fraDateInformation.Controls.Add(Me.lblExpiryDate)
        Me.fraDateInformation.Controls.Add(Me.lblCreate)
        Me.fraDateInformation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDateInformation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDateInformation.Location = New System.Drawing.Point(16, 172)
        Me.fraDateInformation.Name = "fraDateInformation"
        Me.fraDateInformation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDateInformation.Size = New System.Drawing.Size(481, 177)
        Me.fraDateInformation.TabIndex = 4
        Me.fraDateInformation.TabStop = False
        Me.fraDateInformation.Text = "Date Information"
        '
        'txtCreateDate
        '
        Me.txtCreateDate.AcceptsReturn = True
        Me.txtCreateDate.BackColor = System.Drawing.SystemColors.InactiveBorder
        Me.txtCreateDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCreateDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCreateDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCreateDate.Location = New System.Drawing.Point(160, 88)
        Me.txtCreateDate.MaxLength = 0
        Me.txtCreateDate.Name = "txtCreateDate"
        Me.txtCreateDate.ReadOnly = True
        Me.txtCreateDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCreateDate.Size = New System.Drawing.Size(121, 20)
        Me.txtCreateDate.TabIndex = 9
        '
        'txtDocumentDate
        '
        Me.txtDocumentDate.AcceptsReturn = True
        Me.txtDocumentDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtDocumentDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDocumentDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDocumentDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDocumentDate.Location = New System.Drawing.Point(160, 24)
        Me.txtDocumentDate.MaxLength = 0
        Me.txtDocumentDate.Name = "txtDocumentDate"
        Me.txtDocumentDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDocumentDate.Size = New System.Drawing.Size(121, 20)
        Me.txtDocumentDate.TabIndex = 8
        '
        'txtExpiryDate
        '
        Me.txtExpiryDate.AcceptsReturn = True
        Me.txtExpiryDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtExpiryDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtExpiryDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExpiryDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtExpiryDate.Location = New System.Drawing.Point(160, 56)
        Me.txtExpiryDate.MaxLength = 0
        Me.txtExpiryDate.Name = "txtExpiryDate"
        Me.txtExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtExpiryDate.Size = New System.Drawing.Size(121, 20)
        Me.txtExpiryDate.TabIndex = 7
        '
        'cmdDocReset
        '
        Me.cmdDocReset.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdDocReset, True)
        Me.cmdDocReset.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdDocReset, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdDocReset, Nothing)
        Me.cmdDocReset.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDocReset.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDocReset.Location = New System.Drawing.Point(288, 24)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdDocReset, System.Drawing.Color.Silver)
        Me.cmdDocReset.Name = "cmdDocReset"
        Me.cmdDocReset.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDocReset.Size = New System.Drawing.Size(73, 19)
        Me.commandButtonHelper1.SetStyle(Me.cmdDocReset, 0)
        Me.cmdDocReset.TabIndex = 6
        Me.cmdDocReset.Text = "Reset"
        Me.cmdDocReset.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDocReset.UseVisualStyleBackColor = False
        '
        'cmdExpiryReset
        '
        Me.cmdExpiryReset.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdExpiryReset, True)
        Me.cmdExpiryReset.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdExpiryReset, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdExpiryReset, Nothing)
        Me.cmdExpiryReset.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExpiryReset.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExpiryReset.Location = New System.Drawing.Point(288, 56)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdExpiryReset, System.Drawing.Color.Silver)
        Me.cmdExpiryReset.Name = "cmdExpiryReset"
        Me.cmdExpiryReset.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExpiryReset.Size = New System.Drawing.Size(73, 19)
        Me.commandButtonHelper1.SetStyle(Me.cmdExpiryReset, 0)
        Me.cmdExpiryReset.TabIndex = 5
        Me.cmdExpiryReset.Text = "Reset"
        Me.cmdExpiryReset.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdExpiryReset.UseVisualStyleBackColor = False
        '
        'lblDocDate
        '
        Me.lblDocDate.AutoSize = True
        Me.lblDocDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblDocDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDocDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDocDate.Location = New System.Drawing.Point(16, 25)
        Me.lblDocDate.Name = "lblDocDate"
        Me.lblDocDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDocDate.Size = New System.Drawing.Size(82, 13)
        Me.lblDocDate.TabIndex = 12
        Me.lblDocDate.Text = "Document Date"
        '
        'lblExpiryDate
        '
        Me.lblExpiryDate.AutoSize = True
        Me.lblExpiryDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblExpiryDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExpiryDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExpiryDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExpiryDate.Location = New System.Drawing.Point(16, 57)
        Me.lblExpiryDate.Name = "lblExpiryDate"
        Me.lblExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExpiryDate.Size = New System.Drawing.Size(61, 13)
        Me.lblExpiryDate.TabIndex = 11
        Me.lblExpiryDate.Text = "Expiry Date"
        '
        'lblCreate
        '
        Me.lblCreate.AutoSize = True
        Me.lblCreate.BackColor = System.Drawing.SystemColors.Control
        Me.lblCreate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCreate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCreate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCreate.Location = New System.Drawing.Point(16, 89)
        Me.lblCreate.Name = "lblCreate"
        Me.lblCreate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCreate.Size = New System.Drawing.Size(64, 13)
        Me.lblCreate.TabIndex = 10
        Me.lblCreate.Text = "Create Date"
        '
        'fraAdvOptions
        '
        Me.fraAdvOptions.BackColor = System.Drawing.SystemColors.Control
        Me.fraAdvOptions.Controls.Add(Me.fraMultiPage)
        Me.fraAdvOptions.Controls.Add(Me.fraDocName)
        Me.fraAdvOptions.Controls.Add(Me.cboDisplay)
        Me.fraAdvOptions.Controls.Add(Me.txtDelay)
        Me.fraAdvOptions.Controls.Add(Me.UpDown1)
        Me.fraAdvOptions.Controls.Add(Me.lblDisplayScanned)
        Me.fraAdvOptions.Controls.Add(Me.lblMultipage)
        Me.fraAdvOptions.Controls.Add(Me.lblDocNaming)
        Me.fraAdvOptions.Controls.Add(Me.lblSeconds)
        Me.fraAdvOptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAdvOptions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAdvOptions.Location = New System.Drawing.Point(16, 12)
        Me.fraAdvOptions.Name = "fraAdvOptions"
        Me.fraAdvOptions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAdvOptions.Size = New System.Drawing.Size(481, 153)
        Me.fraAdvOptions.TabIndex = 18
        Me.fraAdvOptions.TabStop = False
        Me.fraAdvOptions.Text = "Advanced Options"
        '
        'fraMultiPage
        '
        Me.fraMultiPage.Controls.Add(Me.optMultiYes)
        Me.fraMultiPage.Controls.Add(Me.optMultiNo)
        Me.fraMultiPage.Location = New System.Drawing.Point(152, 58)
        Me.fraMultiPage.Name = "fraMultiPage"
        Me.fraMultiPage.Size = New System.Drawing.Size(282, 27)
        Me.fraMultiPage.TabIndex = 33
        Me.fraMultiPage.TabStop = False
        '
        'optMultiYes
        '
        Me.optMultiYes.BackColor = System.Drawing.SystemColors.Control
        Me.optMultiYes.Cursor = System.Windows.Forms.Cursors.Default
        Me.optMultiYes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optMultiYes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optMultiYes.Location = New System.Drawing.Point(8, 8)
        Me.optMultiYes.Name = "optMultiYes"
        Me.optMultiYes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optMultiYes.Size = New System.Drawing.Size(54, 15)
        Me.optMultiYes.TabIndex = 24
        Me.optMultiYes.TabStop = True
        Me.optMultiYes.Text = "Yes"
        Me.optMultiYes.UseVisualStyleBackColor = False
        '
        'optMultiNo
        '
        Me.optMultiNo.BackColor = System.Drawing.SystemColors.Control
        Me.optMultiNo.Checked = True
        Me.optMultiNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.optMultiNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optMultiNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optMultiNo.Location = New System.Drawing.Point(152, 8)
        Me.optMultiNo.Name = "optMultiNo"
        Me.optMultiNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optMultiNo.Size = New System.Drawing.Size(105, 15)
        Me.optMultiNo.TabIndex = 23
        Me.optMultiNo.TabStop = True
        Me.optMultiNo.Text = "No"
        Me.optMultiNo.UseVisualStyleBackColor = False
        '
        'fraDocName
        '
        Me.fraDocName.Controls.Add(Me.optManual)
        Me.fraDocName.Controls.Add(Me.optAutomatic)
        Me.fraDocName.Location = New System.Drawing.Point(152, 20)
        Me.fraDocName.Name = "fraDocName"
        Me.fraDocName.Size = New System.Drawing.Size(282, 29)
        Me.fraDocName.TabIndex = 32
        Me.fraDocName.TabStop = False
        '
        'optManual
        '
        Me.optManual.BackColor = System.Drawing.SystemColors.Control
        Me.optManual.Cursor = System.Windows.Forms.Cursors.Default
        Me.optManual.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optManual.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optManual.Location = New System.Drawing.Point(152, 7)
        Me.optManual.Name = "optManual"
        Me.optManual.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optManual.Size = New System.Drawing.Size(69, 16)
        Me.optManual.TabIndex = 27
        Me.optManual.TabStop = True
        Me.optManual.Text = "Manual"
        Me.optManual.UseVisualStyleBackColor = False
        '
        'optAutomatic
        '
        Me.optAutomatic.BackColor = System.Drawing.SystemColors.Control
        Me.optAutomatic.Checked = True
        Me.optAutomatic.Cursor = System.Windows.Forms.Cursors.Default
        Me.optAutomatic.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optAutomatic.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optAutomatic.Location = New System.Drawing.Point(8, 7)
        Me.optAutomatic.Name = "optAutomatic"
        Me.optAutomatic.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optAutomatic.Size = New System.Drawing.Size(92, 18)
        Me.optAutomatic.TabIndex = 26
        Me.optAutomatic.TabStop = True
        Me.optAutomatic.Text = "Automatic"
        Me.optAutomatic.UseVisualStyleBackColor = False
        '
        'cboDisplay
        '
        Me.cboDisplay.BackColor = System.Drawing.SystemColors.Window
        Me.cboDisplay.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboDisplay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDisplay.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboDisplay.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboDisplay, New Integer() {0, 0, 0})
        Me.cboDisplay.Items.AddRange(New Object() {"Yes", "Yes - Timed", "No"})
        Me.cboDisplay.Location = New System.Drawing.Point(160, 96)
        Me.cboDisplay.Name = "cboDisplay"
        Me.cboDisplay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboDisplay.Size = New System.Drawing.Size(121, 21)
        Me.cboDisplay.TabIndex = 21
        '
        'txtDelay
        '
        Me.txtDelay.AcceptsReturn = True
        Me.txtDelay.BackColor = System.Drawing.SystemColors.Window
        Me.txtDelay.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDelay.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDelay.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDelay.Location = New System.Drawing.Point(304, 96)
        Me.txtDelay.MaxLength = 0
        Me.txtDelay.Name = "txtDelay"
        Me.txtDelay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDelay.Size = New System.Drawing.Size(57, 20)
        Me.txtDelay.TabIndex = 20
        Me.txtDelay.Text = "0.0"
        '
        'UpDown1
        '
        Me.UpDown1.BackColor = System.Drawing.SystemColors.Control
        Me.UpDown1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.UpDown1.Cursor = System.Windows.Forms.Cursors.Default
        Me.UpDown1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UpDown1.Location = New System.Drawing.Point(344, 96)
        Me.UpDown1.Name = "UpDown1"
        Me.UpDown1.Size = New System.Drawing.Size(16, 19)
        Me.UpDown1.TabIndex = 19
        Me.UpDown1.TabStop = False
        '
        'lblDisplayScanned
        '
        Me.lblDisplayScanned.AutoSize = True
        Me.lblDisplayScanned.BackColor = System.Drawing.SystemColors.Control
        Me.lblDisplayScanned.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDisplayScanned.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDisplayScanned.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDisplayScanned.Location = New System.Drawing.Point(16, 100)
        Me.lblDisplayScanned.Name = "lblDisplayScanned"
        Me.lblDisplayScanned.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDisplayScanned.Size = New System.Drawing.Size(115, 13)
        Me.lblDisplayScanned.TabIndex = 31
        Me.lblDisplayScanned.Text = "Display Scanned Page"
        '
        'lblMultipage
        '
        Me.lblMultipage.AutoSize = True
        Me.lblMultipage.BackColor = System.Drawing.SystemColors.Control
        Me.lblMultipage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMultipage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMultipage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMultipage.Location = New System.Drawing.Point(16, 58)
        Me.lblMultipage.Name = "lblMultipage"
        Me.lblMultipage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMultipage.Size = New System.Drawing.Size(120, 13)
        Me.lblMultipage.TabIndex = 30
        Me.lblMultipage.Text = "Scan to multi-page TIFF"
        '
        'lblDocNaming
        '
        Me.lblDocNaming.AutoSize = True
        Me.lblDocNaming.BackColor = System.Drawing.SystemColors.Control
        Me.lblDocNaming.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDocNaming.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocNaming.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDocNaming.Location = New System.Drawing.Point(16, 24)
        Me.lblDocNaming.Name = "lblDocNaming"
        Me.lblDocNaming.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDocNaming.Size = New System.Drawing.Size(87, 13)
        Me.lblDocNaming.TabIndex = 29
        Me.lblDocNaming.Text = "Document Name"
        '
        'lblSeconds
        '
        Me.lblSeconds.AutoSize = True
        Me.lblSeconds.BackColor = System.Drawing.SystemColors.Control
        Me.lblSeconds.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSeconds.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSeconds.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSeconds.Location = New System.Drawing.Point(368, 104)
        Me.lblSeconds.Name = "lblSeconds"
        Me.lblSeconds.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSeconds.Size = New System.Drawing.Size(47, 13)
        Me.lblSeconds.TabIndex = 28
        Me.lblSeconds.Text = "seconds"
        '
        '_tabMain_TabPage2
        '
        Me._tabMain_TabPage2.Controls.Add(Me.fraView)
        Me._tabMain_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage2.Name = "_tabMain_TabPage2"
        Me._tabMain_TabPage2.Size = New System.Drawing.Size(597, 363)
        Me._tabMain_TabPage2.TabIndex = 2
        Me._tabMain_TabPage2.Text = "3 - Document View"
        Me._tabMain_TabPage2.UseVisualStyleBackColor = True
        '
        'fraView
        '
        Me.fraView.BackColor = System.Drawing.SystemColors.Control
        Me.fraView.Controls.Add(Me.IkDisp1)
        Me.fraView.Controls.Add(Me._cmdView_2)
        Me.fraView.Controls.Add(Me._cmdView_1)
        Me.fraView.Controls.Add(Me._cmdView_0)
        Me.fraView.Controls.Add(Me.picBorder)
        Me.fraView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraView.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraView.Location = New System.Drawing.Point(16, 12)
        Me.fraView.Name = "fraView"
        Me.fraView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraView.Size = New System.Drawing.Size(569, 337)
        Me.fraView.TabIndex = 43
        Me.fraView.TabStop = False
        '
        'IkDisp1
        '
        Me.IkDisp1.AllowDrop = True
        Me.IkDisp1.BackColor = System.Drawing.Color.White
        Me.IkDisp1.Cursor = System.Windows.Forms.Cursors.Default
        Me.IkDisp1.DefaultMouseCursor = System.Windows.Forms.Cursors.Default
        Me.IkDisp1.InvalidHatchPattern = Newtone.ImageKit.Win.HatchPattern.ShowHatch
        Me.IkDisp1.Location = New System.Drawing.Point(72, 16)
        Me.IkDisp1.MouseWheelDirection = Newtone.ImageKit.Win.WheelDirection.Vertical
        Me.IkDisp1.Name = "ImageKit1"
        Me.IkDisp1.Rect = New System.Drawing.Rectangle(0, 0, 0, 0)
        Me.IkDisp1.RectDrawRatio = 0
        Me.IkDisp1.RectMouseCursor = System.Windows.Forms.Cursors.SizeAll
        Me.IkDisp1.Refine1BitImage = True
        Me.IkDisp1.Size = New System.Drawing.Size(152, 96)
        Me.IkDisp1.TabIndex = 2
        Me.IkDisp1.Text = "ImageKit1"
        Me.IkDisp1.ToolTip = Newtone.ImageKit.Win.LengthUnit.None
        Me.IkDisp1.TransparentBlue = CType(0, Byte)
        Me.IkDisp1.TransparentGreen = CType(0, Byte)
        Me.IkDisp1.TransparentRed = CType(0, Byte)
        'Me.IkDisp1.AllowDrop = True
        'Me.IkDisp1.BackColor = System.Drawing.Color.White
        'Me.IkDisp1.Cursor = System.Windows.Forms.Cursors.Default
        'Me.IkDisp1.DefaultMouseCursor = System.Windows.Forms.Cursors.Default
        'Me.IkDisp1.DXFBlack = False
        'Me.IkDisp1.Grad = Newtone.ImageKit.Win.LengthUnit.None
        'Me.IkDisp1.GradBackgroundColor = System.Drawing.Color.FromArgb(CType(CType(236, Byte), Integer), CType(CType(233, Byte), Integer), CType(CType(216, Byte), Integer))
        'Me.IkDisp1.GradColor = System.Drawing.Color.Black
        'Me.IkDisp1.Grid = Newtone.ImageKit.Win.LengthUnit.None
        'Me.IkDisp1.GridColor = System.Drawing.Color.Silver
        'Me.IkDisp1.GridSpace = 10
        'Me.IkDisp1.InvalidHatchPattern = Newtone.ImageKit.Win.HatchPattern.ShowHatch
        'Me.IkDisp1.Location = New System.Drawing.Point(8, 16)
        'Me.IkDisp1.MouseWheelDirection = Newtone.ImageKit.Win.WheelDirection.Vertical
        'Me.IkDisp1.Name = "IkDisp1"
        'Me.IkDisp1.Rect = New System.Drawing.Rectangle(0, 0, 0, 0)
        'Me.IkDisp1.RectDrawRatio = 0
        'Me.IkDisp1.RectMouseCursor = System.Windows.Forms.Cursors.SizeAll
        'Me.IkDisp1.Refine1BitImage = True
        'Me.IkDisp1.Size = New System.Drawing.Size(505, 321)
        'Me.IkDisp1.TabIndex = 75
        'Me.IkDisp1.Text = "ImageKit1"
        'Me.IkDisp1.ToolTip = Newtone.ImageKit.Win.LengthUnit.None
        'Me.IkDisp1.TransparentBlue = CType(0, Byte)
        'Me.IkDisp1.TransparentGreen = CType(0, Byte)
        'Me.IkDisp1.TransparentRed = CType(0, Byte)
        '
        'picBorder
        '
        Me.picBorder.BackColor = System.Drawing.SystemColors.Control
        Me.picBorder.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.picBorder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.picBorder.Location = New System.Drawing.Point(8, 16)
        Me.picBorder.Name = "picBorder"
        Me.picBorder.Size = New System.Drawing.Size(505, 313)
        Me.picBorder.TabIndex = 44
        Me.picBorder.TabStop = False
        '
        '_tabMain_TabPage3
        '
        Me._tabMain_TabPage3.Controls.Add(Me.imgSirius)
        Me._tabMain_TabPage3.Controls.Add(Me.FraRecipient)
        Me._tabMain_TabPage3.Controls.Add(Me.chkEnableTask)
        Me._tabMain_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage3.Name = "_tabMain_TabPage3"
        Me._tabMain_TabPage3.Size = New System.Drawing.Size(597, 363)
        Me._tabMain_TabPage3.TabIndex = 3
        Me._tabMain_TabPage3.Text = "4 - Sirius Back-office"
        Me._tabMain_TabPage3.UseVisualStyleBackColor = True
        '
        'imgSirius
        '
        Me.imgSirius.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgSirius.Image = CType(resources.GetObject("imgSirius.Image"), System.Drawing.Image)
        Me.imgSirius.Location = New System.Drawing.Point(533, 20)
        Me.imgSirius.Name = "imgSirius"
        Me.imgSirius.Size = New System.Drawing.Size(32, 32)
        Me.imgSirius.TabIndex = 0
        Me.imgSirius.TabStop = False
        '
        'FraRecipient
        '
        Me.FraRecipient.BackColor = System.Drawing.SystemColors.Control
        Me.FraRecipient.Controls.Add(Me.cmdTaskDDReset)
        Me.FraRecipient.Controls.Add(Me.txtTaskDD)
        Me.FraRecipient.Controls.Add(Me.cboCustomer)
        Me.FraRecipient.Controls.Add(Me.chkUrgent)
        Me.FraRecipient.Controls.Add(Me.cboPMUserGroupByTask)
        Me.FraRecipient.Controls.Add(Me.cboPMUserLookup)
        Me.FraRecipient.Controls.Add(Me.Label4)
        Me.FraRecipient.Controls.Add(Me.Label3)
        Me.FraRecipient.Controls.Add(Me.Label1)
        Me.FraRecipient.Controls.Add(Me.Label2)
        Me.FraRecipient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FraRecipient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FraRecipient.Location = New System.Drawing.Point(8, 60)
        Me.FraRecipient.Name = "FraRecipient"
        Me.FraRecipient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FraRecipient.Size = New System.Drawing.Size(481, 153)
        Me.FraRecipient.TabIndex = 49
        Me.FraRecipient.TabStop = False
        Me.FraRecipient.Text = "Scheduled Task"
        '
        'cmdTaskDDReset
        '
        Me.cmdTaskDDReset.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdTaskDDReset, True)
        Me.cmdTaskDDReset.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdTaskDDReset, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdTaskDDReset, Nothing)
        Me.cmdTaskDDReset.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTaskDDReset.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdTaskDDReset.Location = New System.Drawing.Point(329, 115)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdTaskDDReset, System.Drawing.Color.Silver)
        Me.cmdTaskDDReset.Name = "cmdTaskDDReset"
        Me.cmdTaskDDReset.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdTaskDDReset.Size = New System.Drawing.Size(73, 19)
        Me.commandButtonHelper1.SetStyle(Me.cmdTaskDDReset, 0)
        Me.cmdTaskDDReset.TabIndex = 55
        Me.cmdTaskDDReset.Text = "Reset"
        Me.cmdTaskDDReset.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdTaskDDReset.UseVisualStyleBackColor = False
        '
        'txtTaskDD
        '
        Me.txtTaskDD.AcceptsReturn = True
        Me.txtTaskDD.BackColor = System.Drawing.SystemColors.Window
        Me.txtTaskDD.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTaskDD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTaskDD.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTaskDD.Location = New System.Drawing.Point(113, 115)
        Me.txtTaskDD.MaxLength = 0
        Me.txtTaskDD.Name = "txtTaskDD"
        Me.txtTaskDD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTaskDD.Size = New System.Drawing.Size(209, 20)
        Me.txtTaskDD.TabIndex = 54
        '
        'cboCustomer
        '
        Me.cboCustomer.BackColor = System.Drawing.SystemColors.Window
        Me.cboCustomer.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.cboCustomer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCustomer.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboCustomer, New Integer(-1) {})
        Me.cboCustomer.Location = New System.Drawing.Point(113, 86)
        Me.cboCustomer.Name = "cboCustomer"
        Me.cboCustomer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCustomer.Size = New System.Drawing.Size(210, 21)
        Me.cboCustomer.TabIndex = 53
        '
        'chkUrgent
        '
        Me.chkUrgent.BackColor = System.Drawing.SystemColors.Control
        Me.chkUrgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkUrgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUrgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkUrgent.Location = New System.Drawing.Point(417, 117)
        Me.chkUrgent.Name = "chkUrgent"
        Me.chkUrgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkUrgent.Size = New System.Drawing.Size(58, 17)
        Me.chkUrgent.TabIndex = 50
        Me.chkUrgent.Text = "Urgent"
        Me.chkUrgent.UseVisualStyleBackColor = False
        '
        'cboPMUserGroupByTask
        '
        Me.cboPMUserGroupByTask.DefaultTaskGroupID = 0
        Me.cboPMUserGroupByTask.FirstItem = ""
        Me.cboPMUserGroupByTask.ListIndex = -1
        Me.cboPMUserGroupByTask.Location = New System.Drawing.Point(113, 58)
        Me.cboPMUserGroupByTask.Name = "cboPMUserGroupByTask"
        Me.cboPMUserGroupByTask.PMTaskGroupID = 0
        Me.cboPMUserGroupByTask.SingleUserGroupID = 0
        Me.cboPMUserGroupByTask.Size = New System.Drawing.Size(210, 21)
        Me.cboPMUserGroupByTask.Sorted = True
        Me.cboPMUserGroupByTask.TabIndex = 51
        Me.cboPMUserGroupByTask.ToolTipText = ""
        Me.cboPMUserGroupByTask.UserGroupID = 0
        '
        'cboPMUserLookup
        '
        Me.cboPMUserLookup.DefaultUserID = 0
        Me.cboPMUserLookup.FirstItem = ""
        Me.cboPMUserLookup.ListIndex = -1
        Me.cboPMUserLookup.Location = New System.Drawing.Point(113, 30)
        Me.cboPMUserLookup.Name = "cboPMUserLookup"
        Me.cboPMUserLookup.PMUserGroupID = 0
        Me.cboPMUserLookup.SingleUserID = 0
        Me.cboPMUserLookup.Size = New System.Drawing.Size(210, 21)
        Me.cboPMUserLookup.Sorted = True
        Me.cboPMUserLookup.TabIndex = 52
        Me.cboPMUserLookup.ToolTipText = ""
        Me.cboPMUserLookup.UserID = 0
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(14, 117)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(93, 13)
        Me.Label4.TabIndex = 59
        Me.Label4.Text = "Task Due Date"
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(14, 58)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(77, 14)
        Me.Label3.TabIndex = 58
        Me.Label3.Text = "User Group"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(14, 87)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(71, 18)
        Me.Label1.TabIndex = 57
        Me.Label1.Text = "Customer"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(14, 30)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(77, 14)
        Me.Label2.TabIndex = 56
        Me.Label2.Text = "User Name"
        '
        'chkEnableTask
        '
        Me.chkEnableTask.BackColor = System.Drawing.SystemColors.Control
        Me.chkEnableTask.Checked = True
        Me.chkEnableTask.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkEnableTask.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkEnableTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkEnableTask.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkEnableTask.Location = New System.Drawing.Point(16, 20)
        Me.chkEnableTask.Name = "chkEnableTask"
        Me.chkEnableTask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkEnableTask.Size = New System.Drawing.Size(169, 33)
        Me.chkEnableTask.TabIndex = 61
        Me.chkEnableTask.Text = "Enable Task Scheduler"
        Me.chkEnableTask.UseVisualStyleBackColor = False
        '
        'tmrDisplay
        '
        Me.tmrDisplay.Interval = 1
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdScan
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(619, 488)
        Me.Controls.Add(Me.cmdViewBatch)
        Me.Controls.Add(Me.cmdScanNext)
        Me.Controls.Add(Me.cmdReScan)
        Me.Controls.Add(Me.cmdScan)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.statMainbar)
        Me.Controls.Add(Me.tabMain)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(-22, 16)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Document Scan"
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.statMainbar.ResumeLayout(False)
        Me.statMainbar.PerformLayout()
        Me.tabMain.ResumeLayout(False)
        Me._tabMain_TabPage0.ResumeLayout(False)
        CType(Me.imgMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraUser.ResumeLayout(False)
        Me.fraUser.PerformLayout()
        Me.fraDocInfo.ResumeLayout(False)
        Me.fraDocInfo.PerformLayout()
        Me.Frame7.ResumeLayout(False)
        Me._tabMain_TabPage1.ResumeLayout(False)
        CType(Me.imgOptions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraDateInformation.ResumeLayout(False)
        Me.fraDateInformation.PerformLayout()
        Me.fraAdvOptions.ResumeLayout(False)
        Me.fraAdvOptions.PerformLayout()
        Me.fraMultiPage.ResumeLayout(False)
        Me.fraDocName.ResumeLayout(False)
        CType(Me.UpDown1, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMain_TabPage2.ResumeLayout(False)
        Me.fraView.ResumeLayout(False)
        CType(Me.picBorder, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMain_TabPage3.ResumeLayout(False)
        CType(Me.imgSirius, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FraRecipient.ResumeLayout(False)
        Me.FraRecipient.PerformLayout()
        CType(Me.commandButtonHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub InitializemnuTab()
		Me.mnuTab(0) = _mnuTab_0
		Me.mnuTab(1) = _mnuTab_1
		Me.mnuTab(2) = _mnuTab_2
	End Sub
	Sub InitializecmdView()
		Me.cmdView(2) = _cmdView_2
		Me.cmdView(1) = _cmdView_1
		Me.cmdView(0) = _cmdView_0
    End Sub
  
#End Region 
End Class