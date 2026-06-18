<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		lvwSearchResults_InitializeColumnKeys()
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
    Public WithEvents cboLastUpdate As System.Windows.Forms.DateTimePicker
    Public WithEvents cboCoverNoteBookStatus As System.Windows.Forms.ComboBox
    Public WithEvents cboBranch As System.Windows.Forms.ComboBox
    Public WithEvents txtPolicyNumber As System.Windows.Forms.TextBox
    Public WithEvents txtEndNumber As System.Windows.Forms.TextBox
    Public WithEvents cmdAgentLookup As System.Windows.Forms.Button
    Public WithEvents txtBookNumber As System.Windows.Forms.TextBox
    Public WithEvents txtAgent As System.Windows.Forms.TextBox
    Public WithEvents txtStartNumber As System.Windows.Forms.TextBox
    Public WithEvents cboAssignedDate As System.Windows.Forms.DateTimePicker
    Public WithEvents lblPolicyNumber As System.Windows.Forms.Label
    Public WithEvents lblLastUpdate As System.Windows.Forms.Label
    Public WithEvents lblAssignedDate As System.Windows.Forms.Label
    Public WithEvents lblEndNumber As System.Windows.Forms.Label
    Public WithEvents lblStartNumber As System.Windows.Forms.Label
    Public WithEvents lblAgent As System.Windows.Forms.Label
    Public WithEvents lblBranch As System.Windows.Forms.Label
    Public WithEvents lblCoverNoteBookStatus As System.Windows.Forms.Label
    Public WithEvents lblBookNumber As System.Windows.Forms.Label
    Public WithEvents fraMain As System.Windows.Forms.GroupBox
    Public WithEvents cmdNew As System.Windows.Forms.Button
    Public WithEvents cmdNewSearch As System.Windows.Forms.Button
    Public WithEvents cmdFindNow As System.Windows.Forms.Button
    Public WithEvents cmdClose As System.Windows.Forms.Button
    Private WithEvents _stbStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents stbStatus As System.Windows.Forms.StatusStrip
    Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents imglImages As System.Windows.Forms.ImageList
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.fraMain = New System.Windows.Forms.GroupBox
        Me.cboLastUpdate = New System.Windows.Forms.DateTimePicker
        Me.cboCoverNoteBookStatus = New System.Windows.Forms.ComboBox
        Me.cboBranch = New System.Windows.Forms.ComboBox
        Me.txtPolicyNumber = New System.Windows.Forms.TextBox
        Me.txtEndNumber = New System.Windows.Forms.TextBox
        Me.cmdAgentLookup = New System.Windows.Forms.Button
        Me.txtBookNumber = New System.Windows.Forms.TextBox
        Me.txtAgent = New System.Windows.Forms.TextBox
        Me.txtStartNumber = New System.Windows.Forms.TextBox
        Me.cboAssignedDate = New System.Windows.Forms.DateTimePicker
        Me.lblPolicyNumber = New System.Windows.Forms.Label
        Me.lblLastUpdate = New System.Windows.Forms.Label
        Me.lblAssignedDate = New System.Windows.Forms.Label
        Me.lblEndNumber = New System.Windows.Forms.Label
        Me.lblStartNumber = New System.Windows.Forms.Label
        Me.lblAgent = New System.Windows.Forms.Label
        Me.lblBranch = New System.Windows.Forms.Label
        Me.lblCoverNoteBookStatus = New System.Windows.Forms.Label
        Me.lblBookNumber = New System.Windows.Forms.Label
        Me.cmdNew = New System.Windows.Forms.Button
        Me.cmdNewSearch = New System.Windows.Forms.Button
        Me.cmdFindNow = New System.Windows.Forms.Button
        Me.cmdClose = New System.Windows.Forms.Button
        Me.stbStatus = New System.Windows.Forms.StatusStrip
        Me._stbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.imglImages = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.Label1 = New System.Windows.Forms.Label
        Me.lvwSearchResults = New System.Windows.Forms.ListView
        Me._lvwSearchResults_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchResults_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchResults_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchResults_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchResults_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchResults_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchResults_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchResults_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchResults_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
        Me.fraMain.SuspendLayout()
        Me.stbStatus.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'fraMain
        '
        Me.fraMain.BackColor = System.Drawing.SystemColors.Control
        Me.fraMain.Controls.Add(Me.cboLastUpdate)
        Me.fraMain.Controls.Add(Me.cboCoverNoteBookStatus)
        Me.fraMain.Controls.Add(Me.cboBranch)
        Me.fraMain.Controls.Add(Me.txtPolicyNumber)
        Me.fraMain.Controls.Add(Me.txtEndNumber)
        Me.fraMain.Controls.Add(Me.cmdAgentLookup)
        Me.fraMain.Controls.Add(Me.txtBookNumber)
        Me.fraMain.Controls.Add(Me.txtAgent)
        Me.fraMain.Controls.Add(Me.txtStartNumber)
        Me.fraMain.Controls.Add(Me.cboAssignedDate)
        Me.fraMain.Controls.Add(Me.lblPolicyNumber)
        Me.fraMain.Controls.Add(Me.lblLastUpdate)
        Me.fraMain.Controls.Add(Me.lblAssignedDate)
        Me.fraMain.Controls.Add(Me.lblEndNumber)
        Me.fraMain.Controls.Add(Me.lblStartNumber)
        Me.fraMain.Controls.Add(Me.lblAgent)
        Me.fraMain.Controls.Add(Me.lblBranch)
        Me.fraMain.Controls.Add(Me.lblCoverNoteBookStatus)
        Me.fraMain.Controls.Add(Me.lblBookNumber)
        Me.fraMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraMain.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraMain.Location = New System.Drawing.Point(8, 8)
        Me.fraMain.Name = "fraMain"
        Me.fraMain.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraMain.Size = New System.Drawing.Size(634, 171)
        Me.fraMain.TabIndex = 0
        Me.fraMain.TabStop = False
        Me.fraMain.Text = "Cover Note Book Details"
        '
        'cboLastUpdate
        '
        Me.cboLastUpdate.Checked = False
        Me.cboLastUpdate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.cboLastUpdate.Location = New System.Drawing.Point(435, 79)
        Me.cboLastUpdate.Name = "cboLastUpdate"
        Me.cboLastUpdate.ShowCheckBox = True
        Me.cboLastUpdate.Size = New System.Drawing.Size(161, 20)
        Me.cboLastUpdate.TabIndex = 11
        '
        'cboCoverNoteBookStatus
        '
        Me.cboCoverNoteBookStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cboCoverNoteBookStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCoverNoteBookStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCoverNoteBookStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCoverNoteBookStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCoverNoteBookStatus.Location = New System.Drawing.Point(120, 137)
        Me.cboCoverNoteBookStatus.Name = "cboCoverNoteBookStatus"
        Me.cboCoverNoteBookStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCoverNoteBookStatus.Size = New System.Drawing.Size(177, 21)
        Me.cboCoverNoteBookStatus.TabIndex = 17
        '
        'cboBranch
        '
        Me.cboBranch.BackColor = System.Drawing.SystemColors.Window
        Me.cboBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBranch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBranch.Location = New System.Drawing.Point(120, 108)
        Me.cboBranch.Name = "cboBranch"
        Me.cboBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBranch.Size = New System.Drawing.Size(177, 21)
        Me.cboBranch.TabIndex = 13
        '
        'txtPolicyNumber
        '
        Me.txtPolicyNumber.AcceptsReturn = True
        Me.txtPolicyNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolicyNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicyNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicyNumber.Location = New System.Drawing.Point(435, 108)
        Me.txtPolicyNumber.MaxLength = 0
        Me.txtPolicyNumber.Name = "txtPolicyNumber"
        Me.txtPolicyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicyNumber.Size = New System.Drawing.Size(185, 20)
        Me.txtPolicyNumber.TabIndex = 15
        '
        'txtEndNumber
        '
        Me.txtEndNumber.AcceptsReturn = True
        Me.txtEndNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtEndNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEndNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEndNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEndNumber.Location = New System.Drawing.Point(435, 50)
        Me.txtEndNumber.MaxLength = 0
        Me.txtEndNumber.Name = "txtEndNumber"
        Me.txtEndNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEndNumber.Size = New System.Drawing.Size(107, 20)
        Me.txtEndNumber.TabIndex = 6
        '
        'cmdAgentLookup
        '
        Me.cmdAgentLookup.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAgentLookup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAgentLookup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAgentLookup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAgentLookup.Location = New System.Drawing.Point(314, 79)
        Me.cmdAgentLookup.Name = "cmdAgentLookup"
        Me.cmdAgentLookup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAgentLookup.Size = New System.Drawing.Size(23, 22)
        Me.cmdAgentLookup.TabIndex = 9
        Me.cmdAgentLookup.Text = "..."
        Me.cmdAgentLookup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAgentLookup.UseVisualStyleBackColor = False
        '
        'txtBookNumber
        '
        Me.txtBookNumber.AcceptsReturn = True
        Me.txtBookNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtBookNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBookNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBookNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBookNumber.Location = New System.Drawing.Point(120, 22)
        Me.txtBookNumber.MaxLength = 50
        Me.txtBookNumber.Name = "txtBookNumber"
        Me.txtBookNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBookNumber.Size = New System.Drawing.Size(209, 20)
        Me.txtBookNumber.TabIndex = 2
        '
        'txtAgent
        '
        Me.txtAgent.AcceptsReturn = True
        Me.txtAgent.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgent.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgent.Enabled = False
        Me.txtAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgent.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgent.Location = New System.Drawing.Point(120, 79)
        Me.txtAgent.MaxLength = 0
        Me.txtAgent.Name = "txtAgent"
        Me.txtAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgent.Size = New System.Drawing.Size(194, 20)
        Me.txtAgent.TabIndex = 8
        '
        'txtStartNumber
        '
        Me.txtStartNumber.AcceptsReturn = True
        Me.txtStartNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtStartNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStartNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStartNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStartNumber.Location = New System.Drawing.Point(120, 50)
        Me.txtStartNumber.MaxLength = 0
        Me.txtStartNumber.Name = "txtStartNumber"
        Me.txtStartNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStartNumber.Size = New System.Drawing.Size(107, 20)
        Me.txtStartNumber.TabIndex = 4
        '
        'cboAssignedDate
        '
        Me.cboAssignedDate.Checked = False
        Me.cboAssignedDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.cboAssignedDate.Location = New System.Drawing.Point(435, 137)
        Me.cboAssignedDate.Name = "cboAssignedDate"
        Me.cboAssignedDate.ShowCheckBox = True
        Me.cboAssignedDate.Size = New System.Drawing.Size(161, 20)
        Me.cboAssignedDate.TabIndex = 19
        '
        'lblPolicyNumber
        '
        Me.lblPolicyNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyNumber.Location = New System.Drawing.Point(344, 112)
        Me.lblPolicyNumber.Name = "lblPolicyNumber"
        Me.lblPolicyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyNumber.Size = New System.Drawing.Size(96, 19)
        Me.lblPolicyNumber.TabIndex = 14
        Me.lblPolicyNumber.Text = "Policy Number:"
        '
        'lblLastUpdate
        '
        Me.lblLastUpdate.BackColor = System.Drawing.SystemColors.Control
        Me.lblLastUpdate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLastUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLastUpdate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLastUpdate.Location = New System.Drawing.Point(344, 82)
        Me.lblLastUpdate.Name = "lblLastUpdate"
        Me.lblLastUpdate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLastUpdate.Size = New System.Drawing.Size(96, 19)
        Me.lblLastUpdate.TabIndex = 10
        Me.lblLastUpdate.Text = "Last Update:"
        '
        'lblAssignedDate
        '
        Me.lblAssignedDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblAssignedDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAssignedDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAssignedDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAssignedDate.Location = New System.Drawing.Point(344, 141)
        Me.lblAssignedDate.Name = "lblAssignedDate"
        Me.lblAssignedDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAssignedDate.Size = New System.Drawing.Size(96, 19)
        Me.lblAssignedDate.TabIndex = 18
        Me.lblAssignedDate.Text = "Assigned Date:"
        '
        'lblEndNumber
        '
        Me.lblEndNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblEndNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEndNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEndNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEndNumber.Location = New System.Drawing.Point(344, 52)
        Me.lblEndNumber.Name = "lblEndNumber"
        Me.lblEndNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEndNumber.Size = New System.Drawing.Size(96, 19)
        Me.lblEndNumber.TabIndex = 5
        Me.lblEndNumber.Text = "End Number:"
        '
        'lblStartNumber
        '
        Me.lblStartNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblStartNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStartNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStartNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStartNumber.Location = New System.Drawing.Point(5, 52)
        Me.lblStartNumber.Name = "lblStartNumber"
        Me.lblStartNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStartNumber.Size = New System.Drawing.Size(114, 19)
        Me.lblStartNumber.TabIndex = 3
        Me.lblStartNumber.Text = "Start Number:"
        '
        'lblAgent
        '
        Me.lblAgent.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgent.Location = New System.Drawing.Point(5, 82)
        Me.lblAgent.Name = "lblAgent"
        Me.lblAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgent.Size = New System.Drawing.Size(114, 19)
        Me.lblAgent.TabIndex = 7
        Me.lblAgent.Text = "Agent:"
        '
        'lblBranch
        '
        Me.lblBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranch.Location = New System.Drawing.Point(5, 112)
        Me.lblBranch.Name = "lblBranch"
        Me.lblBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranch.Size = New System.Drawing.Size(114, 19)
        Me.lblBranch.TabIndex = 12
        Me.lblBranch.Text = "Branch:"
        '
        'lblCoverNoteBookStatus
        '
        Me.lblCoverNoteBookStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblCoverNoteBookStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCoverNoteBookStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCoverNoteBookStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCoverNoteBookStatus.Location = New System.Drawing.Point(5, 141)
        Me.lblCoverNoteBookStatus.Name = "lblCoverNoteBookStatus"
        Me.lblCoverNoteBookStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCoverNoteBookStatus.Size = New System.Drawing.Size(114, 19)
        Me.lblCoverNoteBookStatus.TabIndex = 16
        Me.lblCoverNoteBookStatus.Text = "Cover Note Status:"
        '
        'lblBookNumber
        '
        Me.lblBookNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblBookNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBookNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBookNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBookNumber.Location = New System.Drawing.Point(5, 24)
        Me.lblBookNumber.Name = "lblBookNumber"
        Me.lblBookNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBookNumber.Size = New System.Drawing.Size(114, 19)
        Me.lblBookNumber.TabIndex = 1
        Me.lblBookNumber.Text = "Book Number:"
        '
        'cmdNew
        '
        Me.cmdNew.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNew.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNew.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNew.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNew.Location = New System.Drawing.Point(9, 397)
        Me.cmdNew.Name = "cmdNew"
        Me.cmdNew.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNew.Size = New System.Drawing.Size(73, 22)
        Me.cmdNew.TabIndex = 23
        Me.cmdNew.Text = "N&ew"
        Me.cmdNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNew.UseVisualStyleBackColor = False
        '
        'cmdNewSearch
        '
        Me.cmdNewSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewSearch.Location = New System.Drawing.Point(648, 40)
        Me.cmdNewSearch.Name = "cmdNewSearch"
        Me.cmdNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewSearch.Size = New System.Drawing.Size(87, 22)
        Me.cmdNewSearch.TabIndex = 21
        Me.cmdNewSearch.TabStop = False
        Me.cmdNewSearch.Text = "Ne&w Search"
        Me.cmdNewSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNewSearch.UseVisualStyleBackColor = False
        '
        'cmdFindNow
        '
        Me.cmdFindNow.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindNow.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindNow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindNow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindNow.Location = New System.Drawing.Point(648, 12)
        Me.cmdFindNow.Name = "cmdFindNow"
        Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindNow.Size = New System.Drawing.Size(87, 22)
        Me.cmdFindNow.TabIndex = 20
        Me.cmdFindNow.TabStop = False
        Me.cmdFindNow.Text = "F&ind Now"
        Me.cmdFindNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindNow.UseVisualStyleBackColor = False
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClose.Location = New System.Drawing.Point(646, 397)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClose.Size = New System.Drawing.Size(90, 22)
        Me.cmdClose.TabIndex = 25
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClose.UseVisualStyleBackColor = False
        '
        'stbStatus
        '
        Me.stbStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._stbStatus_Panel1})
        Me.stbStatus.Location = New System.Drawing.Point(0, 421)
        Me.stbStatus.Name = "stbStatus"
        Me.stbStatus.ShowItemToolTips = True
        Me.stbStatus.Size = New System.Drawing.Size(742, 22)
        Me.stbStatus.TabIndex = 26
        '
        '_stbStatus_Panel1
        '
        Me._stbStatus_Panel1.AutoSize = False
        Me._stbStatus_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._stbStatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._stbStatus_Panel1.DoubleClickEnabled = True
        Me._stbStatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._stbStatus_Panel1.Name = "_stbStatus_Panel1"
        Me._stbStatus_Panel1.Size = New System.Drawing.Size(725, 22)
        Me._stbStatus_Panel1.Tag = ""
        Me._stbStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'imglImages
        '
        Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.imglImages.Images.SetKeyName(0, "FindImage")
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Enabled = False
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(88, 397)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 24
        Me.cmdEdit.Text = "E&dit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(467, 304)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(0, 13)
        Me.Label1.TabIndex = 27
        '
        'lvwSearchResults
        '
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwSearchResults, "")
        Me.lvwSearchResults.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSearchResults_ColumnHeader_1, Me._lvwSearchResults_ColumnHeader_2, Me._lvwSearchResults_ColumnHeader_3, Me._lvwSearchResults_ColumnHeader_4, Me._lvwSearchResults_ColumnHeader_5, Me._lvwSearchResults_ColumnHeader_6, Me._lvwSearchResults_ColumnHeader_7, Me._lvwSearchResults_ColumnHeader_8, Me._lvwSearchResults_ColumnHeader_9})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwSearchResults, False)
        Me.listViewHelper1.SetItemClickMethod(Me.lvwSearchResults, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwSearchResults, "")
        Me.lvwSearchResults.Location = New System.Drawing.Point(8, 186)
        Me.lvwSearchResults.Name = "lvwSearchResults"
        Me.lvwSearchResults.Size = New System.Drawing.Size(727, 205)
        Me.listViewHelper1.SetSmallIcons(Me.lvwSearchResults, "")
        Me.listViewHelper1.SetSorted(Me.lvwSearchResults, False)
        Me.listViewHelper1.SetSortKey(Me.lvwSearchResults, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwSearchResults, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwSearchResults.TabIndex = 22
        Me.lvwSearchResults.UseCompatibleStateImageBehavior = False
        Me.lvwSearchResults.View = System.Windows.Forms.View.Details
        '
        '_lvwSearchResults_ColumnHeader_1
        '
        Me._lvwSearchResults_ColumnHeader_1.Text = "1"
        Me._lvwSearchResults_ColumnHeader_1.Width = 0
        '
        '_lvwSearchResults_ColumnHeader_2
        '
        Me._lvwSearchResults_ColumnHeader_2.Text = "2"
        Me._lvwSearchResults_ColumnHeader_2.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_3
        '
        Me._lvwSearchResults_ColumnHeader_3.Text = "3"
        Me._lvwSearchResults_ColumnHeader_3.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_4
        '
        Me._lvwSearchResults_ColumnHeader_4.Text = "4"
        Me._lvwSearchResults_ColumnHeader_4.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_5
        '
        Me._lvwSearchResults_ColumnHeader_5.Text = "5"
        Me._lvwSearchResults_ColumnHeader_5.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_6
        '
        Me._lvwSearchResults_ColumnHeader_6.Text = "6"
        Me._lvwSearchResults_ColumnHeader_6.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_7
        '
        Me._lvwSearchResults_ColumnHeader_7.Text = "7"
        Me._lvwSearchResults_ColumnHeader_7.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_8
        '
        Me._lvwSearchResults_ColumnHeader_8.Text = "8"
        Me._lvwSearchResults_ColumnHeader_8.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_9
        '
        Me._lvwSearchResults_ColumnHeader_9.Text = "9"
        Me._lvwSearchResults_ColumnHeader_9.Width = 97
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdFindNow
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(742, 443)
        Me.Controls.Add(Me.lvwSearchResults)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.fraMain)
        Me.Controls.Add(Me.cmdNew)
        Me.Controls.Add(Me.cmdNewSearch)
        Me.Controls.Add(Me.cmdFindNow)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.stbStatus)
        Me.Controls.Add(Me.cmdEdit)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(159, 148)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Find Cover Note Book"
        Me.fraMain.ResumeLayout(False)
        Me.fraMain.PerformLayout()
        Me.stbStatus.ResumeLayout(False)
        Me.stbStatus.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub lvwSearchResults_InitializeColumnKeys()
        Me._lvwSearchResults_ColumnHeader_1.Name = ""
        Me._lvwSearchResults_ColumnHeader_2.Name = ""
        Me._lvwSearchResults_ColumnHeader_3.Name = ""
        Me._lvwSearchResults_ColumnHeader_4.Name = ""
        Me._lvwSearchResults_ColumnHeader_5.Name = ""
        Me._lvwSearchResults_ColumnHeader_6.Name = ""
        Me._lvwSearchResults_ColumnHeader_7.Name = ""
        Me._lvwSearchResults_ColumnHeader_8.Name = ""
        Me._lvwSearchResults_ColumnHeader_9.Name = ""
    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lvwSearchResults As System.Windows.Forms.ListView
    Friend WithEvents _lvwSearchResults_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSearchResults_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSearchResults_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSearchResults_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSearchResults_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSearchResults_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSearchResults_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSearchResults_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSearchResults_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
#End Region 
End Class