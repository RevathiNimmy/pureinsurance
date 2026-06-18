<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
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
    'developer guide no. Project name changed from AxListbar to Listbar and class name changed from AxSSListBar to SSListBar.
    'Public WithEvents albTaskImages As AxListbar.AxSSListBar
    Public WithEvents albTaskImages As Windows.Forms.PictureBox
	Public WithEvents uctPMResizer1 As PMResizerControl.uctPMResizer
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents imgIcon1 As System.Windows.Forms.PictureBox
	Public WithEvents cboTaskUser As PMUserLookupControl.cboPMUserLookup
	Public WithEvents cboTaskUserGroup As PMUGroupLookupCtrl.cboPMUserGroupByTask
	Public WithEvents lblTaskUser As System.Windows.Forms.Label
	Public WithEvents lblTaskUserGroup As System.Windows.Forms.Label
	Public WithEvents fraAllocation As System.Windows.Forms.GroupBox
	Public WithEvents cboDueDate As System.Windows.Forms.ComboBox
    Public WithEvents cboWrkTaskGroup As PMLookupControl.cboPMLookup
	Public WithEvents txtDueTime As System.Windows.Forms.TextBox
	Public WithEvents chkIsUrgent As System.Windows.Forms.CheckBox
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents txtCustomer As System.Windows.Forms.TextBox
	Public WithEvents txtDueDate As System.Windows.Forms.TextBox
	Public WithEvents chkIsComplete As System.Windows.Forms.CheckBox
	Public WithEvents cboWrkTask As PMTaskLookupControl.cboPMTaskLookup
	Public WithEvents lblTaskGroup As System.Windows.Forms.Label
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents lblCustomer As System.Windows.Forms.Label
	Public WithEvents lblDueDate As System.Windows.Forms.Label
	Public WithEvents lblWrkTask As System.Windows.Forms.Label
	Public WithEvents fraTaskDetails As System.Windows.Forms.GroupBox
	Public WithEvents cmdTaskLog As System.Windows.Forms.Button
	Public WithEvents cmdLinkObject As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents imgIcon2 As System.Windows.Forms.PictureBox
	Public WithEvents txtLoggedByUser As System.Windows.Forms.TextBox
	Public WithEvents txtLoggedDate As System.Windows.Forms.TextBox
	Public WithEvents txtModifiedByUser As System.Windows.Forms.TextBox
	Public WithEvents txtModifiedDate As System.Windows.Forms.TextBox
	Public WithEvents txtLoggedTime As System.Windows.Forms.TextBox
	Public WithEvents txtModifiedTime As System.Windows.Forms.TextBox
	Public WithEvents cboLoggedByUser As PMUserLookupControl.cboPMUserLookup
	Public WithEvents cboModifiedByUser As PMUserLookupControl.cboPMUserLookup
	Public WithEvents lblTaskLoggedByUser As System.Windows.Forms.Label
	Public WithEvents lblTaskLoggedDate As System.Windows.Forms.Label
	Public WithEvents lblTaskModifiedByUser As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents fraAudit As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.albTaskImages = New System.Windows.Forms.PictureBox
        Me.uctPMResizer1 = New PMResizerControl.uctPMResizer
        Me.cmdNavigate = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.imgIcon1 = New System.Windows.Forms.PictureBox
        Me.fraAllocation = New System.Windows.Forms.GroupBox
        Me.cboTaskUser = New PMUserLookupControl.cboPMUserLookup
        Me.cboTaskUserGroup = New PMUGroupLookupCtrl.cboPMUserGroupByTask
        Me.lblTaskUser = New System.Windows.Forms.Label
        Me.lblTaskUserGroup = New System.Windows.Forms.Label
        Me.fraTaskDetails = New System.Windows.Forms.GroupBox
        Me.cboDueDate = New System.Windows.Forms.ComboBox
        Me.cboWrkTaskGroup = New PMLookupControl.cboPMLookup
        Me.txtDueTime = New System.Windows.Forms.TextBox
        Me.chkIsUrgent = New System.Windows.Forms.CheckBox
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.txtCustomer = New System.Windows.Forms.TextBox
        Me.txtDueDate = New System.Windows.Forms.TextBox
        Me.chkIsComplete = New System.Windows.Forms.CheckBox
        Me.cboWrkTask = New PMTaskLookupControl.cboPMTaskLookup
        Me.lblTaskGroup = New System.Windows.Forms.Label
        Me.lblDescription = New System.Windows.Forms.Label
        Me.lblCustomer = New System.Windows.Forms.Label
        Me.lblDueDate = New System.Windows.Forms.Label
        Me.lblWrkTask = New System.Windows.Forms.Label
        Me.cmdTaskLog = New System.Windows.Forms.Button
        Me.cmdLinkObject = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me.imgIcon2 = New System.Windows.Forms.PictureBox
        Me.fraAudit = New System.Windows.Forms.GroupBox
        Me.txtLoggedByUser = New System.Windows.Forms.TextBox
        Me.txtLoggedDate = New System.Windows.Forms.TextBox
        Me.txtModifiedByUser = New System.Windows.Forms.TextBox
        Me.txtModifiedDate = New System.Windows.Forms.TextBox
        Me.txtLoggedTime = New System.Windows.Forms.TextBox
        Me.txtModifiedTime = New System.Windows.Forms.TextBox
        Me.cboLoggedByUser = New PMUserLookupControl.cboPMUserLookup
        Me.cboModifiedByUser = New PMUserLookupControl.cboPMUserLookup
        Me.lblTaskLoggedByUser = New System.Windows.Forms.Label
        Me.lblTaskLoggedDate = New System.Windows.Forms.Label
        Me.lblTaskModifiedByUser = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        CType(Me.albTaskImages, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.imgIcon1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraAllocation.SuspendLayout()
        Me.fraTaskDetails.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        CType(Me.imgIcon2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraAudit.SuspendLayout()
        Me.SuspendLayout()
        '
        'albTaskImages
        '
        Me.albTaskImages.Location = New System.Drawing.Point(176, 392)
        Me.albTaskImages.Name = "albTaskImages"
        Me.albTaskImages.Size = New System.Drawing.Size(9, 9)
        Me.albTaskImages.TabIndex = 40
        Me.albTaskImages.TabStop = False
        Me.albTaskImages.Visible = False
        '
        'uctPMResizer1
        '
        Me.uctPMResizer1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMResizer1.Location = New System.Drawing.Point(112, 392)
        Me.uctPMResizer1.Name = "uctPMResizer1"
        Me.uctPMResizer1.Size = New System.Drawing.Size(32, 30)
        Me.uctPMResizer1.TabIndex = 41
        Me.uctPMResizer1.Visible = False
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(8, 392)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 15
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(536, 392)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 14
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
        Me.cmdCancel.Location = New System.Drawing.Point(456, 392)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 13
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
        Me.cmdOK.Location = New System.Drawing.Point(376, 392)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 12
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage1)
        Me.tabMainTab.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(199, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(605, 381)
        Me.tabMainTab.TabIndex = 16
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.imgIcon1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraAllocation)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraTaskDetails)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdTaskLog)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdLinkObject)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(597, 355)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Task Details"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'imgIcon1
        '
        Me.imgIcon1.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon1.Image = CType(resources.GetObject("imgIcon1.Image"), System.Drawing.Image)
        Me.imgIcon1.Location = New System.Drawing.Point(560, 4)
        Me.imgIcon1.Name = "imgIcon1"
        Me.imgIcon1.Size = New System.Drawing.Size(32, 32)
        Me.imgIcon1.TabIndex = 0
        Me.imgIcon1.TabStop = False
        '
        'fraAllocation
        '
        Me.fraAllocation.BackColor = System.Drawing.SystemColors.Control
        Me.fraAllocation.Controls.Add(Me.cboTaskUser)
        Me.fraAllocation.Controls.Add(Me.cboTaskUserGroup)
        Me.fraAllocation.Controls.Add(Me.lblTaskUser)
        Me.fraAllocation.Controls.Add(Me.lblTaskUserGroup)
        Me.fraAllocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAllocation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAllocation.Location = New System.Drawing.Point(16, 236)
        Me.fraAllocation.Name = "fraAllocation"
        Me.fraAllocation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAllocation.Size = New System.Drawing.Size(577, 65)
        Me.fraAllocation.TabIndex = 22
        Me.fraAllocation.TabStop = False
        Me.fraAllocation.Text = "Allocation"
        '
        'cboTaskUser
        '
        Me.cboTaskUser.DefaultUserID = 0
        Me.cboTaskUser.FirstItem = ""
        Me.cboTaskUser.ListIndex = -1
        Me.cboTaskUser.Location = New System.Drawing.Point(375, 24)
        Me.cboTaskUser.Name = "cboTaskUser"
        Me.cboTaskUser.PMUserGroupID = 0
        Me.cboTaskUser.SingleUserID = 0
        Me.cboTaskUser.Size = New System.Drawing.Size(193, 21)
        Me.cboTaskUser.Sorted = True
        Me.cboTaskUser.TabIndex = 10
        Me.cboTaskUser.ToolTipText = ""
        Me.cboTaskUser.UserID = 0
        '
        'cboTaskUserGroup
        '
        Me.cboTaskUserGroup.DefaultTaskGroupID = 0
        Me.cboTaskUserGroup.FirstItem = ""
        Me.cboTaskUserGroup.ListIndex = -1
        Me.cboTaskUserGroup.Location = New System.Drawing.Point(104, 24)
        Me.cboTaskUserGroup.Name = "cboTaskUserGroup"
        Me.cboTaskUserGroup.PMTaskGroupID = 0
        Me.cboTaskUserGroup.SingleUserGroupID = 0
        Me.cboTaskUserGroup.Size = New System.Drawing.Size(193, 21)
        Me.cboTaskUserGroup.Sorted = True
        Me.cboTaskUserGroup.TabIndex = 9
        Me.cboTaskUserGroup.ToolTipText = ""
        Me.cboTaskUserGroup.UserGroupID = 0
        '
        'lblTaskUser
        '
        Me.lblTaskUser.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaskUser.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaskUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaskUser.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaskUser.Location = New System.Drawing.Point(333, 28)
        Me.lblTaskUser.Name = "lblTaskUser"
        Me.lblTaskUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaskUser.Size = New System.Drawing.Size(33, 13)
        Me.lblTaskUser.TabIndex = 25
        Me.lblTaskUser.Text = "User:"
        '
        'lblTaskUserGroup
        '
        Me.lblTaskUserGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaskUserGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaskUserGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaskUserGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaskUserGroup.Location = New System.Drawing.Point(8, 28)
        Me.lblTaskUserGroup.Name = "lblTaskUserGroup"
        Me.lblTaskUserGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaskUserGroup.Size = New System.Drawing.Size(87, 13)
        Me.lblTaskUserGroup.TabIndex = 24
        Me.lblTaskUserGroup.Text = "User Group:"
        '
        'fraTaskDetails
        '
        Me.fraTaskDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraTaskDetails.Controls.Add(Me.cboDueDate)
        Me.fraTaskDetails.Controls.Add(Me.cboWrkTaskGroup)
        Me.fraTaskDetails.Controls.Add(Me.txtDueTime)
        Me.fraTaskDetails.Controls.Add(Me.chkIsUrgent)
        Me.fraTaskDetails.Controls.Add(Me.txtDescription)
        Me.fraTaskDetails.Controls.Add(Me.txtCustomer)
        Me.fraTaskDetails.Controls.Add(Me.txtDueDate)
        Me.fraTaskDetails.Controls.Add(Me.chkIsComplete)
        Me.fraTaskDetails.Controls.Add(Me.cboWrkTask)
        Me.fraTaskDetails.Controls.Add(Me.lblTaskGroup)
        Me.fraTaskDetails.Controls.Add(Me.lblDescription)
        Me.fraTaskDetails.Controls.Add(Me.lblCustomer)
        Me.fraTaskDetails.Controls.Add(Me.lblDueDate)
        Me.fraTaskDetails.Controls.Add(Me.lblWrkTask)
        Me.fraTaskDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTaskDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTaskDetails.Location = New System.Drawing.Point(16, 44)
        Me.fraTaskDetails.Name = "fraTaskDetails"
        Me.fraTaskDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTaskDetails.Size = New System.Drawing.Size(577, 185)
        Me.fraTaskDetails.TabIndex = 17
        Me.fraTaskDetails.TabStop = False
        Me.fraTaskDetails.Text = "Task Details"
        '
        'cboDueDate
        '
        Me.cboDueDate.BackColor = System.Drawing.SystemColors.Window
        Me.cboDueDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboDueDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDueDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboDueDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboDueDate.Location = New System.Drawing.Point(104, 56)
        Me.cboDueDate.Name = "cboDueDate"
        Me.cboDueDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboDueDate.Size = New System.Drawing.Size(193, 21)
        Me.cboDueDate.TabIndex = 2
        '
        'cboWrkTaskGroup
        '
        Me.cboWrkTaskGroup.DefaultItemId = 0
        Me.cboWrkTaskGroup.FirstItem = ""
        Me.cboWrkTaskGroup.ItemId = 0
        Me.cboWrkTaskGroup.ListIndex = -1
        Me.cboWrkTaskGroup.Location = New System.Drawing.Point(104, 24)
        Me.cboWrkTaskGroup.Name = "cboWrkTaskGroup"
        Me.cboWrkTaskGroup.PMLookupProductFamily = 1
        Me.cboWrkTaskGroup.SingleItemId = 0
        Me.cboWrkTaskGroup.Size = New System.Drawing.Size(193, 21)
        Me.cboWrkTaskGroup.Sorted = True
        Me.cboWrkTaskGroup.TabIndex = 0
        Me.cboWrkTaskGroup.TableName = "PMWrk_Task_Group"
        Me.cboWrkTaskGroup.ToolTipText = ""
        Me.cboWrkTaskGroup.WhereClause = ""
        '
        'txtDueTime
        '
        Me.txtDueTime.AcceptsReturn = True
        Me.txtDueTime.BackColor = System.Drawing.SystemColors.Window
        Me.txtDueTime.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDueTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDueTime.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDueTime.Location = New System.Drawing.Point(488, 56)
        Me.txtDueTime.MaxLength = 0
        Me.txtDueTime.Name = "txtDueTime"
        Me.txtDueTime.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDueTime.Size = New System.Drawing.Size(73, 20)
        Me.txtDueTime.TabIndex = 4
        '
        'chkIsUrgent
        '
        Me.chkIsUrgent.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsUrgent.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsUrgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsUrgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsUrgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsUrgent.Location = New System.Drawing.Point(8, 152)
        Me.chkIsUrgent.Name = "chkIsUrgent"
        Me.chkIsUrgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsUrgent.Size = New System.Drawing.Size(110, 17)
        Me.chkIsUrgent.TabIndex = 7
        Me.chkIsUrgent.Text = "Urgent:"
        Me.chkIsUrgent.UseVisualStyleBackColor = False
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(104, 120)
        Me.txtDescription.MaxLength = 255
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(459, 20)
        Me.txtDescription.TabIndex = 6
        '
        'txtCustomer
        '
        Me.txtCustomer.AcceptsReturn = True
        Me.txtCustomer.BackColor = System.Drawing.SystemColors.Window
        Me.txtCustomer.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCustomer.Enabled = False
        Me.txtCustomer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCustomer.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCustomer.Location = New System.Drawing.Point(104, 88)
        Me.txtCustomer.MaxLength = 255
        Me.txtCustomer.Name = "txtCustomer"
        Me.txtCustomer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCustomer.Size = New System.Drawing.Size(459, 20)
        Me.txtCustomer.TabIndex = 5
        '
        'txtDueDate
        '
        Me.txtDueDate.AcceptsReturn = True
        Me.txtDueDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtDueDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDueDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDueDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDueDate.Location = New System.Drawing.Point(368, 56)
        Me.txtDueDate.MaxLength = 0
        Me.txtDueDate.Name = "txtDueDate"
        Me.txtDueDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDueDate.Size = New System.Drawing.Size(113, 20)
        Me.txtDueDate.TabIndex = 3
        '
        'chkIsComplete
        '
        Me.chkIsComplete.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsComplete.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsComplete.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsComplete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsComplete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsComplete.Location = New System.Drawing.Point(160, 152)
        Me.chkIsComplete.Name = "chkIsComplete"
        Me.chkIsComplete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsComplete.Size = New System.Drawing.Size(85, 17)
        Me.chkIsComplete.TabIndex = 8
        Me.chkIsComplete.Text = "Complete:"
        Me.chkIsComplete.UseVisualStyleBackColor = False
        '
        'cboWrkTask
        '
        Me.cboWrkTask.DefaultTaskID = 0
        Me.cboWrkTask.FirstItem = ""
        Me.cboWrkTask.ListIndex = -1
        Me.cboWrkTask.Location = New System.Drawing.Point(368, 24)
        Me.cboWrkTask.Name = "cboWrkTask"
        Me.cboWrkTask.PMTaskGroupID = 0
        Me.cboWrkTask.SingleTaskID = 0
        Me.cboWrkTask.Size = New System.Drawing.Size(193, 21)
        Me.cboWrkTask.TabIndex = 1
        Me.cboWrkTask.TaskID = 0
        Me.cboWrkTask.ToolTipText = ""
        '
        'lblTaskGroup
        '
        Me.lblTaskGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaskGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaskGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaskGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaskGroup.Location = New System.Drawing.Point(8, 28)
        Me.lblTaskGroup.Name = "lblTaskGroup"
        Me.lblTaskGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaskGroup.Size = New System.Drawing.Size(81, 12)
        Me.lblTaskGroup.TabIndex = 23
        Me.lblTaskGroup.Text = "Task Group:"
        '
        'lblDescription
        '
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(8, 123)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(89, 13)
        Me.lblDescription.TabIndex = 21
        Me.lblDescription.Text = "Description:"
        '
        'lblCustomer
        '
        Me.lblCustomer.BackColor = System.Drawing.SystemColors.Control
        Me.lblCustomer.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCustomer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCustomer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCustomer.Location = New System.Drawing.Point(8, 91)
        Me.lblCustomer.Name = "lblCustomer"
        Me.lblCustomer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCustomer.Size = New System.Drawing.Size(57, 13)
        Me.lblCustomer.TabIndex = 20
        Me.lblCustomer.Text = "Client:"
        '
        'lblDueDate
        '
        Me.lblDueDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblDueDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDueDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDueDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDueDate.Location = New System.Drawing.Point(8, 59)
        Me.lblDueDate.Name = "lblDueDate"
        Me.lblDueDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDueDate.Size = New System.Drawing.Size(97, 13)
        Me.lblDueDate.TabIndex = 19
        Me.lblDueDate.Text = "Due Date/Time:"
        '
        'lblWrkTask
        '
        Me.lblWrkTask.BackColor = System.Drawing.SystemColors.Control
        Me.lblWrkTask.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblWrkTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWrkTask.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblWrkTask.Location = New System.Drawing.Point(333, 28)
        Me.lblWrkTask.Name = "lblWrkTask"
        Me.lblWrkTask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblWrkTask.Size = New System.Drawing.Size(41, 13)
        Me.lblWrkTask.TabIndex = 18
        Me.lblWrkTask.Text = "Task:"
        '
        'cmdTaskLog
        '
        Me.cmdTaskLog.BackColor = System.Drawing.SystemColors.Control
        Me.cmdTaskLog.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdTaskLog.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTaskLog.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdTaskLog.Location = New System.Drawing.Point(520, 324)
        Me.cmdTaskLog.Name = "cmdTaskLog"
        Me.cmdTaskLog.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdTaskLog.Size = New System.Drawing.Size(73, 22)
        Me.cmdTaskLog.TabIndex = 11
        Me.cmdTaskLog.Text = "&Task Log"
        Me.cmdTaskLog.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdTaskLog.UseVisualStyleBackColor = False
        '
        'cmdLinkObject
        '
        Me.cmdLinkObject.BackColor = System.Drawing.SystemColors.Control
        Me.cmdLinkObject.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdLinkObject.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdLinkObject.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdLinkObject.Location = New System.Drawing.Point(16, 324)
        Me.cmdLinkObject.Name = "cmdLinkObject"
        Me.cmdLinkObject.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdLinkObject.Size = New System.Drawing.Size(73, 22)
        Me.cmdLinkObject.TabIndex = 39
        Me.cmdLinkObject.Text = "Link Object"
        Me.cmdLinkObject.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdLinkObject.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.imgIcon2)
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraAudit)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(597, 355)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "2 - Audit Details"
        Me._tabMainTab_TabPage1.UseVisualStyleBackColor = True
        '
        'imgIcon2
        '
        Me.imgIcon2.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon2.Image = CType(resources.GetObject("imgIcon2.Image"), System.Drawing.Image)
        Me.imgIcon2.Location = New System.Drawing.Point(560, 4)
        Me.imgIcon2.Name = "imgIcon2"
        Me.imgIcon2.Size = New System.Drawing.Size(32, 32)
        Me.imgIcon2.TabIndex = 0
        Me.imgIcon2.TabStop = False
        '
        'fraAudit
        '
        Me.fraAudit.BackColor = System.Drawing.SystemColors.Control
        Me.fraAudit.Controls.Add(Me.txtLoggedByUser)
        Me.fraAudit.Controls.Add(Me.txtLoggedDate)
        Me.fraAudit.Controls.Add(Me.txtModifiedByUser)
        Me.fraAudit.Controls.Add(Me.txtModifiedDate)
        Me.fraAudit.Controls.Add(Me.txtLoggedTime)
        Me.fraAudit.Controls.Add(Me.txtModifiedTime)
        Me.fraAudit.Controls.Add(Me.cboLoggedByUser)
        Me.fraAudit.Controls.Add(Me.cboModifiedByUser)
        Me.fraAudit.Controls.Add(Me.lblTaskLoggedByUser)
        Me.fraAudit.Controls.Add(Me.lblTaskLoggedDate)
        Me.fraAudit.Controls.Add(Me.lblTaskModifiedByUser)
        Me.fraAudit.Controls.Add(Me.Label1)
        Me.fraAudit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAudit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAudit.Location = New System.Drawing.Point(8, 60)
        Me.fraAudit.Name = "fraAudit"
        Me.fraAudit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAudit.Size = New System.Drawing.Size(577, 89)
        Me.fraAudit.TabIndex = 26
        Me.fraAudit.TabStop = False
        Me.fraAudit.Text = "Audit"
        '
        'txtLoggedByUser
        '
        Me.txtLoggedByUser.AcceptsReturn = True
        Me.txtLoggedByUser.BackColor = System.Drawing.SystemColors.Control
        Me.txtLoggedByUser.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLoggedByUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLoggedByUser.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLoggedByUser.Location = New System.Drawing.Point(104, 24)
        Me.txtLoggedByUser.MaxLength = 0
        Me.txtLoggedByUser.Name = "txtLoggedByUser"
        Me.txtLoggedByUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLoggedByUser.Size = New System.Drawing.Size(193, 20)
        Me.txtLoggedByUser.TabIndex = 32
        '
        'txtLoggedDate
        '
        Me.txtLoggedDate.AcceptsReturn = True
        Me.txtLoggedDate.BackColor = System.Drawing.SystemColors.Control
        Me.txtLoggedDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLoggedDate.Enabled = False
        Me.txtLoggedDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLoggedDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLoggedDate.Location = New System.Drawing.Point(375, 24)
        Me.txtLoggedDate.MaxLength = 0
        Me.txtLoggedDate.Name = "txtLoggedDate"
        Me.txtLoggedDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLoggedDate.Size = New System.Drawing.Size(113, 20)
        Me.txtLoggedDate.TabIndex = 31
        Me.txtLoggedDate.TabStop = False
        '
        'txtModifiedByUser
        '
        Me.txtModifiedByUser.AcceptsReturn = True
        Me.txtModifiedByUser.BackColor = System.Drawing.SystemColors.Control
        Me.txtModifiedByUser.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtModifiedByUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtModifiedByUser.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtModifiedByUser.Location = New System.Drawing.Point(104, 56)
        Me.txtModifiedByUser.MaxLength = 0
        Me.txtModifiedByUser.Name = "txtModifiedByUser"
        Me.txtModifiedByUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtModifiedByUser.Size = New System.Drawing.Size(193, 20)
        Me.txtModifiedByUser.TabIndex = 30
        '
        'txtModifiedDate
        '
        Me.txtModifiedDate.AcceptsReturn = True
        Me.txtModifiedDate.BackColor = System.Drawing.SystemColors.Control
        Me.txtModifiedDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtModifiedDate.Enabled = False
        Me.txtModifiedDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtModifiedDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtModifiedDate.Location = New System.Drawing.Point(375, 56)
        Me.txtModifiedDate.MaxLength = 0
        Me.txtModifiedDate.Name = "txtModifiedDate"
        Me.txtModifiedDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtModifiedDate.Size = New System.Drawing.Size(113, 20)
        Me.txtModifiedDate.TabIndex = 29
        Me.txtModifiedDate.TabStop = False
        '
        'txtLoggedTime
        '
        Me.txtLoggedTime.AcceptsReturn = True
        Me.txtLoggedTime.BackColor = System.Drawing.SystemColors.Control
        Me.txtLoggedTime.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLoggedTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLoggedTime.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLoggedTime.Location = New System.Drawing.Point(500, 24)
        Me.txtLoggedTime.MaxLength = 0
        Me.txtLoggedTime.Name = "txtLoggedTime"
        Me.txtLoggedTime.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLoggedTime.Size = New System.Drawing.Size(65, 20)
        Me.txtLoggedTime.TabIndex = 28
        '
        'txtModifiedTime
        '
        Me.txtModifiedTime.AcceptsReturn = True
        Me.txtModifiedTime.BackColor = System.Drawing.SystemColors.Control
        Me.txtModifiedTime.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtModifiedTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtModifiedTime.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtModifiedTime.Location = New System.Drawing.Point(500, 56)
        Me.txtModifiedTime.MaxLength = 0
        Me.txtModifiedTime.Name = "txtModifiedTime"
        Me.txtModifiedTime.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtModifiedTime.Size = New System.Drawing.Size(65, 20)
        Me.txtModifiedTime.TabIndex = 27
        '
        'cboLoggedByUser
        '
        Me.cboLoggedByUser.DefaultUserID = 0
        Me.cboLoggedByUser.Enabled = False
        Me.cboLoggedByUser.FirstItem = ""
        Me.cboLoggedByUser.ListIndex = -1
        Me.cboLoggedByUser.Location = New System.Drawing.Point(104, 24)
        Me.cboLoggedByUser.Name = "cboLoggedByUser"
        Me.cboLoggedByUser.PMUserGroupID = 0
        Me.cboLoggedByUser.SingleUserID = 0
        Me.cboLoggedByUser.Size = New System.Drawing.Size(193, 21)
        Me.cboLoggedByUser.Sorted = True
        Me.cboLoggedByUser.TabIndex = 33
        Me.cboLoggedByUser.ToolTipText = ""
        Me.cboLoggedByUser.UserID = 0
        Me.cboLoggedByUser.Visible = False
        '
        'cboModifiedByUser
        '
        Me.cboModifiedByUser.DefaultUserID = 0
        Me.cboModifiedByUser.Enabled = False
        Me.cboModifiedByUser.FirstItem = ""
        Me.cboModifiedByUser.ListIndex = -1
        Me.cboModifiedByUser.Location = New System.Drawing.Point(104, 56)
        Me.cboModifiedByUser.Name = "cboModifiedByUser"
        Me.cboModifiedByUser.PMUserGroupID = 0
        Me.cboModifiedByUser.SingleUserID = 0
        Me.cboModifiedByUser.Size = New System.Drawing.Size(193, 21)
        Me.cboModifiedByUser.Sorted = True
        Me.cboModifiedByUser.TabIndex = 34
        Me.cboModifiedByUser.ToolTipText = ""
        Me.cboModifiedByUser.UserID = 0
        Me.cboModifiedByUser.Visible = False
        '
        'lblTaskLoggedByUser
        '
        Me.lblTaskLoggedByUser.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaskLoggedByUser.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaskLoggedByUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaskLoggedByUser.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaskLoggedByUser.Location = New System.Drawing.Point(8, 28)
        Me.lblTaskLoggedByUser.Name = "lblTaskLoggedByUser"
        Me.lblTaskLoggedByUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaskLoggedByUser.Size = New System.Drawing.Size(65, 13)
        Me.lblTaskLoggedByUser.TabIndex = 38
        Me.lblTaskLoggedByUser.Text = "Logged By:"
        '
        'lblTaskLoggedDate
        '
        Me.lblTaskLoggedDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaskLoggedDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaskLoggedDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaskLoggedDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaskLoggedDate.Location = New System.Drawing.Point(336, 28)
        Me.lblTaskLoggedDate.Name = "lblTaskLoggedDate"
        Me.lblTaskLoggedDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaskLoggedDate.Size = New System.Drawing.Size(25, 13)
        Me.lblTaskLoggedDate.TabIndex = 37
        Me.lblTaskLoggedDate.Text = "At:"
        '
        'lblTaskModifiedByUser
        '
        Me.lblTaskModifiedByUser.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaskModifiedByUser.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaskModifiedByUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaskModifiedByUser.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaskModifiedByUser.Location = New System.Drawing.Point(8, 60)
        Me.lblTaskModifiedByUser.Name = "lblTaskModifiedByUser"
        Me.lblTaskModifiedByUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaskModifiedByUser.Size = New System.Drawing.Size(89, 13)
        Me.lblTaskModifiedByUser.TabIndex = 36
        Me.lblTaskModifiedByUser.Text = "Last Modified By:"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(336, 60)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(25, 13)
        Me.Label1.TabIndex = 35
        Me.Label1.Text = "At:"
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(619, 416)
        Me.Controls.Add(Me.albTaskImages)
        Me.Controls.Add(Me.uctPMResizer1)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.MaximizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = "Task"
        Me.Text = "Work Manager - Task"
        CType(Me.albTaskImages, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        CType(Me.imgIcon1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraAllocation.ResumeLayout(False)
        Me.fraTaskDetails.ResumeLayout(False)
        Me.fraTaskDetails.PerformLayout()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        CType(Me.imgIcon2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraAudit.ResumeLayout(False)
        Me.fraAudit.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class