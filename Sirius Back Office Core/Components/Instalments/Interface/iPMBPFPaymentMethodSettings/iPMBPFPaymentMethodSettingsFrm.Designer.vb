<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPaymentMethodEdit
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializecmdNext()
		tabMainTabPreviousTab = tabMainTab.SelectedIndex
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
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Private WithEvents _cmdNext_0 As System.Windows.Forms.Button
	Public WithEvents txtFilename As System.Windows.Forms.TextBox
	Public WithEvents txtDirectory As System.Windows.Forms.TextBox
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents lblFilename As System.Windows.Forms.Label
	Public WithEvents lblDirectory As System.Windows.Forms.Label
	Public WithEvents fraPath As System.Windows.Forms.GroupBox
	Public WithEvents txtDateFormat As System.Windows.Forms.TextBox
	Public WithEvents chkAccountNumbersOnly As System.Windows.Forms.CheckBox
	Public WithEvents chkQuotedStrings As System.Windows.Forms.CheckBox
	Public WithEvents txtQuote As System.Windows.Forms.TextBox
	Public WithEvents chkQuotedNumerics As System.Windows.Forms.CheckBox
	Public WithEvents chkAmountInPence As System.Windows.Forms.CheckBox
	Public WithEvents chkASCIIValue As System.Windows.Forms.CheckBox
	Public WithEvents txtDelimeter As System.Windows.Forms.TextBox
	Public WithEvents txtDetail As System.Windows.Forms.TextBox
	Public WithEvents txtFooter As System.Windows.Forms.TextBox
	Public WithEvents txtHeader As System.Windows.Forms.TextBox
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents lblExample As System.Windows.Forms.Label
	Public WithEvents lblDateFormat As System.Windows.Forms.Label
	Public WithEvents lblQuote As System.Windows.Forms.Label
	Public WithEvents lblDelimeter As System.Windows.Forms.Label
	Public WithEvents lblDetail As System.Windows.Forms.Label
	Public WithEvents lblFooter As System.Windows.Forms.Label
	Public WithEvents lblHeader As System.Windows.Forms.Label
	Public WithEvents fraFormat As System.Windows.Forms.GroupBox
	Public WithEvents chkExcludeAudis As System.Windows.Forms.CheckBox
	Public WithEvents chkAllowAutoPost As System.Windows.Forms.CheckBox
	Public WithEvents chkDisableExport As System.Windows.Forms.CheckBox
	Public WithEvents fraMain As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public cmdNext(0) As System.Windows.Forms.Button
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtDateFormat = New System.Windows.Forms.TextBox
        Me.chkAccountNumbersOnly = New System.Windows.Forms.CheckBox
        Me.chkQuotedStrings = New System.Windows.Forms.CheckBox
        Me.chkQuotedNumerics = New System.Windows.Forms.CheckBox
        Me.chkAmountInPence = New System.Windows.Forms.CheckBox
        Me.chkASCIIValue = New System.Windows.Forms.CheckBox
        Me.chkExcludeAudis = New System.Windows.Forms.CheckBox
        Me.chkAllowAutoPost = New System.Windows.Forms.CheckBox
        Me.chkDisableExport = New System.Windows.Forms.CheckBox
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.cmdNavigate = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me._cmdNext_0 = New System.Windows.Forms.Button
        Me.fraPath = New System.Windows.Forms.GroupBox
        Me.txtFilename = New System.Windows.Forms.TextBox
        Me.txtDirectory = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblFilename = New System.Windows.Forms.Label
        Me.lblDirectory = New System.Windows.Forms.Label
        Me.fraFormat = New System.Windows.Forms.GroupBox
        Me.txtQuote = New System.Windows.Forms.TextBox
        Me.txtDelimeter = New System.Windows.Forms.TextBox
        Me.txtDetail = New System.Windows.Forms.TextBox
        Me.txtFooter = New System.Windows.Forms.TextBox
        Me.txtHeader = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblExample = New System.Windows.Forms.Label
        Me.lblDateFormat = New System.Windows.Forms.Label
        Me.lblQuote = New System.Windows.Forms.Label
        Me.lblDelimeter = New System.Windows.Forms.Label
        Me.lblDetail = New System.Windows.Forms.Label
        Me.lblFooter = New System.Windows.Forms.Label
        Me.lblHeader = New System.Windows.Forms.Label
        Me.fraMain = New System.Windows.Forms.GroupBox
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.fraPath.SuspendLayout()
        Me.fraFormat.SuspendLayout()
        Me.fraMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtDateFormat
        '
        Me.txtDateFormat.AcceptsReturn = True
        Me.txtDateFormat.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateFormat.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDateFormat.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDateFormat.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateFormat.Location = New System.Drawing.Point(80, 214)
        Me.txtDateFormat.MaxLength = 0
        Me.txtDateFormat.Name = "txtDateFormat"
        Me.txtDateFormat.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateFormat.Size = New System.Drawing.Size(129, 21)
        Me.txtDateFormat.TabIndex = 28
        Me.ToolTip1.SetToolTip(Me.txtDateFormat, "Use standard formatting codes for the Date/Time <D> tag")
        '
        'chkAccountNumbersOnly
        '
        Me.chkAccountNumbersOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkAccountNumbersOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAccountNumbersOnly.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAccountNumbersOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAccountNumbersOnly.Location = New System.Drawing.Point(248, 192)
        Me.chkAccountNumbersOnly.Name = "chkAccountNumbersOnly"
        Me.chkAccountNumbersOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAccountNumbersOnly.Size = New System.Drawing.Size(137, 17)
        Me.chkAccountNumbersOnly.TabIndex = 27
        Me.chkAccountNumbersOnly.Text = "A/c - Numbers Only"
        Me.ToolTip1.SetToolTip(Me.chkAccountNumbersOnly, "Tick if you want just numbers in the Bank Sort-Code and Account Numbers.")
        Me.chkAccountNumbersOnly.UseVisualStyleBackColor = False
        '
        'chkQuotedStrings
        '
        Me.chkQuotedStrings.BackColor = System.Drawing.SystemColors.Control
        Me.chkQuotedStrings.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkQuotedStrings.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkQuotedStrings.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkQuotedStrings.Location = New System.Drawing.Point(248, 168)
        Me.chkQuotedStrings.Name = "chkQuotedStrings"
        Me.chkQuotedStrings.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkQuotedStrings.Size = New System.Drawing.Size(121, 17)
        Me.chkQuotedStrings.TabIndex = 25
        Me.chkQuotedStrings.Text = "Quoted Strings"
        Me.ToolTip1.SetToolTip(Me.chkQuotedStrings, "Tick if you want quotes around text values.")
        Me.chkQuotedStrings.UseVisualStyleBackColor = False
        '
        'chkQuotedNumerics
        '
        Me.chkQuotedNumerics.BackColor = System.Drawing.SystemColors.Control
        Me.chkQuotedNumerics.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkQuotedNumerics.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkQuotedNumerics.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkQuotedNumerics.Location = New System.Drawing.Point(120, 168)
        Me.chkQuotedNumerics.Name = "chkQuotedNumerics"
        Me.chkQuotedNumerics.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkQuotedNumerics.Size = New System.Drawing.Size(121, 17)
        Me.chkQuotedNumerics.TabIndex = 24
        Me.chkQuotedNumerics.Text = "Quoted Numerics"
        Me.ToolTip1.SetToolTip(Me.chkQuotedNumerics, "Tick if you want quotes around numbers.")
        Me.chkQuotedNumerics.UseVisualStyleBackColor = False
        '
        'chkAmountInPence
        '
        Me.chkAmountInPence.BackColor = System.Drawing.SystemColors.Control
        Me.chkAmountInPence.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAmountInPence.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAmountInPence.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAmountInPence.Location = New System.Drawing.Point(248, 144)
        Me.chkAmountInPence.Name = "chkAmountInPence"
        Me.chkAmountInPence.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAmountInPence.Size = New System.Drawing.Size(129, 17)
        Me.chkAmountInPence.TabIndex = 22
        Me.chkAmountInPence.Text = "Amount in Pence"
        Me.ToolTip1.SetToolTip(Me.chkAmountInPence, "Tick if the amount should be exported as pence (x100).")
        Me.chkAmountInPence.UseVisualStyleBackColor = False
        '
        'chkASCIIValue
        '
        Me.chkASCIIValue.BackColor = System.Drawing.SystemColors.Control
        Me.chkASCIIValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkASCIIValue.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkASCIIValue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkASCIIValue.Location = New System.Drawing.Point(120, 144)
        Me.chkASCIIValue.Name = "chkASCIIValue"
        Me.chkASCIIValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkASCIIValue.Size = New System.Drawing.Size(105, 17)
        Me.chkASCIIValue.TabIndex = 21
        Me.chkASCIIValue.Text = "ASCII Value"
        Me.ToolTip1.SetToolTip(Me.chkASCIIValue, "Tick if the delimeter is an ASCII value. Eg. 9=Tab")
        Me.chkASCIIValue.UseVisualStyleBackColor = False
        '
        'chkExcludeAudis
        '
        Me.chkExcludeAudis.BackColor = System.Drawing.SystemColors.Control
        Me.chkExcludeAudis.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkExcludeAudis.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkExcludeAudis.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkExcludeAudis.Location = New System.Drawing.Point(8, 20)
        Me.chkExcludeAudis.Name = "chkExcludeAudis"
        Me.chkExcludeAudis.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkExcludeAudis.Size = New System.Drawing.Size(121, 17)
        Me.chkExcludeAudis.TabIndex = 33
        Me.chkExcludeAudis.Text = "Exclude Audis"
        Me.ToolTip1.SetToolTip(Me.chkExcludeAudis, "Tick if you want the AUDIS line exlcuded?")
        Me.chkExcludeAudis.UseVisualStyleBackColor = False
        '
        'chkAllowAutoPost
        '
        Me.chkAllowAutoPost.BackColor = System.Drawing.SystemColors.Control
        Me.chkAllowAutoPost.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAllowAutoPost.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAllowAutoPost.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAllowAutoPost.Location = New System.Drawing.Point(142, 20)
        Me.chkAllowAutoPost.Name = "chkAllowAutoPost"
        Me.chkAllowAutoPost.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllowAutoPost.Size = New System.Drawing.Size(113, 17)
        Me.chkAllowAutoPost.TabIndex = 34
        Me.chkAllowAutoPost.Text = "Allow Auto Post"
        Me.ToolTip1.SetToolTip(Me.chkAllowAutoPost, "Tick if you want the users to be able to auto-post the export.")
        Me.chkAllowAutoPost.UseVisualStyleBackColor = False
        '
        'chkDisableExport
        '
        Me.chkDisableExport.BackColor = System.Drawing.SystemColors.Control
        Me.chkDisableExport.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDisableExport.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDisableExport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDisableExport.Location = New System.Drawing.Point(276, 18)
        Me.chkDisableExport.Name = "chkDisableExport"
        Me.chkDisableExport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDisableExport.Size = New System.Drawing.Size(105, 17)
        Me.chkDisableExport.TabIndex = 35
        Me.chkDisableExport.Text = "Disable Export"
        Me.ToolTip1.SetToolTip(Me.chkDisableExport, "Tick if you want to disable the export and just post.")
        Me.chkDisableExport.UseVisualStyleBackColor = False
        Me.chkDisableExport.Visible = False
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(88, 446)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 14
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        Me.cmdNavigate.Visible = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(344, 446)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 17
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
        Me.cmdCancel.Location = New System.Drawing.Point(264, 446)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 16
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
        Me.cmdOK.Location = New System.Drawing.Point(184, 446)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 15
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(80, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(413, 437)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdNext_0)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraPath)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraFormat)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraMain)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(405, 411)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "&1 - General"
        '
        '_cmdNext_0
        '
        Me._cmdNext_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_0.Location = New System.Drawing.Point(384, 388)
        Me._cmdNext_0.Name = "_cmdNext_0"
        Me._cmdNext_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_0.Size = New System.Drawing.Size(22, 19)
        Me._cmdNext_0.TabIndex = 18
        Me._cmdNext_0.TabStop = False
        Me._cmdNext_0.Text = ">>"
        Me._cmdNext_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_0.UseVisualStyleBackColor = False
        Me._cmdNext_0.Visible = False
        '
        'fraPath
        '
        Me.fraPath.BackColor = System.Drawing.SystemColors.Control
        Me.fraPath.Controls.Add(Me.txtFilename)
        Me.fraPath.Controls.Add(Me.txtDirectory)
        Me.fraPath.Controls.Add(Me.Label1)
        Me.fraPath.Controls.Add(Me.lblFilename)
        Me.fraPath.Controls.Add(Me.lblDirectory)
        Me.fraPath.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPath.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPath.Location = New System.Drawing.Point(8, 60)
        Me.fraPath.Name = "fraPath"
        Me.fraPath.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPath.Size = New System.Drawing.Size(393, 97)
        Me.fraPath.TabIndex = 7
        Me.fraPath.TabStop = False
        Me.fraPath.Text = "Export Filename"
        '
        'txtFilename
        '
        Me.txtFilename.AcceptsReturn = True
        Me.txtFilename.BackColor = System.Drawing.SystemColors.Window
        Me.txtFilename.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFilename.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFilename.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFilename.Location = New System.Drawing.Point(80, 48)
        Me.txtFilename.MaxLength = 0
        Me.txtFilename.Name = "txtFilename"
        Me.txtFilename.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFilename.Size = New System.Drawing.Size(305, 21)
        Me.txtFilename.TabIndex = 11
        '
        'txtDirectory
        '
        Me.txtDirectory.AcceptsReturn = True
        Me.txtDirectory.BackColor = System.Drawing.SystemColors.Window
        Me.txtDirectory.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDirectory.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDirectory.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDirectory.Location = New System.Drawing.Point(80, 24)
        Me.txtDirectory.MaxLength = 0
        Me.txtDirectory.Name = "txtDirectory"
        Me.txtDirectory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDirectory.Size = New System.Drawing.Size(305, 21)
        Me.txtDirectory.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(8, 72)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(377, 17)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Existing files will always be overwritten."
        '
        'lblFilename
        '
        Me.lblFilename.BackColor = System.Drawing.SystemColors.Control
        Me.lblFilename.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFilename.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFilename.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFilename.Location = New System.Drawing.Point(8, 50)
        Me.lblFilename.Name = "lblFilename"
        Me.lblFilename.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFilename.Size = New System.Drawing.Size(89, 17)
        Me.lblFilename.TabIndex = 3
        Me.lblFilename.Text = "Filename:"
        '
        'lblDirectory
        '
        Me.lblDirectory.BackColor = System.Drawing.SystemColors.Control
        Me.lblDirectory.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDirectory.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDirectory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDirectory.Location = New System.Drawing.Point(8, 26)
        Me.lblDirectory.Name = "lblDirectory"
        Me.lblDirectory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDirectory.Size = New System.Drawing.Size(89, 17)
        Me.lblDirectory.TabIndex = 1
        Me.lblDirectory.Text = "Directory:"
        '
        'fraFormat
        '
        Me.fraFormat.BackColor = System.Drawing.SystemColors.Control
        Me.fraFormat.Controls.Add(Me.txtDateFormat)
        Me.fraFormat.Controls.Add(Me.chkAccountNumbersOnly)
        Me.fraFormat.Controls.Add(Me.chkQuotedStrings)
        Me.fraFormat.Controls.Add(Me.txtQuote)
        Me.fraFormat.Controls.Add(Me.chkQuotedNumerics)
        Me.fraFormat.Controls.Add(Me.chkAmountInPence)
        Me.fraFormat.Controls.Add(Me.chkASCIIValue)
        Me.fraFormat.Controls.Add(Me.txtDelimeter)
        Me.fraFormat.Controls.Add(Me.txtDetail)
        Me.fraFormat.Controls.Add(Me.txtFooter)
        Me.fraFormat.Controls.Add(Me.txtHeader)
        Me.fraFormat.Controls.Add(Me.Label2)
        Me.fraFormat.Controls.Add(Me.lblExample)
        Me.fraFormat.Controls.Add(Me.lblDateFormat)
        Me.fraFormat.Controls.Add(Me.lblQuote)
        Me.fraFormat.Controls.Add(Me.lblDelimeter)
        Me.fraFormat.Controls.Add(Me.lblDetail)
        Me.fraFormat.Controls.Add(Me.lblFooter)
        Me.fraFormat.Controls.Add(Me.lblHeader)
        Me.fraFormat.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraFormat.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFormat.Location = New System.Drawing.Point(8, 164)
        Me.fraFormat.Name = "fraFormat"
        Me.fraFormat.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFormat.Size = New System.Drawing.Size(393, 241)
        Me.fraFormat.TabIndex = 8
        Me.fraFormat.TabStop = False
        Me.fraFormat.Text = "Export Format"
        '
        'txtQuote
        '
        Me.txtQuote.AcceptsReturn = True
        Me.txtQuote.BackColor = System.Drawing.SystemColors.Window
        Me.txtQuote.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtQuote.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtQuote.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtQuote.Location = New System.Drawing.Point(80, 168)
        Me.txtQuote.MaxLength = 0
        Me.txtQuote.Name = "txtQuote"
        Me.txtQuote.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtQuote.Size = New System.Drawing.Size(25, 21)
        Me.txtQuote.TabIndex = 23
        '
        'txtDelimeter
        '
        Me.txtDelimeter.AcceptsReturn = True
        Me.txtDelimeter.BackColor = System.Drawing.SystemColors.Window
        Me.txtDelimeter.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDelimeter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDelimeter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDelimeter.Location = New System.Drawing.Point(80, 144)
        Me.txtDelimeter.MaxLength = 0
        Me.txtDelimeter.Name = "txtDelimeter"
        Me.txtDelimeter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDelimeter.Size = New System.Drawing.Size(25, 21)
        Me.txtDelimeter.TabIndex = 19
        '
        'txtDetail
        '
        Me.txtDetail.AcceptsReturn = True
        Me.txtDetail.BackColor = System.Drawing.SystemColors.Window
        Me.txtDetail.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDetail.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetail.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDetail.Location = New System.Drawing.Point(80, 72)
        Me.txtDetail.MaxLength = 0
        Me.txtDetail.Name = "txtDetail"
        Me.txtDetail.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDetail.Size = New System.Drawing.Size(305, 21)
        Me.txtDetail.TabIndex = 12
        '
        'txtFooter
        '
        Me.txtFooter.AcceptsReturn = True
        Me.txtFooter.BackColor = System.Drawing.SystemColors.Window
        Me.txtFooter.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFooter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFooter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFooter.Location = New System.Drawing.Point(80, 96)
        Me.txtFooter.MaxLength = 0
        Me.txtFooter.Multiline = True
        Me.txtFooter.Name = "txtFooter"
        Me.txtFooter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFooter.Size = New System.Drawing.Size(305, 43)
        Me.txtFooter.TabIndex = 13
        '
        'txtHeader
        '
        Me.txtHeader.AcceptsReturn = True
        Me.txtHeader.BackColor = System.Drawing.SystemColors.Window
        Me.txtHeader.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHeader.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHeader.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtHeader.Location = New System.Drawing.Point(80, 24)
        Me.txtHeader.MaxLength = 0
        Me.txtHeader.Multiline = True
        Me.txtHeader.Name = "txtHeader"
        Me.txtHeader.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHeader.Size = New System.Drawing.Size(305, 43)
        Me.txtHeader.TabIndex = 10
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(8, 200)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(185, 17)
        Me.Label2.TabIndex = 31
        Me.Label2.Text = "Date/Time Format for <D> tag"
        '
        'lblExample
        '
        Me.lblExample.BackColor = System.Drawing.SystemColors.Control
        Me.lblExample.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExample.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExample.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExample.Location = New System.Drawing.Point(224, 216)
        Me.lblExample.Name = "lblExample"
        Me.lblExample.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExample.Size = New System.Drawing.Size(153, 17)
        Me.lblExample.TabIndex = 30
        '
        'lblDateFormat
        '
        Me.lblDateFormat.BackColor = System.Drawing.SystemColors.Control
        Me.lblDateFormat.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDateFormat.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateFormat.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDateFormat.Location = New System.Drawing.Point(8, 216)
        Me.lblDateFormat.Name = "lblDateFormat"
        Me.lblDateFormat.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDateFormat.Size = New System.Drawing.Size(65, 17)
        Me.lblDateFormat.TabIndex = 29
        Me.lblDateFormat.Text = "Format:"
        '
        'lblQuote
        '
        Me.lblQuote.BackColor = System.Drawing.SystemColors.Control
        Me.lblQuote.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblQuote.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblQuote.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblQuote.Location = New System.Drawing.Point(8, 170)
        Me.lblQuote.Name = "lblQuote"
        Me.lblQuote.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblQuote.Size = New System.Drawing.Size(57, 17)
        Me.lblQuote.TabIndex = 25
        Me.lblQuote.Text = "Quote:"
        '
        'lblDelimeter
        '
        Me.lblDelimeter.BackColor = System.Drawing.SystemColors.Control
        Me.lblDelimeter.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDelimeter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDelimeter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDelimeter.Location = New System.Drawing.Point(8, 146)
        Me.lblDelimeter.Name = "lblDelimeter"
        Me.lblDelimeter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDelimeter.Size = New System.Drawing.Size(73, 17)
        Me.lblDelimeter.TabIndex = 20
        Me.lblDelimeter.Text = "Delimeter:"
        '
        'lblDetail
        '
        Me.lblDetail.BackColor = System.Drawing.SystemColors.Control
        Me.lblDetail.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDetail.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDetail.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDetail.Location = New System.Drawing.Point(8, 72)
        Me.lblDetail.Name = "lblDetail"
        Me.lblDetail.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDetail.Size = New System.Drawing.Size(57, 17)
        Me.lblDetail.TabIndex = 5
        Me.lblDetail.Text = "Detail:"
        '
        'lblFooter
        '
        Me.lblFooter.BackColor = System.Drawing.SystemColors.Control
        Me.lblFooter.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFooter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFooter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFooter.Location = New System.Drawing.Point(8, 96)
        Me.lblFooter.Name = "lblFooter"
        Me.lblFooter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFooter.Size = New System.Drawing.Size(89, 17)
        Me.lblFooter.TabIndex = 6
        Me.lblFooter.Text = "Footer:"
        '
        'lblHeader
        '
        Me.lblHeader.BackColor = System.Drawing.SystemColors.Control
        Me.lblHeader.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblHeader.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHeader.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblHeader.Location = New System.Drawing.Point(8, 26)
        Me.lblHeader.Name = "lblHeader"
        Me.lblHeader.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblHeader.Size = New System.Drawing.Size(89, 17)
        Me.lblHeader.TabIndex = 2
        Me.lblHeader.Text = "Header:"
        '
        'fraMain
        '
        Me.fraMain.BackColor = System.Drawing.SystemColors.Control
        Me.fraMain.Controls.Add(Me.chkExcludeAudis)
        Me.fraMain.Controls.Add(Me.chkAllowAutoPost)
        Me.fraMain.Controls.Add(Me.chkDisableExport)
        Me.fraMain.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraMain.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraMain.Location = New System.Drawing.Point(8, 4)
        Me.fraMain.Name = "fraMain"
        Me.fraMain.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraMain.Size = New System.Drawing.Size(393, 49)
        Me.fraMain.TabIndex = 32
        Me.fraMain.TabStop = False
        Me.fraMain.Text = "Main Settings"
        '
        'frmPaymentMethodEdit
        '
        Me.AcceptButton = Me._cmdNext_0
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(425, 473)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPaymentMethodEdit"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Payment Method Definition"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.fraPath.ResumeLayout(False)
        Me.fraPath.PerformLayout()
        Me.fraFormat.ResumeLayout(False)
        Me.fraFormat.PerformLayout()
        Me.fraMain.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
	Sub InitializecmdNext()
		Me.cmdNext(0) = _cmdNext_0
	End Sub
#End Region 
End Class