<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUsers
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwAll_InitializeColumnKeys()
		lvwContents_InitializeColumnKeys()
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
	Public WithEvents cmdNewUser As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents uctPMResizer1 As PMResizerControl.uctPMResizer
	Public WithEvents lblAll As System.Windows.Forms.Label
	Public WithEvents lblContents As System.Windows.Forms.Label
	Public WithEvents imgGroup As System.Windows.Forms.ImageList
	Private WithEvents _lvwContents_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwContents_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwContents As System.Windows.Forms.ListView
	Private WithEvents _lvwAll_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAll_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwAll As System.Windows.Forms.ListView
	Public WithEvents cmdDeleteAllMembers As System.Windows.Forms.Button
	Public WithEvents cmdAddAllMembers As System.Windows.Forms.Button
	Public WithEvents cmdDeleteMembers As System.Windows.Forms.Button
	Public WithEvents cmdAddMembers As System.Windows.Forms.Button
	Private WithEvents _tabMembers_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMembers As System.Windows.Forms.TabControl
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUsers))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdNewUser = New System.Windows.Forms.Button
        Me.cmdDeleteAllMembers = New System.Windows.Forms.Button
        Me.cmdAddAllMembers = New System.Windows.Forms.Button
        Me.cmdDeleteMembers = New System.Windows.Forms.Button
        Me.cmdAddMembers = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.uctPMResizer1 = New PMResizerControl.uctPMResizer
        Me.tabMembers = New System.Windows.Forms.TabControl
        Me._tabMembers_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblAll = New System.Windows.Forms.Label
        Me.lblContents = New System.Windows.Forms.Label
        Me.lvwContents = New System.Windows.Forms.ListView
        Me.imgGroup = New System.Windows.Forms.ImageList(Me.components)
        Me._lvwContents_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwContents_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me.lvwAll = New System.Windows.Forms.ListView
        Me._lvwAll_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwAll_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMembers.SuspendLayout()
        Me._tabMembers_TabPage0.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdNewUser
        '
        Me.cmdNewUser.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewUser.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewUser.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewUser.Location = New System.Drawing.Point(8, 384)
        Me.cmdNewUser.Name = "cmdNewUser"
        Me.cmdNewUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewUser.Size = New System.Drawing.Size(73, 22)
        Me.cmdNewUser.TabIndex = 12
        Me.cmdNewUser.Text = "&New User"
        Me.cmdNewUser.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdNewUser, "Add a new user")
        Me.cmdNewUser.UseVisualStyleBackColor = False
        '
        'cmdDeleteAllMembers
        '
        Me.cmdDeleteAllMembers.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteAllMembers.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteAllMembers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteAllMembers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteAllMembers.Location = New System.Drawing.Point(264, 228)
        Me.cmdDeleteAllMembers.Name = "cmdDeleteAllMembers"
        Me.cmdDeleteAllMembers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteAllMembers.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteAllMembers.TabIndex = 1
        Me.cmdDeleteAllMembers.Text = "<<- Members"
        Me.cmdDeleteAllMembers.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdDeleteAllMembers, "Unselect all members")
        Me.cmdDeleteAllMembers.UseVisualStyleBackColor = False
        '
        'cmdAddAllMembers
        '
        Me.cmdAddAllMembers.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddAllMembers.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddAllMembers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddAllMembers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddAllMembers.Location = New System.Drawing.Point(264, 124)
        Me.cmdAddAllMembers.Name = "cmdAddAllMembers"
        Me.cmdAddAllMembers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddAllMembers.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddAllMembers.TabIndex = 2
        Me.cmdAddAllMembers.Text = "Members ->>"
        Me.cmdAddAllMembers.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdAddAllMembers, "Select all available members")
        Me.cmdAddAllMembers.UseVisualStyleBackColor = False
        '
        'cmdDeleteMembers
        '
        Me.cmdDeleteMembers.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteMembers.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteMembers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteMembers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteMembers.Location = New System.Drawing.Point(264, 188)
        Me.cmdDeleteMembers.Name = "cmdDeleteMembers"
        Me.cmdDeleteMembers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteMembers.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteMembers.TabIndex = 3
        Me.cmdDeleteMembers.Text = "&<- Members"
        Me.cmdDeleteMembers.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdDeleteMembers, "Unselect highlighted members")
        Me.cmdDeleteMembers.UseVisualStyleBackColor = False
        '
        'cmdAddMembers
        '
        Me.cmdAddMembers.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddMembers.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddMembers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddMembers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddMembers.Location = New System.Drawing.Point(264, 84)
        Me.cmdAddMembers.Name = "cmdAddMembers"
        Me.cmdAddMembers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddMembers.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddMembers.TabIndex = 4
        Me.cmdAddMembers.Text = "Members -&>"
        Me.cmdAddMembers.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdAddMembers, "Select all chosen available members")
        Me.cmdAddMembers.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(528, 384)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 11
        Me.cmdHelp.TabStop = False
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
        Me.cmdCancel.Location = New System.Drawing.Point(448, 384)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 10
        Me.cmdCancel.TabStop = False
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(368, 384)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 9
        Me.cmdOK.TabStop = False
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'uctPMResizer1
        '
        Me.uctPMResizer1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMResizer1.Location = New System.Drawing.Point(0, 40)
        Me.uctPMResizer1.Name = "uctPMResizer1"
        Me.uctPMResizer1.Size = New System.Drawing.Size(32, 30)
        Me.uctPMResizer1.TabIndex = 13
        '
        'tabMembers
        '
        Me.tabMembers.Controls.Add(Me._tabMembers_TabPage0)
        Me.tabMembers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMembers.ItemSize = New System.Drawing.Size(199, 18)
        Me.tabMembers.Location = New System.Drawing.Point(0, 0)
        Me.tabMembers.Multiline = True
        Me.tabMembers.Name = "tabMembers"
        Me.tabMembers.SelectedIndex = 0
        Me.tabMembers.Size = New System.Drawing.Size(605, 381)
        Me.tabMembers.TabIndex = 0
        Me.tabMembers.TabStop = False
        '
        '_tabMembers_TabPage0
        '
        Me._tabMembers_TabPage0.Controls.Add(Me.lblAll)
        Me._tabMembers_TabPage0.Controls.Add(Me.lblContents)
        Me._tabMembers_TabPage0.Controls.Add(Me.lvwContents)
        Me._tabMembers_TabPage0.Controls.Add(Me.lvwAll)
        Me._tabMembers_TabPage0.Controls.Add(Me.cmdDeleteAllMembers)
        Me._tabMembers_TabPage0.Controls.Add(Me.cmdAddAllMembers)
        Me._tabMembers_TabPage0.Controls.Add(Me.cmdDeleteMembers)
        Me._tabMembers_TabPage0.Controls.Add(Me.cmdAddMembers)
        Me._tabMembers_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMembers_TabPage0.Name = "_tabMembers_TabPage0"
        Me._tabMembers_TabPage0.Size = New System.Drawing.Size(597, 355)
        Me._tabMembers_TabPage0.TabIndex = 0
        Me._tabMembers_TabPage0.Text = "&1 - Membership Details"
        '
        'lblAll
        '
        Me.lblAll.BackColor = System.Drawing.SystemColors.Control
        Me.lblAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAll.Location = New System.Drawing.Point(16, 12)
        Me.lblAll.Name = "lblAll"
        Me.lblAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAll.Size = New System.Drawing.Size(145, 17)
        Me.lblAll.TabIndex = 7
        Me.lblAll.Text = "Available Members"
        '
        'lblContents
        '
        Me.lblContents.BackColor = System.Drawing.SystemColors.Control
        Me.lblContents.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblContents.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblContents.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblContents.Location = New System.Drawing.Point(344, 12)
        Me.lblContents.Name = "lblContents"
        Me.lblContents.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblContents.Size = New System.Drawing.Size(145, 17)
        Me.lblContents.TabIndex = 8
        Me.lblContents.Text = "Selected Members"
        '
        'lvwContents
        '
        Me.lvwContents.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwContents, "")
        Me.lvwContents.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwContents_ColumnHeader_1, Me._lvwContents_ColumnHeader_2})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwContents, True)
        Me.lvwContents.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwContents.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwContents.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwContents, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwContents, "")
        Me.lvwContents.LargeImageList = Me.imgGroup
        Me.lvwContents.Location = New System.Drawing.Point(344, 28)
        Me.lvwContents.Name = "lvwContents"
        Me.lvwContents.Size = New System.Drawing.Size(241, 313)
        Me.listViewHelper1.SetSmallIcons(Me.lvwContents, "")
        Me.lvwContents.SmallImageList = Me.imgGroup
        Me.listViewHelper1.SetSorted(Me.lvwContents, False)
        Me.listViewHelper1.SetSortKey(Me.lvwContents, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwContents, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwContents.TabIndex = 6
        Me.lvwContents.UseCompatibleStateImageBehavior = False
        Me.lvwContents.View = System.Windows.Forms.View.Details
        '
        'imgGroup
        '
        Me.imgGroup.ImageStream = CType(resources.GetObject("imgGroup.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgGroup.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.imgGroup.Images.SetKeyName(0, "")
        Me.imgGroup.Images.SetKeyName(1, "")
        Me.imgGroup.Images.SetKeyName(2, "")
        '
        '_lvwContents_ColumnHeader_1
        '
        Me._lvwContents_ColumnHeader_1.Tag = ""
        Me._lvwContents_ColumnHeader_1.Text = ""
        Me._lvwContents_ColumnHeader_1.Width = 67
        '
        '_lvwContents_ColumnHeader_2
        '
        Me._lvwContents_ColumnHeader_2.Tag = ""
        Me._lvwContents_ColumnHeader_2.Text = ""
        Me._lvwContents_ColumnHeader_2.Width = 134
        '
        'lvwAll
        '
        Me.lvwAll.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwAll, "")
        Me.lvwAll.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwAll_ColumnHeader_1, Me._lvwAll_ColumnHeader_2})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwAll, True)
        Me.lvwAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwAll.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwAll.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwAll, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwAll, "")
        Me.lvwAll.LargeImageList = Me.imgGroup
        Me.lvwAll.Location = New System.Drawing.Point(16, 28)
        Me.lvwAll.Name = "lvwAll"
        Me.lvwAll.Size = New System.Drawing.Size(241, 313)
        Me.listViewHelper1.SetSmallIcons(Me.lvwAll, "")
        Me.lvwAll.SmallImageList = Me.imgGroup
        Me.listViewHelper1.SetSorted(Me.lvwAll, False)
        Me.listViewHelper1.SetSortKey(Me.lvwAll, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwAll, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwAll.TabIndex = 5
        Me.lvwAll.UseCompatibleStateImageBehavior = False
        Me.lvwAll.View = System.Windows.Forms.View.Details
        '
        '_lvwAll_ColumnHeader_1
        '
        Me._lvwAll_ColumnHeader_1.Tag = ""
        Me._lvwAll_ColumnHeader_1.Text = ""
        Me._lvwAll_ColumnHeader_1.Width = 67
        '
        '_lvwAll_ColumnHeader_2
        '
        Me._lvwAll_ColumnHeader_2.Tag = ""
        Me._lvwAll_ColumnHeader_2.Text = ""
        Me._lvwAll_ColumnHeader_2.Width = 134
        '
        'frmUsers
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(607, 410)
        Me.Controls.Add(Me.cmdNewUser)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.uctPMResizer1)
        Me.Controls.Add(Me.tabMembers)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmUsers"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Users"
        Me.tabMembers.ResumeLayout(False)
        Me._tabMembers_TabPage0.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
	Sub lvwAll_InitializeColumnKeys()
		Me._lvwAll_ColumnHeader_1.Name = ""
		Me._lvwAll_ColumnHeader_2.Name = ""
	End Sub
	Sub lvwContents_InitializeColumnKeys()
		Me._lvwContents_ColumnHeader_1.Name = ""
		Me._lvwContents_ColumnHeader_2.Name = ""
	End Sub
#End Region 
End Class