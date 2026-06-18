<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmRIModelLine
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
    Public WithEvents chkObligatory As System.Windows.Forms.CheckBox
    Public WithEvents cboRIType As PMLookupControl.cboPMLookup
    Public WithEvents cboPremiumCalculationBasis As PMLookupControl.cboPMLookup
    Public WithEvents txtcedingrate As System.Windows.Forms.TextBox
    Public WithEvents txtlowerlimit As System.Windows.Forms.TextBox
    Public WithEvents cboTreatyType As PMLookupControl.cboPMLookup
    Public WithEvents txtTreatyLimit As System.Windows.Forms.TextBox
    Public WithEvents txtAllocatedPercent As System.Windows.Forms.TextBox
    Public WithEvents txtPriority As System.Windows.Forms.TextBox
    Public WithEvents cboTreaty As PMLookupControl.cboPMLookup
    Public WithEvents txtLineLimit As System.Windows.Forms.TextBox
    Public WithEvents divAllocation As uSIRCommonControls.uctDivider
    Public WithEvents txtLines As System.Windows.Forms.TextBox
    Public WithEvents txtSharePercent As System.Windows.Forms.TextBox
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents divRILine As uSIRCommonControls.uctDivider
    Public WithEvents lblReinsuranceType As System.Windows.Forms.Label
    Public WithEvents lblPremiumCalculationBasis As System.Windows.Forms.Label
    Public WithEvents lblcedingrate As System.Windows.Forms.Label
    Public WithEvents lbllowerlimit As System.Windows.Forms.Label
    Public WithEvents lblTreatyType As System.Windows.Forms.Label
    Public WithEvents lblTreatyLimit As System.Windows.Forms.Label
    Public WithEvents lblAllocatedPercent As System.Windows.Forms.Label
    Public WithEvents lblPriority As System.Windows.Forms.Label
    Public WithEvents lblTreaty As System.Windows.Forms.Label
    Public WithEvents lblLineLimit As System.Windows.Forms.Label
    Public WithEvents lblLines As System.Windows.Forms.Label
    Public WithEvents lblSharePercent As System.Windows.Forms.Label
    Public WithEvents chkVariableQuotaShare As System.Windows.Forms.CheckBox
    Public WithEvents tabControl As System.Windows.Forms.TabControl
    Public WithEvents tabTreatyLine As System.Windows.Forms.TabPage
    Private tabVariableQuotaShare As System.Windows.Forms.TabPage
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkObligatory = New System.Windows.Forms.CheckBox()
        Me.cboRIType = New PMLookupControl.cboPMLookup()
        Me.cboPremiumCalculationBasis = New PMLookupControl.cboPMLookup()
        Me.txtcedingrate = New System.Windows.Forms.TextBox()
        Me.txtlowerlimit = New System.Windows.Forms.TextBox()
        Me.cboTreatyType = New PMLookupControl.cboPMLookup()
        Me.txtTreatyLimit = New System.Windows.Forms.TextBox()
        Me.txtAllocatedPercent = New System.Windows.Forms.TextBox()
        Me.txtPriority = New System.Windows.Forms.TextBox()
        Me.cboTreaty = New PMLookupControl.cboPMLookup()
        Me.txtLineLimit = New System.Windows.Forms.TextBox()
        Me.divAllocation = New uSIRCommonControls.uctDivider()
        Me.txtLines = New System.Windows.Forms.TextBox()
        Me.txtSharePercent = New System.Windows.Forms.TextBox()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.divRILine = New uSIRCommonControls.uctDivider()
        Me.lblReinsuranceType = New System.Windows.Forms.Label()
        Me.lblPremiumCalculationBasis = New System.Windows.Forms.Label()
        Me.lblcedingrate = New System.Windows.Forms.Label()
        Me.lbllowerlimit = New System.Windows.Forms.Label()
        Me.lblTreatyType = New System.Windows.Forms.Label()
        Me.lblTreatyLimit = New System.Windows.Forms.Label()
        Me.lblAllocatedPercent = New System.Windows.Forms.Label()
        Me.lblPriority = New System.Windows.Forms.Label()
        Me.lblTreaty = New System.Windows.Forms.Label()
        Me.lblLineLimit = New System.Windows.Forms.Label()
        Me.lblLines = New System.Windows.Forms.Label()
        Me.lblSharePercent = New System.Windows.Forms.Label()
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.chkCedePremiumOnly = New System.Windows.Forms.CheckBox()
        Me.chkVariableQuotaShare = New System.Windows.Forms.CheckBox()
        Me.tabControl = New System.Windows.Forms.TabControl()
        Me.tabTreatyLine = New System.Windows.Forms.TabPage()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabControl.SuspendLayout()
        Me.tabTreatyLine.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkObligatory
        '
        Me.chkObligatory.BackColor = System.Drawing.SystemColors.Control
        Me.chkObligatory.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkObligatory.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkObligatory.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkObligatory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkObligatory.Location = New System.Drawing.Point(430, 41)
        Me.chkObligatory.Name = "chkObligatory"
        Me.chkObligatory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkObligatory.Size = New System.Drawing.Size(200, 17)
        Me.chkObligatory.TabIndex = 26
        Me.chkObligatory.Text = "Is Obligatory ?"
        Me.chkObligatory.UseVisualStyleBackColor = False
        '
        'cboRIType
        '
        Me.cboRIType.DefaultItemId = 0
        Me.cboRIType.FirstItem = ""
        Me.cboRIType.ItemId = 0
        Me.cboRIType.ListIndex = -1
        Me.cboRIType.Location = New System.Drawing.Point(157, 41)
        Me.cboRIType.Name = "cboRIType"
        Me.cboRIType.PMLookupProductFamily = 1
        Me.cboRIType.SingleItemId = 0
        Me.cboRIType.Size = New System.Drawing.Size(230, 21)
        Me.cboRIType.SortColumnName = ""
        Me.cboRIType.Sorted = True
        Me.cboRIType.TabIndex = 25
        Me.cboRIType.TableName = ""
        Me.cboRIType.ToolTipText = ""
        Me.cboRIType.WhereClause = ""
        '
        'cboPremiumCalculationBasis
        '
        Me.cboPremiumCalculationBasis.DefaultItemId = 0
        Me.cboPremiumCalculationBasis.FirstItem = ""
        Me.cboPremiumCalculationBasis.ItemId = 0
        Me.cboPremiumCalculationBasis.ListIndex = -1
        Me.cboPremiumCalculationBasis.Location = New System.Drawing.Point(200, 256)
        Me.cboPremiumCalculationBasis.Name = "cboPremiumCalculationBasis"
        Me.cboPremiumCalculationBasis.PMLookupProductFamily = 1
        Me.cboPremiumCalculationBasis.SingleItemId = 0
        Me.cboPremiumCalculationBasis.Size = New System.Drawing.Size(430, 21)
        Me.cboPremiumCalculationBasis.SortColumnName = ""
        Me.cboPremiumCalculationBasis.Sorted = True
        Me.cboPremiumCalculationBasis.TabIndex = 30
        Me.cboPremiumCalculationBasis.TableName = ""
        Me.cboPremiumCalculationBasis.ToolTipText = ""
        Me.cboPremiumCalculationBasis.WhereClause = ""
        '
        'txtcedingrate
        '
        Me.txtcedingrate.AcceptsReturn = True
        Me.txtcedingrate.BackColor = System.Drawing.SystemColors.Window
        Me.txtcedingrate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtcedingrate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtcedingrate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtcedingrate.Location = New System.Drawing.Point(422, 231)
        Me.txtcedingrate.MaxLength = 10
        Me.txtcedingrate.Name = "txtcedingrate"
        Me.txtcedingrate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtcedingrate.Size = New System.Drawing.Size(178, 20)
        Me.txtcedingrate.TabIndex = 14
        Me.txtcedingrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtcedingrate.Visible = False
        '
        'txtlowerlimit
        '
        Me.txtlowerlimit.AcceptsReturn = True
        Me.txtlowerlimit.BackColor = System.Drawing.SystemColors.Window
        Me.txtlowerlimit.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtlowerlimit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtlowerlimit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtlowerlimit.Location = New System.Drawing.Point(422, 99)
        Me.txtlowerlimit.MaxLength = 20
        Me.txtlowerlimit.Name = "txtlowerlimit"
        Me.txtlowerlimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtlowerlimit.Size = New System.Drawing.Size(208, 20)
        Me.txtlowerlimit.TabIndex = 3
        Me.txtlowerlimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtlowerlimit.Visible = False
        '
        'cboTreatyType
        '
        Me.cboTreatyType.DefaultItemId = 0
        Me.cboTreatyType.FirstItem = ""
        Me.cboTreatyType.ItemId = 0
        Me.cboTreatyType.ListIndex = -1
        Me.cboTreatyType.Location = New System.Drawing.Point(157, 16)
        Me.cboTreatyType.Name = "cboTreatyType"
        Me.cboTreatyType.PMLookupProductFamily = 1
        Me.cboTreatyType.SingleItemId = 0
        Me.cboTreatyType.Size = New System.Drawing.Size(230, 21)
        Me.cboTreatyType.SortColumnName = ""
        Me.cboTreatyType.Sorted = True
        Me.cboTreatyType.TabIndex = 1
        Me.cboTreatyType.TableName = "None"
        Me.cboTreatyType.ToolTipText = ""
        Me.cboTreatyType.WhereClause = ""
        '
        'txtTreatyLimit
        '
        Me.txtTreatyLimit.AcceptsReturn = True
        Me.txtTreatyLimit.BackColor = System.Drawing.SystemColors.Control
        Me.txtTreatyLimit.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTreatyLimit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTreatyLimit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTreatyLimit.Location = New System.Drawing.Point(422, 202)
        Me.txtTreatyLimit.MaxLength = 20
        Me.txtTreatyLimit.Name = "txtTreatyLimit"
        Me.txtTreatyLimit.ReadOnly = True
        Me.txtTreatyLimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTreatyLimit.Size = New System.Drawing.Size(208, 20)
        Me.txtTreatyLimit.TabIndex = 16
        Me.txtTreatyLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAllocatedPercent
        '
        Me.txtAllocatedPercent.AcceptsReturn = True
        Me.txtAllocatedPercent.BackColor = System.Drawing.SystemColors.Control
        Me.txtAllocatedPercent.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAllocatedPercent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAllocatedPercent.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAllocatedPercent.Location = New System.Drawing.Point(157, 231)
        Me.txtAllocatedPercent.MaxLength = 20
        Me.txtAllocatedPercent.Name = "txtAllocatedPercent"
        Me.txtAllocatedPercent.ReadOnly = True
        Me.txtAllocatedPercent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAllocatedPercent.Size = New System.Drawing.Size(133, 20)
        Me.txtAllocatedPercent.TabIndex = 18
        Me.txtAllocatedPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtPriority
        '
        Me.txtPriority.AcceptsReturn = True
        Me.txtPriority.BackColor = System.Drawing.SystemColors.Window
        Me.txtPriority.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPriority.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPriority.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPriority.Location = New System.Drawing.Point(157, 99)
        Me.txtPriority.MaxLength = 5
        Me.txtPriority.Name = "txtPriority"
        Me.txtPriority.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPriority.Size = New System.Drawing.Size(132, 20)
        Me.txtPriority.TabIndex = 2
        Me.txtPriority.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cboTreaty
        '
        Me.cboTreaty.DefaultItemId = 0
        Me.cboTreaty.FirstItem = ""
        Me.cboTreaty.ItemId = 0
        Me.cboTreaty.ListIndex = -1
        Me.cboTreaty.Location = New System.Drawing.Point(157, 173)
        Me.cboTreaty.Name = "cboTreaty"
        Me.cboTreaty.PMLookupProductFamily = 1
        Me.cboTreaty.SingleItemId = 0
        Me.cboTreaty.Size = New System.Drawing.Size(473, 21)
        Me.cboTreaty.SortColumnName = ""
        Me.cboTreaty.Sorted = True
        Me.cboTreaty.TabIndex = 11
        Me.cboTreaty.TableName = "treaty"
        Me.cboTreaty.ToolTipText = ""
        Me.cboTreaty.WhereClause = "1=1 OR effective_date IS NOT NULL"
        '
        'txtLineLimit
        '
        Me.txtLineLimit.AcceptsReturn = True
        Me.txtLineLimit.BackColor = System.Drawing.SystemColors.Window
        Me.txtLineLimit.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLineLimit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLineLimit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLineLimit.Location = New System.Drawing.Point(422, 124)
        Me.txtLineLimit.MaxLength = 20
        Me.txtLineLimit.Name = "txtLineLimit"
        Me.txtLineLimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLineLimit.Size = New System.Drawing.Size(208, 20)
        Me.txtLineLimit.TabIndex = 8
        Me.txtLineLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'divAllocation
        '
        Me.divAllocation.Caption = "Limits and Allocation"
        Me.divAllocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.divAllocation.Location = New System.Drawing.Point(15, 148)
        Me.divAllocation.Name = "divAllocation"
        Me.divAllocation.Size = New System.Drawing.Size(615, 21)
        Me.divAllocation.TabIndex = 9
        Me.divAllocation.TabStop = False
        '
        'txtLines
        '
        Me.txtLines.AcceptsReturn = True
        Me.txtLines.BackColor = System.Drawing.SystemColors.Window
        Me.txtLines.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLines.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLines.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLines.Location = New System.Drawing.Point(157, 124)
        Me.txtLines.MaxLength = 8
        Me.txtLines.Name = "txtLines"
        Me.txtLines.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLines.Size = New System.Drawing.Size(132, 20)
        Me.txtLines.TabIndex = 4
        Me.txtLines.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtSharePercent
        '
        Me.txtSharePercent.AcceptsReturn = True
        Me.txtSharePercent.BackColor = System.Drawing.SystemColors.Window
        Me.txtSharePercent.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSharePercent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSharePercent.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSharePercent.Location = New System.Drawing.Point(157, 202)
        Me.txtSharePercent.MaxLength = 10
        Me.txtSharePercent.Name = "txtSharePercent"
        Me.txtSharePercent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSharePercent.Size = New System.Drawing.Size(133, 20)
        Me.txtSharePercent.TabIndex = 13
        Me.txtSharePercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(500, 284)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(130, 24)
        Me.cmdCancel.TabIndex = 20
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
        Me.cmdOK.Location = New System.Drawing.Point(360, 284)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(130, 24)
        Me.cmdOK.TabIndex = 19
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'divRILine
        '
        Me.divRILine.Caption = "Limits"
        Me.divRILine.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.divRILine.Location = New System.Drawing.Point(15, 66)
        Me.divRILine.Name = "divRILine"
        Me.divRILine.Size = New System.Drawing.Size(615, 21)
        Me.divRILine.TabIndex = 0
        Me.divRILine.TabStop = False
        '
        'lblReinsuranceType
        '
        Me.lblReinsuranceType.AutoSize = True
        Me.lblReinsuranceType.BackColor = System.Drawing.SystemColors.Control
        Me.lblReinsuranceType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReinsuranceType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReinsuranceType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReinsuranceType.Location = New System.Drawing.Point(20, 44)
        Me.lblReinsuranceType.Name = "lblReinsuranceType"
        Me.lblReinsuranceType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReinsuranceType.Size = New System.Drawing.Size(94, 13)
        Me.lblReinsuranceType.TabIndex = 24
        Me.lblReinsuranceType.Text = "Reinsurance Type"
        '
        'lblPremiumCalculationBasis
        '
        Me.lblPremiumCalculationBasis.AutoSize = True
        Me.lblPremiumCalculationBasis.BackColor = System.Drawing.SystemColors.Control
        Me.lblPremiumCalculationBasis.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPremiumCalculationBasis.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPremiumCalculationBasis.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPremiumCalculationBasis.Location = New System.Drawing.Point(20, 259)
        Me.lblPremiumCalculationBasis.Name = "lblPremiumCalculationBasis"
        Me.lblPremiumCalculationBasis.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPremiumCalculationBasis.Size = New System.Drawing.Size(130, 13)
        Me.lblPremiumCalculationBasis.TabIndex = 31
        Me.lblPremiumCalculationBasis.Text = "Premium Calculation Basis:"
        '
        'lblcedingrate
        '
        Me.lblcedingrate.AutoSize = True
        Me.lblcedingrate.BackColor = System.Drawing.SystemColors.Control
        Me.lblcedingrate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblcedingrate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblcedingrate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblcedingrate.Location = New System.Drawing.Point(313, 233)
        Me.lblcedingrate.Name = "lblcedingrate"
        Me.lblcedingrate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblcedingrate.Size = New System.Drawing.Size(80, 13)
        Me.lblcedingrate.TabIndex = 23
        Me.lblcedingrate.Text = "Ceding Rate %:"
        Me.lblcedingrate.Visible = False
        '
        'lbllowerlimit
        '
        Me.lbllowerlimit.AutoSize = True
        Me.lbllowerlimit.BackColor = System.Drawing.SystemColors.Control
        Me.lbllowerlimit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbllowerlimit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbllowerlimit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbllowerlimit.Location = New System.Drawing.Point(313, 101)
        Me.lbllowerlimit.Name = "lbllowerlimit"
        Me.lbllowerlimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbllowerlimit.Size = New System.Drawing.Size(63, 13)
        Me.lbllowerlimit.TabIndex = 22
        Me.lbllowerlimit.Text = "L&ower Limit:"
        Me.lbllowerlimit.Visible = False
        '
        'lblTreatyType
        '
        Me.lblTreatyType.AutoSize = True
        Me.lblTreatyType.BackColor = System.Drawing.SystemColors.Control
        Me.lblTreatyType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTreatyType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTreatyType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTreatyType.Location = New System.Drawing.Point(20, 19)
        Me.lblTreatyType.Name = "lblTreatyType"
        Me.lblTreatyType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTreatyType.Size = New System.Drawing.Size(64, 13)
        Me.lblTreatyType.TabIndex = 21
        Me.lblTreatyType.Text = "Treaty Type"
        '
        'lblTreatyLimit
        '
        Me.lblTreatyLimit.AutoSize = True
        Me.lblTreatyLimit.BackColor = System.Drawing.SystemColors.Control
        Me.lblTreatyLimit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTreatyLimit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTreatyLimit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTreatyLimit.Location = New System.Drawing.Point(313, 204)
        Me.lblTreatyLimit.Name = "lblTreatyLimit"
        Me.lblTreatyLimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTreatyLimit.Size = New System.Drawing.Size(64, 13)
        Me.lblTreatyLimit.TabIndex = 15
        Me.lblTreatyLimit.Text = "Treaty Limit:"
        '
        'lblAllocatedPercent
        '
        Me.lblAllocatedPercent.AutoSize = True
        Me.lblAllocatedPercent.BackColor = System.Drawing.SystemColors.Control
        Me.lblAllocatedPercent.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAllocatedPercent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAllocatedPercent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAllocatedPercent.Location = New System.Drawing.Point(20, 233)
        Me.lblAllocatedPercent.Name = "lblAllocatedPercent"
        Me.lblAllocatedPercent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAllocatedPercent.Size = New System.Drawing.Size(65, 13)
        Me.lblAllocatedPercent.TabIndex = 17
        Me.lblAllocatedPercent.Text = "Allocated %:"
        '
        'lblPriority
        '
        Me.lblPriority.AutoSize = True
        Me.lblPriority.BackColor = System.Drawing.SystemColors.Control
        Me.lblPriority.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPriority.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPriority.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPriority.Location = New System.Drawing.Point(20, 101)
        Me.lblPriority.Name = "lblPriority"
        Me.lblPriority.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPriority.Size = New System.Drawing.Size(41, 13)
        Me.lblPriority.TabIndex = 5
        Me.lblPriority.Text = "&Priority:"
        '
        'lblTreaty
        '
        Me.lblTreaty.AutoSize = True
        Me.lblTreaty.BackColor = System.Drawing.SystemColors.Control
        Me.lblTreaty.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTreaty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTreaty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTreaty.Location = New System.Drawing.Point(20, 175)
        Me.lblTreaty.Name = "lblTreaty"
        Me.lblTreaty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTreaty.Size = New System.Drawing.Size(40, 13)
        Me.lblTreaty.TabIndex = 10
        Me.lblTreaty.Text = "&Treaty:"
        '
        'lblLineLimit
        '
        Me.lblLineLimit.AutoSize = True
        Me.lblLineLimit.BackColor = System.Drawing.SystemColors.Control
        Me.lblLineLimit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLineLimit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLineLimit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLineLimit.Location = New System.Drawing.Point(313, 126)
        Me.lblLineLimit.Name = "lblLineLimit"
        Me.lblLineLimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLineLimit.Size = New System.Drawing.Size(54, 13)
        Me.lblLineLimit.TabIndex = 7
        Me.lblLineLimit.Text = "&Line Limit:"
        '
        'lblLines
        '
        Me.lblLines.AutoSize = True
        Me.lblLines.BackColor = System.Drawing.SystemColors.Control
        Me.lblLines.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLines.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLines.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLines.Location = New System.Drawing.Point(20, 126)
        Me.lblLines.Name = "lblLines"
        Me.lblLines.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLines.Size = New System.Drawing.Size(67, 13)
        Me.lblLines.TabIndex = 6
        Me.lblLines.Text = "&No. of Lines:"
        '
        'lblSharePercent
        '
        Me.lblSharePercent.AutoSize = True
        Me.lblSharePercent.BackColor = System.Drawing.SystemColors.Control
        Me.lblSharePercent.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSharePercent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSharePercent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSharePercent.Location = New System.Drawing.Point(20, 204)
        Me.lblSharePercent.Name = "lblSharePercent"
        Me.lblSharePercent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSharePercent.Size = New System.Drawing.Size(49, 13)
        Me.lblSharePercent.TabIndex = 12
        Me.lblSharePercent.Text = "&Share %:"
        '
        'chkCedePremiumOnly
        '
        Me.chkCedePremiumOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkCedePremiumOnly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkCedePremiumOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCedePremiumOnly.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCedePremiumOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCedePremiumOnly.Location = New System.Drawing.Point(430, 16)
        Me.chkCedePremiumOnly.Name = "chkCedePremiumOnly"
        Me.chkCedePremiumOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCedePremiumOnly.Size = New System.Drawing.Size(200, 17)
        Me.chkCedePremiumOnly.TabIndex = 27
        Me.chkCedePremiumOnly.Text = "Cede Premium Only"
        Me.chkCedePremiumOnly.UseVisualStyleBackColor = False
        '
        'chkVariableQuotaShare
        '
        Me.chkVariableQuotaShare.BackColor = System.Drawing.SystemColors.Control
        Me.chkVariableQuotaShare.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkVariableQuotaShare.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkVariableQuotaShare.Enabled = False
        Me.chkVariableQuotaShare.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkVariableQuotaShare.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkVariableQuotaShare.Location = New System.Drawing.Point(430, 17)
        Me.chkVariableQuotaShare.Name = "chkVariableQuotaShare"
        Me.chkVariableQuotaShare.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkVariableQuotaShare.Size = New System.Drawing.Size(200, 17)
        Me.chkVariableQuotaShare.TabIndex = 28
        Me.chkVariableQuotaShare.Text = "Is Variable Quota Share"
        Me.chkVariableQuotaShare.UseVisualStyleBackColor = False
        '
        'tabControl
        '
        Me.tabControl.Controls.Add(Me.tabTreatyLine)
        Me.tabControl.Location = New System.Drawing.Point(0, 0)
        Me.tabControl.Name = "tabControl"
        Me.tabControl.SelectedIndex = 0
        Me.tabControl.Size = New System.Drawing.Size(650, 345)
        Me.tabControl.TabIndex = 29
        '
        'tabTreatyLine
        '
        Me.tabTreatyLine.BackColor = System.Drawing.SystemColors.Control
        Me.tabTreatyLine.Controls.Add(Me.chkCedePremiumOnly)
        Me.tabTreatyLine.Controls.Add(Me.chkVariableQuotaShare)
        Me.tabTreatyLine.Controls.Add(Me.chkObligatory)
        Me.tabTreatyLine.Controls.Add(Me.cboRIType)
        Me.tabTreatyLine.Controls.Add(Me.cboPremiumCalculationBasis)
        Me.tabTreatyLine.Controls.Add(Me.txtcedingrate)
        Me.tabTreatyLine.Controls.Add(Me.txtlowerlimit)
        Me.tabTreatyLine.Controls.Add(Me.cboTreatyType)
        Me.tabTreatyLine.Controls.Add(Me.txtTreatyLimit)
        Me.tabTreatyLine.Controls.Add(Me.txtAllocatedPercent)
        Me.tabTreatyLine.Controls.Add(Me.txtPriority)
        Me.tabTreatyLine.Controls.Add(Me.cboTreaty)
        Me.tabTreatyLine.Controls.Add(Me.txtLineLimit)
        Me.tabTreatyLine.Controls.Add(Me.divAllocation)
        Me.tabTreatyLine.Controls.Add(Me.txtLines)
        Me.tabTreatyLine.Controls.Add(Me.txtSharePercent)
        Me.tabTreatyLine.Controls.Add(Me.cmdCancel)
        Me.tabTreatyLine.Controls.Add(Me.cmdOK)
        Me.tabTreatyLine.Controls.Add(Me.divRILine)
        Me.tabTreatyLine.Controls.Add(Me.lblReinsuranceType)
        Me.tabTreatyLine.Controls.Add(Me.lblPremiumCalculationBasis)
        Me.tabTreatyLine.Controls.Add(Me.lblcedingrate)
        Me.tabTreatyLine.Controls.Add(Me.lbllowerlimit)
        Me.tabTreatyLine.Controls.Add(Me.lblTreatyType)
        Me.tabTreatyLine.Controls.Add(Me.lblTreatyLimit)
        Me.tabTreatyLine.Controls.Add(Me.lblAllocatedPercent)
        Me.tabTreatyLine.Controls.Add(Me.lblPriority)
        Me.tabTreatyLine.Controls.Add(Me.lblTreaty)
        Me.tabTreatyLine.Controls.Add(Me.lblLineLimit)
        Me.tabTreatyLine.Controls.Add(Me.lblLines)
        Me.tabTreatyLine.Controls.Add(Me.lblSharePercent)
        Me.tabTreatyLine.Location = New System.Drawing.Point(4, 22)
        Me.tabTreatyLine.Name = "tabTreatyLine"
        Me.tabTreatyLine.Padding = New System.Windows.Forms.Padding(3)
        Me.tabTreatyLine.Size = New System.Drawing.Size(642, 319)
        Me.tabTreatyLine.TabIndex = 0
        Me.tabTreatyLine.Text = "Treaty Line"
        '
        'frmRIModelLine
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(649, 340)
        Me.Controls.Add(Me.tabControl)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmRIModelLine"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Treaty Line"
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabControl.ResumeLayout(False)
        Me.tabTreatyLine.ResumeLayout(False)
        Me.tabTreatyLine.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents chkCedePremiumOnly As System.Windows.Forms.CheckBox
#End Region
End Class