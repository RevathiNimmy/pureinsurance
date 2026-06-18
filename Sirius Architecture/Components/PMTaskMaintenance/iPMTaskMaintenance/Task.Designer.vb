<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTask
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents lblTaskName As System.Windows.Forms.Label
	Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents lblTypeOfTask As System.Windows.Forms.Label
	Public WithEvents lblPMNavProcessId As System.Windows.Forms.Label
	Public WithEvents lblComponentClassName As System.Windows.Forms.Label
	Public WithEvents lblAutoDeleteAfterNumDays As System.Windows.Forms.Label
	Public WithEvents imgIcon As System.Windows.Forms.PictureBox
	Public WithEvents lblLinkedClassName As System.Windows.Forms.Label
	Public WithEvents lblLinkedCaption As System.Windows.Forms.Label
	Public WithEvents lblIsSystemTask As System.Windows.Forms.Label
	Public WithEvents lblIsViewOnlyTask As System.Windows.Forms.Label
	Public WithEvents lblIsAvailableTask As System.Windows.Forms.Label
	Public WithEvents lblComponentObjectName As System.Windows.Forms.Label
	Public WithEvents lblLinkedObjectName As System.Windows.Forms.Label
	Public WithEvents lblTaskGroupCategory As System.Windows.Forms.Label
	Public WithEvents cboTaskCategory As PMLookupControl.cboPMLookup
	Public WithEvents pnlEffectiveDate As System.Windows.Forms.Panel
	Public WithEvents txtTaskName As System.Windows.Forms.TextBox
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents chkIsSystemTask As System.Windows.Forms.CheckBox
	Public WithEvents cboTypeOfTask As System.Windows.Forms.ComboBox
	Public WithEvents txtComponentObjectName As System.Windows.Forms.TextBox
	Public WithEvents txtComponentClassName As System.Windows.Forms.TextBox
	Public WithEvents txtAutoDeleteAfterNumDays As System.Windows.Forms.TextBox
	Public WithEvents cboPMNavProcessId As PMLookupControl.cboPMLookup
	Public WithEvents cmdSelectIcon As System.Windows.Forms.Button
	Public WithEvents txtLinkedObjectName As System.Windows.Forms.TextBox
	Public WithEvents txtLinkedClassName As System.Windows.Forms.TextBox
	Public WithEvents chkIsViewOnlyTask As System.Windows.Forms.CheckBox
	Public WithEvents txtLinkedCaption As System.Windows.Forms.TextBox
	Public WithEvents chkIsAvailableTask As System.Windows.Forms.CheckBox
	Private WithEvents _tabTask_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabTask As System.Windows.Forms.TabControl
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents uctPMResizer1 As PMResizerControl.uctPMResizer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTask))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.pnlEffectiveDate = New System.Windows.Forms.Panel
        Me.lblEffectiveDate1 = New System.Windows.Forms.Label
        Me.txtTaskName = New System.Windows.Forms.TextBox
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.chkIsSystemTask = New System.Windows.Forms.CheckBox
        Me.cboTypeOfTask = New System.Windows.Forms.ComboBox
        Me.txtComponentObjectName = New System.Windows.Forms.TextBox
        Me.txtComponentClassName = New System.Windows.Forms.TextBox
        Me.txtAutoDeleteAfterNumDays = New System.Windows.Forms.TextBox
        Me.cmdSelectIcon = New System.Windows.Forms.Button
        Me.txtLinkedObjectName = New System.Windows.Forms.TextBox
        Me.txtLinkedClassName = New System.Windows.Forms.TextBox
        Me.chkIsViewOnlyTask = New System.Windows.Forms.CheckBox
        Me.txtLinkedCaption = New System.Windows.Forms.TextBox
        Me.chkIsAvailableTask = New System.Windows.Forms.CheckBox
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabTask = New System.Windows.Forms.TabControl
        Me._tabTask_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblTaskName = New System.Windows.Forms.Label
        Me.lblEffectiveDate = New System.Windows.Forms.Label
        Me.lblDescription = New System.Windows.Forms.Label
        Me.lblTypeOfTask = New System.Windows.Forms.Label
        Me.lblPMNavProcessId = New System.Windows.Forms.Label
        Me.lblComponentClassName = New System.Windows.Forms.Label
        Me.lblAutoDeleteAfterNumDays = New System.Windows.Forms.Label
        Me.imgIcon = New System.Windows.Forms.PictureBox
        Me.lblLinkedClassName = New System.Windows.Forms.Label
        Me.lblLinkedCaption = New System.Windows.Forms.Label
        Me.lblIsSystemTask = New System.Windows.Forms.Label
        Me.lblIsViewOnlyTask = New System.Windows.Forms.Label
        Me.lblIsAvailableTask = New System.Windows.Forms.Label
        Me.lblComponentObjectName = New System.Windows.Forms.Label
        Me.lblLinkedObjectName = New System.Windows.Forms.Label
        Me.lblTaskGroupCategory = New System.Windows.Forms.Label
        Me.cboTaskCategory = New PMLookupControl.cboPMLookup
        Me.cboPMNavProcessId = New PMLookupControl.cboPMLookup
        Me.uctPMResizer1 = New PMResizerControl.uctPMResizer
        Me.pnlEffectiveDate.SuspendLayout()
        Me.tabTask.SuspendLayout()
        Me._tabTask_TabPage0.SuspendLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(536, 528)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 17
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdCancel, "Cancel changes and return to previous screen")
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'pnlEffectiveDate
        '
        Me.pnlEffectiveDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(184, Byte), Integer))
        Me.pnlEffectiveDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlEffectiveDate.Controls.Add(Me.lblEffectiveDate1)
        Me.pnlEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.26!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlEffectiveDate.Location = New System.Drawing.Point(400, 19)
        Me.pnlEffectiveDate.Name = "pnlEffectiveDate"
        Me.pnlEffectiveDate.Size = New System.Drawing.Size(169, 21)
        Me.pnlEffectiveDate.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.pnlEffectiveDate, "Date when current changes will be effective")
        '
        'lblEffectiveDate1
        '
        Me.lblEffectiveDate1.AutoSize = True
        Me.lblEffectiveDate1.Location = New System.Drawing.Point(3, 0)
        Me.lblEffectiveDate1.Name = "lblEffectiveDate1"
        Me.lblEffectiveDate1.Size = New System.Drawing.Size(0, 15)
        Me.lblEffectiveDate1.TabIndex = 0
        '
        'txtTaskName
        '
        Me.txtTaskName.AcceptsReturn = True
        Me.txtTaskName.BackColor = System.Drawing.SystemColors.Window
        Me.txtTaskName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTaskName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTaskName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTaskName.Location = New System.Drawing.Point(208, 20)
        Me.txtTaskName.MaxLength = 10
        Me.txtTaskName.Name = "txtTaskName"
        Me.txtTaskName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTaskName.Size = New System.Drawing.Size(81, 20)
        Me.txtTaskName.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.txtTaskName, "Task Name")
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(208, 54)
        Me.txtDescription.MaxLength = 255
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(361, 20)
        Me.txtDescription.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.txtDescription, "Description")
        '
        'chkIsSystemTask
        '
        Me.chkIsSystemTask.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsSystemTask.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsSystemTask.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsSystemTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsSystemTask.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsSystemTask.Location = New System.Drawing.Point(13, 85)
        Me.chkIsSystemTask.Name = "chkIsSystemTask"
        Me.chkIsSystemTask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsSystemTask.Size = New System.Drawing.Size(205, 24)
        Me.chkIsSystemTask.TabIndex = 3
        Me.chkIsSystemTask.Text = "Is This A System Task?"
        Me.ToolTip1.SetToolTip(Me.chkIsSystemTask, "Is This A System Task?")
        Me.chkIsSystemTask.UseVisualStyleBackColor = False
        '
        'cboTypeOfTask
        '
        Me.cboTypeOfTask.BackColor = System.Drawing.SystemColors.Window
        Me.cboTypeOfTask.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTypeOfTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTypeOfTask.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTypeOfTask.Location = New System.Drawing.Point(208, 120)
        Me.cboTypeOfTask.Name = "cboTypeOfTask"
        Me.cboTypeOfTask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTypeOfTask.Size = New System.Drawing.Size(217, 21)
        Me.cboTypeOfTask.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.cboTypeOfTask, "Enter the type of task")
        '
        'txtComponentObjectName
        '
        Me.txtComponentObjectName.AcceptsReturn = True
        Me.txtComponentObjectName.BackColor = System.Drawing.SystemColors.Window
        Me.txtComponentObjectName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtComponentObjectName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtComponentObjectName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtComponentObjectName.Location = New System.Drawing.Point(208, 187)
        Me.txtComponentObjectName.MaxLength = 30
        Me.txtComponentObjectName.Name = "txtComponentObjectName"
        Me.txtComponentObjectName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtComponentObjectName.Size = New System.Drawing.Size(217, 20)
        Me.txtComponentObjectName.TabIndex = 7
        Me.ToolTip1.SetToolTip(Me.txtComponentObjectName, "Enter the Object name of the component to run")
        '
        'txtComponentClassName
        '
        Me.txtComponentClassName.AcceptsReturn = True
        Me.txtComponentClassName.BackColor = System.Drawing.SystemColors.Window
        Me.txtComponentClassName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtComponentClassName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtComponentClassName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtComponentClassName.Location = New System.Drawing.Point(208, 220)
        Me.txtComponentClassName.MaxLength = 30
        Me.txtComponentClassName.Name = "txtComponentClassName"
        Me.txtComponentClassName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtComponentClassName.Size = New System.Drawing.Size(217, 20)
        Me.txtComponentClassName.TabIndex = 8
        Me.ToolTip1.SetToolTip(Me.txtComponentClassName, "Enter the class name of the component to run")
        '
        'txtAutoDeleteAfterNumDays
        '
        Me.txtAutoDeleteAfterNumDays.AcceptsReturn = True
        Me.txtAutoDeleteAfterNumDays.BackColor = System.Drawing.SystemColors.Window
        Me.txtAutoDeleteAfterNumDays.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAutoDeleteAfterNumDays.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAutoDeleteAfterNumDays.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAutoDeleteAfterNumDays.Location = New System.Drawing.Point(208, 254)
        Me.txtAutoDeleteAfterNumDays.MaxLength = 4
        Me.txtAutoDeleteAfterNumDays.Name = "txtAutoDeleteAfterNumDays"
        Me.txtAutoDeleteAfterNumDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAutoDeleteAfterNumDays.Size = New System.Drawing.Size(49, 20)
        Me.txtAutoDeleteAfterNumDays.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.txtAutoDeleteAfterNumDays, "After how many days should this be automatically deleted?")
        '
        'cmdSelectIcon
        '
        Me.cmdSelectIcon.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelectIcon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelectIcon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSelectIcon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelectIcon.Location = New System.Drawing.Point(496, 154)
        Me.cmdSelectIcon.Name = "cmdSelectIcon"
        Me.cmdSelectIcon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelectIcon.Size = New System.Drawing.Size(73, 22)
        Me.cmdSelectIcon.TabIndex = 6
        Me.cmdSelectIcon.Text = "Select Icon"
        Me.cmdSelectIcon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdSelectIcon, "Select an Icon")
        Me.cmdSelectIcon.UseVisualStyleBackColor = False
        '
        'txtLinkedObjectName
        '
        Me.txtLinkedObjectName.AcceptsReturn = True
        Me.txtLinkedObjectName.BackColor = System.Drawing.SystemColors.Window
        Me.txtLinkedObjectName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLinkedObjectName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLinkedObjectName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLinkedObjectName.Location = New System.Drawing.Point(208, 287)
        Me.txtLinkedObjectName.MaxLength = 30
        Me.txtLinkedObjectName.Name = "txtLinkedObjectName"
        Me.txtLinkedObjectName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLinkedObjectName.Size = New System.Drawing.Size(217, 20)
        Me.txtLinkedObjectName.TabIndex = 10
        Me.ToolTip1.SetToolTip(Me.txtLinkedObjectName, "Enter the name of the object to be linked with this task.")
        '
        'txtLinkedClassName
        '
        Me.txtLinkedClassName.AcceptsReturn = True
        Me.txtLinkedClassName.BackColor = System.Drawing.SystemColors.Window
        Me.txtLinkedClassName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLinkedClassName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLinkedClassName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLinkedClassName.Location = New System.Drawing.Point(208, 320)
        Me.txtLinkedClassName.MaxLength = 30
        Me.txtLinkedClassName.Name = "txtLinkedClassName"
        Me.txtLinkedClassName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLinkedClassName.Size = New System.Drawing.Size(217, 20)
        Me.txtLinkedClassName.TabIndex = 11
        Me.ToolTip1.SetToolTip(Me.txtLinkedClassName, "Enter the name of the object class to be linked to this task.")
        '
        'chkIsViewOnlyTask
        '
        Me.chkIsViewOnlyTask.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsViewOnlyTask.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsViewOnlyTask.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsViewOnlyTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsViewOnlyTask.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsViewOnlyTask.Location = New System.Drawing.Point(16, 386)
        Me.chkIsViewOnlyTask.Name = "chkIsViewOnlyTask"
        Me.chkIsViewOnlyTask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsViewOnlyTask.Size = New System.Drawing.Size(205, 23)
        Me.chkIsViewOnlyTask.TabIndex = 13
        Me.chkIsViewOnlyTask.Text = "Is This A View Only Task?"
        Me.ToolTip1.SetToolTip(Me.chkIsViewOnlyTask, "Is This A View Only Task?")
        Me.chkIsViewOnlyTask.UseVisualStyleBackColor = False
        '
        'txtLinkedCaption
        '
        Me.txtLinkedCaption.AcceptsReturn = True
        Me.txtLinkedCaption.BackColor = System.Drawing.SystemColors.Window
        Me.txtLinkedCaption.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLinkedCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLinkedCaption.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLinkedCaption.Location = New System.Drawing.Point(208, 354)
        Me.txtLinkedCaption.MaxLength = 30
        Me.txtLinkedCaption.Name = "txtLinkedCaption"
        Me.txtLinkedCaption.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLinkedCaption.Size = New System.Drawing.Size(217, 20)
        Me.txtLinkedCaption.TabIndex = 12
        Me.ToolTip1.SetToolTip(Me.txtLinkedCaption, "Enter the caption to be displayed on the link button when a task instance is crea" & _
                "ted.")
        '
        'chkIsAvailableTask
        '
        Me.chkIsAvailableTask.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsAvailableTask.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsAvailableTask.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsAvailableTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsAvailableTask.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsAvailableTask.Location = New System.Drawing.Point(16, 419)
        Me.chkIsAvailableTask.Name = "chkIsAvailableTask"
        Me.chkIsAvailableTask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsAvailableTask.Size = New System.Drawing.Size(205, 23)
        Me.chkIsAvailableTask.TabIndex = 14
        Me.chkIsAvailableTask.Text = "Is This Task Available To Be Run?"
        Me.ToolTip1.SetToolTip(Me.chkIsAvailableTask, "Can this task be run without additional information that is not obtained from the" & _
                " Linked object or class?")
        Me.chkIsAvailableTask.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(456, 528)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 16
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdOK, "Accept Changes and return to previous screen")
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabTask
        '
        Me.tabTask.Controls.Add(Me._tabTask_TabPage0)
        Me.tabTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabTask.ItemSize = New System.Drawing.Size(199, 18)
        Me.tabTask.Location = New System.Drawing.Point(8, 8)
        Me.tabTask.Multiline = True
        Me.tabTask.Name = "tabTask"
        Me.tabTask.SelectedIndex = 0
        Me.tabTask.Size = New System.Drawing.Size(605, 517)
        Me.tabTask.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.tabTask.TabIndex = 18
        Me.tabTask.TabStop = False
        '
        '_tabTask_TabPage0
        '
        Me._tabTask_TabPage0.Controls.Add(Me.lblTaskName)
        Me._tabTask_TabPage0.Controls.Add(Me.lblEffectiveDate)
        Me._tabTask_TabPage0.Controls.Add(Me.lblDescription)
        Me._tabTask_TabPage0.Controls.Add(Me.lblTypeOfTask)
        Me._tabTask_TabPage0.Controls.Add(Me.lblPMNavProcessId)
        Me._tabTask_TabPage0.Controls.Add(Me.lblComponentClassName)
        Me._tabTask_TabPage0.Controls.Add(Me.lblAutoDeleteAfterNumDays)
        Me._tabTask_TabPage0.Controls.Add(Me.imgIcon)
        Me._tabTask_TabPage0.Controls.Add(Me.lblLinkedClassName)
        Me._tabTask_TabPage0.Controls.Add(Me.lblLinkedCaption)
        Me._tabTask_TabPage0.Controls.Add(Me.lblIsSystemTask)
        Me._tabTask_TabPage0.Controls.Add(Me.lblIsViewOnlyTask)
        Me._tabTask_TabPage0.Controls.Add(Me.lblIsAvailableTask)
        Me._tabTask_TabPage0.Controls.Add(Me.lblComponentObjectName)
        Me._tabTask_TabPage0.Controls.Add(Me.lblLinkedObjectName)
        Me._tabTask_TabPage0.Controls.Add(Me.lblTaskGroupCategory)
        Me._tabTask_TabPage0.Controls.Add(Me.cboTaskCategory)
        Me._tabTask_TabPage0.Controls.Add(Me.pnlEffectiveDate)
        Me._tabTask_TabPage0.Controls.Add(Me.txtTaskName)
        Me._tabTask_TabPage0.Controls.Add(Me.txtDescription)
        Me._tabTask_TabPage0.Controls.Add(Me.chkIsSystemTask)
        Me._tabTask_TabPage0.Controls.Add(Me.cboTypeOfTask)
        Me._tabTask_TabPage0.Controls.Add(Me.txtComponentObjectName)
        Me._tabTask_TabPage0.Controls.Add(Me.txtComponentClassName)
        Me._tabTask_TabPage0.Controls.Add(Me.txtAutoDeleteAfterNumDays)
        Me._tabTask_TabPage0.Controls.Add(Me.cboPMNavProcessId)
        Me._tabTask_TabPage0.Controls.Add(Me.cmdSelectIcon)
        Me._tabTask_TabPage0.Controls.Add(Me.txtLinkedObjectName)
        Me._tabTask_TabPage0.Controls.Add(Me.txtLinkedClassName)
        Me._tabTask_TabPage0.Controls.Add(Me.chkIsViewOnlyTask)
        Me._tabTask_TabPage0.Controls.Add(Me.txtLinkedCaption)
        Me._tabTask_TabPage0.Controls.Add(Me.chkIsAvailableTask)
        Me._tabTask_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabTask_TabPage0.Name = "_tabTask_TabPage0"
        Me._tabTask_TabPage0.Size = New System.Drawing.Size(597, 491)
        Me._tabTask_TabPage0.TabIndex = 0
        Me._tabTask_TabPage0.Text = "&1 - Task Details"
        '
        'lblTaskName
        '
        Me.lblTaskName.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaskName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaskName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaskName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaskName.Location = New System.Drawing.Point(16, 23)
        Me.lblTaskName.Name = "lblTaskName"
        Me.lblTaskName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaskName.Size = New System.Drawing.Size(116, 13)
        Me.lblTaskName.TabIndex = 19
        Me.lblTaskName.Text = "Task Name:"
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(312, 23)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(76, 13)
        Me.lblEffectiveDate.TabIndex = 20
        Me.lblEffectiveDate.Text = "Effective date:"
        '
        'lblDescription
        '
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(16, 57)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(116, 13)
        Me.lblDescription.TabIndex = 21
        Me.lblDescription.Text = "Description"
        '
        'lblTypeOfTask
        '
        Me.lblTypeOfTask.AutoSize = True
        Me.lblTypeOfTask.BackColor = System.Drawing.SystemColors.Control
        Me.lblTypeOfTask.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTypeOfTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTypeOfTask.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTypeOfTask.Location = New System.Drawing.Point(16, 123)
        Me.lblTypeOfTask.Name = "lblTypeOfTask"
        Me.lblTypeOfTask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTypeOfTask.Size = New System.Drawing.Size(75, 13)
        Me.lblTypeOfTask.TabIndex = 22
        Me.lblTypeOfTask.Text = "Type Of Task:"
        '
        'lblPMNavProcessId
        '
        Me.lblPMNavProcessId.BackColor = System.Drawing.SystemColors.Control
        Me.lblPMNavProcessId.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPMNavProcessId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPMNavProcessId.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPMNavProcessId.Location = New System.Drawing.Point(16, 157)
        Me.lblPMNavProcessId.Name = "lblPMNavProcessId"
        Me.lblPMNavProcessId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPMNavProcessId.Size = New System.Drawing.Size(172, 13)
        Me.lblPMNavProcessId.TabIndex = 23
        Me.lblPMNavProcessId.Text = "Navigator Process Id:"
        '
        'lblComponentClassName
        '
        Me.lblComponentClassName.BackColor = System.Drawing.SystemColors.Control
        Me.lblComponentClassName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblComponentClassName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblComponentClassName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblComponentClassName.Location = New System.Drawing.Point(16, 223)
        Me.lblComponentClassName.Name = "lblComponentClassName"
        Me.lblComponentClassName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblComponentClassName.Size = New System.Drawing.Size(172, 13)
        Me.lblComponentClassName.TabIndex = 24
        Me.lblComponentClassName.Text = "Component Class Name:"
        '
        'lblAutoDeleteAfterNumDays
        '
        Me.lblAutoDeleteAfterNumDays.BackColor = System.Drawing.SystemColors.Control
        Me.lblAutoDeleteAfterNumDays.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAutoDeleteAfterNumDays.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAutoDeleteAfterNumDays.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAutoDeleteAfterNumDays.Location = New System.Drawing.Point(16, 257)
        Me.lblAutoDeleteAfterNumDays.Name = "lblAutoDeleteAfterNumDays"
        Me.lblAutoDeleteAfterNumDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAutoDeleteAfterNumDays.Size = New System.Drawing.Size(172, 13)
        Me.lblAutoDeleteAfterNumDays.TabIndex = 25
        Me.lblAutoDeleteAfterNumDays.Text = "Auto Delete After:"
        '
        'imgIcon
        '
        Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon.Image = CType(resources.GetObject("imgIcon.Image"), System.Drawing.Image)
        Me.imgIcon.Location = New System.Drawing.Point(512, 114)
        Me.imgIcon.Name = "imgIcon"
        Me.imgIcon.Size = New System.Drawing.Size(32, 32)
        Me.imgIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.imgIcon.TabIndex = 26
        Me.imgIcon.TabStop = False
        '
        'lblLinkedClassName
        '
        Me.lblLinkedClassName.BackColor = System.Drawing.SystemColors.Control
        Me.lblLinkedClassName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLinkedClassName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLinkedClassName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLinkedClassName.Location = New System.Drawing.Point(16, 323)
        Me.lblLinkedClassName.Name = "lblLinkedClassName"
        Me.lblLinkedClassName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLinkedClassName.Size = New System.Drawing.Size(172, 13)
        Me.lblLinkedClassName.TabIndex = 26
        Me.lblLinkedClassName.Text = "Linked Class Name:"
        '
        'lblLinkedCaption
        '
        Me.lblLinkedCaption.BackColor = System.Drawing.SystemColors.Control
        Me.lblLinkedCaption.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLinkedCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLinkedCaption.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLinkedCaption.Location = New System.Drawing.Point(16, 357)
        Me.lblLinkedCaption.Name = "lblLinkedCaption"
        Me.lblLinkedCaption.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLinkedCaption.Size = New System.Drawing.Size(172, 13)
        Me.lblLinkedCaption.TabIndex = 27
        Me.lblLinkedCaption.Text = "Linked Caption:"
        '
        'lblIsSystemTask
        '
        Me.lblIsSystemTask.BackColor = System.Drawing.SystemColors.Control
        Me.lblIsSystemTask.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIsSystemTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIsSystemTask.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIsSystemTask.Location = New System.Drawing.Point(224, 90)
        Me.lblIsSystemTask.Name = "lblIsSystemTask"
        Me.lblIsSystemTask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIsSystemTask.Size = New System.Drawing.Size(33, 13)
        Me.lblIsSystemTask.TabIndex = 28
        Me.lblIsSystemTask.Text = "No"
        '
        'lblIsViewOnlyTask
        '
        Me.lblIsViewOnlyTask.BackColor = System.Drawing.SystemColors.Control
        Me.lblIsViewOnlyTask.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIsViewOnlyTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIsViewOnlyTask.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIsViewOnlyTask.Location = New System.Drawing.Point(224, 390)
        Me.lblIsViewOnlyTask.Name = "lblIsViewOnlyTask"
        Me.lblIsViewOnlyTask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIsViewOnlyTask.Size = New System.Drawing.Size(33, 13)
        Me.lblIsViewOnlyTask.TabIndex = 29
        Me.lblIsViewOnlyTask.Text = "No"
        '
        'lblIsAvailableTask
        '
        Me.lblIsAvailableTask.BackColor = System.Drawing.SystemColors.Control
        Me.lblIsAvailableTask.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIsAvailableTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIsAvailableTask.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIsAvailableTask.Location = New System.Drawing.Point(224, 423)
        Me.lblIsAvailableTask.Name = "lblIsAvailableTask"
        Me.lblIsAvailableTask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIsAvailableTask.Size = New System.Drawing.Size(33, 13)
        Me.lblIsAvailableTask.TabIndex = 30
        Me.lblIsAvailableTask.Text = "No"
        '
        'lblComponentObjectName
        '
        Me.lblComponentObjectName.BackColor = System.Drawing.SystemColors.Control
        Me.lblComponentObjectName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblComponentObjectName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblComponentObjectName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblComponentObjectName.Location = New System.Drawing.Point(16, 190)
        Me.lblComponentObjectName.Name = "lblComponentObjectName"
        Me.lblComponentObjectName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblComponentObjectName.Size = New System.Drawing.Size(172, 13)
        Me.lblComponentObjectName.TabIndex = 31
        Me.lblComponentObjectName.Text = "Component Object Name:"
        '
        'lblLinkedObjectName
        '
        Me.lblLinkedObjectName.BackColor = System.Drawing.SystemColors.Control
        Me.lblLinkedObjectName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLinkedObjectName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLinkedObjectName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLinkedObjectName.Location = New System.Drawing.Point(16, 290)
        Me.lblLinkedObjectName.Name = "lblLinkedObjectName"
        Me.lblLinkedObjectName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLinkedObjectName.Size = New System.Drawing.Size(172, 13)
        Me.lblLinkedObjectName.TabIndex = 32
        Me.lblLinkedObjectName.Text = "Linked Object Name:"
        '
        'lblTaskGroupCategory
        '
        Me.lblTaskGroupCategory.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaskGroupCategory.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaskGroupCategory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaskGroupCategory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaskGroupCategory.Location = New System.Drawing.Point(16, 457)
        Me.lblTaskGroupCategory.Name = "lblTaskGroupCategory"
        Me.lblTaskGroupCategory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaskGroupCategory.Size = New System.Drawing.Size(121, 13)
        Me.lblTaskGroupCategory.TabIndex = 33
        Me.lblTaskGroupCategory.Text = "Task Group Category:"
        '
        'cboTaskCategory
        '
        Me.cboTaskCategory.DefaultItemId = 0
        Me.cboTaskCategory.FirstItem = ""
        Me.cboTaskCategory.ItemId = 0
        Me.cboTaskCategory.ListIndex = -1
        Me.cboTaskCategory.Location = New System.Drawing.Point(208, 452)
        Me.cboTaskCategory.Name = "cboTaskCategory"
        Me.cboTaskCategory.PMLookupProductFamily = 1
        Me.cboTaskCategory.SingleItemId = 0
        Me.cboTaskCategory.Size = New System.Drawing.Size(217, 21)
        Me.cboTaskCategory.Sorted = True
        Me.cboTaskCategory.TabIndex = 15
        Me.cboTaskCategory.TableName = "PMWrk_Task_Category"
        Me.cboTaskCategory.ToolTipText = ""
        Me.cboTaskCategory.WhereClause = ""
        '
        'cboPMNavProcessId
        '
        Me.cboPMNavProcessId.DefaultItemId = 0
        Me.cboPMNavProcessId.FirstItem = "(None)"
        Me.cboPMNavProcessId.ItemId = 0
        Me.cboPMNavProcessId.ListIndex = -1
        Me.cboPMNavProcessId.Location = New System.Drawing.Point(208, 154)
        Me.cboPMNavProcessId.Name = "cboPMNavProcessId"
        Me.cboPMNavProcessId.PMLookupProductFamily = 1
        Me.cboPMNavProcessId.SingleItemId = 0
        Me.cboPMNavProcessId.Size = New System.Drawing.Size(217, 21)
        Me.cboPMNavProcessId.Sorted = True
        Me.cboPMNavProcessId.TabIndex = 5
        Me.cboPMNavProcessId.TableName = "PMNav_Process"
        Me.cboPMNavProcessId.ToolTipText = "Enter the navigator process id"
        Me.cboPMNavProcessId.WhereClause = ""
        '
        'uctPMResizer1
        '
        Me.uctPMResizer1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMResizer1.Location = New System.Drawing.Point(8, 520)
        Me.uctPMResizer1.Name = "uctPMResizer1"
        Me.uctPMResizer1.Size = New System.Drawing.Size(32, 30)
        Me.uctPMResizer1.TabIndex = 19
        '
        'frmTask
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(619, 553)
        Me.ControlBox = False
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.tabTask)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.uctPMResizer1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(270, 147)
        Me.MinimizeBox = False
        Me.Name = "frmTask"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Edit Task"
        Me.pnlEffectiveDate.ResumeLayout(False)
        Me.pnlEffectiveDate.PerformLayout()
        Me.tabTask.ResumeLayout(False)
        Me._tabTask_TabPage0.ResumeLayout(False)
        Me._tabTask_TabPage0.PerformLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblEffectiveDate1 As System.Windows.Forms.Label
#End Region 
End Class