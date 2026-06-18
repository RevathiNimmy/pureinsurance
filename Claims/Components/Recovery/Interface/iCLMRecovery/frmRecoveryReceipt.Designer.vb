<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRecoveryReceipt
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents txtBalance As System.Windows.Forms.TextBox
	Public WithEvents txtNetReceipt As System.Windows.Forms.TextBox
	Public WithEvents txtLossCurrency As System.Windows.Forms.TextBox
	Public WithEvents txtCurrentBalance As System.Windows.Forms.TextBox
	Public WithEvents txtReceivedToDate As System.Windows.Forms.TextBox
	Public WithEvents txtRecoveryType As System.Windows.Forms.TextBox
	Public WithEvents txtTotalReserve As System.Windows.Forms.TextBox
	Public WithEvents txtInitialReserve As System.Windows.Forms.TextBox
	Public WithEvents txtThisReceipt As System.Windows.Forms.TextBox
	Public WithEvents txtCurrencyRate As System.Windows.Forms.TextBox
	Public WithEvents txtThisReceiptLoss As System.Windows.Forms.TextBox
	Public WithEvents cboTaxType As System.Windows.Forms.ComboBox
	Public WithEvents cboTaxBand As System.Windows.Forms.ComboBox
	Public WithEvents txtTaxAmount As System.Windows.Forms.TextBox
	Public WithEvents txtTaxAmountLoss As System.Windows.Forms.TextBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents txtNetReceiptLoss As System.Windows.Forms.TextBox
	Public WithEvents divTax As uSIRCommonControls.uctDivider
	Public WithEvents cboReceiptCurrency As UserControls.CurrencyLookup
	Public WithEvents divReceipt As uSIRCommonControls.uctDivider
	Public WithEvents divReserve As uSIRCommonControls.uctDivider
	Public WithEvents uctNetReceipt As uSIRCommonControls.uctDivider
	Public WithEvents lblBalance As System.Windows.Forms.Label
	Public WithEvents lblNetReceipt As System.Windows.Forms.Label
	Public WithEvents lblThisReceiptLoss As System.Windows.Forms.Label
	Public WithEvents lblTaxAmountLoss As System.Windows.Forms.Label
	Public WithEvents lblNetReceiptLoss As System.Windows.Forms.Label
	Public WithEvents lblLossCurrency As System.Windows.Forms.Label
	Public WithEvents lblCurrentBalance As System.Windows.Forms.Label
	Public WithEvents lblReceivedToDate As System.Windows.Forms.Label
	Public WithEvents lblTotalReserve As System.Windows.Forms.Label
	Public WithEvents lblInitialReserve As System.Windows.Forms.Label
	Public WithEvents lblRecoveryType As System.Windows.Forms.Label
	Public WithEvents lblReceiptCurrency As System.Windows.Forms.Label
	Public WithEvents lblCurrencyRate As System.Windows.Forms.Label
	Public WithEvents lblThisReceipt As System.Windows.Forms.Label
	Public WithEvents lblLoss As System.Windows.Forms.Label
	Public WithEvents lblReceipt As System.Windows.Forms.Label
	Public WithEvents lblTaxType As System.Windows.Forms.Label
	Public WithEvents lblTaxBand As System.Windows.Forms.Label
	Public WithEvents lblTaxAmount As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRecoveryReceipt))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.txtBalance = New System.Windows.Forms.TextBox
		Me.txtNetReceipt = New System.Windows.Forms.TextBox
		Me.txtLossCurrency = New System.Windows.Forms.TextBox
		Me.txtCurrentBalance = New System.Windows.Forms.TextBox
		Me.txtReceivedToDate = New System.Windows.Forms.TextBox
		Me.txtRecoveryType = New System.Windows.Forms.TextBox
		Me.txtTotalReserve = New System.Windows.Forms.TextBox
		Me.txtInitialReserve = New System.Windows.Forms.TextBox
		Me.txtThisReceipt = New System.Windows.Forms.TextBox
		Me.txtCurrencyRate = New System.Windows.Forms.TextBox
		Me.txtThisReceiptLoss = New System.Windows.Forms.TextBox
		Me.cboTaxType = New System.Windows.Forms.ComboBox
		Me.cboTaxBand = New System.Windows.Forms.ComboBox
		Me.txtTaxAmount = New System.Windows.Forms.TextBox
		Me.txtTaxAmountLoss = New System.Windows.Forms.TextBox
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.txtNetReceiptLoss = New System.Windows.Forms.TextBox
		Me.divTax = New uSIRCommonControls.uctDivider
		Me.cboReceiptCurrency = New UserControls.CurrencyLookup
		Me.divReceipt = New uSIRCommonControls.uctDivider
		Me.divReserve = New uSIRCommonControls.uctDivider
		Me.uctNetReceipt = New uSIRCommonControls.uctDivider
		Me.lblBalance = New System.Windows.Forms.Label
		Me.lblNetReceipt = New System.Windows.Forms.Label
		Me.lblThisReceiptLoss = New System.Windows.Forms.Label
		Me.lblTaxAmountLoss = New System.Windows.Forms.Label
		Me.lblNetReceiptLoss = New System.Windows.Forms.Label
		Me.lblLossCurrency = New System.Windows.Forms.Label
		Me.lblCurrentBalance = New System.Windows.Forms.Label
		Me.lblReceivedToDate = New System.Windows.Forms.Label
		Me.lblTotalReserve = New System.Windows.Forms.Label
		Me.lblInitialReserve = New System.Windows.Forms.Label
		Me.lblRecoveryType = New System.Windows.Forms.Label
		Me.lblReceiptCurrency = New System.Windows.Forms.Label
		Me.lblCurrencyRate = New System.Windows.Forms.Label
		Me.lblThisReceipt = New System.Windows.Forms.Label
		Me.lblLoss = New System.Windows.Forms.Label
		Me.lblReceipt = New System.Windows.Forms.Label
		Me.lblTaxType = New System.Windows.Forms.Label
		Me.lblTaxBand = New System.Windows.Forms.Label
		Me.lblTaxAmount = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' txtBalance
		' 
		Me.txtBalance.AcceptsReturn = True
		Me.txtBalance.AutoSize = False
		Me.txtBalance.BackColor = System.Drawing.SystemColors.Control
		Me.txtBalance.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtBalance.CausesValidation = True
		Me.txtBalance.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtBalance.Enabled = True
		Me.txtBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtBalance.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtBalance.HideSelection = True
		Me.txtBalance.Location = New System.Drawing.Point(420, 217)
		Me.txtBalance.MaxLength = 0
		Me.txtBalance.Multiline = False
		Me.txtBalance.Name = "txtBalance"
		Me.txtBalance.ReadOnly = True
		Me.txtBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtBalance.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtBalance.Size = New System.Drawing.Size(169, 19)
		Me.txtBalance.TabIndex = 40
		Me.txtBalance.TabStop = True
		Me.txtBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtBalance.Visible = True
		' 
		' txtNetReceipt
		' 
		Me.txtNetReceipt.AcceptsReturn = True
		Me.txtNetReceipt.AutoSize = False
		Me.txtNetReceipt.BackColor = System.Drawing.SystemColors.Control
		Me.txtNetReceipt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtNetReceipt.CausesValidation = True
		Me.txtNetReceipt.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtNetReceipt.Enabled = True
		Me.txtNetReceipt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtNetReceipt.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtNetReceipt.HideSelection = True
		Me.txtNetReceipt.Location = New System.Drawing.Point(128, 364)
		Me.txtNetReceipt.MaxLength = 0
		Me.txtNetReceipt.Multiline = False
		Me.txtNetReceipt.Name = "txtNetReceipt"
		Me.txtNetReceipt.ReadOnly = True
		Me.txtNetReceipt.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtNetReceipt.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtNetReceipt.Size = New System.Drawing.Size(169, 19)
		Me.txtNetReceipt.TabIndex = 35
		Me.txtNetReceipt.TabStop = True
		Me.txtNetReceipt.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtNetReceipt.Visible = True
		' 
		' txtLossCurrency
		' 
		Me.txtLossCurrency.AcceptsReturn = True
		Me.txtLossCurrency.AutoSize = False
		Me.txtLossCurrency.BackColor = System.Drawing.SystemColors.Control
		Me.txtLossCurrency.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtLossCurrency.CausesValidation = True
		Me.txtLossCurrency.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtLossCurrency.Enabled = True
		Me.txtLossCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtLossCurrency.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtLossCurrency.HideSelection = True
		Me.txtLossCurrency.Location = New System.Drawing.Point(418, 143)
		Me.txtLossCurrency.MaxLength = 0
		Me.txtLossCurrency.Multiline = False
		Me.txtLossCurrency.Name = "txtLossCurrency"
		Me.txtLossCurrency.ReadOnly = True
		Me.txtLossCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtLossCurrency.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtLossCurrency.Size = New System.Drawing.Size(169, 19)
		Me.txtLossCurrency.TabIndex = 6
		Me.txtLossCurrency.TabStop = True
		Me.txtLossCurrency.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtLossCurrency.Visible = True
		' 
		' txtCurrentBalance
		' 
		Me.txtCurrentBalance.AcceptsReturn = True
		Me.txtCurrentBalance.AutoSize = False
		Me.txtCurrentBalance.BackColor = System.Drawing.SystemColors.Control
		Me.txtCurrentBalance.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtCurrentBalance.CausesValidation = True
		Me.txtCurrentBalance.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtCurrentBalance.Enabled = True
		Me.txtCurrentBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtCurrentBalance.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtCurrentBalance.HideSelection = True
		Me.txtCurrentBalance.Location = New System.Drawing.Point(420, 78)
		Me.txtCurrentBalance.MaxLength = 0
		Me.txtCurrentBalance.Multiline = False
		Me.txtCurrentBalance.Name = "txtCurrentBalance"
		Me.txtCurrentBalance.ReadOnly = True
		Me.txtCurrentBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtCurrentBalance.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtCurrentBalance.Size = New System.Drawing.Size(169, 19)
		Me.txtCurrentBalance.TabIndex = 12
		Me.txtCurrentBalance.TabStop = True
		Me.txtCurrentBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtCurrentBalance.Visible = True
		' 
		' txtReceivedToDate
		' 
		Me.txtReceivedToDate.AcceptsReturn = True
		Me.txtReceivedToDate.AutoSize = False
		Me.txtReceivedToDate.BackColor = System.Drawing.SystemColors.Control
		Me.txtReceivedToDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtReceivedToDate.CausesValidation = True
		Me.txtReceivedToDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtReceivedToDate.Enabled = True
		Me.txtReceivedToDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtReceivedToDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtReceivedToDate.HideSelection = True
		Me.txtReceivedToDate.Location = New System.Drawing.Point(420, 54)
		Me.txtReceivedToDate.MaxLength = 15
		Me.txtReceivedToDate.Multiline = False
		Me.txtReceivedToDate.Name = "txtReceivedToDate"
		Me.txtReceivedToDate.ReadOnly = True
		Me.txtReceivedToDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtReceivedToDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtReceivedToDate.Size = New System.Drawing.Size(169, 19)
		Me.txtReceivedToDate.TabIndex = 10
		Me.txtReceivedToDate.TabStop = True
		Me.txtReceivedToDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtReceivedToDate.Visible = True
		' 
		' txtRecoveryType
		' 
		Me.txtRecoveryType.AcceptsReturn = True
		Me.txtRecoveryType.AutoSize = False
		Me.txtRecoveryType.BackColor = System.Drawing.SystemColors.Control
		Me.txtRecoveryType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtRecoveryType.CausesValidation = True
		Me.txtRecoveryType.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtRecoveryType.Enabled = True
		Me.txtRecoveryType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtRecoveryType.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtRecoveryType.HideSelection = True
		Me.txtRecoveryType.Location = New System.Drawing.Point(128, 30)
		Me.txtRecoveryType.MaxLength = 15
		Me.txtRecoveryType.Multiline = False
		Me.txtRecoveryType.Name = "txtRecoveryType"
		Me.txtRecoveryType.ReadOnly = True
		Me.txtRecoveryType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtRecoveryType.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtRecoveryType.Size = New System.Drawing.Size(169, 19)
		Me.txtRecoveryType.TabIndex = 2
		Me.txtRecoveryType.TabStop = True
		Me.txtRecoveryType.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtRecoveryType.Visible = True
		' 
		' txtTotalReserve
		' 
		Me.txtTotalReserve.AcceptsReturn = True
		Me.txtTotalReserve.AutoSize = False
		Me.txtTotalReserve.BackColor = System.Drawing.SystemColors.Control
		Me.txtTotalReserve.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtTotalReserve.CausesValidation = True
		Me.txtTotalReserve.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtTotalReserve.Enabled = True
		Me.txtTotalReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtTotalReserve.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtTotalReserve.HideSelection = True
		Me.txtTotalReserve.Location = New System.Drawing.Point(420, 30)
		Me.txtTotalReserve.MaxLength = 15
		Me.txtTotalReserve.Multiline = False
		Me.txtTotalReserve.Name = "txtTotalReserve"
		Me.txtTotalReserve.ReadOnly = True
		Me.txtTotalReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTotalReserve.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTotalReserve.Size = New System.Drawing.Size(169, 19)
		Me.txtTotalReserve.TabIndex = 8
		Me.txtTotalReserve.TabStop = True
		Me.txtTotalReserve.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTotalReserve.Visible = True
		' 
		' txtInitialReserve
		' 
		Me.txtInitialReserve.AcceptsReturn = True
		Me.txtInitialReserve.AutoSize = False
		Me.txtInitialReserve.BackColor = System.Drawing.SystemColors.Control
		Me.txtInitialReserve.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtInitialReserve.CausesValidation = True
		Me.txtInitialReserve.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtInitialReserve.Enabled = True
		Me.txtInitialReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtInitialReserve.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtInitialReserve.HideSelection = True
		Me.txtInitialReserve.Location = New System.Drawing.Point(128, 54)
		Me.txtInitialReserve.MaxLength = 15
		Me.txtInitialReserve.Multiline = False
		Me.txtInitialReserve.Name = "txtInitialReserve"
		Me.txtInitialReserve.ReadOnly = True
		Me.txtInitialReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtInitialReserve.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtInitialReserve.Size = New System.Drawing.Size(169, 19)
		Me.txtInitialReserve.TabIndex = 4
		Me.txtInitialReserve.TabStop = True
		Me.txtInitialReserve.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtInitialReserve.Visible = True
		' 
		' txtThisReceipt
		' 
		Me.txtThisReceipt.AcceptsReturn = True
		Me.txtThisReceipt.AutoSize = False
		Me.txtThisReceipt.BackColor = System.Drawing.SystemColors.Window
		Me.txtThisReceipt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtThisReceipt.CausesValidation = True
		Me.txtThisReceipt.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtThisReceipt.Enabled = True
		Me.txtThisReceipt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtThisReceipt.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtThisReceipt.HideSelection = True
		Me.txtThisReceipt.Location = New System.Drawing.Point(128, 193)
		Me.txtThisReceipt.MaxLength = 0
		Me.txtThisReceipt.Multiline = False
		Me.txtThisReceipt.Name = "txtThisReceipt"
		Me.txtThisReceipt.ReadOnly = False
		Me.txtThisReceipt.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtThisReceipt.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtThisReceipt.Size = New System.Drawing.Size(169, 19)
		Me.txtThisReceipt.TabIndex = 20
		Me.txtThisReceipt.TabStop = True
		Me.txtThisReceipt.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtThisReceipt.Visible = True
		' 
		' txtCurrencyRate
		' 
		Me.txtCurrencyRate.AcceptsReturn = True
		Me.txtCurrencyRate.AutoSize = False
		Me.txtCurrencyRate.BackColor = System.Drawing.SystemColors.Window
		Me.txtCurrencyRate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtCurrencyRate.CausesValidation = True
		Me.txtCurrencyRate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtCurrencyRate.Enabled = True
		Me.txtCurrencyRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtCurrencyRate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtCurrencyRate.HideSelection = True
		Me.txtCurrencyRate.Location = New System.Drawing.Point(128, 168)
		Me.txtCurrencyRate.MaxLength = 0
		Me.txtCurrencyRate.Multiline = False
		Me.txtCurrencyRate.Name = "txtCurrencyRate"
		Me.txtCurrencyRate.ReadOnly = False
		Me.txtCurrencyRate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtCurrencyRate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtCurrencyRate.Size = New System.Drawing.Size(169, 19)
		Me.txtCurrencyRate.TabIndex = 18
		Me.txtCurrencyRate.TabStop = True
		Me.txtCurrencyRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtCurrencyRate.Visible = True
		' 
		' txtThisReceiptLoss
		' 
		Me.txtThisReceiptLoss.AcceptsReturn = True
		Me.txtThisReceiptLoss.AutoSize = False
		Me.txtThisReceiptLoss.BackColor = System.Drawing.SystemColors.Control
		Me.txtThisReceiptLoss.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtThisReceiptLoss.CausesValidation = True
		Me.txtThisReceiptLoss.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtThisReceiptLoss.Enabled = True
		Me.txtThisReceiptLoss.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtThisReceiptLoss.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtThisReceiptLoss.HideSelection = True
		Me.txtThisReceiptLoss.Location = New System.Drawing.Point(420, 193)
		Me.txtThisReceiptLoss.MaxLength = 15
		Me.txtThisReceiptLoss.Multiline = False
		Me.txtThisReceiptLoss.Name = "txtThisReceiptLoss"
		Me.txtThisReceiptLoss.ReadOnly = True
		Me.txtThisReceiptLoss.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtThisReceiptLoss.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtThisReceiptLoss.Size = New System.Drawing.Size(169, 19)
		Me.txtThisReceiptLoss.TabIndex = 23
		Me.txtThisReceiptLoss.TabStop = True
		Me.txtThisReceiptLoss.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtThisReceiptLoss.Visible = True
		' 
		' cboTaxType
		' 
		Me.cboTaxType.BackColor = System.Drawing.SystemColors.Window
		Me.cboTaxType.CausesValidation = True
		Me.cboTaxType.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboTaxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboTaxType.Enabled = True
		Me.cboTaxType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboTaxType.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboTaxType.IntegralHeight = True
		Me.cboTaxType.Location = New System.Drawing.Point(128, 264)
		Me.cboTaxType.Name = "cboTaxType"
		Me.cboTaxType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboTaxType.Size = New System.Drawing.Size(169, 21)
		Me.cboTaxType.Sorted = False
		Me.cboTaxType.TabIndex = 26
		Me.cboTaxType.TabStop = True
		Me.cboTaxType.Visible = True
		' 
		' cboTaxBand
		' 
		Me.cboTaxBand.BackColor = System.Drawing.SystemColors.Window
		Me.cboTaxBand.CausesValidation = True
		Me.cboTaxBand.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboTaxBand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboTaxBand.Enabled = True
		Me.cboTaxBand.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboTaxBand.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboTaxBand.IntegralHeight = True
		Me.cboTaxBand.Location = New System.Drawing.Point(128, 290)
		Me.cboTaxBand.Name = "cboTaxBand"
		Me.cboTaxBand.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboTaxBand.Size = New System.Drawing.Size(169, 21)
		Me.cboTaxBand.Sorted = False
		Me.cboTaxBand.TabIndex = 28
		Me.cboTaxBand.TabStop = True
		Me.cboTaxBand.Visible = True
		' 
		' txtTaxAmount
		' 
		Me.txtTaxAmount.AcceptsReturn = True
		Me.txtTaxAmount.AutoSize = False
		Me.txtTaxAmount.BackColor = System.Drawing.SystemColors.Window
		Me.txtTaxAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtTaxAmount.CausesValidation = True
		Me.txtTaxAmount.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtTaxAmount.Enabled = True
		Me.txtTaxAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtTaxAmount.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtTaxAmount.HideSelection = True
		Me.txtTaxAmount.Location = New System.Drawing.Point(128, 316)
		Me.txtTaxAmount.MaxLength = 0
		Me.txtTaxAmount.Multiline = False
		Me.txtTaxAmount.Name = "txtTaxAmount"
		Me.txtTaxAmount.ReadOnly = False
		Me.txtTaxAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTaxAmount.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTaxAmount.Size = New System.Drawing.Size(169, 19)
		Me.txtTaxAmount.TabIndex = 30
		Me.txtTaxAmount.TabStop = True
		Me.txtTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTaxAmount.Visible = True
		' 
		' txtTaxAmountLoss
		' 
		Me.txtTaxAmountLoss.AcceptsReturn = True
		Me.txtTaxAmountLoss.AutoSize = False
		Me.txtTaxAmountLoss.BackColor = System.Drawing.SystemColors.Control
		Me.txtTaxAmountLoss.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtTaxAmountLoss.CausesValidation = True
		Me.txtTaxAmountLoss.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtTaxAmountLoss.Enabled = True
		Me.txtTaxAmountLoss.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtTaxAmountLoss.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtTaxAmountLoss.HideSelection = True
		Me.txtTaxAmountLoss.Location = New System.Drawing.Point(420, 316)
		Me.txtTaxAmountLoss.MaxLength = 0
		Me.txtTaxAmountLoss.Multiline = False
		Me.txtTaxAmountLoss.Name = "txtTaxAmountLoss"
		Me.txtTaxAmountLoss.ReadOnly = True
		Me.txtTaxAmountLoss.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTaxAmountLoss.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTaxAmountLoss.Size = New System.Drawing.Size(169, 19)
		Me.txtTaxAmountLoss.TabIndex = 32
		Me.txtTaxAmountLoss.TabStop = True
		Me.txtTaxAmountLoss.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTaxAmountLoss.Visible = True
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(516, 395)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(80, 22)
		Me.cmdCancel.TabIndex = 39
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(430, 395)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(80, 22)
		Me.cmdOK.TabIndex = 38
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtNetReceiptLoss
		' 
		Me.txtNetReceiptLoss.AcceptsReturn = True
		Me.txtNetReceiptLoss.AutoSize = False
		Me.txtNetReceiptLoss.BackColor = System.Drawing.SystemColors.Control
		Me.txtNetReceiptLoss.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtNetReceiptLoss.CausesValidation = True
		Me.txtNetReceiptLoss.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtNetReceiptLoss.Enabled = True
		Me.txtNetReceiptLoss.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtNetReceiptLoss.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtNetReceiptLoss.HideSelection = True
		Me.txtNetReceiptLoss.Location = New System.Drawing.Point(420, 364)
		Me.txtNetReceiptLoss.MaxLength = 0
		Me.txtNetReceiptLoss.Multiline = False
		Me.txtNetReceiptLoss.Name = "txtNetReceiptLoss"
		Me.txtNetReceiptLoss.ReadOnly = True
		Me.txtNetReceiptLoss.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtNetReceiptLoss.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtNetReceiptLoss.Size = New System.Drawing.Size(169, 19)
		Me.txtNetReceiptLoss.TabIndex = 37
		Me.txtNetReceiptLoss.TabStop = True
		Me.txtNetReceiptLoss.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtNetReceiptLoss.Visible = True
		' 
		' divTax
		' 
		Me.divTax.Caption = "Taxes"
		Me.divTax.Location = New System.Drawing.Point(6, 242)
		Me.divTax.Name = "divTax"
		Me.divTax.Size = New System.Drawing.Size(591, 21)
		Me.divTax.TabIndex = 24
		Me.divTax.TabStop = False
		' 
		' cboReceiptCurrency
		' 
		Me.cboReceiptCurrency.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboReceiptCurrency.Location = New System.Drawing.Point(128, 142)
		Me.cboReceiptCurrency.Name = "cboReceiptCurrency"
		Me.cboReceiptCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actCompanyCurrencies
		Me.cboReceiptCurrency.Size = New System.Drawing.Size(169, 21)
		Me.cboReceiptCurrency.TabIndex = 16
		' 
		' divReceipt
		' 
		Me.divReceipt.Caption = "Receipt"
		Me.divReceipt.Location = New System.Drawing.Point(6, 104)
		Me.divReceipt.Name = "divReceipt"
		Me.divReceipt.Size = New System.Drawing.Size(591, 21)
		Me.divReceipt.TabIndex = 13
		Me.divReceipt.TabStop = False
		' 
		' divReserve
		' 
		Me.divReserve.Caption = "Reserve"
		Me.divReserve.Location = New System.Drawing.Point(6, 8)
		Me.divReserve.Name = "divReserve"
		Me.divReserve.Size = New System.Drawing.Size(591, 21)
		Me.divReserve.TabIndex = 0
		Me.divReserve.TabStop = False
		' 
		' uctNetReceipt
		' 
		Me.uctNetReceipt.Caption = "Net Receipt"
		Me.uctNetReceipt.Location = New System.Drawing.Point(6, 342)
		Me.uctNetReceipt.Name = "uctNetReceipt"
		Me.uctNetReceipt.Size = New System.Drawing.Size(591, 21)
		Me.uctNetReceipt.TabIndex = 33
		Me.uctNetReceipt.TabStop = False
		' 
		' lblBalance
		' 
		Me.lblBalance.AutoSize = True
		Me.lblBalance.BackColor = System.Drawing.SystemColors.Control
		Me.lblBalance.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblBalance.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblBalance.Enabled = True
		Me.lblBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblBalance.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblBalance.Location = New System.Drawing.Point(310, 220)
		Me.lblBalance.Name = "lblBalance"
		Me.lblBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblBalance.Size = New System.Drawing.Size(50, 13)
		Me.lblBalance.TabIndex = 41
		Me.lblBalance.Text = "Balance:"
		Me.lblBalance.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblBalance.UseMnemonic = True
		Me.lblBalance.Visible = True
		' 
		' lblNetReceipt
		' 
		Me.lblNetReceipt.AutoSize = True
		Me.lblNetReceipt.BackColor = System.Drawing.SystemColors.Control
		Me.lblNetReceipt.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblNetReceipt.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblNetReceipt.Enabled = True
		Me.lblNetReceipt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblNetReceipt.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblNetReceipt.Location = New System.Drawing.Point(18, 366)
		Me.lblNetReceipt.Name = "lblNetReceipt"
		Me.lblNetReceipt.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblNetReceipt.Size = New System.Drawing.Size(70, 13)
		Me.lblNetReceipt.TabIndex = 34
		Me.lblNetReceipt.Text = "Net Receipt:"
		Me.lblNetReceipt.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblNetReceipt.UseMnemonic = True
		Me.lblNetReceipt.Visible = True
		' 
		' lblThisReceiptLoss
		' 
		Me.lblThisReceiptLoss.AutoSize = True
		Me.lblThisReceiptLoss.BackColor = System.Drawing.SystemColors.Control
		Me.lblThisReceiptLoss.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblThisReceiptLoss.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblThisReceiptLoss.Enabled = True
		Me.lblThisReceiptLoss.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblThisReceiptLoss.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblThisReceiptLoss.Location = New System.Drawing.Point(310, 196)
		Me.lblThisReceiptLoss.Name = "lblThisReceiptLoss"
		Me.lblThisReceiptLoss.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblThisReceiptLoss.Size = New System.Drawing.Size(74, 13)
		Me.lblThisReceiptLoss.TabIndex = 22
		Me.lblThisReceiptLoss.Text = "This Receipt:"
		Me.lblThisReceiptLoss.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblThisReceiptLoss.UseMnemonic = True
		Me.lblThisReceiptLoss.Visible = True
		' 
		' lblTaxAmountLoss
		' 
		Me.lblTaxAmountLoss.AutoSize = True
		Me.lblTaxAmountLoss.BackColor = System.Drawing.SystemColors.Control
		Me.lblTaxAmountLoss.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTaxAmountLoss.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTaxAmountLoss.Enabled = True
		Me.lblTaxAmountLoss.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTaxAmountLoss.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTaxAmountLoss.Location = New System.Drawing.Point(310, 319)
		Me.lblTaxAmountLoss.Name = "lblTaxAmountLoss"
		Me.lblTaxAmountLoss.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTaxAmountLoss.Size = New System.Drawing.Size(74, 13)
		Me.lblTaxAmountLoss.TabIndex = 31
		Me.lblTaxAmountLoss.Text = "Tax Amount:"
		Me.lblTaxAmountLoss.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTaxAmountLoss.UseMnemonic = True
		Me.lblTaxAmountLoss.Visible = True
		' 
		' lblNetReceiptLoss
		' 
		Me.lblNetReceiptLoss.AutoSize = True
		Me.lblNetReceiptLoss.BackColor = System.Drawing.SystemColors.Control
		Me.lblNetReceiptLoss.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblNetReceiptLoss.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblNetReceiptLoss.Enabled = True
		Me.lblNetReceiptLoss.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblNetReceiptLoss.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblNetReceiptLoss.Location = New System.Drawing.Point(310, 367)
		Me.lblNetReceiptLoss.Name = "lblNetReceiptLoss"
		Me.lblNetReceiptLoss.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblNetReceiptLoss.Size = New System.Drawing.Size(70, 13)
		Me.lblNetReceiptLoss.TabIndex = 36
		Me.lblNetReceiptLoss.Text = "Net Receipt:"
		Me.lblNetReceiptLoss.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblNetReceiptLoss.UseMnemonic = True
		Me.lblNetReceiptLoss.Visible = True
		' 
		' lblLossCurrency
		' 
		Me.lblLossCurrency.AutoSize = True
		Me.lblLossCurrency.BackColor = System.Drawing.SystemColors.Control
		Me.lblLossCurrency.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblLossCurrency.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblLossCurrency.Enabled = True
		Me.lblLossCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblLossCurrency.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLossCurrency.Location = New System.Drawing.Point(310, 146)
		Me.lblLossCurrency.Name = "lblLossCurrency"
		Me.lblLossCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblLossCurrency.Size = New System.Drawing.Size(58, 13)
		Me.lblLossCurrency.TabIndex = 5
		Me.lblLossCurrency.Text = "Currency:"
		Me.lblLossCurrency.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblLossCurrency.UseMnemonic = True
		Me.lblLossCurrency.Visible = True
		' 
		' lblCurrentBalance
		' 
		Me.lblCurrentBalance.AutoSize = True
		Me.lblCurrentBalance.BackColor = System.Drawing.SystemColors.Control
		Me.lblCurrentBalance.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCurrentBalance.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCurrentBalance.Enabled = True
		Me.lblCurrentBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCurrentBalance.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCurrentBalance.Location = New System.Drawing.Point(310, 81)
		Me.lblCurrentBalance.Name = "lblCurrentBalance"
		Me.lblCurrentBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCurrentBalance.Size = New System.Drawing.Size(50, 13)
		Me.lblCurrentBalance.TabIndex = 11
		Me.lblCurrentBalance.Text = "Balance:"
		Me.lblCurrentBalance.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCurrentBalance.UseMnemonic = True
		Me.lblCurrentBalance.Visible = True
		' 
		' lblReceivedToDate
		' 
		Me.lblReceivedToDate.AutoSize = True
		Me.lblReceivedToDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblReceivedToDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblReceivedToDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblReceivedToDate.Enabled = True
		Me.lblReceivedToDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblReceivedToDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblReceivedToDate.Location = New System.Drawing.Point(310, 57)
		Me.lblReceivedToDate.Name = "lblReceivedToDate"
		Me.lblReceivedToDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblReceivedToDate.Size = New System.Drawing.Size(106, 13)
		Me.lblReceivedToDate.TabIndex = 9
		Me.lblReceivedToDate.Text = "Received To Date:"
		Me.lblReceivedToDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblReceivedToDate.UseMnemonic = True
		Me.lblReceivedToDate.Visible = True
		' 
		' lblTotalReserve
		' 
		Me.lblTotalReserve.AutoSize = True
		Me.lblTotalReserve.BackColor = System.Drawing.SystemColors.Control
		Me.lblTotalReserve.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTotalReserve.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTotalReserve.Enabled = True
		Me.lblTotalReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTotalReserve.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTotalReserve.Location = New System.Drawing.Point(310, 33)
		Me.lblTotalReserve.Name = "lblTotalReserve"
		Me.lblTotalReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTotalReserve.Size = New System.Drawing.Size(84, 13)
		Me.lblTotalReserve.TabIndex = 7
		Me.lblTotalReserve.Text = "Total Reserve:"
		Me.lblTotalReserve.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTotalReserve.UseMnemonic = True
		Me.lblTotalReserve.Visible = True
		' 
		' lblInitialReserve
		' 
		Me.lblInitialReserve.AutoSize = True
		Me.lblInitialReserve.BackColor = System.Drawing.SystemColors.Control
		Me.lblInitialReserve.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblInitialReserve.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblInitialReserve.Enabled = True
		Me.lblInitialReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblInitialReserve.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblInitialReserve.Location = New System.Drawing.Point(18, 57)
		Me.lblInitialReserve.Name = "lblInitialReserve"
		Me.lblInitialReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblInitialReserve.Size = New System.Drawing.Size(88, 13)
		Me.lblInitialReserve.TabIndex = 3
		Me.lblInitialReserve.Text = "Initial Reserve:"
		Me.lblInitialReserve.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblInitialReserve.UseMnemonic = True
		Me.lblInitialReserve.Visible = True
		' 
		' lblRecoveryType
		' 
		Me.lblRecoveryType.AutoSize = True
		Me.lblRecoveryType.BackColor = System.Drawing.SystemColors.Control
		Me.lblRecoveryType.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblRecoveryType.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblRecoveryType.Enabled = True
		Me.lblRecoveryType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblRecoveryType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblRecoveryType.Location = New System.Drawing.Point(18, 31)
		Me.lblRecoveryType.Name = "lblRecoveryType"
		Me.lblRecoveryType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblRecoveryType.Size = New System.Drawing.Size(91, 13)
		Me.lblRecoveryType.TabIndex = 1
		Me.lblRecoveryType.Text = "Recovery Type:"
		Me.lblRecoveryType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblRecoveryType.UseMnemonic = True
		Me.lblRecoveryType.Visible = True
		' 
		' lblReceiptCurrency
		' 
		Me.lblReceiptCurrency.AutoSize = True
		Me.lblReceiptCurrency.BackColor = System.Drawing.SystemColors.Control
		Me.lblReceiptCurrency.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblReceiptCurrency.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblReceiptCurrency.Enabled = True
		Me.lblReceiptCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblReceiptCurrency.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblReceiptCurrency.Location = New System.Drawing.Point(18, 146)
		Me.lblReceiptCurrency.Name = "lblReceiptCurrency"
		Me.lblReceiptCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblReceiptCurrency.Size = New System.Drawing.Size(58, 13)
		Me.lblReceiptCurrency.TabIndex = 15
		Me.lblReceiptCurrency.Text = "Currency:"
		Me.lblReceiptCurrency.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblReceiptCurrency.UseMnemonic = True
		Me.lblReceiptCurrency.Visible = True
		' 
		' lblCurrencyRate
		' 
		Me.lblCurrencyRate.AutoSize = True
		Me.lblCurrencyRate.BackColor = System.Drawing.SystemColors.Control
		Me.lblCurrencyRate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCurrencyRate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCurrencyRate.Enabled = True
		Me.lblCurrencyRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCurrencyRate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCurrencyRate.Location = New System.Drawing.Point(18, 172)
		Me.lblCurrencyRate.Name = "lblCurrencyRate"
		Me.lblCurrencyRate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCurrencyRate.Size = New System.Drawing.Size(88, 13)
		Me.lblCurrencyRate.TabIndex = 17
		Me.lblCurrencyRate.Text = "Currency Rate:"
		Me.lblCurrencyRate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCurrencyRate.UseMnemonic = True
		Me.lblCurrencyRate.Visible = True
		' 
		' lblThisReceipt
		' 
		Me.lblThisReceipt.AutoSize = True
		Me.lblThisReceipt.BackColor = System.Drawing.SystemColors.Control
		Me.lblThisReceipt.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblThisReceipt.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblThisReceipt.Enabled = True
		Me.lblThisReceipt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblThisReceipt.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblThisReceipt.Location = New System.Drawing.Point(18, 196)
		Me.lblThisReceipt.Name = "lblThisReceipt"
		Me.lblThisReceipt.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblThisReceipt.Size = New System.Drawing.Size(74, 13)
		Me.lblThisReceipt.TabIndex = 19
		Me.lblThisReceipt.Text = "This Receipt:"
		Me.lblThisReceipt.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblThisReceipt.UseMnemonic = True
		Me.lblThisReceipt.Visible = True
		' 
		' lblLoss
		' 
		Me.lblLoss.AutoSize = True
		Me.lblLoss.BackColor = System.Drawing.SystemColors.Control
		Me.lblLoss.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblLoss.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblLoss.Enabled = True
		Me.lblLoss.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblLoss.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLoss.Location = New System.Drawing.Point(420, 124)
		Me.lblLoss.Name = "lblLoss"
		Me.lblLoss.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblLoss.Size = New System.Drawing.Size(82, 13)
		Me.lblLoss.TabIndex = 21
		Me.lblLoss.Text = "Loss Currency"
		Me.lblLoss.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblLoss.UseMnemonic = True
		Me.lblLoss.Visible = True
		' 
		' lblReceipt
		' 
		Me.lblReceipt.AutoSize = True
		Me.lblReceipt.BackColor = System.Drawing.SystemColors.Control
		Me.lblReceipt.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblReceipt.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblReceipt.Enabled = True
		Me.lblReceipt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblReceipt.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblReceipt.Location = New System.Drawing.Point(128, 124)
		Me.lblReceipt.Name = "lblReceipt"
		Me.lblReceipt.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblReceipt.Size = New System.Drawing.Size(99, 13)
		Me.lblReceipt.TabIndex = 14
		Me.lblReceipt.Text = "Receipt Currency"
		Me.lblReceipt.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblReceipt.UseMnemonic = True
		Me.lblReceipt.Visible = True
		' 
		' lblTaxType
		' 
		Me.lblTaxType.AutoSize = True
		Me.lblTaxType.BackColor = System.Drawing.SystemColors.Control
		Me.lblTaxType.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTaxType.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTaxType.Enabled = True
		Me.lblTaxType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTaxType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTaxType.Location = New System.Drawing.Point(18, 268)
		Me.lblTaxType.Name = "lblTaxType"
		Me.lblTaxType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTaxType.Size = New System.Drawing.Size(58, 13)
		Me.lblTaxType.TabIndex = 25
		Me.lblTaxType.Text = "Tax Type:"
		Me.lblTaxType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTaxType.UseMnemonic = True
		Me.lblTaxType.Visible = True
		' 
		' lblTaxBand
		' 
		Me.lblTaxBand.AutoSize = True
		Me.lblTaxBand.BackColor = System.Drawing.SystemColors.Control
		Me.lblTaxBand.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTaxBand.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTaxBand.Enabled = True
		Me.lblTaxBand.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTaxBand.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTaxBand.Location = New System.Drawing.Point(18, 294)
		Me.lblTaxBand.Name = "lblTaxBand"
		Me.lblTaxBand.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTaxBand.Size = New System.Drawing.Size(59, 13)
		Me.lblTaxBand.TabIndex = 27
		Me.lblTaxBand.Text = "Tax Band:"
		Me.lblTaxBand.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTaxBand.UseMnemonic = True
		Me.lblTaxBand.Visible = True
		' 
		' lblTaxAmount
		' 
		Me.lblTaxAmount.AutoSize = True
		Me.lblTaxAmount.BackColor = System.Drawing.SystemColors.Control
		Me.lblTaxAmount.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTaxAmount.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTaxAmount.Enabled = True
		Me.lblTaxAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTaxAmount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTaxAmount.Location = New System.Drawing.Point(18, 319)
		Me.lblTaxAmount.Name = "lblTaxAmount"
		Me.lblTaxAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTaxAmount.Size = New System.Drawing.Size(74, 13)
		Me.lblTaxAmount.TabIndex = 29
		Me.lblTaxAmount.Text = "Tax Amount:"
		Me.lblTaxAmount.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTaxAmount.UseMnemonic = True
		Me.lblTaxAmount.Visible = True
		' 
		' frmRecoveryReceipt
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(605, 425)
		Me.ControlBox = True
		Me.Controls.Add(Me.txtBalance)
		Me.Controls.Add(Me.txtNetReceipt)
		Me.Controls.Add(Me.txtLossCurrency)
		Me.Controls.Add(Me.txtCurrentBalance)
		Me.Controls.Add(Me.txtReceivedToDate)
		Me.Controls.Add(Me.txtRecoveryType)
		Me.Controls.Add(Me.txtTotalReserve)
		Me.Controls.Add(Me.txtInitialReserve)
		Me.Controls.Add(Me.txtThisReceipt)
		Me.Controls.Add(Me.txtCurrencyRate)
		Me.Controls.Add(Me.txtThisReceiptLoss)
		Me.Controls.Add(Me.cboTaxType)
		Me.Controls.Add(Me.cboTaxBand)
		Me.Controls.Add(Me.txtTaxAmount)
		Me.Controls.Add(Me.txtTaxAmountLoss)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.txtNetReceiptLoss)
		Me.Controls.Add(Me.divTax)
		Me.Controls.Add(Me.cboReceiptCurrency)
		Me.Controls.Add(Me.divReceipt)
		Me.Controls.Add(Me.divReserve)
		Me.Controls.Add(Me.uctNetReceipt)
		Me.Controls.Add(Me.lblBalance)
		Me.Controls.Add(Me.lblNetReceipt)
		Me.Controls.Add(Me.lblThisReceiptLoss)
		Me.Controls.Add(Me.lblTaxAmountLoss)
		Me.Controls.Add(Me.lblNetReceiptLoss)
		Me.Controls.Add(Me.lblLossCurrency)
		Me.Controls.Add(Me.lblCurrentBalance)
		Me.Controls.Add(Me.lblReceivedToDate)
		Me.Controls.Add(Me.lblTotalReserve)
		Me.Controls.Add(Me.lblInitialReserve)
		Me.Controls.Add(Me.lblRecoveryType)
		Me.Controls.Add(Me.lblReceiptCurrency)
		Me.Controls.Add(Me.lblCurrencyRate)
		Me.Controls.Add(Me.lblThisReceipt)
		Me.Controls.Add(Me.lblLoss)
		Me.Controls.Add(Me.lblReceipt)
		Me.Controls.Add(Me.lblTaxType)
		Me.Controls.Add(Me.lblTaxBand)
		Me.Controls.Add(Me.lblTaxAmount)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmRecoveryReceipt"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Recovery Receipt"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class