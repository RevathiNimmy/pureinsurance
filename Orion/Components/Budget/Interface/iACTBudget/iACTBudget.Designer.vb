<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		tabMainTabPreviousTab = tabMainTab.SelectedIndex
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
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents imgIcon As System.Windows.Forms.PictureBox
	Public WithEvents lblBudgetRef As System.Windows.Forms.Label
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents lblPeriodYear As System.Windows.Forms.Label
	Public WithEvents lblRevisesBudget As System.Windows.Forms.Label
	Public WithEvents lblBasedOnBudget As System.Windows.Forms.Label
	Public WithEvents lblStatus As System.Windows.Forms.Label
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents txtBudgetRef As System.Windows.Forms.TextBox
	Public WithEvents cboPeriodYear As System.Windows.Forms.ComboBox
	Public WithEvents txtRevisesBudget As System.Windows.Forms.TextBox
	Public WithEvents txtBasedOnBudget As System.Windows.Forms.TextBox
	Public WithEvents cmdRevisesBudget As System.Windows.Forms.Button
	Public WithEvents cmdBasedOnBudget As System.Windows.Forms.Button
	Public WithEvents cboStatus As System.Windows.Forms.ComboBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
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
        Me.imgIcon = New System.Windows.Forms.PictureBox
        Me.lblBudgetRef = New System.Windows.Forms.Label
        Me.lblDescription = New System.Windows.Forms.Label
        Me.lblPeriodYear = New System.Windows.Forms.Label
        Me.lblRevisesBudget = New System.Windows.Forms.Label
        Me.lblBasedOnBudget = New System.Windows.Forms.Label
        Me.lblStatus = New System.Windows.Forms.Label
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.txtBudgetRef = New System.Windows.Forms.TextBox
        Me.cboPeriodYear = New System.Windows.Forms.ComboBox
        Me.txtRevisesBudget = New System.Windows.Forms.TextBox
        Me.txtBasedOnBudget = New System.Windows.Forms.TextBox
        Me.cmdRevisesBudget = New System.Windows.Forms.Button
        Me.cmdBasedOnBudget = New System.Windows.Forms.Button
        Me.cboStatus = New System.Windows.Forms.ComboBox
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(8, 304)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 11
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
        Me.cmdHelp.Location = New System.Drawing.Point(280, 304)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 9
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
        Me.cmdCancel.Location = New System.Drawing.Point(200, 304)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 8
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
        Me.cmdOK.Location = New System.Drawing.Point(120, 304)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 7
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(68, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(349, 293)
        Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.tabMainTab.TabIndex = 0
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.imgIcon)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblBudgetRef)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPeriodYear)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblRevisesBudget)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblBasedOnBudget)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtBudgetRef)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboPeriodYear)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtRevisesBudget)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtBasedOnBudget)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdRevisesBudget)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdBasedOnBudget)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboStatus)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(341, 267)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - General"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'imgIcon
        '
        Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon.Image = CType(resources.GetObject("imgIcon.Image"), System.Drawing.Image)
        Me.imgIcon.Location = New System.Drawing.Point(304, 4)
        Me.imgIcon.Name = "imgIcon"
        Me.imgIcon.Size = New System.Drawing.Size(32, 32)
        Me.imgIcon.TabIndex = 0
        Me.imgIcon.TabStop = False
        '
        'lblBudgetRef
        '
        Me.lblBudgetRef.AutoSize = True
        Me.lblBudgetRef.BackColor = System.Drawing.SystemColors.Control
        Me.lblBudgetRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBudgetRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBudgetRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBudgetRef.Location = New System.Drawing.Point(16, 28)
        Me.lblBudgetRef.Name = "lblBudgetRef"
        Me.lblBudgetRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBudgetRef.Size = New System.Drawing.Size(67, 13)
        Me.lblBudgetRef.TabIndex = 12
        Me.lblBudgetRef.Text = "&Budget Ref.:"
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(16, 52)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(63, 13)
        Me.lblDescription.TabIndex = 13
        Me.lblDescription.Text = "&Description:"
        '
        'lblPeriodYear
        '
        Me.lblPeriodYear.AutoSize = True
        Me.lblPeriodYear.BackColor = System.Drawing.SystemColors.Control
        Me.lblPeriodYear.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPeriodYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPeriodYear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPeriodYear.Location = New System.Drawing.Point(16, 148)
        Me.lblPeriodYear.Name = "lblPeriodYear"
        Me.lblPeriodYear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPeriodYear.Size = New System.Drawing.Size(65, 13)
        Me.lblPeriodYear.TabIndex = 14
        Me.lblPeriodYear.Text = "&Period Year:"
        '
        'lblRevisesBudget
        '
        Me.lblRevisesBudget.AutoSize = True
        Me.lblRevisesBudget.BackColor = System.Drawing.SystemColors.Control
        Me.lblRevisesBudget.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRevisesBudget.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRevisesBudget.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRevisesBudget.Location = New System.Drawing.Point(16, 172)
        Me.lblRevisesBudget.Name = "lblRevisesBudget"
        Me.lblRevisesBudget.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRevisesBudget.Size = New System.Drawing.Size(84, 13)
        Me.lblRevisesBudget.TabIndex = 15
        Me.lblRevisesBudget.Text = "&Revises budget:"
        '
        'lblBasedOnBudget
        '
        Me.lblBasedOnBudget.AutoSize = True
        Me.lblBasedOnBudget.BackColor = System.Drawing.SystemColors.Control
        Me.lblBasedOnBudget.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBasedOnBudget.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBasedOnBudget.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBasedOnBudget.Location = New System.Drawing.Point(16, 196)
        Me.lblBasedOnBudget.Name = "lblBasedOnBudget"
        Me.lblBasedOnBudget.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBasedOnBudget.Size = New System.Drawing.Size(91, 13)
        Me.lblBasedOnBudget.TabIndex = 16
        Me.lblBasedOnBudget.Text = "B&ased on budget:"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatus.Location = New System.Drawing.Point(16, 220)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatus.Size = New System.Drawing.Size(40, 13)
        Me.lblStatus.TabIndex = 17
        Me.lblStatus.Text = "&Status:"
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(112, 52)
        Me.txtDescription.MaxLength = 0
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(217, 89)
        Me.txtDescription.TabIndex = 2
        '
        'txtBudgetRef
        '
        Me.txtBudgetRef.AcceptsReturn = True
        Me.txtBudgetRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtBudgetRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBudgetRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBudgetRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBudgetRef.Location = New System.Drawing.Point(112, 28)
        Me.txtBudgetRef.MaxLength = 0
        Me.txtBudgetRef.Name = "txtBudgetRef"
        Me.txtBudgetRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBudgetRef.Size = New System.Drawing.Size(113, 20)
        Me.txtBudgetRef.TabIndex = 1
        '
        'cboPeriodYear
        '
        Me.cboPeriodYear.BackColor = System.Drawing.SystemColors.Window
        Me.cboPeriodYear.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPeriodYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPeriodYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPeriodYear.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPeriodYear.Location = New System.Drawing.Point(112, 148)
        Me.cboPeriodYear.Name = "cboPeriodYear"
        Me.cboPeriodYear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPeriodYear.Size = New System.Drawing.Size(134, 21)
        Me.cboPeriodYear.TabIndex = 3
        '
        'txtRevisesBudget
        '
        Me.txtRevisesBudget.AcceptsReturn = True
        Me.txtRevisesBudget.BackColor = System.Drawing.SystemColors.Window
        Me.txtRevisesBudget.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRevisesBudget.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRevisesBudget.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRevisesBudget.Location = New System.Drawing.Point(112, 172)
        Me.txtRevisesBudget.MaxLength = 0
        Me.txtRevisesBudget.Name = "txtRevisesBudget"
        Me.txtRevisesBudget.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRevisesBudget.Size = New System.Drawing.Size(107, 20)
        Me.txtRevisesBudget.TabIndex = 4
        '
        'txtBasedOnBudget
        '
        Me.txtBasedOnBudget.AcceptsReturn = True
        Me.txtBasedOnBudget.BackColor = System.Drawing.SystemColors.Window
        Me.txtBasedOnBudget.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBasedOnBudget.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBasedOnBudget.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBasedOnBudget.Location = New System.Drawing.Point(112, 196)
        Me.txtBasedOnBudget.MaxLength = 0
        Me.txtBasedOnBudget.Name = "txtBasedOnBudget"
        Me.txtBasedOnBudget.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBasedOnBudget.Size = New System.Drawing.Size(106, 20)
        Me.txtBasedOnBudget.TabIndex = 6
        '
        'cmdRevisesBudget
        '
        Me.cmdRevisesBudget.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRevisesBudget.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRevisesBudget.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRevisesBudget.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRevisesBudget.Location = New System.Drawing.Point(222, 172)
        Me.cmdRevisesBudget.Name = "cmdRevisesBudget"
        Me.cmdRevisesBudget.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRevisesBudget.Size = New System.Drawing.Size(24, 19)
        Me.cmdRevisesBudget.TabIndex = 5
        Me.cmdRevisesBudget.Text = "..."
        Me.cmdRevisesBudget.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRevisesBudget.UseVisualStyleBackColor = False
        '
        'cmdBasedOnBudget
        '
        Me.cmdBasedOnBudget.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBasedOnBudget.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBasedOnBudget.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBasedOnBudget.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBasedOnBudget.Location = New System.Drawing.Point(222, 195)
        Me.cmdBasedOnBudget.Name = "cmdBasedOnBudget"
        Me.cmdBasedOnBudget.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBasedOnBudget.Size = New System.Drawing.Size(24, 20)
        Me.cmdBasedOnBudget.TabIndex = 7
        Me.cmdBasedOnBudget.Text = "..."
        Me.cmdBasedOnBudget.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdBasedOnBudget.UseVisualStyleBackColor = False
        '
        'cboStatus
        '
        Me.cboStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cboStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboStatus.Location = New System.Drawing.Point(112, 220)
        Me.cboStatus.Name = "cboStatus"
        Me.cboStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboStatus.Size = New System.Drawing.Size(136, 21)
        Me.cboStatus.TabIndex = 18
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(363, 332)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(329, 181)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Budget"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class