<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDetailsUW
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
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents txtRiskType As System.Windows.Forms.TextBox
	Public WithEvents txtThisPaymentLoss As System.Windows.Forms.TextBox
	Public WithEvents cboTaxType As System.Windows.Forms.ComboBox
	Public WithEvents cboTaxBand As System.Windows.Forms.ComboBox
	Public WithEvents txtTaxValue As System.Windows.Forms.TextBox
	Public WithEvents txtTotalInclTax As System.Windows.Forms.TextBox
	Public WithEvents txtThisPayment As System.Windows.Forms.TextBox
	Public WithEvents txtCurrentReserve As System.Windows.Forms.TextBox
	Public WithEvents txtPaidToDate As System.Windows.Forms.TextBox
	Public WithEvents txtLossCurrencyName As System.Windows.Forms.TextBox
	Public WithEvents cboPaymentCurrency As System.Windows.Forms.ComboBox
	Public WithEvents lblThisPaymentLoss As System.Windows.Forms.Label
	Public WithEvents lblLossCurrency As System.Windows.Forms.Label
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	Public WithEvents lblTaxType As System.Windows.Forms.Label
	Public WithEvents lblTaxBand As System.Windows.Forms.Label
	Public WithEvents lblTaxValue As System.Windows.Forms.Label
	Public WithEvents lblTotalInclTax As System.Windows.Forms.Label
	Public WithEvents lblThisPayment As System.Windows.Forms.Label
	Public WithEvents lblPaidToDate As System.Windows.Forms.Label
	Public WithEvents lblRiskType As System.Windows.Forms.Label
	Public WithEvents lblCurrentReserve As System.Windows.Forms.Label
	Public WithEvents fraReserveDetails As System.Windows.Forms.GroupBox
	Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents SSTab1 As System.Windows.Forms.TabControl
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
        Me.txtRiskType = New System.Windows.Forms.TextBox
        Me.txtThisPaymentLoss = New System.Windows.Forms.TextBox
        Me.cboTaxType = New System.Windows.Forms.ComboBox
        Me.cboTaxBand = New System.Windows.Forms.ComboBox
        Me.txtTaxValue = New System.Windows.Forms.TextBox
        Me.txtTotalInclTax = New System.Windows.Forms.TextBox
        Me.txtThisPayment = New System.Windows.Forms.TextBox
        Me.txtCurrentReserve = New System.Windows.Forms.TextBox
        Me.txtPaidToDate = New System.Windows.Forms.TextBox
        Me.txtLossCurrencyName = New System.Windows.Forms.TextBox
        Me.cboPaymentCurrency = New System.Windows.Forms.ComboBox
        Me.lblThisPaymentLoss = New System.Windows.Forms.Label
        Me.lblLossCurrency = New System.Windows.Forms.Label
        Me.lblCurrency = New System.Windows.Forms.Label
        Me.lblTaxType = New System.Windows.Forms.Label
        Me.lblTaxBand = New System.Windows.Forms.Label
        Me.lblTaxValue = New System.Windows.Forms.Label
        Me.lblTotalInclTax = New System.Windows.Forms.Label
        Me.lblThisPayment = New System.Windows.Forms.Label
        Me.lblPaidToDate = New System.Windows.Forms.Label
        Me.lblRiskType = New System.Windows.Forms.Label
        Me.lblCurrentReserve = New System.Windows.Forms.Label
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me.fraReserveDetails.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(256, 448)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 25
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
        Me.cmdOk.Location = New System.Drawing.Point(176, 448)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(73, 22)
        Me.cmdOk.TabIndex = 24
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
        Me.SSTab1.Size = New System.Drawing.Size(325, 439)
        Me.SSTab1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.SSTab1.TabIndex = 22
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.fraReserveDetails)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(317, 413)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "Payment Details"
        '
        'fraReserveDetails
        '
        Me.fraReserveDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraReserveDetails.Controls.Add(Me.txtRiskType)
        Me.fraReserveDetails.Controls.Add(Me.txtThisPaymentLoss)
        Me.fraReserveDetails.Controls.Add(Me.cboTaxType)
        Me.fraReserveDetails.Controls.Add(Me.cboTaxBand)
        Me.fraReserveDetails.Controls.Add(Me.txtTaxValue)
        Me.fraReserveDetails.Controls.Add(Me.txtTotalInclTax)
        Me.fraReserveDetails.Controls.Add(Me.txtThisPayment)
        Me.fraReserveDetails.Controls.Add(Me.txtCurrentReserve)
        Me.fraReserveDetails.Controls.Add(Me.txtPaidToDate)
        Me.fraReserveDetails.Controls.Add(Me.txtLossCurrencyName)
        Me.fraReserveDetails.Controls.Add(Me.cboPaymentCurrency)
        Me.fraReserveDetails.Controls.Add(Me.lblThisPaymentLoss)
        Me.fraReserveDetails.Controls.Add(Me.lblLossCurrency)
        Me.fraReserveDetails.Controls.Add(Me.lblCurrency)
        Me.fraReserveDetails.Controls.Add(Me.lblTaxType)
        Me.fraReserveDetails.Controls.Add(Me.lblTaxBand)
        Me.fraReserveDetails.Controls.Add(Me.lblTaxValue)
        Me.fraReserveDetails.Controls.Add(Me.lblTotalInclTax)
        Me.fraReserveDetails.Controls.Add(Me.lblThisPayment)
        Me.fraReserveDetails.Controls.Add(Me.lblPaidToDate)
        Me.fraReserveDetails.Controls.Add(Me.lblRiskType)
        Me.fraReserveDetails.Controls.Add(Me.lblCurrentReserve)
        Me.fraReserveDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraReserveDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraReserveDetails.Location = New System.Drawing.Point(7, 4)
        Me.fraReserveDetails.Name = "fraReserveDetails"
        Me.fraReserveDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraReserveDetails.Size = New System.Drawing.Size(305, 400)
        Me.fraReserveDetails.TabIndex = 23
        Me.fraReserveDetails.TabStop = False
        '
        'txtRiskType
        '
        Me.txtRiskType.AcceptsReturn = True
        Me.txtRiskType.BackColor = System.Drawing.SystemColors.Window
        Me.txtRiskType.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRiskType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRiskType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRiskType.Location = New System.Drawing.Point(128, 24)
        Me.txtRiskType.MaxLength = 0
        Me.txtRiskType.Name = "txtRiskType"
        Me.txtRiskType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRiskType.Size = New System.Drawing.Size(169, 19)
        Me.txtRiskType.TabIndex = 1
        '
        'txtThisPaymentLoss
        '
        Me.txtThisPaymentLoss.AcceptsReturn = True
        Me.txtThisPaymentLoss.BackColor = System.Drawing.SystemColors.Window
        Me.txtThisPaymentLoss.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtThisPaymentLoss.Enabled = False
        Me.txtThisPaymentLoss.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThisPaymentLoss.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtThisPaymentLoss.Location = New System.Drawing.Point(128, 114)
        Me.txtThisPaymentLoss.MaxLength = 0
        Me.txtThisPaymentLoss.Name = "txtThisPaymentLoss"
        Me.txtThisPaymentLoss.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtThisPaymentLoss.Size = New System.Drawing.Size(169, 19)
        Me.txtThisPaymentLoss.TabIndex = 7
        '
        'cboTaxType
        '
        Me.cboTaxType.BackColor = System.Drawing.SystemColors.Window
        Me.cboTaxType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTaxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTaxType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTaxType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTaxType.Location = New System.Drawing.Point(128, 234)
        Me.cboTaxType.Name = "cboTaxType"
        Me.cboTaxType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTaxType.Size = New System.Drawing.Size(169, 21)
        Me.cboTaxType.TabIndex = 15
        '
        'cboTaxBand
        '
        Me.cboTaxBand.BackColor = System.Drawing.SystemColors.Window
        Me.cboTaxBand.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTaxBand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTaxBand.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTaxBand.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTaxBand.Location = New System.Drawing.Point(128, 266)
        Me.cboTaxBand.Name = "cboTaxBand"
        Me.cboTaxBand.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTaxBand.Size = New System.Drawing.Size(169, 21)
        Me.cboTaxBand.TabIndex = 17
        '
        'txtTaxValue
        '
        Me.txtTaxValue.AcceptsReturn = True
        Me.txtTaxValue.BackColor = System.Drawing.SystemColors.Window
        Me.txtTaxValue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTaxValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTaxValue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTaxValue.Location = New System.Drawing.Point(128, 298)
        Me.txtTaxValue.MaxLength = 0
        Me.txtTaxValue.Name = "txtTaxValue"
        Me.txtTaxValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTaxValue.Size = New System.Drawing.Size(169, 19)
        Me.txtTaxValue.TabIndex = 19
        '
        'txtTotalInclTax
        '
        Me.txtTotalInclTax.AcceptsReturn = True
        Me.txtTotalInclTax.BackColor = System.Drawing.SystemColors.Window
        Me.txtTotalInclTax.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalInclTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalInclTax.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalInclTax.Location = New System.Drawing.Point(128, 330)
        Me.txtTotalInclTax.MaxLength = 0
        Me.txtTotalInclTax.Name = "txtTotalInclTax"
        Me.txtTotalInclTax.ReadOnly = True
        Me.txtTotalInclTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalInclTax.Size = New System.Drawing.Size(169, 19)
        Me.txtTotalInclTax.TabIndex = 21
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
        Me.txtThisPayment.TabIndex = 9
        '
        'txtCurrentReserve
        '
        Me.txtCurrentReserve.AcceptsReturn = True
        Me.txtCurrentReserve.BackColor = System.Drawing.SystemColors.Window
        Me.txtCurrentReserve.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCurrentReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCurrentReserve.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCurrentReserve.Location = New System.Drawing.Point(128, 56)
        Me.txtCurrentReserve.MaxLength = 0
        Me.txtCurrentReserve.Name = "txtCurrentReserve"
        Me.txtCurrentReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCurrentReserve.Size = New System.Drawing.Size(169, 19)
        Me.txtCurrentReserve.TabIndex = 3
        '
        'txtPaidToDate
        '
        Me.txtPaidToDate.AcceptsReturn = True
        Me.txtPaidToDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtPaidToDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPaidToDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaidToDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPaidToDate.Location = New System.Drawing.Point(128, 88)
        Me.txtPaidToDate.MaxLength = 0
        Me.txtPaidToDate.Name = "txtPaidToDate"
        Me.txtPaidToDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPaidToDate.Size = New System.Drawing.Size(169, 19)
        Me.txtPaidToDate.TabIndex = 5
        '
        'txtLossCurrencyName
        '
        Me.txtLossCurrencyName.AcceptsReturn = True
        Me.txtLossCurrencyName.BackColor = System.Drawing.SystemColors.Window
        Me.txtLossCurrencyName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLossCurrencyName.Enabled = False
        Me.txtLossCurrencyName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLossCurrencyName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLossCurrencyName.Location = New System.Drawing.Point(128, 168)
        Me.txtLossCurrencyName.MaxLength = 0
        Me.txtLossCurrencyName.Name = "txtLossCurrencyName"
        Me.txtLossCurrencyName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLossCurrencyName.Size = New System.Drawing.Size(169, 19)
        Me.txtLossCurrencyName.TabIndex = 11
        '
        'cboPaymentCurrency
        '
        Me.cboPaymentCurrency.BackColor = System.Drawing.SystemColors.Window
        Me.cboPaymentCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPaymentCurrency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPaymentCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPaymentCurrency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPaymentCurrency.Location = New System.Drawing.Point(128, 200)
        Me.cboPaymentCurrency.Name = "cboPaymentCurrency"
        Me.cboPaymentCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPaymentCurrency.Size = New System.Drawing.Size(169, 21)
        Me.cboPaymentCurrency.TabIndex = 13
        '
        'lblThisPaymentLoss
        '
        Me.lblThisPaymentLoss.BackColor = System.Drawing.SystemColors.Control
        Me.lblThisPaymentLoss.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblThisPaymentLoss.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblThisPaymentLoss.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblThisPaymentLoss.Location = New System.Drawing.Point(8, 112)
        Me.lblThisPaymentLoss.Name = "lblThisPaymentLoss"
        Me.lblThisPaymentLoss.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblThisPaymentLoss.Size = New System.Drawing.Size(113, 29)
        Me.lblThisPaymentLoss.TabIndex = 6
        Me.lblThisPaymentLoss.Text = "This Payment : (Loss Currency)"
        '
        'lblLossCurrency
        '
        Me.lblLossCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblLossCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLossCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLossCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLossCurrency.Location = New System.Drawing.Point(8, 166)
        Me.lblLossCurrency.Name = "lblLossCurrency"
        Me.lblLossCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLossCurrency.Size = New System.Drawing.Size(117, 13)
        Me.lblLossCurrency.TabIndex = 10
        Me.lblLossCurrency.Text = "Loss Currency :"
        '
        'lblCurrency
        '
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(8, 203)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(117, 13)
        Me.lblCurrency.TabIndex = 12
        Me.lblCurrency.Text = "Payment Currency :"
        '
        'lblTaxType
        '
        Me.lblTaxType.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxType.Location = New System.Drawing.Point(8, 237)
        Me.lblTaxType.Name = "lblTaxType"
        Me.lblTaxType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxType.Size = New System.Drawing.Size(89, 17)
        Me.lblTaxType.TabIndex = 14
        Me.lblTaxType.Text = "Tax Type :"
        '
        'lblTaxBand
        '
        Me.lblTaxBand.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxBand.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxBand.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxBand.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxBand.Location = New System.Drawing.Point(8, 266)
        Me.lblTaxBand.Name = "lblTaxBand"
        Me.lblTaxBand.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxBand.Size = New System.Drawing.Size(89, 17)
        Me.lblTaxBand.TabIndex = 16
        Me.lblTaxBand.Text = "Tax Band :"
        '
        'lblTaxValue
        '
        Me.lblTaxValue.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxValue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxValue.Location = New System.Drawing.Point(8, 298)
        Me.lblTaxValue.Name = "lblTaxValue"
        Me.lblTaxValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxValue.Size = New System.Drawing.Size(89, 17)
        Me.lblTaxValue.TabIndex = 18
        Me.lblTaxValue.Text = "Tax Value :"
        '
        'lblTotalInclTax
        '
        Me.lblTotalInclTax.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalInclTax.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalInclTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalInclTax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalInclTax.Location = New System.Drawing.Point(8, 330)
        Me.lblTotalInclTax.Name = "lblTotalInclTax"
        Me.lblTotalInclTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalInclTax.Size = New System.Drawing.Size(89, 17)
        Me.lblTotalInclTax.TabIndex = 20
        Me.lblTotalInclTax.Text = "Total incl Tax :"
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
        Me.lblThisPayment.TabIndex = 8
        Me.lblThisPayment.Text = "This Payment :"
        '
        'lblPaidToDate
        '
        Me.lblPaidToDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaidToDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaidToDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaidToDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaidToDate.Location = New System.Drawing.Point(8, 82)
        Me.lblPaidToDate.Name = "lblPaidToDate"
        Me.lblPaidToDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaidToDate.Size = New System.Drawing.Size(121, 29)
        Me.lblPaidToDate.TabIndex = 4
        Me.lblPaidToDate.Text = "Paid to Date :     (Loss Currency)"
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
        Me.lblRiskType.TabIndex = 0
        Me.lblRiskType.Text = "Risk Type :"
        '
        'lblCurrentReserve
        '
        Me.lblCurrentReserve.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrentReserve.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrentReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentReserve.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrentReserve.Location = New System.Drawing.Point(8, 50)
        Me.lblCurrentReserve.Name = "lblCurrentReserve"
        Me.lblCurrentReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrentReserve.Size = New System.Drawing.Size(105, 27)
        Me.lblCurrentReserve.TabIndex = 2
        Me.lblCurrentReserve.Text = "Current Reserve : (Loss Currency)"
        '
        'frmDetailsUW
        '
        Me.AcceptButton = Me.cmdOk
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(337, 476)
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
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Payment Details Screen"
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me.fraReserveDetails.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class