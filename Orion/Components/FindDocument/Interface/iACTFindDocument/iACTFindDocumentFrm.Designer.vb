<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
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
	Public WithEvents uctAnchor As uSIRCommonControls.uctAnchor
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdReverse As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdNew As System.Windows.Forms.Button
	Public WithEvents cmdNewSearch As System.Windows.Forms.Button
	Public WithEvents cmdFindNow As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents lblStatus As System.Windows.Forms.Label
	Public WithEvents lblDocumentRef As System.Windows.Forms.Label
	Public WithEvents lblType As System.Windows.Forms.Label
	Public WithEvents lblDateTo As System.Windows.Forms.Label
	Public WithEvents lblDateFrom As System.Windows.Forms.Label
	Public WithEvents lblBranch As System.Windows.Forms.Label
	Public WithEvents cmbStatus As System.Windows.Forms.ComboBox
	Public WithEvents txtDocumentRef As System.Windows.Forms.TextBox
	Public WithEvents txtDateFrom As System.Windows.Forms.TextBox
	Public WithEvents txtDateTo As System.Windows.Forms.TextBox
	Public WithEvents cmbType As System.Windows.Forms.ComboBox
	Public WithEvents cboSource As System.Windows.Forms.ComboBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Private WithEvents _stbStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents stbStatus As System.Windows.Forms.StatusStrip
	Private WithEvents _lvwSearchDetails_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwSearchDetails As System.Windows.Forms.ListView
	Public WithEvents imglImages As System.Windows.Forms.ImageList
	Public WithEvents ImgImage As System.Windows.Forms.PictureBox
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.uctAnchor = New uSIRCommonControls.uctAnchor
		Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
		Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
		Me.dlgHelpFont = New System.Windows.Forms.FontDialog
		Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
		Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
		Me.cmdReverse = New System.Windows.Forms.Button
		Me.cmdEdit = New System.Windows.Forms.Button
		Me.cmdNew = New System.Windows.Forms.Button
		Me.cmdNewSearch = New System.Windows.Forms.Button
		Me.cmdFindNow = New System.Windows.Forms.Button
		Me.cmdHelp = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOk = New System.Windows.Forms.Button
		Me.tabMainTab = New System.Windows.Forms.TabControl
		Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
		Me.lblStatus = New System.Windows.Forms.Label
		Me.lblDocumentRef = New System.Windows.Forms.Label
		Me.lblType = New System.Windows.Forms.Label
		Me.lblDateTo = New System.Windows.Forms.Label
		Me.lblDateFrom = New System.Windows.Forms.Label
		Me.lblBranch = New System.Windows.Forms.Label
		Me.cmbStatus = New System.Windows.Forms.ComboBox
		Me.txtDocumentRef = New System.Windows.Forms.TextBox
		Me.txtDateFrom = New System.Windows.Forms.TextBox
		Me.txtDateTo = New System.Windows.Forms.TextBox
		Me.cmbType = New System.Windows.Forms.ComboBox
		Me.cboSource = New System.Windows.Forms.ComboBox
		Me.stbStatus = New System.Windows.Forms.StatusStrip
		Me._stbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
		Me.lvwSearchDetails = New System.Windows.Forms.ListView
		Me._lvwSearchDetails_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetails_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetails_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetails_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetails_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetails_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
		Me.imglImages = New System.Windows.Forms.ImageList
		Me.ImgImage = New System.Windows.Forms.PictureBox
		Me.tabMainTab.SuspendLayout()
		Me._tabMainTab_TabPage0.SuspendLayout()
		Me.stbStatus.SuspendLayout()
		Me.lvwSearchDetails.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' uctAnchor
		' 
		Me.uctAnchor.Location = New System.Drawing.Point(342, 314)
		Me.uctAnchor.Name = "uctAnchor"
		' 
		' cmdReverse
		' 
		Me.cmdReverse.BackColor = System.Drawing.SystemColors.Control
		Me.cmdReverse.CausesValidation = True
		Me.cmdReverse.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdReverse.Enabled = False
		Me.cmdReverse.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdReverse.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdReverse.Location = New System.Drawing.Point(168, 312)
		Me.cmdReverse.Name = "cmdReverse"
		Me.cmdReverse.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdReverse.Size = New System.Drawing.Size(73, 22)
		Me.cmdReverse.TabIndex = 11
		Me.cmdReverse.TabStop = True
		Me.cmdReverse.Text = "&Reverse"
		Me.cmdReverse.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdReverse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdEdit
		' 
		Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdEdit.CausesValidation = True
		Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdEdit.Enabled = False
		Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdEdit.Location = New System.Drawing.Point(88, 312)
		Me.cmdEdit.Name = "cmdEdit"
		Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
		Me.cmdEdit.TabIndex = 7
		Me.cmdEdit.TabStop = True
		Me.cmdEdit.Text = "&Edit"
		Me.cmdEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdNew
		' 
		Me.cmdNew.BackColor = System.Drawing.SystemColors.Control
		Me.cmdNew.CausesValidation = True
		Me.cmdNew.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdNew.Enabled = True
		Me.cmdNew.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdNew.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNew.Location = New System.Drawing.Point(8, 312)
		Me.cmdNew.Name = "cmdNew"
		Me.cmdNew.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNew.Size = New System.Drawing.Size(73, 22)
		Me.cmdNew.TabIndex = 6
		Me.cmdNew.TabStop = True
		Me.cmdNew.Text = "N&ew"
		Me.cmdNew.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdNewSearch
		' 
		Me.cmdNewSearch.BackColor = System.Drawing.SystemColors.Control
		Me.cmdNewSearch.CausesValidation = True
		Me.cmdNewSearch.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdNewSearch.Enabled = True
		Me.cmdNewSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdNewSearch.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNewSearch.Location = New System.Drawing.Point(586, 56)
		Me.cmdNewSearch.Name = "cmdNewSearch"
		Me.cmdNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNewSearch.Size = New System.Drawing.Size(81, 23)
		Me.cmdNewSearch.TabIndex = 5
		Me.cmdNewSearch.TabStop = True
		Me.cmdNewSearch.Text = "Ne&w Search"
		Me.cmdNewSearch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdNewSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdFindNow
		' 
		Me.cmdFindNow.BackColor = System.Drawing.SystemColors.Control
		Me.cmdFindNow.CausesValidation = True
		Me.cmdFindNow.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdFindNow.Enabled = False
		Me.cmdFindNow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdFindNow.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdFindNow.Location = New System.Drawing.Point(586, 28)
		Me.cmdFindNow.Name = "cmdFindNow"
		Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdFindNow.Size = New System.Drawing.Size(81, 23)
		Me.cmdFindNow.TabIndex = 4
		Me.cmdFindNow.TabStop = True
		Me.cmdFindNow.Text = "F&ind Now"
		Me.cmdFindNow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdFindNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdHelp
		' 
		Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
		Me.cmdHelp.CausesValidation = True
		Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdHelp.Enabled = True
		Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdHelp.Location = New System.Drawing.Point(596, 312)
		Me.cmdHelp.Name = "cmdHelp"
		Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.cmdHelp.TabIndex = 10
		Me.cmdHelp.TabStop = True
		Me.cmdHelp.Text = "&Help"
		Me.cmdHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(516, 312)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 9
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOk
		' 
		Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOk.CausesValidation = True
		Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOk.Enabled = False
		Me.cmdOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOk.Location = New System.Drawing.Point(436, 312)
		Me.cmdOk.Name = "cmdOk"
		Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOk.Size = New System.Drawing.Size(73, 22)
		Me.cmdOk.TabIndex = 8
		Me.cmdOk.TabStop = True
		Me.cmdOk.Text = "&OK"
		Me.cmdOk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tabMainTab
		' 
		Me.tabMainTab.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabMainTab.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
		Me.tabMainTab.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMainTab.ItemSize = New System.Drawing.Size(187, 18)
		Me.tabMainTab.Location = New System.Drawing.Point(10, 8)
		Me.tabMainTab.Multiline = True
		Me.tabMainTab.Name = "tabMainTab"
		Me.tabMainTab.Size = New System.Drawing.Size(569, 134)
		Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMainTab.TabIndex = 12
		' 
		' _tabMainTab_TabPage0
		' 
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblStatus)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblDocumentRef)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblType)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblDateTo)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblDateFrom)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblBranch)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmbStatus)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtDocumentRef)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtDateFrom)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtDateTo)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmbType)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cboSource)
		Me._tabMainTab_TabPage0.Text = " &1 - Details"
		' 
		' lblStatus
		' 
		Me.lblStatus.AutoSize = True
		Me.lblStatus.BackColor = System.Drawing.SystemColors.Control
		Me.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblStatus.Enabled = True
		Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblStatus.Location = New System.Drawing.Point(16, 80)
		Me.lblStatus.Name = "lblStatus"
		Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblStatus.Size = New System.Drawing.Size(41, 13)
		Me.lblStatus.TabIndex = 14
		Me.lblStatus.Text = "&Status:"
		Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblStatus.UseMnemonic = True
		Me.lblStatus.Visible = True
		' 
		' lblDocumentRef
		' 
		Me.lblDocumentRef.AutoSize = False
		Me.lblDocumentRef.BackColor = System.Drawing.SystemColors.Control
		Me.lblDocumentRef.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDocumentRef.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDocumentRef.Enabled = True
		Me.lblDocumentRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDocumentRef.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDocumentRef.Location = New System.Drawing.Point(16, 15)
		Me.lblDocumentRef.Name = "lblDocumentRef"
		Me.lblDocumentRef.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDocumentRef.Size = New System.Drawing.Size(73, 17)
		Me.lblDocumentRef.TabIndex = 15
		Me.lblDocumentRef.Text = "&Reference:"
		Me.lblDocumentRef.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDocumentRef.UseMnemonic = True
		Me.lblDocumentRef.Visible = True
		' 
		' lblType
		' 
		Me.lblType.AutoSize = True
		Me.lblType.BackColor = System.Drawing.SystemColors.Control
		Me.lblType.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblType.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblType.Enabled = True
		Me.lblType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblType.Location = New System.Drawing.Point(16, 48)
		Me.lblType.Name = "lblType"
		Me.lblType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblType.Size = New System.Drawing.Size(33, 13)
		Me.lblType.TabIndex = 16
		Me.lblType.Text = "T&ype:"
		Me.lblType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblType.UseMnemonic = True
		Me.lblType.Visible = True
		' 
		' lblDateTo
		' 
		Me.lblDateTo.AutoSize = True
		Me.lblDateTo.BackColor = System.Drawing.SystemColors.Control
		Me.lblDateTo.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDateTo.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDateTo.Enabled = True
		Me.lblDateTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDateTo.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDateTo.Location = New System.Drawing.Point(232, 46)
		Me.lblDateTo.Name = "lblDateTo"
		Me.lblDateTo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDateTo.Size = New System.Drawing.Size(50, 13)
		Me.lblDateTo.TabIndex = 17
		Me.lblDateTo.Text = "Date &To:"
		Me.lblDateTo.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDateTo.UseMnemonic = True
		Me.lblDateTo.Visible = True
		' 
		' lblDateFrom
		' 
		Me.lblDateFrom.AutoSize = True
		Me.lblDateFrom.BackColor = System.Drawing.SystemColors.Control
		Me.lblDateFrom.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDateFrom.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDateFrom.Enabled = True
		Me.lblDateFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDateFrom.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDateFrom.Location = New System.Drawing.Point(232, 15)
		Me.lblDateFrom.Name = "lblDateFrom"
		Me.lblDateFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDateFrom.Size = New System.Drawing.Size(65, 13)
		Me.lblDateFrom.TabIndex = 18
		Me.lblDateFrom.Text = "Date &From:"
		Me.lblDateFrom.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDateFrom.UseMnemonic = True
		Me.lblDateFrom.Visible = True
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
		Me.lblBranch.Location = New System.Drawing.Point(232, 80)
		Me.lblBranch.Name = "lblBranch"
		Me.lblBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblBranch.Size = New System.Drawing.Size(45, 13)
		Me.lblBranch.TabIndex = 21
		Me.lblBranch.Text = "&Branch:"
		Me.lblBranch.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblBranch.UseMnemonic = True
		Me.lblBranch.Visible = True
		' 
		' cmbStatus
		' 
		Me.cmbStatus.BackColor = System.Drawing.SystemColors.Window
		Me.cmbStatus.CausesValidation = True
		Me.cmbStatus.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cmbStatus.Enabled = True
		Me.cmbStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmbStatus.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cmbStatus.IntegralHeight = True
		Me.cmbStatus.Location = New System.Drawing.Point(88, 76)
		Me.cmbStatus.Name = "cmbStatus"
		Me.cmbStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmbStatus.Size = New System.Drawing.Size(121, 21)
		Me.cmbStatus.Sorted = True
		Me.cmbStatus.TabIndex = 2
		Me.cmbStatus.TabStop = True
		Me.cmbStatus.Visible = True
		' 
		' txtDocumentRef
		' 
		Me.txtDocumentRef.AcceptsReturn = True
		Me.txtDocumentRef.AutoSize = False
		Me.txtDocumentRef.BackColor = System.Drawing.SystemColors.Window
		Me.txtDocumentRef.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDocumentRef.CausesValidation = True
		Me.txtDocumentRef.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDocumentRef.Enabled = True
		Me.txtDocumentRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDocumentRef.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDocumentRef.HideSelection = True
		Me.txtDocumentRef.Location = New System.Drawing.Point(88, 12)
		Me.txtDocumentRef.MaxLength = 0
		Me.txtDocumentRef.Multiline = False
		Me.txtDocumentRef.Name = "txtDocumentRef"
		Me.txtDocumentRef.ReadOnly = False
		Me.txtDocumentRef.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDocumentRef.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDocumentRef.Size = New System.Drawing.Size(121, 19)
		Me.txtDocumentRef.TabIndex = 1
		Me.txtDocumentRef.TabStop = True
		Me.txtDocumentRef.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDocumentRef.Visible = True
		' 
		' txtDateFrom
		' 
		Me.txtDateFrom.AcceptsReturn = True
		Me.txtDateFrom.AutoSize = False
		Me.txtDateFrom.BackColor = System.Drawing.SystemColors.Window
		Me.txtDateFrom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDateFrom.CausesValidation = True
		Me.txtDateFrom.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDateFrom.Enabled = True
		Me.txtDateFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDateFrom.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDateFrom.HideSelection = True
		Me.txtDateFrom.Location = New System.Drawing.Point(304, 12)
		Me.txtDateFrom.MaxLength = 0
		Me.txtDateFrom.Multiline = False
		Me.txtDateFrom.Name = "txtDateFrom"
		Me.txtDateFrom.ReadOnly = False
		Me.txtDateFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDateFrom.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDateFrom.Size = New System.Drawing.Size(121, 19)
		Me.txtDateFrom.TabIndex = 3
		Me.txtDateFrom.TabStop = True
		Me.txtDateFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDateFrom.Visible = True
		' 
		' txtDateTo
		' 
		Me.txtDateTo.AcceptsReturn = True
		Me.txtDateTo.AutoSize = False
		Me.txtDateTo.BackColor = System.Drawing.SystemColors.Window
		Me.txtDateTo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDateTo.CausesValidation = True
		Me.txtDateTo.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDateTo.Enabled = True
		Me.txtDateTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDateTo.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDateTo.HideSelection = True
		Me.txtDateTo.Location = New System.Drawing.Point(304, 45)
		Me.txtDateTo.MaxLength = 0
		Me.txtDateTo.Multiline = False
		Me.txtDateTo.Name = "txtDateTo"
		Me.txtDateTo.ReadOnly = False
		Me.txtDateTo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDateTo.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDateTo.Size = New System.Drawing.Size(121, 19)
		Me.txtDateTo.TabIndex = 19
		Me.txtDateTo.TabStop = True
		Me.txtDateTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDateTo.Visible = True
		' 
		' cmbType
		' 
		Me.cmbType.BackColor = System.Drawing.SystemColors.Window
		Me.cmbType.CausesValidation = True
		Me.cmbType.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cmbType.Enabled = True
		Me.cmbType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmbType.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cmbType.IntegralHeight = True
		Me.cmbType.Location = New System.Drawing.Point(88, 44)
		Me.cmbType.Name = "cmbType"
		Me.cmbType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmbType.Size = New System.Drawing.Size(121, 21)
		Me.cmbType.Sorted = True
		Me.cmbType.TabIndex = 20
		Me.cmbType.TabStop = True
		Me.cmbType.Visible = True
		' 
		' cboSource
		' 
		Me.cboSource.BackColor = System.Drawing.SystemColors.Window
		Me.cboSource.CausesValidation = True
		Me.cboSource.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboSource.Enabled = True
		Me.cboSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboSource.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboSource.IntegralHeight = True
		Me.cboSource.Location = New System.Drawing.Point(304, 76)
		Me.cboSource.Name = "cboSource"
		Me.cboSource.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboSource.Size = New System.Drawing.Size(121, 21)
		Me.cboSource.Sorted = False
		Me.cboSource.TabIndex = 22
		Me.cboSource.TabStop = True
		Me.cboSource.Visible = True
		' 
		' stbStatus
		' 
		Me.stbStatus.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.stbStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.stbStatus.Location = New System.Drawing.Point(0, 341)
		Me.stbStatus.Name = "stbStatus"
		Me.stbStatus.ShowItemToolTips = True
		Me.stbStatus.Size = New System.Drawing.Size(677, 18)
		Me.stbStatus.TabIndex = 13
		Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me._stbStatus_Panel1})
		' 
		' _stbStatus_Panel1
		' 
		Me._stbStatus_Panel1.AutoSize = True
		Me._stbStatus_Panel1.AutoSize = False
		Me._stbStatus_Panel1.BorderSides = CType(System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom, System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me._stbStatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
		Me._stbStatus_Panel1.DoubleClickEnabled = True
		Me._stbStatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
		Me._stbStatus_Panel1.Size = New System.Drawing.Size(659, 18)
		Me._stbStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._stbStatus_Panel1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
		' 
		' lvwSearchDetails
		' 
		Me.lvwSearchDetails.BackColor = System.Drawing.SystemColors.Window
		Me.lvwSearchDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lvwSearchDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwSearchDetails.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwSearchDetails.FullRowSelect = True
		Me.lvwSearchDetails.HideSelection = False
		Me.lvwSearchDetails.LabelEdit = False
		Me.lvwSearchDetails.LabelWrap = True
		Me.lvwSearchDetails.LargeImageList = imglImages
		Me.lvwSearchDetails.Location = New System.Drawing.Point(8, 144)
		Me.lvwSearchDetails.Name = "lvwSearchDetails"
		Me.lvwSearchDetails.Size = New System.Drawing.Size(659, 161)
		Me.lvwSearchDetails.SmallImageList = imglImages
		Me.lvwSearchDetails.TabIndex = 0
		Me.lvwSearchDetails.View = System.Windows.Forms.View.Details
		Me.lvwSearchDetails.Columns.Add(Me._lvwSearchDetails_ColumnHeader_1)
		Me.lvwSearchDetails.Columns.Add(Me._lvwSearchDetails_ColumnHeader_2)
		Me.lvwSearchDetails.Columns.Add(Me._lvwSearchDetails_ColumnHeader_3)
		Me.lvwSearchDetails.Columns.Add(Me._lvwSearchDetails_ColumnHeader_4)
		Me.lvwSearchDetails.Columns.Add(Me._lvwSearchDetails_ColumnHeader_5)
		Me.lvwSearchDetails.Columns.Add(Me._lvwSearchDetails_ColumnHeader_6)
		' 
		' _lvwSearchDetails_ColumnHeader_1
		' 
		Me._lvwSearchDetails_ColumnHeader_1.Text = "Doc Ref"
		Me._lvwSearchDetails_ColumnHeader_1.Width = 97
		' 
		' _lvwSearchDetails_ColumnHeader_2
		' 
		Me._lvwSearchDetails_ColumnHeader_2.Text = "Doc Date"
		Me._lvwSearchDetails_ColumnHeader_2.Width = 97
		' 
		' _lvwSearchDetails_ColumnHeader_3
		' 
		Me._lvwSearchDetails_ColumnHeader_3.Text = "DocType"
		Me._lvwSearchDetails_ColumnHeader_3.Width = 97
		' 
		' _lvwSearchDetails_ColumnHeader_4
		' 
		Me._lvwSearchDetails_ColumnHeader_4.Text = "Status"
		Me._lvwSearchDetails_ColumnHeader_4.Width = 97
		' 
		' _lvwSearchDetails_ColumnHeader_5
		' 
		Me._lvwSearchDetails_ColumnHeader_5.Text = "Comment"
		Me._lvwSearchDetails_ColumnHeader_5.Width = 97
		' 
		' _lvwSearchDetails_ColumnHeader_6
		' 
		Me._lvwSearchDetails_ColumnHeader_6.Text = "Doc Date Sort"
		Me._lvwSearchDetails_ColumnHeader_6.Width = 97
		' 
		' imglImages
		' 
		Me.imglImages.ImageSize = New System.Drawing.Size(16, 16)
		Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(255, 255, 255)
		Me.imglImages.Images.SetKeyName(0, "FindImage")
		' 
		' ImgImage
		' 
		Me.ImgImage.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.ImgImage.Cursor = System.Windows.Forms.Cursors.Default
		Me.ImgImage.Enabled = True
		Me.ImgImage.Image = CType(resources.GetObject("ImgImage.Image"), System.Drawing.Image)
		Me.ImgImage.Location = New System.Drawing.Point(610, 96)
		Me.ImgImage.Name = "ImgImage"
		Me.ImgImage.Size = New System.Drawing.Size(32, 32)
		Me.ImgImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.ImgImage.Visible = True
		' 
		' frmInterface
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(677, 359)
		Me.ControlBox = True
		Me.Controls.Add(Me.uctAnchor)
		Me.Controls.Add(Me.cmdReverse)
		Me.Controls.Add(Me.cmdEdit)
		Me.Controls.Add(Me.cmdNew)
		Me.Controls.Add(Me.cmdNewSearch)
		Me.Controls.Add(Me.cmdFindNow)
		Me.Controls.Add(Me.cmdHelp)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOk)
		Me.Controls.Add(Me.tabMainTab)
		Me.Controls.Add(Me.stbStatus)
		Me.Controls.Add(Me.lvwSearchDetails)
		Me.Controls.Add(Me.ImgImage)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = True
		Me.Icon = CType(resources.GetObject("frmInterface.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(191, 280)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Find: Document"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMainTab, 1)
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwSearchDetails, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tabMainTab.ResumeLayout(False)
		Me._tabMainTab_TabPage0.ResumeLayout(False)
		Me.stbStatus.ResumeLayout(False)
		Me.lvwSearchDetails.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class