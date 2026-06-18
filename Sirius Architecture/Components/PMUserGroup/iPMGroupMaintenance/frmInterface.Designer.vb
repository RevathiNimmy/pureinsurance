<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdApply = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.imgGroup = New System.Windows.Forms.ImageList(Me.components)
        Me.uctPMResizer1 = New PMResizerControl.uctPMResizer
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.tabGroups = New System.Windows.Forms.TabControl
        Me._tabGroups_TabPage0 = New System.Windows.Forms.TabPage
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.lvwGroups = New System.Windows.Forms.ListView
        Me._lvwGroups_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwGroups_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwGroups_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Panel1.SuspendLayout()
        Me.tabGroups.SuspendLayout()
        Me._tabGroups_TabPage0.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(484, 16)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 9
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdCancel, "Cancel any changes that have not been applied to the db and exit")
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(404, 16)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 8
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdOK, "Accept any changes and exit")
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdApply
        '
        Me.cmdApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApply.Enabled = False
        Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Location = New System.Drawing.Point(564, 16)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApply.Size = New System.Drawing.Size(73, 22)
        Me.cmdApply.TabIndex = 10
        Me.cmdApply.Text = "A&pply"
        Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdApply, "Apply changes to Database")
        Me.cmdApply.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(7, 3)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 5
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdEdit, "Edit details for selected group")
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(7, 35)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 6
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdAdd, "Add a new group")
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(7, 67)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 7
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdDelete, "Delete selected group")
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'imgGroup
        '
        Me.imgGroup.ImageStream = CType(resources.GetObject("imgGroup.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgGroup.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.imgGroup.Images.SetKeyName(0, "group")
        '
        'uctPMResizer1
        '
        Me.uctPMResizer1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMResizer1.Location = New System.Drawing.Point(120, 384)
        Me.uctPMResizer1.Name = "uctPMResizer1"
        Me.uctPMResizer1.Size = New System.Drawing.Size(32, 30)
        Me.uctPMResizer1.TabIndex = 8
        Me.uctPMResizer1.Visible = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.tabGroups)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(642, 366)
        Me.Panel1.TabIndex = 9
        '
        'tabGroups
        '
        Me.tabGroups.Controls.Add(Me._tabGroups_TabPage0)
        Me.tabGroups.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabGroups.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabGroups.ItemSize = New System.Drawing.Size(119, 18)
        Me.tabGroups.Location = New System.Drawing.Point(0, 0)
        Me.tabGroups.Multiline = True
        Me.tabGroups.Name = "tabGroups"
        Me.tabGroups.SelectedIndex = 0
        Me.tabGroups.Size = New System.Drawing.Size(642, 366)
        Me.tabGroups.TabIndex = 1
        Me.tabGroups.TabStop = False
        '
        '_tabGroups_TabPage0
        '
        Me._tabGroups_TabPage0.Controls.Add(Me.Panel3)
        Me._tabGroups_TabPage0.Controls.Add(Me.Panel4)
        Me._tabGroups_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabGroups_TabPage0.Name = "_tabGroups_TabPage0"
        Me._tabGroups_TabPage0.Size = New System.Drawing.Size(634, 340)
        Me._tabGroups_TabPage0.TabIndex = 1
        Me._tabGroups_TabPage0.Text = "1 - Groups"
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.lvwGroups)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(546, 340)
        Me.Panel3.TabIndex = 5
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
        Me.lvwGroups.Size = New System.Drawing.Size(546, 340)
        Me.lvwGroups.SmallImageList = Me.imgGroup
        Me.lvwGroups.TabIndex = 2
        Me.lvwGroups.UseCompatibleStateImageBehavior = False
        Me.lvwGroups.View = System.Windows.Forms.View.Details
        Me.lvwGroups.FullRowSelect = True
        '
        '_lvwGroups_ColumnHeader_1
        '
        Me._lvwGroups_ColumnHeader_1.Tag = ""
        Me._lvwGroups_ColumnHeader_1.Text = "Name"
        Me._lvwGroups_ColumnHeader_1.Width = 132
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
        Me._lvwGroups_ColumnHeader_3.Text = "Group Type"
        Me._lvwGroups_ColumnHeader_3.Width = 130
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.cmdEdit)
        Me.Panel4.Controls.Add(Me.cmdAdd)
        Me.Panel4.Controls.Add(Me.cmdDelete)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel4.Location = New System.Drawing.Point(546, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(88, 340)
        Me.Panel4.TabIndex = 6
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.cmdCancel)
        Me.Panel2.Controls.Add(Me.cmdOK)
        Me.Panel2.Controls.Add(Me.cmdApply)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 366)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(642, 50)
        Me.Panel2.TabIndex = 10
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(642, 416)
        Me.Controls.Add(Me.uctPMResizer1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel2)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.MinimumSize = New System.Drawing.Size(627, 443)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Group Maintenance"
        Me.Panel1.ResumeLayout(False)
        Me.tabGroups.ResumeLayout(False)
        Me._tabGroups_TabPage0.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Sub lvwGroups_InitializeColumnKeys()
        Me._lvwGroups_ColumnHeader_1.Name = ""
        Me._lvwGroups_ColumnHeader_2.Name = ""
        Me._lvwGroups_ColumnHeader_3.Name = ""
    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Public WithEvents tabGroups As System.Windows.Forms.TabControl
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdApply As System.Windows.Forms.Button
    Private WithEvents _tabGroups_TabPage0 As System.Windows.Forms.TabPage
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Public WithEvents lvwGroups As System.Windows.Forms.ListView
    Private WithEvents _lvwGroups_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwGroups_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwGroups_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Public WithEvents cmdEdit As System.Windows.Forms.Button
    Public WithEvents cmdAdd As System.Windows.Forms.Button
    Public WithEvents cmdDelete As System.Windows.Forms.Button
#End Region
End Class