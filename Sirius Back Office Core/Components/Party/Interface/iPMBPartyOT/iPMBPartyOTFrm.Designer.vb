<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializecmdPrevious()
		InitializecmdNext()
		lvwAccidents_InitializeColumnKeys()
		lvwConvictions_InitializeColumnKeys()
		lvwSupSpecSelected_InitializeColumnKeys()
		lvwSupSpecAvailable_InitializeColumnKeys()
		lvwSupBusSelected_InitializeColumnKeys()
		lvwSupBusAvailable_InitializeColumnKeys()
		lvwAddresses_InitializeColumnKeys()
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
    Private WithEvents _Toolbar1_Button1 As System.Windows.Forms.ToolStripButton
    Private WithEvents _Toolbar1_Button2 As System.Windows.Forms.ToolStripButton
    Private WithEvents _Toolbar1_Button3 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents _Toolbar1_Button4 As System.Windows.Forms.ToolStripButton
    Private WithEvents _Toolbar1_Button5 As System.Windows.Forms.ToolStripButton
    Public WithEvents Toolbar1 As System.Windows.Forms.ToolStrip
    Public WithEvents cmdApply As System.Windows.Forms.Button
    Public WithEvents txtCurrency As System.Windows.Forms.TextBox
    Public WithEvents txtDate As System.Windows.Forms.TextBox
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents Image1 As System.Windows.Forms.PictureBox
    Public WithEvents txtReference As System.Windows.Forms.TextBox
    Public WithEvents txtDatePassedTest As System.Windows.Forms.TextBox
    Public WithEvents uctBranch As System.Windows.Forms.ComboBox
    Public WithEvents cboSubBranch As System.Windows.Forms.ComboBox
    Public WithEvents cboPriority As System.Windows.Forms.ComboBox
    Public WithEvents cboAfterHours As System.Windows.Forms.ComboBox
    Public WithEvents cboActive As System.Windows.Forms.ComboBox
    Public WithEvents lblPriority As System.Windows.Forms.Label
    Public WithEvents lblAfterHours As System.Windows.Forms.Label
    Public WithEvents lblActive As System.Windows.Forms.Label
    Public WithEvents fraSupplier As System.Windows.Forms.Panel
    Public WithEvents txtRegNumber As System.Windows.Forms.TextBox
    Public WithEvents cboStatus As System.Windows.Forms.ComboBox
    Public WithEvents txtLicenceNumber As System.Windows.Forms.TextBox
    Public WithEvents cboLicenceType As System.Windows.Forms.ComboBox
    Public WithEvents txtDateOfBirth As System.Windows.Forms.TextBox
    Public WithEvents txtPartyCode As System.Windows.Forms.TextBox
    Public WithEvents txtPartyName As System.Windows.Forms.TextBox
    Public WithEvents ddGender As PMListMgrDropdown.uctDropdown
    Public WithEvents uctCurrency As UserControls.CurrencyLookup
    Public WithEvents txtCompanyNotes As System.Windows.Forms.TextBox
    Public WithEvents lblCompanyNotes As System.Windows.Forms.Label
    Public WithEvents lblReference As System.Windows.Forms.Label
    Public WithEvents lblDatePassedTest As System.Windows.Forms.Label
    Public WithEvents lblCurrency As System.Windows.Forms.Label
    Public WithEvents lblSubBranch As System.Windows.Forms.Label
    Public WithEvents lblBranch As System.Windows.Forms.Label
    Public WithEvents lblRegNumber As System.Windows.Forms.Label
    Public WithEvents lblStatus As System.Windows.Forms.Label
    Public WithEvents lblLicenceNumber As System.Windows.Forms.Label
    Public WithEvents lblLicenceType As System.Windows.Forms.Label
    Public WithEvents lblGender As System.Windows.Forms.Label
    Public WithEvents lblDateOfBirth As System.Windows.Forms.Label
    Public WithEvents lblPartyCode As System.Windows.Forms.Label
    Public WithEvents lblPartyName As System.Windows.Forms.Label
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Private WithEvents _cmdNext_0 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Private WithEvents _lvwAddresses_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddresses_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddresses_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddresses_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddresses_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddresses_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwAddresses As System.Windows.Forms.ListView
    Public WithEvents cmdEditAddress As System.Windows.Forms.Button
    Public WithEvents cmdDeleteAddress As System.Windows.Forms.Button
    Public WithEvents cmdAddAddress As System.Windows.Forms.Button
    Public WithEvents fraAddress As System.Windows.Forms.GroupBox
    Private WithEvents _cmdPrevious_0 As System.Windows.Forms.Button
    Private WithEvents _cmdNext_1 As System.Windows.Forms.Button
    Public WithEvents txtContactTelephoneNo As System.Windows.Forms.TextBox
    Public WithEvents txtContactName As System.Windows.Forms.TextBox
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents lblContactName As System.Windows.Forms.Label
    Public WithEvents fraContactDetails As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
    Private WithEvents _cmdNext_2 As System.Windows.Forms.Button
    Private WithEvents _cmdPrevious_1 As System.Windows.Forms.Button
    Public WithEvents cmdBusinessAdd As System.Windows.Forms.Button
    Public WithEvents cmdBusinessRemove As System.Windows.Forms.Button
    Private WithEvents _lvwSupBusAvailable_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwSupBusAvailable As System.Windows.Forms.ListView
    Private WithEvents _lvwSupBusSelected_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwSupBusSelected As System.Windows.Forms.ListView
    Public WithEvents fraSupply As System.Windows.Forms.GroupBox
    Public WithEvents cmdSpecialityRemove As System.Windows.Forms.Button
    Public WithEvents cmdSpecialityAdd As System.Windows.Forms.Button
    Private WithEvents _lvwSupSpecAvailable_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwSupSpecAvailable As System.Windows.Forms.ListView
    Private WithEvents _lvwSupSpecSelected_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwSupSpecSelected As System.Windows.Forms.ListView
    Public WithEvents fraSpeciality As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
    Private WithEvents _cmdPrevious_2 As System.Windows.Forms.Button
    Public WithEvents cmdEditConviction As System.Windows.Forms.Button
    Public WithEvents cmdDeleteConviction As System.Windows.Forms.Button
    Public WithEvents cmdAddConviction As System.Windows.Forms.Button
    Private WithEvents _lvwConvictions_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwConvictions_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwConvictions_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwConvictions_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwConvictions_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwConvictions_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwConvictions As System.Windows.Forms.ListView
    Public WithEvents Frame2 As System.Windows.Forms.GroupBox
    Private WithEvents _cmdNext_3 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage3 As System.Windows.Forms.TabPage
    Public WithEvents cmdEditAccident As System.Windows.Forms.Button
    Public WithEvents cmdDeleteAccident As System.Windows.Forms.Button
    Public WithEvents cmdAddAccident As System.Windows.Forms.Button
    Private WithEvents _lvwAccidents_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAccidents_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAccidents_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwAccidents As System.Windows.Forms.ListView
    Public WithEvents fraAccident As System.Windows.Forms.GroupBox
    Private WithEvents _cmdPrevious_3 As System.Windows.Forms.Button
    Private WithEvents _cmdNext_4 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage4 As System.Windows.Forms.TabPage
    Private WithEvents _cmdNext_5 As System.Windows.Forms.Button
    Private WithEvents _cmdPrevious_4 As System.Windows.Forms.Button
    Public WithEvents uctPartyTax1 As uctPartyTaxControl.uctPartyTax
    Private WithEvents _tabMainTab_TabPage5 As System.Windows.Forms.TabPage
    Private WithEvents _cmdPrevious_5 As System.Windows.Forms.Button
    Public WithEvents txtInsurerNotes As System.Windows.Forms.TextBox
    Public WithEvents uctPMAddressControl1 As PMAddressControl.uctPMAddressControl
    Public WithEvents txtInsurerEmailAddress As System.Windows.Forms.TextBox
    Public WithEvents txtInsurerContactName As System.Windows.Forms.TextBox
    Public WithEvents txtInsurerFaxNo As System.Windows.Forms.TextBox
    Public WithEvents txtInsurerTelNo As System.Windows.Forms.TextBox
    Public WithEvents txtInsurerName As System.Windows.Forms.TextBox
    Public WithEvents lblInsurerNotes As System.Windows.Forms.Label
    Public WithEvents lblInsurerEmailAddress As System.Windows.Forms.Label
    Public WithEvents lblInsurerContactName As System.Windows.Forms.Label
    Public WithEvents lblInsurerFaxNo As System.Windows.Forms.Label
    Public WithEvents lblInsurerTelNo As System.Windows.Forms.Label
    Public WithEvents lblInsurerName As System.Windows.Forms.Label
    Public WithEvents fraInsurerDetails As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage6 As System.Windows.Forms.TabPage
    Private WithEvents _cmdPrevious_6 As System.Windows.Forms.Button
    Public WithEvents uctPartyBankControl1 As uctPartyBank.uctPartyBankControl
    Private WithEvents _tabMainTab_TabPage7 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public WithEvents ImageList2 As System.Windows.Forms.ImageList
    Public cmdNext(5) As System.Windows.Forms.Button
    Public cmdPrevious(6) As System.Windows.Forms.Button
    'Developer Guide No.(commented the line as it was conflicting with the listview icon)
    Private tabMainTabPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Toolbar1 = New System.Windows.Forms.ToolStrip
        Me.ImageList2 = New System.Windows.Forms.ImageList(Me.components)
        Me._Toolbar1_Button1 = New System.Windows.Forms.ToolStripButton
        Me._Toolbar1_Button2 = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me._Toolbar1_Button4 = New System.Windows.Forms.ToolStripButton
        Me._Toolbar1_Button5 = New System.Windows.Forms.ToolStripButton
        Me._Toolbar1_Button3 = New System.Windows.Forms.ToolStripSeparator
        Me.cmdApply = New System.Windows.Forms.Button
        Me.txtCurrency = New System.Windows.Forms.TextBox
        Me.txtDate = New System.Windows.Forms.TextBox
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.txtReference = New System.Windows.Forms.TextBox
        Me.txtDatePassedTest = New System.Windows.Forms.TextBox
        Me.uctBranch = New System.Windows.Forms.ComboBox
        Me.cboSubBranch = New System.Windows.Forms.ComboBox
        Me.fraSupplier = New System.Windows.Forms.Panel
        Me.cboPriority = New System.Windows.Forms.ComboBox
        Me.cboAfterHours = New System.Windows.Forms.ComboBox
        Me.cboActive = New System.Windows.Forms.ComboBox
        Me.lblPriority = New System.Windows.Forms.Label
        Me.lblAfterHours = New System.Windows.Forms.Label
        Me.lblActive = New System.Windows.Forms.Label
        Me.txtRegNumber = New System.Windows.Forms.TextBox
        Me.cboStatus = New System.Windows.Forms.ComboBox
        Me.txtLicenceNumber = New System.Windows.Forms.TextBox
        Me.cboLicenceType = New System.Windows.Forms.ComboBox
        Me.txtDateOfBirth = New System.Windows.Forms.TextBox
        Me.txtPartyCode = New System.Windows.Forms.TextBox
        Me.txtPartyName = New System.Windows.Forms.TextBox
        Me.ddGender = New PMListMgrDropdown.uctDropdown
        Me.uctCurrency = New UserControls.CurrencyLookup
        Me.txtCompanyNotes = New System.Windows.Forms.TextBox
        Me.lblCompanyNotes = New System.Windows.Forms.Label
        Me.lblReference = New System.Windows.Forms.Label
        Me.lblDatePassedTest = New System.Windows.Forms.Label
        Me.lblCurrency = New System.Windows.Forms.Label
        Me.lblSubBranch = New System.Windows.Forms.Label
        Me.lblBranch = New System.Windows.Forms.Label
        Me.lblRegNumber = New System.Windows.Forms.Label
        Me.lblStatus = New System.Windows.Forms.Label
        Me.lblLicenceNumber = New System.Windows.Forms.Label
        Me.lblLicenceType = New System.Windows.Forms.Label
        Me.lblGender = New System.Windows.Forms.Label
        Me.lblDateOfBirth = New System.Windows.Forms.Label
        Me.lblPartyCode = New System.Windows.Forms.Label
        Me.lblPartyName = New System.Windows.Forms.Label
        Me._cmdNext_0 = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me.fraAddress = New System.Windows.Forms.GroupBox
        Me.lvwAddresses = New System.Windows.Forms.ListView
        Me._lvwAddresses_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddresses_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddresses_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddresses_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddresses_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddresses_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me.cmdEditAddress = New System.Windows.Forms.Button
        Me.cmdDeleteAddress = New System.Windows.Forms.Button
        Me.cmdAddAddress = New System.Windows.Forms.Button
        Me._cmdPrevious_0 = New System.Windows.Forms.Button
        Me._cmdNext_1 = New System.Windows.Forms.Button
        Me.fraContactDetails = New System.Windows.Forms.GroupBox
        Me.txtContactTelephoneNo = New System.Windows.Forms.TextBox
        Me.txtContactName = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblContactName = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage
        Me._cmdNext_2 = New System.Windows.Forms.Button
        Me._cmdPrevious_1 = New System.Windows.Forms.Button
        Me.fraSupply = New System.Windows.Forms.GroupBox
        Me.cmdBusinessAdd = New System.Windows.Forms.Button
        Me.cmdBusinessRemove = New System.Windows.Forms.Button
        Me.lvwSupBusAvailable = New System.Windows.Forms.ListView
        Me._lvwSupBusAvailable_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me.lvwSupBusSelected = New System.Windows.Forms.ListView
        Me._lvwSupBusSelected_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me.fraSpeciality = New System.Windows.Forms.GroupBox
        Me.cmdSpecialityRemove = New System.Windows.Forms.Button
        Me.cmdSpecialityAdd = New System.Windows.Forms.Button
        Me.lvwSupSpecAvailable = New System.Windows.Forms.ListView
        Me._lvwSupSpecAvailable_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me.lvwSupSpecSelected = New System.Windows.Forms.ListView
        Me._lvwSupSpecSelected_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._tabMainTab_TabPage3 = New System.Windows.Forms.TabPage
        Me._cmdPrevious_2 = New System.Windows.Forms.Button
        Me.Frame2 = New System.Windows.Forms.GroupBox
        Me.cmdEditConviction = New System.Windows.Forms.Button
        Me.cmdDeleteConviction = New System.Windows.Forms.Button
        Me.cmdAddConviction = New System.Windows.Forms.Button
        Me.lvwConvictions = New System.Windows.Forms.ListView
        Me._lvwConvictions_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwConvictions_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwConvictions_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwConvictions_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwConvictions_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwConvictions_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._cmdNext_3 = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage4 = New System.Windows.Forms.TabPage
        Me.fraAccident = New System.Windows.Forms.GroupBox
        Me.cmdEditAccident = New System.Windows.Forms.Button
        Me.cmdDeleteAccident = New System.Windows.Forms.Button
        Me.cmdAddAccident = New System.Windows.Forms.Button
        Me.lvwAccidents = New System.Windows.Forms.ListView
        Me._lvwAccidents_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwAccidents_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwAccidents_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._cmdPrevious_3 = New System.Windows.Forms.Button
        Me._cmdNext_4 = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage5 = New System.Windows.Forms.TabPage
        Me.Label2 = New System.Windows.Forms.Label
        Me._cmdNext_5 = New System.Windows.Forms.Button
        Me._cmdPrevious_4 = New System.Windows.Forms.Button
        Me.uctPartyTax1 = New uctPartyTaxControl.uctPartyTax
        Me._tabMainTab_TabPage6 = New System.Windows.Forms.TabPage
        Me._cmdPrevious_5 = New System.Windows.Forms.Button
        Me.fraInsurerDetails = New System.Windows.Forms.GroupBox
        Me.txtInsurerNotes = New System.Windows.Forms.TextBox
        Me.uctPMAddressControl1 = New PMAddressControl.uctPMAddressControl
        Me.txtInsurerEmailAddress = New System.Windows.Forms.TextBox
        Me.txtInsurerContactName = New System.Windows.Forms.TextBox
        Me.txtInsurerFaxNo = New System.Windows.Forms.TextBox
        Me.txtInsurerTelNo = New System.Windows.Forms.TextBox
        Me.txtInsurerName = New System.Windows.Forms.TextBox
        Me.lblInsurerNotes = New System.Windows.Forms.Label
        Me.lblInsurerEmailAddress = New System.Windows.Forms.Label
        Me.lblInsurerContactName = New System.Windows.Forms.Label
        Me.lblInsurerFaxNo = New System.Windows.Forms.Label
        Me.lblInsurerTelNo = New System.Windows.Forms.Label
        Me.lblInsurerName = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage7 = New System.Windows.Forms.TabPage
        Me._cmdNext_6 = New System.Windows.Forms.Button
        Me._cmdPrevious_6 = New System.Windows.Forms.Button
        Me.uctPartyBankControl1 = New uctPartyBank.uctPartyBankControl
        Me._tabMainTab_TabPage8 = New System.Windows.Forms.TabPage
        Me.uctPickListBranches = New uctPickList.PickList
        Me._cmdPrevious_7 = New System.Windows.Forms.Button
        Me.Image1 = New System.Windows.Forms.PictureBox
        Me.cboTPASettle = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Toolbar1.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me.fraSupplier.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me.fraAddress.SuspendLayout()
        Me.fraContactDetails.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me.fraSupply.SuspendLayout()
        Me.fraSpeciality.SuspendLayout()
        Me._tabMainTab_TabPage3.SuspendLayout()
        Me.Frame2.SuspendLayout()
        Me._tabMainTab_TabPage4.SuspendLayout()
        Me.fraAccident.SuspendLayout()
        Me._tabMainTab_TabPage5.SuspendLayout()
        Me._tabMainTab_TabPage6.SuspendLayout()
        Me.fraInsurerDetails.SuspendLayout()
        Me._tabMainTab_TabPage7.SuspendLayout()
        Me._tabMainTab_TabPage8.SuspendLayout()
        CType(Me.Image1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Toolbar1
        '
        Me.Toolbar1.ImageList = Me.ImageList2
        Me.Toolbar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._Toolbar1_Button1, Me._Toolbar1_Button2, Me.ToolStripSeparator1, Me._Toolbar1_Button4, Me._Toolbar1_Button5})
        Me.Toolbar1.Location = New System.Drawing.Point(0, 0)
        Me.Toolbar1.Name = "Toolbar1"
        Me.Toolbar1.Size = New System.Drawing.Size(649, 25)
        Me.Toolbar1.TabIndex = 100
        '
        'ImageList2
        '
        Me.ImageList2.ImageStream = CType(resources.GetObject("ImageList2.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList2.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList2.Images.SetKeyName(0, "FINANCIAL")
        Me.ImageList2.Images.SetKeyName(1, "")
        Me.ImageList2.Images.SetKeyName(2, "NOTE")
        Me.ImageList2.Images.SetKeyName(3, "LETTER")
        Me.ImageList2.Images.SetKeyName(4, "COMMISSION")
        Me.ImageList2.Images.SetKeyName(5, "ADDRESS")
        Me.ImageList2.Images.SetKeyName(6, "")
        Me.ImageList2.Images.SetKeyName(7, "CONVICTIONIMAGE")
        Me.ImageList2.Images.SetKeyName(8, "ACCIDENTIMAGE")
        '
        '_Toolbar1_Button1
        '
        Me._Toolbar1_Button1.AutoSize = False
        Me._Toolbar1_Button1.ImageKey = "FINANCIAL"
        Me._Toolbar1_Button1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Toolbar1_Button1.Name = "_Toolbar1_Button1"
        Me._Toolbar1_Button1.Size = New System.Drawing.Size(24, 22)
        Me._Toolbar1_Button1.Tag = ""
        Me._Toolbar1_Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Toolbar1_Button1.ToolTipText = "Financial"
        '
        '_Toolbar1_Button2
        '
        Me._Toolbar1_Button2.AutoSize = False
        Me._Toolbar1_Button2.ImageKey = "COMMISSION"
        Me._Toolbar1_Button2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Toolbar1_Button2.Name = "_Toolbar1_Button2"
        Me._Toolbar1_Button2.Size = New System.Drawing.Size(24, 22)
        Me._Toolbar1_Button2.Tag = ""
        Me._Toolbar1_Button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Toolbar1_Button2.ToolTipText = "Commission"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        '_Toolbar1_Button4
        '
        Me._Toolbar1_Button4.AutoSize = False
        Me._Toolbar1_Button4.ImageKey = "NOTE"
        Me._Toolbar1_Button4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Toolbar1_Button4.Name = "_Toolbar1_Button4"
        Me._Toolbar1_Button4.Size = New System.Drawing.Size(24, 22)
        Me._Toolbar1_Button4.Tag = ""
        Me._Toolbar1_Button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Toolbar1_Button4.ToolTipText = "Notes"
        '
        '_Toolbar1_Button5
        '
        Me._Toolbar1_Button5.AutoSize = False
        Me._Toolbar1_Button5.ImageKey = "LETTER"
        Me._Toolbar1_Button5.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Toolbar1_Button5.Name = "_Toolbar1_Button5"
        Me._Toolbar1_Button5.Size = New System.Drawing.Size(24, 22)
        Me._Toolbar1_Button5.Tag = ""
        Me._Toolbar1_Button5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Toolbar1_Button5.ToolTipText = "Letter"
        '
        '_Toolbar1_Button3
        '
        Me._Toolbar1_Button3.AutoSize = False
        Me._Toolbar1_Button3.Name = "_Toolbar1_Button3"
        Me._Toolbar1_Button3.Size = New System.Drawing.Size(6, 22)
        Me._Toolbar1_Button3.Tag = ""
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Location = New System.Drawing.Point(8, 449)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApply.Size = New System.Drawing.Size(73, 22)
        Me.cmdApply.TabIndex = 61
        Me.cmdApply.Text = "&Apply"
        Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdApply.UseVisualStyleBackColor = False
        '
        'txtCurrency
        '
        Me.txtCurrency.AcceptsReturn = True
        Me.txtCurrency.BackColor = System.Drawing.SystemColors.Window
        Me.txtCurrency.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCurrency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCurrency.Location = New System.Drawing.Point(168, 454)
        Me.txtCurrency.MaxLength = 0
        Me.txtCurrency.Name = "txtCurrency"
        Me.txtCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCurrency.Size = New System.Drawing.Size(37, 20)
        Me.txtCurrency.TabIndex = 60
        Me.txtCurrency.Visible = False
        '
        'txtDate
        '
        Me.txtDate.AcceptsReturn = True
        Me.txtDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDate.Location = New System.Drawing.Point(112, 454)
        Me.txtDate.MaxLength = 0
        Me.txtDate.Name = "txtDate"
        Me.txtDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDate.Size = New System.Drawing.Size(37, 20)
        Me.txtDate.TabIndex = 59
        Me.txtDate.Visible = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(568, 449)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 43
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
        Me.cmdCancel.Location = New System.Drawing.Point(488, 449)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 42
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(408, 449)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 41
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
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
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(79, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(4, 30)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(645, 418)
        Me.tabMainTab.TabIndex = 44
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.Frame1)
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdNext_0)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 40)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(637, 374)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - General"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.Label3)
        Me.Frame1.Controls.Add(Me.cboTPASettle)
        Me.Frame1.Controls.Add(Me.txtReference)
        Me.Frame1.Controls.Add(Me.txtDatePassedTest)
        Me.Frame1.Controls.Add(Me.uctBranch)
        Me.Frame1.Controls.Add(Me.cboSubBranch)
        Me.Frame1.Controls.Add(Me.fraSupplier)
        Me.Frame1.Controls.Add(Me.txtRegNumber)
        Me.Frame1.Controls.Add(Me.cboStatus)
        Me.Frame1.Controls.Add(Me.txtLicenceNumber)
        Me.Frame1.Controls.Add(Me.cboLicenceType)
        Me.Frame1.Controls.Add(Me.txtDateOfBirth)
        Me.Frame1.Controls.Add(Me.txtPartyCode)
        Me.Frame1.Controls.Add(Me.txtPartyName)
        Me.Frame1.Controls.Add(Me.ddGender)
        Me.Frame1.Controls.Add(Me.uctCurrency)
        Me.Frame1.Controls.Add(Me.txtCompanyNotes)
        Me.Frame1.Controls.Add(Me.lblCompanyNotes)
        Me.Frame1.Controls.Add(Me.lblReference)
        Me.Frame1.Controls.Add(Me.lblDatePassedTest)
        Me.Frame1.Controls.Add(Me.lblCurrency)
        Me.Frame1.Controls.Add(Me.lblSubBranch)
        Me.Frame1.Controls.Add(Me.lblBranch)
        Me.Frame1.Controls.Add(Me.lblRegNumber)
        Me.Frame1.Controls.Add(Me.lblStatus)
        Me.Frame1.Controls.Add(Me.lblLicenceNumber)
        Me.Frame1.Controls.Add(Me.lblLicenceType)
        Me.Frame1.Controls.Add(Me.lblGender)
        Me.Frame1.Controls.Add(Me.lblDateOfBirth)
        Me.Frame1.Controls.Add(Me.lblPartyCode)
        Me.Frame1.Controls.Add(Me.lblPartyName)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(8, 4)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(575, 351)
        Me.Frame1.TabIndex = 45
        Me.Frame1.TabStop = False
        '
        'txtReference
        '
        Me.txtReference.AcceptsReturn = True
        Me.txtReference.BackColor = System.Drawing.SystemColors.Window
        Me.txtReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtReference.Location = New System.Drawing.Point(136, 72)
        Me.txtReference.MaxLength = 20
        Me.txtReference.Name = "txtReference"
        Me.txtReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtReference.Size = New System.Drawing.Size(229, 20)
        Me.txtReference.TabIndex = 3
        '
        'txtDatePassedTest
        '
        Me.txtDatePassedTest.AcceptsReturn = True
        Me.txtDatePassedTest.BackColor = System.Drawing.SystemColors.Window
        Me.txtDatePassedTest.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDatePassedTest.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDatePassedTest.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDatePassedTest.Location = New System.Drawing.Point(136, 272)
        Me.txtDatePassedTest.MaxLength = 0
        Me.txtDatePassedTest.Name = "txtDatePassedTest"
        Me.txtDatePassedTest.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDatePassedTest.Size = New System.Drawing.Size(181, 20)
        Me.txtDatePassedTest.TabIndex = 10
        '
        'uctBranch
        '
        Me.uctBranch.BackColor = System.Drawing.SystemColors.Window
        Me.uctBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.uctBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.uctBranch.Location = New System.Drawing.Point(412, 144)
        Me.uctBranch.Name = "uctBranch"
        Me.uctBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.uctBranch.Size = New System.Drawing.Size(145, 21)
        Me.uctBranch.TabIndex = 74
        Me.uctBranch.Text = " "
        '
        'cboSubBranch
        '
        Me.cboSubBranch.BackColor = System.Drawing.SystemColors.Window
        Me.cboSubBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSubBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSubBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSubBranch.Location = New System.Drawing.Point(412, 176)
        Me.cboSubBranch.Name = "cboSubBranch"
        Me.cboSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSubBranch.Size = New System.Drawing.Size(145, 21)
        Me.cboSubBranch.TabIndex = 73
        '
        'fraSupplier
        '
        Me.fraSupplier.BackColor = System.Drawing.SystemColors.Control
        Me.fraSupplier.Controls.Add(Me.cboPriority)
        Me.fraSupplier.Controls.Add(Me.cboAfterHours)
        Me.fraSupplier.Controls.Add(Me.cboActive)
        Me.fraSupplier.Controls.Add(Me.lblPriority)
        Me.fraSupplier.Controls.Add(Me.lblAfterHours)
        Me.fraSupplier.Controls.Add(Me.lblActive)
        Me.fraSupplier.Cursor = System.Windows.Forms.Cursors.Default
        Me.fraSupplier.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraSupplier.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraSupplier.Location = New System.Drawing.Point(8, 288)
        Me.fraSupplier.Name = "fraSupplier"
        Me.fraSupplier.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraSupplier.Size = New System.Drawing.Size(537, 49)
        Me.fraSupplier.TabIndex = 62
        '
        'cboPriority
        '
        Me.cboPriority.BackColor = System.Drawing.SystemColors.Window
        Me.cboPriority.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPriority.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPriority.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPriority.Location = New System.Drawing.Point(416, 16)
        Me.cboPriority.Name = "cboPriority"
        Me.cboPriority.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPriority.Size = New System.Drawing.Size(57, 21)
        Me.cboPriority.TabIndex = 68
        '
        'cboAfterHours
        '
        Me.cboAfterHours.BackColor = System.Drawing.SystemColors.Window
        Me.cboAfterHours.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboAfterHours.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAfterHours.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboAfterHours.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboAfterHours.Location = New System.Drawing.Point(256, 16)
        Me.cboAfterHours.Name = "cboAfterHours"
        Me.cboAfterHours.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboAfterHours.Size = New System.Drawing.Size(57, 21)
        Me.cboAfterHours.TabIndex = 67
        '
        'cboActive
        '
        Me.cboActive.BackColor = System.Drawing.SystemColors.Window
        Me.cboActive.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboActive.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboActive.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboActive.Location = New System.Drawing.Point(80, 16)
        Me.cboActive.Name = "cboActive"
        Me.cboActive.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboActive.Size = New System.Drawing.Size(57, 21)
        Me.cboActive.TabIndex = 66
        '
        'lblPriority
        '
        Me.lblPriority.BackColor = System.Drawing.SystemColors.Control
        Me.lblPriority.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPriority.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPriority.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPriority.Location = New System.Drawing.Point(352, 16)
        Me.lblPriority.Name = "lblPriority"
        Me.lblPriority.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPriority.Size = New System.Drawing.Size(53, 17)
        Me.lblPriority.TabIndex = 65
        Me.lblPriority.Text = "Priority:"
        '
        'lblAfterHours
        '
        Me.lblAfterHours.BackColor = System.Drawing.SystemColors.Control
        Me.lblAfterHours.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAfterHours.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAfterHours.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAfterHours.Location = New System.Drawing.Point(176, 16)
        Me.lblAfterHours.Name = "lblAfterHours"
        Me.lblAfterHours.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAfterHours.Size = New System.Drawing.Size(77, 17)
        Me.lblAfterHours.TabIndex = 64
        Me.lblAfterHours.Text = "After Hours:"
        '
        'lblActive
        '
        Me.lblActive.BackColor = System.Drawing.SystemColors.Control
        Me.lblActive.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblActive.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblActive.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblActive.Location = New System.Drawing.Point(0, 16)
        Me.lblActive.Name = "lblActive"
        Me.lblActive.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblActive.Size = New System.Drawing.Size(43, 17)
        Me.lblActive.TabIndex = 63
        Me.lblActive.Text = "Active:"
        '
        'txtRegNumber
        '
        Me.txtRegNumber.AcceptsReturn = True
        Me.txtRegNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtRegNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRegNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRegNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRegNumber.Location = New System.Drawing.Point(136, 240)
        Me.txtRegNumber.MaxLength = 0
        Me.txtRegNumber.Name = "txtRegNumber"
        Me.txtRegNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRegNumber.Size = New System.Drawing.Size(181, 20)
        Me.txtRegNumber.TabIndex = 9
        '
        'cboStatus
        '
        Me.cboStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cboStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboStatus.Location = New System.Drawing.Point(136, 208)
        Me.cboStatus.Name = "cboStatus"
        Me.cboStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboStatus.Size = New System.Drawing.Size(185, 21)
        Me.cboStatus.TabIndex = 8
        '
        'txtLicenceNumber
        '
        Me.txtLicenceNumber.AcceptsReturn = True
        Me.txtLicenceNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtLicenceNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLicenceNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLicenceNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLicenceNumber.Location = New System.Drawing.Point(136, 176)
        Me.txtLicenceNumber.MaxLength = 0
        Me.txtLicenceNumber.Name = "txtLicenceNumber"
        Me.txtLicenceNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLicenceNumber.Size = New System.Drawing.Size(183, 20)
        Me.txtLicenceNumber.TabIndex = 7
        '
        'cboLicenceType
        '
        Me.cboLicenceType.BackColor = System.Drawing.SystemColors.Window
        Me.cboLicenceType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboLicenceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLicenceType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboLicenceType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboLicenceType.Location = New System.Drawing.Point(136, 144)
        Me.cboLicenceType.Name = "cboLicenceType"
        Me.cboLicenceType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboLicenceType.Size = New System.Drawing.Size(185, 21)
        Me.cboLicenceType.TabIndex = 6
        '
        'txtDateOfBirth
        '
        Me.txtDateOfBirth.AcceptsReturn = True
        Me.txtDateOfBirth.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateOfBirth.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDateOfBirth.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDateOfBirth.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateOfBirth.Location = New System.Drawing.Point(136, 72)
        Me.txtDateOfBirth.MaxLength = 0
        Me.txtDateOfBirth.Name = "txtDateOfBirth"
        Me.txtDateOfBirth.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateOfBirth.Size = New System.Drawing.Size(181, 20)
        Me.txtDateOfBirth.TabIndex = 2
        '
        'txtPartyCode
        '
        Me.txtPartyCode.AcceptsReturn = True
        Me.txtPartyCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtPartyCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPartyCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPartyCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPartyCode.Location = New System.Drawing.Point(136, 16)
        Me.txtPartyCode.MaxLength = 20
        Me.txtPartyCode.Name = "txtPartyCode"
        Me.txtPartyCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPartyCode.Size = New System.Drawing.Size(229, 20)
        Me.txtPartyCode.TabIndex = 0
        '
        'txtPartyName
        '
        Me.txtPartyName.AcceptsReturn = True
        Me.txtPartyName.BackColor = System.Drawing.SystemColors.Window
        Me.txtPartyName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPartyName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPartyName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPartyName.Location = New System.Drawing.Point(136, 44)
        Me.txtPartyName.MaxLength = 60
        Me.txtPartyName.Name = "txtPartyName"
        Me.txtPartyName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPartyName.Size = New System.Drawing.Size(431, 20)
        Me.txtPartyName.TabIndex = 1
        '
        'ddGender
        '
        Me.ddGender.AllowAbiCodeEntry = False
        Me.ddGender.AutoCompleteText = False
        Me.ddGender.DataModel = "DEF"
        Me.ddGender.ListIndex = -1
        Me.ddGender.ListManager = Nothing
        Me.ddGender.Location = New System.Drawing.Point(136, 104)
        Me.ddGender.Login = False
        Me.ddGender.LongList = False
        Me.ddGender.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddGender.Name = "ddGender"
        Me.ddGender.PropertyId = "131091"
        Me.ddGender.ReadOnly_Renamed = False
        Me.ddGender.SelLength = 0
        Me.ddGender.SelStart = 0
        Me.ddGender.SelText = ""
        Me.ddGender.Size = New System.Drawing.Size(185, 21)
        Me.ddGender.TabIndex = 5
        Me.ddGender.ToolTipText = ""
        Me.ddGender.VehicleListId = ""
        Me.ddGender.VehicleMake = ""
        '
        'uctCurrency
        '
        Me.uctCurrency.CompanyId = 0
        Me.uctCurrency.CurrencyId = 0
        Me.uctCurrency.DefaultCurrencyId = 0
        Me.uctCurrency.FirstItem = ""
        Me.uctCurrency.ListIndex = -1
        Me.uctCurrency.Location = New System.Drawing.Point(412, 208)
        Me.uctCurrency.Name = "uctCurrency"
        Me.uctCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actCompanyCurrencies
        Me.uctCurrency.Size = New System.Drawing.Size(145, 21)
        Me.uctCurrency.TabIndex = 72
        Me.uctCurrency.ToolTipText = ""
        Me.uctCurrency.WhatsThisHelpID = 0
        '
        'txtCompanyNotes
        '
        Me.txtCompanyNotes.AcceptsReturn = True
        Me.txtCompanyNotes.BackColor = System.Drawing.SystemColors.Window
        Me.txtCompanyNotes.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCompanyNotes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCompanyNotes.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCompanyNotes.Location = New System.Drawing.Point(136, 104)
        Me.txtCompanyNotes.MaxLength = 2000
        Me.txtCompanyNotes.Multiline = True
        Me.txtCompanyNotes.Name = "txtCompanyNotes"
        Me.txtCompanyNotes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCompanyNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtCompanyNotes.Size = New System.Drawing.Size(421, 93)
        Me.txtCompanyNotes.TabIndex = 4
        '
        'lblCompanyNotes
        '
        Me.lblCompanyNotes.BackColor = System.Drawing.SystemColors.Control
        Me.lblCompanyNotes.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCompanyNotes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCompanyNotes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCompanyNotes.Location = New System.Drawing.Point(8, 107)
        Me.lblCompanyNotes.Name = "lblCompanyNotes"
        Me.lblCompanyNotes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCompanyNotes.Size = New System.Drawing.Size(83, 17)
        Me.lblCompanyNotes.TabIndex = 99
        Me.lblCompanyNotes.Text = "Notes:"
        '
        'lblReference
        '
        Me.lblReference.BackColor = System.Drawing.SystemColors.Control
        Me.lblReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReference.Location = New System.Drawing.Point(8, 75)
        Me.lblReference.Name = "lblReference"
        Me.lblReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReference.Size = New System.Drawing.Size(83, 17)
        Me.lblReference.TabIndex = 82
        Me.lblReference.Text = "Reference:"
        '
        'lblDatePassedTest
        '
        Me.lblDatePassedTest.BackColor = System.Drawing.SystemColors.Control
        Me.lblDatePassedTest.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDatePassedTest.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDatePassedTest.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDatePassedTest.Location = New System.Drawing.Point(8, 272)
        Me.lblDatePassedTest.Name = "lblDatePassedTest"
        Me.lblDatePassedTest.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDatePassedTest.Size = New System.Drawing.Size(123, 17)
        Me.lblDatePassedTest.TabIndex = 78
        Me.lblDatePassedTest.Text = "Date Passed Test:"
        '
        'lblCurrency
        '
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(334, 208)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(61, 13)
        Me.lblCurrency.TabIndex = 71
        Me.lblCurrency.Text = "Currency:"
        '
        'lblSubBranch
        '
        Me.lblSubBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblSubBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSubBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubBranch.Location = New System.Drawing.Point(334, 180)
        Me.lblSubBranch.Name = "lblSubBranch"
        Me.lblSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSubBranch.Size = New System.Drawing.Size(77, 13)
        Me.lblSubBranch.TabIndex = 70
        Me.lblSubBranch.Text = "Sub branch:"
        '
        'lblBranch
        '
        Me.lblBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranch.Location = New System.Drawing.Point(334, 148)
        Me.lblBranch.Name = "lblBranch"
        Me.lblBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranch.Size = New System.Drawing.Size(61, 13)
        Me.lblBranch.TabIndex = 69
        Me.lblBranch.Text = "Branch:"
        '
        'lblRegNumber
        '
        Me.lblRegNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblRegNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRegNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRegNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRegNumber.Location = New System.Drawing.Point(8, 240)
        Me.lblRegNumber.Name = "lblRegNumber"
        Me.lblRegNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRegNumber.Size = New System.Drawing.Size(123, 17)
        Me.lblRegNumber.TabIndex = 58
        Me.lblRegNumber.Text = "Registration number:"
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatus.Location = New System.Drawing.Point(8, 208)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatus.Size = New System.Drawing.Size(83, 17)
        Me.lblStatus.TabIndex = 57
        Me.lblStatus.Text = "Status:"
        '
        'lblLicenceNumber
        '
        Me.lblLicenceNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblLicenceNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLicenceNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLicenceNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLicenceNumber.Location = New System.Drawing.Point(8, 179)
        Me.lblLicenceNumber.Name = "lblLicenceNumber"
        Me.lblLicenceNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLicenceNumber.Size = New System.Drawing.Size(115, 17)
        Me.lblLicenceNumber.TabIndex = 56
        Me.lblLicenceNumber.Text = "Licence number:"
        '
        'lblLicenceType
        '
        Me.lblLicenceType.BackColor = System.Drawing.SystemColors.Control
        Me.lblLicenceType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLicenceType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLicenceType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLicenceType.Location = New System.Drawing.Point(8, 147)
        Me.lblLicenceType.Name = "lblLicenceType"
        Me.lblLicenceType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLicenceType.Size = New System.Drawing.Size(83, 17)
        Me.lblLicenceType.TabIndex = 55
        Me.lblLicenceType.Text = "Licence type:"
        '
        'lblGender
        '
        Me.lblGender.BackColor = System.Drawing.SystemColors.Control
        Me.lblGender.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGender.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGender.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGender.Location = New System.Drawing.Point(8, 107)
        Me.lblGender.Name = "lblGender"
        Me.lblGender.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGender.Size = New System.Drawing.Size(83, 17)
        Me.lblGender.TabIndex = 50
        Me.lblGender.Text = "Gender:"
        '
        'lblDateOfBirth
        '
        Me.lblDateOfBirth.BackColor = System.Drawing.SystemColors.Control
        Me.lblDateOfBirth.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDateOfBirth.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateOfBirth.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDateOfBirth.Location = New System.Drawing.Point(8, 75)
        Me.lblDateOfBirth.Name = "lblDateOfBirth"
        Me.lblDateOfBirth.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDateOfBirth.Size = New System.Drawing.Size(83, 17)
        Me.lblDateOfBirth.TabIndex = 49
        Me.lblDateOfBirth.Text = "Date of birth:"
        '
        'lblPartyCode
        '
        Me.lblPartyCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblPartyCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPartyCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartyCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPartyCode.Location = New System.Drawing.Point(8, 19)
        Me.lblPartyCode.Name = "lblPartyCode"
        Me.lblPartyCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPartyCode.Size = New System.Drawing.Size(75, 17)
        Me.lblPartyCode.TabIndex = 48
        Me.lblPartyCode.Text = "Party Code:"
        '
        'lblPartyName
        '
        Me.lblPartyName.AutoSize = True
        Me.lblPartyName.BackColor = System.Drawing.SystemColors.Control
        Me.lblPartyName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPartyName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartyName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPartyName.Location = New System.Drawing.Point(8, 47)
        Me.lblPartyName.Name = "lblPartyName"
        Me.lblPartyName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPartyName.Size = New System.Drawing.Size(38, 13)
        Me.lblPartyName.TabIndex = 47
        Me.lblPartyName.Text = "Name:"
        '
        '_cmdNext_0
        '
        Me._cmdNext_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_0.Location = New System.Drawing.Point(560, 364)
        Me._cmdNext_0.Name = "_cmdNext_0"
        Me._cmdNext_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_0.Size = New System.Drawing.Size(30, 19)
        Me._cmdNext_0.TabIndex = 11
        Me._cmdNext_0.Text = "&>>"
        Me._cmdNext_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_0.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraAddress)
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdPrevious_0)
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdNext_1)
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraContactDetails)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(637, 392)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "2 - Address"
        Me._tabMainTab_TabPage1.UseVisualStyleBackColor = True
        '
        'fraAddress
        '
        Me.fraAddress.BackColor = System.Drawing.SystemColors.Control
        Me.fraAddress.Controls.Add(Me.lvwAddresses)
        Me.fraAddress.Controls.Add(Me.cmdEditAddress)
        Me.fraAddress.Controls.Add(Me.cmdDeleteAddress)
        Me.fraAddress.Controls.Add(Me.cmdAddAddress)
        Me.fraAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAddress.Location = New System.Drawing.Point(8, 4)
        Me.fraAddress.Name = "fraAddress"
        Me.fraAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAddress.Size = New System.Drawing.Size(575, 291)
        Me.fraAddress.TabIndex = 46
        Me.fraAddress.TabStop = False
        '
        'lvwAddresses
        '
        Me.lvwAddresses.BackColor = System.Drawing.SystemColors.Window
        Me.lvwAddresses.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwAddresses.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwAddresses_ColumnHeader_1, Me._lvwAddresses_ColumnHeader_2, Me._lvwAddresses_ColumnHeader_3, Me._lvwAddresses_ColumnHeader_4, Me._lvwAddresses_ColumnHeader_5, Me._lvwAddresses_ColumnHeader_6})
        Me.lvwAddresses.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwAddresses.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwAddresses.FullRowSelect = True
        Me.lvwAddresses.LargeImageList = Me.ImageList2
        Me.lvwAddresses.Location = New System.Drawing.Point(8, 16)
        Me.lvwAddresses.Name = "lvwAddresses"
        Me.lvwAddresses.Size = New System.Drawing.Size(557, 235)
        Me.lvwAddresses.SmallImageList = Me.ImageList2
        Me.lvwAddresses.TabIndex = 12
        Me.lvwAddresses.UseCompatibleStateImageBehavior = False
        Me.lvwAddresses.View = System.Windows.Forms.View.Details
        '
        '_lvwAddresses_ColumnHeader_1
        '
        Me._lvwAddresses_ColumnHeader_1.Tag = ""
        Me._lvwAddresses_ColumnHeader_1.Text = ""
        Me._lvwAddresses_ColumnHeader_1.Width = 97
        '
        '_lvwAddresses_ColumnHeader_2
        '
        Me._lvwAddresses_ColumnHeader_2.Tag = ""
        Me._lvwAddresses_ColumnHeader_2.Text = ""
        Me._lvwAddresses_ColumnHeader_2.Width = 97
        '
        '_lvwAddresses_ColumnHeader_3
        '
        Me._lvwAddresses_ColumnHeader_3.Tag = ""
        Me._lvwAddresses_ColumnHeader_3.Text = ""
        Me._lvwAddresses_ColumnHeader_3.Width = 97
        '
        '_lvwAddresses_ColumnHeader_4
        '
        Me._lvwAddresses_ColumnHeader_4.Tag = ""
        Me._lvwAddresses_ColumnHeader_4.Text = ""
        Me._lvwAddresses_ColumnHeader_4.Width = 97
        '
        '_lvwAddresses_ColumnHeader_5
        '
        Me._lvwAddresses_ColumnHeader_5.Tag = ""
        Me._lvwAddresses_ColumnHeader_5.Text = ""
        Me._lvwAddresses_ColumnHeader_5.Width = 97
        '
        '_lvwAddresses_ColumnHeader_6
        '
        Me._lvwAddresses_ColumnHeader_6.Tag = ""
        Me._lvwAddresses_ColumnHeader_6.Text = ""
        Me._lvwAddresses_ColumnHeader_6.Width = 97
        '
        'cmdEditAddress
        '
        Me.cmdEditAddress.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditAddress.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditAddress.Enabled = False
        Me.cmdEditAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditAddress.Location = New System.Drawing.Point(168, 259)
        Me.cmdEditAddress.Name = "cmdEditAddress"
        Me.cmdEditAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditAddress.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditAddress.TabIndex = 15
        Me.cmdEditAddress.Text = "&Edit"
        Me.cmdEditAddress.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditAddress.UseVisualStyleBackColor = False
        '
        'cmdDeleteAddress
        '
        Me.cmdDeleteAddress.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteAddress.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteAddress.Enabled = False
        Me.cmdDeleteAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteAddress.Location = New System.Drawing.Point(88, 259)
        Me.cmdDeleteAddress.Name = "cmdDeleteAddress"
        Me.cmdDeleteAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteAddress.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteAddress.TabIndex = 14
        Me.cmdDeleteAddress.Text = "&Delete"
        Me.cmdDeleteAddress.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteAddress.UseVisualStyleBackColor = False
        '
        'cmdAddAddress
        '
        Me.cmdAddAddress.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddAddress.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddAddress.Location = New System.Drawing.Point(8, 259)
        Me.cmdAddAddress.Name = "cmdAddAddress"
        Me.cmdAddAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddAddress.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddAddress.TabIndex = 13
        Me.cmdAddAddress.Text = "&Add"
        Me.cmdAddAddress.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddAddress.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_0
        '
        Me._cmdPrevious_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_0.Location = New System.Drawing.Point(8, 364)
        Me._cmdPrevious_0.Name = "_cmdPrevious_0"
        Me._cmdPrevious_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_0.Size = New System.Drawing.Size(30, 19)
        Me._cmdPrevious_0.TabIndex = 18
        Me._cmdPrevious_0.Text = "&<<"
        Me._cmdPrevious_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_0.UseVisualStyleBackColor = False
        '
        '_cmdNext_1
        '
        Me._cmdNext_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_1.Location = New System.Drawing.Point(560, 364)
        Me._cmdNext_1.Name = "_cmdNext_1"
        Me._cmdNext_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_1.Size = New System.Drawing.Size(30, 19)
        Me._cmdNext_1.TabIndex = 19
        Me._cmdNext_1.Text = "&>>"
        Me._cmdNext_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_1.UseVisualStyleBackColor = False
        '
        'fraContactDetails
        '
        Me.fraContactDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraContactDetails.Controls.Add(Me.txtContactTelephoneNo)
        Me.fraContactDetails.Controls.Add(Me.txtContactName)
        Me.fraContactDetails.Controls.Add(Me.Label1)
        Me.fraContactDetails.Controls.Add(Me.lblContactName)
        Me.fraContactDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraContactDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraContactDetails.Location = New System.Drawing.Point(8, 300)
        Me.fraContactDetails.Name = "fraContactDetails"
        Me.fraContactDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraContactDetails.Size = New System.Drawing.Size(577, 57)
        Me.fraContactDetails.TabIndex = 79
        Me.fraContactDetails.TabStop = False
        Me.fraContactDetails.Text = "Contact Details"
        '
        'txtContactTelephoneNo
        '
        Me.txtContactTelephoneNo.AcceptsReturn = True
        Me.txtContactTelephoneNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtContactTelephoneNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtContactTelephoneNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtContactTelephoneNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtContactTelephoneNo.Location = New System.Drawing.Point(424, 24)
        Me.txtContactTelephoneNo.MaxLength = 60
        Me.txtContactTelephoneNo.Name = "txtContactTelephoneNo"
        Me.txtContactTelephoneNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtContactTelephoneNo.Size = New System.Drawing.Size(141, 20)
        Me.txtContactTelephoneNo.TabIndex = 17
        '
        'txtContactName
        '
        Me.txtContactName.AcceptsReturn = True
        Me.txtContactName.BackColor = System.Drawing.SystemColors.Window
        Me.txtContactName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtContactName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtContactName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtContactName.Location = New System.Drawing.Point(112, 24)
        Me.txtContactName.MaxLength = 255
        Me.txtContactName.Name = "txtContactName"
        Me.txtContactName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtContactName.Size = New System.Drawing.Size(181, 20)
        Me.txtContactName.TabIndex = 16
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(304, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(123, 17)
        Me.Label1.TabIndex = 81
        Me.Label1.Text = "Telephone Number:"
        '
        'lblContactName
        '
        Me.lblContactName.BackColor = System.Drawing.SystemColors.Control
        Me.lblContactName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblContactName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblContactName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblContactName.Location = New System.Drawing.Point(16, 24)
        Me.lblContactName.Name = "lblContactName"
        Me.lblContactName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblContactName.Size = New System.Drawing.Size(91, 17)
        Me.lblContactName.TabIndex = 80
        Me.lblContactName.Text = "Contact Name:"
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdNext_2)
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdPrevious_1)
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraSupply)
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraSpeciality)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(637, 392)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "3 - Supply"
        Me._tabMainTab_TabPage2.UseVisualStyleBackColor = True
        '
        '_cmdNext_2
        '
        Me._cmdNext_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_2.Location = New System.Drawing.Point(560, 364)
        Me._cmdNext_2.Name = "_cmdNext_2"
        Me._cmdNext_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_2.Size = New System.Drawing.Size(30, 19)
        Me._cmdNext_2.TabIndex = 29
        Me._cmdNext_2.Text = "&>>"
        Me._cmdNext_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_2.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_1
        '
        Me._cmdPrevious_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_1.Location = New System.Drawing.Point(8, 364)
        Me._cmdPrevious_1.Name = "_cmdPrevious_1"
        Me._cmdPrevious_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_1.Size = New System.Drawing.Size(30, 19)
        Me._cmdPrevious_1.TabIndex = 28
        Me._cmdPrevious_1.Text = "&<<"
        Me._cmdPrevious_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_1.UseVisualStyleBackColor = False
        '
        'fraSupply
        '
        Me.fraSupply.BackColor = System.Drawing.SystemColors.Control
        Me.fraSupply.Controls.Add(Me.cmdBusinessAdd)
        Me.fraSupply.Controls.Add(Me.cmdBusinessRemove)
        Me.fraSupply.Controls.Add(Me.lvwSupBusAvailable)
        Me.fraSupply.Controls.Add(Me.lvwSupBusSelected)
        Me.fraSupply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraSupply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraSupply.Location = New System.Drawing.Point(8, 4)
        Me.fraSupply.Name = "fraSupply"
        Me.fraSupply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraSupply.Size = New System.Drawing.Size(575, 173)
        Me.fraSupply.TabIndex = 53
        Me.fraSupply.TabStop = False
        Me.fraSupply.Text = "Supply"
        '
        'cmdBusinessAdd
        '
        Me.cmdBusinessAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBusinessAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBusinessAdd.Enabled = False
        Me.cmdBusinessAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBusinessAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBusinessAdd.Location = New System.Drawing.Point(274, 56)
        Me.cmdBusinessAdd.Name = "cmdBusinessAdd"
        Me.cmdBusinessAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBusinessAdd.Size = New System.Drawing.Size(29, 21)
        Me.cmdBusinessAdd.TabIndex = 22
        Me.cmdBusinessAdd.Text = "->"
        Me.cmdBusinessAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdBusinessAdd.UseVisualStyleBackColor = False
        '
        'cmdBusinessRemove
        '
        Me.cmdBusinessRemove.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBusinessRemove.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBusinessRemove.Enabled = False
        Me.cmdBusinessRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBusinessRemove.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBusinessRemove.Location = New System.Drawing.Point(274, 96)
        Me.cmdBusinessRemove.Name = "cmdBusinessRemove"
        Me.cmdBusinessRemove.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBusinessRemove.Size = New System.Drawing.Size(29, 21)
        Me.cmdBusinessRemove.TabIndex = 23
        Me.cmdBusinessRemove.Text = "<-"
        Me.cmdBusinessRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdBusinessRemove.UseVisualStyleBackColor = False
        '
        'lvwSupBusAvailable
        '
        Me.lvwSupBusAvailable.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSupBusAvailable.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwSupBusAvailable.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSupBusAvailable_ColumnHeader_1})
        Me.lvwSupBusAvailable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSupBusAvailable.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSupBusAvailable.Location = New System.Drawing.Point(10, 16)
        Me.lvwSupBusAvailable.Name = "lvwSupBusAvailable"
        Me.lvwSupBusAvailable.Size = New System.Drawing.Size(261, 149)
        Me.lvwSupBusAvailable.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwSupBusAvailable.TabIndex = 20
        Me.lvwSupBusAvailable.UseCompatibleStateImageBehavior = False
        Me.lvwSupBusAvailable.View = System.Windows.Forms.View.Details
        '
        '_lvwSupBusAvailable_ColumnHeader_1
        '
        Me._lvwSupBusAvailable_ColumnHeader_1.Tag = ""
        Me._lvwSupBusAvailable_ColumnHeader_1.Text = "Available"
        Me._lvwSupBusAvailable_ColumnHeader_1.Width = 221
        '
        'lvwSupBusSelected
        '
        Me.lvwSupBusSelected.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSupBusSelected.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSupBusSelected_ColumnHeader_1})
        Me.lvwSupBusSelected.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSupBusSelected.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSupBusSelected.HideSelection = False
        Me.lvwSupBusSelected.Location = New System.Drawing.Point(304, 16)
        Me.lvwSupBusSelected.Name = "lvwSupBusSelected"
        Me.lvwSupBusSelected.Size = New System.Drawing.Size(261, 149)
        Me.lvwSupBusSelected.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwSupBusSelected.TabIndex = 21
        Me.lvwSupBusSelected.UseCompatibleStateImageBehavior = False
        Me.lvwSupBusSelected.View = System.Windows.Forms.View.Details
        '
        '_lvwSupBusSelected_ColumnHeader_1
        '
        Me._lvwSupBusSelected_ColumnHeader_1.Tag = ""
        Me._lvwSupBusSelected_ColumnHeader_1.Text = "Selected"
        Me._lvwSupBusSelected_ColumnHeader_1.Width = 221
        '
        'fraSpeciality
        '
        Me.fraSpeciality.BackColor = System.Drawing.SystemColors.Control
        Me.fraSpeciality.Controls.Add(Me.cmdSpecialityRemove)
        Me.fraSpeciality.Controls.Add(Me.cmdSpecialityAdd)
        Me.fraSpeciality.Controls.Add(Me.lvwSupSpecAvailable)
        Me.fraSpeciality.Controls.Add(Me.lvwSupSpecSelected)
        Me.fraSpeciality.Enabled = False
        Me.fraSpeciality.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraSpeciality.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraSpeciality.Location = New System.Drawing.Point(8, 180)
        Me.fraSpeciality.Name = "fraSpeciality"
        Me.fraSpeciality.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraSpeciality.Size = New System.Drawing.Size(575, 169)
        Me.fraSpeciality.TabIndex = 54
        Me.fraSpeciality.TabStop = False
        Me.fraSpeciality.Text = "Speciality"
        '
        'cmdSpecialityRemove
        '
        Me.cmdSpecialityRemove.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSpecialityRemove.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSpecialityRemove.Enabled = False
        Me.cmdSpecialityRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSpecialityRemove.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSpecialityRemove.Location = New System.Drawing.Point(274, 96)
        Me.cmdSpecialityRemove.Name = "cmdSpecialityRemove"
        Me.cmdSpecialityRemove.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSpecialityRemove.Size = New System.Drawing.Size(29, 21)
        Me.cmdSpecialityRemove.TabIndex = 27
        Me.cmdSpecialityRemove.Text = "<-"
        Me.cmdSpecialityRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSpecialityRemove.UseVisualStyleBackColor = False
        '
        'cmdSpecialityAdd
        '
        Me.cmdSpecialityAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSpecialityAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSpecialityAdd.Enabled = False
        Me.cmdSpecialityAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSpecialityAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSpecialityAdd.Location = New System.Drawing.Point(274, 56)
        Me.cmdSpecialityAdd.Name = "cmdSpecialityAdd"
        Me.cmdSpecialityAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSpecialityAdd.Size = New System.Drawing.Size(29, 21)
        Me.cmdSpecialityAdd.TabIndex = 26
        Me.cmdSpecialityAdd.Text = "->"
        Me.cmdSpecialityAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSpecialityAdd.UseVisualStyleBackColor = False
        '
        'lvwSupSpecAvailable
        '
        Me.lvwSupSpecAvailable.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSupSpecAvailable.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSupSpecAvailable_ColumnHeader_1})
        Me.lvwSupSpecAvailable.Enabled = False
        Me.lvwSupSpecAvailable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSupSpecAvailable.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSupSpecAvailable.Location = New System.Drawing.Point(10, 16)
        Me.lvwSupSpecAvailable.Name = "lvwSupSpecAvailable"
        Me.lvwSupSpecAvailable.Size = New System.Drawing.Size(261, 145)
        Me.lvwSupSpecAvailable.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwSupSpecAvailable.TabIndex = 24
        Me.lvwSupSpecAvailable.UseCompatibleStateImageBehavior = False
        Me.lvwSupSpecAvailable.View = System.Windows.Forms.View.Details
        '
        '_lvwSupSpecAvailable_ColumnHeader_1
        '
        Me._lvwSupSpecAvailable_ColumnHeader_1.Tag = ""
        Me._lvwSupSpecAvailable_ColumnHeader_1.Text = "Available"
        Me._lvwSupSpecAvailable_ColumnHeader_1.Width = 221
        '
        'lvwSupSpecSelected
        '
        Me.lvwSupSpecSelected.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSupSpecSelected.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSupSpecSelected_ColumnHeader_1})
        Me.lvwSupSpecSelected.Enabled = False
        Me.lvwSupSpecSelected.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSupSpecSelected.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSupSpecSelected.Location = New System.Drawing.Point(304, 16)
        Me.lvwSupSpecSelected.Name = "lvwSupSpecSelected"
        Me.lvwSupSpecSelected.Size = New System.Drawing.Size(261, 145)
        Me.lvwSupSpecSelected.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwSupSpecSelected.TabIndex = 25
        Me.lvwSupSpecSelected.UseCompatibleStateImageBehavior = False
        Me.lvwSupSpecSelected.View = System.Windows.Forms.View.Details
        '
        '_lvwSupSpecSelected_ColumnHeader_1
        '
        Me._lvwSupSpecSelected_ColumnHeader_1.Tag = ""
        Me._lvwSupSpecSelected_ColumnHeader_1.Text = "Selected"
        Me._lvwSupSpecSelected_ColumnHeader_1.Width = 221
        '
        '_tabMainTab_TabPage3
        '
        Me._tabMainTab_TabPage3.Controls.Add(Me._cmdPrevious_2)
        Me._tabMainTab_TabPage3.Controls.Add(Me.Frame2)
        Me._tabMainTab_TabPage3.Controls.Add(Me._cmdNext_3)
        Me._tabMainTab_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage3.Name = "_tabMainTab_TabPage3"
        Me._tabMainTab_TabPage3.Size = New System.Drawing.Size(637, 392)
        Me._tabMainTab_TabPage3.TabIndex = 3
        Me._tabMainTab_TabPage3.Text = "4 - Convictions"
        Me._tabMainTab_TabPage3.UseVisualStyleBackColor = True
        '
        '_cmdPrevious_2
        '
        Me._cmdPrevious_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_2.Location = New System.Drawing.Point(8, 364)
        Me._cmdPrevious_2.Name = "_cmdPrevious_2"
        Me._cmdPrevious_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_2.Size = New System.Drawing.Size(30, 19)
        Me._cmdPrevious_2.TabIndex = 34
        Me._cmdPrevious_2.Text = "&<<"
        Me._cmdPrevious_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_2.UseVisualStyleBackColor = False
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.cmdEditConviction)
        Me.Frame2.Controls.Add(Me.cmdDeleteConviction)
        Me.Frame2.Controls.Add(Me.cmdAddConviction)
        Me.Frame2.Controls.Add(Me.lvwConvictions)
        Me.Frame2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(8, 4)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(575, 347)
        Me.Frame2.TabIndex = 52
        Me.Frame2.TabStop = False
        '
        'cmdEditConviction
        '
        Me.cmdEditConviction.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditConviction.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditConviction.Enabled = False
        Me.cmdEditConviction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditConviction.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditConviction.Location = New System.Drawing.Point(168, 315)
        Me.cmdEditConviction.Name = "cmdEditConviction"
        Me.cmdEditConviction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditConviction.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditConviction.TabIndex = 33
        Me.cmdEditConviction.Text = "&Edit"
        Me.cmdEditConviction.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditConviction.UseVisualStyleBackColor = False
        '
        'cmdDeleteConviction
        '
        Me.cmdDeleteConviction.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteConviction.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteConviction.Enabled = False
        Me.cmdDeleteConviction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteConviction.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteConviction.Location = New System.Drawing.Point(88, 315)
        Me.cmdDeleteConviction.Name = "cmdDeleteConviction"
        Me.cmdDeleteConviction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteConviction.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteConviction.TabIndex = 32
        Me.cmdDeleteConviction.Text = "&Delete"
        Me.cmdDeleteConviction.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteConviction.UseVisualStyleBackColor = False
        '
        'cmdAddConviction
        '
        Me.cmdAddConviction.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddConviction.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddConviction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddConviction.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddConviction.Location = New System.Drawing.Point(8, 315)
        Me.cmdAddConviction.Name = "cmdAddConviction"
        Me.cmdAddConviction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddConviction.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddConviction.TabIndex = 31
        Me.cmdAddConviction.Text = "&Add"
        Me.cmdAddConviction.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddConviction.UseVisualStyleBackColor = False
        '
        'lvwConvictions
        '
        Me.lvwConvictions.BackColor = System.Drawing.SystemColors.Window
        Me.lvwConvictions.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwConvictions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwConvictions_ColumnHeader_1, Me._lvwConvictions_ColumnHeader_2, Me._lvwConvictions_ColumnHeader_3, Me._lvwConvictions_ColumnHeader_4, Me._lvwConvictions_ColumnHeader_5, Me._lvwConvictions_ColumnHeader_6})
        Me.lvwConvictions.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwConvictions.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwConvictions.FullRowSelect = True
        Me.lvwConvictions.LargeImageList = Me.ImageList2
        Me.lvwConvictions.Location = New System.Drawing.Point(8, 16)
        Me.lvwConvictions.Name = "lvwConvictions"
        Me.lvwConvictions.Size = New System.Drawing.Size(557, 291)
        Me.lvwConvictions.SmallImageList = Me.ImageList2
        Me.lvwConvictions.TabIndex = 30
        Me.lvwConvictions.UseCompatibleStateImageBehavior = False
        Me.lvwConvictions.View = System.Windows.Forms.View.Details
        '
        '_lvwConvictions_ColumnHeader_1
        '
        Me._lvwConvictions_ColumnHeader_1.Tag = ""
        Me._lvwConvictions_ColumnHeader_1.Text = "Conviction Type"
        Me._lvwConvictions_ColumnHeader_1.Width = 97
        '
        '_lvwConvictions_ColumnHeader_2
        '
        Me._lvwConvictions_ColumnHeader_2.Tag = ""
        Me._lvwConvictions_ColumnHeader_2.Text = "Date"
        Me._lvwConvictions_ColumnHeader_2.Width = 67
        '
        '_lvwConvictions_ColumnHeader_3
        '
        Me._lvwConvictions_ColumnHeader_3.Tag = ""
        Me._lvwConvictions_ColumnHeader_3.Text = "Description"
        Me._lvwConvictions_ColumnHeader_3.Width = 97
        '
        '_lvwConvictions_ColumnHeader_4
        '
        Me._lvwConvictions_ColumnHeader_4.Tag = ""
        Me._lvwConvictions_ColumnHeader_4.Text = "Fine"
        Me._lvwConvictions_ColumnHeader_4.Width = 134
        '
        '_lvwConvictions_ColumnHeader_5
        '
        Me._lvwConvictions_ColumnHeader_5.Tag = ""
        Me._lvwConvictions_ColumnHeader_5.Text = "Conviction Status"
        Me._lvwConvictions_ColumnHeader_5.Width = 97
        '
        '_lvwConvictions_ColumnHeader_6
        '
        Me._lvwConvictions_ColumnHeader_6.Tag = ""
        Me._lvwConvictions_ColumnHeader_6.Text = "Penalty Points"
        Me._lvwConvictions_ColumnHeader_6.Width = 97
        '
        '_cmdNext_3
        '
        Me._cmdNext_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_3.Location = New System.Drawing.Point(560, 364)
        Me._cmdNext_3.Name = "_cmdNext_3"
        Me._cmdNext_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_3.Size = New System.Drawing.Size(30, 19)
        Me._cmdNext_3.TabIndex = 35
        Me._cmdNext_3.Text = "&>>"
        Me._cmdNext_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_3.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage4
        '
        Me._tabMainTab_TabPage4.Controls.Add(Me.fraAccident)
        Me._tabMainTab_TabPage4.Controls.Add(Me._cmdPrevious_3)
        Me._tabMainTab_TabPage4.Controls.Add(Me._cmdNext_4)
        Me._tabMainTab_TabPage4.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage4.Name = "_tabMainTab_TabPage4"
        Me._tabMainTab_TabPage4.Size = New System.Drawing.Size(637, 392)
        Me._tabMainTab_TabPage4.TabIndex = 4
        Me._tabMainTab_TabPage4.Text = "5 - Accidents"
        Me._tabMainTab_TabPage4.UseVisualStyleBackColor = True
        '
        'fraAccident
        '
        Me.fraAccident.BackColor = System.Drawing.SystemColors.Control
        Me.fraAccident.Controls.Add(Me.cmdEditAccident)
        Me.fraAccident.Controls.Add(Me.cmdDeleteAccident)
        Me.fraAccident.Controls.Add(Me.cmdAddAccident)
        Me.fraAccident.Controls.Add(Me.lvwAccidents)
        Me.fraAccident.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAccident.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAccident.Location = New System.Drawing.Point(8, 4)
        Me.fraAccident.Name = "fraAccident"
        Me.fraAccident.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAccident.Size = New System.Drawing.Size(575, 347)
        Me.fraAccident.TabIndex = 51
        Me.fraAccident.TabStop = False
        '
        'cmdEditAccident
        '
        Me.cmdEditAccident.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditAccident.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditAccident.Enabled = False
        Me.cmdEditAccident.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditAccident.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditAccident.Location = New System.Drawing.Point(168, 315)
        Me.cmdEditAccident.Name = "cmdEditAccident"
        Me.cmdEditAccident.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditAccident.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditAccident.TabIndex = 39
        Me.cmdEditAccident.Text = "&Edit"
        Me.cmdEditAccident.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditAccident.UseVisualStyleBackColor = False
        '
        'cmdDeleteAccident
        '
        Me.cmdDeleteAccident.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteAccident.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteAccident.Enabled = False
        Me.cmdDeleteAccident.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteAccident.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteAccident.Location = New System.Drawing.Point(88, 315)
        Me.cmdDeleteAccident.Name = "cmdDeleteAccident"
        Me.cmdDeleteAccident.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteAccident.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteAccident.TabIndex = 38
        Me.cmdDeleteAccident.Text = "&Delete"
        Me.cmdDeleteAccident.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteAccident.UseVisualStyleBackColor = False
        '
        'cmdAddAccident
        '
        Me.cmdAddAccident.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddAccident.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddAccident.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddAccident.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddAccident.Location = New System.Drawing.Point(8, 315)
        Me.cmdAddAccident.Name = "cmdAddAccident"
        Me.cmdAddAccident.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddAccident.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddAccident.TabIndex = 37
        Me.cmdAddAccident.Text = "&Add"
        Me.cmdAddAccident.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddAccident.UseVisualStyleBackColor = False
        '
        'lvwAccidents
        '
        Me.lvwAccidents.BackColor = System.Drawing.SystemColors.Window
        Me.lvwAccidents.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwAccidents.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwAccidents_ColumnHeader_1, Me._lvwAccidents_ColumnHeader_2, Me._lvwAccidents_ColumnHeader_3})
        Me.lvwAccidents.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwAccidents.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwAccidents.FullRowSelect = True
        Me.lvwAccidents.LargeImageList = Me.ImageList2
        Me.lvwAccidents.Location = New System.Drawing.Point(8, 16)
        Me.lvwAccidents.Name = "lvwAccidents"
        Me.lvwAccidents.Size = New System.Drawing.Size(557, 291)
        Me.lvwAccidents.SmallImageList = Me.ImageList2
        Me.lvwAccidents.TabIndex = 36
        Me.lvwAccidents.UseCompatibleStateImageBehavior = False
        Me.lvwAccidents.View = System.Windows.Forms.View.Details
        '
        '_lvwAccidents_ColumnHeader_1
        '
        Me._lvwAccidents_ColumnHeader_1.Tag = ""
        Me._lvwAccidents_ColumnHeader_1.Text = "Date"
        Me._lvwAccidents_ColumnHeader_1.Width = 193
        '
        '_lvwAccidents_ColumnHeader_2
        '
        Me._lvwAccidents_ColumnHeader_2.Tag = ""
        Me._lvwAccidents_ColumnHeader_2.Text = "Description"
        Me._lvwAccidents_ColumnHeader_2.Width = 193
        '
        '_lvwAccidents_ColumnHeader_3
        '
        Me._lvwAccidents_ColumnHeader_3.Tag = ""
        Me._lvwAccidents_ColumnHeader_3.Text = "At fault"
        Me._lvwAccidents_ColumnHeader_3.Width = 97
        '
        '_cmdPrevious_3
        '
        Me._cmdPrevious_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_3.Location = New System.Drawing.Point(8, 364)
        Me._cmdPrevious_3.Name = "_cmdPrevious_3"
        Me._cmdPrevious_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_3.Size = New System.Drawing.Size(30, 19)
        Me._cmdPrevious_3.TabIndex = 40
        Me._cmdPrevious_3.Text = "&<<"
        Me._cmdPrevious_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_3.UseVisualStyleBackColor = False
        '
        '_cmdNext_4
        '
        Me._cmdNext_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_4.Location = New System.Drawing.Point(560, 364)
        Me._cmdNext_4.Name = "_cmdNext_4"
        Me._cmdNext_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_4.Size = New System.Drawing.Size(30, 19)
        Me._cmdNext_4.TabIndex = 76
        Me._cmdNext_4.Text = "&>>"
        Me._cmdNext_4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_4.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage5
        '
        Me._tabMainTab_TabPage5.Controls.Add(Me.Label2)
        Me._tabMainTab_TabPage5.Controls.Add(Me._cmdNext_5)
        Me._tabMainTab_TabPage5.Controls.Add(Me._cmdPrevious_4)
        Me._tabMainTab_TabPage5.Controls.Add(Me.uctPartyTax1)
        Me._tabMainTab_TabPage5.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage5.Name = "_tabMainTab_TabPage5"
        Me._tabMainTab_TabPage5.Size = New System.Drawing.Size(637, 392)
        Me._tabMainTab_TabPage5.TabIndex = 5
        Me._tabMainTab_TabPage5.Text = "6 - Tax"
        Me._tabMainTab_TabPage5.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(33, 5)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 13)
        Me.Label2.TabIndex = 84
        Me.Label2.Text = "Party Tax"
        '
        '_cmdNext_5
        '
        Me._cmdNext_5.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_5.Location = New System.Drawing.Point(560, 364)
        Me._cmdNext_5.Name = "_cmdNext_5"
        Me._cmdNext_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_5.Size = New System.Drawing.Size(30, 19)
        Me._cmdNext_5.TabIndex = 83
        Me._cmdNext_5.Text = "&>>"
        Me._cmdNext_5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_5.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_4
        '
        Me._cmdPrevious_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_4.Location = New System.Drawing.Point(8, 364)
        Me._cmdPrevious_4.Name = "_cmdPrevious_4"
        Me._cmdPrevious_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_4.Size = New System.Drawing.Size(30, 19)
        Me._cmdPrevious_4.TabIndex = 77
        Me._cmdPrevious_4.Text = "&<<"
        Me._cmdPrevious_4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_4.UseVisualStyleBackColor = False
        '
        'uctPartyTax1
        '
        Me.uctPartyTax1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPartyTax1.FrameOn = True
        Me.uctPartyTax1.IsDomiciledForTax = False
        Me.uctPartyTax1.Location = New System.Drawing.Point(15, 11)
        Me.uctPartyTax1.Name = "uctPartyTax1"
        Me.uctPartyTax1.Size = New System.Drawing.Size(575, 241)
        Me.uctPartyTax1.SizeHorizontal = False
        Me.uctPartyTax1.TabIndex = 75
        Me.uctPartyTax1.TaxExempt = False
        Me.uctPartyTax1.TaxNumber = ""
        Me.uctPartyTax1.TaxPercentage = 0
        '
        '_tabMainTab_TabPage6
        '
        Me._tabMainTab_TabPage6.Controls.Add(Me._cmdPrevious_5)
        Me._tabMainTab_TabPage6.Controls.Add(Me.fraInsurerDetails)
        Me._tabMainTab_TabPage6.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage6.Name = "_tabMainTab_TabPage6"
        Me._tabMainTab_TabPage6.Size = New System.Drawing.Size(637, 392)
        Me._tabMainTab_TabPage6.TabIndex = 6
        Me._tabMainTab_TabPage6.Text = "Insurer"
        Me._tabMainTab_TabPage6.UseVisualStyleBackColor = True
        '
        '_cmdPrevious_5
        '
        Me._cmdPrevious_5.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_5.Location = New System.Drawing.Point(8, 364)
        Me._cmdPrevious_5.Name = "_cmdPrevious_5"
        Me._cmdPrevious_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_5.Size = New System.Drawing.Size(30, 19)
        Me._cmdPrevious_5.TabIndex = 84
        Me._cmdPrevious_5.Text = "&<<"
        Me._cmdPrevious_5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_5.UseVisualStyleBackColor = False
        '
        'fraInsurerDetails
        '
        Me.fraInsurerDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraInsurerDetails.Controls.Add(Me.txtInsurerNotes)
        Me.fraInsurerDetails.Controls.Add(Me.uctPMAddressControl1)
        Me.fraInsurerDetails.Controls.Add(Me.txtInsurerEmailAddress)
        Me.fraInsurerDetails.Controls.Add(Me.txtInsurerContactName)
        Me.fraInsurerDetails.Controls.Add(Me.txtInsurerFaxNo)
        Me.fraInsurerDetails.Controls.Add(Me.txtInsurerTelNo)
        Me.fraInsurerDetails.Controls.Add(Me.txtInsurerName)
        Me.fraInsurerDetails.Controls.Add(Me.lblInsurerNotes)
        Me.fraInsurerDetails.Controls.Add(Me.lblInsurerEmailAddress)
        Me.fraInsurerDetails.Controls.Add(Me.lblInsurerContactName)
        Me.fraInsurerDetails.Controls.Add(Me.lblInsurerFaxNo)
        Me.fraInsurerDetails.Controls.Add(Me.lblInsurerTelNo)
        Me.fraInsurerDetails.Controls.Add(Me.lblInsurerName)
        Me.fraInsurerDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraInsurerDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraInsurerDetails.Location = New System.Drawing.Point(8, 4)
        Me.fraInsurerDetails.Name = "fraInsurerDetails"
        Me.fraInsurerDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraInsurerDetails.Size = New System.Drawing.Size(575, 353)
        Me.fraInsurerDetails.TabIndex = 85
        Me.fraInsurerDetails.TabStop = False
        '
        'txtInsurerNotes
        '
        Me.txtInsurerNotes.AcceptsReturn = True
        Me.txtInsurerNotes.BackColor = System.Drawing.SystemColors.Window
        Me.txtInsurerNotes.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInsurerNotes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInsurerNotes.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInsurerNotes.Location = New System.Drawing.Point(152, 296)
        Me.txtInsurerNotes.MaxLength = 2000
        Me.txtInsurerNotes.Multiline = True
        Me.txtInsurerNotes.Name = "txtInsurerNotes"
        Me.txtInsurerNotes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInsurerNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtInsurerNotes.Size = New System.Drawing.Size(341, 53)
        Me.txtInsurerNotes.TabIndex = 98
        '
        'uctPMAddressControl1
        '
        Me.uctPMAddressControl1.AddressLine1 = ""
        Me.uctPMAddressControl1.AddressLine2 = ""
        Me.uctPMAddressControl1.AddressLine3 = ""
        Me.uctPMAddressControl1.AddressLine4 = ""
        Me.uctPMAddressControl1.Caption = ""
        Me.uctPMAddressControl1.CaptionAddress1 = "No. && street name:"
        Me.uctPMAddressControl1.CaptionAddress2 = "Locality:"
        Me.uctPMAddressControl1.CaptionAddress3 = "Town:"
        Me.uctPMAddressControl1.CaptionAddress4 = "County:"
        Me.uctPMAddressControl1.CaptionCountry = "Country:"
        Me.uctPMAddressControl1.CaptionFontBoldAddress1 = False
        Me.uctPMAddressControl1.CaptionFontBoldPostCode = False
        Me.uctPMAddressControl1.CaptionPostCode = "Postcode:"
        Me.uctPMAddressControl1.ClearButtonCaption = "X"
        Me.uctPMAddressControl1.ClearButtonFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMAddressControl1.ClearButtonLeft = 6675
        Me.uctPMAddressControl1.ClearButtonWidth = 360
        Me.uctPMAddressControl1.CountryId = 0
        Me.uctPMAddressControl1.FaceFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMAddressControl1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMAddressControl1.IsCountryRequired = 1
        Me.uctPMAddressControl1.IsPostCodeRequired = 1
        Me.uctPMAddressControl1.Location = New System.Drawing.Point(16, 40)
        Me.uctPMAddressControl1.Name = "uctPMAddressControl1"
        Me.uctPMAddressControl1.Organisation = ""
        Me.uctPMAddressControl1.PMAddressCnt = 0
        Me.uctPMAddressControl1.PMDatabaseID = 0
        Me.uctPMAddressControl1.PostCode = ""
        Me.uctPMAddressControl1.QAS2PMAddress1 = "3,4,2,5,6"
        Me.uctPMAddressControl1.QAS2PMAddress2 = "8,7"
        Me.uctPMAddressControl1.QAS2PMAddress3 = "9"
        Me.uctPMAddressControl1.QAS2PMAddress4 = ""
        Me.uctPMAddressControl1.QASDatabaseID = 0
        Me.uctPMAddressControl1.SearchButtonCaption = ".."
        Me.uctPMAddressControl1.SearchButtonFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMAddressControl1.SearchButtonHeight = 285
        Me.uctPMAddressControl1.SearchButtonLeft = 6240
        Me.uctPMAddressControl1.SearchButtonTop = 1530
        Me.uctPMAddressControl1.SearchButtonWidth = 360
        Me.uctPMAddressControl1.Size = New System.Drawing.Size(481, 152)
        Me.uctPMAddressControl1.TabIndex = 92
        Me.uctPMAddressControl1.WarningMessage = ""
        '
        'txtInsurerEmailAddress
        '
        Me.txtInsurerEmailAddress.AcceptsReturn = True
        Me.txtInsurerEmailAddress.BackColor = System.Drawing.SystemColors.Window
        Me.txtInsurerEmailAddress.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInsurerEmailAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInsurerEmailAddress.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInsurerEmailAddress.Location = New System.Drawing.Point(152, 272)
        Me.txtInsurerEmailAddress.MaxLength = 255
        Me.txtInsurerEmailAddress.Name = "txtInsurerEmailAddress"
        Me.txtInsurerEmailAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInsurerEmailAddress.Size = New System.Drawing.Size(341, 20)
        Me.txtInsurerEmailAddress.TabIndex = 96
        '
        'txtInsurerContactName
        '
        Me.txtInsurerContactName.AcceptsReturn = True
        Me.txtInsurerContactName.BackColor = System.Drawing.SystemColors.Window
        Me.txtInsurerContactName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInsurerContactName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInsurerContactName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInsurerContactName.Location = New System.Drawing.Point(152, 248)
        Me.txtInsurerContactName.MaxLength = 255
        Me.txtInsurerContactName.Name = "txtInsurerContactName"
        Me.txtInsurerContactName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInsurerContactName.Size = New System.Drawing.Size(341, 20)
        Me.txtInsurerContactName.TabIndex = 95
        '
        'txtInsurerFaxNo
        '
        Me.txtInsurerFaxNo.AcceptsReturn = True
        Me.txtInsurerFaxNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtInsurerFaxNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInsurerFaxNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInsurerFaxNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInsurerFaxNo.Location = New System.Drawing.Point(152, 224)
        Me.txtInsurerFaxNo.MaxLength = 60
        Me.txtInsurerFaxNo.Name = "txtInsurerFaxNo"
        Me.txtInsurerFaxNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInsurerFaxNo.Size = New System.Drawing.Size(149, 20)
        Me.txtInsurerFaxNo.TabIndex = 94
        '
        'txtInsurerTelNo
        '
        Me.txtInsurerTelNo.AcceptsReturn = True
        Me.txtInsurerTelNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtInsurerTelNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInsurerTelNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInsurerTelNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInsurerTelNo.Location = New System.Drawing.Point(152, 200)
        Me.txtInsurerTelNo.MaxLength = 60
        Me.txtInsurerTelNo.Name = "txtInsurerTelNo"
        Me.txtInsurerTelNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInsurerTelNo.Size = New System.Drawing.Size(149, 20)
        Me.txtInsurerTelNo.TabIndex = 93
        '
        'txtInsurerName
        '
        Me.txtInsurerName.AcceptsReturn = True
        Me.txtInsurerName.BackColor = System.Drawing.SystemColors.Window
        Me.txtInsurerName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInsurerName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInsurerName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInsurerName.Location = New System.Drawing.Point(152, 16)
        Me.txtInsurerName.MaxLength = 255
        Me.txtInsurerName.Name = "txtInsurerName"
        Me.txtInsurerName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInsurerName.Size = New System.Drawing.Size(349, 20)
        Me.txtInsurerName.TabIndex = 91
        '
        'lblInsurerNotes
        '
        Me.lblInsurerNotes.BackColor = System.Drawing.SystemColors.Control
        Me.lblInsurerNotes.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInsurerNotes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInsurerNotes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInsurerNotes.Location = New System.Drawing.Point(16, 296)
        Me.lblInsurerNotes.Name = "lblInsurerNotes"
        Me.lblInsurerNotes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInsurerNotes.Size = New System.Drawing.Size(91, 17)
        Me.lblInsurerNotes.TabIndex = 97
        Me.lblInsurerNotes.Text = "Notes:"
        '
        'lblInsurerEmailAddress
        '
        Me.lblInsurerEmailAddress.BackColor = System.Drawing.SystemColors.Control
        Me.lblInsurerEmailAddress.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInsurerEmailAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInsurerEmailAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInsurerEmailAddress.Location = New System.Drawing.Point(16, 272)
        Me.lblInsurerEmailAddress.Name = "lblInsurerEmailAddress"
        Me.lblInsurerEmailAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInsurerEmailAddress.Size = New System.Drawing.Size(91, 17)
        Me.lblInsurerEmailAddress.TabIndex = 90
        Me.lblInsurerEmailAddress.Text = "Email Address:"
        '
        'lblInsurerContactName
        '
        Me.lblInsurerContactName.BackColor = System.Drawing.SystemColors.Control
        Me.lblInsurerContactName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInsurerContactName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInsurerContactName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInsurerContactName.Location = New System.Drawing.Point(16, 248)
        Me.lblInsurerContactName.Name = "lblInsurerContactName"
        Me.lblInsurerContactName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInsurerContactName.Size = New System.Drawing.Size(91, 17)
        Me.lblInsurerContactName.TabIndex = 89
        Me.lblInsurerContactName.Text = "Contact Name:"
        '
        'lblInsurerFaxNo
        '
        Me.lblInsurerFaxNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblInsurerFaxNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInsurerFaxNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInsurerFaxNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInsurerFaxNo.Location = New System.Drawing.Point(16, 224)
        Me.lblInsurerFaxNo.Name = "lblInsurerFaxNo"
        Me.lblInsurerFaxNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInsurerFaxNo.Size = New System.Drawing.Size(91, 17)
        Me.lblInsurerFaxNo.TabIndex = 88
        Me.lblInsurerFaxNo.Text = "Fax Number:"
        '
        'lblInsurerTelNo
        '
        Me.lblInsurerTelNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblInsurerTelNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInsurerTelNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInsurerTelNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInsurerTelNo.Location = New System.Drawing.Point(16, 200)
        Me.lblInsurerTelNo.Name = "lblInsurerTelNo"
        Me.lblInsurerTelNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInsurerTelNo.Size = New System.Drawing.Size(115, 17)
        Me.lblInsurerTelNo.TabIndex = 87
        Me.lblInsurerTelNo.Text = "Telephone Number:"
        '
        'lblInsurerName
        '
        Me.lblInsurerName.BackColor = System.Drawing.SystemColors.Control
        Me.lblInsurerName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInsurerName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInsurerName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInsurerName.Location = New System.Drawing.Point(16, 16)
        Me.lblInsurerName.Name = "lblInsurerName"
        Me.lblInsurerName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInsurerName.Size = New System.Drawing.Size(91, 17)
        Me.lblInsurerName.TabIndex = 86
        Me.lblInsurerName.Text = "Insurer Name:"
        '
        '_tabMainTab_TabPage7
        '
        Me._tabMainTab_TabPage7.Controls.Add(Me._cmdNext_6)
        Me._tabMainTab_TabPage7.Controls.Add(Me._cmdPrevious_6)
        Me._tabMainTab_TabPage7.Controls.Add(Me.uctPartyBankControl1)
        Me._tabMainTab_TabPage7.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage7.Name = "_tabMainTab_TabPage7"
        Me._tabMainTab_TabPage7.Size = New System.Drawing.Size(637, 392)
        Me._tabMainTab_TabPage7.TabIndex = 7
        Me._tabMainTab_TabPage7.Text = "7- Bank"
        Me._tabMainTab_TabPage7.UseVisualStyleBackColor = True
        '
        '_cmdNext_6
        '
        Me._cmdNext_6.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_6.Location = New System.Drawing.Point(603, 360)
        Me._cmdNext_6.Name = "_cmdNext_6"
        Me._cmdNext_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_6.Size = New System.Drawing.Size(30, 19)
        Me._cmdNext_6.TabIndex = 103
        Me._cmdNext_6.Text = "&>>"
        Me._cmdNext_6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_6.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_6
        '
        Me._cmdPrevious_6.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_6.Location = New System.Drawing.Point(4, 360)
        Me._cmdPrevious_6.Name = "_cmdPrevious_6"
        Me._cmdPrevious_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_6.Size = New System.Drawing.Size(30, 19)
        Me._cmdPrevious_6.TabIndex = 101
        Me._cmdPrevious_6.Text = "&<<"
        Me._cmdPrevious_6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_6.UseVisualStyleBackColor = False
        '
        'uctPartyBankControl1
        '
        Me.uctPartyBankControl1.AccountId = Nothing
        Me.uctPartyBankControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPartyBankControl1.IsExternalCreditCardProcessing = False
        Me.uctPartyBankControl1.Location = New System.Drawing.Point(0, 4)
        Me.uctPartyBankControl1.Name = "uctPartyBankControl1"
        Me.uctPartyBankControl1.PartyBankDetails = Nothing
        Me.uctPartyBankControl1.PartyBankHistory = Nothing
        Me.uctPartyBankControl1.PartyCnt = Nothing
        Me.uctPartyBankControl1.Size = New System.Drawing.Size(637, 355)
        Me.uctPartyBankControl1.TabIndex = 102
        '
        '_tabMainTab_TabPage8
        '
        Me._tabMainTab_TabPage8.Controls.Add(Me.uctPickListBranches)
        Me._tabMainTab_TabPage8.Controls.Add(Me._cmdPrevious_7)
        Me._tabMainTab_TabPage8.Location = New System.Drawing.Point(4, 40)
        Me._tabMainTab_TabPage8.Name = "_tabMainTab_TabPage8"
        Me._tabMainTab_TabPage8.Size = New System.Drawing.Size(637, 374)
        Me._tabMainTab_TabPage8.TabIndex = 8
        Me._tabMainTab_TabPage8.Text = "8- Branch"
        Me._tabMainTab_TabPage8.UseVisualStyleBackColor = True
        '
        'uctPickListBranches
        '
        Me.uctPickListBranches.AvailableCaption = "Restrict to Branches"
        Me.uctPickListBranches.BusinessObject = "bSIRPartyOT.Business"
        Me.uctPickListBranches.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPickListBranches.ForeignKeys = CType(resources.GetObject("uctPickListBranches.ForeignKeys"), Microsoft.VisualBasic.Collection)
        Me.uctPickListBranches.IsSearchable = False
        Me.uctPickListBranches.Location = New System.Drawing.Point(27, 21)
        Me.uctPickListBranches.Name = "uctPickListBranches"
        Me.uctPickListBranches.PickListType = "Source"
        Me.uctPickListBranches.Size = New System.Drawing.Size(553, 273)
        Me.uctPickListBranches.TabIndex = 110
        '
        '_cmdPrevious_7
        '
        Me._cmdPrevious_7.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_7.Location = New System.Drawing.Point(3, 359)
        Me._cmdPrevious_7.Name = "_cmdPrevious_7"
        Me._cmdPrevious_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_7.Size = New System.Drawing.Size(30, 19)
        Me._cmdPrevious_7.TabIndex = 104
        Me._cmdPrevious_7.Text = "&<<"
        Me._cmdPrevious_7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_7.UseVisualStyleBackColor = False
        '
        'Image1
        '
        Me.Image1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Image1.Image = CType(resources.GetObject("Image1.Image"), System.Drawing.Image)
        Me.Image1.Location = New System.Drawing.Point(-4440, 32)
        Me.Image1.Name = "Image1"
        Me.Image1.Size = New System.Drawing.Size(32, 32)
        Me.Image1.TabIndex = 0
        Me.Image1.TabStop = False
        '
        'cboTPASettle
        '
        Me.cboTPASettle.FormattingEnabled = True
        Me.cboTPASettle.Items.AddRange(New Object() {"No", "Yes"})
        Me.cboTPASettle.Location = New System.Drawing.Point(436, 237)
        Me.cboTPASettle.Name = "cboTPASettle"
        Me.cboTPASettle.Size = New System.Drawing.Size(120, 21)
        Me.cboTPASettle.TabIndex = 100

        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(334, 243)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(96, 13)
        Me.Label3.TabIndex = 101
        Me.Label3.Text = "TPA Settle Directly"
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(649, 476)
        Me.Controls.Add(Me.Toolbar1)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.txtCurrency)
        Me.Controls.Add(Me.txtDate)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Other Party"
        Me.Toolbar1.ResumeLayout(False)
        Me.Toolbar1.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me.fraSupplier.ResumeLayout(False)
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me.fraAddress.ResumeLayout(False)
        Me.fraContactDetails.ResumeLayout(False)
        Me.fraContactDetails.PerformLayout()
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me.fraSupply.ResumeLayout(False)
        Me.fraSpeciality.ResumeLayout(False)
        Me._tabMainTab_TabPage3.ResumeLayout(False)
        Me.Frame2.ResumeLayout(False)
        Me._tabMainTab_TabPage4.ResumeLayout(False)
        Me.fraAccident.ResumeLayout(False)
        Me._tabMainTab_TabPage5.ResumeLayout(False)
        Me._tabMainTab_TabPage5.PerformLayout()
        Me._tabMainTab_TabPage6.ResumeLayout(False)
        Me.fraInsurerDetails.ResumeLayout(False)
        Me.fraInsurerDetails.PerformLayout()
        Me._tabMainTab_TabPage7.ResumeLayout(False)
        Me._tabMainTab_TabPage8.ResumeLayout(False)
        CType(Me.Image1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializecmdPrevious()
        Me.cmdPrevious(6) = _cmdPrevious_6
        Me.cmdPrevious(5) = _cmdPrevious_5
        Me.cmdPrevious(4) = _cmdPrevious_4
        Me.cmdPrevious(3) = _cmdPrevious_3
        Me.cmdPrevious(2) = _cmdPrevious_2
        Me.cmdPrevious(1) = _cmdPrevious_1
        Me.cmdPrevious(0) = _cmdPrevious_0
    End Sub
    Sub InitializecmdNext()
        Me.cmdNext(5) = _cmdNext_5
        Me.cmdNext(4) = _cmdNext_4
        Me.cmdNext(3) = _cmdNext_3
        Me.cmdNext(2) = _cmdNext_2
        Me.cmdNext(1) = _cmdNext_1
        Me.cmdNext(0) = _cmdNext_0
    End Sub
    Sub lvwAccidents_InitializeColumnKeys()
        Me._lvwAccidents_ColumnHeader_1.Name = ""
        Me._lvwAccidents_ColumnHeader_2.Name = ""
        Me._lvwAccidents_ColumnHeader_3.Name = ""
    End Sub
    Sub lvwConvictions_InitializeColumnKeys()
        Me._lvwConvictions_ColumnHeader_1.Name = ""
        Me._lvwConvictions_ColumnHeader_2.Name = ""
        Me._lvwConvictions_ColumnHeader_3.Name = ""
        Me._lvwConvictions_ColumnHeader_4.Name = ""
        Me._lvwConvictions_ColumnHeader_5.Name = ""
        Me._lvwConvictions_ColumnHeader_6.Name = ""
    End Sub
    Sub lvwSupSpecSelected_InitializeColumnKeys()
        Me._lvwSupSpecSelected_ColumnHeader_1.Name = ""
    End Sub
    Sub lvwSupSpecAvailable_InitializeColumnKeys()
        Me._lvwSupSpecAvailable_ColumnHeader_1.Name = ""
    End Sub
    Sub lvwSupBusSelected_InitializeColumnKeys()
        Me._lvwSupBusSelected_ColumnHeader_1.Name = ""
    End Sub
    Sub lvwSupBusAvailable_InitializeColumnKeys()
        Me._lvwSupBusAvailable_ColumnHeader_1.Name = ""
    End Sub
    Sub lvwAddresses_InitializeColumnKeys()
        Me._lvwAddresses_ColumnHeader_1.Name = ""
        Me._lvwAddresses_ColumnHeader_2.Name = ""
        Me._lvwAddresses_ColumnHeader_3.Name = ""
        Me._lvwAddresses_ColumnHeader_4.Name = ""
        Me._lvwAddresses_ColumnHeader_5.Name = ""
        Me._lvwAddresses_ColumnHeader_6.Name = ""
    End Sub
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Private WithEvents _cmdNext_6 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage8 As System.Windows.Forms.TabPage
    Public WithEvents uctPickListBranches As uctPickList.PickList
    Private WithEvents _cmdPrevious_7 As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cboTPASettle As System.Windows.Forms.ComboBox
#End Region
End Class
