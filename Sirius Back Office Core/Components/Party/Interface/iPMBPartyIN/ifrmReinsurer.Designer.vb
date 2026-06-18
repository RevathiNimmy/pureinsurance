<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReinsurer
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializecmdPrevious()
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
    Public WithEvents cmdApply As System.Windows.Forms.Button
    Private WithEvents FINANCIAL As System.Windows.Forms.ToolStripButton
    Private WithEvents NOTE As System.Windows.Forms.ToolStripButton
    Private WithEvents _Toolbar1_Button3 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents LETTER As System.Windows.Forms.ToolStripButton
    Private WithEvents EMAIL As System.Windows.Forms.ToolStripButton
    Private WithEvents _Toolbar1_Button6 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents WEB As System.Windows.Forms.ToolStripButton
    Public WithEvents Toolbar1 As System.Windows.Forms.ToolStrip
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents imgIcon As System.Windows.Forms.PictureBox
    Public WithEvents cboSubBranch As System.Windows.Forms.ComboBox
    Public WithEvents uctBranch As System.Windows.Forms.ComboBox
    Public WithEvents cboTreatyNumber As PMLookupControl.cboPMLookup
    Public WithEvents txtName As System.Windows.Forms.TextBox
    Public WithEvents txtIDReference As System.Windows.Forms.TextBox
    Public WithEvents lblBranch As System.Windows.Forms.Label
    Public WithEvents lblSubBranch As System.Windows.Forms.Label
    Public WithEvents lblTreatyNumber As System.Windows.Forms.Label
    Public WithEvents lblName As System.Windows.Forms.Label
    Public WithEvents lblIDReference As System.Windows.Forms.Label
    Public WithEvents Frame3 As System.Windows.Forms.GroupBox
    Public WithEvents ChkIsRIBroker As System.Windows.Forms.CheckBox
    Public WithEvents chkIsRetained As System.Windows.Forms.CheckBox
    Public WithEvents cboReinsuranceType As PMLookupControl.cboPMLookup
    Public WithEvents txtDefaultCommissionRate As System.Windows.Forms.TextBox
    Public WithEvents lblCommission As System.Windows.Forms.Label
    Public WithEvents lblReInsuranceType As System.Windows.Forms.Label
    Public WithEvents fraReinsurance As System.Windows.Forms.GroupBox
    Public WithEvents cboReportIndicator As System.Windows.Forms.ComboBox
    Public WithEvents cboBinderIndicator As System.Windows.Forms.ComboBox
    Public WithEvents cboTermsOfPayment As System.Windows.Forms.ComboBox
    Public WithEvents cboCurrency As UserControls.CurrencyLookup
    Public WithEvents lblReportIndicator As System.Windows.Forms.Label
    Public WithEvents lblBinderIndicator As System.Windows.Forms.Label
    Public WithEvents lblTermsOfPayment As System.Windows.Forms.Label
    Public WithEvents lblCurrency As System.Windows.Forms.Label
    Public WithEvents fraAppointment As System.Windows.Forms.GroupBox
    Private WithEvents _cmdNext_0 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Private WithEvents _lvwAddresses_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddresses_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddresses_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddresses_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddresses_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddresses_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddresses_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwAddresses As System.Windows.Forms.ListView
    Public WithEvents cmdAddAd As System.Windows.Forms.Button
    Public WithEvents cmdDeleteAd As System.Windows.Forms.Button
    Public WithEvents cmdEditAd As System.Windows.Forms.Button
    Public WithEvents fraAddress As System.Windows.Forms.GroupBox
    Private WithEvents _cmdNext_1 As System.Windows.Forms.Button
    Private WithEvents _cmdPrevious_0 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents cmdEditCon As System.Windows.Forms.Button
    Public WithEvents cmdDeleteCon As System.Windows.Forms.Button
    Public WithEvents cmdAddCon As System.Windows.Forms.Button
    Private WithEvents _lvwContacts_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwContacts_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwContacts_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwContacts_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwContacts_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwContacts As System.Windows.Forms.ListView
    Public WithEvents fraContact As System.Windows.Forms.GroupBox
    Private WithEvents _cmdPrevious_1 As System.Windows.Forms.Button
    Private WithEvents _cmdNext_2 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
    Private WithEvents _cmdPrevious_2 As System.Windows.Forms.Button
    Private WithEvents _cmdNext_3 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage3 As System.Windows.Forms.TabPage
    Private WithEvents _cmdPrevious_3 As System.Windows.Forms.Button
    Public WithEvents uctPartyBankControl1 As uctPartyBank.uctPartyBankControl
    Private WithEvents _tabMainTab_TabPage4 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public WithEvents ImageList2 As System.Windows.Forms.ImageList
    Public cmdNext(3) As System.Windows.Forms.Button
    Public cmdPrevious(3) As System.Windows.Forms.Button
    Private tabMainTabPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReinsurer))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdApply = New System.Windows.Forms.Button
        Me.Toolbar1 = New System.Windows.Forms.ToolStrip
        Me.ImageList2 = New System.Windows.Forms.ImageList(Me.components)
        Me.FINANCIAL = New System.Windows.Forms.ToolStripButton
        Me.NOTE = New System.Windows.Forms.ToolStripButton
        Me._Toolbar1_Button3 = New System.Windows.Forms.ToolStripSeparator
        Me.LETTER = New System.Windows.Forms.ToolStripButton
        Me.EMAIL = New System.Windows.Forms.ToolStripButton
        Me._Toolbar1_Button6 = New System.Windows.Forms.ToolStripSeparator
        Me.WEB = New System.Windows.Forms.ToolStripButton
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
        Me.Frame3 = New System.Windows.Forms.GroupBox
        Me.cboSubBranch = New System.Windows.Forms.ComboBox
        Me.uctBranch = New System.Windows.Forms.ComboBox
        Me.cboTreatyNumber = New PMLookupControl.cboPMLookup
        Me.txtName = New System.Windows.Forms.TextBox
        Me.txtIDReference = New System.Windows.Forms.TextBox
        Me.lblBranch = New System.Windows.Forms.Label
        Me.lblSubBranch = New System.Windows.Forms.Label
        Me.lblTreatyNumber = New System.Windows.Forms.Label
        Me.lblName = New System.Windows.Forms.Label
        Me.lblIDReference = New System.Windows.Forms.Label
        Me.fraReinsurance = New System.Windows.Forms.GroupBox
        Me.ChkIsRIBroker = New System.Windows.Forms.CheckBox
        Me.chkIsRetained = New System.Windows.Forms.CheckBox
        Me.cboReinsuranceType = New PMLookupControl.cboPMLookup
        Me.txtDefaultCommissionRate = New System.Windows.Forms.TextBox
        Me.chkIsReInsuranceDebitCreditNo = New System.Windows.Forms.CheckBox
        Me.lblCommission = New System.Windows.Forms.Label
        Me.lblReInsuranceType = New System.Windows.Forms.Label
        Me.fraAppointment = New System.Windows.Forms.GroupBox
        Me.cboReportIndicator = New System.Windows.Forms.ComboBox
        Me.cboBinderIndicator = New System.Windows.Forms.ComboBox
        Me.cboTermsOfPayment = New System.Windows.Forms.ComboBox
        Me.cboCurrency = New UserControls.CurrencyLookup
        Me.lblReportIndicator = New System.Windows.Forms.Label
        Me.lblBinderIndicator = New System.Windows.Forms.Label
        Me.lblTermsOfPayment = New System.Windows.Forms.Label
        Me.lblCurrency = New System.Windows.Forms.Label
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
        Me._lvwAddresses_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me.cmdAddAd = New System.Windows.Forms.Button
        Me.cmdDeleteAd = New System.Windows.Forms.Button
        Me.cmdEditAd = New System.Windows.Forms.Button
        Me._cmdNext_1 = New System.Windows.Forms.Button
        Me._cmdPrevious_0 = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage
        Me.fraContact = New System.Windows.Forms.GroupBox
        Me.cmdEditCon = New System.Windows.Forms.Button
        Me.cmdDeleteCon = New System.Windows.Forms.Button
        Me.cmdAddCon = New System.Windows.Forms.Button
        Me.lvwContacts = New System.Windows.Forms.ListView
        Me._lvwContacts_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._cmdPrevious_1 = New System.Windows.Forms.Button
        Me._cmdNext_2 = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage3 = New System.Windows.Forms.TabPage
        Me._cmdPrevious_2 = New System.Windows.Forms.Button
        Me.fraTax = New System.Windows.Forms.GroupBox
        Me.cboTaxGroupID = New PMLookupControl.cboPMLookup
        Me.lblTaxGroupID = New System.Windows.Forms.Label
        Me.uctPartyTax1 = New uctPartyTaxControl.uctPartyTax
        Me._cmdNext_3 = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage4 = New System.Windows.Forms.TabPage
        Me._cmdPrevious_3 = New System.Windows.Forms.Button
        Me.uctPartyBankControl1 = New uctPartyBank.uctPartyBankControl
        Me._tabMainTab_TabPage5 = New System.Windows.Forms.TabPage
        Me.fraCertYears = New System.Windows.Forms.GroupBox
        Me.cmdAddCertYear = New System.Windows.Forms.Button
        Me.cmdDelCertYear = New System.Windows.Forms.Button
        Me.cmdEditCertYear = New System.Windows.Forms.Button
        Me.lvwCertYears = New System.Windows.Forms.ListView
        Me._lvwCertYears_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwCertYears_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwCertYears_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwCertYears_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwCertYears_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._cmdPrevious_4 = New System.Windows.Forms.Button
        Me._cmdNext_4 = New System.Windows.Forms.Button
        Me.Toolbar1.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Frame3.SuspendLayout()
        Me.fraReinsurance.SuspendLayout()
        Me.fraAppointment.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me.fraAddress.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me.fraContact.SuspendLayout()
        Me._tabMainTab_TabPage3.SuspendLayout()
        Me.fraTax.SuspendLayout()
        Me._tabMainTab_TabPage4.SuspendLayout()
        Me._tabMainTab_TabPage5.SuspendLayout()
        Me.fraCertYears.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Location = New System.Drawing.Point(330, 444)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApply.Size = New System.Drawing.Size(73, 22)
        Me.cmdApply.TabIndex = 48
        Me.cmdApply.Text = "&Apply"
        Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdApply.UseVisualStyleBackColor = False
        '
        'Toolbar1
        '
        Me.Toolbar1.ImageList = Me.ImageList2
        Me.Toolbar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FINANCIAL, Me.NOTE, Me._Toolbar1_Button3, Me.LETTER, Me.EMAIL, Me._Toolbar1_Button6, Me.WEB})
        Me.Toolbar1.Location = New System.Drawing.Point(0, 0)
        Me.Toolbar1.Name = "Toolbar1"
        Me.Toolbar1.Size = New System.Drawing.Size(647, 25)
        Me.Toolbar1.TabIndex = 0
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
        Me.ImageList2.Images.SetKeyName(5, "AddressImage")
        Me.ImageList2.Images.SetKeyName(6, "")
        Me.ImageList2.Images.SetKeyName(7, "ContactImage")
        '
        'FINANCIAL
        '
        Me.FINANCIAL.AutoSize = False
        Me.FINANCIAL.ImageIndex = 0
        Me.FINANCIAL.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.FINANCIAL.Name = "FINANCIAL"
        Me.FINANCIAL.Size = New System.Drawing.Size(24, 22)
        Me.FINANCIAL.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.FINANCIAL.ToolTipText = "Financial"
        '
        'NOTE
        '
        Me.NOTE.AutoSize = False
        Me.NOTE.ImageIndex = 2
        Me.NOTE.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.NOTE.Name = "NOTE"
        Me.NOTE.Size = New System.Drawing.Size(24, 22)
        Me.NOTE.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.NOTE.ToolTipText = "Notes"
        '
        '_Toolbar1_Button3
        '
        Me._Toolbar1_Button3.AutoSize = False
        Me._Toolbar1_Button3.Name = "_Toolbar1_Button3"
        Me._Toolbar1_Button3.Size = New System.Drawing.Size(6, 22)
        '
        'LETTER
        '
        Me.LETTER.AutoSize = False
        Me.LETTER.ImageIndex = 3
        Me.LETTER.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.LETTER.Name = "LETTER"
        Me.LETTER.Size = New System.Drawing.Size(24, 22)
        Me.LETTER.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.LETTER.ToolTipText = "Letter"
        '
        'EMAIL
        '
        Me.EMAIL.AutoSize = False
        Me.EMAIL.ImageIndex = 5
        Me.EMAIL.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.EMAIL.Name = "EMAIL"
        Me.EMAIL.Size = New System.Drawing.Size(24, 22)
        Me.EMAIL.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.EMAIL.ToolTipText = "E-Mail"
        '
        '_Toolbar1_Button6
        '
        Me._Toolbar1_Button6.AutoSize = False
        Me._Toolbar1_Button6.Name = "_Toolbar1_Button6"
        Me._Toolbar1_Button6.Size = New System.Drawing.Size(6, 22)
        '
        'WEB
        '
        Me.WEB.AutoSize = False
        Me.WEB.ImageIndex = 6
        Me.WEB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.WEB.Name = "WEB"
        Me.WEB.Size = New System.Drawing.Size(24, 22)
        Me.WEB.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.WEB.ToolTipText = "Web "
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(570, 444)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 51
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
        Me.cmdCancel.Location = New System.Drawing.Point(490, 444)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 50
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
        Me.cmdOK.Location = New System.Drawing.Point(410, 444)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 49
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
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(105, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(7, 32)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(640, 409)
        Me.tabMainTab.TabIndex = 1
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.imgIcon)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Frame3)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraReinsurance)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraAppointment)
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdNext_0)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(632, 383)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Insurer"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'imgIcon
        '
        Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon.Location = New System.Drawing.Point(627, 4)
        Me.imgIcon.Name = "imgIcon"
        Me.imgIcon.Size = New System.Drawing.Size(32, 32)
        Me.imgIcon.TabIndex = 0
        Me.imgIcon.TabStop = False
        '
        'Frame3
        '
        Me.Frame3.BackColor = System.Drawing.SystemColors.Control
        Me.Frame3.Controls.Add(Me.cboSubBranch)
        Me.Frame3.Controls.Add(Me.uctBranch)
        Me.Frame3.Controls.Add(Me.cboTreatyNumber)
        Me.Frame3.Controls.Add(Me.txtName)
        Me.Frame3.Controls.Add(Me.txtIDReference)
        Me.Frame3.Controls.Add(Me.lblBranch)
        Me.Frame3.Controls.Add(Me.lblSubBranch)
        Me.Frame3.Controls.Add(Me.lblTreatyNumber)
        Me.Frame3.Controls.Add(Me.lblName)
        Me.Frame3.Controls.Add(Me.lblIDReference)
        Me.Frame3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame3.Location = New System.Drawing.Point(8, 12)
        Me.Frame3.Name = "Frame3"
        Me.Frame3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame3.Size = New System.Drawing.Size(585, 103)
        Me.Frame3.TabIndex = 16
        Me.Frame3.TabStop = False
        Me.Frame3.Text = "Details"
        '
        'cboSubBranch
        '
        Me.cboSubBranch.BackColor = System.Drawing.SystemColors.Window
        Me.cboSubBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSubBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSubBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSubBranch.Location = New System.Drawing.Point(418, 44)
        Me.cboSubBranch.Name = "cboSubBranch"
        Me.cboSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSubBranch.Size = New System.Drawing.Size(155, 21)
        Me.cboSubBranch.TabIndex = 22
        '
        'uctBranch
        '
        Me.uctBranch.BackColor = System.Drawing.SystemColors.Window
        Me.uctBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.uctBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.uctBranch.Location = New System.Drawing.Point(117, 44)
        Me.uctBranch.Name = "uctBranch"
        Me.uctBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.uctBranch.Size = New System.Drawing.Size(153, 21)
        Me.uctBranch.TabIndex = 21
        Me.uctBranch.Text = " "
        '
        'cboTreatyNumber
        '
        Me.cboTreatyNumber.DefaultItemId = 0
        Me.cboTreatyNumber.FirstItem = ""
        Me.cboTreatyNumber.ItemId = 0
        Me.cboTreatyNumber.ListIndex = -1
        Me.cboTreatyNumber.Location = New System.Drawing.Point(117, 70)
        Me.cboTreatyNumber.Name = "cboTreatyNumber"
        Me.cboTreatyNumber.PMLookupProductFamily = 9
        Me.cboTreatyNumber.SingleItemId = 0
        Me.cboTreatyNumber.Size = New System.Drawing.Size(207, 21)
        Me.cboTreatyNumber.Sorted = True
        Me.cboTreatyNumber.TabIndex = 25
        Me.cboTreatyNumber.TableName = ""
        Me.cboTreatyNumber.ToolTipText = ""
        Me.cboTreatyNumber.Visible = False
        Me.cboTreatyNumber.WhereClause = ""
        '
        'txtName
        '
        Me.txtName.AcceptsReturn = True
        Me.txtName.BackColor = System.Drawing.SystemColors.Window
        Me.txtName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtName.Location = New System.Drawing.Point(308, 18)
        Me.txtName.MaxLength = 0
        Me.txtName.Name = "txtName"
        Me.txtName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtName.Size = New System.Drawing.Size(265, 20)
        Me.txtName.TabIndex = 20
        '
        'txtIDReference
        '
        Me.txtIDReference.AcceptsReturn = True
        Me.txtIDReference.BackColor = System.Drawing.SystemColors.Window
        Me.txtIDReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtIDReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIDReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIDReference.Location = New System.Drawing.Point(117, 18)
        Me.txtIDReference.MaxLength = 20
        Me.txtIDReference.Name = "txtIDReference"
        Me.txtIDReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtIDReference.Size = New System.Drawing.Size(117, 20)
        Me.txtIDReference.TabIndex = 18
        '
        'lblBranch
        '
        Me.lblBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranch.Location = New System.Drawing.Point(10, 48)
        Me.lblBranch.Name = "lblBranch"
        Me.lblBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranch.Size = New System.Drawing.Size(89, 17)
        Me.lblBranch.TabIndex = 23
        Me.lblBranch.Text = "Branch:"
        '
        'lblSubBranch
        '
        Me.lblSubBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblSubBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSubBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubBranch.Location = New System.Drawing.Point(342, 48)
        Me.lblSubBranch.Name = "lblSubBranch"
        Me.lblSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSubBranch.Size = New System.Drawing.Size(85, 17)
        Me.lblSubBranch.TabIndex = 24
        Me.lblSubBranch.Text = "Sub-branch:"
        '
        'lblTreatyNumber
        '
        Me.lblTreatyNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblTreatyNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTreatyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTreatyNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTreatyNumber.Location = New System.Drawing.Point(10, 74)
        Me.lblTreatyNumber.Name = "lblTreatyNumber"
        Me.lblTreatyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTreatyNumber.Size = New System.Drawing.Size(113, 17)
        Me.lblTreatyNumber.TabIndex = 26
        Me.lblTreatyNumber.Text = "Treaty number:"
        Me.lblTreatyNumber.Visible = False
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.BackColor = System.Drawing.SystemColors.Control
        Me.lblName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblName.Location = New System.Drawing.Point(262, 22)
        Me.lblName.Name = "lblName"
        Me.lblName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblName.Size = New System.Drawing.Size(38, 13)
        Me.lblName.TabIndex = 19
        Me.lblName.Text = "Name:"
        '
        'lblIDReference
        '
        Me.lblIDReference.AutoSize = True
        Me.lblIDReference.BackColor = System.Drawing.SystemColors.Control
        Me.lblIDReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIDReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIDReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIDReference.Location = New System.Drawing.Point(10, 22)
        Me.lblIDReference.Name = "lblIDReference"
        Me.lblIDReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIDReference.Size = New System.Drawing.Size(35, 13)
        Me.lblIDReference.TabIndex = 17
        Me.lblIDReference.Text = "Code:"
        '
        'fraReinsurance
        '
        Me.fraReinsurance.BackColor = System.Drawing.SystemColors.Control
        Me.fraReinsurance.Controls.Add(Me.ChkIsRIBroker)
        Me.fraReinsurance.Controls.Add(Me.chkIsRetained)
        Me.fraReinsurance.Controls.Add(Me.cboReinsuranceType)
        Me.fraReinsurance.Controls.Add(Me.txtDefaultCommissionRate)
        Me.fraReinsurance.Controls.Add(Me.chkIsReInsuranceDebitCreditNo)
        Me.fraReinsurance.Controls.Add(Me.lblCommission)
        Me.fraReinsurance.Controls.Add(Me.lblReInsuranceType)
        Me.fraReinsurance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraReinsurance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraReinsurance.Location = New System.Drawing.Point(8, 248)
        Me.fraReinsurance.Name = "fraReinsurance"
        Me.fraReinsurance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraReinsurance.Size = New System.Drawing.Size(585, 93)
        Me.fraReinsurance.TabIndex = 35
        Me.fraReinsurance.TabStop = False
        Me.fraReinsurance.Text = "Re-Insurance"
        '
        'ChkIsRIBroker
        '
        Me.ChkIsRIBroker.BackColor = System.Drawing.SystemColors.Control
        Me.ChkIsRIBroker.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ChkIsRIBroker.Cursor = System.Windows.Forms.Cursors.Default
        Me.ChkIsRIBroker.Enabled = False
        Me.ChkIsRIBroker.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkIsRIBroker.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ChkIsRIBroker.Location = New System.Drawing.Point(471, 43)
        Me.ChkIsRIBroker.Name = "ChkIsRIBroker"
        Me.ChkIsRIBroker.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ChkIsRIBroker.Size = New System.Drawing.Size(96, 17)
        Me.ChkIsRIBroker.TabIndex = 53
        Me.ChkIsRIBroker.Text = "RI Broker"
        Me.ChkIsRIBroker.UseVisualStyleBackColor = False
        Me.ChkIsRIBroker.Visible = False
        '
        'chkIsRetained
        '
        Me.chkIsRetained.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsRetained.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsRetained.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsRetained.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsRetained.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsRetained.Location = New System.Drawing.Point(471, 27)
        Me.chkIsRetained.Name = "chkIsRetained"
        Me.chkIsRetained.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsRetained.Size = New System.Drawing.Size(96, 17)
        Me.chkIsRetained.TabIndex = 38
        Me.chkIsRetained.Text = "Is Retained?"
        Me.chkIsRetained.UseVisualStyleBackColor = False
        '
        'cboReinsuranceType
        '
        Me.cboReinsuranceType.DefaultItemId = 0
        Me.cboReinsuranceType.FirstItem = ""
        Me.cboReinsuranceType.ItemId = 0
        Me.cboReinsuranceType.ListIndex = -1
        Me.cboReinsuranceType.Location = New System.Drawing.Point(136, 24)
        Me.cboReinsuranceType.Name = "cboReinsuranceType"
        Me.cboReinsuranceType.PMLookupProductFamily = 9
        Me.cboReinsuranceType.SingleItemId = 0
        Me.cboReinsuranceType.Size = New System.Drawing.Size(153, 21)
        Me.cboReinsuranceType.Sorted = True
        Me.cboReinsuranceType.TabIndex = 37
        Me.cboReinsuranceType.TableName = ""
        Me.cboReinsuranceType.ToolTipText = ""
        Me.cboReinsuranceType.WhereClause = ""
        '
        'txtDefaultCommissionRate
        '
        Me.txtDefaultCommissionRate.AcceptsReturn = True
        Me.txtDefaultCommissionRate.BackColor = System.Drawing.SystemColors.Window
        Me.txtDefaultCommissionRate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDefaultCommissionRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDefaultCommissionRate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDefaultCommissionRate.Location = New System.Drawing.Point(136, 56)
        Me.txtDefaultCommissionRate.MaxLength = 0
        Me.txtDefaultCommissionRate.Name = "txtDefaultCommissionRate"
        Me.txtDefaultCommissionRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDefaultCommissionRate.Size = New System.Drawing.Size(153, 20)
        Me.txtDefaultCommissionRate.TabIndex = 40
        '
        'chkIsReInsuranceDebitCreditNo
        '
        Me.chkIsReInsuranceDebitCreditNo.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsReInsuranceDebitCreditNo.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsReInsuranceDebitCreditNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsReInsuranceDebitCreditNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsReInsuranceDebitCreditNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsReInsuranceDebitCreditNo.Location = New System.Drawing.Point(381, 58)
        Me.chkIsReInsuranceDebitCreditNo.Name = "chkIsReInsuranceDebitCreditNo"
        Me.chkIsReInsuranceDebitCreditNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsReInsuranceDebitCreditNo.Size = New System.Drawing.Size(186, 18)
        Me.chkIsReInsuranceDebitCreditNo.TabIndex = 41
        Me.chkIsReInsuranceDebitCreditNo.Text = "Re-insurance Debit Credit note?"
        Me.chkIsReInsuranceDebitCreditNo.UseVisualStyleBackColor = False
        '
        'lblCommission
        '
        Me.lblCommission.AutoSize = True
        Me.lblCommission.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommission.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommission.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCommission.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommission.Location = New System.Drawing.Point(14, 52)
        Me.lblCommission.Name = "lblCommission"
        Me.lblCommission.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommission.Size = New System.Drawing.Size(101, 13)
        Me.lblCommission.TabIndex = 39
        Me.lblCommission.Text = "Default commission:"
        '
        'lblReInsuranceType
        '
        Me.lblReInsuranceType.AutoSize = True
        Me.lblReInsuranceType.BackColor = System.Drawing.SystemColors.Control
        Me.lblReInsuranceType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReInsuranceType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReInsuranceType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReInsuranceType.Location = New System.Drawing.Point(14, 28)
        Me.lblReInsuranceType.Name = "lblReInsuranceType"
        Me.lblReInsuranceType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReInsuranceType.Size = New System.Drawing.Size(99, 13)
        Me.lblReInsuranceType.TabIndex = 36
        Me.lblReInsuranceType.Text = "Re-insurance type :"
        '
        'fraAppointment
        '
        Me.fraAppointment.BackColor = System.Drawing.SystemColors.Control
        Me.fraAppointment.Controls.Add(Me.cboReportIndicator)
        Me.fraAppointment.Controls.Add(Me.cboBinderIndicator)
        Me.fraAppointment.Controls.Add(Me.cboTermsOfPayment)
        Me.fraAppointment.Controls.Add(Me.cboCurrency)
        Me.fraAppointment.Controls.Add(Me.lblReportIndicator)
        Me.fraAppointment.Controls.Add(Me.lblBinderIndicator)
        Me.fraAppointment.Controls.Add(Me.lblTermsOfPayment)
        Me.fraAppointment.Controls.Add(Me.lblCurrency)
        Me.fraAppointment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAppointment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAppointment.Location = New System.Drawing.Point(8, 140)
        Me.fraAppointment.Name = "fraAppointment"
        Me.fraAppointment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAppointment.Size = New System.Drawing.Size(585, 79)
        Me.fraAppointment.TabIndex = 27
        Me.fraAppointment.TabStop = False
        Me.fraAppointment.Text = "Finance"
        '
        'cboReportIndicator
        '
        Me.cboReportIndicator.BackColor = System.Drawing.SystemColors.Window
        Me.cboReportIndicator.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboReportIndicator.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboReportIndicator.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboReportIndicator.Location = New System.Drawing.Point(416, 44)
        Me.cboReportIndicator.Name = "cboReportIndicator"
        Me.cboReportIndicator.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboReportIndicator.Size = New System.Drawing.Size(157, 21)
        Me.cboReportIndicator.TabIndex = 33
        '
        'cboBinderIndicator
        '
        Me.cboBinderIndicator.BackColor = System.Drawing.SystemColors.Window
        Me.cboBinderIndicator.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBinderIndicator.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBinderIndicator.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBinderIndicator.Location = New System.Drawing.Point(416, 18)
        Me.cboBinderIndicator.Name = "cboBinderIndicator"
        Me.cboBinderIndicator.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBinderIndicator.Size = New System.Drawing.Size(157, 21)
        Me.cboBinderIndicator.TabIndex = 29
        '
        'cboTermsOfPayment
        '
        Me.cboTermsOfPayment.BackColor = System.Drawing.SystemColors.Window
        Me.cboTermsOfPayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTermsOfPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTermsOfPayment.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTermsOfPayment.Location = New System.Drawing.Point(136, 44)
        Me.cboTermsOfPayment.Name = "cboTermsOfPayment"
        Me.cboTermsOfPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTermsOfPayment.Size = New System.Drawing.Size(153, 21)
        Me.cboTermsOfPayment.TabIndex = 32
        '
        'cboCurrency
        '
        Me.cboCurrency.CompanyId = 0
        Me.cboCurrency.CurrencyId = 0
        Me.cboCurrency.DefaultCurrencyId = 0
        Me.cboCurrency.FirstItem = ""
        Me.cboCurrency.ListIndex = -1
        Me.cboCurrency.Location = New System.Drawing.Point(136, 16)
        Me.cboCurrency.Name = "cboCurrency"
        Me.cboCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actAllCurrencies
        Me.cboCurrency.Size = New System.Drawing.Size(153, 21)
        Me.cboCurrency.TabIndex = 52
        Me.cboCurrency.ToolTipText = ""
        Me.cboCurrency.WhatsThisHelpID = 0
        '
        'lblReportIndicator
        '
        Me.lblReportIndicator.BackColor = System.Drawing.SystemColors.Control
        Me.lblReportIndicator.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReportIndicator.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReportIndicator.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReportIndicator.Location = New System.Drawing.Point(304, 47)
        Me.lblReportIndicator.Name = "lblReportIndicator"
        Me.lblReportIndicator.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReportIndicator.Size = New System.Drawing.Size(137, 17)
        Me.lblReportIndicator.TabIndex = 34
        Me.lblReportIndicator.Text = "Report indicator:"
        '
        'lblBinderIndicator
        '
        Me.lblBinderIndicator.BackColor = System.Drawing.SystemColors.Control
        Me.lblBinderIndicator.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBinderIndicator.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBinderIndicator.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBinderIndicator.Location = New System.Drawing.Point(304, 21)
        Me.lblBinderIndicator.Name = "lblBinderIndicator"
        Me.lblBinderIndicator.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBinderIndicator.Size = New System.Drawing.Size(105, 25)
        Me.lblBinderIndicator.TabIndex = 30
        Me.lblBinderIndicator.Text = "Binder indicator:"
        '
        'lblTermsOfPayment
        '
        Me.lblTermsOfPayment.BackColor = System.Drawing.SystemColors.Control
        Me.lblTermsOfPayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTermsOfPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTermsOfPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTermsOfPayment.Location = New System.Drawing.Point(12, 47)
        Me.lblTermsOfPayment.Name = "lblTermsOfPayment"
        Me.lblTermsOfPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTermsOfPayment.Size = New System.Drawing.Size(121, 17)
        Me.lblTermsOfPayment.TabIndex = 31
        Me.lblTermsOfPayment.Text = "Terms of payment:"
        '
        'lblCurrency
        '
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(12, 21)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(97, 17)
        Me.lblCurrency.TabIndex = 28
        Me.lblCurrency.Text = "Currency:"
        '
        '_cmdNext_0
        '
        Me._cmdNext_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_0.Location = New System.Drawing.Point(552, 356)
        Me._cmdNext_0.Name = "_cmdNext_0"
        Me._cmdNext_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_0.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_0.TabIndex = 45
        Me._cmdNext_0.Text = ">>"
        Me._cmdNext_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_0.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraAddress)
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdNext_1)
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdPrevious_0)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(632, 383)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "2 - Address"
        Me._tabMainTab_TabPage1.UseVisualStyleBackColor = True
        '
        'fraAddress
        '
        Me.fraAddress.BackColor = System.Drawing.SystemColors.Control
        Me.fraAddress.Controls.Add(Me.lvwAddresses)
        Me.fraAddress.Controls.Add(Me.cmdAddAd)
        Me.fraAddress.Controls.Add(Me.cmdDeleteAd)
        Me.fraAddress.Controls.Add(Me.cmdEditAd)
        Me.fraAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAddress.Location = New System.Drawing.Point(8, 4)
        Me.fraAddress.Name = "fraAddress"
        Me.fraAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAddress.Size = New System.Drawing.Size(585, 348)
        Me.fraAddress.TabIndex = 6
        Me.fraAddress.TabStop = False
        '
        'lvwAddresses
        '
        Me.lvwAddresses.BackColor = System.Drawing.SystemColors.Window
        Me.lvwAddresses.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwAddresses.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwAddresses_ColumnHeader_1, Me._lvwAddresses_ColumnHeader_2, Me._lvwAddresses_ColumnHeader_3, Me._lvwAddresses_ColumnHeader_4, Me._lvwAddresses_ColumnHeader_5, Me._lvwAddresses_ColumnHeader_6, Me._lvwAddresses_ColumnHeader_7})
        Me.lvwAddresses.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwAddresses.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwAddresses.LargeImageList = Me.ImageList2
        Me.lvwAddresses.Location = New System.Drawing.Point(8, 16)
        Me.lvwAddresses.Name = "lvwAddresses"
        Me.lvwAddresses.Size = New System.Drawing.Size(569, 293)
        Me.lvwAddresses.SmallImageList = Me.ImageList2
        Me.lvwAddresses.TabIndex = 7
        Me.lvwAddresses.UseCompatibleStateImageBehavior = False
        Me.lvwAddresses.View = System.Windows.Forms.View.Details
        '
        '_lvwAddresses_ColumnHeader_1
        '
        Me._lvwAddresses_ColumnHeader_1.Text = "Post Code"
        Me._lvwAddresses_ColumnHeader_1.Width = 67
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
        '_lvwAddresses_ColumnHeader_7
        '
        Me._lvwAddresses_ColumnHeader_7.Width = 0
        '
        'cmdAddAd
        '
        Me.cmdAddAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddAd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddAd.Location = New System.Drawing.Point(344, 316)
        Me.cmdAddAd.Name = "cmdAddAd"
        Me.cmdAddAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddAd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddAd.TabIndex = 8
        Me.cmdAddAd.Text = "&Add"
        Me.cmdAddAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddAd.UseVisualStyleBackColor = False
        '
        'cmdDeleteAd
        '
        Me.cmdDeleteAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteAd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteAd.Location = New System.Drawing.Point(504, 316)
        Me.cmdDeleteAd.Name = "cmdDeleteAd"
        Me.cmdDeleteAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteAd.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteAd.TabIndex = 10
        Me.cmdDeleteAd.Text = "&Delete"
        Me.cmdDeleteAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteAd.UseVisualStyleBackColor = False
        '
        'cmdEditAd
        '
        Me.cmdEditAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditAd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditAd.Location = New System.Drawing.Point(424, 316)
        Me.cmdEditAd.Name = "cmdEditAd"
        Me.cmdEditAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditAd.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditAd.TabIndex = 9
        Me.cmdEditAd.Text = "&Edit"
        Me.cmdEditAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditAd.UseVisualStyleBackColor = False
        '
        '_cmdNext_1
        '
        Me._cmdNext_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_1.Location = New System.Drawing.Point(552, 356)
        Me._cmdNext_1.Name = "_cmdNext_1"
        Me._cmdNext_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_1.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_1.TabIndex = 46
        Me._cmdNext_1.Text = ">>"
        Me._cmdNext_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_1.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_0
        '
        Me._cmdPrevious_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_0.Location = New System.Drawing.Point(8, 356)
        Me._cmdPrevious_0.Name = "_cmdPrevious_0"
        Me._cmdPrevious_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_0.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_0.TabIndex = 42
        Me._cmdPrevious_0.Text = "<<"
        Me._cmdPrevious_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_0.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraContact)
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdPrevious_1)
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdNext_2)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(632, 383)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "3 - Contacts"
        Me._tabMainTab_TabPage2.UseVisualStyleBackColor = True
        '
        'fraContact
        '
        Me.fraContact.BackColor = System.Drawing.SystemColors.Control
        Me.fraContact.Controls.Add(Me.cmdEditCon)
        Me.fraContact.Controls.Add(Me.cmdDeleteCon)
        Me.fraContact.Controls.Add(Me.cmdAddCon)
        Me.fraContact.Controls.Add(Me.lvwContacts)
        Me.fraContact.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraContact.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraContact.Location = New System.Drawing.Point(8, 4)
        Me.fraContact.Name = "fraContact"
        Me.fraContact.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraContact.Size = New System.Drawing.Size(585, 348)
        Me.fraContact.TabIndex = 11
        Me.fraContact.TabStop = False
        '
        'cmdEditCon
        '
        Me.cmdEditCon.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditCon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditCon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditCon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditCon.Location = New System.Drawing.Point(424, 316)
        Me.cmdEditCon.Name = "cmdEditCon"
        Me.cmdEditCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditCon.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditCon.TabIndex = 14
        Me.cmdEditCon.Text = "&Edit"
        Me.cmdEditCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditCon.UseVisualStyleBackColor = False
        '
        'cmdDeleteCon
        '
        Me.cmdDeleteCon.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteCon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteCon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteCon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteCon.Location = New System.Drawing.Point(504, 316)
        Me.cmdDeleteCon.Name = "cmdDeleteCon"
        Me.cmdDeleteCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteCon.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteCon.TabIndex = 15
        Me.cmdDeleteCon.Text = "&Delete"
        Me.cmdDeleteCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteCon.UseVisualStyleBackColor = False
        '
        'cmdAddCon
        '
        Me.cmdAddCon.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddCon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddCon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddCon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddCon.Location = New System.Drawing.Point(344, 316)
        Me.cmdAddCon.Name = "cmdAddCon"
        Me.cmdAddCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddCon.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddCon.TabIndex = 13
        Me.cmdAddCon.Text = "&Add"
        Me.cmdAddCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddCon.UseVisualStyleBackColor = False
        '
        'lvwContacts
        '
        Me.lvwContacts.BackColor = System.Drawing.SystemColors.Window
        Me.lvwContacts.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwContacts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwContacts_ColumnHeader_1, Me._lvwContacts_ColumnHeader_2, Me._lvwContacts_ColumnHeader_3, Me._lvwContacts_ColumnHeader_4, Me._lvwContacts_ColumnHeader_5})
        Me.lvwContacts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwContacts.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwContacts.LargeImageList = Me.ImageList2
        Me.lvwContacts.Location = New System.Drawing.Point(8, 16)
        Me.lvwContacts.Name = "lvwContacts"
        Me.lvwContacts.Size = New System.Drawing.Size(569, 294)
        Me.lvwContacts.SmallImageList = Me.ImageList2
        Me.lvwContacts.TabIndex = 12
        Me.lvwContacts.UseCompatibleStateImageBehavior = False
        Me.lvwContacts.View = System.Windows.Forms.View.Details
        '
        '_lvwContacts_ColumnHeader_1
        '
        Me._lvwContacts_ColumnHeader_1.Text = "Area Code"
        Me._lvwContacts_ColumnHeader_1.Width = 67
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
        '_cmdPrevious_1
        '
        Me._cmdPrevious_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_1.Location = New System.Drawing.Point(8, 356)
        Me._cmdPrevious_1.Name = "_cmdPrevious_1"
        Me._cmdPrevious_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_1.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_1.TabIndex = 43
        Me._cmdPrevious_1.Text = "<<"
        Me._cmdPrevious_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_1.UseVisualStyleBackColor = False
        '
        '_cmdNext_2
        '
        Me._cmdNext_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_2.Location = New System.Drawing.Point(552, 356)
        Me._cmdNext_2.Name = "_cmdNext_2"
        Me._cmdNext_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_2.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_2.TabIndex = 47
        Me._cmdNext_2.Text = ">>"
        Me._cmdNext_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_2.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage3
        '
        Me._tabMainTab_TabPage3.Controls.Add(Me._cmdPrevious_2)
        Me._tabMainTab_TabPage3.Controls.Add(Me.fraTax)
        Me._tabMainTab_TabPage3.Controls.Add(Me._cmdNext_3)
        Me._tabMainTab_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage3.Name = "_tabMainTab_TabPage3"
        Me._tabMainTab_TabPage3.Size = New System.Drawing.Size(632, 383)
        Me._tabMainTab_TabPage3.TabIndex = 3
        Me._tabMainTab_TabPage3.Text = "4 - Tax"
        Me._tabMainTab_TabPage3.UseVisualStyleBackColor = True
        '
        '_cmdPrevious_2
        '
        Me._cmdPrevious_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_2.Location = New System.Drawing.Point(8, 356)
        Me._cmdPrevious_2.Name = "_cmdPrevious_2"
        Me._cmdPrevious_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_2.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_2.TabIndex = 44
        Me._cmdPrevious_2.Text = "<<"
        Me._cmdPrevious_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_2.UseVisualStyleBackColor = False
        '
        'fraTax
        '
        Me.fraTax.BackColor = System.Drawing.SystemColors.Control
        Me.fraTax.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.fraTax.Controls.Add(Me.cboTaxGroupID)
        Me.fraTax.Controls.Add(Me.lblTaxGroupID)
        Me.fraTax.Controls.Add(Me.uctPartyTax1)
        Me.fraTax.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.fraTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTax.Location = New System.Drawing.Point(8, 4)
        Me.fraTax.Name = "fraTax"
        Me.fraTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTax.Size = New System.Drawing.Size(589, 177)
        Me.fraTax.TabIndex = 2
        Me.fraTax.TabStop = False
        Me.fraTax.Text = "Party Tax"
        '
        'cboTaxGroupID
        '
        Me.cboTaxGroupID.DefaultItemId = 0
        Me.cboTaxGroupID.FirstItem = "(none)"
        Me.cboTaxGroupID.ItemId = 0
        Me.cboTaxGroupID.ListIndex = -1
        Me.cboTaxGroupID.Location = New System.Drawing.Point(141, 128)
        Me.cboTaxGroupID.Name = "cboTaxGroupID"
        Me.cboTaxGroupID.PMLookupProductFamily = 1
        Me.cboTaxGroupID.SingleItemId = 0
        Me.cboTaxGroupID.Size = New System.Drawing.Size(157, 21)
        Me.cboTaxGroupID.Sorted = True
        Me.cboTaxGroupID.TabIndex = 5
        Me.cboTaxGroupID.TableName = "Tax_Group"
        Me.cboTaxGroupID.ToolTipText = ""
        Me.cboTaxGroupID.WhereClause = ""
        '
        'lblTaxGroupID
        '
        Me.lblTaxGroupID.AutoSize = True
        Me.lblTaxGroupID.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxGroupID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxGroupID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxGroupID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxGroupID.Location = New System.Drawing.Point(83, 132)
        Me.lblTaxGroupID.Name = "lblTaxGroupID"
        Me.lblTaxGroupID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxGroupID.Size = New System.Drawing.Size(60, 13)
        Me.lblTaxGroupID.TabIndex = 4
        Me.lblTaxGroupID.Text = "Tax Group:"
        Me.lblTaxGroupID.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'uctPartyTax1
        '
        Me.uctPartyTax1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPartyTax1.FrameOn = False
        Me.uctPartyTax1.IsDomiciledForTax = False
        Me.uctPartyTax1.Location = New System.Drawing.Point(6, 13)
        Me.uctPartyTax1.Name = "uctPartyTax1"
        Me.uctPartyTax1.Size = New System.Drawing.Size(576, 158)
        Me.uctPartyTax1.SizeHorizontal = False
        Me.uctPartyTax1.TabIndex = 3
        Me.uctPartyTax1.TaxExempt = False
        Me.uctPartyTax1.TaxNumber = ""
        Me.uctPartyTax1.TaxPercentage = 0
        '
        '_cmdNext_3
        '
        Me._cmdNext_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_3.Location = New System.Drawing.Point(552, 356)
        Me._cmdNext_3.Name = "_cmdNext_3"
        Me._cmdNext_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_3.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_3.TabIndex = 55
        Me._cmdNext_3.Text = ">>"
        Me._cmdNext_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_3.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage4
        '
        Me._tabMainTab_TabPage4.Controls.Add(Me._cmdNext_4)
        Me._tabMainTab_TabPage4.Controls.Add(Me._cmdPrevious_3)
        Me._tabMainTab_TabPage4.Controls.Add(Me.uctPartyBankControl1)
        Me._tabMainTab_TabPage4.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage4.Name = "_tabMainTab_TabPage4"
        Me._tabMainTab_TabPage4.Size = New System.Drawing.Size(632, 383)
        Me._tabMainTab_TabPage4.TabIndex = 4
        Me._tabMainTab_TabPage4.Text = "5 - Bank"
        Me._tabMainTab_TabPage4.UseVisualStyleBackColor = True
        '
        '_cmdPrevious_3
        '
        Me._cmdPrevious_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_3.Location = New System.Drawing.Point(8, 356)
        Me._cmdPrevious_3.Name = "_cmdPrevious_3"
        Me._cmdPrevious_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_3.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_3.TabIndex = 54
        Me._cmdPrevious_3.Text = "<<"
        Me._cmdPrevious_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_3.UseVisualStyleBackColor = False
        '
        'uctPartyBankControl1
        '
        Me.uctPartyBankControl1.AccountId = Nothing
        Me.uctPartyBankControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPartyBankControl1.IsExternalCreditCardProcessing = False
        Me.uctPartyBankControl1.Location = New System.Drawing.Point(0, 1)
        Me.uctPartyBankControl1.Name = "uctPartyBankControl1"
        Me.uctPartyBankControl1.PartyBankDetails = Nothing
        Me.uctPartyBankControl1.PartyBankHistory = Nothing
        Me.uctPartyBankControl1.PartyCnt = Nothing
        Me.uctPartyBankControl1.Size = New System.Drawing.Size(633, 355)
        Me.uctPartyBankControl1.TabIndex = 56
        '
        '_tabMainTab_TabPage5
        '
        Me._tabMainTab_TabPage5.Controls.Add(Me._cmdPrevious_4)
        Me._tabMainTab_TabPage5.Controls.Add(Me.fraCertYears)
        Me._tabMainTab_TabPage5.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage5.Name = "_tabMainTab_TabPage5"
        Me._tabMainTab_TabPage5.Padding = New System.Windows.Forms.Padding(3)
        Me._tabMainTab_TabPage5.Size = New System.Drawing.Size(632, 383)
        Me._tabMainTab_TabPage5.TabIndex = 5
        Me._tabMainTab_TabPage5.Text = "6 - Certificate Year"
        Me._tabMainTab_TabPage5.UseVisualStyleBackColor = True
        '
        'fraCertYears
        '
        Me.fraCertYears.BackColor = System.Drawing.SystemColors.Control
        Me.fraCertYears.Controls.Add(Me.cmdAddCertYear)
        Me.fraCertYears.Controls.Add(Me.cmdDelCertYear)
        Me.fraCertYears.Controls.Add(Me.cmdEditCertYear)
        Me.fraCertYears.Controls.Add(Me.lvwCertYears)
        Me.fraCertYears.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCertYears.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraCertYears.Location = New System.Drawing.Point(6, 6)
        Me.fraCertYears.Name = "fraCertYears"
        Me.fraCertYears.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraCertYears.Size = New System.Drawing.Size(618, 300)
        Me.fraCertYears.TabIndex = 102
        Me.fraCertYears.TabStop = False
        '
        'cmdAddCertYear
        '
        Me.cmdAddCertYear.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddCertYear.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddCertYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddCertYear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddCertYear.Location = New System.Drawing.Point(8, 248)
        Me.cmdAddCertYear.Name = "cmdAddCertYear"
        Me.cmdAddCertYear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddCertYear.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddCertYear.TabIndex = 28
        Me.cmdAddCertYear.Text = "&Add"
        Me.cmdAddCertYear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddCertYear.UseVisualStyleBackColor = False
        '
        'cmdDelCertYear
        '
        Me.cmdDelCertYear.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelCertYear.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelCertYear.Enabled = False
        Me.cmdDelCertYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelCertYear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelCertYear.Location = New System.Drawing.Point(88, 248)
        Me.cmdDelCertYear.Name = "cmdDelCertYear"
        Me.cmdDelCertYear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelCertYear.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelCertYear.TabIndex = 29
        Me.cmdDelCertYear.Text = "&Delete"
        Me.cmdDelCertYear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelCertYear.UseVisualStyleBackColor = False
        '
        'cmdEditCertYear
        '
        Me.cmdEditCertYear.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditCertYear.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditCertYear.Enabled = False
        Me.cmdEditCertYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditCertYear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditCertYear.Location = New System.Drawing.Point(168, 248)
        Me.cmdEditCertYear.Name = "cmdEditCertYear"
        Me.cmdEditCertYear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditCertYear.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditCertYear.TabIndex = 30
        Me.cmdEditCertYear.Text = "&Edit"
        Me.cmdEditCertYear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditCertYear.UseVisualStyleBackColor = False
        '
        'lvwCertYears
        '
        Me.lvwCertYears.BackColor = System.Drawing.SystemColors.Window
        Me.lvwCertYears.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwCertYears_ColumnHeader_1, Me._lvwCertYears_ColumnHeader_2, Me._lvwCertYears_ColumnHeader_3, Me._lvwCertYears_ColumnHeader_4, Me._lvwCertYears_ColumnHeader_5})
        Me.lvwCertYears.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwCertYears.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwCertYears.LargeImageList = Me.ImageList2
        Me.lvwCertYears.Location = New System.Drawing.Point(6, 19)
        Me.lvwCertYears.Name = "lvwCertYears"
        Me.lvwCertYears.Size = New System.Drawing.Size(573, 225)
        Me.lvwCertYears.TabIndex = 27
        Me.lvwCertYears.UseCompatibleStateImageBehavior = False
        Me.lvwCertYears.View = System.Windows.Forms.View.Details
        '
        '_lvwCertYears_ColumnHeader_1
        '
        Me._lvwCertYears_ColumnHeader_1.Tag = ""
        Me._lvwCertYears_ColumnHeader_1.Text = "Code"
        Me._lvwCertYears_ColumnHeader_1.Width = 97
        '
        '_lvwCertYears_ColumnHeader_2
        '
        Me._lvwCertYears_ColumnHeader_2.Tag = ""
        Me._lvwCertYears_ColumnHeader_2.Text = "Description"
        Me._lvwCertYears_ColumnHeader_2.Width = 97
        '
        '_lvwCertYears_ColumnHeader_3
        '
        Me._lvwCertYears_ColumnHeader_3.Tag = ""
        Me._lvwCertYears_ColumnHeader_3.Text = "Start Date"
        Me._lvwCertYears_ColumnHeader_3.Width = 97
        '
        '_lvwCertYears_ColumnHeader_4
        '
        Me._lvwCertYears_ColumnHeader_4.Tag = ""
        Me._lvwCertYears_ColumnHeader_4.Text = "End Date"
        Me._lvwCertYears_ColumnHeader_4.Width = 97
        '
        '_lvwCertYears_ColumnHeader_5
        '
        Me._lvwCertYears_ColumnHeader_5.Text = ""
        Me._lvwCertYears_ColumnHeader_5.Width = 0
        '
        '_cmdPrevious_4
        '
        Me._cmdPrevious_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_4.Location = New System.Drawing.Point(6, 332)
        Me._cmdPrevious_4.Name = "_cmdPrevious_4"
        Me._cmdPrevious_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_4.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_4.TabIndex = 103
        Me._cmdPrevious_4.Text = "<<"
        Me._cmdPrevious_4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_4.UseVisualStyleBackColor = False
        '
        '_cmdNext_4
        '
        Me._cmdNext_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_4.Location = New System.Drawing.Point(591, 356)
        Me._cmdNext_4.Name = "_cmdNext_4"
        Me._cmdNext_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_4.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_4.TabIndex = 57
        Me._cmdNext_4.Text = ">>"
        Me._cmdNext_4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_4.UseVisualStyleBackColor = False
        '
        'frmReinsurer
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(647, 472)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.Toolbar1)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmReinsurer"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Reinsurer"
        Me.Toolbar1.ResumeLayout(False)
        Me.Toolbar1.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Frame3.ResumeLayout(False)
        Me.Frame3.PerformLayout()
        Me.fraReinsurance.ResumeLayout(False)
        Me.fraReinsurance.PerformLayout()
        Me.fraAppointment.ResumeLayout(False)
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me.fraAddress.ResumeLayout(False)
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me.fraContact.ResumeLayout(False)
        Me._tabMainTab_TabPage3.ResumeLayout(False)
        Me.fraTax.ResumeLayout(False)
        Me.fraTax.PerformLayout()
        Me._tabMainTab_TabPage4.ResumeLayout(False)
        Me._tabMainTab_TabPage5.ResumeLayout(False)
        Me.fraCertYears.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializecmdPrevious()
        Me.cmdPrevious(3) = _cmdPrevious_3
        Me.cmdPrevious(2) = _cmdPrevious_2
        Me.cmdPrevious(1) = _cmdPrevious_1
        Me.cmdPrevious(0) = _cmdPrevious_0
    End Sub
    Sub InitializecmdNext()
        Me.cmdNext(3) = _cmdNext_3
        Me.cmdNext(2) = _cmdNext_2
        Me.cmdNext(1) = _cmdNext_1
        Me.cmdNext(0) = _cmdNext_0
    End Sub
    Public WithEvents fraTax As System.Windows.Forms.GroupBox
    Public WithEvents uctPartyTax1 As uctPartyTaxControl.uctPartyTax
    Public WithEvents chkIsReInsuranceDebitCreditNo As System.Windows.Forms.CheckBox
    Public WithEvents cboTaxGroupID As PMLookupControl.cboPMLookup
    Public WithEvents lblTaxGroupID As System.Windows.Forms.Label
    Friend WithEvents _tabMainTab_TabPage5 As System.Windows.Forms.TabPage
    Public WithEvents fraCertYears As System.Windows.Forms.GroupBox
    Public WithEvents cmdAddCertYear As System.Windows.Forms.Button
    Public WithEvents cmdDelCertYear As System.Windows.Forms.Button
    Public WithEvents cmdEditCertYear As System.Windows.Forms.Button
    Public WithEvents lvwCertYears As System.Windows.Forms.ListView
    Private WithEvents _lvwCertYears_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCertYears_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCertYears_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCertYears_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwCertYears_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Private WithEvents _cmdNext_4 As System.Windows.Forms.Button
    Private WithEvents _cmdPrevious_4 As System.Windows.Forms.Button
#End Region 
End Class
