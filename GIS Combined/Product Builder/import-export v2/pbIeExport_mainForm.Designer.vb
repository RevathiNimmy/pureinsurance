<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class mainForm
#Region "Windows Form Designer generated code "
    <System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        Form_Initialize_Renamed()
    End Sub
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
        If Disposing Then
            If Not components Is Nothing Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(Disposing)
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents _ProgressBar1_0 As System.Windows.Forms.ProgressBar
    Public WithEvents _ProgressBar1_1 As System.Windows.Forms.ProgressBar
    Public WithEvents lblDebugOn As System.Windows.Forms.Label
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Public WithEvents cboGISLookupUserList As uctGISUserDefLookupControl.cboGISLookup
    Public WithEvents cmdBrowse As System.Windows.Forms.Button
    Public WithEvents _txtFileExtension_1 As System.Windows.Forms.TextBox
    Public WithEvents _txtFileName_1 As System.Windows.Forms.TextBox
    Public WithEvents _txtFilePath_1 As System.Windows.Forms.TextBox
    Public WithEvents _Label3_2 As System.Windows.Forms.Label
    Public WithEvents _Label3_1 As System.Windows.Forms.Label
    Public WithEvents _Label3_0 As System.Windows.Forms.Label
    Public WithEvents Frame3 As System.Windows.Forms.GroupBox
    Public WithEvents cmdRestoreUsers As System.Windows.Forms.Button
    Public CommonDialog1Open As System.Windows.Forms.OpenFileDialog
    Public CommonDialog1Save As System.Windows.Forms.SaveFileDialog
    Public CommonDialog1Font As System.Windows.Forms.FontDialog
    Public CommonDialog1Color As System.Windows.Forms.ColorDialog
    Public CommonDialog1Print As System.Windows.Forms.PrintDialog
    Public WithEvents _SSTab2_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents tvDataModel As System.Windows.Forms.TreeView
    Public WithEvents Frame10 As System.Windows.Forms.GroupBox
    Public WithEvents tvUDL As System.Windows.Forms.TreeView
    Public WithEvents Frame9 As System.Windows.Forms.GroupBox
    Public WithEvents _chkAdditionalExportOptions_10 As System.Windows.Forms.CheckBox
    Public WithEvents _chkAdditionalExportOptions_9 As System.Windows.Forms.CheckBox
    Public WithEvents _chkAdditionalExportOptions_8 As System.Windows.Forms.CheckBox
    Public WithEvents _chkAdditionalExportOptions_7 As System.Windows.Forms.CheckBox
    Public WithEvents _chkAdditionalExportOptions_6 As System.Windows.Forms.CheckBox
    Public WithEvents _chkAdditionalExportOptions_2 As System.Windows.Forms.CheckBox
    Public WithEvents _chkAdditionalExportOptions_1 As System.Windows.Forms.CheckBox
    Public WithEvents _chkAdditionalExportOptions_0 As System.Windows.Forms.CheckBox
    Public WithEvents Frame5 As System.Windows.Forms.GroupBox
    Public WithEvents _SSTab2_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents txtExportHeaderComment As System.Windows.Forms.TextBox
    Public WithEvents lblComment As System.Windows.Forms.Label
    Public WithEvents Frame6 As System.Windows.Forms.GroupBox
    Public WithEvents txtExportConfirmationText As System.Windows.Forms.TextBox
    Public WithEvents fraExportHeader As System.Windows.Forms.GroupBox
    Public WithEvents _SSTab2_TabPage2 As System.Windows.Forms.TabPage
    Public WithEvents _txtWarning_1 As System.Windows.Forms.TextBox
    Public WithEvents cmdExportCancel As System.Windows.Forms.Button
    Public WithEvents cmdExport As System.Windows.Forms.Button
    Public WithEvents lblExportInfo As System.Windows.Forms.Label
    Public WithEvents _SSTab2_TabPage3 As System.Windows.Forms.TabPage
    Public WithEvents SSTab2 As System.Windows.Forms.TabControl
    Public WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents chkTarget As System.Windows.Forms.CheckBox
    Public WithEvents cmdImportBrowse As System.Windows.Forms.Button
    Public CommonDialog2Save As System.Windows.Forms.SaveFileDialog
    Public WithEvents _txtFileExtension_0 As System.Windows.Forms.TextBox
    Public WithEvents _txtFileName_0 As System.Windows.Forms.TextBox
    Public WithEvents _txtFilePath_0 As System.Windows.Forms.TextBox
    Public WithEvents lblCheckTarget As System.Windows.Forms.Label
    Public WithEvents _Label3_5 As System.Windows.Forms.Label
    Public WithEvents _Label3_4 As System.Windows.Forms.Label
    Public WithEvents _Label3_3 As System.Windows.Forms.Label
    Public WithEvents Frame2 As System.Windows.Forms.GroupBox
    Public WithEvents chkBackup As System.Windows.Forms.CheckBox
    Public WithEvents cmdBackupBrowse As System.Windows.Forms.Button
    Public WithEvents Dir1 As Microsoft.VisualBasic.Compatibility.VB6.DirListBox
    Public WithEvents Drive1 As Microsoft.VisualBasic.Compatibility.VB6.DriveListBox
    Public WithEvents txtBackupDir As System.Windows.Forms.TextBox
    Public WithEvents lblBackupInfo As System.Windows.Forms.Label
    Public WithEvents _lblBackup_6 As System.Windows.Forms.Label
    Public WithEvents Frame8 As System.Windows.Forms.GroupBox
    Public WithEvents _SSTab3_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents txtManualChanges As System.Windows.Forms.TextBox
    Public WithEvents _SSTab3_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents _optAdditionalImportOptions_0 As System.Windows.Forms.RadioButton
    Public WithEvents _optAdditionalImportOptions_2 As System.Windows.Forms.RadioButton
    Public WithEvents _optAdditionalImportOptions_1 As System.Windows.Forms.RadioButton
    Public WithEvents Frame7 As System.Windows.Forms.GroupBox
    Public WithEvents txtImportConfirmation As System.Windows.Forms.TextBox
    Public WithEvents fraImportHeader As System.Windows.Forms.GroupBox
    Public WithEvents _SSTab3_TabPage2 As System.Windows.Forms.TabPage
    Public WithEvents cmdImport As System.Windows.Forms.Button
    Public WithEvents _txtWarning_0 As System.Windows.Forms.TextBox
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents _SSTab3_TabPage3 As System.Windows.Forms.TabPage
    Public WithEvents SSTab3 As System.Windows.Forms.TabControl
    Public WithEvents _SSTab1_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents _cmdDebug_4 As System.Windows.Forms.Button
    Public WithEvents _cmdDebug_3 As System.Windows.Forms.Button
    Public WithEvents _cmdDebug_2 As System.Windows.Forms.Button
    Public WithEvents _cmdDebug_1 As System.Windows.Forms.Button
    Public WithEvents _cmdDebug_0 As System.Windows.Forms.Button
    Public WithEvents txtDebug As System.Windows.Forms.TextBox
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents _SSTab1_TabPage2 As System.Windows.Forms.TabPage
    Public WithEvents SSTab1 As System.Windows.Forms.TabControl
    Public WithEvents Label3 As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
    Public WithEvents ProgressBar1 As Microsoft.VisualBasic.Compatibility.VB6.ProgressBarArray
    Public WithEvents StatusBar1 As Microsoft.VisualBasic.Compatibility.VB6.StatusStripArray
    Public WithEvents chkAdditionalExportOptions As Microsoft.VisualBasic.Compatibility.VB6.CheckBoxArray
    Public WithEvents cmdDebug As Microsoft.VisualBasic.Compatibility.VB6.ButtonArray
    Public WithEvents lblBackup As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
    Public WithEvents optAdditionalImportOptions As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
    Public WithEvents txtFileExtension As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
    Public WithEvents txtFileName As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
    Public WithEvents txtFilePath As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
    Public WithEvents txtWarning As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(mainForm))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me._StatusBar1_0 = New System.Windows.Forms.StatusStrip
        Me._StatusBar1_0_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me._StatusBar1_2 = New System.Windows.Forms.StatusStrip
        Me._StatusBar1_2_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me._StatusBar1_1 = New System.Windows.Forms.StatusStrip
        Me._StatusBar1_1_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me._ProgressBar1_0 = New System.Windows.Forms.ProgressBar
        Me._ProgressBar1_1 = New System.Windows.Forms.ProgressBar
        Me.lblDebugOn = New System.Windows.Forms.Label
        Me.cboGISLookupUserList = New uctGISUserDefLookupControl.cboGISLookup
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me.SSTab2 = New System.Windows.Forms.TabControl
        Me._SSTab2_TabPage0 = New System.Windows.Forms.TabPage
        Me.Frame3 = New System.Windows.Forms.GroupBox
        Me.cmdBrowse = New System.Windows.Forms.Button
        Me._txtFileExtension_1 = New System.Windows.Forms.TextBox
        Me._txtFileName_1 = New System.Windows.Forms.TextBox
        Me._txtFilePath_1 = New System.Windows.Forms.TextBox
        Me._Label3_2 = New System.Windows.Forms.Label
        Me._Label3_1 = New System.Windows.Forms.Label
        Me._Label3_0 = New System.Windows.Forms.Label
        Me.cmdRestoreUsers = New System.Windows.Forms.Button
        Me._SSTab2_TabPage1 = New System.Windows.Forms.TabPage
        Me.Frame10 = New System.Windows.Forms.GroupBox
        Me.tvDataModel = New System.Windows.Forms.TreeView
        Me.Frame9 = New System.Windows.Forms.GroupBox
        Me.tvUDL = New System.Windows.Forms.TreeView
        Me.Frame5 = New System.Windows.Forms.GroupBox
        Me._chkAdditionalExportOptions_10 = New System.Windows.Forms.CheckBox
        Me.chkGenerateUMLScript = New System.Windows.Forms.CheckBox
        Me.chkIncludeUMLs = New System.Windows.Forms.CheckBox
        Me._chkAdditionalExportOptions_9 = New System.Windows.Forms.CheckBox
        Me._chkAdditionalExportOptions_8 = New System.Windows.Forms.CheckBox
        Me._chkAdditionalExportOptions_7 = New System.Windows.Forms.CheckBox
        Me._chkAdditionalExportOptions_6 = New System.Windows.Forms.CheckBox
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
        Me.chkTarget = New System.Windows.Forms.CheckBox
        Me.cmdImportBrowse = New System.Windows.Forms.Button
        Me._txtFileExtension_0 = New System.Windows.Forms.TextBox
        Me._txtFileName_0 = New System.Windows.Forms.TextBox
        Me._txtFilePath_0 = New System.Windows.Forms.TextBox
        Me.lblCheckTarget = New System.Windows.Forms.Label
        Me._Label3_5 = New System.Windows.Forms.Label
        Me._Label3_4 = New System.Windows.Forms.Label
        Me._Label3_3 = New System.Windows.Forms.Label
        Me.Frame8 = New System.Windows.Forms.GroupBox
        Me.chkBackup = New System.Windows.Forms.CheckBox
        Me.cmdBackupBrowse = New System.Windows.Forms.Button
        Me.Dir1 = New Microsoft.VisualBasic.Compatibility.VB6.DirListBox
        Me.Drive1 = New Microsoft.VisualBasic.Compatibility.VB6.DriveListBox
        Me.txtBackupDir = New System.Windows.Forms.TextBox
        Me.lblBackupInfo = New System.Windows.Forms.Label
        Me._lblBackup_6 = New System.Windows.Forms.Label
        Me._SSTab3_TabPage1 = New System.Windows.Forms.TabPage
        Me.txtManualChanges = New System.Windows.Forms.TextBox
        Me._SSTab3_TabPage2 = New System.Windows.Forms.TabPage
        Me.Frame7 = New System.Windows.Forms.GroupBox
        Me._optAdditionalImportOptions_0 = New System.Windows.Forms.RadioButton
        Me._optAdditionalImportOptions_2 = New System.Windows.Forms.RadioButton
        Me._optAdditionalImportOptions_1 = New System.Windows.Forms.RadioButton
        Me.fraImportHeader = New System.Windows.Forms.GroupBox
        Me.txtImportConfirmation = New System.Windows.Forms.TextBox
        Me._SSTab3_TabPage3 = New System.Windows.Forms.TabPage
        Me.cmdImport = New System.Windows.Forms.Button
        Me._txtWarning_0 = New System.Windows.Forms.TextBox
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
        Me.CommonDialog1Save = New System.Windows.Forms.SaveFileDialog
        Me.CommonDialog1Font = New System.Windows.Forms.FontDialog
        Me.CommonDialog1Color = New System.Windows.Forms.ColorDialog
        Me.CommonDialog1Print = New System.Windows.Forms.PrintDialog
        Me.CommonDialog2Save = New System.Windows.Forms.SaveFileDialog
        Me.Label3 = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.ProgressBar1 = New Microsoft.VisualBasic.Compatibility.VB6.ProgressBarArray(Me.components)
        Me.StatusBar1 = New Microsoft.VisualBasic.Compatibility.VB6.StatusStripArray(Me.components)
        Me.chkAdditionalExportOptions = New Microsoft.VisualBasic.Compatibility.VB6.CheckBoxArray(Me.components)
        Me.cmdDebug = New Microsoft.VisualBasic.Compatibility.VB6.ButtonArray(Me.components)
        Me.lblBackup = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.optAdditionalImportOptions = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.txtFileExtension = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.txtFileName = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.txtFilePath = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.txtWarning = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.chkImportUMLScriptsOnly = New System.Windows.Forms.CheckBox
        Me.Frame1.SuspendLayout()
        Me._StatusBar1_0.SuspendLayout()
        Me._StatusBar1_2.SuspendLayout()
        Me._StatusBar1_1.SuspendLayout()
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me.SSTab2.SuspendLayout()
        Me._SSTab2_TabPage0.SuspendLayout()
        Me.Frame3.SuspendLayout()
        Me._SSTab2_TabPage1.SuspendLayout()
        Me.Frame10.SuspendLayout()
        Me.Frame9.SuspendLayout()
        Me.Frame5.SuspendLayout()
        Me._SSTab2_TabPage2.SuspendLayout()
        Me.Frame6.SuspendLayout()
        Me.fraExportHeader.SuspendLayout()
        Me._SSTab2_TabPage3.SuspendLayout()
        Me._SSTab1_TabPage1.SuspendLayout()
        Me.SSTab3.SuspendLayout()
        Me._SSTab3_TabPage0.SuspendLayout()
        Me.Frame2.SuspendLayout()
        Me.Frame8.SuspendLayout()
        Me._SSTab3_TabPage1.SuspendLayout()
        Me._SSTab3_TabPage2.SuspendLayout()
        Me.Frame7.SuspendLayout()
        Me.fraImportHeader.SuspendLayout()
        Me._SSTab3_TabPage3.SuspendLayout()
        Me._SSTab1_TabPage2.SuspendLayout()
        CType(Me.Label3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ProgressBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.StatusBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkAdditionalExportOptions, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmdDebug, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblBackup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optAdditionalImportOptions, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtFileExtension, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtFileName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtFilePath, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtWarning, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me._StatusBar1_0)
        Me.Frame1.Controls.Add(Me._StatusBar1_2)
        Me.Frame1.Controls.Add(Me._StatusBar1_1)
        Me.Frame1.Controls.Add(Me._ProgressBar1_0)
        Me.Frame1.Controls.Add(Me._ProgressBar1_1)
        Me.Frame1.Controls.Add(Me.lblDebugOn)
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(5, 368)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.Padding = New System.Windows.Forms.Padding(0)
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(742, 95)
        Me.Frame1.TabIndex = 22
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Progress Indicators"
        '
        '_StatusBar1_0
        '
        Me._StatusBar1_0.Dock = System.Windows.Forms.DockStyle.None
        Me._StatusBar1_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._StatusBar1_0.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._StatusBar1_0_Panel1})
        Me._StatusBar1_0.Location = New System.Drawing.Point(13, 15)
        Me._StatusBar1_0.Name = "_StatusBar1_0"
        Me._StatusBar1_0.ShowItemToolTips = True
        Me._StatusBar1_0.Size = New System.Drawing.Size(317, 22)
        Me._StatusBar1_0.TabIndex = 42
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
        '_StatusBar1_2
        '
        Me._StatusBar1_2.Dock = System.Windows.Forms.DockStyle.None
        Me._StatusBar1_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._StatusBar1_2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._StatusBar1_2_Panel1})
        Me._StatusBar1_2.Location = New System.Drawing.Point(13, 62)
        Me._StatusBar1_2.Name = "_StatusBar1_2"
        Me._StatusBar1_2.ShowItemToolTips = True
        Me._StatusBar1_2.Size = New System.Drawing.Size(317, 22)
        Me._StatusBar1_2.TabIndex = 41
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
        Me._StatusBar1_1.Location = New System.Drawing.Point(13, 37)
        Me._StatusBar1_1.Name = "_StatusBar1_1"
        Me._StatusBar1_1.ShowItemToolTips = True
        Me._StatusBar1_1.Size = New System.Drawing.Size(317, 22)
        Me._StatusBar1_1.TabIndex = 40
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
        '_ProgressBar1_0
        '
        Me.ProgressBar1.SetIndex(Me._ProgressBar1_0, CType(0, Short))
        Me._ProgressBar1_0.Location = New System.Drawing.Point(368, 16)
        Me._ProgressBar1_0.Name = "_ProgressBar1_0"
        Me._ProgressBar1_0.Size = New System.Drawing.Size(155, 16)
        Me._ProgressBar1_0.TabIndex = 23
        '
        '_ProgressBar1_1
        '
        Me.ProgressBar1.SetIndex(Me._ProgressBar1_1, CType(1, Short))
        Me._ProgressBar1_1.Location = New System.Drawing.Point(367, 68)
        Me._ProgressBar1_1.Name = "_ProgressBar1_1"
        Me._ProgressBar1_1.Size = New System.Drawing.Size(155, 16)
        Me._ProgressBar1_1.TabIndex = 27
        Me._ProgressBar1_1.Visible = False
        '
        'lblDebugOn
        '
        Me.lblDebugOn.BackColor = System.Drawing.SystemColors.Control
        Me.lblDebugOn.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDebugOn.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDebugOn.Location = New System.Drawing.Point(711, 10)
        Me.lblDebugOn.Name = "lblDebugOn"
        Me.lblDebugOn.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDebugOn.Size = New System.Drawing.Size(10, 10)
        Me.lblDebugOn.TabIndex = 38
        '
        'cboGISLookupUserList
        '
        Me.cboGISLookupUserList.DefaultItemId = 0
        Me.cboGISLookupUserList.FirstItem = ""
        Me.cboGISLookupUserList.GISDataModelCode = "None"
        Me.cboGISLookupUserList.ItemId = 0
        Me.cboGISLookupUserList.ListIndex = -1
        Me.cboGISLookupUserList.Location = New System.Drawing.Point(616, 472)
        Me.cboGISLookupUserList.Name = "cboGISLookupUserList"
        Me.cboGISLookupUserList.ParentDetailId = 0
        Me.cboGISLookupUserList.ParentHeaderId = 0
        Me.cboGISLookupUserList.SingleItemId = 0
        Me.cboGISLookupUserList.Size = New System.Drawing.Size(111, 21)
        Me.cboGISLookupUserList.TabIndex = 1
        Me.cboGISLookupUserList.Table = 0
        Me.cboGISLookupUserList.ToolTipText = ""
        Me.cboGISLookupUserList.Visible = False
        Me.cboGISLookupUserList.WhatsThisHelpID = 0
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage1)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage2)
        Me.SSTab1.ItemSize = New System.Drawing.Size(42, 18)
        Me.SSTab1.Location = New System.Drawing.Point(5, 5)
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(742, 357)
        Me.SSTab1.TabIndex = 0
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.SSTab2)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(734, 331)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "Export"
        '
        'SSTab2
        '
        Me.SSTab2.Controls.Add(Me._SSTab2_TabPage0)
        Me.SSTab2.Controls.Add(Me._SSTab2_TabPage1)
        Me.SSTab2.Controls.Add(Me._SSTab2_TabPage2)
        Me.SSTab2.Controls.Add(Me._SSTab2_TabPage3)
        Me.SSTab2.ItemSize = New System.Drawing.Size(42, 18)
        Me.SSTab2.Location = New System.Drawing.Point(9, 11)
        Me.SSTab2.Name = "SSTab2"
        Me.SSTab2.SelectedIndex = 0
        Me.SSTab2.Size = New System.Drawing.Size(722, 314)
        Me.SSTab2.TabIndex = 3
        '
        '_SSTab2_TabPage0
        '
        Me._SSTab2_TabPage0.Controls.Add(Me.Frame3)
        Me._SSTab2_TabPage0.Controls.Add(Me.cmdRestoreUsers)
        Me._SSTab2_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab2_TabPage0.Name = "_SSTab2_TabPage0"
        Me._SSTab2_TabPage0.Size = New System.Drawing.Size(714, 288)
        Me._SSTab2_TabPage0.TabIndex = 0
        Me._SSTab2_TabPage0.Text = "1) Select File"
        '
        'Frame3
        '
        Me.Frame3.BackColor = System.Drawing.SystemColors.Control
        Me.Frame3.Controls.Add(Me.cmdBrowse)
        Me.Frame3.Controls.Add(Me._txtFileExtension_1)
        Me.Frame3.Controls.Add(Me._txtFileName_1)
        Me.Frame3.Controls.Add(Me._txtFilePath_1)
        Me.Frame3.Controls.Add(Me._Label3_2)
        Me.Frame3.Controls.Add(Me._Label3_1)
        Me.Frame3.Controls.Add(Me._Label3_0)
        Me.Frame3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame3.Location = New System.Drawing.Point(22, 9)
        Me.Frame3.Name = "Frame3"
        Me.Frame3.Padding = New System.Windows.Forms.Padding(0)
        Me.Frame3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame3.Size = New System.Drawing.Size(656, 114)
        Me.Frame3.TabIndex = 4
        Me.Frame3.TabStop = False
        Me.Frame3.Text = "PIE Export File"
        '
        'cmdBrowse
        '
        Me.cmdBrowse.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBrowse.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBrowse.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBrowse.Location = New System.Drawing.Point(344, 24)
        Me.cmdBrowse.Name = "cmdBrowse"
        Me.cmdBrowse.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBrowse.Size = New System.Drawing.Size(25, 17)
        Me.cmdBrowse.TabIndex = 65
        Me.cmdBrowse.Text = "..."
        Me.cmdBrowse.UseVisualStyleBackColor = False
        '
        '_txtFileExtension_1
        '
        Me._txtFileExtension_1.AcceptsReturn = True
        Me._txtFileExtension_1.BackColor = System.Drawing.SystemColors.Window
        Me._txtFileExtension_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFileExtension_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFileExtension.SetIndex(Me._txtFileExtension_1, CType(1, Short))
        Me._txtFileExtension_1.Location = New System.Drawing.Point(77, 74)
        Me._txtFileExtension_1.MaxLength = 0
        Me._txtFileExtension_1.Name = "_txtFileExtension_1"
        Me._txtFileExtension_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFileExtension_1.Size = New System.Drawing.Size(33, 20)
        Me._txtFileExtension_1.TabIndex = 7
        '
        '_txtFileName_1
        '
        Me._txtFileName_1.AcceptsReturn = True
        Me._txtFileName_1.BackColor = System.Drawing.SystemColors.Window
        Me._txtFileName_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFileName_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFileName.SetIndex(Me._txtFileName_1, CType(1, Short))
        Me._txtFileName_1.Location = New System.Drawing.Point(77, 50)
        Me._txtFileName_1.MaxLength = 0
        Me._txtFileName_1.Name = "_txtFileName_1"
        Me._txtFileName_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFileName_1.Size = New System.Drawing.Size(263, 20)
        Me._txtFileName_1.TabIndex = 6
        '
        '_txtFilePath_1
        '
        Me._txtFilePath_1.AcceptsReturn = True
        Me._txtFilePath_1.BackColor = System.Drawing.SystemColors.Window
        Me._txtFilePath_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFilePath_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFilePath.SetIndex(Me._txtFilePath_1, CType(1, Short))
        Me._txtFilePath_1.Location = New System.Drawing.Point(77, 23)
        Me._txtFilePath_1.MaxLength = 0
        Me._txtFilePath_1.Name = "_txtFilePath_1"
        Me._txtFilePath_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFilePath_1.Size = New System.Drawing.Size(264, 20)
        Me._txtFilePath_1.TabIndex = 5
        '
        '_Label3_2
        '
        Me._Label3_2.BackColor = System.Drawing.SystemColors.Control
        Me._Label3_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label3_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.SetIndex(Me._Label3_2, CType(2, Short))
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
        Me._Label3_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.SetIndex(Me._Label3_1, CType(1, Short))
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
        Me._Label3_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.SetIndex(Me._Label3_0, CType(0, Short))
        Me._Label3_0.Location = New System.Drawing.Point(19, 26)
        Me._Label3_0.Name = "_Label3_0"
        Me._Label3_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label3_0.Size = New System.Drawing.Size(71, 15)
        Me._Label3_0.TabIndex = 8
        Me._Label3_0.Text = "Path"
        '
        'cmdRestoreUsers
        '
        Me.cmdRestoreUsers.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRestoreUsers.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRestoreUsers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRestoreUsers.Location = New System.Drawing.Point(538, 129)
        Me.cmdRestoreUsers.Name = "cmdRestoreUsers"
        Me.cmdRestoreUsers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRestoreUsers.Size = New System.Drawing.Size(140, 25)
        Me.cmdRestoreUsers.TabIndex = 64
        Me.cmdRestoreUsers.Text = "Restore User Logins"
        Me.cmdRestoreUsers.UseVisualStyleBackColor = False
        '
        '_SSTab2_TabPage1
        '
        Me._SSTab2_TabPage1.Controls.Add(Me.Frame10)
        Me._SSTab2_TabPage1.Controls.Add(Me.Frame9)
        Me._SSTab2_TabPage1.Controls.Add(Me.Frame5)
        Me._SSTab2_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._SSTab2_TabPage1.Name = "_SSTab2_TabPage1"
        Me._SSTab2_TabPage1.Size = New System.Drawing.Size(714, 288)
        Me._SSTab2_TabPage1.TabIndex = 1
        Me._SSTab2_TabPage1.Text = "2) Select Mode"
        '
        'Frame10
        '
        Me.Frame10.BackColor = System.Drawing.SystemColors.Control
        Me.Frame10.Controls.Add(Me.tvDataModel)
        Me.Frame10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame10.Location = New System.Drawing.Point(8, 14)
        Me.Frame10.Name = "Frame10"
        Me.Frame10.Padding = New System.Windows.Forms.Padding(0)
        Me.Frame10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame10.Size = New System.Drawing.Size(229, 265)
        Me.Frame10.TabIndex = 70
        Me.Frame10.TabStop = False
        Me.Frame10.Text = "Data Models to Export"
        '
        'tvDataModel
        '
        Me.tvDataModel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tvDataModel.CheckBoxes = True
        Me.tvDataModel.Indent = 15
        Me.tvDataModel.LabelEdit = True
        Me.tvDataModel.Location = New System.Drawing.Point(8, 16)
        Me.tvDataModel.Name = "tvDataModel"
        Me.tvDataModel.Size = New System.Drawing.Size(214, 241)
        Me.tvDataModel.TabIndex = 71
        '
        'Frame9
        '
        Me.Frame9.BackColor = System.Drawing.SystemColors.Control
        Me.Frame9.Controls.Add(Me.tvUDL)
        Me.Frame9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame9.Location = New System.Drawing.Point(243, 14)
        Me.Frame9.Name = "Frame9"
        Me.Frame9.Padding = New System.Windows.Forms.Padding(0)
        Me.Frame9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame9.Size = New System.Drawing.Size(229, 265)
        Me.Frame9.TabIndex = 68
        Me.Frame9.TabStop = False
        Me.Frame9.Text = "UDLs to Export"
        '
        'tvUDL
        '
        Me.tvUDL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tvUDL.CheckBoxes = True
        Me.tvUDL.Indent = 15
        Me.tvUDL.LabelEdit = True
        Me.tvUDL.Location = New System.Drawing.Point(8, 16)
        Me.tvUDL.Name = "tvUDL"
        Me.tvUDL.Size = New System.Drawing.Size(214, 241)
        Me.tvUDL.TabIndex = 69
        '
        'Frame5
        '
        Me.Frame5.BackColor = System.Drawing.SystemColors.Control
        Me.Frame5.Controls.Add(Me._chkAdditionalExportOptions_10)
        Me.Frame5.Controls.Add(Me.chkGenerateUMLScript)
        Me.Frame5.Controls.Add(Me.chkIncludeUMLs)
        Me.Frame5.Controls.Add(Me._chkAdditionalExportOptions_9)
        Me.Frame5.Controls.Add(Me._chkAdditionalExportOptions_8)
        Me.Frame5.Controls.Add(Me._chkAdditionalExportOptions_7)
        Me.Frame5.Controls.Add(Me._chkAdditionalExportOptions_6)
        Me.Frame5.Controls.Add(Me._chkAdditionalExportOptions_2)
        Me.Frame5.Controls.Add(Me._chkAdditionalExportOptions_1)
        Me.Frame5.Controls.Add(Me._chkAdditionalExportOptions_0)
        Me.Frame5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame5.Location = New System.Drawing.Point(478, 14)
        Me.Frame5.Name = "Frame5"
        Me.Frame5.Padding = New System.Windows.Forms.Padding(0)
        Me.Frame5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame5.Size = New System.Drawing.Size(226, 265)
        Me.Frame5.TabIndex = 15
        Me.Frame5.TabStop = False
        Me.Frame5.Text = "Additional Options"
        '
        'chkGenerateUMLScript
        '
        Me.chkGenerateUMLScript.BackColor = System.Drawing.SystemColors.Control
        Me.chkGenerateUMLScript.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkGenerateUMLScript.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkGenerateUMLScript.Location = New System.Drawing.Point(12, 209)
        Me.chkGenerateUMLScript.Name = "chkGenerateUMLScript"
        Me.chkGenerateUMLScript.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkGenerateUMLScript.Size = New System.Drawing.Size(201, 21)
        Me.chkGenerateUMLScript.TabIndex = 61
        Me.chkGenerateUMLScript.Text = "Generate Seperate UML Script"
        Me.chkGenerateUMLScript.UseVisualStyleBackColor = False
        '
        'chkIncludeUMLs
        '
        Me.chkIncludeUMLs.BackColor = System.Drawing.SystemColors.Control
        Me.chkIncludeUMLs.Checked = True
        Me.chkIncludeUMLs.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkIncludeUMLs.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIncludeUMLs.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIncludeUMLs.Location = New System.Drawing.Point(12, 182)
        Me.chkIncludeUMLs.Name = "chkIncludeUMLs"
        Me.chkIncludeUMLs.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIncludeUMLs.Size = New System.Drawing.Size(142, 21)
        Me.chkIncludeUMLs.TabIndex = 60
        Me.chkIncludeUMLs.Text = "Include UMLs"
        Me.chkIncludeUMLs.UseVisualStyleBackColor = False
        '
        '_chkAdditionalExportOptions_10
        '
        Me._chkAdditionalExportOptions_10.BackColor = System.Drawing.SystemColors.Control
        Me._chkAdditionalExportOptions_10.Checked = True
        Me._chkAdditionalExportOptions_10.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkAdditionalExportOptions_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkAdditionalExportOptions_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAdditionalExportOptions.SetIndex(Me._chkAdditionalExportOptions_10, CType(10, Short))
        Me._chkAdditionalExportOptions_10.Location = New System.Drawing.Point(12, 176)
        Me._chkAdditionalExportOptions_10.Name = "_chkAdditionalExportOptions_10"
        Me._chkAdditionalExportOptions_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkAdditionalExportOptions_10.Size = New System.Drawing.Size(211, 21)
        Me._chkAdditionalExportOptions_10.TabIndex = 60
        Me._chkAdditionalExportOptions_10.Text = "Include spu_Report_%"
        Me._chkAdditionalExportOptions_10.UseVisualStyleBackColor = False
        '
        '_chkAdditionalExportOptions_9
        '
        Me._chkAdditionalExportOptions_9.BackColor = System.Drawing.SystemColors.Control
        Me._chkAdditionalExportOptions_9.Checked = True
        Me._chkAdditionalExportOptions_9.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkAdditionalExportOptions_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkAdditionalExportOptions_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAdditionalExportOptions.SetIndex(Me._chkAdditionalExportOptions_9, CType(9, Short))
        Me._chkAdditionalExportOptions_9.Location = New System.Drawing.Point(12, 155)
        Me._chkAdditionalExportOptions_9.Name = "_chkAdditionalExportOptions_9"
        Me._chkAdditionalExportOptions_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkAdditionalExportOptions_9.Size = New System.Drawing.Size(139, 21)
        Me._chkAdditionalExportOptions_9.TabIndex = 59
        Me._chkAdditionalExportOptions_9.Text = "Include all Reports"
        Me._chkAdditionalExportOptions_9.UseVisualStyleBackColor = False
        '
        '_chkAdditionalExportOptions_8
        '
        Me._chkAdditionalExportOptions_8.BackColor = System.Drawing.SystemColors.Control
        Me._chkAdditionalExportOptions_8.Checked = True
        Me._chkAdditionalExportOptions_8.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkAdditionalExportOptions_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkAdditionalExportOptions_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAdditionalExportOptions.SetIndex(Me._chkAdditionalExportOptions_8, CType(8, Short))
        Me._chkAdditionalExportOptions_8.Location = New System.Drawing.Point(12, 132)
        Me._chkAdditionalExportOptions_8.Name = "_chkAdditionalExportOptions_8"
        Me._chkAdditionalExportOptions_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkAdditionalExportOptions_8.Size = New System.Drawing.Size(142, 21)
        Me._chkAdditionalExportOptions_8.TabIndex = 58
        Me._chkAdditionalExportOptions_8.Text = "Include spu_ICCS_%"
        Me._chkAdditionalExportOptions_8.UseVisualStyleBackColor = False
        '
        '_chkAdditionalExportOptions_7
        '
        Me._chkAdditionalExportOptions_7.BackColor = System.Drawing.SystemColors.Control
        Me._chkAdditionalExportOptions_7.Checked = True
        Me._chkAdditionalExportOptions_7.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkAdditionalExportOptions_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkAdditionalExportOptions_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAdditionalExportOptions.SetIndex(Me._chkAdditionalExportOptions_7, CType(7, Short))
        Me._chkAdditionalExportOptions_7.Location = New System.Drawing.Point(12, 110)
        Me._chkAdditionalExportOptions_7.Name = "_chkAdditionalExportOptions_7"
        Me._chkAdditionalExportOptions_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkAdditionalExportOptions_7.Size = New System.Drawing.Size(182, 21)
        Me._chkAdditionalExportOptions_7.TabIndex = 57
        Me._chkAdditionalExportOptions_7.Text = "Include User Authority Rules"
        Me._chkAdditionalExportOptions_7.UseVisualStyleBackColor = False
        '
        '_chkAdditionalExportOptions_6
        '
        Me._chkAdditionalExportOptions_6.BackColor = System.Drawing.SystemColors.Control
        Me._chkAdditionalExportOptions_6.Checked = True
        Me._chkAdditionalExportOptions_6.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkAdditionalExportOptions_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkAdditionalExportOptions_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAdditionalExportOptions.SetIndex(Me._chkAdditionalExportOptions_6, CType(6, Short))
        Me._chkAdditionalExportOptions_6.Location = New System.Drawing.Point(12, 87)
        Me._chkAdditionalExportOptions_6.Name = "_chkAdditionalExportOptions_6"
        Me._chkAdditionalExportOptions_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkAdditionalExportOptions_6.Size = New System.Drawing.Size(201, 21)
        Me._chkAdditionalExportOptions_6.TabIndex = 45
        Me._chkAdditionalExportOptions_6.Text = "Include User Defined Lists (UDLs)"
        Me._chkAdditionalExportOptions_6.UseVisualStyleBackColor = False
        '
        '_chkAdditionalExportOptions_2
        '
        Me._chkAdditionalExportOptions_2.BackColor = System.Drawing.SystemColors.Control
        Me._chkAdditionalExportOptions_2.Checked = True
        Me._chkAdditionalExportOptions_2.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkAdditionalExportOptions_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkAdditionalExportOptions_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAdditionalExportOptions.SetIndex(Me._chkAdditionalExportOptions_2, CType(2, Short))
        Me._chkAdditionalExportOptions_2.Location = New System.Drawing.Point(12, 64)
        Me._chkAdditionalExportOptions_2.Name = "_chkAdditionalExportOptions_2"
        Me._chkAdditionalExportOptions_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkAdditionalExportOptions_2.Size = New System.Drawing.Size(211, 21)
        Me._chkAdditionalExportOptions_2.TabIndex = 18
        Me._chkAdditionalExportOptions_2.Text = "Include rule files"
        Me._chkAdditionalExportOptions_2.UseVisualStyleBackColor = False
        '
        '_chkAdditionalExportOptions_1
        '
        Me._chkAdditionalExportOptions_1.BackColor = System.Drawing.SystemColors.Control
        Me._chkAdditionalExportOptions_1.Checked = True
        Me._chkAdditionalExportOptions_1.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkAdditionalExportOptions_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkAdditionalExportOptions_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAdditionalExportOptions.SetIndex(Me._chkAdditionalExportOptions_1, CType(1, Short))
        Me._chkAdditionalExportOptions_1.Location = New System.Drawing.Point(12, 41)
        Me._chkAdditionalExportOptions_1.Name = "_chkAdditionalExportOptions_1"
        Me._chkAdditionalExportOptions_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkAdditionalExportOptions_1.Size = New System.Drawing.Size(186, 21)
        Me._chkAdditionalExportOptions_1.TabIndex = 17
        Me._chkAdditionalExportOptions_1.Text = "Include document templates"
        Me._chkAdditionalExportOptions_1.UseVisualStyleBackColor = False
        '
        '_chkAdditionalExportOptions_0
        '
        Me._chkAdditionalExportOptions_0.BackColor = System.Drawing.SystemColors.Control
        Me._chkAdditionalExportOptions_0.Checked = True
        Me._chkAdditionalExportOptions_0.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkAdditionalExportOptions_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkAdditionalExportOptions_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAdditionalExportOptions.SetIndex(Me._chkAdditionalExportOptions_0, CType(0, Short))
        Me._chkAdditionalExportOptions_0.Location = New System.Drawing.Point(12, 19)
        Me._chkAdditionalExportOptions_0.Name = "_chkAdditionalExportOptions_0"
        Me._chkAdditionalExportOptions_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkAdditionalExportOptions_0.Size = New System.Drawing.Size(187, 21)
        Me._chkAdditionalExportOptions_0.TabIndex = 16
        Me._chkAdditionalExportOptions_0.Text = "Include registry settings"
        Me._chkAdditionalExportOptions_0.UseVisualStyleBackColor = False
        '
        '_SSTab2_TabPage2
        '
        Me._SSTab2_TabPage2.Controls.Add(Me.Frame6)
        Me._SSTab2_TabPage2.Controls.Add(Me.fraExportHeader)
        Me._SSTab2_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._SSTab2_TabPage2.Name = "_SSTab2_TabPage2"
        Me._SSTab2_TabPage2.Size = New System.Drawing.Size(714, 288)
        Me._SSTab2_TabPage2.TabIndex = 2
        Me._SSTab2_TabPage2.Text = "3) Confirm Details"
        '
        'Frame6
        '
        Me.Frame6.BackColor = System.Drawing.SystemColors.Control
        Me.Frame6.Controls.Add(Me.txtExportHeaderComment)
        Me.Frame6.Controls.Add(Me.lblComment)
        Me.Frame6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame6.Location = New System.Drawing.Point(16, 158)
        Me.Frame6.Name = "Frame6"
        Me.Frame6.Padding = New System.Windows.Forms.Padding(0)
        Me.Frame6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame6.Size = New System.Drawing.Size(677, 120)
        Me.Frame6.TabIndex = 19
        Me.Frame6.TabStop = False
        Me.Frame6.Text = "Please enter any additional information to be displayed during the import process" & _
            ""
        '
        'txtExportHeaderComment
        '
        Me.txtExportHeaderComment.AcceptsReturn = True
        Me.txtExportHeaderComment.BackColor = System.Drawing.SystemColors.Window
        Me.txtExportHeaderComment.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtExportHeaderComment.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtExportHeaderComment.Location = New System.Drawing.Point(10, 34)
        Me.txtExportHeaderComment.MaxLength = 0
        Me.txtExportHeaderComment.Multiline = True
        Me.txtExportHeaderComment.Name = "txtExportHeaderComment"
        Me.txtExportHeaderComment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtExportHeaderComment.Size = New System.Drawing.Size(658, 78)
        Me.txtExportHeaderComment.TabIndex = 20
        '
        'lblComment
        '
        Me.lblComment.BackColor = System.Drawing.SystemColors.Control
        Me.lblComment.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblComment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblComment.Location = New System.Drawing.Point(10, 18)
        Me.lblComment.Name = "lblComment"
        Me.lblComment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblComment.Size = New System.Drawing.Size(129, 17)
        Me.lblComment.TabIndex = 21
        Me.lblComment.Text = "Comment"
        '
        'fraExportHeader
        '
        Me.fraExportHeader.BackColor = System.Drawing.SystemColors.Control
        Me.fraExportHeader.Controls.Add(Me.txtExportConfirmationText)
        Me.fraExportHeader.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraExportHeader.Location = New System.Drawing.Point(16, 8)
        Me.fraExportHeader.Name = "fraExportHeader"
        Me.fraExportHeader.Padding = New System.Windows.Forms.Padding(0)
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
        Me.txtExportConfirmationText.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtExportConfirmationText.Location = New System.Drawing.Point(8, 16)
        Me.txtExportConfirmationText.MaxLength = 0
        Me.txtExportConfirmationText.Multiline = True
        Me.txtExportConfirmationText.Name = "txtExportConfirmationText"
        Me.txtExportConfirmationText.ReadOnly = True
        Me.txtExportConfirmationText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtExportConfirmationText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtExportConfirmationText.Size = New System.Drawing.Size(661, 118)
        Me.txtExportConfirmationText.TabIndex = 37
        '
        '_SSTab2_TabPage3
        '
        Me._SSTab2_TabPage3.Controls.Add(Me._txtWarning_1)
        Me._SSTab2_TabPage3.Controls.Add(Me.cmdExportCancel)
        Me._SSTab2_TabPage3.Controls.Add(Me.cmdExport)
        Me._SSTab2_TabPage3.Controls.Add(Me.lblExportInfo)
        Me._SSTab2_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._SSTab2_TabPage3.Name = "_SSTab2_TabPage3"
        Me._SSTab2_TabPage3.Size = New System.Drawing.Size(714, 288)
        Me._SSTab2_TabPage3.TabIndex = 3
        Me._SSTab2_TabPage3.Text = "4) Run Export"
        '
        '_txtWarning_1
        '
        Me._txtWarning_1.AcceptsReturn = True
        Me._txtWarning_1.BackColor = System.Drawing.SystemColors.Window
        Me._txtWarning_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtWarning_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtWarning.SetIndex(Me._txtWarning_1, CType(1, Short))
        Me._txtWarning_1.Location = New System.Drawing.Point(10, 29)
        Me._txtWarning_1.MaxLength = 0
        Me._txtWarning_1.Multiline = True
        Me._txtWarning_1.Name = "_txtWarning_1"
        Me._txtWarning_1.ReadOnly = True
        Me._txtWarning_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtWarning_1.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me._txtWarning_1.Size = New System.Drawing.Size(687, 209)
        Me._txtWarning_1.TabIndex = 33
        Me._txtWarning_1.WordWrap = False
        '
        'cmdExportCancel
        '
        Me.cmdExportCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExportCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExportCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExportCancel.Location = New System.Drawing.Point(536, 250)
        Me.cmdExportCancel.Name = "cmdExportCancel"
        Me.cmdExportCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExportCancel.Size = New System.Drawing.Size(79, 27)
        Me.cmdExportCancel.TabIndex = 32
        Me.cmdExportCancel.Text = "&Cancel"
        Me.cmdExportCancel.UseVisualStyleBackColor = False
        Me.cmdExportCancel.Visible = False
        '
        'cmdExport
        '
        Me.cmdExport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExport.Location = New System.Drawing.Point(619, 250)
        Me.cmdExport.Name = "cmdExport"
        Me.cmdExport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExport.Size = New System.Drawing.Size(79, 27)
        Me.cmdExport.TabIndex = 11
        Me.cmdExport.Text = "&Export"
        Me.cmdExport.UseVisualStyleBackColor = False
        '
        'lblExportInfo
        '
        Me.lblExportInfo.BackColor = System.Drawing.SystemColors.Control
        Me.lblExportInfo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExportInfo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExportInfo.Location = New System.Drawing.Point(10, 14)
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
        Me._SSTab1_TabPage1.Size = New System.Drawing.Size(734, 331)
        Me._SSTab1_TabPage1.TabIndex = 1
        Me._SSTab1_TabPage1.Text = "Import"
        '
        'SSTab3
        '
        Me.SSTab3.Controls.Add(Me._SSTab3_TabPage0)
        Me.SSTab3.Controls.Add(Me._SSTab3_TabPage1)
        Me.SSTab3.Controls.Add(Me._SSTab3_TabPage2)
        Me.SSTab3.Controls.Add(Me._SSTab3_TabPage3)
        Me.SSTab3.ItemSize = New System.Drawing.Size(42, 18)
        Me.SSTab3.Location = New System.Drawing.Point(8, 11)
        Me.SSTab3.Name = "SSTab3"
        Me.SSTab3.SelectedIndex = 0
        Me.SSTab3.Size = New System.Drawing.Size(706, 314)
        Me.SSTab3.TabIndex = 14
        '
        '_SSTab3_TabPage0
        '
        Me._SSTab3_TabPage0.Controls.Add(Me.Frame2)
        Me._SSTab3_TabPage0.Controls.Add(Me.Frame8)
        Me._SSTab3_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab3_TabPage0.Name = "_SSTab3_TabPage0"
        Me._SSTab3_TabPage0.Size = New System.Drawing.Size(698, 288)
        Me._SSTab3_TabPage0.TabIndex = 0
        Me._SSTab3_TabPage0.Text = "1) Select File"
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.chkImportUMLScriptsOnly)
        Me.Frame2.Controls.Add(Me.chkTarget)
        Me.Frame2.Controls.Add(Me.cmdImportBrowse)
        Me.Frame2.Controls.Add(Me._txtFileExtension_0)
        Me.Frame2.Controls.Add(Me._txtFileName_0)
        Me.Frame2.Controls.Add(Me._txtFilePath_0)
        Me.Frame2.Controls.Add(Me.lblCheckTarget)
        Me.Frame2.Controls.Add(Me._Label3_5)
        Me.Frame2.Controls.Add(Me._Label3_4)
        Me.Frame2.Controls.Add(Me._Label3_3)
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(22, 12)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.Padding = New System.Windows.Forms.Padding(0)
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(656, 114)
        Me.Frame2.TabIndex = 28
        Me.Frame2.TabStop = False
        Me.Frame2.Text = "PIE Import File"
        '
        'chkTarget
        '
        Me.chkTarget.BackColor = System.Drawing.SystemColors.Control
        Me.chkTarget.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkTarget.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkTarget.ForeColor = System.Drawing.SystemColors.Highlight
        Me.chkTarget.Location = New System.Drawing.Point(200, 80)
        Me.chkTarget.Name = "chkTarget"
        Me.chkTarget.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkTarget.Size = New System.Drawing.Size(109, 17)
        Me.chkTarget.TabIndex = 74
        Me.chkTarget.Text = "Overwrite Target "
        Me.chkTarget.UseVisualStyleBackColor = False
        Me.chkTarget.Visible = False
        '
        'cmdImportBrowse
        '
        Me.cmdImportBrowse.BackColor = System.Drawing.SystemColors.Control
        Me.cmdImportBrowse.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdImportBrowse.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdImportBrowse.Location = New System.Drawing.Point(328, 24)
        Me.cmdImportBrowse.Name = "cmdImportBrowse"
        Me.cmdImportBrowse.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdImportBrowse.Size = New System.Drawing.Size(25, 17)
        Me.cmdImportBrowse.TabIndex = 72
        Me.cmdImportBrowse.Text = "..."
        Me.cmdImportBrowse.UseVisualStyleBackColor = False
        '
        '_txtFileExtension_0
        '
        Me._txtFileExtension_0.AcceptsReturn = True
        Me._txtFileExtension_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtFileExtension_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFileExtension_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFileExtension.SetIndex(Me._txtFileExtension_0, CType(0, Short))
        Me._txtFileExtension_0.Location = New System.Drawing.Point(77, 74)
        Me._txtFileExtension_0.MaxLength = 0
        Me._txtFileExtension_0.Name = "_txtFileExtension_0"
        Me._txtFileExtension_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFileExtension_0.Size = New System.Drawing.Size(33, 20)
        Me._txtFileExtension_0.TabIndex = 31
        '
        '_txtFileName_0
        '
        Me._txtFileName_0.AcceptsReturn = True
        Me._txtFileName_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtFileName_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFileName_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFileName.SetIndex(Me._txtFileName_0, CType(0, Short))
        Me._txtFileName_0.Location = New System.Drawing.Point(77, 50)
        Me._txtFileName_0.MaxLength = 0
        Me._txtFileName_0.Name = "_txtFileName_0"
        Me._txtFileName_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFileName_0.Size = New System.Drawing.Size(233, 20)
        Me._txtFileName_0.TabIndex = 30
        '
        '_txtFilePath_0
        '
        Me._txtFilePath_0.AcceptsReturn = True
        Me._txtFilePath_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtFilePath_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFilePath_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFilePath.SetIndex(Me._txtFilePath_0, CType(0, Short))
        Me._txtFilePath_0.Location = New System.Drawing.Point(77, 23)
        Me._txtFilePath_0.MaxLength = 0
        Me._txtFilePath_0.Name = "_txtFilePath_0"
        Me._txtFilePath_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFilePath_0.Size = New System.Drawing.Size(233, 20)
        Me._txtFilePath_0.TabIndex = 29
        '
        'lblCheckTarget
        '
        Me.lblCheckTarget.BackColor = System.Drawing.SystemColors.Control
        Me.lblCheckTarget.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCheckTarget.ForeColor = System.Drawing.SystemColors.Highlight
        Me.lblCheckTarget.Location = New System.Drawing.Point(320, 80)
        Me.lblCheckTarget.Name = "lblCheckTarget"
        Me.lblCheckTarget.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCheckTarget.Size = New System.Drawing.Size(321, 17)
        Me.lblCheckTarget.TabIndex = 75
        Me.lblCheckTarget.Text = "(Recommended for multiple Source to single target environments)"
        Me.lblCheckTarget.Visible = False
        '
        '_Label3_5
        '
        Me._Label3_5.BackColor = System.Drawing.SystemColors.Control
        Me._Label3_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label3_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.SetIndex(Me._Label3_5, CType(5, Short))
        Me._Label3_5.Location = New System.Drawing.Point(19, 26)
        Me._Label3_5.Name = "_Label3_5"
        Me._Label3_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label3_5.Size = New System.Drawing.Size(71, 15)
        Me._Label3_5.TabIndex = 34
        Me._Label3_5.Text = "Path"
        '
        '_Label3_4
        '
        Me._Label3_4.BackColor = System.Drawing.SystemColors.Control
        Me._Label3_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label3_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.SetIndex(Me._Label3_4, CType(4, Short))
        Me._Label3_4.Location = New System.Drawing.Point(19, 51)
        Me._Label3_4.Name = "_Label3_4"
        Me._Label3_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label3_4.Size = New System.Drawing.Size(71, 15)
        Me._Label3_4.TabIndex = 35
        Me._Label3_4.Text = "Name"
        '
        '_Label3_3
        '
        Me._Label3_3.BackColor = System.Drawing.SystemColors.Control
        Me._Label3_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label3_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.SetIndex(Me._Label3_3, CType(3, Short))
        Me._Label3_3.Location = New System.Drawing.Point(19, 76)
        Me._Label3_3.Name = "_Label3_3"
        Me._Label3_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label3_3.Size = New System.Drawing.Size(71, 15)
        Me._Label3_3.TabIndex = 36
        Me._Label3_3.Text = "Extension"
        '
        'Frame8
        '
        Me.Frame8.BackColor = System.Drawing.SystemColors.Control
        Me.Frame8.Controls.Add(Me.chkBackup)
        Me.Frame8.Controls.Add(Me.cmdBackupBrowse)
        Me.Frame8.Controls.Add(Me.Dir1)
        Me.Frame8.Controls.Add(Me.Drive1)
        Me.Frame8.Controls.Add(Me.txtBackupDir)
        Me.Frame8.Controls.Add(Me.lblBackupInfo)
        Me.Frame8.Controls.Add(Me._lblBackup_6)
        Me.Frame8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame8.Location = New System.Drawing.Point(22, 135)
        Me.Frame8.Name = "Frame8"
        Me.Frame8.Padding = New System.Windows.Forms.Padding(0)
        Me.Frame8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame8.Size = New System.Drawing.Size(656, 121)
        Me.Frame8.TabIndex = 54
        Me.Frame8.TabStop = False
        Me.Frame8.Text = "PIE Backup Location"
        '
        'chkBackup
        '
        Me.chkBackup.BackColor = System.Drawing.SystemColors.Control
        Me.chkBackup.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkBackup.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkBackup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkBackup.Location = New System.Drawing.Point(8, 48)
        Me.chkBackup.Name = "chkBackup"
        Me.chkBackup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkBackup.Size = New System.Drawing.Size(117, 17)
        Me.chkBackup.TabIndex = 73
        Me.chkBackup.Text = "Skip Backup"
        Me.chkBackup.UseVisualStyleBackColor = False
        '
        'cmdBackupBrowse
        '
        Me.cmdBackupBrowse.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBackupBrowse.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBackupBrowse.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBackupBrowse.Location = New System.Drawing.Point(312, 24)
        Me.cmdBackupBrowse.Name = "cmdBackupBrowse"
        Me.cmdBackupBrowse.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBackupBrowse.Size = New System.Drawing.Size(25, 17)
        Me.cmdBackupBrowse.TabIndex = 66
        Me.cmdBackupBrowse.Text = "..."
        Me.cmdBackupBrowse.UseVisualStyleBackColor = False
        '
        'Dir1
        '
        Me.Dir1.BackColor = System.Drawing.SystemColors.Window
        Me.Dir1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Dir1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Dir1.FormattingEnabled = True
        Me.Dir1.IntegralHeight = False
        Me.Dir1.Location = New System.Drawing.Point(448, 24)
        Me.Dir1.Name = "Dir1"
        Me.Dir1.Size = New System.Drawing.Size(193, 66)
        Me.Dir1.TabIndex = 63
        Me.Dir1.Visible = False
        '
        'Drive1
        '
        Me.Drive1.BackColor = System.Drawing.SystemColors.Window
        Me.Drive1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Drive1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Drive1.FormattingEnabled = True
        Me.Drive1.Location = New System.Drawing.Point(240, 64)
        Me.Drive1.Name = "Drive1"
        Me.Drive1.Size = New System.Drawing.Size(113, 21)
        Me.Drive1.TabIndex = 62
        Me.Drive1.Visible = False
        '
        'txtBackupDir
        '
        Me.txtBackupDir.AcceptsReturn = True
        Me.txtBackupDir.BackColor = System.Drawing.SystemColors.Window
        Me.txtBackupDir.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBackupDir.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBackupDir.Location = New System.Drawing.Point(112, 23)
        Me.txtBackupDir.MaxLength = 0
        Me.txtBackupDir.Name = "txtBackupDir"
        Me.txtBackupDir.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBackupDir.Size = New System.Drawing.Size(185, 20)
        Me.txtBackupDir.TabIndex = 55
        '
        'lblBackupInfo
        '
        Me.lblBackupInfo.BackColor = System.Drawing.SystemColors.Control
        Me.lblBackupInfo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBackupInfo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBackupInfo.Location = New System.Drawing.Point(8, 96)
        Me.lblBackupInfo.Name = "lblBackupInfo"
        Me.lblBackupInfo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBackupInfo.Size = New System.Drawing.Size(449, 17)
        Me.lblBackupInfo.TabIndex = 61
        Me.lblBackupInfo.Text = "(Please note, the backup location must be visible across the network from the dat" & _
            "abase server)"
        '
        '_lblBackup_6
        '
        Me._lblBackup_6.BackColor = System.Drawing.SystemColors.Control
        Me._lblBackup_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblBackup_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBackup.SetIndex(Me._lblBackup_6, CType(6, Short))
        Me._lblBackup_6.Location = New System.Drawing.Point(8, 24)
        Me._lblBackup_6.Name = "_lblBackup_6"
        Me._lblBackup_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblBackup_6.Size = New System.Drawing.Size(97, 17)
        Me._lblBackup_6.TabIndex = 56
        Me._lblBackup_6.Text = "Backup directory"
        '
        '_SSTab3_TabPage1
        '
        Me._SSTab3_TabPage1.Controls.Add(Me.txtManualChanges)
        Me._SSTab3_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._SSTab3_TabPage1.Name = "_SSTab3_TabPage1"
        Me._SSTab3_TabPage1.Size = New System.Drawing.Size(698, 288)
        Me._SSTab3_TabPage1.TabIndex = 1
        Me._SSTab3_TabPage1.Text = "2) Report"
        '
        'txtManualChanges
        '
        Me.txtManualChanges.AcceptsReturn = True
        Me.txtManualChanges.BackColor = System.Drawing.SystemColors.Window
        Me.txtManualChanges.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtManualChanges.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtManualChanges.Location = New System.Drawing.Point(8, 10)
        Me.txtManualChanges.MaxLength = 0
        Me.txtManualChanges.Multiline = True
        Me.txtManualChanges.Name = "txtManualChanges"
        Me.txtManualChanges.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtManualChanges.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtManualChanges.Size = New System.Drawing.Size(681, 269)
        Me.txtManualChanges.TabIndex = 60
        '
        '_SSTab3_TabPage2
        '
        Me._SSTab3_TabPage2.Controls.Add(Me.Frame7)
        Me._SSTab3_TabPage2.Controls.Add(Me.fraImportHeader)
        Me._SSTab3_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._SSTab3_TabPage2.Name = "_SSTab3_TabPage2"
        Me._SSTab3_TabPage2.Size = New System.Drawing.Size(698, 288)
        Me._SSTab3_TabPage2.TabIndex = 2
        Me._SSTab3_TabPage2.Text = "3) Confirm Details"
        '
        'Frame7
        '
        Me.Frame7.BackColor = System.Drawing.SystemColors.Control
        Me.Frame7.Controls.Add(Me._optAdditionalImportOptions_0)
        Me.Frame7.Controls.Add(Me._optAdditionalImportOptions_2)
        Me.Frame7.Controls.Add(Me._optAdditionalImportOptions_1)
        Me.Frame7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame7.Location = New System.Drawing.Point(14, 215)
        Me.Frame7.Name = "Frame7"
        Me.Frame7.Padding = New System.Windows.Forms.Padding(0)
        Me.Frame7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame7.Size = New System.Drawing.Size(520, 53)
        Me.Frame7.TabIndex = 50
        Me.Frame7.TabStop = False
        Me.Frame7.Text = "Additional Options"
        '
        '_optAdditionalImportOptions_0
        '
        Me._optAdditionalImportOptions_0.AutoSize = True
        Me._optAdditionalImportOptions_0.BackColor = System.Drawing.SystemColors.Control
        Me._optAdditionalImportOptions_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optAdditionalImportOptions_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optAdditionalImportOptions.SetIndex(Me._optAdditionalImportOptions_0, CType(0, Short))
        Me._optAdditionalImportOptions_0.Location = New System.Drawing.Point(13, 18)
        Me._optAdditionalImportOptions_0.Name = "_optAdditionalImportOptions_0"
        Me._optAdditionalImportOptions_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optAdditionalImportOptions_0.Size = New System.Drawing.Size(129, 17)
        Me._optAdditionalImportOptions_0.TabIndex = 53
        Me._optAdditionalImportOptions_0.TabStop = True
        Me._optAdditionalImportOptions_0.Text = "Import registry settings"
        Me._optAdditionalImportOptions_0.UseVisualStyleBackColor = False
        '
        '_optAdditionalImportOptions_2
        '
        Me._optAdditionalImportOptions_2.AutoSize = True
        Me._optAdditionalImportOptions_2.BackColor = System.Drawing.SystemColors.Control
        Me._optAdditionalImportOptions_2.Checked = True
        Me._optAdditionalImportOptions_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optAdditionalImportOptions_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optAdditionalImportOptions.SetIndex(Me._optAdditionalImportOptions_2, CType(2, Short))
        Me._optAdditionalImportOptions_2.Location = New System.Drawing.Point(329, 19)
        Me._optAdditionalImportOptions_2.Name = "_optAdditionalImportOptions_2"
        Me._optAdditionalImportOptions_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optAdditionalImportOptions_2.Size = New System.Drawing.Size(119, 17)
        Me._optAdditionalImportOptions_2.TabIndex = 52
        Me._optAdditionalImportOptions_2.TabStop = True
        Me._optAdditionalImportOptions_2.Text = "No registry changes"
        Me._optAdditionalImportOptions_2.UseVisualStyleBackColor = False
        '
        '_optAdditionalImportOptions_1
        '
        Me._optAdditionalImportOptions_1.AutoSize = True
        Me._optAdditionalImportOptions_1.BackColor = System.Drawing.SystemColors.Control
        Me._optAdditionalImportOptions_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optAdditionalImportOptions_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optAdditionalImportOptions.SetIndex(Me._optAdditionalImportOptions_1, CType(1, Short))
        Me._optAdditionalImportOptions_1.Location = New System.Drawing.Point(150, 18)
        Me._optAdditionalImportOptions_1.Name = "_optAdditionalImportOptions_1"
        Me._optAdditionalImportOptions_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optAdditionalImportOptions_1.Size = New System.Drawing.Size(166, 17)
        Me._optAdditionalImportOptions_1.TabIndex = 51
        Me._optAdditionalImportOptions_1.TabStop = True
        Me._optAdditionalImportOptions_1.Text = "Create default registry settings"
        Me._optAdditionalImportOptions_1.UseVisualStyleBackColor = False
        '
        'fraImportHeader
        '
        Me.fraImportHeader.BackColor = System.Drawing.SystemColors.Control
        Me.fraImportHeader.Controls.Add(Me.txtImportConfirmation)
        Me.fraImportHeader.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraImportHeader.Location = New System.Drawing.Point(6, 10)
        Me.fraImportHeader.Name = "fraImportHeader"
        Me.fraImportHeader.Padding = New System.Windows.Forms.Padding(0)
        Me.fraImportHeader.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraImportHeader.Size = New System.Drawing.Size(683, 196)
        Me.fraImportHeader.TabIndex = 48
        Me.fraImportHeader.TabStop = False
        Me.fraImportHeader.Text = "Header Information"
        '
        'txtImportConfirmation
        '
        Me.txtImportConfirmation.AcceptsReturn = True
        Me.txtImportConfirmation.BackColor = System.Drawing.SystemColors.Window
        Me.txtImportConfirmation.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtImportConfirmation.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtImportConfirmation.Location = New System.Drawing.Point(14, 16)
        Me.txtImportConfirmation.MaxLength = 0
        Me.txtImportConfirmation.Multiline = True
        Me.txtImportConfirmation.Name = "txtImportConfirmation"
        Me.txtImportConfirmation.ReadOnly = True
        Me.txtImportConfirmation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtImportConfirmation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtImportConfirmation.Size = New System.Drawing.Size(665, 169)
        Me.txtImportConfirmation.TabIndex = 49
        '
        '_SSTab3_TabPage3
        '
        Me._SSTab3_TabPage3.Controls.Add(Me.cmdImport)
        Me._SSTab3_TabPage3.Controls.Add(Me._txtWarning_0)
        Me._SSTab3_TabPage3.Controls.Add(Me.Label4)
        Me._SSTab3_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._SSTab3_TabPage3.Name = "_SSTab3_TabPage3"
        Me._SSTab3_TabPage3.Size = New System.Drawing.Size(698, 288)
        Me._SSTab3_TabPage3.TabIndex = 3
        Me._SSTab3_TabPage3.Text = "4) Run Import"
        '
        'cmdImport
        '
        Me.cmdImport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdImport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdImport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdImport.Location = New System.Drawing.Point(608, 248)
        Me.cmdImport.Name = "cmdImport"
        Me.cmdImport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdImport.Size = New System.Drawing.Size(79, 29)
        Me.cmdImport.TabIndex = 67
        Me.cmdImport.Text = "Import"
        Me.cmdImport.UseVisualStyleBackColor = False
        '
        '_txtWarning_0
        '
        Me._txtWarning_0.AcceptsReturn = True
        Me._txtWarning_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtWarning_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtWarning_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtWarning.SetIndex(Me._txtWarning_0, CType(0, Short))
        Me._txtWarning_0.Location = New System.Drawing.Point(8, 31)
        Me._txtWarning_0.MaxLength = 0
        Me._txtWarning_0.Multiline = True
        Me._txtWarning_0.Name = "_txtWarning_0"
        Me._txtWarning_0.ReadOnly = True
        Me._txtWarning_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtWarning_0.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me._txtWarning_0.Size = New System.Drawing.Size(677, 211)
        Me._txtWarning_0.TabIndex = 46
        Me._txtWarning_0.WordWrap = False
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(9, 8)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(129, 17)
        Me.Label4.TabIndex = 47
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
        Me._SSTab1_TabPage2.Size = New System.Drawing.Size(734, 331)
        Me._SSTab1_TabPage2.TabIndex = 2
        Me._SSTab1_TabPage2.Text = "Debug"
        '
        '_cmdDebug_4
        '
        Me._cmdDebug_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdDebug_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdDebug_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDebug.SetIndex(Me._cmdDebug_4, CType(4, Short))
        Me._cmdDebug_4.Location = New System.Drawing.Point(628, 130)
        Me._cmdDebug_4.Name = "_cmdDebug_4"
        Me._cmdDebug_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdDebug_4.Size = New System.Drawing.Size(100, 36)
        Me._cmdDebug_4.TabIndex = 43
        Me._cmdDebug_4.Text = "Create XML dataset"
        Me._cmdDebug_4.UseVisualStyleBackColor = False
        '
        '_cmdDebug_3
        '
        Me._cmdDebug_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdDebug_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdDebug_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDebug.SetIndex(Me._cmdDebug_3, CType(3, Short))
        Me._cmdDebug_3.Location = New System.Drawing.Point(628, 88)
        Me._cmdDebug_3.Name = "_cmdDebug_3"
        Me._cmdDebug_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdDebug_3.Size = New System.Drawing.Size(100, 36)
        Me._cmdDebug_3.TabIndex = 42
        Me._cmdDebug_3.Text = "Create default registry settings"
        Me._cmdDebug_3.UseVisualStyleBackColor = False
        '
        '_cmdDebug_2
        '
        Me._cmdDebug_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdDebug_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdDebug_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDebug.SetIndex(Me._cmdDebug_2, CType(2, Short))
        Me._cmdDebug_2.Location = New System.Drawing.Point(628, 172)
        Me._cmdDebug_2.Name = "_cmdDebug_2"
        Me._cmdDebug_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdDebug_2.Size = New System.Drawing.Size(100, 36)
        Me._cmdDebug_2.TabIndex = 41
        Me._cmdDebug_2.Text = "Re-build captions"
        Me._cmdDebug_2.UseVisualStyleBackColor = False
        '
        '_cmdDebug_1
        '
        Me._cmdDebug_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdDebug_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdDebug_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDebug.SetIndex(Me._cmdDebug_1, CType(1, Short))
        Me._cmdDebug_1.Location = New System.Drawing.Point(24, 291)
        Me._cmdDebug_1.Name = "_cmdDebug_1"
        Me._cmdDebug_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdDebug_1.Size = New System.Drawing.Size(188, 26)
        Me._cmdDebug_1.TabIndex = 40
        Me._cmdDebug_1.Text = "Generate object captions "
        Me._cmdDebug_1.UseVisualStyleBackColor = False
        '
        '_cmdDebug_0
        '
        Me._cmdDebug_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdDebug_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdDebug_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDebug.SetIndex(Me._cmdDebug_0, CType(0, Short))
        Me._cmdDebug_0.Location = New System.Drawing.Point(628, 46)
        Me._cmdDebug_0.Name = "_cmdDebug_0"
        Me._cmdDebug_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdDebug_0.Size = New System.Drawing.Size(100, 36)
        Me._cmdDebug_0.TabIndex = 39
        Me._cmdDebug_0.Text = "Create default lists"
        Me._cmdDebug_0.UseVisualStyleBackColor = False
        '
        'txtDebug
        '
        Me.txtDebug.AcceptsReturn = True
        Me.txtDebug.BackColor = System.Drawing.SystemColors.Window
        Me.txtDebug.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDebug.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDebug.Location = New System.Drawing.Point(24, 20)
        Me.txtDebug.MaxLength = 0
        Me.txtDebug.Multiline = True
        Me.txtDebug.Name = "txtDebug"
        Me.txtDebug.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDebug.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtDebug.Size = New System.Drawing.Size(599, 265)
        Me.txtDebug.TabIndex = 2
        Me.txtDebug.WordWrap = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(633, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(95, 13)
        Me.Label1.TabIndex = 44
        Me.Label1.Text = "Datamodel options"
        '
        'CommonDialog1Open
        '
        Me.CommonDialog1Open.DefaultExt = "pbe"
        '
        'chkAdditionalExportOptions
        '
        '
        'cmdDebug
        '
        '
        'chkImportUMLScriptsOnly
        '
        Me.chkImportUMLScriptsOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkImportUMLScriptsOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkImportUMLScriptsOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkImportUMLScriptsOnly.Location = New System.Drawing.Point(452, 26)
        Me.chkImportUMLScriptsOnly.Name = "chkImportUMLScriptsOnly"
        Me.chkImportUMLScriptsOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkImportUMLScriptsOnly.Size = New System.Drawing.Size(189, 21)
        Me.chkImportUMLScriptsOnly.TabIndex = 76
        Me.chkImportUMLScriptsOnly.Text = "Import UML Scripts Only"
        Me.chkImportUMLScriptsOnly.UseVisualStyleBackColor = False
        '
        'mainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(764, 464)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.cboGISLookupUserList)
        Me.Controls.Add(Me.SSTab1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "mainForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Product Builder Product Export/Import Utility"
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me._StatusBar1_0.ResumeLayout(False)
        Me._StatusBar1_0.PerformLayout()
        Me._StatusBar1_2.ResumeLayout(False)
        Me._StatusBar1_2.PerformLayout()
        Me._StatusBar1_1.ResumeLayout(False)
        Me._StatusBar1_1.PerformLayout()
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me.SSTab2.ResumeLayout(False)
        Me._SSTab2_TabPage0.ResumeLayout(False)
        Me.Frame3.ResumeLayout(False)
        Me.Frame3.PerformLayout()
        Me._SSTab2_TabPage1.ResumeLayout(False)
        Me.Frame10.ResumeLayout(False)
        Me.Frame9.ResumeLayout(False)
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
        Me.Frame8.ResumeLayout(False)
        Me.Frame8.PerformLayout()
        Me._SSTab3_TabPage1.ResumeLayout(False)
        Me._SSTab3_TabPage1.PerformLayout()
        Me._SSTab3_TabPage2.ResumeLayout(False)
        Me.Frame7.ResumeLayout(False)
        Me.Frame7.PerformLayout()
        Me.fraImportHeader.ResumeLayout(False)
        Me.fraImportHeader.PerformLayout()
        Me._SSTab3_TabPage3.ResumeLayout(False)
        Me._SSTab3_TabPage3.PerformLayout()
        Me._SSTab1_TabPage2.ResumeLayout(False)
        Me._SSTab1_TabPage2.PerformLayout()
        CType(Me.Label3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ProgressBar1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.StatusBar1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkAdditionalExportOptions, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmdDebug, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblBackup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optAdditionalImportOptions, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtFileExtension, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtFileName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtFilePath, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtWarning, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents _StatusBar1_2 As System.Windows.Forms.StatusStrip
    Private WithEvents _StatusBar1_2_Panel1 As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _StatusBar1_1 As System.Windows.Forms.StatusStrip
    Private WithEvents _StatusBar1_1_Panel1 As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _StatusBar1_0 As System.Windows.Forms.StatusStrip
    Friend WithEvents _StatusBar1_0_Panel1 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents chkIncludeUMLs As System.Windows.Forms.CheckBox
    Public WithEvents chkGenerateUMLScript As System.Windows.Forms.CheckBox
    Public WithEvents chkImportUMLScriptsOnly As System.Windows.Forms.CheckBox
#End Region
End Class