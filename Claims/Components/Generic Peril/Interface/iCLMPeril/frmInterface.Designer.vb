<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializetxtBox()
		Initializelbl()
		InitializefraGeneral()
		InitializecmbBox()
		InitializechkBox()
		InitializeVScroll_Renamed()
		InitializePicture2()
		InitializePicture1()
		SStab1PreviousTab = SStab1.SelectedIndex
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
    Private WithEvents TSB_Event As System.Windows.Forms.ToolStripButton
    Private WithEvents _Toolbar1_Button2 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents Risk_Details As System.Windows.Forms.ToolStripButton
    Private WithEvents _Toolbar1_Button4 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents Information_Checklist As System.Windows.Forms.ToolStripButton
    Private WithEvents _Toolbar1_Button6 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents Party As System.Windows.Forms.ToolStripButton
    Private WithEvents Policy As System.Windows.Forms.ToolStripButton
    Private WithEvents Risk As System.Windows.Forms.ToolStripButton
    Public WithEvents Toolbar1 As System.Windows.Forms.ToolStrip
    Public WithEvents cmdCheckList As System.Windows.Forms.Button
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Private WithEvents _VScroll_0 As System.Windows.Forms.VScrollBar
    Private WithEvents _txtBox_0 As System.Windows.Forms.TextBox
    Private WithEvents _cmbBox_0 As System.Windows.Forms.ComboBox
    Private WithEvents _chkBox_0 As System.Windows.Forms.CheckBox
    Private WithEvents _lbl_0 As System.Windows.Forms.Label
    Private WithEvents _Picture1_0 As System.Windows.Forms.PictureBox
    Private WithEvents _Picture2_0 As System.Windows.Forms.PictureBox
    Private WithEvents _fraGeneral_0 As System.Windows.Forms.GroupBox
    Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
    Private WithEvents _Picture1_1 As System.Windows.Forms.PictureBox
    Private WithEvents _VScroll_1 As System.Windows.Forms.VScrollBar
    Private WithEvents _Picture2_1 As System.Windows.Forms.PictureBox
    Private WithEvents _fraGeneral_1 As System.Windows.Forms.GroupBox
    Private WithEvents _SSTab1_TabPage1 As System.Windows.Forms.TabPage
    Private WithEvents _VScroll_2 As System.Windows.Forms.VScrollBar
    Private WithEvents _Picture1_2 As System.Windows.Forms.PictureBox
    Private WithEvents _Picture2_2 As System.Windows.Forms.PictureBox
    Private WithEvents _fraGeneral_2 As System.Windows.Forms.GroupBox
    Private WithEvents _SSTab1_TabPage2 As System.Windows.Forms.TabPage
    Private WithEvents _VScroll_3 As System.Windows.Forms.VScrollBar
    Private WithEvents _Picture1_3 As System.Windows.Forms.PictureBox
    Private WithEvents _Picture2_3 As System.Windows.Forms.PictureBox
    Private WithEvents _fraGeneral_3 As System.Windows.Forms.GroupBox
    Private WithEvents _SSTab1_TabPage3 As System.Windows.Forms.TabPage
    Private WithEvents _VScroll_4 As System.Windows.Forms.VScrollBar
    Private WithEvents _Picture1_4 As System.Windows.Forms.PictureBox
    Private WithEvents _Picture2_4 As System.Windows.Forms.PictureBox
    Private WithEvents _fraGeneral_4 As System.Windows.Forms.GroupBox
    Private WithEvents _SSTab1_TabPage4 As System.Windows.Forms.TabPage
    Public WithEvents uctDriver As uctClaimPartyControl.uctClaimParty
    Public WithEvents fraDriver As System.Windows.Forms.GroupBox
    Private WithEvents _SSTab1_TabPage5 As System.Windows.Forms.TabPage
    Public WithEvents uctThirdParty As uctClaimPartyControl.uctClaimParty
    Public WithEvents fraThirdParty As System.Windows.Forms.GroupBox
    Private WithEvents _SSTab1_TabPage6 As System.Windows.Forms.TabPage
    Public WithEvents uctRepairer As uctClaimPartyControl.uctClaimParty
    Public WithEvents fraRepairer As System.Windows.Forms.GroupBox
    Private WithEvents _SSTab1_TabPage7 As System.Windows.Forms.TabPage
    Public WithEvents uctWitness As uctClaimPartyControl.uctClaimParty
    Public WithEvents fraWitness As System.Windows.Forms.GroupBox
    Private WithEvents _SSTab1_TabPage8 As System.Windows.Forms.TabPage
    Public WithEvents uctCLMReserve As uctCLMReserveControl.uctCLMReserve
    Private WithEvents _SSTab1_TabPage9 As System.Windows.Forms.TabPage
    Public WithEvents uctCLMPayment As uctCLMPaymentControl.uctCLMPayment1
    Private WithEvents _SSTab1_TabPage10 As System.Windows.Forms.TabPage
    Public WithEvents txtComments As System.Windows.Forms.TextBox
    Public WithEvents fraComments As System.Windows.Forms.GroupBox
    Private WithEvents _SSTab1_TabPage11 As System.Windows.Forms.TabPage
    Public WithEvents SSTab1 As System.Windows.Forms.TabControl
    Public WithEvents ImageList1 As System.Windows.Forms.ImageList
    Public Picture1(4) As System.Windows.Forms.PictureBox
    Public Picture2(4) As System.Windows.Forms.PictureBox
    Public VScroll_Renamed(4) As System.Windows.Forms.VScrollBar
    Public chkBox(0) As System.Windows.Forms.CheckBox
    Public cmbBox(0) As System.Windows.Forms.ComboBox
    Public fraGeneral(4) As System.Windows.Forms.GroupBox
    Public lbl(0) As System.Windows.Forms.Label
    Public txtBox(0) As System.Windows.Forms.TextBox
    Private SStab1PreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Toolbar1 = New System.Windows.Forms.ToolStrip
        Me.TSB_Event = New System.Windows.Forms.ToolStripButton
        Me.Risk_Details = New System.Windows.Forms.ToolStripButton
        Me.Information_Checklist = New System.Windows.Forms.ToolStripButton
        Me.Party = New System.Windows.Forms.ToolStripButton
        Me.Policy = New System.Windows.Forms.ToolStripButton
        Me.Risk = New System.Windows.Forms.ToolStripButton
        Me._Toolbar1_Button2 = New System.Windows.Forms.ToolStripSeparator
        Me._Toolbar1_Button4 = New System.Windows.Forms.ToolStripSeparator
        Me._Toolbar1_Button6 = New System.Windows.Forms.ToolStripSeparator
        Me.cmdCheckList = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me._fraGeneral_0 = New System.Windows.Forms.GroupBox
        Me._Picture2_0 = New System.Windows.Forms.PictureBox
        Me._VScroll_0 = New System.Windows.Forms.VScrollBar
        Me._Picture1_0 = New System.Windows.Forms.PictureBox
        Me._txtBox_0 = New System.Windows.Forms.TextBox
        Me._cmbBox_0 = New System.Windows.Forms.ComboBox
        Me._chkBox_0 = New System.Windows.Forms.CheckBox
        Me._lbl_0 = New System.Windows.Forms.Label
        Me._SSTab1_TabPage1 = New System.Windows.Forms.TabPage
        Me._fraGeneral_1 = New System.Windows.Forms.GroupBox
        Me._Picture2_1 = New System.Windows.Forms.PictureBox
        Me._Picture1_1 = New System.Windows.Forms.PictureBox
        Me._VScroll_1 = New System.Windows.Forms.VScrollBar
        Me._SSTab1_TabPage2 = New System.Windows.Forms.TabPage
        Me._fraGeneral_2 = New System.Windows.Forms.GroupBox
        Me._Picture2_2 = New System.Windows.Forms.PictureBox
        Me._VScroll_2 = New System.Windows.Forms.VScrollBar
        Me._Picture1_2 = New System.Windows.Forms.PictureBox
        Me._SSTab1_TabPage3 = New System.Windows.Forms.TabPage
        Me._fraGeneral_3 = New System.Windows.Forms.GroupBox
        Me._Picture2_3 = New System.Windows.Forms.PictureBox
        Me._VScroll_3 = New System.Windows.Forms.VScrollBar
        Me._Picture1_3 = New System.Windows.Forms.PictureBox
        Me._SSTab1_TabPage4 = New System.Windows.Forms.TabPage
        Me._fraGeneral_4 = New System.Windows.Forms.GroupBox
        Me._Picture2_4 = New System.Windows.Forms.PictureBox
        Me._VScroll_4 = New System.Windows.Forms.VScrollBar
        Me._Picture1_4 = New System.Windows.Forms.PictureBox
        Me._SSTab1_TabPage5 = New System.Windows.Forms.TabPage
        Me.fraDriver = New System.Windows.Forms.GroupBox
        Me.uctDriver = New uctClaimPartyControl.uctClaimParty
        Me._SSTab1_TabPage6 = New System.Windows.Forms.TabPage
        Me.fraThirdParty = New System.Windows.Forms.GroupBox
        Me.uctThirdParty = New uctClaimPartyControl.uctClaimParty
        Me._SSTab1_TabPage7 = New System.Windows.Forms.TabPage
        Me.fraRepairer = New System.Windows.Forms.GroupBox
        Me.uctRepairer = New uctClaimPartyControl.uctClaimParty
        Me._SSTab1_TabPage8 = New System.Windows.Forms.TabPage
        Me.fraWitness = New System.Windows.Forms.GroupBox
        Me.uctWitness = New uctClaimPartyControl.uctClaimParty
        Me._SSTab1_TabPage9 = New System.Windows.Forms.TabPage
        Me.uctCLMReserve = New uctCLMReserveControl.uctCLMReserve
        Me._SSTab1_TabPage10 = New System.Windows.Forms.TabPage
        Me.uctCLMPayment = New uctCLMPaymentControl.uctCLMPayment1
        Me._SSTab1_TabPage11 = New System.Windows.Forms.TabPage
        Me.fraComments = New System.Windows.Forms.GroupBox
        Me.txtComments = New System.Windows.Forms.TextBox
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Toolbar1.SuspendLayout()
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me._fraGeneral_0.SuspendLayout()
        CType(Me._Picture2_0, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Picture2_0.SuspendLayout()
        CType(Me._Picture1_0, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Picture1_0.SuspendLayout()
        Me._SSTab1_TabPage1.SuspendLayout()
        Me._fraGeneral_1.SuspendLayout()
        CType(Me._Picture2_1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Picture2_1.SuspendLayout()
        CType(Me._Picture1_1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._SSTab1_TabPage2.SuspendLayout()
        Me._fraGeneral_2.SuspendLayout()
        CType(Me._Picture2_2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Picture2_2.SuspendLayout()
        CType(Me._Picture1_2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._SSTab1_TabPage3.SuspendLayout()
        Me._fraGeneral_3.SuspendLayout()
        CType(Me._Picture2_3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Picture2_3.SuspendLayout()
        CType(Me._Picture1_3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._SSTab1_TabPage4.SuspendLayout()
        Me._fraGeneral_4.SuspendLayout()
        CType(Me._Picture2_4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Picture2_4.SuspendLayout()
        CType(Me._Picture1_4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._SSTab1_TabPage5.SuspendLayout()
        Me.fraDriver.SuspendLayout()
        Me._SSTab1_TabPage6.SuspendLayout()
        Me.fraThirdParty.SuspendLayout()
        Me._SSTab1_TabPage7.SuspendLayout()
        Me.fraRepairer.SuspendLayout()
        Me._SSTab1_TabPage8.SuspendLayout()
        Me.fraWitness.SuspendLayout()
        Me._SSTab1_TabPage9.SuspendLayout()
        Me._SSTab1_TabPage10.SuspendLayout()
        Me._SSTab1_TabPage11.SuspendLayout()
        Me.fraComments.SuspendLayout()
        Me.SuspendLayout()
        '
        'Toolbar1
        '
        Me.Toolbar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSB_Event, Me.Risk_Details, Me.Information_Checklist, Me.Party, Me.Policy, Me.Risk})
        Me.Toolbar1.Location = New System.Drawing.Point(0, 0)
        Me.Toolbar1.Name = "Toolbar1"
        Me.Toolbar1.Size = New System.Drawing.Size(911, 25)
        Me.Toolbar1.TabIndex = 4
        '
        'TSB_Event
        '
        Me.TSB_Event.AutoSize = False
        Me.TSB_Event.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TSB_Event.Name = "TSB_Event"
        Me.TSB_Event.Size = New System.Drawing.Size(24, 22)
        Me.TSB_Event.Tag = ""
        Me.TSB_Event.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.TSB_Event.ToolTipText = "Event"
        '
        'Risk_Details
        '
        Me.Risk_Details.AutoSize = False
        Me.Risk_Details.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.Risk_Details.Name = "Risk_Details"
        Me.Risk_Details.Size = New System.Drawing.Size(24, 22)
        Me.Risk_Details.Tag = ""
        Me.Risk_Details.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Risk_Details.ToolTipText = "Claim Details"
        '
        'Information_Checklist
        '
        Me.Information_Checklist.AutoSize = False
        Me.Information_Checklist.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.Information_Checklist.Name = "Information_Checklist"
        Me.Information_Checklist.Size = New System.Drawing.Size(24, 22)
        Me.Information_Checklist.Tag = ""
        Me.Information_Checklist.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Information_Checklist.ToolTipText = "Information Checklist"
        '
        'Party
        '
        Me.Party.AutoSize = False
        Me.Party.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.Party.Name = "Party"
        Me.Party.Size = New System.Drawing.Size(24, 22)
        Me.Party.Tag = ""
        Me.Party.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Party.ToolTipText = "Party Summary"
        '
        'Policy
        '
        Me.Policy.AutoSize = False
        Me.Policy.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.Policy.Name = "Policy"
        Me.Policy.Size = New System.Drawing.Size(24, 22)
        Me.Policy.Tag = ""
        Me.Policy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Policy.ToolTipText = "Policy Summary"
        '
        'Risk
        '
        Me.Risk.AutoSize = False
        Me.Risk.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.Risk.Name = "Risk"
        Me.Risk.Size = New System.Drawing.Size(24, 22)
        Me.Risk.Tag = ""
        Me.Risk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Risk.ToolTipText = "Risk Details"
        '
        '_Toolbar1_Button2
        '
        Me._Toolbar1_Button2.AutoSize = False
        Me._Toolbar1_Button2.Name = "_Toolbar1_Button2"
        Me._Toolbar1_Button2.Size = New System.Drawing.Size(6, 22)
        Me._Toolbar1_Button2.Tag = ""
        '
        '_Toolbar1_Button4
        '
        Me._Toolbar1_Button4.AutoSize = False
        Me._Toolbar1_Button4.Name = "_Toolbar1_Button4"
        Me._Toolbar1_Button4.Size = New System.Drawing.Size(6, 22)
        Me._Toolbar1_Button4.Tag = ""
        '
        '_Toolbar1_Button6
        '
        Me._Toolbar1_Button6.AutoSize = False
        Me._Toolbar1_Button6.Name = "_Toolbar1_Button6"
        Me._Toolbar1_Button6.Size = New System.Drawing.Size(6, 22)
        Me._Toolbar1_Button6.Tag = ""
        '
        'cmdCheckList
        '
        Me.cmdCheckList.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCheckList.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCheckList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCheckList.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCheckList.Location = New System.Drawing.Point(9, 642)
        Me.cmdCheckList.Name = "cmdCheckList"
        Me.cmdCheckList.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCheckList.Size = New System.Drawing.Size(73, 22)
        Me.cmdCheckList.TabIndex = 3
        Me.cmdCheckList.Text = "Check List"
        Me.cmdCheckList.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCheckList.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(824, 642)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 2
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(744, 642)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(665, 642)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'SSTab1
        '
        Me.SSTab1.AllowDrop = True
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage1)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage2)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage3)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage4)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage5)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage6)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage7)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage8)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage9)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage10)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage11)
        Me.SSTab1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(89, 18)
        Me.SSTab1.Location = New System.Drawing.Point(0, 32)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(904, 515)
        Me.SSTab1.TabIndex = 5
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me._fraGeneral_0)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(896, 489)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "Tab 0"
        '
        '_fraGeneral_0
        '
        Me._fraGeneral_0.BackColor = System.Drawing.SystemColors.Control
        Me._fraGeneral_0.Controls.Add(Me._Picture2_0)
        Me._fraGeneral_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraGeneral_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraGeneral_0.Location = New System.Drawing.Point(8, 8)
        Me._fraGeneral_0.Name = "_fraGeneral_0"
        Me._fraGeneral_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraGeneral_0.Size = New System.Drawing.Size(721, 369)
        Me._fraGeneral_0.TabIndex = 30
        Me._fraGeneral_0.TabStop = False
        Me._fraGeneral_0.Text = "General Details"
        '
        '_Picture2_0
        '
        Me._Picture2_0.BackColor = System.Drawing.SystemColors.Control
        Me._Picture2_0.Controls.Add(Me._VScroll_0)
        Me._Picture2_0.Controls.Add(Me._Picture1_0)
        Me._Picture2_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._Picture2_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Picture2_0.Location = New System.Drawing.Point(8, 24)
        Me._Picture2_0.Name = "_Picture2_0"
        Me._Picture2_0.Size = New System.Drawing.Size(705, 337)
        Me._Picture2_0.TabIndex = 31
        Me._Picture2_0.TabStop = False
        '
        '_VScroll_0
        '
        Me._VScroll_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._VScroll_0.LargeChange = 1
        Me._VScroll_0.Location = New System.Drawing.Point(680, 0)
        Me._VScroll_0.Maximum = 32767
        Me._VScroll_0.Name = "_VScroll_0"
        Me._VScroll_0.Size = New System.Drawing.Size(25, 361)
        Me._VScroll_0.TabIndex = 37
        Me._VScroll_0.TabStop = True
        Me._VScroll_0.Visible = False
        '
        '_Picture1_0
        '
        Me._Picture1_0.BackColor = System.Drawing.SystemColors.Control
        Me._Picture1_0.Controls.Add(Me._txtBox_0)
        Me._Picture1_0.Controls.Add(Me._cmbBox_0)
        Me._Picture1_0.Controls.Add(Me._chkBox_0)
        Me._Picture1_0.Controls.Add(Me._lbl_0)
        Me._Picture1_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._Picture1_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Picture1_0.Location = New System.Drawing.Point(0, 0)
        Me._Picture1_0.Name = "_Picture1_0"
        Me._Picture1_0.Size = New System.Drawing.Size(681, 337)
        Me._Picture1_0.TabIndex = 32
        Me._Picture1_0.TabStop = False
        '
        '_txtBox_0
        '
        Me._txtBox_0.AcceptsReturn = True
        Me._txtBox_0.AccessibleName = ""
        Me._txtBox_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtBox_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtBox_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtBox_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtBox_0.Location = New System.Drawing.Point(224, 36)
        Me._txtBox_0.MaxLength = 70
        Me._txtBox_0.Name = "_txtBox_0"
        Me._txtBox_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtBox_0.Size = New System.Drawing.Size(456, 20)
        Me._txtBox_0.TabIndex = 35
        Me._txtBox_0.Visible = False
        '
        '_cmbBox_0
        '
        Me._cmbBox_0.BackColor = System.Drawing.SystemColors.Window
        Me._cmbBox_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbBox_0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cmbBox_0.Enabled = False
        Me._cmbBox_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmbBox_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._cmbBox_0.Location = New System.Drawing.Point(224, 8)
        Me._cmbBox_0.Name = "_cmbBox_0"
        Me._cmbBox_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmbBox_0.Size = New System.Drawing.Size(169, 21)
        Me._cmbBox_0.TabIndex = 34
        Me._cmbBox_0.Visible = False
        '
        '_chkBox_0
        '
        Me._chkBox_0.BackColor = System.Drawing.SystemColors.Control
        Me._chkBox_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkBox_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkBox_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkBox_0.Location = New System.Drawing.Point(224, 64)
        Me._chkBox_0.Name = "_chkBox_0"
        Me._chkBox_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkBox_0.Size = New System.Drawing.Size(17, 17)
        Me._chkBox_0.TabIndex = 33
        Me._chkBox_0.UseVisualStyleBackColor = False
        Me._chkBox_0.Visible = False
        '
        '_lbl_0
        '
        Me._lbl_0.BackColor = System.Drawing.SystemColors.Control
        Me._lbl_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lbl_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lbl_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lbl_0.Location = New System.Drawing.Point(8, 8)
        Me._lbl_0.Name = "_lbl_0"
        Me._lbl_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lbl_0.Size = New System.Drawing.Size(209, 25)
        Me._lbl_0.TabIndex = 36
        Me._lbl_0.Tag = "0"
        Me._lbl_0.Text = "Label:"
        Me._lbl_0.Visible = False
        '
        '_SSTab1_TabPage1
        '
        Me._SSTab1_TabPage1.Controls.Add(Me._fraGeneral_1)
        Me._SSTab1_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage1.Name = "_SSTab1_TabPage1"
        Me._SSTab1_TabPage1.Size = New System.Drawing.Size(896, 489)
        Me._SSTab1_TabPage1.TabIndex = 1
        Me._SSTab1_TabPage1.Text = "Tab 1"
        '
        '_fraGeneral_1
        '
        Me._fraGeneral_1.BackColor = System.Drawing.SystemColors.Control
        Me._fraGeneral_1.Controls.Add(Me._Picture2_1)
        Me._fraGeneral_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraGeneral_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraGeneral_1.Location = New System.Drawing.Point(8, 8)
        Me._fraGeneral_1.Name = "_fraGeneral_1"
        Me._fraGeneral_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraGeneral_1.Size = New System.Drawing.Size(721, 369)
        Me._fraGeneral_1.TabIndex = 18
        Me._fraGeneral_1.TabStop = False
        Me._fraGeneral_1.Text = "General Details"
        '
        '_Picture2_1
        '
        Me._Picture2_1.BackColor = System.Drawing.SystemColors.Control
        Me._Picture2_1.Controls.Add(Me._Picture1_1)
        Me._Picture2_1.Controls.Add(Me._VScroll_1)
        Me._Picture2_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._Picture2_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Picture2_1.Location = New System.Drawing.Point(8, 24)
        Me._Picture2_1.Name = "_Picture2_1"
        Me._Picture2_1.Size = New System.Drawing.Size(705, 337)
        Me._Picture2_1.TabIndex = 19
        Me._Picture2_1.TabStop = False
        '
        '_Picture1_1
        '
        Me._Picture1_1.BackColor = System.Drawing.SystemColors.Control
        Me._Picture1_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._Picture1_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Picture1_1.Location = New System.Drawing.Point(0, 0)
        Me._Picture1_1.Name = "_Picture1_1"
        Me._Picture1_1.Size = New System.Drawing.Size(681, 337)
        Me._Picture1_1.TabIndex = 21
        Me._Picture1_1.TabStop = False
        '
        '_VScroll_1
        '
        Me._VScroll_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._VScroll_1.LargeChange = 1
        Me._VScroll_1.Location = New System.Drawing.Point(680, 0)
        Me._VScroll_1.Maximum = 32767
        Me._VScroll_1.Name = "_VScroll_1"
        Me._VScroll_1.Size = New System.Drawing.Size(25, 361)
        Me._VScroll_1.TabIndex = 20
        Me._VScroll_1.TabStop = True
        Me._VScroll_1.Visible = False
        '
        '_SSTab1_TabPage2
        '
        Me._SSTab1_TabPage2.Controls.Add(Me._fraGeneral_2)
        Me._SSTab1_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage2.Name = "_SSTab1_TabPage2"
        Me._SSTab1_TabPage2.Size = New System.Drawing.Size(896, 489)
        Me._SSTab1_TabPage2.TabIndex = 2
        Me._SSTab1_TabPage2.Text = "Tab 2"
        '
        '_fraGeneral_2
        '
        Me._fraGeneral_2.BackColor = System.Drawing.SystemColors.Control
        Me._fraGeneral_2.Controls.Add(Me._Picture2_2)
        Me._fraGeneral_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraGeneral_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraGeneral_2.Location = New System.Drawing.Point(8, 8)
        Me._fraGeneral_2.Name = "_fraGeneral_2"
        Me._fraGeneral_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraGeneral_2.Size = New System.Drawing.Size(721, 369)
        Me._fraGeneral_2.TabIndex = 14
        Me._fraGeneral_2.TabStop = False
        Me._fraGeneral_2.Text = "General Details"
        '
        '_Picture2_2
        '
        Me._Picture2_2.BackColor = System.Drawing.SystemColors.Control
        Me._Picture2_2.Controls.Add(Me._VScroll_2)
        Me._Picture2_2.Controls.Add(Me._Picture1_2)
        Me._Picture2_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._Picture2_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Picture2_2.Location = New System.Drawing.Point(8, 24)
        Me._Picture2_2.Name = "_Picture2_2"
        Me._Picture2_2.Size = New System.Drawing.Size(705, 337)
        Me._Picture2_2.TabIndex = 15
        Me._Picture2_2.TabStop = False
        '
        '_VScroll_2
        '
        Me._VScroll_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._VScroll_2.LargeChange = 1
        Me._VScroll_2.Location = New System.Drawing.Point(680, 0)
        Me._VScroll_2.Maximum = 32767
        Me._VScroll_2.Name = "_VScroll_2"
        Me._VScroll_2.Size = New System.Drawing.Size(25, 361)
        Me._VScroll_2.TabIndex = 17
        Me._VScroll_2.TabStop = True
        Me._VScroll_2.Visible = False
        '
        '_Picture1_2
        '
        Me._Picture1_2.BackColor = System.Drawing.SystemColors.Control
        Me._Picture1_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._Picture1_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Picture1_2.Location = New System.Drawing.Point(0, 0)
        Me._Picture1_2.Name = "_Picture1_2"
        Me._Picture1_2.Size = New System.Drawing.Size(681, 337)
        Me._Picture1_2.TabIndex = 16
        Me._Picture1_2.TabStop = False
        '
        '_SSTab1_TabPage3
        '
        Me._SSTab1_TabPage3.Controls.Add(Me._fraGeneral_3)
        Me._SSTab1_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage3.Name = "_SSTab1_TabPage3"
        Me._SSTab1_TabPage3.Size = New System.Drawing.Size(896, 489)
        Me._SSTab1_TabPage3.TabIndex = 3
        Me._SSTab1_TabPage3.Text = "Tab 3"
        '
        '_fraGeneral_3
        '
        Me._fraGeneral_3.BackColor = System.Drawing.SystemColors.Control
        Me._fraGeneral_3.Controls.Add(Me._Picture2_3)
        Me._fraGeneral_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraGeneral_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraGeneral_3.Location = New System.Drawing.Point(8, 8)
        Me._fraGeneral_3.Name = "_fraGeneral_3"
        Me._fraGeneral_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraGeneral_3.Size = New System.Drawing.Size(721, 369)
        Me._fraGeneral_3.TabIndex = 10
        Me._fraGeneral_3.TabStop = False
        Me._fraGeneral_3.Text = "General Details"
        '
        '_Picture2_3
        '
        Me._Picture2_3.BackColor = System.Drawing.SystemColors.Control
        Me._Picture2_3.Controls.Add(Me._VScroll_3)
        Me._Picture2_3.Controls.Add(Me._Picture1_3)
        Me._Picture2_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._Picture2_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Picture2_3.Location = New System.Drawing.Point(8, 24)
        Me._Picture2_3.Name = "_Picture2_3"
        Me._Picture2_3.Size = New System.Drawing.Size(705, 337)
        Me._Picture2_3.TabIndex = 11
        Me._Picture2_3.TabStop = False
        '
        '_VScroll_3
        '
        Me._VScroll_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._VScroll_3.LargeChange = 1
        Me._VScroll_3.Location = New System.Drawing.Point(680, 0)
        Me._VScroll_3.Maximum = 32767
        Me._VScroll_3.Name = "_VScroll_3"
        Me._VScroll_3.Size = New System.Drawing.Size(25, 361)
        Me._VScroll_3.TabIndex = 13
        Me._VScroll_3.TabStop = True
        Me._VScroll_3.Visible = False
        '
        '_Picture1_3
        '
        Me._Picture1_3.BackColor = System.Drawing.SystemColors.Control
        Me._Picture1_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._Picture1_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Picture1_3.Location = New System.Drawing.Point(0, 0)
        Me._Picture1_3.Name = "_Picture1_3"
        Me._Picture1_3.Size = New System.Drawing.Size(681, 337)
        Me._Picture1_3.TabIndex = 12
        Me._Picture1_3.TabStop = False
        '
        '_SSTab1_TabPage4
        '
        Me._SSTab1_TabPage4.Controls.Add(Me._fraGeneral_4)
        Me._SSTab1_TabPage4.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage4.Name = "_SSTab1_TabPage4"
        Me._SSTab1_TabPage4.Size = New System.Drawing.Size(896, 489)
        Me._SSTab1_TabPage4.TabIndex = 4
        Me._SSTab1_TabPage4.Text = "Tab 4"
        '
        '_fraGeneral_4
        '
        Me._fraGeneral_4.BackColor = System.Drawing.SystemColors.Control
        Me._fraGeneral_4.Controls.Add(Me._Picture2_4)
        Me._fraGeneral_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraGeneral_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraGeneral_4.Location = New System.Drawing.Point(8, 8)
        Me._fraGeneral_4.Name = "_fraGeneral_4"
        Me._fraGeneral_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraGeneral_4.Size = New System.Drawing.Size(721, 369)
        Me._fraGeneral_4.TabIndex = 6
        Me._fraGeneral_4.TabStop = False
        Me._fraGeneral_4.Text = "General Details"
        '
        '_Picture2_4
        '
        Me._Picture2_4.BackColor = System.Drawing.SystemColors.Control
        Me._Picture2_4.Controls.Add(Me._VScroll_4)
        Me._Picture2_4.Controls.Add(Me._Picture1_4)
        Me._Picture2_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._Picture2_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Picture2_4.Location = New System.Drawing.Point(8, 24)
        Me._Picture2_4.Name = "_Picture2_4"
        Me._Picture2_4.Size = New System.Drawing.Size(705, 337)
        Me._Picture2_4.TabIndex = 7
        Me._Picture2_4.TabStop = False
        '
        '_VScroll_4
        '
        Me._VScroll_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._VScroll_4.LargeChange = 1
        Me._VScroll_4.Location = New System.Drawing.Point(680, 0)
        Me._VScroll_4.Maximum = 32767
        Me._VScroll_4.Name = "_VScroll_4"
        Me._VScroll_4.Size = New System.Drawing.Size(25, 361)
        Me._VScroll_4.TabIndex = 9
        Me._VScroll_4.TabStop = True
        Me._VScroll_4.Visible = False
        '
        '_Picture1_4
        '
        Me._Picture1_4.BackColor = System.Drawing.SystemColors.Control
        Me._Picture1_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._Picture1_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Picture1_4.Location = New System.Drawing.Point(0, 0)
        Me._Picture1_4.Name = "_Picture1_4"
        Me._Picture1_4.Size = New System.Drawing.Size(681, 337)
        Me._Picture1_4.TabIndex = 8
        Me._Picture1_4.TabStop = False
        '
        '_SSTab1_TabPage5
        '
        Me._SSTab1_TabPage5.Controls.Add(Me.fraDriver)
        Me._SSTab1_TabPage5.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage5.Name = "_SSTab1_TabPage5"
        Me._SSTab1_TabPage5.Size = New System.Drawing.Size(896, 489)
        Me._SSTab1_TabPage5.TabIndex = 5
        Me._SSTab1_TabPage5.Text = "Tab 5"
        '
        'fraDriver
        '
        Me.fraDriver.BackColor = System.Drawing.SystemColors.Control
        Me.fraDriver.Controls.Add(Me.uctDriver)
        Me.fraDriver.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDriver.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDriver.Location = New System.Drawing.Point(8, 8)
        Me.fraDriver.Name = "fraDriver"
        Me.fraDriver.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDriver.Size = New System.Drawing.Size(721, 369)
        Me.fraDriver.TabIndex = 22
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
        Me.uctDriver.Size = New System.Drawing.Size(705, 345)
        Me.uctDriver.TabIndex = 23
        Me.uctDriver.Task = 0
        '
        '_SSTab1_TabPage6
        '
        Me._SSTab1_TabPage6.Controls.Add(Me.fraThirdParty)
        Me._SSTab1_TabPage6.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage6.Name = "_SSTab1_TabPage6"
        Me._SSTab1_TabPage6.Size = New System.Drawing.Size(896, 489)
        Me._SSTab1_TabPage6.TabIndex = 6
        Me._SSTab1_TabPage6.Text = "Tab 6"
        '
        'fraThirdParty
        '
        Me.fraThirdParty.BackColor = System.Drawing.SystemColors.Control
        Me.fraThirdParty.Controls.Add(Me.uctThirdParty)
        Me.fraThirdParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraThirdParty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraThirdParty.Location = New System.Drawing.Point(8, 8)
        Me.fraThirdParty.Name = "fraThirdParty"
        Me.fraThirdParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraThirdParty.Size = New System.Drawing.Size(721, 369)
        Me.fraThirdParty.TabIndex = 24
        Me.fraThirdParty.TabStop = False
        Me.fraThirdParty.Text = "Third Party Details"
        '
        'uctThirdParty
        '
        Me.uctThirdParty.ClaimId = 0
        Me.uctThirdParty.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctThirdParty.Location = New System.Drawing.Point(8, 16)
        Me.uctThirdParty.Name = "uctThirdParty"
        Me.uctThirdParty.PartyType = 0
        Me.uctThirdParty.PartyTypeCode = ""
        Me.uctThirdParty.PerilTypeId = 0
        Me.uctThirdParty.RiskTypeId = 0
        Me.uctThirdParty.Size = New System.Drawing.Size(705, 345)
        Me.uctThirdParty.TabIndex = 25
        Me.uctThirdParty.Task = 0
        '
        '_SSTab1_TabPage7
        '
        Me._SSTab1_TabPage7.Controls.Add(Me.fraRepairer)
        Me._SSTab1_TabPage7.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage7.Name = "_SSTab1_TabPage7"
        Me._SSTab1_TabPage7.Size = New System.Drawing.Size(896, 489)
        Me._SSTab1_TabPage7.TabIndex = 7
        Me._SSTab1_TabPage7.Text = "Tab 7"
        '
        'fraRepairer
        '
        Me.fraRepairer.BackColor = System.Drawing.SystemColors.Control
        Me.fraRepairer.Controls.Add(Me.uctRepairer)
        Me.fraRepairer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraRepairer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraRepairer.Location = New System.Drawing.Point(8, 8)
        Me.fraRepairer.Name = "fraRepairer"
        Me.fraRepairer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraRepairer.Size = New System.Drawing.Size(721, 377)
        Me.fraRepairer.TabIndex = 26
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
        Me.uctRepairer.Size = New System.Drawing.Size(705, 329)
        Me.uctRepairer.TabIndex = 27
        Me.uctRepairer.Task = 0
        '
        '_SSTab1_TabPage8
        '
        Me._SSTab1_TabPage8.Controls.Add(Me.fraWitness)
        Me._SSTab1_TabPage8.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage8.Name = "_SSTab1_TabPage8"
        Me._SSTab1_TabPage8.Size = New System.Drawing.Size(896, 489)
        Me._SSTab1_TabPage8.TabIndex = 8
        Me._SSTab1_TabPage8.Text = "Tab 8"
        '
        'fraWitness
        '
        Me.fraWitness.BackColor = System.Drawing.SystemColors.Control
        Me.fraWitness.Controls.Add(Me.uctWitness)
        Me.fraWitness.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraWitness.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraWitness.Location = New System.Drawing.Point(8, 8)
        Me.fraWitness.Name = "fraWitness"
        Me.fraWitness.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraWitness.Size = New System.Drawing.Size(721, 369)
        Me.fraWitness.TabIndex = 28
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
        Me.uctWitness.Size = New System.Drawing.Size(705, 345)
        Me.uctWitness.TabIndex = 29
        Me.uctWitness.Task = 0
        '
        '_SSTab1_TabPage9
        '
        Me._SSTab1_TabPage9.Controls.Add(Me.uctCLMReserve)
        Me._SSTab1_TabPage9.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage9.Name = "_SSTab1_TabPage9"
        Me._SSTab1_TabPage9.Size = New System.Drawing.Size(896, 489)
        Me._SSTab1_TabPage9.TabIndex = 9
        Me._SSTab1_TabPage9.Text = "Tab 9"
        '
        'uctCLMReserve
        '
        Me.uctCLMReserve.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctCLMReserve.IsOpenClaimNoTrans = False
        Me.uctCLMReserve.Location = New System.Drawing.Point(8, 16)
        Me.uctCLMReserve.Name = "uctCLMReserve"
        Me.uctCLMReserve.ShowCoInsurers = False
        Me.uctCLMReserve.ShowEdit = True
        Me.uctCLMReserve.Size = New System.Drawing.Size(881, 457)
        Me.uctCLMReserve.TabIndex = 40
        Me.uctCLMReserve.Visible_Renamed = True
        '
        '_SSTab1_TabPage10
        '
        Me._SSTab1_TabPage10.Controls.Add(Me.uctCLMPayment)
        Me._SSTab1_TabPage10.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage10.Name = "_SSTab1_TabPage10"
        Me._SSTab1_TabPage10.Size = New System.Drawing.Size(896, 750)
        Me._SSTab1_TabPage10.TabIndex = 10
        Me._SSTab1_TabPage10.Text = "Tab 10"
        '
        'uctCLMPayment
        '
        Me.uctCLMPayment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctCLMPayment.IsOpenClaimNoTrans = False
        Me.uctCLMPayment.Location = New System.Drawing.Point(8, 8)
        Me.uctCLMPayment.Name = "uctCLMPayment"
        Me.uctCLMPayment.RI2007Enabled = False
        Me.uctCLMPayment.ShowCoInsurers = False
        Me.uctCLMPayment.Size = New System.Drawing.Size(881, 745)
        Me.uctCLMPayment.TabIndex = 41
        Me.uctCLMPayment.WorkClaimID = 0
        Me.uctCLMPayment.WorkClaimPerilId = 0
        '
        '_SSTab1_TabPage11
        '
        Me._SSTab1_TabPage11.Controls.Add(Me.fraComments)
        Me._SSTab1_TabPage11.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage11.Name = "_SSTab1_TabPage11"
        Me._SSTab1_TabPage11.Size = New System.Drawing.Size(896, 489)
        Me._SSTab1_TabPage11.TabIndex = 11
        Me._SSTab1_TabPage11.Text = "Tab 11"
        '
        'fraComments
        '
        Me.fraComments.BackColor = System.Drawing.SystemColors.Control
        Me.fraComments.Controls.Add(Me.txtComments)
        Me.fraComments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraComments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraComments.Location = New System.Drawing.Point(8, 16)
        Me.fraComments.Name = "fraComments"
        Me.fraComments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraComments.Size = New System.Drawing.Size(729, 377)
        Me.fraComments.TabIndex = 38
        Me.fraComments.TabStop = False
        Me.fraComments.Text = "Comments"
        '
        'txtComments
        '
        Me.txtComments.AcceptsReturn = True
        Me.txtComments.BackColor = System.Drawing.SystemColors.Window
        Me.txtComments.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtComments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtComments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtComments.Location = New System.Drawing.Point(8, 16)
        Me.txtComments.MaxLength = 0
        Me.txtComments.Multiline = True
        Me.txtComments.Name = "txtComments"
        Me.txtComments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtComments.Size = New System.Drawing.Size(697, 353)
        Me.txtComments.TabIndex = 39
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
        Me.ImageList1.Images.SetKeyName(6, "")
        Me.ImageList1.Images.SetKeyName(7, "")
        Me.ImageList1.Images.SetKeyName(8, "")
        Me.ImageList1.Images.SetKeyName(9, "")
        Me.ImageList1.Images.SetKeyName(10, "")
        Me.ImageList1.Images.SetKeyName(11, "")
        Me.ImageList1.Images.SetKeyName(12, "")
        Me.ImageList1.Images.SetKeyName(13, "")
        Me.ImageList1.Images.SetKeyName(14, "")
        Me.ImageList1.Images.SetKeyName(15, "")
        Me.ImageList1.Images.SetKeyName(16, "")
        Me.ImageList1.Images.SetKeyName(17, "")
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(911, 670)
        Me.Controls.Add(Me.Toolbar1)
        Me.Controls.Add(Me.cmdCheckList)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.SSTab1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Toolbar1.ResumeLayout(False)
        Me.Toolbar1.PerformLayout()
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me._fraGeneral_0.ResumeLayout(False)
        CType(Me._Picture2_0, System.ComponentModel.ISupportInitialize).EndInit()
        Me._Picture2_0.ResumeLayout(False)
        CType(Me._Picture1_0, System.ComponentModel.ISupportInitialize).EndInit()
        Me._Picture1_0.ResumeLayout(False)
        Me._Picture1_0.PerformLayout()
        Me._SSTab1_TabPage1.ResumeLayout(False)
        Me._fraGeneral_1.ResumeLayout(False)
        CType(Me._Picture2_1, System.ComponentModel.ISupportInitialize).EndInit()
        Me._Picture2_1.ResumeLayout(False)
        CType(Me._Picture1_1, System.ComponentModel.ISupportInitialize).EndInit()
        Me._SSTab1_TabPage2.ResumeLayout(False)
        Me._fraGeneral_2.ResumeLayout(False)
        CType(Me._Picture2_2, System.ComponentModel.ISupportInitialize).EndInit()
        Me._Picture2_2.ResumeLayout(False)
        CType(Me._Picture1_2, System.ComponentModel.ISupportInitialize).EndInit()
        Me._SSTab1_TabPage3.ResumeLayout(False)
        Me._fraGeneral_3.ResumeLayout(False)
        CType(Me._Picture2_3, System.ComponentModel.ISupportInitialize).EndInit()
        Me._Picture2_3.ResumeLayout(False)
        CType(Me._Picture1_3, System.ComponentModel.ISupportInitialize).EndInit()
        Me._SSTab1_TabPage4.ResumeLayout(False)
        Me._fraGeneral_4.ResumeLayout(False)
        CType(Me._Picture2_4, System.ComponentModel.ISupportInitialize).EndInit()
        Me._Picture2_4.ResumeLayout(False)
        CType(Me._Picture1_4, System.ComponentModel.ISupportInitialize).EndInit()
        Me._SSTab1_TabPage5.ResumeLayout(False)
        Me.fraDriver.ResumeLayout(False)
        Me._SSTab1_TabPage6.ResumeLayout(False)
        Me.fraThirdParty.ResumeLayout(False)
        Me._SSTab1_TabPage7.ResumeLayout(False)
        Me.fraRepairer.ResumeLayout(False)
        Me._SSTab1_TabPage8.ResumeLayout(False)
        Me.fraWitness.ResumeLayout(False)
        Me._SSTab1_TabPage9.ResumeLayout(False)
        Me._SSTab1_TabPage10.ResumeLayout(False)
        Me._SSTab1_TabPage11.ResumeLayout(False)
        Me.fraComments.ResumeLayout(False)
        Me.fraComments.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializetxtBox()
        Me.txtBox(0) = _txtBox_0
    End Sub
	Sub Initializelbl()
		Me.lbl(0) = _lbl_0
	End Sub
	Sub InitializefraGeneral()
		Me.fraGeneral(0) = _fraGeneral_0
		Me.fraGeneral(1) = _fraGeneral_1
		Me.fraGeneral(2) = _fraGeneral_2
		Me.fraGeneral(3) = _fraGeneral_3
		Me.fraGeneral(4) = _fraGeneral_4
	End Sub
	Sub InitializecmbBox()
		Me.cmbBox(0) = _cmbBox_0
	End Sub
	Sub InitializechkBox()
		Me.chkBox(0) = _chkBox_0
	End Sub
	Sub InitializeVScroll_Renamed()
		Me.VScroll_Renamed(0) = _VScroll_0
		Me.VScroll_Renamed(1) = _VScroll_1
		Me.VScroll_Renamed(2) = _VScroll_2
		Me.VScroll_Renamed(3) = _VScroll_3
		Me.VScroll_Renamed(4) = _VScroll_4
	End Sub
	Sub InitializePicture2()
		Me.Picture2(0) = _Picture2_0
		Me.Picture2(1) = _Picture2_1
		Me.Picture2(2) = _Picture2_2
		Me.Picture2(3) = _Picture2_3
		Me.Picture2(4) = _Picture2_4
	End Sub
	Sub InitializePicture1()
		Me.Picture1(0) = _Picture1_0
		Me.Picture1(1) = _Picture1_1
		Me.Picture1(2) = _Picture1_2
		Me.Picture1(3) = _Picture1_3
		Me.Picture1(4) = _Picture1_4
	End Sub
#End Region 
End Class
