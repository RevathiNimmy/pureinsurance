<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBankAccountRuleEdit
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializeoptTransactionRule()
		InitializeLine1()
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
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents imgIcon As System.Windows.Forms.PictureBox
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents pnlBankAccount As System.Windows.Forms.Panel
	Public WithEvents cboPMLookupMediaType As PMLookupControl.cboPMLookup
	Public WithEvents chkCodeIsMerchantNumber As System.Windows.Forms.CheckBox
	Public WithEvents chkSkip As System.Windows.Forms.CheckBox
	Public WithEvents chkMatchDate As System.Windows.Forms.CheckBox
	Public WithEvents chkMatchAmount As System.Windows.Forms.CheckBox
	Public WithEvents chkMatchChequeNumber As System.Windows.Forms.CheckBox
	Public WithEvents chkMatchBatchReference As System.Windows.Forms.CheckBox
	Public WithEvents chkMatchCode As System.Windows.Forms.CheckBox
	Public WithEvents chkReferenceIsRemitCode As System.Windows.Forms.CheckBox
	Private WithEvents _Line1_4 As System.Windows.Forms.Label
	Private WithEvents _Line1_3 As System.Windows.Forms.Label
	Private WithEvents _Line1_2 As System.Windows.Forms.Label
	Private WithEvents _Line1_1 As System.Windows.Forms.Label
	Private WithEvents _Line1_0 As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Private WithEvents _optTransactionRule_2 As System.Windows.Forms.RadioButton
	Private WithEvents _optTransactionRule_1 As System.Windows.Forms.RadioButton
	Private WithEvents _optTransactionRule_0 As System.Windows.Forms.RadioButton
	Public WithEvents fraRuleType As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public Line1(4) As System.Windows.Forms.Label
	Public optTransactionRule(2) As System.Windows.Forms.RadioButton
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me.imgIcon = New System.Windows.Forms.PictureBox
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.pnlBankAccount = New System.Windows.Forms.Panel
        Me.cboPMLookupMediaType = New PMLookupControl.cboPMLookup
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.chkCodeIsMerchantNumber = New System.Windows.Forms.CheckBox
        Me.chkSkip = New System.Windows.Forms.CheckBox
        Me.chkMatchDate = New System.Windows.Forms.CheckBox
        Me.chkMatchAmount = New System.Windows.Forms.CheckBox
        Me.chkMatchChequeNumber = New System.Windows.Forms.CheckBox
        Me.chkMatchBatchReference = New System.Windows.Forms.CheckBox
        Me.chkMatchCode = New System.Windows.Forms.CheckBox
        Me.chkReferenceIsRemitCode = New System.Windows.Forms.CheckBox
        Me._Line1_4 = New System.Windows.Forms.Label
        Me._Line1_3 = New System.Windows.Forms.Label
        Me._Line1_2 = New System.Windows.Forms.Label
        Me._Line1_1 = New System.Windows.Forms.Label
        Me._Line1_0 = New System.Windows.Forms.Label
        Me.fraRuleType = New System.Windows.Forms.GroupBox
        Me._optTransactionRule_2 = New System.Windows.Forms.RadioButton
        Me._optTransactionRule_1 = New System.Windows.Forms.RadioButton
        Me._optTransactionRule_0 = New System.Windows.Forms.RadioButton
        Me.tabMainTab.SuspendLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me.fraRuleType.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(258, 366)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(81, 22)
        Me.cmdOK.TabIndex = 15
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(346, 366)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(81, 22)
        Me.cmdCancel.TabIndex = 14
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(208, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(422, 357)
        Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.tabMainTab.TabIndex = 0
        '
        'imgIcon
        '
        Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon.Location = New System.Drawing.Point(627, -36)
        Me.imgIcon.Name = "imgIcon"
        Me.imgIcon.Size = New System.Drawing.Size(32, 32)
        Me.imgIcon.TabIndex = 0
        Me.imgIcon.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.Label1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Label2)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pnlBankAccount)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboPMLookupMediaType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Frame1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraRuleType)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(414, 331)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "Details"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(17, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(105, 17)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Bank Account:"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(17, 52)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(113, 25)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Media Type:"
        '
        'pnlBankAccount
        '
        Me.pnlBankAccount.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.pnlBankAccount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlBankAccount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlBankAccount.Location = New System.Drawing.Point(128, 20)
        Me.pnlBankAccount.Name = "pnlBankAccount"
        Me.pnlBankAccount.Size = New System.Drawing.Size(273, 19)
        Me.pnlBankAccount.TabIndex = 3
        '
        'cboPMLookupMediaType
        '
        Me.cboPMLookupMediaType.DefaultItemId = 0
        Me.cboPMLookupMediaType.FirstItem = ""
        Me.cboPMLookupMediaType.ItemId = 0
        Me.cboPMLookupMediaType.ListIndex = -1
        Me.cboPMLookupMediaType.Location = New System.Drawing.Point(128, 50)
        Me.cboPMLookupMediaType.Name = "cboPMLookupMediaType"
        Me.cboPMLookupMediaType.PMLookupProductFamily = 1
        Me.cboPMLookupMediaType.SingleItemId = 0
        Me.cboPMLookupMediaType.Size = New System.Drawing.Size(137, 21)
        Me.cboPMLookupMediaType.Sorted = True
        Me.cboPMLookupMediaType.TabIndex = 4
        Me.cboPMLookupMediaType.TableName = "mediatype"
        Me.cboPMLookupMediaType.ToolTipText = ""
        Me.cboPMLookupMediaType.WhereClause = ""
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.chkCodeIsMerchantNumber)
        Me.Frame1.Controls.Add(Me.chkSkip)
        Me.Frame1.Controls.Add(Me.chkMatchDate)
        Me.Frame1.Controls.Add(Me.chkMatchAmount)
        Me.Frame1.Controls.Add(Me.chkMatchChequeNumber)
        Me.Frame1.Controls.Add(Me.chkMatchBatchReference)
        Me.Frame1.Controls.Add(Me.chkMatchCode)
        Me.Frame1.Controls.Add(Me.chkReferenceIsRemitCode)
        Me.Frame1.Controls.Add(Me._Line1_4)
        Me.Frame1.Controls.Add(Me._Line1_3)
        Me.Frame1.Controls.Add(Me._Line1_2)
        Me.Frame1.Controls.Add(Me._Line1_1)
        Me.Frame1.Controls.Add(Me._Line1_0)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(16, 122)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(385, 199)
        Me.Frame1.TabIndex = 5
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Rule"
        '
        'chkCodeIsMerchantNumber
        '
        Me.chkCodeIsMerchantNumber.BackColor = System.Drawing.SystemColors.Control
        Me.chkCodeIsMerchantNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCodeIsMerchantNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCodeIsMerchantNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCodeIsMerchantNumber.Location = New System.Drawing.Point(192, 19)
        Me.chkCodeIsMerchantNumber.Name = "chkCodeIsMerchantNumber"
        Me.chkCodeIsMerchantNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCodeIsMerchantNumber.Size = New System.Drawing.Size(177, 18)
        Me.chkCodeIsMerchantNumber.TabIndex = 12
        Me.chkCodeIsMerchantNumber.Text = "Code Is Account Code"
        Me.chkCodeIsMerchantNumber.UseVisualStyleBackColor = False
        '
        'chkSkip
        '
        Me.chkSkip.BackColor = System.Drawing.SystemColors.Control
        Me.chkSkip.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSkip.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSkip.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSkip.Location = New System.Drawing.Point(16, 170)
        Me.chkSkip.Name = "chkSkip"
        Me.chkSkip.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSkip.Size = New System.Drawing.Size(281, 23)
        Me.chkSkip.TabIndex = 11
        Me.chkSkip.Text = "Skip Matching If Dishonour Code Is Blank"
        Me.chkSkip.UseVisualStyleBackColor = False
        '
        'chkMatchDate
        '
        Me.chkMatchDate.BackColor = System.Drawing.SystemColors.Control
        Me.chkMatchDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkMatchDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMatchDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkMatchDate.Location = New System.Drawing.Point(16, 141)
        Me.chkMatchDate.Name = "chkMatchDate"
        Me.chkMatchDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkMatchDate.Size = New System.Drawing.Size(153, 18)
        Me.chkMatchDate.TabIndex = 10
        Me.chkMatchDate.Text = "Match Date"
        Me.chkMatchDate.UseVisualStyleBackColor = False
        '
        'chkMatchAmount
        '
        Me.chkMatchAmount.BackColor = System.Drawing.SystemColors.Control
        Me.chkMatchAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkMatchAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMatchAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkMatchAmount.Location = New System.Drawing.Point(16, 112)
        Me.chkMatchAmount.Name = "chkMatchAmount"
        Me.chkMatchAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkMatchAmount.Size = New System.Drawing.Size(137, 18)
        Me.chkMatchAmount.TabIndex = 9
        Me.chkMatchAmount.Text = "Match Amount"
        Me.chkMatchAmount.UseVisualStyleBackColor = False
        '
        'chkMatchChequeNumber
        '
        Me.chkMatchChequeNumber.BackColor = System.Drawing.SystemColors.Control
        Me.chkMatchChequeNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkMatchChequeNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMatchChequeNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkMatchChequeNumber.Location = New System.Drawing.Point(16, 83)
        Me.chkMatchChequeNumber.Name = "chkMatchChequeNumber"
        Me.chkMatchChequeNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkMatchChequeNumber.Size = New System.Drawing.Size(161, 17)
        Me.chkMatchChequeNumber.TabIndex = 8
        Me.chkMatchChequeNumber.Text = "Match Cheque Number"
        Me.chkMatchChequeNumber.UseVisualStyleBackColor = False
        '
        'chkMatchBatchReference
        '
        Me.chkMatchBatchReference.BackColor = System.Drawing.SystemColors.Control
        Me.chkMatchBatchReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkMatchBatchReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMatchBatchReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkMatchBatchReference.Location = New System.Drawing.Point(16, 54)
        Me.chkMatchBatchReference.Name = "chkMatchBatchReference"
        Me.chkMatchBatchReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkMatchBatchReference.Size = New System.Drawing.Size(169, 17)
        Me.chkMatchBatchReference.TabIndex = 7
        Me.chkMatchBatchReference.Text = "Match Batch Reference"
        Me.chkMatchBatchReference.UseVisualStyleBackColor = False
        '
        'chkMatchCode
        '
        Me.chkMatchCode.BackColor = System.Drawing.SystemColors.Control
        Me.chkMatchCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkMatchCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMatchCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkMatchCode.Location = New System.Drawing.Point(16, 19)
        Me.chkMatchCode.Name = "chkMatchCode"
        Me.chkMatchCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkMatchCode.Size = New System.Drawing.Size(137, 18)
        Me.chkMatchCode.TabIndex = 6
        Me.chkMatchCode.Text = "Match Code"
        Me.chkMatchCode.UseVisualStyleBackColor = False
        '
        'chkReferenceIsRemitCode
        '
        Me.chkReferenceIsRemitCode.BackColor = System.Drawing.SystemColors.Control
        Me.chkReferenceIsRemitCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkReferenceIsRemitCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkReferenceIsRemitCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkReferenceIsRemitCode.Location = New System.Drawing.Point(191, 54)
        Me.chkReferenceIsRemitCode.Name = "chkReferenceIsRemitCode"
        Me.chkReferenceIsRemitCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkReferenceIsRemitCode.Size = New System.Drawing.Size(169, 17)
        Me.chkReferenceIsRemitCode.TabIndex = 13
        Me.chkReferenceIsRemitCode.Text = "Reference Is Remit Code"
        Me.chkReferenceIsRemitCode.UseVisualStyleBackColor = False
        '
        '_Line1_4
        '
        Me._Line1_4.BackColor = System.Drawing.SystemColors.WindowText
        Me._Line1_4.Location = New System.Drawing.Point(8, 74)
        Me._Line1_4.Name = "_Line1_4"
        Me._Line1_4.Size = New System.Drawing.Size(368, 1)
        Me._Line1_4.TabIndex = 14
        '
        '_Line1_3
        '
        Me._Line1_3.BackColor = System.Drawing.SystemColors.WindowText
        Me._Line1_3.Location = New System.Drawing.Point(8, 103)
        Me._Line1_3.Name = "_Line1_3"
        Me._Line1_3.Size = New System.Drawing.Size(368, 1)
        Me._Line1_3.TabIndex = 15
        '
        '_Line1_2
        '
        Me._Line1_2.BackColor = System.Drawing.SystemColors.WindowText
        Me._Line1_2.Location = New System.Drawing.Point(8, 133)
        Me._Line1_2.Name = "_Line1_2"
        Me._Line1_2.Size = New System.Drawing.Size(368, 1)
        Me._Line1_2.TabIndex = 16
        '
        '_Line1_1
        '
        Me._Line1_1.BackColor = System.Drawing.SystemColors.WindowText
        Me._Line1_1.Location = New System.Drawing.Point(8, 162)
        Me._Line1_1.Name = "_Line1_1"
        Me._Line1_1.Size = New System.Drawing.Size(368, 1)
        Me._Line1_1.TabIndex = 17
        '
        '_Line1_0
        '
        Me._Line1_0.BackColor = System.Drawing.SystemColors.WindowText
        Me._Line1_0.Location = New System.Drawing.Point(8, 44)
        Me._Line1_0.Name = "_Line1_0"
        Me._Line1_0.Size = New System.Drawing.Size(368, 1)
        Me._Line1_0.TabIndex = 18
        '
        'fraRuleType
        '
        Me.fraRuleType.BackColor = System.Drawing.SystemColors.Control
        Me.fraRuleType.Controls.Add(Me._optTransactionRule_2)
        Me.fraRuleType.Controls.Add(Me._optTransactionRule_1)
        Me.fraRuleType.Controls.Add(Me._optTransactionRule_0)
        Me.fraRuleType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraRuleType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraRuleType.Location = New System.Drawing.Point(16, 80)
        Me.fraRuleType.Name = "fraRuleType"
        Me.fraRuleType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraRuleType.Size = New System.Drawing.Size(385, 39)
        Me.fraRuleType.TabIndex = 16
        Me.fraRuleType.TabStop = False
        Me.fraRuleType.Text = "Rule Type"
        '
        '_optTransactionRule_2
        '
        Me._optTransactionRule_2.BackColor = System.Drawing.SystemColors.Control
        Me._optTransactionRule_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optTransactionRule_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optTransactionRule_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optTransactionRule_2.Location = New System.Drawing.Point(266, 16)
        Me._optTransactionRule_2.Name = "_optTransactionRule_2"
        Me._optTransactionRule_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optTransactionRule_2.Size = New System.Drawing.Size(103, 17)
        Me._optTransactionRule_2.TabIndex = 19
        Me._optTransactionRule_2.TabStop = True
        Me._optTransactionRule_2.Text = "Instalment"
        Me._optTransactionRule_2.UseVisualStyleBackColor = False
        '
        '_optTransactionRule_1
        '
        Me._optTransactionRule_1.BackColor = System.Drawing.SystemColors.Control
        Me._optTransactionRule_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optTransactionRule_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optTransactionRule_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optTransactionRule_1.Location = New System.Drawing.Point(140, 16)
        Me._optTransactionRule_1.Name = "_optTransactionRule_1"
        Me._optTransactionRule_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optTransactionRule_1.Size = New System.Drawing.Size(141, 17)
        Me._optTransactionRule_1.TabIndex = 18
        Me._optTransactionRule_1.TabStop = True
        Me._optTransactionRule_1.Text = "Transaction"
        Me._optTransactionRule_1.UseVisualStyleBackColor = False
        '
        '_optTransactionRule_0
        '
        Me._optTransactionRule_0.BackColor = System.Drawing.SystemColors.Control
        Me._optTransactionRule_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optTransactionRule_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optTransactionRule_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optTransactionRule_0.Location = New System.Drawing.Point(18, 16)
        Me._optTransactionRule_0.Name = "_optTransactionRule_0"
        Me._optTransactionRule_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optTransactionRule_0.Size = New System.Drawing.Size(141, 17)
        Me._optTransactionRule_0.TabIndex = 17
        Me._optTransactionRule_0.TabStop = True
        Me._optTransactionRule_0.Text = "Receipt"
        Me._optTransactionRule_0.UseVisualStyleBackColor = False
        '
        'frmBankAccountRuleEdit
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(434, 397)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmBankAccountRuleEdit"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Edit Bank Account Rule"
        Me.tabMainTab.ResumeLayout(False)
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.Frame1.ResumeLayout(False)
        Me.fraRuleType.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
	Sub InitializeoptTransactionRule()
		Me.optTransactionRule(2) = _optTransactionRule_2
		Me.optTransactionRule(1) = _optTransactionRule_1
		Me.optTransactionRule(0) = _optTransactionRule_0
	End Sub
	Sub InitializeLine1()
		Me.Line1(4) = _Line1_4
		Me.Line1(3) = _Line1_3
		Me.Line1(2) = _Line1_2
		Me.Line1(1) = _Line1_1
		Me.Line1(0) = _Line1_0
	End Sub
#End Region 
End Class