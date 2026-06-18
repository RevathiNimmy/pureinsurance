<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDetails
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializecmdPrevious()
		InitializecmdNext()
		tabMainTabPreviousTab = tabMainTab.SelectedIndex
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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblRegNo2 As System.Windows.Forms.Label
	Public WithEvents lblRegNo1 As System.Windows.Forms.Label
	Public WithEvents lblCode As System.Windows.Forms.Label
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents imgImage As System.Windows.Forms.PictureBox
	Public WithEvents lblBaseCurrency As System.Windows.Forms.Label
	Public WithEvents txtRegNo2 As System.Windows.Forms.TextBox
	Public WithEvents txtRegNo1 As System.Windows.Forms.TextBox
	Public WithEvents txtCode As System.Windows.Forms.TextBox
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents chkUnderwritingBranch As System.Windows.Forms.CheckBox
	Public WithEvents cboCurrency As UserControls.CurrencyLookup
	Private WithEvents _cmdNext_0 As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Private WithEvents _cmdPrevious_0 As System.Windows.Forms.Button
	Private WithEvents _cmdNext_1 As System.Windows.Forms.Button
	Public WithEvents txtFaxExtension As System.Windows.Forms.TextBox
	Public WithEvents txtPhoneExtension As System.Windows.Forms.TextBox
	Public WithEvents txtAddress3 As System.Windows.Forms.TextBox
	Public WithEvents txtAddress1 As System.Windows.Forms.TextBox
	Public WithEvents txtAddress2 As System.Windows.Forms.TextBox
	Public WithEvents txtAddress4 As System.Windows.Forms.TextBox
	Public WithEvents txtPostalCode As System.Windows.Forms.TextBox
	Public WithEvents cmbCountry As System.Windows.Forms.ComboBox
	Public WithEvents txtPhoneAreaCode As System.Windows.Forms.TextBox
	Public WithEvents txtPhoneNumber As System.Windows.Forms.TextBox
	Public WithEvents txtFaxAreaCode As System.Windows.Forms.TextBox
	Public WithEvents txtFaxNumber As System.Windows.Forms.TextBox
	Public WithEvents lblExtension As System.Windows.Forms.Label
	Public WithEvents lblAddress1 As System.Windows.Forms.Label
	Public WithEvents lblAddress2 As System.Windows.Forms.Label
	Public WithEvents lblAddress3 As System.Windows.Forms.Label
	Public WithEvents lblAddress4 As System.Windows.Forms.Label
	Public WithEvents lblPostalCode As System.Windows.Forms.Label
	Public WithEvents lblCountry As System.Windows.Forms.Label
	Public WithEvents lblPhone As System.Windows.Forms.Label
	Public WithEvents lblFax As System.Windows.Forms.Label
	Public WithEvents lblAreaCode As System.Windows.Forms.Label
	Public WithEvents lblNumber As System.Windows.Forms.Label
	Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
	Private WithEvents _cmdPrevious_1 As System.Windows.Forms.Button
	Private WithEvents _cmdNext_2 As System.Windows.Forms.Button
	Public WithEvents txtDefaultIndicator As System.Windows.Forms.TextBox
	Public WithEvents txtPMSourceNumber As System.Windows.Forms.TextBox
	Public WithEvents txtUserLicenceId As System.Windows.Forms.TextBox
	Public WithEvents txtBrokerABIId As System.Windows.Forms.TextBox
	Public WithEvents txtSenderMailboxId As System.Windows.Forms.TextBox
	Public WithEvents txtVatNo As System.Windows.Forms.TextBox
	Public WithEvents txtEmail As System.Windows.Forms.TextBox
	Public WithEvents lblDefaultIndicator As System.Windows.Forms.Label
	Public WithEvents lblPMSourceNumber As System.Windows.Forms.Label
	Public WithEvents lblUserLicenceId As System.Windows.Forms.Label
	Public WithEvents lblBrokerABIId As System.Windows.Forms.Label
	Public WithEvents lblSenderMailboxId As System.Windows.Forms.Label
	Public WithEvents lblVatNo As System.Windows.Forms.Label
	Public WithEvents lblEmail As System.Windows.Forms.Label
	Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
	Private WithEvents _cmdPrevious_2 As System.Windows.Forms.Button
	Private WithEvents _cmdNext_3 As System.Windows.Forms.Button
	Public WithEvents cmdRates As System.Windows.Forms.Button
	Public WithEvents uctCurrencies As uctPickList.PickList
	Private WithEvents _tabMainTab_TabPage3 As System.Windows.Forms.TabPage
	Private WithEvents _cmdPrevious_3 As System.Windows.Forms.Button
	Public WithEvents cboFSABankType As System.Windows.Forms.ComboBox
	Public WithEvents txtStaffWording As System.Windows.Forms.TextBox
	Public WithEvents cboCompanyCategory As System.Windows.Forms.ComboBox
	Public WithEvents lblFSABankType As System.Windows.Forms.Label
	Public WithEvents lblCompanyCategory As System.Windows.Forms.Label
	Public WithEvents lblStaffWording As System.Windows.Forms.Label
	Private WithEvents _tabMainTab_TabPage4 As System.Windows.Forms.TabPage
	Public WithEvents lblClosedBranch As System.Windows.Forms.Label
	Public WithEvents chkAllowTempMTA As System.Windows.Forms.CheckBox
	Public WithEvents chkAllowPermMTA As System.Windows.Forms.CheckBox
	Public WithEvents chkAllowReports As System.Windows.Forms.CheckBox
	Public WithEvents chkAllowClaims As System.Windows.Forms.CheckBox
	Public WithEvents chkAllowAccounts As System.Windows.Forms.CheckBox
	Private WithEvents _tabMainTab_TabPage5 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public cmdNext(3) As System.Windows.Forms.Button
	Public cmdPrevious(3) As System.Windows.Forms.Button
	Private WithEvents listBoxComboBoxHelper1 As Artinsoft.VB6.Gui.ListControlHelper
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDetails))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblRegNo2 = New System.Windows.Forms.Label
        Me.lblRegNo1 = New System.Windows.Forms.Label
        Me.lblCode = New System.Windows.Forms.Label
        Me.lblDescription = New System.Windows.Forms.Label
        Me.imgImage = New System.Windows.Forms.PictureBox
        Me.lblBaseCurrency = New System.Windows.Forms.Label
        Me.txtRegNo2 = New System.Windows.Forms.TextBox
        Me.txtRegNo1 = New System.Windows.Forms.TextBox
        Me.txtCode = New System.Windows.Forms.TextBox
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.chkUnderwritingBranch = New System.Windows.Forms.CheckBox
        Me.cboCurrency = New UserControls.CurrencyLookup
        Me._cmdNext_0 = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me._cmdPrevious_0 = New System.Windows.Forms.Button
        Me._cmdNext_1 = New System.Windows.Forms.Button
        Me.txtFaxExtension = New System.Windows.Forms.TextBox
        Me.txtPhoneExtension = New System.Windows.Forms.TextBox
        Me.txtAddress3 = New System.Windows.Forms.TextBox
        Me.txtAddress1 = New System.Windows.Forms.TextBox
        Me.txtAddress2 = New System.Windows.Forms.TextBox
        Me.txtAddress4 = New System.Windows.Forms.TextBox
        Me.txtPostalCode = New System.Windows.Forms.TextBox
        Me.cmbCountry = New System.Windows.Forms.ComboBox
        Me.txtPhoneAreaCode = New System.Windows.Forms.TextBox
        Me.txtPhoneNumber = New System.Windows.Forms.TextBox
        Me.txtFaxAreaCode = New System.Windows.Forms.TextBox
        Me.txtFaxNumber = New System.Windows.Forms.TextBox
        Me.lblExtension = New System.Windows.Forms.Label
        Me.lblAddress1 = New System.Windows.Forms.Label
        Me.lblAddress2 = New System.Windows.Forms.Label
        Me.lblAddress3 = New System.Windows.Forms.Label
        Me.lblAddress4 = New System.Windows.Forms.Label
        Me.lblPostalCode = New System.Windows.Forms.Label
        Me.lblCountry = New System.Windows.Forms.Label
        Me.lblPhone = New System.Windows.Forms.Label
        Me.lblFax = New System.Windows.Forms.Label
        Me.lblAreaCode = New System.Windows.Forms.Label
        Me.lblNumber = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage
        Me._cmdPrevious_1 = New System.Windows.Forms.Button
        Me._cmdNext_2 = New System.Windows.Forms.Button
        Me.txtDefaultIndicator = New System.Windows.Forms.TextBox
        Me.txtPMSourceNumber = New System.Windows.Forms.TextBox
        Me.txtUserLicenceId = New System.Windows.Forms.TextBox
        Me.txtBrokerABIId = New System.Windows.Forms.TextBox
        Me.txtSenderMailboxId = New System.Windows.Forms.TextBox
        Me.txtVatNo = New System.Windows.Forms.TextBox
        Me.txtEmail = New System.Windows.Forms.TextBox
        Me.lblDefaultIndicator = New System.Windows.Forms.Label
        Me.lblPMSourceNumber = New System.Windows.Forms.Label
        Me.lblUserLicenceId = New System.Windows.Forms.Label
        Me.lblBrokerABIId = New System.Windows.Forms.Label
        Me.lblSenderMailboxId = New System.Windows.Forms.Label
        Me.lblVatNo = New System.Windows.Forms.Label
        Me.lblEmail = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage3 = New System.Windows.Forms.TabPage
        Me._cmdPrevious_2 = New System.Windows.Forms.Button
        Me._cmdNext_3 = New System.Windows.Forms.Button
        Me.cmdRates = New System.Windows.Forms.Button
        Me.uctCurrencies = New uctPickList.PickList
        Me._tabMainTab_TabPage4 = New System.Windows.Forms.TabPage
        Me._cmdPrevious_3 = New System.Windows.Forms.Button
        Me.cboFSABankType = New System.Windows.Forms.ComboBox
        Me.txtStaffWording = New System.Windows.Forms.TextBox
        Me.cboCompanyCategory = New System.Windows.Forms.ComboBox
        Me.lblFSABankType = New System.Windows.Forms.Label
        Me.lblCompanyCategory = New System.Windows.Forms.Label
        Me.lblStaffWording = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage5 = New System.Windows.Forms.TabPage
        Me.lblClosedBranch = New System.Windows.Forms.Label
        Me.chkAllowTempMTA = New System.Windows.Forms.CheckBox
        Me.chkAllowPermMTA = New System.Windows.Forms.CheckBox
        Me.chkAllowReports = New System.Windows.Forms.CheckBox
        Me.chkAllowClaims = New System.Windows.Forms.CheckBox
        Me.chkAllowAccounts = New System.Windows.Forms.CheckBox
        Me.listBoxComboBoxHelper1 = New Artinsoft.VB6.Gui.ListControlHelper(Me.components)
        Me.HelpProvider1 = New System.Windows.Forms.HelpProvider
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.imgImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me._tabMainTab_TabPage3.SuspendLayout()
        Me._tabMainTab_TabPage4.SuspendLayout()
        Me._tabMainTab_TabPage5.SuspendLayout()
        CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(509, 320)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 40
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(432, 320)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 39
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
        Me.tabMainTab.ItemSize = New System.Drawing.Size(94, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(578, 309)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblRegNo2)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblRegNo1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.imgImage)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblBaseCurrency)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtRegNo2)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtRegNo1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkUnderwritingBranch)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboCurrency)
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdNext_0)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(570, 283)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - General"
        '
        'lblRegNo2
        '
        Me.lblRegNo2.BackColor = System.Drawing.SystemColors.Control
        Me.lblRegNo2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRegNo2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRegNo2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRegNo2.Location = New System.Drawing.Point(16, 139)
        Me.lblRegNo2.Name = "lblRegNo2"
        Me.lblRegNo2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRegNo2.Size = New System.Drawing.Size(113, 17)
        Me.lblRegNo2.TabIndex = 44
        Me.lblRegNo2.Text = "Registration No. 2:"
        '
        'lblRegNo1
        '
        Me.lblRegNo1.BackColor = System.Drawing.SystemColors.Control
        Me.lblRegNo1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRegNo1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRegNo1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRegNo1.Location = New System.Drawing.Point(16, 107)
        Me.lblRegNo1.Name = "lblRegNo1"
        Me.lblRegNo1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRegNo1.Size = New System.Drawing.Size(113, 17)
        Me.lblRegNo1.TabIndex = 43
        Me.lblRegNo1.Text = "Registration No. 1:"
        '
        'lblCode
        '
        Me.lblCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCode.Location = New System.Drawing.Point(16, 43)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCode.Size = New System.Drawing.Size(113, 17)
        Me.lblCode.TabIndex = 41
        Me.lblCode.Text = "Short Name:"
        '
        'lblDescription
        '
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(16, 75)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(113, 17)
        Me.lblDescription.TabIndex = 42
        Me.lblDescription.Text = "Name:"
        '
        'imgImage
        '
        Me.imgImage.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgImage.Image = CType(resources.GetObject("imgImage.Image"), System.Drawing.Image)
        Me.imgImage.Location = New System.Drawing.Point(448, 24)
        Me.imgImage.Name = "imgImage"
        Me.imgImage.Size = New System.Drawing.Size(32, 32)
        Me.imgImage.TabIndex = 45
        Me.imgImage.TabStop = False
        '
        'lblBaseCurrency
        '
        Me.lblBaseCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblBaseCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBaseCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBaseCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBaseCurrency.Location = New System.Drawing.Point(16, 170)
        Me.lblBaseCurrency.Name = "lblBaseCurrency"
        Me.lblBaseCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBaseCurrency.Size = New System.Drawing.Size(105, 17)
        Me.lblBaseCurrency.TabIndex = 56
        Me.lblBaseCurrency.Text = "Base Currency:"
        '
        'txtRegNo2
        '
        Me.txtRegNo2.AcceptsReturn = True
        Me.txtRegNo2.BackColor = System.Drawing.SystemColors.Window
        Me.txtRegNo2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRegNo2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRegNo2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRegNo2.Location = New System.Drawing.Point(144, 136)
        Me.txtRegNo2.MaxLength = 0
        Me.txtRegNo2.Name = "txtRegNo2"
        Me.txtRegNo2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRegNo2.Size = New System.Drawing.Size(153, 21)
        Me.txtRegNo2.TabIndex = 4
        '
        'txtRegNo1
        '
        Me.txtRegNo1.AcceptsReturn = True
        Me.txtRegNo1.BackColor = System.Drawing.SystemColors.Window
        Me.txtRegNo1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRegNo1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRegNo1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRegNo1.Location = New System.Drawing.Point(144, 104)
        Me.txtRegNo1.MaxLength = 0
        Me.txtRegNo1.Name = "txtRegNo1"
        Me.txtRegNo1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRegNo1.Size = New System.Drawing.Size(153, 21)
        Me.txtRegNo1.TabIndex = 3
        '
        'txtCode
        '
        Me.txtCode.AcceptsReturn = True
        Me.txtCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HelpProvider1.SetHelpKeyword(Me.txtCode, "")
        Me.HelpProvider1.SetHelpString(Me.txtCode, "")
        Me.txtCode.Location = New System.Drawing.Point(144, 40)
        Me.txtCode.MaxLength = 0
        Me.txtCode.Name = "txtCode"
        Me.txtCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HelpProvider1.SetShowHelp(Me.txtCode, True)
        Me.txtCode.Size = New System.Drawing.Size(81, 21)
        Me.txtCode.TabIndex = 1
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(144, 72)
        Me.txtDescription.MaxLength = 0
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(289, 21)
        Me.txtDescription.TabIndex = 2
        '
        'chkUnderwritingBranch
        '
        Me.chkUnderwritingBranch.BackColor = System.Drawing.SystemColors.Control
        Me.chkUnderwritingBranch.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkUnderwritingBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkUnderwritingBranch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUnderwritingBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkUnderwritingBranch.Location = New System.Drawing.Point(14, 200)
        Me.chkUnderwritingBranch.Name = "chkUnderwritingBranch"
        Me.chkUnderwritingBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkUnderwritingBranch.Size = New System.Drawing.Size(144, 17)
        Me.chkUnderwritingBranch.TabIndex = 6
        Me.chkUnderwritingBranch.Text = "Underwriting Branch:"
        Me.chkUnderwritingBranch.UseVisualStyleBackColor = False
        '
        'cboCurrency
        '
        Me.cboCurrency.CompanyId = 0
        Me.cboCurrency.CurrencyId = 0
        Me.cboCurrency.DefaultCurrencyId = 26
        Me.cboCurrency.FirstItem = ""
        Me.cboCurrency.ListIndex = -1
        Me.cboCurrency.Location = New System.Drawing.Point(144, 168)
        Me.cboCurrency.Name = "cboCurrency"
        Me.cboCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actBaseCurrencies
        Me.cboCurrency.Size = New System.Drawing.Size(153, 21)
        Me.cboCurrency.TabIndex = 5
        Me.cboCurrency.ToolTipText = ""
        Me.cboCurrency.WhatsThisHelpID = 0
        '
        '_cmdNext_0
        '
        Me._cmdNext_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_0.Location = New System.Drawing.Point(529, 252)
        Me._cmdNext_0.Name = "_cmdNext_0"
        Me._cmdNext_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_0.Size = New System.Drawing.Size(32, 19)
        Me._cmdNext_0.TabIndex = 7
        Me._cmdNext_0.Text = ">>"
        Me._cmdNext_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_0.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdPrevious_0)
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdNext_1)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtFaxExtension)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtPhoneExtension)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtAddress3)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtAddress1)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtAddress2)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtAddress4)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtPostalCode)
        Me._tabMainTab_TabPage1.Controls.Add(Me.cmbCountry)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtPhoneAreaCode)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtPhoneNumber)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtFaxAreaCode)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtFaxNumber)
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblExtension)
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblAddress1)
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblAddress2)
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblAddress3)
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblAddress4)
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblPostalCode)
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblCountry)
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblPhone)
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblFax)
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblAreaCode)
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblNumber)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(570, 283)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "2 - Address & Telephone"
        '
        '_cmdPrevious_0
        '
        Me._cmdPrevious_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_0.Location = New System.Drawing.Point(16, 252)
        Me._cmdPrevious_0.Name = "_cmdPrevious_0"
        Me._cmdPrevious_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_0.Size = New System.Drawing.Size(35, 19)
        Me._cmdPrevious_0.TabIndex = 20
        Me._cmdPrevious_0.Text = "<<"
        Me._cmdPrevious_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_0.UseVisualStyleBackColor = False
        '
        '_cmdNext_1
        '
        Me._cmdNext_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_1.Location = New System.Drawing.Point(524, 252)
        Me._cmdNext_1.Name = "_cmdNext_1"
        Me._cmdNext_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_1.Size = New System.Drawing.Size(34, 19)
        Me._cmdNext_1.TabIndex = 21
        Me._cmdNext_1.Text = ">>"
        Me._cmdNext_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_1.UseVisualStyleBackColor = False
        '
        'txtFaxExtension
        '
        Me.txtFaxExtension.AcceptsReturn = True
        Me.txtFaxExtension.BackColor = System.Drawing.SystemColors.Window
        Me.txtFaxExtension.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFaxExtension.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFaxExtension.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFaxExtension.Location = New System.Drawing.Point(328, 192)
        Me.txtFaxExtension.MaxLength = 0
        Me.txtFaxExtension.Name = "txtFaxExtension"
        Me.txtFaxExtension.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFaxExtension.Size = New System.Drawing.Size(97, 21)
        Me.txtFaxExtension.TabIndex = 19
        '
        'txtPhoneExtension
        '
        Me.txtPhoneExtension.AcceptsReturn = True
        Me.txtPhoneExtension.BackColor = System.Drawing.SystemColors.Window
        Me.txtPhoneExtension.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPhoneExtension.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPhoneExtension.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPhoneExtension.Location = New System.Drawing.Point(328, 168)
        Me.txtPhoneExtension.MaxLength = 0
        Me.txtPhoneExtension.Name = "txtPhoneExtension"
        Me.txtPhoneExtension.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPhoneExtension.Size = New System.Drawing.Size(97, 21)
        Me.txtPhoneExtension.TabIndex = 16
        '
        'txtAddress3
        '
        Me.txtAddress3.AcceptsReturn = True
        Me.txtAddress3.BackColor = System.Drawing.SystemColors.Window
        Me.txtAddress3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAddress3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAddress3.Location = New System.Drawing.Point(120, 56)
        Me.txtAddress3.MaxLength = 0
        Me.txtAddress3.Name = "txtAddress3"
        Me.txtAddress3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAddress3.Size = New System.Drawing.Size(305, 21)
        Me.txtAddress3.TabIndex = 10
        '
        'txtAddress1
        '
        Me.txtAddress1.AcceptsReturn = True
        Me.txtAddress1.BackColor = System.Drawing.SystemColors.Window
        Me.txtAddress1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAddress1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAddress1.Location = New System.Drawing.Point(120, 16)
        Me.txtAddress1.MaxLength = 0
        Me.txtAddress1.Name = "txtAddress1"
        Me.txtAddress1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAddress1.Size = New System.Drawing.Size(305, 21)
        Me.txtAddress1.TabIndex = 8
        '
        'txtAddress2
        '
        Me.txtAddress2.AcceptsReturn = True
        Me.txtAddress2.BackColor = System.Drawing.SystemColors.Window
        Me.txtAddress2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAddress2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAddress2.Location = New System.Drawing.Point(120, 36)
        Me.txtAddress2.MaxLength = 0
        Me.txtAddress2.Name = "txtAddress2"
        Me.txtAddress2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAddress2.Size = New System.Drawing.Size(305, 21)
        Me.txtAddress2.TabIndex = 9
        '
        'txtAddress4
        '
        Me.txtAddress4.AcceptsReturn = True
        Me.txtAddress4.BackColor = System.Drawing.SystemColors.Window
        Me.txtAddress4.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAddress4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAddress4.Location = New System.Drawing.Point(120, 76)
        Me.txtAddress4.MaxLength = 0
        Me.txtAddress4.Name = "txtAddress4"
        Me.txtAddress4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAddress4.Size = New System.Drawing.Size(305, 21)
        Me.txtAddress4.TabIndex = 11
        '
        'txtPostalCode
        '
        Me.txtPostalCode.AcceptsReturn = True
        Me.txtPostalCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtPostalCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPostalCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPostalCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPostalCode.Location = New System.Drawing.Point(120, 96)
        Me.txtPostalCode.MaxLength = 0
        Me.txtPostalCode.Name = "txtPostalCode"
        Me.txtPostalCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPostalCode.Size = New System.Drawing.Size(153, 21)
        Me.txtPostalCode.TabIndex = 12
        '
        'cmbCountry
        '
        Me.cmbCountry.BackColor = System.Drawing.SystemColors.Window
        Me.cmbCountry.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbCountry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCountry.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCountry.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cmbCountry, New Integer(-1) {})
        Me.cmbCountry.Location = New System.Drawing.Point(120, 116)
        Me.cmbCountry.Name = "cmbCountry"
        Me.cmbCountry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbCountry.Size = New System.Drawing.Size(305, 21)
        Me.cmbCountry.TabIndex = 13
        '
        'txtPhoneAreaCode
        '
        Me.txtPhoneAreaCode.AcceptsReturn = True
        Me.txtPhoneAreaCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtPhoneAreaCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPhoneAreaCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPhoneAreaCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPhoneAreaCode.Location = New System.Drawing.Point(120, 168)
        Me.txtPhoneAreaCode.MaxLength = 0
        Me.txtPhoneAreaCode.Name = "txtPhoneAreaCode"
        Me.txtPhoneAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPhoneAreaCode.Size = New System.Drawing.Size(81, 21)
        Me.txtPhoneAreaCode.TabIndex = 14
        '
        'txtPhoneNumber
        '
        Me.txtPhoneNumber.AcceptsReturn = True
        Me.txtPhoneNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtPhoneNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPhoneNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPhoneNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPhoneNumber.Location = New System.Drawing.Point(205, 168)
        Me.txtPhoneNumber.MaxLength = 0
        Me.txtPhoneNumber.Name = "txtPhoneNumber"
        Me.txtPhoneNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPhoneNumber.Size = New System.Drawing.Size(121, 21)
        Me.txtPhoneNumber.TabIndex = 15
        '
        'txtFaxAreaCode
        '
        Me.txtFaxAreaCode.AcceptsReturn = True
        Me.txtFaxAreaCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtFaxAreaCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFaxAreaCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFaxAreaCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFaxAreaCode.Location = New System.Drawing.Point(120, 192)
        Me.txtFaxAreaCode.MaxLength = 0
        Me.txtFaxAreaCode.Name = "txtFaxAreaCode"
        Me.txtFaxAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFaxAreaCode.Size = New System.Drawing.Size(81, 21)
        Me.txtFaxAreaCode.TabIndex = 17
        '
        'txtFaxNumber
        '
        Me.txtFaxNumber.AcceptsReturn = True
        Me.txtFaxNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtFaxNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFaxNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFaxNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFaxNumber.Location = New System.Drawing.Point(205, 192)
        Me.txtFaxNumber.MaxLength = 0
        Me.txtFaxNumber.Name = "txtFaxNumber"
        Me.txtFaxNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFaxNumber.Size = New System.Drawing.Size(121, 21)
        Me.txtFaxNumber.TabIndex = 18
        '
        'lblExtension
        '
        Me.lblExtension.BackColor = System.Drawing.SystemColors.Control
        Me.lblExtension.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExtension.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExtension.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExtension.Location = New System.Drawing.Point(331, 152)
        Me.lblExtension.Name = "lblExtension"
        Me.lblExtension.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExtension.Size = New System.Drawing.Size(73, 17)
        Me.lblExtension.TabIndex = 53
        Me.lblExtension.Text = "Extension"
        '
        'lblAddress1
        '
        Me.lblAddress1.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddress1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddress1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddress1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddress1.Location = New System.Drawing.Point(16, 19)
        Me.lblAddress1.Name = "lblAddress1"
        Me.lblAddress1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddress1.Size = New System.Drawing.Size(97, 17)
        Me.lblAddress1.TabIndex = 45
        Me.lblAddress1.Text = "Line &1:"
        '
        'lblAddress2
        '
        Me.lblAddress2.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddress2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddress2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddress2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddress2.Location = New System.Drawing.Point(16, 39)
        Me.lblAddress2.Name = "lblAddress2"
        Me.lblAddress2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddress2.Size = New System.Drawing.Size(97, 17)
        Me.lblAddress2.TabIndex = 46
        Me.lblAddress2.Text = "Line &2:"
        '
        'lblAddress3
        '
        Me.lblAddress3.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddress3.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddress3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddress3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddress3.Location = New System.Drawing.Point(16, 59)
        Me.lblAddress3.Name = "lblAddress3"
        Me.lblAddress3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddress3.Size = New System.Drawing.Size(97, 17)
        Me.lblAddress3.TabIndex = 47
        Me.lblAddress3.Text = "&Town:"
        '
        'lblAddress4
        '
        Me.lblAddress4.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddress4.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddress4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddress4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddress4.Location = New System.Drawing.Point(16, 79)
        Me.lblAddress4.Name = "lblAddress4"
        Me.lblAddress4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddress4.Size = New System.Drawing.Size(97, 17)
        Me.lblAddress4.TabIndex = 48
        Me.lblAddress4.Text = "&Region:"
        '
        'lblPostalCode
        '
        Me.lblPostalCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblPostalCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPostalCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPostalCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPostalCode.Location = New System.Drawing.Point(16, 99)
        Me.lblPostalCode.Name = "lblPostalCode"
        Me.lblPostalCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPostalCode.Size = New System.Drawing.Size(97, 17)
        Me.lblPostalCode.TabIndex = 49
        Me.lblPostalCode.Text = "&Postal Code:"
        '
        'lblCountry
        '
        Me.lblCountry.BackColor = System.Drawing.SystemColors.Control
        Me.lblCountry.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCountry.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCountry.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCountry.Location = New System.Drawing.Point(16, 119)
        Me.lblCountry.Name = "lblCountry"
        Me.lblCountry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCountry.Size = New System.Drawing.Size(97, 17)
        Me.lblCountry.TabIndex = 50
        Me.lblCountry.Text = "&Country:"
        '
        'lblPhone
        '
        Me.lblPhone.BackColor = System.Drawing.SystemColors.Control
        Me.lblPhone.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPhone.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPhone.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPhone.Location = New System.Drawing.Point(16, 171)
        Me.lblPhone.Name = "lblPhone"
        Me.lblPhone.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPhone.Size = New System.Drawing.Size(89, 17)
        Me.lblPhone.TabIndex = 54
        Me.lblPhone.Text = "Telephone No:"
        '
        'lblFax
        '
        Me.lblFax.BackColor = System.Drawing.SystemColors.Control
        Me.lblFax.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFax.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFax.Location = New System.Drawing.Point(16, 195)
        Me.lblFax.Name = "lblFax"
        Me.lblFax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFax.Size = New System.Drawing.Size(97, 17)
        Me.lblFax.TabIndex = 55
        Me.lblFax.Text = "&Fax No:"
        '
        'lblAreaCode
        '
        Me.lblAreaCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblAreaCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAreaCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAreaCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAreaCode.Location = New System.Drawing.Point(123, 152)
        Me.lblAreaCode.Name = "lblAreaCode"
        Me.lblAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAreaCode.Size = New System.Drawing.Size(73, 17)
        Me.lblAreaCode.TabIndex = 51
        Me.lblAreaCode.Text = "Area Code"
        '
        'lblNumber
        '
        Me.lblNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNumber.Location = New System.Drawing.Point(208, 152)
        Me.lblNumber.Name = "lblNumber"
        Me.lblNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNumber.Size = New System.Drawing.Size(73, 17)
        Me.lblNumber.TabIndex = 52
        Me.lblNumber.Text = "Number"
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdPrevious_1)
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdNext_2)
        Me._tabMainTab_TabPage2.Controls.Add(Me.txtDefaultIndicator)
        Me._tabMainTab_TabPage2.Controls.Add(Me.txtPMSourceNumber)
        Me._tabMainTab_TabPage2.Controls.Add(Me.txtUserLicenceId)
        Me._tabMainTab_TabPage2.Controls.Add(Me.txtBrokerABIId)
        Me._tabMainTab_TabPage2.Controls.Add(Me.txtSenderMailboxId)
        Me._tabMainTab_TabPage2.Controls.Add(Me.txtVatNo)
        Me._tabMainTab_TabPage2.Controls.Add(Me.txtEmail)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblDefaultIndicator)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblPMSourceNumber)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblUserLicenceId)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblBrokerABIId)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblSenderMailboxId)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblVatNo)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblEmail)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(570, 283)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "3 - Other Details"
        '
        '_cmdPrevious_1
        '
        Me._cmdPrevious_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_1.Location = New System.Drawing.Point(16, 252)
        Me._cmdPrevious_1.Name = "_cmdPrevious_1"
        Me._cmdPrevious_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_1.Size = New System.Drawing.Size(31, 19)
        Me._cmdPrevious_1.TabIndex = 29
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
        Me._cmdNext_2.Location = New System.Drawing.Point(528, 252)
        Me._cmdNext_2.Name = "_cmdNext_2"
        Me._cmdNext_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_2.Size = New System.Drawing.Size(30, 19)
        Me._cmdNext_2.TabIndex = 30
        Me._cmdNext_2.Text = ">>"
        Me._cmdNext_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_2.UseVisualStyleBackColor = False
        '
        'txtDefaultIndicator
        '
        Me.txtDefaultIndicator.AcceptsReturn = True
        Me.txtDefaultIndicator.BackColor = System.Drawing.SystemColors.Window
        Me.txtDefaultIndicator.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDefaultIndicator.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDefaultIndicator.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDefaultIndicator.Location = New System.Drawing.Point(304, 174)
        Me.txtDefaultIndicator.MaxLength = 0
        Me.txtDefaultIndicator.Name = "txtDefaultIndicator"
        Me.txtDefaultIndicator.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDefaultIndicator.Size = New System.Drawing.Size(25, 21)
        Me.txtDefaultIndicator.TabIndex = 28
        '
        'txtPMSourceNumber
        '
        Me.txtPMSourceNumber.AcceptsReturn = True
        Me.txtPMSourceNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtPMSourceNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPMSourceNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPMSourceNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPMSourceNumber.Location = New System.Drawing.Point(128, 174)
        Me.txtPMSourceNumber.MaxLength = 0
        Me.txtPMSourceNumber.Name = "txtPMSourceNumber"
        Me.txtPMSourceNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPMSourceNumber.Size = New System.Drawing.Size(25, 21)
        Me.txtPMSourceNumber.TabIndex = 27
        '
        'txtUserLicenceId
        '
        Me.txtUserLicenceId.AcceptsReturn = True
        Me.txtUserLicenceId.BackColor = System.Drawing.SystemColors.Window
        Me.txtUserLicenceId.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtUserLicenceId.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUserLicenceId.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtUserLicenceId.Location = New System.Drawing.Point(128, 142)
        Me.txtUserLicenceId.MaxLength = 4
        Me.txtUserLicenceId.Name = "txtUserLicenceId"
        Me.txtUserLicenceId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtUserLicenceId.Size = New System.Drawing.Size(41, 21)
        Me.txtUserLicenceId.TabIndex = 26
        '
        'txtBrokerABIId
        '
        Me.txtBrokerABIId.AcceptsReturn = True
        Me.txtBrokerABIId.BackColor = System.Drawing.SystemColors.Window
        Me.txtBrokerABIId.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBrokerABIId.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBrokerABIId.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBrokerABIId.Location = New System.Drawing.Point(128, 110)
        Me.txtBrokerABIId.MaxLength = 0
        Me.txtBrokerABIId.Name = "txtBrokerABIId"
        Me.txtBrokerABIId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBrokerABIId.Size = New System.Drawing.Size(49, 21)
        Me.txtBrokerABIId.TabIndex = 25
        '
        'txtSenderMailboxId
        '
        Me.txtSenderMailboxId.AcceptsReturn = True
        Me.txtSenderMailboxId.BackColor = System.Drawing.SystemColors.Window
        Me.txtSenderMailboxId.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSenderMailboxId.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSenderMailboxId.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSenderMailboxId.Location = New System.Drawing.Point(128, 78)
        Me.txtSenderMailboxId.MaxLength = 0
        Me.txtSenderMailboxId.Name = "txtSenderMailboxId"
        Me.txtSenderMailboxId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSenderMailboxId.Size = New System.Drawing.Size(97, 21)
        Me.txtSenderMailboxId.TabIndex = 24
        '
        'txtVatNo
        '
        Me.txtVatNo.AcceptsReturn = True
        Me.txtVatNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtVatNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtVatNo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVatNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtVatNo.Location = New System.Drawing.Point(128, 46)
        Me.txtVatNo.MaxLength = 0
        Me.txtVatNo.Name = "txtVatNo"
        Me.txtVatNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtVatNo.Size = New System.Drawing.Size(129, 21)
        Me.txtVatNo.TabIndex = 23
        '
        'txtEmail
        '
        Me.txtEmail.AcceptsReturn = True
        Me.txtEmail.BackColor = System.Drawing.SystemColors.Window
        Me.txtEmail.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEmail.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEmail.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEmail.Location = New System.Drawing.Point(128, 14)
        Me.txtEmail.MaxLength = 0
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEmail.Size = New System.Drawing.Size(353, 21)
        Me.txtEmail.TabIndex = 22
        '
        'lblDefaultIndicator
        '
        Me.lblDefaultIndicator.BackColor = System.Drawing.SystemColors.Control
        Me.lblDefaultIndicator.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDefaultIndicator.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDefaultIndicator.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDefaultIndicator.Location = New System.Drawing.Point(192, 176)
        Me.lblDefaultIndicator.Name = "lblDefaultIndicator"
        Me.lblDefaultIndicator.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDefaultIndicator.Size = New System.Drawing.Size(105, 17)
        Me.lblDefaultIndicator.TabIndex = 63
        Me.lblDefaultIndicator.Text = "&Default Indicator:"
        '
        'lblPMSourceNumber
        '
        Me.lblPMSourceNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblPMSourceNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPMSourceNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPMSourceNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPMSourceNumber.Location = New System.Drawing.Point(16, 176)
        Me.lblPMSourceNumber.Name = "lblPMSourceNumber"
        Me.lblPMSourceNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPMSourceNumber.Size = New System.Drawing.Size(105, 17)
        Me.lblPMSourceNumber.TabIndex = 62
        Me.lblPMSourceNumber.Text = "&PM Company No:"
        '
        'lblUserLicenceId
        '
        Me.lblUserLicenceId.BackColor = System.Drawing.SystemColors.Control
        Me.lblUserLicenceId.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUserLicenceId.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserLicenceId.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUserLicenceId.Location = New System.Drawing.Point(16, 144)
        Me.lblUserLicenceId.Name = "lblUserLicenceId"
        Me.lblUserLicenceId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUserLicenceId.Size = New System.Drawing.Size(105, 17)
        Me.lblUserLicenceId.TabIndex = 61
        Me.lblUserLicenceId.Text = "&User Licence Id:"
        '
        'lblBrokerABIId
        '
        Me.lblBrokerABIId.BackColor = System.Drawing.SystemColors.Control
        Me.lblBrokerABIId.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBrokerABIId.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBrokerABIId.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBrokerABIId.Location = New System.Drawing.Point(16, 112)
        Me.lblBrokerABIId.Name = "lblBrokerABIId"
        Me.lblBrokerABIId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBrokerABIId.Size = New System.Drawing.Size(105, 17)
        Me.lblBrokerABIId.TabIndex = 60
        Me.lblBrokerABIId.Text = "&Broker ABI Id:"
        '
        'lblSenderMailboxId
        '
        Me.lblSenderMailboxId.BackColor = System.Drawing.SystemColors.Control
        Me.lblSenderMailboxId.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSenderMailboxId.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSenderMailboxId.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSenderMailboxId.Location = New System.Drawing.Point(16, 80)
        Me.lblSenderMailboxId.Name = "lblSenderMailboxId"
        Me.lblSenderMailboxId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSenderMailboxId.Size = New System.Drawing.Size(125, 17)
        Me.lblSenderMailboxId.TabIndex = 59
        Me.lblSenderMailboxId.Text = "&Sender Mailbox Id:"
        '
        'lblVatNo
        '
        Me.lblVatNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblVatNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblVatNo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVatNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblVatNo.Location = New System.Drawing.Point(16, 48)
        Me.lblVatNo.Name = "lblVatNo"
        Me.lblVatNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblVatNo.Size = New System.Drawing.Size(105, 17)
        Me.lblVatNo.TabIndex = 58
        Me.lblVatNo.Text = "&Vat Number:"
        '
        'lblEmail
        '
        Me.lblEmail.BackColor = System.Drawing.SystemColors.Control
        Me.lblEmail.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEmail.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEmail.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEmail.Location = New System.Drawing.Point(16, 16)
        Me.lblEmail.Name = "lblEmail"
        Me.lblEmail.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEmail.Size = New System.Drawing.Size(105, 17)
        Me.lblEmail.TabIndex = 57
        Me.lblEmail.Text = "&E-Mail:"
        '
        '_tabMainTab_TabPage3
        '
        Me._tabMainTab_TabPage3.Controls.Add(Me._cmdPrevious_2)
        Me._tabMainTab_TabPage3.Controls.Add(Me._cmdNext_3)
        Me._tabMainTab_TabPage3.Controls.Add(Me.cmdRates)
        Me._tabMainTab_TabPage3.Controls.Add(Me.uctCurrencies)
        Me._tabMainTab_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage3.Name = "_tabMainTab_TabPage3"
        Me._tabMainTab_TabPage3.Size = New System.Drawing.Size(570, 283)
        Me._tabMainTab_TabPage3.TabIndex = 3
        Me._tabMainTab_TabPage3.Text = "4 - Currencies"
        '
        '_cmdPrevious_2
        '
        Me._cmdPrevious_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_2.Location = New System.Drawing.Point(16, 252)
        Me._cmdPrevious_2.Name = "_cmdPrevious_2"
        Me._cmdPrevious_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_2.Size = New System.Drawing.Size(32, 19)
        Me._cmdPrevious_2.TabIndex = 33
        Me._cmdPrevious_2.Text = "<<"
        Me._cmdPrevious_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_2.UseVisualStyleBackColor = False
        '
        '_cmdNext_3
        '
        Me._cmdNext_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_3.Location = New System.Drawing.Point(521, 252)
        Me._cmdNext_3.Name = "_cmdNext_3"
        Me._cmdNext_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_3.Size = New System.Drawing.Size(37, 19)
        Me._cmdNext_3.TabIndex = 34
        Me._cmdNext_3.Text = ">>"
        Me._cmdNext_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_3.UseVisualStyleBackColor = False
        '
        'cmdRates
        '
        Me.cmdRates.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRates.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRates.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRates.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRates.Location = New System.Drawing.Point(252, 216)
        Me.cmdRates.Name = "cmdRates"
        Me.cmdRates.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRates.Size = New System.Drawing.Size(65, 26)
        Me.cmdRates.TabIndex = 32
        Me.cmdRates.Text = "Rates"
        Me.cmdRates.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRates.UseVisualStyleBackColor = False
        '
        'uctCurrencies
        '
        Me.uctCurrencies.AvailableCaption = "Currencies"
        Me.uctCurrencies.BusinessObject = "bPMSource.Business"
        Me.uctCurrencies.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctCurrencies.ForeignKeys = CType(resources.GetObject("uctCurrencies.ForeignKeys"), Microsoft.VisualBasic.Collection)
        Me.uctCurrencies.IsSearchable = False
        Me.uctCurrencies.Location = New System.Drawing.Point(8, 7)
        Me.uctCurrencies.Name = "uctCurrencies"
        Me.uctCurrencies.PickListType = "Currency"
        Me.uctCurrencies.Size = New System.Drawing.Size(553, 209)
        Me.uctCurrencies.TabIndex = 31
        Me.uctCurrencies.TabStop = False
        '
        '_tabMainTab_TabPage4
        '
        Me._tabMainTab_TabPage4.Controls.Add(Me._cmdPrevious_3)
        Me._tabMainTab_TabPage4.Controls.Add(Me.cboFSABankType)
        Me._tabMainTab_TabPage4.Controls.Add(Me.txtStaffWording)
        Me._tabMainTab_TabPage4.Controls.Add(Me.cboCompanyCategory)
        Me._tabMainTab_TabPage4.Controls.Add(Me.lblFSABankType)
        Me._tabMainTab_TabPage4.Controls.Add(Me.lblCompanyCategory)
        Me._tabMainTab_TabPage4.Controls.Add(Me.lblStaffWording)
        Me._tabMainTab_TabPage4.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage4.Name = "_tabMainTab_TabPage4"
        Me._tabMainTab_TabPage4.Size = New System.Drawing.Size(570, 283)
        Me._tabMainTab_TabPage4.TabIndex = 4
        Me._tabMainTab_TabPage4.Text = "5 - FSA Compliance"
        '
        '_cmdPrevious_3
        '
        Me._cmdPrevious_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_3.Location = New System.Drawing.Point(16, 252)
        Me._cmdPrevious_3.Name = "_cmdPrevious_3"
        Me._cmdPrevious_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_3.Size = New System.Drawing.Size(35, 19)
        Me._cmdPrevious_3.TabIndex = 38
        Me._cmdPrevious_3.Text = "<<"
        Me._cmdPrevious_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_3.UseVisualStyleBackColor = False
        '
        'cboFSABankType
        '
        Me.cboFSABankType.BackColor = System.Drawing.SystemColors.Window
        Me.cboFSABankType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboFSABankType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboFSABankType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboFSABankType, New Integer() {0, 0, 0, 0})
        Me.cboFSABankType.Items.AddRange(New Object() {"", "Statutory", "Non Statutory", "Risk Transfer"})
        Me.cboFSABankType.Location = New System.Drawing.Point(112, 212)
        Me.cboFSABankType.Name = "cboFSABankType"
        Me.cboFSABankType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboFSABankType.Size = New System.Drawing.Size(209, 21)
        Me.cboFSABankType.TabIndex = 37
        Me.cboFSABankType.Text = " "
        '
        'txtStaffWording
        '
        Me.txtStaffWording.AcceptsReturn = True
        Me.txtStaffWording.BackColor = System.Drawing.SystemColors.Window
        Me.txtStaffWording.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStaffWording.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStaffWording.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStaffWording.Location = New System.Drawing.Point(112, 48)
        Me.txtStaffWording.MaxLength = 0
        Me.txtStaffWording.Multiline = True
        Me.txtStaffWording.Name = "txtStaffWording"
        Me.txtStaffWording.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStaffWording.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtStaffWording.Size = New System.Drawing.Size(377, 145)
        Me.txtStaffWording.TabIndex = 36
        '
        'cboCompanyCategory
        '
        Me.cboCompanyCategory.BackColor = System.Drawing.SystemColors.Window
        Me.cboCompanyCategory.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCompanyCategory.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCompanyCategory.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboCompanyCategory, New Integer(-1) {})
        Me.cboCompanyCategory.Location = New System.Drawing.Point(112, 16)
        Me.cboCompanyCategory.Name = "cboCompanyCategory"
        Me.cboCompanyCategory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCompanyCategory.Size = New System.Drawing.Size(377, 21)
        Me.cboCompanyCategory.TabIndex = 35
        Me.cboCompanyCategory.Text = "Combo1"
        '
        'lblFSABankType
        '
        Me.lblFSABankType.BackColor = System.Drawing.SystemColors.Control
        Me.lblFSABankType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFSABankType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFSABankType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFSABankType.Location = New System.Drawing.Point(16, 212)
        Me.lblFSABankType.Name = "lblFSABankType"
        Me.lblFSABankType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFSABankType.Size = New System.Drawing.Size(81, 17)
        Me.lblFSABankType.TabIndex = 72
        Me.lblFSABankType.Text = "Bank Type:"
        '
        'lblCompanyCategory
        '
        Me.lblCompanyCategory.BackColor = System.Drawing.SystemColors.Control
        Me.lblCompanyCategory.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCompanyCategory.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCompanyCategory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCompanyCategory.Location = New System.Drawing.Point(16, 18)
        Me.lblCompanyCategory.Name = "lblCompanyCategory"
        Me.lblCompanyCategory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCompanyCategory.Size = New System.Drawing.Size(145, 17)
        Me.lblCompanyCategory.TabIndex = 65
        Me.lblCompanyCategory.Text = "Category:"
        '
        'lblStaffWording
        '
        Me.lblStaffWording.BackColor = System.Drawing.SystemColors.Control
        Me.lblStaffWording.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStaffWording.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStaffWording.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStaffWording.Location = New System.Drawing.Point(16, 48)
        Me.lblStaffWording.Name = "lblStaffWording"
        Me.lblStaffWording.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStaffWording.Size = New System.Drawing.Size(129, 17)
        Me.lblStaffWording.TabIndex = 64
        Me.lblStaffWording.Text = "Staff Wording:"
        '
        '_tabMainTab_TabPage5
        '
        Me._tabMainTab_TabPage5.Controls.Add(Me.lblClosedBranch)
        Me._tabMainTab_TabPage5.Controls.Add(Me.chkAllowTempMTA)
        Me._tabMainTab_TabPage5.Controls.Add(Me.chkAllowPermMTA)
        Me._tabMainTab_TabPage5.Controls.Add(Me.chkAllowReports)
        Me._tabMainTab_TabPage5.Controls.Add(Me.chkAllowClaims)
        Me._tabMainTab_TabPage5.Controls.Add(Me.chkAllowAccounts)
        Me._tabMainTab_TabPage5.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage5.Name = "_tabMainTab_TabPage5"
        Me._tabMainTab_TabPage5.Size = New System.Drawing.Size(570, 283)
        Me._tabMainTab_TabPage5.TabIndex = 5
        Me._tabMainTab_TabPage5.Text = "6 - Closed Branch"
        '
        'lblClosedBranch
        '
        Me.lblClosedBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblClosedBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClosedBranch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClosedBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClosedBranch.Location = New System.Drawing.Point(16, 22)
        Me.lblClosedBranch.Name = "lblClosedBranch"
        Me.lblClosedBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClosedBranch.Size = New System.Drawing.Size(269, 17)
        Me.lblClosedBranch.TabIndex = 66
        Me.lblClosedBranch.Text = "Allow the following Tasks on Closed Branch:"
        '
        'chkAllowTempMTA
        '
        Me.chkAllowTempMTA.BackColor = System.Drawing.SystemColors.Control
        Me.chkAllowTempMTA.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAllowTempMTA.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAllowTempMTA.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAllowTempMTA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAllowTempMTA.Location = New System.Drawing.Point(80, 52)
        Me.chkAllowTempMTA.Name = "chkAllowTempMTA"
        Me.chkAllowTempMTA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllowTempMTA.Size = New System.Drawing.Size(169, 17)
        Me.chkAllowTempMTA.TabIndex = 67
        Me.chkAllowTempMTA.Text = "Temporary MTA :"
        Me.chkAllowTempMTA.UseVisualStyleBackColor = False
        '
        'chkAllowPermMTA
        '
        Me.chkAllowPermMTA.BackColor = System.Drawing.SystemColors.Control
        Me.chkAllowPermMTA.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAllowPermMTA.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAllowPermMTA.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAllowPermMTA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAllowPermMTA.Location = New System.Drawing.Point(80, 76)
        Me.chkAllowPermMTA.Name = "chkAllowPermMTA"
        Me.chkAllowPermMTA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllowPermMTA.Size = New System.Drawing.Size(169, 17)
        Me.chkAllowPermMTA.TabIndex = 68
        Me.chkAllowPermMTA.Text = "Permanent MTA :"
        Me.chkAllowPermMTA.UseVisualStyleBackColor = False
        '
        'chkAllowReports
        '
        Me.chkAllowReports.BackColor = System.Drawing.SystemColors.Control
        Me.chkAllowReports.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAllowReports.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAllowReports.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAllowReports.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAllowReports.Location = New System.Drawing.Point(80, 100)
        Me.chkAllowReports.Name = "chkAllowReports"
        Me.chkAllowReports.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllowReports.Size = New System.Drawing.Size(169, 17)
        Me.chkAllowReports.TabIndex = 69
        Me.chkAllowReports.Text = "Reports :"
        Me.chkAllowReports.UseVisualStyleBackColor = False
        '
        'chkAllowClaims
        '
        Me.chkAllowClaims.BackColor = System.Drawing.SystemColors.Control
        Me.chkAllowClaims.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAllowClaims.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAllowClaims.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAllowClaims.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAllowClaims.Location = New System.Drawing.Point(80, 124)
        Me.chkAllowClaims.Name = "chkAllowClaims"
        Me.chkAllowClaims.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllowClaims.Size = New System.Drawing.Size(169, 17)
        Me.chkAllowClaims.TabIndex = 70
        Me.chkAllowClaims.Text = "Claims :"
        Me.chkAllowClaims.UseVisualStyleBackColor = False
        '
        'chkAllowAccounts
        '
        Me.chkAllowAccounts.BackColor = System.Drawing.SystemColors.Control
        Me.chkAllowAccounts.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAllowAccounts.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAllowAccounts.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAllowAccounts.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAllowAccounts.Location = New System.Drawing.Point(80, 148)
        Me.chkAllowAccounts.Name = "chkAllowAccounts"
        Me.chkAllowAccounts.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllowAccounts.Size = New System.Drawing.Size(169, 17)
        Me.chkAllowAccounts.TabIndex = 71
        Me.chkAllowAccounts.Text = "Accounts :"
        Me.chkAllowAccounts.UseVisualStyleBackColor = False
        '
        'frmDetails
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(587, 347)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDetails"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Edit Branch"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        CType(Me.imgImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me._tabMainTab_TabPage1.PerformLayout()
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me._tabMainTab_TabPage2.PerformLayout()
        Me._tabMainTab_TabPage3.ResumeLayout(False)
        Me._tabMainTab_TabPage4.ResumeLayout(False)
        Me._tabMainTab_TabPage4.PerformLayout()
        Me._tabMainTab_TabPage5.ResumeLayout(False)
        CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

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
    Friend WithEvents HelpProvider1 As System.Windows.Forms.HelpProvider
#End Region 
End Class