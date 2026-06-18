<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctListEvents
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
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Friend WithEvents txtDate As System.Windows.Forms.TextBox
    Friend WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Friend WithEvents tabMainTab As System.Windows.Forms.TabControl
	Friend WithEvents imglImages As System.Windows.Forms.ImageList
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctListEvents))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.txtDate = New System.Windows.Forms.TextBox
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lvwSearchDetails = New System.Windows.Forms.ListView
        Me._lvwSearchDetails_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
        Me.imglImages = New System.Windows.Forms.ImageList(Me.components)
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.cmdFindClaim = New System.Windows.Forms.Button
        Me.cmdFindPolicy = New System.Windows.Forms.Button
        Me.lblPolicy = New System.Windows.Forms.Label
        Me.lblClaim = New System.Windows.Forms.Label
        Me.lblType = New System.Windows.Forms.Label
        Me.lblUser = New System.Windows.Forms.Label
        Me.lblFromDate = New System.Windows.Forms.Label
        Me.lblToDate = New System.Windows.Forms.Label
        Me.lblCaseNumber = New System.Windows.Forms.Label
        Me.txtPolicy = New System.Windows.Forms.TextBox
        Me.txtClaim = New System.Windows.Forms.TextBox
        Me.cboType = New System.Windows.Forms.ComboBox
        Me.cboUser = New System.Windows.Forms.ComboBox
        Me.txtFromDate = New System.Windows.Forms.TextBox
        Me.txtToDate = New System.Windows.Forms.TextBox
        Me.cmdRefresh = New System.Windows.Forms.Button
        Me.txtCaseNumber = New System.Windows.Forms.TextBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.stbStatus = New System.Windows.Forms.StatusStrip
        Me._stbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.stbStatus.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtDate
        '
        Me.txtDate.AcceptsReturn = True
        Me.txtDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDate.Location = New System.Drawing.Point(112, 352)
        Me.txtDate.MaxLength = 30
        Me.txtDate.Name = "txtDate"
        Me.txtDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDate.Size = New System.Drawing.Size(145, 20)
        Me.txtDate.TabIndex = 14
        Me.txtDate.Visible = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(580, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(0, 0)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(584, 341)
        Me.tabMainTab.TabIndex = 0
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.Panel1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Panel3)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Panel2)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(576, 315)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Events"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.lvwSearchDetails)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 102)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(576, 192)
        Me.Panel1.TabIndex = 21
        '
        'lvwSearchDetails
        '
        Me.lvwSearchDetails.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSearchDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwSearchDetails.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSearchDetails_ColumnHeader_1, Me._lvwSearchDetails_ColumnHeader_2, Me._lvwSearchDetails_ColumnHeader_3, Me._lvwSearchDetails_ColumnHeader_4, Me._lvwSearchDetails_ColumnHeader_5, Me._lvwSearchDetails_ColumnHeader_6, Me._lvwSearchDetails_ColumnHeader_7, Me._lvwSearchDetails_ColumnHeader_8})
        Me.lvwSearchDetails.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwSearchDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSearchDetails.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSearchDetails.FullRowSelect = True
        Me.lvwSearchDetails.HideSelection = False
        Me.lvwSearchDetails.LargeImageList = Me.imglImages
        Me.lvwSearchDetails.Location = New System.Drawing.Point(0, 0)
        Me.lvwSearchDetails.MultiSelect = False
        Me.lvwSearchDetails.Name = "lvwSearchDetails"
        Me.lvwSearchDetails.Size = New System.Drawing.Size(576, 192)
        Me.lvwSearchDetails.SmallImageList = Me.imglImages
        Me.lvwSearchDetails.TabIndex = 11
        Me.lvwSearchDetails.UseCompatibleStateImageBehavior = False
        Me.lvwSearchDetails.View = System.Windows.Forms.View.Details
        '
        '_lvwSearchDetails_ColumnHeader_1
        '
        Me._lvwSearchDetails_ColumnHeader_1.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_1.Text = "Date"
        Me._lvwSearchDetails_ColumnHeader_1.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_2
        '
        Me._lvwSearchDetails_ColumnHeader_2.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_2.Text = "Type"
        Me._lvwSearchDetails_ColumnHeader_2.Width = 167
        '
        '_lvwSearchDetails_ColumnHeader_3
        '
        Me._lvwSearchDetails_ColumnHeader_3.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_3.Text = "Policy"
        Me._lvwSearchDetails_ColumnHeader_3.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_4
        '
        Me._lvwSearchDetails_ColumnHeader_4.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_4.Text = "Claim"
        Me._lvwSearchDetails_ColumnHeader_4.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_5
        '
        Me._lvwSearchDetails_ColumnHeader_5.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_5.Text = "Description"
        Me._lvwSearchDetails_ColumnHeader_5.Width = 481
        '
        '_lvwSearchDetails_ColumnHeader_6
        '
        Me._lvwSearchDetails_ColumnHeader_6.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_6.Text = "User"
        Me._lvwSearchDetails_ColumnHeader_6.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_7
        '
        Me._lvwSearchDetails_ColumnHeader_7.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_7.Text = "Priority"
        Me._lvwSearchDetails_ColumnHeader_7.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_8
        '
        Me._lvwSearchDetails_ColumnHeader_8.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_8.Text = "Status"
        Me._lvwSearchDetails_ColumnHeader_8.Width = 97
        '
        'imglImages
        '
        Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.imglImages.Images.SetKeyName(0, "FindImage")
        Me.imglImages.Images.SetKeyName(1, "NotesImage")
        Me.imglImages.Images.SetKeyName(2, "NotesImage2")
        Me.imglImages.Images.SetKeyName(3, "NotesImage3")
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.cmdFindClaim)
        Me.Panel3.Controls.Add(Me.cmdFindPolicy)
        Me.Panel3.Controls.Add(Me.lblPolicy)
        Me.Panel3.Controls.Add(Me.lblClaim)
        Me.Panel3.Controls.Add(Me.lblType)
        Me.Panel3.Controls.Add(Me.lblUser)
        Me.Panel3.Controls.Add(Me.lblFromDate)
        Me.Panel3.Controls.Add(Me.lblToDate)
        Me.Panel3.Controls.Add(Me.lblCaseNumber)
        Me.Panel3.Controls.Add(Me.txtPolicy)
        Me.Panel3.Controls.Add(Me.txtClaim)
        Me.Panel3.Controls.Add(Me.cboType)
        Me.Panel3.Controls.Add(Me.cboUser)
        Me.Panel3.Controls.Add(Me.txtFromDate)
        Me.Panel3.Controls.Add(Me.txtToDate)
        Me.Panel3.Controls.Add(Me.cmdRefresh)
        Me.Panel3.Controls.Add(Me.txtCaseNumber)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(576, 102)
        Me.Panel3.TabIndex = 23
        '
        'cmdFindClaim
        '
        Me.cmdFindClaim.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindClaim.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindClaim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindClaim.Location = New System.Drawing.Point(260, 12)
        Me.cmdFindClaim.Name = "cmdFindClaim"
        Me.cmdFindClaim.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindClaim.Size = New System.Drawing.Size(89, 21)
        Me.cmdFindClaim.TabIndex = 23
        Me.cmdFindClaim.Text = "Claim code..."
        Me.cmdFindClaim.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindClaim.UseVisualStyleBackColor = False
        '
        'cmdFindPolicy
        '
        Me.cmdFindPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindPolicy.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindPolicy.Location = New System.Drawing.Point(8, 11)
        Me.cmdFindPolicy.Name = "cmdFindPolicy"
        Me.cmdFindPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindPolicy.Size = New System.Drawing.Size(89, 22)
        Me.cmdFindPolicy.TabIndex = 21
        Me.cmdFindPolicy.Text = "Policy code..."
        Me.cmdFindPolicy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindPolicy.UseVisualStyleBackColor = False
        '
        'lblPolicy
        '
        Me.lblPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicy.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicy.Location = New System.Drawing.Point(11, 13)
        Me.lblPolicy.Name = "lblPolicy"
        Me.lblPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicy.Size = New System.Drawing.Size(81, 17)
        Me.lblPolicy.TabIndex = 30
        Me.lblPolicy.Text = "Policy code:"
        Me.lblPolicy.Visible = False
        '
        'lblClaim
        '
        Me.lblClaim.BackColor = System.Drawing.SystemColors.Control
        Me.lblClaim.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClaim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClaim.Location = New System.Drawing.Point(259, 13)
        Me.lblClaim.Name = "lblClaim"
        Me.lblClaim.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClaim.Size = New System.Drawing.Size(81, 17)
        Me.lblClaim.TabIndex = 31
        Me.lblClaim.Text = "Claim code:"
        '
        'lblType
        '
        Me.lblType.BackColor = System.Drawing.SystemColors.Control
        Me.lblType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblType.Location = New System.Drawing.Point(11, 40)
        Me.lblType.Name = "lblType"
        Me.lblType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblType.Size = New System.Drawing.Size(81, 17)
        Me.lblType.TabIndex = 35
        Me.lblType.Text = "Event type:"
        '
        'lblUser
        '
        Me.lblUser.BackColor = System.Drawing.SystemColors.Control
        Me.lblUser.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUser.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUser.Location = New System.Drawing.Point(259, 40)
        Me.lblUser.Name = "lblUser"
        Me.lblUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUser.Size = New System.Drawing.Size(81, 17)
        Me.lblUser.TabIndex = 34
        Me.lblUser.Text = "User name:"
        '
        'lblFromDate
        '
        Me.lblFromDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblFromDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFromDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFromDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFromDate.Location = New System.Drawing.Point(11, 64)
        Me.lblFromDate.Name = "lblFromDate"
        Me.lblFromDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFromDate.Size = New System.Drawing.Size(81, 17)
        Me.lblFromDate.TabIndex = 32
        Me.lblFromDate.Text = "From date:"
        '
        'lblToDate
        '
        Me.lblToDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblToDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblToDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblToDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblToDate.Location = New System.Drawing.Point(259, 64)
        Me.lblToDate.Name = "lblToDate"
        Me.lblToDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblToDate.Size = New System.Drawing.Size(81, 17)
        Me.lblToDate.TabIndex = 33
        Me.lblToDate.Text = "To date:"
        '
        'lblCaseNumber
        '
        Me.lblCaseNumber.AutoSize = True
        Me.lblCaseNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblCaseNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCaseNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCaseNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCaseNumber.Location = New System.Drawing.Point(11, 13)
        Me.lblCaseNumber.Name = "lblCaseNumber"
        Me.lblCaseNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCaseNumber.Size = New System.Drawing.Size(34, 13)
        Me.lblCaseNumber.TabIndex = 36
        Me.lblCaseNumber.Text = "Case:"
        Me.lblCaseNumber.Visible = False
        '
        'txtPolicy
        '
        Me.txtPolicy.AcceptsReturn = True
        Me.txtPolicy.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolicy.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicy.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicy.Location = New System.Drawing.Point(103, 13)
        Me.txtPolicy.MaxLength = 30
        Me.txtPolicy.Name = "txtPolicy"
        Me.txtPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicy.Size = New System.Drawing.Size(145, 20)
        Me.txtPolicy.TabIndex = 22
        '
        'txtClaim
        '
        Me.txtClaim.AcceptsReturn = True
        Me.txtClaim.BackColor = System.Drawing.SystemColors.Window
        Me.txtClaim.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClaim.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClaim.Location = New System.Drawing.Point(355, 13)
        Me.txtClaim.MaxLength = 30
        Me.txtClaim.Name = "txtClaim"
        Me.txtClaim.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClaim.Size = New System.Drawing.Size(145, 20)
        Me.txtClaim.TabIndex = 24
        '
        'cboType
        '
        Me.cboType.BackColor = System.Drawing.SystemColors.Window
        Me.cboType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboType.Location = New System.Drawing.Point(103, 37)
        Me.cboType.Name = "cboType"
        Me.cboType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboType.Size = New System.Drawing.Size(145, 21)
        Me.cboType.TabIndex = 25
        '
        'cboUser
        '
        Me.cboUser.BackColor = System.Drawing.SystemColors.Window
        Me.cboUser.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboUser.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboUser.Location = New System.Drawing.Point(355, 37)
        Me.cboUser.Name = "cboUser"
        Me.cboUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboUser.Size = New System.Drawing.Size(145, 21)
        Me.cboUser.Sorted = True
        Me.cboUser.TabIndex = 26
        '
        'txtFromDate
        '
        Me.txtFromDate.AcceptsReturn = True
        Me.txtFromDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtFromDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFromDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFromDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFromDate.Location = New System.Drawing.Point(103, 61)
        Me.txtFromDate.MaxLength = 30
        Me.txtFromDate.Name = "txtFromDate"
        Me.txtFromDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFromDate.Size = New System.Drawing.Size(145, 20)
        Me.txtFromDate.TabIndex = 27
        '
        'txtToDate
        '
        Me.txtToDate.AcceptsReturn = True
        Me.txtToDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtToDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtToDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtToDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtToDate.Location = New System.Drawing.Point(355, 61)
        Me.txtToDate.MaxLength = 30
        Me.txtToDate.Name = "txtToDate"
        Me.txtToDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtToDate.Size = New System.Drawing.Size(145, 20)
        Me.txtToDate.TabIndex = 28
        '
        'cmdRefresh
        '
        Me.cmdRefresh.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRefresh.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRefresh.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRefresh.Location = New System.Drawing.Point(505, 13)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRefresh.Size = New System.Drawing.Size(64, 22)
        Me.cmdRefresh.TabIndex = 29
        Me.cmdRefresh.Text = "&Refresh"
        Me.cmdRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRefresh.UseVisualStyleBackColor = False
        '
        'txtCaseNumber
        '
        Me.txtCaseNumber.AcceptsReturn = True
        Me.txtCaseNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtCaseNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCaseNumber.Enabled = False
        Me.txtCaseNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCaseNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCaseNumber.Location = New System.Drawing.Point(103, 13)
        Me.txtCaseNumber.MaxLength = 0
        Me.txtCaseNumber.Name = "txtCaseNumber"
        Me.txtCaseNumber.ReadOnly = True
        Me.txtCaseNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCaseNumber.Size = New System.Drawing.Size(145, 20)
        Me.txtCaseNumber.TabIndex = 37
        Me.txtCaseNumber.Visible = False
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.stbStatus)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 294)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(576, 21)
        Me.Panel2.TabIndex = 22
        '
        'stbStatus
        '
        Me.stbStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._stbStatus_Panel1})
        Me.stbStatus.Location = New System.Drawing.Point(0, -1)
        Me.stbStatus.Name = "stbStatus"
        Me.stbStatus.ShowItemToolTips = True
        Me.stbStatus.Size = New System.Drawing.Size(576, 22)
        Me.stbStatus.SizingGrip = False
        Me.stbStatus.TabIndex = 13
        '
        '_stbStatus_Panel1
        '
        Me._stbStatus_Panel1.AutoSize = False
        Me._stbStatus_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._stbStatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._stbStatus_Panel1.DoubleClickEnabled = True
        Me._stbStatus_Panel1.ForeColor = System.Drawing.Color.Black
        Me._stbStatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._stbStatus_Panel1.Name = "_stbStatus_Panel1"
        Me._stbStatus_Panel1.Size = New System.Drawing.Size(572, 22)
        Me._stbStatus_Panel1.Tag = ""
        Me._stbStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'uctListEvents
        '
        Me.Controls.Add(Me.txtDate)
        Me.Controls.Add(Me.tabMainTab)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctListEvents"
        Me.Size = New System.Drawing.Size(584, 341)
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.stbStatus.ResumeLayout(False)
        Me.stbStatus.PerformLayout()
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
    End Sub
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents cmdFindClaim As System.Windows.Forms.Button
    Friend WithEvents cmdFindPolicy As System.Windows.Forms.Button
    Friend WithEvents lblPolicy As System.Windows.Forms.Label
    Friend WithEvents lblClaim As System.Windows.Forms.Label
    Friend WithEvents lblType As System.Windows.Forms.Label
    Friend WithEvents lblUser As System.Windows.Forms.Label
    Friend WithEvents lblFromDate As System.Windows.Forms.Label
    Friend WithEvents lblToDate As System.Windows.Forms.Label
    Friend WithEvents lblCaseNumber As System.Windows.Forms.Label
    Friend WithEvents txtPolicy As System.Windows.Forms.TextBox
    Friend WithEvents txtClaim As System.Windows.Forms.TextBox
    Friend WithEvents cboType As System.Windows.Forms.ComboBox
    Friend WithEvents cboUser As System.Windows.Forms.ComboBox
    Friend WithEvents txtFromDate As System.Windows.Forms.TextBox
    Friend WithEvents txtToDate As System.Windows.Forms.TextBox
    Friend WithEvents cmdRefresh As System.Windows.Forms.Button
    Friend WithEvents txtCaseNumber As System.Windows.Forms.TextBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents stbStatus As System.Windows.Forms.StatusStrip
    Friend WithEvents _stbStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lvwSearchDetails As System.Windows.Forms.ListView
    Friend WithEvents _lvwSearchDetails_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSearchDetails_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSearchDetails_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSearchDetails_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSearchDetails_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSearchDetails_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSearchDetails_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSearchDetails_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
#End Region
#Region "Upgrade Support"
	<System.Runtime.InteropServices.ProgId("lvwSearchDetailsClickEventArgs_NET.lvwSearchDetailsClickEventArgs")> _
	Public NotInheritable Class lvwSearchDetailsClickEventArgs
		Inherits System.EventArgs
		Public lEventCnt As Integer
		Public lPartyCnt As Integer
		Public lInsuranceFileCnt As Integer
		Public sPolicyDesc As String = ""
		Public lInsuranceFileStructureId As Integer
		Public lClaimCnt As Integer
		Public sClaimDesc As String = ""
		Public lOldAddressCnt As Integer
		Public lNewAddressCnt As Integer
		Public lDocumentCnt As Integer
		Public sEventType As String = ""
		Public lOldPartyTypeID As Integer
		Public sDocumentRef As String = ""
		Public dtNoteDate As Date
		Public sEventLogSubject As String = ""
		Public lEventTypeGroupId As Integer
		Public sEventTypeGroupDescription As String = ""
		Public lEventLogSubjectId As Integer
		Public Sub New(ByRef lEventCnt As Integer, ByRef lPartyCnt As Integer, ByRef lInsuranceFileCnt As Integer, ByRef sPolicyDesc As String, ByRef lInsuranceFileStructureId As Integer, ByRef lClaimCnt As Integer, ByRef sClaimDesc As String, ByRef lOldAddressCnt As Integer, ByRef lNewAddressCnt As Integer, ByRef lDocumentCnt As Integer, ByRef sEventType As String, ByRef lOldPartyTypeID As Integer, ByRef sDocumentRef As String, ByRef dtNoteDate As Date, ByRef sEventLogSubject As String, ByRef lEventTypeGroupId As Integer, ByRef sEventTypeGroupDescription As String, ByRef lEventLogSubjectId As Integer)
			MyBase.New()
			Me.lEventCnt = lEventCnt
			Me.lPartyCnt = lPartyCnt
			Me.lInsuranceFileCnt = lInsuranceFileCnt
			Me.sPolicyDesc = sPolicyDesc
			Me.lInsuranceFileStructureId = lInsuranceFileStructureId
			Me.lClaimCnt = lClaimCnt
			Me.sClaimDesc = sClaimDesc
			Me.lOldAddressCnt = lOldAddressCnt
			Me.lNewAddressCnt = lNewAddressCnt
			Me.lDocumentCnt = lDocumentCnt
			Me.sEventType = sEventType
			Me.lOldPartyTypeID = lOldPartyTypeID
			Me.sDocumentRef = sDocumentRef
			Me.dtNoteDate = dtNoteDate
			Me.sEventLogSubject = sEventLogSubject
			Me.lEventTypeGroupId = lEventTypeGroupId
			Me.sEventTypeGroupDescription = sEventTypeGroupDescription
			Me.lEventLogSubjectId = lEventLogSubjectId
		End Sub
    End Class


    <System.Runtime.InteropServices.ProgId("lvwSearchDetailsClickEventArgs_NET.lvwSearchDetailsKeyUpEventArgs")> _
 Public NotInheritable Class lvwSearchDetailsKeyUpEventArgs
        Inherits System.EventArgs
        Public lEventCnt As Integer
        Public lPartyCnt As Integer
        Public lInsuranceFileCnt As Integer
        Public sPolicyDesc As String = ""
        Public lInsuranceFileStructureId As Integer
        Public lClaimCnt As Integer
        Public sClaimDesc As String = ""
        Public lOldAddressCnt As Integer
        Public lNewAddressCnt As Integer
        Public lDocumentCnt As Integer
        Public sEventType As String = ""
        Public lOldPartyTypeID As Integer
        Public sDocumentRef As String = ""
        Public dtNoteDate As Date
        Public sEventLogSubject As String = ""
        Public lEventTypeGroupId As Integer
        Public sEventTypeGroupDescription As String = ""
        Public lEventLogSubjectId As Integer
        Public Sub New(ByRef lEventCnt As Integer, ByRef lPartyCnt As Integer, ByRef lInsuranceFileCnt As Integer, ByRef sPolicyDesc As String, ByRef lInsuranceFileStructureId As Integer, ByRef lClaimCnt As Integer, ByRef sClaimDesc As String, ByRef lOldAddressCnt As Integer, ByRef lNewAddressCnt As Integer, ByRef lDocumentCnt As Integer, ByRef sEventType As String, ByRef lOldPartyTypeID As Integer, ByRef sDocumentRef As String, ByRef dtNoteDate As Date, ByRef sEventLogSubject As String, ByRef lEventTypeGroupId As Integer, ByRef sEventTypeGroupDescription As String, ByRef lEventLogSubjectId As Integer)
            MyBase.New()
            Me.lEventCnt = lEventCnt
            Me.lPartyCnt = lPartyCnt
            Me.lInsuranceFileCnt = lInsuranceFileCnt
            Me.sPolicyDesc = sPolicyDesc
            Me.lInsuranceFileStructureId = lInsuranceFileStructureId
            Me.lClaimCnt = lClaimCnt
            Me.sClaimDesc = sClaimDesc
            Me.lOldAddressCnt = lOldAddressCnt
            Me.lNewAddressCnt = lNewAddressCnt
            Me.lDocumentCnt = lDocumentCnt
            Me.sEventType = sEventType
            Me.lOldPartyTypeID = lOldPartyTypeID
            Me.sDocumentRef = sDocumentRef
            Me.dtNoteDate = dtNoteDate
            Me.sEventLogSubject = sEventLogSubject
            Me.lEventTypeGroupId = lEventTypeGroupId
            Me.sEventTypeGroupDescription = sEventTypeGroupDescription
            Me.lEventLogSubjectId = lEventLogSubjectId
        End Sub
    End Class
	<System.Runtime.InteropServices.ProgId("lvwSearchDetailsDblClickEventArgs_NET.lvwSearchDetailsDblClickEventArgs")> _
	Public NotInheritable Class lvwSearchDetailsDblClickEventArgs
		Inherits System.EventArgs
		Public lEventCnt As Integer
		Public lPartyCnt As Integer
		Public lInsuranceFileCnt As Integer
		Public sPolicyDesc As String = ""
		Public lInsuranceFileStructureId As Integer
		Public lClaimCnt As Integer
		Public sClaimDesc As String = ""
		Public lOldAddressCnt As Integer
		Public lNewAddressCnt As Integer
		Public lDocumentCnt As Integer
		Public sEventType As String = ""
		Public lOldPartyTypeID As Integer
		Public sDocumentRef As String = ""
		Public dtNoteDate As Date
		Public lFSAComplaintFolderCnt As Integer
		Public Sub New(ByRef lEventCnt As Integer, ByRef lPartyCnt As Integer, ByRef lInsuranceFileCnt As Integer, ByRef sPolicyDesc As String, ByRef lInsuranceFileStructureId As Integer, ByRef lClaimCnt As Integer, ByRef sClaimDesc As String, ByRef lOldAddressCnt As Integer, ByRef lNewAddressCnt As Integer, ByRef lDocumentCnt As Integer, ByRef sEventType As String, ByRef lOldPartyTypeID As Integer, ByRef sDocumentRef As String, ByRef dtNoteDate As Date, ByRef lFSAComplaintFolderCnt As Integer)
			MyBase.New()
			Me.lEventCnt = lEventCnt
			Me.lPartyCnt = lPartyCnt
			Me.lInsuranceFileCnt = lInsuranceFileCnt
			Me.sPolicyDesc = sPolicyDesc
			Me.lInsuranceFileStructureId = lInsuranceFileStructureId
			Me.lClaimCnt = lClaimCnt
			Me.sClaimDesc = sClaimDesc
			Me.lOldAddressCnt = lOldAddressCnt
			Me.lNewAddressCnt = lNewAddressCnt
			Me.lDocumentCnt = lDocumentCnt
			Me.sEventType = sEventType
			Me.lOldPartyTypeID = lOldPartyTypeID
			Me.sDocumentRef = sDocumentRef
			Me.dtNoteDate = dtNoteDate
			Me.lFSAComplaintFolderCnt = lFSAComplaintFolderCnt
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