<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents imgIcon As System.Windows.Forms.PictureBox
	Public WithEvents lblDate As System.Windows.Forms.Label
	Public WithEvents lblStatus As System.Windows.Forms.Label
	Public WithEvents lblType As System.Windows.Forms.Label
	Public WithEvents lblReference As System.Windows.Forms.Label
	Public WithEvents lblTotalItems As System.Windows.Forms.Label
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	Public WithEvents lblTotalAmount As System.Windows.Forms.Label
	Public WithEvents lblBank As System.Windows.Forms.Label
	Public WithEvents panStatus As System.Windows.Forms.Label
	Public WithEvents uctBankAccount As UserControls.BankAccount
	Public WithEvents uctCurrency As UserControls.CurrencyLookup
	Public WithEvents uctStatus As UserControls.TypeTable
	Public WithEvents txtDate As System.Windows.Forms.TextBox
	Public WithEvents txtReference As System.Windows.Forms.TextBox
	Public WithEvents txtTotalItems As System.Windows.Forms.TextBox
	Public WithEvents txtTotalAmount As System.Windows.Forms.TextBox
    Public WithEvents uctType As PMLookupControl.cboPMLookup
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
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
        Me.imgIcon = New System.Windows.Forms.PictureBox
        Me.lblDate = New System.Windows.Forms.Label
        Me.lblStatus = New System.Windows.Forms.Label
        Me.lblType = New System.Windows.Forms.Label
        Me.lblReference = New System.Windows.Forms.Label
        Me.lblTotalItems = New System.Windows.Forms.Label
        Me.lblCurrency = New System.Windows.Forms.Label
        Me.lblTotalAmount = New System.Windows.Forms.Label
        Me.lblBank = New System.Windows.Forms.Label
        Me.panStatus = New System.Windows.Forms.Label
        Me.uctBankAccount = New UserControls.BankAccount
        Me.uctCurrency = New UserControls.CurrencyLookup
        Me.uctStatus = New UserControls.TypeTable
        Me.txtDate = New System.Windows.Forms.TextBox
        Me.txtReference = New System.Windows.Forms.TextBox
        Me.txtTotalItems = New System.Windows.Forms.TextBox
        Me.txtTotalAmount = New System.Windows.Forms.TextBox
        Me.uctType = New PMLookupControl.cboPMLookup
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(8, 352)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 11
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        Me.cmdNavigate.Visible = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(280, 352)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 10
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
        Me.cmdCancel.Location = New System.Drawing.Point(200, 352)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 9
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
        Me.cmdOK.Location = New System.Drawing.Point(120, 352)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 8
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(68, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(349, 341)
        Me.tabMainTab.TabIndex = 12
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.imgIcon)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblReference)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblTotalItems)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCurrency)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblTotalAmount)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblBank)
        Me._tabMainTab_TabPage0.Controls.Add(Me.panStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.uctBankAccount)
        Me._tabMainTab_TabPage0.Controls.Add(Me.uctCurrency)
        Me._tabMainTab_TabPage0.Controls.Add(Me.uctStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtReference)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtTotalItems)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtTotalAmount)
        Me._tabMainTab_TabPage0.Controls.Add(Me.uctType)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(341, 315)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - General"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'imgIcon
        '
        Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon.Image = CType(resources.GetObject("imgIcon.Image"), System.Drawing.Image)
        Me.imgIcon.Location = New System.Drawing.Point(304, 4)
        Me.imgIcon.Name = "imgIcon"
        Me.imgIcon.Size = New System.Drawing.Size(32, 32)
        Me.imgIcon.TabIndex = 0
        Me.imgIcon.TabStop = False
        '
        'lblDate
        '
        Me.lblDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDate.Location = New System.Drawing.Point(16, 166)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDate.Size = New System.Drawing.Size(73, 17)
        Me.lblDate.TabIndex = 17
        Me.lblDate.Text = "&Date:"
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatus.Location = New System.Drawing.Point(16, 262)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatus.Size = New System.Drawing.Size(73, 17)
        Me.lblStatus.TabIndex = 20
        Me.lblStatus.Text = "&Status:"
        '
        'lblType
        '
        Me.lblType.BackColor = System.Drawing.SystemColors.Control
        Me.lblType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblType.Location = New System.Drawing.Point(16, 54)
        Me.lblType.Name = "lblType"
        Me.lblType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblType.Size = New System.Drawing.Size(73, 17)
        Me.lblType.TabIndex = 14
        Me.lblType.Text = "T&ype:"
        '
        'lblReference
        '
        Me.lblReference.BackColor = System.Drawing.SystemColors.Control
        Me.lblReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReference.Location = New System.Drawing.Point(16, 22)
        Me.lblReference.Name = "lblReference"
        Me.lblReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReference.Size = New System.Drawing.Size(73, 17)
        Me.lblReference.TabIndex = 13
        Me.lblReference.Text = "&Reference:"
        '
        'lblTotalItems
        '
        Me.lblTotalItems.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalItems.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalItems.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalItems.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalItems.Location = New System.Drawing.Point(16, 231)
        Me.lblTotalItems.Name = "lblTotalItems"
        Me.lblTotalItems.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalItems.Size = New System.Drawing.Size(97, 17)
        Me.lblTotalItems.TabIndex = 19
        Me.lblTotalItems.Text = "&Items Total:"
        '
        'lblCurrency
        '
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(16, 119)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(81, 17)
        Me.lblCurrency.TabIndex = 16
        Me.lblCurrency.Text = "&Currency:"
        '
        'lblTotalAmount
        '
        Me.lblTotalAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalAmount.Location = New System.Drawing.Point(16, 199)
        Me.lblTotalAmount.Name = "lblTotalAmount"
        Me.lblTotalAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalAmount.Size = New System.Drawing.Size(81, 16)
        Me.lblTotalAmount.TabIndex = 18
        Me.lblTotalAmount.Text = "&Amounts Total:"
        '
        'lblBank
        '
        Me.lblBank.BackColor = System.Drawing.SystemColors.Control
        Me.lblBank.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBank.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBank.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBank.Location = New System.Drawing.Point(16, 87)
        Me.lblBank.Name = "lblBank"
        Me.lblBank.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBank.Size = New System.Drawing.Size(97, 17)
        Me.lblBank.TabIndex = 15
        Me.lblBank.Text = "&Bank Account:"
        '
        'panStatus
        '
        Me.panStatus.BackColor = System.Drawing.SystemColors.Control
        Me.panStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.panStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.panStatus.Location = New System.Drawing.Point(120, 260)
        Me.panStatus.Name = "panStatus"
        Me.panStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panStatus.Size = New System.Drawing.Size(153, 20)
        Me.panStatus.TabIndex = 6
        '
        'uctBankAccount
        '
        Me.uctBankAccount.DefaultId = "0"
        Me.uctBankAccount.FirstItem = ""
        Me.uctBankAccount.Id = 0
        'Me.uctBankAccount.ListIndex = -1
        Me.uctBankAccount.Location = New System.Drawing.Point(120, 84)
        Me.uctBankAccount.Name = "uctBankAccount"
        Me.uctBankAccount.Size = New System.Drawing.Size(153, 21)
        Me.uctBankAccount.TabIndex = 1
        Me.uctBankAccount.ToolTipText = ""
        Me.uctBankAccount.WhatsThisHelpID = 0
        '
        'uctCurrency
        '
        Me.uctCurrency.CompanyId = 0
        Me.uctCurrency.CurrencyId = 0
        Me.uctCurrency.DefaultCurrencyId = 0
        Me.uctCurrency.FirstItem = ""
        'Me.uctCurrency.ListIndex = -1
        Me.uctCurrency.Location = New System.Drawing.Point(120, 116)
        Me.uctCurrency.Name = "uctCurrency"
        Me.uctCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actCompanyCurrencies
        Me.uctCurrency.Size = New System.Drawing.Size(153, 21)
        Me.uctCurrency.TabIndex = 2
        Me.uctCurrency.ToolTipText = ""
        Me.uctCurrency.WhatsThisHelpID = 0
        '
        'uctStatus
        '
        Me.uctStatus.BackStyle = 0
        Me.uctStatus.BorderStyle = 0
        Me.uctStatus.DefaultItemId = 0
        Me.uctStatus.FirstItem = ""
        Me.uctStatus.ItemCode = ""
        Me.uctStatus.ItemId = 0
        'Me.uctStatus.ListIndex = -1
        Me.uctStatus.Location = New System.Drawing.Point(280, 260)
        Me.uctStatus.Name = "uctStatus"
        Me.uctStatus.Size = New System.Drawing.Size(25, 21)
        Me.uctStatus.Sorted = True
        Me.uctStatus.TabIndex = 7
        Me.uctStatus.Table = UserControls.TypeTable.actTable.actCashListStatus
        Me.uctStatus.TableName = "CashListStatus"
        Me.uctStatus.ToolTipText = ""
        Me.uctStatus.Visible = False
        Me.uctStatus.WhatsThisHelpID = 0
        '
        'txtDate
        '
        Me.txtDate.AcceptsReturn = True
        Me.txtDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDate.Location = New System.Drawing.Point(120, 164)
        Me.txtDate.MaxLength = 0
        Me.txtDate.Name = "txtDate"
        Me.txtDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDate.Size = New System.Drawing.Size(153, 20)
        Me.txtDate.TabIndex = 3
        '
        'txtReference
        '
        Me.txtReference.AcceptsReturn = True
        Me.txtReference.BackColor = System.Drawing.SystemColors.Window
        Me.txtReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtReference.Location = New System.Drawing.Point(120, 20)
        Me.txtReference.MaxLength = 0
        Me.txtReference.Name = "txtReference"
        Me.txtReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtReference.Size = New System.Drawing.Size(153, 20)
        Me.txtReference.TabIndex = 0
        '
        'txtTotalItems
        '
        Me.txtTotalItems.AcceptsReturn = True
        Me.txtTotalItems.BackColor = System.Drawing.SystemColors.Window
        Me.txtTotalItems.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalItems.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalItems.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalItems.Location = New System.Drawing.Point(120, 228)
        Me.txtTotalItems.MaxLength = 0
        Me.txtTotalItems.Name = "txtTotalItems"
        Me.txtTotalItems.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalItems.Size = New System.Drawing.Size(49, 20)
        Me.txtTotalItems.TabIndex = 5
        '
        'txtTotalAmount
        '
        Me.txtTotalAmount.AcceptsReturn = True
        Me.txtTotalAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtTotalAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalAmount.Location = New System.Drawing.Point(120, 196)
        Me.txtTotalAmount.MaxLength = 0
        Me.txtTotalAmount.Name = "txtTotalAmount"
        Me.txtTotalAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalAmount.Size = New System.Drawing.Size(153, 20)
        Me.txtTotalAmount.TabIndex = 4
        '
        'uctType
        '
        Me.uctType.DefaultItemId = 0
        Me.uctType.FirstItem = ""
        Me.uctType.ItemId = 0
        Me.uctType.ListIndex = -1
        Me.uctType.Location = New System.Drawing.Point(120, 52)
        Me.uctType.Name = "uctType"
        Me.uctType.PMLookupProductFamily = 1
        Me.uctType.SingleItemId = 0
        Me.uctType.Size = New System.Drawing.Size(153, 21)
        Me.uctType.Sorted = True
        Me.uctType.TabIndex = 21
        Me.uctType.TableName = "CashListType"
        Me.uctType.ToolTipText = ""
        Me.uctType.WhereClause = ""
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(361, 381)
        Me.Controls.Add(Me.cmdNavigate)
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
        Me.Text = "Cash List"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class