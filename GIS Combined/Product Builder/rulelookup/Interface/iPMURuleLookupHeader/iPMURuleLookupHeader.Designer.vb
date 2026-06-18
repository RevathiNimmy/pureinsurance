<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwLookupHeader_InitializeColumnKeys()
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
	Public WithEvents lblName As System.Windows.Forms.Label
	Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
	Public WithEvents lblDefinition As System.Windows.Forms.Label
	Public WithEvents lblValidConstant As System.Windows.Forms.Label
	Public WithEvents lblDefault As System.Windows.Forms.Label
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents txtName As System.Windows.Forms.TextBox
	Public WithEvents txtEffectiveDate As System.Windows.Forms.TextBox
	Public WithEvents chkLive As System.Windows.Forms.CheckBox
	Public WithEvents txtDefinition As System.Windows.Forms.TextBox
	Public WithEvents txtValidConstant As System.Windows.Forms.TextBox
	Public WithEvents txtDefault As System.Windows.Forms.TextBox
	Private WithEvents _tabHeaderDetail_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabHeaderDetail As System.Windows.Forms.TabControl
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Private WithEvents _lvwLookupHeader_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwLookupHeader_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwLookupHeader_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwLookupHeader_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwLookupHeader_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwLookupHeader As System.Windows.Forms.ListView
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents txtFormatDate As System.Windows.Forms.TextBox
	Public WithEvents cmdDetail As System.Windows.Forms.Button
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
        Me.tabHeaderDetail = New System.Windows.Forms.TabControl
        Me._tabHeaderDetail_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblName = New System.Windows.Forms.Label
        Me.lblEffectiveDate = New System.Windows.Forms.Label
        Me.lblDefinition = New System.Windows.Forms.Label
        Me.lblValidConstant = New System.Windows.Forms.Label
        Me.lblDefault = New System.Windows.Forms.Label
        Me.txtName = New System.Windows.Forms.TextBox
        Me.txtEffectiveDate = New System.Windows.Forms.TextBox
        Me.chkLive = New System.Windows.Forms.CheckBox
        Me.txtDefinition = New System.Windows.Forms.TextBox
        Me.txtValidConstant = New System.Windows.Forms.TextBox
        Me.txtDefault = New System.Windows.Forms.TextBox
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
        Me.lvwLookupHeader = New System.Windows.Forms.ListView
        Me._lvwLookupHeader_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwLookupHeader_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwLookupHeader_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwLookupHeader_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwLookupHeader_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.txtFormatDate = New System.Windows.Forms.TextBox
        Me.cmdDetail = New System.Windows.Forms.Button

        Me.tabHeaderDetail.SuspendLayout()
        Me._tabHeaderDetail_TabPage0.SuspendLayout()
        Me.tabMaintab.SuspendLayout()
        Me._tabMaintab_TabPage0.SuspendLayout()

        Me.SuspendLayout()
        '
        'tabHeaderDetail
        '
        Me.tabHeaderDetail.Controls.Add(Me._tabHeaderDetail_TabPage0)
        Me.tabHeaderDetail.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabHeaderDetail.ItemSize = New System.Drawing.Size(600, 18)
        Me.tabHeaderDetail.Location = New System.Drawing.Point(-245, 287)
        Me.tabHeaderDetail.Multiline = True
        Me.tabHeaderDetail.Name = "tabHeaderDetail"
        Me.tabHeaderDetail.SelectedIndex = 0
        Me.tabHeaderDetail.Size = New System.Drawing.Size(615, 279)
        Me.tabHeaderDetail.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.tabHeaderDetail.TabIndex = 9
        '
        '_tabHeaderDetail_TabPage0
        '
        Me._tabHeaderDetail_TabPage0.Controls.Add(Me.lblName)
        Me._tabHeaderDetail_TabPage0.Controls.Add(Me.lblEffectiveDate)
        Me._tabHeaderDetail_TabPage0.Controls.Add(Me.lblDefinition)
        Me._tabHeaderDetail_TabPage0.Controls.Add(Me.lblValidConstant)
        Me._tabHeaderDetail_TabPage0.Controls.Add(Me.lblDefault)
        Me._tabHeaderDetail_TabPage0.Controls.Add(Me.txtName)
        Me._tabHeaderDetail_TabPage0.Controls.Add(Me.txtEffectiveDate)
        Me._tabHeaderDetail_TabPage0.Controls.Add(Me.chkLive)
        Me._tabHeaderDetail_TabPage0.Controls.Add(Me.txtDefinition)
        Me._tabHeaderDetail_TabPage0.Controls.Add(Me.txtValidConstant)
        Me._tabHeaderDetail_TabPage0.Controls.Add(Me.txtDefault)
        Me._tabHeaderDetail_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabHeaderDetail_TabPage0.Name = "_tabHeaderDetail_TabPage0"
        Me._tabHeaderDetail_TabPage0.Size = New System.Drawing.Size(607, 253)
        Me._tabHeaderDetail_TabPage0.TabIndex = 0
        Me._tabHeaderDetail_TabPage0.Text = "1-Header Details"
        '
        'lblName
        '
        Me.lblName.BackColor = System.Drawing.SystemColors.Control
        Me.lblName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblName.Location = New System.Drawing.Point(9, 19)
        Me.lblName.Name = "lblName"
        Me.lblName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblName.Size = New System.Drawing.Size(106, 17)
        Me.lblName.TabIndex = 11
        Me.lblName.Text = "Name"
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(9, 51)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(106, 17)
        Me.lblEffectiveDate.TabIndex = 13
        Me.lblEffectiveDate.Text = "Effective Date"
        '
        'lblDefinition
        '
        Me.lblDefinition.BackColor = System.Drawing.SystemColors.Control
        Me.lblDefinition.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDefinition.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDefinition.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDefinition.Location = New System.Drawing.Point(9, 115)
        Me.lblDefinition.Name = "lblDefinition"
        Me.lblDefinition.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDefinition.Size = New System.Drawing.Size(106, 17)
        Me.lblDefinition.TabIndex = 16
        Me.lblDefinition.Text = "Definition"
        '
        'lblValidConstant
        '
        Me.lblValidConstant.BackColor = System.Drawing.SystemColors.Control
        Me.lblValidConstant.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblValidConstant.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblValidConstant.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblValidConstant.Location = New System.Drawing.Point(9, 159)
        Me.lblValidConstant.Name = "lblValidConstant"
        Me.lblValidConstant.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblValidConstant.Size = New System.Drawing.Size(110, 17)
        Me.lblValidConstant.TabIndex = 18
        Me.lblValidConstant.Text = "Valid Constant"
        '
        'lblDefault
        '
        Me.lblDefault.BackColor = System.Drawing.SystemColors.Control
        Me.lblDefault.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDefault.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDefault.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDefault.Location = New System.Drawing.Point(9, 210)
        Me.lblDefault.Name = "lblDefault"
        Me.lblDefault.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDefault.Size = New System.Drawing.Size(106, 17)
        Me.lblDefault.TabIndex = 20
        Me.lblDefault.Text = "Default"
        '
        'txtName
        '
        Me.txtName.AcceptsReturn = True
        Me.txtName.BackColor = System.Drawing.SystemColors.Window
        Me.txtName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtName.Location = New System.Drawing.Point(120, 16)
        Me.txtName.MaxLength = 0
        Me.txtName.Name = "txtName"
        Me.txtName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtName.Size = New System.Drawing.Size(217, 20)
        Me.txtName.TabIndex = 10
        '
        'txtEffectiveDate
        '
        Me.txtEffectiveDate.AcceptsReturn = True
        Me.txtEffectiveDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtEffectiveDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEffectiveDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEffectiveDate.Location = New System.Drawing.Point(120, 48)
        Me.txtEffectiveDate.MaxLength = 0
        Me.txtEffectiveDate.Name = "txtEffectiveDate"
        Me.txtEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEffectiveDate.Size = New System.Drawing.Size(126, 20)
        Me.txtEffectiveDate.TabIndex = 12
        '
        'chkLive
        '
        Me.chkLive.BackColor = System.Drawing.SystemColors.Control
        Me.chkLive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkLive.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkLive.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkLive.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkLive.Location = New System.Drawing.Point(9, 80)
        Me.chkLive.Name = "chkLive"
        Me.chkLive.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkLive.Size = New System.Drawing.Size(119, 17)
        Me.chkLive.TabIndex = 14
        Me.chkLive.Text = "Live"
        Me.chkLive.UseVisualStyleBackColor = False
        '
        'txtDefinition
        '
        Me.txtDefinition.AcceptsReturn = True
        Me.txtDefinition.BackColor = System.Drawing.SystemColors.Window
        Me.txtDefinition.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDefinition.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDefinition.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDefinition.Location = New System.Drawing.Point(120, 112)
        Me.txtDefinition.MaxLength = 0
        Me.txtDefinition.Multiline = True
        Me.txtDefinition.Name = "txtDefinition"
        Me.txtDefinition.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDefinition.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDefinition.Size = New System.Drawing.Size(217, 33)
        Me.txtDefinition.TabIndex = 15
        '
        'txtValidConstant
        '
        Me.txtValidConstant.AcceptsReturn = True
        Me.txtValidConstant.BackColor = System.Drawing.SystemColors.Window
        Me.txtValidConstant.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtValidConstant.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtValidConstant.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtValidConstant.Location = New System.Drawing.Point(120, 159)
        Me.txtValidConstant.MaxLength = 0
        Me.txtValidConstant.Multiline = True
        Me.txtValidConstant.Name = "txtValidConstant"
        Me.txtValidConstant.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtValidConstant.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtValidConstant.Size = New System.Drawing.Size(217, 33)
        Me.txtValidConstant.TabIndex = 17
        '
        'txtDefault
        '
        Me.txtDefault.AcceptsReturn = True
        Me.txtDefault.BackColor = System.Drawing.SystemColors.Window
        Me.txtDefault.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDefault.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDefault.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDefault.Location = New System.Drawing.Point(119, 207)
        Me.txtDefault.MaxLength = 0
        Me.txtDefault.Name = "txtDefault"
        Me.txtDefault.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDefault.Size = New System.Drawing.Size(113, 20)
        Me.txtDefault.TabIndex = 19
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
        Me.cmdCancel.Location = New System.Drawing.Point(449, 292)
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
        Me.cmdHelp.Location = New System.Drawing.Point(536, 292)
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
        Me._tabMaintab_TabPage0.Controls.Add(Me.lvwLookupHeader)
        Me._tabMaintab_TabPage0.Controls.Add(Me.cmdDelete)
        Me._tabMaintab_TabPage0.Controls.Add(Me.cmdEdit)
        Me._tabMaintab_TabPage0.Controls.Add(Me.cmdAdd)
        Me._tabMaintab_TabPage0.Controls.Add(Me.txtFormatDate)
        Me._tabMaintab_TabPage0.Controls.Add(Me.cmdDetail)
        Me._tabMaintab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMaintab_TabPage0.Name = "_tabMaintab_TabPage0"
        Me._tabMaintab_TabPage0.Size = New System.Drawing.Size(597, 253)
        Me._tabMaintab_TabPage0.TabIndex = 0
        Me._tabMaintab_TabPage0.Text = "1-Lookup Header"
        '
        'lvwLookupHeader
        '
        Me.lvwLookupHeader.BackColor = System.Drawing.SystemColors.Window
        Me.lvwLookupHeader.BorderStyle = System.Windows.Forms.BorderStyle.None

        Me.lvwLookupHeader.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwLookupHeader_ColumnHeader_1, Me._lvwLookupHeader_ColumnHeader_2, Me._lvwLookupHeader_ColumnHeader_3, Me._lvwLookupHeader_ColumnHeader_4, Me._lvwLookupHeader_ColumnHeader_5})

        Me.lvwLookupHeader.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwLookupHeader.ForeColor = System.Drawing.SystemColors.WindowText
     
        Me.lvwLookupHeader.LargeImageList = Me.ImageList1
        Me.lvwLookupHeader.Location = New System.Drawing.Point(8, 12)
        Me.lvwLookupHeader.Name = "lvwLookupHeader"
        Me.lvwLookupHeader.Size = New System.Drawing.Size(584, 201)

        Me.lvwLookupHeader.SmallImageList = Me.ImageList1
     
        Me.lvwLookupHeader.TabIndex = 8
        Me.lvwLookupHeader.UseCompatibleStateImageBehavior = False
        Me.lvwLookupHeader.View = System.Windows.Forms.View.Details
        '
        '_lvwLookupHeader_ColumnHeader_1
        '
        Me._lvwLookupHeader_ColumnHeader_1.Tag = ""
        Me._lvwLookupHeader_ColumnHeader_1.Text = "Name"
        Me._lvwLookupHeader_ColumnHeader_1.Width = 134
        '
        '_lvwLookupHeader_ColumnHeader_2
        '
        Me._lvwLookupHeader_ColumnHeader_2.Tag = ""
        Me._lvwLookupHeader_ColumnHeader_2.Text = "Definition"
        Me._lvwLookupHeader_ColumnHeader_2.Width = 97
        '
        '_lvwLookupHeader_ColumnHeader_3
        '
        Me._lvwLookupHeader_ColumnHeader_3.Tag = ""
        Me._lvwLookupHeader_ColumnHeader_3.Text = "Valid Constant"
        Me._lvwLookupHeader_ColumnHeader_3.Width = 97
        '
        '_lvwLookupHeader_ColumnHeader_4
        '
        Me._lvwLookupHeader_ColumnHeader_4.Tag = ""
        Me._lvwLookupHeader_ColumnHeader_4.Text = "Default Value"
        Me._lvwLookupHeader_ColumnHeader_4.Width = 97
        '
        '_lvwLookupHeader_ColumnHeader_5
        '
        Me._lvwLookupHeader_ColumnHeader_5.Tag = ""
        Me._lvwLookupHeader_ColumnHeader_5.Text = "Effective Date"
        Me._lvwLookupHeader_ColumnHeader_5.Width = 97
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "LookupHeader")
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
        'txtFormatDate
        '
        Me.txtFormatDate.AcceptsReturn = True
        Me.txtFormatDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtFormatDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFormatDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFormatDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFormatDate.Location = New System.Drawing.Point(175, 221)
        Me.txtFormatDate.MaxLength = 0
        Me.txtFormatDate.Name = "txtFormatDate"
        Me.txtFormatDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFormatDate.Size = New System.Drawing.Size(81, 20)
        Me.txtFormatDate.TabIndex = 7
        Me.txtFormatDate.Visible = False
        '
        'cmdDetail
        '
        Me.cmdDetail.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDetail.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDetail.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDetail.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDetail.Location = New System.Drawing.Point(7, 217)
        Me.cmdDetail.Name = "cmdDetail"
        Me.cmdDetail.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDetail.Size = New System.Drawing.Size(73, 22)
        Me.cmdDetail.TabIndex = 21
        Me.cmdDetail.Text = "&Detail"
        Me.cmdDetail.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDetail.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(616, 323)
        Me.Controls.Add(Me.tabHeaderDetail)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.tabMaintab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Form1"
        Me.tabHeaderDetail.ResumeLayout(False)
        Me._tabHeaderDetail_TabPage0.ResumeLayout(False)
        Me._tabHeaderDetail_TabPage0.PerformLayout()
        Me.tabMaintab.ResumeLayout(False)
        Me._tabMaintab_TabPage0.ResumeLayout(False)
        Me._tabMaintab_TabPage0.PerformLayout()

        Me.ResumeLayout(False)

    End Sub
	Sub lvwLookupHeader_InitializeColumnKeys()
		Me._lvwLookupHeader_ColumnHeader_1.Name = ""
		Me._lvwLookupHeader_ColumnHeader_2.Name = ""
		Me._lvwLookupHeader_ColumnHeader_3.Name = ""
		Me._lvwLookupHeader_ColumnHeader_4.Name = ""
		Me._lvwLookupHeader_ColumnHeader_5.Name = ""
	End Sub
#End Region
End Class