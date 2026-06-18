<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctSIRSelectClauses
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwSelectClause_InitializeColumnKeys()
	End Sub

	'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public WithEvents imglImages As System.Windows.Forms.ImageList
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Friend WithEvents cmdProperties As System.Windows.Forms.Button
	Friend WithEvents ImageList As System.Windows.Forms.ImageList
	Friend WithEvents _lvwSelectClause_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSelectClause_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSelectClause_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSelectClause_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvwSelectClause As System.Windows.Forms.ListView
    'developer guide no.(Commented as conflicted with the icon displayed in the listview)
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctSIRSelectClauses))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdProperties = New System.Windows.Forms.Button
        Me.ImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.lvwSelectClause = New System.Windows.Forms.ListView
        Me._lvwSelectClause_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwSelectClause_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwSelectClause_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwSelectClause_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me.SuspendLayout()
        '
        'cmdProperties
        '
        Me.cmdProperties.BackColor = System.Drawing.SystemColors.Control
        Me.cmdProperties.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdProperties.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdProperties.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdProperties.Location = New System.Drawing.Point(524, 304)
        Me.cmdProperties.Name = "cmdProperties"
        Me.cmdProperties.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdProperties.Size = New System.Drawing.Size(71, 23)
        Me.cmdProperties.TabIndex = 1
        Me.cmdProperties.Text = "&Select"
        Me.cmdProperties.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdProperties.UseVisualStyleBackColor = False
        '
        'ImageList
        '
        Me.ImageList.ImageStream = CType(resources.GetObject("ImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList.Images.SetKeyName(0, "add")
        Me.ImageList.Images.SetKeyName(1, "history")
        Me.ImageList.Images.SetKeyName(2, "edited")
        Me.ImageList.Images.SetKeyName(3, "delete")
        Me.ImageList.Images.SetKeyName(4, "saved")
        '
        'lvwSelectClause
        '
        Me.lvwSelectClause.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSelectClause.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSelectClause_ColumnHeader_1, Me._lvwSelectClause_ColumnHeader_2, Me._lvwSelectClause_ColumnHeader_3, Me._lvwSelectClause_ColumnHeader_4})
        Me.lvwSelectClause.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSelectClause.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSelectClause.FullRowSelect = True
        Me.lvwSelectClause.Location = New System.Drawing.Point(-2, -2)
        Me.lvwSelectClause.Name = "lvwSelectClause"
        Me.lvwSelectClause.OwnerDraw = True
        Me.lvwSelectClause.Size = New System.Drawing.Size(525, 327)
        Me.lvwSelectClause.SmallImageList = Me.ImageList
        Me.lvwSelectClause.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwSelectClause.TabIndex = 0
        Me.lvwSelectClause.UseCompatibleStateImageBehavior = False
        Me.lvwSelectClause.View = System.Windows.Forms.View.Details
        '
        '_lvwSelectClause_ColumnHeader_1
        '
        Me._lvwSelectClause_ColumnHeader_1.Text = "Code"
        Me._lvwSelectClause_ColumnHeader_1.Width = 124
        '
        '_lvwSelectClause_ColumnHeader_2
        '
        Me._lvwSelectClause_ColumnHeader_2.Text = "Description"
        Me._lvwSelectClause_ColumnHeader_2.Width = 192
        '
        '_lvwSelectClause_ColumnHeader_3
        '
        Me._lvwSelectClause_ColumnHeader_3.Text = "Selected"
        Me._lvwSelectClause_ColumnHeader_3.Width = 104
        '
        '_lvwSelectClause_ColumnHeader_4
        '
        Me._lvwSelectClause_ColumnHeader_4.Text = "Default"
        Me._lvwSelectClause_ColumnHeader_4.Width = 104
        '
        'uctSIRSelectClauses
        '
        Me.Controls.Add(Me.cmdProperties)
        Me.Controls.Add(Me.lvwSelectClause)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctSIRSelectClauses"
        Me.Size = New System.Drawing.Size(595, 328)
        Me.ResumeLayout(False)

    End Sub
	Sub lvwSelectClause_InitializeColumnKeys()
		Me._lvwSelectClause_ColumnHeader_1.Name = "Code"
		Me._lvwSelectClause_ColumnHeader_2.Name = "Description"
		Me._lvwSelectClause_ColumnHeader_3.Name = "Selected"
		Me._lvwSelectClause_ColumnHeader_4.Name = "Default"
	End Sub
#End Region 
End Class