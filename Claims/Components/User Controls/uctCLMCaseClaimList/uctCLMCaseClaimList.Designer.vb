<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctCLMCaseClaim
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		UserControl_Initialize()
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
    Friend WithEvents cmdLink As System.Windows.Forms.Button
	Friend WithEvents cmdUnlink As System.Windows.Forms.Button
	Friend WithEvents cmdOpen As System.Windows.Forms.Button
	Friend WithEvents cmdMaintain As System.Windows.Forms.Button
	Friend WithEvents cmdPay As System.Windows.Forms.Button
	Friend WithEvents cmdSalvage As System.Windows.Forms.Button
	Friend WithEvents cmdTPRecovery As System.Windows.Forms.Button
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdLink = New System.Windows.Forms.Button
        Me.cmdUnlink = New System.Windows.Forms.Button
        Me.cmdOpen = New System.Windows.Forms.Button
        Me.cmdMaintain = New System.Windows.Forms.Button
        Me.cmdPay = New System.Windows.Forms.Button
        Me.cmdSalvage = New System.Windows.Forms.Button
        Me.cmdTPRecovery = New System.Windows.Forms.Button
        Me.lvwCaseClaimlist = New System.Windows.Forms.ListView
        Me.SuspendLayout()
        '
        'cmdLink
        '
        Me.cmdLink.BackColor = System.Drawing.SystemColors.Control
        Me.cmdLink.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdLink.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdLink.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdLink.Location = New System.Drawing.Point(496, 182)
        Me.cmdLink.Name = "cmdLink"
        Me.cmdLink.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdLink.Size = New System.Drawing.Size(73, 22)
        Me.cmdLink.TabIndex = 6
        Me.cmdLink.Text = "Link"
        Me.cmdLink.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdLink.UseVisualStyleBackColor = False
        '
        'cmdUnlink
        '
        Me.cmdUnlink.BackColor = System.Drawing.SystemColors.Control
        Me.cmdUnlink.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdUnlink.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdUnlink.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdUnlink.Location = New System.Drawing.Point(576, 182)
        Me.cmdUnlink.Name = "cmdUnlink"
        Me.cmdUnlink.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdUnlink.Size = New System.Drawing.Size(73, 22)
        Me.cmdUnlink.TabIndex = 5
        Me.cmdUnlink.Text = "Unlink"
        Me.cmdUnlink.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdUnlink.UseVisualStyleBackColor = False
        '
        'cmdOpen
        '
        Me.cmdOpen.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOpen.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOpen.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOpen.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOpen.Location = New System.Drawing.Point(8, 182)
        Me.cmdOpen.Name = "cmdOpen"
        Me.cmdOpen.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOpen.Size = New System.Drawing.Size(73, 22)
        Me.cmdOpen.TabIndex = 4
        Me.cmdOpen.Text = "Open"
        Me.cmdOpen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOpen.UseVisualStyleBackColor = False
        '
        'cmdMaintain
        '
        Me.cmdMaintain.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMaintain.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMaintain.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMaintain.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMaintain.Location = New System.Drawing.Point(88, 182)
        Me.cmdMaintain.Name = "cmdMaintain"
        Me.cmdMaintain.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMaintain.Size = New System.Drawing.Size(73, 22)
        Me.cmdMaintain.TabIndex = 3
        Me.cmdMaintain.Text = "Maintain"
        Me.cmdMaintain.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdMaintain.UseVisualStyleBackColor = False
        '
        'cmdPay
        '
        Me.cmdPay.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPay.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPay.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPay.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPay.Location = New System.Drawing.Point(168, 182)
        Me.cmdPay.Name = "cmdPay"
        Me.cmdPay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPay.Size = New System.Drawing.Size(73, 22)
        Me.cmdPay.TabIndex = 2
        Me.cmdPay.Text = "Pay"
        Me.cmdPay.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPay.UseVisualStyleBackColor = False
        '
        'cmdSalvage
        '
        Me.cmdSalvage.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSalvage.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSalvage.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSalvage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSalvage.Location = New System.Drawing.Point(248, 182)
        Me.cmdSalvage.Name = "cmdSalvage"
        Me.cmdSalvage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSalvage.Size = New System.Drawing.Size(73, 22)
        Me.cmdSalvage.TabIndex = 1
        Me.cmdSalvage.Text = "Salvage"
        Me.cmdSalvage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSalvage.UseVisualStyleBackColor = False
        '
        'cmdTPRecovery
        '
        Me.cmdTPRecovery.BackColor = System.Drawing.SystemColors.Control
        Me.cmdTPRecovery.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdTPRecovery.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTPRecovery.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdTPRecovery.Location = New System.Drawing.Point(328, 182)
        Me.cmdTPRecovery.Name = "cmdTPRecovery"
        Me.cmdTPRecovery.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdTPRecovery.Size = New System.Drawing.Size(89, 22)
        Me.cmdTPRecovery.TabIndex = 0
        Me.cmdTPRecovery.Text = "TP Recovery"
        Me.cmdTPRecovery.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdTPRecovery.UseVisualStyleBackColor = False
        '
        'lvwCaseClaimlist
        '
        Me.lvwCaseClaimlist.Location = New System.Drawing.Point(8, 8)
        Me.lvwCaseClaimlist.Name = "lvwCaseClaimlist"
        Me.lvwCaseClaimlist.Size = New System.Drawing.Size(641, 169)
        Me.lvwCaseClaimlist.TabIndex = 7
        Me.lvwCaseClaimlist.UseCompatibleStateImageBehavior = False
        Me.lvwCaseClaimlist.View = System.Windows.Forms.View.Details
        '
        'uctCLMCaseClaim
        '
        Me.Controls.Add(Me.lvwCaseClaimlist)
        Me.Controls.Add(Me.cmdLink)
        Me.Controls.Add(Me.cmdUnlink)
        Me.Controls.Add(Me.cmdOpen)
        Me.Controls.Add(Me.cmdMaintain)
        Me.Controls.Add(Me.cmdPay)
        Me.Controls.Add(Me.cmdSalvage)
        Me.Controls.Add(Me.cmdTPRecovery)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctCLMCaseClaim"
        Me.Size = New System.Drawing.Size(659, 210)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lvwCaseClaimlist As System.Windows.Forms.ListView
#End Region 
End Class