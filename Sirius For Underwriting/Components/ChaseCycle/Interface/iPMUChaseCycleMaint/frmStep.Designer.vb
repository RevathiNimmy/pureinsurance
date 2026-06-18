<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStep
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
    Public WithEvents chkCheckAutoCancel As System.Windows.Forms.CheckBox
    Public WithEvents chkRunAutoCancel As System.Windows.Forms.CheckBox
    Public WithEvents chkAutoLapseRenewal As System.Windows.Forms.CheckBox
    Public WithEvents fraAutoCancellationActions As System.Windows.Forms.GroupBox
    Public WithEvents cboClientLetter As System.Windows.Forms.ComboBox
    Public WithEvents txtElapsedDays As System.Windows.Forms.TextBox
    Public WithEvents lblClientLetter As System.Windows.Forms.Label
    Public WithEvents lblElapsedDays As System.Windows.Forms.Label
    Public WithEvents fraDirectCustomers As System.Windows.Forms.GroupBox
    Public WithEvents txtStepDescription As System.Windows.Forms.TextBox
    Public WithEvents cboUserGroup As System.Windows.Forms.ComboBox
    Public WithEvents cboTask As System.Windows.Forms.ComboBox
    Public WithEvents cboTaskGroup As System.Windows.Forms.ComboBox
    Public WithEvents cboPMLookupUserGroup As PMLookupControl.cboPMLookup
    Public WithEvents cboPMLookupWrkTask As PMLookupControl.cboPMLookup
    Public WithEvents cboPMLookupActionType As PMLookupControl.cboPMLookup
    Public WithEvents lblDescription As System.Windows.Forms.Label
    Public WithEvents lblAllocateToUserGroup As System.Windows.Forms.Label
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents lblTaskGroup As System.Windows.Forms.Label
    Public WithEvents lblActiontype As System.Windows.Forms.Label
    Public WithEvents lblUserGroup As System.Windows.Forms.Label
    Public WithEvents lblTask As System.Windows.Forms.Label
    Public WithEvents fraOther As System.Windows.Forms.GroupBox
    Public WithEvents txtPreviousStep As System.Windows.Forms.TextBox
    Public WithEvents txtNextStep As System.Windows.Forms.TextBox
    Public WithEvents txtStep As System.Windows.Forms.TextBox
    Public WithEvents lblPreviousStep As System.Windows.Forms.Label
    Public WithEvents lblNextStep As System.Windows.Forms.Label
    Public WithEvents lblStep As System.Windows.Forms.Label
    Public WithEvents fraStepOrder As System.Windows.Forms.GroupBox
    Public WithEvents txtHelper As System.Windows.Forms.TextBox
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.fraAutoCancellationActions = New System.Windows.Forms.GroupBox
        Me.chkCheckAutoCancel = New System.Windows.Forms.CheckBox
        Me.chkRunAutoCancel = New System.Windows.Forms.CheckBox
        Me.chkAutoLapseRenewal = New System.Windows.Forms.CheckBox
        Me.fraDirectCustomers = New System.Windows.Forms.GroupBox
        Me.cboClientLetter = New System.Windows.Forms.ComboBox
        Me.txtElapsedDays = New System.Windows.Forms.TextBox
        Me.lblClientLetter = New System.Windows.Forms.Label
        Me.lblElapsedDays = New System.Windows.Forms.Label
        Me.fraOther = New System.Windows.Forms.GroupBox
        Me.txtStepDescription = New System.Windows.Forms.TextBox
        Me.cboUserGroup = New System.Windows.Forms.ComboBox
        Me.cboTask = New System.Windows.Forms.ComboBox
        Me.cboTaskGroup = New System.Windows.Forms.ComboBox
        Me.cboPMLookupUserGroup = New PMLookupControl.cboPMLookup
        Me.cboPMLookupWrkTask = New PMLookupControl.cboPMLookup
        Me.cboPMLookupActionType = New PMLookupControl.cboPMLookup
        Me.lblDescription = New System.Windows.Forms.Label
        Me.lblAllocateToUserGroup = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.lblTaskGroup = New System.Windows.Forms.Label
        Me.lblActiontype = New System.Windows.Forms.Label
        Me.lblUserGroup = New System.Windows.Forms.Label
        Me.lblTask = New System.Windows.Forms.Label
        Me.fraStepOrder = New System.Windows.Forms.GroupBox
        Me.txtPreviousStep = New System.Windows.Forms.TextBox
        Me.txtNextStep = New System.Windows.Forms.TextBox
        Me.txtStep = New System.Windows.Forms.TextBox
        Me.lblPreviousStep = New System.Windows.Forms.Label
        Me.lblNextStep = New System.Windows.Forms.Label
        Me.lblStep = New System.Windows.Forms.Label
        Me.txtHelper = New System.Windows.Forms.TextBox
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.fraAutoCancellationActions.SuspendLayout()
        Me.fraDirectCustomers.SuspendLayout()
        Me.fraOther.SuspendLayout()
        Me.fraStepOrder.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'fraAutoCancellationActions
        '
        Me.fraAutoCancellationActions.BackColor = System.Drawing.SystemColors.Control
        Me.fraAutoCancellationActions.Controls.Add(Me.chkCheckAutoCancel)
        Me.fraAutoCancellationActions.Controls.Add(Me.chkRunAutoCancel)
        Me.fraAutoCancellationActions.Controls.Add(Me.chkAutoLapseRenewal)
        Me.fraAutoCancellationActions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAutoCancellationActions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAutoCancellationActions.Location = New System.Drawing.Point(12, 128)
        Me.fraAutoCancellationActions.Name = "fraAutoCancellationActions"
        Me.fraAutoCancellationActions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAutoCancellationActions.Size = New System.Drawing.Size(649, 53)
        Me.fraAutoCancellationActions.TabIndex = 29
        Me.fraAutoCancellationActions.TabStop = False
        Me.fraAutoCancellationActions.Text = "Auto Cancellation"
        '
        'chkCheckAutoCancel
        '
        Me.chkCheckAutoCancel.BackColor = System.Drawing.SystemColors.Control
        Me.chkCheckAutoCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCheckAutoCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCheckAutoCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCheckAutoCancel.Location = New System.Drawing.Point(8, 16)
        Me.chkCheckAutoCancel.Name = "chkCheckAutoCancel"
        Me.chkCheckAutoCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCheckAutoCancel.Size = New System.Drawing.Size(197, 29)
        Me.chkCheckAutoCancel.TabIndex = 30
        Me.chkCheckAutoCancel.Tag = "CAP;522"
        Me.chkCheckAutoCancel.Text = "*{Check Auto-Cancel Rules}"
        Me.chkCheckAutoCancel.UseVisualStyleBackColor = False
        '
        'chkRunAutoCancel
        '
        Me.chkRunAutoCancel.BackColor = System.Drawing.SystemColors.Control
        Me.chkRunAutoCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRunAutoCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRunAutoCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRunAutoCancel.Location = New System.Drawing.Point(241, 16)
        Me.chkRunAutoCancel.Name = "chkRunAutoCancel"
        Me.chkRunAutoCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRunAutoCancel.Size = New System.Drawing.Size(183, 29)
        Me.chkRunAutoCancel.TabIndex = 31
        Me.chkRunAutoCancel.Tag = "CAP;523"
        Me.chkRunAutoCancel.Text = "*{Run Auto-Cancel Rules}"
        Me.chkRunAutoCancel.UseVisualStyleBackColor = False
        '
        'chkAutoLapseRenewal
        '
        Me.chkAutoLapseRenewal.BackColor = System.Drawing.SystemColors.Control
        Me.chkAutoLapseRenewal.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAutoLapseRenewal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAutoLapseRenewal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAutoLapseRenewal.Location = New System.Drawing.Point(24, 16)
        Me.chkAutoLapseRenewal.Name = "chkAutoLapseRenewal"
        Me.chkAutoLapseRenewal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAutoLapseRenewal.Size = New System.Drawing.Size(197, 29)
        Me.chkAutoLapseRenewal.TabIndex = 89
        Me.chkAutoLapseRenewal.Tag = "CAP;730"
        Me.chkAutoLapseRenewal.Text = "*{Auto Lapse Renewal}"
        Me.chkAutoLapseRenewal.UseVisualStyleBackColor = False
        '
        'fraDirectCustomers
        '
        Me.fraDirectCustomers.BackColor = System.Drawing.SystemColors.Control
        Me.fraDirectCustomers.Controls.Add(Me.cboClientLetter)
        Me.fraDirectCustomers.Controls.Add(Me.txtElapsedDays)
        Me.fraDirectCustomers.Controls.Add(Me.lblClientLetter)
        Me.fraDirectCustomers.Controls.Add(Me.lblElapsedDays)
        Me.fraDirectCustomers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDirectCustomers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDirectCustomers.Location = New System.Drawing.Point(8, 60)
        Me.fraDirectCustomers.Name = "fraDirectCustomers"
        Me.fraDirectCustomers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDirectCustomers.Size = New System.Drawing.Size(649, 60)
        Me.fraDirectCustomers.TabIndex = 13
        Me.fraDirectCustomers.TabStop = False
        Me.fraDirectCustomers.Text = "Step Action"
        '
        'cboClientLetter
        '
        Me.cboClientLetter.BackColor = System.Drawing.SystemColors.Window
        Me.cboClientLetter.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboClientLetter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboClientLetter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboClientLetter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboClientLetter.Location = New System.Drawing.Point(304, 18)
        Me.cboClientLetter.Name = "cboClientLetter"
        Me.cboClientLetter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboClientLetter.Size = New System.Drawing.Size(329, 21)
        Me.cboClientLetter.TabIndex = 15
        '
        'txtElapsedDays
        '
        Me.txtElapsedDays.AcceptsReturn = True
        Me.txtElapsedDays.BackColor = System.Drawing.SystemColors.Window
        Me.txtElapsedDays.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtElapsedDays.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtElapsedDays.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtElapsedDays.Location = New System.Drawing.Point(126, 18)
        Me.txtElapsedDays.MaxLength = 2
        Me.txtElapsedDays.Name = "txtElapsedDays"
        Me.txtElapsedDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtElapsedDays.Size = New System.Drawing.Size(65, 20)
        Me.txtElapsedDays.TabIndex = 18
        '
        'lblClientLetter
        '
        Me.lblClientLetter.AutoSize = True
        Me.lblClientLetter.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientLetter.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientLetter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientLetter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientLetter.Location = New System.Drawing.Point(202, 21)
        Me.lblClientLetter.Name = "lblClientLetter"
        Me.lblClientLetter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientLetter.Size = New System.Drawing.Size(114, 13)
        Me.lblClientLetter.TabIndex = 14
        Me.lblClientLetter.Tag = "CAP;518"
        Me.lblClientLetter.Text = "*{Client/Broker Letter:}"
        '
        'lblElapsedDays
        '
        Me.lblElapsedDays.AutoSize = True
        Me.lblElapsedDays.BackColor = System.Drawing.SystemColors.Control
        Me.lblElapsedDays.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblElapsedDays.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblElapsedDays.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblElapsedDays.Location = New System.Drawing.Point(18, 21)
        Me.lblElapsedDays.Name = "lblElapsedDays"
        Me.lblElapsedDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblElapsedDays.Size = New System.Drawing.Size(87, 13)
        Me.lblElapsedDays.TabIndex = 17
        Me.lblElapsedDays.Tag = "CAP;509"
        Me.lblElapsedDays.Text = "*{Elapsed Days:}"
        '
        'fraOther
        '
        Me.fraOther.BackColor = System.Drawing.SystemColors.Control
        Me.fraOther.Controls.Add(Me.txtStepDescription)
        Me.fraOther.Controls.Add(Me.cboUserGroup)
        Me.fraOther.Controls.Add(Me.cboTask)
        Me.fraOther.Controls.Add(Me.cboTaskGroup)
        Me.fraOther.Controls.Add(Me.cboPMLookupUserGroup)
        Me.fraOther.Controls.Add(Me.cboPMLookupWrkTask)
        Me.fraOther.Controls.Add(Me.cboPMLookupActionType)
        Me.fraOther.Controls.Add(Me.lblDescription)
        Me.fraOther.Controls.Add(Me.lblAllocateToUserGroup)
        Me.fraOther.Controls.Add(Me.Label3)
        Me.fraOther.Controls.Add(Me.lblTaskGroup)
        Me.fraOther.Controls.Add(Me.lblActiontype)
        Me.fraOther.Controls.Add(Me.lblUserGroup)
        Me.fraOther.Controls.Add(Me.lblTask)
        Me.fraOther.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraOther.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraOther.Location = New System.Drawing.Point(12, 190)
        Me.fraOther.Name = "fraOther"
        Me.fraOther.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraOther.Size = New System.Drawing.Size(649, 114)
        Me.fraOther.TabIndex = 45
        Me.fraOther.TabStop = False
        Me.fraOther.Tag = "CAP;521"
        Me.fraOther.Text = "Create Task"
        '
        'txtStepDescription
        '
        Me.txtStepDescription.AcceptsReturn = True
        Me.txtStepDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtStepDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStepDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStepDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStepDescription.Location = New System.Drawing.Point(96, 78)
        Me.txtStepDescription.MaxLength = 255
        Me.txtStepDescription.Name = "txtStepDescription"
        Me.txtStepDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStepDescription.Size = New System.Drawing.Size(537, 20)
        Me.txtStepDescription.TabIndex = 53
        '
        'cboUserGroup
        '
        Me.cboUserGroup.BackColor = System.Drawing.SystemColors.Window
        Me.cboUserGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboUserGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboUserGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboUserGroup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboUserGroup.Location = New System.Drawing.Point(96, 47)
        Me.cboUserGroup.Name = "cboUserGroup"
        Me.cboUserGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboUserGroup.Size = New System.Drawing.Size(241, 21)
        Me.cboUserGroup.TabIndex = 51
        '
        'cboTask
        '
        Me.cboTask.BackColor = System.Drawing.SystemColors.Window
        Me.cboTask.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTask.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTask.Enabled = False
        Me.cboTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTask.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTask.Location = New System.Drawing.Point(392, 16)
        Me.cboTask.Name = "cboTask"
        Me.cboTask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTask.Size = New System.Drawing.Size(241, 21)
        Me.cboTask.TabIndex = 49
        '
        'cboTaskGroup
        '
        Me.cboTaskGroup.BackColor = System.Drawing.SystemColors.Window
        Me.cboTaskGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTaskGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTaskGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTaskGroup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTaskGroup.Location = New System.Drawing.Point(96, 16)
        Me.cboTaskGroup.Name = "cboTaskGroup"
        Me.cboTaskGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTaskGroup.Size = New System.Drawing.Size(241, 21)
        Me.cboTaskGroup.TabIndex = 47
        '
        'cboPMLookupUserGroup
        '
        Me.cboPMLookupUserGroup.DefaultItemId = 0
        Me.cboPMLookupUserGroup.FirstItem = "("""")"
        Me.cboPMLookupUserGroup.ItemId = 0
        Me.cboPMLookupUserGroup.ListIndex = -1
        Me.cboPMLookupUserGroup.Location = New System.Drawing.Point(160, 160)
        Me.cboPMLookupUserGroup.Name = "cboPMLookupUserGroup"
        Me.cboPMLookupUserGroup.PMLookupProductFamily = 1
        Me.cboPMLookupUserGroup.SingleItemId = 0
        Me.cboPMLookupUserGroup.Size = New System.Drawing.Size(153, 21)
        Me.cboPMLookupUserGroup.Sorted = True
        Me.cboPMLookupUserGroup.TabIndex = 85
        Me.cboPMLookupUserGroup.TableName = "PMUser_Group"
        Me.cboPMLookupUserGroup.TabStop = False
        Me.cboPMLookupUserGroup.ToolTipText = ""
        Me.cboPMLookupUserGroup.WhereClause = ""
        '
        'cboPMLookupWrkTask
        '
        Me.cboPMLookupWrkTask.DefaultItemId = 0
        Me.cboPMLookupWrkTask.FirstItem = "("""")"
        Me.cboPMLookupWrkTask.ItemId = 0
        Me.cboPMLookupWrkTask.ListIndex = -1
        Me.cboPMLookupWrkTask.Location = New System.Drawing.Point(160, 160)
        Me.cboPMLookupWrkTask.Name = "cboPMLookupWrkTask"
        Me.cboPMLookupWrkTask.PMLookupProductFamily = 2
        Me.cboPMLookupWrkTask.SingleItemId = 0
        Me.cboPMLookupWrkTask.Size = New System.Drawing.Size(153, 21)
        Me.cboPMLookupWrkTask.Sorted = True
        Me.cboPMLookupWrkTask.TabIndex = 86
        Me.cboPMLookupWrkTask.TableName = "PMWrk_Task"
        Me.cboPMLookupWrkTask.TabStop = False
        Me.cboPMLookupWrkTask.ToolTipText = ""
        Me.cboPMLookupWrkTask.WhereClause = ""
        '
        'cboPMLookupActionType
        '
        Me.cboPMLookupActionType.DefaultItemId = 0
        Me.cboPMLookupActionType.FirstItem = "("""")"
        Me.cboPMLookupActionType.ItemId = 0
        Me.cboPMLookupActionType.ListIndex = -1
        Me.cboPMLookupActionType.Location = New System.Drawing.Point(160, 160)
        Me.cboPMLookupActionType.Name = "cboPMLookupActionType"
        Me.cboPMLookupActionType.PMLookupProductFamily = 1
        Me.cboPMLookupActionType.SingleItemId = 0
        Me.cboPMLookupActionType.Size = New System.Drawing.Size(153, 21)
        Me.cboPMLookupActionType.Sorted = True
        Me.cboPMLookupActionType.TabIndex = 84
        Me.cboPMLookupActionType.TableName = "PMWrk_Task_Action_type"
        Me.cboPMLookupActionType.TabStop = False
        Me.cboPMLookupActionType.ToolTipText = ""
        Me.cboPMLookupActionType.WhereClause = ""
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(17, 80)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(63, 13)
        Me.lblDescription.TabIndex = 52
        Me.lblDescription.Text = "Description:"
        '
        'lblAllocateToUserGroup
        '
        Me.lblAllocateToUserGroup.AutoSize = True
        Me.lblAllocateToUserGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblAllocateToUserGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAllocateToUserGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAllocateToUserGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAllocateToUserGroup.Location = New System.Drawing.Point(16, 51)
        Me.lblAllocateToUserGroup.Name = "lblAllocateToUserGroup"
        Me.lblAllocateToUserGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAllocateToUserGroup.Size = New System.Drawing.Size(64, 13)
        Me.lblAllocateToUserGroup.TabIndex = 50
        Me.lblAllocateToUserGroup.Text = "User Group:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(352, 20)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(34, 13)
        Me.Label3.TabIndex = 48
        Me.Label3.Text = "Task:"
        '
        'lblTaskGroup
        '
        Me.lblTaskGroup.AutoSize = True
        Me.lblTaskGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaskGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaskGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaskGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaskGroup.Location = New System.Drawing.Point(16, 20)
        Me.lblTaskGroup.Name = "lblTaskGroup"
        Me.lblTaskGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaskGroup.Size = New System.Drawing.Size(66, 13)
        Me.lblTaskGroup.TabIndex = 46
        Me.lblTaskGroup.Text = "Task Group:"
        '
        'lblActiontype
        '
        Me.lblActiontype.AutoSize = True
        Me.lblActiontype.BackColor = System.Drawing.SystemColors.Control
        Me.lblActiontype.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblActiontype.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblActiontype.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblActiontype.Location = New System.Drawing.Point(8, 160)
        Me.lblActiontype.Name = "lblActiontype"
        Me.lblActiontype.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblActiontype.Size = New System.Drawing.Size(79, 13)
        Me.lblActiontype.TabIndex = 83
        Me.lblActiontype.Tag = "CAP;525"
        Me.lblActiontype.Text = "*{Action Type:}"
        '
        'lblUserGroup
        '
        Me.lblUserGroup.AutoSize = True
        Me.lblUserGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblUserGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUserGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUserGroup.Location = New System.Drawing.Point(8, 160)
        Me.lblUserGroup.Name = "lblUserGroup"
        Me.lblUserGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUserGroup.Size = New System.Drawing.Size(94, 13)
        Me.lblUserGroup.TabIndex = 82
        Me.lblUserGroup.Tag = "CAP;525"
        Me.lblUserGroup.Text = "*{For User Group:}"
        '
        'lblTask
        '
        Me.lblTask.AutoSize = True
        Me.lblTask.BackColor = System.Drawing.SystemColors.Control
        Me.lblTask.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTask.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTask.Location = New System.Drawing.Point(8, 160)
        Me.lblTask.Name = "lblTask"
        Me.lblTask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTask.Size = New System.Drawing.Size(153, 13)
        Me.lblTask.TabIndex = 87
        Me.lblTask.Tag = "CAP;524"
        Me.lblTask.Text = "*{Create Work manager Task:}"
        '
        'fraStepOrder
        '
        Me.fraStepOrder.BackColor = System.Drawing.SystemColors.Control
        Me.fraStepOrder.Controls.Add(Me.txtPreviousStep)
        Me.fraStepOrder.Controls.Add(Me.txtNextStep)
        Me.fraStepOrder.Controls.Add(Me.txtStep)
        Me.fraStepOrder.Controls.Add(Me.lblPreviousStep)
        Me.fraStepOrder.Controls.Add(Me.lblNextStep)
        Me.fraStepOrder.Controls.Add(Me.lblStep)
        Me.fraStepOrder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraStepOrder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraStepOrder.Location = New System.Drawing.Point(8, 6)
        Me.fraStepOrder.Name = "fraStepOrder"
        Me.fraStepOrder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraStepOrder.Size = New System.Drawing.Size(649, 48)
        Me.fraStepOrder.TabIndex = 1
        Me.fraStepOrder.TabStop = False
        Me.fraStepOrder.Tag = "CAP;503"
        Me.fraStepOrder.Text = "*{Step Order}"
        '
        'txtPreviousStep
        '
        Me.txtPreviousStep.AcceptsReturn = True
        Me.txtPreviousStep.BackColor = System.Drawing.SystemColors.Window
        Me.txtPreviousStep.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPreviousStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPreviousStep.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPreviousStep.Location = New System.Drawing.Point(368, 20)
        Me.txtPreviousStep.MaxLength = 2
        Me.txtPreviousStep.Name = "txtPreviousStep"
        Me.txtPreviousStep.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPreviousStep.Size = New System.Drawing.Size(41, 20)
        Me.txtPreviousStep.TabIndex = 7
        '
        'txtNextStep
        '
        Me.txtNextStep.AcceptsReturn = True
        Me.txtNextStep.BackColor = System.Drawing.SystemColors.Window
        Me.txtNextStep.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNextStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNextStep.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNextStep.Location = New System.Drawing.Point(216, 20)
        Me.txtNextStep.MaxLength = 2
        Me.txtNextStep.Name = "txtNextStep"
        Me.txtNextStep.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNextStep.Size = New System.Drawing.Size(41, 20)
        Me.txtNextStep.TabIndex = 5
        '
        'txtStep
        '
        Me.txtStep.AcceptsReturn = True
        Me.txtStep.BackColor = System.Drawing.SystemColors.Window
        Me.txtStep.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStep.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStep.Location = New System.Drawing.Point(72, 20)
        Me.txtStep.MaxLength = 2
        Me.txtStep.Name = "txtStep"
        Me.txtStep.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStep.Size = New System.Drawing.Size(41, 20)
        Me.txtStep.TabIndex = 3
        Me.txtStep.Tag = "F;M;"
        '
        'lblPreviousStep
        '
        Me.lblPreviousStep.AutoSize = True
        Me.lblPreviousStep.BackColor = System.Drawing.SystemColors.Control
        Me.lblPreviousStep.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPreviousStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPreviousStep.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPreviousStep.Location = New System.Drawing.Point(256, 21)
        Me.lblPreviousStep.Name = "lblPreviousStep"
        Me.lblPreviousStep.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPreviousStep.Size = New System.Drawing.Size(88, 13)
        Me.lblPreviousStep.TabIndex = 6
        Me.lblPreviousStep.Tag = "CAP;506"
        Me.lblPreviousStep.Text = "*{Previous Step:}"
        Me.lblPreviousStep.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblNextStep
        '
        Me.lblNextStep.AutoSize = True
        Me.lblNextStep.BackColor = System.Drawing.SystemColors.Control
        Me.lblNextStep.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNextStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNextStep.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNextStep.Location = New System.Drawing.Point(127, 21)
        Me.lblNextStep.Name = "lblNextStep"
        Me.lblNextStep.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNextStep.Size = New System.Drawing.Size(69, 13)
        Me.lblNextStep.TabIndex = 4
        Me.lblNextStep.Tag = "CAP;505"
        Me.lblNextStep.Text = "*{Next Step:}"
        Me.lblNextStep.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblStep
        '
        Me.lblStep.AutoSize = True
        Me.lblStep.BackColor = System.Drawing.SystemColors.Control
        Me.lblStep.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStep.Location = New System.Drawing.Point(13, 21)
        Me.lblStep.Name = "lblStep"
        Me.lblStep.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStep.Size = New System.Drawing.Size(52, 13)
        Me.lblStep.TabIndex = 2
        Me.lblStep.Tag = "CAP;504"
        Me.lblStep.Text = "*{Step:}"
        Me.lblStep.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtHelper
        '
        Me.txtHelper.AcceptsReturn = True
        Me.txtHelper.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtHelper.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtHelper.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHelper.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHelper.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtHelper.Location = New System.Drawing.Point(108, 308)
        Me.txtHelper.MaxLength = 0
        Me.txtHelper.Multiline = True
        Me.txtHelper.Name = "txtHelper"
        Me.txtHelper.ReadOnly = True
        Me.txtHelper.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHelper.Size = New System.Drawing.Size(125, 29)
        Me.txtHelper.TabIndex = 88
        Me.txtHelper.Visible = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(504, 310)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 24)
        Me.cmdOK.TabIndex = 58
        Me.cmdOK.Tag = "CAP;209"
        Me.cmdOK.Text = "*{&OK}"
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
        Me.cmdCancel.Location = New System.Drawing.Point(584, 310)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 24)
        Me.cmdCancel.TabIndex = 59
        Me.cmdCancel.Tag = "CAP;210"
        Me.cmdCancel.Text = "*{&Cancel}"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'frmStep
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(664, 342)
        Me.ControlBox = False
        Me.Controls.Add(Me.fraAutoCancellationActions)
        Me.Controls.Add(Me.fraDirectCustomers)
        Me.Controls.Add(Me.fraOther)
        Me.Controls.Add(Me.fraStepOrder)
        Me.Controls.Add(Me.txtHelper)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmStep"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "*{Add/Edit Step}"
        Me.fraAutoCancellationActions.ResumeLayout(False)
        Me.fraDirectCustomers.ResumeLayout(False)
        Me.fraDirectCustomers.PerformLayout()
        Me.fraOther.ResumeLayout(False)
        Me.fraOther.PerformLayout()
        Me.fraStepOrder.ResumeLayout(False)
        Me.fraStepOrder.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class