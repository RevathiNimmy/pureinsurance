<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAdd
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializeoptSalvage()
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
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblDepreciationPercent As System.Windows.Forms.Label
	Public WithEvents lblItemDescription As System.Windows.Forms.Label
	Public WithEvents lblStartingValue As System.Windows.Forms.Label
	Public WithEvents lblSettlementMethod As System.Windows.Forms.Label
	Public WithEvents lblAge As System.Windows.Forms.Label
	Public WithEvents lblPayeeOrSupplier As System.Windows.Forms.Label
	Public WithEvents lblExcess As System.Windows.Forms.Label
	Public WithEvents lblDateEntered As System.Windows.Forms.Label
	Public WithEvents lblPaymentAmount As System.Windows.Forms.Label
	Public WithEvents lblGST As System.Windows.Forms.Label
	Public WithEvents lblItemAmount As System.Windows.Forms.Label
	Public WithEvents lblDepreciation As System.Windows.Forms.Label
	Public WithEvents lblItemClaimed As System.Windows.Forms.Label
	Public WithEvents lblItemNumber As System.Windows.Forms.Label
	Public WithEvents lblLife As System.Windows.Forms.Label
	Public WithEvents lblStatus As System.Windows.Forms.Label
	Public WithEvents lblSalvage As System.Windows.Forms.Label
	Public WithEvents lblDatePaid As System.Windows.Forms.Label
	Public WithEvents lblPODate As System.Windows.Forms.Label
	Private WithEvents _optSalvage_1 As System.Windows.Forms.RadioButton
	Private WithEvents _optSalvage_0 As System.Windows.Forms.RadioButton
	Public WithEvents fraSalvage As System.Windows.Forms.Panel
	Public WithEvents txtDateEntered As System.Windows.Forms.TextBox
	Public WithEvents txtItemNumber As System.Windows.Forms.TextBox
	Public WithEvents txtItemClaimed As System.Windows.Forms.TextBox
	Public WithEvents txtPayeeOrSupplier As System.Windows.Forms.TextBox
	Public WithEvents cboSettlementMethod As System.Windows.Forms.ComboBox
	Public WithEvents txtExcess As System.Windows.Forms.TextBox
	Public WithEvents txtPaymentAmount As System.Windows.Forms.TextBox
	Public WithEvents txtGST As System.Windows.Forms.TextBox
	Public WithEvents txtDepreciation As System.Windows.Forms.TextBox
	Public WithEvents txtDepreciationPercent As System.Windows.Forms.TextBox
	Public WithEvents txtAge As System.Windows.Forms.TextBox
	Public WithEvents txtLife As System.Windows.Forms.TextBox
	Public WithEvents txtStartingValue As System.Windows.Forms.TextBox
	Public WithEvents txtItemAmount As System.Windows.Forms.TextBox
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents txtPODate As System.Windows.Forms.TextBox
	Public WithEvents txtDatePaid As System.Windows.Forms.TextBox
	Public WithEvents cboStatus As System.Windows.Forms.ComboBox
	Public WithEvents cmdItemClaimed As System.Windows.Forms.Button
	Public WithEvents cmdPayeeOrSupplier As System.Windows.Forms.Button
	Private WithEvents _tabItemDetails_TabPage0 As System.Windows.Forms.TabPage
	Private WithEvents _tabItemDetails_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents tabItemDetails As System.Windows.Forms.TabControl
	Public optSalvage(1) As System.Windows.Forms.RadioButton
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAdd))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.tabItemDetails = New System.Windows.Forms.TabControl
		Me._tabItemDetails_TabPage0 = New System.Windows.Forms.TabPage
		Me.lblDepreciationPercent = New System.Windows.Forms.Label
		Me.lblItemDescription = New System.Windows.Forms.Label
		Me.lblStartingValue = New System.Windows.Forms.Label
		Me.lblSettlementMethod = New System.Windows.Forms.Label
		Me.lblAge = New System.Windows.Forms.Label
		Me.lblPayeeOrSupplier = New System.Windows.Forms.Label
		Me.lblExcess = New System.Windows.Forms.Label
		Me.lblDateEntered = New System.Windows.Forms.Label
		Me.lblPaymentAmount = New System.Windows.Forms.Label
		Me.lblGST = New System.Windows.Forms.Label
		Me.lblItemAmount = New System.Windows.Forms.Label
		Me.lblDepreciation = New System.Windows.Forms.Label
		Me.lblItemClaimed = New System.Windows.Forms.Label
		Me.lblItemNumber = New System.Windows.Forms.Label
		Me.lblLife = New System.Windows.Forms.Label
		Me.lblStatus = New System.Windows.Forms.Label
		Me.lblSalvage = New System.Windows.Forms.Label
		Me.lblDatePaid = New System.Windows.Forms.Label
		Me.lblPODate = New System.Windows.Forms.Label
		Me.fraSalvage = New System.Windows.Forms.Panel
		Me._optSalvage_1 = New System.Windows.Forms.RadioButton
		Me._optSalvage_0 = New System.Windows.Forms.RadioButton
		Me.txtDateEntered = New System.Windows.Forms.TextBox
		Me.txtItemNumber = New System.Windows.Forms.TextBox
		Me.txtItemClaimed = New System.Windows.Forms.TextBox
		Me.txtPayeeOrSupplier = New System.Windows.Forms.TextBox
		Me.cboSettlementMethod = New System.Windows.Forms.ComboBox
		Me.txtExcess = New System.Windows.Forms.TextBox
		Me.txtPaymentAmount = New System.Windows.Forms.TextBox
		Me.txtGST = New System.Windows.Forms.TextBox
		Me.txtDepreciation = New System.Windows.Forms.TextBox
		Me.txtDepreciationPercent = New System.Windows.Forms.TextBox
		Me.txtAge = New System.Windows.Forms.TextBox
		Me.txtLife = New System.Windows.Forms.TextBox
		Me.txtStartingValue = New System.Windows.Forms.TextBox
		Me.txtItemAmount = New System.Windows.Forms.TextBox
		Me.txtDescription = New System.Windows.Forms.TextBox
		Me.txtPODate = New System.Windows.Forms.TextBox
		Me.txtDatePaid = New System.Windows.Forms.TextBox
		Me.cboStatus = New System.Windows.Forms.ComboBox
		Me.cmdItemClaimed = New System.Windows.Forms.Button
		Me.cmdPayeeOrSupplier = New System.Windows.Forms.Button
		Me._tabItemDetails_TabPage1 = New System.Windows.Forms.TabPage
		Me.tabItemDetails.SuspendLayout()
		Me._tabItemDetails_TabPage0.SuspendLayout()
		Me.fraSalvage.SuspendLayout()
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
		Me.cmdCancel.Location = New System.Drawing.Point(448, 560)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 1
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "Cancel"
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
		Me.cmdOK.Location = New System.Drawing.Point(368, 560)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 0
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "Ok"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tabItemDetails
		' 
		Me.tabItemDetails.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabItemDetails.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabItemDetails.Controls.Add(Me._tabItemDetails_TabPage0)
		Me.tabItemDetails.Controls.Add(Me._tabItemDetails_TabPage1)
		Me.tabItemDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabItemDetails.ItemSize = New System.Drawing.Size(259, 18)
		Me.tabItemDetails.Location = New System.Drawing.Point(0, 8)
		Me.tabItemDetails.Multiline = True
		Me.tabItemDetails.Name = "tabItemDetails"
		Me.tabItemDetails.Size = New System.Drawing.Size(525, 549)
		Me.tabItemDetails.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabItemDetails.TabIndex = 2
		' 
		' _tabItemDetails_TabPage0
		' 
		Me._tabItemDetails_TabPage0.Controls.Add(Me.lblDepreciationPercent)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.lblItemDescription)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.lblStartingValue)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.lblSettlementMethod)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.lblAge)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.lblPayeeOrSupplier)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.lblExcess)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.lblDateEntered)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.lblPaymentAmount)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.lblGST)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.lblItemAmount)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.lblDepreciation)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.lblItemClaimed)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.lblItemNumber)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.lblLife)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.lblStatus)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.lblSalvage)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.lblDatePaid)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.lblPODate)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.fraSalvage)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.txtDateEntered)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.txtItemNumber)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.txtItemClaimed)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.txtPayeeOrSupplier)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.cboSettlementMethod)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.txtExcess)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.txtPaymentAmount)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.txtGST)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.txtDepreciation)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.txtDepreciationPercent)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.txtAge)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.txtLife)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.txtStartingValue)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.txtItemAmount)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.txtDescription)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.txtPODate)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.txtDatePaid)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.cboStatus)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.cmdItemClaimed)
		Me._tabItemDetails_TabPage0.Controls.Add(Me.cmdPayeeOrSupplier)
		Me._tabItemDetails_TabPage0.Text = "&1 - Item Details"
		' 
		' lblDepreciationPercent
		' 
		Me.lblDepreciationPercent.AutoSize = False
		Me.lblDepreciationPercent.BackColor = System.Drawing.SystemColors.Control
		Me.lblDepreciationPercent.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDepreciationPercent.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDepreciationPercent.Enabled = True
		Me.lblDepreciationPercent.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDepreciationPercent.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDepreciationPercent.Location = New System.Drawing.Point(16, 212)
		Me.lblDepreciationPercent.Name = "lblDepreciationPercent"
		Me.lblDepreciationPercent.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDepreciationPercent.Size = New System.Drawing.Size(113, 17)
		Me.lblDepreciationPercent.TabIndex = 3
		Me.lblDepreciationPercent.Text = "Depreciation %"
		Me.lblDepreciationPercent.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblDepreciationPercent.UseMnemonic = True
		Me.lblDepreciationPercent.Visible = True
		' 
		' lblItemDescription
		' 
		Me.lblItemDescription.AutoSize = False
		Me.lblItemDescription.BackColor = System.Drawing.SystemColors.Control
		Me.lblItemDescription.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblItemDescription.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblItemDescription.Enabled = True
		Me.lblItemDescription.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblItemDescription.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblItemDescription.Location = New System.Drawing.Point(16, 87)
		Me.lblItemDescription.Name = "lblItemDescription"
		Me.lblItemDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblItemDescription.Size = New System.Drawing.Size(113, 17)
		Me.lblItemDescription.TabIndex = 4
		Me.lblItemDescription.Text = "Item Description"
		Me.lblItemDescription.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblItemDescription.UseMnemonic = True
		Me.lblItemDescription.Visible = True
		' 
		' lblStartingValue
		' 
		Me.lblStartingValue.AutoSize = False
		Me.lblStartingValue.BackColor = System.Drawing.SystemColors.Control
		Me.lblStartingValue.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblStartingValue.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblStartingValue.Enabled = True
		Me.lblStartingValue.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblStartingValue.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblStartingValue.Location = New System.Drawing.Point(16, 137)
		Me.lblStartingValue.Name = "lblStartingValue"
		Me.lblStartingValue.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblStartingValue.Size = New System.Drawing.Size(113, 17)
		Me.lblStartingValue.TabIndex = 5
		Me.lblStartingValue.Text = "Starting Value"
		Me.lblStartingValue.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblStartingValue.UseMnemonic = True
		Me.lblStartingValue.Visible = True
		' 
		' lblSettlementMethod
		' 
		Me.lblSettlementMethod.AutoSize = False
		Me.lblSettlementMethod.BackColor = System.Drawing.SystemColors.Control
		Me.lblSettlementMethod.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSettlementMethod.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSettlementMethod.Enabled = True
		Me.lblSettlementMethod.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSettlementMethod.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSettlementMethod.Location = New System.Drawing.Point(16, 112)
		Me.lblSettlementMethod.Name = "lblSettlementMethod"
		Me.lblSettlementMethod.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSettlementMethod.Size = New System.Drawing.Size(113, 17)
		Me.lblSettlementMethod.TabIndex = 6
		Me.lblSettlementMethod.Text = "Settlement Method"
		Me.lblSettlementMethod.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblSettlementMethod.UseMnemonic = True
		Me.lblSettlementMethod.Visible = True
		' 
		' lblAge
		' 
		Me.lblAge.AutoSize = False
		Me.lblAge.BackColor = System.Drawing.SystemColors.Control
		Me.lblAge.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAge.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAge.Enabled = True
		Me.lblAge.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAge.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAge.Location = New System.Drawing.Point(16, 162)
		Me.lblAge.Name = "lblAge"
		Me.lblAge.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAge.Size = New System.Drawing.Size(113, 17)
		Me.lblAge.TabIndex = 7
		Me.lblAge.Text = "Age"
		Me.lblAge.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblAge.UseMnemonic = True
		Me.lblAge.Visible = True
		' 
		' lblPayeeOrSupplier
		' 
		Me.lblPayeeOrSupplier.AutoSize = False
		Me.lblPayeeOrSupplier.BackColor = System.Drawing.SystemColors.Control
		Me.lblPayeeOrSupplier.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPayeeOrSupplier.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPayeeOrSupplier.Enabled = True
		Me.lblPayeeOrSupplier.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPayeeOrSupplier.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPayeeOrSupplier.Location = New System.Drawing.Point(16, 362)
		Me.lblPayeeOrSupplier.Name = "lblPayeeOrSupplier"
		Me.lblPayeeOrSupplier.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPayeeOrSupplier.Size = New System.Drawing.Size(113, 17)
		Me.lblPayeeOrSupplier.TabIndex = 8
		Me.lblPayeeOrSupplier.Text = "Payee Or Supplier"
		Me.lblPayeeOrSupplier.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblPayeeOrSupplier.UseMnemonic = True
		Me.lblPayeeOrSupplier.Visible = True
		' 
		' lblExcess
		' 
		Me.lblExcess.AutoSize = False
		Me.lblExcess.BackColor = System.Drawing.SystemColors.Control
		Me.lblExcess.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblExcess.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblExcess.Enabled = True
		Me.lblExcess.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblExcess.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblExcess.Location = New System.Drawing.Point(16, 337)
		Me.lblExcess.Name = "lblExcess"
		Me.lblExcess.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblExcess.Size = New System.Drawing.Size(113, 17)
		Me.lblExcess.TabIndex = 9
		Me.lblExcess.Text = "Excess"
		Me.lblExcess.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblExcess.UseMnemonic = True
		Me.lblExcess.Visible = True
		' 
		' lblDateEntered
		' 
		Me.lblDateEntered.AutoSize = False
		Me.lblDateEntered.BackColor = System.Drawing.SystemColors.Control
		Me.lblDateEntered.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDateEntered.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDateEntered.Enabled = True
		Me.lblDateEntered.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDateEntered.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDateEntered.Location = New System.Drawing.Point(16, 12)
		Me.lblDateEntered.Name = "lblDateEntered"
		Me.lblDateEntered.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDateEntered.Size = New System.Drawing.Size(113, 17)
		Me.lblDateEntered.TabIndex = 10
		Me.lblDateEntered.Text = "Date Entered"
		Me.lblDateEntered.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblDateEntered.UseMnemonic = True
		Me.lblDateEntered.Visible = True
		' 
		' lblPaymentAmount
		' 
		Me.lblPaymentAmount.AutoSize = False
		Me.lblPaymentAmount.BackColor = System.Drawing.SystemColors.Control
		Me.lblPaymentAmount.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPaymentAmount.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPaymentAmount.Enabled = True
		Me.lblPaymentAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPaymentAmount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPaymentAmount.Location = New System.Drawing.Point(16, 312)
		Me.lblPaymentAmount.Name = "lblPaymentAmount"
		Me.lblPaymentAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPaymentAmount.Size = New System.Drawing.Size(113, 17)
		Me.lblPaymentAmount.TabIndex = 11
		Me.lblPaymentAmount.Text = "Payment Amount"
		Me.lblPaymentAmount.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblPaymentAmount.UseMnemonic = True
		Me.lblPaymentAmount.Visible = True
		' 
		' lblGST
		' 
		Me.lblGST.AutoSize = False
		Me.lblGST.BackColor = System.Drawing.SystemColors.Control
		Me.lblGST.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblGST.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblGST.Enabled = True
		Me.lblGST.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblGST.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblGST.Location = New System.Drawing.Point(16, 287)
		Me.lblGST.Name = "lblGST"
		Me.lblGST.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblGST.Size = New System.Drawing.Size(113, 17)
		Me.lblGST.TabIndex = 12
		Me.lblGST.Text = "GST"
		Me.lblGST.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblGST.UseMnemonic = True
		Me.lblGST.Visible = True
		' 
		' lblItemAmount
		' 
		Me.lblItemAmount.AutoSize = False
		Me.lblItemAmount.BackColor = System.Drawing.SystemColors.Control
		Me.lblItemAmount.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblItemAmount.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblItemAmount.Enabled = True
		Me.lblItemAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblItemAmount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblItemAmount.Location = New System.Drawing.Point(16, 262)
		Me.lblItemAmount.Name = "lblItemAmount"
		Me.lblItemAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblItemAmount.Size = New System.Drawing.Size(113, 17)
		Me.lblItemAmount.TabIndex = 13
		Me.lblItemAmount.Text = "Item Amount"
		Me.lblItemAmount.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblItemAmount.UseMnemonic = True
		Me.lblItemAmount.Visible = True
		' 
		' lblDepreciation
		' 
		Me.lblDepreciation.AutoSize = False
		Me.lblDepreciation.BackColor = System.Drawing.SystemColors.Control
		Me.lblDepreciation.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDepreciation.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDepreciation.Enabled = True
		Me.lblDepreciation.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDepreciation.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDepreciation.Location = New System.Drawing.Point(16, 237)
		Me.lblDepreciation.Name = "lblDepreciation"
		Me.lblDepreciation.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDepreciation.Size = New System.Drawing.Size(113, 17)
		Me.lblDepreciation.TabIndex = 14
		Me.lblDepreciation.Text = "Depreciation"
		Me.lblDepreciation.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblDepreciation.UseMnemonic = True
		Me.lblDepreciation.Visible = True
		' 
		' lblItemClaimed
		' 
		Me.lblItemClaimed.AutoSize = False
		Me.lblItemClaimed.BackColor = System.Drawing.SystemColors.Control
		Me.lblItemClaimed.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblItemClaimed.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblItemClaimed.Enabled = True
		Me.lblItemClaimed.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblItemClaimed.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblItemClaimed.Location = New System.Drawing.Point(16, 62)
		Me.lblItemClaimed.Name = "lblItemClaimed"
		Me.lblItemClaimed.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblItemClaimed.Size = New System.Drawing.Size(113, 17)
		Me.lblItemClaimed.TabIndex = 15
		Me.lblItemClaimed.Text = "Item Claimed"
		Me.lblItemClaimed.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblItemClaimed.UseMnemonic = True
		Me.lblItemClaimed.Visible = True
		' 
		' lblItemNumber
		' 
		Me.lblItemNumber.AutoSize = False
		Me.lblItemNumber.BackColor = System.Drawing.SystemColors.Control
		Me.lblItemNumber.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblItemNumber.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblItemNumber.Enabled = True
		Me.lblItemNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblItemNumber.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblItemNumber.Location = New System.Drawing.Point(16, 37)
		Me.lblItemNumber.Name = "lblItemNumber"
		Me.lblItemNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblItemNumber.Size = New System.Drawing.Size(113, 17)
		Me.lblItemNumber.TabIndex = 16
		Me.lblItemNumber.Text = "Item Number"
		Me.lblItemNumber.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblItemNumber.UseMnemonic = True
		Me.lblItemNumber.Visible = True
		' 
		' lblLife
		' 
		Me.lblLife.AutoSize = False
		Me.lblLife.BackColor = System.Drawing.SystemColors.Control
		Me.lblLife.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblLife.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblLife.Enabled = True
		Me.lblLife.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblLife.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLife.Location = New System.Drawing.Point(16, 187)
		Me.lblLife.Name = "lblLife"
		Me.lblLife.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblLife.Size = New System.Drawing.Size(113, 17)
		Me.lblLife.TabIndex = 17
		Me.lblLife.Text = "Life"
		Me.lblLife.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblLife.UseMnemonic = True
		Me.lblLife.Visible = True
		' 
		' lblStatus
		' 
		Me.lblStatus.AutoSize = False
		Me.lblStatus.BackColor = System.Drawing.SystemColors.Control
		Me.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblStatus.Enabled = True
		Me.lblStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblStatus.Location = New System.Drawing.Point(16, 484)
		Me.lblStatus.Name = "lblStatus"
		Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblStatus.Size = New System.Drawing.Size(113, 17)
		Me.lblStatus.TabIndex = 18
		Me.lblStatus.Text = "Status"
		Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblStatus.UseMnemonic = True
		Me.lblStatus.Visible = True
		' 
		' lblSalvage
		' 
		Me.lblSalvage.AutoSize = False
		Me.lblSalvage.BackColor = System.Drawing.SystemColors.Control
		Me.lblSalvage.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSalvage.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSalvage.Enabled = True
		Me.lblSalvage.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSalvage.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSalvage.Location = New System.Drawing.Point(16, 444)
		Me.lblSalvage.Name = "lblSalvage"
		Me.lblSalvage.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSalvage.Size = New System.Drawing.Size(113, 17)
		Me.lblSalvage.TabIndex = 19
		Me.lblSalvage.Text = "Salvage"
		Me.lblSalvage.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblSalvage.UseMnemonic = True
		Me.lblSalvage.Visible = True
		' 
		' lblDatePaid
		' 
		Me.lblDatePaid.AutoSize = False
		Me.lblDatePaid.BackColor = System.Drawing.SystemColors.Control
		Me.lblDatePaid.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDatePaid.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDatePaid.Enabled = True
		Me.lblDatePaid.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDatePaid.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDatePaid.Location = New System.Drawing.Point(16, 412)
		Me.lblDatePaid.Name = "lblDatePaid"
		Me.lblDatePaid.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDatePaid.Size = New System.Drawing.Size(113, 17)
		Me.lblDatePaid.TabIndex = 20
		Me.lblDatePaid.Text = "Date Paid"
		Me.lblDatePaid.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblDatePaid.UseMnemonic = True
		Me.lblDatePaid.Visible = True
		' 
		' lblPODate
		' 
		Me.lblPODate.AutoSize = False
		Me.lblPODate.BackColor = System.Drawing.SystemColors.Control
		Me.lblPODate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPODate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPODate.Enabled = True
		Me.lblPODate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPODate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPODate.Location = New System.Drawing.Point(16, 387)
		Me.lblPODate.Name = "lblPODate"
		Me.lblPODate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPODate.Size = New System.Drawing.Size(113, 17)
		Me.lblPODate.TabIndex = 21
		Me.lblPODate.Text = "PO Date"
		Me.lblPODate.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblPODate.UseMnemonic = True
		Me.lblPODate.Visible = True
		' 
		' fraSalvage
		' 
		Me.fraSalvage.BackColor = System.Drawing.SystemColors.Control
		Me.fraSalvage.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.fraSalvage.Controls.Add(Me._optSalvage_1)
		Me.fraSalvage.Controls.Add(Me._optSalvage_0)
		Me.fraSalvage.Cursor = System.Windows.Forms.Cursors.Default
		Me.fraSalvage.Enabled = True
		Me.fraSalvage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraSalvage.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraSalvage.Location = New System.Drawing.Point(136, 429)
		Me.fraSalvage.Name = "fraSalvage"
		Me.fraSalvage.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraSalvage.Size = New System.Drawing.Size(97, 49)
		Me.fraSalvage.TabIndex = 22
		Me.fraSalvage.Text = "Frame1"
		Me.fraSalvage.Visible = True
		' 
		' _optSalvage_1
		' 
		Me._optSalvage_1.Appearance = System.Windows.Forms.Appearance.Normal
		Me._optSalvage_1.BackColor = System.Drawing.SystemColors.Control
		Me._optSalvage_1.CausesValidation = True
		Me._optSalvage_1.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optSalvage_1.Checked = False
		Me._optSalvage_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._optSalvage_1.Enabled = True
		Me._optSalvage_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._optSalvage_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._optSalvage_1.Location = New System.Drawing.Point(16, 24)
		Me._optSalvage_1.Name = "_optSalvage_1"
		Me._optSalvage_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optSalvage_1.Size = New System.Drawing.Size(89, 33)
		Me._optSalvage_1.TabIndex = 44
		Me._optSalvage_1.TabStop = True
		Me._optSalvage_1.Text = "Yes"
		Me._optSalvage_1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optSalvage_1.Visible = True
		' 
		' _optSalvage_0
		' 
		Me._optSalvage_0.Appearance = System.Windows.Forms.Appearance.Normal
		Me._optSalvage_0.BackColor = System.Drawing.SystemColors.Control
		Me._optSalvage_0.CausesValidation = True
		Me._optSalvage_0.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optSalvage_0.Checked = False
		Me._optSalvage_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._optSalvage_0.Enabled = True
		Me._optSalvage_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._optSalvage_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me._optSalvage_0.Location = New System.Drawing.Point(16, 0)
		Me._optSalvage_0.Name = "_optSalvage_0"
		Me._optSalvage_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optSalvage_0.Size = New System.Drawing.Size(89, 33)
		Me._optSalvage_0.TabIndex = 23
		Me._optSalvage_0.TabStop = True
		Me._optSalvage_0.Text = "No"
		Me._optSalvage_0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optSalvage_0.Visible = True
		' 
		' txtDateEntered
		' 
		Me.txtDateEntered.AcceptsReturn = True
		Me.txtDateEntered.AutoSize = False
		Me.txtDateEntered.BackColor = System.Drawing.SystemColors.Window
		Me.txtDateEntered.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDateEntered.CausesValidation = True
		Me.txtDateEntered.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDateEntered.Enabled = True
		Me.txtDateEntered.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDateEntered.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDateEntered.HideSelection = True
		Me.txtDateEntered.Location = New System.Drawing.Point(152, 12)
		Me.txtDateEntered.MaxLength = 0
		Me.txtDateEntered.Multiline = False
		Me.txtDateEntered.Name = "txtDateEntered"
		Me.txtDateEntered.ReadOnly = False
		Me.txtDateEntered.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDateEntered.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDateEntered.Size = New System.Drawing.Size(113, 19)
		Me.txtDateEntered.TabIndex = 24
		Me.txtDateEntered.TabStop = True
		Me.txtDateEntered.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDateEntered.Visible = True
		' 
		' txtItemNumber
		' 
		Me.txtItemNumber.AcceptsReturn = True
		Me.txtItemNumber.AutoSize = False
		Me.txtItemNumber.BackColor = System.Drawing.SystemColors.Window
		Me.txtItemNumber.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtItemNumber.CausesValidation = True
		Me.txtItemNumber.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtItemNumber.Enabled = True
		Me.txtItemNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtItemNumber.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtItemNumber.HideSelection = True
		Me.txtItemNumber.Location = New System.Drawing.Point(152, 36)
		Me.txtItemNumber.MaxLength = 0
		Me.txtItemNumber.Multiline = False
		Me.txtItemNumber.Name = "txtItemNumber"
		Me.txtItemNumber.ReadOnly = False
		Me.txtItemNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtItemNumber.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtItemNumber.Size = New System.Drawing.Size(113, 19)
		Me.txtItemNumber.TabIndex = 25
		Me.txtItemNumber.TabStop = True
		Me.txtItemNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtItemNumber.Visible = True
		' 
		' txtItemClaimed
		' 
		Me.txtItemClaimed.AcceptsReturn = True
		Me.txtItemClaimed.AutoSize = False
		Me.txtItemClaimed.BackColor = System.Drawing.SystemColors.Window
		Me.txtItemClaimed.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtItemClaimed.CausesValidation = True
		Me.txtItemClaimed.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtItemClaimed.Enabled = False
		Me.txtItemClaimed.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtItemClaimed.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtItemClaimed.HideSelection = True
		Me.txtItemClaimed.Location = New System.Drawing.Point(152, 62)
		Me.txtItemClaimed.MaxLength = 0
		Me.txtItemClaimed.Multiline = False
		Me.txtItemClaimed.Name = "txtItemClaimed"
		Me.txtItemClaimed.ReadOnly = False
		Me.txtItemClaimed.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtItemClaimed.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtItemClaimed.Size = New System.Drawing.Size(305, 19)
		Me.txtItemClaimed.TabIndex = 26
		Me.txtItemClaimed.TabStop = True
		Me.txtItemClaimed.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtItemClaimed.Visible = True
		' 
		' txtPayeeOrSupplier
		' 
		Me.txtPayeeOrSupplier.AcceptsReturn = True
		Me.txtPayeeOrSupplier.AutoSize = False
		Me.txtPayeeOrSupplier.BackColor = System.Drawing.SystemColors.Window
		Me.txtPayeeOrSupplier.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPayeeOrSupplier.CausesValidation = True
		Me.txtPayeeOrSupplier.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPayeeOrSupplier.Enabled = False
		Me.txtPayeeOrSupplier.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPayeeOrSupplier.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPayeeOrSupplier.HideSelection = True
		Me.txtPayeeOrSupplier.Location = New System.Drawing.Point(152, 362)
		Me.txtPayeeOrSupplier.MaxLength = 0
		Me.txtPayeeOrSupplier.Multiline = False
		Me.txtPayeeOrSupplier.Name = "txtPayeeOrSupplier"
		Me.txtPayeeOrSupplier.ReadOnly = False
		Me.txtPayeeOrSupplier.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPayeeOrSupplier.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPayeeOrSupplier.Size = New System.Drawing.Size(305, 19)
		Me.txtPayeeOrSupplier.TabIndex = 27
		Me.txtPayeeOrSupplier.TabStop = True
		Me.txtPayeeOrSupplier.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPayeeOrSupplier.Visible = True
		' 
		' cboSettlementMethod
		' 
		Me.cboSettlementMethod.BackColor = System.Drawing.SystemColors.Window
		Me.cboSettlementMethod.CausesValidation = True
		Me.cboSettlementMethod.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboSettlementMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown
		Me.cboSettlementMethod.Enabled = True
		Me.cboSettlementMethod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboSettlementMethod.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboSettlementMethod.IntegralHeight = True
		Me.cboSettlementMethod.Location = New System.Drawing.Point(152, 112)
		Me.cboSettlementMethod.Name = "cboSettlementMethod"
		Me.cboSettlementMethod.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboSettlementMethod.Size = New System.Drawing.Size(137, 21)
		Me.cboSettlementMethod.Sorted = False
		Me.cboSettlementMethod.TabIndex = 28
		Me.cboSettlementMethod.TabStop = True
		Me.cboSettlementMethod.Visible = True
		' 
		' txtExcess
		' 
		Me.txtExcess.AcceptsReturn = True
		Me.txtExcess.AutoSize = False
		Me.txtExcess.BackColor = System.Drawing.SystemColors.Window
		Me.txtExcess.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtExcess.CausesValidation = True
		Me.txtExcess.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtExcess.Enabled = True
		Me.txtExcess.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtExcess.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtExcess.HideSelection = True
		Me.txtExcess.Location = New System.Drawing.Point(152, 337)
		Me.txtExcess.MaxLength = 0
		Me.txtExcess.Multiline = False
		Me.txtExcess.Name = "txtExcess"
		Me.txtExcess.ReadOnly = False
		Me.txtExcess.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtExcess.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtExcess.Size = New System.Drawing.Size(113, 19)
		Me.txtExcess.TabIndex = 29
		Me.txtExcess.TabStop = True
		Me.txtExcess.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtExcess.Visible = True
		' 
		' txtPaymentAmount
		' 
		Me.txtPaymentAmount.AcceptsReturn = True
		Me.txtPaymentAmount.AutoSize = False
		Me.txtPaymentAmount.BackColor = System.Drawing.SystemColors.Window
		Me.txtPaymentAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPaymentAmount.CausesValidation = True
		Me.txtPaymentAmount.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPaymentAmount.Enabled = True
		Me.txtPaymentAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPaymentAmount.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPaymentAmount.HideSelection = True
		Me.txtPaymentAmount.Location = New System.Drawing.Point(152, 312)
		Me.txtPaymentAmount.MaxLength = 0
		Me.txtPaymentAmount.Multiline = False
		Me.txtPaymentAmount.Name = "txtPaymentAmount"
		Me.txtPaymentAmount.ReadOnly = False
		Me.txtPaymentAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPaymentAmount.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPaymentAmount.Size = New System.Drawing.Size(113, 19)
		Me.txtPaymentAmount.TabIndex = 30
		Me.txtPaymentAmount.TabStop = True
		Me.txtPaymentAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPaymentAmount.Visible = True
		' 
		' txtGST
		' 
		Me.txtGST.AcceptsReturn = True
		Me.txtGST.AutoSize = False
		Me.txtGST.BackColor = System.Drawing.SystemColors.Window
		Me.txtGST.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtGST.CausesValidation = True
		Me.txtGST.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtGST.Enabled = True
		Me.txtGST.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtGST.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtGST.HideSelection = True
		Me.txtGST.Location = New System.Drawing.Point(152, 287)
		Me.txtGST.MaxLength = 0
		Me.txtGST.Multiline = False
		Me.txtGST.Name = "txtGST"
		Me.txtGST.ReadOnly = False
		Me.txtGST.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtGST.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtGST.Size = New System.Drawing.Size(113, 19)
		Me.txtGST.TabIndex = 31
		Me.txtGST.TabStop = True
		Me.txtGST.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtGST.Visible = True
		' 
		' txtDepreciation
		' 
		Me.txtDepreciation.AcceptsReturn = True
		Me.txtDepreciation.AutoSize = False
		Me.txtDepreciation.BackColor = System.Drawing.SystemColors.Window
		Me.txtDepreciation.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDepreciation.CausesValidation = True
		Me.txtDepreciation.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDepreciation.Enabled = True
		Me.txtDepreciation.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDepreciation.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDepreciation.HideSelection = True
		Me.txtDepreciation.Location = New System.Drawing.Point(152, 237)
		Me.txtDepreciation.MaxLength = 0
		Me.txtDepreciation.Multiline = False
		Me.txtDepreciation.Name = "txtDepreciation"
		Me.txtDepreciation.ReadOnly = False
		Me.txtDepreciation.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDepreciation.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDepreciation.Size = New System.Drawing.Size(113, 19)
		Me.txtDepreciation.TabIndex = 32
		Me.txtDepreciation.TabStop = True
		Me.txtDepreciation.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDepreciation.Visible = True
		' 
		' txtDepreciationPercent
		' 
		Me.txtDepreciationPercent.AcceptsReturn = True
		Me.txtDepreciationPercent.AutoSize = False
		Me.txtDepreciationPercent.BackColor = System.Drawing.SystemColors.Window
		Me.txtDepreciationPercent.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDepreciationPercent.CausesValidation = True
		Me.txtDepreciationPercent.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDepreciationPercent.Enabled = True
		Me.txtDepreciationPercent.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDepreciationPercent.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDepreciationPercent.HideSelection = True
		Me.txtDepreciationPercent.Location = New System.Drawing.Point(152, 212)
		Me.txtDepreciationPercent.MaxLength = 0
		Me.txtDepreciationPercent.Multiline = False
		Me.txtDepreciationPercent.Name = "txtDepreciationPercent"
		Me.txtDepreciationPercent.ReadOnly = False
		Me.txtDepreciationPercent.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDepreciationPercent.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDepreciationPercent.Size = New System.Drawing.Size(113, 19)
		Me.txtDepreciationPercent.TabIndex = 33
		Me.txtDepreciationPercent.TabStop = True
		Me.txtDepreciationPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDepreciationPercent.Visible = True
		' 
		' txtAge
		' 
		Me.txtAge.AcceptsReturn = True
		Me.txtAge.AutoSize = False
		Me.txtAge.BackColor = System.Drawing.SystemColors.Window
		Me.txtAge.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtAge.CausesValidation = True
		Me.txtAge.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAge.Enabled = True
		Me.txtAge.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtAge.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAge.HideSelection = True
		Me.txtAge.Location = New System.Drawing.Point(152, 162)
		Me.txtAge.MaxLength = 0
		Me.txtAge.Multiline = False
		Me.txtAge.Name = "txtAge"
		Me.txtAge.ReadOnly = False
		Me.txtAge.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAge.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAge.Size = New System.Drawing.Size(113, 19)
		Me.txtAge.TabIndex = 34
		Me.txtAge.TabStop = True
		Me.txtAge.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtAge.Visible = True
		' 
		' txtLife
		' 
		Me.txtLife.AcceptsReturn = True
		Me.txtLife.AutoSize = False
		Me.txtLife.BackColor = System.Drawing.SystemColors.Window
		Me.txtLife.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtLife.CausesValidation = True
		Me.txtLife.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtLife.Enabled = True
		Me.txtLife.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtLife.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtLife.HideSelection = True
		Me.txtLife.Location = New System.Drawing.Point(152, 187)
		Me.txtLife.MaxLength = 0
		Me.txtLife.Multiline = False
		Me.txtLife.Name = "txtLife"
		Me.txtLife.ReadOnly = False
		Me.txtLife.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtLife.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtLife.Size = New System.Drawing.Size(113, 19)
		Me.txtLife.TabIndex = 35
		Me.txtLife.TabStop = True
		Me.txtLife.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtLife.Visible = True
		' 
		' txtStartingValue
		' 
		Me.txtStartingValue.AcceptsReturn = True
		Me.txtStartingValue.AutoSize = False
		Me.txtStartingValue.BackColor = System.Drawing.SystemColors.Window
		Me.txtStartingValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtStartingValue.CausesValidation = True
		Me.txtStartingValue.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtStartingValue.Enabled = True
		Me.txtStartingValue.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtStartingValue.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtStartingValue.HideSelection = True
		Me.txtStartingValue.Location = New System.Drawing.Point(152, 137)
		Me.txtStartingValue.MaxLength = 0
		Me.txtStartingValue.Multiline = False
		Me.txtStartingValue.Name = "txtStartingValue"
		Me.txtStartingValue.ReadOnly = False
		Me.txtStartingValue.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtStartingValue.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtStartingValue.Size = New System.Drawing.Size(113, 19)
		Me.txtStartingValue.TabIndex = 36
		Me.txtStartingValue.TabStop = True
		Me.txtStartingValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtStartingValue.Visible = True
		' 
		' txtItemAmount
		' 
		Me.txtItemAmount.AcceptsReturn = True
		Me.txtItemAmount.AutoSize = False
		Me.txtItemAmount.BackColor = System.Drawing.SystemColors.Window
		Me.txtItemAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtItemAmount.CausesValidation = True
		Me.txtItemAmount.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtItemAmount.Enabled = True
		Me.txtItemAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtItemAmount.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtItemAmount.HideSelection = True
		Me.txtItemAmount.Location = New System.Drawing.Point(152, 262)
		Me.txtItemAmount.MaxLength = 0
		Me.txtItemAmount.Multiline = False
		Me.txtItemAmount.Name = "txtItemAmount"
		Me.txtItemAmount.ReadOnly = False
		Me.txtItemAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtItemAmount.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtItemAmount.Size = New System.Drawing.Size(113, 19)
		Me.txtItemAmount.TabIndex = 37
		Me.txtItemAmount.TabStop = True
		Me.txtItemAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtItemAmount.Visible = True
		' 
		' txtDescription
		' 
		Me.txtDescription.AcceptsReturn = True
		Me.txtDescription.AutoSize = False
		Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
		Me.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDescription.CausesValidation = True
		Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDescription.Enabled = True
		Me.txtDescription.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDescription.HideSelection = True
		Me.txtDescription.Location = New System.Drawing.Point(152, 87)
		Me.txtDescription.MaxLength = 0
		Me.txtDescription.Multiline = False
		Me.txtDescription.Name = "txtDescription"
		Me.txtDescription.ReadOnly = False
		Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDescription.Size = New System.Drawing.Size(305, 19)
		Me.txtDescription.TabIndex = 38
		Me.txtDescription.TabStop = True
		Me.txtDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDescription.Visible = True
		' 
		' txtPODate
		' 
		Me.txtPODate.AcceptsReturn = True
		Me.txtPODate.AutoSize = False
		Me.txtPODate.BackColor = System.Drawing.SystemColors.Window
		Me.txtPODate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPODate.CausesValidation = True
		Me.txtPODate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPODate.Enabled = True
		Me.txtPODate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPODate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPODate.HideSelection = True
		Me.txtPODate.Location = New System.Drawing.Point(152, 387)
		Me.txtPODate.MaxLength = 0
		Me.txtPODate.Multiline = False
		Me.txtPODate.Name = "txtPODate"
		Me.txtPODate.ReadOnly = False
		Me.txtPODate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPODate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPODate.Size = New System.Drawing.Size(113, 19)
		Me.txtPODate.TabIndex = 39
		Me.txtPODate.TabStop = True
		Me.txtPODate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPODate.Visible = True
		' 
		' txtDatePaid
		' 
		Me.txtDatePaid.AcceptsReturn = True
		Me.txtDatePaid.AutoSize = False
		Me.txtDatePaid.BackColor = System.Drawing.SystemColors.Window
		Me.txtDatePaid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDatePaid.CausesValidation = True
		Me.txtDatePaid.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDatePaid.Enabled = True
		Me.txtDatePaid.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDatePaid.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDatePaid.HideSelection = True
		Me.txtDatePaid.Location = New System.Drawing.Point(152, 412)
		Me.txtDatePaid.MaxLength = 0
		Me.txtDatePaid.Multiline = False
		Me.txtDatePaid.Name = "txtDatePaid"
		Me.txtDatePaid.ReadOnly = False
		Me.txtDatePaid.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDatePaid.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDatePaid.Size = New System.Drawing.Size(113, 19)
		Me.txtDatePaid.TabIndex = 40
		Me.txtDatePaid.TabStop = True
		Me.txtDatePaid.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDatePaid.Visible = True
		' 
		' cboStatus
		' 
		Me.cboStatus.BackColor = System.Drawing.SystemColors.Window
		Me.cboStatus.CausesValidation = True
		Me.cboStatus.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown
		Me.cboStatus.Enabled = True
		Me.cboStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboStatus.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboStatus.IntegralHeight = True
		Me.cboStatus.Location = New System.Drawing.Point(152, 484)
		Me.cboStatus.Name = "cboStatus"
		Me.cboStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboStatus.Size = New System.Drawing.Size(137, 21)
		Me.cboStatus.Sorted = False
		Me.cboStatus.TabIndex = 41
		Me.cboStatus.TabStop = True
		Me.cboStatus.Visible = True
		' 
		' cmdItemClaimed
		' 
		Me.cmdItemClaimed.BackColor = System.Drawing.SystemColors.Control
		Me.cmdItemClaimed.CausesValidation = True
		Me.cmdItemClaimed.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdItemClaimed.Enabled = True
		Me.cmdItemClaimed.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdItemClaimed.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdItemClaimed.Location = New System.Drawing.Point(464, 62)
		Me.cmdItemClaimed.Name = "cmdItemClaimed"
		Me.cmdItemClaimed.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdItemClaimed.Size = New System.Drawing.Size(25, 19)
		Me.cmdItemClaimed.TabIndex = 42
		Me.cmdItemClaimed.TabStop = True
		Me.cmdItemClaimed.Text = "..."
		Me.cmdItemClaimed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdItemClaimed.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdPayeeOrSupplier
		' 
		Me.cmdPayeeOrSupplier.BackColor = System.Drawing.SystemColors.Control
		Me.cmdPayeeOrSupplier.CausesValidation = True
		Me.cmdPayeeOrSupplier.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdPayeeOrSupplier.Enabled = True
		Me.cmdPayeeOrSupplier.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdPayeeOrSupplier.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdPayeeOrSupplier.Location = New System.Drawing.Point(464, 362)
		Me.cmdPayeeOrSupplier.Name = "cmdPayeeOrSupplier"
		Me.cmdPayeeOrSupplier.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdPayeeOrSupplier.Size = New System.Drawing.Size(25, 19)
		Me.cmdPayeeOrSupplier.TabIndex = 43
		Me.cmdPayeeOrSupplier.TabStop = True
		Me.cmdPayeeOrSupplier.Text = "..."
		Me.cmdPayeeOrSupplier.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdPayeeOrSupplier.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' _tabItemDetails_TabPage1
		' 
		Me._tabItemDetails_TabPage1.Text = "Tab 1"
		' 
		' frmAdd
		' 
		Me.AcceptButton = Me.cmdCancel
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(524, 585)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.tabItemDetails)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmAdd"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "Item Details"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabItemDetails, 2)
		Me.tabItemDetails.ResumeLayout(False)
		Me._tabItemDetails_TabPage0.ResumeLayout(False)
		Me.fraSalvage.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub InitializeoptSalvage()
		Me.optSalvage(1) = _optSalvage_1
		Me.optSalvage(0) = _optSalvage_0
	End Sub
#End Region 
End Class