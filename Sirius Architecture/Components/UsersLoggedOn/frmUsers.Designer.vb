<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUsers
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lstInstances_InitializeColumnKeys()
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
	Public WithEvents cmdMessage As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdRefresh As System.Windows.Forms.Button
	Public WithEvents cmdReset As System.Windows.Forms.Button
	Public WithEvents tmrRefreshInstances As System.Windows.Forms.Timer
	Private WithEvents _lstInstances_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lstInstances_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lstInstances_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents lstInstances As System.Windows.Forms.ListView
	Public WithEvents ilsUser As System.Windows.Forms.ImageList
	Public WithEvents lblUsers As System.Windows.Forms.Label
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUsers))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdMessage = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cmdRefresh = New System.Windows.Forms.Button
		Me.cmdReset = New System.Windows.Forms.Button
		Me.tmrRefreshInstances = New System.Windows.Forms.Timer(components)
		Me.lstInstances = New System.Windows.Forms.ListView
		Me._lstInstances_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lstInstances_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lstInstances_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me.ilsUser = New System.Windows.Forms.ImageList
		Me.lblUsers = New System.Windows.Forms.Label
		Me.lstInstances.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdMessage
		' 
		Me.cmdMessage.BackColor = System.Drawing.SystemColors.Control
		Me.cmdMessage.CausesValidation = True
		Me.cmdMessage.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdMessage.Enabled = True
		Me.cmdMessage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdMessage.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdMessage.Location = New System.Drawing.Point(168, 208)
		Me.cmdMessage.Name = "cmdMessage"
		Me.cmdMessage.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdMessage.Size = New System.Drawing.Size(73, 22)
		Me.cmdMessage.TabIndex = 6
		Me.cmdMessage.TabStop = True
		Me.cmdMessage.Text = "&Message"
		Me.cmdMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdMessage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(364, 208)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 5
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
		Me.cmdOK.Location = New System.Drawing.Point(284, 208)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 4
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdRefresh
		' 
		Me.cmdRefresh.BackColor = System.Drawing.SystemColors.Control
		Me.cmdRefresh.CausesValidation = True
		Me.cmdRefresh.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdRefresh.Enabled = True
		Me.cmdRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdRefresh.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdRefresh.Location = New System.Drawing.Point(8, 208)
		Me.cmdRefresh.Name = "cmdRefresh"
		Me.cmdRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdRefresh.Size = New System.Drawing.Size(73, 22)
		Me.cmdRefresh.TabIndex = 3
		Me.cmdRefresh.TabStop = True
		Me.cmdRefresh.Text = "&Refresh"
		Me.cmdRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdReset
		' 
		Me.cmdReset.BackColor = System.Drawing.SystemColors.Control
		Me.cmdReset.CausesValidation = True
		Me.cmdReset.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdReset.Enabled = True
		Me.cmdReset.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdReset.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdReset.Location = New System.Drawing.Point(88, 208)
		Me.cmdReset.Name = "cmdReset"
		Me.cmdReset.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdReset.Size = New System.Drawing.Size(73, 22)
		Me.cmdReset.TabIndex = 2
		Me.cmdReset.TabStop = True
		Me.cmdReset.Text = "Re&set"
		Me.cmdReset.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdReset.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tmrRefreshInstances
		' 
		Me.tmrRefreshInstances.Enabled = False
		Me.tmrRefreshInstances.Interval = 60000
		' 
		' lstInstances
		' 
		Me.lstInstances.BackColor = System.Drawing.SystemColors.Window
		Me.lstInstances.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lstInstances.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lstInstances.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lstInstances.HideSelection = True
		Me.lstInstances.LabelEdit = True
		Me.lstInstances.LabelWrap = True
		Me.lstInstances.Location = New System.Drawing.Point(8, 48)
		Me.lstInstances.Name = "lstInstances"
		Me.lstInstances.Size = New System.Drawing.Size(429, 153)
		Me.lstInstances.SmallImageList = ilsUser
		Me.lstInstances.TabIndex = 1
		Me.lstInstances.View = System.Windows.Forms.View.Details
		Me.lstInstances.Columns.Add(Me._lstInstances_ColumnHeader_1)
		Me.lstInstances.Columns.Add(Me._lstInstances_ColumnHeader_2)
		Me.lstInstances.Columns.Add(Me._lstInstances_ColumnHeader_3)
		' 
		' _lstInstances_ColumnHeader_1
		' 
		Me._lstInstances_ColumnHeader_1.Tag = ""
		Me._lstInstances_ColumnHeader_1.Text = "User Name"
		Me._lstInstances_ColumnHeader_1.Width = 101
		' 
		' _lstInstances_ColumnHeader_2
		' 
		Me._lstInstances_ColumnHeader_2.Tag = ""
		Me._lstInstances_ColumnHeader_2.Text = "Logged on at Client"
		Me._lstInstances_ColumnHeader_2.Width = 101
		' 
		' _lstInstances_ColumnHeader_3
		' 
		Me._lstInstances_ColumnHeader_3.Tag = ""
		Me._lstInstances_ColumnHeader_3.Text = "Logon Time"
		Me._lstInstances_ColumnHeader_3.Width = 94
		' 
		' ilsUser
		' 
		Me.ilsUser.ImageSize = New System.Drawing.Size(16, 16)
		Me.ilsUser.ImageStream = CType(resources.GetObject("ilsUser.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.ilsUser.TransparentColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.ilsUser.Images.SetKeyName(0, "User")
		' 
		' lblUsers
		' 
		Me.lblUsers.AutoSize = False
		Me.lblUsers.BackColor = System.Drawing.SystemColors.Control
		Me.lblUsers.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblUsers.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblUsers.Enabled = True
		Me.lblUsers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblUsers.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblUsers.Location = New System.Drawing.Point(8, 8)
		Me.lblUsers.Name = "lblUsers"
		Me.lblUsers.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblUsers.Size = New System.Drawing.Size(429, 33)
		Me.lblUsers.TabIndex = 0
		Me.lblUsers.Text = "The following users are logged on to Sirius. They must be logged off before continuing with processes that modify the server configuration."
		Me.lblUsers.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblUsers.UseMnemonic = True
		Me.lblUsers.Visible = True
		' 
		' frmUsers
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(445, 242)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdMessage)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cmdRefresh)
		Me.Controls.Add(Me.cmdReset)
		Me.Controls.Add(Me.lstInstances)
		Me.Controls.Add(Me.lblUsers)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmUsers.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmUsers"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Users Logged On"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lstInstances, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.lstInstances.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub lstInstances_InitializeColumnKeys()
		Me._lstInstances_ColumnHeader_1.Name = ""
		Me._lstInstances_ColumnHeader_2.Name = ""
		Me._lstInstances_ColumnHeader_3.Name = ""
	End Sub
#End Region 
End Class