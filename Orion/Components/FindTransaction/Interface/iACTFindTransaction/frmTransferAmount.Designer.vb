<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTransferAmount
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
	Public WithEvents txtCurrencyAmount As System.Windows.Forms.TextBox
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents lblTransferAmount As System.Windows.Forms.Label
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTransferAmount))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.txtCurrencyAmount = New System.Windows.Forms.TextBox
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.lblTransferAmount = New System.Windows.Forms.Label
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' txtCurrencyAmount
		' 
		Me.txtCurrencyAmount.AcceptsReturn = True
		Me.txtCurrencyAmount.AutoSize = False
		Me.txtCurrencyAmount.BackColor = System.Drawing.SystemColors.Window
		Me.txtCurrencyAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtCurrencyAmount.CausesValidation = True
		Me.txtCurrencyAmount.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtCurrencyAmount.Enabled = True
		Me.txtCurrencyAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtCurrencyAmount.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtCurrencyAmount.HideSelection = True
		Me.txtCurrencyAmount.Location = New System.Drawing.Point(184, 14)
		Me.txtCurrencyAmount.MaxLength = 0
		Me.txtCurrencyAmount.Multiline = False
		Me.txtCurrencyAmount.Name = "txtCurrencyAmount"
		Me.txtCurrencyAmount.ReadOnly = False
		Me.txtCurrencyAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtCurrencyAmount.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtCurrencyAmount.Size = New System.Drawing.Size(121, 21)
		Me.txtCurrencyAmount.TabIndex = 3
		Me.txtCurrencyAmount.TabStop = True
		Me.txtCurrencyAmount.Tag = "F;FMT;$;"
		Me.txtCurrencyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtCurrencyAmount.Visible = True
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(152, 50)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 23)
		Me.cmdOK.TabIndex = 1
		Me.cmdOK.TabStop = False
		Me.cmdOK.Tag = "CAP;200"
		Me.cmdOK.Text = "{&OK}"
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
		Me.cmdCancel.Location = New System.Drawing.Point(232, 50)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 23)
		Me.cmdCancel.TabIndex = 0
		Me.cmdCancel.TabStop = False
		Me.cmdCancel.Tag = "CAP;201"
		Me.cmdCancel.Text = "{&Cancel}"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lblTransferAmount
		' 
		Me.lblTransferAmount.AutoSize = False
		Me.lblTransferAmount.BackColor = System.Drawing.SystemColors.Control
		Me.lblTransferAmount.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTransferAmount.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTransferAmount.Enabled = True
		Me.lblTransferAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTransferAmount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTransferAmount.Location = New System.Drawing.Point(16, 16)
		Me.lblTransferAmount.Name = "lblTransferAmount"
		Me.lblTransferAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTransferAmount.Size = New System.Drawing.Size(168, 19)
		Me.lblTransferAmount.TabIndex = 2
		Me.lblTransferAmount.Tag = "CAP;731"
		Me.lblTransferAmount.Text = "{Transfer Amount:}"
		Me.lblTransferAmount.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTransferAmount.UseMnemonic = True
		Me.lblTransferAmount.Visible = True
		' 
		' frmTransferAmount
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(321, 83)
		Me.ControlBox = True
		Me.Controls.Add(Me.txtCurrencyAmount)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.lblTransferAmount)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = True
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmTransferAmount"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Tag = "CAP;732"
		Me.Text = "{Transfer}"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class