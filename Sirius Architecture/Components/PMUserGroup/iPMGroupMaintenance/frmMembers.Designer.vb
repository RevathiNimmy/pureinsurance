<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMembers
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwAll_InitializeColumnKeys()
		lvwContents_InitializeColumnKeys()
		Form_Initialize_Renamed()
	End Sub
    Private Sub Ctx_mnuSupervisor_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuSupervisor.Opening
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        Ctx_mnuSupervisor.Items.Clear()
        'We are moving the submenus from original menu to the context menu before displaying it
        For Each item As System.Windows.Forms.ToolStripItem In mnuSupervisor.DropDownItems
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            Ctx_mnuSupervisor.Items.Add(item)
        Next item
        e.Cancel = False
    End Sub
    Private Sub Ctx_mnuSupervisor_Closing(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuSupervisor.Closing
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        'We are moving the submenus the context menu back to the original menu after displaying
        For Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuSupervisor.Items
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            mnuSupervisor.DropDownItems.Add(item)
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
    Public WithEvents imgGroup As System.Windows.Forms.ImageList
    Public WithEvents uctPMResizer1 As PMResizerControl.uctPMResizer
    Public WithEvents Ctx_mnuSupervisor As System.Windows.Forms.ContextMenuStrip
    'TODOLIST-Commented the listviewhelper as it was conflicting with icon displai in listview)
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMembers))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdNewUser = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdAddMembers = New System.Windows.Forms.Button
        Me.cmdDeleteMembers = New System.Windows.Forms.Button
        Me.cmdAddAllMembers = New System.Windows.Forms.Button
        Me.cmdDeleteAllMembers = New System.Windows.Forms.Button
        Me.imgGroup = New System.Windows.Forms.ImageList(Me.components)
        Me.uctPMResizer1 = New PMResizerControl.uctPMResizer
        Me.Ctx_mnuSupervisor = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuSupervisor = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSuper = New System.Windows.Forms.ToolStripMenuItem
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.tabMembers = New System.Windows.Forms.TabControl
        Me._tabMembers_TabPage0 = New System.Windows.Forms.TabPage
        Me.Panel6 = New System.Windows.Forms.Panel
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.lvwContents = New System.Windows.Forms.ListView
        Me._lvwContents_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwContents_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.lvwAll = New System.Windows.Forms.ListView
        Me._lvwAll_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwAll_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.lblContents = New System.Windows.Forms.Label
        Me.lblAll = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.MainMenu1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.tabMembers.SuspendLayout()
        Me._tabMembers_TabPage0.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdNewUser
        '
        Me.cmdNewUser.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewUser.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewUser.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewUser.Location = New System.Drawing.Point(4, 7)
        Me.cmdNewUser.Name = "cmdNewUser"
        Me.cmdNewUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewUser.Size = New System.Drawing.Size(73, 22)
        Me.cmdNewUser.TabIndex = 16
        Me.cmdNewUser.Text = "&New User"
        Me.cmdNewUser.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdNewUser, "Add a new user")
        Me.cmdNewUser.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(545, 6)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 15
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdHelp, "Cancel changes and return to previous screen")
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(385, 6)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 13
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdOK, "Accept Changes and return to previous screen")
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(465, 6)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 14
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdCancel, "Cancel changes and return to previous screen")
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdAddMembers
        '
        Me.cmdAddMembers.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddMembers.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddMembers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddMembers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddMembers.Location = New System.Drawing.Point(6, 68)
        Me.cmdAddMembers.Name = "cmdAddMembers"
        Me.cmdAddMembers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddMembers.Size = New System.Drawing.Size(89, 22)
        Me.cmdAddMembers.TabIndex = 6
        Me.cmdAddMembers.Text = "Members -&>"
        Me.cmdAddMembers.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdAddMembers, "Select all chosen available members")
        Me.cmdAddMembers.UseVisualStyleBackColor = False
        '
        'cmdDeleteMembers
        '
        Me.cmdDeleteMembers.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteMembers.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteMembers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteMembers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteMembers.Location = New System.Drawing.Point(6, 176)
        Me.cmdDeleteMembers.Name = "cmdDeleteMembers"
        Me.cmdDeleteMembers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteMembers.Size = New System.Drawing.Size(89, 22)
        Me.cmdDeleteMembers.TabIndex = 8
        Me.cmdDeleteMembers.Text = "&<- Members"
        Me.cmdDeleteMembers.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdDeleteMembers, "Unselect highlighted members")
        Me.cmdDeleteMembers.UseVisualStyleBackColor = False
        '
        'cmdAddAllMembers
        '
        Me.cmdAddAllMembers.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddAllMembers.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddAllMembers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddAllMembers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddAllMembers.Location = New System.Drawing.Point(6, 105)
        Me.cmdAddAllMembers.Name = "cmdAddAllMembers"
        Me.cmdAddAllMembers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddAllMembers.Size = New System.Drawing.Size(89, 22)
        Me.cmdAddAllMembers.TabIndex = 7
        Me.cmdAddAllMembers.Text = "Members ->>"
        Me.cmdAddAllMembers.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdAddAllMembers, "Select all available members")
        Me.cmdAddAllMembers.UseVisualStyleBackColor = False
        '
        'cmdDeleteAllMembers
        '
        Me.cmdDeleteAllMembers.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteAllMembers.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteAllMembers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteAllMembers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteAllMembers.Location = New System.Drawing.Point(6, 220)
        Me.cmdDeleteAllMembers.Name = "cmdDeleteAllMembers"
        Me.cmdDeleteAllMembers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteAllMembers.Size = New System.Drawing.Size(89, 22)
        Me.cmdDeleteAllMembers.TabIndex = 9
        Me.cmdDeleteAllMembers.Text = "<<- Members"
        Me.cmdDeleteAllMembers.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdDeleteAllMembers, "Unselect all members")
        Me.cmdDeleteAllMembers.UseVisualStyleBackColor = False
        '
        'imgGroup
        '
        Me.imgGroup.ImageStream = CType(resources.GetObject("imgGroup.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgGroup.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.imgGroup.Images.SetKeyName(0, "user")
        Me.imgGroup.Images.SetKeyName(1, "group")
        Me.imgGroup.Images.SetKeyName(2, "supervisor")
        '
        'uctPMResizer1
        '
        Me.uctPMResizer1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMResizer1.Location = New System.Drawing.Point(184, 408)
        Me.uctPMResizer1.Name = "uctPMResizer1"
        Me.uctPMResizer1.Size = New System.Drawing.Size(32, 30)
        Me.uctPMResizer1.TabIndex = 13
        Me.uctPMResizer1.Visible = False
        '
        'Ctx_mnuSupervisor
        '
        Me.Ctx_mnuSupervisor.Name = "Ctx_mnuSupervisor"
        Me.Ctx_mnuSupervisor.Size = New System.Drawing.Size(61, 4)
        '
        'mnuSupervisor
        '
        Me.mnuSupervisor.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSuper})
        Me.mnuSupervisor.Enabled = False
        Me.mnuSupervisor.Name = "mnuSupervisor"
        Me.mnuSupervisor.Size = New System.Drawing.Size(70, 20)
        Me.mnuSupervisor.Text = "Supervisor"
        '
        'mnuSuper
        '
        Me.mnuSuper.Name = "mnuSuper"
        Me.mnuSuper.Size = New System.Drawing.Size(125, 22)
        Me.mnuSuper.Text = "Supervisor"
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSupervisor})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(619, 24)
        Me.MainMenu1.TabIndex = 14
        Me.MainMenu1.Visible = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.tabMembers)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(619, 408)
        Me.Panel1.TabIndex = 15
        '
        'tabMembers
        '
        Me.tabMembers.Controls.Add(Me._tabMembers_TabPage0)
        Me.tabMembers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMembers.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMembers.ItemSize = New System.Drawing.Size(199, 18)
        Me.tabMembers.Location = New System.Drawing.Point(0, 0)
        Me.tabMembers.Multiline = True
        Me.tabMembers.Name = "tabMembers"
        Me.tabMembers.SelectedIndex = 0
        Me.tabMembers.Size = New System.Drawing.Size(619, 408)
        Me.tabMembers.TabIndex = 1
        Me.tabMembers.TabStop = False
        '
        '_tabMembers_TabPage0
        '
        Me._tabMembers_TabPage0.Controls.Add(Me.Panel6)
        Me._tabMembers_TabPage0.Controls.Add(Me.Panel5)
        Me._tabMembers_TabPage0.Controls.Add(Me.Panel4)
        Me._tabMembers_TabPage0.Controls.Add(Me.Panel3)
        Me._tabMembers_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMembers_TabPage0.Name = "_tabMembers_TabPage0"
        Me._tabMembers_TabPage0.Size = New System.Drawing.Size(611, 382)
        Me._tabMembers_TabPage0.TabIndex = 1
        Me._tabMembers_TabPage0.Text = "1 - Membership Details"
        '
        'Panel6
        '
        Me.Panel6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.Panel6.Controls.Add(Me.cmdAddMembers)
        Me.Panel6.Controls.Add(Me.cmdDeleteMembers)
        Me.Panel6.Controls.Add(Me.cmdAddAllMembers)
        Me.Panel6.Controls.Add(Me.cmdDeleteAllMembers)
        Me.Panel6.Location = New System.Drawing.Point(261, 34)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(101, 348)
        Me.Panel6.TabIndex = 15
        '
        'Panel5
        '
        Me.Panel5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.Panel5.Controls.Add(Me.lvwContents)
        Me.Panel5.Location = New System.Drawing.Point(362, 34)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(249, 348)
        Me.Panel5.TabIndex = 14
        '
        'lvwContents
        '
        Me.lvwContents.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwContents.BackColor = System.Drawing.SystemColors.Window
        Me.lvwContents.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwContents_ColumnHeader_1, Me._lvwContents_ColumnHeader_2})
        Me.lvwContents.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwContents.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwContents.HideSelection = False
        Me.lvwContents.LargeImageList = Me.imgGroup
        Me.lvwContents.Location = New System.Drawing.Point(0, 0)
        Me.lvwContents.Name = "lvwContents"
        Me.lvwContents.Size = New System.Drawing.Size(249, 348)
        Me.lvwContents.SmallImageList = Me.imgGroup
        Me.lvwContents.TabIndex = 7
        Me.lvwContents.UseCompatibleStateImageBehavior = False
        Me.lvwContents.View = System.Windows.Forms.View.Details
        '
        '_lvwContents_ColumnHeader_1
        '
        Me._lvwContents_ColumnHeader_1.Tag = ""
        Me._lvwContents_ColumnHeader_1.Text = "Username"
        Me._lvwContents_ColumnHeader_1.Width = 161
        '
        '_lvwContents_ColumnHeader_2
        '
        Me._lvwContents_ColumnHeader_2.Tag = ""
        Me._lvwContents_ColumnHeader_2.Text = "Authority"
        Me._lvwContents_ColumnHeader_2.Width = 161
        '
        'Panel4
        '
        Me.Panel4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel4.Controls.Add(Me.lvwAll)
        Me.Panel4.Location = New System.Drawing.Point(0, 34)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(261, 348)
        Me.Panel4.TabIndex = 13
        '
        'lvwAll
        '
        Me.lvwAll.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwAll.BackColor = System.Drawing.SystemColors.Window
        Me.lvwAll.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwAll_ColumnHeader_1, Me._lvwAll_ColumnHeader_2})
        Me.lvwAll.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwAll.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwAll.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lvwAll.HideSelection = False
        Me.lvwAll.LargeImageList = Me.imgGroup
        Me.lvwAll.Location = New System.Drawing.Point(0, 0)
        Me.lvwAll.Name = "lvwAll"
        Me.lvwAll.Size = New System.Drawing.Size(261, 348)
        Me.lvwAll.SmallImageList = Me.imgGroup
        Me.lvwAll.TabIndex = 2
        Me.lvwAll.UseCompatibleStateImageBehavior = False
        Me.lvwAll.View = System.Windows.Forms.View.Details
        '
        '_lvwAll_ColumnHeader_1
        '
        Me._lvwAll_ColumnHeader_1.Tag = ""
        Me._lvwAll_ColumnHeader_1.Text = "Userename"
        Me._lvwAll_ColumnHeader_1.Width = 161
        '
        '_lvwAll_ColumnHeader_2
        '
        Me._lvwAll_ColumnHeader_2.Tag = ""
        Me._lvwAll_ColumnHeader_2.Text = "Authority"
        Me._lvwAll_ColumnHeader_2.Width = 161
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.lblContents)
        Me.Panel3.Controls.Add(Me.lblAll)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(611, 34)
        Me.Panel3.TabIndex = 12
        '
        'lblContents
        '
        Me.lblContents.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblContents.BackColor = System.Drawing.SystemColors.Control
        Me.lblContents.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblContents.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblContents.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblContents.Location = New System.Drawing.Point(359, 9)
        Me.lblContents.Name = "lblContents"
        Me.lblContents.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblContents.Size = New System.Drawing.Size(145, 17)
        Me.lblContents.TabIndex = 12
        Me.lblContents.Text = "Selected Members"
        '
        'lblAll
        '
        Me.lblAll.BackColor = System.Drawing.SystemColors.Control
        Me.lblAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAll.Location = New System.Drawing.Point(5, 9)
        Me.lblAll.Name = "lblAll"
        Me.lblAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAll.Size = New System.Drawing.Size(145, 17)
        Me.lblAll.TabIndex = 13
        Me.lblAll.Text = "Available Members"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.cmdNewUser)
        Me.Panel2.Controls.Add(Me.cmdHelp)
        Me.Panel2.Controls.Add(Me.cmdOK)
        Me.Panel2.Controls.Add(Me.cmdCancel)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 408)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(619, 32)
        Me.Panel2.TabIndex = 16
        '
        'frmMembers
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(619, 440)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.uctPMResizer1)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(627, 467)
        Me.Name = "frmMembers"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmMembers"
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.tabMembers.ResumeLayout(False)
        Me._tabMembers_TabPage0.ResumeLayout(False)
        Me.Panel6.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub lvwAll_InitializeColumnKeys()
        Me._lvwAll_ColumnHeader_1.Name = ""
        Me._lvwAll_ColumnHeader_2.Name = ""
    End Sub
    Sub lvwContents_InitializeColumnKeys()
        Me._lvwContents_ColumnHeader_1.Name = ""
        Me._lvwContents_ColumnHeader_2.Name = ""
    End Sub
    Public WithEvents mnuSupervisor As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuSuper As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Public WithEvents tabMembers As System.Windows.Forms.TabControl
    Private WithEvents _tabMembers_TabPage0 As System.Windows.Forms.TabPage
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Public WithEvents cmdNewUser As System.Windows.Forms.Button
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Public WithEvents lblContents As System.Windows.Forms.Label
    Public WithEvents lblAll As System.Windows.Forms.Label
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Public WithEvents lvwAll As System.Windows.Forms.ListView
    Private WithEvents _lvwAll_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAll_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
    Public WithEvents cmdAddMembers As System.Windows.Forms.Button
    Public WithEvents cmdDeleteMembers As System.Windows.Forms.Button
    Public WithEvents cmdAddAllMembers As System.Windows.Forms.Button
    Public WithEvents cmdDeleteAllMembers As System.Windows.Forms.Button
    Public WithEvents lvwContents As System.Windows.Forms.ListView
    Private WithEvents _lvwContents_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwContents_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
#End Region
End Class