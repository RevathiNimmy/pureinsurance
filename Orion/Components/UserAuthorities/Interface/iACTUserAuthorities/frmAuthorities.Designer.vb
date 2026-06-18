<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAuthorities
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents chkHasRefundAuthority As System.Windows.Forms.CheckBox
	Public WithEvents chkHasTransferAuthority As System.Windows.Forms.CheckBox
	Public WithEvents fmeAuthority As System.Windows.Forms.GroupBox
	Public WithEvents txtFeeDiscount As System.Windows.Forms.TextBox
	Public WithEvents lblFeeDiscount As System.Windows.Forms.Label
	Public WithEvents fmeFees As System.Windows.Forms.GroupBox
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents chkUnrestrictedEnquiry As System.Windows.Forms.CheckBox
	Public WithEvents chkUnrestrictedUpdate As System.Windows.Forms.CheckBox
	Public WithEvents fmeAccess As System.Windows.Forms.GroupBox
	Public WithEvents chkTransWriteOffs As System.Windows.Forms.CheckBox
	Public WithEvents txtTransWriteOff As System.Windows.Forms.TextBox
	Public WithEvents chkWriteOffs As System.Windows.Forms.CheckBox
	Public WithEvents txtWriteOff As System.Windows.Forms.TextBox
	Public WithEvents cboTransWriteOffsCurrency As UserControls.CurrencyLookup
	Public WithEvents cboWriteOffsCurrency As UserControls.CurrencyLookup
	Public WithEvents lblWriteOffsCurrency As System.Windows.Forms.Label
	Public WithEvents lblTransWriteOffsCurrency As System.Windows.Forms.Label
	Public WithEvents lblTransAmount As System.Windows.Forms.Label
	Public WithEvents lblAmount As System.Windows.Forms.Label
	Public WithEvents fmeWriteOffs As System.Windows.Forms.GroupBox
	Public WithEvents chkClaimPayments As System.Windows.Forms.CheckBox
	Public WithEvents txtClaimPayments As System.Windows.Forms.TextBox
	Public WithEvents cboClaimPaymentsCurrency As UserControls.CurrencyLookup
	Public WithEvents lblClaimPaymentsCurrency As System.Windows.Forms.Label
	Public WithEvents lblClaimPayments As System.Windows.Forms.Label
	Public WithEvents fmeClaimPayments As System.Windows.Forms.GroupBox
	Public WithEvents cboPaymentsCurrency As UserControls.CurrencyLookup
	Public WithEvents chkPayments As System.Windows.Forms.CheckBox
	Public WithEvents txtPayments As System.Windows.Forms.TextBox
	Public WithEvents lblPaymentsCurrency As System.Windows.Forms.Label
	Public WithEvents lblPayments As System.Windows.Forms.Label
	Public WithEvents fmePayments As System.Windows.Forms.GroupBox
	Public WithEvents chkOverrideRate As System.Windows.Forms.CheckBox
	Public WithEvents chkOverrideDate As System.Windows.Forms.CheckBox
	Public WithEvents chkOverridePrePolicyRate As System.Windows.Forms.CheckBox
	Public WithEvents chkOverridePrePolicyDate As System.Windows.Forms.CheckBox
	Public WithEvents fmeOverride As System.Windows.Forms.GroupBox
	Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMain As System.Windows.Forms.TabControl
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAuthorities))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.fmeAuthority = New System.Windows.Forms.GroupBox
        Me.chkHasRefundAuthority = New System.Windows.Forms.CheckBox
        Me.chkHasTransferAuthority = New System.Windows.Forms.CheckBox
        Me.fmeFees = New System.Windows.Forms.GroupBox
        Me.txtFeeDiscount = New System.Windows.Forms.TextBox
        Me.lblFeeDiscount = New System.Windows.Forms.Label
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.tabMain = New System.Windows.Forms.TabControl
        Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage
        Me.fmeAccess = New System.Windows.Forms.GroupBox
        Me.chkUnrestrictedEnquiry = New System.Windows.Forms.CheckBox
        Me.chkUnrestrictedUpdate = New System.Windows.Forms.CheckBox
        Me.fmeWriteOffs = New System.Windows.Forms.GroupBox
        Me.chkTransWriteOffs = New System.Windows.Forms.CheckBox
        Me.txtTransWriteOff = New System.Windows.Forms.TextBox
        Me.chkWriteOffs = New System.Windows.Forms.CheckBox
        Me.txtWriteOff = New System.Windows.Forms.TextBox
        Me.cboTransWriteOffsCurrency = New UserControls.CurrencyLookup
        Me.cboWriteOffsCurrency = New UserControls.CurrencyLookup
        Me.lblWriteOffsCurrency = New System.Windows.Forms.Label
        Me.lblTransWriteOffsCurrency = New System.Windows.Forms.Label
        Me.lblTransAmount = New System.Windows.Forms.Label
        Me.lblAmount = New System.Windows.Forms.Label
        Me.fmeClaimPayments = New System.Windows.Forms.GroupBox
        Me.chkClaimPayments = New System.Windows.Forms.CheckBox
        Me.txtClaimPayments = New System.Windows.Forms.TextBox
        Me.cboClaimPaymentsCurrency = New UserControls.CurrencyLookup
        Me.lblClaimPaymentsCurrency = New System.Windows.Forms.Label
        Me.lblClaimPayments = New System.Windows.Forms.Label
        Me.fmePayments = New System.Windows.Forms.GroupBox
        Me.cboPaymentsCurrency = New UserControls.CurrencyLookup
        Me.chkPayments = New System.Windows.Forms.CheckBox
        Me.txtPayments = New System.Windows.Forms.TextBox
        Me.lblPaymentsCurrency = New System.Windows.Forms.Label
        Me.lblPayments = New System.Windows.Forms.Label
        Me.fmeOverride = New System.Windows.Forms.GroupBox
        Me.chkOverrideRate = New System.Windows.Forms.CheckBox
        Me.chkOverrideDate = New System.Windows.Forms.CheckBox
        Me.chkOverridePrePolicyRate = New System.Windows.Forms.CheckBox
        Me.chkOverridePrePolicyDate = New System.Windows.Forms.CheckBox
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.fmeAuthority.SuspendLayout()
        Me.fmeFees.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me._tabMain_TabPage0.SuspendLayout()
        Me.fmeAccess.SuspendLayout()
        Me.fmeWriteOffs.SuspendLayout()
        Me.fmeClaimPayments.SuspendLayout()
        Me.fmePayments.SuspendLayout()
        Me.fmeOverride.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'fmeAuthority
        '
        Me.fmeAuthority.BackColor = System.Drawing.SystemColors.Control
        Me.fmeAuthority.Controls.Add(Me.chkHasRefundAuthority)
        Me.fmeAuthority.Controls.Add(Me.chkHasTransferAuthority)
        Me.fmeAuthority.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fmeAuthority.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fmeAuthority.Location = New System.Drawing.Point(576, 32)
        Me.fmeAuthority.Name = "fmeAuthority"
        Me.fmeAuthority.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fmeAuthority.Size = New System.Drawing.Size(269, 76)
        Me.fmeAuthority.TabIndex = 33
        Me.fmeAuthority.TabStop = False
        Me.fmeAuthority.Text = "Authority"
        Me.fmeAuthority.Visible = False
        '
        'chkHasRefundAuthority
        '
        Me.chkHasRefundAuthority.BackColor = System.Drawing.SystemColors.Control
        Me.chkHasRefundAuthority.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkHasRefundAuthority.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkHasRefundAuthority.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkHasRefundAuthority.Location = New System.Drawing.Point(16, 21)
        Me.chkHasRefundAuthority.Name = "chkHasRefundAuthority"
        Me.chkHasRefundAuthority.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkHasRefundAuthority.Size = New System.Drawing.Size(243, 17)
        Me.chkHasRefundAuthority.TabIndex = 35
        Me.chkHasRefundAuthority.Text = "User has refund aut&hority"
        Me.chkHasRefundAuthority.UseVisualStyleBackColor = False
        Me.chkHasRefundAuthority.Visible = False
        '
        'chkHasTransferAuthority
        '
        Me.chkHasTransferAuthority.BackColor = System.Drawing.SystemColors.Control
        Me.chkHasTransferAuthority.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkHasTransferAuthority.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkHasTransferAuthority.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkHasTransferAuthority.Location = New System.Drawing.Point(16, 46)
        Me.chkHasTransferAuthority.Name = "chkHasTransferAuthority"
        Me.chkHasTransferAuthority.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkHasTransferAuthority.Size = New System.Drawing.Size(243, 17)
        Me.chkHasTransferAuthority.TabIndex = 34
        Me.chkHasTransferAuthority.Text = "User has &transfer authority"
        Me.chkHasTransferAuthority.UseVisualStyleBackColor = False
        Me.chkHasTransferAuthority.Visible = False
        '
        'fmeFees
        '
        Me.fmeFees.BackColor = System.Drawing.SystemColors.Control
        Me.fmeFees.Controls.Add(Me.txtFeeDiscount)
        Me.fmeFees.Controls.Add(Me.lblFeeDiscount)
        Me.fmeFees.Enabled = False
        Me.fmeFees.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fmeFees.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fmeFees.Location = New System.Drawing.Point(578, 114)
        Me.fmeFees.Name = "fmeFees"
        Me.fmeFees.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fmeFees.Size = New System.Drawing.Size(215, 67)
        Me.fmeFees.TabIndex = 18
        Me.fmeFees.TabStop = False
        Me.fmeFees.Text = "Fees"
        Me.fmeFees.Visible = False
        '
        'txtFeeDiscount
        '
        Me.txtFeeDiscount.AcceptsReturn = True
        Me.txtFeeDiscount.BackColor = System.Drawing.SystemColors.Window
        Me.txtFeeDiscount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFeeDiscount.Enabled = False
        Me.txtFeeDiscount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFeeDiscount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFeeDiscount.Location = New System.Drawing.Point(96, 27)
        Me.txtFeeDiscount.MaxLength = 0
        Me.txtFeeDiscount.Name = "txtFeeDiscount"
        Me.txtFeeDiscount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFeeDiscount.Size = New System.Drawing.Size(104, 21)
        Me.txtFeeDiscount.TabIndex = 19
        Me.txtFeeDiscount.Visible = False
        '
        'lblFeeDiscount
        '
        Me.lblFeeDiscount.AutoSize = True
        Me.lblFeeDiscount.BackColor = System.Drawing.SystemColors.Control
        Me.lblFeeDiscount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFeeDiscount.Enabled = False
        Me.lblFeeDiscount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFeeDiscount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFeeDiscount.Location = New System.Drawing.Point(12, 30)
        Me.lblFeeDiscount.Name = "lblFeeDiscount"
        Me.lblFeeDiscount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFeeDiscount.Size = New System.Drawing.Size(71, 13)
        Me.lblFeeDiscount.TabIndex = 20
        Me.lblFeeDiscount.Text = "Fee &discount:"
        Me.lblFeeDiscount.Visible = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(344, 367)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 10
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
        Me.cmdCancel.Location = New System.Drawing.Point(424, 367)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 11
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
        Me.cmdHelp.Location = New System.Drawing.Point(504, 367)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 12
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me._tabMain_TabPage0)
        Me.tabMain.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMain.ItemSize = New System.Drawing.Size(188, 18)
        Me.tabMain.Location = New System.Drawing.Point(8, 10)
        Me.tabMain.Multiline = True
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(573, 357)
        Me.tabMain.TabIndex = 13
        '
        '_tabMain_TabPage0
        '
        Me._tabMain_TabPage0.Controls.Add(Me.fmeAccess)
        Me._tabMain_TabPage0.Controls.Add(Me.fmeWriteOffs)
        Me._tabMain_TabPage0.Controls.Add(Me.fmeClaimPayments)
        Me._tabMain_TabPage0.Controls.Add(Me.fmePayments)
        Me._tabMain_TabPage0.Controls.Add(Me.fmeOverride)
        Me._tabMain_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage0.Name = "_tabMain_TabPage0"
        Me._tabMain_TabPage0.Size = New System.Drawing.Size(565, 331)
        Me._tabMain_TabPage0.TabIndex = 0
        Me._tabMain_TabPage0.Text = "&1 - Authority"
        '
        'fmeAccess
        '
        Me.fmeAccess.BackColor = System.Drawing.SystemColors.Control
        Me.fmeAccess.Controls.Add(Me.chkUnrestrictedEnquiry)
        Me.fmeAccess.Controls.Add(Me.chkUnrestrictedUpdate)
        Me.fmeAccess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fmeAccess.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fmeAccess.Location = New System.Drawing.Point(10, 10)
        Me.fmeAccess.Name = "fmeAccess"
        Me.fmeAccess.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fmeAccess.Size = New System.Drawing.Size(269, 71)
        Me.fmeAccess.TabIndex = 14
        Me.fmeAccess.TabStop = False
        Me.fmeAccess.Text = "Account Access"
        '
        'chkUnrestrictedEnquiry
        '
        Me.chkUnrestrictedEnquiry.BackColor = System.Drawing.SystemColors.Control
        Me.chkUnrestrictedEnquiry.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkUnrestrictedEnquiry.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUnrestrictedEnquiry.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkUnrestrictedEnquiry.Location = New System.Drawing.Point(16, 19)
        Me.chkUnrestrictedEnquiry.Name = "chkUnrestrictedEnquiry"
        Me.chkUnrestrictedEnquiry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkUnrestrictedEnquiry.Size = New System.Drawing.Size(243, 17)
        Me.chkUnrestrictedEnquiry.TabIndex = 0
        Me.chkUnrestrictedEnquiry.Text = "U&ser has unrestricted enquiry access"
        Me.chkUnrestrictedEnquiry.UseVisualStyleBackColor = False
        '
        'chkUnrestrictedUpdate
        '
        Me.chkUnrestrictedUpdate.BackColor = System.Drawing.SystemColors.Control
        Me.chkUnrestrictedUpdate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkUnrestrictedUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUnrestrictedUpdate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkUnrestrictedUpdate.Location = New System.Drawing.Point(16, 44)
        Me.chkUnrestrictedUpdate.Name = "chkUnrestrictedUpdate"
        Me.chkUnrestrictedUpdate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkUnrestrictedUpdate.Size = New System.Drawing.Size(243, 17)
        Me.chkUnrestrictedUpdate.TabIndex = 1
        Me.chkUnrestrictedUpdate.Text = "Us&er has unrestricted update access"
        Me.chkUnrestrictedUpdate.UseVisualStyleBackColor = False
        '
        'fmeWriteOffs
        '
        Me.fmeWriteOffs.BackColor = System.Drawing.SystemColors.Control
        Me.fmeWriteOffs.Controls.Add(Me.chkTransWriteOffs)
        Me.fmeWriteOffs.Controls.Add(Me.txtTransWriteOff)
        Me.fmeWriteOffs.Controls.Add(Me.chkWriteOffs)
        Me.fmeWriteOffs.Controls.Add(Me.txtWriteOff)
        Me.fmeWriteOffs.Controls.Add(Me.cboTransWriteOffsCurrency)
        Me.fmeWriteOffs.Controls.Add(Me.cboWriteOffsCurrency)
        Me.fmeWriteOffs.Controls.Add(Me.lblWriteOffsCurrency)
        Me.fmeWriteOffs.Controls.Add(Me.lblTransWriteOffsCurrency)
        Me.fmeWriteOffs.Controls.Add(Me.lblTransAmount)
        Me.fmeWriteOffs.Controls.Add(Me.lblAmount)
        Me.fmeWriteOffs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fmeWriteOffs.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fmeWriteOffs.Location = New System.Drawing.Point(288, 10)
        Me.fmeWriteOffs.Name = "fmeWriteOffs"
        Me.fmeWriteOffs.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fmeWriteOffs.Size = New System.Drawing.Size(269, 199)
        Me.fmeWriteOffs.TabIndex = 15
        Me.fmeWriteOffs.TabStop = False
        Me.fmeWriteOffs.Text = "Write-offs"
        '
        'chkTransWriteOffs
        '
        Me.chkTransWriteOffs.BackColor = System.Drawing.SystemColors.Control
        Me.chkTransWriteOffs.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkTransWriteOffs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTransWriteOffs.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkTransWriteOffs.Location = New System.Drawing.Point(16, 104)
        Me.chkTransWriteOffs.Name = "chkTransWriteOffs"
        Me.chkTransWriteOffs.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkTransWriteOffs.Size = New System.Drawing.Size(243, 17)
        Me.chkTransWriteOffs.TabIndex = 4
        Me.chkTransWriteOffs.Text = "User c&an perform debt write-offs"
        Me.chkTransWriteOffs.UseVisualStyleBackColor = False
        '
        'txtTransWriteOff
        '
        Me.txtTransWriteOff.AcceptsReturn = True
        Me.txtTransWriteOff.BackColor = System.Drawing.SystemColors.Window
        Me.txtTransWriteOff.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTransWriteOff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTransWriteOff.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTransWriteOff.Location = New System.Drawing.Point(96, 152)
        Me.txtTransWriteOff.MaxLength = 0
        Me.txtTransWriteOff.Name = "txtTransWriteOff"
        Me.txtTransWriteOff.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTransWriteOff.Size = New System.Drawing.Size(157, 21)
        Me.txtTransWriteOff.TabIndex = 5
        '
        'chkWriteOffs
        '
        Me.chkWriteOffs.BackColor = System.Drawing.SystemColors.Control
        Me.chkWriteOffs.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkWriteOffs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkWriteOffs.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkWriteOffs.Location = New System.Drawing.Point(16, 21)
        Me.chkWriteOffs.Name = "chkWriteOffs"
        Me.chkWriteOffs.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkWriteOffs.Size = New System.Drawing.Size(243, 17)
        Me.chkWriteOffs.TabIndex = 2
        Me.chkWriteOffs.Text = "Use&r can perform allocation write-offs"
        Me.chkWriteOffs.UseVisualStyleBackColor = False
        '
        'txtWriteOff
        '
        Me.txtWriteOff.AcceptsReturn = True
        Me.txtWriteOff.BackColor = System.Drawing.SystemColors.Window
        Me.txtWriteOff.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtWriteOff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWriteOff.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtWriteOff.Location = New System.Drawing.Point(96, 72)
        Me.txtWriteOff.MaxLength = 0
        Me.txtWriteOff.Name = "txtWriteOff"
        Me.txtWriteOff.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtWriteOff.Size = New System.Drawing.Size(157, 21)
        Me.txtWriteOff.TabIndex = 3
        '
        'cboTransWriteOffsCurrency
        '
        Me.cboTransWriteOffsCurrency.CompanyId = 0
        Me.cboTransWriteOffsCurrency.CurrencyId = 0
        Me.cboTransWriteOffsCurrency.DefaultCurrencyId = 0
        Me.cboTransWriteOffsCurrency.FirstItem = ""
        Me.cboTransWriteOffsCurrency.ListIndex = -1
        Me.cboTransWriteOffsCurrency.Location = New System.Drawing.Point(96, 128)
        Me.cboTransWriteOffsCurrency.Name = "cboTransWriteOffsCurrency"
        Me.cboTransWriteOffsCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actBaseCurrencies
        Me.cboTransWriteOffsCurrency.Size = New System.Drawing.Size(161, 21)
        Me.cboTransWriteOffsCurrency.TabIndex = 29
        Me.cboTransWriteOffsCurrency.ToolTipText = ""
        Me.cboTransWriteOffsCurrency.WhatsThisHelpID = 0
        '
        'cboWriteOffsCurrency
        '
        Me.cboWriteOffsCurrency.CompanyId = 0
        Me.cboWriteOffsCurrency.CurrencyId = 0
        Me.cboWriteOffsCurrency.DefaultCurrencyId = 0
        Me.cboWriteOffsCurrency.FirstItem = ""
        Me.cboWriteOffsCurrency.ListIndex = -1
        Me.cboWriteOffsCurrency.Location = New System.Drawing.Point(96, 48)
        Me.cboWriteOffsCurrency.Name = "cboWriteOffsCurrency"
        Me.cboWriteOffsCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actBaseCurrencies
        Me.cboWriteOffsCurrency.Size = New System.Drawing.Size(161, 21)
        Me.cboWriteOffsCurrency.TabIndex = 31
        Me.cboWriteOffsCurrency.ToolTipText = ""
        Me.cboWriteOffsCurrency.WhatsThisHelpID = 0
        '
        'lblWriteOffsCurrency
        '
        Me.lblWriteOffsCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblWriteOffsCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblWriteOffsCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWriteOffsCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblWriteOffsCurrency.Location = New System.Drawing.Point(16, 50)
        Me.lblWriteOffsCurrency.Name = "lblWriteOffsCurrency"
        Me.lblWriteOffsCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblWriteOffsCurrency.Size = New System.Drawing.Size(73, 17)
        Me.lblWriteOffsCurrency.TabIndex = 32
        Me.lblWriteOffsCurrency.Text = "Currency:"
        '
        'lblTransWriteOffsCurrency
        '
        Me.lblTransWriteOffsCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblTransWriteOffsCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTransWriteOffsCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTransWriteOffsCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTransWriteOffsCurrency.Location = New System.Drawing.Point(16, 130)
        Me.lblTransWriteOffsCurrency.Name = "lblTransWriteOffsCurrency"
        Me.lblTransWriteOffsCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTransWriteOffsCurrency.Size = New System.Drawing.Size(73, 17)
        Me.lblTransWriteOffsCurrency.TabIndex = 30
        Me.lblTransWriteOffsCurrency.Text = "Currency:"
        '
        'lblTransAmount
        '
        Me.lblTransAmount.AutoSize = True
        Me.lblTransAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblTransAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTransAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTransAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTransAmount.Location = New System.Drawing.Point(16, 154)
        Me.lblTransAmount.Name = "lblTransAmount"
        Me.lblTransAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTransAmount.Size = New System.Drawing.Size(46, 13)
        Me.lblTransAmount.TabIndex = 17
        Me.lblTransAmount.Text = "Amo&unt:"
        '
        'lblAmount
        '
        Me.lblAmount.AutoSize = True
        Me.lblAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAmount.Location = New System.Drawing.Point(16, 74)
        Me.lblAmount.Name = "lblAmount"
        Me.lblAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAmount.Size = New System.Drawing.Size(46, 13)
        Me.lblAmount.TabIndex = 16
        Me.lblAmount.Text = "A&mount:"
        '
        'fmeClaimPayments
        '
        Me.fmeClaimPayments.BackColor = System.Drawing.SystemColors.Control
        Me.fmeClaimPayments.Controls.Add(Me.chkClaimPayments)
        Me.fmeClaimPayments.Controls.Add(Me.txtClaimPayments)
        Me.fmeClaimPayments.Controls.Add(Me.cboClaimPaymentsCurrency)
        Me.fmeClaimPayments.Controls.Add(Me.lblClaimPaymentsCurrency)
        Me.fmeClaimPayments.Controls.Add(Me.lblClaimPayments)
        Me.fmeClaimPayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fmeClaimPayments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fmeClaimPayments.Location = New System.Drawing.Point(288, 214)
        Me.fmeClaimPayments.Name = "fmeClaimPayments"
        Me.fmeClaimPayments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fmeClaimPayments.Size = New System.Drawing.Size(269, 108)
        Me.fmeClaimPayments.TabIndex = 21
        Me.fmeClaimPayments.TabStop = False
        Me.fmeClaimPayments.Text = "Claim Payments"
        '
        'chkClaimPayments
        '
        Me.chkClaimPayments.BackColor = System.Drawing.SystemColors.Control
        Me.chkClaimPayments.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkClaimPayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkClaimPayments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkClaimPayments.Location = New System.Drawing.Point(16, 21)
        Me.chkClaimPayments.Name = "chkClaimPayments"
        Me.chkClaimPayments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkClaimPayments.Size = New System.Drawing.Size(240, 17)
        Me.chkClaimPayments.TabIndex = 8
        Me.chkClaimPayments.Text = "User has &Claim Payments authority"
        Me.chkClaimPayments.UseVisualStyleBackColor = False
        '
        'txtClaimPayments
        '
        Me.txtClaimPayments.AcceptsReturn = True
        Me.txtClaimPayments.BackColor = System.Drawing.SystemColors.Window
        Me.txtClaimPayments.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClaimPayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClaimPayments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClaimPayments.Location = New System.Drawing.Point(96, 72)
        Me.txtClaimPayments.MaxLength = 0
        Me.txtClaimPayments.Name = "txtClaimPayments"
        Me.txtClaimPayments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClaimPayments.Size = New System.Drawing.Size(157, 21)
        Me.txtClaimPayments.TabIndex = 9
        '
        'cboClaimPaymentsCurrency
        '
        Me.cboClaimPaymentsCurrency.CompanyId = 0
        Me.cboClaimPaymentsCurrency.CurrencyId = 0
        Me.cboClaimPaymentsCurrency.DefaultCurrencyId = 0
        Me.cboClaimPaymentsCurrency.FirstItem = ""
        Me.cboClaimPaymentsCurrency.ListIndex = -1
        Me.cboClaimPaymentsCurrency.Location = New System.Drawing.Point(96, 48)
        Me.cboClaimPaymentsCurrency.Name = "cboClaimPaymentsCurrency"
        Me.cboClaimPaymentsCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actBaseCurrencies
        Me.cboClaimPaymentsCurrency.Size = New System.Drawing.Size(161, 21)
        Me.cboClaimPaymentsCurrency.TabIndex = 27
        Me.cboClaimPaymentsCurrency.ToolTipText = ""
        Me.cboClaimPaymentsCurrency.WhatsThisHelpID = 0
        '
        'lblClaimPaymentsCurrency
        '
        Me.lblClaimPaymentsCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblClaimPaymentsCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClaimPaymentsCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClaimPaymentsCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClaimPaymentsCurrency.Location = New System.Drawing.Point(16, 50)
        Me.lblClaimPaymentsCurrency.Name = "lblClaimPaymentsCurrency"
        Me.lblClaimPaymentsCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClaimPaymentsCurrency.Size = New System.Drawing.Size(73, 17)
        Me.lblClaimPaymentsCurrency.TabIndex = 28
        Me.lblClaimPaymentsCurrency.Text = "Currency:"
        '
        'lblClaimPayments
        '
        Me.lblClaimPayments.AutoSize = True
        Me.lblClaimPayments.BackColor = System.Drawing.SystemColors.Control
        Me.lblClaimPayments.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClaimPayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClaimPayments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClaimPayments.Location = New System.Drawing.Point(16, 74)
        Me.lblClaimPayments.Name = "lblClaimPayments"
        Me.lblClaimPayments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClaimPayments.Size = New System.Drawing.Size(46, 13)
        Me.lblClaimPayments.TabIndex = 23
        Me.lblClaimPayments.Text = "Amount:"
        '
        'fmePayments
        '
        Me.fmePayments.BackColor = System.Drawing.SystemColors.Control
        Me.fmePayments.Controls.Add(Me.cboPaymentsCurrency)
        Me.fmePayments.Controls.Add(Me.chkPayments)
        Me.fmePayments.Controls.Add(Me.txtPayments)
        Me.fmePayments.Controls.Add(Me.lblPaymentsCurrency)
        Me.fmePayments.Controls.Add(Me.lblPayments)
        Me.fmePayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fmePayments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fmePayments.Location = New System.Drawing.Point(10, 214)
        Me.fmePayments.Name = "fmePayments"
        Me.fmePayments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fmePayments.Size = New System.Drawing.Size(269, 108)
        Me.fmePayments.TabIndex = 22
        Me.fmePayments.TabStop = False
        Me.fmePayments.Text = "Payments"
        '
        'cboPaymentsCurrency
        '
        Me.cboPaymentsCurrency.CompanyId = 0
        Me.cboPaymentsCurrency.CurrencyId = 0
        Me.cboPaymentsCurrency.DefaultCurrencyId = 0
        Me.cboPaymentsCurrency.FirstItem = ""
        Me.cboPaymentsCurrency.ListIndex = -1
        Me.cboPaymentsCurrency.Location = New System.Drawing.Point(96, 48)
        Me.cboPaymentsCurrency.Name = "cboPaymentsCurrency"
        Me.cboPaymentsCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actBaseCurrencies
        Me.cboPaymentsCurrency.Size = New System.Drawing.Size(161, 21)
        Me.cboPaymentsCurrency.TabIndex = 25
        Me.cboPaymentsCurrency.ToolTipText = ""
        Me.cboPaymentsCurrency.WhatsThisHelpID = 0
        '
        'chkPayments
        '
        Me.chkPayments.BackColor = System.Drawing.SystemColors.Control
        Me.chkPayments.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPayments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPayments.Location = New System.Drawing.Point(16, 21)
        Me.chkPayments.Name = "chkPayments"
        Me.chkPayments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPayments.Size = New System.Drawing.Size(243, 17)
        Me.chkPayments.TabIndex = 6
        Me.chkPayments.Text = "User has &Payments authority"
        Me.chkPayments.UseVisualStyleBackColor = False
        '
        'txtPayments
        '
        Me.txtPayments.AcceptsReturn = True
        Me.txtPayments.BackColor = System.Drawing.SystemColors.Window
        Me.txtPayments.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPayments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPayments.Location = New System.Drawing.Point(96, 72)
        Me.txtPayments.MaxLength = 0
        Me.txtPayments.Name = "txtPayments"
        Me.txtPayments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPayments.Size = New System.Drawing.Size(157, 21)
        Me.txtPayments.TabIndex = 7
        '
        'lblPaymentsCurrency
        '
        Me.lblPaymentsCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentsCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentsCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentsCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentsCurrency.Location = New System.Drawing.Point(16, 50)
        Me.lblPaymentsCurrency.Name = "lblPaymentsCurrency"
        Me.lblPaymentsCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentsCurrency.Size = New System.Drawing.Size(73, 17)
        Me.lblPaymentsCurrency.TabIndex = 26
        Me.lblPaymentsCurrency.Text = "Currency:"
        '
        'lblPayments
        '
        Me.lblPayments.AutoSize = True
        Me.lblPayments.BackColor = System.Drawing.SystemColors.Control
        Me.lblPayments.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPayments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPayments.Location = New System.Drawing.Point(16, 74)
        Me.lblPayments.Name = "lblPayments"
        Me.lblPayments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPayments.Size = New System.Drawing.Size(46, 13)
        Me.lblPayments.TabIndex = 24
        Me.lblPayments.Text = "Amount:"
        '
        'fmeOverride
        '
        Me.fmeOverride.BackColor = System.Drawing.SystemColors.Control
        Me.fmeOverride.Controls.Add(Me.chkOverrideRate)
        Me.fmeOverride.Controls.Add(Me.chkOverrideDate)
        Me.fmeOverride.Controls.Add(Me.chkOverridePrePolicyRate)
        Me.fmeOverride.Controls.Add(Me.chkOverridePrePolicyDate)
        Me.fmeOverride.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fmeOverride.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fmeOverride.Location = New System.Drawing.Point(10, 86)
        Me.fmeOverride.Name = "fmeOverride"
        Me.fmeOverride.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fmeOverride.Size = New System.Drawing.Size(269, 123)
        Me.fmeOverride.TabIndex = 36
        Me.fmeOverride.TabStop = False
        Me.fmeOverride.Text = "Exchange Rates - User Can Override"
        '
        'chkOverrideRate
        '
        Me.chkOverrideRate.BackColor = System.Drawing.SystemColors.Control
        Me.chkOverrideRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOverrideRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOverrideRate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOverrideRate.Location = New System.Drawing.Point(16, 90)
        Me.chkOverrideRate.Name = "chkOverrideRate"
        Me.chkOverrideRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOverrideRate.Size = New System.Drawing.Size(243, 25)
        Me.chkOverrideRate.TabIndex = 40
        Me.chkOverrideRate.Text = "Exchange Rate on other multi-currency screens"
        Me.chkOverrideRate.UseVisualStyleBackColor = False
        '
        'chkOverrideDate
        '
        Me.chkOverrideDate.BackColor = System.Drawing.SystemColors.Control
        Me.chkOverrideDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOverrideDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOverrideDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOverrideDate.Location = New System.Drawing.Point(16, 56)
        Me.chkOverrideDate.Name = "chkOverrideDate"
        Me.chkOverrideDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOverrideDate.Size = New System.Drawing.Size(243, 29)
        Me.chkOverrideDate.TabIndex = 39
        Me.chkOverrideDate.Text = "Exchange Date on other multi-currency screens"
        Me.chkOverrideDate.UseVisualStyleBackColor = False
        '
        'chkOverridePrePolicyRate
        '
        Me.chkOverridePrePolicyRate.BackColor = System.Drawing.SystemColors.Control
        Me.chkOverridePrePolicyRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOverridePrePolicyRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOverridePrePolicyRate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOverridePrePolicyRate.Location = New System.Drawing.Point(16, 36)
        Me.chkOverridePrePolicyRate.Name = "chkOverridePrePolicyRate"
        Me.chkOverridePrePolicyRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOverridePrePolicyRate.Size = New System.Drawing.Size(243, 17)
        Me.chkOverridePrePolicyRate.TabIndex = 38
        Me.chkOverridePrePolicyRate.Text = "Exchange Rate on Policy Screen"
        Me.chkOverridePrePolicyRate.UseVisualStyleBackColor = False
        '
        'chkOverridePrePolicyDate
        '
        Me.chkOverridePrePolicyDate.BackColor = System.Drawing.SystemColors.Control
        Me.chkOverridePrePolicyDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOverridePrePolicyDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOverridePrePolicyDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOverridePrePolicyDate.Location = New System.Drawing.Point(16, 18)
        Me.chkOverridePrePolicyDate.Name = "chkOverridePrePolicyDate"
        Me.chkOverridePrePolicyDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOverridePrePolicyDate.Size = New System.Drawing.Size(243, 17)
        Me.chkOverridePrePolicyDate.TabIndex = 37
        Me.chkOverridePrePolicyDate.Text = "Exchange Date on Policy Screen"
        Me.chkOverridePrePolicyDate.UseVisualStyleBackColor = False
        '
        'frmAuthorities
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(584, 396)
        Me.Controls.Add(Me.fmeAuthority)
        Me.Controls.Add(Me.fmeFees)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.tabMain)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAuthorities"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Authorities -"
        Me.fmeAuthority.ResumeLayout(False)
        Me.fmeFees.ResumeLayout(False)
        Me.fmeFees.PerformLayout()
        Me.tabMain.ResumeLayout(False)
        Me._tabMain_TabPage0.ResumeLayout(False)
        Me.fmeAccess.ResumeLayout(False)
        Me.fmeWriteOffs.ResumeLayout(False)
        Me.fmeWriteOffs.PerformLayout()
        Me.fmeClaimPayments.ResumeLayout(False)
        Me.fmeClaimPayments.PerformLayout()
        Me.fmePayments.ResumeLayout(False)
        Me.fmePayments.PerformLayout()
        Me.fmeOverride.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class