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
	Public WithEvents cmdTaskLog As System.Windows.Forms.Button
	Public dlgMainOpen As System.Windows.Forms.OpenFileDialog
	Public dlgMainSave As System.Windows.Forms.SaveFileDialog
	Public dlgMainFont As System.Windows.Forms.FontDialog
	Public dlgMainColor As System.Windows.Forms.ColorDialog
	Public dlgMainPrint As System.Windows.Forms.PrintDialog
	Public WithEvents uctPMResizer As PMResizerControl.uctPMResizer
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents txtEventSubject As System.Windows.Forms.TextBox
	Public WithEvents txtEventType As System.Windows.Forms.TextBox
	Public WithEvents txtEventUser As System.Windows.Forms.TextBox
	Public WithEvents txtEventDescription As System.Windows.Forms.TextBox
	Public WithEvents txtEventDate As System.Windows.Forms.TextBox
	Public WithEvents cboEventSubject As System.Windows.Forms.ComboBox
	Public WithEvents cboEventType As System.Windows.Forms.ComboBox
	Public WithEvents lblEventUser As System.Windows.Forms.Label
	Public WithEvents lblEventDate As System.Windows.Forms.Label
	Public WithEvents lblEventDescription As System.Windows.Forms.Label
	Public WithEvents lblEventSubject As System.Windows.Forms.Label
	Public WithEvents lblEventType As System.Windows.Forms.Label
	Public WithEvents fraEvent As System.Windows.Forms.GroupBox
	Public WithEvents txtActionType As System.Windows.Forms.TextBox
	Public WithEvents txtTask As System.Windows.Forms.TextBox
	Public WithEvents txtTaskGroup As System.Windows.Forms.TextBox
	Public WithEvents txtWorkflow As System.Windows.Forms.TextBox
	Public WithEvents txtClient As System.Windows.Forms.TextBox
	Public WithEvents chkComplete As System.Windows.Forms.CheckBox
	Public WithEvents txtOutcome As System.Windows.Forms.TextBox
	Public WithEvents cboOutcome As System.Windows.Forms.ComboBox
	Public WithEvents txtOutcomeDate As System.Windows.Forms.TextBox
	Public WithEvents txtOutcomeDateTime As System.Windows.Forms.TextBox
	Public WithEvents lblOutcome As System.Windows.Forms.Label
	Public WithEvents lblOutcomeDate As System.Windows.Forms.Label
	Public WithEvents fraComplete As System.Windows.Forms.GroupBox
	Public WithEvents txtAllocateToUser As System.Windows.Forms.TextBox
	Public WithEvents txtAllocateToUserGroup As System.Windows.Forms.TextBox
	Public WithEvents cboUserGroup As System.Windows.Forms.ComboBox
	Public WithEvents cboUser As System.Windows.Forms.ComboBox
	Public WithEvents lblAllocateToUserGroup As System.Windows.Forms.Label
	Public WithEvents lblAllocateToUser As System.Windows.Forms.Label
	Public WithEvents fraAllocate As System.Windows.Forms.GroupBox
	Public WithEvents chkTaskUrgent As System.Windows.Forms.CheckBox
	Public WithEvents txtTaskDescription As System.Windows.Forms.TextBox
	Public WithEvents txtTaskDueDateTime As System.Windows.Forms.TextBox
	Public WithEvents txtTaskDueDate As System.Windows.Forms.TextBox
	Public WithEvents cboTask As System.Windows.Forms.ComboBox
	Public WithEvents cboActionType As System.Windows.Forms.ComboBox
	Public WithEvents cboTaskGroup As System.Windows.Forms.ComboBox
	Public WithEvents lblClient As System.Windows.Forms.Label
	Public WithEvents lblWorkflowInfo As System.Windows.Forms.Label
	Public WithEvents lblTaskDueDate As System.Windows.Forms.Label
	Public WithEvents lblTask As System.Windows.Forms.Label
	Public WithEvents lblUrgent As System.Windows.Forms.Label
	Public WithEvents lblTaskDescription As System.Windows.Forms.Label
	Public WithEvents lblActionType As System.Windows.Forms.Label
	Public WithEvents lblTaskGroup As System.Windows.Forms.Label
	Public WithEvents fraTask As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdTaskLog = New System.Windows.Forms.Button
		Me.dlgMainOpen = New System.Windows.Forms.OpenFileDialog
		Me.dlgMainSave = New System.Windows.Forms.SaveFileDialog
		Me.dlgMainFont = New System.Windows.Forms.FontDialog
		Me.dlgMainColor = New System.Windows.Forms.ColorDialog
		Me.dlgMainPrint = New System.Windows.Forms.PrintDialog
		Me.uctPMResizer = New PMResizerControl.uctPMResizer
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.tabMainTab = New System.Windows.Forms.TabControl
		Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
		Me.fraEvent = New System.Windows.Forms.GroupBox
		Me.txtEventSubject = New System.Windows.Forms.TextBox
		Me.txtEventType = New System.Windows.Forms.TextBox
		Me.txtEventUser = New System.Windows.Forms.TextBox
		Me.txtEventDescription = New System.Windows.Forms.TextBox
		Me.txtEventDate = New System.Windows.Forms.TextBox
		Me.cboEventSubject = New System.Windows.Forms.ComboBox
		Me.cboEventType = New System.Windows.Forms.ComboBox
		Me.lblEventUser = New System.Windows.Forms.Label
		Me.lblEventDate = New System.Windows.Forms.Label
		Me.lblEventDescription = New System.Windows.Forms.Label
		Me.lblEventSubject = New System.Windows.Forms.Label
		Me.lblEventType = New System.Windows.Forms.Label
		Me.fraTask = New System.Windows.Forms.GroupBox
		Me.txtActionType = New System.Windows.Forms.TextBox
		Me.txtTask = New System.Windows.Forms.TextBox
		Me.txtTaskGroup = New System.Windows.Forms.TextBox
		Me.txtWorkflow = New System.Windows.Forms.TextBox
		Me.txtClient = New System.Windows.Forms.TextBox
		Me.chkComplete = New System.Windows.Forms.CheckBox
		Me.fraComplete = New System.Windows.Forms.GroupBox
		Me.txtOutcome = New System.Windows.Forms.TextBox
		Me.cboOutcome = New System.Windows.Forms.ComboBox
		Me.txtOutcomeDate = New System.Windows.Forms.TextBox
		Me.txtOutcomeDateTime = New System.Windows.Forms.TextBox
		Me.lblOutcome = New System.Windows.Forms.Label
		Me.lblOutcomeDate = New System.Windows.Forms.Label
		Me.fraAllocate = New System.Windows.Forms.GroupBox
		Me.txtAllocateToUser = New System.Windows.Forms.TextBox
		Me.txtAllocateToUserGroup = New System.Windows.Forms.TextBox
		Me.cboUserGroup = New System.Windows.Forms.ComboBox
		Me.cboUser = New System.Windows.Forms.ComboBox
		Me.lblAllocateToUserGroup = New System.Windows.Forms.Label
		Me.lblAllocateToUser = New System.Windows.Forms.Label
		Me.chkTaskUrgent = New System.Windows.Forms.CheckBox
		Me.txtTaskDescription = New System.Windows.Forms.TextBox
		Me.txtTaskDueDateTime = New System.Windows.Forms.TextBox
		Me.txtTaskDueDate = New System.Windows.Forms.TextBox
		Me.cboTask = New System.Windows.Forms.ComboBox
		Me.cboActionType = New System.Windows.Forms.ComboBox
		Me.cboTaskGroup = New System.Windows.Forms.ComboBox
		Me.lblClient = New System.Windows.Forms.Label
		Me.lblWorkflowInfo = New System.Windows.Forms.Label
		Me.lblTaskDueDate = New System.Windows.Forms.Label
		Me.lblTask = New System.Windows.Forms.Label
		Me.lblUrgent = New System.Windows.Forms.Label
		Me.lblTaskDescription = New System.Windows.Forms.Label
		Me.lblActionType = New System.Windows.Forms.Label
		Me.lblTaskGroup = New System.Windows.Forms.Label
		Me.tabMainTab.SuspendLayout()
		Me._tabMainTab_TabPage0.SuspendLayout()
		Me.fraEvent.SuspendLayout()
		Me.fraTask.SuspendLayout()
		Me.fraComplete.SuspendLayout()
		Me.fraAllocate.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdTaskLog
		' 
		Me.cmdTaskLog.BackColor = System.Drawing.SystemColors.Control
		Me.cmdTaskLog.CausesValidation = True
		Me.cmdTaskLog.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdTaskLog.Enabled = True
		Me.cmdTaskLog.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdTaskLog.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdTaskLog.Location = New System.Drawing.Point(8, 408)
		Me.cmdTaskLog.Name = "cmdTaskLog"
		Me.cmdTaskLog.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdTaskLog.Size = New System.Drawing.Size(73, 22)
		Me.cmdTaskLog.TabIndex = 28
		Me.cmdTaskLog.TabStop = True
		Me.cmdTaskLog.Text = "Task Log"
		Me.cmdTaskLog.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdTaskLog.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' uctPMResizer
		' 
		Me.uctPMResizer.Location = New System.Drawing.Point(392, 400)
		Me.uctPMResizer.Name = "uctPMResizer"
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(576, 408)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 30
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(496, 408)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 29
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tabMainTab
		' 
		Me.tabMainTab.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabMainTab.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
		Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMainTab.ItemSize = New System.Drawing.Size(128, 18)
		Me.tabMainTab.Location = New System.Drawing.Point(1, 0)
		Me.tabMainTab.Multiline = True
		Me.tabMainTab.Name = "tabMainTab"
		Me.tabMainTab.Size = New System.Drawing.Size(652, 405)
		Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMainTab.TabIndex = 31
		Me.tabMainTab.TabStop = False
		' 
		' _tabMainTab_TabPage0
		' 
		Me._tabMainTab_TabPage0.Controls.Add(Me.fraEvent)
		Me._tabMainTab_TabPage0.Controls.Add(Me.fraTask)
		Me._tabMainTab_TabPage0.Text = "&1 - Workflow Task"
		' 
		' fraEvent
		' 
		Me.fraEvent.BackColor = System.Drawing.SystemColors.Control
		Me.fraEvent.Controls.Add(Me.txtEventSubject)
		Me.fraEvent.Controls.Add(Me.txtEventType)
		Me.fraEvent.Controls.Add(Me.txtEventUser)
		Me.fraEvent.Controls.Add(Me.txtEventDescription)
		Me.fraEvent.Controls.Add(Me.txtEventDate)
		Me.fraEvent.Controls.Add(Me.cboEventSubject)
		Me.fraEvent.Controls.Add(Me.cboEventType)
		Me.fraEvent.Controls.Add(Me.lblEventUser)
		Me.fraEvent.Controls.Add(Me.lblEventDate)
		Me.fraEvent.Controls.Add(Me.lblEventDescription)
		Me.fraEvent.Controls.Add(Me.lblEventSubject)
		Me.fraEvent.Controls.Add(Me.lblEventType)
		Me.fraEvent.Enabled = True
		Me.fraEvent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraEvent.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraEvent.Location = New System.Drawing.Point(8, 4)
		Me.fraEvent.Name = "fraEvent"
		Me.fraEvent.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraEvent.Size = New System.Drawing.Size(633, 97)
		Me.fraEvent.TabIndex = 32
		Me.fraEvent.Text = "Event"
		Me.fraEvent.Visible = True
		' 
		' txtEventSubject
		' 
		Me.txtEventSubject.AcceptsReturn = True
		Me.txtEventSubject.AutoSize = False
		Me.txtEventSubject.BackColor = System.Drawing.SystemColors.Window
		Me.txtEventSubject.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtEventSubject.CausesValidation = True
		Me.txtEventSubject.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtEventSubject.Enabled = True
		Me.txtEventSubject.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtEventSubject.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtEventSubject.HideSelection = True
		Me.txtEventSubject.Location = New System.Drawing.Point(280, 40)
		Me.txtEventSubject.MaxLength = 0
		Me.txtEventSubject.Multiline = False
		Me.txtEventSubject.Name = "txtEventSubject"
		Me.txtEventSubject.ReadOnly = False
		Me.txtEventSubject.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtEventSubject.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtEventSubject.Size = New System.Drawing.Size(33, 19)
		Me.txtEventSubject.TabIndex = 4
		Me.txtEventSubject.TabStop = True
		Me.txtEventSubject.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtEventSubject.Visible = False
		' 
		' txtEventType
		' 
		Me.txtEventType.AcceptsReturn = True
		Me.txtEventType.AutoSize = False
		Me.txtEventType.BackColor = System.Drawing.SystemColors.Window
		Me.txtEventType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtEventType.CausesValidation = True
		Me.txtEventType.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtEventType.Enabled = True
		Me.txtEventType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtEventType.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtEventType.HideSelection = True
		Me.txtEventType.Location = New System.Drawing.Point(280, 16)
		Me.txtEventType.MaxLength = 0
		Me.txtEventType.Multiline = False
		Me.txtEventType.Name = "txtEventType"
		Me.txtEventType.ReadOnly = False
		Me.txtEventType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtEventType.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtEventType.Size = New System.Drawing.Size(33, 19)
		Me.txtEventType.TabIndex = 1
		Me.txtEventType.TabStop = True
		Me.txtEventType.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtEventType.Visible = False
		' 
		' txtEventUser
		' 
		Me.txtEventUser.AcceptsReturn = True
		Me.txtEventUser.AutoSize = False
		Me.txtEventUser.BackColor = System.Drawing.SystemColors.Window
		Me.txtEventUser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtEventUser.CausesValidation = True
		Me.txtEventUser.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtEventUser.Enabled = False
		Me.txtEventUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtEventUser.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtEventUser.HideSelection = True
		Me.txtEventUser.Location = New System.Drawing.Point(408, 40)
		Me.txtEventUser.MaxLength = 0
		Me.txtEventUser.Multiline = False
		Me.txtEventUser.Name = "txtEventUser"
		Me.txtEventUser.ReadOnly = False
		Me.txtEventUser.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtEventUser.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtEventUser.Size = New System.Drawing.Size(145, 21)
		Me.txtEventUser.TabIndex = 5
		Me.txtEventUser.TabStop = True
		Me.txtEventUser.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtEventUser.Visible = True
		' 
		' txtEventDescription
		' 
		Me.txtEventDescription.AcceptsReturn = True
		Me.txtEventDescription.AutoSize = False
		Me.txtEventDescription.BackColor = System.Drawing.SystemColors.Window
		Me.txtEventDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtEventDescription.CausesValidation = True
		Me.txtEventDescription.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtEventDescription.Enabled = True
		Me.txtEventDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtEventDescription.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtEventDescription.HideSelection = True
		Me.txtEventDescription.Location = New System.Drawing.Point(96, 64)
		Me.txtEventDescription.MaxLength = 0
		Me.txtEventDescription.Multiline = False
		Me.txtEventDescription.Name = "txtEventDescription"
		Me.txtEventDescription.ReadOnly = False
		Me.txtEventDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtEventDescription.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtEventDescription.Size = New System.Drawing.Size(529, 21)
		Me.txtEventDescription.TabIndex = 6
		Me.txtEventDescription.TabStop = True
		Me.txtEventDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtEventDescription.Visible = True
		' 
		' txtEventDate
		' 
		Me.txtEventDate.AcceptsReturn = True
		Me.txtEventDate.AutoSize = False
		Me.txtEventDate.BackColor = System.Drawing.SystemColors.Window
		Me.txtEventDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtEventDate.CausesValidation = True
		Me.txtEventDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtEventDate.Enabled = False
		Me.txtEventDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtEventDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtEventDate.HideSelection = True
		Me.txtEventDate.Location = New System.Drawing.Point(408, 16)
		Me.txtEventDate.MaxLength = 0
		Me.txtEventDate.Multiline = False
		Me.txtEventDate.Name = "txtEventDate"
		Me.txtEventDate.ReadOnly = False
		Me.txtEventDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtEventDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtEventDate.Size = New System.Drawing.Size(73, 21)
		Me.txtEventDate.TabIndex = 2
		Me.txtEventDate.TabStop = True
		Me.txtEventDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtEventDate.Visible = True
		' 
		' cboEventSubject
		' 
		Me.cboEventSubject.BackColor = System.Drawing.SystemColors.Window
		Me.cboEventSubject.CausesValidation = True
		Me.cboEventSubject.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboEventSubject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboEventSubject.Enabled = True
		Me.cboEventSubject.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboEventSubject.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboEventSubject.IntegralHeight = True
		Me.cboEventSubject.Location = New System.Drawing.Point(96, 40)
		Me.cboEventSubject.Name = "cboEventSubject"
		Me.cboEventSubject.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboEventSubject.Size = New System.Drawing.Size(249, 21)
		Me.cboEventSubject.Sorted = False
		Me.cboEventSubject.TabIndex = 3
		Me.cboEventSubject.TabStop = True
		Me.cboEventSubject.Visible = True
		' 
		' cboEventType
		' 
		Me.cboEventType.BackColor = System.Drawing.SystemColors.Window
		Me.cboEventType.CausesValidation = True
		Me.cboEventType.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboEventType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboEventType.Enabled = True
		Me.cboEventType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboEventType.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboEventType.IntegralHeight = True
		Me.cboEventType.Location = New System.Drawing.Point(96, 16)
		Me.cboEventType.Name = "cboEventType"
		Me.cboEventType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboEventType.Size = New System.Drawing.Size(249, 21)
		Me.cboEventType.Sorted = False
		Me.cboEventType.TabIndex = 0
		Me.cboEventType.TabStop = True
		Me.cboEventType.Visible = True
		' 
		' lblEventUser
		' 
		Me.lblEventUser.AutoSize = True
		Me.lblEventUser.BackColor = System.Drawing.SystemColors.Control
		Me.lblEventUser.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblEventUser.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblEventUser.Enabled = True
		Me.lblEventUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblEventUser.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblEventUser.Location = New System.Drawing.Point(368, 44)
		Me.lblEventUser.Name = "lblEventUser"
		Me.lblEventUser.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblEventUser.Size = New System.Drawing.Size(31, 13)
		Me.lblEventUser.TabIndex = 38
		Me.lblEventUser.Text = "User:"
		Me.lblEventUser.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblEventUser.UseMnemonic = True
		Me.lblEventUser.Visible = True
		' 
		' lblEventDate
		' 
		Me.lblEventDate.AutoSize = True
		Me.lblEventDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblEventDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblEventDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblEventDate.Enabled = True
		Me.lblEventDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblEventDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblEventDate.Location = New System.Drawing.Point(367, 20)
		Me.lblEventDate.Name = "lblEventDate"
		Me.lblEventDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblEventDate.Size = New System.Drawing.Size(32, 13)
		Me.lblEventDate.TabIndex = 37
		Me.lblEventDate.Text = "Date:"
		Me.lblEventDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblEventDate.UseMnemonic = True
		Me.lblEventDate.Visible = True
		' 
		' lblEventDescription
		' 
		Me.lblEventDescription.AutoSize = True
		Me.lblEventDescription.BackColor = System.Drawing.SystemColors.Control
		Me.lblEventDescription.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblEventDescription.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblEventDescription.Enabled = True
		Me.lblEventDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblEventDescription.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblEventDescription.Location = New System.Drawing.Point(16, 68)
		Me.lblEventDescription.Name = "lblEventDescription"
		Me.lblEventDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblEventDescription.Size = New System.Drawing.Size(69, 13)
		Me.lblEventDescription.TabIndex = 36
		Me.lblEventDescription.Text = "Description:"
		Me.lblEventDescription.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblEventDescription.UseMnemonic = True
		Me.lblEventDescription.Visible = True
		' 
		' lblEventSubject
		' 
		Me.lblEventSubject.AutoSize = True
		Me.lblEventSubject.BackColor = System.Drawing.SystemColors.Control
		Me.lblEventSubject.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblEventSubject.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblEventSubject.Enabled = True
		Me.lblEventSubject.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblEventSubject.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblEventSubject.Location = New System.Drawing.Point(37, 44)
		Me.lblEventSubject.Name = "lblEventSubject"
		Me.lblEventSubject.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblEventSubject.Size = New System.Drawing.Size(48, 13)
		Me.lblEventSubject.TabIndex = 35
		Me.lblEventSubject.Text = "Subject:"
		Me.lblEventSubject.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblEventSubject.UseMnemonic = True
		Me.lblEventSubject.Visible = True
		' 
		' lblEventType
		' 
		Me.lblEventType.AutoSize = True
		Me.lblEventType.BackColor = System.Drawing.SystemColors.Control
		Me.lblEventType.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblEventType.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblEventType.Enabled = True
		Me.lblEventType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblEventType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblEventType.Location = New System.Drawing.Point(52, 20)
		Me.lblEventType.Name = "lblEventType"
		Me.lblEventType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblEventType.Size = New System.Drawing.Size(33, 13)
		Me.lblEventType.TabIndex = 34
		Me.lblEventType.Text = "Type:"
		Me.lblEventType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblEventType.UseMnemonic = True
		Me.lblEventType.Visible = True
		' 
		' fraTask
		' 
		Me.fraTask.BackColor = System.Drawing.SystemColors.Control
		Me.fraTask.Controls.Add(Me.txtActionType)
		Me.fraTask.Controls.Add(Me.txtTask)
		Me.fraTask.Controls.Add(Me.txtTaskGroup)
		Me.fraTask.Controls.Add(Me.txtWorkflow)
		Me.fraTask.Controls.Add(Me.txtClient)
		Me.fraTask.Controls.Add(Me.chkComplete)
		Me.fraTask.Controls.Add(Me.fraComplete)
		Me.fraTask.Controls.Add(Me.fraAllocate)
		Me.fraTask.Controls.Add(Me.chkTaskUrgent)
		Me.fraTask.Controls.Add(Me.txtTaskDescription)
		Me.fraTask.Controls.Add(Me.txtTaskDueDateTime)
		Me.fraTask.Controls.Add(Me.txtTaskDueDate)
		Me.fraTask.Controls.Add(Me.cboTask)
		Me.fraTask.Controls.Add(Me.cboActionType)
		Me.fraTask.Controls.Add(Me.cboTaskGroup)
		Me.fraTask.Controls.Add(Me.lblClient)
		Me.fraTask.Controls.Add(Me.lblWorkflowInfo)
		Me.fraTask.Controls.Add(Me.lblTaskDueDate)
		Me.fraTask.Controls.Add(Me.lblTask)
		Me.fraTask.Controls.Add(Me.lblUrgent)
		Me.fraTask.Controls.Add(Me.lblTaskDescription)
		Me.fraTask.Controls.Add(Me.lblActionType)
		Me.fraTask.Controls.Add(Me.lblTaskGroup)
		Me.fraTask.Enabled = False
		Me.fraTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraTask.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraTask.Location = New System.Drawing.Point(8, 108)
		Me.fraTask.Name = "fraTask"
		Me.fraTask.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraTask.Size = New System.Drawing.Size(633, 265)
		Me.fraTask.TabIndex = 33
		Me.fraTask.Text = "Task"
		Me.fraTask.Visible = True
		' 
		' txtActionType
		' 
		Me.txtActionType.AcceptsReturn = True
		Me.txtActionType.AutoSize = False
		Me.txtActionType.BackColor = System.Drawing.SystemColors.Window
		Me.txtActionType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtActionType.CausesValidation = True
		Me.txtActionType.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtActionType.Enabled = True
		Me.txtActionType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtActionType.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtActionType.HideSelection = True
		Me.txtActionType.Location = New System.Drawing.Point(232, 40)
		Me.txtActionType.MaxLength = 0
		Me.txtActionType.Multiline = False
		Me.txtActionType.Name = "txtActionType"
		Me.txtActionType.ReadOnly = False
		Me.txtActionType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtActionType.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtActionType.Size = New System.Drawing.Size(33, 19)
		Me.txtActionType.TabIndex = 12
		Me.txtActionType.TabStop = True
		Me.txtActionType.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtActionType.Visible = False
		' 
		' txtTask
		' 
		Me.txtTask.AcceptsReturn = True
		Me.txtTask.AutoSize = False
		Me.txtTask.BackColor = System.Drawing.SystemColors.Window
		Me.txtTask.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtTask.CausesValidation = True
		Me.txtTask.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtTask.Enabled = True
		Me.txtTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtTask.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtTask.HideSelection = True
		Me.txtTask.Location = New System.Drawing.Point(560, 16)
		Me.txtTask.MaxLength = 0
		Me.txtTask.Multiline = False
		Me.txtTask.Name = "txtTask"
		Me.txtTask.ReadOnly = False
		Me.txtTask.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTask.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTask.Size = New System.Drawing.Size(33, 19)
		Me.txtTask.TabIndex = 10
		Me.txtTask.TabStop = True
		Me.txtTask.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTask.Visible = False
		' 
		' txtTaskGroup
		' 
		Me.txtTaskGroup.AcceptsReturn = True
		Me.txtTaskGroup.AutoSize = False
		Me.txtTaskGroup.BackColor = System.Drawing.SystemColors.Window
		Me.txtTaskGroup.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtTaskGroup.CausesValidation = True
		Me.txtTaskGroup.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtTaskGroup.Enabled = True
		Me.txtTaskGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtTaskGroup.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtTaskGroup.HideSelection = True
		Me.txtTaskGroup.Location = New System.Drawing.Point(240, 16)
		Me.txtTaskGroup.MaxLength = 0
		Me.txtTaskGroup.Multiline = False
		Me.txtTaskGroup.Name = "txtTaskGroup"
		Me.txtTaskGroup.ReadOnly = False
		Me.txtTaskGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTaskGroup.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTaskGroup.Size = New System.Drawing.Size(33, 19)
		Me.txtTaskGroup.TabIndex = 8
		Me.txtTaskGroup.TabStop = True
		Me.txtTaskGroup.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTaskGroup.Visible = False
		' 
		' txtWorkflow
		' 
		Me.txtWorkflow.AcceptsReturn = True
		Me.txtWorkflow.AutoSize = False
		Me.txtWorkflow.BackColor = System.Drawing.SystemColors.Menu
		Me.txtWorkflow.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtWorkflow.CausesValidation = True
		Me.txtWorkflow.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtWorkflow.Enabled = False
		Me.txtWorkflow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtWorkflow.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtWorkflow.HideSelection = True
		Me.txtWorkflow.Location = New System.Drawing.Point(96, 112)
		Me.txtWorkflow.MaxLength = 0
		Me.txtWorkflow.Multiline = False
		Me.txtWorkflow.Name = "txtWorkflow"
		Me.txtWorkflow.ReadOnly = False
		Me.txtWorkflow.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtWorkflow.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtWorkflow.Size = New System.Drawing.Size(529, 21)
		Me.txtWorkflow.TabIndex = 17
		Me.txtWorkflow.TabStop = True
		Me.txtWorkflow.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtWorkflow.Visible = True
		' 
		' txtClient
		' 
		Me.txtClient.AcceptsReturn = True
		Me.txtClient.AutoSize = False
		Me.txtClient.BackColor = System.Drawing.SystemColors.Window
		Me.txtClient.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtClient.CausesValidation = True
		Me.txtClient.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtClient.Enabled = True
		Me.txtClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtClient.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtClient.HideSelection = True
		Me.txtClient.Location = New System.Drawing.Point(96, 88)
		Me.txtClient.MaxLength = 0
		Me.txtClient.Multiline = False
		Me.txtClient.Name = "txtClient"
		Me.txtClient.ReadOnly = False
		Me.txtClient.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtClient.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtClient.Size = New System.Drawing.Size(249, 21)
		Me.txtClient.TabIndex = 16
		Me.txtClient.TabStop = True
		Me.txtClient.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtClient.Visible = True
		' 
		' chkComplete
		' 
		Me.chkComplete.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkComplete.BackColor = System.Drawing.SystemColors.Control
		Me.chkComplete.CausesValidation = True
		Me.chkComplete.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkComplete.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkComplete.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkComplete.Enabled = True
		Me.chkComplete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkComplete.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkComplete.Location = New System.Drawing.Point(80, 206)
		Me.chkComplete.Name = "chkComplete"
		Me.chkComplete.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkComplete.Size = New System.Drawing.Size(21, 21)
		Me.chkComplete.TabIndex = 23
		Me.chkComplete.TabStop = True
		Me.chkComplete.Text = ""
		Me.chkComplete.Visible = True
		' 
		' fraComplete
		' 
		Me.fraComplete.BackColor = System.Drawing.SystemColors.Control
		Me.fraComplete.Controls.Add(Me.txtOutcome)
		Me.fraComplete.Controls.Add(Me.cboOutcome)
		Me.fraComplete.Controls.Add(Me.txtOutcomeDate)
		Me.fraComplete.Controls.Add(Me.txtOutcomeDateTime)
		Me.fraComplete.Controls.Add(Me.lblOutcome)
		Me.fraComplete.Controls.Add(Me.lblOutcomeDate)
		Me.fraComplete.Enabled = True
		Me.fraComplete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraComplete.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraComplete.Location = New System.Drawing.Point(8, 208)
		Me.fraComplete.Name = "fraComplete"
		Me.fraComplete.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraComplete.Size = New System.Drawing.Size(617, 49)
		Me.fraComplete.TabIndex = 48
		Me.fraComplete.Text = "Complete"
		Me.fraComplete.Visible = True
		' 
		' txtOutcome
		' 
		Me.txtOutcome.AcceptsReturn = True
		Me.txtOutcome.AutoSize = False
		Me.txtOutcome.BackColor = System.Drawing.SystemColors.Window
		Me.txtOutcome.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtOutcome.CausesValidation = True
		Me.txtOutcome.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtOutcome.Enabled = True
		Me.txtOutcome.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtOutcome.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtOutcome.HideSelection = True
		Me.txtOutcome.Location = New System.Drawing.Point(200, 16)
		Me.txtOutcome.MaxLength = 0
		Me.txtOutcome.Multiline = False
		Me.txtOutcome.Name = "txtOutcome"
		Me.txtOutcome.ReadOnly = False
		Me.txtOutcome.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtOutcome.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtOutcome.Size = New System.Drawing.Size(33, 19)
		Me.txtOutcome.TabIndex = 25
		Me.txtOutcome.TabStop = True
		Me.txtOutcome.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtOutcome.Visible = False
		' 
		' cboOutcome
		' 
		Me.cboOutcome.BackColor = System.Drawing.SystemColors.Window
		Me.cboOutcome.CausesValidation = True
		Me.cboOutcome.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboOutcome.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboOutcome.Enabled = True
		Me.cboOutcome.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboOutcome.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboOutcome.IntegralHeight = True
		Me.cboOutcome.Location = New System.Drawing.Point(96, 16)
		Me.cboOutcome.Name = "cboOutcome"
		Me.cboOutcome.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboOutcome.Size = New System.Drawing.Size(217, 21)
		Me.cboOutcome.Sorted = False
		Me.cboOutcome.TabIndex = 24
		Me.cboOutcome.TabStop = True
		Me.cboOutcome.Visible = True
		' 
		' txtOutcomeDate
		' 
		Me.txtOutcomeDate.AcceptsReturn = True
		Me.txtOutcomeDate.AutoSize = False
		Me.txtOutcomeDate.BackColor = System.Drawing.SystemColors.Window
		Me.txtOutcomeDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtOutcomeDate.CausesValidation = True
		Me.txtOutcomeDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtOutcomeDate.Enabled = False
		Me.txtOutcomeDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtOutcomeDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtOutcomeDate.HideSelection = True
		Me.txtOutcomeDate.Location = New System.Drawing.Point(464, 16)
		Me.txtOutcomeDate.MaxLength = 0
		Me.txtOutcomeDate.Multiline = False
		Me.txtOutcomeDate.Name = "txtOutcomeDate"
		Me.txtOutcomeDate.ReadOnly = False
		Me.txtOutcomeDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtOutcomeDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtOutcomeDate.Size = New System.Drawing.Size(73, 21)
		Me.txtOutcomeDate.TabIndex = 26
		Me.txtOutcomeDate.TabStop = True
		Me.txtOutcomeDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtOutcomeDate.Visible = True
		' 
		' txtOutcomeDateTime
		' 
		Me.txtOutcomeDateTime.AcceptsReturn = True
		Me.txtOutcomeDateTime.AutoSize = False
		Me.txtOutcomeDateTime.BackColor = System.Drawing.SystemColors.Window
		Me.txtOutcomeDateTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtOutcomeDateTime.CausesValidation = True
		Me.txtOutcomeDateTime.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtOutcomeDateTime.Enabled = False
		Me.txtOutcomeDateTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtOutcomeDateTime.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtOutcomeDateTime.HideSelection = True
		Me.txtOutcomeDateTime.Location = New System.Drawing.Point(544, 16)
		Me.txtOutcomeDateTime.MaxLength = 0
		Me.txtOutcomeDateTime.Multiline = False
		Me.txtOutcomeDateTime.Name = "txtOutcomeDateTime"
		Me.txtOutcomeDateTime.ReadOnly = False
		Me.txtOutcomeDateTime.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtOutcomeDateTime.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtOutcomeDateTime.Size = New System.Drawing.Size(65, 21)
		Me.txtOutcomeDateTime.TabIndex = 27
		Me.txtOutcomeDateTime.TabStop = True
		Me.txtOutcomeDateTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtOutcomeDateTime.Visible = True
		' 
		' lblOutcome
		' 
		Me.lblOutcome.AutoSize = True
		Me.lblOutcome.BackColor = System.Drawing.SystemColors.Control
		Me.lblOutcome.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblOutcome.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblOutcome.Enabled = True
		Me.lblOutcome.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblOutcome.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblOutcome.Location = New System.Drawing.Point(32, 20)
		Me.lblOutcome.Name = "lblOutcome"
		Me.lblOutcome.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblOutcome.Size = New System.Drawing.Size(56, 13)
		Me.lblOutcome.TabIndex = 50
		Me.lblOutcome.Text = "Outcome:"
		Me.lblOutcome.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblOutcome.UseMnemonic = True
		Me.lblOutcome.Visible = True
		' 
		' lblOutcomeDate
		' 
		Me.lblOutcomeDate.AutoSize = True
		Me.lblOutcomeDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblOutcomeDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblOutcomeDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblOutcomeDate.Enabled = True
		Me.lblOutcomeDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblOutcomeDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblOutcomeDate.Location = New System.Drawing.Point(328, 20)
		Me.lblOutcomeDate.Name = "lblOutcomeDate"
		Me.lblOutcomeDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblOutcomeDate.Size = New System.Drawing.Size(128, 13)
		Me.lblOutcomeDate.TabIndex = 49
		Me.lblOutcomeDate.Text = "Outcome Date / Time:"
		Me.lblOutcomeDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblOutcomeDate.UseMnemonic = True
		Me.lblOutcomeDate.Visible = True
		' 
		' fraAllocate
		' 
		Me.fraAllocate.BackColor = System.Drawing.SystemColors.Control
		Me.fraAllocate.Controls.Add(Me.txtAllocateToUser)
		Me.fraAllocate.Controls.Add(Me.txtAllocateToUserGroup)
		Me.fraAllocate.Controls.Add(Me.cboUserGroup)
		Me.fraAllocate.Controls.Add(Me.cboUser)
		Me.fraAllocate.Controls.Add(Me.lblAllocateToUserGroup)
		Me.fraAllocate.Controls.Add(Me.lblAllocateToUser)
		Me.fraAllocate.Enabled = True
		Me.fraAllocate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraAllocate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraAllocate.Location = New System.Drawing.Point(8, 156)
		Me.fraAllocate.Name = "fraAllocate"
		Me.fraAllocate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraAllocate.Size = New System.Drawing.Size(617, 49)
		Me.fraAllocate.TabIndex = 45
		Me.fraAllocate.Text = "Allocate To"
		Me.fraAllocate.Visible = True
		' 
		' txtAllocateToUser
		' 
		Me.txtAllocateToUser.AcceptsReturn = True
		Me.txtAllocateToUser.AutoSize = False
		Me.txtAllocateToUser.BackColor = System.Drawing.SystemColors.Window
		Me.txtAllocateToUser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtAllocateToUser.CausesValidation = True
		Me.txtAllocateToUser.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAllocateToUser.Enabled = True
		Me.txtAllocateToUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtAllocateToUser.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAllocateToUser.HideSelection = True
		Me.txtAllocateToUser.Location = New System.Drawing.Point(472, 16)
		Me.txtAllocateToUser.MaxLength = 0
		Me.txtAllocateToUser.Multiline = False
		Me.txtAllocateToUser.Name = "txtAllocateToUser"
		Me.txtAllocateToUser.ReadOnly = False
		Me.txtAllocateToUser.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAllocateToUser.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAllocateToUser.Size = New System.Drawing.Size(41, 19)
		Me.txtAllocateToUser.TabIndex = 22
		Me.txtAllocateToUser.TabStop = True
		Me.txtAllocateToUser.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtAllocateToUser.Visible = False
		' 
		' txtAllocateToUserGroup
		' 
		Me.txtAllocateToUserGroup.AcceptsReturn = True
		Me.txtAllocateToUserGroup.AutoSize = False
		Me.txtAllocateToUserGroup.BackColor = System.Drawing.SystemColors.Window
		Me.txtAllocateToUserGroup.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtAllocateToUserGroup.CausesValidation = True
		Me.txtAllocateToUserGroup.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAllocateToUserGroup.Enabled = True
		Me.txtAllocateToUserGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtAllocateToUserGroup.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAllocateToUserGroup.HideSelection = True
		Me.txtAllocateToUserGroup.Location = New System.Drawing.Point(192, 16)
		Me.txtAllocateToUserGroup.MaxLength = 0
		Me.txtAllocateToUserGroup.Multiline = False
		Me.txtAllocateToUserGroup.Name = "txtAllocateToUserGroup"
		Me.txtAllocateToUserGroup.ReadOnly = False
		Me.txtAllocateToUserGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAllocateToUserGroup.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAllocateToUserGroup.Size = New System.Drawing.Size(33, 19)
		Me.txtAllocateToUserGroup.TabIndex = 20
		Me.txtAllocateToUserGroup.TabStop = True
		Me.txtAllocateToUserGroup.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtAllocateToUserGroup.Visible = False
		' 
		' cboUserGroup
		' 
		Me.cboUserGroup.BackColor = System.Drawing.SystemColors.Window
		Me.cboUserGroup.CausesValidation = True
		Me.cboUserGroup.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboUserGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboUserGroup.Enabled = True
		Me.cboUserGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboUserGroup.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboUserGroup.IntegralHeight = True
		Me.cboUserGroup.Location = New System.Drawing.Point(96, 16)
		Me.cboUserGroup.Name = "cboUserGroup"
		Me.cboUserGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboUserGroup.Size = New System.Drawing.Size(241, 21)
		Me.cboUserGroup.Sorted = False
		Me.cboUserGroup.TabIndex = 19
		Me.cboUserGroup.TabStop = True
		Me.cboUserGroup.Visible = True
		' 
		' cboUser
		' 
		Me.cboUser.BackColor = System.Drawing.SystemColors.Window
		Me.cboUser.CausesValidation = True
		Me.cboUser.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboUser.Enabled = True
		Me.cboUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboUser.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboUser.IntegralHeight = True
		Me.cboUser.Location = New System.Drawing.Point(400, 16)
		Me.cboUser.Name = "cboUser"
		Me.cboUser.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboUser.Size = New System.Drawing.Size(185, 21)
		Me.cboUser.Sorted = False
		Me.cboUser.TabIndex = 21
		Me.cboUser.TabStop = True
		Me.cboUser.Visible = True
		' 
		' lblAllocateToUserGroup
		' 
		Me.lblAllocateToUserGroup.AutoSize = True
		Me.lblAllocateToUserGroup.BackColor = System.Drawing.SystemColors.Control
		Me.lblAllocateToUserGroup.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAllocateToUserGroup.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAllocateToUserGroup.Enabled = True
		Me.lblAllocateToUserGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAllocateToUserGroup.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAllocateToUserGroup.Location = New System.Drawing.Point(16, 20)
		Me.lblAllocateToUserGroup.Name = "lblAllocateToUserGroup"
		Me.lblAllocateToUserGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAllocateToUserGroup.Size = New System.Drawing.Size(70, 13)
		Me.lblAllocateToUserGroup.TabIndex = 47
		Me.lblAllocateToUserGroup.Text = "User Group:"
		Me.lblAllocateToUserGroup.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAllocateToUserGroup.UseMnemonic = True
		Me.lblAllocateToUserGroup.Visible = True
		' 
		' lblAllocateToUser
		' 
		Me.lblAllocateToUser.AutoSize = True
		Me.lblAllocateToUser.BackColor = System.Drawing.SystemColors.Control
		Me.lblAllocateToUser.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAllocateToUser.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAllocateToUser.Enabled = True
		Me.lblAllocateToUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAllocateToUser.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAllocateToUser.Location = New System.Drawing.Point(360, 20)
		Me.lblAllocateToUser.Name = "lblAllocateToUser"
		Me.lblAllocateToUser.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAllocateToUser.Size = New System.Drawing.Size(31, 13)
		Me.lblAllocateToUser.TabIndex = 46
		Me.lblAllocateToUser.Text = "User:"
		Me.lblAllocateToUser.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAllocateToUser.UseMnemonic = True
		Me.lblAllocateToUser.Visible = True
		' 
		' chkTaskUrgent
		' 
		Me.chkTaskUrgent.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkTaskUrgent.BackColor = System.Drawing.SystemColors.Control
		Me.chkTaskUrgent.CausesValidation = True
		Me.chkTaskUrgent.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkTaskUrgent.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkTaskUrgent.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkTaskUrgent.Enabled = True
		Me.chkTaskUrgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkTaskUrgent.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkTaskUrgent.Location = New System.Drawing.Point(96, 136)
		Me.chkTaskUrgent.Name = "chkTaskUrgent"
		Me.chkTaskUrgent.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkTaskUrgent.Size = New System.Drawing.Size(21, 21)
		Me.chkTaskUrgent.TabIndex = 18
		Me.chkTaskUrgent.TabStop = True
		Me.chkTaskUrgent.Text = ""
		Me.chkTaskUrgent.Visible = True
		' 
		' txtTaskDescription
		' 
		Me.txtTaskDescription.AcceptsReturn = True
		Me.txtTaskDescription.AutoSize = False
		Me.txtTaskDescription.BackColor = System.Drawing.SystemColors.Window
		Me.txtTaskDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtTaskDescription.CausesValidation = True
		Me.txtTaskDescription.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtTaskDescription.Enabled = True
		Me.txtTaskDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtTaskDescription.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtTaskDescription.HideSelection = True
		Me.txtTaskDescription.Location = New System.Drawing.Point(96, 64)
		Me.txtTaskDescription.MaxLength = 0
		Me.txtTaskDescription.Multiline = False
		Me.txtTaskDescription.Name = "txtTaskDescription"
		Me.txtTaskDescription.ReadOnly = False
		Me.txtTaskDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTaskDescription.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTaskDescription.Size = New System.Drawing.Size(529, 21)
		Me.txtTaskDescription.TabIndex = 15
		Me.txtTaskDescription.TabStop = True
		Me.txtTaskDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTaskDescription.Visible = True
		' 
		' txtTaskDueDateTime
		' 
		Me.txtTaskDueDateTime.AcceptsReturn = True
		Me.txtTaskDueDateTime.AutoSize = False
		Me.txtTaskDueDateTime.BackColor = System.Drawing.SystemColors.Window
		Me.txtTaskDueDateTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtTaskDueDateTime.CausesValidation = True
		Me.txtTaskDueDateTime.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtTaskDueDateTime.Enabled = True
		Me.txtTaskDueDateTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtTaskDueDateTime.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtTaskDueDateTime.HideSelection = True
		Me.txtTaskDueDateTime.Location = New System.Drawing.Point(560, 40)
		Me.txtTaskDueDateTime.MaxLength = 0
		Me.txtTaskDueDateTime.Multiline = False
		Me.txtTaskDueDateTime.Name = "txtTaskDueDateTime"
		Me.txtTaskDueDateTime.ReadOnly = False
		Me.txtTaskDueDateTime.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTaskDueDateTime.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTaskDueDateTime.Size = New System.Drawing.Size(65, 21)
		Me.txtTaskDueDateTime.TabIndex = 14
		Me.txtTaskDueDateTime.TabStop = True
		Me.txtTaskDueDateTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTaskDueDateTime.Visible = True
		' 
		' txtTaskDueDate
		' 
		Me.txtTaskDueDate.AcceptsReturn = True
		Me.txtTaskDueDate.AutoSize = False
		Me.txtTaskDueDate.BackColor = System.Drawing.SystemColors.Window
		Me.txtTaskDueDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtTaskDueDate.CausesValidation = True
		Me.txtTaskDueDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtTaskDueDate.Enabled = True
		Me.txtTaskDueDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtTaskDueDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtTaskDueDate.HideSelection = True
		Me.txtTaskDueDate.Location = New System.Drawing.Point(480, 40)
		Me.txtTaskDueDate.MaxLength = 0
		Me.txtTaskDueDate.Multiline = False
		Me.txtTaskDueDate.Name = "txtTaskDueDate"
		Me.txtTaskDueDate.ReadOnly = False
		Me.txtTaskDueDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTaskDueDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTaskDueDate.Size = New System.Drawing.Size(73, 21)
		Me.txtTaskDueDate.TabIndex = 13
		Me.txtTaskDueDate.TabStop = True
		Me.txtTaskDueDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTaskDueDate.Visible = True
		' 
		' cboTask
		' 
		Me.cboTask.BackColor = System.Drawing.SystemColors.Window
		Me.cboTask.CausesValidation = True
		Me.cboTask.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboTask.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboTask.Enabled = False
		Me.cboTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboTask.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboTask.IntegralHeight = True
		Me.cboTask.Location = New System.Drawing.Point(408, 16)
		Me.cboTask.Name = "cboTask"
		Me.cboTask.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboTask.Size = New System.Drawing.Size(217, 21)
		Me.cboTask.Sorted = False
		Me.cboTask.TabIndex = 9
		Me.cboTask.TabStop = True
		Me.cboTask.Visible = True
		' 
		' cboActionType
		' 
		Me.cboActionType.BackColor = System.Drawing.SystemColors.Window
		Me.cboActionType.CausesValidation = True
		Me.cboActionType.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboActionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboActionType.Enabled = False
		Me.cboActionType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboActionType.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboActionType.IntegralHeight = True
		Me.cboActionType.Location = New System.Drawing.Point(96, 40)
		Me.cboActionType.Name = "cboActionType"
		Me.cboActionType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboActionType.Size = New System.Drawing.Size(249, 21)
		Me.cboActionType.Sorted = False
		Me.cboActionType.TabIndex = 11
		Me.cboActionType.TabStop = True
		Me.cboActionType.Visible = True
		' 
		' cboTaskGroup
		' 
		Me.cboTaskGroup.BackColor = System.Drawing.SystemColors.Window
		Me.cboTaskGroup.CausesValidation = True
		Me.cboTaskGroup.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboTaskGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboTaskGroup.Enabled = True
		Me.cboTaskGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboTaskGroup.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboTaskGroup.IntegralHeight = True
		Me.cboTaskGroup.Location = New System.Drawing.Point(96, 16)
		Me.cboTaskGroup.Name = "cboTaskGroup"
		Me.cboTaskGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboTaskGroup.Size = New System.Drawing.Size(201, 21)
		Me.cboTaskGroup.Sorted = False
		Me.cboTaskGroup.TabIndex = 7
		Me.cboTaskGroup.TabStop = True
		Me.cboTaskGroup.Visible = True
		' 
		' lblClient
		' 
		Me.lblClient.AutoSize = True
		Me.lblClient.BackColor = System.Drawing.SystemColors.Control
		Me.lblClient.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblClient.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblClient.Enabled = True
		Me.lblClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblClient.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblClient.Location = New System.Drawing.Point(49, 92)
		Me.lblClient.Name = "lblClient"
		Me.lblClient.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblClient.Size = New System.Drawing.Size(38, 13)
		Me.lblClient.TabIndex = 52
		Me.lblClient.Text = "Client:"
		Me.lblClient.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblClient.UseMnemonic = True
		Me.lblClient.Visible = True
		' 
		' lblWorkflowInfo
		' 
		Me.lblWorkflowInfo.AutoSize = True
		Me.lblWorkflowInfo.BackColor = System.Drawing.SystemColors.Control
		Me.lblWorkflowInfo.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblWorkflowInfo.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblWorkflowInfo.Enabled = True
		Me.lblWorkflowInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblWorkflowInfo.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblWorkflowInfo.Location = New System.Drawing.Point(29, 116)
		Me.lblWorkflowInfo.Name = "lblWorkflowInfo"
		Me.lblWorkflowInfo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblWorkflowInfo.Size = New System.Drawing.Size(58, 13)
		Me.lblWorkflowInfo.TabIndex = 51
		Me.lblWorkflowInfo.Text = "Workflow:"
		Me.lblWorkflowInfo.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblWorkflowInfo.UseMnemonic = True
		Me.lblWorkflowInfo.Visible = True
		' 
		' lblTaskDueDate
		' 
		Me.lblTaskDueDate.AutoSize = True
		Me.lblTaskDueDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblTaskDueDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTaskDueDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTaskDueDate.Enabled = True
		Me.lblTaskDueDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTaskDueDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTaskDueDate.Location = New System.Drawing.Point(368, 44)
		Me.lblTaskDueDate.Name = "lblTaskDueDate"
		Me.lblTaskDueDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTaskDueDate.Size = New System.Drawing.Size(100, 13)
		Me.lblTaskDueDate.TabIndex = 44
		Me.lblTaskDueDate.Text = "Due Date / Time:"
		Me.lblTaskDueDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTaskDueDate.UseMnemonic = True
		Me.lblTaskDueDate.Visible = True
		' 
		' lblTask
		' 
		Me.lblTask.AutoSize = True
		Me.lblTask.BackColor = System.Drawing.SystemColors.Control
		Me.lblTask.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTask.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTask.Enabled = True
		Me.lblTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTask.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTask.Location = New System.Drawing.Point(368, 20)
		Me.lblTask.Name = "lblTask"
		Me.lblTask.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTask.Size = New System.Drawing.Size(32, 13)
		Me.lblTask.TabIndex = 43
		Me.lblTask.Text = "Task:"
		Me.lblTask.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTask.UseMnemonic = True
		Me.lblTask.Visible = True
		' 
		' lblUrgent
		' 
		Me.lblUrgent.AutoSize = True
		Me.lblUrgent.BackColor = System.Drawing.SystemColors.Control
		Me.lblUrgent.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblUrgent.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblUrgent.Enabled = True
		Me.lblUrgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblUrgent.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblUrgent.Location = New System.Drawing.Point(44, 140)
		Me.lblUrgent.Name = "lblUrgent"
		Me.lblUrgent.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblUrgent.Size = New System.Drawing.Size(43, 13)
		Me.lblUrgent.TabIndex = 42
		Me.lblUrgent.Text = "Urgent:"
		Me.lblUrgent.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblUrgent.UseMnemonic = True
		Me.lblUrgent.Visible = True
		' 
		' lblTaskDescription
		' 
		Me.lblTaskDescription.AutoSize = True
		Me.lblTaskDescription.BackColor = System.Drawing.SystemColors.Control
		Me.lblTaskDescription.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTaskDescription.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTaskDescription.Enabled = True
		Me.lblTaskDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTaskDescription.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTaskDescription.Location = New System.Drawing.Point(18, 68)
		Me.lblTaskDescription.Name = "lblTaskDescription"
		Me.lblTaskDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTaskDescription.Size = New System.Drawing.Size(69, 13)
		Me.lblTaskDescription.TabIndex = 41
		Me.lblTaskDescription.Text = "Description:"
		Me.lblTaskDescription.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTaskDescription.UseMnemonic = True
		Me.lblTaskDescription.Visible = True
		' 
		' lblActionType
		' 
		Me.lblActionType.AutoSize = True
		Me.lblActionType.BackColor = System.Drawing.SystemColors.Control
		Me.lblActionType.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblActionType.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblActionType.Enabled = True
		Me.lblActionType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblActionType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblActionType.Location = New System.Drawing.Point(19, 44)
		Me.lblActionType.Name = "lblActionType"
		Me.lblActionType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblActionType.Size = New System.Drawing.Size(68, 13)
		Me.lblActionType.TabIndex = 40
		Me.lblActionType.Text = "ActionType:"
		Me.lblActionType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblActionType.UseMnemonic = True
		Me.lblActionType.Visible = True
		' 
		' lblTaskGroup
		' 
		Me.lblTaskGroup.AutoSize = True
		Me.lblTaskGroup.BackColor = System.Drawing.SystemColors.Control
		Me.lblTaskGroup.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTaskGroup.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTaskGroup.Enabled = True
		Me.lblTaskGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTaskGroup.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTaskGroup.Location = New System.Drawing.Point(16, 20)
		Me.lblTaskGroup.Name = "lblTaskGroup"
		Me.lblTaskGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTaskGroup.Size = New System.Drawing.Size(71, 13)
		Me.lblTaskGroup.TabIndex = 39
		Me.lblTaskGroup.Text = "Task Group:"
		Me.lblTaskGroup.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTaskGroup.UseMnemonic = True
		Me.lblTaskGroup.Visible = True
		' 
		' frmInterface
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(655, 439)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdTaskLog)
		Me.Controls.Add(Me.uctPMResizer)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.tabMainTab)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.dlgMainOpen.DefaultExt = "csv"
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = True
		Me.Icon = CType(resources.GetObject("frmInterface.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(203, 163)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Workflow Task"
		Me.dlgMainOpen.Title = "Save As..."
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMainTab, 1)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tabMainTab.ResumeLayout(False)
		Me._tabMainTab_TabPage0.ResumeLayout(False)
		Me.fraEvent.ResumeLayout(False)
		Me.fraTask.ResumeLayout(False)
		Me.fraComplete.ResumeLayout(False)
		Me.fraAllocate.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class