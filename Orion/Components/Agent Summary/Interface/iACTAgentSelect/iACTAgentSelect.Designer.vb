<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		lvwSearchDetails_InitializeColumnKeys()
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
    Public WithEvents cmdOk As System.Windows.Forms.Button
    Public WithEvents cmdSelectAll As System.Windows.Forms.Button
    Public WithEvents cmdNewSearch As System.Windows.Forms.Button
    Public WithEvents cmdFindNow As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Private WithEvents _stbStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents stbStatus As System.Windows.Forms.StatusStrip
    Private WithEvents _lvwSearchDetails_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchDetails_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchDetails_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwSearchDetails As System.Windows.Forms.ListView
    Public WithEvents imglImages As System.Windows.Forms.ImageList
    Public WithEvents ImgImage As System.Windows.Forms.PictureBox
    Private WithEvents listBoxComboBoxHelper1 As Artinsoft.VB6.Gui.ListControlHelper
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
        Me.cmdOk = New System.Windows.Forms.Button
        Me.cmdSelectAll = New System.Windows.Forms.Button
        Me.cmdNewSearch = New System.Windows.Forms.Button
        Me.cmdFindNow = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.stbStatus = New System.Windows.Forms.StatusStrip
        Me._stbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.lvwSearchDetails = New System.Windows.Forms.ListView
        Me._lvwSearchDetails_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me.imglImages = New System.Windows.Forms.ImageList(Me.components)
        Me.ImgImage = New System.Windows.Forms.PictureBox
        Me.TxtAgentCode = New System.Windows.Forms.TextBox
        Me.cmdAgentLookup = New System.Windows.Forms.Button
        Me.TxtAgentName = New System.Windows.Forms.TextBox
        Me.CheckExcludeBrokerssettledNeofComm = New System.Windows.Forms.CheckBox
        Me.lblCurrency = New System.Windows.Forms.Label
        Me.lblAgentType = New System.Windows.Forms.Label
        Me.lblBranch = New System.Windows.Forms.Label
        Me.lblName = New System.Windows.Forms.Label
        Me.lblAgentCode = New System.Windows.Forms.Label
        Me.cboCurrency = New UserControls.CurrencyLookup
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.cmbBranch = New PMLookupControl.cboPMLookup
        Me.cmbAgentType = New System.Windows.Forms.ComboBox
        Me.stbStatus.SuspendLayout()
        CType(Me.ImgImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabMainTab.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdOk
        '
        Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOk.Location = New System.Drawing.Point(313, 402)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(73, 22)
        Me.cmdOk.TabIndex = 14
        Me.cmdOk.TabStop = False
        Me.cmdOk.Text = "&OK"
        Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOk.UseVisualStyleBackColor = False
        '
        'cmdSelectAll
        '
        Me.cmdSelectAll.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelectAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelectAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSelectAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelectAll.Location = New System.Drawing.Point(10, 402)
        Me.cmdSelectAll.Name = "cmdSelectAll"
        Me.cmdSelectAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelectAll.Size = New System.Drawing.Size(73, 22)
        Me.cmdSelectAll.TabIndex = 13
        Me.cmdSelectAll.TabStop = False
        Me.cmdSelectAll.Text = "&Select All"
        Me.cmdSelectAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSelectAll.UseVisualStyleBackColor = False
        '
        'cmdNewSearch
        '
        Me.cmdNewSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewSearch.Location = New System.Drawing.Point(472, 64)
        Me.cmdNewSearch.Name = "cmdNewSearch"
        Me.cmdNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewSearch.Size = New System.Drawing.Size(81, 22)
        Me.cmdNewSearch.TabIndex = 11
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
        Me.cmdFindNow.Location = New System.Drawing.Point(472, 36)
        Me.cmdFindNow.Name = "cmdFindNow"
        Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindNow.Size = New System.Drawing.Size(81, 22)
        Me.cmdFindNow.TabIndex = 10
        Me.cmdFindNow.TabStop = False
        Me.cmdFindNow.Text = "F&ind Now"
        Me.cmdFindNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindNow.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(392, 402)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 16
        Me.cmdCancel.TabStop = False
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'stbStatus
        '
        Me.stbStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._stbStatus_Panel1})
        Me.stbStatus.Location = New System.Drawing.Point(0, 437)
        Me.stbStatus.Name = "stbStatus"
        Me.stbStatus.ShowItemToolTips = True
        Me.stbStatus.Size = New System.Drawing.Size(568, 22)
        Me.stbStatus.TabIndex = 14
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
        Me._stbStatus_Panel1.Size = New System.Drawing.Size(546, 22)
        Me._stbStatus_Panel1.Tag = ""
        Me._stbStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lvwSearchDetails
        '
        Me.lvwSearchDetails.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSearchDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwSearchDetails.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSearchDetails_ColumnHeader_1, Me._lvwSearchDetails_ColumnHeader_2, Me._lvwSearchDetails_ColumnHeader_3, Me._lvwSearchDetails_ColumnHeader_4, Me._lvwSearchDetails_ColumnHeader_5})
        Me.lvwSearchDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSearchDetails.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSearchDetails.FullRowSelect = True
        Me.lvwSearchDetails.HideSelection = False
        Me.lvwSearchDetails.LargeImageList = Me.imglImages
        Me.lvwSearchDetails.Location = New System.Drawing.Point(8, 222)
        Me.lvwSearchDetails.Name = "lvwSearchDetails"
        Me.lvwSearchDetails.Size = New System.Drawing.Size(548, 174)
        Me.lvwSearchDetails.SmallImageList = Me.imglImages
        Me.lvwSearchDetails.TabIndex = 12
        Me.lvwSearchDetails.UseCompatibleStateImageBehavior = False
        Me.lvwSearchDetails.View = System.Windows.Forms.View.Details
        '
        '_lvwSearchDetails_ColumnHeader_1
        '
        Me._lvwSearchDetails_ColumnHeader_1.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_1.Text = "Agent Code"
        Me._lvwSearchDetails_ColumnHeader_1.Width = 100
        '
        '_lvwSearchDetails_ColumnHeader_2
        '
        Me._lvwSearchDetails_ColumnHeader_2.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_2.Text = "Name"
        Me._lvwSearchDetails_ColumnHeader_2.Width = 160
        '
        '_lvwSearchDetails_ColumnHeader_3
        '
        Me._lvwSearchDetails_ColumnHeader_3.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_3.Text = "Agent Type"
        Me._lvwSearchDetails_ColumnHeader_3.Width = 160
        '
        '_lvwSearchDetails_ColumnHeader_4
        '
        Me._lvwSearchDetails_ColumnHeader_4.Text = "Currency"
        Me._lvwSearchDetails_ColumnHeader_4.Width = 160
        '
        '_lvwSearchDetails_ColumnHeader_5
        '
        Me._lvwSearchDetails_ColumnHeader_5.Text = "Sub Branch"
        '
        'imglImages
        '
        Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.imglImages.Images.SetKeyName(0, "FindImage")
        '
        'ImgImage
        '
        Me.ImgImage.Cursor = System.Windows.Forms.Cursors.Default
        Me.ImgImage.Image = CType(resources.GetObject("ImgImage.Image"), System.Drawing.Image)
        Me.ImgImage.Location = New System.Drawing.Point(480, 108)
        Me.ImgImage.Name = "ImgImage"
        Me.ImgImage.Size = New System.Drawing.Size(32, 32)
        Me.ImgImage.TabIndex = 23
        Me.ImgImage.TabStop = False
        '
        'TxtAgentCode
        '
        Me.TxtAgentCode.Location = New System.Drawing.Point(149, 10)
        Me.TxtAgentCode.Name = "TxtAgentCode"
        Me.TxtAgentCode.Size = New System.Drawing.Size(125, 21)
        Me.TxtAgentCode.TabIndex = 40
        '
        'cmdAgentLookup
        '
        Me.cmdAgentLookup.Location = New System.Drawing.Point(275, 10)
        Me.cmdAgentLookup.Name = "cmdAgentLookup"
        Me.cmdAgentLookup.Size = New System.Drawing.Size(29, 22)
        Me.cmdAgentLookup.TabIndex = 39
        Me.cmdAgentLookup.Text = "..."
        Me.cmdAgentLookup.UseVisualStyleBackColor = True
        '
        'TxtAgentName
        '
        Me.TxtAgentName.Location = New System.Drawing.Point(149, 37)
        Me.TxtAgentName.Name = "TxtAgentName"
        Me.TxtAgentName.Size = New System.Drawing.Size(156, 21)
        Me.TxtAgentName.TabIndex = 38
        '
        'CheckExcludeBrokerssettledNeofComm
        '
        Me.CheckExcludeBrokerssettledNeofComm.AutoSize = True
        Me.CheckExcludeBrokerssettledNeofComm.Location = New System.Drawing.Point(15, 147)
        Me.CheckExcludeBrokerssettledNeofComm.Name = "CheckExcludeBrokerssettledNeofComm"
        Me.CheckExcludeBrokerssettledNeofComm.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.CheckExcludeBrokerssettledNeofComm.Size = New System.Drawing.Size(241, 17)
        Me.CheckExcludeBrokerssettledNeofComm.TabIndex = 37
        Me.CheckExcludeBrokerssettledNeofComm.Text = "Exclude Brokers settled Net of Comm"
        Me.CheckExcludeBrokerssettledNeofComm.UseVisualStyleBackColor = True
        '
        'lblCurrency
        '
        Me.lblCurrency.AutoSize = True
        Me.lblCurrency.Location = New System.Drawing.Point(16, 123)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.Size = New System.Drawing.Size(65, 13)
        Me.lblCurrency.TabIndex = 34
        Me.lblCurrency.Text = "Currency:"
        '
        'lblAgentType
        '
        Me.lblAgentType.AutoSize = True
        Me.lblAgentType.Location = New System.Drawing.Point(16, 96)
        Me.lblAgentType.Name = "lblAgentType"
        Me.lblAgentType.Size = New System.Drawing.Size(77, 13)
        Me.lblAgentType.TabIndex = 28
        Me.lblAgentType.Text = "Agent Type:"
        '
        'lblBranch
        '
        Me.lblBranch.AutoSize = True
        Me.lblBranch.Location = New System.Drawing.Point(16, 69)
        Me.lblBranch.Name = "lblBranch"
        Me.lblBranch.Size = New System.Drawing.Size(78, 13)
        Me.lblBranch.TabIndex = 26
        Me.lblBranch.Text = "Sub Branch:"
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Location = New System.Drawing.Point(16, 42)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(45, 13)
        Me.lblName.TabIndex = 1
        Me.lblName.Text = "Name:"
        '
        'lblAgentCode
        '
        Me.lblAgentCode.AutoSize = True
        Me.lblAgentCode.Location = New System.Drawing.Point(16, 15)
        Me.lblAgentCode.Name = "lblAgentCode"
        Me.lblAgentCode.Size = New System.Drawing.Size(79, 13)
        Me.lblAgentCode.TabIndex = 0
        Me.lblAgentCode.Text = "Agent Code:"
        '
        'cboCurrency
        '
        Me.cboCurrency.CompanyId = 0
        Me.cboCurrency.CurrencyId = 0
        Me.cboCurrency.DefaultCurrencyId = 0
        Me.cboCurrency.FirstItem = "(ALL)"
        Me.cboCurrency.ListIndex = -1
        Me.cboCurrency.Location = New System.Drawing.Point(149, 118)
        Me.cboCurrency.Name = "cboCurrency"
        Me.cboCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actAllCurrencies
        Me.cboCurrency.Size = New System.Drawing.Size(156, 21)
        Me.cboCurrency.TabIndex = 4
        Me.cboCurrency.ToolTipText = ""
        Me.cboCurrency.WhatsThisHelpID = 0
        '
        'cmdHelp
        '
        Me.cmdHelp.Location = New System.Drawing.Point(472, 402)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.Size = New System.Drawing.Size(75, 22)
        Me.cmdHelp.TabIndex = 25
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.UseVisualStyleBackColor = True
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me.TabPage1)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 2)
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(442, 213)
        Me.tabMainTab.TabIndex = 26
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.cmbBranch)
        Me.TabPage1.Controls.Add(Me.cmbAgentType)
        Me.TabPage1.Controls.Add(Me.cboCurrency)
        Me.TabPage1.Controls.Add(Me.TxtAgentCode)
        Me.TabPage1.Controls.Add(Me.lblAgentCode)
        Me.TabPage1.Controls.Add(Me.lblName)
        Me.TabPage1.Controls.Add(Me.lblBranch)
        Me.TabPage1.Controls.Add(Me.lblAgentType)
        Me.TabPage1.Controls.Add(Me.cmdAgentLookup)
        Me.TabPage1.Controls.Add(Me.lblCurrency)
        Me.TabPage1.Controls.Add(Me.TxtAgentName)
        Me.TabPage1.Controls.Add(Me.CheckExcludeBrokerssettledNeofComm)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(434, 187)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "1 – Company / Person"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'cmbBranch
        '
        Me.cmbBranch.DefaultItemId = 0
        Me.cmbBranch.FirstItem = "(ALL)"
        Me.cmbBranch.ItemId = 0
        Me.cmbBranch.ListIndex = -1
        Me.cmbBranch.Location = New System.Drawing.Point(149, 64)
        Me.cmbBranch.Name = "cmbBranch"
        Me.cmbBranch.PMLookupProductFamily = 1
        Me.cmbBranch.SingleItemId = 0
        Me.cmbBranch.Size = New System.Drawing.Size(156, 21)
        Me.cmbBranch.Sorted = True
        Me.cmbBranch.TabIndex = 48
        Me.cmbBranch.TableName = "sub_branch"
        Me.cmbBranch.ToolTipText = ""
        Me.cmbBranch.WhereClause = ""
        '
        'cmbAgentType
        '
        Me.cmbAgentType.FormattingEnabled = True
        Me.cmbAgentType.Location = New System.Drawing.Point(149, 91)
        Me.cmbAgentType.Name = "cmbAgentType"
        Me.cmbAgentType.Size = New System.Drawing.Size(156, 21)
        Me.cmbAgentType.TabIndex = 47
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdFindNow
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(568, 459)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.cmdSelectAll)
        Me.Controls.Add(Me.cmdNewSearch)
        Me.Controls.Add(Me.cmdFindNow)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.stbStatus)
        Me.Controls.Add(Me.lvwSearchDetails)
        Me.Controls.Add(Me.ImgImage)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(333, 130)
        Me.MaximizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Commission Payments - Agent Select"
        Me.stbStatus.ResumeLayout(False)
        Me.stbStatus.PerformLayout()
        CType(Me.ImgImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabMainTab.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub lvwSearchDetails_InitializeColumnKeys()
        Me._lvwSearchDetails_ColumnHeader_1.Name = ""
        Me._lvwSearchDetails_ColumnHeader_2.Name = ""
        Me._lvwSearchDetails_ColumnHeader_3.Name = ""
        Me._lvwSearchDetails_ColumnHeader_3.Name = ""
        Me._lvwSearchDetails_ColumnHeader_4.Name = ""
    End Sub
    Friend WithEvents _lvwSearchDetails_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents CheckExcludeBrokerssettledNeofComm As System.Windows.Forms.CheckBox
    Friend WithEvents lblCurrency As System.Windows.Forms.Label
    Friend WithEvents lblAgentType As System.Windows.Forms.Label
    Friend WithEvents lblBranch As System.Windows.Forms.Label
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents lblAgentCode As System.Windows.Forms.Label
    Friend WithEvents TxtAgentName As System.Windows.Forms.TextBox
    Friend WithEvents TxtAgentCode As System.Windows.Forms.TextBox
    Friend WithEvents cmdAgentLookup As System.Windows.Forms.Button
    Friend WithEvents cmdHelp As System.Windows.Forms.Button
    Friend WithEvents tabMainTab As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents cmbAgentType As System.Windows.Forms.ComboBox
    Public WithEvents cmbBranch As PMLookupControl.cboPMLookup
    Public WithEvents cboCurrency As UserControls.CurrencyLookup
    Friend WithEvents _lvwSearchDetails_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
#End Region
End Class