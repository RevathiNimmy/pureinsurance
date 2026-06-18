<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		tabMainTabPreviousTab = tabMainTab.SelectedIndex
        Form_Initialize_Renamed()
        FormResize()
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
	Public WithEvents mnuExit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReadLicenceFile As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuUpdateSystemLimit As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuUpdateLicenceLimit As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
    Public dlgBrowseOpen As System.Windows.Forms.OpenFileDialog
    Public WithEvents cmdReset As System.Windows.Forms.Button
    Public WithEvents cmdRefresh As System.Windows.Forms.Button
    Public WithEvents tmrRefreshInstances As System.Windows.Forms.Timer
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents lblLicenceLimit As System.Windows.Forms.Label
    Public WithEvents lblNumAllocatedTitle As System.Windows.Forms.Label
    Private WithEvents _lstInstances_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lstInstances_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lstInstances_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Public WithEvents lstInstances As System.Windows.Forms.ListView
    Public WithEvents panLicenceLimit As System.Windows.Forms.Panel
    Public WithEvents panNumAllocated As System.Windows.Forms.Panel
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents sldRefresh As System.Windows.Forms.TrackBar
    Public WithEvents lblSlow As System.Windows.Forms.Label
    Public WithEvents lblFast As System.Windows.Forms.Label
    Public WithEvents lblMinutes As System.Windows.Forms.Label
    Public WithEvents lblDsplMins As System.Windows.Forms.Label
    Public WithEvents frarefresh As System.Windows.Forms.GroupBox
    Public WithEvents chkAutoRef As System.Windows.Forms.CheckBox
    Public WithEvents lblYN As System.Windows.Forms.Label
    Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public WithEvents imgImageList As System.Windows.Forms.ImageList
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    Private tabMainTabPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    '<System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip()
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuUpdateLicenceLimit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReadLicenceFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuUpdateSystemLimit = New System.Windows.Forms.ToolStripMenuItem()
        Me.dlgBrowseOpen = New System.Windows.Forms.OpenFileDialog()
        Me.cmdReset = New System.Windows.Forms.Button()
        Me.cmdRefresh = New System.Windows.Forms.Button()
        Me.tmrRefreshInstances = New System.Windows.Forms.Timer(Me.components)
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.lblLicenceValidity1 = New System.Windows.Forms.Label()
        Me.lblLicenceLimit = New System.Windows.Forms.Label()
        Me.lblNumAllocatedTitle = New System.Windows.Forms.Label()
        Me.lstInstances = New System.Windows.Forms.ListView()
        Me._lstInstances_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lstInstances_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lstInstances_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.panLicenceLimit = New System.Windows.Forms.Panel()
        Me.lblLicenceLimit1 = New System.Windows.Forms.Label()
        Me.panNumAllocated = New System.Windows.Forms.Panel()
        Me.lblNumAllocated = New System.Windows.Forms.Label()
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage()
        Me.frarefresh = New System.Windows.Forms.GroupBox()
        Me.sldRefresh = New System.Windows.Forms.TrackBar()
        Me.lblSlow = New System.Windows.Forms.Label()
        Me.lblFast = New System.Windows.Forms.Label()
        Me.lblMinutes = New System.Windows.Forms.Label()
        Me.lblDsplMins = New System.Windows.Forms.Label()
        Me.chkAutoRef = New System.Windows.Forms.CheckBox()
        Me.lblYN = New System.Windows.Forms.Label()
        Me.imgImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.pnlLicencevalidity = New System.Windows.Forms.Panel()
        Me.lblLicenceValidity = New System.Windows.Forms.Label()
        Me.MainMenu1.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.panLicenceLimit.SuspendLayout()
        Me.panNumAllocated.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me.frarefresh.SuspendLayout()
        CType(Me.sldRefresh, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlLicencevalidity.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuUpdateLicenceLimit})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(436, 30)
        Me.MainMenu1.TabIndex = 10
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuExit})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.ShowShortcutKeys = False
        Me.mnuFile.Size = New System.Drawing.Size(46, 26)
        Me.mnuFile.Text = "&File"
        '
        'mnuExit
        '
        Me.mnuExit.Name = "mnuExit"
        Me.mnuExit.Size = New System.Drawing.Size(116, 26)
        Me.mnuExit.Text = "&Exit"
        '
        'mnuUpdateLicenceLimit
        '
        Me.mnuUpdateLicenceLimit.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReadLicenceFile, Me.mnuUpdateSystemLimit})
        Me.mnuUpdateLicenceLimit.Name = "mnuUpdateLicenceLimit"
        Me.mnuUpdateLicenceLimit.ShowShortcutKeys = False
        Me.mnuUpdateLicenceLimit.Size = New System.Drawing.Size(125, 26)
        Me.mnuUpdateLicenceLimit.Text = "&Update Licence"
        '
        'mnuReadLicenceFile
        '
        Me.mnuReadLicenceFile.Name = "mnuReadLicenceFile"
        Me.mnuReadLicenceFile.Size = New System.Drawing.Size(245, 26)
        Me.mnuReadLicenceFile.Text = "&View Licence File"
        '
        'mnuUpdateSystemLimit
        '
        Me.mnuUpdateSystemLimit.Name = "mnuUpdateSystemLimit"
        Me.mnuUpdateSystemLimit.Size = New System.Drawing.Size(245, 26)
        Me.mnuUpdateSystemLimit.Text = "Update &System Licence"
        '
        'cmdReset
        '
        Me.cmdReset.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReset.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReset.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReset.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReset.Location = New System.Drawing.Point(111, 458)
        Me.cmdReset.Name = "cmdReset"
        Me.cmdReset.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReset.Size = New System.Drawing.Size(97, 26)
        Me.cmdReset.TabIndex = 9
        Me.cmdReset.Text = "Re&set"
        Me.cmdReset.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdReset.UseVisualStyleBackColor = False
        '
        'cmdRefresh
        '
        Me.cmdRefresh.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRefresh.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRefresh.Enabled = False
        Me.cmdRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRefresh.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRefresh.Location = New System.Drawing.Point(5, 458)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRefresh.Size = New System.Drawing.Size(98, 26)
        Me.cmdRefresh.TabIndex = 1
        Me.cmdRefresh.Text = "&Refresh"
        Me.cmdRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRefresh.UseVisualStyleBackColor = False
        '
        'tmrRefreshInstances
        '
        Me.tmrRefreshInstances.Enabled = True
        Me.tmrRefreshInstances.Interval = 60000
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Enabled = False
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(427, 458)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(97, 26)
        Me.cmdHelp.TabIndex = 3
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(321, 458)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(98, 26)
        Me.cmdOK.TabIndex = 2
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage2)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(127, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(11, 33)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(606, 417)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.pnlLicencevalidity)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblLicenceValidity1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblLicenceLimit)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblNumAllocatedTitle)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lstInstances)
        Me._tabMainTab_TabPage0.Controls.Add(Me.panLicenceLimit)
        Me._tabMainTab_TabPage0.Controls.Add(Me.panNumAllocated)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(598, 391)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "System Licence"
        '
        'lblLicenceValidity1
        '
        Me.lblLicenceValidity1.BackColor = System.Drawing.SystemColors.Control
        Me.lblLicenceValidity1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLicenceValidity1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLicenceValidity1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLicenceValidity1.Location = New System.Drawing.Point(299, 24)
        Me.lblLicenceValidity1.Name = "lblLicenceValidity1"
        Me.lblLicenceValidity1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLicenceValidity1.Size = New System.Drawing.Size(116, 19)
        Me.lblLicenceValidity1.TabIndex = 11
        Me.lblLicenceValidity1.Text = "Licence Validity:"
        '
        'lblLicenceLimit
        '
        Me.lblLicenceLimit.BackColor = System.Drawing.SystemColors.Control
        Me.lblLicenceLimit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLicenceLimit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLicenceLimit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLicenceLimit.Location = New System.Drawing.Point(11, 24)
        Me.lblLicenceLimit.Name = "lblLicenceLimit"
        Me.lblLicenceLimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLicenceLimit.Size = New System.Drawing.Size(118, 21)
        Me.lblLicenceLimit.TabIndex = 6
        Me.lblLicenceLimit.Text = "Licence Limit:"
        '
        'lblNumAllocatedTitle
        '
        Me.lblNumAllocatedTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblNumAllocatedTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNumAllocatedTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNumAllocatedTitle.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNumAllocatedTitle.Location = New System.Drawing.Point(156, 24)
        Me.lblNumAllocatedTitle.Name = "lblNumAllocatedTitle"
        Me.lblNumAllocatedTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNumAllocatedTitle.Size = New System.Drawing.Size(146, 21)
        Me.lblNumAllocatedTitle.TabIndex = 8
        Me.lblNumAllocatedTitle.Text = "Licences Allocated:"
        '
        'lstInstances
        '
        Me.lstInstances.BackColor = System.Drawing.SystemColors.Window
        Me.lstInstances.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lstInstances, "")
        Me.lstInstances.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lstInstances_ColumnHeader_1, Me._lstInstances_ColumnHeader_2, Me._lstInstances_ColumnHeader_3})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lstInstances, True)
        Me.lstInstances.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstInstances.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lstInstances.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lstInstances, "")
        Me.lstInstances.LabelEdit = True
        Me.listViewHelper1.SetLargeIcons(Me.lstInstances, "")
        Me.lstInstances.Location = New System.Drawing.Point(11, 73)
        Me.lstInstances.Name = "lstInstances"
        Me.lstInstances.Size = New System.Drawing.Size(548, 309)
        Me.listViewHelper1.SetSmallIcons(Me.lstInstances, "")
        Me.listViewHelper1.SetSorted(Me.lstInstances, True)
        Me.lstInstances.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.listViewHelper1.SetSortKey(Me.lstInstances, 0)
        Me.listViewHelper1.SetSortOrder(Me.lstInstances, System.Windows.Forms.SortOrder.Ascending)
        Me.lstInstances.TabIndex = 7
        Me.lstInstances.UseCompatibleStateImageBehavior = False
        Me.lstInstances.View = System.Windows.Forms.View.Details
        '
        '_lstInstances_ColumnHeader_1
        '
        Me._lstInstances_ColumnHeader_1.Text = "User Name"
        Me._lstInstances_ColumnHeader_1.Width = 101
        '
        '_lstInstances_ColumnHeader_2
        '
        Me._lstInstances_ColumnHeader_2.Text = "Logged on at Client"
        Me._lstInstances_ColumnHeader_2.Width = 101
        '
        '_lstInstances_ColumnHeader_3
        '
        Me._lstInstances_ColumnHeader_3.Text = "Logon Time"
        Me._lstInstances_ColumnHeader_3.Width = 94
        '
        'panLicenceLimit
        '
        Me.panLicenceLimit.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panLicenceLimit.Controls.Add(Me.lblLicenceLimit1)
        Me.panLicenceLimit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panLicenceLimit.Location = New System.Drawing.Point(12, 44)
        Me.panLicenceLimit.Name = "panLicenceLimit"
        Me.panLicenceLimit.Size = New System.Drawing.Size(129, 20)
        Me.panLicenceLimit.TabIndex = 5
        '
        'lblLicenceLimit1
        '
        Me.lblLicenceLimit1.AutoSize = True
        Me.lblLicenceLimit1.Location = New System.Drawing.Point(0, 0)
        Me.lblLicenceLimit1.Name = "lblLicenceLimit1"
        Me.lblLicenceLimit1.Size = New System.Drawing.Size(0, 17)
        Me.lblLicenceLimit1.TabIndex = 0
        '
        'panNumAllocated
        '
        Me.panNumAllocated.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panNumAllocated.Controls.Add(Me.lblNumAllocated)
        Me.panNumAllocated.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panNumAllocated.Location = New System.Drawing.Point(156, 44)
        Me.panNumAllocated.Name = "panNumAllocated"
        Me.panNumAllocated.Size = New System.Drawing.Size(129, 20)
        Me.panNumAllocated.TabIndex = 4
        '
        'lblNumAllocated
        '
        Me.lblNumAllocated.AutoSize = True
        Me.lblNumAllocated.Location = New System.Drawing.Point(-3, 2)
        Me.lblNumAllocated.Name = "lblNumAllocated"
        Me.lblNumAllocated.Size = New System.Drawing.Size(0, 17)
        Me.lblNumAllocated.TabIndex = 0
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me.frarefresh)
        Me._tabMainTab_TabPage2.Controls.Add(Me.chkAutoRef)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblYN)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(598, 391)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "Refresh Settings"
        '
        'frarefresh
        '
        Me.frarefresh.BackColor = System.Drawing.SystemColors.Control
        Me.frarefresh.Controls.Add(Me.sldRefresh)
        Me.frarefresh.Controls.Add(Me.lblSlow)
        Me.frarefresh.Controls.Add(Me.lblFast)
        Me.frarefresh.Controls.Add(Me.lblMinutes)
        Me.frarefresh.Controls.Add(Me.lblDsplMins)
        Me.frarefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frarefresh.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frarefresh.Location = New System.Drawing.Point(21, 73)
        Me.frarefresh.Name = "frarefresh"
        Me.frarefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frarefresh.Size = New System.Drawing.Size(411, 172)
        Me.frarefresh.TabIndex = 11
        Me.frarefresh.TabStop = False
        Me.frarefresh.Text = "Refresh Rate"
        '
        'sldRefresh
        '
        Me.sldRefresh.LargeChange = 2
        Me.sldRefresh.Location = New System.Drawing.Point(47, 56)
        Me.sldRefresh.Maximum = 60
        Me.sldRefresh.Minimum = 1
        Me.sldRefresh.Name = "sldRefresh"
        Me.sldRefresh.Size = New System.Drawing.Size(353, 56)
        Me.sldRefresh.TabIndex = 12
        Me.sldRefresh.Value = 1
        '
        'lblSlow
        '
        Me.lblSlow.AutoSize = True
        Me.lblSlow.BackColor = System.Drawing.SystemColors.Control
        Me.lblSlow.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSlow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSlow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSlow.Location = New System.Drawing.Point(352, 108)
        Me.lblSlow.Name = "lblSlow"
        Me.lblSlow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSlow.Size = New System.Drawing.Size(37, 17)
        Me.lblSlow.TabIndex = 16
        Me.lblSlow.Text = "Slow"
        '
        'lblFast
        '
        Me.lblFast.AutoSize = True
        Me.lblFast.BackColor = System.Drawing.SystemColors.Control
        Me.lblFast.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFast.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFast.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFast.Location = New System.Drawing.Point(43, 108)
        Me.lblFast.Name = "lblFast"
        Me.lblFast.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFast.Size = New System.Drawing.Size(35, 17)
        Me.lblFast.TabIndex = 15
        Me.lblFast.Text = "Fast"
        '
        'lblMinutes
        '
        Me.lblMinutes.AutoSize = True
        Me.lblMinutes.BackColor = System.Drawing.SystemColors.Control
        Me.lblMinutes.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMinutes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMinutes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMinutes.Location = New System.Drawing.Point(249, 33)
        Me.lblMinutes.Name = "lblMinutes"
        Me.lblMinutes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMinutes.Size = New System.Drawing.Size(50, 17)
        Me.lblMinutes.TabIndex = 14
        Me.lblMinutes.Text = "Minute"
        '
        'lblDsplMins
        '
        Me.lblDsplMins.BackColor = System.Drawing.Color.White
        Me.lblDsplMins.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblDsplMins.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDsplMins.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDsplMins.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDsplMins.Location = New System.Drawing.Point(224, 32)
        Me.lblDsplMins.Name = "lblDsplMins"
        Me.lblDsplMins.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDsplMins.Size = New System.Drawing.Size(24, 20)
        Me.lblDsplMins.TabIndex = 13
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
        Me.chkAutoRef.Location = New System.Drawing.Point(21, 34)
        Me.chkAutoRef.Name = "chkAutoRef"
        Me.chkAutoRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAutoRef.Size = New System.Drawing.Size(130, 21)
        Me.chkAutoRef.TabIndex = 10
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
        Me.lblYN.Location = New System.Drawing.Point(164, 35)
        Me.lblYN.Name = "lblYN"
        Me.lblYN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblYN.Size = New System.Drawing.Size(32, 17)
        Me.lblYN.TabIndex = 17
        Me.lblYN.Text = "Yes"
        '
        'imgImageList
        '
        Me.imgImageList.ImageStream = CType(resources.GetObject("imgImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgImageList.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.imgImageList.Images.SetKeyName(0, "")
        '
        'Timer1
        '
        '
        'pnlLicencevalidity
        '
        Me.pnlLicencevalidity.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlLicencevalidity.Controls.Add(Me.lblLicenceValidity)
        Me.pnlLicencevalidity.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlLicencevalidity.Location = New System.Drawing.Point(299, 45)
        Me.pnlLicencevalidity.Name = "pnlLicencevalidity"
        Me.pnlLicencevalidity.Size = New System.Drawing.Size(129, 20)
        Me.pnlLicencevalidity.TabIndex = 12

        '
        'lblLicenceValidity
        '
        Me.lblLicenceValidity.AutoSize = True
        Me.lblLicenceValidity.Location = New System.Drawing.Point(0, 0)
        Me.lblLicenceValidity.Name = "lblLicenceValidity"
        Me.lblLicenceValidity.Size = New System.Drawing.Size(0, 17)
        Me.lblLicenceValidity.TabIndex = 1
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(8, 17)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdOK
        Me.ClientSize = New System.Drawing.Size(436, 422)
        Me.Controls.Add(Me.cmdReset)
        Me.Controls.Add(Me.cmdRefresh)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(328, 246)
        Me.MaximizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Licence Administrator"
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.panLicenceLimit.ResumeLayout(False)
        Me.panLicenceLimit.PerformLayout()
        Me.panNumAllocated.ResumeLayout(False)
        Me.panNumAllocated.PerformLayout()
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me._tabMainTab_TabPage2.PerformLayout()
        Me.frarefresh.ResumeLayout(False)
        Me.frarefresh.PerformLayout()
        CType(Me.sldRefresh, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlLicencevalidity.ResumeLayout(False)
        Me.pnlLicencevalidity.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblNumAllocated As System.Windows.Forms.Label
    Friend WithEvents lblLicenceLimit1 As System.Windows.Forms.Label
    Friend WithEvents lblLicenceValidity1 As Label
    Friend WithEvents Timer1 As Timer
    Public WithEvents pnlLicencevalidity As Panel
    Friend WithEvents lblLicenceValidity As Label
#End Region
End Class