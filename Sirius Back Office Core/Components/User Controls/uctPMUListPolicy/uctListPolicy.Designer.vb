<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctListPolicy
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwSearchDetails_InitializeColumnKeys()
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents TB_Event As System.Windows.Forms.ToolStripButton
	Friend WithEvents _Toolbar1_Button2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TB_RiskDetails As System.Windows.Forms.ToolStripButton
	Friend WithEvents _Toolbar1_Button4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TB_InformationChecklist As System.Windows.Forms.ToolStripButton
    Friend WithEvents TB_Policy As System.Windows.Forms.ToolStripButton
	Friend WithEvents Toolbar1 As System.Windows.Forms.ToolStrip
	Friend WithEvents lblStatus As System.Windows.Forms.Label
	Friend WithEvents lblClient As System.Windows.Forms.Label
	Friend WithEvents imglImages As System.Windows.Forms.ImageList
	Friend WithEvents lblPolicy As System.Windows.Forms.Label
    Friend WithEvents _lvwSearchDetails_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSearchDetails_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSearchDetails_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSearchDetails_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSearchDetails_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSearchDetails_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSearchDetails_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSearchDetails_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSearchDetails_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSearchDetails_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lvwSearchDetails As System.Windows.Forms.ListView
	Friend WithEvents txtClientCode As System.Windows.Forms.TextBox
	Friend WithEvents cboStatus As System.Windows.Forms.ComboBox
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Friend WithEvents txtPolicy As System.Windows.Forms.TextBox
	Friend WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Friend WithEvents tabMainTab As System.Windows.Forms.TabControl
	Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctListPolicy))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Toolbar1 = New System.Windows.Forms.ToolStrip
        Me.TB_Event = New System.Windows.Forms.ToolStripButton
        Me.sep1 = New System.Windows.Forms.ToolStripSeparator
        Me.TB_RiskDetails = New System.Windows.Forms.ToolStripButton
        Me.sep2 = New System.Windows.Forms.ToolStripSeparator
        Me.TB_InformationChecklist = New System.Windows.Forms.ToolStripButton
        Me.TB_Policy = New System.Windows.Forms.ToolStripButton
        Me._Toolbar1_Button2 = New System.Windows.Forms.ToolStripSeparator
        Me._Toolbar1_Button4 = New System.Windows.Forms.ToolStripSeparator
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.stbStatus = New System.Windows.Forms.ToolStripStatusLabel
        Me.lblStatus = New System.Windows.Forms.Label
        Me.lblClient = New System.Windows.Forms.Label
        Me.lblPolicy = New System.Windows.Forms.Label
        Me.lvwSearchDetails = New System.Windows.Forms.ListView
        Me._lvwSearchDetails_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_10 = New System.Windows.Forms.ColumnHeader
        Me.imglImages = New System.Windows.Forms.ImageList(Me.components)
        Me.txtClientCode = New System.Windows.Forms.TextBox
        Me.cboStatus = New System.Windows.Forms.ComboBox
        Me.txtPolicy = New System.Windows.Forms.TextBox
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Toolbar1.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Toolbar1
        '
        Me.Toolbar1.Dock = System.Windows.Forms.DockStyle.None
        Me.Toolbar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TB_Event, Me.sep1, Me.TB_RiskDetails, Me.sep2, Me.TB_InformationChecklist, Me.TB_Policy})
        Me.Toolbar1.Location = New System.Drawing.Point(0, 0)
        Me.Toolbar1.Name = "Toolbar1"
        Me.Toolbar1.Size = New System.Drawing.Size(118, 25)
        Me.Toolbar1.TabIndex = 9
        '
        'TB_Event
        '
        Me.TB_Event.AutoSize = False
        Me.TB_Event.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TB_Event.Name = "TB_Event"
        Me.TB_Event.Size = New System.Drawing.Size(24, 22)
        Me.TB_Event.Tag = ""
        Me.TB_Event.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.TB_Event.ToolTipText = "Event"
        '
        'sep1
        '
        Me.sep1.Name = "sep1"
        Me.sep1.Size = New System.Drawing.Size(6, 25)
        '
        'TB_RiskDetails
        '
        Me.TB_RiskDetails.AutoSize = False
        Me.TB_RiskDetails.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TB_RiskDetails.Name = "TB_RiskDetails"
        Me.TB_RiskDetails.Size = New System.Drawing.Size(24, 22)
        Me.TB_RiskDetails.Tag = ""
        Me.TB_RiskDetails.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.TB_RiskDetails.ToolTipText = "Claim Details"
        '
        'sep2
        '
        Me.sep2.Name = "sep2"
        Me.sep2.Size = New System.Drawing.Size(6, 25)
        '
        'TB_InformationChecklist
        '
        Me.TB_InformationChecklist.AutoSize = False
        Me.TB_InformationChecklist.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TB_InformationChecklist.Name = "TB_InformationChecklist"
        Me.TB_InformationChecklist.Size = New System.Drawing.Size(24, 22)
        Me.TB_InformationChecklist.Tag = ""
        Me.TB_InformationChecklist.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.TB_InformationChecklist.ToolTipText = "Information Checklist"
        '
        'TB_Policy
        '
        Me.TB_Policy.AutoSize = False
        Me.TB_Policy.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TB_Policy.Name = "TB_Policy"
        Me.TB_Policy.Size = New System.Drawing.Size(24, 22)
        Me.TB_Policy.Tag = ""
        Me.TB_Policy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.TB_Policy.ToolTipText = "Policy"
        '
        '_Toolbar1_Button2
        '
        Me._Toolbar1_Button2.AutoSize = False
        Me._Toolbar1_Button2.Name = "_Toolbar1_Button2"
        Me._Toolbar1_Button2.Size = New System.Drawing.Size(6, 22)
        Me._Toolbar1_Button2.Tag = ""
        '
        '_Toolbar1_Button4
        '
        Me._Toolbar1_Button4.AutoSize = False
        Me._Toolbar1_Button4.Name = "_Toolbar1_Button4"
        Me._Toolbar1_Button4.Size = New System.Drawing.Size(6, 22)
        Me._Toolbar1_Button4.Tag = ""
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(576, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 32)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(581, 301)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.StatusStrip1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblClient)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPolicy)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwSearchDetails)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtClientCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtPolicy)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(573, 275)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Policy "
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.stbStatus})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 253)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(573, 22)
        Me.StatusStrip1.TabIndex = 10
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(0, 17)
        '
        'stbStatus
        '
        Me.stbStatus.AutoSize = False
        Me.stbStatus.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.stbStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken
        Me.stbStatus.Name = "stbStatus"
        Me.stbStatus.Size = New System.Drawing.Size(527, 17)
        Me.stbStatus.Spring = True
        Me.stbStatus.Text = "ToolStripStatusLabel2"
        Me.stbStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatus.Location = New System.Drawing.Point(16, 39)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatus.Size = New System.Drawing.Size(81, 17)
        Me.lblStatus.TabIndex = 5
        Me.lblStatus.Text = "Status:"
        '
        'lblClient
        '
        Me.lblClient.BackColor = System.Drawing.SystemColors.Control
        Me.lblClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClient.Location = New System.Drawing.Point(16, 15)
        Me.lblClient.Name = "lblClient"
        Me.lblClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClient.Size = New System.Drawing.Size(81, 17)
        Me.lblClient.TabIndex = 6
        Me.lblClient.Text = "Client code:"
        '
        'lblPolicy
        '
        Me.lblPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicy.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicy.Location = New System.Drawing.Point(296, 43)
        Me.lblPolicy.Name = "lblPolicy"
        Me.lblPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicy.Size = New System.Drawing.Size(44, 17)
        Me.lblPolicy.TabIndex = 8
        Me.lblPolicy.Text = "Policy:"
        '
        'lvwSearchDetails
        '
        Me.lvwSearchDetails.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSearchDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwSearchDetails.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSearchDetails_ColumnHeader_1, Me._lvwSearchDetails_ColumnHeader_2, Me._lvwSearchDetails_ColumnHeader_3, Me._lvwSearchDetails_ColumnHeader_4, Me._lvwSearchDetails_ColumnHeader_5, Me._lvwSearchDetails_ColumnHeader_6, Me._lvwSearchDetails_ColumnHeader_7, Me._lvwSearchDetails_ColumnHeader_8, Me._lvwSearchDetails_ColumnHeader_9, Me._lvwSearchDetails_ColumnHeader_10})
        Me.lvwSearchDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSearchDetails.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSearchDetails.FullRowSelect = True
        Me.lvwSearchDetails.LargeImageList = Me.imglImages
        Me.lvwSearchDetails.Location = New System.Drawing.Point(8, 68)
        Me.lvwSearchDetails.MultiSelect = False
        Me.lvwSearchDetails.Name = "lvwSearchDetails"
        Me.lvwSearchDetails.Size = New System.Drawing.Size(553, 160)
        Me.lvwSearchDetails.SmallImageList = Me.imglImages
        Me.lvwSearchDetails.TabIndex = 4
        Me.lvwSearchDetails.UseCompatibleStateImageBehavior = False
        Me.lvwSearchDetails.View = System.Windows.Forms.View.Details
        '
        '_lvwSearchDetails_ColumnHeader_1
        '
        Me._lvwSearchDetails_ColumnHeader_1.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_1.Text = "Policy Number"
        Me._lvwSearchDetails_ColumnHeader_1.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_2
        '
        Me._lvwSearchDetails_ColumnHeader_2.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_2.Text = "Regarding"
        Me._lvwSearchDetails_ColumnHeader_2.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_3
        '
        Me._lvwSearchDetails_ColumnHeader_3.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_3.Text = "Renewal Date"
        Me._lvwSearchDetails_ColumnHeader_3.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_4
        '
        Me._lvwSearchDetails_ColumnHeader_4.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_4.Text = "Insurer"
        Me._lvwSearchDetails_ColumnHeader_4.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_5
        '
        Me._lvwSearchDetails_ColumnHeader_5.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_5.Text = "Risk Type Description"
        Me._lvwSearchDetails_ColumnHeader_5.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_6
        '
        Me._lvwSearchDetails_ColumnHeader_6.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_6.Text = "Premium"
        Me._lvwSearchDetails_ColumnHeader_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwSearchDetails_ColumnHeader_6.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_7
        '
        Me._lvwSearchDetails_ColumnHeader_7.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_7.Text = "Status"
        Me._lvwSearchDetails_ColumnHeader_7.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_8
        '
        Me._lvwSearchDetails_ColumnHeader_8.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_8.Text = "Policy Type"
        Me._lvwSearchDetails_ColumnHeader_8.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_9
        '
        Me._lvwSearchDetails_ColumnHeader_9.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_9.Text = "Risk Type"
        Me._lvwSearchDetails_ColumnHeader_9.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_10
        '
        Me._lvwSearchDetails_ColumnHeader_10.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_10.Text = "Event Description"
        Me._lvwSearchDetails_ColumnHeader_10.Width = 201
        '
        'imglImages
        '
        Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.imglImages.Images.SetKeyName(0, "FindImage")
        '
        'txtClientCode
        '
        Me.txtClientCode.AcceptsReturn = True
        Me.txtClientCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtClientCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClientCode.Enabled = False
        Me.txtClientCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClientCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClientCode.Location = New System.Drawing.Point(104, 12)
        Me.txtClientCode.MaxLength = 30
        Me.txtClientCode.Name = "txtClientCode"
        Me.txtClientCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClientCode.Size = New System.Drawing.Size(217, 20)
        Me.txtClientCode.TabIndex = 2
        '
        'cboStatus
        '
        Me.cboStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cboStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboStatus.Location = New System.Drawing.Point(104, 36)
        Me.cboStatus.Name = "cboStatus"
        Me.cboStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboStatus.Size = New System.Drawing.Size(121, 21)
        Me.cboStatus.TabIndex = 3
        '
        'txtPolicy
        '
        Me.txtPolicy.AcceptsReturn = True
        Me.txtPolicy.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolicy.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicy.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicy.Location = New System.Drawing.Point(343, 41)
        Me.txtPolicy.MaxLength = 30
        Me.txtPolicy.Name = "txtPolicy"
        Me.txtPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicy.Size = New System.Drawing.Size(217, 20)
        Me.txtPolicy.TabIndex = 1
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
        'uctListPolicy
        '
        Me.Controls.Add(Me.Toolbar1)
        Me.Controls.Add(Me.tabMainTab)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctListPolicy"
        Me.Size = New System.Drawing.Size(580, 334)
        Me.Toolbar1.ResumeLayout(False)
        Me.Toolbar1.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub lvwSearchDetails_InitializeColumnKeys()
        Me._lvwSearchDetails_ColumnHeader_1.Name = ""
        Me._lvwSearchDetails_ColumnHeader_2.Name = ""
        Me._lvwSearchDetails_ColumnHeader_3.Name = ""
        Me._lvwSearchDetails_ColumnHeader_4.Name = ""
        Me._lvwSearchDetails_ColumnHeader_5.Name = ""
        Me._lvwSearchDetails_ColumnHeader_6.Name = ""
        Me._lvwSearchDetails_ColumnHeader_7.Name = ""
        Me._lvwSearchDetails_ColumnHeader_8.Name = ""
        Me._lvwSearchDetails_ColumnHeader_9.Name = ""
        Me._lvwSearchDetails_ColumnHeader_10.Name = ""
    End Sub
    Friend WithEvents sep1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents sep2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents stbStatus As System.Windows.Forms.ToolStripStatusLabel
#End Region
#Region "Upgrade Support"
	<System.Runtime.InteropServices.ProgId("lvwSearchDetailsMouseDownEventArgs_NET.lvwSearchDetailsMouseDownEventArgs")> _
	Public NotInheritable Class lvwSearchDetailsMouseDownEventArgs
		Inherits System.EventArgs
		Public m_lSelected As Integer
		Public Sub New(ByRef m_lSelected As Integer)
			MyBase.New()
			Me.m_lSelected = m_lSelected
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("PolicyListRefreshedEventArgs_NET.PolicyListRefreshedEventArgs")> _
	Public NotInheritable Class PolicyListRefreshedEventArgs
		Inherits System.EventArgs
		Public ItemsFound As Integer
		Public Sub New(ByRef ItemsFound As Integer)
			MyBase.New()
			Me.ItemsFound = ItemsFound
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("lvwSearchDetailsDblClickEventArgs_NET.lvwSearchDetailsDblClickEventArgs")> _
	Public NotInheritable Class lvwSearchDetailsDblClickEventArgs
		Inherits System.EventArgs
		Public m_lInsHolderCnt As Integer
		Public m_lInsuranceFolderCnt As Integer
		Public m_lInsFileCnt As Integer
		Public m_sShortName As String = ""
		Public m_sInsReference As String = ""
		Public m_lPolicyTypeId As Integer
		Public Sub New(ByRef m_lInsHolderCnt As Integer, ByRef m_lInsuranceFolderCnt As Integer, ByRef m_lInsFileCnt As Integer, ByRef m_sShortName As String, ByRef m_sInsReference As String, ByRef m_lPolicyTypeId As Integer)
			MyBase.New()
			Me.m_lInsHolderCnt = m_lInsHolderCnt
			Me.m_lInsuranceFolderCnt = m_lInsuranceFolderCnt
			Me.m_lInsFileCnt = m_lInsFileCnt
			Me.m_sShortName = m_sShortName
			Me.m_sInsReference = m_sInsReference
			Me.m_lPolicyTypeId = m_lPolicyTypeId
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