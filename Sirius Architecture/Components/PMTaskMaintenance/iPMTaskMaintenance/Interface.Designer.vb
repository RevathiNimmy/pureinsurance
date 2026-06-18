<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwTasks_InitializeColumnKeys()
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
    Public WithEvents albImageStore As SListBar.ListBarControl.ListBar
    Public WithEvents uctPMResizer1 As PMResizerControl.uctPMResizer
	Public WithEvents cmdApply As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents imgTask As System.Windows.Forms.ImageList
	Private WithEvents _lvwTasks_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTasks_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTasks_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwTasks As System.Windows.Forms.ListView
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Private WithEvents _tabTask_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabTask As System.Windows.Forms.TabControl
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdApply = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.albImageStore = New SListBar.ListBarControl.ListBar()
        Me.uctPMResizer1 = New PMResizerControl.uctPMResizer
        Me.tabTask = New System.Windows.Forms.TabControl
        Me._tabTask_TabPage0 = New System.Windows.Forms.TabPage
        Me.lvwTasks = New System.Windows.Forms.ListView
        Me._lvwTasks_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwTasks_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwTasks_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me.imgTask = New System.Windows.Forms.ImageList(Me.components)
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        CType(Me.albImageStore, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabTask.SuspendLayout()
        Me._tabTask_TabPage0.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApply.Enabled = False
        Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Location = New System.Drawing.Point(539, 393)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApply.Size = New System.Drawing.Size(73, 22)
        Me.cmdApply.TabIndex = 2
        Me.cmdApply.Text = "&Apply"
        Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdApply, "Apply changes to Database")
        Me.cmdApply.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(379, 393)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdOK, "Accept any changes and exit")
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(459, 392)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdCancel, "Cancel any changes that have not been applied to the db and exit")
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(515, 76)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 6
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdDelete, "Delete selected user")
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(515, 45)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 5
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdAdd, "Add a new task")
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(515, 15)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 4
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdEdit, "Edit details for selected task")
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'albImageStore
        '
        Me.albImageStore.AllowDragGroups = True
        Me.albImageStore.AllowDragItems = True
        Me.albImageStore.CurrentSelected = Nothing
        Me.albImageStore.DrawStyle = SListBar.ListBarControl.ListBarDrawStyle.ListBarDrawStyleOfficeXP
        Me.albImageStore.LargeImageList = Nothing
        Me.albImageStore.Location = New System.Drawing.Point(0, 0)
        Me.albImageStore.Name = "albImageStore"
        Me.albImageStore.SelectedGroup = Nothing
        Me.albImageStore.SelectOnMouseDown = False
        Me.albImageStore.Size = New System.Drawing.Size(150, 150)
        Me.albImageStore.SmallImageList = Nothing
        Me.albImageStore.TabIndex = 0
        Me.albImageStore.ToolTip = Nothing
        Me.albImageStore.Visible = False
        '
        'uctPMResizer1
        '
        Me.uctPMResizer1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMResizer1.Location = New System.Drawing.Point(16, 392)
        Me.uctPMResizer1.Name = "uctPMResizer1"
        Me.uctPMResizer1.Size = New System.Drawing.Size(32, 30)
        Me.uctPMResizer1.TabIndex = 9
        '
        'tabTask
        '
        Me.tabTask.Controls.Add(Me._tabTask_TabPage0)
        Me.tabTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabTask.ItemSize = New System.Drawing.Size(199, 18)
        Me.tabTask.Location = New System.Drawing.Point(8, 8)
        Me.tabTask.Multiline = True
        Me.tabTask.Name = "tabTask"
        Me.tabTask.SelectedIndex = 0
        Me.tabTask.Size = New System.Drawing.Size(605, 381)
        Me.tabTask.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.tabTask.TabIndex = 7
        Me.tabTask.TabStop = False
        '
        '_tabTask_TabPage0
        '
        Me._tabTask_TabPage0.Controls.Add(Me.lvwTasks)
        Me._tabTask_TabPage0.Controls.Add(Me.cmdDelete)
        Me._tabTask_TabPage0.Controls.Add(Me.cmdAdd)
        Me._tabTask_TabPage0.Controls.Add(Me.cmdEdit)
        Me._tabTask_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabTask_TabPage0.Name = "_tabTask_TabPage0"
        Me._tabTask_TabPage0.Size = New System.Drawing.Size(597, 355)
        Me._tabTask_TabPage0.TabIndex = 0
        Me._tabTask_TabPage0.Text = "&1 - Tasks"
        '
        'lvwTasks
        '
        Me.lvwTasks.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwTasks, "")
        Me.lvwTasks.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwTasks_ColumnHeader_1, Me._lvwTasks_ColumnHeader_2, Me._lvwTasks_ColumnHeader_3})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwTasks, True)
        Me.lvwTasks.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwTasks.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwTasks.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwTasks, "lvwTasks_ItemClick")
        Me.listViewHelper1.SetLargeIcons(Me.lvwTasks, "")
        Me.lvwTasks.Location = New System.Drawing.Point(10, 15)
        Me.lvwTasks.Name = "lvwTasks"
        Me.lvwTasks.Size = New System.Drawing.Size(495, 330)
        Me.listViewHelper1.SetSmallIcons(Me.lvwTasks, "")
        Me.lvwTasks.SmallImageList = Me.imgTask
        Me.listViewHelper1.SetSorted(Me.lvwTasks, False)
        Me.listViewHelper1.SetSortKey(Me.lvwTasks, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwTasks, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwTasks.TabIndex = 3
        Me.lvwTasks.UseCompatibleStateImageBehavior = False
        Me.lvwTasks.View = System.Windows.Forms.View.Details
        '
        '_lvwTasks_ColumnHeader_1
        '
        Me._lvwTasks_ColumnHeader_1.Tag = ""
        Me._lvwTasks_ColumnHeader_1.Text = "Task Name"
        Me._lvwTasks_ColumnHeader_1.Width = 132
        '
        '_lvwTasks_ColumnHeader_2
        '
        Me._lvwTasks_ColumnHeader_2.Tag = ""
        Me._lvwTasks_ColumnHeader_2.Text = "Description"
        Me._lvwTasks_ColumnHeader_2.Width = 132
        '
        '_lvwTasks_ColumnHeader_3
        '
        Me._lvwTasks_ColumnHeader_3.Tag = ""
        Me._lvwTasks_ColumnHeader_3.Text = "Effective Date"
        Me._lvwTasks_ColumnHeader_3.Width = 132
        '
        'imgTask
        '
        Me.imgTask.ImageStream = CType(resources.GetObject("imgTask.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgTask.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.imgTask.Images.SetKeyName(0, "task")
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(621, 418)
        Me.Controls.Add(Me.albImageStore)
        Me.Controls.Add(Me.uctPMResizer1)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.tabTask)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(183, 176)
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Task Maintenance"
        CType(Me.albImageStore, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabTask.ResumeLayout(False)
        Me._tabTask_TabPage0.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
	Sub lvwTasks_InitializeColumnKeys()
		Me._lvwTasks_ColumnHeader_1.Name = ""
		Me._lvwTasks_ColumnHeader_2.Name = ""
		Me._lvwTasks_ColumnHeader_3.Name = ""
	End Sub
#End Region 
End Class