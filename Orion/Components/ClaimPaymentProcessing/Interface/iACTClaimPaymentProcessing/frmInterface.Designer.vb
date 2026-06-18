<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
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
	Public WithEvents cmdClose As System.Windows.Forms.Button
	Public WithEvents txtDateOfPaymentTo As System.Windows.Forms.TextBox
	Public WithEvents txtDateOfPaymentFrom As System.Windows.Forms.TextBox
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents lblDateOfPayment As System.Windows.Forms.Label
	Public WithEvents fraDateOfPayment As System.Windows.Forms.GroupBox
	Public WithEvents txtAccountName As System.Windows.Forms.TextBox
	Public WithEvents txtAccountCode As System.Windows.Forms.TextBox
	Public WithEvents lblAccountName As System.Windows.Forms.Label
	Public WithEvents lblAccount As System.Windows.Forms.Label
	Public WithEvents fraAccountSearch As System.Windows.Forms.GroupBox
	Public WithEvents cmdNewSearch As System.Windows.Forms.Button
	Public WithEvents cmdFindNow As System.Windows.Forms.Button
	Public WithEvents fraPayee As System.Windows.Forms.GroupBox
	Public WithEvents cmdSettleAll As System.Windows.Forms.Button
	Public WithEvents cmdPayAsOne As System.Windows.Forms.Button
	Public WithEvents cmdSelectAll As System.Windows.Forms.Button
	Public WithEvents cmdPayIndividually As System.Windows.Forms.Button
	Private WithEvents _lvwClaimPayments_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClaimPayments_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClaimPayments_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClaimPayments_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClaimPayments_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClaimPayments_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwClaimPayments As System.Windows.Forms.ListView
	Public WithEvents fraUnallocatedClaimPayments As System.Windows.Forms.GroupBox
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdPayAsOne = New System.Windows.Forms.Button
        Me.cmdPayIndividually = New System.Windows.Forms.Button
        Me.cmdClose = New System.Windows.Forms.Button
        Me.fraPayee = New System.Windows.Forms.GroupBox
        Me.fraDateOfPayment = New System.Windows.Forms.GroupBox
        Me.txtDateOfPaymentTo = New System.Windows.Forms.TextBox
        Me.txtDateOfPaymentFrom = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblDateOfPayment = New System.Windows.Forms.Label
        Me.fraAccountSearch = New System.Windows.Forms.GroupBox
        Me.txtAccountName = New System.Windows.Forms.TextBox
        Me.txtAccountCode = New System.Windows.Forms.TextBox
        Me.lblAccountName = New System.Windows.Forms.Label
        Me.lblAccount = New System.Windows.Forms.Label
        Me.cmdNewSearch = New System.Windows.Forms.Button
        Me.cmdFindNow = New System.Windows.Forms.Button
        Me.fraUnallocatedClaimPayments = New System.Windows.Forms.GroupBox
        Me.cmdSettleAll = New System.Windows.Forms.Button
        Me.cmdSelectAll = New System.Windows.Forms.Button
        Me.lvwClaimPayments = New System.Windows.Forms.ListView
        Me._lvwClaimPayments_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwClaimPayments_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwClaimPayments_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwClaimPayments_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwClaimPayments_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwClaimPayments_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.fraPayee.SuspendLayout()
        Me.fraDateOfPayment.SuspendLayout()
        Me.fraAccountSearch.SuspendLayout()
        Me.fraUnallocatedClaimPayments.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdPayAsOne
        '
        Me.cmdPayAsOne.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPayAsOne.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPayAsOne.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPayAsOne.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPayAsOne.Location = New System.Drawing.Point(160, 408)
        Me.cmdPayAsOne.Name = "cmdPayAsOne"
        Me.cmdPayAsOne.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPayAsOne.Size = New System.Drawing.Size(147, 25)
        Me.cmdPayAsOne.TabIndex = 15
        Me.cmdPayAsOne.Text = "Make &Single Payment"
        Me.cmdPayAsOne.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdPayAsOne, "Make a single payment for all selected items")
        Me.cmdPayAsOne.UseVisualStyleBackColor = False
        '
        'cmdPayIndividually
        '
        Me.cmdPayIndividually.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPayIndividually.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPayIndividually.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPayIndividually.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPayIndividually.Location = New System.Drawing.Point(8, 408)
        Me.cmdPayIndividually.Name = "cmdPayIndividually"
        Me.cmdPayIndividually.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPayIndividually.Size = New System.Drawing.Size(147, 25)
        Me.cmdPayIndividually.TabIndex = 14
        Me.cmdPayIndividually.Text = "Make &Multiple Payments"
        Me.cmdPayIndividually.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdPayIndividually, "Makes an individual payment for each item selected")
        Me.cmdPayIndividually.UseVisualStyleBackColor = False
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClose.Location = New System.Drawing.Point(672, 560)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClose.Size = New System.Drawing.Size(113, 25)
        Me.cmdClose.TabIndex = 18
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClose.UseVisualStyleBackColor = False
        '
        'fraPayee
        '
        Me.fraPayee.BackColor = System.Drawing.SystemColors.Control
        Me.fraPayee.Controls.Add(Me.fraDateOfPayment)
        Me.fraPayee.Controls.Add(Me.fraAccountSearch)
        Me.fraPayee.Controls.Add(Me.cmdNewSearch)
        Me.fraPayee.Controls.Add(Me.cmdFindNow)
        Me.fraPayee.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPayee.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPayee.Location = New System.Drawing.Point(4, 0)
        Me.fraPayee.Name = "fraPayee"
        Me.fraPayee.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPayee.Size = New System.Drawing.Size(777, 105)
        Me.fraPayee.TabIndex = 0
        Me.fraPayee.TabStop = False
        Me.fraPayee.Text = "Payment Search"
        '
        'fraDateOfPayment
        '
        Me.fraDateOfPayment.BackColor = System.Drawing.SystemColors.Control
        Me.fraDateOfPayment.Controls.Add(Me.txtDateOfPaymentTo)
        Me.fraDateOfPayment.Controls.Add(Me.txtDateOfPaymentFrom)
        Me.fraDateOfPayment.Controls.Add(Me.Label1)
        Me.fraDateOfPayment.Controls.Add(Me.lblDateOfPayment)
        Me.fraDateOfPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDateOfPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDateOfPayment.Location = New System.Drawing.Point(456, 16)
        Me.fraDateOfPayment.Name = "fraDateOfPayment"
        Me.fraDateOfPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDateOfPayment.Size = New System.Drawing.Size(225, 81)
        Me.fraDateOfPayment.TabIndex = 6
        Me.fraDateOfPayment.TabStop = False
        Me.fraDateOfPayment.Text = "Search by Date"
        '
        'txtDateOfPaymentTo
        '
        Me.txtDateOfPaymentTo.AcceptsReturn = True
        Me.txtDateOfPaymentTo.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateOfPaymentTo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDateOfPaymentTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDateOfPaymentTo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateOfPaymentTo.Location = New System.Drawing.Point(128, 44)
        Me.txtDateOfPaymentTo.MaxLength = 0
        Me.txtDateOfPaymentTo.Name = "txtDateOfPaymentTo"
        Me.txtDateOfPaymentTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateOfPaymentTo.Size = New System.Drawing.Size(89, 20)
        Me.txtDateOfPaymentTo.TabIndex = 9
        '
        'txtDateOfPaymentFrom
        '
        Me.txtDateOfPaymentFrom.AcceptsReturn = True
        Me.txtDateOfPaymentFrom.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateOfPaymentFrom.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDateOfPaymentFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDateOfPaymentFrom.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateOfPaymentFrom.Location = New System.Drawing.Point(128, 16)
        Me.txtDateOfPaymentFrom.MaxLength = 0
        Me.txtDateOfPaymentFrom.Name = "txtDateOfPaymentFrom"
        Me.txtDateOfPaymentFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateOfPaymentFrom.Size = New System.Drawing.Size(89, 20)
        Me.txtDateOfPaymentFrom.TabIndex = 8
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(8, 48)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(93, 13)
        Me.Label1.TabIndex = 19
        Me.Label1.Text = "Payment Date To:"
        '
        'lblDateOfPayment
        '
        Me.lblDateOfPayment.AutoSize = True
        Me.lblDateOfPayment.BackColor = System.Drawing.SystemColors.Control
        Me.lblDateOfPayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDateOfPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateOfPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDateOfPayment.Location = New System.Drawing.Point(8, 20)
        Me.lblDateOfPayment.Name = "lblDateOfPayment"
        Me.lblDateOfPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDateOfPayment.Size = New System.Drawing.Size(103, 13)
        Me.lblDateOfPayment.TabIndex = 7
        Me.lblDateOfPayment.Text = "Payment Date From:"
        '
        'fraAccountSearch
        '
        Me.fraAccountSearch.BackColor = System.Drawing.SystemColors.Control
        Me.fraAccountSearch.Controls.Add(Me.txtAccountName)
        Me.fraAccountSearch.Controls.Add(Me.txtAccountCode)
        Me.fraAccountSearch.Controls.Add(Me.lblAccountName)
        Me.fraAccountSearch.Controls.Add(Me.lblAccount)
        Me.fraAccountSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAccountSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAccountSearch.Location = New System.Drawing.Point(8, 16)
        Me.fraAccountSearch.Name = "fraAccountSearch"
        Me.fraAccountSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAccountSearch.Size = New System.Drawing.Size(441, 81)
        Me.fraAccountSearch.TabIndex = 1
        Me.fraAccountSearch.TabStop = False
        Me.fraAccountSearch.Text = "Search by Account"
        '
        'txtAccountName
        '
        Me.txtAccountName.AcceptsReturn = True
        Me.txtAccountName.BackColor = System.Drawing.SystemColors.Control
        Me.txtAccountName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccountName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccountName.Location = New System.Drawing.Point(99, 48)
        Me.txtAccountName.MaxLength = 0
        Me.txtAccountName.Name = "txtAccountName"
        Me.txtAccountName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccountName.Size = New System.Drawing.Size(337, 20)
        Me.txtAccountName.TabIndex = 5
        Me.txtAccountName.TabStop = False
        '
        'txtAccountCode
        '
        Me.txtAccountCode.AcceptsReturn = True
        Me.txtAccountCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccountCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccountCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccountCode.Location = New System.Drawing.Point(99, 16)
        Me.txtAccountCode.MaxLength = 255
        Me.txtAccountCode.Name = "txtAccountCode"
        Me.txtAccountCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccountCode.Size = New System.Drawing.Size(209, 20)
        Me.txtAccountCode.TabIndex = 3
        '
        'lblAccountName
        '
        Me.lblAccountName.AutoSize = True
        Me.lblAccountName.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountName.Location = New System.Drawing.Point(8, 52)
        Me.lblAccountName.Name = "lblAccountName"
        Me.lblAccountName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountName.Size = New System.Drawing.Size(81, 13)
        Me.lblAccountName.TabIndex = 4
        Me.lblAccountName.Text = "Account Name:"
        '
        'lblAccount
        '
        Me.lblAccount.AutoSize = True
        Me.lblAccount.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccount.Location = New System.Drawing.Point(11, 20)
        Me.lblAccount.Name = "lblAccount"
        Me.lblAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccount.Size = New System.Drawing.Size(78, 13)
        Me.lblAccount.TabIndex = 2
        Me.lblAccount.Text = "Account Code:"
        '
        'cmdNewSearch
        '
        Me.cmdNewSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewSearch.Location = New System.Drawing.Point(688, 56)
        Me.cmdNewSearch.Name = "cmdNewSearch"
        Me.cmdNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewSearch.Size = New System.Drawing.Size(79, 23)
        Me.cmdNewSearch.TabIndex = 11
        Me.cmdNewSearch.Text = "&New Search"
        Me.cmdNewSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNewSearch.UseVisualStyleBackColor = False
        '
        'cmdFindNow
        '
        Me.cmdFindNow.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindNow.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindNow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindNow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindNow.Location = New System.Drawing.Point(688, 24)
        Me.cmdFindNow.Name = "cmdFindNow"
        Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindNow.Size = New System.Drawing.Size(79, 23)
        Me.cmdFindNow.TabIndex = 10
        Me.cmdFindNow.Text = "&Find Now"
        Me.cmdFindNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindNow.UseVisualStyleBackColor = False
        '
        'fraUnallocatedClaimPayments
        '
        Me.fraUnallocatedClaimPayments.BackColor = System.Drawing.SystemColors.Control
        Me.fraUnallocatedClaimPayments.Controls.Add(Me.cmdSettleAll)
        Me.fraUnallocatedClaimPayments.Controls.Add(Me.cmdPayAsOne)
        Me.fraUnallocatedClaimPayments.Controls.Add(Me.cmdSelectAll)
        Me.fraUnallocatedClaimPayments.Controls.Add(Me.cmdPayIndividually)
        Me.fraUnallocatedClaimPayments.Controls.Add(Me.lvwClaimPayments)
        Me.fraUnallocatedClaimPayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraUnallocatedClaimPayments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraUnallocatedClaimPayments.Location = New System.Drawing.Point(4, 112)
        Me.fraUnallocatedClaimPayments.Name = "fraUnallocatedClaimPayments"
        Me.fraUnallocatedClaimPayments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraUnallocatedClaimPayments.Size = New System.Drawing.Size(777, 441)
        Me.fraUnallocatedClaimPayments.TabIndex = 12
        Me.fraUnallocatedClaimPayments.TabStop = False
        Me.fraUnallocatedClaimPayments.Text = "Claim Payments"
        '
        'cmdSettleAll
        '
        Me.cmdSettleAll.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSettleAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSettleAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSettleAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSettleAll.Location = New System.Drawing.Point(462, 408)
        Me.cmdSettleAll.Name = "cmdSettleAll"
        Me.cmdSettleAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSettleAll.Size = New System.Drawing.Size(145, 25)
        Me.cmdSettleAll.TabIndex = 17
        Me.cmdSettleAll.Text = "S&ettle All Selected"
        Me.cmdSettleAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSettleAll.UseVisualStyleBackColor = False
        '
        'cmdSelectAll
        '
        Me.cmdSelectAll.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelectAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelectAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSelectAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelectAll.Location = New System.Drawing.Point(312, 408)
        Me.cmdSelectAll.Name = "cmdSelectAll"
        Me.cmdSelectAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelectAll.Size = New System.Drawing.Size(145, 25)
        Me.cmdSelectAll.TabIndex = 16
        Me.cmdSelectAll.Text = "Select &All Payments"
        Me.cmdSelectAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSelectAll.UseVisualStyleBackColor = False
        '
        'lvwClaimPayments
        '
        Me.lvwClaimPayments.BackColor = System.Drawing.SystemColors.Window
        Me.lvwClaimPayments.CheckBoxes = True
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwClaimPayments, "")
        Me.lvwClaimPayments.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwClaimPayments_ColumnHeader_1, Me._lvwClaimPayments_ColumnHeader_2, Me._lvwClaimPayments_ColumnHeader_3, Me._lvwClaimPayments_ColumnHeader_4, Me._lvwClaimPayments_ColumnHeader_5, Me._lvwClaimPayments_ColumnHeader_6})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwClaimPayments, False)
        Me.lvwClaimPayments.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwClaimPayments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwClaimPayments.FullRowSelect = True
        Me.lvwClaimPayments.GridLines = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwClaimPayments, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwClaimPayments, "")
        Me.lvwClaimPayments.Location = New System.Drawing.Point(8, 16)
        Me.lvwClaimPayments.Name = "lvwClaimPayments"
        Me.lvwClaimPayments.Size = New System.Drawing.Size(761, 385)
        Me.listViewHelper1.SetSmallIcons(Me.lvwClaimPayments, "")
        Me.listViewHelper1.SetSorted(Me.lvwClaimPayments, False)
        Me.listViewHelper1.SetSortKey(Me.lvwClaimPayments, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwClaimPayments, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwClaimPayments.TabIndex = 13
        Me.lvwClaimPayments.UseCompatibleStateImageBehavior = False
        Me.lvwClaimPayments.View = System.Windows.Forms.View.Details
        '
        '_lvwClaimPayments_ColumnHeader_1
        '
        Me._lvwClaimPayments_ColumnHeader_1.Width = 21
        '
        '_lvwClaimPayments_ColumnHeader_2
        '
        Me._lvwClaimPayments_ColumnHeader_2.Text = "No"
        Me._lvwClaimPayments_ColumnHeader_2.Width = 97
        '
        '_lvwClaimPayments_ColumnHeader_3
        '
        Me._lvwClaimPayments_ColumnHeader_3.Text = "Date"
        Me._lvwClaimPayments_ColumnHeader_3.Width = 97
        '
        '_lvwClaimPayments_ColumnHeader_4
        '
        Me._lvwClaimPayments_ColumnHeader_4.Text = "Amount"
        Me._lvwClaimPayments_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwClaimPayments_ColumnHeader_4.Width = 97
        '
        '_lvwClaimPayments_ColumnHeader_5
        '
        Me._lvwClaimPayments_ColumnHeader_5.Text = "Claim Reference"
        Me._lvwClaimPayments_ColumnHeader_5.Width = 97
        '
        '_lvwClaimPayments_ColumnHeader_6
        '
        Me._lvwClaimPayments_ColumnHeader_6.Text = "Claim Payment Currency"
        Me._lvwClaimPayments_ColumnHeader_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwClaimPayments_ColumnHeader_6.Width = 97
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(787, 588)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.fraPayee)
        Me.Controls.Add(Me.fraUnallocatedClaimPayments)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 17)
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Claims Payment Processing"
        Me.fraPayee.ResumeLayout(False)
        Me.fraDateOfPayment.ResumeLayout(False)
        Me.fraDateOfPayment.PerformLayout()
        Me.fraAccountSearch.ResumeLayout(False)
        Me.fraAccountSearch.PerformLayout()
        Me.fraUnallocatedClaimPayments.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class