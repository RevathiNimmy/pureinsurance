<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmUsers
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
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
	Public WithEvents tmrRefreshInstances As System.Windows.Forms.Timer
	Public WithEvents cmdMessage As System.Windows.Forms.Button
	Public WithEvents cmdReset As System.Windows.Forms.Button
	Public WithEvents cmdRefresh As System.Windows.Forms.Button
	Public WithEvents ilsUser As System.Windows.Forms.ImageList
	Public WithEvents _lstInstances_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lstInstances_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lstInstances_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents lstInstances As System.Windows.Forms.ListView
	Public WithEvents lblUsers As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUsers))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tmrRefreshInstances = New System.Windows.Forms.Timer(Me.components)
        Me.cmdMessage = New System.Windows.Forms.Button
        Me.cmdReset = New System.Windows.Forms.Button
        Me.cmdRefresh = New System.Windows.Forms.Button
        Me.ilsUser = New System.Windows.Forms.ImageList(Me.components)
        Me.lstInstances = New System.Windows.Forms.ListView
        Me._lstInstances_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lstInstances_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lstInstances_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me.lblUsers = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(384, 176)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(49, 25)
        Me.cmdCancel.TabIndex = 6
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(312, 176)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(57, 25)
        Me.cmdOK.TabIndex = 5
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tmrRefreshInstances
        '
        Me.tmrRefreshInstances.Enabled = True
        Me.tmrRefreshInstances.Interval = 5000
        '
        'cmdMessage
        '
        Me.cmdMessage.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMessage.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMessage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMessage.Location = New System.Drawing.Point(129, 176)
        Me.cmdMessage.Name = "cmdMessage"
        Me.cmdMessage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMessage.Size = New System.Drawing.Size(74, 25)
        Me.cmdMessage.TabIndex = 4
        Me.cmdMessage.Text = "&Message"
        Me.cmdMessage.UseVisualStyleBackColor = False
        '
        'cmdReset
        '
        Me.cmdReset.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReset.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReset.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReset.Location = New System.Drawing.Point(72, 176)
        Me.cmdReset.Name = "cmdReset"
        Me.cmdReset.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReset.Size = New System.Drawing.Size(51, 25)
        Me.cmdReset.TabIndex = 3
        Me.cmdReset.Text = "Re&set"
        Me.cmdReset.UseVisualStyleBackColor = False
        '
        'cmdRefresh
        '
        Me.cmdRefresh.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRefresh.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRefresh.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRefresh.Location = New System.Drawing.Point(8, 176)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRefresh.Size = New System.Drawing.Size(57, 25)
        Me.cmdRefresh.TabIndex = 2
        Me.cmdRefresh.Text = "&Refresh"
        Me.cmdRefresh.UseVisualStyleBackColor = False
        '
        'ilsUser
        '
        Me.ilsUser.ImageStream = CType(resources.GetObject("ilsUser.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ilsUser.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ilsUser.Images.SetKeyName(0, "User")
        '
        'lstInstances
        '
        Me.lstInstances.BackColor = System.Drawing.SystemColors.Window
        Me.lstInstances.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lstInstances.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lstInstances_ColumnHeader_1, Me._lstInstances_ColumnHeader_2, Me._lstInstances_ColumnHeader_3})
        Me.lstInstances.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lstInstances.LabelEdit = True
        Me.lstInstances.Location = New System.Drawing.Point(10, 52)
        Me.lstInstances.Name = "lstInstances"
        Me.lstInstances.Size = New System.Drawing.Size(421, 112)
        Me.lstInstances.SmallImageList = Me.ilsUser
        Me.lstInstances.TabIndex = 1
        Me.lstInstances.UseCompatibleStateImageBehavior = False
        Me.lstInstances.View = System.Windows.Forms.View.Details
        '
        '_lstInstances_ColumnHeader_1
        '
        Me._lstInstances_ColumnHeader_1.Text = "User Name"
        Me._lstInstances_ColumnHeader_1.Width = 170
        '
        '_lstInstances_ColumnHeader_2
        '
        Me._lstInstances_ColumnHeader_2.Text = "Logged on at Client"
        Me._lstInstances_ColumnHeader_2.Width = 170
        '
        '_lstInstances_ColumnHeader_3
        '
        Me._lstInstances_ColumnHeader_3.Text = "Logon Time"
        Me._lstInstances_ColumnHeader_3.Width = 170
        '
        'lblUsers
        '
        Me.lblUsers.BackColor = System.Drawing.SystemColors.Control
        Me.lblUsers.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUsers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUsers.Location = New System.Drawing.Point(8, 8)
        Me.lblUsers.Name = "lblUsers"
        Me.lblUsers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUsers.Size = New System.Drawing.Size(401, 41)
        Me.lblUsers.TabIndex = 0
        Me.lblUsers.Text = "There may be users other than you logged on to S4i. Please ensure that any other " & _
            "users have logged off before continuing with processes that modify the server co" & _
            "nfiguration"
        '
        'frmUsers
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(446, 213)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdMessage)
        Me.Controls.Add(Me.cmdReset)
        Me.Controls.Add(Me.cmdRefresh)
        Me.Controls.Add(Me.lstInstances)
        Me.Controls.Add(Me.lblUsers)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmUsers"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Current users"
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class