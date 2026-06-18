<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmService
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
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents lblRequirement As System.Windows.Forms.Label
	Public WithEvents lblContact As System.Windows.Forms.Label
	Public WithEvents lblDesc As System.Windows.Forms.Label
	Public WithEvents lblDateCrit As System.Windows.Forms.Label
	Public WithEvents lblDateRecv As System.Windows.Forms.Label
	Public WithEvents lblDateReq As System.Windows.Forms.Label
	Public WithEvents cmdParty As System.Windows.Forms.Button
	Public WithEvents txtRequirement As System.Windows.Forms.TextBox
	Public WithEvents txtReference As System.Windows.Forms.TextBox
	Public WithEvents txtContact As System.Windows.Forms.TextBox
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents txtDateCritical As System.Windows.Forms.TextBox
	Public WithEvents txtDateReceived As System.Windows.Forms.TextBox
	Public WithEvents txtDateRequested As System.Windows.Forms.TextBox
	Public WithEvents txtTimeRequested As System.Windows.Forms.TextBox
	Public WithEvents txtTimeReceived As System.Windows.Forms.TextBox
	Public WithEvents txtTimeCritical As System.Windows.Forms.TextBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblRequirement = New System.Windows.Forms.Label
        Me.lblContact = New System.Windows.Forms.Label
        Me.lblDesc = New System.Windows.Forms.Label
        Me.lblDateCrit = New System.Windows.Forms.Label
        Me.lblDateRecv = New System.Windows.Forms.Label
        Me.lblDateReq = New System.Windows.Forms.Label
        Me.cmdParty = New System.Windows.Forms.Button
        Me.txtRequirement = New System.Windows.Forms.TextBox
        Me.txtReference = New System.Windows.Forms.TextBox
        Me.txtContact = New System.Windows.Forms.TextBox
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.txtDateCritical = New System.Windows.Forms.TextBox
        Me.txtDateReceived = New System.Windows.Forms.TextBox
        Me.txtDateRequested = New System.Windows.Forms.TextBox
        Me.txtTimeRequested = New System.Windows.Forms.TextBox
        Me.txtTimeReceived = New System.Windows.Forms.TextBox
        Me.txtTimeCritical = New System.Windows.Forms.TextBox
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(420, 392)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 10
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(497, 392)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 11
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(112, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(569, 381)
        Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.tabMainTab.TabIndex = 13
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblRequirement)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblContact)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDesc)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDateCrit)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDateRecv)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDateReq)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdParty)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtRequirement)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtReference)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtContact)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDateCritical)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDateReceived)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDateRequested)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtTimeRequested)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtTimeReceived)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtTimeCritical)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(561, 355)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "Tab 0"
        '
        'lblRequirement
        '
        Me.lblRequirement.AutoSize = True
        Me.lblRequirement.BackColor = System.Drawing.SystemColors.Control
        Me.lblRequirement.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRequirement.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRequirement.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRequirement.Location = New System.Drawing.Point(12, 19)
        Me.lblRequirement.Name = "lblRequirement"
        Me.lblRequirement.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRequirement.Size = New System.Drawing.Size(0, 13)
        Me.lblRequirement.TabIndex = 14
        '
        'lblContact
        '
        Me.lblContact.AutoSize = True
        Me.lblContact.BackColor = System.Drawing.SystemColors.Control
        Me.lblContact.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblContact.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblContact.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblContact.Location = New System.Drawing.Point(12, 147)
        Me.lblContact.Name = "lblContact"
        Me.lblContact.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblContact.Size = New System.Drawing.Size(0, 13)
        Me.lblContact.TabIndex = 15
        '
        'lblDesc
        '
        Me.lblDesc.AutoSize = True
        Me.lblDesc.BackColor = System.Drawing.SystemColors.Control
        Me.lblDesc.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDesc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDesc.Location = New System.Drawing.Point(12, 239)
        Me.lblDesc.Name = "lblDesc"
        Me.lblDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDesc.Size = New System.Drawing.Size(0, 13)
        Me.lblDesc.TabIndex = 16
        '
        'lblDateCrit
        '
        Me.lblDateCrit.AutoSize = True
        Me.lblDateCrit.BackColor = System.Drawing.SystemColors.Control
        Me.lblDateCrit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDateCrit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateCrit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDateCrit.Location = New System.Drawing.Point(12, 115)
        Me.lblDateCrit.Name = "lblDateCrit"
        Me.lblDateCrit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDateCrit.Size = New System.Drawing.Size(0, 13)
        Me.lblDateCrit.TabIndex = 17
        '
        'lblDateRecv
        '
        Me.lblDateRecv.AutoSize = True
        Me.lblDateRecv.BackColor = System.Drawing.SystemColors.Control
        Me.lblDateRecv.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDateRecv.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateRecv.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDateRecv.Location = New System.Drawing.Point(304, 83)
        Me.lblDateRecv.Name = "lblDateRecv"
        Me.lblDateRecv.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDateRecv.Size = New System.Drawing.Size(0, 13)
        Me.lblDateRecv.TabIndex = 18
        '
        'lblDateReq
        '
        Me.lblDateReq.AutoSize = True
        Me.lblDateReq.BackColor = System.Drawing.SystemColors.Control
        Me.lblDateReq.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDateReq.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateReq.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDateReq.Location = New System.Drawing.Point(12, 83)
        Me.lblDateReq.Name = "lblDateReq"
        Me.lblDateReq.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDateReq.Size = New System.Drawing.Size(0, 13)
        Me.lblDateReq.TabIndex = 19
        '
        'cmdParty
        '
        Me.cmdParty.BackColor = System.Drawing.SystemColors.Control
        Me.cmdParty.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdParty.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdParty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdParty.Location = New System.Drawing.Point(12, 48)
        Me.cmdParty.Name = "cmdParty"
        Me.cmdParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdParty.Size = New System.Drawing.Size(61, 22)
        Me.cmdParty.TabIndex = 1
        Me.cmdParty.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdParty.UseVisualStyleBackColor = False
        '
        'txtRequirement
        '
        Me.txtRequirement.AcceptsReturn = True
        Me.txtRequirement.BackColor = System.Drawing.SystemColors.Window
        Me.txtRequirement.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRequirement.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRequirement.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRequirement.Location = New System.Drawing.Point(126, 16)
        Me.txtRequirement.MaxLength = 0
        Me.txtRequirement.Name = "txtRequirement"
        Me.txtRequirement.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRequirement.Size = New System.Drawing.Size(427, 20)
        Me.txtRequirement.TabIndex = 0
        '
        'txtReference
        '
        Me.txtReference.AcceptsReturn = True
        Me.txtReference.BackColor = System.Drawing.SystemColors.Menu
        Me.txtReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtReference.Location = New System.Drawing.Point(126, 48)
        Me.txtReference.MaxLength = 0
        Me.txtReference.Name = "txtReference"
        Me.txtReference.ReadOnly = True
        Me.txtReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtReference.Size = New System.Drawing.Size(427, 20)
        Me.txtReference.TabIndex = 12
        Me.txtReference.TabStop = False
        '
        'txtContact
        '
        Me.txtContact.AcceptsReturn = True
        Me.txtContact.BackColor = System.Drawing.SystemColors.Window
        Me.txtContact.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtContact.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtContact.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtContact.Location = New System.Drawing.Point(126, 144)
        Me.txtContact.MaxLength = 255
        Me.txtContact.Multiline = True
        Me.txtContact.Name = "txtContact"
        Me.txtContact.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtContact.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtContact.Size = New System.Drawing.Size(427, 91)
        Me.txtContact.TabIndex = 8
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(126, 248)
        Me.txtDescription.MaxLength = 255
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDescription.Size = New System.Drawing.Size(427, 93)
        Me.txtDescription.TabIndex = 9
        '
        'txtDateCritical
        '
        Me.txtDateCritical.AcceptsReturn = True
        Me.txtDateCritical.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateCritical.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDateCritical.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDateCritical.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateCritical.Location = New System.Drawing.Point(126, 108)
        Me.txtDateCritical.MaxLength = 0
        Me.txtDateCritical.Name = "txtDateCritical"
        Me.txtDateCritical.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateCritical.Size = New System.Drawing.Size(119, 20)
        Me.txtDateCritical.TabIndex = 6
        '
        'txtDateReceived
        '
        Me.txtDateReceived.AcceptsReturn = True
        Me.txtDateReceived.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateReceived.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDateReceived.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDateReceived.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateReceived.Location = New System.Drawing.Point(392, 80)
        Me.txtDateReceived.MaxLength = 0
        Me.txtDateReceived.Name = "txtDateReceived"
        Me.txtDateReceived.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateReceived.Size = New System.Drawing.Size(115, 20)
        Me.txtDateReceived.TabIndex = 4
        '
        'txtDateRequested
        '
        Me.txtDateRequested.AcceptsReturn = True
        Me.txtDateRequested.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateRequested.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDateRequested.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDateRequested.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateRequested.Location = New System.Drawing.Point(126, 80)
        Me.txtDateRequested.MaxLength = 0
        Me.txtDateRequested.Name = "txtDateRequested"
        Me.txtDateRequested.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateRequested.Size = New System.Drawing.Size(119, 20)
        Me.txtDateRequested.TabIndex = 2
        '
        'txtTimeRequested
        '
        Me.txtTimeRequested.AcceptsReturn = True
        Me.txtTimeRequested.BackColor = System.Drawing.SystemColors.Window
        Me.txtTimeRequested.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTimeRequested.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTimeRequested.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTimeRequested.Location = New System.Drawing.Point(248, 80)
        Me.txtTimeRequested.MaxLength = 0
        Me.txtTimeRequested.Name = "txtTimeRequested"
        Me.txtTimeRequested.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTimeRequested.Size = New System.Drawing.Size(43, 20)
        Me.txtTimeRequested.TabIndex = 3
        '
        'txtTimeReceived
        '
        Me.txtTimeReceived.AcceptsReturn = True
        Me.txtTimeReceived.BackColor = System.Drawing.SystemColors.Window
        Me.txtTimeReceived.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTimeReceived.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTimeReceived.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTimeReceived.Location = New System.Drawing.Point(510, 80)
        Me.txtTimeReceived.MaxLength = 0
        Me.txtTimeReceived.Name = "txtTimeReceived"
        Me.txtTimeReceived.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTimeReceived.Size = New System.Drawing.Size(43, 20)
        Me.txtTimeReceived.TabIndex = 5
        '
        'txtTimeCritical
        '
        Me.txtTimeCritical.AcceptsReturn = True
        Me.txtTimeCritical.BackColor = System.Drawing.SystemColors.Window
        Me.txtTimeCritical.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTimeCritical.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTimeCritical.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTimeCritical.Location = New System.Drawing.Point(248, 108)
        Me.txtTimeCritical.MaxLength = 0
        Me.txtTimeCritical.Name = "txtTimeCritical"
        Me.txtTimeCritical.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTimeCritical.Size = New System.Drawing.Size(43, 20)
        Me.txtTimeCritical.TabIndex = 7
        '
        'frmService
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(579, 418)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmService"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Add Service"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class