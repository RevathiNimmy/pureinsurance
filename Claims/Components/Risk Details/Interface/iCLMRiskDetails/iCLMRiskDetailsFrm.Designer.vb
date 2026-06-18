<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializetxtText()
		InitializetxtInteger()
		InitializetxtDate()
		InitializetxtComment()
		InitializelblLabel()
		InitializefraGeneralDetails()
		InitializecmdNext()
		InitializecmbLookup()
		InitializechkCheck()
		InitializeVScroll1()
		InitializePicture2()
		InitializePicture1()
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
    Private WithEvents _Financial As System.Windows.Forms.ToolStripButton
    Private WithEvents _Event As System.Windows.Forms.ToolStripButton
    Private WithEvents _Party As System.Windows.Forms.ToolStripButton
    Private WithEvents _Policy As System.Windows.Forms.ToolStripButton
    Private WithEvents _Risk As System.Windows.Forms.ToolStripButton
    Private WithEvents _DocArchive As System.Windows.Forms.ToolStripButton
    Public WithEvents Toolbar1 As System.Windows.Forms.ToolStrip
    Public WithEvents cmdAddTask As System.Windows.Forms.Button
    Public WithEvents cmdNavigate As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents lblSecondaryCause As System.Windows.Forms.Label
    Public WithEvents lblPrimaryCause As System.Windows.Forms.Label
    Public WithEvents lblDescription As System.Windows.Forms.Label
    Public WithEvents lblStatus As System.Windows.Forms.Label
    Public WithEvents lblProgressStatus As System.Windows.Forms.Label
    Public WithEvents ImageList1 As System.Windows.Forms.ImageList
    Private WithEvents _cmdNext_0 As System.Windows.Forms.Button
    Public WithEvents cmbPrimaryCause As System.Windows.Forms.ComboBox
    Public WithEvents txtDescription As System.Windows.Forms.TextBox
    Public WithEvents txtStatus As System.Windows.Forms.TextBox
    Public WithEvents cmbProgressStatus As System.Windows.Forms.ComboBox
    Public WithEvents cmbSecondaryCause As System.Windows.Forms.ComboBox
    Public WithEvents uctCLMPerilRT1 As uctCLMPerilRTControl.uctCLMPerilRT
    Public WithEvents fraPeril As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Private WithEvents _txtComment_0 As System.Windows.Forms.TextBox
    Private WithEvents _cmdNext_1 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents uctDriver As uctClaimPartyControl.uctClaimParty
    Public WithEvents fraDriver As System.Windows.Forms.GroupBox
    Private WithEvents _cmdNext_2 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
    Private WithEvents _cmdNext_3 As System.Windows.Forms.Button
    Public WithEvents uctThirdParty As uctClaimPartyControl.uctClaimParty
    Public WithEvents fraThirdParty As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage3 As System.Windows.Forms.TabPage
    Private WithEvents _cmdNext_4 As System.Windows.Forms.Button
    Public WithEvents uctRepairer As uctClaimPartyControl.uctClaimParty
    Public WithEvents fraRepairer As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage4 As System.Windows.Forms.TabPage
    Private WithEvents _cmdNext_5 As System.Windows.Forms.Button
    Public WithEvents uctWitness As uctClaimPartyControl.uctClaimParty
    Public WithEvents fraWitness As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage5 As System.Windows.Forms.TabPage
    Private WithEvents _VScroll1_0 As System.Windows.Forms.VScrollBar
    Private WithEvents _cmbLookup_0 As System.Windows.Forms.ComboBox
    Private WithEvents _txtDate_0 As System.Windows.Forms.TextBox
    Private WithEvents _txtInteger_0 As System.Windows.Forms.TextBox
    Private WithEvents _txtText_0 As System.Windows.Forms.TextBox
    Private WithEvents _chkCheck_0 As System.Windows.Forms.CheckBox
    Private WithEvents _lblLabel_0 As System.Windows.Forms.Label
    Private WithEvents _Picture2_0 As System.Windows.Forms.PictureBox
    Private WithEvents _Picture1_0 As System.Windows.Forms.PictureBox
    Private WithEvents _fraGeneralDetails_0 As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage6 As System.Windows.Forms.TabPage
    Private WithEvents _Picture2_1 As System.Windows.Forms.PictureBox
    Private WithEvents _VScroll1_1 As System.Windows.Forms.VScrollBar
    Private WithEvents _Picture1_1 As System.Windows.Forms.PictureBox
    Private WithEvents _fraGeneralDetails_1 As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage7 As System.Windows.Forms.TabPage
    Private WithEvents _VScroll1_2 As System.Windows.Forms.VScrollBar
    Private WithEvents _Picture2_2 As System.Windows.Forms.PictureBox
    Private WithEvents _Picture1_2 As System.Windows.Forms.PictureBox
    Private WithEvents _fraGeneralDetails_2 As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage8 As System.Windows.Forms.TabPage
    Private WithEvents _VScroll1_3 As System.Windows.Forms.VScrollBar
    Private WithEvents _Picture2_3 As System.Windows.Forms.PictureBox
    Private WithEvents _Picture1_3 As System.Windows.Forms.PictureBox
    Private WithEvents _fraGeneralDetails_3 As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage9 As System.Windows.Forms.TabPage
    Private WithEvents _VScroll1_4 As System.Windows.Forms.VScrollBar
    Private WithEvents _Picture2_4 As System.Windows.Forms.PictureBox
    Private WithEvents _Picture1_4 As System.Windows.Forms.PictureBox
    Private WithEvents _fraGeneralDetails_4 As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage10 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public Picture1(4) As System.Windows.Forms.PictureBox
    Public Picture2(4) As System.Windows.Forms.PictureBox
    Public VScroll1(4) As System.Windows.Forms.VScrollBar
    Public chkCheck(0) As System.Windows.Forms.CheckBox
    Public cmbLookup(0) As System.Windows.Forms.ComboBox
    Public cmdNext(5) As System.Windows.Forms.Button
    Public fraGeneralDetails(4) As System.Windows.Forms.GroupBox
    Public lblLabel(0) As System.Windows.Forms.Label
    Public txtComment(0) As System.Windows.Forms.TextBox
    Public txtDate(0) As System.Windows.Forms.TextBox
    Public txtInteger(0) As System.Windows.Forms.TextBox
    Public txtText(0) As System.Windows.Forms.TextBox
    Private tabMainTabPreviousTab As Integer
    'Developer Guide No. 7
    Private Const vbFormCode As Integer = 0
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Toolbar1 = New System.Windows.Forms.ToolStrip
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me._Financial = New System.Windows.Forms.ToolStripButton
        Me._Event = New System.Windows.Forms.ToolStripButton
        Me._Party = New System.Windows.Forms.ToolStripButton
        Me._Policy = New System.Windows.Forms.ToolStripButton
        Me._Risk = New System.Windows.Forms.ToolStripButton
        Me._DocArchive = New System.Windows.Forms.ToolStripButton
        Me.cmdAddTask = New System.Windows.Forms.Button
        Me.cmdNavigate = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblSecondaryCause = New System.Windows.Forms.Label
        Me.lblPrimaryCause = New System.Windows.Forms.Label
        Me.lblDescription = New System.Windows.Forms.Label
        Me.lblStatus = New System.Windows.Forms.Label
        Me.lblProgressStatus = New System.Windows.Forms.Label
        Me._cmdNext_0 = New System.Windows.Forms.Button
        Me.cmbPrimaryCause = New System.Windows.Forms.ComboBox
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.txtStatus = New System.Windows.Forms.TextBox
        Me.cmbProgressStatus = New System.Windows.Forms.ComboBox
        Me.cmbSecondaryCause = New System.Windows.Forms.ComboBox
        Me.fraPeril = New System.Windows.Forms.GroupBox
        Me.uctCLMPerilRT1 = New uctCLMPerilRTControl.uctCLMPerilRT
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me._txtComment_0 = New System.Windows.Forms.TextBox
        Me._cmdNext_1 = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage
        Me.fraDriver = New System.Windows.Forms.GroupBox
        Me.uctDriver = New uctClaimPartyControl.uctClaimParty
        Me._cmdNext_2 = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage3 = New System.Windows.Forms.TabPage
        Me._cmdNext_3 = New System.Windows.Forms.Button
        Me.fraThirdParty = New System.Windows.Forms.GroupBox
        Me.uctThirdParty = New uctClaimPartyControl.uctClaimParty
        Me._tabMainTab_TabPage4 = New System.Windows.Forms.TabPage
        Me._cmdNext_4 = New System.Windows.Forms.Button
        Me.fraRepairer = New System.Windows.Forms.GroupBox
        Me.uctRepairer = New uctClaimPartyControl.uctClaimParty
        Me._tabMainTab_TabPage5 = New System.Windows.Forms.TabPage
        Me._cmdNext_5 = New System.Windows.Forms.Button
        Me.fraWitness = New System.Windows.Forms.GroupBox
        Me.uctWitness = New uctClaimPartyControl.uctClaimParty
        Me._tabMainTab_TabPage6 = New System.Windows.Forms.TabPage
        Me._fraGeneralDetails_0 = New System.Windows.Forms.GroupBox
        Me._Picture1_0 = New System.Windows.Forms.PictureBox
        Me._VScroll1_0 = New System.Windows.Forms.VScrollBar
        Me._Picture2_0 = New System.Windows.Forms.PictureBox
        Me._cmbLookup_0 = New System.Windows.Forms.ComboBox
        Me._txtDate_0 = New System.Windows.Forms.TextBox
        Me._txtInteger_0 = New System.Windows.Forms.TextBox
        Me._txtText_0 = New System.Windows.Forms.TextBox
        Me._chkCheck_0 = New System.Windows.Forms.CheckBox
        Me._lblLabel_0 = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage7 = New System.Windows.Forms.TabPage
        Me._fraGeneralDetails_1 = New System.Windows.Forms.GroupBox
        Me._Picture1_1 = New System.Windows.Forms.PictureBox
        Me._Picture2_1 = New System.Windows.Forms.PictureBox
        Me._VScroll1_1 = New System.Windows.Forms.VScrollBar
        Me._tabMainTab_TabPage8 = New System.Windows.Forms.TabPage
        Me._fraGeneralDetails_2 = New System.Windows.Forms.GroupBox
        Me._Picture1_2 = New System.Windows.Forms.PictureBox
        Me._VScroll1_2 = New System.Windows.Forms.VScrollBar
        Me._Picture2_2 = New System.Windows.Forms.PictureBox
        Me._tabMainTab_TabPage9 = New System.Windows.Forms.TabPage
        Me._fraGeneralDetails_3 = New System.Windows.Forms.GroupBox
        Me._Picture1_3 = New System.Windows.Forms.PictureBox
        Me._VScroll1_3 = New System.Windows.Forms.VScrollBar
        Me._Picture2_3 = New System.Windows.Forms.PictureBox
        Me._tabMainTab_TabPage10 = New System.Windows.Forms.TabPage
        Me._fraGeneralDetails_4 = New System.Windows.Forms.GroupBox
        Me._Picture1_4 = New System.Windows.Forms.PictureBox
        Me._VScroll1_4 = New System.Windows.Forms.VScrollBar
        Me._Picture2_4 = New System.Windows.Forms.PictureBox
        Me.Toolbar1.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.fraPeril.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me.fraDriver.SuspendLayout()
        Me._tabMainTab_TabPage3.SuspendLayout()
        Me.fraThirdParty.SuspendLayout()
        Me._tabMainTab_TabPage4.SuspendLayout()
        Me.fraRepairer.SuspendLayout()
        Me._tabMainTab_TabPage5.SuspendLayout()
        Me.fraWitness.SuspendLayout()
        Me._tabMainTab_TabPage6.SuspendLayout()
        Me._fraGeneralDetails_0.SuspendLayout()
        CType(Me._Picture1_0, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Picture1_0.SuspendLayout()
        CType(Me._Picture2_0, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Picture2_0.SuspendLayout()
        Me._tabMainTab_TabPage7.SuspendLayout()
        Me._fraGeneralDetails_1.SuspendLayout()
        CType(Me._Picture1_1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Picture1_1.SuspendLayout()
        CType(Me._Picture2_1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMainTab_TabPage8.SuspendLayout()
        Me._fraGeneralDetails_2.SuspendLayout()
        CType(Me._Picture1_2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Picture1_2.SuspendLayout()
        CType(Me._Picture2_2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMainTab_TabPage9.SuspendLayout()
        Me._fraGeneralDetails_3.SuspendLayout()
        CType(Me._Picture1_3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Picture1_3.SuspendLayout()
        CType(Me._Picture2_3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMainTab_TabPage10.SuspendLayout()
        Me._fraGeneralDetails_4.SuspendLayout()
        CType(Me._Picture1_4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Picture1_4.SuspendLayout()
        CType(Me._Picture2_4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Toolbar1
        '
        Me.Toolbar1.ImageList = Me.ImageList1
        Me.Toolbar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._Financial, Me._Event, Me._Party, Me._Policy, Me._Risk, Me._DocArchive})
        Me.Toolbar1.Location = New System.Drawing.Point(0, 0)
        Me.Toolbar1.Name = "Toolbar1"
        Me.Toolbar1.Size = New System.Drawing.Size(683, 25)
        Me.Toolbar1.TabIndex = 17
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "")
        Me.ImageList1.Images.SetKeyName(1, "")
        Me.ImageList1.Images.SetKeyName(2, "")
        Me.ImageList1.Images.SetKeyName(3, "")
        Me.ImageList1.Images.SetKeyName(4, "")
        Me.ImageList1.Images.SetKeyName(5, "")
        '
        '_Financial
        '
        Me._Financial.AutoSize = False
        Me._Financial.ImageIndex = 0
        Me._Financial.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Financial.Name = "_Financial"
        Me._Financial.Size = New System.Drawing.Size(24, 22)
        Me._Financial.Tag = ""
        Me._Financial.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Financial.ToolTipText = "Financial Details"
        '
        '_Event
        '
        Me._Event.AutoSize = False
        Me._Event.ImageIndex = 1
        Me._Event.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Event.Name = "_Event"
        Me._Event.Size = New System.Drawing.Size(24, 22)
        Me._Event.Tag = ""
        Me._Event.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Event.ToolTipText = "Event logs"
        '
        '_Party
        '
        Me._Party.AutoSize = False
        Me._Party.ImageIndex = 3
        Me._Party.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Party.Name = "_Party"
        Me._Party.Size = New System.Drawing.Size(24, 22)
        Me._Party.Tag = ""
        Me._Party.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Party.ToolTipText = "Party Details"
        '
        '_Policy
        '
        Me._Policy.AutoSize = False
        Me._Policy.ImageIndex = 4
        Me._Policy.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Policy.Name = "_Policy"
        Me._Policy.Size = New System.Drawing.Size(24, 22)
        Me._Policy.Tag = ""
        Me._Policy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Policy.ToolTipText = "Policy Details"
        '
        '_Risk
        '
        Me._Risk.AutoSize = False
        Me._Risk.ImageIndex = 2
        Me._Risk.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Risk.Name = "_Risk"
        Me._Risk.Size = New System.Drawing.Size(24, 22)
        Me._Risk.Tag = ""
        Me._Risk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Risk.ToolTipText = "Risk details"
        '
        '_DocArchive
        '
        Me._DocArchive.AutoSize = False
        Me._DocArchive.ImageIndex = 5
        Me._DocArchive.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._DocArchive.Name = "_DocArchive"
        Me._DocArchive.Size = New System.Drawing.Size(24, 22)
        Me._DocArchive.Tag = ""
        Me._DocArchive.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._DocArchive.ToolTipText = "Document Archive"
        '
        'cmdAddTask
        '
        Me.cmdAddTask.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddTask.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddTask.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddTask.Location = New System.Drawing.Point(360, 363)
        Me.cmdAddTask.Name = "cmdAddTask"
        Me.cmdAddTask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddTask.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddTask.TabIndex = 59
        Me.cmdAddTask.Text = "&Add Task"
        Me.cmdAddTask.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddTask.UseVisualStyleBackColor = False
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(2, 360)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 0
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        Me.cmdNavigate.Visible = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(441, 363)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 1
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(522, 363)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(603, 363)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 3
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage1)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage2)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage3)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage4)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage5)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage6)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage7)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage8)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage9)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage10)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(60, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(3, 28)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(680, 335)
        Me.tabMainTab.TabIndex = 4
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblSecondaryCause)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPrimaryCause)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblProgressStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdNext_0)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmbPrimaryCause)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmbProgressStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmbSecondaryCause)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraPeril)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(672, 309)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "&1 - Peril Details"
        '
        'lblSecondaryCause
        '
        Me.lblSecondaryCause.AutoSize = True
        Me.lblSecondaryCause.BackColor = System.Drawing.SystemColors.Control
        Me.lblSecondaryCause.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSecondaryCause.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSecondaryCause.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSecondaryCause.Location = New System.Drawing.Point(306, 87)
        Me.lblSecondaryCause.Name = "lblSecondaryCause"
        Me.lblSecondaryCause.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSecondaryCause.Size = New System.Drawing.Size(93, 13)
        Me.lblSecondaryCause.TabIndex = 12
        Me.lblSecondaryCause.Text = "Secondary cause:"
        '
        'lblPrimaryCause
        '
        Me.lblPrimaryCause.AutoSize = True
        Me.lblPrimaryCause.BackColor = System.Drawing.SystemColors.Control
        Me.lblPrimaryCause.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPrimaryCause.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPrimaryCause.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPrimaryCause.Location = New System.Drawing.Point(32, 87)
        Me.lblPrimaryCause.Name = "lblPrimaryCause"
        Me.lblPrimaryCause.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPrimaryCause.Size = New System.Drawing.Size(76, 13)
        Me.lblPrimaryCause.TabIndex = 13
        Me.lblPrimaryCause.Text = "Primary cause:"
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(32, 63)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(63, 13)
        Me.lblDescription.TabIndex = 14
        Me.lblDescription.Text = "Description:"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatus.Location = New System.Drawing.Point(306, 39)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatus.Size = New System.Drawing.Size(40, 13)
        Me.lblStatus.TabIndex = 15
        Me.lblStatus.Text = "Status:"
        '
        'lblProgressStatus
        '
        Me.lblProgressStatus.AutoSize = True
        Me.lblProgressStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblProgressStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProgressStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProgressStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProgressStatus.Location = New System.Drawing.Point(32, 36)
        Me.lblProgressStatus.Name = "lblProgressStatus"
        Me.lblProgressStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProgressStatus.Size = New System.Drawing.Size(85, 13)
        Me.lblProgressStatus.TabIndex = 16
        Me.lblProgressStatus.Text = "Progress status :"
        '
        '_cmdNext_0
        '
        Me._cmdNext_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_0.Location = New System.Drawing.Point(574, 314)
        Me._cmdNext_0.Name = "_cmdNext_0"
        Me._cmdNext_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_0.Size = New System.Drawing.Size(28, 19)
        Me._cmdNext_0.TabIndex = 6
        Me._cmdNext_0.TabStop = False
        Me._cmdNext_0.Text = ">>"
        Me._cmdNext_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_0.UseVisualStyleBackColor = False
        Me._cmdNext_0.Visible = False
        '
        'cmbPrimaryCause
        '
        Me.cmbPrimaryCause.BackColor = System.Drawing.SystemColors.Window
        Me.cmbPrimaryCause.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbPrimaryCause.Enabled = False
        Me.cmbPrimaryCause.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbPrimaryCause.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbPrimaryCause.Location = New System.Drawing.Point(145, 84)
        Me.cmbPrimaryCause.Name = "cmbPrimaryCause"
        Me.cmbPrimaryCause.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbPrimaryCause.Size = New System.Drawing.Size(153, 21)
        Me.cmbPrimaryCause.TabIndex = 7
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Enabled = False
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(145, 60)
        Me.txtDescription.MaxLength = 0
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(427, 20)
        Me.txtDescription.TabIndex = 8
        '
        'txtStatus
        '
        Me.txtStatus.AcceptsReturn = True
        Me.txtStatus.BackColor = System.Drawing.SystemColors.Window
        Me.txtStatus.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStatus.Enabled = False
        Me.txtStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStatus.Location = New System.Drawing.Point(419, 36)
        Me.txtStatus.MaxLength = 0
        Me.txtStatus.Name = "txtStatus"
        Me.txtStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStatus.Size = New System.Drawing.Size(153, 20)
        Me.txtStatus.TabIndex = 9
        '
        'cmbProgressStatus
        '
        Me.cmbProgressStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cmbProgressStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbProgressStatus.Enabled = False
        Me.cmbProgressStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbProgressStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbProgressStatus.Location = New System.Drawing.Point(145, 36)
        Me.cmbProgressStatus.Name = "cmbProgressStatus"
        Me.cmbProgressStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbProgressStatus.Size = New System.Drawing.Size(153, 21)
        Me.cmbProgressStatus.TabIndex = 10
        '
        'cmbSecondaryCause
        '
        Me.cmbSecondaryCause.BackColor = System.Drawing.SystemColors.Window
        Me.cmbSecondaryCause.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbSecondaryCause.Enabled = False
        Me.cmbSecondaryCause.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbSecondaryCause.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbSecondaryCause.Location = New System.Drawing.Point(419, 84)
        Me.cmbSecondaryCause.Name = "cmbSecondaryCause"
        Me.cmbSecondaryCause.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbSecondaryCause.Size = New System.Drawing.Size(153, 21)
        Me.cmbSecondaryCause.TabIndex = 11
        '
        'fraPeril
        '
        Me.fraPeril.BackColor = System.Drawing.SystemColors.Control
        Me.fraPeril.Controls.Add(Me.uctCLMPerilRT1)
        Me.fraPeril.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPeril.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPeril.Location = New System.Drawing.Point(8, 116)
        Me.fraPeril.Name = "fraPeril"
        Me.fraPeril.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPeril.Size = New System.Drawing.Size(660, 185)
        Me.fraPeril.TabIndex = 57
        Me.fraPeril.TabStop = False
        Me.fraPeril.Text = "Covers/Perils"
        '
        'uctCLMPerilRT1
        '
        Me.uctCLMPerilRT1.Claimid = 0
        Me.uctCLMPerilRT1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctCLMPerilRT1.IsOpenClaimNoTrans = False
        Me.uctCLMPerilRT1.Location = New System.Drawing.Point(8, 16)
        Me.uctCLMPerilRT1.Name = "uctCLMPerilRT1"
        Me.uctCLMPerilRT1.Policy = 0
        Me.uctCLMPerilRT1.Risk = 0
        Me.uctCLMPerilRT1.ScreenCaption = ""
        Me.uctCLMPerilRT1.Size = New System.Drawing.Size(625, 161)
        Me.uctCLMPerilRT1.Status = 0
        Me.uctCLMPerilRT1.TabIndex = 58
        Me.uctCLMPerilRT1.ViewRiskFlag = False
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me._txtComment_0)
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdNext_1)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(672, 309)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "&2 - Comments"
        '
        '_txtComment_0
        '
        Me._txtComment_0.AcceptsReturn = True
        Me._txtComment_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtComment_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtComment_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtComment_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtComment_0.Location = New System.Drawing.Point(5, 3)
        Me._txtComment_0.MaxLength = 0
        Me._txtComment_0.Multiline = True
        Me._txtComment_0.Name = "_txtComment_0"
        Me._txtComment_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtComment_0.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me._txtComment_0.Size = New System.Drawing.Size(659, 301)
        Me._txtComment_0.TabIndex = 18
        Me._txtComment_0.Visible = False
        '
        '_cmdNext_1
        '
        Me._cmdNext_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_1.Location = New System.Drawing.Point(574, 314)
        Me._cmdNext_1.Name = "_cmdNext_1"
        Me._cmdNext_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_1.Size = New System.Drawing.Size(28, 19)
        Me._cmdNext_1.TabIndex = 5
        Me._cmdNext_1.TabStop = False
        Me._cmdNext_1.Text = ">>"
        Me._cmdNext_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_1.UseVisualStyleBackColor = False
        Me._cmdNext_1.Visible = False
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraDriver)
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdNext_2)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(672, 309)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "Tab 2"
        '
        'fraDriver
        '
        Me.fraDriver.BackColor = System.Drawing.SystemColors.Control
        Me.fraDriver.Controls.Add(Me.uctDriver)
        Me.fraDriver.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDriver.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDriver.Location = New System.Drawing.Point(8, 24)
        Me.fraDriver.Name = "fraDriver"
        Me.fraDriver.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDriver.Size = New System.Drawing.Size(657, 281)
        Me.fraDriver.TabIndex = 30
        Me.fraDriver.TabStop = False
        Me.fraDriver.Text = "Driver Details"
        '
        'uctDriver
        '
        Me.uctDriver.ClaimId = 0
        Me.uctDriver.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctDriver.Location = New System.Drawing.Point(8, 16)
        Me.uctDriver.Name = "uctDriver"
        Me.uctDriver.PartyType = 0
        Me.uctDriver.PartyTypeCode = ""
        Me.uctDriver.PerilTypeId = 0
        Me.uctDriver.RiskTypeId = 0
        Me.uctDriver.Size = New System.Drawing.Size(641, 257)
        Me.uctDriver.TabIndex = 37
        Me.uctDriver.Task = 0
        '
        '_cmdNext_2
        '
        Me._cmdNext_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_2.Location = New System.Drawing.Point(574, 314)
        Me._cmdNext_2.Name = "_cmdNext_2"
        Me._cmdNext_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_2.Size = New System.Drawing.Size(28, 19)
        Me._cmdNext_2.TabIndex = 19
        Me._cmdNext_2.TabStop = False
        Me._cmdNext_2.Text = ">>"
        Me._cmdNext_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_2.UseVisualStyleBackColor = False
        Me._cmdNext_2.Visible = False
        '
        '_tabMainTab_TabPage3
        '
        Me._tabMainTab_TabPage3.Controls.Add(Me._cmdNext_3)
        Me._tabMainTab_TabPage3.Controls.Add(Me.fraThirdParty)
        Me._tabMainTab_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage3.Name = "_tabMainTab_TabPage3"
        Me._tabMainTab_TabPage3.Size = New System.Drawing.Size(672, 309)
        Me._tabMainTab_TabPage3.TabIndex = 3
        Me._tabMainTab_TabPage3.Text = "Tab 3"
        '
        '_cmdNext_3
        '
        Me._cmdNext_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_3.Location = New System.Drawing.Point(576, 312)
        Me._cmdNext_3.Name = "_cmdNext_3"
        Me._cmdNext_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_3.Size = New System.Drawing.Size(28, 19)
        Me._cmdNext_3.TabIndex = 34
        Me._cmdNext_3.TabStop = False
        Me._cmdNext_3.Text = ">>"
        Me._cmdNext_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_3.UseVisualStyleBackColor = False
        Me._cmdNext_3.Visible = False
        '
        'fraThirdParty
        '
        Me.fraThirdParty.BackColor = System.Drawing.SystemColors.Control
        Me.fraThirdParty.Controls.Add(Me.uctThirdParty)
        Me.fraThirdParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraThirdParty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraThirdParty.Location = New System.Drawing.Point(8, 24)
        Me.fraThirdParty.Name = "fraThirdParty"
        Me.fraThirdParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraThirdParty.Size = New System.Drawing.Size(657, 281)
        Me.fraThirdParty.TabIndex = 31
        Me.fraThirdParty.TabStop = False
        Me.fraThirdParty.Text = "Third Party Details"
        '
        'uctThirdParty
        '
        Me.uctThirdParty.ClaimId = 0
        Me.uctThirdParty.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctThirdParty.Location = New System.Drawing.Point(16, 16)
        Me.uctThirdParty.Name = "uctThirdParty"
        Me.uctThirdParty.PartyType = 0
        Me.uctThirdParty.PartyTypeCode = ""
        Me.uctThirdParty.PerilTypeId = 0
        Me.uctThirdParty.RiskTypeId = 0
        Me.uctThirdParty.Size = New System.Drawing.Size(625, 257)
        Me.uctThirdParty.TabIndex = 38
        Me.uctThirdParty.Task = 0
        '
        '_tabMainTab_TabPage4
        '
        Me._tabMainTab_TabPage4.Controls.Add(Me._cmdNext_4)
        Me._tabMainTab_TabPage4.Controls.Add(Me.fraRepairer)
        Me._tabMainTab_TabPage4.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage4.Name = "_tabMainTab_TabPage4"
        Me._tabMainTab_TabPage4.Size = New System.Drawing.Size(672, 309)
        Me._tabMainTab_TabPage4.TabIndex = 4
        Me._tabMainTab_TabPage4.Text = "Tab 4"
        '
        '_cmdNext_4
        '
        Me._cmdNext_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_4.Location = New System.Drawing.Point(568, 312)
        Me._cmdNext_4.Name = "_cmdNext_4"
        Me._cmdNext_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_4.Size = New System.Drawing.Size(28, 19)
        Me._cmdNext_4.TabIndex = 35
        Me._cmdNext_4.TabStop = False
        Me._cmdNext_4.Text = ">>"
        Me._cmdNext_4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_4.UseVisualStyleBackColor = False
        Me._cmdNext_4.Visible = False
        '
        'fraRepairer
        '
        Me.fraRepairer.BackColor = System.Drawing.SystemColors.Control
        Me.fraRepairer.Controls.Add(Me.uctRepairer)
        Me.fraRepairer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraRepairer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraRepairer.Location = New System.Drawing.Point(8, 24)
        Me.fraRepairer.Name = "fraRepairer"
        Me.fraRepairer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraRepairer.Size = New System.Drawing.Size(657, 281)
        Me.fraRepairer.TabIndex = 32
        Me.fraRepairer.TabStop = False
        Me.fraRepairer.Text = "Repairer Details"
        '
        'uctRepairer
        '
        Me.uctRepairer.ClaimId = 0
        Me.uctRepairer.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctRepairer.Location = New System.Drawing.Point(8, 16)
        Me.uctRepairer.Name = "uctRepairer"
        Me.uctRepairer.PartyType = 0
        Me.uctRepairer.PartyTypeCode = ""
        Me.uctRepairer.PerilTypeId = 0
        Me.uctRepairer.RiskTypeId = 0
        Me.uctRepairer.Size = New System.Drawing.Size(641, 257)
        Me.uctRepairer.TabIndex = 39
        Me.uctRepairer.Task = 0
        '
        '_tabMainTab_TabPage5
        '
        Me._tabMainTab_TabPage5.Controls.Add(Me._cmdNext_5)
        Me._tabMainTab_TabPage5.Controls.Add(Me.fraWitness)
        Me._tabMainTab_TabPage5.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage5.Name = "_tabMainTab_TabPage5"
        Me._tabMainTab_TabPage5.Size = New System.Drawing.Size(672, 309)
        Me._tabMainTab_TabPage5.TabIndex = 5
        Me._tabMainTab_TabPage5.Text = "Tab 5"
        '
        '_cmdNext_5
        '
        Me._cmdNext_5.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_5.Location = New System.Drawing.Point(568, 312)
        Me._cmdNext_5.Name = "_cmdNext_5"
        Me._cmdNext_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_5.Size = New System.Drawing.Size(28, 19)
        Me._cmdNext_5.TabIndex = 36
        Me._cmdNext_5.TabStop = False
        Me._cmdNext_5.Text = ">>"
        Me._cmdNext_5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_5.UseVisualStyleBackColor = False
        Me._cmdNext_5.Visible = False
        '
        'fraWitness
        '
        Me.fraWitness.BackColor = System.Drawing.SystemColors.Control
        Me.fraWitness.Controls.Add(Me.uctWitness)
        Me.fraWitness.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraWitness.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraWitness.Location = New System.Drawing.Point(8, 24)
        Me.fraWitness.Name = "fraWitness"
        Me.fraWitness.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraWitness.Size = New System.Drawing.Size(657, 281)
        Me.fraWitness.TabIndex = 33
        Me.fraWitness.TabStop = False
        Me.fraWitness.Text = "Witness Details"
        '
        'uctWitness
        '
        Me.uctWitness.ClaimId = 0
        Me.uctWitness.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctWitness.Location = New System.Drawing.Point(8, 16)
        Me.uctWitness.Name = "uctWitness"
        Me.uctWitness.PartyType = 0
        Me.uctWitness.PartyTypeCode = ""
        Me.uctWitness.PerilTypeId = 0
        Me.uctWitness.RiskTypeId = 0
        Me.uctWitness.Size = New System.Drawing.Size(641, 257)
        Me.uctWitness.TabIndex = 40
        Me.uctWitness.Task = 0
        '
        '_tabMainTab_TabPage6
        '
        Me._tabMainTab_TabPage6.Controls.Add(Me._fraGeneralDetails_0)
        Me._tabMainTab_TabPage6.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage6.Name = "_tabMainTab_TabPage6"
        Me._tabMainTab_TabPage6.Size = New System.Drawing.Size(672, 309)
        Me._tabMainTab_TabPage6.TabIndex = 6
        Me._tabMainTab_TabPage6.Text = "Tab 6"
        '
        '_fraGeneralDetails_0
        '
        Me._fraGeneralDetails_0.BackColor = System.Drawing.SystemColors.Control
        Me._fraGeneralDetails_0.Controls.Add(Me._Picture1_0)
        Me._fraGeneralDetails_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraGeneralDetails_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraGeneralDetails_0.Location = New System.Drawing.Point(8, 12)
        Me._fraGeneralDetails_0.Name = "_fraGeneralDetails_0"
        Me._fraGeneralDetails_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraGeneralDetails_0.Size = New System.Drawing.Size(659, 289)
        Me._fraGeneralDetails_0.TabIndex = 20
        Me._fraGeneralDetails_0.TabStop = False
        Me._fraGeneralDetails_0.Text = "General Details"
        '
        '_Picture1_0
        '
        Me._Picture1_0.BackColor = System.Drawing.SystemColors.Control
        Me._Picture1_0.Controls.Add(Me._VScroll1_0)
        Me._Picture1_0.Controls.Add(Me._Picture2_0)
        Me._Picture1_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._Picture1_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Picture1_0.Location = New System.Drawing.Point(8, 16)
        Me._Picture1_0.Name = "_Picture1_0"
        Me._Picture1_0.Size = New System.Drawing.Size(651, 271)
        Me._Picture1_0.TabIndex = 21
        Me._Picture1_0.TabStop = False
        '
        '_VScroll1_0
        '
        Me._VScroll1_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._VScroll1_0.LargeChange = 2595
        Me._VScroll1_0.Location = New System.Drawing.Point(638, 0)
        Me._VScroll1_0.Maximum = 5189
        Me._VScroll1_0.Name = "_VScroll1_0"
        Me._VScroll1_0.Size = New System.Drawing.Size(21, 279)
        Me._VScroll1_0.SmallChange = 30
        Me._VScroll1_0.TabIndex = 22
        Me._VScroll1_0.TabStop = True
        Me._VScroll1_0.Visible = False
        '
        '_Picture2_0
        '
        Me._Picture2_0.BackColor = System.Drawing.SystemColors.Control
        Me._Picture2_0.Controls.Add(Me._cmbLookup_0)
        Me._Picture2_0.Controls.Add(Me._txtDate_0)
        Me._Picture2_0.Controls.Add(Me._txtInteger_0)
        Me._Picture2_0.Controls.Add(Me._txtText_0)
        Me._Picture2_0.Controls.Add(Me._chkCheck_0)
        Me._Picture2_0.Controls.Add(Me._lblLabel_0)
        Me._Picture2_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._Picture2_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Picture2_0.Location = New System.Drawing.Point(0, 2)
        Me._Picture2_0.Name = "_Picture2_0"
        Me._Picture2_0.Size = New System.Drawing.Size(617, 165)
        Me._Picture2_0.TabIndex = 23
        Me._Picture2_0.TabStop = False
        '
        '_cmbLookup_0
        '
        Me._cmbLookup_0.BackColor = System.Drawing.SystemColors.Window
        Me._cmbLookup_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbLookup_0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cmbLookup_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmbLookup_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._cmbLookup_0.Location = New System.Drawing.Point(40, 58)
        Me._cmbLookup_0.Name = "_cmbLookup_0"
        Me._cmbLookup_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmbLookup_0.Size = New System.Drawing.Size(151, 21)
        Me._cmbLookup_0.TabIndex = 28
        Me._cmbLookup_0.Visible = False
        '
        '_txtDate_0
        '
        Me._txtDate_0.AcceptsReturn = True
        Me._txtDate_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtDate_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtDate_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtDate_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtDate_0.Location = New System.Drawing.Point(44, 92)
        Me._txtDate_0.MaxLength = 0
        Me._txtDate_0.Name = "_txtDate_0"
        Me._txtDate_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtDate_0.Size = New System.Drawing.Size(151, 20)
        Me._txtDate_0.TabIndex = 27
        Me._txtDate_0.Visible = False
        '
        '_txtInteger_0
        '
        Me._txtInteger_0.AcceptsReturn = True
        Me._txtInteger_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtInteger_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtInteger_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtInteger_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtInteger_0.Location = New System.Drawing.Point(46, 30)
        Me._txtInteger_0.MaxLength = 0
        Me._txtInteger_0.Name = "_txtInteger_0"
        Me._txtInteger_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtInteger_0.Size = New System.Drawing.Size(151, 20)
        Me._txtInteger_0.TabIndex = 26
        Me._txtInteger_0.Visible = False
        '
        '_txtText_0
        '
        Me._txtText_0.AcceptsReturn = True
        Me._txtText_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtText_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtText_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtText_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtText_0.Location = New System.Drawing.Point(46, 2)
        Me._txtText_0.MaxLength = 255
        Me._txtText_0.Name = "_txtText_0"
        Me._txtText_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtText_0.Size = New System.Drawing.Size(504, 20)
        Me._txtText_0.TabIndex = 25
        Me._txtText_0.Visible = False
        '
        '_chkCheck_0
        '
        Me._chkCheck_0.BackColor = System.Drawing.SystemColors.Control
        Me._chkCheck_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkCheck_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkCheck_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkCheck_0.Location = New System.Drawing.Point(46, 120)
        Me._chkCheck_0.Name = "_chkCheck_0"
        Me._chkCheck_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkCheck_0.Size = New System.Drawing.Size(73, 17)
        Me._chkCheck_0.TabIndex = 24
        Me._chkCheck_0.UseVisualStyleBackColor = False
        Me._chkCheck_0.Visible = False
        '
        '_lblLabel_0
        '
        Me._lblLabel_0.AutoSize = True
        Me._lblLabel_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_0.Location = New System.Drawing.Point(8, 2)
        Me._lblLabel_0.Name = "_lblLabel_0"
        Me._lblLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_0.Size = New System.Drawing.Size(39, 13)
        Me._lblLabel_0.TabIndex = 29
        Me._lblLabel_0.Text = "Label1"
        Me._lblLabel_0.Visible = False
        '
        '_tabMainTab_TabPage7
        '
        Me._tabMainTab_TabPage7.Controls.Add(Me._fraGeneralDetails_1)
        Me._tabMainTab_TabPage7.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage7.Name = "_tabMainTab_TabPage7"
        Me._tabMainTab_TabPage7.Size = New System.Drawing.Size(672, 309)
        Me._tabMainTab_TabPage7.TabIndex = 7
        Me._tabMainTab_TabPage7.Text = "Tab 7"
        '
        '_fraGeneralDetails_1
        '
        Me._fraGeneralDetails_1.BackColor = System.Drawing.SystemColors.Control
        Me._fraGeneralDetails_1.Controls.Add(Me._Picture1_1)
        Me._fraGeneralDetails_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraGeneralDetails_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraGeneralDetails_1.Location = New System.Drawing.Point(8, 12)
        Me._fraGeneralDetails_1.Name = "_fraGeneralDetails_1"
        Me._fraGeneralDetails_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraGeneralDetails_1.Size = New System.Drawing.Size(659, 289)
        Me._fraGeneralDetails_1.TabIndex = 41
        Me._fraGeneralDetails_1.TabStop = False
        Me._fraGeneralDetails_1.Text = "General Details"
        '
        '_Picture1_1
        '
        Me._Picture1_1.BackColor = System.Drawing.SystemColors.Control
        Me._Picture1_1.Controls.Add(Me._Picture2_1)
        Me._Picture1_1.Controls.Add(Me._VScroll1_1)
        Me._Picture1_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._Picture1_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Picture1_1.Location = New System.Drawing.Point(8, 16)
        Me._Picture1_1.Name = "_Picture1_1"
        Me._Picture1_1.Size = New System.Drawing.Size(651, 271)
        Me._Picture1_1.TabIndex = 42
        Me._Picture1_1.TabStop = False
        '
        '_Picture2_1
        '
        Me._Picture2_1.BackColor = System.Drawing.SystemColors.Control
        Me._Picture2_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._Picture2_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Picture2_1.Location = New System.Drawing.Point(0, 2)
        Me._Picture2_1.Name = "_Picture2_1"
        Me._Picture2_1.Size = New System.Drawing.Size(617, 165)
        Me._Picture2_1.TabIndex = 44
        Me._Picture2_1.TabStop = False
        '
        '_VScroll1_1
        '
        Me._VScroll1_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._VScroll1_1.LargeChange = 2595
        Me._VScroll1_1.Location = New System.Drawing.Point(638, 0)
        Me._VScroll1_1.Maximum = 5189
        Me._VScroll1_1.Name = "_VScroll1_1"
        Me._VScroll1_1.Size = New System.Drawing.Size(21, 279)
        Me._VScroll1_1.SmallChange = 30
        Me._VScroll1_1.TabIndex = 43
        Me._VScroll1_1.TabStop = True
        Me._VScroll1_1.Visible = False
        '
        '_tabMainTab_TabPage8
        '
        Me._tabMainTab_TabPage8.Controls.Add(Me._fraGeneralDetails_2)
        Me._tabMainTab_TabPage8.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage8.Name = "_tabMainTab_TabPage8"
        Me._tabMainTab_TabPage8.Size = New System.Drawing.Size(672, 309)
        Me._tabMainTab_TabPage8.TabIndex = 8
        Me._tabMainTab_TabPage8.Text = "Tab 8"
        '
        '_fraGeneralDetails_2
        '
        Me._fraGeneralDetails_2.BackColor = System.Drawing.SystemColors.Control
        Me._fraGeneralDetails_2.Controls.Add(Me._Picture1_2)
        Me._fraGeneralDetails_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraGeneralDetails_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraGeneralDetails_2.Location = New System.Drawing.Point(8, 12)
        Me._fraGeneralDetails_2.Name = "_fraGeneralDetails_2"
        Me._fraGeneralDetails_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraGeneralDetails_2.Size = New System.Drawing.Size(659, 289)
        Me._fraGeneralDetails_2.TabIndex = 45
        Me._fraGeneralDetails_2.TabStop = False
        Me._fraGeneralDetails_2.Text = "General Details"
        '
        '_Picture1_2
        '
        Me._Picture1_2.BackColor = System.Drawing.SystemColors.Control
        Me._Picture1_2.Controls.Add(Me._VScroll1_2)
        Me._Picture1_2.Controls.Add(Me._Picture2_2)
        Me._Picture1_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._Picture1_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Picture1_2.Location = New System.Drawing.Point(8, 16)
        Me._Picture1_2.Name = "_Picture1_2"
        Me._Picture1_2.Size = New System.Drawing.Size(651, 271)
        Me._Picture1_2.TabIndex = 46
        Me._Picture1_2.TabStop = False
        '
        '_VScroll1_2
        '
        Me._VScroll1_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._VScroll1_2.LargeChange = 2595
        Me._VScroll1_2.Location = New System.Drawing.Point(638, 0)
        Me._VScroll1_2.Maximum = 5189
        Me._VScroll1_2.Name = "_VScroll1_2"
        Me._VScroll1_2.Size = New System.Drawing.Size(21, 279)
        Me._VScroll1_2.SmallChange = 30
        Me._VScroll1_2.TabIndex = 48
        Me._VScroll1_2.TabStop = True
        Me._VScroll1_2.Visible = False
        '
        '_Picture2_2
        '
        Me._Picture2_2.BackColor = System.Drawing.SystemColors.Control
        Me._Picture2_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._Picture2_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Picture2_2.Location = New System.Drawing.Point(0, 2)
        Me._Picture2_2.Name = "_Picture2_2"
        Me._Picture2_2.Size = New System.Drawing.Size(617, 165)
        Me._Picture2_2.TabIndex = 47
        Me._Picture2_2.TabStop = False
        '
        '_tabMainTab_TabPage9
        '
        Me._tabMainTab_TabPage9.Controls.Add(Me._fraGeneralDetails_3)
        Me._tabMainTab_TabPage9.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage9.Name = "_tabMainTab_TabPage9"
        Me._tabMainTab_TabPage9.Size = New System.Drawing.Size(672, 309)
        Me._tabMainTab_TabPage9.TabIndex = 9
        Me._tabMainTab_TabPage9.Text = "Tab 9"
        '
        '_fraGeneralDetails_3
        '
        Me._fraGeneralDetails_3.BackColor = System.Drawing.SystemColors.Control
        Me._fraGeneralDetails_3.Controls.Add(Me._Picture1_3)
        Me._fraGeneralDetails_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraGeneralDetails_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraGeneralDetails_3.Location = New System.Drawing.Point(8, 12)
        Me._fraGeneralDetails_3.Name = "_fraGeneralDetails_3"
        Me._fraGeneralDetails_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraGeneralDetails_3.Size = New System.Drawing.Size(659, 289)
        Me._fraGeneralDetails_3.TabIndex = 49
        Me._fraGeneralDetails_3.TabStop = False
        Me._fraGeneralDetails_3.Text = "General Details"
        '
        '_Picture1_3
        '
        Me._Picture1_3.BackColor = System.Drawing.SystemColors.Control
        Me._Picture1_3.Controls.Add(Me._VScroll1_3)
        Me._Picture1_3.Controls.Add(Me._Picture2_3)
        Me._Picture1_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._Picture1_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Picture1_3.Location = New System.Drawing.Point(8, 16)
        Me._Picture1_3.Name = "_Picture1_3"
        Me._Picture1_3.Size = New System.Drawing.Size(651, 271)
        Me._Picture1_3.TabIndex = 50
        Me._Picture1_3.TabStop = False
        '
        '_VScroll1_3
        '
        Me._VScroll1_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._VScroll1_3.LargeChange = 2595
        Me._VScroll1_3.Location = New System.Drawing.Point(638, 0)
        Me._VScroll1_3.Maximum = 5189
        Me._VScroll1_3.Name = "_VScroll1_3"
        Me._VScroll1_3.Size = New System.Drawing.Size(21, 279)
        Me._VScroll1_3.SmallChange = 30
        Me._VScroll1_3.TabIndex = 52
        Me._VScroll1_3.TabStop = True
        Me._VScroll1_3.Visible = False
        '
        '_Picture2_3
        '
        Me._Picture2_3.BackColor = System.Drawing.SystemColors.Control
        Me._Picture2_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._Picture2_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Picture2_3.Location = New System.Drawing.Point(0, 2)
        Me._Picture2_3.Name = "_Picture2_3"
        Me._Picture2_3.Size = New System.Drawing.Size(617, 165)
        Me._Picture2_3.TabIndex = 51
        Me._Picture2_3.TabStop = False
        '
        '_tabMainTab_TabPage10
        '
        Me._tabMainTab_TabPage10.Controls.Add(Me._fraGeneralDetails_4)
        Me._tabMainTab_TabPage10.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage10.Name = "_tabMainTab_TabPage10"
        Me._tabMainTab_TabPage10.Size = New System.Drawing.Size(672, 309)
        Me._tabMainTab_TabPage10.TabIndex = 10
        Me._tabMainTab_TabPage10.Text = "Tab 10"
        '
        '_fraGeneralDetails_4
        '
        Me._fraGeneralDetails_4.BackColor = System.Drawing.SystemColors.Control
        Me._fraGeneralDetails_4.Controls.Add(Me._Picture1_4)
        Me._fraGeneralDetails_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraGeneralDetails_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraGeneralDetails_4.Location = New System.Drawing.Point(8, 12)
        Me._fraGeneralDetails_4.Name = "_fraGeneralDetails_4"
        Me._fraGeneralDetails_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraGeneralDetails_4.Size = New System.Drawing.Size(659, 289)
        Me._fraGeneralDetails_4.TabIndex = 53
        Me._fraGeneralDetails_4.TabStop = False
        Me._fraGeneralDetails_4.Text = "General Details"
        '
        '_Picture1_4
        '
        Me._Picture1_4.BackColor = System.Drawing.SystemColors.Control
        Me._Picture1_4.Controls.Add(Me._VScroll1_4)
        Me._Picture1_4.Controls.Add(Me._Picture2_4)
        Me._Picture1_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._Picture1_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Picture1_4.Location = New System.Drawing.Point(8, 16)
        Me._Picture1_4.Name = "_Picture1_4"
        Me._Picture1_4.Size = New System.Drawing.Size(651, 271)
        Me._Picture1_4.TabIndex = 54
        Me._Picture1_4.TabStop = False
        '
        '_VScroll1_4
        '
        Me._VScroll1_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._VScroll1_4.LargeChange = 2595
        Me._VScroll1_4.Location = New System.Drawing.Point(638, 0)
        Me._VScroll1_4.Maximum = 5189
        Me._VScroll1_4.Name = "_VScroll1_4"
        Me._VScroll1_4.Size = New System.Drawing.Size(21, 279)
        Me._VScroll1_4.SmallChange = 30
        Me._VScroll1_4.TabIndex = 56
        Me._VScroll1_4.TabStop = True
        Me._VScroll1_4.Visible = False
        '
        '_Picture2_4
        '
        Me._Picture2_4.BackColor = System.Drawing.SystemColors.Control
        Me._Picture2_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._Picture2_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Picture2_4.Location = New System.Drawing.Point(0, 2)
        Me._Picture2_4.Name = "_Picture2_4"
        Me._Picture2_4.Size = New System.Drawing.Size(617, 165)
        Me._Picture2_4.TabIndex = 55
        Me._Picture2_4.TabStop = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(683, 388)
        Me.Controls.Add(Me.Toolbar1)
        Me.Controls.Add(Me.cmdAddTask)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Risk Details"
        Me.Toolbar1.ResumeLayout(False)
        Me.Toolbar1.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.fraPeril.ResumeLayout(False)
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me._tabMainTab_TabPage1.PerformLayout()
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me.fraDriver.ResumeLayout(False)
        Me._tabMainTab_TabPage3.ResumeLayout(False)
        Me.fraThirdParty.ResumeLayout(False)
        Me._tabMainTab_TabPage4.ResumeLayout(False)
        Me.fraRepairer.ResumeLayout(False)
        Me._tabMainTab_TabPage5.ResumeLayout(False)
        Me.fraWitness.ResumeLayout(False)
        Me._tabMainTab_TabPage6.ResumeLayout(False)
        Me._fraGeneralDetails_0.ResumeLayout(False)
        CType(Me._Picture1_0, System.ComponentModel.ISupportInitialize).EndInit()
        Me._Picture1_0.ResumeLayout(False)
        CType(Me._Picture2_0, System.ComponentModel.ISupportInitialize).EndInit()
        Me._Picture2_0.ResumeLayout(False)
        Me._Picture2_0.PerformLayout()
        Me._tabMainTab_TabPage7.ResumeLayout(False)
        Me._fraGeneralDetails_1.ResumeLayout(False)
        CType(Me._Picture1_1, System.ComponentModel.ISupportInitialize).EndInit()
        Me._Picture1_1.ResumeLayout(False)
        CType(Me._Picture2_1, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMainTab_TabPage8.ResumeLayout(False)
        Me._fraGeneralDetails_2.ResumeLayout(False)
        CType(Me._Picture1_2, System.ComponentModel.ISupportInitialize).EndInit()
        Me._Picture1_2.ResumeLayout(False)
        CType(Me._Picture2_2, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMainTab_TabPage9.ResumeLayout(False)
        Me._fraGeneralDetails_3.ResumeLayout(False)
        CType(Me._Picture1_3, System.ComponentModel.ISupportInitialize).EndInit()
        Me._Picture1_3.ResumeLayout(False)
        CType(Me._Picture2_3, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMainTab_TabPage10.ResumeLayout(False)
        Me._fraGeneralDetails_4.ResumeLayout(False)
        CType(Me._Picture1_4, System.ComponentModel.ISupportInitialize).EndInit()
        Me._Picture1_4.ResumeLayout(False)
        CType(Me._Picture2_4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub InitializetxtText()
		Me.txtText(0) = _txtText_0
	End Sub
	Sub InitializetxtInteger()
		Me.txtInteger(0) = _txtInteger_0
	End Sub
	Sub InitializetxtDate()
		Me.txtDate(0) = _txtDate_0
	End Sub
	Sub InitializetxtComment()
		Me.txtComment(0) = _txtComment_0
	End Sub
	Sub InitializelblLabel()
		Me.lblLabel(0) = _lblLabel_0
	End Sub
	Sub InitializefraGeneralDetails()
		Me.fraGeneralDetails(4) = _fraGeneralDetails_4
		Me.fraGeneralDetails(3) = _fraGeneralDetails_3
		Me.fraGeneralDetails(2) = _fraGeneralDetails_2
		Me.fraGeneralDetails(1) = _fraGeneralDetails_1
		Me.fraGeneralDetails(0) = _fraGeneralDetails_0
	End Sub
	Sub InitializecmdNext()
		Me.cmdNext(5) = _cmdNext_5
		Me.cmdNext(4) = _cmdNext_4
		Me.cmdNext(3) = _cmdNext_3
		Me.cmdNext(2) = _cmdNext_2
		Me.cmdNext(0) = _cmdNext_0
		Me.cmdNext(1) = _cmdNext_1
	End Sub
	Sub InitializecmbLookup()
		Me.cmbLookup(0) = _cmbLookup_0
	End Sub
	Sub InitializechkCheck()
		Me.chkCheck(0) = _chkCheck_0
	End Sub
	Sub InitializeVScroll1()
		Me.VScroll1(4) = _VScroll1_4
		Me.VScroll1(3) = _VScroll1_3
		Me.VScroll1(2) = _VScroll1_2
		Me.VScroll1(1) = _VScroll1_1
		Me.VScroll1(0) = _VScroll1_0
	End Sub
	Sub InitializePicture2()
		Me.Picture2(4) = _Picture2_4
		Me.Picture2(3) = _Picture2_3
		Me.Picture2(2) = _Picture2_2
		Me.Picture2(1) = _Picture2_1
		Me.Picture2(0) = _Picture2_0
	End Sub
	Sub InitializePicture1()
		Me.Picture1(4) = _Picture1_4
		Me.Picture1(3) = _Picture1_3
		Me.Picture1(2) = _Picture1_2
		Me.Picture1(1) = _Picture1_1
		Me.Picture1(0) = _Picture1_0
	End Sub
#End Region 
End Class