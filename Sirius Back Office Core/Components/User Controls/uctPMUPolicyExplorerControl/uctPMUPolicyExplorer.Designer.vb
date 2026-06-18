<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctPMUPolicyExplorer
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		UserControl_Initialize()
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Friend WithEvents cboLivePolicies As System.Windows.Forms.ComboBox
    Friend WithEvents TB_Event As System.Windows.Forms.ToolStripButton
    Friend WithEvents TB_RiskDetails As System.Windows.Forms.ToolStripButton
    Friend WithEvents sep2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TB_InformationChecklist As System.Windows.Forms.ToolStripButton
    Friend WithEvents Toolbar1 As System.Windows.Forms.ToolStrip
    Friend WithEvents imgPolicies As System.Windows.Forms.ImageList
    Friend WithEvents cmdView As System.Windows.Forms.Button
    Friend WithEvents cmdPrint As System.Windows.Forms.Button
    Friend WithEvents tvwPolicies As System.Windows.Forms.TreeView
    Friend WithEvents _lvwRisks_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwRisks_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwRisks_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwRisks_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwRisks_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwRisks_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwRisks_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwRisks_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwRisks_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwRisks_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvwRisks As System.Windows.Forms.ListView
    Friend WithEvents fraRisks As System.Windows.Forms.GroupBox
    Friend WithEvents _lvwVersions_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwVersions_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwVersions_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwVersions_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwVersions_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwVersions_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwVersions_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwVersions_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwVersions_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwVersions_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwVersions_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwVersions_ColumnHeader_12 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwVersions_ColumnHeader_13 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvwVersions As System.Windows.Forms.ListView
    Friend WithEvents fraVersions As System.Windows.Forms.GroupBox
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents lblNavigator As System.Windows.Forms.Label
    Friend WithEvents imgHSize As System.Windows.Forms.PictureBox
    Friend WithEvents imgVSize As System.Windows.Forms.PictureBox
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctPMUPolicyExplorer))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cboLivePolicies = New System.Windows.Forms.ComboBox()
        Me.Toolbar1 = New System.Windows.Forms.ToolStrip()
        Me.TB_Event = New System.Windows.Forms.ToolStripButton()
        Me.sep1 = New System.Windows.Forms.ToolStripSeparator()
        Me.TB_RiskDetails = New System.Windows.Forms.ToolStripButton()
        Me.sep2 = New System.Windows.Forms.ToolStripSeparator()
        Me.TB_InformationChecklist = New System.Windows.Forms.ToolStripButton()
        Me.imgPolicies = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdView = New System.Windows.Forms.Button()
        Me.cmdPrint = New System.Windows.Forms.Button()
        Me.tvwPolicies = New System.Windows.Forms.TreeView()
        Me.fraRisks = New System.Windows.Forms.GroupBox()
        Me.lvwRisks = New System.Windows.Forms.ListView()
        Me._lvwRisks_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRisks_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRisks_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRisks_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRisks_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRisks_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRisks_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRisks_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRisks_ColumnHeader_9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRisks_ColumnHeader_10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.fraVersions = New System.Windows.Forms.GroupBox()
        Me.includecancelledquote = New System.Windows.Forms.CheckBox()
        Me.lvwVersions = New System.Windows.Forms.ListView()
        Me._lvwVersions_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwVersions_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwVersions_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwVersions_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwVersions_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwVersions_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwVersions_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwVersions_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwVersions_ColumnHeader_9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwVersions_ColumnHeader_10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwVersions_ColumnHeader_11 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwVersions_ColumnHeader_12 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwVersions_ColumnHeader_13 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog()
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog()
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.lblNavigator = New System.Windows.Forms.Label()
        Me.imgHSize = New System.Windows.Forms.PictureBox()
        Me.imgVSize = New System.Windows.Forms.PictureBox()
        Me.cmdCopyPolicy = New System.Windows.Forms.Button()
        Me.Toolbar1.SuspendLayout()
        Me.fraRisks.SuspendLayout()
        Me.fraVersions.SuspendLayout()
        CType(Me.imgHSize, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.imgVSize, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cboLivePolicies
        '
        Me.cboLivePolicies.BackColor = System.Drawing.SystemColors.Window
        Me.cboLivePolicies.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboLivePolicies.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLivePolicies.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboLivePolicies.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboLivePolicies.Location = New System.Drawing.Point(120, 29)
        Me.cboLivePolicies.Name = "cboLivePolicies"
        Me.cboLivePolicies.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboLivePolicies.Size = New System.Drawing.Size(121, 28)
        Me.cboLivePolicies.TabIndex = 9
        '
        'Toolbar1
        '
        Me.Toolbar1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.Toolbar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TB_Event, Me.sep1, Me.TB_RiskDetails, Me.sep2, Me.TB_InformationChecklist})
        Me.Toolbar1.Location = New System.Drawing.Point(0, 0)
        Me.Toolbar1.Name = "Toolbar1"
        Me.Toolbar1.Size = New System.Drawing.Size(573, 27)
        Me.Toolbar1.TabIndex = 8
        '
        'TB_Event
        '
        Me.TB_Event.AutoSize = False
        Me.TB_Event.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TB_Event.Name = "TB_Event"
        Me.TB_Event.Size = New System.Drawing.Size(24, 22)
        Me.TB_Event.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.TB_Event.ToolTipText = "Event"
        '
        'sep1
        '
        Me.sep1.Name = "sep1"
        Me.sep1.Size = New System.Drawing.Size(6, 27)
        '
        'TB_RiskDetails
        '
        Me.TB_RiskDetails.AutoSize = False
        Me.TB_RiskDetails.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TB_RiskDetails.Name = "TB_RiskDetails"
        Me.TB_RiskDetails.Size = New System.Drawing.Size(24, 22)
        Me.TB_RiskDetails.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.TB_RiskDetails.ToolTipText = "Claim Details"
        '
        'sep2
        '
        Me.sep2.AutoSize = False
        Me.sep2.Name = "sep2"
        Me.sep2.Size = New System.Drawing.Size(6, 22)
        '
        'TB_InformationChecklist
        '
        Me.TB_InformationChecklist.AutoSize = False
        Me.TB_InformationChecklist.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TB_InformationChecklist.Name = "TB_InformationChecklist"
        Me.TB_InformationChecklist.Size = New System.Drawing.Size(24, 22)
        Me.TB_InformationChecklist.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.TB_InformationChecklist.ToolTipText = "Information Checklist"
        '
        'imgPolicies
        '
        Me.imgPolicies.ImageStream = CType(resources.GetObject("imgPolicies.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgPolicies.TransparentColor = System.Drawing.Color.Transparent
        Me.imgPolicies.Images.SetKeyName(0, "Closed")
        Me.imgPolicies.Images.SetKeyName(1, "Open")
        Me.imgPolicies.Images.SetKeyName(2, "Policy")
        Me.imgPolicies.Images.SetKeyName(3, "AnniversaryPol")
        '
        'cmdView
        '
        Me.cmdView.BackColor = System.Drawing.SystemColors.Control
        Me.cmdView.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdView.Enabled = False
        Me.cmdView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdView.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdView.Location = New System.Drawing.Point(465, 320)
        Me.cmdView.Name = "cmdView"
        Me.cmdView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdView.Size = New System.Drawing.Size(103, 24)
        Me.cmdView.TabIndex = 4
        Me.cmdView.Text = "&View Details"
        Me.cmdView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdView.UseVisualStyleBackColor = False
        '
        'cmdPrint
        '
        Me.cmdPrint.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPrint.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPrint.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPrint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPrint.Location = New System.Drawing.Point(351, 320)
        Me.cmdPrint.Name = "cmdPrint"
        Me.cmdPrint.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPrint.Size = New System.Drawing.Size(103, 24)
        Me.cmdPrint.TabIndex = 3
        Me.cmdPrint.Text = "&Print Summary"
        Me.cmdPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPrint.UseVisualStyleBackColor = False
        '
        'tvwPolicies
        '
        Me.tvwPolicies.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.tvwPolicies.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tvwPolicies.ImageIndex = 0
        Me.tvwPolicies.ImageList = Me.imgPolicies
        Me.tvwPolicies.Indent = 0
        Me.tvwPolicies.Location = New System.Drawing.Point(8, 48)
        Me.tvwPolicies.Name = "tvwPolicies"
        Me.tvwPolicies.SelectedImageIndex = 0
        Me.tvwPolicies.Size = New System.Drawing.Size(233, 280)
        Me.tvwPolicies.Sorted = True
        Me.tvwPolicies.TabIndex = 0
        '
        'fraRisks
        '
        Me.fraRisks.BackColor = System.Drawing.SystemColors.Control
        Me.fraRisks.Controls.Add(Me.lvwRisks)
        Me.fraRisks.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraRisks.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraRisks.Location = New System.Drawing.Point(264, 184)
        Me.fraRisks.Name = "fraRisks"
        Me.fraRisks.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraRisks.Size = New System.Drawing.Size(304, 128)
        Me.fraRisks.TabIndex = 6
        Me.fraRisks.TabStop = False
        Me.fraRisks.Text = "Risks"
        '
        'lvwRisks
        '
        Me.lvwRisks.BackColor = System.Drawing.SystemColors.Control
        Me.lvwRisks.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwRisks_ColumnHeader_1, Me._lvwRisks_ColumnHeader_2, Me._lvwRisks_ColumnHeader_3, Me._lvwRisks_ColumnHeader_4, Me._lvwRisks_ColumnHeader_5, Me._lvwRisks_ColumnHeader_6, Me._lvwRisks_ColumnHeader_7, Me._lvwRisks_ColumnHeader_8, Me._lvwRisks_ColumnHeader_9, Me._lvwRisks_ColumnHeader_10})
        Me.lvwRisks.Enabled = False
        Me.lvwRisks.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwRisks.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwRisks.FullRowSelect = True
        Me.lvwRisks.HideSelection = False
        Me.lvwRisks.Location = New System.Drawing.Point(8, 16)
        Me.lvwRisks.Name = "lvwRisks"
        Me.lvwRisks.Size = New System.Drawing.Size(285, 101)
        Me.lvwRisks.TabIndex = 2
        Me.lvwRisks.UseCompatibleStateImageBehavior = False
        Me.lvwRisks.View = System.Windows.Forms.View.Details
        '
        '_lvwRisks_ColumnHeader_1
        '
        Me._lvwRisks_ColumnHeader_1.Text = "Risk Description"
        Me._lvwRisks_ColumnHeader_1.Width = 97
        '
        '_lvwRisks_ColumnHeader_2
        '
        Me._lvwRisks_ColumnHeader_2.Text = "Coverage"
        Me._lvwRisks_ColumnHeader_2.Width = 0
        '
        '_lvwRisks_ColumnHeader_3
        '
        Me._lvwRisks_ColumnHeader_3.Text = "Sum Insured"
        Me._lvwRisks_ColumnHeader_3.Width = 97
        '
        '_lvwRisks_ColumnHeader_4
        '
        Me._lvwRisks_ColumnHeader_4.Text = "Excess"
        Me._lvwRisks_ColumnHeader_4.Width = 0
        '
        '_lvwRisks_ColumnHeader_5
        '
        Me._lvwRisks_ColumnHeader_5.Text = "Extensions"
        Me._lvwRisks_ColumnHeader_5.Width = 0
        '
        '_lvwRisks_ColumnHeader_6
        '
        Me._lvwRisks_ColumnHeader_6.Text = "NCB"
        Me._lvwRisks_ColumnHeader_6.Width = 0
        '
        '_lvwRisks_ColumnHeader_7
        '
        Me._lvwRisks_ColumnHeader_7.Text = "Gross Premium"
        Me._lvwRisks_ColumnHeader_7.Width = 0
        '
        '_lvwRisks_ColumnHeader_8
        '
        Me._lvwRisks_ColumnHeader_8.Text = "Is Deleted"
        Me._lvwRisks_ColumnHeader_8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._lvwRisks_ColumnHeader_8.Width = 0
        '
        '_lvwRisks_ColumnHeader_9
        '
        Me._lvwRisks_ColumnHeader_9.Text = "Changed Status"
        Me._lvwRisks_ColumnHeader_9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._lvwRisks_ColumnHeader_9.Width = 2540
        '
        '_lvwRisks_ColumnHeader_10
        '
        Me._lvwRisks_ColumnHeader_10.Text = "Changed Date"
        Me._lvwRisks_ColumnHeader_10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._lvwRisks_ColumnHeader_10.Width = 2540
        '
        'fraVersions
        '
        Me.fraVersions.BackColor = System.Drawing.SystemColors.Control
        Me.fraVersions.Controls.Add(Me.includecancelledquote)
        Me.fraVersions.Controls.Add(Me.lvwVersions)
        Me.fraVersions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraVersions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraVersions.Location = New System.Drawing.Point(264, 28)
        Me.fraVersions.Name = "fraVersions"
        Me.fraVersions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraVersions.Size = New System.Drawing.Size(304, 138)
        Me.fraVersions.TabIndex = 5
        Me.fraVersions.TabStop = False
        Me.fraVersions.Text = "Policy Versions"
        '
        'includecancelledquote
        '
        Me.includecancelledquote.AutoSize = True
        Me.includecancelledquote.Location = New System.Drawing.Point(7, 14)
        Me.includecancelledquote.Name = "includecancelledquote"
        Me.includecancelledquote.Size = New System.Drawing.Size(217, 24)
        Me.includecancelledquote.TabIndex = 2
        Me.includecancelledquote.Text = "Include Cancelled Quotes"
        Me.includecancelledquote.UseVisualStyleBackColor = True
        '
        'lvwVersions
        '
        Me.lvwVersions.BackColor = System.Drawing.SystemColors.Control
        Me.lvwVersions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwVersions_ColumnHeader_1, Me._lvwVersions_ColumnHeader_2, Me._lvwVersions_ColumnHeader_3, Me._lvwVersions_ColumnHeader_4, Me._lvwVersions_ColumnHeader_5, Me._lvwVersions_ColumnHeader_6, Me._lvwVersions_ColumnHeader_7, Me._lvwVersions_ColumnHeader_8, Me._lvwVersions_ColumnHeader_9, Me._lvwVersions_ColumnHeader_10, Me._lvwVersions_ColumnHeader_11, Me._lvwVersions_ColumnHeader_12, Me._lvwVersions_ColumnHeader_13})
        Me.lvwVersions.Enabled = False
        Me.lvwVersions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwVersions.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwVersions.FullRowSelect = True
        Me.lvwVersions.HideSelection = False
        Me.lvwVersions.Location = New System.Drawing.Point(8, 37)
        Me.lvwVersions.Name = "lvwVersions"
        Me.lvwVersions.Size = New System.Drawing.Size(285, 95)
        Me.lvwVersions.TabIndex = 1
        Me.lvwVersions.UseCompatibleStateImageBehavior = False
        Me.lvwVersions.View = System.Windows.Forms.View.Details
        '
        '_lvwVersions_ColumnHeader_1
        '
        Me._lvwVersions_ColumnHeader_1.Tag = "DateSort7"
        Me._lvwVersions_ColumnHeader_1.Text = "Cover From"
        Me._lvwVersions_ColumnHeader_1.Width = 97
        '
        '_lvwVersions_ColumnHeader_2
        '
        Me._lvwVersions_ColumnHeader_2.Tag = "DateSort8"
        Me._lvwVersions_ColumnHeader_2.Text = "Renewal Date"
        Me._lvwVersions_ColumnHeader_2.Width = 97
        '
        '_lvwVersions_ColumnHeader_3
        '
        Me._lvwVersions_ColumnHeader_3.Text = "Lapse/Cancel Date"
        Me._lvwVersions_ColumnHeader_3.Width = 97
        '
        '_lvwVersions_ColumnHeader_4
        '
        Me._lvwVersions_ColumnHeader_4.Text = "Insured Persons"
        Me._lvwVersions_ColumnHeader_4.Width = 97
        '
        '_lvwVersions_ColumnHeader_5
        '
        Me._lvwVersions_ColumnHeader_5.Text = "Regarding"
        Me._lvwVersions_ColumnHeader_5.Width = 97
        '
        '_lvwVersions_ColumnHeader_6
        '
        Me._lvwVersions_ColumnHeader_6.Text = "Billing Method"
        Me._lvwVersions_ColumnHeader_6.Width = 97
        '
        '_lvwVersions_ColumnHeader_7
        '
        Me._lvwVersions_ColumnHeader_7.Text = "Amount"
        Me._lvwVersions_ColumnHeader_7.Width = 97
        '
        '_lvwVersions_ColumnHeader_8
        '
        Me._lvwVersions_ColumnHeader_8.Text = "Currency"
        Me._lvwVersions_ColumnHeader_8.Width = 97
        '
        '_lvwVersions_ColumnHeader_9
        '
        Me._lvwVersions_ColumnHeader_9.Text = "Intermediary"
        Me._lvwVersions_ColumnHeader_9.Width = 97
        '
        '_lvwVersions_ColumnHeader_10
        '
        Me._lvwVersions_ColumnHeader_10.Text = "Policy Type"
        Me._lvwVersions_ColumnHeader_10.Width = 97
        '
        '_lvwVersions_ColumnHeader_11
        '
        Me._lvwVersions_ColumnHeader_11.Text = "Event Description"
        Me._lvwVersions_ColumnHeader_11.Width = 201
        '
        '_lvwVersions_ColumnHeader_12
        '
        Me._lvwVersions_ColumnHeader_12.Text = "Status"
        Me._lvwVersions_ColumnHeader_12.Width = 97
        '
        '_lvwVersions_ColumnHeader_13
        '
        Me._lvwVersions_ColumnHeader_13.Tag = "DateSort9"
        Me._lvwVersions_ColumnHeader_13.Text = "Transaction Date"
        Me._lvwVersions_ColumnHeader_13.Width = 97
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
        'lblNavigator
        '
        Me.lblNavigator.BackColor = System.Drawing.SystemColors.Control
        Me.lblNavigator.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNavigator.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNavigator.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNavigator.Location = New System.Drawing.Point(8, 32)
        Me.lblNavigator.Name = "lblNavigator"
        Me.lblNavigator.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNavigator.Size = New System.Drawing.Size(118, 14)
        Me.lblNavigator.TabIndex = 7
        Me.lblNavigator.Text = "Policy Navigator"
        '
        'imgHSize
        '
        Me.imgHSize.Cursor = System.Windows.Forms.Cursors.SizeWE
        Me.imgHSize.Location = New System.Drawing.Point(248, 24)
        Me.imgHSize.Name = "imgHSize"
        Me.imgHSize.Size = New System.Drawing.Size(9, 317)
        Me.imgHSize.TabIndex = 10
        Me.imgHSize.TabStop = False
        '
        'imgVSize
        '
        Me.imgVSize.Cursor = System.Windows.Forms.Cursors.SizeNS
        Me.imgVSize.Location = New System.Drawing.Point(272, 171)
        Me.imgVSize.Name = "imgVSize"
        Me.imgVSize.Size = New System.Drawing.Size(297, 10)
        Me.imgVSize.TabIndex = 11
        Me.imgVSize.TabStop = False
        '
        'cmdCopyPolicy
        '
        Me.cmdCopyPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCopyPolicy.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCopyPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCopyPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCopyPolicy.Location = New System.Drawing.Point(213, 320)
        Me.cmdCopyPolicy.Name = "cmdCopyPolicy"
        Me.cmdCopyPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCopyPolicy.Size = New System.Drawing.Size(133, 24)
        Me.cmdCopyPolicy.TabIndex = 12
        Me.cmdCopyPolicy.Text = "Copy and Create Quote"
        Me.cmdCopyPolicy.UseVisualStyleBackColor = False
        Me.cmdCopyPolicy.Visible = False
        '
        'uctPMUPolicyExplorer
        '
        Me.Controls.Add(Me.cboLivePolicies)
        Me.Controls.Add(Me.Toolbar1)
        Me.Controls.Add(Me.cmdView)
        Me.Controls.Add(Me.cmdPrint)
        Me.Controls.Add(Me.tvwPolicies)
        Me.Controls.Add(Me.fraRisks)
        Me.Controls.Add(Me.fraVersions)
        Me.Controls.Add(Me.lblNavigator)
        Me.Controls.Add(Me.imgHSize)
        Me.Controls.Add(Me.imgVSize)
        Me.Controls.Add(Me.cmdCopyPolicy)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctPMUPolicyExplorer"
        Me.Size = New System.Drawing.Size(573, 347)
        Me.Toolbar1.ResumeLayout(False)
        Me.Toolbar1.PerformLayout()
        Me.fraRisks.ResumeLayout(False)
        Me.fraVersions.ResumeLayout(False)
        Me.fraVersions.PerformLayout()
        CType(Me.imgHSize, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.imgVSize, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents sep1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents includecancelledquote As System.Windows.Forms.CheckBox
	 Friend WithEvents cmdCopyPolicy As System.Windows.Forms.Button
#End Region 
#Region "Upgrade Support"
	<System.Runtime.InteropServices.ProgId("lvwVersionsDblClickEventArgs_NET.lvwVersionsDblClickEventArgs")> _
	Public NotInheritable Class lvwVersionsDblClickEventArgs
		Inherits System.EventArgs
		Public m_lInsHolderCnt As Integer
		Public m_lInsuranceFolderCnt As Integer
		Public m_lInsFileCnt As Integer
		Public m_sShortName As String = ""
		Public m_sInsReference As String = ""
		Public m_lPolicyTypeID As Integer
		Public Sub New(ByRef m_lInsHolderCnt As Integer, ByRef m_lInsuranceFolderCnt As Integer, ByRef m_lInsFileCnt As Integer, ByRef m_sShortName As String, ByRef m_sInsReference As String, ByRef m_lPolicyTypeID As Integer)
			MyBase.New()
			Me.m_lInsHolderCnt = m_lInsHolderCnt
			Me.m_lInsuranceFolderCnt = m_lInsuranceFolderCnt
			Me.m_lInsFileCnt = m_lInsFileCnt
			Me.m_sShortName = m_sShortName
			Me.m_sInsReference = m_sInsReference
			Me.m_lPolicyTypeID = m_lPolicyTypeID
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("lvwRisksClickEventArgs_NET.lvwRisksClickEventArgs")> _
	Public NotInheritable Class lvwRisksClickEventArgs
		Inherits System.EventArgs
		Public v_bSelected As Boolean
		Public v_lRiskID As Integer
		Public v_lScreenId As Integer
		Public v_lRiskTypeId As Integer
		Public Sub New(ByRef v_bSelected As Boolean, ByRef v_lRiskID As Integer, ByRef v_lScreenId As Integer, ByRef v_lRiskTypeId As Integer)
			MyBase.New()
			Me.v_bSelected = v_bSelected
			Me.v_lRiskID = v_lRiskID
			Me.v_lScreenId = v_lScreenId
			Me.v_lRiskTypeId = v_lRiskTypeId
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("lvwRisksDblClickEventArgs_NET.lvwRisksDblClickEventArgs")> _
	Public NotInheritable Class lvwRisksDblClickEventArgs
		Inherits System.EventArgs
		Public v_lInsFileCnt As Integer
		Public v_lRiskID As Integer
		Public v_sRiskDescription As String = ""
		Public v_sRiskTypeDescription As String = ""
		Public v_dtRiskInceptionDate As Date
		Public v_dtRiskExpiryDate As Date
		Public v_vRiskTotalSI As Object
		Public v_vRiskTotalPremium As Object
		Public v_lRiskGisScreen As Integer
		Public v_lRiskTypeId As Integer
		Public v_lIsReInsuranceAtRiskLevel As Integer
        Public v_lInsuranceFolderCnt As Integer
        'PM035142 - Added v_sShortName 
        Public v_sShortName As String = ""

        Public Sub New(ByRef v_lInsFileCnt As Integer, ByRef v_lRiskID As Integer, ByRef v_sRiskDescription As String, ByRef v_sRiskTypeDescription As String, ByRef v_dtRiskInceptionDate As Date, ByRef v_dtRiskExpiryDate As Date, ByRef v_vRiskTotalSI As Object, ByRef v_vRiskTotalPremium As Object, ByRef v_lRiskGisScreen As Integer, ByRef v_lRiskTypeId As Integer, ByRef v_lIsReInsuranceAtRiskLevel As Integer, ByRef v_lInsuranceFolderCnt As Integer, ByRef v_sShortName As String)
            'PM035142 - Added v_sShortName 
            MyBase.New()
            Me.v_lInsFileCnt = v_lInsFileCnt
            Me.v_lRiskID = v_lRiskID
            Me.v_sRiskDescription = v_sRiskDescription
            Me.v_sRiskTypeDescription = v_sRiskTypeDescription
            Me.v_dtRiskInceptionDate = v_dtRiskInceptionDate
            Me.v_dtRiskExpiryDate = v_dtRiskExpiryDate
            Me.v_vRiskTotalSI = v_vRiskTotalSI
            Me.v_vRiskTotalPremium = v_vRiskTotalPremium
            Me.v_lRiskGisScreen = v_lRiskGisScreen
            Me.v_lRiskTypeId = v_lRiskTypeId
            Me.v_lIsReInsuranceAtRiskLevel = v_lIsReInsuranceAtRiskLevel
            Me.v_lInsuranceFolderCnt = v_lInsuranceFolderCnt
            Me.v_sShortName = v_sShortName
        End Sub
	End Class
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