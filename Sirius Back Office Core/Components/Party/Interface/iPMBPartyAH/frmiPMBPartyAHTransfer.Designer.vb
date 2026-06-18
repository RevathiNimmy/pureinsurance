<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTransfer
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents lblHandler As System.Windows.Forms.Label
	Public WithEvents lblExecutive As System.Windows.Forms.Label
	Public WithEvents pnlAccountHandler As System.Windows.Forms.Label
	Public WithEvents pnlAccountExecutive As System.Windows.Forms.Label
	Public WithEvents cmdLookUpAccountHandler As System.Windows.Forms.Button
	Public WithEvents cmdLookupAccountExecutive As System.Windows.Forms.Button
	Private WithEvents _tab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tab As System.Windows.Forms.TabControl
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
        Me.tab = New System.Windows.Forms.TabControl
        Me._tab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblHandler = New System.Windows.Forms.Label
        Me.lblExecutive = New System.Windows.Forms.Label
        Me.pnlAccountHandler = New System.Windows.Forms.Label
        Me.pnlAccountExecutive = New System.Windows.Forms.Label
        Me.cmdLookUpAccountHandler = New System.Windows.Forms.Button
        Me.cmdLookupAccountExecutive = New System.Windows.Forms.Button
        Me.tab.SuspendLayout()
        Me._tab_TabPage0.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(426, 122)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 5
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
        Me.cmdCancel.Location = New System.Drawing.Point(348, 122)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 4
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
        Me.cmdOK.Location = New System.Drawing.Point(270, 122)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 3
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tab
        '
        Me.tab.Controls.Add(Me._tab_TabPage0)
        Me.tab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tab.ItemSize = New System.Drawing.Size(492, 18)
        Me.tab.Location = New System.Drawing.Point(6, 6)
        Me.tab.Multiline = True
        Me.tab.Name = "tab"
        Me.tab.SelectedIndex = 0
        Me.tab.Size = New System.Drawing.Size(497, 115)
        Me.tab.TabIndex = 0
        '
        '_tab_TabPage0
        '
        Me._tab_TabPage0.Controls.Add(Me.lblHandler)
        Me._tab_TabPage0.Controls.Add(Me.lblExecutive)
        Me._tab_TabPage0.Controls.Add(Me.pnlAccountHandler)
        Me._tab_TabPage0.Controls.Add(Me.pnlAccountExecutive)
        Me._tab_TabPage0.Controls.Add(Me.cmdLookUpAccountHandler)
        Me._tab_TabPage0.Controls.Add(Me.cmdLookupAccountExecutive)
        Me._tab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tab_TabPage0.Name = "_tab_TabPage0"
        Me._tab_TabPage0.Size = New System.Drawing.Size(489, 89)
        Me._tab_TabPage0.TabIndex = 0
        Me._tab_TabPage0.Text = "Transfer Details"
        '
        'lblHandler
        '
        Me.lblHandler.BackColor = System.Drawing.SystemColors.Control
        Me.lblHandler.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblHandler.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHandler.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblHandler.Location = New System.Drawing.Point(12, 22)
        Me.lblHandler.Name = "lblHandler"
        Me.lblHandler.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblHandler.Size = New System.Drawing.Size(225, 17)
        Me.lblHandler.TabIndex = 1
        Me.lblHandler.Text = "Transfer Policies to Account Handler :"
        '
        'lblExecutive
        '
        Me.lblExecutive.BackColor = System.Drawing.SystemColors.Control
        Me.lblExecutive.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExecutive.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExecutive.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExecutive.Location = New System.Drawing.Point(12, 54)
        Me.lblExecutive.Name = "lblExecutive"
        Me.lblExecutive.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExecutive.Size = New System.Drawing.Size(226, 19)
        Me.lblExecutive.TabIndex = 2
        Me.lblExecutive.Text = "Transfer Clients to Account Executive:"
        '
        'pnlAccountHandler
        '
        Me.pnlAccountHandler.BackColor = System.Drawing.SystemColors.ActiveBorder
        Me.pnlAccountHandler.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlAccountHandler.Cursor = System.Windows.Forms.Cursors.Default
        Me.pnlAccountHandler.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlAccountHandler.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlAccountHandler.Location = New System.Drawing.Point(244, 18)
        Me.pnlAccountHandler.Name = "pnlAccountHandler"
        Me.pnlAccountHandler.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlAccountHandler.Size = New System.Drawing.Size(199, 21)
        Me.pnlAccountHandler.TabIndex = 8
        '
        'pnlAccountExecutive
        '
        Me.pnlAccountExecutive.BackColor = System.Drawing.SystemColors.Control
        Me.pnlAccountExecutive.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlAccountExecutive.Cursor = System.Windows.Forms.Cursors.Default
        Me.pnlAccountExecutive.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlAccountExecutive.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlAccountExecutive.Location = New System.Drawing.Point(244, 50)
        Me.pnlAccountExecutive.Name = "pnlAccountExecutive"
        Me.pnlAccountExecutive.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlAccountExecutive.Size = New System.Drawing.Size(199, 21)
        Me.pnlAccountExecutive.TabIndex = 9
        '
        'cmdLookUpAccountHandler
        '
        Me.cmdLookUpAccountHandler.BackColor = System.Drawing.SystemColors.Control
        Me.cmdLookUpAccountHandler.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdLookUpAccountHandler.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdLookUpAccountHandler.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdLookUpAccountHandler.Location = New System.Drawing.Point(444, 18)
        Me.cmdLookUpAccountHandler.Name = "cmdLookUpAccountHandler"
        Me.cmdLookUpAccountHandler.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdLookUpAccountHandler.Size = New System.Drawing.Size(33, 22)
        Me.cmdLookUpAccountHandler.TabIndex = 6
        Me.cmdLookUpAccountHandler.Text = "..."
        Me.cmdLookUpAccountHandler.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdLookUpAccountHandler.UseVisualStyleBackColor = False
        '
        'cmdLookupAccountExecutive
        '
        Me.cmdLookupAccountExecutive.BackColor = System.Drawing.SystemColors.Control
        Me.cmdLookupAccountExecutive.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdLookupAccountExecutive.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdLookupAccountExecutive.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdLookupAccountExecutive.Location = New System.Drawing.Point(444, 50)
        Me.cmdLookupAccountExecutive.Name = "cmdLookupAccountExecutive"
        Me.cmdLookupAccountExecutive.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdLookupAccountExecutive.Size = New System.Drawing.Size(33, 22)
        Me.cmdLookupAccountExecutive.TabIndex = 7
        Me.cmdLookupAccountExecutive.Text = "..."
        Me.cmdLookupAccountExecutive.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdLookupAccountExecutive.UseVisualStyleBackColor = False
        '
        'frmTransfer
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(506, 151)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmTransfer"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Transfer Data"
        Me.tab.ResumeLayout(False)
        Me._tab_TabPage0.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class