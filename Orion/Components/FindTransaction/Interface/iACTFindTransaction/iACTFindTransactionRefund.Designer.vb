<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRefund
#Region "Windows Form Designer generated code "
	Friend Sub New()
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
	Public WithEvents cboMediaType As System.Windows.Forms.ComboBox
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents txtRefundAmount As System.Windows.Forms.TextBox
	Public WithEvents lblPaymentMethod As System.Windows.Forms.Label
	Public WithEvents lblRefundAmount As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRefund))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cboMediaType = New System.Windows.Forms.ComboBox
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.txtRefundAmount = New System.Windows.Forms.TextBox
		Me.lblPaymentMethod = New System.Windows.Forms.Label
		Me.lblRefundAmount = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' cboMediaType
		' 
		Me.cboMediaType.BackColor = System.Drawing.SystemColors.Window
		Me.cboMediaType.CausesValidation = True
		Me.cboMediaType.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboMediaType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboMediaType.Enabled = True
		Me.cboMediaType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboMediaType.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboMediaType.IntegralHeight = True
		Me.cboMediaType.Location = New System.Drawing.Point(170, 16)
		Me.cboMediaType.Name = "cboMediaType"
		Me.cboMediaType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboMediaType.Size = New System.Drawing.Size(228, 21)
		Me.cboMediaType.Sorted = False
		Me.cboMediaType.TabIndex = 0
		Me.cboMediaType.TabStop = True
		Me.cboMediaType.Visible = True
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(245, 84)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 23)
		Me.cmdOK.TabIndex = 2
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "*{&OK}"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(325, 84)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 23)
		Me.cmdCancel.TabIndex = 3
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "*{&Cancel}"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtRefundAmount
		' 
		Me.txtRefundAmount.AcceptsReturn = True
		Me.txtRefundAmount.AutoSize = False
		Me.txtRefundAmount.BackColor = System.Drawing.SystemColors.Window
		Me.txtRefundAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtRefundAmount.CausesValidation = True
		Me.txtRefundAmount.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtRefundAmount.Enabled = True
		Me.txtRefundAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtRefundAmount.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtRefundAmount.HideSelection = True
		Me.txtRefundAmount.Location = New System.Drawing.Point(170, 48)
		Me.txtRefundAmount.MaxLength = 0
		Me.txtRefundAmount.Multiline = False
		Me.txtRefundAmount.Name = "txtRefundAmount"
		Me.txtRefundAmount.ReadOnly = False
		Me.txtRefundAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtRefundAmount.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtRefundAmount.Size = New System.Drawing.Size(147, 21)
		Me.txtRefundAmount.TabIndex = 1
		Me.txtRefundAmount.TabStop = True
		Me.txtRefundAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.txtRefundAmount.Visible = True
		' 
		' lblPaymentMethod
		' 
		Me.lblPaymentMethod.AutoSize = True
		Me.lblPaymentMethod.BackColor = System.Drawing.SystemColors.Control
		Me.lblPaymentMethod.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPaymentMethod.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPaymentMethod.Enabled = True
		Me.lblPaymentMethod.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPaymentMethod.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPaymentMethod.Location = New System.Drawing.Point(16, 18)
		Me.lblPaymentMethod.Name = "lblPaymentMethod"
		Me.lblPaymentMethod.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPaymentMethod.Size = New System.Drawing.Size(153, 19)
		Me.lblPaymentMethod.TabIndex = 4
		Me.lblPaymentMethod.Text = "*{Method of Payment:}"
		Me.lblPaymentMethod.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPaymentMethod.UseMnemonic = True
		Me.lblPaymentMethod.Visible = True
		' 
		' lblRefundAmount
		' 
		Me.lblRefundAmount.AutoSize = False
		Me.lblRefundAmount.BackColor = System.Drawing.SystemColors.Control
		Me.lblRefundAmount.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblRefundAmount.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblRefundAmount.Enabled = True
		Me.lblRefundAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblRefundAmount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblRefundAmount.Location = New System.Drawing.Point(16, 50)
		Me.lblRefundAmount.Name = "lblRefundAmount"
		Me.lblRefundAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblRefundAmount.Size = New System.Drawing.Size(167, 19)
		Me.lblRefundAmount.TabIndex = 5
		Me.lblRefundAmount.Text = "*{Refund Amount:}"
		Me.lblRefundAmount.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblRefundAmount.UseMnemonic = True
		Me.lblRefundAmount.Visible = True
		' 
		' frmRefund
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(412, 121)
		Me.ControlBox = True
		Me.Controls.Add(Me.cboMediaType)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.txtRefundAmount)
		Me.Controls.Add(Me.lblPaymentMethod)
		Me.Controls.Add(Me.lblRefundAmount)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = True
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmRefund"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Refund"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class