<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRuleSet
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
	Public WithEvents pnlRuleFile As System.Windows.Forms.Panel
	Public WithEvents txtCode As System.Windows.Forms.TextBox
	Public WithEvents chkLive As System.Windows.Forms.CheckBox
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents txtEffectiveDate As System.Windows.Forms.TextBox
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
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.cmdRuleFile = New System.Windows.Forms.Button()
        Me.lblFile = New System.Windows.Forms.Label()
        Me.UctCompiledRule1 = New uctCompiledRule.uctCompiledRule()
        Me.lblRuleType = New System.Windows.Forms.Label()
        Me.cboRuleType = New System.Windows.Forms.ComboBox()
        Me.lblCode = New System.Windows.Forms.Label()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.lblEffectiveDate = New System.Windows.Forms.Label()
        Me.pnlRuleFile = New System.Windows.Forms.Panel()
        Me.lblRuleFile = New System.Windows.Forms.Label()
        Me.txtCode = New System.Windows.Forms.TextBox()
        Me.chkLive = New System.Windows.Forms.CheckBox()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.txtEffectiveDate = New System.Windows.Forms.TextBox()
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog()
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog()
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.pnlRuleFile.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(297, 245)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 6
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
        Me.cmdCancel.Location = New System.Drawing.Point(216, 245)
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
        Me.cmdOK.Location = New System.Drawing.Point(137, 245)
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
        Me.tabMainTab.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(357, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(374, 231)
        Me.tabMainTab.TabIndex = 7
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdRuleFile)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblFile)
        Me._tabMainTab_TabPage0.Controls.Add(Me.UctCompiledRule1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblRuleType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboRuleType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblEffectiveDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pnlRuleFile)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkLive)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtEffectiveDate)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(366, 205)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "Rule"
        '
        'cmdRuleFile
        '
        Me.cmdRuleFile.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRuleFile.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRuleFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRuleFile.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRuleFile.Location = New System.Drawing.Point(320, 151)
        Me.cmdRuleFile.Name = "cmdRuleFile"
        Me.cmdRuleFile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRuleFile.Size = New System.Drawing.Size(38, 26)
        Me.cmdRuleFile.TabIndex = 18
        Me.cmdRuleFile.Text = "..."
        Me.cmdRuleFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRuleFile.UseVisualStyleBackColor = False
        '
        'lblFile
        '
        Me.lblFile.BackColor = System.Drawing.SystemColors.Control
        Me.lblFile.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFile.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFile.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFile.Location = New System.Drawing.Point(21, 152)
        Me.lblFile.Name = "lblFile"
        Me.lblFile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFile.Size = New System.Drawing.Size(108, 28)
        Me.lblFile.TabIndex = 17
        Me.lblFile.Text = "Compiled Rule Assembly:"
        '
        'UctCompiledRule1
        '
        Me.UctCompiledRule1.bEnterOnlyAssemblyName = False
        Me.UctCompiledRule1.Location = New System.Drawing.Point(144, 157)
        Me.UctCompiledRule1.Name = "UctCompiledRule1"
        Me.UctCompiledRule1.Size = New System.Drawing.Size(170, 20)
        Me.UctCompiledRule1.TabIndex = 15
        '
        'lblRuleType
        '
        Me.lblRuleType.AutoSize = True
        Me.lblRuleType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRuleType.Location = New System.Drawing.Point(21, 118)
        Me.lblRuleType.Name = "lblRuleType"
        Me.lblRuleType.Size = New System.Drawing.Size(69, 13)
        Me.lblRuleType.TabIndex = 14
        Me.lblRuleType.Text = "Rule Type:"
        '
        'cboRuleType
        '
        Me.cboRuleType.FormattingEnabled = True
        Me.cboRuleType.Location = New System.Drawing.Point(145, 115)
        Me.cboRuleType.Name = "cboRuleType"
        Me.cboRuleType.Size = New System.Drawing.Size(169, 21)
        Me.cboRuleType.TabIndex = 13
        '
        'lblCode
        '
        Me.lblCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCode.Location = New System.Drawing.Point(21, 19)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCode.Size = New System.Drawing.Size(100, 17)
        Me.lblCode.TabIndex = 8
        Me.lblCode.Text = "Code"
        '
        'lblDescription
        '
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(21, 51)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(100, 17)
        Me.lblDescription.TabIndex = 10
        Me.lblDescription.Text = "Description"
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(21, 83)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(106, 17)
        Me.lblEffectiveDate.TabIndex = 11
        Me.lblEffectiveDate.Text = "Effective Date:"
        '
        'pnlRuleFile
        '
        Me.pnlRuleFile.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.pnlRuleFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlRuleFile.Controls.Add(Me.lblRuleFile)
        Me.pnlRuleFile.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlRuleFile.Location = New System.Drawing.Point(145, 151)
        Me.pnlRuleFile.Name = "pnlRuleFile"
        Me.pnlRuleFile.Size = New System.Drawing.Size(169, 17)
        Me.pnlRuleFile.TabIndex = 12
        '
        'lblRuleFile
        '
        Me.lblRuleFile.AutoSize = True
        Me.lblRuleFile.Location = New System.Drawing.Point(4, -2)
        Me.lblRuleFile.Name = "lblRuleFile"
        Me.lblRuleFile.Size = New System.Drawing.Size(0, 13)
        Me.lblRuleFile.TabIndex = 0
        '
        'txtCode
        '
        Me.txtCode.AcceptsReturn = True
        Me.txtCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCode.Location = New System.Drawing.Point(145, 16)
        Me.txtCode.MaxLength = 10
        Me.txtCode.Name = "txtCode"
        Me.txtCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCode.Size = New System.Drawing.Size(169, 20)
        Me.txtCode.TabIndex = 0
        '
        'chkLive
        '
        Me.chkLive.BackColor = System.Drawing.SystemColors.Control
        Me.chkLive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkLive.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkLive.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkLive.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkLive.Location = New System.Drawing.Point(21, 183)
        Me.chkLive.Name = "chkLive"
        Me.chkLive.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkLive.Size = New System.Drawing.Size(139, 19)
        Me.chkLive.TabIndex = 9
        Me.chkLive.Text = "Live"
        Me.chkLive.UseVisualStyleBackColor = False
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(145, 48)
        Me.txtDescription.MaxLength = 0
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(169, 20)
        Me.txtDescription.TabIndex = 1
        '
        'txtEffectiveDate
        '
        Me.txtEffectiveDate.AcceptsReturn = True
        Me.txtEffectiveDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtEffectiveDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEffectiveDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEffectiveDate.Location = New System.Drawing.Point(145, 80)
        Me.txtEffectiveDate.MaxLength = 0
        Me.txtEffectiveDate.Name = "txtEffectiveDate"
        Me.txtEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEffectiveDate.Size = New System.Drawing.Size(169, 20)
        Me.txtEffectiveDate.TabIndex = 2
        '
        'frmRuleSet
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(394, 269)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmRuleSet"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Rule Set"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.pnlRuleFile.ResumeLayout(False)
        Me.pnlRuleFile.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblRuleFile As System.Windows.Forms.Label
    Friend WithEvents lblRuleType As System.Windows.Forms.Label
    Friend WithEvents cboRuleType As System.Windows.Forms.ComboBox
    Friend WithEvents UctCompiledRule1 As uctCompiledRule.uctCompiledRule
    Public WithEvents cmdRuleFile As System.Windows.Forms.Button
    Public WithEvents lblFile As System.Windows.Forms.Label
#End Region
End Class