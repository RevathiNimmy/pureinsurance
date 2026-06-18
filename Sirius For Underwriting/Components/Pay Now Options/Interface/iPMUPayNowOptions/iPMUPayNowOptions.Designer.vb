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
    Public WithEvents txtTotalDue As System.Windows.Forms.TextBox
    Public WithEvents txtPolicyRef As System.Windows.Forms.TextBox
    Public WithEvents optclient As System.Windows.Forms.RadioButton
    Public WithEvents optAgent As System.Windows.Forms.RadioButton
    Public WithEvents txtAccountBalance As System.Windows.Forms.TextBox
    Public WithEvents optAccount As System.Windows.Forms.RadioButton
    Public WithEvents txtUnallocatedCredit As System.Windows.Forms.TextBox
    Public WithEvents txtFloatBalance As System.Windows.Forms.TextBox
    Public WithEvents txtOverdraft As System.Windows.Forms.TextBox
    Public WithEvents optUnallocatedCredit As System.Windows.Forms.RadioButton
    Public WithEvents optFloatBalance As System.Windows.Forms.RadioButton
    Public WithEvents optOverdraft As System.Windows.Forms.RadioButton
    Public WithEvents fraDebitAgainst As System.Windows.Forms.GroupBox
    Public WithEvents fraAccount As System.Windows.Forms.GroupBox
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Private WithEvents grdCredit As Artinsoft.Windows.Forms.ExtendedDataGridView
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtTotalDue = New System.Windows.Forms.TextBox
        Me.txtPolicyRef = New System.Windows.Forms.TextBox
        Me.fraAccount = New System.Windows.Forms.GroupBox
        Me.optclient = New System.Windows.Forms.RadioButton
        Me.optAgent = New System.Windows.Forms.RadioButton
        Me.fraDebitAgainst = New System.Windows.Forms.GroupBox
        Me.txtAccountBalance = New System.Windows.Forms.TextBox
        Me.optAccount = New System.Windows.Forms.RadioButton
        Me.txtUnallocatedCredit = New System.Windows.Forms.TextBox
        Me.txtFloatBalance = New System.Windows.Forms.TextBox
        Me.txtOverdraft = New System.Windows.Forms.TextBox
        Me.optUnallocatedCredit = New System.Windows.Forms.RadioButton
        Me.optFloatBalance = New System.Windows.Forms.RadioButton
        Me.optOverdraft = New System.Windows.Forms.RadioButton
        Me.grdCredit = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Column1 = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column8 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column9 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.fraAccount.SuspendLayout()
        Me.fraDebitAgainst.SuspendLayout()
        CType(Me.grdCredit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtTotalDue
        '
        Me.txtTotalDue.AcceptsReturn = True
        Me.txtTotalDue.BackColor = System.Drawing.SystemColors.Window
        Me.txtTotalDue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalDue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalDue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalDue.Location = New System.Drawing.Point(456, 16)
        Me.txtTotalDue.MaxLength = 10
        Me.txtTotalDue.Name = "txtTotalDue"
        Me.txtTotalDue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalDue.Size = New System.Drawing.Size(153, 20)
        Me.txtTotalDue.TabIndex = 15
        '
        'txtPolicyRef
        '
        Me.txtPolicyRef.AcceptsReturn = True
        Me.txtPolicyRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolicyRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicyRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicyRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicyRef.Location = New System.Drawing.Point(112, 16)
        Me.txtPolicyRef.MaxLength = 25
        Me.txtPolicyRef.Name = "txtPolicyRef"
        Me.txtPolicyRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicyRef.Size = New System.Drawing.Size(209, 20)
        Me.txtPolicyRef.TabIndex = 14
        '
        'fraAccount
        '
        Me.fraAccount.BackColor = System.Drawing.SystemColors.Control
        Me.fraAccount.Controls.Add(Me.optclient)
        Me.fraAccount.Controls.Add(Me.optAgent)
        Me.fraAccount.Controls.Add(Me.fraDebitAgainst)
        Me.fraAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAccount.Location = New System.Drawing.Point(8, 48)
        Me.fraAccount.Name = "fraAccount"
        Me.fraAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAccount.Size = New System.Drawing.Size(617, 313)
        Me.fraAccount.TabIndex = 2
        Me.fraAccount.TabStop = False
        Me.fraAccount.Text = "Account"
        '
        'optclient
        '
        Me.optclient.BackColor = System.Drawing.SystemColors.Control
        Me.optclient.Cursor = System.Windows.Forms.Cursors.Default
        Me.optclient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optclient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optclient.Location = New System.Drawing.Point(387, 20)
        Me.optclient.Name = "optclient"
        Me.optclient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optclient.Size = New System.Drawing.Size(125, 16)
        Me.optclient.TabIndex = 7
        Me.optclient.TabStop = True
        Me.optclient.Text = "Client"
        Me.optclient.UseVisualStyleBackColor = False
        '
        'optAgent
        '
        Me.optAgent.BackColor = System.Drawing.SystemColors.Control
        Me.optAgent.Checked = True
        Me.optAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.optAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optAgent.Location = New System.Drawing.Point(142, 19)
        Me.optAgent.Name = "optAgent"
        Me.optAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optAgent.Size = New System.Drawing.Size(125, 17)
        Me.optAgent.TabIndex = 6
        Me.optAgent.TabStop = True
        Me.optAgent.Text = "Agent"
        Me.optAgent.UseVisualStyleBackColor = False
        '
        'fraDebitAgainst
        '
        Me.fraDebitAgainst.BackColor = System.Drawing.SystemColors.Control
        Me.fraDebitAgainst.Controls.Add(Me.txtAccountBalance)
        Me.fraDebitAgainst.Controls.Add(Me.optAccount)
        Me.fraDebitAgainst.Controls.Add(Me.txtUnallocatedCredit)
        Me.fraDebitAgainst.Controls.Add(Me.txtFloatBalance)
        Me.fraDebitAgainst.Controls.Add(Me.txtOverdraft)
        Me.fraDebitAgainst.Controls.Add(Me.optUnallocatedCredit)
        Me.fraDebitAgainst.Controls.Add(Me.optFloatBalance)
        Me.fraDebitAgainst.Controls.Add(Me.optOverdraft)
        Me.fraDebitAgainst.Controls.Add(Me.grdCredit)
        Me.fraDebitAgainst.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDebitAgainst.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDebitAgainst.Location = New System.Drawing.Point(8, 36)
        Me.fraDebitAgainst.Name = "fraDebitAgainst"
        Me.fraDebitAgainst.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDebitAgainst.Size = New System.Drawing.Size(601, 273)
        Me.fraDebitAgainst.TabIndex = 5
        Me.fraDebitAgainst.TabStop = False
        Me.fraDebitAgainst.Text = "Debit Against"
        '
        'txtAccountBalance
        '
        Me.txtAccountBalance.AcceptsReturn = True
        Me.txtAccountBalance.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccountBalance.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccountBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountBalance.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccountBalance.Location = New System.Drawing.Point(200, 24)
        Me.txtAccountBalance.MaxLength = 10
        Me.txtAccountBalance.Name = "txtAccountBalance"
        Me.txtAccountBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccountBalance.Size = New System.Drawing.Size(153, 20)
        Me.txtAccountBalance.TabIndex = 17
        '
        'optAccount
        '
        Me.optAccount.BackColor = System.Drawing.SystemColors.Control
        Me.optAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.optAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optAccount.Location = New System.Drawing.Point(360, 24)
        Me.optAccount.Name = "optAccount"
        Me.optAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optAccount.Size = New System.Drawing.Size(89, 21)
        Me.optAccount.TabIndex = 16
        Me.optAccount.TabStop = True
        Me.optAccount.Text = "Account"
        Me.optAccount.UseVisualStyleBackColor = False
        '
        'txtUnallocatedCredit
        '
        Me.txtUnallocatedCredit.AcceptsReturn = True
        Me.txtUnallocatedCredit.BackColor = System.Drawing.SystemColors.Window
        Me.txtUnallocatedCredit.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtUnallocatedCredit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUnallocatedCredit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtUnallocatedCredit.Location = New System.Drawing.Point(200, 80)
        Me.txtUnallocatedCredit.MaxLength = 10
        Me.txtUnallocatedCredit.Name = "txtUnallocatedCredit"
        Me.txtUnallocatedCredit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtUnallocatedCredit.Size = New System.Drawing.Size(153, 20)
        Me.txtUnallocatedCredit.TabIndex = 13
        '
        'txtFloatBalance
        '
        Me.txtFloatBalance.AcceptsReturn = True
        Me.txtFloatBalance.BackColor = System.Drawing.SystemColors.Window
        Me.txtFloatBalance.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFloatBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFloatBalance.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFloatBalance.Location = New System.Drawing.Point(200, 52)
        Me.txtFloatBalance.MaxLength = 10
        Me.txtFloatBalance.Name = "txtFloatBalance"
        Me.txtFloatBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFloatBalance.Size = New System.Drawing.Size(153, 20)
        Me.txtFloatBalance.TabIndex = 12
        '
        'txtOverdraft
        '
        Me.txtOverdraft.AcceptsReturn = True
        Me.txtOverdraft.BackColor = System.Drawing.SystemColors.Window
        Me.txtOverdraft.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOverdraft.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOverdraft.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOverdraft.Location = New System.Drawing.Point(200, 24)
        Me.txtOverdraft.MaxLength = 10
        Me.txtOverdraft.Name = "txtOverdraft"
        Me.txtOverdraft.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOverdraft.Size = New System.Drawing.Size(153, 20)
        Me.txtOverdraft.TabIndex = 11
        '
        'optUnallocatedCredit
        '
        Me.optUnallocatedCredit.BackColor = System.Drawing.SystemColors.Control
        Me.optUnallocatedCredit.Cursor = System.Windows.Forms.Cursors.Default
        Me.optUnallocatedCredit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optUnallocatedCredit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optUnallocatedCredit.Location = New System.Drawing.Point(24, 80)
        Me.optUnallocatedCredit.Name = "optUnallocatedCredit"
        Me.optUnallocatedCredit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optUnallocatedCredit.Size = New System.Drawing.Size(145, 21)
        Me.optUnallocatedCredit.TabIndex = 10
        Me.optUnallocatedCredit.TabStop = True
        Me.optUnallocatedCredit.Text = "Unallocated Credit"
        Me.optUnallocatedCredit.UseVisualStyleBackColor = False
        '
        'optFloatBalance
        '
        Me.optFloatBalance.BackColor = System.Drawing.SystemColors.Control
        Me.optFloatBalance.Cursor = System.Windows.Forms.Cursors.Default
        Me.optFloatBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optFloatBalance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optFloatBalance.Location = New System.Drawing.Point(24, 52)
        Me.optFloatBalance.Name = "optFloatBalance"
        Me.optFloatBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optFloatBalance.Size = New System.Drawing.Size(145, 21)
        Me.optFloatBalance.TabIndex = 9
        Me.optFloatBalance.TabStop = True
        Me.optFloatBalance.Text = "Float Balance"
        Me.optFloatBalance.UseVisualStyleBackColor = False
        '
        'optOverdraft
        '
        Me.optOverdraft.BackColor = System.Drawing.SystemColors.Control
        Me.optOverdraft.Cursor = System.Windows.Forms.Cursors.Default
        Me.optOverdraft.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optOverdraft.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optOverdraft.Location = New System.Drawing.Point(24, 24)
        Me.optOverdraft.Name = "optOverdraft"
        Me.optOverdraft.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optOverdraft.Size = New System.Drawing.Size(145, 21)
        Me.optOverdraft.TabIndex = 8
        Me.optOverdraft.TabStop = True
        Me.optOverdraft.Text = "Overdraft"
        Me.optOverdraft.UseVisualStyleBackColor = False
        '
        'grdCredit
        '
        Me.grdCredit.AllowBigSelection = False
        Me.grdCredit.AllowRowSelection = False
        Me.grdCredit.AllowUserToAddRows = False
        Me.grdCredit.AlternatingRows = False
        Me.grdCredit.BackColorFixed = System.Drawing.Color.Empty
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.grdCredit.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.grdCredit.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4, Me.Column5, Me.Column6, Me.Column7, Me.Column8, Me.Column9})
        Me.grdCredit.ColumnsCount = 9
        Me.grdCredit.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me.grdCredit.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.grdCredit.EvenStyle = DataGridViewCellStyle3
        Me.grdCredit.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me.grdCredit.FixedColumns = -1
        Me.grdCredit.FixedRows = -1
        Me.grdCredit.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me.grdCredit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grdCredit.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me.grdCredit.GridLineWidth = 0
        Me.grdCredit.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me.grdCredit.Location = New System.Drawing.Point(8, 116)
        Me.grdCredit.Name = "grdCredit"
        Me.grdCredit.OddStyle = DataGridViewCellStyle4
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.grdCredit.RowHeadersDefaultCellStyle = DataGridViewCellStyle5
        Me.grdCredit.RowHeightMin = 0
        Me.grdCredit.RowsCount = 0
        Me.grdCredit.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me.grdCredit.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.grdCredit.SelectedStyle = Nothing
        Me.grdCredit.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.grdCredit.SelLength = -1
        Me.grdCredit.SelStart = -1
        Me.grdCredit.Size = New System.Drawing.Size(585, 149)
        Me.grdCredit.TabIndex = 18
        Me.grdCredit.ToolTipText = ""
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(528, 368)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(80, 22)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(440, 368)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(80, 22)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(376, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(57, 17)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Total Due"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(16, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(73, 17)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Policy Ref"
        '
        'Column1
        '
        Me.Column1.HeaderText = "Column1"
        Me.Column1.Name = "Column1"
        Me.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'Column2
        '
        Me.Column2.HeaderText = "Column2"
        Me.Column2.Name = "Column2"
        Me.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Column3
        '
        Me.Column3.HeaderText = "Column3"
        Me.Column3.Name = "Column3"
        Me.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Column4
        '
        Me.Column4.HeaderText = "Column4"
        Me.Column4.Name = "Column4"
        Me.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Column5
        '
        Me.Column5.HeaderText = "Column5"
        Me.Column5.Name = "Column5"
        Me.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Column6
        '
        Me.Column6.HeaderText = "Column6"
        Me.Column6.Name = "Column6"
        Me.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Column7
        '
        Me.Column7.HeaderText = "Column7"
        Me.Column7.Name = "Column7"
        Me.Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Column8
        '
        Me.Column8.HeaderText = "Column8"
        Me.Column8.Name = "Column8"
        Me.Column8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Column9
        '
        DataGridViewCellStyle2.Format = "D"
        DataGridViewCellStyle2.NullValue = Nothing
        Me.Column9.DefaultCellStyle = DataGridViewCellStyle2
        Me.Column9.HeaderText = "Column9"
        Me.Column9.Name = "Column9"
        Me.Column9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(633, 395)
        Me.Controls.Add(Me.txtTotalDue)
        Me.Controls.Add(Me.txtPolicyRef)
        Me.Controls.Add(Me.fraAccount)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Make Live - Account Information"
        Me.fraAccount.ResumeLayout(False)
        Me.fraDebitAgainst.ResumeLayout(False)
        Me.fraDebitAgainst.PerformLayout()
        CType(Me.grdCredit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column8 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column9 As System.Windows.Forms.DataGridViewTextBoxColumn
#End Region
End Class