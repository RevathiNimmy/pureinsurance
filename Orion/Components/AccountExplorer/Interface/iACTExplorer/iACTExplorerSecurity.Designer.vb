<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSecurity
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
	Public WithEvents uctPickListUpdate As uctPickList.PickList
	Public WithEvents fmeUpdate As System.Windows.Forms.GroupBox
	Public WithEvents uctPickListView As uctPickList.PickList
	Public WithEvents fmeView As System.Windows.Forms.GroupBox
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSecurity))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.fmeUpdate = New System.Windows.Forms.GroupBox
		Me.uctPickListUpdate = New uctPickList.PickList
		Me.fmeView = New System.Windows.Forms.GroupBox
		Me.uctPickListView = New uctPickList.PickList
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.fmeUpdate.SuspendLayout()
		Me.fmeView.SuspendLayout()
		Me.SuspendLayout()
		' 
		' fmeUpdate
		' 
		Me.fmeUpdate.BackColor = System.Drawing.SystemColors.Control
		Me.fmeUpdate.Controls.Add(Me.uctPickListUpdate)
		Me.fmeUpdate.Enabled = True
		Me.fmeUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fmeUpdate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fmeUpdate.Location = New System.Drawing.Point(8, 248)
		Me.fmeUpdate.Name = "fmeUpdate"
		Me.fmeUpdate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fmeUpdate.Size = New System.Drawing.Size(545, 233)
		Me.fmeUpdate.TabIndex = 3
		Me.fmeUpdate.Text = "Allowed to View and Update"
		Me.fmeUpdate.Visible = True
		' 
		' uctPickListUpdate
		' 
		Me.uctPickListUpdate.AvailableCaption = "User Groups"
		Me.uctPickListUpdate.BusinessObject = "bACTExplorer.Form"
		Me.uctPickListUpdate.Location = New System.Drawing.Point(8, 16)
		Me.uctPickListUpdate.Name = "uctPickListUpdate"
		Me.uctPickListUpdate.PickListType = "Update"
		Me.uctPickListUpdate.Size = New System.Drawing.Size(529, 209)
		Me.uctPickListUpdate.TabIndex = 4
		' 
		' fmeView
		' 
		Me.fmeView.BackColor = System.Drawing.SystemColors.Control
		Me.fmeView.Controls.Add(Me.uctPickListView)
		Me.fmeView.Enabled = True
		Me.fmeView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fmeView.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fmeView.Location = New System.Drawing.Point(8, 8)
		Me.fmeView.Name = "fmeView"
		Me.fmeView.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fmeView.Size = New System.Drawing.Size(545, 233)
		Me.fmeView.TabIndex = 2
		Me.fmeView.Text = "Allowed to View"
		Me.fmeView.Visible = True
		' 
		' uctPickListView
		' 
		Me.uctPickListView.AvailableCaption = "User Groups"
		Me.uctPickListView.BusinessObject = "bACTExplorer.Form"
		Me.uctPickListView.Location = New System.Drawing.Point(8, 16)
		Me.uctPickListView.Name = "uctPickListView"
		Me.uctPickListView.PickListType = "View"
		Me.uctPickListView.Size = New System.Drawing.Size(529, 209)
		Me.uctPickListView.TabIndex = 5
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(400, 488)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(74, 23)
		Me.cmdOK.TabIndex = 1
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(480, 488)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(74, 23)
		Me.cmdCancel.TabIndex = 0
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' frmSecurity
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(563, 518)
		Me.ControlBox = True
		Me.Controls.Add(Me.fmeUpdate)
		Me.Controls.Add(Me.fmeView)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cmdCancel)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmSecurity"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "[Folder Name] Security"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.fmeUpdate.ResumeLayout(False)
		Me.fmeView.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class