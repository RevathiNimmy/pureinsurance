<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        lvwRisks_InitializeColumnKeys()
        tabMainTabPreviousTab = tabMainTab.SelectedIndex
        Form_Initialize_Renamed()
    End Sub
    Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
        Dispose(True)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Private WithEvents _Toolbar1_Button1 As System.Windows.Forms.ToolStripButton
    Private WithEvents _Toolbar1_Button2 As System.Windows.Forms.ToolStripButton
    Private WithEvents _Toolbar1_Button3 As System.Windows.Forms.ToolStripButton
    Private WithEvents _Toolbar1_Button4 As System.Windows.Forms.ToolStripButton
    Private WithEvents _Toolbar1_Button5 As System.Windows.Forms.ToolStripButton
    Public WithEvents Toolbar1 As System.Windows.Forms.ToolStrip
    Public WithEvents cmdCopy As System.Windows.Forms.Button
    Public WithEvents cmdRemoveTask As System.Windows.Forms.Button
    Public WithEvents cmdTask As System.Windows.Forms.Button
    Public WithEvents cmdPrint As System.Windows.Forms.Button
    Public WithEvents cmdEdit As System.Windows.Forms.Button
    Public WithEvents cmdNavigate As System.Windows.Forms.Button
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
    Public WithEvents lblCode As System.Windows.Forms.Label
    Public WithEvents lblDescription As System.Windows.Forms.Label
    Public WithEvents lblType As System.Windows.Forms.Label
    Public WithEvents lblSlot As System.Windows.Forms.Label
    Public WithEvents lblGroup As System.Windows.Forms.Label
    Public WithEvents lblRisk As System.Windows.Forms.Label
    Public WithEvents lblIsEditableAfterMerging As System.Windows.Forms.Label
    Public WithEvents lblIsTypeEditable As System.Windows.Forms.Label
    Public WithEvents lblSourceId As System.Windows.Forms.Label
    Public WithEvents lblPrinter As System.Windows.Forms.Label
    Public WithEvents lblChaser As System.Windows.Forms.Label
    Public WithEvents lblDocument_Filter As System.Windows.Forms.Label
    Public WithEvents lblVisibleFromWeb As System.Windows.Forms.Label
    Public WithEvents WebBrowser1 As System.Windows.Forms.WebBrowser
    Public WithEvents cboClient As System.Windows.Forms.ComboBox
    Public WithEvents cboPolicy As System.Windows.Forms.ComboBox
    Public WithEvents cboClaim As System.Windows.Forms.ComboBox
    Public WithEvents cboEffectiveDate As System.Windows.Forms.DateTimePicker
    Public WithEvents txtCode As System.Windows.Forms.TextBox
    Public WithEvents txtDescription As System.Windows.Forms.TextBox
    Public WithEvents cboType As System.Windows.Forms.ComboBox
    Public WithEvents cboGroup As System.Windows.Forms.ComboBox
    Public WithEvents cboRisk As System.Windows.Forms.ComboBox
    Public WithEvents chkIsEditableAfterMerging As System.Windows.Forms.CheckBox
    Public WithEvents chkIsTypeEditable As System.Windows.Forms.CheckBox
    Public WithEvents cboSourceId As System.Windows.Forms.ComboBox
    Public WithEvents cboPrinter As System.Windows.Forms.ComboBox
    Public WithEvents cboChaser As System.Windows.Forms.ComboBox
    Public WithEvents txtDocument_Filter As System.Windows.Forms.TextBox
    Public WithEvents chkVisibleFromWeb As System.Windows.Forms.CheckBox
    Public WithEvents chkVisibleFromClientManager As System.Windows.Forms.CheckBox
    Public WithEvents chkArchiveWithNoPrint As System.Windows.Forms.CheckBox
    Public WithEvents chkSpoolDocument As System.Windows.Forms.CheckBox
    Public WithEvents chkSendDocumentAsEmailBody As System.Windows.Forms.CheckBox
    Public WithEvents chkArchiveAsText As System.Windows.Forms.CheckBox
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Private WithEvents _lvwRisks_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRisks_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRisks_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwRisks As System.Windows.Forms.ListView
    Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public WithEvents ImageList1 As System.Windows.Forms.ImageList
    Private tabMainTabPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Toolbar1 = New System.Windows.Forms.ToolStrip()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me._Toolbar1_Button1 = New System.Windows.Forms.ToolStripButton()
        Me._Toolbar1_Button2 = New System.Windows.Forms.ToolStripButton()
        Me._Toolbar1_Button3 = New System.Windows.Forms.ToolStripButton()
        Me._Toolbar1_Button4 = New System.Windows.Forms.ToolStripButton()
        Me._Toolbar1_Button5 = New System.Windows.Forms.ToolStripButton()
        Me.cmdCopy = New System.Windows.Forms.Button()
        Me.cmdRemoveTask = New System.Windows.Forms.Button()
        Me.cmdTask = New System.Windows.Forms.Button()
        Me.cmdPrint = New System.Windows.Forms.Button()
        Me.cmdEdit = New System.Windows.Forms.Button()
        Me.cmdNavigate = New System.Windows.Forms.Button()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.gbEMailOptions = New System.Windows.Forms.GroupBox()
        Me.lblEmailInfo = New System.Windows.Forms.Label()
        Me.lblEMailAttachemntTemplates = New System.Windows.Forms.Label()
        Me.txtEMailAttachemntTemplates = New System.Windows.Forms.TextBox()
        Me.lblEMailSubDoc = New System.Windows.Forms.Label()
        Me.txtEMailSubDoc = New System.Windows.Forms.TextBox()
        Me.btnCCMTemplateSync = New System.Windows.Forms.Button()
        Me.lblCCMDocTemplate = New System.Windows.Forms.Label()
        Me.cboCCMMapping = New System.Windows.Forms.ComboBox()
        Me.cboGroup = New System.Windows.Forms.ComboBox()
        Me.cboRisk = New System.Windows.Forms.ComboBox()
        Me.cboTemplateSubGroup = New PMLookupControl.cboPMLookup()
        Me.cboTemplateGroup = New PMLookupControl.cboPMLookup()
        Me.grpWebPortal = New System.Windows.Forms.GroupBox()
        Me.chkIsInternalOnly = New System.Windows.Forms.CheckBox()
        Me.chkIsSelectedByDefault = New System.Windows.Forms.CheckBox()
        Me.grpGroupOptions = New System.Windows.Forms.GroupBox()
        Me.chkArchiveAsXML = New System.Windows.Forms.CheckBox()
        Me.lblIsTypeEditable = New System.Windows.Forms.Label()
        Me.lblIsEditableAfterMerging = New System.Windows.Forms.Label()
        Me.lblVisibleFromClientManager = New System.Windows.Forms.Label()
        Me.lblVisibleFromWeb = New System.Windows.Forms.Label()
        Me.chkVisibleFromClientManager = New System.Windows.Forms.CheckBox()
        Me.chkIsTypeEditable = New System.Windows.Forms.CheckBox()
        Me.chkIsEditableAfterMerging = New System.Windows.Forms.CheckBox()
        Me.chkVisibleFromWeb = New System.Windows.Forms.CheckBox()
        Me.chkArchiveWithNoPrint = New System.Windows.Forms.CheckBox()
        Me.chkSpoolDocument = New System.Windows.Forms.CheckBox()
        Me.chkSendDocumentAsEmailBody = New System.Windows.Forms.CheckBox()
        Me.chkArchiveAsText = New System.Windows.Forms.CheckBox()
        Me.lblEffectiveDate = New System.Windows.Forms.Label()
        Me.lblCode = New System.Windows.Forms.Label()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.lblType = New System.Windows.Forms.Label()
        Me.lblSlot = New System.Windows.Forms.Label()
        Me.lblGroup = New System.Windows.Forms.Label()
        Me.lblRisk = New System.Windows.Forms.Label()
        Me.lblSourceId = New System.Windows.Forms.Label()
        Me.lblTemplateSubGroup = New System.Windows.Forms.Label()
        Me.lblPrinter = New System.Windows.Forms.Label()
        Me.lblTemplateGroup = New System.Windows.Forms.Label()
        Me.lblChaser = New System.Windows.Forms.Label()
        Me.lblDocument_Filter = New System.Windows.Forms.Label()
        Me.WebBrowser1 = New System.Windows.Forms.WebBrowser()
        Me.cboClient = New System.Windows.Forms.ComboBox()
        Me.cboPolicy = New System.Windows.Forms.ComboBox()
        Me.cboClaim = New System.Windows.Forms.ComboBox()
        Me.cboEffectiveDate = New System.Windows.Forms.DateTimePicker()
        Me.txtCode = New System.Windows.Forms.TextBox()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.cboType = New System.Windows.Forms.ComboBox()
        Me.cboSourceId = New System.Windows.Forms.ComboBox()
        Me.cboPrinter = New System.Windows.Forms.ComboBox()
        Me.cboChaser = New System.Windows.Forms.ComboBox()
        Me.txtDocument_Filter = New System.Windows.Forms.TextBox()
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage()
        Me.lvwRisks = New System.Windows.Forms.ListView()
        Me._lvwRisks_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRisks_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRisks_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog()
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog()
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog()
        Me.Toolbar1.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.gbEMailOptions.SuspendLayout()
        Me.grpWebPortal.SuspendLayout()
        Me.grpGroupOptions.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Toolbar1
        '
        Me.Toolbar1.ImageList = Me.ImageList1
        Me.Toolbar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._Toolbar1_Button1, Me._Toolbar1_Button2, Me._Toolbar1_Button3, Me._Toolbar1_Button4, Me._Toolbar1_Button5})
        Me.Toolbar1.Location = New System.Drawing.Point(0, 0)
        Me.Toolbar1.Name = "Toolbar1"
        Me.Toolbar1.Size = New System.Drawing.Size(849, 27)
        Me.Toolbar1.TabIndex = 29
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "Policy")
        Me.ImageList1.Images.SetKeyName(1, "Edit")
        Me.ImageList1.Images.SetKeyName(2, "Print")
        Me.ImageList1.Images.SetKeyName(3, "Mail")
        Me.ImageList1.Images.SetKeyName(4, "Archive")
        Me.ImageList1.Images.SetKeyName(5, "Spool")
        Me.ImageList1.Images.SetKeyName(6, "")
        '
        '_Toolbar1_Button1
        '
        Me._Toolbar1_Button1.AutoSize = False
        Me._Toolbar1_Button1.ImageIndex = 1
        Me._Toolbar1_Button1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Toolbar1_Button1.Name = "_Toolbar1_Button1"
        Me._Toolbar1_Button1.Size = New System.Drawing.Size(24, 24)
        Me._Toolbar1_Button1.Tag = ""
        Me._Toolbar1_Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Toolbar1_Button1.ToolTipText = "Edit Document"
        '
        '_Toolbar1_Button2
        '
        Me._Toolbar1_Button2.AutoSize = False
        Me._Toolbar1_Button2.ImageIndex = 2
        Me._Toolbar1_Button2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Toolbar1_Button2.Name = "_Toolbar1_Button2"
        Me._Toolbar1_Button2.Size = New System.Drawing.Size(24, 24)
        Me._Toolbar1_Button2.Tag = ""
        Me._Toolbar1_Button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Toolbar1_Button2.ToolTipText = "Print Document"
        '
        '_Toolbar1_Button3
        '
        Me._Toolbar1_Button3.AutoSize = False
        Me._Toolbar1_Button3.ImageIndex = 3
        Me._Toolbar1_Button3.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Toolbar1_Button3.Name = "_Toolbar1_Button3"
        Me._Toolbar1_Button3.Size = New System.Drawing.Size(24, 24)
        Me._Toolbar1_Button3.Tag = ""
        Me._Toolbar1_Button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Toolbar1_Button3.ToolTipText = "Mail Document"
        '
        '_Toolbar1_Button4
        '
        Me._Toolbar1_Button4.AutoSize = False
        Me._Toolbar1_Button4.ImageIndex = 4
        Me._Toolbar1_Button4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Toolbar1_Button4.Name = "_Toolbar1_Button4"
        Me._Toolbar1_Button4.Size = New System.Drawing.Size(24, 24)
        Me._Toolbar1_Button4.Tag = ""
        Me._Toolbar1_Button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Toolbar1_Button4.ToolTipText = "Archive Document"
        '
        '_Toolbar1_Button5
        '
        Me._Toolbar1_Button5.AutoSize = False
        Me._Toolbar1_Button5.ImageIndex = 5
        Me._Toolbar1_Button5.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Toolbar1_Button5.Name = "_Toolbar1_Button5"
        Me._Toolbar1_Button5.Size = New System.Drawing.Size(24, 24)
        Me._Toolbar1_Button5.Tag = ""
        Me._Toolbar1_Button5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Toolbar1_Button5.ToolTipText = "Spool Document"
        '
        'cmdCopy
        '
        Me.cmdCopy.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCopy.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCopy.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCopy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCopy.Location = New System.Drawing.Point(432, 595)
        Me.cmdCopy.Name = "cmdCopy"
        Me.cmdCopy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCopy.Size = New System.Drawing.Size(57, 22)
        Me.cmdCopy.TabIndex = 24
        Me.cmdCopy.Text = "Copy"
        Me.cmdCopy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCopy.UseVisualStyleBackColor = False
        '
        'cmdRemoveTask
        '
        Me.cmdRemoveTask.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRemoveTask.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRemoveTask.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRemoveTask.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRemoveTask.Location = New System.Drawing.Point(328, 595)
        Me.cmdRemoveTask.Name = "cmdRemoveTask"
        Me.cmdRemoveTask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRemoveTask.Size = New System.Drawing.Size(97, 22)
        Me.cmdRemoveTask.TabIndex = 23
        Me.cmdRemoveTask.Text = "Remove Task"
        Me.cmdRemoveTask.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRemoveTask.UseVisualStyleBackColor = False
        Me.cmdRemoveTask.Visible = False
        '
        'cmdTask
        '
        Me.cmdTask.BackColor = System.Drawing.SystemColors.Control
        Me.cmdTask.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdTask.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTask.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdTask.Location = New System.Drawing.Point(248, 595)
        Me.cmdTask.Name = "cmdTask"
        Me.cmdTask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdTask.Size = New System.Drawing.Size(74, 22)
        Me.cmdTask.TabIndex = 22
        Me.cmdTask.Text = "Task"
        Me.cmdTask.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdTask.UseVisualStyleBackColor = False
        Me.cmdTask.Visible = False
        '
        'cmdPrint
        '
        Me.cmdPrint.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPrint.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPrint.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPrint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPrint.Location = New System.Drawing.Point(168, 595)
        Me.cmdPrint.Name = "cmdPrint"
        Me.cmdPrint.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPrint.Size = New System.Drawing.Size(73, 22)
        Me.cmdPrint.TabIndex = 21
        Me.cmdPrint.Text = "&Print"
        Me.cmdPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPrint.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(88, 595)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 20
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(8, 595)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 19
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(778, 595)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(65, 22)
        Me.cmdHelp.TabIndex = 27
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(706, 595)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(65, 22)
        Me.cmdCancel.TabIndex = 26
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(634, 595)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(65, 22)
        Me.cmdOK.TabIndex = 25
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.gbEMailOptions)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage1)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(139, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(4, 32)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(845, 555)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.gbEMailOptions)
        Me._tabMainTab_TabPage0.Controls.Add(Me.btnCCMTemplateSync)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCCMDocTemplate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboCCMMapping)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboGroup)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboRisk)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboTemplateSubGroup)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboTemplateGroup)
        Me._tabMainTab_TabPage0.Controls.Add(Me.grpWebPortal)
        Me._tabMainTab_TabPage0.Controls.Add(Me.grpGroupOptions)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblEffectiveDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblSlot)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblGroup)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblRisk)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblSourceId)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblTemplateSubGroup)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPrinter)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblTemplateGroup)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblChaser)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDocument_Filter)
        Me._tabMainTab_TabPage0.Controls.Add(Me.WebBrowser1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboClient)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboPolicy)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboClaim)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboEffectiveDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboSourceId)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboPrinter)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboChaser)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDocument_Filter)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(837, 529)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "&1 - Document Template"
        '
        'gbEMailOptions
        '
        Me.gbEMailOptions.Controls.Add(Me.lblEmailInfo)
        Me.gbEMailOptions.Controls.Add(Me.lblEMailAttachemntTemplates)
        Me.gbEMailOptions.Controls.Add(Me.txtEMailAttachemntTemplates)
        Me.gbEMailOptions.Controls.Add(Me.lblEMailSubDoc)
        Me.gbEMailOptions.Controls.Add(Me.txtEMailSubDoc)
        Me.gbEMailOptions.Location = New System.Drawing.Point(468, 287)
        Me.gbEMailOptions.Name = "gbEMailOptions"
        Me.gbEMailOptions.Size = New System.Drawing.Size(357, 91)
        Me.gbEMailOptions.TabIndex = 63
        Me.gbEMailOptions.TabStop = False
        Me.gbEMailOptions.Text = "EMail Attachments:"
        '
        'lblEmailInfo
        '
        Me.lblEmailInfo.BackColor = System.Drawing.SystemColors.Control
        Me.lblEmailInfo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEmailInfo.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEmailInfo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEmailInfo.Location = New System.Drawing.Point(147, 70)
        Me.lblEmailInfo.Name = "lblEmailInfo"
        Me.lblEmailInfo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEmailInfo.Size = New System.Drawing.Size(205, 18)
        Me.lblEmailInfo.TabIndex = 38
        Me.lblEmailInfo.Text = "(Template codes with comma separator)"
        '
        'lblEMailAttachemntTemplates
        '
        Me.lblEMailAttachemntTemplates.BackColor = System.Drawing.SystemColors.Control
        Me.lblEMailAttachemntTemplates.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEMailAttachemntTemplates.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEMailAttachemntTemplates.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEMailAttachemntTemplates.Location = New System.Drawing.Point(6, 49)
        Me.lblEMailAttachemntTemplates.Name = "lblEMailAttachemntTemplates"
        Me.lblEMailAttachemntTemplates.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEMailAttachemntTemplates.Size = New System.Drawing.Size(143, 18)
        Me.lblEMailAttachemntTemplates.TabIndex = 37
        Me.lblEMailAttachemntTemplates.Text = "Attachment Template:"
        '
        'txtEMailAttachemntTemplates
        '
        Me.txtEMailAttachemntTemplates.AcceptsReturn = True
        Me.txtEMailAttachemntTemplates.BackColor = System.Drawing.SystemColors.Window
        Me.txtEMailAttachemntTemplates.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEMailAttachemntTemplates.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEMailAttachemntTemplates.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEMailAttachemntTemplates.Location = New System.Drawing.Point(151, 46)
        Me.txtEMailAttachemntTemplates.MaxLength = 0
        Me.txtEMailAttachemntTemplates.Name = "txtEMailAttachemntTemplates"
        Me.txtEMailAttachemntTemplates.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEMailAttachemntTemplates.Size = New System.Drawing.Size(186, 21)
        Me.txtEMailAttachemntTemplates.TabIndex = 36
        '
        'lblEMailSubDoc
        '
        Me.lblEMailSubDoc.BackColor = System.Drawing.SystemColors.Control
        Me.lblEMailSubDoc.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEMailSubDoc.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEMailSubDoc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEMailSubDoc.Location = New System.Drawing.Point(6, 22)
        Me.lblEMailSubDoc.Name = "lblEMailSubDoc"
        Me.lblEMailSubDoc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEMailSubDoc.Size = New System.Drawing.Size(127, 17)
        Me.lblEMailSubDoc.TabIndex = 35
        Me.lblEMailSubDoc.Text = "Subject Template:"
        '
        'txtEMailSubDoc
        '
        Me.txtEMailSubDoc.AcceptsReturn = True
        Me.txtEMailSubDoc.BackColor = System.Drawing.SystemColors.Window
        Me.txtEMailSubDoc.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEMailSubDoc.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEMailSubDoc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEMailSubDoc.Location = New System.Drawing.Point(151, 19)
        Me.txtEMailSubDoc.MaxLength = 0
        Me.txtEMailSubDoc.Name = "txtEMailSubDoc"
        Me.txtEMailSubDoc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEMailSubDoc.Size = New System.Drawing.Size(186, 21)
        Me.txtEMailSubDoc.TabIndex = 34
        '
        'btnCCMTemplateSync
        '
        Me.btnCCMTemplateSync.Location = New System.Drawing.Point(539, 197)
        Me.btnCCMTemplateSync.Name = "btnCCMTemplateSync"
        Me.btnCCMTemplateSync.Size = New System.Drawing.Size(143, 23)
        Me.btnCCMTemplateSync.TabIndex = 67
        Me.btnCCMTemplateSync.Text = "KCM Template Sync"
        Me.btnCCMTemplateSync.UseVisualStyleBackColor = True
        '
        'lblCCMDocTemplate
        '
        Me.lblCCMDocTemplate.AutoSize = True
        Me.lblCCMDocTemplate.Location = New System.Drawing.Point(10, 202)
        Me.lblCCMDocTemplate.Name = "lblCCMDocTemplate"
        Me.lblCCMDocTemplate.Size = New System.Drawing.Size(211, 13)
        Me.lblCCMDocTemplate.TabIndex = 66
        Me.lblCCMDocTemplate.Text = "KCM Document Template Mapping: "
        '
        'cboCCMMapping
        '
        Me.cboCCMMapping.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboCCMMapping.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboCCMMapping.FormattingEnabled = True
        Me.cboCCMMapping.Location = New System.Drawing.Point(224, 199)
        Me.cboCCMMapping.Name = "cboCCMMapping"
        Me.cboCCMMapping.Size = New System.Drawing.Size(290, 21)
        Me.cboCCMMapping.TabIndex = 65
        '
        'cboGroup
        '
        Me.cboGroup.BackColor = System.Drawing.SystemColors.Window
        Me.cboGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboGroup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboGroup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboGroup.Location = New System.Drawing.Point(520, 84)
        Me.cboGroup.Name = "cboGroup"
        Me.cboGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboGroup.Size = New System.Drawing.Size(169, 21)
        Me.cboGroup.TabIndex = 12
        '
        'cboRisk
        '
        Me.cboRisk.BackColor = System.Drawing.SystemColors.Window
        Me.cboRisk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboRisk.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRisk.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRisk.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboRisk.Location = New System.Drawing.Point(520, 111)
        Me.cboRisk.Name = "cboRisk"
        Me.cboRisk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboRisk.Size = New System.Drawing.Size(169, 21)
        Me.cboRisk.TabIndex = 8
        '
        'cboTemplateSubGroup
        '
        Me.cboTemplateSubGroup.DefaultItemId = 0
        Me.cboTemplateSubGroup.FirstItem = ""
        Me.cboTemplateSubGroup.ItemId = 0
        Me.cboTemplateSubGroup.ListIndex = -1
        Me.cboTemplateSubGroup.Location = New System.Drawing.Point(520, 135)
        Me.cboTemplateSubGroup.Name = "cboTemplateSubGroup"
        Me.cboTemplateSubGroup.PMLookupProductFamily = 1
        Me.cboTemplateSubGroup.SingleItemId = 0
        Me.cboTemplateSubGroup.Size = New System.Drawing.Size(169, 21)
        Me.cboTemplateSubGroup.SortColumnName = ""
        Me.cboTemplateSubGroup.Sorted = True
        Me.cboTemplateSubGroup.TabIndex = 62
        Me.cboTemplateSubGroup.TableName = "Document_Template_Sub_Group"
        Me.cboTemplateSubGroup.ToolTipText = ""
        Me.cboTemplateSubGroup.WhereClause = ""
        '
        'cboTemplateGroup
        '
        Me.cboTemplateGroup.DefaultItemId = 0
        Me.cboTemplateGroup.FirstItem = ""
        Me.cboTemplateGroup.ItemId = 0
        Me.cboTemplateGroup.ListIndex = -1
        Me.cboTemplateGroup.Location = New System.Drawing.Point(112, 137)
        Me.cboTemplateGroup.Name = "cboTemplateGroup"
        Me.cboTemplateGroup.PMLookupProductFamily = 1
        Me.cboTemplateGroup.SingleItemId = 0
        Me.cboTemplateGroup.Size = New System.Drawing.Size(169, 21)
        Me.cboTemplateGroup.SortColumnName = ""
        Me.cboTemplateGroup.Sorted = True
        Me.cboTemplateGroup.TabIndex = 62
        Me.cboTemplateGroup.TableName = "Document_Template_Group"
        Me.cboTemplateGroup.ToolTipText = ""
        Me.cboTemplateGroup.WhereClause = ""
        '
        'grpWebPortal
        '
        Me.grpWebPortal.Controls.Add(Me.chkIsInternalOnly)
        Me.grpWebPortal.Controls.Add(Me.chkIsSelectedByDefault)
        Me.grpWebPortal.Location = New System.Drawing.Point(468, 226)
        Me.grpWebPortal.Name = "grpWebPortal"
        Me.grpWebPortal.Size = New System.Drawing.Size(360, 56)
        Me.grpWebPortal.TabIndex = 50
        Me.grpWebPortal.TabStop = False
        Me.grpWebPortal.Text = "Web Portal Defaults"
        '
        'chkIsInternalOnly
        '
        Me.chkIsInternalOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsInternalOnly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsInternalOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsInternalOnly.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsInternalOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsInternalOnly.Location = New System.Drawing.Point(7, 33)
        Me.chkIsInternalOnly.Name = "chkIsInternalOnly"
        Me.chkIsInternalOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsInternalOnly.Size = New System.Drawing.Size(207, 17)
        Me.chkIsInternalOnly.TabIndex = 47
        Me.chkIsInternalOnly.Text = "Internal Only"
        Me.chkIsInternalOnly.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkIsInternalOnly.UseVisualStyleBackColor = False
        '
        'chkIsSelectedByDefault
        '
        Me.chkIsSelectedByDefault.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsSelectedByDefault.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsSelectedByDefault.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsSelectedByDefault.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsSelectedByDefault.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsSelectedByDefault.Location = New System.Drawing.Point(7, 16)
        Me.chkIsSelectedByDefault.Name = "chkIsSelectedByDefault"
        Me.chkIsSelectedByDefault.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsSelectedByDefault.Size = New System.Drawing.Size(207, 17)
        Me.chkIsSelectedByDefault.TabIndex = 47
        Me.chkIsSelectedByDefault.Text = "Selected by default"
        Me.chkIsSelectedByDefault.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkIsSelectedByDefault.UseVisualStyleBackColor = False
        '
        'grpGroupOptions
        '
        Me.grpGroupOptions.Controls.Add(Me.chkArchiveAsXML)
        Me.grpGroupOptions.Controls.Add(Me.lblIsTypeEditable)
        Me.grpGroupOptions.Controls.Add(Me.lblIsEditableAfterMerging)
        Me.grpGroupOptions.Controls.Add(Me.lblVisibleFromClientManager)
        Me.grpGroupOptions.Controls.Add(Me.lblVisibleFromWeb)
        Me.grpGroupOptions.Controls.Add(Me.chkVisibleFromClientManager)
        Me.grpGroupOptions.Controls.Add(Me.chkIsTypeEditable)
        Me.grpGroupOptions.Controls.Add(Me.chkIsEditableAfterMerging)
        Me.grpGroupOptions.Controls.Add(Me.chkVisibleFromWeb)
        Me.grpGroupOptions.Controls.Add(Me.chkArchiveWithNoPrint)
        Me.grpGroupOptions.Controls.Add(Me.chkSpoolDocument)
        Me.grpGroupOptions.Controls.Add(Me.chkSendDocumentAsEmailBody)
        Me.grpGroupOptions.Controls.Add(Me.chkArchiveAsText)
        Me.grpGroupOptions.Location = New System.Drawing.Point(9, 226)
        Me.grpGroupOptions.Name = "grpGroupOptions"
        Me.grpGroupOptions.Size = New System.Drawing.Size(453, 152)
        Me.grpGroupOptions.TabIndex = 49
        Me.grpGroupOptions.TabStop = False
        Me.grpGroupOptions.Text = "Additional Options"
        '
        'chkArchiveAsXML
        '
        Me.chkArchiveAsXML.BackColor = System.Drawing.SystemColors.Control
        Me.chkArchiveAsXML.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkArchiveAsXML.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkArchiveAsXML.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkArchiveAsXML.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkArchiveAsXML.Location = New System.Drawing.Point(225, 87)
        Me.chkArchiveAsXML.Name = "chkArchiveAsXML"
        Me.chkArchiveAsXML.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkArchiveAsXML.Size = New System.Drawing.Size(207, 17)
        Me.chkArchiveAsXML.TabIndex = 48
        Me.chkArchiveAsXML.Text = "Archive as XML?"
        Me.chkArchiveAsXML.UseVisualStyleBackColor = False
        '
        'lblIsTypeEditable
        '
        Me.lblIsTypeEditable.BackColor = System.Drawing.SystemColors.Control
        Me.lblIsTypeEditable.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIsTypeEditable.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIsTypeEditable.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIsTypeEditable.Location = New System.Drawing.Point(14, 17)
        Me.lblIsTypeEditable.Name = "lblIsTypeEditable"
        Me.lblIsTypeEditable.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIsTypeEditable.Size = New System.Drawing.Size(137, 17)
        Me.lblIsTypeEditable.TabIndex = 31
        Me.lblIsTypeEditable.Text = "Type editable?"
        '
        'lblIsEditableAfterMerging
        '
        Me.lblIsEditableAfterMerging.BackColor = System.Drawing.SystemColors.Control
        Me.lblIsEditableAfterMerging.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIsEditableAfterMerging.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIsEditableAfterMerging.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIsEditableAfterMerging.Location = New System.Drawing.Point(14, 34)
        Me.lblIsEditableAfterMerging.Name = "lblIsEditableAfterMerging"
        Me.lblIsEditableAfterMerging.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIsEditableAfterMerging.Size = New System.Drawing.Size(149, 16)
        Me.lblIsEditableAfterMerging.TabIndex = 34
        Me.lblIsEditableAfterMerging.Text = "Editable after merging?" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'lblVisibleFromClientManager
        '
        Me.lblVisibleFromClientManager.BackColor = System.Drawing.SystemColors.Control
        Me.lblVisibleFromClientManager.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblVisibleFromClientManager.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVisibleFromClientManager.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblVisibleFromClientManager.Location = New System.Drawing.Point(14, 69)
        Me.lblVisibleFromClientManager.Name = "lblVisibleFromClientManager"
        Me.lblVisibleFromClientManager.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblVisibleFromClientManager.Size = New System.Drawing.Size(168, 16)
        Me.lblVisibleFromClientManager.TabIndex = 35
        Me.lblVisibleFromClientManager.Text = "Visible from Client Manager?"
        '
        'lblVisibleFromWeb
        '
        Me.lblVisibleFromWeb.BackColor = System.Drawing.SystemColors.Control
        Me.lblVisibleFromWeb.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblVisibleFromWeb.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVisibleFromWeb.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblVisibleFromWeb.Location = New System.Drawing.Point(14, 51)
        Me.lblVisibleFromWeb.Name = "lblVisibleFromWeb"
        Me.lblVisibleFromWeb.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblVisibleFromWeb.Size = New System.Drawing.Size(137, 17)
        Me.lblVisibleFromWeb.TabIndex = 35
        Me.lblVisibleFromWeb.Text = "Visible from Web?"
        '
        'chkVisibleFromClientManager
        '
        Me.chkVisibleFromClientManager.BackColor = System.Drawing.SystemColors.Control
        Me.chkVisibleFromClientManager.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkVisibleFromClientManager.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkVisibleFromClientManager.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkVisibleFromClientManager.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkVisibleFromClientManager.Location = New System.Drawing.Point(185, 68)
        Me.chkVisibleFromClientManager.Margin = New System.Windows.Forms.Padding(0)
        Me.chkVisibleFromClientManager.Name = "chkVisibleFromClientManager"
        Me.chkVisibleFromClientManager.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkVisibleFromClientManager.Size = New System.Drawing.Size(17, 17)
        Me.chkVisibleFromClientManager.TabIndex = 11
        Me.chkVisibleFromClientManager.UseVisualStyleBackColor = False
        '
        'chkIsTypeEditable
        '
        Me.chkIsTypeEditable.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsTypeEditable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsTypeEditable.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsTypeEditable.Enabled = False
        Me.chkIsTypeEditable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsTypeEditable.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsTypeEditable.Location = New System.Drawing.Point(185, 16)
        Me.chkIsTypeEditable.Name = "chkIsTypeEditable"
        Me.chkIsTypeEditable.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsTypeEditable.Size = New System.Drawing.Size(17, 17)
        Me.chkIsTypeEditable.TabIndex = 2
        Me.chkIsTypeEditable.UseVisualStyleBackColor = False
        '
        'chkIsEditableAfterMerging
        '
        Me.chkIsEditableAfterMerging.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsEditableAfterMerging.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsEditableAfterMerging.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsEditableAfterMerging.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsEditableAfterMerging.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsEditableAfterMerging.Location = New System.Drawing.Point(185, 33)
        Me.chkIsEditableAfterMerging.Name = "chkIsEditableAfterMerging"
        Me.chkIsEditableAfterMerging.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsEditableAfterMerging.Size = New System.Drawing.Size(17, 17)
        Me.chkIsEditableAfterMerging.TabIndex = 7
        Me.chkIsEditableAfterMerging.UseVisualStyleBackColor = False
        '
        'chkVisibleFromWeb
        '
        Me.chkVisibleFromWeb.BackColor = System.Drawing.SystemColors.Control
        Me.chkVisibleFromWeb.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkVisibleFromWeb.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkVisibleFromWeb.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkVisibleFromWeb.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkVisibleFromWeb.Location = New System.Drawing.Point(185, 51)
        Me.chkVisibleFromWeb.Name = "chkVisibleFromWeb"
        Me.chkVisibleFromWeb.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkVisibleFromWeb.Size = New System.Drawing.Size(17, 15)
        Me.chkVisibleFromWeb.TabIndex = 10
        Me.chkVisibleFromWeb.UseVisualStyleBackColor = False
        '
        'chkArchiveWithNoPrint
        '
        Me.chkArchiveWithNoPrint.BackColor = System.Drawing.SystemColors.Control
        Me.chkArchiveWithNoPrint.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkArchiveWithNoPrint.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkArchiveWithNoPrint.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkArchiveWithNoPrint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkArchiveWithNoPrint.Location = New System.Drawing.Point(225, 17)
        Me.chkArchiveWithNoPrint.Name = "chkArchiveWithNoPrint"
        Me.chkArchiveWithNoPrint.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkArchiveWithNoPrint.Size = New System.Drawing.Size(207, 16)
        Me.chkArchiveWithNoPrint.TabIndex = 44
        Me.chkArchiveWithNoPrint.Text = "Archive with No Print ?"
        Me.chkArchiveWithNoPrint.UseVisualStyleBackColor = False
        '
        'chkSpoolDocument
        '
        Me.chkSpoolDocument.BackColor = System.Drawing.SystemColors.Control
        Me.chkSpoolDocument.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkSpoolDocument.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSpoolDocument.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSpoolDocument.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSpoolDocument.Location = New System.Drawing.Point(225, 50)
        Me.chkSpoolDocument.Name = "chkSpoolDocument"
        Me.chkSpoolDocument.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSpoolDocument.Size = New System.Drawing.Size(207, 17)
        Me.chkSpoolDocument.TabIndex = 45
        Me.chkSpoolDocument.Text = "Automatically Spool Document?"
        Me.chkSpoolDocument.UseVisualStyleBackColor = False
        '
        'chkSendDocumentAsEmailBody
        '
        Me.chkSendDocumentAsEmailBody.BackColor = System.Drawing.SystemColors.Control
        Me.chkSendDocumentAsEmailBody.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkSendDocumentAsEmailBody.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSendDocumentAsEmailBody.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSendDocumentAsEmailBody.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSendDocumentAsEmailBody.Location = New System.Drawing.Point(225, 68)
        Me.chkSendDocumentAsEmailBody.Name = "chkSendDocumentAsEmailBody"
        Me.chkSendDocumentAsEmailBody.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSendDocumentAsEmailBody.Size = New System.Drawing.Size(207, 17)
        Me.chkSendDocumentAsEmailBody.TabIndex = 46
        Me.chkSendDocumentAsEmailBody.Text = "Send Document As E-Mail Body?"
        Me.chkSendDocumentAsEmailBody.UseVisualStyleBackColor = False
        '
        'chkArchiveAsText
        '
        Me.chkArchiveAsText.BackColor = System.Drawing.SystemColors.Control
        Me.chkArchiveAsText.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkArchiveAsText.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkArchiveAsText.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkArchiveAsText.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkArchiveAsText.Location = New System.Drawing.Point(225, 33)
        Me.chkArchiveAsText.Name = "chkArchiveAsText"
        Me.chkArchiveAsText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkArchiveAsText.Size = New System.Drawing.Size(207, 17)
        Me.chkArchiveAsText.TabIndex = 47
        Me.chkArchiveAsText.Text = "Archive as Text?"
        Me.chkArchiveAsText.UseVisualStyleBackColor = False
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.AutoSize = True
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(422, 18)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(92, 13)
        Me.lblEffectiveDate.TabIndex = 39
        Me.lblEffectiveDate.Text = "Effective Date:"
        Me.lblEffectiveDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblEffectiveDate.Visible = False
        '
        'lblCode
        '
        Me.lblCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCode.Location = New System.Drawing.Point(10, 15)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCode.Size = New System.Drawing.Size(81, 17)
        Me.lblCode.TabIndex = 30
        Me.lblCode.Text = "Code:"
        '
        'lblDescription
        '
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(10, 39)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(81, 17)
        Me.lblDescription.TabIndex = 33
        Me.lblDescription.Text = "Description:"
        '
        'lblType
        '
        Me.lblType.BackColor = System.Drawing.SystemColors.Control
        Me.lblType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblType.Location = New System.Drawing.Point(10, 63)
        Me.lblType.Name = "lblType"
        Me.lblType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblType.Size = New System.Drawing.Size(81, 17)
        Me.lblType.TabIndex = 37
        Me.lblType.Text = "Type:"
        '
        'lblSlot
        '
        Me.lblSlot.BackColor = System.Drawing.SystemColors.Control
        Me.lblSlot.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSlot.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSlot.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSlot.Location = New System.Drawing.Point(476, 60)
        Me.lblSlot.Name = "lblSlot"
        Me.lblSlot.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSlot.Size = New System.Drawing.Size(41, 17)
        Me.lblSlot.TabIndex = 32
        Me.lblSlot.Text = "Slot:"
        Me.lblSlot.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblGroup
        '
        Me.lblGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGroup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGroup.Location = New System.Drawing.Point(468, 87)
        Me.lblGroup.Name = "lblGroup"
        Me.lblGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGroup.Size = New System.Drawing.Size(53, 18)
        Me.lblGroup.TabIndex = 38
        Me.lblGroup.Text = "Group:"
        Me.lblGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblRisk
        '
        Me.lblRisk.BackColor = System.Drawing.SystemColors.Control
        Me.lblRisk.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRisk.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRisk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRisk.Location = New System.Drawing.Point(469, 114)
        Me.lblRisk.Name = "lblRisk"
        Me.lblRisk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRisk.Size = New System.Drawing.Size(52, 18)
        Me.lblRisk.TabIndex = 36
        Me.lblRisk.Text = "Risk:"
        Me.lblRisk.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblSourceId
        '
        Me.lblSourceId.BackColor = System.Drawing.SystemColors.Control
        Me.lblSourceId.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSourceId.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSourceId.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSourceId.Location = New System.Drawing.Point(459, 39)
        Me.lblSourceId.Name = "lblSourceId"
        Me.lblSourceId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSourceId.Size = New System.Drawing.Size(58, 14)
        Me.lblSourceId.TabIndex = 40
        Me.lblSourceId.Text = "Branch:"
        Me.lblSourceId.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblTemplateSubGroup
        '
        Me.lblTemplateSubGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblTemplateSubGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTemplateSubGroup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemplateSubGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTemplateSubGroup.Location = New System.Drawing.Point(381, 137)
        Me.lblTemplateSubGroup.Name = "lblTemplateSubGroup"
        Me.lblTemplateSubGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTemplateSubGroup.Size = New System.Drawing.Size(140, 19)
        Me.lblTemplateSubGroup.TabIndex = 43
        Me.lblTemplateSubGroup.Text = "Template Sub Group:"
        Me.lblTemplateSubGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPrinter
        '
        Me.lblPrinter.BackColor = System.Drawing.SystemColors.Control
        Me.lblPrinter.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPrinter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPrinter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPrinter.Location = New System.Drawing.Point(10, 90)
        Me.lblPrinter.Name = "lblPrinter"
        Me.lblPrinter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPrinter.Size = New System.Drawing.Size(81, 17)
        Me.lblPrinter.TabIndex = 41
        Me.lblPrinter.Text = "Printer:"
        '
        'lblTemplateGroup
        '
        Me.lblTemplateGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblTemplateGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTemplateGroup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemplateGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTemplateGroup.Location = New System.Drawing.Point(10, 139)
        Me.lblTemplateGroup.Name = "lblTemplateGroup"
        Me.lblTemplateGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTemplateGroup.Size = New System.Drawing.Size(123, 17)
        Me.lblTemplateGroup.TabIndex = 43
        Me.lblTemplateGroup.Text = "Template Group:"
        '
        'lblChaser
        '
        Me.lblChaser.BackColor = System.Drawing.SystemColors.Control
        Me.lblChaser.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChaser.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChaser.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblChaser.Location = New System.Drawing.Point(10, 176)
        Me.lblChaser.Name = "lblChaser"
        Me.lblChaser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChaser.Size = New System.Drawing.Size(97, 17)
        Me.lblChaser.TabIndex = 43
        Me.lblChaser.Text = "Chaser For:"
        '
        'lblDocument_Filter
        '
        Me.lblDocument_Filter.BackColor = System.Drawing.SystemColors.Control
        Me.lblDocument_Filter.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDocument_Filter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocument_Filter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDocument_Filter.Location = New System.Drawing.Point(10, 113)
        Me.lblDocument_Filter.Name = "lblDocument_Filter"
        Me.lblDocument_Filter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDocument_Filter.Size = New System.Drawing.Size(41, 17)
        Me.lblDocument_Filter.TabIndex = 42
        Me.lblDocument_Filter.Text = "Filter:"
        '
        'WebBrowser1
        '
        Me.WebBrowser1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.WebBrowser1.Location = New System.Drawing.Point(9, 384)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.Size = New System.Drawing.Size(820, 143)
        Me.WebBrowser1.TabIndex = 18
        Me.WebBrowser1.Visible = False
        '
        'cboClient
        '
        Me.cboClient.BackColor = System.Drawing.SystemColors.Window
        Me.cboClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboClient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboClient.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboClient.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboClient.Location = New System.Drawing.Point(520, 59)
        Me.cboClient.Name = "cboClient"
        Me.cboClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboClient.Size = New System.Drawing.Size(169, 21)
        Me.cboClient.TabIndex = 5
        '
        'cboPolicy
        '
        Me.cboPolicy.BackColor = System.Drawing.SystemColors.Window
        Me.cboPolicy.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPolicy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPolicy.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPolicy.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPolicy.Location = New System.Drawing.Point(520, 59)
        Me.cboPolicy.Name = "cboPolicy"
        Me.cboPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPolicy.Size = New System.Drawing.Size(169, 21)
        Me.cboPolicy.TabIndex = 4
        '
        'cboClaim
        '
        Me.cboClaim.BackColor = System.Drawing.SystemColors.Window
        Me.cboClaim.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboClaim.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboClaim.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboClaim.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboClaim.Location = New System.Drawing.Point(520, 59)
        Me.cboClaim.Name = "cboClaim"
        Me.cboClaim.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboClaim.Size = New System.Drawing.Size(169, 21)
        Me.cboClaim.TabIndex = 3
        '
        'cboEffectiveDate
        '
        Me.cboEffectiveDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.cboEffectiveDate.Location = New System.Drawing.Point(520, 12)
        Me.cboEffectiveDate.Name = "cboEffectiveDate"
        Me.cboEffectiveDate.Size = New System.Drawing.Size(169, 21)
        Me.cboEffectiveDate.TabIndex = 13
        Me.cboEffectiveDate.Visible = False
        '
        'txtCode
        '
        Me.txtCode.AcceptsReturn = True
        Me.txtCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCode.Enabled = False
        Me.txtCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCode.Location = New System.Drawing.Point(112, 12)
        Me.txtCode.MaxLength = 10
        Me.txtCode.Name = "txtCode"
        Me.txtCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCode.Size = New System.Drawing.Size(169, 21)
        Me.txtCode.TabIndex = 1
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(112, 36)
        Me.txtDescription.MaxLength = 0
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(169, 21)
        Me.txtDescription.TabIndex = 6
        '
        'cboType
        '
        Me.cboType.BackColor = System.Drawing.SystemColors.Window
        Me.cboType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboType.Location = New System.Drawing.Point(112, 60)
        Me.cboType.Name = "cboType"
        Me.cboType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboType.Size = New System.Drawing.Size(169, 21)
        Me.cboType.TabIndex = 9
        '
        'cboSourceId
        '
        Me.cboSourceId.BackColor = System.Drawing.SystemColors.Window
        Me.cboSourceId.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSourceId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSourceId.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSourceId.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSourceId.Location = New System.Drawing.Point(520, 35)
        Me.cboSourceId.Name = "cboSourceId"
        Me.cboSourceId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSourceId.Size = New System.Drawing.Size(169, 21)
        Me.cboSourceId.TabIndex = 14
        '
        'cboPrinter
        '
        Me.cboPrinter.BackColor = System.Drawing.SystemColors.Window
        Me.cboPrinter.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPrinter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPrinter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPrinter.Location = New System.Drawing.Point(112, 86)
        Me.cboPrinter.Name = "cboPrinter"
        Me.cboPrinter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPrinter.Size = New System.Drawing.Size(350, 21)
        Me.cboPrinter.TabIndex = 15
        '
        'cboChaser
        '
        Me.cboChaser.BackColor = System.Drawing.SystemColors.Window
        Me.cboChaser.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboChaser.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboChaser.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboChaser.Location = New System.Drawing.Point(224, 172)
        Me.cboChaser.Name = "cboChaser"
        Me.cboChaser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboChaser.Size = New System.Drawing.Size(290, 21)
        Me.cboChaser.TabIndex = 17
        '
        'txtDocument_Filter
        '
        Me.txtDocument_Filter.AcceptsReturn = True
        Me.txtDocument_Filter.BackColor = System.Drawing.SystemColors.Window
        Me.txtDocument_Filter.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDocument_Filter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDocument_Filter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDocument_Filter.Location = New System.Drawing.Point(112, 110)
        Me.txtDocument_Filter.MaxLength = 50
        Me.txtDocument_Filter.Name = "txtDocument_Filter"
        Me.txtDocument_Filter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDocument_Filter.Size = New System.Drawing.Size(350, 21)
        Me.txtDocument_Filter.TabIndex = 16
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.lvwRisks)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(837, 529)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "&2 - Risks"
        '
        'lvwRisks
        '
        Me.lvwRisks.BackColor = System.Drawing.SystemColors.Window
        Me.lvwRisks.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwRisks_ColumnHeader_1, Me._lvwRisks_ColumnHeader_2, Me._lvwRisks_ColumnHeader_3})
        Me.lvwRisks.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwRisks.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwRisks.LabelEdit = True
        Me.lvwRisks.Location = New System.Drawing.Point(18, 22)
        Me.lvwRisks.Name = "lvwRisks"
        Me.lvwRisks.Size = New System.Drawing.Size(661, 337)
        Me.lvwRisks.TabIndex = 28
        Me.lvwRisks.UseCompatibleStateImageBehavior = False
        Me.lvwRisks.View = System.Windows.Forms.View.Details
        '
        '_lvwRisks_ColumnHeader_1
        '
        Me._lvwRisks_ColumnHeader_1.Tag = ""
        Me._lvwRisks_ColumnHeader_1.Text = ""
        Me._lvwRisks_ColumnHeader_1.Width = 48
        '
        '_lvwRisks_ColumnHeader_2
        '
        Me._lvwRisks_ColumnHeader_2.Tag = ""
        Me._lvwRisks_ColumnHeader_2.Text = ""
        Me._lvwRisks_ColumnHeader_2.Width = 67
        '
        '_lvwRisks_ColumnHeader_3
        '
        Me._lvwRisks_ColumnHeader_3.Tag = ""
        Me._lvwRisks_ColumnHeader_3.Text = ""
        Me._lvwRisks_ColumnHeader_3.Width = 481
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(849, 658)
        Me.Controls.Add(Me.Toolbar1)
        Me.Controls.Add(Me.cmdCopy)
        Me.Controls.Add(Me.cmdRemoveTask)
        Me.Controls.Add(Me.cmdTask)
        Me.Controls.Add(Me.cmdPrint)
        Me.Controls.Add(Me.cmdEdit)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Document Templates"
        Me.Toolbar1.ResumeLayout(False)
        Me.Toolbar1.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.gbEMailOptions.ResumeLayout(False)
        Me.gbEMailOptions.PerformLayout()
        Me.grpWebPortal.ResumeLayout(False)
        Me.grpGroupOptions.ResumeLayout(False)
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub lvwRisks_InitializeColumnKeys()
        Me._lvwRisks_ColumnHeader_1.Name = ""
        Me._lvwRisks_ColumnHeader_2.Name = ""
        Me._lvwRisks_ColumnHeader_3.Name = ""
    End Sub
    Friend WithEvents grpGroupOptions As System.Windows.Forms.GroupBox
    Friend WithEvents grpWebPortal As System.Windows.Forms.GroupBox
    Public WithEvents lblVisibleFromClientManager As System.Windows.Forms.Label
    Public WithEvents chkIsSelectedByDefault As System.Windows.Forms.CheckBox
    Public WithEvents chkIsInternalOnly As System.Windows.Forms.CheckBox
    Friend WithEvents cboTemplateSubGroup As PMLookupControl.cboPMLookup
    Friend WithEvents cboTemplateGroup As PMLookupControl.cboPMLookup
    Public WithEvents lblTemplateSubGroup As System.Windows.Forms.Label
    Public WithEvents lblTemplateGroup As System.Windows.Forms.Label
    Public WithEvents chkArchiveAsXML As System.Windows.Forms.CheckBox
    Friend WithEvents lblCCMDocTemplate As System.Windows.Forms.Label
    Friend WithEvents cboCCMMapping As System.Windows.Forms.ComboBox
    Friend WithEvents btnCCMTemplateSync As System.Windows.Forms.Button
    Friend WithEvents gbEMailOptions As System.Windows.Forms.GroupBox
    Public WithEvents lblEmailInfo As System.Windows.Forms.Label
    Public WithEvents lblEMailAttachemntTemplates As System.Windows.Forms.Label
    Public WithEvents txtEMailAttachemntTemplates As System.Windows.Forms.TextBox
    Public WithEvents lblEMailSubDoc As System.Windows.Forms.Label
    Public WithEvents txtEMailSubDoc As System.Windows.Forms.TextBox
#End Region
End Class