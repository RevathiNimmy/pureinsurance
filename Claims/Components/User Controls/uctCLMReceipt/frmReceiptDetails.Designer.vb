<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReceiptDetails
#Region "Windows Form Designer generated code "
	Friend Sub New()
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
	Public WithEvents txtLCNetReceipt As System.Windows.Forms.TextBox
	Public WithEvents txtNetReceipt As System.Windows.Forms.TextBox
	Public WithEvents lblLCNetReceipt As System.Windows.Forms.Label
	Public WithEvents lbNetReceipt As System.Windows.Forms.Label
	Public WithEvents fraTotal As System.Windows.Forms.GroupBox
	Public WithEvents txtLCScriptedTaxAmount As System.Windows.Forms.TextBox
	Public WithEvents txtScriptedTaxAmount As System.Windows.Forms.TextBox
	Public WithEvents txtLCTaxAmount As System.Windows.Forms.TextBox
	Public WithEvents txtTaxAmount As System.Windows.Forms.TextBox
	Public WithEvents cboTaxGroup As System.Windows.Forms.ComboBox
	Public WithEvents txtTaxGroup As System.Windows.Forms.TextBox
	Public WithEvents lblLCScriptedTaxAmount As System.Windows.Forms.Label
	Public WithEvents lblScriptedTaxAmount As System.Windows.Forms.Label
	Public WithEvents lblTaxAmount As System.Windows.Forms.Label
	Public WithEvents lblLCTaxAnount As System.Windows.Forms.Label
	Public WithEvents lblTaxGroup As System.Windows.Forms.Label
	Public WithEvents fraTaxes As System.Windows.Forms.GroupBox
	Public WithEvents txtLCThisReceiptBalance As System.Windows.Forms.TextBox
	Public WithEvents txtLCThisReceipt As System.Windows.Forms.TextBox
	Public WithEvents txtLossCurrency As System.Windows.Forms.TextBox
	Public WithEvents txtThisReceipt As System.Windows.Forms.TextBox
	Public WithEvents txtCurrencyRate As System.Windows.Forms.TextBox
	Public WithEvents cboCurrency As System.Windows.Forms.ComboBox
	Public WithEvents txtCurrency As System.Windows.Forms.TextBox
	Public WithEvents lblLCThisReceiptBalance As System.Windows.Forms.Label
	Public WithEvents lblCurrency1 As System.Windows.Forms.Label
	Public WithEvents lblPaymentCurrency As System.Windows.Forms.Label
	Public WithEvents lblLCThisReceipt As System.Windows.Forms.Label
	Public WithEvents lblossCurrency As System.Windows.Forms.Label
	Public WithEvents lblThisReceipt As System.Windows.Forms.Label
	Public WithEvents lblCurrencyRate As System.Windows.Forms.Label
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	Public WithEvents fraPayment As System.Windows.Forms.GroupBox
	Public WithEvents txtBalance As System.Windows.Forms.TextBox
	Public WithEvents txtRiskType As System.Windows.Forms.TextBox
	Public WithEvents txtRecoveryType As System.Windows.Forms.TextBox
	Public WithEvents txtRecoveredToDate As System.Windows.Forms.TextBox
	Public WithEvents txtTotalReserve As System.Windows.Forms.TextBox
	Public WithEvents lblBalance As System.Windows.Forms.Label
	Public WithEvents lblReceivedToDate As System.Windows.Forms.Label
	Public WithEvents lblTotalReserve As System.Windows.Forms.Label
	Public WithEvents lblRiskType As System.Windows.Forms.Label
	Public WithEvents lblForRecoveyType As System.Windows.Forms.Label
	Public WithEvents fraReserve As System.Windows.Forms.GroupBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReceiptDetails))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOk = New System.Windows.Forms.Button
		Me.fraTotal = New System.Windows.Forms.GroupBox
		Me.txtLCNetReceipt = New System.Windows.Forms.TextBox
		Me.txtNetReceipt = New System.Windows.Forms.TextBox
		Me.lblLCNetReceipt = New System.Windows.Forms.Label
		Me.lbNetReceipt = New System.Windows.Forms.Label
		Me.fraTaxes = New System.Windows.Forms.GroupBox
		Me.txtLCScriptedTaxAmount = New System.Windows.Forms.TextBox
		Me.txtScriptedTaxAmount = New System.Windows.Forms.TextBox
		Me.txtLCTaxAmount = New System.Windows.Forms.TextBox
		Me.txtTaxAmount = New System.Windows.Forms.TextBox
		Me.cboTaxGroup = New System.Windows.Forms.ComboBox
		Me.txtTaxGroup = New System.Windows.Forms.TextBox
		Me.lblLCScriptedTaxAmount = New System.Windows.Forms.Label
		Me.lblScriptedTaxAmount = New System.Windows.Forms.Label
		Me.lblTaxAmount = New System.Windows.Forms.Label
		Me.lblLCTaxAnount = New System.Windows.Forms.Label
		Me.lblTaxGroup = New System.Windows.Forms.Label
		Me.fraPayment = New System.Windows.Forms.GroupBox
		Me.txtLCThisReceiptBalance = New System.Windows.Forms.TextBox
		Me.txtLCThisReceipt = New System.Windows.Forms.TextBox
		Me.txtLossCurrency = New System.Windows.Forms.TextBox
		Me.txtThisReceipt = New System.Windows.Forms.TextBox
		Me.txtCurrencyRate = New System.Windows.Forms.TextBox
		Me.cboCurrency = New System.Windows.Forms.ComboBox
		Me.txtCurrency = New System.Windows.Forms.TextBox
		Me.lblLCThisReceiptBalance = New System.Windows.Forms.Label
		Me.lblCurrency1 = New System.Windows.Forms.Label
		Me.lblPaymentCurrency = New System.Windows.Forms.Label
		Me.lblLCThisReceipt = New System.Windows.Forms.Label
		Me.lblossCurrency = New System.Windows.Forms.Label
		Me.lblThisReceipt = New System.Windows.Forms.Label
		Me.lblCurrencyRate = New System.Windows.Forms.Label
		Me.lblCurrency = New System.Windows.Forms.Label
		Me.fraReserve = New System.Windows.Forms.GroupBox
		Me.txtBalance = New System.Windows.Forms.TextBox
		Me.txtRiskType = New System.Windows.Forms.TextBox
		Me.txtRecoveryType = New System.Windows.Forms.TextBox
		Me.txtRecoveredToDate = New System.Windows.Forms.TextBox
		Me.txtTotalReserve = New System.Windows.Forms.TextBox
		Me.lblBalance = New System.Windows.Forms.Label
		Me.lblReceivedToDate = New System.Windows.Forms.Label
		Me.lblTotalReserve = New System.Windows.Forms.Label
		Me.lblRiskType = New System.Windows.Forms.Label
		Me.lblForRecoveyType = New System.Windows.Forms.Label
		Me.fraTotal.SuspendLayout()
		Me.fraTaxes.SuspendLayout()
		Me.fraPayment.SuspendLayout()
		Me.fraReserve.SuspendLayout()
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
		Me.cmdCancel.Location = New System.Drawing.Point(464, 392)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 21)
		Me.cmdCancel.TabIndex = 43
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOk
		' 
		Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOk.CausesValidation = True
		Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOk.Enabled = True
		Me.cmdOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOk.Location = New System.Drawing.Point(384, 392)
		Me.cmdOk.Name = "cmdOk"
		Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOk.Size = New System.Drawing.Size(73, 21)
		Me.cmdOk.TabIndex = 42
		Me.cmdOk.TabStop = True
		Me.cmdOk.Text = "&OK"
		Me.cmdOk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' fraTotal
		' 
		Me.fraTotal.BackColor = System.Drawing.SystemColors.Control
		Me.fraTotal.Controls.Add(Me.txtLCNetReceipt)
		Me.fraTotal.Controls.Add(Me.txtNetReceipt)
		Me.fraTotal.Controls.Add(Me.lblLCNetReceipt)
		Me.fraTotal.Controls.Add(Me.lbNetReceipt)
		Me.fraTotal.Enabled = True
		Me.fraTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraTotal.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraTotal.Location = New System.Drawing.Point(8, 336)
		Me.fraTotal.Name = "fraTotal"
		Me.fraTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraTotal.Size = New System.Drawing.Size(529, 49)
		Me.fraTotal.TabIndex = 37
		Me.fraTotal.Text = "Total"
		Me.fraTotal.Visible = True
		' 
		' txtLCNetReceipt
		' 
		Me.txtLCNetReceipt.AcceptsReturn = True
		Me.txtLCNetReceipt.AutoSize = False
		Me.txtLCNetReceipt.BackColor = System.Drawing.SystemColors.Control
		Me.txtLCNetReceipt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtLCNetReceipt.CausesValidation = True
		Me.txtLCNetReceipt.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtLCNetReceipt.Enabled = True
		Me.txtLCNetReceipt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtLCNetReceipt.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtLCNetReceipt.HideSelection = True
		Me.txtLCNetReceipt.Location = New System.Drawing.Point(384, 16)
		Me.txtLCNetReceipt.MaxLength = 0
		Me.txtLCNetReceipt.Multiline = False
		Me.txtLCNetReceipt.Name = "txtLCNetReceipt"
		Me.txtLCNetReceipt.ReadOnly = True
		Me.txtLCNetReceipt.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtLCNetReceipt.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtLCNetReceipt.Size = New System.Drawing.Size(137, 21)
		Me.txtLCNetReceipt.TabIndex = 41
		Me.txtLCNetReceipt.TabStop = False
		Me.txtLCNetReceipt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.txtLCNetReceipt.Visible = True
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
		Me.txtNetReceipt.Location = New System.Drawing.Point(128, 16)
		Me.txtNetReceipt.MaxLength = 0
		Me.txtNetReceipt.Multiline = False
		Me.txtNetReceipt.Name = "txtNetReceipt"
		Me.txtNetReceipt.ReadOnly = True
		Me.txtNetReceipt.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtNetReceipt.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtNetReceipt.Size = New System.Drawing.Size(137, 21)
		Me.txtNetReceipt.TabIndex = 39
		Me.txtNetReceipt.TabStop = False
		Me.txtNetReceipt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.txtNetReceipt.Visible = True
		' 
		' lblLCNetReceipt
		' 
		Me.lblLCNetReceipt.AutoSize = True
		Me.lblLCNetReceipt.BackColor = System.Drawing.SystemColors.Control
		Me.lblLCNetReceipt.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblLCNetReceipt.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblLCNetReceipt.Enabled = True
		Me.lblLCNetReceipt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblLCNetReceipt.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLCNetReceipt.Location = New System.Drawing.Point(306, 20)
		Me.lblLCNetReceipt.Name = "lblLCNetReceipt"
		Me.lblLCNetReceipt.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblLCNetReceipt.Size = New System.Drawing.Size(70, 13)
		Me.lblLCNetReceipt.TabIndex = 40
		Me.lblLCNetReceipt.Text = "Net Receipt:"
		Me.lblLCNetReceipt.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblLCNetReceipt.UseMnemonic = True
		Me.lblLCNetReceipt.Visible = True
		' 
		' lbNetReceipt
		' 
		Me.lbNetReceipt.AutoSize = True
		Me.lbNetReceipt.BackColor = System.Drawing.SystemColors.Control
		Me.lbNetReceipt.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lbNetReceipt.Cursor = System.Windows.Forms.Cursors.Default
		Me.lbNetReceipt.Enabled = True
		Me.lbNetReceipt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lbNetReceipt.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lbNetReceipt.Location = New System.Drawing.Point(50, 20)
		Me.lbNetReceipt.Name = "lbNetReceipt"
		Me.lbNetReceipt.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lbNetReceipt.Size = New System.Drawing.Size(70, 13)
		Me.lbNetReceipt.TabIndex = 38
		Me.lbNetReceipt.Text = "Net Receipt:"
		Me.lbNetReceipt.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lbNetReceipt.UseMnemonic = True
		Me.lbNetReceipt.Visible = True
		' 
		' fraTaxes
		' 
		Me.fraTaxes.BackColor = System.Drawing.SystemColors.Control
		Me.fraTaxes.Controls.Add(Me.txtLCScriptedTaxAmount)
		Me.fraTaxes.Controls.Add(Me.txtScriptedTaxAmount)
		Me.fraTaxes.Controls.Add(Me.txtLCTaxAmount)
		Me.fraTaxes.Controls.Add(Me.txtTaxAmount)
		Me.fraTaxes.Controls.Add(Me.cboTaxGroup)
		Me.fraTaxes.Controls.Add(Me.txtTaxGroup)
		Me.fraTaxes.Controls.Add(Me.lblLCScriptedTaxAmount)
		Me.fraTaxes.Controls.Add(Me.lblScriptedTaxAmount)
		Me.fraTaxes.Controls.Add(Me.lblTaxAmount)
		Me.fraTaxes.Controls.Add(Me.lblLCTaxAnount)
		Me.fraTaxes.Controls.Add(Me.lblTaxGroup)
		Me.fraTaxes.Enabled = True
		Me.fraTaxes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraTaxes.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraTaxes.Location = New System.Drawing.Point(8, 240)
		Me.fraTaxes.Name = "fraTaxes"
		Me.fraTaxes.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraTaxes.Size = New System.Drawing.Size(529, 92)
		Me.fraTaxes.TabIndex = 26
		Me.fraTaxes.Text = "Taxes"
		Me.fraTaxes.Visible = True
		' 
		' txtLCScriptedTaxAmount
		' 
		Me.txtLCScriptedTaxAmount.AcceptsReturn = True
		Me.txtLCScriptedTaxAmount.AutoSize = False
		Me.txtLCScriptedTaxAmount.BackColor = System.Drawing.SystemColors.Control
		Me.txtLCScriptedTaxAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtLCScriptedTaxAmount.CausesValidation = True
		Me.txtLCScriptedTaxAmount.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtLCScriptedTaxAmount.Enabled = True
		Me.txtLCScriptedTaxAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtLCScriptedTaxAmount.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtLCScriptedTaxAmount.HideSelection = True
		Me.txtLCScriptedTaxAmount.Location = New System.Drawing.Point(384, 64)
		Me.txtLCScriptedTaxAmount.MaxLength = 0
		Me.txtLCScriptedTaxAmount.Multiline = False
		Me.txtLCScriptedTaxAmount.Name = "txtLCScriptedTaxAmount"
		Me.txtLCScriptedTaxAmount.ReadOnly = True
		Me.txtLCScriptedTaxAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtLCScriptedTaxAmount.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtLCScriptedTaxAmount.Size = New System.Drawing.Size(137, 21)
		Me.txtLCScriptedTaxAmount.TabIndex = 36
		Me.txtLCScriptedTaxAmount.TabStop = False
		Me.txtLCScriptedTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.txtLCScriptedTaxAmount.Visible = True
		' 
		' txtScriptedTaxAmount
		' 
		Me.txtScriptedTaxAmount.AcceptsReturn = True
		Me.txtScriptedTaxAmount.AutoSize = False
		Me.txtScriptedTaxAmount.BackColor = System.Drawing.SystemColors.Control
		Me.txtScriptedTaxAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtScriptedTaxAmount.CausesValidation = True
		Me.txtScriptedTaxAmount.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtScriptedTaxAmount.Enabled = True
		Me.txtScriptedTaxAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtScriptedTaxAmount.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtScriptedTaxAmount.HideSelection = True
		Me.txtScriptedTaxAmount.Location = New System.Drawing.Point(128, 64)
		Me.txtScriptedTaxAmount.MaxLength = 0
		Me.txtScriptedTaxAmount.Multiline = False
		Me.txtScriptedTaxAmount.Name = "txtScriptedTaxAmount"
		Me.txtScriptedTaxAmount.ReadOnly = True
		Me.txtScriptedTaxAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtScriptedTaxAmount.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtScriptedTaxAmount.Size = New System.Drawing.Size(137, 21)
		Me.txtScriptedTaxAmount.TabIndex = 34
		Me.txtScriptedTaxAmount.TabStop = False
		Me.txtScriptedTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.txtScriptedTaxAmount.Visible = True
		' 
		' txtLCTaxAmount
		' 
		Me.txtLCTaxAmount.AcceptsReturn = True
		Me.txtLCTaxAmount.AutoSize = False
		Me.txtLCTaxAmount.BackColor = System.Drawing.SystemColors.Control
		Me.txtLCTaxAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtLCTaxAmount.CausesValidation = True
		Me.txtLCTaxAmount.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtLCTaxAmount.Enabled = True
		Me.txtLCTaxAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtLCTaxAmount.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtLCTaxAmount.HideSelection = True
		Me.txtLCTaxAmount.Location = New System.Drawing.Point(384, 40)
		Me.txtLCTaxAmount.MaxLength = 0
		Me.txtLCTaxAmount.Multiline = False
		Me.txtLCTaxAmount.Name = "txtLCTaxAmount"
		Me.txtLCTaxAmount.ReadOnly = True
		Me.txtLCTaxAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtLCTaxAmount.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtLCTaxAmount.Size = New System.Drawing.Size(137, 21)
		Me.txtLCTaxAmount.TabIndex = 32
		Me.txtLCTaxAmount.TabStop = False
		Me.txtLCTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.txtLCTaxAmount.Visible = True
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
		Me.txtTaxAmount.Location = New System.Drawing.Point(128, 40)
		Me.txtTaxAmount.MaxLength = 0
		Me.txtTaxAmount.Multiline = False
		Me.txtTaxAmount.Name = "txtTaxAmount"
		Me.txtTaxAmount.ReadOnly = False
		Me.txtTaxAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTaxAmount.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTaxAmount.Size = New System.Drawing.Size(137, 21)
		Me.txtTaxAmount.TabIndex = 30
		Me.txtTaxAmount.TabStop = True
		Me.txtTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.txtTaxAmount.Visible = True
		' 
		' cboTaxGroup
		' 
		Me.cboTaxGroup.BackColor = System.Drawing.SystemColors.Window
		Me.cboTaxGroup.CausesValidation = True
		Me.cboTaxGroup.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboTaxGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboTaxGroup.Enabled = True
		Me.cboTaxGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboTaxGroup.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboTaxGroup.IntegralHeight = True
		Me.cboTaxGroup.Location = New System.Drawing.Point(128, 16)
		Me.cboTaxGroup.Name = "cboTaxGroup"
		Me.cboTaxGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboTaxGroup.Size = New System.Drawing.Size(161, 21)
		Me.cboTaxGroup.Sorted = False
		Me.cboTaxGroup.TabIndex = 28
		Me.cboTaxGroup.TabStop = True
		Me.cboTaxGroup.Visible = True
		' 
		' txtTaxGroup
		' 
		Me.txtTaxGroup.AcceptsReturn = True
		Me.txtTaxGroup.AutoSize = False
		Me.txtTaxGroup.BackColor = System.Drawing.SystemColors.Control
		Me.txtTaxGroup.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtTaxGroup.CausesValidation = True
		Me.txtTaxGroup.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtTaxGroup.Enabled = True
		Me.txtTaxGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtTaxGroup.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtTaxGroup.HideSelection = True
		Me.txtTaxGroup.Location = New System.Drawing.Point(128, 16)
		Me.txtTaxGroup.MaxLength = 0
		Me.txtTaxGroup.Multiline = False
		Me.txtTaxGroup.Name = "txtTaxGroup"
		Me.txtTaxGroup.ReadOnly = True
		Me.txtTaxGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTaxGroup.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTaxGroup.Size = New System.Drawing.Size(161, 21)
		Me.txtTaxGroup.TabIndex = 45
		Me.txtTaxGroup.TabStop = False
		Me.txtTaxGroup.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTaxGroup.Visible = True
		' 
		' lblLCScriptedTaxAmount
		' 
		Me.lblLCScriptedTaxAmount.AutoSize = True
		Me.lblLCScriptedTaxAmount.BackColor = System.Drawing.SystemColors.Control
		Me.lblLCScriptedTaxAmount.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblLCScriptedTaxAmount.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblLCScriptedTaxAmount.Enabled = True
		Me.lblLCScriptedTaxAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblLCScriptedTaxAmount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLCScriptedTaxAmount.Location = New System.Drawing.Point(299, 68)
		Me.lblLCScriptedTaxAmount.Name = "lblLCScriptedTaxAmount"
		Me.lblLCScriptedTaxAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblLCScriptedTaxAmount.Size = New System.Drawing.Size(77, 13)
		Me.lblLCScriptedTaxAmount.TabIndex = 35
		Me.lblLCScriptedTaxAmount.Text = "Scripted Tax:"
		Me.lblLCScriptedTaxAmount.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblLCScriptedTaxAmount.UseMnemonic = True
		Me.lblLCScriptedTaxAmount.Visible = True
		' 
		' lblScriptedTaxAmount
		' 
		Me.lblScriptedTaxAmount.AutoSize = True
		Me.lblScriptedTaxAmount.BackColor = System.Drawing.SystemColors.Control
		Me.lblScriptedTaxAmount.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblScriptedTaxAmount.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblScriptedTaxAmount.Enabled = True
		Me.lblScriptedTaxAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblScriptedTaxAmount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblScriptedTaxAmount.Location = New System.Drawing.Point(43, 68)
		Me.lblScriptedTaxAmount.Name = "lblScriptedTaxAmount"
		Me.lblScriptedTaxAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblScriptedTaxAmount.Size = New System.Drawing.Size(77, 13)
		Me.lblScriptedTaxAmount.TabIndex = 33
		Me.lblScriptedTaxAmount.Text = "Scripted Tax:"
		Me.lblScriptedTaxAmount.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblScriptedTaxAmount.UseMnemonic = True
		Me.lblScriptedTaxAmount.Visible = True
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
		Me.lblTaxAmount.Location = New System.Drawing.Point(46, 44)
		Me.lblTaxAmount.Name = "lblTaxAmount"
		Me.lblTaxAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTaxAmount.Size = New System.Drawing.Size(74, 13)
		Me.lblTaxAmount.TabIndex = 29
		Me.lblTaxAmount.Text = "Tax Amount:"
		Me.lblTaxAmount.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTaxAmount.UseMnemonic = True
		Me.lblTaxAmount.Visible = True
		' 
		' lblLCTaxAnount
		' 
		Me.lblLCTaxAnount.AutoSize = True
		Me.lblLCTaxAnount.BackColor = System.Drawing.SystemColors.Control
		Me.lblLCTaxAnount.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblLCTaxAnount.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblLCTaxAnount.Enabled = True
		Me.lblLCTaxAnount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblLCTaxAnount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLCTaxAnount.Location = New System.Drawing.Point(302, 44)
		Me.lblLCTaxAnount.Name = "lblLCTaxAnount"
		Me.lblLCTaxAnount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblLCTaxAnount.Size = New System.Drawing.Size(74, 13)
		Me.lblLCTaxAnount.TabIndex = 31
		Me.lblLCTaxAnount.Text = "Tax Amount:"
		Me.lblLCTaxAnount.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblLCTaxAnount.UseMnemonic = True
		Me.lblLCTaxAnount.Visible = True
		' 
		' lblTaxGroup
		' 
		Me.lblTaxGroup.AutoSize = True
		Me.lblTaxGroup.BackColor = System.Drawing.SystemColors.Control
		Me.lblTaxGroup.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTaxGroup.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTaxGroup.Enabled = True
		Me.lblTaxGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTaxGroup.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTaxGroup.Location = New System.Drawing.Point(55, 20)
		Me.lblTaxGroup.Name = "lblTaxGroup"
		Me.lblTaxGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTaxGroup.Size = New System.Drawing.Size(65, 13)
		Me.lblTaxGroup.TabIndex = 27
		Me.lblTaxGroup.Text = "Tax Group:"
		Me.lblTaxGroup.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTaxGroup.UseMnemonic = True
		Me.lblTaxGroup.Visible = True
		' 
		' fraPayment
		' 
		Me.fraPayment.BackColor = System.Drawing.SystemColors.Control
		Me.fraPayment.Controls.Add(Me.txtLCThisReceiptBalance)
		Me.fraPayment.Controls.Add(Me.txtLCThisReceipt)
		Me.fraPayment.Controls.Add(Me.txtLossCurrency)
		Me.fraPayment.Controls.Add(Me.txtThisReceipt)
		Me.fraPayment.Controls.Add(Me.txtCurrencyRate)
		Me.fraPayment.Controls.Add(Me.cboCurrency)
		Me.fraPayment.Controls.Add(Me.txtCurrency)
		Me.fraPayment.Controls.Add(Me.lblLCThisReceiptBalance)
		Me.fraPayment.Controls.Add(Me.lblCurrency1)
		Me.fraPayment.Controls.Add(Me.lblPaymentCurrency)
		Me.fraPayment.Controls.Add(Me.lblLCThisReceipt)
		Me.fraPayment.Controls.Add(Me.lblossCurrency)
		Me.fraPayment.Controls.Add(Me.lblThisReceipt)
		Me.fraPayment.Controls.Add(Me.lblCurrencyRate)
		Me.fraPayment.Controls.Add(Me.lblCurrency)
		Me.fraPayment.Enabled = True
		Me.fraPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraPayment.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraPayment.Location = New System.Drawing.Point(8, 104)
		Me.fraPayment.Name = "fraPayment"
		Me.fraPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraPayment.Size = New System.Drawing.Size(529, 132)
		Me.fraPayment.TabIndex = 11
		Me.fraPayment.Text = "Receipt"
		Me.fraPayment.Visible = True
		' 
		' txtLCThisReceiptBalance
		' 
		Me.txtLCThisReceiptBalance.AcceptsReturn = True
		Me.txtLCThisReceiptBalance.AutoSize = False
		Me.txtLCThisReceiptBalance.BackColor = System.Drawing.SystemColors.Control
		Me.txtLCThisReceiptBalance.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtLCThisReceiptBalance.CausesValidation = True
		Me.txtLCThisReceiptBalance.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtLCThisReceiptBalance.Enabled = True
		Me.txtLCThisReceiptBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtLCThisReceiptBalance.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtLCThisReceiptBalance.HideSelection = True
		Me.txtLCThisReceiptBalance.Location = New System.Drawing.Point(384, 104)
		Me.txtLCThisReceiptBalance.MaxLength = 0
		Me.txtLCThisReceiptBalance.Multiline = False
		Me.txtLCThisReceiptBalance.Name = "txtLCThisReceiptBalance"
		Me.txtLCThisReceiptBalance.ReadOnly = True
		Me.txtLCThisReceiptBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtLCThisReceiptBalance.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtLCThisReceiptBalance.Size = New System.Drawing.Size(137, 21)
		Me.txtLCThisReceiptBalance.TabIndex = 25
		Me.txtLCThisReceiptBalance.TabStop = False
		Me.txtLCThisReceiptBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.txtLCThisReceiptBalance.Visible = True
		' 
		' txtLCThisReceipt
		' 
		Me.txtLCThisReceipt.AcceptsReturn = True
		Me.txtLCThisReceipt.AutoSize = False
		Me.txtLCThisReceipt.BackColor = System.Drawing.SystemColors.Control
		Me.txtLCThisReceipt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtLCThisReceipt.CausesValidation = True
		Me.txtLCThisReceipt.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtLCThisReceipt.Enabled = True
		Me.txtLCThisReceipt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtLCThisReceipt.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtLCThisReceipt.HideSelection = True
		Me.txtLCThisReceipt.Location = New System.Drawing.Point(384, 80)
		Me.txtLCThisReceipt.MaxLength = 0
		Me.txtLCThisReceipt.Multiline = False
		Me.txtLCThisReceipt.Name = "txtLCThisReceipt"
		Me.txtLCThisReceipt.ReadOnly = True
		Me.txtLCThisReceipt.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtLCThisReceipt.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtLCThisReceipt.Size = New System.Drawing.Size(137, 21)
		Me.txtLCThisReceipt.TabIndex = 23
		Me.txtLCThisReceipt.TabStop = False
		Me.txtLCThisReceipt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.txtLCThisReceipt.Visible = True
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
		Me.txtLossCurrency.Location = New System.Drawing.Point(384, 32)
		Me.txtLossCurrency.MaxLength = 0
		Me.txtLossCurrency.Multiline = False
		Me.txtLossCurrency.Name = "txtLossCurrency"
		Me.txtLossCurrency.ReadOnly = True
		Me.txtLossCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtLossCurrency.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtLossCurrency.Size = New System.Drawing.Size(137, 21)
		Me.txtLossCurrency.TabIndex = 17
		Me.txtLossCurrency.TabStop = False
		Me.txtLossCurrency.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtLossCurrency.Visible = True
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
		Me.txtThisReceipt.Location = New System.Drawing.Point(128, 80)
		Me.txtThisReceipt.MaxLength = 0
		Me.txtThisReceipt.Multiline = False
		Me.txtThisReceipt.Name = "txtThisReceipt"
		Me.txtThisReceipt.ReadOnly = False
		Me.txtThisReceipt.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtThisReceipt.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtThisReceipt.Size = New System.Drawing.Size(137, 21)
		Me.txtThisReceipt.TabIndex = 21
		Me.txtThisReceipt.TabStop = True
		Me.txtThisReceipt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.txtThisReceipt.Visible = True
		' 
		' txtCurrencyRate
		' 
		Me.txtCurrencyRate.AcceptsReturn = True
		Me.txtCurrencyRate.AutoSize = False
		Me.txtCurrencyRate.BackColor = System.Drawing.SystemColors.Control
		Me.txtCurrencyRate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtCurrencyRate.CausesValidation = True
		Me.txtCurrencyRate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtCurrencyRate.Enabled = True
		Me.txtCurrencyRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtCurrencyRate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtCurrencyRate.HideSelection = True
		Me.txtCurrencyRate.Location = New System.Drawing.Point(128, 56)
		Me.txtCurrencyRate.MaxLength = 0
		Me.txtCurrencyRate.Multiline = False
		Me.txtCurrencyRate.Name = "txtCurrencyRate"
		Me.txtCurrencyRate.ReadOnly = True
		Me.txtCurrencyRate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtCurrencyRate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtCurrencyRate.Size = New System.Drawing.Size(137, 21)
		Me.txtCurrencyRate.TabIndex = 19
		Me.txtCurrencyRate.TabStop = False
		Me.txtCurrencyRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.txtCurrencyRate.Visible = True
		' 
		' cboCurrency
		' 
		Me.cboCurrency.BackColor = System.Drawing.SystemColors.Window
		Me.cboCurrency.CausesValidation = True
		Me.cboCurrency.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboCurrency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboCurrency.Enabled = True
		Me.cboCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboCurrency.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboCurrency.IntegralHeight = True
		Me.cboCurrency.Location = New System.Drawing.Point(128, 32)
		Me.cboCurrency.Name = "cboCurrency"
		Me.cboCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboCurrency.Size = New System.Drawing.Size(161, 21)
		Me.cboCurrency.Sorted = False
		Me.cboCurrency.TabIndex = 15
		Me.cboCurrency.TabStop = True
		Me.cboCurrency.Visible = True
		' 
		' txtCurrency
		' 
		Me.txtCurrency.AcceptsReturn = True
		Me.txtCurrency.AutoSize = False
		Me.txtCurrency.BackColor = System.Drawing.SystemColors.Control
		Me.txtCurrency.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtCurrency.CausesValidation = True
		Me.txtCurrency.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtCurrency.Enabled = True
		Me.txtCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtCurrency.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtCurrency.HideSelection = True
		Me.txtCurrency.Location = New System.Drawing.Point(128, 32)
		Me.txtCurrency.MaxLength = 0
		Me.txtCurrency.Multiline = False
		Me.txtCurrency.Name = "txtCurrency"
		Me.txtCurrency.ReadOnly = True
		Me.txtCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtCurrency.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtCurrency.Size = New System.Drawing.Size(161, 21)
		Me.txtCurrency.TabIndex = 44
		Me.txtCurrency.TabStop = False
		Me.txtCurrency.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtCurrency.Visible = True
		' 
		' lblLCThisReceiptBalance
		' 
		Me.lblLCThisReceiptBalance.AutoSize = True
		Me.lblLCThisReceiptBalance.BackColor = System.Drawing.SystemColors.Control
		Me.lblLCThisReceiptBalance.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblLCThisReceiptBalance.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblLCThisReceiptBalance.Enabled = True
		Me.lblLCThisReceiptBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblLCThisReceiptBalance.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLCThisReceiptBalance.Location = New System.Drawing.Point(326, 108)
		Me.lblLCThisReceiptBalance.Name = "lblLCThisReceiptBalance"
		Me.lblLCThisReceiptBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblLCThisReceiptBalance.Size = New System.Drawing.Size(50, 13)
		Me.lblLCThisReceiptBalance.TabIndex = 24
		Me.lblLCThisReceiptBalance.Text = "Balance:"
		Me.lblLCThisReceiptBalance.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblLCThisReceiptBalance.UseMnemonic = True
		Me.lblLCThisReceiptBalance.Visible = True
		' 
		' lblCurrency1
		' 
		Me.lblCurrency1.AutoSize = True
		Me.lblCurrency1.BackColor = System.Drawing.SystemColors.Control
		Me.lblCurrency1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCurrency1.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCurrency1.Enabled = True
		Me.lblCurrency1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCurrency1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCurrency1.Location = New System.Drawing.Point(318, 36)
		Me.lblCurrency1.Name = "lblCurrency1"
		Me.lblCurrency1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCurrency1.Size = New System.Drawing.Size(58, 13)
		Me.lblCurrency1.TabIndex = 16
		Me.lblCurrency1.Text = "Currency:"
		Me.lblCurrency1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCurrency1.UseMnemonic = True
		Me.lblCurrency1.Visible = True
		' 
		' lblPaymentCurrency
		' 
		Me.lblPaymentCurrency.AutoSize = True
		Me.lblPaymentCurrency.BackColor = System.Drawing.SystemColors.Control
		Me.lblPaymentCurrency.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPaymentCurrency.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPaymentCurrency.Enabled = True
		Me.lblPaymentCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPaymentCurrency.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPaymentCurrency.Location = New System.Drawing.Point(128, 12)
		Me.lblPaymentCurrency.Name = "lblPaymentCurrency"
		Me.lblPaymentCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPaymentCurrency.Size = New System.Drawing.Size(99, 13)
		Me.lblPaymentCurrency.TabIndex = 12
		Me.lblPaymentCurrency.Text = "Receipt Currency"
		Me.lblPaymentCurrency.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPaymentCurrency.UseMnemonic = True
		Me.lblPaymentCurrency.Visible = True
		' 
		' lblLCThisReceipt
		' 
		Me.lblLCThisReceipt.AutoSize = True
		Me.lblLCThisReceipt.BackColor = System.Drawing.SystemColors.Control
		Me.lblLCThisReceipt.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblLCThisReceipt.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblLCThisReceipt.Enabled = True
		Me.lblLCThisReceipt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblLCThisReceipt.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLCThisReceipt.Location = New System.Drawing.Point(302, 84)
		Me.lblLCThisReceipt.Name = "lblLCThisReceipt"
		Me.lblLCThisReceipt.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblLCThisReceipt.Size = New System.Drawing.Size(74, 13)
		Me.lblLCThisReceipt.TabIndex = 22
		Me.lblLCThisReceipt.Text = "This Receipt:"
		Me.lblLCThisReceipt.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblLCThisReceipt.UseMnemonic = True
		Me.lblLCThisReceipt.Visible = True
		' 
		' lblossCurrency
		' 
		Me.lblossCurrency.AutoSize = True
		Me.lblossCurrency.BackColor = System.Drawing.SystemColors.Control
		Me.lblossCurrency.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblossCurrency.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblossCurrency.Enabled = True
		Me.lblossCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblossCurrency.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblossCurrency.Location = New System.Drawing.Point(384, 12)
		Me.lblossCurrency.Name = "lblossCurrency"
		Me.lblossCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblossCurrency.Size = New System.Drawing.Size(82, 13)
		Me.lblossCurrency.TabIndex = 13
		Me.lblossCurrency.Text = "Loss Currency"
		Me.lblossCurrency.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblossCurrency.UseMnemonic = True
		Me.lblossCurrency.Visible = True
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
		Me.lblThisReceipt.Location = New System.Drawing.Point(46, 84)
		Me.lblThisReceipt.Name = "lblThisReceipt"
		Me.lblThisReceipt.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblThisReceipt.Size = New System.Drawing.Size(74, 13)
		Me.lblThisReceipt.TabIndex = 20
		Me.lblThisReceipt.Text = "This Receipt:"
		Me.lblThisReceipt.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblThisReceipt.UseMnemonic = True
		Me.lblThisReceipt.Visible = True
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
		Me.lblCurrencyRate.Location = New System.Drawing.Point(32, 60)
		Me.lblCurrencyRate.Name = "lblCurrencyRate"
		Me.lblCurrencyRate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCurrencyRate.Size = New System.Drawing.Size(88, 13)
		Me.lblCurrencyRate.TabIndex = 18
		Me.lblCurrencyRate.Text = "Currency Rate:"
		Me.lblCurrencyRate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCurrencyRate.UseMnemonic = True
		Me.lblCurrencyRate.Visible = True
		' 
		' lblCurrency
		' 
		Me.lblCurrency.AutoSize = True
		Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
		Me.lblCurrency.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCurrency.Enabled = True
		Me.lblCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCurrency.Location = New System.Drawing.Point(62, 36)
		Me.lblCurrency.Name = "lblCurrency"
		Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCurrency.Size = New System.Drawing.Size(58, 13)
		Me.lblCurrency.TabIndex = 14
		Me.lblCurrency.Text = "Currency:"
		Me.lblCurrency.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCurrency.UseMnemonic = True
		Me.lblCurrency.Visible = True
		' 
		' fraReserve
		' 
		Me.fraReserve.BackColor = System.Drawing.SystemColors.Control
		Me.fraReserve.Controls.Add(Me.txtBalance)
		Me.fraReserve.Controls.Add(Me.txtRiskType)
		Me.fraReserve.Controls.Add(Me.txtRecoveryType)
		Me.fraReserve.Controls.Add(Me.txtRecoveredToDate)
		Me.fraReserve.Controls.Add(Me.txtTotalReserve)
		Me.fraReserve.Controls.Add(Me.lblBalance)
		Me.fraReserve.Controls.Add(Me.lblReceivedToDate)
		Me.fraReserve.Controls.Add(Me.lblTotalReserve)
		Me.fraReserve.Controls.Add(Me.lblRiskType)
		Me.fraReserve.Controls.Add(Me.lblForRecoveyType)
		Me.fraReserve.Enabled = True
		Me.fraReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraReserve.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraReserve.Location = New System.Drawing.Point(8, 8)
		Me.fraReserve.Name = "fraReserve"
		Me.fraReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraReserve.Size = New System.Drawing.Size(529, 92)
		Me.fraReserve.TabIndex = 0
		Me.fraReserve.Text = "Reserve"
		Me.fraReserve.Visible = True
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
		Me.txtBalance.Location = New System.Drawing.Point(384, 64)
		Me.txtBalance.MaxLength = 0
		Me.txtBalance.Multiline = False
		Me.txtBalance.Name = "txtBalance"
		Me.txtBalance.ReadOnly = True
		Me.txtBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtBalance.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtBalance.Size = New System.Drawing.Size(137, 21)
		Me.txtBalance.TabIndex = 10
		Me.txtBalance.TabStop = False
		Me.txtBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.txtBalance.Visible = True
		' 
		' txtRiskType
		' 
		Me.txtRiskType.AcceptsReturn = True
		Me.txtRiskType.AutoSize = False
		Me.txtRiskType.BackColor = System.Drawing.SystemColors.Control
		Me.txtRiskType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtRiskType.CausesValidation = True
		Me.txtRiskType.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtRiskType.Enabled = True
		Me.txtRiskType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtRiskType.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtRiskType.HideSelection = True
		Me.txtRiskType.Location = New System.Drawing.Point(128, 40)
		Me.txtRiskType.MaxLength = 0
		Me.txtRiskType.Multiline = False
		Me.txtRiskType.Name = "txtRiskType"
		Me.txtRiskType.ReadOnly = True
		Me.txtRiskType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtRiskType.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtRiskType.Size = New System.Drawing.Size(137, 21)
		Me.txtRiskType.TabIndex = 6
		Me.txtRiskType.TabStop = False
		Me.txtRiskType.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtRiskType.Visible = True
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
		Me.txtRecoveryType.Location = New System.Drawing.Point(128, 16)
		Me.txtRecoveryType.MaxLength = 0
		Me.txtRecoveryType.Multiline = False
		Me.txtRecoveryType.Name = "txtRecoveryType"
		Me.txtRecoveryType.ReadOnly = True
		Me.txtRecoveryType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtRecoveryType.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtRecoveryType.Size = New System.Drawing.Size(137, 21)
		Me.txtRecoveryType.TabIndex = 2
		Me.txtRecoveryType.TabStop = False
		Me.txtRecoveryType.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtRecoveryType.Visible = True
		' 
		' txtRecoveredToDate
		' 
		Me.txtRecoveredToDate.AcceptsReturn = True
		Me.txtRecoveredToDate.AutoSize = False
		Me.txtRecoveredToDate.BackColor = System.Drawing.SystemColors.Control
		Me.txtRecoveredToDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtRecoveredToDate.CausesValidation = True
		Me.txtRecoveredToDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtRecoveredToDate.Enabled = True
		Me.txtRecoveredToDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtRecoveredToDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtRecoveredToDate.HideSelection = True
		Me.txtRecoveredToDate.Location = New System.Drawing.Point(384, 40)
		Me.txtRecoveredToDate.MaxLength = 0
		Me.txtRecoveredToDate.Multiline = False
		Me.txtRecoveredToDate.Name = "txtRecoveredToDate"
		Me.txtRecoveredToDate.ReadOnly = True
		Me.txtRecoveredToDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtRecoveredToDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtRecoveredToDate.Size = New System.Drawing.Size(137, 21)
		Me.txtRecoveredToDate.TabIndex = 8
		Me.txtRecoveredToDate.TabStop = False
		Me.txtRecoveredToDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.txtRecoveredToDate.Visible = True
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
		Me.txtTotalReserve.Location = New System.Drawing.Point(384, 16)
		Me.txtTotalReserve.MaxLength = 0
		Me.txtTotalReserve.Multiline = False
		Me.txtTotalReserve.Name = "txtTotalReserve"
		Me.txtTotalReserve.ReadOnly = True
		Me.txtTotalReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTotalReserve.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTotalReserve.Size = New System.Drawing.Size(137, 21)
		Me.txtTotalReserve.TabIndex = 4
		Me.txtTotalReserve.TabStop = False
		Me.txtTotalReserve.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.txtTotalReserve.Visible = True
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
		Me.lblBalance.Location = New System.Drawing.Point(326, 68)
		Me.lblBalance.Name = "lblBalance"
		Me.lblBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblBalance.Size = New System.Drawing.Size(50, 13)
		Me.lblBalance.TabIndex = 9
		Me.lblBalance.Text = "Balance:"
		Me.lblBalance.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblBalance.UseMnemonic = True
		Me.lblBalance.Visible = True
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
		Me.lblReceivedToDate.Location = New System.Drawing.Point(270, 44)
		Me.lblReceivedToDate.Name = "lblReceivedToDate"
		Me.lblReceivedToDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblReceivedToDate.Size = New System.Drawing.Size(106, 13)
		Me.lblReceivedToDate.TabIndex = 7
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
		Me.lblTotalReserve.Location = New System.Drawing.Point(292, 20)
		Me.lblTotalReserve.Name = "lblTotalReserve"
		Me.lblTotalReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTotalReserve.Size = New System.Drawing.Size(84, 13)
		Me.lblTotalReserve.TabIndex = 3
		Me.lblTotalReserve.Text = "Total Reserve:"
		Me.lblTotalReserve.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTotalReserve.UseMnemonic = True
		Me.lblTotalReserve.Visible = True
		' 
		' lblRiskType
		' 
		Me.lblRiskType.AutoSize = True
		Me.lblRiskType.BackColor = System.Drawing.SystemColors.Control
		Me.lblRiskType.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblRiskType.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblRiskType.Enabled = True
		Me.lblRiskType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblRiskType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblRiskType.Location = New System.Drawing.Point(59, 44)
		Me.lblRiskType.Name = "lblRiskType"
		Me.lblRiskType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblRiskType.Size = New System.Drawing.Size(61, 13)
		Me.lblRiskType.TabIndex = 5
		Me.lblRiskType.Text = "Risk Type:"
		Me.lblRiskType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblRiskType.UseMnemonic = True
		Me.lblRiskType.Visible = True
		' 
		' lblForRecoveyType
		' 
		Me.lblForRecoveyType.AutoSize = True
		Me.lblForRecoveyType.BackColor = System.Drawing.SystemColors.Control
		Me.lblForRecoveyType.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblForRecoveyType.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblForRecoveyType.Enabled = True
		Me.lblForRecoveyType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblForRecoveyType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblForRecoveyType.Location = New System.Drawing.Point(7, 20)
		Me.lblForRecoveyType.Name = "lblForRecoveyType"
		Me.lblForRecoveyType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblForRecoveyType.Size = New System.Drawing.Size(113, 13)
		Me.lblForRecoveyType.TabIndex = 1
		Me.lblForRecoveyType.Text = "For Recovery Type:"
		Me.lblForRecoveyType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblForRecoveyType.UseMnemonic = True
		Me.lblForRecoveyType.Visible = True
		' 
		' frmReceiptDetails
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(541, 416)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOk)
		Me.Controls.Add(Me.fraTotal)
		Me.Controls.Add(Me.fraTaxes)
		Me.Controls.Add(Me.fraPayment)
		Me.Controls.Add(Me.fraReserve)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 29)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmReceiptDetails"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Recovery Details"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.fraTotal.ResumeLayout(False)
		Me.fraTaxes.ResumeLayout(False)
		Me.fraPayment.ResumeLayout(False)
		Me.fraReserve.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class