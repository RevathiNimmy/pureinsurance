<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctPMUFees
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
	Friend WithEvents cmdEditFees As System.Windows.Forms.Button
	Friend WithEvents cmdDeleteFees As System.Windows.Forms.Button
	Friend WithEvents cmdAddFees As System.Windows.Forms.Button
	Friend WithEvents lvwFees As System.Windows.Forms.ListView
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdEditFees = New System.Windows.Forms.Button
        Me.cmdDeleteFees = New System.Windows.Forms.Button
        Me.cmdAddFees = New System.Windows.Forms.Button
        Me.lvwFees = New System.Windows.Forms.ListView
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdEditFees
        '
        Me.cmdEditFees.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditFees.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditFees.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditFees.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditFees.Location = New System.Drawing.Point(160, 176)
        Me.cmdEditFees.Name = "cmdEditFees"
        Me.cmdEditFees.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditFees.Size = New System.Drawing.Size(73, 23)
        Me.cmdEditFees.TabIndex = 3
        Me.cmdEditFees.Text = "&Edit"
        Me.cmdEditFees.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditFees.UseVisualStyleBackColor = False
        '
        'cmdDeleteFees
        '
        Me.cmdDeleteFees.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteFees.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteFees.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteFees.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteFees.Location = New System.Drawing.Point(80, 176)
        Me.cmdDeleteFees.Name = "cmdDeleteFees"
        Me.cmdDeleteFees.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteFees.Size = New System.Drawing.Size(73, 23)
        Me.cmdDeleteFees.TabIndex = 2
        Me.cmdDeleteFees.Text = "&Delete"
        Me.cmdDeleteFees.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteFees.UseVisualStyleBackColor = False
        '
        'cmdAddFees
        '
        Me.cmdAddFees.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddFees.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddFees.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddFees.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddFees.Location = New System.Drawing.Point(0, 176)
        Me.cmdAddFees.Name = "cmdAddFees"
        Me.cmdAddFees.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddFees.Size = New System.Drawing.Size(73, 23)
        Me.cmdAddFees.TabIndex = 1
        Me.cmdAddFees.Text = "Re-&Add"
        Me.cmdAddFees.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddFees.UseVisualStyleBackColor = False
        '
        'lvwFees
        '
        Me.lvwFees.BackColor = System.Drawing.SystemColors.Window
        Me.lvwFees.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwFees, "")
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwFees, True)
        Me.lvwFees.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwFees.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwFees.FullRowSelect = True
        Me.lvwFees.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwFees, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwFees, "")
        Me.lvwFees.Location = New System.Drawing.Point(0, 0)
        Me.lvwFees.MultiSelect = False
        Me.lvwFees.Name = "lvwFees"
        Me.lvwFees.Size = New System.Drawing.Size(641, 169)
        Me.listViewHelper1.SetSmallIcons(Me.lvwFees, "")
        Me.listViewHelper1.SetSorted(Me.lvwFees, False)
        Me.listViewHelper1.SetSortKey(Me.lvwFees, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwFees, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwFees.TabIndex = 0
        Me.lvwFees.UseCompatibleStateImageBehavior = False
        Me.lvwFees.View = System.Windows.Forms.View.Details
        '
        'uctPMUFees
        '
        Me.Controls.Add(Me.cmdEditFees)
        Me.Controls.Add(Me.cmdDeleteFees)
        Me.Controls.Add(Me.cmdAddFees)
        Me.Controls.Add(Me.lvwFees)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctPMUFees"
        Me.Size = New System.Drawing.Size(644, 207)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class