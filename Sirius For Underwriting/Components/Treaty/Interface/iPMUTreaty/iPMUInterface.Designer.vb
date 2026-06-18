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
	Public WithEvents chkHideDeleted As System.Windows.Forms.CheckBox
	Public WithEvents chkHideExpired As System.Windows.Forms.CheckBox
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdClose As System.Windows.Forms.Button
	Private WithEvents _lvwTreaties_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTreaties_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTreaties_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTreaties_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTreaties_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTreaties_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTreaties_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwTreaties As System.Windows.Forms.ListView
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkHideDeleted = New System.Windows.Forms.CheckBox
        Me.chkHideExpired = New System.Windows.Forms.CheckBox
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdClose = New System.Windows.Forms.Button
        Me.lvwTreaties = New System.Windows.Forms.ListView
        Me._lvwTreaties_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwTreaties_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwTreaties_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwTreaties_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwTreaties_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwTreaties_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwTreaties_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        'Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        'CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'chkHideDeleted
        '
        Me.chkHideDeleted.BackColor = System.Drawing.SystemColors.Control
        Me.chkHideDeleted.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkHideDeleted.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkHideDeleted.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkHideDeleted.Location = New System.Drawing.Point(456, 8)
        Me.chkHideDeleted.Name = "chkHideDeleted"
        Me.chkHideDeleted.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkHideDeleted.Size = New System.Drawing.Size(93, 15)
        Me.chkHideDeleted.TabIndex = 0
        Me.chkHideDeleted.Text = "Hide De&leted"
        Me.chkHideDeleted.UseVisualStyleBackColor = False
        '
        'chkHideExpired
        '
        Me.chkHideExpired.BackColor = System.Drawing.SystemColors.Control
        Me.chkHideExpired.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkHideExpired.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkHideExpired.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkHideExpired.Location = New System.Drawing.Point(566, 8)
        Me.chkHideExpired.Name = "chkHideExpired"
        Me.chkHideExpired.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkHideExpired.Size = New System.Drawing.Size(93, 16)
        Me.chkHideExpired.TabIndex = 1
        Me.chkHideExpired.Text = "Hide E&xpired"
        Me.chkHideExpired.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(4, 360)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 3
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(84, 360)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 4
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(164, 360)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 5
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClose.Location = New System.Drawing.Point(584, 360)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClose.Size = New System.Drawing.Size(73, 22)
        Me.cmdClose.TabIndex = 6
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClose.UseVisualStyleBackColor = False
        '
        'lvwTreaties
        '
        Me.lvwTreaties.BackColor = System.Drawing.SystemColors.Window
        Me.lvwTreaties.BorderStyle = System.Windows.Forms.BorderStyle.None
        'Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwTreaties, "")
        Me.lvwTreaties.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwTreaties_ColumnHeader_1, Me._lvwTreaties_ColumnHeader_2, Me._lvwTreaties_ColumnHeader_3, Me._lvwTreaties_ColumnHeader_4, Me._lvwTreaties_ColumnHeader_5, Me._lvwTreaties_ColumnHeader_6, Me._lvwTreaties_ColumnHeader_7})
        'Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwTreaties, True)
        Me.lvwTreaties.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwTreaties.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwTreaties.FullRowSelect = True
        Me.lvwTreaties.HideSelection = False
        'Me.listViewHelper1.SetItemClickMethod(Me.lvwTreaties, "lvwTreaties_ItemClick")
        'Me.listViewHelper1.SetLargeIcons(Me.lvwTreaties, "")
        Me.lvwTreaties.Location = New System.Drawing.Point(4, 27)
        Me.lvwTreaties.Name = "lvwTreaties"
        Me.lvwTreaties.Size = New System.Drawing.Size(653, 327)
        'Me.listViewHelper1.SetSmallIcons(Me.lvwTreaties, "")
        'Me.listViewHelper1.SetSorted(Me.lvwTreaties, False)
        'Me.listViewHelper1.SetSortKey(Me.lvwTreaties, 0)
        'Me.listViewHelper1.SetSortOrder(Me.lvwTreaties, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwTreaties.TabIndex = 2
        Me.lvwTreaties.UseCompatibleStateImageBehavior = False
        Me.lvwTreaties.View = System.Windows.Forms.View.Details
        '
        '_lvwTreaties_ColumnHeader_1
        '
        Me._lvwTreaties_ColumnHeader_1.Text = "Code"
        Me._lvwTreaties_ColumnHeader_1.Width = 101
        '
        '_lvwTreaties_ColumnHeader_2
        '
        Me._lvwTreaties_ColumnHeader_2.Text = "Description"
        Me._lvwTreaties_ColumnHeader_2.Width = 161
        '
        '_lvwTreaties_ColumnHeader_3
        '
        Me._lvwTreaties_ColumnHeader_3.Text = "Effective Date"
        Me._lvwTreaties_ColumnHeader_3.Width = 101
        '
        '_lvwTreaties_ColumnHeader_4
        '
        Me._lvwTreaties_ColumnHeader_4.Text = "Expiry Date"
        Me._lvwTreaties_ColumnHeader_4.Width = 101
        '
        '_lvwTreaties_ColumnHeader_5
        '
        Me._lvwTreaties_ColumnHeader_5.Text = "Agreement Code"
        Me._lvwTreaties_ColumnHeader_5.Width = 121
        '
        '_lvwTreaties_ColumnHeader_6
        '
        Me._lvwTreaties_ColumnHeader_6.Text = "RI Type"
        Me._lvwTreaties_ColumnHeader_6.Width = 101
        '
        '_lvwTreaties_ColumnHeader_7
        '
        Me._lvwTreaties_ColumnHeader_7.Text = "Replaces Treaty"
        Me._lvwTreaties_ColumnHeader_7.Width = 161
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(661, 386)
        Me.Controls.Add(Me.chkHideDeleted)
        Me.Controls.Add(Me.chkHideExpired)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.cmdEdit)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.lvwTreaties)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Treaty List"
        'CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class