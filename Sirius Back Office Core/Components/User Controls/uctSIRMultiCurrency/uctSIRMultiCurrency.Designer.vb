<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctSIRMultiCurrency
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		UserControl_Initialize()
	End Sub
    'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Friend WithEvents txtLossCurrencyAmount As System.Windows.Forms.TextBox
	Friend WithEvents cboLossCurrency As PMLookupControl.cboPMLookup
	Friend WithEvents lblLossCurrency As System.Windows.Forms.Label
	Friend WithEvents lblLossCurrencyAmount As System.Windows.Forms.Label
	Friend WithEvents fraLossCurrency As System.Windows.Forms.GroupBox
	Friend WithEvents txtSystemCurrencyAmount As System.Windows.Forms.TextBox
	Friend WithEvents txtSystemCurrencyRate As System.Windows.Forms.TextBox
	Friend WithEvents cboSystemCurrency As PMLookupControl.cboPMLookup
	Friend WithEvents lblSystemCurrencyAmount As System.Windows.Forms.Label
	Friend WithEvents lblSystemCurrencyRate As System.Windows.Forms.Label
	Friend WithEvents lblSystemCurrency As System.Windows.Forms.Label
	Friend WithEvents fraSystemCurrency As System.Windows.Forms.GroupBox
	Friend WithEvents txtAccountCurrencyAmount As System.Windows.Forms.TextBox
	Friend WithEvents txtAccountCurrencyRate As System.Windows.Forms.TextBox
	Friend WithEvents cboAccountCurrency As PMLookupControl.cboPMLookup
	Friend WithEvents lblAccountCurrencyAmount As System.Windows.Forms.Label
	Friend WithEvents lblAccountCurrencyRate As System.Windows.Forms.Label
	Friend WithEvents lblAccountCurrency As System.Windows.Forms.Label
	Friend WithEvents fraAccountCurrency As System.Windows.Forms.GroupBox
	Friend WithEvents cboReason As PMLookupControl.cboPMLookup
	Friend WithEvents lblReason As System.Windows.Forms.Label
	Friend WithEvents fraRateOverrideReason As System.Windows.Forms.GroupBox
	Friend WithEvents txtBaseCurrencyAmount As System.Windows.Forms.TextBox
	Friend WithEvents txtBaseCurrencyRate As System.Windows.Forms.TextBox
	Friend WithEvents cboBaseCurrency As PMLookupControl.cboPMLookup
	Friend WithEvents lblBaseCurrencyAmount As System.Windows.Forms.Label
	Friend WithEvents lblBaseCurrencyRate As System.Windows.Forms.Label
	Friend WithEvents lblBaseCurrency As System.Windows.Forms.Label
	Friend WithEvents fraBaseCurrency As System.Windows.Forms.GroupBox
	Friend WithEvents txtTransactionValue As System.Windows.Forms.TextBox
	Friend WithEvents cboTransactionCurrency As PMLookupControl.cboPMLookup
	Friend WithEvents cboEffectiveDate As System.Windows.Forms.DateTimePicker
	Friend WithEvents lblEffectiveDate As System.Windows.Forms.Label
	Friend WithEvents lblTransactionCurrency As System.Windows.Forms.Label
	Friend WithEvents lblTransactionValue As System.Windows.Forms.Label
	Friend WithEvents fraTransaction As System.Windows.Forms.GroupBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.fraLossCurrency = New System.Windows.Forms.GroupBox
        Me.txtLossCurrencyAmount = New System.Windows.Forms.TextBox
        Me.cboLossCurrency = New PMLookupControl.cboPMLookup
        Me.lblLossCurrency = New System.Windows.Forms.Label
        Me.lblLossCurrencyAmount = New System.Windows.Forms.Label
        Me.fraSystemCurrency = New System.Windows.Forms.GroupBox
        Me.txtSystemCurrencyAmount = New System.Windows.Forms.TextBox
        Me.txtSystemCurrencyRate = New System.Windows.Forms.TextBox
        Me.cboSystemCurrency = New PMLookupControl.cboPMLookup
        Me.lblSystemCurrencyAmount = New System.Windows.Forms.Label
        Me.lblSystemCurrencyRate = New System.Windows.Forms.Label
        Me.lblSystemCurrency = New System.Windows.Forms.Label
        Me.fraAccountCurrency = New System.Windows.Forms.GroupBox
        Me.txtAccountCurrencyAmount = New System.Windows.Forms.TextBox
        Me.txtAccountCurrencyRate = New System.Windows.Forms.TextBox
        Me.cboAccountCurrency = New PMLookupControl.cboPMLookup
        Me.lblAccountCurrencyAmount = New System.Windows.Forms.Label
        Me.lblAccountCurrencyRate = New System.Windows.Forms.Label
        Me.lblAccountCurrency = New System.Windows.Forms.Label
        Me.fraRateOverrideReason = New System.Windows.Forms.GroupBox
        Me.cboReason = New PMLookupControl.cboPMLookup
        Me.lblReason = New System.Windows.Forms.Label
        Me.fraBaseCurrency = New System.Windows.Forms.GroupBox
        Me.txtBaseCurrencyAmount = New System.Windows.Forms.TextBox
        Me.txtBaseCurrencyRate = New System.Windows.Forms.TextBox
        Me.cboBaseCurrency = New PMLookupControl.cboPMLookup
        Me.lblBaseCurrencyAmount = New System.Windows.Forms.Label
        Me.lblBaseCurrencyRate = New System.Windows.Forms.Label
        Me.lblBaseCurrency = New System.Windows.Forms.Label
        Me.fraTransaction = New System.Windows.Forms.GroupBox
        Me.txtTransactionValue = New System.Windows.Forms.TextBox
        Me.cboTransactionCurrency = New PMLookupControl.cboPMLookup
        Me.cboEffectiveDate = New System.Windows.Forms.DateTimePicker
        Me.lblEffectiveDate = New System.Windows.Forms.Label
        Me.lblTransactionCurrency = New System.Windows.Forms.Label
        Me.lblTransactionValue = New System.Windows.Forms.Label
        Me.fraLossCurrency.SuspendLayout()
        Me.fraSystemCurrency.SuspendLayout()
        Me.fraAccountCurrency.SuspendLayout()
        Me.fraRateOverrideReason.SuspendLayout()
        Me.fraBaseCurrency.SuspendLayout()
        Me.fraTransaction.SuspendLayout()
        Me.SuspendLayout()
        '
        'fraLossCurrency
        '
        Me.fraLossCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.fraLossCurrency.Controls.Add(Me.txtLossCurrencyAmount)
        Me.fraLossCurrency.Controls.Add(Me.cboLossCurrency)
        Me.fraLossCurrency.Controls.Add(Me.lblLossCurrency)
        Me.fraLossCurrency.Controls.Add(Me.lblLossCurrencyAmount)
        Me.fraLossCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraLossCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraLossCurrency.Location = New System.Drawing.Point(8, 240)
        Me.fraLossCurrency.Name = "fraLossCurrency"
        Me.fraLossCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraLossCurrency.Size = New System.Drawing.Size(321, 81)
        Me.fraLossCurrency.TabIndex = 31
        Me.fraLossCurrency.TabStop = False
        Me.fraLossCurrency.Text = "Loss Currency"
        Me.fraLossCurrency.Visible = False
        '
        'txtLossCurrencyAmount
        '
        Me.txtLossCurrencyAmount.AcceptsReturn = True
        Me.txtLossCurrencyAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtLossCurrencyAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLossCurrencyAmount.Enabled = False
        Me.txtLossCurrencyAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLossCurrencyAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLossCurrencyAmount.Location = New System.Drawing.Point(160, 48)
        Me.txtLossCurrencyAmount.MaxLength = 0
        Me.txtLossCurrencyAmount.Name = "txtLossCurrencyAmount"
        Me.txtLossCurrencyAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLossCurrencyAmount.Size = New System.Drawing.Size(145, 21)
        Me.txtLossCurrencyAmount.TabIndex = 34
        Me.txtLossCurrencyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cboLossCurrency
        '
        Me.cboLossCurrency.DefaultItemId = 0
        Me.cboLossCurrency.Enabled = False
        Me.cboLossCurrency.ItemId = 0
        Me.cboLossCurrency.ListIndex = -1
        Me.cboLossCurrency.Location = New System.Drawing.Point(160, 20)
        Me.cboLossCurrency.Name = "cboLossCurrency"
        Me.cboLossCurrency.PMLookupProductFamily = 1
        Me.cboLossCurrency.SingleItemId = 0
        Me.cboLossCurrency.Size = New System.Drawing.Size(145, 21)
        Me.cboLossCurrency.Sorted = True
        Me.cboLossCurrency.TabIndex = 32
        Me.cboLossCurrency.TableName = "Currency"
        Me.cboLossCurrency.ToolTipText = ""
        Me.cboLossCurrency.WhereClause = ""
        '
        'lblLossCurrency
        '
        Me.lblLossCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblLossCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLossCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLossCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLossCurrency.Location = New System.Drawing.Point(16, 22)
        Me.lblLossCurrency.Name = "lblLossCurrency"
        Me.lblLossCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLossCurrency.Size = New System.Drawing.Size(145, 17)
        Me.lblLossCurrency.TabIndex = 33
        Me.lblLossCurrency.Text = "Loss Currency:"
        '
        'lblLossCurrencyAmount
        '
        Me.lblLossCurrencyAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblLossCurrencyAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLossCurrencyAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLossCurrencyAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLossCurrencyAmount.Location = New System.Drawing.Point(16, 50)
        Me.lblLossCurrencyAmount.Name = "lblLossCurrencyAmount"
        Me.lblLossCurrencyAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLossCurrencyAmount.Size = New System.Drawing.Size(145, 17)
        Me.lblLossCurrencyAmount.TabIndex = 35
        Me.lblLossCurrencyAmount.Text = "Loss Currency Amount:"
        '
        'fraSystemCurrency
        '
        Me.fraSystemCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.fraSystemCurrency.Controls.Add(Me.txtSystemCurrencyAmount)
        Me.fraSystemCurrency.Controls.Add(Me.txtSystemCurrencyRate)
        Me.fraSystemCurrency.Controls.Add(Me.cboSystemCurrency)
        Me.fraSystemCurrency.Controls.Add(Me.lblSystemCurrencyAmount)
        Me.fraSystemCurrency.Controls.Add(Me.lblSystemCurrencyRate)
        Me.fraSystemCurrency.Controls.Add(Me.lblSystemCurrency)
        Me.fraSystemCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraSystemCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraSystemCurrency.Location = New System.Drawing.Point(336, 126)
        Me.fraSystemCurrency.Name = "fraSystemCurrency"
        Me.fraSystemCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraSystemCurrency.Size = New System.Drawing.Size(321, 107)
        Me.fraSystemCurrency.TabIndex = 24
        Me.fraSystemCurrency.TabStop = False
        Me.fraSystemCurrency.Text = "System Currency"
        '
        'txtSystemCurrencyAmount
        '
        Me.txtSystemCurrencyAmount.AcceptsReturn = True
        Me.txtSystemCurrencyAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtSystemCurrencyAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSystemCurrencyAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSystemCurrencyAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSystemCurrencyAmount.Location = New System.Drawing.Point(172, 74)
        Me.txtSystemCurrencyAmount.MaxLength = 0
        Me.txtSystemCurrencyAmount.Name = "txtSystemCurrencyAmount"
        Me.txtSystemCurrencyAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSystemCurrencyAmount.Size = New System.Drawing.Size(133, 21)
        Me.txtSystemCurrencyAmount.TabIndex = 29
        Me.txtSystemCurrencyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtSystemCurrencyRate
        '
        Me.txtSystemCurrencyRate.AcceptsReturn = True
        Me.txtSystemCurrencyRate.BackColor = System.Drawing.SystemColors.Window
        Me.txtSystemCurrencyRate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSystemCurrencyRate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSystemCurrencyRate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSystemCurrencyRate.Location = New System.Drawing.Point(224, 48)
        Me.txtSystemCurrencyRate.MaxLength = 0
        Me.txtSystemCurrencyRate.Name = "txtSystemCurrencyRate"
        Me.txtSystemCurrencyRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSystemCurrencyRate.Size = New System.Drawing.Size(81, 21)
        Me.txtSystemCurrencyRate.TabIndex = 27
        Me.txtSystemCurrencyRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cboSystemCurrency
        '
        Me.cboSystemCurrency.DefaultItemId = 0
        Me.cboSystemCurrency.ItemId = 0
        Me.cboSystemCurrency.ListIndex = -1
        Me.cboSystemCurrency.Location = New System.Drawing.Point(160, 20)
        Me.cboSystemCurrency.Name = "cboSystemCurrency"
        Me.cboSystemCurrency.PMLookupProductFamily = 1
        Me.cboSystemCurrency.SingleItemId = 0
        Me.cboSystemCurrency.Size = New System.Drawing.Size(145, 21)
        Me.cboSystemCurrency.Sorted = True
        Me.cboSystemCurrency.TabIndex = 25
        Me.cboSystemCurrency.TableName = "Currency"
        Me.cboSystemCurrency.ToolTipText = ""
        Me.cboSystemCurrency.WhereClause = ""
        '
        'lblSystemCurrencyAmount
        '
        Me.lblSystemCurrencyAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblSystemCurrencyAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSystemCurrencyAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSystemCurrencyAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSystemCurrencyAmount.Location = New System.Drawing.Point(16, 76)
        Me.lblSystemCurrencyAmount.Name = "lblSystemCurrencyAmount"
        Me.lblSystemCurrencyAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSystemCurrencyAmount.Size = New System.Drawing.Size(161, 17)
        Me.lblSystemCurrencyAmount.TabIndex = 30
        Me.lblSystemCurrencyAmount.Text = "System Currency Amount:"
        '
        'lblSystemCurrencyRate
        '
        Me.lblSystemCurrencyRate.BackColor = System.Drawing.SystemColors.Control
        Me.lblSystemCurrencyRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSystemCurrencyRate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSystemCurrencyRate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSystemCurrencyRate.Location = New System.Drawing.Point(16, 50)
        Me.lblSystemCurrencyRate.Name = "lblSystemCurrencyRate"
        Me.lblSystemCurrencyRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSystemCurrencyRate.Size = New System.Drawing.Size(145, 17)
        Me.lblSystemCurrencyRate.TabIndex = 28
        Me.lblSystemCurrencyRate.Text = "System Currency Rate:"
        '
        'lblSystemCurrency
        '
        Me.lblSystemCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblSystemCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSystemCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSystemCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSystemCurrency.Location = New System.Drawing.Point(16, 22)
        Me.lblSystemCurrency.Name = "lblSystemCurrency"
        Me.lblSystemCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSystemCurrency.Size = New System.Drawing.Size(145, 17)
        Me.lblSystemCurrency.TabIndex = 26
        Me.lblSystemCurrency.Text = "System Currency:"
        '
        'fraAccountCurrency
        '
        Me.fraAccountCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.fraAccountCurrency.Controls.Add(Me.txtAccountCurrencyAmount)
        Me.fraAccountCurrency.Controls.Add(Me.txtAccountCurrencyRate)
        Me.fraAccountCurrency.Controls.Add(Me.cboAccountCurrency)
        Me.fraAccountCurrency.Controls.Add(Me.lblAccountCurrencyAmount)
        Me.fraAccountCurrency.Controls.Add(Me.lblAccountCurrencyRate)
        Me.fraAccountCurrency.Controls.Add(Me.lblAccountCurrency)
        Me.fraAccountCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAccountCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAccountCurrency.Location = New System.Drawing.Point(336, 8)
        Me.fraAccountCurrency.Name = "fraAccountCurrency"
        Me.fraAccountCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAccountCurrency.Size = New System.Drawing.Size(321, 109)
        Me.fraAccountCurrency.TabIndex = 17
        Me.fraAccountCurrency.TabStop = False
        Me.fraAccountCurrency.Text = "Account Currency"
        '
        'txtAccountCurrencyAmount
        '
        Me.txtAccountCurrencyAmount.AcceptsReturn = True
        Me.txtAccountCurrencyAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccountCurrencyAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccountCurrencyAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountCurrencyAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccountCurrencyAmount.Location = New System.Drawing.Point(174, 74)
        Me.txtAccountCurrencyAmount.MaxLength = 0
        Me.txtAccountCurrencyAmount.Name = "txtAccountCurrencyAmount"
        Me.txtAccountCurrencyAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccountCurrencyAmount.Size = New System.Drawing.Size(133, 21)
        Me.txtAccountCurrencyAmount.TabIndex = 22
        Me.txtAccountCurrencyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAccountCurrencyRate
        '
        Me.txtAccountCurrencyRate.AcceptsReturn = True
        Me.txtAccountCurrencyRate.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccountCurrencyRate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccountCurrencyRate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountCurrencyRate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccountCurrencyRate.Location = New System.Drawing.Point(224, 48)
        Me.txtAccountCurrencyRate.MaxLength = 0
        Me.txtAccountCurrencyRate.Name = "txtAccountCurrencyRate"
        Me.txtAccountCurrencyRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccountCurrencyRate.Size = New System.Drawing.Size(81, 21)
        Me.txtAccountCurrencyRate.TabIndex = 20
        Me.txtAccountCurrencyRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cboAccountCurrency
        '
        Me.cboAccountCurrency.DefaultItemId = 0
        Me.cboAccountCurrency.ItemId = 0
        Me.cboAccountCurrency.ListIndex = -1
        Me.cboAccountCurrency.Location = New System.Drawing.Point(160, 20)
        Me.cboAccountCurrency.Name = "cboAccountCurrency"
        Me.cboAccountCurrency.PMLookupProductFamily = 1
        Me.cboAccountCurrency.SingleItemId = 0
        Me.cboAccountCurrency.Size = New System.Drawing.Size(145, 21)
        Me.cboAccountCurrency.Sorted = True
        Me.cboAccountCurrency.TabIndex = 18
        Me.cboAccountCurrency.TableName = "Currency"
        Me.cboAccountCurrency.ToolTipText = ""
        Me.cboAccountCurrency.WhereClause = ""
        '
        'lblAccountCurrencyAmount
        '
        Me.lblAccountCurrencyAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountCurrencyAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountCurrencyAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountCurrencyAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountCurrencyAmount.Location = New System.Drawing.Point(16, 76)
        Me.lblAccountCurrencyAmount.Name = "lblAccountCurrencyAmount"
        Me.lblAccountCurrencyAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountCurrencyAmount.Size = New System.Drawing.Size(163, 17)
        Me.lblAccountCurrencyAmount.TabIndex = 23
        Me.lblAccountCurrencyAmount.Text = "Account Currency Amount:"
        '
        'lblAccountCurrencyRate
        '
        Me.lblAccountCurrencyRate.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountCurrencyRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountCurrencyRate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountCurrencyRate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountCurrencyRate.Location = New System.Drawing.Point(16, 50)
        Me.lblAccountCurrencyRate.Name = "lblAccountCurrencyRate"
        Me.lblAccountCurrencyRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountCurrencyRate.Size = New System.Drawing.Size(145, 17)
        Me.lblAccountCurrencyRate.TabIndex = 21
        Me.lblAccountCurrencyRate.Text = "Account Currency Rate:"
        '
        'lblAccountCurrency
        '
        Me.lblAccountCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountCurrency.Location = New System.Drawing.Point(16, 22)
        Me.lblAccountCurrency.Name = "lblAccountCurrency"
        Me.lblAccountCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountCurrency.Size = New System.Drawing.Size(145, 17)
        Me.lblAccountCurrency.TabIndex = 19
        Me.lblAccountCurrency.Text = "Account Currency:"
        '
        'fraRateOverrideReason
        '
        Me.fraRateOverrideReason.BackColor = System.Drawing.SystemColors.Control
        Me.fraRateOverrideReason.Controls.Add(Me.cboReason)
        Me.fraRateOverrideReason.Controls.Add(Me.lblReason)
        Me.fraRateOverrideReason.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraRateOverrideReason.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraRateOverrideReason.Location = New System.Drawing.Point(336, 240)
        Me.fraRateOverrideReason.Name = "fraRateOverrideReason"
        Me.fraRateOverrideReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraRateOverrideReason.Size = New System.Drawing.Size(321, 81)
        Me.fraRateOverrideReason.TabIndex = 14
        Me.fraRateOverrideReason.TabStop = False
        Me.fraRateOverrideReason.Text = "Rate Override Reason"
        '
        'cboReason
        '
        Me.cboReason.DefaultItemId = 0
        Me.cboReason.Enabled = False
        Me.cboReason.ItemId = 0
        Me.cboReason.ListIndex = -1
        Me.cboReason.Location = New System.Drawing.Point(72, 32)
        Me.cboReason.Name = "cboReason"
        Me.cboReason.PMLookupProductFamily = 1
        Me.cboReason.SingleItemId = 0
        Me.cboReason.Size = New System.Drawing.Size(233, 21)
        Me.cboReason.Sorted = True
        Me.cboReason.TabIndex = 15
        Me.cboReason.TableName = "Exchange_Rate_Override_Reason"
        Me.cboReason.ToolTipText = ""
        Me.cboReason.WhereClause = ""
        '
        'lblReason
        '
        Me.lblReason.BackColor = System.Drawing.SystemColors.Control
        Me.lblReason.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReason.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReason.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReason.Location = New System.Drawing.Point(16, 36)
        Me.lblReason.Name = "lblReason"
        Me.lblReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReason.Size = New System.Drawing.Size(51, 17)
        Me.lblReason.TabIndex = 16
        Me.lblReason.Text = "Reason:"
        '
        'fraBaseCurrency
        '
        Me.fraBaseCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.fraBaseCurrency.Controls.Add(Me.txtBaseCurrencyAmount)
        Me.fraBaseCurrency.Controls.Add(Me.txtBaseCurrencyRate)
        Me.fraBaseCurrency.Controls.Add(Me.cboBaseCurrency)
        Me.fraBaseCurrency.Controls.Add(Me.lblBaseCurrencyAmount)
        Me.fraBaseCurrency.Controls.Add(Me.lblBaseCurrencyRate)
        Me.fraBaseCurrency.Controls.Add(Me.lblBaseCurrency)
        Me.fraBaseCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraBaseCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraBaseCurrency.Location = New System.Drawing.Point(8, 126)
        Me.fraBaseCurrency.Name = "fraBaseCurrency"
        Me.fraBaseCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraBaseCurrency.Size = New System.Drawing.Size(321, 107)
        Me.fraBaseCurrency.TabIndex = 7
        Me.fraBaseCurrency.TabStop = False
        Me.fraBaseCurrency.Text = "Base Currency"
        '
        'txtBaseCurrencyAmount
        '
        Me.txtBaseCurrencyAmount.AcceptsReturn = True
        Me.txtBaseCurrencyAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtBaseCurrencyAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBaseCurrencyAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBaseCurrencyAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBaseCurrencyAmount.Location = New System.Drawing.Point(160, 74)
        Me.txtBaseCurrencyAmount.MaxLength = 0
        Me.txtBaseCurrencyAmount.Name = "txtBaseCurrencyAmount"
        Me.txtBaseCurrencyAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBaseCurrencyAmount.Size = New System.Drawing.Size(145, 21)
        Me.txtBaseCurrencyAmount.TabIndex = 12
        Me.txtBaseCurrencyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtBaseCurrencyRate
        '
        Me.txtBaseCurrencyRate.AcceptsReturn = True
        Me.txtBaseCurrencyRate.BackColor = System.Drawing.SystemColors.Window
        Me.txtBaseCurrencyRate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBaseCurrencyRate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBaseCurrencyRate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBaseCurrencyRate.Location = New System.Drawing.Point(224, 48)
        Me.txtBaseCurrencyRate.MaxLength = 0
        Me.txtBaseCurrencyRate.Name = "txtBaseCurrencyRate"
        Me.txtBaseCurrencyRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBaseCurrencyRate.Size = New System.Drawing.Size(81, 21)
        Me.txtBaseCurrencyRate.TabIndex = 10
        Me.txtBaseCurrencyRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cboBaseCurrency
        '
        Me.cboBaseCurrency.DefaultItemId = 0
        Me.cboBaseCurrency.ItemId = 0
        Me.cboBaseCurrency.ListIndex = -1
        Me.cboBaseCurrency.Location = New System.Drawing.Point(160, 20)
        Me.cboBaseCurrency.Name = "cboBaseCurrency"
        Me.cboBaseCurrency.PMLookupProductFamily = 1
        Me.cboBaseCurrency.SingleItemId = 0
        Me.cboBaseCurrency.Size = New System.Drawing.Size(145, 21)
        Me.cboBaseCurrency.Sorted = True
        Me.cboBaseCurrency.TabIndex = 8
        Me.cboBaseCurrency.TableName = "Currency"
        Me.cboBaseCurrency.ToolTipText = ""
        Me.cboBaseCurrency.WhereClause = ""
        '
        'lblBaseCurrencyAmount
        '
        Me.lblBaseCurrencyAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblBaseCurrencyAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBaseCurrencyAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBaseCurrencyAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBaseCurrencyAmount.Location = New System.Drawing.Point(16, 76)
        Me.lblBaseCurrencyAmount.Name = "lblBaseCurrencyAmount"
        Me.lblBaseCurrencyAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBaseCurrencyAmount.Size = New System.Drawing.Size(145, 17)
        Me.lblBaseCurrencyAmount.TabIndex = 13
        Me.lblBaseCurrencyAmount.Text = "Base Currency Amount:"
        '
        'lblBaseCurrencyRate
        '
        Me.lblBaseCurrencyRate.BackColor = System.Drawing.SystemColors.Control
        Me.lblBaseCurrencyRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBaseCurrencyRate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBaseCurrencyRate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBaseCurrencyRate.Location = New System.Drawing.Point(16, 50)
        Me.lblBaseCurrencyRate.Name = "lblBaseCurrencyRate"
        Me.lblBaseCurrencyRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBaseCurrencyRate.Size = New System.Drawing.Size(145, 17)
        Me.lblBaseCurrencyRate.TabIndex = 11
        Me.lblBaseCurrencyRate.Text = "Base Currency Rate:"
        '
        'lblBaseCurrency
        '
        Me.lblBaseCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblBaseCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBaseCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBaseCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBaseCurrency.Location = New System.Drawing.Point(16, 22)
        Me.lblBaseCurrency.Name = "lblBaseCurrency"
        Me.lblBaseCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBaseCurrency.Size = New System.Drawing.Size(145, 17)
        Me.lblBaseCurrency.TabIndex = 9
        Me.lblBaseCurrency.Text = "Base Currency:"
        '
        'fraTransaction
        '
        Me.fraTransaction.BackColor = System.Drawing.SystemColors.Control
        Me.fraTransaction.Controls.Add(Me.txtTransactionValue)
        Me.fraTransaction.Controls.Add(Me.cboTransactionCurrency)
        Me.fraTransaction.Controls.Add(Me.cboEffectiveDate)
        Me.fraTransaction.Controls.Add(Me.lblEffectiveDate)
        Me.fraTransaction.Controls.Add(Me.lblTransactionCurrency)
        Me.fraTransaction.Controls.Add(Me.lblTransactionValue)
        Me.fraTransaction.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTransaction.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTransaction.Location = New System.Drawing.Point(8, 8)
        Me.fraTransaction.Name = "fraTransaction"
        Me.fraTransaction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTransaction.Size = New System.Drawing.Size(321, 109)
        Me.fraTransaction.TabIndex = 0
        Me.fraTransaction.TabStop = False
        Me.fraTransaction.Text = "Transaction"
        '
        'txtTransactionValue
        '
        Me.txtTransactionValue.AcceptsReturn = True
        Me.txtTransactionValue.BackColor = System.Drawing.SystemColors.Window
        Me.txtTransactionValue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTransactionValue.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTransactionValue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTransactionValue.Location = New System.Drawing.Point(160, 20)
        Me.txtTransactionValue.MaxLength = 0
        Me.txtTransactionValue.Name = "txtTransactionValue"
        Me.txtTransactionValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTransactionValue.Size = New System.Drawing.Size(145, 21)
        Me.txtTransactionValue.TabIndex = 1
        Me.txtTransactionValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cboTransactionCurrency
        '
        Me.cboTransactionCurrency.DefaultItemId = 0
        Me.cboTransactionCurrency.ItemId = 0
        Me.cboTransactionCurrency.ListIndex = -1
        Me.cboTransactionCurrency.Location = New System.Drawing.Point(160, 46)
        Me.cboTransactionCurrency.Name = "cboTransactionCurrency"
        Me.cboTransactionCurrency.PMLookupProductFamily = 1
        Me.cboTransactionCurrency.SingleItemId = 0
        Me.cboTransactionCurrency.Size = New System.Drawing.Size(145, 21)
        Me.cboTransactionCurrency.Sorted = True
        Me.cboTransactionCurrency.TabIndex = 3
        Me.cboTransactionCurrency.TableName = "Currency"
        Me.cboTransactionCurrency.ToolTipText = ""
        Me.cboTransactionCurrency.WhereClause = ""
        '
        'cboEffectiveDate
        '
        Me.cboEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.cboEffectiveDate.Location = New System.Drawing.Point(160, 74)
        Me.cboEffectiveDate.Name = "cboEffectiveDate"
        Me.cboEffectiveDate.Size = New System.Drawing.Size(145, 21)
        Me.cboEffectiveDate.TabIndex = 5
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(16, 76)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(161, 17)
        Me.lblEffectiveDate.TabIndex = 6
        Me.lblEffectiveDate.Text = "Date of Exchange:"
        '
        'lblTransactionCurrency
        '
        Me.lblTransactionCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblTransactionCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTransactionCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTransactionCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTransactionCurrency.Location = New System.Drawing.Point(16, 48)
        Me.lblTransactionCurrency.Name = "lblTransactionCurrency"
        Me.lblTransactionCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTransactionCurrency.Size = New System.Drawing.Size(153, 17)
        Me.lblTransactionCurrency.TabIndex = 4
        Me.lblTransactionCurrency.Text = "Transaction Currency:"
        '
        'lblTransactionValue
        '
        Me.lblTransactionValue.BackColor = System.Drawing.SystemColors.Control
        Me.lblTransactionValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTransactionValue.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTransactionValue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTransactionValue.Location = New System.Drawing.Point(16, 22)
        Me.lblTransactionValue.Name = "lblTransactionValue"
        Me.lblTransactionValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTransactionValue.Size = New System.Drawing.Size(145, 17)
        Me.lblTransactionValue.TabIndex = 2
        Me.lblTransactionValue.Text = "Transaction Value:"
        '
        'uctSIRMultiCurrency
        '
        Me.Controls.Add(Me.fraLossCurrency)
        Me.Controls.Add(Me.fraSystemCurrency)
        Me.Controls.Add(Me.fraAccountCurrency)
        Me.Controls.Add(Me.fraRateOverrideReason)
        Me.Controls.Add(Me.fraBaseCurrency)
        Me.Controls.Add(Me.fraTransaction)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctSIRMultiCurrency"
        Me.Size = New System.Drawing.Size(665, 330)
        Me.fraLossCurrency.ResumeLayout(False)
        Me.fraLossCurrency.PerformLayout()
        Me.fraSystemCurrency.ResumeLayout(False)
        Me.fraSystemCurrency.PerformLayout()
        Me.fraAccountCurrency.ResumeLayout(False)
        Me.fraAccountCurrency.PerformLayout()
        Me.fraRateOverrideReason.ResumeLayout(False)
        Me.fraBaseCurrency.ResumeLayout(False)
        Me.fraBaseCurrency.PerformLayout()
        Me.fraTransaction.ResumeLayout(False)
        Me.fraTransaction.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class