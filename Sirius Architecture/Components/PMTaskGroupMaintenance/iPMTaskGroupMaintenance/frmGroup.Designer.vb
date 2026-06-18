<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGroup
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializeimgDisplayIcon()
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
    Public imgDisplayIcon(10) As System.Windows.Forms.PictureBox
    'TODOLIST-Commented the listviewhelper as it was conflicting with icon display in listview
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
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
        Me.cmdTasks = New System.Windows.Forms.Button
        Me.imgGroup = New System.Windows.Forms.ImageList(Me.components)
        Me.uctPMResizer1 = New PMResizerControl.uctPMResizer
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.tabGroup = New System.Windows.Forms.TabControl
        Me._tabGroup_TabPage0 = New System.Windows.Forms.TabPage
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.Panel6 = New System.Windows.Forms.Panel
        Me.Panel7 = New System.Windows.Forms.Panel
        Me.lvwGroups = New System.Windows.Forms.ListView
        Me._lvwGroups_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwGroups_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me.Panel5 = New System.Windows.Forms.Panel
        Me._imgDisplayIcon_0 = New System.Windows.Forms.PictureBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.lblEffectiveDate1 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.pnlEffectiveDate = New System.Windows.Forms.Panel
        Me.lblEffectiveDate = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtGroupName = New System.Windows.Forms.TextBox
        Me.txtGroupDescription = New System.Windows.Forms.TextBox
        Me.cboDisplayIcon = New System.Windows.Forms.ComboBox
        Me._imgDisplayIcon_1 = New System.Windows.Forms.PictureBox
        Me._imgDisplayIcon_2 = New System.Windows.Forms.PictureBox
        Me._imgDisplayIcon_3 = New System.Windows.Forms.PictureBox
        Me._imgDisplayIcon_4 = New System.Windows.Forms.PictureBox
        Me._imgDisplayIcon_5 = New System.Windows.Forms.PictureBox
        Me._imgDisplayIcon_6 = New System.Windows.Forms.PictureBox
        Me._imgDisplayIcon_7 = New System.Windows.Forms.PictureBox
        Me._imgDisplayIcon_8 = New System.Windows.Forms.PictureBox
        Me._imgDisplayIcon_9 = New System.Windows.Forms.PictureBox
        Me._imgDisplayIcon_10 = New System.Windows.Forms.PictureBox
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Panel1.SuspendLayout()
        Me.tabGroup.SuspendLayout()
        Me._tabGroup_TabPage0.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.Panel7.SuspendLayout()
        Me.Panel5.SuspendLayout()
        CType(Me._imgDisplayIcon_0, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlEffectiveDate.SuspendLayout()
        CType(Me._imgDisplayIcon_1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._imgDisplayIcon_2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._imgDisplayIcon_3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._imgDisplayIcon_4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._imgDisplayIcon_5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._imgDisplayIcon_6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._imgDisplayIcon_7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._imgDisplayIcon_8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._imgDisplayIcon_9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._imgDisplayIcon_10, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.cmdHelp.Location = New System.Drawing.Point(542, 12)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 12
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
        Me.cmdCancel.Location = New System.Drawing.Point(463, 12)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 11
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
        Me.cmdOK.Location = New System.Drawing.Point(384, 12)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 10
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdOK, "Accept Changes and return to previous screen")
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdTasks
        '
        Me.cmdTasks.BackColor = System.Drawing.SystemColors.Control
        Me.cmdTasks.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdTasks.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTasks.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdTasks.Location = New System.Drawing.Point(4, 10)
        Me.cmdTasks.Name = "cmdTasks"
        Me.cmdTasks.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdTasks.Size = New System.Drawing.Size(153, 22)
        Me.cmdTasks.TabIndex = 7
        Me.cmdTasks.Text = "Add/Remove &Tasks"
        Me.cmdTasks.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdTasks, "Maintain Group Tasks")
        Me.cmdTasks.UseVisualStyleBackColor = False
        '
        'imgGroup
        '
        Me.imgGroup.ImageStream = CType(resources.GetObject("imgGroup.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgGroup.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.imgGroup.Images.SetKeyName(0, "task")
        '
        'uctPMResizer1
        '
        Me.uctPMResizer1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMResizer1.Location = New System.Drawing.Point(8, 384)
        Me.uctPMResizer1.Name = "uctPMResizer1"
        Me.uctPMResizer1.Size = New System.Drawing.Size(32, 30)
        Me.uctPMResizer1.TabIndex = 10
        Me.uctPMResizer1.Visible = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.tabGroup)
        Me.Panel1.Controls.Add(Me.Panel4)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(625, 417)
        Me.Panel1.TabIndex = 11
        '
        'tabGroup
        '
        Me.tabGroup.Controls.Add(Me._tabGroup_TabPage0)
        Me.tabGroup.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabGroup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabGroup.ItemSize = New System.Drawing.Size(199, 18)
        Me.tabGroup.Location = New System.Drawing.Point(0, 12)
        Me.tabGroup.Multiline = True
        Me.tabGroup.Name = "tabGroup"
        Me.tabGroup.SelectedIndex = 0
        Me.tabGroup.Size = New System.Drawing.Size(625, 405)
        Me.tabGroup.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.tabGroup.TabIndex = 1
        Me.tabGroup.TabStop = False
        '
        '_tabGroup_TabPage0
        '
        Me._tabGroup_TabPage0.Controls.Add(Me.Panel3)
        Me._tabGroup_TabPage0.Controls.Add(Me._imgDisplayIcon_1)
        Me._tabGroup_TabPage0.Controls.Add(Me._imgDisplayIcon_2)
        Me._tabGroup_TabPage0.Controls.Add(Me._imgDisplayIcon_3)
        Me._tabGroup_TabPage0.Controls.Add(Me._imgDisplayIcon_4)
        Me._tabGroup_TabPage0.Controls.Add(Me._imgDisplayIcon_5)
        Me._tabGroup_TabPage0.Controls.Add(Me._imgDisplayIcon_6)
        Me._tabGroup_TabPage0.Controls.Add(Me._imgDisplayIcon_7)
        Me._tabGroup_TabPage0.Controls.Add(Me._imgDisplayIcon_8)
        Me._tabGroup_TabPage0.Controls.Add(Me._imgDisplayIcon_9)
        Me._tabGroup_TabPage0.Controls.Add(Me._imgDisplayIcon_10)
        Me._tabGroup_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabGroup_TabPage0.Name = "_tabGroup_TabPage0"
        Me._tabGroup_TabPage0.Size = New System.Drawing.Size(617, 379)
        Me._tabGroup_TabPage0.TabIndex = 1
        Me._tabGroup_TabPage0.Text = "1 - Task Group Details"
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.Panel6)
        Me.Panel3.Controls.Add(Me.Panel5)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(617, 379)
        Me.Panel3.TabIndex = 26
        '
        'Panel6
        '
        Me.Panel6.Controls.Add(Me.lvwGroups)
        Me.Panel6.Controls.Add(Me.Panel7)
        Me.Panel6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel6.Location = New System.Drawing.Point(0, 132)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(617, 247)
        Me.Panel6.TabIndex = 34
        '
        'Panel7
        '
        Me.Panel7.Controls.Add(Me.cmdTasks)
        Me.Panel7.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel7.Location = New System.Drawing.Point(0, 208)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(617, 39)
        Me.Panel7.TabIndex = 9
        '
        'lvwGroups
        '
        Me.lvwGroups.BackColor = System.Drawing.SystemColors.Window
        Me.lvwGroups.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwGroups_ColumnHeader_1, Me._lvwGroups_ColumnHeader_2})
        Me.lvwGroups.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwGroups.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwGroups.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwGroups.HideSelection = False
        Me.lvwGroups.LargeImageList = Me.imgGroup
        Me.lvwGroups.Location = New System.Drawing.Point(0, 0)
        Me.lvwGroups.Name = "lvwGroups"
        Me.lvwGroups.Size = New System.Drawing.Size(617, 208)
        Me.lvwGroups.SmallImageList = Me.imgGroup
        Me.lvwGroups.TabIndex = 8
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
        'Panel5
        '
        Me.Panel5.Controls.Add(Me._imgDisplayIcon_0)
        Me.Panel5.Controls.Add(Me.Label6)
        Me.Panel5.Controls.Add(Me.lblEffectiveDate1)
        Me.Panel5.Controls.Add(Me.Label4)
        Me.Panel5.Controls.Add(Me.Label3)
        Me.Panel5.Controls.Add(Me.Label2)
        Me.Panel5.Controls.Add(Me.pnlEffectiveDate)
        Me.Panel5.Controls.Add(Me.txtGroupName)
        Me.Panel5.Controls.Add(Me.txtGroupDescription)
        Me.Panel5.Controls.Add(Me.cboDisplayIcon)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel5.Location = New System.Drawing.Point(0, 0)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(617, 132)
        Me.Panel5.TabIndex = 33
        '
        '_imgDisplayIcon_0
        '
        Me._imgDisplayIcon_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._imgDisplayIcon_0.Image = CType(resources.GetObject("_imgDisplayIcon_0.Image"), System.Drawing.Image)
        Me._imgDisplayIcon_0.Location = New System.Drawing.Point(538, 12)
        Me._imgDisplayIcon_0.Name = "_imgDisplayIcon_0"
        Me._imgDisplayIcon_0.Size = New System.Drawing.Size(32, 32)
        Me._imgDisplayIcon_0.TabIndex = 15
        Me._imgDisplayIcon_0.TabStop = False
        Me._imgDisplayIcon_0.Visible = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.SystemColors.Control
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(8, 12)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(84, 13)
        Me.Label6.TabIndex = 37
        Me.Label6.Text = "Group Name:"
        '
        'lblEffectiveDate1
        '
        Me.lblEffectiveDate1.AutoSize = True
        Me.lblEffectiveDate1.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate1.Location = New System.Drawing.Point(167, 12)
        Me.lblEffectiveDate1.Name = "lblEffectiveDate1"
        Me.lblEffectiveDate1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate1.Size = New System.Drawing.Size(76, 13)
        Me.lblEffectiveDate1.TabIndex = 38
        Me.lblEffectiveDate1.Text = "Effective date:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(8, 71)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(95, 13)
        Me.Label4.TabIndex = 39
        Me.Label4.Text = "Group Description:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(27, 116)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(99, 13)
        Me.Label3.TabIndex = 40
        Me.Label3.Text = "Group Membership:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(321, 71)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(41, 13)
        Me.Label2.TabIndex = 41
        Me.Label2.Text = "Display"
        '
        'pnlEffectiveDate
        '
        Me.pnlEffectiveDate.AutoSize = True
        Me.pnlEffectiveDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(184, Byte), Integer))
        Me.pnlEffectiveDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlEffectiveDate.Controls.Add(Me.lblEffectiveDate)
        Me.pnlEffectiveDate.Controls.Add(Me.Label1)
        Me.pnlEffectiveDate.Font = New System.Drawing.Font("Verdana", 8.26!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlEffectiveDate.Location = New System.Drawing.Point(170, 31)
        Me.pnlEffectiveDate.Name = "pnlEffectiveDate"
        Me.pnlEffectiveDate.Size = New System.Drawing.Size(165, 21)
        Me.pnlEffectiveDate.TabIndex = 34
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.Location = New System.Drawing.Point(3, 1)
        Me.lblEffectiveDate.MinimumSize = New System.Drawing.Size(150, 5)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.Size = New System.Drawing.Size(155, 16)
        Me.lblEffectiveDate.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 1)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(0, 14)
        Me.Label1.TabIndex = 0
        '
        'txtGroupName
        '
        Me.txtGroupName.AcceptsReturn = True
        Me.txtGroupName.BackColor = System.Drawing.SystemColors.Window
        Me.txtGroupName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtGroupName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGroupName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtGroupName.Location = New System.Drawing.Point(11, 31)
        Me.txtGroupName.MaxLength = 10
        Me.txtGroupName.Name = "txtGroupName"
        Me.txtGroupName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtGroupName.Size = New System.Drawing.Size(136, 20)
        Me.txtGroupName.TabIndex = 33
        '
        'txtGroupDescription
        '
        Me.txtGroupDescription.AcceptsReturn = True
        Me.txtGroupDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtGroupDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtGroupDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGroupDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtGroupDescription.Location = New System.Drawing.Point(11, 88)
        Me.txtGroupDescription.MaxLength = 255
        Me.txtGroupDescription.Name = "txtGroupDescription"
        Me.txtGroupDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtGroupDescription.Size = New System.Drawing.Size(296, 20)
        Me.txtGroupDescription.TabIndex = 35
        '
        'cboDisplayIcon
        '
        Me.cboDisplayIcon.BackColor = System.Drawing.SystemColors.Window
        Me.cboDisplayIcon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboDisplayIcon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboDisplayIcon.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboDisplayIcon.Location = New System.Drawing.Point(324, 88)
        Me.cboDisplayIcon.Name = "cboDisplayIcon"
        Me.cboDisplayIcon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboDisplayIcon.Size = New System.Drawing.Size(183, 21)
        Me.cboDisplayIcon.TabIndex = 36
        '
        '_imgDisplayIcon_1
        '
        Me._imgDisplayIcon_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._imgDisplayIcon_1.Image = CType(resources.GetObject("_imgDisplayIcon_1.Image"), System.Drawing.Image)
        Me._imgDisplayIcon_1.Location = New System.Drawing.Point(536, 12)
        Me._imgDisplayIcon_1.Name = "_imgDisplayIcon_1"
        Me._imgDisplayIcon_1.Size = New System.Drawing.Size(32, 32)
        Me._imgDisplayIcon_1.TabIndex = 16
        Me._imgDisplayIcon_1.TabStop = False
        Me._imgDisplayIcon_1.Visible = False
        '
        '_imgDisplayIcon_2
        '
        Me._imgDisplayIcon_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._imgDisplayIcon_2.Image = CType(resources.GetObject("_imgDisplayIcon_2.Image"), System.Drawing.Image)
        Me._imgDisplayIcon_2.Location = New System.Drawing.Point(536, 12)
        Me._imgDisplayIcon_2.Name = "_imgDisplayIcon_2"
        Me._imgDisplayIcon_2.Size = New System.Drawing.Size(32, 32)
        Me._imgDisplayIcon_2.TabIndex = 17
        Me._imgDisplayIcon_2.TabStop = False
        Me._imgDisplayIcon_2.Visible = False
        '
        '_imgDisplayIcon_3
        '
        Me._imgDisplayIcon_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._imgDisplayIcon_3.Image = CType(resources.GetObject("_imgDisplayIcon_3.Image"), System.Drawing.Image)
        Me._imgDisplayIcon_3.Location = New System.Drawing.Point(536, 12)
        Me._imgDisplayIcon_3.Name = "_imgDisplayIcon_3"
        Me._imgDisplayIcon_3.Size = New System.Drawing.Size(32, 32)
        Me._imgDisplayIcon_3.TabIndex = 18
        Me._imgDisplayIcon_3.TabStop = False
        Me._imgDisplayIcon_3.Visible = False
        '
        '_imgDisplayIcon_4
        '
        Me._imgDisplayIcon_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._imgDisplayIcon_4.Image = CType(resources.GetObject("_imgDisplayIcon_4.Image"), System.Drawing.Image)
        Me._imgDisplayIcon_4.Location = New System.Drawing.Point(536, 12)
        Me._imgDisplayIcon_4.Name = "_imgDisplayIcon_4"
        Me._imgDisplayIcon_4.Size = New System.Drawing.Size(32, 32)
        Me._imgDisplayIcon_4.TabIndex = 19
        Me._imgDisplayIcon_4.TabStop = False
        Me._imgDisplayIcon_4.Visible = False
        '
        '_imgDisplayIcon_5
        '
        Me._imgDisplayIcon_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._imgDisplayIcon_5.Image = CType(resources.GetObject("_imgDisplayIcon_5.Image"), System.Drawing.Image)
        Me._imgDisplayIcon_5.Location = New System.Drawing.Point(536, 12)
        Me._imgDisplayIcon_5.Name = "_imgDisplayIcon_5"
        Me._imgDisplayIcon_5.Size = New System.Drawing.Size(32, 32)
        Me._imgDisplayIcon_5.TabIndex = 20
        Me._imgDisplayIcon_5.TabStop = False
        Me._imgDisplayIcon_5.Visible = False
        '
        '_imgDisplayIcon_6
        '
        Me._imgDisplayIcon_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._imgDisplayIcon_6.Image = CType(resources.GetObject("_imgDisplayIcon_6.Image"), System.Drawing.Image)
        Me._imgDisplayIcon_6.Location = New System.Drawing.Point(536, 12)
        Me._imgDisplayIcon_6.Name = "_imgDisplayIcon_6"
        Me._imgDisplayIcon_6.Size = New System.Drawing.Size(32, 32)
        Me._imgDisplayIcon_6.TabIndex = 21
        Me._imgDisplayIcon_6.TabStop = False
        Me._imgDisplayIcon_6.Visible = False
        '
        '_imgDisplayIcon_7
        '
        Me._imgDisplayIcon_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._imgDisplayIcon_7.Image = CType(resources.GetObject("_imgDisplayIcon_7.Image"), System.Drawing.Image)
        Me._imgDisplayIcon_7.Location = New System.Drawing.Point(536, 12)
        Me._imgDisplayIcon_7.Name = "_imgDisplayIcon_7"
        Me._imgDisplayIcon_7.Size = New System.Drawing.Size(32, 32)
        Me._imgDisplayIcon_7.TabIndex = 22
        Me._imgDisplayIcon_7.TabStop = False
        Me._imgDisplayIcon_7.Visible = False
        '
        '_imgDisplayIcon_8
        '
        Me._imgDisplayIcon_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._imgDisplayIcon_8.Image = CType(resources.GetObject("_imgDisplayIcon_8.Image"), System.Drawing.Image)
        Me._imgDisplayIcon_8.Location = New System.Drawing.Point(536, 12)
        Me._imgDisplayIcon_8.Name = "_imgDisplayIcon_8"
        Me._imgDisplayIcon_8.Size = New System.Drawing.Size(32, 32)
        Me._imgDisplayIcon_8.TabIndex = 23
        Me._imgDisplayIcon_8.TabStop = False
        Me._imgDisplayIcon_8.Visible = False
        '
        '_imgDisplayIcon_9
        '
        Me._imgDisplayIcon_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._imgDisplayIcon_9.Image = CType(resources.GetObject("_imgDisplayIcon_9.Image"), System.Drawing.Image)
        Me._imgDisplayIcon_9.Location = New System.Drawing.Point(536, 12)
        Me._imgDisplayIcon_9.Name = "_imgDisplayIcon_9"
        Me._imgDisplayIcon_9.Size = New System.Drawing.Size(32, 32)
        Me._imgDisplayIcon_9.TabIndex = 24
        Me._imgDisplayIcon_9.TabStop = False
        Me._imgDisplayIcon_9.Visible = False
        '
        '_imgDisplayIcon_10
        '
        Me._imgDisplayIcon_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._imgDisplayIcon_10.Image = CType(resources.GetObject("_imgDisplayIcon_10.Image"), System.Drawing.Image)
        Me._imgDisplayIcon_10.Location = New System.Drawing.Point(536, 12)
        Me._imgDisplayIcon_10.Name = "_imgDisplayIcon_10"
        Me._imgDisplayIcon_10.Size = New System.Drawing.Size(32, 32)
        Me._imgDisplayIcon_10.TabIndex = 25
        Me._imgDisplayIcon_10.TabStop = False
        Me._imgDisplayIcon_10.Visible = False
        '
        'Panel4
        '
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel4.Location = New System.Drawing.Point(0, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(625, 12)
        Me.Panel4.TabIndex = 2
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.cmdHelp)
        Me.Panel2.Controls.Add(Me.cmdCancel)
        Me.Panel2.Controls.Add(Me.cmdOK)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 417)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(625, 46)
        Me.Panel2.TabIndex = 12
        '
        'frmGroup
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(625, 463)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.uctPMResizer1)
        Me.Controls.Add(Me.Panel2)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(270, 147)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(629, 490)
        Me.Name = "frmGroup"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add Task Group"
        Me.Panel1.ResumeLayout(False)
        Me.tabGroup.ResumeLayout(False)
        Me._tabGroup_TabPage0.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel6.ResumeLayout(False)
        Me.Panel7.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        CType(Me._imgDisplayIcon_0, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlEffectiveDate.ResumeLayout(False)
        Me.pnlEffectiveDate.PerformLayout()
        CType(Me._imgDisplayIcon_1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._imgDisplayIcon_2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._imgDisplayIcon_3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._imgDisplayIcon_4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._imgDisplayIcon_5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._imgDisplayIcon_6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._imgDisplayIcon_7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._imgDisplayIcon_8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._imgDisplayIcon_9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._imgDisplayIcon_10, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Sub InitializeimgDisplayIcon()
        Me.imgDisplayIcon(10) = _imgDisplayIcon_10
        Me.imgDisplayIcon(9) = _imgDisplayIcon_9
        Me.imgDisplayIcon(8) = _imgDisplayIcon_8
        Me.imgDisplayIcon(7) = _imgDisplayIcon_7
        Me.imgDisplayIcon(6) = _imgDisplayIcon_6
        Me.imgDisplayIcon(5) = _imgDisplayIcon_5
        Me.imgDisplayIcon(4) = _imgDisplayIcon_4
        Me.imgDisplayIcon(3) = _imgDisplayIcon_3
        Me.imgDisplayIcon(2) = _imgDisplayIcon_2
        Me.imgDisplayIcon(1) = _imgDisplayIcon_1
        Me.imgDisplayIcon(0) = _imgDisplayIcon_0
    End Sub
    Sub lvwGroups_InitializeColumnKeys()
        Me._lvwGroups_ColumnHeader_1.Name = ""
        Me._lvwGroups_ColumnHeader_2.Name = ""
    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Public WithEvents tabGroup As System.Windows.Forms.TabControl
    Private WithEvents _tabGroup_TabPage0 As System.Windows.Forms.TabPage
    Private WithEvents _imgDisplayIcon_1 As System.Windows.Forms.PictureBox
    Private WithEvents _imgDisplayIcon_2 As System.Windows.Forms.PictureBox
    Private WithEvents _imgDisplayIcon_3 As System.Windows.Forms.PictureBox
    Private WithEvents _imgDisplayIcon_4 As System.Windows.Forms.PictureBox
    Private WithEvents _imgDisplayIcon_5 As System.Windows.Forms.PictureBox
    Private WithEvents _imgDisplayIcon_6 As System.Windows.Forms.PictureBox
    Private WithEvents _imgDisplayIcon_7 As System.Windows.Forms.PictureBox
    Private WithEvents _imgDisplayIcon_8 As System.Windows.Forms.PictureBox
    Private WithEvents _imgDisplayIcon_9 As System.Windows.Forms.PictureBox
    Private WithEvents _imgDisplayIcon_10 As System.Windows.Forms.PictureBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Public WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents lblEffectiveDate1 As System.Windows.Forms.Label
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents pnlEffectiveDate As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents txtGroupName As System.Windows.Forms.TextBox
    Public WithEvents txtGroupDescription As System.Windows.Forms.TextBox
    Public WithEvents cboDisplayIcon As System.Windows.Forms.ComboBox
    Public WithEvents lvwGroups As System.Windows.Forms.ListView
    Private WithEvents _lvwGroups_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwGroups_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Panel7 As System.Windows.Forms.Panel
    Public WithEvents cmdTasks As System.Windows.Forms.Button
    Friend WithEvents lblEffectiveDate As System.Windows.Forms.Label
    Private WithEvents _imgDisplayIcon_0 As System.Windows.Forms.PictureBox
#End Region
End Class