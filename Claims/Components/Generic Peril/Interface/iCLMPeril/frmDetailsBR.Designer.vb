<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDetailsBR
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
	Public WithEvents txtTempCurrent As System.Windows.Forms.TextBox
	Public WithEvents txtTempRevised As System.Windows.Forms.TextBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents txtCurrentReserve As System.Windows.Forms.TextBox
	Public WithEvents txtThisPayment As System.Windows.Forms.TextBox
	Public WithEvents Combo1 As System.Windows.Forms.ComboBox
	Public WithEvents txtinitialreserve As System.Windows.Forms.TextBox
	Public WithEvents txtrevisedreserve As System.Windows.Forms.TextBox
	Public WithEvents lblCurrentReserve As System.Windows.Forms.Label
	Public WithEvents lblThisPayment As System.Windows.Forms.Label
	Public WithEvents lblInitialReserve As System.Windows.Forms.Label
	Public WithEvents lblRevisedReserve As System.Windows.Forms.Label
	Public WithEvents lblRiskType As System.Windows.Forms.Label
	Public WithEvents fraReserveDetails As System.Windows.Forms.GroupBox
	Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents SSTab1 As System.Windows.Forms.TabControl
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtTempCurrent = New System.Windows.Forms.TextBox
        Me.txtTempRevised = New System.Windows.Forms.TextBox
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOk = New System.Windows.Forms.Button
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me.fraReserveDetails = New System.Windows.Forms.GroupBox
        Me.txtCurrentReserve = New System.Windows.Forms.TextBox
        Me.txtThisPayment = New System.Windows.Forms.TextBox
        Me.Combo1 = New System.Windows.Forms.ComboBox
        Me.txtinitialreserve = New System.Windows.Forms.TextBox
        Me.txtrevisedreserve = New System.Windows.Forms.TextBox
        Me.lblCurrentReserve = New System.Windows.Forms.Label
        Me.lblThisPayment = New System.Windows.Forms.Label
        Me.lblInitialReserve = New System.Windows.Forms.Label
        Me.lblRevisedReserve = New System.Windows.Forms.Label
        Me.lblRiskType = New System.Windows.Forms.Label
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me.fraReserveDetails.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtTempCurrent
        '
        Me.txtTempCurrent.AcceptsReturn = True
        Me.txtTempCurrent.BackColor = System.Drawing.SystemColors.Window
        Me.txtTempCurrent.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTempCurrent.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTempCurrent.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTempCurrent.Location = New System.Drawing.Point(8, 240)
        Me.txtTempCurrent.MaxLength = 0
        Me.txtTempCurrent.Name = "txtTempCurrent"
        Me.txtTempCurrent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTempCurrent.Size = New System.Drawing.Size(169, 19)
        Me.txtTempCurrent.TabIndex = 15
        Me.txtTempCurrent.Visible = False
        '
        'txtTempRevised
        '
        Me.txtTempRevised.AcceptsReturn = True
        Me.txtTempRevised.BackColor = System.Drawing.SystemColors.Window
        Me.txtTempRevised.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTempRevised.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTempRevised.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTempRevised.Location = New System.Drawing.Point(8, 224)
        Me.txtTempRevised.MaxLength = 0
        Me.txtTempRevised.Name = "txtTempRevised"
        Me.txtTempRevised.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTempRevised.Size = New System.Drawing.Size(169, 19)
        Me.txtTempRevised.TabIndex = 14
        Me.txtTempRevised.Visible = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(256, 232)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 8
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOk
        '
        Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOk.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOk.Location = New System.Drawing.Point(176, 232)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(73, 22)
        Me.cmdOk.TabIndex = 7
        Me.cmdOk.Text = "&OK"
        Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOk.UseVisualStyleBackColor = False
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(106, 18)
        Me.SSTab1.Location = New System.Drawing.Point(8, 8)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(325, 221)
        Me.SSTab1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.SSTab1.TabIndex = 0
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.fraReserveDetails)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(317, 195)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "Reserve Details"
        '
        'fraReserveDetails
        '
        Me.fraReserveDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraReserveDetails.Controls.Add(Me.txtCurrentReserve)
        Me.fraReserveDetails.Controls.Add(Me.txtThisPayment)
        Me.fraReserveDetails.Controls.Add(Me.Combo1)
        Me.fraReserveDetails.Controls.Add(Me.txtinitialreserve)
        Me.fraReserveDetails.Controls.Add(Me.txtrevisedreserve)
        Me.fraReserveDetails.Controls.Add(Me.lblCurrentReserve)
        Me.fraReserveDetails.Controls.Add(Me.lblThisPayment)
        Me.fraReserveDetails.Controls.Add(Me.lblInitialReserve)
        Me.fraReserveDetails.Controls.Add(Me.lblRevisedReserve)
        Me.fraReserveDetails.Controls.Add(Me.lblRiskType)
        Me.fraReserveDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraReserveDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraReserveDetails.Location = New System.Drawing.Point(8, 4)
        Me.fraReserveDetails.Name = "fraReserveDetails"
        Me.fraReserveDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraReserveDetails.Size = New System.Drawing.Size(305, 185)
        Me.fraReserveDetails.TabIndex = 1
        Me.fraReserveDetails.TabStop = False
        '
        'txtCurrentReserve
        '
        Me.txtCurrentReserve.AcceptsReturn = True
        Me.txtCurrentReserve.BackColor = System.Drawing.SystemColors.Window
        Me.txtCurrentReserve.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCurrentReserve.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCurrentReserve.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCurrentReserve.Location = New System.Drawing.Point(128, 120)
        Me.txtCurrentReserve.MaxLength = 0
        Me.txtCurrentReserve.Name = "txtCurrentReserve"
        Me.txtCurrentReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCurrentReserve.Size = New System.Drawing.Size(169, 19)
        Me.txtCurrentReserve.TabIndex = 13
        '
        'txtThisPayment
        '
        Me.txtThisPayment.AcceptsReturn = True
        Me.txtThisPayment.BackColor = System.Drawing.SystemColors.Window
        Me.txtThisPayment.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtThisPayment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThisPayment.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtThisPayment.Location = New System.Drawing.Point(128, 152)
        Me.txtThisPayment.MaxLength = 0
        Me.txtThisPayment.Name = "txtThisPayment"
        Me.txtThisPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtThisPayment.Size = New System.Drawing.Size(169, 19)
        Me.txtThisPayment.TabIndex = 10
        '
        'Combo1
        '
        Me.Combo1.BackColor = System.Drawing.SystemColors.Window
        Me.Combo1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Combo1.Enabled = False
        Me.Combo1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Combo1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Combo1.Location = New System.Drawing.Point(128, 24)
        Me.Combo1.Name = "Combo1"
        Me.Combo1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Combo1.Size = New System.Drawing.Size(169, 21)
        Me.Combo1.TabIndex = 9
        Me.Combo1.Text = "Combo1"
        '
        'txtinitialreserve
        '
        Me.txtinitialreserve.AcceptsReturn = True
        Me.txtinitialreserve.BackColor = System.Drawing.SystemColors.Window
        Me.txtinitialreserve.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtinitialreserve.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtinitialreserve.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtinitialreserve.Location = New System.Drawing.Point(128, 56)
        Me.txtinitialreserve.MaxLength = 0
        Me.txtinitialreserve.Name = "txtinitialreserve"
        Me.txtinitialreserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtinitialreserve.Size = New System.Drawing.Size(169, 19)
        Me.txtinitialreserve.TabIndex = 5
        '
        'txtrevisedreserve
        '
        Me.txtrevisedreserve.AcceptsReturn = True
        Me.txtrevisedreserve.BackColor = System.Drawing.SystemColors.Window
        Me.txtrevisedreserve.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtrevisedreserve.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtrevisedreserve.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtrevisedreserve.Location = New System.Drawing.Point(128, 88)
        Me.txtrevisedreserve.MaxLength = 0
        Me.txtrevisedreserve.Name = "txtrevisedreserve"
        Me.txtrevisedreserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtrevisedreserve.Size = New System.Drawing.Size(169, 19)
        Me.txtrevisedreserve.TabIndex = 3
        '
        'lblCurrentReserve
        '
        Me.lblCurrentReserve.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrentReserve.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrentReserve.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentReserve.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrentReserve.Location = New System.Drawing.Point(8, 120)
        Me.lblCurrentReserve.Name = "lblCurrentReserve"
        Me.lblCurrentReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrentReserve.Size = New System.Drawing.Size(113, 17)
        Me.lblCurrentReserve.TabIndex = 12
        Me.lblCurrentReserve.Text = "Current Reserve :"
        '
        'lblThisPayment
        '
        Me.lblThisPayment.BackColor = System.Drawing.SystemColors.Control
        Me.lblThisPayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblThisPayment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblThisPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblThisPayment.Location = New System.Drawing.Point(8, 152)
        Me.lblThisPayment.Name = "lblThisPayment"
        Me.lblThisPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblThisPayment.Size = New System.Drawing.Size(113, 17)
        Me.lblThisPayment.TabIndex = 11
        Me.lblThisPayment.Text = "This Payment :"
        '
        'lblInitialReserve
        '
        Me.lblInitialReserve.BackColor = System.Drawing.SystemColors.Control
        Me.lblInitialReserve.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInitialReserve.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInitialReserve.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInitialReserve.Location = New System.Drawing.Point(8, 56)
        Me.lblInitialReserve.Name = "lblInitialReserve"
        Me.lblInitialReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInitialReserve.Size = New System.Drawing.Size(105, 17)
        Me.lblInitialReserve.TabIndex = 6
        Me.lblInitialReserve.Text = "Initial Reserve :"
        '
        'lblRevisedReserve
        '
        Me.lblRevisedReserve.BackColor = System.Drawing.SystemColors.Control
        Me.lblRevisedReserve.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRevisedReserve.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRevisedReserve.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRevisedReserve.Location = New System.Drawing.Point(8, 88)
        Me.lblRevisedReserve.Name = "lblRevisedReserve"
        Me.lblRevisedReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRevisedReserve.Size = New System.Drawing.Size(145, 17)
        Me.lblRevisedReserve.TabIndex = 4
        Me.lblRevisedReserve.Text = "Revised Reserve :"
        '
        'lblRiskType
        '
        Me.lblRiskType.BackColor = System.Drawing.SystemColors.Control
        Me.lblRiskType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRiskType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRiskType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRiskType.Location = New System.Drawing.Point(8, 24)
        Me.lblRiskType.Name = "lblRiskType"
        Me.lblRiskType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRiskType.Size = New System.Drawing.Size(89, 17)
        Me.lblRiskType.TabIndex = 2
        Me.lblRiskType.Text = "Risk Type :"
        '
        'frmDetailsBR
        '
        Me.AcceptButton = Me.cmdOk
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(336, 257)
        Me.Controls.Add(Me.txtTempCurrent)
        Me.Controls.Add(Me.txtTempRevised)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.SSTab1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDetailsBR"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Reserve Details Screen"
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me.fraReserveDetails.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class