<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmMap
#Region "Windows Form Designer generated code "
    'NIIT- Constructor is overloaded to get control values from FrmImport 
    Public str_ListType As String, Int_ListVers As Integer, DT_EffDate As Date
    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        InitializeLine1()
    End Sub
    'NIIT- Constructor is overloaded to get control values from FrmImport 
    Public Sub New(ByVal s_ListType As String, ByVal i_ListVersion As Integer, ByVal d_EffDate As Date)
        MyBase.New()
        'This call is required by the Windows Form Designer.
        str_ListType = s_ListType
        Int_ListVers = i_ListVersion
        DT_EffDate = d_EffDate

        InitializeComponent()
        InitializeLine1()
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
    Public WithEvents cmdClearMapping As System.Windows.Forms.Button
    Private WithEvents _Line1_0 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Private WithEvents _lvwCustomFields_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwCustomFields As System.Windows.Forms.ListView
    Private WithEvents _lvwNewFields_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwNewFields As System.Windows.Forms.ListView
    Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents SSTab1 As System.Windows.Forms.TabControl
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdUpdate As System.Windows.Forms.Button
    Public Line1(0) As System.Windows.Forms.Label
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMap))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdClearMapping = New System.Windows.Forms.Button
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me._Line1_0 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.lvwCustomFields = New System.Windows.Forms.ListView
        Me._lvwCustomFields_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me.lvwNewFields = New System.Windows.Forms.ListView
        Me._lvwNewFields_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdUpdate = New System.Windows.Forms.Button
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdClearMapping
        '
        Me.cmdClearMapping.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClearMapping.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClearMapping.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClearMapping.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClearMapping.Location = New System.Drawing.Point(288, 320)
        Me.cmdClearMapping.Name = "cmdClearMapping"
        Me.cmdClearMapping.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClearMapping.Size = New System.Drawing.Size(73, 22)
        Me.cmdClearMapping.TabIndex = 7
        Me.cmdClearMapping.Text = "Clear Links"
        Me.cmdClearMapping.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClearMapping.UseVisualStyleBackColor = False
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(528, 18)
        Me.SSTab1.Location = New System.Drawing.Point(8, 8)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(533, 309)
        Me.SSTab1.TabIndex = 2
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me._Line1_0)
        Me._SSTab1_TabPage0.Controls.Add(Me.Label1)
        Me._SSTab1_TabPage0.Controls.Add(Me.Label2)
        Me._SSTab1_TabPage0.Controls.Add(Me.lvwCustomFields)
        Me._SSTab1_TabPage0.Controls.Add(Me.lvwNewFields)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(525, 283)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "Field Mappings"
        '
        '_Line1_0
        '
        Me._Line1_0.BackColor = System.Drawing.SystemColors.WindowText
        Me._Line1_0.Location = New System.Drawing.Point(224, 52)
        Me._Line1_0.Name = "_Line1_0"
        Me._Line1_0.Size = New System.Drawing.Size(80, 10)
        Me._Line1_0.TabIndex = 0
        Me._Line1_0.Visible = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(312, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(161, 17)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Custom Entries"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(8, 28)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(153, 17)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "New Field Entries"
        '
        'lvwCustomFields
        '
        Me.lvwCustomFields.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwCustomFields, "")
        Me.lvwCustomFields.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwCustomFields_ColumnHeader_1})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwCustomFields, False)
        Me.lvwCustomFields.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwCustomFields.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwCustomFields.FullRowSelect = True
        Me.lvwCustomFields.GridLines = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwCustomFields, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwCustomFields, "")
        Me.lvwCustomFields.Location = New System.Drawing.Point(312, 44)
        Me.lvwCustomFields.Name = "lvwCustomFields"
        Me.lvwCustomFields.Size = New System.Drawing.Size(209, 233)
        Me.listViewHelper1.SetSmallIcons(Me.lvwCustomFields, "")
        Me.listViewHelper1.SetSorted(Me.lvwCustomFields, False)
        Me.listViewHelper1.SetSortKey(Me.lvwCustomFields, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwCustomFields, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwCustomFields.TabIndex = 4
        Me.lvwCustomFields.UseCompatibleStateImageBehavior = False
        Me.lvwCustomFields.View = System.Windows.Forms.View.Details
        '
        '_lvwCustomFields_ColumnHeader_1
        '
        Me._lvwCustomFields_ColumnHeader_1.Text = "Custom List Items"
        Me._lvwCustomFields_ColumnHeader_1.Width = 97
        '
        'lvwNewFields
        '
        Me.lvwNewFields.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwNewFields, "")
        Me.lvwNewFields.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwNewFields_ColumnHeader_1})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwNewFields, False)
        Me.lvwNewFields.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwNewFields.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwNewFields.FullRowSelect = True
        Me.lvwNewFields.GridLines = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwNewFields, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwNewFields, "")
        Me.lvwNewFields.Location = New System.Drawing.Point(8, 44)
        Me.lvwNewFields.Name = "lvwNewFields"
        Me.lvwNewFields.Size = New System.Drawing.Size(209, 233)
        Me.listViewHelper1.SetSmallIcons(Me.lvwNewFields, "")
        Me.listViewHelper1.SetSorted(Me.lvwNewFields, False)
        Me.listViewHelper1.SetSortKey(Me.lvwNewFields, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwNewFields, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwNewFields.TabIndex = 3
        Me.lvwNewFields.UseCompatibleStateImageBehavior = False
        Me.lvwNewFields.View = System.Windows.Forms.View.Details
        '
        '_lvwNewFields_ColumnHeader_1
        '
        Me._lvwNewFields_ColumnHeader_1.Text = "New List Items"
        Me._lvwNewFields_ColumnHeader_1.Width = 97
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(464, 320)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdUpdate
        '
        Me.cmdUpdate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdUpdate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdUpdate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdUpdate.Location = New System.Drawing.Point(376, 320)
        Me.cmdUpdate.Name = "cmdUpdate"
        Me.cmdUpdate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdUpdate.Size = New System.Drawing.Size(73, 22)
        Me.cmdUpdate.TabIndex = 0
        Me.cmdUpdate.Text = "Update List"
        Me.cmdUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdUpdate.UseVisualStyleBackColor = False
        '
        'FrmMap
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(547, 347)
        Me.Controls.Add(Me.cmdClearMapping)
        Me.Controls.Add(Me.SSTab1)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdUpdate)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmMap"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Post Import Clearup"
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Sub InitializeLine1()
        Me.Line1(0) = _Line1_0
    End Sub
#End Region
End Class