<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAdd
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		tabMainTabPreviousTab = tabMainTab.SelectedIndex
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
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents Image1 As System.Windows.Forms.PictureBox
	Public WithEvents lblPeriodPrefix As System.Windows.Forms.Label
	Public WithEvents lblPrevYearEndDate As System.Windows.Forms.Label
	Public WithEvents lblPeriodLength As System.Windows.Forms.Label
	Public WithEvents lblYearName As System.Windows.Forms.Label
	Public WithEvents txtPeriodPrefix As System.Windows.Forms.TextBox
	Public WithEvents txtYearName As System.Windows.Forms.TextBox
	Public WithEvents cmbPeriodLength As System.Windows.Forms.ComboBox
	Public WithEvents txtPrevYearEndDate As System.Windows.Forms.TextBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAdd))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.cmdNavigate = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.Image1 = New System.Windows.Forms.PictureBox
        Me.lblPeriodPrefix = New System.Windows.Forms.Label
        Me.lblPrevYearEndDate = New System.Windows.Forms.Label
        Me.lblPeriodLength = New System.Windows.Forms.Label
        Me.lblYearName = New System.Windows.Forms.Label
        Me.txtPeriodPrefix = New System.Windows.Forms.TextBox
        Me.txtYearName = New System.Windows.Forms.TextBox
        Me.cmbPeriodLength = New System.Windows.Forms.ComboBox
        Me.txtPrevYearEndDate = New System.Windows.Forms.TextBox
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.Image1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(8, 272)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 8
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        Me.cmdNavigate.Visible = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(328, 272)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 11
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(248, 272)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 10
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
        Me.cmdOK.Location = New System.Drawing.Point(168, 272)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 9
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(195, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(397, 261)
        Me.tabMainTab.TabIndex = 12
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.Image1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPeriodPrefix)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPrevYearEndDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPeriodLength)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblYearName)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtPeriodPrefix)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtYearName)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmbPeriodLength)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtPrevYearEndDate)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(389, 235)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Accounting Year"
        '
        'Image1
        '
        Me.Image1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Image1.Image = CType(resources.GetObject("Image1.Image"), System.Drawing.Image)
        Me.Image1.Location = New System.Drawing.Point(352, 4)
        Me.Image1.Name = "Image1"
        Me.Image1.Size = New System.Drawing.Size(32, 32)
        Me.Image1.TabIndex = 0
        Me.Image1.TabStop = False
        '
        'lblPeriodPrefix
        '
        Me.lblPeriodPrefix.BackColor = System.Drawing.SystemColors.Control
        Me.lblPeriodPrefix.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPeriodPrefix.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPeriodPrefix.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPeriodPrefix.Location = New System.Drawing.Point(24, 119)
        Me.lblPeriodPrefix.Name = "lblPeriodPrefix"
        Me.lblPeriodPrefix.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPeriodPrefix.Size = New System.Drawing.Size(137, 17)
        Me.lblPeriodPrefix.TabIndex = 6
        Me.lblPeriodPrefix.Text = "Period Names Prefix:"
        '
        'lblPrevYearEndDate
        '
        Me.lblPrevYearEndDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblPrevYearEndDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPrevYearEndDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPrevYearEndDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPrevYearEndDate.Location = New System.Drawing.Point(24, 23)
        Me.lblPrevYearEndDate.Name = "lblPrevYearEndDate"
        Me.lblPrevYearEndDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPrevYearEndDate.Size = New System.Drawing.Size(145, 17)
        Me.lblPrevYearEndDate.TabIndex = 0
        Me.lblPrevYearEndDate.Text = "Previous Year End Date:"
        '
        'lblPeriodLength
        '
        Me.lblPeriodLength.BackColor = System.Drawing.SystemColors.Control
        Me.lblPeriodLength.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPeriodLength.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPeriodLength.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPeriodLength.Location = New System.Drawing.Point(24, 55)
        Me.lblPeriodLength.Name = "lblPeriodLength"
        Me.lblPeriodLength.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPeriodLength.Size = New System.Drawing.Size(137, 17)
        Me.lblPeriodLength.TabIndex = 2
        Me.lblPeriodLength.Text = "Length of Periods:"
        '
        'lblYearName
        '
        Me.lblYearName.BackColor = System.Drawing.SystemColors.Control
        Me.lblYearName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblYearName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblYearName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblYearName.Location = New System.Drawing.Point(24, 87)
        Me.lblYearName.Name = "lblYearName"
        Me.lblYearName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblYearName.Size = New System.Drawing.Size(137, 17)
        Me.lblYearName.TabIndex = 4
        Me.lblYearName.Text = "Accounting Year Name:"
        '
        'txtPeriodPrefix
        '
        Me.txtPeriodPrefix.AcceptsReturn = True
        Me.txtPeriodPrefix.BackColor = System.Drawing.SystemColors.Window
        Me.txtPeriodPrefix.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPeriodPrefix.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPeriodPrefix.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPeriodPrefix.Location = New System.Drawing.Point(176, 116)
        Me.txtPeriodPrefix.MaxLength = 40
        Me.txtPeriodPrefix.Name = "txtPeriodPrefix"
        Me.txtPeriodPrefix.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPeriodPrefix.Size = New System.Drawing.Size(169, 20)
        Me.txtPeriodPrefix.TabIndex = 7
        '
        'txtYearName
        '
        Me.txtYearName.AcceptsReturn = True
        Me.txtYearName.BackColor = System.Drawing.SystemColors.Window
        Me.txtYearName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtYearName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtYearName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtYearName.Location = New System.Drawing.Point(176, 84)
        Me.txtYearName.MaxLength = 40
        Me.txtYearName.Name = "txtYearName"
        Me.txtYearName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtYearName.Size = New System.Drawing.Size(169, 20)
        Me.txtYearName.TabIndex = 5
        '
        'cmbPeriodLength
        '
        Me.cmbPeriodLength.BackColor = System.Drawing.SystemColors.Window
        Me.cmbPeriodLength.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbPeriodLength.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPeriodLength.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbPeriodLength.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbPeriodLength.Location = New System.Drawing.Point(176, 52)
        Me.cmbPeriodLength.Name = "cmbPeriodLength"
        Me.cmbPeriodLength.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbPeriodLength.Size = New System.Drawing.Size(121, 21)
        Me.cmbPeriodLength.TabIndex = 3
        '
        'txtPrevYearEndDate
        '
        Me.txtPrevYearEndDate.AcceptsReturn = True
        Me.txtPrevYearEndDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtPrevYearEndDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPrevYearEndDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPrevYearEndDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPrevYearEndDate.Location = New System.Drawing.Point(176, 20)
        Me.txtPrevYearEndDate.MaxLength = 40
        Me.txtPrevYearEndDate.Name = "txtPrevYearEndDate"
        Me.txtPrevYearEndDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPrevYearEndDate.Size = New System.Drawing.Size(121, 20)
        Me.txtPrevYearEndDate.TabIndex = 1
        '
        'frmAdd
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(409, 300)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(417, 289)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAdd"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Accounting Periods"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        CType(Me.Image1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class