<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
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
	Public WithEvents txtInsuredName As System.Windows.Forms.TextBox
	Public WithEvents txtAgent As System.Windows.Forms.TextBox
	Public WithEvents txtIncDate As System.Windows.Forms.TextBox
	Public WithEvents txtCoverFromDate As System.Windows.Forms.TextBox
	Public WithEvents txtExpiryDate As System.Windows.Forms.TextBox
	Public WithEvents txtNetPolicyPremium As System.Windows.Forms.TextBox
	Public WithEvents txtTax As System.Windows.Forms.TextBox
	Public WithEvents txtTotalPremium As System.Windows.Forms.TextBox
	Public WithEvents txtPolicyFee As System.Windows.Forms.TextBox
	Public WithEvents txtCurrency As System.Windows.Forms.TextBox
	Public WithEvents cmdRequote As System.Windows.Forms.Button
	Public WithEvents cmdSaveQuote As System.Windows.Forms.Button
	Public WithEvents cmdMakeLive As System.Windows.Forms.Button
	Public WithEvents lblInsuredName As System.Windows.Forms.Label
	Public WithEvents lblAgent As System.Windows.Forms.Label
	Public WithEvents lblIncDate As System.Windows.Forms.Label
	Public WithEvents lblCoverFromDate As System.Windows.Forms.Label
	Public WithEvents lblExpiryDate As System.Windows.Forms.Label
	Public WithEvents lblNetPolicyPremium As System.Windows.Forms.Label
	Public WithEvents lblTax As System.Windows.Forms.Label
	Public WithEvents lblPolicyFee As System.Windows.Forms.Label
	Public WithEvents lblTotalPremium As System.Windows.Forms.Label
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.txtInsuredName = New System.Windows.Forms.TextBox
		Me.txtAgent = New System.Windows.Forms.TextBox
		Me.txtIncDate = New System.Windows.Forms.TextBox
		Me.txtCoverFromDate = New System.Windows.Forms.TextBox
		Me.txtExpiryDate = New System.Windows.Forms.TextBox
		Me.txtNetPolicyPremium = New System.Windows.Forms.TextBox
		Me.txtTax = New System.Windows.Forms.TextBox
		Me.txtTotalPremium = New System.Windows.Forms.TextBox
		Me.txtPolicyFee = New System.Windows.Forms.TextBox
		Me.txtCurrency = New System.Windows.Forms.TextBox
		Me.cmdRequote = New System.Windows.Forms.Button
		Me.cmdSaveQuote = New System.Windows.Forms.Button
		Me.cmdMakeLive = New System.Windows.Forms.Button
		Me.lblInsuredName = New System.Windows.Forms.Label
		Me.lblAgent = New System.Windows.Forms.Label
		Me.lblIncDate = New System.Windows.Forms.Label
		Me.lblCoverFromDate = New System.Windows.Forms.Label
		Me.lblExpiryDate = New System.Windows.Forms.Label
		Me.lblNetPolicyPremium = New System.Windows.Forms.Label
		Me.lblTax = New System.Windows.Forms.Label
		Me.lblPolicyFee = New System.Windows.Forms.Label
		Me.lblTotalPremium = New System.Windows.Forms.Label
		Me.lblCurrency = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' txtInsuredName
		' 
		Me.txtInsuredName.AcceptsReturn = True
		Me.txtInsuredName.AutoSize = False
		Me.txtInsuredName.BackColor = System.Drawing.SystemColors.Control
		Me.txtInsuredName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtInsuredName.CausesValidation = True
		Me.txtInsuredName.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtInsuredName.Enabled = True
		Me.txtInsuredName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtInsuredName.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtInsuredName.HideSelection = True
		Me.txtInsuredName.Location = New System.Drawing.Point(272, 16)
		Me.txtInsuredName.MaxLength = 0
		Me.txtInsuredName.Multiline = False
		Me.txtInsuredName.Name = "txtInsuredName"
		Me.txtInsuredName.ReadOnly = True
		Me.txtInsuredName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtInsuredName.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtInsuredName.Size = New System.Drawing.Size(249, 20)
		Me.txtInsuredName.TabIndex = 0
		Me.txtInsuredName.TabStop = True
		Me.txtInsuredName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtInsuredName.Visible = True
		' 
		' txtAgent
		' 
		Me.txtAgent.AcceptsReturn = True
		Me.txtAgent.AutoSize = False
		Me.txtAgent.BackColor = System.Drawing.SystemColors.Control
		Me.txtAgent.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtAgent.CausesValidation = True
		Me.txtAgent.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAgent.Enabled = True
		Me.txtAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtAgent.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAgent.HideSelection = True
		Me.txtAgent.Location = New System.Drawing.Point(272, 40)
		Me.txtAgent.MaxLength = 0
		Me.txtAgent.Multiline = False
		Me.txtAgent.Name = "txtAgent"
		Me.txtAgent.ReadOnly = True
		Me.txtAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAgent.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAgent.Size = New System.Drawing.Size(249, 20)
		Me.txtAgent.TabIndex = 1
		Me.txtAgent.TabStop = True
		Me.txtAgent.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtAgent.Visible = True
		' 
		' txtIncDate
		' 
		Me.txtIncDate.AcceptsReturn = True
		Me.txtIncDate.AutoSize = False
		Me.txtIncDate.BackColor = System.Drawing.SystemColors.Control
		Me.txtIncDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtIncDate.CausesValidation = True
		Me.txtIncDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtIncDate.Enabled = True
		Me.txtIncDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtIncDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtIncDate.HideSelection = True
		Me.txtIncDate.Location = New System.Drawing.Point(272, 72)
		Me.txtIncDate.MaxLength = 0
		Me.txtIncDate.Multiline = False
		Me.txtIncDate.Name = "txtIncDate"
		Me.txtIncDate.ReadOnly = True
		Me.txtIncDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtIncDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtIncDate.Size = New System.Drawing.Size(185, 20)
		Me.txtIncDate.TabIndex = 2
		Me.txtIncDate.TabStop = True
		Me.txtIncDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtIncDate.Visible = True
		' 
		' txtCoverFromDate
		' 
		Me.txtCoverFromDate.AcceptsReturn = True
		Me.txtCoverFromDate.AutoSize = False
		Me.txtCoverFromDate.BackColor = System.Drawing.SystemColors.Control
		Me.txtCoverFromDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtCoverFromDate.CausesValidation = True
		Me.txtCoverFromDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtCoverFromDate.Enabled = True
		Me.txtCoverFromDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtCoverFromDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtCoverFromDate.HideSelection = True
		Me.txtCoverFromDate.Location = New System.Drawing.Point(272, 96)
		Me.txtCoverFromDate.MaxLength = 0
		Me.txtCoverFromDate.Multiline = False
		Me.txtCoverFromDate.Name = "txtCoverFromDate"
		Me.txtCoverFromDate.ReadOnly = True
		Me.txtCoverFromDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtCoverFromDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtCoverFromDate.Size = New System.Drawing.Size(185, 20)
		Me.txtCoverFromDate.TabIndex = 3
		Me.txtCoverFromDate.TabStop = True
		Me.txtCoverFromDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtCoverFromDate.Visible = True
		' 
		' txtExpiryDate
		' 
		Me.txtExpiryDate.AcceptsReturn = True
		Me.txtExpiryDate.AutoSize = False
		Me.txtExpiryDate.BackColor = System.Drawing.SystemColors.Control
		Me.txtExpiryDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtExpiryDate.CausesValidation = True
		Me.txtExpiryDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtExpiryDate.Enabled = True
		Me.txtExpiryDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtExpiryDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtExpiryDate.HideSelection = True
		Me.txtExpiryDate.Location = New System.Drawing.Point(272, 120)
		Me.txtExpiryDate.MaxLength = 0
		Me.txtExpiryDate.Multiline = False
		Me.txtExpiryDate.Name = "txtExpiryDate"
		Me.txtExpiryDate.ReadOnly = True
		Me.txtExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtExpiryDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtExpiryDate.Size = New System.Drawing.Size(185, 20)
		Me.txtExpiryDate.TabIndex = 4
		Me.txtExpiryDate.TabStop = True
		Me.txtExpiryDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtExpiryDate.Visible = True
		' 
		' txtNetPolicyPremium
		' 
		Me.txtNetPolicyPremium.AcceptsReturn = True
		Me.txtNetPolicyPremium.AutoSize = False
		Me.txtNetPolicyPremium.BackColor = System.Drawing.SystemColors.Control
		Me.txtNetPolicyPremium.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtNetPolicyPremium.CausesValidation = True
		Me.txtNetPolicyPremium.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtNetPolicyPremium.Enabled = True
		Me.txtNetPolicyPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtNetPolicyPremium.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtNetPolicyPremium.HideSelection = True
		Me.txtNetPolicyPremium.Location = New System.Drawing.Point(272, 152)
		Me.txtNetPolicyPremium.MaxLength = 0
		Me.txtNetPolicyPremium.Multiline = False
		Me.txtNetPolicyPremium.Name = "txtNetPolicyPremium"
		Me.txtNetPolicyPremium.ReadOnly = True
		Me.txtNetPolicyPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtNetPolicyPremium.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtNetPolicyPremium.Size = New System.Drawing.Size(121, 20)
		Me.txtNetPolicyPremium.TabIndex = 5
		Me.txtNetPolicyPremium.TabStop = True
		Me.txtNetPolicyPremium.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtNetPolicyPremium.Visible = True
		' 
		' txtTax
		' 
		Me.txtTax.AcceptsReturn = True
		Me.txtTax.AutoSize = False
		Me.txtTax.BackColor = System.Drawing.SystemColors.Control
		Me.txtTax.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtTax.CausesValidation = True
		Me.txtTax.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtTax.Enabled = True
		Me.txtTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtTax.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtTax.HideSelection = True
		Me.txtTax.Location = New System.Drawing.Point(272, 176)
		Me.txtTax.MaxLength = 0
		Me.txtTax.Multiline = False
		Me.txtTax.Name = "txtTax"
		Me.txtTax.ReadOnly = True
		Me.txtTax.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTax.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTax.Size = New System.Drawing.Size(121, 20)
		Me.txtTax.TabIndex = 6
		Me.txtTax.TabStop = True
		Me.txtTax.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTax.Visible = True
		' 
		' txtTotalPremium
		' 
		Me.txtTotalPremium.AcceptsReturn = True
		Me.txtTotalPremium.AutoSize = False
		Me.txtTotalPremium.BackColor = System.Drawing.SystemColors.Control
		Me.txtTotalPremium.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtTotalPremium.CausesValidation = True
		Me.txtTotalPremium.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtTotalPremium.Enabled = True
		Me.txtTotalPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtTotalPremium.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtTotalPremium.HideSelection = True
		Me.txtTotalPremium.Location = New System.Drawing.Point(272, 224)
		Me.txtTotalPremium.MaxLength = 0
		Me.txtTotalPremium.Multiline = False
		Me.txtTotalPremium.Name = "txtTotalPremium"
		Me.txtTotalPremium.ReadOnly = True
		Me.txtTotalPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTotalPremium.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTotalPremium.Size = New System.Drawing.Size(121, 20)
		Me.txtTotalPremium.TabIndex = 8
		Me.txtTotalPremium.TabStop = True
		Me.txtTotalPremium.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTotalPremium.Visible = True
		' 
		' txtPolicyFee
		' 
		Me.txtPolicyFee.AcceptsReturn = True
		Me.txtPolicyFee.AutoSize = False
		Me.txtPolicyFee.BackColor = System.Drawing.SystemColors.Control
		Me.txtPolicyFee.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPolicyFee.CausesValidation = True
		Me.txtPolicyFee.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPolicyFee.Enabled = True
		Me.txtPolicyFee.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPolicyFee.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPolicyFee.HideSelection = True
		Me.txtPolicyFee.Location = New System.Drawing.Point(272, 200)
		Me.txtPolicyFee.MaxLength = 0
		Me.txtPolicyFee.Multiline = False
		Me.txtPolicyFee.Name = "txtPolicyFee"
		Me.txtPolicyFee.ReadOnly = True
		Me.txtPolicyFee.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPolicyFee.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPolicyFee.Size = New System.Drawing.Size(121, 20)
		Me.txtPolicyFee.TabIndex = 7
		Me.txtPolicyFee.TabStop = True
		Me.txtPolicyFee.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPolicyFee.Visible = True
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
		Me.txtCurrency.Location = New System.Drawing.Point(272, 248)
		Me.txtCurrency.MaxLength = 0
		Me.txtCurrency.Multiline = False
		Me.txtCurrency.Name = "txtCurrency"
		Me.txtCurrency.ReadOnly = True
		Me.txtCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtCurrency.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtCurrency.Size = New System.Drawing.Size(185, 20)
		Me.txtCurrency.TabIndex = 9
		Me.txtCurrency.TabStop = True
		Me.txtCurrency.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtCurrency.Visible = True
		' 
		' cmdRequote
		' 
		Me.cmdRequote.BackColor = System.Drawing.SystemColors.Control
		Me.cmdRequote.CausesValidation = True
		Me.cmdRequote.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdRequote.Enabled = True
		Me.cmdRequote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdRequote.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdRequote.Location = New System.Drawing.Point(98, 288)
		Me.cmdRequote.Name = "cmdRequote"
		Me.cmdRequote.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdRequote.Size = New System.Drawing.Size(105, 22)
		Me.cmdRequote.TabIndex = 10
		Me.cmdRequote.TabStop = True
		Me.cmdRequote.Text = "&Requote"
		Me.cmdRequote.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdRequote.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdSaveQuote
		' 
		Me.cmdSaveQuote.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSaveQuote.CausesValidation = True
		Me.cmdSaveQuote.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSaveQuote.Enabled = True
		Me.cmdSaveQuote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdSaveQuote.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSaveQuote.Location = New System.Drawing.Point(214, 288)
		Me.cmdSaveQuote.Name = "cmdSaveQuote"
		Me.cmdSaveQuote.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSaveQuote.Size = New System.Drawing.Size(105, 22)
		Me.cmdSaveQuote.TabIndex = 11
		Me.cmdSaveQuote.TabStop = True
		Me.cmdSaveQuote.Text = "&Save Quote"
		Me.cmdSaveQuote.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSaveQuote.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdMakeLive
		' 
		Me.cmdMakeLive.BackColor = System.Drawing.SystemColors.Control
		Me.cmdMakeLive.CausesValidation = True
		Me.cmdMakeLive.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdMakeLive.Enabled = True
		Me.cmdMakeLive.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdMakeLive.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdMakeLive.Location = New System.Drawing.Point(330, 288)
		Me.cmdMakeLive.Name = "cmdMakeLive"
		Me.cmdMakeLive.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdMakeLive.Size = New System.Drawing.Size(105, 22)
		Me.cmdMakeLive.TabIndex = 12
		Me.cmdMakeLive.TabStop = True
		Me.cmdMakeLive.Text = "&Make Live"
		Me.cmdMakeLive.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdMakeLive.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lblInsuredName
		' 
		Me.lblInsuredName.AutoSize = True
		Me.lblInsuredName.BackColor = System.Drawing.SystemColors.Control
		Me.lblInsuredName.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblInsuredName.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblInsuredName.Enabled = True
		Me.lblInsuredName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblInsuredName.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblInsuredName.Location = New System.Drawing.Point(16, 20)
		Me.lblInsuredName.Name = "lblInsuredName"
		Me.lblInsuredName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblInsuredName.Size = New System.Drawing.Size(81, 13)
		Me.lblInsuredName.TabIndex = 22
		Me.lblInsuredName.Text = "Insured Name"
		Me.lblInsuredName.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblInsuredName.UseMnemonic = True
		Me.lblInsuredName.Visible = True
		' 
		' lblAgent
		' 
		Me.lblAgent.AutoSize = True
		Me.lblAgent.BackColor = System.Drawing.SystemColors.Control
		Me.lblAgent.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAgent.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAgent.Enabled = True
		Me.lblAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAgent.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAgent.Location = New System.Drawing.Point(16, 44)
		Me.lblAgent.Name = "lblAgent"
		Me.lblAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAgent.Size = New System.Drawing.Size(33, 13)
		Me.lblAgent.TabIndex = 21
		Me.lblAgent.Text = "Agent"
		Me.lblAgent.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAgent.UseMnemonic = True
		Me.lblAgent.Visible = True
		' 
		' lblIncDate
		' 
		Me.lblIncDate.AutoSize = True
		Me.lblIncDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblIncDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblIncDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblIncDate.Enabled = True
		Me.lblIncDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblIncDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblIncDate.Location = New System.Drawing.Point(16, 76)
		Me.lblIncDate.Name = "lblIncDate"
		Me.lblIncDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblIncDate.Size = New System.Drawing.Size(236, 13)
		Me.lblIncDate.TabIndex = 20
		Me.lblIncDate.Text = "Inception Date - This Period of Insurance"
		Me.lblIncDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblIncDate.UseMnemonic = True
		Me.lblIncDate.Visible = True
		' 
		' lblCoverFromDate
		' 
		Me.lblCoverFromDate.AutoSize = True
		Me.lblCoverFromDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblCoverFromDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCoverFromDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCoverFromDate.Enabled = True
		Me.lblCoverFromDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCoverFromDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCoverFromDate.Location = New System.Drawing.Point(16, 100)
		Me.lblCoverFromDate.Name = "lblCoverFromDate"
		Me.lblCoverFromDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCoverFromDate.Size = New System.Drawing.Size(99, 13)
		Me.lblCoverFromDate.TabIndex = 19
		Me.lblCoverFromDate.Text = "Cover From Date"
		Me.lblCoverFromDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCoverFromDate.UseMnemonic = True
		Me.lblCoverFromDate.Visible = True
		' 
		' lblExpiryDate
		' 
		Me.lblExpiryDate.AutoSize = True
		Me.lblExpiryDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblExpiryDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblExpiryDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblExpiryDate.Enabled = True
		Me.lblExpiryDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblExpiryDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblExpiryDate.Location = New System.Drawing.Point(16, 124)
		Me.lblExpiryDate.Name = "lblExpiryDate"
		Me.lblExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblExpiryDate.Size = New System.Drawing.Size(67, 13)
		Me.lblExpiryDate.TabIndex = 18
		Me.lblExpiryDate.Text = "Expiry Date"
		Me.lblExpiryDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblExpiryDate.UseMnemonic = True
		Me.lblExpiryDate.Visible = True
		' 
		' lblNetPolicyPremium
		' 
		Me.lblNetPolicyPremium.AutoSize = True
		Me.lblNetPolicyPremium.BackColor = System.Drawing.SystemColors.Control
		Me.lblNetPolicyPremium.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblNetPolicyPremium.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblNetPolicyPremium.Enabled = True
		Me.lblNetPolicyPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblNetPolicyPremium.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblNetPolicyPremium.Location = New System.Drawing.Point(16, 156)
		Me.lblNetPolicyPremium.Name = "lblNetPolicyPremium"
		Me.lblNetPolicyPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblNetPolicyPremium.Size = New System.Drawing.Size(111, 13)
		Me.lblNetPolicyPremium.TabIndex = 17
		Me.lblNetPolicyPremium.Text = "Net Policy Premium"
		Me.lblNetPolicyPremium.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblNetPolicyPremium.UseMnemonic = True
		Me.lblNetPolicyPremium.Visible = True
		' 
		' lblTax
		' 
		Me.lblTax.AutoSize = True
		Me.lblTax.BackColor = System.Drawing.SystemColors.Control
		Me.lblTax.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTax.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTax.Enabled = True
		Me.lblTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTax.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTax.Location = New System.Drawing.Point(16, 180)
		Me.lblTax.Name = "lblTax"
		Me.lblTax.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTax.Size = New System.Drawing.Size(21, 13)
		Me.lblTax.TabIndex = 16
		Me.lblTax.Text = "Tax"
		Me.lblTax.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTax.UseMnemonic = True
		Me.lblTax.Visible = True
		' 
		' lblPolicyFee
		' 
		Me.lblPolicyFee.AutoSize = True
		Me.lblPolicyFee.BackColor = System.Drawing.SystemColors.Control
		Me.lblPolicyFee.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPolicyFee.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPolicyFee.Enabled = True
		Me.lblPolicyFee.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPolicyFee.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPolicyFee.Location = New System.Drawing.Point(16, 204)
		Me.lblPolicyFee.Name = "lblPolicyFee"
		Me.lblPolicyFee.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPolicyFee.Size = New System.Drawing.Size(57, 13)
		Me.lblPolicyFee.TabIndex = 15
		Me.lblPolicyFee.Text = "Policy Fee"
		Me.lblPolicyFee.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPolicyFee.UseMnemonic = True
		Me.lblPolicyFee.Visible = True
		' 
		' lblTotalPremium
		' 
		Me.lblTotalPremium.AutoSize = True
		Me.lblTotalPremium.BackColor = System.Drawing.SystemColors.Control
		Me.lblTotalPremium.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTotalPremium.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTotalPremium.Enabled = True
		Me.lblTotalPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTotalPremium.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTotalPremium.Location = New System.Drawing.Point(16, 228)
		Me.lblTotalPremium.Name = "lblTotalPremium"
		Me.lblTotalPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTotalPremium.Size = New System.Drawing.Size(227, 13)
		Me.lblTotalPremium.TabIndex = 14
		Me.lblTotalPremium.Text = "Total Policy Premium (incl Tax and Fee)"
		Me.lblTotalPremium.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTotalPremium.UseMnemonic = True
		Me.lblTotalPremium.Visible = True
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
		Me.lblCurrency.Location = New System.Drawing.Point(16, 252)
		Me.lblCurrency.Name = "lblCurrency"
		Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCurrency.Size = New System.Drawing.Size(53, 13)
		Me.lblCurrency.TabIndex = 13
		Me.lblCurrency.Text = "Currency"
		Me.lblCurrency.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCurrency.UseMnemonic = True
		Me.lblCurrency.Visible = True
		' 
		' frmInterface
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(533, 321)
		Me.ControlBox = True
		Me.Controls.Add(Me.txtInsuredName)
		Me.Controls.Add(Me.txtAgent)
		Me.Controls.Add(Me.txtIncDate)
		Me.Controls.Add(Me.txtCoverFromDate)
		Me.Controls.Add(Me.txtExpiryDate)
		Me.Controls.Add(Me.txtNetPolicyPremium)
		Me.Controls.Add(Me.txtTax)
		Me.Controls.Add(Me.txtTotalPremium)
		Me.Controls.Add(Me.txtPolicyFee)
		Me.Controls.Add(Me.txtCurrency)
		Me.Controls.Add(Me.cmdRequote)
		Me.Controls.Add(Me.cmdSaveQuote)
		Me.Controls.Add(Me.cmdMakeLive)
		Me.Controls.Add(Me.lblInsuredName)
		Me.Controls.Add(Me.lblAgent)
		Me.Controls.Add(Me.lblIncDate)
		Me.Controls.Add(Me.lblCoverFromDate)
		Me.Controls.Add(Me.lblExpiryDate)
		Me.Controls.Add(Me.lblNetPolicyPremium)
		Me.Controls.Add(Me.lblTax)
		Me.Controls.Add(Me.lblPolicyFee)
		Me.Controls.Add(Me.lblTotalPremium)
		Me.Controls.Add(Me.lblCurrency)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Policy Summary"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class