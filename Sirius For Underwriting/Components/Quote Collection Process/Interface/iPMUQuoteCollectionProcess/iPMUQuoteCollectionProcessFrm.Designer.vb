<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
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
	Public WithEvents cmdAddTask As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdMakePayment As System.Windows.Forms.Button
	Public WithEvents cmdSelectAlll As System.Windows.Forms.Button
	Private WithEvents _stbstatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents stbstatus As System.Windows.Forms.StatusStrip
	Public WithEvents lvwsearchdetails As System.Windows.Forms.ListView
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents txtProduct As System.Windows.Forms.TextBox
	Public WithEvents cmdProduct As System.Windows.Forms.Button
	Public WithEvents cmdQuoteRef As System.Windows.Forms.Button
	Public WithEvents cmdClient As System.Windows.Forms.Button
	Public WithEvents cmdAgent As System.Windows.Forms.Button
	Public WithEvents txtRiskIndex As System.Windows.Forms.TextBox
	Public WithEvents chkDirectBusiness As System.Windows.Forms.CheckBox
	Public WithEvents cmdNewSearch As System.Windows.Forms.Button
	Public WithEvents cmdFindNow As System.Windows.Forms.Button
	Public WithEvents dtpEndDate As System.Windows.Forms.DateTimePicker
	Public WithEvents dtpStartDate As System.Windows.Forms.DateTimePicker
	Public WithEvents lblEndDate As System.Windows.Forms.Label
	Public WithEvents lblStartDate As System.Windows.Forms.Label
	Public WithEvents fraCoverStartDateBetween As System.Windows.Forms.GroupBox
	Public WithEvents txtAgent As System.Windows.Forms.TextBox
	Public WithEvents txtClient As System.Windows.Forms.TextBox
	Public WithEvents txtQuoteRef As System.Windows.Forms.TextBox
	Public WithEvents lblRiskIndex As System.Windows.Forms.Label
	Public WithEvents fraMain As System.Windows.Forms.GroupBox
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents lblNetAmount As System.Windows.Forms.Label
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdAddTask = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdMakePayment = New System.Windows.Forms.Button
        Me.cmdSelectAlll = New System.Windows.Forms.Button
        Me.stbstatus = New System.Windows.Forms.StatusStrip
        Me._stbstatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.lvwsearchdetails = New System.Windows.Forms.ListView
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.fraMain = New System.Windows.Forms.GroupBox
        Me.txtProduct = New System.Windows.Forms.TextBox
        Me.cmdProduct = New System.Windows.Forms.Button
        Me.cmdQuoteRef = New System.Windows.Forms.Button
        Me.cmdClient = New System.Windows.Forms.Button
        Me.cmdAgent = New System.Windows.Forms.Button
        Me.txtRiskIndex = New System.Windows.Forms.TextBox
        Me.chkDirectBusiness = New System.Windows.Forms.CheckBox
        Me.cmdNewSearch = New System.Windows.Forms.Button
        Me.cmdFindNow = New System.Windows.Forms.Button
        Me.fraCoverStartDateBetween = New System.Windows.Forms.GroupBox
        Me.dtpEndDate = New System.Windows.Forms.DateTimePicker
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker
        Me.lblEndDate = New System.Windows.Forms.Label
        Me.lblStartDate = New System.Windows.Forms.Label
        Me.txtAgent = New System.Windows.Forms.TextBox
        Me.txtClient = New System.Windows.Forms.TextBox
        Me.txtQuoteRef = New System.Windows.Forms.TextBox
        Me.lblRiskIndex = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblNetAmount = New System.Windows.Forms.Label
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.stbstatus.SuspendLayout()
        Me.fraMain.SuspendLayout()
        Me.fraCoverStartDateBetween.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(430, 408)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(89, 23)
        Me.cmdOK.TabIndex = 21
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdOK, "Confirm Changes")
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdAddTask
        '
        Me.cmdAddTask.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddTask.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddTask.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddTask.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddTask.Location = New System.Drawing.Point(624, 408)
        Me.cmdAddTask.Name = "cmdAddTask"
        Me.cmdAddTask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddTask.Size = New System.Drawing.Size(89, 23)
        Me.cmdAddTask.TabIndex = 23
        Me.cmdAddTask.Text = "Add &Task"
        Me.cmdAddTask.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddTask.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(529, 409)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(89, 23)
        Me.cmdCancel.TabIndex = 22
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdMakePayment
        '
        Me.cmdMakePayment.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMakePayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMakePayment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMakePayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMakePayment.Location = New System.Drawing.Point(152, 408)
        Me.cmdMakePayment.Name = "cmdMakePayment"
        Me.cmdMakePayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMakePayment.Size = New System.Drawing.Size(129, 22)
        Me.cmdMakePayment.TabIndex = 20
        Me.cmdMakePayment.Text = "Make Payment"
        Me.cmdMakePayment.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdMakePayment.UseVisualStyleBackColor = False
        '
        'cmdSelectAlll
        '
        Me.cmdSelectAlll.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelectAlll.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelectAlll.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSelectAlll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelectAlll.Location = New System.Drawing.Point(8, 408)
        Me.cmdSelectAlll.Name = "cmdSelectAlll"
        Me.cmdSelectAlll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelectAlll.Size = New System.Drawing.Size(129, 22)
        Me.cmdSelectAlll.TabIndex = 19
        Me.cmdSelectAlll.Text = "Select All"
        Me.cmdSelectAlll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSelectAlll.UseVisualStyleBackColor = False
        '
        'stbstatus
        '
        Me.stbstatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbstatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._stbstatus_Panel1})
        Me.stbstatus.Location = New System.Drawing.Point(0, 435)
        Me.stbstatus.Name = "stbstatus"
        Me.stbstatus.ShowItemToolTips = True
        Me.stbstatus.Size = New System.Drawing.Size(716, 23)
        Me.stbstatus.TabIndex = 18
        '
        '_stbstatus_Panel1
        '
        Me._stbstatus_Panel1.AutoSize = False
        Me._stbstatus_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._stbstatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._stbstatus_Panel1.DoubleClickEnabled = True
        Me._stbstatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._stbstatus_Panel1.Name = "_stbstatus_Panel1"
        Me._stbstatus_Panel1.RightToLeftAutoMirrorImage = True
        Me._stbstatus_Panel1.Size = New System.Drawing.Size(700, 23)
        Me._stbstatus_Panel1.Tag = ""
        Me._stbstatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lvwsearchdetails
        '
        Me.lvwsearchdetails.BackColor = System.Drawing.SystemColors.Window
        Me.lvwsearchdetails.CheckBoxes = True
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwsearchdetails, "")
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwsearchdetails, False)
        Me.lvwsearchdetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwsearchdetails.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwsearchdetails.FullRowSelect = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwsearchdetails, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwsearchdetails, "")
        Me.lvwsearchdetails.Location = New System.Drawing.Point(8, 184)
        Me.lvwsearchdetails.Name = "lvwsearchdetails"
        Me.lvwsearchdetails.Size = New System.Drawing.Size(705, 185)
        Me.listViewHelper1.SetSmallIcons(Me.lvwsearchdetails, "")
        Me.listViewHelper1.SetSorted(Me.lvwsearchdetails, False)
        Me.listViewHelper1.SetSortKey(Me.lvwsearchdetails, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwsearchdetails, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwsearchdetails.TabIndex = 18
        Me.lvwsearchdetails.UseCompatibleStateImageBehavior = False
        Me.lvwsearchdetails.View = System.Windows.Forms.View.Details
        '
        'fraMain
        '
        Me.fraMain.BackColor = System.Drawing.SystemColors.Control
        Me.fraMain.Controls.Add(Me.txtProduct)
        Me.fraMain.Controls.Add(Me.cmdProduct)
        Me.fraMain.Controls.Add(Me.cmdQuoteRef)
        Me.fraMain.Controls.Add(Me.cmdClient)
        Me.fraMain.Controls.Add(Me.cmdAgent)
        Me.fraMain.Controls.Add(Me.txtRiskIndex)
        Me.fraMain.Controls.Add(Me.chkDirectBusiness)
        Me.fraMain.Controls.Add(Me.cmdNewSearch)
        Me.fraMain.Controls.Add(Me.cmdFindNow)
        Me.fraMain.Controls.Add(Me.fraCoverStartDateBetween)
        Me.fraMain.Controls.Add(Me.txtAgent)
        Me.fraMain.Controls.Add(Me.txtClient)
        Me.fraMain.Controls.Add(Me.txtQuoteRef)
        Me.fraMain.Controls.Add(Me.lblRiskIndex)
        Me.fraMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraMain.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraMain.Location = New System.Drawing.Point(8, 0)
        Me.fraMain.Name = "fraMain"
        Me.fraMain.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraMain.Size = New System.Drawing.Size(705, 177)
        Me.fraMain.TabIndex = 0
        Me.fraMain.TabStop = False
        '
        'txtProduct
        '
        Me.txtProduct.AcceptsReturn = True
        Me.txtProduct.BackColor = System.Drawing.SystemColors.Control
        Me.txtProduct.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtProduct.Enabled = False
        Me.txtProduct.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtProduct.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtProduct.Location = New System.Drawing.Point(104, 117)
        Me.txtProduct.MaxLength = 0
        Me.txtProduct.Name = "txtProduct"
        Me.txtProduct.ReadOnly = True
        Me.txtProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtProduct.Size = New System.Drawing.Size(169, 21)
        Me.txtProduct.TabIndex = 8
        '
        'cmdProduct
        '
        Me.cmdProduct.BackColor = System.Drawing.SystemColors.Control
        Me.cmdProduct.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdProduct.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdProduct.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdProduct.Location = New System.Drawing.Point(24, 117)
        Me.cmdProduct.Name = "cmdProduct"
        Me.cmdProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdProduct.Size = New System.Drawing.Size(81, 21)
        Me.cmdProduct.TabIndex = 7
        Me.cmdProduct.Text = "&Product"
        Me.cmdProduct.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdProduct.UseVisualStyleBackColor = False
        '
        'cmdQuoteRef
        '
        Me.cmdQuoteRef.BackColor = System.Drawing.SystemColors.Control
        Me.cmdQuoteRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdQuoteRef.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdQuoteRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdQuoteRef.Location = New System.Drawing.Point(24, 24)
        Me.cmdQuoteRef.Name = "cmdQuoteRef"
        Me.cmdQuoteRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdQuoteRef.Size = New System.Drawing.Size(81, 21)
        Me.cmdQuoteRef.TabIndex = 1
        Me.cmdQuoteRef.Text = "&Quote"
        Me.cmdQuoteRef.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdQuoteRef.UseVisualStyleBackColor = False
        '
        'cmdClient
        '
        Me.cmdClient.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClient.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClient.Location = New System.Drawing.Point(24, 55)
        Me.cmdClient.Name = "cmdClient"
        Me.cmdClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClient.Size = New System.Drawing.Size(81, 21)
        Me.cmdClient.TabIndex = 3
        Me.cmdClient.Text = "&Client"
        Me.cmdClient.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClient.UseVisualStyleBackColor = False
        '
        'cmdAgent
        '
        Me.cmdAgent.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAgent.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAgent.Location = New System.Drawing.Point(24, 86)
        Me.cmdAgent.Name = "cmdAgent"
        Me.cmdAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAgent.Size = New System.Drawing.Size(81, 21)
        Me.cmdAgent.TabIndex = 5
        Me.cmdAgent.Text = "&Agent"
        Me.cmdAgent.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAgent.UseVisualStyleBackColor = False
        '
        'txtRiskIndex
        '
        Me.txtRiskIndex.AcceptsReturn = True
        Me.txtRiskIndex.BackColor = System.Drawing.SystemColors.Window
        Me.txtRiskIndex.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRiskIndex.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRiskIndex.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRiskIndex.Location = New System.Drawing.Point(104, 148)
        Me.txtRiskIndex.MaxLength = 0
        Me.txtRiskIndex.Name = "txtRiskIndex"
        Me.txtRiskIndex.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRiskIndex.Size = New System.Drawing.Size(169, 21)
        Me.txtRiskIndex.TabIndex = 10
        '
        'chkDirectBusiness
        '
        Me.chkDirectBusiness.BackColor = System.Drawing.SystemColors.Control
        Me.chkDirectBusiness.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDirectBusiness.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDirectBusiness.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDirectBusiness.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDirectBusiness.Location = New System.Drawing.Point(304, 152)
        Me.chkDirectBusiness.Name = "chkDirectBusiness"
        Me.chkDirectBusiness.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDirectBusiness.Size = New System.Drawing.Size(129, 16)
        Me.chkDirectBusiness.TabIndex = 15
        Me.chkDirectBusiness.Text = "Direct Business"
        Me.chkDirectBusiness.UseVisualStyleBackColor = False
        '
        'cmdNewSearch
        '
        Me.cmdNewSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewSearch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewSearch.Location = New System.Drawing.Point(608, 62)
        Me.cmdNewSearch.Name = "cmdNewSearch"
        Me.cmdNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewSearch.Size = New System.Drawing.Size(89, 21)
        Me.cmdNewSearch.TabIndex = 17
        Me.cmdNewSearch.Text = "New Search"
        Me.cmdNewSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNewSearch.UseVisualStyleBackColor = False
        '
        'cmdFindNow
        '
        Me.cmdFindNow.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindNow.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindNow.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindNow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindNow.Location = New System.Drawing.Point(608, 32)
        Me.cmdFindNow.Name = "cmdFindNow"
        Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindNow.Size = New System.Drawing.Size(89, 21)
        Me.cmdFindNow.TabIndex = 16
        Me.cmdFindNow.Text = "Find Now"
        Me.cmdFindNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindNow.UseVisualStyleBackColor = False
        '
        'fraCoverStartDateBetween
        '
        Me.fraCoverStartDateBetween.BackColor = System.Drawing.SystemColors.Control
        Me.fraCoverStartDateBetween.Controls.Add(Me.dtpEndDate)
        Me.fraCoverStartDateBetween.Controls.Add(Me.dtpStartDate)
        Me.fraCoverStartDateBetween.Controls.Add(Me.lblEndDate)
        Me.fraCoverStartDateBetween.Controls.Add(Me.lblStartDate)
        Me.fraCoverStartDateBetween.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCoverStartDateBetween.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraCoverStartDateBetween.Location = New System.Drawing.Point(304, 24)
        Me.fraCoverStartDateBetween.Name = "fraCoverStartDateBetween"
        Me.fraCoverStartDateBetween.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraCoverStartDateBetween.Size = New System.Drawing.Size(209, 89)
        Me.fraCoverStartDateBetween.TabIndex = 11
        Me.fraCoverStartDateBetween.TabStop = False
        Me.fraCoverStartDateBetween.Text = "Cover Start Date Between"
        '
        'dtpEndDate
        '
        Me.dtpEndDate.Checked = False
        Me.dtpEndDate.Location = New System.Drawing.Point(48, 54)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.ShowCheckBox = True
        Me.dtpEndDate.Size = New System.Drawing.Size(129, 21)
        Me.dtpEndDate.TabIndex = 14
        '
        'dtpStartDate
        '
        Me.dtpStartDate.Checked = False
        Me.dtpStartDate.Location = New System.Drawing.Point(48, 22)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.ShowCheckBox = True
        Me.dtpStartDate.Size = New System.Drawing.Size(129, 21)
        Me.dtpStartDate.TabIndex = 13
        '
        'lblEndDate
        '
        Me.lblEndDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEndDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEndDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEndDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEndDate.Location = New System.Drawing.Point(9, 54)
        Me.lblEndDate.Name = "lblEndDate"
        Me.lblEndDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEndDate.Size = New System.Drawing.Size(33, 13)
        Me.lblEndDate.TabIndex = 11
        Me.lblEndDate.Text = "To :"
        '
        'lblStartDate
        '
        Me.lblStartDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblStartDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStartDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStartDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStartDate.Location = New System.Drawing.Point(9, 30)
        Me.lblStartDate.Name = "lblStartDate"
        Me.lblStartDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStartDate.Size = New System.Drawing.Size(43, 13)
        Me.lblStartDate.TabIndex = 10
        Me.lblStartDate.Text = "From :"
        '
        'txtAgent
        '
        Me.txtAgent.AcceptsReturn = True
        Me.txtAgent.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgent.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgent.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgent.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgent.Location = New System.Drawing.Point(104, 86)
        Me.txtAgent.MaxLength = 0
        Me.txtAgent.Name = "txtAgent"
        Me.txtAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgent.Size = New System.Drawing.Size(169, 21)
        Me.txtAgent.TabIndex = 6
        '
        'txtClient
        '
        Me.txtClient.AcceptsReturn = True
        Me.txtClient.BackColor = System.Drawing.SystemColors.Window
        Me.txtClient.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClient.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClient.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClient.Location = New System.Drawing.Point(104, 55)
        Me.txtClient.MaxLength = 0
        Me.txtClient.Name = "txtClient"
        Me.txtClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClient.Size = New System.Drawing.Size(169, 21)
        Me.txtClient.TabIndex = 4
        '
        'txtQuoteRef
        '
        Me.txtQuoteRef.AcceptsReturn = True
        Me.txtQuoteRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtQuoteRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtQuoteRef.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtQuoteRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtQuoteRef.Location = New System.Drawing.Point(104, 24)
        Me.txtQuoteRef.MaxLength = 0
        Me.txtQuoteRef.Name = "txtQuoteRef"
        Me.txtQuoteRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtQuoteRef.Size = New System.Drawing.Size(169, 21)
        Me.txtQuoteRef.TabIndex = 2
        '
        'lblRiskIndex
        '
        Me.lblRiskIndex.BackColor = System.Drawing.SystemColors.Control
        Me.lblRiskIndex.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRiskIndex.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRiskIndex.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRiskIndex.Location = New System.Drawing.Point(24, 152)
        Me.lblRiskIndex.Name = "lblRiskIndex"
        Me.lblRiskIndex.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRiskIndex.Size = New System.Drawing.Size(81, 13)
        Me.lblRiskIndex.TabIndex = 9
        Me.lblRiskIndex.Text = "Risk Index"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(528, 382)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(41, 13)
        Me.Label1.TabIndex = 27
        Me.Label1.Text = "Total"
        '
        'lblNetAmount
        '
        Me.lblNetAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblNetAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblNetAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNetAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNetAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblNetAmount.Location = New System.Drawing.Point(584, 376)
        Me.lblNetAmount.Name = "lblNetAmount"
        Me.lblNetAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNetAmount.Size = New System.Drawing.Size(129, 25)
        Me.lblNetAmount.TabIndex = 26
        Me.lblNetAmount.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(716, 458)
        Me.Controls.Add(Me.cmdAddTask)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdMakePayment)
        Me.Controls.Add(Me.cmdSelectAlll)
        Me.Controls.Add(Me.stbstatus)
        Me.Controls.Add(Me.lvwsearchdetails)
        Me.Controls.Add(Me.fraMain)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblNetAmount)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Quote Collection Process"
        Me.stbstatus.ResumeLayout(False)
        Me.stbstatus.PerformLayout()
        Me.fraMain.ResumeLayout(False)
        Me.fraMain.PerformLayout()
        Me.fraCoverStartDateBetween.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class