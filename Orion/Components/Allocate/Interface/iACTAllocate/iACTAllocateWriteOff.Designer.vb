<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWriteOff
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents lblWriteOffReason As System.Windows.Forms.Label
	Public WithEvents lbDebitAmount As System.Windows.Forms.Label
	Public WithEvents lblCreditAmount As System.Windows.Forms.Label
	Public WithEvents lblWriteOffAmount As System.Windows.Forms.Label
	Public WithEvents cboWriteoffReason As System.Windows.Forms.ComboBox
	Public WithEvents panDebitAmount As System.Windows.Forms.Panel
	Public WithEvents panCreditAmount As System.Windows.Forms.Panel
	Public WithEvents panWriteOffAmount As System.Windows.Forms.Panel
	Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMain As System.Windows.Forms.TabControl
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmWriteOff))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.tabMain = New System.Windows.Forms.TabControl
		Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage
		Me.lblWriteOffReason = New System.Windows.Forms.Label
		Me.lbDebitAmount = New System.Windows.Forms.Label
		Me.lblCreditAmount = New System.Windows.Forms.Label
		Me.lblWriteOffAmount = New System.Windows.Forms.Label
		Me.cboWriteoffReason = New System.Windows.Forms.ComboBox
		Me.panDebitAmount = New System.Windows.Forms.Panel
		Me.panCreditAmount = New System.Windows.Forms.Panel
		Me.panWriteOffAmount = New System.Windows.Forms.Panel
		Me.tabMain.SuspendLayout()
		Me._tabMain_TabPage0.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(280, 208)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 2
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(360, 208)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 1
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tabMain
		' 
		Me.tabMain.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabMain.Controls.Add(Me._tabMain_TabPage0)
		Me.tabMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMain.ItemSize = New System.Drawing.Size(140, 18)
		Me.tabMain.Location = New System.Drawing.Point(8, 8)
		Me.tabMain.Multiline = True
		Me.tabMain.Name = "tabMain"
		Me.tabMain.Size = New System.Drawing.Size(429, 197)
		Me.tabMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMain.TabIndex = 0
		' 
		' _tabMain_TabPage0
		' 
		Me._tabMain_TabPage0.Controls.Add(Me.lblWriteOffReason)
		Me._tabMain_TabPage0.Controls.Add(Me.lbDebitAmount)
		Me._tabMain_TabPage0.Controls.Add(Me.lblCreditAmount)
		Me._tabMain_TabPage0.Controls.Add(Me.lblWriteOffAmount)
		Me._tabMain_TabPage0.Controls.Add(Me.cboWriteoffReason)
		Me._tabMain_TabPage0.Controls.Add(Me.panDebitAmount)
		Me._tabMain_TabPage0.Controls.Add(Me.panCreditAmount)
		Me._tabMain_TabPage0.Controls.Add(Me.panWriteOffAmount)
		Me._tabMain_TabPage0.Text = "&! - Write Off"
		' 
		' lblWriteOffReason
		' 
		Me.lblWriteOffReason.AutoSize = True
		Me.lblWriteOffReason.BackColor = System.Drawing.SystemColors.Control
		Me.lblWriteOffReason.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblWriteOffReason.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblWriteOffReason.Enabled = True
		Me.lblWriteOffReason.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblWriteOffReason.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblWriteOffReason.Location = New System.Drawing.Point(16, 116)
		Me.lblWriteOffReason.Name = "lblWriteOffReason"
		Me.lblWriteOffReason.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblWriteOffReason.Size = New System.Drawing.Size(153, 13)
		Me.lblWriteOffReason.TabIndex = 4
		Me.lblWriteOffReason.Text = "Please select a write off reason :"
		Me.lblWriteOffReason.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblWriteOffReason.UseMnemonic = True
		Me.lblWriteOffReason.Visible = True
		' 
		' lbDebitAmount
		' 
		Me.lbDebitAmount.AutoSize = True
		Me.lbDebitAmount.BackColor = System.Drawing.SystemColors.Control
		Me.lbDebitAmount.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lbDebitAmount.Cursor = System.Windows.Forms.Cursors.Default
		Me.lbDebitAmount.Enabled = True
		Me.lbDebitAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lbDebitAmount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lbDebitAmount.Location = New System.Drawing.Point(16, 20)
		Me.lbDebitAmount.Name = "lbDebitAmount"
		Me.lbDebitAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lbDebitAmount.Size = New System.Drawing.Size(67, 13)
		Me.lbDebitAmount.TabIndex = 5
		Me.lbDebitAmount.Text = "Debit Amount:"
		Me.lbDebitAmount.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lbDebitAmount.UseMnemonic = True
		Me.lbDebitAmount.Visible = True
		' 
		' lblCreditAmount
		' 
		Me.lblCreditAmount.AutoSize = True
		Me.lblCreditAmount.BackColor = System.Drawing.SystemColors.Control
		Me.lblCreditAmount.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCreditAmount.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCreditAmount.Enabled = True
		Me.lblCreditAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCreditAmount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCreditAmount.Location = New System.Drawing.Point(16, 52)
		Me.lblCreditAmount.Name = "lblCreditAmount"
		Me.lblCreditAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCreditAmount.Size = New System.Drawing.Size(66, 13)
		Me.lblCreditAmount.TabIndex = 7
		Me.lblCreditAmount.Text = "CreditAmount:"
		Me.lblCreditAmount.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCreditAmount.UseMnemonic = True
		Me.lblCreditAmount.Visible = True
		' 
		' lblWriteOffAmount
		' 
		Me.lblWriteOffAmount.AutoSize = False
		Me.lblWriteOffAmount.BackColor = System.Drawing.SystemColors.Control
		Me.lblWriteOffAmount.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblWriteOffAmount.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblWriteOffAmount.Enabled = True
		Me.lblWriteOffAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblWriteOffAmount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblWriteOffAmount.Location = New System.Drawing.Point(16, 84)
		Me.lblWriteOffAmount.Name = "lblWriteOffAmount"
		Me.lblWriteOffAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblWriteOffAmount.Size = New System.Drawing.Size(105, 17)
		Me.lblWriteOffAmount.TabIndex = 10
		Me.lblWriteOffAmount.Text = "Write Off Amount:"
		Me.lblWriteOffAmount.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblWriteOffAmount.UseMnemonic = True
		Me.lblWriteOffAmount.Visible = True
		' 
		' cboWriteoffReason
		' 
		Me.cboWriteoffReason.BackColor = System.Drawing.SystemColors.Window
		Me.cboWriteoffReason.CausesValidation = True
		Me.cboWriteoffReason.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboWriteoffReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboWriteoffReason.Enabled = True
		Me.cboWriteoffReason.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboWriteoffReason.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboWriteoffReason.IntegralHeight = True
		Me.cboWriteoffReason.Location = New System.Drawing.Point(16, 132)
		Me.cboWriteoffReason.Name = "cboWriteoffReason"
		Me.cboWriteoffReason.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboWriteoffReason.Size = New System.Drawing.Size(393, 21)
		Me.cboWriteoffReason.Sorted = False
		Me.cboWriteoffReason.TabIndex = 3
		Me.cboWriteoffReason.TabStop = True
		Me.cboWriteoffReason.Visible = True
		' 
		' panDebitAmount
		' 
		Me.panDebitAmount.BackColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.panDebitAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.panDebitAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.panDebitAmount.Location = New System.Drawing.Point(152, 20)
		Me.panDebitAmount.Name = "panDebitAmount"
		Me.panDebitAmount.Size = New System.Drawing.Size(121, 17)
		Me.panDebitAmount.TabIndex = 6
		' 
		' panCreditAmount
		' 
		Me.panCreditAmount.BackColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.panCreditAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.panCreditAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.panCreditAmount.Location = New System.Drawing.Point(152, 52)
		Me.panCreditAmount.Name = "panCreditAmount"
		Me.panCreditAmount.Size = New System.Drawing.Size(121, 17)
		Me.panCreditAmount.TabIndex = 8
		' 
		' panWriteOffAmount
		' 
		Me.panWriteOffAmount.BackColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.panWriteOffAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.panWriteOffAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.panWriteOffAmount.Location = New System.Drawing.Point(152, 84)
		Me.panWriteOffAmount.Name = "panWriteOffAmount"
		Me.panWriteOffAmount.Size = New System.Drawing.Size(121, 17)
		Me.panWriteOffAmount.TabIndex = 9
		' 
		' frmWriteOff
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(445, 237)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.tabMain)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmWriteOff"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Write Off"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMain, 1)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tabMain.ResumeLayout(False)
		Me._tabMain_TabPage0.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class