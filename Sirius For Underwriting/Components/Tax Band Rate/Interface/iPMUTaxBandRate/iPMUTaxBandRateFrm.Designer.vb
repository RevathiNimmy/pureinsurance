<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializeoptBasis()
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
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
	Public WithEvents lblCode As System.Windows.Forms.Label
	Public WithEvents cmdDetailOK As System.Windows.Forms.Button
	Public WithEvents cmdDetailCancel As System.Windows.Forms.Button
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents txtCode As System.Windows.Forms.TextBox
	Public WithEvents txtEffectiveDate As System.Windows.Forms.TextBox
	Public WithEvents chkAllowCredit As System.Windows.Forms.CheckBox
	Public WithEvents chkRounded As System.Windows.Forms.CheckBox
	Public WithEvents txtRateValue As System.Windows.Forms.TextBox
	Public WithEvents txtRatePercentage As System.Windows.Forms.TextBox
	Public WithEvents chkIsValue As System.Windows.Forms.CheckBox
	Private WithEvents _optBasis_3 As System.Windows.Forms.RadioButton
	Private WithEvents _optBasis_2 As System.Windows.Forms.RadioButton
	Public WithEvents txtSumInsuredValue As System.Windows.Forms.TextBox
	Private WithEvents _optBasis_0 As System.Windows.Forms.RadioButton
	Private WithEvents _optBasis_1 As System.Windows.Forms.RadioButton
	Public WithEvents cboCurrency As UserControls.CurrencyLookup
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	Public WithEvents lblOfSI As System.Windows.Forms.Label
	Public WithEvents lblRate As System.Windows.Forms.Label
	Public WithEvents lblValue As System.Windows.Forms.Label
	Public WithEvents lblPer As System.Windows.Forms.Label
	Public WithEvents fraCalculation As System.Windows.Forms.GroupBox
	Public WithEvents chkREN As System.Windows.Forms.CheckBox
	Public WithEvents chkCANC As System.Windows.Forms.CheckBox
	Public WithEvents chkRMTA As System.Windows.Forms.CheckBox
	Public WithEvents chkAMTA As System.Windows.Forms.CheckBox
	Public WithEvents chkNB As System.Windows.Forms.CheckBox
	Public WithEvents fraAffectedTransactions As System.Windows.Forms.GroupBox
	Public WithEvents cboCountry As PMLookupControl.cboPMLookup
	Public WithEvents cboState As PMLookupControl.cboPMLookup
	Public WithEvents cboCOB As PMLookupControl.cboPMLookup
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents lblCOB As System.Windows.Forms.Label
	Public WithEvents lblState As System.Windows.Forms.Label
	Public WithEvents lblCountry As System.Windows.Forms.Label
	Public WithEvents fraFilters As System.Windows.Forms.GroupBox
	Public WithEvents chkTTI As System.Windows.Forms.CheckBox
	Public WithEvents chkTTRIC As System.Windows.Forms.CheckBox
	Public WithEvents chkTTRI As System.Windows.Forms.CheckBox
	Public WithEvents chkTTAC As System.Windows.Forms.CheckBox
	Public WithEvents chkTTF As System.Windows.Forms.CheckBox
	Public WithEvents chkTTCR As System.Windows.Forms.CheckBox
	Public WithEvents chkTTCS As System.Windows.Forms.CheckBox
	Public WithEvents chkTTCP As System.Windows.Forms.CheckBox
	Public WithEvents lblTTPolicy As System.Windows.Forms.Label
	Public WithEvents lblTTClaim As System.Windows.Forms.Label
	Public WithEvents fraTransactionTypes As System.Windows.Forms.GroupBox
	Private WithEvents _tabDetailTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabDetailTab As System.Windows.Forms.TabControl
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents lblTaxBand As System.Windows.Forms.Label
	Public WithEvents pnlTaxBand As System.Windows.Forms.Label
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Private WithEvents _lvwTaxBandRate_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTaxBandRate_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTaxBandRate_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTaxBandRate_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTaxBandRate_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTaxBandRate_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwTaxBandRate As System.Windows.Forms.ListView
	Public WithEvents cmdCopy As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public optBasis(4) As System.Windows.Forms.RadioButton
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tabDetailTab = New System.Windows.Forms.TabControl()
        Me._tabDetailTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.lblEffectiveDate = New System.Windows.Forms.Label()
        Me.lblCode = New System.Windows.Forms.Label()
        Me.cmdDetailOK = New System.Windows.Forms.Button()
        Me.cmdDetailCancel = New System.Windows.Forms.Button()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.txtCode = New System.Windows.Forms.TextBox()
        Me.txtEffectiveDate = New System.Windows.Forms.TextBox()
        Me.fraCalculation = New System.Windows.Forms.GroupBox()
        Me.chkUseForBackdatedNB = New System.Windows.Forms.CheckBox()
        Me.chkUseForRefundWhenexpired = New System.Windows.Forms.CheckBox()
        Me._optBasis_4 = New System.Windows.Forms.RadioButton()
        Me.chkAllowCredit = New System.Windows.Forms.CheckBox()
        Me.chkRounded = New System.Windows.Forms.CheckBox()
        Me.txtRateValue = New System.Windows.Forms.TextBox()
        Me.txtRatePercentage = New System.Windows.Forms.TextBox()
        Me.chkIsValue = New System.Windows.Forms.CheckBox()
        Me._optBasis_3 = New System.Windows.Forms.RadioButton()
        Me._optBasis_2 = New System.Windows.Forms.RadioButton()
        Me.txtSumInsuredValue = New System.Windows.Forms.TextBox()
        Me._optBasis_0 = New System.Windows.Forms.RadioButton()
        Me._optBasis_1 = New System.Windows.Forms.RadioButton()
        Me.cboCurrency = New UserControls.CurrencyLookup()
        Me.lblCurrency = New System.Windows.Forms.Label()
        Me.lblOfSI = New System.Windows.Forms.Label()
        Me.lblRate = New System.Windows.Forms.Label()
        Me.lblValue = New System.Windows.Forms.Label()
        Me.lblPer = New System.Windows.Forms.Label()
        Me.fraAffectedTransactions = New System.Windows.Forms.GroupBox()
        Me.chkREN = New System.Windows.Forms.CheckBox()
        Me.chkCANC = New System.Windows.Forms.CheckBox()
        Me.chkRMTA = New System.Windows.Forms.CheckBox()
        Me.chkAMTA = New System.Windows.Forms.CheckBox()
        Me.chkNB = New System.Windows.Forms.CheckBox()
        Me.fraFilters = New System.Windows.Forms.GroupBox()
        Me.cboCountry = New PMLookupControl.cboPMLookup()
        Me.cboState = New PMLookupControl.cboPMLookup()
        Me.cboCOB = New PMLookupControl.cboPMLookup()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblCOB = New System.Windows.Forms.Label()
        Me.lblState = New System.Windows.Forms.Label()
        Me.lblCountry = New System.Windows.Forms.Label()
        Me.fraTransactionTypes = New System.Windows.Forms.GroupBox()
        Me.chkTTRIPR = New System.Windows.Forms.CheckBox()
        Me.chkTTI = New System.Windows.Forms.CheckBox()
        Me.chkTTRIC = New System.Windows.Forms.CheckBox()
        Me.chkTTRI = New System.Windows.Forms.CheckBox()
        Me.chkTTAC = New System.Windows.Forms.CheckBox()
        Me.chkTTF = New System.Windows.Forms.CheckBox()
        Me.chkTTCR = New System.Windows.Forms.CheckBox()
        Me.chkTTCS = New System.Windows.Forms.CheckBox()
        Me.chkTTCP = New System.Windows.Forms.CheckBox()
        Me.lblTTPolicy = New System.Windows.Forms.Label()
        Me.lblTTClaim = New System.Windows.Forms.Label()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog()
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog()
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog()
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.lblTaxBand = New System.Windows.Forms.Label()
        Me.pnlTaxBand = New System.Windows.Forms.Label()
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.cmdEdit = New System.Windows.Forms.Button()
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.lvwTaxBandRate = New System.Windows.Forms.ListView()
        Me._lvwTaxBandRate_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTaxBandRate_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTaxBandRate_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTaxBandRate_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTaxBandRate_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTaxBandRate_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmdCopy = New System.Windows.Forms.Button()
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabDetailTab.SuspendLayout()
        Me._tabDetailTab_TabPage0.SuspendLayout()
        Me.fraCalculation.SuspendLayout()
        Me.fraAffectedTransactions.SuspendLayout()
        Me.fraFilters.SuspendLayout()
        Me.fraTransactionTypes.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tabDetailTab
        '
        Me.tabDetailTab.Controls.Add(Me._tabDetailTab_TabPage0)
        Me.tabDetailTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabDetailTab.ItemSize = New System.Drawing.Size(608, 18)
        Me.tabDetailTab.Location = New System.Drawing.Point(6, 0)
        Me.tabDetailTab.Multiline = True
        Me.tabDetailTab.Name = "tabDetailTab"
        Me.tabDetailTab.SelectedIndex = 0
        Me.tabDetailTab.Size = New System.Drawing.Size(613, 557)
        Me.tabDetailTab.TabIndex = 0
        '
        '_tabDetailTab_TabPage0
        '
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblDescription)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblEffectiveDate)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblCode)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdDetailOK)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdDetailCancel)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtDescription)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtCode)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtEffectiveDate)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.fraCalculation)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.fraAffectedTransactions)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.fraFilters)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.fraTransactionTypes)
        Me._tabDetailTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabDetailTab_TabPage0.Name = "_tabDetailTab_TabPage0"
        Me._tabDetailTab_TabPage0.Size = New System.Drawing.Size(605, 531)
        Me._tabDetailTab_TabPage0.TabIndex = 0
        Me._tabDetailTab_TabPage0.Text = "2 - Tax Band Rate"
        Me._tabDetailTab_TabPage0.UseVisualStyleBackColor = True
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(56, 43)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(63, 13)
        Me.lblDescription.TabIndex = 5
        Me.lblDescription.Text = "Description:"
        Me.lblDescription.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.AutoSize = True
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(348, 17)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(78, 13)
        Me.lblEffectiveDate.TabIndex = 3
        Me.lblEffectiveDate.Text = "Effective Date:"
        Me.lblEffectiveDate.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblCode
        '
        Me.lblCode.AutoSize = True
        Me.lblCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCode.Location = New System.Drawing.Point(90, 17)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCode.Size = New System.Drawing.Size(35, 13)
        Me.lblCode.TabIndex = 1
        Me.lblCode.Text = "Code:"
        Me.lblCode.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmdDetailOK
        '
        Me.cmdDetailOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDetailOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDetailOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDetailOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDetailOK.Location = New System.Drawing.Point(446, 503)
        Me.cmdDetailOK.Name = "cmdDetailOK"
        Me.cmdDetailOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDetailOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdDetailOK.TabIndex = 49
        Me.cmdDetailOK.Text = "&OK"
        Me.cmdDetailOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDetailOK.UseVisualStyleBackColor = False
        '
        'cmdDetailCancel
        '
        Me.cmdDetailCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDetailCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDetailCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDetailCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDetailCancel.Location = New System.Drawing.Point(526, 503)
        Me.cmdDetailCancel.Name = "cmdDetailCancel"
        Me.cmdDetailCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDetailCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdDetailCancel.TabIndex = 50
        Me.cmdDetailCancel.Text = "&Cancel"
        Me.cmdDetailCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDetailCancel.UseVisualStyleBackColor = False
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(134, 40)
        Me.txtDescription.MaxLength = 0
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(451, 20)
        Me.txtDescription.TabIndex = 6
        '
        'txtCode
        '
        Me.txtCode.AcceptsReturn = True
        Me.txtCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCode.Enabled = False
        Me.txtCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCode.Location = New System.Drawing.Point(134, 14)
        Me.txtCode.MaxLength = 0
        Me.txtCode.Name = "txtCode"
        Me.txtCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCode.Size = New System.Drawing.Size(140, 20)
        Me.txtCode.TabIndex = 2
        '
        'txtEffectiveDate
        '
        Me.txtEffectiveDate.AcceptsReturn = True
        Me.txtEffectiveDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtEffectiveDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEffectiveDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEffectiveDate.Location = New System.Drawing.Point(444, 14)
        Me.txtEffectiveDate.MaxLength = 0
        Me.txtEffectiveDate.Name = "txtEffectiveDate"
        Me.txtEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEffectiveDate.Size = New System.Drawing.Size(140, 20)
        Me.txtEffectiveDate.TabIndex = 4
        '
        'fraCalculation
        '
        Me.fraCalculation.BackColor = System.Drawing.SystemColors.Control
        Me.fraCalculation.Controls.Add(Me.chkUseForBackdatedNB)
        Me.fraCalculation.Controls.Add(Me.chkUseForRefundWhenexpired)
        Me.fraCalculation.Controls.Add(Me._optBasis_4)
        Me.fraCalculation.Controls.Add(Me.chkAllowCredit)
        Me.fraCalculation.Controls.Add(Me.chkRounded)
        Me.fraCalculation.Controls.Add(Me.txtRateValue)
        Me.fraCalculation.Controls.Add(Me.txtRatePercentage)
        Me.fraCalculation.Controls.Add(Me.chkIsValue)
        Me.fraCalculation.Controls.Add(Me._optBasis_3)
        Me.fraCalculation.Controls.Add(Me._optBasis_2)
        Me.fraCalculation.Controls.Add(Me.txtSumInsuredValue)
        Me.fraCalculation.Controls.Add(Me._optBasis_0)
        Me.fraCalculation.Controls.Add(Me._optBasis_1)
        Me.fraCalculation.Controls.Add(Me.cboCurrency)
        Me.fraCalculation.Controls.Add(Me.lblCurrency)
        Me.fraCalculation.Controls.Add(Me.lblOfSI)
        Me.fraCalculation.Controls.Add(Me.lblRate)
        Me.fraCalculation.Controls.Add(Me.lblValue)
        Me.fraCalculation.Controls.Add(Me.lblPer)
        Me.fraCalculation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCalculation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraCalculation.Location = New System.Drawing.Point(8, 68)
        Me.fraCalculation.Name = "fraCalculation"
        Me.fraCalculation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraCalculation.Size = New System.Drawing.Size(591, 153)
        Me.fraCalculation.TabIndex = 7
        Me.fraCalculation.TabStop = False
        Me.fraCalculation.Text = "Calculation"
        '
        'chkUseForBackdatedNB
        '
        Me.chkUseForBackdatedNB.AutoSize = True
        Me.chkUseForBackdatedNB.CheckAlign = System.Drawing.ContentAlignment.TopRight
        Me.chkUseForBackdatedNB.Location = New System.Drawing.Point(343, 69)
        Me.chkUseForBackdatedNB.Name = "chkUseForBackdatedNB"
        Me.chkUseForBackdatedNB.Size = New System.Drawing.Size(190, 17)
        Me.chkUseForBackdatedNB.TabIndex = 26
        Me.chkUseForBackdatedNB.Text = "Use for backdated NB transaction:"
        Me.chkUseForBackdatedNB.UseVisualStyleBackColor = True
        '
        'chkUseForRefundWhenexpired
        '
        Me.chkUseForRefundWhenexpired.AutoSize = True
        Me.chkUseForRefundWhenexpired.CheckAlign = System.Drawing.ContentAlignment.TopRight
        Me.chkUseForRefundWhenexpired.Location = New System.Drawing.Point(371, 46)
        Me.chkUseForRefundWhenexpired.Name = "chkUseForRefundWhenexpired"
        Me.chkUseForRefundWhenexpired.Size = New System.Drawing.Size(162, 17)
        Me.chkUseForRefundWhenexpired.TabIndex = 25
        Me.chkUseForRefundWhenexpired.Text = "Use for refund when expired:"
        Me.chkUseForRefundWhenexpired.UseVisualStyleBackColor = True
        '
        '_optBasis_4
        '
        Me._optBasis_4.BackColor = System.Drawing.SystemColors.Control
        Me._optBasis_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._optBasis_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optBasis_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optBasis_4.Location = New System.Drawing.Point(126, 35)
        Me._optBasis_4.Name = "_optBasis_4"
        Me._optBasis_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optBasis_4.Size = New System.Drawing.Size(128, 19)
        Me._optBasis_4.TabIndex = 24
        Me._optBasis_4.TabStop = True
        Me._optBasis_4.Text = "Total COB Premium"
        Me._optBasis_4.UseVisualStyleBackColor = False
        '
        'chkAllowCredit
        '
        Me.chkAllowCredit.BackColor = System.Drawing.SystemColors.Control
        Me.chkAllowCredit.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAllowCredit.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAllowCredit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAllowCredit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAllowCredit.Location = New System.Drawing.Point(32, 132)
        Me.chkAllowCredit.Name = "chkAllowCredit"
        Me.chkAllowCredit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllowCredit.Size = New System.Drawing.Size(107, 17)
        Me.chkAllowCredit.TabIndex = 23
        Me.chkAllowCredit.Text = "Allow CR Tax?"
        Me.chkAllowCredit.UseVisualStyleBackColor = False
        '
        'chkRounded
        '
        Me.chkRounded.BackColor = System.Drawing.SystemColors.Control
        Me.chkRounded.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkRounded.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRounded.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRounded.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRounded.Location = New System.Drawing.Point(322, 130)
        Me.chkRounded.Name = "chkRounded"
        Me.chkRounded.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRounded.Size = New System.Drawing.Size(85, 17)
        Me.chkRounded.TabIndex = 22
        Me.chkRounded.Text = "Rounded?"
        Me.chkRounded.UseVisualStyleBackColor = False
        '
        'txtRateValue
        '
        Me.txtRateValue.AcceptsReturn = True
        Me.txtRateValue.BackColor = System.Drawing.SystemColors.Window
        Me.txtRateValue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRateValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRateValue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRateValue.Location = New System.Drawing.Point(127, 81)
        Me.txtRateValue.MaxLength = 0
        Me.txtRateValue.Name = "txtRateValue"
        Me.txtRateValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRateValue.Size = New System.Drawing.Size(140, 20)
        Me.txtRateValue.TabIndex = 16
        Me.txtRateValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtRatePercentage
        '
        Me.txtRatePercentage.AcceptsReturn = True
        Me.txtRatePercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtRatePercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRatePercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRatePercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRatePercentage.Location = New System.Drawing.Point(127, 81)
        Me.txtRatePercentage.MaxLength = 0
        Me.txtRatePercentage.Name = "txtRatePercentage"
        Me.txtRatePercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRatePercentage.Size = New System.Drawing.Size(140, 20)
        Me.txtRatePercentage.TabIndex = 15
        Me.txtRatePercentage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtRatePercentage.Visible = False
        '
        'chkIsValue
        '
        Me.chkIsValue.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsValue.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsValue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsValue.Location = New System.Drawing.Point(60, 56)
        Me.chkIsValue.Name = "chkIsValue"
        Me.chkIsValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsValue.Size = New System.Drawing.Size(79, 19)
        Me.chkIsValue.TabIndex = 13
        Me.chkIsValue.Text = "Is Value?"
        Me.chkIsValue.UseVisualStyleBackColor = False
        '
        '_optBasis_3
        '
        Me._optBasis_3.BackColor = System.Drawing.SystemColors.Control
        Me._optBasis_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._optBasis_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optBasis_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optBasis_3.Location = New System.Drawing.Point(477, 19)
        Me._optBasis_3.Name = "_optBasis_3"
        Me._optBasis_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optBasis_3.Size = New System.Drawing.Size(99, 19)
        Me._optBasis_3.TabIndex = 12
        Me._optBasis_3.TabStop = True
        Me._optBasis_3.Text = "Running Total"
        Me._optBasis_3.UseVisualStyleBackColor = False
        '
        '_optBasis_2
        '
        Me._optBasis_2.BackColor = System.Drawing.SystemColors.Control
        Me._optBasis_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optBasis_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optBasis_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optBasis_2.Location = New System.Drawing.Point(320, 16)
        Me._optBasis_2.Name = "_optBasis_2"
        Me._optBasis_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optBasis_2.Size = New System.Drawing.Size(143, 22)
        Me._optBasis_2.TabIndex = 11
        Me._optBasis_2.TabStop = True
        Me._optBasis_2.Text = "Sum Insured Change"
        Me._optBasis_2.UseVisualStyleBackColor = False
        '
        'txtSumInsuredValue
        '
        Me.txtSumInsuredValue.AcceptsReturn = True
        Me.txtSumInsuredValue.BackColor = System.Drawing.SystemColors.Window
        Me.txtSumInsuredValue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSumInsuredValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSumInsuredValue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSumInsuredValue.Location = New System.Drawing.Point(364, 104)
        Me.txtSumInsuredValue.MaxLength = 0
        Me.txtSumInsuredValue.Name = "txtSumInsuredValue"
        Me.txtSumInsuredValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSumInsuredValue.Size = New System.Drawing.Size(140, 20)
        Me.txtSumInsuredValue.TabIndex = 18
        Me.txtSumInsuredValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        '_optBasis_0
        '
        Me._optBasis_0.BackColor = System.Drawing.SystemColors.Control
        Me._optBasis_0.Checked = True
        Me._optBasis_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optBasis_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optBasis_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optBasis_0.Location = New System.Drawing.Point(126, 19)
        Me._optBasis_0.Name = "_optBasis_0"
        Me._optBasis_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optBasis_0.Size = New System.Drawing.Size(71, 15)
        Me._optBasis_0.TabIndex = 9
        Me._optBasis_0.TabStop = True
        Me._optBasis_0.Text = "Premium"
        Me._optBasis_0.UseVisualStyleBackColor = False
        '
        '_optBasis_1
        '
        Me._optBasis_1.BackColor = System.Drawing.SystemColors.Control
        Me._optBasis_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optBasis_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optBasis_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optBasis_1.Location = New System.Drawing.Point(211, 19)
        Me._optBasis_1.Name = "_optBasis_1"
        Me._optBasis_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optBasis_1.Size = New System.Drawing.Size(95, 15)
        Me._optBasis_1.TabIndex = 10
        Me._optBasis_1.TabStop = True
        Me._optBasis_1.Text = "Sum Insured"
        Me._optBasis_1.UseVisualStyleBackColor = False
        '
        'cboCurrency
        '
        Me.cboCurrency.CompanyId = 0
        Me.cboCurrency.CurrencyId = 0
        Me.cboCurrency.DefaultCurrencyId = 0
        Me.cboCurrency.FirstItem = ""
        Me.cboCurrency.ListIndex = -1
        Me.cboCurrency.Location = New System.Drawing.Point(126, 104)
        Me.cboCurrency.Name = "cboCurrency"
        Me.cboCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actAllCurrencies
        Me.cboCurrency.Size = New System.Drawing.Size(196, 21)
        Me.cboCurrency.TabIndex = 21
        Me.cboCurrency.ToolTipText = ""
        Me.cboCurrency.WhatsThisHelpID = 0
        '
        'lblCurrency
        '
        Me.lblCurrency.AutoSize = True
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(57, 107)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(52, 13)
        Me.lblCurrency.TabIndex = 20
        Me.lblCurrency.Text = "Currency:"
        Me.lblCurrency.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblOfSI
        '
        Me.lblOfSI.AutoSize = True
        Me.lblOfSI.BackColor = System.Drawing.SystemColors.Control
        Me.lblOfSI.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOfSI.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOfSI.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOfSI.Location = New System.Drawing.Point(510, 104)
        Me.lblOfSI.Name = "lblOfSI"
        Me.lblOfSI.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOfSI.Size = New System.Drawing.Size(78, 13)
        Me.lblOfSI.TabIndex = 19
        Me.lblOfSI.Text = "of Sum Insured"
        '
        'lblRate
        '
        Me.lblRate.AutoSize = True
        Me.lblRate.BackColor = System.Drawing.SystemColors.Control
        Me.lblRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRate.Location = New System.Drawing.Point(84, 83)
        Me.lblRate.Name = "lblRate"
        Me.lblRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRate.Size = New System.Drawing.Size(33, 13)
        Me.lblRate.TabIndex = 14
        Me.lblRate.Text = "Rate:"
        Me.lblRate.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblValue
        '
        Me.lblValue.AutoSize = True
        Me.lblValue.BackColor = System.Drawing.SystemColors.Control
        Me.lblValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblValue.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblValue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblValue.Location = New System.Drawing.Point(4, 19)
        Me.lblValue.Name = "lblValue"
        Me.lblValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblValue.Size = New System.Drawing.Size(121, 13)
        Me.lblValue.TabIndex = 8
        Me.lblValue.Text = "Calculation Basis:"
        Me.lblValue.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblPer
        '
        Me.lblPer.AutoSize = True
        Me.lblPer.BackColor = System.Drawing.SystemColors.Control
        Me.lblPer.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPer.Location = New System.Drawing.Point(329, 104)
        Me.lblPer.Name = "lblPer"
        Me.lblPer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPer.Size = New System.Drawing.Size(22, 13)
        Me.lblPer.TabIndex = 17
        Me.lblPer.Text = "per"
        '
        'fraAffectedTransactions
        '
        Me.fraAffectedTransactions.BackColor = System.Drawing.SystemColors.Control
        Me.fraAffectedTransactions.Controls.Add(Me.chkREN)
        Me.fraAffectedTransactions.Controls.Add(Me.chkCANC)
        Me.fraAffectedTransactions.Controls.Add(Me.chkRMTA)
        Me.fraAffectedTransactions.Controls.Add(Me.chkAMTA)
        Me.fraAffectedTransactions.Controls.Add(Me.chkNB)
        Me.fraAffectedTransactions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAffectedTransactions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAffectedTransactions.Location = New System.Drawing.Point(8, 226)
        Me.fraAffectedTransactions.Name = "fraAffectedTransactions"
        Me.fraAffectedTransactions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAffectedTransactions.Size = New System.Drawing.Size(591, 63)
        Me.fraAffectedTransactions.TabIndex = 24
        Me.fraAffectedTransactions.TabStop = False
        Me.fraAffectedTransactions.Text = "Affected Policy Transactions"
        '
        'chkREN
        '
        Me.chkREN.BackColor = System.Drawing.SystemColors.Control
        Me.chkREN.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkREN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkREN.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkREN.Location = New System.Drawing.Point(486, 26)
        Me.chkREN.Name = "chkREN"
        Me.chkREN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkREN.Size = New System.Drawing.Size(75, 18)
        Me.chkREN.TabIndex = 29
        Me.chkREN.Text = "Renewals"
        Me.chkREN.UseVisualStyleBackColor = False
        '
        'chkCANC
        '
        Me.chkCANC.BackColor = System.Drawing.SystemColors.Control
        Me.chkCANC.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCANC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCANC.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCANC.Location = New System.Drawing.Point(373, 26)
        Me.chkCANC.Name = "chkCANC"
        Me.chkCANC.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCANC.Size = New System.Drawing.Size(91, 18)
        Me.chkCANC.TabIndex = 28
        Me.chkCANC.Text = "Cancellation"
        Me.chkCANC.UseVisualStyleBackColor = False
        '
        'chkRMTA
        '
        Me.chkRMTA.BackColor = System.Drawing.SystemColors.Control
        Me.chkRMTA.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRMTA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRMTA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRMTA.Location = New System.Drawing.Point(260, 26)
        Me.chkRMTA.Name = "chkRMTA"
        Me.chkRMTA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRMTA.Size = New System.Drawing.Size(87, 18)
        Me.chkRMTA.TabIndex = 27
        Me.chkRMTA.Text = "Return MTA"
        Me.chkRMTA.UseVisualStyleBackColor = False
        '
        'chkAMTA
        '
        Me.chkAMTA.BackColor = System.Drawing.SystemColors.Control
        Me.chkAMTA.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAMTA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAMTA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAMTA.Location = New System.Drawing.Point(127, 26)
        Me.chkAMTA.Name = "chkAMTA"
        Me.chkAMTA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAMTA.Size = New System.Drawing.Size(105, 18)
        Me.chkAMTA.TabIndex = 26
        Me.chkAMTA.Text = "Additional MTA"
        Me.chkAMTA.UseVisualStyleBackColor = False
        '
        'chkNB
        '
        Me.chkNB.BackColor = System.Drawing.SystemColors.Control
        Me.chkNB.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkNB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkNB.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkNB.Location = New System.Drawing.Point(14, 26)
        Me.chkNB.Name = "chkNB"
        Me.chkNB.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkNB.Size = New System.Drawing.Size(99, 18)
        Me.chkNB.TabIndex = 25
        Me.chkNB.Text = "New Business"
        Me.chkNB.UseVisualStyleBackColor = False
        '
        'fraFilters
        '
        Me.fraFilters.BackColor = System.Drawing.SystemColors.Control
        Me.fraFilters.Controls.Add(Me.cboCountry)
        Me.fraFilters.Controls.Add(Me.cboState)
        Me.fraFilters.Controls.Add(Me.cboCOB)
        Me.fraFilters.Controls.Add(Me.Label1)
        Me.fraFilters.Controls.Add(Me.lblCOB)
        Me.fraFilters.Controls.Add(Me.lblState)
        Me.fraFilters.Controls.Add(Me.lblCountry)
        Me.fraFilters.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraFilters.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFilters.Location = New System.Drawing.Point(8, 402)
        Me.fraFilters.Name = "fraFilters"
        Me.fraFilters.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFilters.Size = New System.Drawing.Size(591, 95)
        Me.fraFilters.TabIndex = 41
        Me.fraFilters.TabStop = False
        Me.fraFilters.Text = "Filters"
        '
        'cboCountry
        '
        Me.cboCountry.DefaultItemId = 0
        Me.cboCountry.FirstItem = ""
        Me.cboCountry.ItemId = 0
        Me.cboCountry.ListIndex = -1
        Me.cboCountry.Location = New System.Drawing.Point(126, 20)
        Me.cboCountry.Name = "cboCountry"
        Me.cboCountry.PMLookupProductFamily = 1
        Me.cboCountry.SingleItemId = 0
        Me.cboCountry.Size = New System.Drawing.Size(180, 21)
        Me.cboCountry.Sorted = True
        Me.cboCountry.TabIndex = 43
        Me.cboCountry.TableName = "Country"
        Me.cboCountry.ToolTipText = ""
        Me.cboCountry.WhereClause = ""
        '
        'cboState
        '
        Me.cboState.DefaultItemId = 0
        Me.cboState.FirstItem = ""
        Me.cboState.ItemId = 0
        Me.cboState.ListIndex = -1
        Me.cboState.Location = New System.Drawing.Point(398, 20)
        Me.cboState.Name = "cboState"
        Me.cboState.PMLookupProductFamily = 1
        Me.cboState.SingleItemId = 0
        Me.cboState.Size = New System.Drawing.Size(180, 21)
        Me.cboState.Sorted = True
        Me.cboState.TabIndex = 45
        Me.cboState.TableName = "State"
        Me.cboState.ToolTipText = ""
        Me.cboState.WhereClause = "country_id is null"
        '
        'cboCOB
        '
        Me.cboCOB.DefaultItemId = 0
        Me.cboCOB.FirstItem = "(Any)"
        Me.cboCOB.ItemId = 0
        Me.cboCOB.ListIndex = -1
        Me.cboCOB.Location = New System.Drawing.Point(126, 48)
        Me.cboCOB.Name = "cboCOB"
        Me.cboCOB.PMLookupProductFamily = 1
        Me.cboCOB.SingleItemId = 0
        Me.cboCOB.Size = New System.Drawing.Size(180, 21)
        Me.cboCOB.Sorted = True
        Me.cboCOB.TabIndex = 47
        Me.cboCOB.TableName = "Class_of_Business"
        Me.cboCOB.ToolTipText = ""
        Me.cboCOB.WhereClause = ""
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label1.Location = New System.Drawing.Point(312, 48)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(265, 41)
        Me.Label1.TabIndex = 48
        Me.Label1.Text = "IMPORTANT NB: These filters do not apply when selecting Claim Taxes for Payments," & _
    " Salvage or Third Party Recovery"
        '
        'lblCOB
        '
        Me.lblCOB.AutoSize = True
        Me.lblCOB.BackColor = System.Drawing.SystemColors.Control
        Me.lblCOB.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCOB.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCOB.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCOB.Location = New System.Drawing.Point(5, 52)
        Me.lblCOB.Name = "lblCOB"
        Me.lblCOB.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCOB.Size = New System.Drawing.Size(123, 13)
        Me.lblCOB.TabIndex = 46
        Me.lblCOB.Text = "Class of Business:"
        Me.lblCOB.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblState
        '
        Me.lblState.AutoSize = True
        Me.lblState.BackColor = System.Drawing.SystemColors.Control
        Me.lblState.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblState.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblState.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblState.Location = New System.Drawing.Point(358, 24)
        Me.lblState.Name = "lblState"
        Me.lblState.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblState.Size = New System.Drawing.Size(35, 13)
        Me.lblState.TabIndex = 44
        Me.lblState.Text = "State:"
        Me.lblState.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblCountry
        '
        Me.lblCountry.AutoSize = True
        Me.lblCountry.BackColor = System.Drawing.SystemColors.Control
        Me.lblCountry.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCountry.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCountry.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCountry.Location = New System.Drawing.Point(70, 24)
        Me.lblCountry.Name = "lblCountry"
        Me.lblCountry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCountry.Size = New System.Drawing.Size(46, 13)
        Me.lblCountry.TabIndex = 42
        Me.lblCountry.Text = "Country:"
        Me.lblCountry.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'fraTransactionTypes
        '
        Me.fraTransactionTypes.BackColor = System.Drawing.SystemColors.Control
        Me.fraTransactionTypes.Controls.Add(Me.chkTTRIPR)
        Me.fraTransactionTypes.Controls.Add(Me.chkTTI)
        Me.fraTransactionTypes.Controls.Add(Me.chkTTRIC)
        Me.fraTransactionTypes.Controls.Add(Me.chkTTRI)
        Me.fraTransactionTypes.Controls.Add(Me.chkTTAC)
        Me.fraTransactionTypes.Controls.Add(Me.chkTTF)
        Me.fraTransactionTypes.Controls.Add(Me.chkTTCR)
        Me.fraTransactionTypes.Controls.Add(Me.chkTTCS)
        Me.fraTransactionTypes.Controls.Add(Me.chkTTCP)
        Me.fraTransactionTypes.Controls.Add(Me.lblTTPolicy)
        Me.fraTransactionTypes.Controls.Add(Me.lblTTClaim)
        Me.fraTransactionTypes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTransactionTypes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTransactionTypes.Location = New System.Drawing.Point(8, 294)
        Me.fraTransactionTypes.Name = "fraTransactionTypes"
        Me.fraTransactionTypes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTransactionTypes.Size = New System.Drawing.Size(591, 101)
        Me.fraTransactionTypes.TabIndex = 30
        Me.fraTransactionTypes.TabStop = False
        Me.fraTransactionTypes.Text = "Other Transaction Types"
        '
        'chkTTRIPR
        '
        Me.chkTTRIPR.BackColor = System.Drawing.SystemColors.Control
        Me.chkTTRIPR.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkTTRIPR.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTTRIPR.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkTTRIPR.Location = New System.Drawing.Point(60, 50)
        Me.chkTTRIPR.Name = "chkTTRIPR"
        Me.chkTTRIPR.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkTTRIPR.Size = New System.Drawing.Size(152, 18)
        Me.chkTTRIPR.TabIndex = 41
        Me.chkTTRIPR.Text = "RI Payments/Recoveries"
        Me.chkTTRIPR.UseVisualStyleBackColor = False
        '
        'chkTTI
        '
        Me.chkTTI.BackColor = System.Drawing.SystemColors.Control
        Me.chkTTI.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkTTI.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTTI.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkTTI.Location = New System.Drawing.Point(494, 26)
        Me.chkTTI.Name = "chkTTI"
        Me.chkTTI.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkTTI.Size = New System.Drawing.Size(91, 18)
        Me.chkTTI.TabIndex = 36
        Me.chkTTI.Text = "Instalments"
        Me.chkTTI.UseVisualStyleBackColor = False
        '
        'chkTTRIC
        '
        Me.chkTTRIC.BackColor = System.Drawing.SystemColors.Control
        Me.chkTTRIC.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkTTRIC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTTRIC.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkTTRIC.Location = New System.Drawing.Point(165, 26)
        Me.chkTTRIC.Name = "chkTTRIC"
        Me.chkTTRIC.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkTTRIC.Size = New System.Drawing.Size(107, 18)
        Me.chkTTRIC.TabIndex = 33
        Me.chkTTRIC.Text = "RI Commission"
        Me.chkTTRIC.UseVisualStyleBackColor = False
        '
        'chkTTRI
        '
        Me.chkTTRI.BackColor = System.Drawing.SystemColors.Control
        Me.chkTTRI.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkTTRI.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTTRI.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkTTRI.Location = New System.Drawing.Point(60, 26)
        Me.chkTTRI.Name = "chkTTRI"
        Me.chkTTRI.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkTTRI.Size = New System.Drawing.Size(91, 18)
        Me.chkTTRI.TabIndex = 32
        Me.chkTTRI.Text = "Reinsurance"
        Me.chkTTRI.UseVisualStyleBackColor = False
        '
        'chkTTAC
        '
        Me.chkTTAC.BackColor = System.Drawing.SystemColors.Control
        Me.chkTTAC.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkTTAC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTTAC.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkTTAC.Location = New System.Drawing.Point(284, 26)
        Me.chkTTAC.Name = "chkTTAC"
        Me.chkTTAC.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkTTAC.Size = New System.Drawing.Size(127, 18)
        Me.chkTTAC.TabIndex = 34
        Me.chkTTAC.Text = "Agent Commission"
        Me.chkTTAC.UseVisualStyleBackColor = False
        '
        'chkTTF
        '
        Me.chkTTF.BackColor = System.Drawing.SystemColors.Control
        Me.chkTTF.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkTTF.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTTF.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkTTF.Location = New System.Drawing.Point(430, 26)
        Me.chkTTF.Name = "chkTTF"
        Me.chkTTF.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkTTF.Size = New System.Drawing.Size(47, 18)
        Me.chkTTF.TabIndex = 35
        Me.chkTTF.Text = "Fees"
        Me.chkTTF.UseVisualStyleBackColor = False
        '
        'chkTTCR
        '
        Me.chkTTCR.BackColor = System.Drawing.SystemColors.Control
        Me.chkTTCR.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkTTCR.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTTCR.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkTTCR.Location = New System.Drawing.Point(284, 75)
        Me.chkTTCR.Name = "chkTTCR"
        Me.chkTTCR.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkTTCR.Size = New System.Drawing.Size(141, 19)
        Me.chkTTCR.TabIndex = 40
        Me.chkTTCR.Text = "Third Party Recovery"
        Me.chkTTCR.UseVisualStyleBackColor = False
        '
        'chkTTCS
        '
        Me.chkTTCS.BackColor = System.Drawing.SystemColors.Control
        Me.chkTTCS.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkTTCS.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTTCS.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkTTCS.Location = New System.Drawing.Point(165, 75)
        Me.chkTTCS.Name = "chkTTCS"
        Me.chkTTCS.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkTTCS.Size = New System.Drawing.Size(67, 19)
        Me.chkTTCS.TabIndex = 39
        Me.chkTTCS.Text = "Salvage"
        Me.chkTTCS.UseVisualStyleBackColor = False
        '
        'chkTTCP
        '
        Me.chkTTCP.BackColor = System.Drawing.SystemColors.Control
        Me.chkTTCP.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkTTCP.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTTCP.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkTTCP.Location = New System.Drawing.Point(60, 75)
        Me.chkTTCP.Name = "chkTTCP"
        Me.chkTTCP.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkTTCP.Size = New System.Drawing.Size(77, 19)
        Me.chkTTCP.TabIndex = 38
        Me.chkTTCP.Text = "Payments"
        Me.chkTTCP.UseVisualStyleBackColor = False
        '
        'lblTTPolicy
        '
        Me.lblTTPolicy.AutoSize = True
        Me.lblTTPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.lblTTPolicy.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTTPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTTPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTTPolicy.Location = New System.Drawing.Point(11, 26)
        Me.lblTTPolicy.Name = "lblTTPolicy"
        Me.lblTTPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTTPolicy.Size = New System.Drawing.Size(38, 13)
        Me.lblTTPolicy.TabIndex = 31
        Me.lblTTPolicy.Text = "Policy:"
        Me.lblTTPolicy.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblTTClaim
        '
        Me.lblTTClaim.AutoSize = True
        Me.lblTTClaim.BackColor = System.Drawing.SystemColors.Control
        Me.lblTTClaim.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTTClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTTClaim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTTClaim.Location = New System.Drawing.Point(6, 77)
        Me.lblTTClaim.Name = "lblTTClaim"
        Me.lblTTClaim.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTTClaim.Size = New System.Drawing.Size(35, 13)
        Me.lblTTClaim.TabIndex = 37
        Me.lblTTClaim.Text = "Claim:"
        Me.lblTTClaim.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(542, 568)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 61
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(462, 568)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 60
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(382, 568)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 59
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(608, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(6, 0)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(613, 533)
        Me.tabMainTab.TabIndex = 51
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblTaxBand)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pnlTaxBand)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdDelete)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdEdit)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAdd)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwTaxBandRate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdCopy)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(605, 507)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Tax Band Rates"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'lblTaxBand
        '
        Me.lblTaxBand.AutoSize = True
        Me.lblTaxBand.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxBand.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxBand.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxBand.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxBand.Location = New System.Drawing.Point(22, 16)
        Me.lblTaxBand.Name = "lblTaxBand"
        Me.lblTaxBand.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxBand.Size = New System.Drawing.Size(56, 13)
        Me.lblTaxBand.TabIndex = 52
        Me.lblTaxBand.Text = "Tax Band:"
        '
        'pnlTaxBand
        '
        Me.pnlTaxBand.BackColor = System.Drawing.SystemColors.Control
        Me.pnlTaxBand.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlTaxBand.Cursor = System.Windows.Forms.Cursors.Default
        Me.pnlTaxBand.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlTaxBand.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlTaxBand.Location = New System.Drawing.Point(100, 14)
        Me.pnlTaxBand.Name = "pnlTaxBand"
        Me.pnlTaxBand.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlTaxBand.Size = New System.Drawing.Size(497, 19)
        Me.pnlTaxBand.TabIndex = 53
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(526, 470)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 58
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(446, 470)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 57
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(286, 470)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 55
        Me.cmdAdd.Text = "&New"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'lvwTaxBandRate
        '
        Me.lvwTaxBandRate.BackColor = System.Drawing.SystemColors.Window
        Me.lvwTaxBandRate.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwTaxBandRate, "")
        Me.lvwTaxBandRate.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwTaxBandRate_ColumnHeader_1, Me._lvwTaxBandRate_ColumnHeader_2, Me._lvwTaxBandRate_ColumnHeader_3, Me._lvwTaxBandRate_ColumnHeader_4, Me._lvwTaxBandRate_ColumnHeader_5, Me._lvwTaxBandRate_ColumnHeader_6})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwTaxBandRate, True)
        Me.lvwTaxBandRate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwTaxBandRate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwTaxBandRate.FullRowSelect = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwTaxBandRate, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwTaxBandRate, "")
        Me.lvwTaxBandRate.Location = New System.Drawing.Point(8, 42)
        Me.lvwTaxBandRate.Name = "lvwTaxBandRate"
        Me.lvwTaxBandRate.Size = New System.Drawing.Size(591, 419)
        Me.listViewHelper1.SetSmallIcons(Me.lvwTaxBandRate, "")
        Me.listViewHelper1.SetSorted(Me.lvwTaxBandRate, False)
        Me.listViewHelper1.SetSortKey(Me.lvwTaxBandRate, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwTaxBandRate, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwTaxBandRate.TabIndex = 54
        Me.lvwTaxBandRate.UseCompatibleStateImageBehavior = False
        Me.lvwTaxBandRate.View = System.Windows.Forms.View.Details
        '
        '_lvwTaxBandRate_ColumnHeader_1
        '
        Me._lvwTaxBandRate_ColumnHeader_1.Text = "Description"
        Me._lvwTaxBandRate_ColumnHeader_1.Width = 201
        '
        '_lvwTaxBandRate_ColumnHeader_2
        '
        Me._lvwTaxBandRate_ColumnHeader_2.Text = "Effective Date"
        Me._lvwTaxBandRate_ColumnHeader_2.Width = 97
        '
        '_lvwTaxBandRate_ColumnHeader_3
        '
        Me._lvwTaxBandRate_ColumnHeader_3.Text = "Rate"
        Me._lvwTaxBandRate_ColumnHeader_3.Width = 97
        '
        '_lvwTaxBandRate_ColumnHeader_4
        '
        Me._lvwTaxBandRate_ColumnHeader_4.Text = "Class of Business"
        Me._lvwTaxBandRate_ColumnHeader_4.Width = 161
        '
        '_lvwTaxBandRate_ColumnHeader_5
        '
        Me._lvwTaxBandRate_ColumnHeader_5.Text = "Country"
        Me._lvwTaxBandRate_ColumnHeader_5.Width = 121
        '
        '_lvwTaxBandRate_ColumnHeader_6
        '
        Me._lvwTaxBandRate_ColumnHeader_6.Text = "State"
        Me._lvwTaxBandRate_ColumnHeader_6.Width = 121
        '
        'cmdCopy
        '
        Me.cmdCopy.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCopy.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCopy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCopy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCopy.Location = New System.Drawing.Point(366, 470)
        Me.cmdCopy.Name = "cmdCopy"
        Me.cmdCopy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCopy.Size = New System.Drawing.Size(73, 22)
        Me.cmdCopy.TabIndex = 56
        Me.cmdCopy.Text = "C&opy"
        Me.cmdCopy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCopy.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(624, 597)
        Me.Controls.Add(Me.tabDetailTab)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Tax Band Rates"
        Me.tabDetailTab.ResumeLayout(False)
        Me._tabDetailTab_TabPage0.ResumeLayout(False)
        Me._tabDetailTab_TabPage0.PerformLayout()
        Me.fraCalculation.ResumeLayout(False)
        Me.fraCalculation.PerformLayout()
        Me.fraAffectedTransactions.ResumeLayout(False)
        Me.fraFilters.ResumeLayout(False)
        Me.fraFilters.PerformLayout()
        Me.fraTransactionTypes.ResumeLayout(False)
        Me.fraTransactionTypes.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Sub InitializeoptBasis()
        Me.optBasis(3) = _optBasis_3
        Me.optBasis(2) = _optBasis_2
        Me.optBasis(0) = _optBasis_0
        Me.optBasis(1) = _optBasis_1
        Me.optBasis(4) = _optBasis_4
    End Sub
    Private WithEvents _optBasis_4 As System.Windows.Forms.RadioButton
    Friend WithEvents chkUseForBackdatedNB As System.Windows.Forms.CheckBox
    Friend WithEvents chkUseForRefundWhenexpired As System.Windows.Forms.CheckBox
    Public WithEvents chkTTRIPR As System.Windows.Forms.CheckBox
#End Region 
End Class
