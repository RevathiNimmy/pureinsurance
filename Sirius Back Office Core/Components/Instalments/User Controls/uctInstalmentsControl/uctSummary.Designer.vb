<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctSummary
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		UserControl_Initialize()
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
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Friend WithEvents txtLastInstalmentDate As System.Windows.Forms.TextBox
	Friend WithEvents txtLastInstAmount As System.Windows.Forms.TextBox
	Friend WithEvents txtNextInstalmentDate As System.Windows.Forms.TextBox
	Friend WithEvents txtFirstInstalmentDate As System.Windows.Forms.TextBox
	Friend WithEvents txtOtherInstAmount As System.Windows.Forms.TextBox
	Friend WithEvents txtFirstInstAmount As System.Windows.Forms.TextBox
	Friend WithEvents txtTaxes As System.Windows.Forms.TextBox
	Friend WithEvents txtProtection As System.Windows.Forms.TextBox
	Friend WithEvents txtDeposit As System.Windows.Forms.TextBox
	Friend WithEvents txtInterest As System.Windows.Forms.TextBox
	Friend WithEvents txtAdminCharge As System.Windows.Forms.TextBox
	Friend WithEvents lblLastInstalmentDate As System.Windows.Forms.Label
	Friend WithEvents lblLastInstAmount As System.Windows.Forms.Label
	Friend WithEvents lblOtherInstAmount As System.Windows.Forms.Label
	Friend WithEvents lblFirstInstAmount As System.Windows.Forms.Label
	Friend WithEvents lblNextInstalmentDate As System.Windows.Forms.Label
	Friend WithEvents lblFirstInstalmentDate As System.Windows.Forms.Label
	Friend WithEvents Line1 As System.Windows.Forms.Label
	Friend WithEvents lblTaxes As System.Windows.Forms.Label
	Friend WithEvents lblInterest As System.Windows.Forms.Label
	Friend WithEvents lblAdminCharge As System.Windows.Forms.Label
	Friend WithEvents lblProtection As System.Windows.Forms.Label
	Friend WithEvents lblDeposit As System.Windows.Forms.Label
	Friend WithEvents fraBreakdown As System.Windows.Forms.GroupBox
	Friend WithEvents txtBalance As System.Windows.Forms.TextBox
	Friend WithEvents txtOriginalDebt As System.Windows.Forms.TextBox
	Friend WithEvents txtPlanStatus As System.Windows.Forms.TextBox
	Friend WithEvents txtArrears As System.Windows.Forms.TextBox
	Friend WithEvents lblPlanStatus As System.Windows.Forms.Label
	Friend WithEvents lblArrears As System.Windows.Forms.Label
	Friend WithEvents lblBalance As System.Windows.Forms.Label
	Friend WithEvents lblOriginalDebt As System.Windows.Forms.Label
	Friend WithEvents fraSummary As System.Windows.Forms.GroupBox
	Friend WithEvents txtReference As System.Windows.Forms.TextBox
	Friend WithEvents txtAgent As System.Windows.Forms.TextBox
	Friend WithEvents cmdAgent As System.Windows.Forms.Button
	Friend WithEvents lblReference As System.Windows.Forms.Label
	Friend WithEvents fraAgent As System.Windows.Forms.GroupBox
	Friend WithEvents cmdCalculate As System.Windows.Forms.Button
	Friend WithEvents txtInstalment As System.Windows.Forms.TextBox
	Friend WithEvents txtFirstPayment As System.Windows.Forms.TextBox
	Friend WithEvents cboDayInMonth As System.Windows.Forms.ComboBox
	Friend WithEvents cboFrequency As System.Windows.Forms.ComboBox
	Friend WithEvents cboMediaType As System.Windows.Forms.ComboBox
	Friend WithEvents cboWeekday As System.Windows.Forms.ComboBox
	Friend WithEvents lblInstalment As System.Windows.Forms.Label
	Friend WithEvents lblFirstPayment As System.Windows.Forms.Label
	Friend WithEvents lblFrequency As System.Windows.Forms.Label
	Friend WithEvents lblDayInMonth As System.Windows.Forms.Label
	Friend WithEvents lblMediaType As System.Windows.Forms.Label
	Friend WithEvents lblWeekday As System.Windows.Forms.Label
	Friend WithEvents fraRequest As System.Windows.Forms.GroupBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctSummary))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.fraBreakdown = New System.Windows.Forms.GroupBox
		Me.txtLastInstalmentDate = New System.Windows.Forms.TextBox
		Me.txtLastInstAmount = New System.Windows.Forms.TextBox
		Me.txtNextInstalmentDate = New System.Windows.Forms.TextBox
		Me.txtFirstInstalmentDate = New System.Windows.Forms.TextBox
		Me.txtOtherInstAmount = New System.Windows.Forms.TextBox
		Me.txtFirstInstAmount = New System.Windows.Forms.TextBox
		Me.txtTaxes = New System.Windows.Forms.TextBox
		Me.txtProtection = New System.Windows.Forms.TextBox
		Me.txtDeposit = New System.Windows.Forms.TextBox
		Me.txtInterest = New System.Windows.Forms.TextBox
		Me.txtAdminCharge = New System.Windows.Forms.TextBox
		Me.lblLastInstalmentDate = New System.Windows.Forms.Label
		Me.lblLastInstAmount = New System.Windows.Forms.Label
		Me.lblOtherInstAmount = New System.Windows.Forms.Label
		Me.lblFirstInstAmount = New System.Windows.Forms.Label
		Me.lblNextInstalmentDate = New System.Windows.Forms.Label
		Me.lblFirstInstalmentDate = New System.Windows.Forms.Label
		Me.Line1 = New System.Windows.Forms.Label
		Me.lblTaxes = New System.Windows.Forms.Label
		Me.lblInterest = New System.Windows.Forms.Label
		Me.lblAdminCharge = New System.Windows.Forms.Label
		Me.lblProtection = New System.Windows.Forms.Label
		Me.lblDeposit = New System.Windows.Forms.Label
		Me.fraSummary = New System.Windows.Forms.GroupBox
		Me.txtBalance = New System.Windows.Forms.TextBox
		Me.txtOriginalDebt = New System.Windows.Forms.TextBox
		Me.txtPlanStatus = New System.Windows.Forms.TextBox
		Me.txtArrears = New System.Windows.Forms.TextBox
		Me.lblPlanStatus = New System.Windows.Forms.Label
		Me.lblArrears = New System.Windows.Forms.Label
		Me.lblBalance = New System.Windows.Forms.Label
		Me.lblOriginalDebt = New System.Windows.Forms.Label
		Me.fraAgent = New System.Windows.Forms.GroupBox
		Me.txtReference = New System.Windows.Forms.TextBox
		Me.txtAgent = New System.Windows.Forms.TextBox
		Me.cmdAgent = New System.Windows.Forms.Button
		Me.lblReference = New System.Windows.Forms.Label
		Me.fraRequest = New System.Windows.Forms.GroupBox
		Me.cmdCalculate = New System.Windows.Forms.Button
		Me.txtInstalment = New System.Windows.Forms.TextBox
		Me.txtFirstPayment = New System.Windows.Forms.TextBox
		Me.cboDayInMonth = New System.Windows.Forms.ComboBox
		Me.cboFrequency = New System.Windows.Forms.ComboBox
		Me.cboMediaType = New System.Windows.Forms.ComboBox
		Me.cboWeekday = New System.Windows.Forms.ComboBox
		Me.lblInstalment = New System.Windows.Forms.Label
		Me.lblFirstPayment = New System.Windows.Forms.Label
		Me.lblFrequency = New System.Windows.Forms.Label
		Me.lblDayInMonth = New System.Windows.Forms.Label
		Me.lblMediaType = New System.Windows.Forms.Label
		Me.lblWeekday = New System.Windows.Forms.Label
		Me.fraBreakdown.SuspendLayout()
		Me.fraSummary.SuspendLayout()
		Me.fraAgent.SuspendLayout()
		Me.fraRequest.SuspendLayout()
		Me.SuspendLayout()
		' 
		' fraBreakdown
		' 
		Me.fraBreakdown.BackColor = System.Drawing.SystemColors.Control
		Me.fraBreakdown.Controls.Add(Me.txtLastInstalmentDate)
		Me.fraBreakdown.Controls.Add(Me.txtLastInstAmount)
		Me.fraBreakdown.Controls.Add(Me.txtNextInstalmentDate)
		Me.fraBreakdown.Controls.Add(Me.txtFirstInstalmentDate)
		Me.fraBreakdown.Controls.Add(Me.txtOtherInstAmount)
		Me.fraBreakdown.Controls.Add(Me.txtFirstInstAmount)
		Me.fraBreakdown.Controls.Add(Me.txtTaxes)
		Me.fraBreakdown.Controls.Add(Me.txtProtection)
		Me.fraBreakdown.Controls.Add(Me.txtDeposit)
		Me.fraBreakdown.Controls.Add(Me.txtInterest)
		Me.fraBreakdown.Controls.Add(Me.txtAdminCharge)
		Me.fraBreakdown.Controls.Add(Me.lblLastInstalmentDate)
		Me.fraBreakdown.Controls.Add(Me.lblLastInstAmount)
		Me.fraBreakdown.Controls.Add(Me.lblOtherInstAmount)
		Me.fraBreakdown.Controls.Add(Me.lblFirstInstAmount)
		Me.fraBreakdown.Controls.Add(Me.lblNextInstalmentDate)
		Me.fraBreakdown.Controls.Add(Me.lblFirstInstalmentDate)
		Me.fraBreakdown.Controls.Add(Me.Line1)
		Me.fraBreakdown.Controls.Add(Me.lblTaxes)
		Me.fraBreakdown.Controls.Add(Me.lblInterest)
		Me.fraBreakdown.Controls.Add(Me.lblAdminCharge)
		Me.fraBreakdown.Controls.Add(Me.lblProtection)
		Me.fraBreakdown.Controls.Add(Me.lblDeposit)
		Me.fraBreakdown.Enabled = True
		Me.fraBreakdown.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraBreakdown.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraBreakdown.Location = New System.Drawing.Point(8, 280)
		Me.fraBreakdown.Name = "fraBreakdown"
		Me.fraBreakdown.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraBreakdown.Size = New System.Drawing.Size(593, 201)
		Me.fraBreakdown.TabIndex = 27
		Me.fraBreakdown.Text = "Breakdown"
		Me.fraBreakdown.Visible = True
		' 
		' txtLastInstalmentDate
		' 
		Me.txtLastInstalmentDate.AcceptsReturn = True
		Me.txtLastInstalmentDate.AutoSize = False
		Me.txtLastInstalmentDate.BackColor = System.Drawing.SystemColors.Window
		Me.txtLastInstalmentDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtLastInstalmentDate.CausesValidation = True
		Me.txtLastInstalmentDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtLastInstalmentDate.Enabled = True
		Me.txtLastInstalmentDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtLastInstalmentDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtLastInstalmentDate.HideSelection = True
		Me.txtLastInstalmentDate.Location = New System.Drawing.Point(152, 168)
		Me.txtLastInstalmentDate.MaxLength = 0
		Me.txtLastInstalmentDate.Multiline = False
		Me.txtLastInstalmentDate.Name = "txtLastInstalmentDate"
		Me.txtLastInstalmentDate.ReadOnly = True
		Me.txtLastInstalmentDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtLastInstalmentDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtLastInstalmentDate.Size = New System.Drawing.Size(121, 21)
		Me.txtLastInstalmentDate.TabIndex = 48
		Me.txtLastInstalmentDate.TabStop = True
		Me.txtLastInstalmentDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtLastInstalmentDate.Visible = True
		' 
		' txtLastInstAmount
		' 
		Me.txtLastInstAmount.AcceptsReturn = True
		Me.txtLastInstAmount.AutoSize = False
		Me.txtLastInstAmount.BackColor = System.Drawing.SystemColors.Window
		Me.txtLastInstAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtLastInstAmount.CausesValidation = True
		Me.txtLastInstAmount.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtLastInstAmount.Enabled = True
		Me.txtLastInstAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtLastInstAmount.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtLastInstAmount.HideSelection = True
		Me.txtLastInstAmount.Location = New System.Drawing.Point(400, 168)
		Me.txtLastInstAmount.MaxLength = 0
		Me.txtLastInstAmount.Multiline = False
		Me.txtLastInstAmount.Name = "txtLastInstAmount"
		Me.txtLastInstAmount.ReadOnly = True
		Me.txtLastInstAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtLastInstAmount.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtLastInstAmount.Size = New System.Drawing.Size(174, 21)
		Me.txtLastInstAmount.TabIndex = 46
		Me.txtLastInstAmount.TabStop = True
		Me.txtLastInstAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtLastInstAmount.Visible = True
		' 
		' txtNextInstalmentDate
		' 
		Me.txtNextInstalmentDate.AcceptsReturn = True
		Me.txtNextInstalmentDate.AutoSize = False
		Me.txtNextInstalmentDate.BackColor = System.Drawing.SystemColors.Window
		Me.txtNextInstalmentDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtNextInstalmentDate.CausesValidation = True
		Me.txtNextInstalmentDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtNextInstalmentDate.Enabled = True
		Me.txtNextInstalmentDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtNextInstalmentDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtNextInstalmentDate.HideSelection = True
		Me.txtNextInstalmentDate.Location = New System.Drawing.Point(152, 144)
		Me.txtNextInstalmentDate.MaxLength = 0
		Me.txtNextInstalmentDate.Multiline = False
		Me.txtNextInstalmentDate.Name = "txtNextInstalmentDate"
		Me.txtNextInstalmentDate.ReadOnly = True
		Me.txtNextInstalmentDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtNextInstalmentDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtNextInstalmentDate.Size = New System.Drawing.Size(121, 21)
		Me.txtNextInstalmentDate.TabIndex = 41
		Me.txtNextInstalmentDate.TabStop = True
		Me.txtNextInstalmentDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtNextInstalmentDate.Visible = True
		' 
		' txtFirstInstalmentDate
		' 
		Me.txtFirstInstalmentDate.AcceptsReturn = True
		Me.txtFirstInstalmentDate.AutoSize = False
		Me.txtFirstInstalmentDate.BackColor = System.Drawing.SystemColors.Window
		Me.txtFirstInstalmentDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtFirstInstalmentDate.CausesValidation = True
		Me.txtFirstInstalmentDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtFirstInstalmentDate.Enabled = True
		Me.txtFirstInstalmentDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtFirstInstalmentDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtFirstInstalmentDate.HideSelection = True
		Me.txtFirstInstalmentDate.Location = New System.Drawing.Point(152, 120)
		Me.txtFirstInstalmentDate.MaxLength = 0
		Me.txtFirstInstalmentDate.Multiline = False
		Me.txtFirstInstalmentDate.Name = "txtFirstInstalmentDate"
		Me.txtFirstInstalmentDate.ReadOnly = True
		Me.txtFirstInstalmentDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtFirstInstalmentDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtFirstInstalmentDate.Size = New System.Drawing.Size(121, 21)
		Me.txtFirstInstalmentDate.TabIndex = 40
		Me.txtFirstInstalmentDate.TabStop = True
		Me.txtFirstInstalmentDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtFirstInstalmentDate.Visible = True
		' 
		' txtOtherInstAmount
		' 
		Me.txtOtherInstAmount.AcceptsReturn = True
		Me.txtOtherInstAmount.AutoSize = False
		Me.txtOtherInstAmount.BackColor = System.Drawing.SystemColors.Window
		Me.txtOtherInstAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtOtherInstAmount.CausesValidation = True
		Me.txtOtherInstAmount.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtOtherInstAmount.Enabled = True
		Me.txtOtherInstAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtOtherInstAmount.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtOtherInstAmount.HideSelection = True
		Me.txtOtherInstAmount.Location = New System.Drawing.Point(400, 144)
		Me.txtOtherInstAmount.MaxLength = 0
		Me.txtOtherInstAmount.Multiline = False
		Me.txtOtherInstAmount.Name = "txtOtherInstAmount"
		Me.txtOtherInstAmount.ReadOnly = True
		Me.txtOtherInstAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtOtherInstAmount.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtOtherInstAmount.Size = New System.Drawing.Size(174, 21)
		Me.txtOtherInstAmount.TabIndex = 39
		Me.txtOtherInstAmount.TabStop = True
		Me.txtOtherInstAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtOtherInstAmount.Visible = True
		' 
		' txtFirstInstAmount
		' 
		Me.txtFirstInstAmount.AcceptsReturn = True
		Me.txtFirstInstAmount.AutoSize = False
		Me.txtFirstInstAmount.BackColor = System.Drawing.SystemColors.Window
		Me.txtFirstInstAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtFirstInstAmount.CausesValidation = True
		Me.txtFirstInstAmount.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtFirstInstAmount.Enabled = True
		Me.txtFirstInstAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtFirstInstAmount.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtFirstInstAmount.HideSelection = True
		Me.txtFirstInstAmount.Location = New System.Drawing.Point(400, 120)
		Me.txtFirstInstAmount.MaxLength = 0
		Me.txtFirstInstAmount.Multiline = False
		Me.txtFirstInstAmount.Name = "txtFirstInstAmount"
		Me.txtFirstInstAmount.ReadOnly = True
		Me.txtFirstInstAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtFirstInstAmount.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtFirstInstAmount.Size = New System.Drawing.Size(174, 21)
		Me.txtFirstInstAmount.TabIndex = 38
		Me.txtFirstInstAmount.TabStop = True
		Me.txtFirstInstAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtFirstInstAmount.Visible = True
		' 
		' txtTaxes
		' 
		Me.txtTaxes.AcceptsReturn = True
		Me.txtTaxes.AutoSize = False
		Me.txtTaxes.BackColor = System.Drawing.SystemColors.Window
		Me.txtTaxes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtTaxes.CausesValidation = True
		Me.txtTaxes.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtTaxes.Enabled = True
		Me.txtTaxes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtTaxes.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtTaxes.HideSelection = True
		Me.txtTaxes.Location = New System.Drawing.Point(400, 72)
		Me.txtTaxes.MaxLength = 0
		Me.txtTaxes.Multiline = False
		Me.txtTaxes.Name = "txtTaxes"
		Me.txtTaxes.ReadOnly = True
		Me.txtTaxes.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTaxes.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTaxes.Size = New System.Drawing.Size(174, 21)
		Me.txtTaxes.TabIndex = 36
		Me.txtTaxes.TabStop = True
		Me.txtTaxes.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTaxes.Visible = True
		' 
		' txtProtection
		' 
		Me.txtProtection.AcceptsReturn = True
		Me.txtProtection.AutoSize = False
		Me.txtProtection.BackColor = System.Drawing.SystemColors.Window
		Me.txtProtection.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtProtection.CausesValidation = True
		Me.txtProtection.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtProtection.Enabled = True
		Me.txtProtection.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtProtection.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtProtection.HideSelection = True
		Me.txtProtection.Location = New System.Drawing.Point(152, 48)
		Me.txtProtection.MaxLength = 0
		Me.txtProtection.Multiline = False
		Me.txtProtection.Name = "txtProtection"
		Me.txtProtection.ReadOnly = True
		Me.txtProtection.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtProtection.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtProtection.Size = New System.Drawing.Size(121, 21)
		Me.txtProtection.TabIndex = 31
		Me.txtProtection.TabStop = True
		Me.txtProtection.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtProtection.Visible = True
		' 
		' txtDeposit
		' 
		Me.txtDeposit.AcceptsReturn = True
		Me.txtDeposit.AutoSize = False
		Me.txtDeposit.BackColor = System.Drawing.SystemColors.Window
		Me.txtDeposit.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDeposit.CausesValidation = True
		Me.txtDeposit.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDeposit.Enabled = True
		Me.txtDeposit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDeposit.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDeposit.HideSelection = True
		Me.txtDeposit.Location = New System.Drawing.Point(152, 24)
		Me.txtDeposit.MaxLength = 0
		Me.txtDeposit.Multiline = False
		Me.txtDeposit.Name = "txtDeposit"
		Me.txtDeposit.ReadOnly = True
		Me.txtDeposit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDeposit.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDeposit.Size = New System.Drawing.Size(121, 21)
		Me.txtDeposit.TabIndex = 30
		Me.txtDeposit.TabStop = True
		Me.txtDeposit.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDeposit.Visible = True
		' 
		' txtInterest
		' 
		Me.txtInterest.AcceptsReturn = True
		Me.txtInterest.AutoSize = False
		Me.txtInterest.BackColor = System.Drawing.SystemColors.Window
		Me.txtInterest.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtInterest.CausesValidation = True
		Me.txtInterest.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtInterest.Enabled = True
		Me.txtInterest.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtInterest.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtInterest.HideSelection = True
		Me.txtInterest.Location = New System.Drawing.Point(400, 48)
		Me.txtInterest.MaxLength = 0
		Me.txtInterest.Multiline = False
		Me.txtInterest.Name = "txtInterest"
		Me.txtInterest.ReadOnly = True
		Me.txtInterest.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtInterest.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtInterest.Size = New System.Drawing.Size(174, 21)
		Me.txtInterest.TabIndex = 29
		Me.txtInterest.TabStop = True
		Me.txtInterest.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtInterest.Visible = True
		' 
		' txtAdminCharge
		' 
		Me.txtAdminCharge.AcceptsReturn = True
		Me.txtAdminCharge.AutoSize = False
		Me.txtAdminCharge.BackColor = System.Drawing.SystemColors.Window
		Me.txtAdminCharge.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtAdminCharge.CausesValidation = True
		Me.txtAdminCharge.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAdminCharge.Enabled = True
		Me.txtAdminCharge.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtAdminCharge.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAdminCharge.HideSelection = True
		Me.txtAdminCharge.Location = New System.Drawing.Point(400, 24)
		Me.txtAdminCharge.MaxLength = 0
		Me.txtAdminCharge.Multiline = False
		Me.txtAdminCharge.Name = "txtAdminCharge"
		Me.txtAdminCharge.ReadOnly = True
		Me.txtAdminCharge.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAdminCharge.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAdminCharge.Size = New System.Drawing.Size(174, 21)
		Me.txtAdminCharge.TabIndex = 28
		Me.txtAdminCharge.TabStop = True
		Me.txtAdminCharge.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtAdminCharge.Visible = True
		' 
		' lblLastInstalmentDate
		' 
		Me.lblLastInstalmentDate.AutoSize = True
		Me.lblLastInstalmentDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblLastInstalmentDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblLastInstalmentDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblLastInstalmentDate.Enabled = True
		Me.lblLastInstalmentDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblLastInstalmentDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLastInstalmentDate.Location = New System.Drawing.Point(17, 172)
		Me.lblLastInstalmentDate.Name = "lblLastInstalmentDate"
		Me.lblLastInstalmentDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblLastInstalmentDate.Size = New System.Drawing.Size(119, 13)
		Me.lblLastInstalmentDate.TabIndex = 49
		Me.lblLastInstalmentDate.Text = "Last Instalment Date"
		Me.lblLastInstalmentDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblLastInstalmentDate.UseMnemonic = True
		Me.lblLastInstalmentDate.Visible = True
		' 
		' lblLastInstAmount
		' 
		Me.lblLastInstAmount.AutoSize = True
		Me.lblLastInstAmount.BackColor = System.Drawing.SystemColors.Control
		Me.lblLastInstAmount.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblLastInstAmount.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblLastInstAmount.Enabled = True
		Me.lblLastInstAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblLastInstAmount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLastInstAmount.Location = New System.Drawing.Point(296, 172)
		Me.lblLastInstAmount.Name = "lblLastInstAmount"
		Me.lblLastInstAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblLastInstAmount.Size = New System.Drawing.Size(88, 13)
		Me.lblLastInstAmount.TabIndex = 47
		Me.lblLastInstAmount.Text = "Last Instalment"
		Me.lblLastInstAmount.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblLastInstAmount.UseMnemonic = True
		Me.lblLastInstAmount.Visible = True
		' 
		' lblOtherInstAmount
		' 
		Me.lblOtherInstAmount.AutoSize = True
		Me.lblOtherInstAmount.BackColor = System.Drawing.SystemColors.Control
		Me.lblOtherInstAmount.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblOtherInstAmount.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblOtherInstAmount.Enabled = True
		Me.lblOtherInstAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblOtherInstAmount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblOtherInstAmount.Location = New System.Drawing.Point(287, 148)
		Me.lblOtherInstAmount.Name = "lblOtherInstAmount"
		Me.lblOtherInstAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblOtherInstAmount.Size = New System.Drawing.Size(97, 13)
		Me.lblOtherInstAmount.TabIndex = 45
		Me.lblOtherInstAmount.Text = "Other Instalment"
		Me.lblOtherInstAmount.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblOtherInstAmount.UseMnemonic = True
		Me.lblOtherInstAmount.Visible = True
		' 
		' lblFirstInstAmount
		' 
		Me.lblFirstInstAmount.AutoSize = True
		Me.lblFirstInstAmount.BackColor = System.Drawing.SystemColors.Control
		Me.lblFirstInstAmount.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblFirstInstAmount.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblFirstInstAmount.Enabled = True
		Me.lblFirstInstAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblFirstInstAmount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblFirstInstAmount.Location = New System.Drawing.Point(295, 124)
		Me.lblFirstInstAmount.Name = "lblFirstInstAmount"
		Me.lblFirstInstAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblFirstInstAmount.Size = New System.Drawing.Size(89, 13)
		Me.lblFirstInstAmount.TabIndex = 44
		Me.lblFirstInstAmount.Text = "First Instalment"
		Me.lblFirstInstAmount.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblFirstInstAmount.UseMnemonic = True
		Me.lblFirstInstAmount.Visible = True
		' 
		' lblNextInstalmentDate
		' 
		Me.lblNextInstalmentDate.AutoSize = True
		Me.lblNextInstalmentDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblNextInstalmentDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblNextInstalmentDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblNextInstalmentDate.Enabled = True
		Me.lblNextInstalmentDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblNextInstalmentDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblNextInstalmentDate.Location = New System.Drawing.Point(16, 148)
		Me.lblNextInstalmentDate.Name = "lblNextInstalmentDate"
		Me.lblNextInstalmentDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblNextInstalmentDate.Size = New System.Drawing.Size(122, 13)
		Me.lblNextInstalmentDate.TabIndex = 43
		Me.lblNextInstalmentDate.Text = "Next Instalment Date"
		Me.lblNextInstalmentDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblNextInstalmentDate.UseMnemonic = True
		Me.lblNextInstalmentDate.Visible = True
		' 
		' lblFirstInstalmentDate
		' 
		Me.lblFirstInstalmentDate.AutoSize = True
		Me.lblFirstInstalmentDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblFirstInstalmentDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblFirstInstalmentDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblFirstInstalmentDate.Enabled = True
		Me.lblFirstInstalmentDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblFirstInstalmentDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblFirstInstalmentDate.Location = New System.Drawing.Point(16, 124)
		Me.lblFirstInstalmentDate.Name = "lblFirstInstalmentDate"
		Me.lblFirstInstalmentDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblFirstInstalmentDate.Size = New System.Drawing.Size(120, 13)
		Me.lblFirstInstalmentDate.TabIndex = 42
		Me.lblFirstInstalmentDate.Text = "First Instalment Date"
		Me.lblFirstInstalmentDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblFirstInstalmentDate.UseMnemonic = True
		Me.lblFirstInstalmentDate.Visible = True
		' 
		' Line1
		' 
		Me.Line1.BackColor = System.Drawing.SystemColors.WindowText
		Me.Line1.Location = New System.Drawing.Point(16, 104)
		Me.Line1.Name = "Line1"
		Me.Line1.Size = New System.Drawing.Size(560, 1)
		Me.Line1.Visible = True
		' 
		' lblTaxes
		' 
		Me.lblTaxes.AutoSize = True
		Me.lblTaxes.BackColor = System.Drawing.SystemColors.Control
		Me.lblTaxes.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTaxes.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTaxes.Enabled = True
		Me.lblTaxes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTaxes.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTaxes.Location = New System.Drawing.Point(352, 76)
		Me.lblTaxes.Name = "lblTaxes"
		Me.lblTaxes.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTaxes.Size = New System.Drawing.Size(34, 13)
		Me.lblTaxes.TabIndex = 37
		Me.lblTaxes.Text = "Taxes"
		Me.lblTaxes.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTaxes.UseMnemonic = True
		Me.lblTaxes.Visible = True
		' 
		' lblInterest
		' 
		Me.lblInterest.AutoSize = True
		Me.lblInterest.BackColor = System.Drawing.SystemColors.Control
		Me.lblInterest.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblInterest.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblInterest.Enabled = True
		Me.lblInterest.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblInterest.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblInterest.Location = New System.Drawing.Point(341, 52)
		Me.lblInterest.Name = "lblInterest"
		Me.lblInterest.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblInterest.Size = New System.Drawing.Size(45, 13)
		Me.lblInterest.TabIndex = 35
		Me.lblInterest.Text = "Interest"
		Me.lblInterest.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblInterest.UseMnemonic = True
		Me.lblInterest.Visible = True
		' 
		' lblAdminCharge
		' 
		Me.lblAdminCharge.AutoSize = True
		Me.lblAdminCharge.BackColor = System.Drawing.SystemColors.Control
		Me.lblAdminCharge.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAdminCharge.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAdminCharge.Enabled = True
		Me.lblAdminCharge.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAdminCharge.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAdminCharge.Location = New System.Drawing.Point(304, 28)
		Me.lblAdminCharge.Name = "lblAdminCharge"
		Me.lblAdminCharge.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAdminCharge.Size = New System.Drawing.Size(82, 13)
		Me.lblAdminCharge.TabIndex = 34
		Me.lblAdminCharge.Text = "Admin Charge"
		Me.lblAdminCharge.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAdminCharge.UseMnemonic = True
		Me.lblAdminCharge.Visible = True
		' 
		' lblProtection
		' 
		Me.lblProtection.AutoSize = True
		Me.lblProtection.BackColor = System.Drawing.SystemColors.Control
		Me.lblProtection.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblProtection.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblProtection.Enabled = True
		Me.lblProtection.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblProtection.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblProtection.Location = New System.Drawing.Point(36, 52)
		Me.lblProtection.Name = "lblProtection"
		Me.lblProtection.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblProtection.Size = New System.Drawing.Size(103, 13)
		Me.lblProtection.TabIndex = 33
		Me.lblProtection.Text = "Protection Charge"
		Me.lblProtection.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblProtection.UseMnemonic = True
		Me.lblProtection.Visible = True
		' 
		' lblDeposit
		' 
		Me.lblDeposit.AutoSize = True
		Me.lblDeposit.BackColor = System.Drawing.SystemColors.Control
		Me.lblDeposit.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDeposit.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDeposit.Enabled = True
		Me.lblDeposit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDeposit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDeposit.Location = New System.Drawing.Point(96, 28)
		Me.lblDeposit.Name = "lblDeposit"
		Me.lblDeposit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDeposit.Size = New System.Drawing.Size(43, 13)
		Me.lblDeposit.TabIndex = 32
		Me.lblDeposit.Text = "Deposit"
		Me.lblDeposit.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDeposit.UseMnemonic = True
		Me.lblDeposit.Visible = True
		' 
		' fraSummary
		' 
		Me.fraSummary.BackColor = System.Drawing.SystemColors.Control
		Me.fraSummary.Controls.Add(Me.txtBalance)
		Me.fraSummary.Controls.Add(Me.txtOriginalDebt)
		Me.fraSummary.Controls.Add(Me.txtPlanStatus)
		Me.fraSummary.Controls.Add(Me.txtArrears)
		Me.fraSummary.Controls.Add(Me.lblPlanStatus)
		Me.fraSummary.Controls.Add(Me.lblArrears)
		Me.fraSummary.Controls.Add(Me.lblBalance)
		Me.fraSummary.Controls.Add(Me.lblOriginalDebt)
		Me.fraSummary.Enabled = True
		Me.fraSummary.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraSummary.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraSummary.Location = New System.Drawing.Point(8, 192)
		Me.fraSummary.Name = "fraSummary"
		Me.fraSummary.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraSummary.Size = New System.Drawing.Size(593, 81)
		Me.fraSummary.TabIndex = 18
		Me.fraSummary.Text = "Summary"
		Me.fraSummary.Visible = True
		' 
		' txtBalance
		' 
		Me.txtBalance.AcceptsReturn = True
		Me.txtBalance.AutoSize = False
		Me.txtBalance.BackColor = System.Drawing.SystemColors.Window
		Me.txtBalance.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtBalance.CausesValidation = True
		Me.txtBalance.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtBalance.Enabled = True
		Me.txtBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtBalance.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtBalance.HideSelection = True
		Me.txtBalance.Location = New System.Drawing.Point(152, 48)
		Me.txtBalance.MaxLength = 0
		Me.txtBalance.Multiline = False
		Me.txtBalance.Name = "txtBalance"
		Me.txtBalance.ReadOnly = True
		Me.txtBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtBalance.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtBalance.Size = New System.Drawing.Size(121, 21)
		Me.txtBalance.TabIndex = 22
		Me.txtBalance.TabStop = True
		Me.txtBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtBalance.Visible = True
		' 
		' txtOriginalDebt
		' 
		Me.txtOriginalDebt.AcceptsReturn = True
		Me.txtOriginalDebt.AutoSize = False
		Me.txtOriginalDebt.BackColor = System.Drawing.SystemColors.Window
		Me.txtOriginalDebt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtOriginalDebt.CausesValidation = True
		Me.txtOriginalDebt.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtOriginalDebt.Enabled = True
		Me.txtOriginalDebt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtOriginalDebt.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtOriginalDebt.HideSelection = True
		Me.txtOriginalDebt.Location = New System.Drawing.Point(152, 24)
		Me.txtOriginalDebt.MaxLength = 0
		Me.txtOriginalDebt.Multiline = False
		Me.txtOriginalDebt.Name = "txtOriginalDebt"
		Me.txtOriginalDebt.ReadOnly = True
		Me.txtOriginalDebt.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtOriginalDebt.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtOriginalDebt.Size = New System.Drawing.Size(121, 21)
		Me.txtOriginalDebt.TabIndex = 21
		Me.txtOriginalDebt.TabStop = True
		Me.txtOriginalDebt.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtOriginalDebt.Visible = True
		' 
		' txtPlanStatus
		' 
		Me.txtPlanStatus.AcceptsReturn = True
		Me.txtPlanStatus.AutoSize = False
		Me.txtPlanStatus.BackColor = System.Drawing.SystemColors.Window
		Me.txtPlanStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPlanStatus.CausesValidation = True
		Me.txtPlanStatus.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPlanStatus.Enabled = True
		Me.txtPlanStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPlanStatus.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPlanStatus.HideSelection = True
		Me.txtPlanStatus.Location = New System.Drawing.Point(400, 48)
		Me.txtPlanStatus.MaxLength = 0
		Me.txtPlanStatus.Multiline = False
		Me.txtPlanStatus.Name = "txtPlanStatus"
		Me.txtPlanStatus.ReadOnly = True
		Me.txtPlanStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPlanStatus.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPlanStatus.Size = New System.Drawing.Size(174, 21)
		Me.txtPlanStatus.TabIndex = 20
		Me.txtPlanStatus.TabStop = True
		Me.txtPlanStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPlanStatus.Visible = True
		' 
		' txtArrears
		' 
		Me.txtArrears.AcceptsReturn = True
		Me.txtArrears.AutoSize = False
		Me.txtArrears.BackColor = System.Drawing.SystemColors.Window
		Me.txtArrears.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtArrears.CausesValidation = True
		Me.txtArrears.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtArrears.Enabled = True
		Me.txtArrears.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtArrears.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtArrears.HideSelection = True
		Me.txtArrears.Location = New System.Drawing.Point(400, 24)
		Me.txtArrears.MaxLength = 0
		Me.txtArrears.Multiline = False
		Me.txtArrears.Name = "txtArrears"
		Me.txtArrears.ReadOnly = True
		Me.txtArrears.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtArrears.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtArrears.Size = New System.Drawing.Size(174, 21)
		Me.txtArrears.TabIndex = 19
		Me.txtArrears.TabStop = True
		Me.txtArrears.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtArrears.Visible = True
		' 
		' lblPlanStatus
		' 
		Me.lblPlanStatus.AutoSize = True
		Me.lblPlanStatus.BackColor = System.Drawing.SystemColors.Control
		Me.lblPlanStatus.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPlanStatus.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPlanStatus.Enabled = True
		Me.lblPlanStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPlanStatus.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPlanStatus.Location = New System.Drawing.Point(323, 52)
		Me.lblPlanStatus.Name = "lblPlanStatus"
		Me.lblPlanStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPlanStatus.Size = New System.Drawing.Size(64, 13)
		Me.lblPlanStatus.TabIndex = 26
		Me.lblPlanStatus.Text = "Plan Status"
		Me.lblPlanStatus.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPlanStatus.UseMnemonic = True
		Me.lblPlanStatus.Visible = True
		' 
		' lblArrears
		' 
		Me.lblArrears.AutoSize = True
		Me.lblArrears.BackColor = System.Drawing.SystemColors.Control
		Me.lblArrears.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblArrears.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblArrears.Enabled = True
		Me.lblArrears.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblArrears.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblArrears.Location = New System.Drawing.Point(344, 28)
		Me.lblArrears.Name = "lblArrears"
		Me.lblArrears.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblArrears.Size = New System.Drawing.Size(43, 13)
		Me.lblArrears.TabIndex = 25
		Me.lblArrears.Text = "Arrears"
		Me.lblArrears.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblArrears.UseMnemonic = True
		Me.lblArrears.Visible = True
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
		Me.lblBalance.Location = New System.Drawing.Point(56, 52)
		Me.lblBalance.Name = "lblBalance"
		Me.lblBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblBalance.Size = New System.Drawing.Size(84, 13)
		Me.lblBalance.TabIndex = 24
		Me.lblBalance.Text = "Balance Owing"
		Me.lblBalance.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblBalance.UseMnemonic = True
		Me.lblBalance.Visible = True
		' 
		' lblOriginalDebt
		' 
		Me.lblOriginalDebt.AutoSize = True
		Me.lblOriginalDebt.BackColor = System.Drawing.SystemColors.Control
		Me.lblOriginalDebt.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblOriginalDebt.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblOriginalDebt.Enabled = True
		Me.lblOriginalDebt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblOriginalDebt.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblOriginalDebt.Location = New System.Drawing.Point(65, 28)
		Me.lblOriginalDebt.Name = "lblOriginalDebt"
		Me.lblOriginalDebt.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblOriginalDebt.Size = New System.Drawing.Size(75, 13)
		Me.lblOriginalDebt.TabIndex = 23
		Me.lblOriginalDebt.Text = "Original Debt"
		Me.lblOriginalDebt.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblOriginalDebt.UseMnemonic = True
		Me.lblOriginalDebt.Visible = True
		' 
		' fraAgent
		' 
		Me.fraAgent.BackColor = System.Drawing.SystemColors.Control
		Me.fraAgent.Controls.Add(Me.txtReference)
		Me.fraAgent.Controls.Add(Me.txtAgent)
		Me.fraAgent.Controls.Add(Me.cmdAgent)
		Me.fraAgent.Controls.Add(Me.lblReference)
		Me.fraAgent.Enabled = True
		Me.fraAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraAgent.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraAgent.Location = New System.Drawing.Point(8, 128)
		Me.fraAgent.Name = "fraAgent"
		Me.fraAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraAgent.Size = New System.Drawing.Size(593, 57)
		Me.fraAgent.TabIndex = 13
		Me.fraAgent.Text = "Agent"
		Me.fraAgent.Visible = True
		' 
		' txtReference
		' 
		Me.txtReference.AcceptsReturn = True
		Me.txtReference.AutoSize = False
		Me.txtReference.BackColor = System.Drawing.SystemColors.Window
		Me.txtReference.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtReference.CausesValidation = True
		Me.txtReference.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtReference.Enabled = True
		Me.txtReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtReference.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtReference.HideSelection = True
		Me.txtReference.Location = New System.Drawing.Point(400, 23)
		Me.txtReference.MaxLength = 0
		Me.txtReference.Multiline = False
		Me.txtReference.Name = "txtReference"
		Me.txtReference.ReadOnly = False
		Me.txtReference.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtReference.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtReference.Size = New System.Drawing.Size(169, 19)
		Me.txtReference.TabIndex = 17
		Me.txtReference.TabStop = True
		Me.txtReference.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtReference.Visible = True
		' 
		' txtAgent
		' 
		Me.txtAgent.AcceptsReturn = True
		Me.txtAgent.AutoSize = False
		Me.txtAgent.BackColor = System.Drawing.SystemColors.Window
		Me.txtAgent.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.txtAgent.CausesValidation = True
		Me.txtAgent.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAgent.Enabled = True
		Me.txtAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtAgent.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAgent.HideSelection = True
		Me.txtAgent.Location = New System.Drawing.Point(96, 23)
		Me.txtAgent.MaxLength = 0
		Me.txtAgent.Multiline = False
		Me.txtAgent.Name = "txtAgent"
		Me.txtAgent.ReadOnly = True
		Me.txtAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAgent.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAgent.Size = New System.Drawing.Size(177, 19)
		Me.txtAgent.TabIndex = 15
		Me.txtAgent.TabStop = True
		Me.txtAgent.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtAgent.Visible = True
		' 
		' cmdAgent
		' 
		Me.cmdAgent.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAgent.CausesValidation = True
		Me.cmdAgent.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAgent.Enabled = True
		Me.cmdAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAgent.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAgent.Location = New System.Drawing.Point(16, 24)
		Me.cmdAgent.Name = "cmdAgent"
		Me.cmdAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAgent.Size = New System.Drawing.Size(57, 17)
		Me.cmdAgent.TabIndex = 14
		Me.cmdAgent.TabStop = True
		Me.cmdAgent.Text = "Agent..."
		Me.cmdAgent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAgent.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lblReference
		' 
		Me.lblReference.AutoSize = True
		Me.lblReference.BackColor = System.Drawing.SystemColors.Control
		Me.lblReference.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblReference.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblReference.Enabled = True
		Me.lblReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblReference.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblReference.Location = New System.Drawing.Point(328, 26)
		Me.lblReference.Name = "lblReference"
		Me.lblReference.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblReference.Size = New System.Drawing.Size(58, 13)
		Me.lblReference.TabIndex = 16
		Me.lblReference.Text = "Reference"
		Me.lblReference.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblReference.UseMnemonic = True
		Me.lblReference.Visible = True
		' 
		' fraRequest
		' 
		Me.fraRequest.BackColor = System.Drawing.SystemColors.Control
		Me.fraRequest.Controls.Add(Me.cmdCalculate)
		Me.fraRequest.Controls.Add(Me.txtInstalment)
		Me.fraRequest.Controls.Add(Me.txtFirstPayment)
		Me.fraRequest.Controls.Add(Me.cboDayInMonth)
		Me.fraRequest.Controls.Add(Me.cboFrequency)
		Me.fraRequest.Controls.Add(Me.cboMediaType)
		Me.fraRequest.Controls.Add(Me.cboWeekday)
		Me.fraRequest.Controls.Add(Me.lblInstalment)
		Me.fraRequest.Controls.Add(Me.lblFirstPayment)
		Me.fraRequest.Controls.Add(Me.lblFrequency)
		Me.fraRequest.Controls.Add(Me.lblDayInMonth)
		Me.fraRequest.Controls.Add(Me.lblMediaType)
		Me.fraRequest.Controls.Add(Me.lblWeekday)
		Me.fraRequest.Enabled = True
		Me.fraRequest.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraRequest.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraRequest.Location = New System.Drawing.Point(8, 8)
		Me.fraRequest.Name = "fraRequest"
		Me.fraRequest.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraRequest.Size = New System.Drawing.Size(593, 113)
		Me.fraRequest.TabIndex = 0
		Me.fraRequest.Text = "Instalment Plan Requested"
		Me.fraRequest.Visible = True
		' 
		' cmdCalculate
		' 
		Me.cmdCalculate.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCalculate.CausesValidation = True
		Me.cmdCalculate.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCalculate.Enabled = True
		Me.cmdCalculate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCalculate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCalculate.Location = New System.Drawing.Point(512, 80)
		Me.cmdCalculate.Name = "cmdCalculate"
		Me.cmdCalculate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCalculate.Size = New System.Drawing.Size(73, 21)
		Me.cmdCalculate.TabIndex = 50
		Me.cmdCalculate.TabStop = True
		Me.cmdCalculate.Text = "Calculate"
		Me.cmdCalculate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCalculate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtInstalment
		' 
		Me.txtInstalment.AcceptsReturn = True
		Me.txtInstalment.AutoSize = False
		Me.txtInstalment.BackColor = System.Drawing.SystemColors.Window
		Me.txtInstalment.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtInstalment.CausesValidation = True
		Me.txtInstalment.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtInstalment.Enabled = True
		Me.txtInstalment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtInstalment.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtInstalment.HideSelection = True
		Me.txtInstalment.Location = New System.Drawing.Point(400, 80)
		Me.txtInstalment.MaxLength = 0
		Me.txtInstalment.Multiline = False
		Me.txtInstalment.Name = "txtInstalment"
		Me.txtInstalment.ReadOnly = False
		Me.txtInstalment.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtInstalment.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtInstalment.Size = New System.Drawing.Size(105, 21)
		Me.txtInstalment.TabIndex = 12
		Me.txtInstalment.TabStop = True
		Me.txtInstalment.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtInstalment.Visible = True
		' 
		' txtFirstPayment
		' 
		Me.txtFirstPayment.AcceptsReturn = True
		Me.txtFirstPayment.AutoSize = False
		Me.txtFirstPayment.BackColor = System.Drawing.SystemColors.Window
		Me.txtFirstPayment.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtFirstPayment.CausesValidation = True
		Me.txtFirstPayment.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtFirstPayment.Enabled = True
		Me.txtFirstPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtFirstPayment.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtFirstPayment.HideSelection = True
		Me.txtFirstPayment.Location = New System.Drawing.Point(152, 80)
		Me.txtFirstPayment.MaxLength = 0
		Me.txtFirstPayment.Multiline = False
		Me.txtFirstPayment.Name = "txtFirstPayment"
		Me.txtFirstPayment.ReadOnly = False
		Me.txtFirstPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtFirstPayment.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtFirstPayment.Size = New System.Drawing.Size(121, 21)
		Me.txtFirstPayment.TabIndex = 11
		Me.txtFirstPayment.TabStop = True
		Me.txtFirstPayment.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtFirstPayment.Visible = True
		' 
		' cboDayInMonth
		' 
		Me.cboDayInMonth.BackColor = System.Drawing.SystemColors.Window
		Me.cboDayInMonth.CausesValidation = True
		Me.cboDayInMonth.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboDayInMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown
		Me.cboDayInMonth.Enabled = True
		Me.cboDayInMonth.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboDayInMonth.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboDayInMonth.IntegralHeight = True
		Me.cboDayInMonth.Location = New System.Drawing.Point(400, 27)
		Me.cboDayInMonth.Name = "cboDayInMonth"
		Me.cboDayInMonth.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboDayInMonth.Size = New System.Drawing.Size(105, 21)
		Me.cboDayInMonth.Sorted = False
		Me.cboDayInMonth.TabIndex = 8
		Me.cboDayInMonth.TabStop = True
		Me.cboDayInMonth.Text = "Combo1"
		Me.cboDayInMonth.Visible = True
		' 
		' cboFrequency
		' 
		Me.cboFrequency.BackColor = System.Drawing.SystemColors.Window
		Me.cboFrequency.CausesValidation = True
		Me.cboFrequency.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboFrequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown
		Me.cboFrequency.Enabled = True
		Me.cboFrequency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboFrequency.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboFrequency.IntegralHeight = True
		Me.cboFrequency.Location = New System.Drawing.Point(400, 54)
		Me.cboFrequency.Name = "cboFrequency"
		Me.cboFrequency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboFrequency.Size = New System.Drawing.Size(105, 21)
		Me.cboFrequency.Sorted = False
		Me.cboFrequency.TabIndex = 7
		Me.cboFrequency.TabStop = True
		Me.cboFrequency.Visible = True
		' 
		' cboMediaType
		' 
		Me.cboMediaType.BackColor = System.Drawing.SystemColors.Window
		Me.cboMediaType.CausesValidation = True
		Me.cboMediaType.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboMediaType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown
		Me.cboMediaType.Enabled = True
		Me.cboMediaType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboMediaType.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboMediaType.IntegralHeight = True
		Me.cboMediaType.Location = New System.Drawing.Point(152, 54)
		Me.cboMediaType.Name = "cboMediaType"
		Me.cboMediaType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboMediaType.Size = New System.Drawing.Size(121, 21)
		Me.cboMediaType.Sorted = False
		Me.cboMediaType.TabIndex = 4
		Me.cboMediaType.TabStop = True
		Me.cboMediaType.Visible = True
		' 
		' cboWeekday
		' 
		Me.cboWeekday.BackColor = System.Drawing.SystemColors.Window
		Me.cboWeekday.CausesValidation = True
		Me.cboWeekday.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboWeekday.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown
		Me.cboWeekday.Enabled = True
		Me.cboWeekday.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboWeekday.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboWeekday.IntegralHeight = True
		Me.cboWeekday.Location = New System.Drawing.Point(152, 27)
		Me.cboWeekday.Name = "cboWeekday"
		Me.cboWeekday.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboWeekday.Size = New System.Drawing.Size(121, 21)
		Me.cboWeekday.Sorted = False
		Me.cboWeekday.TabIndex = 3
		Me.cboWeekday.TabStop = True
		Me.cboWeekday.Text = "Combo1"
		Me.cboWeekday.Visible = True
		' 
		' lblInstalment
		' 
		Me.lblInstalment.AutoSize = True
		Me.lblInstalment.BackColor = System.Drawing.SystemColors.Control
		Me.lblInstalment.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblInstalment.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblInstalment.Enabled = True
		Me.lblInstalment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblInstalment.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblInstalment.Location = New System.Drawing.Point(326, 84)
		Me.lblInstalment.Name = "lblInstalment"
		Me.lblInstalment.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblInstalment.Size = New System.Drawing.Size(61, 13)
		Me.lblInstalment.TabIndex = 10
		Me.lblInstalment.Text = "Instalment"
		Me.lblInstalment.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblInstalment.UseMnemonic = True
		Me.lblInstalment.Visible = True
		' 
		' lblFirstPayment
		' 
		Me.lblFirstPayment.AutoSize = True
		Me.lblFirstPayment.BackColor = System.Drawing.SystemColors.Control
		Me.lblFirstPayment.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblFirstPayment.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblFirstPayment.Enabled = True
		Me.lblFirstPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblFirstPayment.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblFirstPayment.Location = New System.Drawing.Point(64, 84)
		Me.lblFirstPayment.Name = "lblFirstPayment"
		Me.lblFirstPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblFirstPayment.Size = New System.Drawing.Size(78, 13)
		Me.lblFirstPayment.TabIndex = 9
		Me.lblFirstPayment.Text = "First Payment"
		Me.lblFirstPayment.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblFirstPayment.UseMnemonic = True
		Me.lblFirstPayment.Visible = True
		' 
		' lblFrequency
		' 
		Me.lblFrequency.AutoSize = True
		Me.lblFrequency.BackColor = System.Drawing.SystemColors.Control
		Me.lblFrequency.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblFrequency.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblFrequency.Enabled = True
		Me.lblFrequency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblFrequency.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblFrequency.Location = New System.Drawing.Point(328, 58)
		Me.lblFrequency.Name = "lblFrequency"
		Me.lblFrequency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblFrequency.Size = New System.Drawing.Size(59, 13)
		Me.lblFrequency.TabIndex = 6
		Me.lblFrequency.Text = "Frequency"
		Me.lblFrequency.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblFrequency.UseMnemonic = True
		Me.lblFrequency.Visible = True
		' 
		' lblDayInMonth
		' 
		Me.lblDayInMonth.AutoSize = True
		Me.lblDayInMonth.BackColor = System.Drawing.SystemColors.Control
		Me.lblDayInMonth.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDayInMonth.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDayInMonth.Enabled = True
		Me.lblDayInMonth.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDayInMonth.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDayInMonth.Location = New System.Drawing.Point(312, 31)
		Me.lblDayInMonth.Name = "lblDayInMonth"
		Me.lblDayInMonth.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDayInMonth.Size = New System.Drawing.Size(75, 13)
		Me.lblDayInMonth.TabIndex = 5
		Me.lblDayInMonth.Text = "Day in Month"
		Me.lblDayInMonth.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDayInMonth.UseMnemonic = True
		Me.lblDayInMonth.Visible = True
		' 
		' lblMediaType
		' 
		Me.lblMediaType.AutoSize = True
		Me.lblMediaType.BackColor = System.Drawing.SystemColors.Control
		Me.lblMediaType.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMediaType.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMediaType.Enabled = True
		Me.lblMediaType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMediaType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMediaType.Location = New System.Drawing.Point(77, 58)
		Me.lblMediaType.Name = "lblMediaType"
		Me.lblMediaType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMediaType.Size = New System.Drawing.Size(65, 13)
		Me.lblMediaType.TabIndex = 2
		Me.lblMediaType.Text = "Media Type"
		Me.lblMediaType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMediaType.UseMnemonic = True
		Me.lblMediaType.Visible = True
		' 
		' lblWeekday
		' 
		Me.lblWeekday.AutoSize = True
		Me.lblWeekday.BackColor = System.Drawing.SystemColors.Control
		Me.lblWeekday.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblWeekday.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblWeekday.Enabled = True
		Me.lblWeekday.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblWeekday.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblWeekday.Location = New System.Drawing.Point(89, 31)
		Me.lblWeekday.Name = "lblWeekday"
		Me.lblWeekday.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblWeekday.Size = New System.Drawing.Size(53, 13)
		Me.lblWeekday.TabIndex = 1
		Me.lblWeekday.Text = "Weekday"
		Me.lblWeekday.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblWeekday.UseMnemonic = True
		Me.lblWeekday.Visible = True
		' 
		' uctSummary
		' 
		Me.ClientSize = New System.Drawing.Size(611, 481)
		Me.Controls.Add(Me.fraBreakdown)
		Me.Controls.Add(Me.fraSummary)
		Me.Controls.Add(Me.fraAgent)
		Me.Controls.Add(Me.fraRequest)
		MyBase.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		MyBase.Location = New System.Drawing.Point(0, 0)
		MyBase.Name = "uctSummary"
		Me.fraBreakdown.ResumeLayout(False)
		Me.fraSummary.ResumeLayout(False)
		Me.fraAgent.ResumeLayout(False)
		Me.fraRequest.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class