<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmColumns
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
	Public WithEvents ProgressBar As System.Windows.Forms.ProgressBar
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Private WithEvents _lvwColumns_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwColumns_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwColumns_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwColumns_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwColumns As System.Windows.Forms.ListView
	Public WithEvents TreeView1 As System.Windows.Forms.TreeView
	Public WithEvents txtColumnHeader As System.Windows.Forms.TextBox
	Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents SSTab1 As System.Windows.Forms.TabControl
	Public WithEvents cmdView As System.Windows.Forms.Button
	Public WithEvents cmdImport As System.Windows.Forms.Button
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmColumns))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ProgressBar = New System.Windows.Forms.ProgressBar
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me.lvwColumns = New System.Windows.Forms.ListView
        Me._lvwColumns_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwColumns_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwColumns_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwColumns_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me.TreeView1 = New System.Windows.Forms.TreeView
        Me.txtColumnHeader = New System.Windows.Forms.TextBox
        Me.cmdView = New System.Windows.Forms.Button
        Me.cmdImport = New System.Windows.Forms.Button
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ProgressBar
        '
        Me.ProgressBar.Location = New System.Drawing.Point(12, 249)
        Me.ProgressBar.Name = "ProgressBar"
        Me.ProgressBar.Size = New System.Drawing.Size(212, 19)
        Me.ProgressBar.TabIndex = 6
        Me.ProgressBar.Visible = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(400, 248)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 4
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(464, 18)
        Me.SSTab1.Location = New System.Drawing.Point(8, 8)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(469, 237)
        Me.SSTab1.TabIndex = 2
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.lvwColumns)
        Me._SSTab1_TabPage0.Controls.Add(Me.TreeView1)
        Me._SSTab1_TabPage0.Controls.Add(Me.txtColumnHeader)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(461, 211)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "Column Definition"
        '
        'lvwColumns
        '
        Me.lvwColumns.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwColumns, "")
        Me.lvwColumns.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwColumns_ColumnHeader_1, Me._lvwColumns_ColumnHeader_2, Me._lvwColumns_ColumnHeader_3, Me._lvwColumns_ColumnHeader_4})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwColumns, True)
        Me.lvwColumns.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwColumns.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwColumns.GridLines = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwColumns, "lvwColumns_ItemClick")
        Me.listViewHelper1.SetLargeIcons(Me.lvwColumns, "")
        Me.lvwColumns.Location = New System.Drawing.Point(8, 0)
        Me.lvwColumns.Name = "lvwColumns"
        Me.lvwColumns.Size = New System.Drawing.Size(449, 208)
        Me.listViewHelper1.SetSmallIcons(Me.lvwColumns, "")
        Me.listViewHelper1.SetSorted(Me.lvwColumns, False)
        Me.listViewHelper1.SetSortKey(Me.lvwColumns, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwColumns, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwColumns.TabIndex = 3
        Me.lvwColumns.UseCompatibleStateImageBehavior = False
        Me.lvwColumns.View = System.Windows.Forms.View.Details
        '
        '_lvwColumns_ColumnHeader_1
        '
        Me._lvwColumns_ColumnHeader_1.Text = "Position"
        Me._lvwColumns_ColumnHeader_1.Width = 97
        '
        '_lvwColumns_ColumnHeader_2
        '
        Me._lvwColumns_ColumnHeader_2.Text = "Column Name"
        Me._lvwColumns_ColumnHeader_2.Width = 97
        '
        '_lvwColumns_ColumnHeader_3
        '
        Me._lvwColumns_ColumnHeader_3.Text = "Code ?"
        Me._lvwColumns_ColumnHeader_3.Width = 97
        '
        '_lvwColumns_ColumnHeader_4
        '
        Me._lvwColumns_ColumnHeader_4.Text = "Include ?"
        Me._lvwColumns_ColumnHeader_4.Width = 97
        '
        'TreeView1
        '
        Me.TreeView1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TreeView1.LabelEdit = True
        Me.TreeView1.Location = New System.Drawing.Point(160, 76)
        Me.TreeView1.Name = "TreeView1"
        Me.TreeView1.Size = New System.Drawing.Size(2, 9)
        Me.TreeView1.TabIndex = 5
        '
        'txtColumnHeader
        '
        Me.txtColumnHeader.AcceptsReturn = True
        Me.txtColumnHeader.BackColor = System.Drawing.SystemColors.Highlight
        Me.txtColumnHeader.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtColumnHeader.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtColumnHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtColumnHeader.ForeColor = System.Drawing.Color.White
        Me.txtColumnHeader.Location = New System.Drawing.Point(168, 140)
        Me.txtColumnHeader.MaxLength = 0
        Me.txtColumnHeader.Name = "txtColumnHeader"
        Me.txtColumnHeader.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtColumnHeader.Size = New System.Drawing.Size(105, 13)
        Me.txtColumnHeader.TabIndex = 7
        Me.txtColumnHeader.Visible = False
        '
        'cmdView
        '
        Me.cmdView.BackColor = System.Drawing.SystemColors.Control
        Me.cmdView.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdView.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdView.Location = New System.Drawing.Point(230, 248)
        Me.cmdView.Name = "cmdView"
        Me.cmdView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdView.Size = New System.Drawing.Size(73, 22)
        Me.cmdView.TabIndex = 1
        Me.cmdView.Text = "View"
        Me.cmdView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdView.UseVisualStyleBackColor = False
        '
        'cmdImport
        '
        Me.cmdImport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdImport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdImport.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdImport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdImport.Location = New System.Drawing.Point(312, 248)
        Me.cmdImport.Name = "cmdImport"
        Me.cmdImport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdImport.Size = New System.Drawing.Size(73, 22)
        Me.cmdImport.TabIndex = 0
        Me.cmdImport.Text = "Import"
        Me.cmdImport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdImport.UseVisualStyleBackColor = False
        '
        'frmColumns
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(479, 276)
        Me.Controls.Add(Me.ProgressBar)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdView)
        Me.Controls.Add(Me.cmdImport)
        Me.Controls.Add(Me.SSTab1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmColumns"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Import New List"
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me._SSTab1_TabPage0.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class