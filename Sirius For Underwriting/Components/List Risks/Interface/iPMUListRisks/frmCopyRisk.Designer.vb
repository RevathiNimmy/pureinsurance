<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCopyRisk
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializeoptCopyType()
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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Private WithEvents _optCopyType_1 As System.Windows.Forms.RadioButton
	Private WithEvents _optCopyType_0 As System.Windows.Forms.RadioButton
	Public WithEvents lblDupeQuote As System.Windows.Forms.Label
	Public WithEvents lblCompQuote As System.Windows.Forms.Label
	Public WithEvents fraType As System.Windows.Forms.GroupBox
	Public optCopyType(1) As System.Windows.Forms.RadioButton
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCopyRisk))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.fraType = New System.Windows.Forms.GroupBox
		Me._optCopyType_1 = New System.Windows.Forms.RadioButton
		Me._optCopyType_0 = New System.Windows.Forms.RadioButton
		Me.lblDupeQuote = New System.Windows.Forms.Label
		Me.lblCompQuote = New System.Windows.Forms.Label
		Me.fraType.SuspendLayout()
		Me.SuspendLayout()
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(254, 196)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(71, 25)
		Me.cmdCancel.TabIndex = 2
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(178, 196)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(71, 25)
		Me.cmdOK.TabIndex = 1
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' fraType
		' 
		Me.fraType.BackColor = System.Drawing.SystemColors.Control
		Me.fraType.Controls.Add(Me._optCopyType_1)
		Me.fraType.Controls.Add(Me._optCopyType_0)
		Me.fraType.Controls.Add(Me.lblDupeQuote)
		Me.fraType.Controls.Add(Me.lblCompQuote)
		Me.fraType.Enabled = True
		Me.fraType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraType.Location = New System.Drawing.Point(6, 8)
		Me.fraType.Name = "fraType"
		Me.fraType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraType.Size = New System.Drawing.Size(319, 183)
		Me.fraType.TabIndex = 0
		Me.fraType.Text = "Type of copy to be made"
		Me.fraType.Visible = True
		' 
		' _optCopyType_1
		' 
		Me._optCopyType_1.Appearance = System.Windows.Forms.Appearance.Normal
		Me._optCopyType_1.BackColor = System.Drawing.SystemColors.Control
		Me._optCopyType_1.CausesValidation = True
		Me._optCopyType_1.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optCopyType_1.Checked = False
		Me._optCopyType_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._optCopyType_1.Enabled = True
		Me._optCopyType_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._optCopyType_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._optCopyType_1.Location = New System.Drawing.Point(16, 88)
		Me._optCopyType_1.Name = "_optCopyType_1"
		Me._optCopyType_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optCopyType_1.Size = New System.Drawing.Size(241, 23)
		Me._optCopyType_1.TabIndex = 4
		Me._optCopyType_1.TabStop = True
		Me._optCopyType_1.Text = "&Duplicate Risk"
		Me._optCopyType_1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optCopyType_1.Visible = True
		' 
		' _optCopyType_0
		' 
		Me._optCopyType_0.Appearance = System.Windows.Forms.Appearance.Normal
		Me._optCopyType_0.BackColor = System.Drawing.SystemColors.Control
		Me._optCopyType_0.CausesValidation = True
		Me._optCopyType_0.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optCopyType_0.Checked = False
		Me._optCopyType_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._optCopyType_0.Enabled = True
		Me._optCopyType_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._optCopyType_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me._optCopyType_0.Location = New System.Drawing.Point(16, 24)
		Me._optCopyType_0.Name = "_optCopyType_0"
		Me._optCopyType_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optCopyType_0.Size = New System.Drawing.Size(241, 23)
		Me._optCopyType_0.TabIndex = 3
		Me._optCopyType_0.TabStop = True
		Me._optCopyType_0.Text = "Comparative &Risk"
		Me._optCopyType_0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optCopyType_0.Visible = True
		' 
		' lblDupeQuote
		' 
		Me.lblDupeQuote.AutoSize = False
		Me.lblDupeQuote.BackColor = System.Drawing.SystemColors.Control
		Me.lblDupeQuote.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDupeQuote.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDupeQuote.Enabled = True
		Me.lblDupeQuote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDupeQuote.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDupeQuote.Location = New System.Drawing.Point(34, 114)
		Me.lblDupeQuote.Name = "lblDupeQuote"
		Me.lblDupeQuote.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDupeQuote.Size = New System.Drawing.Size(275, 29)
		Me.lblDupeQuote.TabIndex = 6
		Me.lblDupeQuote.Text = "Complete copy of the existing Risk. Any number of duplicate Risks can be made live."
		Me.lblDupeQuote.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDupeQuote.UseMnemonic = True
		Me.lblDupeQuote.Visible = True
		' 
		' lblCompQuote
		' 
		Me.lblCompQuote.AutoSize = False
		Me.lblCompQuote.BackColor = System.Drawing.SystemColors.Control
		Me.lblCompQuote.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCompQuote.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCompQuote.Enabled = True
		Me.lblCompQuote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCompQuote.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCompQuote.Location = New System.Drawing.Point(34, 48)
		Me.lblCompQuote.Name = "lblCompQuote"
		Me.lblCompQuote.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCompQuote.Size = New System.Drawing.Size(257, 29)
		Me.lblCompQuote.TabIndex = 5
		Me.lblCompQuote.Text = "New variance of the existing Risk. Only one version of the copy may be made live."
		Me.lblCompQuote.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCompQuote.UseMnemonic = True
		Me.lblCompQuote.Visible = True
		' 
		' frmCopyRisk
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(333, 227)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.fraType)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 29)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmCopyRisk"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "Copy Risk"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.fraType.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub InitializeoptCopyType()
		Me.optCopyType(1) = _optCopyType_1
		Me.optCopyType(0) = _optCopyType_0
	End Sub
#End Region 
End Class