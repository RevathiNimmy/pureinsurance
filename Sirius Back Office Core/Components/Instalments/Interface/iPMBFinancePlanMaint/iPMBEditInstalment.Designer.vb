<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInstalment
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
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
    Public WithEvents cboPMLookuppfinstalmentsresult As PMLookupControl.cboPMLookup
    Public WithEvents cboPMLookuppfinstalmentsStatus As PMLookupControl.cboPMLookup
    Public WithEvents txtStatus As System.Windows.Forms.TextBox
    Public WithEvents txtTransactionCode As System.Windows.Forms.TextBox
    Public WithEvents txtReason As System.Windows.Forms.TextBox
    Public WithEvents lblStatus As System.Windows.Forms.Label
    Public WithEvents lblTransactionCode As System.Windows.Forms.Label
    Public WithEvents lblReason As System.Windows.Forms.Label
    Public WithEvents fraStatus As System.Windows.Forms.GroupBox
    Public WithEvents txtPaidDate As System.Windows.Forms.TextBox
    Public WithEvents txtPostedDate As System.Windows.Forms.TextBox
    Public WithEvents lblDueDate As System.Windows.Forms.Label
    Public WithEvents lblPaidDate As System.Windows.Forms.Label
    Public WithEvents lblPostedDate As System.Windows.Forms.Label
    Public WithEvents fraDates As System.Windows.Forms.GroupBox
    Public WithEvents txtBatchNo As System.Windows.Forms.TextBox
    Public WithEvents txtInstalmentNo As System.Windows.Forms.TextBox
    Public WithEvents txtAmount As System.Windows.Forms.TextBox
    Public WithEvents txtFee As System.Windows.Forms.TextBox
    Public WithEvents lblFee As System.Windows.Forms.Label
    Public WithEvents lblBatchNo As System.Windows.Forms.Label
    Public WithEvents lblInstalmentNo As System.Windows.Forms.Label
    Public WithEvents lblAmount As System.Windows.Forms.Label
    Public WithEvents fraInstalment As System.Windows.Forms.GroupBox
    Public WithEvents cmdNavigate As System.Windows.Forms.Button
    Public WithEvents lvwInstalmentEvents As System.Windows.Forms.ListView
    Public WithEvents fraInstalmentEvents As System.Windows.Forms.GroupBox
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInstalment))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.fraStatus = New System.Windows.Forms.GroupBox
        Me.cboWriteOffReasonID = New PMLookupControl.cboPMLookup
        Me.lblWriteOffReasonID = New System.Windows.Forms.Label
        Me.cboPMLookuppfinstalmentsresult = New PMLookupControl.cboPMLookup
        Me.cboPMLookuppfinstalmentsStatus = New PMLookupControl.cboPMLookup
        Me.txtStatus = New System.Windows.Forms.TextBox
        Me.txtTransactionCode = New System.Windows.Forms.TextBox
        Me.txtReason = New System.Windows.Forms.TextBox
        Me.lblStatus = New System.Windows.Forms.Label
        Me.lblTransactionCode = New System.Windows.Forms.Label
        Me.lblReason = New System.Windows.Forms.Label
        Me.fraDates = New System.Windows.Forms.GroupBox
        Me.CboDueDate = New System.Windows.Forms.ComboBox
        Me.txtPaidDate = New System.Windows.Forms.TextBox
        Me.txtPostedDate = New System.Windows.Forms.TextBox
        Me.lblDueDate = New System.Windows.Forms.Label
        Me.lblPaidDate = New System.Windows.Forms.Label
        Me.lblPostedDate = New System.Windows.Forms.Label
        Me.fraInstalment = New System.Windows.Forms.GroupBox
        Me.txtBatchNo = New System.Windows.Forms.TextBox
        Me.txtInstalmentNo = New System.Windows.Forms.TextBox
        Me.txtAmount = New System.Windows.Forms.TextBox
        Me.txtFee = New System.Windows.Forms.TextBox
        Me.lblFee = New System.Windows.Forms.Label
        Me.lblBatchNo = New System.Windows.Forms.Label
        Me.lblInstalmentNo = New System.Windows.Forms.Label
        Me.lblAmount = New System.Windows.Forms.Label
        Me.cmdNavigate = New System.Windows.Forms.Button
        Me.fraInstalmentEvents = New System.Windows.Forms.GroupBox
        Me.lvwInstalmentEvents = New System.Windows.Forms.ListView
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.fraStatus.SuspendLayout()
        Me.fraDates.SuspendLayout()
        Me.fraInstalment.SuspendLayout()
        Me.fraInstalmentEvents.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'fraStatus
        '
        Me.fraStatus.BackColor = System.Drawing.SystemColors.Control
        Me.fraStatus.Controls.Add(Me.cboWriteOffReasonID)
        Me.fraStatus.Controls.Add(Me.lblWriteOffReasonID)
        Me.fraStatus.Controls.Add(Me.cboPMLookuppfinstalmentsresult)
        Me.fraStatus.Controls.Add(Me.cboPMLookuppfinstalmentsStatus)
        Me.fraStatus.Controls.Add(Me.txtStatus)
        Me.fraStatus.Controls.Add(Me.txtTransactionCode)
        Me.fraStatus.Controls.Add(Me.txtReason)
        Me.fraStatus.Controls.Add(Me.lblStatus)
        Me.fraStatus.Controls.Add(Me.lblTransactionCode)
        Me.fraStatus.Controls.Add(Me.lblReason)
        Me.fraStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraStatus.Location = New System.Drawing.Point(8, 240)
        Me.fraStatus.Name = "fraStatus"
        Me.fraStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraStatus.Size = New System.Drawing.Size(305, 121)
        Me.fraStatus.TabIndex = 17
        Me.fraStatus.TabStop = False
        Me.fraStatus.Text = "Status"
        '
        'cboWriteOffReasonID
        '
        Me.cboWriteOffReasonID.DefaultItemId = 0
        Me.cboWriteOffReasonID.FirstItem = ""
        Me.cboWriteOffReasonID.ItemId = 0
        Me.cboWriteOffReasonID.ListIndex = -1
        Me.cboWriteOffReasonID.Location = New System.Drawing.Point(124, 90)
        Me.cboWriteOffReasonID.Name = "cboWriteOffReasonID"
        Me.cboWriteOffReasonID.PMLookupProductFamily = 1
        Me.cboWriteOffReasonID.SingleItemId = 0
        Me.cboWriteOffReasonID.Size = New System.Drawing.Size(169, 21)
        Me.cboWriteOffReasonID.Sorted = True
        Me.cboWriteOffReasonID.TabIndex = 26
        Me.cboWriteOffReasonID.TableName = "Write_Off_Reason"
        Me.cboWriteOffReasonID.ToolTipText = ""
        Me.cboWriteOffReasonID.WhereClause = ""
        '
        'lblWriteOffReasonID
        '
        Me.lblWriteOffReasonID.BackColor = System.Drawing.SystemColors.Control
        Me.lblWriteOffReasonID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblWriteOffReasonID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWriteOffReasonID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblWriteOffReasonID.Location = New System.Drawing.Point(7, 92)
        Me.lblWriteOffReasonID.Name = "lblWriteOffReasonID"
        Me.lblWriteOffReasonID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblWriteOffReasonID.Size = New System.Drawing.Size(125, 17)
        Me.lblWriteOffReasonID.TabIndex = 27
        Me.lblWriteOffReasonID.Text = "Write-Off Reason:"
        '
        'cboPMLookuppfinstalmentsresult
        '
        Me.cboPMLookuppfinstalmentsresult.DefaultItemId = 0
        Me.cboPMLookuppfinstalmentsresult.FirstItem = ""
        Me.cboPMLookuppfinstalmentsresult.ItemId = 0
        Me.cboPMLookuppfinstalmentsresult.ListIndex = -1
        Me.cboPMLookuppfinstalmentsresult.Location = New System.Drawing.Point(124, 63)
        Me.cboPMLookuppfinstalmentsresult.Name = "cboPMLookuppfinstalmentsresult"
        Me.cboPMLookuppfinstalmentsresult.PMLookupProductFamily = 1
        Me.cboPMLookuppfinstalmentsresult.SingleItemId = 0
        Me.cboPMLookuppfinstalmentsresult.Size = New System.Drawing.Size(169, 21)
        Me.cboPMLookuppfinstalmentsresult.Sorted = True
        Me.cboPMLookuppfinstalmentsresult.TabIndex = 25
        Me.cboPMLookuppfinstalmentsresult.TableName = "pfInstalments_result"
        Me.cboPMLookuppfinstalmentsresult.ToolTipText = ""
        Me.cboPMLookuppfinstalmentsresult.WhereClause = ""
        '
        'cboPMLookuppfinstalmentsStatus
        '
        Me.cboPMLookuppfinstalmentsStatus.DefaultItemId = 0
        Me.cboPMLookuppfinstalmentsStatus.FirstItem = ""
        Me.cboPMLookuppfinstalmentsStatus.ItemId = 0
        Me.cboPMLookuppfinstalmentsStatus.ListIndex = -1
        Me.cboPMLookuppfinstalmentsStatus.Location = New System.Drawing.Point(124, 40)
        Me.cboPMLookuppfinstalmentsStatus.Name = "cboPMLookuppfinstalmentsStatus"
        Me.cboPMLookuppfinstalmentsStatus.PMLookupProductFamily = 1
        Me.cboPMLookuppfinstalmentsStatus.SingleItemId = 0
        Me.cboPMLookuppfinstalmentsStatus.Size = New System.Drawing.Size(169, 21)
        Me.cboPMLookuppfinstalmentsStatus.Sorted = True
        Me.cboPMLookuppfinstalmentsStatus.TabIndex = 24
        Me.cboPMLookuppfinstalmentsStatus.TableName = "pfInstalments_status"
        Me.cboPMLookuppfinstalmentsStatus.ToolTipText = ""
        Me.cboPMLookuppfinstalmentsStatus.WhereClause = ""
        '
        'txtStatus
        '
        Me.txtStatus.AcceptsReturn = True
        Me.txtStatus.BackColor = System.Drawing.SystemColors.Window
        Me.txtStatus.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStatus.Enabled = False
        Me.txtStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStatus.Location = New System.Drawing.Point(124, 40)
        Me.txtStatus.MaxLength = 0
        Me.txtStatus.Name = "txtStatus"
        Me.txtStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStatus.Size = New System.Drawing.Size(160, 20)
        Me.txtStatus.TabIndex = 20
        Me.txtStatus.TabStop = False
        '
        'txtTransactionCode
        '
        Me.txtTransactionCode.AcceptsReturn = True
        Me.txtTransactionCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtTransactionCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTransactionCode.Enabled = False
        Me.txtTransactionCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTransactionCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTransactionCode.Location = New System.Drawing.Point(124, 16)
        Me.txtTransactionCode.MaxLength = 0
        Me.txtTransactionCode.Name = "txtTransactionCode"
        Me.txtTransactionCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTransactionCode.Size = New System.Drawing.Size(169, 20)
        Me.txtTransactionCode.TabIndex = 19
        Me.txtTransactionCode.TabStop = False
        '
        'txtReason
        '
        Me.txtReason.AcceptsReturn = True
        Me.txtReason.BackColor = System.Drawing.SystemColors.Window
        Me.txtReason.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtReason.Enabled = False
        Me.txtReason.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReason.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtReason.Location = New System.Drawing.Point(124, 64)
        Me.txtReason.MaxLength = 0
        Me.txtReason.Name = "txtReason"
        Me.txtReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtReason.Size = New System.Drawing.Size(160, 20)
        Me.txtReason.TabIndex = 18
        Me.txtReason.TabStop = False
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.BackColor = System.Drawing.Color.Transparent
        Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatus.Location = New System.Drawing.Point(8, 43)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatus.Size = New System.Drawing.Size(40, 13)
        Me.lblStatus.TabIndex = 23
        Me.lblStatus.Text = "Status:"
        '
        'lblTransactionCode
        '
        Me.lblTransactionCode.AutoSize = True
        Me.lblTransactionCode.BackColor = System.Drawing.Color.Transparent
        Me.lblTransactionCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTransactionCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTransactionCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTransactionCode.Location = New System.Drawing.Point(8, 19)
        Me.lblTransactionCode.Name = "lblTransactionCode"
        Me.lblTransactionCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTransactionCode.Size = New System.Drawing.Size(94, 13)
        Me.lblTransactionCode.TabIndex = 22
        Me.lblTransactionCode.Text = "Transaction Code:"
        '
        'lblReason
        '
        Me.lblReason.AutoSize = True
        Me.lblReason.BackColor = System.Drawing.Color.Transparent
        Me.lblReason.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReason.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReason.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReason.Location = New System.Drawing.Point(8, 67)
        Me.lblReason.Name = "lblReason"
        Me.lblReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReason.Size = New System.Drawing.Size(47, 13)
        Me.lblReason.TabIndex = 21
        Me.lblReason.Text = "Reason:"
        '
        'fraDates
        '
        Me.fraDates.BackColor = System.Drawing.SystemColors.Control
        Me.fraDates.Controls.Add(Me.CboDueDate)
        Me.fraDates.Controls.Add(Me.txtPaidDate)
        Me.fraDates.Controls.Add(Me.txtPostedDate)
        Me.fraDates.Controls.Add(Me.lblDueDate)
        Me.fraDates.Controls.Add(Me.lblPaidDate)
        Me.fraDates.Controls.Add(Me.lblPostedDate)
        Me.fraDates.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDates.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDates.Location = New System.Drawing.Point(8, 136)
        Me.fraDates.Name = "fraDates"
        Me.fraDates.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDates.Size = New System.Drawing.Size(305, 97)
        Me.fraDates.TabIndex = 10
        Me.fraDates.TabStop = False
        Me.fraDates.Text = "Dates"
        '
        'CboDueDate
        '
        Me.CboDueDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboDueDate.FormattingEnabled = True
        Me.CboDueDate.Location = New System.Drawing.Point(124, 16)
        Me.CboDueDate.Name = "CboDueDate"
        Me.CboDueDate.Size = New System.Drawing.Size(106, 21)
        Me.CboDueDate.TabIndex = 17
        '
        'txtPaidDate
        '
        Me.txtPaidDate.AcceptsReturn = True
        Me.txtPaidDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtPaidDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPaidDate.Enabled = False
        Me.txtPaidDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaidDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPaidDate.Location = New System.Drawing.Point(124, 40)
        Me.txtPaidDate.MaxLength = 0
        Me.txtPaidDate.Name = "txtPaidDate"
        Me.txtPaidDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPaidDate.Size = New System.Drawing.Size(106, 20)
        Me.txtPaidDate.TabIndex = 12
        Me.txtPaidDate.TabStop = False
        Me.txtPaidDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtPostedDate
        '
        Me.txtPostedDate.AcceptsReturn = True
        Me.txtPostedDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtPostedDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPostedDate.Enabled = False
        Me.txtPostedDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPostedDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPostedDate.Location = New System.Drawing.Point(124, 64)
        Me.txtPostedDate.MaxLength = 0
        Me.txtPostedDate.Name = "txtPostedDate"
        Me.txtPostedDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPostedDate.Size = New System.Drawing.Size(106, 20)
        Me.txtPostedDate.TabIndex = 11
        Me.txtPostedDate.TabStop = False
        Me.txtPostedDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblDueDate
        '
        Me.lblDueDate.AutoSize = True
        Me.lblDueDate.BackColor = System.Drawing.Color.Transparent
        Me.lblDueDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDueDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDueDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDueDate.Location = New System.Drawing.Point(8, 19)
        Me.lblDueDate.Name = "lblDueDate"
        Me.lblDueDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDueDate.Size = New System.Drawing.Size(56, 13)
        Me.lblDueDate.TabIndex = 16
        Me.lblDueDate.Text = "Due Date:"
        '
        'lblPaidDate
        '
        Me.lblPaidDate.AutoSize = True
        Me.lblPaidDate.BackColor = System.Drawing.Color.Transparent
        Me.lblPaidDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaidDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaidDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaidDate.Location = New System.Drawing.Point(8, 43)
        Me.lblPaidDate.Name = "lblPaidDate"
        Me.lblPaidDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaidDate.Size = New System.Drawing.Size(57, 13)
        Me.lblPaidDate.TabIndex = 15
        Me.lblPaidDate.Text = "Paid Date:"
        '
        'lblPostedDate
        '
        Me.lblPostedDate.AutoSize = True
        Me.lblPostedDate.BackColor = System.Drawing.Color.Transparent
        Me.lblPostedDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPostedDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPostedDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPostedDate.Location = New System.Drawing.Point(8, 67)
        Me.lblPostedDate.Name = "lblPostedDate"
        Me.lblPostedDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPostedDate.Size = New System.Drawing.Size(69, 13)
        Me.lblPostedDate.TabIndex = 14
        Me.lblPostedDate.Text = "Posted Date:"
        '
        'fraInstalment
        '
        Me.fraInstalment.BackColor = System.Drawing.SystemColors.Control
        Me.fraInstalment.Controls.Add(Me.txtBatchNo)
        Me.fraInstalment.Controls.Add(Me.txtInstalmentNo)
        Me.fraInstalment.Controls.Add(Me.txtAmount)
        Me.fraInstalment.Controls.Add(Me.txtFee)
        Me.fraInstalment.Controls.Add(Me.lblFee)
        Me.fraInstalment.Controls.Add(Me.lblBatchNo)
        Me.fraInstalment.Controls.Add(Me.lblInstalmentNo)
        Me.fraInstalment.Controls.Add(Me.lblAmount)
        Me.fraInstalment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraInstalment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraInstalment.Location = New System.Drawing.Point(8, 8)
        Me.fraInstalment.Name = "fraInstalment"
        Me.fraInstalment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraInstalment.Size = New System.Drawing.Size(305, 121)
        Me.fraInstalment.TabIndex = 1
        Me.fraInstalment.TabStop = False
        Me.fraInstalment.Text = "Instalment"
        '
        'txtBatchNo
        '
        Me.txtBatchNo.AcceptsReturn = True
        Me.txtBatchNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtBatchNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBatchNo.Enabled = False
        Me.txtBatchNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBatchNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBatchNo.Location = New System.Drawing.Point(124, 40)
        Me.txtBatchNo.MaxLength = 0
        Me.txtBatchNo.Name = "txtBatchNo"
        Me.txtBatchNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBatchNo.Size = New System.Drawing.Size(60, 20)
        Me.txtBatchNo.TabIndex = 5
        Me.txtBatchNo.TabStop = False
        Me.txtBatchNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtInstalmentNo
        '
        Me.txtInstalmentNo.AcceptsReturn = True
        Me.txtInstalmentNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtInstalmentNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInstalmentNo.Enabled = False
        Me.txtInstalmentNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInstalmentNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInstalmentNo.Location = New System.Drawing.Point(124, 16)
        Me.txtInstalmentNo.MaxLength = 0
        Me.txtInstalmentNo.Name = "txtInstalmentNo"
        Me.txtInstalmentNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInstalmentNo.Size = New System.Drawing.Size(60, 20)
        Me.txtInstalmentNo.TabIndex = 4
        Me.txtInstalmentNo.TabStop = False
        Me.txtInstalmentNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAmount
        '
        Me.txtAmount.AcceptsReturn = True
        Me.txtAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAmount.Enabled = False
        Me.txtAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAmount.Location = New System.Drawing.Point(124, 64)
        Me.txtAmount.MaxLength = 0
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAmount.Size = New System.Drawing.Size(80, 20)
        Me.txtAmount.TabIndex = 3
        Me.txtAmount.TabStop = False
        Me.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtFee
        '
        Me.txtFee.AcceptsReturn = True
        Me.txtFee.BackColor = System.Drawing.SystemColors.Window
        Me.txtFee.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFee.Enabled = False
        Me.txtFee.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFee.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFee.Location = New System.Drawing.Point(124, 88)
        Me.txtFee.MaxLength = 0
        Me.txtFee.Name = "txtFee"
        Me.txtFee.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFee.Size = New System.Drawing.Size(80, 20)
        Me.txtFee.TabIndex = 2
        Me.txtFee.TabStop = False
        Me.txtFee.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblFee
        '
        Me.lblFee.AutoSize = True
        Me.lblFee.BackColor = System.Drawing.Color.Transparent
        Me.lblFee.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFee.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFee.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFee.Location = New System.Drawing.Point(8, 88)
        Me.lblFee.Name = "lblFee"
        Me.lblFee.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFee.Size = New System.Drawing.Size(28, 13)
        Me.lblFee.TabIndex = 9
        Me.lblFee.Text = "Fee:"
        '
        'lblBatchNo
        '
        Me.lblBatchNo.AutoSize = True
        Me.lblBatchNo.BackColor = System.Drawing.Color.Transparent
        Me.lblBatchNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBatchNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBatchNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBatchNo.Location = New System.Drawing.Point(8, 43)
        Me.lblBatchNo.Name = "lblBatchNo"
        Me.lblBatchNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBatchNo.Size = New System.Drawing.Size(55, 13)
        Me.lblBatchNo.TabIndex = 8
        Me.lblBatchNo.Text = "Batch No:"
        '
        'lblInstalmentNo
        '
        Me.lblInstalmentNo.AutoSize = True
        Me.lblInstalmentNo.BackColor = System.Drawing.Color.Transparent
        Me.lblInstalmentNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInstalmentNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstalmentNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInstalmentNo.Location = New System.Drawing.Point(8, 19)
        Me.lblInstalmentNo.Name = "lblInstalmentNo"
        Me.lblInstalmentNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInstalmentNo.Size = New System.Drawing.Size(75, 13)
        Me.lblInstalmentNo.TabIndex = 7
        Me.lblInstalmentNo.Text = "Instalment No:"
        '
        'lblAmount
        '
        Me.lblAmount.AutoSize = True
        Me.lblAmount.BackColor = System.Drawing.Color.Transparent
        Me.lblAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAmount.Location = New System.Drawing.Point(8, 67)
        Me.lblAmount.Name = "lblAmount"
        Me.lblAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAmount.Size = New System.Drawing.Size(46, 13)
        Me.lblAmount.TabIndex = 6
        Me.lblAmount.Text = "Amount:"
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(744, 367)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(65, 22)
        Me.cmdNavigate.TabIndex = 0
        Me.cmdNavigate.TabStop = False
        Me.cmdNavigate.Text = "&OK"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        '
        'fraInstalmentEvents
        '
        Me.fraInstalmentEvents.BackColor = System.Drawing.SystemColors.Control
        Me.fraInstalmentEvents.Controls.Add(Me.lvwInstalmentEvents)
        Me.fraInstalmentEvents.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraInstalmentEvents.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraInstalmentEvents.Location = New System.Drawing.Point(320, 8)
        Me.fraInstalmentEvents.Name = "fraInstalmentEvents"
        Me.fraInstalmentEvents.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraInstalmentEvents.Size = New System.Drawing.Size(489, 353)
        Me.fraInstalmentEvents.TabIndex = 26
        Me.fraInstalmentEvents.TabStop = False
        Me.fraInstalmentEvents.Text = "Instalment Events"
        '
        'lvwInstalmentEvents
        '
        Me.lvwInstalmentEvents.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwInstalmentEvents, "")
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwInstalmentEvents, False)
        Me.lvwInstalmentEvents.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwInstalmentEvents.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me.lvwInstalmentEvents, "")
        Me.lvwInstalmentEvents.LabelEdit = True
        Me.listViewHelper1.SetLargeIcons(Me.lvwInstalmentEvents, "")
        Me.lvwInstalmentEvents.Location = New System.Drawing.Point(8, 16)
        Me.lvwInstalmentEvents.Name = "lvwInstalmentEvents"
        Me.lvwInstalmentEvents.Size = New System.Drawing.Size(473, 331)
        Me.listViewHelper1.SetSmallIcons(Me.lvwInstalmentEvents, "")
        Me.listViewHelper1.SetSorted(Me.lvwInstalmentEvents, False)
        Me.listViewHelper1.SetSortKey(Me.lvwInstalmentEvents, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwInstalmentEvents, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwInstalmentEvents.TabIndex = 27
        Me.lvwInstalmentEvents.UseCompatibleStateImageBehavior = False
        Me.lvwInstalmentEvents.View = System.Windows.Forms.View.Details
        '
        'frmInstalment
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(819, 391)
        Me.Controls.Add(Me.fraStatus)
        Me.Controls.Add(Me.fraDates)
        Me.Controls.Add(Me.fraInstalment)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.fraInstalmentEvents)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(2, 21)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInstalment"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.Text = "Edit Instalment"
        Me.fraStatus.ResumeLayout(False)
        Me.fraStatus.PerformLayout()
        Me.fraDates.ResumeLayout(False)
        Me.fraDates.PerformLayout()
        Me.fraInstalment.ResumeLayout(False)
        Me.fraInstalment.PerformLayout()
        Me.fraInstalmentEvents.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents cboWriteOffReasonID As PMLookupControl.cboPMLookup
    Public WithEvents lblWriteOffReasonID As System.Windows.Forms.Label
    Friend WithEvents CboDueDate As System.Windows.Forms.ComboBox
#End Region
End Class