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

    Public WithEvents uctBankGuarenteeControl As uctBGControl.uctBankGuarenteeControl
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents cmdAppy As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOk = New System.Windows.Forms.Button
        Me.cmdAppy = New System.Windows.Forms.Button
        Me.uctBankGuarenteeControl = New uctBGControl.uctBankGuarenteeControl
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(638, 460)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(103, 23)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.Text = "C&ancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOk
        '
        Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOk.Location = New System.Drawing.Point(530, 460)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(103, 23)
        Me.cmdOk.TabIndex = 1
        Me.cmdOk.Text = "&Ok"
        Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOk.UseVisualStyleBackColor = False
        '
        'cmdAppy
        '
        Me.cmdAppy.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAppy.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAppy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAppy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAppy.Location = New System.Drawing.Point(422, 460)
        Me.cmdAppy.Name = "cmdAppy"
        Me.cmdAppy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAppy.Size = New System.Drawing.Size(103, 23)
        Me.cmdAppy.TabIndex = 0
        Me.cmdAppy.Text = "&Apply"
        Me.cmdAppy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAppy.UseVisualStyleBackColor = False
        '
        'uctBankGuarenteeControl
        '
        Me.uctBankGuarenteeControl.AccountId = CType(0, Byte)
        Me.uctBankGuarenteeControl.BankGuaranteeDetails = Nothing
        Me.uctBankGuarenteeControl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctBankGuarenteeControl.Location = New System.Drawing.Point(0, 0)
        Me.uctBankGuarenteeControl.Name = "uctBankGuarenteeControl"
        Me.uctBankGuarenteeControl.PartyCnt = CType(0, Byte)
        Me.uctBankGuarenteeControl.PartyCode = ""
        Me.uctBankGuarenteeControl.PartyName = ""
        Me.uctBankGuarenteeControl.SelBgId = 0
        Me.uctBankGuarenteeControl.SelectedBranches = Nothing
        Me.uctBankGuarenteeControl.SelectedProducts = Nothing
        Me.uctBankGuarenteeControl.Size = New System.Drawing.Size(749, 452)
        Me.uctBankGuarenteeControl.SystemCurrency = 0
        Me.uctBankGuarenteeControl.TabIndex = 3
        Me.uctBankGuarenteeControl.Task = 0
        Me.uctBankGuarenteeControl.ViewMode = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(752, 487)
        Me.Controls.Add(Me.uctBankGuarenteeControl)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.cmdAppy)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Bank Guarantee Details"
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class