<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmThirdPartyDetails
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
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
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblCurrentReserve As System.Windows.Forms.Label
	Public WithEvents lblRevisedReserve As System.Windows.Forms.Label
	Public WithEvents lblInitialReserve As System.Windows.Forms.Label
	Public WithEvents lblRecoveryType As System.Windows.Forms.Label
	Public WithEvents lblTaxType As System.Windows.Forms.Label
	Public WithEvents lblTaxBand As System.Windows.Forms.Label
	Public WithEvents lblTaxAmount As System.Windows.Forms.Label
	Public WithEvents lblNetPayment As System.Windows.Forms.Label
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	Public WithEvents lblCurrencyRate As System.Windows.Forms.Label
	Public WithEvents lblLossCurrency As System.Windows.Forms.Label
	Public WithEvents lblRevisedReserveLoss As System.Windows.Forms.Label
	Public WithEvents cboCurrency As UserControls.CurrencyLookup
	Public WithEvents txtCurrentReserve As System.Windows.Forms.TextBox
	Public WithEvents txtRevisedReserve As System.Windows.Forms.TextBox
	Public WithEvents txtInitialReserve As System.Windows.Forms.TextBox
	Public WithEvents cboRecoveryType As System.Windows.Forms.ComboBox
	Public WithEvents cboTaxType As System.Windows.Forms.ComboBox
	Public WithEvents cboTaxBand As System.Windows.Forms.ComboBox
	Public WithEvents txtTaxAmount As System.Windows.Forms.TextBox
	Public WithEvents txtNetPayment As System.Windows.Forms.TextBox
	Public WithEvents txtCurrencyRate As System.Windows.Forms.TextBox
	Public WithEvents txtLossCurrency As System.Windows.Forms.TextBox
	Public WithEvents txtRevisedReserveLoss As System.Windows.Forms.TextBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmThirdPartyDetails))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.tabMainTab = New System.Windows.Forms.TabControl
		Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
		Me.lblCurrentReserve = New System.Windows.Forms.Label
		Me.lblRevisedReserve = New System.Windows.Forms.Label
		Me.lblInitialReserve = New System.Windows.Forms.Label
		Me.lblRecoveryType = New System.Windows.Forms.Label
		Me.lblTaxType = New System.Windows.Forms.Label
		Me.lblTaxBand = New System.Windows.Forms.Label
		Me.lblTaxAmount = New System.Windows.Forms.Label
		Me.lblNetPayment = New System.Windows.Forms.Label
		Me.lblCurrency = New System.Windows.Forms.Label
		Me.lblCurrencyRate = New System.Windows.Forms.Label
		Me.lblLossCurrency = New System.Windows.Forms.Label
		Me.lblRevisedReserveLoss = New System.Windows.Forms.Label
		Me.cboCurrency = New UserControls.CurrencyLookup
		Me.txtCurrentReserve = New System.Windows.Forms.TextBox
		Me.txtRevisedReserve = New System.Windows.Forms.TextBox
		Me.txtInitialReserve = New System.Windows.Forms.TextBox
		Me.cboRecoveryType = New System.Windows.Forms.ComboBox
		Me.cboTaxType = New System.Windows.Forms.ComboBox
		Me.cboTaxBand = New System.Windows.Forms.ComboBox
		Me.txtTaxAmount = New System.Windows.Forms.TextBox
		Me.txtNetPayment = New System.Windows.Forms.TextBox
		Me.txtCurrencyRate = New System.Windows.Forms.TextBox
		Me.txtLossCurrency = New System.Windows.Forms.TextBox
		Me.txtRevisedReserveLoss = New System.Windows.Forms.TextBox
		Me.tabMainTab.SuspendLayout()
		Me._tabMainTab_TabPage0.SuspendLayout()
		Me.SuspendLayout()
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(264, 456)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 6
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
		Me.cmdOK.Location = New System.Drawing.Point(176, 456)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 5
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tabMainTab
		' 
		Me.tabMainTab.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabMainTab.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
		Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMainTab.ItemSize = New System.Drawing.Size(108, 18)
		Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
		Me.tabMainTab.Multiline = True
		Me.tabMainTab.Name = "tabMainTab"
		Me.tabMainTab.Size = New System.Drawing.Size(333, 445)
		Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMainTab.TabIndex = 0
		Me.tabMainTab.TabStop = False
		' 
		' _tabMainTab_TabPage0
		' 
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblCurrentReserve)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblRevisedReserve)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblInitialReserve)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblRecoveryType)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblTaxType)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblTaxBand)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblTaxAmount)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblNetPayment)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblCurrency)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblCurrencyRate)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblLossCurrency)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblRevisedReserveLoss)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cboCurrency)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtCurrentReserve)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtRevisedReserve)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtInitialReserve)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cboRecoveryType)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cboTaxType)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cboTaxBand)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtTaxAmount)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtNetPayment)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtCurrencyRate)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtLossCurrency)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtRevisedReserveLoss)
		Me._tabMainTab_TabPage0.Text = "&1 - General"
		' 
		' lblCurrentReserve
		' 
		Me.lblCurrentReserve.AutoSize = False
		Me.lblCurrentReserve.BackColor = System.Drawing.SystemColors.Control
		Me.lblCurrentReserve.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCurrentReserve.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCurrentReserve.Enabled = True
		Me.lblCurrentReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCurrentReserve.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCurrentReserve.Location = New System.Drawing.Point(16, 95)
		Me.lblCurrentReserve.Name = "lblCurrentReserve"
		Me.lblCurrentReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCurrentReserve.Size = New System.Drawing.Size(133, 25)
		Me.lblCurrentReserve.TabIndex = 7
		Me.lblCurrentReserve.Text = "Current  Reserve :"
		Me.lblCurrentReserve.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCurrentReserve.UseMnemonic = True
		Me.lblCurrentReserve.Visible = True
		' 
		' lblRevisedReserve
		' 
		Me.lblRevisedReserve.AutoSize = False
		Me.lblRevisedReserve.BackColor = System.Drawing.SystemColors.Control
		Me.lblRevisedReserve.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblRevisedReserve.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblRevisedReserve.Enabled = True
		Me.lblRevisedReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblRevisedReserve.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblRevisedReserve.Location = New System.Drawing.Point(16, 167)
		Me.lblRevisedReserve.Name = "lblRevisedReserve"
		Me.lblRevisedReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblRevisedReserve.Size = New System.Drawing.Size(133, 25)
		Me.lblRevisedReserve.TabIndex = 8
		Me.lblRevisedReserve.Text = "Revised  Reserve  :"
		Me.lblRevisedReserve.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblRevisedReserve.UseMnemonic = True
		Me.lblRevisedReserve.Visible = True
		' 
		' lblInitialReserve
		' 
		Me.lblInitialReserve.AutoSize = False
		Me.lblInitialReserve.BackColor = System.Drawing.SystemColors.Control
		Me.lblInitialReserve.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblInitialReserve.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblInitialReserve.Enabled = True
		Me.lblInitialReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblInitialReserve.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblInitialReserve.Location = New System.Drawing.Point(16, 55)
		Me.lblInitialReserve.Name = "lblInitialReserve"
		Me.lblInitialReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblInitialReserve.Size = New System.Drawing.Size(134, 25)
		Me.lblInitialReserve.TabIndex = 9
		Me.lblInitialReserve.Text = "Initial Reserve :"
		Me.lblInitialReserve.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblInitialReserve.UseMnemonic = True
		Me.lblInitialReserve.Visible = True
		' 
		' lblRecoveryType
		' 
		Me.lblRecoveryType.AutoSize = False
		Me.lblRecoveryType.BackColor = System.Drawing.SystemColors.Control
		Me.lblRecoveryType.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblRecoveryType.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblRecoveryType.Enabled = True
		Me.lblRecoveryType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblRecoveryType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblRecoveryType.Location = New System.Drawing.Point(16, 23)
		Me.lblRecoveryType.Name = "lblRecoveryType"
		Me.lblRecoveryType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblRecoveryType.Size = New System.Drawing.Size(134, 25)
		Me.lblRecoveryType.TabIndex = 10
		Me.lblRecoveryType.Text = "Recovery Type :"
		Me.lblRecoveryType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblRecoveryType.UseMnemonic = True
		Me.lblRecoveryType.Visible = True
		' 
		' lblTaxType
		' 
		Me.lblTaxType.AutoSize = False
		Me.lblTaxType.BackColor = System.Drawing.SystemColors.Control
		Me.lblTaxType.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTaxType.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTaxType.Enabled = True
		Me.lblTaxType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTaxType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTaxType.Location = New System.Drawing.Point(16, 292)
		Me.lblTaxType.Name = "lblTaxType"
		Me.lblTaxType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTaxType.Size = New System.Drawing.Size(137, 17)
		Me.lblTaxType.TabIndex = 15
		Me.lblTaxType.Text = "Tax Type :"
		Me.lblTaxType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTaxType.UseMnemonic = True
		Me.lblTaxType.Visible = True
		' 
		' lblTaxBand
		' 
		Me.lblTaxBand.AutoSize = False
		Me.lblTaxBand.BackColor = System.Drawing.SystemColors.Control
		Me.lblTaxBand.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTaxBand.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTaxBand.Enabled = True
		Me.lblTaxBand.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTaxBand.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTaxBand.Location = New System.Drawing.Point(16, 324)
		Me.lblTaxBand.Name = "lblTaxBand"
		Me.lblTaxBand.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTaxBand.Size = New System.Drawing.Size(137, 17)
		Me.lblTaxBand.TabIndex = 16
		Me.lblTaxBand.Text = "Tax Band :"
		Me.lblTaxBand.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTaxBand.UseMnemonic = True
		Me.lblTaxBand.Visible = True
		' 
		' lblTaxAmount
		' 
		Me.lblTaxAmount.AutoSize = False
		Me.lblTaxAmount.BackColor = System.Drawing.SystemColors.Control
		Me.lblTaxAmount.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTaxAmount.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTaxAmount.Enabled = True
		Me.lblTaxAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTaxAmount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTaxAmount.Location = New System.Drawing.Point(16, 356)
		Me.lblTaxAmount.Name = "lblTaxAmount"
		Me.lblTaxAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTaxAmount.Size = New System.Drawing.Size(145, 17)
		Me.lblTaxAmount.TabIndex = 17
		Me.lblTaxAmount.Text = "Tax Amount :"
		Me.lblTaxAmount.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTaxAmount.UseMnemonic = True
		Me.lblTaxAmount.Visible = True
		' 
		' lblNetPayment
		' 
		Me.lblNetPayment.AutoSize = False
		Me.lblNetPayment.BackColor = System.Drawing.SystemColors.Control
		Me.lblNetPayment.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblNetPayment.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblNetPayment.Enabled = True
		Me.lblNetPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblNetPayment.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblNetPayment.Location = New System.Drawing.Point(16, 388)
		Me.lblNetPayment.Name = "lblNetPayment"
		Me.lblNetPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblNetPayment.Size = New System.Drawing.Size(145, 17)
		Me.lblNetPayment.TabIndex = 18
		Me.lblNetPayment.Text = "Total incl Tax :"
		Me.lblNetPayment.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblNetPayment.UseMnemonic = True
		Me.lblNetPayment.Visible = True
		' 
		' lblCurrency
		' 
		Me.lblCurrency.AutoSize = False
		Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
		Me.lblCurrency.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCurrency.Enabled = True
		Me.lblCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCurrency.Location = New System.Drawing.Point(16, 220)
		Me.lblCurrency.Name = "lblCurrency"
		Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCurrency.Size = New System.Drawing.Size(125, 13)
		Me.lblCurrency.TabIndex = 22
		Me.lblCurrency.Text = "Receipt Currency :"
		Me.lblCurrency.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCurrency.UseMnemonic = True
		Me.lblCurrency.Visible = True
		' 
		' lblCurrencyRate
		' 
		Me.lblCurrencyRate.AutoSize = False
		Me.lblCurrencyRate.BackColor = System.Drawing.SystemColors.Control
		Me.lblCurrencyRate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCurrencyRate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCurrencyRate.Enabled = True
		Me.lblCurrencyRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCurrencyRate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCurrencyRate.Location = New System.Drawing.Point(16, 252)
		Me.lblCurrencyRate.Name = "lblCurrencyRate"
		Me.lblCurrencyRate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCurrencyRate.Size = New System.Drawing.Size(113, 25)
		Me.lblCurrencyRate.TabIndex = 23
		Me.lblCurrencyRate.Text = "Receipt To Loss Rate :"
		Me.lblCurrencyRate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCurrencyRate.UseMnemonic = True
		Me.lblCurrencyRate.Visible = True
		' 
		' lblLossCurrency
		' 
		Me.lblLossCurrency.AutoSize = False
		Me.lblLossCurrency.BackColor = System.Drawing.SystemColors.Control
		Me.lblLossCurrency.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblLossCurrency.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblLossCurrency.Enabled = True
		Me.lblLossCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblLossCurrency.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLossCurrency.Location = New System.Drawing.Point(16, 196)
		Me.lblLossCurrency.Name = "lblLossCurrency"
		Me.lblLossCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblLossCurrency.Size = New System.Drawing.Size(117, 13)
		Me.lblLossCurrency.TabIndex = 24
		Me.lblLossCurrency.Text = "Loss Currency :"
		Me.lblLossCurrency.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblLossCurrency.UseMnemonic = True
		Me.lblLossCurrency.Visible = True
		' 
		' lblRevisedReserveLoss
		' 
		Me.lblRevisedReserveLoss.AutoSize = False
		Me.lblRevisedReserveLoss.BackColor = System.Drawing.SystemColors.Control
		Me.lblRevisedReserveLoss.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblRevisedReserveLoss.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblRevisedReserveLoss.Enabled = True
		Me.lblRevisedReserveLoss.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblRevisedReserveLoss.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblRevisedReserveLoss.Location = New System.Drawing.Point(16, 124)
		Me.lblRevisedReserveLoss.Name = "lblRevisedReserveLoss"
		Me.lblRevisedReserveLoss.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblRevisedReserveLoss.Size = New System.Drawing.Size(113, 29)
		Me.lblRevisedReserveLoss.TabIndex = 26
		Me.lblRevisedReserveLoss.Text = "Receipt Amount : (Loss Currency)"
		Me.lblRevisedReserveLoss.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblRevisedReserveLoss.UseMnemonic = True
		Me.lblRevisedReserveLoss.Visible = True
		' 
		' cboCurrency
		' 
		Me.cboCurrency.Location = New System.Drawing.Point(144, 220)
		Me.cboCurrency.Name = "cboCurrency"
		Me.cboCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actCompanyCurrencies
		Me.cboCurrency.Size = New System.Drawing.Size(169, 21)
		Me.cboCurrency.TabIndex = 19
		' 
		' txtCurrentReserve
		' 
		Me.txtCurrentReserve.AcceptsReturn = True
		Me.txtCurrentReserve.AutoSize = False
		Me.txtCurrentReserve.BackColor = System.Drawing.SystemColors.Window
		Me.txtCurrentReserve.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtCurrentReserve.CausesValidation = True
		Me.txtCurrentReserve.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtCurrentReserve.Enabled = True
		Me.txtCurrentReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtCurrentReserve.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtCurrentReserve.HideSelection = True
		Me.txtCurrentReserve.Location = New System.Drawing.Point(144, 92)
		Me.txtCurrentReserve.MaxLength = 15
		Me.txtCurrentReserve.Multiline = False
		Me.txtCurrentReserve.Name = "txtCurrentReserve"
		Me.txtCurrentReserve.ReadOnly = False
		Me.txtCurrentReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtCurrentReserve.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtCurrentReserve.Size = New System.Drawing.Size(169, 19)
		Me.txtCurrentReserve.TabIndex = 4
		Me.txtCurrentReserve.TabStop = True
		Me.txtCurrentReserve.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtCurrentReserve.Visible = True
		' 
		' txtRevisedReserve
		' 
		Me.txtRevisedReserve.AcceptsReturn = True
		Me.txtRevisedReserve.AutoSize = False
		Me.txtRevisedReserve.BackColor = System.Drawing.SystemColors.Window
		Me.txtRevisedReserve.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtRevisedReserve.CausesValidation = True
		Me.txtRevisedReserve.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtRevisedReserve.Enabled = True
		Me.txtRevisedReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtRevisedReserve.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtRevisedReserve.HideSelection = True
		Me.txtRevisedReserve.Location = New System.Drawing.Point(144, 164)
		Me.txtRevisedReserve.MaxLength = 15
		Me.txtRevisedReserve.Multiline = False
		Me.txtRevisedReserve.Name = "txtRevisedReserve"
		Me.txtRevisedReserve.ReadOnly = False
		Me.txtRevisedReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtRevisedReserve.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtRevisedReserve.Size = New System.Drawing.Size(169, 19)
		Me.txtRevisedReserve.TabIndex = 3
		Me.txtRevisedReserve.TabStop = True
		Me.txtRevisedReserve.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtRevisedReserve.Visible = True
		' 
		' txtInitialReserve
		' 
		Me.txtInitialReserve.AcceptsReturn = True
		Me.txtInitialReserve.AutoSize = False
		Me.txtInitialReserve.BackColor = System.Drawing.SystemColors.Window
		Me.txtInitialReserve.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtInitialReserve.CausesValidation = True
		Me.txtInitialReserve.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtInitialReserve.Enabled = True
		Me.txtInitialReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtInitialReserve.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtInitialReserve.HideSelection = True
		Me.txtInitialReserve.Location = New System.Drawing.Point(144, 52)
		Me.txtInitialReserve.MaxLength = 15
		Me.txtInitialReserve.Multiline = False
		Me.txtInitialReserve.Name = "txtInitialReserve"
		Me.txtInitialReserve.ReadOnly = False
		Me.txtInitialReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtInitialReserve.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtInitialReserve.Size = New System.Drawing.Size(169, 19)
		Me.txtInitialReserve.TabIndex = 2
		Me.txtInitialReserve.TabStop = True
		Me.txtInitialReserve.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtInitialReserve.Visible = True
		' 
		' cboRecoveryType
		' 
		Me.cboRecoveryType.BackColor = System.Drawing.SystemColors.Window
		Me.cboRecoveryType.CausesValidation = True
		Me.cboRecoveryType.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboRecoveryType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboRecoveryType.Enabled = True
		Me.cboRecoveryType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboRecoveryType.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboRecoveryType.IntegralHeight = True
		Me.cboRecoveryType.Location = New System.Drawing.Point(144, 20)
		Me.cboRecoveryType.Name = "cboRecoveryType"
		Me.cboRecoveryType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboRecoveryType.Size = New System.Drawing.Size(169, 21)
		Me.cboRecoveryType.Sorted = False
		Me.cboRecoveryType.TabIndex = 1
		Me.cboRecoveryType.TabStop = True
		Me.cboRecoveryType.Visible = True
		' 
		' cboTaxType
		' 
		Me.cboTaxType.BackColor = System.Drawing.SystemColors.Window
		Me.cboTaxType.CausesValidation = True
		Me.cboTaxType.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboTaxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown
		Me.cboTaxType.Enabled = True
		Me.cboTaxType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboTaxType.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboTaxType.IntegralHeight = True
		Me.cboTaxType.Location = New System.Drawing.Point(144, 292)
		Me.cboTaxType.Name = "cboTaxType"
		Me.cboTaxType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboTaxType.Size = New System.Drawing.Size(169, 21)
		Me.cboTaxType.Sorted = False
		Me.cboTaxType.TabIndex = 11
		Me.cboTaxType.TabStop = True
		Me.cboTaxType.Text = "Combo1"
		Me.cboTaxType.Visible = True
		' 
		' cboTaxBand
		' 
		Me.cboTaxBand.BackColor = System.Drawing.SystemColors.Window
		Me.cboTaxBand.CausesValidation = True
		Me.cboTaxBand.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboTaxBand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown
		Me.cboTaxBand.Enabled = True
		Me.cboTaxBand.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboTaxBand.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboTaxBand.IntegralHeight = True
		Me.cboTaxBand.Location = New System.Drawing.Point(144, 324)
		Me.cboTaxBand.Name = "cboTaxBand"
		Me.cboTaxBand.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboTaxBand.Size = New System.Drawing.Size(169, 21)
		Me.cboTaxBand.Sorted = False
		Me.cboTaxBand.TabIndex = 12
		Me.cboTaxBand.TabStop = True
		Me.cboTaxBand.Text = "Combo1"
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
		Me.txtTaxAmount.Location = New System.Drawing.Point(144, 356)
		Me.txtTaxAmount.MaxLength = 0
		Me.txtTaxAmount.Multiline = False
		Me.txtTaxAmount.Name = "txtTaxAmount"
		Me.txtTaxAmount.ReadOnly = True
		Me.txtTaxAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTaxAmount.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTaxAmount.Size = New System.Drawing.Size(169, 19)
		Me.txtTaxAmount.TabIndex = 13
		Me.txtTaxAmount.TabStop = True
		Me.txtTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTaxAmount.Visible = True
		' 
		' txtNetPayment
		' 
		Me.txtNetPayment.AcceptsReturn = True
		Me.txtNetPayment.AutoSize = False
		Me.txtNetPayment.BackColor = System.Drawing.SystemColors.Window
		Me.txtNetPayment.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtNetPayment.CausesValidation = True
		Me.txtNetPayment.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtNetPayment.Enabled = True
		Me.txtNetPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtNetPayment.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtNetPayment.HideSelection = True
		Me.txtNetPayment.Location = New System.Drawing.Point(144, 388)
		Me.txtNetPayment.MaxLength = 0
		Me.txtNetPayment.Multiline = False
		Me.txtNetPayment.Name = "txtNetPayment"
		Me.txtNetPayment.ReadOnly = True
		Me.txtNetPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtNetPayment.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtNetPayment.Size = New System.Drawing.Size(169, 19)
		Me.txtNetPayment.TabIndex = 14
		Me.txtNetPayment.TabStop = True
		Me.txtNetPayment.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtNetPayment.Visible = True
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
		Me.txtCurrencyRate.Location = New System.Drawing.Point(144, 252)
		Me.txtCurrencyRate.MaxLength = 0
		Me.txtCurrencyRate.Multiline = False
		Me.txtCurrencyRate.Name = "txtCurrencyRate"
		Me.txtCurrencyRate.ReadOnly = False
		Me.txtCurrencyRate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtCurrencyRate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtCurrencyRate.Size = New System.Drawing.Size(169, 19)
		Me.txtCurrencyRate.TabIndex = 20
		Me.txtCurrencyRate.TabStop = True
		Me.txtCurrencyRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtCurrencyRate.Visible = True
		' 
		' txtLossCurrency
		' 
		Me.txtLossCurrency.AcceptsReturn = True
		Me.txtLossCurrency.AutoSize = False
		Me.txtLossCurrency.BackColor = System.Drawing.SystemColors.Window
		Me.txtLossCurrency.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtLossCurrency.CausesValidation = True
		Me.txtLossCurrency.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtLossCurrency.Enabled = False
		Me.txtLossCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtLossCurrency.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtLossCurrency.HideSelection = True
		Me.txtLossCurrency.Location = New System.Drawing.Point(144, 196)
		Me.txtLossCurrency.MaxLength = 0
		Me.txtLossCurrency.Multiline = False
		Me.txtLossCurrency.Name = "txtLossCurrency"
		Me.txtLossCurrency.ReadOnly = False
		Me.txtLossCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtLossCurrency.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtLossCurrency.Size = New System.Drawing.Size(169, 19)
		Me.txtLossCurrency.TabIndex = 21
		Me.txtLossCurrency.TabStop = True
		Me.txtLossCurrency.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtLossCurrency.Visible = True
		' 
		' txtRevisedReserveLoss
		' 
		Me.txtRevisedReserveLoss.AcceptsReturn = True
		Me.txtRevisedReserveLoss.AutoSize = False
		Me.txtRevisedReserveLoss.BackColor = System.Drawing.SystemColors.Window
		Me.txtRevisedReserveLoss.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtRevisedReserveLoss.CausesValidation = True
		Me.txtRevisedReserveLoss.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtRevisedReserveLoss.Enabled = False
		Me.txtRevisedReserveLoss.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtRevisedReserveLoss.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtRevisedReserveLoss.HideSelection = True
		Me.txtRevisedReserveLoss.Location = New System.Drawing.Point(144, 132)
		Me.txtRevisedReserveLoss.MaxLength = 0
		Me.txtRevisedReserveLoss.Multiline = False
		Me.txtRevisedReserveLoss.Name = "txtRevisedReserveLoss"
		Me.txtRevisedReserveLoss.ReadOnly = False
		Me.txtRevisedReserveLoss.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtRevisedReserveLoss.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtRevisedReserveLoss.Size = New System.Drawing.Size(169, 19)
		Me.txtRevisedReserveLoss.TabIndex = 25
		Me.txtRevisedReserveLoss.TabStop = True
		Me.txtRevisedReserveLoss.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtRevisedReserveLoss.Visible = True
		' 
		' frmThirdPartyDetails
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(345, 485)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.tabMainTab)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmThirdPartyDetails"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "ThirdPartyDetails"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMainTab, 1)
		Me.tabMainTab.ResumeLayout(False)
		Me._tabMainTab_TabPage0.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class