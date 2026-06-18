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
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents ddGender As PMListMgrDropdown.uctDropdown
	Public WithEvents ddSecondaryOccupation As PMListMgrDropdown.uctDropdown
	Public WithEvents ddOccupation As PMListMgrDropdown.uctDropdown
	Public WithEvents chkSmoker As System.Windows.Forms.CheckBox
	Public WithEvents txtDOB As System.Windows.Forms.TextBox
	Public WithEvents txtName As System.Windows.Forms.TextBox
	Public WithEvents cboCategory As System.Windows.Forms.ComboBox
	Public WithEvents lblSmoker As System.Windows.Forms.Label
	Public WithEvents lblName As System.Windows.Forms.Label
	Public WithEvents lblDOB As System.Windows.Forms.Label
	Public WithEvents lblGenderCode As System.Windows.Forms.Label
	Public WithEvents lblOccupationCode As System.Windows.Forms.Label
	Public WithEvents lblSecOccCode As System.Windows.Forms.Label
	Public WithEvents lblCategory As System.Windows.Forms.Label
	Public WithEvents fraGeneral As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdNavigate = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.fraGeneral = New System.Windows.Forms.GroupBox
        Me.ddGender = New PMListMgrDropdown.uctDropdown
        Me.ddSecondaryOccupation = New PMListMgrDropdown.uctDropdown
        Me.ddOccupation = New PMListMgrDropdown.uctDropdown
        Me.chkSmoker = New System.Windows.Forms.CheckBox
        Me.txtDOB = New System.Windows.Forms.TextBox
        Me.txtName = New System.Windows.Forms.TextBox
        Me.cboCategory = New System.Windows.Forms.ComboBox
        Me.lblSmoker = New System.Windows.Forms.Label
        Me.lblName = New System.Windows.Forms.Label
        Me.lblDOB = New System.Windows.Forms.Label
        Me.lblGenderCode = New System.Windows.Forms.Label
        Me.lblOccupationCode = New System.Windows.Forms.Label
        Me.lblSecOccCode = New System.Windows.Forms.Label
        Me.lblCategory = New System.Windows.Forms.Label
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.fraGeneral.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(8, 312)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 7
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
        Me.cmdHelp.Location = New System.Drawing.Point(472, 312)
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
        Me.cmdCancel.Location = New System.Drawing.Point(392, 312)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 9
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
        Me.cmdOK.Location = New System.Drawing.Point(312, 312)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 8
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(106, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(541, 301)
        Me.tabMainTab.TabIndex = 10
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraGeneral)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(533, 275)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - General"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'fraGeneral
        '
        Me.fraGeneral.BackColor = System.Drawing.SystemColors.Control
        Me.fraGeneral.Controls.Add(Me.ddGender)
        Me.fraGeneral.Controls.Add(Me.ddSecondaryOccupation)
        Me.fraGeneral.Controls.Add(Me.ddOccupation)
        Me.fraGeneral.Controls.Add(Me.chkSmoker)
        Me.fraGeneral.Controls.Add(Me.txtDOB)
        Me.fraGeneral.Controls.Add(Me.txtName)
        Me.fraGeneral.Controls.Add(Me.cboCategory)
        Me.fraGeneral.Controls.Add(Me.lblSmoker)
        Me.fraGeneral.Controls.Add(Me.lblName)
        Me.fraGeneral.Controls.Add(Me.lblDOB)
        Me.fraGeneral.Controls.Add(Me.lblGenderCode)
        Me.fraGeneral.Controls.Add(Me.lblOccupationCode)
        Me.fraGeneral.Controls.Add(Me.lblSecOccCode)
        Me.fraGeneral.Controls.Add(Me.lblCategory)
        Me.fraGeneral.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraGeneral.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraGeneral.Location = New System.Drawing.Point(16, 12)
        Me.fraGeneral.Name = "fraGeneral"
        Me.fraGeneral.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraGeneral.Size = New System.Drawing.Size(457, 249)
        Me.fraGeneral.TabIndex = 12
        Me.fraGeneral.TabStop = False
        '
        'ddGender
        '
        Me.ddGender.AllowAbiCodeEntry = False
        Me.ddGender.AutoCompleteText = False
        Me.ddGender.DataModel = "GIIM"
        Me.ddGender.ListIndex = -1
        Me.ddGender.ListManager = Nothing
        Me.ddGender.Location = New System.Drawing.Point(200, 120)
        Me.ddGender.Login = False
        Me.ddGender.LongList = False
        Me.ddGender.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddGender.Name = "ddGender"
        Me.ddGender.PropertyId = "131091"
        Me.ddGender.ReadOnly_Renamed = False
        Me.ddGender.SelLength = 0
        Me.ddGender.SelStart = 0
        Me.ddGender.SelText = ""
        Me.ddGender.Size = New System.Drawing.Size(241, 21)
        Me.ddGender.TabIndex = 3
        Me.ddGender.ToolTipText = ""
        Me.ddGender.VehicleListId = ""
        Me.ddGender.VehicleMake = ""
        '
        'ddSecondaryOccupation
        '
        Me.ddSecondaryOccupation.AllowAbiCodeEntry = False
        Me.ddSecondaryOccupation.AutoCompleteText = False
        Me.ddSecondaryOccupation.DataModel = "GIIM"
        Me.ddSecondaryOccupation.ListIndex = -1
        Me.ddSecondaryOccupation.ListManager = Nothing
        Me.ddSecondaryOccupation.Location = New System.Drawing.Point(200, 184)
        Me.ddSecondaryOccupation.Login = False
        Me.ddSecondaryOccupation.LongList = True
        Me.ddSecondaryOccupation.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddSecondaryOccupation.Name = "ddSecondaryOccupation"
        Me.ddSecondaryOccupation.PropertyId = "2228226"
        Me.ddSecondaryOccupation.ReadOnly_Renamed = False
        Me.ddSecondaryOccupation.SelLength = 0
        Me.ddSecondaryOccupation.SelStart = 0
        Me.ddSecondaryOccupation.SelText = ""
        Me.ddSecondaryOccupation.Size = New System.Drawing.Size(241, 21)
        Me.ddSecondaryOccupation.TabIndex = 5
        Me.ddSecondaryOccupation.ToolTipText = ""
        Me.ddSecondaryOccupation.VehicleListId = ""
        Me.ddSecondaryOccupation.VehicleMake = ""
        '
        'ddOccupation
        '
        Me.ddOccupation.AllowAbiCodeEntry = False
        Me.ddOccupation.AutoCompleteText = False
        Me.ddOccupation.DataModel = "GIIM"
        Me.ddOccupation.ListIndex = -1
        Me.ddOccupation.ListManager = Nothing
        Me.ddOccupation.Location = New System.Drawing.Point(200, 152)
        Me.ddOccupation.Login = False
        Me.ddOccupation.LongList = True
        Me.ddOccupation.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddOccupation.Name = "ddOccupation"
        Me.ddOccupation.PropertyId = "2228226"
        Me.ddOccupation.ReadOnly_Renamed = False
        Me.ddOccupation.SelLength = 0
        Me.ddOccupation.SelStart = 0
        Me.ddOccupation.SelText = ""
        Me.ddOccupation.Size = New System.Drawing.Size(241, 21)
        Me.ddOccupation.TabIndex = 4
        Me.ddOccupation.ToolTipText = ""
        Me.ddOccupation.VehicleListId = ""
        Me.ddOccupation.VehicleMake = ""
        '
        'chkSmoker
        '
        Me.chkSmoker.BackColor = System.Drawing.SystemColors.Control
        Me.chkSmoker.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSmoker.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSmoker.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSmoker.Location = New System.Drawing.Point(200, 216)
        Me.chkSmoker.Name = "chkSmoker"
        Me.chkSmoker.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSmoker.Size = New System.Drawing.Size(37, 17)
        Me.chkSmoker.TabIndex = 6
        Me.chkSmoker.UseVisualStyleBackColor = False
        '
        'txtDOB
        '
        Me.txtDOB.AcceptsReturn = True
        Me.txtDOB.BackColor = System.Drawing.SystemColors.Window
        Me.txtDOB.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDOB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDOB.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDOB.Location = New System.Drawing.Point(200, 56)
        Me.txtDOB.MaxLength = 0
        Me.txtDOB.Name = "txtDOB"
        Me.txtDOB.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDOB.Size = New System.Drawing.Size(241, 20)
        Me.txtDOB.TabIndex = 1
        '
        'txtName
        '
        Me.txtName.AcceptsReturn = True
        Me.txtName.BackColor = System.Drawing.SystemColors.Window
        Me.txtName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtName.Location = New System.Drawing.Point(200, 24)
        Me.txtName.MaxLength = 0
        Me.txtName.Name = "txtName"
        Me.txtName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtName.Size = New System.Drawing.Size(241, 20)
        Me.txtName.TabIndex = 0
        '
        'cboCategory
        '
        Me.cboCategory.BackColor = System.Drawing.SystemColors.Window
        Me.cboCategory.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCategory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCategory.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCategory.Location = New System.Drawing.Point(200, 88)
        Me.cboCategory.Name = "cboCategory"
        Me.cboCategory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCategory.Size = New System.Drawing.Size(241, 21)
        Me.cboCategory.TabIndex = 2
        '
        'lblSmoker
        '
        Me.lblSmoker.BackColor = System.Drawing.SystemColors.Control
        Me.lblSmoker.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSmoker.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSmoker.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSmoker.Location = New System.Drawing.Point(16, 216)
        Me.lblSmoker.Name = "lblSmoker"
        Me.lblSmoker.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSmoker.Size = New System.Drawing.Size(169, 17)
        Me.lblSmoker.TabIndex = 19
        Me.lblSmoker.Text = "Smoker:"
        '
        'lblName
        '
        Me.lblName.BackColor = System.Drawing.SystemColors.Control
        Me.lblName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblName.Location = New System.Drawing.Point(16, 24)
        Me.lblName.Name = "lblName"
        Me.lblName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblName.Size = New System.Drawing.Size(49, 17)
        Me.lblName.TabIndex = 18
        Me.lblName.Text = "Name:"
        '
        'lblDOB
        '
        Me.lblDOB.BackColor = System.Drawing.SystemColors.Control
        Me.lblDOB.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDOB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDOB.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDOB.Location = New System.Drawing.Point(16, 56)
        Me.lblDOB.Name = "lblDOB"
        Me.lblDOB.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDOB.Size = New System.Drawing.Size(89, 17)
        Me.lblDOB.TabIndex = 17
        Me.lblDOB.Text = "Date of birth:"
        '
        'lblGenderCode
        '
        Me.lblGenderCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblGenderCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGenderCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGenderCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGenderCode.Location = New System.Drawing.Point(16, 120)
        Me.lblGenderCode.Name = "lblGenderCode"
        Me.lblGenderCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGenderCode.Size = New System.Drawing.Size(89, 17)
        Me.lblGenderCode.TabIndex = 16
        Me.lblGenderCode.Text = "Gender code:"
        '
        'lblOccupationCode
        '
        Me.lblOccupationCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblOccupationCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOccupationCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOccupationCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOccupationCode.Location = New System.Drawing.Point(16, 152)
        Me.lblOccupationCode.Name = "lblOccupationCode"
        Me.lblOccupationCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOccupationCode.Size = New System.Drawing.Size(113, 17)
        Me.lblOccupationCode.TabIndex = 15
        Me.lblOccupationCode.Text = "Occupation code:"
        '
        'lblSecOccCode
        '
        Me.lblSecOccCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblSecOccCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSecOccCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSecOccCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSecOccCode.Location = New System.Drawing.Point(16, 184)
        Me.lblSecOccCode.Name = "lblSecOccCode"
        Me.lblSecOccCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSecOccCode.Size = New System.Drawing.Size(169, 17)
        Me.lblSecOccCode.TabIndex = 14
        Me.lblSecOccCode.Text = "Secondary occupation code:"
        '
        'lblCategory
        '
        Me.lblCategory.BackColor = System.Drawing.SystemColors.Control
        Me.lblCategory.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCategory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCategory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCategory.Location = New System.Drawing.Point(16, 88)
        Me.lblCategory.Name = "lblCategory"
        Me.lblCategory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCategory.Size = New System.Drawing.Size(65, 17)
        Me.lblCategory.TabIndex = 13
        Me.lblCategory.Text = "Category:"
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(553, 341)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Lifestyle"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.fraGeneral.ResumeLayout(False)
        Me.fraGeneral.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class