<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializefraToClient()
		InitializefraFromClient()
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
	Public WithEvents txtToClientCode As System.Windows.Forms.TextBox
	Public WithEvents cmdToClientSelect As System.Windows.Forms.Button
	Public WithEvents txtToClientName As System.Windows.Forms.TextBox
	Public WithEvents txtToClientType As System.Windows.Forms.TextBox
	Public WithEvents lblToClient As System.Windows.Forms.Label
	Public WithEvents lblToClientName As System.Windows.Forms.Label
	Public WithEvents lblToClientType As System.Windows.Forms.Label
	Private WithEvents _fraToClient_0 As System.Windows.Forms.GroupBox
	Public WithEvents txtFromClientType As System.Windows.Forms.TextBox
	Public WithEvents txtFromClientName As System.Windows.Forms.TextBox
	Public WithEvents cmdFromClientSelect As System.Windows.Forms.Button
	Public WithEvents txtFromClientCode As System.Windows.Forms.TextBox
	Public WithEvents lblFromClientType As System.Windows.Forms.Label
	Public WithEvents lblFromClientName As System.Windows.Forms.Label
	Public WithEvents lblFromClient As System.Windows.Forms.Label
	Private WithEvents _fraFromClient_0 As System.Windows.Forms.GroupBox
	Public fraFromClient(0) As System.Windows.Forms.GroupBox
	Public fraToClient(0) As System.Windows.Forms.GroupBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOk = New System.Windows.Forms.Button
        Me._fraToClient_0 = New System.Windows.Forms.GroupBox
        Me.txtToClientCode = New System.Windows.Forms.TextBox
        Me.cmdToClientSelect = New System.Windows.Forms.Button
        Me.txtToClientName = New System.Windows.Forms.TextBox
        Me.txtToClientType = New System.Windows.Forms.TextBox
        Me.lblToClient = New System.Windows.Forms.Label
        Me.lblToClientName = New System.Windows.Forms.Label
        Me.lblToClientType = New System.Windows.Forms.Label
        Me._fraFromClient_0 = New System.Windows.Forms.GroupBox
        Me.txtFromClientType = New System.Windows.Forms.TextBox
        Me.txtFromClientName = New System.Windows.Forms.TextBox
        Me.cmdFromClientSelect = New System.Windows.Forms.Button
        Me.txtFromClientCode = New System.Windows.Forms.TextBox
        Me.lblFromClientType = New System.Windows.Forms.Label
        Me.lblFromClientName = New System.Windows.Forms.Label
        Me.lblFromClient = New System.Windows.Forms.Label
        Me._fraToClient_0.SuspendLayout()
        Me._fraFromClient_0.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(326, 266)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(77, 23)
        Me.cmdCancel.TabIndex = 9
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOk
        '
        Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOk.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOk.Location = New System.Drawing.Point(242, 266)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(77, 23)
        Me.cmdOk.TabIndex = 10
        Me.cmdOk.Text = "&Ok"
        Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOk.UseVisualStyleBackColor = False
        '
        '_fraToClient_0
        '
        Me._fraToClient_0.BackColor = System.Drawing.SystemColors.Control
        Me._fraToClient_0.Controls.Add(Me.txtToClientCode)
        Me._fraToClient_0.Controls.Add(Me.cmdToClientSelect)
        Me._fraToClient_0.Controls.Add(Me.txtToClientName)
        Me._fraToClient_0.Controls.Add(Me.txtToClientType)
        Me._fraToClient_0.Controls.Add(Me.lblToClient)
        Me._fraToClient_0.Controls.Add(Me.lblToClientName)
        Me._fraToClient_0.Controls.Add(Me.lblToClientType)
        Me._fraToClient_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraToClient_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraToClient_0.Location = New System.Drawing.Point(4, 136)
        Me._fraToClient_0.Name = "_fraToClient_0"
        Me._fraToClient_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraToClient_0.Size = New System.Drawing.Size(401, 121)
        Me._fraToClient_0.TabIndex = 14
        Me._fraToClient_0.TabStop = False
        Me._fraToClient_0.Text = "Client To"
        '
        'txtToClientCode
        '
        Me.txtToClientCode.AcceptsReturn = True
        Me.txtToClientCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtToClientCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtToClientCode.Enabled = False
        Me.txtToClientCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtToClientCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtToClientCode.Location = New System.Drawing.Point(114, 28)
        Me.txtToClientCode.MaxLength = 0
        Me.txtToClientCode.Name = "txtToClientCode"
        Me.txtToClientCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtToClientCode.Size = New System.Drawing.Size(233, 21)
        Me.txtToClientCode.TabIndex = 4
        '
        'cmdToClientSelect
        '
        Me.cmdToClientSelect.BackColor = System.Drawing.SystemColors.Control
        Me.cmdToClientSelect.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdToClientSelect.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdToClientSelect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdToClientSelect.Location = New System.Drawing.Point(352, 28)
        Me.cmdToClientSelect.Name = "cmdToClientSelect"
        Me.cmdToClientSelect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdToClientSelect.Size = New System.Drawing.Size(33, 21)
        Me.cmdToClientSelect.TabIndex = 5
        Me.cmdToClientSelect.Text = "..."
        Me.cmdToClientSelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdToClientSelect.UseVisualStyleBackColor = False
        '
        'txtToClientName
        '
        Me.txtToClientName.AcceptsReturn = True
        Me.txtToClientName.BackColor = System.Drawing.SystemColors.Window
        Me.txtToClientName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtToClientName.Enabled = False
        Me.txtToClientName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtToClientName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtToClientName.Location = New System.Drawing.Point(114, 54)
        Me.txtToClientName.MaxLength = 0
        Me.txtToClientName.Name = "txtToClientName"
        Me.txtToClientName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtToClientName.Size = New System.Drawing.Size(273, 21)
        Me.txtToClientName.TabIndex = 6
        '
        'txtToClientType
        '
        Me.txtToClientType.AcceptsReturn = True
        Me.txtToClientType.BackColor = System.Drawing.SystemColors.Window
        Me.txtToClientType.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtToClientType.Enabled = False
        Me.txtToClientType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtToClientType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtToClientType.Location = New System.Drawing.Point(114, 80)
        Me.txtToClientType.MaxLength = 0
        Me.txtToClientType.Name = "txtToClientType"
        Me.txtToClientType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtToClientType.Size = New System.Drawing.Size(273, 21)
        Me.txtToClientType.TabIndex = 8
        '
        'lblToClient
        '
        Me.lblToClient.BackColor = System.Drawing.SystemColors.Control
        Me.lblToClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblToClient.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblToClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblToClient.Location = New System.Drawing.Point(50, 28)
        Me.lblToClient.Name = "lblToClient"
        Me.lblToClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblToClient.Size = New System.Drawing.Size(51, 25)
        Me.lblToClient.TabIndex = 17
        Me.lblToClient.Text = "Client:"
        '
        'lblToClientName
        '
        Me.lblToClientName.BackColor = System.Drawing.SystemColors.Control
        Me.lblToClientName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblToClientName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblToClientName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblToClientName.Location = New System.Drawing.Point(50, 57)
        Me.lblToClientName.Name = "lblToClientName"
        Me.lblToClientName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblToClientName.Size = New System.Drawing.Size(51, 19)
        Me.lblToClientName.TabIndex = 16
        Me.lblToClientName.Text = "Name:"
        '
        'lblToClientType
        '
        Me.lblToClientType.BackColor = System.Drawing.SystemColors.Control
        Me.lblToClientType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblToClientType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblToClientType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblToClientType.Location = New System.Drawing.Point(25, 80)
        Me.lblToClientType.Name = "lblToClientType"
        Me.lblToClientType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblToClientType.Size = New System.Drawing.Size(83, 29)
        Me.lblToClientType.TabIndex = 15
        Me.lblToClientType.Text = "Client Type:"
        '
        '_fraFromClient_0
        '
        Me._fraFromClient_0.BackColor = System.Drawing.SystemColors.Control
        Me._fraFromClient_0.Controls.Add(Me.txtFromClientType)
        Me._fraFromClient_0.Controls.Add(Me.txtFromClientName)
        Me._fraFromClient_0.Controls.Add(Me.cmdFromClientSelect)
        Me._fraFromClient_0.Controls.Add(Me.txtFromClientCode)
        Me._fraFromClient_0.Controls.Add(Me.lblFromClientType)
        Me._fraFromClient_0.Controls.Add(Me.lblFromClientName)
        Me._fraFromClient_0.Controls.Add(Me.lblFromClient)
        Me._fraFromClient_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraFromClient_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraFromClient_0.Location = New System.Drawing.Point(4, 8)
        Me._fraFromClient_0.Name = "_fraFromClient_0"
        Me._fraFromClient_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraFromClient_0.Size = New System.Drawing.Size(401, 121)
        Me._fraFromClient_0.TabIndex = 7
        Me._fraFromClient_0.TabStop = False
        Me._fraFromClient_0.Text = "Client From"
        '
        'txtFromClientType
        '
        Me.txtFromClientType.AcceptsReturn = True
        Me.txtFromClientType.BackColor = System.Drawing.SystemColors.Window
        Me.txtFromClientType.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFromClientType.Enabled = False
        Me.txtFromClientType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFromClientType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFromClientType.Location = New System.Drawing.Point(112, 84)
        Me.txtFromClientType.MaxLength = 0
        Me.txtFromClientType.Name = "txtFromClientType"
        Me.txtFromClientType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFromClientType.Size = New System.Drawing.Size(273, 21)
        Me.txtFromClientType.TabIndex = 3
        '
        'txtFromClientName
        '
        Me.txtFromClientName.AcceptsReturn = True
        Me.txtFromClientName.BackColor = System.Drawing.SystemColors.Window
        Me.txtFromClientName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFromClientName.Enabled = False
        Me.txtFromClientName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFromClientName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFromClientName.Location = New System.Drawing.Point(112, 56)
        Me.txtFromClientName.MaxLength = 0
        Me.txtFromClientName.Name = "txtFromClientName"
        Me.txtFromClientName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFromClientName.Size = New System.Drawing.Size(273, 21)
        Me.txtFromClientName.TabIndex = 2
        '
        'cmdFromClientSelect
        '
        Me.cmdFromClientSelect.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFromClientSelect.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFromClientSelect.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFromClientSelect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFromClientSelect.Location = New System.Drawing.Point(350, 28)
        Me.cmdFromClientSelect.Name = "cmdFromClientSelect"
        Me.cmdFromClientSelect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFromClientSelect.Size = New System.Drawing.Size(33, 21)
        Me.cmdFromClientSelect.TabIndex = 1
        Me.cmdFromClientSelect.Text = "..."
        Me.cmdFromClientSelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFromClientSelect.UseVisualStyleBackColor = False
        '
        'txtFromClientCode
        '
        Me.txtFromClientCode.AcceptsReturn = True
        Me.txtFromClientCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtFromClientCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFromClientCode.Enabled = False
        Me.txtFromClientCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFromClientCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFromClientCode.Location = New System.Drawing.Point(112, 28)
        Me.txtFromClientCode.MaxLength = 0
        Me.txtFromClientCode.Name = "txtFromClientCode"
        Me.txtFromClientCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFromClientCode.Size = New System.Drawing.Size(233, 21)
        Me.txtFromClientCode.TabIndex = 0
        '
        'lblFromClientType
        '
        Me.lblFromClientType.BackColor = System.Drawing.SystemColors.Control
        Me.lblFromClientType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFromClientType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFromClientType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFromClientType.Location = New System.Drawing.Point(22, 84)
        Me.lblFromClientType.Name = "lblFromClientType"
        Me.lblFromClientType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFromClientType.Size = New System.Drawing.Size(79, 34)
        Me.lblFromClientType.TabIndex = 13
        Me.lblFromClientType.Text = "Client Type:"
        '
        'lblFromClientName
        '
        Me.lblFromClientName.BackColor = System.Drawing.SystemColors.Control
        Me.lblFromClientName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFromClientName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFromClientName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFromClientName.Location = New System.Drawing.Point(50, 59)
        Me.lblFromClientName.Name = "lblFromClientName"
        Me.lblFromClientName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFromClientName.Size = New System.Drawing.Size(51, 19)
        Me.lblFromClientName.TabIndex = 12
        Me.lblFromClientName.Text = "Name:"
        '
        'lblFromClient
        '
        Me.lblFromClient.BackColor = System.Drawing.SystemColors.Control
        Me.lblFromClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFromClient.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFromClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFromClient.Location = New System.Drawing.Point(50, 28)
        Me.lblFromClient.Name = "lblFromClient"
        Me.lblFromClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFromClient.Size = New System.Drawing.Size(51, 25)
        Me.lblFromClient.TabIndex = 11
        Me.lblFromClient.Text = "Client:"
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(416, 299)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me._fraToClient_0)
        Me.Controls.Add(Me._fraFromClient_0)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Client Portfolio Transfer - Select Clients"
        Me._fraToClient_0.ResumeLayout(False)
        Me._fraToClient_0.PerformLayout()
        Me._fraFromClient_0.ResumeLayout(False)
        Me._fraFromClient_0.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
	Sub InitializefraToClient()
		Me.fraToClient(0) = _fraToClient_0
	End Sub
	Sub InitializefraFromClient()
		Me.fraFromClient(0) = _fraFromClient_0
	End Sub
#End Region 
End Class