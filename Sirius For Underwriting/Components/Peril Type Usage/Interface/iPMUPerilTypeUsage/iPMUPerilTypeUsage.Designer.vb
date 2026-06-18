<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        lvwPerilTypeUsage_InitializeColumnKeys()
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
    Public WithEvents lblSharePercentage As System.Windows.Forms.Label
    Public WithEvents lblPerilType As System.Windows.Forms.Label
    Public WithEvents txtSharePercentage As System.Windows.Forms.TextBox
    Public WithEvents cmdDetailOK As System.Windows.Forms.Button
    Public WithEvents cmdDetailCancel As System.Windows.Forms.Button
    Public WithEvents cboPerilType As PMLookupControl.cboPMLookup
    Private WithEvents _tabDetailTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents tabDetailTab As System.Windows.Forms.TabControl
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents lblPerilGroup As System.Windows.Forms.Label
    Public WithEvents lblPercAllocated As System.Windows.Forms.Label
    Public WithEvents pnlPercAllocated As System.Windows.Forms.Panel
    Public WithEvents cmdDelete As System.Windows.Forms.Button
    Public WithEvents cmdEdit As System.Windows.Forms.Button
    Public WithEvents cmdAdd As System.Windows.Forms.Button
    Private WithEvents _lvwPerilTypeUsage_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwPerilTypeUsage_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwPerilTypeUsage As System.Windows.Forms.ListView
    Public WithEvents txtFormatPercent As System.Windows.Forms.TextBox
    Public WithEvents pnlPerilGroup As System.Windows.Forms.Panel
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public WithEvents ImageList1 As System.Windows.Forms.ImageList

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tabDetailTab = New System.Windows.Forms.TabControl
        Me._tabDetailTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblSharePercentage = New System.Windows.Forms.Label
        Me.lblPerilType = New System.Windows.Forms.Label
        Me.txtSharePercentage = New System.Windows.Forms.TextBox
        Me.cmdDetailOK = New System.Windows.Forms.Button
        Me.cmdDetailCancel = New System.Windows.Forms.Button
        Me.cboPerilType = New PMLookupControl.cboPMLookup
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblPerilGroup = New System.Windows.Forms.Label
        Me.lblPercAllocated = New System.Windows.Forms.Label
        Me.pnlPercAllocated = New System.Windows.Forms.Panel
        Me.txtPercAllocated = New System.Windows.Forms.TextBox
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.lvwPerilTypeUsage = New System.Windows.Forms.ListView
        Me._lvwPerilTypeUsage_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwPerilTypeUsage_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.txtFormatPercent = New System.Windows.Forms.TextBox
        Me.pnlPerilGroup = New System.Windows.Forms.Panel
        Me.txtPerilGroup = New System.Windows.Forms.TextBox
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.tabDetailTab.SuspendLayout()
        Me._tabDetailTab_TabPage0.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.pnlPercAllocated.SuspendLayout()
        Me.pnlPerilGroup.SuspendLayout()
        Me.SuspendLayout()
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
        Me.tabDetailTab.TabIndex = 11
        '
        '_tabDetailTab_TabPage0
        '
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblSharePercentage)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblPerilType)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtSharePercentage)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdDetailOK)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdDetailCancel)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cboPerilType)
        Me._tabDetailTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabDetailTab_TabPage0.Name = "_tabDetailTab_TabPage0"
        Me._tabDetailTab_TabPage0.Size = New System.Drawing.Size(597, 259)
        Me._tabDetailTab_TabPage0.TabIndex = 0
        Me._tabDetailTab_TabPage0.Text = "&2 - Share"
        '
        'lblSharePercentage
        '
        Me.lblSharePercentage.BackColor = System.Drawing.SystemColors.Control
        Me.lblSharePercentage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSharePercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSharePercentage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSharePercentage.Location = New System.Drawing.Point(24, 95)
        Me.lblSharePercentage.Name = "lblSharePercentage"
        Me.lblSharePercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSharePercentage.Size = New System.Drawing.Size(130, 17)
        Me.lblSharePercentage.TabIndex = 12
        Me.lblSharePercentage.Text = "Share percentage:"
        '
        'lblPerilType
        '
        Me.lblPerilType.BackColor = System.Drawing.SystemColors.Control
        Me.lblPerilType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPerilType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPerilType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPerilType.Location = New System.Drawing.Point(24, 55)
        Me.lblPerilType.Name = "lblPerilType"
        Me.lblPerilType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPerilType.Size = New System.Drawing.Size(130, 17)
        Me.lblPerilType.TabIndex = 15
        Me.lblPerilType.Text = "Peril type:"
        '
        'txtSharePercentage
        '
        Me.txtSharePercentage.AcceptsReturn = True
        Me.txtSharePercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtSharePercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSharePercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSharePercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSharePercentage.Location = New System.Drawing.Point(160, 92)
        Me.txtSharePercentage.MaxLength = 0
        Me.txtSharePercentage.Name = "txtSharePercentage"
        Me.txtSharePercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSharePercentage.Size = New System.Drawing.Size(113, 20)
        Me.txtSharePercentage.TabIndex = 8
        Me.txtSharePercentage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
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
        Me.cmdDetailOK.TabIndex = 9
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
        Me.cmdDetailCancel.TabIndex = 10
        Me.cmdDetailCancel.Text = "&Cancel"
        Me.cmdDetailCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDetailCancel.UseVisualStyleBackColor = False
        '
        'cboPerilType
        '
        Me.cboPerilType.DefaultItemId = 0
        Me.cboPerilType.FirstItem = ""
        Me.cboPerilType.ItemId = 0
        Me.cboPerilType.ListIndex = -1
        Me.cboPerilType.Location = New System.Drawing.Point(160, 52)
        Me.cboPerilType.Name = "cboPerilType"
        Me.cboPerilType.PMLookupProductFamily = 9
        Me.cboPerilType.SingleItemId = 0
        Me.cboPerilType.Size = New System.Drawing.Size(393, 21)
        Me.cboPerilType.Sorted = True
        Me.cboPerilType.TabIndex = 6
        Me.cboPerilType.TableName = "peril_type"
        Me.cboPerilType.ToolTipText = ""
        Me.cboPerilType.WhereClause = ""
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
        Me.cmdHelp.TabIndex = 7
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
        Me.tabMainTab.Location = New System.Drawing.Point(8, 12)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(605, 269)
        Me.tabMainTab.TabIndex = 13
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPerilGroup)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPercAllocated)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pnlPercAllocated)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdDelete)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdEdit)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAdd)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwPerilTypeUsage)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtFormatPercent)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pnlPerilGroup)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(597, 243)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Peril Type Usage"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'lblPerilGroup
        '
        Me.lblPerilGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblPerilGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPerilGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPerilGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPerilGroup.Location = New System.Drawing.Point(8, 20)
        Me.lblPerilGroup.Name = "lblPerilGroup"
        Me.lblPerilGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPerilGroup.Size = New System.Drawing.Size(72, 17)
        Me.lblPerilGroup.TabIndex = 16
        Me.lblPerilGroup.Text = "Peril group:"
        '
        'lblPercAllocated
        '
        Me.lblPercAllocated.BackColor = System.Drawing.SystemColors.Control
        Me.lblPercAllocated.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPercAllocated.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPercAllocated.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPercAllocated.Location = New System.Drawing.Point(272, 20)
        Me.lblPercAllocated.Name = "lblPercAllocated"
        Me.lblPercAllocated.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPercAllocated.Size = New System.Drawing.Size(67, 17)
        Me.lblPercAllocated.TabIndex = 18
        Me.lblPercAllocated.Text = "% allocated:"
        '
        'pnlPercAllocated
        '
        Me.pnlPercAllocated.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.pnlPercAllocated.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlPercAllocated.Controls.Add(Me.txtPercAllocated)
        Me.pnlPercAllocated.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlPercAllocated.Location = New System.Drawing.Point(358, 21)
        Me.pnlPercAllocated.Name = "pnlPercAllocated"
        Me.pnlPercAllocated.Size = New System.Drawing.Size(129, 17)
        Me.pnlPercAllocated.TabIndex = 19
        '
        'txtPercAllocated
        '
        Me.txtPercAllocated.BackColor = System.Drawing.Color.Silver
        Me.txtPercAllocated.Location = New System.Drawing.Point(-2, -4)
        Me.txtPercAllocated.Name = "txtPercAllocated"
        Me.txtPercAllocated.Size = New System.Drawing.Size(129, 21)
        Me.txtPercAllocated.TabIndex = 1
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(520, 220)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(65, 22)
        Me.cmdDelete.TabIndex = 3
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
        Me.cmdEdit.Location = New System.Drawing.Point(352, 220)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 1
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
        Me.cmdAdd.Location = New System.Drawing.Point(440, 220)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 2
        Me.cmdAdd.Text = "&New"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'lvwPerilTypeUsage
        '
        Me.lvwPerilTypeUsage.BackColor = System.Drawing.SystemColors.Window
        Me.lvwPerilTypeUsage.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwPerilTypeUsage.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwPerilTypeUsage_ColumnHeader_1, Me._lvwPerilTypeUsage_ColumnHeader_2})
        Me.lvwPerilTypeUsage.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwPerilTypeUsage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwPerilTypeUsage.LargeImageList = Me.ImageList1
        Me.lvwPerilTypeUsage.Location = New System.Drawing.Point(8, 44)
        Me.lvwPerilTypeUsage.Name = "lvwPerilTypeUsage"
        Me.lvwPerilTypeUsage.Size = New System.Drawing.Size(577, 169)
        Me.lvwPerilTypeUsage.SmallImageList = Me.ImageList1
        Me.lvwPerilTypeUsage.TabIndex = 0
        Me.lvwPerilTypeUsage.UseCompatibleStateImageBehavior = False
        Me.lvwPerilTypeUsage.View = System.Windows.Forms.View.Details
        '
        '_lvwPerilTypeUsage_ColumnHeader_1
        '
        Me._lvwPerilTypeUsage_ColumnHeader_1.Tag = ""
        Me._lvwPerilTypeUsage_ColumnHeader_1.Text = "Peril Type"
        Me._lvwPerilTypeUsage_ColumnHeader_1.Width = 201
        '
        '_lvwPerilTypeUsage_ColumnHeader_2
        '
        Me._lvwPerilTypeUsage_ColumnHeader_2.Tag = ""
        Me._lvwPerilTypeUsage_ColumnHeader_2.Text = "Share %"
        Me._lvwPerilTypeUsage_ColumnHeader_2.Width = 97
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "FindImage")
        '
        'txtFormatPercent
        '
        Me.txtFormatPercent.AcceptsReturn = True
        Me.txtFormatPercent.BackColor = System.Drawing.SystemColors.Window
        Me.txtFormatPercent.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFormatPercent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFormatPercent.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFormatPercent.Location = New System.Drawing.Point(8, 220)
        Me.txtFormatPercent.MaxLength = 0
        Me.txtFormatPercent.Name = "txtFormatPercent"
        Me.txtFormatPercent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFormatPercent.Size = New System.Drawing.Size(97, 20)
        Me.txtFormatPercent.TabIndex = 14
        Me.txtFormatPercent.Visible = False
        '
        'pnlPerilGroup
        '
        Me.pnlPerilGroup.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.pnlPerilGroup.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlPerilGroup.Controls.Add(Me.txtPerilGroup)
        Me.pnlPerilGroup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlPerilGroup.Location = New System.Drawing.Point(108, 21)
        Me.pnlPerilGroup.Name = "pnlPerilGroup"
        Me.pnlPerilGroup.Size = New System.Drawing.Size(129, 17)
        Me.pnlPerilGroup.TabIndex = 17
        '
        'txtPerilGroup
        '
        Me.txtPerilGroup.BackColor = System.Drawing.Color.Silver
        Me.txtPerilGroup.Location = New System.Drawing.Point(-2, -5)
        Me.txtPerilGroup.Name = "txtPerilGroup"
        Me.txtPerilGroup.Size = New System.Drawing.Size(129, 21)
        Me.txtPerilGroup.TabIndex = 0
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(619, 321)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.tabDetailTab)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Peril Type Usage"
        Me.tabDetailTab.ResumeLayout(False)
        Me._tabDetailTab_TabPage0.ResumeLayout(False)
        Me._tabDetailTab_TabPage0.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.pnlPercAllocated.ResumeLayout(False)
        Me.pnlPercAllocated.PerformLayout()
        Me.pnlPerilGroup.ResumeLayout(False)
        Me.pnlPerilGroup.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Sub lvwPerilTypeUsage_InitializeColumnKeys()
        Me._lvwPerilTypeUsage_ColumnHeader_1.Name = ""
        Me._lvwPerilTypeUsage_ColumnHeader_2.Name = ""
    End Sub
    Friend WithEvents txtPercAllocated As System.Windows.Forms.TextBox
    Friend WithEvents txtPerilGroup As System.Windows.Forms.TextBox
#End Region
End Class