<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctListDocuments
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
	Friend WithEvents chkHidePrintedDocs As System.Windows.Forms.CheckBox
	Friend WithEvents lblClient As System.Windows.Forms.Label
	Friend WithEvents imglImages As System.Windows.Forms.ImageList
	Friend WithEvents lblPolicy As System.Windows.Forms.Label
	Friend WithEvents lblUser As System.Windows.Forms.Label
	Friend WithEvents lblShowDocuments As System.Windows.Forms.Label
	Friend WithEvents lblAccountHandler As System.Windows.Forms.Label
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
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
	Friend WithEvents _lvwSearchDetails_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSearchDetails_ColumnHeader_12 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lvwSearchDetails As System.Windows.Forms.ListView
	Friend WithEvents _stbStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Friend WithEvents stbStatus As System.Windows.Forms.StatusStrip
	Friend WithEvents txtClientCode As System.Windows.Forms.TextBox
	Friend WithEvents txtPolicy As System.Windows.Forms.TextBox
	Friend WithEvents cboUser As System.Windows.Forms.ComboBox
	Friend WithEvents chkShowCurrent As System.Windows.Forms.CheckBox
	Friend WithEvents chkSelectPrinter As System.Windows.Forms.CheckBox
	Friend WithEvents chkOffice As System.Windows.Forms.CheckBox
	Friend WithEvents chkAgent As System.Windows.Forms.CheckBox
	Friend WithEvents chkClient As System.Windows.Forms.CheckBox
	Friend WithEvents fraGroupClientAgentOffice As System.Windows.Forms.GroupBox
	Friend WithEvents optAgentPartyThenProduction As System.Windows.Forms.RadioButton
	Friend WithEvents optPartyThenProductionOrder As System.Windows.Forms.RadioButton
	Friend WithEvents optPrintDate As System.Windows.Forms.RadioButton
	Friend WithEvents fraOrderBy As System.Windows.Forms.GroupBox
	Friend WithEvents cboAccountHandler As System.Windows.Forms.ComboBox
	Friend WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Friend WithEvents tabMainTab As System.Windows.Forms.TabControl
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctListDocuments))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkHidePrintedDocs = New System.Windows.Forms.CheckBox()
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.lblYear = New System.Windows.Forms.Label()
        Me.CboYear = New System.Windows.Forms.ComboBox()
        Me.cboAccountHandler = New System.Windows.Forms.ComboBox()
        Me.lblClient = New System.Windows.Forms.Label()
        Me.lblPolicy = New System.Windows.Forms.Label()
        Me.lblUser = New System.Windows.Forms.Label()
        Me.lblShowDocuments = New System.Windows.Forms.Label()
        Me.lblAccountHandler = New System.Windows.Forms.Label()
        Me.lvwSearchDetails = New System.Windows.Forms.ListView()
        Me._lvwSearchDetails_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_11 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_12 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.imglImages = New System.Windows.Forms.ImageList(Me.components)
        Me.stbStatus = New System.Windows.Forms.StatusStrip()
        Me._stbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.txtClientCode = New System.Windows.Forms.TextBox()
        Me.txtPolicy = New System.Windows.Forms.TextBox()
        Me.cboUser = New System.Windows.Forms.ComboBox()
        Me.chkShowCurrent = New System.Windows.Forms.CheckBox()
        Me.chkSelectPrinter = New System.Windows.Forms.CheckBox()
        Me.fraGroupClientAgentOffice = New System.Windows.Forms.GroupBox()
        Me.chkOffice = New System.Windows.Forms.CheckBox()
        Me.chkAgent = New System.Windows.Forms.CheckBox()
        Me.chkClient = New System.Windows.Forms.CheckBox()
        Me.fraOrderBy = New System.Windows.Forms.GroupBox()
        Me.optPrintDate = New System.Windows.Forms.RadioButton()
        Me.optPartyThenProductionOrder = New System.Windows.Forms.RadioButton()
        Me.optAgentPartyThenProduction = New System.Windows.Forms.RadioButton()
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog()
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog()
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.stbStatus.SuspendLayout()
        Me.fraGroupClientAgentOffice.SuspendLayout()
        Me.fraOrderBy.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkHidePrintedDocs
        '
        Me.chkHidePrintedDocs.BackColor = System.Drawing.SystemColors.Control
        Me.chkHidePrintedDocs.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkHidePrintedDocs.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkHidePrintedDocs.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkHidePrintedDocs.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkHidePrintedDocs.Location = New System.Drawing.Point(16, 115)
        Me.chkHidePrintedDocs.Name = "chkHidePrintedDocs"
        Me.chkHidePrintedDocs.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkHidePrintedDocs.Size = New System.Drawing.Size(249, 22)
        Me.chkHidePrintedDocs.TabIndex = 12
        Me.chkHidePrintedDocs.Text = "Hide Documents Already Printed:"
        Me.chkHidePrintedDocs.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(173, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(0, 0)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(877, 621)
        Me.tabMainTab.TabIndex = 0
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblYear)
        Me._tabMainTab_TabPage0.Controls.Add(Me.CboYear)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboAccountHandler)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblClient)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPolicy)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblUser)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblShowDocuments)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblAccountHandler)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwSearchDetails)
        Me._tabMainTab_TabPage0.Controls.Add(Me.stbStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtClientCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtPolicy)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboUser)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkShowCurrent)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkSelectPrinter)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraGroupClientAgentOffice)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraOrderBy)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(869, 595)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Documents"
        '
        'lblYear
        '
        Me.lblYear.BackColor = System.Drawing.SystemColors.Control
        Me.lblYear.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblYear.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblYear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblYear.Location = New System.Drawing.Point(329, 65)
        Me.lblYear.Name = "lblYear"
        Me.lblYear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblYear.Size = New System.Drawing.Size(52, 20)
        Me.lblYear.TabIndex = 27
        Me.lblYear.Text = "Year:"
        Me.lblYear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CboYear
        '
        Me.CboYear.FormattingEnabled = True
        Me.CboYear.Location = New System.Drawing.Point(385, 64)
        Me.CboYear.Name = "CboYear"
        Me.CboYear.Size = New System.Drawing.Size(121, 21)
        Me.CboYear.TabIndex = 26
        '
        'cboAccountHandler
        '
        Me.cboAccountHandler.BackColor = System.Drawing.SystemColors.Window
        Me.cboAccountHandler.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboAccountHandler.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAccountHandler.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboAccountHandler.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboAccountHandler.Items.AddRange(New Object() {"(All)"})
        Me.cboAccountHandler.Location = New System.Drawing.Point(385, 39)
        Me.cboAccountHandler.Name = "cboAccountHandler"
        Me.cboAccountHandler.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboAccountHandler.Size = New System.Drawing.Size(193, 21)
        Me.cboAccountHandler.TabIndex = 21
        '
        'lblClient
        '
        Me.lblClient.BackColor = System.Drawing.SystemColors.Control
        Me.lblClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClient.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClient.Location = New System.Drawing.Point(14, 44)
        Me.lblClient.Name = "lblClient"
        Me.lblClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClient.Size = New System.Drawing.Size(81, 17)
        Me.lblClient.TabIndex = 2
        Me.lblClient.Text = "Client code:"
        '
        'lblPolicy
        '
        Me.lblPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicy.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicy.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicy.Location = New System.Drawing.Point(304, 90)
        Me.lblPolicy.Name = "lblPolicy"
        Me.lblPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicy.Size = New System.Drawing.Size(77, 15)
        Me.lblPolicy.TabIndex = 5
        Me.lblPolicy.Text = "Policy code:"
        '
        'lblUser
        '
        Me.lblUser.BackColor = System.Drawing.SystemColors.Control
        Me.lblUser.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUser.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUser.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUser.Location = New System.Drawing.Point(340, 10)
        Me.lblUser.Name = "lblUser"
        Me.lblUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUser.Size = New System.Drawing.Size(41, 20)
        Me.lblUser.TabIndex = 8
        Me.lblUser.Text = "User:"
        '
        'lblShowDocuments
        '
        Me.lblShowDocuments.AutoSize = True
        Me.lblShowDocuments.BackColor = System.Drawing.SystemColors.Control
        Me.lblShowDocuments.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowDocuments.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowDocuments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblShowDocuments.Location = New System.Drawing.Point(13, 13)
        Me.lblShowDocuments.Name = "lblShowDocuments"
        Me.lblShowDocuments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowDocuments.Size = New System.Drawing.Size(231, 13)
        Me.lblShowDocuments.TabIndex = 9
        Me.lblShowDocuments.Text = "Show documents for current user only:"
        '
        'lblAccountHandler
        '
        Me.lblAccountHandler.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountHandler.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountHandler.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountHandler.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountHandler.Location = New System.Drawing.Point(266, 40)
        Me.lblAccountHandler.Name = "lblAccountHandler"
        Me.lblAccountHandler.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountHandler.Size = New System.Drawing.Size(115, 20)
        Me.lblAccountHandler.TabIndex = 22
        Me.lblAccountHandler.Text = "Account handler:"
        '
        'lvwSearchDetails
        '
        Me.lvwSearchDetails.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSearchDetails.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSearchDetails_ColumnHeader_1, Me._lvwSearchDetails_ColumnHeader_2, Me._lvwSearchDetails_ColumnHeader_3, Me._lvwSearchDetails_ColumnHeader_4, Me._lvwSearchDetails_ColumnHeader_5, Me._lvwSearchDetails_ColumnHeader_6, Me._lvwSearchDetails_ColumnHeader_7, Me._lvwSearchDetails_ColumnHeader_8, Me._lvwSearchDetails_ColumnHeader_9, Me._lvwSearchDetails_ColumnHeader_10, Me._lvwSearchDetails_ColumnHeader_11, Me._lvwSearchDetails_ColumnHeader_12})
        Me.lvwSearchDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSearchDetails.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSearchDetails.LargeImageList = Me.imglImages
        Me.lvwSearchDetails.Location = New System.Drawing.Point(13, 151)
        Me.lvwSearchDetails.Name = "lvwSearchDetails"
        Me.lvwSearchDetails.Size = New System.Drawing.Size(839, 426)
        Me.lvwSearchDetails.SmallImageList = Me.imglImages
        Me.lvwSearchDetails.TabIndex = 6
        Me.lvwSearchDetails.UseCompatibleStateImageBehavior = False
        Me.lvwSearchDetails.View = System.Windows.Forms.View.Details
        '
        '_lvwSearchDetails_ColumnHeader_1
        '
        Me._lvwSearchDetails_ColumnHeader_1.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_1.Text = "Party"
        Me._lvwSearchDetails_ColumnHeader_1.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_2
        '
        Me._lvwSearchDetails_ColumnHeader_2.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_2.Text = "Policy"
        Me._lvwSearchDetails_ColumnHeader_2.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_3
        '
        Me._lvwSearchDetails_ColumnHeader_3.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_3.Text = "Claim"
        Me._lvwSearchDetails_ColumnHeader_3.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_4
        '
        Me._lvwSearchDetails_ColumnHeader_4.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_4.Text = "Description"
        Me._lvwSearchDetails_ColumnHeader_4.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_5
        '
        Me._lvwSearchDetails_ColumnHeader_5.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_5.Text = "Created By"
        Me._lvwSearchDetails_ColumnHeader_5.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_6
        '
        Me._lvwSearchDetails_ColumnHeader_6.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_6.Text = "Created When"
        Me._lvwSearchDetails_ColumnHeader_6.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_7
        '
        Me._lvwSearchDetails_ColumnHeader_7.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_7.Text = "Modified By"
        Me._lvwSearchDetails_ColumnHeader_7.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_8
        '
        Me._lvwSearchDetails_ColumnHeader_8.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_8.Text = "Modified When"
        Me._lvwSearchDetails_ColumnHeader_8.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_9
        '
        Me._lvwSearchDetails_ColumnHeader_9.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_9.Text = "Times Printed"
        Me._lvwSearchDetails_ColumnHeader_9.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_10
        '
        Me._lvwSearchDetails_ColumnHeader_10.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_10.Text = "Times Archived"
        Me._lvwSearchDetails_ColumnHeader_10.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_11
        '
        Me._lvwSearchDetails_ColumnHeader_11.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_11.Text = "Lead Agent"
        Me._lvwSearchDetails_ColumnHeader_11.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_12
        '
        Me._lvwSearchDetails_ColumnHeader_12.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_12.Text = ""
        Me._lvwSearchDetails_ColumnHeader_12.Width = 97
        '
        'imglImages
        '
        Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.imglImages.Images.SetKeyName(0, "FindImages")
        '
        'stbStatus
        '
        Me.stbStatus.Dock = System.Windows.Forms.DockStyle.None
        Me.stbStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._stbStatus_Panel1})
        Me.stbStatus.Location = New System.Drawing.Point(16, 118)
        Me.stbStatus.Name = "stbStatus"
        Me.stbStatus.ShowItemToolTips = True
        Me.stbStatus.Size = New System.Drawing.Size(532, 22)
        Me.stbStatus.TabIndex = 3
        '
        '_stbStatus_Panel1
        '
        Me._stbStatus_Panel1.AutoSize = False
        Me._stbStatus_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._stbStatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._stbStatus_Panel1.DoubleClickEnabled = True
        Me._stbStatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._stbStatus_Panel1.Name = "_stbStatus_Panel1"
        Me._stbStatus_Panel1.Size = New System.Drawing.Size(515, 22)
        Me._stbStatus_Panel1.Tag = ""
        Me._stbStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtClientCode
        '
        Me.txtClientCode.AcceptsReturn = True
        Me.txtClientCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtClientCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClientCode.Enabled = False
        Me.txtClientCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClientCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClientCode.Location = New System.Drawing.Point(95, 40)
        Me.txtClientCode.MaxLength = 30
        Me.txtClientCode.Name = "txtClientCode"
        Me.txtClientCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClientCode.Size = New System.Drawing.Size(161, 20)
        Me.txtClientCode.TabIndex = 1
        '
        'txtPolicy
        '
        Me.txtPolicy.AcceptsReturn = True
        Me.txtPolicy.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolicy.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicy.Enabled = False
        Me.txtPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicy.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicy.Location = New System.Drawing.Point(385, 87)
        Me.txtPolicy.MaxLength = 30
        Me.txtPolicy.Name = "txtPolicy"
        Me.txtPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicy.Size = New System.Drawing.Size(193, 20)
        Me.txtPolicy.TabIndex = 4
        '
        'cboUser
        '
        Me.cboUser.BackColor = System.Drawing.SystemColors.Window
        Me.cboUser.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboUser.Enabled = False
        Me.cboUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboUser.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboUser.Items.AddRange(New Object() {"(All)"})
        Me.cboUser.Location = New System.Drawing.Point(385, 9)
        Me.cboUser.Name = "cboUser"
        Me.cboUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboUser.Size = New System.Drawing.Size(193, 21)
        Me.cboUser.TabIndex = 7
        '
        'chkShowCurrent
        '
        Me.chkShowCurrent.BackColor = System.Drawing.SystemColors.Control
        Me.chkShowCurrent.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkShowCurrent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShowCurrent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkShowCurrent.Location = New System.Drawing.Point(243, 14)
        Me.chkShowCurrent.Name = "chkShowCurrent"
        Me.chkShowCurrent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkShowCurrent.Size = New System.Drawing.Size(11, 13)
        Me.chkShowCurrent.TabIndex = 10
        Me.chkShowCurrent.UseVisualStyleBackColor = False
        '
        'chkSelectPrinter
        '
        Me.chkSelectPrinter.BackColor = System.Drawing.SystemColors.Control
        Me.chkSelectPrinter.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkSelectPrinter.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSelectPrinter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSelectPrinter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSelectPrinter.Location = New System.Drawing.Point(12, 71)
        Me.chkSelectPrinter.Name = "chkSelectPrinter"
        Me.chkSelectPrinter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSelectPrinter.Size = New System.Drawing.Size(249, 15)
        Me.chkSelectPrinter.TabIndex = 11
        Me.chkSelectPrinter.Text = "Select Printer:"
        Me.chkSelectPrinter.UseVisualStyleBackColor = False
        '
        'fraGroupClientAgentOffice
        '
        Me.fraGroupClientAgentOffice.BackColor = System.Drawing.SystemColors.Control
        Me.fraGroupClientAgentOffice.Controls.Add(Me.chkOffice)
        Me.fraGroupClientAgentOffice.Controls.Add(Me.chkAgent)
        Me.fraGroupClientAgentOffice.Controls.Add(Me.chkClient)
        Me.fraGroupClientAgentOffice.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraGroupClientAgentOffice.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraGroupClientAgentOffice.Location = New System.Drawing.Point(584, 3)
        Me.fraGroupClientAgentOffice.Name = "fraGroupClientAgentOffice"
        Me.fraGroupClientAgentOffice.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraGroupClientAgentOffice.Size = New System.Drawing.Size(266, 46)
        Me.fraGroupClientAgentOffice.TabIndex = 13
        Me.fraGroupClientAgentOffice.TabStop = False
        '
        'chkOffice
        '
        Me.chkOffice.BackColor = System.Drawing.SystemColors.Control
        Me.chkOffice.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOffice.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOffice.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOffice.Location = New System.Drawing.Point(176, 18)
        Me.chkOffice.Name = "chkOffice"
        Me.chkOffice.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOffice.Size = New System.Drawing.Size(85, 15)
        Me.chkOffice.TabIndex = 20
        Me.chkOffice.Text = "For Office"
        Me.chkOffice.UseVisualStyleBackColor = False
        '
        'chkAgent
        '
        Me.chkAgent.BackColor = System.Drawing.SystemColors.Control
        Me.chkAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAgent.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAgent.Location = New System.Drawing.Point(94, 15)
        Me.chkAgent.Name = "chkAgent"
        Me.chkAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAgent.Size = New System.Drawing.Size(83, 22)
        Me.chkAgent.TabIndex = 19
        Me.chkAgent.Text = "For Agent"
        Me.chkAgent.UseVisualStyleBackColor = False
        '
        'chkClient
        '
        Me.chkClient.BackColor = System.Drawing.SystemColors.Control
        Me.chkClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkClient.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkClient.Location = New System.Drawing.Point(7, 18)
        Me.chkClient.Name = "chkClient"
        Me.chkClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkClient.Size = New System.Drawing.Size(83, 15)
        Me.chkClient.TabIndex = 18
        Me.chkClient.Text = "For Client"
        Me.chkClient.UseVisualStyleBackColor = False
        '
        'fraOrderBy
        '
        Me.fraOrderBy.BackColor = System.Drawing.SystemColors.Control
        Me.fraOrderBy.Controls.Add(Me.optPrintDate)
        Me.fraOrderBy.Controls.Add(Me.optPartyThenProductionOrder)
        Me.fraOrderBy.Controls.Add(Me.optAgentPartyThenProduction)
        Me.fraOrderBy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraOrderBy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraOrderBy.Location = New System.Drawing.Point(584, 51)
        Me.fraOrderBy.Name = "fraOrderBy"
        Me.fraOrderBy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraOrderBy.Size = New System.Drawing.Size(266, 80)
        Me.fraOrderBy.TabIndex = 14
        Me.fraOrderBy.TabStop = False
        Me.fraOrderBy.Text = "Ordering"
        '
        'optPrintDate
        '
        Me.optPrintDate.BackColor = System.Drawing.SystemColors.Control
        Me.optPrintDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.optPrintDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optPrintDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optPrintDate.Location = New System.Drawing.Point(4, 15)
        Me.optPrintDate.Name = "optPrintDate"
        Me.optPrintDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optPrintDate.Size = New System.Drawing.Size(150, 15)
        Me.optPrintDate.TabIndex = 15
        Me.optPrintDate.TabStop = True
        Me.optPrintDate.Text = "Modified Date"
        Me.optPrintDate.UseVisualStyleBackColor = False
        '
        'optPartyThenProductionOrder
        '
        Me.optPartyThenProductionOrder.BackColor = System.Drawing.SystemColors.Control
        Me.optPartyThenProductionOrder.Cursor = System.Windows.Forms.Cursors.Default
        Me.optPartyThenProductionOrder.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optPartyThenProductionOrder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optPartyThenProductionOrder.Location = New System.Drawing.Point(4, 27)
        Me.optPartyThenProductionOrder.Name = "optPartyThenProductionOrder"
        Me.optPartyThenProductionOrder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optPartyThenProductionOrder.Size = New System.Drawing.Size(222, 31)
        Me.optPartyThenProductionOrder.TabIndex = 16
        Me.optPartyThenProductionOrder.TabStop = True
        Me.optPartyThenProductionOrder.Text = "Party, then Production Order"
        Me.optPartyThenProductionOrder.UseVisualStyleBackColor = False
        '
        'optAgentPartyThenProduction
        '
        Me.optAgentPartyThenProduction.BackColor = System.Drawing.SystemColors.Control
        Me.optAgentPartyThenProduction.Cursor = System.Windows.Forms.Cursors.Default
        Me.optAgentPartyThenProduction.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optAgentPartyThenProduction.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optAgentPartyThenProduction.Location = New System.Drawing.Point(4, 55)
        Me.optAgentPartyThenProduction.Name = "optAgentPartyThenProduction"
        Me.optAgentPartyThenProduction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optAgentPartyThenProduction.Size = New System.Drawing.Size(228, 18)
        Me.optAgentPartyThenProduction.TabIndex = 17
        Me.optAgentPartyThenProduction.TabStop = True
        Me.optAgentPartyThenProduction.Text = "Agent, Party, then Production Order"
        Me.optAgentPartyThenProduction.UseVisualStyleBackColor = False
        '
        'uctListDocuments
        '
        Me.Controls.Add(Me.chkHidePrintedDocs)
        Me.Controls.Add(Me.tabMainTab)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctListDocuments"
        Me.Size = New System.Drawing.Size(876, 618)
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.stbStatus.ResumeLayout(False)
        Me.stbStatus.PerformLayout()
        Me.fraGroupClientAgentOffice.ResumeLayout(False)
        Me.fraOrderBy.ResumeLayout(False)
        Me.ResumeLayout(False)

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
        Me._lvwSearchDetails_ColumnHeader_11.Name = ""
        Me._lvwSearchDetails_ColumnHeader_12.Name = ""
    End Sub

    Friend WithEvents CboYear As System.Windows.Forms.ComboBox
    Friend WithEvents lblYear As System.Windows.Forms.Label
#End Region 
#Region "Upgrade Support"
	<System.Runtime.InteropServices.ProgId("lvwSearchDetailsItemClickEventArgs_NET.lvwSearchDetailsItemClickEventArgs")> _
	Public NotInheritable Class lvwSearchDetailsItemClickEventArgs
		Inherits System.EventArgs
		Public bIsEditable As Boolean
		Public Sub New(ByRef bIsEditable As Boolean)
			MyBase.New()
			Me.bIsEditable = bIsEditable
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("lvwSearchDetailsClickEventArgs_NET.lvwSearchDetailsClickEventArgs")> _
	Public NotInheritable Class lvwSearchDetailsClickEventArgs
		Inherits System.EventArgs
		Public bSelected As Boolean
		Public Sub New(ByRef bSelected As Boolean)
			MyBase.New()
			Me.bSelected = bSelected
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