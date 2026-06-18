<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctLinkedAccounts
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		UserControl_Initialize()
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
	Friend WithEvents uctAnchor As uSIRCommonControls.uctAnchor
	Friend WithEvents _lvwCashDeposit_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwCashDeposit_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwCashDeposit_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwCashDeposit_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwCashDeposit_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lvwCashDeposit As System.Windows.Forms.ListView
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.uctAnchor = New uSIRCommonControls.uctAnchor
        Me.lvwCashDeposit = New System.Windows.Forms.ListView
        Me._lvwCashDeposit_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwCashDeposit_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwCashDeposit_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwCashDeposit_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwCashDeposit_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'uctAnchor
        '
        Me.uctAnchor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctAnchor.Location = New System.Drawing.Point(190, 285)
        Me.uctAnchor.Name = "uctAnchor"
        Me.uctAnchor.Size = New System.Drawing.Size(44, 16)
        Me.uctAnchor.TabIndex = 0
        '
        'lvwCashDeposit
        '
        Me.lvwCashDeposit.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwCashDeposit, "")
        Me.lvwCashDeposit.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwCashDeposit_ColumnHeader_1, Me._lvwCashDeposit_ColumnHeader_2, Me._lvwCashDeposit_ColumnHeader_3, Me._lvwCashDeposit_ColumnHeader_4, Me._lvwCashDeposit_ColumnHeader_5})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwCashDeposit, False)
        Me.lvwCashDeposit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwCashDeposit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me.lvwCashDeposit, "")
        Me.lvwCashDeposit.LabelEdit = True
        Me.listViewHelper1.SetLargeIcons(Me.lvwCashDeposit, "")
        Me.lvwCashDeposit.Location = New System.Drawing.Point(5, 5)
        Me.lvwCashDeposit.Name = "lvwCashDeposit"
        Me.lvwCashDeposit.Size = New System.Drawing.Size(706, 341)
        Me.listViewHelper1.SetSmallIcons(Me.lvwCashDeposit, "")
        Me.listViewHelper1.SetSorted(Me.lvwCashDeposit, False)
        Me.listViewHelper1.SetSortKey(Me.lvwCashDeposit, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwCashDeposit, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwCashDeposit.TabIndex = 0
        Me.lvwCashDeposit.UseCompatibleStateImageBehavior = False
        Me.lvwCashDeposit.View = System.Windows.Forms.View.Details
        '
        '_lvwCashDeposit_ColumnHeader_1
        '
        Me._lvwCashDeposit_ColumnHeader_1.Text = "Cash Deposit Account Number"
        Me._lvwCashDeposit_ColumnHeader_1.Width = 97
        '
        '_lvwCashDeposit_ColumnHeader_2
        '
        Me._lvwCashDeposit_ColumnHeader_2.Text = "Branch"
        Me._lvwCashDeposit_ColumnHeader_2.Width = 97
        '
        '_lvwCashDeposit_ColumnHeader_3
        '
        Me._lvwCashDeposit_ColumnHeader_3.Text = "Product"
        Me._lvwCashDeposit_ColumnHeader_3.Width = 97
        '
        '_lvwCashDeposit_ColumnHeader_4
        '
        Me._lvwCashDeposit_ColumnHeader_4.Text = "Date Created"
        Me._lvwCashDeposit_ColumnHeader_4.Width = 97
        '
        '_lvwCashDeposit_ColumnHeader_5
        '
        Me._lvwCashDeposit_ColumnHeader_5.Text = "User Name"
        Me._lvwCashDeposit_ColumnHeader_5.Width = 97
        '
        'uctLinkedAccounts
        '
        Me.Controls.Add(Me.lvwCashDeposit)
        Me.Controls.Add(Me.uctAnchor)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctLinkedAccounts"
        Me.Size = New System.Drawing.Size(720, 352)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class