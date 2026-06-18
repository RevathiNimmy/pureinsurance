<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDetails
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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cboOverdue As System.Windows.Forms.ComboBox
	Public WithEvents lblOverDueStep As System.Windows.Forms.Label
	Public WithEvents frmOverdue As System.Windows.Forms.GroupBox
	Public WithEvents cboInTime As System.Windows.Forms.ComboBox
	Public WithEvents lblInTimeStep As System.Windows.Forms.Label
	Public WithEvents frmInTime As System.Windows.Forms.GroupBox
	Public WithEvents txtEventSubject As System.Windows.Forms.TextBox
	Public WithEvents txtEventType As System.Windows.Forms.TextBox
	Public WithEvents txtEventDescription As System.Windows.Forms.TextBox
	Public WithEvents cboEventSubject As System.Windows.Forms.ComboBox
	Public WithEvents cboEventType As System.Windows.Forms.ComboBox
	Public WithEvents lblEventDescription As System.Windows.Forms.Label
	Public WithEvents lblEventSubject As System.Windows.Forms.Label
	Public WithEvents lblEventType As System.Windows.Forms.Label
	Public WithEvents fraEvent As System.Windows.Forms.GroupBox
	Public WithEvents chkTaskUrgent As System.Windows.Forms.CheckBox
	Public WithEvents txtClient As System.Windows.Forms.TextBox
	Public WithEvents txtWorkflow As System.Windows.Forms.TextBox
	Public WithEvents txtTaskDescription As System.Windows.Forms.TextBox
	Public WithEvents chkExecutable As System.Windows.Forms.CheckBox
	Public WithEvents txtActionType As System.Windows.Forms.TextBox
	Public WithEvents txtTask As System.Windows.Forms.TextBox
	Public WithEvents txtTaskGroup As System.Windows.Forms.TextBox
	Public WithEvents txtBranch As System.Windows.Forms.TextBox
	Public WithEvents cboBranch As System.Windows.Forms.ComboBox
	Public WithEvents txtAllocateToUser As System.Windows.Forms.TextBox
	Public WithEvents txtAllocateToUserGroup As System.Windows.Forms.TextBox
	Public WithEvents cboUserGroup As System.Windows.Forms.ComboBox
	Public WithEvents cboUser As System.Windows.Forms.ComboBox
	Public WithEvents lblBranch As System.Windows.Forms.Label
	Public WithEvents lblAllocateToUserGroup As System.Windows.Forms.Label
	Public WithEvents lblAllocateToUser As System.Windows.Forms.Label
	Public WithEvents fraAllocate As System.Windows.Forms.GroupBox
	Public WithEvents txtStepDaysDuration As System.Windows.Forms.TextBox
	Public WithEvents cboTask As System.Windows.Forms.ComboBox
	Public WithEvents cboActionType As System.Windows.Forms.ComboBox
	Public WithEvents cboTaskGroup As System.Windows.Forms.ComboBox
	Public WithEvents lblUrgent As System.Windows.Forms.Label
	Public WithEvents lblWorkflowInfo As System.Windows.Forms.Label
	Public WithEvents lblClient As System.Windows.Forms.Label
	Public WithEvents lblTaskDescription As System.Windows.Forms.Label
	Public WithEvents lblExecuteTask As System.Windows.Forms.Label
	Public WithEvents lblStepDaysDuration As System.Windows.Forms.Label
	Public WithEvents lblTask As System.Windows.Forms.Label
	Public WithEvents lblActionType As System.Windows.Forms.Label
	Public WithEvents lblTaskGroup As System.Windows.Forms.Label
	Public WithEvents fraTask As System.Windows.Forms.GroupBox
	Public WithEvents txtCode As System.Windows.Forms.TextBox
	Public WithEvents txtEffectiveDate As System.Windows.Forms.TextBox
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents lblCode As System.Windows.Forms.Label
	Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents fraWorkflowStep As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDetails))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.tabMainTab = New System.Windows.Forms.TabControl
		Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
		Me.fraWorkflowStep = New System.Windows.Forms.GroupBox
		Me.frmOverdue = New System.Windows.Forms.GroupBox
		Me.cboOverdue = New System.Windows.Forms.ComboBox
		Me.lblOverDueStep = New System.Windows.Forms.Label
		Me.frmInTime = New System.Windows.Forms.GroupBox
		Me.cboInTime = New System.Windows.Forms.ComboBox
		Me.lblInTimeStep = New System.Windows.Forms.Label
		Me.fraEvent = New System.Windows.Forms.GroupBox
		Me.txtEventSubject = New System.Windows.Forms.TextBox
		Me.txtEventType = New System.Windows.Forms.TextBox
		Me.txtEventDescription = New System.Windows.Forms.TextBox
		Me.cboEventSubject = New System.Windows.Forms.ComboBox
		Me.cboEventType = New System.Windows.Forms.ComboBox
		Me.lblEventDescription = New System.Windows.Forms.Label
		Me.lblEventSubject = New System.Windows.Forms.Label
		Me.lblEventType = New System.Windows.Forms.Label
		Me.fraTask = New System.Windows.Forms.GroupBox
		Me.chkTaskUrgent = New System.Windows.Forms.CheckBox
		Me.txtClient = New System.Windows.Forms.TextBox
		Me.txtWorkflow = New System.Windows.Forms.TextBox
		Me.txtTaskDescription = New System.Windows.Forms.TextBox
		Me.chkExecutable = New System.Windows.Forms.CheckBox
		Me.txtActionType = New System.Windows.Forms.TextBox
		Me.txtTask = New System.Windows.Forms.TextBox
		Me.txtTaskGroup = New System.Windows.Forms.TextBox
		Me.fraAllocate = New System.Windows.Forms.GroupBox
		Me.txtBranch = New System.Windows.Forms.TextBox
		Me.cboBranch = New System.Windows.Forms.ComboBox
		Me.txtAllocateToUser = New System.Windows.Forms.TextBox
		Me.txtAllocateToUserGroup = New System.Windows.Forms.TextBox
		Me.cboUserGroup = New System.Windows.Forms.ComboBox
		Me.cboUser = New System.Windows.Forms.ComboBox
		Me.lblBranch = New System.Windows.Forms.Label
		Me.lblAllocateToUserGroup = New System.Windows.Forms.Label
		Me.lblAllocateToUser = New System.Windows.Forms.Label
		Me.txtStepDaysDuration = New System.Windows.Forms.TextBox
		Me.cboTask = New System.Windows.Forms.ComboBox
		Me.cboActionType = New System.Windows.Forms.ComboBox
		Me.cboTaskGroup = New System.Windows.Forms.ComboBox
		Me.lblUrgent = New System.Windows.Forms.Label
		Me.lblWorkflowInfo = New System.Windows.Forms.Label
		Me.lblClient = New System.Windows.Forms.Label
		Me.lblTaskDescription = New System.Windows.Forms.Label
		Me.lblExecuteTask = New System.Windows.Forms.Label
		Me.lblStepDaysDuration = New System.Windows.Forms.Label
		Me.lblTask = New System.Windows.Forms.Label
		Me.lblActionType = New System.Windows.Forms.Label
		Me.lblTaskGroup = New System.Windows.Forms.Label
		Me.txtCode = New System.Windows.Forms.TextBox
		Me.txtEffectiveDate = New System.Windows.Forms.TextBox
		Me.txtDescription = New System.Windows.Forms.TextBox
		Me.lblCode = New System.Windows.Forms.Label
		Me.lblEffectiveDate = New System.Windows.Forms.Label
		Me.lblDescription = New System.Windows.Forms.Label
		Me.cmdOK = New System.Windows.Forms.Button
		Me.tabMainTab.SuspendLayout()
		Me._tabMainTab_TabPage0.SuspendLayout()
		Me.fraWorkflowStep.SuspendLayout()
		Me.frmOverdue.SuspendLayout()
		Me.frmInTime.SuspendLayout()
		Me.fraEvent.SuspendLayout()
		Me.fraTask.SuspendLayout()
		Me.fraAllocate.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(584, 480)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 29
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tabMainTab
		' 
		Me.tabMainTab.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabMainTab.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
		Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMainTab.ItemSize = New System.Drawing.Size(736, 18)
		Me.tabMainTab.Location = New System.Drawing.Point(0, 0)
		Me.tabMainTab.Multiline = True
		Me.tabMainTab.Name = "tabMainTab"
		Me.tabMainTab.Size = New System.Drawing.Size(741, 475)
		Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMainTab.TabIndex = 30
		Me.tabMainTab.TabStop = False
		' 
		' _tabMainTab_TabPage0
		' 
		Me._tabMainTab_TabPage0.Controls.Add(Me.fraWorkflowStep)
		Me._tabMainTab_TabPage0.Text = "&Details"
		' 
		' fraWorkflowStep
		' 
		Me.fraWorkflowStep.BackColor = System.Drawing.SystemColors.Control
		Me.fraWorkflowStep.Controls.Add(Me.frmOverdue)
		Me.fraWorkflowStep.Controls.Add(Me.frmInTime)
		Me.fraWorkflowStep.Controls.Add(Me.fraEvent)
		Me.fraWorkflowStep.Controls.Add(Me.fraTask)
		Me.fraWorkflowStep.Controls.Add(Me.txtCode)
		Me.fraWorkflowStep.Controls.Add(Me.txtEffectiveDate)
		Me.fraWorkflowStep.Controls.Add(Me.txtDescription)
		Me.fraWorkflowStep.Controls.Add(Me.lblCode)
		Me.fraWorkflowStep.Controls.Add(Me.lblEffectiveDate)
		Me.fraWorkflowStep.Controls.Add(Me.lblDescription)
		Me.fraWorkflowStep.Enabled = True
		Me.fraWorkflowStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraWorkflowStep.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraWorkflowStep.Location = New System.Drawing.Point(8, 4)
		Me.fraWorkflowStep.Name = "fraWorkflowStep"
		Me.fraWorkflowStep.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraWorkflowStep.Size = New System.Drawing.Size(721, 441)
		Me.fraWorkflowStep.TabIndex = 31
		Me.fraWorkflowStep.Text = "Step"
		Me.fraWorkflowStep.Visible = True
		' 
		' frmOverdue
		' 
		Me.frmOverdue.BackColor = System.Drawing.SystemColors.Control
		Me.frmOverdue.Controls.Add(Me.cboOverdue)
		Me.frmOverdue.Controls.Add(Me.lblOverDueStep)
		Me.frmOverdue.Enabled = True
		Me.frmOverdue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.frmOverdue.ForeColor = System.Drawing.SystemColors.ControlText
		Me.frmOverdue.Location = New System.Drawing.Point(368, 380)
		Me.frmOverdue.Name = "frmOverdue"
		Me.frmOverdue.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.frmOverdue.Size = New System.Drawing.Size(347, 57)
		Me.frmOverdue.TabIndex = 49
		Me.frmOverdue.Text = "Next Step When Completed Overdue"
		Me.frmOverdue.Visible = True
		' 
		' cboOverdue
		' 
		Me.cboOverdue.BackColor = System.Drawing.SystemColors.Window
		Me.cboOverdue.CausesValidation = True
		Me.cboOverdue.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboOverdue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboOverdue.Enabled = True
		Me.cboOverdue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboOverdue.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboOverdue.IntegralHeight = True
		Me.cboOverdue.Location = New System.Drawing.Point(104, 21)
		Me.cboOverdue.Name = "cboOverdue"
		Me.cboOverdue.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboOverdue.Size = New System.Drawing.Size(174, 21)
		Me.cboOverdue.Sorted = False
		Me.cboOverdue.TabIndex = 27
		Me.cboOverdue.TabStop = True
		Me.cboOverdue.Visible = True
		' 
		' lblOverDueStep
		' 
		Me.lblOverDueStep.AutoSize = True
		Me.lblOverDueStep.BackColor = System.Drawing.SystemColors.Control
		Me.lblOverDueStep.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblOverDueStep.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblOverDueStep.Enabled = True
		Me.lblOverDueStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblOverDueStep.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblOverDueStep.Location = New System.Drawing.Point(64, 25)
		Me.lblOverDueStep.Name = "lblOverDueStep"
		Me.lblOverDueStep.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblOverDueStep.Size = New System.Drawing.Size(31, 13)
		Me.lblOverDueStep.TabIndex = 50
		Me.lblOverDueStep.Text = "S&tep:"
		Me.lblOverDueStep.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblOverDueStep.UseMnemonic = True
		Me.lblOverDueStep.Visible = True
		' 
		' frmInTime
		' 
		Me.frmInTime.BackColor = System.Drawing.SystemColors.Control
		Me.frmInTime.Controls.Add(Me.cboInTime)
		Me.frmInTime.Controls.Add(Me.lblInTimeStep)
		Me.frmInTime.Enabled = True
		Me.frmInTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.frmInTime.ForeColor = System.Drawing.SystemColors.ControlText
		Me.frmInTime.Location = New System.Drawing.Point(3, 380)
		Me.frmInTime.Name = "frmInTime"
		Me.frmInTime.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.frmInTime.Size = New System.Drawing.Size(347, 57)
		Me.frmInTime.TabIndex = 34
		Me.frmInTime.Text = "Next Step When Completed in Time"
		Me.frmInTime.Visible = True
		' 
		' cboInTime
		' 
		Me.cboInTime.BackColor = System.Drawing.SystemColors.Window
		Me.cboInTime.CausesValidation = True
		Me.cboInTime.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboInTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboInTime.Enabled = True
		Me.cboInTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboInTime.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboInTime.IntegralHeight = True
		Me.cboInTime.Location = New System.Drawing.Point(104, 21)
		Me.cboInTime.Name = "cboInTime"
		Me.cboInTime.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboInTime.Size = New System.Drawing.Size(174, 21)
		Me.cboInTime.Sorted = False
		Me.cboInTime.TabIndex = 26
		Me.cboInTime.TabStop = True
		Me.cboInTime.Visible = True
		' 
		' lblInTimeStep
		' 
		Me.lblInTimeStep.AutoSize = True
		Me.lblInTimeStep.BackColor = System.Drawing.SystemColors.Control
		Me.lblInTimeStep.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblInTimeStep.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblInTimeStep.Enabled = True
		Me.lblInTimeStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblInTimeStep.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblInTimeStep.Location = New System.Drawing.Point(64, 25)
		Me.lblInTimeStep.Name = "lblInTimeStep"
		Me.lblInTimeStep.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblInTimeStep.Size = New System.Drawing.Size(31, 13)
		Me.lblInTimeStep.TabIndex = 48
		Me.lblInTimeStep.Text = "&Step:"
		Me.lblInTimeStep.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblInTimeStep.UseMnemonic = True
		Me.lblInTimeStep.Visible = True
		' 
		' fraEvent
		' 
		Me.fraEvent.BackColor = System.Drawing.SystemColors.Control
		Me.fraEvent.Controls.Add(Me.txtEventSubject)
		Me.fraEvent.Controls.Add(Me.txtEventType)
		Me.fraEvent.Controls.Add(Me.txtEventDescription)
		Me.fraEvent.Controls.Add(Me.cboEventSubject)
		Me.fraEvent.Controls.Add(Me.cboEventType)
		Me.fraEvent.Controls.Add(Me.lblEventDescription)
		Me.fraEvent.Controls.Add(Me.lblEventSubject)
		Me.fraEvent.Controls.Add(Me.lblEventType)
		Me.fraEvent.Enabled = True
		Me.fraEvent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraEvent.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraEvent.Location = New System.Drawing.Point(3, 280)
		Me.fraEvent.Name = "fraEvent"
		Me.fraEvent.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraEvent.Size = New System.Drawing.Size(713, 97)
		Me.fraEvent.TabIndex = 44
		Me.fraEvent.Text = "Associated Event"
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
		Me.txtEventSubject.Location = New System.Drawing.Point(280, 41)
		Me.txtEventSubject.MaxLength = 0
		Me.txtEventSubject.Multiline = False
		Me.txtEventSubject.Name = "txtEventSubject"
		Me.txtEventSubject.ReadOnly = False
		Me.txtEventSubject.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtEventSubject.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtEventSubject.Size = New System.Drawing.Size(33, 19)
		Me.txtEventSubject.TabIndex = 24
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
		Me.txtEventType.Location = New System.Drawing.Point(280, 17)
		Me.txtEventType.MaxLength = 0
		Me.txtEventType.Multiline = False
		Me.txtEventType.Name = "txtEventType"
		Me.txtEventType.ReadOnly = False
		Me.txtEventType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtEventType.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtEventType.Size = New System.Drawing.Size(33, 19)
		Me.txtEventType.TabIndex = 22
		Me.txtEventType.TabStop = True
		Me.txtEventType.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtEventType.Visible = False
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
		Me.txtEventDescription.Location = New System.Drawing.Point(102, 64)
		Me.txtEventDescription.MaxLength = 0
		Me.txtEventDescription.Multiline = False
		Me.txtEventDescription.Name = "txtEventDescription"
		Me.txtEventDescription.ReadOnly = False
		Me.txtEventDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtEventDescription.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtEventDescription.Size = New System.Drawing.Size(529, 21)
		Me.txtEventDescription.TabIndex = 25
		Me.txtEventDescription.TabStop = True
		Me.txtEventDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtEventDescription.Visible = True
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
		Me.cboEventSubject.Location = New System.Drawing.Point(102, 40)
		Me.cboEventSubject.Name = "cboEventSubject"
		Me.cboEventSubject.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboEventSubject.Size = New System.Drawing.Size(249, 21)
		Me.cboEventSubject.Sorted = False
		Me.cboEventSubject.TabIndex = 23
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
		Me.cboEventType.Location = New System.Drawing.Point(102, 16)
		Me.cboEventType.Name = "cboEventType"
		Me.cboEventType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboEventType.Size = New System.Drawing.Size(249, 21)
		Me.cboEventType.Sorted = False
		Me.cboEventType.TabIndex = 21
		Me.cboEventType.TabStop = True
		Me.cboEventType.Visible = True
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
		Me.lblEventDescription.Location = New System.Drawing.Point(24, 68)
		Me.lblEventDescription.Name = "lblEventDescription"
		Me.lblEventDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblEventDescription.Size = New System.Drawing.Size(69, 13)
		Me.lblEventDescription.TabIndex = 47
		Me.lblEventDescription.Text = "Description:"
		Me.lblEventDescription.TextAlign = System.Drawing.ContentAlignment.TopRight
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
		Me.lblEventSubject.Location = New System.Drawing.Point(45, 44)
		Me.lblEventSubject.Name = "lblEventSubject"
		Me.lblEventSubject.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblEventSubject.Size = New System.Drawing.Size(48, 13)
		Me.lblEventSubject.TabIndex = 46
		Me.lblEventSubject.Text = "Subject:"
		Me.lblEventSubject.TextAlign = System.Drawing.ContentAlignment.TopRight
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
		Me.lblEventType.Location = New System.Drawing.Point(60, 20)
		Me.lblEventType.Name = "lblEventType"
		Me.lblEventType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblEventType.Size = New System.Drawing.Size(33, 13)
		Me.lblEventType.TabIndex = 45
		Me.lblEventType.Text = "Type:"
		Me.lblEventType.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblEventType.UseMnemonic = True
		Me.lblEventType.Visible = True
		' 
		' fraTask
		' 
		Me.fraTask.BackColor = System.Drawing.SystemColors.Control
		Me.fraTask.Controls.Add(Me.chkTaskUrgent)
		Me.fraTask.Controls.Add(Me.txtClient)
		Me.fraTask.Controls.Add(Me.txtWorkflow)
		Me.fraTask.Controls.Add(Me.txtTaskDescription)
		Me.fraTask.Controls.Add(Me.chkExecutable)
		Me.fraTask.Controls.Add(Me.txtActionType)
		Me.fraTask.Controls.Add(Me.txtTask)
		Me.fraTask.Controls.Add(Me.txtTaskGroup)
		Me.fraTask.Controls.Add(Me.fraAllocate)
		Me.fraTask.Controls.Add(Me.txtStepDaysDuration)
		Me.fraTask.Controls.Add(Me.cboTask)
		Me.fraTask.Controls.Add(Me.cboActionType)
		Me.fraTask.Controls.Add(Me.cboTaskGroup)
		Me.fraTask.Controls.Add(Me.lblUrgent)
		Me.fraTask.Controls.Add(Me.lblWorkflowInfo)
		Me.fraTask.Controls.Add(Me.lblClient)
		Me.fraTask.Controls.Add(Me.lblTaskDescription)
		Me.fraTask.Controls.Add(Me.lblExecuteTask)
		Me.fraTask.Controls.Add(Me.lblStepDaysDuration)
		Me.fraTask.Controls.Add(Me.lblTask)
		Me.fraTask.Controls.Add(Me.lblActionType)
		Me.fraTask.Controls.Add(Me.lblTaskGroup)
		Me.fraTask.Enabled = True
		Me.fraTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraTask.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraTask.Location = New System.Drawing.Point(3, 88)
		Me.fraTask.Name = "fraTask"
		Me.fraTask.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraTask.Size = New System.Drawing.Size(713, 191)
		Me.fraTask.TabIndex = 36
		Me.fraTask.Text = "Task"
		Me.fraTask.Visible = True
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
		Me.chkTaskUrgent.Location = New System.Drawing.Point(493, 120)
		Me.chkTaskUrgent.Name = "chkTaskUrgent"
		Me.chkTaskUrgent.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkTaskUrgent.Size = New System.Drawing.Size(21, 21)
		Me.chkTaskUrgent.TabIndex = 14
		Me.chkTaskUrgent.TabStop = True
		Me.chkTaskUrgent.Text = ""
		Me.chkTaskUrgent.Visible = True
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
		Me.txtClient.Location = New System.Drawing.Point(102, 94)
		Me.txtClient.MaxLength = 0
		Me.txtClient.Multiline = False
		Me.txtClient.Name = "txtClient"
		Me.txtClient.ReadOnly = False
		Me.txtClient.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtClient.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtClient.Size = New System.Drawing.Size(249, 21)
		Me.txtClient.TabIndex = 11
		Me.txtClient.TabStop = True
		Me.txtClient.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtClient.Visible = True
		' 
		' txtWorkflow
		' 
		Me.txtWorkflow.AcceptsReturn = True
		Me.txtWorkflow.AutoSize = False
		Me.txtWorkflow.BackColor = System.Drawing.SystemColors.Window
		Me.txtWorkflow.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtWorkflow.CausesValidation = True
		Me.txtWorkflow.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtWorkflow.Enabled = True
		Me.txtWorkflow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtWorkflow.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtWorkflow.HideSelection = True
		Me.txtWorkflow.Location = New System.Drawing.Point(102, 120)
		Me.txtWorkflow.MaxLength = 0
		Me.txtWorkflow.Multiline = False
		Me.txtWorkflow.Name = "txtWorkflow"
		Me.txtWorkflow.ReadOnly = False
		Me.txtWorkflow.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtWorkflow.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtWorkflow.Size = New System.Drawing.Size(249, 21)
		Me.txtWorkflow.TabIndex = 13
		Me.txtWorkflow.TabStop = True
		Me.txtWorkflow.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtWorkflow.Visible = True
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
		Me.txtTaskDescription.Location = New System.Drawing.Point(102, 68)
		Me.txtTaskDescription.MaxLength = 0
		Me.txtTaskDescription.Multiline = False
		Me.txtTaskDescription.Name = "txtTaskDescription"
		Me.txtTaskDescription.ReadOnly = False
		Me.txtTaskDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTaskDescription.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTaskDescription.Size = New System.Drawing.Size(521, 21)
		Me.txtTaskDescription.TabIndex = 10
		Me.txtTaskDescription.TabStop = True
		Me.txtTaskDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTaskDescription.Visible = True
		' 
		' chkExecutable
		' 
		Me.chkExecutable.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkExecutable.BackColor = System.Drawing.SystemColors.Control
		Me.chkExecutable.CausesValidation = True
		Me.chkExecutable.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkExecutable.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkExecutable.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkExecutable.Enabled = True
		Me.chkExecutable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkExecutable.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkExecutable.Location = New System.Drawing.Point(493, 98)
		Me.chkExecutable.Name = "chkExecutable"
		Me.chkExecutable.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkExecutable.Size = New System.Drawing.Size(17, 13)
		Me.chkExecutable.TabIndex = 12
		Me.chkExecutable.TabStop = True
		Me.chkExecutable.Text = "Check1"
		Me.chkExecutable.Visible = True
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
		Me.txtActionType.Location = New System.Drawing.Point(232, 43)
		Me.txtActionType.MaxLength = 0
		Me.txtActionType.Multiline = False
		Me.txtActionType.Name = "txtActionType"
		Me.txtActionType.ReadOnly = False
		Me.txtActionType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtActionType.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtActionType.Size = New System.Drawing.Size(33, 19)
		Me.txtActionType.TabIndex = 8
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
		Me.txtTask.Location = New System.Drawing.Point(560, 17)
		Me.txtTask.MaxLength = 0
		Me.txtTask.Multiline = False
		Me.txtTask.Name = "txtTask"
		Me.txtTask.ReadOnly = False
		Me.txtTask.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTask.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTask.Size = New System.Drawing.Size(33, 19)
		Me.txtTask.TabIndex = 6
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
		Me.txtTaskGroup.Location = New System.Drawing.Point(240, 17)
		Me.txtTaskGroup.MaxLength = 0
		Me.txtTaskGroup.Multiline = False
		Me.txtTaskGroup.Name = "txtTaskGroup"
		Me.txtTaskGroup.ReadOnly = False
		Me.txtTaskGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTaskGroup.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTaskGroup.Size = New System.Drawing.Size(33, 19)
		Me.txtTaskGroup.TabIndex = 4
		Me.txtTaskGroup.TabStop = True
		Me.txtTaskGroup.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTaskGroup.Visible = False
		' 
		' fraAllocate
		' 
		Me.fraAllocate.BackColor = System.Drawing.SystemColors.Control
		Me.fraAllocate.Controls.Add(Me.txtBranch)
		Me.fraAllocate.Controls.Add(Me.cboBranch)
		Me.fraAllocate.Controls.Add(Me.txtAllocateToUser)
		Me.fraAllocate.Controls.Add(Me.txtAllocateToUserGroup)
		Me.fraAllocate.Controls.Add(Me.cboUserGroup)
		Me.fraAllocate.Controls.Add(Me.cboUser)
		Me.fraAllocate.Controls.Add(Me.lblBranch)
		Me.fraAllocate.Controls.Add(Me.lblAllocateToUserGroup)
		Me.fraAllocate.Controls.Add(Me.lblAllocateToUser)
		Me.fraAllocate.Enabled = True
		Me.fraAllocate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraAllocate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraAllocate.Location = New System.Drawing.Point(3, 139)
		Me.fraAllocate.Name = "fraAllocate"
		Me.fraAllocate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraAllocate.Size = New System.Drawing.Size(705, 49)
		Me.fraAllocate.TabIndex = 37
		Me.fraAllocate.Text = "Allocate To"
		Me.fraAllocate.Visible = True
		' 
		' txtBranch
		' 
		Me.txtBranch.AcceptsReturn = True
		Me.txtBranch.AutoSize = False
		Me.txtBranch.BackColor = System.Drawing.SystemColors.Window
		Me.txtBranch.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtBranch.CausesValidation = True
		Me.txtBranch.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtBranch.Enabled = True
		Me.txtBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtBranch.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtBranch.HideSelection = True
		Me.txtBranch.Location = New System.Drawing.Point(576, 17)
		Me.txtBranch.MaxLength = 0
		Me.txtBranch.Multiline = False
		Me.txtBranch.Name = "txtBranch"
		Me.txtBranch.ReadOnly = False
		Me.txtBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtBranch.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtBranch.Size = New System.Drawing.Size(41, 19)
		Me.txtBranch.TabIndex = 20
		Me.txtBranch.TabStop = True
		Me.txtBranch.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtBranch.Visible = False
		' 
		' cboBranch
		' 
		Me.cboBranch.BackColor = System.Drawing.SystemColors.Window
		Me.cboBranch.CausesValidation = True
		Me.cboBranch.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboBranch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboBranch.Enabled = True
		Me.cboBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboBranch.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboBranch.IntegralHeight = True
		Me.cboBranch.Location = New System.Drawing.Point(520, 16)
		Me.cboBranch.Name = "cboBranch"
		Me.cboBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboBranch.Size = New System.Drawing.Size(177, 21)
		Me.cboBranch.Sorted = False
		Me.cboBranch.TabIndex = 19
		Me.cboBranch.TabStop = True
		Me.cboBranch.Visible = True
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
		Me.txtAllocateToUser.Location = New System.Drawing.Point(384, 17)
		Me.txtAllocateToUser.MaxLength = 0
		Me.txtAllocateToUser.Multiline = False
		Me.txtAllocateToUser.Name = "txtAllocateToUser"
		Me.txtAllocateToUser.ReadOnly = False
		Me.txtAllocateToUser.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAllocateToUser.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAllocateToUser.Size = New System.Drawing.Size(41, 19)
		Me.txtAllocateToUser.TabIndex = 18
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
		Me.txtAllocateToUserGroup.Location = New System.Drawing.Point(192, 17)
		Me.txtAllocateToUserGroup.MaxLength = 0
		Me.txtAllocateToUserGroup.Multiline = False
		Me.txtAllocateToUserGroup.Name = "txtAllocateToUserGroup"
		Me.txtAllocateToUserGroup.ReadOnly = False
		Me.txtAllocateToUserGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAllocateToUserGroup.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAllocateToUserGroup.Size = New System.Drawing.Size(33, 19)
		Me.txtAllocateToUserGroup.TabIndex = 16
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
		Me.cboUserGroup.Location = New System.Drawing.Point(80, 16)
		Me.cboUserGroup.Name = "cboUserGroup"
		Me.cboUserGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboUserGroup.Size = New System.Drawing.Size(177, 21)
		Me.cboUserGroup.Sorted = False
		Me.cboUserGroup.TabIndex = 15
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
		Me.cboUser.Location = New System.Drawing.Point(304, 16)
		Me.cboUser.Name = "cboUser"
		Me.cboUser.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboUser.Size = New System.Drawing.Size(161, 21)
		Me.cboUser.Sorted = False
		Me.cboUser.TabIndex = 17
		Me.cboUser.TabStop = True
		Me.cboUser.Visible = True
		' 
		' lblBranch
		' 
		Me.lblBranch.AutoSize = True
		Me.lblBranch.BackColor = System.Drawing.SystemColors.Control
		Me.lblBranch.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblBranch.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblBranch.Enabled = True
		Me.lblBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblBranch.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblBranch.Location = New System.Drawing.Point(472, 20)
		Me.lblBranch.Name = "lblBranch"
		Me.lblBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblBranch.Size = New System.Drawing.Size(45, 13)
		Me.lblBranch.TabIndex = 56
		Me.lblBranch.Text = "Branch:"
		Me.lblBranch.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblBranch.UseMnemonic = True
		Me.lblBranch.Visible = True
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
		Me.lblAllocateToUserGroup.Location = New System.Drawing.Point(8, 20)
		Me.lblAllocateToUserGroup.Name = "lblAllocateToUserGroup"
		Me.lblAllocateToUserGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAllocateToUserGroup.Size = New System.Drawing.Size(70, 13)
		Me.lblAllocateToUserGroup.TabIndex = 39
		Me.lblAllocateToUserGroup.Text = "User Group:"
		Me.lblAllocateToUserGroup.TextAlign = System.Drawing.ContentAlignment.TopRight
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
		Me.lblAllocateToUser.Location = New System.Drawing.Point(272, 20)
		Me.lblAllocateToUser.Name = "lblAllocateToUser"
		Me.lblAllocateToUser.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAllocateToUser.Size = New System.Drawing.Size(31, 13)
		Me.lblAllocateToUser.TabIndex = 38
		Me.lblAllocateToUser.Text = "User:"
		Me.lblAllocateToUser.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblAllocateToUser.UseMnemonic = True
		Me.lblAllocateToUser.Visible = True
		' 
		' txtStepDaysDuration
		' 
		Me.txtStepDaysDuration.AcceptsReturn = True
		Me.txtStepDaysDuration.AutoSize = False
		Me.txtStepDaysDuration.BackColor = System.Drawing.SystemColors.Window
		Me.txtStepDaysDuration.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtStepDaysDuration.CausesValidation = True
		Me.txtStepDaysDuration.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtStepDaysDuration.Enabled = True
		Me.txtStepDaysDuration.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtStepDaysDuration.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtStepDaysDuration.HideSelection = True
		Me.txtStepDaysDuration.Location = New System.Drawing.Point(464, 44)
		Me.txtStepDaysDuration.MaxLength = 0
		Me.txtStepDaysDuration.Multiline = False
		Me.txtStepDaysDuration.Name = "txtStepDaysDuration"
		Me.txtStepDaysDuration.ReadOnly = False
		Me.txtStepDaysDuration.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtStepDaysDuration.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtStepDaysDuration.Size = New System.Drawing.Size(73, 21)
		Me.txtStepDaysDuration.TabIndex = 9
		Me.txtStepDaysDuration.TabStop = True
		Me.txtStepDaysDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtStepDaysDuration.Visible = True
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
		Me.cboTask.TabIndex = 5
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
		Me.cboActionType.Location = New System.Drawing.Point(102, 42)
		Me.cboActionType.Name = "cboActionType"
		Me.cboActionType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboActionType.Size = New System.Drawing.Size(249, 21)
		Me.cboActionType.Sorted = False
		Me.cboActionType.TabIndex = 7
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
		Me.cboTaskGroup.Location = New System.Drawing.Point(102, 16)
		Me.cboTaskGroup.Name = "cboTaskGroup"
		Me.cboTaskGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboTaskGroup.Size = New System.Drawing.Size(201, 21)
		Me.cboTaskGroup.Sorted = False
		Me.cboTaskGroup.TabIndex = 3
		Me.cboTaskGroup.TabStop = True
		Me.cboTaskGroup.Visible = True
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
		Me.lblUrgent.Location = New System.Drawing.Point(444, 124)
		Me.lblUrgent.Name = "lblUrgent"
		Me.lblUrgent.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblUrgent.Size = New System.Drawing.Size(43, 13)
		Me.lblUrgent.TabIndex = 55
		Me.lblUrgent.Text = "Urgent:"
		Me.lblUrgent.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblUrgent.UseMnemonic = True
		Me.lblUrgent.Visible = True
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
		Me.lblWorkflowInfo.Location = New System.Drawing.Point(37, 124)
		Me.lblWorkflowInfo.Name = "lblWorkflowInfo"
		Me.lblWorkflowInfo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblWorkflowInfo.Size = New System.Drawing.Size(58, 13)
		Me.lblWorkflowInfo.TabIndex = 54
		Me.lblWorkflowInfo.Text = "Workflow:"
		Me.lblWorkflowInfo.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblWorkflowInfo.UseMnemonic = True
		Me.lblWorkflowInfo.Visible = True
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
		Me.lblClient.Location = New System.Drawing.Point(57, 98)
		Me.lblClient.Name = "lblClient"
		Me.lblClient.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblClient.Size = New System.Drawing.Size(38, 13)
		Me.lblClient.TabIndex = 53
		Me.lblClient.Text = "Client:"
		Me.lblClient.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblClient.UseMnemonic = True
		Me.lblClient.Visible = True
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
		Me.lblTaskDescription.Location = New System.Drawing.Point(26, 72)
		Me.lblTaskDescription.Name = "lblTaskDescription"
		Me.lblTaskDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTaskDescription.Size = New System.Drawing.Size(69, 13)
		Me.lblTaskDescription.TabIndex = 52
		Me.lblTaskDescription.Text = "Description:"
		Me.lblTaskDescription.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTaskDescription.UseMnemonic = True
		Me.lblTaskDescription.Visible = True
		' 
		' lblExecuteTask
		' 
		Me.lblExecuteTask.AutoSize = True
		Me.lblExecuteTask.BackColor = System.Drawing.SystemColors.Control
		Me.lblExecuteTask.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblExecuteTask.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblExecuteTask.Enabled = True
		Me.lblExecuteTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblExecuteTask.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblExecuteTask.Location = New System.Drawing.Point(406, 98)
		Me.lblExecuteTask.Name = "lblExecuteTask"
		Me.lblExecuteTask.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblExecuteTask.Size = New System.Drawing.Size(81, 13)
		Me.lblExecuteTask.TabIndex = 51
		Me.lblExecuteTask.Text = "Execute Task:"
		Me.lblExecuteTask.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblExecuteTask.UseMnemonic = True
		Me.lblExecuteTask.Visible = True
		' 
		' lblStepDaysDuration
		' 
		Me.lblStepDaysDuration.AutoSize = True
		Me.lblStepDaysDuration.BackColor = System.Drawing.SystemColors.Control
		Me.lblStepDaysDuration.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblStepDaysDuration.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblStepDaysDuration.Enabled = True
		Me.lblStepDaysDuration.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblStepDaysDuration.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblStepDaysDuration.Location = New System.Drawing.Point(368, 48)
		Me.lblStepDaysDuration.Name = "lblStepDaysDuration"
		Me.lblStepDaysDuration.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblStepDaysDuration.Size = New System.Drawing.Size(85, 13)
		Me.lblStepDaysDuration.TabIndex = 43
		Me.lblStepDaysDuration.Text = "Task Duration:"
		Me.lblStepDaysDuration.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblStepDaysDuration.UseMnemonic = True
		Me.lblStepDaysDuration.Visible = True
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
		Me.lblTask.TabIndex = 42
		Me.lblTask.Text = "Task:"
		Me.lblTask.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblTask.UseMnemonic = True
		Me.lblTask.Visible = True
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
		Me.lblActionType.Location = New System.Drawing.Point(27, 46)
		Me.lblActionType.Name = "lblActionType"
		Me.lblActionType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblActionType.Size = New System.Drawing.Size(68, 13)
		Me.lblActionType.TabIndex = 41
		Me.lblActionType.Text = "ActionType:"
		Me.lblActionType.TextAlign = System.Drawing.ContentAlignment.TopRight
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
		Me.lblTaskGroup.Location = New System.Drawing.Point(24, 20)
		Me.lblTaskGroup.Name = "lblTaskGroup"
		Me.lblTaskGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTaskGroup.Size = New System.Drawing.Size(71, 13)
		Me.lblTaskGroup.TabIndex = 40
		Me.lblTaskGroup.Text = "Task Group:"
		Me.lblTaskGroup.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblTaskGroup.UseMnemonic = True
		Me.lblTaskGroup.Visible = True
		' 
		' txtCode
		' 
		Me.txtCode.AcceptsReturn = True
		Me.txtCode.AutoSize = False
		Me.txtCode.BackColor = System.Drawing.SystemColors.Window
		Me.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtCode.CausesValidation = True
		Me.txtCode.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtCode.Enabled = True
		Me.txtCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtCode.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtCode.HideSelection = True
		Me.txtCode.Location = New System.Drawing.Point(104, 16)
		Me.txtCode.MaxLength = 10
		Me.txtCode.Multiline = False
		Me.txtCode.Name = "txtCode"
		Me.txtCode.ReadOnly = False
		Me.txtCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtCode.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtCode.Size = New System.Drawing.Size(89, 21)
		Me.txtCode.TabIndex = 0
		Me.txtCode.TabStop = True
		Me.txtCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtCode.Visible = True
		' 
		' txtEffectiveDate
		' 
		Me.txtEffectiveDate.AcceptsReturn = True
		Me.txtEffectiveDate.AutoSize = False
		Me.txtEffectiveDate.BackColor = System.Drawing.SystemColors.Window
		Me.txtEffectiveDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtEffectiveDate.CausesValidation = True
		Me.txtEffectiveDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtEffectiveDate.Enabled = True
		Me.txtEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtEffectiveDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtEffectiveDate.HideSelection = True
		Me.txtEffectiveDate.Location = New System.Drawing.Point(104, 67)
		Me.txtEffectiveDate.MaxLength = 0
		Me.txtEffectiveDate.Multiline = False
		Me.txtEffectiveDate.Name = "txtEffectiveDate"
		Me.txtEffectiveDate.ReadOnly = False
		Me.txtEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtEffectiveDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtEffectiveDate.Size = New System.Drawing.Size(89, 21)
		Me.txtEffectiveDate.TabIndex = 2
		Me.txtEffectiveDate.TabStop = True
		Me.txtEffectiveDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtEffectiveDate.Visible = True
		' 
		' txtDescription
		' 
		Me.txtDescription.AcceptsReturn = True
		Me.txtDescription.AutoSize = False
		Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
		Me.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDescription.CausesValidation = True
		Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDescription.Enabled = True
		Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDescription.HideSelection = True
		Me.txtDescription.Location = New System.Drawing.Point(104, 41)
		Me.txtDescription.MaxLength = 255
		Me.txtDescription.Multiline = False
		Me.txtDescription.Name = "txtDescription"
		Me.txtDescription.ReadOnly = False
		Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDescription.Size = New System.Drawing.Size(289, 21)
		Me.txtDescription.TabIndex = 1
		Me.txtDescription.TabStop = True
		Me.txtDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDescription.Visible = True
		' 
		' lblCode
		' 
		Me.lblCode.AutoSize = True
		Me.lblCode.BackColor = System.Drawing.SystemColors.Control
		Me.lblCode.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCode.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCode.Enabled = True
		Me.lblCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCode.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCode.Location = New System.Drawing.Point(66, 20)
		Me.lblCode.Name = "lblCode"
		Me.lblCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCode.Size = New System.Drawing.Size(35, 13)
		Me.lblCode.TabIndex = 35
		Me.lblCode.Text = "Code:"
		Me.lblCode.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblCode.UseMnemonic = True
		Me.lblCode.Visible = True
		' 
		' lblEffectiveDate
		' 
		Me.lblEffectiveDate.AutoSize = True
		Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblEffectiveDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblEffectiveDate.Enabled = True
		Me.lblEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblEffectiveDate.Location = New System.Drawing.Point(16, 71)
		Me.lblEffectiveDate.Name = "lblEffectiveDate"
		Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblEffectiveDate.Size = New System.Drawing.Size(85, 13)
		Me.lblEffectiveDate.TabIndex = 33
		Me.lblEffectiveDate.Text = "Effective Date:"
		Me.lblEffectiveDate.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblEffectiveDate.UseMnemonic = True
		Me.lblEffectiveDate.Visible = True
		' 
		' lblDescription
		' 
		Me.lblDescription.AutoSize = True
		Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
		Me.lblDescription.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDescription.Enabled = True
		Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDescription.Location = New System.Drawing.Point(32, 45)
		Me.lblDescription.Name = "lblDescription"
		Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDescription.Size = New System.Drawing.Size(69, 13)
		Me.lblDescription.TabIndex = 32
		Me.lblDescription.Text = "Description:"
		Me.lblDescription.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblDescription.UseMnemonic = True
		Me.lblDescription.Visible = True
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(504, 480)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 28
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' frmDetails
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(739, 508)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.tabMainTab)
		Me.Controls.Add(Me.cmdOK)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmDetails.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmDetails"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Edit Action Type"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMainTab, 1)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tabMainTab.ResumeLayout(False)
		Me._tabMainTab_TabPage0.ResumeLayout(False)
		Me.fraWorkflowStep.ResumeLayout(False)
		Me.frmOverdue.ResumeLayout(False)
		Me.frmInTime.ResumeLayout(False)
		Me.fraEvent.ResumeLayout(False)
		Me.fraTask.ResumeLayout(False)
		Me.fraAllocate.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class