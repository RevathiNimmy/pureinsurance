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
	Public WithEvents cmdDefineFields As System.Windows.Forms.Button
	Public WithEvents lvwRiskType As System.Windows.Forms.ListView
	Public WithEvents cmdGISScreen As System.Windows.Forms.Button
	Public WithEvents cmdCloseClaim As System.Windows.Forms.Button
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
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
        Me.cmdDefineFields = New System.Windows.Forms.Button
        Me.lvwRiskType = New System.Windows.Forms.ListView
        Me.cmdGISScreen = New System.Windows.Forms.Button
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.cmdCloseClaim = New System.Windows.Forms.Button
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(440, 263)
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
        Me.cmdCancel.Location = New System.Drawing.Point(360, 263)
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
        Me.cmdOK.Location = New System.Drawing.Point(280, 263)
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
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdDefineFields)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwRiskType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdGISScreen)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Frame1)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(501, 223)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "Tab 0"
        '
        'cmdDefineFields
        '
        Me.cmdDefineFields.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDefineFields.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDefineFields.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDefineFields.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDefineFields.Location = New System.Drawing.Point(411, 174)
        Me.cmdDefineFields.Name = "cmdDefineFields"
        Me.cmdDefineFields.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDefineFields.Size = New System.Drawing.Size(81, 22)
        Me.cmdDefineFields.TabIndex = 3
        Me.cmdDefineFields.Text = "Define &fields"
        Me.cmdDefineFields.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDefineFields.UseVisualStyleBackColor = False
        '
        'lvwRiskType
        '
        Me.lvwRiskType.BackColor = System.Drawing.SystemColors.Window
        Me.lvwRiskType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwRiskType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwRiskType.FullRowSelect = True
        Me.lvwRiskType.Location = New System.Drawing.Point(8, 16)
        Me.lvwRiskType.Name = "lvwRiskType"
        Me.lvwRiskType.Size = New System.Drawing.Size(385, 201)
        Me.lvwRiskType.TabIndex = 0
        Me.lvwRiskType.UseCompatibleStateImageBehavior = False
        Me.lvwRiskType.View = System.Windows.Forms.View.Details
        '
        'cmdGISScreen
        '
        Me.cmdGISScreen.BackColor = System.Drawing.SystemColors.Control
        Me.cmdGISScreen.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdGISScreen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdGISScreen.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdGISScreen.Location = New System.Drawing.Point(411, 144)
        Me.cmdGISScreen.Name = "cmdGISScreen"
        Me.cmdGISScreen.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdGISScreen.Size = New System.Drawing.Size(81, 22)
        Me.cmdGISScreen.TabIndex = 2
        Me.cmdGISScreen.Text = "&GIS Screen"
        Me.cmdGISScreen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdGISScreen.UseVisualStyleBackColor = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.cmdCloseClaim)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(400, 12)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(97, 81)
        Me.Frame1.TabIndex = 1
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Rule Scripts"
        Me.Frame1.Visible = False
        '
        'cmdCloseClaim
        '
        Me.cmdCloseClaim.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCloseClaim.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCloseClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCloseClaim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCloseClaim.Location = New System.Drawing.Point(8, 16)
        Me.cmdCloseClaim.Name = "cmdCloseClaim"
        Me.cmdCloseClaim.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCloseClaim.Size = New System.Drawing.Size(81, 22)
        Me.cmdCloseClaim.TabIndex = 0
        Me.cmdCloseClaim.Text = "Close Claim"
        Me.cmdCloseClaim.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCloseClaim.UseVisualStyleBackColor = False
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
        Me.Frame1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class