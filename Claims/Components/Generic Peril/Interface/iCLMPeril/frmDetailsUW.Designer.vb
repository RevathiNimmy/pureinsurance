<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDetailsUW
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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents txtThisPaymentLoss As System.Windows.Forms.TextBox
	Public WithEvents txtCurrencyRate As System.Windows.Forms.TextBox
	Public WithEvents cboCurrency As System.Windows.Forms.ComboBox
	Public WithEvents txtLossCurrency As System.Windows.Forms.TextBox
	Public WithEvents cboTaxType As System.Windows.Forms.ComboBox
	Public WithEvents cboTaxBand As System.Windows.Forms.ComboBox
	Public WithEvents txtTaxAmount As System.Windows.Forms.TextBox
	Public WithEvents txtNetPayment As System.Windows.Forms.TextBox
	Public WithEvents txtThisRevision As System.Windows.Forms.TextBox
	Public WithEvents txtThisPayment As System.Windows.Forms.TextBox
	Public WithEvents Combo1 As System.Windows.Forms.ComboBox
	Public WithEvents txtInitialReserve As System.Windows.Forms.TextBox
	Public WithEvents txtRevisedReserve As System.Windows.Forms.TextBox
	Public WithEvents lblThisPaymentLoss As System.Windows.Forms.Label
	Public WithEvents lblCurrencyRate As System.Windows.Forms.Label
	Public WithEvents lblLossCurrency As System.Windows.Forms.Label
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	Public WithEvents lblTaxType As System.Windows.Forms.Label
	Public WithEvents lblTaxBand As System.Windows.Forms.Label
	Public WithEvents lblTaxAmount As System.Windows.Forms.Label
	Public WithEvents lblNetPayment As System.Windows.Forms.Label
	Public WithEvents lblThisRevision As System.Windows.Forms.Label
	Public WithEvents lblThisPayment As System.Windows.Forms.Label
	Public WithEvents lblRevisedReserve As System.Windows.Forms.Label
	Public WithEvents lblRiskType As System.Windows.Forms.Label
	Public WithEvents lblInitialReserve As System.Windows.Forms.Label
	Public WithEvents fraReserveDetails As System.Windows.Forms.GroupBox
	Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents SSTab1 As System.Windows.Forms.TabControl
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOk = New System.Windows.Forms.Button
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me.fraReserveDetails = New System.Windows.Forms.GroupBox
        Me.txtThisPaymentLoss = New System.Windows.Forms.TextBox
        Me.txtCurrencyRate = New System.Windows.Forms.TextBox
        Me.cboCurrency = New System.Windows.Forms.ComboBox
        Me.txtLossCurrency = New System.Windows.Forms.TextBox
        Me.cboTaxType = New System.Windows.Forms.ComboBox
        Me.cboTaxBand = New System.Windows.Forms.ComboBox
        Me.txtTaxAmount = New System.Windows.Forms.TextBox
        Me.txtNetPayment = New System.Windows.Forms.TextBox
        Me.txtThisRevision = New System.Windows.Forms.TextBox
        Me.txtThisPayment = New System.Windows.Forms.TextBox
        Me.Combo1 = New System.Windows.Forms.ComboBox
        Me.txtInitialReserve = New System.Windows.Forms.TextBox
        Me.txtRevisedReserve = New System.Windows.Forms.TextBox
        Me.lblThisPaymentLoss = New System.Windows.Forms.Label
        Me.lblCurrencyRate = New System.Windows.Forms.Label
        Me.lblLossCurrency = New System.Windows.Forms.Label
        Me.lblCurrency = New System.Windows.Forms.Label
        Me.lblTaxType = New System.Windows.Forms.Label
        Me.lblTaxBand = New System.Windows.Forms.Label
        Me.lblTaxAmount = New System.Windows.Forms.Label
        Me.lblNetPayment = New System.Windows.Forms.Label
        Me.lblThisRevision = New System.Windows.Forms.Label
        Me.lblThisPayment = New System.Windows.Forms.Label
        Me.lblRevisedReserve = New System.Windows.Forms.Label
        Me.lblRiskType = New System.Windows.Forms.Label
        Me.lblInitialReserve = New System.Windows.Forms.Label
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me.fraReserveDetails.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(256, 440)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 29
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
        Me.cmdOk.Location = New System.Drawing.Point(176, 440)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(73, 22)
        Me.cmdOk.TabIndex = 28
        Me.cmdOk.Text = "&OK"
        Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOk.UseVisualStyleBackColor = False
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(106, 18)
        Me.SSTab1.Location = New System.Drawing.Point(8, 8)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(325, 431)
        Me.SSTab1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.SSTab1.TabIndex = 0
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.fraReserveDetails)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(317, 405)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "Reserve Details"
        '
        'fraReserveDetails
        '
        Me.fraReserveDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraReserveDetails.Controls.Add(Me.txtThisPaymentLoss)
        Me.fraReserveDetails.Controls.Add(Me.txtCurrencyRate)
        Me.fraReserveDetails.Controls.Add(Me.cboCurrency)
        Me.fraReserveDetails.Controls.Add(Me.txtLossCurrency)
        Me.fraReserveDetails.Controls.Add(Me.cboTaxType)
        Me.fraReserveDetails.Controls.Add(Me.cboTaxBand)
        Me.fraReserveDetails.Controls.Add(Me.txtTaxAmount)
        Me.fraReserveDetails.Controls.Add(Me.txtNetPayment)
        Me.fraReserveDetails.Controls.Add(Me.txtThisRevision)
        Me.fraReserveDetails.Controls.Add(Me.txtThisPayment)
        Me.fraReserveDetails.Controls.Add(Me.Combo1)
        Me.fraReserveDetails.Controls.Add(Me.txtInitialReserve)
        Me.fraReserveDetails.Controls.Add(Me.txtRevisedReserve)
        Me.fraReserveDetails.Controls.Add(Me.lblThisPaymentLoss)
        Me.fraReserveDetails.Controls.Add(Me.lblCurrencyRate)
        Me.fraReserveDetails.Controls.Add(Me.lblLossCurrency)
        Me.fraReserveDetails.Controls.Add(Me.lblCurrency)
        Me.fraReserveDetails.Controls.Add(Me.lblTaxType)
        Me.fraReserveDetails.Controls.Add(Me.lblTaxBand)
        Me.fraReserveDetails.Controls.Add(Me.lblTaxAmount)
        Me.fraReserveDetails.Controls.Add(Me.lblNetPayment)
        Me.fraReserveDetails.Controls.Add(Me.lblThisRevision)
        Me.fraReserveDetails.Controls.Add(Me.lblThisPayment)
        Me.fraReserveDetails.Controls.Add(Me.lblRevisedReserve)
        Me.fraReserveDetails.Controls.Add(Me.lblRiskType)
        Me.fraReserveDetails.Controls.Add(Me.lblInitialReserve)
        Me.fraReserveDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraReserveDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraReserveDetails.Location = New System.Drawing.Point(7, 4)
        Me.fraReserveDetails.Name = "fraReserveDetails"
        Me.fraReserveDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraReserveDetails.Size = New System.Drawing.Size(305, 392)
        Me.fraReserveDetails.TabIndex = 1
        Me.fraReserveDetails.TabStop = False
        '
        'txtThisPaymentLoss
        '
        Me.txtThisPaymentLoss.AcceptsReturn = True
        Me.txtThisPaymentLoss.BackColor = System.Drawing.SystemColors.Window
        Me.txtThisPaymentLoss.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtThisPaymentLoss.Enabled = False
        Me.txtThisPaymentLoss.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThisPaymentLoss.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtThisPaymentLoss.Location = New System.Drawing.Point(128, 166)
        Me.txtThisPaymentLoss.MaxLength = 0
        Me.txtThisPaymentLoss.Name = "txtThisPaymentLoss"
        Me.txtThisPaymentLoss.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtThisPaymentLoss.Size = New System.Drawing.Size(169, 19)
        Me.txtThisPaymentLoss.TabIndex = 12
        '
        'txtCurrencyRate
        '
        Me.txtCurrencyRate.AcceptsReturn = True
        Me.txtCurrencyRate.BackColor = System.Drawing.SystemColors.Window
        Me.txtCurrencyRate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCurrencyRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCurrencyRate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCurrencyRate.Location = New System.Drawing.Point(130, 382)
        Me.txtCurrencyRate.MaxLength = 0
        Me.txtCurrencyRate.Name = "txtCurrencyRate"
        Me.txtCurrencyRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCurrencyRate.Size = New System.Drawing.Size(169, 19)
        Me.txtCurrencyRate.TabIndex = 27
        Me.txtCurrencyRate.Visible = False
        '
        'cboCurrency
        '
        Me.cboCurrency.BackColor = System.Drawing.SystemColors.Window
        Me.cboCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCurrency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCurrency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCurrency.Location = New System.Drawing.Point(128, 114)
        Me.cboCurrency.Name = "cboCurrency"
        Me.cboCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCurrency.Size = New System.Drawing.Size(169, 21)
        Me.cboCurrency.TabIndex = 9
        '
        'txtLossCurrency
        '
        Me.txtLossCurrency.AcceptsReturn = True
        Me.txtLossCurrency.BackColor = System.Drawing.SystemColors.Window
        Me.txtLossCurrency.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLossCurrency.Enabled = False
        Me.txtLossCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLossCurrency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLossCurrency.Location = New System.Drawing.Point(128, 228)
        Me.txtLossCurrency.MaxLength = 0
        Me.txtLossCurrency.Name = "txtLossCurrency"
        Me.txtLossCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLossCurrency.Size = New System.Drawing.Size(169, 19)
        Me.txtLossCurrency.TabIndex = 17
        '
        'cboTaxType
        '
        Me.cboTaxType.BackColor = System.Drawing.SystemColors.Window
        Me.cboTaxType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTaxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTaxType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTaxType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTaxType.Location = New System.Drawing.Point(128, 258)
        Me.cboTaxType.Name = "cboTaxType"
        Me.cboTaxType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTaxType.Size = New System.Drawing.Size(169, 21)
        Me.cboTaxType.TabIndex = 19
        '
        'cboTaxBand
        '
        Me.cboTaxBand.BackColor = System.Drawing.SystemColors.Window
        Me.cboTaxBand.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTaxBand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTaxBand.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTaxBand.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTaxBand.Location = New System.Drawing.Point(128, 290)
        Me.cboTaxBand.Name = "cboTaxBand"
        Me.cboTaxBand.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTaxBand.Size = New System.Drawing.Size(169, 21)
        Me.cboTaxBand.TabIndex = 21
        '
        'txtTaxAmount
        '
        Me.txtTaxAmount.AcceptsReturn = True
        Me.txtTaxAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtTaxAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTaxAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTaxAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTaxAmount.Location = New System.Drawing.Point(128, 322)
        Me.txtTaxAmount.MaxLength = 0
        Me.txtTaxAmount.Name = "txtTaxAmount"
        Me.txtTaxAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTaxAmount.Size = New System.Drawing.Size(169, 19)
        Me.txtTaxAmount.TabIndex = 23
        '
        'txtNetPayment
        '
        Me.txtNetPayment.AcceptsReturn = True
        Me.txtNetPayment.BackColor = System.Drawing.SystemColors.Window
        Me.txtNetPayment.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNetPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNetPayment.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNetPayment.Location = New System.Drawing.Point(128, 354)
        Me.txtNetPayment.MaxLength = 0
        Me.txtNetPayment.Name = "txtNetPayment"
        Me.txtNetPayment.ReadOnly = True
        Me.txtNetPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNetPayment.Size = New System.Drawing.Size(169, 19)
        Me.txtNetPayment.TabIndex = 25
        '
        'txtThisRevision
        '
        Me.txtThisRevision.AcceptsReturn = True
        Me.txtThisRevision.BackColor = System.Drawing.SystemColors.Window
        Me.txtThisRevision.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtThisRevision.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThisRevision.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtThisRevision.Location = New System.Drawing.Point(128, 202)
        Me.txtThisRevision.MaxLength = 0
        Me.txtThisRevision.Name = "txtThisRevision"
        Me.txtThisRevision.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtThisRevision.Size = New System.Drawing.Size(169, 19)
        Me.txtThisRevision.TabIndex = 14
        '
        'txtThisPayment
        '
        Me.txtThisPayment.AcceptsReturn = True
        Me.txtThisPayment.BackColor = System.Drawing.SystemColors.Window
        Me.txtThisPayment.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtThisPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThisPayment.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtThisPayment.Location = New System.Drawing.Point(128, 142)
        Me.txtThisPayment.MaxLength = 0
        Me.txtThisPayment.Name = "txtThisPayment"
        Me.txtThisPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtThisPayment.Size = New System.Drawing.Size(169, 19)
        Me.txtThisPayment.TabIndex = 10
        '
        'Combo1
        '
        Me.Combo1.BackColor = System.Drawing.SystemColors.Window
        Me.Combo1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Combo1.Enabled = False
        Me.Combo1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Combo1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Combo1.Location = New System.Drawing.Point(128, 24)
        Me.Combo1.Name = "Combo1"
        Me.Combo1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Combo1.Size = New System.Drawing.Size(169, 21)
        Me.Combo1.TabIndex = 3
        Me.Combo1.Text = "Combo1"
        '
        'txtInitialReserve
        '
        Me.txtInitialReserve.AcceptsReturn = True
        Me.txtInitialReserve.BackColor = System.Drawing.SystemColors.Window
        Me.txtInitialReserve.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInitialReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInitialReserve.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInitialReserve.Location = New System.Drawing.Point(128, 56)
        Me.txtInitialReserve.MaxLength = 0
        Me.txtInitialReserve.Name = "txtInitialReserve"
        Me.txtInitialReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInitialReserve.Size = New System.Drawing.Size(169, 19)
        Me.txtInitialReserve.TabIndex = 4
        '
        'txtRevisedReserve
        '
        Me.txtRevisedReserve.AcceptsReturn = True
        Me.txtRevisedReserve.BackColor = System.Drawing.SystemColors.Window
        Me.txtRevisedReserve.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRevisedReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRevisedReserve.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRevisedReserve.Location = New System.Drawing.Point(128, 80)
        Me.txtRevisedReserve.MaxLength = 0
        Me.txtRevisedReserve.Name = "txtRevisedReserve"
        Me.txtRevisedReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRevisedReserve.Size = New System.Drawing.Size(169, 19)
        Me.txtRevisedReserve.TabIndex = 6
        '
        'lblThisPaymentLoss
        '
        Me.lblThisPaymentLoss.BackColor = System.Drawing.SystemColors.Control
        Me.lblThisPaymentLoss.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblThisPaymentLoss.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblThisPaymentLoss.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblThisPaymentLoss.Location = New System.Drawing.Point(8, 169)
        Me.lblThisPaymentLoss.Name = "lblThisPaymentLoss"
        Me.lblThisPaymentLoss.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblThisPaymentLoss.Size = New System.Drawing.Size(113, 29)
        Me.lblThisPaymentLoss.TabIndex = 13
        Me.lblThisPaymentLoss.Text = "This Payment : (Loss Currency)"
        '
        'lblCurrencyRate
        '
        Me.lblCurrencyRate.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrencyRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrencyRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrencyRate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrencyRate.Location = New System.Drawing.Point(8, 378)
        Me.lblCurrencyRate.Name = "lblCurrencyRate"
        Me.lblCurrencyRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrencyRate.Size = New System.Drawing.Size(113, 25)
        Me.lblCurrencyRate.TabIndex = 26
        Me.lblCurrencyRate.Text = "Payment To Loss Rate :"
        Me.lblCurrencyRate.Visible = False
        '
        'lblLossCurrency
        '
        Me.lblLossCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblLossCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLossCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLossCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLossCurrency.Location = New System.Drawing.Point(8, 230)
        Me.lblLossCurrency.Name = "lblLossCurrency"
        Me.lblLossCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLossCurrency.Size = New System.Drawing.Size(117, 13)
        Me.lblLossCurrency.TabIndex = 16
        Me.lblLossCurrency.Text = "Loss Currency :"
        '
        'lblCurrency
        '
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(8, 118)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(117, 13)
        Me.lblCurrency.TabIndex = 8
        Me.lblCurrency.Text = "Payment Currency :"
        '
        'lblTaxType
        '
        Me.lblTaxType.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxType.Location = New System.Drawing.Point(8, 261)
        Me.lblTaxType.Name = "lblTaxType"
        Me.lblTaxType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxType.Size = New System.Drawing.Size(89, 17)
        Me.lblTaxType.TabIndex = 18
        Me.lblTaxType.Text = "Tax Type :"
        '
        'lblTaxBand
        '
        Me.lblTaxBand.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxBand.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxBand.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxBand.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxBand.Location = New System.Drawing.Point(8, 290)
        Me.lblTaxBand.Name = "lblTaxBand"
        Me.lblTaxBand.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxBand.Size = New System.Drawing.Size(89, 17)
        Me.lblTaxBand.TabIndex = 20
        Me.lblTaxBand.Text = "Tax Band :"
        '
        'lblTaxAmount
        '
        Me.lblTaxAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxAmount.Location = New System.Drawing.Point(8, 322)
        Me.lblTaxAmount.Name = "lblTaxAmount"
        Me.lblTaxAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxAmount.Size = New System.Drawing.Size(89, 17)
        Me.lblTaxAmount.TabIndex = 22
        Me.lblTaxAmount.Text = "Tax Value :"
        '
        'lblNetPayment
        '
        Me.lblNetPayment.BackColor = System.Drawing.SystemColors.Control
        Me.lblNetPayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNetPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNetPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNetPayment.Location = New System.Drawing.Point(8, 354)
        Me.lblNetPayment.Name = "lblNetPayment"
        Me.lblNetPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNetPayment.Size = New System.Drawing.Size(89, 17)
        Me.lblNetPayment.TabIndex = 24
        Me.lblNetPayment.Text = "Total incl Tax :"
        '
        'lblThisRevision
        '
        Me.lblThisRevision.BackColor = System.Drawing.SystemColors.Control
        Me.lblThisRevision.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblThisRevision.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblThisRevision.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblThisRevision.Location = New System.Drawing.Point(8, 204)
        Me.lblThisRevision.Name = "lblThisRevision"
        Me.lblThisRevision.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblThisRevision.Size = New System.Drawing.Size(113, 17)
        Me.lblThisRevision.TabIndex = 15
        Me.lblThisRevision.Text = "This Revision :"
        '
        'lblThisPayment
        '
        Me.lblThisPayment.BackColor = System.Drawing.SystemColors.Control
        Me.lblThisPayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblThisPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblThisPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblThisPayment.Location = New System.Drawing.Point(8, 145)
        Me.lblThisPayment.Name = "lblThisPayment"
        Me.lblThisPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblThisPayment.Size = New System.Drawing.Size(113, 17)
        Me.lblThisPayment.TabIndex = 11
        Me.lblThisPayment.Text = "This Payment :"
        '
        'lblRevisedReserve
        '
        Me.lblRevisedReserve.BackColor = System.Drawing.SystemColors.Control
        Me.lblRevisedReserve.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRevisedReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRevisedReserve.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRevisedReserve.Location = New System.Drawing.Point(8, 82)
        Me.lblRevisedReserve.Name = "lblRevisedReserve"
        Me.lblRevisedReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRevisedReserve.Size = New System.Drawing.Size(121, 29)
        Me.lblRevisedReserve.TabIndex = 7
        Me.lblRevisedReserve.Text = "Revision Amount :"
        '
        'lblRiskType
        '
        Me.lblRiskType.BackColor = System.Drawing.SystemColors.Control
        Me.lblRiskType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRiskType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRiskType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRiskType.Location = New System.Drawing.Point(8, 27)
        Me.lblRiskType.Name = "lblRiskType"
        Me.lblRiskType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRiskType.Size = New System.Drawing.Size(89, 17)
        Me.lblRiskType.TabIndex = 2
        Me.lblRiskType.Text = "Risk Type :"
        '
        'lblInitialReserve
        '
        Me.lblInitialReserve.BackColor = System.Drawing.SystemColors.Control
        Me.lblInitialReserve.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInitialReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInitialReserve.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInitialReserve.Location = New System.Drawing.Point(8, 58)
        Me.lblInitialReserve.Name = "lblInitialReserve"
        Me.lblInitialReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInitialReserve.Size = New System.Drawing.Size(105, 27)
        Me.lblInitialReserve.TabIndex = 5
        Me.lblInitialReserve.Text = "Initial Reserve :"
        '
        'frmDetailsUW
        '
        Me.AcceptButton = Me.cmdOk
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(337, 470)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.SSTab1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDetailsUW"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Reserve Details"
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me.fraReserveDetails.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class