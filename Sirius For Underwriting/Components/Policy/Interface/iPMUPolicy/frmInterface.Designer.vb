<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
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
    '<System.Diagnostics.DebuggerNonUserCode()> _
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
	Public WithEvents cmdDocArchive As System.Windows.Forms.Button
	Public WithEvents uctPMUPolicyControl1 As PMUPolicyControl.uctPMUPolicyControl
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdLapseQuote As System.Windows.Forms.Button
	Public WithEvents cmdCommission As System.Windows.Forms.Button
	Public WithEvents cmdReInsurance As System.Windows.Forms.Button
	Public WithEvents cmdFee As System.Windows.Forms.Button
	Public WithEvents cmdPolicyTax As System.Windows.Forms.Button
	Public WithEvents cmdInstalment As System.Windows.Forms.Button
	Public WithEvents cmdMTACancel As System.Windows.Forms.Button
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
    '<System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdDocArchive = New System.Windows.Forms.Button()
        Me.uctPMUPolicyControl1 = New PMUPolicyControl.uctPMUPolicyControl()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdLapseQuote = New System.Windows.Forms.Button()
        Me.cmdCommission = New System.Windows.Forms.Button()
        Me.cmdReInsurance = New System.Windows.Forms.Button()
        Me.cmdFee = New System.Windows.Forms.Button()
        Me.cmdPolicyTax = New System.Windows.Forms.Button()
        Me.cmdInstalment = New System.Windows.Forms.Button()
        Me.cmdMTACancel = New System.Windows.Forms.Button()
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog()
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog()
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog()
        Me.SuspendLayout()
        '
        'cmdDocArchive
        '
        Me.cmdDocArchive.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDocArchive.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDocArchive.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDocArchive.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDocArchive.Location = New System.Drawing.Point(0, 557)
        Me.cmdDocArchive.Name = "cmdDocArchive"
        Me.cmdDocArchive.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDocArchive.Size = New System.Drawing.Size(92, 22)
        Me.cmdDocArchive.TabIndex = 7
        Me.cmdDocArchive.Text = "&Doc Archive"
        Me.cmdDocArchive.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDocArchive.UseVisualStyleBackColor = False
        '
        'uctPMUPolicyControl1
        '
        Me.uctPMUPolicyControl1.BackDatedMTAsAllowed = False
        Me.uctPMUPolicyControl1.BackStyle = 0
        Me.uctPMUPolicyControl1.BorderStyle = 0
        Me.uctPMUPolicyControl1.BusinessTypeId = Nothing
        Me.uctPMUPolicyControl1.Enabled = False
        Me.uctPMUPolicyControl1.EventRaised = False
        Me.uctPMUPolicyControl1.FromEvent = False
        Me.uctPMUPolicyControl1.InsuranceFileCnt = 0
        Me.uctPMUPolicyControl1.InsuranceFolderCnt = 0
        Me.uctPMUPolicyControl1.IsExit = False
        Me.uctPMUPolicyControl1.IsMTATemp = "False"
        Me.uctPMUPolicyControl1.IsPriorDate = False
        Me.uctPMUPolicyControl1.IsRenewal = False
        Me.uctPMUPolicyControl1.IsRenewed = False
        Me.uctPMUPolicyControl1.LapseAQuote = False
        Me.uctPMUPolicyControl1.LapsedDate = New Date(CType(0, Long))
        Me.uctPMUPolicyControl1.Location = New System.Drawing.Point(0, 0)
        Me.uctPMUPolicyControl1.Name = "uctPMUPolicyControl1"
        Me.uctPMUPolicyControl1.PartyCnt = 0
        Me.uctPMUPolicyControl1.PMRaiseEvent = False
        Me.uctPMUPolicyControl1.PolicyTypeId = Nothing
        Me.uctPMUPolicyControl1.ProductId = 0
        Me.uctPMUPolicyControl1.Renewaldate = New Date(CType(0, Long))
        Me.uctPMUPolicyControl1.RiskCodeId = Nothing
        Me.uctPMUPolicyControl1.RiskGroupId = Nothing
        Me.uctPMUPolicyControl1.SelectedPolicyStatus = ""
        Me.uctPMUPolicyControl1.SetQuoteToLapsed = False
        Me.uctPMUPolicyControl1.Size = New System.Drawing.Size(609, 525)
        Me.uctPMUPolicyControl1.SourceId = 0
        Me.uctPMUPolicyControl1.Status = 0
        Me.uctPMUPolicyControl1.TabIndex = 0
        Me.uctPMUPolicyControl1.Task = 0
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(416, 555)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(61, 22)
        Me.cmdOK.TabIndex = 8
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(480, 555)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(61, 22)
        Me.cmdCancel.TabIndex = 9
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(544, 555)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(61, 22)
        Me.cmdHelp.TabIndex = 10
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdLapseQuote
        '
        Me.cmdLapseQuote.BackColor = System.Drawing.SystemColors.Control
        Me.cmdLapseQuote.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdLapseQuote.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdLapseQuote.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdLapseQuote.Location = New System.Drawing.Point(0, 531)
        Me.cmdLapseQuote.Name = "cmdLapseQuote"
        Me.cmdLapseQuote.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdLapseQuote.Size = New System.Drawing.Size(92, 22)
        Me.cmdLapseQuote.TabIndex = 1
        Me.cmdLapseQuote.Text = "&Lapse Quote"
        Me.cmdLapseQuote.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdLapseQuote.UseVisualStyleBackColor = False
        Me.cmdLapseQuote.Visible = False
        '
        'cmdCommission
        '
        Me.cmdCommission.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCommission.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCommission.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdCommission.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCommission.Location = New System.Drawing.Point(308, 531)
        Me.cmdCommission.Name = "cmdCommission"
        Me.cmdCommission.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCommission.Size = New System.Drawing.Size(92, 22)
        Me.cmdCommission.TabIndex = 4
        Me.cmdCommission.Text = "Co&mmission"
        Me.cmdCommission.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCommission.UseVisualStyleBackColor = False
        '
        'cmdReInsurance
        '
        Me.cmdReInsurance.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReInsurance.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReInsurance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdReInsurance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReInsurance.Location = New System.Drawing.Point(410, 531)
        Me.cmdReInsurance.Name = "cmdReInsurance"
        Me.cmdReInsurance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReInsurance.Size = New System.Drawing.Size(92, 22)
        Me.cmdReInsurance.TabIndex = 5
        Me.cmdReInsurance.Text = "&ReInsurance"
        Me.cmdReInsurance.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdReInsurance.UseVisualStyleBackColor = False
        Me.cmdReInsurance.Visible = False
        '
        'cmdFee
        '
        Me.cmdFee.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFee.CausesValidation = False
        Me.cmdFee.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFee.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdFee.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFee.Location = New System.Drawing.Point(205, 531)
        Me.cmdFee.Name = "cmdFee"
        Me.cmdFee.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFee.Size = New System.Drawing.Size(92, 22)
        Me.cmdFee.TabIndex = 3
        Me.cmdFee.Text = "Policy &Fee"
        Me.cmdFee.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFee.UseVisualStyleBackColor = False
        '
        'cmdPolicyTax
        '
        Me.cmdPolicyTax.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPolicyTax.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPolicyTax.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdPolicyTax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPolicyTax.Location = New System.Drawing.Point(103, 531)
        Me.cmdPolicyTax.Name = "cmdPolicyTax"
        Me.cmdPolicyTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPolicyTax.Size = New System.Drawing.Size(92, 22)
        Me.cmdPolicyTax.TabIndex = 2
        Me.cmdPolicyTax.Text = "Policy &Tax"
        Me.cmdPolicyTax.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPolicyTax.UseVisualStyleBackColor = False
        '
        'cmdInstalment
        '
        Me.cmdInstalment.BackColor = System.Drawing.SystemColors.Control
        Me.cmdInstalment.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdInstalment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdInstalment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdInstalment.Location = New System.Drawing.Point(512, 531)
        Me.cmdInstalment.Name = "cmdInstalment"
        Me.cmdInstalment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdInstalment.Size = New System.Drawing.Size(92, 22)
        Me.cmdInstalment.TabIndex = 6
        Me.cmdInstalment.Text = "&Instalment"
        Me.cmdInstalment.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdInstalment.UseVisualStyleBackColor = False
        '
        'cmdMTACancel
        '
        Me.cmdMTACancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMTACancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMTACancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdMTACancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMTACancel.Location = New System.Drawing.Point(0, 531)
        Me.cmdMTACancel.Name = "cmdMTACancel"
        Me.cmdMTACancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMTACancel.Size = New System.Drawing.Size(92, 22)
        Me.cmdMTACancel.TabIndex = 3
        Me.cmdMTACancel.Text = "Cancel &MTA"
        Me.cmdMTACancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdMTACancel.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(612, 579)
        Me.Controls.Add(Me.cmdDocArchive)
        Me.Controls.Add(Me.uctPMUPolicyControl1)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdLapseQuote)
        Me.Controls.Add(Me.cmdCommission)
        Me.Controls.Add(Me.cmdReInsurance)
        Me.Controls.Add(Me.cmdFee)
        Me.Controls.Add(Me.cmdPolicyTax)
        Me.Controls.Add(Me.cmdInstalment)
        Me.Controls.Add(Me.cmdMTACancel)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Policy"
        Me.ResumeLayout(False)

End Sub
#End Region 
End Class