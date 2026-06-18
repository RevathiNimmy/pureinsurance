<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializemnuViewSortBy()
		InitializelblTitle()
		lstScheduledTasks_InitializeColumnKeys()
	End Sub
	Private Sub Ctx_mnuTask_Opening(ByVal sender As object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuTask.Opening
		Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
		Ctx_mnuTask.Items.Clear()
		'We are moving the submenus from original menu to the context menu before displaying it
		For	Each item As System.Windows.Forms.ToolStripItem In mnuTask.DropDownItems
			list.Add(item)
		Next item
		For	Each item As System.Windows.Forms.ToolStripItem In list
			Ctx_mnuTask.Items.Add(item)
		Next item
		e.Cancel = False
	End Sub
	Private Sub Ctx_mnuTask_Closing(ByVal sender As object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuTask.Closing
		Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
		'We are moving the submenus the context menu back to the original menu after displaying
		For	Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuTask.Items
			list.Add(item)
		Next item
		For	Each item As System.Windows.Forms.ToolStripItem In list
			mnuTask.DropDownItems.Add(item)
		Next item
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
	Public WithEvents mnuFileExit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuTaskEdit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuTaskAssign As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuTaskView As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuTask As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuViewSortBy_1 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuViewSortBy_2 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuViewSortBy_3 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuViewSortBy_4 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuViewSortBy_5 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuViewSortBy_6 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuViewSortBy_7 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuViewSortBy_8 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuViewSortSchedule As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuViewBar4 As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuViewRefresh As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuView As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	Private WithEvents _tbToolBar_Button1 As System.Windows.Forms.ToolStripButton
	Private WithEvents _tbToolBar_Button2 As System.Windows.Forms.ToolStripButton
	Private WithEvents _tbToolBar_Button3 As System.Windows.Forms.ToolStripSeparator
	Private WithEvents _tbToolBar_Button4 As System.Windows.Forms.ToolStripButton
	Public WithEvents tbToolBar As System.Windows.Forms.ToolStrip
	Public WithEvents tmrSystemTasks As System.Windows.Forms.Timer
	Public WithEvents cboAllUsers As PMUserLookupControl.cboPMUserLookup
	Public WithEvents cboTaskStatus As System.Windows.Forms.ComboBox
	Public WithEvents cboDateRange As System.Windows.Forms.ComboBox
	Public WithEvents cboUserGroup As PMUGroupLookupCtrl.cboPMUserGroupByUser
	Private WithEvents _lstScheduledTasks_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lstScheduledTasks_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lstScheduledTasks_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lstScheduledTasks_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lstScheduledTasks_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lstScheduledTasks_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lstScheduledTasks_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lstScheduledTasks_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lstScheduledTasks_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Public WithEvents lstScheduledTasks As System.Windows.Forms.ListView
	Public WithEvents cboUser As PMUserLookupControl.cboPMUserLookup
	Public WithEvents panMainTab As System.Windows.Forms.Panel
	Private WithEvents _lblTitle_2 As System.Windows.Forms.Label
	Public WithEvents picTitles As System.Windows.Forms.PictureBox
	Public WithEvents uctPMResizer1 As PMResizerControl.uctPMResizer
	Private WithEvents _sbStatusBar_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Private WithEvents _sbStatusBar_Panel2 As System.Windows.Forms.ToolStripStatusLabel
	Private WithEvents _sbStatusBar_Panel3 As System.Windows.Forms.ToolStripStatusLabel
	Private WithEvents _sbStatusBar_Panel4 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents sbStatusBar As System.Windows.Forms.StatusStrip
	Public WithEvents imlToolbarIcons As System.Windows.Forms.ImageList
	Public WithEvents imlScheduledTasks As System.Windows.Forms.ImageList
	Public WithEvents ImageList2 As System.Windows.Forms.ImageList
	Public lblTitle(2) As System.Windows.Forms.Label
	Public mnuViewSortBy(8) As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents Ctx_mnuTask As System.Windows.Forms.ContextMenuStrip
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.MainMenu1 = New System.Windows.Forms.MenuStrip
		Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFileExit = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuTask = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuTaskEdit = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuTaskAssign = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuTaskView = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuView = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuViewSortSchedule = New System.Windows.Forms.ToolStripMenuItem
		Me._mnuViewSortBy_1 = New System.Windows.Forms.ToolStripMenuItem
		Me._mnuViewSortBy_2 = New System.Windows.Forms.ToolStripMenuItem
		Me._mnuViewSortBy_3 = New System.Windows.Forms.ToolStripMenuItem
		Me._mnuViewSortBy_4 = New System.Windows.Forms.ToolStripMenuItem
		Me._mnuViewSortBy_5 = New System.Windows.Forms.ToolStripMenuItem
		Me._mnuViewSortBy_6 = New System.Windows.Forms.ToolStripMenuItem
		Me._mnuViewSortBy_7 = New System.Windows.Forms.ToolStripMenuItem
		Me._mnuViewSortBy_8 = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuViewBar4 = New System.Windows.Forms.ToolStripSeparator
		Me.mnuViewRefresh = New System.Windows.Forms.ToolStripMenuItem
		Me.tbToolBar = New System.Windows.Forms.ToolStrip
		Me._tbToolBar_Button1 = New System.Windows.Forms.ToolStripButton
		Me._tbToolBar_Button2 = New System.Windows.Forms.ToolStripButton
		Me._tbToolBar_Button3 = New System.Windows.Forms.ToolStripSeparator
		Me._tbToolBar_Button4 = New System.Windows.Forms.ToolStripButton
		Me.tmrSystemTasks = New System.Windows.Forms.Timer(components)
		Me.panMainTab = New System.Windows.Forms.Panel
		Me.cboAllUsers = New PMUserLookupControl.cboPMUserLookup
		Me.cboTaskStatus = New System.Windows.Forms.ComboBox
		Me.cboDateRange = New System.Windows.Forms.ComboBox
		Me.cboUserGroup = New PMUGroupLookupCtrl.cboPMUserGroupByUser
		Me.lstScheduledTasks = New System.Windows.Forms.ListView
		Me._lstScheduledTasks_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lstScheduledTasks_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lstScheduledTasks_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me._lstScheduledTasks_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
		Me._lstScheduledTasks_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
		Me._lstScheduledTasks_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
		Me._lstScheduledTasks_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
		Me._lstScheduledTasks_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
		Me._lstScheduledTasks_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
		Me.cboUser = New PMUserLookupControl.cboPMUserLookup
		Me.picTitles = New System.Windows.Forms.PictureBox
		Me._lblTitle_2 = New System.Windows.Forms.Label
		Me.uctPMResizer1 = New PMResizerControl.uctPMResizer
		Me.sbStatusBar = New System.Windows.Forms.StatusStrip
		Me._sbStatusBar_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
		Me._sbStatusBar_Panel2 = New System.Windows.Forms.ToolStripStatusLabel
		Me._sbStatusBar_Panel3 = New System.Windows.Forms.ToolStripStatusLabel
		Me._sbStatusBar_Panel4 = New System.Windows.Forms.ToolStripStatusLabel
		Me.imlToolbarIcons = New System.Windows.Forms.ImageList
		Me.imlScheduledTasks = New System.Windows.Forms.ImageList
		Me.ImageList2 = New System.Windows.Forms.ImageList
		Me.tbToolBar.SuspendLayout()
		Me.panMainTab.SuspendLayout()
		Me.lstScheduledTasks.SuspendLayout()
		Me.picTitles.SuspendLayout()
		Me.sbStatusBar.SuspendLayout()
		Me.SuspendLayout()
		'Ctx_mnuTask
		Me.Ctx_mnuTask = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.Ctx_mnuTask.Size = New System.Drawing.Size(153, 26)
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' MainMenu1
		' 
		Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuFile, Me.mnuTask, Me.mnuView})
		' 
		' mnuFile
		' 
		Me.mnuFile.Available = True
		Me.mnuFile.Checked = False
		Me.mnuFile.Enabled = True
		Me.mnuFile.Name = "mnuFile"
		Me.mnuFile.Text = "&File"
		Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuFileExit})
		' 
		' mnuFileExit
		' 
		Me.mnuFileExit.Available = True
		Me.mnuFileExit.Checked = False
		Me.mnuFileExit.Enabled = True
		Me.mnuFileExit.Name = "mnuFileExit"
		Me.mnuFileExit.Text = "E&xit"
		' 
		' mnuTask
		' 
		Me.mnuTask.Available = True
		Me.mnuTask.Checked = False
		Me.mnuTask.Enabled = True
		Me.mnuTask.Name = "mnuTask"
		Me.mnuTask.Text = "&Task"
		Me.mnuTask.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuTaskEdit, Me.mnuTaskAssign, Me.mnuTaskView})
		' 
		' mnuTaskEdit
		' 
		Me.mnuTaskEdit.Available = True
		Me.mnuTaskEdit.Checked = False
		Me.mnuTaskEdit.Enabled = True
		Me.mnuTaskEdit.Name = "mnuTaskEdit"
		Me.mnuTaskEdit.ShortcutKeys = CType(System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.E, System.Windows.Forms.Keys)
		Me.mnuTaskEdit.Text = "&Edit"
		' 
		' mnuTaskAssign
		' 
		Me.mnuTaskAssign.Available = True
		Me.mnuTaskAssign.Checked = False
		Me.mnuTaskAssign.Enabled = True
		Me.mnuTaskAssign.Name = "mnuTaskAssign"
		Me.mnuTaskAssign.ShortcutKeys = CType(System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A, System.Windows.Forms.Keys)
		Me.mnuTaskAssign.Text = "&Assign"
		' 
		' mnuTaskView
		' 
		Me.mnuTaskView.Available = True
		Me.mnuTaskView.Checked = False
		Me.mnuTaskView.Enabled = True
		Me.mnuTaskView.Name = "mnuTaskView"
		Me.mnuTaskView.ShortcutKeys = CType(System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V, System.Windows.Forms.Keys)
		Me.mnuTaskView.Text = "&View"
		' 
		' mnuView
		' 
		Me.mnuView.Available = True
		Me.mnuView.Checked = False
		Me.mnuView.Enabled = True
		Me.mnuView.Name = "mnuView"
		Me.mnuView.Text = "&View"
		Me.mnuView.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuViewSortSchedule, Me.mnuViewBar4, Me.mnuViewRefresh})
		' 
		' mnuViewSortSchedule
		' 
		Me.mnuViewSortSchedule.Available = True
		Me.mnuViewSortSchedule.Checked = False
		Me.mnuViewSortSchedule.Enabled = True
		Me.mnuViewSortSchedule.Name = "mnuViewSortSchedule"
		Me.mnuViewSortSchedule.Text = "Sort Schedule"
		Me.mnuViewSortSchedule.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me._mnuViewSortBy_1, Me._mnuViewSortBy_2, Me._mnuViewSortBy_3, Me._mnuViewSortBy_4, Me._mnuViewSortBy_5, Me._mnuViewSortBy_6, Me._mnuViewSortBy_7, Me._mnuViewSortBy_8})
		' 
		' _mnuViewSortBy_1
		' 
		Me._mnuViewSortBy_1.Available = True
		Me._mnuViewSortBy_1.Checked = False
		Me._mnuViewSortBy_1.Enabled = True
		Me._mnuViewSortBy_1.Name = "_mnuViewSortBy_1"
		Me._mnuViewSortBy_1.Text = "by Urgency"
		' 
		' _mnuViewSortBy_2
		' 
		Me._mnuViewSortBy_2.Available = True
		Me._mnuViewSortBy_2.Checked = False
		Me._mnuViewSortBy_2.Enabled = True
		Me._mnuViewSortBy_2.Name = "_mnuViewSortBy_2"
		Me._mnuViewSortBy_2.Text = "by Status"
		' 
		' _mnuViewSortBy_3
		' 
		Me._mnuViewSortBy_3.Available = True
		Me._mnuViewSortBy_3.Checked = False
		Me._mnuViewSortBy_3.Enabled = True
		Me._mnuViewSortBy_3.Name = "_mnuViewSortBy_3"
		Me._mnuViewSortBy_3.Text = "by Due Date"
		' 
		' _mnuViewSortBy_4
		' 
		Me._mnuViewSortBy_4.Available = True
		Me._mnuViewSortBy_4.Checked = False
		Me._mnuViewSortBy_4.Enabled = True
		Me._mnuViewSortBy_4.Name = "_mnuViewSortBy_4"
		Me._mnuViewSortBy_4.Text = "by Description"
		' 
		' _mnuViewSortBy_5
		' 
		Me._mnuViewSortBy_5.Available = True
		Me._mnuViewSortBy_5.Checked = False
		Me._mnuViewSortBy_5.Enabled = True
		Me._mnuViewSortBy_5.Name = "_mnuViewSortBy_5"
		Me._mnuViewSortBy_5.Text = "by Customer"
		' 
		' _mnuViewSortBy_6
		' 
		Me._mnuViewSortBy_6.Available = True
		Me._mnuViewSortBy_6.Checked = False
		Me._mnuViewSortBy_6.Enabled = True
		Me._mnuViewSortBy_6.Name = "_mnuViewSortBy_6"
		Me._mnuViewSortBy_6.Text = "by Type"
		' 
		' _mnuViewSortBy_7
		' 
		Me._mnuViewSortBy_7.Available = True
		Me._mnuViewSortBy_7.Checked = False
		Me._mnuViewSortBy_7.Enabled = True
		Me._mnuViewSortBy_7.Name = "_mnuViewSortBy_7"
		Me._mnuViewSortBy_7.Text = "by User Group"
		' 
		' _mnuViewSortBy_8
		' 
		Me._mnuViewSortBy_8.Available = True
		Me._mnuViewSortBy_8.Checked = False
		Me._mnuViewSortBy_8.Enabled = True
		Me._mnuViewSortBy_8.Name = "_mnuViewSortBy_8"
		Me._mnuViewSortBy_8.Text = "by User"
		' 
		' mnuViewBar4
		' 
		Me.mnuViewBar4.Available = True
		Me.mnuViewBar4.Enabled = True
		Me.mnuViewBar4.Name = "mnuViewBar4"
		' 
		' mnuViewRefresh
		' 
		Me.mnuViewRefresh.Available = True
		Me.mnuViewRefresh.Checked = False
		Me.mnuViewRefresh.Enabled = True
		Me.mnuViewRefresh.Name = "mnuViewRefresh"
		Me.mnuViewRefresh.ShortcutKeys = CType(System.Windows.Forms.Keys.F5, System.Windows.Forms.Keys)
		Me.mnuViewRefresh.Text = "&Refresh"
		' 
		' tbToolBar
		' 
		Me.tbToolBar.CanOverflow = False
		Me.tbToolBar.Dock = System.Windows.Forms.DockStyle.Top
		Me.tbToolBar.ImageList = imlToolbarIcons
		Me.tbToolBar.Location = New System.Drawing.Point(0, 24)
		Me.tbToolBar.Name = "tbToolBar"
		Me.tbToolBar.ShowItemToolTips = True
		Me.tbToolBar.Size = New System.Drawing.Size(724, 28)
		Me.tbToolBar.TabIndex = 0
		Me.tbToolBar.Items.Add(Me._tbToolBar_Button1)
		Me.tbToolBar.Items.Add(Me._tbToolBar_Button2)
		Me.tbToolBar.Items.Add(Me._tbToolBar_Button4)
		' 
		' _tbToolBar_Button1
		' 
		Me._tbToolBar_Button1.AutoSize = False
		Me._tbToolBar_Button1.Checked = True
		Me._tbToolBar_Button1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._tbToolBar_Button1.Enabled = False
		Me._tbToolBar_Button1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._tbToolBar_Button1.Name = "ViewQuickStartBar"
		Me._tbToolBar_Button1.Size = New System.Drawing.Size(24, 22)
		Me._tbToolBar_Button1.Tag = ""
		Me._tbToolBar_Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._tbToolBar_Button1.ToolTipText = "View Quick Start Bar"
		Me._tbToolBar_Button1.Visible = False
		' 
		' _tbToolBar_Button2
		' 
		Me._tbToolBar_Button2.AutoSize = False
		Me._tbToolBar_Button2.Checked = True
		Me._tbToolBar_Button2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._tbToolBar_Button2.Enabled = False
		Me._tbToolBar_Button2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._tbToolBar_Button2.Name = "ViewAvailableTasks"
		Me._tbToolBar_Button2.Size = New System.Drawing.Size(24, 22)
		Me._tbToolBar_Button2.Tag = ""
		Me._tbToolBar_Button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._tbToolBar_Button2.ToolTipText = "View Available Tasks"
		Me._tbToolBar_Button2.Visible = False
		' 
		' _tbToolBar_Button3
		' 
		Me._tbToolBar_Button3.AutoSize = False
		Me._tbToolBar_Button3.Name = ""
		Me._tbToolBar_Button3.Size = New System.Drawing.Size(6, 22)
		Me._tbToolBar_Button3.Tag = ""
		Me._tbToolBar_Button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' _tbToolBar_Button4
		' 
		Me._tbToolBar_Button4.AutoSize = False
		Me._tbToolBar_Button4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._tbToolBar_Button4.ImageIndex = 16
		Me._tbToolBar_Button4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._tbToolBar_Button4.Name = "Refresh"
		Me._tbToolBar_Button4.Size = New System.Drawing.Size(24, 22)
		Me._tbToolBar_Button4.Tag = ""
		Me._tbToolBar_Button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._tbToolBar_Button4.ToolTipText = "Refresh"
		' 
		' tmrSystemTasks
		' 
		Me.tmrSystemTasks.Enabled = False
		Me.tmrSystemTasks.Interval = 60000
		' 
		' panMainTab
		' 
		Me.panMainTab.BackColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.panMainTab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.panMainTab.Controls.Add(Me.cboAllUsers)
		Me.panMainTab.Controls.Add(Me.cboTaskStatus)
		Me.panMainTab.Controls.Add(Me.cboDateRange)
		Me.panMainTab.Controls.Add(Me.cboUserGroup)
		Me.panMainTab.Controls.Add(Me.lstScheduledTasks)
		Me.panMainTab.Controls.Add(Me.cboUser)
		Me.panMainTab.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.panMainTab.Location = New System.Drawing.Point(0, 72)
		Me.panMainTab.Name = "panMainTab"
		Me.panMainTab.Size = New System.Drawing.Size(679, 472)
		Me.panMainTab.TabIndex = 2
		' 
		' cboAllUsers
		' 
		Me.cboAllUsers.Location = New System.Drawing.Point(632, 8)
		Me.cboAllUsers.Name = "cboAllUsers"
		Me.cboAllUsers.Size = New System.Drawing.Size(17, 21)
        Me.cboAllUsers.Sorted = True
		Me.cboAllUsers.TabIndex = 10
		Me.cboAllUsers.Visible = False
		' 
		' cboTaskStatus
		' 
		Me.cboTaskStatus.BackColor = System.Drawing.SystemColors.Window
		Me.cboTaskStatus.CausesValidation = True
		Me.cboTaskStatus.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboTaskStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboTaskStatus.Enabled = True
		Me.cboTaskStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboTaskStatus.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboTaskStatus.IntegralHeight = True
		Me.cboTaskStatus.Location = New System.Drawing.Point(8, 8)
		Me.cboTaskStatus.Name = "cboTaskStatus"
		Me.cboTaskStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboTaskStatus.Size = New System.Drawing.Size(113, 21)
		Me.cboTaskStatus.Sorted = False
		Me.cboTaskStatus.TabIndex = 6
		Me.cboTaskStatus.TabStop = True
		Me.cboTaskStatus.Visible = True
		' 
		' cboDateRange
		' 
		Me.cboDateRange.BackColor = System.Drawing.SystemColors.Window
		Me.cboDateRange.CausesValidation = True
		Me.cboDateRange.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboDateRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboDateRange.Enabled = True
		Me.cboDateRange.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboDateRange.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboDateRange.IntegralHeight = True
		Me.cboDateRange.Location = New System.Drawing.Point(384, 8)
		Me.cboDateRange.Name = "cboDateRange"
		Me.cboDateRange.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboDateRange.Size = New System.Drawing.Size(113, 21)
		Me.cboDateRange.Sorted = False
		Me.cboDateRange.TabIndex = 5
		Me.cboDateRange.TabStop = True
		Me.cboDateRange.Visible = True
		' 
		' cboUserGroup
		' 
		Me.cboUserGroup.Location = New System.Drawing.Point(131, 8)
		Me.cboUserGroup.Name = "cboUserGroup"
		Me.cboUserGroup.Size = New System.Drawing.Size(113, 21)
        Me.cboUserGroup.Sorted = True

		Me.cboUserGroup.TabIndex = 4
		' 
		' lstScheduledTasks
		' 
		Me.lstScheduledTasks.BackColor = System.Drawing.SystemColors.Window
		Me.lstScheduledTasks.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lstScheduledTasks.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lstScheduledTasks.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lstScheduledTasks.HideSelection = True
		Me.lstScheduledTasks.LabelEdit = False
		Me.lstScheduledTasks.LabelWrap = True
		Me.lstScheduledTasks.Location = New System.Drawing.Point(8, 40)
		Me.lstScheduledTasks.Name = "lstScheduledTasks"
		Me.lstScheduledTasks.Size = New System.Drawing.Size(660, 423)
		Me.lstScheduledTasks.SmallImageList = imlScheduledTasks
		Me.lstScheduledTasks.TabIndex = 7
		Me.lstScheduledTasks.View = System.Windows.Forms.View.Details
		Me.lstScheduledTasks.Columns.Add(Me._lstScheduledTasks_ColumnHeader_1)
		Me.lstScheduledTasks.Columns.Add(Me._lstScheduledTasks_ColumnHeader_2)
		Me.lstScheduledTasks.Columns.Add(Me._lstScheduledTasks_ColumnHeader_3)
		Me.lstScheduledTasks.Columns.Add(Me._lstScheduledTasks_ColumnHeader_4)
		Me.lstScheduledTasks.Columns.Add(Me._lstScheduledTasks_ColumnHeader_5)
		Me.lstScheduledTasks.Columns.Add(Me._lstScheduledTasks_ColumnHeader_6)
		Me.lstScheduledTasks.Columns.Add(Me._lstScheduledTasks_ColumnHeader_7)
		Me.lstScheduledTasks.Columns.Add(Me._lstScheduledTasks_ColumnHeader_8)
		Me.lstScheduledTasks.Columns.Add(Me._lstScheduledTasks_ColumnHeader_9)
		' 
		' _lstScheduledTasks_ColumnHeader_1
		' 
		Me._lstScheduledTasks_ColumnHeader_1.Tag = ""
		Me._lstScheduledTasks_ColumnHeader_1.Text = "Urgent"
		Me._lstScheduledTasks_ColumnHeader_1.Width = 28
		' 
		' _lstScheduledTasks_ColumnHeader_2
		' 
		Me._lstScheduledTasks_ColumnHeader_2.Tag = ""
		Me._lstScheduledTasks_ColumnHeader_2.Text = "Status"
		Me._lstScheduledTasks_ColumnHeader_2.Width = 54
		' 
		' _lstScheduledTasks_ColumnHeader_3
		' 
		Me._lstScheduledTasks_ColumnHeader_3.Tag = ""
		Me._lstScheduledTasks_ColumnHeader_3.Text = "Due Date"
		Me._lstScheduledTasks_ColumnHeader_3.Width = 41
		' 
		' _lstScheduledTasks_ColumnHeader_4
		' 
		Me._lstScheduledTasks_ColumnHeader_4.Tag = ""
		Me._lstScheduledTasks_ColumnHeader_4.Text = "Description"
		Me._lstScheduledTasks_ColumnHeader_4.Width = 97
		' 
		' _lstScheduledTasks_ColumnHeader_5
		' 
		Me._lstScheduledTasks_ColumnHeader_5.Tag = ""
		Me._lstScheduledTasks_ColumnHeader_5.Text = "Customer"
		Me._lstScheduledTasks_ColumnHeader_5.Width = 97
		' 
		' _lstScheduledTasks_ColumnHeader_6
		' 
		Me._lstScheduledTasks_ColumnHeader_6.Tag = ""
		Me._lstScheduledTasks_ColumnHeader_6.Text = "Type"
		Me._lstScheduledTasks_ColumnHeader_6.Width = 108
		' 
		' _lstScheduledTasks_ColumnHeader_7
		' 
		Me._lstScheduledTasks_ColumnHeader_7.Tag = ""
		Me._lstScheduledTasks_ColumnHeader_7.Text = "User Group"
		Me._lstScheduledTasks_ColumnHeader_7.Width = 97
		' 
		' _lstScheduledTasks_ColumnHeader_8
		' 
		Me._lstScheduledTasks_ColumnHeader_8.Tag = ""
		Me._lstScheduledTasks_ColumnHeader_8.Text = "User"
		Me._lstScheduledTasks_ColumnHeader_8.Width = 41
		' 
		' _lstScheduledTasks_ColumnHeader_9
		' 
		Me._lstScheduledTasks_ColumnHeader_9.Tag = ""
		Me._lstScheduledTasks_ColumnHeader_9.Text = "DueDateSortable"
		Me._lstScheduledTasks_ColumnHeader_9.Width = 0
		' 
		' cboUser
		' 
		Me.cboUser.Location = New System.Drawing.Point(256, 8)
		Me.cboUser.Name = "cboUser"
		Me.cboUser.Size = New System.Drawing.Size(113, 21)
        Me.cboUser.Sorted = True
		Me.cboUser.TabIndex = 8
		' 
		' picTitles
		' 
		Me.picTitles.BackColor = System.Drawing.SystemColors.Control
		Me.picTitles.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.picTitles.CausesValidation = True
		Me.picTitles.Controls.Add(Me._lblTitle_2)
		Me.picTitles.Cursor = System.Windows.Forms.Cursors.Default
		Me.picTitles.Dock = System.Windows.Forms.DockStyle.Top
		Me.picTitles.Enabled = True
		Me.picTitles.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.picTitles.Location = New System.Drawing.Point(0, 52)
		Me.picTitles.Name = "picTitles"
		Me.picTitles.Size = New System.Drawing.Size(724, 20)
		Me.picTitles.TabIndex = 1
		Me.picTitles.TabStop = False
		Me.picTitles.Visible = True
		' 
		' _lblTitle_2
		' 
		Me._lblTitle_2.AutoSize = False
		Me._lblTitle_2.BackColor = System.Drawing.SystemColors.Control
		Me._lblTitle_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lblTitle_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblTitle_2.Enabled = True
		Me._lblTitle_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._lblTitle_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblTitle_2.Location = New System.Drawing.Point(0, 0)
		Me._lblTitle_2.Name = "_lblTitle_2"
		Me._lblTitle_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblTitle_2.Size = New System.Drawing.Size(663, 18)
		Me._lblTitle_2.TabIndex = 3
		Me._lblTitle_2.Tag = " ListView:"
		Me._lblTitle_2.Text = " Scheduled Tasks && Reminders"
		Me._lblTitle_2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblTitle_2.UseMnemonic = True
		Me._lblTitle_2.Visible = True
		' 
		' uctPMResizer1
		' 
		Me.uctPMResizer1.Location = New System.Drawing.Point(752, 536)
		Me.uctPMResizer1.Name = "uctPMResizer1"
		' 
		' sbStatusBar
		' 
		Me.sbStatusBar.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.sbStatusBar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.sbStatusBar.Location = New System.Drawing.Point(0, 542)
		Me.sbStatusBar.Name = "sbStatusBar"
		Me.sbStatusBar.ShowItemToolTips = True
		Me.sbStatusBar.Size = New System.Drawing.Size(724, 18)
		Me.sbStatusBar.TabIndex = 9
		Me.sbStatusBar.Text = ""
		Me.sbStatusBar.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me._sbStatusBar_Panel1})
		Me.sbStatusBar.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me._sbStatusBar_Panel2})
		Me.sbStatusBar.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me._sbStatusBar_Panel3})
		Me.sbStatusBar.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me._sbStatusBar_Panel4})
		' 
		' _sbStatusBar_Panel1
		' 
		Me._sbStatusBar_Panel1.AutoSize = True
		Me._sbStatusBar_Panel1.AutoSize = False
		Me._sbStatusBar_Panel1.BorderSides = CType(System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom, System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me._sbStatusBar_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
		Me._sbStatusBar_Panel1.DoubleClickEnabled = True
		Me._sbStatusBar_Panel1.Margin = New System.Windows.Forms.Padding(0)
		Me._sbStatusBar_Panel1.Name = ""
		Me._sbStatusBar_Panel1.Size = New System.Drawing.Size(134, 18)
		Me._sbStatusBar_Panel1.Tag = ""
		Me._sbStatusBar_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._sbStatusBar_Panel1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
		' 
		' _sbStatusBar_Panel2
		' 
		Me._sbStatusBar_Panel2.AutoSize = False
		Me._sbStatusBar_Panel2.BorderSides = CType(System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom, System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me._sbStatusBar_Panel2.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
		Me._sbStatusBar_Panel2.DoubleClickEnabled = True
		Me._sbStatusBar_Panel2.Margin = New System.Windows.Forms.Padding(0)
		Me._sbStatusBar_Panel2.Name = ""
		Me._sbStatusBar_Panel2.Size = New System.Drawing.Size(96, 18)
		Me._sbStatusBar_Panel2.Tag = ""
		Me._sbStatusBar_Panel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._sbStatusBar_Panel2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
		' 
		' _sbStatusBar_Panel3
		' 
		Me._sbStatusBar_Panel3.AutoSize = False
		Me._sbStatusBar_Panel3.BorderSides = CType(System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom, System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me._sbStatusBar_Panel3.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
		Me._sbStatusBar_Panel3.DoubleClickEnabled = True
		Me._sbStatusBar_Panel3.Margin = New System.Windows.Forms.Padding(0)
		Me._sbStatusBar_Panel3.Name = ""
		Me._sbStatusBar_Panel3.Size = New System.Drawing.Size(134, 18)
		Me._sbStatusBar_Panel3.Tag = ""
		Me._sbStatusBar_Panel3.Text = "x Item(s) Found"
		Me._sbStatusBar_Panel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._sbStatusBar_Panel3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
		' 
		' _sbStatusBar_Panel4
		' 
		Me._sbStatusBar_Panel4.AutoSize = True
		Me._sbStatusBar_Panel4.AutoSize = False
		Me._sbStatusBar_Panel4.BorderSides = CType(System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom, System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me._sbStatusBar_Panel4.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
		Me._sbStatusBar_Panel4.DoubleClickEnabled = True
		Me._sbStatusBar_Panel4.Margin = New System.Windows.Forms.Padding(0)
		Me._sbStatusBar_Panel4.Name = ""
		Me._sbStatusBar_Panel4.Size = New System.Drawing.Size(344, 18)
		Me._sbStatusBar_Panel4.Tag = ""
		Me._sbStatusBar_Panel4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		Me._sbStatusBar_Panel4.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
		' 
		' imlToolbarIcons
		' 
		Me.imlToolbarIcons.ImageSize = New System.Drawing.Size(16, 16)
		Me.imlToolbarIcons.ImageStream = CType(resources.GetObject("imlToolbarIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.imlToolbarIcons.TransparentColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.imlToolbarIcons.Images.SetKeyName(0, "")
		Me.imlToolbarIcons.Images.SetKeyName(1, "")
		Me.imlToolbarIcons.Images.SetKeyName(2, "")
		Me.imlToolbarIcons.Images.SetKeyName(3, "")
		Me.imlToolbarIcons.Images.SetKeyName(4, "")
		Me.imlToolbarIcons.Images.SetKeyName(5, "")
		Me.imlToolbarIcons.Images.SetKeyName(6, "")
		Me.imlToolbarIcons.Images.SetKeyName(7, "")
		Me.imlToolbarIcons.Images.SetKeyName(8, "")
		Me.imlToolbarIcons.Images.SetKeyName(9, "")
		Me.imlToolbarIcons.Images.SetKeyName(10, "")
		Me.imlToolbarIcons.Images.SetKeyName(11, "")
		Me.imlToolbarIcons.Images.SetKeyName(12, "")
		Me.imlToolbarIcons.Images.SetKeyName(13, "")
		Me.imlToolbarIcons.Images.SetKeyName(14, "")
		Me.imlToolbarIcons.Images.SetKeyName(15, "")
		Me.imlToolbarIcons.Images.SetKeyName(16, "")
		' 
		' imlScheduledTasks
		' 
		Me.imlScheduledTasks.ImageSize = New System.Drawing.Size(16, 16)
		Me.imlScheduledTasks.ImageStream = CType(resources.GetObject("imlScheduledTasks.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.imlScheduledTasks.TransparentColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.imlScheduledTasks.Images.SetKeyName(0, "")
		Me.imlScheduledTasks.Images.SetKeyName(1, "Urgent")
		Me.imlScheduledTasks.Images.SetKeyName(2, "")
		Me.imlScheduledTasks.Images.SetKeyName(3, "")
		Me.imlScheduledTasks.Images.SetKeyName(4, "")
		Me.imlScheduledTasks.Images.SetKeyName(5, "")
		Me.imlScheduledTasks.Images.SetKeyName(6, "")
		Me.imlScheduledTasks.Images.SetKeyName(7, "")
		' 
		' ImageList2
		' 
		Me.ImageList2.ImageSize = New System.Drawing.Size(16, 16)
		Me.ImageList2.ImageStream = CType(resources.GetObject("ImageList2.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.ImageList2.TransparentColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.ImageList2.Images.SetKeyName(0, "")
		Me.ImageList2.Images.SetKeyName(1, "")
		Me.ImageList2.Images.SetKeyName(2, "")
		Me.ImageList2.Images.SetKeyName(3, "")
		Me.ImageList2.Images.SetKeyName(4, "")
		Me.ImageList2.Images.SetKeyName(5, "")
		Me.ImageList2.Images.SetKeyName(6, "")
		Me.ImageList2.Images.SetKeyName(7, "")
		Me.ImageList2.Images.SetKeyName(8, "")
		Me.ImageList2.Images.SetKeyName(9, "")
		Me.ImageList2.Images.SetKeyName(10, "")
		Me.ImageList2.Images.SetKeyName(11, "")
		Me.ImageList2.Images.SetKeyName(12, "")
		Me.ImageList2.Images.SetKeyName(13, "")
		Me.ImageList2.Images.SetKeyName(14, "")
		Me.ImageList2.Images.SetKeyName(15, "")
		Me.ImageList2.Images.SetKeyName(16, "")
		' 
		' frmInterface
		' 
		Me.Text = "Scheduled Tasks By Key:  "
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(724, 560)
		Me.ControlBox = True
		Me.Controls.Add(Me.tbToolBar)
		Me.Controls.Add(Me.panMainTab)
		Me.Controls.Add(Me.picTitles)
		Me.Controls.Add(Me.uctPMResizer1)
		Me.Controls.Add(Me.sbStatusBar)
		Me.Controls.Add(MainMenu1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmInterface.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(102, 77)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
		Me.listViewHelper1.SetSorted(Me.lstScheduledTasks, True)
		Me.listViewHelper1.SetItemClickMethod(Me.lstScheduledTasks, "lstScheduledTasks_ItemClick")
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lstScheduledTasks, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tbToolBar.ResumeLayout(False)
		Me.panMainTab.ResumeLayout(False)
		Me.lstScheduledTasks.ResumeLayout(False)
		Me.picTitles.ResumeLayout(False)
		Me.sbStatusBar.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub InitializemnuViewSortBy()
		Me.mnuViewSortBy(1) = _mnuViewSortBy_1
		Me.mnuViewSortBy(2) = _mnuViewSortBy_2
		Me.mnuViewSortBy(3) = _mnuViewSortBy_3
		Me.mnuViewSortBy(4) = _mnuViewSortBy_4
		Me.mnuViewSortBy(5) = _mnuViewSortBy_5
		Me.mnuViewSortBy(6) = _mnuViewSortBy_6
		Me.mnuViewSortBy(7) = _mnuViewSortBy_7
		Me.mnuViewSortBy(8) = _mnuViewSortBy_8
	End Sub
	Sub InitializelblTitle()
		Me.lblTitle(2) = _lblTitle_2
	End Sub
	Sub lstScheduledTasks_InitializeColumnKeys()
		Me._lstScheduledTasks_ColumnHeader_1.Name = "Urgent"
		Me._lstScheduledTasks_ColumnHeader_2.Name = "Status"
		Me._lstScheduledTasks_ColumnHeader_3.Name = "DueDate"
		Me._lstScheduledTasks_ColumnHeader_4.Name = "Description"
		Me._lstScheduledTasks_ColumnHeader_5.Name = "Customer"
		Me._lstScheduledTasks_ColumnHeader_6.Name = "Type"
		Me._lstScheduledTasks_ColumnHeader_7.Name = "UserGroup"
		Me._lstScheduledTasks_ColumnHeader_8.Name = "User"
		Me._lstScheduledTasks_ColumnHeader_9.Name = "DueDateSortable"
	End Sub
#End Region 
End Class