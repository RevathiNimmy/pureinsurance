<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class mainForm
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializetxtWarning()
		InitializetxtFilePath()
		InitializetxtFileName()
		InitializetxtFileExtension()
		InitializeradioExportBasedOn()
		InitializeoptAdditionalImportOptions()
		InitializecmdDebug()
		InitializechkAdditionalExportOptions()
		InitializeStatusBar1()
		InitializeProgressBar1()
		InitializeLabel3()
		SSTab3PreviousTab = SSTab3.SelectedIndex
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
	Private WithEvents _ProgressBar1_0 As System.Windows.Forms.ProgressBar
	Private WithEvents _StatusBar1_0_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Private WithEvents _StatusBar1_0 As System.Windows.Forms.StatusStrip
	Private WithEvents _StatusBar1_1_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Private WithEvents _StatusBar1_1 As System.Windows.Forms.StatusStrip
	Private WithEvents _StatusBar1_2_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Private WithEvents _StatusBar1_2 As System.Windows.Forms.StatusStrip
	Private WithEvents _ProgressBar1_1 As System.Windows.Forms.ProgressBar
	Public WithEvents lblDebugOn As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents cboGISLookupUserList As uctGISUserDefLookupControl.cboGISLookup
	Private WithEvents _txtFileExtension_1 As System.Windows.Forms.TextBox
	Private WithEvents _txtFileName_1 As System.Windows.Forms.TextBox
	Private WithEvents _txtFilePath_1 As System.Windows.Forms.TextBox
	Private WithEvents _Label3_2 As System.Windows.Forms.Label
	Private WithEvents _Label3_1 As System.Windows.Forms.Label
	Private WithEvents _Label3_0 As System.Windows.Forms.Label
	Public WithEvents Frame3 As System.Windows.Forms.GroupBox
	Private WithEvents _SSTab2_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents cboDataModel As PMLookupControl.cboPMLookup
	Private WithEvents _radioExportBasedOn_3 As System.Windows.Forms.RadioButton
	Public WithEvents txtdatamodelId As System.Windows.Forms.TextBox
	Private WithEvents _radioExportBasedOn_2 As System.Windows.Forms.RadioButton
	Private WithEvents _radioExportBasedOn_1 As System.Windows.Forms.RadioButton
	Private WithEvents _radioExportBasedOn_0 As System.Windows.Forms.RadioButton
	Public WithEvents Frame4 As System.Windows.Forms.GroupBox
	Private WithEvents _chkAdditionalExportOptions_6 As System.Windows.Forms.CheckBox
	Private WithEvents _chkAdditionalExportOptions_5 As System.Windows.Forms.CheckBox
	Private WithEvents _chkAdditionalExportOptions_4 As System.Windows.Forms.CheckBox
	Private WithEvents _chkAdditionalExportOptions_3 As System.Windows.Forms.CheckBox
	Private WithEvents _chkAdditionalExportOptions_2 As System.Windows.Forms.CheckBox
	Private WithEvents _chkAdditionalExportOptions_1 As System.Windows.Forms.CheckBox
	Private WithEvents _chkAdditionalExportOptions_0 As System.Windows.Forms.CheckBox
	Public WithEvents Frame5 As System.Windows.Forms.GroupBox
	Private WithEvents _SSTab2_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents txtExportHeaderComment As System.Windows.Forms.TextBox
	Public WithEvents lblComment As System.Windows.Forms.Label
	Public WithEvents Frame6 As System.Windows.Forms.GroupBox
	Public WithEvents txtExportConfirmationText As System.Windows.Forms.TextBox
	Public WithEvents fraExportHeader As System.Windows.Forms.GroupBox
	Private WithEvents _SSTab2_TabPage2 As System.Windows.Forms.TabPage
	Private WithEvents _txtWarning_1 As System.Windows.Forms.TextBox
	Public WithEvents cmdExportCancel As System.Windows.Forms.Button
	Public WithEvents cmdExport As System.Windows.Forms.Button
	Public WithEvents lblExportInfo As System.Windows.Forms.Label
	Private WithEvents _SSTab2_TabPage3 As System.Windows.Forms.TabPage
	Public WithEvents SSTab2 As System.Windows.Forms.TabControl
	Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
	Private WithEvents _txtFileExtension_0 As System.Windows.Forms.TextBox
	Private WithEvents _txtFileName_0 As System.Windows.Forms.TextBox
	Private WithEvents _txtFilePath_0 As System.Windows.Forms.TextBox
	Private WithEvents _Label3_5 As System.Windows.Forms.Label
	Private WithEvents _Label3_4 As System.Windows.Forms.Label
	Private WithEvents _Label3_3 As System.Windows.Forms.Label
	Public WithEvents Frame2 As System.Windows.Forms.GroupBox
	Private WithEvents _SSTab3_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents txtImportConfirmation As System.Windows.Forms.TextBox
	Public WithEvents fraImportHeader As System.Windows.Forms.GroupBox
	Private WithEvents _optAdditionalImportOptions_1 As System.Windows.Forms.RadioButton
	Private WithEvents _optAdditionalImportOptions_2 As System.Windows.Forms.RadioButton
	Private WithEvents _optAdditionalImportOptions_0 As System.Windows.Forms.RadioButton
	Public WithEvents Frame7 As System.Windows.Forms.GroupBox
	Private WithEvents _SSTab3_TabPage1 As System.Windows.Forms.TabPage
	Private WithEvents _txtWarning_0 As System.Windows.Forms.TextBox
	Public WithEvents cmdImport As System.Windows.Forms.Button
	Public WithEvents Label4 As System.Windows.Forms.Label
	Private WithEvents _SSTab3_TabPage2 As System.Windows.Forms.TabPage
	Public WithEvents SSTab3 As System.Windows.Forms.TabControl
	Private WithEvents _SSTab1_TabPage1 As System.Windows.Forms.TabPage
	Private WithEvents _cmdDebug_4 As System.Windows.Forms.Button
	Private WithEvents _cmdDebug_3 As System.Windows.Forms.Button
	Private WithEvents _cmdDebug_2 As System.Windows.Forms.Button
	Private WithEvents _cmdDebug_1 As System.Windows.Forms.Button
	Private WithEvents _cmdDebug_0 As System.Windows.Forms.Button
	Public WithEvents txtDebug As System.Windows.Forms.TextBox
	Public WithEvents Label1 As System.Windows.Forms.Label
	Private WithEvents _SSTab1_TabPage2 As System.Windows.Forms.TabPage
	Public WithEvents SSTab1 As System.Windows.Forms.TabControl
    Public Label3(5) As System.Windows.Forms.Label
    Public ProgressBar1(1) As System.Windows.Forms.ProgressBar
    Public StatusBar1(2) As System.Windows.Forms.StatusStrip
    Public chkAdditionalExportOptions(7) As System.Windows.Forms.CheckBox
    Public cmdDebug(4) As System.Windows.Forms.Button
    Public optAdditionalImportOptions(2) As System.Windows.Forms.RadioButton
    Public radioExportBasedOn(3) As System.Windows.Forms.RadioButton
    Public txtFileExtension(1) As System.Windows.Forms.TextBox
    Public txtFileName(1) As System.Windows.Forms.TextBox
    Public txtFilePath(1) As System.Windows.Forms.TextBox
    Public txtWarning(1) As System.Windows.Forms.TextBox
	Dim Private SSTab3PreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(mainForm))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me._chkAdditionalExportOptions_5 = New System.Windows.Forms.CheckBox
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me._StatusBar1_2 = New System.Windows.Forms.StatusStrip
        Me._StatusBar1_2_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me._StatusBar1_1 = New System.Windows.Forms.StatusStrip
        Me._StatusBar1_1_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me._StatusBar1_0 = New System.Windows.Forms.StatusStrip
        Me._StatusBar1_0_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me._ProgressBar1_0 = New System.Windows.Forms.ProgressBar
        Me._ProgressBar1_1 = New System.Windows.Forms.ProgressBar
        Me.lblDebugOn = New System.Windows.Forms.Label
        Me.cboGISLookupUserList = New uctGISUserDefLookupControl.cboGISLookup
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me.SSTab2 = New System.Windows.Forms.TabControl
        Me._SSTab2_TabPage0 = New System.Windows.Forms.TabPage
        Me.Frame3 = New System.Windows.Forms.GroupBox
        Me._txtFileExtension_1 = New System.Windows.Forms.TextBox
        Me._txtFileName_1 = New System.Windows.Forms.TextBox
        Me._txtFilePath_1 = New System.Windows.Forms.TextBox
        Me._Label3_2 = New System.Windows.Forms.Label
        Me._Label3_1 = New System.Windows.Forms.Label
        Me._Label3_0 = New System.Windows.Forms.Label
        Me._SSTab2_TabPage1 = New System.Windows.Forms.TabPage
        Me.Frame4 = New System.Windows.Forms.GroupBox
        Me.cboDataModel = New PMLookupControl.cboPMLookup
        Me._radioExportBasedOn_3 = New System.Windows.Forms.RadioButton
        Me.txtdatamodelId = New System.Windows.Forms.TextBox
        Me._radioExportBasedOn_2 = New System.Windows.Forms.RadioButton
        Me._radioExportBasedOn_1 = New System.Windows.Forms.RadioButton
        Me._radioExportBasedOn_0 = New System.Windows.Forms.RadioButton
        Me.Frame5 = New System.Windows.Forms.GroupBox
        Me.chkExportSPUICCS = New System.Windows.Forms.CheckBox
        Me.chkGenerateUMLScript = New System.Windows.Forms.CheckBox
        Me.chkIncludeUMLs = New System.Windows.Forms.CheckBox
        Me._chkAdditionalExportOptions_6 = New System.Windows.Forms.CheckBox
        Me._chkAdditionalExportOptions_4 = New System.Windows.Forms.CheckBox
        Me._chkAdditionalExportOptions_3 = New System.Windows.Forms.CheckBox
        Me._chkAdditionalExportOptions_2 = New System.Windows.Forms.CheckBox
        Me._chkAdditionalExportOptions_1 = New System.Windows.Forms.CheckBox
        Me._chkAdditionalExportOptions_0 = New System.Windows.Forms.CheckBox
        Me._SSTab2_TabPage2 = New System.Windows.Forms.TabPage
        Me.Frame6 = New System.Windows.Forms.GroupBox
        Me.txtExportHeaderComment = New System.Windows.Forms.TextBox
        Me.lblComment = New System.Windows.Forms.Label
        Me.fraExportHeader = New System.Windows.Forms.GroupBox
        Me.txtExportConfirmationText = New System.Windows.Forms.TextBox
        Me._SSTab2_TabPage3 = New System.Windows.Forms.TabPage
        Me._txtWarning_1 = New System.Windows.Forms.TextBox
        Me.cmdExportCancel = New System.Windows.Forms.Button
        Me.cmdExport = New System.Windows.Forms.Button
        Me.lblExportInfo = New System.Windows.Forms.Label
        Me._SSTab1_TabPage1 = New System.Windows.Forms.TabPage
        Me.SSTab3 = New System.Windows.Forms.TabControl
        Me._SSTab3_TabPage0 = New System.Windows.Forms.TabPage
        Me.Frame2 = New System.Windows.Forms.GroupBox
        Me.cmdImportBrowse = New System.Windows.Forms.Button
        Me.chkImportUMLScriptsOnly = New System.Windows.Forms.CheckBox
        Me._txtFileExtension_0 = New System.Windows.Forms.TextBox
        Me._txtFileName_0 = New System.Windows.Forms.TextBox
        Me._txtFilePath_0 = New System.Windows.Forms.TextBox
        Me._Label3_5 = New System.Windows.Forms.Label
        Me._Label3_4 = New System.Windows.Forms.Label
        Me._Label3_3 = New System.Windows.Forms.Label
        Me._SSTab3_TabPage1 = New System.Windows.Forms.TabPage
        Me.fraImportHeader = New System.Windows.Forms.GroupBox
        Me.txtImportConfirmation = New System.Windows.Forms.TextBox
        Me.Frame7 = New System.Windows.Forms.GroupBox
        Me._optAdditionalImportOptions_1 = New System.Windows.Forms.RadioButton
        Me._optAdditionalImportOptions_2 = New System.Windows.Forms.RadioButton
        Me._optAdditionalImportOptions_0 = New System.Windows.Forms.RadioButton
        Me._SSTab3_TabPage2 = New System.Windows.Forms.TabPage
        Me._txtWarning_0 = New System.Windows.Forms.TextBox
        Me.cmdImport = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me._SSTab1_TabPage2 = New System.Windows.Forms.TabPage
        Me._cmdDebug_4 = New System.Windows.Forms.Button
        Me._cmdDebug_3 = New System.Windows.Forms.Button
        Me._cmdDebug_2 = New System.Windows.Forms.Button
        Me._cmdDebug_1 = New System.Windows.Forms.Button
        Me._cmdDebug_0 = New System.Windows.Forms.Button
        Me.txtDebug = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.CommonDialog1Open = New System.Windows.Forms.OpenFileDialog
        Me.Frame1.SuspendLayout()
        Me._StatusBar1_2.SuspendLayout()
        Me._StatusBar1_1.SuspendLayout()
        Me._StatusBar1_0.SuspendLayout()
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me.SSTab2.SuspendLayout()
        Me._SSTab2_TabPage0.SuspendLayout()
        Me.Frame3.SuspendLayout()
        Me._SSTab2_TabPage1.SuspendLayout()
        Me.Frame4.SuspendLayout()
        Me.Frame5.SuspendLayout()
        Me._SSTab2_TabPage2.SuspendLayout()
        Me.Frame6.SuspendLayout()
        Me.fraExportHeader.SuspendLayout()
        Me._SSTab2_TabPage3.SuspendLayout()
        Me._SSTab1_TabPage1.SuspendLayout()
        Me.SSTab3.SuspendLayout()
        Me._SSTab3_TabPage0.SuspendLayout()
        Me.Frame2.SuspendLayout()
        Me._SSTab3_TabPage1.SuspendLayout()
        Me.fraImportHeader.SuspendLayout()
        Me.Frame7.SuspendLayout()
        Me._SSTab3_TabPage2.SuspendLayout()
        Me._SSTab1_TabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        '_chkAdditionalExportOptions_5
        '
        Me._chkAdditionalExportOptions_5.BackColor = System.Drawing.SystemColors.Control
        Me._chkAdditionalExportOptions_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkAdditionalExportOptions_5.Enabled = False
        Me._chkAdditionalExportOptions_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkAdditionalExportOptions_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkAdditionalExportOptions_5.Location = New System.Drawing.Point(30, 57)
        Me._chkAdditionalExportOptions_5.Name = "_chkAdditionalExportOptions_5"
        Me._chkAdditionalExportOptions_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkAdditionalExportOptions_5.Size = New System.Drawing.Size(131, 21)
        Me._chkAdditionalExportOptions_5.TabIndex = 63
        Me._chkAdditionalExportOptions_5.Text = "PBDocs Only"
        Me.ToolTip1.SetToolTip(Me._chkAdditionalExportOptions_5, "Select this option to export documents of type PBDocs only")
        Me._chkAdditionalExportOptions_5.UseVisualStyleBackColor = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me._StatusBar1_2)
        Me.Frame1.Controls.Add(Me._StatusBar1_1)
        Me.Frame1.Controls.Add(Me._StatusBar1_0)
        Me.Frame1.Controls.Add(Me._ProgressBar1_0)
        Me.Frame1.Controls.Add(Me._ProgressBar1_1)
        Me.Frame1.Controls.Add(Me.lblDebugOn)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(5, 368)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(724, 90)
        Me.Frame1.TabIndex = 27
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Progress Indicators"
        '
        '_StatusBar1_2
        '
        Me._StatusBar1_2.Dock = System.Windows.Forms.DockStyle.None
        Me._StatusBar1_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._StatusBar1_2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._StatusBar1_2_Panel1})
        Me._StatusBar1_2.Location = New System.Drawing.Point(4, 64)
        Me._StatusBar1_2.Name = "_StatusBar1_2"
        Me._StatusBar1_2.ShowItemToolTips = True
        Me._StatusBar1_2.Size = New System.Drawing.Size(317, 22)
        Me._StatusBar1_2.TabIndex = 31
        '
        '_StatusBar1_2_Panel1
        '
        Me._StatusBar1_2_Panel1.AutoSize = False
        Me._StatusBar1_2_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._StatusBar1_2_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._StatusBar1_2_Panel1.DoubleClickEnabled = True
        Me._StatusBar1_2_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._StatusBar1_2_Panel1.Name = "_StatusBar1_2_Panel1"
        Me._StatusBar1_2_Panel1.Size = New System.Drawing.Size(300, 22)
        Me._StatusBar1_2_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_StatusBar1_1
        '
        Me._StatusBar1_1.Dock = System.Windows.Forms.DockStyle.None
        Me._StatusBar1_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._StatusBar1_1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._StatusBar1_1_Panel1})
        Me._StatusBar1_1.Location = New System.Drawing.Point(4, 39)
        Me._StatusBar1_1.Name = "_StatusBar1_1"
        Me._StatusBar1_1.ShowItemToolTips = True
        Me._StatusBar1_1.Size = New System.Drawing.Size(317, 22)
        Me._StatusBar1_1.TabIndex = 30
        '
        '_StatusBar1_1_Panel1
        '
        Me._StatusBar1_1_Panel1.AutoSize = False
        Me._StatusBar1_1_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._StatusBar1_1_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._StatusBar1_1_Panel1.DoubleClickEnabled = True
        Me._StatusBar1_1_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._StatusBar1_1_Panel1.Name = "_StatusBar1_1_Panel1"
        Me._StatusBar1_1_Panel1.Size = New System.Drawing.Size(300, 22)
        Me._StatusBar1_1_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_StatusBar1_0
        '
        Me._StatusBar1_0.Dock = System.Windows.Forms.DockStyle.None
        Me._StatusBar1_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._StatusBar1_0.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._StatusBar1_0_Panel1})
        Me._StatusBar1_0.Location = New System.Drawing.Point(4, 14)
        Me._StatusBar1_0.Name = "_StatusBar1_0"
        Me._StatusBar1_0.ShowItemToolTips = True
        Me._StatusBar1_0.Size = New System.Drawing.Size(317, 22)
        Me._StatusBar1_0.TabIndex = 29
        '
        '_StatusBar1_0_Panel1
        '
        Me._StatusBar1_0_Panel1.AutoSize = False
        Me._StatusBar1_0_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._StatusBar1_0_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._StatusBar1_0_Panel1.DoubleClickEnabled = True
        Me._StatusBar1_0_Panel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._StatusBar1_0_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._StatusBar1_0_Panel1.Name = "_StatusBar1_0_Panel1"
        Me._StatusBar1_0_Panel1.Size = New System.Drawing.Size(300, 22)
        Me._StatusBar1_0_Panel1.Text = "14888"
        Me._StatusBar1_0_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_ProgressBar1_0
        '
        Me._ProgressBar1_0.Location = New System.Drawing.Point(394, 20)
        Me._ProgressBar1_0.Name = "_ProgressBar1_0"
        Me._ProgressBar1_0.Size = New System.Drawing.Size(155, 16)
        Me._ProgressBar1_0.TabIndex = 28
        '
        '_ProgressBar1_1
        '
        Me._ProgressBar1_1.Location = New System.Drawing.Point(394, 66)
        Me._ProgressBar1_1.Name = "_ProgressBar1_1"
        Me._ProgressBar1_1.Size = New System.Drawing.Size(155, 16)
        Me._ProgressBar1_1.TabIndex = 32
        '
        'lblDebugOn
        '
        Me.lblDebugOn.BackColor = System.Drawing.SystemColors.Control
        Me.lblDebugOn.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDebugOn.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDebugOn.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDebugOn.Location = New System.Drawing.Point(711, 10)
        Me.lblDebugOn.Name = "lblDebugOn"
        Me.lblDebugOn.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDebugOn.Size = New System.Drawing.Size(10, 10)
        Me.lblDebugOn.TabIndex = 53
        '
        'cboGISLookupUserList
        '
        Me.cboGISLookupUserList.DefaultItemId = 0
        Me.cboGISLookupUserList.FirstItem = ""
        Me.cboGISLookupUserList.GISDataModelCode = "None"
        Me.cboGISLookupUserList.ItemId = 0
        Me.cboGISLookupUserList.ListIndex = -1
        Me.cboGISLookupUserList.Location = New System.Drawing.Point(424, 568)
        Me.cboGISLookupUserList.Name = "cboGISLookupUserList"
        Me.cboGISLookupUserList.ParentDetailId = 0
        Me.cboGISLookupUserList.ParentHeaderId = 0
        Me.cboGISLookupUserList.SingleItemId = 0
        Me.cboGISLookupUserList.Size = New System.Drawing.Size(111, 21)
        Me.cboGISLookupUserList.TabIndex = 1
        Me.cboGISLookupUserList.Table = 0
        Me.cboGISLookupUserList.ToolTipText = ""
        Me.cboGISLookupUserList.WhatsThisHelpID = 0
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage1)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage2)
        Me.SSTab1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(71, 18)
        Me.SSTab1.Location = New System.Drawing.Point(5, 7)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(730, 353)
        Me.SSTab1.TabIndex = 0
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.SSTab2)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(722, 327)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "Export"
        '
        'SSTab2
        '
        Me.SSTab2.Controls.Add(Me._SSTab2_TabPage0)
        Me.SSTab2.Controls.Add(Me._SSTab2_TabPage1)
        Me.SSTab2.Controls.Add(Me._SSTab2_TabPage2)
        Me.SSTab2.Controls.Add(Me._SSTab2_TabPage3)
        Me.SSTab2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab2.ItemSize = New System.Drawing.Size(175, 18)
        Me.SSTab2.Location = New System.Drawing.Point(9, 10)
        Me.SSTab2.Multiline = True
        Me.SSTab2.Name = "SSTab2"
        Me.SSTab2.SelectedIndex = 0
        Me.SSTab2.Size = New System.Drawing.Size(710, 318)
        Me.SSTab2.TabIndex = 3
        '
        '_SSTab2_TabPage0
        '
        Me._SSTab2_TabPage0.Controls.Add(Me.Frame3)
        Me._SSTab2_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab2_TabPage0.Name = "_SSTab2_TabPage0"
        Me._SSTab2_TabPage0.Size = New System.Drawing.Size(702, 292)
        Me._SSTab2_TabPage0.TabIndex = 0
        Me._SSTab2_TabPage0.Text = "1) Select File"
        '
        'Frame3
        '
        Me.Frame3.BackColor = System.Drawing.SystemColors.Control
        Me.Frame3.Controls.Add(Me._txtFileExtension_1)
        Me.Frame3.Controls.Add(Me._txtFileName_1)
        Me.Frame3.Controls.Add(Me._txtFilePath_1)
        Me.Frame3.Controls.Add(Me._Label3_2)
        Me.Frame3.Controls.Add(Me._Label3_1)
        Me.Frame3.Controls.Add(Me._Label3_0)
        Me.Frame3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame3.Location = New System.Drawing.Point(22, 26)
        Me.Frame3.Name = "Frame3"
        Me.Frame3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame3.Size = New System.Drawing.Size(656, 114)
        Me.Frame3.TabIndex = 4
        Me.Frame3.TabStop = False
        '
        '_txtFileExtension_1
        '
        Me._txtFileExtension_1.AcceptsReturn = True
        Me._txtFileExtension_1.BackColor = System.Drawing.SystemColors.Window
        Me._txtFileExtension_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFileExtension_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtFileExtension_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtFileExtension_1.Location = New System.Drawing.Point(77, 74)
        Me._txtFileExtension_1.MaxLength = 0
        Me._txtFileExtension_1.Name = "_txtFileExtension_1"
        Me._txtFileExtension_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFileExtension_1.Size = New System.Drawing.Size(33, 20)
        Me._txtFileExtension_1.TabIndex = 7
        Me._txtFileExtension_1.Text = "pbe"
        '
        '_txtFileName_1
        '
        Me._txtFileName_1.AcceptsReturn = True
        Me._txtFileName_1.BackColor = System.Drawing.SystemColors.Window
        Me._txtFileName_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFileName_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtFileName_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtFileName_1.Location = New System.Drawing.Point(77, 50)
        Me._txtFileName_1.MaxLength = 0
        Me._txtFileName_1.Name = "_txtFileName_1"
        Me._txtFileName_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFileName_1.Size = New System.Drawing.Size(250, 20)
        Me._txtFileName_1.TabIndex = 6
        Me._txtFileName_1.Text = "pbexport"
        '
        '_txtFilePath_1
        '
        Me._txtFilePath_1.AcceptsReturn = True
        Me._txtFilePath_1.BackColor = System.Drawing.SystemColors.Window
        Me._txtFilePath_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFilePath_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtFilePath_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtFilePath_1.Location = New System.Drawing.Point(77, 23)
        Me._txtFilePath_1.MaxLength = 0
        Me._txtFilePath_1.Name = "_txtFilePath_1"
        Me._txtFilePath_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFilePath_1.Size = New System.Drawing.Size(250, 20)
        Me._txtFilePath_1.TabIndex = 5
        Me._txtFilePath_1.Text = "c:\temp\"
        '
        '_Label3_2
        '
        Me._Label3_2.BackColor = System.Drawing.SystemColors.Control
        Me._Label3_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label3_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Label3_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._Label3_2.Location = New System.Drawing.Point(19, 76)
        Me._Label3_2.Name = "_Label3_2"
        Me._Label3_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label3_2.Size = New System.Drawing.Size(71, 15)
        Me._Label3_2.TabIndex = 10
        Me._Label3_2.Text = "Extension"
        '
        '_Label3_1
        '
        Me._Label3_1.BackColor = System.Drawing.SystemColors.Control
        Me._Label3_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label3_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Label3_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._Label3_1.Location = New System.Drawing.Point(19, 51)
        Me._Label3_1.Name = "_Label3_1"
        Me._Label3_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label3_1.Size = New System.Drawing.Size(71, 15)
        Me._Label3_1.TabIndex = 9
        Me._Label3_1.Text = "Name"
        '
        '_Label3_0
        '
        Me._Label3_0.BackColor = System.Drawing.SystemColors.Control
        Me._Label3_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label3_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Label3_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._Label3_0.Location = New System.Drawing.Point(19, 26)
        Me._Label3_0.Name = "_Label3_0"
        Me._Label3_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label3_0.Size = New System.Drawing.Size(71, 15)
        Me._Label3_0.TabIndex = 8
        Me._Label3_0.Text = "Path"
        '
        '_SSTab2_TabPage1
        '
        Me._SSTab2_TabPage1.Controls.Add(Me.Frame4)
        Me._SSTab2_TabPage1.Controls.Add(Me.Frame5)
        Me._SSTab2_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._SSTab2_TabPage1.Name = "_SSTab2_TabPage1"
        Me._SSTab2_TabPage1.Size = New System.Drawing.Size(702, 292)
        Me._SSTab2_TabPage1.TabIndex = 1
        Me._SSTab2_TabPage1.Text = "2) Select Mode"
        '
        'Frame4
        '
        Me.Frame4.BackColor = System.Drawing.SystemColors.Control
        Me.Frame4.Controls.Add(Me.cboDataModel)
        Me.Frame4.Controls.Add(Me._radioExportBasedOn_3)
        Me.Frame4.Controls.Add(Me.txtdatamodelId)
        Me.Frame4.Controls.Add(Me._radioExportBasedOn_2)
        Me.Frame4.Controls.Add(Me._radioExportBasedOn_1)
        Me.Frame4.Controls.Add(Me._radioExportBasedOn_0)
        Me.Frame4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame4.Location = New System.Drawing.Point(16, 10)
        Me.Frame4.Name = "Frame4"
        Me.Frame4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame4.Size = New System.Drawing.Size(312, 100)
        Me.Frame4.TabIndex = 14
        Me.Frame4.TabStop = False
        Me.Frame4.Text = "Export based upon..."
        '
        'cboDataModel
        '
        Me.cboDataModel.DefaultItemId = 0
        Me.cboDataModel.FirstItem = ""
        Me.cboDataModel.ItemId = 0
        Me.cboDataModel.ListIndex = -1
        Me.cboDataModel.Location = New System.Drawing.Point(104, 24)
        Me.cboDataModel.Name = "cboDataModel"
        Me.cboDataModel.PMLookupProductFamily = 9
        Me.cboDataModel.SingleItemId = 0
        Me.cboDataModel.Size = New System.Drawing.Size(193, 21)
        Me.cboDataModel.Sorted = True
        Me.cboDataModel.TabIndex = 61
        Me.cboDataModel.TableName = "gis_data_model"
        Me.cboDataModel.ToolTipText = ""
        Me.cboDataModel.WhereClause = ""
        '
        '_radioExportBasedOn_3
        '
        Me._radioExportBasedOn_3.BackColor = System.Drawing.SystemColors.Control
        Me._radioExportBasedOn_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._radioExportBasedOn_3.Enabled = False
        Me._radioExportBasedOn_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._radioExportBasedOn_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._radioExportBasedOn_3.Location = New System.Drawing.Point(178, 76)
        Me._radioExportBasedOn_3.Name = "_radioExportBasedOn_3"
        Me._radioExportBasedOn_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._radioExportBasedOn_3.Size = New System.Drawing.Size(70, 15)
        Me._radioExportBasedOn_3.TabIndex = 52
        Me._radioExportBasedOn_3.TabStop = True
        Me._radioExportBasedOn_3.Text = "Migration"
        Me._radioExportBasedOn_3.UseVisualStyleBackColor = False
        '
        'txtdatamodelId
        '
        Me.txtdatamodelId.AcceptsReturn = True
        Me.txtdatamodelId.BackColor = System.Drawing.SystemColors.Window
        Me.txtdatamodelId.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtdatamodelId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtdatamodelId.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtdatamodelId.Location = New System.Drawing.Point(260, 74)
        Me.txtdatamodelId.MaxLength = 0
        Me.txtdatamodelId.Name = "txtdatamodelId"
        Me.txtdatamodelId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtdatamodelId.Size = New System.Drawing.Size(38, 20)
        Me.txtdatamodelId.TabIndex = 51
        Me.txtdatamodelId.Text = "0"
        Me.txtdatamodelId.Visible = False
        '
        '_radioExportBasedOn_2
        '
        Me._radioExportBasedOn_2.BackColor = System.Drawing.SystemColors.Control
        Me._radioExportBasedOn_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._radioExportBasedOn_2.Enabled = False
        Me._radioExportBasedOn_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._radioExportBasedOn_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._radioExportBasedOn_2.Location = New System.Drawing.Point(12, 74)
        Me._radioExportBasedOn_2.Name = "_radioExportBasedOn_2"
        Me._radioExportBasedOn_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._radioExportBasedOn_2.Size = New System.Drawing.Size(79, 15)
        Me._radioExportBasedOn_2.TabIndex = 26
        Me._radioExportBasedOn_2.TabStop = True
        Me._radioExportBasedOn_2.Text = "Screen"
        Me._radioExportBasedOn_2.UseVisualStyleBackColor = False
        '
        '_radioExportBasedOn_1
        '
        Me._radioExportBasedOn_1.BackColor = System.Drawing.SystemColors.Control
        Me._radioExportBasedOn_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._radioExportBasedOn_1.Enabled = False
        Me._radioExportBasedOn_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._radioExportBasedOn_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._radioExportBasedOn_1.Location = New System.Drawing.Point(12, 51)
        Me._radioExportBasedOn_1.Name = "_radioExportBasedOn_1"
        Me._radioExportBasedOn_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._radioExportBasedOn_1.Size = New System.Drawing.Size(79, 15)
        Me._radioExportBasedOn_1.TabIndex = 25
        Me._radioExportBasedOn_1.TabStop = True
        Me._radioExportBasedOn_1.Text = "Scheme"
        Me._radioExportBasedOn_1.UseVisualStyleBackColor = False
        '
        '_radioExportBasedOn_0
        '
        Me._radioExportBasedOn_0.BackColor = System.Drawing.SystemColors.Control
        Me._radioExportBasedOn_0.Checked = True
        Me._radioExportBasedOn_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._radioExportBasedOn_0.Enabled = False
        Me._radioExportBasedOn_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._radioExportBasedOn_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._radioExportBasedOn_0.Location = New System.Drawing.Point(12, 27)
        Me._radioExportBasedOn_0.Name = "_radioExportBasedOn_0"
        Me._radioExportBasedOn_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._radioExportBasedOn_0.Size = New System.Drawing.Size(79, 15)
        Me._radioExportBasedOn_0.TabIndex = 24
        Me._radioExportBasedOn_0.TabStop = True
        Me._radioExportBasedOn_0.Text = "Data Model"
        Me._radioExportBasedOn_0.UseVisualStyleBackColor = False
        '
        'Frame5
        '
        Me.Frame5.BackColor = System.Drawing.SystemColors.Control
        Me.Frame5.Controls.Add(Me.chkExportSPUICCS)
        Me.Frame5.Controls.Add(Me.chkGenerateUMLScript)
        Me.Frame5.Controls.Add(Me.chkIncludeUMLs)
        Me.Frame5.Controls.Add(Me._chkAdditionalExportOptions_6)
        Me.Frame5.Controls.Add(Me._chkAdditionalExportOptions_5)
        Me.Frame5.Controls.Add(Me._chkAdditionalExportOptions_4)
        Me.Frame5.Controls.Add(Me._chkAdditionalExportOptions_3)
        Me.Frame5.Controls.Add(Me._chkAdditionalExportOptions_2)
        Me.Frame5.Controls.Add(Me._chkAdditionalExportOptions_1)
        Me.Frame5.Controls.Add(Me._chkAdditionalExportOptions_0)
        Me.Frame5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame5.Location = New System.Drawing.Point(15, 116)
        Me.Frame5.Name = "Frame5"
        Me.Frame5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame5.Size = New System.Drawing.Size(477, 161)
        Me.Frame5.TabIndex = 17
        Me.Frame5.TabStop = False
        Me.Frame5.Text = "Additional Options"
        '
        'chkExportSPUICCS
        '
        Me.chkExportSPUICCS.BackColor = System.Drawing.SystemColors.Control
        Me.chkExportSPUICCS.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkExportSPUICCS.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkExportSPUICCS.Location = New System.Drawing.Point(261, 19)
        Me.chkExportSPUICCS.Name = "chkExportSPUICCS"
        Me.chkExportSPUICCS.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkExportSPUICCS.Size = New System.Drawing.Size(142, 21)
        Me.chkExportSPUICCS.TabIndex = 67
        Me.chkExportSPUICCS.Text = "Include spu_ICCS_%"
        Me.chkExportSPUICCS.UseVisualStyleBackColor = False
        '
        'chkGenerateUMLScript
        '
        Me.chkGenerateUMLScript.BackColor = System.Drawing.SystemColors.Control
        Me.chkGenerateUMLScript.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkGenerateUMLScript.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkGenerateUMLScript.Location = New System.Drawing.Point(261, 57)
        Me.chkGenerateUMLScript.Name = "chkGenerateUMLScript"
        Me.chkGenerateUMLScript.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkGenerateUMLScript.Size = New System.Drawing.Size(201, 21)
        Me.chkGenerateUMLScript.TabIndex = 66
        Me.chkGenerateUMLScript.Text = "Generate Seperate UML Script"
        Me.chkGenerateUMLScript.UseVisualStyleBackColor = False
        '
        'chkIncludeUMLs
        '
        Me.chkIncludeUMLs.BackColor = System.Drawing.SystemColors.Control
        Me.chkIncludeUMLs.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIncludeUMLs.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIncludeUMLs.Location = New System.Drawing.Point(261, 39)
        Me.chkIncludeUMLs.Name = "chkIncludeUMLs"
        Me.chkIncludeUMLs.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIncludeUMLs.Size = New System.Drawing.Size(142, 21)
        Me.chkIncludeUMLs.TabIndex = 65
        Me.chkIncludeUMLs.Text = "Include UMLs"
        Me.chkIncludeUMLs.UseVisualStyleBackColor = False
        '
        '_chkAdditionalExportOptions_6
        '
        Me._chkAdditionalExportOptions_6.BackColor = System.Drawing.SystemColors.Control
        Me._chkAdditionalExportOptions_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkAdditionalExportOptions_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkAdditionalExportOptions_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkAdditionalExportOptions_6.Location = New System.Drawing.Point(12, 136)
        Me._chkAdditionalExportOptions_6.Name = "_chkAdditionalExportOptions_6"
        Me._chkAdditionalExportOptions_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkAdditionalExportOptions_6.Size = New System.Drawing.Size(170, 21)
        Me._chkAdditionalExportOptions_6.TabIndex = 64
        Me._chkAdditionalExportOptions_6.Text = "Include User Defined Lists"
        Me._chkAdditionalExportOptions_6.UseVisualStyleBackColor = False
        '
        '_chkAdditionalExportOptions_4
        '
        Me._chkAdditionalExportOptions_4.BackColor = System.Drawing.SystemColors.Control
        Me._chkAdditionalExportOptions_4.Checked = True
        Me._chkAdditionalExportOptions_4.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkAdditionalExportOptions_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkAdditionalExportOptions_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkAdditionalExportOptions_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkAdditionalExportOptions_4.Location = New System.Drawing.Point(12, 116)
        Me._chkAdditionalExportOptions_4.Name = "_chkAdditionalExportOptions_4"
        Me._chkAdditionalExportOptions_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkAdditionalExportOptions_4.Size = New System.Drawing.Size(170, 21)
        Me._chkAdditionalExportOptions_4.TabIndex = 62
        Me._chkAdditionalExportOptions_4.Text = "Include 3D Lookups"
        Me._chkAdditionalExportOptions_4.UseVisualStyleBackColor = False
        '
        '_chkAdditionalExportOptions_3
        '
        Me._chkAdditionalExportOptions_3.BackColor = System.Drawing.SystemColors.Control
        Me._chkAdditionalExportOptions_3.Checked = True
        Me._chkAdditionalExportOptions_3.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkAdditionalExportOptions_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkAdditionalExportOptions_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkAdditionalExportOptions_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkAdditionalExportOptions_3.Location = New System.Drawing.Point(12, 96)
        Me._chkAdditionalExportOptions_3.Name = "_chkAdditionalExportOptions_3"
        Me._chkAdditionalExportOptions_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkAdditionalExportOptions_3.Size = New System.Drawing.Size(170, 21)
        Me._chkAdditionalExportOptions_3.TabIndex = 60
        Me._chkAdditionalExportOptions_3.Text = "Include Risk Groups/Codes"
        Me._chkAdditionalExportOptions_3.UseVisualStyleBackColor = False
        '
        '_chkAdditionalExportOptions_2
        '
        Me._chkAdditionalExportOptions_2.BackColor = System.Drawing.SystemColors.Control
        Me._chkAdditionalExportOptions_2.Checked = True
        Me._chkAdditionalExportOptions_2.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkAdditionalExportOptions_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkAdditionalExportOptions_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkAdditionalExportOptions_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkAdditionalExportOptions_2.Location = New System.Drawing.Point(12, 77)
        Me._chkAdditionalExportOptions_2.Name = "_chkAdditionalExportOptions_2"
        Me._chkAdditionalExportOptions_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkAdditionalExportOptions_2.Size = New System.Drawing.Size(170, 21)
        Me._chkAdditionalExportOptions_2.TabIndex = 20
        Me._chkAdditionalExportOptions_2.Text = "Include rule files"
        Me._chkAdditionalExportOptions_2.UseVisualStyleBackColor = False
        '
        '_chkAdditionalExportOptions_1
        '
        Me._chkAdditionalExportOptions_1.BackColor = System.Drawing.SystemColors.Control
        Me._chkAdditionalExportOptions_1.Checked = True
        Me._chkAdditionalExportOptions_1.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkAdditionalExportOptions_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkAdditionalExportOptions_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkAdditionalExportOptions_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkAdditionalExportOptions_1.Location = New System.Drawing.Point(12, 39)
        Me._chkAdditionalExportOptions_1.Name = "_chkAdditionalExportOptions_1"
        Me._chkAdditionalExportOptions_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkAdditionalExportOptions_1.Size = New System.Drawing.Size(170, 21)
        Me._chkAdditionalExportOptions_1.TabIndex = 19
        Me._chkAdditionalExportOptions_1.Text = "Include document templates"
        Me._chkAdditionalExportOptions_1.UseVisualStyleBackColor = False
        '
        '_chkAdditionalExportOptions_0
        '
        Me._chkAdditionalExportOptions_0.BackColor = System.Drawing.SystemColors.Control
        Me._chkAdditionalExportOptions_0.Checked = True
        Me._chkAdditionalExportOptions_0.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkAdditionalExportOptions_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkAdditionalExportOptions_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkAdditionalExportOptions_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkAdditionalExportOptions_0.Location = New System.Drawing.Point(12, 19)
        Me._chkAdditionalExportOptions_0.Name = "_chkAdditionalExportOptions_0"
        Me._chkAdditionalExportOptions_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkAdditionalExportOptions_0.Size = New System.Drawing.Size(170, 21)
        Me._chkAdditionalExportOptions_0.TabIndex = 18
        Me._chkAdditionalExportOptions_0.Text = "Include registry settings"
        Me._chkAdditionalExportOptions_0.UseVisualStyleBackColor = False
        '
        '_SSTab2_TabPage2
        '
        Me._SSTab2_TabPage2.Controls.Add(Me.Frame6)
        Me._SSTab2_TabPage2.Controls.Add(Me.fraExportHeader)
        Me._SSTab2_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._SSTab2_TabPage2.Name = "_SSTab2_TabPage2"
        Me._SSTab2_TabPage2.Size = New System.Drawing.Size(702, 292)
        Me._SSTab2_TabPage2.TabIndex = 2
        Me._SSTab2_TabPage2.Text = "3) Confirm Details"
        '
        'Frame6
        '
        Me.Frame6.BackColor = System.Drawing.SystemColors.Control
        Me.Frame6.Controls.Add(Me.txtExportHeaderComment)
        Me.Frame6.Controls.Add(Me.lblComment)
        Me.Frame6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame6.Location = New System.Drawing.Point(16, 167)
        Me.Frame6.Name = "Frame6"
        Me.Frame6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame6.Size = New System.Drawing.Size(677, 112)
        Me.Frame6.TabIndex = 21
        Me.Frame6.TabStop = False
        Me.Frame6.Text = "Please enter any additional information to be displayed during the import process" & _
            ""
        '
        'txtExportHeaderComment
        '
        Me.txtExportHeaderComment.AcceptsReturn = True
        Me.txtExportHeaderComment.BackColor = System.Drawing.SystemColors.Window
        Me.txtExportHeaderComment.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtExportHeaderComment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExportHeaderComment.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtExportHeaderComment.Location = New System.Drawing.Point(10, 34)
        Me.txtExportHeaderComment.MaxLength = 0
        Me.txtExportHeaderComment.Multiline = True
        Me.txtExportHeaderComment.Name = "txtExportHeaderComment"
        Me.txtExportHeaderComment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtExportHeaderComment.Size = New System.Drawing.Size(662, 71)
        Me.txtExportHeaderComment.TabIndex = 22
        '
        'lblComment
        '
        Me.lblComment.BackColor = System.Drawing.SystemColors.Control
        Me.lblComment.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblComment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblComment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblComment.Location = New System.Drawing.Point(10, 18)
        Me.lblComment.Name = "lblComment"
        Me.lblComment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblComment.Size = New System.Drawing.Size(129, 17)
        Me.lblComment.TabIndex = 23
        Me.lblComment.Text = "Comment"
        '
        'fraExportHeader
        '
        Me.fraExportHeader.BackColor = System.Drawing.SystemColors.Control
        Me.fraExportHeader.Controls.Add(Me.txtExportConfirmationText)
        Me.fraExportHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraExportHeader.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraExportHeader.Location = New System.Drawing.Point(16, 11)
        Me.fraExportHeader.Name = "fraExportHeader"
        Me.fraExportHeader.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraExportHeader.Size = New System.Drawing.Size(677, 142)
        Me.fraExportHeader.TabIndex = 13
        Me.fraExportHeader.TabStop = False
        Me.fraExportHeader.Text = "Export Information"
        '
        'txtExportConfirmationText
        '
        Me.txtExportConfirmationText.AcceptsReturn = True
        Me.txtExportConfirmationText.BackColor = System.Drawing.SystemColors.Window
        Me.txtExportConfirmationText.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtExportConfirmationText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExportConfirmationText.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtExportConfirmationText.Location = New System.Drawing.Point(8, 16)
        Me.txtExportConfirmationText.MaxLength = 0
        Me.txtExportConfirmationText.Multiline = True
        Me.txtExportConfirmationText.Name = "txtExportConfirmationText"
        Me.txtExportConfirmationText.ReadOnly = True
        Me.txtExportConfirmationText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtExportConfirmationText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtExportConfirmationText.Size = New System.Drawing.Size(661, 118)
        Me.txtExportConfirmationText.TabIndex = 50
        '
        '_SSTab2_TabPage3
        '
        Me._SSTab2_TabPage3.Controls.Add(Me._txtWarning_1)
        Me._SSTab2_TabPage3.Controls.Add(Me.cmdExportCancel)
        Me._SSTab2_TabPage3.Controls.Add(Me.cmdExport)
        Me._SSTab2_TabPage3.Controls.Add(Me.lblExportInfo)
        Me._SSTab2_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._SSTab2_TabPage3.Name = "_SSTab2_TabPage3"
        Me._SSTab2_TabPage3.Size = New System.Drawing.Size(702, 292)
        Me._SSTab2_TabPage3.TabIndex = 3
        Me._SSTab2_TabPage3.Text = "4) Run Export"
        '
        '_txtWarning_1
        '
        Me._txtWarning_1.AcceptsReturn = True
        Me._txtWarning_1.BackColor = System.Drawing.SystemColors.Window
        Me._txtWarning_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtWarning_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtWarning_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtWarning_1.Location = New System.Drawing.Point(10, 26)
        Me._txtWarning_1.MaxLength = 0
        Me._txtWarning_1.Multiline = True
        Me._txtWarning_1.Name = "_txtWarning_1"
        Me._txtWarning_1.ReadOnly = True
        Me._txtWarning_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtWarning_1.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me._txtWarning_1.Size = New System.Drawing.Size(641, 209)
        Me._txtWarning_1.TabIndex = 39
        Me._txtWarning_1.WordWrap = False
        '
        'cmdExportCancel
        '
        Me.cmdExportCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExportCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExportCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExportCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExportCancel.Location = New System.Drawing.Point(486, 247)
        Me.cmdExportCancel.Name = "cmdExportCancel"
        Me.cmdExportCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExportCancel.Size = New System.Drawing.Size(79, 27)
        Me.cmdExportCancel.TabIndex = 37
        Me.cmdExportCancel.Text = "&Cancel"
        Me.cmdExportCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdExportCancel.UseVisualStyleBackColor = False
        Me.cmdExportCancel.Visible = False
        '
        'cmdExport
        '
        Me.cmdExport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExport.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExport.Location = New System.Drawing.Point(569, 247)
        Me.cmdExport.Name = "cmdExport"
        Me.cmdExport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExport.Size = New System.Drawing.Size(79, 27)
        Me.cmdExport.TabIndex = 11
        Me.cmdExport.Text = "&Export"
        Me.cmdExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdExport.UseVisualStyleBackColor = False
        '
        'lblExportInfo
        '
        Me.lblExportInfo.BackColor = System.Drawing.SystemColors.Control
        Me.lblExportInfo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExportInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExportInfo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExportInfo.Location = New System.Drawing.Point(10, 8)
        Me.lblExportInfo.Name = "lblExportInfo"
        Me.lblExportInfo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExportInfo.Size = New System.Drawing.Size(129, 17)
        Me.lblExportInfo.TabIndex = 12
        Me.lblExportInfo.Text = "Export Information"
        '
        '_SSTab1_TabPage1
        '
        Me._SSTab1_TabPage1.Controls.Add(Me.SSTab3)
        Me._SSTab1_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage1.Name = "_SSTab1_TabPage1"
        Me._SSTab1_TabPage1.Size = New System.Drawing.Size(722, 327)
        Me._SSTab1_TabPage1.TabIndex = 1
        Me._SSTab1_TabPage1.Text = "Import"
        '
        'SSTab3
        '
        Me.SSTab3.Controls.Add(Me._SSTab3_TabPage0)
        Me.SSTab3.Controls.Add(Me._SSTab3_TabPage1)
        Me.SSTab3.Controls.Add(Me._SSTab3_TabPage2)
        Me.SSTab3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab3.ItemSize = New System.Drawing.Size(234, 18)
        Me.SSTab3.Location = New System.Drawing.Point(9, 10)
        Me.SSTab3.Multiline = True
        Me.SSTab3.Name = "SSTab3"
        Me.SSTab3.SelectedIndex = 0
        Me.SSTab3.Size = New System.Drawing.Size(710, 318)
        Me.SSTab3.TabIndex = 15
        '
        '_SSTab3_TabPage0
        '
        Me._SSTab3_TabPage0.Controls.Add(Me.Frame2)
        Me._SSTab3_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab3_TabPage0.Name = "_SSTab3_TabPage0"
        Me._SSTab3_TabPage0.Size = New System.Drawing.Size(702, 292)
        Me._SSTab3_TabPage0.TabIndex = 0
        Me._SSTab3_TabPage0.Text = "1) Select File"
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.cmdImportBrowse)
        Me.Frame2.Controls.Add(Me.chkImportUMLScriptsOnly)
        Me.Frame2.Controls.Add(Me._txtFileExtension_0)
        Me.Frame2.Controls.Add(Me._txtFileName_0)
        Me.Frame2.Controls.Add(Me._txtFilePath_0)
        Me.Frame2.Controls.Add(Me._Label3_5)
        Me.Frame2.Controls.Add(Me._Label3_4)
        Me.Frame2.Controls.Add(Me._Label3_3)
        Me.Frame2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(22, 26)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(656, 114)
        Me.Frame2.TabIndex = 33
        Me.Frame2.TabStop = False
        '
        'cmdImportBrowse
        '
        Me.cmdImportBrowse.BackColor = System.Drawing.SystemColors.Control
        Me.cmdImportBrowse.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdImportBrowse.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdImportBrowse.Location = New System.Drawing.Point(333, 25)
        Me.cmdImportBrowse.Name = "cmdImportBrowse"
        Me.cmdImportBrowse.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdImportBrowse.Size = New System.Drawing.Size(25, 20)
        Me.cmdImportBrowse.TabIndex = 78
        Me.cmdImportBrowse.Text = "..."
        Me.cmdImportBrowse.UseVisualStyleBackColor = False
        '
        'chkImportUMLScriptsOnly
        '
        Me.chkImportUMLScriptsOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkImportUMLScriptsOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkImportUMLScriptsOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkImportUMLScriptsOnly.Location = New System.Drawing.Point(461, 26)
        Me.chkImportUMLScriptsOnly.Name = "chkImportUMLScriptsOnly"
        Me.chkImportUMLScriptsOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkImportUMLScriptsOnly.Size = New System.Drawing.Size(189, 21)
        Me.chkImportUMLScriptsOnly.TabIndex = 77
        Me.chkImportUMLScriptsOnly.Text = "Import UML Scripts Only"
        Me.chkImportUMLScriptsOnly.UseVisualStyleBackColor = False
        '
        '_txtFileExtension_0
        '
        Me._txtFileExtension_0.AcceptsReturn = True
        Me._txtFileExtension_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtFileExtension_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFileExtension_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtFileExtension_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtFileExtension_0.Location = New System.Drawing.Point(77, 74)
        Me._txtFileExtension_0.MaxLength = 0
        Me._txtFileExtension_0.Name = "_txtFileExtension_0"
        Me._txtFileExtension_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFileExtension_0.Size = New System.Drawing.Size(33, 20)
        Me._txtFileExtension_0.TabIndex = 36
        Me._txtFileExtension_0.Text = "pbe"
        '
        '_txtFileName_0
        '
        Me._txtFileName_0.AcceptsReturn = True
        Me._txtFileName_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtFileName_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFileName_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtFileName_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtFileName_0.Location = New System.Drawing.Point(77, 50)
        Me._txtFileName_0.MaxLength = 0
        Me._txtFileName_0.Name = "_txtFileName_0"
        Me._txtFileName_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFileName_0.Size = New System.Drawing.Size(250, 20)
        Me._txtFileName_0.TabIndex = 35
        Me._txtFileName_0.Text = "pbexport"
        '
        '_txtFilePath_0
        '
        Me._txtFilePath_0.AcceptsReturn = True
        Me._txtFilePath_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtFilePath_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFilePath_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtFilePath_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtFilePath_0.Location = New System.Drawing.Point(77, 23)
        Me._txtFilePath_0.MaxLength = 0
        Me._txtFilePath_0.Name = "_txtFilePath_0"
        Me._txtFilePath_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFilePath_0.Size = New System.Drawing.Size(250, 20)
        Me._txtFilePath_0.TabIndex = 34
        Me._txtFilePath_0.Text = "c:\temp\"
        '
        '_Label3_5
        '
        Me._Label3_5.BackColor = System.Drawing.SystemColors.Control
        Me._Label3_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label3_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Label3_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._Label3_5.Location = New System.Drawing.Point(19, 26)
        Me._Label3_5.Name = "_Label3_5"
        Me._Label3_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label3_5.Size = New System.Drawing.Size(71, 15)
        Me._Label3_5.TabIndex = 42
        Me._Label3_5.Text = "Path"
        '
        '_Label3_4
        '
        Me._Label3_4.BackColor = System.Drawing.SystemColors.Control
        Me._Label3_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label3_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Label3_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._Label3_4.Location = New System.Drawing.Point(19, 51)
        Me._Label3_4.Name = "_Label3_4"
        Me._Label3_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label3_4.Size = New System.Drawing.Size(71, 15)
        Me._Label3_4.TabIndex = 43
        Me._Label3_4.Text = "Name"
        '
        '_Label3_3
        '
        Me._Label3_3.BackColor = System.Drawing.SystemColors.Control
        Me._Label3_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label3_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Label3_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._Label3_3.Location = New System.Drawing.Point(19, 76)
        Me._Label3_3.Name = "_Label3_3"
        Me._Label3_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label3_3.Size = New System.Drawing.Size(71, 15)
        Me._Label3_3.TabIndex = 44
        Me._Label3_3.Text = "Extension"
        '
        '_SSTab3_TabPage1
        '
        Me._SSTab3_TabPage1.Controls.Add(Me.fraImportHeader)
        Me._SSTab3_TabPage1.Controls.Add(Me.Frame7)
        Me._SSTab3_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._SSTab3_TabPage1.Name = "_SSTab3_TabPage1"
        Me._SSTab3_TabPage1.Size = New System.Drawing.Size(702, 292)
        Me._SSTab3_TabPage1.TabIndex = 1
        Me._SSTab3_TabPage1.Text = "2) Confirm Details"
        '
        'fraImportHeader
        '
        Me.fraImportHeader.BackColor = System.Drawing.SystemColors.Control
        Me.fraImportHeader.Controls.Add(Me.txtImportConfirmation)
        Me.fraImportHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraImportHeader.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraImportHeader.Location = New System.Drawing.Point(19, 12)
        Me.fraImportHeader.Name = "fraImportHeader"
        Me.fraImportHeader.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraImportHeader.Size = New System.Drawing.Size(683, 196)
        Me.fraImportHeader.TabIndex = 41
        Me.fraImportHeader.TabStop = False
        Me.fraImportHeader.Text = "Header Information"
        '
        'txtImportConfirmation
        '
        Me.txtImportConfirmation.AcceptsReturn = True
        Me.txtImportConfirmation.BackColor = System.Drawing.SystemColors.Window
        Me.txtImportConfirmation.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtImportConfirmation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtImportConfirmation.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtImportConfirmation.Location = New System.Drawing.Point(8, 16)
        Me.txtImportConfirmation.MaxLength = 0
        Me.txtImportConfirmation.Multiline = True
        Me.txtImportConfirmation.Name = "txtImportConfirmation"
        Me.txtImportConfirmation.ReadOnly = True
        Me.txtImportConfirmation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtImportConfirmation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtImportConfirmation.Size = New System.Drawing.Size(665, 169)
        Me.txtImportConfirmation.TabIndex = 49
        '
        'Frame7
        '
        Me.Frame7.BackColor = System.Drawing.SystemColors.Control
        Me.Frame7.Controls.Add(Me._optAdditionalImportOptions_1)
        Me.Frame7.Controls.Add(Me._optAdditionalImportOptions_2)
        Me.Frame7.Controls.Add(Me._optAdditionalImportOptions_0)
        Me.Frame7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame7.Location = New System.Drawing.Point(19, 219)
        Me.Frame7.Name = "Frame7"
        Me.Frame7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame7.Size = New System.Drawing.Size(317, 65)
        Me.Frame7.TabIndex = 45
        Me.Frame7.TabStop = False
        Me.Frame7.Text = "Additional Options"
        '
        '_optAdditionalImportOptions_1
        '
        Me._optAdditionalImportOptions_1.BackColor = System.Drawing.SystemColors.Control
        Me._optAdditionalImportOptions_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optAdditionalImportOptions_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optAdditionalImportOptions_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optAdditionalImportOptions_1.Location = New System.Drawing.Point(116, 18)
        Me._optAdditionalImportOptions_1.Name = "_optAdditionalImportOptions_1"
        Me._optAdditionalImportOptions_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optAdditionalImportOptions_1.Size = New System.Drawing.Size(93, 29)
        Me._optAdditionalImportOptions_1.TabIndex = 48
        Me._optAdditionalImportOptions_1.TabStop = True
        Me._optAdditionalImportOptions_1.Text = "Create default registry settings"
        Me._optAdditionalImportOptions_1.UseVisualStyleBackColor = False
        '
        '_optAdditionalImportOptions_2
        '
        Me._optAdditionalImportOptions_2.BackColor = System.Drawing.SystemColors.Control
        Me._optAdditionalImportOptions_2.Checked = True
        Me._optAdditionalImportOptions_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optAdditionalImportOptions_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optAdditionalImportOptions_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optAdditionalImportOptions_2.Location = New System.Drawing.Point(227, 18)
        Me._optAdditionalImportOptions_2.Name = "_optAdditionalImportOptions_2"
        Me._optAdditionalImportOptions_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optAdditionalImportOptions_2.Size = New System.Drawing.Size(84, 29)
        Me._optAdditionalImportOptions_2.TabIndex = 47
        Me._optAdditionalImportOptions_2.TabStop = True
        Me._optAdditionalImportOptions_2.Text = "No registry changes"
        Me._optAdditionalImportOptions_2.UseVisualStyleBackColor = False
        '
        '_optAdditionalImportOptions_0
        '
        Me._optAdditionalImportOptions_0.BackColor = System.Drawing.SystemColors.Control
        Me._optAdditionalImportOptions_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optAdditionalImportOptions_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optAdditionalImportOptions_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optAdditionalImportOptions_0.Location = New System.Drawing.Point(12, 18)
        Me._optAdditionalImportOptions_0.Name = "_optAdditionalImportOptions_0"
        Me._optAdditionalImportOptions_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optAdditionalImportOptions_0.Size = New System.Drawing.Size(90, 29)
        Me._optAdditionalImportOptions_0.TabIndex = 46
        Me._optAdditionalImportOptions_0.TabStop = True
        Me._optAdditionalImportOptions_0.Text = "Import registry settings"
        Me._optAdditionalImportOptions_0.UseVisualStyleBackColor = False
        '
        '_SSTab3_TabPage2
        '
        Me._SSTab3_TabPage2.Controls.Add(Me._txtWarning_0)
        Me._SSTab3_TabPage2.Controls.Add(Me.cmdImport)
        Me._SSTab3_TabPage2.Controls.Add(Me.Label4)
        Me._SSTab3_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._SSTab3_TabPage2.Name = "_SSTab3_TabPage2"
        Me._SSTab3_TabPage2.Size = New System.Drawing.Size(702, 292)
        Me._SSTab3_TabPage2.TabIndex = 2
        Me._SSTab3_TabPage2.Text = "3) Run Import"
        '
        '_txtWarning_0
        '
        Me._txtWarning_0.AcceptsReturn = True
        Me._txtWarning_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtWarning_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtWarning_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtWarning_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtWarning_0.Location = New System.Drawing.Point(11, 27)
        Me._txtWarning_0.MaxLength = 0
        Me._txtWarning_0.Multiline = True
        Me._txtWarning_0.Name = "_txtWarning_0"
        Me._txtWarning_0.ReadOnly = True
        Me._txtWarning_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtWarning_0.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me._txtWarning_0.Size = New System.Drawing.Size(677, 209)
        Me._txtWarning_0.TabIndex = 40
        Me._txtWarning_0.WordWrap = False
        '
        'cmdImport
        '
        Me.cmdImport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdImport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdImport.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdImport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdImport.Location = New System.Drawing.Point(607, 249)
        Me.cmdImport.Name = "cmdImport"
        Me.cmdImport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdImport.Size = New System.Drawing.Size(79, 29)
        Me.cmdImport.TabIndex = 16
        Me.cmdImport.Text = "Import"
        Me.cmdImport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdImport.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(12, 8)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(129, 17)
        Me.Label4.TabIndex = 38
        Me.Label4.Text = "Import Information"
        '
        '_SSTab1_TabPage2
        '
        Me._SSTab1_TabPage2.Controls.Add(Me._cmdDebug_4)
        Me._SSTab1_TabPage2.Controls.Add(Me._cmdDebug_3)
        Me._SSTab1_TabPage2.Controls.Add(Me._cmdDebug_2)
        Me._SSTab1_TabPage2.Controls.Add(Me._cmdDebug_1)
        Me._SSTab1_TabPage2.Controls.Add(Me._cmdDebug_0)
        Me._SSTab1_TabPage2.Controls.Add(Me.txtDebug)
        Me._SSTab1_TabPage2.Controls.Add(Me.Label1)
        Me._SSTab1_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage2.Name = "_SSTab1_TabPage2"
        Me._SSTab1_TabPage2.Size = New System.Drawing.Size(722, 327)
        Me._SSTab1_TabPage2.TabIndex = 2
        Me._SSTab1_TabPage2.Text = "Debug"
        '
        '_cmdDebug_4
        '
        Me._cmdDebug_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdDebug_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdDebug_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdDebug_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdDebug_4.Location = New System.Drawing.Point(628, 84)
        Me._cmdDebug_4.Name = "_cmdDebug_4"
        Me._cmdDebug_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdDebug_4.Size = New System.Drawing.Size(95, 29)
        Me._cmdDebug_4.TabIndex = 58
        Me._cmdDebug_4.Text = "Create XML dataset"
        Me._cmdDebug_4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdDebug_4.UseVisualStyleBackColor = False
        '
        '_cmdDebug_3
        '
        Me._cmdDebug_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdDebug_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdDebug_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdDebug_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdDebug_3.Location = New System.Drawing.Point(627, 53)
        Me._cmdDebug_3.Name = "_cmdDebug_3"
        Me._cmdDebug_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdDebug_3.Size = New System.Drawing.Size(96, 29)
        Me._cmdDebug_3.TabIndex = 57
        Me._cmdDebug_3.Text = "Create default registry settings"
        Me._cmdDebug_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdDebug_3.UseVisualStyleBackColor = False
        '
        '_cmdDebug_2
        '
        Me._cmdDebug_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdDebug_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdDebug_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdDebug_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdDebug_2.Location = New System.Drawing.Point(627, 181)
        Me._cmdDebug_2.Name = "_cmdDebug_2"
        Me._cmdDebug_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdDebug_2.Size = New System.Drawing.Size(94, 23)
        Me._cmdDebug_2.TabIndex = 56
        Me._cmdDebug_2.Text = "Re-build captions"
        Me._cmdDebug_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdDebug_2.UseVisualStyleBackColor = False
        '
        '_cmdDebug_1
        '
        Me._cmdDebug_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdDebug_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdDebug_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdDebug_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdDebug_1.Location = New System.Drawing.Point(41, 295)
        Me._cmdDebug_1.Name = "_cmdDebug_1"
        Me._cmdDebug_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdDebug_1.Size = New System.Drawing.Size(132, 23)
        Me._cmdDebug_1.TabIndex = 55
        Me._cmdDebug_1.Text = "Generate object captions "
        Me._cmdDebug_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdDebug_1.UseVisualStyleBackColor = False
        '
        '_cmdDebug_0
        '
        Me._cmdDebug_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdDebug_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdDebug_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdDebug_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdDebug_0.Location = New System.Drawing.Point(627, 28)
        Me._cmdDebug_0.Name = "_cmdDebug_0"
        Me._cmdDebug_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdDebug_0.Size = New System.Drawing.Size(95, 24)
        Me._cmdDebug_0.TabIndex = 54
        Me._cmdDebug_0.Text = "Create default lists"
        Me._cmdDebug_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdDebug_0.UseVisualStyleBackColor = False
        '
        'txtDebug
        '
        Me.txtDebug.AcceptsReturn = True
        Me.txtDebug.BackColor = System.Drawing.SystemColors.Window
        Me.txtDebug.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDebug.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDebug.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDebug.Location = New System.Drawing.Point(24, 14)
        Me.txtDebug.MaxLength = 0
        Me.txtDebug.Multiline = True
        Me.txtDebug.Name = "txtDebug"
        Me.txtDebug.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDebug.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtDebug.Size = New System.Drawing.Size(599, 270)
        Me.txtDebug.TabIndex = 2
        Me.txtDebug.WordWrap = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(628, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(94, 22)
        Me.Label1.TabIndex = 59
        Me.Label1.Text = "Datamodel options"
        '
        'mainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(736, 464)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.cboGISLookupUserList)
        Me.Controls.Add(Me.SSTab1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "mainForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Product Builder Product Export/Import Utility"
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me._StatusBar1_2.ResumeLayout(False)
        Me._StatusBar1_2.PerformLayout()
        Me._StatusBar1_1.ResumeLayout(False)
        Me._StatusBar1_1.PerformLayout()
        Me._StatusBar1_0.ResumeLayout(False)
        Me._StatusBar1_0.PerformLayout()
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me.SSTab2.ResumeLayout(False)
        Me._SSTab2_TabPage0.ResumeLayout(False)
        Me.Frame3.ResumeLayout(False)
        Me.Frame3.PerformLayout()
        Me._SSTab2_TabPage1.ResumeLayout(False)
        Me.Frame4.ResumeLayout(False)
        Me.Frame4.PerformLayout()
        Me.Frame5.ResumeLayout(False)
        Me._SSTab2_TabPage2.ResumeLayout(False)
        Me.Frame6.ResumeLayout(False)
        Me.Frame6.PerformLayout()
        Me.fraExportHeader.ResumeLayout(False)
        Me.fraExportHeader.PerformLayout()
        Me._SSTab2_TabPage3.ResumeLayout(False)
        Me._SSTab2_TabPage3.PerformLayout()
        Me._SSTab1_TabPage1.ResumeLayout(False)
        Me.SSTab3.ResumeLayout(False)
        Me._SSTab3_TabPage0.ResumeLayout(False)
        Me.Frame2.ResumeLayout(False)
        Me.Frame2.PerformLayout()
        Me._SSTab3_TabPage1.ResumeLayout(False)
        Me.fraImportHeader.ResumeLayout(False)
        Me.fraImportHeader.PerformLayout()
        Me.Frame7.ResumeLayout(False)
        Me._SSTab3_TabPage2.ResumeLayout(False)
        Me._SSTab3_TabPage2.PerformLayout()
        Me._SSTab1_TabPage2.ResumeLayout(False)
        Me._SSTab1_TabPage2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Sub InitializetxtWarning()
        Me.txtWarning(0) = _txtWarning_0
        Me.txtWarning(1) = _txtWarning_1
    End Sub
    Sub InitializetxtFilePath()
        Me.txtFilePath(0) = _txtFilePath_0
        Me.txtFilePath(1) = _txtFilePath_1
    End Sub
    Sub InitializetxtFileName()
        Me.txtFileName(0) = _txtFileName_0
        Me.txtFileName(1) = _txtFileName_1
    End Sub
    Sub InitializetxtFileExtension()
        Me.txtFileExtension(0) = _txtFileExtension_0
        Me.txtFileExtension(1) = _txtFileExtension_1
    End Sub
    Sub InitializeradioExportBasedOn()
        Me.radioExportBasedOn(3) = _radioExportBasedOn_3
        Me.radioExportBasedOn(2) = _radioExportBasedOn_2
        Me.radioExportBasedOn(1) = _radioExportBasedOn_1
        Me.radioExportBasedOn(0) = _radioExportBasedOn_0
    End Sub
    Sub InitializeoptAdditionalImportOptions()
        Me.optAdditionalImportOptions(1) = _optAdditionalImportOptions_1
        Me.optAdditionalImportOptions(2) = _optAdditionalImportOptions_2
        Me.optAdditionalImportOptions(0) = _optAdditionalImportOptions_0
    End Sub
    Sub InitializecmdDebug()
        Me.cmdDebug(4) = _cmdDebug_4
        Me.cmdDebug(3) = _cmdDebug_3
        Me.cmdDebug(2) = _cmdDebug_2
        Me.cmdDebug(1) = _cmdDebug_1
        Me.cmdDebug(0) = _cmdDebug_0
    End Sub
    Sub InitializechkAdditionalExportOptions()
        Me.chkAdditionalExportOptions(6) = chkExportSPUICCS
        Me.chkAdditionalExportOptions(6) = _chkAdditionalExportOptions_6
        Me.chkAdditionalExportOptions(5) = _chkAdditionalExportOptions_5
        Me.chkAdditionalExportOptions(4) = _chkAdditionalExportOptions_4
        Me.chkAdditionalExportOptions(3) = _chkAdditionalExportOptions_3
        Me.chkAdditionalExportOptions(2) = _chkAdditionalExportOptions_2
        Me.chkAdditionalExportOptions(1) = _chkAdditionalExportOptions_1
        Me.chkAdditionalExportOptions(0) = _chkAdditionalExportOptions_0
    End Sub
    Sub InitializeStatusBar1()
        Me.StatusBar1(0) = _StatusBar1_0
        Me.StatusBar1(1) = _StatusBar1_1
        Me.StatusBar1(2) = _StatusBar1_2
    End Sub
    Sub InitializeProgressBar1()
        Me.ProgressBar1(0) = _ProgressBar1_0
        Me.ProgressBar1(1) = _ProgressBar1_1
    End Sub
    Sub InitializeLabel3()
        Me.Label3(5) = _Label3_5
        Me.Label3(4) = _Label3_4
        Me.Label3(3) = _Label3_3
        Me.Label3(2) = _Label3_2
        Me.Label3(1) = _Label3_1
        Me.Label3(0) = _Label3_0
    End Sub
    Public WithEvents chkGenerateUMLScript As System.Windows.Forms.CheckBox
    Public WithEvents chkIncludeUMLs As System.Windows.Forms.CheckBox
    Public WithEvents chkImportUMLScriptsOnly As System.Windows.Forms.CheckBox
    Public WithEvents chkExportSPUICCS As System.Windows.Forms.CheckBox
    Public WithEvents cmdImportBrowse As System.Windows.Forms.Button
    Friend WithEvents CommonDialog1Open As System.Windows.Forms.OpenFileDialog
#End Region 
End Class