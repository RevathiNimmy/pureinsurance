<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwRiskTypeRIValues_InitializeColumnKeys()
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
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblRiskType As System.Windows.Forms.Label
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Private WithEvents _lvwRiskTypeRIValues_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRiskTypeRIValues_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwRiskTypeRIValues As System.Windows.Forms.ListView
	Public WithEvents pnlRiskType As System.Windows.Forms.Panel
	Public WithEvents cmdValues As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents lblValue As System.Windows.Forms.Label
	Public WithEvents cmdDetailOK As System.Windows.Forms.Button
	Public WithEvents cmdDetailCancel As System.Windows.Forms.Button
	Public WithEvents txtValue As System.Windows.Forms.TextBox
	Private WithEvents _tabDetailTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabDetailTab As System.Windows.Forms.TabControl
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public WithEvents ImageList1 As System.Windows.Forms.ImageList
    'TODOLIST-Commented the listviewhelper as it was conflicting with icon displai in listview)
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblRiskType = New System.Windows.Forms.Label
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.lvwRiskTypeRIValues = New System.Windows.Forms.ListView
        Me._lvwRiskTypeRIValues_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwRiskTypeRIValues_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.pnlRiskType = New System.Windows.Forms.Panel
        Me.cmdValues = New System.Windows.Forms.Button
        Me.tabDetailTab = New System.Windows.Forms.TabControl
        Me._tabDetailTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblValue = New System.Windows.Forms.Label
        Me.cmdDetailOK = New System.Windows.Forms.Button
        Me.cmdDetailCancel = New System.Windows.Forms.Button
        Me.txtValue = New System.Windows.Forms.TextBox
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.lblpRiskType = New System.Windows.Forms.Label
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.pnlRiskType.SuspendLayout()
        Me.tabDetailTab.SuspendLayout()
        Me._tabDetailTab_TabPage0.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(536, 296)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 6
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(456, 296)
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
        Me.cmdOK.Location = New System.Drawing.Point(368, 296)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 4
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(600, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(605, 277)
        Me.tabMainTab.TabIndex = 8
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblRiskType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdEdit)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwRiskTypeRIValues)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pnlRiskType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdValues)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(597, 251)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "&1 - RI Values"
        '
        'lblRiskType
        '
        Me.lblRiskType.BackColor = System.Drawing.SystemColors.Control
        Me.lblRiskType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRiskType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRiskType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRiskType.Location = New System.Drawing.Point(8, 20)
        Me.lblRiskType.Name = "lblRiskType"
        Me.lblRiskType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRiskType.Size = New System.Drawing.Size(89, 17)
        Me.lblRiskType.TabIndex = 9
        Me.lblRiskType.Text = "Risk Type:"
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Enabled = False
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(520, 220)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(65, 22)
        Me.cmdEdit.TabIndex = 1
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'lvwRiskTypeRIValues
        '
        Me.lvwRiskTypeRIValues.BackColor = System.Drawing.SystemColors.Window
        Me.lvwRiskTypeRIValues.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwRiskTypeRIValues.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwRiskTypeRIValues_ColumnHeader_1, Me._lvwRiskTypeRIValues_ColumnHeader_2})
        Me.lvwRiskTypeRIValues.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwRiskTypeRIValues.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwRiskTypeRIValues.LargeImageList = Me.ImageList1
        Me.lvwRiskTypeRIValues.Location = New System.Drawing.Point(8, 44)
        Me.lvwRiskTypeRIValues.Name = "lvwRiskTypeRIValues"
        Me.lvwRiskTypeRIValues.Size = New System.Drawing.Size(577, 169)
        Me.lvwRiskTypeRIValues.SmallImageList = Me.ImageList1
        Me.lvwRiskTypeRIValues.TabIndex = 0
        Me.lvwRiskTypeRIValues.UseCompatibleStateImageBehavior = False
        Me.lvwRiskTypeRIValues.View = System.Windows.Forms.View.Details
        '
        '_lvwRiskTypeRIValues_ColumnHeader_1
        '
        Me._lvwRiskTypeRIValues_ColumnHeader_1.Tag = ""
        Me._lvwRiskTypeRIValues_ColumnHeader_1.Text = "Breakdown"
        Me._lvwRiskTypeRIValues_ColumnHeader_1.Width = 201
        '
        '_lvwRiskTypeRIValues_ColumnHeader_2
        '
        Me._lvwRiskTypeRIValues_ColumnHeader_2.Tag = ""
        Me._lvwRiskTypeRIValues_ColumnHeader_2.Text = "Value"
        Me._lvwRiskTypeRIValues_ColumnHeader_2.Width = 97
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "FindImage")
        '
        'pnlRiskType
        '
        Me.pnlRiskType.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.pnlRiskType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlRiskType.Controls.Add(Me.lblpRiskType)
        Me.pnlRiskType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlRiskType.Location = New System.Drawing.Point(103, 20)
        Me.pnlRiskType.Name = "pnlRiskType"
        Me.pnlRiskType.Size = New System.Drawing.Size(114, 18)
        Me.pnlRiskType.TabIndex = 10
        '
        'cmdValues
        '
        Me.cmdValues.BackColor = System.Drawing.SystemColors.Control
        Me.cmdValues.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdValues.Enabled = False
        Me.cmdValues.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdValues.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdValues.Location = New System.Drawing.Point(8, 220)
        Me.cmdValues.Name = "cmdValues"
        Me.cmdValues.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdValues.Size = New System.Drawing.Size(73, 22)
        Me.cmdValues.TabIndex = 12
        Me.cmdValues.Text = "&Values"
        Me.cmdValues.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdValues.UseVisualStyleBackColor = False
        '
        'tabDetailTab
        '
        Me.tabDetailTab.Appearance = System.Windows.Forms.TabAppearance.Normal
        Me.tabDetailTab.Controls.Add(Me._tabDetailTab_TabPage0)
        Me.tabDetailTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabDetailTab.ItemSize = New System.Drawing.Size(600, 18)
        Me.tabDetailTab.Location = New System.Drawing.Point(8, 675)
        Me.tabDetailTab.Multiline = True
        Me.tabDetailTab.Name = "tabDetailTab"
        Me.tabDetailTab.SelectedIndex = 0
        Me.tabDetailTab.Size = New System.Drawing.Size(605, 285)
        Me.tabDetailTab.SizeMode = System.Windows.Forms.TabSizeMode.Normal
        Me.tabDetailTab.TabIndex = 7
        '
        '_tabDetailTab_TabPage0
        '
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblValue)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdDetailOK)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdDetailCancel)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtValue)
        Me._tabDetailTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabDetailTab_TabPage0.Name = "_tabDetailTab_TabPage0"
        Me._tabDetailTab_TabPage0.Size = New System.Drawing.Size(597, 259)
        Me._tabDetailTab_TabPage0.TabIndex = 0
        Me._tabDetailTab_TabPage0.Text = "&2 - Value"
        '
        'lblValue
        '
        Me.lblValue.BackColor = System.Drawing.SystemColors.Control
        Me.lblValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblValue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblValue.Location = New System.Drawing.Point(24, 31)
        Me.lblValue.Name = "lblValue"
        Me.lblValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblValue.Size = New System.Drawing.Size(70, 17)
        Me.lblValue.TabIndex = 11
        Me.lblValue.Text = "Value:"
        '
        'cmdDetailOK
        '
        Me.cmdDetailOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDetailOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDetailOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDetailOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDetailOK.Location = New System.Drawing.Point(448, 228)
        Me.cmdDetailOK.Name = "cmdDetailOK"
        Me.cmdDetailOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDetailOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdDetailOK.TabIndex = 2
        Me.cmdDetailOK.Text = "&OK"
        Me.cmdDetailOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDetailOK.UseVisualStyleBackColor = False
        '
        'cmdDetailCancel
        '
        Me.cmdDetailCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDetailCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDetailCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDetailCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDetailCancel.Location = New System.Drawing.Point(528, 228)
        Me.cmdDetailCancel.Name = "cmdDetailCancel"
        Me.cmdDetailCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDetailCancel.Size = New System.Drawing.Size(65, 22)
        Me.cmdDetailCancel.TabIndex = 3
        Me.cmdDetailCancel.Text = "&Cancel"
        Me.cmdDetailCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDetailCancel.UseVisualStyleBackColor = False
        '
        'txtValue
        '
        Me.txtValue.AcceptsReturn = True
        Me.txtValue.BackColor = System.Drawing.SystemColors.Window
        Me.txtValue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtValue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtValue.Location = New System.Drawing.Point(104, 28)
        Me.txtValue.MaxLength = 0
        Me.txtValue.Name = "txtValue"
        Me.txtValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtValue.Size = New System.Drawing.Size(105, 20)
        Me.txtValue.TabIndex = 13
        '
        'lblpRiskType
        '
        Me.lblpRiskType.AutoSize = True
        Me.lblpRiskType.Location = New System.Drawing.Point(1, 2)
        Me.lblpRiskType.Name = "lblpRiskType"
        Me.lblpRiskType.Size = New System.Drawing.Size(0, 13)
        Me.lblpRiskType.TabIndex = 0
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(619, 321)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.tabDetailTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "RI Values"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.pnlRiskType.ResumeLayout(False)
        Me.pnlRiskType.PerformLayout()
        Me.tabDetailTab.ResumeLayout(False)
        Me._tabDetailTab_TabPage0.ResumeLayout(False)
        Me._tabDetailTab_TabPage0.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Sub lvwRiskTypeRIValues_InitializeColumnKeys()
        Me._lvwRiskTypeRIValues_ColumnHeader_1.Name = ""
        Me._lvwRiskTypeRIValues_ColumnHeader_2.Name = ""
    End Sub
    Friend WithEvents lblpRiskType As System.Windows.Forms.Label
#End Region 
End Class