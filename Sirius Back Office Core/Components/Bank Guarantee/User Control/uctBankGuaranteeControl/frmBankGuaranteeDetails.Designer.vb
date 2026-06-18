<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBankGuaranteeDetails
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
	Public WithEvents cmdApply As System.Windows.Forms.Button
	Public WithEvents txtBankNameId As System.Windows.Forms.TextBox
	Public WithEvents cmdFindBank As System.Windows.Forms.Button
	Public WithEvents chkIsSinglePolicyLock As System.Windows.Forms.CheckBox
	Public WithEvents txtExpiryDate As System.Windows.Forms.TextBox
	Public WithEvents txtIssueDate As System.Windows.Forms.TextBox
	Public WithEvents txtBGNo As System.Windows.Forms.TextBox
	Public WithEvents txtBankBranch As System.Windows.Forms.TextBox
	Public WithEvents txtLimitsAvailable As System.Windows.Forms.TextBox
	Public WithEvents txtAmountSysCurr As System.Windows.Forms.TextBox
	Public WithEvents txtAmount As System.Windows.Forms.TextBox
	Public WithEvents cboCurrency As PMLookupControl.cboPMLookup
	Public WithEvents lblLmitsAvailable As System.Windows.Forms.Label
	Public WithEvents lblAmtSysCurr As System.Windows.Forms.Label
	Public WithEvents Label5 As System.Windows.Forms.Label
	Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents cboCustodyBranch As PMLookupControl.cboPMLookup
	Public WithEvents Label7 As System.Windows.Forms.Label
	Public WithEvents Label11 As System.Windows.Forms.Label
	Public WithEvents Label10 As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Private WithEvents _SSBGDetails_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents PickListProducts As uctPickList.PickList
	Public WithEvents Frame2 As System.Windows.Forms.GroupBox
	Private WithEvents _SSBGDetails_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents PickListBranches As uctPickList.PickList
	Public WithEvents Frame3 As System.Windows.Forms.GroupBox
	Private WithEvents _SSBGDetails_TabPage2 As System.Windows.Forms.TabPage
	Public WithEvents txtTotalTransAmt As System.Windows.Forms.TextBox
	Private WithEvents _lvwPolicydetails_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwPolicydetails_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwPolicydetails_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwPolicydetails_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwPolicydetails_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwPolicydetails_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwPolicydetails_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwPolicydetails_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwPolicydetails_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwPolicydetails As System.Windows.Forms.ListView
	Public WithEvents Label6 As System.Windows.Forms.Label
	Public WithEvents Frame4 As System.Windows.Forms.GroupBox
	Private WithEvents _SSBGDetails_TabPage3 As System.Windows.Forms.TabPage
	Public WithEvents SSBGDetails As System.Windows.Forms.TabControl
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents cmdAddTask As System.Windows.Forms.Button
	Public WithEvents ImageList1 As System.Windows.Forms.ImageList
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBankGuaranteeDetails))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdApply = New System.Windows.Forms.Button
        Me.SSBGDetails = New System.Windows.Forms.TabControl
        Me._SSBGDetails_TabPage0 = New System.Windows.Forms.TabPage
        Me.txtBankNameId = New System.Windows.Forms.TextBox
        Me.cmdFindBank = New System.Windows.Forms.Button
        Me.chkIsSinglePolicyLock = New System.Windows.Forms.CheckBox
        Me.txtExpiryDate = New System.Windows.Forms.TextBox
        Me.txtIssueDate = New System.Windows.Forms.TextBox
        Me.txtBGNo = New System.Windows.Forms.TextBox
        Me.txtBankBranch = New System.Windows.Forms.TextBox
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.txtLimitsAvailable = New System.Windows.Forms.TextBox
        Me.txtAmountSysCurr = New System.Windows.Forms.TextBox
        Me.txtAmount = New System.Windows.Forms.TextBox
        Me.cboCurrency = New PMLookupControl.cboPMLookup
        Me.lblLmitsAvailable = New System.Windows.Forms.Label
        Me.lblAmtSysCurr = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.cboCustodyBranch = New PMLookupControl.cboPMLookup
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me._SSBGDetails_TabPage1 = New System.Windows.Forms.TabPage
        Me.Frame2 = New System.Windows.Forms.GroupBox
        Me.PickListProducts = New uctPickList.PickList
        Me._SSBGDetails_TabPage2 = New System.Windows.Forms.TabPage
        Me.Frame3 = New System.Windows.Forms.GroupBox
        Me.PickListBranches = New uctPickList.PickList
        Me._SSBGDetails_TabPage3 = New System.Windows.Forms.TabPage
        Me.Frame4 = New System.Windows.Forms.GroupBox
        Me.txtTotalTransAmt = New System.Windows.Forms.TextBox
        Me.lvwPolicydetails = New System.Windows.Forms.ListView
        Me._lvwPolicydetails_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwPolicydetails_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwPolicydetails_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwPolicydetails_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwPolicydetails_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwPolicydetails_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwPolicydetails_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._lvwPolicydetails_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
        Me._lvwPolicydetails_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Label6 = New System.Windows.Forms.Label
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOk = New System.Windows.Forms.Button
        Me.cmdAddTask = New System.Windows.Forms.Button
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.SSBGDetails.SuspendLayout()
        Me._SSBGDetails_TabPage0.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me._SSBGDetails_TabPage1.SuspendLayout()
        Me.Frame2.SuspendLayout()
        Me._SSBGDetails_TabPage2.SuspendLayout()
        Me.Frame3.SuspendLayout()
        Me._SSBGDetails_TabPage3.SuspendLayout()
        Me.Frame4.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApply.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Location = New System.Drawing.Point(274, 480)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApply.Size = New System.Drawing.Size(85, 25)
        Me.cmdApply.TabIndex = 29
        Me.cmdApply.Text = "&Apply"
        Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdApply.UseVisualStyleBackColor = False
        '
        'SSBGDetails
        '
        Me.SSBGDetails.Controls.Add(Me._SSBGDetails_TabPage0)
        Me.SSBGDetails.Controls.Add(Me._SSBGDetails_TabPage1)
        Me.SSBGDetails.Controls.Add(Me._SSBGDetails_TabPage2)
        Me.SSBGDetails.Controls.Add(Me._SSBGDetails_TabPage3)
        Me.SSBGDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSBGDetails.ItemSize = New System.Drawing.Size(133, 18)
        Me.SSBGDetails.Location = New System.Drawing.Point(2, 2)
        Me.SSBGDetails.Multiline = True
        Me.SSBGDetails.Name = "SSBGDetails"
        Me.SSBGDetails.SelectedIndex = 0
        Me.SSBGDetails.Size = New System.Drawing.Size(541, 475)
        Me.SSBGDetails.TabIndex = 15
        '
        '_SSBGDetails_TabPage0
        '
        Me._SSBGDetails_TabPage0.Controls.Add(Me.txtBankNameId)
        Me._SSBGDetails_TabPage0.Controls.Add(Me.cmdFindBank)
        Me._SSBGDetails_TabPage0.Controls.Add(Me.chkIsSinglePolicyLock)
        Me._SSBGDetails_TabPage0.Controls.Add(Me.txtExpiryDate)
        Me._SSBGDetails_TabPage0.Controls.Add(Me.txtIssueDate)
        Me._SSBGDetails_TabPage0.Controls.Add(Me.txtBGNo)
        Me._SSBGDetails_TabPage0.Controls.Add(Me.txtBankBranch)
        Me._SSBGDetails_TabPage0.Controls.Add(Me.Frame1)
        Me._SSBGDetails_TabPage0.Controls.Add(Me.cboCustodyBranch)
        Me._SSBGDetails_TabPage0.Controls.Add(Me.Label7)
        Me._SSBGDetails_TabPage0.Controls.Add(Me.Label11)
        Me._SSBGDetails_TabPage0.Controls.Add(Me.Label10)
        Me._SSBGDetails_TabPage0.Controls.Add(Me.Label3)
        Me._SSBGDetails_TabPage0.Controls.Add(Me.Label2)
        Me._SSBGDetails_TabPage0.Controls.Add(Me.Label1)
        Me._SSBGDetails_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSBGDetails_TabPage0.Name = "_SSBGDetails_TabPage0"
        Me._SSBGDetails_TabPage0.Size = New System.Drawing.Size(533, 449)
        Me._SSBGDetails_TabPage0.TabIndex = 0
        Me._SSBGDetails_TabPage0.Text = "Bank Guarentee Details"
        '
        'txtBankNameId
        '
        Me.txtBankNameId.AcceptsReturn = True
        Me.txtBankNameId.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankNameId.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankNameId.Enabled = False
        Me.txtBankNameId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankNameId.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankNameId.Location = New System.Drawing.Point(160, 30)
        Me.txtBankNameId.MaxLength = 0
        Me.txtBankNameId.Name = "txtBankNameId"
        Me.txtBankNameId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankNameId.Size = New System.Drawing.Size(205, 20)
        Me.txtBankNameId.TabIndex = 1
        '
        'cmdFindBank
        '
        Me.cmdFindBank.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindBank.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindBank.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindBank.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindBank.Location = New System.Drawing.Point(42, 28)
        Me.cmdFindBank.Name = "cmdFindBank"
        Me.cmdFindBank.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindBank.Size = New System.Drawing.Size(93, 19)
        Me.cmdFindBank.TabIndex = 0
        Me.cmdFindBank.Text = "Bank Name"
        Me.cmdFindBank.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindBank.UseVisualStyleBackColor = False
        '
        'chkIsSinglePolicyLock
        '
        Me.chkIsSinglePolicyLock.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsSinglePolicyLock.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsSinglePolicyLock.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsSinglePolicyLock.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsSinglePolicyLock.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsSinglePolicyLock.Location = New System.Drawing.Point(34, 327)
        Me.chkIsSinglePolicyLock.Name = "chkIsSinglePolicyLock"
        Me.chkIsSinglePolicyLock.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsSinglePolicyLock.Size = New System.Drawing.Size(137, 17)
        Me.chkIsSinglePolicyLock.TabIndex = 11
        Me.chkIsSinglePolicyLock.Text = "Single Policy Lock"
        Me.chkIsSinglePolicyLock.UseVisualStyleBackColor = False
        '
        'txtExpiryDate
        '
        Me.txtExpiryDate.AcceptsReturn = True
        Me.txtExpiryDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtExpiryDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtExpiryDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExpiryDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtExpiryDate.Location = New System.Drawing.Point(158, 296)
        Me.txtExpiryDate.MaxLength = 0
        Me.txtExpiryDate.Name = "txtExpiryDate"
        Me.txtExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtExpiryDate.Size = New System.Drawing.Size(123, 20)
        Me.txtExpiryDate.TabIndex = 10
        '
        'txtIssueDate
        '
        Me.txtIssueDate.AcceptsReturn = True
        Me.txtIssueDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtIssueDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtIssueDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIssueDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIssueDate.Location = New System.Drawing.Point(158, 266)
        Me.txtIssueDate.MaxLength = 0
        Me.txtIssueDate.Name = "txtIssueDate"
        Me.txtIssueDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtIssueDate.Size = New System.Drawing.Size(123, 20)
        Me.txtIssueDate.TabIndex = 9
        '
        'txtBGNo
        '
        Me.txtBGNo.AcceptsReturn = True
        Me.txtBGNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtBGNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBGNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBGNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBGNo.Location = New System.Drawing.Point(160, 88)
        Me.txtBGNo.MaxLength = 0
        Me.txtBGNo.Name = "txtBGNo"
        Me.txtBGNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBGNo.Size = New System.Drawing.Size(205, 20)
        Me.txtBGNo.TabIndex = 3
        '
        'txtBankBranch
        '
        Me.txtBankBranch.AcceptsReturn = True
        Me.txtBankBranch.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankBranch.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankBranch.Location = New System.Drawing.Point(160, 58)
        Me.txtBankBranch.MaxLength = 0
        Me.txtBankBranch.Name = "txtBankBranch"
        Me.txtBankBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankBranch.Size = New System.Drawing.Size(205, 20)
        Me.txtBankBranch.TabIndex = 2
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.txtLimitsAvailable)
        Me.Frame1.Controls.Add(Me.txtAmountSysCurr)
        Me.Frame1.Controls.Add(Me.txtAmount)
        Me.Frame1.Controls.Add(Me.cboCurrency)
        Me.Frame1.Controls.Add(Me.lblLmitsAvailable)
        Me.Frame1.Controls.Add(Me.lblAmtSysCurr)
        Me.Frame1.Controls.Add(Me.Label5)
        Me.Frame1.Controls.Add(Me.Label4)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(4, 146)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(523, 109)
        Me.Frame1.TabIndex = 5
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "BG Limit"
        '
        'txtLimitsAvailable
        '
        Me.txtLimitsAvailable.AcceptsReturn = True
        Me.txtLimitsAvailable.BackColor = System.Drawing.SystemColors.Window
        Me.txtLimitsAvailable.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLimitsAvailable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLimitsAvailable.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLimitsAvailable.Location = New System.Drawing.Point(156, 74)
        Me.txtLimitsAvailable.MaxLength = 0
        Me.txtLimitsAvailable.Name = "txtLimitsAvailable"
        Me.txtLimitsAvailable.ReadOnly = True
        Me.txtLimitsAvailable.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLimitsAvailable.Size = New System.Drawing.Size(149, 20)
        Me.txtLimitsAvailable.TabIndex = 8
        '
        'txtAmountSysCurr
        '
        Me.txtAmountSysCurr.AcceptsReturn = True
        Me.txtAmountSysCurr.BackColor = System.Drawing.SystemColors.Window
        Me.txtAmountSysCurr.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAmountSysCurr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmountSysCurr.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAmountSysCurr.Location = New System.Drawing.Point(156, 146)
        Me.txtAmountSysCurr.MaxLength = 0
        Me.txtAmountSysCurr.Name = "txtAmountSysCurr"
        Me.txtAmountSysCurr.ReadOnly = True
        Me.txtAmountSysCurr.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAmountSysCurr.Size = New System.Drawing.Size(149, 20)
        Me.txtAmountSysCurr.TabIndex = 24
        Me.txtAmountSysCurr.Visible = False
        '
        'txtAmount
        '
        Me.txtAmount.AcceptsReturn = True
        Me.txtAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAmount.Location = New System.Drawing.Point(156, 46)
        Me.txtAmount.MaxLength = 0
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAmount.Size = New System.Drawing.Size(149, 20)
        Me.txtAmount.TabIndex = 7
        '
        'cboCurrency
        '
        Me.cboCurrency.DefaultItemId = 0
        Me.cboCurrency.FirstItem = ""
        Me.cboCurrency.ItemId = 0
        Me.cboCurrency.ListIndex = -1
        Me.cboCurrency.Location = New System.Drawing.Point(156, 18)
        Me.cboCurrency.Name = "cboCurrency"
        Me.cboCurrency.PMLookupProductFamily = 1
        Me.cboCurrency.SingleItemId = 0
        Me.cboCurrency.Size = New System.Drawing.Size(183, 21)
        Me.cboCurrency.Sorted = True
        Me.cboCurrency.TabIndex = 6
        Me.cboCurrency.TableName = "currency"
        Me.cboCurrency.ToolTipText = ""
        Me.cboCurrency.WhereClause = ""
        '
        'lblLmitsAvailable
        '
        Me.lblLmitsAvailable.AutoSize = True
        Me.lblLmitsAvailable.BackColor = System.Drawing.SystemColors.Control
        Me.lblLmitsAvailable.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLmitsAvailable.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLmitsAvailable.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLmitsAvailable.Location = New System.Drawing.Point(34, 78)
        Me.lblLmitsAvailable.Name = "lblLmitsAvailable"
        Me.lblLmitsAvailable.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLmitsAvailable.Size = New System.Drawing.Size(96, 13)
        Me.lblLmitsAvailable.TabIndex = 23
        Me.lblLmitsAvailable.Text = "Limits Available"
        '
        'lblAmtSysCurr
        '
        Me.lblAmtSysCurr.BackColor = System.Drawing.SystemColors.Control
        Me.lblAmtSysCurr.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAmtSysCurr.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAmtSysCurr.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAmtSysCurr.Location = New System.Drawing.Point(34, 140)
        Me.lblAmtSysCurr.Name = "lblAmtSysCurr"
        Me.lblAmtSysCurr.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAmtSysCurr.Size = New System.Drawing.Size(100, 39)
        Me.lblAmtSysCurr.TabIndex = 22
        Me.lblAmtSysCurr.Text = "Amount in System Currency"
        Me.lblAmtSysCurr.Visible = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.SystemColors.Control
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(36, 50)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(51, 13)
        Me.Label5.TabIndex = 21
        Me.Label5.Text = "Amount"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(36, 20)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(60, 13)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "Currency"
        '
        'cboCustodyBranch
        '
        Me.cboCustodyBranch.DefaultItemId = 0
        Me.cboCustodyBranch.FirstItem = ""
        Me.cboCustodyBranch.ItemId = 0
        Me.cboCustodyBranch.ListIndex = -1
        Me.cboCustodyBranch.Location = New System.Drawing.Point(160, 116)
        Me.cboCustodyBranch.Name = "cboCustodyBranch"
        Me.cboCustodyBranch.PMLookupProductFamily = 1
        Me.cboCustodyBranch.SingleItemId = 0
        Me.cboCustodyBranch.Size = New System.Drawing.Size(183, 21)
        Me.cboCustodyBranch.Sorted = True
        Me.cboCustodyBranch.TabIndex = 4
        Me.cboCustodyBranch.TableName = "Source"
        Me.cboCustodyBranch.ToolTipText = ""
        Me.cboCustodyBranch.WhereClause = ""
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.SystemColors.Control
        Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label7.Location = New System.Drawing.Point(42, 118)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label7.Size = New System.Drawing.Size(119, 13)
        Me.Label7.TabIndex = 30
        Me.Label7.Text = "BG Custody Branch"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.SystemColors.Control
        Me.Label11.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label11.Location = New System.Drawing.Point(40, 268)
        Me.Label11.Name = "Label11"
        Me.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label11.Size = New System.Drawing.Size(69, 13)
        Me.Label11.TabIndex = 26
        Me.Label11.Text = "Issue Date"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.SystemColors.Control
        Me.Label10.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label10.Location = New System.Drawing.Point(38, 298)
        Me.Label10.Name = "Label10"
        Me.Label10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label10.Size = New System.Drawing.Size(74, 13)
        Me.Label10.TabIndex = 25
        Me.Label10.Text = "Expiry Date"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(42, 92)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(52, 13)
        Me.Label3.TabIndex = 18
        Me.Label3.Text = "Number"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(42, 62)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(80, 13)
        Me.Label2.TabIndex = 17
        Me.Label2.Text = "Bank Branch"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(42, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(73, 13)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Bank Name"
        '
        '_SSBGDetails_TabPage1
        '
        Me._SSBGDetails_TabPage1.Controls.Add(Me.Frame2)
        Me._SSBGDetails_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._SSBGDetails_TabPage1.Name = "_SSBGDetails_TabPage1"
        Me._SSBGDetails_TabPage1.Size = New System.Drawing.Size(533, 449)
        Me._SSBGDetails_TabPage1.TabIndex = 1
        Me._SSBGDetails_TabPage1.Text = "Products"
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.PickListProducts)
        Me.Frame2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(4, 6)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(529, 441)
        Me.Frame2.TabIndex = 27
        Me.Frame2.TabStop = False
        Me.Frame2.Text = "Select Products"
        '
        'PickListProducts
        '
        Me.PickListProducts.AvailableCaption = "Searched Products "
        Me.PickListProducts.BusinessObject = "bSIRBankGuarantee.Business"
        Me.PickListProducts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PickListProducts.ForeignKeys = CType(resources.GetObject("PickListProducts.ForeignKeys"), Microsoft.VisualBasic.Collection)
        Me.PickListProducts.IsSearchable = True
        Me.PickListProducts.Location = New System.Drawing.Point(2, 12)
        Me.PickListProducts.Name = "PickListProducts"
        Me.PickListProducts.PickListType = ""
        Me.PickListProducts.Size = New System.Drawing.Size(525, 427)
        Me.PickListProducts.TabIndex = 32
        '
        '_SSBGDetails_TabPage2
        '
        Me._SSBGDetails_TabPage2.Controls.Add(Me.Frame3)
        Me._SSBGDetails_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._SSBGDetails_TabPage2.Name = "_SSBGDetails_TabPage2"
        Me._SSBGDetails_TabPage2.Size = New System.Drawing.Size(533, 449)
        Me._SSBGDetails_TabPage2.TabIndex = 2
        Me._SSBGDetails_TabPage2.Text = "Branches"
        '
        'Frame3
        '
        Me.Frame3.BackColor = System.Drawing.SystemColors.Control
        Me.Frame3.Controls.Add(Me.PickListBranches)
        Me.Frame3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame3.Location = New System.Drawing.Point(4, 6)
        Me.Frame3.Name = "Frame3"
        Me.Frame3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame3.Size = New System.Drawing.Size(529, 441)
        Me.Frame3.TabIndex = 28
        Me.Frame3.TabStop = False
        Me.Frame3.Text = "Select Braches"
        '
        'PickListBranches
        '
        Me.PickListBranches.AvailableCaption = "Searched Branches"
        Me.PickListBranches.BusinessObject = "bSIRBankGuarantee.Business"
        Me.PickListBranches.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PickListBranches.ForeignKeys = CType(resources.GetObject("PickListBranches.ForeignKeys"), Microsoft.VisualBasic.Collection)
        Me.PickListBranches.IsSearchable = True
        Me.PickListBranches.Location = New System.Drawing.Point(2, 12)
        Me.PickListBranches.Name = "PickListBranches"
        Me.PickListBranches.PickListType = ""
        Me.PickListBranches.Size = New System.Drawing.Size(525, 427)
        Me.PickListBranches.TabIndex = 33
        '
        '_SSBGDetails_TabPage3
        '
        Me._SSBGDetails_TabPage3.Controls.Add(Me.Frame4)
        Me._SSBGDetails_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._SSBGDetails_TabPage3.Name = "_SSBGDetails_TabPage3"
        Me._SSBGDetails_TabPage3.Size = New System.Drawing.Size(533, 449)
        Me._SSBGDetails_TabPage3.TabIndex = 3
        Me._SSBGDetails_TabPage3.Text = "Policies"
        '
        'Frame4
        '
        Me.Frame4.BackColor = System.Drawing.SystemColors.Control
        Me.Frame4.Controls.Add(Me.txtTotalTransAmt)
        Me.Frame4.Controls.Add(Me.lvwPolicydetails)
        Me.Frame4.Controls.Add(Me.Label6)
        Me.Frame4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame4.Location = New System.Drawing.Point(4, 4)
        Me.Frame4.Name = "Frame4"
        Me.Frame4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame4.Size = New System.Drawing.Size(527, 441)
        Me.Frame4.TabIndex = 31
        Me.Frame4.TabStop = False
        Me.Frame4.Text = "Policies"
        '
        'txtTotalTransAmt
        '
        Me.txtTotalTransAmt.AcceptsReturn = True
        Me.txtTotalTransAmt.BackColor = System.Drawing.SystemColors.Control
        Me.txtTotalTransAmt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalTransAmt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalTransAmt.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalTransAmt.Location = New System.Drawing.Point(140, 18)
        Me.txtTotalTransAmt.MaxLength = 0
        Me.txtTotalTransAmt.Name = "txtTotalTransAmt"
        Me.txtTotalTransAmt.ReadOnly = True
        Me.txtTotalTransAmt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalTransAmt.Size = New System.Drawing.Size(147, 20)
        Me.txtTotalTransAmt.TabIndex = 35
        Me.txtTotalTransAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lvwPolicydetails
        '
        Me.lvwPolicydetails.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwPolicydetails, "")
        Me.lvwPolicydetails.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwPolicydetails_ColumnHeader_1, Me._lvwPolicydetails_ColumnHeader_2, Me._lvwPolicydetails_ColumnHeader_3, Me._lvwPolicydetails_ColumnHeader_4, Me._lvwPolicydetails_ColumnHeader_5, Me._lvwPolicydetails_ColumnHeader_6, Me._lvwPolicydetails_ColumnHeader_7, Me._lvwPolicydetails_ColumnHeader_8, Me._lvwPolicydetails_ColumnHeader_9})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwPolicydetails, False)
        Me.lvwPolicydetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwPolicydetails.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwPolicydetails.FullRowSelect = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwPolicydetails, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwPolicydetails, "")
        Me.lvwPolicydetails.LargeImageList = Me.ImageList1
        Me.lvwPolicydetails.Location = New System.Drawing.Point(2, 58)
        Me.lvwPolicydetails.Name = "lvwPolicydetails"
        Me.lvwPolicydetails.Size = New System.Drawing.Size(521, 379)
        Me.listViewHelper1.SetSmallIcons(Me.lvwPolicydetails, "")
        Me.lvwPolicydetails.SmallImageList = Me.ImageList1
        Me.listViewHelper1.SetSorted(Me.lvwPolicydetails, False)
        Me.listViewHelper1.SetSortKey(Me.lvwPolicydetails, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwPolicydetails, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwPolicydetails.TabIndex = 11
        Me.lvwPolicydetails.UseCompatibleStateImageBehavior = False
        Me.lvwPolicydetails.View = System.Windows.Forms.View.Details
        '
        '_lvwPolicydetails_ColumnHeader_1
        '
        Me._lvwPolicydetails_ColumnHeader_1.Text = "Client Code"
        Me._lvwPolicydetails_ColumnHeader_1.Width = 97
        '
        '_lvwPolicydetails_ColumnHeader_2
        '
        Me._lvwPolicydetails_ColumnHeader_2.Text = "Client Name"
        Me._lvwPolicydetails_ColumnHeader_2.Width = 97
        '
        '_lvwPolicydetails_ColumnHeader_3
        '
        Me._lvwPolicydetails_ColumnHeader_3.Text = "Insurance Ref"
        Me._lvwPolicydetails_ColumnHeader_3.Width = 97
        '
        '_lvwPolicydetails_ColumnHeader_4
        '
        Me._lvwPolicydetails_ColumnHeader_4.Text = "Agent"
        Me._lvwPolicydetails_ColumnHeader_4.Width = 97
        '
        '_lvwPolicydetails_ColumnHeader_5
        '
        Me._lvwPolicydetails_ColumnHeader_5.Text = "Branch"
        Me._lvwPolicydetails_ColumnHeader_5.Width = 97
        '
        '_lvwPolicydetails_ColumnHeader_6
        '
        Me._lvwPolicydetails_ColumnHeader_6.Text = "Product"
        Me._lvwPolicydetails_ColumnHeader_6.Width = 97
        '
        '_lvwPolicydetails_ColumnHeader_7
        '
        Me._lvwPolicydetails_ColumnHeader_7.Text = "Amount"
        Me._lvwPolicydetails_ColumnHeader_7.Width = 97
        '
        '_lvwPolicydetails_ColumnHeader_8
        '
        Me._lvwPolicydetails_ColumnHeader_8.Text = "Cover From"
        Me._lvwPolicydetails_ColumnHeader_8.Width = 97
        '
        '_lvwPolicydetails_ColumnHeader_9
        '
        Me._lvwPolicydetails_ColumnHeader_9.Text = "Cover To"
        Me._lvwPolicydetails_ColumnHeader_9.Width = 97
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "add")
        Me.ImageList1.Images.SetKeyName(1, "history")
        Me.ImageList1.Images.SetKeyName(2, "edited")
        Me.ImageList1.Images.SetKeyName(3, "delete")
        Me.ImageList1.Images.SetKeyName(4, "saved")
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.SystemColors.Control
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(6, 22)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(129, 13)
        Me.Label6.TabIndex = 34
        Me.Label6.Text = "Total Transaction Amount"
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(454, 480)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(85, 25)
        Me.cmdCancel.TabIndex = 14
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOk
        '
        Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOk.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdOk.Enabled = False
        Me.cmdOk.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOk.Location = New System.Drawing.Point(364, 480)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(85, 25)
        Me.cmdOk.TabIndex = 13
        Me.cmdOk.Text = "&Ok"
        Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOk.UseVisualStyleBackColor = False
        '
        'cmdAddTask
        '
        Me.cmdAddTask.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddTask.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddTask.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddTask.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddTask.Location = New System.Drawing.Point(182, 480)
        Me.cmdAddTask.Name = "cmdAddTask"
        Me.cmdAddTask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddTask.Size = New System.Drawing.Size(85, 25)
        Me.cmdAddTask.TabIndex = 12
        Me.cmdAddTask.Text = "&Add Task"
        Me.cmdAddTask.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddTask.UseVisualStyleBackColor = False
        '
        'frmBankGuaranteeDetails
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(542, 510)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.SSBGDetails)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.cmdAddTask)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmBankGuaranteeDetails"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Bank Guaranatee Setup"
        Me.SSBGDetails.ResumeLayout(False)
        Me._SSBGDetails_TabPage0.ResumeLayout(False)
        Me._SSBGDetails_TabPage0.PerformLayout()
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me._SSBGDetails_TabPage1.ResumeLayout(False)
        Me.Frame2.ResumeLayout(False)
        Me._SSBGDetails_TabPage2.ResumeLayout(False)
        Me.Frame3.ResumeLayout(False)
        Me._SSBGDetails_TabPage3.ResumeLayout(False)
        Me.Frame4.ResumeLayout(False)
        Me.Frame4.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class