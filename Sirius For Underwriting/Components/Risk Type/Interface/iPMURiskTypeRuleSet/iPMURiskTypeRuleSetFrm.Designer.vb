<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
    Public WithEvents lblCode As System.Windows.Forms.Label
    Public WithEvents lblDescription As System.Windows.Forms.Label
    Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
    Public WithEvents lblRuleFile As System.Windows.Forms.Label
    Public WithEvents txtCode As System.Windows.Forms.TextBox
    Public WithEvents chkLive As System.Windows.Forms.CheckBox
    Public WithEvents txtDescription As System.Windows.Forms.TextBox
    Public WithEvents txtEffectiveDate As System.Windows.Forms.TextBox
    Public WithEvents cmdRuleFile As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.lblPREVersion = New System.Windows.Forms.Label()
        Me.cboPREVersion = New System.Windows.Forms.ComboBox()
        Me.lblRuleEffectiveDate = New System.Windows.Forms.Label()
        Me.cboRuleEffectiveDate = New System.Windows.Forms.ComboBox()
        Me.chkChildRuleEffectiveDate = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.UctCompiledRule1 = New uctCompiledRule.uctCompiledRule()
        Me.txtRuleFileText = New System.Windows.Forms.TextBox()
        Me.grpDRERules = New System.Windows.Forms.GroupBox()
        Me.lblPREAssembly = New System.Windows.Forms.Label()
        Me.UctCompiledRulePRE = New uctCompiledRule.uctCompiledRule()
        Me.chkPrePRE = New System.Windows.Forms.CheckBox()
        Me.chkDREDefault = New System.Windows.Forms.CheckBox()
        Me.chkDREValidate = New System.Windows.Forms.CheckBox()
        Me.chkPostPRE = New System.Windows.Forms.CheckBox()
        Me.chkDREQuote = New System.Windows.Forms.CheckBox()
        Me.txtDREDefaultToken = New System.Windows.Forms.TextBox()
        Me.lblDREDefaultToken = New System.Windows.Forms.Label()
        Me.txtDREExecutorURL = New System.Windows.Forms.TextBox()
        Me.lblDREExecutorURL = New System.Windows.Forms.Label()
        Me.cboRuleType = New System.Windows.Forms.ComboBox()
        Me.lblCode = New System.Windows.Forms.Label()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.lblEffectiveDate = New System.Windows.Forms.Label()
        Me.lblRuleFile = New System.Windows.Forms.Label()
        Me.txtCode = New System.Windows.Forms.TextBox()
        Me.chkLive = New System.Windows.Forms.CheckBox()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.txtEffectiveDate = New System.Windows.Forms.TextBox()
        Me.cmdRuleFile = New System.Windows.Forms.Button()
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog()
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog()
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.grpDRERules.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(12, 461)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 12
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
        Me.cmdCancel.Location = New System.Drawing.Point(366, 461)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 11
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(286, 462)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 10
        Me.cmdOK.Text = "OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(424, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(435, 448)
        Me.tabMainTab.TabIndex = 13
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPREVersion)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboPREVersion)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblRuleEffectiveDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboRuleEffectiveDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkChildRuleEffectiveDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Label1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.UctCompiledRule1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtRuleFileText)
        Me._tabMainTab_TabPage0.Controls.Add(Me.grpDRERules)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDREDefaultToken)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDREDefaultToken)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDREExecutorURL)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDREExecutorURL)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboRuleType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblEffectiveDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblRuleFile)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkLive)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtEffectiveDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdRuleFile)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(427, 422)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "Rule"
        '
        'lblPREVersion
        '
        Me.lblPREVersion.AutoSize = True
        Me.lblPREVersion.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblPREVersion.Location = New System.Drawing.Point(13, 194)
        Me.lblPREVersion.Name = "lblPREVersion"
        Me.lblPREVersion.Size = New System.Drawing.Size(88, 13)
        Me.lblPREVersion.TabIndex = 26
        Me.lblPREVersion.Text = "PRE Version:"
        '
        'cboPREVersion
        '
        Me.cboPREVersion.FormattingEnabled = True
        Me.cboPREVersion.Items.AddRange(New Object() {"DRE, PRE", "PRE2"})
        Me.cboPREVersion.Location = New System.Drawing.Point(145, 191)
        Me.cboPREVersion.Name = "cboPREVersion"
        Me.cboPREVersion.Size = New System.Drawing.Size(265, 21)
        Me.cboPREVersion.TabIndex = 25
        '
        'lblRuleEffectiveDate
        '
        Me.lblRuleEffectiveDate.AutoSize = True
        Me.lblRuleEffectiveDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblRuleEffectiveDate.Location = New System.Drawing.Point(13, 273)
        Me.lblRuleEffectiveDate.Name = "lblRuleEffectiveDate"
        Me.lblRuleEffectiveDate.Size = New System.Drawing.Size(181, 13)
        Me.lblRuleEffectiveDate.TabIndex = 24
        Me.lblRuleEffectiveDate.Text = "Pre Ruleset Effective Date:"
        '
        'cboRuleEffectiveDate
        '
        Me.cboRuleEffectiveDate.FormattingEnabled = True
        Me.cboRuleEffectiveDate.Items.AddRange(New Object() {"Transaction Date", "Cover Effective Date", "Inception Date TPI", "Inception Date TPI(Monthly)"})
        Me.cboRuleEffectiveDate.Location = New System.Drawing.Point(196, 270)
        Me.cboRuleEffectiveDate.Name = "cboRuleEffectiveDate"
        Me.cboRuleEffectiveDate.Size = New System.Drawing.Size(214, 21)
        Me.cboRuleEffectiveDate.TabIndex = 23
        '
        'chkChildRuleEffectiveDate
        '
        Me.chkChildRuleEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.chkChildRuleEffectiveDate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkChildRuleEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkChildRuleEffectiveDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkChildRuleEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkChildRuleEffectiveDate.Location = New System.Drawing.Point(165, 293)
        Me.chkChildRuleEffectiveDate.Name = "chkChildRuleEffectiveDate"
        Me.chkChildRuleEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkChildRuleEffectiveDate.Size = New System.Drawing.Size(241, 19)
        Me.chkChildRuleEffectiveDate.TabIndex = 22
        Me.chkChildRuleEffectiveDate.Text = "Use Effective Date in Child Rule Set"
        Me.chkChildRuleEffectiveDate.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(15, 137)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 13)
        Me.Label1.TabIndex = 21
        Me.Label1.Text = "Rule Type:"
        '
        'UctCompiledRule1
        '
        Me.UctCompiledRule1.bEnterOnlyAssemblyName = False
        Me.UctCompiledRule1.Location = New System.Drawing.Point(145, 163)
        Me.UctCompiledRule1.Name = "UctCompiledRule1"
        Me.UctCompiledRule1.Size = New System.Drawing.Size(263, 20)
        Me.UctCompiledRule1.TabIndex = 20
        '
        'txtRuleFileText
        '
        Me.txtRuleFileText.Location = New System.Drawing.Point(145, 163)
        Me.txtRuleFileText.Name = "txtRuleFileText"
        Me.txtRuleFileText.Size = New System.Drawing.Size(234, 20)
        Me.txtRuleFileText.TabIndex = 19
        '
        'grpDRERules
        '
        Me.grpDRERules.Controls.Add(Me.lblPREAssembly)
        Me.grpDRERules.Controls.Add(Me.UctCompiledRulePRE)
        Me.grpDRERules.Controls.Add(Me.chkPrePRE)
        Me.grpDRERules.Controls.Add(Me.chkDREDefault)
        Me.grpDRERules.Controls.Add(Me.chkDREValidate)
        Me.grpDRERules.Controls.Add(Me.chkPostPRE)
        Me.grpDRERules.Controls.Add(Me.chkDREQuote)
        Me.grpDRERules.Location = New System.Drawing.Point(16, 313)
        Me.grpDRERules.Name = "grpDRERules"
        Me.grpDRERules.Size = New System.Drawing.Size(409, 102)
        Me.grpDRERules.TabIndex = 16
        Me.grpDRERules.TabStop = False
        Me.grpDRERules.Text = "Use PRE Rules"
        '
        'lblPREAssembly
        '
        Me.lblPREAssembly.BackColor = System.Drawing.SystemColors.Control
        Me.lblPREAssembly.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPREAssembly.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPREAssembly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPREAssembly.Location = New System.Drawing.Point(4, 74)
        Me.lblPREAssembly.Name = "lblPREAssembly"
        Me.lblPREAssembly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPREAssembly.Size = New System.Drawing.Size(106, 19)
        Me.lblPREAssembly.TabIndex = 23
        Me.lblPREAssembly.Text = "Assembly Name:"
        '
        'UctCompiledRulePRE
        '
        Me.UctCompiledRulePRE.bEnterOnlyAssemblyName = False
        Me.UctCompiledRulePRE.Location = New System.Drawing.Point(113, 73)
        Me.UctCompiledRulePRE.Name = "UctCompiledRulePRE"
        Me.UctCompiledRulePRE.Size = New System.Drawing.Size(282, 20)
        Me.UctCompiledRulePRE.TabIndex = 21
        '
        'chkPrePRE
        '
        Me.chkPrePRE.AutoSize = True
        Me.chkPrePRE.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrePRE.Location = New System.Drawing.Point(8, 46)
        Me.chkPrePRE.Name = "chkPrePRE"
        Me.chkPrePRE.Size = New System.Drawing.Size(183, 17)
        Me.chkPrePRE.TabIndex = 19
        Me.chkPrePRE.Text = "Pre-PRE run additional Rule"
        Me.chkPrePRE.UseVisualStyleBackColor = True
        '
        'chkDREDefault
        '
        Me.chkDREDefault.AutoSize = True
        Me.chkDREDefault.Enabled = False
        Me.chkDREDefault.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDREDefault.Location = New System.Drawing.Point(8, 17)
        Me.chkDREDefault.Name = "chkDREDefault"
        Me.chkDREDefault.Size = New System.Drawing.Size(67, 17)
        Me.chkDREDefault.TabIndex = 0
        Me.chkDREDefault.Text = "Default"
        Me.chkDREDefault.UseVisualStyleBackColor = True
        '
        'chkDREValidate
        '
        Me.chkDREValidate.AutoSize = True
        Me.chkDREValidate.Enabled = False
        Me.chkDREValidate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDREValidate.Location = New System.Drawing.Point(321, 17)
        Me.chkDREValidate.Name = "chkDREValidate"
        Me.chkDREValidate.Size = New System.Drawing.Size(81, 17)
        Me.chkDREValidate.TabIndex = 2
        Me.chkDREValidate.Text = "Validation"
        Me.chkDREValidate.UseVisualStyleBackColor = True
        '
        'chkPostPRE
        '
        Me.chkPostPRE.AutoSize = True
        Me.chkPostPRE.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPostPRE.Location = New System.Drawing.Point(192, 46)
        Me.chkPostPRE.Name = "chkPostPRE"
        Me.chkPostPRE.Size = New System.Drawing.Size(188, 17)
        Me.chkPostPRE.TabIndex = 18
        Me.chkPostPRE.Text = "Post-PRE run additional Rule"
        Me.chkPostPRE.UseVisualStyleBackColor = True
        '
        'chkDREQuote
        '
        Me.chkDREQuote.AutoSize = True
        Me.chkDREQuote.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDREQuote.Location = New System.Drawing.Point(192, 17)
        Me.chkDREQuote.Name = "chkDREQuote"
        Me.chkDREQuote.Size = New System.Drawing.Size(60, 17)
        Me.chkDREQuote.TabIndex = 1
        Me.chkDREQuote.Text = "Quote"
        Me.chkDREQuote.UseVisualStyleBackColor = True
        '
        'txtDREDefaultToken
        '
        Me.txtDREDefaultToken.Location = New System.Drawing.Point(145, 245)
        Me.txtDREDefaultToken.Name = "txtDREDefaultToken"
        Me.txtDREDefaultToken.Size = New System.Drawing.Size(261, 20)
        Me.txtDREDefaultToken.TabIndex = 15
        '
        'lblDREDefaultToken
        '
        Me.lblDREDefaultToken.AutoSize = True
        Me.lblDREDefaultToken.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDREDefaultToken.Location = New System.Drawing.Point(13, 246)
        Me.lblDREDefaultToken.Name = "lblDREDefaultToken"
        Me.lblDREDefaultToken.Size = New System.Drawing.Size(126, 13)
        Me.lblDREDefaultToken.TabIndex = 14
        Me.lblDREDefaultToken.Text = "PRE Profile Token:"
        '
        'txtDREExecutorURL
        '
        Me.txtDREExecutorURL.Location = New System.Drawing.Point(145, 218)
        Me.txtDREExecutorURL.Name = "txtDREExecutorURL"
        Me.txtDREExecutorURL.Size = New System.Drawing.Size(262, 20)
        Me.txtDREExecutorURL.TabIndex = 13
        '
        'lblDREExecutorURL
        '
        Me.lblDREExecutorURL.AutoSize = True
        Me.lblDREExecutorURL.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDREExecutorURL.Location = New System.Drawing.Point(13, 219)
        Me.lblDREExecutorURL.Name = "lblDREExecutorURL"
        Me.lblDREExecutorURL.Size = New System.Drawing.Size(125, 13)
        Me.lblDREExecutorURL.TabIndex = 12
        Me.lblDREExecutorURL.Text = "PRE Executor URL:"
        '
        'cboRuleType
        '
        Me.cboRuleType.FormattingEnabled = True
        Me.cboRuleType.Location = New System.Drawing.Point(145, 134)
        Me.cboRuleType.Name = "cboRuleType"
        Me.cboRuleType.Size = New System.Drawing.Size(263, 21)
        Me.cboRuleType.TabIndex = 11
        '
        'lblCode
        '
        Me.lblCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCode.Location = New System.Drawing.Point(15, 17)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCode.Size = New System.Drawing.Size(100, 19)
        Me.lblCode.TabIndex = 0
        Me.lblCode.Text = "Code:"
        '
        'lblDescription
        '
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(15, 67)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(100, 19)
        Me.lblDescription.TabIndex = 4
        Me.lblDescription.Text = "Description:"
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(15, 42)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(106, 19)
        Me.lblEffectiveDate.TabIndex = 2
        Me.lblEffectiveDate.Text = "Effective Date:"
        '
        'lblRuleFile
        '
        Me.lblRuleFile.BackColor = System.Drawing.SystemColors.Control
        Me.lblRuleFile.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRuleFile.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRuleFile.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRuleFile.Location = New System.Drawing.Point(15, 166)
        Me.lblRuleFile.Name = "lblRuleFile"
        Me.lblRuleFile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRuleFile.Size = New System.Drawing.Size(108, 28)
        Me.lblRuleFile.TabIndex = 6
        Me.lblRuleFile.Text = "Rule File:"
        '
        'txtCode
        '
        Me.txtCode.AcceptsReturn = True
        Me.txtCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCode.Location = New System.Drawing.Point(145, 15)
        Me.txtCode.MaxLength = 10
        Me.txtCode.Name = "txtCode"
        Me.txtCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCode.Size = New System.Drawing.Size(124, 20)
        Me.txtCode.TabIndex = 1
        '
        'chkLive
        '
        Me.chkLive.BackColor = System.Drawing.SystemColors.Control
        Me.chkLive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkLive.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkLive.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkLive.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkLive.Location = New System.Drawing.Point(16, 293)
        Me.chkLive.Name = "chkLive"
        Me.chkLive.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkLive.Size = New System.Drawing.Size(143, 19)
        Me.chkLive.TabIndex = 9
        Me.chkLive.Text = "Rule is Live:"
        Me.chkLive.UseVisualStyleBackColor = False
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(145, 65)
        Me.txtDescription.MaxLength = 255
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDescription.Size = New System.Drawing.Size(264, 63)
        Me.txtDescription.TabIndex = 5
        '
        'txtEffectiveDate
        '
        Me.txtEffectiveDate.AcceptsReturn = True
        Me.txtEffectiveDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtEffectiveDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEffectiveDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEffectiveDate.Location = New System.Drawing.Point(145, 40)
        Me.txtEffectiveDate.MaxLength = 20
        Me.txtEffectiveDate.Name = "txtEffectiveDate"
        Me.txtEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEffectiveDate.Size = New System.Drawing.Size(124, 20)
        Me.txtEffectiveDate.TabIndex = 3
        '
        'cmdRuleFile
        '
        Me.cmdRuleFile.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRuleFile.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRuleFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRuleFile.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRuleFile.Location = New System.Drawing.Point(386, 159)
        Me.cmdRuleFile.Name = "cmdRuleFile"
        Me.cmdRuleFile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRuleFile.Size = New System.Drawing.Size(23, 22)
        Me.cmdRuleFile.TabIndex = 8
        Me.cmdRuleFile.Text = "..."
        Me.cmdRuleFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRuleFile.UseVisualStyleBackColor = False
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "tickgrey.gif")
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(449, 491)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Risk Type Rule Set"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.grpDRERules.ResumeLayout(False)
        Me.grpDRERules.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cboRuleType As System.Windows.Forms.ComboBox
    Friend WithEvents txtDREDefaultToken As System.Windows.Forms.TextBox
    Friend WithEvents lblDREDefaultToken As System.Windows.Forms.Label
    Friend WithEvents txtDREExecutorURL As System.Windows.Forms.TextBox
    Friend WithEvents lblDREExecutorURL As System.Windows.Forms.Label
    Friend WithEvents grpDRERules As System.Windows.Forms.GroupBox
    Friend WithEvents chkDREDefault As System.Windows.Forms.CheckBox
    Friend WithEvents chkDREValidate As System.Windows.Forms.CheckBox
    Friend WithEvents chkDREQuote As System.Windows.Forms.CheckBox
    Friend WithEvents chkPostPRE As System.Windows.Forms.CheckBox
    Friend WithEvents txtRuleFileText As System.Windows.Forms.TextBox
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents UctCompiledRule1 As uctCompiledRule.uctCompiledRule
    Friend WithEvents Label1 As Label
    Public WithEvents lblPREAssembly As Label
    Friend WithEvents UctCompiledRulePRE As uctCompiledRule.uctCompiledRule
    Friend WithEvents chkPrePRE As CheckBox
    Public WithEvents chkChildRuleEffectiveDate As CheckBox
    Friend WithEvents lblRuleEffectiveDate As Label
    Friend WithEvents cboRuleEffectiveDate As ComboBox
    Friend WithEvents lblPREVersion As Label
    Friend WithEvents cboPREVersion As ComboBox
#End Region
End Class