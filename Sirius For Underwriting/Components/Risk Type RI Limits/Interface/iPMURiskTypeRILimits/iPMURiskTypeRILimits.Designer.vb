<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
        isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		lvwRiskTypeRILimits_InitializeColumnKeys()
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
    Public WithEvents lblGISObject As System.Windows.Forms.Label
    Public WithEvents lblGISProperty As System.Windows.Forms.Label
    Public WithEvents cmdDetailOK As System.Windows.Forms.Button
    Public WithEvents cmdDetailCancel As System.Windows.Forms.Button
    Public WithEvents cboGISObject As System.Windows.Forms.ComboBox
    Public WithEvents cboGISProperty As System.Windows.Forms.ComboBox
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.tabDetailTab = New System.Windows.Forms.TabControl()
        Me._tabDetailTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.lblGISObject = New System.Windows.Forms.Label()
        Me.lblGISProperty = New System.Windows.Forms.Label()
        Me.cmdDetailOK = New System.Windows.Forms.Button()
        Me.cmdDetailCancel = New System.Windows.Forms.Button()
        Me.cboGISObject = New System.Windows.Forms.ComboBox()
        Me.cboGISProperty = New System.Windows.Forms.ComboBox()
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog()
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog()
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog()
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.txtExpiryDate = New System.Windows.Forms.TextBox()
        Me.txtEffectiveDate = New System.Windows.Forms.TextBox()
        Me.lblExpiryDate = New System.Windows.Forms.Label()
        Me.lblEffectiveDate = New System.Windows.Forms.Label()
        Me.txtDescription = New System.Windows.Forms.RichTextBox()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.lblRiskType = New System.Windows.Forms.Label()
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.lvwRiskTypeRILimits = New System.Windows.Forms.ListView()
        Me._lvwRiskTypeRILimits_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRiskTypeRILimits_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.pnlRiskType = New System.Windows.Forms.Panel()
        Me.lblpRiskType = New System.Windows.Forms.Label()
        Me.cmdLimits = New System.Windows.Forms.Button()
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me.tabDetailTab.SuspendLayout()
        Me._tabDetailTab_TabPage0.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.pnlRiskType.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(536, 346)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 9
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
        Me.cmdCancel.Location = New System.Drawing.Point(456, 346)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 8
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
        Me.cmdOK.Location = New System.Drawing.Point(368, 346)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 7
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "FindImage")
        '
        'tabDetailTab
        '
        Me.tabDetailTab.Controls.Add(Me._tabDetailTab_TabPage0)
        Me.tabDetailTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabDetailTab.ItemSize = New System.Drawing.Size(600, 18)
        Me.tabDetailTab.Location = New System.Drawing.Point(8, 675)
        Me.tabDetailTab.Multiline = True
        Me.tabDetailTab.Name = "tabDetailTab"
        Me.tabDetailTab.SelectedIndex = 0
        Me.tabDetailTab.Size = New System.Drawing.Size(605, 285)
        Me.tabDetailTab.TabIndex = 10
        '
        '_tabDetailTab_TabPage0
        '
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblGISObject)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblGISProperty)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdDetailOK)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdDetailCancel)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cboGISObject)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cboGISProperty)
        Me._tabDetailTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabDetailTab_TabPage0.Name = "_tabDetailTab_TabPage0"
        Me._tabDetailTab_TabPage0.Size = New System.Drawing.Size(597, 259)
        Me._tabDetailTab_TabPage0.TabIndex = 0
        Me._tabDetailTab_TabPage0.Text = "&2 - GIS Property"
        '
        'lblGISObject
        '
        Me.lblGISObject.BackColor = System.Drawing.SystemColors.Control
        Me.lblGISObject.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGISObject.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGISObject.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGISObject.Location = New System.Drawing.Point(24, 31)
        Me.lblGISObject.Name = "lblGISObject"
        Me.lblGISObject.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGISObject.Size = New System.Drawing.Size(90, 17)
        Me.lblGISObject.TabIndex = 14
        Me.lblGISObject.Text = "Object:"
        '
        'lblGISProperty
        '
        Me.lblGISProperty.BackColor = System.Drawing.SystemColors.Control
        Me.lblGISProperty.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGISProperty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGISProperty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGISProperty.Location = New System.Drawing.Point(24, 71)
        Me.lblGISProperty.Name = "lblGISProperty"
        Me.lblGISProperty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGISProperty.Size = New System.Drawing.Size(90, 17)
        Me.lblGISProperty.TabIndex = 15
        Me.lblGISProperty.Text = "Property:"
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
        Me.cmdDetailOK.TabIndex = 5
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
        Me.cmdDetailCancel.TabIndex = 6
        Me.cmdDetailCancel.Text = "&Cancel"
        Me.cmdDetailCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDetailCancel.UseVisualStyleBackColor = False
        '
        'cboGISObject
        '
        Me.cboGISObject.BackColor = System.Drawing.SystemColors.Window
        Me.cboGISObject.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboGISObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboGISObject.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboGISObject.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboGISObject.Location = New System.Drawing.Point(120, 28)
        Me.cboGISObject.Name = "cboGISObject"
        Me.cboGISObject.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboGISObject.Size = New System.Drawing.Size(185, 21)
        Me.cboGISObject.TabIndex = 3
        '
        'cboGISProperty
        '
        Me.cboGISProperty.BackColor = System.Drawing.SystemColors.Window
        Me.cboGISProperty.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboGISProperty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboGISProperty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboGISProperty.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboGISProperty.Location = New System.Drawing.Point(120, 68)
        Me.cboGISProperty.Name = "cboGISProperty"
        Me.cboGISProperty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboGISProperty.Size = New System.Drawing.Size(185, 21)
        Me.cboGISProperty.TabIndex = 4
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtExpiryDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtEffectiveDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblExpiryDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblEffectiveDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblRiskType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdDelete)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAdd)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwRiskTypeRILimits)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pnlRiskType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdLimits)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(597, 306)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "&1 - RI Limits"
        '
        'txtExpiryDate
        '
        Me.txtExpiryDate.Location = New System.Drawing.Point(409, 55)
        Me.txtExpiryDate.Name = "txtExpiryDate"
        Me.txtExpiryDate.Size = New System.Drawing.Size(130, 21)
        Me.txtExpiryDate.TabIndex = 22
        '
        'txtEffectiveDate
        '
        Me.txtEffectiveDate.Location = New System.Drawing.Point(409, 17)
        Me.txtEffectiveDate.Name = "txtEffectiveDate"
        Me.txtEffectiveDate.Size = New System.Drawing.Size(130, 21)
        Me.txtEffectiveDate.TabIndex = 21
        '
        'lblExpiryDate
        '
        Me.lblExpiryDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblExpiryDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExpiryDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExpiryDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExpiryDate.Location = New System.Drawing.Point(314, 59)
        Me.lblExpiryDate.Name = "lblExpiryDate"
        Me.lblExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExpiryDate.Size = New System.Drawing.Size(89, 17)
        Me.lblExpiryDate.TabIndex = 20
        Me.lblExpiryDate.Text = "Expiry Date:"
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(314, 19)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(89, 17)
        Me.lblEffectiveDate.TabIndex = 19
        Me.lblEffectiveDate.Text = "Effective Date:"
        '
        'txtDescription
        '
        Me.txtDescription.Location = New System.Drawing.Point(100, 56)
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.Size = New System.Drawing.Size(114, 41)
        Me.txtDescription.TabIndex = 18
        Me.txtDescription.Text = ""
        '
        'lblDescription
        '
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(8, 59)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(89, 17)
        Me.lblDescription.TabIndex = 17
        Me.lblDescription.Text = "Description:"
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
        Me.lblRiskType.TabIndex = 12
        Me.lblRiskType.Text = "Risk Type:"
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Enabled = False
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(520, 281)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(65, 22)
        Me.cmdDelete.TabIndex = 2
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(440, 281)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 1
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'lvwRiskTypeRILimits
        '
        Me.lvwRiskTypeRILimits.BackColor = System.Drawing.SystemColors.Window
        Me.lvwRiskTypeRILimits.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwRiskTypeRILimits.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwRiskTypeRILimits_ColumnHeader_1, Me._lvwRiskTypeRILimits_ColumnHeader_2})
        Me.lvwRiskTypeRILimits.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwRiskTypeRILimits.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwRiskTypeRILimits.LargeImageList = Me.ImageList1
        Me.lvwRiskTypeRILimits.Location = New System.Drawing.Point(8, 105)
        Me.lvwRiskTypeRILimits.Name = "lvwRiskTypeRILimits"
        Me.lvwRiskTypeRILimits.Size = New System.Drawing.Size(577, 169)
        Me.lvwRiskTypeRILimits.SmallImageList = Me.ImageList1
        Me.lvwRiskTypeRILimits.TabIndex = 0
        Me.lvwRiskTypeRILimits.UseCompatibleStateImageBehavior = False
        Me.lvwRiskTypeRILimits.View = System.Windows.Forms.View.Details
        '
        '_lvwRiskTypeRILimits_ColumnHeader_1
        '
        Me._lvwRiskTypeRILimits_ColumnHeader_1.Tag = ""
        Me._lvwRiskTypeRILimits_ColumnHeader_1.Text = "Object"
        Me._lvwRiskTypeRILimits_ColumnHeader_1.Width = 201
        '
        '_lvwRiskTypeRILimits_ColumnHeader_2
        '
        Me._lvwRiskTypeRILimits_ColumnHeader_2.Tag = ""
        Me._lvwRiskTypeRILimits_ColumnHeader_2.Text = "Property"
        Me._lvwRiskTypeRILimits_ColumnHeader_2.Width = 97
        '
        'pnlRiskType
        '
        Me.pnlRiskType.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.pnlRiskType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlRiskType.Controls.Add(Me.lblpRiskType)
        Me.pnlRiskType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlRiskType.Location = New System.Drawing.Point(100, 20)
        Me.pnlRiskType.Name = "pnlRiskType"
        Me.pnlRiskType.Size = New System.Drawing.Size(114, 18)
        Me.pnlRiskType.TabIndex = 13
        '
        'lblpRiskType
        '
        Me.lblpRiskType.AutoSize = True
        Me.lblpRiskType.Location = New System.Drawing.Point(2, 1)
        Me.lblpRiskType.Name = "lblpRiskType"
        Me.lblpRiskType.Size = New System.Drawing.Size(0, 13)
        Me.lblpRiskType.TabIndex = 0
        '
        'cmdLimits
        '
        Me.cmdLimits.BackColor = System.Drawing.SystemColors.Control
        Me.cmdLimits.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdLimits.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdLimits.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdLimits.Location = New System.Drawing.Point(8, 281)
        Me.cmdLimits.Name = "cmdLimits"
        Me.cmdLimits.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdLimits.Size = New System.Drawing.Size(73, 22)
        Me.cmdLimits.TabIndex = 16
        Me.cmdLimits.Text = "&Limits"
        Me.cmdLimits.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdLimits.UseVisualStyleBackColor = False
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
        Me.tabMainTab.Size = New System.Drawing.Size(605, 332)
        Me.tabMainTab.TabIndex = 11
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(619, 380)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabDetailTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "RI Limits"
        Me.tabDetailTab.ResumeLayout(False)
        Me._tabDetailTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.pnlRiskType.ResumeLayout(False)
        Me.pnlRiskType.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Sub lvwRiskTypeRILimits_InitializeColumnKeys()
        Me._lvwRiskTypeRILimits_ColumnHeader_1.Name = ""
        Me._lvwRiskTypeRILimits_ColumnHeader_2.Name = ""
    End Sub
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents lblRiskType As System.Windows.Forms.Label
    Public WithEvents cmdDelete As System.Windows.Forms.Button
    Public WithEvents cmdAdd As System.Windows.Forms.Button
    Public WithEvents lvwRiskTypeRILimits As System.Windows.Forms.ListView
    Private WithEvents _lvwRiskTypeRILimits_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRiskTypeRILimits_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Public WithEvents pnlRiskType As System.Windows.Forms.Panel
    Friend WithEvents lblpRiskType As System.Windows.Forms.Label
    Public WithEvents cmdLimits As System.Windows.Forms.Button
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public WithEvents lblDescription As System.Windows.Forms.Label
    Public WithEvents lblExpiryDate As System.Windows.Forms.Label
    Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
    Friend WithEvents txtDescription As System.Windows.Forms.RichTextBox
    Friend WithEvents txtExpiryDate As Control
    Friend WithEvents txtEffectiveDate As System.Windows.Forms.TextBox
#End Region
End Class