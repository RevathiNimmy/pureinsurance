<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CashDeposit
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Friend WithEvents cmdCashDepositEdit As System.Windows.Forms.Button
	Friend WithEvents cmdCashDepositAdd As System.Windows.Forms.Button
	Friend WithEvents lvwCashDeposit As System.Windows.Forms.ListView
	Friend WithEvents pnlCashDeposit As System.Windows.Forms.GroupBox
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.pnlCashDeposit = New System.Windows.Forms.GroupBox
        Me.cmdCashDepositEdit = New System.Windows.Forms.Button
        Me.cmdCashDepositAdd = New System.Windows.Forms.Button
        Me.lvwCashDeposit = New System.Windows.Forms.ListView
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.pnlCashDeposit.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlCashDeposit
        '
        Me.pnlCashDeposit.BackColor = System.Drawing.SystemColors.Control
        Me.pnlCashDeposit.Controls.Add(Me.cmdCashDepositEdit)
        Me.pnlCashDeposit.Controls.Add(Me.cmdCashDepositAdd)
        Me.pnlCashDeposit.Controls.Add(Me.lvwCashDeposit)
        Me.pnlCashDeposit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlCashDeposit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlCashDeposit.Location = New System.Drawing.Point(4, 4)
        Me.pnlCashDeposit.Name = "pnlCashDeposit"
        Me.pnlCashDeposit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlCashDeposit.Size = New System.Drawing.Size(627, 414)
        Me.pnlCashDeposit.TabIndex = 0
        Me.pnlCashDeposit.TabStop = False
        '
        'cmdCashDepositEdit
        '
        Me.cmdCashDepositEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCashDepositEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCashDepositEdit.Enabled = False
        Me.cmdCashDepositEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCashDepositEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCashDepositEdit.Location = New System.Drawing.Point(122, 368)
        Me.cmdCashDepositEdit.Name = "cmdCashDepositEdit"
        Me.cmdCashDepositEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCashDepositEdit.Size = New System.Drawing.Size(85, 25)
        Me.cmdCashDepositEdit.TabIndex = 3
        Me.cmdCashDepositEdit.Text = "Edit"
        Me.cmdCashDepositEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCashDepositEdit.UseVisualStyleBackColor = False
        '
        'cmdCashDepositAdd
        '
        Me.cmdCashDepositAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCashDepositAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCashDepositAdd.Enabled = False
        Me.cmdCashDepositAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCashDepositAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCashDepositAdd.Location = New System.Drawing.Point(18, 366)
        Me.cmdCashDepositAdd.Name = "cmdCashDepositAdd"
        Me.cmdCashDepositAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCashDepositAdd.Size = New System.Drawing.Size(85, 25)
        Me.cmdCashDepositAdd.TabIndex = 2
        Me.cmdCashDepositAdd.Text = "Add"
        Me.cmdCashDepositAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCashDepositAdd.UseVisualStyleBackColor = False
        '
        'lvwCashDeposit
        '
        Me.lvwCashDeposit.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwCashDeposit, "")
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwCashDeposit, True)
        Me.lvwCashDeposit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwCashDeposit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwCashDeposit.FullRowSelect = True
        Me.lvwCashDeposit.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwCashDeposit, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwCashDeposit, "")
        Me.lvwCashDeposit.Location = New System.Drawing.Point(6, 18)
        Me.lvwCashDeposit.MultiSelect = False
        Me.lvwCashDeposit.Name = "lvwCashDeposit"
        Me.lvwCashDeposit.Size = New System.Drawing.Size(607, 334)
        Me.listViewHelper1.SetSmallIcons(Me.lvwCashDeposit, "")
        Me.listViewHelper1.SetSorted(Me.lvwCashDeposit, False)
        Me.listViewHelper1.SetSortKey(Me.lvwCashDeposit, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwCashDeposit, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwCashDeposit.TabIndex = 1
        Me.lvwCashDeposit.UseCompatibleStateImageBehavior = False
        Me.lvwCashDeposit.View = System.Windows.Forms.View.Details
        '
        'CashDeposit
        '
        Me.Controls.Add(Me.pnlCashDeposit)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "CashDeposit"
        Me.Size = New System.Drawing.Size(640, 427)
        Me.pnlCashDeposit.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class