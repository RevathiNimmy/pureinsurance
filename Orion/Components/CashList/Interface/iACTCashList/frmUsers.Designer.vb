<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUsers
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
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdClose As System.Windows.Forms.Button
	Public WithEvents cmdRefresh As System.Windows.Forms.Button
	Private WithEvents _lvwUsers_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwUsers As System.Windows.Forms.ListView
	Public WithEvents lblMessage As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUsers))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdNavigate = New System.Windows.Forms.Button
		Me.cmdClose = New System.Windows.Forms.Button
		Me.cmdRefresh = New System.Windows.Forms.Button
		Me.lvwUsers = New System.Windows.Forms.ListView
		Me._lvwUsers_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me.lblMessage = New System.Windows.Forms.Label
		Me.lvwUsers.SuspendLayout()
		Me.SuspendLayout()
		' 
		' cmdNavigate
		' 
		Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
		Me.cmdNavigate.CausesValidation = True
		Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdNavigate.Enabled = True
		Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNavigate.Location = New System.Drawing.Point(8, 300)
		Me.cmdNavigate.Name = "cmdNavigate"
		Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
		Me.cmdNavigate.TabIndex = 2
		Me.cmdNavigate.TabStop = True
		Me.cmdNavigate.Tag = "CAP;203"
		Me.cmdNavigate.Text = "*{Navigate}"
		Me.cmdNavigate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.cmdNavigate.Visible = False
		' 
		' cmdClose
		' 
		Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
		Me.cmdClose.CausesValidation = True
		Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdClose.Enabled = True
		Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdClose.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdClose.Location = New System.Drawing.Point(136, 276)
		Me.cmdClose.Name = "cmdClose"
		Me.cmdClose.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdClose.Size = New System.Drawing.Size(73, 22)
		Me.cmdClose.TabIndex = 1
		Me.cmdClose.TabStop = True
		Me.cmdClose.Tag = "CAP;471"
		Me.cmdClose.Text = "*{Close}"
		Me.cmdClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdRefresh
		' 
		Me.cmdRefresh.BackColor = System.Drawing.SystemColors.Control
		Me.cmdRefresh.CausesValidation = True
		Me.cmdRefresh.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdRefresh.Enabled = True
		Me.cmdRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdRefresh.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdRefresh.Location = New System.Drawing.Point(8, 276)
		Me.cmdRefresh.Name = "cmdRefresh"
		Me.cmdRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdRefresh.Size = New System.Drawing.Size(73, 22)
		Me.cmdRefresh.TabIndex = 0
		Me.cmdRefresh.TabStop = True
		Me.cmdRefresh.Tag = "CAP;470"
		Me.cmdRefresh.Text = "*{Refresh}"
		Me.cmdRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lvwUsers
		' 
		Me.lvwUsers.BackColor = System.Drawing.SystemColors.Window
		Me.lvwUsers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwUsers.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwUsers.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwUsers.FullRowSelect = True
		Me.lvwUsers.HideSelection = False
		Me.lvwUsers.LabelEdit = False
		Me.lvwUsers.LabelWrap = True
		Me.lvwUsers.Location = New System.Drawing.Point(8, 60)
		Me.lvwUsers.Name = "lvwUsers"
		Me.lvwUsers.Size = New System.Drawing.Size(201, 209)
		Me.lvwUsers.TabIndex = 3
		Me.lvwUsers.Tag = "CAP;452"
		Me.lvwUsers.View = System.Windows.Forms.View.Details
		Me.lvwUsers.Columns.Add(Me._lvwUsers_ColumnHeader_1)
		' 
		' _lvwUsers_ColumnHeader_1
		' 
		Me._lvwUsers_ColumnHeader_1.Text = "*{User}"
		Me._lvwUsers_ColumnHeader_1.Width = 97
		' 
		' lblMessage
		' 
		Me.lblMessage.AutoSize = False
		Me.lblMessage.BackColor = System.Drawing.SystemColors.Control
		Me.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMessage.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMessage.Enabled = True
		Me.lblMessage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMessage.Location = New System.Drawing.Point(8, 8)
		Me.lblMessage.Name = "lblMessage"
		Me.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMessage.Size = New System.Drawing.Size(201, 49)
		Me.lblMessage.TabIndex = 4
		Me.lblMessage.Tag = "CAP;451"
		Me.lblMessage.Text = "*{Message}"
		Me.lblMessage.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMessage.UseMnemonic = True
		Me.lblMessage.Visible = True
		' 
		' frmUsers
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(217, 304)
		Me.ControlBox = False
		Me.Controls.Add(Me.cmdNavigate)
		Me.Controls.Add(Me.cmdClose)
		Me.Controls.Add(Me.cmdRefresh)
		Me.Controls.Add(Me.lvwUsers)
		Me.Controls.Add(Me.lblMessage)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = True
		Me.Icon = CType(resources.GetObject("frmUsers.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(203, 163)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmUsers"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Tag = "CAP;450"
		Me.Text = "*{Title}"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.lvwUsers.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class