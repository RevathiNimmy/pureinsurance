<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSettle
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
	Public WithEvents txtSettleDate As System.Windows.Forms.TextBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblSettle As System.Windows.Forms.Label
	Public WithEvents lblSettleDate As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSettle))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.txtSettleDate = New System.Windows.Forms.TextBox
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.lblSettle = New System.Windows.Forms.Label
		Me.lblSettleDate = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' txtSettleDate
		' 
		Me.txtSettleDate.AcceptsReturn = True
		Me.txtSettleDate.AutoSize = False
		Me.txtSettleDate.BackColor = System.Drawing.SystemColors.Window
		Me.txtSettleDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtSettleDate.CausesValidation = True
		Me.txtSettleDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtSettleDate.Enabled = True
		Me.txtSettleDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtSettleDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtSettleDate.HideSelection = True
		Me.txtSettleDate.Location = New System.Drawing.Point(128, 8)
		Me.txtSettleDate.MaxLength = 0
		Me.txtSettleDate.Multiline = False
		Me.txtSettleDate.Name = "txtSettleDate"
		Me.txtSettleDate.ReadOnly = False
		Me.txtSettleDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtSettleDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtSettleDate.Size = New System.Drawing.Size(121, 19)
		Me.txtSettleDate.TabIndex = 2
		Me.txtSettleDate.TabStop = True
		Me.txtSettleDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtSettleDate.Visible = True
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(184, 96)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(65, 22)
		Me.cmdCancel.TabIndex = 1
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
		Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(112, 96)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(65, 22)
		Me.cmdOK.TabIndex = 0
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lblSettle
		' 
		Me.lblSettle.AutoSize = False
		Me.lblSettle.BackColor = System.Drawing.SystemColors.Control
		Me.lblSettle.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSettle.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSettle.Enabled = True
		Me.lblSettle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSettle.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSettle.Location = New System.Drawing.Point(8, 40)
		Me.lblSettle.Name = "lblSettle"
		Me.lblSettle.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSettle.Size = New System.Drawing.Size(241, 49)
		Me.lblSettle.TabIndex = 4
		Me.lblSettle.Text = "Label2"
		Me.lblSettle.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblSettle.UseMnemonic = True
		Me.lblSettle.Visible = True
		' 
		' lblSettleDate
		' 
		Me.lblSettleDate.AutoSize = False
		Me.lblSettleDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblSettleDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSettleDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSettleDate.Enabled = True
		Me.lblSettleDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSettleDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSettleDate.Location = New System.Drawing.Point(8, 8)
		Me.lblSettleDate.Name = "lblSettleDate"
		Me.lblSettleDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSettleDate.Size = New System.Drawing.Size(121, 17)
		Me.lblSettleDate.TabIndex = 3
		Me.lblSettleDate.Text = "Settlement Date:"
		Me.lblSettleDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblSettleDate.UseMnemonic = True
		Me.lblSettleDate.Visible = True
		' 
		' frmSettle
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(256, 124)
		Me.ControlBox = True
		Me.Controls.Add(Me.txtSettleDate)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.lblSettle)
		Me.Controls.Add(Me.lblSettleDate)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmSettle"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "Plan Settlement"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class