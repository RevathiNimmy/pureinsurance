Imports PMLookupControl
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPartyBankDetails
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        isInitializingComponent = True
        InitializeComponent()
        isInitializingComponent = False
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
    Public WithEvents cmdOk As System.Windows.Forms.Button
    Public WithEvents cmdApply As System.Windows.Forms.Button
    Public WithEvents txtAccountType As System.Windows.Forms.TextBox
    Public WithEvents optCreditCard As System.Windows.Forms.RadioButton
    Public WithEvents optBankAccount As System.Windows.Forms.RadioButton
    Public WithEvents txtAccHolderName As System.Windows.Forms.TextBox
    Public WithEvents cboPaymentType As PMLookupControl.cboPMLookup
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents lblAccountType As System.Windows.Forms.Label
    Public WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents Frame3 As System.Windows.Forms.GroupBox
    Public WithEvents Command1 As System.Windows.Forms.Button
    Public WithEvents Command2 As System.Windows.Forms.Button
    Public WithEvents Command3 As System.Windows.Forms.Button
    Public WithEvents Check2 As System.Windows.Forms.CheckBox
    Public WithEvents uctPMAddressControl4 As PMAddressControl.uctPMAddressControl
    Public WithEvents uctCreditCard2 As System.Windows.Forms.PictureBox
    Public WithEvents Frame2 As System.Windows.Forms.GroupBox
    Public WithEvents txtBankBranchCode As System.Windows.Forms.TextBox
    Public WithEvents txtBankBranch As System.Windows.Forms.TextBox
    Public WithEvents txtAccNumber As System.Windows.Forms.TextBox
    Public WithEvents uctPMBankAddress As PMAddressControl.uctPMAddressControl
    Public WithEvents cboBankName As PMLookupControl.cboPMLookup
    Public WithEvents Label13 As System.Windows.Forms.Label
    Public WithEvents Label12 As System.Windows.Forms.Label
    Public WithEvents Label11 As System.Windows.Forms.Label
    Public WithEvents Label10 As System.Windows.Forms.Label
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Private WithEvents _SSBankDetails_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents uctCreditCardDetails As uctACTCreditCard.uctCreditCard
    Public WithEvents chkIsRegistered As System.Windows.Forms.CheckBox
    Public WithEvents uctPMAddressCC As PMAddressControl.uctPMAddressControl
    Public WithEvents Frame5 As System.Windows.Forms.GroupBox
    Private WithEvents _SSBankDetails_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents SSBankDetails As System.Windows.Forms.TabControl
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    '<System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOk = New System.Windows.Forms.Button
        Me.cmdApply = New System.Windows.Forms.Button
        Me.Frame3 = New System.Windows.Forms.GroupBox
        Me.txtAccountType = New System.Windows.Forms.TextBox
        Me.optCreditCard = New System.Windows.Forms.RadioButton
        Me.optBankAccount = New System.Windows.Forms.RadioButton
        Me.txtAccHolderName = New System.Windows.Forms.TextBox
        Me.cboPaymentType = New PMLookupControl.cboPMLookup
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblAccountType = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.SSBankDetails = New System.Windows.Forms.TabControl
        Me._SSBankDetails_TabPage0 = New System.Windows.Forms.TabPage
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.txtIBAN = New System.Windows.Forms.TextBox
        Me.lblIBAN = New System.Windows.Forms.Label
        Me.txtBIC = New System.Windows.Forms.TextBox
        Me.lblBIC = New System.Windows.Forms.Label
        Me.txtBankBranchCode = New System.Windows.Forms.TextBox
        Me.txtBankBranch = New System.Windows.Forms.TextBox
        Me.txtAccNumber = New System.Windows.Forms.TextBox
        Me.uctPMBankAddress = New PMAddressControl.uctPMAddressControl
        Me.cboBankName = New PMLookupControl.cboPMLookup
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me._SSBankDetails_TabPage1 = New System.Windows.Forms.TabPage
        Me.Frame5 = New System.Windows.Forms.GroupBox
        Me.chkIsRegistered = New System.Windows.Forms.CheckBox
        Me.uctPMAddressCC = New PMAddressControl.uctPMAddressControl
        Me.Command1 = New System.Windows.Forms.Button
        Me.Command2 = New System.Windows.Forms.Button
        Me.Command3 = New System.Windows.Forms.Button
        Me.Frame2 = New System.Windows.Forms.GroupBox
        Me.Check2 = New System.Windows.Forms.CheckBox
        Me.uctPMAddressControl4 = New PMAddressControl.uctPMAddressControl
        Me.uctCreditCard2 = New System.Windows.Forms.PictureBox
        Me.uctCreditCardDetails = New uctACTCreditCard.uctCreditCard
        Me.Frame3.SuspendLayout()
        Me.SSBankDetails.SuspendLayout()
        Me._SSBankDetails_TabPage0.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me._SSBankDetails_TabPage1.SuspendLayout()
        Me.Frame5.SuspendLayout()
        Me.Frame2.SuspendLayout()
        CType(Me.uctCreditCard2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(862, 397)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(85, 25)
        Me.cmdCancel.TabIndex = 15
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOk
        '
        Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOk.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOk.Location = New System.Drawing.Point(770, 397)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(85, 25)
        Me.cmdOk.TabIndex = 14
        Me.cmdOk.Text = "&Ok"
        Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOk.UseVisualStyleBackColor = False
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApply.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Location = New System.Drawing.Point(680, 397)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApply.Size = New System.Drawing.Size(85, 25)
        Me.cmdApply.TabIndex = 13
        Me.cmdApply.Text = "&Apply"
        Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdApply.UseVisualStyleBackColor = False
        '
        'Frame3
        '
        Me.Frame3.BackColor = System.Drawing.SystemColors.Control
        Me.Frame3.Controls.Add(Me.txtAccountType)
        Me.Frame3.Controls.Add(Me.optCreditCard)
        Me.Frame3.Controls.Add(Me.optBankAccount)
        Me.Frame3.Controls.Add(Me.txtAccHolderName)
        Me.Frame3.Controls.Add(Me.cboPaymentType)
        Me.Frame3.Controls.Add(Me.Label1)
        Me.Frame3.Controls.Add(Me.lblAccountType)
        Me.Frame3.Controls.Add(Me.Label6)
        Me.Frame3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame3.Location = New System.Drawing.Point(2, -4)
        Me.Frame3.Name = "Frame3"
        Me.Frame3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame3.Size = New System.Drawing.Size(953, 111)
        Me.Frame3.TabIndex = 17
        Me.Frame3.TabStop = False
        '
        'txtAccountType
        '
        Me.txtAccountType.AcceptsReturn = True
        Me.txtAccountType.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccountType.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccountType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccountType.Location = New System.Drawing.Point(198, 42)
        Me.txtAccountType.MaxLength = 100
        Me.txtAccountType.Name = "txtAccountType"
        Me.txtAccountType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccountType.Size = New System.Drawing.Size(237, 20)
        Me.txtAccountType.TabIndex = 2
        '
        'optCreditCard
        '
        Me.optCreditCard.BackColor = System.Drawing.SystemColors.Control
        Me.optCreditCard.Cursor = System.Windows.Forms.Cursors.Default
        Me.optCreditCard.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optCreditCard.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optCreditCard.Location = New System.Drawing.Point(162, 78)
        Me.optCreditCard.Name = "optCreditCard"
        Me.optCreditCard.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optCreditCard.Size = New System.Drawing.Size(95, 27)
        Me.optCreditCard.TabIndex = 4
        Me.optCreditCard.TabStop = True
        Me.optCreditCard.Text = "&Credit Card"
        Me.optCreditCard.UseVisualStyleBackColor = False
        '
        'optBankAccount
        '
        Me.optBankAccount.BackColor = System.Drawing.SystemColors.Control
        Me.optBankAccount.Checked = True
        Me.optBankAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.optBankAccount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optBankAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optBankAccount.Location = New System.Drawing.Point(48, 78)
        Me.optBankAccount.Name = "optBankAccount"
        Me.optBankAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optBankAccount.Size = New System.Drawing.Size(101, 27)
        Me.optBankAccount.TabIndex = 3
        Me.optBankAccount.TabStop = True
        Me.optBankAccount.Text = "&Bank Account"
        Me.optBankAccount.UseVisualStyleBackColor = False
        '
        'txtAccHolderName
        '
        Me.txtAccHolderName.AcceptsReturn = True
        Me.txtAccHolderName.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccHolderName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccHolderName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccHolderName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccHolderName.Location = New System.Drawing.Point(630, 18)
        Me.txtAccHolderName.MaxLength = 50
        Me.txtAccHolderName.Name = "txtAccHolderName"
        Me.txtAccHolderName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccHolderName.Size = New System.Drawing.Size(245, 20)
        Me.txtAccHolderName.TabIndex = 1
        '
        'cboPaymentType
        '
        Me.cboPaymentType.DefaultItemId = 0
        Me.cboPaymentType.FirstItem = ""
        Me.cboPaymentType.ItemId = 0
        Me.cboPaymentType.ListIndex = -1
        Me.cboPaymentType.Location = New System.Drawing.Point(198, 18)
        Me.cboPaymentType.Name = "cboPaymentType"
        Me.cboPaymentType.PMLookupProductFamily = 1
        Me.cboPaymentType.SingleItemId = 0
        Me.cboPaymentType.Size = New System.Drawing.Size(237, 21)
        Me.cboPaymentType.TabIndex = 0
        Me.cboPaymentType.TableName = "Bank_Payment_Type"
        Me.cboPaymentType.ToolTipText = ""
        Me.cboPaymentType.WhereClause = ""
        Me.cboPaymentType.FirstItem = "(Select Payment Type)"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(486, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(135, 37)
        Me.Label1.TabIndex = 19
        Me.Label1.Text = "Account/Card  Holder's Name"
        '
        'lblAccountType
        '
        Me.lblAccountType.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountType.Location = New System.Drawing.Point(20, 48)
        Me.lblAccountType.Name = "lblAccountType"
        Me.lblAccountType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountType.Size = New System.Drawing.Size(113, 17)
        Me.lblAccountType.TabIndex = 18
        Me.lblAccountType.Text = "Account Type"
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.SystemColors.Control
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(20, 20)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(131, 17)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "Bank Payment Type"
        '
        'SSBankDetails
        '
        Me.SSBankDetails.Controls.Add(Me._SSBankDetails_TabPage0)
        Me.SSBankDetails.Controls.Add(Me._SSBankDetails_TabPage1)
        Me.SSBankDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSBankDetails.ItemSize = New System.Drawing.Size(474, 18)
        Me.SSBankDetails.Location = New System.Drawing.Point(4, 110)
        Me.SSBankDetails.Multiline = True
        Me.SSBankDetails.Name = "SSBankDetails"
        Me.SSBankDetails.SelectedIndex = 0
        Me.SSBankDetails.Size = New System.Drawing.Size(955, 319)
        Me.SSBankDetails.TabIndex = 20
        '
        '_SSBankDetails_TabPage0
        '
        Me._SSBankDetails_TabPage0.Controls.Add(Me.Frame1)
        Me._SSBankDetails_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSBankDetails_TabPage0.Name = "_SSBankDetails_TabPage0"
        Me._SSBankDetails_TabPage0.Size = New System.Drawing.Size(947, 233)
        Me._SSBankDetails_TabPage0.TabIndex = 0
        Me._SSBankDetails_TabPage0.Text = "Bank Account"
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.txtIBAN)
        Me.Frame1.Controls.Add(Me.lblIBAN)
        Me.Frame1.Controls.Add(Me.txtBIC)
        Me.Frame1.Controls.Add(Me.lblBIC)
        Me.Frame1.Controls.Add(Me.txtBankBranchCode)
        Me.Frame1.Controls.Add(Me.txtBankBranch)
        Me.Frame1.Controls.Add(Me.txtAccNumber)
        Me.Frame1.Controls.Add(Me.uctPMBankAddress)
        Me.Frame1.Controls.Add(Me.cboBankName)
        Me.Frame1.Controls.Add(Me.Label13)
        Me.Frame1.Controls.Add(Me.Label12)
        Me.Frame1.Controls.Add(Me.Label11)
        Me.Frame1.Controls.Add(Me.Label10)
        Me.Frame1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(6, 14)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(937, 216)
        Me.Frame1.TabIndex = 28
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Bank Account Details"
        '
        'txtIBAN
        '
        Me.txtIBAN.AcceptsReturn = True
        Me.txtIBAN.BackColor = System.Drawing.SystemColors.Window
        Me.txtIBAN.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtIBAN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIBAN.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIBAN.Location = New System.Drawing.Point(200, 154)
        Me.txtIBAN.MaxLength = 50
        Me.txtIBAN.Name = "txtIBAN"
        Me.txtIBAN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtIBAN.Size = New System.Drawing.Size(241, 20)
        Me.txtIBAN.TabIndex = 35
        '
        'lblIBAN
        '
        Me.lblIBAN.BackColor = System.Drawing.SystemColors.Control
        Me.lblIBAN.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIBAN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIBAN.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIBAN.Location = New System.Drawing.Point(42, 156)
        Me.lblIBAN.Name = "lblIBAN"
        Me.lblIBAN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIBAN.Size = New System.Drawing.Size(113, 17)
        Me.lblIBAN.TabIndex = 36
        Me.lblIBAN.Text = "IBAN"
        '
        'txtBIC
        '
        Me.txtBIC.AcceptsReturn = True
        Me.txtBIC.BackColor = System.Drawing.SystemColors.Window
        Me.txtBIC.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBIC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBIC.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBIC.Location = New System.Drawing.Point(200, 129)
        Me.txtBIC.MaxLength = 50
        Me.txtBIC.Name = "txtBIC"
        Me.txtBIC.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBIC.Size = New System.Drawing.Size(241, 20)
        Me.txtBIC.TabIndex = 33
        '
        'lblBIC
        '
        Me.lblBIC.BackColor = System.Drawing.SystemColors.Control
        Me.lblBIC.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBIC.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBIC.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBIC.Location = New System.Drawing.Point(42, 131)
        Me.lblBIC.Name = "lblBIC"
        Me.lblBIC.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBIC.Size = New System.Drawing.Size(113, 17)
        Me.lblBIC.TabIndex = 34
        Me.lblBIC.Text = "BIC"
        '
        'txtBankBranchCode
        '
        Me.txtBankBranchCode.AcceptsReturn = True
        Me.txtBankBranchCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankBranchCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankBranchCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankBranchCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankBranchCode.Location = New System.Drawing.Point(200, 52)
        Me.txtBankBranchCode.MaxLength = 50
        Me.txtBankBranchCode.Name = "txtBankBranchCode"
        Me.txtBankBranchCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankBranchCode.Size = New System.Drawing.Size(241, 20)
        Me.txtBankBranchCode.TabIndex = 6
        '
        'txtBankBranch
        '
        Me.txtBankBranch.AcceptsReturn = True
        Me.txtBankBranch.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankBranch.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankBranch.Location = New System.Drawing.Point(200, 78)
        Me.txtBankBranch.MaxLength = 50
        Me.txtBankBranch.Name = "txtBankBranch"
        Me.txtBankBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankBranch.Size = New System.Drawing.Size(241, 20)
        Me.txtBankBranch.TabIndex = 7
        '
        'txtAccNumber
        '
        Me.txtAccNumber.AcceptsReturn = True
        Me.txtAccNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccNumber.Location = New System.Drawing.Point(200, 28)
        Me.txtAccNumber.MaxLength = 50
        Me.txtAccNumber.Name = "txtAccNumber"
        Me.txtAccNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccNumber.Size = New System.Drawing.Size(241, 20)
        Me.txtAccNumber.TabIndex = 5
        '
        'uctPMBankAddress
        '
        Me.uctPMBankAddress.AddressLine1 = ""
        Me.uctPMBankAddress.AddressLine2 = ""
        Me.uctPMBankAddress.AddressLine3 = ""
        Me.uctPMBankAddress.AddressLine4 = ""
        Me.uctPMBankAddress.Caption = ""
        Me.uctPMBankAddress.CaptionAddress1 = "No. && street name:"
        Me.uctPMBankAddress.CaptionAddress2 = "Locality:"
        Me.uctPMBankAddress.CaptionAddress3 = "Town:"
        Me.uctPMBankAddress.CaptionAddress4 = "County:"
        Me.uctPMBankAddress.CaptionCountry = "Country:"
        Me.uctPMBankAddress.CaptionFontBoldAddress1 = False
        Me.uctPMBankAddress.CaptionFontBoldPostCode = False
        Me.uctPMBankAddress.CaptionPostCode = "Postcode:"
        Me.uctPMBankAddress.ClearButtonCaption = "X"
        Me.uctPMBankAddress.ClearButtonFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMBankAddress.ClearButtonLeft = 4830
        Me.uctPMBankAddress.ClearButtonWidth = 360
        Me.uctPMBankAddress.CountryId = 3
        Me.uctPMBankAddress.FaceFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMBankAddress.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMBankAddress.IsCountryRequired = 1
        Me.uctPMBankAddress.IsPostCodeRequired = 1
        Me.uctPMBankAddress.Location = New System.Drawing.Point(514, 8)
        Me.uctPMBankAddress.Name = "uctPMBankAddress"
        Me.uctPMBankAddress.Organisation = ""
        Me.uctPMBankAddress.PMAddressCnt = 0
        Me.uctPMBankAddress.PMDatabaseID = 0
        Me.uctPMBankAddress.PostCode = ""
        Me.uctPMBankAddress.QAS2PMAddress1 = ""
        Me.uctPMBankAddress.QAS2PMAddress2 = ""
        Me.uctPMBankAddress.QAS2PMAddress3 = ""
        Me.uctPMBankAddress.QAS2PMAddress4 = ""
        Me.uctPMBankAddress.QASDatabaseID = 0
        Me.uctPMBankAddress.SearchButtonCaption = ".."
        Me.uctPMBankAddress.SearchButtonFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMBankAddress.SearchButtonHeight = 285
        Me.uctPMBankAddress.SearchButtonLeft = 4395
        Me.uctPMBankAddress.SearchButtonTop = 1530
        Me.uctPMBankAddress.SearchButtonWidth = 360
        Me.uctPMBankAddress.Size = New System.Drawing.Size(417, 152)
        Me.uctPMBankAddress.TabIndex = 9
        Me.uctPMBankAddress.WarningMessage = ""
        '
        'cboBankName
        '
        Me.cboBankName.DefaultItemId = 0
        Me.cboBankName.FirstItem = ""
        Me.cboBankName.ItemId = 0
        Me.cboBankName.ListIndex = -1
        Me.cboBankName.Location = New System.Drawing.Point(200, 102)
        Me.cboBankName.Name = "cboBankName"
        Me.cboBankName.PMLookupProductFamily = 1
        Me.cboBankName.SingleItemId = 0
        Me.cboBankName.Size = New System.Drawing.Size(243, 21)
        Me.cboBankName.TabIndex = 8
        Me.cboBankName.TableName = "cashlistitem_bank"
        Me.cboBankName.ToolTipText = ""
        Me.cboBankName.WhereClause = ""
        '
        'Label13
        '
        Me.Label13.BackColor = System.Drawing.SystemColors.Control
        Me.Label13.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label13.Location = New System.Drawing.Point(42, 82)
        Me.Label13.Name = "Label13"
        Me.Label13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label13.Size = New System.Drawing.Size(113, 17)
        Me.Label13.TabIndex = 32
        Me.Label13.Text = "Bank Branch"
        '
        'Label12
        '
        Me.Label12.BackColor = System.Drawing.SystemColors.Control
        Me.Label12.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label12.Location = New System.Drawing.Point(42, 56)
        Me.Label12.Name = "Label12"
        Me.Label12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label12.Size = New System.Drawing.Size(128, 17)
        Me.Label12.TabIndex = 31
        Me.Label12.Text = "Bank Branch Code"
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.SystemColors.Control
        Me.Label11.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label11.Location = New System.Drawing.Point(42, 106)
        Me.Label11.Name = "Label11"
        Me.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label11.Size = New System.Drawing.Size(113, 15)
        Me.Label11.TabIndex = 30
        Me.Label11.Text = "Bank Name"
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.SystemColors.Control
        Me.Label10.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label10.Location = New System.Drawing.Point(42, 30)
        Me.Label10.Name = "Label10"
        Me.Label10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label10.Size = New System.Drawing.Size(113, 17)
        Me.Label10.TabIndex = 29
        Me.Label10.Text = "Account Number"
        '
        '_SSBankDetails_TabPage1
        '
        Me._SSBankDetails_TabPage1.Controls.Add(Me.Frame5)
        Me._SSBankDetails_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._SSBankDetails_TabPage1.Name = "_SSBankDetails_TabPage1"
        Me._SSBankDetails_TabPage1.Size = New System.Drawing.Size(947, 293)
        Me._SSBankDetails_TabPage1.TabIndex = 1
        Me._SSBankDetails_TabPage1.Text = "Credit Card"
        '
        'Frame5
        '
        Me.Frame5.BackColor = System.Drawing.SystemColors.Control
        Me.Frame5.Controls.Add(Me.uctCreditCardDetails)
        Me.Frame5.Controls.Add(Me.chkIsRegistered)
        Me.Frame5.Controls.Add(Me.uctPMAddressCC)
        Me.Frame5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame5.Location = New System.Drawing.Point(6, 14)
        Me.Frame5.Name = "Frame5"
        Me.Frame5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame5.Size = New System.Drawing.Size(937, 245)
        Me.Frame5.TabIndex = 27
        Me.Frame5.TabStop = False
        Me.Frame5.Text = "Credit/Debit Card Details"
        '
        'chkIsRegistered
        '
        Me.chkIsRegistered.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsRegistered.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsRegistered.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsRegistered.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsRegistered.Location = New System.Drawing.Point(570, 8)
        Me.chkIsRegistered.Name = "chkIsRegistered"
        Me.chkIsRegistered.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsRegistered.Size = New System.Drawing.Size(151, 17)
        Me.chkIsRegistered.TabIndex = 11
        Me.chkIsRegistered.Text = "Registered CardHolder"
        Me.chkIsRegistered.UseVisualStyleBackColor = False
        '
        'uctPMAddressCC
        '
        Me.uctPMAddressCC.AddressLine1 = ""
        Me.uctPMAddressCC.AddressLine2 = ""
        Me.uctPMAddressCC.AddressLine3 = ""
        Me.uctPMAddressCC.AddressLine4 = ""
        Me.uctPMAddressCC.Caption = ""
        Me.uctPMAddressCC.CaptionAddress1 = "No. && street name:"
        Me.uctPMAddressCC.CaptionAddress2 = "Locality:"
        Me.uctPMAddressCC.CaptionAddress3 = "Town:"
        Me.uctPMAddressCC.CaptionAddress4 = "County:"
        Me.uctPMAddressCC.CaptionCountry = "Country:"
        Me.uctPMAddressCC.CaptionFontBoldAddress1 = False
        Me.uctPMAddressCC.CaptionFontBoldPostCode = False
        Me.uctPMAddressCC.CaptionPostCode = "Postcode:"
        Me.uctPMAddressCC.ClearButtonCaption = "X"
        Me.uctPMAddressCC.ClearButtonFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMAddressCC.ClearButtonLeft = 4845
        Me.uctPMAddressCC.ClearButtonWidth = 360
        Me.uctPMAddressCC.CountryId = 3
        Me.uctPMAddressCC.FaceFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMAddressCC.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMAddressCC.IsCountryRequired = 1
        Me.uctPMAddressCC.IsPostCodeRequired = 1
        Me.uctPMAddressCC.Location = New System.Drawing.Point(562, 10)
        Me.uctPMAddressCC.Name = "uctPMAddressCC"
        Me.uctPMAddressCC.Organisation = ""
        Me.uctPMAddressCC.PMAddressCnt = 0
        Me.uctPMAddressCC.PMDatabaseID = 0
        Me.uctPMAddressCC.PostCode = ""
        Me.uctPMAddressCC.QAS2PMAddress1 = ""
        Me.uctPMAddressCC.QAS2PMAddress2 = ""
        Me.uctPMAddressCC.QAS2PMAddress3 = ""
        Me.uctPMAddressCC.QAS2PMAddress4 = ""
        Me.uctPMAddressCC.QASDatabaseID = 0
        Me.uctPMAddressCC.SearchButtonCaption = ".."
        Me.uctPMAddressCC.SearchButtonFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMAddressCC.SearchButtonHeight = 285
        Me.uctPMAddressCC.SearchButtonLeft = 4410
        Me.uctPMAddressCC.SearchButtonTop = 1530
        Me.uctPMAddressCC.SearchButtonWidth = 360
        Me.uctPMAddressCC.Size = New System.Drawing.Size(366, 152)
        Me.uctPMAddressCC.TabIndex = 12
        Me.uctPMAddressCC.WarningMessage = ""
        '
        'Command1
        '
        Me.Command1.BackColor = System.Drawing.SystemColors.Control
        Me.Command1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Command1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Command1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Command1.Location = New System.Drawing.Point(-4134, 202)
        Me.Command1.Name = "Command1"
        Me.Command1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Command1.Size = New System.Drawing.Size(85, 25)
        Me.Command1.TabIndex = 26
        Me.Command1.Text = "Cancel"
        Me.Command1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Command1.UseVisualStyleBackColor = False
        '
        'Command2
        '
        Me.Command2.BackColor = System.Drawing.SystemColors.Control
        Me.Command2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Command2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Command2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Command2.Location = New System.Drawing.Point(-4226, 202)
        Me.Command2.Name = "Command2"
        Me.Command2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Command2.Size = New System.Drawing.Size(85, 25)
        Me.Command2.TabIndex = 25
        Me.Command2.Text = "Ok"
        Me.Command2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Command2.UseVisualStyleBackColor = False
        '
        'Command3
        '
        Me.Command3.BackColor = System.Drawing.SystemColors.Control
        Me.Command3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Command3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Command3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Command3.Location = New System.Drawing.Point(-4318, 202)
        Me.Command3.Name = "Command3"
        Me.Command3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Command3.Size = New System.Drawing.Size(85, 25)
        Me.Command3.TabIndex = 24
        Me.Command3.Text = "Apply"
        Me.Command3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Command3.UseVisualStyleBackColor = False
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.Check2)
        Me.Frame2.Controls.Add(Me.uctPMAddressControl4)
        Me.Frame2.Controls.Add(Me.uctCreditCard2)
        Me.Frame2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(-4986, 30)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(937, 163)
        Me.Frame2.TabIndex = 21
        Me.Frame2.TabStop = False
        Me.Frame2.Text = "Credit/Debit Card Details"
        '
        'Check2
        '
        Me.Check2.BackColor = System.Drawing.SystemColors.Control
        Me.Check2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Check2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Check2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Check2.Location = New System.Drawing.Point(570, 18)
        Me.Check2.Name = "Check2"
        Me.Check2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Check2.Size = New System.Drawing.Size(127, 17)
        Me.Check2.TabIndex = 22
        Me.Check2.Text = "Registered CardHolder"
        Me.Check2.UseVisualStyleBackColor = False
        '
        'uctPMAddressControl4
        '
        Me.uctPMAddressControl4.AddressLine1 = ""
        Me.uctPMAddressControl4.AddressLine2 = ""
        Me.uctPMAddressControl4.AddressLine3 = ""
        Me.uctPMAddressControl4.AddressLine4 = ""
        Me.uctPMAddressControl4.Caption = ""
        Me.uctPMAddressControl4.CaptionAddress1 = "No. && street name:"
        Me.uctPMAddressControl4.CaptionAddress2 = "Locality:"
        Me.uctPMAddressControl4.CaptionAddress3 = "Town:"
        Me.uctPMAddressControl4.CaptionAddress4 = "County:"
        Me.uctPMAddressControl4.CaptionCountry = "Country:"
        Me.uctPMAddressControl4.CaptionFontBoldAddress1 = False
        Me.uctPMAddressControl4.CaptionFontBoldPostCode = False
        Me.uctPMAddressControl4.CaptionPostCode = "Postcode:"
        Me.uctPMAddressControl4.ClearButtonCaption = "X"
        Me.uctPMAddressControl4.ClearButtonFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMAddressControl4.ClearButtonLeft = 4950
        Me.uctPMAddressControl4.ClearButtonWidth = 360
        Me.uctPMAddressControl4.CountryId = 3
        Me.uctPMAddressControl4.FaceFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMAddressControl4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMAddressControl4.IsCountryRequired = 0
        Me.uctPMAddressControl4.IsPostCodeRequired = 1
        Me.uctPMAddressControl4.Location = New System.Drawing.Point(562, 20)
        Me.uctPMAddressControl4.Name = "uctPMAddressControl4"
        Me.uctPMAddressControl4.Organisation = ""
        Me.uctPMAddressControl4.PMAddressCnt = 0
        Me.uctPMAddressControl4.PMDatabaseID = 0
        Me.uctPMAddressControl4.PostCode = ""
        Me.uctPMAddressControl4.QAS2PMAddress1 = ""
        Me.uctPMAddressControl4.QAS2PMAddress2 = ""
        Me.uctPMAddressControl4.QAS2PMAddress3 = ""
        Me.uctPMAddressControl4.QAS2PMAddress4 = ""
        Me.uctPMAddressControl4.QASDatabaseID = 0
        Me.uctPMAddressControl4.SearchButtonCaption = ".."
        Me.uctPMAddressControl4.SearchButtonFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMAddressControl4.SearchButtonHeight = 285
        Me.uctPMAddressControl4.SearchButtonLeft = 4515
        Me.uctPMAddressControl4.SearchButtonTop = 1530
        Me.uctPMAddressControl4.SearchButtonWidth = 360
        Me.uctPMAddressControl4.Size = New System.Drawing.Size(366, 129)
        Me.uctPMAddressControl4.TabIndex = 23
        Me.uctPMAddressControl4.WarningMessage = ""
        '
        'uctCreditCard2
        '
        Me.uctCreditCard2.BackColor = System.Drawing.SystemColors.Control
        Me.uctCreditCard2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.uctCreditCard2.Cursor = System.Windows.Forms.Cursors.Default
        Me.uctCreditCard2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctCreditCard2.Location = New System.Drawing.Point(8, 20)
        Me.uctCreditCard2.Name = "uctCreditCard2"
        Me.uctCreditCard2.Size = New System.Drawing.Size(555, 135)
        Me.uctCreditCard2.TabIndex = 33
        '
        'uctCreditCardDetails
        '
        Me.uctCreditCardDetails.CardTransSlipNo = ""
        Me.uctCreditCardDetails.CardTypeId = -1
        Me.uctCreditCardDetails.CCAutoAuthCode = ""
        Me.uctCreditCardDetails.CCBankId = -1
        Me.uctCreditCardDetails.CCCustomerFlag = ""
        Me.uctCreditCardDetails.CCExpiry = ""
        Me.uctCreditCardDetails.CCIssue = ""
        Me.uctCreditCardDetails.CCManualAuthCode = ""
        Me.uctCreditCardDetails.CCName = ""
        Me.uctCreditCardDetails.CCNumber = ""
        Me.uctCreditCardDetails.CCPIN = ""
        Me.uctCreditCardDetails.CCStart = ""
        Me.uctCreditCardDetails.CCTransactionCode = ""
        Me.uctCreditCardDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctCreditCardDetails.IsAdditionalDetailOption = False
        Me.uctCreditCardDetails.IsExternalCreditCardProcessing = False
        Me.uctCreditCardDetails.Location = New System.Drawing.Point(4, 14)
        Me.uctCreditCardDetails.Name = "uctCreditCardDetails"
        Me.uctCreditCardDetails.Size = New System.Drawing.Size(549, 155)
        Me.uctCreditCardDetails.TabIndex = 10
        Me.uctCreditCardDetails.ViewOnlyMode = True
        '
        'frmPartyBankDetails
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(958, 466)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.Frame3)
        Me.Controls.Add(Me.SSBankDetails)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPartyBankDetails"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Payment Details"
        Me.Frame3.ResumeLayout(False)
        Me.SSBankDetails.ResumeLayout(False)
        Me._SSBankDetails_TabPage0.ResumeLayout(False)
        Me.Frame1.ResumeLayout(False)
        Me._SSBankDetails_TabPage1.ResumeLayout(False)
        Me.Frame5.ResumeLayout(False)
        Me.Frame2.ResumeLayout(False)
        CType(Me.uctCreditCard2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents txtIBAN As System.Windows.Forms.TextBox
    Public WithEvents lblIBAN As System.Windows.Forms.Label
    Public WithEvents txtBIC As System.Windows.Forms.TextBox
    Public WithEvents lblBIC As System.Windows.Forms.Label
#End Region
End Class
