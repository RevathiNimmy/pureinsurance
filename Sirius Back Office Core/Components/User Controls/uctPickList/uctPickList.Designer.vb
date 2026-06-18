<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PickList
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
	Friend WithEvents _lvwAll_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lvwAll As System.Windows.Forms.ListView
	Friend WithEvents cmdFind As System.Windows.Forms.Button
	Friend WithEvents txtFindText As System.Windows.Forms.TextBox
	Friend WithEvents cmdRemoveAll As System.Windows.Forms.Button
	Friend WithEvents cmdRemoveOne As System.Windows.Forms.Button
	Friend WithEvents cmdAddAll As System.Windows.Forms.Button
	Friend WithEvents cmdAddOne As System.Windows.Forms.Button
	Friend WithEvents _lvwContents_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvwContents As System.Windows.Forms.ListView
    'commented the code as conflicting with icon display in istview
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.lvwAll = New System.Windows.Forms.ListView
        Me._lvwAll_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me.cmdFind = New System.Windows.Forms.Button
        Me.txtFindText = New System.Windows.Forms.TextBox
        Me.cmdRemoveAll = New System.Windows.Forms.Button
        Me.cmdRemoveOne = New System.Windows.Forms.Button
        Me.cmdAddAll = New System.Windows.Forms.Button
        Me.cmdAddOne = New System.Windows.Forms.Button
        Me.lvwContents = New System.Windows.Forms.ListView
        Me._lvwContents_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me.SuspendLayout()
        '
        'lvwAll
        '
        Me.lvwAll.BackColor = System.Drawing.SystemColors.Window
        Me.lvwAll.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwAll_ColumnHeader_1})
        Me.lvwAll.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwAll.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwAll.FullRowSelect = True
        Me.lvwAll.Location = New System.Drawing.Point(8, 8)
        Me.lvwAll.Name = "lvwAll"
        Me.lvwAll.Size = New System.Drawing.Size(177, 279)
        Me.lvwAll.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwAll.TabIndex = 0
        Me.lvwAll.UseCompatibleStateImageBehavior = False
        Me.lvwAll.View = System.Windows.Forms.View.Details
        '
        '_lvwAll_ColumnHeader_1
        '
        Me._lvwAll_ColumnHeader_1.Width = 97
        '
        'cmdFind
        '
        Me.cmdFind.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFind.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFind.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFind.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFind.Location = New System.Drawing.Point(134, 8)
        Me.cmdFind.Name = "cmdFind"
        Me.cmdFind.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFind.Size = New System.Drawing.Size(51, 21)
        Me.cmdFind.TabIndex = 7
        Me.cmdFind.Text = "&Find"
        Me.cmdFind.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFind.UseVisualStyleBackColor = False
        '
        'txtFindText
        '
        Me.txtFindText.AcceptsReturn = True
        Me.txtFindText.BackColor = System.Drawing.SystemColors.Window
        Me.txtFindText.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFindText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFindText.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFindText.Location = New System.Drawing.Point(10, 8)
        Me.txtFindText.MaxLength = 0
        Me.txtFindText.Name = "txtFindText"
        Me.txtFindText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFindText.Size = New System.Drawing.Size(121, 21)
        Me.txtFindText.TabIndex = 6
        '
        'cmdRemoveAll
        '
        Me.cmdRemoveAll.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRemoveAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRemoveAll.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRemoveAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRemoveAll.Location = New System.Drawing.Point(192, 192)
        Me.cmdRemoveAll.Name = "cmdRemoveAll"
        Me.cmdRemoveAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRemoveAll.Size = New System.Drawing.Size(65, 25)
        Me.cmdRemoveAll.TabIndex = 4
        Me.cmdRemoveAll.Text = "<<-"
        Me.cmdRemoveAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRemoveAll.UseVisualStyleBackColor = False
        '
        'cmdRemoveOne
        '
        Me.cmdRemoveOne.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRemoveOne.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRemoveOne.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRemoveOne.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRemoveOne.Location = New System.Drawing.Point(192, 160)
        Me.cmdRemoveOne.Name = "cmdRemoveOne"
        Me.cmdRemoveOne.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRemoveOne.Size = New System.Drawing.Size(65, 25)
        Me.cmdRemoveOne.TabIndex = 3
        Me.cmdRemoveOne.Text = "<-"
        Me.cmdRemoveOne.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRemoveOne.UseVisualStyleBackColor = False
        '
        'cmdAddAll
        '
        Me.cmdAddAll.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddAll.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddAll.Location = New System.Drawing.Point(192, 112)
        Me.cmdAddAll.Name = "cmdAddAll"
        Me.cmdAddAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddAll.Size = New System.Drawing.Size(65, 25)
        Me.cmdAddAll.TabIndex = 2
        Me.cmdAddAll.Text = "->>"
        Me.cmdAddAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddAll.UseVisualStyleBackColor = False
        '
        'cmdAddOne
        '
        Me.cmdAddOne.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddOne.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddOne.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddOne.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddOne.Location = New System.Drawing.Point(192, 80)
        Me.cmdAddOne.Name = "cmdAddOne"
        Me.cmdAddOne.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddOne.Size = New System.Drawing.Size(65, 25)
        Me.cmdAddOne.TabIndex = 1
        Me.cmdAddOne.Text = "->"
        Me.cmdAddOne.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddOne.UseVisualStyleBackColor = False
        '
        'lvwContents
        '
        Me.lvwContents.BackColor = System.Drawing.SystemColors.Window
        Me.lvwContents.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwContents_ColumnHeader_1})
        Me.lvwContents.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwContents.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwContents.FullRowSelect = True
        Me.lvwContents.Location = New System.Drawing.Point(264, 8)
        Me.lvwContents.Name = "lvwContents"
        Me.lvwContents.Size = New System.Drawing.Size(177, 281)
        'Me.lvwContents.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwContents.TabIndex = 5
        Me.lvwContents.UseCompatibleStateImageBehavior = False
        Me.lvwContents.View = System.Windows.Forms.View.Details
        '
        '_lvwContents_ColumnHeader_1
        '
        Me._lvwContents_ColumnHeader_1.Text = "Chosen"
        Me._lvwContents_ColumnHeader_1.Width = 97
        '
        'PickList
        '
        Me.Controls.Add(Me.lvwAll)
        Me.Controls.Add(Me.cmdFind)
        Me.Controls.Add(Me.txtFindText)
        Me.Controls.Add(Me.cmdRemoveAll)
        Me.Controls.Add(Me.cmdRemoveOne)
        Me.Controls.Add(Me.cmdAddAll)
        Me.Controls.Add(Me.cmdAddOne)
        Me.Controls.Add(Me.lvwContents)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "PickList"
        Me.Size = New System.Drawing.Size(449, 291)
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class