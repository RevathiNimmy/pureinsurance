<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer. 
        InitializeComponent()
        InitializemnuItem()
        lvwInstalment_InitializeColumnKeys()
        Form_Initialize_Renamed()
    End Sub
    Private Sub Ctx_mnuPopUp_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuPopUp.Opening
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()


        'We are moving the submenus from original menu to the context menu before displaying it
        For Each item As System.Windows.Forms.ToolStripItem In mnuPopUp.DropDownItems
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            Ctx_mnuPopUp.Items.Add(item)
        Next item
        e.Cancel = False
    End Sub
    'Private Sub Ctx_mnuPopUp_Closing(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuPopUp.Closing
    '    'Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
    '    ''We are moving the submenus the context menu back to the original menu after displaying
    '    'For Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuPopUp.Items
    '    '    list.Add(item)
    '    'Next item
    '    'For Each item As System.Windows.Forms.ToolStripItem In list
    '    '    mnuPopUp.DropDownItems.Add(item)
    '    'Next item
    '    Ctx_mnuPopUp.Items.Clear()
    '    mnuItem = Nothing


    'End Sub
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
    Private WithEvents _mnuItem_0 As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuPopUp As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
    Public WithEvents cboMediaType As PMLookupControl.cboPMLookup
    Public WithEvents txtBatchNo As System.Windows.Forms.TextBox
    Private WithEvents _StatusBar_Panel1 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents StatusBar As System.Windows.Forms.StatusStrip
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public WithEvents cboPaymentMethod As System.Windows.Forms.ComboBox
    Private WithEvents _lvwInstalment_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwInstalment_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwInstalment_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwInstalment_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwInstalment_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwInstalment_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwInstalment_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwInstalment As System.Windows.Forms.ListView
    Public WithEvents cmdRefresh As System.Windows.Forms.Button
    Public WithEvents txtFormatCurrency As System.Windows.Forms.TextBox
    Public WithEvents txtLeadDays As System.Windows.Forms.TextBox
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents lblMediaType As System.Windows.Forms.Label
    Public WithEvents lblBar As System.Windows.Forms.Label
    Public WithEvents lblPaymentMethod As System.Windows.Forms.Label
    Public WithEvents lblLeadDays As System.Windows.Forms.Label
    Public WithEvents lblBatchNo As System.Windows.Forms.Label
    Public mnuItem(0) As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents Ctx_mnuPopUp As System.Windows.Forms.ContextMenuStrip
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip()
        Me.mnuPopUp = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuItem_0 = New System.Windows.Forms.ToolStripMenuItem()
        Me.cboMediaType = New PMLookupControl.cboPMLookup()
        Me.txtBatchNo = New System.Windows.Forms.TextBox()
        Me.StatusBar = New System.Windows.Forms.StatusStrip()
        Me._StatusBar_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me._StatusBar_Panel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog()
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog()
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog()
        Me.cboPaymentMethod = New System.Windows.Forms.ComboBox()
        Me.lvwInstalment = New System.Windows.Forms.ListView()
        Me._lvwInstalment_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwInstalment_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwInstalment_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwInstalment_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwInstalment_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwInstalment_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwInstalment_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmdRefresh = New System.Windows.Forms.Button()
        Me.txtFormatCurrency = New System.Windows.Forms.TextBox()
        Me.txtLeadDays = New System.Windows.Forms.TextBox()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.lblMediaType = New System.Windows.Forms.Label()
        Me.lblBar = New System.Windows.Forms.Label()
        Me.lblPaymentMethod = New System.Windows.Forms.Label()
        Me.lblLeadDays = New System.Windows.Forms.Label()
        Me.lblBatchNo = New System.Windows.Forms.Label()
        Me.Ctx_mnuPopUp = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.txtDocumentRef = New System.Windows.Forms.TextBox()
        Me.lblDocumentRef = New System.Windows.Forms.Label()
        Me.MainMenu1.SuspendLayout()
        Me.StatusBar.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPopUp})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(656, 24)
        Me.MainMenu1.TabIndex = 16
        Me.MainMenu1.Visible = False
        '
        'mnuPopUp
        '
        Me.mnuPopUp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._mnuItem_0})
        Me.mnuPopUp.Name = "mnuPopUp"
        Me.mnuPopUp.Size = New System.Drawing.Size(55, 20)
        Me.mnuPopUp.Text = "PopUp"
        Me.mnuPopUp.Visible = False
        '
        '_mnuItem_0
        '
        Me._mnuItem_0.Name = "_mnuItem_0"
        Me._mnuItem_0.Size = New System.Drawing.Size(67, 22)
        '
        'cboMediaType
        '
        Me.cboMediaType.DefaultItemId = 0
        Me.cboMediaType.FirstItem = ""
        Me.cboMediaType.ItemId = 0
        Me.cboMediaType.ListIndex = -1
        Me.cboMediaType.Location = New System.Drawing.Point(438, 12)
        Me.cboMediaType.Name = "cboMediaType"
        Me.cboMediaType.PMLookupProductFamily = 1
        Me.cboMediaType.SingleItemId = 0
        Me.cboMediaType.Size = New System.Drawing.Size(209, 21)
        Me.cboMediaType.Sorted = True
        Me.cboMediaType.TabIndex = 14
        Me.cboMediaType.TableName = "MediaType"
        Me.cboMediaType.ToolTipText = ""
        Me.cboMediaType.Visible = False
        Me.cboMediaType.WhereClause = ""
        '
        'txtBatchNo
        '
        Me.txtBatchNo.AcceptsReturn = True
        Me.txtBatchNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtBatchNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBatchNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBatchNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBatchNo.Location = New System.Drawing.Point(88, 12)
        Me.txtBatchNo.MaxLength = 0
        Me.txtBatchNo.Name = "txtBatchNo"
        Me.txtBatchNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBatchNo.Size = New System.Drawing.Size(75, 20)
        Me.txtBatchNo.TabIndex = 13
        Me.txtBatchNo.TabStop = False
        Me.txtBatchNo.Visible = False
        '
        'StatusBar
        '
        Me.StatusBar.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusBar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._StatusBar_Panel1, Me._StatusBar_Panel2})
        Me.StatusBar.Location = New System.Drawing.Point(0, 381)
        Me.StatusBar.Name = "StatusBar"
        Me.StatusBar.ShowItemToolTips = True
        Me.StatusBar.Size = New System.Drawing.Size(656, 22)
        Me.StatusBar.TabIndex = 11
        '
        '_StatusBar_Panel1
        '
        Me._StatusBar_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._StatusBar_Panel1.DoubleClickEnabled = True
        Me._StatusBar_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._StatusBar_Panel1.Name = "_StatusBar_Panel1"
        Me._StatusBar_Panel1.Size = New System.Drawing.Size(43, 22)
        Me._StatusBar_Panel1.Tag = ""
        Me._StatusBar_Panel1.Text = "Ready"
        Me._StatusBar_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_StatusBar_Panel2
        '
        Me._StatusBar_Panel2.Name = "_StatusBar_Panel2"
        Me._StatusBar_Panel2.Size = New System.Drawing.Size(0, 17)
        Me._StatusBar_Panel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cboPaymentMethod
        '
        Me.cboPaymentMethod.BackColor = System.Drawing.SystemColors.Window
        Me.cboPaymentMethod.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPaymentMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPaymentMethod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPaymentMethod.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPaymentMethod.Location = New System.Drawing.Point(439, 39)
        Me.cboPaymentMethod.Name = "cboPaymentMethod"
        Me.cboPaymentMethod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPaymentMethod.Size = New System.Drawing.Size(209, 21)
        Me.cboPaymentMethod.TabIndex = 9
        Me.cboPaymentMethod.Visible = False
        '
        'lvwInstalment
        '
        Me.lvwInstalment.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwInstalment, "")
        Me.lvwInstalment.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwInstalment_ColumnHeader_1, Me._lvwInstalment_ColumnHeader_2, Me._lvwInstalment_ColumnHeader_3, Me._lvwInstalment_ColumnHeader_4, Me._lvwInstalment_ColumnHeader_5, Me._lvwInstalment_ColumnHeader_6, Me._lvwInstalment_ColumnHeader_7})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwInstalment, False)
        Me.lvwInstalment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwInstalment.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwInstalment.FullRowSelect = True
        Me.lvwInstalment.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwInstalment, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwInstalment, "")
        Me.lvwInstalment.Location = New System.Drawing.Point(8, 88)
        Me.lvwInstalment.MultiSelect = False
        Me.lvwInstalment.Name = "lvwInstalment"
        Me.lvwInstalment.Size = New System.Drawing.Size(641, 246)
        Me.listViewHelper1.SetSmallIcons(Me.lvwInstalment, "")
        Me.listViewHelper1.SetSorted(Me.lvwInstalment, False)
        Me.listViewHelper1.SetSortKey(Me.lvwInstalment, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwInstalment, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwInstalment.TabIndex = 8
        Me.lvwInstalment.UseCompatibleStateImageBehavior = False
        Me.lvwInstalment.View = System.Windows.Forms.View.Details
        '
        '_lvwInstalment_ColumnHeader_1
        '
        Me._lvwInstalment_ColumnHeader_1.Tag = ""
        Me._lvwInstalment_ColumnHeader_1.Text = "Instalment No"
        Me._lvwInstalment_ColumnHeader_1.Width = 97
        '
        '_lvwInstalment_ColumnHeader_2
        '
        Me._lvwInstalment_ColumnHeader_2.Tag = ""
        Me._lvwInstalment_ColumnHeader_2.Text = "Status"
        Me._lvwInstalment_ColumnHeader_2.Width = 97
        '
        '_lvwInstalment_ColumnHeader_3
        '
        Me._lvwInstalment_ColumnHeader_3.Tag = ""
        Me._lvwInstalment_ColumnHeader_3.Text = "Plan Ref"
        Me._lvwInstalment_ColumnHeader_3.Width = 97
        '
        '_lvwInstalment_ColumnHeader_4
        '
        Me._lvwInstalment_ColumnHeader_4.Tag = ""
        Me._lvwInstalment_ColumnHeader_4.Text = "Account No"
        Me._lvwInstalment_ColumnHeader_4.Width = 97
        '
        '_lvwInstalment_ColumnHeader_5
        '
        Me._lvwInstalment_ColumnHeader_5.Tag = ""
        Me._lvwInstalment_ColumnHeader_5.Text = "Account Name"
        Me._lvwInstalment_ColumnHeader_5.Width = 97
        '
        '_lvwInstalment_ColumnHeader_6
        '
        Me._lvwInstalment_ColumnHeader_6.Tag = ""
        Me._lvwInstalment_ColumnHeader_6.Text = "Bank"
        Me._lvwInstalment_ColumnHeader_6.Width = 97
        '
        '_lvwInstalment_ColumnHeader_7
        '
        Me._lvwInstalment_ColumnHeader_7.Tag = ""
        Me._lvwInstalment_ColumnHeader_7.Text = "Amount"
        Me._lvwInstalment_ColumnHeader_7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwInstalment_ColumnHeader_7.Width = 97
        '
        'cmdRefresh
        '
        Me.cmdRefresh.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRefresh.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRefresh.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRefresh.Location = New System.Drawing.Point(8, 348)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRefresh.Size = New System.Drawing.Size(73, 22)
        Me.cmdRefresh.TabIndex = 7
        Me.cmdRefresh.Text = "&Refresh"
        Me.cmdRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRefresh.UseVisualStyleBackColor = False
        '
        'txtFormatCurrency
        '
        Me.txtFormatCurrency.AcceptsReturn = True
        Me.txtFormatCurrency.BackColor = System.Drawing.SystemColors.Window
        Me.txtFormatCurrency.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFormatCurrency.Enabled = False
        Me.txtFormatCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFormatCurrency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFormatCurrency.Location = New System.Drawing.Point(214, 348)
        Me.txtFormatCurrency.MaxLength = 0
        Me.txtFormatCurrency.Name = "txtFormatCurrency"
        Me.txtFormatCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFormatCurrency.Size = New System.Drawing.Size(71, 20)
        Me.txtFormatCurrency.TabIndex = 6
        Me.txtFormatCurrency.TabStop = False
        Me.txtFormatCurrency.Visible = False
        '
        'txtLeadDays
        '
        Me.txtLeadDays.AcceptsReturn = True
        Me.txtLeadDays.BackColor = System.Drawing.SystemColors.Window
        Me.txtLeadDays.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLeadDays.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLeadDays.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLeadDays.Location = New System.Drawing.Point(88, 24)
        Me.txtLeadDays.MaxLength = 0
        Me.txtLeadDays.Name = "txtLeadDays"
        Me.txtLeadDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLeadDays.Size = New System.Drawing.Size(55, 20)
        Me.txtLeadDays.TabIndex = 4
        Me.txtLeadDays.TabStop = False
        Me.txtLeadDays.Visible = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(574, 348)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 2
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(494, 348)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Enabled = False
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(414, 348)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'lblMediaType
        '
        Me.lblMediaType.BackColor = System.Drawing.Color.Transparent
        Me.lblMediaType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMediaType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMediaType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMediaType.Location = New System.Drawing.Point(358, 16)
        Me.lblMediaType.Name = "lblMediaType"
        Me.lblMediaType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMediaType.Size = New System.Drawing.Size(77, 17)
        Me.lblMediaType.TabIndex = 15
        Me.lblMediaType.Text = "Media Type:"
        Me.lblMediaType.Visible = False
        '
        'lblBar
        '
        Me.lblBar.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.lblBar.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBar.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBar.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.lblBar.Location = New System.Drawing.Point(6, 66)
        Me.lblBar.Name = "lblBar"
        Me.lblBar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBar.Size = New System.Drawing.Size(643, 19)
        Me.lblBar.TabIndex = 12
        Me.lblBar.Text = " Instalments in Batch"
        '
        'lblPaymentMethod
        '
        Me.lblPaymentMethod.BackColor = System.Drawing.Color.Transparent
        Me.lblPaymentMethod.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentMethod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentMethod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentMethod.Location = New System.Drawing.Point(353, 40)
        Me.lblPaymentMethod.Name = "lblPaymentMethod"
        Me.lblPaymentMethod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentMethod.Size = New System.Drawing.Size(83, 17)
        Me.lblPaymentMethod.TabIndex = 10
        Me.lblPaymentMethod.Text = "Export Format:"
        Me.lblPaymentMethod.Visible = False
        '
        'lblLeadDays
        '
        Me.lblLeadDays.BackColor = System.Drawing.Color.Transparent
        Me.lblLeadDays.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLeadDays.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLeadDays.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLeadDays.Location = New System.Drawing.Point(8, 27)
        Me.lblLeadDays.Name = "lblLeadDays"
        Me.lblLeadDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLeadDays.Size = New System.Drawing.Size(83, 17)
        Me.lblLeadDays.TabIndex = 5
        Me.lblLeadDays.Text = "Lead Days:"
        Me.lblLeadDays.Visible = False
        '
        'lblBatchNo
        '
        Me.lblBatchNo.BackColor = System.Drawing.Color.Transparent
        Me.lblBatchNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBatchNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBatchNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBatchNo.Location = New System.Drawing.Point(8, 14)
        Me.lblBatchNo.Name = "lblBatchNo"
        Me.lblBatchNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBatchNo.Size = New System.Drawing.Size(83, 17)
        Me.lblBatchNo.TabIndex = 3
        Me.lblBatchNo.Text = "Batch No:"
        Me.lblBatchNo.Visible = False
        '
        'Ctx_mnuPopUp
        '
        Me.Ctx_mnuPopUp.Name = "Ctx_mnuPopUp"
        Me.Ctx_mnuPopUp.Size = New System.Drawing.Size(61, 4)
        '
        'txtDocumentRef
        '
        Me.txtDocumentRef.Location = New System.Drawing.Point(252, 14)
        Me.txtDocumentRef.Name = "txtDocumentRef"
        Me.txtDocumentRef.Size = New System.Drawing.Size(100, 21)
        Me.txtDocumentRef.TabIndex = 17
        Me.txtDocumentRef.TabStop = False
        Me.txtDocumentRef.Visible = False
        '
        'lblDocumentRef
        '
        Me.lblDocumentRef.AutoSize = True
        Me.lblDocumentRef.Location = New System.Drawing.Point(191, 18)
        Me.lblDocumentRef.Name = "lblDocumentRef"
        Me.lblDocumentRef.Size = New System.Drawing.Size(57, 13)
        Me.lblDocumentRef.TabIndex = 18
        Me.lblDocumentRef.Text = "Doc Ref:"
        Me.lblDocumentRef.Visible = False
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdRefresh
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(656, 403)
        Me.Controls.Add(Me.lblDocumentRef)
        Me.Controls.Add(Me.txtDocumentRef)
        Me.Controls.Add(Me.StatusBar)
        Me.Controls.Add(Me.cboMediaType)
        Me.Controls.Add(Me.txtBatchNo)
        Me.Controls.Add(Me.cboPaymentMethod)
        Me.Controls.Add(Me.lvwInstalment)
        Me.Controls.Add(Me.txtFormatCurrency)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdRefresh)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.lblMediaType)
        Me.Controls.Add(Me.txtLeadDays)
        Me.Controls.Add(Me.MainMenu1)
        Me.Controls.Add(Me.lblBar)
        Me.Controls.Add(Me.lblPaymentMethod)
        Me.Controls.Add(Me.lblBatchNo)
        Me.Controls.Add(Me.lblLeadDays)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(10, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Instalments"
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.StatusBar.ResumeLayout(False)
        Me.StatusBar.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializemnuItem()
        Me.mnuItem(0) = _mnuItem_0
    End Sub
    Sub lvwInstalment_InitializeColumnKeys()
        Me._lvwInstalment_ColumnHeader_1.Name = ""
        Me._lvwInstalment_ColumnHeader_2.Name = ""
        Me._lvwInstalment_ColumnHeader_3.Name = ""
        Me._lvwInstalment_ColumnHeader_4.Name = ""
        Me._lvwInstalment_ColumnHeader_5.Name = ""
        Me._lvwInstalment_ColumnHeader_6.Name = ""
        Me._lvwInstalment_ColumnHeader_7.Name = ""
    End Sub
    Friend WithEvents _StatusBar_Panel2 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents txtDocumentRef As System.Windows.Forms.TextBox
    Friend WithEvents lblDocumentRef As System.Windows.Forms.Label
#End Region
End Class