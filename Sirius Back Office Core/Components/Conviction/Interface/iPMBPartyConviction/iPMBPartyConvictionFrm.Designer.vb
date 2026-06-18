<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents txtDate As System.Windows.Forms.TextBox
	Public WithEvents txtFine As System.Windows.Forms.TextBox
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents ddConvictionType As PMListMgrDropdown.uctDropdown
	Public WithEvents ddConvStatus As PMListMgrDropdown.uctDropdown
	Public WithEvents lblDate As System.Windows.Forms.Label
	Public WithEvents lblFine As System.Windows.Forms.Label
	Public WithEvents lblConvictionStatus As System.Windows.Forms.Label
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents lblConvictionType As System.Windows.Forms.Label
	Public WithEvents fraConvictionDetails As System.Windows.Forms.GroupBox
	Public WithEvents txtSentDate As System.Windows.Forms.TextBox
	Public WithEvents txtSentDuration As System.Windows.Forms.TextBox
	Public WithEvents txtSentDesc As System.Windows.Forms.TextBox
	Public WithEvents ddSentence As PMListMgrDropdown.uctDropdown
	Public WithEvents ddTimeUnit As PMListMgrDropdown.uctDropdown
	Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
	Public WithEvents lblSentDuration As System.Windows.Forms.Label
	Public WithEvents lblTimeUnit As System.Windows.Forms.Label
	Public WithEvents lblSentenceDesc As System.Windows.Forms.Label
	Public WithEvents lblSentence As System.Windows.Forms.Label
	Public WithEvents fraSentence As System.Windows.Forms.GroupBox
	Public WithEvents txtPenaltyPts As System.Windows.Forms.TextBox
	Public WithEvents txtAlcLevel As System.Windows.Forms.TextBox
	Public WithEvents ddAlcMeasurement As PMListMgrDropdown.uctDropdown
	Public WithEvents lblPenaltyPoints As System.Windows.Forms.Label
	Public WithEvents lblAlcoholLevel As System.Windows.Forms.Label
	Public WithEvents lblAlcMethod As System.Windows.Forms.Label
	Public WithEvents fraMotoring As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
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
        Me.fraConvictionDetails = New System.Windows.Forms.GroupBox
        Me.txtDate = New System.Windows.Forms.TextBox
        Me.txtFine = New System.Windows.Forms.TextBox
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.ddConvictionType = New PMListMgrDropdown.uctDropdown
        Me.ddConvStatus = New PMListMgrDropdown.uctDropdown
        Me.lblDate = New System.Windows.Forms.Label
        Me.lblFine = New System.Windows.Forms.Label
        Me.lblConvictionStatus = New System.Windows.Forms.Label
        Me.lblDescription = New System.Windows.Forms.Label
        Me.lblConvictionType = New System.Windows.Forms.Label
        Me.fraSentence = New System.Windows.Forms.GroupBox
        Me.txtSentDate = New System.Windows.Forms.TextBox
        Me.txtSentDuration = New System.Windows.Forms.TextBox
        Me.txtSentDesc = New System.Windows.Forms.TextBox
        Me.ddSentence = New PMListMgrDropdown.uctDropdown
        Me.ddTimeUnit = New PMListMgrDropdown.uctDropdown
        Me.lblEffectiveDate = New System.Windows.Forms.Label
        Me.lblSentDuration = New System.Windows.Forms.Label
        Me.lblTimeUnit = New System.Windows.Forms.Label
        Me.lblSentenceDesc = New System.Windows.Forms.Label
        Me.lblSentence = New System.Windows.Forms.Label
        Me.fraMotoring = New System.Windows.Forms.GroupBox
        Me.txtPenaltyPts = New System.Windows.Forms.TextBox
        Me.txtAlcLevel = New System.Windows.Forms.TextBox
        Me.ddAlcMeasurement = New PMListMgrDropdown.uctDropdown
        Me.lblPenaltyPoints = New System.Windows.Forms.Label
        Me.lblAlcoholLevel = New System.Windows.Forms.Label
        Me.lblAlcMethod = New System.Windows.Forms.Label
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.fraConvictionDetails.SuspendLayout()
        Me.fraSentence.SuspendLayout()
        Me.fraMotoring.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(8, 440)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 14
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
        Me.cmdHelp.Location = New System.Drawing.Point(632, 440)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 17
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
        Me.cmdCancel.Location = New System.Drawing.Point(552, 440)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 16
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
        Me.cmdOK.Location = New System.Drawing.Point(472, 440)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 15
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(138, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(701, 429)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraConvictionDetails)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraSentence)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraMotoring)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(693, 403)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "&Conviction"
        '
        'fraConvictionDetails
        '
        Me.fraConvictionDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraConvictionDetails.Controls.Add(Me.txtDate)
        Me.fraConvictionDetails.Controls.Add(Me.txtFine)
        Me.fraConvictionDetails.Controls.Add(Me.txtDescription)
        Me.fraConvictionDetails.Controls.Add(Me.ddConvictionType)
        Me.fraConvictionDetails.Controls.Add(Me.ddConvStatus)
        Me.fraConvictionDetails.Controls.Add(Me.lblDate)
        Me.fraConvictionDetails.Controls.Add(Me.lblFine)
        Me.fraConvictionDetails.Controls.Add(Me.lblConvictionStatus)
        Me.fraConvictionDetails.Controls.Add(Me.lblDescription)
        Me.fraConvictionDetails.Controls.Add(Me.lblConvictionType)
        Me.fraConvictionDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraConvictionDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraConvictionDetails.Location = New System.Drawing.Point(16, 20)
        Me.fraConvictionDetails.Name = "fraConvictionDetails"
        Me.fraConvictionDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraConvictionDetails.Size = New System.Drawing.Size(633, 121)
        Me.fraConvictionDetails.TabIndex = 18
        Me.fraConvictionDetails.TabStop = False
        Me.fraConvictionDetails.Text = "Conviction Details"
        '
        'txtDate
        '
        Me.txtDate.AcceptsReturn = True
        Me.txtDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDate.Location = New System.Drawing.Point(432, 56)
        Me.txtDate.MaxLength = 0
        Me.txtDate.Name = "txtDate"
        Me.txtDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDate.Size = New System.Drawing.Size(185, 20)
        Me.txtDate.TabIndex = 4
        Me.txtDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtFine
        '
        Me.txtFine.AcceptsReturn = True
        Me.txtFine.BackColor = System.Drawing.SystemColors.Window
        Me.txtFine.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFine.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFine.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFine.Location = New System.Drawing.Point(432, 24)
        Me.txtFine.MaxLength = 0
        Me.txtFine.Name = "txtFine"
        Me.txtFine.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFine.Size = New System.Drawing.Size(185, 20)
        Me.txtFine.TabIndex = 2
        Me.txtFine.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(112, 88)
        Me.txtDescription.MaxLength = 0
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(233, 20)
        Me.txtDescription.TabIndex = 5
        '
        'ddConvictionType
        '
        Me.ddConvictionType.AllowAbiCodeEntry = False
        Me.ddConvictionType.AutoCompleteText = False
        Me.ddConvictionType.DataModel = "RSA"
        Me.ddConvictionType.ListIndex = -1
        Me.ddConvictionType.ListManager = Nothing
        Me.ddConvictionType.Location = New System.Drawing.Point(112, 24)
        Me.ddConvictionType.Login = False
        Me.ddConvictionType.LongList = False
        Me.ddConvictionType.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddConvictionType.Name = "ddConvictionType"
        Me.ddConvictionType.PropertyId = "1114113"
        Me.ddConvictionType.ReadOnly_Renamed = True
        Me.ddConvictionType.SelLength = 0
        Me.ddConvictionType.SelStart = 0
        Me.ddConvictionType.SelText = ""
        Me.ddConvictionType.Size = New System.Drawing.Size(233, 21)
        Me.ddConvictionType.TabIndex = 1
        Me.ddConvictionType.ToolTipText = ""
        Me.ddConvictionType.VehicleListId = ""
        Me.ddConvictionType.VehicleMake = ""
        '
        'ddConvStatus
        '
        Me.ddConvStatus.AllowAbiCodeEntry = False
        Me.ddConvStatus.AutoCompleteText = False
        Me.ddConvStatus.DataModel = "RSA"
        Me.ddConvStatus.ListIndex = -1
        Me.ddConvStatus.ListManager = Nothing
        Me.ddConvStatus.Location = New System.Drawing.Point(112, 56)
        Me.ddConvStatus.Login = False
        Me.ddConvStatus.LongList = False
        Me.ddConvStatus.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddConvStatus.Name = "ddConvStatus"
        Me.ddConvStatus.PropertyId = "1114124"
        Me.ddConvStatus.ReadOnly_Renamed = True
        Me.ddConvStatus.SelLength = 0
        Me.ddConvStatus.SelStart = 0
        Me.ddConvStatus.SelText = ""
        Me.ddConvStatus.Size = New System.Drawing.Size(233, 21)
        Me.ddConvStatus.TabIndex = 3
        Me.ddConvStatus.ToolTipText = ""
        Me.ddConvStatus.VehicleListId = ""
        Me.ddConvStatus.VehicleMake = ""
        '
        'lblDate
        '
        Me.lblDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDate.Location = New System.Drawing.Point(368, 59)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDate.Size = New System.Drawing.Size(41, 17)
        Me.lblDate.TabIndex = 23
        Me.lblDate.Text = "Date:"
        '
        'lblFine
        '
        Me.lblFine.BackColor = System.Drawing.SystemColors.Control
        Me.lblFine.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFine.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFine.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFine.Location = New System.Drawing.Point(368, 27)
        Me.lblFine.Name = "lblFine"
        Me.lblFine.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFine.Size = New System.Drawing.Size(41, 17)
        Me.lblFine.TabIndex = 22
        Me.lblFine.Text = "Fine:"
        '
        'lblConvictionStatus
        '
        Me.lblConvictionStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblConvictionStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblConvictionStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConvictionStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblConvictionStatus.Location = New System.Drawing.Point(16, 59)
        Me.lblConvictionStatus.Name = "lblConvictionStatus"
        Me.lblConvictionStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblConvictionStatus.Size = New System.Drawing.Size(113, 17)
        Me.lblConvictionStatus.TabIndex = 21
        Me.lblConvictionStatus.Text = "Status:"
        '
        'lblDescription
        '
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(16, 91)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(89, 17)
        Me.lblDescription.TabIndex = 20
        Me.lblDescription.Text = "Description:"
        '
        'lblConvictionType
        '
        Me.lblConvictionType.BackColor = System.Drawing.SystemColors.Control
        Me.lblConvictionType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblConvictionType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConvictionType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblConvictionType.Location = New System.Drawing.Point(16, 27)
        Me.lblConvictionType.Name = "lblConvictionType"
        Me.lblConvictionType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblConvictionType.Size = New System.Drawing.Size(113, 17)
        Me.lblConvictionType.TabIndex = 19
        Me.lblConvictionType.Text = "Type:"
        '
        'fraSentence
        '
        Me.fraSentence.BackColor = System.Drawing.SystemColors.Control
        Me.fraSentence.Controls.Add(Me.txtSentDate)
        Me.fraSentence.Controls.Add(Me.txtSentDuration)
        Me.fraSentence.Controls.Add(Me.txtSentDesc)
        Me.fraSentence.Controls.Add(Me.ddSentence)
        Me.fraSentence.Controls.Add(Me.ddTimeUnit)
        Me.fraSentence.Controls.Add(Me.lblEffectiveDate)
        Me.fraSentence.Controls.Add(Me.lblSentDuration)
        Me.fraSentence.Controls.Add(Me.lblTimeUnit)
        Me.fraSentence.Controls.Add(Me.lblSentenceDesc)
        Me.fraSentence.Controls.Add(Me.lblSentence)
        Me.fraSentence.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraSentence.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraSentence.Location = New System.Drawing.Point(16, 156)
        Me.fraSentence.Name = "fraSentence"
        Me.fraSentence.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraSentence.Size = New System.Drawing.Size(633, 121)
        Me.fraSentence.TabIndex = 24
        Me.fraSentence.TabStop = False
        Me.fraSentence.Text = "Sentence"
        '
        'txtSentDate
        '
        Me.txtSentDate.AcceptsReturn = True
        Me.txtSentDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtSentDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSentDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSentDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSentDate.Location = New System.Drawing.Point(440, 24)
        Me.txtSentDate.MaxLength = 0
        Me.txtSentDate.Name = "txtSentDate"
        Me.txtSentDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSentDate.Size = New System.Drawing.Size(177, 20)
        Me.txtSentDate.TabIndex = 7
        Me.txtSentDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtSentDuration
        '
        Me.txtSentDuration.AcceptsReturn = True
        Me.txtSentDuration.BackColor = System.Drawing.SystemColors.Window
        Me.txtSentDuration.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSentDuration.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSentDuration.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSentDuration.Location = New System.Drawing.Point(440, 56)
        Me.txtSentDuration.MaxLength = 0
        Me.txtSentDuration.Name = "txtSentDuration"
        Me.txtSentDuration.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSentDuration.Size = New System.Drawing.Size(177, 20)
        Me.txtSentDuration.TabIndex = 9
        Me.txtSentDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtSentDesc
        '
        Me.txtSentDesc.AcceptsReturn = True
        Me.txtSentDesc.BackColor = System.Drawing.SystemColors.Window
        Me.txtSentDesc.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSentDesc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSentDesc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSentDesc.Location = New System.Drawing.Point(112, 56)
        Me.txtSentDesc.MaxLength = 0
        Me.txtSentDesc.Name = "txtSentDesc"
        Me.txtSentDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSentDesc.Size = New System.Drawing.Size(233, 20)
        Me.txtSentDesc.TabIndex = 8
        '
        'ddSentence
        '
        Me.ddSentence.AllowAbiCodeEntry = False
        Me.ddSentence.AutoCompleteText = False
        Me.ddSentence.DataModel = "RSA"
        Me.ddSentence.ListIndex = -1
        Me.ddSentence.ListManager = Nothing
        Me.ddSentence.Location = New System.Drawing.Point(112, 24)
        Me.ddSentence.Login = False
        Me.ddSentence.LongList = False
        Me.ddSentence.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddSentence.Name = "ddSentence"
        Me.ddSentence.PropertyId = "1114119"
        Me.ddSentence.ReadOnly_Renamed = True
        Me.ddSentence.SelLength = 0
        Me.ddSentence.SelStart = 0
        Me.ddSentence.SelText = ""
        Me.ddSentence.Size = New System.Drawing.Size(233, 21)
        Me.ddSentence.TabIndex = 6
        Me.ddSentence.ToolTipText = ""
        Me.ddSentence.VehicleListId = ""
        Me.ddSentence.VehicleMake = ""
        '
        'ddTimeUnit
        '
        Me.ddTimeUnit.AllowAbiCodeEntry = False
        Me.ddTimeUnit.AutoCompleteText = False
        Me.ddTimeUnit.DataModel = "RSA"
        Me.ddTimeUnit.ListIndex = -1
        Me.ddTimeUnit.ListManager = Nothing
        Me.ddTimeUnit.Location = New System.Drawing.Point(440, 88)
        Me.ddTimeUnit.Login = False
        Me.ddTimeUnit.LongList = False
        Me.ddTimeUnit.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddTimeUnit.Name = "ddTimeUnit"
        Me.ddTimeUnit.PropertyId = "1114122"
        Me.ddTimeUnit.ReadOnly_Renamed = True
        Me.ddTimeUnit.SelLength = 0
        Me.ddTimeUnit.SelStart = 0
        Me.ddTimeUnit.SelText = ""
        Me.ddTimeUnit.Size = New System.Drawing.Size(177, 21)
        Me.ddTimeUnit.TabIndex = 10
        Me.ddTimeUnit.ToolTipText = ""
        Me.ddTimeUnit.VehicleListId = ""
        Me.ddTimeUnit.VehicleMake = ""
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(368, 27)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(41, 17)
        Me.lblEffectiveDate.TabIndex = 29
        Me.lblEffectiveDate.Text = "Date:"
        '
        'lblSentDuration
        '
        Me.lblSentDuration.BackColor = System.Drawing.SystemColors.Control
        Me.lblSentDuration.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSentDuration.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSentDuration.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSentDuration.Location = New System.Drawing.Point(368, 59)
        Me.lblSentDuration.Name = "lblSentDuration"
        Me.lblSentDuration.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSentDuration.Size = New System.Drawing.Size(57, 17)
        Me.lblSentDuration.TabIndex = 28
        Me.lblSentDuration.Text = "Duration:"
        '
        'lblTimeUnit
        '
        Me.lblTimeUnit.BackColor = System.Drawing.SystemColors.Control
        Me.lblTimeUnit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTimeUnit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTimeUnit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTimeUnit.Location = New System.Drawing.Point(368, 91)
        Me.lblTimeUnit.Name = "lblTimeUnit"
        Me.lblTimeUnit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTimeUnit.Size = New System.Drawing.Size(65, 17)
        Me.lblTimeUnit.TabIndex = 27
        Me.lblTimeUnit.Text = "Time unit:"
        '
        'lblSentenceDesc
        '
        Me.lblSentenceDesc.BackColor = System.Drawing.SystemColors.Control
        Me.lblSentenceDesc.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSentenceDesc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSentenceDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSentenceDesc.Location = New System.Drawing.Point(16, 59)
        Me.lblSentenceDesc.Name = "lblSentenceDesc"
        Me.lblSentenceDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSentenceDesc.Size = New System.Drawing.Size(89, 17)
        Me.lblSentenceDesc.TabIndex = 26
        Me.lblSentenceDesc.Text = "Description:"
        '
        'lblSentence
        '
        Me.lblSentence.BackColor = System.Drawing.SystemColors.Control
        Me.lblSentence.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSentence.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSentence.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSentence.Location = New System.Drawing.Point(16, 27)
        Me.lblSentence.Name = "lblSentence"
        Me.lblSentence.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSentence.Size = New System.Drawing.Size(57, 17)
        Me.lblSentence.TabIndex = 25
        Me.lblSentence.Text = "Type:"
        '
        'fraMotoring
        '
        Me.fraMotoring.BackColor = System.Drawing.SystemColors.Control
        Me.fraMotoring.Controls.Add(Me.txtPenaltyPts)
        Me.fraMotoring.Controls.Add(Me.txtAlcLevel)
        Me.fraMotoring.Controls.Add(Me.ddAlcMeasurement)
        Me.fraMotoring.Controls.Add(Me.lblPenaltyPoints)
        Me.fraMotoring.Controls.Add(Me.lblAlcoholLevel)
        Me.fraMotoring.Controls.Add(Me.lblAlcMethod)
        Me.fraMotoring.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraMotoring.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraMotoring.Location = New System.Drawing.Point(16, 292)
        Me.fraMotoring.Name = "fraMotoring"
        Me.fraMotoring.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraMotoring.Size = New System.Drawing.Size(633, 97)
        Me.fraMotoring.TabIndex = 30
        Me.fraMotoring.TabStop = False
        Me.fraMotoring.Text = "Motoring Related"
        '
        'txtPenaltyPts
        '
        Me.txtPenaltyPts.AcceptsReturn = True
        Me.txtPenaltyPts.BackColor = System.Drawing.SystemColors.Window
        Me.txtPenaltyPts.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPenaltyPts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPenaltyPts.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPenaltyPts.Location = New System.Drawing.Point(440, 64)
        Me.txtPenaltyPts.MaxLength = 0
        Me.txtPenaltyPts.Name = "txtPenaltyPts"
        Me.txtPenaltyPts.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPenaltyPts.Size = New System.Drawing.Size(177, 20)
        Me.txtPenaltyPts.TabIndex = 13
        Me.txtPenaltyPts.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAlcLevel
        '
        Me.txtAlcLevel.AcceptsReturn = True
        Me.txtAlcLevel.BackColor = System.Drawing.SystemColors.Window
        Me.txtAlcLevel.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAlcLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAlcLevel.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAlcLevel.Location = New System.Drawing.Point(440, 32)
        Me.txtAlcLevel.MaxLength = 0
        Me.txtAlcLevel.Name = "txtAlcLevel"
        Me.txtAlcLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAlcLevel.Size = New System.Drawing.Size(177, 20)
        Me.txtAlcLevel.TabIndex = 12
        Me.txtAlcLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ddAlcMeasurement
        '
        Me.ddAlcMeasurement.AllowAbiCodeEntry = False
        Me.ddAlcMeasurement.AutoCompleteText = False
        Me.ddAlcMeasurement.DataModel = "RSA"
        Me.ddAlcMeasurement.ListIndex = -1
        Me.ddAlcMeasurement.ListManager = Nothing
        Me.ddAlcMeasurement.Location = New System.Drawing.Point(112, 32)
        Me.ddAlcMeasurement.Login = False
        Me.ddAlcMeasurement.LongList = False
        Me.ddAlcMeasurement.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddAlcMeasurement.Name = "ddAlcMeasurement"
        Me.ddAlcMeasurement.PropertyId = "1114126"
        Me.ddAlcMeasurement.ReadOnly_Renamed = True
        Me.ddAlcMeasurement.SelLength = 0
        Me.ddAlcMeasurement.SelStart = 0
        Me.ddAlcMeasurement.SelText = ""
        Me.ddAlcMeasurement.Size = New System.Drawing.Size(233, 21)
        Me.ddAlcMeasurement.TabIndex = 11
        Me.ddAlcMeasurement.ToolTipText = ""
        Me.ddAlcMeasurement.VehicleListId = ""
        Me.ddAlcMeasurement.VehicleMake = ""
        '
        'lblPenaltyPoints
        '
        Me.lblPenaltyPoints.BackColor = System.Drawing.SystemColors.Control
        Me.lblPenaltyPoints.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPenaltyPoints.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPenaltyPoints.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPenaltyPoints.Location = New System.Drawing.Point(368, 57)
        Me.lblPenaltyPoints.Name = "lblPenaltyPoints"
        Me.lblPenaltyPoints.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPenaltyPoints.Size = New System.Drawing.Size(57, 33)
        Me.lblPenaltyPoints.TabIndex = 33
        Me.lblPenaltyPoints.Text = "Penalty points:"
        '
        'lblAlcoholLevel
        '
        Me.lblAlcoholLevel.BackColor = System.Drawing.SystemColors.Control
        Me.lblAlcoholLevel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAlcoholLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAlcoholLevel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAlcoholLevel.Location = New System.Drawing.Point(368, 27)
        Me.lblAlcoholLevel.Name = "lblAlcoholLevel"
        Me.lblAlcoholLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAlcoholLevel.Size = New System.Drawing.Size(65, 25)
        Me.lblAlcoholLevel.TabIndex = 32
        Me.lblAlcoholLevel.Text = "Alcohol level:"
        '
        'lblAlcMethod
        '
        Me.lblAlcMethod.BackColor = System.Drawing.SystemColors.Control
        Me.lblAlcMethod.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAlcMethod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAlcMethod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAlcMethod.Location = New System.Drawing.Point(16, 26)
        Me.lblAlcMethod.Name = "lblAlcMethod"
        Me.lblAlcMethod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAlcMethod.Size = New System.Drawing.Size(89, 41)
        Me.lblAlcMethod.TabIndex = 31
        Me.lblAlcMethod.Text = "Alcohol measurement method:"
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(713, 469)
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
        Me.Text = "Conviction"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.fraConvictionDetails.ResumeLayout(False)
        Me.fraConvictionDetails.PerformLayout()
        Me.fraSentence.ResumeLayout(False)
        Me.fraSentence.PerformLayout()
        Me.fraMotoring.ResumeLayout(False)
        Me.fraMotoring.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class