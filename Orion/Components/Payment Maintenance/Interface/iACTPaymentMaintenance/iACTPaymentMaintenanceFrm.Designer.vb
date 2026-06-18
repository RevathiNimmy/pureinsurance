<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
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
	Public WithEvents ImageList1 As System.Windows.Forms.ImageList
	Private WithEvents _stbStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents stbStatus As System.Windows.Forms.StatusStrip
	Public WithEvents cboPaymentStatus As PMLookupControl.cboPMLookup
	Public WithEvents cmdCancelPayment As System.Windows.Forms.Button
	Public WithEvents cmdAddTask As System.Windows.Forms.Button
	Public WithEvents chkShowOnlyOutstanding As System.Windows.Forms.CheckBox
	Public WithEvents txtDateFrom As System.Windows.Forms.TextBox
	Public WithEvents txtDateTo As System.Windows.Forms.TextBox
	Public WithEvents txtAmountRangeTo As System.Windows.Forms.TextBox
	Public WithEvents cboBranch As System.Windows.Forms.ComboBox
	Public WithEvents txtClientAccountNumber As System.Windows.Forms.TextBox
	Public WithEvents txtPayeeName As System.Windows.Forms.TextBox
	Public WithEvents cmdClientCode As System.Windows.Forms.Button
	Public WithEvents txtPolicyClaimNumber As System.Windows.Forms.TextBox
	Public WithEvents txtBatchReference As System.Windows.Forms.TextBox
	Public WithEvents txtMediaReferenceFrom As System.Windows.Forms.TextBox
	Public WithEvents txtMediaReferenceTo As System.Windows.Forms.TextBox
	Public WithEvents txtAmountRangeFrom As System.Windows.Forms.TextBox
	Public WithEvents uctBankAccount As UserControls.BankAccount
	Public WithEvents cboMediaType As UserControls.TypeTable
	Public WithEvents cboPaymentType As PMLookupControl.cboPMLookup
	Public WithEvents lblDateFrom As System.Windows.Forms.Label
	Public WithEvents lblDateTo As System.Windows.Forms.Label
	Public WithEvents lblClientCode As System.Windows.Forms.Label
	Public WithEvents lblAmountRangeTo As System.Windows.Forms.Label
	Public WithEvents lblBranch As System.Windows.Forms.Label
	Public WithEvents lblBank As System.Windows.Forms.Label
	Public WithEvents lblClientAccountNumber As System.Windows.Forms.Label
	Public WithEvents lblPayeeName As System.Windows.Forms.Label
	Public WithEvents pnlClientCode As System.Windows.Forms.Label
	Public WithEvents lblPaymentType As System.Windows.Forms.Label
	Public WithEvents lblPolicyClaimNumber As System.Windows.Forms.Label
	Public WithEvents lblMediaType As System.Windows.Forms.Label
	Public WithEvents lblBatchReference As System.Windows.Forms.Label
	Public WithEvents lblPaymentStatus As System.Windows.Forms.Label
	Public WithEvents lblMediaReferenceFrom As System.Windows.Forms.Label
	Public WithEvents lblMediaReferenceTo As System.Windows.Forms.Label
	Public WithEvents lblAmountRangeFrom As System.Windows.Forms.Label
	Public WithEvents fraFindParty As System.Windows.Forms.GroupBox
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdNewSearch As System.Windows.Forms.Button
	Public WithEvents cmdFindNow As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Private WithEvents _lvwFindParty_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_12 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_13 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_14 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_15 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_16 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_17 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_18 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_19 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_20 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_21 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_22 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_23 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_24 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_25 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_26 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_27 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_28 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_29 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_30 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindParty_ColumnHeader_31 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwFindParty As System.Windows.Forms.ListView
	Public WithEvents lblShowOnlyOutstanding As System.Windows.Forms.Label
	Public WithEvents ImgImage As System.Windows.Forms.PictureBox
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.stbStatus = New System.Windows.Forms.StatusStrip
        Me._stbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.cboPaymentStatus = New PMLookupControl.cboPMLookup
        Me.cmdCancelPayment = New System.Windows.Forms.Button
        Me.cmdAddTask = New System.Windows.Forms.Button
        Me.chkShowOnlyOutstanding = New System.Windows.Forms.CheckBox
        Me.fraFindParty = New System.Windows.Forms.GroupBox
        Me.txtDateFrom = New System.Windows.Forms.TextBox
        Me.txtDateTo = New System.Windows.Forms.TextBox
        Me.txtAmountRangeTo = New System.Windows.Forms.TextBox
        Me.cboBranch = New System.Windows.Forms.ComboBox
        Me.txtClientAccountNumber = New System.Windows.Forms.TextBox
        Me.txtPayeeName = New System.Windows.Forms.TextBox
        Me.cmdClientCode = New System.Windows.Forms.Button
        Me.txtPolicyClaimNumber = New System.Windows.Forms.TextBox
        Me.txtBatchReference = New System.Windows.Forms.TextBox
        Me.txtMediaReferenceFrom = New System.Windows.Forms.TextBox
        Me.txtMediaReferenceTo = New System.Windows.Forms.TextBox
        Me.txtAmountRangeFrom = New System.Windows.Forms.TextBox
        Me.uctBankAccount = New UserControls.BankAccount
        Me.cboMediaType = New UserControls.TypeTable
        Me.cboPaymentType = New PMLookupControl.cboPMLookup
        Me.lblDateFrom = New System.Windows.Forms.Label
        Me.lblDateTo = New System.Windows.Forms.Label
        Me.lblClientCode = New System.Windows.Forms.Label
        Me.lblAmountRangeTo = New System.Windows.Forms.Label
        Me.lblBranch = New System.Windows.Forms.Label
        Me.lblBank = New System.Windows.Forms.Label
        Me.lblClientAccountNumber = New System.Windows.Forms.Label
        Me.lblPayeeName = New System.Windows.Forms.Label
        Me.pnlClientCode = New System.Windows.Forms.Label
        Me.lblPaymentType = New System.Windows.Forms.Label
        Me.lblPolicyClaimNumber = New System.Windows.Forms.Label
        Me.lblMediaType = New System.Windows.Forms.Label
        Me.lblBatchReference = New System.Windows.Forms.Label
        Me.lblPaymentStatus = New System.Windows.Forms.Label
        Me.lblMediaReferenceFrom = New System.Windows.Forms.Label
        Me.lblMediaReferenceTo = New System.Windows.Forms.Label
        Me.lblAmountRangeFrom = New System.Windows.Forms.Label
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.cmdNewSearch = New System.Windows.Forms.Button
        Me.cmdFindNow = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.lvwFindParty = New System.Windows.Forms.ListView
        Me._lvwFindParty_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_10 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_11 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_12 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_13 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_14 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_15 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_16 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_17 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_18 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_19 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_20 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_21 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_22 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_23 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_24 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_25 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_26 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_27 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_28 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_29 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_30 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindParty_ColumnHeader_31 = New System.Windows.Forms.ColumnHeader
        Me.lblShowOnlyOutstanding = New System.Windows.Forms.Label
        Me.ImgImage = New System.Windows.Forms.PictureBox
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.stbStatus.SuspendLayout()
        Me.fraFindParty.SuspendLayout()
        CType(Me.ImgImage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "")
        '
        'stbStatus
        '
        Me.stbStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._stbStatus_Panel1})
        Me.stbStatus.Location = New System.Drawing.Point(0, 480)
        Me.stbStatus.Name = "stbStatus"
        Me.stbStatus.ShowItemToolTips = True
        Me.stbStatus.Size = New System.Drawing.Size(725, 25)
        Me.stbStatus.TabIndex = 42
        '
        '_stbStatus_Panel1
        '
        Me._stbStatus_Panel1.AutoSize = False
        Me._stbStatus_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._stbStatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._stbStatus_Panel1.DoubleClickEnabled = True
        Me._stbStatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._stbStatus_Panel1.Name = "_stbStatus_Panel1"
        Me._stbStatus_Panel1.Size = New System.Drawing.Size(400, 25)
        Me._stbStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboPaymentStatus
        '
        Me.cboPaymentStatus.DefaultItemId = 0
        Me.cboPaymentStatus.FirstItem = ""
        Me.cboPaymentStatus.ItemId = 0
        Me.cboPaymentStatus.ListIndex = -1
        Me.cboPaymentStatus.Location = New System.Drawing.Point(454, 120)
        Me.cboPaymentStatus.Name = "cboPaymentStatus"
        Me.cboPaymentStatus.PMLookupProductFamily = 1
        Me.cboPaymentStatus.SingleItemId = 0
        Me.cboPaymentStatus.Size = New System.Drawing.Size(153, 21)
        Me.cboPaymentStatus.Sorted = True
        Me.cboPaymentStatus.TabIndex = 9
        Me.cboPaymentStatus.TableName = "cashlistitem_payment_status"
        Me.cboPaymentStatus.ToolTipText = ""
        Me.cboPaymentStatus.WhereClause = ""
        '
        'cmdCancelPayment
        '
        Me.cmdCancelPayment.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancelPayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancelPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancelPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancelPayment.Location = New System.Drawing.Point(530, 448)
        Me.cmdCancelPayment.Name = "cmdCancelPayment"
        Me.cmdCancelPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancelPayment.Size = New System.Drawing.Size(111, 22)
        Me.cmdCancelPayment.TabIndex = 20
        Me.cmdCancelPayment.Text = "Cancel &Payment"
        Me.cmdCancelPayment.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancelPayment.UseVisualStyleBackColor = False
        '
        'cmdAddTask
        '
        Me.cmdAddTask.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddTask.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddTask.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddTask.Location = New System.Drawing.Point(4, 448)
        Me.cmdAddTask.Name = "cmdAddTask"
        Me.cmdAddTask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddTask.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddTask.TabIndex = 19
        Me.cmdAddTask.Text = "&Add Task"
        Me.cmdAddTask.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddTask.UseVisualStyleBackColor = False
        '
        'chkShowOnlyOutstanding
        '
        Me.chkShowOnlyOutstanding.BackColor = System.Drawing.SystemColors.Control
        Me.chkShowOnlyOutstanding.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkShowOnlyOutstanding.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkShowOnlyOutstanding.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShowOnlyOutstanding.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkShowOnlyOutstanding.Location = New System.Drawing.Point(666, 158)
        Me.chkShowOnlyOutstanding.Name = "chkShowOnlyOutstanding"
        Me.chkShowOnlyOutstanding.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkShowOnlyOutstanding.Size = New System.Drawing.Size(13, 13)
        Me.chkShowOnlyOutstanding.TabIndex = 18
        Me.chkShowOnlyOutstanding.UseVisualStyleBackColor = False
        '
        'fraFindParty
        '
        Me.fraFindParty.BackColor = System.Drawing.SystemColors.Control
        Me.fraFindParty.Controls.Add(Me.txtDateFrom)
        Me.fraFindParty.Controls.Add(Me.txtDateTo)
        Me.fraFindParty.Controls.Add(Me.txtAmountRangeTo)
        Me.fraFindParty.Controls.Add(Me.cboBranch)
        Me.fraFindParty.Controls.Add(Me.txtClientAccountNumber)
        Me.fraFindParty.Controls.Add(Me.txtPayeeName)
        Me.fraFindParty.Controls.Add(Me.cmdClientCode)
        Me.fraFindParty.Controls.Add(Me.txtPolicyClaimNumber)
        Me.fraFindParty.Controls.Add(Me.txtBatchReference)
        Me.fraFindParty.Controls.Add(Me.txtMediaReferenceFrom)
        Me.fraFindParty.Controls.Add(Me.txtMediaReferenceTo)
        Me.fraFindParty.Controls.Add(Me.txtAmountRangeFrom)
        Me.fraFindParty.Controls.Add(Me.uctBankAccount)
        Me.fraFindParty.Controls.Add(Me.cboMediaType)
        Me.fraFindParty.Controls.Add(Me.cboPaymentType)
        Me.fraFindParty.Controls.Add(Me.lblDateFrom)
        Me.fraFindParty.Controls.Add(Me.lblDateTo)
        Me.fraFindParty.Controls.Add(Me.lblClientCode)
        Me.fraFindParty.Controls.Add(Me.lblAmountRangeTo)
        Me.fraFindParty.Controls.Add(Me.lblBranch)
        Me.fraFindParty.Controls.Add(Me.lblBank)
        Me.fraFindParty.Controls.Add(Me.lblClientAccountNumber)
        Me.fraFindParty.Controls.Add(Me.lblPayeeName)
        Me.fraFindParty.Controls.Add(Me.pnlClientCode)
        Me.fraFindParty.Controls.Add(Me.lblPaymentType)
        Me.fraFindParty.Controls.Add(Me.lblPolicyClaimNumber)
        Me.fraFindParty.Controls.Add(Me.lblMediaType)
        Me.fraFindParty.Controls.Add(Me.lblBatchReference)
        Me.fraFindParty.Controls.Add(Me.lblPaymentStatus)
        Me.fraFindParty.Controls.Add(Me.lblMediaReferenceFrom)
        Me.fraFindParty.Controls.Add(Me.lblMediaReferenceTo)
        Me.fraFindParty.Controls.Add(Me.lblAmountRangeFrom)
        Me.fraFindParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraFindParty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFindParty.Location = New System.Drawing.Point(4, 4)
        Me.fraFindParty.Name = "fraFindParty"
        Me.fraFindParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFindParty.Size = New System.Drawing.Size(611, 215)
        Me.fraFindParty.TabIndex = 23
        Me.fraFindParty.TabStop = False
        Me.fraFindParty.Text = "Payment"
        '
        'txtDateFrom
        '
        Me.txtDateFrom.AcceptsReturn = True
        Me.txtDateFrom.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateFrom.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDateFrom.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDateFrom.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateFrom.Location = New System.Drawing.Point(130, 186)
        Me.txtDateFrom.MaxLength = 0
        Me.txtDateFrom.Name = "txtDateFrom"
        Me.txtDateFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateFrom.Size = New System.Drawing.Size(105, 21)
        Me.txtDateFrom.TabIndex = 14
        '
        'txtDateTo
        '
        Me.txtDateTo.AcceptsReturn = True
        Me.txtDateTo.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateTo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDateTo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDateTo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateTo.Location = New System.Drawing.Point(450, 186)
        Me.txtDateTo.MaxLength = 0
        Me.txtDateTo.Name = "txtDateTo"
        Me.txtDateTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateTo.Size = New System.Drawing.Size(105, 21)
        Me.txtDateTo.TabIndex = 15
        '
        'txtAmountRangeTo
        '
        Me.txtAmountRangeTo.AcceptsReturn = True
        Me.txtAmountRangeTo.BackColor = System.Drawing.SystemColors.Window
        Me.txtAmountRangeTo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAmountRangeTo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmountRangeTo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAmountRangeTo.Location = New System.Drawing.Point(450, 162)
        Me.txtAmountRangeTo.MaxLength = 15
        Me.txtAmountRangeTo.Name = "txtAmountRangeTo"
        Me.txtAmountRangeTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAmountRangeTo.Size = New System.Drawing.Size(153, 21)
        Me.txtAmountRangeTo.TabIndex = 13
        '
        'cboBranch
        '
        Me.cboBranch.BackColor = System.Drawing.SystemColors.Window
        Me.cboBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBranch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBranch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBranch.Location = New System.Drawing.Point(130, 22)
        Me.cboBranch.Name = "cboBranch"
        Me.cboBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBranch.Size = New System.Drawing.Size(153, 21)
        Me.cboBranch.TabIndex = 0
        '
        'txtClientAccountNumber
        '
        Me.txtClientAccountNumber.AcceptsReturn = True
        Me.txtClientAccountNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtClientAccountNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClientAccountNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClientAccountNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClientAccountNumber.Location = New System.Drawing.Point(450, 46)
        Me.txtClientAccountNumber.MaxLength = 20
        Me.txtClientAccountNumber.Name = "txtClientAccountNumber"
        Me.txtClientAccountNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClientAccountNumber.Size = New System.Drawing.Size(153, 21)
        Me.txtClientAccountNumber.TabIndex = 3
        '
        'txtPayeeName
        '
        Me.txtPayeeName.AcceptsReturn = True
        Me.txtPayeeName.BackColor = System.Drawing.SystemColors.Window
        Me.txtPayeeName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPayeeName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPayeeName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPayeeName.Location = New System.Drawing.Point(130, 70)
        Me.txtPayeeName.MaxLength = 60
        Me.txtPayeeName.Name = "txtPayeeName"
        Me.txtPayeeName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPayeeName.Size = New System.Drawing.Size(153, 21)
        Me.txtPayeeName.TabIndex = 4
        '
        'cmdClientCode
        '
        Me.cmdClientCode.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClientCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClientCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClientCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClientCode.Location = New System.Drawing.Point(266, 50)
        Me.cmdClientCode.Name = "cmdClientCode"
        Me.cmdClientCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClientCode.Size = New System.Drawing.Size(30, 20)
        Me.cmdClientCode.TabIndex = 2
        Me.cmdClientCode.Text = "..."
        Me.cmdClientCode.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClientCode.UseVisualStyleBackColor = False
        '
        'txtPolicyClaimNumber
        '
        Me.txtPolicyClaimNumber.AcceptsReturn = True
        Me.txtPolicyClaimNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolicyClaimNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicyClaimNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicyClaimNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicyClaimNumber.Location = New System.Drawing.Point(450, 70)
        Me.txtPolicyClaimNumber.MaxLength = 30
        Me.txtPolicyClaimNumber.Name = "txtPolicyClaimNumber"
        Me.txtPolicyClaimNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicyClaimNumber.Size = New System.Drawing.Size(153, 21)
        Me.txtPolicyClaimNumber.TabIndex = 5
        '
        'txtBatchReference
        '
        Me.txtBatchReference.AcceptsReturn = True
        Me.txtBatchReference.BackColor = System.Drawing.SystemColors.Window
        Me.txtBatchReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBatchReference.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBatchReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBatchReference.Location = New System.Drawing.Point(130, 116)
        Me.txtBatchReference.MaxLength = 25
        Me.txtBatchReference.Name = "txtBatchReference"
        Me.txtBatchReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBatchReference.Size = New System.Drawing.Size(153, 21)
        Me.txtBatchReference.TabIndex = 8
        '
        'txtMediaReferenceFrom
        '
        Me.txtMediaReferenceFrom.AcceptsReturn = True
        Me.txtMediaReferenceFrom.BackColor = System.Drawing.SystemColors.Window
        Me.txtMediaReferenceFrom.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMediaReferenceFrom.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMediaReferenceFrom.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMediaReferenceFrom.Location = New System.Drawing.Point(130, 140)
        Me.txtMediaReferenceFrom.MaxLength = 100
        Me.txtMediaReferenceFrom.Name = "txtMediaReferenceFrom"
        Me.txtMediaReferenceFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMediaReferenceFrom.Size = New System.Drawing.Size(153, 21)
        Me.txtMediaReferenceFrom.TabIndex = 10
        '
        'txtMediaReferenceTo
        '
        Me.txtMediaReferenceTo.AcceptsReturn = True
        Me.txtMediaReferenceTo.BackColor = System.Drawing.SystemColors.Window
        Me.txtMediaReferenceTo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMediaReferenceTo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMediaReferenceTo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMediaReferenceTo.Location = New System.Drawing.Point(450, 140)
        Me.txtMediaReferenceTo.MaxLength = 100
        Me.txtMediaReferenceTo.Name = "txtMediaReferenceTo"
        Me.txtMediaReferenceTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMediaReferenceTo.Size = New System.Drawing.Size(153, 21)
        Me.txtMediaReferenceTo.TabIndex = 11
        '
        'txtAmountRangeFrom
        '
        Me.txtAmountRangeFrom.AcceptsReturn = True
        Me.txtAmountRangeFrom.BackColor = System.Drawing.SystemColors.Window
        Me.txtAmountRangeFrom.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAmountRangeFrom.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmountRangeFrom.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAmountRangeFrom.Location = New System.Drawing.Point(130, 162)
        Me.txtAmountRangeFrom.MaxLength = 15
        Me.txtAmountRangeFrom.Name = "txtAmountRangeFrom"
        Me.txtAmountRangeFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAmountRangeFrom.Size = New System.Drawing.Size(153, 21)
        Me.txtAmountRangeFrom.TabIndex = 12
        '
        'uctBankAccount
        '
        Me.uctBankAccount.DefaultId = "0"
        Me.uctBankAccount.FirstItem = ""
        Me.uctBankAccount.Id = 0
        Me.uctBankAccount.ListIndex = -1
        Me.uctBankAccount.Location = New System.Drawing.Point(450, 22)
        Me.uctBankAccount.Name = "uctBankAccount"
        Me.uctBankAccount.Size = New System.Drawing.Size(153, 21)
        Me.uctBankAccount.TabIndex = 1
        Me.uctBankAccount.ToolTipText = ""
        Me.uctBankAccount.WhatsThisHelpID = 0
        '
        'cboMediaType
        '
        Me.cboMediaType.BackStyle = 0
        Me.cboMediaType.BorderStyle = 0
        Me.cboMediaType.DefaultItemId = 0
        Me.cboMediaType.FirstItem = ""
        Me.cboMediaType.ItemCode = ""
        Me.cboMediaType.ItemId = 0
        Me.cboMediaType.ListIndex = -1
        Me.cboMediaType.Location = New System.Drawing.Point(450, 92)
        Me.cboMediaType.Name = "cboMediaType"
        Me.cboMediaType.Size = New System.Drawing.Size(153, 21)
        Me.cboMediaType.Sorted = True
        Me.cboMediaType.TabIndex = 7
        Me.cboMediaType.Table = UserControls.TypeTable.actTable.actMediaType
        Me.cboMediaType.TableName = "MediaType"
        Me.cboMediaType.ToolTipText = ""
        Me.cboMediaType.WhatsThisHelpID = 0
        '
        'cboPaymentType
        '
        Me.cboPaymentType.DefaultItemId = 0
        Me.cboPaymentType.FirstItem = ""
        Me.cboPaymentType.ItemId = 0
        Me.cboPaymentType.ListIndex = -1
        Me.cboPaymentType.Location = New System.Drawing.Point(130, 92)
        Me.cboPaymentType.Name = "cboPaymentType"
        Me.cboPaymentType.PMLookupProductFamily = 1
        Me.cboPaymentType.SingleItemId = 0
        Me.cboPaymentType.Size = New System.Drawing.Size(153, 21)
        Me.cboPaymentType.Sorted = True
        Me.cboPaymentType.TabIndex = 6
        Me.cboPaymentType.TableName = "cashlistitem_payment_type"
        Me.cboPaymentType.ToolTipText = ""
        Me.cboPaymentType.WhereClause = ""
        '
        'lblDateFrom
        '
        Me.lblDateFrom.BackColor = System.Drawing.SystemColors.Control
        Me.lblDateFrom.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDateFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateFrom.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDateFrom.Location = New System.Drawing.Point(8, 186)
        Me.lblDateFrom.Name = "lblDateFrom"
        Me.lblDateFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDateFrom.Size = New System.Drawing.Size(125, 17)
        Me.lblDateFrom.TabIndex = 41
        Me.lblDateFrom.Text = "Date From:"
        '
        'lblDateTo
        '
        Me.lblDateTo.BackColor = System.Drawing.SystemColors.Control
        Me.lblDateTo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDateTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDateTo.Location = New System.Drawing.Point(312, 186)
        Me.lblDateTo.Name = "lblDateTo"
        Me.lblDateTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDateTo.Size = New System.Drawing.Size(141, 17)
        Me.lblDateTo.TabIndex = 40
        Me.lblDateTo.Text = "To:"
        '
        'lblClientCode
        '
        Me.lblClientCode.AutoSize = True
        Me.lblClientCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientCode.Location = New System.Drawing.Point(8, 48)
        Me.lblClientCode.Name = "lblClientCode"
        Me.lblClientCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientCode.Size = New System.Drawing.Size(64, 13)
        Me.lblClientCode.TabIndex = 39
        Me.lblClientCode.Text = "Client Code:"
        '
        'lblAmountRangeTo
        '
        Me.lblAmountRangeTo.BackColor = System.Drawing.SystemColors.Control
        Me.lblAmountRangeTo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAmountRangeTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAmountRangeTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAmountRangeTo.Location = New System.Drawing.Point(312, 162)
        Me.lblAmountRangeTo.Name = "lblAmountRangeTo"
        Me.lblAmountRangeTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAmountRangeTo.Size = New System.Drawing.Size(141, 17)
        Me.lblAmountRangeTo.TabIndex = 38
        Me.lblAmountRangeTo.Text = "To:"
        '
        'lblBranch
        '
        Me.lblBranch.AutoSize = True
        Me.lblBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranch.Location = New System.Drawing.Point(8, 22)
        Me.lblBranch.Name = "lblBranch"
        Me.lblBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranch.Size = New System.Drawing.Size(44, 13)
        Me.lblBranch.TabIndex = 37
        Me.lblBranch.Text = "Branch:"
        '
        'lblBank
        '
        Me.lblBank.BackColor = System.Drawing.SystemColors.Control
        Me.lblBank.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBank.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBank.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBank.Location = New System.Drawing.Point(312, 22)
        Me.lblBank.Name = "lblBank"
        Me.lblBank.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBank.Size = New System.Drawing.Size(141, 17)
        Me.lblBank.TabIndex = 36
        Me.lblBank.Text = "Bank Account:"
        '
        'lblClientAccountNumber
        '
        Me.lblClientAccountNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientAccountNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientAccountNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientAccountNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientAccountNumber.Location = New System.Drawing.Point(312, 48)
        Me.lblClientAccountNumber.Name = "lblClientAccountNumber"
        Me.lblClientAccountNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientAccountNumber.Size = New System.Drawing.Size(141, 17)
        Me.lblClientAccountNumber.TabIndex = 35
        Me.lblClientAccountNumber.Text = "Client Account Number:"
        '
        'lblPayeeName
        '
        Me.lblPayeeName.BackColor = System.Drawing.SystemColors.Control
        Me.lblPayeeName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPayeeName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPayeeName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPayeeName.Location = New System.Drawing.Point(8, 70)
        Me.lblPayeeName.Name = "lblPayeeName"
        Me.lblPayeeName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPayeeName.Size = New System.Drawing.Size(125, 17)
        Me.lblPayeeName.TabIndex = 34
        Me.lblPayeeName.Text = "Payee Name:"
        '
        'pnlClientCode
        '
        Me.pnlClientCode.BackColor = System.Drawing.SystemColors.Control
        Me.pnlClientCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlClientCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.pnlClientCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlClientCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlClientCode.Location = New System.Drawing.Point(130, 48)
        Me.pnlClientCode.Name = "pnlClientCode"
        Me.pnlClientCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlClientCode.Size = New System.Drawing.Size(127, 19)
        Me.pnlClientCode.TabIndex = 33
        Me.pnlClientCode.UseMnemonic = False
        '
        'lblPaymentType
        '
        Me.lblPaymentType.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentType.Location = New System.Drawing.Point(8, 94)
        Me.lblPaymentType.Name = "lblPaymentType"
        Me.lblPaymentType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentType.Size = New System.Drawing.Size(125, 17)
        Me.lblPaymentType.TabIndex = 32
        Me.lblPaymentType.Text = "Payment Type:"
        '
        'lblPolicyClaimNumber
        '
        Me.lblPolicyClaimNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyClaimNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyClaimNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyClaimNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyClaimNumber.Location = New System.Drawing.Point(312, 70)
        Me.lblPolicyClaimNumber.Name = "lblPolicyClaimNumber"
        Me.lblPolicyClaimNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyClaimNumber.Size = New System.Drawing.Size(141, 17)
        Me.lblPolicyClaimNumber.TabIndex = 31
        Me.lblPolicyClaimNumber.Text = "Policy/Claim Number:"
        '
        'lblMediaType
        '
        Me.lblMediaType.BackColor = System.Drawing.SystemColors.Control
        Me.lblMediaType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMediaType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMediaType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMediaType.Location = New System.Drawing.Point(312, 92)
        Me.lblMediaType.Name = "lblMediaType"
        Me.lblMediaType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMediaType.Size = New System.Drawing.Size(141, 17)
        Me.lblMediaType.TabIndex = 30
        Me.lblMediaType.Text = "Media Type:"
        '
        'lblBatchReference
        '
        Me.lblBatchReference.BackColor = System.Drawing.SystemColors.Control
        Me.lblBatchReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBatchReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBatchReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBatchReference.Location = New System.Drawing.Point(8, 118)
        Me.lblBatchReference.Name = "lblBatchReference"
        Me.lblBatchReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBatchReference.Size = New System.Drawing.Size(125, 17)
        Me.lblBatchReference.TabIndex = 29
        Me.lblBatchReference.Text = "Batch Reference:"
        '
        'lblPaymentStatus
        '
        Me.lblPaymentStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentStatus.Location = New System.Drawing.Point(312, 116)
        Me.lblPaymentStatus.Name = "lblPaymentStatus"
        Me.lblPaymentStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentStatus.Size = New System.Drawing.Size(141, 17)
        Me.lblPaymentStatus.TabIndex = 28
        Me.lblPaymentStatus.Text = "Payment Status:"
        '
        'lblMediaReferenceFrom
        '
        Me.lblMediaReferenceFrom.BackColor = System.Drawing.SystemColors.Control
        Me.lblMediaReferenceFrom.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMediaReferenceFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMediaReferenceFrom.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMediaReferenceFrom.Location = New System.Drawing.Point(8, 140)
        Me.lblMediaReferenceFrom.Name = "lblMediaReferenceFrom"
        Me.lblMediaReferenceFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMediaReferenceFrom.Size = New System.Drawing.Size(125, 17)
        Me.lblMediaReferenceFrom.TabIndex = 27
        Me.lblMediaReferenceFrom.Text = "Media Ref From:"
        '
        'lblMediaReferenceTo
        '
        Me.lblMediaReferenceTo.BackColor = System.Drawing.SystemColors.Control
        Me.lblMediaReferenceTo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMediaReferenceTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMediaReferenceTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMediaReferenceTo.Location = New System.Drawing.Point(312, 140)
        Me.lblMediaReferenceTo.Name = "lblMediaReferenceTo"
        Me.lblMediaReferenceTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMediaReferenceTo.Size = New System.Drawing.Size(141, 17)
        Me.lblMediaReferenceTo.TabIndex = 26
        Me.lblMediaReferenceTo.Text = "To:"
        '
        'lblAmountRangeFrom
        '
        Me.lblAmountRangeFrom.BackColor = System.Drawing.SystemColors.Control
        Me.lblAmountRangeFrom.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAmountRangeFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAmountRangeFrom.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAmountRangeFrom.Location = New System.Drawing.Point(8, 162)
        Me.lblAmountRangeFrom.Name = "lblAmountRangeFrom"
        Me.lblAmountRangeFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAmountRangeFrom.Size = New System.Drawing.Size(125, 17)
        Me.lblAmountRangeFrom.TabIndex = 25
        Me.lblAmountRangeFrom.Text = "Amount Range From:"
        '
        'cmdNewSearch
        '
        Me.cmdNewSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewSearch.Location = New System.Drawing.Point(634, 60)
        Me.cmdNewSearch.Name = "cmdNewSearch"
        Me.cmdNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewSearch.Size = New System.Drawing.Size(81, 22)
        Me.cmdNewSearch.TabIndex = 17
        Me.cmdNewSearch.Text = "Ne&w Search"
        Me.cmdNewSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNewSearch.UseVisualStyleBackColor = False
        '
        'cmdFindNow
        '
        Me.cmdFindNow.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindNow.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindNow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindNow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindNow.Location = New System.Drawing.Point(634, 28)
        Me.cmdFindNow.Name = "cmdFindNow"
        Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindNow.Size = New System.Drawing.Size(81, 22)
        Me.cmdFindNow.TabIndex = 16
        Me.cmdFindNow.Text = "F&ind Now"
        Me.cmdFindNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindNow.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(442, 448)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 22
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        Me.cmdHelp.Visible = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(646, 448)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 21
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Enabled = False
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(700, 618)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 24
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'lvwFindParty
        '
        Me.lvwFindParty.BackColor = System.Drawing.SystemColors.Window
        Me.lvwFindParty.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwFindParty, "")
        Me.lvwFindParty.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwFindParty_ColumnHeader_1, Me._lvwFindParty_ColumnHeader_2, Me._lvwFindParty_ColumnHeader_3, Me._lvwFindParty_ColumnHeader_4, Me._lvwFindParty_ColumnHeader_5, Me._lvwFindParty_ColumnHeader_6, Me._lvwFindParty_ColumnHeader_7, Me._lvwFindParty_ColumnHeader_8, Me._lvwFindParty_ColumnHeader_9, Me._lvwFindParty_ColumnHeader_10, Me._lvwFindParty_ColumnHeader_11, Me._lvwFindParty_ColumnHeader_12, Me._lvwFindParty_ColumnHeader_13, Me._lvwFindParty_ColumnHeader_14, Me._lvwFindParty_ColumnHeader_15, Me._lvwFindParty_ColumnHeader_16, Me._lvwFindParty_ColumnHeader_17, Me._lvwFindParty_ColumnHeader_18, Me._lvwFindParty_ColumnHeader_19, Me._lvwFindParty_ColumnHeader_20, Me._lvwFindParty_ColumnHeader_21, Me._lvwFindParty_ColumnHeader_22, Me._lvwFindParty_ColumnHeader_23, Me._lvwFindParty_ColumnHeader_24, Me._lvwFindParty_ColumnHeader_25, Me._lvwFindParty_ColumnHeader_26, Me._lvwFindParty_ColumnHeader_27, Me._lvwFindParty_ColumnHeader_28, Me._lvwFindParty_ColumnHeader_29, Me._lvwFindParty_ColumnHeader_30, Me._lvwFindParty_ColumnHeader_31})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwFindParty, True)
        Me.lvwFindParty.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwFindParty.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwFindParty.FullRowSelect = True
        Me.lvwFindParty.GridLines = True
        Me.lvwFindParty.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwFindParty, "lvwFindParty_ItemClick")
        Me.listViewHelper1.SetLargeIcons(Me.lvwFindParty, "")
        Me.lvwFindParty.Location = New System.Drawing.Point(4, 222)
        Me.lvwFindParty.MultiSelect = False
        Me.lvwFindParty.Name = "lvwFindParty"
        Me.lvwFindParty.Size = New System.Drawing.Size(713, 213)
        Me.listViewHelper1.SetSmallIcons(Me.lvwFindParty, "")
        Me.listViewHelper1.SetSorted(Me.lvwFindParty, False)
        Me.listViewHelper1.SetSortKey(Me.lvwFindParty, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwFindParty, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwFindParty.TabIndex = 44
        Me.lvwFindParty.UseCompatibleStateImageBehavior = False
        Me.lvwFindParty.View = System.Windows.Forms.View.Details
        '
        '_lvwFindParty_ColumnHeader_1
        '
        Me._lvwFindParty_ColumnHeader_1.Text = "Client Code"
        Me._lvwFindParty_ColumnHeader_1.Width = 108
        '
        '_lvwFindParty_ColumnHeader_2
        '
        Me._lvwFindParty_ColumnHeader_2.Text = "Policy Holder"
        Me._lvwFindParty_ColumnHeader_2.Width = 108
        '
        '_lvwFindParty_ColumnHeader_3
        '
        Me._lvwFindParty_ColumnHeader_3.Text = "Policy/Claim Number"
        Me._lvwFindParty_ColumnHeader_3.Width = 134
        '
        '_lvwFindParty_ColumnHeader_4
        '
        Me._lvwFindParty_ColumnHeader_4.Text = "Amount"
        Me._lvwFindParty_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwFindParty_ColumnHeader_4.Width = 97
        '
        '_lvwFindParty_ColumnHeader_5
        '
        Me._lvwFindParty_ColumnHeader_5.Text = "Payment Date"
        Me._lvwFindParty_ColumnHeader_5.Width = 97
        '
        '_lvwFindParty_ColumnHeader_6
        '
        Me._lvwFindParty_ColumnHeader_6.Text = "Media Reference"
        Me._lvwFindParty_ColumnHeader_6.Width = 108
        '
        '_lvwFindParty_ColumnHeader_7
        '
        Me._lvwFindParty_ColumnHeader_7.Text = "Payment Status"
        Me._lvwFindParty_ColumnHeader_7.Width = 108
        '
        '_lvwFindParty_ColumnHeader_8
        '
        Me._lvwFindParty_ColumnHeader_8.Text = "Bank Reconcilation Date"
        Me._lvwFindParty_ColumnHeader_8.Width = 167
        '
        '_lvwFindParty_ColumnHeader_9
        '
        Me._lvwFindParty_ColumnHeader_9.Text = "Cancellation Reason"
        Me._lvwFindParty_ColumnHeader_9.Width = 134
        '
        '_lvwFindParty_ColumnHeader_10
        '
        Me._lvwFindParty_ColumnHeader_10.Text = "Cancellation Date"
        Me._lvwFindParty_ColumnHeader_10.Width = 114
        '
        '_lvwFindParty_ColumnHeader_11
        '
        Me._lvwFindParty_ColumnHeader_11.Text = "Account Number"
        Me._lvwFindParty_ColumnHeader_11.Width = 108
        '
        '_lvwFindParty_ColumnHeader_12
        '
        Me._lvwFindParty_ColumnHeader_12.Text = "Bank Sort Code"
        Me._lvwFindParty_ColumnHeader_12.Width = 108
        '
        '_lvwFindParty_ColumnHeader_13
        '
        Me._lvwFindParty_ColumnHeader_13.Text = "Branch Code"
        Me._lvwFindParty_ColumnHeader_13.Width = 97
        '
        '_lvwFindParty_ColumnHeader_14
        '
        Me._lvwFindParty_ColumnHeader_14.Text = "Bank Account"
        Me._lvwFindParty_ColumnHeader_14.Width = 97
        '
        '_lvwFindParty_ColumnHeader_15
        '
        Me._lvwFindParty_ColumnHeader_15.Text = "Batch Reference"
        Me._lvwFindParty_ColumnHeader_15.Width = 108
        '
        '_lvwFindParty_ColumnHeader_16
        '
        Me._lvwFindParty_ColumnHeader_16.Text = "Their Reference"
        Me._lvwFindParty_ColumnHeader_16.Width = 108
        '
        '_lvwFindParty_ColumnHeader_17
        '
        Me._lvwFindParty_ColumnHeader_17.Text = "Our Reference"
        Me._lvwFindParty_ColumnHeader_17.Width = 97
        '
        '_lvwFindParty_ColumnHeader_18
        '
        Me._lvwFindParty_ColumnHeader_18.Text = "Document Reference"
        Me._lvwFindParty_ColumnHeader_18.Width = 134
        '
        '_lvwFindParty_ColumnHeader_19
        '
        Me._lvwFindParty_ColumnHeader_19.Text = "Payment Type"
        Me._lvwFindParty_ColumnHeader_19.Width = 97
        '
        '_lvwFindParty_ColumnHeader_20
        '
        Me._lvwFindParty_ColumnHeader_20.Text = "Media Type"
        Me._lvwFindParty_ColumnHeader_20.Width = 97
        '
        '_lvwFindParty_ColumnHeader_21
        '
        Me._lvwFindParty_ColumnHeader_21.Text = "User"
        Me._lvwFindParty_ColumnHeader_21.Width = 97
        '
        '_lvwFindParty_ColumnHeader_22
        '
        Me._lvwFindParty_ColumnHeader_22.Text = "Payee Name"
        Me._lvwFindParty_ColumnHeader_22.Width = 97
        '
        '_lvwFindParty_ColumnHeader_23
        '
        Me._lvwFindParty_ColumnHeader_23.Text = "Reverse ID"
        Me._lvwFindParty_ColumnHeader_23.Width = 0
        '
        '_lvwFindParty_ColumnHeader_24
        '
        Me._lvwFindParty_ColumnHeader_24.Text = "Allow Reverse Allocation"
        Me._lvwFindParty_ColumnHeader_24.Width = 0
        '
        '_lvwFindParty_ColumnHeader_25
        '
        Me._lvwFindParty_ColumnHeader_25.Text = "Reverse Allocation Days"
        Me._lvwFindParty_ColumnHeader_25.Width = 0
        '
        '_lvwFindParty_ColumnHeader_26
        '
        Me._lvwFindParty_ColumnHeader_26.Text = "CashListItem ID"
        Me._lvwFindParty_ColumnHeader_26.Width = 0
        '
        '_lvwFindParty_ColumnHeader_27
        '
        Me._lvwFindParty_ColumnHeader_27.Text = "Trans Detail ID"
        Me._lvwFindParty_ColumnHeader_27.Width = 0
        '
        '_lvwFindParty_ColumnHeader_28
        '
        Me._lvwFindParty_ColumnHeader_28.Text = "PartyCnt"
        Me._lvwFindParty_ColumnHeader_28.Width = 0
        '
        '_lvwFindParty_ColumnHeader_29
        '
        Me._lvwFindParty_ColumnHeader_29.Text = "Insurance_File_Cnt"
        Me._lvwFindParty_ColumnHeader_29.Width = 0
        '
        '_lvwFindParty_ColumnHeader_30
        '
        Me._lvwFindParty_ColumnHeader_30.Text = "Claim_Cnt"
        Me._lvwFindParty_ColumnHeader_30.Width = 0
        '
        '_lvwFindParty_ColumnHeader_31
        '
        Me._lvwFindParty_ColumnHeader_31.Text = "CurrencyId"
        Me._lvwFindParty_ColumnHeader_31.Width = 0
        '
        'lblShowOnlyOutstanding
        '
        Me.lblShowOnlyOutstanding.BackColor = System.Drawing.SystemColors.Control
        Me.lblShowOnlyOutstanding.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowOnlyOutstanding.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowOnlyOutstanding.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblShowOnlyOutstanding.Location = New System.Drawing.Point(634, 128)
        Me.lblShowOnlyOutstanding.Name = "lblShowOnlyOutstanding"
        Me.lblShowOnlyOutstanding.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowOnlyOutstanding.Size = New System.Drawing.Size(77, 33)
        Me.lblShowOnlyOutstanding.TabIndex = 43
        Me.lblShowOnlyOutstanding.Text = "Show only outstanding"
        Me.lblShowOnlyOutstanding.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'ImgImage
        '
        Me.ImgImage.Cursor = System.Windows.Forms.Cursors.Default
        Me.ImgImage.Image = CType(resources.GetObject("ImgImage.Image"), System.Drawing.Image)
        Me.ImgImage.Location = New System.Drawing.Point(656, 90)
        Me.ImgImage.Name = "ImgImage"
        Me.ImgImage.Size = New System.Drawing.Size(32, 32)
        Me.ImgImage.TabIndex = 45
        Me.ImgImage.TabStop = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(725, 505)
        Me.Controls.Add(Me.stbStatus)
        Me.Controls.Add(Me.cboPaymentStatus)
        Me.Controls.Add(Me.cmdCancelPayment)
        Me.Controls.Add(Me.cmdAddTask)
        Me.Controls.Add(Me.chkShowOnlyOutstanding)
        Me.Controls.Add(Me.fraFindParty)
        Me.Controls.Add(Me.cmdNewSearch)
        Me.Controls.Add(Me.cmdFindNow)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.lvwFindParty)
        Me.Controls.Add(Me.lblShowOnlyOutstanding)
        Me.Controls.Add(Me.ImgImage)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(190, 279)
        Me.MaximizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Find Payment"
        Me.stbStatus.ResumeLayout(False)
        Me.stbStatus.PerformLayout()
        Me.fraFindParty.ResumeLayout(False)
        Me.fraFindParty.PerformLayout()
        CType(Me.ImgImage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class