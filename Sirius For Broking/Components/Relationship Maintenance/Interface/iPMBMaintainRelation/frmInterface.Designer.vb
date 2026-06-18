<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwMain_InitializeColumnKeys()
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
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public WithEvents imlListView As System.Windows.Forms.ImageList
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.imlListView = New System.Windows.Forms.ImageList(Me.components)
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.lvwMain = New System.Windows.Forms.ListView
        Me._lvwMain_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwMain_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwMain_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwMain_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me.uctPMResizer = New PMResizerControl.uctPMResizer
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.tabMain = New System.Windows.Forms.TabControl
        Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.LblRelGroup = New System.Windows.Forms.Label
        Me.cboPartyRelationshipGroup = New System.Windows.Forms.ComboBox
        Me.picRelationGroup = New System.Windows.Forms.PictureBox
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me._tabMain_TabPage0.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.picRelationGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'imlListView
        '
        Me.imlListView.ImageStream = CType(resources.GetObject("imlListView.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imlListView.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.imlListView.Images.SetKeyName(0, "Icon")
        '
        'lvwMain
        '
        Me.lvwMain.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwMain, "")
        Me.lvwMain.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwMain_ColumnHeader_1, Me._lvwMain_ColumnHeader_2, Me._lvwMain_ColumnHeader_3, Me._lvwMain_ColumnHeader_4})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwMain, False)
        Me.lvwMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwMain.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwMain.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwMain, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwMain, "")
        Me.lvwMain.LargeImageList = Me.imlListView
        Me.lvwMain.Location = New System.Drawing.Point(0, 0)
        Me.lvwMain.MultiSelect = False
        Me.lvwMain.Name = "lvwMain"
        Me.lvwMain.Size = New System.Drawing.Size(469, 217)
        Me.listViewHelper1.SetSmallIcons(Me.lvwMain, "")
        Me.lvwMain.SmallImageList = Me.imlListView
        Me.listViewHelper1.SetSorted(Me.lvwMain, False)
        Me.listViewHelper1.SetSortKey(Me.lvwMain, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwMain, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwMain.TabIndex = 4
        Me.lvwMain.UseCompatibleStateImageBehavior = False
        Me.lvwMain.View = System.Windows.Forms.View.Details
        '
        '_lvwMain_ColumnHeader_1
        '
        Me._lvwMain_ColumnHeader_1.Tag = ""
        Me._lvwMain_ColumnHeader_1.Text = "Code"
        Me._lvwMain_ColumnHeader_1.Width = 97
        '
        '_lvwMain_ColumnHeader_2
        '
        Me._lvwMain_ColumnHeader_2.Tag = ""
        Me._lvwMain_ColumnHeader_2.Text = "Description"
        Me._lvwMain_ColumnHeader_2.Width = 97
        '
        '_lvwMain_ColumnHeader_3
        '
        Me._lvwMain_ColumnHeader_3.Tag = ""
        Me._lvwMain_ColumnHeader_3.Text = "Partner"
        Me._lvwMain_ColumnHeader_3.Width = 97
        '
        '_lvwMain_ColumnHeader_4
        '
        Me._lvwMain_ColumnHeader_4.Tag = ""
        Me._lvwMain_ColumnHeader_4.Text = "In use"
        Me._lvwMain_ColumnHeader_4.Width = 41
        '
        'uctPMResizer
        '
        Me.uctPMResizer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMResizer.Location = New System.Drawing.Point(4, 276)
        Me.uctPMResizer.Name = "uctPMResizer"
        Me.uctPMResizer.Size = New System.Drawing.Size(32, 30)
        Me.uctPMResizer.TabIndex = 9
        Me.uctPMResizer.Visible = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.cmdHelp)
        Me.Panel1.Controls.Add(Me.cmdCancel)
        Me.Panel1.Controls.Add(Me.cmdOK)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 276)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(477, 29)
        Me.Panel1.TabIndex = 10
        '
        'cmdHelp
        '
        Me.cmdHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(401, 4)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 6
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(321, 4)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 5
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(241, 4)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 4
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.tabMain)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(477, 276)
        Me.Panel2.TabIndex = 11
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me._tabMain_TabPage0)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMain.ItemSize = New System.Drawing.Size(154, 18)
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Multiline = True
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(477, 276)
        Me.tabMain.TabIndex = 1
        '
        '_tabMain_TabPage0
        '
        Me._tabMain_TabPage0.Controls.Add(Me.lvwMain)
        Me._tabMain_TabPage0.Controls.Add(Me.Panel4)
        Me._tabMain_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage0.Name = "_tabMain_TabPage0"
        Me._tabMain_TabPage0.Size = New System.Drawing.Size(469, 250)
        Me._tabMain_TabPage0.TabIndex = 0
        Me._tabMain_TabPage0.Text = "&1 General"
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.cmdAdd)
        Me.Panel4.Controls.Add(Me.cmdEdit)
        Me.Panel4.Controls.Add(Me.cmdDelete)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel4.Location = New System.Drawing.Point(0, 217)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(469, 33)
        Me.Panel4.TabIndex = 5
        '
        'cmdAdd
        '
        Me.cmdAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(236, 8)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 8
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Enabled = False
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(316, 8)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 9
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Enabled = False
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(396, 8)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 10
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'Panel3
        '
        Me.Panel3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel3.Controls.Add(Me.LblRelGroup)
        Me.Panel3.Controls.Add(Me.cboPartyRelationshipGroup)
        Me.Panel3.Controls.Add(Me.picRelationGroup)
        Me.Panel3.Location = New System.Drawing.Point(75, 2)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(397, 17)
        Me.Panel3.TabIndex = 12
        '
        'LblRelGroup
        '
        Me.LblRelGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LblRelGroup.AutoSize = True
        Me.LblRelGroup.BackColor = System.Drawing.SystemColors.Control
        Me.LblRelGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblRelGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRelGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblRelGroup.Location = New System.Drawing.Point(114, 0)
        Me.LblRelGroup.Name = "LblRelGroup"
        Me.LblRelGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LblRelGroup.Size = New System.Drawing.Size(97, 13)
        Me.LblRelGroup.TabIndex = 9
        Me.LblRelGroup.Text = "Relationship Group"
        '
        'cboPartyRelationshipGroup
        '
        Me.cboPartyRelationshipGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboPartyRelationshipGroup.BackColor = System.Drawing.SystemColors.Window
        Me.cboPartyRelationshipGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPartyRelationshipGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPartyRelationshipGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPartyRelationshipGroup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPartyRelationshipGroup.Location = New System.Drawing.Point(222, -2)
        Me.cboPartyRelationshipGroup.Name = "cboPartyRelationshipGroup"
        Me.cboPartyRelationshipGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPartyRelationshipGroup.Size = New System.Drawing.Size(177, 21)
        Me.cboPartyRelationshipGroup.TabIndex = 10
        '
        'picRelationGroup
        '
        Me.picRelationGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picRelationGroup.BackColor = System.Drawing.SystemColors.Control
        Me.picRelationGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.picRelationGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.picRelationGroup.Location = New System.Drawing.Point(89, 0)
        Me.picRelationGroup.Name = "picRelationGroup"
        Me.picRelationGroup.Size = New System.Drawing.Size(300, 21)
        Me.picRelationGroup.TabIndex = 9
        Me.picRelationGroup.TabStop = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(477, 305)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.uctPMResizer)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 23)
        Me.MinimumSize = New System.Drawing.Size(485, 332)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Party Relationship Maintenance"
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me._tabMain_TabPage0.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.picRelationGroup, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Sub lvwMain_InitializeColumnKeys()
        Me._lvwMain_ColumnHeader_1.Name = ""
        Me._lvwMain_ColumnHeader_2.Name = ""
        Me._lvwMain_ColumnHeader_3.Name = ""
        Me._lvwMain_ColumnHeader_4.Name = ""
    End Sub
    Public WithEvents uctPMResizer As PMResizerControl.uctPMResizer
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Public WithEvents tabMain As System.Windows.Forms.TabControl
    Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents lvwMain As System.Windows.Forms.ListView
    Private WithEvents _lvwMain_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwMain_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwMain_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwMain_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Public WithEvents cboPartyRelationshipGroup As System.Windows.Forms.ComboBox
    Public WithEvents LblRelGroup As System.Windows.Forms.Label
    Public WithEvents picRelationGroup As System.Windows.Forms.PictureBox
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Public WithEvents cmdAdd As System.Windows.Forms.Button
    Public WithEvents cmdEdit As System.Windows.Forms.Button
    Public WithEvents cmdDelete As System.Windows.Forms.Button
#End Region
End Class