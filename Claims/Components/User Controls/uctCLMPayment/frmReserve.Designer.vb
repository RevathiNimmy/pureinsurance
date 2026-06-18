<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReserve
#Region "Windows Form Designer generated code "
	Friend Sub New()
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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents txtRevisionAmount As System.Windows.Forms.TextBox
	Public WithEvents txtLossCurrency As System.Windows.Forms.TextBox
	Public WithEvents txtInitialReserve As System.Windows.Forms.TextBox
	Public WithEvents txtPaymentLine As System.Windows.Forms.TextBox
	Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReserve))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOk = New System.Windows.Forms.Button
		Me.Frame1 = New System.Windows.Forms.GroupBox
		Me.txtRevisionAmount = New System.Windows.Forms.TextBox
		Me.txtLossCurrency = New System.Windows.Forms.TextBox
		Me.txtInitialReserve = New System.Windows.Forms.TextBox
		Me.txtPaymentLine = New System.Windows.Forms.TextBox
		Me.Label4 = New System.Windows.Forms.Label
		Me.Label3 = New System.Windows.Forms.Label
		Me.Label2 = New System.Windows.Forms.Label
		Me.Label1 = New System.Windows.Forms.Label
		Me.Frame1.SuspendLayout()
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
		Me.cmdCancel.Location = New System.Drawing.Point(232, 152)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 25)
		Me.cmdCancel.TabIndex = 5
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOk
		' 
		Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOk.CausesValidation = True
		Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOk.Enabled = True
		Me.cmdOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOk.Location = New System.Drawing.Point(152, 152)
		Me.cmdOk.Name = "cmdOk"
		Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOk.Size = New System.Drawing.Size(73, 25)
		Me.cmdOk.TabIndex = 4
		Me.cmdOk.TabStop = True
		Me.cmdOk.Text = "Ok"
		Me.cmdOk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' Frame1
		' 
		Me.Frame1.BackColor = System.Drawing.SystemColors.Control
		Me.Frame1.Controls.Add(Me.txtRevisionAmount)
		Me.Frame1.Controls.Add(Me.txtLossCurrency)
		Me.Frame1.Controls.Add(Me.txtInitialReserve)
		Me.Frame1.Controls.Add(Me.txtPaymentLine)
		Me.Frame1.Controls.Add(Me.Label4)
		Me.Frame1.Controls.Add(Me.Label3)
		Me.Frame1.Controls.Add(Me.Label2)
		Me.Frame1.Controls.Add(Me.Label1)
		Me.Frame1.Enabled = True
		Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Frame1.Location = New System.Drawing.Point(8, 8)
		Me.Frame1.Name = "Frame1"
		Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Frame1.Size = New System.Drawing.Size(297, 137)
		Me.Frame1.TabIndex = 6
		Me.Frame1.Text = "Reserve Details"
		Me.Frame1.Visible = True
		' 
		' txtRevisionAmount
		' 
		Me.txtRevisionAmount.AcceptsReturn = True
		Me.txtRevisionAmount.AutoSize = False
		Me.txtRevisionAmount.BackColor = System.Drawing.SystemColors.Window
		Me.txtRevisionAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.txtRevisionAmount.CausesValidation = True
		Me.txtRevisionAmount.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtRevisionAmount.Enabled = True
		Me.txtRevisionAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtRevisionAmount.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtRevisionAmount.HideSelection = True
		Me.txtRevisionAmount.Location = New System.Drawing.Point(112, 77)
		Me.txtRevisionAmount.MaxLength = 15
		Me.txtRevisionAmount.Multiline = False
		Me.txtRevisionAmount.Name = "txtRevisionAmount"
		Me.txtRevisionAmount.ReadOnly = False
		Me.txtRevisionAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtRevisionAmount.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtRevisionAmount.Size = New System.Drawing.Size(169, 19)
		Me.txtRevisionAmount.TabIndex = 2
		Me.txtRevisionAmount.TabStop = True
		Me.txtRevisionAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtRevisionAmount.Visible = True
		' 
		' txtLossCurrency
		' 
		Me.txtLossCurrency.AcceptsReturn = True
		Me.txtLossCurrency.AutoSize = False
		Me.txtLossCurrency.BackColor = System.Drawing.SystemColors.Control
		Me.txtLossCurrency.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.txtLossCurrency.CausesValidation = True
		Me.txtLossCurrency.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtLossCurrency.Enabled = True
		Me.txtLossCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtLossCurrency.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtLossCurrency.HideSelection = True
		Me.txtLossCurrency.Location = New System.Drawing.Point(112, 104)
		Me.txtLossCurrency.MaxLength = 0
		Me.txtLossCurrency.Multiline = False
		Me.txtLossCurrency.Name = "txtLossCurrency"
		Me.txtLossCurrency.ReadOnly = True
		Me.txtLossCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtLossCurrency.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtLossCurrency.Size = New System.Drawing.Size(169, 19)
		Me.txtLossCurrency.TabIndex = 3
		Me.txtLossCurrency.TabStop = False
		Me.txtLossCurrency.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtLossCurrency.Visible = True
		' 
		' txtInitialReserve
		' 
		Me.txtInitialReserve.AcceptsReturn = True
		Me.txtInitialReserve.AutoSize = False
		Me.txtInitialReserve.BackColor = System.Drawing.SystemColors.Control
		Me.txtInitialReserve.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.txtInitialReserve.CausesValidation = True
		Me.txtInitialReserve.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtInitialReserve.Enabled = True
		Me.txtInitialReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtInitialReserve.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtInitialReserve.HideSelection = True
		Me.txtInitialReserve.Location = New System.Drawing.Point(112, 50)
		Me.txtInitialReserve.MaxLength = 0
		Me.txtInitialReserve.Multiline = False
		Me.txtInitialReserve.Name = "txtInitialReserve"
		Me.txtInitialReserve.ReadOnly = True
		Me.txtInitialReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtInitialReserve.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtInitialReserve.Size = New System.Drawing.Size(169, 19)
		Me.txtInitialReserve.TabIndex = 1
		Me.txtInitialReserve.TabStop = False
		Me.txtInitialReserve.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtInitialReserve.Visible = True
		' 
		' txtPaymentLine
		' 
		Me.txtPaymentLine.AcceptsReturn = True
		Me.txtPaymentLine.AutoSize = False
		Me.txtPaymentLine.BackColor = System.Drawing.SystemColors.Control
		Me.txtPaymentLine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.txtPaymentLine.CausesValidation = True
		Me.txtPaymentLine.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPaymentLine.Enabled = True
		Me.txtPaymentLine.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPaymentLine.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPaymentLine.HideSelection = True
		Me.txtPaymentLine.Location = New System.Drawing.Point(112, 24)
		Me.txtPaymentLine.MaxLength = 0
		Me.txtPaymentLine.Multiline = False
		Me.txtPaymentLine.Name = "txtPaymentLine"
		Me.txtPaymentLine.ReadOnly = True
		Me.txtPaymentLine.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPaymentLine.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPaymentLine.Size = New System.Drawing.Size(169, 19)
		Me.txtPaymentLine.TabIndex = 0
		Me.txtPaymentLine.TabStop = False
		Me.txtPaymentLine.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPaymentLine.Visible = True
		' 
		' Label4
		' 
		Me.Label4.AutoSize = True
		Me.Label4.BackColor = System.Drawing.SystemColors.Control
		Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label4.Enabled = True
		Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label4.Location = New System.Drawing.Point(16, 107)
		Me.Label4.Name = "Label4"
		Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label4.Size = New System.Drawing.Size(67, 13)
		Me.Label4.TabIndex = 10
		Me.Label4.Text = "Loss Currency"
		Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label4.UseMnemonic = True
		Me.Label4.Visible = True
		' 
		' Label3
		' 
		Me.Label3.AutoSize = True
		Me.Label3.BackColor = System.Drawing.SystemColors.Control
		Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label3.Enabled = True
		Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label3.Location = New System.Drawing.Point(16, 80)
		Me.Label3.Name = "Label3"
		Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label3.Size = New System.Drawing.Size(80, 13)
		Me.Label3.TabIndex = 9
		Me.Label3.Text = "Revision Amount"
		Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label3.UseMnemonic = True
		Me.Label3.Visible = True
		' 
		' Label2
		' 
		Me.Label2.AutoSize = True
		Me.Label2.BackColor = System.Drawing.SystemColors.Control
		Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label2.Enabled = True
		Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label2.Location = New System.Drawing.Point(16, 53)
		Me.Label2.Name = "Label2"
		Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label2.Size = New System.Drawing.Size(67, 13)
		Me.Label2.TabIndex = 8
		Me.Label2.Text = "Initial Reserve"
		Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label2.UseMnemonic = True
		Me.Label2.Visible = True
		' 
		' Label1
		' 
		Me.Label1.AutoSize = True
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.Enabled = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Location = New System.Drawing.Point(16, 27)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(64, 13)
		Me.Label1.TabIndex = 7
		Me.Label1.Text = "Payment Line"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' frmReserve
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(312, 184)
		Me.ControlBox = False
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOk)
		Me.Controls.Add(Me.Frame1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 29)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmReserve"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Reserve Details"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Frame1.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class