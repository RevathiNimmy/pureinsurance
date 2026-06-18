<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwLookupData_InitializeColumnKeys()
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
	Public WithEvents lblLineKey As System.Windows.Forms.Label
	Public WithEvents lblKeyLevel As System.Windows.Forms.Label
	Public WithEvents lblValue As System.Windows.Forms.Label
	Public WithEvents lblType As System.Windows.Forms.Label
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents txtLineKey As System.Windows.Forms.TextBox
	Public WithEvents txtKeyLevel As System.Windows.Forms.TextBox
	Public WithEvents txtValue As System.Windows.Forms.TextBox
	Public WithEvents cboType As System.Windows.Forms.ComboBox
	Private WithEvents _tabLookupDetail_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabLookupDetail As System.Windows.Forms.TabControl
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Private WithEvents _lvwLookupData_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwLookupData_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwLookupData_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwLookupData_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwLookupData As System.Windows.Forms.ListView
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Private WithEvents _tabMaintab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMaintab As System.Windows.Forms.TabControl
	Public WithEvents ImageList1 As System.Windows.Forms.ImageList
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tabLookupDetail = New System.Windows.Forms.TabControl
        Me._tabLookupDetail_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblLineKey = New System.Windows.Forms.Label
        Me.lblKeyLevel = New System.Windows.Forms.Label
        Me.lblValue = New System.Windows.Forms.Label
        Me.lblType = New System.Windows.Forms.Label
        Me.txtLineKey = New System.Windows.Forms.TextBox
        Me.txtKeyLevel = New System.Windows.Forms.TextBox
        Me.txtValue = New System.Windows.Forms.TextBox
        Me.cboType = New System.Windows.Forms.ComboBox
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.tabMaintab = New System.Windows.Forms.TabControl
        Me._tabMaintab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lvwLookupData = New System.Windows.Forms.ListView
        Me._lvwLookupData_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwLookupData_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwLookupData_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwLookupData_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.tabLookupDetail.SuspendLayout()
        Me._tabLookupDetail_TabPage0.SuspendLayout()
        Me.tabMaintab.SuspendLayout()
        Me._tabMaintab_TabPage0.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabLookupDetail
        '
        Me.tabLookupDetail.Controls.Add(Me._tabLookupDetail_TabPage0)
        Me.tabLookupDetail.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabLookupDetail.ItemSize = New System.Drawing.Size(600, 18)
        Me.tabLookupDetail.Location = New System.Drawing.Point(-232, 221)
        Me.tabLookupDetail.Multiline = True
        Me.tabLookupDetail.Name = "tabLookupDetail"
        Me.tabLookupDetail.SelectedIndex = 0
        Me.tabLookupDetail.Size = New System.Drawing.Size(605, 279)
        Me.tabLookupDetail.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.tabLookupDetail.TabIndex = 8
        '
        '_tabLookupDetail_TabPage0
        '
        Me._tabLookupDetail_TabPage0.Controls.Add(Me.lblLineKey)
        Me._tabLookupDetail_TabPage0.Controls.Add(Me.lblKeyLevel)
        Me._tabLookupDetail_TabPage0.Controls.Add(Me.lblValue)
        Me._tabLookupDetail_TabPage0.Controls.Add(Me.lblType)
        Me._tabLookupDetail_TabPage0.Controls.Add(Me.txtLineKey)
        Me._tabLookupDetail_TabPage0.Controls.Add(Me.txtKeyLevel)
        Me._tabLookupDetail_TabPage0.Controls.Add(Me.txtValue)
        Me._tabLookupDetail_TabPage0.Controls.Add(Me.cboType)
        Me._tabLookupDetail_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabLookupDetail_TabPage0.Name = "_tabLookupDetail_TabPage0"
        Me._tabLookupDetail_TabPage0.Size = New System.Drawing.Size(597, 253)
        Me._tabLookupDetail_TabPage0.TabIndex = 0
        Me._tabLookupDetail_TabPage0.Text = "1-Header Details"
        '
        'lblLineKey
        '
        Me.lblLineKey.BackColor = System.Drawing.SystemColors.Control
        Me.lblLineKey.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLineKey.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLineKey.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLineKey.Location = New System.Drawing.Point(9, 19)
        Me.lblLineKey.Name = "lblLineKey"
        Me.lblLineKey.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLineKey.Size = New System.Drawing.Size(106, 17)
        Me.lblLineKey.TabIndex = 10
        Me.lblLineKey.Text = "Line Key"
        '
        'lblKeyLevel
        '
        Me.lblKeyLevel.BackColor = System.Drawing.SystemColors.Control
        Me.lblKeyLevel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblKeyLevel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblKeyLevel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblKeyLevel.Location = New System.Drawing.Point(9, 51)
        Me.lblKeyLevel.Name = "lblKeyLevel"
        Me.lblKeyLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblKeyLevel.Size = New System.Drawing.Size(106, 17)
        Me.lblKeyLevel.TabIndex = 12
        Me.lblKeyLevel.Text = "Key Level"
        '
        'lblValue
        '
        Me.lblValue.BackColor = System.Drawing.SystemColors.Control
        Me.lblValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblValue.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblValue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblValue.Location = New System.Drawing.Point(9, 83)
        Me.lblValue.Name = "lblValue"
        Me.lblValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblValue.Size = New System.Drawing.Size(106, 17)
        Me.lblValue.TabIndex = 14
        Me.lblValue.Text = "Value"
        '
        'lblType
        '
        Me.lblType.BackColor = System.Drawing.SystemColors.Control
        Me.lblType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblType.Location = New System.Drawing.Point(9, 115)
        Me.lblType.Name = "lblType"
        Me.lblType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblType.Size = New System.Drawing.Size(106, 17)
        Me.lblType.TabIndex = 15
        Me.lblType.Text = "Type"
        '
        'txtLineKey
        '
        Me.txtLineKey.AcceptsReturn = True
        Me.txtLineKey.BackColor = System.Drawing.SystemColors.GrayText
        Me.txtLineKey.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLineKey.Enabled = False
        Me.txtLineKey.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLineKey.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLineKey.Location = New System.Drawing.Point(120, 16)
        Me.txtLineKey.MaxLength = 0
        Me.txtLineKey.Name = "txtLineKey"
        Me.txtLineKey.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLineKey.Size = New System.Drawing.Size(126, 20)
        Me.txtLineKey.TabIndex = 9
        '
        'txtKeyLevel
        '
        Me.txtKeyLevel.AcceptsReturn = True
        Me.txtKeyLevel.BackColor = System.Drawing.SystemColors.Window
        Me.txtKeyLevel.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtKeyLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtKeyLevel.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtKeyLevel.Location = New System.Drawing.Point(120, 48)
        Me.txtKeyLevel.MaxLength = 0
        Me.txtKeyLevel.Name = "txtKeyLevel"
        Me.txtKeyLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtKeyLevel.Size = New System.Drawing.Size(126, 20)
        Me.txtKeyLevel.TabIndex = 11
        '
        'txtValue
        '
        Me.txtValue.AcceptsReturn = True
        Me.txtValue.BackColor = System.Drawing.SystemColors.Window
        Me.txtValue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtValue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtValue.Location = New System.Drawing.Point(120, 80)
        Me.txtValue.MaxLength = 0
        Me.txtValue.Name = "txtValue"
        Me.txtValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtValue.Size = New System.Drawing.Size(126, 20)
        Me.txtValue.TabIndex = 13
        '
        'cboType
        '
        Me.cboType.BackColor = System.Drawing.SystemColors.Window
        Me.cboType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboType.Location = New System.Drawing.Point(120, 112)
        Me.cboType.Name = "cboType"
        Me.cboType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboType.Size = New System.Drawing.Size(126, 21)
        Me.cboType.TabIndex = 16
        Me.cboType.Text = "Combo1"
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(360, 292)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 3
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(448, 292)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 4
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(544, 292)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 5
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'tabMaintab
        '
        Me.tabMaintab.Controls.Add(Me._tabMaintab_TabPage0)
        Me.tabMaintab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMaintab.ItemSize = New System.Drawing.Size(600, 18)
        Me.tabMaintab.Location = New System.Drawing.Point(8, 8)
        Me.tabMaintab.Multiline = True
        Me.tabMaintab.Name = "tabMaintab"
        Me.tabMaintab.SelectedIndex = 0
        Me.tabMaintab.Size = New System.Drawing.Size(605, 279)
        Me.tabMaintab.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.tabMaintab.TabIndex = 6
        '
        '_tabMaintab_TabPage0
        '
        Me._tabMaintab_TabPage0.Controls.Add(Me.lvwLookupData)
        Me._tabMaintab_TabPage0.Controls.Add(Me.cmdDelete)
        Me._tabMaintab_TabPage0.Controls.Add(Me.cmdEdit)
        Me._tabMaintab_TabPage0.Controls.Add(Me.cmdAdd)
        Me._tabMaintab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMaintab_TabPage0.Name = "_tabMaintab_TabPage0"
        Me._tabMaintab_TabPage0.Size = New System.Drawing.Size(597, 253)
        Me._tabMaintab_TabPage0.TabIndex = 0
        Me._tabMaintab_TabPage0.Text = "1-Lookup Data"
        '
        'lvwLookupData
        '
        Me.lvwLookupData.BackColor = System.Drawing.SystemColors.Window
        Me.lvwLookupData.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwLookupData.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwLookupData_ColumnHeader_1, Me._lvwLookupData_ColumnHeader_2, Me._lvwLookupData_ColumnHeader_3, Me._lvwLookupData_ColumnHeader_4})
        Me.lvwLookupData.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwLookupData.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwLookupData.LargeImageList = Me.ImageList1
        Me.lvwLookupData.Location = New System.Drawing.Point(8, 12)
        Me.lvwLookupData.Name = "lvwLookupData"
        Me.lvwLookupData.Size = New System.Drawing.Size(584, 201)
        Me.lvwLookupData.SmallImageList = Me.ImageList1
        Me.lvwLookupData.TabIndex = 7
        Me.lvwLookupData.UseCompatibleStateImageBehavior = False
        Me.lvwLookupData.View = System.Windows.Forms.View.Details
        '
        '_lvwLookupData_ColumnHeader_1
        '
        Me._lvwLookupData_ColumnHeader_1.Tag = ""
        Me._lvwLookupData_ColumnHeader_1.Text = "Line Key"
        Me._lvwLookupData_ColumnHeader_1.Width = 134
        '
        '_lvwLookupData_ColumnHeader_2
        '
        Me._lvwLookupData_ColumnHeader_2.Tag = ""
        Me._lvwLookupData_ColumnHeader_2.Text = "Key Level"
        Me._lvwLookupData_ColumnHeader_2.Width = 97
        '
        '_lvwLookupData_ColumnHeader_3
        '
        Me._lvwLookupData_ColumnHeader_3.Tag = ""
        Me._lvwLookupData_ColumnHeader_3.Text = "Value"
        Me._lvwLookupData_ColumnHeader_3.Width = 97
        '
        '_lvwLookupData_ColumnHeader_4
        '
        Me._lvwLookupData_ColumnHeader_4.Tag = ""
        Me._lvwLookupData_ColumnHeader_4.Text = "Type"
        Me._lvwLookupData_ColumnHeader_4.Width = 97
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "LookupData")
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(528, 217)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(65, 22)
        Me.cmdDelete.TabIndex = 2
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(368, 217)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 0
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(448, 217)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 1
        Me.cmdAdd.Text = "&New"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(616, 323)
        Me.Controls.Add(Me.tabLookupDetail)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.tabMaintab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Form1"
        Me.tabLookupDetail.ResumeLayout(False)
        Me._tabLookupDetail_TabPage0.ResumeLayout(False)
        Me._tabLookupDetail_TabPage0.PerformLayout()
        Me.tabMaintab.ResumeLayout(False)
        Me._tabMaintab_TabPage0.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
	Sub lvwLookupData_InitializeColumnKeys()
		Me._lvwLookupData_ColumnHeader_1.Name = ""
		Me._lvwLookupData_ColumnHeader_2.Name = ""
		Me._lvwLookupData_ColumnHeader_3.Name = ""
		Me._lvwLookupData_ColumnHeader_4.Name = ""
	End Sub
#End Region 
End Class