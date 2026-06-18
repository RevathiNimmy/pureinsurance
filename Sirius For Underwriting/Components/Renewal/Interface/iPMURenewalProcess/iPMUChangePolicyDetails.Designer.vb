<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChangePolicyDetails
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
	Public WithEvents dtpCoverExpiryDate As System.Windows.Forms.DateTimePicker
	Public WithEvents dtpCoverStartDate As System.Windows.Forms.DateTimePicker
	Public WithEvents txtPolicyNum As System.Windows.Forms.TextBox
	Public WithEvents cmdChange As System.Windows.Forms.Button
	Public WithEvents lblPolicyNum As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.dtpCoverExpiryDate = New System.Windows.Forms.DateTimePicker
        Me.dtpCoverStartDate = New System.Windows.Forms.DateTimePicker
        Me.txtPolicyNum = New System.Windows.Forms.TextBox
        Me.cmdChange = New System.Windows.Forms.Button
        Me.lblPolicyNum = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.Frame1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.dtpCoverExpiryDate)
        Me.Frame1.Controls.Add(Me.dtpCoverStartDate)
        Me.Frame1.Controls.Add(Me.txtPolicyNum)
        Me.Frame1.Controls.Add(Me.cmdChange)
        Me.Frame1.Controls.Add(Me.lblPolicyNum)
        Me.Frame1.Controls.Add(Me.Label1)
        Me.Frame1.Controls.Add(Me.Label2)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(3, 3)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(406, 112)
        Me.Frame1.TabIndex = 2
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Policy Details"
        '
        'dtpCoverExpiryDate
        '
        Me.dtpCoverExpiryDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpCoverExpiryDate.Location = New System.Drawing.Point(108, 78)
        Me.dtpCoverExpiryDate.Name = "dtpCoverExpiryDate"
        Me.dtpCoverExpiryDate.Size = New System.Drawing.Size(226, 20)
        Me.dtpCoverExpiryDate.TabIndex = 9
        '
        'dtpCoverStartDate
        '
        Me.dtpCoverStartDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpCoverStartDate.Location = New System.Drawing.Point(108, 48)
        Me.dtpCoverStartDate.Name = "dtpCoverStartDate"
        Me.dtpCoverStartDate.Size = New System.Drawing.Size(226, 20)
        Me.dtpCoverStartDate.TabIndex = 8
        '
        'txtPolicyNum
        '
        Me.txtPolicyNum.AcceptsReturn = True
        Me.txtPolicyNum.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolicyNum.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicyNum.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicyNum.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicyNum.Location = New System.Drawing.Point(108, 18)
        Me.txtPolicyNum.MaxLength = 0
        Me.txtPolicyNum.Name = "txtPolicyNum"
        Me.txtPolicyNum.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicyNum.Size = New System.Drawing.Size(226, 22)
        Me.txtPolicyNum.TabIndex = 4
        '
        'cmdChange
        '
        Me.cmdChange.BackColor = System.Drawing.SystemColors.Control
        Me.cmdChange.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdChange.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdChange.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdChange.Location = New System.Drawing.Point(339, 18)
        Me.cmdChange.Name = "cmdChange"
        Me.cmdChange.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdChange.Size = New System.Drawing.Size(58, 22)
        Me.cmdChange.TabIndex = 3
        Me.cmdChange.Text = "C&hange"
        Me.cmdChange.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdChange.UseVisualStyleBackColor = False
        '
        'lblPolicyNum
        '
        Me.lblPolicyNum.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyNum.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyNum.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyNum.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyNum.Location = New System.Drawing.Point(18, 21)
        Me.lblPolicyNum.Name = "lblPolicyNum"
        Me.lblPolicyNum.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyNum.Size = New System.Drawing.Size(76, 16)
        Me.lblPolicyNum.TabIndex = 7
        Me.lblPolicyNum.Text = "Policy Number"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(18, 51)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(65, 14)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Cover Start Date"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(18, 81)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(75, 14)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Cover Expiry Date"
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(336, 120)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(261, 120)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'frmChangePolicyDetails
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(412, 145)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmChangePolicyDetails"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Change Policy Details"
        Me.Frame1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class