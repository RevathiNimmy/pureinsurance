<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDetails
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblRegNo2 As System.Windows.Forms.Label
	Public WithEvents lblRegNo1 As System.Windows.Forms.Label
	Public WithEvents lblCode As System.Windows.Forms.Label
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents imgImage As System.Windows.Forms.PictureBox
	Public WithEvents lblBaseCurrency As System.Windows.Forms.Label
	Public WithEvents uctCurrency As UserControls.CurrencyLookup
	Private WithEvents _cmdNext_0 As System.Windows.Forms.Button
	Public WithEvents txtRegNo2 As System.Windows.Forms.TextBox
	Public WithEvents txtRegNo1 As System.Windows.Forms.TextBox
	Public WithEvents txtCode As System.Windows.Forms.TextBox
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
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
	Public WithEvents txtDefaultIndicator As System.Windows.Forms.TextBox
	Public WithEvents txtPMCompanyNumber As System.Windows.Forms.TextBox
	Public WithEvents txtUserLicenceId As System.Windows.Forms.TextBox
	Public WithEvents txtBrokerABIId As System.Windows.Forms.TextBox
	Public WithEvents txtSenderMailboxId As System.Windows.Forms.TextBox
	Public WithEvents txtVatNo As System.Windows.Forms.TextBox
	Public WithEvents txtEmail As System.Windows.Forms.TextBox
	Public WithEvents lblDefaultIndicator As System.Windows.Forms.Label
	Public WithEvents lblPMCompanyNumber As System.Windows.Forms.Label
	Public WithEvents lblUserLicenceId As System.Windows.Forms.Label
	Public WithEvents lblBrokerABIId As System.Windows.Forms.Label
	Public WithEvents lblSenderMailboxId As System.Windows.Forms.Label
	Public WithEvents lblVatNo As System.Windows.Forms.Label
	Public WithEvents lblEmail As System.Windows.Forms.Label
	Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public cmdNext(1) As System.Windows.Forms.Button
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDetails))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
		Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
		Me.dlgHelpFont = New System.Windows.Forms.FontDialog
		Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
		Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
		Me.cmdNavigate = New System.Windows.Forms.Button
		Me.cmdHelp = New System.Windows.Forms.Button
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
		Me.uctCurrency = New UserControls.CurrencyLookup
		Me._cmdNext_0 = New System.Windows.Forms.Button
		Me.txtRegNo2 = New System.Windows.Forms.TextBox
		Me.txtRegNo1 = New System.Windows.Forms.TextBox
		Me.txtCode = New System.Windows.Forms.TextBox
		Me.txtDescription = New System.Windows.Forms.TextBox
		Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
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
		Me.txtDefaultIndicator = New System.Windows.Forms.TextBox
		Me.txtPMCompanyNumber = New System.Windows.Forms.TextBox
		Me.txtUserLicenceId = New System.Windows.Forms.TextBox
		Me.txtBrokerABIId = New System.Windows.Forms.TextBox
		Me.txtSenderMailboxId = New System.Windows.Forms.TextBox
		Me.txtVatNo = New System.Windows.Forms.TextBox
		Me.txtEmail = New System.Windows.Forms.TextBox
		Me.lblDefaultIndicator = New System.Windows.Forms.Label
		Me.lblPMCompanyNumber = New System.Windows.Forms.Label
		Me.lblUserLicenceId = New System.Windows.Forms.Label
		Me.lblBrokerABIId = New System.Windows.Forms.Label
		Me.lblSenderMailboxId = New System.Windows.Forms.Label
		Me.lblVatNo = New System.Windows.Forms.Label
		Me.lblEmail = New System.Windows.Forms.Label
		Me.tabMainTab.SuspendLayout()
		Me._tabMainTab_TabPage0.SuspendLayout()
		Me._tabMainTab_TabPage1.SuspendLayout()
		Me._tabMainTab_TabPage2.SuspendLayout()
		Me.SuspendLayout()
		' 
		' cmdNavigate
		' 
		Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
		Me.cmdNavigate.CausesValidation = True
		Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdNavigate.Enabled = True
		Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNavigate.Location = New System.Drawing.Point(8, 288)
		Me.cmdNavigate.Name = "cmdNavigate"
		Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
		Me.cmdNavigate.TabIndex = 20
		Me.cmdNavigate.TabStop = True
		Me.cmdNavigate.Text = "&Navigate"
		Me.cmdNavigate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.cmdNavigate.Visible = False
		' 
		' cmdHelp
		' 
		Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
		Me.cmdHelp.CausesValidation = True
		Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdHelp.Enabled = True
		Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdHelp.Location = New System.Drawing.Point(384, 288)
		Me.cmdHelp.Name = "cmdHelp"
		Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.cmdHelp.TabIndex = 19
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
		Me.cmdCancel.Location = New System.Drawing.Point(304, 288)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 18
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(224, 288)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 17
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tabMainTab
		' 
		Me.tabMainTab.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabMainTab.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
		Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage1)
		Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage2)
		Me.tabMainTab.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMainTab.ItemSize = New System.Drawing.Size(88, 18)
		Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
		Me.tabMainTab.Multiline = True
		Me.tabMainTab.Name = "tabMainTab"
		Me.tabMainTab.Size = New System.Drawing.Size(453, 277)
		Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMainTab.TabIndex = 21
		' 
		' _tabMainTab_TabPage0
		' 
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblRegNo2)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblRegNo1)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblCode)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblDescription)
		Me._tabMainTab_TabPage0.Controls.Add(Me.imgImage)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblBaseCurrency)
		Me._tabMainTab_TabPage0.Controls.Add(Me.uctCurrency)
		Me._tabMainTab_TabPage0.Controls.Add(Me._cmdNext_0)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtRegNo2)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtRegNo1)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtCode)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtDescription)
		Me._tabMainTab_TabPage0.Text = "&1 - General"
		' 
		' lblRegNo2
		' 
		Me.lblRegNo2.AutoSize = False
		Me.lblRegNo2.BackColor = System.Drawing.SystemColors.Control
		Me.lblRegNo2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblRegNo2.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblRegNo2.Enabled = True
		Me.lblRegNo2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblRegNo2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblRegNo2.Location = New System.Drawing.Point(16, 119)
		Me.lblRegNo2.Name = "lblRegNo2"
		Me.lblRegNo2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblRegNo2.Size = New System.Drawing.Size(113, 17)
		Me.lblRegNo2.TabIndex = 25
		Me.lblRegNo2.Text = "Registration No. 2:"
		Me.lblRegNo2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblRegNo2.UseMnemonic = True
		Me.lblRegNo2.Visible = True
		' 
		' lblRegNo1
		' 
		Me.lblRegNo1.AutoSize = False
		Me.lblRegNo1.BackColor = System.Drawing.SystemColors.Control
		Me.lblRegNo1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblRegNo1.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblRegNo1.Enabled = True
		Me.lblRegNo1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblRegNo1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblRegNo1.Location = New System.Drawing.Point(16, 87)
		Me.lblRegNo1.Name = "lblRegNo1"
		Me.lblRegNo1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblRegNo1.Size = New System.Drawing.Size(113, 17)
		Me.lblRegNo1.TabIndex = 24
		Me.lblRegNo1.Text = "Registration No. 1:"
		Me.lblRegNo1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblRegNo1.UseMnemonic = True
		Me.lblRegNo1.Visible = True
		' 
		' lblCode
		' 
		Me.lblCode.AutoSize = False
		Me.lblCode.BackColor = System.Drawing.SystemColors.Control
		Me.lblCode.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCode.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCode.Enabled = True
		Me.lblCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCode.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCode.Location = New System.Drawing.Point(16, 23)
		Me.lblCode.Name = "lblCode"
		Me.lblCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCode.Size = New System.Drawing.Size(113, 17)
		Me.lblCode.TabIndex = 22
		Me.lblCode.Text = "Short Name:"
		Me.lblCode.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCode.UseMnemonic = True
		Me.lblCode.Visible = True
		' 
		' lblDescription
		' 
		Me.lblDescription.AutoSize = False
		Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
		Me.lblDescription.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDescription.Enabled = True
		Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDescription.Location = New System.Drawing.Point(16, 55)
		Me.lblDescription.Name = "lblDescription"
		Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDescription.Size = New System.Drawing.Size(113, 17)
		Me.lblDescription.TabIndex = 23
		Me.lblDescription.Text = "Name:"
		Me.lblDescription.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDescription.UseMnemonic = True
		Me.lblDescription.Visible = True
		' 
		' imgImage
		' 
		Me.imgImage.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.imgImage.Cursor = System.Windows.Forms.Cursors.Default
		Me.imgImage.Enabled = True
		Me.imgImage.Image = CType(resources.GetObject("imgImage.Image"), System.Drawing.Image)
		Me.imgImage.Location = New System.Drawing.Point(408, 4)
		Me.imgImage.Name = "imgImage"
		Me.imgImage.Size = New System.Drawing.Size(32, 32)
		Me.imgImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.imgImage.Visible = True
		' 
		' lblBaseCurrency
		' 
		Me.lblBaseCurrency.AutoSize = False
		Me.lblBaseCurrency.BackColor = System.Drawing.SystemColors.Control
		Me.lblBaseCurrency.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblBaseCurrency.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblBaseCurrency.Enabled = True
		Me.lblBaseCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblBaseCurrency.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblBaseCurrency.Location = New System.Drawing.Point(16, 150)
		Me.lblBaseCurrency.Name = "lblBaseCurrency"
		Me.lblBaseCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblBaseCurrency.Size = New System.Drawing.Size(105, 17)
		Me.lblBaseCurrency.TabIndex = 38
		Me.lblBaseCurrency.Text = "Base Currency:"
		Me.lblBaseCurrency.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblBaseCurrency.UseMnemonic = True
		Me.lblBaseCurrency.Visible = True
		' 
		' uctCurrency
		' 
		Me.uctCurrency.Location = New System.Drawing.Point(128, 148)
		Me.uctCurrency.Name = "uctCurrency"
		Me.uctCurrency.Size = New System.Drawing.Size(153, 21)
		Me.uctCurrency.TabIndex = 37
		Me.uctCurrency.WhatsThisHelpID = 2005
		' 
		' _cmdNext_0
		' 
		Me._cmdNext_0.BackColor = System.Drawing.SystemColors.Control
		Me._cmdNext_0.CausesValidation = True
		Me._cmdNext_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._cmdNext_0.Enabled = True
		Me._cmdNext_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._cmdNext_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me._cmdNext_0.Location = New System.Drawing.Point(416, 220)
		Me._cmdNext_0.Name = "_cmdNext_0"
		Me._cmdNext_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._cmdNext_0.Size = New System.Drawing.Size(22, 19)
		Me._cmdNext_0.TabIndex = 4
		Me._cmdNext_0.TabStop = True
		Me._cmdNext_0.Text = ">>"
		Me._cmdNext_0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me._cmdNext_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtRegNo2
		' 
		Me.txtRegNo2.AcceptsReturn = True
		Me.txtRegNo2.AutoSize = False
		Me.txtRegNo2.BackColor = System.Drawing.SystemColors.Window
		Me.txtRegNo2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtRegNo2.CausesValidation = True
		Me.txtRegNo2.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtRegNo2.Enabled = True
		Me.txtRegNo2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtRegNo2.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtRegNo2.HideSelection = True
		Me.txtRegNo2.Location = New System.Drawing.Point(128, 116)
		Me.txtRegNo2.MaxLength = 0
		Me.txtRegNo2.Multiline = False
		Me.txtRegNo2.Name = "txtRegNo2"
		Me.txtRegNo2.ReadOnly = False
		Me.txtRegNo2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtRegNo2.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtRegNo2.Size = New System.Drawing.Size(153, 19)
		Me.txtRegNo2.TabIndex = 3
		Me.txtRegNo2.TabStop = True
		Me.txtRegNo2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtRegNo2.Visible = True
		' 
		' txtRegNo1
		' 
		Me.txtRegNo1.AcceptsReturn = True
		Me.txtRegNo1.AutoSize = False
		Me.txtRegNo1.BackColor = System.Drawing.SystemColors.Window
		Me.txtRegNo1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtRegNo1.CausesValidation = True
		Me.txtRegNo1.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtRegNo1.Enabled = True
		Me.txtRegNo1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtRegNo1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtRegNo1.HideSelection = True
		Me.txtRegNo1.Location = New System.Drawing.Point(128, 84)
		Me.txtRegNo1.MaxLength = 0
		Me.txtRegNo1.Multiline = False
		Me.txtRegNo1.Name = "txtRegNo1"
		Me.txtRegNo1.ReadOnly = False
		Me.txtRegNo1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtRegNo1.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtRegNo1.Size = New System.Drawing.Size(153, 19)
		Me.txtRegNo1.TabIndex = 2
		Me.txtRegNo1.TabStop = True
		Me.txtRegNo1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtRegNo1.Visible = True
		' 
		' txtCode
		' 
		Me.txtCode.AcceptsReturn = True
		Me.txtCode.AutoSize = False
		Me.txtCode.BackColor = System.Drawing.SystemColors.Window
		Me.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtCode.CausesValidation = True
		Me.txtCode.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtCode.Enabled = True
		Me.txtCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtCode.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtCode.HideSelection = True
		Me.txtCode.Location = New System.Drawing.Point(128, 20)
		Me.txtCode.MaxLength = 0
		Me.txtCode.Multiline = False
		Me.txtCode.Name = "txtCode"
		Me.txtCode.ReadOnly = False
		Me.txtCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtCode.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtCode.Size = New System.Drawing.Size(81, 19)
		Me.txtCode.TabIndex = 0
		Me.txtCode.TabStop = True
		Me.txtCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtCode.Visible = True
		' 
		' txtDescription
		' 
		Me.txtDescription.AcceptsReturn = True
		Me.txtDescription.AutoSize = False
		Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
		Me.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDescription.CausesValidation = True
		Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDescription.Enabled = True
		Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDescription.HideSelection = True
		Me.txtDescription.Location = New System.Drawing.Point(128, 52)
		Me.txtDescription.MaxLength = 0
		Me.txtDescription.Multiline = False
		Me.txtDescription.Name = "txtDescription"
		Me.txtDescription.ReadOnly = False
		Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDescription.Size = New System.Drawing.Size(289, 19)
		Me.txtDescription.TabIndex = 1
		Me.txtDescription.TabStop = True
		Me.txtDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDescription.Visible = True
		' 
		' _tabMainTab_TabPage1
		' 
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
		Me._tabMainTab_TabPage1.Text = "&2 - Address && Telephone"
		' 
		' _cmdNext_1
		' 
		Me._cmdNext_1.BackColor = System.Drawing.SystemColors.Control
		Me._cmdNext_1.CausesValidation = True
		Me._cmdNext_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._cmdNext_1.Enabled = True
		Me._cmdNext_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._cmdNext_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._cmdNext_1.Location = New System.Drawing.Point(416, 220)
		Me._cmdNext_1.Name = "_cmdNext_1"
		Me._cmdNext_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._cmdNext_1.Size = New System.Drawing.Size(22, 19)
		Me._cmdNext_1.TabIndex = 53
		Me._cmdNext_1.TabStop = True
		Me._cmdNext_1.Text = ">>"
		Me._cmdNext_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me._cmdNext_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtFaxExtension
		' 
		Me.txtFaxExtension.AcceptsReturn = True
		Me.txtFaxExtension.AutoSize = False
		Me.txtFaxExtension.BackColor = System.Drawing.SystemColors.Window
		Me.txtFaxExtension.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtFaxExtension.CausesValidation = True
		Me.txtFaxExtension.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtFaxExtension.Enabled = True
		Me.txtFaxExtension.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtFaxExtension.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtFaxExtension.HideSelection = True
		Me.txtFaxExtension.Location = New System.Drawing.Point(328, 196)
		Me.txtFaxExtension.MaxLength = 0
		Me.txtFaxExtension.Multiline = False
		Me.txtFaxExtension.Name = "txtFaxExtension"
		Me.txtFaxExtension.ReadOnly = False
		Me.txtFaxExtension.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtFaxExtension.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtFaxExtension.Size = New System.Drawing.Size(81, 19)
		Me.txtFaxExtension.TabIndex = 16
		Me.txtFaxExtension.TabStop = True
		Me.txtFaxExtension.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtFaxExtension.Visible = True
		' 
		' txtPhoneExtension
		' 
		Me.txtPhoneExtension.AcceptsReturn = True
		Me.txtPhoneExtension.AutoSize = False
		Me.txtPhoneExtension.BackColor = System.Drawing.SystemColors.Window
		Me.txtPhoneExtension.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPhoneExtension.CausesValidation = True
		Me.txtPhoneExtension.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPhoneExtension.Enabled = True
		Me.txtPhoneExtension.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPhoneExtension.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPhoneExtension.HideSelection = True
		Me.txtPhoneExtension.Location = New System.Drawing.Point(328, 172)
		Me.txtPhoneExtension.MaxLength = 0
		Me.txtPhoneExtension.Multiline = False
		Me.txtPhoneExtension.Name = "txtPhoneExtension"
		Me.txtPhoneExtension.ReadOnly = False
		Me.txtPhoneExtension.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPhoneExtension.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPhoneExtension.Size = New System.Drawing.Size(81, 19)
		Me.txtPhoneExtension.TabIndex = 13
		Me.txtPhoneExtension.TabStop = True
		Me.txtPhoneExtension.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPhoneExtension.Visible = True
		' 
		' txtAddress3
		' 
		Me.txtAddress3.AcceptsReturn = True
		Me.txtAddress3.AutoSize = False
		Me.txtAddress3.BackColor = System.Drawing.SystemColors.Window
		Me.txtAddress3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtAddress3.CausesValidation = True
		Me.txtAddress3.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAddress3.Enabled = True
		Me.txtAddress3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtAddress3.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAddress3.HideSelection = True
		Me.txtAddress3.Location = New System.Drawing.Point(120, 60)
		Me.txtAddress3.MaxLength = 0
		Me.txtAddress3.Multiline = False
		Me.txtAddress3.Name = "txtAddress3"
		Me.txtAddress3.ReadOnly = False
		Me.txtAddress3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAddress3.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAddress3.Size = New System.Drawing.Size(289, 19)
		Me.txtAddress3.TabIndex = 7
		Me.txtAddress3.TabStop = True
		Me.txtAddress3.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtAddress3.Visible = True
		' 
		' txtAddress1
		' 
		Me.txtAddress1.AcceptsReturn = True
		Me.txtAddress1.AutoSize = False
		Me.txtAddress1.BackColor = System.Drawing.SystemColors.Window
		Me.txtAddress1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtAddress1.CausesValidation = True
		Me.txtAddress1.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAddress1.Enabled = True
		Me.txtAddress1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtAddress1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAddress1.HideSelection = True
		Me.txtAddress1.Location = New System.Drawing.Point(120, 20)
		Me.txtAddress1.MaxLength = 0
		Me.txtAddress1.Multiline = False
		Me.txtAddress1.Name = "txtAddress1"
		Me.txtAddress1.ReadOnly = False
		Me.txtAddress1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAddress1.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAddress1.Size = New System.Drawing.Size(289, 19)
		Me.txtAddress1.TabIndex = 5
		Me.txtAddress1.TabStop = True
		Me.txtAddress1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtAddress1.Visible = True
		' 
		' txtAddress2
		' 
		Me.txtAddress2.AcceptsReturn = True
		Me.txtAddress2.AutoSize = False
		Me.txtAddress2.BackColor = System.Drawing.SystemColors.Window
		Me.txtAddress2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtAddress2.CausesValidation = True
		Me.txtAddress2.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAddress2.Enabled = True
		Me.txtAddress2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtAddress2.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAddress2.HideSelection = True
		Me.txtAddress2.Location = New System.Drawing.Point(120, 40)
		Me.txtAddress2.MaxLength = 0
		Me.txtAddress2.Multiline = False
		Me.txtAddress2.Name = "txtAddress2"
		Me.txtAddress2.ReadOnly = False
		Me.txtAddress2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAddress2.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAddress2.Size = New System.Drawing.Size(289, 19)
		Me.txtAddress2.TabIndex = 6
		Me.txtAddress2.TabStop = True
		Me.txtAddress2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtAddress2.Visible = True
		' 
		' txtAddress4
		' 
		Me.txtAddress4.AcceptsReturn = True
		Me.txtAddress4.AutoSize = False
		Me.txtAddress4.BackColor = System.Drawing.SystemColors.Window
		Me.txtAddress4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtAddress4.CausesValidation = True
		Me.txtAddress4.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAddress4.Enabled = True
		Me.txtAddress4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtAddress4.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAddress4.HideSelection = True
		Me.txtAddress4.Location = New System.Drawing.Point(120, 80)
		Me.txtAddress4.MaxLength = 0
		Me.txtAddress4.Multiline = False
		Me.txtAddress4.Name = "txtAddress4"
		Me.txtAddress4.ReadOnly = False
		Me.txtAddress4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAddress4.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAddress4.Size = New System.Drawing.Size(289, 19)
		Me.txtAddress4.TabIndex = 8
		Me.txtAddress4.TabStop = True
		Me.txtAddress4.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtAddress4.Visible = True
		' 
		' txtPostalCode
		' 
		Me.txtPostalCode.AcceptsReturn = True
		Me.txtPostalCode.AutoSize = False
		Me.txtPostalCode.BackColor = System.Drawing.SystemColors.Window
		Me.txtPostalCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPostalCode.CausesValidation = True
		Me.txtPostalCode.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPostalCode.Enabled = True
		Me.txtPostalCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPostalCode.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPostalCode.HideSelection = True
		Me.txtPostalCode.Location = New System.Drawing.Point(120, 100)
		Me.txtPostalCode.MaxLength = 0
		Me.txtPostalCode.Multiline = False
		Me.txtPostalCode.Name = "txtPostalCode"
		Me.txtPostalCode.ReadOnly = False
		Me.txtPostalCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPostalCode.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPostalCode.Size = New System.Drawing.Size(153, 19)
		Me.txtPostalCode.TabIndex = 9
		Me.txtPostalCode.TabStop = True
		Me.txtPostalCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPostalCode.Visible = True
		' 
		' cmbCountry
		' 
		Me.cmbCountry.BackColor = System.Drawing.SystemColors.Window
		Me.cmbCountry.CausesValidation = True
		Me.cmbCountry.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmbCountry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cmbCountry.Enabled = True
		Me.cmbCountry.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmbCountry.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cmbCountry.IntegralHeight = True
		Me.cmbCountry.Location = New System.Drawing.Point(120, 120)
		Me.cmbCountry.Name = "cmbCountry"
		Me.cmbCountry.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmbCountry.Size = New System.Drawing.Size(289, 21)
		Me.cmbCountry.Sorted = False
		Me.cmbCountry.TabIndex = 10
		Me.cmbCountry.TabStop = True
		Me.cmbCountry.Visible = True
		' 
		' txtPhoneAreaCode
		' 
		Me.txtPhoneAreaCode.AcceptsReturn = True
		Me.txtPhoneAreaCode.AutoSize = False
		Me.txtPhoneAreaCode.BackColor = System.Drawing.SystemColors.Window
		Me.txtPhoneAreaCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPhoneAreaCode.CausesValidation = True
		Me.txtPhoneAreaCode.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPhoneAreaCode.Enabled = True
		Me.txtPhoneAreaCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPhoneAreaCode.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPhoneAreaCode.HideSelection = True
		Me.txtPhoneAreaCode.Location = New System.Drawing.Point(120, 172)
		Me.txtPhoneAreaCode.MaxLength = 0
		Me.txtPhoneAreaCode.Multiline = False
		Me.txtPhoneAreaCode.Name = "txtPhoneAreaCode"
		Me.txtPhoneAreaCode.ReadOnly = False
		Me.txtPhoneAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPhoneAreaCode.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPhoneAreaCode.Size = New System.Drawing.Size(81, 19)
		Me.txtPhoneAreaCode.TabIndex = 11
		Me.txtPhoneAreaCode.TabStop = True
		Me.txtPhoneAreaCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPhoneAreaCode.Visible = True
		' 
		' txtPhoneNumber
		' 
		Me.txtPhoneNumber.AcceptsReturn = True
		Me.txtPhoneNumber.AutoSize = False
		Me.txtPhoneNumber.BackColor = System.Drawing.SystemColors.Window
		Me.txtPhoneNumber.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPhoneNumber.CausesValidation = True
		Me.txtPhoneNumber.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPhoneNumber.Enabled = True
		Me.txtPhoneNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPhoneNumber.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPhoneNumber.HideSelection = True
		Me.txtPhoneNumber.Location = New System.Drawing.Point(205, 172)
		Me.txtPhoneNumber.MaxLength = 0
		Me.txtPhoneNumber.Multiline = False
		Me.txtPhoneNumber.Name = "txtPhoneNumber"
		Me.txtPhoneNumber.ReadOnly = False
		Me.txtPhoneNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPhoneNumber.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPhoneNumber.Size = New System.Drawing.Size(121, 19)
		Me.txtPhoneNumber.TabIndex = 12
		Me.txtPhoneNumber.TabStop = True
		Me.txtPhoneNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPhoneNumber.Visible = True
		' 
		' txtFaxAreaCode
		' 
		Me.txtFaxAreaCode.AcceptsReturn = True
		Me.txtFaxAreaCode.AutoSize = False
		Me.txtFaxAreaCode.BackColor = System.Drawing.SystemColors.Window
		Me.txtFaxAreaCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtFaxAreaCode.CausesValidation = True
		Me.txtFaxAreaCode.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtFaxAreaCode.Enabled = True
		Me.txtFaxAreaCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtFaxAreaCode.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtFaxAreaCode.HideSelection = True
		Me.txtFaxAreaCode.Location = New System.Drawing.Point(120, 196)
		Me.txtFaxAreaCode.MaxLength = 0
		Me.txtFaxAreaCode.Multiline = False
		Me.txtFaxAreaCode.Name = "txtFaxAreaCode"
		Me.txtFaxAreaCode.ReadOnly = False
		Me.txtFaxAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtFaxAreaCode.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtFaxAreaCode.Size = New System.Drawing.Size(81, 19)
		Me.txtFaxAreaCode.TabIndex = 14
		Me.txtFaxAreaCode.TabStop = True
		Me.txtFaxAreaCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtFaxAreaCode.Visible = True
		' 
		' txtFaxNumber
		' 
		Me.txtFaxNumber.AcceptsReturn = True
		Me.txtFaxNumber.AutoSize = False
		Me.txtFaxNumber.BackColor = System.Drawing.SystemColors.Window
		Me.txtFaxNumber.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtFaxNumber.CausesValidation = True
		Me.txtFaxNumber.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtFaxNumber.Enabled = True
		Me.txtFaxNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtFaxNumber.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtFaxNumber.HideSelection = True
		Me.txtFaxNumber.Location = New System.Drawing.Point(205, 196)
		Me.txtFaxNumber.MaxLength = 0
		Me.txtFaxNumber.Multiline = False
		Me.txtFaxNumber.Name = "txtFaxNumber"
		Me.txtFaxNumber.ReadOnly = False
		Me.txtFaxNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtFaxNumber.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtFaxNumber.Size = New System.Drawing.Size(121, 19)
		Me.txtFaxNumber.TabIndex = 15
		Me.txtFaxNumber.TabStop = True
		Me.txtFaxNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtFaxNumber.Visible = True
		' 
		' lblExtension
		' 
		Me.lblExtension.AutoSize = False
		Me.lblExtension.BackColor = System.Drawing.SystemColors.Control
		Me.lblExtension.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblExtension.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblExtension.Enabled = True
		Me.lblExtension.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblExtension.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblExtension.Location = New System.Drawing.Point(331, 156)
		Me.lblExtension.Name = "lblExtension"
		Me.lblExtension.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblExtension.Size = New System.Drawing.Size(73, 17)
		Me.lblExtension.TabIndex = 34
		Me.lblExtension.Text = "Extension"
		Me.lblExtension.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblExtension.UseMnemonic = True
		Me.lblExtension.Visible = True
		' 
		' lblAddress1
		' 
		Me.lblAddress1.AutoSize = False
		Me.lblAddress1.BackColor = System.Drawing.SystemColors.Control
		Me.lblAddress1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAddress1.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAddress1.Enabled = True
		Me.lblAddress1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAddress1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAddress1.Location = New System.Drawing.Point(16, 23)
		Me.lblAddress1.Name = "lblAddress1"
		Me.lblAddress1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAddress1.Size = New System.Drawing.Size(97, 17)
		Me.lblAddress1.TabIndex = 26
		Me.lblAddress1.Text = "Line &1:"
		Me.lblAddress1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAddress1.UseMnemonic = True
		Me.lblAddress1.Visible = True
		' 
		' lblAddress2
		' 
		Me.lblAddress2.AutoSize = False
		Me.lblAddress2.BackColor = System.Drawing.SystemColors.Control
		Me.lblAddress2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAddress2.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAddress2.Enabled = True
		Me.lblAddress2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAddress2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAddress2.Location = New System.Drawing.Point(16, 43)
		Me.lblAddress2.Name = "lblAddress2"
		Me.lblAddress2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAddress2.Size = New System.Drawing.Size(97, 17)
		Me.lblAddress2.TabIndex = 27
		Me.lblAddress2.Text = "Line &2:"
		Me.lblAddress2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAddress2.UseMnemonic = True
		Me.lblAddress2.Visible = True
		' 
		' lblAddress3
		' 
		Me.lblAddress3.AutoSize = False
		Me.lblAddress3.BackColor = System.Drawing.SystemColors.Control
		Me.lblAddress3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAddress3.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAddress3.Enabled = True
		Me.lblAddress3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAddress3.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAddress3.Location = New System.Drawing.Point(16, 63)
		Me.lblAddress3.Name = "lblAddress3"
		Me.lblAddress3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAddress3.Size = New System.Drawing.Size(97, 17)
		Me.lblAddress3.TabIndex = 28
		Me.lblAddress3.Text = "&Town:"
		Me.lblAddress3.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAddress3.UseMnemonic = True
		Me.lblAddress3.Visible = True
		' 
		' lblAddress4
		' 
		Me.lblAddress4.AutoSize = False
		Me.lblAddress4.BackColor = System.Drawing.SystemColors.Control
		Me.lblAddress4.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAddress4.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAddress4.Enabled = True
		Me.lblAddress4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAddress4.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAddress4.Location = New System.Drawing.Point(16, 83)
		Me.lblAddress4.Name = "lblAddress4"
		Me.lblAddress4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAddress4.Size = New System.Drawing.Size(97, 17)
		Me.lblAddress4.TabIndex = 29
		Me.lblAddress4.Text = "&Region:"
		Me.lblAddress4.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAddress4.UseMnemonic = True
		Me.lblAddress4.Visible = True
		' 
		' lblPostalCode
		' 
		Me.lblPostalCode.AutoSize = False
		Me.lblPostalCode.BackColor = System.Drawing.SystemColors.Control
		Me.lblPostalCode.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPostalCode.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPostalCode.Enabled = True
		Me.lblPostalCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPostalCode.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPostalCode.Location = New System.Drawing.Point(16, 103)
		Me.lblPostalCode.Name = "lblPostalCode"
		Me.lblPostalCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPostalCode.Size = New System.Drawing.Size(97, 17)
		Me.lblPostalCode.TabIndex = 30
		Me.lblPostalCode.Text = "&Postal Code:"
		Me.lblPostalCode.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPostalCode.UseMnemonic = True
		Me.lblPostalCode.Visible = True
		' 
		' lblCountry
		' 
		Me.lblCountry.AutoSize = False
		Me.lblCountry.BackColor = System.Drawing.SystemColors.Control
		Me.lblCountry.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCountry.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCountry.Enabled = True
		Me.lblCountry.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCountry.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCountry.Location = New System.Drawing.Point(16, 123)
		Me.lblCountry.Name = "lblCountry"
		Me.lblCountry.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCountry.Size = New System.Drawing.Size(97, 17)
		Me.lblCountry.TabIndex = 31
		Me.lblCountry.Text = "&Country:"
		Me.lblCountry.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCountry.UseMnemonic = True
		Me.lblCountry.Visible = True
		' 
		' lblPhone
		' 
		Me.lblPhone.AutoSize = False
		Me.lblPhone.BackColor = System.Drawing.SystemColors.Control
		Me.lblPhone.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPhone.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPhone.Enabled = True
		Me.lblPhone.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPhone.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPhone.Location = New System.Drawing.Point(16, 175)
		Me.lblPhone.Name = "lblPhone"
		Me.lblPhone.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPhone.Size = New System.Drawing.Size(89, 17)
		Me.lblPhone.TabIndex = 35
		Me.lblPhone.Text = "T&elephone No:"
		Me.lblPhone.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPhone.UseMnemonic = True
		Me.lblPhone.Visible = True
		' 
		' lblFax
		' 
		Me.lblFax.AutoSize = False
		Me.lblFax.BackColor = System.Drawing.SystemColors.Control
		Me.lblFax.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblFax.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblFax.Enabled = True
		Me.lblFax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblFax.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblFax.Location = New System.Drawing.Point(16, 199)
		Me.lblFax.Name = "lblFax"
		Me.lblFax.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblFax.Size = New System.Drawing.Size(97, 17)
		Me.lblFax.TabIndex = 36
		Me.lblFax.Text = "&Fax No:"
		Me.lblFax.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblFax.UseMnemonic = True
		Me.lblFax.Visible = True
		' 
		' lblAreaCode
		' 
		Me.lblAreaCode.AutoSize = False
		Me.lblAreaCode.BackColor = System.Drawing.SystemColors.Control
		Me.lblAreaCode.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAreaCode.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAreaCode.Enabled = True
		Me.lblAreaCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAreaCode.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAreaCode.Location = New System.Drawing.Point(123, 156)
		Me.lblAreaCode.Name = "lblAreaCode"
		Me.lblAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAreaCode.Size = New System.Drawing.Size(73, 17)
		Me.lblAreaCode.TabIndex = 32
		Me.lblAreaCode.Text = "Area Code"
		Me.lblAreaCode.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAreaCode.UseMnemonic = True
		Me.lblAreaCode.Visible = True
		' 
		' lblNumber
		' 
		Me.lblNumber.AutoSize = False
		Me.lblNumber.BackColor = System.Drawing.SystemColors.Control
		Me.lblNumber.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblNumber.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblNumber.Enabled = True
		Me.lblNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblNumber.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblNumber.Location = New System.Drawing.Point(208, 156)
		Me.lblNumber.Name = "lblNumber"
		Me.lblNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblNumber.Size = New System.Drawing.Size(73, 17)
		Me.lblNumber.TabIndex = 33
		Me.lblNumber.Text = "Number"
		Me.lblNumber.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblNumber.UseMnemonic = True
		Me.lblNumber.Visible = True
		' 
		' _tabMainTab_TabPage2
		' 
		Me._tabMainTab_TabPage2.Controls.Add(Me.txtDefaultIndicator)
		Me._tabMainTab_TabPage2.Controls.Add(Me.txtPMCompanyNumber)
		Me._tabMainTab_TabPage2.Controls.Add(Me.txtUserLicenceId)
		Me._tabMainTab_TabPage2.Controls.Add(Me.txtBrokerABIId)
		Me._tabMainTab_TabPage2.Controls.Add(Me.txtSenderMailboxId)
		Me._tabMainTab_TabPage2.Controls.Add(Me.txtVatNo)
		Me._tabMainTab_TabPage2.Controls.Add(Me.txtEmail)
		Me._tabMainTab_TabPage2.Controls.Add(Me.lblDefaultIndicator)
		Me._tabMainTab_TabPage2.Controls.Add(Me.lblPMCompanyNumber)
		Me._tabMainTab_TabPage2.Controls.Add(Me.lblUserLicenceId)
		Me._tabMainTab_TabPage2.Controls.Add(Me.lblBrokerABIId)
		Me._tabMainTab_TabPage2.Controls.Add(Me.lblSenderMailboxId)
		Me._tabMainTab_TabPage2.Controls.Add(Me.lblVatNo)
		Me._tabMainTab_TabPage2.Controls.Add(Me.lblEmail)
		Me._tabMainTab_TabPage2.Text = "&3 - Other Details"
		' 
		' txtDefaultIndicator
		' 
		Me.txtDefaultIndicator.AcceptsReturn = True
		Me.txtDefaultIndicator.AutoSize = False
		Me.txtDefaultIndicator.BackColor = System.Drawing.SystemColors.Window
		Me.txtDefaultIndicator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDefaultIndicator.CausesValidation = True
		Me.txtDefaultIndicator.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDefaultIndicator.Enabled = True
		Me.txtDefaultIndicator.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDefaultIndicator.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDefaultIndicator.HideSelection = True
		Me.txtDefaultIndicator.Location = New System.Drawing.Point(120, 212)
		Me.txtDefaultIndicator.MaxLength = 0
		Me.txtDefaultIndicator.Multiline = False
		Me.txtDefaultIndicator.Name = "txtDefaultIndicator"
		Me.txtDefaultIndicator.ReadOnly = False
		Me.txtDefaultIndicator.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDefaultIndicator.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDefaultIndicator.Size = New System.Drawing.Size(25, 19)
		Me.txtDefaultIndicator.TabIndex = 52
		Me.txtDefaultIndicator.TabStop = True
		Me.txtDefaultIndicator.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDefaultIndicator.Visible = True
		' 
		' txtPMCompanyNumber
		' 
		Me.txtPMCompanyNumber.AcceptsReturn = True
		Me.txtPMCompanyNumber.AutoSize = False
		Me.txtPMCompanyNumber.BackColor = System.Drawing.SystemColors.Window
		Me.txtPMCompanyNumber.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPMCompanyNumber.CausesValidation = True
		Me.txtPMCompanyNumber.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPMCompanyNumber.Enabled = True
		Me.txtPMCompanyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPMCompanyNumber.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPMCompanyNumber.HideSelection = True
		Me.txtPMCompanyNumber.Location = New System.Drawing.Point(120, 180)
		Me.txtPMCompanyNumber.MaxLength = 0
		Me.txtPMCompanyNumber.Multiline = False
		Me.txtPMCompanyNumber.Name = "txtPMCompanyNumber"
		Me.txtPMCompanyNumber.ReadOnly = False
		Me.txtPMCompanyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPMCompanyNumber.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPMCompanyNumber.Size = New System.Drawing.Size(25, 19)
		Me.txtPMCompanyNumber.TabIndex = 50
		Me.txtPMCompanyNumber.TabStop = True
		Me.txtPMCompanyNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPMCompanyNumber.Visible = True
		' 
		' txtUserLicenceId
		' 
		Me.txtUserLicenceId.AcceptsReturn = True
		Me.txtUserLicenceId.AutoSize = False
		Me.txtUserLicenceId.BackColor = System.Drawing.SystemColors.Window
		Me.txtUserLicenceId.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtUserLicenceId.CausesValidation = True
		Me.txtUserLicenceId.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtUserLicenceId.Enabled = True
		Me.txtUserLicenceId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtUserLicenceId.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtUserLicenceId.HideSelection = True
		Me.txtUserLicenceId.Location = New System.Drawing.Point(120, 148)
		Me.txtUserLicenceId.MaxLength = 0
		Me.txtUserLicenceId.Multiline = False
		Me.txtUserLicenceId.Name = "txtUserLicenceId"
		Me.txtUserLicenceId.ReadOnly = False
		Me.txtUserLicenceId.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtUserLicenceId.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtUserLicenceId.Size = New System.Drawing.Size(33, 19)
		Me.txtUserLicenceId.TabIndex = 48
		Me.txtUserLicenceId.TabStop = True
		Me.txtUserLicenceId.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtUserLicenceId.Visible = True
		' 
		' txtBrokerABIId
		' 
		Me.txtBrokerABIId.AcceptsReturn = True
		Me.txtBrokerABIId.AutoSize = False
		Me.txtBrokerABIId.BackColor = System.Drawing.SystemColors.Window
		Me.txtBrokerABIId.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtBrokerABIId.CausesValidation = True
		Me.txtBrokerABIId.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtBrokerABIId.Enabled = True
		Me.txtBrokerABIId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtBrokerABIId.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtBrokerABIId.HideSelection = True
		Me.txtBrokerABIId.Location = New System.Drawing.Point(120, 116)
		Me.txtBrokerABIId.MaxLength = 0
		Me.txtBrokerABIId.Multiline = False
		Me.txtBrokerABIId.Name = "txtBrokerABIId"
		Me.txtBrokerABIId.ReadOnly = False
		Me.txtBrokerABIId.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtBrokerABIId.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtBrokerABIId.Size = New System.Drawing.Size(49, 19)
		Me.txtBrokerABIId.TabIndex = 46
		Me.txtBrokerABIId.TabStop = True
		Me.txtBrokerABIId.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtBrokerABIId.Visible = True
		' 
		' txtSenderMailboxId
		' 
		Me.txtSenderMailboxId.AcceptsReturn = True
		Me.txtSenderMailboxId.AutoSize = False
		Me.txtSenderMailboxId.BackColor = System.Drawing.SystemColors.Window
		Me.txtSenderMailboxId.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtSenderMailboxId.CausesValidation = True
		Me.txtSenderMailboxId.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtSenderMailboxId.Enabled = True
		Me.txtSenderMailboxId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtSenderMailboxId.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtSenderMailboxId.HideSelection = True
		Me.txtSenderMailboxId.Location = New System.Drawing.Point(120, 84)
		Me.txtSenderMailboxId.MaxLength = 0
		Me.txtSenderMailboxId.Multiline = False
		Me.txtSenderMailboxId.Name = "txtSenderMailboxId"
		Me.txtSenderMailboxId.ReadOnly = False
		Me.txtSenderMailboxId.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtSenderMailboxId.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtSenderMailboxId.Size = New System.Drawing.Size(97, 19)
		Me.txtSenderMailboxId.TabIndex = 44
		Me.txtSenderMailboxId.TabStop = True
		Me.txtSenderMailboxId.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtSenderMailboxId.Visible = True
		' 
		' txtVatNo
		' 
		Me.txtVatNo.AcceptsReturn = True
		Me.txtVatNo.AutoSize = False
		Me.txtVatNo.BackColor = System.Drawing.SystemColors.Window
		Me.txtVatNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtVatNo.CausesValidation = True
		Me.txtVatNo.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtVatNo.Enabled = True
		Me.txtVatNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtVatNo.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtVatNo.HideSelection = True
		Me.txtVatNo.Location = New System.Drawing.Point(120, 52)
		Me.txtVatNo.MaxLength = 0
		Me.txtVatNo.Multiline = False
		Me.txtVatNo.Name = "txtVatNo"
		Me.txtVatNo.ReadOnly = False
		Me.txtVatNo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtVatNo.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtVatNo.Size = New System.Drawing.Size(129, 19)
		Me.txtVatNo.TabIndex = 42
		Me.txtVatNo.TabStop = True
		Me.txtVatNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtVatNo.Visible = True
		' 
		' txtEmail
		' 
		Me.txtEmail.AcceptsReturn = True
		Me.txtEmail.AutoSize = False
		Me.txtEmail.BackColor = System.Drawing.SystemColors.Window
		Me.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtEmail.CausesValidation = True
		Me.txtEmail.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtEmail.Enabled = True
		Me.txtEmail.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtEmail.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtEmail.HideSelection = True
		Me.txtEmail.Location = New System.Drawing.Point(120, 20)
		Me.txtEmail.MaxLength = 0
		Me.txtEmail.Multiline = False
		Me.txtEmail.Name = "txtEmail"
		Me.txtEmail.ReadOnly = False
		Me.txtEmail.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtEmail.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtEmail.Size = New System.Drawing.Size(313, 19)
		Me.txtEmail.TabIndex = 39
		Me.txtEmail.TabStop = True
		Me.txtEmail.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtEmail.Visible = True
		' 
		' lblDefaultIndicator
		' 
		Me.lblDefaultIndicator.AutoSize = False
		Me.lblDefaultIndicator.BackColor = System.Drawing.SystemColors.Control
		Me.lblDefaultIndicator.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDefaultIndicator.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDefaultIndicator.Enabled = True
		Me.lblDefaultIndicator.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDefaultIndicator.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDefaultIndicator.Location = New System.Drawing.Point(16, 212)
		Me.lblDefaultIndicator.Name = "lblDefaultIndicator"
		Me.lblDefaultIndicator.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDefaultIndicator.Size = New System.Drawing.Size(105, 17)
		Me.lblDefaultIndicator.TabIndex = 51
		Me.lblDefaultIndicator.Text = "&Default Indicator:"
		Me.lblDefaultIndicator.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDefaultIndicator.UseMnemonic = True
		Me.lblDefaultIndicator.Visible = True
		' 
		' lblPMCompanyNumber
		' 
		Me.lblPMCompanyNumber.AutoSize = False
		Me.lblPMCompanyNumber.BackColor = System.Drawing.SystemColors.Control
		Me.lblPMCompanyNumber.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPMCompanyNumber.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPMCompanyNumber.Enabled = True
		Me.lblPMCompanyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPMCompanyNumber.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPMCompanyNumber.Location = New System.Drawing.Point(16, 180)
		Me.lblPMCompanyNumber.Name = "lblPMCompanyNumber"
		Me.lblPMCompanyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPMCompanyNumber.Size = New System.Drawing.Size(105, 17)
		Me.lblPMCompanyNumber.TabIndex = 49
		Me.lblPMCompanyNumber.Text = "&PM Company No:"
		Me.lblPMCompanyNumber.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPMCompanyNumber.UseMnemonic = True
		Me.lblPMCompanyNumber.Visible = True
		' 
		' lblUserLicenceId
		' 
		Me.lblUserLicenceId.AutoSize = False
		Me.lblUserLicenceId.BackColor = System.Drawing.SystemColors.Control
		Me.lblUserLicenceId.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblUserLicenceId.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblUserLicenceId.Enabled = True
		Me.lblUserLicenceId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblUserLicenceId.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblUserLicenceId.Location = New System.Drawing.Point(16, 148)
		Me.lblUserLicenceId.Name = "lblUserLicenceId"
		Me.lblUserLicenceId.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblUserLicenceId.Size = New System.Drawing.Size(105, 17)
		Me.lblUserLicenceId.TabIndex = 47
		Me.lblUserLicenceId.Text = "&User Licence Id:"
		Me.lblUserLicenceId.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblUserLicenceId.UseMnemonic = True
		Me.lblUserLicenceId.Visible = True
		' 
		' lblBrokerABIId
		' 
		Me.lblBrokerABIId.AutoSize = False
		Me.lblBrokerABIId.BackColor = System.Drawing.SystemColors.Control
		Me.lblBrokerABIId.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblBrokerABIId.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblBrokerABIId.Enabled = True
		Me.lblBrokerABIId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblBrokerABIId.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblBrokerABIId.Location = New System.Drawing.Point(16, 116)
		Me.lblBrokerABIId.Name = "lblBrokerABIId"
		Me.lblBrokerABIId.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblBrokerABIId.Size = New System.Drawing.Size(105, 17)
		Me.lblBrokerABIId.TabIndex = 45
		Me.lblBrokerABIId.Text = "&Broker ABI Id:"
		Me.lblBrokerABIId.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblBrokerABIId.UseMnemonic = True
		Me.lblBrokerABIId.Visible = True
		' 
		' lblSenderMailboxId
		' 
		Me.lblSenderMailboxId.AutoSize = False
		Me.lblSenderMailboxId.BackColor = System.Drawing.SystemColors.Control
		Me.lblSenderMailboxId.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSenderMailboxId.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSenderMailboxId.Enabled = True
		Me.lblSenderMailboxId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSenderMailboxId.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSenderMailboxId.Location = New System.Drawing.Point(16, 84)
		Me.lblSenderMailboxId.Name = "lblSenderMailboxId"
		Me.lblSenderMailboxId.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSenderMailboxId.Size = New System.Drawing.Size(105, 17)
		Me.lblSenderMailboxId.TabIndex = 43
		Me.lblSenderMailboxId.Text = "&Sender Mailbox Id:"
		Me.lblSenderMailboxId.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblSenderMailboxId.UseMnemonic = True
		Me.lblSenderMailboxId.Visible = True
		' 
		' lblVatNo
		' 
		Me.lblVatNo.AutoSize = False
		Me.lblVatNo.BackColor = System.Drawing.SystemColors.Control
		Me.lblVatNo.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblVatNo.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblVatNo.Enabled = True
		Me.lblVatNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblVatNo.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblVatNo.Location = New System.Drawing.Point(16, 52)
		Me.lblVatNo.Name = "lblVatNo"
		Me.lblVatNo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblVatNo.Size = New System.Drawing.Size(105, 17)
		Me.lblVatNo.TabIndex = 41
		Me.lblVatNo.Text = "&Vat Number:"
		Me.lblVatNo.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblVatNo.UseMnemonic = True
		Me.lblVatNo.Visible = True
		' 
		' lblEmail
		' 
		Me.lblEmail.AutoSize = False
		Me.lblEmail.BackColor = System.Drawing.SystemColors.Control
		Me.lblEmail.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblEmail.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblEmail.Enabled = True
		Me.lblEmail.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblEmail.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblEmail.Location = New System.Drawing.Point(16, 20)
		Me.lblEmail.Name = "lblEmail"
		Me.lblEmail.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblEmail.Size = New System.Drawing.Size(105, 17)
		Me.lblEmail.TabIndex = 40
		Me.lblEmail.Text = "&E-Mail:"
		Me.lblEmail.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblEmail.UseMnemonic = True
		Me.lblEmail.Visible = True
		' 
		' frmDetails
		' 
		Me.AcceptButton = Me._cmdNext_1
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(465, 315)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdNavigate)
		Me.Controls.Add(Me.cmdHelp)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.tabMainTab)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = True
		Me.Icon = CType(resources.GetObject("frmDetails.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(203, 163)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmDetails"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Company"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.tabMainTab.ResumeLayout(False)
		Me._tabMainTab_TabPage0.ResumeLayout(False)
		Me._tabMainTab_TabPage1.ResumeLayout(False)
		Me._tabMainTab_TabPage2.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub InitializecmdNext()
		Me.cmdNext(1) = _cmdNext_1
		Me.cmdNext(0) = _cmdNext_0
	End Sub
#End Region 
End Class