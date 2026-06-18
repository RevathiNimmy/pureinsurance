<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializefraRTF()
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
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents chkWANOptimise As System.Windows.Forms.CheckBox
	Public WithEvents fraWAN As System.Windows.Forms.GroupBox
	Public WithEvents cmdDefaultManager As System.Windows.Forms.Button
	Public WithEvents cboMaxAuto As System.Windows.Forms.ComboBox
	Public WithEvents cboMaxFolders As System.Windows.Forms.ComboBox
	Public WithEvents cboMaxFilter As System.Windows.Forms.ComboBox
	Public WithEvents lblMaxAuto As System.Windows.Forms.Label
	Public WithEvents lblMaxFolders As System.Windows.Forms.Label
	Public WithEvents lblMaxFilter As System.Windows.Forms.Label
	Public WithEvents fraLimits As System.Windows.Forms.GroupBox
	Public WithEvents chkHomeFolder As System.Windows.Forms.CheckBox
	Public WithEvents chkDisplayFolders As System.Windows.Forms.CheckBox
	Public WithEvents fraDisplay As System.Windows.Forms.GroupBox
	Public WithEvents imgNavigation As System.Windows.Forms.PictureBox
	Private WithEvents _tabOptions_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents cmdDefaultViewer As System.Windows.Forms.Button
	Public WithEvents chkAllowCopyPaste As System.Windows.Forms.CheckBox
	Public WithEvents Frame2 As System.Windows.Forms.GroupBox
	Private WithEvents _tabOptions_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents txtShare As System.Windows.Forms.TextBox
	Public WithEvents cmdShareBrowse As System.Windows.Forms.Button
	Public WithEvents fraDefDoc As System.Windows.Forms.GroupBox
	Public WithEvents txtCacheLocation As System.Windows.Forms.TextBox
	Public WithEvents cmdCacheBrowse As System.Windows.Forms.Button
	Public WithEvents lblCache As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents cmdDefaultConfig As System.Windows.Forms.Button
	Public WithEvents chkPrintW As System.Windows.Forms.CheckBox
	Public WithEvents chkViewW As System.Windows.Forms.CheckBox
	Public WithEvents chkAutoKeyword As System.Windows.Forms.CheckBox
	Private WithEvents _fraRTF_0 As System.Windows.Forms.GroupBox
	Public WithEvents imgConfiguration As System.Windows.Forms.PictureBox
	Private WithEvents _tabOptions_TabPage2 As System.Windows.Forms.TabPage
	Public WithEvents txtDocumentExpiry As System.Windows.Forms.TextBox
	Public WithEvents cboDocumentDate As System.Windows.Forms.ComboBox
    'Public WithEvents updDocDate As AxComCtl2.AxUpDown
	Public WithEvents lblDocumentExpiry As System.Windows.Forms.Label
	Public WithEvents lblDocumentDate As System.Windows.Forms.Label
	Public WithEvents lblMinus As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents lblDays As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents fraScanSettings As System.Windows.Forms.GroupBox
	Public WithEvents cmdDeafaultDates As System.Windows.Forms.Button
	Public WithEvents chkImageViewer As System.Windows.Forms.CheckBox
	Public WithEvents fraImageViewer As System.Windows.Forms.GroupBox
	Public WithEvents imgDocument As System.Windows.Forms.PictureBox
	Private WithEvents _tabOptions_TabPage3 As System.Windows.Forms.TabPage
	Public WithEvents chkScanExternal As System.Windows.Forms.CheckBox
	Public WithEvents chkMoveV2 As System.Windows.Forms.CheckBox
	Public WithEvents fraWarnIf As System.Windows.Forms.GroupBox
	Public WithEvents cmdDefaultWarnings As System.Windows.Forms.Button
	Public WithEvents imgWarnings As System.Windows.Forms.PictureBox
	Private WithEvents _tabOptions_TabPage4 As System.Windows.Forms.TabPage
	Public WithEvents tabOptions As System.Windows.Forms.TabControl
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdApply As System.Windows.Forms.Button
	Public fraRTF(0) As System.Windows.Forms.GroupBox
	Private WithEvents listBoxComboBoxHelper1 As Artinsoft.VB6.Gui.ListControlHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabOptions = New System.Windows.Forms.TabControl
        Me._tabOptions_TabPage0 = New System.Windows.Forms.TabPage
        Me.fraWAN = New System.Windows.Forms.GroupBox
        Me.chkWANOptimise = New System.Windows.Forms.CheckBox
        Me.cmdDefaultManager = New System.Windows.Forms.Button
        Me.fraLimits = New System.Windows.Forms.GroupBox
        Me.cboMaxAuto = New System.Windows.Forms.ComboBox
        Me.cboMaxFolders = New System.Windows.Forms.ComboBox
        Me.cboMaxFilter = New System.Windows.Forms.ComboBox
        Me.lblMaxAuto = New System.Windows.Forms.Label
        Me.lblMaxFolders = New System.Windows.Forms.Label
        Me.lblMaxFilter = New System.Windows.Forms.Label
        Me.fraDisplay = New System.Windows.Forms.GroupBox
        Me.chkHomeFolder = New System.Windows.Forms.CheckBox
        Me.chkDisplayFolders = New System.Windows.Forms.CheckBox
        Me.imgNavigation = New System.Windows.Forms.PictureBox
        Me._tabOptions_TabPage1 = New System.Windows.Forms.TabPage
        Me.cmdDefaultViewer = New System.Windows.Forms.Button
        Me.Frame2 = New System.Windows.Forms.GroupBox
        Me.chkAllowCopyPaste = New System.Windows.Forms.CheckBox
        Me._tabOptions_TabPage2 = New System.Windows.Forms.TabPage
        Me.fraDefDoc = New System.Windows.Forms.GroupBox
        Me.txtShare = New System.Windows.Forms.TextBox
        Me.cmdShareBrowse = New System.Windows.Forms.Button
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.txtCacheLocation = New System.Windows.Forms.TextBox
        Me.cmdCacheBrowse = New System.Windows.Forms.Button
        Me.lblCache = New System.Windows.Forms.Label
        Me.cmdDefaultConfig = New System.Windows.Forms.Button
        Me._fraRTF_0 = New System.Windows.Forms.GroupBox
        Me.chkPrintW = New System.Windows.Forms.CheckBox
        Me.chkViewW = New System.Windows.Forms.CheckBox
        Me.chkAutoKeyword = New System.Windows.Forms.CheckBox
        Me.imgConfiguration = New System.Windows.Forms.PictureBox
        Me._tabOptions_TabPage3 = New System.Windows.Forms.TabPage
        Me.fraScanSettings = New System.Windows.Forms.GroupBox
        Me.txtDocumentExpiry = New System.Windows.Forms.TextBox
        Me.cboDocumentDate = New System.Windows.Forms.ComboBox
        Me.lblDocumentExpiry = New System.Windows.Forms.Label
        Me.lblDocumentDate = New System.Windows.Forms.Label
        Me.lblMinus = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblDays = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmdDeafaultDates = New System.Windows.Forms.Button
        Me.fraImageViewer = New System.Windows.Forms.GroupBox
        Me.chkImageViewer = New System.Windows.Forms.CheckBox
        Me.imgDocument = New System.Windows.Forms.PictureBox
        Me._tabOptions_TabPage4 = New System.Windows.Forms.TabPage
        Me.fraWarnIf = New System.Windows.Forms.GroupBox
        Me.chkScanExternal = New System.Windows.Forms.CheckBox
        Me.chkMoveV2 = New System.Windows.Forms.CheckBox
        Me.cmdDefaultWarnings = New System.Windows.Forms.Button
        Me.imgWarnings = New System.Windows.Forms.PictureBox
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdApply = New System.Windows.Forms.Button
        Me.listBoxComboBoxHelper1 = New Artinsoft.VB6.Gui.ListControlHelper(Me.components)
        Me.tabOptions.SuspendLayout()
        Me._tabOptions_TabPage0.SuspendLayout()
        Me.fraWAN.SuspendLayout()
        Me.fraLimits.SuspendLayout()
        Me.fraDisplay.SuspendLayout()
        CType(Me.imgNavigation, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabOptions_TabPage1.SuspendLayout()
        Me.Frame2.SuspendLayout()
        Me._tabOptions_TabPage2.SuspendLayout()
        Me.fraDefDoc.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me._fraRTF_0.SuspendLayout()
        CType(Me.imgConfiguration, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabOptions_TabPage3.SuspendLayout()
        Me.fraScanSettings.SuspendLayout()
        Me.fraImageViewer.SuspendLayout()
        CType(Me.imgDocument, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabOptions_TabPage4.SuspendLayout()
        Me.fraWarnIf.SuspendLayout()
        CType(Me.imgWarnings, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Enabled = False
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(120, 304)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 17
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabOptions
        '
        Me.tabOptions.Controls.Add(Me._tabOptions_TabPage0)
        Me.tabOptions.Controls.Add(Me._tabOptions_TabPage1)
        Me.tabOptions.Controls.Add(Me._tabOptions_TabPage2)
        Me.tabOptions.Controls.Add(Me._tabOptions_TabPage3)
        Me.tabOptions.Controls.Add(Me._tabOptions_TabPage4)
        Me.tabOptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabOptions.ItemSize = New System.Drawing.Size(92, 18)
        Me.tabOptions.Location = New System.Drawing.Point(8, 8)
        Me.tabOptions.Multiline = True
        Me.tabOptions.Name = "tabOptions"
        Me.tabOptions.SelectedIndex = 0
        Me.tabOptions.Size = New System.Drawing.Size(480, 293)
        Me.tabOptions.TabIndex = 6
        '
        '_tabOptions_TabPage0
        '
        Me._tabOptions_TabPage0.Controls.Add(Me.fraWAN)
        Me._tabOptions_TabPage0.Controls.Add(Me.cmdDefaultManager)
        Me._tabOptions_TabPage0.Controls.Add(Me.fraLimits)
        Me._tabOptions_TabPage0.Controls.Add(Me.fraDisplay)
        Me._tabOptions_TabPage0.Controls.Add(Me.imgNavigation)
        Me._tabOptions_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabOptions_TabPage0.Name = "_tabOptions_TabPage0"
        Me._tabOptions_TabPage0.Size = New System.Drawing.Size(472, 267)
        Me._tabOptions_TabPage0.TabIndex = 0
        Me._tabOptions_TabPage0.Text = "1 - Manager"
        Me._tabOptions_TabPage0.UseVisualStyleBackColor = True
        '
        'fraWAN
        '
        Me.fraWAN.BackColor = System.Drawing.SystemColors.Control
        Me.fraWAN.Controls.Add(Me.chkWANOptimise)
        Me.fraWAN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraWAN.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraWAN.Location = New System.Drawing.Point(296, 168)
        Me.fraWAN.Name = "fraWAN"
        Me.fraWAN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraWAN.Size = New System.Drawing.Size(153, 73)
        Me.fraWAN.TabIndex = 15
        Me.fraWAN.TabStop = False
        '
        'chkWANOptimise
        '
        Me.chkWANOptimise.BackColor = System.Drawing.SystemColors.Control
        Me.chkWANOptimise.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkWANOptimise.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkWANOptimise.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkWANOptimise.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkWANOptimise.Location = New System.Drawing.Point(16, 20)
        Me.chkWANOptimise.Name = "chkWANOptimise"
        Me.chkWANOptimise.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkWANOptimise.Size = New System.Drawing.Size(121, 17)
        Me.chkWANOptimise.TabIndex = 16
        Me.chkWANOptimise.Text = "Optimise for WAN"
        Me.chkWANOptimise.UseVisualStyleBackColor = False
        '
        'cmdDefaultManager
        '
        Me.cmdDefaultManager.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDefaultManager.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDefaultManager.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDefaultManager.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDefaultManager.Location = New System.Drawing.Point(376, 139)
        Me.cmdDefaultManager.Name = "cmdDefaultManager"
        Me.cmdDefaultManager.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDefaultManager.Size = New System.Drawing.Size(73, 22)
        Me.cmdDefaultManager.TabIndex = 2
        Me.cmdDefaultManager.Text = "&Defaults"
        Me.cmdDefaultManager.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDefaultManager.UseVisualStyleBackColor = False
        '
        'fraLimits
        '
        Me.fraLimits.BackColor = System.Drawing.SystemColors.Control
        Me.fraLimits.Controls.Add(Me.cboMaxAuto)
        Me.fraLimits.Controls.Add(Me.cboMaxFolders)
        Me.fraLimits.Controls.Add(Me.cboMaxFilter)
        Me.fraLimits.Controls.Add(Me.lblMaxAuto)
        Me.fraLimits.Controls.Add(Me.lblMaxFolders)
        Me.fraLimits.Controls.Add(Me.lblMaxFilter)
        Me.fraLimits.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraLimits.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraLimits.Location = New System.Drawing.Point(16, 32)
        Me.fraLimits.Name = "fraLimits"
        Me.fraLimits.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraLimits.Size = New System.Drawing.Size(345, 129)
        Me.fraLimits.TabIndex = 8
        Me.fraLimits.TabStop = False
        Me.fraLimits.Text = "Limits"
        '
        'cboMaxAuto
        '
        Me.cboMaxAuto.BackColor = System.Drawing.SystemColors.Window
        Me.cboMaxAuto.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboMaxAuto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMaxAuto.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboMaxAuto, New Integer() {0, 0, 0, 0, 0, 0})
        Me.cboMaxAuto.Items.AddRange(New Object() {"50", "100", "500", "1000", "1500", "2000"})
        Me.cboMaxAuto.Location = New System.Drawing.Point(216, 88)
        Me.cboMaxAuto.Name = "cboMaxAuto"
        Me.cboMaxAuto.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboMaxAuto.Size = New System.Drawing.Size(49, 21)
        Me.cboMaxAuto.TabIndex = 14
        Me.cboMaxAuto.Text = "50"
        '
        'cboMaxFolders
        '
        Me.cboMaxFolders.BackColor = System.Drawing.SystemColors.Window
        Me.cboMaxFolders.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboMaxFolders.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMaxFolders.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboMaxFolders, New Integer() {0, 0, 0, 0, 0, 0, 0})
        Me.cboMaxFolders.Items.AddRange(New Object() {"10", "20", "30", "40", "50", "100", "All"})
        Me.cboMaxFolders.Location = New System.Drawing.Point(216, 24)
        Me.cboMaxFolders.Name = "cboMaxFolders"
        Me.cboMaxFolders.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboMaxFolders.Size = New System.Drawing.Size(49, 21)
        Me.cboMaxFolders.TabIndex = 0
        Me.cboMaxFolders.Text = "All"
        '
        'cboMaxFilter
        '
        Me.cboMaxFilter.BackColor = System.Drawing.SystemColors.Window
        Me.cboMaxFilter.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboMaxFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMaxFilter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboMaxFilter, New Integer() {0, 0, 0, 0, 0, 0, 0})
        Me.cboMaxFilter.Items.AddRange(New Object() {"10", "20", "30", "40", "50", "100", "All"})
        Me.cboMaxFilter.Location = New System.Drawing.Point(216, 56)
        Me.cboMaxFilter.Name = "cboMaxFilter"
        Me.cboMaxFilter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboMaxFilter.Size = New System.Drawing.Size(49, 21)
        Me.cboMaxFilter.TabIndex = 1
        Me.cboMaxFilter.Text = "50"
        '
        'lblMaxAuto
        '
        Me.lblMaxAuto.AutoSize = True
        Me.lblMaxAuto.BackColor = System.Drawing.SystemColors.Control
        Me.lblMaxAuto.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMaxAuto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMaxAuto.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMaxAuto.Location = New System.Drawing.Point(16, 92)
        Me.lblMaxAuto.Name = "lblMaxAuto"
        Me.lblMaxAuto.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMaxAuto.Size = New System.Drawing.Size(152, 13)
        Me.lblMaxAuto.TabIndex = 13
        Me.lblMaxAuto.Text = "Maximum Folders Auto-Expand"
        '
        'lblMaxFolders
        '
        Me.lblMaxFolders.AutoSize = True
        Me.lblMaxFolders.BackColor = System.Drawing.SystemColors.Control
        Me.lblMaxFolders.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMaxFolders.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMaxFolders.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMaxFolders.Location = New System.Drawing.Point(16, 28)
        Me.lblMaxFolders.Name = "lblMaxFolders"
        Me.lblMaxFolders.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMaxFolders.Size = New System.Drawing.Size(135, 13)
        Me.lblMaxFolders.TabIndex = 10
        Me.lblMaxFolders.Text = "Maximum Folders Returned"
        '
        'lblMaxFilter
        '
        Me.lblMaxFilter.AutoSize = True
        Me.lblMaxFilter.BackColor = System.Drawing.SystemColors.Control
        Me.lblMaxFilter.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMaxFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMaxFilter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMaxFilter.Location = New System.Drawing.Point(16, 60)
        Me.lblMaxFilter.Name = "lblMaxFilter"
        Me.lblMaxFilter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMaxFilter.Size = New System.Drawing.Size(160, 13)
        Me.lblMaxFilter.TabIndex = 9
        Me.lblMaxFilter.Text = "Filter Maximum Folders Returned"
        '
        'fraDisplay
        '
        Me.fraDisplay.BackColor = System.Drawing.SystemColors.Control
        Me.fraDisplay.Controls.Add(Me.chkHomeFolder)
        Me.fraDisplay.Controls.Add(Me.chkDisplayFolders)
        Me.fraDisplay.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDisplay.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDisplay.Location = New System.Drawing.Point(16, 168)
        Me.fraDisplay.Name = "fraDisplay"
        Me.fraDisplay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDisplay.Size = New System.Drawing.Size(273, 73)
        Me.fraDisplay.TabIndex = 7
        Me.fraDisplay.TabStop = False
        Me.fraDisplay.Text = "Display"
        '
        'chkHomeFolder
        '
        Me.chkHomeFolder.BackColor = System.Drawing.SystemColors.Control
        Me.chkHomeFolder.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkHomeFolder.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkHomeFolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkHomeFolder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkHomeFolder.Location = New System.Drawing.Point(16, 20)
        Me.chkHomeFolder.Name = "chkHomeFolder"
        Me.chkHomeFolder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkHomeFolder.Size = New System.Drawing.Size(241, 17)
        Me.chkHomeFolder.TabIndex = 12
        Me.chkHomeFolder.Text = "Start in Home Folder"
        Me.chkHomeFolder.UseVisualStyleBackColor = False
        '
        'chkDisplayFolders
        '
        Me.chkDisplayFolders.BackColor = System.Drawing.SystemColors.Control
        Me.chkDisplayFolders.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDisplayFolders.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDisplayFolders.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDisplayFolders.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDisplayFolders.Location = New System.Drawing.Point(16, 40)
        Me.chkDisplayFolders.Name = "chkDisplayFolders"
        Me.chkDisplayFolders.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDisplayFolders.Size = New System.Drawing.Size(241, 17)
        Me.chkDisplayFolders.TabIndex = 11
        Me.chkDisplayFolders.Text = "Display Folders in Document Window"
        Me.chkDisplayFolders.UseVisualStyleBackColor = False
        '
        'imgNavigation
        '
        Me.imgNavigation.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgNavigation.Image = CType(resources.GetObject("imgNavigation.Image"), System.Drawing.Image)
        Me.imgNavigation.Location = New System.Drawing.Point(408, 40)
        Me.imgNavigation.Name = "imgNavigation"
        Me.imgNavigation.Size = New System.Drawing.Size(32, 32)
        Me.imgNavigation.TabIndex = 16
        Me.imgNavigation.TabStop = False
        '
        '_tabOptions_TabPage1
        '
        Me._tabOptions_TabPage1.Controls.Add(Me.cmdDefaultViewer)
        Me._tabOptions_TabPage1.Controls.Add(Me.Frame2)
        Me._tabOptions_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabOptions_TabPage1.Name = "_tabOptions_TabPage1"
        Me._tabOptions_TabPage1.Size = New System.Drawing.Size(472, 267)
        Me._tabOptions_TabPage1.TabIndex = 1
        Me._tabOptions_TabPage1.Text = "2 - Viewer"
        Me._tabOptions_TabPage1.UseVisualStyleBackColor = True
        '
        'cmdDefaultViewer
        '
        Me.cmdDefaultViewer.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDefaultViewer.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDefaultViewer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDefaultViewer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDefaultViewer.Location = New System.Drawing.Point(376, 219)
        Me.cmdDefaultViewer.Name = "cmdDefaultViewer"
        Me.cmdDefaultViewer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDefaultViewer.Size = New System.Drawing.Size(73, 22)
        Me.cmdDefaultViewer.TabIndex = 47
        Me.cmdDefaultViewer.Text = "&Defaults"
        Me.cmdDefaultViewer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDefaultViewer.UseVisualStyleBackColor = False
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.chkAllowCopyPaste)
        Me.Frame2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(16, 32)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(345, 69)
        Me.Frame2.TabIndex = 48
        Me.Frame2.TabStop = False
        Me.Frame2.Text = "Word documents"
        '
        'chkAllowCopyPaste
        '
        Me.chkAllowCopyPaste.BackColor = System.Drawing.SystemColors.Control
        Me.chkAllowCopyPaste.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAllowCopyPaste.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAllowCopyPaste.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAllowCopyPaste.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAllowCopyPaste.Location = New System.Drawing.Point(20, 32)
        Me.chkAllowCopyPaste.Name = "chkAllowCopyPaste"
        Me.chkAllowCopyPaste.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllowCopyPaste.Size = New System.Drawing.Size(241, 17)
        Me.chkAllowCopyPaste.TabIndex = 49
        Me.chkAllowCopyPaste.Text = "Allow Copy && Paste"
        Me.chkAllowCopyPaste.UseVisualStyleBackColor = False
        '
        '_tabOptions_TabPage2
        '
        Me._tabOptions_TabPage2.Controls.Add(Me.fraDefDoc)
        Me._tabOptions_TabPage2.Controls.Add(Me.Frame1)
        Me._tabOptions_TabPage2.Controls.Add(Me.cmdDefaultConfig)
        Me._tabOptions_TabPage2.Controls.Add(Me._fraRTF_0)
        Me._tabOptions_TabPage2.Controls.Add(Me.imgConfiguration)
        Me._tabOptions_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabOptions_TabPage2.Name = "_tabOptions_TabPage2"
        Me._tabOptions_TabPage2.Size = New System.Drawing.Size(472, 267)
        Me._tabOptions_TabPage2.TabIndex = 2
        Me._tabOptions_TabPage2.Text = "3 - Configuration"
        Me._tabOptions_TabPage2.UseVisualStyleBackColor = True
        '
        'fraDefDoc
        '
        Me.fraDefDoc.BackColor = System.Drawing.SystemColors.Control
        Me.fraDefDoc.Controls.Add(Me.txtShare)
        Me.fraDefDoc.Controls.Add(Me.cmdShareBrowse)
        Me.fraDefDoc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDefDoc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDefDoc.Location = New System.Drawing.Point(16, 108)
        Me.fraDefDoc.Name = "fraDefDoc"
        Me.fraDefDoc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDefDoc.Size = New System.Drawing.Size(345, 49)
        Me.fraDefDoc.TabIndex = 44
        Me.fraDefDoc.TabStop = False
        Me.fraDefDoc.Text = "DocuMaster Document Store"
        '
        'txtShare
        '
        Me.txtShare.AcceptsReturn = True
        Me.txtShare.BackColor = System.Drawing.SystemColors.Window
        Me.txtShare.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtShare.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtShare.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtShare.Location = New System.Drawing.Point(24, 16)
        Me.txtShare.MaxLength = 0
        Me.txtShare.Name = "txtShare"
        Me.txtShare.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtShare.Size = New System.Drawing.Size(273, 20)
        Me.txtShare.TabIndex = 46
        Me.txtShare.Tag = "admin"
        '
        'cmdShareBrowse
        '
        Me.cmdShareBrowse.BackColor = System.Drawing.SystemColors.Control
        Me.cmdShareBrowse.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdShareBrowse.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdShareBrowse.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdShareBrowse.Location = New System.Drawing.Point(304, 16)
        Me.cmdShareBrowse.Name = "cmdShareBrowse"
        Me.cmdShareBrowse.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdShareBrowse.Size = New System.Drawing.Size(17, 19)
        Me.cmdShareBrowse.TabIndex = 45
        Me.cmdShareBrowse.Text = "..."
        Me.cmdShareBrowse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdShareBrowse.UseVisualStyleBackColor = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.txtCacheLocation)
        Me.Frame1.Controls.Add(Me.cmdCacheBrowse)
        Me.Frame1.Controls.Add(Me.lblCache)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(16, 32)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(345, 65)
        Me.Frame1.TabIndex = 40
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Cache"
        '
        'txtCacheLocation
        '
        Me.txtCacheLocation.AcceptsReturn = True
        Me.txtCacheLocation.BackColor = System.Drawing.SystemColors.Window
        Me.txtCacheLocation.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCacheLocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCacheLocation.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCacheLocation.Location = New System.Drawing.Point(104, 24)
        Me.txtCacheLocation.MaxLength = 0
        Me.txtCacheLocation.Name = "txtCacheLocation"
        Me.txtCacheLocation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCacheLocation.Size = New System.Drawing.Size(193, 20)
        Me.txtCacheLocation.TabIndex = 42
        '
        'cmdCacheBrowse
        '
        Me.cmdCacheBrowse.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCacheBrowse.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCacheBrowse.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCacheBrowse.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCacheBrowse.Location = New System.Drawing.Point(304, 24)
        Me.cmdCacheBrowse.Name = "cmdCacheBrowse"
        Me.cmdCacheBrowse.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCacheBrowse.Size = New System.Drawing.Size(17, 19)
        Me.cmdCacheBrowse.TabIndex = 41
        Me.cmdCacheBrowse.Text = "..."
        Me.cmdCacheBrowse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCacheBrowse.UseVisualStyleBackColor = False
        '
        'lblCache
        '
        Me.lblCache.AutoSize = True
        Me.lblCache.BackColor = System.Drawing.SystemColors.Control
        Me.lblCache.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCache.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCache.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCache.Location = New System.Drawing.Point(16, 27)
        Me.lblCache.Name = "lblCache"
        Me.lblCache.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCache.Size = New System.Drawing.Size(78, 13)
        Me.lblCache.TabIndex = 43
        Me.lblCache.Text = "Cache location"
        '
        'cmdDefaultConfig
        '
        Me.cmdDefaultConfig.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDefaultConfig.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDefaultConfig.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDefaultConfig.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDefaultConfig.Location = New System.Drawing.Point(372, 219)
        Me.cmdDefaultConfig.Name = "cmdDefaultConfig"
        Me.cmdDefaultConfig.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDefaultConfig.Size = New System.Drawing.Size(73, 22)
        Me.cmdDefaultConfig.TabIndex = 39
        Me.cmdDefaultConfig.Text = "&Defaults"
        Me.cmdDefaultConfig.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDefaultConfig.UseVisualStyleBackColor = False
        '
        '_fraRTF_0
        '
        Me._fraRTF_0.BackColor = System.Drawing.SystemColors.Control
        Me._fraRTF_0.Controls.Add(Me.chkPrintW)
        Me._fraRTF_0.Controls.Add(Me.chkViewW)
        Me._fraRTF_0.Controls.Add(Me.chkAutoKeyword)
        Me._fraRTF_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraRTF_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraRTF_0.Location = New System.Drawing.Point(16, 168)
        Me._fraRTF_0.Name = "_fraRTF_0"
        Me._fraRTF_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraRTF_0.Size = New System.Drawing.Size(345, 96)
        Me._fraRTF_0.TabIndex = 35
        Me._fraRTF_0.TabStop = False
        Me._fraRTF_0.Text = "File Options"
        '
        'chkPrintW
        '
        Me.chkPrintW.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrintW.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkPrintW.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPrintW.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrintW.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrintW.Location = New System.Drawing.Point(16, 18)
        Me.chkPrintW.Name = "chkPrintW"
        Me.chkPrintW.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPrintW.Size = New System.Drawing.Size(173, 19)
        Me.chkPrintW.TabIndex = 38
        Me.chkPrintW.Text = "Print using Word"
        Me.chkPrintW.UseVisualStyleBackColor = False
        '
        'chkViewW
        '
        Me.chkViewW.BackColor = System.Drawing.SystemColors.Control
        Me.chkViewW.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkViewW.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkViewW.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkViewW.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkViewW.Location = New System.Drawing.Point(16, 43)
        Me.chkViewW.Name = "chkViewW"
        Me.chkViewW.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkViewW.Size = New System.Drawing.Size(173, 22)
        Me.chkViewW.TabIndex = 37
        Me.chkViewW.Text = "View using Word"
        Me.chkViewW.UseVisualStyleBackColor = False
        '
        'chkAutoKeyword
        '
        Me.chkAutoKeyword.BackColor = System.Drawing.SystemColors.Control
        Me.chkAutoKeyword.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAutoKeyword.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAutoKeyword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAutoKeyword.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAutoKeyword.Location = New System.Drawing.Point(16, 64)
        Me.chkAutoKeyword.Name = "chkAutoKeyword"
        Me.chkAutoKeyword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAutoKeyword.Size = New System.Drawing.Size(173, 26)
        Me.chkAutoKeyword.TabIndex = 36
        Me.chkAutoKeyword.Text = "Auto Keyword/Annotations"
        Me.chkAutoKeyword.UseVisualStyleBackColor = False
        '
        'imgConfiguration
        '
        Me.imgConfiguration.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgConfiguration.Image = CType(resources.GetObject("imgConfiguration.Image"), System.Drawing.Image)
        Me.imgConfiguration.Location = New System.Drawing.Point(404, 40)
        Me.imgConfiguration.Name = "imgConfiguration"
        Me.imgConfiguration.Size = New System.Drawing.Size(32, 32)
        Me.imgConfiguration.TabIndex = 45
        Me.imgConfiguration.TabStop = False
        '
        '_tabOptions_TabPage3
        '
        Me._tabOptions_TabPage3.Controls.Add(Me.fraScanSettings)
        Me._tabOptions_TabPage3.Controls.Add(Me.cmdDeafaultDates)
        Me._tabOptions_TabPage3.Controls.Add(Me.fraImageViewer)
        Me._tabOptions_TabPage3.Controls.Add(Me.imgDocument)
        Me._tabOptions_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabOptions_TabPage3.Name = "_tabOptions_TabPage3"
        Me._tabOptions_TabPage3.Size = New System.Drawing.Size(472, 267)
        Me._tabOptions_TabPage3.TabIndex = 3
        Me._tabOptions_TabPage3.Text = "4 - Document Default Dates"
        Me._tabOptions_TabPage3.UseVisualStyleBackColor = True
        '
        'fraScanSettings
        '
        Me.fraScanSettings.BackColor = System.Drawing.SystemColors.Control
        Me.fraScanSettings.Controls.Add(Me.txtDocumentExpiry)
        Me.fraScanSettings.Controls.Add(Me.cboDocumentDate)
        Me.fraScanSettings.Controls.Add(Me.lblDocumentExpiry)
        Me.fraScanSettings.Controls.Add(Me.lblDocumentDate)
        Me.fraScanSettings.Controls.Add(Me.lblMinus)
        Me.fraScanSettings.Controls.Add(Me.Label1)
        Me.fraScanSettings.Controls.Add(Me.lblDays)
        Me.fraScanSettings.Controls.Add(Me.Label2)
        Me.fraScanSettings.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraScanSettings.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraScanSettings.Location = New System.Drawing.Point(8, 24)
        Me.fraScanSettings.Name = "fraScanSettings"
        Me.fraScanSettings.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraScanSettings.Size = New System.Drawing.Size(345, 97)
        Me.fraScanSettings.TabIndex = 25
        Me.fraScanSettings.TabStop = False
        Me.fraScanSettings.Text = "Scan Settings"
        '
        'txtDocumentExpiry
        '
        Me.txtDocumentExpiry.AcceptsReturn = True
        Me.txtDocumentExpiry.BackColor = System.Drawing.SystemColors.Window
        Me.txtDocumentExpiry.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDocumentExpiry.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDocumentExpiry.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDocumentExpiry.Location = New System.Drawing.Point(160, 56)
        Me.txtDocumentExpiry.MaxLength = 0
        Me.txtDocumentExpiry.Name = "txtDocumentExpiry"
        Me.txtDocumentExpiry.ReadOnly = True
        Me.txtDocumentExpiry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDocumentExpiry.Size = New System.Drawing.Size(33, 20)
        Me.txtDocumentExpiry.TabIndex = 28
        '
        'cboDocumentDate
        '
        Me.cboDocumentDate.BackColor = System.Drawing.SystemColors.Window
        Me.cboDocumentDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboDocumentDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDocumentDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboDocumentDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboDocumentDate, New Integer() {0, 0, 0, 0, 0})
        Me.cboDocumentDate.Items.AddRange(New Object() {"1", "2", "3", "4", "5"})
        Me.cboDocumentDate.Location = New System.Drawing.Point(160, 24)
        Me.cboDocumentDate.Name = "cboDocumentDate"
        Me.cboDocumentDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboDocumentDate.Size = New System.Drawing.Size(49, 21)
        Me.cboDocumentDate.TabIndex = 27
        '
        'lblDocumentExpiry
        '
        Me.lblDocumentExpiry.AutoSize = True
        Me.lblDocumentExpiry.BackColor = System.Drawing.SystemColors.Control
        Me.lblDocumentExpiry.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDocumentExpiry.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocumentExpiry.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDocumentExpiry.Location = New System.Drawing.Point(16, 59)
        Me.lblDocumentExpiry.Name = "lblDocumentExpiry"
        Me.lblDocumentExpiry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDocumentExpiry.Size = New System.Drawing.Size(113, 13)
        Me.lblDocumentExpiry.TabIndex = 34
        Me.lblDocumentExpiry.Text = "Document Expiry Date"
        '
        'lblDocumentDate
        '
        Me.lblDocumentDate.AutoSize = True
        Me.lblDocumentDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblDocumentDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDocumentDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocumentDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDocumentDate.Location = New System.Drawing.Point(16, 28)
        Me.lblDocumentDate.Name = "lblDocumentDate"
        Me.lblDocumentDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDocumentDate.Size = New System.Drawing.Size(82, 13)
        Me.lblDocumentDate.TabIndex = 33
        Me.lblDocumentDate.Text = "Document Date"
        '
        'lblMinus
        '
        Me.lblMinus.AutoSize = True
        Me.lblMinus.BackColor = System.Drawing.SystemColors.Control
        Me.lblMinus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMinus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMinus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMinus.Location = New System.Drawing.Point(152, 28)
        Me.lblMinus.Name = "lblMinus"
        Me.lblMinus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMinus.Size = New System.Drawing.Size(11, 13)
        Me.lblMinus.TabIndex = 32
        Me.lblMinus.Text = "-"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(152, 59)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(13, 13)
        Me.Label1.TabIndex = 31
        Me.Label1.Text = "+"
        '
        'lblDays
        '
        Me.lblDays.AutoSize = True
        Me.lblDays.BackColor = System.Drawing.SystemColors.Control
        Me.lblDays.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDays.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDays.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDays.Location = New System.Drawing.Point(216, 28)
        Me.lblDays.Name = "lblDays"
        Me.lblDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDays.Size = New System.Drawing.Size(29, 13)
        Me.lblDays.TabIndex = 30
        Me.lblDays.Text = "days"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(216, 59)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(29, 13)
        Me.Label2.TabIndex = 29
        Me.Label2.Text = "days"
        '
        'cmdDeafaultDates
        '
        Me.cmdDeafaultDates.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeafaultDates.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeafaultDates.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeafaultDates.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeafaultDates.Location = New System.Drawing.Point(368, 211)
        Me.cmdDeafaultDates.Name = "cmdDeafaultDates"
        Me.cmdDeafaultDates.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeafaultDates.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeafaultDates.TabIndex = 24
        Me.cmdDeafaultDates.Text = "&Defaults"
        Me.cmdDeafaultDates.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeafaultDates.UseVisualStyleBackColor = False
        '
        'fraImageViewer
        '
        Me.fraImageViewer.BackColor = System.Drawing.SystemColors.Control
        Me.fraImageViewer.Controls.Add(Me.chkImageViewer)
        Me.fraImageViewer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraImageViewer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraImageViewer.Location = New System.Drawing.Point(8, 136)
        Me.fraImageViewer.Name = "fraImageViewer"
        Me.fraImageViewer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraImageViewer.Size = New System.Drawing.Size(345, 57)
        Me.fraImageViewer.TabIndex = 22
        Me.fraImageViewer.TabStop = False
        Me.fraImageViewer.Text = "Image Viewer"
        '
        'chkImageViewer
        '
        Me.chkImageViewer.BackColor = System.Drawing.SystemColors.Control
        Me.chkImageViewer.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkImageViewer.Checked = True
        Me.chkImageViewer.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkImageViewer.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkImageViewer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkImageViewer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkImageViewer.Location = New System.Drawing.Point(16, 24)
        Me.chkImageViewer.Name = "chkImageViewer"
        Me.chkImageViewer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkImageViewer.Size = New System.Drawing.Size(233, 17)
        Me.chkImageViewer.TabIndex = 23
        Me.chkImageViewer.Text = "Use Default Image Viewer"
        Me.chkImageViewer.UseVisualStyleBackColor = False
        '
        'imgDocument
        '
        Me.imgDocument.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgDocument.Image = CType(resources.GetObject("imgDocument.Image"), System.Drawing.Image)
        Me.imgDocument.Location = New System.Drawing.Point(400, 32)
        Me.imgDocument.Name = "imgDocument"
        Me.imgDocument.Size = New System.Drawing.Size(32, 32)
        Me.imgDocument.TabIndex = 26
        Me.imgDocument.TabStop = False
        '
        '_tabOptions_TabPage4
        '
        Me._tabOptions_TabPage4.Controls.Add(Me.fraWarnIf)
        Me._tabOptions_TabPage4.Controls.Add(Me.cmdDefaultWarnings)
        Me._tabOptions_TabPage4.Controls.Add(Me.imgWarnings)
        Me._tabOptions_TabPage4.Location = New System.Drawing.Point(4, 22)
        Me._tabOptions_TabPage4.Name = "_tabOptions_TabPage4"
        Me._tabOptions_TabPage4.Size = New System.Drawing.Size(472, 267)
        Me._tabOptions_TabPage4.TabIndex = 4
        Me._tabOptions_TabPage4.Text = "5 - Warnings"
        Me._tabOptions_TabPage4.UseVisualStyleBackColor = True
        '
        'fraWarnIf
        '
        Me.fraWarnIf.BackColor = System.Drawing.SystemColors.Control
        Me.fraWarnIf.Controls.Add(Me.chkScanExternal)
        Me.fraWarnIf.Controls.Add(Me.chkMoveV2)
        Me.fraWarnIf.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraWarnIf.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraWarnIf.Location = New System.Drawing.Point(16, 28)
        Me.fraWarnIf.Name = "fraWarnIf"
        Me.fraWarnIf.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraWarnIf.Size = New System.Drawing.Size(345, 97)
        Me.fraWarnIf.TabIndex = 19
        Me.fraWarnIf.TabStop = False
        Me.fraWarnIf.Text = "Warn if..."
        '
        'chkScanExternal
        '
        Me.chkScanExternal.BackColor = System.Drawing.SystemColors.Control
        Me.chkScanExternal.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkScanExternal.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkScanExternal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkScanExternal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkScanExternal.Location = New System.Drawing.Point(16, 24)
        Me.chkScanExternal.Name = "chkScanExternal"
        Me.chkScanExternal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkScanExternal.Size = New System.Drawing.Size(281, 17)
        Me.chkScanExternal.TabIndex = 21
        Me.chkScanExternal.Text = "Scan to External Folder"
        Me.chkScanExternal.UseVisualStyleBackColor = False
        '
        'chkMoveV2
        '
        Me.chkMoveV2.BackColor = System.Drawing.SystemColors.Control
        Me.chkMoveV2.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkMoveV2.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkMoveV2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMoveV2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkMoveV2.Location = New System.Drawing.Point(16, 56)
        Me.chkMoveV2.Name = "chkMoveV2"
        Me.chkMoveV2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkMoveV2.Size = New System.Drawing.Size(281, 17)
        Me.chkMoveV2.TabIndex = 20
        Me.chkMoveV2.Text = "Move Documents to Non Version 2 Folder"
        Me.chkMoveV2.UseVisualStyleBackColor = False
        '
        'cmdDefaultWarnings
        '
        Me.cmdDefaultWarnings.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDefaultWarnings.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDefaultWarnings.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDefaultWarnings.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDefaultWarnings.Location = New System.Drawing.Point(376, 215)
        Me.cmdDefaultWarnings.Name = "cmdDefaultWarnings"
        Me.cmdDefaultWarnings.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDefaultWarnings.Size = New System.Drawing.Size(73, 22)
        Me.cmdDefaultWarnings.TabIndex = 18
        Me.cmdDefaultWarnings.Text = "&Defaults"
        Me.cmdDefaultWarnings.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDefaultWarnings.UseVisualStyleBackColor = False
        '
        'imgWarnings
        '
        Me.imgWarnings.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgWarnings.Image = CType(resources.GetObject("imgWarnings.Image"), System.Drawing.Image)
        Me.imgWarnings.Location = New System.Drawing.Point(408, 36)
        Me.imgWarnings.Name = "imgWarnings"
        Me.imgWarnings.Size = New System.Drawing.Size(32, 32)
        Me.imgWarnings.TabIndex = 20
        Me.imgWarnings.TabStop = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Enabled = False
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(384, 304)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 5
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(296, 304)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 4
        Me.cmdCancel.Text = "&Close"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApply.Enabled = False
        Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Location = New System.Drawing.Point(208, 304)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApply.Size = New System.Drawing.Size(73, 22)
        Me.cmdApply.TabIndex = 3
        Me.cmdApply.Text = "&Apply"
        Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdApply.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(500, 343)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabOptions)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdApply)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Options"
        Me.tabOptions.ResumeLayout(False)
        Me._tabOptions_TabPage0.ResumeLayout(False)
        Me.fraWAN.ResumeLayout(False)
        Me.fraLimits.ResumeLayout(False)
        Me.fraLimits.PerformLayout()
        Me.fraDisplay.ResumeLayout(False)
        CType(Me.imgNavigation, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabOptions_TabPage1.ResumeLayout(False)
        Me.Frame2.ResumeLayout(False)
        Me._tabOptions_TabPage2.ResumeLayout(False)
        Me.fraDefDoc.ResumeLayout(False)
        Me.fraDefDoc.PerformLayout()
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me._fraRTF_0.ResumeLayout(False)
        CType(Me.imgConfiguration, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabOptions_TabPage3.ResumeLayout(False)
        Me.fraScanSettings.ResumeLayout(False)
        Me.fraScanSettings.PerformLayout()
        Me.fraImageViewer.ResumeLayout(False)
        CType(Me.imgDocument, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabOptions_TabPage4.ResumeLayout(False)
        Me.fraWarnIf.ResumeLayout(False)
        CType(Me.imgWarnings, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
	Sub InitializefraRTF()
		Me.fraRTF(0) = _fraRTF_0
	End Sub
#End Region 
End Class