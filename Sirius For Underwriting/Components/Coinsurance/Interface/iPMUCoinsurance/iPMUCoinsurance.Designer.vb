''developer guide no.  imports pmlookupcontrol to avoid cbopmlookup errors
Imports PMLookupControl
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        lvwCoinsurance_InitializeColumnKeys()
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
    Public WithEvents lblAllocatedPerc As System.Windows.Forms.Label
    Public WithEvents lblCOIDefault As System.Windows.Forms.Label
    Public WithEvents lblInsuranceFileRef As System.Windows.Forms.Label
    Public WithEvents pnlInsuranceFileRef As System.Windows.Forms.Panel
    Public WithEvents cmdDelete As System.Windows.Forms.Button
    Public WithEvents cmdEdit As System.Windows.Forms.Button
    Public WithEvents cmdAdd As System.Windows.Forms.Button
    Private WithEvents _lvwCoinsurance_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCoinsurance_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCoinsurance_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCoinsurance_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwCoinsurance As System.Windows.Forms.ListView
    Public WithEvents pnlPercAllocated As System.Windows.Forms.Panel
    Public WithEvents txtFormatPercent As System.Windows.Forms.TextBox
    Public WithEvents cboCOIDefault As cboPMLookup
    Public WithEvents chkIsRecovered As System.Windows.Forms.CheckBox
    Public WithEvents chkIsSurcharged As System.Windows.Forms.CheckBox
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public WithEvents lblSharePercentage As System.Windows.Forms.Label
    Public WithEvents lblCommissionPercentage As System.Windows.Forms.Label
    Public WithEvents lblArrangementRef As System.Windows.Forms.Label
    Public WithEvents cmdInsurerLookup As System.Windows.Forms.Button
    Public WithEvents txtSharePercentage As System.Windows.Forms.TextBox
    Public WithEvents cmdDetailOK As System.Windows.Forms.Button
    Public WithEvents cmdDetailCancel As System.Windows.Forms.Button
    Public WithEvents pnlClient As System.Windows.Forms.Panel
    Public WithEvents txtCommissionPercentage As System.Windows.Forms.TextBox
    Public WithEvents txtArrangementRef As System.Windows.Forms.TextBox
    Private WithEvents _tabDetailTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents tabDetailTab As System.Windows.Forms.TabControl
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
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
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblAllocatedPerc = New System.Windows.Forms.Label
        Me.lblCOIDefault = New System.Windows.Forms.Label
        Me.lblInsuranceFileRef = New System.Windows.Forms.Label
        Me.pnlInsuranceFileRef = New System.Windows.Forms.Panel
        Me.lblInsuranceFile = New System.Windows.Forms.Label
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.lvwCoinsurance = New System.Windows.Forms.ListView
        Me._lvwCoinsurance_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwCoinsurance_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwCoinsurance_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwCoinsurance_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.pnlPercAllocated = New System.Windows.Forms.Panel
        Me.lblPercAllocated = New System.Windows.Forms.Label
        Me.txtFormatPercent = New System.Windows.Forms.TextBox
        Me.cboCOIDefault = New PMLookupControl.cboPMLookup
        Me.chkIsRecovered = New System.Windows.Forms.CheckBox
        Me.chkIsSurcharged = New System.Windows.Forms.CheckBox
        Me.tabDetailTab = New System.Windows.Forms.TabControl
        Me._tabDetailTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblSharePercentage = New System.Windows.Forms.Label
        Me.lblCommissionPercentage = New System.Windows.Forms.Label
        Me.lblArrangementRef = New System.Windows.Forms.Label
        Me.cmdInsurerLookup = New System.Windows.Forms.Button
        Me.txtSharePercentage = New System.Windows.Forms.TextBox
        Me.cmdDetailOK = New System.Windows.Forms.Button
        Me.cmdDetailCancel = New System.Windows.Forms.Button
        Me.pnlClient = New System.Windows.Forms.Panel
        Me.lblClient = New System.Windows.Forms.Label
        Me.txtCommissionPercentage = New System.Windows.Forms.TextBox
        Me.txtArrangementRef = New System.Windows.Forms.TextBox
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.pnlInsuranceFileRef.SuspendLayout()
        Me.pnlPercAllocated.SuspendLayout()
        Me.tabDetailTab.SuspendLayout()
        Me._tabDetailTab_TabPage0.SuspendLayout()
        Me.pnlClient.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.cmdCancel.Location = New System.Drawing.Point(456, 296)
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
        Me.cmdOK.Location = New System.Drawing.Point(368, 296)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 7
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(600, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(3, 7)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(606, 283)
        Me.tabMainTab.TabIndex = 18
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblAllocatedPerc)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCOIDefault)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblInsuranceFileRef)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pnlInsuranceFileRef)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdDelete)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdEdit)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAdd)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwCoinsurance)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pnlPercAllocated)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtFormatPercent)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboCOIDefault)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkIsRecovered)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkIsSurcharged)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(598, 257)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "&1 - Coinsurance"
        '
        'lblAllocatedPerc
        '
        Me.lblAllocatedPerc.BackColor = System.Drawing.SystemColors.Control
        Me.lblAllocatedPerc.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAllocatedPerc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAllocatedPerc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAllocatedPerc.Location = New System.Drawing.Point(248, 47)
        Me.lblAllocatedPerc.Name = "lblAllocatedPerc"
        Me.lblAllocatedPerc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAllocatedPerc.Size = New System.Drawing.Size(81, 17)
        Me.lblAllocatedPerc.TabIndex = 19
        Me.lblAllocatedPerc.Text = "% allocated:"
        '
        'lblCOIDefault
        '
        Me.lblCOIDefault.BackColor = System.Drawing.SystemColors.Control
        Me.lblCOIDefault.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCOIDefault.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCOIDefault.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCOIDefault.Location = New System.Drawing.Point(408, 47)
        Me.lblCOIDefault.Name = "lblCOIDefault"
        Me.lblCOIDefault.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCOIDefault.Size = New System.Drawing.Size(57, 17)
        Me.lblCOIDefault.TabIndex = 2
        Me.lblCOIDefault.Text = "Default:"
        '
        'lblInsuranceFileRef
        '
        Me.lblInsuranceFileRef.BackColor = System.Drawing.SystemColors.Control
        Me.lblInsuranceFileRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInsuranceFileRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInsuranceFileRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInsuranceFileRef.Location = New System.Drawing.Point(8, 15)
        Me.lblInsuranceFileRef.Name = "lblInsuranceFileRef"
        Me.lblInsuranceFileRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInsuranceFileRef.Size = New System.Drawing.Size(90, 14)
        Me.lblInsuranceFileRef.TabIndex = 26
        Me.lblInsuranceFileRef.Text = "Insurance File:"
        '
        'pnlInsuranceFileRef
        '
        Me.pnlInsuranceFileRef.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlInsuranceFileRef.Controls.Add(Me.lblInsuranceFile)
        Me.pnlInsuranceFileRef.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlInsuranceFileRef.Location = New System.Drawing.Point(104, 12)
        Me.pnlInsuranceFileRef.Name = "pnlInsuranceFileRef"
        Me.pnlInsuranceFileRef.Size = New System.Drawing.Size(177, 17)
        Me.pnlInsuranceFileRef.TabIndex = 27
        '
        'lblInsuranceFile
        '
        Me.lblInsuranceFile.AutoSize = True
        Me.lblInsuranceFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInsuranceFile.Location = New System.Drawing.Point(0, 0)
        Me.lblInsuranceFile.Name = "lblInsuranceFile"
        Me.lblInsuranceFile.Size = New System.Drawing.Size(2, 15)
        Me.lblInsuranceFile.TabIndex = 0
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
        Me.cmdDelete.TabIndex = 6
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
        Me.cmdEdit.TabIndex = 4
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
        Me.cmdAdd.TabIndex = 5
        Me.cmdAdd.Text = "&New"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'lvwCoinsurance
        '
        Me.lvwCoinsurance.BackColor = System.Drawing.SystemColors.Window
        Me.lvwCoinsurance.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwCoinsurance, "")
        Me.lvwCoinsurance.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwCoinsurance_ColumnHeader_1, Me._lvwCoinsurance_ColumnHeader_2, Me._lvwCoinsurance_ColumnHeader_3, Me._lvwCoinsurance_ColumnHeader_4})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwCoinsurance, False)
        Me.lvwCoinsurance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwCoinsurance.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwCoinsurance.FullRowSelect = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwCoinsurance, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwCoinsurance, "")
        Me.lvwCoinsurance.LargeImageList = Me.ImageList1
        Me.lvwCoinsurance.Location = New System.Drawing.Point(8, 68)
        Me.lvwCoinsurance.MultiSelect = False
        Me.lvwCoinsurance.Name = "lvwCoinsurance"
        Me.lvwCoinsurance.Size = New System.Drawing.Size(577, 145)
        Me.listViewHelper1.SetSmallIcons(Me.lvwCoinsurance, "")
        Me.lvwCoinsurance.SmallImageList = Me.ImageList1
        Me.listViewHelper1.SetSorted(Me.lvwCoinsurance, False)
        Me.listViewHelper1.SetSortKey(Me.lvwCoinsurance, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwCoinsurance, System.Windows.Forms.SortOrder.Ascending)
		Me.lvwCoinsurance.StateImageList = Me.ImageList1
        Me.lvwCoinsurance.TabIndex = 3
        Me.lvwCoinsurance.UseCompatibleStateImageBehavior = False
        Me.lvwCoinsurance.View = System.Windows.Forms.View.Details
        '
        '_lvwCoinsurance_ColumnHeader_1
        '
        Me._lvwCoinsurance_ColumnHeader_1.Tag = ""
        Me._lvwCoinsurance_ColumnHeader_1.Text = "Coinsurer"
        Me._lvwCoinsurance_ColumnHeader_1.Width = 201
        '
        '_lvwCoinsurance_ColumnHeader_2
        '
        Me._lvwCoinsurance_ColumnHeader_2.Tag = ""
        Me._lvwCoinsurance_ColumnHeader_2.Text = "Arrangement Ref"
        Me._lvwCoinsurance_ColumnHeader_2.Width = 97
        '
        '_lvwCoinsurance_ColumnHeader_3
        '
        Me._lvwCoinsurance_ColumnHeader_3.Tag = ""
        Me._lvwCoinsurance_ColumnHeader_3.Text = "Share %"
        Me._lvwCoinsurance_ColumnHeader_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwCoinsurance_ColumnHeader_3.Width = 97
        '
        '_lvwCoinsurance_ColumnHeader_4
        '
        Me._lvwCoinsurance_ColumnHeader_4.Tag = ""
        Me._lvwCoinsurance_ColumnHeader_4.Text = "Commission %"
        Me._lvwCoinsurance_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwCoinsurance_ColumnHeader_4.Width = 97
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "FindImage")
        '
        'pnlPercAllocated
        '
        Me.pnlPercAllocated.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlPercAllocated.Controls.Add(Me.lblPercAllocated)
        Me.pnlPercAllocated.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlPercAllocated.Location = New System.Drawing.Point(328, 44)
        Me.pnlPercAllocated.Name = "pnlPercAllocated"
        Me.pnlPercAllocated.Size = New System.Drawing.Size(57, 17)
        Me.pnlPercAllocated.TabIndex = 20
        '
        'lblPercAllocated
        '
        Me.lblPercAllocated.AutoSize = True
        Me.lblPercAllocated.Location = New System.Drawing.Point(0, 0)
        Me.lblPercAllocated.Name = "lblPercAllocated"
        Me.lblPercAllocated.Size = New System.Drawing.Size(0, 13)
        Me.lblPercAllocated.TabIndex = 28
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
        Me.txtFormatPercent.TabIndex = 21
        Me.txtFormatPercent.Visible = False
        '
        'cboCOIDefault
        '
        Me.cboCOIDefault.DefaultItemId = 0
        Me.cboCOIDefault.FirstItem = ""
        Me.cboCOIDefault.ItemId = 0
        Me.cboCOIDefault.ListIndex = -1
        Me.cboCOIDefault.Location = New System.Drawing.Point(464, 44)
        Me.cboCOIDefault.Name = "cboCOIDefault"
        Me.cboCOIDefault.PMLookupProductFamily = 9
        Me.cboCOIDefault.SingleItemId = 0
        Me.cboCOIDefault.Size = New System.Drawing.Size(121, 21)
        Me.cboCOIDefault.Sorted = True
        Me.cboCOIDefault.TabIndex = 23
        Me.cboCOIDefault.TableName = "COI_Default"
        Me.cboCOIDefault.ToolTipText = ""
        Me.cboCOIDefault.WhereClause = ""
        '
        'chkIsRecovered
        '
        Me.chkIsRecovered.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsRecovered.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsRecovered.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsRecovered.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsRecovered.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsRecovered.Location = New System.Drawing.Point(8, 47)
        Me.chkIsRecovered.Name = "chkIsRecovered"
        Me.chkIsRecovered.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsRecovered.Size = New System.Drawing.Size(105, 17)
        Me.chkIsRecovered.TabIndex = 0
        Me.chkIsRecovered.Text = "Is recovered?"
        Me.chkIsRecovered.UseVisualStyleBackColor = False
        '
        'chkIsSurcharged
        '
        Me.chkIsSurcharged.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsSurcharged.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsSurcharged.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsSurcharged.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsSurcharged.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsSurcharged.Location = New System.Drawing.Point(128, 47)
        Me.chkIsSurcharged.Name = "chkIsSurcharged"
        Me.chkIsSurcharged.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsSurcharged.Size = New System.Drawing.Size(105, 17)
        Me.chkIsSurcharged.TabIndex = 1
        Me.chkIsSurcharged.Text = "Is surcharged?"
        Me.chkIsSurcharged.UseVisualStyleBackColor = False
        '
        'tabDetailTab
        '
        Me.tabDetailTab.Controls.Add(Me._tabDetailTab_TabPage0)
        Me.tabDetailTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabDetailTab.ItemSize = New System.Drawing.Size(600, 18)
        Me.tabDetailTab.Location = New System.Drawing.Point(7, 324)
        Me.tabDetailTab.Multiline = True
        Me.tabDetailTab.Name = "tabDetailTab"
        Me.tabDetailTab.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tabDetailTab.SelectedIndex = 0
        Me.tabDetailTab.Size = New System.Drawing.Size(605, 285)
        Me.tabDetailTab.TabIndex = 16
        '
        '_tabDetailTab_TabPage0
        '
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblSharePercentage)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblCommissionPercentage)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblArrangementRef)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdInsurerLookup)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtSharePercentage)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdDetailOK)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdDetailCancel)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.pnlClient)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtCommissionPercentage)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtArrangementRef)
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
        Me.lblSharePercentage.Size = New System.Drawing.Size(145, 17)
        Me.lblSharePercentage.TabIndex = 17
        Me.lblSharePercentage.Text = "Share percentage:"
        '
        'lblCommissionPercentage
        '
        Me.lblCommissionPercentage.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommissionPercentage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommissionPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCommissionPercentage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommissionPercentage.Location = New System.Drawing.Point(24, 127)
        Me.lblCommissionPercentage.Name = "lblCommissionPercentage"
        Me.lblCommissionPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommissionPercentage.Size = New System.Drawing.Size(145, 17)
        Me.lblCommissionPercentage.TabIndex = 24
        Me.lblCommissionPercentage.Text = "Commission percentage:"
        '
        'lblArrangementRef
        '
        Me.lblArrangementRef.BackColor = System.Drawing.SystemColors.Control
        Me.lblArrangementRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblArrangementRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblArrangementRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblArrangementRef.Location = New System.Drawing.Point(24, 63)
        Me.lblArrangementRef.Name = "lblArrangementRef"
        Me.lblArrangementRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblArrangementRef.Size = New System.Drawing.Size(145, 17)
        Me.lblArrangementRef.TabIndex = 25
        Me.lblArrangementRef.Text = "Arrangement ref:"
        '
        'cmdInsurerLookup
        '
        Me.cmdInsurerLookup.BackColor = System.Drawing.SystemColors.Control
        Me.cmdInsurerLookup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdInsurerLookup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdInsurerLookup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdInsurerLookup.Location = New System.Drawing.Point(24, 28)
        Me.cmdInsurerLookup.Name = "cmdInsurerLookup"
        Me.cmdInsurerLookup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdInsurerLookup.Size = New System.Drawing.Size(113, 20)
        Me.cmdInsurerLookup.TabIndex = 10
        Me.cmdInsurerLookup.Text = "CoInsurer..."
        Me.cmdInsurerLookup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdInsurerLookup.UseVisualStyleBackColor = False
        '
        'txtSharePercentage
        '
        Me.txtSharePercentage.AcceptsReturn = True
        Me.txtSharePercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtSharePercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSharePercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSharePercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSharePercentage.Location = New System.Drawing.Point(200, 92)
        Me.txtSharePercentage.MaxLength = 0
        Me.txtSharePercentage.Name = "txtSharePercentage"
        Me.txtSharePercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSharePercentage.Size = New System.Drawing.Size(113, 20)
        Me.txtSharePercentage.TabIndex = 12
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
        Me.cmdDetailOK.TabIndex = 14
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
        Me.cmdDetailCancel.TabIndex = 15
        Me.cmdDetailCancel.Text = "&Cancel"
        Me.cmdDetailCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDetailCancel.UseVisualStyleBackColor = False
        '
        'pnlClient
        '
        Me.pnlClient.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlClient.Controls.Add(Me.lblClient)
        Me.pnlClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlClient.Location = New System.Drawing.Point(200, 28)
        Me.pnlClient.Name = "pnlClient"
        Me.pnlClient.Size = New System.Drawing.Size(313, 17)
        Me.pnlClient.TabIndex = 22
        '
        'lblClient
        '
        Me.lblClient.AutoSize = True
        Me.lblClient.Location = New System.Drawing.Point(0, 0)
        Me.lblClient.Name = "lblClient"
        Me.lblClient.Size = New System.Drawing.Size(0, 13)
        Me.lblClient.TabIndex = 0
        '
        'txtCommissionPercentage
        '
        Me.txtCommissionPercentage.AcceptsReturn = True
        Me.txtCommissionPercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtCommissionPercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCommissionPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCommissionPercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCommissionPercentage.Location = New System.Drawing.Point(200, 124)
        Me.txtCommissionPercentage.MaxLength = 0
        Me.txtCommissionPercentage.Name = "txtCommissionPercentage"
        Me.txtCommissionPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCommissionPercentage.Size = New System.Drawing.Size(113, 20)
        Me.txtCommissionPercentage.TabIndex = 13
        Me.txtCommissionPercentage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtArrangementRef
        '
        Me.txtArrangementRef.AcceptsReturn = True
        Me.txtArrangementRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtArrangementRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtArrangementRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtArrangementRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtArrangementRef.Location = New System.Drawing.Point(200, 60)
        Me.txtArrangementRef.MaxLength = 0
        Me.txtArrangementRef.Name = "txtArrangementRef"
        Me.txtArrangementRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtArrangementRef.Size = New System.Drawing.Size(169, 20)
        Me.txtArrangementRef.TabIndex = 11
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(622, 324)
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
        Me.Text = "Coinsurance"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.pnlInsuranceFileRef.ResumeLayout(False)
        Me.pnlInsuranceFileRef.PerformLayout()
        Me.pnlPercAllocated.ResumeLayout(False)
        Me.pnlPercAllocated.PerformLayout()
        Me.tabDetailTab.ResumeLayout(False)
        Me._tabDetailTab_TabPage0.ResumeLayout(False)
        Me._tabDetailTab_TabPage0.PerformLayout()
        Me.pnlClient.ResumeLayout(False)
        Me.pnlClient.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Sub lvwCoinsurance_InitializeColumnKeys()
        Me._lvwCoinsurance_ColumnHeader_1.Name = ""
        Me._lvwCoinsurance_ColumnHeader_2.Name = ""
        Me._lvwCoinsurance_ColumnHeader_3.Name = ""
        Me._lvwCoinsurance_ColumnHeader_4.Name = ""
    End Sub
    Friend WithEvents lblInsuranceFile As System.Windows.Forms.Label
    Friend WithEvents lblPercAllocated As System.Windows.Forms.Label
    Friend WithEvents lblClient As System.Windows.Forms.Label
#End Region
End Class