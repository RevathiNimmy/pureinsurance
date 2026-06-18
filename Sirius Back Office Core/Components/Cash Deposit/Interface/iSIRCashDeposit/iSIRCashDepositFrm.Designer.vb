<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
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
	Public WithEvents uctCashDepositControl As uctCashDepositControl.CashDeposit
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents txtPartyName As System.Windows.Forms.TextBox
	Public WithEvents txtPartyCode As System.Windows.Forms.TextBox
	Public WithEvents lblPartyName As System.Windows.Forms.Label
	Public WithEvents lblPartyCode As System.Windows.Forms.Label
	Public WithEvents fraPartyDetails As System.Windows.Forms.GroupBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.uctCashDepositControl = New uctCashDepositControl.CashDeposit
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.fraPartyDetails = New System.Windows.Forms.GroupBox
        Me.txtPartyName = New System.Windows.Forms.TextBox
        Me.txtPartyCode = New System.Windows.Forms.TextBox
        Me.lblPartyName = New System.Windows.Forms.Label
        Me.lblPartyCode = New System.Windows.Forms.Label
        Me.fraPartyDetails.SuspendLayout()
        Me.SuspendLayout()
        '
        'uctCashDepositControl
        '
        Me.uctCashDepositControl.BankCode = ""
        Me.uctCashDepositControl.CashDepositRef = ""
        Me.uctCashDepositControl.FindCashDepositRef = ""
        Me.uctCashDepositControl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctCashDepositControl.FromAgentOrClientMaintenance = False
        Me.uctCashDepositControl.IsClient = False
        Me.uctCashDepositControl.IsFind = False
        Me.uctCashDepositControl.Location = New System.Drawing.Point(16, 98)
        Me.uctCashDepositControl.Name = "uctCashDepositControl"
        Me.uctCashDepositControl.PartyCnt = 0
        Me.uctCashDepositControl.PartyCode = ""
        Me.uctCashDepositControl.PartyName = ""
        Me.uctCashDepositControl.Size = New System.Drawing.Size(640, 427)
        Me.uctCashDepositControl.TabIndex = 7
        Me.uctCashDepositControl.Task = 0
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(558, 534)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(86, 26)
        Me.cmdCancel.TabIndex = 6
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(460, 534)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(86, 26)
        Me.cmdOK.TabIndex = 5
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'fraPartyDetails
        '
        Me.fraPartyDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraPartyDetails.Controls.Add(Me.txtPartyName)
        Me.fraPartyDetails.Controls.Add(Me.txtPartyCode)
        Me.fraPartyDetails.Controls.Add(Me.lblPartyName)
        Me.fraPartyDetails.Controls.Add(Me.lblPartyCode)
        Me.fraPartyDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPartyDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPartyDetails.Location = New System.Drawing.Point(16, 16)
        Me.fraPartyDetails.Name = "fraPartyDetails"
        Me.fraPartyDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPartyDetails.Size = New System.Drawing.Size(629, 76)
        Me.fraPartyDetails.TabIndex = 0
        Me.fraPartyDetails.TabStop = False
        Me.fraPartyDetails.Text = "Party Details"
        '
        'txtPartyName
        '
        Me.txtPartyName.AcceptsReturn = True
        Me.txtPartyName.BackColor = System.Drawing.SystemColors.Window
        Me.txtPartyName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPartyName.Enabled = False
        Me.txtPartyName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPartyName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPartyName.Location = New System.Drawing.Point(415, 24)
        Me.txtPartyName.MaxLength = 0
        Me.txtPartyName.Name = "txtPartyName"
        Me.txtPartyName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPartyName.Size = New System.Drawing.Size(136, 26)
        Me.txtPartyName.TabIndex = 4
        '
        'txtPartyCode
        '
        Me.txtPartyCode.AcceptsReturn = True
        Me.txtPartyCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtPartyCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPartyCode.Enabled = False
        Me.txtPartyCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPartyCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPartyCode.Location = New System.Drawing.Point(115, 25)
        Me.txtPartyCode.MaxLength = 0
        Me.txtPartyCode.Name = "txtPartyCode"
        Me.txtPartyCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPartyCode.Size = New System.Drawing.Size(136, 26)
        Me.txtPartyCode.TabIndex = 2
        '
        'lblPartyName
        '
        Me.lblPartyName.AutoSize = True
        Me.lblPartyName.BackColor = System.Drawing.SystemColors.Control
        Me.lblPartyName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPartyName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartyName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPartyName.Location = New System.Drawing.Point(285, 30)
        Me.lblPartyName.Name = "lblPartyName"
        Me.lblPartyName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPartyName.Size = New System.Drawing.Size(76, 13)
        Me.lblPartyName.TabIndex = 3
        Me.lblPartyName.Text = "Party Name:"
        '
        'lblPartyCode
        '
        Me.lblPartyCode.AutoSize = True
        Me.lblPartyCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblPartyCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPartyCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartyCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPartyCode.Location = New System.Drawing.Point(20, 30)
        Me.lblPartyCode.Name = "lblPartyCode"
        Me.lblPartyCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPartyCode.Size = New System.Drawing.Size(73, 13)
        Me.lblPartyCode.TabIndex = 1
        Me.lblPartyCode.Text = "Party Code:"
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(7, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(672, 575)
        Me.Controls.Add(Me.uctCashDepositControl)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.fraPartyDetails)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(10, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Cash Deposit Accounts"
        Me.fraPartyDetails.ResumeLayout(False)
        Me.fraPartyDetails.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class