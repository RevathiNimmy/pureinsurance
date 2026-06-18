<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwSearchDetailsReceipt_InitializeColumnKeys()
		lvwSearchDetailsPayment_InitializeColumnKeys()
		tabReceiptTabPreviousTab = tabReceiptTab.SelectedIndex
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
	Public WithEvents cmdView As System.Windows.Forms.Button
	Private WithEvents _lvwSearchDetailsPayment_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetailsPayment_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetailsPayment_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetailsPayment_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetailsPayment_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetailsPayment_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetailsPayment_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetailsPayment_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetailsPayment_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetailsPayment_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetailsPayment_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetailsPayment_ColumnHeader_12 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwSearchDetailsPayment As System.Windows.Forms.ListView
	Public WithEvents lblPMLookUpPaymentStatus As System.Windows.Forms.Label
	Public WithEvents lblPaymentBatchRef As System.Windows.Forms.Label
	Public WithEvents lblAmountPayment As System.Windows.Forms.Label
	Public WithEvents lblPMLookupPaymentMediaType As System.Windows.Forms.Label
	Public WithEvents lblPayeeName As System.Windows.Forms.Label
	Public WithEvents lblPaymentAccount As System.Windows.Forms.Label
	Public WithEvents lblChequeEFTNo As System.Windows.Forms.Label
	Public WithEvents lblPMLookUpPaymentType As System.Windows.Forms.Label
	Public WithEvents uctPaymentAccountLookUp As UserControls.AccountLookup
	Public WithEvents uctPMLookUpPaymentStatus As PMLookupControl.cboPMLookup
	Public WithEvents uctPMLookupPaymentMediaType As PMLookupControl.cboPMLookup
	Public WithEvents uctPMLookUpPaymentType As PMLookupControl.cboPMLookup
	Public WithEvents txtChequeEFTNo As System.Windows.Forms.TextBox
	Public WithEvents txtPaymentBatchReference As System.Windows.Forms.TextBox
	Public WithEvents txtAmountPayment As System.Windows.Forms.TextBox
	Public WithEvents txtPayeeName As System.Windows.Forms.TextBox
	Private WithEvents _tabPaymentTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabPaymentTab As System.Windows.Forms.TabControl
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdPrint As System.Windows.Forms.Button
	Public WithEvents cmdNewSearch As System.Windows.Forms.Button
	Public WithEvents cmdFindNow As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblPMLookupReceiptType As System.Windows.Forms.Label
	Public WithEvents lblMediaReference As System.Windows.Forms.Label
	Public WithEvents lblDateTo As System.Windows.Forms.Label
	Public WithEvents lblDateFrom As System.Windows.Forms.Label
	Public WithEvents lblCashDrawers As System.Windows.Forms.Label
	Public WithEvents lblPMLookupMediaType As System.Windows.Forms.Label
	Public WithEvents lblAmount As System.Windows.Forms.Label
	Public WithEvents lblReceiptBatchReference As System.Windows.Forms.Label
	Public WithEvents lblTheirReference As System.Windows.Forms.Label
	Public WithEvents lblReceiptNumber As System.Windows.Forms.Label
	Public WithEvents uctAccountLookup As UserControls.AccountLookup
	Public WithEvents cboPMLookupMediaType As PMLookupControl.cboPMLookup
	Public WithEvents uctPMLookupReceiptType As PMLookupControl.cboPMLookup
	Public WithEvents txtDateFrom As System.Windows.Forms.TextBox
	Public WithEvents txtDateTo As System.Windows.Forms.TextBox
	Public WithEvents txtAmount As System.Windows.Forms.TextBox
	Public WithEvents txtTheirReference As System.Windows.Forms.TextBox
	Public WithEvents txtReceiptNumber As System.Windows.Forms.TextBox
	Public WithEvents txtReceiptBatchReference As System.Windows.Forms.TextBox
	Public WithEvents txtMediaReference As System.Windows.Forms.TextBox
	Private WithEvents _tabReceiptTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabReceiptTab As System.Windows.Forms.TabControl
	Private WithEvents _stbStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents stbStatus As System.Windows.Forms.StatusStrip
	Private WithEvents _lvwSearchDetailsReceipt_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetailsReceipt_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetailsReceipt_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetailsReceipt_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetailsReceipt_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetailsReceipt_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetailsReceipt_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetailsReceipt_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetailsReceipt_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetailsReceipt_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetailsReceipt_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwSearchDetailsReceipt As System.Windows.Forms.ListView
	Public WithEvents ImgImage As System.Windows.Forms.PictureBox
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	Dim Private tabReceiptTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdView = New System.Windows.Forms.Button
		Me.lvwSearchDetailsPayment = New System.Windows.Forms.ListView
		Me._lvwSearchDetailsPayment_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetailsPayment_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetailsPayment_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetailsPayment_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetailsPayment_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetailsPayment_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetailsPayment_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetailsPayment_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetailsPayment_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetailsPayment_ColumnHeader_10 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetailsPayment_ColumnHeader_11 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetailsPayment_ColumnHeader_12 = New System.Windows.Forms.ColumnHeader
		Me.tabPaymentTab = New System.Windows.Forms.TabControl
		Me._tabPaymentTab_TabPage0 = New System.Windows.Forms.TabPage
		Me.lblPMLookUpPaymentStatus = New System.Windows.Forms.Label
		Me.lblPaymentBatchRef = New System.Windows.Forms.Label
		Me.lblAmountPayment = New System.Windows.Forms.Label
		Me.lblPMLookupPaymentMediaType = New System.Windows.Forms.Label
		Me.lblPayeeName = New System.Windows.Forms.Label
		Me.lblPaymentAccount = New System.Windows.Forms.Label
		Me.lblChequeEFTNo = New System.Windows.Forms.Label
		Me.lblPMLookUpPaymentType = New System.Windows.Forms.Label
		Me.uctPaymentAccountLookUp = New UserControls.AccountLookup
		Me.uctPMLookUpPaymentStatus = New PMLookupControl.cboPMLookup
		Me.uctPMLookupPaymentMediaType = New PMLookupControl.cboPMLookup
		Me.uctPMLookUpPaymentType = New PMLookupControl.cboPMLookup
		Me.txtChequeEFTNo = New System.Windows.Forms.TextBox
		Me.txtPaymentBatchReference = New System.Windows.Forms.TextBox
		Me.txtAmountPayment = New System.Windows.Forms.TextBox
		Me.txtPayeeName = New System.Windows.Forms.TextBox
		Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
		Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
		Me.dlgHelpFont = New System.Windows.Forms.FontDialog
		Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
		Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
		Me.cmdNavigate = New System.Windows.Forms.Button
		Me.cmdPrint = New System.Windows.Forms.Button
		Me.cmdNewSearch = New System.Windows.Forms.Button
		Me.cmdFindNow = New System.Windows.Forms.Button
		Me.cmdHelp = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.tabReceiptTab = New System.Windows.Forms.TabControl
		Me._tabReceiptTab_TabPage0 = New System.Windows.Forms.TabPage
		Me.lblPMLookupReceiptType = New System.Windows.Forms.Label
		Me.lblMediaReference = New System.Windows.Forms.Label
		Me.lblDateTo = New System.Windows.Forms.Label
		Me.lblDateFrom = New System.Windows.Forms.Label
		Me.lblCashDrawers = New System.Windows.Forms.Label
		Me.lblPMLookupMediaType = New System.Windows.Forms.Label
		Me.lblAmount = New System.Windows.Forms.Label
		Me.lblReceiptBatchReference = New System.Windows.Forms.Label
		Me.lblTheirReference = New System.Windows.Forms.Label
		Me.lblReceiptNumber = New System.Windows.Forms.Label
		Me.uctAccountLookup = New UserControls.AccountLookup
		Me.cboPMLookupMediaType = New PMLookupControl.cboPMLookup
		Me.uctPMLookupReceiptType = New PMLookupControl.cboPMLookup
		Me.txtDateFrom = New System.Windows.Forms.TextBox
		Me.txtDateTo = New System.Windows.Forms.TextBox
		Me.txtAmount = New System.Windows.Forms.TextBox
		Me.txtTheirReference = New System.Windows.Forms.TextBox
		Me.txtReceiptNumber = New System.Windows.Forms.TextBox
		Me.txtReceiptBatchReference = New System.Windows.Forms.TextBox
		Me.txtMediaReference = New System.Windows.Forms.TextBox
		Me.stbStatus = New System.Windows.Forms.StatusStrip
		Me._stbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
		Me.lvwSearchDetailsReceipt = New System.Windows.Forms.ListView
		Me._lvwSearchDetailsReceipt_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetailsReceipt_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetailsReceipt_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetailsReceipt_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetailsReceipt_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetailsReceipt_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetailsReceipt_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetailsReceipt_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetailsReceipt_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetailsReceipt_ColumnHeader_10 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetailsReceipt_ColumnHeader_11 = New System.Windows.Forms.ColumnHeader
		Me.ImgImage = New System.Windows.Forms.PictureBox
		Me.lvwSearchDetailsPayment.SuspendLayout()
		Me.tabPaymentTab.SuspendLayout()
		Me._tabPaymentTab_TabPage0.SuspendLayout()
		Me.tabReceiptTab.SuspendLayout()
		Me._tabReceiptTab_TabPage0.SuspendLayout()
		Me.stbStatus.SuspendLayout()
		Me.lvwSearchDetailsReceipt.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdView
		' 
		Me.cmdView.BackColor = System.Drawing.SystemColors.Control
		Me.cmdView.CausesValidation = True
		Me.cmdView.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdView.Enabled = True
		Me.cmdView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdView.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdView.Location = New System.Drawing.Point(88, 380)
		Me.cmdView.Name = "cmdView"
		Me.cmdView.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdView.Size = New System.Drawing.Size(73, 22)
		Me.cmdView.TabIndex = 48
		Me.cmdView.TabStop = True
		Me.cmdView.Tag = "CAP;821"
		Me.cmdView.Text = "*{View}"
		Me.cmdView.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.cmdView.Visible = False
		' 
		' lvwSearchDetailsPayment
		' 
		Me.lvwSearchDetailsPayment.BackColor = System.Drawing.SystemColors.Window
		Me.lvwSearchDetailsPayment.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lvwSearchDetailsPayment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwSearchDetailsPayment.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwSearchDetailsPayment.HideSelection = False
		Me.lvwSearchDetailsPayment.LabelEdit = False
		Me.lvwSearchDetailsPayment.LabelWrap = True
		Me.lvwSearchDetailsPayment.Location = New System.Drawing.Point(8, 204)
		Me.lvwSearchDetailsPayment.Name = "lvwSearchDetailsPayment"
		Me.lvwSearchDetailsPayment.Size = New System.Drawing.Size(633, 169)
		Me.lvwSearchDetailsPayment.TabIndex = 46
		Me.lvwSearchDetailsPayment.Tag = "CAP;809"
		Me.lvwSearchDetailsPayment.View = System.Windows.Forms.View.Details
		Me.lvwSearchDetailsPayment.Columns.Add(Me._lvwSearchDetailsPayment_ColumnHeader_1)
		Me.lvwSearchDetailsPayment.Columns.Add(Me._lvwSearchDetailsPayment_ColumnHeader_2)
		Me.lvwSearchDetailsPayment.Columns.Add(Me._lvwSearchDetailsPayment_ColumnHeader_3)
		Me.lvwSearchDetailsPayment.Columns.Add(Me._lvwSearchDetailsPayment_ColumnHeader_4)
		Me.lvwSearchDetailsPayment.Columns.Add(Me._lvwSearchDetailsPayment_ColumnHeader_5)
		Me.lvwSearchDetailsPayment.Columns.Add(Me._lvwSearchDetailsPayment_ColumnHeader_6)
		Me.lvwSearchDetailsPayment.Columns.Add(Me._lvwSearchDetailsPayment_ColumnHeader_7)
		Me.lvwSearchDetailsPayment.Columns.Add(Me._lvwSearchDetailsPayment_ColumnHeader_8)
		Me.lvwSearchDetailsPayment.Columns.Add(Me._lvwSearchDetailsPayment_ColumnHeader_9)
		Me.lvwSearchDetailsPayment.Columns.Add(Me._lvwSearchDetailsPayment_ColumnHeader_10)
		Me.lvwSearchDetailsPayment.Columns.Add(Me._lvwSearchDetailsPayment_ColumnHeader_11)
		Me.lvwSearchDetailsPayment.Columns.Add(Me._lvwSearchDetailsPayment_ColumnHeader_12)
		' 
		' _lvwSearchDetailsPayment_ColumnHeader_1
		' 
		Me._lvwSearchDetailsPayment_ColumnHeader_1.Tag = ""
		Me._lvwSearchDetailsPayment_ColumnHeader_1.Text = "*{Payment Number}"
		Me._lvwSearchDetailsPayment_ColumnHeader_1.Width = 117
		' 
		' _lvwSearchDetailsPayment_ColumnHeader_2
		' 
		Me._lvwSearchDetailsPayment_ColumnHeader_2.Tag = ""
		Me._lvwSearchDetailsPayment_ColumnHeader_2.Text = "*{Payee Name}"
		Me._lvwSearchDetailsPayment_ColumnHeader_2.Width = 97
		' 
		' _lvwSearchDetailsPayment_ColumnHeader_3
		' 
		Me._lvwSearchDetailsPayment_ColumnHeader_3.Tag = ""
		Me._lvwSearchDetailsPayment_ColumnHeader_3.Text = "*{Account Number}"
		Me._lvwSearchDetailsPayment_ColumnHeader_3.Width = 117
		' 
		' _lvwSearchDetailsPayment_ColumnHeader_4
		' 
		Me._lvwSearchDetailsPayment_ColumnHeader_4.Tag = ""
		Me._lvwSearchDetailsPayment_ColumnHeader_4.Text = "*{Payment Type}"
		Me._lvwSearchDetailsPayment_ColumnHeader_4.Width = 94
		' 
		' _lvwSearchDetailsPayment_ColumnHeader_5
		' 
		Me._lvwSearchDetailsPayment_ColumnHeader_5.Tag = ""
		Me._lvwSearchDetailsPayment_ColumnHeader_5.Text = "*{Payment Method}"
		Me._lvwSearchDetailsPayment_ColumnHeader_5.Width = 87
		' 
		' _lvwSearchDetailsPayment_ColumnHeader_6
		' 
		Me._lvwSearchDetailsPayment_ColumnHeader_6.Tag = ""
		Me._lvwSearchDetailsPayment_ColumnHeader_6.Text = "*{Cheque/EFT Number}"
		Me._lvwSearchDetailsPayment_ColumnHeader_6.Width = 147
		' 
		' _lvwSearchDetailsPayment_ColumnHeader_7
		' 
		Me._lvwSearchDetailsPayment_ColumnHeader_7.Tag = ""
		Me._lvwSearchDetailsPayment_ColumnHeader_7.Text = "*{Amount}"
		Me._lvwSearchDetailsPayment_ColumnHeader_7.Width = 67
		' 
		' _lvwSearchDetailsPayment_ColumnHeader_8
		' 
		Me._lvwSearchDetailsPayment_ColumnHeader_8.Tag = ""
		Me._lvwSearchDetailsPayment_ColumnHeader_8.Text = "*{Payment Status}"
		Me._lvwSearchDetailsPayment_ColumnHeader_8.Width = 117
		' 
		' _lvwSearchDetailsPayment_ColumnHeader_9
		' 
		Me._lvwSearchDetailsPayment_ColumnHeader_9.Tag = "DATESORT"
		Me._lvwSearchDetailsPayment_ColumnHeader_9.Text = "*{Date Presented}"
		Me._lvwSearchDetailsPayment_ColumnHeader_9.Width = 117
		' 
		' _lvwSearchDetailsPayment_ColumnHeader_10
		' 
		Me._lvwSearchDetailsPayment_ColumnHeader_10.Tag = ""
		Me._lvwSearchDetailsPayment_ColumnHeader_10.Text = "*{Batch Reference}"
		Me._lvwSearchDetailsPayment_ColumnHeader_10.Width = 117
		' 
		' _lvwSearchDetailsPayment_ColumnHeader_11
		' 
		Me._lvwSearchDetailsPayment_ColumnHeader_11.Tag = ""
		Me._lvwSearchDetailsPayment_ColumnHeader_11.Text = "*{Username}"
		Me._lvwSearchDetailsPayment_ColumnHeader_11.Width = 97
		' 
		' _lvwSearchDetailsPayment_ColumnHeader_12
		' 
		Me._lvwSearchDetailsPayment_ColumnHeader_12.Tag = "HIDDEN"
		Me._lvwSearchDetailsPayment_ColumnHeader_12.Text = "*{CashListId}"
		Me._lvwSearchDetailsPayment_ColumnHeader_12.Width = 0
		' 
		' tabPaymentTab
		' 
		Me.tabPaymentTab.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabPaymentTab.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabPaymentTab.Controls.Add(Me._tabPaymentTab_TabPage0)
		Me.tabPaymentTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabPaymentTab.ItemSize = New System.Drawing.Size(179, 18)
		Me.tabPaymentTab.Location = New System.Drawing.Point(8, 8)
		Me.tabPaymentTab.Multiline = True
		Me.tabPaymentTab.Name = "tabPaymentTab"
		Me.tabPaymentTab.Size = New System.Drawing.Size(545, 194)
		Me.tabPaymentTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabPaymentTab.TabIndex = 30
		Me.tabPaymentTab.Tag = "CAP;808"
		' 
		' _tabPaymentTab_TabPage0
		' 
		Me._tabPaymentTab_TabPage0.Controls.Add(Me.lblPMLookUpPaymentStatus)
		Me._tabPaymentTab_TabPage0.Controls.Add(Me.lblPaymentBatchRef)
		Me._tabPaymentTab_TabPage0.Controls.Add(Me.lblAmountPayment)
		Me._tabPaymentTab_TabPage0.Controls.Add(Me.lblPMLookupPaymentMediaType)
		Me._tabPaymentTab_TabPage0.Controls.Add(Me.lblPayeeName)
		Me._tabPaymentTab_TabPage0.Controls.Add(Me.lblPaymentAccount)
		Me._tabPaymentTab_TabPage0.Controls.Add(Me.lblChequeEFTNo)
		Me._tabPaymentTab_TabPage0.Controls.Add(Me.lblPMLookUpPaymentType)
		Me._tabPaymentTab_TabPage0.Controls.Add(Me.uctPaymentAccountLookUp)
		Me._tabPaymentTab_TabPage0.Controls.Add(Me.uctPMLookUpPaymentStatus)
		Me._tabPaymentTab_TabPage0.Controls.Add(Me.uctPMLookupPaymentMediaType)
		Me._tabPaymentTab_TabPage0.Controls.Add(Me.uctPMLookUpPaymentType)
		Me._tabPaymentTab_TabPage0.Controls.Add(Me.txtChequeEFTNo)
		Me._tabPaymentTab_TabPage0.Controls.Add(Me.txtPaymentBatchReference)
		Me._tabPaymentTab_TabPage0.Controls.Add(Me.txtAmountPayment)
		Me._tabPaymentTab_TabPage0.Controls.Add(Me.txtPayeeName)
		Me._tabPaymentTab_TabPage0.Text = "*{&1 - Drawer}"
		' 
		' lblPMLookUpPaymentStatus
		' 
		Me.lblPMLookUpPaymentStatus.AutoSize = False
		Me.lblPMLookUpPaymentStatus.BackColor = System.Drawing.SystemColors.Control
		Me.lblPMLookUpPaymentStatus.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPMLookUpPaymentStatus.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPMLookUpPaymentStatus.Enabled = True
		Me.lblPMLookUpPaymentStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPMLookUpPaymentStatus.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPMLookUpPaymentStatus.Location = New System.Drawing.Point(256, 96)
		Me.lblPMLookUpPaymentStatus.Name = "lblPMLookUpPaymentStatus"
		Me.lblPMLookUpPaymentStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPMLookUpPaymentStatus.Size = New System.Drawing.Size(101, 17)
		Me.lblPMLookUpPaymentStatus.TabIndex = 34
		Me.lblPMLookUpPaymentStatus.Tag = "CAP;806"
		Me.lblPMLookUpPaymentStatus.Text = "*{Payment Status:}"
		Me.lblPMLookUpPaymentStatus.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPMLookUpPaymentStatus.UseMnemonic = True
		Me.lblPMLookUpPaymentStatus.Visible = True
		' 
		' lblPaymentBatchRef
		' 
		Me.lblPaymentBatchRef.AutoSize = False
		Me.lblPaymentBatchRef.BackColor = System.Drawing.SystemColors.Control
		Me.lblPaymentBatchRef.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPaymentBatchRef.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPaymentBatchRef.Enabled = True
		Me.lblPaymentBatchRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPaymentBatchRef.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPaymentBatchRef.Location = New System.Drawing.Point(256, 132)
		Me.lblPaymentBatchRef.Name = "lblPaymentBatchRef"
		Me.lblPaymentBatchRef.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPaymentBatchRef.Size = New System.Drawing.Size(101, 17)
		Me.lblPaymentBatchRef.TabIndex = 37
		Me.lblPaymentBatchRef.Tag = "CAP;807"
		Me.lblPaymentBatchRef.Text = "*{Batch Reference:}"
		Me.lblPaymentBatchRef.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPaymentBatchRef.UseMnemonic = True
		Me.lblPaymentBatchRef.Visible = True
		' 
		' lblAmountPayment
		' 
		Me.lblAmountPayment.AutoSize = False
		Me.lblAmountPayment.BackColor = System.Drawing.SystemColors.Control
		Me.lblAmountPayment.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAmountPayment.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAmountPayment.Enabled = True
		Me.lblAmountPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAmountPayment.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAmountPayment.Location = New System.Drawing.Point(8, 132)
		Me.lblAmountPayment.Name = "lblAmountPayment"
		Me.lblAmountPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAmountPayment.Size = New System.Drawing.Size(101, 17)
		Me.lblAmountPayment.TabIndex = 39
		Me.lblAmountPayment.Tag = "CAP;306"
		Me.lblAmountPayment.Text = "*{Amount:}"
		Me.lblAmountPayment.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAmountPayment.UseMnemonic = True
		Me.lblAmountPayment.Visible = True
		' 
		' lblPMLookupPaymentMediaType
		' 
		Me.lblPMLookupPaymentMediaType.AutoSize = False
		Me.lblPMLookupPaymentMediaType.BackColor = System.Drawing.SystemColors.Control
		Me.lblPMLookupPaymentMediaType.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPMLookupPaymentMediaType.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPMLookupPaymentMediaType.Enabled = True
		Me.lblPMLookupPaymentMediaType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPMLookupPaymentMediaType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPMLookupPaymentMediaType.Location = New System.Drawing.Point(256, 58)
		Me.lblPMLookupPaymentMediaType.Name = "lblPMLookupPaymentMediaType"
		Me.lblPMLookupPaymentMediaType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPMLookupPaymentMediaType.Size = New System.Drawing.Size(129, 17)
		Me.lblPMLookupPaymentMediaType.TabIndex = 41
		Me.lblPMLookupPaymentMediaType.Tag = "CAP;805"
		Me.lblPMLookupPaymentMediaType.Text = "*{Media Type:}"
		Me.lblPMLookupPaymentMediaType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPMLookupPaymentMediaType.UseMnemonic = True
		Me.lblPMLookupPaymentMediaType.Visible = True
		' 
		' lblPayeeName
		' 
		Me.lblPayeeName.AutoSize = False
		Me.lblPayeeName.BackColor = System.Drawing.SystemColors.Control
		Me.lblPayeeName.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPayeeName.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPayeeName.Enabled = True
		Me.lblPayeeName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPayeeName.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPayeeName.Location = New System.Drawing.Point(8, 22)
		Me.lblPayeeName.Name = "lblPayeeName"
		Me.lblPayeeName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPayeeName.Size = New System.Drawing.Size(101, 17)
		Me.lblPayeeName.TabIndex = 42
		Me.lblPayeeName.Tag = "CAP;801"
		Me.lblPayeeName.Text = "*{Payee Name:}"
		Me.lblPayeeName.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPayeeName.UseMnemonic = True
		Me.lblPayeeName.Visible = True
		' 
		' lblPaymentAccount
		' 
		Me.lblPaymentAccount.AutoSize = False
		Me.lblPaymentAccount.BackColor = System.Drawing.SystemColors.Control
		Me.lblPaymentAccount.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPaymentAccount.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPaymentAccount.Enabled = True
		Me.lblPaymentAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPaymentAccount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPaymentAccount.Location = New System.Drawing.Point(256, 22)
		Me.lblPaymentAccount.Name = "lblPaymentAccount"
		Me.lblPaymentAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPaymentAccount.Size = New System.Drawing.Size(129, 17)
		Me.lblPaymentAccount.TabIndex = 43
		Me.lblPaymentAccount.Tag = "CAP;804"
		Me.lblPaymentAccount.Text = "*{Account Number:}"
		Me.lblPaymentAccount.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPaymentAccount.UseMnemonic = True
		Me.lblPaymentAccount.Visible = True
		' 
		' lblChequeEFTNo
		' 
		Me.lblChequeEFTNo.AutoSize = False
		Me.lblChequeEFTNo.BackColor = System.Drawing.SystemColors.Control
		Me.lblChequeEFTNo.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblChequeEFTNo.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblChequeEFTNo.Enabled = True
		Me.lblChequeEFTNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblChequeEFTNo.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblChequeEFTNo.Location = New System.Drawing.Point(8, 96)
		Me.lblChequeEFTNo.Name = "lblChequeEFTNo"
		Me.lblChequeEFTNo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblChequeEFTNo.Size = New System.Drawing.Size(101, 17)
		Me.lblChequeEFTNo.TabIndex = 44
		Me.lblChequeEFTNo.Tag = "CAP;803"
		Me.lblChequeEFTNo.Text = "*{Cheque/EFT No:}"
		Me.lblChequeEFTNo.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblChequeEFTNo.UseMnemonic = True
		Me.lblChequeEFTNo.Visible = True
		' 
		' lblPMLookUpPaymentType
		' 
		Me.lblPMLookUpPaymentType.AutoSize = False
		Me.lblPMLookUpPaymentType.BackColor = System.Drawing.SystemColors.Control
		Me.lblPMLookUpPaymentType.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPMLookUpPaymentType.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPMLookUpPaymentType.Enabled = True
		Me.lblPMLookUpPaymentType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPMLookUpPaymentType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPMLookUpPaymentType.Location = New System.Drawing.Point(8, 60)
		Me.lblPMLookUpPaymentType.Name = "lblPMLookUpPaymentType"
		Me.lblPMLookUpPaymentType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPMLookUpPaymentType.Size = New System.Drawing.Size(101, 17)
		Me.lblPMLookUpPaymentType.TabIndex = 45
		Me.lblPMLookUpPaymentType.Tag = "CAP;802"
		Me.lblPMLookUpPaymentType.Text = "*{Payment Type:}"
		Me.lblPMLookUpPaymentType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPMLookUpPaymentType.UseMnemonic = True
		Me.lblPMLookUpPaymentType.Visible = True
		' 
		' uctPaymentAccountLookUp
		' 
		Me.uctPaymentAccountLookUp.Location = New System.Drawing.Point(384, 20)
		Me.uctPaymentAccountLookUp.Name = "uctPaymentAccountLookUp"
		Me.uctPaymentAccountLookUp.OnlyUpdatableAccounts = True
		Me.uctPaymentAccountLookUp.Size = New System.Drawing.Size(141, 19)
		Me.uctPaymentAccountLookUp.TabIndex = 47
        'developer Guide No.15 
        'Me.uctPaymentAccountLookUp.WhatsThisHelpID = 16004
		' 
		' uctPMLookUpPaymentStatus
		' 
		Me.uctPMLookUpPaymentStatus.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.uctPMLookUpPaymentStatus.Location = New System.Drawing.Point(384, 92)
		Me.uctPMLookUpPaymentStatus.Name = "uctPMLookUpPaymentStatus"
		Me.uctPMLookUpPaymentStatus.Size = New System.Drawing.Size(141, 21)
		Me.uctPMLookUpPaymentStatus.Sorted = True
		Me.uctPMLookUpPaymentStatus.TabIndex = 36
        'Developer guide no.77
        Me.uctPMLookUpPaymentStatus.TableName = "CashListItem_Payment_Status"
		' 
		' uctPMLookupPaymentMediaType
		' 
		Me.uctPMLookupPaymentMediaType.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.uctPMLookupPaymentMediaType.Location = New System.Drawing.Point(384, 56)
		Me.uctPMLookupPaymentMediaType.Name = "uctPMLookupPaymentMediaType"
		Me.uctPMLookupPaymentMediaType.Size = New System.Drawing.Size(141, 21)
		Me.uctPMLookupPaymentMediaType.Sorted = True
		Me.uctPMLookupPaymentMediaType.TabIndex = 33
        'Developer guide no.77
        Me.uctPMLookupPaymentMediaType.TableName = "mediatype"
		Me.uctPMLookupPaymentMediaType.WhereClause = "is_payment=1 AND is_deleted=0"
		' 
		' uctPMLookUpPaymentType
		' 
		Me.uctPMLookUpPaymentType.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.uctPMLookUpPaymentType.Location = New System.Drawing.Point(108, 56)
		Me.uctPMLookUpPaymentType.Name = "uctPMLookUpPaymentType"
		Me.uctPMLookUpPaymentType.Size = New System.Drawing.Size(141, 21)
		Me.uctPMLookUpPaymentType.Sorted = True
		Me.uctPMLookUpPaymentType.TabIndex = 32
        'Developer guide no.77
        Me.uctPMLookUpPaymentType.TableName = "CashListItem_Payment_Type"
		' 
		' txtChequeEFTNo
		' 
		Me.txtChequeEFTNo.AcceptsReturn = True
		Me.txtChequeEFTNo.AutoSize = False
		Me.txtChequeEFTNo.BackColor = System.Drawing.SystemColors.Window
		Me.txtChequeEFTNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtChequeEFTNo.CausesValidation = True
		Me.txtChequeEFTNo.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtChequeEFTNo.Enabled = True
		Me.txtChequeEFTNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtChequeEFTNo.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtChequeEFTNo.HideSelection = True
		Me.txtChequeEFTNo.Location = New System.Drawing.Point(108, 92)
		Me.txtChequeEFTNo.MaxLength = 0
		Me.txtChequeEFTNo.Multiline = False
		Me.txtChequeEFTNo.Name = "txtChequeEFTNo"
		Me.txtChequeEFTNo.ReadOnly = False
		Me.txtChequeEFTNo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtChequeEFTNo.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtChequeEFTNo.Size = New System.Drawing.Size(141, 19)
		Me.txtChequeEFTNo.TabIndex = 35
		Me.txtChequeEFTNo.TabStop = True
		Me.txtChequeEFTNo.Tag = "F;"
		Me.txtChequeEFTNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtChequeEFTNo.Visible = True
		' 
		' txtPaymentBatchReference
		' 
		Me.txtPaymentBatchReference.AcceptsReturn = True
		Me.txtPaymentBatchReference.AutoSize = False
		Me.txtPaymentBatchReference.BackColor = System.Drawing.SystemColors.Window
		Me.txtPaymentBatchReference.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPaymentBatchReference.CausesValidation = True
		Me.txtPaymentBatchReference.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPaymentBatchReference.Enabled = True
		Me.txtPaymentBatchReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPaymentBatchReference.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPaymentBatchReference.HideSelection = True
		Me.txtPaymentBatchReference.Location = New System.Drawing.Point(384, 128)
		Me.txtPaymentBatchReference.MaxLength = 25
		Me.txtPaymentBatchReference.Multiline = False
		Me.txtPaymentBatchReference.Name = "txtPaymentBatchReference"
		Me.txtPaymentBatchReference.ReadOnly = False
		Me.txtPaymentBatchReference.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPaymentBatchReference.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPaymentBatchReference.Size = New System.Drawing.Size(141, 19)
		Me.txtPaymentBatchReference.TabIndex = 40
		Me.txtPaymentBatchReference.TabStop = True
		Me.txtPaymentBatchReference.Tag = "F;"
		Me.txtPaymentBatchReference.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPaymentBatchReference.Visible = True
		' 
		' txtAmountPayment
		' 
		Me.txtAmountPayment.AcceptsReturn = True
		Me.txtAmountPayment.AutoSize = False
		Me.txtAmountPayment.BackColor = System.Drawing.SystemColors.Window
		Me.txtAmountPayment.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtAmountPayment.CausesValidation = True
		Me.txtAmountPayment.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAmountPayment.Enabled = True
		Me.txtAmountPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtAmountPayment.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAmountPayment.HideSelection = True
		Me.txtAmountPayment.Location = New System.Drawing.Point(108, 128)
		Me.txtAmountPayment.MaxLength = 0
		Me.txtAmountPayment.Multiline = False
		Me.txtAmountPayment.Name = "txtAmountPayment"
		Me.txtAmountPayment.ReadOnly = False
		Me.txtAmountPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAmountPayment.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAmountPayment.Size = New System.Drawing.Size(141, 19)
		Me.txtAmountPayment.TabIndex = 38
		Me.txtAmountPayment.TabStop = True
		Me.txtAmountPayment.Tag = "F;FMT;$;"
		Me.txtAmountPayment.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtAmountPayment.Visible = True
		' 
		' txtPayeeName
		' 
		Me.txtPayeeName.AcceptsReturn = True
		Me.txtPayeeName.AutoSize = False
		Me.txtPayeeName.BackColor = System.Drawing.SystemColors.Window
		Me.txtPayeeName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPayeeName.CausesValidation = True
		Me.txtPayeeName.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPayeeName.Enabled = True
		Me.txtPayeeName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPayeeName.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPayeeName.HideSelection = True
		Me.txtPayeeName.Location = New System.Drawing.Point(108, 20)
		Me.txtPayeeName.MaxLength = 0
		Me.txtPayeeName.Multiline = False
		Me.txtPayeeName.Name = "txtPayeeName"
		Me.txtPayeeName.ReadOnly = False
		Me.txtPayeeName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPayeeName.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPayeeName.Size = New System.Drawing.Size(141, 19)
		Me.txtPayeeName.TabIndex = 31
		Me.txtPayeeName.TabStop = True
		Me.txtPayeeName.Tag = "F;"
		Me.txtPayeeName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPayeeName.Visible = True
		' 
		' cmdNavigate
		' 
		Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
		Me.cmdNavigate.CausesValidation = True
		Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdNavigate.Enabled = True
		Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNavigate.Location = New System.Drawing.Point(84, 380)
		Me.cmdNavigate.Name = "cmdNavigate"
		Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
		Me.cmdNavigate.TabIndex = 24
		Me.cmdNavigate.TabStop = True
		Me.cmdNavigate.Text = "*{Navigate}"
		Me.cmdNavigate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.cmdNavigate.Visible = False
		' 
		' cmdPrint
		' 
		Me.cmdPrint.BackColor = System.Drawing.SystemColors.Control
		Me.cmdPrint.CausesValidation = True
		Me.cmdPrint.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdPrint.Enabled = True
		Me.cmdPrint.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdPrint.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdPrint.Location = New System.Drawing.Point(8, 380)
		Me.cmdPrint.Name = "cmdPrint"
		Me.cmdPrint.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdPrint.Size = New System.Drawing.Size(73, 22)
		Me.cmdPrint.TabIndex = 23
		Me.cmdPrint.TabStop = True
		Me.cmdPrint.Tag = "CAP;204"
		Me.cmdPrint.Text = "*{Print}"
		Me.cmdPrint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdNewSearch
		' 
		Me.cmdNewSearch.BackColor = System.Drawing.SystemColors.Control
		Me.cmdNewSearch.CausesValidation = True
		Me.cmdNewSearch.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdNewSearch.Enabled = True
		Me.cmdNewSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdNewSearch.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNewSearch.Location = New System.Drawing.Point(556, 56)
		Me.cmdNewSearch.Name = "cmdNewSearch"
		Me.cmdNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNewSearch.Size = New System.Drawing.Size(85, 22)
		Me.cmdNewSearch.TabIndex = 21
		Me.cmdNewSearch.TabStop = True
		Me.cmdNewSearch.Tag = "CAP;207"
		Me.cmdNewSearch.Text = "*{New Search}"
		Me.cmdNewSearch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdNewSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdFindNow
		' 
		Me.cmdFindNow.BackColor = System.Drawing.SystemColors.Control
		Me.cmdFindNow.CausesValidation = True
		Me.cmdFindNow.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdFindNow.Enabled = True
		Me.cmdFindNow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdFindNow.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdFindNow.Location = New System.Drawing.Point(556, 28)
		Me.cmdFindNow.Name = "cmdFindNow"
		Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdFindNow.Size = New System.Drawing.Size(85, 22)
		Me.cmdFindNow.TabIndex = 20
		Me.cmdFindNow.TabStop = True
		Me.cmdFindNow.Tag = "CAP;206"
		Me.cmdFindNow.Text = "*{Find Now}"
		Me.cmdFindNow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdFindNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdHelp
		' 
		Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
		Me.cmdHelp.CausesValidation = True
		Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdHelp.Enabled = True
		Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdHelp.Location = New System.Drawing.Point(572, 380)
		Me.cmdHelp.Name = "cmdHelp"
		Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.cmdHelp.TabIndex = 27
		Me.cmdHelp.TabStop = True
		Me.cmdHelp.Tag = "CAP;202"
		Me.cmdHelp.Text = "*{Help}"
		Me.cmdHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(492, 380)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 26
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Tag = "CAP;201"
		Me.cmdCancel.Text = "*{Cancel}"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = False
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(412, 380)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 25
		Me.cmdOK.TabStop = True
		Me.cmdOK.Tag = "CAP;200"
		Me.cmdOK.Text = "*{OK}"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tabReceiptTab
		' 
		Me.tabReceiptTab.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabReceiptTab.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabReceiptTab.Controls.Add(Me._tabReceiptTab_TabPage0)
		Me.tabReceiptTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabReceiptTab.ItemSize = New System.Drawing.Size(179, 18)
		Me.tabReceiptTab.Location = New System.Drawing.Point(8, 8)
		Me.tabReceiptTab.Multiline = True
		Me.tabReceiptTab.Name = "tabReceiptTab"
		Me.tabReceiptTab.Size = New System.Drawing.Size(545, 194)
		Me.tabReceiptTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabReceiptTab.TabIndex = 0
		Me.tabReceiptTab.Tag = "CAP;101"
		' 
		' _tabReceiptTab_TabPage0
		' 
		Me._tabReceiptTab_TabPage0.Controls.Add(Me.lblPMLookupReceiptType)
		Me._tabReceiptTab_TabPage0.Controls.Add(Me.lblMediaReference)
		Me._tabReceiptTab_TabPage0.Controls.Add(Me.lblDateTo)
		Me._tabReceiptTab_TabPage0.Controls.Add(Me.lblDateFrom)
		Me._tabReceiptTab_TabPage0.Controls.Add(Me.lblCashDrawers)
		Me._tabReceiptTab_TabPage0.Controls.Add(Me.lblPMLookupMediaType)
		Me._tabReceiptTab_TabPage0.Controls.Add(Me.lblAmount)
		Me._tabReceiptTab_TabPage0.Controls.Add(Me.lblReceiptBatchReference)
		Me._tabReceiptTab_TabPage0.Controls.Add(Me.lblTheirReference)
		Me._tabReceiptTab_TabPage0.Controls.Add(Me.lblReceiptNumber)
		Me._tabReceiptTab_TabPage0.Controls.Add(Me.uctAccountLookup)
		Me._tabReceiptTab_TabPage0.Controls.Add(Me.cboPMLookupMediaType)
		Me._tabReceiptTab_TabPage0.Controls.Add(Me.uctPMLookupReceiptType)
		Me._tabReceiptTab_TabPage0.Controls.Add(Me.txtDateFrom)
		Me._tabReceiptTab_TabPage0.Controls.Add(Me.txtDateTo)
		Me._tabReceiptTab_TabPage0.Controls.Add(Me.txtAmount)
		Me._tabReceiptTab_TabPage0.Controls.Add(Me.txtTheirReference)
		Me._tabReceiptTab_TabPage0.Controls.Add(Me.txtReceiptNumber)
		Me._tabReceiptTab_TabPage0.Controls.Add(Me.txtReceiptBatchReference)
		Me._tabReceiptTab_TabPage0.Controls.Add(Me.txtMediaReference)
		Me._tabReceiptTab_TabPage0.Text = "*{&1 - Drawer}"
		' 
		' lblPMLookupReceiptType
		' 
		Me.lblPMLookupReceiptType.AutoSize = False
		Me.lblPMLookupReceiptType.BackColor = System.Drawing.SystemColors.Control
		Me.lblPMLookupReceiptType.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPMLookupReceiptType.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPMLookupReceiptType.Enabled = True
		Me.lblPMLookupReceiptType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPMLookupReceiptType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPMLookupReceiptType.Location = New System.Drawing.Point(12, 48)
		Me.lblPMLookupReceiptType.Name = "lblPMLookupReceiptType"
		Me.lblPMLookupReceiptType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPMLookupReceiptType.Size = New System.Drawing.Size(101, 17)
		Me.lblPMLookupReceiptType.TabIndex = 5
		Me.lblPMLookupReceiptType.Tag = "CAP;302"
		Me.lblPMLookupReceiptType.Text = "*{Receipt Type:}"
		Me.lblPMLookupReceiptType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPMLookupReceiptType.UseMnemonic = True
		Me.lblPMLookupReceiptType.Visible = True
		' 
		' lblMediaReference
		' 
		Me.lblMediaReference.AutoSize = False
		Me.lblMediaReference.BackColor = System.Drawing.SystemColors.Control
		Me.lblMediaReference.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMediaReference.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMediaReference.Enabled = True
		Me.lblMediaReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMediaReference.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMediaReference.Location = New System.Drawing.Point(12, 76)
		Me.lblMediaReference.Name = "lblMediaReference"
		Me.lblMediaReference.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMediaReference.Size = New System.Drawing.Size(101, 17)
		Me.lblMediaReference.TabIndex = 8
		Me.lblMediaReference.Tag = "CAP;304"
		Me.lblMediaReference.Text = "*{Media Reference:}"
		Me.lblMediaReference.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMediaReference.UseMnemonic = True
		Me.lblMediaReference.Visible = True
		' 
		' lblDateTo
		' 
		Me.lblDateTo.AutoSize = False
		Me.lblDateTo.BackColor = System.Drawing.SystemColors.Control
		Me.lblDateTo.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDateTo.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDateTo.Enabled = True
		Me.lblDateTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDateTo.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDateTo.Location = New System.Drawing.Point(260, 18)
		Me.lblDateTo.Name = "lblDateTo"
		Me.lblDateTo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDateTo.Size = New System.Drawing.Size(129, 17)
		Me.lblDateTo.TabIndex = 3
		Me.lblDateTo.Tag = "CAP;301"
		Me.lblDateTo.Text = "*{To Date:}"
		Me.lblDateTo.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDateTo.UseMnemonic = True
		Me.lblDateTo.Visible = True
		' 
		' lblDateFrom
		' 
		Me.lblDateFrom.AutoSize = False
		Me.lblDateFrom.BackColor = System.Drawing.SystemColors.Control
		Me.lblDateFrom.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDateFrom.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDateFrom.Enabled = True
		Me.lblDateFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDateFrom.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDateFrom.Location = New System.Drawing.Point(12, 18)
		Me.lblDateFrom.Name = "lblDateFrom"
		Me.lblDateFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDateFrom.Size = New System.Drawing.Size(101, 17)
		Me.lblDateFrom.TabIndex = 1
		Me.lblDateFrom.Tag = "CAP;300"
		Me.lblDateFrom.Text = "*{From Date:}"
		Me.lblDateFrom.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDateFrom.UseMnemonic = True
		Me.lblDateFrom.Visible = True
		' 
		' lblCashDrawers
		' 
		Me.lblCashDrawers.AutoSize = False
		Me.lblCashDrawers.BackColor = System.Drawing.SystemColors.Control
		Me.lblCashDrawers.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCashDrawers.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCashDrawers.Enabled = True
		Me.lblCashDrawers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCashDrawers.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCashDrawers.Location = New System.Drawing.Point(260, 46)
		Me.lblCashDrawers.Name = "lblCashDrawers"
		Me.lblCashDrawers.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCashDrawers.Size = New System.Drawing.Size(129, 17)
		Me.lblCashDrawers.TabIndex = 7
		Me.lblCashDrawers.Tag = "CAP;303"
		Me.lblCashDrawers.Text = "*{Account:}"
		Me.lblCashDrawers.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCashDrawers.UseMnemonic = True
		Me.lblCashDrawers.Visible = True
		' 
		' lblPMLookupMediaType
		' 
		Me.lblPMLookupMediaType.AutoSize = False
		Me.lblPMLookupMediaType.BackColor = System.Drawing.SystemColors.Control
		Me.lblPMLookupMediaType.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPMLookupMediaType.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPMLookupMediaType.Enabled = True
		Me.lblPMLookupMediaType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPMLookupMediaType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPMLookupMediaType.Location = New System.Drawing.Point(12, 132)
		Me.lblPMLookupMediaType.Name = "lblPMLookupMediaType"
		Me.lblPMLookupMediaType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPMLookupMediaType.Size = New System.Drawing.Size(101, 17)
		Me.lblPMLookupMediaType.TabIndex = 16
		Me.lblPMLookupMediaType.Tag = "CAP;308"
		Me.lblPMLookupMediaType.Text = "*{Media Type:}"
		Me.lblPMLookupMediaType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPMLookupMediaType.UseMnemonic = True
		Me.lblPMLookupMediaType.Visible = True
		' 
		' lblAmount
		' 
		Me.lblAmount.AutoSize = False
		Me.lblAmount.BackColor = System.Drawing.SystemColors.Control
		Me.lblAmount.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAmount.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAmount.Enabled = True
		Me.lblAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAmount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAmount.Location = New System.Drawing.Point(12, 104)
		Me.lblAmount.Name = "lblAmount"
		Me.lblAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAmount.Size = New System.Drawing.Size(101, 17)
		Me.lblAmount.TabIndex = 12
		Me.lblAmount.Tag = "CAP;306"
		Me.lblAmount.Text = "*{Amount:}"
		Me.lblAmount.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAmount.UseMnemonic = True
		Me.lblAmount.Visible = True
		' 
		' lblReceiptBatchReference
		' 
		Me.lblReceiptBatchReference.AutoSize = False
		Me.lblReceiptBatchReference.BackColor = System.Drawing.SystemColors.Control
		Me.lblReceiptBatchReference.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblReceiptBatchReference.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblReceiptBatchReference.Enabled = True
		Me.lblReceiptBatchReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblReceiptBatchReference.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblReceiptBatchReference.Location = New System.Drawing.Point(260, 104)
		Me.lblReceiptBatchReference.Name = "lblReceiptBatchReference"
		Me.lblReceiptBatchReference.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblReceiptBatchReference.Size = New System.Drawing.Size(101, 17)
		Me.lblReceiptBatchReference.TabIndex = 14
		Me.lblReceiptBatchReference.Tag = "CAP;307"
		Me.lblReceiptBatchReference.Text = "*{Batch Reference:}"
		Me.lblReceiptBatchReference.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblReceiptBatchReference.UseMnemonic = True
		Me.lblReceiptBatchReference.Visible = True
		' 
		' lblTheirReference
		' 
		Me.lblTheirReference.AutoSize = False
		Me.lblTheirReference.BackColor = System.Drawing.SystemColors.Control
		Me.lblTheirReference.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTheirReference.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTheirReference.Enabled = True
		Me.lblTheirReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTheirReference.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTheirReference.Location = New System.Drawing.Point(260, 76)
		Me.lblTheirReference.Name = "lblTheirReference"
		Me.lblTheirReference.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTheirReference.Size = New System.Drawing.Size(101, 17)
		Me.lblTheirReference.TabIndex = 10
		Me.lblTheirReference.Tag = "CAP;305"
		Me.lblTheirReference.Text = "*{Their Reference:}"
		Me.lblTheirReference.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTheirReference.UseMnemonic = True
		Me.lblTheirReference.Visible = True
		' 
		' lblReceiptNumber
		' 
		Me.lblReceiptNumber.AutoSize = False
		Me.lblReceiptNumber.BackColor = System.Drawing.SystemColors.Control
		Me.lblReceiptNumber.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblReceiptNumber.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblReceiptNumber.Enabled = True
		Me.lblReceiptNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblReceiptNumber.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblReceiptNumber.Location = New System.Drawing.Point(260, 132)
		Me.lblReceiptNumber.Name = "lblReceiptNumber"
		Me.lblReceiptNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblReceiptNumber.Size = New System.Drawing.Size(101, 17)
		Me.lblReceiptNumber.TabIndex = 18
		Me.lblReceiptNumber.Tag = "CAP;309"
		Me.lblReceiptNumber.Text = "*{Receipt Number:}"
		Me.lblReceiptNumber.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblReceiptNumber.UseMnemonic = True
		Me.lblReceiptNumber.Visible = True
		' 
		' uctAccountLookup
		' 
		Me.uctAccountLookup.Location = New System.Drawing.Point(388, 44)
		Me.uctAccountLookup.Name = "uctAccountLookup"
		Me.uctAccountLookup.OnlyUpdatableAccounts = True
		Me.uctAccountLookup.Size = New System.Drawing.Size(141, 19)
        Me.uctAccountLookup.TabIndex = 29
        'Developer Guide No Solutions no.15
        'Me.uctAccountLookup.WhatsThisHelpID = 16004
		' 
		' cboPMLookupMediaType
		' 
		Me.cboPMLookupMediaType.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboPMLookupMediaType.Location = New System.Drawing.Point(112, 128)
		Me.cboPMLookupMediaType.Name = "cboPMLookupMediaType"
		Me.cboPMLookupMediaType.Size = New System.Drawing.Size(141, 21)
		Me.cboPMLookupMediaType.Sorted = True
		Me.cboPMLookupMediaType.TabIndex = 17
        'Developer guide no.77
        Me.cboPMLookupMediaType.TableName = "MediaType"
		Me.cboPMLookupMediaType.WhereClause = "is_receipt=1 AND is_deleted=0"
		' 
		' uctPMLookupReceiptType
		' 
		Me.uctPMLookupReceiptType.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.uctPMLookupReceiptType.Location = New System.Drawing.Point(112, 44)
		Me.uctPMLookupReceiptType.Name = "uctPMLookupReceiptType"
		Me.uctPMLookupReceiptType.Size = New System.Drawing.Size(141, 21)
		Me.uctPMLookupReceiptType.Sorted = True
		Me.uctPMLookupReceiptType.TabIndex = 6
        'Developer guide no.77
        Me.uctPMLookupReceiptType.TableName = "CashListItem_Receipt_Type"
		' 
		' txtDateFrom
		' 
		Me.txtDateFrom.AcceptsReturn = True
		Me.txtDateFrom.AutoSize = False
		Me.txtDateFrom.BackColor = System.Drawing.SystemColors.Window
		Me.txtDateFrom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDateFrom.CausesValidation = True
		Me.txtDateFrom.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDateFrom.Enabled = True
		Me.txtDateFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDateFrom.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDateFrom.HideSelection = True
		Me.txtDateFrom.Location = New System.Drawing.Point(112, 16)
		Me.txtDateFrom.MaxLength = 0
		Me.txtDateFrom.Multiline = False
		Me.txtDateFrom.Name = "txtDateFrom"
		Me.txtDateFrom.ReadOnly = False
		Me.txtDateFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDateFrom.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDateFrom.Size = New System.Drawing.Size(141, 19)
		Me.txtDateFrom.TabIndex = 2
		Me.txtDateFrom.TabStop = True
		Me.txtDateFrom.Tag = "F;FMT;DT;"
		Me.txtDateFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDateFrom.Visible = True
		' 
		' txtDateTo
		' 
		Me.txtDateTo.AcceptsReturn = True
		Me.txtDateTo.AutoSize = False
		Me.txtDateTo.BackColor = System.Drawing.SystemColors.Window
		Me.txtDateTo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDateTo.CausesValidation = True
		Me.txtDateTo.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDateTo.Enabled = True
		Me.txtDateTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDateTo.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDateTo.HideSelection = True
		Me.txtDateTo.Location = New System.Drawing.Point(388, 16)
		Me.txtDateTo.MaxLength = 0
		Me.txtDateTo.Multiline = False
		Me.txtDateTo.Name = "txtDateTo"
		Me.txtDateTo.ReadOnly = False
		Me.txtDateTo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDateTo.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDateTo.Size = New System.Drawing.Size(141, 19)
		Me.txtDateTo.TabIndex = 4
		Me.txtDateTo.TabStop = True
		Me.txtDateTo.Tag = "F;FMT;DT;"
		Me.txtDateTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDateTo.Visible = True
		' 
		' txtAmount
		' 
		Me.txtAmount.AcceptsReturn = True
		Me.txtAmount.AutoSize = False
		Me.txtAmount.BackColor = System.Drawing.SystemColors.Window
		Me.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtAmount.CausesValidation = True
		Me.txtAmount.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAmount.Enabled = True
		Me.txtAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtAmount.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAmount.HideSelection = True
		Me.txtAmount.Location = New System.Drawing.Point(112, 100)
		Me.txtAmount.MaxLength = 0
		Me.txtAmount.Multiline = False
		Me.txtAmount.Name = "txtAmount"
		Me.txtAmount.ReadOnly = False
		Me.txtAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAmount.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAmount.Size = New System.Drawing.Size(141, 19)
		Me.txtAmount.TabIndex = 13
		Me.txtAmount.TabStop = True
		Me.txtAmount.Tag = "F;FMT;$;"
		Me.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtAmount.Visible = True
		' 
		' txtTheirReference
		' 
		Me.txtTheirReference.AcceptsReturn = True
		Me.txtTheirReference.AutoSize = False
		Me.txtTheirReference.BackColor = System.Drawing.SystemColors.Window
		Me.txtTheirReference.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtTheirReference.CausesValidation = True
		Me.txtTheirReference.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtTheirReference.Enabled = True
		Me.txtTheirReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtTheirReference.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtTheirReference.HideSelection = True
		Me.txtTheirReference.Location = New System.Drawing.Point(388, 72)
		Me.txtTheirReference.MaxLength = 0
		Me.txtTheirReference.Multiline = False
		Me.txtTheirReference.Name = "txtTheirReference"
		Me.txtTheirReference.ReadOnly = False
		Me.txtTheirReference.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTheirReference.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTheirReference.Size = New System.Drawing.Size(141, 19)
		Me.txtTheirReference.TabIndex = 11
		Me.txtTheirReference.TabStop = True
		Me.txtTheirReference.Tag = "F;"
		Me.txtTheirReference.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTheirReference.Visible = True
		' 
		' txtReceiptNumber
		' 
		Me.txtReceiptNumber.AcceptsReturn = True
		Me.txtReceiptNumber.AutoSize = False
		Me.txtReceiptNumber.BackColor = System.Drawing.SystemColors.Window
		Me.txtReceiptNumber.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtReceiptNumber.CausesValidation = True
		Me.txtReceiptNumber.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtReceiptNumber.Enabled = True
		Me.txtReceiptNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtReceiptNumber.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtReceiptNumber.HideSelection = True
		Me.txtReceiptNumber.Location = New System.Drawing.Point(388, 128)
		Me.txtReceiptNumber.MaxLength = 0
		Me.txtReceiptNumber.Multiline = False
		Me.txtReceiptNumber.Name = "txtReceiptNumber"
		Me.txtReceiptNumber.ReadOnly = False
		Me.txtReceiptNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtReceiptNumber.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtReceiptNumber.Size = New System.Drawing.Size(141, 19)
		Me.txtReceiptNumber.TabIndex = 19
		Me.txtReceiptNumber.TabStop = True
		Me.txtReceiptNumber.Tag = "F;"
		Me.txtReceiptNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtReceiptNumber.Visible = True
		' 
		' txtReceiptBatchReference
		' 
		Me.txtReceiptBatchReference.AcceptsReturn = True
		Me.txtReceiptBatchReference.AutoSize = False
		Me.txtReceiptBatchReference.BackColor = System.Drawing.SystemColors.Window
		Me.txtReceiptBatchReference.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtReceiptBatchReference.CausesValidation = True
		Me.txtReceiptBatchReference.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtReceiptBatchReference.Enabled = True
		Me.txtReceiptBatchReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtReceiptBatchReference.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtReceiptBatchReference.HideSelection = True
		Me.txtReceiptBatchReference.Location = New System.Drawing.Point(388, 100)
		Me.txtReceiptBatchReference.MaxLength = 0
		Me.txtReceiptBatchReference.Multiline = False
		Me.txtReceiptBatchReference.Name = "txtReceiptBatchReference"
		Me.txtReceiptBatchReference.ReadOnly = False
		Me.txtReceiptBatchReference.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtReceiptBatchReference.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtReceiptBatchReference.Size = New System.Drawing.Size(141, 19)
		Me.txtReceiptBatchReference.TabIndex = 15
		Me.txtReceiptBatchReference.TabStop = True
		Me.txtReceiptBatchReference.Tag = "F;"
		Me.txtReceiptBatchReference.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtReceiptBatchReference.Visible = True
		' 
		' txtMediaReference
		' 
		Me.txtMediaReference.AcceptsReturn = True
		Me.txtMediaReference.AutoSize = False
		Me.txtMediaReference.BackColor = System.Drawing.SystemColors.Window
		Me.txtMediaReference.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtMediaReference.CausesValidation = True
		Me.txtMediaReference.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtMediaReference.Enabled = True
		Me.txtMediaReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtMediaReference.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtMediaReference.HideSelection = True
		Me.txtMediaReference.Location = New System.Drawing.Point(112, 72)
		Me.txtMediaReference.MaxLength = 0
		Me.txtMediaReference.Multiline = False
		Me.txtMediaReference.Name = "txtMediaReference"
		Me.txtMediaReference.ReadOnly = False
		Me.txtMediaReference.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtMediaReference.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtMediaReference.Size = New System.Drawing.Size(141, 19)
		Me.txtMediaReference.TabIndex = 9
		Me.txtMediaReference.TabStop = True
		Me.txtMediaReference.Tag = "F;"
		Me.txtMediaReference.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtMediaReference.Visible = True
		' 
		' stbStatus
		' 
		Me.stbStatus.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.stbStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.stbStatus.Location = New System.Drawing.Point(0, 406)
		Me.stbStatus.Name = "stbStatus"
		Me.stbStatus.ShowItemToolTips = True
		Me.stbStatus.Size = New System.Drawing.Size(648, 18)
		Me.stbStatus.TabIndex = 28
		Me.stbStatus.Text = ""
		Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me._stbStatus_Panel1})
		' 
		' _stbStatus_Panel1
		' 
		Me._stbStatus_Panel1.AutoSize = True
		Me._stbStatus_Panel1.AutoSize = False
		Me._stbStatus_Panel1.BorderSides = CType(System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom, System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me._stbStatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
		Me._stbStatus_Panel1.DoubleClickEnabled = True
		Me._stbStatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
		Me._stbStatus_Panel1.Name = ""
		Me._stbStatus_Panel1.Size = New System.Drawing.Size(648, 18)
		Me._stbStatus_Panel1.Tag = ""
		Me._stbStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._stbStatus_Panel1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
		' 
		' lvwSearchDetailsReceipt
		' 
		Me.lvwSearchDetailsReceipt.BackColor = System.Drawing.SystemColors.Window
		Me.lvwSearchDetailsReceipt.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lvwSearchDetailsReceipt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwSearchDetailsReceipt.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwSearchDetailsReceipt.HideSelection = False
		Me.lvwSearchDetailsReceipt.LabelEdit = False
		Me.lvwSearchDetailsReceipt.LabelWrap = True
		Me.lvwSearchDetailsReceipt.Location = New System.Drawing.Point(8, 204)
		Me.lvwSearchDetailsReceipt.Name = "lvwSearchDetailsReceipt"
		Me.lvwSearchDetailsReceipt.Size = New System.Drawing.Size(633, 169)
		Me.lvwSearchDetailsReceipt.TabIndex = 22
		Me.lvwSearchDetailsReceipt.Tag = "CAP;350"
		Me.lvwSearchDetailsReceipt.View = System.Windows.Forms.View.Details
		Me.lvwSearchDetailsReceipt.Columns.Add(Me._lvwSearchDetailsReceipt_ColumnHeader_1)
		Me.lvwSearchDetailsReceipt.Columns.Add(Me._lvwSearchDetailsReceipt_ColumnHeader_2)
		Me.lvwSearchDetailsReceipt.Columns.Add(Me._lvwSearchDetailsReceipt_ColumnHeader_3)
		Me.lvwSearchDetailsReceipt.Columns.Add(Me._lvwSearchDetailsReceipt_ColumnHeader_4)
		Me.lvwSearchDetailsReceipt.Columns.Add(Me._lvwSearchDetailsReceipt_ColumnHeader_5)
		Me.lvwSearchDetailsReceipt.Columns.Add(Me._lvwSearchDetailsReceipt_ColumnHeader_6)
		Me.lvwSearchDetailsReceipt.Columns.Add(Me._lvwSearchDetailsReceipt_ColumnHeader_7)
		Me.lvwSearchDetailsReceipt.Columns.Add(Me._lvwSearchDetailsReceipt_ColumnHeader_8)
		Me.lvwSearchDetailsReceipt.Columns.Add(Me._lvwSearchDetailsReceipt_ColumnHeader_9)
		Me.lvwSearchDetailsReceipt.Columns.Add(Me._lvwSearchDetailsReceipt_ColumnHeader_10)
		Me.lvwSearchDetailsReceipt.Columns.Add(Me._lvwSearchDetailsReceipt_ColumnHeader_11)
		' 
		' _lvwSearchDetailsReceipt_ColumnHeader_1
		' 
		Me._lvwSearchDetailsReceipt_ColumnHeader_1.Tag = ""
		Me._lvwSearchDetailsReceipt_ColumnHeader_1.Text = "*{Receipt Number}"
		Me._lvwSearchDetailsReceipt_ColumnHeader_1.Width = 97
		' 
		' _lvwSearchDetailsReceipt_ColumnHeader_2
		' 
		Me._lvwSearchDetailsReceipt_ColumnHeader_2.Tag = "DATESORT"
		Me._lvwSearchDetailsReceipt_ColumnHeader_2.Text = "*{Transaction Date}"
		Me._lvwSearchDetailsReceipt_ColumnHeader_2.Width = 97
		' 
		' _lvwSearchDetailsReceipt_ColumnHeader_3
		' 
		Me._lvwSearchDetailsReceipt_ColumnHeader_3.Tag = ""
		Me._lvwSearchDetailsReceipt_ColumnHeader_3.Text = "*{Media Type}"
		Me._lvwSearchDetailsReceipt_ColumnHeader_3.Width = 74
		' 
		' _lvwSearchDetailsReceipt_ColumnHeader_4
		' 
		Me._lvwSearchDetailsReceipt_ColumnHeader_4.Tag = ""
		Me._lvwSearchDetailsReceipt_ColumnHeader_4.Text = "*{Media Reference}"
		Me._lvwSearchDetailsReceipt_ColumnHeader_4.Width = 94
		' 
		' _lvwSearchDetailsReceipt_ColumnHeader_5
		' 
		Me._lvwSearchDetailsReceipt_ColumnHeader_5.Tag = ""
		Me._lvwSearchDetailsReceipt_ColumnHeader_5.Text = "*{Their Reference}"
		Me._lvwSearchDetailsReceipt_ColumnHeader_5.Width = 127
		' 
		' _lvwSearchDetailsReceipt_ColumnHeader_6
		' 
		Me._lvwSearchDetailsReceipt_ColumnHeader_6.Tag = ""
		Me._lvwSearchDetailsReceipt_ColumnHeader_6.Text = "*{Receipt Type}"
		Me._lvwSearchDetailsReceipt_ColumnHeader_6.Width = 67
		' 
		' _lvwSearchDetailsReceipt_ColumnHeader_7
		' 
		Me._lvwSearchDetailsReceipt_ColumnHeader_7.Tag = ""
		Me._lvwSearchDetailsReceipt_ColumnHeader_7.Text = "*{Account}"
		Me._lvwSearchDetailsReceipt_ColumnHeader_7.Width = 97
		' 
		' _lvwSearchDetailsReceipt_ColumnHeader_8
		' 
		Me._lvwSearchDetailsReceipt_ColumnHeader_8.Tag = ""
		Me._lvwSearchDetailsReceipt_ColumnHeader_8.Text = "*{Amount}"
		Me._lvwSearchDetailsReceipt_ColumnHeader_8.Width = 97
		' 
		' _lvwSearchDetailsReceipt_ColumnHeader_9
		' 
		Me._lvwSearchDetailsReceipt_ColumnHeader_9.Tag = ""
		Me._lvwSearchDetailsReceipt_ColumnHeader_9.Text = "*{Allocation Status}"
		Me._lvwSearchDetailsReceipt_ColumnHeader_9.Width = 97
		' 
		' _lvwSearchDetailsReceipt_ColumnHeader_10
		' 
		Me._lvwSearchDetailsReceipt_ColumnHeader_10.Tag = ""
		Me._lvwSearchDetailsReceipt_ColumnHeader_10.Text = "*{PMUser}"
		Me._lvwSearchDetailsReceipt_ColumnHeader_10.Width = 97
		' 
		' _lvwSearchDetailsReceipt_ColumnHeader_11
		' 
		Me._lvwSearchDetailsReceipt_ColumnHeader_11.Tag = "HIDDEN"
		Me._lvwSearchDetailsReceipt_ColumnHeader_11.Text = "*{CashListId}"
		Me._lvwSearchDetailsReceipt_ColumnHeader_11.Width = 0
		' 
		' ImgImage
		' 
		Me.ImgImage.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.ImgImage.Cursor = System.Windows.Forms.Cursors.Default
		Me.ImgImage.Enabled = True
		Me.ImgImage.Image = CType(resources.GetObject("ImgImage.Image"), System.Drawing.Image)
		Me.ImgImage.Location = New System.Drawing.Point(576, 104)
		Me.ImgImage.Name = "ImgImage"
		Me.ImgImage.Size = New System.Drawing.Size(32, 32)
		Me.ImgImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.ImgImage.Visible = True
		' 
		' frmInterface
		' 
		Me.AcceptButton = Me.cmdFindNow
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(648, 424)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdView)
		Me.Controls.Add(Me.lvwSearchDetailsPayment)
		Me.Controls.Add(Me.tabPaymentTab)
		Me.Controls.Add(Me.cmdNavigate)
		Me.Controls.Add(Me.cmdPrint)
		Me.Controls.Add(Me.cmdNewSearch)
		Me.Controls.Add(Me.cmdFindNow)
		Me.Controls.Add(Me.cmdHelp)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.tabReceiptTab)
		Me.Controls.Add(Me.stbStatus)
		Me.Controls.Add(Me.lvwSearchDetailsReceipt)
		Me.Controls.Add(Me.ImgImage)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = True
		Me.Icon = CType(resources.GetObject("frmInterface.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(158, 247)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Tag = "CAP;100"
		Me.Text = "*{Find Cash Drawer}"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabPaymentTab, 1)
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabReceiptTab, 1)
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwSearchDetailsPayment, True)
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwSearchDetailsReceipt, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.lvwSearchDetailsPayment.ResumeLayout(False)
		Me.tabPaymentTab.ResumeLayout(False)
		Me._tabPaymentTab_TabPage0.ResumeLayout(False)
		Me.tabReceiptTab.ResumeLayout(False)
		Me._tabReceiptTab_TabPage0.ResumeLayout(False)
		Me.stbStatus.ResumeLayout(False)
		Me.lvwSearchDetailsReceipt.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub lvwSearchDetailsReceipt_InitializeColumnKeys()
		Me._lvwSearchDetailsReceipt_ColumnHeader_1.Name = ""
		Me._lvwSearchDetailsReceipt_ColumnHeader_2.Name = ""
		Me._lvwSearchDetailsReceipt_ColumnHeader_3.Name = ""
		Me._lvwSearchDetailsReceipt_ColumnHeader_4.Name = ""
		Me._lvwSearchDetailsReceipt_ColumnHeader_5.Name = ""
		Me._lvwSearchDetailsReceipt_ColumnHeader_6.Name = ""
		Me._lvwSearchDetailsReceipt_ColumnHeader_7.Name = ""
		Me._lvwSearchDetailsReceipt_ColumnHeader_8.Name = ""
		Me._lvwSearchDetailsReceipt_ColumnHeader_9.Name = ""
		Me._lvwSearchDetailsReceipt_ColumnHeader_10.Name = ""
		Me._lvwSearchDetailsReceipt_ColumnHeader_11.Name = ""
	End Sub
	Sub lvwSearchDetailsPayment_InitializeColumnKeys()
		Me._lvwSearchDetailsPayment_ColumnHeader_1.Name = ""
		Me._lvwSearchDetailsPayment_ColumnHeader_2.Name = ""
		Me._lvwSearchDetailsPayment_ColumnHeader_3.Name = ""
		Me._lvwSearchDetailsPayment_ColumnHeader_4.Name = ""
		Me._lvwSearchDetailsPayment_ColumnHeader_5.Name = ""
		Me._lvwSearchDetailsPayment_ColumnHeader_6.Name = ""
		Me._lvwSearchDetailsPayment_ColumnHeader_7.Name = ""
		Me._lvwSearchDetailsPayment_ColumnHeader_8.Name = ""
		Me._lvwSearchDetailsPayment_ColumnHeader_9.Name = ""
		Me._lvwSearchDetailsPayment_ColumnHeader_10.Name = ""
		Me._lvwSearchDetailsPayment_ColumnHeader_11.Name = ""
		Me._lvwSearchDetailsPayment_ColumnHeader_12.Name = ""
	End Sub
#End Region 
End Class