<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializefraUserDefined()
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
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cboPeriod As System.Windows.Forms.ComboBox
	Public WithEvents cboType As System.Windows.Forms.ComboBox
	Public WithEvents txtPercentage As System.Windows.Forms.TextBox
	Public WithEvents txtEffectiveDate As System.Windows.Forms.TextBox
	Public WithEvents txtValue As System.Windows.Forms.TextBox
	Public WithEvents lblPercentage As System.Windows.Forms.Label
	Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
	Public WithEvents lblValue As System.Windows.Forms.Label
	Public WithEvents lblPeriod As System.Windows.Forms.Label
	Public WithEvents lblType As System.Windows.Forms.Label
	Private WithEvents _fraUserDefined_0 As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public fraUserDefined(0) As System.Windows.Forms.GroupBox
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me._fraUserDefined_0 = New System.Windows.Forms.GroupBox
        Me.cboPeriod = New System.Windows.Forms.ComboBox
        Me.cboType = New System.Windows.Forms.ComboBox
        Me.txtPercentage = New System.Windows.Forms.TextBox
        Me.txtEffectiveDate = New System.Windows.Forms.TextBox
        Me.txtValue = New System.Windows.Forms.TextBox
        Me.lblPercentage = New System.Windows.Forms.Label
        Me.lblEffectiveDate = New System.Windows.Forms.Label
        Me.lblValue = New System.Windows.Forms.Label
        Me.lblPeriod = New System.Windows.Forms.Label
        Me.lblType = New System.Windows.Forms.Label
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me._fraUserDefined_0.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(339, 276)
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
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(259, 276)
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
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(179, 276)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(79, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(407, 264)
        Me.tabMainTab.TabIndex = 3
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me._fraUserDefined_0)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(399, 238)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "&1-Short Period Rate"
        '
        '_fraUserDefined_0
        '
        Me._fraUserDefined_0.BackColor = System.Drawing.SystemColors.Control
        Me._fraUserDefined_0.Controls.Add(Me.cboPeriod)
        Me._fraUserDefined_0.Controls.Add(Me.cboType)
        Me._fraUserDefined_0.Controls.Add(Me.txtPercentage)
        Me._fraUserDefined_0.Controls.Add(Me.txtEffectiveDate)
        Me._fraUserDefined_0.Controls.Add(Me.txtValue)
        Me._fraUserDefined_0.Controls.Add(Me.lblPercentage)
        Me._fraUserDefined_0.Controls.Add(Me.lblEffectiveDate)
        Me._fraUserDefined_0.Controls.Add(Me.lblValue)
        Me._fraUserDefined_0.Controls.Add(Me.lblPeriod)
        Me._fraUserDefined_0.Controls.Add(Me.lblType)
        Me._fraUserDefined_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraUserDefined_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraUserDefined_0.Location = New System.Drawing.Point(16, 12)
        Me._fraUserDefined_0.Name = "_fraUserDefined_0"
        Me._fraUserDefined_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraUserDefined_0.Size = New System.Drawing.Size(371, 209)
        Me._fraUserDefined_0.TabIndex = 4
        Me._fraUserDefined_0.TabStop = False
        '
        'cboPeriod
        '
        Me.cboPeriod.BackColor = System.Drawing.SystemColors.Window
        Me.cboPeriod.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPeriod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPeriod.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPeriod.Location = New System.Drawing.Point(138, 56)
        Me.cboPeriod.Name = "cboPeriod"
        Me.cboPeriod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPeriod.Size = New System.Drawing.Size(81, 21)
        Me.cboPeriod.TabIndex = 14
        '
        'cboType
        '
        Me.cboType.BackColor = System.Drawing.SystemColors.Window
        Me.cboType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboType.Location = New System.Drawing.Point(138, 24)
        Me.cboType.Name = "cboType"
        Me.cboType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboType.Size = New System.Drawing.Size(153, 21)
        Me.cboType.TabIndex = 13
        '
        'txtPercentage
        '
        Me.txtPercentage.AcceptsReturn = True
        Me.txtPercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtPercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPercentage.Location = New System.Drawing.Point(138, 152)
        Me.txtPercentage.MaxLength = 0
        Me.txtPercentage.Name = "txtPercentage"
        Me.txtPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPercentage.Size = New System.Drawing.Size(145, 19)
        Me.txtPercentage.TabIndex = 9
        '
        'txtEffectiveDate
        '
        Me.txtEffectiveDate.AcceptsReturn = True
        Me.txtEffectiveDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtEffectiveDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEffectiveDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEffectiveDate.Location = New System.Drawing.Point(138, 120)
        Me.txtEffectiveDate.MaxLength = 0
        Me.txtEffectiveDate.Name = "txtEffectiveDate"
        Me.txtEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEffectiveDate.Size = New System.Drawing.Size(145, 19)
        Me.txtEffectiveDate.TabIndex = 8
        '
        'txtValue
        '
        Me.txtValue.AcceptsReturn = True
        Me.txtValue.BackColor = System.Drawing.SystemColors.Window
        Me.txtValue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtValue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtValue.Location = New System.Drawing.Point(138, 88)
        Me.txtValue.MaxLength = 0
        Me.txtValue.Name = "txtValue"
        Me.txtValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtValue.Size = New System.Drawing.Size(145, 19)
        Me.txtValue.TabIndex = 7
        '
        'lblPercentage
        '
        Me.lblPercentage.BackColor = System.Drawing.SystemColors.Control
        Me.lblPercentage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPercentage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPercentage.Location = New System.Drawing.Point(16, 155)
        Me.lblPercentage.Name = "lblPercentage"
        Me.lblPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPercentage.Size = New System.Drawing.Size(89, 17)
        Me.lblPercentage.TabIndex = 12
        Me.lblPercentage.Text = "Percentage:"
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(16, 123)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(113, 17)
        Me.lblEffectiveDate.TabIndex = 11
        Me.lblEffectiveDate.Text = "Effective Date:"
        '
        'lblValue
        '
        Me.lblValue.BackColor = System.Drawing.SystemColors.Control
        Me.lblValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblValue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblValue.Location = New System.Drawing.Point(16, 91)
        Me.lblValue.Name = "lblValue"
        Me.lblValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblValue.Size = New System.Drawing.Size(89, 17)
        Me.lblValue.TabIndex = 10
        Me.lblValue.Text = "Value:"
        '
        'lblPeriod
        '
        Me.lblPeriod.BackColor = System.Drawing.SystemColors.Control
        Me.lblPeriod.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPeriod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPeriod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPeriod.Location = New System.Drawing.Point(16, 59)
        Me.lblPeriod.Name = "lblPeriod"
        Me.lblPeriod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPeriod.Size = New System.Drawing.Size(89, 17)
        Me.lblPeriod.TabIndex = 6
        Me.lblPeriod.Text = "Period:"
        '
        'lblType
        '
        Me.lblType.BackColor = System.Drawing.SystemColors.Control
        Me.lblType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblType.Location = New System.Drawing.Point(16, 27)
        Me.lblType.Name = "lblType"
        Me.lblType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblType.Size = New System.Drawing.Size(89, 17)
        Me.lblType.TabIndex = 5
        Me.lblType.Text = "Type:"
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(420, 309)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Short Period Rate"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._fraUserDefined_0.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
	Sub InitializefraUserDefined()
		Me.fraUserDefined(0) = _fraUserDefined_0
	End Sub
#End Region 
End Class