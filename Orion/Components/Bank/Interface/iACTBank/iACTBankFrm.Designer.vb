<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializecmdPrevious()
		InitializecmdNext()
		lvwAccounts_InitializeColumnKeys()
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
    Public WithEvents cmdNavigate As System.Windows.Forms.Button
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents imgIcon As System.Windows.Forms.PictureBox
	Public WithEvents lblBranchCode As System.Windows.Forms.Label
	Public WithEvents lblName As System.Windows.Forms.Label
	Public WithEvents lblCode As System.Windows.Forms.Label
	Public WithEvents lblExtension As System.Windows.Forms.Label
	Public WithEvents lblAddress1 As System.Windows.Forms.Label
	Public WithEvents lblAddress2 As System.Windows.Forms.Label
	Public WithEvents lblAddress3 As System.Windows.Forms.Label
	Public WithEvents lblAddress4 As System.Windows.Forms.Label
	Public WithEvents lblPostalCode As System.Windows.Forms.Label
	Public WithEvents lblAddressCountry As System.Windows.Forms.Label
	Public WithEvents lblTelephone As System.Windows.Forms.Label
	Public WithEvents lblFax As System.Windows.Forms.Label
	Public WithEvents lblAreaCode As System.Windows.Forms.Label
	Public WithEvents lblNumber As System.Windows.Forms.Label
	Public WithEvents lblBankAccountType As System.Windows.Forms.Label
	Public WithEvents cboBankAccountType As PMLookupControl.cboPMLookup
	Public WithEvents txtBranchCode As System.Windows.Forms.TextBox
	Public WithEvents txtBankName As System.Windows.Forms.TextBox
	Public WithEvents txtCode As System.Windows.Forms.TextBox
	Public WithEvents txtPhoneExtension As System.Windows.Forms.TextBox
	Public WithEvents txtAddress3 As System.Windows.Forms.TextBox
	Public WithEvents txtAddress1 As System.Windows.Forms.TextBox
	Public WithEvents txtAddress2 As System.Windows.Forms.TextBox
	Public WithEvents txtAddress4 As System.Windows.Forms.TextBox
	Public WithEvents cboAddressCountry As System.Windows.Forms.ComboBox
	Public WithEvents txtPhoneAreaCode As System.Windows.Forms.TextBox
	Public WithEvents txtPhoneNumber As System.Windows.Forms.TextBox
	Public WithEvents txtFaxAreaCode As System.Windows.Forms.TextBox
	Public WithEvents txtFaxNumber As System.Windows.Forms.TextBox
	Private WithEvents _cmdNext_0 As System.Windows.Forms.Button
	Public WithEvents txtPostalCode As System.Windows.Forms.TextBox
	Public WithEvents cmdHeadOffice As System.Windows.Forms.Button
	Public WithEvents pnlHeadOffice As System.Windows.Forms.Panel
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Private WithEvents _cmdPrevious_0 As System.Windows.Forms.Button
	Public WithEvents txtComments As System.Windows.Forms.TextBox
	Private WithEvents _cmdNext_1 As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
	Private WithEvents _cmdPrevious_1 As System.Windows.Forms.Button
	Public WithEvents cmdEditAcc As System.Windows.Forms.Button
	Public WithEvents cmdDeleteAcc As System.Windows.Forms.Button
	Public WithEvents cmdAddAcc As System.Windows.Forms.Button
	Private WithEvents _lvwAccounts_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAccounts_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAccounts_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAccounts_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAccounts_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAccounts_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwAccounts As System.Windows.Forms.ListView
	Public WithEvents fraAccounts As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents ImageList1 As System.Windows.Forms.ImageList
	Public cmdNext(1) As System.Windows.Forms.Button
	Public cmdPrevious(1) As System.Windows.Forms.Button
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
        Me.cmdNavigate = New System.Windows.Forms.Button
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.imgIcon = New System.Windows.Forms.PictureBox
        Me.lblBranchCode = New System.Windows.Forms.Label
        Me.lblName = New System.Windows.Forms.Label
        Me.lblCode = New System.Windows.Forms.Label
        Me.lblExtension = New System.Windows.Forms.Label
        Me.lblAddress1 = New System.Windows.Forms.Label
        Me.lblAddress2 = New System.Windows.Forms.Label
        Me.lblAddress3 = New System.Windows.Forms.Label
        Me.lblAddress4 = New System.Windows.Forms.Label
        Me.lblPostalCode = New System.Windows.Forms.Label
        Me.lblAddressCountry = New System.Windows.Forms.Label
        Me.lblTelephone = New System.Windows.Forms.Label
        Me.lblFax = New System.Windows.Forms.Label
        Me.lblAreaCode = New System.Windows.Forms.Label
        Me.lblNumber = New System.Windows.Forms.Label
        Me.lblBankAccountType = New System.Windows.Forms.Label
        Me.cboBankAccountType = New PMLookupControl.cboPMLookup
        Me.txtBranchCode = New System.Windows.Forms.TextBox
        Me.txtBankName = New System.Windows.Forms.TextBox
        Me.txtCode = New System.Windows.Forms.TextBox
        Me.txtPhoneExtension = New System.Windows.Forms.TextBox
        Me.txtAddress3 = New System.Windows.Forms.TextBox
        Me.txtAddress1 = New System.Windows.Forms.TextBox
        Me.txtAddress2 = New System.Windows.Forms.TextBox
        Me.txtAddress4 = New System.Windows.Forms.TextBox
        Me.cboAddressCountry = New System.Windows.Forms.ComboBox
        Me.txtPhoneAreaCode = New System.Windows.Forms.TextBox
        Me.txtPhoneNumber = New System.Windows.Forms.TextBox
        Me.txtFaxAreaCode = New System.Windows.Forms.TextBox
        Me.txtFaxNumber = New System.Windows.Forms.TextBox
        Me._cmdNext_0 = New System.Windows.Forms.Button
        Me.txtPostalCode = New System.Windows.Forms.TextBox
        Me.cmdHeadOffice = New System.Windows.Forms.Button
        Me.pnlHeadOffice = New System.Windows.Forms.Panel
        Me.lblHeadOffice = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me._cmdPrevious_0 = New System.Windows.Forms.Button
        Me.txtComments = New System.Windows.Forms.TextBox
        Me._cmdNext_1 = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage
        Me._cmdPrevious_1 = New System.Windows.Forms.Button
        Me.fraAccounts = New System.Windows.Forms.GroupBox
        Me.cmdEditAcc = New System.Windows.Forms.Button
        Me.cmdDeleteAcc = New System.Windows.Forms.Button
        Me.cmdAddAcc = New System.Windows.Forms.Button
        Me.lvwAccounts = New System.Windows.Forms.ListView
        Me._lvwAccounts_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwAccounts_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwAccounts_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwAccounts_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwAccounts_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwAccounts_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlHeadOffice.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me.fraAccounts.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(8, 456)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 25
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        Me.cmdNavigate.Visible = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(232, 456)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 26
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
        Me.tabMainTab.ItemSize = New System.Drawing.Size(152, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(464, 441)
        Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.tabMainTab.TabIndex = 29
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.imgIcon)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblBranchCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblName)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblExtension)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblAddress1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblAddress2)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblAddress3)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblAddress4)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPostalCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblAddressCountry)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblTelephone)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblFax)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblAreaCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblBankAccountType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboBankAccountType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtBranchCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtBankName)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtPhoneExtension)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtAddress3)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtAddress1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtAddress2)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtAddress4)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboAddressCountry)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtPhoneAreaCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtPhoneNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtFaxAreaCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtFaxNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdNext_0)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtPostalCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdHeadOffice)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pnlHeadOffice)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(456, 415)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Bank Details"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'imgIcon
        '
        Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon.Location = New System.Drawing.Point(627, -16)
        Me.imgIcon.Name = "imgIcon"
        Me.imgIcon.Size = New System.Drawing.Size(32, 32)
        Me.imgIcon.TabIndex = 0
        Me.imgIcon.TabStop = False
        '
        'lblBranchCode
        '
        Me.lblBranchCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranchCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranchCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranchCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranchCode.Location = New System.Drawing.Point(16, 109)
        Me.lblBranchCode.Name = "lblBranchCode"
        Me.lblBranchCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranchCode.Size = New System.Drawing.Size(121, 17)
        Me.lblBranchCode.TabIndex = 30
        Me.lblBranchCode.Text = "Branch Code:"
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.BackColor = System.Drawing.SystemColors.Control
        Me.lblName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblName.Location = New System.Drawing.Point(16, 82)
        Me.lblName.Name = "lblName"
        Me.lblName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblName.Size = New System.Drawing.Size(38, 13)
        Me.lblName.TabIndex = 31
        Me.lblName.Text = "Name:"
        '
        'lblCode
        '
        Me.lblCode.AutoSize = True
        Me.lblCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCode.Location = New System.Drawing.Point(16, 51)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCode.Size = New System.Drawing.Size(91, 13)
        Me.lblCode.TabIndex = 32
        Me.lblCode.Text = "Bank Short Code:"
        '
        'lblExtension
        '
        Me.lblExtension.BackColor = System.Drawing.SystemColors.Control
        Me.lblExtension.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExtension.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExtension.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExtension.Location = New System.Drawing.Point(352, 320)
        Me.lblExtension.Name = "lblExtension"
        Me.lblExtension.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExtension.Size = New System.Drawing.Size(81, 17)
        Me.lblExtension.TabIndex = 33
        Me.lblExtension.Text = "Extension"
        Me.lblExtension.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblAddress1
        '
        Me.lblAddress1.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddress1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddress1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddress1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddress1.Location = New System.Drawing.Point(16, 165)
        Me.lblAddress1.Name = "lblAddress1"
        Me.lblAddress1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddress1.Size = New System.Drawing.Size(97, 17)
        Me.lblAddress1.TabIndex = 34
        Me.lblAddress1.Text = "Line &1:"
        '
        'lblAddress2
        '
        Me.lblAddress2.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddress2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddress2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddress2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddress2.Location = New System.Drawing.Point(16, 189)
        Me.lblAddress2.Name = "lblAddress2"
        Me.lblAddress2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddress2.Size = New System.Drawing.Size(97, 17)
        Me.lblAddress2.TabIndex = 35
        Me.lblAddress2.Text = "Line &2:"
        '
        'lblAddress3
        '
        Me.lblAddress3.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddress3.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddress3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddress3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddress3.Location = New System.Drawing.Point(16, 213)
        Me.lblAddress3.Name = "lblAddress3"
        Me.lblAddress3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddress3.Size = New System.Drawing.Size(97, 17)
        Me.lblAddress3.TabIndex = 36
        Me.lblAddress3.Text = "&Town:"
        '
        'lblAddress4
        '
        Me.lblAddress4.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddress4.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddress4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddress4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddress4.Location = New System.Drawing.Point(16, 237)
        Me.lblAddress4.Name = "lblAddress4"
        Me.lblAddress4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddress4.Size = New System.Drawing.Size(97, 17)
        Me.lblAddress4.TabIndex = 37
        Me.lblAddress4.Text = "&Region:"
        '
        'lblPostalCode
        '
        Me.lblPostalCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblPostalCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPostalCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPostalCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPostalCode.Location = New System.Drawing.Point(16, 261)
        Me.lblPostalCode.Name = "lblPostalCode"
        Me.lblPostalCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPostalCode.Size = New System.Drawing.Size(97, 17)
        Me.lblPostalCode.TabIndex = 38
        Me.lblPostalCode.Text = "&Postal Code:"
        '
        'lblAddressCountry
        '
        Me.lblAddressCountry.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddressCountry.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddressCountry.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddressCountry.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddressCountry.Location = New System.Drawing.Point(16, 294)
        Me.lblAddressCountry.Name = "lblAddressCountry"
        Me.lblAddressCountry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddressCountry.Size = New System.Drawing.Size(97, 17)
        Me.lblAddressCountry.TabIndex = 39
        Me.lblAddressCountry.Text = "&Country:"
        '
        'lblTelephone
        '
        Me.lblTelephone.BackColor = System.Drawing.SystemColors.Control
        Me.lblTelephone.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTelephone.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTelephone.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTelephone.Location = New System.Drawing.Point(16, 337)
        Me.lblTelephone.Name = "lblTelephone"
        Me.lblTelephone.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTelephone.Size = New System.Drawing.Size(89, 17)
        Me.lblTelephone.TabIndex = 40
        Me.lblTelephone.Text = "T&elephone No:"
        '
        'lblFax
        '
        Me.lblFax.BackColor = System.Drawing.SystemColors.Control
        Me.lblFax.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFax.Location = New System.Drawing.Point(16, 361)
        Me.lblFax.Name = "lblFax"
        Me.lblFax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFax.Size = New System.Drawing.Size(97, 17)
        Me.lblFax.TabIndex = 41
        Me.lblFax.Text = "&Fax No:"
        '
        'lblAreaCode
        '
        Me.lblAreaCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblAreaCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAreaCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAreaCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAreaCode.Location = New System.Drawing.Point(144, 320)
        Me.lblAreaCode.Name = "lblAreaCode"
        Me.lblAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAreaCode.Size = New System.Drawing.Size(81, 17)
        Me.lblAreaCode.TabIndex = 42
        Me.lblAreaCode.Text = "Area Code"
        Me.lblAreaCode.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblNumber
        '
        Me.lblNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNumber.Location = New System.Drawing.Point(232, 320)
        Me.lblNumber.Name = "lblNumber"
        Me.lblNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNumber.Size = New System.Drawing.Size(113, 17)
        Me.lblNumber.TabIndex = 43
        Me.lblNumber.Text = "Number"
        Me.lblNumber.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblBankAccountType
        '
        Me.lblBankAccountType.AutoSize = True
        Me.lblBankAccountType.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankAccountType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankAccountType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankAccountType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankAccountType.Location = New System.Drawing.Point(16, 20)
        Me.lblBankAccountType.Name = "lblBankAccountType"
        Me.lblBankAccountType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankAccountType.Size = New System.Drawing.Size(105, 13)
        Me.lblBankAccountType.TabIndex = 46
        Me.lblBankAccountType.Text = "Bank Account Type:"
        Me.lblBankAccountType.Visible = False
        '
        'cboBankAccountType
        '
        Me.cboBankAccountType.DefaultItemId = 0
        Me.cboBankAccountType.FirstItem = ""
        Me.cboBankAccountType.ItemId = 0
        Me.cboBankAccountType.ListIndex = -1
        Me.cboBankAccountType.Location = New System.Drawing.Point(144, 20)
        Me.cboBankAccountType.Name = "cboBankAccountType"
        Me.cboBankAccountType.PMLookupProductFamily = 1
        Me.cboBankAccountType.SingleItemId = 0
        Me.cboBankAccountType.Size = New System.Drawing.Size(153, 21)
        Me.cboBankAccountType.Sorted = True
        Me.cboBankAccountType.TabIndex = 0
        Me.cboBankAccountType.TableName = "Bank_Account_Type"
        Me.cboBankAccountType.ToolTipText = ""
        Me.cboBankAccountType.Visible = False
        Me.cboBankAccountType.WhereClause = ""
        '
        'txtBranchCode
        '
        Me.txtBranchCode.AcceptsReturn = True
        Me.txtBranchCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtBranchCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBranchCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBranchCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBranchCode.Location = New System.Drawing.Point(144, 108)
        Me.txtBranchCode.MaxLength = 0
        Me.txtBranchCode.Name = "txtBranchCode"
        Me.txtBranchCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBranchCode.Size = New System.Drawing.Size(161, 20)
        Me.txtBranchCode.TabIndex = 3
        '
        'txtBankName
        '
        Me.txtBankName.AcceptsReturn = True
        Me.txtBankName.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankName.Location = New System.Drawing.Point(144, 79)
        Me.txtBankName.MaxLength = 0
        Me.txtBankName.Name = "txtBankName"
        Me.txtBankName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankName.Size = New System.Drawing.Size(209, 20)
        Me.txtBankName.TabIndex = 2
        '
        'txtCode
        '
        Me.txtCode.AcceptsReturn = True
        Me.txtCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCode.Location = New System.Drawing.Point(144, 48)
        Me.txtCode.MaxLength = 10
        Me.txtCode.Name = "txtCode"
        Me.txtCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCode.Size = New System.Drawing.Size(153, 20)
        Me.txtCode.TabIndex = 1
        '
        'txtPhoneExtension
        '
        Me.txtPhoneExtension.AcceptsReturn = True
        Me.txtPhoneExtension.BackColor = System.Drawing.SystemColors.Window
        Me.txtPhoneExtension.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPhoneExtension.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPhoneExtension.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPhoneExtension.Location = New System.Drawing.Point(352, 336)
        Me.txtPhoneExtension.MaxLength = 0
        Me.txtPhoneExtension.Name = "txtPhoneExtension"
        Me.txtPhoneExtension.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPhoneExtension.Size = New System.Drawing.Size(81, 20)
        Me.txtPhoneExtension.TabIndex = 13
        '
        'txtAddress3
        '
        Me.txtAddress3.AcceptsReturn = True
        Me.txtAddress3.BackColor = System.Drawing.SystemColors.Window
        Me.txtAddress3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAddress3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAddress3.Location = New System.Drawing.Point(144, 212)
        Me.txtAddress3.MaxLength = 0
        Me.txtAddress3.Name = "txtAddress3"
        Me.txtAddress3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAddress3.Size = New System.Drawing.Size(289, 20)
        Me.txtAddress3.TabIndex = 7
        '
        'txtAddress1
        '
        Me.txtAddress1.AcceptsReturn = True
        Me.txtAddress1.BackColor = System.Drawing.SystemColors.Window
        Me.txtAddress1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAddress1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAddress1.Location = New System.Drawing.Point(144, 164)
        Me.txtAddress1.MaxLength = 0
        Me.txtAddress1.Name = "txtAddress1"
        Me.txtAddress1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAddress1.Size = New System.Drawing.Size(289, 20)
        Me.txtAddress1.TabIndex = 5
        '
        'txtAddress2
        '
        Me.txtAddress2.AcceptsReturn = True
        Me.txtAddress2.BackColor = System.Drawing.SystemColors.Window
        Me.txtAddress2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAddress2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAddress2.Location = New System.Drawing.Point(144, 188)
        Me.txtAddress2.MaxLength = 0
        Me.txtAddress2.Name = "txtAddress2"
        Me.txtAddress2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAddress2.Size = New System.Drawing.Size(289, 20)
        Me.txtAddress2.TabIndex = 6
        '
        'txtAddress4
        '
        Me.txtAddress4.AcceptsReturn = True
        Me.txtAddress4.BackColor = System.Drawing.SystemColors.Window
        Me.txtAddress4.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAddress4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAddress4.Location = New System.Drawing.Point(144, 236)
        Me.txtAddress4.MaxLength = 0
        Me.txtAddress4.Name = "txtAddress4"
        Me.txtAddress4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAddress4.Size = New System.Drawing.Size(289, 20)
        Me.txtAddress4.TabIndex = 8
        '
        'cboAddressCountry
        '
        Me.cboAddressCountry.BackColor = System.Drawing.SystemColors.Window
        Me.cboAddressCountry.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboAddressCountry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAddressCountry.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboAddressCountry.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboAddressCountry.Location = New System.Drawing.Point(144, 292)
        Me.cboAddressCountry.Name = "cboAddressCountry"
        Me.cboAddressCountry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboAddressCountry.Size = New System.Drawing.Size(289, 21)
        Me.cboAddressCountry.TabIndex = 10
        '
        'txtPhoneAreaCode
        '
        Me.txtPhoneAreaCode.AcceptsReturn = True
        Me.txtPhoneAreaCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtPhoneAreaCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPhoneAreaCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPhoneAreaCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPhoneAreaCode.Location = New System.Drawing.Point(144, 336)
        Me.txtPhoneAreaCode.MaxLength = 0
        Me.txtPhoneAreaCode.Name = "txtPhoneAreaCode"
        Me.txtPhoneAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPhoneAreaCode.Size = New System.Drawing.Size(81, 20)
        Me.txtPhoneAreaCode.TabIndex = 11
        '
        'txtPhoneNumber
        '
        Me.txtPhoneNumber.AcceptsReturn = True
        Me.txtPhoneNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtPhoneNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPhoneNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPhoneNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPhoneNumber.Location = New System.Drawing.Point(232, 336)
        Me.txtPhoneNumber.MaxLength = 0
        Me.txtPhoneNumber.Name = "txtPhoneNumber"
        Me.txtPhoneNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPhoneNumber.Size = New System.Drawing.Size(113, 20)
        Me.txtPhoneNumber.TabIndex = 12
        '
        'txtFaxAreaCode
        '
        Me.txtFaxAreaCode.AcceptsReturn = True
        Me.txtFaxAreaCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtFaxAreaCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFaxAreaCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFaxAreaCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFaxAreaCode.Location = New System.Drawing.Point(144, 360)
        Me.txtFaxAreaCode.MaxLength = 0
        Me.txtFaxAreaCode.Name = "txtFaxAreaCode"
        Me.txtFaxAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFaxAreaCode.Size = New System.Drawing.Size(81, 20)
        Me.txtFaxAreaCode.TabIndex = 14
        '
        'txtFaxNumber
        '
        Me.txtFaxNumber.AcceptsReturn = True
        Me.txtFaxNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtFaxNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFaxNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFaxNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFaxNumber.Location = New System.Drawing.Point(232, 360)
        Me.txtFaxNumber.MaxLength = 0
        Me.txtFaxNumber.Name = "txtFaxNumber"
        Me.txtFaxNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFaxNumber.Size = New System.Drawing.Size(113, 20)
        Me.txtFaxNumber.TabIndex = 15
        '
        '_cmdNext_0
        '
        Me._cmdNext_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_0.Location = New System.Drawing.Point(408, 388)
        Me._cmdNext_0.Name = "_cmdNext_0"
        Me._cmdNext_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_0.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_0.TabIndex = 16
        Me._cmdNext_0.Text = "&>>"
        Me._cmdNext_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_0.UseVisualStyleBackColor = False
        '
        'txtPostalCode
        '
        Me.txtPostalCode.AcceptsReturn = True
        Me.txtPostalCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtPostalCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPostalCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPostalCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPostalCode.Location = New System.Drawing.Point(144, 260)
        Me.txtPostalCode.MaxLength = 0
        Me.txtPostalCode.Name = "txtPostalCode"
        Me.txtPostalCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPostalCode.Size = New System.Drawing.Size(113, 20)
        Me.txtPostalCode.TabIndex = 9
        '
        'cmdHeadOffice
        '
        Me.cmdHeadOffice.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHeadOffice.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHeadOffice.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHeadOffice.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHeadOffice.Location = New System.Drawing.Point(8, 132)
        Me.cmdHeadOffice.Name = "cmdHeadOffice"
        Me.cmdHeadOffice.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHeadOffice.Size = New System.Drawing.Size(113, 22)
        Me.cmdHeadOffice.TabIndex = 4
        Me.cmdHeadOffice.Text = "Head O&ffice ..."
        Me.cmdHeadOffice.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHeadOffice.UseVisualStyleBackColor = False
        '
        'pnlHeadOffice
        '
        Me.pnlHeadOffice.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.pnlHeadOffice.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlHeadOffice.Controls.Add(Me.lblHeadOffice)
        Me.pnlHeadOffice.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlHeadOffice.Location = New System.Drawing.Point(144, 132)
        Me.pnlHeadOffice.Name = "pnlHeadOffice"
        Me.pnlHeadOffice.Size = New System.Drawing.Size(289, 17)
        Me.pnlHeadOffice.TabIndex = 45
        '
        'lblHeadOffice
        '
        Me.lblHeadOffice.AutoSize = True
        Me.lblHeadOffice.Location = New System.Drawing.Point(1, -2)
        Me.lblHeadOffice.Name = "lblHeadOffice"
        Me.lblHeadOffice.Size = New System.Drawing.Size(0, 13)
        Me.lblHeadOffice.TabIndex = 0
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdPrevious_0)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtComments)
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdNext_1)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(456, 415)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "2 - Comments"
        Me._tabMainTab_TabPage1.UseVisualStyleBackColor = True
        '
        '_cmdPrevious_0
        '
        Me._cmdPrevious_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_0.Location = New System.Drawing.Point(16, 356)
        Me._cmdPrevious_0.Name = "_cmdPrevious_0"
        Me._cmdPrevious_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_0.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_0.TabIndex = 18
        Me._cmdPrevious_0.Text = "<<"
        Me._cmdPrevious_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_0.UseVisualStyleBackColor = False
        '
        'txtComments
        '
        Me.txtComments.AcceptsReturn = True
        Me.txtComments.BackColor = System.Drawing.SystemColors.Window
        Me.txtComments.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtComments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtComments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtComments.Location = New System.Drawing.Point(16, 12)
        Me.txtComments.MaxLength = 255
        Me.txtComments.Multiline = True
        Me.txtComments.Name = "txtComments"
        Me.txtComments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtComments.Size = New System.Drawing.Size(423, 328)
        Me.txtComments.TabIndex = 17
        '
        '_cmdNext_1
        '
        Me._cmdNext_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_1.Location = New System.Drawing.Point(408, 388)
        Me._cmdNext_1.Name = "_cmdNext_1"
        Me._cmdNext_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_1.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_1.TabIndex = 19
        Me._cmdNext_1.Text = "&>>"
        Me._cmdNext_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_1.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdPrevious_1)
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraAccounts)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(456, 415)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "3 - Accounts"
        Me._tabMainTab_TabPage2.UseVisualStyleBackColor = True
        '
        '_cmdPrevious_1
        '
        Me._cmdPrevious_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_1.Location = New System.Drawing.Point(16, 348)
        Me._cmdPrevious_1.Name = "_cmdPrevious_1"
        Me._cmdPrevious_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_1.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_1.TabIndex = 24
        Me._cmdPrevious_1.Text = "<<"
        Me._cmdPrevious_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_1.UseVisualStyleBackColor = False
        '
        'fraAccounts
        '
        Me.fraAccounts.BackColor = System.Drawing.SystemColors.Control
        Me.fraAccounts.Controls.Add(Me.cmdEditAcc)
        Me.fraAccounts.Controls.Add(Me.cmdDeleteAcc)
        Me.fraAccounts.Controls.Add(Me.cmdAddAcc)
        Me.fraAccounts.Controls.Add(Me.lvwAccounts)
        Me.fraAccounts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAccounts.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAccounts.Location = New System.Drawing.Point(8, 12)
        Me.fraAccounts.Name = "fraAccounts"
        Me.fraAccounts.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAccounts.Size = New System.Drawing.Size(432, 331)
        Me.fraAccounts.TabIndex = 44
        Me.fraAccounts.TabStop = False
        '
        'cmdEditAcc
        '
        Me.cmdEditAcc.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditAcc.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditAcc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditAcc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditAcc.Location = New System.Drawing.Point(168, 296)
        Me.cmdEditAcc.Name = "cmdEditAcc"
        Me.cmdEditAcc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditAcc.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditAcc.TabIndex = 23
        Me.cmdEditAcc.Text = "&Edit"
        Me.cmdEditAcc.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditAcc.UseVisualStyleBackColor = False
        '
        'cmdDeleteAcc
        '
        Me.cmdDeleteAcc.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteAcc.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteAcc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteAcc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteAcc.Location = New System.Drawing.Point(88, 296)
        Me.cmdDeleteAcc.Name = "cmdDeleteAcc"
        Me.cmdDeleteAcc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteAcc.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteAcc.TabIndex = 22
        Me.cmdDeleteAcc.Text = "&Delete"
        Me.cmdDeleteAcc.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteAcc.UseVisualStyleBackColor = False
        '
        'cmdAddAcc
        '
        Me.cmdAddAcc.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddAcc.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddAcc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddAcc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddAcc.Location = New System.Drawing.Point(8, 296)
        Me.cmdAddAcc.Name = "cmdAddAcc"
        Me.cmdAddAcc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddAcc.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddAcc.TabIndex = 21
        Me.cmdAddAcc.Text = "&Add"
        Me.cmdAddAcc.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddAcc.UseVisualStyleBackColor = False
        '
        'lvwAccounts
        '
        Me.lvwAccounts.BackColor = System.Drawing.SystemColors.Window
        Me.lvwAccounts.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwAccounts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwAccounts_ColumnHeader_1, Me._lvwAccounts_ColumnHeader_2, Me._lvwAccounts_ColumnHeader_3, Me._lvwAccounts_ColumnHeader_4, Me._lvwAccounts_ColumnHeader_5, Me._lvwAccounts_ColumnHeader_6})
        Me.lvwAccounts.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwAccounts.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwAccounts.FullRowSelect = True
        Me.lvwAccounts.LargeImageList = Me.ImageList1
        Me.lvwAccounts.Location = New System.Drawing.Point(8, 24)
        Me.lvwAccounts.MultiSelect = False
        Me.lvwAccounts.Name = "lvwAccounts"
        Me.lvwAccounts.Size = New System.Drawing.Size(412, 257)
        Me.lvwAccounts.SmallImageList = Me.ImageList1
        Me.lvwAccounts.TabIndex = 20
        Me.lvwAccounts.UseCompatibleStateImageBehavior = False
        Me.lvwAccounts.View = System.Windows.Forms.View.Details
        '
        '_lvwAccounts_ColumnHeader_1
        '
        Me._lvwAccounts_ColumnHeader_1.Tag = ""
        Me._lvwAccounts_ColumnHeader_1.Text = "Account Code"
        Me._lvwAccounts_ColumnHeader_1.Width = 67
        '
        '_lvwAccounts_ColumnHeader_2
        '
        Me._lvwAccounts_ColumnHeader_2.Tag = ""
        Me._lvwAccounts_ColumnHeader_2.Text = "Account No."
        Me._lvwAccounts_ColumnHeader_2.Width = 97
        '
        '_lvwAccounts_ColumnHeader_3
        '
        Me._lvwAccounts_ColumnHeader_3.Tag = ""
        Me._lvwAccounts_ColumnHeader_3.Text = "Account Name"
        Me._lvwAccounts_ColumnHeader_3.Width = 97
        '
        '_lvwAccounts_ColumnHeader_4
        '
        Me._lvwAccounts_ColumnHeader_4.Tag = ""
        Me._lvwAccounts_ColumnHeader_4.Text = "Description"
        Me._lvwAccounts_ColumnHeader_4.Width = 97
        '
        '_lvwAccounts_ColumnHeader_5
        '
        Me._lvwAccounts_ColumnHeader_5.Tag = ""
        Me._lvwAccounts_ColumnHeader_5.Text = "Branch"
        Me._lvwAccounts_ColumnHeader_5.Width = 67
        '
        '_lvwAccounts_ColumnHeader_6
        '
        Me._lvwAccounts_ColumnHeader_6.Tag = ""
        Me._lvwAccounts_ColumnHeader_6.Text = "Sub Branch"
        Me._lvwAccounts_ColumnHeader_6.Width = 67
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "AccountImage")
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(311, 456)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 30
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(399, 456)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 31
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(473, 485)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdNavigate)
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
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Bank"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlHeadOffice.ResumeLayout(False)
        Me.pnlHeadOffice.PerformLayout()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me._tabMainTab_TabPage1.PerformLayout()
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me.fraAccounts.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Sub InitializecmdPrevious()
        Me.cmdPrevious(1) = _cmdPrevious_1
        Me.cmdPrevious(0) = _cmdPrevious_0
    End Sub
    Sub InitializecmdNext()
        Me.cmdNext(1) = _cmdNext_1
        Me.cmdNext(0) = _cmdNext_0
    End Sub
    Sub lvwAccounts_InitializeColumnKeys()
        Me._lvwAccounts_ColumnHeader_1.Name = ""
        Me._lvwAccounts_ColumnHeader_2.Name = ""
        Me._lvwAccounts_ColumnHeader_3.Name = ""
        Me._lvwAccounts_ColumnHeader_4.Name = ""
        Me._lvwAccounts_ColumnHeader_5.Name = ""
        Me._lvwAccounts_ColumnHeader_6.Name = ""
    End Sub
    Friend WithEvents lblHeadOffice As System.Windows.Forms.Label
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdHelp As System.Windows.Forms.Button
#End Region 
End Class