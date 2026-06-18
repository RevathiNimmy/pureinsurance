<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
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
    Public WithEvents mnuAbout As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
    Public WithEvents cmdLogOff As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents lblUnifiedLogin As System.Windows.Forms.Label
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents lblPMBLink As System.Windows.Forms.Label
    Public WithEvents lblPMBCompany As System.Windows.Forms.Label
    Public WithEvents lblLoggedOnTo As System.Windows.Forms.Label
    Public WithEvents lblLogonTime As System.Windows.Forms.Label
    Public WithEvents lblLogonName As System.Windows.Forms.Label
    Public WithEvents pan3UnifiedLogin As System.Windows.Forms.Panel
    Public WithEvents panVersion As System.Windows.Forms.Panel
    Public WithEvents panSource As System.Windows.Forms.Panel
    Public WithEvents pan3PMBLink As System.Windows.Forms.Panel
    Public WithEvents pan3PMBCompany As System.Windows.Forms.Panel
    Public WithEvents pan3LoggedOnTo As System.Windows.Forms.Panel
    Public WithEvents pan3LogonTime As System.Windows.Forms.Panel
    Public WithEvents pan3LogonName As System.Windows.Forms.Panel
    'Modified by Archana Tokas on 5/26/2010 9:32:17 AM labels added to show the values todolist
    Public WithEvents lbl3UnifiedLogin As System.Windows.Forms.Label
    Public WithEvents lblVersion As System.Windows.Forms.Label
    Public WithEvents lblSource As System.Windows.Forms.Label
    Public WithEvents lbl3PMBLink As System.Windows.Forms.Label
    Public WithEvents lbl3PMBCompany As System.Windows.Forms.Label
    Public WithEvents lbl3LoggedOnTo As System.Windows.Forms.Label
    Public WithEvents lbl3LogonTime As System.Windows.Forms.Label
    Public WithEvents lbl3LogonName As System.Windows.Forms.Label

    Public WithEvents cmdApplyLogDetails As System.Windows.Forms.Button
	Public WithEvents txtLogFilename As System.Windows.Forms.TextBox
	Public WithEvents cmbUserLogLevel As System.Windows.Forms.ComboBox
	Public WithEvents lblDesc2 As System.Windows.Forms.Label
	Public WithEvents lblLogFilename As System.Windows.Forms.Label
	Public WithEvents lblUserLogLevel As System.Windows.Forms.Label
	Public WithEvents lblDesc1 As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents lblUserLanguage As System.Windows.Forms.Label
	Public WithEvents lblOldPassword As System.Windows.Forms.Label
	Public WithEvents lblNewPassword As System.Windows.Forms.Label
	Public WithEvents lblServerPrinter As System.Windows.Forms.Label
	Public WithEvents lblConfirmPassword As System.Windows.Forms.Label
	Public WithEvents cmbUserLanguage As System.Windows.Forms.ComboBox
	Public WithEvents cmdChangePassword As System.Windows.Forms.Button
	Public WithEvents txtOldPassword As System.Windows.Forms.TextBox
	Public WithEvents txtNewPassword As System.Windows.Forms.TextBox
	Public WithEvents cmdApply As System.Windows.Forms.Button
	Public WithEvents cmbServerPrinter As System.Windows.Forms.ComboBox
	Public WithEvents txtConfirmPassword As System.Windows.Forms.TextBox
	Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents Label2 As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAbout = New System.Windows.Forms.ToolStripMenuItem
        Me.cmdLogOff = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblUnifiedLogin = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblPMBLink = New System.Windows.Forms.Label
        Me.lblPMBCompany = New System.Windows.Forms.Label
        Me.lblLoggedOnTo = New System.Windows.Forms.Label
        Me.lblLogonTime = New System.Windows.Forms.Label
        Me.lblLogonName = New System.Windows.Forms.Label
        Me.pan3UnifiedLogin = New System.Windows.Forms.Panel
        Me.lbl3UnifiedLogin = New System.Windows.Forms.Label
        Me.panVersion = New System.Windows.Forms.Panel
        Me.lblVersion = New System.Windows.Forms.Label
        Me.panSource = New System.Windows.Forms.Panel
        Me.lblSource = New System.Windows.Forms.Label
        Me.pan3PMBLink = New System.Windows.Forms.Panel
        Me.lbl3PMBLink = New System.Windows.Forms.Label
        Me.pan3PMBCompany = New System.Windows.Forms.Panel
        Me.lbl3PMBCompany = New System.Windows.Forms.Label
        Me.pan3LoggedOnTo = New System.Windows.Forms.Panel
        Me.lbl3LoggedOnTo = New System.Windows.Forms.Label
        Me.pan3LogonTime = New System.Windows.Forms.Panel
        Me.lbl3LogonTime = New System.Windows.Forms.Label
        Me.pan3LogonName = New System.Windows.Forms.Panel
        Me.lbl3LogonName = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.cmdApplyLogDetails = New System.Windows.Forms.Button
        Me.txtLogFilename = New System.Windows.Forms.TextBox
        Me.cmbUserLogLevel = New System.Windows.Forms.ComboBox
        Me.lblDesc2 = New System.Windows.Forms.Label
        Me.lblLogFilename = New System.Windows.Forms.Label
        Me.lblUserLogLevel = New System.Windows.Forms.Label
        Me.lblDesc1 = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage
        Me.lblUserLanguage = New System.Windows.Forms.Label
        Me.lblOldPassword = New System.Windows.Forms.Label
        Me.lblNewPassword = New System.Windows.Forms.Label
        Me.lblServerPrinter = New System.Windows.Forms.Label
        Me.lblConfirmPassword = New System.Windows.Forms.Label
        Me.cmbUserLanguage = New System.Windows.Forms.ComboBox
        Me.cmdChangePassword = New System.Windows.Forms.Button
        Me.txtOldPassword = New System.Windows.Forms.TextBox
        Me.txtNewPassword = New System.Windows.Forms.TextBox
        Me.cmdApply = New System.Windows.Forms.Button
        Me.cmbServerPrinter = New System.Windows.Forms.ComboBox
        Me.txtConfirmPassword = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.MainMenu1.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.pan3UnifiedLogin.SuspendLayout()
        Me.panVersion.SuspendLayout()
        Me.panSource.SuspendLayout()
        Me.pan3PMBLink.SuspendLayout()
        Me.pan3PMBCompany.SuspendLayout()
        Me.pan3LoggedOnTo.SuspendLayout()
        Me.pan3LogonTime.SuspendLayout()
        Me.pan3LogonName.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuHelp})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(356, 24)
        Me.MainMenu1.TabIndex = 23
        '
        'mnuHelp
        '
        Me.mnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAbout})
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(40, 20)
        Me.mnuHelp.Text = "&Help"
        '
        'mnuAbout
        '
        Me.mnuAbout.Name = "mnuAbout"
        Me.mnuAbout.Size = New System.Drawing.Size(179, 22)
        Me.mnuAbout.Text = "&About Pure Insurance"
        '
        'cmdLogOff
        '
        Me.cmdLogOff.BackColor = System.Drawing.SystemColors.Control
        Me.cmdLogOff.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdLogOff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdLogOff.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdLogOff.Location = New System.Drawing.Point(8, 328)
        Me.cmdLogOff.Name = "cmdLogOff"
        Me.cmdLogOff.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdLogOff.Size = New System.Drawing.Size(73, 22)
        Me.cmdLogOff.TabIndex = 11
        Me.cmdLogOff.Text = "&Log Off"
        Me.cmdLogOff.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdLogOff.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(272, 328)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 13
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
        Me.cmdOK.Location = New System.Drawing.Point(192, 328)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 12
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage1)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage2)
        Me.tabMainTab.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(111, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 32)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(341, 293)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblUnifiedLogin)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Label3)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Label1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPMBLink)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPMBCompany)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblLoggedOnTo)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblLogonTime)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblLogonName)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pan3UnifiedLogin)
        Me._tabMainTab_TabPage0.Controls.Add(Me.panVersion)
        Me._tabMainTab_TabPage0.Controls.Add(Me.panSource)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pan3PMBLink)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pan3PMBCompany)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pan3LoggedOnTo)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pan3LogonTime)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pan3LogonName)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(333, 267)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "General Details"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'lblUnifiedLogin
        '
        Me.lblUnifiedLogin.BackColor = System.Drawing.SystemColors.Control
        Me.lblUnifiedLogin.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUnifiedLogin.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnifiedLogin.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUnifiedLogin.Location = New System.Drawing.Point(12, 88)
        Me.lblUnifiedLogin.Name = "lblUnifiedLogin"
        Me.lblUnifiedLogin.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUnifiedLogin.Size = New System.Drawing.Size(73, 17)
        Me.lblUnifiedLogin.TabIndex = 33
        Me.lblUnifiedLogin.Text = "Unified Login: "
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(12, 24)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(81, 17)
        Me.Label3.TabIndex = 34
        Me.Label3.Text = "Version:"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(12, 184)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(81, 17)
        Me.Label1.TabIndex = 35
        Me.Label1.Text = "Branch:"
        '
        'lblPMBLink
        '
        Me.lblPMBLink.BackColor = System.Drawing.SystemColors.Control
        Me.lblPMBLink.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPMBLink.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPMBLink.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPMBLink.Location = New System.Drawing.Point(12, 216)
        Me.lblPMBLink.Name = "lblPMBLink"
        Me.lblPMBLink.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPMBLink.Size = New System.Drawing.Size(81, 17)
        Me.lblPMBLink.TabIndex = 36
        Me.lblPMBLink.Text = "PMB Link:"
        '
        'lblPMBCompany
        '
        Me.lblPMBCompany.BackColor = System.Drawing.SystemColors.Control
        Me.lblPMBCompany.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPMBCompany.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPMBCompany.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPMBCompany.Location = New System.Drawing.Point(12, 248)
        Me.lblPMBCompany.Name = "lblPMBCompany"
        Me.lblPMBCompany.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPMBCompany.Size = New System.Drawing.Size(81, 17)
        Me.lblPMBCompany.TabIndex = 37
        Me.lblPMBCompany.Text = "PMB Company:"
        '
        'lblLoggedOnTo
        '
        Me.lblLoggedOnTo.BackColor = System.Drawing.SystemColors.Control
        Me.lblLoggedOnTo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLoggedOnTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLoggedOnTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLoggedOnTo.Location = New System.Drawing.Point(12, 152)
        Me.lblLoggedOnTo.Name = "lblLoggedOnTo"
        Me.lblLoggedOnTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLoggedOnTo.Size = New System.Drawing.Size(81, 17)
        Me.lblLoggedOnTo.TabIndex = 38
        Me.lblLoggedOnTo.Text = "Logged On To:"
        '
        'lblLogonTime
        '
        Me.lblLogonTime.BackColor = System.Drawing.SystemColors.Control
        Me.lblLogonTime.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLogonTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLogonTime.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLogonTime.Location = New System.Drawing.Point(12, 120)
        Me.lblLogonTime.Name = "lblLogonTime"
        Me.lblLogonTime.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLogonTime.Size = New System.Drawing.Size(65, 17)
        Me.lblLogonTime.TabIndex = 39
        Me.lblLogonTime.Text = "Logon Time:"
        '
        'lblLogonName
        '
        Me.lblLogonName.BackColor = System.Drawing.SystemColors.Control
        Me.lblLogonName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLogonName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLogonName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLogonName.Location = New System.Drawing.Point(12, 56)
        Me.lblLogonName.Name = "lblLogonName"
        Me.lblLogonName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLogonName.Size = New System.Drawing.Size(73, 17)
        Me.lblLogonName.TabIndex = 40
        Me.lblLogonName.Text = "Logon Name: "
        '
        'pan3UnifiedLogin
        '
        Me.pan3UnifiedLogin.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pan3UnifiedLogin.Controls.Add(Me.lbl3UnifiedLogin)
        Me.pan3UnifiedLogin.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pan3UnifiedLogin.Location = New System.Drawing.Point(108, 88)
        Me.pan3UnifiedLogin.Name = "pan3UnifiedLogin"
        Me.pan3UnifiedLogin.Size = New System.Drawing.Size(209, 20)
        Me.pan3UnifiedLogin.TabIndex = 32
        '
        'lbl3UnifiedLogin
        '
        Me.lbl3UnifiedLogin.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl3UnifiedLogin.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl3UnifiedLogin.Location = New System.Drawing.Point(0, 0)
        Me.lbl3UnifiedLogin.Name = "lbl3UnifiedLogin"
        Me.lbl3UnifiedLogin.Size = New System.Drawing.Size(209, 20)
        Me.lbl3UnifiedLogin.TabIndex = 32
        '
        'panVersion
        '
        Me.panVersion.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panVersion.Controls.Add(Me.lblVersion)
        Me.panVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panVersion.Location = New System.Drawing.Point(108, 24)
        Me.panVersion.Name = "panVersion"
        Me.panVersion.Size = New System.Drawing.Size(209, 20)
        Me.panVersion.TabIndex = 31
        '
        'lblVersion
        '
        Me.lblVersion.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVersion.Location = New System.Drawing.Point(0, 0)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(209, 20)
        Me.lblVersion.TabIndex = 0
        '
        'panSource
        '
        Me.panSource.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panSource.Controls.Add(Me.lblSource)
        Me.panSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panSource.Location = New System.Drawing.Point(108, 184)
        Me.panSource.Name = "panSource"
        Me.panSource.Size = New System.Drawing.Size(209, 20)
        Me.panSource.TabIndex = 30
        '
        'lblSource
        '
        Me.lblSource.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSource.Location = New System.Drawing.Point(0, 0)
        Me.lblSource.Name = "lblSource"
        Me.lblSource.Size = New System.Drawing.Size(209, 20)
        Me.lblSource.TabIndex = 0
        '
        'pan3PMBLink
        '
        Me.pan3PMBLink.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pan3PMBLink.Controls.Add(Me.lbl3PMBLink)
        Me.pan3PMBLink.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pan3PMBLink.Location = New System.Drawing.Point(108, 216)
        Me.pan3PMBLink.Name = "pan3PMBLink"
        Me.pan3PMBLink.Size = New System.Drawing.Size(209, 20)
        Me.pan3PMBLink.TabIndex = 29
        '
        'lbl3PMBLink
        '
        Me.lbl3PMBLink.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl3PMBLink.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl3PMBLink.Location = New System.Drawing.Point(0, 0)
        Me.lbl3PMBLink.Name = "lbl3PMBLink"
        Me.lbl3PMBLink.Size = New System.Drawing.Size(209, 20)
        Me.lbl3PMBLink.TabIndex = 0
        '
        'pan3PMBCompany
        '
        Me.pan3PMBCompany.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pan3PMBCompany.Controls.Add(Me.lbl3PMBCompany)
        Me.pan3PMBCompany.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pan3PMBCompany.Location = New System.Drawing.Point(108, 248)
        Me.pan3PMBCompany.Name = "pan3PMBCompany"
        Me.pan3PMBCompany.Size = New System.Drawing.Size(209, 20)
        Me.pan3PMBCompany.TabIndex = 28
        '
        'lbl3PMBCompany
        '
        Me.lbl3PMBCompany.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl3PMBCompany.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl3PMBCompany.Location = New System.Drawing.Point(0, 0)
        Me.lbl3PMBCompany.Name = "lbl3PMBCompany"
        Me.lbl3PMBCompany.Size = New System.Drawing.Size(209, 20)
        Me.lbl3PMBCompany.TabIndex = 0
        '
        'pan3LoggedOnTo
        '
        Me.pan3LoggedOnTo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pan3LoggedOnTo.Controls.Add(Me.lbl3LoggedOnTo)
        Me.pan3LoggedOnTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pan3LoggedOnTo.Location = New System.Drawing.Point(108, 152)
        Me.pan3LoggedOnTo.Name = "pan3LoggedOnTo"
        Me.pan3LoggedOnTo.Size = New System.Drawing.Size(209, 20)
        Me.pan3LoggedOnTo.TabIndex = 27
        '
        'lbl3LoggedOnTo
        '
        Me.lbl3LoggedOnTo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl3LoggedOnTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl3LoggedOnTo.Location = New System.Drawing.Point(0, 0)
        Me.lbl3LoggedOnTo.Name = "lbl3LoggedOnTo"
        Me.lbl3LoggedOnTo.Size = New System.Drawing.Size(209, 20)
        Me.lbl3LoggedOnTo.TabIndex = 0
        '
        'pan3LogonTime
        '
        Me.pan3LogonTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pan3LogonTime.Controls.Add(Me.lbl3LogonTime)
        Me.pan3LogonTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pan3LogonTime.Location = New System.Drawing.Point(108, 120)
        Me.pan3LogonTime.Name = "pan3LogonTime"
        Me.pan3LogonTime.Size = New System.Drawing.Size(209, 20)
        Me.pan3LogonTime.TabIndex = 26
        '
        'lbl3LogonTime
        '
        Me.lbl3LogonTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl3LogonTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl3LogonTime.Location = New System.Drawing.Point(0, 0)
        Me.lbl3LogonTime.Name = "lbl3LogonTime"
        Me.lbl3LogonTime.Size = New System.Drawing.Size(209, 20)
        Me.lbl3LogonTime.TabIndex = 0
        '
        'pan3LogonName
        '
        Me.pan3LogonName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pan3LogonName.Controls.Add(Me.lbl3LogonName)
        Me.pan3LogonName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pan3LogonName.Location = New System.Drawing.Point(108, 56)
        Me.pan3LogonName.Name = "pan3LogonName"
        Me.pan3LogonName.Size = New System.Drawing.Size(209, 20)
        Me.pan3LogonName.TabIndex = 25
        '
        'lbl3LogonName
        '
        Me.lbl3LogonName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl3LogonName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl3LogonName.Location = New System.Drawing.Point(0, 0)
        Me.lbl3LogonName.Name = "lbl3LogonName"
        Me.lbl3LogonName.Size = New System.Drawing.Size(209, 20)
        Me.lbl3LogonName.TabIndex = 0
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.Frame1)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(333, 267)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "Options"
        Me._tabMainTab_TabPage1.UseVisualStyleBackColor = True
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.cmdApplyLogDetails)
        Me.Frame1.Controls.Add(Me.txtLogFilename)
        Me.Frame1.Controls.Add(Me.cmbUserLogLevel)
        Me.Frame1.Controls.Add(Me.lblDesc2)
        Me.Frame1.Controls.Add(Me.lblLogFilename)
        Me.Frame1.Controls.Add(Me.lblUserLogLevel)
        Me.Frame1.Controls.Add(Me.lblDesc1)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(24, 28)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(289, 217)
        Me.Frame1.TabIndex = 14
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Message Logging"
        '
        'cmdApplyLogDetails
        '
        Me.cmdApplyLogDetails.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApplyLogDetails.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApplyLogDetails.Enabled = False
        Me.cmdApplyLogDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApplyLogDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApplyLogDetails.Location = New System.Drawing.Point(200, 184)
        Me.cmdApplyLogDetails.Name = "cmdApplyLogDetails"
        Me.cmdApplyLogDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApplyLogDetails.Size = New System.Drawing.Size(73, 25)
        Me.cmdApplyLogDetails.TabIndex = 3
        Me.cmdApplyLogDetails.Text = "&Apply"
        Me.cmdApplyLogDetails.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdApplyLogDetails.UseVisualStyleBackColor = False
        '
        'txtLogFilename
        '
        Me.txtLogFilename.AcceptsReturn = True
        Me.txtLogFilename.BackColor = System.Drawing.SystemColors.Window
        Me.txtLogFilename.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLogFilename.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLogFilename.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLogFilename.Location = New System.Drawing.Point(88, 152)
        Me.txtLogFilename.MaxLength = 0
        Me.txtLogFilename.Name = "txtLogFilename"
        Me.txtLogFilename.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLogFilename.Size = New System.Drawing.Size(185, 20)
        Me.txtLogFilename.TabIndex = 2
        '
        'cmbUserLogLevel
        '
        Me.cmbUserLogLevel.BackColor = System.Drawing.SystemColors.Window
        Me.cmbUserLogLevel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbUserLogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbUserLogLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbUserLogLevel.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbUserLogLevel.Location = New System.Drawing.Point(88, 72)
        Me.cmbUserLogLevel.Name = "cmbUserLogLevel"
        Me.cmbUserLogLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbUserLogLevel.Size = New System.Drawing.Size(153, 21)
        Me.cmbUserLogLevel.TabIndex = 1
        '
        'lblDesc2
        '
        Me.lblDesc2.BackColor = System.Drawing.SystemColors.Control
        Me.lblDesc2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDesc2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesc2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDesc2.Location = New System.Drawing.Point(24, 120)
        Me.lblDesc2.Name = "lblDesc2"
        Me.lblDesc2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDesc2.Size = New System.Drawing.Size(225, 17)
        Me.lblDesc2.TabIndex = 18
        Me.lblDesc2.Text = "Enter the log file name below."
        '
        'lblLogFilename
        '
        Me.lblLogFilename.BackColor = System.Drawing.SystemColors.Control
        Me.lblLogFilename.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLogFilename.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLogFilename.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLogFilename.Location = New System.Drawing.Point(24, 154)
        Me.lblLogFilename.Name = "lblLogFilename"
        Me.lblLogFilename.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLogFilename.Size = New System.Drawing.Size(49, 17)
        Me.lblLogFilename.TabIndex = 17
        Me.lblLogFilename.Text = "Log File:"
        '
        'lblUserLogLevel
        '
        Me.lblUserLogLevel.BackColor = System.Drawing.SystemColors.Control
        Me.lblUserLogLevel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUserLogLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserLogLevel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUserLogLevel.Location = New System.Drawing.Point(24, 76)
        Me.lblUserLogLevel.Name = "lblUserLogLevel"
        Me.lblUserLogLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUserLogLevel.Size = New System.Drawing.Size(57, 17)
        Me.lblUserLogLevel.TabIndex = 16
        Me.lblUserLogLevel.Text = "Log Level:"
        '
        'lblDesc1
        '
        Me.lblDesc1.BackColor = System.Drawing.SystemColors.Control
        Me.lblDesc1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDesc1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesc1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDesc1.Location = New System.Drawing.Point(24, 32)
        Me.lblDesc1.Name = "lblDesc1"
        Me.lblDesc1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDesc1.Size = New System.Drawing.Size(249, 49)
        Me.lblDesc1.TabIndex = 15
        Me.lblDesc1.Text = "To change the current message logging level, choose a new level from the list bel" & _
            "ow."
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblUserLanguage)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblOldPassword)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblNewPassword)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblServerPrinter)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblConfirmPassword)
        Me._tabMainTab_TabPage2.Controls.Add(Me.cmbUserLanguage)
        Me._tabMainTab_TabPage2.Controls.Add(Me.cmdChangePassword)
        Me._tabMainTab_TabPage2.Controls.Add(Me.txtOldPassword)
        Me._tabMainTab_TabPage2.Controls.Add(Me.txtNewPassword)
        Me._tabMainTab_TabPage2.Controls.Add(Me.cmdApply)
        Me._tabMainTab_TabPage2.Controls.Add(Me.cmbServerPrinter)
        Me._tabMainTab_TabPage2.Controls.Add(Me.txtConfirmPassword)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(333, 267)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "User Details"
        Me._tabMainTab_TabPage2.UseVisualStyleBackColor = True
        '
        'lblUserLanguage
        '
        Me.lblUserLanguage.BackColor = System.Drawing.SystemColors.Control
        Me.lblUserLanguage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUserLanguage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserLanguage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUserLanguage.Location = New System.Drawing.Point(24, 24)
        Me.lblUserLanguage.Name = "lblUserLanguage"
        Me.lblUserLanguage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUserLanguage.Size = New System.Drawing.Size(81, 17)
        Me.lblUserLanguage.TabIndex = 19
        Me.lblUserLanguage.Text = "Language:"
        '
        'lblOldPassword
        '
        Me.lblOldPassword.BackColor = System.Drawing.SystemColors.Control
        Me.lblOldPassword.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOldPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOldPassword.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOldPassword.Location = New System.Drawing.Point(24, 124)
        Me.lblOldPassword.Name = "lblOldPassword"
        Me.lblOldPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOldPassword.Size = New System.Drawing.Size(81, 17)
        Me.lblOldPassword.TabIndex = 20
        Me.lblOldPassword.Text = "Old Password:"
        '
        'lblNewPassword
        '
        Me.lblNewPassword.BackColor = System.Drawing.SystemColors.Control
        Me.lblNewPassword.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNewPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNewPassword.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNewPassword.Location = New System.Drawing.Point(24, 164)
        Me.lblNewPassword.Name = "lblNewPassword"
        Me.lblNewPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNewPassword.Size = New System.Drawing.Size(81, 17)
        Me.lblNewPassword.TabIndex = 21
        Me.lblNewPassword.Text = "New Password:"
        '
        'lblServerPrinter
        '
        Me.lblServerPrinter.BackColor = System.Drawing.SystemColors.Control
        Me.lblServerPrinter.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblServerPrinter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblServerPrinter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblServerPrinter.Location = New System.Drawing.Point(24, 52)
        Me.lblServerPrinter.Name = "lblServerPrinter"
        Me.lblServerPrinter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblServerPrinter.Size = New System.Drawing.Size(81, 17)
        Me.lblServerPrinter.TabIndex = 23
        Me.lblServerPrinter.Text = "Server Printer:"
        '
        'lblConfirmPassword
        '
        Me.lblConfirmPassword.BackColor = System.Drawing.SystemColors.Control
        Me.lblConfirmPassword.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblConfirmPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConfirmPassword.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblConfirmPassword.Location = New System.Drawing.Point(24, 204)
        Me.lblConfirmPassword.Name = "lblConfirmPassword"
        Me.lblConfirmPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblConfirmPassword.Size = New System.Drawing.Size(105, 25)
        Me.lblConfirmPassword.TabIndex = 24
        Me.lblConfirmPassword.Text = "Confirm Password:"
        '
        'cmbUserLanguage
        '
        Me.cmbUserLanguage.BackColor = System.Drawing.SystemColors.Window
        Me.cmbUserLanguage.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbUserLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbUserLanguage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbUserLanguage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbUserLanguage.Location = New System.Drawing.Point(128, 20)
        Me.cmbUserLanguage.Name = "cmbUserLanguage"
        Me.cmbUserLanguage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbUserLanguage.Size = New System.Drawing.Size(201, 21)
        Me.cmbUserLanguage.TabIndex = 4
        '
        'cmdChangePassword
        '
        Me.cmdChangePassword.BackColor = System.Drawing.SystemColors.Control
        Me.cmdChangePassword.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdChangePassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdChangePassword.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdChangePassword.Location = New System.Drawing.Point(24, 84)
        Me.cmdChangePassword.Name = "cmdChangePassword"
        Me.cmdChangePassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdChangePassword.Size = New System.Drawing.Size(113, 25)
        Me.cmdChangePassword.TabIndex = 6
        Me.cmdChangePassword.Text = "Change Password"
        Me.cmdChangePassword.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdChangePassword.UseVisualStyleBackColor = False
        '
        'txtOldPassword
        '
        Me.txtOldPassword.AcceptsReturn = True
        Me.txtOldPassword.BackColor = System.Drawing.SystemColors.Window
        Me.txtOldPassword.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOldPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOldPassword.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOldPassword.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.txtOldPassword.Location = New System.Drawing.Point(128, 124)
        Me.txtOldPassword.MaxLength = 0
        Me.txtOldPassword.Name = "txtOldPassword"
        Me.txtOldPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtOldPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOldPassword.Size = New System.Drawing.Size(137, 20)
        Me.txtOldPassword.TabIndex = 7
        '
        'txtNewPassword
        '
        Me.txtNewPassword.AcceptsReturn = True
        Me.txtNewPassword.BackColor = System.Drawing.SystemColors.Window
        Me.txtNewPassword.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNewPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNewPassword.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNewPassword.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.txtNewPassword.Location = New System.Drawing.Point(128, 164)
        Me.txtNewPassword.MaxLength = 0
        Me.txtNewPassword.Name = "txtNewPassword"
        Me.txtNewPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtNewPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNewPassword.Size = New System.Drawing.Size(137, 20)
        Me.txtNewPassword.TabIndex = 8
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApply.Enabled = False
        Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Location = New System.Drawing.Point(256, 236)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApply.Size = New System.Drawing.Size(73, 25)
        Me.cmdApply.TabIndex = 10
        Me.cmdApply.Text = "&Apply"
        Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdApply.UseVisualStyleBackColor = False
        '
        'cmbServerPrinter
        '
        Me.cmbServerPrinter.BackColor = System.Drawing.SystemColors.Window
        Me.cmbServerPrinter.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbServerPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbServerPrinter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbServerPrinter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbServerPrinter.Location = New System.Drawing.Point(128, 52)
        Me.cmbServerPrinter.Name = "cmbServerPrinter"
        Me.cmbServerPrinter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbServerPrinter.Size = New System.Drawing.Size(201, 21)
        Me.cmbServerPrinter.TabIndex = 5
        '
        'txtConfirmPassword
        '
        Me.txtConfirmPassword.AcceptsReturn = True
        Me.txtConfirmPassword.BackColor = System.Drawing.SystemColors.Window
        Me.txtConfirmPassword.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtConfirmPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtConfirmPassword.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtConfirmPassword.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.txtConfirmPassword.Location = New System.Drawing.Point(128, 204)
        Me.txtConfirmPassword.MaxLength = 0
        Me.txtConfirmPassword.Name = "txtConfirmPassword"
        Me.txtConfirmPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtConfirmPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtConfirmPassword.Size = New System.Drawing.Size(137, 20)
        Me.txtConfirmPassword.TabIndex = 9
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(32, 192)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(57, 17)
        Me.Label2.TabIndex = 22
        Me.Label2.Text = "Old Password"
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(356, 357)
        Me.Controls.Add(Me.cmdLogOff)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(345, 167)
        Me.MaximizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "User Logon: Details"
        Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.pan3UnifiedLogin.ResumeLayout(False)
        Me.panVersion.ResumeLayout(False)
        Me.panSource.ResumeLayout(False)
        Me.pan3PMBLink.ResumeLayout(False)
        Me.pan3PMBCompany.ResumeLayout(False)
        Me.pan3LoggedOnTo.ResumeLayout(False)
        Me.pan3LogonTime.ResumeLayout(False)
        Me.pan3LogonName.ResumeLayout(False)
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me._tabMainTab_TabPage2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
#End Region 
End Class