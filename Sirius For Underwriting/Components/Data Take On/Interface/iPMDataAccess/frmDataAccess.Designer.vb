<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDataAccess
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
	Public WithEvents proProgress As System.Windows.Forms.ProgressBar
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdWriteToDatabase As System.Windows.Forms.Button
	Public WithEvents lblProgress As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDataAccess))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.proProgress = New System.Windows.Forms.ProgressBar
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdWriteToDatabase = New System.Windows.Forms.Button
		Me.lblProgress = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' proProgress
		' 
		Me.proProgress.Location = New System.Drawing.Point(19, 120)
		Me.proProgress.Name = "proProgress"
		Me.proProgress.Size = New System.Drawing.Size(209, 17)
		Me.proProgress.TabIndex = 2
		Me.proProgress.Visible = False
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(83, 56)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(89, 25)
		Me.cmdCancel.TabIndex = 1
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdWriteToDatabase
		' 
		Me.cmdWriteToDatabase.BackColor = System.Drawing.SystemColors.Control
		Me.cmdWriteToDatabase.CausesValidation = True
		Me.cmdWriteToDatabase.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdWriteToDatabase.Enabled = True
		Me.cmdWriteToDatabase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdWriteToDatabase.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdWriteToDatabase.Location = New System.Drawing.Point(83, 16)
		Me.cmdWriteToDatabase.Name = "cmdWriteToDatabase"
		Me.cmdWriteToDatabase.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdWriteToDatabase.Size = New System.Drawing.Size(89, 25)
		Me.cmdWriteToDatabase.TabIndex = 0
		Me.cmdWriteToDatabase.TabStop = True
		Me.cmdWriteToDatabase.Text = "&Write To DB"
		Me.cmdWriteToDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdWriteToDatabase.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lblProgress
		' 
		Me.lblProgress.AutoSize = False
		Me.lblProgress.BackColor = System.Drawing.SystemColors.Control
		Me.lblProgress.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblProgress.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblProgress.Enabled = True
		Me.lblProgress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblProgress.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblProgress.Location = New System.Drawing.Point(19, 96)
		Me.lblProgress.Name = "lblProgress"
		Me.lblProgress.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblProgress.Size = New System.Drawing.Size(209, 17)
		Me.lblProgress.TabIndex = 3
		Me.lblProgress.Text = "Label1"
		Me.lblProgress.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me.lblProgress.UseMnemonic = True
		Me.lblProgress.Visible = False
		' 
		' frmDataAccess
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(247, 153)
		Me.ControlBox = True
		Me.Controls.Add(Me.proProgress)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdWriteToDatabase)
		Me.Controls.Add(Me.lblProgress)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = False
		Me.MinimizeBox = True
		Me.Name = "frmDataAccess"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Data take on"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class