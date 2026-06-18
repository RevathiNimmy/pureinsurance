<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		lvwSearchCredits_InitializeColumnKeys()
		lvwSearchDebits_InitializeColumnKeys()
		tabMainPreviousTab = tabMain.SelectedIndex
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
	Public WithEvents mnuFindAccount As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFindDocument As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	Private WithEvents _lvwSearchDebits_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDebits_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDebits_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDebits_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDebits_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDebits_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDebits_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDebits_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDebits_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDebits_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDebits_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDebits_ColumnHeader_12 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDebits_ColumnHeader_13 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDebits_ColumnHeader_14 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDebits_ColumnHeader_15 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDebits_ColumnHeader_16 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDebits_ColumnHeader_17 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDebits_ColumnHeader_18 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDebits_ColumnHeader_19 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDebits_ColumnHeader_20 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwSearchDebits As System.Windows.Forms.ListView
	Private WithEvents _TabResults_TabPage0 As System.Windows.Forms.TabPage
	Private WithEvents _lvwSearchCredits_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchCredits_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchCredits_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchCredits_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchCredits_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchCredits_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchCredits_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchCredits_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchCredits_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchCredits_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchCredits_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchCredits_ColumnHeader_12 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchCredits_ColumnHeader_13 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchCredits_ColumnHeader_14 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchCredits_ColumnHeader_15 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchCredits_ColumnHeader_16 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchCredits_ColumnHeader_17 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchCredits_ColumnHeader_18 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchCredits_ColumnHeader_19 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchCredits_ColumnHeader_20 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwSearchCredits As System.Windows.Forms.ListView
	Private WithEvents _TabResults_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents TabResults As System.Windows.Forms.TabControl
	Public WithEvents cmdNewSearch As System.Windows.Forms.Button
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents lblAccountName As System.Windows.Forms.Label
	Public WithEvents lblAccountBalance As System.Windows.Forms.Label
	Public WithEvents lblTelephone As System.Windows.Forms.Label
	Public WithEvents lblContactName As System.Windows.Forms.Label
	Public WithEvents lblAccountCode As System.Windows.Forms.Label
	Public WithEvents lblDebitsMarked As System.Windows.Forms.Label
	Public WithEvents lblCreditsMarked As System.Windows.Forms.Label
	Public WithEvents panAccountName As System.Windows.Forms.Panel
	Public WithEvents panContactName As System.Windows.Forms.Panel
	Public WithEvents panAccountBalance As System.Windows.Forms.Panel
	Public WithEvents panPhoneExtension As System.Windows.Forms.Panel
	Public WithEvents panPhoneNumber As System.Windows.Forms.Panel
	Public WithEvents panPhoneAreaCode As System.Windows.Forms.Panel
	Public WithEvents panStatus As System.Windows.Forms.Panel
	Public WithEvents cmdAccountCode As System.Windows.Forms.Button
	Public WithEvents txtAccountCode As System.Windows.Forms.TextBox
	Public WithEvents panDebitsMarked As System.Windows.Forms.Panel
	Public WithEvents panCreditsMarked As System.Windows.Forms.Panel
	Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents lblPeriod As System.Windows.Forms.Label
	Public WithEvents lblDocTypeGroup As System.Windows.Forms.Label
	Public WithEvents lblDateFrom As System.Windows.Forms.Label
	Public WithEvents lblDateTo As System.Windows.Forms.Label
	Public WithEvents lblOperatorID As System.Windows.Forms.Label
	Public WithEvents lblDepartment As System.Windows.Forms.Label
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	Public WithEvents lblSubBranch As System.Windows.Forms.Label
	Public WithEvents lblDocumentType As System.Windows.Forms.Label
	Public WithEvents cmbCurrency As PMLookupControl.cboPMLookup
	Public WithEvents cboDepartment As PMLookupControl.cboPMLookup
	Public WithEvents cboPMUser As PMUserLookupControl.cboPMUserLookup
	Public WithEvents txtDateTo As System.Windows.Forms.TextBox
	Public WithEvents txtDateFrom As System.Windows.Forms.TextBox
	Public WithEvents cmbPeriod As System.Windows.Forms.ComboBox
	Public WithEvents cmbDocTypeGroup As System.Windows.Forms.ComboBox
	Public WithEvents cboSubBranch As System.Windows.Forms.ComboBox
	Public WithEvents cmbDocumentType As System.Windows.Forms.ComboBox
	Private WithEvents _tabMain_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents tabMain As System.Windows.Forms.TabControl
	Public WithEvents cmdFindNow As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Private WithEvents _stbStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents stbStatus As System.Windows.Forms.StatusStrip
	Public WithEvents cmdAllocate As System.Windows.Forms.Button
	Public WithEvents cmdMark As System.Windows.Forms.Button
	Public WithEvents cmdWriteOff As System.Windows.Forms.Button
	Public WithEvents ImgImage As System.Windows.Forms.PictureBox
	Public WithEvents imglImages As System.Windows.Forms.ImageList
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	Dim Private tabMainPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.MainMenu1 = New System.Windows.Forms.MenuStrip
		Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFindAccount = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFindDocument = New System.Windows.Forms.ToolStripMenuItem
		Me.TabResults = New System.Windows.Forms.TabControl
		Me._TabResults_TabPage0 = New System.Windows.Forms.TabPage
		Me.lvwSearchDebits = New System.Windows.Forms.ListView
		Me._lvwSearchDebits_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDebits_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDebits_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDebits_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDebits_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDebits_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDebits_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDebits_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDebits_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDebits_ColumnHeader_10 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDebits_ColumnHeader_11 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDebits_ColumnHeader_12 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDebits_ColumnHeader_13 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDebits_ColumnHeader_14 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDebits_ColumnHeader_15 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDebits_ColumnHeader_16 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDebits_ColumnHeader_17 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDebits_ColumnHeader_18 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDebits_ColumnHeader_19 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDebits_ColumnHeader_20 = New System.Windows.Forms.ColumnHeader
		Me._TabResults_TabPage1 = New System.Windows.Forms.TabPage
		Me.lvwSearchCredits = New System.Windows.Forms.ListView
		Me._lvwSearchCredits_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchCredits_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchCredits_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchCredits_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchCredits_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchCredits_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchCredits_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchCredits_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchCredits_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchCredits_ColumnHeader_10 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchCredits_ColumnHeader_11 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchCredits_ColumnHeader_12 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchCredits_ColumnHeader_13 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchCredits_ColumnHeader_14 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchCredits_ColumnHeader_15 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchCredits_ColumnHeader_16 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchCredits_ColumnHeader_17 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchCredits_ColumnHeader_18 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchCredits_ColumnHeader_19 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchCredits_ColumnHeader_20 = New System.Windows.Forms.ColumnHeader
		Me.cmdNewSearch = New System.Windows.Forms.Button
		Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
		Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
		Me.dlgHelpFont = New System.Windows.Forms.FontDialog
		Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
		Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
		Me.tabMain = New System.Windows.Forms.TabControl
		Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage
		Me.lblAccountName = New System.Windows.Forms.Label
		Me.lblAccountBalance = New System.Windows.Forms.Label
		Me.lblTelephone = New System.Windows.Forms.Label
		Me.lblContactName = New System.Windows.Forms.Label
		Me.lblAccountCode = New System.Windows.Forms.Label
		Me.lblDebitsMarked = New System.Windows.Forms.Label
		Me.lblCreditsMarked = New System.Windows.Forms.Label
		Me.panAccountName = New System.Windows.Forms.Panel
		Me.panContactName = New System.Windows.Forms.Panel
		Me.panAccountBalance = New System.Windows.Forms.Panel
		Me.panPhoneExtension = New System.Windows.Forms.Panel
		Me.panPhoneNumber = New System.Windows.Forms.Panel
		Me.panPhoneAreaCode = New System.Windows.Forms.Panel
		Me.panStatus = New System.Windows.Forms.Panel
		Me.cmdAccountCode = New System.Windows.Forms.Button
		Me.txtAccountCode = New System.Windows.Forms.TextBox
		Me.panDebitsMarked = New System.Windows.Forms.Panel
		Me.panCreditsMarked = New System.Windows.Forms.Panel
		Me._tabMain_TabPage1 = New System.Windows.Forms.TabPage
		Me.lblPeriod = New System.Windows.Forms.Label
		Me.lblDocTypeGroup = New System.Windows.Forms.Label
		Me.lblDateFrom = New System.Windows.Forms.Label
		Me.lblDateTo = New System.Windows.Forms.Label
		Me.lblOperatorID = New System.Windows.Forms.Label
		Me.lblDepartment = New System.Windows.Forms.Label
		Me.lblCurrency = New System.Windows.Forms.Label
		Me.lblSubBranch = New System.Windows.Forms.Label
		Me.lblDocumentType = New System.Windows.Forms.Label
		Me.cmbCurrency = New PMLookupControl.cboPMLookup
		Me.cboDepartment = New PMLookupControl.cboPMLookup
		Me.cboPMUser = New PMUserLookupControl.cboPMUserLookup
		Me.txtDateTo = New System.Windows.Forms.TextBox
		Me.txtDateFrom = New System.Windows.Forms.TextBox
		Me.cmbPeriod = New System.Windows.Forms.ComboBox
		Me.cmbDocTypeGroup = New System.Windows.Forms.ComboBox
		Me.cboSubBranch = New System.Windows.Forms.ComboBox
		Me.cmbDocumentType = New System.Windows.Forms.ComboBox
		Me.cmdFindNow = New System.Windows.Forms.Button
		Me.cmdHelp = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.stbStatus = New System.Windows.Forms.StatusStrip
		Me._stbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
		Me.cmdAllocate = New System.Windows.Forms.Button
		Me.cmdMark = New System.Windows.Forms.Button
		Me.cmdWriteOff = New System.Windows.Forms.Button
		Me.ImgImage = New System.Windows.Forms.PictureBox
		Me.imglImages = New System.Windows.Forms.ImageList
		Me.TabResults.SuspendLayout()
		Me._TabResults_TabPage0.SuspendLayout()
		Me.lvwSearchDebits.SuspendLayout()
		Me._TabResults_TabPage1.SuspendLayout()
		Me.lvwSearchCredits.SuspendLayout()
		Me.tabMain.SuspendLayout()
		Me._tabMain_TabPage0.SuspendLayout()
		Me._tabMain_TabPage1.SuspendLayout()
		Me.stbStatus.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' MainMenu1
		' 
		Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuFile})
		' 
		' mnuFile
		' 
		Me.mnuFile.Available = False
		Me.mnuFile.Checked = False
		Me.mnuFile.Enabled = True
		Me.mnuFile.Name = "mnuFile"
		Me.mnuFile.Text = "&File"
		Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuFindAccount, Me.mnuFindDocument})
		' 
		' mnuFindAccount
		' 
		Me.mnuFindAccount.Available = True
		Me.mnuFindAccount.Checked = False
		Me.mnuFindAccount.Enabled = True
		Me.mnuFindAccount.Name = "mnuFindAccount"
		Me.mnuFindAccount.Text = "Find &Account Transactions"
		' 
		' mnuFindDocument
		' 
		Me.mnuFindDocument.Available = True
		Me.mnuFindDocument.Checked = False
		Me.mnuFindDocument.Enabled = True
		Me.mnuFindDocument.Name = "mnuFindDocument"
		Me.mnuFindDocument.Text = "Find &Document Transactions"
		' 
		' TabResults
		' 
		Me.TabResults.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.TabResults.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.TabResults.Controls.Add(Me._TabResults_TabPage0)
		Me.TabResults.Controls.Add(Me._TabResults_TabPage1)
		Me.TabResults.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.TabResults.ItemSize = New System.Drawing.Size(351, 18)
		Me.TabResults.Location = New System.Drawing.Point(8, 220)
		Me.TabResults.Multiline = True
		Me.TabResults.Name = "TabResults"
		Me.TabResults.Size = New System.Drawing.Size(709, 197)
		Me.TabResults.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.TabResults.TabIndex = 29
		' 
		' _TabResults_TabPage0
		' 
		Me._TabResults_TabPage0.Controls.Add(Me.lvwSearchDebits)
		Me._TabResults_TabPage0.Text = "&Debits"
		' 
		' lvwSearchDebits
		' 
		Me.lvwSearchDebits.BackColor = System.Drawing.SystemColors.Window
		Me.lvwSearchDebits.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lvwSearchDebits.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwSearchDebits.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwSearchDebits.HideSelection = False
		Me.lvwSearchDebits.LabelEdit = False
		Me.lvwSearchDebits.LabelWrap = True
		Me.lvwSearchDebits.LargeImageList = imglImages
		Me.lvwSearchDebits.Location = New System.Drawing.Point(16, 12)
		Me.lvwSearchDebits.MultiSelect = True
		Me.lvwSearchDebits.Name = "lvwSearchDebits"
		Me.lvwSearchDebits.Size = New System.Drawing.Size(673, 145)
		Me.lvwSearchDebits.SmallImageList = imglImages
		Me.lvwSearchDebits.TabIndex = 30
		Me.lvwSearchDebits.TabStop = False
		Me.lvwSearchDebits.View = System.Windows.Forms.View.Details
		Me.lvwSearchDebits.Columns.Add(Me._lvwSearchDebits_ColumnHeader_1)
		Me.lvwSearchDebits.Columns.Add(Me._lvwSearchDebits_ColumnHeader_2)
		Me.lvwSearchDebits.Columns.Add(Me._lvwSearchDebits_ColumnHeader_3)
		Me.lvwSearchDebits.Columns.Add(Me._lvwSearchDebits_ColumnHeader_4)
		Me.lvwSearchDebits.Columns.Add(Me._lvwSearchDebits_ColumnHeader_5)
		Me.lvwSearchDebits.Columns.Add(Me._lvwSearchDebits_ColumnHeader_6)
		Me.lvwSearchDebits.Columns.Add(Me._lvwSearchDebits_ColumnHeader_7)
		Me.lvwSearchDebits.Columns.Add(Me._lvwSearchDebits_ColumnHeader_8)
		Me.lvwSearchDebits.Columns.Add(Me._lvwSearchDebits_ColumnHeader_9)
		Me.lvwSearchDebits.Columns.Add(Me._lvwSearchDebits_ColumnHeader_10)
		Me.lvwSearchDebits.Columns.Add(Me._lvwSearchDebits_ColumnHeader_11)
		Me.lvwSearchDebits.Columns.Add(Me._lvwSearchDebits_ColumnHeader_12)
		Me.lvwSearchDebits.Columns.Add(Me._lvwSearchDebits_ColumnHeader_13)
		Me.lvwSearchDebits.Columns.Add(Me._lvwSearchDebits_ColumnHeader_14)
		Me.lvwSearchDebits.Columns.Add(Me._lvwSearchDebits_ColumnHeader_15)
		Me.lvwSearchDebits.Columns.Add(Me._lvwSearchDebits_ColumnHeader_16)
		Me.lvwSearchDebits.Columns.Add(Me._lvwSearchDebits_ColumnHeader_17)
		Me.lvwSearchDebits.Columns.Add(Me._lvwSearchDebits_ColumnHeader_18)
		Me.lvwSearchDebits.Columns.Add(Me._lvwSearchDebits_ColumnHeader_19)
		Me.lvwSearchDebits.Columns.Add(Me._lvwSearchDebits_ColumnHeader_20)
		' 
		' _lvwSearchDebits_ColumnHeader_1
		' 
		Me._lvwSearchDebits_ColumnHeader_1.Tag = ""
		Me._lvwSearchDebits_ColumnHeader_1.Text = "1"
		Me._lvwSearchDebits_ColumnHeader_1.Width = 97
		' 
		' _lvwSearchDebits_ColumnHeader_2
		' 
		Me._lvwSearchDebits_ColumnHeader_2.Tag = ""
		Me._lvwSearchDebits_ColumnHeader_2.Text = "2"
		Me._lvwSearchDebits_ColumnHeader_2.Width = 97
		' 
		' _lvwSearchDebits_ColumnHeader_3
		' 
		Me._lvwSearchDebits_ColumnHeader_3.Tag = ""
		Me._lvwSearchDebits_ColumnHeader_3.Text = "3"
		Me._lvwSearchDebits_ColumnHeader_3.Width = 97
		' 
		' _lvwSearchDebits_ColumnHeader_4
		' 
		Me._lvwSearchDebits_ColumnHeader_4.Tag = ""
		Me._lvwSearchDebits_ColumnHeader_4.Text = "4"
		Me._lvwSearchDebits_ColumnHeader_4.Width = 97
		' 
		' _lvwSearchDebits_ColumnHeader_5
		' 
		Me._lvwSearchDebits_ColumnHeader_5.Tag = ""
		Me._lvwSearchDebits_ColumnHeader_5.Text = "5"
		Me._lvwSearchDebits_ColumnHeader_5.Width = 97
		' 
		' _lvwSearchDebits_ColumnHeader_6
		' 
		Me._lvwSearchDebits_ColumnHeader_6.Tag = ""
		Me._lvwSearchDebits_ColumnHeader_6.Text = "6"
		Me._lvwSearchDebits_ColumnHeader_6.Width = 97
		' 
		' _lvwSearchDebits_ColumnHeader_7
		' 
		Me._lvwSearchDebits_ColumnHeader_7.Tag = ""
		Me._lvwSearchDebits_ColumnHeader_7.Text = "7"
		Me._lvwSearchDebits_ColumnHeader_7.Width = 97
		' 
		' _lvwSearchDebits_ColumnHeader_8
		' 
		Me._lvwSearchDebits_ColumnHeader_8.Tag = ""
		Me._lvwSearchDebits_ColumnHeader_8.Text = "8"
		Me._lvwSearchDebits_ColumnHeader_8.Width = 97
		' 
		' _lvwSearchDebits_ColumnHeader_9
		' 
		Me._lvwSearchDebits_ColumnHeader_9.Tag = ""
		Me._lvwSearchDebits_ColumnHeader_9.Text = "9"
		Me._lvwSearchDebits_ColumnHeader_9.Width = 97
		' 
		' _lvwSearchDebits_ColumnHeader_10
		' 
		Me._lvwSearchDebits_ColumnHeader_10.Tag = ""
		Me._lvwSearchDebits_ColumnHeader_10.Text = "10"
		Me._lvwSearchDebits_ColumnHeader_10.Width = 97
		' 
		' _lvwSearchDebits_ColumnHeader_11
		' 
		Me._lvwSearchDebits_ColumnHeader_11.Tag = ""
		Me._lvwSearchDebits_ColumnHeader_11.Text = "11"
		Me._lvwSearchDebits_ColumnHeader_11.Width = 97
		' 
		' _lvwSearchDebits_ColumnHeader_12
		' 
		Me._lvwSearchDebits_ColumnHeader_12.Tag = ""
		Me._lvwSearchDebits_ColumnHeader_12.Text = "12"
		Me._lvwSearchDebits_ColumnHeader_12.Width = 97
		' 
		' _lvwSearchDebits_ColumnHeader_13
		' 
		Me._lvwSearchDebits_ColumnHeader_13.Tag = ""
		Me._lvwSearchDebits_ColumnHeader_13.Text = "13"
		Me._lvwSearchDebits_ColumnHeader_13.Width = 97
		' 
		' _lvwSearchDebits_ColumnHeader_14
		' 
		Me._lvwSearchDebits_ColumnHeader_14.Tag = ""
		Me._lvwSearchDebits_ColumnHeader_14.Text = "14"
		Me._lvwSearchDebits_ColumnHeader_14.Width = 97
		' 
		' _lvwSearchDebits_ColumnHeader_15
		' 
		Me._lvwSearchDebits_ColumnHeader_15.Tag = ""
		Me._lvwSearchDebits_ColumnHeader_15.Text = "15"
		Me._lvwSearchDebits_ColumnHeader_15.Width = 97
		' 
		' _lvwSearchDebits_ColumnHeader_16
		' 
		Me._lvwSearchDebits_ColumnHeader_16.Tag = ""
		Me._lvwSearchDebits_ColumnHeader_16.Text = "16"
		Me._lvwSearchDebits_ColumnHeader_16.Width = 97
		' 
		' _lvwSearchDebits_ColumnHeader_17
		' 
		Me._lvwSearchDebits_ColumnHeader_17.Tag = ""
		Me._lvwSearchDebits_ColumnHeader_17.Text = "17"
		Me._lvwSearchDebits_ColumnHeader_17.Width = 97
		' 
		' _lvwSearchDebits_ColumnHeader_18
		' 
		Me._lvwSearchDebits_ColumnHeader_18.Tag = ""
		Me._lvwSearchDebits_ColumnHeader_18.Text = "18"
		Me._lvwSearchDebits_ColumnHeader_18.Width = 97
		' 
		' _lvwSearchDebits_ColumnHeader_19
		' 
		Me._lvwSearchDebits_ColumnHeader_19.Tag = ""
		Me._lvwSearchDebits_ColumnHeader_19.Text = ""
		Me._lvwSearchDebits_ColumnHeader_19.Width = 97
		' 
		' _lvwSearchDebits_ColumnHeader_20
		' 
		Me._lvwSearchDebits_ColumnHeader_20.Tag = ""
		Me._lvwSearchDebits_ColumnHeader_20.Text = ""
		Me._lvwSearchDebits_ColumnHeader_20.Width = 97
		' 
		' _TabResults_TabPage1
		' 
		Me._TabResults_TabPage1.Controls.Add(Me.lvwSearchCredits)
		Me._TabResults_TabPage1.Text = "&Credits"
		' 
		' lvwSearchCredits
		' 
		Me.lvwSearchCredits.BackColor = System.Drawing.SystemColors.Window
		Me.lvwSearchCredits.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lvwSearchCredits.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwSearchCredits.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwSearchCredits.HideSelection = True
		Me.lvwSearchCredits.LabelEdit = False
		Me.lvwSearchCredits.LabelWrap = True
		Me.lvwSearchCredits.LargeImageList = imglImages
		Me.lvwSearchCredits.Location = New System.Drawing.Point(16, 12)
		Me.lvwSearchCredits.Name = "lvwSearchCredits"
		Me.lvwSearchCredits.Size = New System.Drawing.Size(673, 145)
		Me.lvwSearchCredits.SmallImageList = imglImages
		Me.lvwSearchCredits.TabIndex = 31
		Me.lvwSearchCredits.View = System.Windows.Forms.View.Details
		Me.lvwSearchCredits.Columns.Add(Me._lvwSearchCredits_ColumnHeader_1)
		Me.lvwSearchCredits.Columns.Add(Me._lvwSearchCredits_ColumnHeader_2)
		Me.lvwSearchCredits.Columns.Add(Me._lvwSearchCredits_ColumnHeader_3)
		Me.lvwSearchCredits.Columns.Add(Me._lvwSearchCredits_ColumnHeader_4)
		Me.lvwSearchCredits.Columns.Add(Me._lvwSearchCredits_ColumnHeader_5)
		Me.lvwSearchCredits.Columns.Add(Me._lvwSearchCredits_ColumnHeader_6)
		Me.lvwSearchCredits.Columns.Add(Me._lvwSearchCredits_ColumnHeader_7)
		Me.lvwSearchCredits.Columns.Add(Me._lvwSearchCredits_ColumnHeader_8)
		Me.lvwSearchCredits.Columns.Add(Me._lvwSearchCredits_ColumnHeader_9)
		Me.lvwSearchCredits.Columns.Add(Me._lvwSearchCredits_ColumnHeader_10)
		Me.lvwSearchCredits.Columns.Add(Me._lvwSearchCredits_ColumnHeader_11)
		Me.lvwSearchCredits.Columns.Add(Me._lvwSearchCredits_ColumnHeader_12)
		Me.lvwSearchCredits.Columns.Add(Me._lvwSearchCredits_ColumnHeader_13)
		Me.lvwSearchCredits.Columns.Add(Me._lvwSearchCredits_ColumnHeader_14)
		Me.lvwSearchCredits.Columns.Add(Me._lvwSearchCredits_ColumnHeader_15)
		Me.lvwSearchCredits.Columns.Add(Me._lvwSearchCredits_ColumnHeader_16)
		Me.lvwSearchCredits.Columns.Add(Me._lvwSearchCredits_ColumnHeader_17)
		Me.lvwSearchCredits.Columns.Add(Me._lvwSearchCredits_ColumnHeader_18)
		Me.lvwSearchCredits.Columns.Add(Me._lvwSearchCredits_ColumnHeader_19)
		Me.lvwSearchCredits.Columns.Add(Me._lvwSearchCredits_ColumnHeader_20)
		' 
		' _lvwSearchCredits_ColumnHeader_1
		' 
		Me._lvwSearchCredits_ColumnHeader_1.Tag = ""
		Me._lvwSearchCredits_ColumnHeader_1.Text = ""
		Me._lvwSearchCredits_ColumnHeader_1.Width = 97
		' 
		' _lvwSearchCredits_ColumnHeader_2
		' 
		Me._lvwSearchCredits_ColumnHeader_2.Tag = ""
		Me._lvwSearchCredits_ColumnHeader_2.Text = ""
		Me._lvwSearchCredits_ColumnHeader_2.Width = 97
		' 
		' _lvwSearchCredits_ColumnHeader_3
		' 
		Me._lvwSearchCredits_ColumnHeader_3.Tag = ""
		Me._lvwSearchCredits_ColumnHeader_3.Text = ""
		Me._lvwSearchCredits_ColumnHeader_3.Width = 97
		' 
		' _lvwSearchCredits_ColumnHeader_4
		' 
		Me._lvwSearchCredits_ColumnHeader_4.Tag = ""
		Me._lvwSearchCredits_ColumnHeader_4.Text = ""
		Me._lvwSearchCredits_ColumnHeader_4.Width = 97
		' 
		' _lvwSearchCredits_ColumnHeader_5
		' 
		Me._lvwSearchCredits_ColumnHeader_5.Tag = ""
		Me._lvwSearchCredits_ColumnHeader_5.Text = ""
		Me._lvwSearchCredits_ColumnHeader_5.Width = 97
		' 
		' _lvwSearchCredits_ColumnHeader_6
		' 
		Me._lvwSearchCredits_ColumnHeader_6.Tag = ""
		Me._lvwSearchCredits_ColumnHeader_6.Text = ""
		Me._lvwSearchCredits_ColumnHeader_6.Width = 97
		' 
		' _lvwSearchCredits_ColumnHeader_7
		' 
		Me._lvwSearchCredits_ColumnHeader_7.Tag = ""
		Me._lvwSearchCredits_ColumnHeader_7.Text = ""
		Me._lvwSearchCredits_ColumnHeader_7.Width = 97
		' 
		' _lvwSearchCredits_ColumnHeader_8
		' 
		Me._lvwSearchCredits_ColumnHeader_8.Tag = ""
		Me._lvwSearchCredits_ColumnHeader_8.Text = ""
		Me._lvwSearchCredits_ColumnHeader_8.Width = 97
		' 
		' _lvwSearchCredits_ColumnHeader_9
		' 
		Me._lvwSearchCredits_ColumnHeader_9.Tag = ""
		Me._lvwSearchCredits_ColumnHeader_9.Text = ""
		Me._lvwSearchCredits_ColumnHeader_9.Width = 97
		' 
		' _lvwSearchCredits_ColumnHeader_10
		' 
		Me._lvwSearchCredits_ColumnHeader_10.Tag = ""
		Me._lvwSearchCredits_ColumnHeader_10.Text = ""
		Me._lvwSearchCredits_ColumnHeader_10.Width = 97
		' 
		' _lvwSearchCredits_ColumnHeader_11
		' 
		Me._lvwSearchCredits_ColumnHeader_11.Tag = ""
		Me._lvwSearchCredits_ColumnHeader_11.Text = ""
		Me._lvwSearchCredits_ColumnHeader_11.Width = 97
		' 
		' _lvwSearchCredits_ColumnHeader_12
		' 
		Me._lvwSearchCredits_ColumnHeader_12.Tag = ""
		Me._lvwSearchCredits_ColumnHeader_12.Text = ""
		Me._lvwSearchCredits_ColumnHeader_12.Width = 97
		' 
		' _lvwSearchCredits_ColumnHeader_13
		' 
		Me._lvwSearchCredits_ColumnHeader_13.Tag = ""
		Me._lvwSearchCredits_ColumnHeader_13.Text = ""
		Me._lvwSearchCredits_ColumnHeader_13.Width = 97
		' 
		' _lvwSearchCredits_ColumnHeader_14
		' 
		Me._lvwSearchCredits_ColumnHeader_14.Tag = ""
		Me._lvwSearchCredits_ColumnHeader_14.Text = ""
		Me._lvwSearchCredits_ColumnHeader_14.Width = 97
		' 
		' _lvwSearchCredits_ColumnHeader_15
		' 
		Me._lvwSearchCredits_ColumnHeader_15.Tag = ""
		Me._lvwSearchCredits_ColumnHeader_15.Text = ""
		Me._lvwSearchCredits_ColumnHeader_15.Width = 97
		' 
		' _lvwSearchCredits_ColumnHeader_16
		' 
		Me._lvwSearchCredits_ColumnHeader_16.Tag = ""
		Me._lvwSearchCredits_ColumnHeader_16.Text = ""
		Me._lvwSearchCredits_ColumnHeader_16.Width = 97
		' 
		' _lvwSearchCredits_ColumnHeader_17
		' 
		Me._lvwSearchCredits_ColumnHeader_17.Tag = ""
		Me._lvwSearchCredits_ColumnHeader_17.Text = ""
		Me._lvwSearchCredits_ColumnHeader_17.Width = 97
		' 
		' _lvwSearchCredits_ColumnHeader_18
		' 
		Me._lvwSearchCredits_ColumnHeader_18.Tag = ""
		Me._lvwSearchCredits_ColumnHeader_18.Text = ""
		Me._lvwSearchCredits_ColumnHeader_18.Width = 97
		' 
		' _lvwSearchCredits_ColumnHeader_19
		' 
		Me._lvwSearchCredits_ColumnHeader_19.Tag = ""
		Me._lvwSearchCredits_ColumnHeader_19.Text = ""
		Me._lvwSearchCredits_ColumnHeader_19.Width = 97
		' 
		' _lvwSearchCredits_ColumnHeader_20
		' 
		Me._lvwSearchCredits_ColumnHeader_20.Tag = ""
		Me._lvwSearchCredits_ColumnHeader_20.Text = ""
		Me._lvwSearchCredits_ColumnHeader_20.Width = 97
		' 
		' cmdNewSearch
		' 
		Me.cmdNewSearch.BackColor = System.Drawing.SystemColors.Control
		Me.cmdNewSearch.CausesValidation = True
		Me.cmdNewSearch.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdNewSearch.Enabled = True
		Me.cmdNewSearch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdNewSearch.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNewSearch.Location = New System.Drawing.Point(640, 88)
		Me.cmdNewSearch.Name = "cmdNewSearch"
		Me.cmdNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNewSearch.Size = New System.Drawing.Size(73, 34)
		Me.cmdNewSearch.TabIndex = 6
		Me.cmdNewSearch.TabStop = False
		Me.cmdNewSearch.Text = "Ne&w Search"
		Me.cmdNewSearch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdNewSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tabMain
		' 
		Me.tabMain.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabMain.Controls.Add(Me._tabMain_TabPage0)
		Me.tabMain.Controls.Add(Me._tabMain_TabPage1)
		Me.tabMain.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMain.ItemSize = New System.Drawing.Size(311, 18)
		Me.tabMain.Location = New System.Drawing.Point(8, 32)
		Me.tabMain.Multiline = True
		Me.tabMain.Name = "tabMain"
		Me.tabMain.Size = New System.Drawing.Size(629, 183)
		Me.tabMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMain.TabIndex = 0
		Me.tabMain.TabStop = False
		' 
		' _tabMain_TabPage0
		' 
		Me._tabMain_TabPage0.Controls.Add(Me.lblAccountName)
		Me._tabMain_TabPage0.Controls.Add(Me.lblAccountBalance)
		Me._tabMain_TabPage0.Controls.Add(Me.lblTelephone)
		Me._tabMain_TabPage0.Controls.Add(Me.lblContactName)
		Me._tabMain_TabPage0.Controls.Add(Me.lblAccountCode)
		Me._tabMain_TabPage0.Controls.Add(Me.lblDebitsMarked)
		Me._tabMain_TabPage0.Controls.Add(Me.lblCreditsMarked)
		Me._tabMain_TabPage0.Controls.Add(Me.panAccountName)
		Me._tabMain_TabPage0.Controls.Add(Me.panContactName)
		Me._tabMain_TabPage0.Controls.Add(Me.panAccountBalance)
		Me._tabMain_TabPage0.Controls.Add(Me.panPhoneExtension)
		Me._tabMain_TabPage0.Controls.Add(Me.panPhoneNumber)
		Me._tabMain_TabPage0.Controls.Add(Me.panPhoneAreaCode)
		Me._tabMain_TabPage0.Controls.Add(Me.panStatus)
		Me._tabMain_TabPage0.Controls.Add(Me.cmdAccountCode)
		Me._tabMain_TabPage0.Controls.Add(Me.txtAccountCode)
		Me._tabMain_TabPage0.Controls.Add(Me.panDebitsMarked)
		Me._tabMain_TabPage0.Controls.Add(Me.panCreditsMarked)
		Me._tabMain_TabPage0.Text = "&1 - Account"
		' 
		' lblAccountName
		' 
		Me.lblAccountName.AutoSize = False
		Me.lblAccountName.BackColor = System.Drawing.SystemColors.Control
		Me.lblAccountName.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAccountName.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAccountName.Enabled = True
		Me.lblAccountName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAccountName.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAccountName.Location = New System.Drawing.Point(16, 68)
		Me.lblAccountName.Name = "lblAccountName"
		Me.lblAccountName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAccountName.Size = New System.Drawing.Size(105, 17)
		Me.lblAccountName.TabIndex = 24
		Me.lblAccountName.Text = "Account Name:"
		Me.lblAccountName.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAccountName.UseMnemonic = True
		Me.lblAccountName.Visible = True
		' 
		' lblAccountBalance
		' 
		Me.lblAccountBalance.AutoSize = False
		Me.lblAccountBalance.BackColor = System.Drawing.SystemColors.Control
		Me.lblAccountBalance.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAccountBalance.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAccountBalance.Enabled = True
		Me.lblAccountBalance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAccountBalance.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAccountBalance.Location = New System.Drawing.Point(404, 68)
		Me.lblAccountBalance.Name = "lblAccountBalance"
		Me.lblAccountBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAccountBalance.Size = New System.Drawing.Size(119, 17)
		Me.lblAccountBalance.TabIndex = 25
		Me.lblAccountBalance.Text = "Balance:"
		Me.lblAccountBalance.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAccountBalance.UseMnemonic = True
		Me.lblAccountBalance.Visible = True
		' 
		' lblTelephone
		' 
		Me.lblTelephone.AutoSize = False
		Me.lblTelephone.BackColor = System.Drawing.SystemColors.Control
		Me.lblTelephone.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTelephone.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTelephone.Enabled = True
		Me.lblTelephone.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTelephone.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTelephone.Location = New System.Drawing.Point(16, 118)
		Me.lblTelephone.Name = "lblTelephone"
		Me.lblTelephone.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTelephone.Size = New System.Drawing.Size(89, 17)
		Me.lblTelephone.TabIndex = 26
		Me.lblTelephone.Text = "Telephone No:"
		Me.lblTelephone.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTelephone.UseMnemonic = True
		Me.lblTelephone.Visible = True
		' 
		' lblContactName
		' 
		Me.lblContactName.AutoSize = False
		Me.lblContactName.BackColor = System.Drawing.SystemColors.Control
		Me.lblContactName.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblContactName.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblContactName.Enabled = True
		Me.lblContactName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblContactName.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblContactName.Location = New System.Drawing.Point(16, 94)
		Me.lblContactName.Name = "lblContactName"
		Me.lblContactName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblContactName.Size = New System.Drawing.Size(97, 17)
		Me.lblContactName.TabIndex = 27
		Me.lblContactName.Text = "Contact Name:"
		Me.lblContactName.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblContactName.UseMnemonic = True
		Me.lblContactName.Visible = True
		' 
		' lblAccountCode
		' 
		Me.lblAccountCode.AutoSize = False
		Me.lblAccountCode.BackColor = System.Drawing.SystemColors.Control
		Me.lblAccountCode.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAccountCode.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAccountCode.Enabled = True
		Me.lblAccountCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAccountCode.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAccountCode.Location = New System.Drawing.Point(16, 28)
		Me.lblAccountCode.Name = "lblAccountCode"
		Me.lblAccountCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAccountCode.Size = New System.Drawing.Size(105, 17)
		Me.lblAccountCode.TabIndex = 28
		Me.lblAccountCode.Text = "&Account:"
		Me.lblAccountCode.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAccountCode.UseMnemonic = True
		Me.lblAccountCode.Visible = True
		' 
		' lblDebitsMarked
		' 
		Me.lblDebitsMarked.AutoSize = False
		Me.lblDebitsMarked.BackColor = System.Drawing.SystemColors.Control
		Me.lblDebitsMarked.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDebitsMarked.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDebitsMarked.Enabled = True
		Me.lblDebitsMarked.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDebitsMarked.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDebitsMarked.Location = New System.Drawing.Point(404, 94)
		Me.lblDebitsMarked.Name = "lblDebitsMarked"
		Me.lblDebitsMarked.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDebitsMarked.Size = New System.Drawing.Size(101, 17)
		Me.lblDebitsMarked.TabIndex = 36
		Me.lblDebitsMarked.Text = "Debits Marked:"
		Me.lblDebitsMarked.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDebitsMarked.UseMnemonic = True
		Me.lblDebitsMarked.Visible = True
		' 
		' lblCreditsMarked
		' 
		Me.lblCreditsMarked.AutoSize = False
		Me.lblCreditsMarked.BackColor = System.Drawing.SystemColors.Control
		Me.lblCreditsMarked.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCreditsMarked.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCreditsMarked.Enabled = True
		Me.lblCreditsMarked.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCreditsMarked.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCreditsMarked.Location = New System.Drawing.Point(404, 118)
		Me.lblCreditsMarked.Name = "lblCreditsMarked"
		Me.lblCreditsMarked.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCreditsMarked.Size = New System.Drawing.Size(107, 17)
		Me.lblCreditsMarked.TabIndex = 37
		Me.lblCreditsMarked.Text = "Credits Marked:"
		Me.lblCreditsMarked.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCreditsMarked.UseMnemonic = True
		Me.lblCreditsMarked.Visible = True
		' 
		' panAccountName
		' 
		Me.panAccountName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.panAccountName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.panAccountName.Location = New System.Drawing.Point(108, 68)
		Me.panAccountName.Name = "panAccountName"
		Me.panAccountName.Size = New System.Drawing.Size(286, 19)
		Me.panAccountName.TabIndex = 23
		' 
		' panContactName
		' 
		Me.panContactName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.panContactName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.panContactName.Location = New System.Drawing.Point(108, 92)
		Me.panContactName.Name = "panContactName"
		Me.panContactName.Size = New System.Drawing.Size(286, 19)
		Me.panContactName.TabIndex = 22
		' 
		' panAccountBalance
		' 
		Me.panAccountBalance.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.panAccountBalance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.panAccountBalance.Location = New System.Drawing.Point(504, 68)
		Me.panAccountBalance.Name = "panAccountBalance"
		Me.panAccountBalance.Size = New System.Drawing.Size(105, 19)
		Me.panAccountBalance.TabIndex = 21
		' 
		' panPhoneExtension
		' 
		Me.panPhoneExtension.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.panPhoneExtension.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.panPhoneExtension.Location = New System.Drawing.Point(312, 116)
		Me.panPhoneExtension.Name = "panPhoneExtension"
		Me.panPhoneExtension.Size = New System.Drawing.Size(82, 19)
		Me.panPhoneExtension.TabIndex = 20
		' 
		' panPhoneNumber
		' 
		Me.panPhoneNumber.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.panPhoneNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.panPhoneNumber.Location = New System.Drawing.Point(192, 116)
		Me.panPhoneNumber.Name = "panPhoneNumber"
		Me.panPhoneNumber.Size = New System.Drawing.Size(113, 19)
		Me.panPhoneNumber.TabIndex = 19
		' 
		' panPhoneAreaCode
		' 
		Me.panPhoneAreaCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.panPhoneAreaCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.panPhoneAreaCode.Location = New System.Drawing.Point(108, 116)
		Me.panPhoneAreaCode.Name = "panPhoneAreaCode"
		Me.panPhoneAreaCode.Size = New System.Drawing.Size(77, 19)
		Me.panPhoneAreaCode.TabIndex = 18
		' 
		' panStatus
		' 
		Me.panStatus.BackColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.panStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.panStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.panStatus.Location = New System.Drawing.Point(308, 28)
		Me.panStatus.Name = "panStatus"
		Me.panStatus.Size = New System.Drawing.Size(95, 19)
		Me.panStatus.TabIndex = 17
		' 
		' cmdAccountCode
		' 
		Me.cmdAccountCode.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAccountCode.CausesValidation = True
		Me.cmdAccountCode.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAccountCode.Enabled = True
		Me.cmdAccountCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAccountCode.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAccountCode.Location = New System.Drawing.Point(280, 28)
		Me.cmdAccountCode.Name = "cmdAccountCode"
		Me.cmdAccountCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAccountCode.Size = New System.Drawing.Size(25, 19)
		Me.cmdAccountCode.TabIndex = 15
		Me.cmdAccountCode.TabStop = True
		Me.cmdAccountCode.Text = "..."
		Me.cmdAccountCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAccountCode.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtAccountCode
		' 
		Me.txtAccountCode.AcceptsReturn = True
		Me.txtAccountCode.AutoSize = False
		Me.txtAccountCode.BackColor = System.Drawing.SystemColors.Window
		Me.txtAccountCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtAccountCode.CausesValidation = True
		Me.txtAccountCode.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAccountCode.Enabled = True
		Me.txtAccountCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtAccountCode.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAccountCode.HideSelection = True
		Me.txtAccountCode.Location = New System.Drawing.Point(107, 28)
		Me.txtAccountCode.MaxLength = 0
		Me.txtAccountCode.Multiline = False
		Me.txtAccountCode.Name = "txtAccountCode"
		Me.txtAccountCode.ReadOnly = False
		Me.txtAccountCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAccountCode.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAccountCode.Size = New System.Drawing.Size(173, 19)
		Me.txtAccountCode.TabIndex = 16
		Me.txtAccountCode.TabStop = True
		Me.txtAccountCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtAccountCode.Visible = True
		' 
		' panDebitsMarked
		' 
		Me.panDebitsMarked.BackColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.panDebitsMarked.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.panDebitsMarked.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.panDebitsMarked.Location = New System.Drawing.Point(504, 92)
		Me.panDebitsMarked.Name = "panDebitsMarked"
		Me.panDebitsMarked.Size = New System.Drawing.Size(105, 19)
		Me.panDebitsMarked.TabIndex = 34
		' 
		' panCreditsMarked
		' 
		Me.panCreditsMarked.BackColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.panCreditsMarked.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.panCreditsMarked.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.panCreditsMarked.Location = New System.Drawing.Point(504, 116)
		Me.panCreditsMarked.Name = "panCreditsMarked"
		Me.panCreditsMarked.Size = New System.Drawing.Size(105, 19)
		Me.panCreditsMarked.TabIndex = 35
		' 
		' _tabMain_TabPage1
		' 
		Me._tabMain_TabPage1.Controls.Add(Me.lblPeriod)
		Me._tabMain_TabPage1.Controls.Add(Me.lblDocTypeGroup)
		Me._tabMain_TabPage1.Controls.Add(Me.lblDateFrom)
		Me._tabMain_TabPage1.Controls.Add(Me.lblDateTo)
		Me._tabMain_TabPage1.Controls.Add(Me.lblOperatorID)
		Me._tabMain_TabPage1.Controls.Add(Me.lblDepartment)
		Me._tabMain_TabPage1.Controls.Add(Me.lblCurrency)
		Me._tabMain_TabPage1.Controls.Add(Me.lblSubBranch)
		Me._tabMain_TabPage1.Controls.Add(Me.lblDocumentType)
		Me._tabMain_TabPage1.Controls.Add(Me.cmbCurrency)
		Me._tabMain_TabPage1.Controls.Add(Me.cboDepartment)
		Me._tabMain_TabPage1.Controls.Add(Me.cboPMUser)
		Me._tabMain_TabPage1.Controls.Add(Me.txtDateTo)
		Me._tabMain_TabPage1.Controls.Add(Me.txtDateFrom)
		Me._tabMain_TabPage1.Controls.Add(Me.cmbPeriod)
		Me._tabMain_TabPage1.Controls.Add(Me.cmbDocTypeGroup)
		Me._tabMain_TabPage1.Controls.Add(Me.cboSubBranch)
		Me._tabMain_TabPage1.Controls.Add(Me.cmbDocumentType)
		Me._tabMain_TabPage1.Text = "&2 - Document"
		' 
		' lblPeriod
		' 
		Me.lblPeriod.AutoSize = False
		Me.lblPeriod.BackColor = System.Drawing.SystemColors.Control
		Me.lblPeriod.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPeriod.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPeriod.Enabled = True
		Me.lblPeriod.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPeriod.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPeriod.Location = New System.Drawing.Point(16, 102)
		Me.lblPeriod.Name = "lblPeriod"
		Me.lblPeriod.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPeriod.Size = New System.Drawing.Size(81, 17)
		Me.lblPeriod.TabIndex = 11
		Me.lblPeriod.Text = "Period:"
		Me.lblPeriod.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPeriod.UseMnemonic = True
		Me.lblPeriod.Visible = True
		' 
		' lblDocTypeGroup
		' 
		Me.lblDocTypeGroup.AutoSize = False
		Me.lblDocTypeGroup.BackColor = System.Drawing.SystemColors.Control
		Me.lblDocTypeGroup.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDocTypeGroup.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDocTypeGroup.Enabled = True
		Me.lblDocTypeGroup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDocTypeGroup.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDocTypeGroup.Location = New System.Drawing.Point(16, 44)
		Me.lblDocTypeGroup.Name = "lblDocTypeGroup"
		Me.lblDocTypeGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDocTypeGroup.Size = New System.Drawing.Size(105, 17)
		Me.lblDocTypeGroup.TabIndex = 12
		Me.lblDocTypeGroup.Text = "Doc. Type Group:"
		Me.lblDocTypeGroup.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDocTypeGroup.UseMnemonic = True
		Me.lblDocTypeGroup.Visible = True
		' 
		' lblDateFrom
		' 
		Me.lblDateFrom.AutoSize = False
		Me.lblDateFrom.BackColor = System.Drawing.SystemColors.Control
		Me.lblDateFrom.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDateFrom.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDateFrom.Enabled = True
		Me.lblDateFrom.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDateFrom.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDateFrom.Location = New System.Drawing.Point(312, 70)
		Me.lblDateFrom.Name = "lblDateFrom"
		Me.lblDateFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDateFrom.Size = New System.Drawing.Size(81, 17)
		Me.lblDateFrom.TabIndex = 13
		Me.lblDateFrom.Text = "From:"
		Me.lblDateFrom.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDateFrom.UseMnemonic = True
		Me.lblDateFrom.Visible = True
		' 
		' lblDateTo
		' 
		Me.lblDateTo.AutoSize = False
		Me.lblDateTo.BackColor = System.Drawing.SystemColors.Control
		Me.lblDateTo.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDateTo.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDateTo.Enabled = True
		Me.lblDateTo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDateTo.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDateTo.Location = New System.Drawing.Point(312, 98)
		Me.lblDateTo.Name = "lblDateTo"
		Me.lblDateTo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDateTo.Size = New System.Drawing.Size(49, 17)
		Me.lblDateTo.TabIndex = 14
		Me.lblDateTo.Text = "To:"
		Me.lblDateTo.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDateTo.UseMnemonic = True
		Me.lblDateTo.Visible = True
		' 
		' lblOperatorID
		' 
		Me.lblOperatorID.AutoSize = False
		Me.lblOperatorID.BackColor = System.Drawing.SystemColors.Control
		Me.lblOperatorID.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblOperatorID.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblOperatorID.Enabled = True
		Me.lblOperatorID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblOperatorID.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblOperatorID.Location = New System.Drawing.Point(16, 14)
		Me.lblOperatorID.Name = "lblOperatorID"
		Me.lblOperatorID.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblOperatorID.Size = New System.Drawing.Size(101, 17)
		Me.lblOperatorID.TabIndex = 40
		Me.lblOperatorID.Text = "Operator Name:"
		Me.lblOperatorID.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblOperatorID.UseMnemonic = True
		Me.lblOperatorID.Visible = True
		' 
		' lblDepartment
		' 
		Me.lblDepartment.AutoSize = False
		Me.lblDepartment.BackColor = System.Drawing.SystemColors.Control
		Me.lblDepartment.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDepartment.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDepartment.Enabled = True
		Me.lblDepartment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDepartment.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDepartment.Location = New System.Drawing.Point(312, 14)
		Me.lblDepartment.Name = "lblDepartment"
		Me.lblDepartment.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDepartment.Size = New System.Drawing.Size(81, 17)
		Me.lblDepartment.TabIndex = 42
		Me.lblDepartment.Text = "Department:"
		Me.lblDepartment.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDepartment.UseMnemonic = True
		Me.lblDepartment.Visible = True
		' 
		' lblCurrency
		' 
		Me.lblCurrency.AutoSize = False
		Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
		Me.lblCurrency.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCurrency.Enabled = True
		Me.lblCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCurrency.Location = New System.Drawing.Point(312, 44)
		Me.lblCurrency.Name = "lblCurrency"
		Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCurrency.Size = New System.Drawing.Size(81, 17)
		Me.lblCurrency.TabIndex = 44
		Me.lblCurrency.Text = "Currency:"
		Me.lblCurrency.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCurrency.UseMnemonic = True
		Me.lblCurrency.Visible = True
		' 
		' lblSubBranch
		' 
		Me.lblSubBranch.AutoSize = True
		Me.lblSubBranch.BackColor = System.Drawing.SystemColors.Control
		Me.lblSubBranch.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSubBranch.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSubBranch.Enabled = True
		Me.lblSubBranch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSubBranch.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSubBranch.Location = New System.Drawing.Point(16, 128)
		Me.lblSubBranch.Name = "lblSubBranch"
		Me.lblSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSubBranch.Size = New System.Drawing.Size(120, 13)
		Me.lblSubBranch.TabIndex = 46
		Me.lblSubBranch.Text = "Current Sub-Branch:"
		Me.lblSubBranch.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblSubBranch.UseMnemonic = True
		Me.lblSubBranch.Visible = True
		' 
		' lblDocumentType
		' 
		Me.lblDocumentType.AutoSize = False
		Me.lblDocumentType.BackColor = System.Drawing.SystemColors.Control
		Me.lblDocumentType.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDocumentType.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDocumentType.Enabled = True
		Me.lblDocumentType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDocumentType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDocumentType.Location = New System.Drawing.Point(16, 72)
		Me.lblDocumentType.Name = "lblDocumentType"
		Me.lblDocumentType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDocumentType.Size = New System.Drawing.Size(115, 17)
		Me.lblDocumentType.TabIndex = 48
		Me.lblDocumentType.Text = "Document Type:"
		Me.lblDocumentType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDocumentType.UseMnemonic = True
		Me.lblDocumentType.Visible = True
		' 
		' cmbCurrency
		' 
		Me.cmbCurrency.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmbCurrency.Location = New System.Drawing.Point(416, 40)
		Me.cmbCurrency.Name = "cmbCurrency"
		Me.cmbCurrency.PMLookupProductFamily = 3
		Me.cmbCurrency.Size = New System.Drawing.Size(185, 21)
		Me.cmbCurrency.TabIndex = 43
        'Developer Guide No.77
        'Me.cmbCurrency.Table = "Currency"
        Me.cmbCurrency.TableName = "Currency"
		' 
		' cboDepartment
		' 
		Me.cboDepartment.FirstItem = "(all)"
		Me.cboDepartment.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboDepartment.Location = New System.Drawing.Point(416, 12)
		Me.cboDepartment.Name = "cboDepartment"
		Me.cboDepartment.PMLookupProductFamily = 3
		Me.cboDepartment.Size = New System.Drawing.Size(185, 21)
        Me.cboDepartment.TabIndex = 41
        'Developer Guide No.77
        'Me.cboDepartment.Table = "Department"
        Me.cboDepartment.TableName = "Department"
		' 
		' cboPMUser
		' 
		Me.cboPMUser.FirstItem = "(all)"
		Me.cboPMUser.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboPMUser.Location = New System.Drawing.Point(142, 12)
		Me.cboPMUser.Name = "cboPMUser"
		Me.cboPMUser.Size = New System.Drawing.Size(153, 21)
		Me.cboPMUser.Sorted = True
		Me.cboPMUser.TabIndex = 39
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
		Me.txtDateTo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDateTo.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDateTo.HideSelection = True
		Me.txtDateTo.Location = New System.Drawing.Point(416, 96)
		Me.txtDateTo.MaxLength = 0
		Me.txtDateTo.Multiline = False
		Me.txtDateTo.Name = "txtDateTo"
		Me.txtDateTo.ReadOnly = False
		Me.txtDateTo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDateTo.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDateTo.Size = New System.Drawing.Size(113, 19)
		Me.txtDateTo.TabIndex = 7
		Me.txtDateTo.TabStop = True
		Me.txtDateTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDateTo.Visible = True
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
		Me.txtDateFrom.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDateFrom.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDateFrom.HideSelection = True
		Me.txtDateFrom.Location = New System.Drawing.Point(416, 68)
		Me.txtDateFrom.MaxLength = 0
		Me.txtDateFrom.Multiline = False
		Me.txtDateFrom.Name = "txtDateFrom"
		Me.txtDateFrom.ReadOnly = False
		Me.txtDateFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDateFrom.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDateFrom.Size = New System.Drawing.Size(113, 19)
		Me.txtDateFrom.TabIndex = 8
		Me.txtDateFrom.TabStop = True
		Me.txtDateFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDateFrom.Visible = True
		' 
		' cmbPeriod
		' 
		Me.cmbPeriod.BackColor = System.Drawing.SystemColors.Window
		Me.cmbPeriod.CausesValidation = True
		Me.cmbPeriod.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmbPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cmbPeriod.Enabled = True
		Me.cmbPeriod.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmbPeriod.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cmbPeriod.IntegralHeight = True
		Me.cmbPeriod.Location = New System.Drawing.Point(142, 96)
		Me.cmbPeriod.Name = "cmbPeriod"
		Me.cmbPeriod.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmbPeriod.Size = New System.Drawing.Size(153, 21)
		Me.cmbPeriod.Sorted = False
		Me.cmbPeriod.TabIndex = 9
		Me.cmbPeriod.TabStop = True
		Me.cmbPeriod.Visible = True
		' 
		' cmbDocTypeGroup
		' 
		Me.cmbDocTypeGroup.BackColor = System.Drawing.SystemColors.Window
		Me.cmbDocTypeGroup.CausesValidation = True
		Me.cmbDocTypeGroup.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmbDocTypeGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cmbDocTypeGroup.Enabled = True
		Me.cmbDocTypeGroup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmbDocTypeGroup.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cmbDocTypeGroup.IntegralHeight = True
		Me.cmbDocTypeGroup.Location = New System.Drawing.Point(142, 40)
		Me.cmbDocTypeGroup.Name = "cmbDocTypeGroup"
		Me.cmbDocTypeGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmbDocTypeGroup.Size = New System.Drawing.Size(153, 21)
		Me.cmbDocTypeGroup.Sorted = False
		Me.cmbDocTypeGroup.TabIndex = 10
		Me.cmbDocTypeGroup.TabStop = True
		Me.cmbDocTypeGroup.Visible = True
		' 
		' cboSubBranch
		' 
		Me.cboSubBranch.BackColor = System.Drawing.SystemColors.Window
		Me.cboSubBranch.CausesValidation = True
		Me.cboSubBranch.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboSubBranch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown
		Me.cboSubBranch.Enabled = True
		Me.cboSubBranch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboSubBranch.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboSubBranch.IntegralHeight = True
		Me.cboSubBranch.Location = New System.Drawing.Point(142, 124)
		Me.cboSubBranch.Name = "cboSubBranch"
		Me.cboSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboSubBranch.Size = New System.Drawing.Size(153, 21)
		Me.cboSubBranch.Sorted = False
		Me.cboSubBranch.TabIndex = 45
		Me.cboSubBranch.TabStop = True
		Me.cboSubBranch.Text = "cboSubBranch"
		Me.cboSubBranch.Visible = True
		' 
		' cmbDocumentType
		' 
		Me.cmbDocumentType.BackColor = System.Drawing.SystemColors.Window
		Me.cmbDocumentType.CausesValidation = True
		Me.cmbDocumentType.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmbDocumentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cmbDocumentType.Enabled = True
		Me.cmbDocumentType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmbDocumentType.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cmbDocumentType.IntegralHeight = True
		Me.cmbDocumentType.Location = New System.Drawing.Point(142, 68)
		Me.cmbDocumentType.Name = "cmbDocumentType"
		Me.cmbDocumentType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmbDocumentType.Size = New System.Drawing.Size(153, 21)
		Me.cmbDocumentType.Sorted = False
		Me.cmbDocumentType.TabIndex = 47
		Me.cmbDocumentType.TabStop = True
		Me.cmbDocumentType.Visible = True
		' 
		' cmdFindNow
		' 
		Me.cmdFindNow.BackColor = System.Drawing.SystemColors.Control
		Me.cmdFindNow.CausesValidation = True
		Me.cmdFindNow.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdFindNow.Enabled = False
		Me.cmdFindNow.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdFindNow.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdFindNow.Location = New System.Drawing.Point(640, 52)
		Me.cmdFindNow.Name = "cmdFindNow"
		Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdFindNow.Size = New System.Drawing.Size(73, 22)
		Me.cmdFindNow.TabIndex = 1
		Me.cmdFindNow.TabStop = False
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
		Me.cmdHelp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdHelp.Location = New System.Drawing.Point(640, 416)
		Me.cmdHelp.Name = "cmdHelp"
		Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.cmdHelp.TabIndex = 4
		Me.cmdHelp.TabStop = False
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
		Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(560, 416)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 3
		Me.cmdCancel.TabStop = False
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = False
		Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(480, 416)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 2
		Me.cmdOK.TabStop = False
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' stbStatus
		' 
		Me.stbStatus.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.stbStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.stbStatus.Location = New System.Drawing.Point(0, 448)
		Me.stbStatus.Name = "stbStatus"
		Me.stbStatus.ShowItemToolTips = True
		Me.stbStatus.Size = New System.Drawing.Size(721, 18)
		Me.stbStatus.TabIndex = 5
		Me.stbStatus.Text = ""
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
		Me._stbStatus_Panel1.Name = ""
		Me._stbStatus_Panel1.Size = New System.Drawing.Size(704, 18)
		Me._stbStatus_Panel1.Tag = ""
		Me._stbStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._stbStatus_Panel1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
		' 
		' cmdAllocate
		' 
		Me.cmdAllocate.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAllocate.CausesValidation = True
		Me.cmdAllocate.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAllocate.Enabled = True
		Me.cmdAllocate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAllocate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAllocate.Location = New System.Drawing.Point(8, 416)
		Me.cmdAllocate.Name = "cmdAllocate"
		Me.cmdAllocate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAllocate.Size = New System.Drawing.Size(73, 22)
		Me.cmdAllocate.TabIndex = 32
		Me.cmdAllocate.TabStop = True
		Me.cmdAllocate.Text = "&Allocate"
		Me.cmdAllocate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAllocate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdMark
		' 
		Me.cmdMark.BackColor = System.Drawing.SystemColors.Control
		Me.cmdMark.CausesValidation = True
		Me.cmdMark.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdMark.Enabled = True
		Me.cmdMark.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdMark.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdMark.Location = New System.Drawing.Point(88, 416)
		Me.cmdMark.Name = "cmdMark"
		Me.cmdMark.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdMark.Size = New System.Drawing.Size(73, 22)
		Me.cmdMark.TabIndex = 33
		Me.cmdMark.TabStop = True
		Me.cmdMark.Text = "&Mark"
		Me.cmdMark.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdMark.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdWriteOff
		' 
		Me.cmdWriteOff.BackColor = System.Drawing.SystemColors.Control
		Me.cmdWriteOff.CausesValidation = True
		Me.cmdWriteOff.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdWriteOff.Enabled = True
		Me.cmdWriteOff.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdWriteOff.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdWriteOff.Location = New System.Drawing.Point(168, 416)
		Me.cmdWriteOff.Name = "cmdWriteOff"
		Me.cmdWriteOff.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdWriteOff.Size = New System.Drawing.Size(73, 22)
		Me.cmdWriteOff.TabIndex = 38
		Me.cmdWriteOff.TabStop = True
		Me.cmdWriteOff.Text = "&WriteOff"
		Me.cmdWriteOff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdWriteOff.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' ImgImage
		' 
		Me.ImgImage.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.ImgImage.Cursor = System.Windows.Forms.Cursors.Default
		Me.ImgImage.Enabled = True
		Me.ImgImage.Image = CType(resources.GetObject("ImgImage.Image"), System.Drawing.Image)
		Me.ImgImage.Location = New System.Drawing.Point(664, 152)
		Me.ImgImage.Name = "ImgImage"
		Me.ImgImage.Size = New System.Drawing.Size(32, 32)
		Me.ImgImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.ImgImage.Visible = True
		' 
		' imglImages
		' 
		Me.imglImages.ImageSize = New System.Drawing.Size(16, 16)
		Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(255, 255, 255)
		Me.imglImages.Images.SetKeyName(0, "check")
		' 
		' frmInterface
		' 
		Me.AcceptButton = Me.cmdFindNow
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdFindNow
		Me.ClientSize = New System.Drawing.Size(721, 466)
		Me.ControlBox = True
		Me.Controls.Add(Me.TabResults)
		Me.Controls.Add(Me.cmdNewSearch)
		Me.Controls.Add(Me.tabMain)
		Me.Controls.Add(Me.cmdFindNow)
		Me.Controls.Add(Me.cmdHelp)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.stbStatus)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cmdAllocate)
		Me.Controls.Add(Me.cmdMark)
		Me.Controls.Add(Me.cmdWriteOff)
		Me.Controls.Add(Me.ImgImage)
		Me.Controls.Add(MainMenu1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = True
		Me.Icon = CType(resources.GetObject("frmInterface.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(4, 9)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Batch Allocation"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.TabResults, 2)
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMain, 2)
		Me.listViewHelper1.SetItemClickMethod(Me.lvwSearchDebits, "lvwSearchDebits_ItemClick")
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwSearchDebits, True)
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwSearchCredits, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.TabResults.ResumeLayout(False)
		Me._TabResults_TabPage0.ResumeLayout(False)
		Me.lvwSearchDebits.ResumeLayout(False)
		Me._TabResults_TabPage1.ResumeLayout(False)
		Me.lvwSearchCredits.ResumeLayout(False)
		Me.tabMain.ResumeLayout(False)
		Me._tabMain_TabPage0.ResumeLayout(False)
		Me._tabMain_TabPage1.ResumeLayout(False)
		Me.stbStatus.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub lvwSearchCredits_InitializeColumnKeys()
		Me._lvwSearchCredits_ColumnHeader_1.Name = ""
		Me._lvwSearchCredits_ColumnHeader_2.Name = ""
		Me._lvwSearchCredits_ColumnHeader_3.Name = ""
		Me._lvwSearchCredits_ColumnHeader_4.Name = ""
		Me._lvwSearchCredits_ColumnHeader_5.Name = ""
		Me._lvwSearchCredits_ColumnHeader_6.Name = ""
		Me._lvwSearchCredits_ColumnHeader_7.Name = ""
		Me._lvwSearchCredits_ColumnHeader_8.Name = ""
		Me._lvwSearchCredits_ColumnHeader_9.Name = ""
		Me._lvwSearchCredits_ColumnHeader_10.Name = ""
		Me._lvwSearchCredits_ColumnHeader_11.Name = ""
		Me._lvwSearchCredits_ColumnHeader_12.Name = ""
		Me._lvwSearchCredits_ColumnHeader_13.Name = ""
		Me._lvwSearchCredits_ColumnHeader_14.Name = ""
		Me._lvwSearchCredits_ColumnHeader_15.Name = ""
		Me._lvwSearchCredits_ColumnHeader_16.Name = ""
		Me._lvwSearchCredits_ColumnHeader_17.Name = ""
		Me._lvwSearchCredits_ColumnHeader_18.Name = ""
		Me._lvwSearchCredits_ColumnHeader_19.Name = ""
		Me._lvwSearchCredits_ColumnHeader_20.Name = ""
	End Sub
	Sub lvwSearchDebits_InitializeColumnKeys()
		Me._lvwSearchDebits_ColumnHeader_1.Name = ""
		Me._lvwSearchDebits_ColumnHeader_2.Name = ""
		Me._lvwSearchDebits_ColumnHeader_3.Name = ""
		Me._lvwSearchDebits_ColumnHeader_4.Name = ""
		Me._lvwSearchDebits_ColumnHeader_5.Name = ""
		Me._lvwSearchDebits_ColumnHeader_6.Name = ""
		Me._lvwSearchDebits_ColumnHeader_7.Name = ""
		Me._lvwSearchDebits_ColumnHeader_8.Name = ""
		Me._lvwSearchDebits_ColumnHeader_9.Name = ""
		Me._lvwSearchDebits_ColumnHeader_10.Name = ""
		Me._lvwSearchDebits_ColumnHeader_11.Name = ""
		Me._lvwSearchDebits_ColumnHeader_12.Name = ""
		Me._lvwSearchDebits_ColumnHeader_13.Name = ""
		Me._lvwSearchDebits_ColumnHeader_14.Name = ""
		Me._lvwSearchDebits_ColumnHeader_15.Name = ""
		Me._lvwSearchDebits_ColumnHeader_16.Name = ""
		Me._lvwSearchDebits_ColumnHeader_17.Name = ""
		Me._lvwSearchDebits_ColumnHeader_18.Name = ""
		Me._lvwSearchDebits_ColumnHeader_19.Name = ""
		Me._lvwSearchDebits_ColumnHeader_20.Name = ""
	End Sub
#End Region 
End Class