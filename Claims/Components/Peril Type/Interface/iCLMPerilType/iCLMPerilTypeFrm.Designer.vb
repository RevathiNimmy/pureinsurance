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
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents CmdShowScreen As System.Windows.Forms.Button
	Public WithEvents lvwPerilType As System.Windows.Forms.ListView
	Public WithEvents cmdDefinefields As System.Windows.Forms.Button
	Public WithEvents CmdReserves As System.Windows.Forms.Button
	Public WithEvents cmdFieldLevel As System.Windows.Forms.Button
	Public WithEvents cmdInitialisation As System.Windows.Forms.Button
	Public WithEvents cmdRowLevel As System.Windows.Forms.Button
	Public WithEvents cmdValidate As System.Windows.Forms.Button
	Public WithEvents cmdPayment As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
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
        Me.CmdShowScreen = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lvwPerilType = New System.Windows.Forms.ListView
        Me.cmdDefinefields = New System.Windows.Forms.Button
        Me.CmdReserves = New System.Windows.Forms.Button
        Me.cmdFieldLevel = New System.Windows.Forms.Button
        Me.cmdInitialisation = New System.Windows.Forms.Button
        Me.cmdRowLevel = New System.Windows.Forms.Button
        Me.cmdValidate = New System.Windows.Forms.Button
        Me.cmdPayment = New System.Windows.Forms.Button
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(442, 263)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 4
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
        Me.cmdCancel.Location = New System.Drawing.Point(362, 263)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 3
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
        Me.cmdOK.Location = New System.Drawing.Point(282, 263)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 2
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'CmdShowScreen
        '
        Me.CmdShowScreen.BackColor = System.Drawing.SystemColors.Control
        Me.CmdShowScreen.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdShowScreen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdShowScreen.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdShowScreen.Location = New System.Drawing.Point(8, 263)
        Me.CmdShowScreen.Name = "CmdShowScreen"
        Me.CmdShowScreen.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdShowScreen.Size = New System.Drawing.Size(87, 22)
        Me.CmdShowScreen.TabIndex = 1
        Me.CmdShowScreen.Text = "&Show screen"
        Me.CmdShowScreen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.CmdShowScreen.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(55, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(509, 249)
        Me.tabMainTab.TabIndex = 0
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwPerilType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdDefinefields)
        Me._tabMainTab_TabPage0.Controls.Add(Me.CmdReserves)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdFieldLevel)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdInitialisation)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdRowLevel)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdValidate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdPayment)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(501, 223)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "Tab 0"
        '
        'lvwPerilType
        '
        Me.lvwPerilType.BackColor = System.Drawing.SystemColors.Window
        Me.lvwPerilType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwPerilType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwPerilType.Location = New System.Drawing.Point(8, 16)
        Me.lvwPerilType.Name = "lvwPerilType"
        Me.lvwPerilType.Size = New System.Drawing.Size(393, 201)
        Me.lvwPerilType.TabIndex = 0
        Me.lvwPerilType.UseCompatibleStateImageBehavior = False
        Me.lvwPerilType.View = System.Windows.Forms.View.Details
        '
        'cmdDefinefields
        '
        Me.cmdDefinefields.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDefinefields.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDefinefields.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDefinefields.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDefinefields.Location = New System.Drawing.Point(416, 195)
        Me.cmdDefinefields.Name = "cmdDefinefields"
        Me.cmdDefinefields.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDefinefields.Size = New System.Drawing.Size(81, 22)
        Me.cmdDefinefields.TabIndex = 7
        Me.cmdDefinefields.Text = "&Define Fields"
        Me.cmdDefinefields.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDefinefields.UseVisualStyleBackColor = False
        '
        'CmdReserves
        '
        Me.CmdReserves.BackColor = System.Drawing.SystemColors.Control
        Me.CmdReserves.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdReserves.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdReserves.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdReserves.Location = New System.Drawing.Point(416, 165)
        Me.CmdReserves.Name = "CmdReserves"
        Me.CmdReserves.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdReserves.Size = New System.Drawing.Size(81, 22)
        Me.CmdReserves.TabIndex = 6
        Me.CmdReserves.Text = "&Reserves"
        Me.CmdReserves.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.CmdReserves.UseVisualStyleBackColor = False
        '
        'cmdFieldLevel
        '
        Me.cmdFieldLevel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFieldLevel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFieldLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFieldLevel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFieldLevel.Location = New System.Drawing.Point(416, 73)
        Me.cmdFieldLevel.Name = "cmdFieldLevel"
        Me.cmdFieldLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFieldLevel.Size = New System.Drawing.Size(81, 22)
        Me.cmdFieldLevel.TabIndex = 3
        Me.cmdFieldLevel.TabStop = False
        Me.cmdFieldLevel.Text = "&Field Level"
        Me.cmdFieldLevel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFieldLevel.UseVisualStyleBackColor = False
        Me.cmdFieldLevel.Visible = False
        '
        'cmdInitialisation
        '
        Me.cmdInitialisation.BackColor = System.Drawing.SystemColors.Control
        Me.cmdInitialisation.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdInitialisation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdInitialisation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdInitialisation.Location = New System.Drawing.Point(416, 12)
        Me.cmdInitialisation.Name = "cmdInitialisation"
        Me.cmdInitialisation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdInitialisation.Size = New System.Drawing.Size(81, 22)
        Me.cmdInitialisation.TabIndex = 1
        Me.cmdInitialisation.TabStop = False
        Me.cmdInitialisation.Text = "&Initialisation"
        Me.cmdInitialisation.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdInitialisation.UseVisualStyleBackColor = False
        Me.cmdInitialisation.Visible = False
        '
        'cmdRowLevel
        '
        Me.cmdRowLevel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRowLevel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRowLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRowLevel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRowLevel.Location = New System.Drawing.Point(416, 104)
        Me.cmdRowLevel.Name = "cmdRowLevel"
        Me.cmdRowLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRowLevel.Size = New System.Drawing.Size(81, 22)
        Me.cmdRowLevel.TabIndex = 4
        Me.cmdRowLevel.TabStop = False
        Me.cmdRowLevel.Text = "R&ow Level"
        Me.cmdRowLevel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRowLevel.UseVisualStyleBackColor = False
        Me.cmdRowLevel.Visible = False
        '
        'cmdValidate
        '
        Me.cmdValidate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdValidate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdValidate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdValidate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdValidate.Location = New System.Drawing.Point(416, 43)
        Me.cmdValidate.Name = "cmdValidate"
        Me.cmdValidate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdValidate.Size = New System.Drawing.Size(81, 22)
        Me.cmdValidate.TabIndex = 2
        Me.cmdValidate.TabStop = False
        Me.cmdValidate.Text = "&Validate"
        Me.cmdValidate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdValidate.UseVisualStyleBackColor = False
        Me.cmdValidate.Visible = False
        '
        'cmdPayment
        '
        Me.cmdPayment.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPayment.Location = New System.Drawing.Point(416, 134)
        Me.cmdPayment.Name = "cmdPayment"
        Me.cmdPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPayment.Size = New System.Drawing.Size(81, 22)
        Me.cmdPayment.TabIndex = 5
        Me.cmdPayment.TabStop = False
        Me.cmdPayment.Text = "&Payment"
        Me.cmdPayment.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPayment.UseVisualStyleBackColor = False
        Me.cmdPayment.Visible = False
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(522, 292)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.CmdShowScreen)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "RiskType"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class