<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPaymentDetails
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents txtLCPaymentTotal As System.Windows.Forms.TextBox
	Public WithEvents txtPaymentTotal As System.Windows.Forms.TextBox
	Public WithEvents lblLCPaymentTotal As System.Windows.Forms.Label
	Public WithEvents lblPaymentTotal As System.Windows.Forms.Label
	Public WithEvents fraTotal As System.Windows.Forms.GroupBox
	Public WithEvents txtLCTaxAmount As System.Windows.Forms.TextBox
	Public WithEvents txtTaxAmount As System.Windows.Forms.TextBox
	Public WithEvents cboTaxGroup As System.Windows.Forms.ComboBox
	Public WithEvents txtTaxGroup As System.Windows.Forms.TextBox
	Public WithEvents lblTaxAmount As System.Windows.Forms.Label
	Public WithEvents lblLCTaxAnount As System.Windows.Forms.Label
	Public WithEvents lblTaxGroup As System.Windows.Forms.Label
	Public WithEvents fraTaxes As System.Windows.Forms.GroupBox
	Public WithEvents txtLossCurrency As System.Windows.Forms.TextBox
	Public WithEvents txtLCPaymentAmount As System.Windows.Forms.TextBox
	Public WithEvents txtPaymentAmount As System.Windows.Forms.TextBox
	Public WithEvents txtCurrencyRate As System.Windows.Forms.TextBox
	Public WithEvents cboCurrency As System.Windows.Forms.ComboBox
	Public WithEvents txtCurrency As System.Windows.Forms.TextBox
	Public WithEvents lblCurrency1 As System.Windows.Forms.Label
	Public WithEvents lblPaymentCurrency As System.Windows.Forms.Label
	Public WithEvents lblLCPaymentAmount As System.Windows.Forms.Label
	Public WithEvents lblossCurrency As System.Windows.Forms.Label
	Public WithEvents lblPaymentAmount As System.Windows.Forms.Label
	Public WithEvents lblCurrencyRate As System.Windows.Forms.Label
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	Public WithEvents fraPayment As System.Windows.Forms.GroupBox
	Public WithEvents chkReverseExcessPayment As System.Windows.Forms.CheckBox
	Public WithEvents txtRiskType As System.Windows.Forms.TextBox
	Public WithEvents txtReserveType As System.Windows.Forms.TextBox
	Public WithEvents txtBalance As System.Windows.Forms.TextBox
	Public WithEvents txtPaidToDate As System.Windows.Forms.TextBox
	Public WithEvents txtTotalReserve As System.Windows.Forms.TextBox
	Public WithEvents lblReverseExcessPayment As System.Windows.Forms.Label
	Public WithEvents lblBalance As System.Windows.Forms.Label
	Public WithEvents lblPaidToDate As System.Windows.Forms.Label
	Public WithEvents lblTotalReserve As System.Windows.Forms.Label
	Public WithEvents lblRiskType As System.Windows.Forms.Label
	Public WithEvents lblForReserveType As System.Windows.Forms.Label
	Public WithEvents fraReserve As System.Windows.Forms.GroupBox
	Public WithEvents lblPaymentAdjustment As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOk = New System.Windows.Forms.Button
        Me.fraTotal = New System.Windows.Forms.GroupBox
        Me.txtLCPaymentTotal = New System.Windows.Forms.TextBox
        Me.txtPaymentTotal = New System.Windows.Forms.TextBox
        Me.lblLCPaymentTotal = New System.Windows.Forms.Label
        Me.lblPaymentTotal = New System.Windows.Forms.Label
        Me.fraTaxes = New System.Windows.Forms.GroupBox
        Me.txtLCTaxAmount = New System.Windows.Forms.TextBox
        Me.txtTaxAmount = New System.Windows.Forms.TextBox
        Me.cboTaxGroup = New System.Windows.Forms.ComboBox
        Me.txtTaxGroup = New System.Windows.Forms.TextBox
        Me.lblTaxAmount = New System.Windows.Forms.Label
        Me.lblLCTaxAnount = New System.Windows.Forms.Label
        Me.lblTaxGroup = New System.Windows.Forms.Label
        Me.fraPayment = New System.Windows.Forms.GroupBox
        Me.txtLossCurrency = New System.Windows.Forms.TextBox
        Me.txtLCPaymentAmount = New System.Windows.Forms.TextBox
        Me.txtPaymentAmount = New System.Windows.Forms.TextBox
        Me.txtCurrencyRate = New System.Windows.Forms.TextBox
        Me.cboCurrency = New System.Windows.Forms.ComboBox
        Me.txtCurrency = New System.Windows.Forms.TextBox
        Me.lblCurrency1 = New System.Windows.Forms.Label
        Me.lblPaymentCurrency = New System.Windows.Forms.Label
        Me.lblLCPaymentAmount = New System.Windows.Forms.Label
        Me.lblossCurrency = New System.Windows.Forms.Label
        Me.lblPaymentAmount = New System.Windows.Forms.Label
        Me.lblCurrencyRate = New System.Windows.Forms.Label
        Me.lblCurrency = New System.Windows.Forms.Label
        Me.fraReserve = New System.Windows.Forms.GroupBox
        Me.chkReverseExcessPayment = New System.Windows.Forms.CheckBox
        Me.txtRiskType = New System.Windows.Forms.TextBox
        Me.txtReserveType = New System.Windows.Forms.TextBox
        Me.txtBalance = New System.Windows.Forms.TextBox
        Me.txtPaidToDate = New System.Windows.Forms.TextBox
        Me.txtTotalReserve = New System.Windows.Forms.TextBox
        Me.lblReverseExcessPayment = New System.Windows.Forms.Label
        Me.lblBalance = New System.Windows.Forms.Label
        Me.lblPaidToDate = New System.Windows.Forms.Label
        Me.lblTotalReserve = New System.Windows.Forms.Label
        Me.lblRiskType = New System.Windows.Forms.Label
        Me.lblForReserveType = New System.Windows.Forms.Label
        Me.lblPaymentAdjustment = New System.Windows.Forms.Label
        Me.fraTotal.SuspendLayout()
        Me.fraTaxes.SuspendLayout()
        Me.fraPayment.SuspendLayout()
        Me.fraReserve.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(464, 339)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 21)
        Me.cmdCancel.TabIndex = 37
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOk
        '
        Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOk.Location = New System.Drawing.Point(384, 339)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(73, 21)
        Me.cmdOk.TabIndex = 36
        Me.cmdOk.Text = "&OK"
        Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOk.UseVisualStyleBackColor = False
        '
        'fraTotal
        '
        Me.fraTotal.BackColor = System.Drawing.SystemColors.Control
        Me.fraTotal.Controls.Add(Me.txtLCPaymentTotal)
        Me.fraTotal.Controls.Add(Me.txtPaymentTotal)
        Me.fraTotal.Controls.Add(Me.lblLCPaymentTotal)
        Me.fraTotal.Controls.Add(Me.lblPaymentTotal)
        Me.fraTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTotal.Location = New System.Drawing.Point(8, 280)
        Me.fraTotal.Name = "fraTotal"
        Me.fraTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTotal.Size = New System.Drawing.Size(529, 49)
        Me.fraTotal.TabIndex = 31
        Me.fraTotal.TabStop = False
        Me.fraTotal.Text = "Total"
        '
        'txtLCPaymentTotal
        '
        Me.txtLCPaymentTotal.AcceptsReturn = True
        Me.txtLCPaymentTotal.BackColor = System.Drawing.SystemColors.Control
        Me.txtLCPaymentTotal.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLCPaymentTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLCPaymentTotal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLCPaymentTotal.Location = New System.Drawing.Point(384, 16)
        Me.txtLCPaymentTotal.MaxLength = 0
        Me.txtLCPaymentTotal.Name = "txtLCPaymentTotal"
        Me.txtLCPaymentTotal.ReadOnly = True
        Me.txtLCPaymentTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLCPaymentTotal.Size = New System.Drawing.Size(137, 20)
        Me.txtLCPaymentTotal.TabIndex = 35
        Me.txtLCPaymentTotal.TabStop = False
        Me.txtLCPaymentTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtPaymentTotal
        '
        Me.txtPaymentTotal.AcceptsReturn = True
        Me.txtPaymentTotal.BackColor = System.Drawing.SystemColors.Control
        Me.txtPaymentTotal.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPaymentTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaymentTotal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPaymentTotal.Location = New System.Drawing.Point(128, 16)
        Me.txtPaymentTotal.MaxLength = 0
        Me.txtPaymentTotal.Name = "txtPaymentTotal"
        Me.txtPaymentTotal.ReadOnly = True
        Me.txtPaymentTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPaymentTotal.Size = New System.Drawing.Size(137, 20)
        Me.txtPaymentTotal.TabIndex = 33
        Me.txtPaymentTotal.TabStop = False
        Me.txtPaymentTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblLCPaymentTotal
        '
        Me.lblLCPaymentTotal.AutoSize = True
        Me.lblLCPaymentTotal.BackColor = System.Drawing.SystemColors.Control
        Me.lblLCPaymentTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLCPaymentTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLCPaymentTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLCPaymentTotal.Location = New System.Drawing.Point(289, 20)
        Me.lblLCPaymentTotal.Name = "lblLCPaymentTotal"
        Me.lblLCPaymentTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLCPaymentTotal.Size = New System.Drawing.Size(78, 13)
        Me.lblLCPaymentTotal.TabIndex = 34
        Me.lblLCPaymentTotal.Text = "Payment Total:"
        '
        'lblPaymentTotal
        '
        Me.lblPaymentTotal.AutoSize = True
        Me.lblPaymentTotal.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentTotal.Location = New System.Drawing.Point(33, 20)
        Me.lblPaymentTotal.Name = "lblPaymentTotal"
        Me.lblPaymentTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentTotal.Size = New System.Drawing.Size(78, 13)
        Me.lblPaymentTotal.TabIndex = 32
        Me.lblPaymentTotal.Text = "Payment Total:"
        '
        'fraTaxes
        '
        Me.fraTaxes.BackColor = System.Drawing.SystemColors.Control
        Me.fraTaxes.Controls.Add(Me.txtLCTaxAmount)
        Me.fraTaxes.Controls.Add(Me.txtTaxAmount)
        Me.fraTaxes.Controls.Add(Me.cboTaxGroup)
        Me.fraTaxes.Controls.Add(Me.txtTaxGroup)
        Me.fraTaxes.Controls.Add(Me.lblTaxAmount)
        Me.fraTaxes.Controls.Add(Me.lblLCTaxAnount)
        Me.fraTaxes.Controls.Add(Me.lblTaxGroup)
        Me.fraTaxes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTaxes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTaxes.Location = New System.Drawing.Point(8, 208)
        Me.fraTaxes.Name = "fraTaxes"
        Me.fraTaxes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTaxes.Size = New System.Drawing.Size(529, 72)
        Me.fraTaxes.TabIndex = 24
        Me.fraTaxes.TabStop = False
        Me.fraTaxes.Text = "Taxes"
        '
        'txtLCTaxAmount
        '
        Me.txtLCTaxAmount.AcceptsReturn = True
        Me.txtLCTaxAmount.BackColor = System.Drawing.SystemColors.Control
        Me.txtLCTaxAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLCTaxAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLCTaxAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLCTaxAmount.Location = New System.Drawing.Point(384, 40)
        Me.txtLCTaxAmount.MaxLength = 0
        Me.txtLCTaxAmount.Name = "txtLCTaxAmount"
        Me.txtLCTaxAmount.ReadOnly = True
        Me.txtLCTaxAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLCTaxAmount.Size = New System.Drawing.Size(137, 20)
        Me.txtLCTaxAmount.TabIndex = 30
        Me.txtLCTaxAmount.TabStop = False
        Me.txtLCTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTaxAmount
        '
        Me.txtTaxAmount.AcceptsReturn = True
        Me.txtTaxAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtTaxAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTaxAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTaxAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTaxAmount.Location = New System.Drawing.Point(128, 40)
        Me.txtTaxAmount.MaxLength = 0
        Me.txtTaxAmount.Name = "txtTaxAmount"
        Me.txtTaxAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTaxAmount.Size = New System.Drawing.Size(137, 20)
        Me.txtTaxAmount.TabIndex = 28
        Me.txtTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cboTaxGroup
        '
        Me.cboTaxGroup.BackColor = System.Drawing.SystemColors.Window
        Me.cboTaxGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTaxGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTaxGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTaxGroup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTaxGroup.Location = New System.Drawing.Point(128, 16)
        Me.cboTaxGroup.Name = "cboTaxGroup"
        Me.cboTaxGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTaxGroup.Size = New System.Drawing.Size(161, 21)
        Me.cboTaxGroup.TabIndex = 26
        '
        'txtTaxGroup
        '
        Me.txtTaxGroup.AcceptsReturn = True
        Me.txtTaxGroup.BackColor = System.Drawing.SystemColors.Control
        Me.txtTaxGroup.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTaxGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTaxGroup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTaxGroup.Location = New System.Drawing.Point(128, 16)
        Me.txtTaxGroup.MaxLength = 0
        Me.txtTaxGroup.Name = "txtTaxGroup"
        Me.txtTaxGroup.ReadOnly = True
        Me.txtTaxGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTaxGroup.Size = New System.Drawing.Size(161, 20)
        Me.txtTaxGroup.TabIndex = 39
        Me.txtTaxGroup.TabStop = False
        Me.txtTaxGroup.Visible = False
        '
        'lblTaxAmount
        '
        Me.lblTaxAmount.AutoSize = True
        Me.lblTaxAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxAmount.Location = New System.Drawing.Point(46, 44)
        Me.lblTaxAmount.Name = "lblTaxAmount"
        Me.lblTaxAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxAmount.Size = New System.Drawing.Size(67, 13)
        Me.lblTaxAmount.TabIndex = 27
        Me.lblTaxAmount.Text = "Tax Amount:"
        '
        'lblLCTaxAnount
        '
        Me.lblLCTaxAnount.AutoSize = True
        Me.lblLCTaxAnount.BackColor = System.Drawing.SystemColors.Control
        Me.lblLCTaxAnount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLCTaxAnount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLCTaxAnount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLCTaxAnount.Location = New System.Drawing.Point(302, 44)
        Me.lblLCTaxAnount.Name = "lblLCTaxAnount"
        Me.lblLCTaxAnount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLCTaxAnount.Size = New System.Drawing.Size(67, 13)
        Me.lblLCTaxAnount.TabIndex = 29
        Me.lblLCTaxAnount.Text = "Tax Amount:"
        '
        'lblTaxGroup
        '
        Me.lblTaxGroup.AutoSize = True
        Me.lblTaxGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxGroup.Location = New System.Drawing.Point(55, 20)
        Me.lblTaxGroup.Name = "lblTaxGroup"
        Me.lblTaxGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxGroup.Size = New System.Drawing.Size(60, 13)
        Me.lblTaxGroup.TabIndex = 25
        Me.lblTaxGroup.Text = "Tax Group:"
        '
        'fraPayment
        '
        Me.fraPayment.BackColor = System.Drawing.SystemColors.Control
        Me.fraPayment.Controls.Add(Me.txtLossCurrency)
        Me.fraPayment.Controls.Add(Me.txtLCPaymentAmount)
        Me.fraPayment.Controls.Add(Me.txtPaymentAmount)
        Me.fraPayment.Controls.Add(Me.txtCurrencyRate)
        Me.fraPayment.Controls.Add(Me.cboCurrency)
        Me.fraPayment.Controls.Add(Me.txtCurrency)
        Me.fraPayment.Controls.Add(Me.lblCurrency1)
        Me.fraPayment.Controls.Add(Me.lblPaymentCurrency)
        Me.fraPayment.Controls.Add(Me.lblLCPaymentAmount)
        Me.fraPayment.Controls.Add(Me.lblossCurrency)
        Me.fraPayment.Controls.Add(Me.lblPaymentAmount)
        Me.fraPayment.Controls.Add(Me.lblCurrencyRate)
        Me.fraPayment.Controls.Add(Me.lblCurrency)
        Me.fraPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPayment.Location = New System.Drawing.Point(8, 100)
        Me.fraPayment.Name = "fraPayment"
        Me.fraPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPayment.Size = New System.Drawing.Size(529, 108)
        Me.fraPayment.TabIndex = 13
        Me.fraPayment.TabStop = False
        Me.fraPayment.Text = "Payment"
        '
        'txtLossCurrency
        '
        Me.txtLossCurrency.AcceptsReturn = True
        Me.txtLossCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.txtLossCurrency.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLossCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLossCurrency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLossCurrency.Location = New System.Drawing.Point(384, 32)
        Me.txtLossCurrency.MaxLength = 0
        Me.txtLossCurrency.Name = "txtLossCurrency"
        Me.txtLossCurrency.ReadOnly = True
        Me.txtLossCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLossCurrency.Size = New System.Drawing.Size(137, 20)
        Me.txtLossCurrency.TabIndex = 41
        Me.txtLossCurrency.TabStop = False
        '
        'txtLCPaymentAmount
        '
        Me.txtLCPaymentAmount.AcceptsReturn = True
        Me.txtLCPaymentAmount.BackColor = System.Drawing.SystemColors.Control
        Me.txtLCPaymentAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLCPaymentAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLCPaymentAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLCPaymentAmount.Location = New System.Drawing.Point(384, 80)
        Me.txtLCPaymentAmount.MaxLength = 0
        Me.txtLCPaymentAmount.Name = "txtLCPaymentAmount"
        Me.txtLCPaymentAmount.ReadOnly = True
        Me.txtLCPaymentAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLCPaymentAmount.Size = New System.Drawing.Size(137, 20)
        Me.txtLCPaymentAmount.TabIndex = 40
        Me.txtLCPaymentAmount.TabStop = False
        Me.txtLCPaymentAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtPaymentAmount
        '
        Me.txtPaymentAmount.AcceptsReturn = True
        Me.txtPaymentAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtPaymentAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPaymentAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaymentAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPaymentAmount.Location = New System.Drawing.Point(128, 80)
        Me.txtPaymentAmount.MaxLength = 18
        Me.txtPaymentAmount.Name = "txtPaymentAmount"
        Me.txtPaymentAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPaymentAmount.Size = New System.Drawing.Size(137, 20)
        Me.txtPaymentAmount.TabIndex = 22
        Me.txtPaymentAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCurrencyRate
        '
        Me.txtCurrencyRate.AcceptsReturn = True
        Me.txtCurrencyRate.BackColor = System.Drawing.SystemColors.Control
        Me.txtCurrencyRate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCurrencyRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCurrencyRate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCurrencyRate.Location = New System.Drawing.Point(128, 56)
        Me.txtCurrencyRate.MaxLength = 0
        Me.txtCurrencyRate.Name = "txtCurrencyRate"
        Me.txtCurrencyRate.ReadOnly = True
        Me.txtCurrencyRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCurrencyRate.Size = New System.Drawing.Size(137, 20)
        Me.txtCurrencyRate.TabIndex = 20
        Me.txtCurrencyRate.TabStop = False
        Me.txtCurrencyRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cboCurrency
        '
        Me.cboCurrency.BackColor = System.Drawing.SystemColors.Window
        Me.cboCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCurrency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCurrency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCurrency.Location = New System.Drawing.Point(128, 32)
        Me.cboCurrency.Name = "cboCurrency"
        Me.cboCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCurrency.Size = New System.Drawing.Size(161, 21)
        Me.cboCurrency.TabIndex = 17
        '
        'txtCurrency
        '
        Me.txtCurrency.AcceptsReturn = True
        Me.txtCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.txtCurrency.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCurrency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCurrency.Location = New System.Drawing.Point(128, 32)
        Me.txtCurrency.MaxLength = 0
        Me.txtCurrency.Name = "txtCurrency"
        Me.txtCurrency.ReadOnly = True
        Me.txtCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCurrency.Size = New System.Drawing.Size(161, 20)
        Me.txtCurrency.TabIndex = 38
        Me.txtCurrency.TabStop = False
        Me.txtCurrency.Visible = False
        '
        'lblCurrency1
        '
        Me.lblCurrency1.AutoSize = True
        Me.lblCurrency1.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency1.Location = New System.Drawing.Point(318, 36)
        Me.lblCurrency1.Name = "lblCurrency1"
        Me.lblCurrency1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency1.Size = New System.Drawing.Size(52, 13)
        Me.lblCurrency1.TabIndex = 18
        Me.lblCurrency1.Text = "Currency:"
        '
        'lblPaymentCurrency
        '
        Me.lblPaymentCurrency.AutoSize = True
        Me.lblPaymentCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentCurrency.Location = New System.Drawing.Point(128, 12)
        Me.lblPaymentCurrency.Name = "lblPaymentCurrency"
        Me.lblPaymentCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentCurrency.Size = New System.Drawing.Size(93, 13)
        Me.lblPaymentCurrency.TabIndex = 14
        Me.lblPaymentCurrency.Text = "Payment Currency"
        '
        'lblLCPaymentAmount
        '
        Me.lblLCPaymentAmount.AutoSize = True
        Me.lblLCPaymentAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblLCPaymentAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLCPaymentAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLCPaymentAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLCPaymentAmount.Location = New System.Drawing.Point(273, 84)
        Me.lblLCPaymentAmount.Name = "lblLCPaymentAmount"
        Me.lblLCPaymentAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLCPaymentAmount.Size = New System.Drawing.Size(90, 13)
        Me.lblLCPaymentAmount.TabIndex = 23
        Me.lblLCPaymentAmount.Text = "Payment Amount:"
        '
        'lblossCurrency
        '
        Me.lblossCurrency.AutoSize = True
        Me.lblossCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblossCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblossCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblossCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblossCurrency.Location = New System.Drawing.Point(384, 12)
        Me.lblossCurrency.Name = "lblossCurrency"
        Me.lblossCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblossCurrency.Size = New System.Drawing.Size(74, 13)
        Me.lblossCurrency.TabIndex = 15
        Me.lblossCurrency.Text = "Loss Currency"
        '
        'lblPaymentAmount
        '
        Me.lblPaymentAmount.AutoSize = True
        Me.lblPaymentAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentAmount.Location = New System.Drawing.Point(17, 84)
        Me.lblPaymentAmount.Name = "lblPaymentAmount"
        Me.lblPaymentAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentAmount.Size = New System.Drawing.Size(90, 13)
        Me.lblPaymentAmount.TabIndex = 21
        Me.lblPaymentAmount.Text = "Payment Amount:"
        '
        'lblCurrencyRate
        '
        Me.lblCurrencyRate.AutoSize = True
        Me.lblCurrencyRate.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrencyRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrencyRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrencyRate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrencyRate.Location = New System.Drawing.Point(32, 60)
        Me.lblCurrencyRate.Name = "lblCurrencyRate"
        Me.lblCurrencyRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrencyRate.Size = New System.Drawing.Size(78, 13)
        Me.lblCurrencyRate.TabIndex = 19
        Me.lblCurrencyRate.Text = "Currency Rate:"
        '
        'lblCurrency
        '
        Me.lblCurrency.AutoSize = True
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(62, 36)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(52, 13)
        Me.lblCurrency.TabIndex = 16
        Me.lblCurrency.Text = "Currency:"
        '
        'fraReserve
        '
        Me.fraReserve.BackColor = System.Drawing.SystemColors.Control
        Me.fraReserve.Controls.Add(Me.chkReverseExcessPayment)
        Me.fraReserve.Controls.Add(Me.txtRiskType)
        Me.fraReserve.Controls.Add(Me.txtReserveType)
        Me.fraReserve.Controls.Add(Me.txtBalance)
        Me.fraReserve.Controls.Add(Me.txtPaidToDate)
        Me.fraReserve.Controls.Add(Me.txtTotalReserve)
        Me.fraReserve.Controls.Add(Me.lblReverseExcessPayment)
        Me.fraReserve.Controls.Add(Me.lblBalance)
        Me.fraReserve.Controls.Add(Me.lblPaidToDate)
        Me.fraReserve.Controls.Add(Me.lblTotalReserve)
        Me.fraReserve.Controls.Add(Me.lblRiskType)
        Me.fraReserve.Controls.Add(Me.lblForReserveType)
        Me.fraReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraReserve.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraReserve.Location = New System.Drawing.Point(8, 8)
        Me.fraReserve.Name = "fraReserve"
        Me.fraReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraReserve.Size = New System.Drawing.Size(529, 92)
        Me.fraReserve.TabIndex = 0
        Me.fraReserve.TabStop = False
        Me.fraReserve.Text = "Reserve"
        '
        'chkReverseExcessPayment
        '
        Me.chkReverseExcessPayment.BackColor = System.Drawing.SystemColors.Control
        Me.chkReverseExcessPayment.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkReverseExcessPayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkReverseExcessPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkReverseExcessPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkReverseExcessPayment.Location = New System.Drawing.Point(128, 64)
        Me.chkReverseExcessPayment.Name = "chkReverseExcessPayment"
        Me.chkReverseExcessPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkReverseExcessPayment.Size = New System.Drawing.Size(13, 21)
        Me.chkReverseExcessPayment.TabIndex = 10
        Me.chkReverseExcessPayment.UseVisualStyleBackColor = False
        '
        'txtRiskType
        '
        Me.txtRiskType.AcceptsReturn = True
        Me.txtRiskType.BackColor = System.Drawing.SystemColors.Control
        Me.txtRiskType.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRiskType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRiskType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRiskType.Location = New System.Drawing.Point(128, 40)
        Me.txtRiskType.MaxLength = 0
        Me.txtRiskType.Name = "txtRiskType"
        Me.txtRiskType.ReadOnly = True
        Me.txtRiskType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRiskType.Size = New System.Drawing.Size(137, 20)
        Me.txtRiskType.TabIndex = 6
        Me.txtRiskType.TabStop = False
        '
        'txtReserveType
        '
        Me.txtReserveType.AcceptsReturn = True
        Me.txtReserveType.BackColor = System.Drawing.SystemColors.Control
        Me.txtReserveType.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtReserveType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReserveType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtReserveType.Location = New System.Drawing.Point(128, 16)
        Me.txtReserveType.MaxLength = 0
        Me.txtReserveType.Name = "txtReserveType"
        Me.txtReserveType.ReadOnly = True
        Me.txtReserveType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtReserveType.Size = New System.Drawing.Size(137, 20)
        Me.txtReserveType.TabIndex = 2
        Me.txtReserveType.TabStop = False
        '
        'txtBalance
        '
        Me.txtBalance.AcceptsReturn = True
        Me.txtBalance.BackColor = System.Drawing.SystemColors.Control
        Me.txtBalance.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBalance.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBalance.Location = New System.Drawing.Point(384, 64)
        Me.txtBalance.MaxLength = 0
        Me.txtBalance.Name = "txtBalance"
        Me.txtBalance.ReadOnly = True
        Me.txtBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBalance.Size = New System.Drawing.Size(137, 20)
        Me.txtBalance.TabIndex = 12
        Me.txtBalance.TabStop = False
        Me.txtBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtPaidToDate
        '
        Me.txtPaidToDate.AcceptsReturn = True
        Me.txtPaidToDate.BackColor = System.Drawing.SystemColors.Control
        Me.txtPaidToDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPaidToDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaidToDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPaidToDate.Location = New System.Drawing.Point(384, 40)
        Me.txtPaidToDate.MaxLength = 0
        Me.txtPaidToDate.Name = "txtPaidToDate"
        Me.txtPaidToDate.ReadOnly = True
        Me.txtPaidToDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPaidToDate.Size = New System.Drawing.Size(137, 20)
        Me.txtPaidToDate.TabIndex = 8
        Me.txtPaidToDate.TabStop = False
        Me.txtPaidToDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTotalReserve
        '
        Me.txtTotalReserve.AcceptsReturn = True
        Me.txtTotalReserve.BackColor = System.Drawing.SystemColors.Control
        Me.txtTotalReserve.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalReserve.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalReserve.Location = New System.Drawing.Point(384, 16)
        Me.txtTotalReserve.MaxLength = 0
        Me.txtTotalReserve.Name = "txtTotalReserve"
        Me.txtTotalReserve.ReadOnly = True
        Me.txtTotalReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalReserve.Size = New System.Drawing.Size(137, 20)
        Me.txtTotalReserve.TabIndex = 4
        Me.txtTotalReserve.TabStop = False
        Me.txtTotalReserve.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblReverseExcessPayment
        '
        Me.lblReverseExcessPayment.AutoSize = True
        Me.lblReverseExcessPayment.BackColor = System.Drawing.SystemColors.Control
        Me.lblReverseExcessPayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReverseExcessPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReverseExcessPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReverseExcessPayment.Location = New System.Drawing.Point(25, 68)
        Me.lblReverseExcessPayment.Name = "lblReverseExcessPayment"
        Me.lblReverseExcessPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReverseExcessPayment.Size = New System.Drawing.Size(87, 13)
        Me.lblReverseExcessPayment.TabIndex = 9
        Me.lblReverseExcessPayment.Text = "Reverse Excess:"
        '
        'lblBalance
        '
        Me.lblBalance.AutoSize = True
        Me.lblBalance.BackColor = System.Drawing.SystemColors.Control
        Me.lblBalance.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBalance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBalance.Location = New System.Drawing.Point(326, 68)
        Me.lblBalance.Name = "lblBalance"
        Me.lblBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBalance.Size = New System.Drawing.Size(49, 13)
        Me.lblBalance.TabIndex = 11
        Me.lblBalance.Text = "Balance:"
        '
        'lblPaidToDate
        '
        Me.lblPaidToDate.AutoSize = True
        Me.lblPaidToDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaidToDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaidToDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaidToDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaidToDate.Location = New System.Drawing.Point(298, 44)
        Me.lblPaidToDate.Name = "lblPaidToDate"
        Me.lblPaidToDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaidToDate.Size = New System.Drawing.Size(73, 13)
        Me.lblPaidToDate.TabIndex = 7
        Me.lblPaidToDate.Text = "Paid To Date:"
        '
        'lblTotalReserve
        '
        Me.lblTotalReserve.AutoSize = True
        Me.lblTotalReserve.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalReserve.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalReserve.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalReserve.Location = New System.Drawing.Point(292, 20)
        Me.lblTotalReserve.Name = "lblTotalReserve"
        Me.lblTotalReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalReserve.Size = New System.Drawing.Size(77, 13)
        Me.lblTotalReserve.TabIndex = 3
        Me.lblTotalReserve.Text = "Total Reserve:"
        '
        'lblRiskType
        '
        Me.lblRiskType.AutoSize = True
        Me.lblRiskType.BackColor = System.Drawing.SystemColors.Control
        Me.lblRiskType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRiskType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRiskType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRiskType.Location = New System.Drawing.Point(59, 44)
        Me.lblRiskType.Name = "lblRiskType"
        Me.lblRiskType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRiskType.Size = New System.Drawing.Size(58, 13)
        Me.lblRiskType.TabIndex = 5
        Me.lblRiskType.Text = "Risk Type:"
        '
        'lblForReserveType
        '
        Me.lblForReserveType.AutoSize = True
        Me.lblForReserveType.BackColor = System.Drawing.SystemColors.Control
        Me.lblForReserveType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblForReserveType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblForReserveType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblForReserveType.Location = New System.Drawing.Point(14, 20)
        Me.lblForReserveType.Name = "lblForReserveType"
        Me.lblForReserveType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblForReserveType.Size = New System.Drawing.Size(95, 13)
        Me.lblForReserveType.TabIndex = 1
        Me.lblForReserveType.Text = "For Reserve Type:"
        '
        'lblPaymentAdjustment
        '
        Me.lblPaymentAdjustment.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentAdjustment.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentAdjustment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentAdjustment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentAdjustment.Location = New System.Drawing.Point(10, 342)
        Me.lblPaymentAdjustment.Name = "lblPaymentAdjustment"
        Me.lblPaymentAdjustment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentAdjustment.Size = New System.Drawing.Size(375, 17)
        Me.lblPaymentAdjustment.TabIndex = 42
        '
        'frmPaymentDetails
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(541, 369)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.fraTotal)
        Me.Controls.Add(Me.fraTaxes)
        Me.Controls.Add(Me.fraPayment)
        Me.Controls.Add(Me.fraReserve)
        Me.Controls.Add(Me.lblPaymentAdjustment)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(177, 92)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPaymentDetails"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Payment Details"
        Me.fraTotal.ResumeLayout(False)
        Me.fraTotal.PerformLayout()
        Me.fraTaxes.ResumeLayout(False)
        Me.fraTaxes.PerformLayout()
        Me.fraPayment.ResumeLayout(False)
        Me.fraPayment.PerformLayout()
        Me.fraReserve.ResumeLayout(False)
        Me.fraReserve.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class