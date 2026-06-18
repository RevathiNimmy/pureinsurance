<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmInterface
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		Form_Initialize_renamed()
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			Static fTerminateCalled As Boolean
			If Not fTerminateCalled Then
				Form_Terminate_renamed()
				fTerminateCalled = True
			End If
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents cmdSave As System.Windows.Forms.Button
	Public WithEvents cmdPreviewStatement As System.Windows.Forms.Button
	Public WithEvents cmdNext As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdFindNow As System.Windows.Forms.Button
	Public WithEvents cmdNewSearch As System.Windows.Forms.Button
	Public WithEvents chkTransAuthLimit As System.Windows.Forms.CheckBox
	Public WithEvents txtAuthCurrency As System.Windows.Forms.TextBox
	Public WithEvents txtLimitAmount As System.Windows.Forms.TextBox
	Public WithEvents lblAuthCurrency As System.Windows.Forms.Label
	Public WithEvents lblLimitAmount As System.Windows.Forms.Label
	Public WithEvents fraAuthLimit As System.Windows.Forms.GroupBox
    Public WithEvents cmbProduct As PMLookupControl.cboPMLookup
    Public WithEvents cboCurrency As UserControls.CurrencyLookup
    Public WithEvents cmbBranch As PMLookupControl.cboPMLookup
	Public WithEvents lblBranch As System.Windows.Forms.Label
	Public WithEvents lblProduct As System.Windows.Forms.Label
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	Public WithEvents fraFilterTrans As System.Windows.Forms.GroupBox
    Public WithEvents dateFrom As System.Windows.Forms.DateTimePicker
    Public WithEvents dateStatementDate As System.Windows.Forms.DateTimePicker
    Public WithEvents dateTo As System.Windows.Forms.DateTimePicker
	Public WithEvents lblTransDateTo As System.Windows.Forms.Label
	Public WithEvents lblTransDateFrom As System.Windows.Forms.Label
	Public WithEvents lblStatementDate As System.Windows.Forms.Label
	Public WithEvents fraMainFilter As System.Windows.Forms.GroupBox
	Public WithEvents _stbStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents stbStatus As System.Windows.Forms.StatusStrip
	Public WithEvents _lvwCommPayments_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwCommPayments_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwCommPayments_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwCommPayments_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwCommPayments_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwCommPayments_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwCommPayments_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwCommPayments As System.Windows.Forms.ListView
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdSave = New System.Windows.Forms.Button()
        Me.cmdPreviewStatement = New System.Windows.Forms.Button()
        Me.cmdNext = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdFindNow = New System.Windows.Forms.Button()
        Me.cmdNewSearch = New System.Windows.Forms.Button()
        Me.fraMainFilter = New System.Windows.Forms.GroupBox()
        Me.CboMediaType = New PMLookupControl.cboPMLookup()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.fraAuthLimit = New System.Windows.Forms.GroupBox()
        Me.chkTransAuthLimit = New System.Windows.Forms.CheckBox()
        Me.txtAuthCurrency = New System.Windows.Forms.TextBox()
        Me.txtLimitAmount = New System.Windows.Forms.TextBox()
        Me.lblAuthCurrency = New System.Windows.Forms.Label()
        Me.lblLimitAmount = New System.Windows.Forms.Label()
        Me.fraFilterTrans = New System.Windows.Forms.GroupBox()
        Me.lblLeadDays = New System.Windows.Forms.Label()
        Me.txtLeadDays = New System.Windows.Forms.TextBox()
        Me.lblAlctdBfr = New System.Windows.Forms.Label()
        Me.chkCommForAllctdTrans = New System.Windows.Forms.CheckBox()
        Me.cmbProduct = New PMLookupControl.cboPMLookup()
        Me.cboCurrency = New UserControls.CurrencyLookup()
        Me.cmbBranch = New PMLookupControl.cboPMLookup()
        Me.lblBranch = New System.Windows.Forms.Label()
        Me.lblProduct = New System.Windows.Forms.Label()
        Me.lblCurrency = New System.Windows.Forms.Label()
        Me.dateFrom = New System.Windows.Forms.DateTimePicker()
        Me.dateStatementDate = New System.Windows.Forms.DateTimePicker()
        Me.dateTo = New System.Windows.Forms.DateTimePicker()
        Me.lblTransDateTo = New System.Windows.Forms.Label()
        Me.lblTransDateFrom = New System.Windows.Forms.Label()
        Me.lblStatementDate = New System.Windows.Forms.Label()
        Me.stbStatus = New System.Windows.Forms.StatusStrip()
        Me._stbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lvwCommPayments = New System.Windows.Forms.ListView()
        Me._lvwCommPayments_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwCommPayments_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwCommPayments_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwCommPayments_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwCommPayments_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwCommPayments_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwCommPayments_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.fraMainFilter.SuspendLayout()
        Me.fraAuthLimit.SuspendLayout()
        Me.fraFilterTrans.SuspendLayout()
        Me.stbStatus.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdSave
        '
        Me.cmdSave.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSave.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSave.Enabled = False
        Me.cmdSave.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSave.Location = New System.Drawing.Point(142, 472)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSave.Size = New System.Drawing.Size(80, 22)
        Me.cmdSave.TabIndex = 12
        Me.cmdSave.Text = "&Save"
        Me.cmdSave.UseVisualStyleBackColor = False
        '
        'cmdPreviewStatement
        '
        Me.cmdPreviewStatement.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPreviewStatement.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPreviewStatement.Enabled = False
        Me.cmdPreviewStatement.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPreviewStatement.Location = New System.Drawing.Point(8, 472)
        Me.cmdPreviewStatement.Name = "cmdPreviewStatement"
        Me.cmdPreviewStatement.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPreviewStatement.Size = New System.Drawing.Size(128, 22)
        Me.cmdPreviewStatement.TabIndex = 11
        Me.cmdPreviewStatement.Text = "&Preview Statement"
        Me.cmdPreviewStatement.UseVisualStyleBackColor = False
        '
        'cmdNext
        '
        Me.cmdNext.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNext.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNext.Enabled = False
        Me.cmdNext.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNext.Location = New System.Drawing.Point(541, 472)
        Me.cmdNext.Name = "cmdNext"
        Me.cmdNext.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNext.Size = New System.Drawing.Size(80, 22)
        Me.cmdNext.TabIndex = 13
        Me.cmdNext.Text = "&Next"
        Me.cmdNext.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(626, 472)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(80, 22)
        Me.cmdCancel.TabIndex = 14
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdFindNow
        '
        Me.cmdFindNow.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindNow.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindNow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindNow.Location = New System.Drawing.Point(634, 17)
        Me.cmdFindNow.Name = "cmdFindNow"
        Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindNow.Size = New System.Drawing.Size(82, 22)
        Me.cmdFindNow.TabIndex = 15
        Me.cmdFindNow.Text = "F&ind Now"
        Me.cmdFindNow.UseVisualStyleBackColor = False
        '
        'cmdNewSearch
        '
        Me.cmdNewSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewSearch.Location = New System.Drawing.Point(634, 46)
        Me.cmdNewSearch.Name = "cmdNewSearch"
        Me.cmdNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewSearch.Size = New System.Drawing.Size(82, 22)
        Me.cmdNewSearch.TabIndex = 16
        Me.cmdNewSearch.Text = "Ne&w Search"
        Me.cmdNewSearch.UseVisualStyleBackColor = False
        '
        'fraMainFilter
        '
        Me.fraMainFilter.BackColor = System.Drawing.SystemColors.Control
        Me.fraMainFilter.Controls.Add(Me.CboMediaType)
        Me.fraMainFilter.Controls.Add(Me.Label1)
        Me.fraMainFilter.Controls.Add(Me.fraAuthLimit)
        Me.fraMainFilter.Controls.Add(Me.fraFilterTrans)
        Me.fraMainFilter.Controls.Add(Me.dateFrom)
        Me.fraMainFilter.Controls.Add(Me.dateStatementDate)
        Me.fraMainFilter.Controls.Add(Me.dateTo)
        Me.fraMainFilter.Controls.Add(Me.lblTransDateTo)
        Me.fraMainFilter.Controls.Add(Me.lblTransDateFrom)
        Me.fraMainFilter.Controls.Add(Me.lblStatementDate)
        Me.fraMainFilter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraMainFilter.Location = New System.Drawing.Point(8, 8)
        Me.fraMainFilter.Name = "fraMainFilter"
        Me.fraMainFilter.Padding = New System.Windows.Forms.Padding(0)
        Me.fraMainFilter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraMainFilter.Size = New System.Drawing.Size(617, 258)
        Me.fraMainFilter.TabIndex = 0
        Me.fraMainFilter.TabStop = False
        '
        'CboMediaType
        '
        Me.CboMediaType.DefaultItemId = 0
        Me.CboMediaType.FirstItem = "(ALL)"
        Me.CboMediaType.ItemId = 0
        Me.CboMediaType.ListIndex = -1
        Me.CboMediaType.Location = New System.Drawing.Point(444, 24)
        Me.CboMediaType.Name = "CboMediaType"
        Me.CboMediaType.PMLookupProductFamily = 1
        Me.CboMediaType.SingleItemId = 0
        Me.CboMediaType.Size = New System.Drawing.Size(158, 21)
        Me.CboMediaType.SortColumnName = ""
        Me.CboMediaType.Sorted = True
        Me.CboMediaType.TabIndex = 26
        Me.CboMediaType.TableName = "MediaType"
        Me.CboMediaType.ToolTipText = ""
        Me.CboMediaType.WhereClause = ""
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(338, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(77, 13)
        Me.Label1.TabIndex = 25
        Me.Label1.Text = "Media Type:"
        '
        'fraAuthLimit
        '
        Me.fraAuthLimit.BackColor = System.Drawing.SystemColors.Control
        Me.fraAuthLimit.Controls.Add(Me.chkTransAuthLimit)
        Me.fraAuthLimit.Controls.Add(Me.txtAuthCurrency)
        Me.fraAuthLimit.Controls.Add(Me.txtLimitAmount)
        Me.fraAuthLimit.Controls.Add(Me.lblAuthCurrency)
        Me.fraAuthLimit.Controls.Add(Me.lblLimitAmount)
        Me.fraAuthLimit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAuthLimit.Location = New System.Drawing.Point(302, 89)
        Me.fraAuthLimit.Name = "fraAuthLimit"
        Me.fraAuthLimit.Padding = New System.Windows.Forms.Padding(0)
        Me.fraAuthLimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAuthLimit.Size = New System.Drawing.Size(307, 106)
        Me.fraAuthLimit.TabIndex = 24
        Me.fraAuthLimit.TabStop = False
        Me.fraAuthLimit.Text = "Authority Limit"
        '
        'chkTransAuthLimit
        '
        Me.chkTransAuthLimit.BackColor = System.Drawing.SystemColors.Control
        Me.chkTransAuthLimit.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkTransAuthLimit.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkTransAuthLimit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkTransAuthLimit.Location = New System.Drawing.Point(6, 75)
        Me.chkTransAuthLimit.Name = "chkTransAuthLimit"
        Me.chkTransAuthLimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkTransAuthLimit.Size = New System.Drawing.Size(294, 17)
        Me.chkTransAuthLimit.TabIndex = 9
        Me.chkTransAuthLimit.Text = "Only display transactions within Authority Limits"
        Me.chkTransAuthLimit.UseVisualStyleBackColor = False
        '
        'txtAuthCurrency
        '
        Me.txtAuthCurrency.AcceptsReturn = True
        Me.txtAuthCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.txtAuthCurrency.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAuthCurrency.Enabled = False
        Me.txtAuthCurrency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAuthCurrency.Location = New System.Drawing.Point(142, 20)
        Me.txtAuthCurrency.MaxLength = 0
        Me.txtAuthCurrency.Name = "txtAuthCurrency"
        Me.txtAuthCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAuthCurrency.Size = New System.Drawing.Size(158, 21)
        Me.txtAuthCurrency.TabIndex = 7
        '
        'txtLimitAmount
        '
        Me.txtLimitAmount.AcceptsReturn = True
        Me.txtLimitAmount.BackColor = System.Drawing.SystemColors.Control
        Me.txtLimitAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLimitAmount.Enabled = False
        Me.txtLimitAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLimitAmount.Location = New System.Drawing.Point(142, 46)
        Me.txtLimitAmount.MaxLength = 0
        Me.txtLimitAmount.Name = "txtLimitAmount"
        Me.txtLimitAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLimitAmount.Size = New System.Drawing.Size(158, 21)
        Me.txtLimitAmount.TabIndex = 8
        Me.txtLimitAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblAuthCurrency
        '
        Me.lblAuthCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblAuthCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAuthCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAuthCurrency.Location = New System.Drawing.Point(6, 24)
        Me.lblAuthCurrency.Name = "lblAuthCurrency"
        Me.lblAuthCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAuthCurrency.Size = New System.Drawing.Size(68, 13)
        Me.lblAuthCurrency.TabIndex = 26
        Me.lblAuthCurrency.Text = "Currency:"
        '
        'lblLimitAmount
        '
        Me.lblLimitAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblLimitAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLimitAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLimitAmount.Location = New System.Drawing.Point(6, 50)
        Me.lblLimitAmount.Name = "lblLimitAmount"
        Me.lblLimitAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLimitAmount.Size = New System.Drawing.Size(98, 13)
        Me.lblLimitAmount.TabIndex = 25
        Me.lblLimitAmount.Text = "Limit Amount:"
        '
        'fraFilterTrans
        '
        Me.fraFilterTrans.BackColor = System.Drawing.SystemColors.Control
        Me.fraFilterTrans.Controls.Add(Me.lblLeadDays)
        Me.fraFilterTrans.Controls.Add(Me.txtLeadDays)
        Me.fraFilterTrans.Controls.Add(Me.lblAlctdBfr)
        Me.fraFilterTrans.Controls.Add(Me.chkCommForAllctdTrans)
        Me.fraFilterTrans.Controls.Add(Me.cmbProduct)
        Me.fraFilterTrans.Controls.Add(Me.cboCurrency)
        Me.fraFilterTrans.Controls.Add(Me.cmbBranch)
        Me.fraFilterTrans.Controls.Add(Me.lblBranch)
        Me.fraFilterTrans.Controls.Add(Me.lblProduct)
        Me.fraFilterTrans.Controls.Add(Me.lblCurrency)
        Me.fraFilterTrans.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFilterTrans.Location = New System.Drawing.Point(14, 89)
        Me.fraFilterTrans.Name = "fraFilterTrans"
        Me.fraFilterTrans.Padding = New System.Windows.Forms.Padding(0)
        Me.fraFilterTrans.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFilterTrans.Size = New System.Drawing.Size(284, 166)
        Me.fraFilterTrans.TabIndex = 20
        Me.fraFilterTrans.TabStop = False
        Me.fraFilterTrans.Text = "Filter Transactions"
        '
        'lblLeadDays
        '
        Me.lblLeadDays.BackColor = System.Drawing.SystemColors.Control
        Me.lblLeadDays.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLeadDays.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLeadDays.Location = New System.Drawing.Point(204, 134)
        Me.lblLeadDays.Name = "lblLeadDays"
        Me.lblLeadDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLeadDays.Size = New System.Drawing.Size(69, 18)
        Me.lblLeadDays.TabIndex = 28
        Me.lblLeadDays.Text = "Lead Days"
        '
        'txtLeadDays
        '
        Me.txtLeadDays.Location = New System.Drawing.Point(136, 131)
        Me.txtLeadDays.Name = "txtLeadDays"
        Me.txtLeadDays.Size = New System.Drawing.Size(62, 21)
        Me.txtLeadDays.TabIndex = 27
        '
        'lblAlctdBfr
        '
        Me.lblAlctdBfr.BackColor = System.Drawing.SystemColors.Control
        Me.lblAlctdBfr.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAlctdBfr.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAlctdBfr.Location = New System.Drawing.Point(15, 134)
        Me.lblAlctdBfr.Name = "lblAlctdBfr"
        Me.lblAlctdBfr.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAlctdBfr.Size = New System.Drawing.Size(105, 18)
        Me.lblAlctdBfr.TabIndex = 26
        Me.lblAlctdBfr.Text = "Allocated before"
        '
        'chkCommForAllctdTrans
        '
        Me.chkCommForAllctdTrans.BackColor = System.Drawing.SystemColors.Control
        Me.chkCommForAllctdTrans.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkCommForAllctdTrans.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCommForAllctdTrans.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCommForAllctdTrans.Location = New System.Drawing.Point(11, 101)
        Me.chkCommForAllctdTrans.Name = "chkCommForAllctdTrans"
        Me.chkCommForAllctdTrans.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCommForAllctdTrans.Size = New System.Drawing.Size(263, 18)
        Me.chkCommForAllctdTrans.TabIndex = 25
        Me.chkCommForAllctdTrans.Text = "Comission for Allocated transactions only"
        Me.chkCommForAllctdTrans.UseVisualStyleBackColor = False
        '
        'cmbProduct
        '
        Me.cmbProduct.DefaultItemId = 0
        Me.cmbProduct.FirstItem = "(ALL)"
        Me.cmbProduct.ItemId = 0
        Me.cmbProduct.ListIndex = -1
        Me.cmbProduct.Location = New System.Drawing.Point(136, 47)
        Me.cmbProduct.Name = "cmbProduct"
        Me.cmbProduct.PMLookupProductFamily = 1
        Me.cmbProduct.SingleItemId = 0
        Me.cmbProduct.Size = New System.Drawing.Size(143, 21)
        Me.cmbProduct.SortColumnName = ""
        Me.cmbProduct.Sorted = True
        Me.cmbProduct.TabIndex = 5
        Me.cmbProduct.TableName = "Product"
        Me.cmbProduct.ToolTipText = ""
        Me.cmbProduct.WhereClause = ""
        '
        'cboCurrency
        '
        Me.cboCurrency.CompanyId = 0
        Me.cboCurrency.CurrencyId = 0
        Me.cboCurrency.DefaultCurrencyId = 0
        Me.cboCurrency.FirstItem = "(ALL)"
        Me.cboCurrency.ListIndex = -1
        Me.cboCurrency.Location = New System.Drawing.Point(136, 20)
        Me.cboCurrency.Name = "cboCurrency"
        Me.cboCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actAllCurrencies
        Me.cboCurrency.Size = New System.Drawing.Size(143, 21)
        Me.cboCurrency.TabIndex = 4
        Me.cboCurrency.ToolTipText = ""
        Me.cboCurrency.WhatsThisHelpID = 0
        '
        'cmbBranch
        '
        Me.cmbBranch.DefaultItemId = 0
        Me.cmbBranch.FirstItem = "(ALL)"
        Me.cmbBranch.ItemId = 0
        Me.cmbBranch.ListIndex = -1
        Me.cmbBranch.Location = New System.Drawing.Point(136, 74)
        Me.cmbBranch.Name = "cmbBranch"
        Me.cmbBranch.PMLookupProductFamily = 1
        Me.cmbBranch.SingleItemId = 0
        Me.cmbBranch.Size = New System.Drawing.Size(143, 21)
        Me.cmbBranch.SortColumnName = ""
        Me.cmbBranch.Sorted = True
        Me.cmbBranch.TabIndex = 6
        Me.cmbBranch.TableName = "sub_branch"
        Me.cmbBranch.ToolTipText = ""
        Me.cmbBranch.WhereClause = ""
        '
        'lblBranch
        '
        Me.lblBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranch.Location = New System.Drawing.Point(13, 75)
        Me.lblBranch.Name = "lblBranch"
        Me.lblBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranch.Size = New System.Drawing.Size(89, 17)
        Me.lblBranch.TabIndex = 23
        Me.lblBranch.Text = "Sub Branch:"
        '
        'lblProduct
        '
        Me.lblProduct.BackColor = System.Drawing.SystemColors.Control
        Me.lblProduct.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProduct.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProduct.Location = New System.Drawing.Point(13, 49)
        Me.lblProduct.Name = "lblProduct"
        Me.lblProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProduct.Size = New System.Drawing.Size(66, 13)
        Me.lblProduct.TabIndex = 22
        Me.lblProduct.Text = "Product:"
        '
        'lblCurrency
        '
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(13, 24)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(66, 13)
        Me.lblCurrency.TabIndex = 21
        Me.lblCurrency.Text = "Currency:"
        '
        'dateFrom
        '
        Me.dateFrom.Checked = False
        Me.dateFrom.Location = New System.Drawing.Point(152, 57)
        Me.dateFrom.Name = "dateFrom"
        Me.dateFrom.ShowCheckBox = True
        Me.dateFrom.Size = New System.Drawing.Size(159, 21)
        Me.dateFrom.TabIndex = 2
        '
        'dateStatementDate
        '
        Me.dateStatementDate.Location = New System.Drawing.Point(152, 24)
        Me.dateStatementDate.Name = "dateStatementDate"
        Me.dateStatementDate.Size = New System.Drawing.Size(159, 21)
        Me.dateStatementDate.TabIndex = 1
        '
        'dateTo
        '
        Me.dateTo.Location = New System.Drawing.Point(444, 57)
        Me.dateTo.Name = "dateTo"
        Me.dateTo.ShowCheckBox = True
        Me.dateTo.Size = New System.Drawing.Size(158, 21)
        Me.dateTo.TabIndex = 3
        '
        'lblTransDateTo
        '
        Me.lblTransDateTo.BackColor = System.Drawing.SystemColors.Control
        Me.lblTransDateTo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTransDateTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTransDateTo.Location = New System.Drawing.Point(338, 58)
        Me.lblTransDateTo.Name = "lblTransDateTo"
        Me.lblTransDateTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTransDateTo.Size = New System.Drawing.Size(49, 13)
        Me.lblTransDateTo.TabIndex = 19
        Me.lblTransDateTo.Text = "To:"
        '
        'lblTransDateFrom
        '
        Me.lblTransDateFrom.BackColor = System.Drawing.SystemColors.Control
        Me.lblTransDateFrom.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTransDateFrom.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTransDateFrom.Location = New System.Drawing.Point(11, 58)
        Me.lblTransDateFrom.Name = "lblTransDateFrom"
        Me.lblTransDateFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTransDateFrom.Size = New System.Drawing.Size(139, 13)
        Me.lblTransDateFrom.TabIndex = 18
        Me.lblTransDateFrom.Text = "Transaction Date From:"
        '
        'lblStatementDate
        '
        Me.lblStatementDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblStatementDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatementDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatementDate.Location = New System.Drawing.Point(11, 28)
        Me.lblStatementDate.Name = "lblStatementDate"
        Me.lblStatementDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatementDate.Size = New System.Drawing.Size(123, 13)
        Me.lblStatementDate.TabIndex = 17
        Me.lblStatementDate.Text = "Statement Date:"
        '
        'stbStatus
        '
        Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._stbStatus_Panel1})
        Me.stbStatus.Location = New System.Drawing.Point(0, 504)
        Me.stbStatus.Name = "stbStatus"
        Me.stbStatus.Size = New System.Drawing.Size(723, 25)
        Me.stbStatus.TabIndex = 27
        '
        '_stbStatus_Panel1
        '
        Me._stbStatus_Panel1.AutoSize = False
        Me._stbStatus_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._stbStatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._stbStatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._stbStatus_Panel1.Name = "_stbStatus_Panel1"
        Me._stbStatus_Panel1.Size = New System.Drawing.Size(96, 25)
        Me._stbStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lvwCommPayments
        '
        Me.lvwCommPayments.BackColor = System.Drawing.SystemColors.Window
        Me.lvwCommPayments.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwCommPayments.CheckBoxes = True
        Me.lvwCommPayments.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwCommPayments_ColumnHeader_1, Me._lvwCommPayments_ColumnHeader_2, Me._lvwCommPayments_ColumnHeader_3, Me._lvwCommPayments_ColumnHeader_4, Me._lvwCommPayments_ColumnHeader_5, Me._lvwCommPayments_ColumnHeader_6, Me._lvwCommPayments_ColumnHeader_7})
        Me.lvwCommPayments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwCommPayments.FullRowSelect = True
        Me.lvwCommPayments.HideSelection = False
        Me.lvwCommPayments.Location = New System.Drawing.Point(8, 272)
        Me.lvwCommPayments.Name = "lvwCommPayments"
        Me.lvwCommPayments.Size = New System.Drawing.Size(697, 193)
        Me.lvwCommPayments.TabIndex = 10
        Me.lvwCommPayments.TabStop = False
        Me.lvwCommPayments.UseCompatibleStateImageBehavior = False
        Me.lvwCommPayments.View = System.Windows.Forms.View.Details
        '
        '_lvwCommPayments_ColumnHeader_1
        '
        Me._lvwCommPayments_ColumnHeader_1.Width = 0
        '
        '_lvwCommPayments_ColumnHeader_2
        '
        Me._lvwCommPayments_ColumnHeader_2.Width = 170
        '
        '_lvwCommPayments_ColumnHeader_3
        '
        Me._lvwCommPayments_ColumnHeader_3.Width = 170
        '
        '_lvwCommPayments_ColumnHeader_4
        '
        Me._lvwCommPayments_ColumnHeader_4.Width = 170
        '
        '_lvwCommPayments_ColumnHeader_5
        '
        Me._lvwCommPayments_ColumnHeader_5.Width = 170
        '
        '_lvwCommPayments_ColumnHeader_6
        '
        Me._lvwCommPayments_ColumnHeader_6.Width = 170
        '
        '_lvwCommPayments_ColumnHeader_7
        '
        Me._lvwCommPayments_ColumnHeader_7.Width = 170
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdFindNow
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(723, 529)
        Me.Controls.Add(Me.cmdSave)
        Me.Controls.Add(Me.cmdPreviewStatement)
        Me.Controls.Add(Me.cmdNext)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdFindNow)
        Me.Controls.Add(Me.cmdNewSearch)
        Me.Controls.Add(Me.fraMainFilter)
        Me.Controls.Add(Me.stbStatus)
        Me.Controls.Add(Me.lvwCommPayments)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 30)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Commission Payment - Agent Summary"
        Me.fraMainFilter.ResumeLayout(False)
        Me.fraMainFilter.PerformLayout()
        Me.fraAuthLimit.ResumeLayout(False)
        Me.fraAuthLimit.PerformLayout()
        Me.fraFilterTrans.ResumeLayout(False)
        Me.fraFilterTrans.PerformLayout()
        Me.stbStatus.ResumeLayout(False)
        Me.stbStatus.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CboMediaType As PMLookupControl.cboPMLookup
    Friend WithEvents txtLeadDays As System.Windows.Forms.TextBox
    Public WithEvents lblAlctdBfr As System.Windows.Forms.Label
    Public WithEvents chkCommForAllctdTrans As System.Windows.Forms.CheckBox
    Public WithEvents lblLeadDays As System.Windows.Forms.Label
#End Region 
End Class