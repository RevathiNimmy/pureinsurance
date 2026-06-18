Imports PMLookupControl

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctCreditCard
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		UserControl_Initialize()
	End Sub
    'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Friend WithEvents txtCSVPIN As System.Windows.Forms.TextBox
	Friend WithEvents txtNameOnCard As System.Windows.Forms.TextBox
	Friend WithEvents txtStartDate As System.Windows.Forms.TextBox
	Friend WithEvents txtManualAuth As System.Windows.Forms.TextBox
	Friend WithEvents txtTrackingNumber As System.Windows.Forms.TextBox
	Friend WithEvents txtCardTransSlipNo As System.Windows.Forms.TextBox
	Friend WithEvents txtStatus As System.Windows.Forms.TextBox
	Friend WithEvents cboCustomer As System.Windows.Forms.ComboBox
	Friend WithEvents txtAutoAuthCode As System.Windows.Forms.TextBox
	Friend WithEvents txtIssueNumber As System.Windows.Forms.TextBox
	Friend WithEvents txtExpiryDate As System.Windows.Forms.TextBox
	Friend WithEvents txtCardNumber As System.Windows.Forms.TextBox
	Friend WithEvents cboCardNumber As System.Windows.Forms.ComboBox
	Friend WithEvents cboCardType As cboPMLookup
	Friend WithEvents cboCCBank As cboPMLookup
	Friend WithEvents lblCCBank As System.Windows.Forms.Label
	Friend WithEvents lblCardType As System.Windows.Forms.Label
	Friend WithEvents lblCardTransSlipNo As System.Windows.Forms.Label
	Friend WithEvents lblTrackingNumber As System.Windows.Forms.Label
	Friend WithEvents lblCustomer As System.Windows.Forms.Label
	Friend WithEvents lblManualAuth As System.Windows.Forms.Label
	Friend WithEvents lblCSVPIN As System.Windows.Forms.Label
	Friend WithEvents lblAutoAuthCode As System.Windows.Forms.Label
	Friend WithEvents lblStartDate As System.Windows.Forms.Label
	Friend WithEvents lblNameOnCard As System.Windows.Forms.Label
	Friend WithEvents lblIssueNumber As System.Windows.Forms.Label
	Friend WithEvents lblExpiryDate As System.Windows.Forms.Label
	Friend WithEvents lblCardNumber As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtCSVPIN = New System.Windows.Forms.TextBox()
        Me.txtNameOnCard = New System.Windows.Forms.TextBox()
        Me.txtStartDate = New System.Windows.Forms.TextBox()
        Me.txtManualAuth = New System.Windows.Forms.TextBox()
        Me.txtTrackingNumber = New System.Windows.Forms.TextBox()
        Me.txtCardTransSlipNo = New System.Windows.Forms.TextBox()
        Me.txtStatus = New System.Windows.Forms.TextBox()
        Me.cboCustomer = New System.Windows.Forms.ComboBox()
        Me.txtAutoAuthCode = New System.Windows.Forms.TextBox()
        Me.txtIssueNumber = New System.Windows.Forms.TextBox()
        Me.txtExpiryDate = New System.Windows.Forms.TextBox()
        Me.txtCardNumber = New System.Windows.Forms.TextBox()
        Me.cboCardNumber = New System.Windows.Forms.ComboBox()
        Me.cboCardType = New PMLookupControl.cboPMLookup()
        Me.cboCCBank = New PMLookupControl.cboPMLookup()
        Me.lblCCBank = New System.Windows.Forms.Label()
        Me.lblCardType = New System.Windows.Forms.Label()
        Me.lblCardTransSlipNo = New System.Windows.Forms.Label()
        Me.lblTrackingNumber = New System.Windows.Forms.Label()
        Me.lblCustomer = New System.Windows.Forms.Label()
        Me.lblManualAuth = New System.Windows.Forms.Label()
        Me.lblCSVPIN = New System.Windows.Forms.Label()
        Me.lblAutoAuthCode = New System.Windows.Forms.Label()
        Me.lblStartDate = New System.Windows.Forms.Label()
        Me.lblNameOnCard = New System.Windows.Forms.Label()
        Me.lblIssueNumber = New System.Windows.Forms.Label()
        Me.lblExpiryDate = New System.Windows.Forms.Label()
        Me.lblCardNumber = New System.Windows.Forms.Label()
        Me.chkIsDefault = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'txtCSVPIN
        '
        Me.txtCSVPIN.AcceptsReturn = True
        Me.txtCSVPIN.BackColor = System.Drawing.SystemColors.Window
        Me.txtCSVPIN.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCSVPIN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCSVPIN.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCSVPIN.Location = New System.Drawing.Point(444, 75)
        Me.txtCSVPIN.MaxLength = 20
        Me.txtCSVPIN.Name = "txtCSVPIN"
        Me.txtCSVPIN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCSVPIN.Size = New System.Drawing.Size(57, 21)
        Me.txtCSVPIN.TabIndex = 5
        '
        'txtNameOnCard
        '
        Me.txtNameOnCard.AcceptsReturn = True
        Me.txtNameOnCard.BackColor = System.Drawing.SystemColors.Window
        Me.txtNameOnCard.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNameOnCard.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNameOnCard.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNameOnCard.Location = New System.Drawing.Point(444, 23)
        Me.txtNameOnCard.MaxLength = 50
        Me.txtNameOnCard.Name = "txtNameOnCard"
        Me.txtNameOnCard.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNameOnCard.Size = New System.Drawing.Size(145, 21)
        Me.txtNameOnCard.TabIndex = 1
        '
        'txtStartDate
        '
        Me.txtStartDate.AcceptsReturn = True
        Me.txtStartDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtStartDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStartDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStartDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStartDate.Location = New System.Drawing.Point(444, 49)
        Me.txtStartDate.MaxLength = 5
        Me.txtStartDate.Name = "txtStartDate"
        Me.txtStartDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStartDate.Size = New System.Drawing.Size(57, 21)
        Me.txtStartDate.TabIndex = 3
        '
        'txtManualAuth
        '
        Me.txtManualAuth.AcceptsReturn = True
        Me.txtManualAuth.BackColor = System.Drawing.SystemColors.Window
        Me.txtManualAuth.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtManualAuth.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtManualAuth.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtManualAuth.Location = New System.Drawing.Point(444, 100)
        Me.txtManualAuth.MaxLength = 20
        Me.txtManualAuth.Name = "txtManualAuth"
        Me.txtManualAuth.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtManualAuth.Size = New System.Drawing.Size(97, 21)
        Me.txtManualAuth.TabIndex = 7
        '
        'txtTrackingNumber
        '
        Me.txtTrackingNumber.AcceptsReturn = True
        Me.txtTrackingNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtTrackingNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTrackingNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTrackingNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTrackingNumber.Location = New System.Drawing.Point(444, 180)
        Me.txtTrackingNumber.MaxLength = 50
        Me.txtTrackingNumber.Name = "txtTrackingNumber"
        Me.txtTrackingNumber.ReadOnly = True
        Me.txtTrackingNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTrackingNumber.Size = New System.Drawing.Size(121, 21)
        Me.txtTrackingNumber.TabIndex = 13
        '
        'txtCardTransSlipNo
        '
        Me.txtCardTransSlipNo.AcceptsReturn = True
        Me.txtCardTransSlipNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtCardTransSlipNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCardTransSlipNo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCardTransSlipNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCardTransSlipNo.Location = New System.Drawing.Point(140, 177)
        Me.txtCardTransSlipNo.MaxLength = 50
        Me.txtCardTransSlipNo.Name = "txtCardTransSlipNo"
        Me.txtCardTransSlipNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCardTransSlipNo.Size = New System.Drawing.Size(145, 21)
        Me.txtCardTransSlipNo.TabIndex = 12
        '
        'txtStatus
        '
        Me.txtStatus.AcceptsReturn = True
        Me.txtStatus.BackColor = System.Drawing.SystemColors.Control
        Me.txtStatus.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStatus.Location = New System.Drawing.Point(298, 126)
        Me.txtStatus.MaxLength = 0
        Me.txtStatus.Name = "txtStatus"
        Me.txtStatus.ReadOnly = True
        Me.txtStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStatus.Size = New System.Drawing.Size(267, 21)
        Me.txtStatus.TabIndex = 9
        Me.txtStatus.TabStop = False
        Me.txtStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cboCustomer
        '
        Me.cboCustomer.BackColor = System.Drawing.SystemColors.Window
        Me.cboCustomer.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCustomer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCustomer.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCustomer.Location = New System.Drawing.Point(140, 125)
        Me.cboCustomer.Name = "cboCustomer"
        Me.cboCustomer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCustomer.Size = New System.Drawing.Size(145, 21)
        Me.cboCustomer.TabIndex = 8
        '
        'txtAutoAuthCode
        '
        Me.txtAutoAuthCode.AcceptsReturn = True
        Me.txtAutoAuthCode.BackColor = System.Drawing.SystemColors.Control
        Me.txtAutoAuthCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAutoAuthCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAutoAuthCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAutoAuthCode.Location = New System.Drawing.Point(140, 100)
        Me.txtAutoAuthCode.MaxLength = 0
        Me.txtAutoAuthCode.Name = "txtAutoAuthCode"
        Me.txtAutoAuthCode.ReadOnly = True
        Me.txtAutoAuthCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAutoAuthCode.Size = New System.Drawing.Size(97, 21)
        Me.txtAutoAuthCode.TabIndex = 6
        Me.txtAutoAuthCode.TabStop = False
        '
        'txtIssueNumber
        '
        Me.txtIssueNumber.AcceptsReturn = True
        Me.txtIssueNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtIssueNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtIssueNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIssueNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIssueNumber.Location = New System.Drawing.Point(140, 75)
        Me.txtIssueNumber.MaxLength = 2
        Me.txtIssueNumber.Name = "txtIssueNumber"
        Me.txtIssueNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtIssueNumber.Size = New System.Drawing.Size(57, 21)
        Me.txtIssueNumber.TabIndex = 4
        '
        'txtExpiryDate
        '
        Me.txtExpiryDate.AcceptsReturn = True
        Me.txtExpiryDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtExpiryDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtExpiryDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExpiryDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtExpiryDate.Location = New System.Drawing.Point(140, 49)
        Me.txtExpiryDate.MaxLength = 5
        Me.txtExpiryDate.Name = "txtExpiryDate"
        Me.txtExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtExpiryDate.Size = New System.Drawing.Size(57, 21)
        Me.txtExpiryDate.TabIndex = 2
        '
        'txtCardNumber
        '
        Me.txtCardNumber.AcceptsReturn = True
        Me.txtCardNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtCardNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCardNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCardNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCardNumber.Location = New System.Drawing.Point(140, 24)
        Me.txtCardNumber.MaxLength = 30
        Me.txtCardNumber.Name = "txtCardNumber"
        Me.txtCardNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCardNumber.Size = New System.Drawing.Size(121, 21)
        Me.txtCardNumber.TabIndex = 0
        '
        'cboCardNumber
        '
        Me.cboCardNumber.BackColor = System.Drawing.SystemColors.Window
        Me.cboCardNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCardNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCardNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCardNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCardNumber.Location = New System.Drawing.Point(140, 23)
        Me.cboCardNumber.Name = "cboCardNumber"
        Me.cboCardNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCardNumber.Size = New System.Drawing.Size(121, 21)
        Me.cboCardNumber.TabIndex = 1
        '
        'cboCardType
        '
        Me.cboCardType.DefaultItemId = 0
        Me.cboCardType.FirstItem = ""
        Me.cboCardType.ItemId = 0
        Me.cboCardType.ListIndex = -1
        Me.cboCardType.Location = New System.Drawing.Point(140, 151)
        Me.cboCardType.Name = "cboCardType"
        Me.cboCardType.PMLookupProductFamily = 1
        Me.cboCardType.SingleItemId = 0
        Me.cboCardType.Size = New System.Drawing.Size(145, 21)
        Me.cboCardType.SortColumnName = ""
        Me.cboCardType.Sorted = True
        Me.cboCardType.TabIndex = 10
        Me.cboCardType.TableName = "Type_Of_Card"
        Me.cboCardType.ToolTipText = ""
        Me.cboCardType.WhereClause = "is_deleted<>1 and effective_date < = getdate()"
        '
        'cboCCBank
        '
        Me.cboCCBank.DefaultItemId = 0
        Me.cboCCBank.FirstItem = ""
        Me.cboCCBank.ItemId = 0
        Me.cboCCBank.ListIndex = -1
        Me.cboCCBank.Location = New System.Drawing.Point(444, 151)
        Me.cboCCBank.Name = "cboCCBank"
        Me.cboCCBank.PMLookupProductFamily = 1
        Me.cboCCBank.SingleItemId = 0
        Me.cboCCBank.Size = New System.Drawing.Size(125, 21)
        Me.cboCCBank.SortColumnName = ""
        Me.cboCCBank.Sorted = True
        Me.cboCCBank.TabIndex = 11
        Me.cboCCBank.TableName = "CashListItem_Bank"
        Me.cboCCBank.ToolTipText = ""
        Me.cboCCBank.WhereClause = "is_deleted<>1 and effective_date < = getdate()"
        '
        'lblCCBank
        '
        Me.lblCCBank.BackColor = System.Drawing.SystemColors.Control
        Me.lblCCBank.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCCBank.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCCBank.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCCBank.Location = New System.Drawing.Point(300, 153)
        Me.lblCCBank.Name = "lblCCBank"
        Me.lblCCBank.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCCBank.Size = New System.Drawing.Size(153, 25)
        Me.lblCCBank.TabIndex = 21
        Me.lblCCBank.Text = "Card Issuing Bank Name:"
        '
        'lblCardType
        '
        Me.lblCardType.BackColor = System.Drawing.SystemColors.Control
        Me.lblCardType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCardType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCardType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCardType.Location = New System.Drawing.Point(8, 152)
        Me.lblCardType.Name = "lblCardType"
        Me.lblCardType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCardType.Size = New System.Drawing.Size(129, 20)
        Me.lblCardType.TabIndex = 20
        Me.lblCardType.Text = "Type of Card:"
        '
        'lblCardTransSlipNo
        '
        Me.lblCardTransSlipNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblCardTransSlipNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCardTransSlipNo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCardTransSlipNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCardTransSlipNo.Location = New System.Drawing.Point(8, 172)
        Me.lblCardTransSlipNo.Name = "lblCardTransSlipNo"
        Me.lblCardTransSlipNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCardTransSlipNo.Size = New System.Drawing.Size(129, 28)
        Me.lblCardTransSlipNo.TabIndex = 19
        Me.lblCardTransSlipNo.Text = "Transaction Slip Number:"
        '
        'lblTrackingNumber
        '
        Me.lblTrackingNumber.AutoSize = True
        Me.lblTrackingNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblTrackingNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTrackingNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTrackingNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTrackingNumber.Location = New System.Drawing.Point(300, 180)
        Me.lblTrackingNumber.Name = "lblTrackingNumber"
        Me.lblTrackingNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTrackingNumber.Size = New System.Drawing.Size(110, 13)
        Me.lblTrackingNumber.TabIndex = 16
        Me.lblTrackingNumber.Text = "Tracking Number:"
        '
        'lblCustomer
        '
        Me.lblCustomer.BackColor = System.Drawing.SystemColors.Control
        Me.lblCustomer.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCustomer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCustomer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCustomer.Location = New System.Drawing.Point(8, 125)
        Me.lblCustomer.Name = "lblCustomer"
        Me.lblCustomer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCustomer.Size = New System.Drawing.Size(129, 20)
        Me.lblCustomer.TabIndex = 13
        Me.lblCustomer.Text = "Customer:"
        '
        'lblManualAuth
        '
        Me.lblManualAuth.BackColor = System.Drawing.SystemColors.Control
        Me.lblManualAuth.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblManualAuth.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblManualAuth.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblManualAuth.Location = New System.Drawing.Point(300, 101)
        Me.lblManualAuth.Name = "lblManualAuth"
        Me.lblManualAuth.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblManualAuth.Size = New System.Drawing.Size(129, 17)
        Me.lblManualAuth.TabIndex = 12
        Me.lblManualAuth.Text = "Manual Auth:"
        '
        'lblCSVPIN
        '
        Me.lblCSVPIN.BackColor = System.Drawing.SystemColors.Control
        Me.lblCSVPIN.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCSVPIN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCSVPIN.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCSVPIN.Location = New System.Drawing.Point(300, 75)
        Me.lblCSVPIN.Name = "lblCSVPIN"
        Me.lblCSVPIN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCSVPIN.Size = New System.Drawing.Size(129, 17)
        Me.lblCSVPIN.TabIndex = 9
        Me.lblCSVPIN.Text = "CSV/PIN:"
        '
        'lblAutoAuthCode
        '
        Me.lblAutoAuthCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblAutoAuthCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAutoAuthCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAutoAuthCode.ForeColor = System.Drawing.SystemColors.GrayText
        Me.lblAutoAuthCode.Location = New System.Drawing.Point(8, 99)
        Me.lblAutoAuthCode.Name = "lblAutoAuthCode"
        Me.lblAutoAuthCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAutoAuthCode.Size = New System.Drawing.Size(129, 20)
        Me.lblAutoAuthCode.TabIndex = 10
        Me.lblAutoAuthCode.Text = "Auth Code:"
        '
        'lblStartDate
        '
        Me.lblStartDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblStartDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStartDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStartDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStartDate.Location = New System.Drawing.Point(300, 50)
        Me.lblStartDate.Name = "lblStartDate"
        Me.lblStartDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStartDate.Size = New System.Drawing.Size(129, 17)
        Me.lblStartDate.TabIndex = 6
        Me.lblStartDate.Text = "Start Date:"
        '
        'lblNameOnCard
        '
        Me.lblNameOnCard.BackColor = System.Drawing.SystemColors.Control
        Me.lblNameOnCard.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNameOnCard.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNameOnCard.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNameOnCard.Location = New System.Drawing.Point(300, 23)
        Me.lblNameOnCard.Name = "lblNameOnCard"
        Me.lblNameOnCard.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNameOnCard.Size = New System.Drawing.Size(145, 17)
        Me.lblNameOnCard.TabIndex = 3
        Me.lblNameOnCard.Text = "Name on Card:"
        '
        'lblIssueNumber
        '
        Me.lblIssueNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblIssueNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIssueNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIssueNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIssueNumber.Location = New System.Drawing.Point(8, 74)
        Me.lblIssueNumber.Name = "lblIssueNumber"
        Me.lblIssueNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIssueNumber.Size = New System.Drawing.Size(129, 20)
        Me.lblIssueNumber.TabIndex = 7
        Me.lblIssueNumber.Text = "Issue Number:"
        '
        'lblExpiryDate
        '
        Me.lblExpiryDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblExpiryDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExpiryDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExpiryDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExpiryDate.Location = New System.Drawing.Point(8, 48)
        Me.lblExpiryDate.Name = "lblExpiryDate"
        Me.lblExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExpiryDate.Size = New System.Drawing.Size(134, 20)
        Me.lblExpiryDate.TabIndex = 4
        Me.lblExpiryDate.Text = "Expiry Date:"
        '
        'lblCardNumber
        '
        Me.lblCardNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblCardNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCardNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCardNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCardNumber.Location = New System.Drawing.Point(8, 23)
        Me.lblCardNumber.Name = "lblCardNumber"
        Me.lblCardNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCardNumber.Size = New System.Drawing.Size(129, 20)
        Me.lblCardNumber.TabIndex = 0
        Me.lblCardNumber.Text = "Card Number:"
        '
        'chkIsDefault
        '
        Me.chkIsDefault.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsDefault.Location = New System.Drawing.Point(11, 0)
        Me.chkIsDefault.Name = "chkIsDefault"
        Me.chkIsDefault.Size = New System.Drawing.Size(145, 24)
        Me.chkIsDefault.TabIndex = 28
        Me.chkIsDefault.Text = "Is Default:"
        Me.chkIsDefault.UseVisualStyleBackColor = True
        Me.chkIsDefault.Visible = False
        '
        'uctCreditCard
        '
        Me.Controls.Add(Me.chkIsDefault)
        Me.Controls.Add(Me.txtCSVPIN)
        Me.Controls.Add(Me.txtNameOnCard)
        Me.Controls.Add(Me.txtStartDate)
        Me.Controls.Add(Me.txtManualAuth)
        Me.Controls.Add(Me.txtTrackingNumber)
        Me.Controls.Add(Me.txtCardTransSlipNo)
        Me.Controls.Add(Me.txtStatus)
        Me.Controls.Add(Me.cboCustomer)
        Me.Controls.Add(Me.txtAutoAuthCode)
        Me.Controls.Add(Me.txtIssueNumber)
        Me.Controls.Add(Me.txtExpiryDate)
        Me.Controls.Add(Me.txtCardNumber)
        Me.Controls.Add(Me.cboCardNumber)
        Me.Controls.Add(Me.cboCardType)
        Me.Controls.Add(Me.cboCCBank)
        Me.Controls.Add(Me.lblCCBank)
        Me.Controls.Add(Me.lblCardType)
        Me.Controls.Add(Me.lblCardTransSlipNo)
        Me.Controls.Add(Me.lblTrackingNumber)
        Me.Controls.Add(Me.lblCustomer)
        Me.Controls.Add(Me.lblManualAuth)
        Me.Controls.Add(Me.lblCSVPIN)
        Me.Controls.Add(Me.lblAutoAuthCode)
        Me.Controls.Add(Me.lblStartDate)
        Me.Controls.Add(Me.lblNameOnCard)
        Me.Controls.Add(Me.lblIssueNumber)
        Me.Controls.Add(Me.lblExpiryDate)
        Me.Controls.Add(Me.lblCardNumber)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctCreditCard"
        Me.Size = New System.Drawing.Size(596, 214)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents chkIsDefault As CheckBox
#End Region
End Class