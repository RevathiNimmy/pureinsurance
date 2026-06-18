<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReceiptImport
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
	Public WithEvents cmdFixExpected As System.Windows.Forms.Button
	Public WithEvents txtReceiptTotal As System.Windows.Forms.TextBox
	Public WithEvents txtTotalRecords As System.Windows.Forms.TextBox
	Public WithEvents txtInvalidRecords As System.Windows.Forms.TextBox
	Public WithEvents txtDate As System.Windows.Forms.TextBox
	Public WithEvents txtCurrency As System.Windows.Forms.TextBox
	Public WithEvents txtBatchReference As System.Windows.Forms.TextBox
	Public WithEvents txtExpectedRecords As System.Windows.Forms.TextBox
	Public WithEvents cmdLookupAccount As System.Windows.Forms.Button
	Public WithEvents cmdApply As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Private WithEvents _lvwReceipts_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwReceipts_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwReceipts_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwReceipts_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwReceipts_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwReceipts_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwReceipts_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwReceipts As System.Windows.Forms.ListView
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents txtExpectedTotal As System.Windows.Forms.TextBox
	Public WithEvents txtBankAccount As System.Windows.Forms.TextBox
	Public WithEvents uctAnchor1 As uSIRCommonControls.uctAnchor
	Public WithEvents lblReceiptTotal As System.Windows.Forms.Label
	Public WithEvents lblTotalRecords As System.Windows.Forms.Label
	Public WithEvents lblExpectedRecords As System.Windows.Forms.Label
	Public WithEvents lblExpectedTotal As System.Windows.Forms.Label
	Public WithEvents lblBankAccount As System.Windows.Forms.Label
	Public WithEvents lblBatchReference As System.Windows.Forms.Label
	Public WithEvents lblInvalidRecords As System.Windows.Forms.Label
	Public WithEvents lblDate As System.Windows.Forms.Label
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReceiptImport))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdFixExpected = New System.Windows.Forms.Button
		Me.txtReceiptTotal = New System.Windows.Forms.TextBox
		Me.txtTotalRecords = New System.Windows.Forms.TextBox
		Me.txtInvalidRecords = New System.Windows.Forms.TextBox
		Me.txtDate = New System.Windows.Forms.TextBox
		Me.txtCurrency = New System.Windows.Forms.TextBox
		Me.txtBatchReference = New System.Windows.Forms.TextBox
		Me.txtExpectedRecords = New System.Windows.Forms.TextBox
		Me.cmdLookupAccount = New System.Windows.Forms.Button
		Me.cmdApply = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.lvwReceipts = New System.Windows.Forms.ListView
		Me._lvwReceipts_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwReceipts_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvwReceipts_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me._lvwReceipts_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
		Me._lvwReceipts_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
		Me._lvwReceipts_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
		Me._lvwReceipts_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.txtExpectedTotal = New System.Windows.Forms.TextBox
		Me.txtBankAccount = New System.Windows.Forms.TextBox
		Me.uctAnchor1 = New uSIRCommonControls.uctAnchor
		Me.lblReceiptTotal = New System.Windows.Forms.Label
		Me.lblTotalRecords = New System.Windows.Forms.Label
		Me.lblExpectedRecords = New System.Windows.Forms.Label
		Me.lblExpectedTotal = New System.Windows.Forms.Label
		Me.lblBankAccount = New System.Windows.Forms.Label
		Me.lblBatchReference = New System.Windows.Forms.Label
		Me.lblInvalidRecords = New System.Windows.Forms.Label
		Me.lblDate = New System.Windows.Forms.Label
		Me.lblCurrency = New System.Windows.Forms.Label
		Me.lvwReceipts.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdFixExpected
		' 
		Me.cmdFixExpected.BackColor = System.Drawing.SystemColors.Control
		Me.cmdFixExpected.CausesValidation = True
		Me.cmdFixExpected.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdFixExpected.Enabled = True
		Me.cmdFixExpected.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdFixExpected.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdFixExpected.Location = New System.Drawing.Point(162, 464)
		Me.cmdFixExpected.Name = "cmdFixExpected"
		Me.cmdFixExpected.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdFixExpected.Size = New System.Drawing.Size(150, 22)
		Me.cmdFixExpected.TabIndex = 23
		Me.cmdFixExpected.TabStop = True
		Me.cmdFixExpected.Text = "Fix Expected Values"
		Me.cmdFixExpected.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdFixExpected.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtReceiptTotal
		' 
		Me.txtReceiptTotal.AcceptsReturn = True
		Me.txtReceiptTotal.AutoSize = False
		Me.txtReceiptTotal.BackColor = System.Drawing.SystemColors.Control
		Me.txtReceiptTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtReceiptTotal.CausesValidation = True
		Me.txtReceiptTotal.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtReceiptTotal.Enabled = True
		Me.txtReceiptTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtReceiptTotal.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtReceiptTotal.HideSelection = True
		Me.txtReceiptTotal.Location = New System.Drawing.Point(118, 60)
		Me.txtReceiptTotal.MaxLength = 0
		Me.txtReceiptTotal.Multiline = False
		Me.txtReceiptTotal.Name = "txtReceiptTotal"
		Me.txtReceiptTotal.ReadOnly = True
		Me.txtReceiptTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtReceiptTotal.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtReceiptTotal.Size = New System.Drawing.Size(180, 21)
		Me.txtReceiptTotal.TabIndex = 21
		Me.txtReceiptTotal.TabStop = True
		Me.txtReceiptTotal.Text = "#,##0.00"
		Me.txtReceiptTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.txtReceiptTotal.Visible = True
		' 
		' txtTotalRecords
		' 
		Me.txtTotalRecords.AcceptsReturn = True
		Me.txtTotalRecords.AutoSize = False
		Me.txtTotalRecords.BackColor = System.Drawing.SystemColors.Control
		Me.txtTotalRecords.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtTotalRecords.CausesValidation = True
		Me.txtTotalRecords.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtTotalRecords.Enabled = True
		Me.txtTotalRecords.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtTotalRecords.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtTotalRecords.HideSelection = True
		Me.txtTotalRecords.Location = New System.Drawing.Point(118, 86)
		Me.txtTotalRecords.MaxLength = 0
		Me.txtTotalRecords.Multiline = False
		Me.txtTotalRecords.Name = "txtTotalRecords"
		Me.txtTotalRecords.ReadOnly = True
		Me.txtTotalRecords.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTotalRecords.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTotalRecords.Size = New System.Drawing.Size(80, 21)
		Me.txtTotalRecords.TabIndex = 19
		Me.txtTotalRecords.TabStop = True
		Me.txtTotalRecords.Text = "#0"
		Me.txtTotalRecords.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.txtTotalRecords.Visible = True
		' 
		' txtInvalidRecords
		' 
		Me.txtInvalidRecords.AcceptsReturn = True
		Me.txtInvalidRecords.AutoSize = False
		Me.txtInvalidRecords.BackColor = System.Drawing.SystemColors.Control
		Me.txtInvalidRecords.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtInvalidRecords.CausesValidation = True
		Me.txtInvalidRecords.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtInvalidRecords.Enabled = True
		Me.txtInvalidRecords.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtInvalidRecords.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtInvalidRecords.HideSelection = True
		Me.txtInvalidRecords.Location = New System.Drawing.Point(530, 86)
		Me.txtInvalidRecords.MaxLength = 0
		Me.txtInvalidRecords.Multiline = False
		Me.txtInvalidRecords.Name = "txtInvalidRecords"
		Me.txtInvalidRecords.ReadOnly = True
		Me.txtInvalidRecords.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtInvalidRecords.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtInvalidRecords.Size = New System.Drawing.Size(80, 21)
		Me.txtInvalidRecords.TabIndex = 14
		Me.txtInvalidRecords.TabStop = True
		Me.txtInvalidRecords.Text = "#0"
		Me.txtInvalidRecords.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.txtInvalidRecords.Visible = True
		' 
		' txtDate
		' 
		Me.txtDate.AcceptsReturn = True
		Me.txtDate.AutoSize = False
		Me.txtDate.BackColor = System.Drawing.SystemColors.Control
		Me.txtDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDate.CausesValidation = True
		Me.txtDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDate.Enabled = True
		Me.txtDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDate.HideSelection = True
		Me.txtDate.Location = New System.Drawing.Point(510, 8)
		Me.txtDate.MaxLength = 0
		Me.txtDate.Multiline = False
		Me.txtDate.Name = "txtDate"
		Me.txtDate.ReadOnly = True
		Me.txtDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDate.Size = New System.Drawing.Size(100, 21)
		Me.txtDate.TabIndex = 13
		Me.txtDate.TabStop = True
		Me.txtDate.Text = "30/02/2005"
		Me.txtDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDate.Visible = True
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
		Me.txtCurrency.Location = New System.Drawing.Point(510, 34)
		Me.txtCurrency.MaxLength = 0
		Me.txtCurrency.Multiline = False
		Me.txtCurrency.Name = "txtCurrency"
		Me.txtCurrency.ReadOnly = True
		Me.txtCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtCurrency.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtCurrency.Size = New System.Drawing.Size(100, 21)
		Me.txtCurrency.TabIndex = 12
		Me.txtCurrency.TabStop = True
		Me.txtCurrency.Text = "GBP"
		Me.txtCurrency.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtCurrency.Visible = True
		' 
		' txtBatchReference
		' 
		Me.txtBatchReference.AcceptsReturn = True
		Me.txtBatchReference.AutoSize = False
		Me.txtBatchReference.BackColor = System.Drawing.SystemColors.Control
		Me.txtBatchReference.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtBatchReference.CausesValidation = True
		Me.txtBatchReference.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtBatchReference.Enabled = True
		Me.txtBatchReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtBatchReference.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtBatchReference.HideSelection = True
		Me.txtBatchReference.Location = New System.Drawing.Point(118, 8)
		Me.txtBatchReference.MaxLength = 0
		Me.txtBatchReference.Multiline = False
		Me.txtBatchReference.Name = "txtBatchReference"
		Me.txtBatchReference.ReadOnly = True
		Me.txtBatchReference.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtBatchReference.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtBatchReference.Size = New System.Drawing.Size(332, 21)
		Me.txtBatchReference.TabIndex = 11
		Me.txtBatchReference.TabStop = True
		Me.txtBatchReference.Text = "Batch ref here"
		Me.txtBatchReference.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtBatchReference.Visible = True
		' 
		' txtExpectedRecords
		' 
		Me.txtExpectedRecords.AcceptsReturn = True
		Me.txtExpectedRecords.AutoSize = False
		Me.txtExpectedRecords.BackColor = System.Drawing.SystemColors.Control
		Me.txtExpectedRecords.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtExpectedRecords.CausesValidation = True
		Me.txtExpectedRecords.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtExpectedRecords.Enabled = True
		Me.txtExpectedRecords.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtExpectedRecords.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtExpectedRecords.HideSelection = True
		Me.txtExpectedRecords.Location = New System.Drawing.Point(328, 86)
		Me.txtExpectedRecords.MaxLength = 0
		Me.txtExpectedRecords.Multiline = False
		Me.txtExpectedRecords.Name = "txtExpectedRecords"
		Me.txtExpectedRecords.ReadOnly = True
		Me.txtExpectedRecords.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtExpectedRecords.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtExpectedRecords.Size = New System.Drawing.Size(80, 21)
		Me.txtExpectedRecords.TabIndex = 9
		Me.txtExpectedRecords.TabStop = True
		Me.txtExpectedRecords.Text = "#0"
		Me.txtExpectedRecords.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.txtExpectedRecords.Visible = True
		' 
		' cmdLookupAccount
		' 
		Me.cmdLookupAccount.BackColor = System.Drawing.SystemColors.Control
		Me.cmdLookupAccount.CausesValidation = True
		Me.cmdLookupAccount.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdLookupAccount.Enabled = True
		Me.cmdLookupAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdLookupAccount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdLookupAccount.Location = New System.Drawing.Point(6, 464)
		Me.cmdLookupAccount.Name = "cmdLookupAccount"
		Me.cmdLookupAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdLookupAccount.Size = New System.Drawing.Size(150, 22)
		Me.cmdLookupAccount.TabIndex = 8
		Me.cmdLookupAccount.TabStop = True
		Me.cmdLookupAccount.Text = "Lookup Account Code"
		Me.cmdLookupAccount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdLookupAccount.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdApply
		' 
		Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
		Me.cmdApply.CausesValidation = True
		Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdApply.Enabled = True
		Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdApply.Location = New System.Drawing.Point(614, 464)
		Me.cmdApply.Name = "cmdApply"
		Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdApply.Size = New System.Drawing.Size(80, 22)
		Me.cmdApply.TabIndex = 7
		Me.cmdApply.TabStop = True
		Me.cmdApply.Text = "&Apply"
		Me.cmdApply.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(442, 464)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(80, 22)
		Me.cmdOK.TabIndex = 6
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&Import"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lvwReceipts
		' 
		Me.lvwReceipts.BackColor = System.Drawing.SystemColors.Window
		Me.lvwReceipts.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lvwReceipts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwReceipts.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwReceipts.FullRowSelect = True
		Me.lvwReceipts.GridLines = True
		Me.lvwReceipts.HideSelection = False
		Me.lvwReceipts.LabelEdit = False
		Me.lvwReceipts.LabelWrap = False
		Me.lvwReceipts.Location = New System.Drawing.Point(6, 114)
		Me.lvwReceipts.Name = "lvwReceipts"
		Me.lvwReceipts.Size = New System.Drawing.Size(688, 343)
		Me.lvwReceipts.TabIndex = 5
		Me.lvwReceipts.View = System.Windows.Forms.View.Details
		Me.lvwReceipts.Columns.Add(Me._lvwReceipts_ColumnHeader_1)
		Me.lvwReceipts.Columns.Add(Me._lvwReceipts_ColumnHeader_2)
		Me.lvwReceipts.Columns.Add(Me._lvwReceipts_ColumnHeader_3)
		Me.lvwReceipts.Columns.Add(Me._lvwReceipts_ColumnHeader_4)
		Me.lvwReceipts.Columns.Add(Me._lvwReceipts_ColumnHeader_5)
		Me.lvwReceipts.Columns.Add(Me._lvwReceipts_ColumnHeader_6)
		Me.lvwReceipts.Columns.Add(Me._lvwReceipts_ColumnHeader_7)
		' 
		' _lvwReceipts_ColumnHeader_1
		' 
		Me._lvwReceipts_ColumnHeader_1.Text = "Account Code"
		Me._lvwReceipts_ColumnHeader_1.Width = 161
		' 
		' _lvwReceipts_ColumnHeader_2
		' 
		Me._lvwReceipts_ColumnHeader_2.Text = "Amount"
		Me._lvwReceipts_ColumnHeader_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._lvwReceipts_ColumnHeader_2.Width = 121
		' 
		' _lvwReceipts_ColumnHeader_3
		' 
		Me._lvwReceipts_ColumnHeader_3.Text = "Media Type"
		Me._lvwReceipts_ColumnHeader_3.Width = 121
		' 
		' _lvwReceipts_ColumnHeader_4
		' 
		Me._lvwReceipts_ColumnHeader_4.Text = "Media Reference"
		Me._lvwReceipts_ColumnHeader_4.Width = 201
		' 
		' _lvwReceipts_ColumnHeader_5
		' 
		Me._lvwReceipts_ColumnHeader_5.Text = "Original Account"
		Me._lvwReceipts_ColumnHeader_5.Width = 161
		' 
		' _lvwReceipts_ColumnHeader_6
		' 
		Me._lvwReceipts_ColumnHeader_6.Text = "Policy Reference"
		Me._lvwReceipts_ColumnHeader_6.Width = 161
		' 
		' _lvwReceipts_ColumnHeader_7
		' 
		Me._lvwReceipts_ColumnHeader_7.Text = "Error message"
		Me._lvwReceipts_ColumnHeader_7.Width = 334
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(528, 464)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(80, 22)
		Me.cmdCancel.TabIndex = 4
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtExpectedTotal
		' 
		Me.txtExpectedTotal.AcceptsReturn = True
		Me.txtExpectedTotal.AutoSize = False
		Me.txtExpectedTotal.BackColor = System.Drawing.SystemColors.Control
		Me.txtExpectedTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtExpectedTotal.CausesValidation = True
		Me.txtExpectedTotal.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtExpectedTotal.Enabled = True
		Me.txtExpectedTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtExpectedTotal.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtExpectedTotal.HideSelection = True
		Me.txtExpectedTotal.Location = New System.Drawing.Point(430, 60)
		Me.txtExpectedTotal.MaxLength = 0
		Me.txtExpectedTotal.Multiline = False
		Me.txtExpectedTotal.Name = "txtExpectedTotal"
		Me.txtExpectedTotal.ReadOnly = True
		Me.txtExpectedTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtExpectedTotal.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtExpectedTotal.Size = New System.Drawing.Size(180, 21)
		Me.txtExpectedTotal.TabIndex = 3
		Me.txtExpectedTotal.TabStop = True
		Me.txtExpectedTotal.Text = "#,##0.00"
		Me.txtExpectedTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.txtExpectedTotal.Visible = True
		' 
		' txtBankAccount
		' 
		Me.txtBankAccount.AcceptsReturn = True
		Me.txtBankAccount.AutoSize = False
		Me.txtBankAccount.BackColor = System.Drawing.SystemColors.Control
		Me.txtBankAccount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtBankAccount.CausesValidation = True
		Me.txtBankAccount.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtBankAccount.Enabled = True
		Me.txtBankAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtBankAccount.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtBankAccount.HideSelection = True
		Me.txtBankAccount.Location = New System.Drawing.Point(118, 34)
		Me.txtBankAccount.MaxLength = 0
		Me.txtBankAccount.Multiline = False
		Me.txtBankAccount.Name = "txtBankAccount"
		Me.txtBankAccount.ReadOnly = True
		Me.txtBankAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtBankAccount.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtBankAccount.Size = New System.Drawing.Size(280, 21)
		Me.txtBankAccount.TabIndex = 1
		Me.txtBankAccount.TabStop = True
		Me.txtBankAccount.Text = "My bank name here"
		Me.txtBankAccount.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtBankAccount.Visible = True
		' 
		' uctAnchor1
		' 
		Me.uctAnchor1.Location = New System.Drawing.Point(-46, 0)
		Me.uctAnchor1.Name = "uctAnchor1"
		' 
		' lblReceiptTotal
		' 
		Me.lblReceiptTotal.AutoSize = True
		Me.lblReceiptTotal.BackColor = System.Drawing.SystemColors.Control
		Me.lblReceiptTotal.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblReceiptTotal.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblReceiptTotal.Enabled = True
		Me.lblReceiptTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblReceiptTotal.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblReceiptTotal.Location = New System.Drawing.Point(8, 62)
		Me.lblReceiptTotal.Name = "lblReceiptTotal"
		Me.lblReceiptTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblReceiptTotal.Size = New System.Drawing.Size(79, 13)
		Me.lblReceiptTotal.TabIndex = 22
		Me.lblReceiptTotal.Text = "Receipt Total:"
		Me.lblReceiptTotal.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblReceiptTotal.UseMnemonic = True
		Me.lblReceiptTotal.Visible = True
		' 
		' lblTotalRecords
		' 
		Me.lblTotalRecords.AutoSize = True
		Me.lblTotalRecords.BackColor = System.Drawing.SystemColors.Control
		Me.lblTotalRecords.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTotalRecords.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTotalRecords.Enabled = True
		Me.lblTotalRecords.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTotalRecords.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTotalRecords.Location = New System.Drawing.Point(8, 89)
		Me.lblTotalRecords.Name = "lblTotalRecords"
		Me.lblTotalRecords.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTotalRecords.Size = New System.Drawing.Size(83, 13)
		Me.lblTotalRecords.TabIndex = 20
		Me.lblTotalRecords.Text = "Total Records:"
		Me.lblTotalRecords.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTotalRecords.UseMnemonic = True
		Me.lblTotalRecords.Visible = True
		' 
		' lblExpectedRecords
		' 
		Me.lblExpectedRecords.AutoSize = True
		Me.lblExpectedRecords.BackColor = System.Drawing.SystemColors.Control
		Me.lblExpectedRecords.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblExpectedRecords.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblExpectedRecords.Enabled = True
		Me.lblExpectedRecords.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblExpectedRecords.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblExpectedRecords.Location = New System.Drawing.Point(212, 89)
		Me.lblExpectedRecords.Name = "lblExpectedRecords"
		Me.lblExpectedRecords.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblExpectedRecords.Size = New System.Drawing.Size(107, 13)
		Me.lblExpectedRecords.TabIndex = 18
		Me.lblExpectedRecords.Text = "Expected Records:"
		Me.lblExpectedRecords.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblExpectedRecords.UseMnemonic = True
		Me.lblExpectedRecords.Visible = True
		' 
		' lblExpectedTotal
		' 
		Me.lblExpectedTotal.AutoSize = True
		Me.lblExpectedTotal.BackColor = System.Drawing.SystemColors.Control
		Me.lblExpectedTotal.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblExpectedTotal.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblExpectedTotal.Enabled = True
		Me.lblExpectedTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblExpectedTotal.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblExpectedTotal.Location = New System.Drawing.Point(320, 62)
		Me.lblExpectedTotal.Name = "lblExpectedTotal"
		Me.lblExpectedTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblExpectedTotal.Size = New System.Drawing.Size(89, 13)
		Me.lblExpectedTotal.TabIndex = 17
		Me.lblExpectedTotal.Text = "Expected Total:"
		Me.lblExpectedTotal.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblExpectedTotal.UseMnemonic = True
		Me.lblExpectedTotal.Visible = True
		' 
		' lblBankAccount
		' 
		Me.lblBankAccount.AutoSize = True
		Me.lblBankAccount.BackColor = System.Drawing.SystemColors.Control
		Me.lblBankAccount.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblBankAccount.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblBankAccount.Enabled = True
		Me.lblBankAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblBankAccount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblBankAccount.Location = New System.Drawing.Point(8, 37)
		Me.lblBankAccount.Name = "lblBankAccount"
		Me.lblBankAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblBankAccount.Size = New System.Drawing.Size(83, 13)
		Me.lblBankAccount.TabIndex = 16
		Me.lblBankAccount.Text = "Bank Account:"
		Me.lblBankAccount.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblBankAccount.UseMnemonic = True
		Me.lblBankAccount.Visible = True
		' 
		' lblBatchReference
		' 
		Me.lblBatchReference.AutoSize = True
		Me.lblBatchReference.BackColor = System.Drawing.SystemColors.Control
		Me.lblBatchReference.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblBatchReference.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblBatchReference.Enabled = True
		Me.lblBatchReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblBatchReference.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblBatchReference.Location = New System.Drawing.Point(8, 11)
		Me.lblBatchReference.Name = "lblBatchReference"
		Me.lblBatchReference.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblBatchReference.Size = New System.Drawing.Size(99, 13)
		Me.lblBatchReference.TabIndex = 15
		Me.lblBatchReference.Text = "Batch Reference:"
		Me.lblBatchReference.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblBatchReference.UseMnemonic = True
		Me.lblBatchReference.Visible = True
		' 
		' lblInvalidRecords
		' 
		Me.lblInvalidRecords.AutoSize = True
		Me.lblInvalidRecords.BackColor = System.Drawing.SystemColors.Control
		Me.lblInvalidRecords.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblInvalidRecords.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblInvalidRecords.Enabled = True
		Me.lblInvalidRecords.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblInvalidRecords.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblInvalidRecords.Location = New System.Drawing.Point(424, 89)
		Me.lblInvalidRecords.Name = "lblInvalidRecords"
		Me.lblInvalidRecords.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblInvalidRecords.Size = New System.Drawing.Size(94, 13)
		Me.lblInvalidRecords.TabIndex = 10
		Me.lblInvalidRecords.Text = "Invalid Records:"
		Me.lblInvalidRecords.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblInvalidRecords.UseMnemonic = True
		Me.lblInvalidRecords.Visible = True
		' 
		' lblDate
		' 
		Me.lblDate.AutoSize = True
		Me.lblDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDate.Enabled = True
		Me.lblDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDate.Location = New System.Drawing.Point(466, 11)
		Me.lblDate.Name = "lblDate"
		Me.lblDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDate.Size = New System.Drawing.Size(32, 13)
		Me.lblDate.TabIndex = 2
		Me.lblDate.Text = "Date:"
		Me.lblDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDate.UseMnemonic = True
		Me.lblDate.Visible = True
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
		Me.lblCurrency.Location = New System.Drawing.Point(406, 37)
		Me.lblCurrency.Name = "lblCurrency"
		Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCurrency.Size = New System.Drawing.Size(92, 13)
		Me.lblCurrency.TabIndex = 0
		Me.lblCurrency.Text = "Currency Code:"
		Me.lblCurrency.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCurrency.UseMnemonic = True
		Me.lblCurrency.Visible = True
		' 
		' frmReceiptImport
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(700, 492)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdFixExpected)
		Me.Controls.Add(Me.txtReceiptTotal)
		Me.Controls.Add(Me.txtTotalRecords)
		Me.Controls.Add(Me.txtInvalidRecords)
		Me.Controls.Add(Me.txtDate)
		Me.Controls.Add(Me.txtCurrency)
		Me.Controls.Add(Me.txtBatchReference)
		Me.Controls.Add(Me.txtExpectedRecords)
		Me.Controls.Add(Me.cmdLookupAccount)
		Me.Controls.Add(Me.cmdApply)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.lvwReceipts)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.txtExpectedTotal)
		Me.Controls.Add(Me.txtBankAccount)
		Me.Controls.Add(Me.uctAnchor1)
		Me.Controls.Add(Me.lblReceiptTotal)
		Me.Controls.Add(Me.lblTotalRecords)
		Me.Controls.Add(Me.lblExpectedRecords)
		Me.Controls.Add(Me.lblExpectedTotal)
		Me.Controls.Add(Me.lblBankAccount)
		Me.Controls.Add(Me.lblBatchReference)
		Me.Controls.Add(Me.lblInvalidRecords)
		Me.Controls.Add(Me.lblDate)
		Me.Controls.Add(Me.lblCurrency)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmReceiptImport.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmReceiptImport"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "Receipt Import Review"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.listViewHelper1.SetItemClickMethod(Me.lvwReceipts, "lvwReceipts_ItemClick")
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwReceipts, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.lvwReceipts.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class