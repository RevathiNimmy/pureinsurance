<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctPMURITax
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		UserControl_InitProperties()
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
	Friend WithEvents cmdEdit As System.Windows.Forms.Button
	Friend WithEvents lvwRITax As System.Windows.Forms.ListView
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.lvwRITax = New System.Windows.Forms.ListView
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(0, 200)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 23)
        Me.cmdEdit.TabIndex = 0
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'lvwRITax
        '
        Me.lvwRITax.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwRITax, "")
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwRITax, True)
        Me.lvwRITax.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwRITax.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwRITax.FullRowSelect = True
        Me.lvwRITax.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwRITax, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwRITax, "")
        Me.lvwRITax.Location = New System.Drawing.Point(0, 0)
        Me.lvwRITax.MultiSelect = False
        Me.lvwRITax.Name = "lvwRITax"
        Me.lvwRITax.Size = New System.Drawing.Size(641, 193)
        Me.listViewHelper1.SetSmallIcons(Me.lvwRITax, "")
        Me.listViewHelper1.SetSorted(Me.lvwRITax, False)
        Me.listViewHelper1.SetSortKey(Me.lvwRITax, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwRITax, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwRITax.TabIndex = 1
        Me.lvwRITax.UseCompatibleStateImageBehavior = False
        Me.lvwRITax.View = System.Windows.Forms.View.Details
        '
        'uctPMURITax
        '
        Me.Controls.Add(Me.cmdEdit)
        Me.Controls.Add(Me.lvwRITax)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctPMURITax"
        Me.Size = New System.Drawing.Size(645, 245)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class