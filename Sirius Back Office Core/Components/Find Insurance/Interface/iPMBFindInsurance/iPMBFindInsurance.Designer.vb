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
	Public WithEvents cmdView As System.Windows.Forms.Button
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdNewSearch As System.Windows.Forms.Button
	Public WithEvents cmdFindNow As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblProduct As System.Windows.Forms.Label
	Public WithEvents lblInsReference As System.Windows.Forms.Label
	Public WithEvents lblRiskIndex As System.Windows.Forms.Label
	Public WithEvents txtInsReference As System.Windows.Forms.TextBox
	Public WithEvents cmbProduct As System.Windows.Forms.ComboBox
	Public WithEvents txtRiskIndex As System.Windows.Forms.TextBox
	Public WithEvents optMTAReins As System.Windows.Forms.RadioButton
	Public WithEvents optAllTypes As System.Windows.Forms.RadioButton
	Public WithEvents optPolicy As System.Windows.Forms.RadioButton
	Public WithEvents optQuote As System.Windows.Forms.RadioButton
	Public WithEvents optRenewal As System.Windows.Forms.RadioButton
	Public WithEvents optMTAQuote As System.Windows.Forms.RadioButton
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents lblType As System.Windows.Forms.Label
	Public WithEvents lblShortName As System.Windows.Forms.Label
	Public WithEvents cmdRelatedPartyFind As System.Windows.Forms.Button
	Public WithEvents cmbType As System.Windows.Forms.ComboBox
	Public WithEvents txtShortName As System.Windows.Forms.TextBox
	Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents lblRegistrationNumber As System.Windows.Forms.Label
	Public WithEvents txtRegistrationNumber As System.Windows.Forms.TextBox
	Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Private WithEvents _stbStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents stbStatus As System.Windows.Forms.StatusStrip
	Private WithEvents _lvwSearchDetails_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwSearchDetails As System.Windows.Forms.ListView
	Public WithEvents imglImages As System.Windows.Forms.ImageList
	Public WithEvents ImgImage As System.Windows.Forms.PictureBox
	Private WithEvents listBoxComboBoxHelper1 As Artinsoft.VB6.Gui.ListControlHelper
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdView = New System.Windows.Forms.Button
        Me.cmdNavigate = New System.Windows.Forms.Button
        Me.cmdNewSearch = New System.Windows.Forms.Button
        Me.cmdFindNow = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblProduct = New System.Windows.Forms.Label
        Me.lblInsReference = New System.Windows.Forms.Label
        Me.lblRiskIndex = New System.Windows.Forms.Label
        Me.txtInsReference = New System.Windows.Forms.TextBox
        Me.cmbProduct = New System.Windows.Forms.ComboBox
        Me.txtRiskIndex = New System.Windows.Forms.TextBox
        Me.optMTAReins = New System.Windows.Forms.RadioButton
        Me.optAllTypes = New System.Windows.Forms.RadioButton
        Me.optPolicy = New System.Windows.Forms.RadioButton
        Me.optQuote = New System.Windows.Forms.RadioButton
        Me.optRenewal = New System.Windows.Forms.RadioButton
        Me.optMTAQuote = New System.Windows.Forms.RadioButton
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me.lblType = New System.Windows.Forms.Label
        Me.lblShortName = New System.Windows.Forms.Label
        Me.cmdRelatedPartyFind = New System.Windows.Forms.Button
        Me.cmbType = New System.Windows.Forms.ComboBox
        Me.txtShortName = New System.Windows.Forms.TextBox
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage
        Me.lblRegistrationNumber = New System.Windows.Forms.Label
        Me.txtRegistrationNumber = New System.Windows.Forms.TextBox
        Me.stbStatus = New System.Windows.Forms.StatusStrip
        Me._stbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.lvwSearchDetails = New System.Windows.Forms.ListView
        Me._lvwSearchDetails_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me.imglImages = New System.Windows.Forms.ImageList(Me.components)
        Me.ImgImage = New System.Windows.Forms.PictureBox
        Me.listBoxComboBoxHelper1 = New Artinsoft.VB6.Gui.ListControlHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me.stbStatus.SuspendLayout()
        CType(Me.ImgImage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdView
        '
        Me.cmdView.BackColor = System.Drawing.SystemColors.Control
        Me.cmdView.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdView.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdView.Location = New System.Drawing.Point(88, 360)
        Me.cmdView.Name = "cmdView"
        Me.cmdView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdView.Size = New System.Drawing.Size(73, 22)
        Me.cmdView.TabIndex = 14
        Me.cmdView.TabStop = False
        Me.cmdView.Text = "&View"
        Me.cmdView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdView.UseVisualStyleBackColor = False
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(8, 360)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 13
        Me.cmdNavigate.TabStop = False
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        Me.cmdNavigate.Visible = False
        '
        'cmdNewSearch
        '
        Me.cmdNewSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewSearch.Location = New System.Drawing.Point(472, 56)
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
        Me.cmdFindNow.Enabled = False
        Me.cmdFindNow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindNow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindNow.Location = New System.Drawing.Point(472, 32)
        Me.cmdFindNow.Name = "cmdFindNow"
        Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindNow.Size = New System.Drawing.Size(81, 22)
        Me.cmdFindNow.TabIndex = 10
        Me.cmdFindNow.TabStop = False
        Me.cmdFindNow.Text = "F&ind Now"
        Me.cmdFindNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindNow.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(482, 360)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 17
        Me.cmdHelp.TabStop = False
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
        Me.cmdCancel.Location = New System.Drawing.Point(402, 360)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 16
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
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(322, 360)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 15
        Me.cmdOK.TabStop = False
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage1)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage2)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(151, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 12)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(461, 166)
        Me.tabMainTab.TabIndex = 18
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblProduct)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblInsReference)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblRiskIndex)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtInsReference)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmbProduct)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtRiskIndex)
        Me._tabMainTab_TabPage0.Controls.Add(Me.optMTAReins)
        Me._tabMainTab_TabPage0.Controls.Add(Me.optAllTypes)
        Me._tabMainTab_TabPage0.Controls.Add(Me.optPolicy)
        Me._tabMainTab_TabPage0.Controls.Add(Me.optQuote)
        Me._tabMainTab_TabPage0.Controls.Add(Me.optRenewal)
        Me._tabMainTab_TabPage0.Controls.Add(Me.optMTAQuote)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(453, 140)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "&1 - Policy"
        '
        'lblProduct
        '
        Me.lblProduct.BackColor = System.Drawing.SystemColors.Control
        Me.lblProduct.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProduct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProduct.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProduct.Location = New System.Drawing.Point(16, 87)
        Me.lblProduct.Name = "lblProduct"
        Me.lblProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProduct.Size = New System.Drawing.Size(89, 17)
        Me.lblProduct.TabIndex = 17
        Me.lblProduct.Text = "Pro&duct Group:"
        '
        'lblInsReference
        '
        Me.lblInsReference.BackColor = System.Drawing.SystemColors.Control
        Me.lblInsReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInsReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInsReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInsReference.Location = New System.Drawing.Point(16, 23)
        Me.lblInsReference.Name = "lblInsReference"
        Me.lblInsReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInsReference.Size = New System.Drawing.Size(89, 17)
        Me.lblInsReference.TabIndex = 18
        Me.lblInsReference.Text = "&Reference:"
        '
        'lblRiskIndex
        '
        Me.lblRiskIndex.BackColor = System.Drawing.SystemColors.Control
        Me.lblRiskIndex.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRiskIndex.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRiskIndex.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRiskIndex.Location = New System.Drawing.Point(16, 55)
        Me.lblRiskIndex.Name = "lblRiskIndex"
        Me.lblRiskIndex.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRiskIndex.Size = New System.Drawing.Size(89, 17)
        Me.lblRiskIndex.TabIndex = 19
        Me.lblRiskIndex.Text = "Risk &Index:"
        '
        'txtInsReference
        '
        Me.txtInsReference.AcceptsReturn = True
        Me.txtInsReference.BackColor = System.Drawing.SystemColors.Window
        Me.txtInsReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInsReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInsReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInsReference.Location = New System.Drawing.Point(104, 20)
        Me.txtInsReference.MaxLength = 30
        Me.txtInsReference.Name = "txtInsReference"
        Me.txtInsReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInsReference.Size = New System.Drawing.Size(217, 20)
        Me.txtInsReference.TabIndex = 0
        '
        'cmbProduct
        '
        Me.cmbProduct.BackColor = System.Drawing.SystemColors.Window
        Me.cmbProduct.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbProduct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbProduct.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cmbProduct, New Integer(-1) {})
        Me.cmbProduct.Location = New System.Drawing.Point(104, 84)
        Me.cmbProduct.Name = "cmbProduct"
        Me.cmbProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbProduct.Size = New System.Drawing.Size(217, 21)
        Me.cmbProduct.TabIndex = 2
        '
        'txtRiskIndex
        '
        Me.txtRiskIndex.AcceptsReturn = True
        Me.txtRiskIndex.BackColor = System.Drawing.SystemColors.Window
        Me.txtRiskIndex.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRiskIndex.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRiskIndex.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRiskIndex.Location = New System.Drawing.Point(104, 52)
        Me.txtRiskIndex.MaxLength = 30
        Me.txtRiskIndex.Name = "txtRiskIndex"
        Me.txtRiskIndex.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRiskIndex.Size = New System.Drawing.Size(217, 20)
        Me.txtRiskIndex.TabIndex = 1
        '
        'optMTAReins
        '
        Me.optMTAReins.BackColor = System.Drawing.SystemColors.Control
        Me.optMTAReins.Cursor = System.Windows.Forms.Cursors.Default
        Me.optMTAReins.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optMTAReins.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optMTAReins.Location = New System.Drawing.Point(336, 100)
        Me.optMTAReins.Name = "optMTAReins"
        Me.optMTAReins.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optMTAReins.Size = New System.Drawing.Size(113, 21)
        Me.optMTAReins.TabIndex = 9
        Me.optMTAReins.TabStop = True
        Me.optMTAReins.Text = "MT&R Quote"
        Me.optMTAReins.UseVisualStyleBackColor = False
        '
        'optAllTypes
        '
        Me.optAllTypes.BackColor = System.Drawing.SystemColors.Control
        Me.optAllTypes.Checked = True
        Me.optAllTypes.Cursor = System.Windows.Forms.Cursors.Default
        Me.optAllTypes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optAllTypes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optAllTypes.Location = New System.Drawing.Point(336, 12)
        Me.optAllTypes.Name = "optAllTypes"
        Me.optAllTypes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optAllTypes.Size = New System.Drawing.Size(113, 21)
        Me.optAllTypes.TabIndex = 4
        Me.optAllTypes.TabStop = True
        Me.optAllTypes.Text = "&All Types"
        Me.optAllTypes.UseVisualStyleBackColor = False
        '
        'optPolicy
        '
        Me.optPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.optPolicy.Cursor = System.Windows.Forms.Cursors.Default
        Me.optPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optPolicy.Location = New System.Drawing.Point(336, 65)
        Me.optPolicy.Name = "optPolicy"
        Me.optPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optPolicy.Size = New System.Drawing.Size(113, 21)
        Me.optPolicy.TabIndex = 7
        Me.optPolicy.TabStop = True
        Me.optPolicy.Text = "P&olicy"
        Me.optPolicy.UseVisualStyleBackColor = False
        '
        'optQuote
        '
        Me.optQuote.BackColor = System.Drawing.SystemColors.Control
        Me.optQuote.Cursor = System.Windows.Forms.Cursors.Default
        Me.optQuote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optQuote.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optQuote.Location = New System.Drawing.Point(336, 30)
        Me.optQuote.Name = "optQuote"
        Me.optQuote.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optQuote.Size = New System.Drawing.Size(113, 21)
        Me.optQuote.TabIndex = 5
        Me.optQuote.TabStop = True
        Me.optQuote.Text = "NB Q&uote"
        Me.optQuote.UseVisualStyleBackColor = False
        '
        'optRenewal
        '
        Me.optRenewal.BackColor = System.Drawing.SystemColors.Control
        Me.optRenewal.Cursor = System.Windows.Forms.Cursors.Default
        Me.optRenewal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optRenewal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optRenewal.Location = New System.Drawing.Point(336, 83)
        Me.optRenewal.Name = "optRenewal"
        Me.optRenewal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optRenewal.Size = New System.Drawing.Size(113, 21)
        Me.optRenewal.TabIndex = 8
        Me.optRenewal.TabStop = True
        Me.optRenewal.Text = "R&enewal"
        Me.optRenewal.UseVisualStyleBackColor = False
        '
        'optMTAQuote
        '
        Me.optMTAQuote.BackColor = System.Drawing.SystemColors.Control
        Me.optMTAQuote.Cursor = System.Windows.Forms.Cursors.Default
        Me.optMTAQuote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optMTAQuote.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optMTAQuote.Location = New System.Drawing.Point(336, 47)
        Me.optMTAQuote.Name = "optMTAQuote"
        Me.optMTAQuote.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optMTAQuote.Size = New System.Drawing.Size(113, 21)
        Me.optMTAQuote.TabIndex = 6
        Me.optMTAQuote.TabStop = True
        Me.optMTAQuote.Text = "M&TA/MTC Quote"
        Me.optMTAQuote.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblType)
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblShortName)
        Me._tabMainTab_TabPage1.Controls.Add(Me.cmdRelatedPartyFind)
        Me._tabMainTab_TabPage1.Controls.Add(Me.cmbType)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtShortName)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(453, 140)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "&2 - Related Client"
        '
        'lblType
        '
        Me.lblType.BackColor = System.Drawing.SystemColors.Control
        Me.lblType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblType.Location = New System.Drawing.Point(16, 55)
        Me.lblType.Name = "lblType"
        Me.lblType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblType.Size = New System.Drawing.Size(73, 17)
        Me.lblType.TabIndex = 15
        Me.lblType.Text = "Type:"
        '
        'lblShortName
        '
        Me.lblShortName.BackColor = System.Drawing.SystemColors.Control
        Me.lblShortName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShortName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShortName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblShortName.Location = New System.Drawing.Point(16, 23)
        Me.lblShortName.Name = "lblShortName"
        Me.lblShortName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShortName.Size = New System.Drawing.Size(73, 17)
        Me.lblShortName.TabIndex = 16
        Me.lblShortName.Text = "Short Name:"
        '
        'cmdRelatedPartyFind
        '
        Me.cmdRelatedPartyFind.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRelatedPartyFind.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRelatedPartyFind.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRelatedPartyFind.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRelatedPartyFind.Location = New System.Drawing.Point(257, 20)
        Me.cmdRelatedPartyFind.Name = "cmdRelatedPartyFind"
        Me.cmdRelatedPartyFind.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRelatedPartyFind.Size = New System.Drawing.Size(28, 19)
        Me.cmdRelatedPartyFind.TabIndex = 20
        Me.cmdRelatedPartyFind.Text = "..."
        Me.cmdRelatedPartyFind.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRelatedPartyFind.UseVisualStyleBackColor = False
        '
        'cmbType
        '
        Me.cmbType.BackColor = System.Drawing.SystemColors.Window
        Me.cmbType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cmbType, New Integer() {0, 0})
        Me.cmbType.Items.AddRange(New Object() {"cmbType", "cmbType"})
        Me.cmbType.Location = New System.Drawing.Point(104, 52)
        Me.cmbType.Name = "cmbType"
        Me.cmbType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbType.Size = New System.Drawing.Size(113, 21)
        Me.cmbType.TabIndex = 21
        '
        'txtShortName
        '
        Me.txtShortName.AcceptsReturn = True
        Me.txtShortName.BackColor = System.Drawing.SystemColors.Window
        Me.txtShortName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtShortName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtShortName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtShortName.Location = New System.Drawing.Point(104, 20)
        Me.txtShortName.MaxLength = 0
        Me.txtShortName.Name = "txtShortName"
        Me.txtShortName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtShortName.Size = New System.Drawing.Size(153, 20)
        Me.txtShortName.TabIndex = 19
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblRegistrationNumber)
        Me._tabMainTab_TabPage2.Controls.Add(Me.txtRegistrationNumber)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(453, 140)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "&3 - Registration Number"
        '
        'lblRegistrationNumber
        '
        Me.lblRegistrationNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblRegistrationNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRegistrationNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRegistrationNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRegistrationNumber.Location = New System.Drawing.Point(32, 23)
        Me.lblRegistrationNumber.Name = "lblRegistrationNumber"
        Me.lblRegistrationNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRegistrationNumber.Size = New System.Drawing.Size(89, 17)
        Me.lblRegistrationNumber.TabIndex = 21
        Me.lblRegistrationNumber.Text = "&Registration:"
        '
        'txtRegistrationNumber
        '
        Me.txtRegistrationNumber.AcceptsReturn = True
        Me.txtRegistrationNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtRegistrationNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRegistrationNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRegistrationNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRegistrationNumber.Location = New System.Drawing.Point(120, 20)
        Me.txtRegistrationNumber.MaxLength = 30
        Me.txtRegistrationNumber.Name = "txtRegistrationNumber"
        Me.txtRegistrationNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRegistrationNumber.Size = New System.Drawing.Size(81, 20)
        Me.txtRegistrationNumber.TabIndex = 24
        '
        'stbStatus
        '
        Me.stbStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._stbStatus_Panel1})
        Me.stbStatus.Location = New System.Drawing.Point(0, 385)
        Me.stbStatus.Name = "stbStatus"
        Me.stbStatus.ShowItemToolTips = True
        Me.stbStatus.Size = New System.Drawing.Size(563, 22)
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
        Me.lvwSearchDetails.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSearchDetails_ColumnHeader_1, Me._lvwSearchDetails_ColumnHeader_2, Me._lvwSearchDetails_ColumnHeader_3, Me._lvwSearchDetails_ColumnHeader_4, Me._lvwSearchDetails_ColumnHeader_5, Me._lvwSearchDetails_ColumnHeader_6, Me._lvwSearchDetails_ColumnHeader_7})
        Me.lvwSearchDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSearchDetails.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSearchDetails.FullRowSelect = True
        Me.lvwSearchDetails.HideSelection = False
        Me.lvwSearchDetails.LargeImageList = Me.imglImages
        Me.lvwSearchDetails.Location = New System.Drawing.Point(8, 184)
        Me.lvwSearchDetails.MultiSelect = False
        Me.lvwSearchDetails.Name = "lvwSearchDetails"
        Me.lvwSearchDetails.Size = New System.Drawing.Size(547, 169)
        Me.lvwSearchDetails.SmallImageList = Me.imglImages
        Me.lvwSearchDetails.TabIndex = 12
        Me.lvwSearchDetails.UseCompatibleStateImageBehavior = False
        Me.lvwSearchDetails.View = System.Windows.Forms.View.Details
        '
        '_lvwSearchDetails_ColumnHeader_1
        '
        Me._lvwSearchDetails_ColumnHeader_1.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_1.Text = "1"
        Me._lvwSearchDetails_ColumnHeader_1.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_2
        '
        Me._lvwSearchDetails_ColumnHeader_2.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_2.Text = "2"
        Me._lvwSearchDetails_ColumnHeader_2.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_3
        '
        Me._lvwSearchDetails_ColumnHeader_3.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_3.Text = "3"
        Me._lvwSearchDetails_ColumnHeader_3.Width = 121
        '
        '_lvwSearchDetails_ColumnHeader_4
        '
        Me._lvwSearchDetails_ColumnHeader_4.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_4.Text = "4"
        Me._lvwSearchDetails_ColumnHeader_4.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_5
        '
        Me._lvwSearchDetails_ColumnHeader_5.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_5.Text = "5"
        Me._lvwSearchDetails_ColumnHeader_5.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_6
        '
        Me._lvwSearchDetails_ColumnHeader_6.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_6.Text = "6"
        Me._lvwSearchDetails_ColumnHeader_6.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_7
        '
        Me._lvwSearchDetails_ColumnHeader_7.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_7.Text = "7"
        Me._lvwSearchDetails_ColumnHeader_7.Width = 97
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
        Me.ImgImage.Location = New System.Drawing.Point(496, 104)
        Me.ImgImage.Name = "ImgImage"
        Me.ImgImage.Size = New System.Drawing.Size(32, 32)
        Me.ImgImage.TabIndex = 23
        Me.ImgImage.TabStop = False
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdFindNow
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(563, 407)
        Me.Controls.Add(Me.cmdView)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdNewSearch)
        Me.Controls.Add(Me.cmdFindNow)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.stbStatus)
        Me.Controls.Add(Me.lvwSearchDetails)
        Me.Controls.Add(Me.ImgImage)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(333, 130)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Find: Insurance File"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me._tabMainTab_TabPage1.PerformLayout()
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me._tabMainTab_TabPage2.PerformLayout()
        Me.stbStatus.ResumeLayout(False)
        Me.stbStatus.PerformLayout()
        CType(Me.ImgImage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub lvwSearchDetails_InitializeColumnKeys()
		Me._lvwSearchDetails_ColumnHeader_1.Name = ""
		Me._lvwSearchDetails_ColumnHeader_2.Name = ""
		Me._lvwSearchDetails_ColumnHeader_3.Name = ""
		Me._lvwSearchDetails_ColumnHeader_4.Name = ""
		Me._lvwSearchDetails_ColumnHeader_5.Name = ""
		Me._lvwSearchDetails_ColumnHeader_6.Name = ""
		Me._lvwSearchDetails_ColumnHeader_7.Name = ""
	End Sub
#End Region 
End Class