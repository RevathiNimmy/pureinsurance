<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmInstallation
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        InitializelblFailedemailWorkManagerTask()
    End Sub
    Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
        Dispose(True)
    End Sub
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    Public WithEvents cboOption5068 As System.Windows.Forms.ComboBox
    Public WithEvents chkOption5010 As System.Windows.Forms.CheckBox
    Public WithEvents ChkOption5023 As System.Windows.Forms.CheckBox
    Public WithEvents txtSMTPEmailReturnAddress As System.Windows.Forms.TextBox
    Public WithEvents txtSMTPEmailServerPort As System.Windows.Forms.TextBox
    Public WithEvents txtSMTPEmailServer As System.Windows.Forms.TextBox
    Public WithEvents chkOption5009 As System.Windows.Forms.CheckBox
    Public WithEvents chkOption5008 As System.Windows.Forms.CheckBox
    Private WithEvents _lblFailedemailWorkManagerTask_5068 As System.Windows.Forms.Label
    Public WithEvents lblSMTPEmailReturnAddress As System.Windows.Forms.Label
    Public WithEvents lblSMTPEmailServerPort As System.Windows.Forms.Label
    Public WithEvents lblSMTPEmailServer As System.Windows.Forms.Label
    Public lblFailedemailWorkManagerTask(5068) As System.Windows.Forms.Label
    Public lblOption15(4) As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cboOption5068 = New System.Windows.Forms.ComboBox()
        Me.chkOption5010 = New System.Windows.Forms.CheckBox()
        Me.ChkOption5023 = New System.Windows.Forms.CheckBox()
        Me.txtSMTPEmailReturnAddress = New System.Windows.Forms.TextBox()
        Me.txtSMTPEmailServerPort = New System.Windows.Forms.TextBox()
        Me.txtSMTPEmailServer = New System.Windows.Forms.TextBox()
        Me.chkOption5009 = New System.Windows.Forms.CheckBox()
        Me.chkOption5008 = New System.Windows.Forms.CheckBox()
        Me._lblFailedemailWorkManagerTask_5068 = New System.Windows.Forms.Label()
        Me.lblSMTPEmailReturnAddress = New System.Windows.Forms.Label()
        Me.lblSMTPEmailServerPort = New System.Windows.Forms.Label()
        Me.lblSMTPEmailServer = New System.Windows.Forms.Label()
        Me.cboOption10 = New System.Windows.Forms.ComboBox()
        Me.lblOption10 = New System.Windows.Forms.Label()
        Me.txtSMTPEmailBCCAddress = New System.Windows.Forms.TextBox()
        Me.lblSMTPEmailBCCAddress = New System.Windows.Forms.Label()
        Me.txt5085 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txt5086 = New System.Windows.Forms.TextBox()
        Me.lbl5086 = New System.Windows.Forms.Label()
        Me.chkOption5097 = New System.Windows.Forms.CheckBox()
        Me.lblDocProduction = New System.Windows.Forms.Label()
        Me.lblCCMWebURL = New System.Windows.Forms.Label()
        Me.lblPartner = New System.Windows.Forms.Label()
        Me.lblCustomer = New System.Windows.Forms.Label()
        Me.lblConrtactTypeName = New System.Windows.Forms.Label()
        Me.cmbOption5163 = New System.Windows.Forms.ComboBox()
        Me.txtOption5164 = New System.Windows.Forms.TextBox()
        Me.txtOption5165 = New System.Windows.Forms.TextBox()
        Me.txtOption5166 = New System.Windows.Forms.TextBox()
        Me.txtOption5167 = New System.Windows.Forms.TextBox()
        Me.lblRepositoryProj = New System.Windows.Forms.Label()
        Me.txtOption5168 = New System.Windows.Forms.TextBox()
        Me.lblContractTypeVersion = New System.Windows.Forms.Label()
        Me.txtOption5169 = New System.Windows.Forms.TextBox()
        Me.grpOptionDocProduction = New System.Windows.Forms.GroupBox()
        Me.chkOption5207 = New System.Windows.Forms.CheckBox()
        Me.chkOption5181 = New System.Windows.Forms.CheckBox()
        Me.txtOption5173 = New System.Windows.Forms.TextBox()
        Me.lblOption5173 = New System.Windows.Forms.Label()
        Me.btnCCMTemplateSync = New System.Windows.Forms.Button()
        Me.cmbOption5171 = New System.Windows.Forms.ComboBox()
        Me.lblCCMStatus = New System.Windows.Forms.Label()
        Me.chkOption5170 = New System.Windows.Forms.CheckBox()
        Me.cboOption5146 = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.chkOption5145 = New System.Windows.Forms.CheckBox()
        Me.chkOption5177 = New System.Windows.Forms.CheckBox()
        Me.lblSharepointUserName = New System.Windows.Forms.Label()
        Me.txtOption5178 = New System.Windows.Forms.TextBox()
        Me.lblSharepointPassword = New System.Windows.Forms.Label()
        Me.txtOption5179 = New System.Windows.Forms.TextBox()
        Me.txtUserPassword = New System.Windows.Forms.TextBox()
        Me.lblUserPassword = New System.Windows.Forms.Label()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.chkOption5209 = New System.Windows.Forms.CheckBox()
        Me.txtSMTPUserName = New System.Windows.Forms.TextBox()
        Me.lblSMTPUserName = New System.Windows.Forms.Label()
        Me.chkOption5257 = New System.Windows.Forms.CheckBox()
        Me.lblAppClientId = New System.Windows.Forms.Label()
        Me.txtOption5258 = New System.Windows.Forms.TextBox()
        Me.txtOption5259 = New System.Windows.Forms.TextBox()
        Me.lblTenantId = New System.Windows.Forms.Label()
        Me.grpOptionDocProduction.SuspendLayout()
        Me.SuspendLayout()
        '
        'cboOption5068
        '
        Me.cboOption5068.AccessibleDescription = "Failed Email Workmanager Task Group:"
        Me.cboOption5068.BackColor = System.Drawing.SystemColors.Window
        Me.cboOption5068.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboOption5068.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOption5068.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOption5068.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboOption5068.Location = New System.Drawing.Point(265, 567)
        Me.cboOption5068.Name = "cboOption5068"
        Me.cboOption5068.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboOption5068.Size = New System.Drawing.Size(225, 21)
        Me.cboOption5068.TabIndex = 16
        Me.cboOption5068.Tag = "5068"
        '
        'chkOption5010
        '
        Me.chkOption5010.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption5010.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5010.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption5010.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption5010.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption5010.Location = New System.Drawing.Point(20, 396)
        Me.chkOption5010.Name = "chkOption5010"
        Me.chkOption5010.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption5010.Size = New System.Drawing.Size(291, 25)
        Me.chkOption5010.TabIndex = 7
        Me.chkOption5010.Tag = "5010"
        Me.chkOption5010.Text = "Email documents as PDF:"
        Me.chkOption5010.UseVisualStyleBackColor = False
        '
        'ChkOption5023
        '
        Me.ChkOption5023.BackColor = System.Drawing.SystemColors.Control
        Me.ChkOption5023.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ChkOption5023.Cursor = System.Windows.Forms.Cursors.Default
        Me.ChkOption5023.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkOption5023.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ChkOption5023.Location = New System.Drawing.Point(317, 405)
        Me.ChkOption5023.Name = "ChkOption5023"
        Me.ChkOption5023.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ChkOption5023.Size = New System.Drawing.Size(291, 21)
        Me.ChkOption5023.TabIndex = 8
        Me.ChkOption5023.Tag = "5023"
        Me.ChkOption5023.Text = "Digital Signature for PDF Documents enabled:"
        Me.ChkOption5023.UseVisualStyleBackColor = False
        '
        'txtSMTPEmailReturnAddress
        '
        Me.txtSMTPEmailReturnAddress.AcceptsReturn = True
        Me.txtSMTPEmailReturnAddress.AccessibleDescription = "SMTP Email From Address:"
        Me.txtSMTPEmailReturnAddress.BackColor = System.Drawing.SystemColors.Window
        Me.txtSMTPEmailReturnAddress.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSMTPEmailReturnAddress.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSMTPEmailReturnAddress.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSMTPEmailReturnAddress.Location = New System.Drawing.Point(232, 466)
        Me.txtSMTPEmailReturnAddress.MaxLength = 0
        Me.txtSMTPEmailReturnAddress.Name = "txtSMTPEmailReturnAddress"
        Me.txtSMTPEmailReturnAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSMTPEmailReturnAddress.Size = New System.Drawing.Size(257, 21)
        Me.txtSMTPEmailReturnAddress.TabIndex = 11
        Me.txtSMTPEmailReturnAddress.Tag = "5047"
        '
        'txtSMTPEmailServerPort
        '
        Me.txtSMTPEmailServerPort.AcceptsReturn = True
        Me.txtSMTPEmailServerPort.AccessibleDescription = "SMTP Email Server Port:"
        Me.txtSMTPEmailServerPort.BackColor = System.Drawing.SystemColors.Window
        Me.txtSMTPEmailServerPort.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSMTPEmailServerPort.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSMTPEmailServerPort.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSMTPEmailServerPort.Location = New System.Drawing.Point(232, 444)
        Me.txtSMTPEmailServerPort.MaxLength = 0
        Me.txtSMTPEmailServerPort.Name = "txtSMTPEmailServerPort"
        Me.txtSMTPEmailServerPort.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSMTPEmailServerPort.Size = New System.Drawing.Size(257, 21)
        Me.txtSMTPEmailServerPort.TabIndex = 10
        Me.txtSMTPEmailServerPort.Tag = "5046"
        '
        'txtSMTPEmailServer
        '
        Me.txtSMTPEmailServer.AcceptsReturn = True
        Me.txtSMTPEmailServer.AccessibleDescription = "SMTP Email Server:"
        Me.txtSMTPEmailServer.BackColor = System.Drawing.SystemColors.Window
        Me.txtSMTPEmailServer.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSMTPEmailServer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSMTPEmailServer.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSMTPEmailServer.Location = New System.Drawing.Point(232, 420)
        Me.txtSMTPEmailServer.MaxLength = 0
        Me.txtSMTPEmailServer.Name = "txtSMTPEmailServer"
        Me.txtSMTPEmailServer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSMTPEmailServer.Size = New System.Drawing.Size(257, 21)
        Me.txtSMTPEmailServer.TabIndex = 9
        Me.txtSMTPEmailServer.Tag = "5045"
        '
        'chkOption5009
        '
        Me.chkOption5009.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption5009.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5009.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption5009.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption5009.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption5009.Location = New System.Drawing.Point(354, 35)
        Me.chkOption5009.Margin = New System.Windows.Forms.Padding(0)
        Me.chkOption5009.Name = "chkOption5009"
        Me.chkOption5009.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption5009.Size = New System.Drawing.Size(127, 26)
        Me.chkOption5009.TabIndex = 3
        Me.chkOption5009.Tag = "5009"
        Me.chkOption5009.Text = "Archive as PDF:"
        Me.chkOption5009.UseVisualStyleBackColor = False
        '
        'chkOption5008
        '
        Me.chkOption5008.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption5008.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5008.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption5008.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption5008.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption5008.Location = New System.Drawing.Point(224, 35)
        Me.chkOption5008.Margin = New System.Windows.Forms.Padding(0)
        Me.chkOption5008.Name = "chkOption5008"
        Me.chkOption5008.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption5008.Size = New System.Drawing.Size(106, 26)
        Me.chkOption5008.TabIndex = 2
        Me.chkOption5008.Tag = "5008"
        Me.chkOption5008.Text = "Auto-archive:"
        Me.chkOption5008.UseVisualStyleBackColor = False
        '
        '_lblFailedemailWorkManagerTask_5068
        '
        Me._lblFailedemailWorkManagerTask_5068.AutoSize = True
        Me._lblFailedemailWorkManagerTask_5068.BackColor = System.Drawing.SystemColors.Control
        Me._lblFailedemailWorkManagerTask_5068.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFailedemailWorkManagerTask_5068.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFailedemailWorkManagerTask_5068.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFailedemailWorkManagerTask_5068.Location = New System.Drawing.Point(21, 574)
        Me._lblFailedemailWorkManagerTask_5068.Name = "_lblFailedemailWorkManagerTask_5068"
        Me._lblFailedemailWorkManagerTask_5068.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFailedemailWorkManagerTask_5068.Size = New System.Drawing.Size(232, 13)
        Me._lblFailedemailWorkManagerTask_5068.TabIndex = 17
        Me._lblFailedemailWorkManagerTask_5068.Tag = "5068"
        Me._lblFailedemailWorkManagerTask_5068.Text = "Failed Email Workmanager Task Group:"
        '
        'lblSMTPEmailReturnAddress
        '
        Me.lblSMTPEmailReturnAddress.BackColor = System.Drawing.SystemColors.Control
        Me.lblSMTPEmailReturnAddress.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSMTPEmailReturnAddress.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSMTPEmailReturnAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSMTPEmailReturnAddress.Location = New System.Drawing.Point(21, 475)
        Me.lblSMTPEmailReturnAddress.Name = "lblSMTPEmailReturnAddress"
        Me.lblSMTPEmailReturnAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSMTPEmailReturnAddress.Size = New System.Drawing.Size(214, 15)
        Me.lblSMTPEmailReturnAddress.TabIndex = 15
        Me.lblSMTPEmailReturnAddress.Tag = "5047"
        Me.lblSMTPEmailReturnAddress.Text = "SMTP Email From Address:"
        '
        'lblSMTPEmailServerPort
        '
        Me.lblSMTPEmailServerPort.BackColor = System.Drawing.SystemColors.Control
        Me.lblSMTPEmailServerPort.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSMTPEmailServerPort.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSMTPEmailServerPort.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSMTPEmailServerPort.Location = New System.Drawing.Point(21, 451)
        Me.lblSMTPEmailServerPort.Name = "lblSMTPEmailServerPort"
        Me.lblSMTPEmailServerPort.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSMTPEmailServerPort.Size = New System.Drawing.Size(214, 15)
        Me.lblSMTPEmailServerPort.TabIndex = 14
        Me.lblSMTPEmailServerPort.Tag = "5046"
        Me.lblSMTPEmailServerPort.Text = "SMTP Email Server Port:"
        '
        'lblSMTPEmailServer
        '
        Me.lblSMTPEmailServer.BackColor = System.Drawing.SystemColors.Control
        Me.lblSMTPEmailServer.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSMTPEmailServer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSMTPEmailServer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSMTPEmailServer.Location = New System.Drawing.Point(21, 428)
        Me.lblSMTPEmailServer.Name = "lblSMTPEmailServer"
        Me.lblSMTPEmailServer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSMTPEmailServer.Size = New System.Drawing.Size(214, 15)
        Me.lblSMTPEmailServer.TabIndex = 13
        Me.lblSMTPEmailServer.Tag = "5045"
        Me.lblSMTPEmailServer.Text = "SMTP Email Server:"
        '
        'cboOption10
        '
        Me.cboOption10.AccessibleDescription = "Document Archive:"
        Me.cboOption10.BackColor = System.Drawing.SystemColors.Window
        Me.cboOption10.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboOption10.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOption10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOption10.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboOption10.Location = New System.Drawing.Point(224, 11)
        Me.cboOption10.Name = "cboOption10"
        Me.cboOption10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboOption10.Size = New System.Drawing.Size(257, 21)
        Me.cboOption10.TabIndex = 18
        Me.cboOption10.Tag = "10"
        '
        'lblOption10
        '
        Me.lblOption10.AutoSize = True
        Me.lblOption10.BackColor = System.Drawing.SystemColors.Control
        Me.lblOption10.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOption10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOption10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOption10.Location = New System.Drawing.Point(12, 14)
        Me.lblOption10.Name = "lblOption10"
        Me.lblOption10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOption10.Size = New System.Drawing.Size(117, 13)
        Me.lblOption10.TabIndex = 19
        Me.lblOption10.Tag = "10,,M"
        Me.lblOption10.Text = "Document Archive:"
        '
        'txtSMTPEmailBCCAddress
        '
        Me.txtSMTPEmailBCCAddress.AcceptsReturn = True
        Me.txtSMTPEmailBCCAddress.AccessibleDescription = "SMTP Email BCC Address:"
        Me.txtSMTPEmailBCCAddress.BackColor = System.Drawing.SystemColors.Window
        Me.txtSMTPEmailBCCAddress.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSMTPEmailBCCAddress.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSMTPEmailBCCAddress.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSMTPEmailBCCAddress.Location = New System.Drawing.Point(232, 493)
        Me.txtSMTPEmailBCCAddress.MaxLength = 0
        Me.txtSMTPEmailBCCAddress.Name = "txtSMTPEmailBCCAddress"
        Me.txtSMTPEmailBCCAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSMTPEmailBCCAddress.Size = New System.Drawing.Size(257, 21)
        Me.txtSMTPEmailBCCAddress.TabIndex = 24
        Me.txtSMTPEmailBCCAddress.Tag = "5094"
        '
        'lblSMTPEmailBCCAddress
        '
        Me.lblSMTPEmailBCCAddress.BackColor = System.Drawing.SystemColors.Control
        Me.lblSMTPEmailBCCAddress.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSMTPEmailBCCAddress.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSMTPEmailBCCAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSMTPEmailBCCAddress.Location = New System.Drawing.Point(21, 498)
        Me.lblSMTPEmailBCCAddress.Name = "lblSMTPEmailBCCAddress"
        Me.lblSMTPEmailBCCAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSMTPEmailBCCAddress.Size = New System.Drawing.Size(214, 16)
        Me.lblSMTPEmailBCCAddress.TabIndex = 25
        Me.lblSMTPEmailBCCAddress.Tag = "5094"
        Me.lblSMTPEmailBCCAddress.Text = "SMTP Email BCC Address:"
        '
        'txt5085
        '
        Me.txt5085.AcceptsReturn = True
        Me.txt5085.AccessibleDescription = "Sharepoint Server:"
        Me.txt5085.BackColor = System.Drawing.SystemColors.Window
        Me.txt5085.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txt5085.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt5085.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txt5085.Location = New System.Drawing.Point(221, 62)
        Me.txt5085.MaxLength = 0
        Me.txt5085.Name = "txt5085"
        Me.txt5085.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txt5085.Size = New System.Drawing.Size(244, 21)
        Me.txt5085.TabIndex = 26
        Me.txt5085.Tag = "5085,ValidateRequired"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(12, 65)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(214, 11)
        Me.Label1.TabIndex = 27
        Me.Label1.Tag = "5085"
        Me.Label1.Text = "Sharepoint Server:"
        '
        'txt5086
        '
        Me.txt5086.AcceptsReturn = True
        Me.txt5086.AccessibleDescription = "Sharepoint Document Library:"
        Me.txt5086.BackColor = System.Drawing.SystemColors.Window
        Me.txt5086.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txt5086.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt5086.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txt5086.Location = New System.Drawing.Point(221, 86)
        Me.txt5086.MaxLength = 0
        Me.txt5086.Name = "txt5086"
        Me.txt5086.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txt5086.Size = New System.Drawing.Size(244, 21)
        Me.txt5086.TabIndex = 28
        Me.txt5086.Tag = "5086,ValidateRequired"
        '
        'lbl5086
        '
        Me.lbl5086.BackColor = System.Drawing.SystemColors.Control
        Me.lbl5086.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbl5086.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl5086.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbl5086.Location = New System.Drawing.Point(12, 89)
        Me.lbl5086.Name = "lbl5086"
        Me.lbl5086.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbl5086.Size = New System.Drawing.Size(214, 18)
        Me.lbl5086.TabIndex = 29
        Me.lbl5086.Tag = "5086"
        Me.lbl5086.Text = "Sharepoint Document Library:"
        '
        'chkOption5097
        '
        Me.chkOption5097.AutoSize = True
        Me.chkOption5097.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5097.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption5097.Location = New System.Drawing.Point(23, 594)
        Me.chkOption5097.Name = "chkOption5097"
        Me.chkOption5097.Size = New System.Drawing.Size(237, 17)
        Me.chkOption5097.TabIndex = 24
        Me.chkOption5097.Tag = "5097"
        Me.chkOption5097.Text = "Produce Document without Message:"
        Me.chkOption5097.UseVisualStyleBackColor = False
        '
        'lblDocProduction
        '
        Me.lblDocProduction.AutoSize = True
        Me.lblDocProduction.Location = New System.Drawing.Point(6, 26)
        Me.lblDocProduction.Name = "lblDocProduction"
        Me.lblDocProduction.Size = New System.Drawing.Size(176, 13)
        Me.lblDocProduction.TabIndex = 0
        Me.lblDocProduction.Text = "Document Production System"
        '
        'lblCCMWebURL
        '
        Me.lblCCMWebURL.AutoSize = True
        Me.lblCCMWebURL.Location = New System.Drawing.Point(6, 75)
        Me.lblCCMWebURL.Name = "lblCCMWebURL"
        Me.lblCCMWebURL.Size = New System.Drawing.Size(131, 13)
        Me.lblCCMWebURL.TabIndex = 1
        Me.lblCCMWebURL.Text = "KCM web service URL"
        '
        'lblPartner
        '
        Me.lblPartner.AutoSize = True
        Me.lblPartner.Location = New System.Drawing.Point(6, 102)
        Me.lblPartner.Name = "lblPartner"
        Me.lblPartner.Size = New System.Drawing.Size(49, 13)
        Me.lblPartner.TabIndex = 2
        Me.lblPartner.Text = "Partner"
        '
        'lblCustomer
        '
        Me.lblCustomer.AutoSize = True
        Me.lblCustomer.Location = New System.Drawing.Point(6, 129)
        Me.lblCustomer.Name = "lblCustomer"
        Me.lblCustomer.Size = New System.Drawing.Size(63, 13)
        Me.lblCustomer.TabIndex = 3
        Me.lblCustomer.Text = "Customer"
        '
        'lblConrtactTypeName
        '
        Me.lblConrtactTypeName.AutoSize = True
        Me.lblConrtactTypeName.Location = New System.Drawing.Point(6, 156)
        Me.lblConrtactTypeName.Name = "lblConrtactTypeName"
        Me.lblConrtactTypeName.Size = New System.Drawing.Size(124, 13)
        Me.lblConrtactTypeName.TabIndex = 4
        Me.lblConrtactTypeName.Text = "Contract Type Name"
        '
        'cmbOption5163
        '
        Me.cmbOption5163.AccessibleDescription = "Document Production System"
        Me.cmbOption5163.BackColor = System.Drawing.SystemColors.Window
        Me.cmbOption5163.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbOption5163.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOption5163.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbOption5163.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbOption5163.Location = New System.Drawing.Point(214, 21)
        Me.cmbOption5163.Name = "cmbOption5163"
        Me.cmbOption5163.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbOption5163.Size = New System.Drawing.Size(350, 21)
        Me.cmbOption5163.TabIndex = 7
        Me.cmbOption5163.Tag = "5163"
        '
        'txtOption5164
        '
        Me.txtOption5164.AcceptsReturn = True
        Me.txtOption5164.AccessibleDescription = "KCM web service URL"
        Me.txtOption5164.BackColor = System.Drawing.SystemColors.Window
        Me.txtOption5164.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOption5164.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOption5164.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOption5164.Location = New System.Drawing.Point(213, 72)
        Me.txtOption5164.MaxLength = 0
        Me.txtOption5164.Name = "txtOption5164"
        Me.txtOption5164.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOption5164.Size = New System.Drawing.Size(357, 21)
        Me.txtOption5164.TabIndex = 10
        Me.txtOption5164.Tag = "5164"
        '
        'txtOption5165
        '
        Me.txtOption5165.AcceptsReturn = True
        Me.txtOption5165.AccessibleDescription = "Document Production: Partner"
        Me.txtOption5165.BackColor = System.Drawing.SystemColors.Window
        Me.txtOption5165.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOption5165.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOption5165.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOption5165.Location = New System.Drawing.Point(213, 99)
        Me.txtOption5165.MaxLength = 0
        Me.txtOption5165.Name = "txtOption5165"
        Me.txtOption5165.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOption5165.Size = New System.Drawing.Size(109, 21)
        Me.txtOption5165.TabIndex = 11
        Me.txtOption5165.Tag = "5165"
        '
        'txtOption5166
        '
        Me.txtOption5166.AcceptsReturn = True
        Me.txtOption5166.AccessibleDescription = "Document Production: Customer"
        Me.txtOption5166.BackColor = System.Drawing.SystemColors.Window
        Me.txtOption5166.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOption5166.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOption5166.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOption5166.Location = New System.Drawing.Point(213, 126)
        Me.txtOption5166.MaxLength = 0
        Me.txtOption5166.Name = "txtOption5166"
        Me.txtOption5166.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOption5166.Size = New System.Drawing.Size(109, 21)
        Me.txtOption5166.TabIndex = 12
        Me.txtOption5166.Tag = "5166"
        '
        'txtOption5167
        '
        Me.txtOption5167.AcceptsReturn = True
        Me.txtOption5167.AccessibleDescription = "Document Production: Contract Type Name"
        Me.txtOption5167.BackColor = System.Drawing.SystemColors.Window
        Me.txtOption5167.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOption5167.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOption5167.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOption5167.Location = New System.Drawing.Point(213, 153)
        Me.txtOption5167.MaxLength = 0
        Me.txtOption5167.Name = "txtOption5167"
        Me.txtOption5167.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOption5167.Size = New System.Drawing.Size(109, 21)
        Me.txtOption5167.TabIndex = 13
        Me.txtOption5167.Tag = "5167"
        '
        'lblRepositoryProj
        '
        Me.lblRepositoryProj.AutoSize = True
        Me.lblRepositoryProj.Location = New System.Drawing.Point(327, 129)
        Me.lblRepositoryProj.Name = "lblRepositoryProj"
        Me.lblRepositoryProj.Size = New System.Drawing.Size(112, 13)
        Me.lblRepositoryProj.TabIndex = 14
        Me.lblRepositoryProj.Text = "Repository Project"
        '
        'txtOption5168
        '
        Me.txtOption5168.AcceptsReturn = True
        Me.txtOption5168.AccessibleDescription = "Document Production: Repository Project"
        Me.txtOption5168.BackColor = System.Drawing.SystemColors.Window
        Me.txtOption5168.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOption5168.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOption5168.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOption5168.Location = New System.Drawing.Point(461, 126)
        Me.txtOption5168.MaxLength = 0
        Me.txtOption5168.Name = "txtOption5168"
        Me.txtOption5168.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOption5168.Size = New System.Drawing.Size(109, 21)
        Me.txtOption5168.TabIndex = 15
        Me.txtOption5168.Tag = "5168"
        '
        'lblContractTypeVersion
        '
        Me.lblContractTypeVersion.AutoSize = True
        Me.lblContractTypeVersion.Location = New System.Drawing.Point(327, 156)
        Me.lblContractTypeVersion.Name = "lblContractTypeVersion"
        Me.lblContractTypeVersion.Size = New System.Drawing.Size(133, 13)
        Me.lblContractTypeVersion.TabIndex = 16
        Me.lblContractTypeVersion.Text = "Contract Type Version"
        '
        'txtOption5169
        '
        Me.txtOption5169.AcceptsReturn = True
        Me.txtOption5169.AccessibleDescription = "Document Production: Contract Type Version"
        Me.txtOption5169.BackColor = System.Drawing.SystemColors.Window
        Me.txtOption5169.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOption5169.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOption5169.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOption5169.Location = New System.Drawing.Point(461, 153)
        Me.txtOption5169.MaxLength = 0
        Me.txtOption5169.Name = "txtOption5169"
        Me.txtOption5169.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOption5169.Size = New System.Drawing.Size(109, 21)
        Me.txtOption5169.TabIndex = 17
        Me.txtOption5169.Tag = "5169"
        '
        'grpOptionDocProduction
        '
        Me.grpOptionDocProduction.Controls.Add(Me.chkOption5207)
        Me.grpOptionDocProduction.Controls.Add(Me.chkOption5181)
        Me.grpOptionDocProduction.Controls.Add(Me.txtOption5173)
        Me.grpOptionDocProduction.Controls.Add(Me.lblOption5173)
        Me.grpOptionDocProduction.Controls.Add(Me.btnCCMTemplateSync)
        Me.grpOptionDocProduction.Controls.Add(Me.cmbOption5171)
        Me.grpOptionDocProduction.Controls.Add(Me.lblCCMStatus)
        Me.grpOptionDocProduction.Controls.Add(Me.chkOption5170)
        Me.grpOptionDocProduction.Controls.Add(Me.txtOption5169)
        Me.grpOptionDocProduction.Controls.Add(Me.lblContractTypeVersion)
        Me.grpOptionDocProduction.Controls.Add(Me.txtOption5168)
        Me.grpOptionDocProduction.Controls.Add(Me.lblRepositoryProj)
        Me.grpOptionDocProduction.Controls.Add(Me.txtOption5167)
        Me.grpOptionDocProduction.Controls.Add(Me.txtOption5166)
        Me.grpOptionDocProduction.Controls.Add(Me.txtOption5165)
        Me.grpOptionDocProduction.Controls.Add(Me.txtOption5164)
        Me.grpOptionDocProduction.Controls.Add(Me.cmbOption5163)
        Me.grpOptionDocProduction.Controls.Add(Me.lblConrtactTypeName)
        Me.grpOptionDocProduction.Controls.Add(Me.lblCustomer)
        Me.grpOptionDocProduction.Controls.Add(Me.lblPartner)
        Me.grpOptionDocProduction.Controls.Add(Me.lblCCMWebURL)
        Me.grpOptionDocProduction.Controls.Add(Me.lblDocProduction)
        Me.grpOptionDocProduction.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.grpOptionDocProduction.Location = New System.Drawing.Point(15, 162)
        Me.grpOptionDocProduction.Name = "grpOptionDocProduction"
        Me.grpOptionDocProduction.Size = New System.Drawing.Size(577, 235)
        Me.grpOptionDocProduction.TabIndex = 33
        Me.grpOptionDocProduction.TabStop = False
        Me.grpOptionDocProduction.Text = "Document Production"
        Me.grpOptionDocProduction.Visible = False
        '
        'chkOption5207
        '
        Me.chkOption5207.AutoSize = True
        Me.chkOption5207.Location = New System.Drawing.Point(4, 49)
        Me.chkOption5207.Name = "chkOption5207"
        Me.chkOption5207.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.chkOption5207.Size = New System.Drawing.Size(225, 17)
        Me.chkOption5207.TabIndex = 52
        Me.chkOption5207.Tag = "5207"
        Me.chkOption5207.Text = "Applicable for selected documents "
        Me.chkOption5207.UseVisualStyleBackColor = True
        '
        'chkOption5181
        '
        Me.chkOption5181.AutoSize = True
        Me.chkOption5181.Location = New System.Drawing.Point(327, 182)
        Me.chkOption5181.Name = "chkOption5181"
        Me.chkOption5181.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.chkOption5181.Size = New System.Drawing.Size(155, 17)
        Me.chkOption5181.TabIndex = 41
        Me.chkOption5181.Tag = "5181"
        Me.chkOption5181.Text = "Enable dataset logging"
        Me.chkOption5181.UseVisualStyleBackColor = True
        '
        'txtOption5173
        '
        Me.txtOption5173.AcceptsReturn = True
        Me.txtOption5173.AccessibleDescription = "Document Production: Clause Storage Location"
        Me.txtOption5173.BackColor = System.Drawing.SystemColors.Window
        Me.txtOption5173.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOption5173.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOption5173.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOption5173.Location = New System.Drawing.Point(213, 205)
        Me.txtOption5173.MaxLength = 0
        Me.txtOption5173.Name = "txtOption5173"
        Me.txtOption5173.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOption5173.Size = New System.Drawing.Size(110, 21)
        Me.txtOption5173.TabIndex = 39
        Me.txtOption5173.Tag = "5173"
        '
        'lblOption5173
        '
        Me.lblOption5173.AutoSize = True
        Me.lblOption5173.Location = New System.Drawing.Point(6, 208)
        Me.lblOption5173.Name = "lblOption5173"
        Me.lblOption5173.Size = New System.Drawing.Size(146, 13)
        Me.lblOption5173.TabIndex = 38
        Me.lblOption5173.Text = "Clause Storage Location"
        '
        'btnCCMTemplateSync
        '
        Me.btnCCMTemplateSync.Location = New System.Drawing.Point(425, 202)
        Me.btnCCMTemplateSync.Name = "btnCCMTemplateSync"
        Me.btnCCMTemplateSync.Size = New System.Drawing.Size(145, 23)
        Me.btnCCMTemplateSync.TabIndex = 37
        Me.btnCCMTemplateSync.Tag = "5172, CCMTemplateSync"
        Me.btnCCMTemplateSync.Text = "KCM Templates Sync"
        Me.btnCCMTemplateSync.UseVisualStyleBackColor = True
        '
        'cmbOption5171
        '
        Me.cmbOption5171.AccessibleDescription = "KCM Status"
        Me.cmbOption5171.BackColor = System.Drawing.SystemColors.Window
        Me.cmbOption5171.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbOption5171.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOption5171.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbOption5171.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbOption5171.Location = New System.Drawing.Point(213, 180)
        Me.cmbOption5171.Name = "cmbOption5171"
        Me.cmbOption5171.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbOption5171.Size = New System.Drawing.Size(110, 21)
        Me.cmbOption5171.TabIndex = 36
        Me.cmbOption5171.Tag = "5171"
        '
        'lblCCMStatus
        '
        Me.lblCCMStatus.AutoSize = True
        Me.lblCCMStatus.Location = New System.Drawing.Point(6, 183)
        Me.lblCCMStatus.Name = "lblCCMStatus"
        Me.lblCCMStatus.Size = New System.Drawing.Size(73, 13)
        Me.lblCCMStatus.TabIndex = 35
        Me.lblCCMStatus.Text = "KCM Status"
        '
        'chkOption5170
        '
        Me.chkOption5170.AutoSize = True
        Me.chkOption5170.Location = New System.Drawing.Point(327, 102)
        Me.chkOption5170.Name = "chkOption5170"
        Me.chkOption5170.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.chkOption5170.Size = New System.Drawing.Size(148, 17)
        Me.chkOption5170.TabIndex = 34
        Me.chkOption5170.Tag = "5170"
        Me.chkOption5170.Text = "                         SSL"
        Me.chkOption5170.UseVisualStyleBackColor = True
        '
        'cboOption5146
        '
        Me.cboOption5146.AccessibleDescription = "Timestamp Format:"
        Me.cboOption5146.BackColor = System.Drawing.SystemColors.Window
        Me.cboOption5146.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboOption5146.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOption5146.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOption5146.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboOption5146.Location = New System.Drawing.Point(233, 615)
        Me.cboOption5146.Name = "cboOption5146"
        Me.cboOption5146.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboOption5146.Size = New System.Drawing.Size(227, 21)
        Me.cboOption5146.TabIndex = 41
        Me.cboOption5146.Tag = "5146"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(23, 617)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(119, 13)
        Me.Label2.TabIndex = 42
        Me.Label2.Tag = ""
        Me.Label2.Text = "Timestamp Format:"
        '
        'chkOption5145
        '
        Me.chkOption5145.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption5145.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5145.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption5145.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption5145.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption5145.Location = New System.Drawing.Point(266, 591)
        Me.chkOption5145.Name = "chkOption5145"
        Me.chkOption5145.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption5145.Size = New System.Drawing.Size(238, 25)
        Me.chkOption5145.TabIndex = 40
        Me.chkOption5145.Tag = "5145"
        Me.chkOption5145.Text = "Archive Document with Timestamp:"
        Me.chkOption5145.UseVisualStyleBackColor = False
        '
        'chkOption5177
        '
        Me.chkOption5177.AutoSize = True
        Me.chkOption5177.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.chkOption5177.Location = New System.Drawing.Point(463, 62)
        Me.chkOption5177.Name = "chkOption5177"
        Me.chkOption5177.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.chkOption5177.Size = New System.Drawing.Size(147, 17)
        Me.chkOption5177.TabIndex = 43
        Me.chkOption5177.Tag = "5177"
        Me.chkOption5177.Text = " Is Sharepoint Online"
        Me.chkOption5177.UseVisualStyleBackColor = True
        '
        'lblSharepointUserName
        '
        Me.lblSharepointUserName.BackColor = System.Drawing.SystemColors.Control
        Me.lblSharepointUserName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSharepointUserName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSharepointUserName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSharepointUserName.Location = New System.Drawing.Point(12, 115)
        Me.lblSharepointUserName.Name = "lblSharepointUserName"
        Me.lblSharepointUserName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSharepointUserName.Size = New System.Drawing.Size(214, 18)
        Me.lblSharepointUserName.TabIndex = 44
        Me.lblSharepointUserName.Tag = ""
        Me.lblSharepointUserName.Text = "Sharepoint Online Certificate Path:"
        '
        'txtOption5178
        '
        Me.txtOption5178.AcceptsReturn = True
        Me.txtOption5178.AccessibleDescription = "Sharepoint Online User Name:"
        Me.txtOption5178.BackColor = System.Drawing.SystemColors.Window
        Me.txtOption5178.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOption5178.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOption5178.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOption5178.Location = New System.Drawing.Point(220, 112)
        Me.txtOption5178.MaxLength = 0
        Me.txtOption5178.Name = "txtOption5178"
        Me.txtOption5178.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOption5178.Size = New System.Drawing.Size(123, 21)
        Me.txtOption5178.TabIndex = 29
        Me.txtOption5178.Tag = "5178,ValidateRequired"
        '
        'lblSharepointPassword
        '
        Me.lblSharepointPassword.BackColor = System.Drawing.SystemColors.Control
        Me.lblSharepointPassword.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSharepointPassword.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSharepointPassword.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSharepointPassword.Location = New System.Drawing.Point(354, 114)
        Me.lblSharepointPassword.Name = "lblSharepointPassword"
        Me.lblSharepointPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSharepointPassword.Size = New System.Drawing.Size(138, 18)
        Me.lblSharepointPassword.TabIndex = 45
        Me.lblSharepointPassword.Tag = ""
        Me.lblSharepointPassword.Text = "Certificate Password:"
        '
        'txtOption5179
        '
        Me.txtOption5179.AcceptsReturn = True
        Me.txtOption5179.AccessibleDescription = "Sharepoint Online Password:"
        Me.txtOption5179.BackColor = System.Drawing.SystemColors.Window
        Me.txtOption5179.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOption5179.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOption5179.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOption5179.Location = New System.Drawing.Point(482, 111)
        Me.txtOption5179.MaxLength = 20
        Me.txtOption5179.Name = "txtOption5179"
        Me.txtOption5179.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtOption5179.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOption5179.Size = New System.Drawing.Size(102, 21)
        Me.txtOption5179.TabIndex = 30
        Me.txtOption5179.Tag = "5179,ValidateRequired"
        '
        'txtUserPassword
        '
        Me.txtUserPassword.AccessibleDescription = "SMTP User Password:"
        Me.txtUserPassword.Location = New System.Drawing.Point(234, 542)
        Me.txtUserPassword.Name = "txtUserPassword"
        Me.txtUserPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtUserPassword.Size = New System.Drawing.Size(152, 20)
        Me.txtUserPassword.TabIndex = 32
        Me.txtUserPassword.Tag = "5183"
        '
        'lblUserPassword
        '
        Me.lblUserPassword.AutoSize = True
        Me.lblUserPassword.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserPassword.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblUserPassword.Location = New System.Drawing.Point(21, 542)
        Me.lblUserPassword.Name = "lblUserPassword"
        Me.lblUserPassword.Size = New System.Drawing.Size(96, 13)
        Me.lblUserPassword.TabIndex = 33
        Me.lblUserPassword.Tag = "5183"
        Me.lblUserPassword.Text = "User Password:"
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox1.Location = New System.Drawing.Point(395, 518)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(95, 17)
        Me.CheckBox1.TabIndex = 35
        Me.CheckBox1.Tag = "5184"
        Me.CheckBox1.Text = "Enable SSL:"
        Me.CheckBox1.UseVisualStyleBackColor = False
        '
        'chkOption5209
        '
        Me.chkOption5209.AutoSize = True
        Me.chkOption5209.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5209.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption5209.Location = New System.Drawing.Point(23, 638)
        Me.chkOption5209.Name = "chkOption5209"
        Me.chkOption5209.Size = New System.Drawing.Size(225, 17)
        Me.chkOption5209.TabIndex = 46
        Me.chkOption5209.Tag = "5209"
        Me.chkOption5209.Text = "User's Email Address as Sender:   "
        Me.chkOption5209.UseVisualStyleBackColor = False
        '
        'txtSMTPUserName
        '
        Me.txtSMTPUserName.AcceptsReturn = True
        Me.txtSMTPUserName.AccessibleDescription = "SMTP User Name:"
        Me.txtSMTPUserName.BackColor = System.Drawing.SystemColors.Window
        Me.txtSMTPUserName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSMTPUserName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSMTPUserName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSMTPUserName.Location = New System.Drawing.Point(233, 518)
        Me.txtSMTPUserName.MaxLength = 0
        Me.txtSMTPUserName.Name = "txtSMTPUserName"
        Me.txtSMTPUserName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSMTPUserName.Size = New System.Drawing.Size(153, 21)
        Me.txtSMTPUserName.TabIndex = 47
        Me.txtSMTPUserName.Tag = "5244"
        '
        'lblSMTPUserName
        '
        Me.lblSMTPUserName.BackColor = System.Drawing.SystemColors.Control
        Me.lblSMTPUserName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSMTPUserName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSMTPUserName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSMTPUserName.Location = New System.Drawing.Point(21, 516)
        Me.lblSMTPUserName.Name = "lblSMTPUserName"
        Me.lblSMTPUserName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSMTPUserName.Size = New System.Drawing.Size(158, 16)
        Me.lblSMTPUserName.TabIndex = 48
        Me.lblSMTPUserName.Tag = "5244"
        Me.lblSMTPUserName.Text = "SMTP User Name:"
        '
        'chkOption5257
        '
        Me.chkOption5257.AutoSize = True
        Me.chkOption5257.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5257.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption5257.Location = New System.Drawing.Point(313, 638)
        Me.chkOption5257.Name = "chkOption5257"
        Me.chkOption5257.Size = New System.Drawing.Size(295, 17)
        Me.chkOption5257.TabIndex = 47
        Me.chkOption5257.Tag = "5257"
        Me.chkOption5257.Text = "Archive Email with Email Subject as File Name:"
        Me.chkOption5257.UseVisualStyleBackColor = False
        '
        'lblAppClientId
        '
        Me.lblAppClientId.AutoSize = True
        Me.lblAppClientId.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAppClientId.Location = New System.Drawing.Point(14, 142)
        Me.lblAppClientId.Name = "lblAppClientId"
        Me.lblAppClientId.Size = New System.Drawing.Size(195, 13)
        Me.lblAppClientId.TabIndex = 49
        Me.lblAppClientId.Text = "Sharepoint Online App Client ID:"
        '
        'txtOption5258
        '
        Me.txtOption5258.AccessibleDescription = "Sharepoint Online App Client ID:"
        Me.txtOption5258.Location = New System.Drawing.Point(220, 138)
        Me.txtOption5258.Name = "txtOption5258"
        Me.txtOption5258.Size = New System.Drawing.Size(123, 20)
        Me.txtOption5258.TabIndex = 50
        Me.txtOption5258.Tag = "5258,ValidateRequired"
        '
        'txtOption5259
        '
        Me.txtOption5259.AccessibleDescription = "Tenant ID:"
        Me.txtOption5259.Location = New System.Drawing.Point(440, 139)
        Me.txtOption5259.Name = "txtOption5259"
        Me.txtOption5259.Size = New System.Drawing.Size(144, 20)
        Me.txtOption5259.TabIndex = 52
        Me.txtOption5259.Tag = "5259,ValidateRequired"
        '
        'lblTenantId
        '
        Me.lblTenantId.AutoSize = True
        Me.lblTenantId.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTenantId.Location = New System.Drawing.Point(354, 141)
        Me.lblTenantId.Name = "lblTenantId"
        Me.lblTenantId.Size = New System.Drawing.Size(68, 13)
        Me.lblTenantId.TabIndex = 51
        Me.lblTenantId.Text = "Tenant ID:"
        '
        'frmInstallation
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(665, 636)
        Me.Controls.Add(Me.txtOption5259)
        Me.Controls.Add(Me.lblTenantId)
        Me.Controls.Add(Me.txtOption5258)
        Me.Controls.Add(Me.lblAppClientId)
        Me.Controls.Add(Me.txtSMTPUserName)
        Me.Controls.Add(Me.lblSMTPUserName)
        Me.Controls.Add(Me.chkOption5209)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.lblUserPassword)
        Me.Controls.Add(Me.txtUserPassword)
        Me.Controls.Add(Me.txtOption5179)
        Me.Controls.Add(Me.lblSharepointPassword)
        Me.Controls.Add(Me.txtOption5178)
        Me.Controls.Add(Me.lblSharepointUserName)
        Me.Controls.Add(Me.chkOption5177)
        Me.Controls.Add(Me.cboOption5146)
        Me.Controls.Add(Me.grpOptionDocProduction)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.chkOption5145)
        Me.Controls.Add(Me.chkOption5097)
        Me.Controls.Add(Me.txt5086)
        Me.Controls.Add(Me.lbl5086)
        Me.Controls.Add(Me.txt5085)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtSMTPEmailBCCAddress)
        Me.Controls.Add(Me.lblSMTPEmailBCCAddress)
        Me.Controls.Add(Me.cboOption10)
        Me.Controls.Add(Me.lblOption10)
        Me.Controls.Add(Me.cboOption5068)
        Me.Controls.Add(Me.chkOption5010)
        Me.Controls.Add(Me.ChkOption5023)
        Me.Controls.Add(Me.txtSMTPEmailReturnAddress)
        Me.Controls.Add(Me.txtSMTPEmailServerPort)
        Me.Controls.Add(Me.txtSMTPEmailServer)
        Me.Controls.Add(Me.chkOption5009)
        Me.Controls.Add(Me.chkOption5008)
        Me.Controls.Add(Me._lblFailedemailWorkManagerTask_5068)
        Me.Controls.Add(Me.lblSMTPEmailReturnAddress)
        Me.Controls.Add(Me.lblSMTPEmailServerPort)
        Me.Controls.Add(Me.lblSMTPEmailServer)
        Me.Controls.Add(Me.chkOption5257)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmInstallation"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Form2"
        Me.grpOptionDocProduction.ResumeLayout(False)
        Me.grpOptionDocProduction.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializelblFailedemailWorkManagerTask()
        Me.lblFailedemailWorkManagerTask(5068) = _lblFailedemailWorkManagerTask_5068
    End Sub
    Public WithEvents cboOption10 As System.Windows.Forms.ComboBox
    Private WithEvents lblOption10 As System.Windows.Forms.Label
    Friend WithEvents chkOption5097 As System.Windows.Forms.CheckBox
    Public WithEvents txtSMTPEmailBCCAddress As System.Windows.Forms.TextBox
    Public WithEvents lblSMTPEmailBCCAddress As System.Windows.Forms.Label
    Public WithEvents txt5085 As System.Windows.Forms.TextBox
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents txt5086 As System.Windows.Forms.TextBox
    Public WithEvents lbl5086 As System.Windows.Forms.Label
    Friend WithEvents lblDocProduction As System.Windows.Forms.Label
    Friend WithEvents lblCCMWebURL As System.Windows.Forms.Label
    Friend WithEvents lblPartner As System.Windows.Forms.Label
    Friend WithEvents lblCustomer As System.Windows.Forms.Label
    Friend WithEvents lblConrtactTypeName As System.Windows.Forms.Label
    Public WithEvents cmbOption5163 As System.Windows.Forms.ComboBox
    Public WithEvents txtOption5164 As System.Windows.Forms.TextBox
    Public WithEvents txtOption5165 As System.Windows.Forms.TextBox
    Public WithEvents txtOption5166 As System.Windows.Forms.TextBox
    Public WithEvents txtOption5167 As System.Windows.Forms.TextBox
    Friend WithEvents lblRepositoryProj As System.Windows.Forms.Label
    Public WithEvents txtOption5168 As System.Windows.Forms.TextBox
    Friend WithEvents lblContractTypeVersion As System.Windows.Forms.Label
    Public WithEvents txtOption5169 As System.Windows.Forms.TextBox
    Friend WithEvents grpOptionDocProduction As System.Windows.Forms.GroupBox
    Friend WithEvents chkOption5170 As System.Windows.Forms.CheckBox
    Friend WithEvents lblCCMStatus As System.Windows.Forms.Label
    Public WithEvents cmbOption5171 As System.Windows.Forms.ComboBox
    Friend WithEvents btnCCMTemplateSync As System.Windows.Forms.Button
    Public WithEvents txtOption5173 As System.Windows.Forms.TextBox
    Friend WithEvents lblOption5173 As System.Windows.Forms.Label
    Public WithEvents cboOption5146 As ComboBox
    Private WithEvents Label2 As Label
    Public WithEvents chkOption5145 As CheckBox
    Friend WithEvents chkOption5177 As System.Windows.Forms.CheckBox
    Public WithEvents lblSharepointUserName As System.Windows.Forms.Label
    Public WithEvents txtOption5178 As System.Windows.Forms.TextBox
    Public WithEvents lblSharepointPassword As System.Windows.Forms.Label
    Public WithEvents txtOption5179 As System.Windows.Forms.TextBox
    Friend WithEvents chkOption5181 As CheckBox
    Friend WithEvents txtUserPassword As System.Windows.Forms.TextBox
    Friend WithEvents lblUserPassword As System.Windows.Forms.Label
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents chkOption5207 As System.Windows.Forms.CheckBox

    Friend WithEvents chkOption5209 As System.Windows.Forms.CheckBox
    Public WithEvents txtSMTPUserName As TextBox
    Public WithEvents lblSMTPUserName As Label
    Friend WithEvents chkOption5257 As CheckBox
    Friend WithEvents lblAppClientId As Label
    Friend WithEvents txtOption5258 As TextBox
    Friend WithEvents txtOption5259 As TextBox
    Friend WithEvents lblTenantId As Label
#End Region
End Class
