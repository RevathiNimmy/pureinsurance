<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializecmdPrev()
		InitializecmdNext()
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
    Public WithEvents txtProductType As System.Windows.Forms.TextBox
    Public WithEvents txtTransType As System.Windows.Forms.TextBox
    Public WithEvents txtEffectiveDate As System.Windows.Forms.TextBox
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents imgIcon As System.Windows.Forms.PictureBox
    Public WithEvents chkRiskTransferAgreement As System.Windows.Forms.CheckBox
    Public WithEvents chkDelegatedAuthority As System.Windows.Forms.CheckBox
    Public WithEvents cboFSAProduct As PMLookupControl.cboPMLookup
    Public WithEvents uctCurrency As UserControls.CurrencyLookup
    Public WithEvents txtAgencyNum As System.Windows.Forms.TextBox
    Public WithEvents cmdEditFees As System.Windows.Forms.Button
    Public WithEvents cmdDeleteFees As System.Windows.Forms.Button
    Public WithEvents cmdAddFees As System.Windows.Forms.Button
    Public WithEvents cboPaymentMethod As System.Windows.Forms.ComboBox
    Public WithEvents txtAccountName As System.Windows.Forms.TextBox
    Public WithEvents txtAccountCode As System.Windows.Forms.TextBox
    Private WithEvents _lvwFees_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwFees_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwFees_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwFees_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwFees_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwFees_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwFees_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwFees As System.Windows.Forms.ListView
    Public WithEvents lblFSAProduct As System.Windows.Forms.Label
    Public WithEvents lblAgencyNum As System.Windows.Forms.Label
    Public WithEvents lblAccountType As System.Windows.Forms.Label
    Public WithEvents lblCurrency As System.Windows.Forms.Label
    Public WithEvents lblPaymentMethod As System.Windows.Forms.Label
    Public WithEvents lblAccType As System.Windows.Forms.Label
    Public WithEvents lblAccountName As System.Windows.Forms.Label
    Public WithEvents lblAccountCode As System.Windows.Forms.Label
    Public WithEvents fmeMain As System.Windows.Forms.GroupBox
    Public WithEvents txtPercentage As System.Windows.Forms.TextBox
    Public WithEvents txtAmount As System.Windows.Forms.TextBox
    Private WithEvents _cmdNext_0 As System.Windows.Forms.Button
    Public WithEvents chkCRFee_charge As System.Windows.Forms.CheckBox
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Private WithEvents _cmdPrev_0 As System.Windows.Forms.Button
    Private WithEvents _cmdNext_1 As System.Windows.Forms.Button
    Public WithEvents cmdEditAd As System.Windows.Forms.Button
    Public WithEvents cmdDeleteAd As System.Windows.Forms.Button
    Public WithEvents cmdAddAd As System.Windows.Forms.Button
    Private WithEvents _lvwAddresses_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddresses_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddresses_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddresses_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddresses_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddresses_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwAddresses As System.Windows.Forms.ListView
    Public WithEvents fraAddress As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
    Private WithEvents _cmdNext_2 As System.Windows.Forms.Button
    Private WithEvents _cmdPrev_1 As System.Windows.Forms.Button
    Public WithEvents cmdAddCon As System.Windows.Forms.Button
    Public WithEvents cmdDeleteCon As System.Windows.Forms.Button
    Public WithEvents cmdEditCon As System.Windows.Forms.Button
    Private WithEvents _lvwContacts_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwContacts_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwContacts_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwContacts_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwContacts_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwContacts As System.Windows.Forms.ListView
    Public WithEvents fraContact As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
    Private WithEvents _cmdPrev_2 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage3 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public WithEvents imlIcons As System.Windows.Forms.ImageList
    Public cmdNext(2) As System.Windows.Forms.Button
    Public cmdPrev(2) As System.Windows.Forms.Button
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    Private tabMainTabPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtProductType = New System.Windows.Forms.TextBox
        Me.txtTransType = New System.Windows.Forms.TextBox
        Me.txtEffectiveDate = New System.Windows.Forms.TextBox
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
        Me.imgIcon = New System.Windows.Forms.PictureBox
        Me.fmeMain = New System.Windows.Forms.GroupBox
        Me.chkRiskTransferAgreement = New System.Windows.Forms.CheckBox
        Me.chkDelegatedAuthority = New System.Windows.Forms.CheckBox
        Me.cboFSAProduct = New PMLookupControl.cboPMLookup
        Me.uctCurrency = New UserControls.CurrencyLookup
        Me.txtAgencyNum = New System.Windows.Forms.TextBox
        Me.cmdEditFees = New System.Windows.Forms.Button
        Me.cmdDeleteFees = New System.Windows.Forms.Button
        Me.cmdAddFees = New System.Windows.Forms.Button
        Me.cboPaymentMethod = New System.Windows.Forms.ComboBox
        Me.txtAccountName = New System.Windows.Forms.TextBox
        Me.txtAccountCode = New System.Windows.Forms.TextBox
        Me.lvwFees = New System.Windows.Forms.ListView
        Me._lvwFees_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwFees_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwFees_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwFees_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwFees_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwFees_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwFees_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me.lblFSAProduct = New System.Windows.Forms.Label
        Me.lblAgencyNum = New System.Windows.Forms.Label
        Me.lblAccountType = New System.Windows.Forms.Label
        Me.lblCurrency = New System.Windows.Forms.Label
        Me.lblPaymentMethod = New System.Windows.Forms.Label
        Me.lblAccType = New System.Windows.Forms.Label
        Me.lblAccountName = New System.Windows.Forms.Label
        Me.lblAccountCode = New System.Windows.Forms.Label
        Me.txtPercentage = New System.Windows.Forms.TextBox
        Me.txtAmount = New System.Windows.Forms.TextBox
        Me._cmdNext_0 = New System.Windows.Forms.Button
        Me.chkCRFee_charge = New System.Windows.Forms.CheckBox
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me._cmdPrev_0 = New System.Windows.Forms.Button
        Me._cmdNext_1 = New System.Windows.Forms.Button
        Me.fraAddress = New System.Windows.Forms.GroupBox
        Me.cmdEditAd = New System.Windows.Forms.Button
        Me.cmdDeleteAd = New System.Windows.Forms.Button
        Me.cmdAddAd = New System.Windows.Forms.Button
        Me.lvwAddresses = New System.Windows.Forms.ListView
        Me._lvwAddresses_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddresses_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddresses_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddresses_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddresses_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddresses_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage
        Me._cmdNext_2 = New System.Windows.Forms.Button
        Me._cmdPrev_1 = New System.Windows.Forms.Button
        Me.fraContact = New System.Windows.Forms.GroupBox
        Me.cmdAddCon = New System.Windows.Forms.Button
        Me.cmdDeleteCon = New System.Windows.Forms.Button
        Me.cmdEditCon = New System.Windows.Forms.Button
        Me.lvwContacts = New System.Windows.Forms.ListView
        Me._lvwContacts_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._tabMainTab_TabPage3 = New System.Windows.Forms.TabPage
        Me._cmdPrev_2 = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.uctPartyTax1 = New uctPartyTaxControl.uctPartyTax
        Me.imlIcons = New System.Windows.Forms.ImageList(Me.components)
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fmeMain.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me.fraAddress.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me.fraContact.SuspendLayout()
        Me._tabMainTab_TabPage3.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtProductType
        '
        Me.txtProductType.AcceptsReturn = True
        Me.txtProductType.BackColor = System.Drawing.SystemColors.Window
        Me.txtProductType.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtProductType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtProductType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtProductType.Location = New System.Drawing.Point(48, 488)
        Me.txtProductType.MaxLength = 0
        Me.txtProductType.Name = "txtProductType"
        Me.txtProductType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtProductType.Size = New System.Drawing.Size(12, 20)
        Me.txtProductType.TabIndex = 35
        Me.txtProductType.Text = " "
        Me.txtProductType.Visible = False
        '
        'txtTransType
        '
        Me.txtTransType.AcceptsReturn = True
        Me.txtTransType.BackColor = System.Drawing.SystemColors.Window
        Me.txtTransType.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTransType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTransType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTransType.Location = New System.Drawing.Point(24, 488)
        Me.txtTransType.MaxLength = 0
        Me.txtTransType.Name = "txtTransType"
        Me.txtTransType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTransType.Size = New System.Drawing.Size(17, 20)
        Me.txtTransType.TabIndex = 34
        Me.txtTransType.Text = " "
        Me.txtTransType.Visible = False
        '
        'txtEffectiveDate
        '
        Me.txtEffectiveDate.AcceptsReturn = True
        Me.txtEffectiveDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtEffectiveDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEffectiveDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEffectiveDate.Location = New System.Drawing.Point(0, 488)
        Me.txtEffectiveDate.MaxLength = 0
        Me.txtEffectiveDate.Name = "txtEffectiveDate"
        Me.txtEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEffectiveDate.Size = New System.Drawing.Size(17, 20)
        Me.txtEffectiveDate.TabIndex = 33
        Me.txtEffectiveDate.Text = " "
        Me.txtEffectiveDate.Visible = False
        '
        'dlgHelpOpen
        '
        Me.dlgHelpOpen.FileName = "v"
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(533, 368)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 12
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
        Me.cmdCancel.Location = New System.Drawing.Point(453, 368)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 11
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
        Me.cmdOK.Location = New System.Drawing.Point(373, 368)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 10
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
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(148, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 0)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(602, 365)
        Me.tabMainTab.TabIndex = 13
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.imgIcon)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fmeMain)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtPercentage)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtAmount)
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdNext_0)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkCRFee_charge)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(594, 339)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Special Party"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'imgIcon
        '
        Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon.Location = New System.Drawing.Point(627, 44)
        Me.imgIcon.Name = "imgIcon"
        Me.imgIcon.Size = New System.Drawing.Size(32, 32)
        Me.imgIcon.TabIndex = 0
        Me.imgIcon.TabStop = False
        '
        'fmeMain
        '
        Me.fmeMain.BackColor = System.Drawing.SystemColors.Control
        Me.fmeMain.Controls.Add(Me.chkRiskTransferAgreement)
        Me.fmeMain.Controls.Add(Me.chkDelegatedAuthority)
        Me.fmeMain.Controls.Add(Me.cboFSAProduct)
        Me.fmeMain.Controls.Add(Me.uctCurrency)
        Me.fmeMain.Controls.Add(Me.txtAgencyNum)
        Me.fmeMain.Controls.Add(Me.cmdEditFees)
        Me.fmeMain.Controls.Add(Me.cmdDeleteFees)
        Me.fmeMain.Controls.Add(Me.cmdAddFees)
        Me.fmeMain.Controls.Add(Me.cboPaymentMethod)
        Me.fmeMain.Controls.Add(Me.txtAccountName)
        Me.fmeMain.Controls.Add(Me.txtAccountCode)
        Me.fmeMain.Controls.Add(Me.lvwFees)
        Me.fmeMain.Controls.Add(Me.lblFSAProduct)
        Me.fmeMain.Controls.Add(Me.lblAgencyNum)
        Me.fmeMain.Controls.Add(Me.lblAccountType)
        Me.fmeMain.Controls.Add(Me.lblCurrency)
        Me.fmeMain.Controls.Add(Me.lblPaymentMethod)
        Me.fmeMain.Controls.Add(Me.lblAccType)
        Me.fmeMain.Controls.Add(Me.lblAccountName)
        Me.fmeMain.Controls.Add(Me.lblAccountCode)
        Me.fmeMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fmeMain.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fmeMain.Location = New System.Drawing.Point(8, 4)
        Me.fmeMain.Name = "fmeMain"
        Me.fmeMain.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fmeMain.Size = New System.Drawing.Size(577, 305)
        Me.fmeMain.TabIndex = 24
        Me.fmeMain.TabStop = False
        '
        'chkRiskTransferAgreement
        '
        Me.chkRiskTransferAgreement.BackColor = System.Drawing.SystemColors.Control
        Me.chkRiskTransferAgreement.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkRiskTransferAgreement.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRiskTransferAgreement.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRiskTransferAgreement.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRiskTransferAgreement.Location = New System.Drawing.Point(8, 128)
        Me.chkRiskTransferAgreement.Name = "chkRiskTransferAgreement"
        Me.chkRiskTransferAgreement.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRiskTransferAgreement.Size = New System.Drawing.Size(257, 16)
        Me.chkRiskTransferAgreement.TabIndex = 46
        Me.chkRiskTransferAgreement.Text = "Risk Transfer Agreement:"
        Me.chkRiskTransferAgreement.UseVisualStyleBackColor = False
        '
        'chkDelegatedAuthority
        '
        Me.chkDelegatedAuthority.BackColor = System.Drawing.SystemColors.Control
        Me.chkDelegatedAuthority.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDelegatedAuthority.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDelegatedAuthority.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDelegatedAuthority.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDelegatedAuthority.Location = New System.Drawing.Point(280, 128)
        Me.chkDelegatedAuthority.Name = "chkDelegatedAuthority"
        Me.chkDelegatedAuthority.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDelegatedAuthority.Size = New System.Drawing.Size(281, 16)
        Me.chkDelegatedAuthority.TabIndex = 45
        Me.chkDelegatedAuthority.Text = "Delegated Authority:"
        Me.chkDelegatedAuthority.UseVisualStyleBackColor = False
        '
        'cboFSAProduct
        '
        Me.cboFSAProduct.DefaultItemId = 0
        Me.cboFSAProduct.FirstItem = ""
        Me.cboFSAProduct.ItemId = 0
        Me.cboFSAProduct.ListIndex = -1
        Me.cboFSAProduct.Location = New System.Drawing.Point(392, 100)
        Me.cboFSAProduct.Name = "cboFSAProduct"
        Me.cboFSAProduct.PMLookupProductFamily = 1
        Me.cboFSAProduct.SingleItemId = 0
        Me.cboFSAProduct.Size = New System.Drawing.Size(172, 21)
        Me.cboFSAProduct.Sorted = True
        Me.cboFSAProduct.TabIndex = 43
        Me.cboFSAProduct.TableName = "FSA_Product"
        Me.cboFSAProduct.ToolTipText = ""
        Me.cboFSAProduct.WhereClause = ""
        '
        'uctCurrency
        '
        Me.uctCurrency.CompanyId = 0
        Me.uctCurrency.CurrencyId = 0
        Me.uctCurrency.DefaultCurrencyId = 0
        Me.uctCurrency.FirstItem = ""
        Me.uctCurrency.ListIndex = -1
        Me.uctCurrency.Location = New System.Drawing.Point(392, 16)
        Me.uctCurrency.Name = "uctCurrency"
        Me.uctCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actCompanyCurrencies
        Me.uctCurrency.Size = New System.Drawing.Size(172, 21)
        Me.uctCurrency.TabIndex = 1
        Me.uctCurrency.ToolTipText = ""
        Me.uctCurrency.WhatsThisHelpID = 0
        '
        'txtAgencyNum
        '
        Me.txtAgencyNum.AcceptsReturn = True
        Me.txtAgencyNum.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgencyNum.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgencyNum.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgencyNum.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgencyNum.Location = New System.Drawing.Point(112, 100)
        Me.txtAgencyNum.MaxLength = 0
        Me.txtAgencyNum.Name = "txtAgencyNum"
        Me.txtAgencyNum.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgencyNum.Size = New System.Drawing.Size(156, 20)
        Me.txtAgencyNum.TabIndex = 5
        '
        'cmdEditFees
        '
        Me.cmdEditFees.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditFees.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditFees.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditFees.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditFees.Location = New System.Drawing.Point(168, 272)
        Me.cmdEditFees.Name = "cmdEditFees"
        Me.cmdEditFees.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditFees.Size = New System.Drawing.Size(73, 23)
        Me.cmdEditFees.TabIndex = 9
        Me.cmdEditFees.Text = "&Edit"
        Me.cmdEditFees.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditFees.UseVisualStyleBackColor = False
        '
        'cmdDeleteFees
        '
        Me.cmdDeleteFees.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteFees.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteFees.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteFees.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteFees.Location = New System.Drawing.Point(88, 272)
        Me.cmdDeleteFees.Name = "cmdDeleteFees"
        Me.cmdDeleteFees.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteFees.Size = New System.Drawing.Size(73, 23)
        Me.cmdDeleteFees.TabIndex = 8
        Me.cmdDeleteFees.Text = "&Delete"
        Me.cmdDeleteFees.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteFees.UseVisualStyleBackColor = False
        '
        'cmdAddFees
        '
        Me.cmdAddFees.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddFees.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddFees.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddFees.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddFees.Location = New System.Drawing.Point(8, 272)
        Me.cmdAddFees.Name = "cmdAddFees"
        Me.cmdAddFees.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddFees.Size = New System.Drawing.Size(73, 23)
        Me.cmdAddFees.TabIndex = 7
        Me.cmdAddFees.Text = "&Add"
        Me.cmdAddFees.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddFees.UseVisualStyleBackColor = False
        '
        'cboPaymentMethod
        '
        Me.cboPaymentMethod.BackColor = System.Drawing.SystemColors.Window
        Me.cboPaymentMethod.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPaymentMethod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPaymentMethod.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPaymentMethod.Location = New System.Drawing.Point(392, 44)
        Me.cboPaymentMethod.Name = "cboPaymentMethod"
        Me.cboPaymentMethod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPaymentMethod.Size = New System.Drawing.Size(172, 21)
        Me.cboPaymentMethod.TabIndex = 3
        Me.cboPaymentMethod.Tag = "3851999 Risk_Event,Payment_Method"
        Me.cboPaymentMethod.Text = " "
        '
        'txtAccountName
        '
        Me.txtAccountName.AcceptsReturn = True
        Me.txtAccountName.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccountName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccountName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccountName.Location = New System.Drawing.Point(112, 72)
        Me.txtAccountName.MaxLength = 0
        Me.txtAccountName.Name = "txtAccountName"
        Me.txtAccountName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccountName.Size = New System.Drawing.Size(453, 20)
        Me.txtAccountName.TabIndex = 4
        '
        'txtAccountCode
        '
        Me.txtAccountCode.AcceptsReturn = True
        Me.txtAccountCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccountCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccountCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccountCode.Location = New System.Drawing.Point(112, 44)
        Me.txtAccountCode.MaxLength = 0
        Me.txtAccountCode.Name = "txtAccountCode"
        Me.txtAccountCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccountCode.Size = New System.Drawing.Size(156, 20)
        Me.txtAccountCode.TabIndex = 2
        '
        'lvwFees
        '
        Me.lvwFees.BackColor = System.Drawing.SystemColors.Window
        Me.lvwFees.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwFees, "")
        Me.lvwFees.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwFees_ColumnHeader_1, Me._lvwFees_ColumnHeader_2, Me._lvwFees_ColumnHeader_3, Me._lvwFees_ColumnHeader_4, Me._lvwFees_ColumnHeader_5, Me._lvwFees_ColumnHeader_6, Me._lvwFees_ColumnHeader_7})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwFees, False)
        Me.lvwFees.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwFees.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwFees.FullRowSelect = True
        Me.lvwFees.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwFees, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwFees, "")
        Me.lvwFees.Location = New System.Drawing.Point(10, 152)
        Me.lvwFees.MultiSelect = False
        Me.lvwFees.Name = "lvwFees"
        Me.lvwFees.Size = New System.Drawing.Size(556, 113)
        Me.listViewHelper1.SetSmallIcons(Me.lvwFees, "")
        Me.listViewHelper1.SetSorted(Me.lvwFees, False)
        Me.listViewHelper1.SetSortKey(Me.lvwFees, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwFees, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwFees.TabIndex = 6
        Me.lvwFees.UseCompatibleStateImageBehavior = False
        Me.lvwFees.View = System.Windows.Forms.View.Details
        '
        '_lvwFees_ColumnHeader_1
        '
        Me._lvwFees_ColumnHeader_1.Text = "Risk Group"
        Me._lvwFees_ColumnHeader_1.Width = 94
        '
        '_lvwFees_ColumnHeader_2
        '
        Me._lvwFees_ColumnHeader_2.Text = "Scheme"
        Me._lvwFees_ColumnHeader_2.Width = 87
        '
        '_lvwFees_ColumnHeader_3
        '
        Me._lvwFees_ColumnHeader_3.Text = "Currency"
        Me._lvwFees_ColumnHeader_3.Width = 97
        '
        '_lvwFees_ColumnHeader_4
        '
        Me._lvwFees_ColumnHeader_4.Text = "Percentage"
        Me._lvwFees_ColumnHeader_4.Width = 81
        '
        '_lvwFees_ColumnHeader_5
        '
        Me._lvwFees_ColumnHeader_5.Text = "Amount"
        Me._lvwFees_ColumnHeader_5.Width = 61
        '
        '_lvwFees_ColumnHeader_6
        '
        Me._lvwFees_ColumnHeader_6.Text = "Commission Percentage"
        Me._lvwFees_ColumnHeader_6.Width = 108
        '
        '_lvwFees_ColumnHeader_7
        '
        Me._lvwFees_ColumnHeader_7.Text = "Commission Amount"
        Me._lvwFees_ColumnHeader_7.Width = 108
        '
        'lblFSAProduct
        '
        Me.lblFSAProduct.BackColor = System.Drawing.SystemColors.Control
        Me.lblFSAProduct.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFSAProduct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFSAProduct.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFSAProduct.Location = New System.Drawing.Point(280, 102)
        Me.lblFSAProduct.Name = "lblFSAProduct"
        Me.lblFSAProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFSAProduct.Size = New System.Drawing.Size(97, 17)
        Me.lblFSAProduct.TabIndex = 44
        Me.lblFSAProduct.Text = "FSA Product:"
        '
        'lblAgencyNum
        '
        Me.lblAgencyNum.AutoSize = True
        Me.lblAgencyNum.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgencyNum.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgencyNum.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgencyNum.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgencyNum.Location = New System.Drawing.Point(8, 102)
        Me.lblAgencyNum.Name = "lblAgencyNum"
        Me.lblAgencyNum.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgencyNum.Size = New System.Drawing.Size(86, 13)
        Me.lblAgencyNum.TabIndex = 32
        Me.lblAgencyNum.Text = "Agency Number:"
        '
        'lblAccountType
        '
        Me.lblAccountType.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblAccountType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountType.Location = New System.Drawing.Point(112, 16)
        Me.lblAccountType.Name = "lblAccountType"
        Me.lblAccountType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountType.Size = New System.Drawing.Size(156, 21)
        Me.lblAccountType.TabIndex = 0
        Me.lblAccountType.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblCurrency
        '
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(280, 16)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(97, 19)
        Me.lblCurrency.TabIndex = 29
        Me.lblCurrency.Text = "Currency:"
        '
        'lblPaymentMethod
        '
        Me.lblPaymentMethod.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentMethod.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentMethod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentMethod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentMethod.Location = New System.Drawing.Point(280, 46)
        Me.lblPaymentMethod.Name = "lblPaymentMethod"
        Me.lblPaymentMethod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentMethod.Size = New System.Drawing.Size(105, 19)
        Me.lblPaymentMethod.TabIndex = 28
        Me.lblPaymentMethod.Text = "Payment method:"
        '
        'lblAccType
        '
        Me.lblAccType.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccType.Location = New System.Drawing.Point(10, 18)
        Me.lblAccType.Name = "lblAccType"
        Me.lblAccType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccType.Size = New System.Drawing.Size(105, 19)
        Me.lblAccType.TabIndex = 27
        Me.lblAccType.Text = "Account type:"
        '
        'lblAccountName
        '
        Me.lblAccountName.AutoSize = True
        Me.lblAccountName.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountName.Location = New System.Drawing.Point(10, 74)
        Me.lblAccountName.Name = "lblAccountName"
        Me.lblAccountName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountName.Size = New System.Drawing.Size(79, 13)
        Me.lblAccountName.TabIndex = 26
        Me.lblAccountName.Text = "Account name:"
        '
        'lblAccountCode
        '
        Me.lblAccountCode.AutoSize = True
        Me.lblAccountCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountCode.Location = New System.Drawing.Point(10, 46)
        Me.lblAccountCode.Name = "lblAccountCode"
        Me.lblAccountCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountCode.Size = New System.Drawing.Size(77, 13)
        Me.lblAccountCode.TabIndex = 25
        Me.lblAccountCode.Text = "Account code:"
        '
        'txtPercentage
        '
        Me.txtPercentage.AcceptsReturn = True
        Me.txtPercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtPercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPercentage.Location = New System.Drawing.Point(15, 348)
        Me.txtPercentage.MaxLength = 0
        Me.txtPercentage.Name = "txtPercentage"
        Me.txtPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPercentage.Size = New System.Drawing.Size(93, 20)
        Me.txtPercentage.TabIndex = 30
        Me.txtPercentage.Visible = False
        '
        'txtAmount
        '
        Me.txtAmount.AcceptsReturn = True
        Me.txtAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAmount.Location = New System.Drawing.Point(112, 348)
        Me.txtAmount.MaxLength = 0
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAmount.Size = New System.Drawing.Size(93, 20)
        Me.txtAmount.TabIndex = 31
        Me.txtAmount.Visible = False
        '
        '_cmdNext_0
        '
        Me._cmdNext_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_0.Location = New System.Drawing.Point(547, 316)
        Me._cmdNext_0.Name = "_cmdNext_0"
        Me._cmdNext_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_0.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_0.TabIndex = 36
        Me._cmdNext_0.Text = ">>"
        Me._cmdNext_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_0.UseVisualStyleBackColor = False
        '
        'chkCRFee_charge
        '
        Me.chkCRFee_charge.BackColor = System.Drawing.SystemColors.Control
        Me.chkCRFee_charge.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkCRFee_charge.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCRFee_charge.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCRFee_charge.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCRFee_charge.Location = New System.Drawing.Point(16, 316)
        Me.chkCRFee_charge.Name = "chkCRFee_charge"
        Me.chkCRFee_charge.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCRFee_charge.Size = New System.Drawing.Size(257, 16)
        Me.chkCRFee_charge.TabIndex = 47
        Me.chkCRFee_charge.Text = "Charge Fee on CR transactions:"
        Me.chkCRFee_charge.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdPrev_0)
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdNext_1)
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraAddress)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(594, 339)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "2 - Address"
        Me._tabMainTab_TabPage1.UseVisualStyleBackColor = True
        '
        '_cmdPrev_0
        '
        Me._cmdPrev_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrev_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrev_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrev_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrev_0.Location = New System.Drawing.Point(8, 316)
        Me._cmdPrev_0.Name = "_cmdPrev_0"
        Me._cmdPrev_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrev_0.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrev_0.TabIndex = 38
        Me._cmdPrev_0.Text = "<<"
        Me._cmdPrev_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrev_0.UseVisualStyleBackColor = False
        '
        '_cmdNext_1
        '
        Me._cmdNext_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_1.Location = New System.Drawing.Point(547, 316)
        Me._cmdNext_1.Name = "_cmdNext_1"
        Me._cmdNext_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_1.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_1.TabIndex = 37
        Me._cmdNext_1.Text = ">>"
        Me._cmdNext_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_1.UseVisualStyleBackColor = False
        '
        'fraAddress
        '
        Me.fraAddress.BackColor = System.Drawing.SystemColors.Control
        Me.fraAddress.Controls.Add(Me.cmdEditAd)
        Me.fraAddress.Controls.Add(Me.cmdDeleteAd)
        Me.fraAddress.Controls.Add(Me.cmdAddAd)
        Me.fraAddress.Controls.Add(Me.lvwAddresses)
        Me.fraAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAddress.Location = New System.Drawing.Point(8, 4)
        Me.fraAddress.Name = "fraAddress"
        Me.fraAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAddress.Size = New System.Drawing.Size(577, 305)
        Me.fraAddress.TabIndex = 14
        Me.fraAddress.TabStop = False
        '
        'cmdEditAd
        '
        Me.cmdEditAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditAd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditAd.Location = New System.Drawing.Point(168, 272)
        Me.cmdEditAd.Name = "cmdEditAd"
        Me.cmdEditAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditAd.Size = New System.Drawing.Size(73, 23)
        Me.cmdEditAd.TabIndex = 18
        Me.cmdEditAd.Text = "&Edit"
        Me.cmdEditAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditAd.UseVisualStyleBackColor = False
        '
        'cmdDeleteAd
        '
        Me.cmdDeleteAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteAd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteAd.Location = New System.Drawing.Point(88, 272)
        Me.cmdDeleteAd.Name = "cmdDeleteAd"
        Me.cmdDeleteAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteAd.Size = New System.Drawing.Size(73, 23)
        Me.cmdDeleteAd.TabIndex = 17
        Me.cmdDeleteAd.Text = "&Delete"
        Me.cmdDeleteAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteAd.UseVisualStyleBackColor = False
        '
        'cmdAddAd
        '
        Me.cmdAddAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddAd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddAd.Location = New System.Drawing.Point(8, 272)
        Me.cmdAddAd.Name = "cmdAddAd"
        Me.cmdAddAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddAd.Size = New System.Drawing.Size(73, 23)
        Me.cmdAddAd.TabIndex = 16
        Me.cmdAddAd.Text = "&Add"
        Me.cmdAddAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddAd.UseVisualStyleBackColor = False
        '
        'lvwAddresses
        '
        Me.lvwAddresses.BackColor = System.Drawing.SystemColors.Window
        Me.lvwAddresses.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwAddresses, "")
        Me.lvwAddresses.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwAddresses_ColumnHeader_1, Me._lvwAddresses_ColumnHeader_2, Me._lvwAddresses_ColumnHeader_3, Me._lvwAddresses_ColumnHeader_4, Me._lvwAddresses_ColumnHeader_5, Me._lvwAddresses_ColumnHeader_6})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwAddresses, False)
        Me.lvwAddresses.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwAddresses.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwAddresses.FullRowSelect = True
        Me.lvwAddresses.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwAddresses, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwAddresses, "")
        Me.lvwAddresses.Location = New System.Drawing.Point(10, 16)
        Me.lvwAddresses.Name = "lvwAddresses"
        Me.lvwAddresses.Size = New System.Drawing.Size(556, 249)
        Me.listViewHelper1.SetSmallIcons(Me.lvwAddresses, "")
        Me.listViewHelper1.SetSorted(Me.lvwAddresses, False)
        Me.listViewHelper1.SetSortKey(Me.lvwAddresses, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwAddresses, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwAddresses.TabIndex = 15
        Me.lvwAddresses.UseCompatibleStateImageBehavior = False
        Me.lvwAddresses.View = System.Windows.Forms.View.Details
        '
        '_lvwAddresses_ColumnHeader_1
        '
        Me._lvwAddresses_ColumnHeader_1.Text = "Post Code"
        Me._lvwAddresses_ColumnHeader_1.Width = 74
        '
        '_lvwAddresses_ColumnHeader_2
        '
        Me._lvwAddresses_ColumnHeader_2.Text = "Address Usage"
        Me._lvwAddresses_ColumnHeader_2.Width = 167
        '
        '_lvwAddresses_ColumnHeader_3
        '
        Me._lvwAddresses_ColumnHeader_3.Text = "Address Line 1"
        Me._lvwAddresses_ColumnHeader_3.Width = 97
        '
        '_lvwAddresses_ColumnHeader_4
        '
        Me._lvwAddresses_ColumnHeader_4.Text = "Address Line 2"
        Me._lvwAddresses_ColumnHeader_4.Width = 97
        '
        '_lvwAddresses_ColumnHeader_5
        '
        Me._lvwAddresses_ColumnHeader_5.Text = "Address Line 3"
        Me._lvwAddresses_ColumnHeader_5.Width = 97
        '
        '_lvwAddresses_ColumnHeader_6
        '
        Me._lvwAddresses_ColumnHeader_6.Text = "Address Line 4"
        Me._lvwAddresses_ColumnHeader_6.Width = 97
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdNext_2)
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdPrev_1)
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraContact)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(594, 339)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "3 - Contact"
        Me._tabMainTab_TabPage2.UseVisualStyleBackColor = True
        '
        '_cmdNext_2
        '
        Me._cmdNext_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_2.Location = New System.Drawing.Point(547, 316)
        Me._cmdNext_2.Name = "_cmdNext_2"
        Me._cmdNext_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_2.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_2.TabIndex = 41
        Me._cmdNext_2.Text = ">>"
        Me._cmdNext_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_2.UseVisualStyleBackColor = False
        '
        '_cmdPrev_1
        '
        Me._cmdPrev_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrev_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrev_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrev_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrev_1.Location = New System.Drawing.Point(8, 316)
        Me._cmdPrev_1.Name = "_cmdPrev_1"
        Me._cmdPrev_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrev_1.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrev_1.TabIndex = 39
        Me._cmdPrev_1.Text = "<<"
        Me._cmdPrev_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrev_1.UseVisualStyleBackColor = False
        '
        'fraContact
        '
        Me.fraContact.BackColor = System.Drawing.SystemColors.Control
        Me.fraContact.Controls.Add(Me.cmdAddCon)
        Me.fraContact.Controls.Add(Me.cmdDeleteCon)
        Me.fraContact.Controls.Add(Me.cmdEditCon)
        Me.fraContact.Controls.Add(Me.lvwContacts)
        Me.fraContact.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraContact.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraContact.Location = New System.Drawing.Point(8, 4)
        Me.fraContact.Name = "fraContact"
        Me.fraContact.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraContact.Size = New System.Drawing.Size(577, 305)
        Me.fraContact.TabIndex = 19
        Me.fraContact.TabStop = False
        '
        'cmdAddCon
        '
        Me.cmdAddCon.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddCon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddCon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddCon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddCon.Location = New System.Drawing.Point(8, 272)
        Me.cmdAddCon.Name = "cmdAddCon"
        Me.cmdAddCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddCon.Size = New System.Drawing.Size(73, 23)
        Me.cmdAddCon.TabIndex = 21
        Me.cmdAddCon.Text = "&Add"
        Me.cmdAddCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddCon.UseVisualStyleBackColor = False
        '
        'cmdDeleteCon
        '
        Me.cmdDeleteCon.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteCon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteCon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteCon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteCon.Location = New System.Drawing.Point(88, 272)
        Me.cmdDeleteCon.Name = "cmdDeleteCon"
        Me.cmdDeleteCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteCon.Size = New System.Drawing.Size(73, 23)
        Me.cmdDeleteCon.TabIndex = 22
        Me.cmdDeleteCon.Text = "&Delete"
        Me.cmdDeleteCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteCon.UseVisualStyleBackColor = False
        '
        'cmdEditCon
        '
        Me.cmdEditCon.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditCon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditCon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditCon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditCon.Location = New System.Drawing.Point(168, 272)
        Me.cmdEditCon.Name = "cmdEditCon"
        Me.cmdEditCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditCon.Size = New System.Drawing.Size(73, 23)
        Me.cmdEditCon.TabIndex = 23
        Me.cmdEditCon.Text = "&Edit"
        Me.cmdEditCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditCon.UseVisualStyleBackColor = False
        '
        'lvwContacts
        '
        Me.lvwContacts.BackColor = System.Drawing.SystemColors.Window
        Me.lvwContacts.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwContacts, "")
        Me.lvwContacts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwContacts_ColumnHeader_1, Me._lvwContacts_ColumnHeader_2, Me._lvwContacts_ColumnHeader_3, Me._lvwContacts_ColumnHeader_4, Me._lvwContacts_ColumnHeader_5})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwContacts, False)
        Me.lvwContacts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwContacts.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwContacts.FullRowSelect = True
        Me.lvwContacts.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwContacts, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwContacts, "")
        Me.lvwContacts.Location = New System.Drawing.Point(10, 16)
        Me.lvwContacts.Name = "lvwContacts"
        Me.lvwContacts.Size = New System.Drawing.Size(556, 249)
        Me.listViewHelper1.SetSmallIcons(Me.lvwContacts, "")
        Me.listViewHelper1.SetSorted(Me.lvwContacts, False)
        Me.listViewHelper1.SetSortKey(Me.lvwContacts, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwContacts, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwContacts.TabIndex = 20
        Me.lvwContacts.UseCompatibleStateImageBehavior = False
        Me.lvwContacts.View = System.Windows.Forms.View.Details
        '
        '_lvwContacts_ColumnHeader_1
        '
        Me._lvwContacts_ColumnHeader_1.Text = "Area Code"
        Me._lvwContacts_ColumnHeader_1.Width = 74
        '
        '_lvwContacts_ColumnHeader_2
        '
        Me._lvwContacts_ColumnHeader_2.Text = "Number"
        Me._lvwContacts_ColumnHeader_2.Width = 67
        '
        '_lvwContacts_ColumnHeader_3
        '
        Me._lvwContacts_ColumnHeader_3.Text = "Extension"
        Me._lvwContacts_ColumnHeader_3.Width = 67
        '
        '_lvwContacts_ColumnHeader_4
        '
        Me._lvwContacts_ColumnHeader_4.Text = "Type"
        Me._lvwContacts_ColumnHeader_4.Width = 97
        '
        '_lvwContacts_ColumnHeader_5
        '
        Me._lvwContacts_ColumnHeader_5.Text = "Description"
        Me._lvwContacts_ColumnHeader_5.Width = 134
        '
        '_tabMainTab_TabPage3
        '
        Me._tabMainTab_TabPage3.Controls.Add(Me.GroupBox1)
        Me._tabMainTab_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage3.Name = "_tabMainTab_TabPage3"
        Me._tabMainTab_TabPage3.Size = New System.Drawing.Size(594, 339)
        Me._tabMainTab_TabPage3.TabIndex = 3
        Me._tabMainTab_TabPage3.Text = "4 - Tax"
        Me._tabMainTab_TabPage3.UseVisualStyleBackColor = True
        '
        '_cmdPrev_2
        '
        Me._cmdPrev_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrev_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrev_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrev_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrev_2.Location = New System.Drawing.Point(6, 305)
        Me._cmdPrev_2.Name = "_cmdPrev_2"
        Me._cmdPrev_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrev_2.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrev_2.TabIndex = 42
        Me._cmdPrev_2.Text = "<<"
        Me._cmdPrev_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrev_2.UseVisualStyleBackColor = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me._cmdPrev_2)
        Me.GroupBox1.Controls.Add(Me.uctPartyTax1)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(594, 339)
        Me.GroupBox1.TabIndex = 43
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Party Tax"
        '
        'uctPartyTax1
        '
        Me.uctPartyTax1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.uctPartyTax1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPartyTax1.FrameOn = True
        Me.uctPartyTax1.IsDomiciledForTax = False
        Me.uctPartyTax1.Location = New System.Drawing.Point(3, 17)
        Me.uctPartyTax1.Name = "uctPartyTax1"
        Me.uctPartyTax1.Size = New System.Drawing.Size(588, 319)
        Me.uctPartyTax1.SizeHorizontal = False
        Me.uctPartyTax1.TabIndex = 41
        Me.uctPartyTax1.TaxExempt = False
        Me.uctPartyTax1.TaxNumber = ""
        Me.uctPartyTax1.TaxPercentage = 0
        '
        'imlIcons
        '
        Me.imlIcons.ImageStream = CType(resources.GetObject("imlIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imlIcons.TransparentColor = System.Drawing.SystemColors.Control
        Me.imlIcons.Images.SetKeyName(0, "AddressImage")
        Me.imlIcons.Images.SetKeyName(1, "ContactImage")
        Me.imlIcons.Images.SetKeyName(2, "PolicyImage")
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(615, 398)
        Me.Controls.Add(Me.txtProductType)
        Me.Controls.Add(Me.txtTransType)
        Me.Controls.Add(Me.txtEffectiveDate)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Tag = "3851999 Risk_Event,Payment_Method"
        Me.Text = "Special Party"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fmeMain.ResumeLayout(False)
        Me.fmeMain.PerformLayout()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me.fraAddress.ResumeLayout(False)
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me.fraContact.ResumeLayout(False)
        Me._tabMainTab_TabPage3.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializecmdPrev()
        Me.cmdPrev(2) = _cmdPrev_2
        Me.cmdPrev(1) = _cmdPrev_1
        Me.cmdPrev(0) = _cmdPrev_0
    End Sub
    Sub InitializecmdNext()
        Me.cmdNext(2) = _cmdNext_2
        Me.cmdNext(1) = _cmdNext_1
        Me.cmdNext(0) = _cmdNext_0
    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Public WithEvents uctPartyTax1 As uctPartyTaxControl.uctPartyTax
#End Region
End Class