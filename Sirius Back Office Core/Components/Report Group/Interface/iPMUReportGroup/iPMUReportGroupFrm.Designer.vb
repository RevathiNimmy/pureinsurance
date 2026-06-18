<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializepnlReportGroup()
        InitializelblReportGroup()
        InitializelblRG()
		lvwReportGroupUserGroups_InitializeColumnKeys()
		lvwReportGroupContents_InitializeColumnKeys()
		tabMainTabPreviousTab = tabMainTab.SelectedIndex
		Form_Initialize_Renamed()
	End Sub
    Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
        Dispose(True)
    End Sub
    'Form overrides dispose to clean up the component list.
    '<System.Diagnostics.DebuggerNonUserCode()> _
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
    Public WithEvents txtFormatCurrency As System.Windows.Forms.TextBox
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Private WithEvents _lblReportGroup_0 As System.Windows.Forms.Label
    Private WithEvents _lvwReportGroupContents_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwReportGroupContents_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwReportGroupContents_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwReportGroupContents As System.Windows.Forms.ListView
    Private WithEvents _pnlReportGroup_0 As System.Windows.Forms.Panel
    Public WithEvents cmdSelectReportGroup As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents cmdSelectUserGroups As System.Windows.Forms.Button
    Private WithEvents _pnlReportGroup_1 As System.Windows.Forms.Panel
    Private WithEvents _lvwReportGroupUserGroups_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwReportGroupUserGroups_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwReportGroupUserGroups As System.Windows.Forms.ListView
    Private WithEvents _lblReportGroup_1 As System.Windows.Forms.Label
    Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public WithEvents _pnlLblRG_0 As System.Windows.Forms.Label
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public WithEvents ImageList1 As System.Windows.Forms.ImageList
    Public lblReportGroup(1) As System.Windows.Forms.Label
    Public pnlLblRG(1) As System.Windows.Forms.Label
    Public pnlReportGroup(1) As System.Windows.Forms.Panel
    Private tabMainTabPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    '<System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdApply = New System.Windows.Forms.Button
        Me.txtFormatCurrency = New System.Windows.Forms.TextBox
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me._lblReportGroup_0 = New System.Windows.Forms.Label
        Me.lvwReportGroupContents = New System.Windows.Forms.ListView
        Me._lvwReportGroupContents_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwReportGroupContents_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwReportGroupContents_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me._pnlReportGroup_0 = New System.Windows.Forms.Panel
        Me._pnlLblRG_0 = New System.Windows.Forms.Label
        Me.cmdSelectReportGroup = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me.cmdSelectUserGroups = New System.Windows.Forms.Button
        Me._pnlReportGroup_1 = New System.Windows.Forms.Panel
        Me._pnlLblRG_1 = New System.Windows.Forms.Label
        Me.lvwReportGroupUserGroups = New System.Windows.Forms.ListView
        Me._lvwReportGroupUserGroups_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwReportGroupUserGroups_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lblReportGroup_1 = New System.Windows.Forms.Label
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me._pnlReportGroup_0.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me._pnlReportGroup_1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Location = New System.Drawing.Point(16, 336)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApply.Size = New System.Drawing.Size(73, 22)
        Me.cmdApply.TabIndex = 12
        Me.cmdApply.Text = "&Apply"
        Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdApply.UseVisualStyleBackColor = False
        '
        'txtFormatCurrency
        '
        Me.txtFormatCurrency.AcceptsReturn = True
        Me.txtFormatCurrency.BackColor = System.Drawing.SystemColors.Window
        Me.txtFormatCurrency.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFormatCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFormatCurrency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFormatCurrency.Location = New System.Drawing.Point(160, 336)
        Me.txtFormatCurrency.MaxLength = 0
        Me.txtFormatCurrency.Name = "txtFormatCurrency"
        Me.txtFormatCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFormatCurrency.Size = New System.Drawing.Size(97, 20)
        Me.txtFormatCurrency.TabIndex = 11
        Me.txtFormatCurrency.Visible = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(528, 336)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 3
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
        Me.cmdCancel.Location = New System.Drawing.Point(448, 336)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 2
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
        Me.cmdOK.Location = New System.Drawing.Point(360, 336)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 1
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage1)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(299, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(605, 317)
        Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.tabMainTab.TabIndex = 4
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me._lblReportGroup_0)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwReportGroupContents)
        Me._tabMainTab_TabPage0.Controls.Add(Me._pnlReportGroup_0)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdSelectReportGroup)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(597, 291)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Report Group Contents"
        '
        '_lblReportGroup_0
        '
        Me._lblReportGroup_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblReportGroup_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblReportGroup_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblReportGroup_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblReportGroup_0.Location = New System.Drawing.Point(8, 16)
        Me._lblReportGroup_0.Name = "_lblReportGroup_0"
        Me._lblReportGroup_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblReportGroup_0.Size = New System.Drawing.Size(89, 17)
        Me._lblReportGroup_0.TabIndex = 5
        Me._lblReportGroup_0.Text = "Report Group:"
        '
        'lvwReportGroupContents
        '
        Me.lvwReportGroupContents.BackColor = System.Drawing.SystemColors.Window
        Me.lvwReportGroupContents.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwReportGroupContents.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwReportGroupContents_ColumnHeader_1, Me._lvwReportGroupContents_ColumnHeader_2, Me._lvwReportGroupContents_ColumnHeader_3})
        Me.lvwReportGroupContents.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwReportGroupContents.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwReportGroupContents.FullRowSelect = True
        Me.lvwReportGroupContents.LargeImageList = Me.ImageList1
        Me.lvwReportGroupContents.Location = New System.Drawing.Point(8, 40)
        Me.lvwReportGroupContents.Name = "lvwReportGroupContents"
        Me.lvwReportGroupContents.Size = New System.Drawing.Size(577, 209)
        Me.lvwReportGroupContents.SmallImageList = Me.ImageList1
        Me.lvwReportGroupContents.TabIndex = 6
        Me.lvwReportGroupContents.UseCompatibleStateImageBehavior = False
        Me.lvwReportGroupContents.View = System.Windows.Forms.View.Details
        '
        '_lvwReportGroupContents_ColumnHeader_1
        '
        Me._lvwReportGroupContents_ColumnHeader_1.Tag = ""
        Me._lvwReportGroupContents_ColumnHeader_1.Text = ""
        Me._lvwReportGroupContents_ColumnHeader_1.Width = 201
        '
        '_lvwReportGroupContents_ColumnHeader_2
        '
        Me._lvwReportGroupContents_ColumnHeader_2.Tag = ""
        Me._lvwReportGroupContents_ColumnHeader_2.Text = ""
        Me._lvwReportGroupContents_ColumnHeader_2.Width = 97
        '
        '_lvwReportGroupContents_ColumnHeader_3
        '
        Me._lvwReportGroupContents_ColumnHeader_3.Tag = ""
        Me._lvwReportGroupContents_ColumnHeader_3.Text = ""
        Me._lvwReportGroupContents_ColumnHeader_3.Width = 97
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "")
        '
        '_pnlReportGroup_0
        '
        Me._pnlReportGroup_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._pnlReportGroup_0.Controls.Add(Me._pnlLblRG_0)
        Me._pnlReportGroup_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._pnlReportGroup_0.Location = New System.Drawing.Point(104, 16)
        Me._pnlReportGroup_0.Name = "_pnlReportGroup_0"
        Me._pnlReportGroup_0.Size = New System.Drawing.Size(129, 17)
        Me._pnlReportGroup_0.TabIndex = 6
        '
        '_pnlLblRG_0
        '
        Me._pnlLblRG_0.AutoSize = True
        Me._pnlLblRG_0.Location = New System.Drawing.Point(3, 0)
        Me._pnlLblRG_0.Name = "_pnlLblRG_0"
        Me._pnlLblRG_0.Size = New System.Drawing.Size(0, 13)
        Me._pnlLblRG_0.TabIndex = 0
        '
        'cmdSelectReportGroup
        '
        Me.cmdSelectReportGroup.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelectReportGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelectReportGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSelectReportGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelectReportGroup.Location = New System.Drawing.Point(512, 260)
        Me.cmdSelectReportGroup.Name = "cmdSelectReportGroup"
        Me.cmdSelectReportGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelectReportGroup.Size = New System.Drawing.Size(73, 22)
        Me.cmdSelectReportGroup.TabIndex = 10
        Me.cmdSelectReportGroup.Text = "&Select"
        Me.cmdSelectReportGroup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSelectReportGroup.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.cmdSelectUserGroups)
        Me._tabMainTab_TabPage1.Controls.Add(Me._pnlReportGroup_1)
        Me._tabMainTab_TabPage1.Controls.Add(Me.lvwReportGroupUserGroups)
        Me._tabMainTab_TabPage1.Controls.Add(Me._lblReportGroup_1)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(597, 291)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "2 - Report Group User Groups"
        '
        'cmdSelectUserGroups
        '
        Me.cmdSelectUserGroups.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelectUserGroups.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelectUserGroups.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSelectUserGroups.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelectUserGroups.Location = New System.Drawing.Point(512, 260)
        Me.cmdSelectUserGroups.Name = "cmdSelectUserGroups"
        Me.cmdSelectUserGroups.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelectUserGroups.Size = New System.Drawing.Size(73, 22)
        Me.cmdSelectUserGroups.TabIndex = 13
        Me.cmdSelectUserGroups.Text = "&Select"
        Me.cmdSelectUserGroups.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSelectUserGroups.UseVisualStyleBackColor = False
        '
        '_pnlReportGroup_1
        '
        Me._pnlReportGroup_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._pnlReportGroup_1.Controls.Add(Me._pnlLblRG_1)
        Me._pnlReportGroup_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._pnlReportGroup_1.Location = New System.Drawing.Point(104, 16)
        Me._pnlReportGroup_1.Name = "_pnlReportGroup_1"
        Me._pnlReportGroup_1.Size = New System.Drawing.Size(129, 17)
        Me._pnlReportGroup_1.TabIndex = 7
        '
        '_pnlLblRG_1
        '
        Me._pnlLblRG_1.AutoSize = True
        Me._pnlLblRG_1.Location = New System.Drawing.Point(3, 0)
        Me._pnlLblRG_1.Name = "_pnlLblRG_1"
        Me._pnlLblRG_1.Size = New System.Drawing.Size(0, 13)
        Me._pnlLblRG_1.TabIndex = 0
        '
        'lvwReportGroupUserGroups
        '
        Me.lvwReportGroupUserGroups.BackColor = System.Drawing.SystemColors.Window
        Me.lvwReportGroupUserGroups.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwReportGroupUserGroups.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwReportGroupUserGroups_ColumnHeader_1, Me._lvwReportGroupUserGroups_ColumnHeader_2})
        Me.lvwReportGroupUserGroups.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwReportGroupUserGroups.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwReportGroupUserGroups.LargeImageList = Me.ImageList1
        Me.lvwReportGroupUserGroups.Location = New System.Drawing.Point(8, 40)
        Me.lvwReportGroupUserGroups.Name = "lvwReportGroupUserGroups"
        Me.lvwReportGroupUserGroups.Size = New System.Drawing.Size(577, 209)
        Me.lvwReportGroupUserGroups.SmallImageList = Me.ImageList1
        Me.lvwReportGroupUserGroups.TabIndex = 9
        Me.lvwReportGroupUserGroups.UseCompatibleStateImageBehavior = False
        Me.lvwReportGroupUserGroups.View = System.Windows.Forms.View.Details
        '
        '_lvwReportGroupUserGroups_ColumnHeader_1
        '
        Me._lvwReportGroupUserGroups_ColumnHeader_1.Tag = ""
        Me._lvwReportGroupUserGroups_ColumnHeader_1.Text = ""
        Me._lvwReportGroupUserGroups_ColumnHeader_1.Width = 201
        '
        '_lvwReportGroupUserGroups_ColumnHeader_2
        '
        Me._lvwReportGroupUserGroups_ColumnHeader_2.Tag = ""
        Me._lvwReportGroupUserGroups_ColumnHeader_2.Text = ""
        Me._lvwReportGroupUserGroups_ColumnHeader_2.Width = 97
        '
        '_lblReportGroup_1
        '
        Me._lblReportGroup_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblReportGroup_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblReportGroup_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblReportGroup_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblReportGroup_1.Location = New System.Drawing.Point(8, 16)
        Me._lblReportGroup_1.Name = "_lblReportGroup_1"
        Me._lblReportGroup_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblReportGroup_1.Size = New System.Drawing.Size(89, 17)
        Me._lblReportGroup_1.TabIndex = 8
        Me._lblReportGroup_1.Text = "Report Group:"
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(619, 369)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.txtFormatCurrency)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "RI Model Lines"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._pnlReportGroup_0.ResumeLayout(False)
        Me._pnlReportGroup_0.PerformLayout()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me._pnlReportGroup_1.ResumeLayout(False)
        Me._pnlReportGroup_1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub InitializepnlReportGroup()
		Me.pnlReportGroup(0) = _pnlReportGroup_0
		Me.pnlReportGroup(1) = _pnlReportGroup_1
	End Sub
	Sub InitializelblReportGroup()
		Me.lblReportGroup(1) = _lblReportGroup_1
		Me.lblReportGroup(0) = _lblReportGroup_0
	End Sub
	Sub lvwReportGroupUserGroups_InitializeColumnKeys()
		Me._lvwReportGroupUserGroups_ColumnHeader_1.Name = ""
		Me._lvwReportGroupUserGroups_ColumnHeader_2.Name = ""
	End Sub
	Sub lvwReportGroupContents_InitializeColumnKeys()
		Me._lvwReportGroupContents_ColumnHeader_1.Name = ""
		Me._lvwReportGroupContents_ColumnHeader_2.Name = ""
		Me._lvwReportGroupContents_ColumnHeader_3.Name = ""
    End Sub
    Sub InitializelblRG()
        Me.pnlLblRG(1) = _pnlLblRG_1
        Me.pnlLblRG(0) = _pnlLblRG_0
    End Sub
    Friend WithEvents _pnlLblRG_1 As System.Windows.Forms.Label
#End Region
End Class