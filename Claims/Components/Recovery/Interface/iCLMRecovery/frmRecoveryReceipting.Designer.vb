<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRecoveryReceipting
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
	Public WithEvents uctCLMReceipt As uctCLMReceiptControl.uctCLMReceipt
	Public WithEvents cmcCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.uctCLMReceipt = New uctCLMReceiptControl.uctCLMReceipt
        Me.cmcCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'uctCLMReceipt
        '
        Me.uctCLMReceipt.ClaimID = 0
        Me.uctCLMReceipt.ClaimPerilId = 0
        Me.uctCLMReceipt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctCLMReceipt.Location = New System.Drawing.Point(0, 0)
        Me.uctCLMReceipt.Name = "uctCLMReceipt"
        Me.uctCLMReceipt.Size = New System.Drawing.Size(881, 489)
        Me.uctCLMReceipt.TabIndex = 2
        '
        'cmcCancel
        '
        Me.cmcCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmcCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmcCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmcCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmcCancel.Location = New System.Drawing.Point(808, 496)
        Me.cmcCancel.Name = "cmcCancel"
        Me.cmcCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmcCancel.Size = New System.Drawing.Size(73, 21)
        Me.cmcCancel.TabIndex = 1
        Me.cmcCancel.Text = "&Cancel"
        Me.cmcCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmcCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(728, 496)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 21)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'frmRecoveryReceipting
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(882, 519)
        Me.Controls.Add(Me.uctCLMReceipt)
        Me.Controls.Add(Me.cmcCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 30)
        Me.Name = "frmRecoveryReceipting"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Recovery Receipting"
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class