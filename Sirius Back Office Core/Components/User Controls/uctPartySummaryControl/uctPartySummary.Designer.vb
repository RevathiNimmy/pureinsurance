<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctPartySummControl
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwContacts_InitializeColumnKeys()
		UserControl_InitProperties()
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents _Event As System.Windows.Forms.ToolStripButton
    Friend WithEvents Toolbar1 As System.Windows.Forms.ToolStrip
    Friend WithEvents txtSubBranch As System.Windows.Forms.TextBox
    Friend WithEvents txtLoyaltyNo As System.Windows.Forms.TextBox
    Friend WithEvents txtDOB As System.Windows.Forms.TextBox
    Friend WithEvents txtTradingName As System.Windows.Forms.TextBox
    Friend WithEvents cboBranchName As System.Windows.Forms.ComboBox
    Friend WithEvents cmdDeleteLink As System.Windows.Forms.Button
    Friend WithEvents picLink As System.Windows.Forms.PictureBox
    Friend WithEvents txtBranch As System.Windows.Forms.TextBox
    Friend WithEvents txtConsultant As System.Windows.Forms.TextBox
    Friend WithEvents txtAgent As System.Windows.Forms.TextBox
    Friend WithEvents txtIDReference As System.Windows.Forms.TextBox
    Friend WithEvents txtAddress As System.Windows.Forms.TextBox
    Friend WithEvents txtResolvedName As System.Windows.Forms.TextBox
    Friend WithEvents _lvwContacts_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwContacts_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwContacts_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvwContacts As System.Windows.Forms.ListView
    Friend WithEvents lblClientContactDetails As System.Windows.Forms.Label
    Friend WithEvents lblSubBranch As System.Windows.Forms.Label
    Friend WithEvents lblLoyaltyNo As System.Windows.Forms.Label
    Friend WithEvents lblDOB As System.Windows.Forms.Label
    Friend WithEvents lblTradingName As System.Windows.Forms.Label
    Friend WithEvents lblExists As System.Windows.Forms.Label
    Friend WithEvents lblSwiftLink As System.Windows.Forms.Label
    Friend WithEvents lblBranch As System.Windows.Forms.Label
    Friend WithEvents lblConsultant As System.Windows.Forms.Label
    Friend WithEvents lblAgent As System.Windows.Forms.Label
    Friend WithEvents lblIDReference As System.Windows.Forms.Label
    Friend WithEvents lblAddress As System.Windows.Forms.Label
    Friend WithEvents lblResolvedName As System.Windows.Forms.Label
    Friend WithEvents fraClientSum As System.Windows.Forms.GroupBox
    Friend WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Friend WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Private WithEvents commandButtonHelper1 As Artinsoft.VB6.Gui.CommandButtonHelper
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctPartySummControl))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdDeleteLink = New System.Windows.Forms.Button
        Me.Toolbar1 = New System.Windows.Forms.ToolStrip
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me._Event = New System.Windows.Forms.ToolStripButton
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.fraClientSum = New System.Windows.Forms.GroupBox
        Me.txtSubBranch = New System.Windows.Forms.TextBox
        Me.txtLoyaltyNo = New System.Windows.Forms.TextBox
        Me.txtDOB = New System.Windows.Forms.TextBox
        Me.txtTradingName = New System.Windows.Forms.TextBox
        Me.cboBranchName = New System.Windows.Forms.ComboBox
        Me.picLink = New System.Windows.Forms.PictureBox
        Me.txtBranch = New System.Windows.Forms.TextBox
        Me.txtConsultant = New System.Windows.Forms.TextBox
        Me.txtAgent = New System.Windows.Forms.TextBox
        Me.txtIDReference = New System.Windows.Forms.TextBox
        Me.txtAddress = New System.Windows.Forms.TextBox
        Me.txtResolvedName = New System.Windows.Forms.TextBox
        Me.lvwContacts = New System.Windows.Forms.ListView
        Me._lvwContacts_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me.lblClientContactDetails = New System.Windows.Forms.Label
        Me.lblSubBranch = New System.Windows.Forms.Label
        Me.lblLoyaltyNo = New System.Windows.Forms.Label
        Me.lblDOB = New System.Windows.Forms.Label
        Me.lblTradingName = New System.Windows.Forms.Label
        Me.lblExists = New System.Windows.Forms.Label
        Me.lblSwiftLink = New System.Windows.Forms.Label
        Me.lblBranch = New System.Windows.Forms.Label
        Me.lblConsultant = New System.Windows.Forms.Label
        Me.lblAgent = New System.Windows.Forms.Label
        Me.lblIDReference = New System.Windows.Forms.Label
        Me.lblAddress = New System.Windows.Forms.Label
        Me.lblResolvedName = New System.Windows.Forms.Label
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.commandButtonHelper1 = New Artinsoft.VB6.Gui.CommandButtonHelper(Me.components)
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.Toolbar1.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.fraClientSum.SuspendLayout()
        CType(Me.picLink, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.commandButtonHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdDeleteLink
        '
        Me.cmdDeleteLink.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdDeleteLink, True)
        Me.cmdDeleteLink.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdDeleteLink, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdDeleteLink, Nothing)
        Me.cmdDeleteLink.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteLink.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteLink.Image = CType(resources.GetObject("cmdDeleteLink.Image"), System.Drawing.Image)
        Me.cmdDeleteLink.Location = New System.Drawing.Point(524, 262)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdDeleteLink, System.Drawing.Color.Silver)
        Me.cmdDeleteLink.Name = "cmdDeleteLink"
        Me.cmdDeleteLink.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteLink.Size = New System.Drawing.Size(29, 25)
        Me.commandButtonHelper1.SetStyle(Me.cmdDeleteLink, 1)
        Me.cmdDeleteLink.TabIndex = 17
        Me.cmdDeleteLink.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdDeleteLink.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdDeleteLink, "Delete Link")
        Me.cmdDeleteLink.UseVisualStyleBackColor = False
        '
        'Toolbar1
        '
        Me.Toolbar1.Dock = System.Windows.Forms.DockStyle.None
        Me.Toolbar1.ImageList = Me.ImageList1
        Me.Toolbar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._Event})
        Me.Toolbar1.Location = New System.Drawing.Point(0, 0)
        Me.Toolbar1.Name = "Toolbar1"
        Me.Toolbar1.Size = New System.Drawing.Size(65, 25)
        Me.Toolbar1.TabIndex = 28
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "")
        Me.ImageList1.Images.SetKeyName(1, "")
        Me.ImageList1.Images.SetKeyName(2, "")
        Me.ImageList1.Images.SetKeyName(3, "")
        Me.ImageList1.Images.SetKeyName(4, "")
        Me.ImageList1.Images.SetKeyName(5, "")
        Me.ImageList1.Images.SetKeyName(6, "")
        Me.ImageList1.Images.SetKeyName(7, "")
        Me.ImageList1.Images.SetKeyName(8, "")
        Me.ImageList1.Images.SetKeyName(9, "")
        Me.ImageList1.Images.SetKeyName(10, "")
        Me.ImageList1.Images.SetKeyName(11, "")
        Me.ImageList1.Images.SetKeyName(12, "")
        Me.ImageList1.Images.SetKeyName(13, "")
        Me.ImageList1.Images.SetKeyName(14, "")
        Me.ImageList1.Images.SetKeyName(15, "")
        '
        '_Event
        '
        Me._Event.AutoSize = False
        Me._Event.ImageIndex = 10
        Me._Event.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Event.Name = "_Event"
        Me._Event.Size = New System.Drawing.Size(24, 22)
        Me._Event.Tag = ""
        Me._Event.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Event.ToolTipText = "Event"
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(600, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(0, 24)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(605, 349)
        Me.tabMainTab.TabIndex = 6
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraClientSum)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(597, 323)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "&1 - Summary"
        '
        'fraClientSum
        '
        Me.fraClientSum.BackColor = System.Drawing.SystemColors.Control
        Me.fraClientSum.Controls.Add(Me.txtSubBranch)
        Me.fraClientSum.Controls.Add(Me.txtLoyaltyNo)
        Me.fraClientSum.Controls.Add(Me.txtDOB)
        Me.fraClientSum.Controls.Add(Me.txtTradingName)
        Me.fraClientSum.Controls.Add(Me.cboBranchName)
        Me.fraClientSum.Controls.Add(Me.cmdDeleteLink)
        Me.fraClientSum.Controls.Add(Me.picLink)
        Me.fraClientSum.Controls.Add(Me.txtBranch)
        Me.fraClientSum.Controls.Add(Me.txtConsultant)
        Me.fraClientSum.Controls.Add(Me.txtAgent)
        Me.fraClientSum.Controls.Add(Me.txtIDReference)
        Me.fraClientSum.Controls.Add(Me.txtAddress)
        Me.fraClientSum.Controls.Add(Me.txtResolvedName)
        Me.fraClientSum.Controls.Add(Me.lvwContacts)
        Me.fraClientSum.Controls.Add(Me.lblClientContactDetails)
        Me.fraClientSum.Controls.Add(Me.lblSubBranch)
        Me.fraClientSum.Controls.Add(Me.lblLoyaltyNo)
        Me.fraClientSum.Controls.Add(Me.lblDOB)
        Me.fraClientSum.Controls.Add(Me.lblTradingName)
        Me.fraClientSum.Controls.Add(Me.lblExists)
        Me.fraClientSum.Controls.Add(Me.lblSwiftLink)
        Me.fraClientSum.Controls.Add(Me.lblBranch)
        Me.fraClientSum.Controls.Add(Me.lblConsultant)
        Me.fraClientSum.Controls.Add(Me.lblAgent)
        Me.fraClientSum.Controls.Add(Me.lblIDReference)
        Me.fraClientSum.Controls.Add(Me.lblAddress)
        Me.fraClientSum.Controls.Add(Me.lblResolvedName)
        Me.fraClientSum.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraClientSum.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraClientSum.Location = New System.Drawing.Point(8, 4)
        Me.fraClientSum.Name = "fraClientSum"
        Me.fraClientSum.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraClientSum.Size = New System.Drawing.Size(585, 303)
        Me.fraClientSum.TabIndex = 7
        Me.fraClientSum.TabStop = False
        '
        'txtSubBranch
        '
        Me.txtSubBranch.AcceptsReturn = True
        Me.txtSubBranch.BackColor = System.Drawing.SystemColors.Window
        Me.txtSubBranch.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSubBranch.Enabled = False
        Me.txtSubBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSubBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSubBranch.Location = New System.Drawing.Point(96, 272)
        Me.txtSubBranch.MaxLength = 0
        Me.txtSubBranch.Name = "txtSubBranch"
        Me.txtSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSubBranch.Size = New System.Drawing.Size(177, 20)
        Me.txtSubBranch.TabIndex = 25
        '
        'txtLoyaltyNo
        '
        Me.txtLoyaltyNo.AcceptsReturn = True
        Me.txtLoyaltyNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtLoyaltyNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLoyaltyNo.Enabled = False
        Me.txtLoyaltyNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLoyaltyNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLoyaltyNo.Location = New System.Drawing.Point(384, 216)
        Me.txtLoyaltyNo.MaxLength = 0
        Me.txtLoyaltyNo.Name = "txtLoyaltyNo"
        Me.txtLoyaltyNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLoyaltyNo.Size = New System.Drawing.Size(177, 20)
        Me.txtLoyaltyNo.TabIndex = 23
        '
        'txtDOB
        '
        Me.txtDOB.AcceptsReturn = True
        Me.txtDOB.BackColor = System.Drawing.SystemColors.Window
        Me.txtDOB.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDOB.Enabled = False
        Me.txtDOB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDOB.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDOB.Location = New System.Drawing.Point(96, 120)
        Me.txtDOB.MaxLength = 0
        Me.txtDOB.Name = "txtDOB"
        Me.txtDOB.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDOB.Size = New System.Drawing.Size(177, 20)
        Me.txtDOB.TabIndex = 21
        '
        'txtTradingName
        '
        Me.txtTradingName.AcceptsReturn = True
        Me.txtTradingName.BackColor = System.Drawing.SystemColors.Window
        Me.txtTradingName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTradingName.Enabled = False
        Me.txtTradingName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTradingName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTradingName.Location = New System.Drawing.Point(96, 96)
        Me.txtTradingName.MaxLength = 0
        Me.txtTradingName.Name = "txtTradingName"
        Me.txtTradingName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTradingName.Size = New System.Drawing.Size(177, 20)
        Me.txtTradingName.TabIndex = 19
        '
        'cboBranchName
        '
        Me.cboBranchName.BackColor = System.Drawing.SystemColors.Window
        Me.cboBranchName.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBranchName.Enabled = False
        Me.cboBranchName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBranchName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBranchName.Location = New System.Drawing.Point(288, 280)
        Me.cboBranchName.Name = "cboBranchName"
        Me.cboBranchName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBranchName.Size = New System.Drawing.Size(145, 21)
        Me.cboBranchName.TabIndex = 18
        Me.cboBranchName.Visible = False
        '
        'picLink
        '
        Me.picLink.BackColor = System.Drawing.SystemColors.Control
        Me.picLink.Cursor = System.Windows.Forms.Cursors.Default
        Me.picLink.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.picLink.Image = CType(resources.GetObject("picLink.Image"), System.Drawing.Image)
        Me.picLink.Location = New System.Drawing.Point(432, 256)
        Me.picLink.Name = "picLink"
        Me.picLink.Size = New System.Drawing.Size(32, 32)
        Me.picLink.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.picLink.TabIndex = 15
        Me.picLink.TabStop = False
        '
        'txtBranch
        '
        Me.txtBranch.AcceptsReturn = True
        Me.txtBranch.BackColor = System.Drawing.SystemColors.Window
        Me.txtBranch.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBranch.Enabled = False
        Me.txtBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBranch.Location = New System.Drawing.Point(96, 248)
        Me.txtBranch.MaxLength = 0
        Me.txtBranch.Name = "txtBranch"
        Me.txtBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBranch.Size = New System.Drawing.Size(177, 20)
        Me.txtBranch.TabIndex = 5
        '
        'txtConsultant
        '
        Me.txtConsultant.AcceptsReturn = True
        Me.txtConsultant.BackColor = System.Drawing.SystemColors.Window
        Me.txtConsultant.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtConsultant.Enabled = False
        Me.txtConsultant.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtConsultant.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtConsultant.Location = New System.Drawing.Point(384, 188)
        Me.txtConsultant.MaxLength = 0
        Me.txtConsultant.Name = "txtConsultant"
        Me.txtConsultant.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtConsultant.Size = New System.Drawing.Size(177, 20)
        Me.txtConsultant.TabIndex = 4
        '
        'txtAgent
        '
        Me.txtAgent.AcceptsReturn = True
        Me.txtAgent.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgent.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgent.Enabled = False
        Me.txtAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgent.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgent.Location = New System.Drawing.Point(384, 160)
        Me.txtAgent.MaxLength = 0
        Me.txtAgent.Name = "txtAgent"
        Me.txtAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgent.Size = New System.Drawing.Size(177, 20)
        Me.txtAgent.TabIndex = 3
        '
        'txtIDReference
        '
        Me.txtIDReference.AcceptsReturn = True
        Me.txtIDReference.BackColor = System.Drawing.SystemColors.Window
        Me.txtIDReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtIDReference.Enabled = False
        Me.txtIDReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIDReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIDReference.Location = New System.Drawing.Point(96, 32)
        Me.txtIDReference.MaxLength = 0
        Me.txtIDReference.Name = "txtIDReference"
        Me.txtIDReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtIDReference.Size = New System.Drawing.Size(177, 20)
        Me.txtIDReference.TabIndex = 0
        '
        'txtAddress
        '
        Me.txtAddress.AcceptsReturn = True
        Me.txtAddress.BackColor = System.Drawing.SystemColors.Window
        Me.txtAddress.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAddress.Enabled = False
        Me.txtAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAddress.Location = New System.Drawing.Point(96, 152)
        Me.txtAddress.MaxLength = 0
        Me.txtAddress.Multiline = True
        Me.txtAddress.Name = "txtAddress"
        Me.txtAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAddress.Size = New System.Drawing.Size(177, 83)
        Me.txtAddress.TabIndex = 2
        '
        'txtResolvedName
        '
        Me.txtResolvedName.AcceptsReturn = True
        Me.txtResolvedName.BackColor = System.Drawing.SystemColors.Window
        Me.txtResolvedName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtResolvedName.Enabled = False
        Me.txtResolvedName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtResolvedName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtResolvedName.Location = New System.Drawing.Point(96, 56)
        Me.txtResolvedName.MaxLength = 0
        Me.txtResolvedName.Name = "txtResolvedName"
        Me.txtResolvedName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtResolvedName.Size = New System.Drawing.Size(177, 20)
        Me.txtResolvedName.TabIndex = 1
        '
        'lvwContacts
        '
        Me.lvwContacts.BackColor = System.Drawing.SystemColors.Window
        Me.lvwContacts.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwContacts, "")
        Me.lvwContacts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwContacts_ColumnHeader_1, Me._lvwContacts_ColumnHeader_2, Me._lvwContacts_ColumnHeader_3})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwContacts, False)
        Me.lvwContacts.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwContacts.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me.lvwContacts, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwContacts, "")
        Me.lvwContacts.Location = New System.Drawing.Point(304, 50)
        Me.lvwContacts.Name = "lvwContacts"
        Me.lvwContacts.Size = New System.Drawing.Size(257, 89)
        Me.listViewHelper1.SetSmallIcons(Me.lvwContacts, "")
        Me.listViewHelper1.SetSorted(Me.lvwContacts, False)
        Me.listViewHelper1.SetSortKey(Me.lvwContacts, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwContacts, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwContacts.TabIndex = 27
        Me.lvwContacts.UseCompatibleStateImageBehavior = False
        Me.lvwContacts.View = System.Windows.Forms.View.Details
        '
        '_lvwContacts_ColumnHeader_1
        '
        Me._lvwContacts_ColumnHeader_1.Tag = ""
        Me._lvwContacts_ColumnHeader_1.Text = "Type"
        Me._lvwContacts_ColumnHeader_1.Width = 67
        '
        '_lvwContacts_ColumnHeader_2
        '
        Me._lvwContacts_ColumnHeader_2.Tag = ""
        Me._lvwContacts_ColumnHeader_2.Text = "Detail"
        Me._lvwContacts_ColumnHeader_2.Width = 110
        '
        '_lvwContacts_ColumnHeader_3
        '
        Me._lvwContacts_ColumnHeader_3.Tag = ""
        Me._lvwContacts_ColumnHeader_3.Text = "Description"
        Me._lvwContacts_ColumnHeader_3.Width = 97
        '
        'lblClientContactDetails
        '
        Me.lblClientContactDetails.AutoSize = True
        Me.lblClientContactDetails.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientContactDetails.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientContactDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientContactDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientContactDetails.Location = New System.Drawing.Point(304, 32)
        Me.lblClientContactDetails.Name = "lblClientContactDetails"
        Me.lblClientContactDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientContactDetails.Size = New System.Drawing.Size(114, 13)
        Me.lblClientContactDetails.TabIndex = 29
        Me.lblClientContactDetails.Text = "Client Contact Details :"
        '
        'lblSubBranch
        '
        Me.lblSubBranch.AutoSize = True
        Me.lblSubBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblSubBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSubBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubBranch.Location = New System.Drawing.Point(8, 275)
        Me.lblSubBranch.Name = "lblSubBranch"
        Me.lblSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSubBranch.Size = New System.Drawing.Size(65, 13)
        Me.lblSubBranch.TabIndex = 26
        Me.lblSubBranch.Text = "Sub-branch:"
        '
        'lblLoyaltyNo
        '
        Me.lblLoyaltyNo.AutoSize = True
        Me.lblLoyaltyNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblLoyaltyNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLoyaltyNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLoyaltyNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLoyaltyNo.Location = New System.Drawing.Point(296, 219)
        Me.lblLoyaltyNo.Name = "lblLoyaltyNo"
        Me.lblLoyaltyNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLoyaltyNo.Size = New System.Drawing.Size(60, 13)
        Me.lblLoyaltyNo.TabIndex = 24
        Me.lblLoyaltyNo.Text = "Loyalty No."
        '
        'lblDOB
        '
        Me.lblDOB.AutoSize = True
        Me.lblDOB.BackColor = System.Drawing.SystemColors.Control
        Me.lblDOB.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDOB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDOB.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDOB.Location = New System.Drawing.Point(8, 123)
        Me.lblDOB.Name = "lblDOB"
        Me.lblDOB.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDOB.Size = New System.Drawing.Size(69, 13)
        Me.lblDOB.TabIndex = 22
        Me.lblDOB.Text = "Date of Birth:"
        '
        'lblTradingName
        '
        Me.lblTradingName.AutoSize = True
        Me.lblTradingName.BackColor = System.Drawing.SystemColors.Control
        Me.lblTradingName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTradingName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTradingName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTradingName.Location = New System.Drawing.Point(8, 99)
        Me.lblTradingName.Name = "lblTradingName"
        Me.lblTradingName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTradingName.Size = New System.Drawing.Size(77, 13)
        Me.lblTradingName.TabIndex = 20
        Me.lblTradingName.Text = "Trading Name:"
        '
        'lblExists
        '
        Me.lblExists.AutoSize = True
        Me.lblExists.BackColor = System.Drawing.SystemColors.Control
        Me.lblExists.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExists.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExists.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExists.Location = New System.Drawing.Point(480, 264)
        Me.lblExists.Name = "lblExists"
        Me.lblExists.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExists.Size = New System.Drawing.Size(36, 13)
        Me.lblExists.TabIndex = 16
        Me.lblExists.Text = "exists."
        '
        'lblSwiftLink
        '
        Me.lblSwiftLink.AutoSize = True
        Me.lblSwiftLink.BackColor = System.Drawing.SystemColors.Control
        Me.lblSwiftLink.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSwiftLink.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSwiftLink.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSwiftLink.Location = New System.Drawing.Point(344, 264)
        Me.lblSwiftLink.Name = "lblSwiftLink"
        Me.lblSwiftLink.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSwiftLink.Size = New System.Drawing.Size(68, 13)
        Me.lblSwiftLink.TabIndex = 14
        Me.lblSwiftLink.Text = "Link to Swift:"
        '
        'lblBranch
        '
        Me.lblBranch.AutoSize = True
        Me.lblBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranch.Location = New System.Drawing.Point(8, 251)
        Me.lblBranch.Name = "lblBranch"
        Me.lblBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranch.Size = New System.Drawing.Size(44, 13)
        Me.lblBranch.TabIndex = 13
        Me.lblBranch.Text = "Branch:"
        '
        'lblConsultant
        '
        Me.lblConsultant.AutoSize = True
        Me.lblConsultant.BackColor = System.Drawing.SystemColors.Control
        Me.lblConsultant.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblConsultant.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConsultant.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblConsultant.Location = New System.Drawing.Point(296, 191)
        Me.lblConsultant.Name = "lblConsultant"
        Me.lblConsultant.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblConsultant.Size = New System.Drawing.Size(76, 13)
        Me.lblConsultant.TabIndex = 12
        Me.lblConsultant.Text = "Account exec:"
        '
        'lblAgent
        '
        Me.lblAgent.AutoSize = True
        Me.lblAgent.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgent.Location = New System.Drawing.Point(296, 163)
        Me.lblAgent.Name = "lblAgent"
        Me.lblAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgent.Size = New System.Drawing.Size(38, 13)
        Me.lblAgent.TabIndex = 11
        Me.lblAgent.Text = "Agent:"
        '
        'lblIDReference
        '
        Me.lblIDReference.AutoSize = True
        Me.lblIDReference.BackColor = System.Drawing.SystemColors.Control
        Me.lblIDReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIDReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIDReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIDReference.Location = New System.Drawing.Point(8, 35)
        Me.lblIDReference.Name = "lblIDReference"
        Me.lblIDReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIDReference.Size = New System.Drawing.Size(63, 13)
        Me.lblIDReference.TabIndex = 10
        Me.lblIDReference.Text = "Client code:"
        '
        'lblAddress
        '
        Me.lblAddress.AutoSize = True
        Me.lblAddress.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddress.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddress.Location = New System.Drawing.Point(8, 155)
        Me.lblAddress.Name = "lblAddress"
        Me.lblAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddress.Size = New System.Drawing.Size(48, 13)
        Me.lblAddress.TabIndex = 9
        Me.lblAddress.Text = "Address:"
        '
        'lblResolvedName
        '
        Me.lblResolvedName.AutoSize = True
        Me.lblResolvedName.BackColor = System.Drawing.SystemColors.Control
        Me.lblResolvedName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblResolvedName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblResolvedName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblResolvedName.Location = New System.Drawing.Point(8, 59)
        Me.lblResolvedName.Name = "lblResolvedName"
        Me.lblResolvedName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblResolvedName.Size = New System.Drawing.Size(38, 13)
        Me.lblResolvedName.TabIndex = 8
        Me.lblResolvedName.Text = "Name:"
        '
        'uctPartySummControl
        '
        Me.Controls.Add(Me.Toolbar1)
        Me.Controls.Add(Me.tabMainTab)
        Me.Name = "uctPartySummControl"
        Me.Size = New System.Drawing.Size(613, 376)
        Me.Toolbar1.ResumeLayout(False)
        Me.Toolbar1.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.fraClientSum.ResumeLayout(False)
        Me.fraClientSum.PerformLayout()
        CType(Me.picLink, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.commandButtonHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub lvwContacts_InitializeColumnKeys()
		Me._lvwContacts_ColumnHeader_1.Name = ""
		Me._lvwContacts_ColumnHeader_2.Name = ""
		Me._lvwContacts_ColumnHeader_3.Name = ""
	End Sub
#End Region 
#Region "Upgrade Support"
	<System.Runtime.InteropServices.ProgId("MouseUpEventArgs_NET.MouseUpEventArgs")> _
	Public NotInheritable Class MouseUpEventArgs
		Inherits System.EventArgs
		Public Button As Integer
		Public Shift As Integer
		Public x As Single
		Public y As Single
		Public Sub New(ByRef Button As Integer, ByRef Shift As Integer, ByRef x As Single, ByRef y As Single)
			MyBase.New()
			Me.Button = Button
			Me.Shift = Shift
			Me.x = x
			Me.y = y
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("MouseMoveEventArgs_NET.MouseMoveEventArgs")> _
	Public NotInheritable Class MouseMoveEventArgs
		Inherits System.EventArgs
		Public Button As Integer
		Public Shift As Integer
		Public x As Single
		Public y As Single
		Public Sub New(ByRef Button As Integer, ByRef Shift As Integer, ByRef x As Single, ByRef y As Single)
			MyBase.New()
			Me.Button = Button
			Me.Shift = Shift
			Me.x = x
			Me.y = y
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("MouseDownEventArgs_NET.MouseDownEventArgs")> _
	Public NotInheritable Class MouseDownEventArgs
		Inherits System.EventArgs
		Public Button As Integer
		Public Shift As Integer
		Public x As Single
		Public y As Single
		Public Sub New(ByRef Button As Integer, ByRef Shift As Integer, ByRef x As Single, ByRef y As Single)
			MyBase.New()
			Me.Button = Button
			Me.Shift = Shift
			Me.x = x
			Me.y = y
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("KeyUpEventArgs_NET.KeyUpEventArgs")> _
	Public NotInheritable Class KeyUpEventArgs
		Inherits System.EventArgs
		Public KeyCode As Integer
		Public Shift As Integer
		Public Sub New(ByRef KeyCode As Integer, ByRef Shift As Integer)
			MyBase.New()
			Me.KeyCode = KeyCode
			Me.Shift = Shift
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("KeyPressEventArgs_NET.KeyPressEventArgs")> _
	Public NotInheritable Class KeyPressEventArgs
		Inherits System.EventArgs
		Public KeyAscii As Integer
		Public Sub New(ByRef KeyAscii As Integer)
			MyBase.New()
			Me.KeyAscii = KeyAscii
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("KeyDownEventArgs_NET.KeyDownEventArgs")> _
	Public NotInheritable Class KeyDownEventArgs
		Inherits System.EventArgs
		Public KeyCode As Integer
		Public Shift As Integer
		Public Sub New(ByRef KeyCode As Integer, ByRef Shift As Integer)
			MyBase.New()
			Me.KeyCode = KeyCode
			Me.Shift = Shift
		End Sub
	End Class
#End Region 
End Class