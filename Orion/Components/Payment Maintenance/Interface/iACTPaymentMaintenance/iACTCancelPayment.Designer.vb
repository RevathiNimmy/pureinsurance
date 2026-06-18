<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCancelPayment
#Region "Windows Form Designer generated code "
	Public Sub New()
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
	Private WithEvents _lvwCancelPayment_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCancelPayment_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCancelPayment_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCancelPayment_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCancelPayment_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwCancelPayment As System.Windows.Forms.ListView
	Public WithEvents fraListCancelPayment As System.Windows.Forms.GroupBox
	Public WithEvents txtDocRef As System.Windows.Forms.TextBox
	Public WithEvents txtBankSortCode As System.Windows.Forms.TextBox
	Public WithEvents txtMediaType As System.Windows.Forms.TextBox
	Public WithEvents txtPaymentDate As System.Windows.Forms.TextBox
	Public WithEvents txtPolicyHolder As System.Windows.Forms.TextBox
	Public WithEvents txtBankAccNo As System.Windows.Forms.TextBox
	Public WithEvents txtMediaRef As System.Windows.Forms.TextBox
	Public WithEvents txtAmount As System.Windows.Forms.TextBox
	Public WithEvents txtClientCode As System.Windows.Forms.TextBox
	Public WithEvents cboCancelledReason As PMLookupControl.cboPMLookup
	Public WithEvents lblCancelledReason As System.Windows.Forms.Label
	Public WithEvents lblDocRef As System.Windows.Forms.Label
	Public WithEvents lblBankSortCode As System.Windows.Forms.Label
	Public WithEvents lblMediaType As System.Windows.Forms.Label
	Public WithEvents lblPaymentDate As System.Windows.Forms.Label
	Public WithEvents lblPolicyHolder As System.Windows.Forms.Label
	Public WithEvents lblBankAccountNo As System.Windows.Forms.Label
	Public WithEvents lblMediaReference As System.Windows.Forms.Label
	Public WithEvents lblAmount As System.Windows.Forms.Label
	Public WithEvents lblClientCode As System.Windows.Forms.Label
	Public WithEvents fraCtlCancelPayment As System.Windows.Forms.GroupBox
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.fraListCancelPayment = New System.Windows.Forms.GroupBox
        Me.lvwCancelPayment = New System.Windows.Forms.ListView
        Me._lvwCancelPayment_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwCancelPayment_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwCancelPayment_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwCancelPayment_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwCancelPayment_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me.fraCtlCancelPayment = New System.Windows.Forms.GroupBox
        Me.txtDocRef = New System.Windows.Forms.TextBox
        Me.txtBankSortCode = New System.Windows.Forms.TextBox
        Me.txtMediaType = New System.Windows.Forms.TextBox
        Me.txtPaymentDate = New System.Windows.Forms.TextBox
        Me.txtPolicyHolder = New System.Windows.Forms.TextBox
        Me.txtBankAccNo = New System.Windows.Forms.TextBox
        Me.txtMediaRef = New System.Windows.Forms.TextBox
        Me.txtAmount = New System.Windows.Forms.TextBox
        Me.txtClientCode = New System.Windows.Forms.TextBox
        Me.cboCancelledReason = New PMLookupControl.cboPMLookup
        Me.lblCancelledReason = New System.Windows.Forms.Label
        Me.lblDocRef = New System.Windows.Forms.Label
        Me.lblBankSortCode = New System.Windows.Forms.Label
        Me.lblMediaType = New System.Windows.Forms.Label
        Me.lblPaymentDate = New System.Windows.Forms.Label
        Me.lblPolicyHolder = New System.Windows.Forms.Label
        Me.lblBankAccountNo = New System.Windows.Forms.Label
        Me.lblMediaReference = New System.Windows.Forms.Label
        Me.lblAmount = New System.Windows.Forms.Label
        Me.lblClientCode = New System.Windows.Forms.Label
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.fraListCancelPayment.SuspendLayout()
        Me.fraCtlCancelPayment.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'fraListCancelPayment
        '
        Me.fraListCancelPayment.BackColor = System.Drawing.SystemColors.Control
        Me.fraListCancelPayment.Controls.Add(Me.lvwCancelPayment)
        Me.fraListCancelPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraListCancelPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraListCancelPayment.Location = New System.Drawing.Point(2, 170)
        Me.fraListCancelPayment.Name = "fraListCancelPayment"
        Me.fraListCancelPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraListCancelPayment.Size = New System.Drawing.Size(627, 195)
        Me.fraListCancelPayment.TabIndex = 14
        Me.fraListCancelPayment.TabStop = False
        Me.fraListCancelPayment.Text = "Allocations"
        '
        'lvwCancelPayment
        '
        Me.lvwCancelPayment.BackColor = System.Drawing.SystemColors.Window
        Me.lvwCancelPayment.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwCancelPayment, "")
        Me.lvwCancelPayment.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwCancelPayment_ColumnHeader_1, Me._lvwCancelPayment_ColumnHeader_2, Me._lvwCancelPayment_ColumnHeader_3, Me._lvwCancelPayment_ColumnHeader_4, Me._lvwCancelPayment_ColumnHeader_5})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwCancelPayment, False)
        Me.lvwCancelPayment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwCancelPayment.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwCancelPayment.FullRowSelect = True
        Me.lvwCancelPayment.GridLines = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwCancelPayment, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwCancelPayment, "")
        Me.lvwCancelPayment.Location = New System.Drawing.Point(8, 16)
        Me.lvwCancelPayment.MultiSelect = False
        Me.lvwCancelPayment.Name = "lvwCancelPayment"
        Me.lvwCancelPayment.Size = New System.Drawing.Size(609, 171)
        Me.listViewHelper1.SetSmallIcons(Me.lvwCancelPayment, "")
        Me.listViewHelper1.SetSorted(Me.lvwCancelPayment, False)
        Me.listViewHelper1.SetSortKey(Me.lvwCancelPayment, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwCancelPayment, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwCancelPayment.TabIndex = 12
        Me.lvwCancelPayment.UseCompatibleStateImageBehavior = False
        Me.lvwCancelPayment.View = System.Windows.Forms.View.Details
        '
        '_lvwCancelPayment_ColumnHeader_1
        '
        Me._lvwCancelPayment_ColumnHeader_1.Text = "Policy/Claim Number"
        Me._lvwCancelPayment_ColumnHeader_1.Width = 134
        '
        '_lvwCancelPayment_ColumnHeader_2
        '
        Me._lvwCancelPayment_ColumnHeader_2.Text = "Amount"
        Me._lvwCancelPayment_ColumnHeader_2.Width = 94
        '
        '_lvwCancelPayment_ColumnHeader_3
        '
        Me._lvwCancelPayment_ColumnHeader_3.Text = "Doc Ref"
        Me._lvwCancelPayment_ColumnHeader_3.Width = 101
        '
        '_lvwCancelPayment_ColumnHeader_4
        '
        Me._lvwCancelPayment_ColumnHeader_4.Text = "Doc Type"
        Me._lvwCancelPayment_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwCancelPayment_ColumnHeader_4.Width = 101
        '
        '_lvwCancelPayment_ColumnHeader_5
        '
        Me._lvwCancelPayment_ColumnHeader_5.Text = "Transaction Date"
        Me._lvwCancelPayment_ColumnHeader_5.Width = 114
        '
        'fraCtlCancelPayment
        '
        Me.fraCtlCancelPayment.BackColor = System.Drawing.SystemColors.Control
        Me.fraCtlCancelPayment.Controls.Add(Me.txtDocRef)
        Me.fraCtlCancelPayment.Controls.Add(Me.txtBankSortCode)
        Me.fraCtlCancelPayment.Controls.Add(Me.txtMediaType)
        Me.fraCtlCancelPayment.Controls.Add(Me.txtPaymentDate)
        Me.fraCtlCancelPayment.Controls.Add(Me.txtPolicyHolder)
        Me.fraCtlCancelPayment.Controls.Add(Me.txtBankAccNo)
        Me.fraCtlCancelPayment.Controls.Add(Me.txtMediaRef)
        Me.fraCtlCancelPayment.Controls.Add(Me.txtAmount)
        Me.fraCtlCancelPayment.Controls.Add(Me.txtClientCode)
        Me.fraCtlCancelPayment.Controls.Add(Me.cboCancelledReason)
        Me.fraCtlCancelPayment.Controls.Add(Me.lblCancelledReason)
        Me.fraCtlCancelPayment.Controls.Add(Me.lblDocRef)
        Me.fraCtlCancelPayment.Controls.Add(Me.lblBankSortCode)
        Me.fraCtlCancelPayment.Controls.Add(Me.lblMediaType)
        Me.fraCtlCancelPayment.Controls.Add(Me.lblPaymentDate)
        Me.fraCtlCancelPayment.Controls.Add(Me.lblPolicyHolder)
        Me.fraCtlCancelPayment.Controls.Add(Me.lblBankAccountNo)
        Me.fraCtlCancelPayment.Controls.Add(Me.lblMediaReference)
        Me.fraCtlCancelPayment.Controls.Add(Me.lblAmount)
        Me.fraCtlCancelPayment.Controls.Add(Me.lblClientCode)
        Me.fraCtlCancelPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCtlCancelPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraCtlCancelPayment.Location = New System.Drawing.Point(2, 0)
        Me.fraCtlCancelPayment.Name = "fraCtlCancelPayment"
        Me.fraCtlCancelPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraCtlCancelPayment.Size = New System.Drawing.Size(627, 169)
        Me.fraCtlCancelPayment.TabIndex = 13
        Me.fraCtlCancelPayment.TabStop = False
        '
        'txtDocRef
        '
        Me.txtDocRef.AcceptsReturn = True
        Me.txtDocRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtDocRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDocRef.Enabled = False
        Me.txtDocRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDocRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDocRef.Location = New System.Drawing.Point(262, 112)
        Me.txtDocRef.MaxLength = 0
        Me.txtDocRef.Name = "txtDocRef"
        Me.txtDocRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDocRef.Size = New System.Drawing.Size(153, 20)
        Me.txtDocRef.TabIndex = 11
        '
        'txtBankSortCode
        '
        Me.txtBankSortCode.AcceptsReturn = True
        Me.txtBankSortCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankSortCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankSortCode.Enabled = False
        Me.txtBankSortCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankSortCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankSortCode.Location = New System.Drawing.Point(460, 86)
        Me.txtBankSortCode.MaxLength = 0
        Me.txtBankSortCode.Name = "txtBankSortCode"
        Me.txtBankSortCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankSortCode.Size = New System.Drawing.Size(153, 20)
        Me.txtBankSortCode.TabIndex = 10
        '
        'txtMediaType
        '
        Me.txtMediaType.AcceptsReturn = True
        Me.txtMediaType.BackColor = System.Drawing.SystemColors.Window
        Me.txtMediaType.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMediaType.Enabled = False
        Me.txtMediaType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMediaType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMediaType.Location = New System.Drawing.Point(460, 62)
        Me.txtMediaType.MaxLength = 0
        Me.txtMediaType.Name = "txtMediaType"
        Me.txtMediaType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMediaType.Size = New System.Drawing.Size(153, 20)
        Me.txtMediaType.TabIndex = 8
        '
        'txtPaymentDate
        '
        Me.txtPaymentDate.AcceptsReturn = True
        Me.txtPaymentDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtPaymentDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPaymentDate.Enabled = False
        Me.txtPaymentDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaymentDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPaymentDate.Location = New System.Drawing.Point(460, 38)
        Me.txtPaymentDate.MaxLength = 0
        Me.txtPaymentDate.Name = "txtPaymentDate"
        Me.txtPaymentDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPaymentDate.Size = New System.Drawing.Size(153, 20)
        Me.txtPaymentDate.TabIndex = 6
        '
        'txtPolicyHolder
        '
        Me.txtPolicyHolder.AcceptsReturn = True
        Me.txtPolicyHolder.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolicyHolder.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicyHolder.Enabled = False
        Me.txtPolicyHolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicyHolder.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicyHolder.Location = New System.Drawing.Point(460, 14)
        Me.txtPolicyHolder.MaxLength = 0
        Me.txtPolicyHolder.Name = "txtPolicyHolder"
        Me.txtPolicyHolder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicyHolder.Size = New System.Drawing.Size(153, 20)
        Me.txtPolicyHolder.TabIndex = 4
        '
        'txtBankAccNo
        '
        Me.txtBankAccNo.AcceptsReturn = True
        Me.txtBankAccNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankAccNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankAccNo.Enabled = False
        Me.txtBankAccNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankAccNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankAccNo.Location = New System.Drawing.Point(118, 86)
        Me.txtBankAccNo.MaxLength = 0
        Me.txtBankAccNo.Name = "txtBankAccNo"
        Me.txtBankAccNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankAccNo.Size = New System.Drawing.Size(153, 20)
        Me.txtBankAccNo.TabIndex = 9
        '
        'txtMediaRef
        '
        Me.txtMediaRef.AcceptsReturn = True
        Me.txtMediaRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtMediaRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMediaRef.Enabled = False
        Me.txtMediaRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMediaRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMediaRef.Location = New System.Drawing.Point(118, 62)
        Me.txtMediaRef.MaxLength = 100
        Me.txtMediaRef.Name = "txtMediaRef"
        Me.txtMediaRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMediaRef.Size = New System.Drawing.Size(153, 20)
        Me.txtMediaRef.TabIndex = 7
        '
        'txtAmount
        '
        Me.txtAmount.AcceptsReturn = True
        Me.txtAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAmount.Enabled = False
        Me.txtAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAmount.Location = New System.Drawing.Point(118, 38)
        Me.txtAmount.MaxLength = 0
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAmount.Size = New System.Drawing.Size(153, 20)
        Me.txtAmount.TabIndex = 5
        '
        'txtClientCode
        '
        Me.txtClientCode.AcceptsReturn = True
        Me.txtClientCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtClientCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClientCode.Enabled = False
        Me.txtClientCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClientCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClientCode.Location = New System.Drawing.Point(118, 14)
        Me.txtClientCode.MaxLength = 0
        Me.txtClientCode.Name = "txtClientCode"
        Me.txtClientCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClientCode.Size = New System.Drawing.Size(153, 20)
        Me.txtClientCode.TabIndex = 3
        '
        'cboCancelledReason
        '
        Me.cboCancelledReason.DefaultItemId = 0
        Me.cboCancelledReason.FirstItem = ""
        Me.cboCancelledReason.ItemId = 0
        Me.cboCancelledReason.ListIndex = -1
        Me.cboCancelledReason.Location = New System.Drawing.Point(262, 136)
        Me.cboCancelledReason.Name = "cboCancelledReason"
        Me.cboCancelledReason.PMLookupProductFamily = 1
        Me.cboCancelledReason.SingleItemId = 0
        Me.cboCancelledReason.Size = New System.Drawing.Size(153, 21)
        Me.cboCancelledReason.Sorted = True
        Me.cboCancelledReason.TabIndex = 0
        Me.cboCancelledReason.TableName = "cashlistitem_reverse_reason"
        Me.cboCancelledReason.ToolTipText = ""
        Me.cboCancelledReason.WhereClause = ""
        '
        'lblCancelledReason
        '
        Me.lblCancelledReason.AutoSize = True
        Me.lblCancelledReason.BackColor = System.Drawing.SystemColors.Control
        Me.lblCancelledReason.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCancelledReason.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCancelledReason.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCancelledReason.Location = New System.Drawing.Point(110, 136)
        Me.lblCancelledReason.Name = "lblCancelledReason"
        Me.lblCancelledReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCancelledReason.Size = New System.Drawing.Size(125, 13)
        Me.lblCancelledReason.TabIndex = 24
        Me.lblCancelledReason.Text = "Cancelled Reason:"
        '
        'lblDocRef
        '
        Me.lblDocRef.AutoSize = True
        Me.lblDocRef.BackColor = System.Drawing.SystemColors.Control
        Me.lblDocRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDocRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDocRef.Location = New System.Drawing.Point(168, 112)
        Me.lblDocRef.Name = "lblDocRef"
        Me.lblDocRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDocRef.Size = New System.Drawing.Size(50, 13)
        Me.lblDocRef.TabIndex = 23
        Me.lblDocRef.Text = "Doc Ref:"
        '
        'lblBankSortCode
        '
        Me.lblBankSortCode.AutoSize = True
        Me.lblBankSortCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankSortCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankSortCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankSortCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankSortCode.Location = New System.Drawing.Point(330, 86)
        Me.lblBankSortCode.Name = "lblBankSortCode"
        Me.lblBankSortCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankSortCode.Size = New System.Drawing.Size(85, 13)
        Me.lblBankSortCode.TabIndex = 22
        Me.lblBankSortCode.Text = "Bank Sort Code:"
        '
        'lblMediaType
        '
        Me.lblMediaType.AutoSize = True
        Me.lblMediaType.BackColor = System.Drawing.SystemColors.Control
        Me.lblMediaType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMediaType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMediaType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMediaType.Location = New System.Drawing.Point(330, 62)
        Me.lblMediaType.Name = "lblMediaType"
        Me.lblMediaType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMediaType.Size = New System.Drawing.Size(66, 13)
        Me.lblMediaType.TabIndex = 21
        Me.lblMediaType.Text = "Media Type:"
        '
        'lblPaymentDate
        '
        Me.lblPaymentDate.AutoSize = True
        Me.lblPaymentDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentDate.Location = New System.Drawing.Point(330, 38)
        Me.lblPaymentDate.Name = "lblPaymentDate"
        Me.lblPaymentDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentDate.Size = New System.Drawing.Size(77, 13)
        Me.lblPaymentDate.TabIndex = 20
        Me.lblPaymentDate.Text = "Payment Date:"
        '
        'lblPolicyHolder
        '
        Me.lblPolicyHolder.AutoSize = True
        Me.lblPolicyHolder.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyHolder.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyHolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyHolder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyHolder.Location = New System.Drawing.Point(330, 14)
        Me.lblPolicyHolder.Name = "lblPolicyHolder"
        Me.lblPolicyHolder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyHolder.Size = New System.Drawing.Size(72, 13)
        Me.lblPolicyHolder.TabIndex = 19
        Me.lblPolicyHolder.Text = "Policy Holder:"
        '
        'lblBankAccountNo
        '
        Me.lblBankAccountNo.AutoSize = True
        Me.lblBankAccountNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankAccountNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankAccountNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankAccountNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankAccountNo.Location = New System.Drawing.Point(12, 86)
        Me.lblBankAccountNo.Name = "lblBankAccountNo"
        Me.lblBankAccountNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankAccountNo.Size = New System.Drawing.Size(74, 13)
        Me.lblBankAccountNo.TabIndex = 18
        Me.lblBankAccountNo.Text = "Bank Acc No:"
        '
        'lblMediaReference
        '
        Me.lblMediaReference.AutoSize = True
        Me.lblMediaReference.BackColor = System.Drawing.SystemColors.Control
        Me.lblMediaReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMediaReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMediaReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMediaReference.Location = New System.Drawing.Point(12, 62)
        Me.lblMediaReference.Name = "lblMediaReference"
        Me.lblMediaReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMediaReference.Size = New System.Drawing.Size(59, 13)
        Me.lblMediaReference.TabIndex = 17
        Me.lblMediaReference.Text = "Media Ref:"
        '
        'lblAmount
        '
        Me.lblAmount.AutoSize = True
        Me.lblAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAmount.Location = New System.Drawing.Point(12, 38)
        Me.lblAmount.Name = "lblAmount"
        Me.lblAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAmount.Size = New System.Drawing.Size(46, 13)
        Me.lblAmount.TabIndex = 16
        Me.lblAmount.Text = "Amount:"
        '
        'lblClientCode
        '
        Me.lblClientCode.AutoSize = True
        Me.lblClientCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientCode.Location = New System.Drawing.Point(12, 14)
        Me.lblClientCode.Name = "lblClientCode"
        Me.lblClientCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientCode.Size = New System.Drawing.Size(64, 13)
        Me.lblClientCode.TabIndex = 15
        Me.lblClientCode.Text = "Client Code:"
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(478, 376)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 1
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(556, 376)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'frmCancelPayment
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(633, 402)
        Me.Controls.Add(Me.fraListCancelPayment)
        Me.Controls.Add(Me.fraCtlCancelPayment)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCancelPayment"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Cancel Payment"
        Me.fraListCancelPayment.ResumeLayout(False)
        Me.fraCtlCancelPayment.ResumeLayout(False)
        Me.fraCtlCancelPayment.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class