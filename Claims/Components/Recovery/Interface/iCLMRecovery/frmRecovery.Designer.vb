<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRecovery
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializetxtRecoveryType()
		InitializetxtRecoveryAmount()
		InitializelblRecoveryType()
		InitializelblRecoveryAmount()
		Form_Initialize_Renamed()
	End Sub
	Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
		Dispose(True)
	End Sub
    'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents txtReceiptCurrency As System.Windows.Forms.TextBox
	Public WithEvents txtLossCurrency As System.Windows.Forms.TextBox
	Public WithEvents txtClaimNumber As System.Windows.Forms.TextBox
	Public WithEvents txtPerilType As System.Windows.Forms.TextBox
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Private WithEvents _lvwRecovery_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRecovery_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRecovery_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRecovery_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRecovery_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRecovery_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRecovery_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRecovery_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRecovery_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRecovery_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwRecovery As System.Windows.Forms.ListView
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Private WithEvents _lblRecoveryAmount_0 As System.Windows.Forms.Label
	Private WithEvents _lblRecoveryType_0 As System.Windows.Forms.Label
	Private WithEvents _lvwCoinsurance_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCoinsurance_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCoinsurance_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCoinsurance_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwCoinsurance As System.Windows.Forms.ListView
	Private WithEvents _txtRecoveryAmount_0 As System.Windows.Forms.TextBox
	Private WithEvents _txtRecoveryType_0 As System.Windows.Forms.TextBox
	Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
	Private WithEvents _lblRecoveryAmount_1 As System.Windows.Forms.Label
	Private WithEvents _lblRecoveryType_1 As System.Windows.Forms.Label
	Private WithEvents _lvwReinsurance_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwReinsurance_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwReinsurance_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwReinsurance_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwReinsurance As System.Windows.Forms.ListView
	Private WithEvents _txtRecoveryAmount_1 As System.Windows.Forms.TextBox
	Private WithEvents _txtRecoveryType_1 As System.Windows.Forms.TextBox
	Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents imglImages As System.Windows.Forms.ImageList
	Public WithEvents lblReceiptCurrency As System.Windows.Forms.Label
	Public WithEvents lblLossCurrency As System.Windows.Forms.Label
	Public WithEvents lblClaimNumber As System.Windows.Forms.Label
	Public WithEvents lblPerilType As System.Windows.Forms.Label
	Public lblRecoveryAmount(1) As System.Windows.Forms.Label
	Public lblRecoveryType(1) As System.Windows.Forms.Label
	Public txtRecoveryAmount(1) As System.Windows.Forms.TextBox
    Public txtRecoveryType(1) As System.Windows.Forms.TextBox
    'TODOLIST-Commented the listviewhelper as it was conflicting with icon displai in listview)
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRecovery))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtReceiptCurrency = New System.Windows.Forms.TextBox
        Me.txtLossCurrency = New System.Windows.Forms.TextBox
        Me.txtClaimNumber = New System.Windows.Forms.TextBox
        Me.txtPerilType = New System.Windows.Forms.TextBox
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.lvwRecovery = New System.Windows.Forms.ListView
        Me._lvwRecovery_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwRecovery_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwRecovery_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwRecovery_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwRecovery_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwRecovery_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwRecovery_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._lvwRecovery_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
        Me._lvwRecovery_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
        Me._lvwRecovery_ColumnHeader_10 = New System.Windows.Forms.ColumnHeader
        Me.imglImages = New System.Windows.Forms.ImageList(Me.components)
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me._lblRecoveryAmount_0 = New System.Windows.Forms.Label
        Me._lblRecoveryType_0 = New System.Windows.Forms.Label
        Me.lvwCoinsurance = New System.Windows.Forms.ListView
        Me._lvwCoinsurance_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwCoinsurance_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwCoinsurance_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwCoinsurance_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._txtRecoveryAmount_0 = New System.Windows.Forms.TextBox
        Me._txtRecoveryType_0 = New System.Windows.Forms.TextBox
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage
        Me._lblRecoveryAmount_1 = New System.Windows.Forms.Label
        Me._lblRecoveryType_1 = New System.Windows.Forms.Label
        Me.lvwReinsurance = New System.Windows.Forms.ListView
        Me._lvwReinsurance_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwReinsurance_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwReinsurance_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwReinsurance_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._txtRecoveryAmount_1 = New System.Windows.Forms.TextBox
        Me._txtRecoveryType_1 = New System.Windows.Forms.TextBox
        Me.lblReceiptCurrency = New System.Windows.Forms.Label
        Me.lblLossCurrency = New System.Windows.Forms.Label
        Me.lblClaimNumber = New System.Windows.Forms.Label
        Me.lblPerilType = New System.Windows.Forms.Label
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtReceiptCurrency
        '
        Me.txtReceiptCurrency.AcceptsReturn = True
        Me.txtReceiptCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.txtReceiptCurrency.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtReceiptCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptCurrency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtReceiptCurrency.Location = New System.Drawing.Point(444, 36)
        Me.txtReceiptCurrency.MaxLength = 0
        Me.txtReceiptCurrency.Name = "txtReceiptCurrency"
        Me.txtReceiptCurrency.ReadOnly = True
        Me.txtReceiptCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtReceiptCurrency.Size = New System.Drawing.Size(160, 20)
        Me.txtReceiptCurrency.TabIndex = 24
        '
        'txtLossCurrency
        '
        Me.txtLossCurrency.AcceptsReturn = True
        Me.txtLossCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.txtLossCurrency.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLossCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLossCurrency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLossCurrency.Location = New System.Drawing.Point(444, 10)
        Me.txtLossCurrency.MaxLength = 0
        Me.txtLossCurrency.Name = "txtLossCurrency"
        Me.txtLossCurrency.ReadOnly = True
        Me.txtLossCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLossCurrency.Size = New System.Drawing.Size(160, 20)
        Me.txtLossCurrency.TabIndex = 23
        '
        'txtClaimNumber
        '
        Me.txtClaimNumber.AcceptsReturn = True
        Me.txtClaimNumber.BackColor = System.Drawing.SystemColors.Control
        Me.txtClaimNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClaimNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClaimNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClaimNumber.Location = New System.Drawing.Point(121, 10)
        Me.txtClaimNumber.MaxLength = 0
        Me.txtClaimNumber.Name = "txtClaimNumber"
        Me.txtClaimNumber.ReadOnly = True
        Me.txtClaimNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClaimNumber.Size = New System.Drawing.Size(180, 20)
        Me.txtClaimNumber.TabIndex = 11
        '
        'txtPerilType
        '
        Me.txtPerilType.AcceptsReturn = True
        Me.txtPerilType.BackColor = System.Drawing.SystemColors.Control
        Me.txtPerilType.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPerilType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPerilType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPerilType.Location = New System.Drawing.Point(121, 36)
        Me.txtPerilType.MaxLength = 0
        Me.txtPerilType.Name = "txtPerilType"
        Me.txtPerilType.ReadOnly = True
        Me.txtPerilType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPerilType.Size = New System.Drawing.Size(180, 20)
        Me.txtPerilType.TabIndex = 10
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(534, 408)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 2
        Me.cmdHelp.TabStop = False
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(456, 408)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.TabStop = False
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
        Me.cmdOK.Location = New System.Drawing.Point(378, 408)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.TabStop = False
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
        Me.tabMainTab.ItemSize = New System.Drawing.Size(199, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(6, 66)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(605, 341)
        Me.tabMainTab.TabIndex = 3
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAdd)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdEdit)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdDelete)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwRecovery)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(597, 315)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Recovery Amounts"
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(520, 8)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 4
        Me.cmdAdd.TabStop = False
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(520, 36)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 5
        Me.cmdEdit.TabStop = False
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(520, 64)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 6
        Me.cmdDelete.TabStop = False
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'lvwRecovery
        '
        Me.lvwRecovery.BackColor = System.Drawing.SystemColors.Window
        Me.lvwRecovery.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwRecovery.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwRecovery_ColumnHeader_1, Me._lvwRecovery_ColumnHeader_2, Me._lvwRecovery_ColumnHeader_3, Me._lvwRecovery_ColumnHeader_4, Me._lvwRecovery_ColumnHeader_5, Me._lvwRecovery_ColumnHeader_6, Me._lvwRecovery_ColumnHeader_7, Me._lvwRecovery_ColumnHeader_8, Me._lvwRecovery_ColumnHeader_9, Me._lvwRecovery_ColumnHeader_10})
        Me.lvwRecovery.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwRecovery.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwRecovery.FullRowSelect = True
        Me.lvwRecovery.LargeImageList = Me.imglImages
        Me.lvwRecovery.Location = New System.Drawing.Point(6, 8)
        Me.lvwRecovery.Name = "lvwRecovery"
        Me.lvwRecovery.Size = New System.Drawing.Size(509, 301)
        Me.lvwRecovery.SmallImageList = Me.imglImages
        Me.lvwRecovery.TabIndex = 9
        Me.lvwRecovery.UseCompatibleStateImageBehavior = False
        Me.lvwRecovery.View = System.Windows.Forms.View.Details
        '
        '_lvwRecovery_ColumnHeader_1
        '
        Me._lvwRecovery_ColumnHeader_1.Text = "Recovery Type"
        Me._lvwRecovery_ColumnHeader_1.Width = 97
        '
        '_lvwRecovery_ColumnHeader_2
        '
        Me._lvwRecovery_ColumnHeader_2.Text = "Recovery Party Type"
        Me._lvwRecovery_ColumnHeader_2.Width = 97
        '
        '_lvwRecovery_ColumnHeader_3
        '
        Me._lvwRecovery_ColumnHeader_3.Text = "Recovery Party"
        Me._lvwRecovery_ColumnHeader_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwRecovery_ColumnHeader_3.Width = 97
        '
        '_lvwRecovery_ColumnHeader_4
        '
        Me._lvwRecovery_ColumnHeader_4.Text = "Total Reserve"
        Me._lvwRecovery_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwRecovery_ColumnHeader_4.Width = 97
        '
        '_lvwRecovery_ColumnHeader_5
        '
        Me._lvwRecovery_ColumnHeader_5.Text = "Recovered To Date"
        Me._lvwRecovery_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwRecovery_ColumnHeader_5.Width = 97
        '
        '_lvwRecovery_ColumnHeader_6
        '
        Me._lvwRecovery_ColumnHeader_6.Text = "This Recovery (Loss)"
        Me._lvwRecovery_ColumnHeader_6.Width = 97
        '
        '_lvwRecovery_ColumnHeader_7
        '
        Me._lvwRecovery_ColumnHeader_7.Text = "Balance"
        Me._lvwRecovery_ColumnHeader_7.Width = 97
        '
        '_lvwRecovery_ColumnHeader_8
        '
        Me._lvwRecovery_ColumnHeader_8.Text = "Tax Band"
        Me._lvwRecovery_ColumnHeader_8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwRecovery_ColumnHeader_8.Width = 97
        '
        '_lvwRecovery_ColumnHeader_9
        '
        Me._lvwRecovery_ColumnHeader_9.Text = "Tax Amount"
        Me._lvwRecovery_ColumnHeader_9.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwRecovery_ColumnHeader_9.Width = 97
        '
        '_lvwRecovery_ColumnHeader_10
        '
        Me._lvwRecovery_ColumnHeader_10.Text = "Net Receipt"
        Me._lvwRecovery_ColumnHeader_10.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwRecovery_ColumnHeader_10.Width = 97
        '
        'imglImages
        '
        Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.imglImages.Images.SetKeyName(0, "FindImage")
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me._lblRecoveryAmount_0)
        Me._tabMainTab_TabPage1.Controls.Add(Me._lblRecoveryType_0)
        Me._tabMainTab_TabPage1.Controls.Add(Me.lvwCoinsurance)
        Me._tabMainTab_TabPage1.Controls.Add(Me._txtRecoveryAmount_0)
        Me._tabMainTab_TabPage1.Controls.Add(Me._txtRecoveryType_0)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(597, 315)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "2 - Coinsurance Recovery"
        '
        '_lblRecoveryAmount_0
        '
        Me._lblRecoveryAmount_0.AutoSize = True
        Me._lblRecoveryAmount_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblRecoveryAmount_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblRecoveryAmount_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblRecoveryAmount_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblRecoveryAmount_0.Location = New System.Drawing.Point(320, 15)
        Me._lblRecoveryAmount_0.Name = "_lblRecoveryAmount_0"
        Me._lblRecoveryAmount_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblRecoveryAmount_0.Size = New System.Drawing.Size(95, 13)
        Me._lblRecoveryAmount_0.TabIndex = 17
        Me._lblRecoveryAmount_0.Text = "Recovery Amount:"
        '
        '_lblRecoveryType_0
        '
        Me._lblRecoveryType_0.AutoSize = True
        Me._lblRecoveryType_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblRecoveryType_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblRecoveryType_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblRecoveryType_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblRecoveryType_0.Location = New System.Drawing.Point(12, 15)
        Me._lblRecoveryType_0.Name = "_lblRecoveryType_0"
        Me._lblRecoveryType_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblRecoveryType_0.Size = New System.Drawing.Size(83, 13)
        Me._lblRecoveryType_0.TabIndex = 18
        Me._lblRecoveryType_0.Text = "Recovery Type:"
        '
        'lvwCoinsurance
        '
        Me.lvwCoinsurance.BackColor = System.Drawing.SystemColors.Window
        Me.lvwCoinsurance.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwCoinsurance.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwCoinsurance_ColumnHeader_1, Me._lvwCoinsurance_ColumnHeader_2, Me._lvwCoinsurance_ColumnHeader_3, Me._lvwCoinsurance_ColumnHeader_4})
        Me.lvwCoinsurance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwCoinsurance.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwCoinsurance.FullRowSelect = True
        Me.lvwCoinsurance.LargeImageList = Me.imglImages
        Me.lvwCoinsurance.Location = New System.Drawing.Point(6, 38)
        Me.lvwCoinsurance.Name = "lvwCoinsurance"
        Me.lvwCoinsurance.Size = New System.Drawing.Size(587, 271)
        Me.lvwCoinsurance.SmallImageList = Me.imglImages
        Me.lvwCoinsurance.TabIndex = 7
        Me.lvwCoinsurance.UseCompatibleStateImageBehavior = False
        Me.lvwCoinsurance.View = System.Windows.Forms.View.Details
        '
        '_lvwCoinsurance_ColumnHeader_1
        '
        Me._lvwCoinsurance_ColumnHeader_1.Text = "Coinsurer"
        Me._lvwCoinsurance_ColumnHeader_1.Width = 97
        '
        '_lvwCoinsurance_ColumnHeader_2
        '
        Me._lvwCoinsurance_ColumnHeader_2.Text = "Share %"
        Me._lvwCoinsurance_ColumnHeader_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwCoinsurance_ColumnHeader_2.Width = 97
        '
        '_lvwCoinsurance_ColumnHeader_3
        '
        Me._lvwCoinsurance_ColumnHeader_3.Text = "Recovery To Date"
        Me._lvwCoinsurance_ColumnHeader_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwCoinsurance_ColumnHeader_3.Width = 97
        '
        '_lvwCoinsurance_ColumnHeader_4
        '
        Me._lvwCoinsurance_ColumnHeader_4.Text = "This Recovery"
        Me._lvwCoinsurance_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwCoinsurance_ColumnHeader_4.Width = 97
        '
        '_txtRecoveryAmount_0
        '
        Me._txtRecoveryAmount_0.AcceptsReturn = True
        Me._txtRecoveryAmount_0.BackColor = System.Drawing.SystemColors.Control
        Me._txtRecoveryAmount_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtRecoveryAmount_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtRecoveryAmount_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtRecoveryAmount_0.Location = New System.Drawing.Point(438, 12)
        Me._txtRecoveryAmount_0.MaxLength = 0
        Me._txtRecoveryAmount_0.Name = "_txtRecoveryAmount_0"
        Me._txtRecoveryAmount_0.ReadOnly = True
        Me._txtRecoveryAmount_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtRecoveryAmount_0.Size = New System.Drawing.Size(153, 20)
        Me._txtRecoveryAmount_0.TabIndex = 15
        '
        '_txtRecoveryType_0
        '
        Me._txtRecoveryType_0.AcceptsReturn = True
        Me._txtRecoveryType_0.BackColor = System.Drawing.SystemColors.Control
        Me._txtRecoveryType_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtRecoveryType_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtRecoveryType_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtRecoveryType_0.Location = New System.Drawing.Point(115, 12)
        Me._txtRecoveryType_0.MaxLength = 0
        Me._txtRecoveryType_0.Name = "_txtRecoveryType_0"
        Me._txtRecoveryType_0.ReadOnly = True
        Me._txtRecoveryType_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtRecoveryType_0.Size = New System.Drawing.Size(180, 20)
        Me._txtRecoveryType_0.TabIndex = 16
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me._lblRecoveryAmount_1)
        Me._tabMainTab_TabPage2.Controls.Add(Me._lblRecoveryType_1)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lvwReinsurance)
        Me._tabMainTab_TabPage2.Controls.Add(Me._txtRecoveryAmount_1)
        Me._tabMainTab_TabPage2.Controls.Add(Me._txtRecoveryType_1)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(597, 315)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "3 - Reinsurance Recovery"
        '
        '_lblRecoveryAmount_1
        '
        Me._lblRecoveryAmount_1.AutoSize = True
        Me._lblRecoveryAmount_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblRecoveryAmount_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblRecoveryAmount_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblRecoveryAmount_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblRecoveryAmount_1.Location = New System.Drawing.Point(320, 15)
        Me._lblRecoveryAmount_1.Name = "_lblRecoveryAmount_1"
        Me._lblRecoveryAmount_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblRecoveryAmount_1.Size = New System.Drawing.Size(95, 13)
        Me._lblRecoveryAmount_1.TabIndex = 21
        Me._lblRecoveryAmount_1.Text = "Recovery Amount:"
        '
        '_lblRecoveryType_1
        '
        Me._lblRecoveryType_1.AutoSize = True
        Me._lblRecoveryType_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblRecoveryType_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblRecoveryType_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblRecoveryType_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblRecoveryType_1.Location = New System.Drawing.Point(12, 15)
        Me._lblRecoveryType_1.Name = "_lblRecoveryType_1"
        Me._lblRecoveryType_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblRecoveryType_1.Size = New System.Drawing.Size(83, 13)
        Me._lblRecoveryType_1.TabIndex = 22
        Me._lblRecoveryType_1.Text = "Recovery Type:"
        '
        'lvwReinsurance
        '
        Me.lvwReinsurance.BackColor = System.Drawing.SystemColors.Window
        Me.lvwReinsurance.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwReinsurance.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwReinsurance_ColumnHeader_1, Me._lvwReinsurance_ColumnHeader_2, Me._lvwReinsurance_ColumnHeader_3, Me._lvwReinsurance_ColumnHeader_4})
        Me.lvwReinsurance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwReinsurance.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwReinsurance.FullRowSelect = True
        Me.lvwReinsurance.LargeImageList = Me.imglImages
        Me.lvwReinsurance.Location = New System.Drawing.Point(6, 38)
        Me.lvwReinsurance.Name = "lvwReinsurance"
        Me.lvwReinsurance.Size = New System.Drawing.Size(587, 271)
        Me.lvwReinsurance.SmallImageList = Me.imglImages
        Me.lvwReinsurance.TabIndex = 8
        Me.lvwReinsurance.UseCompatibleStateImageBehavior = False
        Me.lvwReinsurance.View = System.Windows.Forms.View.Details
        '
        '_lvwReinsurance_ColumnHeader_1
        '
        Me._lvwReinsurance_ColumnHeader_1.Text = "Reinsurer"
        Me._lvwReinsurance_ColumnHeader_1.Width = 97
        '
        '_lvwReinsurance_ColumnHeader_2
        '
        Me._lvwReinsurance_ColumnHeader_2.Text = "Share %"
        Me._lvwReinsurance_ColumnHeader_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwReinsurance_ColumnHeader_2.Width = 97
        '
        '_lvwReinsurance_ColumnHeader_3
        '
        Me._lvwReinsurance_ColumnHeader_3.Text = "Recovered To Date"
        Me._lvwReinsurance_ColumnHeader_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwReinsurance_ColumnHeader_3.Width = 97
        '
        '_lvwReinsurance_ColumnHeader_4
        '
        Me._lvwReinsurance_ColumnHeader_4.Text = "This Recovery"
        Me._lvwReinsurance_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwReinsurance_ColumnHeader_4.Width = 97
        '
        '_txtRecoveryAmount_1
        '
        Me._txtRecoveryAmount_1.AcceptsReturn = True
        Me._txtRecoveryAmount_1.BackColor = System.Drawing.SystemColors.Control
        Me._txtRecoveryAmount_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtRecoveryAmount_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtRecoveryAmount_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtRecoveryAmount_1.Location = New System.Drawing.Point(438, 12)
        Me._txtRecoveryAmount_1.MaxLength = 0
        Me._txtRecoveryAmount_1.Name = "_txtRecoveryAmount_1"
        Me._txtRecoveryAmount_1.ReadOnly = True
        Me._txtRecoveryAmount_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtRecoveryAmount_1.Size = New System.Drawing.Size(153, 20)
        Me._txtRecoveryAmount_1.TabIndex = 19
        '
        '_txtRecoveryType_1
        '
        Me._txtRecoveryType_1.AcceptsReturn = True
        Me._txtRecoveryType_1.BackColor = System.Drawing.SystemColors.Control
        Me._txtRecoveryType_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtRecoveryType_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtRecoveryType_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtRecoveryType_1.Location = New System.Drawing.Point(115, 12)
        Me._txtRecoveryType_1.MaxLength = 0
        Me._txtRecoveryType_1.Name = "_txtRecoveryType_1"
        Me._txtRecoveryType_1.ReadOnly = True
        Me._txtRecoveryType_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtRecoveryType_1.Size = New System.Drawing.Size(180, 20)
        Me._txtRecoveryType_1.TabIndex = 20
        '
        'lblReceiptCurrency
        '
        Me.lblReceiptCurrency.AutoSize = True
        Me.lblReceiptCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblReceiptCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReceiptCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReceiptCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReceiptCurrency.Location = New System.Drawing.Point(326, 39)
        Me.lblReceiptCurrency.Name = "lblReceiptCurrency"
        Me.lblReceiptCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReceiptCurrency.Size = New System.Drawing.Size(92, 13)
        Me.lblReceiptCurrency.TabIndex = 25
        Me.lblReceiptCurrency.Text = "Receipt Currency:"
        '
        'lblLossCurrency
        '
        Me.lblLossCurrency.AutoSize = True
        Me.lblLossCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblLossCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLossCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLossCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLossCurrency.Location = New System.Drawing.Point(326, 13)
        Me.lblLossCurrency.Name = "lblLossCurrency"
        Me.lblLossCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLossCurrency.Size = New System.Drawing.Size(77, 13)
        Me.lblLossCurrency.TabIndex = 14
        Me.lblLossCurrency.Text = "Loss Currency:"
        '
        'lblClaimNumber
        '
        Me.lblClaimNumber.AutoSize = True
        Me.lblClaimNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblClaimNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClaimNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClaimNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClaimNumber.Location = New System.Drawing.Point(10, 13)
        Me.lblClaimNumber.Name = "lblClaimNumber"
        Me.lblClaimNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClaimNumber.Size = New System.Drawing.Size(75, 13)
        Me.lblClaimNumber.TabIndex = 13
        Me.lblClaimNumber.Text = "Claim Number:"
        '
        'lblPerilType
        '
        Me.lblPerilType.AutoSize = True
        Me.lblPerilType.BackColor = System.Drawing.SystemColors.Control
        Me.lblPerilType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPerilType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPerilType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPerilType.Location = New System.Drawing.Point(10, 39)
        Me.lblPerilType.Name = "lblPerilType"
        Me.lblPerilType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPerilType.Size = New System.Drawing.Size(57, 13)
        Me.lblPerilType.TabIndex = 12
        Me.lblPerilType.Text = "Peril Type:"
        '
        'frmRecovery
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(614, 437)
        Me.Controls.Add(Me.txtReceiptCurrency)
        Me.Controls.Add(Me.txtLossCurrency)
        Me.Controls.Add(Me.txtClaimNumber)
        Me.Controls.Add(Me.txtPerilType)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.lblReceiptCurrency)
        Me.Controls.Add(Me.lblLossCurrency)
        Me.Controls.Add(Me.lblClaimNumber)
        Me.Controls.Add(Me.lblPerilType)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(332, 129)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmRecovery"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Salvage Receipt [Paul Hayes Q199710061516]"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me._tabMainTab_TabPage1.PerformLayout()
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me._tabMainTab_TabPage2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub InitializetxtRecoveryType()
		Me.txtRecoveryType(1) = _txtRecoveryType_1
		Me.txtRecoveryType(0) = _txtRecoveryType_0
	End Sub
	Sub InitializetxtRecoveryAmount()
		Me.txtRecoveryAmount(1) = _txtRecoveryAmount_1
		Me.txtRecoveryAmount(0) = _txtRecoveryAmount_0
	End Sub
	Sub InitializelblRecoveryType()
		Me.lblRecoveryType(1) = _lblRecoveryType_1
		Me.lblRecoveryType(0) = _lblRecoveryType_0
	End Sub
	Sub InitializelblRecoveryAmount()
		Me.lblRecoveryAmount(1) = _lblRecoveryAmount_1
		Me.lblRecoveryAmount(0) = _lblRecoveryAmount_0
	End Sub
#End Region 
End Class
