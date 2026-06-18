<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUWRenewal
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializelblOption()
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
	Public WithEvents cboOption1021 As System.Windows.Forms.ComboBox
	Public WithEvents cboOption1022 As System.Windows.Forms.ComboBox
	Public WithEvents chkOption1024 As System.Windows.Forms.CheckBox
	Public WithEvents txtOption1010 As System.Windows.Forms.TextBox
	Public WithEvents chkOption1012 As System.Windows.Forms.CheckBox
	Public WithEvents chkOption1013 As System.Windows.Forms.CheckBox
	Public WithEvents txtOption155 As System.Windows.Forms.TextBox
	Public WithEvents chkOption1036 As System.Windows.Forms.CheckBox
	Public WithEvents chkOption1037 As System.Windows.Forms.CheckBox
	Public WithEvents chkOption1038 As System.Windows.Forms.CheckBox
	Private WithEvents _lblOption_1022 As System.Windows.Forms.Label
	Private WithEvents _lblOption_1021 As System.Windows.Forms.Label
	Private WithEvents _lblOption_4 As System.Windows.Forms.Label
	Private WithEvents _lblOption_0 As System.Windows.Forms.Label
	Public lblOption(1022) As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cboOption1021 = New System.Windows.Forms.ComboBox()
        Me.cboOption1022 = New System.Windows.Forms.ComboBox()
        Me.chkOption1024 = New System.Windows.Forms.CheckBox()
        Me.txtOption1010 = New System.Windows.Forms.TextBox()
        Me.chkOption1012 = New System.Windows.Forms.CheckBox()
        Me.chkOption1013 = New System.Windows.Forms.CheckBox()
        Me.txtOption155 = New System.Windows.Forms.TextBox()
        Me.chkOption1036 = New System.Windows.Forms.CheckBox()
        Me.chkOption1037 = New System.Windows.Forms.CheckBox()
        Me.chkOption1038 = New System.Windows.Forms.CheckBox()
        Me._lblOption_1022 = New System.Windows.Forms.Label()
        Me._lblOption_1021 = New System.Windows.Forms.Label()
        Me._lblOption_4 = New System.Windows.Forms.Label()
        Me._lblOption_0 = New System.Windows.Forms.Label()
        Me.chkOption5262 = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'cboOption1021
        '
        Me.cboOption1021.AccessibleDescription = "Renewal Task Group:"
        Me.cboOption1021.BackColor = System.Drawing.SystemColors.Window
        Me.cboOption1021.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboOption1021.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOption1021.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOption1021.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboOption1021.Location = New System.Drawing.Point(197, 8)
        Me.cboOption1021.Name = "cboOption1021"
        Me.cboOption1021.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboOption1021.Size = New System.Drawing.Size(257, 21)
        Me.cboOption1021.TabIndex = 0
        Me.cboOption1021.Tag = "1021"
        '
        'cboOption1022
        '
        Me.cboOption1022.AccessibleDescription = "Renewal User Group:"
        Me.cboOption1022.BackColor = System.Drawing.SystemColors.Window
        Me.cboOption1022.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboOption1022.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOption1022.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOption1022.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboOption1022.Location = New System.Drawing.Point(197, 33)
        Me.cboOption1022.Name = "cboOption1022"
        Me.cboOption1022.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboOption1022.Size = New System.Drawing.Size(257, 21)
        Me.cboOption1022.TabIndex = 1
        Me.cboOption1022.Tag = "1022"
        '
        'chkOption1024
        '
        Me.chkOption1024.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption1024.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption1024.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption1024.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption1024.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption1024.Location = New System.Drawing.Point(8, 84)
        Me.chkOption1024.Name = "chkOption1024"
        Me.chkOption1024.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption1024.Size = New System.Drawing.Size(203, 16)
        Me.chkOption1024.TabIndex = 3
        Me.chkOption1024.Tag = "1024"
        Me.chkOption1024.Text = "Auto Instalments:"
        Me.chkOption1024.UseVisualStyleBackColor = False
        '
        'txtOption1010
        '
        Me.txtOption1010.AcceptsReturn = True
        Me.txtOption1010.AccessibleDescription = "Renewal Pre Debit Days:"
        Me.txtOption1010.BackColor = System.Drawing.SystemColors.Window
        Me.txtOption1010.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOption1010.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOption1010.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOption1010.Location = New System.Drawing.Point(198, 58)
        Me.txtOption1010.MaxLength = 3
        Me.txtOption1010.Name = "txtOption1010"
        Me.txtOption1010.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOption1010.Size = New System.Drawing.Size(41, 21)
        Me.txtOption1010.TabIndex = 2
        Me.txtOption1010.Tag = "1010,ValidateNumeric"
        Me.txtOption1010.Text = "2"
        '
        'chkOption1012
        '
        Me.chkOption1012.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption1012.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption1012.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption1012.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption1012.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption1012.Location = New System.Drawing.Point(8, 106)
        Me.chkOption1012.Name = "chkOption1012"
        Me.chkOption1012.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption1012.Size = New System.Drawing.Size(203, 37)
        Me.chkOption1012.TabIndex = 4
        Me.chkOption1012.Tag = "1012"
        Me.chkOption1012.Text = "Produce Renewal Status Report:"
        Me.chkOption1012.UseVisualStyleBackColor = False
        '
        'chkOption1013
        '
        Me.chkOption1013.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption1013.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption1013.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption1013.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption1013.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption1013.Location = New System.Drawing.Point(8, 151)
        Me.chkOption1013.Name = "chkOption1013"
        Me.chkOption1013.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption1013.Size = New System.Drawing.Size(203, 21)
        Me.chkOption1013.TabIndex = 5
        Me.chkOption1013.Tag = "1013"
        Me.chkOption1013.Text = "Produce Agent Renewal List:"
        Me.chkOption1013.UseVisualStyleBackColor = False
        '
        'txtOption155
        '
        Me.txtOption155.AcceptsReturn = True
        Me.txtOption155.AccessibleDescription = "Exchange Rate Message:"
        Me.txtOption155.BackColor = System.Drawing.SystemColors.Window
        Me.txtOption155.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOption155.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOption155.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOption155.Location = New System.Drawing.Point(198, 254)
        Me.txtOption155.MaxLength = 3
        Me.txtOption155.Name = "txtOption155"
        Me.txtOption155.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOption155.Size = New System.Drawing.Size(257, 21)
        Me.txtOption155.TabIndex = 9
        Me.txtOption155.Tag = "155"
        '
        'chkOption1036
        '
        Me.chkOption1036.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption1036.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption1036.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption1036.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption1036.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption1036.Location = New System.Drawing.Point(8, 177)
        Me.chkOption1036.Name = "chkOption1036"
        Me.chkOption1036.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption1036.Size = New System.Drawing.Size(203, 21)
        Me.chkOption1036.TabIndex = 6
        Me.chkOption1036.Tag = "1036"
        Me.chkOption1036.Text = "Renewal Schedule Printing:"
        Me.chkOption1036.UseVisualStyleBackColor = False
        '
        'chkOption1037
        '
        Me.chkOption1037.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption1037.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption1037.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption1037.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption1037.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption1037.Location = New System.Drawing.Point(8, 203)
        Me.chkOption1037.Name = "chkOption1037"
        Me.chkOption1037.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption1037.Size = New System.Drawing.Size(203, 21)
        Me.chkOption1037.TabIndex = 7
        Me.chkOption1037.Tag = "1037"
        Me.chkOption1037.Text = "Renewal Certificate Printing:"
        Me.chkOption1037.UseVisualStyleBackColor = False
        '
        'chkOption1038
        '
        Me.chkOption1038.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption1038.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption1038.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption1038.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption1038.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption1038.Location = New System.Drawing.Point(8, 229)
        Me.chkOption1038.Name = "chkOption1038"
        Me.chkOption1038.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption1038.Size = New System.Drawing.Size(203, 19)
        Me.chkOption1038.TabIndex = 8
        Me.chkOption1038.Tag = "1038"
        Me.chkOption1038.Text = "Renewal Debit Note Printing:"
        Me.chkOption1038.UseVisualStyleBackColor = False
        '
        '_lblOption_1022
        '
        Me._lblOption_1022.AutoSize = True
        Me._lblOption_1022.BackColor = System.Drawing.SystemColors.Control
        Me._lblOption_1022.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblOption_1022.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblOption_1022.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblOption_1022.Location = New System.Drawing.Point(8, 37)
        Me._lblOption_1022.Name = "_lblOption_1022"
        Me._lblOption_1022.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblOption_1022.Size = New System.Drawing.Size(129, 13)
        Me._lblOption_1022.TabIndex = 11
        Me._lblOption_1022.Text = "Renewal User Group:"
        '
        '_lblOption_1021
        '
        Me._lblOption_1021.AutoSize = True
        Me._lblOption_1021.BackColor = System.Drawing.SystemColors.Control
        Me._lblOption_1021.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblOption_1021.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblOption_1021.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblOption_1021.Location = New System.Drawing.Point(8, 12)
        Me._lblOption_1021.Name = "_lblOption_1021"
        Me._lblOption_1021.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblOption_1021.Size = New System.Drawing.Size(129, 13)
        Me._lblOption_1021.TabIndex = 10
        Me._lblOption_1021.Text = "Renewal Task Group:"
        '
        '_lblOption_4
        '
        Me._lblOption_4.AutoSize = True
        Me._lblOption_4.BackColor = System.Drawing.SystemColors.Control
        Me._lblOption_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblOption_4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblOption_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblOption_4.Location = New System.Drawing.Point(8, 62)
        Me._lblOption_4.Name = "_lblOption_4"
        Me._lblOption_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblOption_4.Size = New System.Drawing.Size(150, 13)
        Me._lblOption_4.TabIndex = 12
        Me._lblOption_4.Text = "Renewal Pre Debit Days:"
        '
        '_lblOption_0
        '
        Me._lblOption_0.AutoSize = True
        Me._lblOption_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblOption_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblOption_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblOption_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblOption_0.Location = New System.Drawing.Point(12, 257)
        Me._lblOption_0.Name = "_lblOption_0"
        Me._lblOption_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblOption_0.Size = New System.Drawing.Size(150, 13)
        Me._lblOption_0.TabIndex = 13
        Me._lblOption_0.Text = "Exchange Rate Message:"
        '
        'chkOption5262
        '
        Me.chkOption5262.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption5262.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5262.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption5262.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption5262.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption5262.Location = New System.Drawing.Point(12, 270)
        Me.chkOption5262.Name = "chkOption5262"
        Me.chkOption5262.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption5262.Size = New System.Drawing.Size(200, 54)
        Me.chkOption5262.TabIndex = 14
        Me.chkOption5262.Tag = "5262"
        Me.chkOption5262.Text = "Apply MTA Tax Rates on OOS Renewal:"
        Me.chkOption5262.UseVisualStyleBackColor = False
        '
        'frmUWRenewal
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(535, 380)
        Me.Controls.Add(Me.cboOption1021)
        Me.Controls.Add(Me.cboOption1022)
        Me.Controls.Add(Me.chkOption1024)
        Me.Controls.Add(Me.txtOption1010)
        Me.Controls.Add(Me.chkOption1012)
        Me.Controls.Add(Me.chkOption1013)
        Me.Controls.Add(Me.txtOption155)
        Me.Controls.Add(Me.chkOption1036)
        Me.Controls.Add(Me.chkOption1037)
        Me.Controls.Add(Me.chkOption1038)
        Me.Controls.Add(Me._lblOption_1022)
        Me.Controls.Add(Me._lblOption_1021)
        Me.Controls.Add(Me._lblOption_4)
        Me.Controls.Add(Me._lblOption_0)
        Me.Controls.Add(Me.chkOption5262)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmUWRenewal"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializelblOption()
        Me.lblOption(1022) = _lblOption_1022
        Me.lblOption(1021) = _lblOption_1021
        Me.lblOption(4) = _lblOption_4
        Me.lblOption(0) = _lblOption_0
    End Sub
    Public WithEvents chkOption5262 As CheckBox
#End Region
End Class