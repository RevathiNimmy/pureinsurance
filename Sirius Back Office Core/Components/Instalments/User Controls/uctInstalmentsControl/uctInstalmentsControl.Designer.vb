Partial Class uctInstalments
    Inherits System.Windows.Forms.UserControl
#Region "Windows Form Designer generated code "
    'devloper guide no.211
    'Friend Sub New()
    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        InitializeoptMTAType()
        ssTabMainPreviousTab = ssTabMain.SelectedIndex
        UserControl_Initialize()
        InitializeoptDepositType()
    End Sub
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
        If Disposing Then
            UserControl_Terminate()
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(Disposing)
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Image1 As System.Windows.Forms.PictureBox
    Friend WithEvents picProcessing As System.Windows.Forms.PictureBox
    Friend WithEvents txtInvisible As System.Windows.Forms.TextBox
    Friend WithEvents txtInstalments As System.Windows.Forms.TextBox
    Friend WithEvents txtApr As System.Windows.Forms.TextBox
    Friend WithEvents txtTotalPayable As System.Windows.Forms.TextBox
    Friend WithEvents txtTransactions As System.Windows.Forms.TextBox
    Friend WithEvents txtRate As System.Windows.Forms.TextBox
    Friend WithEvents txtFinancedAmount As System.Windows.Forms.TextBox
    Friend WithEvents lblApr As System.Windows.Forms.Label
    Friend WithEvents lblInstalments As System.Windows.Forms.Label
    Friend WithEvents lblTotalPayable As System.Windows.Forms.Label
    Friend WithEvents lblRate As System.Windows.Forms.Label
    Friend WithEvents lblTransactions As System.Windows.Forms.Label
    Friend WithEvents lblFinancedAmount As System.Windows.Forms.Label
    Friend WithEvents fraSummary As System.Windows.Forms.GroupBox
    Friend WithEvents _ssTabMain_TabPage0 As System.Windows.Forms.TabPage
    Friend WithEvents lblSummary As System.Windows.Forms.Label
    Friend WithEvents txtLastInstalDate As System.Windows.Forms.TextBox
    Friend WithEvents txtTaxes As System.Windows.Forms.TextBox
    Friend WithEvents txtNextInstalDate As System.Windows.Forms.TextBox
    Friend WithEvents txtOtherInstalmentAmount As System.Windows.Forms.TextBox
    Friend WithEvents txtDeposit As System.Windows.Forms.TextBox
    Friend WithEvents txtFirstInstalDate As System.Windows.Forms.TextBox
    Friend WithEvents txtProtection As System.Windows.Forms.TextBox
    Friend WithEvents txtAdminCharge As System.Windows.Forms.TextBox
    Friend WithEvents txtFirstInstalmentAmount As System.Windows.Forms.TextBox
    Friend WithEvents txtInterest As System.Windows.Forms.TextBox
    Friend WithEvents Line1 As System.Windows.Forms.Label
    Friend WithEvents lblLastInstDate As System.Windows.Forms.Label
    Friend WithEvents lblTaxes As System.Windows.Forms.Label
    Friend WithEvents lblNextInstDate As System.Windows.Forms.Label
    Friend WithEvents lblOtherInstsl As System.Windows.Forms.Label
    Friend WithEvents lblDeposit As System.Windows.Forms.Label
    Friend WithEvents lblProtectionCharge As System.Windows.Forms.Label
    Friend WithEvents lblFirstInstDate As System.Windows.Forms.Label
    Friend WithEvents lblAdminCharge As System.Windows.Forms.Label
    Friend WithEvents lblInterest As System.Windows.Forms.Label
    Friend WithEvents lblFirstInstal As System.Windows.Forms.Label
    Friend WithEvents fraBreakdown As System.Windows.Forms.GroupBox
    Friend WithEvents _ssTabMain_TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents chkOverrideInterestRate As System.Windows.Forms.CheckBox
    Friend WithEvents txtNewRate As System.Windows.Forms.TextBox
    Friend WithEvents chkCommissionOverride As System.Windows.Forms.CheckBox
    Friend WithEvents txtOverrideReference As System.Windows.Forms.TextBox
    Friend WithEvents chkDepositOverride As System.Windows.Forms.CheckBox
    Friend WithEvents txtOverrideDeposit As System.Windows.Forms.TextBox
    Friend WithEvents lblNewInterestRate As System.Windows.Forms.Label
    Friend WithEvents lblReference As System.Windows.Forms.Label
    Friend WithEvents lblDepositOverride As System.Windows.Forms.Label
    Friend WithEvents fraOverrideOptions As System.Windows.Forms.GroupBox
    Friend WithEvents chkPaymentProtection As System.Windows.Forms.CheckBox
    Friend WithEvents fraAdditionalOptions As System.Windows.Forms.GroupBox
    Friend WithEvents _ssTabMain_TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents txtMinDeposit As System.Windows.Forms.TextBox
    Friend WithEvents txtTaxDeposit As System.Windows.Forms.TextBox
    Friend WithEvents txtFeeDeposit As System.Windows.Forms.TextBox
    Friend WithEvents lblMinDeposit As System.Windows.Forms.Label
    Friend WithEvents lblTaxDeposit As System.Windows.Forms.Label
    Friend WithEvents lblFeeDeposit As System.Windows.Forms.Label
    Friend WithEvents fraMinDeposit As System.Windows.Forms.GroupBox
    Friend WithEvents txtAmountFinanced As System.Windows.Forms.TextBox
    Friend WithEvents txtTotalTax As System.Windows.Forms.TextBox
    Friend WithEvents txtTotalFee As System.Windows.Forms.TextBox
    Friend WithEvents txtGross As System.Windows.Forms.TextBox
    Friend WithEvents lblTotalFinanced As System.Windows.Forms.Label
    Friend WithEvents lblTaxExcluded As System.Windows.Forms.Label
    Friend WithEvents lblFeeExcluded As System.Windows.Forms.Label
    Friend WithEvents lblGrossDue As System.Windows.Forms.Label
    Friend WithEvents fraFinanceDetails As System.Windows.Forms.GroupBox
    Friend WithEvents _ssTabMain_TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents ssTabMain As System.Windows.Forms.TabControl
    Friend WithEvents lblMonthDay As System.Windows.Forms.Label
    Friend WithEvents lblPreferredFirstDate As System.Windows.Forms.Label
    Friend WithEvents lblWeekday As System.Windows.Forms.Label
    Friend WithEvents cboPreferredDate As System.Windows.Forms.ComboBox
    Friend WithEvents cboMonthDay As System.Windows.Forms.ComboBox
    Friend WithEvents cboWeekDay As System.Windows.Forms.ComboBox
    Friend WithEvents _ssTabOptions_TabPage0 As System.Windows.Forms.TabPage
    Friend WithEvents _optMTAType_2 As System.Windows.Forms.RadioButton
    Friend WithEvents _optMTAType_1 As System.Windows.Forms.RadioButton
    Friend WithEvents _optMTAType_0 As System.Windows.Forms.RadioButton
    Friend WithEvents _ssTabOptions_TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents lblExistingBankAccDtls As System.Windows.Forms.Label
    Friend WithEvents lblExistingCreditCardDtls As System.Windows.Forms.Label
    Friend WithEvents cboExistingBankAccDtls As System.Windows.Forms.ComboBox
    Friend WithEvents cboExistingCreditCardDtls As System.Windows.Forms.ComboBox
    Friend WithEvents _ssTabOptions_TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents ssTabOptions As System.Windows.Forms.TabControl
    Friend optMTAType(2) As System.Windows.Forms.RadioButton
    Private WithEvents tdgListInstalment As Artinsoft.Windows.Forms.ExtendedDataGridView
    Private WithEvents tdgInstalment As Artinsoft.Windows.Forms.ExtendedDataGridView
    Private ssTabMainPreviousTab As Integer
    Public optDepositType(1) As System.Windows.Forms.RadioButton
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctInstalments))
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.picProcessing = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Image1 = New System.Windows.Forms.PictureBox()
        Me.txtInvisible = New System.Windows.Forms.TextBox()
        Me.ssTabMain = New System.Windows.Forms.TabControl()
        Me._ssTabMain_TabPage0 = New System.Windows.Forms.TabPage()
        Me.tdgListInstalment = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me.tdgInstalment = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me.colScheme = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colPayment = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colMedia = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colDeposit = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colAmt = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colFunding = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.fraSummary = New System.Windows.Forms.GroupBox()
        Me.txtInstalments = New System.Windows.Forms.TextBox()
        Me.txtApr = New System.Windows.Forms.TextBox()
        Me.txtTotalPayable = New System.Windows.Forms.TextBox()
        Me.txtTransactions = New System.Windows.Forms.TextBox()
        Me.txtRate = New System.Windows.Forms.TextBox()
        Me.txtFinancedAmount = New System.Windows.Forms.TextBox()
        Me.lblApr = New System.Windows.Forms.Label()
        Me.lblInstalments = New System.Windows.Forms.Label()
        Me.lblTotalPayable = New System.Windows.Forms.Label()
        Me.lblRate = New System.Windows.Forms.Label()
        Me.lblTransactions = New System.Windows.Forms.Label()
        Me.lblFinancedAmount = New System.Windows.Forms.Label()
        Me._ssTabMain_TabPage1 = New System.Windows.Forms.TabPage()
        Me.fraBreakdown = New System.Windows.Forms.GroupBox()
        Me.txtLastInstalDate = New System.Windows.Forms.TextBox()
        Me.txtTaxes = New System.Windows.Forms.TextBox()
        Me.txtNextInstalDate = New System.Windows.Forms.TextBox()
        Me.txtOtherInstalmentAmount = New System.Windows.Forms.TextBox()
        Me.txtDeposit = New System.Windows.Forms.TextBox()
        Me.txtFirstInstalDate = New System.Windows.Forms.TextBox()
        Me.txtProtection = New System.Windows.Forms.TextBox()
        Me.txtAdminCharge = New System.Windows.Forms.TextBox()
        Me.txtFirstInstalmentAmount = New System.Windows.Forms.TextBox()
        Me.txtInterest = New System.Windows.Forms.TextBox()
        Me.Line1 = New System.Windows.Forms.Label()
        Me.lblLastInstDate = New System.Windows.Forms.Label()
        Me.lblTaxes = New System.Windows.Forms.Label()
        Me.lblNextInstDate = New System.Windows.Forms.Label()
        Me.lblOtherInstsl = New System.Windows.Forms.Label()
        Me.lblDeposit = New System.Windows.Forms.Label()
        Me.lblProtectionCharge = New System.Windows.Forms.Label()
        Me.lblFirstInstDate = New System.Windows.Forms.Label()
        Me.lblAdminCharge = New System.Windows.Forms.Label()
        Me.lblInterest = New System.Windows.Forms.Label()
        Me.lblFirstInstal = New System.Windows.Forms.Label()
        Me.lblSummary = New System.Windows.Forms.Label()
        Me._ssTabMain_TabPage2 = New System.Windows.Forms.TabPage()
        Me.fraAdditionalOptions = New System.Windows.Forms.GroupBox()
        Me.chkUseTransCurrency = New System.Windows.Forms.CheckBox()
        Me.chkPaymentProtection = New System.Windows.Forms.CheckBox()
        Me.fraOverrideOptions = New System.Windows.Forms.GroupBox()
        Me._optDepositOverride_1 = New System.Windows.Forms.RadioButton()
        Me._optDepositOverride_0 = New System.Windows.Forms.RadioButton()
        Me.chkOverrideInterestRate = New System.Windows.Forms.CheckBox()
        Me.txtNewRate = New System.Windows.Forms.TextBox()
        Me.chkCommissionOverride = New System.Windows.Forms.CheckBox()
        Me.txtOverrideReference = New System.Windows.Forms.TextBox()
        Me.chkDepositOverride = New System.Windows.Forms.CheckBox()
        Me.txtOverrideDeposit = New System.Windows.Forms.TextBox()
        Me.lblNewInterestRate = New System.Windows.Forms.Label()
        Me.lblReference = New System.Windows.Forms.Label()
        Me.lblDepositOverride = New System.Windows.Forms.Label()
        Me._ssTabMain_TabPage3 = New System.Windows.Forms.TabPage()
        Me.fraFinanceDetails = New System.Windows.Forms.GroupBox()
        Me.txtAmountFinanced = New System.Windows.Forms.TextBox()
        Me.txtTotalTax = New System.Windows.Forms.TextBox()
        Me.txtTotalFee = New System.Windows.Forms.TextBox()
        Me.txtGross = New System.Windows.Forms.TextBox()
        Me.lblTotalFinanced = New System.Windows.Forms.Label()
        Me.lblTaxExcluded = New System.Windows.Forms.Label()
        Me.lblFeeExcluded = New System.Windows.Forms.Label()
        Me.lblGrossDue = New System.Windows.Forms.Label()
        Me.fraMinDeposit = New System.Windows.Forms.GroupBox()
        Me.txtMinDeposit = New System.Windows.Forms.TextBox()
        Me.txtTaxDeposit = New System.Windows.Forms.TextBox()
        Me.txtFeeDeposit = New System.Windows.Forms.TextBox()
        Me.lblMinDeposit = New System.Windows.Forms.Label()
        Me.lblTaxDeposit = New System.Windows.Forms.Label()
        Me.lblFeeDeposit = New System.Windows.Forms.Label()
        Me.ssTabOptions = New System.Windows.Forms.TabControl()
        Me._ssTabOptions_TabPage0 = New System.Windows.Forms.TabPage()
        Me.lblMonthDay = New System.Windows.Forms.Label()
        Me.lblPreferredFirstDate = New System.Windows.Forms.Label()
        Me.lblWeekday = New System.Windows.Forms.Label()
        Me.cboPreferredDate = New System.Windows.Forms.ComboBox()
        Me.cboMonthDay = New System.Windows.Forms.ComboBox()
        Me.cboWeekDay = New System.Windows.Forms.ComboBox()
        Me._ssTabOptions_TabPage1 = New System.Windows.Forms.TabPage()
        Me._optMTAType_2 = New System.Windows.Forms.RadioButton()
        Me._optMTAType_1 = New System.Windows.Forms.RadioButton()
        Me._optMTAType_0 = New System.Windows.Forms.RadioButton()
        Me._ssTabOptions_TabPage2 = New System.Windows.Forms.TabPage()
        Me.cboExistingCreditCardDtls = New System.Windows.Forms.ComboBox()
        Me.cboExistingBankAccDtls = New System.Windows.Forms.ComboBox()
        Me.lblExistingCreditCardDtls = New System.Windows.Forms.Label()
        Me.lblExistingBankAccDtls = New System.Windows.Forms.Label()
        Me.shpProcessing = New Artinsoft.VB6.Gui.ShapeHelper()
        CType(Me.picProcessing, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.picProcessing.SuspendLayout()
        CType(Me.Image1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ssTabMain.SuspendLayout()
        Me._ssTabMain_TabPage0.SuspendLayout()
        CType(Me.tdgListInstalment, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tdgInstalment, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraSummary.SuspendLayout()
        Me._ssTabMain_TabPage1.SuspendLayout()
        Me.fraBreakdown.SuspendLayout()
        Me._ssTabMain_TabPage2.SuspendLayout()
        Me.fraAdditionalOptions.SuspendLayout()
        Me.fraOverrideOptions.SuspendLayout()
        Me._ssTabMain_TabPage3.SuspendLayout()
        Me.fraFinanceDetails.SuspendLayout()
        Me.fraMinDeposit.SuspendLayout()
        Me.ssTabOptions.SuspendLayout()
        Me._ssTabOptions_TabPage0.SuspendLayout()
        Me._ssTabOptions_TabPage1.SuspendLayout()
        Me._ssTabOptions_TabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'picProcessing
        '
        Me.picProcessing.BackColor = System.Drawing.SystemColors.Control
        Me.picProcessing.Controls.Add(Me.Label1)
        Me.picProcessing.Controls.Add(Me.Image1)
        Me.picProcessing.Cursor = System.Windows.Forms.Cursors.Default
        Me.picProcessing.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.picProcessing.Location = New System.Drawing.Point(219, 38)
        Me.picProcessing.Name = "picProcessing"
        Me.picProcessing.Size = New System.Drawing.Size(249, 33)
        Me.picProcessing.TabIndex = 53
        Me.picProcessing.TabStop = False
        Me.picProcessing.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(40, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(147, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Processing Quotes.."
        '
        'Image1
        '
        Me.Image1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Image1.Image = CType(resources.GetObject("Image1.Image"), System.Drawing.Image)
        Me.Image1.Location = New System.Drawing.Point(16, 8)
        Me.Image1.Name = "Image1"
        Me.Image1.Size = New System.Drawing.Size(16, 16)
        Me.Image1.TabIndex = 55
        Me.Image1.TabStop = False
        '
        'txtInvisible
        '
        Me.txtInvisible.AcceptsReturn = True
        Me.txtInvisible.BackColor = System.Drawing.SystemColors.Window
        Me.txtInvisible.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInvisible.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInvisible.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInvisible.Location = New System.Drawing.Point(242, 152)
        Me.txtInvisible.MaxLength = 0
        Me.txtInvisible.Name = "txtInvisible"
        Me.txtInvisible.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInvisible.Size = New System.Drawing.Size(81, 20)
        Me.txtInvisible.TabIndex = 1
        Me.txtInvisible.Tag = "F;FMT;$;"
        Me.txtInvisible.Text = "Text1"
        Me.txtInvisible.Visible = False
        '
        'ssTabMain
        '
        Me.ssTabMain.Controls.Add(Me._ssTabMain_TabPage0)
        Me.ssTabMain.Controls.Add(Me._ssTabMain_TabPage1)
        Me.ssTabMain.Controls.Add(Me._ssTabMain_TabPage2)
        Me.ssTabMain.Controls.Add(Me._ssTabMain_TabPage3)
        Me.ssTabMain.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ssTabMain.ItemSize = New System.Drawing.Size(139, 18)
        Me.ssTabMain.Location = New System.Drawing.Point(8, 79)
        Me.ssTabMain.Multiline = True
        Me.ssTabMain.Name = "ssTabMain"
        Me.ssTabMain.SelectedIndex = 0
        Me.ssTabMain.Size = New System.Drawing.Size(711, 253)
        Me.ssTabMain.TabIndex = 1
        Me.ssTabMain.Tag = "CAP;101"
        '
        '_ssTabMain_TabPage0
        '
        Me._ssTabMain_TabPage0.Controls.Add(Me.picProcessing)
        Me._ssTabMain_TabPage0.Controls.Add(Me.tdgListInstalment)
        Me._ssTabMain_TabPage0.Controls.Add(Me.tdgInstalment)
        Me._ssTabMain_TabPage0.Controls.Add(Me.fraSummary)
        Me._ssTabMain_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._ssTabMain_TabPage0.Name = "_ssTabMain_TabPage0"
        Me._ssTabMain_TabPage0.Size = New System.Drawing.Size(703, 227)
        Me._ssTabMain_TabPage0.TabIndex = 0
        Me._ssTabMain_TabPage0.Text = "1 - Plans Available"
        Me._ssTabMain_TabPage0.UseVisualStyleBackColor = True
        '
        'tdgListInstalment
        '
        Me.tdgListInstalment.AllowBigSelection = False
        Me.tdgListInstalment.AllowRowSelection = False
        Me.tdgListInstalment.AllowUserToAddRows = False
        Me.tdgListInstalment.AlternatingRows = False
        Me.tdgListInstalment.BackColorFixed = System.Drawing.Color.Empty
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.tdgListInstalment.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle9
        Me.tdgListInstalment.ColumnHeadersHeight = 18
        Me.tdgListInstalment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.tdgListInstalment.ColumnsCount = 0
        Me.tdgListInstalment.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me.tdgListInstalment.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.tdgListInstalment.EvenStyle = DataGridViewCellStyle10
        Me.tdgListInstalment.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me.tdgListInstalment.FixedColumns = -1
        Me.tdgListInstalment.FixedRows = -1
        Me.tdgListInstalment.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me.tdgListInstalment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tdgListInstalment.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me.tdgListInstalment.GridLineWidth = 0
        Me.tdgListInstalment.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me.tdgListInstalment.Location = New System.Drawing.Point(8, 12)
        Me.tdgListInstalment.MultiSelect = False
        Me.tdgListInstalment.Name = "tdgListInstalment"
        Me.tdgListInstalment.OddStyle = DataGridViewCellStyle11
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.tdgListInstalment.RowHeadersDefaultCellStyle = DataGridViewCellStyle12
        Me.tdgListInstalment.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.tdgListInstalment.RowHeightMin = 0
        Me.tdgListInstalment.RowsCount = 0
        Me.tdgListInstalment.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me.tdgListInstalment.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.tdgListInstalment.SelectedStyle = Nothing
        Me.tdgListInstalment.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.tdgListInstalment.SelLength = -1
        Me.tdgListInstalment.SelStart = -1
        Me.tdgListInstalment.Size = New System.Drawing.Size(679, 113)
        Me.tdgListInstalment.TabIndex = 0
        Me.tdgListInstalment.Tag = "F;FMT;$;"
        Me.tdgListInstalment.ToolTipText = ""
        Me.tdgListInstalment.Visible = False
        '
        'tdgInstalment
        '
        Me.tdgInstalment.AllowBigSelection = False
        Me.tdgInstalment.AllowRowSelection = False
        Me.tdgInstalment.AlternatingRows = False
        Me.tdgInstalment.BackColorFixed = System.Drawing.Color.Empty
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.tdgInstalment.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle13
        Me.tdgInstalment.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colScheme, Me.colPayment, Me.colMedia, Me.colDeposit, Me.colAmt, Me.colType, Me.colFunding})
        Me.tdgInstalment.ColumnsCount = 7
        Me.tdgInstalment.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me.tdgInstalment.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.tdgInstalment.EvenStyle = DataGridViewCellStyle14
        Me.tdgInstalment.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me.tdgInstalment.FixedColumns = -1
        Me.tdgInstalment.FixedRows = -1
        Me.tdgInstalment.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me.tdgInstalment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tdgInstalment.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me.tdgInstalment.GridLineWidth = 0
        Me.tdgInstalment.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me.tdgInstalment.Location = New System.Drawing.Point(8, 12)
        Me.tdgInstalment.Name = "tdgInstalment"
        Me.tdgInstalment.OddStyle = DataGridViewCellStyle15
        DataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.tdgInstalment.RowHeadersDefaultCellStyle = DataGridViewCellStyle16
        Me.tdgInstalment.RowHeightMin = 3
        Me.tdgInstalment.RowsCount = 1
        Me.tdgInstalment.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me.tdgInstalment.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.tdgInstalment.SelectedStyle = Nothing
        Me.tdgInstalment.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.tdgInstalment.SelLength = -1
        Me.tdgInstalment.SelStart = -1
        Me.tdgInstalment.Size = New System.Drawing.Size(679, 121)
        Me.tdgInstalment.TabIndex = 51
        Me.tdgInstalment.Tag = "F;FMT;$;"
        Me.tdgInstalment.ToolTipText = ""
        Me.tdgInstalment.Visible = False
        '
        'colScheme
        '
        Me.colScheme.HeaderText = "Scheme"
        Me.colScheme.Name = "colScheme"
        '
        'colPayment
        '
        Me.colPayment.HeaderText = "Payment"
        Me.colPayment.Name = "colPayment"
        '
        'colMedia
        '
        Me.colMedia.HeaderText = "Media Type"
        Me.colMedia.Name = "colMedia"
        '
        'colDeposit
        '
        Me.colDeposit.HeaderText = "Deposit"
        Me.colDeposit.Name = "colDeposit"
        '
        'colAmt
        '
        Me.colAmt.HeaderText = "Amount"
        Me.colAmt.Name = "colAmt"
        '
        'colType
        '
        Me.colType.HeaderText = "Type"
        Me.colType.Name = "colType"
        '
        'colFunding
        '
        Me.colFunding.HeaderText = "Funding"
        Me.colFunding.Name = "colFunding"
        '
        'fraSummary
        '
        Me.fraSummary.BackColor = System.Drawing.SystemColors.Control
        Me.fraSummary.Controls.Add(Me.txtInstalments)
        Me.fraSummary.Controls.Add(Me.txtApr)
        Me.fraSummary.Controls.Add(Me.txtTotalPayable)
        Me.fraSummary.Controls.Add(Me.txtTransactions)
        Me.fraSummary.Controls.Add(Me.txtRate)
        Me.fraSummary.Controls.Add(Me.txtFinancedAmount)
        Me.fraSummary.Controls.Add(Me.lblApr)
        Me.fraSummary.Controls.Add(Me.lblInstalments)
        Me.fraSummary.Controls.Add(Me.lblTotalPayable)
        Me.fraSummary.Controls.Add(Me.lblRate)
        Me.fraSummary.Controls.Add(Me.lblTransactions)
        Me.fraSummary.Controls.Add(Me.lblFinancedAmount)
        Me.fraSummary.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraSummary.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraSummary.Location = New System.Drawing.Point(8, 132)
        Me.fraSummary.Name = "fraSummary"
        Me.fraSummary.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraSummary.Size = New System.Drawing.Size(679, 89)
        Me.fraSummary.TabIndex = 1
        Me.fraSummary.TabStop = False
        Me.fraSummary.Tag = "CAP;304"
        Me.fraSummary.Text = "Summary"
        '
        'txtInstalments
        '
        Me.txtInstalments.AcceptsReturn = True
        Me.txtInstalments.BackColor = System.Drawing.SystemColors.Window
        Me.txtInstalments.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInstalments.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInstalments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInstalments.Location = New System.Drawing.Point(400, 39)
        Me.txtInstalments.MaxLength = 0
        Me.txtInstalments.Name = "txtInstalments"
        Me.txtInstalments.ReadOnly = True
        Me.txtInstalments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInstalments.Size = New System.Drawing.Size(129, 21)
        Me.txtInstalments.TabIndex = 9
        Me.txtInstalments.Tag = "F;"
        '
        'txtApr
        '
        Me.txtApr.AcceptsReturn = True
        Me.txtApr.BackColor = System.Drawing.SystemColors.Window
        Me.txtApr.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtApr.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtApr.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtApr.Location = New System.Drawing.Point(400, 61)
        Me.txtApr.MaxLength = 0
        Me.txtApr.Name = "txtApr"
        Me.txtApr.ReadOnly = True
        Me.txtApr.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtApr.Size = New System.Drawing.Size(129, 21)
        Me.txtApr.TabIndex = 11
        Me.txtApr.Tag = "F;"
        '
        'txtTotalPayable
        '
        Me.txtTotalPayable.AcceptsReturn = True
        Me.txtTotalPayable.BackColor = System.Drawing.SystemColors.Window
        Me.txtTotalPayable.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalPayable.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalPayable.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalPayable.Location = New System.Drawing.Point(400, 16)
        Me.txtTotalPayable.MaxLength = 0
        Me.txtTotalPayable.Name = "txtTotalPayable"
        Me.txtTotalPayable.ReadOnly = True
        Me.txtTotalPayable.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalPayable.Size = New System.Drawing.Size(129, 21)
        Me.txtTotalPayable.TabIndex = 7
        Me.txtTotalPayable.Tag = "F;FMT;$;"
        '
        'txtTransactions
        '
        Me.txtTransactions.AcceptsReturn = True
        Me.txtTransactions.BackColor = System.Drawing.SystemColors.Window
        Me.txtTransactions.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTransactions.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTransactions.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTransactions.Location = New System.Drawing.Point(136, 40)
        Me.txtTransactions.MaxLength = 0
        Me.txtTransactions.Name = "txtTransactions"
        Me.txtTransactions.ReadOnly = True
        Me.txtTransactions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTransactions.Size = New System.Drawing.Size(129, 21)
        Me.txtTransactions.TabIndex = 3
        Me.txtTransactions.Tag = "F;"
        '
        'txtRate
        '
        Me.txtRate.AcceptsReturn = True
        Me.txtRate.BackColor = System.Drawing.SystemColors.Window
        Me.txtRate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRate.Location = New System.Drawing.Point(136, 63)
        Me.txtRate.MaxLength = 0
        Me.txtRate.Name = "txtRate"
        Me.txtRate.ReadOnly = True
        Me.txtRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRate.Size = New System.Drawing.Size(129, 21)
        Me.txtRate.TabIndex = 5
        Me.txtRate.Tag = "F;"
        '
        'txtFinancedAmount
        '
        Me.txtFinancedAmount.AcceptsReturn = True
        Me.txtFinancedAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtFinancedAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFinancedAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFinancedAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFinancedAmount.Location = New System.Drawing.Point(136, 18)
        Me.txtFinancedAmount.MaxLength = 0
        Me.txtFinancedAmount.Name = "txtFinancedAmount"
        Me.txtFinancedAmount.ReadOnly = True
        Me.txtFinancedAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFinancedAmount.Size = New System.Drawing.Size(129, 21)
        Me.txtFinancedAmount.TabIndex = 1
        Me.txtFinancedAmount.Tag = "F;FMT;$;"
        '
        'lblApr
        '
        Me.lblApr.BackColor = System.Drawing.SystemColors.Control
        Me.lblApr.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblApr.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblApr.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblApr.Location = New System.Drawing.Point(288, 66)
        Me.lblApr.Name = "lblApr"
        Me.lblApr.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblApr.Size = New System.Drawing.Size(113, 17)
        Me.lblApr.TabIndex = 10
        Me.lblApr.Tag = "CAP;310"
        Me.lblApr.Text = "APR:"
        '
        'lblInstalments
        '
        Me.lblInstalments.BackColor = System.Drawing.SystemColors.Control
        Me.lblInstalments.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInstalments.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstalments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInstalments.Location = New System.Drawing.Point(288, 43)
        Me.lblInstalments.Name = "lblInstalments"
        Me.lblInstalments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInstalments.Size = New System.Drawing.Size(113, 17)
        Me.lblInstalments.TabIndex = 8
        Me.lblInstalments.Tag = "CAP;309"
        Me.lblInstalments.Text = "Instalments"
        '
        'lblTotalPayable
        '
        Me.lblTotalPayable.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalPayable.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalPayable.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalPayable.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalPayable.Location = New System.Drawing.Point(288, 20)
        Me.lblTotalPayable.Name = "lblTotalPayable"
        Me.lblTotalPayable.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalPayable.Size = New System.Drawing.Size(113, 17)
        Me.lblTotalPayable.TabIndex = 6
        Me.lblTotalPayable.Tag = "CAP;308"
        Me.lblTotalPayable.Text = "Total Payable"
        '
        'lblRate
        '
        Me.lblRate.BackColor = System.Drawing.SystemColors.Control
        Me.lblRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRate.Location = New System.Drawing.Point(11, 67)
        Me.lblRate.Name = "lblRate"
        Me.lblRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRate.Size = New System.Drawing.Size(124, 17)
        Me.lblRate.TabIndex = 4
        Me.lblRate.Tag = "CAP;307"
        Me.lblRate.Text = "Intereset Rate(%):"
        '
        'lblTransactions
        '
        Me.lblTransactions.BackColor = System.Drawing.SystemColors.Control
        Me.lblTransactions.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTransactions.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTransactions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTransactions.Location = New System.Drawing.Point(11, 44)
        Me.lblTransactions.Name = "lblTransactions"
        Me.lblTransactions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTransactions.Size = New System.Drawing.Size(113, 17)
        Me.lblTransactions.TabIndex = 2
        Me.lblTransactions.Tag = "CAP;306"
        Me.lblTransactions.Text = "Transactions"
        '
        'lblFinancedAmount
        '
        Me.lblFinancedAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblFinancedAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFinancedAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFinancedAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFinancedAmount.Location = New System.Drawing.Point(11, 21)
        Me.lblFinancedAmount.Name = "lblFinancedAmount"
        Me.lblFinancedAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFinancedAmount.Size = New System.Drawing.Size(113, 17)
        Me.lblFinancedAmount.TabIndex = 0
        Me.lblFinancedAmount.Tag = "CAP;305"
        Me.lblFinancedAmount.Text = "Financed Amount"
        '
        '_ssTabMain_TabPage1
        '
        Me._ssTabMain_TabPage1.Controls.Add(Me.fraBreakdown)
        Me._ssTabMain_TabPage1.Controls.Add(Me.lblSummary)
        Me._ssTabMain_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._ssTabMain_TabPage1.Name = "_ssTabMain_TabPage1"
        Me._ssTabMain_TabPage1.Size = New System.Drawing.Size(703, 227)
        Me._ssTabMain_TabPage1.TabIndex = 1
        Me._ssTabMain_TabPage1.Text = "2 - Details"
        Me._ssTabMain_TabPage1.UseVisualStyleBackColor = True
        '
        'fraBreakdown
        '
        Me.fraBreakdown.BackColor = System.Drawing.SystemColors.Control
        Me.fraBreakdown.Controls.Add(Me.txtLastInstalDate)
        Me.fraBreakdown.Controls.Add(Me.txtTaxes)
        Me.fraBreakdown.Controls.Add(Me.txtNextInstalDate)
        Me.fraBreakdown.Controls.Add(Me.txtOtherInstalmentAmount)
        Me.fraBreakdown.Controls.Add(Me.txtDeposit)
        Me.fraBreakdown.Controls.Add(Me.txtFirstInstalDate)
        Me.fraBreakdown.Controls.Add(Me.txtProtection)
        Me.fraBreakdown.Controls.Add(Me.txtAdminCharge)
        Me.fraBreakdown.Controls.Add(Me.txtFirstInstalmentAmount)
        Me.fraBreakdown.Controls.Add(Me.txtInterest)
        Me.fraBreakdown.Controls.Add(Me.Line1)
        Me.fraBreakdown.Controls.Add(Me.lblLastInstDate)
        Me.fraBreakdown.Controls.Add(Me.lblTaxes)
        Me.fraBreakdown.Controls.Add(Me.lblNextInstDate)
        Me.fraBreakdown.Controls.Add(Me.lblOtherInstsl)
        Me.fraBreakdown.Controls.Add(Me.lblDeposit)
        Me.fraBreakdown.Controls.Add(Me.lblProtectionCharge)
        Me.fraBreakdown.Controls.Add(Me.lblFirstInstDate)
        Me.fraBreakdown.Controls.Add(Me.lblAdminCharge)
        Me.fraBreakdown.Controls.Add(Me.lblInterest)
        Me.fraBreakdown.Controls.Add(Me.lblFirstInstal)
        Me.fraBreakdown.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraBreakdown.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraBreakdown.Location = New System.Drawing.Point(14, 44)
        Me.fraBreakdown.Name = "fraBreakdown"
        Me.fraBreakdown.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraBreakdown.Size = New System.Drawing.Size(676, 177)
        Me.fraBreakdown.TabIndex = 1
        Me.fraBreakdown.TabStop = False
        Me.fraBreakdown.Tag = "CAP;311"
        Me.fraBreakdown.Text = "Breakdown"
        '
        'txtLastInstalDate
        '
        Me.txtLastInstalDate.AcceptsReturn = True
        Me.txtLastInstalDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtLastInstalDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLastInstalDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLastInstalDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLastInstalDate.Location = New System.Drawing.Point(152, 146)
        Me.txtLastInstalDate.MaxLength = 0
        Me.txtLastInstalDate.Name = "txtLastInstalDate"
        Me.txtLastInstalDate.ReadOnly = True
        Me.txtLastInstalDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLastInstalDate.Size = New System.Drawing.Size(121, 21)
        Me.txtLastInstalDate.TabIndex = 10
        Me.txtLastInstalDate.Tag = "F;"
        '
        'txtTaxes
        '
        Me.txtTaxes.AcceptsReturn = True
        Me.txtTaxes.BackColor = System.Drawing.SystemColors.Window
        Me.txtTaxes.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTaxes.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTaxes.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTaxes.Location = New System.Drawing.Point(400, 146)
        Me.txtTaxes.MaxLength = 0
        Me.txtTaxes.Name = "txtTaxes"
        Me.txtTaxes.ReadOnly = True
        Me.txtTaxes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTaxes.Size = New System.Drawing.Size(121, 21)
        Me.txtTaxes.TabIndex = 20
        Me.txtTaxes.Tag = "F;FMT;$;"
        '
        'txtNextInstalDate
        '
        Me.txtNextInstalDate.AcceptsReturn = True
        Me.txtNextInstalDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtNextInstalDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNextInstalDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNextInstalDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNextInstalDate.Location = New System.Drawing.Point(152, 122)
        Me.txtNextInstalDate.MaxLength = 0
        Me.txtNextInstalDate.Name = "txtNextInstalDate"
        Me.txtNextInstalDate.ReadOnly = True
        Me.txtNextInstalDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNextInstalDate.Size = New System.Drawing.Size(121, 21)
        Me.txtNextInstalDate.TabIndex = 8
        Me.txtNextInstalDate.Tag = "F;"
        '
        'txtOtherInstalmentAmount
        '
        Me.txtOtherInstalmentAmount.AcceptsReturn = True
        Me.txtOtherInstalmentAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtOtherInstalmentAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOtherInstalmentAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOtherInstalmentAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOtherInstalmentAmount.Location = New System.Drawing.Point(400, 122)
        Me.txtOtherInstalmentAmount.MaxLength = 0
        Me.txtOtherInstalmentAmount.Name = "txtOtherInstalmentAmount"
        Me.txtOtherInstalmentAmount.ReadOnly = True
        Me.txtOtherInstalmentAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOtherInstalmentAmount.Size = New System.Drawing.Size(121, 21)
        Me.txtOtherInstalmentAmount.TabIndex = 18
        Me.txtOtherInstalmentAmount.Tag = "F;FMT;$;"
        '
        'txtDeposit
        '
        Me.txtDeposit.AcceptsReturn = True
        Me.txtDeposit.BackColor = System.Drawing.SystemColors.Window
        Me.txtDeposit.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDeposit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDeposit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDeposit.Location = New System.Drawing.Point(152, 27)
        Me.txtDeposit.MaxLength = 0
        Me.txtDeposit.Name = "txtDeposit"
        Me.txtDeposit.ReadOnly = True
        Me.txtDeposit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDeposit.Size = New System.Drawing.Size(121, 21)
        Me.txtDeposit.TabIndex = 1
        Me.txtDeposit.Tag = "F;FMT;$;"
        '
        'txtFirstInstalDate
        '
        Me.txtFirstInstalDate.AcceptsReturn = True
        Me.txtFirstInstalDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtFirstInstalDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFirstInstalDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFirstInstalDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFirstInstalDate.Location = New System.Drawing.Point(152, 98)
        Me.txtFirstInstalDate.MaxLength = 0
        Me.txtFirstInstalDate.Name = "txtFirstInstalDate"
        Me.txtFirstInstalDate.ReadOnly = True
        Me.txtFirstInstalDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFirstInstalDate.Size = New System.Drawing.Size(121, 21)
        Me.txtFirstInstalDate.TabIndex = 6
        Me.txtFirstInstalDate.Tag = "F;"
        '
        'txtProtection
        '
        Me.txtProtection.AcceptsReturn = True
        Me.txtProtection.BackColor = System.Drawing.SystemColors.Window
        Me.txtProtection.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtProtection.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtProtection.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtProtection.Location = New System.Drawing.Point(152, 50)
        Me.txtProtection.MaxLength = 0
        Me.txtProtection.Name = "txtProtection"
        Me.txtProtection.ReadOnly = True
        Me.txtProtection.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtProtection.Size = New System.Drawing.Size(121, 21)
        Me.txtProtection.TabIndex = 3
        Me.txtProtection.Tag = "F;FMT;$;"
        '
        'txtAdminCharge
        '
        Me.txtAdminCharge.AcceptsReturn = True
        Me.txtAdminCharge.BackColor = System.Drawing.SystemColors.Window
        Me.txtAdminCharge.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAdminCharge.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdminCharge.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAdminCharge.Location = New System.Drawing.Point(400, 27)
        Me.txtAdminCharge.MaxLength = 0
        Me.txtAdminCharge.Name = "txtAdminCharge"
        Me.txtAdminCharge.ReadOnly = True
        Me.txtAdminCharge.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAdminCharge.Size = New System.Drawing.Size(121, 21)
        Me.txtAdminCharge.TabIndex = 12
        Me.txtAdminCharge.Tag = "F;FMT;$;"
        '
        'txtFirstInstalmentAmount
        '
        Me.txtFirstInstalmentAmount.AcceptsReturn = True
        Me.txtFirstInstalmentAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtFirstInstalmentAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFirstInstalmentAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFirstInstalmentAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFirstInstalmentAmount.Location = New System.Drawing.Point(400, 98)
        Me.txtFirstInstalmentAmount.MaxLength = 0
        Me.txtFirstInstalmentAmount.Name = "txtFirstInstalmentAmount"
        Me.txtFirstInstalmentAmount.ReadOnly = True
        Me.txtFirstInstalmentAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFirstInstalmentAmount.Size = New System.Drawing.Size(121, 21)
        Me.txtFirstInstalmentAmount.TabIndex = 16
        Me.txtFirstInstalmentAmount.Tag = "F;FMT;$;"
        '
        'txtInterest
        '
        Me.txtInterest.AcceptsReturn = True
        Me.txtInterest.BackColor = System.Drawing.SystemColors.Window
        Me.txtInterest.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInterest.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInterest.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInterest.Location = New System.Drawing.Point(400, 50)
        Me.txtInterest.MaxLength = 0
        Me.txtInterest.Name = "txtInterest"
        Me.txtInterest.ReadOnly = True
        Me.txtInterest.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInterest.Size = New System.Drawing.Size(121, 21)
        Me.txtInterest.TabIndex = 14
        Me.txtInterest.Tag = "F;FMT;$;"
        '
        'Line1
        '
        Me.Line1.BackColor = System.Drawing.SystemColors.WindowText
        Me.Line1.Location = New System.Drawing.Point(16, 80)
        Me.Line1.Name = "Line1"
        Me.Line1.Size = New System.Drawing.Size(504, 1)
        Me.Line1.TabIndex = 4
        '
        'lblLastInstDate
        '
        Me.lblLastInstDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblLastInstDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLastInstDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLastInstDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLastInstDate.Location = New System.Drawing.Point(16, 150)
        Me.lblLastInstDate.Name = "lblLastInstDate"
        Me.lblLastInstDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLastInstDate.Size = New System.Drawing.Size(137, 17)
        Me.lblLastInstDate.TabIndex = 9
        Me.lblLastInstDate.Tag = "CAP;316"
        Me.lblLastInstDate.Text = "Last Instalment Date"
        '
        'lblTaxes
        '
        Me.lblTaxes.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxes.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxes.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxes.Location = New System.Drawing.Point(288, 149)
        Me.lblTaxes.Name = "lblTaxes"
        Me.lblTaxes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxes.Size = New System.Drawing.Size(113, 17)
        Me.lblTaxes.TabIndex = 19
        Me.lblTaxes.Tag = "CAP;321"
        Me.lblTaxes.Text = "Taxes:"
        '
        'lblNextInstDate
        '
        Me.lblNextInstDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblNextInstDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNextInstDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNextInstDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNextInstDate.Location = New System.Drawing.Point(16, 126)
        Me.lblNextInstDate.Name = "lblNextInstDate"
        Me.lblNextInstDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNextInstDate.Size = New System.Drawing.Size(137, 17)
        Me.lblNextInstDate.TabIndex = 7
        Me.lblNextInstDate.Tag = "CAP;315"
        Me.lblNextInstDate.Text = "Next Instalment Date"
        '
        'lblOtherInstsl
        '
        Me.lblOtherInstsl.BackColor = System.Drawing.SystemColors.Control
        Me.lblOtherInstsl.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOtherInstsl.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOtherInstsl.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOtherInstsl.Location = New System.Drawing.Point(288, 125)
        Me.lblOtherInstsl.Name = "lblOtherInstsl"
        Me.lblOtherInstsl.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOtherInstsl.Size = New System.Drawing.Size(121, 17)
        Me.lblOtherInstsl.TabIndex = 17
        Me.lblOtherInstsl.Tag = "CAP;320"
        Me.lblOtherInstsl.Text = "Other Instalments"
        '
        'lblDeposit
        '
        Me.lblDeposit.BackColor = System.Drawing.SystemColors.Control
        Me.lblDeposit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDeposit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDeposit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDeposit.Location = New System.Drawing.Point(16, 27)
        Me.lblDeposit.Name = "lblDeposit"
        Me.lblDeposit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDeposit.Size = New System.Drawing.Size(113, 17)
        Me.lblDeposit.TabIndex = 0
        Me.lblDeposit.Tag = "CAP;312"
        Me.lblDeposit.Text = "Deposit"
        '
        'lblProtectionCharge
        '
        Me.lblProtectionCharge.BackColor = System.Drawing.SystemColors.Control
        Me.lblProtectionCharge.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProtectionCharge.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProtectionCharge.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProtectionCharge.Location = New System.Drawing.Point(16, 50)
        Me.lblProtectionCharge.Name = "lblProtectionCharge"
        Me.lblProtectionCharge.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProtectionCharge.Size = New System.Drawing.Size(121, 17)
        Me.lblProtectionCharge.TabIndex = 2
        Me.lblProtectionCharge.Tag = "CAP;313"
        Me.lblProtectionCharge.Text = "Protection Charge"
        '
        'lblFirstInstDate
        '
        Me.lblFirstInstDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblFirstInstDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFirstInstDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFirstInstDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFirstInstDate.Location = New System.Drawing.Point(16, 102)
        Me.lblFirstInstDate.Name = "lblFirstInstDate"
        Me.lblFirstInstDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFirstInstDate.Size = New System.Drawing.Size(137, 17)
        Me.lblFirstInstDate.TabIndex = 5
        Me.lblFirstInstDate.Tag = "CAP;314"
        Me.lblFirstInstDate.Text = "First Instalment Date"
        '
        'lblAdminCharge
        '
        Me.lblAdminCharge.BackColor = System.Drawing.SystemColors.Control
        Me.lblAdminCharge.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAdminCharge.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAdminCharge.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAdminCharge.Location = New System.Drawing.Point(288, 27)
        Me.lblAdminCharge.Name = "lblAdminCharge"
        Me.lblAdminCharge.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAdminCharge.Size = New System.Drawing.Size(113, 17)
        Me.lblAdminCharge.TabIndex = 11
        Me.lblAdminCharge.Tag = "CAP;317"
        Me.lblAdminCharge.Text = "Admin Charge"
        '
        'lblInterest
        '
        Me.lblInterest.BackColor = System.Drawing.SystemColors.Control
        Me.lblInterest.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInterest.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInterest.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInterest.Location = New System.Drawing.Point(288, 50)
        Me.lblInterest.Name = "lblInterest"
        Me.lblInterest.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInterest.Size = New System.Drawing.Size(113, 17)
        Me.lblInterest.TabIndex = 13
        Me.lblInterest.Tag = "CAP;318"
        Me.lblInterest.Text = "Interest Amount"
        '
        'lblFirstInstal
        '
        Me.lblFirstInstal.BackColor = System.Drawing.SystemColors.Control
        Me.lblFirstInstal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFirstInstal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFirstInstal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFirstInstal.Location = New System.Drawing.Point(288, 102)
        Me.lblFirstInstal.Name = "lblFirstInstal"
        Me.lblFirstInstal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFirstInstal.Size = New System.Drawing.Size(113, 17)
        Me.lblFirstInstal.TabIndex = 15
        Me.lblFirstInstal.Tag = "CAP;319"
        Me.lblFirstInstal.Text = "First Instalment"
        '
        'lblSummary
        '
        Me.lblSummary.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.lblSummary.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSummary.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSummary.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSummary.Location = New System.Drawing.Point(13, 12)
        Me.lblSummary.Name = "lblSummary"
        Me.lblSummary.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSummary.Size = New System.Drawing.Size(676, 29)
        Me.lblSummary.TabIndex = 0
        Me.lblSummary.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_ssTabMain_TabPage2
        '
        Me._ssTabMain_TabPage2.Controls.Add(Me.fraAdditionalOptions)
        Me._ssTabMain_TabPage2.Controls.Add(Me.fraOverrideOptions)
        Me._ssTabMain_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._ssTabMain_TabPage2.Name = "_ssTabMain_TabPage2"
        Me._ssTabMain_TabPage2.Size = New System.Drawing.Size(703, 227)
        Me._ssTabMain_TabPage2.TabIndex = 2
        Me._ssTabMain_TabPage2.Text = "3- Override"
        Me._ssTabMain_TabPage2.UseVisualStyleBackColor = True
        '
        'fraAdditionalOptions
        '
        Me.fraAdditionalOptions.BackColor = System.Drawing.SystemColors.Control
        Me.fraAdditionalOptions.Controls.Add(Me.chkUseTransCurrency)
        Me.fraAdditionalOptions.Controls.Add(Me.chkPaymentProtection)
        Me.fraAdditionalOptions.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAdditionalOptions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAdditionalOptions.Location = New System.Drawing.Point(355, 4)
        Me.fraAdditionalOptions.Name = "fraAdditionalOptions"
        Me.fraAdditionalOptions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAdditionalOptions.Size = New System.Drawing.Size(317, 209)
        Me.fraAdditionalOptions.TabIndex = 1
        Me.fraAdditionalOptions.TabStop = False
        Me.fraAdditionalOptions.Tag = "CAP;323"
        Me.fraAdditionalOptions.Text = "Additional Options"
        '
        'chkUseTransCurrency
        '
        Me.chkUseTransCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.chkUseTransCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkUseTransCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUseTransCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkUseTransCurrency.Location = New System.Drawing.Point(28, 56)
        Me.chkUseTransCurrency.Name = "chkUseTransCurrency"
        Me.chkUseTransCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkUseTransCurrency.Size = New System.Drawing.Size(154, 24)
        Me.chkUseTransCurrency.TabIndex = 7
        Me.chkUseTransCurrency.Tag = "CAP;328"
        Me.chkUseTransCurrency.Text = "Use Trans. Currency"
        Me.chkUseTransCurrency.UseVisualStyleBackColor = False
        '
        'chkPaymentProtection
        '
        Me.chkPaymentProtection.BackColor = System.Drawing.SystemColors.Control
        Me.chkPaymentProtection.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPaymentProtection.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPaymentProtection.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPaymentProtection.Location = New System.Drawing.Point(28, 120)
        Me.chkPaymentProtection.Name = "chkPaymentProtection"
        Me.chkPaymentProtection.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPaymentProtection.Size = New System.Drawing.Size(169, 33)
        Me.chkPaymentProtection.TabIndex = 0
        Me.chkPaymentProtection.Tag = "CAP;328"
        Me.chkPaymentProtection.Text = "Payment Protection?"
        Me.chkPaymentProtection.UseVisualStyleBackColor = False
        '
        'fraOverrideOptions
        '
        Me.fraOverrideOptions.BackColor = System.Drawing.SystemColors.Control
        Me.fraOverrideOptions.Controls.Add(Me._optDepositOverride_1)
        Me.fraOverrideOptions.Controls.Add(Me._optDepositOverride_0)
        Me.fraOverrideOptions.Controls.Add(Me.chkOverrideInterestRate)
        Me.fraOverrideOptions.Controls.Add(Me.txtNewRate)
        Me.fraOverrideOptions.Controls.Add(Me.chkCommissionOverride)
        Me.fraOverrideOptions.Controls.Add(Me.txtOverrideReference)
        Me.fraOverrideOptions.Controls.Add(Me.chkDepositOverride)
        Me.fraOverrideOptions.Controls.Add(Me.txtOverrideDeposit)
        Me.fraOverrideOptions.Controls.Add(Me.lblNewInterestRate)
        Me.fraOverrideOptions.Controls.Add(Me.lblReference)
        Me.fraOverrideOptions.Controls.Add(Me.lblDepositOverride)
        Me.fraOverrideOptions.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraOverrideOptions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraOverrideOptions.Location = New System.Drawing.Point(8, 4)
        Me.fraOverrideOptions.Name = "fraOverrideOptions"
        Me.fraOverrideOptions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraOverrideOptions.Size = New System.Drawing.Size(339, 209)
        Me.fraOverrideOptions.TabIndex = 0
        Me.fraOverrideOptions.TabStop = False
        Me.fraOverrideOptions.Tag = "CAP;322"
        Me.fraOverrideOptions.Text = "Override Options"
        '
        '_optDepositOverride_1
        '
        Me._optDepositOverride_1.AutoSize = True
        Me._optDepositOverride_1.Enabled = False
        Me._optDepositOverride_1.Location = New System.Drawing.Point(227, 178)
        Me._optDepositOverride_1.Name = "_optDepositOverride_1"
        Me._optDepositOverride_1.Size = New System.Drawing.Size(69, 17)
        Me._optDepositOverride_1.TabIndex = 10
        Me._optDepositOverride_1.Text = "Amount"
        Me._optDepositOverride_1.UseVisualStyleBackColor = True
        '
        '_optDepositOverride_0
        '
        Me._optDepositOverride_0.AutoSize = True
        Me._optDepositOverride_0.Enabled = False
        Me._optDepositOverride_0.Location = New System.Drawing.Point(184, 178)
        Me._optDepositOverride_0.Name = "_optDepositOverride_0"
        Me._optDepositOverride_0.Size = New System.Drawing.Size(37, 17)
        Me._optDepositOverride_0.TabIndex = 9
        Me._optDepositOverride_0.Text = "%"
        Me._optDepositOverride_0.UseVisualStyleBackColor = True
        '
        'chkOverrideInterestRate
        '
        Me.chkOverrideInterestRate.BackColor = System.Drawing.SystemColors.Control
        Me.chkOverrideInterestRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOverrideInterestRate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOverrideInterestRate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOverrideInterestRate.Location = New System.Drawing.Point(88, 24)
        Me.chkOverrideInterestRate.Name = "chkOverrideInterestRate"
        Me.chkOverrideInterestRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOverrideInterestRate.Size = New System.Drawing.Size(169, 33)
        Me.chkOverrideInterestRate.TabIndex = 0
        Me.chkOverrideInterestRate.Tag = "CAP;324"
        Me.chkOverrideInterestRate.Text = "Override Interest Rate?"
        Me.chkOverrideInterestRate.UseVisualStyleBackColor = False
        '
        'txtNewRate
        '
        Me.txtNewRate.AcceptsReturn = True
        Me.txtNewRate.BackColor = System.Drawing.SystemColors.Control
        Me.txtNewRate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNewRate.Enabled = False
        Me.txtNewRate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNewRate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNewRate.Location = New System.Drawing.Point(88, 56)
        Me.txtNewRate.MaxLength = 0
        Me.txtNewRate.Name = "txtNewRate"
        Me.txtNewRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNewRate.Size = New System.Drawing.Size(89, 21)
        Me.txtNewRate.TabIndex = 2
        Me.txtNewRate.Tag = "F;"
        Me.txtNewRate.Text = " "
        '
        'chkCommissionOverride
        '
        Me.chkCommissionOverride.BackColor = System.Drawing.SystemColors.Control
        Me.chkCommissionOverride.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCommissionOverride.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCommissionOverride.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCommissionOverride.Location = New System.Drawing.Point(88, 88)
        Me.chkCommissionOverride.Name = "chkCommissionOverride"
        Me.chkCommissionOverride.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCommissionOverride.Size = New System.Drawing.Size(169, 33)
        Me.chkCommissionOverride.TabIndex = 3
        Me.chkCommissionOverride.Tag = "CAP;326"
        Me.chkCommissionOverride.Text = "Commission Override?"
        Me.chkCommissionOverride.UseVisualStyleBackColor = False
        '
        'txtOverrideReference
        '
        Me.txtOverrideReference.AcceptsReturn = True
        Me.txtOverrideReference.BackColor = System.Drawing.SystemColors.Control
        Me.txtOverrideReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOverrideReference.Enabled = False
        Me.txtOverrideReference.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOverrideReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOverrideReference.Location = New System.Drawing.Point(88, 120)
        Me.txtOverrideReference.MaxLength = 0
        Me.txtOverrideReference.Name = "txtOverrideReference"
        Me.txtOverrideReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOverrideReference.Size = New System.Drawing.Size(89, 21)
        Me.txtOverrideReference.TabIndex = 5
        Me.txtOverrideReference.Tag = "F;"
        Me.txtOverrideReference.Text = " "
        '
        'chkDepositOverride
        '
        Me.chkDepositOverride.BackColor = System.Drawing.SystemColors.Control
        Me.chkDepositOverride.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDepositOverride.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDepositOverride.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDepositOverride.Location = New System.Drawing.Point(88, 152)
        Me.chkDepositOverride.Name = "chkDepositOverride"
        Me.chkDepositOverride.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDepositOverride.Size = New System.Drawing.Size(153, 17)
        Me.chkDepositOverride.TabIndex = 6
        Me.chkDepositOverride.Text = "Deposit Override?"
        Me.chkDepositOverride.UseVisualStyleBackColor = False
        '
        'txtOverrideDeposit
        '
        Me.txtOverrideDeposit.AcceptsReturn = True
        Me.txtOverrideDeposit.BackColor = System.Drawing.SystemColors.Control
        Me.txtOverrideDeposit.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOverrideDeposit.Enabled = False
        Me.txtOverrideDeposit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOverrideDeposit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOverrideDeposit.Location = New System.Drawing.Point(88, 176)
        Me.txtOverrideDeposit.MaxLength = 0
        Me.txtOverrideDeposit.Name = "txtOverrideDeposit"
        Me.txtOverrideDeposit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOverrideDeposit.Size = New System.Drawing.Size(89, 21)
        Me.txtOverrideDeposit.TabIndex = 8
        Me.txtOverrideDeposit.Tag = "F;"
        '
        'lblNewInterestRate
        '
        Me.lblNewInterestRate.BackColor = System.Drawing.SystemColors.Control
        Me.lblNewInterestRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNewInterestRate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNewInterestRate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNewInterestRate.Location = New System.Drawing.Point(8, 56)
        Me.lblNewInterestRate.Name = "lblNewInterestRate"
        Me.lblNewInterestRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNewInterestRate.Size = New System.Drawing.Size(73, 17)
        Me.lblNewInterestRate.TabIndex = 1
        Me.lblNewInterestRate.Tag = "CAP;325"
        Me.lblNewInterestRate.Text = "New Rate:"
        '
        'lblReference
        '
        Me.lblReference.BackColor = System.Drawing.SystemColors.Control
        Me.lblReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReference.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReference.Location = New System.Drawing.Point(8, 121)
        Me.lblReference.Name = "lblReference"
        Me.lblReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReference.Size = New System.Drawing.Size(81, 17)
        Me.lblReference.TabIndex = 4
        Me.lblReference.Tag = "CAP;327"
        Me.lblReference.Text = "Reference:"
        '
        'lblDepositOverride
        '
        Me.lblDepositOverride.BackColor = System.Drawing.SystemColors.Control
        Me.lblDepositOverride.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDepositOverride.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDepositOverride.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDepositOverride.Location = New System.Drawing.Point(8, 177)
        Me.lblDepositOverride.Name = "lblDepositOverride"
        Me.lblDepositOverride.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDepositOverride.Size = New System.Drawing.Size(81, 17)
        Me.lblDepositOverride.TabIndex = 7
        Me.lblDepositOverride.Text = "Deposit: "
        '
        '_ssTabMain_TabPage3
        '
        Me._ssTabMain_TabPage3.Controls.Add(Me.fraFinanceDetails)
        Me._ssTabMain_TabPage3.Controls.Add(Me.fraMinDeposit)
        Me._ssTabMain_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._ssTabMain_TabPage3.Name = "_ssTabMain_TabPage3"
        Me._ssTabMain_TabPage3.Size = New System.Drawing.Size(703, 227)
        Me._ssTabMain_TabPage3.TabIndex = 3
        Me._ssTabMain_TabPage3.Text = "4- Other Details"
        Me._ssTabMain_TabPage3.UseVisualStyleBackColor = True
        '
        'fraFinanceDetails
        '
        Me.fraFinanceDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraFinanceDetails.Controls.Add(Me.txtAmountFinanced)
        Me.fraFinanceDetails.Controls.Add(Me.txtTotalTax)
        Me.fraFinanceDetails.Controls.Add(Me.txtTotalFee)
        Me.fraFinanceDetails.Controls.Add(Me.txtGross)
        Me.fraFinanceDetails.Controls.Add(Me.lblTotalFinanced)
        Me.fraFinanceDetails.Controls.Add(Me.lblTaxExcluded)
        Me.fraFinanceDetails.Controls.Add(Me.lblFeeExcluded)
        Me.fraFinanceDetails.Controls.Add(Me.lblGrossDue)
        Me.fraFinanceDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraFinanceDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFinanceDetails.Location = New System.Drawing.Point(8, 4)
        Me.fraFinanceDetails.Name = "fraFinanceDetails"
        Me.fraFinanceDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFinanceDetails.Size = New System.Drawing.Size(676, 121)
        Me.fraFinanceDetails.TabIndex = 0
        Me.fraFinanceDetails.TabStop = False
        Me.fraFinanceDetails.Text = "Finance Details"
        '
        'txtAmountFinanced
        '
        Me.txtAmountFinanced.AcceptsReturn = True
        Me.txtAmountFinanced.BackColor = System.Drawing.SystemColors.Control
        Me.txtAmountFinanced.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAmountFinanced.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmountFinanced.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAmountFinanced.Location = New System.Drawing.Point(320, 90)
        Me.txtAmountFinanced.MaxLength = 0
        Me.txtAmountFinanced.Name = "txtAmountFinanced"
        Me.txtAmountFinanced.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAmountFinanced.Size = New System.Drawing.Size(201, 21)
        Me.txtAmountFinanced.TabIndex = 7
        '
        'txtTotalTax
        '
        Me.txtTotalTax.AcceptsReturn = True
        Me.txtTotalTax.BackColor = System.Drawing.SystemColors.Control
        Me.txtTotalTax.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalTax.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalTax.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalTax.Location = New System.Drawing.Point(320, 66)
        Me.txtTotalTax.MaxLength = 0
        Me.txtTotalTax.Name = "txtTotalTax"
        Me.txtTotalTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalTax.Size = New System.Drawing.Size(201, 21)
        Me.txtTotalTax.TabIndex = 5
        '
        'txtTotalFee
        '
        Me.txtTotalFee.AcceptsReturn = True
        Me.txtTotalFee.BackColor = System.Drawing.SystemColors.Control
        Me.txtTotalFee.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalFee.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalFee.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalFee.Location = New System.Drawing.Point(320, 42)
        Me.txtTotalFee.MaxLength = 0
        Me.txtTotalFee.Name = "txtTotalFee"
        Me.txtTotalFee.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalFee.Size = New System.Drawing.Size(201, 21)
        Me.txtTotalFee.TabIndex = 3
        '
        'txtGross
        '
        Me.txtGross.AcceptsReturn = True
        Me.txtGross.BackColor = System.Drawing.SystemColors.Control
        Me.txtGross.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtGross.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGross.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtGross.Location = New System.Drawing.Point(320, 18)
        Me.txtGross.MaxLength = 0
        Me.txtGross.Name = "txtGross"
        Me.txtGross.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtGross.Size = New System.Drawing.Size(201, 21)
        Me.txtGross.TabIndex = 1
        '
        'lblTotalFinanced
        '
        Me.lblTotalFinanced.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalFinanced.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalFinanced.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalFinanced.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalFinanced.Location = New System.Drawing.Point(32, 90)
        Me.lblTotalFinanced.Name = "lblTotalFinanced"
        Me.lblTotalFinanced.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalFinanced.Size = New System.Drawing.Size(209, 17)
        Me.lblTotalFinanced.TabIndex = 6
        Me.lblTotalFinanced.Text = "Total amount able to be Financed"
        '
        'lblTaxExcluded
        '
        Me.lblTaxExcluded.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxExcluded.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxExcluded.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxExcluded.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxExcluded.Location = New System.Drawing.Point(32, 66)
        Me.lblTaxExcluded.Name = "lblTaxExcluded"
        Me.lblTaxExcluded.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxExcluded.Size = New System.Drawing.Size(263, 17)
        Me.lblTaxExcluded.TabIndex = 4
        Me.lblTaxExcluded.Text = "Total Taxes excluded for financing"
        '
        'lblFeeExcluded
        '
        Me.lblFeeExcluded.BackColor = System.Drawing.SystemColors.Control
        Me.lblFeeExcluded.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFeeExcluded.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFeeExcluded.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFeeExcluded.Location = New System.Drawing.Point(32, 42)
        Me.lblFeeExcluded.Name = "lblFeeExcluded"
        Me.lblFeeExcluded.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFeeExcluded.Size = New System.Drawing.Size(257, 25)
        Me.lblFeeExcluded.TabIndex = 2
        Me.lblFeeExcluded.Text = "Total Fees excluded for financing"
        '
        'lblGrossDue
        '
        Me.lblGrossDue.BackColor = System.Drawing.SystemColors.Control
        Me.lblGrossDue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGrossDue.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGrossDue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGrossDue.Location = New System.Drawing.Point(32, 18)
        Me.lblGrossDue.Name = "lblGrossDue"
        Me.lblGrossDue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGrossDue.Size = New System.Drawing.Size(137, 17)
        Me.lblGrossDue.TabIndex = 0
        Me.lblGrossDue.Text = "Gross Due from Client"
        '
        'fraMinDeposit
        '
        Me.fraMinDeposit.BackColor = System.Drawing.SystemColors.Control
        Me.fraMinDeposit.Controls.Add(Me.txtMinDeposit)
        Me.fraMinDeposit.Controls.Add(Me.txtTaxDeposit)
        Me.fraMinDeposit.Controls.Add(Me.txtFeeDeposit)
        Me.fraMinDeposit.Controls.Add(Me.lblMinDeposit)
        Me.fraMinDeposit.Controls.Add(Me.lblTaxDeposit)
        Me.fraMinDeposit.Controls.Add(Me.lblFeeDeposit)
        Me.fraMinDeposit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraMinDeposit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraMinDeposit.Location = New System.Drawing.Point(8, 128)
        Me.fraMinDeposit.Name = "fraMinDeposit"
        Me.fraMinDeposit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraMinDeposit.Size = New System.Drawing.Size(676, 93)
        Me.fraMinDeposit.TabIndex = 1
        Me.fraMinDeposit.TabStop = False
        Me.fraMinDeposit.Text = "Minimum Deposit Required"
        '
        'txtMinDeposit
        '
        Me.txtMinDeposit.AcceptsReturn = True
        Me.txtMinDeposit.BackColor = System.Drawing.SystemColors.Control
        Me.txtMinDeposit.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMinDeposit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMinDeposit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMinDeposit.Location = New System.Drawing.Point(320, 64)
        Me.txtMinDeposit.MaxLength = 0
        Me.txtMinDeposit.Name = "txtMinDeposit"
        Me.txtMinDeposit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMinDeposit.Size = New System.Drawing.Size(201, 21)
        Me.txtMinDeposit.TabIndex = 5
        '
        'txtTaxDeposit
        '
        Me.txtTaxDeposit.AcceptsReturn = True
        Me.txtTaxDeposit.BackColor = System.Drawing.SystemColors.Control
        Me.txtTaxDeposit.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTaxDeposit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTaxDeposit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTaxDeposit.Location = New System.Drawing.Point(320, 40)
        Me.txtTaxDeposit.MaxLength = 0
        Me.txtTaxDeposit.Name = "txtTaxDeposit"
        Me.txtTaxDeposit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTaxDeposit.Size = New System.Drawing.Size(201, 21)
        Me.txtTaxDeposit.TabIndex = 3
        '
        'txtFeeDeposit
        '
        Me.txtFeeDeposit.AcceptsReturn = True
        Me.txtFeeDeposit.BackColor = System.Drawing.SystemColors.Control
        Me.txtFeeDeposit.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFeeDeposit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFeeDeposit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFeeDeposit.Location = New System.Drawing.Point(320, 16)
        Me.txtFeeDeposit.MaxLength = 0
        Me.txtFeeDeposit.Name = "txtFeeDeposit"
        Me.txtFeeDeposit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFeeDeposit.Size = New System.Drawing.Size(201, 21)
        Me.txtFeeDeposit.TabIndex = 1
        '
        'lblMinDeposit
        '
        Me.lblMinDeposit.BackColor = System.Drawing.SystemColors.Control
        Me.lblMinDeposit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMinDeposit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMinDeposit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMinDeposit.Location = New System.Drawing.Point(32, 64)
        Me.lblMinDeposit.Name = "lblMinDeposit"
        Me.lblMinDeposit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMinDeposit.Size = New System.Drawing.Size(241, 17)
        Me.lblMinDeposit.TabIndex = 4
        Me.lblMinDeposit.Text = "Total Minimum Deposit"
        '
        'lblTaxDeposit
        '
        Me.lblTaxDeposit.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxDeposit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxDeposit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxDeposit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxDeposit.Location = New System.Drawing.Point(32, 40)
        Me.lblTaxDeposit.Name = "lblTaxDeposit"
        Me.lblTaxDeposit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxDeposit.Size = New System.Drawing.Size(265, 17)
        Me.lblTaxDeposit.TabIndex = 2
        Me.lblTaxDeposit.Text = "Total Taxes must be collected as Deposit"
        '
        'lblFeeDeposit
        '
        Me.lblFeeDeposit.BackColor = System.Drawing.SystemColors.Control
        Me.lblFeeDeposit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFeeDeposit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFeeDeposit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFeeDeposit.Location = New System.Drawing.Point(32, 16)
        Me.lblFeeDeposit.Name = "lblFeeDeposit"
        Me.lblFeeDeposit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFeeDeposit.Size = New System.Drawing.Size(281, 17)
        Me.lblFeeDeposit.TabIndex = 0
        Me.lblFeeDeposit.Text = "Total Fees must be collected as Deposit"
        '
        'ssTabOptions
        '
        Me.ssTabOptions.Controls.Add(Me._ssTabOptions_TabPage0)
        Me.ssTabOptions.Controls.Add(Me._ssTabOptions_TabPage1)
        Me.ssTabOptions.Controls.Add(Me._ssTabOptions_TabPage2)
        Me.ssTabOptions.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ssTabOptions.ItemSize = New System.Drawing.Size(186, 18)
        Me.ssTabOptions.Location = New System.Drawing.Point(8, 8)
        Me.ssTabOptions.Multiline = True
        Me.ssTabOptions.Name = "ssTabOptions"
        Me.ssTabOptions.SelectedIndex = 0
        Me.ssTabOptions.Size = New System.Drawing.Size(711, 69)
        Me.ssTabOptions.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.ssTabOptions.TabIndex = 0
        Me.ssTabOptions.Tag = "CAP;330"
        '
        '_ssTabOptions_TabPage0
        '
        Me._ssTabOptions_TabPage0.Controls.Add(Me.lblMonthDay)
        Me._ssTabOptions_TabPage0.Controls.Add(Me.lblPreferredFirstDate)
        Me._ssTabOptions_TabPage0.Controls.Add(Me.lblWeekday)
        Me._ssTabOptions_TabPage0.Controls.Add(Me.cboPreferredDate)
        Me._ssTabOptions_TabPage0.Controls.Add(Me.cboMonthDay)
        Me._ssTabOptions_TabPage0.Controls.Add(Me.cboWeekDay)
        Me._ssTabOptions_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._ssTabOptions_TabPage0.Name = "_ssTabOptions_TabPage0"
        Me._ssTabOptions_TabPage0.Size = New System.Drawing.Size(703, 43)
        Me._ssTabOptions_TabPage0.TabIndex = 0
        Me._ssTabOptions_TabPage0.Text = "Preferred Days"
        Me._ssTabOptions_TabPage0.UseVisualStyleBackColor = True
        '
        'lblMonthDay
        '
        Me.lblMonthDay.BackColor = System.Drawing.SystemColors.Control
        Me.lblMonthDay.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMonthDay.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMonthDay.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMonthDay.Location = New System.Drawing.Point(174, 16)
        Me.lblMonthDay.Name = "lblMonthDay"
        Me.lblMonthDay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMonthDay.Size = New System.Drawing.Size(89, 17)
        Me.lblMonthDay.TabIndex = 2
        Me.lblMonthDay.Tag = "CAP;301"
        Me.lblMonthDay.Text = "Day in Month"
        '
        'lblPreferredFirstDate
        '
        Me.lblPreferredFirstDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblPreferredFirstDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPreferredFirstDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPreferredFirstDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPreferredFirstDate.Location = New System.Drawing.Point(328, 16)
        Me.lblPreferredFirstDate.Name = "lblPreferredFirstDate"
        Me.lblPreferredFirstDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPreferredFirstDate.Size = New System.Drawing.Size(97, 17)
        Me.lblPreferredFirstDate.TabIndex = 4
        Me.lblPreferredFirstDate.Tag = "CAP;302"
        Me.lblPreferredFirstDate.Text = "First Payment"
        '
        'lblWeekday
        '
        Me.lblWeekday.BackColor = System.Drawing.SystemColors.Control
        Me.lblWeekday.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblWeekday.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWeekday.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblWeekday.Location = New System.Drawing.Point(8, 16)
        Me.lblWeekday.Name = "lblWeekday"
        Me.lblWeekday.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblWeekday.Size = New System.Drawing.Size(65, 17)
        Me.lblWeekday.TabIndex = 0
        Me.lblWeekday.Tag = "CAP;300"
        Me.lblWeekday.Text = "Weekday"
        '
        'cboPreferredDate
        '
        Me.cboPreferredDate.BackColor = System.Drawing.SystemColors.Window
        Me.cboPreferredDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPreferredDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPreferredDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPreferredDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPreferredDate.Location = New System.Drawing.Point(427, 14)
        Me.cboPreferredDate.Name = "cboPreferredDate"
        Me.cboPreferredDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPreferredDate.Size = New System.Drawing.Size(97, 21)
        Me.cboPreferredDate.TabIndex = 5
        '
        'cboMonthDay
        '
        Me.cboMonthDay.BackColor = System.Drawing.SystemColors.Window
        Me.cboMonthDay.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboMonthDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMonthDay.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMonthDay.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboMonthDay.Location = New System.Drawing.Point(267, 14)
        Me.cboMonthDay.Name = "cboMonthDay"
        Me.cboMonthDay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboMonthDay.Size = New System.Drawing.Size(49, 21)
        Me.cboMonthDay.TabIndex = 3
        '
        'cboWeekDay
        '
        Me.cboWeekDay.BackColor = System.Drawing.SystemColors.Window
        Me.cboWeekDay.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboWeekDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboWeekDay.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboWeekDay.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboWeekDay.Location = New System.Drawing.Point(75, 14)
        Me.cboWeekDay.Name = "cboWeekDay"
        Me.cboWeekDay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboWeekDay.Size = New System.Drawing.Size(89, 21)
        Me.cboWeekDay.TabIndex = 1
        '
        '_ssTabOptions_TabPage1
        '
        Me._ssTabOptions_TabPage1.Controls.Add(Me._optMTAType_2)
        Me._ssTabOptions_TabPage1.Controls.Add(Me._optMTAType_1)
        Me._ssTabOptions_TabPage1.Controls.Add(Me._optMTAType_0)
        Me._ssTabOptions_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._ssTabOptions_TabPage1.Name = "_ssTabOptions_TabPage1"
        Me._ssTabOptions_TabPage1.Size = New System.Drawing.Size(703, 43)
        Me._ssTabOptions_TabPage1.TabIndex = 1
        Me._ssTabOptions_TabPage1.Text = "MTA"
        Me._ssTabOptions_TabPage1.UseVisualStyleBackColor = True
        '
        '_optMTAType_2
        '
        Me._optMTAType_2.BackColor = System.Drawing.SystemColors.Control
        Me._optMTAType_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optMTAType_2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optMTAType_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optMTAType_2.Location = New System.Drawing.Point(395, 12)
        Me._optMTAType_2.Name = "_optMTAType_2"
        Me._optMTAType_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optMTAType_2.Size = New System.Drawing.Size(169, 25)
        Me._optMTAType_2.TabIndex = 38
        Me._optMTAType_2.Tag = "CAP;335"
        Me._optMTAType_2.Text = "Put MTA Amount into new plan"
        Me._optMTAType_2.UseVisualStyleBackColor = False
        '
        '_optMTAType_1
        '
        Me._optMTAType_1.BackColor = System.Drawing.SystemColors.Control
        Me._optMTAType_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optMTAType_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optMTAType_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optMTAType_1.Location = New System.Drawing.Point(206, 12)
        Me._optMTAType_1.Name = "_optMTAType_1"
        Me._optMTAType_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optMTAType_1.Size = New System.Drawing.Size(172, 25)
        Me._optMTAType_1.TabIndex = 37
        Me._optMTAType_1.Tag = "CAP;334"
        Me._optMTAType_1.Text = "Put MTA Amount in next instalment"
        Me._optMTAType_1.UseVisualStyleBackColor = False
        '
        '_optMTAType_0
        '
        Me._optMTAType_0.BackColor = System.Drawing.SystemColors.Control
        Me._optMTAType_0.Checked = True
        Me._optMTAType_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optMTAType_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optMTAType_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optMTAType_0.Location = New System.Drawing.Point(28, 12)
        Me._optMTAType_0.Name = "_optMTAType_0"
        Me._optMTAType_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optMTAType_0.Size = New System.Drawing.Size(155, 25)
        Me._optMTAType_0.TabIndex = 36
        Me._optMTAType_0.TabStop = True
        Me._optMTAType_0.Tag = "CAP;333"
        Me._optMTAType_0.Text = "Spread over remaining plan"
        Me._optMTAType_0.UseVisualStyleBackColor = False
        '
        '_ssTabOptions_TabPage2
        '
        Me._ssTabOptions_TabPage2.Controls.Add(Me.cboExistingCreditCardDtls)
        Me._ssTabOptions_TabPage2.Controls.Add(Me.cboExistingBankAccDtls)
        Me._ssTabOptions_TabPage2.Controls.Add(Me.lblExistingCreditCardDtls)
        Me._ssTabOptions_TabPage2.Controls.Add(Me.lblExistingBankAccDtls)
        Me._ssTabOptions_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._ssTabOptions_TabPage2.Name = "_ssTabOptions_TabPage2"
        Me._ssTabOptions_TabPage2.Size = New System.Drawing.Size(703, 43)
        Me._ssTabOptions_TabPage2.TabIndex = 2
        Me._ssTabOptions_TabPage2.Text = "Use Existing Plan Details"
        Me._ssTabOptions_TabPage2.UseVisualStyleBackColor = True
        '
        'cboExistingCreditCardDtls
        '
        Me.cboExistingCreditCardDtls.BackColor = System.Drawing.SystemColors.Window
        Me.cboExistingCreditCardDtls.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboExistingCreditCardDtls.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboExistingCreditCardDtls.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboExistingCreditCardDtls.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboExistingCreditCardDtls.Location = New System.Drawing.Point(392, 12)
        Me.cboExistingCreditCardDtls.Name = "cboExistingCreditCardDtls"
        Me.cboExistingCreditCardDtls.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboExistingCreditCardDtls.Size = New System.Drawing.Size(161, 21)
        Me.cboExistingCreditCardDtls.TabIndex = 3
        '
        'cboExistingBankAccDtls
        '
        Me.cboExistingBankAccDtls.BackColor = System.Drawing.SystemColors.Window
        Me.cboExistingBankAccDtls.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboExistingBankAccDtls.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboExistingBankAccDtls.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboExistingBankAccDtls.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboExistingBankAccDtls.Location = New System.Drawing.Point(101, 12)
        Me.cboExistingBankAccDtls.Name = "cboExistingBankAccDtls"
        Me.cboExistingBankAccDtls.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboExistingBankAccDtls.Size = New System.Drawing.Size(161, 21)
        Me.cboExistingBankAccDtls.TabIndex = 1
        '
        'lblExistingCreditCardDtls
        '
        Me.lblExistingCreditCardDtls.BackColor = System.Drawing.SystemColors.Control
        Me.lblExistingCreditCardDtls.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExistingCreditCardDtls.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExistingCreditCardDtls.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExistingCreditCardDtls.Location = New System.Drawing.Point(268, 16)
        Me.lblExistingCreditCardDtls.Name = "lblExistingCreditCardDtls"
        Me.lblExistingCreditCardDtls.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExistingCreditCardDtls.Size = New System.Drawing.Size(118, 17)
        Me.lblExistingCreditCardDtls.TabIndex = 2
        Me.lblExistingCreditCardDtls.Tag = "CAP;802"
        Me.lblExistingCreditCardDtls.Text = "Credit Card Details"
        '
        'lblExistingBankAccDtls
        '
        Me.lblExistingBankAccDtls.BackColor = System.Drawing.SystemColors.Control
        Me.lblExistingBankAccDtls.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExistingBankAccDtls.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExistingBankAccDtls.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExistingBankAccDtls.Location = New System.Drawing.Point(8, 16)
        Me.lblExistingBankAccDtls.Name = "lblExistingBankAccDtls"
        Me.lblExistingBankAccDtls.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExistingBankAccDtls.Size = New System.Drawing.Size(87, 17)
        Me.lblExistingBankAccDtls.TabIndex = 0
        Me.lblExistingBankAccDtls.Tag = "CAP;801"
        Me.lblExistingBankAccDtls.Text = "Bank Details"
        '
        'shpProcessing
        '
        Me.shpProcessing.BackColor = System.Drawing.SystemColors.Window
        Me.shpProcessing.BackStyle = 0
        Me.shpProcessing.BorderColor = System.Drawing.SystemColors.ControlDark
        Me.shpProcessing.BorderStyle = 1
        Me.shpProcessing.BorderWidth = 1
        Me.shpProcessing.FillColor = System.Drawing.Color.Transparent
        Me.shpProcessing.FillStyle = 1
        Me.shpProcessing.Location = New System.Drawing.Point(0, 0)
        Me.shpProcessing.Name = "shpProcessing"
        Me.shpProcessing.RoundPercent = 15
        Me.shpProcessing.Shape = 0
        Me.shpProcessing.Size = New System.Drawing.Size(233, 33)
        Me.shpProcessing.TabIndex = 56
        '
        'uctInstalments
        '
        Me.Controls.Add(Me.txtInvisible)
        Me.Controls.Add(Me.ssTabMain)
        Me.Controls.Add(Me.ssTabOptions)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctInstalments"
        Me.Size = New System.Drawing.Size(722, 341)
        CType(Me.picProcessing, System.ComponentModel.ISupportInitialize).EndInit()
        Me.picProcessing.ResumeLayout(False)
        Me.picProcessing.PerformLayout()
        CType(Me.Image1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ssTabMain.ResumeLayout(False)
        Me._ssTabMain_TabPage0.ResumeLayout(False)
        CType(Me.tdgListInstalment, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tdgInstalment, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraSummary.ResumeLayout(False)
        Me.fraSummary.PerformLayout()
        Me._ssTabMain_TabPage1.ResumeLayout(False)
        Me.fraBreakdown.ResumeLayout(False)
        Me.fraBreakdown.PerformLayout()
        Me._ssTabMain_TabPage2.ResumeLayout(False)
        Me.fraAdditionalOptions.ResumeLayout(False)
        Me.fraOverrideOptions.ResumeLayout(False)
        Me.fraOverrideOptions.PerformLayout()
        Me._ssTabMain_TabPage3.ResumeLayout(False)
        Me.fraFinanceDetails.ResumeLayout(False)
        Me.fraFinanceDetails.PerformLayout()
        Me.fraMinDeposit.ResumeLayout(False)
        Me.fraMinDeposit.PerformLayout()
        Me.ssTabOptions.ResumeLayout(False)
        Me._ssTabOptions_TabPage0.ResumeLayout(False)
        Me._ssTabOptions_TabPage1.ResumeLayout(False)
        Me._ssTabOptions_TabPage2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializeoptMTAType()
        Me.optMTAType(2) = _optMTAType_2
        Me.optMTAType(1) = _optMTAType_1
        Me.optMTAType(0) = _optMTAType_0
    End Sub
    Sub InitializeoptDepositType()
        Me.optDepositType(0) = _optDepositOverride_0
        Me.optDepositType(1) = _optDepositOverride_1
    End Sub
    Friend WithEvents shpProcessing As Artinsoft.VB6.Gui.ShapeHelper
    Private WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Private WithEvents colScheme As System.Windows.Forms.DataGridViewTextBoxColumn
    Private WithEvents colPayment As System.Windows.Forms.DataGridViewTextBoxColumn
    Private WithEvents colMedia As System.Windows.Forms.DataGridViewTextBoxColumn
    Private WithEvents colDeposit As System.Windows.Forms.DataGridViewTextBoxColumn
    Private WithEvents colAmt As System.Windows.Forms.DataGridViewTextBoxColumn
    Private WithEvents colType As System.Windows.Forms.DataGridViewTextBoxColumn
    Private WithEvents colFunding As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents chkUseTransCurrency As System.Windows.Forms.CheckBox
    Friend WithEvents _optDepositOverride_1 As System.Windows.Forms.RadioButton
    Friend WithEvents _optDepositOverride_0 As System.Windows.Forms.RadioButton
#End Region
End Class
