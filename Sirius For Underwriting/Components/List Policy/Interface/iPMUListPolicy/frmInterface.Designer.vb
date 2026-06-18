<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
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
	Public WithEvents uctListPolicyControl As uctListPolicyControl.uctListPolicy
	Public WithEvents txtMTADate As System.Windows.Forms.TextBox
	Public WithEvents chkPermanentMTA As System.Windows.Forms.CheckBox
	Public WithEvents lblMTADate As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Private WithEvents _tabMTAtab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMTAtab As System.Windows.Forms.TabControl
	Public WithEvents cmdNew As System.Windows.Forms.Button
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.uctListPolicyControl = New uctListPolicyControl.uctListPolicy
        Me.tabMTAtab = New System.Windows.Forms.TabControl
        Me._tabMTAtab_TabPage0 = New System.Windows.Forms.TabPage
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.txtMTADate = New System.Windows.Forms.TextBox
        Me.chkPermanentMTA = New System.Windows.Forms.CheckBox
        Me.lblMTADate = New System.Windows.Forms.Label
        Me.cmdNew = New System.Windows.Forms.Button
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMTAtab.SuspendLayout()
        Me._tabMTAtab_TabPage0.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me.SuspendLayout()
        '
        'uctListPolicyControl
        '
        Me.uctListPolicyControl.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctListPolicyControl.InsHolderCnt = 0
        Me.uctListPolicyControl.InsReference = ""
        Me.uctListPolicyControl.Location = New System.Drawing.Point(8, 8)
        Me.uctListPolicyControl.Name = "uctListPolicyControl"
        Me.uctListPolicyControl.NewInsuranceFileCnt = 0
        Me.uctListPolicyControl.ShortName = ""
        Me.uctListPolicyControl.Size = New System.Drawing.Size(585, 337)
        Me.uctListPolicyControl.Status = 0
        Me.uctListPolicyControl.TabIndex = 9
        Me.uctListPolicyControl.TargetLongName = ""
        Me.uctListPolicyControl.TargetPartyType = ""
        Me.uctListPolicyControl.TargetResolvedName = ""
        Me.uctListPolicyControl.TargetShortName = ""
        Me.uctListPolicyControl.Task = 0
        '
        'tabMTAtab
        '
        Me.tabMTAtab.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tabMTAtab.Controls.Add(Me._tabMTAtab_TabPage0)
        Me.tabMTAtab.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMTAtab.ItemSize = New System.Drawing.Size(191, 18)
        Me.tabMTAtab.Location = New System.Drawing.Point(8, 675)
        Me.tabMTAtab.Multiline = True
        Me.tabMTAtab.Name = "tabMTAtab"
        Me.tabMTAtab.SelectedIndex = 0
        Me.tabMTAtab.Size = New System.Drawing.Size(581, 341)
        Me.tabMTAtab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.tabMTAtab.TabIndex = 4
        Me.tabMTAtab.Visible = False
        '
        '_tabMTAtab_TabPage0
        '
        Me._tabMTAtab_TabPage0.Controls.Add(Me.Frame1)
        Me._tabMTAtab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMTAtab_TabPage0.Name = "_tabMTAtab_TabPage0"
        Me._tabMTAtab_TabPage0.Size = New System.Drawing.Size(573, 315)
        Me._tabMTAtab_TabPage0.TabIndex = 0
        Me._tabMTAtab_TabPage0.Text = "&1 - MTA"
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.txtMTADate)
        Me.Frame1.Controls.Add(Me.chkPermanentMTA)
        Me.Frame1.Controls.Add(Me.lblMTADate)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(8, 4)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(561, 305)
        Me.Frame1.TabIndex = 5
        Me.Frame1.TabStop = False
        '
        'txtMTADate
        '
        Me.txtMTADate.AcceptsReturn = True
        Me.txtMTADate.BackColor = System.Drawing.SystemColors.Window
        Me.txtMTADate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMTADate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMTADate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMTADate.Location = New System.Drawing.Point(136, 40)
        Me.txtMTADate.MaxLength = 0
        Me.txtMTADate.Name = "txtMTADate"
        Me.txtMTADate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMTADate.Size = New System.Drawing.Size(177, 20)
        Me.txtMTADate.TabIndex = 7
        '
        'chkPermanentMTA
        '
        Me.chkPermanentMTA.BackColor = System.Drawing.SystemColors.Control
        Me.chkPermanentMTA.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkPermanentMTA.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPermanentMTA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPermanentMTA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPermanentMTA.Location = New System.Drawing.Point(16, 80)
        Me.chkPermanentMTA.Name = "chkPermanentMTA"
        Me.chkPermanentMTA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPermanentMTA.Size = New System.Drawing.Size(129, 17)
        Me.chkPermanentMTA.TabIndex = 6
        Me.chkPermanentMTA.Text = "Permanent MTA?"
        Me.chkPermanentMTA.UseVisualStyleBackColor = False
        '
        'lblMTADate
        '
        Me.lblMTADate.BackColor = System.Drawing.SystemColors.Control
        Me.lblMTADate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMTADate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMTADate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMTADate.Location = New System.Drawing.Point(16, 43)
        Me.lblMTADate.Name = "lblMTADate"
        Me.lblMTADate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMTADate.Size = New System.Drawing.Size(97, 17)
        Me.lblMTADate.TabIndex = 8
        Me.lblMTADate.Text = "MTA date:"
        '
        'cmdNew
        '
        Me.cmdNew.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNew.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNew.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNew.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNew.Location = New System.Drawing.Point(8, 360)
        Me.cmdNew.Name = "cmdNew"
        Me.cmdNew.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNew.Size = New System.Drawing.Size(73, 22)
        Me.cmdNew.TabIndex = 3
        Me.cmdNew.Text = "&New"
        Me.cmdNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNew.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(512, 360)
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
        Me.cmdCancel.Location = New System.Drawing.Point(424, 360)
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
        Me.cmdOK.Location = New System.Drawing.Point(336, 360)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(607, 386)
        Me.Controls.Add(Me.uctListPolicyControl)
        Me.Controls.Add(Me.tabMTAtab)
        Me.Controls.Add(Me.cmdNew)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Policy"
        Me.tabMTAtab.ResumeLayout(False)
        Me._tabMTAtab_TabPage0.ResumeLayout(False)
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class