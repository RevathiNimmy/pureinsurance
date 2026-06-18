<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGroup
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwGroups_InitializeColumnKeys()
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
    Public WithEvents imgGroup As System.Windows.Forms.ImageList
    Public WithEvents uctPMResizer1 As PMResizerControl.uctPMResizer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmGroup))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdMembers = New System.Windows.Forms.Button
        Me.cmdTasks = New System.Windows.Forms.Button
        Me.pnlEffectiveDate = New System.Windows.Forms.Panel
        Me.lblpEffectiveDate = New System.Windows.Forms.Label
        Me.txtGroupName = New System.Windows.Forms.TextBox
        Me.txtGroupDescription = New System.Windows.Forms.TextBox
        Me.chkIsSysAdminGroup = New System.Windows.Forms.CheckBox
        Me.imgGroup = New System.Windows.Forms.ImageList(Me.components)
        Me.uctPMResizer1 = New PMResizerControl.uctPMResizer
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.tabGroup = New System.Windows.Forms.TabControl
        Me._tabGroup_TabPage0 = New System.Windows.Forms.TabPage
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.lvwGroups = New System.Windows.Forms.ListView
        Me._lvwGroups_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwGroups_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwGroups_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.lblGroupName = New System.Windows.Forms.Label
        Me.lblEffectiveDate = New System.Windows.Forms.Label
        Me.lblGroupDescription = New System.Windows.Forms.Label
        Me.lblGroups = New System.Windows.Forms.Label
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.pnlEffectiveDate.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.tabGroup.SuspendLayout()
        Me._tabGroup_TabPage0.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdHelp
        '
        Me.cmdHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(541, 10)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 13
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdHelp, "Help")
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(461, 10)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 12
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdCancel, "Cancel changes and return to previous screen")
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(381, 10)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 11
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdOK, "Accept Changes and return to previous screen")
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdMembers
        '
        Me.cmdMembers.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMembers.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMembers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMembers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMembers.Location = New System.Drawing.Point(13, 8)
        Me.cmdMembers.Name = "cmdMembers"
        Me.cmdMembers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMembers.Size = New System.Drawing.Size(153, 22)
        Me.cmdMembers.TabIndex = 8
        Me.cmdMembers.Text = "Add/Remove &Members"
        Me.cmdMembers.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdMembers, "Maintain Group Memberships")
        Me.cmdMembers.UseVisualStyleBackColor = False
        '
        'cmdTasks
        '
        Me.cmdTasks.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdTasks.BackColor = System.Drawing.SystemColors.Control
        Me.cmdTasks.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdTasks.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTasks.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdTasks.Location = New System.Drawing.Point(429, 8)
        Me.cmdTasks.Name = "cmdTasks"
        Me.cmdTasks.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdTasks.Size = New System.Drawing.Size(153, 22)
        Me.cmdTasks.TabIndex = 9
        Me.cmdTasks.Text = "Add/Remove &Tasks"
        Me.cmdTasks.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdTasks, "Maintain Group Tasks")
        Me.cmdTasks.UseVisualStyleBackColor = False
        '
        'pnlEffectiveDate
        '
        Me.pnlEffectiveDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(184, Byte), Integer))
        Me.pnlEffectiveDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlEffectiveDate.Controls.Add(Me.lblpEffectiveDate)
        Me.pnlEffectiveDate.Font = New System.Drawing.Font("Verdana", 8.26!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlEffectiveDate.Location = New System.Drawing.Point(177, 26)
        Me.pnlEffectiveDate.Name = "pnlEffectiveDate"
        Me.pnlEffectiveDate.Size = New System.Drawing.Size(144, 21)
        Me.pnlEffectiveDate.TabIndex = 16
        Me.ToolTip1.SetToolTip(Me.pnlEffectiveDate, "Date when current changes will be effective")
        '
        'lblpEffectiveDate
        '
        Me.lblpEffectiveDate.AutoSize = True
        Me.lblpEffectiveDate.Location = New System.Drawing.Point(2, 1)
        Me.lblpEffectiveDate.Name = "lblpEffectiveDate"
        Me.lblpEffectiveDate.Size = New System.Drawing.Size(0, 14)
        Me.lblpEffectiveDate.TabIndex = 0
        '
        'txtGroupName
        '
        Me.txtGroupName.AcceptsReturn = True
        Me.txtGroupName.BackColor = System.Drawing.SystemColors.Window
        Me.txtGroupName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtGroupName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGroupName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtGroupName.Location = New System.Drawing.Point(17, 26)
        Me.txtGroupName.MaxLength = 10
        Me.txtGroupName.Name = "txtGroupName"
        Me.txtGroupName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtGroupName.Size = New System.Drawing.Size(136, 20)
        Me.txtGroupName.TabIndex = 15
        Me.ToolTip1.SetToolTip(Me.txtGroupName, "Group Name")
        '
        'txtGroupDescription
        '
        Me.txtGroupDescription.AcceptsReturn = True
        Me.txtGroupDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtGroupDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtGroupDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGroupDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtGroupDescription.Location = New System.Drawing.Point(17, 75)
        Me.txtGroupDescription.MaxLength = 0
        Me.txtGroupDescription.Name = "txtGroupDescription"
        Me.txtGroupDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtGroupDescription.Size = New System.Drawing.Size(296, 20)
        Me.txtGroupDescription.TabIndex = 17
        Me.ToolTip1.SetToolTip(Me.txtGroupDescription, "The description for the group")
        '
        'chkIsSysAdminGroup
        '
        Me.chkIsSysAdminGroup.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsSysAdminGroup.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsSysAdminGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsSysAdminGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsSysAdminGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsSysAdminGroup.Location = New System.Drawing.Point(385, 78)
        Me.chkIsSysAdminGroup.Name = "chkIsSysAdminGroup"
        Me.chkIsSysAdminGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsSysAdminGroup.Size = New System.Drawing.Size(193, 17)
        Me.chkIsSysAdminGroup.TabIndex = 18
        Me.chkIsSysAdminGroup.Text = "System Administration Group"
        Me.ToolTip1.SetToolTip(Me.chkIsSysAdminGroup, "Is This Group A System Administrator")
        Me.chkIsSysAdminGroup.UseVisualStyleBackColor = False
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
        Me.uctPMResizer1.Location = New System.Drawing.Point(16, 384)
        Me.uctPMResizer1.Name = "uctPMResizer1"
        Me.uctPMResizer1.Size = New System.Drawing.Size(32, 30)
        Me.uctPMResizer1.TabIndex = 11
        Me.uctPMResizer1.Visible = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.tabGroup)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(619, 370)
        Me.Panel1.TabIndex = 12
        '
        'tabGroup
        '
        Me.tabGroup.Controls.Add(Me._tabGroup_TabPage0)
        Me.tabGroup.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabGroup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabGroup.ItemSize = New System.Drawing.Size(199, 18)
        Me.tabGroup.Location = New System.Drawing.Point(0, 0)
        Me.tabGroup.Multiline = True
        Me.tabGroup.Name = "tabGroup"
        Me.tabGroup.SelectedIndex = 0
        Me.tabGroup.Size = New System.Drawing.Size(619, 370)
        Me.tabGroup.TabIndex = 1
        Me.tabGroup.TabStop = False
        '
        '_tabGroup_TabPage0
        '
        Me._tabGroup_TabPage0.Controls.Add(Me.Panel4)
        Me._tabGroup_TabPage0.Controls.Add(Me.Panel5)
        Me._tabGroup_TabPage0.Controls.Add(Me.Panel3)
        Me._tabGroup_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabGroup_TabPage0.Name = "_tabGroup_TabPage0"
        Me._tabGroup_TabPage0.Size = New System.Drawing.Size(611, 344)
        Me._tabGroup_TabPage0.TabIndex = 0
        Me._tabGroup_TabPage0.Text = "1 - Group Details"
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.lvwGroups)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel4.Location = New System.Drawing.Point(0, 121)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(611, 184)
        Me.Panel4.TabIndex = 16
        '
        'lvwGroups
        '
        Me.lvwGroups.BackColor = System.Drawing.SystemColors.Window
        Me.lvwGroups.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwGroups_ColumnHeader_1, Me._lvwGroups_ColumnHeader_2, Me._lvwGroups_ColumnHeader_3})
        Me.lvwGroups.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwGroups.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwGroups.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwGroups.HideSelection = False
        Me.lvwGroups.LargeImageList = Me.imgGroup
        Me.lvwGroups.Location = New System.Drawing.Point(0, 0)
        Me.lvwGroups.Name = "lvwGroups"
        Me.lvwGroups.Size = New System.Drawing.Size(611, 184)
        Me.lvwGroups.SmallImageList = Me.imgGroup
        Me.lvwGroups.TabIndex = 7
        Me.lvwGroups.UseCompatibleStateImageBehavior = False
        Me.lvwGroups.View = System.Windows.Forms.View.Details
        '
        '_lvwGroups_ColumnHeader_1
        '
        Me._lvwGroups_ColumnHeader_1.Tag = ""
        Me._lvwGroups_ColumnHeader_1.Text = "Name"
        Me._lvwGroups_ColumnHeader_1.Width = 134
        '
        '_lvwGroups_ColumnHeader_2
        '
        Me._lvwGroups_ColumnHeader_2.Tag = ""
        Me._lvwGroups_ColumnHeader_2.Text = "Description"
        Me._lvwGroups_ColumnHeader_2.Width = 134
        '
        '_lvwGroups_ColumnHeader_3
        '
        Me._lvwGroups_ColumnHeader_3.Tag = ""
        Me._lvwGroups_ColumnHeader_3.Text = "Group Or User Type"
        Me._lvwGroups_ColumnHeader_3.Width = 134
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.lblGroupName)
        Me.Panel5.Controls.Add(Me.lblEffectiveDate)
        Me.Panel5.Controls.Add(Me.lblGroupDescription)
        Me.Panel5.Controls.Add(Me.lblGroups)
        Me.Panel5.Controls.Add(Me.pnlEffectiveDate)
        Me.Panel5.Controls.Add(Me.txtGroupName)
        Me.Panel5.Controls.Add(Me.txtGroupDescription)
        Me.Panel5.Controls.Add(Me.chkIsSysAdminGroup)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel5.Location = New System.Drawing.Point(0, 0)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(611, 121)
        Me.Panel5.TabIndex = 17
        '
        'lblGroupName
        '
        Me.lblGroupName.BackColor = System.Drawing.SystemColors.Control
        Me.lblGroupName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGroupName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGroupName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGroupName.Location = New System.Drawing.Point(19, 4)
        Me.lblGroupName.Name = "lblGroupName"
        Me.lblGroupName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGroupName.Size = New System.Drawing.Size(116, 21)
        Me.lblGroupName.TabIndex = 19
        Me.lblGroupName.Text = "Group Name:"
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(179, 3)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(116, 21)
        Me.lblEffectiveDate.TabIndex = 20
        Me.lblEffectiveDate.Text = "Effective date:"
        '
        'lblGroupDescription
        '
        Me.lblGroupDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblGroupDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGroupDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGroupDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGroupDescription.Location = New System.Drawing.Point(19, 53)
        Me.lblGroupDescription.Name = "lblGroupDescription"
        Me.lblGroupDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGroupDescription.Size = New System.Drawing.Size(116, 21)
        Me.lblGroupDescription.TabIndex = 21
        Me.lblGroupDescription.Text = "Group Description:"
        '
        'lblGroups
        '
        Me.lblGroups.BackColor = System.Drawing.SystemColors.Control
        Me.lblGroups.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGroups.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGroups.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGroups.Location = New System.Drawing.Point(18, 102)
        Me.lblGroups.Name = "lblGroups"
        Me.lblGroups.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGroups.Size = New System.Drawing.Size(116, 21)
        Me.lblGroups.TabIndex = 22
        Me.lblGroups.Text = "Group Membership:"
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.cmdMembers)
        Me.Panel3.Controls.Add(Me.cmdTasks)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(0, 305)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(611, 39)
        Me.Panel3.TabIndex = 15
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.cmdHelp)
        Me.Panel2.Controls.Add(Me.cmdCancel)
        Me.Panel2.Controls.Add(Me.cmdOK)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 370)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(619, 46)
        Me.Panel2.TabIndex = 13
        '
        'frmGroup
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(619, 416)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.uctPMResizer1)
        Me.Controls.Add(Me.Panel2)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(270, 147)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(627, 443)
        Me.Name = "frmGroup"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add Group"
        Me.pnlEffectiveDate.ResumeLayout(False)
        Me.pnlEffectiveDate.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.tabGroup.ResumeLayout(False)
        Me._tabGroup_TabPage0.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Sub lvwGroups_InitializeColumnKeys()
        Me._lvwGroups_ColumnHeader_1.Name = ""
        Me._lvwGroups_ColumnHeader_2.Name = ""
        Me._lvwGroups_ColumnHeader_3.Name = ""
    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Public WithEvents tabGroup As System.Windows.Forms.TabControl
    Private WithEvents _tabGroup_TabPage0 As System.Windows.Forms.TabPage
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Public WithEvents cmdMembers As System.Windows.Forms.Button
    Public WithEvents cmdTasks As System.Windows.Forms.Button
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Public WithEvents lblGroupName As System.Windows.Forms.Label
    Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
    Public WithEvents lblGroupDescription As System.Windows.Forms.Label
    Public WithEvents lblGroups As System.Windows.Forms.Label
    Public WithEvents pnlEffectiveDate As System.Windows.Forms.Panel
    Friend WithEvents lblpEffectiveDate As System.Windows.Forms.Label
    Public WithEvents txtGroupName As System.Windows.Forms.TextBox
    Public WithEvents txtGroupDescription As System.Windows.Forms.TextBox
    Public WithEvents chkIsSysAdminGroup As System.Windows.Forms.CheckBox
    Public WithEvents lvwGroups As System.Windows.Forms.ListView
    Private WithEvents _lvwGroups_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwGroups_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwGroups_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
#End Region
End Class