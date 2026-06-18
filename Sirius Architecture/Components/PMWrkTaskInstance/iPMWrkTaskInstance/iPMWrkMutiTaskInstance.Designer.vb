<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAssignMultipleTask
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lstScheduledTasks_InitializeColumnKeys()
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
	Private WithEvents _barStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents barStatus As System.Windows.Forms.StatusStrip
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents uctPMResizer1 As PMResizerControl.uctPMResizer
	Public WithEvents imgIcon1 As System.Windows.Forms.PictureBox
	Private WithEvents _lstScheduledTasks_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lstScheduledTasks_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lstScheduledTasks_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lstScheduledTasks_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lstScheduledTasks_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lstScheduledTasks_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lstScheduledTasks_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Public WithEvents lstScheduledTasks As System.Windows.Forms.ListView
	Public WithEvents fraTaskDetails As System.Windows.Forms.GroupBox
	Public WithEvents cboUserGroup As System.Windows.Forms.ComboBox
	Public WithEvents cboGroupUsers As System.Windows.Forms.ComboBox
	Public WithEvents lblUserGroup As System.Windows.Forms.Label
	Public WithEvents lblUser As System.Windows.Forms.Label
	Public WithEvents fraAllocation As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents cboAllUsers As PMUserLookupControl.cboPMUserLookup
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAssignMultipleTask))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.barStatus = New System.Windows.Forms.StatusStrip
        Me._barStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.uctPMResizer1 = New PMResizerControl.uctPMResizer
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.imgIcon1 = New System.Windows.Forms.PictureBox
        Me.fraTaskDetails = New System.Windows.Forms.GroupBox
        Me.lstScheduledTasks = New System.Windows.Forms.ListView
        Me._lstScheduledTasks_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lstScheduledTasks_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lstScheduledTasks_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lstScheduledTasks_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lstScheduledTasks_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lstScheduledTasks_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lstScheduledTasks_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me.fraAllocation = New System.Windows.Forms.GroupBox
        Me.cboUserGroup = New System.Windows.Forms.ComboBox
        Me.cboGroupUsers = New System.Windows.Forms.ComboBox
        Me.lblUserGroup = New System.Windows.Forms.Label
        Me.lblUser = New System.Windows.Forms.Label
        Me.cboAllUsers = New PMUserLookupControl.cboPMUserLookup
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.barStatus.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.imgIcon1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraTaskDetails.SuspendLayout()
        Me.fraAllocation.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'barStatus
        '
        Me.barStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._barStatus_Panel1})
        Me.barStatus.Location = New System.Drawing.Point(0, 384)
        Me.barStatus.Name = "barStatus"
        Me.barStatus.ShowItemToolTips = True
        Me.barStatus.Size = New System.Drawing.Size(619, 22)
        Me.barStatus.TabIndex = 11
        '
        '_barStatus_Panel1
        '
        Me._barStatus_Panel1.AutoSize = False
        Me._barStatus_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._barStatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._barStatus_Panel1.DoubleClickEnabled = True
        Me._barStatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._barStatus_Panel1.Name = "_barStatus_Panel1"
        Me._barStatus_Panel1.Size = New System.Drawing.Size(619, 19)
        Me._barStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(376, 358)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 2
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(456, 358)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(536, 358)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 0
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'uctPMResizer1
        '
        Me.uctPMResizer1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMResizer1.Location = New System.Drawing.Point(8, 384)
        Me.uctPMResizer1.Name = "uctPMResizer1"
        Me.uctPMResizer1.Size = New System.Drawing.Size(32, 30)
        Me.uctPMResizer1.TabIndex = 12
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(199, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(605, 345)
        Me.tabMainTab.TabIndex = 3
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.imgIcon1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraTaskDetails)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraAllocation)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(597, 319)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Task Details"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'imgIcon1
        '
        Me.imgIcon1.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon1.Image = CType(resources.GetObject("imgIcon1.Image"), System.Drawing.Image)
        Me.imgIcon1.Location = New System.Drawing.Point(557, 6)
        Me.imgIcon1.Name = "imgIcon1"
        Me.imgIcon1.Size = New System.Drawing.Size(32, 32)
        Me.imgIcon1.TabIndex = 0
        Me.imgIcon1.TabStop = False
        '
        'fraTaskDetails
        '
        Me.fraTaskDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraTaskDetails.Controls.Add(Me.lstScheduledTasks)
        Me.fraTaskDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTaskDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTaskDetails.Location = New System.Drawing.Point(9, 43)
        Me.fraTaskDetails.Name = "fraTaskDetails"
        Me.fraTaskDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTaskDetails.Size = New System.Drawing.Size(582, 185)
        Me.fraTaskDetails.TabIndex = 4
        Me.fraTaskDetails.TabStop = False
        Me.fraTaskDetails.Text = "Tasks Details"
        '
        'lstScheduledTasks
        '
        Me.lstScheduledTasks.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lstScheduledTasks, "")
        Me.lstScheduledTasks.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lstScheduledTasks_ColumnHeader_1, Me._lstScheduledTasks_ColumnHeader_2, Me._lstScheduledTasks_ColumnHeader_3, Me._lstScheduledTasks_ColumnHeader_4, Me._lstScheduledTasks_ColumnHeader_5, Me._lstScheduledTasks_ColumnHeader_6, Me._lstScheduledTasks_ColumnHeader_7})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lstScheduledTasks, False)
        Me.lstScheduledTasks.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstScheduledTasks.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lstScheduledTasks.FullRowSelect = True
        Me.listViewHelper1.SetItemClickMethod(Me.lstScheduledTasks, "")
        Me.listViewHelper1.SetLargeIcons(Me.lstScheduledTasks, "")
        Me.lstScheduledTasks.Location = New System.Drawing.Point(9, 21)
        Me.lstScheduledTasks.Name = "lstScheduledTasks"
        Me.lstScheduledTasks.Size = New System.Drawing.Size(562, 151)
        Me.listViewHelper1.SetSmallIcons(Me.lstScheduledTasks, "")
        Me.listViewHelper1.SetSorted(Me.lstScheduledTasks, True)
        Me.lstScheduledTasks.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.listViewHelper1.SetSortKey(Me.lstScheduledTasks, 0)
        Me.listViewHelper1.SetSortOrder(Me.lstScheduledTasks, System.Windows.Forms.SortOrder.Descending)
        Me.lstScheduledTasks.TabIndex = 12
        Me.lstScheduledTasks.UseCompatibleStateImageBehavior = False
        Me.lstScheduledTasks.View = System.Windows.Forms.View.Details
        '
        '_lstScheduledTasks_ColumnHeader_1
        '
        Me._lstScheduledTasks_ColumnHeader_1.Text = "Urgent"
        Me._lstScheduledTasks_ColumnHeader_1.Width = 54
        '
        '_lstScheduledTasks_ColumnHeader_2
        '
        Me._lstScheduledTasks_ColumnHeader_2.Text = "Status"
        Me._lstScheduledTasks_ColumnHeader_2.Width = 54
        '
        '_lstScheduledTasks_ColumnHeader_3
        '
        Me._lstScheduledTasks_ColumnHeader_3.Text = "Due Date"
        Me._lstScheduledTasks_ColumnHeader_3.Width = 81
        '
        '_lstScheduledTasks_ColumnHeader_4
        '
        Me._lstScheduledTasks_ColumnHeader_4.Text = "Description"
        Me._lstScheduledTasks_ColumnHeader_4.Width = 188
        '
        '_lstScheduledTasks_ColumnHeader_5
        '
        Me._lstScheduledTasks_ColumnHeader_5.Text = "Customer"
        Me._lstScheduledTasks_ColumnHeader_5.Width = 121
        '
        '_lstScheduledTasks_ColumnHeader_6
        '
        Me._lstScheduledTasks_ColumnHeader_6.Text = "User"
        Me._lstScheduledTasks_ColumnHeader_6.Width = 67
        '
        '_lstScheduledTasks_ColumnHeader_7
        '
        Me._lstScheduledTasks_ColumnHeader_7.Text = "Workflow Information"
        Me._lstScheduledTasks_ColumnHeader_7.Width = 121
        '
        'fraAllocation
        '
        Me.fraAllocation.BackColor = System.Drawing.SystemColors.Control
        Me.fraAllocation.Controls.Add(Me.cboUserGroup)
        Me.fraAllocation.Controls.Add(Me.cboGroupUsers)
        Me.fraAllocation.Controls.Add(Me.lblUserGroup)
        Me.fraAllocation.Controls.Add(Me.lblUser)
        Me.fraAllocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAllocation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAllocation.Location = New System.Drawing.Point(9, 236)
        Me.fraAllocation.Name = "fraAllocation"
        Me.fraAllocation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAllocation.Size = New System.Drawing.Size(582, 65)
        Me.fraAllocation.TabIndex = 5
        Me.fraAllocation.TabStop = False
        Me.fraAllocation.Text = "Allocation"
        '
        'cboUserGroup
        '
        Me.cboUserGroup.BackColor = System.Drawing.SystemColors.Window
        Me.cboUserGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboUserGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboUserGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboUserGroup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboUserGroup.Location = New System.Drawing.Point(104, 24)
        Me.cboUserGroup.Name = "cboUserGroup"
        Me.cboUserGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboUserGroup.Size = New System.Drawing.Size(193, 21)
        Me.cboUserGroup.TabIndex = 10
        '
        'cboGroupUsers
        '
        Me.cboGroupUsers.BackColor = System.Drawing.SystemColors.Window
        Me.cboGroupUsers.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboGroupUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboGroupUsers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboGroupUsers.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboGroupUsers.Location = New System.Drawing.Point(377, 24)
        Me.cboGroupUsers.Name = "cboGroupUsers"
        Me.cboGroupUsers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboGroupUsers.Size = New System.Drawing.Size(189, 21)
        Me.cboGroupUsers.TabIndex = 9
        '
        'lblUserGroup
        '
        Me.lblUserGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblUserGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUserGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUserGroup.Location = New System.Drawing.Point(8, 26)
        Me.lblUserGroup.Name = "lblUserGroup"
        Me.lblUserGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUserGroup.Size = New System.Drawing.Size(65, 19)
        Me.lblUserGroup.TabIndex = 7
        Me.lblUserGroup.Text = "User Group:"
        '
        'lblUser
        '
        Me.lblUser.BackColor = System.Drawing.SystemColors.Control
        Me.lblUser.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUser.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUser.Location = New System.Drawing.Point(333, 26)
        Me.lblUser.Name = "lblUser"
        Me.lblUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUser.Size = New System.Drawing.Size(33, 19)
        Me.lblUser.TabIndex = 6
        Me.lblUser.Text = "User:"
        '
        'cboAllUsers
        '
        Me.cboAllUsers.DefaultUserID = 0
        Me.cboAllUsers.FirstItem = "()"
        Me.cboAllUsers.ListIndex = -1
        Me.cboAllUsers.Location = New System.Drawing.Point(16, 356)
        Me.cboAllUsers.Name = "cboAllUsers"
        Me.cboAllUsers.PMUserGroupID = 0
        Me.cboAllUsers.SingleUserID = 0
        Me.cboAllUsers.Size = New System.Drawing.Size(153, 21)
        Me.cboAllUsers.Sorted = True
        Me.cboAllUsers.TabIndex = 8
        Me.cboAllUsers.ToolTipText = ""
        Me.cboAllUsers.UserID = 0
        Me.cboAllUsers.Visible = False
        '
        'frmAssignMultipleTask
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(619, 406)
        Me.Controls.Add(Me.barStatus)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.uctPMResizer1)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.cboAllUsers)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAssignMultipleTask"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Work Manager - Mutiple Tasks Assignment"
        Me.barStatus.ResumeLayout(False)
        Me.barStatus.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        CType(Me.imgIcon1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraTaskDetails.ResumeLayout(False)
        Me.fraAllocation.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub lstScheduledTasks_InitializeColumnKeys()
		Me._lstScheduledTasks_ColumnHeader_1.Name = "Urgent"
		Me._lstScheduledTasks_ColumnHeader_2.Name = "Status"
		Me._lstScheduledTasks_ColumnHeader_3.Name = "DueDate"
		Me._lstScheduledTasks_ColumnHeader_4.Name = "Description"
		Me._lstScheduledTasks_ColumnHeader_5.Name = "Customer"
		Me._lstScheduledTasks_ColumnHeader_6.Name = "User"
		Me._lstScheduledTasks_ColumnHeader_7.Name = "WorkflowInformation"
	End Sub
#End Region 
End Class