<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmInterface
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        isInitializingComponent = True
        InitializeComponent()
        isInitializingComponent = False
        tabIEPreviousTab = tabIE.SelectedIndex
        Form_Initialize_Renamed()
    End Sub
    Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed

        If Not (components Is Nothing) Then
            components.Dispose()
        End If

        MyBase.Dispose(Disposing)
    End Sub
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    Public WithEvents cmdReview As System.Windows.Forms.Button
    Public WithEvents cmdRefresh As System.Windows.Forms.Button
    Public WithEvents Label4 As System.Windows.Forms.Label
    Private WithEvents _lvwImport_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwImport_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwImport_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwImport_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwImport_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwImport As System.Windows.Forms.ListView
    Public WithEvents cmdImport As System.Windows.Forms.Button
    Private WithEvents _tabIE_TabPage0 As System.Windows.Forms.TabPage
    Private WithEvents _lvwImported_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwImported_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwImported_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwImported_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwImported_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwImported As System.Windows.Forms.ListView
    Private WithEvents _tabIE_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents lblEventType As System.Windows.Forms.Label
    Private WithEvents _lvwExport_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwExport_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwExport_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwExport_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwExport_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwExport As System.Windows.Forms.ListView
    Public WithEvents cboExportList As System.Windows.Forms.ComboBox
    Public WithEvents cmdExport As System.Windows.Forms.Button
    Public WithEvents dtpStartDate As System.Windows.Forms.DateTimePicker
    Public WithEvents cboPMExportMediaType As PMLookupControl.cboPMLookup
    Public WithEvents chkExportAutoPost As System.Windows.Forms.CheckBox
    Public WithEvents txtExportLeadDays As System.Windows.Forms.TextBox
    Public WithEvents txtExportBatchID As System.Windows.Forms.TextBox
    Public WithEvents cboPMExportBankAccountName As PMLookupControl.cboPMLookup
    Public WithEvents cboPMExportPFSchemeTypeCode As PMLookupControl.cboPMLookup
    Public WithEvents dtpEndDate As System.Windows.Forms.DateTimePicker
    Public WithEvents lblEndDate As System.Windows.Forms.Label
    Public WithEvents lblStartDate As System.Windows.Forms.Label
    Public WithEvents lblExportPFSchemeTypeCode As System.Windows.Forms.Label
    Public WithEvents lblExportMediaType As System.Windows.Forms.Label
    Public WithEvents lblExportLeadDays As System.Windows.Forms.Label
    Public WithEvents lblExportBankAccountName As System.Windows.Forms.Label
    Public WithEvents lblExportBatchID As System.Windows.Forms.Label
    Public WithEvents frameExport As System.Windows.Forms.GroupBox
    Public WithEvents cboEventType As System.Windows.Forms.ComboBox
    Private WithEvents _tabIE_TabPage2 As System.Windows.Forms.TabPage
    Public WithEvents tabIE As System.Windows.Forms.TabControl
    Public WithEvents cmdClose As System.Windows.Forms.Button
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    Private tabIEPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdReview = New System.Windows.Forms.Button()
        Me.cmdRefresh = New System.Windows.Forms.Button()
        Me.tabIE = New System.Windows.Forms.TabControl()
        Me._tabIE_TabPage0 = New System.Windows.Forms.TabPage()
        Me.cmdImportSchedule = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lvwImport = New System.Windows.Forms.ListView()
        Me._lvwImport_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwImport_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwImport_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwImport_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwImport_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmdImport = New System.Windows.Forms.Button()
        Me._tabIE_TabPage1 = New System.Windows.Forms.TabPage()
        Me.lvwImported = New System.Windows.Forms.ListView()
        Me._lvwImported_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwImported_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwImported_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwImported_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwImported_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._tabIE_TabPage2 = New System.Windows.Forms.TabPage()
        Me.cmdExportSchedule = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblEventType = New System.Windows.Forms.Label()
        Me.lvwExport = New System.Windows.Forms.ListView()
        Me._lvwExport_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwExport_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwExport_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwExport_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwExport_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cboExportList = New System.Windows.Forms.ComboBox()
        Me.cmdExport = New System.Windows.Forms.Button()
        Me.frameExport = New System.Windows.Forms.GroupBox()
        Me.cboExportDueDay = New System.Windows.Forms.ComboBox()
        Me.chkExportDueDay = New System.Windows.Forms.CheckBox()
        Me.cmbAgentType = New System.Windows.Forms.ComboBox()
        Me.lblAgentType = New System.Windows.Forms.Label()
        Me.cboCurrency = New UserControls.CurrencyLookup()
        Me.lblCurrency = New System.Windows.Forms.Label()
        Me.chkExportMetadata = New System.Windows.Forms.CheckBox()
        Me.lblPeriod = New System.Windows.Forms.Label()
        Me.cboAllPeriod = New System.Windows.Forms.ComboBox()
        Me.txtXSLTDestExtension = New System.Windows.Forms.TextBox()
        Me.lblXSLTDestExtension = New System.Windows.Forms.Label()
        Me.txtXSLTDestFolder = New System.Windows.Forms.TextBox()
        Me.lblXSLTDestFolder = New System.Windows.Forms.Label()
        Me.txtXSLTFilename = New System.Windows.Forms.TextBox()
        Me.lblXSLTFilename = New System.Windows.Forms.Label()
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker()
        Me.cboPMExportMediaType = New PMLookupControl.cboPMLookup()
        Me.chkExportAutoPost = New System.Windows.Forms.CheckBox()
        Me.txtExportLeadDays = New System.Windows.Forms.TextBox()
        Me.txtExportBatchID = New System.Windows.Forms.TextBox()
        Me.cboPMExportBankAccountName = New PMLookupControl.cboPMLookup()
        Me.cboPMExportPFSchemeTypeCode = New PMLookupControl.cboPMLookup()
        Me.dtpEndDate = New System.Windows.Forms.DateTimePicker()
        Me.lblEndDate = New System.Windows.Forms.Label()
        Me.lblStartDate = New System.Windows.Forms.Label()
        Me.lblExportPFSchemeTypeCode = New System.Windows.Forms.Label()
        Me.lblExportMediaType = New System.Windows.Forms.Label()
        Me.lblExportLeadDays = New System.Windows.Forms.Label()
        Me.lblExportBankAccountName = New System.Windows.Forms.Label()
        Me.lblExportBatchID = New System.Windows.Forms.Label()
        Me.cboEventType = New System.Windows.Forms.ComboBox()
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabIE.SuspendLayout()
        Me._tabIE_TabPage0.SuspendLayout()
        Me._tabIE_TabPage1.SuspendLayout()
        Me._tabIE_TabPage2.SuspendLayout()
        Me.frameExport.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdReview
        '
        Me.cmdReview.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReview.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReview.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReview.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReview.Location = New System.Drawing.Point(82, 553)
        Me.cmdReview.Name = "cmdReview"
        Me.cmdReview.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReview.Size = New System.Drawing.Size(73, 22)
        Me.cmdReview.TabIndex = 1
        Me.cmdReview.Text = "Re&view"
        Me.cmdReview.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdReview.UseVisualStyleBackColor = False
        '
        'cmdRefresh
        '
        Me.cmdRefresh.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRefresh.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRefresh.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRefresh.Location = New System.Drawing.Point(4, 553)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRefresh.Size = New System.Drawing.Size(73, 22)
        Me.cmdRefresh.TabIndex = 0
        Me.cmdRefresh.Text = "&Refresh"
        Me.cmdRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRefresh.UseVisualStyleBackColor = False
        '
        'tabIE
        '
        Me.tabIE.Controls.Add(Me._tabIE_TabPage0)
        Me.tabIE.Controls.Add(Me._tabIE_TabPage1)
        Me.tabIE.Controls.Add(Me._tabIE_TabPage2)
        Me.tabIE.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabIE.ItemSize = New System.Drawing.Size(216, 18)
        Me.tabIE.Location = New System.Drawing.Point(4, 4)
        Me.tabIE.Multiline = True
        Me.tabIE.Name = "tabIE"
        Me.tabIE.SelectedIndex = 0
        Me.tabIE.Size = New System.Drawing.Size(662, 543)
        Me.tabIE.TabIndex = 3
        '
        '_tabIE_TabPage0
        '
        Me._tabIE_TabPage0.Controls.Add(Me.cmdImportSchedule)
        Me._tabIE_TabPage0.Controls.Add(Me.Label4)
        Me._tabIE_TabPage0.Controls.Add(Me.lvwImport)
        Me._tabIE_TabPage0.Controls.Add(Me.cmdImport)
        Me._tabIE_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabIE_TabPage0.Name = "_tabIE_TabPage0"
        Me._tabIE_TabPage0.Size = New System.Drawing.Size(654, 517)
        Me._tabIE_TabPage0.TabIndex = 0
        Me._tabIE_TabPage0.Text = "Waiting to be Imported"
        '
        'cmdImportSchedule
        '
        Me.cmdImportSchedule.BackColor = System.Drawing.SystemColors.Control
        Me.cmdImportSchedule.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdImportSchedule.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdImportSchedule.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdImportSchedule.Location = New System.Drawing.Point(550, 20)
        Me.cmdImportSchedule.Name = "cmdImportSchedule"
        Me.cmdImportSchedule.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdImportSchedule.Size = New System.Drawing.Size(94, 22)
        Me.cmdImportSchedule.TabIndex = 15
        Me.cmdImportSchedule.Text = "&Schedule"
        Me.cmdImportSchedule.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdImportSchedule.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(8, 52)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(113, 17)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Files to import:"
        '
        'lvwImport
        '
        Me.lvwImport.BackColor = System.Drawing.SystemColors.Window
        Me.lvwImport.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwImport, "")
        Me.lvwImport.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwImport_ColumnHeader_1, Me._lvwImport_ColumnHeader_2, Me._lvwImport_ColumnHeader_3, Me._lvwImport_ColumnHeader_4, Me._lvwImport_ColumnHeader_5})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwImport, True)
        Me.lvwImport.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwImport.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwImport.FullRowSelect = True
        Me.lvwImport.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwImport, "lvwImport_ItemClick")
        Me.listViewHelper1.SetLargeIcons(Me.lvwImport, "")
        Me.lvwImport.Location = New System.Drawing.Point(6, 70)
        Me.lvwImport.Name = "lvwImport"
        Me.lvwImport.Size = New System.Drawing.Size(645, 444)
        Me.listViewHelper1.SetSmallIcons(Me.lvwImport, "")
        Me.listViewHelper1.SetSorted(Me.lvwImport, False)
        Me.listViewHelper1.SetSortKey(Me.lvwImport, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwImport, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwImport.TabIndex = 5
        Me.lvwImport.UseCompatibleStateImageBehavior = False
        Me.lvwImport.View = System.Windows.Forms.View.Details
        '
        '_lvwImport_ColumnHeader_1
        '
        Me._lvwImport_ColumnHeader_1.Text = "Date"
        Me._lvwImport_ColumnHeader_1.Width = 101
        '
        '_lvwImport_ColumnHeader_2
        '
        Me._lvwImport_ColumnHeader_2.Text = "Interface"
        Me._lvwImport_ColumnHeader_2.Width = 161
        '
        '_lvwImport_ColumnHeader_3
        '
        Me._lvwImport_ColumnHeader_3.Text = "Batch Reference"
        Me._lvwImport_ColumnHeader_3.Width = 161
        '
        '_lvwImport_ColumnHeader_4
        '
        Me._lvwImport_ColumnHeader_4.Text = "Estimated Records"
        Me._lvwImport_ColumnHeader_4.Width = 101
        '
        '_lvwImport_ColumnHeader_5
        '
        Me._lvwImport_ColumnHeader_5.Text = "Filename"
        Me._lvwImport_ColumnHeader_5.Width = 241
        '
        'cmdImport
        '
        Me.cmdImport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdImport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdImport.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdImport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdImport.Location = New System.Drawing.Point(8, 20)
        Me.cmdImport.Name = "cmdImport"
        Me.cmdImport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdImport.Size = New System.Drawing.Size(105, 22)
        Me.cmdImport.TabIndex = 4
        Me.cmdImport.Text = "&Import All Files"
        Me.cmdImport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdImport.UseVisualStyleBackColor = False
        '
        '_tabIE_TabPage1
        '
        Me._tabIE_TabPage1.Controls.Add(Me.lvwImported)
        Me._tabIE_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabIE_TabPage1.Name = "_tabIE_TabPage1"
        Me._tabIE_TabPage1.Size = New System.Drawing.Size(654, 517)
        Me._tabIE_TabPage1.TabIndex = 1
        Me._tabIE_TabPage1.Text = "Imported"
        '
        'lvwImported
        '
        Me.lvwImported.BackColor = System.Drawing.SystemColors.Window
        Me.lvwImported.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwImported, "")
        Me.lvwImported.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwImported_ColumnHeader_1, Me._lvwImported_ColumnHeader_2, Me._lvwImported_ColumnHeader_3, Me._lvwImported_ColumnHeader_4, Me._lvwImported_ColumnHeader_5})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwImported, True)
        Me.lvwImported.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwImported.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwImported.FullRowSelect = True
        Me.lvwImported.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwImported, "lvwImported_ItemClick")
        Me.listViewHelper1.SetLargeIcons(Me.lvwImported, "")
        Me.lvwImported.Location = New System.Drawing.Point(6, 6)
        Me.lvwImported.Name = "lvwImported"
        Me.lvwImported.Size = New System.Drawing.Size(643, 508)
        Me.listViewHelper1.SetSmallIcons(Me.lvwImported, "")
        Me.listViewHelper1.SetSorted(Me.lvwImported, False)
        Me.listViewHelper1.SetSortKey(Me.lvwImported, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwImported, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwImported.TabIndex = 6
        Me.lvwImported.UseCompatibleStateImageBehavior = False
        Me.lvwImported.View = System.Windows.Forms.View.Details
        '
        '_lvwImported_ColumnHeader_1
        '
        Me._lvwImported_ColumnHeader_1.Text = "Date Imported"
        Me._lvwImported_ColumnHeader_1.Width = 101
        '
        '_lvwImported_ColumnHeader_2
        '
        Me._lvwImported_ColumnHeader_2.Text = "Interface"
        Me._lvwImported_ColumnHeader_2.Width = 161
        '
        '_lvwImported_ColumnHeader_3
        '
        Me._lvwImported_ColumnHeader_3.Text = "Batch Reference"
        Me._lvwImported_ColumnHeader_3.Width = 161
        '
        '_lvwImported_ColumnHeader_4
        '
        Me._lvwImported_ColumnHeader_4.Text = "Estimated Records"
        Me._lvwImported_ColumnHeader_4.Width = 101
        '
        '_lvwImported_ColumnHeader_5
        '
        Me._lvwImported_ColumnHeader_5.Text = "Filename"
        Me._lvwImported_ColumnHeader_5.Width = 241
        '
        '_tabIE_TabPage2
        '
        Me._tabIE_TabPage2.Controls.Add(Me.cmdExportSchedule)
        Me._tabIE_TabPage2.Controls.Add(Me.Label1)
        Me._tabIE_TabPage2.Controls.Add(Me.Label3)
        Me._tabIE_TabPage2.Controls.Add(Me.lblEventType)
        Me._tabIE_TabPage2.Controls.Add(Me.lvwExport)
        Me._tabIE_TabPage2.Controls.Add(Me.cboExportList)
        Me._tabIE_TabPage2.Controls.Add(Me.cmdExport)
        Me._tabIE_TabPage2.Controls.Add(Me.frameExport)
        Me._tabIE_TabPage2.Controls.Add(Me.cboEventType)
        Me._tabIE_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabIE_TabPage2.Name = "_tabIE_TabPage2"
        Me._tabIE_TabPage2.Size = New System.Drawing.Size(654, 517)
        Me._tabIE_TabPage2.TabIndex = 2
        Me._tabIE_TabPage2.Text = "Exported"
        '
        'cmdExportSchedule
        '
        Me.cmdExportSchedule.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExportSchedule.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExportSchedule.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExportSchedule.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExportSchedule.Location = New System.Drawing.Point(547, 15)
        Me.cmdExportSchedule.Name = "cmdExportSchedule"
        Me.cmdExportSchedule.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExportSchedule.Size = New System.Drawing.Size(94, 22)
        Me.cmdExportSchedule.TabIndex = 27
        Me.cmdExportSchedule.Text = "&Schedule"
        Me.cmdExportSchedule.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdExportSchedule.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(8, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(42, 17)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Export:"
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(3, 313)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(87, 12)
        Me.Label3.TabIndex = 17
        Me.Label3.Text = "Exported files:"
        '
        'lblEventType
        '
        Me.lblEventType.BackColor = System.Drawing.Color.Transparent
        Me.lblEventType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEventType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEventType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEventType.Location = New System.Drawing.Point(296, 17)
        Me.lblEventType.Name = "lblEventType"
        Me.lblEventType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEventType.Size = New System.Drawing.Size(70, 17)
        Me.lblEventType.TabIndex = 26
        Me.lblEventType.Text = "Event Type:"
        '
        'lvwExport
        '
        Me.lvwExport.BackColor = System.Drawing.SystemColors.Window
        Me.lvwExport.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwExport, "")
        Me.lvwExport.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwExport_ColumnHeader_1, Me._lvwExport_ColumnHeader_2, Me._lvwExport_ColumnHeader_3, Me._lvwExport_ColumnHeader_4, Me._lvwExport_ColumnHeader_5})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwExport, True)
        Me.lvwExport.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwExport.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwExport.FullRowSelect = True
        Me.lvwExport.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwExport, "lvwExport_ItemClick")
        Me.listViewHelper1.SetLargeIcons(Me.lvwExport, "")
        Me.lvwExport.Location = New System.Drawing.Point(3, 334)
        Me.lvwExport.Name = "lvwExport"
        Me.lvwExport.Size = New System.Drawing.Size(641, 202)
        Me.listViewHelper1.SetSmallIcons(Me.lvwExport, "")
        Me.listViewHelper1.SetSorted(Me.lvwExport, False)
        Me.listViewHelper1.SetSortKey(Me.lvwExport, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwExport, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwExport.TabIndex = 15
        Me.lvwExport.UseCompatibleStateImageBehavior = False
        Me.lvwExport.View = System.Windows.Forms.View.Details
        '
        '_lvwExport_ColumnHeader_1
        '
        Me._lvwExport_ColumnHeader_1.Text = "Date Exported"
        Me._lvwExport_ColumnHeader_1.Width = 101
        '
        '_lvwExport_ColumnHeader_2
        '
        Me._lvwExport_ColumnHeader_2.Text = "Interface"
        Me._lvwExport_ColumnHeader_2.Width = 161
        '
        '_lvwExport_ColumnHeader_3
        '
        Me._lvwExport_ColumnHeader_3.Text = "Batch Reference"
        Me._lvwExport_ColumnHeader_3.Width = 161
        '
        '_lvwExport_ColumnHeader_4
        '
        Me._lvwExport_ColumnHeader_4.Text = "Estimated Records"
        Me._lvwExport_ColumnHeader_4.Width = 101
        '
        '_lvwExport_ColumnHeader_5
        '
        Me._lvwExport_ColumnHeader_5.Text = "Filename"
        Me._lvwExport_ColumnHeader_5.Width = 241
        '
        'cboExportList
        '
        Me.cboExportList.BackColor = System.Drawing.SystemColors.Window
        Me.cboExportList.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboExportList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboExportList.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboExportList.Location = New System.Drawing.Point(56, 15)
        Me.cboExportList.Name = "cboExportList"
        Me.cboExportList.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboExportList.Size = New System.Drawing.Size(209, 21)
        Me.cboExportList.TabIndex = 7
        Me.cboExportList.Text = "Combo1"
        '
        'cmdExport
        '
        Me.cmdExport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExport.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExport.Location = New System.Drawing.Point(540, 15)
        Me.cmdExport.Name = "cmdExport"
        Me.cmdExport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExport.Size = New System.Drawing.Size(73, 22)
        Me.cmdExport.TabIndex = 8
        Me.cmdExport.Text = "&Export"
        Me.cmdExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdExport.UseVisualStyleBackColor = False
        '
        'frameExport
        '
        Me.frameExport.BackColor = System.Drawing.SystemColors.Control
        Me.frameExport.Controls.Add(Me.cboExportDueDay)
        Me.frameExport.Controls.Add(Me.chkExportDueDay)
        Me.frameExport.Controls.Add(Me.cmbAgentType)
        Me.frameExport.Controls.Add(Me.lblAgentType)
        Me.frameExport.Controls.Add(Me.cboCurrency)
        Me.frameExport.Controls.Add(Me.lblCurrency)
        Me.frameExport.Controls.Add(Me.chkExportMetadata)
        Me.frameExport.Controls.Add(Me.lblPeriod)
        Me.frameExport.Controls.Add(Me.cboAllPeriod)
        Me.frameExport.Controls.Add(Me.txtXSLTDestExtension)
        Me.frameExport.Controls.Add(Me.lblXSLTDestExtension)
        Me.frameExport.Controls.Add(Me.txtXSLTDestFolder)
        Me.frameExport.Controls.Add(Me.lblXSLTDestFolder)
        Me.frameExport.Controls.Add(Me.txtXSLTFilename)
        Me.frameExport.Controls.Add(Me.lblXSLTFilename)
        Me.frameExport.Controls.Add(Me.dtpStartDate)
        Me.frameExport.Controls.Add(Me.cboPMExportMediaType)
        Me.frameExport.Controls.Add(Me.chkExportAutoPost)
        Me.frameExport.Controls.Add(Me.txtExportLeadDays)
        Me.frameExport.Controls.Add(Me.txtExportBatchID)
        Me.frameExport.Controls.Add(Me.cboPMExportBankAccountName)
        Me.frameExport.Controls.Add(Me.cboPMExportPFSchemeTypeCode)
        Me.frameExport.Controls.Add(Me.dtpEndDate)
        Me.frameExport.Controls.Add(Me.lblEndDate)
        Me.frameExport.Controls.Add(Me.lblStartDate)
        Me.frameExport.Controls.Add(Me.lblExportPFSchemeTypeCode)
        Me.frameExport.Controls.Add(Me.lblExportMediaType)
        Me.frameExport.Controls.Add(Me.lblExportLeadDays)
        Me.frameExport.Controls.Add(Me.lblExportBankAccountName)
        Me.frameExport.Controls.Add(Me.lblExportBatchID)
        Me.frameExport.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frameExport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frameExport.Location = New System.Drawing.Point(6, 44)
        Me.frameExport.Name = "frameExport"
        Me.frameExport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frameExport.Size = New System.Drawing.Size(641, 266)
        Me.frameExport.TabIndex = 18
        Me.frameExport.TabStop = False
        Me.frameExport.Text = "Parameters"
        '
        'cboExportDueDay
        '
        Me.cboExportDueDay.FormattingEnabled = True
        Me.cboExportDueDay.Location = New System.Drawing.Point(560, 200)
        Me.cboExportDueDay.Name = "cboExportDueDay"
        Me.cboExportDueDay.Size = New System.Drawing.Size(75, 21)
        Me.cboExportDueDay.TabIndex = 51
        '
        'chkExportDueDay
        '
        Me.chkExportDueDay.BackColor = System.Drawing.SystemColors.Control
        Me.chkExportDueDay.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkExportDueDay.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkExportDueDay.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkExportDueDay.Location = New System.Drawing.Point(463, 200)
        Me.chkExportDueDay.Name = "chkExportDueDay"
        Me.chkExportDueDay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkExportDueDay.Size = New System.Drawing.Size(91, 22)
        Me.chkExportDueDay.TabIndex = 50
        Me.chkExportDueDay.Text = "Due Date"
        Me.chkExportDueDay.UseVisualStyleBackColor = False
        '
        'cmbAgentType
        '
        Me.cmbAgentType.FormattingEnabled = True
        Me.cmbAgentType.Location = New System.Drawing.Point(119, 239)
        Me.cmbAgentType.Name = "cmbAgentType"
        Me.cmbAgentType.Size = New System.Drawing.Size(172, 21)
        Me.cmbAgentType.TabIndex = 49
        '
        'lblAgentType
        '
        Me.lblAgentType.AutoSize = True
        Me.lblAgentType.Location = New System.Drawing.Point(36, 239)
        Me.lblAgentType.Name = "lblAgentType"
        Me.lblAgentType.Size = New System.Drawing.Size(65, 13)
        Me.lblAgentType.TabIndex = 48
        Me.lblAgentType.Text = "Agent Type:"
        '
        'cboCurrency
        '
        Me.cboCurrency.CompanyId = 0
        Me.cboCurrency.CurrencyId = 0
        Me.cboCurrency.DefaultCurrencyId = 0
        Me.cboCurrency.FirstItem = ""
        Me.cboCurrency.ListIndex = -1
        Me.cboCurrency.Location = New System.Drawing.Point(119, 205)
        Me.cboCurrency.Name = "cboCurrency"
        Me.cboCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actAllCurrencies
        Me.cboCurrency.Size = New System.Drawing.Size(172, 21)
        Me.cboCurrency.TabIndex = 38
        Me.cboCurrency.ToolTipText = ""
        Me.cboCurrency.WhatsThisHelpID = 0
        '
        'lblCurrency
        '
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(48, 209)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(66, 13)
        Me.lblCurrency.TabIndex = 37
        Me.lblCurrency.Text = "Currency:"
        '
        'chkExportMetadata
        '
        Me.chkExportMetadata.BackColor = System.Drawing.SystemColors.Control
        Me.chkExportMetadata.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkExportMetadata.Checked = True
        Me.chkExportMetadata.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkExportMetadata.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkExportMetadata.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkExportMetadata.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkExportMetadata.Location = New System.Drawing.Point(463, 234)
        Me.chkExportMetadata.Name = "chkExportMetadata"
        Me.chkExportMetadata.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkExportMetadata.Size = New System.Drawing.Size(172, 22)
        Me.chkExportMetadata.TabIndex = 33
        Me.chkExportMetadata.Text = "Export Metadata:"
        Me.chkExportMetadata.UseVisualStyleBackColor = False
        '
        'lblPeriod
        '
        Me.lblPeriod.AutoSize = True
        Me.lblPeriod.BackColor = System.Drawing.SystemColors.Control
        Me.lblPeriod.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPeriod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPeriod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPeriod.Location = New System.Drawing.Point(59, 180)
        Me.lblPeriod.Name = "lblPeriod"
        Me.lblPeriod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPeriod.Size = New System.Drawing.Size(40, 13)
        Me.lblPeriod.TabIndex = 32
        Me.lblPeriod.Text = "Period:"
        '
        'cboAllPeriod
        '
        Me.cboAllPeriod.BackColor = System.Drawing.SystemColors.Window
        Me.cboAllPeriod.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboAllPeriod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboAllPeriod.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboAllPeriod.Location = New System.Drawing.Point(119, 177)
        Me.cboAllPeriod.Name = "cboAllPeriod"
        Me.cboAllPeriod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboAllPeriod.Size = New System.Drawing.Size(172, 21)
        Me.cboAllPeriod.TabIndex = 31
        Me.cboAllPeriod.Text = "Combo1"
        '
        'txtXSLTDestExtension
        '
        Me.txtXSLTDestExtension.AcceptsReturn = True
        Me.txtXSLTDestExtension.BackColor = System.Drawing.SystemColors.Window
        Me.txtXSLTDestExtension.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtXSLTDestExtension.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtXSLTDestExtension.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtXSLTDestExtension.Location = New System.Drawing.Point(119, 147)
        Me.txtXSLTDestExtension.MaxLength = 0
        Me.txtXSLTDestExtension.Name = "txtXSLTDestExtension"
        Me.txtXSLTDestExtension.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtXSLTDestExtension.Size = New System.Drawing.Size(198, 20)
        Me.txtXSLTDestExtension.TabIndex = 36
        '
        'lblXSLTDestExtension
        '
        Me.lblXSLTDestExtension.AutoSize = True
        Me.lblXSLTDestExtension.Location = New System.Drawing.Point(5, 150)
        Me.lblXSLTDestExtension.Name = "lblXSLTDestExtension"
        Me.lblXSLTDestExtension.Size = New System.Drawing.Size(111, 13)
        Me.lblXSLTDestExtension.TabIndex = 35
        Me.lblXSLTDestExtension.Text = "XSLT Dest Extension:"
        '
        'txtXSLTDestFolder
        '
        Me.txtXSLTDestFolder.AcceptsReturn = True
        Me.txtXSLTDestFolder.BackColor = System.Drawing.SystemColors.Window
        Me.txtXSLTDestFolder.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtXSLTDestFolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtXSLTDestFolder.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtXSLTDestFolder.Location = New System.Drawing.Point(119, 118)
        Me.txtXSLTDestFolder.MaxLength = 0
        Me.txtXSLTDestFolder.Name = "txtXSLTDestFolder"
        Me.txtXSLTDestFolder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtXSLTDestFolder.Size = New System.Drawing.Size(198, 20)
        Me.txtXSLTDestFolder.TabIndex = 34
        '
        'lblXSLTDestFolder
        '
        Me.lblXSLTDestFolder.AutoSize = True
        Me.lblXSLTDestFolder.Location = New System.Drawing.Point(5, 118)
        Me.lblXSLTDestFolder.Name = "lblXSLTDestFolder"
        Me.lblXSLTDestFolder.Size = New System.Drawing.Size(94, 13)
        Me.lblXSLTDestFolder.TabIndex = 33
        Me.lblXSLTDestFolder.Text = "XSLT Dest Folder:"
        '
        'txtXSLTFilename
        '
        Me.txtXSLTFilename.AcceptsReturn = True
        Me.txtXSLTFilename.BackColor = System.Drawing.SystemColors.Window
        Me.txtXSLTFilename.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtXSLTFilename.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtXSLTFilename.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtXSLTFilename.Location = New System.Drawing.Point(119, 86)
        Me.txtXSLTFilename.MaxLength = 0
        Me.txtXSLTFilename.Name = "txtXSLTFilename"
        Me.txtXSLTFilename.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtXSLTFilename.Size = New System.Drawing.Size(198, 20)
        Me.txtXSLTFilename.TabIndex = 32
        '
        'lblXSLTFilename
        '
        Me.lblXSLTFilename.AutoSize = True
        Me.lblXSLTFilename.Location = New System.Drawing.Point(17, 87)
        Me.lblXSLTFilename.Name = "lblXSLTFilename"
        Me.lblXSLTFilename.Size = New System.Drawing.Size(82, 13)
        Me.lblXSLTFilename.TabIndex = 31
        Me.lblXSLTFilename.Text = "XSLT Filename:"
        '
        'dtpStartDate
        '
        Me.dtpStartDate.Checked = False
        Me.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpStartDate.Location = New System.Drawing.Point(466, 118)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.ShowCheckBox = True
        Me.dtpStartDate.Size = New System.Drawing.Size(169, 20)
        Me.dtpStartDate.TabIndex = 29
        '
        'cboPMExportMediaType
        '
        Me.cboPMExportMediaType.DefaultItemId = 0
        Me.cboPMExportMediaType.FirstItem = ""
        Me.cboPMExportMediaType.ItemId = 0
        Me.cboPMExportMediaType.ListIndex = -1
        Me.cboPMExportMediaType.Location = New System.Drawing.Point(466, 54)
        Me.cboPMExportMediaType.Name = "cboPMExportMediaType"
        Me.cboPMExportMediaType.PMLookupProductFamily = 1
        Me.cboPMExportMediaType.SingleItemId = 0
        Me.cboPMExportMediaType.Size = New System.Drawing.Size(169, 21)
        Me.cboPMExportMediaType.SortColumnName = ""
        Me.cboPMExportMediaType.Sorted = True
        Me.cboPMExportMediaType.TabIndex = 12
        Me.cboPMExportMediaType.TableName = "Mediatype"
        Me.cboPMExportMediaType.ToolTipText = ""
        Me.cboPMExportMediaType.WhereClause = ""
        '
        'chkExportAutoPost
        '
        Me.chkExportAutoPost.BackColor = System.Drawing.SystemColors.Control
        Me.chkExportAutoPost.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkExportAutoPost.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkExportAutoPost.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkExportAutoPost.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkExportAutoPost.Location = New System.Drawing.Point(463, 176)
        Me.chkExportAutoPost.Name = "chkExportAutoPost"
        Me.chkExportAutoPost.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkExportAutoPost.Size = New System.Drawing.Size(172, 22)
        Me.chkExportAutoPost.TabIndex = 13
        Me.chkExportAutoPost.Text = "AutoPost:"
        Me.chkExportAutoPost.UseVisualStyleBackColor = False
        '
        'txtExportLeadDays
        '
        Me.txtExportLeadDays.AcceptsReturn = True
        Me.txtExportLeadDays.BackColor = System.Drawing.SystemColors.Window
        Me.txtExportLeadDays.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtExportLeadDays.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExportLeadDays.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtExportLeadDays.Location = New System.Drawing.Point(119, 54)
        Me.txtExportLeadDays.MaxLength = 0
        Me.txtExportLeadDays.Name = "txtExportLeadDays"
        Me.txtExportLeadDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtExportLeadDays.Size = New System.Drawing.Size(73, 20)
        Me.txtExportLeadDays.TabIndex = 11
        '
        'txtExportBatchID
        '
        Me.txtExportBatchID.AcceptsReturn = True
        Me.txtExportBatchID.BackColor = System.Drawing.SystemColors.Window
        Me.txtExportBatchID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtExportBatchID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExportBatchID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtExportBatchID.Location = New System.Drawing.Point(119, 23)
        Me.txtExportBatchID.MaxLength = 0
        Me.txtExportBatchID.Name = "txtExportBatchID"
        Me.txtExportBatchID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtExportBatchID.Size = New System.Drawing.Size(73, 20)
        Me.txtExportBatchID.TabIndex = 9
        '
        'cboPMExportBankAccountName
        '
        Me.cboPMExportBankAccountName.DefaultItemId = 0
        Me.cboPMExportBankAccountName.FirstItem = ""
        Me.cboPMExportBankAccountName.ItemId = 0
        Me.cboPMExportBankAccountName.ListIndex = -1
        Me.cboPMExportBankAccountName.Location = New System.Drawing.Point(466, 22)
        Me.cboPMExportBankAccountName.Name = "cboPMExportBankAccountName"
        Me.cboPMExportBankAccountName.PMLookupProductFamily = 1
        Me.cboPMExportBankAccountName.SingleItemId = 0
        Me.cboPMExportBankAccountName.Size = New System.Drawing.Size(169, 21)
        Me.cboPMExportBankAccountName.SortColumnName = ""
        Me.cboPMExportBankAccountName.Sorted = True
        Me.cboPMExportBankAccountName.TabIndex = 10
        Me.cboPMExportBankAccountName.TableName = "BankAccount"
        Me.cboPMExportBankAccountName.ToolTipText = ""
        Me.cboPMExportBankAccountName.WhereClause = ""
        '
        'cboPMExportPFSchemeTypeCode
        '
        Me.cboPMExportPFSchemeTypeCode.DefaultItemId = 0
        Me.cboPMExportPFSchemeTypeCode.FirstItem = ""
        Me.cboPMExportPFSchemeTypeCode.ItemId = 0
        Me.cboPMExportPFSchemeTypeCode.ListIndex = -1
        Me.cboPMExportPFSchemeTypeCode.Location = New System.Drawing.Point(466, 86)
        Me.cboPMExportPFSchemeTypeCode.Name = "cboPMExportPFSchemeTypeCode"
        Me.cboPMExportPFSchemeTypeCode.PMLookupProductFamily = 1
        Me.cboPMExportPFSchemeTypeCode.SingleItemId = 0
        Me.cboPMExportPFSchemeTypeCode.Size = New System.Drawing.Size(169, 21)
        Me.cboPMExportPFSchemeTypeCode.SortColumnName = ""
        Me.cboPMExportPFSchemeTypeCode.Sorted = True
        Me.cboPMExportPFSchemeTypeCode.TabIndex = 23
        Me.cboPMExportPFSchemeTypeCode.TableName = "PFScheme_Type"
        Me.cboPMExportPFSchemeTypeCode.ToolTipText = ""
        Me.cboPMExportPFSchemeTypeCode.WhereClause = ""
        '
        'dtpEndDate
        '
        Me.dtpEndDate.Checked = False
        Me.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpEndDate.Location = New System.Drawing.Point(466, 150)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.ShowCheckBox = True
        Me.dtpEndDate.Size = New System.Drawing.Size(169, 20)
        Me.dtpEndDate.TabIndex = 30
        '
        'lblEndDate
        '
        Me.lblEndDate.AutoSize = True
        Me.lblEndDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEndDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEndDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEndDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEndDate.Location = New System.Drawing.Point(402, 150)
        Me.lblEndDate.Name = "lblEndDate"
        Me.lblEndDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEndDate.Size = New System.Drawing.Size(55, 13)
        Me.lblEndDate.TabIndex = 28
        Me.lblEndDate.Text = "End Date:"
        '
        'lblStartDate
        '
        Me.lblStartDate.AutoSize = True
        Me.lblStartDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblStartDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStartDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStartDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStartDate.Location = New System.Drawing.Point(402, 121)
        Me.lblStartDate.Name = "lblStartDate"
        Me.lblStartDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStartDate.Size = New System.Drawing.Size(61, 13)
        Me.lblStartDate.TabIndex = 27
        Me.lblStartDate.Text = "Start Date: "
        '
        'lblExportPFSchemeTypeCode
        '
        Me.lblExportPFSchemeTypeCode.AutoSize = True
        Me.lblExportPFSchemeTypeCode.BackColor = System.Drawing.Color.Transparent
        Me.lblExportPFSchemeTypeCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExportPFSchemeTypeCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExportPFSchemeTypeCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExportPFSchemeTypeCode.Location = New System.Drawing.Point(343, 89)
        Me.lblExportPFSchemeTypeCode.Name = "lblExportPFSchemeTypeCode"
        Me.lblExportPFSchemeTypeCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExportPFSchemeTypeCode.Size = New System.Drawing.Size(117, 13)
        Me.lblExportPFSchemeTypeCode.TabIndex = 24
        Me.lblExportPFSchemeTypeCode.Text = "Finance Scheme Type:"
        Me.lblExportPFSchemeTypeCode.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblExportMediaType
        '
        Me.lblExportMediaType.AutoSize = True
        Me.lblExportMediaType.BackColor = System.Drawing.Color.Transparent
        Me.lblExportMediaType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExportMediaType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExportMediaType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExportMediaType.Location = New System.Drawing.Point(394, 58)
        Me.lblExportMediaType.Name = "lblExportMediaType"
        Me.lblExportMediaType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExportMediaType.Size = New System.Drawing.Size(66, 13)
        Me.lblExportMediaType.TabIndex = 22
        Me.lblExportMediaType.Text = "Media Type:"
        Me.lblExportMediaType.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblExportLeadDays
        '
        Me.lblExportLeadDays.BackColor = System.Drawing.Color.Transparent
        Me.lblExportLeadDays.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExportLeadDays.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExportLeadDays.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExportLeadDays.Location = New System.Drawing.Point(34, 54)
        Me.lblExportLeadDays.Name = "lblExportLeadDays"
        Me.lblExportLeadDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExportLeadDays.Size = New System.Drawing.Size(65, 17)
        Me.lblExportLeadDays.TabIndex = 21
        Me.lblExportLeadDays.Text = "Lead Days:"
        Me.lblExportLeadDays.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblExportBankAccountName
        '
        Me.lblExportBankAccountName.AutoSize = True
        Me.lblExportBankAccountName.BackColor = System.Drawing.Color.Transparent
        Me.lblExportBankAccountName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExportBankAccountName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExportBankAccountName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExportBankAccountName.Location = New System.Drawing.Point(348, 23)
        Me.lblExportBankAccountName.Name = "lblExportBankAccountName"
        Me.lblExportBankAccountName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExportBankAccountName.Size = New System.Drawing.Size(109, 13)
        Me.lblExportBankAccountName.TabIndex = 20
        Me.lblExportBankAccountName.Text = "Bank Account Name:"
        Me.lblExportBankAccountName.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblExportBatchID
        '
        Me.lblExportBatchID.BackColor = System.Drawing.Color.Transparent
        Me.lblExportBatchID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExportBatchID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExportBatchID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExportBatchID.Location = New System.Drawing.Point(28, 23)
        Me.lblExportBatchID.Name = "lblExportBatchID"
        Me.lblExportBatchID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExportBatchID.Size = New System.Drawing.Size(73, 17)
        Me.lblExportBatchID.TabIndex = 19
        Me.lblExportBatchID.Text = "Batch ID:"
        Me.lblExportBatchID.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cboEventType
        '
        Me.cboEventType.BackColor = System.Drawing.SystemColors.Window
        Me.cboEventType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboEventType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboEventType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboEventType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboEventType.Location = New System.Drawing.Point(372, 15)
        Me.cboEventType.Name = "cboEventType"
        Me.cboEventType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboEventType.Size = New System.Drawing.Size(141, 21)
        Me.cboEventType.TabIndex = 25
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClose.Location = New System.Drawing.Point(584, 553)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClose.Size = New System.Drawing.Size(73, 22)
        Me.cmdClose.TabIndex = 2
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClose.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(664, 587)
        Me.Controls.Add(Me.cmdReview)
        Me.Controls.Add(Me.cmdRefresh)
        Me.Controls.Add(Me.tabIE)
        Me.Controls.Add(Me.cmdClose)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Imports and Exports"
        Me.tabIE.ResumeLayout(False)
        Me._tabIE_TabPage0.ResumeLayout(False)
        Me._tabIE_TabPage1.ResumeLayout(False)
        Me._tabIE_TabPage2.ResumeLayout(False)
        Me.frameExport.ResumeLayout(False)
        Me.frameExport.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents lblPeriod As System.Windows.Forms.Label
    Public WithEvents chkExportMetadata As System.Windows.Forms.CheckBox
    Friend WithEvents lblXSLTDestFolder As System.Windows.Forms.Label
    Public WithEvents txtXSLTFilename As System.Windows.Forms.TextBox
    Friend WithEvents lblXSLTFilename As System.Windows.Forms.Label
    Public WithEvents txtXSLTDestFolder As System.Windows.Forms.TextBox
    Public WithEvents txtXSLTDestExtension As System.Windows.Forms.TextBox
    Friend WithEvents lblXSLTDestExtension As System.Windows.Forms.Label
    Public WithEvents lblCurrency As System.Windows.Forms.Label
    Friend WithEvents cboCurrency As UserControls.CurrencyLookup
    Public WithEvents cboAllPeriod As System.Windows.Forms.ComboBox
    Friend WithEvents cmbAgentType As System.Windows.Forms.ComboBox
    Friend WithEvents lblAgentType As System.Windows.Forms.Label
    Public WithEvents cmdImportSchedule As Button
    Public WithEvents cmdExportSchedule As Button
    Public WithEvents chkExportDueDay As CheckBox
    Friend WithEvents cboExportDueDay As ComboBox
#End Region
End Class
