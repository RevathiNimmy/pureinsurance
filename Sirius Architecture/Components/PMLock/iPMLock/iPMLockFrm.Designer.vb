<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwUsers_InitializeColumnKeys()
		lvwLocks_InitializeColumnKeys()
		tabMainTabPreviousTab = tabMainTab.SelectedIndex
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
	Public WithEvents cmdRefresh As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents imgIcon As System.Windows.Forms.PictureBox
	Private WithEvents _lvwLocks_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwLocks_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwLocks_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwLocks_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwLocks_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwLocks As System.Windows.Forms.ListView
	Public WithEvents cmdDeleteLock As System.Windows.Forms.Button
	Public WithEvents cmdClear As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Private WithEvents _lvwUsers_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwUsers_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwUsers As System.Windows.Forms.ListView
	Public WithEvents cmdDeleteUser As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents tmrPMLock As System.Windows.Forms.Timer
	Public WithEvents sldRefreshRate As System.Windows.Forms.TrackBar
	Public WithEvents txtSeconds As System.Windows.Forms.TextBox
	Public WithEvents lblSeconds As System.Windows.Forms.Label
	Public WithEvents lblMin As System.Windows.Forms.Label
	Public WithEvents lblMax As System.Windows.Forms.Label
	Public WithEvents fraRefresh As System.Windows.Forms.GroupBox
	Public WithEvents chkAutoRefresh As System.Windows.Forms.CheckBox
	Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdRefresh = New System.Windows.Forms.Button()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.imgIcon = New System.Windows.Forms.PictureBox()
        Me.lvwLocks = New System.Windows.Forms.ListView()
        Me._lvwLocks_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwLocks_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwLocks_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwLocks_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwLocks_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmdDeleteLock = New System.Windows.Forms.Button()
        Me.cmdClear = New System.Windows.Forms.Button()
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage()
        Me.lvwUsers = New System.Windows.Forms.ListView()
        Me._lvwUsers_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwUsers_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmdDeleteUser = New System.Windows.Forms.Button()
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage()
        Me.fraRefresh = New System.Windows.Forms.GroupBox()
        Me.sldRefreshRate = New System.Windows.Forms.TrackBar()
        Me.txtSeconds = New System.Windows.Forms.TextBox()
        Me.lblSeconds = New System.Windows.Forms.Label()
        Me.lblMin = New System.Windows.Forms.Label()
        Me.lblMax = New System.Windows.Forms.Label()
        Me.chkAutoRefresh = New System.Windows.Forms.CheckBox()
        Me.tmrPMLock = New System.Windows.Forms.Timer(Me.components)
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me.fraRefresh.SuspendLayout()
        CType(Me.sldRefreshRate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdRefresh
        '
        Me.cmdRefresh.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRefresh.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRefresh.Enabled = False
        Me.cmdRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRefresh.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRefresh.Location = New System.Drawing.Point(8, 272)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRefresh.Size = New System.Drawing.Size(73, 22)
        Me.cmdRefresh.TabIndex = 1
        Me.cmdRefresh.Text = "&Refresh"
        Me.cmdRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRefresh.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(376, 272)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 3
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(296, 272)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 2
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage1)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage2)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(87, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(445, 261)
        Me.tabMainTab.TabIndex = 0
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.imgIcon)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwLocks)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdDeleteLock)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdClear)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(437, 235)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Locks"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'imgIcon
        '
        Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon.Image = CType(resources.GetObject("imgIcon.Image"), System.Drawing.Image)
        Me.imgIcon.Location = New System.Drawing.Point(400, 12)
        Me.imgIcon.Name = "imgIcon"
        Me.imgIcon.Size = New System.Drawing.Size(32, 32)
        Me.imgIcon.TabIndex = 0
        Me.imgIcon.TabStop = False
        '
        'lvwLocks
        '
        Me.lvwLocks.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwLocks, "")
        Me.lvwLocks.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwLocks_ColumnHeader_1, Me._lvwLocks_ColumnHeader_2, Me._lvwLocks_ColumnHeader_3, Me._lvwLocks_ColumnHeader_4, Me._lvwLocks_ColumnHeader_5})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwLocks, True)
        Me.lvwLocks.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwLocks.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwLocks.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwLocks, "lvwLocks_ItemClick")
        Me.lvwLocks.LabelWrap = False
        Me.listViewHelper1.SetLargeIcons(Me.lvwLocks, "")
        Me.lvwLocks.Location = New System.Drawing.Point(16, 52)
        Me.lvwLocks.Name = "lvwLocks"
        Me.lvwLocks.Size = New System.Drawing.Size(409, 137)
        Me.listViewHelper1.SetSmallIcons(Me.lvwLocks, "")
        Me.listViewHelper1.SetSorted(Me.lvwLocks, False)
        Me.listViewHelper1.SetSortKey(Me.lvwLocks, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwLocks, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwLocks.TabIndex = 0
        Me.lvwLocks.UseCompatibleStateImageBehavior = False
        Me.lvwLocks.View = System.Windows.Forms.View.Details
        '
        '_lvwLocks_ColumnHeader_1
        '
        Me._lvwLocks_ColumnHeader_1.Tag = ""
        Me._lvwLocks_ColumnHeader_1.Text = "Name"
        Me._lvwLocks_ColumnHeader_1.Width = 145
        '
        '_lvwLocks_ColumnHeader_2
        '
        Me._lvwLocks_ColumnHeader_2.Tag = ""
        Me._lvwLocks_ColumnHeader_2.Text = "Value"
        Me._lvwLocks_ColumnHeader_2.Width = 34
        '
        '_lvwLocks_ColumnHeader_3
        '
        Me._lvwLocks_ColumnHeader_3.Tag = ""
        Me._lvwLocks_ColumnHeader_3.Text = "Sub Value"
        Me._lvwLocks_ColumnHeader_3.Width = 97
        '
        '_lvwLocks_ColumnHeader_4
        '
        Me._lvwLocks_ColumnHeader_4.Tag = ""
        Me._lvwLocks_ColumnHeader_4.Text = "User"
        Me._lvwLocks_ColumnHeader_4.Width = 54
        '
        '_lvwLocks_ColumnHeader_5
        '
        Me._lvwLocks_ColumnHeader_5.Tag = ""
        Me._lvwLocks_ColumnHeader_5.Text = "Time"
        Me._lvwLocks_ColumnHeader_5.Width = 97
        '
        'cmdDeleteLock
        '
        Me.cmdDeleteLock.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteLock.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteLock.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteLock.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteLock.Location = New System.Drawing.Point(16, 196)
        Me.cmdDeleteLock.Name = "cmdDeleteLock"
        Me.cmdDeleteLock.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteLock.Size = New System.Drawing.Size(81, 22)
        Me.cmdDeleteLock.TabIndex = 1
        Me.cmdDeleteLock.Text = "Delete &Lock"
        Me.cmdDeleteLock.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteLock.UseVisualStyleBackColor = False
        '
        'cmdClear
        '
        Me.cmdClear.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClear.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClear.Location = New System.Drawing.Point(104, 196)
        Me.cmdClear.Name = "cmdClear"
        Me.cmdClear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClear.Size = New System.Drawing.Size(81, 22)
        Me.cmdClear.TabIndex = 2
        Me.cmdClear.Text = "&Clear"
        Me.cmdClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClear.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.lvwUsers)
        Me._tabMainTab_TabPage1.Controls.Add(Me.cmdDeleteUser)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(437, 235)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "2 - Users"
        Me._tabMainTab_TabPage1.UseVisualStyleBackColor = True
        '
        'lvwUsers
        '
        Me.lvwUsers.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwUsers, "")
        Me.lvwUsers.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwUsers_ColumnHeader_1, Me._lvwUsers_ColumnHeader_2})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwUsers, True)
        Me.lvwUsers.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwUsers.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwUsers.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwUsers, "lvwUsers_ItemClick")
        Me.lvwUsers.LabelWrap = False
        Me.listViewHelper1.SetLargeIcons(Me.lvwUsers, "")
        Me.lvwUsers.Location = New System.Drawing.Point(16, 52)
        Me.lvwUsers.Name = "lvwUsers"
        Me.lvwUsers.Size = New System.Drawing.Size(409, 137)
        Me.listViewHelper1.SetSmallIcons(Me.lvwUsers, "")
        Me.listViewHelper1.SetSorted(Me.lvwUsers, False)
        Me.listViewHelper1.SetSortKey(Me.lvwUsers, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwUsers, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwUsers.TabIndex = 3
        Me.lvwUsers.UseCompatibleStateImageBehavior = False
        Me.lvwUsers.View = System.Windows.Forms.View.Details
        '
        '_lvwUsers_ColumnHeader_1
        '
        Me._lvwUsers_ColumnHeader_1.Tag = ""
        Me._lvwUsers_ColumnHeader_1.Text = "User Name"
        Me._lvwUsers_ColumnHeader_1.Width = 97
        '
        '_lvwUsers_ColumnHeader_2
        '
        Me._lvwUsers_ColumnHeader_2.Tag = ""
        Me._lvwUsers_ColumnHeader_2.Text = "No. of locks held"
        Me._lvwUsers_ColumnHeader_2.Width = 97
        '
        'cmdDeleteUser
        '
        Me.cmdDeleteUser.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteUser.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteUser.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteUser.Location = New System.Drawing.Point(16, 196)
        Me.cmdDeleteUser.Name = "cmdDeleteUser"
        Me.cmdDeleteUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteUser.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteUser.TabIndex = 4
        Me.cmdDeleteUser.Text = "&Delete"
        Me.cmdDeleteUser.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteUser.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraRefresh)
        Me._tabMainTab_TabPage2.Controls.Add(Me.chkAutoRefresh)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(437, 235)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "3 - Refresh Options"
        Me._tabMainTab_TabPage2.UseVisualStyleBackColor = True
        '
        'fraRefresh
        '
        Me.fraRefresh.BackColor = System.Drawing.SystemColors.Control
        Me.fraRefresh.Controls.Add(Me.sldRefreshRate)
        Me.fraRefresh.Controls.Add(Me.txtSeconds)
        Me.fraRefresh.Controls.Add(Me.lblSeconds)
        Me.fraRefresh.Controls.Add(Me.lblMin)
        Me.fraRefresh.Controls.Add(Me.lblMax)
        Me.fraRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraRefresh.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraRefresh.Location = New System.Drawing.Point(32, 68)
        Me.fraRefresh.Name = "fraRefresh"
        Me.fraRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraRefresh.Size = New System.Drawing.Size(377, 128)
        Me.fraRefresh.TabIndex = 6
        Me.fraRefresh.TabStop = False
        Me.fraRefresh.Text = "Refresh Rate"
        '
        'sldRefreshRate
        '
        Me.sldRefreshRate.Location = New System.Drawing.Point(72, 55)
        Me.sldRefreshRate.Maximum = 60
        Me.sldRefreshRate.Minimum = 1
        Me.sldRefreshRate.Name = "sldRefreshRate"
        Me.sldRefreshRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.sldRefreshRate.Size = New System.Drawing.Size(253, 45)
        Me.sldRefreshRate.TabIndex = 9
        Me.sldRefreshRate.Value = 1
        '
        'txtSeconds
        '
        Me.txtSeconds.AcceptsReturn = True
        Me.txtSeconds.BackColor = System.Drawing.SystemColors.Window
        Me.txtSeconds.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSeconds.Enabled = False
        Me.txtSeconds.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSeconds.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSeconds.Location = New System.Drawing.Point(137, 29)
        Me.txtSeconds.MaxLength = 0
        Me.txtSeconds.Name = "txtSeconds"
        Me.txtSeconds.ReadOnly = True
        Me.txtSeconds.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSeconds.Size = New System.Drawing.Size(18, 20)
        Me.txtSeconds.TabIndex = 7
        Me.txtSeconds.Text = "1"
        '
        'lblSeconds
        '
        Me.lblSeconds.AutoSize = True
        Me.lblSeconds.BackColor = System.Drawing.SystemColors.Control
        Me.lblSeconds.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSeconds.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSeconds.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSeconds.Location = New System.Drawing.Point(81, 32)
        Me.lblSeconds.Name = "lblSeconds"
        Me.lblSeconds.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSeconds.Size = New System.Drawing.Size(49, 13)
        Me.lblSeconds.TabIndex = 8
        Me.lblSeconds.Text = "Seconds"
        '
        'lblMin
        '
        Me.lblMin.AutoSize = True
        Me.lblMin.BackColor = System.Drawing.SystemColors.Control
        Me.lblMin.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMin.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMin.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMin.Location = New System.Drawing.Point(80, 100)
        Me.lblMin.Name = "lblMin"
        Me.lblMin.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMin.Size = New System.Drawing.Size(13, 13)
        Me.lblMin.TabIndex = 10
        Me.lblMin.Text = "1"
        '
        'lblMax
        '
        Me.lblMax.AutoSize = True
        Me.lblMax.BackColor = System.Drawing.SystemColors.Control
        Me.lblMax.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMax.Location = New System.Drawing.Point(306, 100)
        Me.lblMax.Name = "lblMax"
        Me.lblMax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMax.Size = New System.Drawing.Size(19, 13)
        Me.lblMax.TabIndex = 11
        Me.lblMax.Text = "60"
        '
        'chkAutoRefresh
        '
        Me.chkAutoRefresh.BackColor = System.Drawing.SystemColors.Control
        Me.chkAutoRefresh.Checked = True
        Me.chkAutoRefresh.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAutoRefresh.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAutoRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAutoRefresh.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAutoRefresh.Location = New System.Drawing.Point(32, 28)
        Me.chkAutoRefresh.Name = "chkAutoRefresh"
        Me.chkAutoRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAutoRefresh.Size = New System.Drawing.Size(177, 25)
        Me.chkAutoRefresh.TabIndex = 5
        Me.chkAutoRefresh.Text = "&Auto Refresh"
        Me.chkAutoRefresh.UseVisualStyleBackColor = False
        '
        'tmrPMLock
        '
        Me.tmrPMLock.Enabled = True
        Me.tmrPMLock.Interval = 5000
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(457, 301)
        Me.Controls.Add(Me.cmdRefresh)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Lock File Management"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me.fraRefresh.ResumeLayout(False)
        Me.fraRefresh.PerformLayout()
        CType(Me.sldRefreshRate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
	Sub lvwUsers_InitializeColumnKeys()
		Me._lvwUsers_ColumnHeader_1.Name = ""
		Me._lvwUsers_ColumnHeader_2.Name = ""
	End Sub
	Sub lvwLocks_InitializeColumnKeys()
		Me._lvwLocks_ColumnHeader_1.Name = ""
		Me._lvwLocks_ColumnHeader_2.Name = ""
		Me._lvwLocks_ColumnHeader_3.Name = ""
		Me._lvwLocks_ColumnHeader_4.Name = ""
		Me._lvwLocks_ColumnHeader_5.Name = ""
	End Sub
#End Region 
End Class