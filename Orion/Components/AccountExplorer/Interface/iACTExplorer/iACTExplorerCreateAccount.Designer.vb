<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCreateAccount
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
    Public WithEvents lblType As System.Windows.Forms.Label
    Public WithEvents lblCode As System.Windows.Forms.Label
    Public WithEvents lblName As System.Windows.Forms.Label
    Public WithEvents lblPath As System.Windows.Forms.Label
    Public WithEvents lblFullPath As System.Windows.Forms.Label
    Public WithEvents lblTotallingIs As System.Windows.Forms.Label
    Public WithEvents lblReportCode As System.Windows.Forms.Label
    Public WithEvents lblAccountMap As System.Windows.Forms.Label
    Public WithEvents lblTotalHeader As System.Windows.Forms.Label
    Public WithEvents lblCompany As System.Windows.Forms.Label
    Public WithEvents lblSubBranch As System.Windows.Forms.Label
    Public WithEvents lblLedgerID As System.Windows.Forms.Label
    Public WithEvents uctAccountType As UserControls.TypeTable
    Public WithEvents txtCode As System.Windows.Forms.TextBox
    Public WithEvents txtAccName As System.Windows.Forms.TextBox
    Public WithEvents cboTotallingType As System.Windows.Forms.ComboBox
    Public WithEvents cboReportMapId As System.Windows.Forms.ComboBox
    Public WithEvents uctAccountLookup As UserControls.AccountLookup
    Public WithEvents txtDescription As System.Windows.Forms.TextBox
    Public WithEvents cboSubBranch As System.Windows.Forms.ComboBox
    Public WithEvents cboBranch As System.Windows.Forms.ComboBox
    Public WithEvents cboLedgerID As System.Windows.Forms.ComboBox
    Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents tabMain As System.Windows.Forms.TabControl
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tabMain = New System.Windows.Forms.TabControl
        Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblType = New System.Windows.Forms.Label
        Me.lblCode = New System.Windows.Forms.Label
        Me.lblName = New System.Windows.Forms.Label
        Me.lblPath = New System.Windows.Forms.Label
        Me.lblFullPath = New System.Windows.Forms.Label
        Me.lblTotallingIs = New System.Windows.Forms.Label
        Me.lblReportCode = New System.Windows.Forms.Label
        Me.lblAccountMap = New System.Windows.Forms.Label
        Me.lblTotalHeader = New System.Windows.Forms.Label
        Me.lblCompany = New System.Windows.Forms.Label
        Me.lblSubBranch = New System.Windows.Forms.Label
        Me.lblLedgerID = New System.Windows.Forms.Label
        Me.uctAccountType = New UserControls.TypeTable
        Me.txtCode = New System.Windows.Forms.TextBox
        Me.txtAccName = New System.Windows.Forms.TextBox
        Me.cboTotallingType = New System.Windows.Forms.ComboBox
        Me.cboReportMapId = New System.Windows.Forms.ComboBox
        Me.uctAccountLookup = New UserControls.AccountLookup
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.cboSubBranch = New System.Windows.Forms.ComboBox
        Me.cboBranch = New System.Windows.Forms.ComboBox
        Me.cboLedgerID = New System.Windows.Forms.ComboBox
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.tabMain.SuspendLayout()
        Me._tabMain_TabPage0.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me._tabMain_TabPage0)
        Me.tabMain.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMain.ItemSize = New System.Drawing.Size(346, 18)
        Me.tabMain.Location = New System.Drawing.Point(4, 8)
        Me.tabMain.Multiline = True
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(351, 375)
        Me.tabMain.TabIndex = 12
        '
        '_tabMain_TabPage0
        '
        Me._tabMain_TabPage0.Controls.Add(Me.lblType)
        Me._tabMain_TabPage0.Controls.Add(Me.lblCode)
        Me._tabMain_TabPage0.Controls.Add(Me.lblName)
        Me._tabMain_TabPage0.Controls.Add(Me.lblPath)
        Me._tabMain_TabPage0.Controls.Add(Me.lblFullPath)
        Me._tabMain_TabPage0.Controls.Add(Me.lblTotallingIs)
        Me._tabMain_TabPage0.Controls.Add(Me.lblReportCode)
        Me._tabMain_TabPage0.Controls.Add(Me.lblAccountMap)
        Me._tabMain_TabPage0.Controls.Add(Me.lblTotalHeader)
        Me._tabMain_TabPage0.Controls.Add(Me.lblCompany)
        Me._tabMain_TabPage0.Controls.Add(Me.lblSubBranch)
        Me._tabMain_TabPage0.Controls.Add(Me.lblLedgerID)
        Me._tabMain_TabPage0.Controls.Add(Me.uctAccountType)
        Me._tabMain_TabPage0.Controls.Add(Me.txtCode)
        Me._tabMain_TabPage0.Controls.Add(Me.txtAccName)
        Me._tabMain_TabPage0.Controls.Add(Me.cboTotallingType)
        Me._tabMain_TabPage0.Controls.Add(Me.cboReportMapId)
        Me._tabMain_TabPage0.Controls.Add(Me.uctAccountLookup)
        Me._tabMain_TabPage0.Controls.Add(Me.txtDescription)
        Me._tabMain_TabPage0.Controls.Add(Me.cboSubBranch)
        Me._tabMain_TabPage0.Controls.Add(Me.cboBranch)
        Me._tabMain_TabPage0.Controls.Add(Me.cboLedgerID)
        Me._tabMain_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage0.Name = "_tabMain_TabPage0"
        Me._tabMain_TabPage0.Size = New System.Drawing.Size(343, 349)
        Me._tabMain_TabPage0.TabIndex = 0
        Me._tabMain_TabPage0.Text = "Details"
        Me._tabMain_TabPage0.UseVisualStyleBackColor = True
        '
        'lblType
        '
        Me.lblType.AutoSize = True
        Me.lblType.BackColor = System.Drawing.SystemColors.Control
        Me.lblType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblType.Location = New System.Drawing.Point(8, 118)
        Me.lblType.Name = "lblType"
        Me.lblType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblType.Size = New System.Drawing.Size(34, 13)
        Me.lblType.TabIndex = 14
        Me.lblType.Text = "&Type:"
        '
        'lblCode
        '
        Me.lblCode.AutoSize = True
        Me.lblCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCode.Location = New System.Drawing.Point(8, 68)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCode.Size = New System.Drawing.Size(87, 13)
        Me.lblCode.TabIndex = 15
        Me.lblCode.Text = "&Short Name:"
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.BackColor = System.Drawing.SystemColors.Control
        Me.lblName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblName.Location = New System.Drawing.Point(8, 93)
        Me.lblName.Name = "lblName"
        Me.lblName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblName.Size = New System.Drawing.Size(48, 13)
        Me.lblName.TabIndex = 16
        Me.lblName.Text = "&Name:"
        '
        'lblPath
        '
        Me.lblPath.AutoSize = True
        Me.lblPath.BackColor = System.Drawing.SystemColors.Control
        Me.lblPath.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPath.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPath.Location = New System.Drawing.Point(8, 180)
        Me.lblPath.Name = "lblPath"
        Me.lblPath.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPath.Size = New System.Drawing.Size(32, 13)
        Me.lblPath.TabIndex = 17
        Me.lblPath.Text = "Path:"
        '
        'lblFullPath
        '
        Me.lblFullPath.BackColor = System.Drawing.SystemColors.Control
        Me.lblFullPath.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblFullPath.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFullPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFullPath.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFullPath.Location = New System.Drawing.Point(112, 180)
        Me.lblFullPath.Name = "lblFullPath"
        Me.lblFullPath.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFullPath.Size = New System.Drawing.Size(217, 68)
        Me.lblFullPath.TabIndex = 6
        '
        'lblTotallingIs
        '
        Me.lblTotallingIs.AutoSize = True
        Me.lblTotallingIs.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotallingIs.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotallingIs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotallingIs.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotallingIs.Location = New System.Drawing.Point(8, 252)
        Me.lblTotallingIs.Name = "lblTotallingIs"
        Me.lblTotallingIs.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotallingIs.Size = New System.Drawing.Size(77, 13)
        Me.lblTotallingIs.TabIndex = 18
        Me.lblTotallingIs.Text = "&Account Type:"
        '
        'lblReportCode
        '
        Me.lblReportCode.AutoSize = True
        Me.lblReportCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblReportCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReportCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReportCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReportCode.Location = New System.Drawing.Point(8, 300)
        Me.lblReportCode.Name = "lblReportCode"
        Me.lblReportCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReportCode.Size = New System.Drawing.Size(65, 13)
        Me.lblReportCode.TabIndex = 19
        Me.lblReportCode.Text = "&Cost Centre:"
        '
        'lblAccountMap
        '
        Me.lblAccountMap.AutoSize = True
        Me.lblAccountMap.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountMap.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountMap.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountMap.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountMap.Location = New System.Drawing.Point(8, 324)
        Me.lblAccountMap.Name = "lblAccountMap"
        Me.lblAccountMap.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountMap.Size = New System.Drawing.Size(85, 13)
        Me.lblAccountMap.TabIndex = 20
        Me.lblAccountMap.Text = "&Map To Income:"
        '
        'lblTotalHeader
        '
        Me.lblTotalHeader.AutoSize = True
        Me.lblTotalHeader.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalHeader.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalHeader.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalHeader.Location = New System.Drawing.Point(8, 276)
        Me.lblTotalHeader.Name = "lblTotalHeader"
        Me.lblTotalHeader.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalHeader.Size = New System.Drawing.Size(72, 13)
        Me.lblTotalHeader.TabIndex = 21
        Me.lblTotalHeader.Text = "Total &Header:"
        '
        'lblCompany
        '
        Me.lblCompany.AutoSize = True
        Me.lblCompany.BackColor = System.Drawing.SystemColors.Control
        Me.lblCompany.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCompany.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCompany.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCompany.Location = New System.Drawing.Point(8, 15)
        Me.lblCompany.Name = "lblCompany"
        Me.lblCompany.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCompany.Size = New System.Drawing.Size(71, 13)
        Me.lblCompany.TabIndex = 22
        Me.lblCompany.Text = "Company:"
        '
        'lblSubBranch
        '
        Me.lblSubBranch.AutoSize = True
        Me.lblSubBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblSubBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSubBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubBranch.Location = New System.Drawing.Point(8, 39)
        Me.lblSubBranch.Name = "lblSubBranch"
        Me.lblSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSubBranch.Size = New System.Drawing.Size(66, 13)
        Me.lblSubBranch.TabIndex = 23
        Me.lblSubBranch.Text = "Sub &Branch:"
        '
        'lblLedgerID
        '
        Me.lblLedgerID.BackColor = System.Drawing.SystemColors.Control
        Me.lblLedgerID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLedgerID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLedgerID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLedgerID.Location = New System.Drawing.Point(8, 148)
        Me.lblLedgerID.Name = "lblLedgerID"
        Me.lblLedgerID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLedgerID.Size = New System.Drawing.Size(100, 17)
        Me.lblLedgerID.TabIndex = 24
        Me.lblLedgerID.Text = "&Ledger:"
        '
        'uctAccountType
        '
        Me.uctAccountType.BackStyle = 0
        Me.uctAccountType.BorderStyle = 0
        Me.uctAccountType.DefaultItemId = 0
        Me.uctAccountType.FirstItem = ""
        Me.uctAccountType.ItemCode = ""
        Me.uctAccountType.ItemId = 0
        Me.uctAccountType.ListIndex = -1
        Me.uctAccountType.Location = New System.Drawing.Point(112, 116)
        Me.uctAccountType.Name = "uctAccountType"
        Me.uctAccountType.Size = New System.Drawing.Size(161, 21)
        Me.uctAccountType.Sorted = True
        Me.uctAccountType.TabIndex = 4
        Me.uctAccountType.Table = UserControls.TypeTable.actTable.actAccountType
        Me.uctAccountType.TableName = "AccountType"
        Me.uctAccountType.ToolTipText = ""
        Me.uctAccountType.WhatsThisHelpID = 0
        '
        'txtCode
        '
        Me.txtCode.AcceptsReturn = True
        Me.txtCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCode.Location = New System.Drawing.Point(112, 68)
        Me.txtCode.MaxLength = 20
        Me.txtCode.Name = "txtCode"
        Me.txtCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCode.Size = New System.Drawing.Size(217, 20)
        Me.txtCode.TabIndex = 2
        '
        'txtAccName
        '
        Me.txtAccName.AcceptsReturn = True
        Me.txtAccName.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccName.Location = New System.Drawing.Point(112, 89)
        Me.txtAccName.MaxLength = 60
        Me.txtAccName.Name = "txtAccName"
        Me.txtAccName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccName.Size = New System.Drawing.Size(217, 20)
        Me.txtAccName.TabIndex = 3
        '
        'cboTotallingType
        '
        Me.cboTotallingType.BackColor = System.Drawing.SystemColors.Window
        Me.cboTotallingType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTotallingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTotallingType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTotallingType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTotallingType.Location = New System.Drawing.Point(112, 252)
        Me.cboTotallingType.Name = "cboTotallingType"
        Me.cboTotallingType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTotallingType.Size = New System.Drawing.Size(217, 21)
        Me.cboTotallingType.TabIndex = 7
        '
        'cboReportMapId
        '
        Me.cboReportMapId.BackColor = System.Drawing.SystemColors.Window
        Me.cboReportMapId.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboReportMapId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboReportMapId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboReportMapId.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboReportMapId.Location = New System.Drawing.Point(112, 300)
        Me.cboReportMapId.Name = "cboReportMapId"
        Me.cboReportMapId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboReportMapId.Size = New System.Drawing.Size(217, 21)
        Me.cboReportMapId.TabIndex = 9
        '
        'uctAccountLookup
        '
        Me.uctAccountLookup.AccountId = 0
        Me.uctAccountLookup.AllowStoppedAccounts = False
        Me.uctAccountLookup.BackStyle = 0
        Me.uctAccountLookup.CompanyId = 0
        Me.uctAccountLookup.Default_Renamed = False
        Me.uctAccountLookup.Location = New System.Drawing.Point(112, 324)
        Me.uctAccountLookup.LookupCaption = "..."
        Me.uctAccountLookup.LookupHeight = 285
        Me.uctAccountLookup.LookupLeft = 2895
        Me.uctAccountLookup.LookupTextLeft = 0
        Me.uctAccountLookup.LookupTextWidth = 2895
        Me.uctAccountLookup.LookupWidth = 360
        Me.uctAccountLookup.Name = "uctAccountLookup"
        Me.uctAccountLookup.OnlyUpdatableAccounts = False
        Me.uctAccountLookup.SelLength = 0
        Me.uctAccountLookup.SelStart = 0
        Me.uctAccountLookup.SelText = ""
        Me.uctAccountLookup.ShowEditOnFindAccount = False
        Me.uctAccountLookup.Size = New System.Drawing.Size(217, 19)
        Me.uctAccountLookup.TabIndex = 10
        Me.uctAccountLookup.ToolTipText = ""
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(112, 276)
        Me.txtDescription.MaxLength = 0
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(217, 20)
        Me.txtDescription.TabIndex = 8
        '
        'cboSubBranch
        '
        Me.cboSubBranch.BackColor = System.Drawing.SystemColors.Window
        Me.cboSubBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSubBranch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSubBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSubBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSubBranch.Location = New System.Drawing.Point(112, 35)
        Me.cboSubBranch.Name = "cboSubBranch"
        Me.cboSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSubBranch.Size = New System.Drawing.Size(217, 21)
        Me.cboSubBranch.TabIndex = 1
        '
        'cboBranch
        '
        Me.cboBranch.BackColor = System.Drawing.SystemColors.Window
        Me.cboBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBranch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBranch.Location = New System.Drawing.Point(112, 10)
        Me.cboBranch.Name = "cboBranch"
        Me.cboBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBranch.Size = New System.Drawing.Size(217, 21)
        Me.cboBranch.TabIndex = 0
        '
        'cboLedgerID
        '
        Me.cboLedgerID.BackColor = System.Drawing.SystemColors.Window
        Me.cboLedgerID.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboLedgerID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLedgerID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboLedgerID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboLedgerID.Location = New System.Drawing.Point(112, 148)
        Me.cboLedgerID.Name = "cboLedgerID"
        Me.cboLedgerID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboLedgerID.Size = New System.Drawing.Size(155, 21)
        Me.cboLedgerID.TabIndex = 5
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(201, 389)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(74, 23)
        Me.cmdOK.TabIndex = 11
        Me.cmdOK.Text = "OK"
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
        Me.cmdCancel.Location = New System.Drawing.Point(281, 389)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(74, 23)
        Me.cmdCancel.TabIndex = 13
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'frmCreateAccount
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(355, 446)
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCreateAccount"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Create Account"
        Me.tabMain.ResumeLayout(False)
        Me._tabMain_TabPage0.ResumeLayout(False)
        Me._tabMain_TabPage0.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region
End Class

