<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmInterface
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        isInitializingComponent = True
        InitializeComponent()
        isInitializingComponent = False
        InitializelblMarkedStatus()
        Form_Initialize_Renamed()
    End Sub
    Private Sub Ctx_mnuAddEditComment_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuAddEditComment.Opening
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        Ctx_mnuAddEditComment.Items.Clear()
        'We are moving the submenus from original menu to the context menu before displaying it
        For Each item As System.Windows.Forms.ToolStripItem In mnuAddEditComment.DropDownItems
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            Ctx_mnuAddEditComment.Items.Add(item)
        Next item
        e.Cancel = False
    End Sub
    Private Sub Ctx_mnuAddEditComment_Closing(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuAddEditComment.Closing
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        'We are moving the submenus the context menu back to the original menu after displaying
        For Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuAddEditComment.Items
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            mnuAddEditComment.DropDownItems.Add(item)
        Next item
    End Sub
    Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
        Dispose(True)
    End Sub
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    Public WithEvents mnuAddComment As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuEditComment As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuAddEditComment As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
    Public WithEvents cmdAllocate As System.Windows.Forms.Button
    Public WithEvents cboTransType As System.Windows.Forms.ComboBox
    Public WithEvents txtAlternateRef As System.Windows.Forms.TextBox
    Public WithEvents cmdwriteoff As System.Windows.Forms.Button
    Public WithEvents TxttotalWriteoff As System.Windows.Forms.TextBox
    Public WithEvents cmdReport As System.Windows.Forms.Button
    Public WithEvents uctAnchor As uSIRCommonControls.uctAnchor
    Public WithEvents cmdMarkAll As System.Windows.Forms.Button
    Public WithEvents divTransDetails As uSIRCommonControls.uctDivider
    Public WithEvents cmdDrill As System.Windows.Forms.Button
    Public WithEvents cmdMarkTrans As System.Windows.Forms.Button
    Public WithEvents cmdMarkEntries As System.Windows.Forms.Button
    Public WithEvents cmdPartPay As System.Windows.Forms.Button
    Public WithEvents imglImages As System.Windows.Forms.ImageList
    Public WithEvents divTransactions As uSIRCommonControls.uctDivider
    Public WithEvents divSearch As uSIRCommonControls.uctDivider
    Public WithEvents txtMarked As System.Windows.Forms.TextBox
    Private WithEvents _lvwEntries_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwEntries_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwEntries_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwEntries_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwEntries_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwEntries_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwEntries_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwEntries_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwEntries_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwEntries_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwEntries As System.Windows.Forms.ListView
    Private WithEvents _lvwTransactions_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwTransactions_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwTransactions_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwTransactions_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwTransactions_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwTransactions_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwTransactions_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwTransactions_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwTransactions_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwTransactions_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwTransactions_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwTransactions_ColumnHeader_12 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwTransactions_ColumnHeader_13 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwTransactions_ColumnHeader_14 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwTransactions As System.Windows.Forms.ListView
    Public WithEvents cboMonth As System.Windows.Forms.ComboBox
    Public WithEvents cboMarkedStatus As System.Windows.Forms.ComboBox
    Public WithEvents cmdFindAccount As System.Windows.Forms.Button
    Public WithEvents txtAccountCode As System.Windows.Forms.TextBox
    Public WithEvents optEffectiveDate As System.Windows.Forms.RadioButton
    Public WithEvents optTransDate As System.Windows.Forms.RadioButton
    Public WithEvents cmdBinder As System.Windows.Forms.Button
    Public WithEvents cmdPay As System.Windows.Forms.Button
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents stbStatus As System.Windows.Forms.StatusStrip
    Public WithEvents optViewByTransaction As System.Windows.Forms.RadioButton
    Public WithEvents optViewByAccount As System.Windows.Forms.RadioButton
    Public WithEvents fraViewBy As System.Windows.Forms.GroupBox
    Public WithEvents cmdFindNow As System.Windows.Forms.Button
    Public WithEvents cmdNewSearch As System.Windows.Forms.Button
    Public WithEvents lblTransType As System.Windows.Forms.Label
    Public WithEvents lblAlternateRef As System.Windows.Forms.Label
    Public WithEvents LblTotalWriteoff As System.Windows.Forms.Label
    Public WithEvents lblMarked As System.Windows.Forms.Label
    Public WithEvents lblAccountCode As System.Windows.Forms.Label
    Private WithEvents _lblMarkedStatus_0 As System.Windows.Forms.Label
    Public WithEvents lblMonth As System.Windows.Forms.Label
    Public WithEvents lblPaymentGroup As System.Windows.Forms.Label
    Public lblMarkedStatus(0) As System.Windows.Forms.Label
    Public WithEvents Ctx_mnuAddEditComment As System.Windows.Forms.ContextMenuStrip
    Private WithEvents listBoxComboBoxHelper1 As Artinsoft.VB6.Gui.ListControlHelper
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip()
        Me.mnuAddEditComment = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddComment = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEditComment = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmdAllocate = New System.Windows.Forms.Button()
        Me.cboTransType = New System.Windows.Forms.ComboBox()
        Me.txtAlternateRef = New System.Windows.Forms.TextBox()
        Me.cmdwriteoff = New System.Windows.Forms.Button()
        Me.TxttotalWriteoff = New System.Windows.Forms.TextBox()
        Me.cmdReport = New System.Windows.Forms.Button()
        Me.uctAnchor = New uSIRCommonControls.uctAnchor()
        Me.cmdMarkAll = New System.Windows.Forms.Button()
        Me.divTransDetails = New uSIRCommonControls.uctDivider()
        Me.cmdDrill = New System.Windows.Forms.Button()
        Me.cmdMarkTrans = New System.Windows.Forms.Button()
        Me.cmdMarkEntries = New System.Windows.Forms.Button()
        Me.cmdPartPay = New System.Windows.Forms.Button()
        Me.imglImages = New System.Windows.Forms.ImageList(Me.components)
        Me.divTransactions = New uSIRCommonControls.uctDivider()
        Me.divSearch = New uSIRCommonControls.uctDivider()
        Me.txtMarked = New System.Windows.Forms.TextBox()
        Me.lvwEntries = New System.Windows.Forms.ListView()
        Me._lvwEntries_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwEntries_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwEntries_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwEntries_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwEntries_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwEntries_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwEntries_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwEntries_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwEntries_ColumnHeader_9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwEntries_ColumnHeader_10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwEntries_ColumnHeader_11 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lvwTransactions = New System.Windows.Forms.ListView()
        Me._lvwTransactions_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTransactions_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTransactions_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTransactions_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTransactions_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTransactions_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTransactions_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTransactions_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTransactions_ColumnHeader_9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTransactions_ColumnHeader_10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTransactions_ColumnHeader_11 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTransactions_ColumnHeader_12 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTransactions_ColumnHeader_13 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTransactions_ColumnHeader_14 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTransactions_ColumnHeader_15 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTransactions_ColumnHeader_16 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTransactions_ColumnHeader_17 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cboMonth = New System.Windows.Forms.ComboBox()
        Me.cboMarkedStatus = New System.Windows.Forms.ComboBox()
        Me.cmdFindAccount = New System.Windows.Forms.Button()
        Me.txtAccountCode = New System.Windows.Forms.TextBox()
        Me.optEffectiveDate = New System.Windows.Forms.RadioButton()
        Me.optTransDate = New System.Windows.Forms.RadioButton()
        Me.cmdBinder = New System.Windows.Forms.Button()
        Me.cmdPay = New System.Windows.Forms.Button()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.stbStatus = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsPgBarTransactions = New System.Windows.Forms.ToolStripProgressBar()
        Me.fraViewBy = New System.Windows.Forms.GroupBox()
        Me.optViewByTransaction = New System.Windows.Forms.RadioButton()
        Me.optViewByAccount = New System.Windows.Forms.RadioButton()
        Me.cmdFindNow = New System.Windows.Forms.Button()
        Me.cmdNewSearch = New System.Windows.Forms.Button()
        Me.lblTransType = New System.Windows.Forms.Label()
        Me.lblAlternateRef = New System.Windows.Forms.Label()
        Me.LblTotalWriteoff = New System.Windows.Forms.Label()
        Me.lblMarked = New System.Windows.Forms.Label()
        Me.lblAccountCode = New System.Windows.Forms.Label()
        Me._lblMarkedStatus_0 = New System.Windows.Forms.Label()
        Me.lblMonth = New System.Windows.Forms.Label()
        Me.lblPaymentGroup = New System.Windows.Forms.Label()
        Me.chkAllocationPeriod = New System.Windows.Forms.CheckedListBox()
        Me.lblAllocationPeriod = New System.Windows.Forms.Label()
        Me.lblCurrency = New System.Windows.Forms.Label()
        Me.uctTransactionCurrency = New UserControls.CurrencyLookup()
        Me.Ctx_mnuAddEditComment = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.listBoxComboBoxHelper1 = New Artinsoft.VB6.Gui.ListControlHelper(Me.components)
        Me.cboPaymentGroup = New System.Windows.Forms.ComboBox()
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.lvwInstalmentEntries = New System.Windows.Forms.ListView()
        Me._lvwInstalmentEntries_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwInstalmentEntries_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwInstalmentEntries_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwInstalmentEntries_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwInstalmentEntries_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwInstalmentEntries_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwInstalmentEntries_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwInstalmentEntries_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lblDueDateFrom = New System.Windows.Forms.Label()
        Me.lblDueDateTo = New System.Windows.Forms.Label()
        Me.txtDueDateTo = New System.Windows.Forms.DateTimePicker()
        Me.txtDueDateFrom = New System.Windows.Forms.DateTimePicker()
        Me.lblReference = New System.Windows.Forms.Label()
        Me.lblMediaType = New System.Windows.Forms.Label()
        Me.txtReference = New System.Windows.Forms.TextBox()
        Me.fraCommission = New System.Windows.Forms.GroupBox()
        Me.optGross = New System.Windows.Forms.RadioButton()
        Me.optNet = New System.Windows.Forms.RadioButton()
        Me.lblRecieptPaymentAmount = New System.Windows.Forms.Label()
        Me.txtReciptPaymentAmount = New System.Windows.Forms.TextBox()
        Me.lblReciptPaymentCurrency = New System.Windows.Forms.Label()
        Me.chkDateFilterToinstalmentDueDates = New System.Windows.Forms.CheckBox()
        Me.lblUnallocatedamount = New System.Windows.Forms.Label()
        Me.txtunallocatedAmount = New System.Windows.Forms.TextBox()
        Me.UctCurrency = New UserControls.CurrencyLookup()
        Me.cboMediaType = New PMLookupControl.cboPMLookup()
        Me.PgBarTransactions = New System.Windows.Forms.ProgressBar()
        Me.optDueDate = New System.Windows.Forms.RadioButton()
        Me.cmdUnMarkAll = New System.Windows.Forms.Button()
        Me.MainMenu1.SuspendLayout()
        Me.stbStatus.SuspendLayout()
        Me.fraViewBy.SuspendLayout()
        CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraCommission.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAddEditComment})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(908, 24)
        Me.MainMenu1.TabIndex = 46
        Me.MainMenu1.Visible = False
        '
        'mnuAddEditComment
        '
        Me.mnuAddEditComment.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAddComment, Me.mnuEditComment})
        Me.mnuAddEditComment.Name = "mnuAddEditComment"
        Me.mnuAddEditComment.Size = New System.Drawing.Size(101, 20)
        Me.mnuAddEditComment.Text = "AddEditComment"
        Me.mnuAddEditComment.Visible = False
        '
        'mnuAddComment
        '
        Me.mnuAddComment.Name = "mnuAddComment"
        Me.mnuAddComment.Size = New System.Drawing.Size(141, 22)
        Me.mnuAddComment.Text = "&Add Comment"
        '
        'mnuEditComment
        '
        Me.mnuEditComment.Name = "mnuEditComment"
        Me.mnuEditComment.Size = New System.Drawing.Size(141, 22)
        Me.mnuEditComment.Text = "&Edit Comment"
        '
        'cmdAllocate
        '
        Me.cmdAllocate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAllocate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAllocate.Enabled = False
        Me.cmdAllocate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAllocate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAllocate.Location = New System.Drawing.Point(184, 528)
        Me.cmdAllocate.Name = "cmdAllocate"
        Me.cmdAllocate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAllocate.Size = New System.Drawing.Size(80, 22)
        Me.cmdAllocate.TabIndex = 44
        Me.cmdAllocate.Text = "&Allocate"
        Me.cmdAllocate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAllocate.UseVisualStyleBackColor = False
        '
        'cboTransType
        '
        Me.cboTransType.BackColor = System.Drawing.SystemColors.Window
        Me.cboTransType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTransType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTransType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTransType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboTransType, New Integer() {1, 0, 0})
        Me.cboTransType.Items.AddRange(New Object() {"All Transactions", "Claim Transactions", "Premium Transactions"})
        Me.cboTransType.Location = New System.Drawing.Point(400, 100)
        Me.cboTransType.Name = "cboTransType"
        Me.cboTransType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTransType.Size = New System.Drawing.Size(182, 21)
        Me.cboTransType.Sorted = True
        Me.cboTransType.TabIndex = 43
        '
        'txtAlternateRef
        '
        Me.txtAlternateRef.AcceptsReturn = True
        Me.txtAlternateRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtAlternateRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAlternateRef.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAlternateRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAlternateRef.Location = New System.Drawing.Point(113, 76)
        Me.txtAlternateRef.MaxLength = 0
        Me.txtAlternateRef.Name = "txtAlternateRef"
        Me.txtAlternateRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAlternateRef.Size = New System.Drawing.Size(161, 21)
        Me.txtAlternateRef.TabIndex = 19
        '
        'cmdwriteoff
        '
        Me.cmdwriteoff.BackColor = System.Drawing.SystemColors.Control
        Me.cmdwriteoff.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdwriteoff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdwriteoff.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdwriteoff.Location = New System.Drawing.Point(943, 462)
        Me.cmdwriteoff.Name = "cmdwriteoff"
        Me.cmdwriteoff.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdwriteoff.Size = New System.Drawing.Size(80, 25)
        Me.cmdwriteoff.TabIndex = 40
        Me.cmdwriteoff.Text = "Write Off"
        Me.cmdwriteoff.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdwriteoff.UseVisualStyleBackColor = False
        '
        'TxttotalWriteoff
        '
        Me.TxttotalWriteoff.AcceptsReturn = True
        Me.TxttotalWriteoff.BackColor = System.Drawing.SystemColors.Control
        Me.TxttotalWriteoff.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TxttotalWriteoff.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxttotalWriteoff.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TxttotalWriteoff.Location = New System.Drawing.Point(739, 73)
        Me.TxttotalWriteoff.MaxLength = 0
        Me.TxttotalWriteoff.Name = "TxttotalWriteoff"
        Me.TxttotalWriteoff.ReadOnly = True
        Me.TxttotalWriteoff.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TxttotalWriteoff.Size = New System.Drawing.Size(147, 21)
        Me.TxttotalWriteoff.TabIndex = 39
        Me.TxttotalWriteoff.Text = "0.00"
        Me.TxttotalWriteoff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cmdReport
        '
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReport.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Location = New System.Drawing.Point(943, 77)
        Me.cmdReport.Name = "cmdReport"
        Me.cmdReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReport.Size = New System.Drawing.Size(80, 22)
        Me.cmdReport.TabIndex = 22
        Me.cmdReport.Text = "&Report"
        Me.cmdReport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdReport.UseVisualStyleBackColor = False
        '
        'uctAnchor
        '
        Me.uctAnchor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctAnchor.Location = New System.Drawing.Point(966, 503)
        Me.uctAnchor.Name = "uctAnchor"
        Me.uctAnchor.Size = New System.Drawing.Size(44, 16)
        Me.uctAnchor.TabIndex = 45
        Me.uctAnchor.Visible = False
        '
        'cmdMarkAll
        '
        Me.cmdMarkAll.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMarkAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMarkAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMarkAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMarkAll.Location = New System.Drawing.Point(943, 250)
        Me.cmdMarkAll.Name = "cmdMarkAll"
        Me.cmdMarkAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMarkAll.Size = New System.Drawing.Size(80, 22)
        Me.cmdMarkAll.TabIndex = 25
        Me.cmdMarkAll.Text = "M&ark All"
        Me.cmdMarkAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdMarkAll.UseVisualStyleBackColor = False
        '
        'divTransDetails
        '
        Me.divTransDetails.Caption = "Outstanding Transaction Details"
        Me.divTransDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.divTransDetails.Location = New System.Drawing.Point(4, 386)
        Me.divTransDetails.Name = "divTransDetails"
        Me.divTransDetails.Size = New System.Drawing.Size(1039, 15)
        Me.divTransDetails.TabIndex = 28
        '
        'cmdDrill
        '
        Me.cmdDrill.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDrill.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDrill.Enabled = False
        Me.cmdDrill.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDrill.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDrill.Location = New System.Drawing.Point(943, 324)
        Me.cmdDrill.Name = "cmdDrill"
        Me.cmdDrill.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDrill.Size = New System.Drawing.Size(80, 22)
        Me.cmdDrill.TabIndex = 27
        Me.cmdDrill.Text = "&Drill"
        Me.cmdDrill.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDrill.UseVisualStyleBackColor = False
        '
        'cmdMarkTrans
        '
        Me.cmdMarkTrans.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMarkTrans.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMarkTrans.Enabled = False
        Me.cmdMarkTrans.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMarkTrans.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMarkTrans.Location = New System.Drawing.Point(943, 300)
        Me.cmdMarkTrans.Name = "cmdMarkTrans"
        Me.cmdMarkTrans.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMarkTrans.Size = New System.Drawing.Size(80, 22)
        Me.cmdMarkTrans.TabIndex = 26
        Me.cmdMarkTrans.Text = "&Mark"
        Me.cmdMarkTrans.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdMarkTrans.UseVisualStyleBackColor = False
        '
        'cmdMarkEntries
        '
        Me.cmdMarkEntries.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMarkEntries.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMarkEntries.Enabled = False
        Me.cmdMarkEntries.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMarkEntries.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMarkEntries.Location = New System.Drawing.Point(943, 408)
        Me.cmdMarkEntries.Name = "cmdMarkEntries"
        Me.cmdMarkEntries.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMarkEntries.Size = New System.Drawing.Size(80, 22)
        Me.cmdMarkEntries.TabIndex = 30
        Me.cmdMarkEntries.Text = "Mar&k"
        Me.cmdMarkEntries.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdMarkEntries.UseVisualStyleBackColor = False
        '
        'cmdPartPay
        '
        Me.cmdPartPay.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPartPay.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPartPay.Enabled = False
        Me.cmdPartPay.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPartPay.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPartPay.Location = New System.Drawing.Point(943, 434)
        Me.cmdPartPay.Name = "cmdPartPay"
        Me.cmdPartPay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPartPay.Size = New System.Drawing.Size(80, 22)
        Me.cmdPartPay.TabIndex = 31
        Me.cmdPartPay.Text = "Pa&rt Pay"
        Me.cmdPartPay.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPartPay.UseVisualStyleBackColor = False
        '
        'imglImages
        '
        Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imglImages.Tag = "Sirius For Broking Rules"
        Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.imglImages.Images.SetKeyName(0, "check")
        Me.imglImages.Images.SetKeyName(1, "part")
        Me.imglImages.Images.SetKeyName(2, "blank")
        Me.imglImages.Images.SetKeyName(3, "NoComment")
        Me.imglImages.Images.SetKeyName(4, "Comment")
        '
        'divTransactions
        '
        Me.divTransactions.Caption = "Transactions"
        Me.divTransactions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.divTransactions.Location = New System.Drawing.Point(5, 234)
        Me.divTransactions.Name = "divTransactions"
        Me.divTransactions.Size = New System.Drawing.Size(1039, 14)
        Me.divTransactions.TabIndex = 23
        '
        'divSearch
        '
        Me.divSearch.Caption = "Search"
        Me.divSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.divSearch.Location = New System.Drawing.Point(4, 4)
        Me.divSearch.Name = "divSearch"
        Me.divSearch.Size = New System.Drawing.Size(1039, 16)
        Me.divSearch.TabIndex = 0
        '
        'txtMarked
        '
        Me.txtMarked.AcceptsReturn = True
        Me.txtMarked.BackColor = System.Drawing.SystemColors.Control
        Me.txtMarked.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMarked.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMarked.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMarked.Location = New System.Drawing.Point(739, 49)
        Me.txtMarked.MaxLength = 0
        Me.txtMarked.Name = "txtMarked"
        Me.txtMarked.ReadOnly = True
        Me.txtMarked.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMarked.Size = New System.Drawing.Size(147, 21)
        Me.txtMarked.TabIndex = 15
        Me.txtMarked.Text = "0.00"
        Me.txtMarked.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lvwEntries
        '
        Me.lvwEntries.BackColor = System.Drawing.SystemColors.Window
        Me.lvwEntries.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwEntries, "")
        Me.lvwEntries.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwEntries_ColumnHeader_1, Me._lvwEntries_ColumnHeader_2, Me._lvwEntries_ColumnHeader_3, Me._lvwEntries_ColumnHeader_4, Me._lvwEntries_ColumnHeader_5, Me._lvwEntries_ColumnHeader_6, Me._lvwEntries_ColumnHeader_7, Me._lvwEntries_ColumnHeader_8, Me._lvwEntries_ColumnHeader_9, Me._lvwEntries_ColumnHeader_10, Me._lvwEntries_ColumnHeader_11})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwEntries, False)
        Me.lvwEntries.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwEntries.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwEntries.FullRowSelect = True
        Me.lvwEntries.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwEntries, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwEntries, "")
        Me.lvwEntries.LargeImageList = Me.imglImages
        Me.lvwEntries.Location = New System.Drawing.Point(0, 408)
        Me.lvwEntries.Name = "lvwEntries"
        Me.lvwEntries.Size = New System.Drawing.Size(918, 116)
        Me.listViewHelper1.SetSmallIcons(Me.lvwEntries, "")
        Me.lvwEntries.SmallImageList = Me.imglImages
        Me.listViewHelper1.SetSorted(Me.lvwEntries, False)
        Me.listViewHelper1.SetSortKey(Me.lvwEntries, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwEntries, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwEntries.TabIndex = 29
        Me.lvwEntries.UseCompatibleStateImageBehavior = False
        Me.lvwEntries.View = System.Windows.Forms.View.Details
        '
        '_lvwEntries_ColumnHeader_1
        '
        Me._lvwEntries_ColumnHeader_1.Text = " "
        Me._lvwEntries_ColumnHeader_1.Width = 97
        '
        '_lvwEntries_ColumnHeader_2
        '
        Me._lvwEntries_ColumnHeader_2.Width = 97
        '
        '_lvwEntries_ColumnHeader_3
        '
        Me._lvwEntries_ColumnHeader_3.Width = 97
        '
        '_lvwEntries_ColumnHeader_4
        '
        Me._lvwEntries_ColumnHeader_4.Width = 97
        '
        '_lvwEntries_ColumnHeader_5
        '
        Me._lvwEntries_ColumnHeader_5.Width = 97
        '
        '_lvwEntries_ColumnHeader_6
        '
        Me._lvwEntries_ColumnHeader_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwEntries_ColumnHeader_6.Width = 97
        '
        '_lvwEntries_ColumnHeader_7
        '
        Me._lvwEntries_ColumnHeader_7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwEntries_ColumnHeader_7.Width = 97
        '
        '_lvwEntries_ColumnHeader_8
        '
        Me._lvwEntries_ColumnHeader_8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwEntries_ColumnHeader_8.Width = 97
        '
        '_lvwEntries_ColumnHeader_9
        '
        Me._lvwEntries_ColumnHeader_9.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwEntries_ColumnHeader_9.Width = 97
        '
        '_lvwEntries_ColumnHeader_10
        '
        Me._lvwEntries_ColumnHeader_10.Width = 97
        '
        '_lvwEntries_ColumnHeader_11
        '
        Me._lvwEntries_ColumnHeader_11.Width = 110
        '
        'lvwTransactions
        '
        Me.lvwTransactions.BackColor = System.Drawing.SystemColors.Window
        Me.lvwTransactions.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwTransactions, "")
        Me.lvwTransactions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwTransactions_ColumnHeader_1, Me._lvwTransactions_ColumnHeader_2, Me._lvwTransactions_ColumnHeader_3, Me._lvwTransactions_ColumnHeader_4, Me._lvwTransactions_ColumnHeader_5, Me._lvwTransactions_ColumnHeader_6, Me._lvwTransactions_ColumnHeader_7, Me._lvwTransactions_ColumnHeader_8, Me._lvwTransactions_ColumnHeader_9, Me._lvwTransactions_ColumnHeader_10, Me._lvwTransactions_ColumnHeader_11, Me._lvwTransactions_ColumnHeader_12, Me._lvwTransactions_ColumnHeader_13, Me._lvwTransactions_ColumnHeader_14, Me._lvwTransactions_ColumnHeader_15, Me._lvwTransactions_ColumnHeader_16, Me._lvwTransactions_ColumnHeader_17})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwTransactions, False)
        Me.lvwTransactions.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwTransactions.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwTransactions.FullRowSelect = True
        Me.lvwTransactions.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwTransactions, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwTransactions, "")
        Me.lvwTransactions.LargeImageList = Me.imglImages
        Me.lvwTransactions.Location = New System.Drawing.Point(4, 250)
        Me.lvwTransactions.Name = "lvwTransactions"
        Me.lvwTransactions.Size = New System.Drawing.Size(914, 125)
        Me.listViewHelper1.SetSmallIcons(Me.lvwTransactions, "")
        Me.lvwTransactions.SmallImageList = Me.imglImages
        Me.listViewHelper1.SetSorted(Me.lvwTransactions, False)
        Me.listViewHelper1.SetSortKey(Me.lvwTransactions, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwTransactions, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwTransactions.TabIndex = 24
        Me.lvwTransactions.UseCompatibleStateImageBehavior = False
        Me.lvwTransactions.View = System.Windows.Forms.View.Details
        '
        '_lvwTransactions_ColumnHeader_1
        '
        Me._lvwTransactions_ColumnHeader_1.Text = ""
        Me._lvwTransactions_ColumnHeader_1.Width = 97
        '
        '_lvwTransactions_ColumnHeader_2
        '
        Me._lvwTransactions_ColumnHeader_2.Width = 97
        '
        '_lvwTransactions_ColumnHeader_3
        '
        Me._lvwTransactions_ColumnHeader_3.Width = 97
        '
        '_lvwTransactions_ColumnHeader_4
        '
        Me._lvwTransactions_ColumnHeader_4.Width = 97
        '
        '_lvwTransactions_ColumnHeader_5
        '
        Me._lvwTransactions_ColumnHeader_5.Width = 97
        '
        '_lvwTransactions_ColumnHeader_6
        '
        Me._lvwTransactions_ColumnHeader_6.Width = 97
        '
        '_lvwTransactions_ColumnHeader_7
        '
        Me._lvwTransactions_ColumnHeader_7.Width = 97
        '
        '_lvwTransactions_ColumnHeader_8
        '
        Me._lvwTransactions_ColumnHeader_8.Width = 97
        '
        '_lvwTransactions_ColumnHeader_9
        '
        Me._lvwTransactions_ColumnHeader_9.Width = 97
        '
        '_lvwTransactions_ColumnHeader_10
        '
        Me._lvwTransactions_ColumnHeader_10.Width = 97
        '
        '_lvwTransactions_ColumnHeader_11
        '
        Me._lvwTransactions_ColumnHeader_11.Width = 97
        '
        '_lvwTransactions_ColumnHeader_12
        '
        Me._lvwTransactions_ColumnHeader_12.Width = 97
        '
        '_lvwTransactions_ColumnHeader_13
        '
        Me._lvwTransactions_ColumnHeader_13.Width = 97
        '
        '_lvwTransactions_ColumnHeader_14
        '
        Me._lvwTransactions_ColumnHeader_14.Width = 97
        '
        '_lvwTransactions_ColumnHeader_15
        '
        Me._lvwTransactions_ColumnHeader_15.Width = 110
        '
        'cboMonth
        '
        Me.cboMonth.BackColor = System.Drawing.SystemColors.Window
        Me.cboMonth.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMonth.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMonth.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboMonth, New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0})
        Me.cboMonth.Items.AddRange(New Object() {"All", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"})
        Me.cboMonth.Location = New System.Drawing.Point(506, 75)
        Me.cboMonth.Name = "cboMonth"
        Me.cboMonth.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboMonth.Size = New System.Drawing.Size(76, 21)
        Me.cboMonth.TabIndex = 13
        '
        'cboMarkedStatus
        '
        Me.cboMarkedStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cboMarkedStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboMarkedStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMarkedStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMarkedStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboMarkedStatus, New Integer() {0, 0, 0})
        Me.cboMarkedStatus.Items.AddRange(New Object() {"Any", "No", "Yes"})
        Me.cboMarkedStatus.Location = New System.Drawing.Point(400, 74)
        Me.cboMarkedStatus.Name = "cboMarkedStatus"
        Me.cboMarkedStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboMarkedStatus.Size = New System.Drawing.Size(48, 21)
        Me.cboMarkedStatus.TabIndex = 11
        '
        'cmdFindAccount
        '
        Me.cmdFindAccount.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindAccount.Location = New System.Drawing.Point(257, 26)
        Me.cmdFindAccount.Name = "cmdFindAccount"
        Me.cmdFindAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindAccount.Size = New System.Drawing.Size(17, 19)
        Me.cmdFindAccount.TabIndex = 3
        Me.cmdFindAccount.Text = "…"
        Me.cmdFindAccount.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindAccount.UseVisualStyleBackColor = False
        '
        'txtAccountCode
        '
        Me.txtAccountCode.AcceptsReturn = True
        Me.txtAccountCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccountCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccountCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccountCode.Location = New System.Drawing.Point(113, 25)
        Me.txtAccountCode.MaxLength = 0
        Me.txtAccountCode.Name = "txtAccountCode"
        Me.txtAccountCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccountCode.Size = New System.Drawing.Size(143, 21)
        Me.txtAccountCode.TabIndex = 2
        '
        'optEffectiveDate
        '
        Me.optEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.optEffectiveDate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.optEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.optEffectiveDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optEffectiveDate.Location = New System.Drawing.Point(290, 130)
        Me.optEffectiveDate.Name = "optEffectiveDate"
        Me.optEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optEffectiveDate.Size = New System.Drawing.Size(124, 20)
        Me.optEffectiveDate.TabIndex = 6
        Me.optEffectiveDate.TabStop = True
        Me.optEffectiveDate.Text = "Effective Date:"
        Me.optEffectiveDate.UseVisualStyleBackColor = False
        '
        'optTransDate
        '
        Me.optTransDate.BackColor = System.Drawing.SystemColors.Control
        Me.optTransDate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.optTransDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.optTransDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optTransDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optTransDate.Location = New System.Drawing.Point(414, 129)
        Me.optTransDate.Name = "optTransDate"
        Me.optTransDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optTransDate.Size = New System.Drawing.Size(138, 20)
        Me.optTransDate.TabIndex = 7
        Me.optTransDate.TabStop = True
        Me.optTransDate.Text = "Transaction Date:"
        Me.optTransDate.UseVisualStyleBackColor = False
        '
        'cmdBinder
        '
        Me.cmdBinder.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBinder.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBinder.Enabled = False
        Me.cmdBinder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBinder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBinder.Location = New System.Drawing.Point(8, 528)
        Me.cmdBinder.Name = "cmdBinder"
        Me.cmdBinder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBinder.Size = New System.Drawing.Size(80, 22)
        Me.cmdBinder.TabIndex = 32
        Me.cmdBinder.Text = "&Binder"
        Me.cmdBinder.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdBinder.UseVisualStyleBackColor = False
        '
        'cmdPay
        '
        Me.cmdPay.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPay.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPay.Enabled = False
        Me.cmdPay.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPay.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPay.Location = New System.Drawing.Point(96, 528)
        Me.cmdPay.Name = "cmdPay"
        Me.cmdPay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPay.Size = New System.Drawing.Size(80, 22)
        Me.cmdPay.TabIndex = 33
        Me.cmdPay.Text = "&Pay"
        Me.cmdPay.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPay.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(859, 528)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(80, 22)
        Me.cmdHelp.TabIndex = 36
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(773, 528)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(80, 22)
        Me.cmdCancel.TabIndex = 35
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(685, 528)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(80, 22)
        Me.cmdOK.TabIndex = 34
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'stbStatus
        '
        Me.stbStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel})
        Me.stbStatus.Location = New System.Drawing.Point(0, 576)
        Me.stbStatus.Name = "stbStatus"
        Me.stbStatus.ShowItemToolTips = True
        Me.stbStatus.Size = New System.Drawing.Size(1028, 22)
        Me.stbStatus.TabIndex = 37
        '
        'ToolStripStatusLabel
        '
        Me.ToolStripStatusLabel.Name = "ToolStripStatusLabel"
        Me.ToolStripStatusLabel.Size = New System.Drawing.Size(0, 17)
        '
        'tsPgBarTransactions
        '
        Me.tsPgBarTransactions.Name = "tsPgBarTransactions"
        Me.tsPgBarTransactions.Size = New System.Drawing.Size(100, 16)
        Me.tsPgBarTransactions.Visible = False
        '
        'fraViewBy
        '
        Me.fraViewBy.BackColor = System.Drawing.SystemColors.Control
        Me.fraViewBy.Controls.Add(Me.optViewByTransaction)
        Me.fraViewBy.Controls.Add(Me.optViewByAccount)
        Me.fraViewBy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraViewBy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraViewBy.Location = New System.Drawing.Point(624, 150)
        Me.fraViewBy.Name = "fraViewBy"
        Me.fraViewBy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraViewBy.Size = New System.Drawing.Size(193, 48)
        Me.fraViewBy.TabIndex = 16
        Me.fraViewBy.TabStop = False
        Me.fraViewBy.Text = "View By"
        '
        'optViewByTransaction
        '
        Me.optViewByTransaction.BackColor = System.Drawing.SystemColors.Control
        Me.optViewByTransaction.Checked = True
        Me.optViewByTransaction.Cursor = System.Windows.Forms.Cursors.Default
        Me.optViewByTransaction.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optViewByTransaction.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optViewByTransaction.Location = New System.Drawing.Point(16, 12)
        Me.optViewByTransaction.Name = "optViewByTransaction"
        Me.optViewByTransaction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optViewByTransaction.Size = New System.Drawing.Size(169, 17)
        Me.optViewByTransaction.TabIndex = 17
        Me.optViewByTransaction.TabStop = True
        Me.optViewByTransaction.Text = "Transaction Currency"
        Me.optViewByTransaction.UseVisualStyleBackColor = False
        '
        'optViewByAccount
        '
        Me.optViewByAccount.BackColor = System.Drawing.SystemColors.Control
        Me.optViewByAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.optViewByAccount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optViewByAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optViewByAccount.Location = New System.Drawing.Point(16, 28)
        Me.optViewByAccount.Name = "optViewByAccount"
        Me.optViewByAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optViewByAccount.Size = New System.Drawing.Size(169, 17)
        Me.optViewByAccount.TabIndex = 18
        Me.optViewByAccount.TabStop = True
        Me.optViewByAccount.Text = "Account Currency (x)"
        Me.optViewByAccount.UseVisualStyleBackColor = False
        '
        'cmdFindNow
        '
        Me.cmdFindNow.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindNow.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindNow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindNow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindNow.Location = New System.Drawing.Point(943, 25)
        Me.cmdFindNow.Name = "cmdFindNow"
        Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindNow.Size = New System.Drawing.Size(80, 22)
        Me.cmdFindNow.TabIndex = 20
        Me.cmdFindNow.Text = "F&ind Now"
        Me.cmdFindNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindNow.UseVisualStyleBackColor = False
        '
        'cmdNewSearch
        '
        Me.cmdNewSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewSearch.Location = New System.Drawing.Point(943, 51)
        Me.cmdNewSearch.Name = "cmdNewSearch"
        Me.cmdNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewSearch.Size = New System.Drawing.Size(80, 22)
        Me.cmdNewSearch.TabIndex = 21
        Me.cmdNewSearch.Text = "Ne&w Search"
        Me.cmdNewSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNewSearch.UseVisualStyleBackColor = False
        '
        'lblTransType
        '
        Me.lblTransType.BackColor = System.Drawing.SystemColors.Control
        Me.lblTransType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTransType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTransType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTransType.Location = New System.Drawing.Point(290, 103)
        Me.lblTransType.Name = "lblTransType"
        Me.lblTransType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTransType.Size = New System.Drawing.Size(122, 13)
        Me.lblTransType.TabIndex = 42
        Me.lblTransType.Text = "Trans Type:"
        '
        'lblAlternateRef
        '
        Me.lblAlternateRef.AutoSize = True
        Me.lblAlternateRef.BackColor = System.Drawing.SystemColors.Control
        Me.lblAlternateRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAlternateRef.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAlternateRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAlternateRef.Location = New System.Drawing.Point(12, 84)
        Me.lblAlternateRef.Name = "lblAlternateRef"
        Me.lblAlternateRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAlternateRef.Size = New System.Drawing.Size(87, 13)
        Me.lblAlternateRef.TabIndex = 41
        Me.lblAlternateRef.Text = "Alternate Ref:"
        '
        'LblTotalWriteoff
        '
        Me.LblTotalWriteoff.AutoSize = True
        Me.LblTotalWriteoff.BackColor = System.Drawing.SystemColors.Control
        Me.LblTotalWriteoff.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblTotalWriteoff.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblTotalWriteoff.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblTotalWriteoff.Location = New System.Drawing.Point(635, 77)
        Me.LblTotalWriteoff.Name = "LblTotalWriteoff"
        Me.LblTotalWriteoff.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LblTotalWriteoff.Size = New System.Drawing.Size(91, 13)
        Me.LblTotalWriteoff.TabIndex = 38
        Me.LblTotalWriteoff.Text = "Total WriteOff:"
        '
        'lblMarked
        '
        Me.lblMarked.AutoSize = True
        Me.lblMarked.BackColor = System.Drawing.SystemColors.Control
        Me.lblMarked.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMarked.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMarked.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMarked.Location = New System.Drawing.Point(641, 52)
        Me.lblMarked.Name = "lblMarked"
        Me.lblMarked.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMarked.Size = New System.Drawing.Size(86, 13)
        Me.lblMarked.TabIndex = 14
        Me.lblMarked.Text = "Total Marked:"
        '
        'lblAccountCode
        '
        Me.lblAccountCode.AutoSize = True
        Me.lblAccountCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountCode.Location = New System.Drawing.Point(12, 27)
        Me.lblAccountCode.Name = "lblAccountCode"
        Me.lblAccountCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountCode.Size = New System.Drawing.Size(91, 13)
        Me.lblAccountCode.TabIndex = 1
        Me.lblAccountCode.Text = "Account Code:"
        '
        '_lblMarkedStatus_0
        '
        Me._lblMarkedStatus_0.AutoSize = True
        Me._lblMarkedStatus_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblMarkedStatus_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblMarkedStatus_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblMarkedStatus_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblMarkedStatus_0.Location = New System.Drawing.Point(290, 78)
        Me._lblMarkedStatus_0.Name = "_lblMarkedStatus_0"
        Me._lblMarkedStatus_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblMarkedStatus_0.Size = New System.Drawing.Size(94, 13)
        Me._lblMarkedStatus_0.TabIndex = 10
        Me._lblMarkedStatus_0.Text = "Marked Status:"
        '
        'lblMonth
        '
        Me.lblMonth.AutoSize = True
        Me.lblMonth.BackColor = System.Drawing.SystemColors.Control
        Me.lblMonth.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMonth.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMonth.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMonth.Location = New System.Drawing.Point(454, 80)
        Me.lblMonth.Name = "lblMonth"
        Me.lblMonth.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMonth.Size = New System.Drawing.Size(46, 13)
        Me.lblMonth.TabIndex = 12
        Me.lblMonth.Text = "Month:"
        '
        'lblPaymentGroup
        '
        Me.lblPaymentGroup.AutoSize = True
        Me.lblPaymentGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentGroup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentGroup.Location = New System.Drawing.Point(12, 57)
        Me.lblPaymentGroup.Name = "lblPaymentGroup"
        Me.lblPaymentGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentGroup.Size = New System.Drawing.Size(101, 13)
        Me.lblPaymentGroup.TabIndex = 8
        Me.lblPaymentGroup.Text = "Payment Group:"
        '
        'chkAllocationPeriod
        '
        Me.chkAllocationPeriod.FormattingEnabled = True
        Me.chkAllocationPeriod.HorizontalScrollbar = True
        Me.listBoxComboBoxHelper1.SetItemData(Me.chkAllocationPeriod, New Integer(-1) {})
        Me.chkAllocationPeriod.Location = New System.Drawing.Point(739, 98)
        Me.chkAllocationPeriod.Name = "chkAllocationPeriod"
        Me.chkAllocationPeriod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllocationPeriod.Size = New System.Drawing.Size(147, 20)
        Me.chkAllocationPeriod.TabIndex = 48
        '
        'lblAllocationPeriod
        '
        Me.lblAllocationPeriod.BackColor = System.Drawing.SystemColors.Control
        Me.lblAllocationPeriod.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAllocationPeriod.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAllocationPeriod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAllocationPeriod.Location = New System.Drawing.Point(621, 100)
        Me.lblAllocationPeriod.Name = "lblAllocationPeriod"
        Me.lblAllocationPeriod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAllocationPeriod.Size = New System.Drawing.Size(119, 13)
        Me.lblAllocationPeriod.TabIndex = 47
        Me.lblAllocationPeriod.Text = "Allocation Period:"
        '
        'lblCurrency
        '
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(661, 123)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(67, 16)
        Me.lblCurrency.TabIndex = 49
        Me.lblCurrency.Text = "Currency:"
        '
        'uctTransactionCurrency
        '
        Me.uctTransactionCurrency.CompanyId = 0
        Me.uctTransactionCurrency.CurrencyId = 0
        Me.uctTransactionCurrency.DefaultCurrencyId = 0
        Me.uctTransactionCurrency.FirstItem = "(select all)"
        Me.uctTransactionCurrency.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.uctTransactionCurrency.ListIndex = -1
        Me.uctTransactionCurrency.Location = New System.Drawing.Point(739, 122)
        Me.uctTransactionCurrency.Name = "uctTransactionCurrency"
        Me.uctTransactionCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actCompanyCurrencies
        Me.uctTransactionCurrency.Size = New System.Drawing.Size(147, 21)
        Me.uctTransactionCurrency.TabIndex = 50
        Me.uctTransactionCurrency.ToolTipText = ""
        Me.uctTransactionCurrency.WhatsThisHelpID = 0
        '
        'Ctx_mnuAddEditComment
        '
        Me.Ctx_mnuAddEditComment.Name = "Ctx_mnuAddEditComment"
        Me.Ctx_mnuAddEditComment.Size = New System.Drawing.Size(61, 4)
        '
        'cboPaymentGroup
        '
        Me.cboPaymentGroup.BackColor = System.Drawing.SystemColors.Window
        Me.cboPaymentGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPaymentGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPaymentGroup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPaymentGroup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboPaymentGroup, New Integer(-1) {})
        Me.cboPaymentGroup.Location = New System.Drawing.Point(113, 51)
        Me.cboPaymentGroup.Name = "cboPaymentGroup"
        Me.cboPaymentGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPaymentGroup.Size = New System.Drawing.Size(161, 21)
        Me.cboPaymentGroup.TabIndex = 9
        '
        'lvwInstalmentEntries
        '
        Me.lvwInstalmentEntries.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwInstalmentEntries, "")
        Me.lvwInstalmentEntries.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwInstalmentEntries_ColumnHeader_1, Me._lvwInstalmentEntries_ColumnHeader_2, Me._lvwInstalmentEntries_ColumnHeader_3, Me._lvwInstalmentEntries_ColumnHeader_4, Me._lvwInstalmentEntries_ColumnHeader_5, Me._lvwInstalmentEntries_ColumnHeader_6, Me._lvwInstalmentEntries_ColumnHeader_7, Me._lvwInstalmentEntries_ColumnHeader_8})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwInstalmentEntries, False)
        Me.lvwInstalmentEntries.FullRowSelect = True
        Me.lvwInstalmentEntries.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwInstalmentEntries, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwInstalmentEntries, "")
        Me.lvwInstalmentEntries.LargeImageList = Me.imglImages
        Me.lvwInstalmentEntries.Location = New System.Drawing.Point(0, 408)
        Me.lvwInstalmentEntries.Name = "lvwInstalmentEntries"
        Me.lvwInstalmentEntries.Size = New System.Drawing.Size(918, 116)
        Me.listViewHelper1.SetSmallIcons(Me.lvwInstalmentEntries, "")
        Me.lvwInstalmentEntries.SmallImageList = Me.imglImages
        Me.listViewHelper1.SetSorted(Me.lvwInstalmentEntries, False)
        Me.listViewHelper1.SetSortKey(Me.lvwInstalmentEntries, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwInstalmentEntries, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwInstalmentEntries.TabIndex = 65
        Me.lvwInstalmentEntries.UseCompatibleStateImageBehavior = False
        Me.lvwInstalmentEntries.View = System.Windows.Forms.View.Details
        '
        '_lvwInstalmentEntries_ColumnHeader_1
        '
        Me._lvwInstalmentEntries_ColumnHeader_1.Text = ""
        '
        '_lvwInstalmentEntries_ColumnHeader_2
        '
        Me._lvwInstalmentEntries_ColumnHeader_2.Text = "ColumnHeader"
        '
        '_lvwInstalmentEntries_ColumnHeader_3
        '
        Me._lvwInstalmentEntries_ColumnHeader_3.Text = "ColumnHeader"
        '
        '_lvwInstalmentEntries_ColumnHeader_4
        '
        Me._lvwInstalmentEntries_ColumnHeader_4.Text = "ColumnHeader"
        '
        '_lvwInstalmentEntries_ColumnHeader_5
        '
        Me._lvwInstalmentEntries_ColumnHeader_5.Text = "ColumnHeader"
        '
        '_lvwInstalmentEntries_ColumnHeader_6
        '
        Me._lvwInstalmentEntries_ColumnHeader_6.Text = "ColumnHeader"
        '
        '_lvwInstalmentEntries_ColumnHeader_7
        '
        Me._lvwInstalmentEntries_ColumnHeader_7.Text = "ColumnHeader"
        '
        '_lvwInstalmentEntries_ColumnHeader_8
        '
        Me._lvwInstalmentEntries_ColumnHeader_8.Text = "ColumnHeader"
        '
        'lblDueDateFrom
        '
        Me.lblDueDateFrom.AutoSize = True
        Me.lblDueDateFrom.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDueDateFrom.Location = New System.Drawing.Point(12, 134)
        Me.lblDueDateFrom.Name = "lblDueDateFrom"
        Me.lblDueDateFrom.Size = New System.Drawing.Size(41, 13)
        Me.lblDueDateFrom.TabIndex = 47
        Me.lblDueDateFrom.Text = "From:"
        '
        'lblDueDateTo
        '
        Me.lblDueDateTo.AutoSize = True
        Me.lblDueDateTo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDueDateTo.Location = New System.Drawing.Point(12, 162)
        Me.lblDueDateTo.Name = "lblDueDateTo"
        Me.lblDueDateTo.Size = New System.Drawing.Size(26, 13)
        Me.lblDueDateTo.TabIndex = 49
        Me.lblDueDateTo.Text = "To:"
        '
        'txtDueDateTo
        '
        Me.txtDueDateTo.Checked = False
        Me.txtDueDateTo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDueDateTo.Location = New System.Drawing.Point(113, 154)
        Me.txtDueDateTo.Name = "txtDueDateTo"
        Me.txtDueDateTo.ShowCheckBox = True
        Me.txtDueDateTo.Size = New System.Drawing.Size(161, 21)
        Me.txtDueDateTo.TabIndex = 51
        '
        'txtDueDateFrom
        '
        Me.txtDueDateFrom.Checked = False
        Me.txtDueDateFrom.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDueDateFrom.Location = New System.Drawing.Point(113, 128)
        Me.txtDueDateFrom.Name = "txtDueDateFrom"
        Me.txtDueDateFrom.ShowCheckBox = True
        Me.txtDueDateFrom.Size = New System.Drawing.Size(161, 21)
        Me.txtDueDateFrom.TabIndex = 52
        '
        'lblReference
        '
        Me.lblReference.AutoSize = True
        Me.lblReference.Location = New System.Drawing.Point(12, 109)
        Me.lblReference.Name = "lblReference"
        Me.lblReference.Size = New System.Drawing.Size(70, 13)
        Me.lblReference.TabIndex = 53
        Me.lblReference.Text = "Reference:"
        '
        'lblMediaType
        '
        Me.lblMediaType.AutoSize = True
        Me.lblMediaType.Location = New System.Drawing.Point(12, 185)
        Me.lblMediaType.Name = "lblMediaType"
        Me.lblMediaType.Size = New System.Drawing.Size(77, 13)
        Me.lblMediaType.TabIndex = 54
        Me.lblMediaType.Text = "Media Type:"
        '
        'txtReference
        '
        Me.txtReference.Location = New System.Drawing.Point(113, 101)
        Me.txtReference.Name = "txtReference"
        Me.txtReference.Size = New System.Drawing.Size(161, 21)
        Me.txtReference.TabIndex = 55
        '
        'fraCommission
        '
        Me.fraCommission.Controls.Add(Me.optGross)
        Me.fraCommission.Controls.Add(Me.optNet)
        Me.fraCommission.Location = New System.Drawing.Point(823, 149)
        Me.fraCommission.Name = "fraCommission"
        Me.fraCommission.Size = New System.Drawing.Size(95, 49)
        Me.fraCommission.TabIndex = 56
        Me.fraCommission.TabStop = False
        Me.fraCommission.Text = "Commission"
        '
        'optGross
        '
        Me.optGross.AutoSize = True
        Me.optGross.Location = New System.Drawing.Point(19, 28)
        Me.optGross.Name = "optGross"
        Me.optGross.Size = New System.Drawing.Size(58, 17)
        Me.optGross.TabIndex = 1
        Me.optGross.TabStop = True
        Me.optGross.Text = "Gross"
        Me.optGross.UseVisualStyleBackColor = True
        '
        'optNet
        '
        Me.optNet.AutoSize = True
        Me.optNet.Location = New System.Drawing.Point(19, 14)
        Me.optNet.Name = "optNet"
        Me.optNet.Size = New System.Drawing.Size(44, 17)
        Me.optNet.TabIndex = 0
        Me.optNet.TabStop = True
        Me.optNet.Text = "Net"
        Me.optNet.UseVisualStyleBackColor = True
        '
        'lblRecieptPaymentAmount
        '
        Me.lblRecieptPaymentAmount.AutoSize = True
        Me.lblRecieptPaymentAmount.Location = New System.Drawing.Point(290, 31)
        Me.lblRecieptPaymentAmount.Name = "lblRecieptPaymentAmount"
        Me.lblRecieptPaymentAmount.Size = New System.Drawing.Size(102, 13)
        Me.lblRecieptPaymentAmount.TabIndex = 57
        Me.lblRecieptPaymentAmount.Text = "Receipt Amount:"
        '
        'txtReciptPaymentAmount
        '
        Me.txtReciptPaymentAmount.Location = New System.Drawing.Point(400, 26)
        Me.txtReciptPaymentAmount.Name = "txtReciptPaymentAmount"
        Me.txtReciptPaymentAmount.Size = New System.Drawing.Size(182, 21)
        Me.txtReciptPaymentAmount.TabIndex = 58
        '
        'lblReciptPaymentCurrency
        '
        Me.lblReciptPaymentCurrency.AutoSize = True
        Me.lblReciptPaymentCurrency.Location = New System.Drawing.Point(290, 52)
        Me.lblReciptPaymentCurrency.Name = "lblReciptPaymentCurrency"
        Me.lblReciptPaymentCurrency.Size = New System.Drawing.Size(111, 13)
        Me.lblReciptPaymentCurrency.TabIndex = 59
        Me.lblReciptPaymentCurrency.Text = "Receipt Currency:"
        '
        'chkDateFilterToinstalmentDueDates
        '
        Me.chkDateFilterToinstalmentDueDates.AutoSize = True
        Me.chkDateFilterToinstalmentDueDates.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDateFilterToinstalmentDueDates.Location = New System.Drawing.Point(290, 156)
        Me.chkDateFilterToinstalmentDueDates.Name = "chkDateFilterToinstalmentDueDates"
        Me.chkDateFilterToinstalmentDueDates.Size = New System.Drawing.Size(261, 17)
        Me.chkDateFilterToinstalmentDueDates.TabIndex = 60
        Me.chkDateFilterToinstalmentDueDates.Text = "Apply Date filter to instalment Due Dates"
        Me.chkDateFilterToinstalmentDueDates.UseVisualStyleBackColor = True
        '
        'lblUnallocatedamount
        '
        Me.lblUnallocatedamount.AutoSize = True
        Me.lblUnallocatedamount.Location = New System.Drawing.Point(601, 28)
        Me.lblUnallocatedamount.Name = "lblUnallocatedamount"
        Me.lblUnallocatedamount.Size = New System.Drawing.Size(126, 13)
        Me.lblUnallocatedamount.TabIndex = 61
        Me.lblUnallocatedamount.Text = "Unallocated Amount:"
        '
        'txtunallocatedAmount
        '
        Me.txtunallocatedAmount.Location = New System.Drawing.Point(739, 25)
        Me.txtunallocatedAmount.Name = "txtunallocatedAmount"
        Me.txtunallocatedAmount.ReadOnly = True
        Me.txtunallocatedAmount.Size = New System.Drawing.Size(147, 21)
        Me.txtunallocatedAmount.TabIndex = 62
        Me.txtunallocatedAmount.Text = "0.00"
        Me.txtunallocatedAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'UctCurrency
        '
        Me.UctCurrency.CompanyId = 0
        Me.UctCurrency.CurrencyId = 0
        Me.UctCurrency.DefaultCurrencyId = 0
        Me.UctCurrency.FirstItem = "(None)"
        Me.UctCurrency.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.UctCurrency.ListIndex = -1
        Me.UctCurrency.Location = New System.Drawing.Point(400, 52)
        Me.UctCurrency.Name = "UctCurrency"
        Me.UctCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actCompanyCurrencies
        Me.UctCurrency.Size = New System.Drawing.Size(182, 21)
        Me.UctCurrency.TabIndex = 63
        Me.UctCurrency.ToolTipText = ""
        Me.UctCurrency.WhatsThisHelpID = 0
        '
        'cboMediaType
        '
        Me.cboMediaType.DefaultItemId = 0
        Me.cboMediaType.FirstItem = "(All)"
        Me.cboMediaType.ItemId = 0
        Me.cboMediaType.ListIndex = -1
        Me.cboMediaType.Location = New System.Drawing.Point(113, 180)
        Me.cboMediaType.Name = "cboMediaType"
        Me.cboMediaType.PMLookupProductFamily = 1
        Me.cboMediaType.SingleItemId = 0
        Me.cboMediaType.Size = New System.Drawing.Size(161, 21)
        Me.cboMediaType.SortColumnName = ""
        Me.cboMediaType.Sorted = True
        Me.cboMediaType.TabIndex = 64
        Me.cboMediaType.TableName = "MediaType"
        Me.cboMediaType.ToolTipText = ""
        Me.cboMediaType.WhereClause = ""
        '
        'PgBarTransactions
        '
        Me.PgBarTransactions.Location = New System.Drawing.Point(310, 530)
        Me.PgBarTransactions.Name = "PgBarTransactions"
        Me.PgBarTransactions.Size = New System.Drawing.Size(313, 21)
        Me.PgBarTransactions.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.PgBarTransactions.TabIndex = 66
        Me.PgBarTransactions.Visible = False
        '
        'optDueDate
        '
        Me.optDueDate.AutoSize = True
        Me.optDueDate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.optDueDate.Location = New System.Drawing.Point(557, 130)
        Me.optDueDate.Name = "optDueDate"
        Me.optDueDate.Size = New System.Drawing.Size(84, 17)
        Me.optDueDate.TabIndex = 67
        Me.optDueDate.TabStop = True
        Me.optDueDate.Text = "Due Date:"
        Me.optDueDate.UseVisualStyleBackColor = True
        '
        'cmdUnMarkAll
        '
        Me.cmdUnMarkAll.BackColor = System.Drawing.SystemColors.Control
        Me.cmdUnMarkAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdUnMarkAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdUnMarkAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdUnMarkAll.Location = New System.Drawing.Point(943, 276)
        Me.cmdUnMarkAll.Name = "cmdUnMarkAll"
        Me.cmdUnMarkAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdUnMarkAll.Size = New System.Drawing.Size(80, 22)
        Me.cmdUnMarkAll.TabIndex = 67
        Me.cmdUnMarkAll.Text = "&UnMark All"
        Me.cmdUnMarkAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdUnMarkAll.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdFindNow
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1028, 598)
        Me.Controls.Add(Me.optDueDate)
        Me.Controls.Add(Me.cmdUnMarkAll)
        Me.Controls.Add(Me.PgBarTransactions)
        Me.Controls.Add(Me.cboMediaType)
        Me.Controls.Add(Me.UctCurrency)
        Me.Controls.Add(Me.txtunallocatedAmount)
        Me.Controls.Add(Me.lblUnallocatedamount)
        Me.Controls.Add(Me.chkDateFilterToinstalmentDueDates)
        Me.Controls.Add(Me.lblReciptPaymentCurrency)
        Me.Controls.Add(Me.txtReciptPaymentAmount)
        Me.Controls.Add(Me.lblRecieptPaymentAmount)
        Me.Controls.Add(Me.txtReference)
        Me.Controls.Add(Me.lblMediaType)
        Me.Controls.Add(Me.fraCommission)
        Me.Controls.Add(Me.uctTransactionCurrency)
        Me.Controls.Add(Me.lblReference)
        Me.Controls.Add(Me.chkAllocationPeriod)
        Me.Controls.Add(Me.lblCurrency)
        Me.Controls.Add(Me.txtDueDateFrom)
        Me.Controls.Add(Me.txtDueDateTo)
        Me.Controls.Add(Me.lblAllocationPeriod)
        Me.Controls.Add(Me.cmdAllocate)
        Me.Controls.Add(Me.lblDueDateTo)
        Me.Controls.Add(Me.cboTransType)
        Me.Controls.Add(Me.txtAlternateRef)
        Me.Controls.Add(Me.lblDueDateFrom)
        Me.Controls.Add(Me.TxttotalWriteoff)
        Me.Controls.Add(Me.cmdwriteoff)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.uctAnchor)
        Me.Controls.Add(Me.divTransDetails)
        Me.Controls.Add(Me.cmdDrill)
        Me.Controls.Add(Me.cmdMarkEntries)
        Me.Controls.Add(Me.cmdMarkAll)
        Me.Controls.Add(Me.cmdMarkTrans)
        Me.Controls.Add(Me.divSearch)
        Me.Controls.Add(Me.txtMarked)
        Me.Controls.Add(Me.cmdPartPay)
        Me.Controls.Add(Me.divTransactions)
        Me.Controls.Add(Me.cboMonth)
        Me.Controls.Add(Me.cboMarkedStatus)
        Me.Controls.Add(Me.lvwTransactions)
        Me.Controls.Add(Me.cboPaymentGroup)
        Me.Controls.Add(Me.cmdFindAccount)
        Me.Controls.Add(Me.lvwEntries)
        Me.Controls.Add(Me.txtAccountCode)
        Me.Controls.Add(Me.cmdBinder)
        Me.Controls.Add(Me.optEffectiveDate)
        Me.Controls.Add(Me.cmdPay)
        Me.Controls.Add(Me.optTransDate)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.stbStatus)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdFindNow)
        Me.Controls.Add(Me.cmdNewSearch)
        Me.Controls.Add(Me.lblTransType)
        Me.Controls.Add(Me.fraViewBy)
        Me.Controls.Add(Me.LblTotalWriteoff)
        Me.Controls.Add(Me.lblAlternateRef)
        Me.Controls.Add(Me.lblMarked)
        Me.Controls.Add(Me.lblAccountCode)
        Me.Controls.Add(Me._lblMarkedStatus_0)
        Me.Controls.Add(Me.lblMonth)
        Me.Controls.Add(Me.MainMenu1)
        Me.Controls.Add(Me.lblPaymentGroup)
        Me.Controls.Add(Me.lvwInstalmentEntries)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(191, 280)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RightToLeftLayout = True
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Insurer/Agent Payment"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.stbStatus.ResumeLayout(False)
        Me.stbStatus.PerformLayout()
        Me.fraViewBy.ResumeLayout(False)
        CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraCommission.ResumeLayout(False)
        Me.fraCommission.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializelblMarkedStatus()
        Me.lblMarkedStatus(0) = _lblMarkedStatus_0
    End Sub
    Friend WithEvents ToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents lblAllocationPeriod As System.Windows.Forms.Label
    Friend WithEvents chkAllocationPeriod As System.Windows.Forms.CheckedListBox
    Public WithEvents lblCurrency As System.Windows.Forms.Label
    Friend WithEvents _lvwEntries_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
    Public WithEvents uctTransactionCurrency As UserControls.CurrencyLookup
    'Friend WithEvents uctCurrency As UserControls.CurrencyLookup
    Friend WithEvents lblDueDateFrom As System.Windows.Forms.Label
    Friend WithEvents lblDueDateTo As System.Windows.Forms.Label
    'Private WithEvents _lvwTransactions_ColumnHeader_15 As System.Windows.Forms.ColumnHeader
    Friend WithEvents txtDueDateTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents txtDueDateFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents _lvwTransactions_ColumnHeader_15 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwTransactions_ColumnHeader_16 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblReference As System.Windows.Forms.Label
    Friend WithEvents lblMediaType As System.Windows.Forms.Label
    Friend WithEvents txtReference As System.Windows.Forms.TextBox
    Friend WithEvents fraCommission As System.Windows.Forms.GroupBox
    Friend WithEvents optGross As System.Windows.Forms.RadioButton
    Friend WithEvents optNet As System.Windows.Forms.RadioButton
    Friend WithEvents lblRecieptPaymentAmount As System.Windows.Forms.Label
    Friend WithEvents txtReciptPaymentAmount As System.Windows.Forms.TextBox
    Friend WithEvents lblReciptPaymentCurrency As System.Windows.Forms.Label
    Friend WithEvents chkDateFilterToinstalmentDueDates As System.Windows.Forms.CheckBox
    Friend WithEvents lblUnallocatedamount As System.Windows.Forms.Label
    Friend WithEvents txtunallocatedAmount As System.Windows.Forms.TextBox
    Public WithEvents UctCurrency As UserControls.CurrencyLookup
    Friend WithEvents cboMediaType As PMLookupControl.cboPMLookup
    Friend WithEvents _lvwTransactions_ColumnHeader_17 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwInstalmentEntries_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwInstalmentEntries_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwInstalmentEntries_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwInstalmentEntries_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwInstalmentEntries_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwInstalmentEntries_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwInstalmentEntries_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwInstalmentEntries_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwInstalmentEntries As System.Windows.Forms.ListView
    Public WithEvents cboPaymentGroup As System.Windows.Forms.ComboBox
    Friend WithEvents tsPgBarTransactions As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents PgBarTransactions As System.Windows.Forms.ProgressBar
    Friend WithEvents optDueDate As System.Windows.Forms.RadioButton
    Public WithEvents cmdUnMarkAll As System.Windows.Forms.Button
#End Region
End Class
