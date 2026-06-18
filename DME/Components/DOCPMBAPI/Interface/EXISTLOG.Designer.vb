<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmExistingLogin
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
    Public WithEvents lbl3StatusBar As System.Windows.Forms.Label
	Public WithEvents pan3StatusBar As System.Windows.Forms.Panel
	Public WithEvents pan3StatusBarOut As System.Windows.Forms.Panel
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents outUser As AxMSOutl.AxOutline
	Public WithEvents pan3UserList As System.Windows.Forms.Panel
	Public WithEvents fra3ExistingLogins As System.Windows.Forms.GroupBox
	Public WithEvents pan3ExistingLogins As System.Windows.Forms.Panel
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmExistingLogin))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.pan3StatusBarOut = New System.Windows.Forms.Panel
		Me.pan3StatusBar = New System.Windows.Forms.Panel
		Me.pan3ExistingLogins = New System.Windows.Forms.Panel
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.fra3ExistingLogins = New System.Windows.Forms.GroupBox
        Me.pan3UserList = New System.Windows.Forms.Panel
        Me.lbl3StatusBar = New Label
		Me.outUser = New AxMSOutl.AxOutline
		CType(Me.outUser, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pan3StatusBarOut.SuspendLayout()
		Me.pan3ExistingLogins.SuspendLayout()
		Me.fra3ExistingLogins.SuspendLayout()
		Me.pan3UserList.SuspendLayout()
		Me.SuspendLayout()
		' 
		' pan3StatusBarOut
		' 
		Me.pan3StatusBarOut.BackColor = System.Drawing.SystemColors.Control
		Me.pan3StatusBarOut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.pan3StatusBarOut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.pan3StatusBarOut.Controls.Add(Me.pan3StatusBar)
		Me.pan3StatusBarOut.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.pan3StatusBarOut.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3StatusBarOut.Location = New System.Drawing.Point(0, 229)
		Me.pan3StatusBarOut.Name = "pan3StatusBarOut"
		Me.pan3StatusBarOut.Size = New System.Drawing.Size(382, 32)
        Me.pan3StatusBarOut.TabIndex = 1
        Me.pan3StatusBarOut.Controls.Add(lbl3StatusBar)
		' 
		' pan3StatusBar
		' 
		Me.pan3StatusBar.BackColor = System.Drawing.SystemColors.Control
		Me.pan3StatusBar.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.pan3StatusBar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3StatusBar.Location = New System.Drawing.Point(8, 8)
		Me.pan3StatusBar.Name = "pan3StatusBar"
		Me.pan3StatusBar.Size = New System.Drawing.Size(365, 17)
		Me.pan3StatusBar.TabIndex = 2
		' 
		' pan3ExistingLogins
		' 
		Me.pan3ExistingLogins.BackColor = System.Drawing.SystemColors.Control
		Me.pan3ExistingLogins.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.pan3ExistingLogins.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.pan3ExistingLogins.Controls.Add(Me.cmdCancel)
		Me.pan3ExistingLogins.Controls.Add(Me.cmdOK)
		Me.pan3ExistingLogins.Controls.Add(Me.fra3ExistingLogins)
		Me.pan3ExistingLogins.Dock = System.Windows.Forms.DockStyle.Top
		Me.pan3ExistingLogins.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3ExistingLogins.Location = New System.Drawing.Point(0, 0)
		Me.pan3ExistingLogins.Name = "pan3ExistingLogins"
		Me.pan3ExistingLogins.Size = New System.Drawing.Size(382, 229)
		Me.pan3ExistingLogins.TabIndex = 0
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(296, 188)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 25)
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
		Me.cmdOK.Enabled = False
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(216, 188)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 25)
		Me.cmdOK.TabIndex = 4
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' fra3ExistingLogins
		' 
		Me.fra3ExistingLogins.Controls.Add(Me.pan3UserList)
		Me.fra3ExistingLogins.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)

        'Me.fra3ExistingLogins.Font3D = Threed.enumFont3DConstants._InsetLight

		Me.fra3ExistingLogins.Location = New System.Drawing.Point(16, 12)
		Me.fra3ExistingLogins.Name = "fra3ExistingLogins"
		Me.fra3ExistingLogins.Size = New System.Drawing.Size(353, 163)
		Me.fra3ExistingLogins.TabIndex = 3
		' 
		' pan3UserList
		' 
		Me.pan3UserList.BackColor = System.Drawing.SystemColors.Control
		Me.pan3UserList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.pan3UserList.Controls.Add(Me.outUser)
		Me.pan3UserList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3UserList.Location = New System.Drawing.Point(8, 24)
		Me.pan3UserList.Name = "pan3UserList"
		Me.pan3UserList.Size = New System.Drawing.Size(337, 129)
		Me.pan3UserList.TabIndex = 6
		' 
		' outUser
		' 
		Me.outUser.Location = New System.Drawing.Point(2, 2)
		Me.outUser.Name = "outUser"
        Me.outUser.OcxState = CType(resources.GetObject("outUser.OcxState"), System.Windows.Forms.AxHost.State)
		Me.outUser.Size = New System.Drawing.Size(333, 125)
		Me.outUser.TabIndex = 7
		' 
		' frmExistingLogin
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
		Me.BackColor = System.Drawing.SystemColors.Window
		Me.ClientSize = New System.Drawing.Size(382, 261)
		Me.ControlBox = False
		Me.Controls.Add(Me.pan3StatusBarOut)
		Me.Controls.Add(Me.pan3ExistingLogins)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.ForeColor = System.Drawing.SystemColors.WindowText
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(171, 150)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmExistingLogin"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Existing Logins"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		CType(Me.outUser, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pan3StatusBarOut.ResumeLayout(False)
		Me.pan3ExistingLogins.ResumeLayout(False)
		Me.fra3ExistingLogins.ResumeLayout(False)
		Me.pan3UserList.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class