<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEditWarning
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
	Public WithEvents cmdView As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents lblWarning As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEditWarning))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdView = New System.Windows.Forms.Button
		Me.cmdEdit = New System.Windows.Forms.Button
		Me.lblWarning = New System.Windows.Forms.Label
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
		Me.cmdCancel.Location = New System.Drawing.Point(168, 64)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 21)
		Me.cmdCancel.TabIndex = 2
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdView
		' 
		Me.cmdView.BackColor = System.Drawing.SystemColors.Control
		Me.cmdView.CausesValidation = True
		Me.cmdView.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdView.Enabled = True
		Me.cmdView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdView.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdView.Location = New System.Drawing.Point(8, 64)
		Me.cmdView.Name = "cmdView"
		Me.cmdView.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdView.Size = New System.Drawing.Size(73, 21)
		Me.cmdView.TabIndex = 0
		Me.cmdView.TabStop = True
		Me.cmdView.Text = "&View"
		Me.cmdView.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdEdit
		' 
		Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdEdit.CausesValidation = True
		Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdEdit.Enabled = True
		Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdEdit.Location = New System.Drawing.Point(88, 64)
		Me.cmdEdit.Name = "cmdEdit"
		Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdEdit.Size = New System.Drawing.Size(73, 21)
		Me.cmdEdit.TabIndex = 1
		Me.cmdEdit.TabStop = True
		Me.cmdEdit.Text = "&Edit"
		Me.cmdEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lblWarning
		' 
		Me.lblWarning.AutoSize = False
		Me.lblWarning.BackColor = System.Drawing.SystemColors.Control
		Me.lblWarning.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblWarning.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblWarning.Enabled = True
		Me.lblWarning.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblWarning.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lblWarning.Location = New System.Drawing.Point(8, 8)
		Me.lblWarning.Name = "lblWarning"
		Me.lblWarning.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblWarning.Size = New System.Drawing.Size(233, 49)
		Me.lblWarning.TabIndex = 3
		Me.lblWarning.Text = "WARNING :- Editing this item will reset all tax details for this item. "
		Me.lblWarning.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblWarning.UseMnemonic = True
		Me.lblWarning.Visible = True
		' 
		' frmEditWarning
		' 
		Me.AcceptButton = Me.cmdEdit
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(245, 88)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdView)
		Me.Controls.Add(Me.cmdEdit)
		Me.Controls.Add(Me.lblWarning)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 30)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmEditWarning"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Edit Warning"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class