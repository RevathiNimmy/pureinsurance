<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAddPeril
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
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents lblPeril As System.Windows.Forms.Label
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents cmbPeril As System.Windows.Forms.ComboBox
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Private WithEvents _tabAddPeril_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabAddPeril As System.Windows.Forms.TabControl
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.tabAddPeril = New System.Windows.Forms.TabControl
        Me._tabAddPeril_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblPeril = New System.Windows.Forms.Label
        Me.lblDescription = New System.Windows.Forms.Label
        Me.cmbPeril = New System.Windows.Forms.ComboBox
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.tabAddPeril.SuspendLayout()
        Me._tabAddPeril_TabPage0.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(191, 141)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 4
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(271, 141)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 3
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'tabAddPeril
        '
        Me.tabAddPeril.Controls.Add(Me._tabAddPeril_TabPage0)
        Me.tabAddPeril.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabAddPeril.ItemSize = New System.Drawing.Size(111, 18)
        Me.tabAddPeril.Location = New System.Drawing.Point(8, 8)
        Me.tabAddPeril.Multiline = True
        Me.tabAddPeril.Name = "tabAddPeril"
        Me.tabAddPeril.SelectedIndex = 0
        Me.tabAddPeril.Size = New System.Drawing.Size(340, 126)
        Me.tabAddPeril.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.tabAddPeril.TabIndex = 0
        '
        '_tabAddPeril_TabPage0
        '
        Me._tabAddPeril_TabPage0.Controls.Add(Me.lblPeril)
        Me._tabAddPeril_TabPage0.Controls.Add(Me.lblDescription)
        Me._tabAddPeril_TabPage0.Controls.Add(Me.cmbPeril)
        Me._tabAddPeril_TabPage0.Controls.Add(Me.txtDescription)
        Me._tabAddPeril_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabAddPeril_TabPage0.Name = "_tabAddPeril_TabPage0"
        Me._tabAddPeril_TabPage0.Size = New System.Drawing.Size(332, 100)
        Me._tabAddPeril_TabPage0.TabIndex = 0
        Me._tabAddPeril_TabPage0.Text = "1 - Peril"
        '
        'lblPeril
        '
        Me.lblPeril.BackColor = System.Drawing.SystemColors.Control
        Me.lblPeril.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPeril.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPeril.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPeril.Location = New System.Drawing.Point(13, 16)
        Me.lblPeril.Name = "lblPeril"
        Me.lblPeril.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPeril.Size = New System.Drawing.Size(37, 21)
        Me.lblPeril.TabIndex = 2
        Me.lblPeril.Text = "Peril:"
        '
        'lblDescription
        '
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(13, 58)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(69, 21)
        Me.lblDescription.TabIndex = 5
        Me.lblDescription.Text = "Description:"
        '
        'cmbPeril
        '
        Me.cmbPeril.BackColor = System.Drawing.SystemColors.Window
        Me.cmbPeril.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbPeril.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPeril.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbPeril.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbPeril.Location = New System.Drawing.Point(93, 16)
        Me.cmbPeril.Name = "cmbPeril"
        Me.cmbPeril.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbPeril.Size = New System.Drawing.Size(135, 21)
        Me.cmbPeril.TabIndex = 1
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(93, 58)
        Me.txtDescription.MaxLength = 0
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(227, 21)
        Me.txtDescription.TabIndex = 6
        '
        'frmAddPeril
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(354, 172)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.tabAddPeril)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAddPeril"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Add Peril"
        Me.tabAddPeril.ResumeLayout(False)
        Me._tabAddPeril_TabPage0.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class