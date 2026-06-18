<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializelblMediaType()
		InitializecmdPolicyNumber()
		InitializecmdClientCode()
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
    Public WithEvents cmdSelectAll As System.Windows.Forms.Button
    Public WithEvents cmdUpdateAllSelected As System.Windows.Forms.Button
    Public WithEvents cmdOk As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents ImageList1 As System.Windows.Forms.ImageList
	Public WithEvents txtDocumentRef As System.Windows.Forms.TextBox
	Public WithEvents txtCollectionDateTo As System.Windows.Forms.TextBox
	Public WithEvents cboDrawnBankName As PMLookupControl.cboPMLookup
	Public WithEvents txtCollectionDateFrom As System.Windows.Forms.TextBox
	Public WithEvents cboBranch As System.Windows.Forms.ComboBox
	Private WithEvents _cmdClientCode_0 As System.Windows.Forms.Button
	Public WithEvents txtPolicyNumber As System.Windows.Forms.TextBox
	Public WithEvents txtMediaReference As System.Windows.Forms.TextBox
	Public WithEvents uctBankAccount As UserControls.BankAccount
	Public WithEvents cboMediaTypeStatus As PMLookupControl.cboPMLookup
	Public WithEvents lblDocumentRef As System.Windows.Forms.Label
	Public WithEvents lblClientCode As System.Windows.Forms.Label
	Public WithEvents lblBranch As System.Windows.Forms.Label
	Public WithEvents lblBank As System.Windows.Forms.Label
	Public WithEvents lblCollectionDateFrom As System.Windows.Forms.Label
	Public WithEvents lblPaymentType As System.Windows.Forms.Label
	Public WithEvents lblPolicyNumber As System.Windows.Forms.Label
	Private WithEvents _lblMediaType_0 As System.Windows.Forms.Label
	Public WithEvents lblMediaReference As System.Windows.Forms.Label
	Public WithEvents lblPaymentStatus As System.Windows.Forms.Label
	Public WithEvents lblDrawnBankName As System.Windows.Forms.Label
	Public WithEvents fraFindReceipts As System.Windows.Forms.GroupBox
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdNewSearch As System.Windows.Forms.Button
	Public WithEvents cmdFindNow As System.Windows.Forms.Button
	Private WithEvents _lvwFindReceipts_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindReceipts_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindReceipts_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindReceipts_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindReceipts_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindReceipts_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindReceipts_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindReceipts_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindReceipts_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindReceipts_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindReceipts_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindReceipts_ColumnHeader_12 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindReceipts_ColumnHeader_13 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindReceipts_ColumnHeader_14 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindReceipts_ColumnHeader_15 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFindReceipts_ColumnHeader_16 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwFindReceipts As System.Windows.Forms.ListView
	Public cmdClientCode(0) As System.Windows.Forms.Button
	Public cmdPolicyNumber(1) As System.Windows.Forms.Button
	Public lblMediaType(0) As System.Windows.Forms.Label
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.cmdSelectAll = New System.Windows.Forms.Button
        Me.cmdUpdateAllSelected = New System.Windows.Forms.Button
        Me.cmdOk = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.fraFindReceipts = New System.Windows.Forms.GroupBox
        Me._cmdPolicyNumber_1 = New System.Windows.Forms.Button
        Me.txtClientCode = New System.Windows.Forms.TextBox
        Me.txtDocumentRef = New System.Windows.Forms.TextBox
        Me.txtCollectionDateTo = New System.Windows.Forms.TextBox
        Me.cboDrawnBankName = New PMLookupControl.cboPMLookup
        Me.txtCollectionDateFrom = New System.Windows.Forms.TextBox
        Me.cboBranch = New System.Windows.Forms.ComboBox
        Me._cmdClientCode_0 = New System.Windows.Forms.Button
        Me.txtPolicyNumber = New System.Windows.Forms.TextBox
        Me.txtMediaReference = New System.Windows.Forms.TextBox
        Me.uctBankAccount = New UserControls.BankAccount
        Me.cboMediaTypeStatus = New PMLookupControl.cboPMLookup
        Me.lblDocumentRef = New System.Windows.Forms.Label
        Me.lblClientCode = New System.Windows.Forms.Label
        Me.lblBranch = New System.Windows.Forms.Label
        Me.lblBank = New System.Windows.Forms.Label
        Me.lblCollectionDateFrom = New System.Windows.Forms.Label
        Me.lblPaymentType = New System.Windows.Forms.Label
        Me.lblPolicyNumber = New System.Windows.Forms.Label
        Me._lblMediaType_0 = New System.Windows.Forms.Label
        Me.lblMediaReference = New System.Windows.Forms.Label
        Me.lblPaymentStatus = New System.Windows.Forms.Label
        Me.lblDrawnBankName = New System.Windows.Forms.Label
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.cmdNewSearch = New System.Windows.Forms.Button
        Me.cmdFindNow = New System.Windows.Forms.Button
        Me.lvwFindReceipts = New System.Windows.Forms.ListView
        Me._lvwFindReceipts_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindReceipts_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindReceipts_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindReceipts_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindReceipts_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindReceipts_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindReceipts_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindReceipts_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindReceipts_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindReceipts_ColumnHeader_10 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindReceipts_ColumnHeader_11 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindReceipts_ColumnHeader_12 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindReceipts_ColumnHeader_13 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindReceipts_ColumnHeader_14 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindReceipts_ColumnHeader_15 = New System.Windows.Forms.ColumnHeader
        Me._lvwFindReceipts_ColumnHeader_16 = New System.Windows.Forms.ColumnHeader
        Me.fraFindReceipts.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdSelectAll
        '
        Me.cmdSelectAll.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelectAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelectAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSelectAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelectAll.Location = New System.Drawing.Point(328, 384)
        Me.cmdSelectAll.Name = "cmdSelectAll"
        Me.cmdSelectAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelectAll.Size = New System.Drawing.Size(81, 22)
        Me.cmdSelectAll.TabIndex = 15
        Me.cmdSelectAll.Text = "&Select All"
        Me.cmdSelectAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSelectAll.UseVisualStyleBackColor = False
        '
        'cmdUpdateAllSelected
        '
        Me.cmdUpdateAllSelected.BackColor = System.Drawing.SystemColors.Control
        Me.cmdUpdateAllSelected.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdUpdateAllSelected.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdUpdateAllSelected.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdUpdateAllSelected.Location = New System.Drawing.Point(416, 384)
        Me.cmdUpdateAllSelected.Name = "cmdUpdateAllSelected"
        Me.cmdUpdateAllSelected.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdUpdateAllSelected.Size = New System.Drawing.Size(121, 22)
        Me.cmdUpdateAllSelected.TabIndex = 16
        Me.cmdUpdateAllSelected.Text = "&Update All Selected"
        Me.cmdUpdateAllSelected.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdUpdateAllSelected.UseVisualStyleBackColor = False
        '
        'cmdOk
        '
        Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOk.Location = New System.Drawing.Point(549, 384)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(81, 22)
        Me.cmdOk.TabIndex = 17
        Me.cmdOk.Text = "O&k"
        Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOk.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(640, 384)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(81, 22)
        Me.cmdCancel.TabIndex = 18
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "")
        '
        'fraFindReceipts
        '
        Me.fraFindReceipts.BackColor = System.Drawing.SystemColors.Control
        Me.fraFindReceipts.Controls.Add(Me._cmdPolicyNumber_1)
        Me.fraFindReceipts.Controls.Add(Me.txtClientCode)
        Me.fraFindReceipts.Controls.Add(Me.txtDocumentRef)
        Me.fraFindReceipts.Controls.Add(Me.txtCollectionDateTo)
        Me.fraFindReceipts.Controls.Add(Me.cboDrawnBankName)
        Me.fraFindReceipts.Controls.Add(Me.txtCollectionDateFrom)
        Me.fraFindReceipts.Controls.Add(Me.cboBranch)
        Me.fraFindReceipts.Controls.Add(Me._cmdClientCode_0)
        Me.fraFindReceipts.Controls.Add(Me.txtPolicyNumber)
        Me.fraFindReceipts.Controls.Add(Me.txtMediaReference)
        Me.fraFindReceipts.Controls.Add(Me.uctBankAccount)
        Me.fraFindReceipts.Controls.Add(Me.cboMediaTypeStatus)
        Me.fraFindReceipts.Controls.Add(Me.lblDocumentRef)
        Me.fraFindReceipts.Controls.Add(Me.lblClientCode)
        Me.fraFindReceipts.Controls.Add(Me.lblBranch)
        Me.fraFindReceipts.Controls.Add(Me.lblBank)
        Me.fraFindReceipts.Controls.Add(Me.lblCollectionDateFrom)
        Me.fraFindReceipts.Controls.Add(Me.lblPaymentType)
        Me.fraFindReceipts.Controls.Add(Me.lblPolicyNumber)
        Me.fraFindReceipts.Controls.Add(Me._lblMediaType_0)
        Me.fraFindReceipts.Controls.Add(Me.lblMediaReference)
        Me.fraFindReceipts.Controls.Add(Me.lblPaymentStatus)
        Me.fraFindReceipts.Controls.Add(Me.lblDrawnBankName)
        Me.fraFindReceipts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraFindReceipts.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFindReceipts.Location = New System.Drawing.Point(4, 4)
        Me.fraFindReceipts.Name = "fraFindReceipts"
        Me.fraFindReceipts.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFindReceipts.Size = New System.Drawing.Size(611, 143)
        Me.fraFindReceipts.TabIndex = 13
        Me.fraFindReceipts.TabStop = False
        Me.fraFindReceipts.Text = "Receipts"
        '
        '_cmdPolicyNumber_1
        '
        Me._cmdPolicyNumber_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPolicyNumber_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPolicyNumber_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPolicyNumber_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPolicyNumber_1.Location = New System.Drawing.Point(573, 46)
        Me._cmdPolicyNumber_1.Name = "_cmdPolicyNumber_1"
        Me._cmdPolicyNumber_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPolicyNumber_1.Size = New System.Drawing.Size(30, 20)
        Me._cmdPolicyNumber_1.TabIndex = 9
        Me._cmdPolicyNumber_1.Text = "..."
        Me._cmdPolicyNumber_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPolicyNumber_1.UseVisualStyleBackColor = False
        '
        'txtClientCode
        '
        Me.txtClientCode.AcceptsReturn = True
        Me.txtClientCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtClientCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClientCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClientCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClientCode.Location = New System.Drawing.Point(138, 46)
        Me.txtClientCode.MaxLength = 60
        Me.txtClientCode.Name = "txtClientCode"
        Me.txtClientCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClientCode.Size = New System.Drawing.Size(121, 20)
        Me.txtClientCode.TabIndex = 2
        '
        'txtDocumentRef
        '
        Me.txtDocumentRef.AcceptsReturn = True
        Me.txtDocumentRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtDocumentRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDocumentRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDocumentRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDocumentRef.Location = New System.Drawing.Point(450, 117)
        Me.txtDocumentRef.MaxLength = 0
        Me.txtDocumentRef.Name = "txtDocumentRef"
        Me.txtDocumentRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDocumentRef.Size = New System.Drawing.Size(153, 20)
        Me.txtDocumentRef.TabIndex = 12
        '
        'txtCollectionDateTo
        '
        Me.txtCollectionDateTo.AcceptsReturn = True
        Me.txtCollectionDateTo.BackColor = System.Drawing.SystemColors.Window
        Me.txtCollectionDateTo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCollectionDateTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCollectionDateTo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCollectionDateTo.Location = New System.Drawing.Point(450, 69)
        Me.txtCollectionDateTo.MaxLength = 0
        Me.txtCollectionDateTo.Name = "txtCollectionDateTo"
        Me.txtCollectionDateTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCollectionDateTo.Size = New System.Drawing.Size(153, 20)
        Me.txtCollectionDateTo.TabIndex = 10
        '
        'cboDrawnBankName
        '
        Me.cboDrawnBankName.DefaultItemId = 0
        Me.cboDrawnBankName.FirstItem = ""
        Me.cboDrawnBankName.ItemId = 0
        Me.cboDrawnBankName.ListIndex = -1
        Me.cboDrawnBankName.Location = New System.Drawing.Point(138, 112)
        Me.cboDrawnBankName.Name = "cboDrawnBankName"
        Me.cboDrawnBankName.PMLookupProductFamily = 1
        Me.cboDrawnBankName.SingleItemId = 0
        Me.cboDrawnBankName.Size = New System.Drawing.Size(153, 21)
        Me.cboDrawnBankName.Sorted = True
        Me.cboDrawnBankName.TabIndex = 6
        Me.cboDrawnBankName.TableName = "CashListItem_Bank"
        Me.cboDrawnBankName.ToolTipText = ""
        Me.cboDrawnBankName.WhereClause = ""
        '
        'txtCollectionDateFrom
        '
        Me.txtCollectionDateFrom.AcceptsReturn = True
        Me.txtCollectionDateFrom.BackColor = System.Drawing.SystemColors.Window
        Me.txtCollectionDateFrom.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCollectionDateFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCollectionDateFrom.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCollectionDateFrom.Location = New System.Drawing.Point(138, 70)
        Me.txtCollectionDateFrom.MaxLength = 0
        Me.txtCollectionDateFrom.Name = "txtCollectionDateFrom"
        Me.txtCollectionDateFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCollectionDateFrom.Size = New System.Drawing.Size(153, 20)
        Me.txtCollectionDateFrom.TabIndex = 4
        '
        'cboBranch
        '
        Me.cboBranch.BackColor = System.Drawing.SystemColors.Window
        Me.cboBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBranch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBranch.Location = New System.Drawing.Point(138, 22)
        Me.cboBranch.Name = "cboBranch"
        Me.cboBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBranch.Size = New System.Drawing.Size(153, 21)
        Me.cboBranch.TabIndex = 1
        '
        '_cmdClientCode_0
        '
        Me._cmdClientCode_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdClientCode_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdClientCode_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdClientCode_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdClientCode_0.Location = New System.Drawing.Point(259, 47)
        Me._cmdClientCode_0.Name = "_cmdClientCode_0"
        Me._cmdClientCode_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdClientCode_0.Size = New System.Drawing.Size(30, 20)
        Me._cmdClientCode_0.TabIndex = 3
        Me._cmdClientCode_0.Text = "..."
        Me._cmdClientCode_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdClientCode_0.UseVisualStyleBackColor = False
        '
        'txtPolicyNumber
        '
        Me.txtPolicyNumber.AcceptsReturn = True
        Me.txtPolicyNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolicyNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicyNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicyNumber.Location = New System.Drawing.Point(450, 47)
        Me.txtPolicyNumber.MaxLength = 30
        Me.txtPolicyNumber.Name = "txtPolicyNumber"
        Me.txtPolicyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicyNumber.Size = New System.Drawing.Size(121, 20)
        Me.txtPolicyNumber.TabIndex = 8
        '
        'txtMediaReference
        '
        Me.txtMediaReference.AcceptsReturn = True
        Me.txtMediaReference.BackColor = System.Drawing.SystemColors.Window
        Me.txtMediaReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMediaReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMediaReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMediaReference.Location = New System.Drawing.Point(138, 92)
        Me.txtMediaReference.MaxLength = 25
        Me.txtMediaReference.Name = "txtMediaReference"
        Me.txtMediaReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMediaReference.Size = New System.Drawing.Size(153, 20)
        Me.txtMediaReference.TabIndex = 5
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
        Me.uctBankAccount.TabIndex = 7
        Me.uctBankAccount.ToolTipText = ""
        Me.uctBankAccount.WhatsThisHelpID = 0
        '
        'cboMediaTypeStatus
        '
        Me.cboMediaTypeStatus.DefaultItemId = 0
        Me.cboMediaTypeStatus.FirstItem = ""
        Me.cboMediaTypeStatus.ItemId = 0
        Me.cboMediaTypeStatus.ListIndex = -1
        Me.cboMediaTypeStatus.Location = New System.Drawing.Point(450, 91)
        Me.cboMediaTypeStatus.Name = "cboMediaTypeStatus"
        Me.cboMediaTypeStatus.PMLookupProductFamily = 1
        Me.cboMediaTypeStatus.SingleItemId = 0
        Me.cboMediaTypeStatus.Size = New System.Drawing.Size(153, 21)
        Me.cboMediaTypeStatus.Sorted = True
        Me.cboMediaTypeStatus.TabIndex = 11
        Me.cboMediaTypeStatus.TableName = "MediaType_Status"
        Me.cboMediaTypeStatus.ToolTipText = ""
        Me.cboMediaTypeStatus.WhereClause = ""
        '
        'lblDocumentRef
        '
        Me.lblDocumentRef.BackColor = System.Drawing.SystemColors.Control
        Me.lblDocumentRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDocumentRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocumentRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDocumentRef.Location = New System.Drawing.Point(312, 120)
        Me.lblDocumentRef.Name = "lblDocumentRef"
        Me.lblDocumentRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDocumentRef.Size = New System.Drawing.Size(97, 17)
        Me.lblDocumentRef.TabIndex = 100
        Me.lblDocumentRef.Text = "Document Ref:"
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
        Me.lblClientCode.TabIndex = 100
        Me.lblClientCode.Text = "Client Code:"
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
        Me.lblBranch.TabIndex = 100
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
        Me.lblBank.TabIndex = 100
        Me.lblBank.Text = "Bank Account:"
        '
        'lblCollectionDateFrom
        '
        Me.lblCollectionDateFrom.BackColor = System.Drawing.SystemColors.Control
        Me.lblCollectionDateFrom.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCollectionDateFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCollectionDateFrom.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCollectionDateFrom.Location = New System.Drawing.Point(8, 70)
        Me.lblCollectionDateFrom.Name = "lblCollectionDateFrom"
        Me.lblCollectionDateFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCollectionDateFrom.Size = New System.Drawing.Size(125, 17)
        Me.lblCollectionDateFrom.TabIndex = 100
        Me.lblCollectionDateFrom.Text = "Collection Date From:"
        '
        'lblPaymentType
        '
        Me.lblPaymentType.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentType.Location = New System.Drawing.Point(312, 46)
        Me.lblPaymentType.Name = "lblPaymentType"
        Me.lblPaymentType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentType.Size = New System.Drawing.Size(125, 17)
        Me.lblPaymentType.TabIndex = 100
        Me.lblPaymentType.Text = "Policy Number:"
        '
        'lblPolicyNumber
        '
        Me.lblPolicyNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyNumber.Location = New System.Drawing.Point(312, 70)
        Me.lblPolicyNumber.Name = "lblPolicyNumber"
        Me.lblPolicyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyNumber.Size = New System.Drawing.Size(141, 1)
        Me.lblPolicyNumber.TabIndex = 100
        Me.lblPolicyNumber.Text = "Policy Number:"
        '
        '_lblMediaType_0
        '
        Me._lblMediaType_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblMediaType_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblMediaType_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblMediaType_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblMediaType_0.Location = New System.Drawing.Point(312, 92)
        Me._lblMediaType_0.Name = "_lblMediaType_0"
        Me._lblMediaType_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblMediaType_0.Size = New System.Drawing.Size(141, 17)
        Me._lblMediaType_0.TabIndex = 100
        Me._lblMediaType_0.Text = "Media Type Status:"
        '
        'lblMediaReference
        '
        Me.lblMediaReference.BackColor = System.Drawing.SystemColors.Control
        Me.lblMediaReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMediaReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMediaReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMediaReference.Location = New System.Drawing.Point(8, 94)
        Me.lblMediaReference.Name = "lblMediaReference"
        Me.lblMediaReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMediaReference.Size = New System.Drawing.Size(125, 17)
        Me.lblMediaReference.TabIndex = 100
        Me.lblMediaReference.Text = "Media Reference:"
        '
        'lblPaymentStatus
        '
        Me.lblPaymentStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentStatus.Location = New System.Drawing.Point(312, 69)
        Me.lblPaymentStatus.Name = "lblPaymentStatus"
        Me.lblPaymentStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentStatus.Size = New System.Drawing.Size(141, 21)
        Me.lblPaymentStatus.TabIndex = 100
        Me.lblPaymentStatus.Text = "Collection Date To:"
        '
        'lblDrawnBankName
        '
        Me.lblDrawnBankName.BackColor = System.Drawing.SystemColors.Control
        Me.lblDrawnBankName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDrawnBankName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDrawnBankName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDrawnBankName.Location = New System.Drawing.Point(8, 116)
        Me.lblDrawnBankName.Name = "lblDrawnBankName"
        Me.lblDrawnBankName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDrawnBankName.Size = New System.Drawing.Size(125, 17)
        Me.lblDrawnBankName.TabIndex = 100
        Me.lblDrawnBankName.Text = "Drawn Bank Name:"
        '
        'cmdNewSearch
        '
        Me.cmdNewSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewSearch.Location = New System.Drawing.Point(626, 44)
        Me.cmdNewSearch.Name = "cmdNewSearch"
        Me.cmdNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewSearch.Size = New System.Drawing.Size(81, 22)
        Me.cmdNewSearch.TabIndex = 14
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
        Me.cmdFindNow.Location = New System.Drawing.Point(626, 12)
        Me.cmdFindNow.Name = "cmdFindNow"
        Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindNow.Size = New System.Drawing.Size(81, 22)
        Me.cmdFindNow.TabIndex = 13
        Me.cmdFindNow.Text = "F&ind Now"
        Me.cmdFindNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindNow.UseVisualStyleBackColor = False
        '
        'lvwFindReceipts
        '
        Me.lvwFindReceipts.BackColor = System.Drawing.SystemColors.Window
        Me.lvwFindReceipts.CheckBoxes = True
        Me.lvwFindReceipts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwFindReceipts_ColumnHeader_1, Me._lvwFindReceipts_ColumnHeader_2, Me._lvwFindReceipts_ColumnHeader_3, Me._lvwFindReceipts_ColumnHeader_4, Me._lvwFindReceipts_ColumnHeader_5, Me._lvwFindReceipts_ColumnHeader_6, Me._lvwFindReceipts_ColumnHeader_7, Me._lvwFindReceipts_ColumnHeader_8, Me._lvwFindReceipts_ColumnHeader_9, Me._lvwFindReceipts_ColumnHeader_10, Me._lvwFindReceipts_ColumnHeader_11, Me._lvwFindReceipts_ColumnHeader_12, Me._lvwFindReceipts_ColumnHeader_13, Me._lvwFindReceipts_ColumnHeader_14, Me._lvwFindReceipts_ColumnHeader_15, Me._lvwFindReceipts_ColumnHeader_16})
        Me.lvwFindReceipts.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwFindReceipts.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwFindReceipts.FullRowSelect = True
        Me.lvwFindReceipts.GridLines = True
        Me.lvwFindReceipts.Location = New System.Drawing.Point(4, 150)
        Me.lvwFindReceipts.Name = "lvwFindReceipts"
        Me.lvwFindReceipts.Size = New System.Drawing.Size(713, 213)
        Me.lvwFindReceipts.TabIndex = 20
        Me.lvwFindReceipts.UseCompatibleStateImageBehavior = False
        Me.lvwFindReceipts.View = System.Windows.Forms.View.Details
        '
        '_lvwFindReceipts_ColumnHeader_1
        '
        Me._lvwFindReceipts_ColumnHeader_1.Text = "Select"
        Me._lvwFindReceipts_ColumnHeader_1.Width = 67
        '
        '_lvwFindReceipts_ColumnHeader_2
        '
        Me._lvwFindReceipts_ColumnHeader_2.Text = "Document Ref"
        Me._lvwFindReceipts_ColumnHeader_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._lvwFindReceipts_ColumnHeader_2.Width = 97
        '
        '_lvwFindReceipts_ColumnHeader_3
        '
        Me._lvwFindReceipts_ColumnHeader_3.Text = "Branch"
        Me._lvwFindReceipts_ColumnHeader_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._lvwFindReceipts_ColumnHeader_3.Width = 97
        '
        '_lvwFindReceipts_ColumnHeader_4
        '
        Me._lvwFindReceipts_ColumnHeader_4.Text = "Client Code"
        Me._lvwFindReceipts_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._lvwFindReceipts_ColumnHeader_4.Width = 97
        '
        '_lvwFindReceipts_ColumnHeader_5
        '
        Me._lvwFindReceipts_ColumnHeader_5.Text = "Client Name"
        Me._lvwFindReceipts_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._lvwFindReceipts_ColumnHeader_5.Width = 97
        '
        '_lvwFindReceipts_ColumnHeader_6
        '
        Me._lvwFindReceipts_ColumnHeader_6.Text = "Policy Number"
        Me._lvwFindReceipts_ColumnHeader_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._lvwFindReceipts_ColumnHeader_6.Width = 97
        '
        '_lvwFindReceipts_ColumnHeader_7
        '
        Me._lvwFindReceipts_ColumnHeader_7.Text = "Media Type"
        Me._lvwFindReceipts_ColumnHeader_7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._lvwFindReceipts_ColumnHeader_7.Width = 97
        '
        '_lvwFindReceipts_ColumnHeader_8
        '
        Me._lvwFindReceipts_ColumnHeader_8.Text = "Media reference/Alternate reference"
        Me._lvwFindReceipts_ColumnHeader_8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._lvwFindReceipts_ColumnHeader_8.Width = 134
        '
        '_lvwFindReceipts_ColumnHeader_9
        '
        Me._lvwFindReceipts_ColumnHeader_9.Text = "Drawn Bank Name"
        Me._lvwFindReceipts_ColumnHeader_9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._lvwFindReceipts_ColumnHeader_9.Width = 97
        '
        '_lvwFindReceipts_ColumnHeader_10
        '
        Me._lvwFindReceipts_ColumnHeader_10.Text = "Media type Status"
        Me._lvwFindReceipts_ColumnHeader_10.Width = 97
        '
        '_lvwFindReceipts_ColumnHeader_11
        '
        Me._lvwFindReceipts_ColumnHeader_11.Text = "CashlistItemId"
        Me._lvwFindReceipts_ColumnHeader_11.Width = 0
        '
        '_lvwFindReceipts_ColumnHeader_12
        '
        Me._lvwFindReceipts_ColumnHeader_12.Text = "MediaTypeId"
        Me._lvwFindReceipts_ColumnHeader_12.Width = 0
        '
        '_lvwFindReceipts_ColumnHeader_13
        '
        Me._lvwFindReceipts_ColumnHeader_13.Text = "MediaTypeStatusid"
        Me._lvwFindReceipts_ColumnHeader_13.Width = 0
        '
        '_lvwFindReceipts_ColumnHeader_14
        '
        Me._lvwFindReceipts_ColumnHeader_14.Text = "InsuranceFileId"
        Me._lvwFindReceipts_ColumnHeader_14.Width = 0
        '
        '_lvwFindReceipts_ColumnHeader_15
        '
        Me._lvwFindReceipts_ColumnHeader_15.Text = "UpdatedDate"
        Me._lvwFindReceipts_ColumnHeader_15.Width = 0
        '
        '_lvwFindReceipts_ColumnHeader_16
        '
        Me._lvwFindReceipts_ColumnHeader_16.Text = "Comments"
        Me._lvwFindReceipts_ColumnHeader_16.Width = 0
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(723, 413)
        Me.Controls.Add(Me.cmdSelectAll)
        Me.Controls.Add(Me.cmdUpdateAllSelected)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.fraFindReceipts)
        Me.Controls.Add(Me.cmdNewSearch)
        Me.Controls.Add(Me.cmdFindNow)
        Me.Controls.Add(Me.lvwFindReceipts)
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
        Me.Text = "Find: Receipt(s)"
        Me.fraFindReceipts.ResumeLayout(False)
        Me.fraFindReceipts.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
	Sub InitializelblMediaType()
		Me.lblMediaType(0) = _lblMediaType_0
	End Sub
	Sub InitializecmdPolicyNumber()
		Me.cmdPolicyNumber(1) = _cmdPolicyNumber_1
	End Sub
	Sub InitializecmdClientCode()
		Me.cmdClientCode(0) = _cmdClientCode_0
    End Sub
    Public WithEvents txtClientCode As System.Windows.Forms.TextBox
    Private WithEvents _cmdPolicyNumber_1 As System.Windows.Forms.Button
#End Region 
End Class