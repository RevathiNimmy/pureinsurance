<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwProperties_InitializeColumnKeys()
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
	Public WithEvents cboGISListId As System.Windows.Forms.ComboBox
	Public WithEvents cmdList As System.Windows.Forms.Button
	Public dlgPrintPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdPrint As System.Windows.Forms.Button
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblObjects As System.Windows.Forms.Label
	Public WithEvents lblProperties As System.Windows.Forms.Label
	Public WithEvents lblDataModel As System.Windows.Forms.Label
	Public WithEvents imgSplitterV As System.Windows.Forms.PictureBox
	Public WithEvents lblBOObjects As System.Windows.Forms.Label
	Public WithEvents imgSplitterH As System.Windows.Forms.PictureBox
	Public WithEvents lblStatus As System.Windows.Forms.Label
	Public WithEvents pnlStatus As System.Windows.Forms.Label
	Public WithEvents lblAccessibleViaSAM As System.Windows.Forms.Label
	Public WithEvents tvwObjects As System.Windows.Forms.TreeView
	Public WithEvents cmdObjectAdd As System.Windows.Forms.Button
	Public WithEvents cmdObjectEdit As System.Windows.Forms.Button
	Private WithEvents _lvwProperties_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwProperties_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwProperties_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwProperties_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwProperties As System.Windows.Forms.ListView
	Public WithEvents cmdPropertyAdd As System.Windows.Forms.Button
	Public WithEvents cmdPropertyEdit As System.Windows.Forms.Button
	Public WithEvents txtDataModel As System.Windows.Forms.TextBox
	Public WithEvents picSplitterV As System.Windows.Forms.PictureBox
	Public WithEvents picSplitterH As System.Windows.Forms.PictureBox
	Public WithEvents chkShowKeys As System.Windows.Forms.CheckBox
	Public WithEvents cmdPropertyDelete As System.Windows.Forms.Button
	Public WithEvents chkAccessibleViaSAM As System.Windows.Forms.CheckBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents ImageList1 As System.Windows.Forms.ImageList
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cboGISListId = New System.Windows.Forms.ComboBox
        Me.cmdList = New System.Windows.Forms.Button
        Me.dlgPrintPrint = New System.Windows.Forms.PrintDialog
        Me.cmdPrint = New System.Windows.Forms.Button
        Me.cmdNavigate = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblObjects = New System.Windows.Forms.Label
        Me.lblProperties = New System.Windows.Forms.Label
        Me.lblDataModel = New System.Windows.Forms.Label
        Me.imgSplitterV = New System.Windows.Forms.PictureBox
        Me.lblBOObjects = New System.Windows.Forms.Label
        Me.imgSplitterH = New System.Windows.Forms.PictureBox
        Me.lblStatus = New System.Windows.Forms.Label
        Me.pnlStatus = New System.Windows.Forms.Label
        Me.lblAccessibleViaSAM = New System.Windows.Forms.Label
        Me.tvwObjects = New System.Windows.Forms.TreeView
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdObjectAdd = New System.Windows.Forms.Button
        Me.cmdObjectEdit = New System.Windows.Forms.Button
        Me.lvwProperties = New System.Windows.Forms.ListView
        Me._lvwProperties_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwProperties_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwProperties_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwProperties_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me.cmdPropertyAdd = New System.Windows.Forms.Button
        Me.cmdPropertyEdit = New System.Windows.Forms.Button
        Me.txtDataModel = New System.Windows.Forms.TextBox
        Me.picSplitterV = New System.Windows.Forms.PictureBox
        Me.picSplitterH = New System.Windows.Forms.PictureBox
        Me.chkShowKeys = New System.Windows.Forms.CheckBox
        Me.cmdPropertyDelete = New System.Windows.Forms.Button
        Me.chkAccessibleViaSAM = New System.Windows.Forms.CheckBox
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.HelpProvider1 = New System.Windows.Forms.HelpProvider
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.imgSplitterV, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.imgSplitterH, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picSplitterV, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picSplitterH, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cboGISListId
        '
        Me.cboGISListId.BackColor = System.Drawing.SystemColors.Window
        Me.cboGISListId.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboGISListId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboGISListId.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboGISListId.Location = New System.Drawing.Point(185, 561)
        Me.cboGISListId.Name = "cboGISListId"
        Me.cboGISListId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboGISListId.Size = New System.Drawing.Size(110, 21)
        Me.cboGISListId.TabIndex = 11
        Me.cboGISListId.Text = "Combo1"
        Me.cboGISListId.Visible = False
        '
        'cmdList
        '
        Me.cmdList.BackColor = System.Drawing.SystemColors.Control
        Me.cmdList.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdList.ForeColor = System.Drawing.SystemColors.ControlText
        Me.HelpProvider1.SetHelpNavigator(Me.cmdList, System.Windows.Forms.HelpNavigator.Index)
        Me.cmdList.Location = New System.Drawing.Point(523, 560)
        Me.cmdList.Name = "cmdList"
        Me.cmdList.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HelpProvider1.SetShowHelp(Me.cmdList, True)
        Me.cmdList.Size = New System.Drawing.Size(73, 22)
        Me.cmdList.TabIndex = 12
        Me.cmdList.Text = "&List"
        Me.cmdList.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdList.UseVisualStyleBackColor = False
        '
        'cmdPrint
        '
        Me.cmdPrint.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPrint.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPrint.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPrint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPrint.Location = New System.Drawing.Point(603, 560)
        Me.cmdPrint.Name = "cmdPrint"
        Me.cmdPrint.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPrint.Size = New System.Drawing.Size(73, 22)
        Me.cmdPrint.TabIndex = 13
        Me.cmdPrint.Text = "&Print"
        Me.cmdPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPrint.UseVisualStyleBackColor = False
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(8, 560)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 10
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        Me.cmdNavigate.Visible = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(840, 560)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 16
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
        Me.cmdCancel.Location = New System.Drawing.Point(761, 560)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 15
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
        Me.cmdOK.Location = New System.Drawing.Point(682, 560)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 14
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(180, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(7, 7)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(909, 549)
        Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.tabMainTab.TabIndex = 17
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblObjects)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblProperties)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDataModel)
        Me._tabMainTab_TabPage0.Controls.Add(Me.imgSplitterV)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblBOObjects)
        Me._tabMainTab_TabPage0.Controls.Add(Me.imgSplitterH)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pnlStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblAccessibleViaSAM)
        Me._tabMainTab_TabPage0.Controls.Add(Me.tvwObjects)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdObjectAdd)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdObjectEdit)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwProperties)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdPropertyAdd)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdPropertyEdit)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDataModel)
        Me._tabMainTab_TabPage0.Controls.Add(Me.picSplitterV)
        Me._tabMainTab_TabPage0.Controls.Add(Me.picSplitterH)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkShowKeys)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdPropertyDelete)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkAccessibleViaSAM)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(901, 523)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "&1 - General"
        '
        'lblObjects
        '
        Me.lblObjects.BackColor = System.Drawing.SystemColors.Control
        Me.lblObjects.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblObjects.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblObjects.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblObjects.Location = New System.Drawing.Point(16, 52)
        Me.lblObjects.Name = "lblObjects"
        Me.lblObjects.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblObjects.Size = New System.Drawing.Size(135, 17)
        Me.lblObjects.TabIndex = 18
        Me.lblObjects.Text = "Risk Objects / tables"
        '
        'lblProperties
        '
        Me.lblProperties.BackColor = System.Drawing.SystemColors.Control
        Me.lblProperties.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProperties.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProperties.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProperties.Location = New System.Drawing.Point(284, 52)
        Me.lblProperties.Name = "lblProperties"
        Me.lblProperties.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProperties.Size = New System.Drawing.Size(141, 17)
        Me.lblProperties.TabIndex = 19
        Me.lblProperties.Text = "Properties / columns"
        '
        'lblDataModel
        '
        Me.lblDataModel.BackColor = System.Drawing.SystemColors.Control
        Me.lblDataModel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDataModel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDataModel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDataModel.Location = New System.Drawing.Point(16, 15)
        Me.lblDataModel.Name = "lblDataModel"
        Me.lblDataModel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDataModel.Size = New System.Drawing.Size(91, 17)
        Me.lblDataModel.TabIndex = 20
        Me.lblDataModel.Text = "Data Model:"
        '
        'imgSplitterV
        '
        Me.imgSplitterV.Cursor = System.Windows.Forms.Cursors.SizeWE
        Me.imgSplitterV.Location = New System.Drawing.Point(274, 68)
        Me.imgSplitterV.Name = "imgSplitterV"
        Me.imgSplitterV.Size = New System.Drawing.Size(10, 416)
        Me.imgSplitterV.TabIndex = 21
        Me.imgSplitterV.TabStop = False
        '
        'lblBOObjects
        '
        Me.lblBOObjects.BackColor = System.Drawing.SystemColors.Control
        Me.lblBOObjects.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBOObjects.Enabled = False
        Me.lblBOObjects.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBOObjects.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBOObjects.Location = New System.Drawing.Point(496, 12)
        Me.lblBOObjects.Name = "lblBOObjects"
        Me.lblBOObjects.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBOObjects.Size = New System.Drawing.Size(167, 15)
        Me.lblBOObjects.TabIndex = 23
        Me.lblBOObjects.Text = "Back Office Objects / tables"
        Me.lblBOObjects.Visible = False
        '
        'imgSplitterH
        '
        Me.imgSplitterH.Cursor = System.Windows.Forms.Cursors.SizeNS
        Me.imgSplitterH.Enabled = False
        Me.imgSplitterH.Location = New System.Drawing.Point(495, 14)
        Me.imgSplitterH.Name = "imgSplitterH"
        Me.imgSplitterH.Size = New System.Drawing.Size(259, 24)
        Me.imgSplitterH.TabIndex = 24
        Me.imgSplitterH.TabStop = False
        Me.imgSplitterH.Visible = False
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatus.Location = New System.Drawing.Point(526, 493)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatus.Size = New System.Drawing.Size(42, 18)
        Me.lblStatus.TabIndex = 25
        Me.lblStatus.Text = "Status"
        '
        'pnlStatus
        '
        Me.pnlStatus.BackColor = System.Drawing.SystemColors.Control
        Me.pnlStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.pnlStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlStatus.Location = New System.Drawing.Point(574, 492)
        Me.pnlStatus.Name = "pnlStatus"
        Me.pnlStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlStatus.Size = New System.Drawing.Size(312, 22)
        Me.pnlStatus.TabIndex = 24
        '
        'lblAccessibleViaSAM
        '
        Me.lblAccessibleViaSAM.AutoSize = True
        Me.lblAccessibleViaSAM.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccessibleViaSAM.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccessibleViaSAM.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccessibleViaSAM.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccessibleViaSAM.Location = New System.Drawing.Point(320, 15)
        Me.lblAccessibleViaSAM.Name = "lblAccessibleViaSAM"
        Me.lblAccessibleViaSAM.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccessibleViaSAM.Size = New System.Drawing.Size(105, 13)
        Me.lblAccessibleViaSAM.TabIndex = 26
        Me.lblAccessibleViaSAM.Text = "Accessible Via SAM:"
        Me.lblAccessibleViaSAM.Visible = False
        '
        'tvwObjects
        '
        Me.tvwObjects.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tvwObjects.HideSelection = False
        Me.tvwObjects.ImageIndex = 2
        Me.tvwObjects.ImageList = Me.ImageList1
        Me.tvwObjects.Indent = 16
        Me.tvwObjects.Location = New System.Drawing.Point(16, 69)
        Me.tvwObjects.Margin = New System.Windows.Forms.Padding(0, 3, 3, 3)
        Me.tvwObjects.Name = "tvwObjects"
        Me.tvwObjects.SelectedImageIndex = 0
        Me.tvwObjects.Size = New System.Drawing.Size(259, 415)
        Me.tvwObjects.TabIndex = 3
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "Closed")
        Me.ImageList1.Images.SetKeyName(1, "Open")
        Me.ImageList1.Images.SetKeyName(2, "none.gif")
        '
        'cmdObjectAdd
        '
        Me.cmdObjectAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdObjectAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdObjectAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdObjectAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdObjectAdd.Location = New System.Drawing.Point(16, 490)
        Me.cmdObjectAdd.Name = "cmdObjectAdd"
        Me.cmdObjectAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdObjectAdd.Size = New System.Drawing.Size(73, 23)
        Me.cmdObjectAdd.TabIndex = 5
        Me.cmdObjectAdd.Text = "Add"
        Me.cmdObjectAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdObjectAdd.UseVisualStyleBackColor = False
        '
        'cmdObjectEdit
        '
        Me.cmdObjectEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdObjectEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdObjectEdit.Enabled = False
        Me.cmdObjectEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdObjectEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdObjectEdit.Location = New System.Drawing.Point(94, 490)
        Me.cmdObjectEdit.Name = "cmdObjectEdit"
        Me.cmdObjectEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdObjectEdit.Size = New System.Drawing.Size(73, 23)
        Me.cmdObjectEdit.TabIndex = 6
        Me.cmdObjectEdit.Text = "Edit"
        Me.cmdObjectEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdObjectEdit.UseVisualStyleBackColor = False
        '
        'lvwProperties
        '
        Me.lvwProperties.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwProperties, "")
        Me.lvwProperties.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwProperties_ColumnHeader_1, Me._lvwProperties_ColumnHeader_2, Me._lvwProperties_ColumnHeader_3, Me._lvwProperties_ColumnHeader_4})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwProperties, False)
        Me.lvwProperties.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwProperties.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me.lvwProperties, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwProperties, "")
        Me.lvwProperties.Location = New System.Drawing.Point(280, 68)
        Me.lvwProperties.Name = "lvwProperties"
        Me.lvwProperties.Size = New System.Drawing.Size(609, 417)
        Me.listViewHelper1.SetSmallIcons(Me.lvwProperties, "")
        Me.listViewHelper1.SetSorted(Me.lvwProperties, False)
        Me.listViewHelper1.SetSortKey(Me.lvwProperties, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwProperties, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwProperties.TabIndex = 4
        Me.lvwProperties.UseCompatibleStateImageBehavior = False
        Me.lvwProperties.View = System.Windows.Forms.View.Details
        '
        '_lvwProperties_ColumnHeader_1
        '
        Me._lvwProperties_ColumnHeader_1.Tag = ""
        Me._lvwProperties_ColumnHeader_1.Text = "Property"
        Me._lvwProperties_ColumnHeader_1.Width = 134
        '
        '_lvwProperties_ColumnHeader_2
        '
        Me._lvwProperties_ColumnHeader_2.Tag = ""
        Me._lvwProperties_ColumnHeader_2.Text = "Column"
        Me._lvwProperties_ColumnHeader_2.Width = 134
        '
        '_lvwProperties_ColumnHeader_3
        '
        Me._lvwProperties_ColumnHeader_3.Tag = ""
        Me._lvwProperties_ColumnHeader_3.Text = ""
        Me._lvwProperties_ColumnHeader_3.Width = 134
        '
        '_lvwProperties_ColumnHeader_4
        '
        Me._lvwProperties_ColumnHeader_4.Tag = ""
        Me._lvwProperties_ColumnHeader_4.Text = ""
        Me._lvwProperties_ColumnHeader_4.Width = 134
        '
        'cmdPropertyAdd
        '
        Me.cmdPropertyAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPropertyAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPropertyAdd.Enabled = False
        Me.cmdPropertyAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPropertyAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPropertyAdd.Location = New System.Drawing.Point(282, 490)
        Me.cmdPropertyAdd.Name = "cmdPropertyAdd"
        Me.cmdPropertyAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPropertyAdd.Size = New System.Drawing.Size(73, 23)
        Me.cmdPropertyAdd.TabIndex = 7
        Me.cmdPropertyAdd.Text = "Add"
        Me.cmdPropertyAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPropertyAdd.UseVisualStyleBackColor = False
        '
        'cmdPropertyEdit
        '
        Me.cmdPropertyEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPropertyEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPropertyEdit.Enabled = False
        Me.cmdPropertyEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPropertyEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPropertyEdit.Location = New System.Drawing.Point(360, 490)
        Me.cmdPropertyEdit.Name = "cmdPropertyEdit"
        Me.cmdPropertyEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPropertyEdit.Size = New System.Drawing.Size(73, 23)
        Me.cmdPropertyEdit.TabIndex = 8
        Me.cmdPropertyEdit.Text = "Edit"
        Me.cmdPropertyEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPropertyEdit.UseVisualStyleBackColor = False
        '
        'txtDataModel
        '
        Me.txtDataModel.AcceptsReturn = True
        Me.txtDataModel.BackColor = System.Drawing.SystemColors.Window
        Me.txtDataModel.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDataModel.Enabled = False
        Me.txtDataModel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDataModel.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDataModel.Location = New System.Drawing.Point(113, 12)
        Me.txtDataModel.MaxLength = 0
        Me.txtDataModel.Name = "txtDataModel"
        Me.txtDataModel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDataModel.Size = New System.Drawing.Size(185, 20)
        Me.txtDataModel.TabIndex = 0
        '
        'picSplitterV
        '
        Me.picSplitterV.BackColor = System.Drawing.Color.Gray
        Me.picSplitterV.Cursor = System.Windows.Forms.Cursors.Default
        Me.picSplitterV.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.picSplitterV.Location = New System.Drawing.Point(276, 69)
        Me.picSplitterV.Name = "picSplitterV"
        Me.picSplitterV.Size = New System.Drawing.Size(5, 416)
        Me.picSplitterV.TabIndex = 21
        Me.picSplitterV.TabStop = False
        Me.picSplitterV.Visible = False
        '
        'picSplitterH
        '
        Me.picSplitterH.BackColor = System.Drawing.Color.Gray
        Me.picSplitterH.Cursor = System.Windows.Forms.Cursors.Default
        Me.picSplitterH.Enabled = False
        Me.picSplitterH.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.picSplitterH.Location = New System.Drawing.Point(489, 30)
        Me.picSplitterH.Name = "picSplitterH"
        Me.picSplitterH.Size = New System.Drawing.Size(259, 24)
        Me.picSplitterH.TabIndex = 22
        Me.picSplitterH.TabStop = False
        Me.picSplitterH.Visible = False
        '
        'chkShowKeys
        '
        Me.chkShowKeys.BackColor = System.Drawing.SystemColors.Control
        Me.chkShowKeys.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkShowKeys.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkShowKeys.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShowKeys.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkShowKeys.Location = New System.Drawing.Point(757, 47)
        Me.chkShowKeys.Name = "chkShowKeys"
        Me.chkShowKeys.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkShowKeys.Size = New System.Drawing.Size(129, 18)
        Me.chkShowKeys.TabIndex = 2
        Me.chkShowKeys.Text = "Display Keys"
        Me.chkShowKeys.UseVisualStyleBackColor = False
        '
        'cmdPropertyDelete
        '
        Me.cmdPropertyDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPropertyDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPropertyDelete.Enabled = False
        Me.cmdPropertyDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPropertyDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPropertyDelete.Location = New System.Drawing.Point(438, 490)
        Me.cmdPropertyDelete.Name = "cmdPropertyDelete"
        Me.cmdPropertyDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPropertyDelete.Size = New System.Drawing.Size(73, 23)
        Me.cmdPropertyDelete.TabIndex = 9
        Me.cmdPropertyDelete.Text = "Delete"
        Me.cmdPropertyDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPropertyDelete.UseVisualStyleBackColor = False
        '
        'chkAccessibleViaSAM
        '
        Me.chkAccessibleViaSAM.BackColor = System.Drawing.SystemColors.Control
        Me.chkAccessibleViaSAM.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAccessibleViaSAM.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAccessibleViaSAM.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAccessibleViaSAM.Location = New System.Drawing.Point(448, 12)
        Me.chkAccessibleViaSAM.Name = "chkAccessibleViaSAM"
        Me.chkAccessibleViaSAM.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAccessibleViaSAM.Size = New System.Drawing.Size(33, 17)
        Me.chkAccessibleViaSAM.TabIndex = 1
        Me.chkAccessibleViaSAM.UseVisualStyleBackColor = False
        Me.chkAccessibleViaSAM.Visible = False
        '
        'HelpProvider1
        '
        Me.HelpProvider1.HelpNamespace = "\\10.10.20.33\pm\Orion\Common\Help\Orion.hlp"
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(921, 587)
        Me.Controls.Add(Me.cboGISListId)
        Me.Controls.Add(Me.cmdList)
        Me.Controls.Add(Me.cmdPrint)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Maintain Data Dictionary"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        CType(Me.imgSplitterV, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.imgSplitterH, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picSplitterV, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picSplitterH, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
	Sub lvwProperties_InitializeColumnKeys()
		Me._lvwProperties_ColumnHeader_1.Name = ""
		Me._lvwProperties_ColumnHeader_2.Name = ""
		Me._lvwProperties_ColumnHeader_3.Name = ""
		Me._lvwProperties_ColumnHeader_4.Name = ""
    End Sub
    Friend WithEvents HelpProvider1 As System.Windows.Forms.HelpProvider
#End Region 
End Class