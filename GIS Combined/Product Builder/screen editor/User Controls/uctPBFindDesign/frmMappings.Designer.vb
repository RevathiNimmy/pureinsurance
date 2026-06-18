<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMappings
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializeLine1()
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
    Public WithEvents txtEditCell As System.Windows.Forms.TextBox
    Public WithEvents cmdClear As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents lvwViewFields As System.Windows.Forms.ListView
    Public WithEvents cboView As System.Windows.Forms.ComboBox
    'Public WithEvents MSFlexGrid1 As unresolved-support-class
    Public WithEvents lvwControls As System.Windows.Forms.ListView
    Private WithEvents _Line1_0 As System.Windows.Forms.Label
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public Line1(0) As System.Windows.Forms.Label
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMappings))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtEditCell = New System.Windows.Forms.TextBox
        Me.cmdClear = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.lvwViewFields = New System.Windows.Forms.ListView
        Me.cboView = New System.Windows.Forms.ComboBox
        Me.lvwControls = New System.Windows.Forms.ListView
        Me._Line1_0 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.CustomColumn1 = New Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn
        Me.CustomColumn2 = New Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn
        Me.CustomColumn3 = New Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn
        Me.CustomColumn4 = New Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn
        Me.CustomColumn5 = New Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn
        Me.CustomColumn6 = New Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn
        Me.CustomColumn7 = New Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn
        Me.CustomColumn8 = New Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn
        Me.CustomColumn9 = New Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn
        Me.CustomColumn10 = New Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn
        Me.CustomColumn11 = New Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtEditCell
        '
        Me.txtEditCell.AcceptsReturn = True
        Me.txtEditCell.BackColor = System.Drawing.SystemColors.Highlight
        Me.txtEditCell.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEditCell.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEditCell.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEditCell.ForeColor = System.Drawing.SystemColors.Window
        Me.txtEditCell.Location = New System.Drawing.Point(368, 24)
        Me.txtEditCell.MaxLength = 255
        Me.txtEditCell.Name = "txtEditCell"
        Me.txtEditCell.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEditCell.Size = New System.Drawing.Size(121, 13)
        Me.txtEditCell.TabIndex = 10
        Me.txtEditCell.Visible = False
        '
        'cmdClear
        '
        Me.cmdClear.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClear.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClear.Location = New System.Drawing.Point(8, 312)
        Me.cmdClear.Name = "cmdClear"
        Me.cmdClear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClear.Size = New System.Drawing.Size(77, 22)
        Me.cmdClear.TabIndex = 6
        Me.cmdClear.Text = "Clear &Links"
        Me.cmdClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClear.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(896, 570)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 5
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
        Me.cmdOK.Location = New System.Drawing.Point(816, 570)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 4
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'lvwViewFields
        '
        Me.lvwViewFields.AllowDrop = True
        Me.lvwViewFields.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwViewFields, "")
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwViewFields, False)
        Me.lvwViewFields.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwViewFields.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me.lvwViewFields, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwViewFields, "")
        Me.lvwViewFields.Location = New System.Drawing.Point(8, 64)
        Me.lvwViewFields.Name = "lvwViewFields"
        Me.lvwViewFields.Size = New System.Drawing.Size(200, 247)
        Me.listViewHelper1.SetSmallIcons(Me.lvwViewFields, "")
        Me.listViewHelper1.SetSorted(Me.lvwViewFields, False)
        Me.listViewHelper1.SetSortKey(Me.lvwViewFields, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwViewFields, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwViewFields.TabIndex = 2
        Me.lvwViewFields.UseCompatibleStateImageBehavior = False
        '
        'cboView
        '
        Me.cboView.BackColor = System.Drawing.SystemColors.Window
        Me.cboView.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboView.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboView.Location = New System.Drawing.Point(64, 16)
        Me.cboView.Name = "cboView"
        Me.cboView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboView.Size = New System.Drawing.Size(145, 21)
        Me.cboView.TabIndex = 0
        '
        'lvwControls
        '
        Me.lvwControls.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwControls, "")
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwControls, False)
        Me.lvwControls.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwControls.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me.lvwControls, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwControls, "")
        Me.lvwControls.Location = New System.Drawing.Point(950, 64)
        Me.lvwControls.Name = "lvwControls"
        Me.lvwControls.Size = New System.Drawing.Size(10, 231)
        Me.listViewHelper1.SetSmallIcons(Me.lvwControls, "")
        Me.listViewHelper1.SetSorted(Me.lvwControls, False)
        Me.listViewHelper1.SetSortKey(Me.lvwControls, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwControls, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwControls.TabIndex = 3
        Me.lvwControls.UseCompatibleStateImageBehavior = False
        Me.lvwControls.Visible = False
        '
        '_Line1_0
        '
        Me._Line1_0.BackColor = System.Drawing.SystemColors.WindowText
        Me._Line1_0.Location = New System.Drawing.Point(208, 328)
        Me._Line1_0.Name = "_Line1_0"
        Me._Line1_0.Size = New System.Drawing.Size(64, 1)
        Me._Line1_0.TabIndex = 11
        Me._Line1_0.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(272, 48)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(106, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Risk Screen Controls"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(8, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "View Fields"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(8, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(41, 17)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "View"
        '
        'CustomColumn1
        '
        Me.CustomColumn1.DividerWidth = 1
        Me.CustomColumn1.HeaderText = ""
        Me.CustomColumn1.Name = "CustomColumn1"
        Me.CustomColumn1.Width = 66
        '
        'CustomColumn2
        '
        Me.CustomColumn2.DividerWidth = 1
        Me.CustomColumn2.HeaderText = ""
        Me.CustomColumn2.Name = "CustomColumn2"
        Me.CustomColumn2.Width = 66
        '
        'CustomColumn3
        '
        Me.CustomColumn3.DividerWidth = 1
        Me.CustomColumn3.HeaderText = ""
        Me.CustomColumn3.Name = "CustomColumn3"
        Me.CustomColumn3.Width = 66
        '
        'CustomColumn4
        '
        Me.CustomColumn4.DividerWidth = 1
        Me.CustomColumn4.HeaderText = ""
        Me.CustomColumn4.Name = "CustomColumn4"
        Me.CustomColumn4.Width = 66
        '
        'CustomColumn5
        '
        Me.CustomColumn5.DividerWidth = 1
        Me.CustomColumn5.HeaderText = ""
        Me.CustomColumn5.Name = "CustomColumn5"
        Me.CustomColumn5.Width = 66
        '
        'CustomColumn6
        '
        Me.CustomColumn6.DividerWidth = 1
        Me.CustomColumn6.HeaderText = ""
        Me.CustomColumn6.Name = "CustomColumn6"
        Me.CustomColumn6.Width = 66
        '
        'CustomColumn7
        '
        Me.CustomColumn7.DividerWidth = 1
        Me.CustomColumn7.HeaderText = ""
        Me.CustomColumn7.Name = "CustomColumn7"
        Me.CustomColumn7.Width = 66
        '
        'CustomColumn8
        '
        Me.CustomColumn8.DividerWidth = 1
        Me.CustomColumn8.HeaderText = ""
        Me.CustomColumn8.Name = "CustomColumn8"
        Me.CustomColumn8.Width = 66
        '
        'CustomColumn9
        '
        Me.CustomColumn9.DividerWidth = 1
        Me.CustomColumn9.HeaderText = ""
        Me.CustomColumn9.Name = "CustomColumn9"
        Me.CustomColumn9.Width = 66
        '
        'CustomColumn10
        '
        Me.CustomColumn10.DividerWidth = 1
        Me.CustomColumn10.HeaderText = ""
        Me.CustomColumn10.Name = "CustomColumn10"
        Me.CustomColumn10.Width = 66
        '
        'CustomColumn11
        '
        Me.CustomColumn11.DividerWidth = 1
        Me.CustomColumn11.HeaderText = ""
        Me.CustomColumn11.Name = "CustomColumn11"
        Me.CustomColumn11.Width = 66
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowDrop = True
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeColumns = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke
        Me.DataGridView1.Location = New System.Drawing.Point(275, 64)
        Me.DataGridView1.MaximumSize = New System.Drawing.Size(669, 500)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowHeadersWidth = 6
        Me.DataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.DataGridView1.Size = New System.Drawing.Size(669, 500)
        Me.DataGridView1.TabIndex = 12
        '
        'frmMappings
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(972, 595)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.txtEditCell)
        Me.Controls.Add(Me.cmdClear)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.lvwViewFields)
        Me.Controls.Add(Me.cboView)
        Me.Controls.Add(Me.lvwControls)
        Me.Controls.Add(Me._Line1_0)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(978, 620)
        Me.MinimizeBox = False
        Me.Name = "frmMappings"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Control Mappings"
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializeLine1()
        Me.Line1(0) = _Line1_0
    End Sub
    Friend CustomColumn1 As Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn
    Friend CustomColumn2 As Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn
    Friend CustomColumn3 As Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn
    Friend CustomColumn4 As Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn
    Friend CustomColumn5 As Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn
    Friend CustomColumn6 As Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn
    Friend CustomColumn7 As Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn
    Friend CustomColumn8 As Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn
    Friend CustomColumn9 As Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn
    Friend CustomColumn10 As Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn
    Friend CustomColumn11 As Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn
    Private WithEvents DataGridView1 As System.Windows.Forms.DataGridView
#End Region
End Class