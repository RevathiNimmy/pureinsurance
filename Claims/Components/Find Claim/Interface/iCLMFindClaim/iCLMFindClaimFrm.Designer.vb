<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		lvwsearchdetails_InitializeColumnKeys()
		tabMainTabPreviousTab = tabMainTab.SelectedIndex
		Form_Initialize_Renamed()
	End Sub
    Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
        Dispose(True)
    End Sub
    Dim fTerminateCalled_Form_Terminate_Renamed As Boolean
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
     Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
        If Disposing Then
            If Not fTerminateCalled_Form_Terminate_Renamed Then
                fTerminateCalled_Form_Terminate_Renamed = True
                Form_Terminate_Renamed()
            End If
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(Disposing)
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Private WithEvents _lvwsearchdetails_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwsearchdetails As System.Windows.Forms.ListView
    Public WithEvents cmdView As System.Windows.Forms.Button
    Public WithEvents cmdNewSearch As System.Windows.Forms.Button
    Public WithEvents cmdFindNow As System.Windows.Forms.Button
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents stbstatus As System.Windows.Forms.StatusStrip
    Public WithEvents lblClaimRef As System.Windows.Forms.Label
    Public WithEvents lblDPARequired As System.Windows.Forms.Label
    Public WithEvents lblLossDateEndLimit As System.Windows.Forms.Label
    Public WithEvents lblLossDateStartLimit As System.Windows.Forms.Label
    Public WithEvents lblRegNumber As System.Windows.Forms.Label
    Public WithEvents lblSSYesNo As System.Windows.Forms.Label
    Public WithEvents lblClientName As System.Windows.Forms.Label
    Public WithEvents lblRegistration As System.Windows.Forms.Label
    Public WithEvents cmdPolicy As System.Windows.Forms.Button
    Public WithEvents CmdClient As System.Windows.Forms.Button
    Public WithEvents txtPolicyHolder As System.Windows.Forms.TextBox
    Public WithEvents txtPolicy As System.Windows.Forms.TextBox
    Public WithEvents txtClaimRef As System.Windows.Forms.TextBox
    Public WithEvents chkDPARequired As System.Windows.Forms.CheckBox
    Public WithEvents txtRegNumber As System.Windows.Forms.TextBox
    Public WithEvents txtclaimstartdate As System.Windows.Forms.TextBox
    Public WithEvents txtclaimenddate As System.Windows.Forms.TextBox
    Public WithEvents ChkCLosedClaim As System.Windows.Forms.CheckBox
    Public WithEvents txtClientName As System.Windows.Forms.TextBox
    Public WithEvents txtInsurer As System.Windows.Forms.TextBox
    Public WithEvents txtAccountExec As System.Windows.Forms.TextBox
    Public WithEvents cmdInsurer As System.Windows.Forms.Button
    Public WithEvents cmdAccountExec As System.Windows.Forms.Button
    Public WithEvents txtRegistration As System.Windows.Forms.TextBox
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public WithEvents imglImages As System.Windows.Forms.ImageList
    Public WithEvents ImgImage As System.Windows.Forms.PictureBox
    'TODOLIST-Commented the listviewhelper as it was conflicting with icon display in listview
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    Private tabMainTabPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.lvwsearchdetails = New System.Windows.Forms.ListView
        Me._lvwsearchdetails_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me.imglImages = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdView = New System.Windows.Forms.Button
        Me.cmdNewSearch = New System.Windows.Forms.Button
        Me.cmdFindNow = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.stbstatus = New System.Windows.Forms.StatusStrip
        Me._stbstatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.cmdTPA = New System.Windows.Forms.Button
        Me.txtTPA = New System.Windows.Forms.TextBox
        Me.lblClaimRef = New System.Windows.Forms.Label
        Me.lblDPARequired = New System.Windows.Forms.Label
        Me.lblLossDateEndLimit = New System.Windows.Forms.Label
        Me.lblLossDateStartLimit = New System.Windows.Forms.Label
        Me.lblRegNumber = New System.Windows.Forms.Label
        Me.lblSSYesNo = New System.Windows.Forms.Label
        Me.lblClientName = New System.Windows.Forms.Label
        Me.lblRegistration = New System.Windows.Forms.Label
        Me.cmdPolicy = New System.Windows.Forms.Button
        Me.CmdClient = New System.Windows.Forms.Button
        Me.txtPolicyHolder = New System.Windows.Forms.TextBox
        Me.txtPolicy = New System.Windows.Forms.TextBox
        Me.txtClaimRef = New System.Windows.Forms.TextBox
        Me.chkDPARequired = New System.Windows.Forms.CheckBox
        Me.txtRegNumber = New System.Windows.Forms.TextBox
        Me.txtclaimstartdate = New System.Windows.Forms.TextBox
        Me.txtclaimenddate = New System.Windows.Forms.TextBox
        Me.ChkCLosedClaim = New System.Windows.Forms.CheckBox
        Me.txtClientName = New System.Windows.Forms.TextBox
        Me.txtInsurer = New System.Windows.Forms.TextBox
        Me.txtAccountExec = New System.Windows.Forms.TextBox
        Me.cmdInsurer = New System.Windows.Forms.Button
        Me.cmdAccountExec = New System.Windows.Forms.Button
        Me.txtRegistration = New System.Windows.Forms.TextBox
        Me.ImgImage = New System.Windows.Forms.PictureBox
        Me.stbstatus.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.ImgImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lvwsearchdetails
        '
        Me.lvwsearchdetails.BackColor = System.Drawing.SystemColors.Window
        Me.lvwsearchdetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwsearchdetails.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwsearchdetails_ColumnHeader_1})
        Me.lvwsearchdetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwsearchdetails.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwsearchdetails.LargeImageList = Me.imglImages
        Me.lvwsearchdetails.Location = New System.Drawing.Point(8, 251)
        Me.lvwsearchdetails.Name = "lvwsearchdetails"
        Me.lvwsearchdetails.Size = New System.Drawing.Size(718, 111)
        Me.lvwsearchdetails.SmallImageList = Me.imglImages
        Me.lvwsearchdetails.TabIndex = 17
        Me.lvwsearchdetails.UseCompatibleStateImageBehavior = False
        Me.lvwsearchdetails.View = System.Windows.Forms.View.Details
        '
        '_lvwsearchdetails_ColumnHeader_1
        '
        Me._lvwsearchdetails_ColumnHeader_1.Tag = ""
        Me._lvwsearchdetails_ColumnHeader_1.Text = ""
        Me._lvwsearchdetails_ColumnHeader_1.Width = 97
        '
        'imglImages
        '
        Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.imglImages.Images.SetKeyName(0, "FindImage")
        '
        'cmdView
        '
        Me.cmdView.BackColor = System.Drawing.SystemColors.Control
        Me.cmdView.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdView.Enabled = False
        Me.cmdView.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdView.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdView.Location = New System.Drawing.Point(16, 423)
        Me.cmdView.Name = "cmdView"
        Me.cmdView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdView.Size = New System.Drawing.Size(73, 22)
        Me.cmdView.TabIndex = 18
        Me.cmdView.TabStop = False
        Me.cmdView.Text = "&View"
        Me.cmdView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdView.UseVisualStyleBackColor = False
        Me.cmdView.Visible = False
        '
        'cmdNewSearch
        '
        Me.cmdNewSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewSearch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewSearch.Location = New System.Drawing.Point(648, 56)
        Me.cmdNewSearch.Name = "cmdNewSearch"
        Me.cmdNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewSearch.Size = New System.Drawing.Size(81, 22)
        Me.cmdNewSearch.TabIndex = 16
        Me.cmdNewSearch.TabStop = False
        Me.cmdNewSearch.Text = "Ne&w Search"
        Me.cmdNewSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNewSearch.UseVisualStyleBackColor = False
        '
        'cmdFindNow
        '
        Me.cmdFindNow.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindNow.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindNow.Enabled = False
        Me.cmdFindNow.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindNow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindNow.Location = New System.Drawing.Point(648, 28)
        Me.cmdFindNow.Name = "cmdFindNow"
        Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindNow.Size = New System.Drawing.Size(81, 22)
        Me.cmdFindNow.TabIndex = 15
        Me.cmdFindNow.TabStop = False
        Me.cmdFindNow.Text = "F&ind Now"
        Me.cmdFindNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindNow.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(656, 423)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 21
        Me.cmdHelp.TabStop = False
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(576, 423)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 20
        Me.cmdCancel.TabStop = False
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Enabled = False
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(496, 423)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 19
        Me.cmdOK.TabStop = False
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'stbstatus
        '
        Me.stbstatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbstatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._stbstatus_Panel1})
        Me.stbstatus.Location = New System.Drawing.Point(0, 451)
        Me.stbstatus.Name = "stbstatus"
        Me.stbstatus.ShowItemToolTips = True
        Me.stbstatus.Size = New System.Drawing.Size(734, 22)
        Me.stbstatus.TabIndex = 22
        '
        '_stbstatus_Panel1
        '
        Me._stbstatus_Panel1.AutoSize = False
        Me._stbstatus_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._stbstatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._stbstatus_Panel1.DoubleClickEnabled = True
        Me._stbstatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._stbstatus_Panel1.Name = "_stbstatus_Panel1"
        Me._stbstatus_Panel1.Size = New System.Drawing.Size(717, 22)
        Me._stbstatus_Panel1.Tag = ""
        Me._stbstatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(210, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(639, 213)
        Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.tabMainTab.TabIndex = 23
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdTPA)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtTPA)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblClaimRef)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDPARequired)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblLossDateEndLimit)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblLossDateStartLimit)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblRegNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblSSYesNo)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblClientName)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblRegistration)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdPolicy)
        Me._tabMainTab_TabPage0.Controls.Add(Me.CmdClient)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtPolicyHolder)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtPolicy)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtClaimRef)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkDPARequired)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtRegNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtclaimstartdate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtclaimenddate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.ChkCLosedClaim)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtClientName)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtInsurer)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtAccountExec)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdInsurer)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAccountExec)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtRegistration)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(631, 187)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - General"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'cmdTPA
        '
        Me.cmdTPA.BackColor = System.Drawing.SystemColors.Control
        Me.cmdTPA.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdTPA.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTPA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdTPA.Location = New System.Drawing.Point(16, 116)
        Me.cmdTPA.Name = "cmdTPA"
        Me.cmdTPA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdTPA.Size = New System.Drawing.Size(80, 19)
        Me.cmdTPA.TabIndex = 33
        Me.cmdTPA.Text = "TPA"
        Me.cmdTPA.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdTPA.UseVisualStyleBackColor = False
        '
        'txtTPA
        '
        Me.txtTPA.AcceptsReturn = True
        Me.txtTPA.BackColor = System.Drawing.SystemColors.Window
        Me.txtTPA.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTPA.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTPA.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTPA.Location = New System.Drawing.Point(120, 116)
        Me.txtTPA.MaxLength = 0
        Me.txtTPA.Name = "txtTPA"
        Me.txtTPA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTPA.Size = New System.Drawing.Size(137, 21)
        Me.txtTPA.TabIndex = 34
        '
        'lblClaimRef
        '
        Me.lblClaimRef.AutoSize = True
        Me.lblClaimRef.BackColor = System.Drawing.SystemColors.Control
        Me.lblClaimRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClaimRef.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClaimRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClaimRef.Location = New System.Drawing.Point(8, 44)
        Me.lblClaimRef.Name = "lblClaimRef"
        Me.lblClaimRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClaimRef.Size = New System.Drawing.Size(107, 13)
        Me.lblClaimRef.TabIndex = 24
        Me.lblClaimRef.Text = "&Claim Reference:"
        '
        'lblDPARequired
        '
        Me.lblDPARequired.BackColor = System.Drawing.SystemColors.Control
        Me.lblDPARequired.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDPARequired.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDPARequired.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDPARequired.Location = New System.Drawing.Point(144, 18)
        Me.lblDPARequired.Name = "lblDPARequired"
        Me.lblDPARequired.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDPARequired.Size = New System.Drawing.Size(53, 21)
        Me.lblDPARequired.TabIndex = 26
        Me.lblDPARequired.Text = "No"
        '
        'lblLossDateEndLimit
        '
        Me.lblLossDateEndLimit.AutoSize = True
        Me.lblLossDateEndLimit.BackColor = System.Drawing.SystemColors.Control
        Me.lblLossDateEndLimit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLossDateEndLimit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLossDateEndLimit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLossDateEndLimit.Location = New System.Drawing.Point(336, 68)
        Me.lblLossDateEndLimit.Name = "lblLossDateEndLimit"
        Me.lblLossDateEndLimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLossDateEndLimit.Size = New System.Drawing.Size(124, 13)
        Me.lblLossDateEndLimit.TabIndex = 28
        Me.lblLossDateEndLimit.Text = "Loss Date End Limit:"
        '
        'lblLossDateStartLimit
        '
        Me.lblLossDateStartLimit.AutoSize = True
        Me.lblLossDateStartLimit.BackColor = System.Drawing.SystemColors.Control
        Me.lblLossDateStartLimit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLossDateStartLimit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLossDateStartLimit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLossDateStartLimit.Location = New System.Drawing.Point(329, 44)
        Me.lblLossDateStartLimit.Name = "lblLossDateStartLimit"
        Me.lblLossDateStartLimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLossDateStartLimit.Size = New System.Drawing.Size(131, 13)
        Me.lblLossDateStartLimit.TabIndex = 27
        Me.lblLossDateStartLimit.Text = "Loss Date Start Limit:"
        '
        'lblRegNumber
        '
        Me.lblRegNumber.AutoSize = True
        Me.lblRegNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblRegNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRegNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRegNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRegNumber.Location = New System.Drawing.Point(336, 116)
        Me.lblRegNumber.Name = "lblRegNumber"
        Me.lblRegNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRegNumber.Size = New System.Drawing.Size(73, 13)
        Me.lblRegNumber.TabIndex = 30
        Me.lblRegNumber.Text = "Risk Index:"
        '
        'lblSSYesNo
        '
        Me.lblSSYesNo.AutoSize = True
        Me.lblSSYesNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblSSYesNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSSYesNo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSSYesNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSSYesNo.Location = New System.Drawing.Point(336, 164)
        Me.lblSSYesNo.Name = "lblSSYesNo"
        Me.lblSSYesNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSSYesNo.Size = New System.Drawing.Size(141, 13)
        Me.lblSSYesNo.TabIndex = 32
        Me.lblSSYesNo.Text = "Include Closed Claims?"
        '
        'lblClientName
        '
        Me.lblClientName.AutoSize = True
        Me.lblClientName.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientName.Location = New System.Drawing.Point(336, 92)
        Me.lblClientName.Name = "lblClientName"
        Me.lblClientName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientName.Size = New System.Drawing.Size(82, 13)
        Me.lblClientName.TabIndex = 29
        Me.lblClientName.Text = "Client Name:"
        '
        'lblRegistration
        '
        Me.lblRegistration.AutoSize = True
        Me.lblRegistration.BackColor = System.Drawing.SystemColors.Control
        Me.lblRegistration.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRegistration.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRegistration.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRegistration.Location = New System.Drawing.Point(336, 140)
        Me.lblRegistration.Name = "lblRegistration"
        Me.lblRegistration.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRegistration.Size = New System.Drawing.Size(174, 13)
        Me.lblRegistration.TabIndex = 31
        Me.lblRegistration.Text = "Vehicle Registration Number:"
        '
        'cmdPolicy
        '
        Me.cmdPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPolicy.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPolicy.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPolicy.Location = New System.Drawing.Point(16, 68)
        Me.cmdPolicy.Name = "cmdPolicy"
        Me.cmdPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPolicy.Size = New System.Drawing.Size(80, 19)
        Me.cmdPolicy.TabIndex = 1
        Me.cmdPolicy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPolicy.UseVisualStyleBackColor = False
        '
        'CmdClient
        '
        Me.CmdClient.BackColor = System.Drawing.SystemColors.Control
        Me.CmdClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdClient.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdClient.Location = New System.Drawing.Point(16, 92)
        Me.CmdClient.Name = "CmdClient"
        Me.CmdClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdClient.Size = New System.Drawing.Size(80, 19)
        Me.CmdClient.TabIndex = 3
        Me.CmdClient.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.CmdClient.UseVisualStyleBackColor = False
        '
        'txtPolicyHolder
        '
        Me.txtPolicyHolder.AcceptsReturn = True
        Me.txtPolicyHolder.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolicyHolder.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicyHolder.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicyHolder.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicyHolder.Location = New System.Drawing.Point(120, 92)
        Me.txtPolicyHolder.MaxLength = 0
        Me.txtPolicyHolder.Name = "txtPolicyHolder"
        Me.txtPolicyHolder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicyHolder.Size = New System.Drawing.Size(137, 21)
        Me.txtPolicyHolder.TabIndex = 4
        '
        'txtPolicy
        '
        Me.txtPolicy.AcceptsReturn = True
        Me.txtPolicy.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolicy.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicy.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicy.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicy.Location = New System.Drawing.Point(120, 68)
        Me.txtPolicy.MaxLength = 30
        Me.txtPolicy.Name = "txtPolicy"
        Me.txtPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicy.Size = New System.Drawing.Size(201, 21)
        Me.txtPolicy.TabIndex = 2
        '
        'txtClaimRef
        '
        Me.txtClaimRef.AcceptsReturn = True
        Me.txtClaimRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtClaimRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClaimRef.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClaimRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClaimRef.Location = New System.Drawing.Point(120, 44)
        Me.txtClaimRef.MaxLength = 30
        Me.txtClaimRef.Name = "txtClaimRef"
        Me.txtClaimRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClaimRef.Size = New System.Drawing.Size(201, 21)
        Me.txtClaimRef.TabIndex = 0
        '
        'chkDPARequired
        '
        Me.chkDPARequired.BackColor = System.Drawing.SystemColors.Control
        Me.chkDPARequired.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDPARequired.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDPARequired.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDPARequired.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDPARequired.Location = New System.Drawing.Point(8, 12)
        Me.chkDPARequired.Name = "chkDPARequired"
        Me.chkDPARequired.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDPARequired.Size = New System.Drawing.Size(133, 25)
        Me.chkDPARequired.TabIndex = 25
        Me.chkDPARequired.Text = "DPA Info Required?"
        Me.chkDPARequired.UseVisualStyleBackColor = False
        '
        'txtRegNumber
        '
        Me.txtRegNumber.AcceptsReturn = True
        Me.txtRegNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtRegNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRegNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRegNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRegNumber.Location = New System.Drawing.Point(423, 116)
        Me.txtRegNumber.MaxLength = 30
        Me.txtRegNumber.Name = "txtRegNumber"
        Me.txtRegNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRegNumber.Size = New System.Drawing.Size(198, 21)
        Me.txtRegNumber.TabIndex = 8
        '
        'txtclaimstartdate
        '
        Me.txtclaimstartdate.AcceptsReturn = True
        Me.txtclaimstartdate.BackColor = System.Drawing.SystemColors.Window
        Me.txtclaimstartdate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtclaimstartdate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtclaimstartdate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtclaimstartdate.Location = New System.Drawing.Point(461, 41)
        Me.txtclaimstartdate.MaxLength = 0
        Me.txtclaimstartdate.Name = "txtclaimstartdate"
        Me.txtclaimstartdate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtclaimstartdate.Size = New System.Drawing.Size(161, 21)
        Me.txtclaimstartdate.TabIndex = 5
        '
        'txtclaimenddate
        '
        Me.txtclaimenddate.AcceptsReturn = True
        Me.txtclaimenddate.BackColor = System.Drawing.SystemColors.Window
        Me.txtclaimenddate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtclaimenddate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtclaimenddate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtclaimenddate.Location = New System.Drawing.Point(461, 68)
        Me.txtclaimenddate.MaxLength = 0
        Me.txtclaimenddate.Name = "txtclaimenddate"
        Me.txtclaimenddate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtclaimenddate.Size = New System.Drawing.Size(161, 21)
        Me.txtclaimenddate.TabIndex = 6
        '
        'ChkCLosedClaim
        '
        Me.ChkCLosedClaim.BackColor = System.Drawing.SystemColors.Control
        Me.ChkCLosedClaim.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ChkCLosedClaim.Cursor = System.Windows.Forms.Cursors.Default
        Me.ChkCLosedClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkCLosedClaim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ChkCLosedClaim.Location = New System.Drawing.Point(508, 164)
        Me.ChkCLosedClaim.Name = "ChkCLosedClaim"
        Me.ChkCLosedClaim.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ChkCLosedClaim.Size = New System.Drawing.Size(17, 17)
        Me.ChkCLosedClaim.TabIndex = 7
        Me.ChkCLosedClaim.Text = "Check1"
        Me.ChkCLosedClaim.UseVisualStyleBackColor = False
        '
        'txtClientName
        '
        Me.txtClientName.AcceptsReturn = True
        Me.txtClientName.BackColor = System.Drawing.SystemColors.Window
        Me.txtClientName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClientName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClientName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClientName.Location = New System.Drawing.Point(423, 92)
        Me.txtClientName.MaxLength = 0
        Me.txtClientName.Name = "txtClientName"
        Me.txtClientName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClientName.Size = New System.Drawing.Size(198, 21)
        Me.txtClientName.TabIndex = 14
        '
        'txtInsurer
        '
        Me.txtInsurer.AcceptsReturn = True
        Me.txtInsurer.BackColor = System.Drawing.Color.White
        Me.txtInsurer.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInsurer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInsurer.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInsurer.Location = New System.Drawing.Point(120, 140)
        Me.txtInsurer.MaxLength = 0
        Me.txtInsurer.Name = "txtInsurer"
        Me.txtInsurer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInsurer.Size = New System.Drawing.Size(137, 21)
        Me.txtInsurer.TabIndex = 11
        '
        'txtAccountExec
        '
        Me.txtAccountExec.AcceptsReturn = True
        Me.txtAccountExec.BackColor = System.Drawing.Color.White
        Me.txtAccountExec.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccountExec.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountExec.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccountExec.Location = New System.Drawing.Point(120, 164)
        Me.txtAccountExec.MaxLength = 0
        Me.txtAccountExec.Name = "txtAccountExec"
        Me.txtAccountExec.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccountExec.Size = New System.Drawing.Size(137, 21)
        Me.txtAccountExec.TabIndex = 13
        '
        'cmdInsurer
        '
        Me.cmdInsurer.BackColor = System.Drawing.SystemColors.Control
        Me.cmdInsurer.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdInsurer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdInsurer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdInsurer.Location = New System.Drawing.Point(16, 142)
        Me.cmdInsurer.Name = "cmdInsurer"
        Me.cmdInsurer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdInsurer.Size = New System.Drawing.Size(80, 19)
        Me.cmdInsurer.TabIndex = 10
        Me.cmdInsurer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdInsurer.UseVisualStyleBackColor = False
        '
        'cmdAccountExec
        '
        Me.cmdAccountExec.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAccountExec.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAccountExec.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAccountExec.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAccountExec.Location = New System.Drawing.Point(16, 164)
        Me.cmdAccountExec.Name = "cmdAccountExec"
        Me.cmdAccountExec.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAccountExec.Size = New System.Drawing.Size(80, 19)
        Me.cmdAccountExec.TabIndex = 12
        Me.cmdAccountExec.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAccountExec.UseVisualStyleBackColor = False
        '
        'txtRegistration
        '
        Me.txtRegistration.AcceptsReturn = True
        Me.txtRegistration.BackColor = System.Drawing.SystemColors.Window
        Me.txtRegistration.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRegistration.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRegistration.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRegistration.Location = New System.Drawing.Point(509, 140)
        Me.txtRegistration.MaxLength = 0
        Me.txtRegistration.Name = "txtRegistration"
        Me.txtRegistration.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRegistration.Size = New System.Drawing.Size(112, 21)
        Me.txtRegistration.TabIndex = 9
        '
        'ImgImage
        '
        Me.ImgImage.Cursor = System.Windows.Forms.Cursors.Default
        Me.ImgImage.Image = CType(resources.GetObject("ImgImage.Image"), System.Drawing.Image)
        Me.ImgImage.Location = New System.Drawing.Point(669, 104)
        Me.ImgImage.Name = "ImgImage"
        Me.ImgImage.Size = New System.Drawing.Size(32, 32)
        Me.ImgImage.TabIndex = 24
        Me.ImgImage.TabStop = False
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdFindNow
        Me.AutoScaleBaseSize = New System.Drawing.Size(7, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(734, 473)
        Me.Controls.Add(Me.lvwsearchdetails)
        Me.Controls.Add(Me.cmdView)
        Me.Controls.Add(Me.cmdNewSearch)
        Me.Controls.Add(Me.cmdFindNow)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.stbstatus)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.ImgImage)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(333, 130)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Find Claim"
        Me.stbstatus.ResumeLayout(False)
        Me.stbstatus.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        CType(Me.ImgImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub lvwsearchdetails_InitializeColumnKeys()
        Me._lvwsearchdetails_ColumnHeader_1.Name = ""
    End Sub
    Private WithEvents _stbstatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents cmdTPA As System.Windows.Forms.Button
    Public WithEvents txtTPA As System.Windows.Forms.TextBox
#End Region
End Class
