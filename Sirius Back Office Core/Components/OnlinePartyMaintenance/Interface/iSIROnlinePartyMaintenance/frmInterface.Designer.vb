<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
        InitializeComponent()
        lvwNoAccess_InitializeColumnKeys()
        lvwAccess_InitializeColumnKeys()
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
	Public WithEvents cmdApply As System.Windows.Forms.Button
	Public WithEvents uctPMResizer1 As PMResizerControl.uctPMResizer
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents imgGroup As System.Windows.Forms.ImageList
	Public WithEvents lblNoAccess As System.Windows.Forms.Label
	Public WithEvents lblAccess As System.Windows.Forms.Label
	Private WithEvents _lvwAccess_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAccess_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAccess_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAccess_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAccess_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwAccess As System.Windows.Forms.ListView
	Private WithEvents _lvwNoAccess_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwNoAccess_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwNoAccess_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwNoAccess_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwNoAccess_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwNoAccess As System.Windows.Forms.ListView
	Public WithEvents cmdDeleteClients As System.Windows.Forms.Button
	Public WithEvents cmdAddClients As System.Windows.Forms.Button
	Public WithEvents cmdAddAllClients As System.Windows.Forms.Button
	Public WithEvents cmdDeleteAllClients As System.Windows.Forms.Button
	Public WithEvents cmdFind As System.Windows.Forms.Button
	Public WithEvents txtPostalCode As System.Windows.Forms.TextBox
	Public WithEvents txtAddress1 As System.Windows.Forms.TextBox
	Public WithEvents cmbType As System.Windows.Forms.ComboBox
	Public WithEvents txtLongName As System.Windows.Forms.TextBox
	Public WithEvents txtShortName As System.Windows.Forms.TextBox
	Public WithEvents lblPostalCode As System.Windows.Forms.Label
	Public WithEvents lblAddress1 As System.Windows.Forms.Label
	Public WithEvents lblType As System.Windows.Forms.Label
	Public WithEvents lblLongName As System.Windows.Forms.Label
	Public WithEvents lblShortName As System.Windows.Forms.Label
	Public WithEvents fraFind As System.Windows.Forms.GroupBox
	Private WithEvents _tabClients_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents tabClients As System.Windows.Forms.TabControl
    'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdApply = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdDeleteClients = New System.Windows.Forms.Button
        Me.cmdAddClients = New System.Windows.Forms.Button
        Me.cmdAddAllClients = New System.Windows.Forms.Button
        Me.cmdDeleteAllClients = New System.Windows.Forms.Button
        Me.uctPMResizer1 = New PMResizerControl.uctPMResizer
        Me.tabClients = New System.Windows.Forms.TabControl
        Me._tabClients_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblNoAccess = New System.Windows.Forms.Label
        Me.lblAccess = New System.Windows.Forms.Label
        Me.lvwAccess = New System.Windows.Forms.ListView
        Me._lvwAccess_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwAccess_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwAccess_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwAccess_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwAccess_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me.imgGroup = New System.Windows.Forms.ImageList(Me.components)
        Me.lvwNoAccess = New System.Windows.Forms.ListView
        Me._lvwNoAccess_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwNoAccess_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwNoAccess_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwNoAccess_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwNoAccess_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me.fraFind = New System.Windows.Forms.GroupBox
        Me.cmdFind = New System.Windows.Forms.Button
        Me.txtPostalCode = New System.Windows.Forms.TextBox
        Me.txtAddress1 = New System.Windows.Forms.TextBox
        Me.cmbType = New System.Windows.Forms.ComboBox
        Me.txtLongName = New System.Windows.Forms.TextBox
        Me.txtShortName = New System.Windows.Forms.TextBox
        Me.lblPostalCode = New System.Windows.Forms.Label
        Me.lblAddress1 = New System.Windows.Forms.Label
        Me.lblType = New System.Windows.Forms.Label
        Me.lblLongName = New System.Windows.Forms.Label
        Me.lblShortName = New System.Windows.Forms.Label
        Me.tabClients.SuspendLayout()
        Me._tabClients_TabPage0.SuspendLayout()
        Me.fraFind.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApply.Enabled = False
        Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Location = New System.Drawing.Point(736, 592)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApply.Size = New System.Drawing.Size(73, 22)
        Me.cmdApply.TabIndex = 14
        Me.cmdApply.Text = "&Apply"
        Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdApply, "Accept Changes and return to previous screen")
        Me.cmdApply.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Location = New System.Drawing.Point(816, 592)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(73, 22)
        Me.cmdExit.TabIndex = 15
        Me.cmdExit.Text = "E&xit"
        Me.cmdExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Cancel changes and return to previous screen")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(656, 592)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 13
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdOK, "Accept Changes and return to previous screen")
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdDeleteClients
        '
        Me.cmdDeleteClients.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteClients.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteClients.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteClients.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteClients.Location = New System.Drawing.Point(408, 372)
        Me.cmdDeleteClients.Name = "cmdDeleteClients"
        Me.cmdDeleteClients.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteClients.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteClients.TabIndex = 11
        Me.cmdDeleteClients.Text = "&<- Clients"
        Me.cmdDeleteClients.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdDeleteClients, "Unselect chosen task groups")
        Me.cmdDeleteClients.UseVisualStyleBackColor = False
        '
        'cmdAddClients
        '
        Me.cmdAddClients.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddClients.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddClients.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddClients.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddClients.Location = New System.Drawing.Point(408, 252)
        Me.cmdAddClients.Name = "cmdAddClients"
        Me.cmdAddClients.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddClients.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddClients.TabIndex = 8
        Me.cmdAddClients.Text = "Clients -&>"
        Me.cmdAddClients.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdAddClients, "Select all available task groups")
        Me.cmdAddClients.UseVisualStyleBackColor = False
        '
        'cmdAddAllClients
        '
        Me.cmdAddAllClients.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddAllClients.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddAllClients.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddAllClients.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddAllClients.Location = New System.Drawing.Point(408, 292)
        Me.cmdAddAllClients.Name = "cmdAddAllClients"
        Me.cmdAddAllClients.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddAllClients.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddAllClients.TabIndex = 9
        Me.cmdAddAllClients.Text = "Clients ->>"
        Me.cmdAddAllClients.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdAddAllClients, "Select all available task groups")
        Me.cmdAddAllClients.UseVisualStyleBackColor = False
        '
        'cmdDeleteAllClients
        '
        Me.cmdDeleteAllClients.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteAllClients.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteAllClients.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteAllClients.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteAllClients.Location = New System.Drawing.Point(408, 412)
        Me.cmdDeleteAllClients.Name = "cmdDeleteAllClients"
        Me.cmdDeleteAllClients.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteAllClients.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteAllClients.TabIndex = 10
        Me.cmdDeleteAllClients.Text = "<<- Clients"
        Me.cmdDeleteAllClients.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdDeleteAllClients, "Unselect all task groups")
        Me.cmdDeleteAllClients.UseVisualStyleBackColor = False
        '
        'uctPMResizer1
        '
        Me.uctPMResizer1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMResizer1.Location = New System.Drawing.Point(28, 587)
        Me.uctPMResizer1.Name = "uctPMResizer1"
        Me.uctPMResizer1.Size = New System.Drawing.Size(32, 30)
        Me.uctPMResizer1.TabIndex = 15
        Me.uctPMResizer1.Visible = False
        '
        'tabClients
        '
        Me.tabClients.Controls.Add(Me._tabClients_TabPage0)
        Me.tabClients.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabClients.ItemSize = New System.Drawing.Size(292, 18)
        Me.tabClients.Location = New System.Drawing.Point(8, 8)
        Me.tabClients.Multiline = True
        Me.tabClients.Name = "tabClients"
        Me.tabClients.SelectedIndex = 0
        Me.tabClients.Size = New System.Drawing.Size(885, 573)
        Me.tabClients.TabIndex = 6
        Me.tabClients.TabStop = False
        '
        '_tabClients_TabPage0
        '
        Me._tabClients_TabPage0.Controls.Add(Me.lblNoAccess)
        Me._tabClients_TabPage0.Controls.Add(Me.lblAccess)
        Me._tabClients_TabPage0.Controls.Add(Me.lvwAccess)
        Me._tabClients_TabPage0.Controls.Add(Me.lvwNoAccess)
        Me._tabClients_TabPage0.Controls.Add(Me.cmdDeleteClients)
        Me._tabClients_TabPage0.Controls.Add(Me.cmdAddClients)
        Me._tabClients_TabPage0.Controls.Add(Me.cmdAddAllClients)
        Me._tabClients_TabPage0.Controls.Add(Me.cmdDeleteAllClients)
        Me._tabClients_TabPage0.Controls.Add(Me.fraFind)
        Me._tabClients_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabClients_TabPage0.Name = "_tabClients_TabPage0"
        Me._tabClients_TabPage0.Size = New System.Drawing.Size(877, 547)
        Me._tabClients_TabPage0.TabIndex = 0
        Me._tabClients_TabPage0.Text = "1 - Clients"
        '
        'lblNoAccess
        '
        Me.lblNoAccess.AutoSize = True
        Me.lblNoAccess.BackColor = System.Drawing.SystemColors.Control
        Me.lblNoAccess.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNoAccess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNoAccess.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNoAccess.Location = New System.Drawing.Point(16, 124)
        Me.lblNoAccess.Name = "lblNoAccess"
        Me.lblNoAccess.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNoAccess.Size = New System.Drawing.Size(149, 13)
        Me.lblNoAccess.TabIndex = 16
        Me.lblNoAccess.Text = "Clients Without Online Access"
        '
        'lblAccess
        '
        Me.lblAccess.AutoSize = True
        Me.lblAccess.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccess.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccess.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccess.Location = New System.Drawing.Point(496, 124)
        Me.lblAccess.Name = "lblAccess"
        Me.lblAccess.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccess.Size = New System.Drawing.Size(134, 13)
        Me.lblAccess.TabIndex = 17
        Me.lblAccess.Text = "Clients With Online Access"
        '
        'lvwAccess
        '
        Me.lvwAccess.BackColor = System.Drawing.SystemColors.Window
        Me.lvwAccess.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwAccess_ColumnHeader_1, Me._lvwAccess_ColumnHeader_2, Me._lvwAccess_ColumnHeader_3, Me._lvwAccess_ColumnHeader_4, Me._lvwAccess_ColumnHeader_5})
        Me.lvwAccess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwAccess.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwAccess.FullRowSelect = True
        Me.lvwAccess.HideSelection = False
        Me.lvwAccess.LargeImageList = Me.imgGroup
        Me.lvwAccess.Location = New System.Drawing.Point(496, 140)
        Me.lvwAccess.Name = "lvwAccess"
        Me.lvwAccess.Size = New System.Drawing.Size(369, 393)
        Me.lvwAccess.SmallImageList = Me.imgGroup
        Me.lvwAccess.TabIndex = 12
        Me.lvwAccess.UseCompatibleStateImageBehavior = False
        Me.lvwAccess.View = System.Windows.Forms.View.Details
        '
        '_lvwAccess_ColumnHeader_1
        '
        Me._lvwAccess_ColumnHeader_1.Tag = ""
        Me._lvwAccess_ColumnHeader_1.Text = "Client Code"
        Me._lvwAccess_ColumnHeader_1.Width = 101
        '
        '_lvwAccess_ColumnHeader_2
        '
        Me._lvwAccess_ColumnHeader_2.Tag = ""
        Me._lvwAccess_ColumnHeader_2.Text = "Name"
        Me._lvwAccess_ColumnHeader_2.Width = 167
        '
        '_lvwAccess_ColumnHeader_3
        '
        Me._lvwAccess_ColumnHeader_3.Tag = ""
        Me._lvwAccess_ColumnHeader_3.Text = "Address Line 1"
        Me._lvwAccess_ColumnHeader_3.Width = 134
        '
        '_lvwAccess_ColumnHeader_4
        '
        Me._lvwAccess_ColumnHeader_4.Tag = ""
        Me._lvwAccess_ColumnHeader_4.Text = "Address Line 2"
        Me._lvwAccess_ColumnHeader_4.Width = 134
        '
        '_lvwAccess_ColumnHeader_5
        '
        Me._lvwAccess_ColumnHeader_5.Tag = ""
        Me._lvwAccess_ColumnHeader_5.Text = "Post Code"
        Me._lvwAccess_ColumnHeader_5.Width = 67
        '
        'imgGroup
        '
        Me.imgGroup.ImageStream = CType(resources.GetObject("imgGroup.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgGroup.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.imgGroup.Images.SetKeyName(0, "FindImage")
        '
        'lvwNoAccess
        '
        Me.lvwNoAccess.BackColor = System.Drawing.SystemColors.Window
        Me.lvwNoAccess.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwNoAccess_ColumnHeader_1, Me._lvwNoAccess_ColumnHeader_2, Me._lvwNoAccess_ColumnHeader_3, Me._lvwNoAccess_ColumnHeader_4, Me._lvwNoAccess_ColumnHeader_5})
        Me.lvwNoAccess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwNoAccess.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwNoAccess.FullRowSelect = True
        Me.lvwNoAccess.HideSelection = False
        Me.lvwNoAccess.LargeImageList = Me.imgGroup
        Me.lvwNoAccess.Location = New System.Drawing.Point(16, 140)
        Me.lvwNoAccess.Name = "lvwNoAccess"
        Me.lvwNoAccess.Size = New System.Drawing.Size(377, 393)
        Me.lvwNoAccess.SmallImageList = Me.imgGroup
        Me.lvwNoAccess.TabIndex = 7
        Me.lvwNoAccess.UseCompatibleStateImageBehavior = False
        Me.lvwNoAccess.View = System.Windows.Forms.View.Details
        '
        '_lvwNoAccess_ColumnHeader_1
        '
        Me._lvwNoAccess_ColumnHeader_1.Tag = ""
        Me._lvwNoAccess_ColumnHeader_1.Text = "Client Code"
        Me._lvwNoAccess_ColumnHeader_1.Width = 101
        '
        '_lvwNoAccess_ColumnHeader_2
        '
        Me._lvwNoAccess_ColumnHeader_2.Tag = ""
        Me._lvwNoAccess_ColumnHeader_2.Text = "Name"
        Me._lvwNoAccess_ColumnHeader_2.Width = 167
        '
        '_lvwNoAccess_ColumnHeader_3
        '
        Me._lvwNoAccess_ColumnHeader_3.Tag = ""
        Me._lvwNoAccess_ColumnHeader_3.Text = "Address Line 1"
        Me._lvwNoAccess_ColumnHeader_3.Width = 134
        '
        '_lvwNoAccess_ColumnHeader_4
        '
        Me._lvwNoAccess_ColumnHeader_4.Tag = ""
        Me._lvwNoAccess_ColumnHeader_4.Text = "Address Line 2"
        Me._lvwNoAccess_ColumnHeader_4.Width = 134
        '
        '_lvwNoAccess_ColumnHeader_5
        '
        Me._lvwNoAccess_ColumnHeader_5.Tag = ""
        Me._lvwNoAccess_ColumnHeader_5.Text = "Post Code"
        Me._lvwNoAccess_ColumnHeader_5.Width = 67
        '
        'fraFind
        '
        Me.fraFind.BackColor = System.Drawing.SystemColors.Control
        Me.fraFind.Controls.Add(Me.cmdFind)
        Me.fraFind.Controls.Add(Me.txtPostalCode)
        Me.fraFind.Controls.Add(Me.txtAddress1)
        Me.fraFind.Controls.Add(Me.cmbType)
        Me.fraFind.Controls.Add(Me.txtLongName)
        Me.fraFind.Controls.Add(Me.txtShortName)
        Me.fraFind.Controls.Add(Me.lblPostalCode)
        Me.fraFind.Controls.Add(Me.lblAddress1)
        Me.fraFind.Controls.Add(Me.lblType)
        Me.fraFind.Controls.Add(Me.lblLongName)
        Me.fraFind.Controls.Add(Me.lblShortName)
        Me.fraFind.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraFind.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFind.Location = New System.Drawing.Point(16, 12)
        Me.fraFind.Name = "fraFind"
        Me.fraFind.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFind.Size = New System.Drawing.Size(849, 97)
        Me.fraFind.TabIndex = 18
        Me.fraFind.TabStop = False
        Me.fraFind.Text = "Find Clients"
        '
        'cmdFind
        '
        Me.cmdFind.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFind.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFind.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFind.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFind.Location = New System.Drawing.Point(760, 64)
        Me.cmdFind.Name = "cmdFind"
        Me.cmdFind.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFind.Size = New System.Drawing.Size(73, 25)
        Me.cmdFind.TabIndex = 5
        Me.cmdFind.Text = "&Find"
        Me.cmdFind.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFind.UseVisualStyleBackColor = False
        '
        'txtPostalCode
        '
        Me.txtPostalCode.AcceptsReturn = True
        Me.txtPostalCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtPostalCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPostalCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPostalCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPostalCode.Location = New System.Drawing.Point(520, 40)
        Me.txtPostalCode.MaxLength = 0
        Me.txtPostalCode.Name = "txtPostalCode"
        Me.txtPostalCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPostalCode.Size = New System.Drawing.Size(105, 20)
        Me.txtPostalCode.TabIndex = 4
        '
        'txtAddress1
        '
        Me.txtAddress1.AcceptsReturn = True
        Me.txtAddress1.BackColor = System.Drawing.SystemColors.Window
        Me.txtAddress1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAddress1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAddress1.Location = New System.Drawing.Point(520, 16)
        Me.txtAddress1.MaxLength = 0
        Me.txtAddress1.Name = "txtAddress1"
        Me.txtAddress1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAddress1.Size = New System.Drawing.Size(313, 20)
        Me.txtAddress1.TabIndex = 3
        '
        'cmbType
        '
        Me.cmbType.BackColor = System.Drawing.SystemColors.Window
        Me.cmbType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbType.Location = New System.Drawing.Point(80, 64)
        Me.cmbType.Name = "cmbType"
        Me.cmbType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbType.Size = New System.Drawing.Size(97, 21)
        Me.cmbType.TabIndex = 2
        '
        'txtLongName
        '
        Me.txtLongName.AcceptsReturn = True
        Me.txtLongName.BackColor = System.Drawing.SystemColors.Window
        Me.txtLongName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLongName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLongName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLongName.Location = New System.Drawing.Point(80, 40)
        Me.txtLongName.MaxLength = 0
        Me.txtLongName.Name = "txtLongName"
        Me.txtLongName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLongName.Size = New System.Drawing.Size(297, 20)
        Me.txtLongName.TabIndex = 1
        '
        'txtShortName
        '
        Me.txtShortName.AcceptsReturn = True
        Me.txtShortName.BackColor = System.Drawing.SystemColors.Window
        Me.txtShortName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtShortName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtShortName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtShortName.Location = New System.Drawing.Point(80, 16)
        Me.txtShortName.MaxLength = 0
        Me.txtShortName.Name = "txtShortName"
        Me.txtShortName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtShortName.Size = New System.Drawing.Size(297, 20)
        Me.txtShortName.TabIndex = 0
        '
        'lblPostalCode
        '
        Me.lblPostalCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblPostalCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPostalCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPostalCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPostalCode.Location = New System.Drawing.Point(432, 40)
        Me.lblPostalCode.Name = "lblPostalCode"
        Me.lblPostalCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPostalCode.Size = New System.Drawing.Size(89, 17)
        Me.lblPostalCode.TabIndex = 23
        Me.lblPostalCode.Text = "Postcode:"
        '
        'lblAddress1
        '
        Me.lblAddress1.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddress1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddress1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddress1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddress1.Location = New System.Drawing.Point(432, 19)
        Me.lblAddress1.Name = "lblAddress1"
        Me.lblAddress1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddress1.Size = New System.Drawing.Size(89, 17)
        Me.lblAddress1.TabIndex = 22
        Me.lblAddress1.Text = "Address line 1:"
        '
        'lblType
        '
        Me.lblType.BackColor = System.Drawing.SystemColors.Control
        Me.lblType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblType.Location = New System.Drawing.Point(8, 66)
        Me.lblType.Name = "lblType"
        Me.lblType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblType.Size = New System.Drawing.Size(89, 17)
        Me.lblType.TabIndex = 21
        Me.lblType.Text = "Type:"
        '
        'lblLongName
        '
        Me.lblLongName.BackColor = System.Drawing.SystemColors.Control
        Me.lblLongName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLongName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLongName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLongName.Location = New System.Drawing.Point(8, 43)
        Me.lblLongName.Name = "lblLongName"
        Me.lblLongName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLongName.Size = New System.Drawing.Size(145, 17)
        Me.lblLongName.TabIndex = 20
        Me.lblLongName.Text = "Name:"
        '
        'lblShortName
        '
        Me.lblShortName.BackColor = System.Drawing.SystemColors.Control
        Me.lblShortName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShortName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShortName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblShortName.Location = New System.Drawing.Point(8, 20)
        Me.lblShortName.Name = "lblShortName"
        Me.lblShortName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShortName.Size = New System.Drawing.Size(161, 13)
        Me.lblShortName.TabIndex = 19
        Me.lblShortName.Text = "Client code:"
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdFind
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(897, 624)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.uctPMResizer1)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabClients)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Edit Online Access"
        Me.tabClients.ResumeLayout(False)
        Me._tabClients_TabPage0.ResumeLayout(False)
        Me._tabClients_TabPage0.PerformLayout()
        Me.fraFind.ResumeLayout(False)
        Me.fraFind.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
	Sub lvwNoAccess_InitializeColumnKeys()
		Me._lvwNoAccess_ColumnHeader_1.Name = ""
		Me._lvwNoAccess_ColumnHeader_2.Name = ""
		Me._lvwNoAccess_ColumnHeader_3.Name = ""
		Me._lvwNoAccess_ColumnHeader_4.Name = ""
		Me._lvwNoAccess_ColumnHeader_5.Name = ""
	End Sub
	Sub lvwAccess_InitializeColumnKeys()
		Me._lvwAccess_ColumnHeader_1.Name = ""
		Me._lvwAccess_ColumnHeader_2.Name = ""
		Me._lvwAccess_ColumnHeader_3.Name = ""
		Me._lvwAccess_ColumnHeader_4.Name = ""
		Me._lvwAccess_ColumnHeader_5.Name = ""
	End Sub
#End Region 
End Class