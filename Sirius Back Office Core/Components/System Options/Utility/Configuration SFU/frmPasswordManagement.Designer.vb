<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPasswordManagement
    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        'InitializelblOption5061()
        'InitializelblOption15()
    End Sub
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lblPswrdStrRegx = New System.Windows.Forms.Label()
        Me.txtPswrdStrRegx = New System.Windows.Forms.TextBox()
        Me.lblPswrdFailMsg = New System.Windows.Forms.Label()
        Me.txtlblPswrdFailMsg = New System.Windows.Forms.TextBox()
        Me.lblPswrdExpDur = New System.Windows.Forms.Label()
        Me.lblPswrdHis = New System.Windows.Forms.Label()
        Me.lblLockLim = New System.Windows.Forms.Label()
        Me.txtPswrdExpDur = New System.Windows.Forms.TextBox()
        Me.txtPswrdHis = New System.Windows.Forms.TextBox()
        Me.txtLockLim = New System.Windows.Forms.TextBox()
        Me.txtTmpPswrdExp = New System.Windows.Forms.TextBox()
        Me.lblTmpPswrdExp = New System.Windows.Forms.Label()
        Me.lblPswrdExpWarn = New System.Windows.Forms.Label()
        Me.txtPswrdExpWarn = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'lblPswrdStrRegx
        '
        Me.lblPswrdStrRegx.BackColor = System.Drawing.SystemColors.Control
        Me.lblPswrdStrRegx.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPswrdStrRegx.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPswrdStrRegx.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPswrdStrRegx.Location = New System.Drawing.Point(23, 11)
        Me.lblPswrdStrRegx.Name = "lblPswrdStrRegx"
        Me.lblPswrdStrRegx.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPswrdStrRegx.Size = New System.Drawing.Size(244, 18)
        Me.lblPswrdStrRegx.TabIndex = 3
        Me.lblPswrdStrRegx.Tag = "5100"
        Me.lblPswrdStrRegx.Text = "Password Strength Regular Expression:"
        '
        'txtPswrdStrRegx
        '
        Me.txtPswrdStrRegx.AcceptsReturn = True
        Me.txtPswrdStrRegx.AccessibleDescription = "Password Strength Regular Expression"
        Me.txtPswrdStrRegx.BackColor = System.Drawing.SystemColors.Window
        Me.txtPswrdStrRegx.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPswrdStrRegx.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPswrdStrRegx.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPswrdStrRegx.Location = New System.Drawing.Point(290, 8)
        Me.txtPswrdStrRegx.Name = "txtPswrdStrRegx"
        Me.txtPswrdStrRegx.Size = New System.Drawing.Size(293, 21)
        Me.txtPswrdStrRegx.TabIndex = 2
        Me.txtPswrdStrRegx.Tag = "5101,Regex"
        Me.txtPswrdStrRegx.Text = "^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,15}$"
        '
        'lblPswrdFailMsg
        '
        Me.lblPswrdFailMsg.BackColor = System.Drawing.SystemColors.Control
        Me.lblPswrdFailMsg.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPswrdFailMsg.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPswrdFailMsg.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPswrdFailMsg.Location = New System.Drawing.Point(23, 39)
        Me.lblPswrdFailMsg.Name = "lblPswrdFailMsg"
        Me.lblPswrdFailMsg.Size = New System.Drawing.Size(220, 23)
        Me.lblPswrdFailMsg.TabIndex = 3
        Me.lblPswrdFailMsg.Tag = "5112"
        Me.lblPswrdFailMsg.Text = "Password Failure Message Text:"
        '
        'txtlblPswrdFailMsg
        '
        Me.txtlblPswrdFailMsg.AcceptsReturn = True
        Me.txtlblPswrdFailMsg.AccessibleDescription = "Password Failure Message Text"
        Me.txtlblPswrdFailMsg.BackColor = System.Drawing.SystemColors.Window
        Me.txtlblPswrdFailMsg.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtlblPswrdFailMsg.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtlblPswrdFailMsg.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtlblPswrdFailMsg.Location = New System.Drawing.Point(290, 36)
        Me.txtlblPswrdFailMsg.Multiline = True
        Me.txtlblPswrdFailMsg.Name = "txtlblPswrdFailMsg"
        Me.txtlblPswrdFailMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtlblPswrdFailMsg.Size = New System.Drawing.Size(293, 72)
        Me.txtlblPswrdFailMsg.TabIndex = 4
        Me.txtlblPswrdFailMsg.Tag = "5113,FailureMsg"
        Me.txtlblPswrdFailMsg.Text = "Password must be between eight and fifteen characters in length, be a mix of uppe" &
    "r and lowercase letters, and contain at least one number. Special characters are" &
    " not permitted"
        '
        'lblPswrdExpDur
        '
        Me.lblPswrdExpDur.AutoSize = True
        Me.lblPswrdExpDur.BackColor = System.Drawing.SystemColors.Control
        Me.lblPswrdExpDur.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPswrdExpDur.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPswrdExpDur.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPswrdExpDur.Location = New System.Drawing.Point(23, 119)
        Me.lblPswrdExpDur.Name = "lblPswrdExpDur"
        Me.lblPswrdExpDur.Size = New System.Drawing.Size(202, 13)
        Me.lblPswrdExpDur.TabIndex = 5
        Me.lblPswrdExpDur.Tag = "5102,,M"
        Me.lblPswrdExpDur.Text = "Password Expiry Duration (Days):"
        '
        'lblPswrdHis
        '
        Me.lblPswrdHis.AutoSize = True
        Me.lblPswrdHis.BackColor = System.Drawing.SystemColors.Control
        Me.lblPswrdHis.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPswrdHis.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPswrdHis.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPswrdHis.Location = New System.Drawing.Point(23, 147)
        Me.lblPswrdHis.Name = "lblPswrdHis"
        Me.lblPswrdHis.Size = New System.Drawing.Size(110, 13)
        Me.lblPswrdHis.TabIndex = 6
        Me.lblPswrdHis.Tag = "5104,,M"
        Me.lblPswrdHis.Text = "Password History:"
        '
        'lblLockLim
        '
        Me.lblLockLim.AutoSize = True
        Me.lblLockLim.BackColor = System.Drawing.SystemColors.Control
        Me.lblLockLim.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLockLim.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLockLim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLockLim.Location = New System.Drawing.Point(23, 175)
        Me.lblLockLim.Name = "lblLockLim"
        Me.lblLockLim.Size = New System.Drawing.Size(203, 13)
        Me.lblLockLim.TabIndex = 7
        Me.lblLockLim.Tag = "5106,,M"
        Me.lblLockLim.Text = "Lock Account after Failed Log-ons:"
        '
        'txtPswrdExpDur
        '
        Me.txtPswrdExpDur.AcceptsReturn = True
        Me.txtPswrdExpDur.AccessibleDescription = "Password Expiry Duration (Days)"
        Me.txtPswrdExpDur.BackColor = System.Drawing.SystemColors.Window
        Me.txtPswrdExpDur.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPswrdExpDur.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPswrdExpDur.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPswrdExpDur.Location = New System.Drawing.Point(290, 116)
        Me.txtPswrdExpDur.MaxLength = 2
        Me.txtPswrdExpDur.Name = "txtPswrdExpDur"
        Me.txtPswrdExpDur.Size = New System.Drawing.Size(106, 21)
        Me.txtPswrdExpDur.TabIndex = 8
        Me.txtPswrdExpDur.TabStop = False
        Me.txtPswrdExpDur.Tag = "5103,ValidateNumeric,M"
        Me.txtPswrdExpDur.Text = "30"
        '
        'txtPswrdHis
        '
        Me.txtPswrdHis.AcceptsReturn = True
        Me.txtPswrdHis.AccessibleDescription = "Password History"
        Me.txtPswrdHis.BackColor = System.Drawing.SystemColors.Window
        Me.txtPswrdHis.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPswrdHis.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPswrdHis.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPswrdHis.Location = New System.Drawing.Point(290, 144)
        Me.txtPswrdHis.MaxLength = 1
        Me.txtPswrdHis.Name = "txtPswrdHis"
        Me.txtPswrdHis.Size = New System.Drawing.Size(106, 21)
        Me.txtPswrdHis.TabIndex = 9
        Me.txtPswrdHis.Tag = "5105,ValidateNumeric,M"
        Me.txtPswrdHis.Text = "3"
        '
        'txtLockLim
        '
        Me.txtLockLim.AcceptsReturn = True
        Me.txtLockLim.AccessibleDescription = "Lock Account after Failed Log-ons"
        Me.txtLockLim.BackColor = System.Drawing.SystemColors.Window
        Me.txtLockLim.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLockLim.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLockLim.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLockLim.Location = New System.Drawing.Point(290, 172)
        Me.txtLockLim.MaxLength = 1
        Me.txtLockLim.Name = "txtLockLim"
        Me.txtLockLim.Size = New System.Drawing.Size(106, 21)
        Me.txtLockLim.TabIndex = 10
        Me.txtLockLim.Tag = "5107,ValidateNumeric,M"
        Me.txtLockLim.Text = "3"
        '
        'txtTmpPswrdExp
        '
        Me.txtTmpPswrdExp.AcceptsReturn = True
        Me.txtTmpPswrdExp.AccessibleDescription = "Temporary Password Validity Duration(days)"
        Me.txtTmpPswrdExp.BackColor = System.Drawing.SystemColors.Window
        Me.txtTmpPswrdExp.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTmpPswrdExp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTmpPswrdExp.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTmpPswrdExp.Location = New System.Drawing.Point(290, 200)
        Me.txtTmpPswrdExp.MaxLength = 1
        Me.txtTmpPswrdExp.Name = "txtTmpPswrdExp"
        Me.txtTmpPswrdExp.Size = New System.Drawing.Size(106, 21)
        Me.txtTmpPswrdExp.TabIndex = 11
        Me.txtTmpPswrdExp.Tag = "5109,ValidateNumeric,M"
        Me.txtTmpPswrdExp.Text = "1"
        '
        'lblTmpPswrdExp
        '
        Me.lblTmpPswrdExp.AutoSize = True
        Me.lblTmpPswrdExp.BackColor = System.Drawing.SystemColors.Control
        Me.lblTmpPswrdExp.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTmpPswrdExp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTmpPswrdExp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTmpPswrdExp.Location = New System.Drawing.Point(23, 203)
        Me.lblTmpPswrdExp.Name = "lblTmpPswrdExp"
        Me.lblTmpPswrdExp.Size = New System.Drawing.Size(267, 13)
        Me.lblTmpPswrdExp.TabIndex = 12
        Me.lblTmpPswrdExp.Tag = "5108,,M"
        Me.lblTmpPswrdExp.Text = "Temporary Password Validity Duration(days):"
        '
        'lblPswrdExpWarn
        '
        Me.lblPswrdExpWarn.AutoSize = True
        Me.lblPswrdExpWarn.BackColor = System.Drawing.SystemColors.Control
        Me.lblPswrdExpWarn.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPswrdExpWarn.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPswrdExpWarn.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPswrdExpWarn.Location = New System.Drawing.Point(23, 231)
        Me.lblPswrdExpWarn.Name = "lblPswrdExpWarn"
        Me.lblPswrdExpWarn.Size = New System.Drawing.Size(200, 13)
        Me.lblPswrdExpWarn.TabIndex = 13
        Me.lblPswrdExpWarn.Tag = "5110,,M"
        Me.lblPswrdExpWarn.Text = "Password expiry warning (days) :"
        '
        'txtPswrdExpWarn
        '
        Me.txtPswrdExpWarn.AcceptsReturn = True
        Me.txtPswrdExpWarn.AccessibleDescription = "Password expiry warning (days)"
        Me.txtPswrdExpWarn.BackColor = System.Drawing.SystemColors.Window
        Me.txtPswrdExpWarn.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPswrdExpWarn.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPswrdExpWarn.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPswrdExpWarn.Location = New System.Drawing.Point(290, 228)
        Me.txtPswrdExpWarn.MaxLength = 2
        Me.txtPswrdExpWarn.Name = "txtPswrdExpWarn"
        Me.txtPswrdExpWarn.Size = New System.Drawing.Size(106, 21)
        Me.txtPswrdExpWarn.TabIndex = 14
        Me.txtPswrdExpWarn.Tag = "5111,ValidateNumeric,M"
        Me.txtPswrdExpWarn.Text = "10"
        '
        'frmPasswordManagement
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(804, 502)
        Me.Controls.Add(Me.txtPswrdExpWarn)
        Me.Controls.Add(Me.lblPswrdExpWarn)
        Me.Controls.Add(Me.lblTmpPswrdExp)
        Me.Controls.Add(Me.txtTmpPswrdExp)
        Me.Controls.Add(Me.txtLockLim)
        Me.Controls.Add(Me.txtPswrdHis)
        Me.Controls.Add(Me.txtPswrdExpDur)
        Me.Controls.Add(Me.lblLockLim)
        Me.Controls.Add(Me.lblPswrdHis)
        Me.Controls.Add(Me.lblPswrdExpDur)
        Me.Controls.Add(Me.lblPswrdFailMsg)
        Me.Controls.Add(Me.txtlblPswrdFailMsg)
        Me.Controls.Add(Me.txtPswrdStrRegx)
        Me.Controls.Add(Me.lblPswrdStrRegx)
        Me.Name = "frmPasswordManagement"
        Me.Text = "frmPasswordManagement"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblPswrdStrRegx As System.Windows.Forms.Label
    Friend WithEvents txtPswrdStrRegx As System.Windows.Forms.TextBox
    Friend WithEvents lblPswrdFailMsg As System.Windows.Forms.Label
    Friend WithEvents txtlblPswrdFailMsg As System.Windows.Forms.TextBox
    Friend WithEvents lblPswrdExpDur As System.Windows.Forms.Label
    Friend WithEvents lblPswrdHis As System.Windows.Forms.Label
    Friend WithEvents lblLockLim As System.Windows.Forms.Label
    Friend WithEvents txtPswrdExpDur As System.Windows.Forms.TextBox
    Friend WithEvents txtPswrdHis As System.Windows.Forms.TextBox
    Friend WithEvents txtLockLim As System.Windows.Forms.TextBox
    Friend WithEvents txtTmpPswrdExp As System.Windows.Forms.TextBox
    Friend WithEvents lblTmpPswrdExp As System.Windows.Forms.Label
    Friend WithEvents lblPswrdExpWarn As System.Windows.Forms.Label
    Friend WithEvents txtPswrdExpWarn As System.Windows.Forms.TextBox
End Class
