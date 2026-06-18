<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lstScheduledTasks_InitializeColumnKeys()
		Form_Initialize_Renamed()
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
	Public WithEvents mnuEdit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuAssign As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuView As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuStart As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuComplete As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuInComplete As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuTaskLog As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuTask As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	Public WithEvents cboTaskStatus As System.Windows.Forms.ComboBox
	Public WithEvents cboDateRange As System.Windows.Forms.ComboBox
	Public WithEvents uctPMResizer1 As PMResizerControl.uctPMResizer
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
	Public WithEvents cboUserGroup As PMUGroupLookupCtrl.cboPMUserGroupByUser
	Public WithEvents cboUser As PMUserLookupControl.cboPMUserLookup
	Public WithEvents imlScheduledTasks As System.Windows.Forms.ImageList
	Public WithEvents Ctx_mnuTask As System.Windows.Forms.ContextMenuStrip
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip
        Me.mnuTask = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEdit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAssign = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuView = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuStart = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuComplete = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuInComplete = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuTaskLog = New System.Windows.Forms.ToolStripMenuItem
        Me.cboTaskStatus = New System.Windows.Forms.ComboBox
        Me.cboDateRange = New System.Windows.Forms.ComboBox
        Me.uctPMResizer1 = New PMResizerControl.uctPMResizer
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
        Me.imlScheduledTasks = New System.Windows.Forms.ImageList(Me.components)
        Me.Ctx_mnuTask = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cboUserGroup = New PMUGroupLookupCtrl.cboPMUserGroupByUser
        Me.cboUser = New PMUserLookupControl.cboPMUserLookup
        Me.MainMenu1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuTask})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(738, 24)
        Me.MainMenu1.TabIndex = 5
        '
        'mnuTask
        '
        Me.mnuTask.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuEdit, Me.mnuAssign, Me.mnuView, Me.mnuStart, Me.mnuComplete, Me.mnuInComplete, Me.mnuTaskLog})
        Me.mnuTask.Name = "mnuTask"
        Me.mnuTask.Size = New System.Drawing.Size(41, 20)
        Me.mnuTask.Text = "Task"
        '
        'mnuEdit
        '
        Me.mnuEdit.Name = "mnuEdit"
        Me.mnuEdit.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.mnuEdit.Size = New System.Drawing.Size(183, 22)
        Me.mnuEdit.Text = "Edit"
        '
        'mnuAssign
        '
        Me.mnuAssign.Name = "mnuAssign"
        Me.mnuAssign.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.mnuAssign.Size = New System.Drawing.Size(183, 22)
        Me.mnuAssign.Text = "Assign             "
        '
        'mnuView
        '
        Me.mnuView.Name = "mnuView"
        Me.mnuView.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.mnuView.Size = New System.Drawing.Size(183, 22)
        Me.mnuView.Text = "View"
        '
        'mnuStart
        '
        Me.mnuStart.Name = "mnuStart"
        Me.mnuStart.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.mnuStart.Size = New System.Drawing.Size(183, 22)
        Me.mnuStart.Text = "Start"
        '
        'mnuComplete
        '
        Me.mnuComplete.Name = "mnuComplete"
        Me.mnuComplete.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.mnuComplete.Size = New System.Drawing.Size(183, 22)
        Me.mnuComplete.Text = "Complete"
        '
        'mnuInComplete
        '
        Me.mnuInComplete.Name = "mnuInComplete"
        Me.mnuInComplete.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.I), System.Windows.Forms.Keys)
        Me.mnuInComplete.Size = New System.Drawing.Size(183, 22)
        Me.mnuInComplete.Text = "Incomplete"
        '
        'mnuTaskLog
        '
        Me.mnuTaskLog.Name = "mnuTaskLog"
        Me.mnuTaskLog.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.L), System.Windows.Forms.Keys)
        Me.mnuTaskLog.Size = New System.Drawing.Size(183, 22)
        Me.mnuTaskLog.Text = "Task Log"
        '
        'cboTaskStatus
        '
        Me.cboTaskStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cboTaskStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTaskStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTaskStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTaskStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTaskStatus.Location = New System.Drawing.Point(3, 26)
        Me.cboTaskStatus.Name = "cboTaskStatus"
        Me.cboTaskStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTaskStatus.Size = New System.Drawing.Size(113, 21)
        Me.cboTaskStatus.TabIndex = 0
        '
        'cboDateRange
        '
        Me.cboDateRange.BackColor = System.Drawing.SystemColors.Window
        Me.cboDateRange.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboDateRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDateRange.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboDateRange.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboDateRange.Location = New System.Drawing.Point(371, 26)
        Me.cboDateRange.Name = "cboDateRange"
        Me.cboDateRange.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboDateRange.Size = New System.Drawing.Size(113, 21)
        Me.cboDateRange.TabIndex = 3
        '
        'uctPMResizer1
        '
        Me.uctPMResizer1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMResizer1.Location = New System.Drawing.Point(0, 24)
        Me.uctPMResizer1.Name = "uctPMResizer1"
        Me.uctPMResizer1.Size = New System.Drawing.Size(32, 30)
        Me.uctPMResizer1.TabIndex = 4
        Me.uctPMResizer1.Visible = False
        '
        'lstScheduledTasks
        '
        Me.lstScheduledTasks.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstScheduledTasks.BackColor = System.Drawing.SystemColors.Window
        Me.lstScheduledTasks.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lstScheduledTasks.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lstScheduledTasks_ColumnHeader_1, Me._lstScheduledTasks_ColumnHeader_2, Me._lstScheduledTasks_ColumnHeader_3, Me._lstScheduledTasks_ColumnHeader_4, Me._lstScheduledTasks_ColumnHeader_5, Me._lstScheduledTasks_ColumnHeader_6, Me._lstScheduledTasks_ColumnHeader_7, Me._lstScheduledTasks_ColumnHeader_8, Me._lstScheduledTasks_ColumnHeader_9})
        Me.lstScheduledTasks.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstScheduledTasks.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lstScheduledTasks.FullRowSelect = True
        Me.lstScheduledTasks.GridLines = True
        Me.lstScheduledTasks.Location = New System.Drawing.Point(3, 51)
        Me.lstScheduledTasks.MultiSelect = False
        Me.lstScheduledTasks.Name = "lstScheduledTasks"
        Me.lstScheduledTasks.Size = New System.Drawing.Size(732, 377)
        Me.lstScheduledTasks.SmallImageList = Me.imlScheduledTasks
        Me.lstScheduledTasks.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstScheduledTasks.TabIndex = 4
        Me.lstScheduledTasks.UseCompatibleStateImageBehavior = False
        Me.lstScheduledTasks.View = System.Windows.Forms.View.Details
        '
        '_lstScheduledTasks_ColumnHeader_1
        '
        Me._lstScheduledTasks_ColumnHeader_1.Tag = ""
        Me._lstScheduledTasks_ColumnHeader_1.Text = "Urgent"
        Me._lstScheduledTasks_ColumnHeader_1.Width = 28
        '
        '_lstScheduledTasks_ColumnHeader_2
        '
        Me._lstScheduledTasks_ColumnHeader_2.Tag = ""
        Me._lstScheduledTasks_ColumnHeader_2.Text = "Status"
        Me._lstScheduledTasks_ColumnHeader_2.Width = 54
        '
        '_lstScheduledTasks_ColumnHeader_3
        '
        Me._lstScheduledTasks_ColumnHeader_3.Tag = ""
        Me._lstScheduledTasks_ColumnHeader_3.Text = "Due Date"
        Me._lstScheduledTasks_ColumnHeader_3.Width = 42
        '
        '_lstScheduledTasks_ColumnHeader_4
        '
        Me._lstScheduledTasks_ColumnHeader_4.Tag = ""
        Me._lstScheduledTasks_ColumnHeader_4.Text = "Description"
        Me._lstScheduledTasks_ColumnHeader_4.Width = 97
        '
        '_lstScheduledTasks_ColumnHeader_5
        '
        Me._lstScheduledTasks_ColumnHeader_5.Tag = ""
        Me._lstScheduledTasks_ColumnHeader_5.Text = "Customer"
        Me._lstScheduledTasks_ColumnHeader_5.Width = 97
        '
        '_lstScheduledTasks_ColumnHeader_6
        '
        Me._lstScheduledTasks_ColumnHeader_6.Tag = ""
        Me._lstScheduledTasks_ColumnHeader_6.Text = "Type"
        Me._lstScheduledTasks_ColumnHeader_6.Width = 108
        '
        '_lstScheduledTasks_ColumnHeader_7
        '
        Me._lstScheduledTasks_ColumnHeader_7.Tag = ""
        Me._lstScheduledTasks_ColumnHeader_7.Text = "User Group"
        Me._lstScheduledTasks_ColumnHeader_7.Width = 97
        '
        '_lstScheduledTasks_ColumnHeader_8
        '
        Me._lstScheduledTasks_ColumnHeader_8.Tag = ""
        Me._lstScheduledTasks_ColumnHeader_8.Text = "User"
        Me._lstScheduledTasks_ColumnHeader_8.Width = 42
        '
        '_lstScheduledTasks_ColumnHeader_9
        '
        Me._lstScheduledTasks_ColumnHeader_9.Tag = ""
        Me._lstScheduledTasks_ColumnHeader_9.Text = "DueDateSortable"
        Me._lstScheduledTasks_ColumnHeader_9.Width = 0
        '
        'imlScheduledTasks
        '
        Me.imlScheduledTasks.ImageStream = CType(resources.GetObject("imlScheduledTasks.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imlScheduledTasks.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.imlScheduledTasks.Images.SetKeyName(0, "")
        Me.imlScheduledTasks.Images.SetKeyName(1, "Urgent")
        Me.imlScheduledTasks.Images.SetKeyName(2, "")
        Me.imlScheduledTasks.Images.SetKeyName(3, "")
        Me.imlScheduledTasks.Images.SetKeyName(4, "")
        Me.imlScheduledTasks.Images.SetKeyName(5, "")
        Me.imlScheduledTasks.Images.SetKeyName(6, "")
        Me.imlScheduledTasks.Images.SetKeyName(7, "")
        '
        'Ctx_mnuTask
        '
        Me.Ctx_mnuTask.Name = "Ctx_mnuTask"
        Me.Ctx_mnuTask.Size = New System.Drawing.Size(61, 4)
        '
        'cboUserGroup
        '
        Me.cboUserGroup.DefaultTaskGroupID = 0
        Me.cboUserGroup.FirstItem = "()"
        Me.cboUserGroup.ListIndex = -1
        Me.cboUserGroup.Location = New System.Drawing.Point(126, 26)
        Me.cboUserGroup.Name = "cboUserGroup"
        Me.cboUserGroup.PMUserID = 0
        Me.cboUserGroup.SingleUserGroupID = 0
        Me.cboUserGroup.Size = New System.Drawing.Size(113, 21)
        Me.cboUserGroup.Sorted = True
        Me.cboUserGroup.TabIndex = 1
        Me.cboUserGroup.ToolTipText = ""
        Me.cboUserGroup.UserGroupID = 0
        '
        'cboUser
        '
        Me.cboUser.DefaultUserID = 0
        Me.cboUser.FirstItem = "()"
        Me.cboUser.ListIndex = -1
        Me.cboUser.Location = New System.Drawing.Point(248, 26)
        Me.cboUser.Name = "cboUser"
        Me.cboUser.PMUserGroupID = 0
        Me.cboUser.SingleUserID = 0
        Me.cboUser.Size = New System.Drawing.Size(113, 21)
        Me.cboUser.Sorted = True
        Me.cboUser.TabIndex = 2
        Me.cboUser.ToolTipText = ""
        Me.cboUser.UserID = 0
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(738, 431)
        Me.Controls.Add(Me.cboDateRange)
        Me.Controls.Add(Me.cboTaskStatus)
        Me.Controls.Add(Me.uctPMResizer1)
        Me.Controls.Add(Me.lstScheduledTasks)
        Me.Controls.Add(Me.cboUserGroup)
        Me.Controls.Add(Me.cboUser)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(262, 204)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Task List"
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

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