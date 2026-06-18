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
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cboCompany As System.Windows.Forms.ComboBox
	Public WithEvents cboPaymentGroup As System.Windows.Forms.ComboBox
	Public WithEvents lblCompany As System.Windows.Forms.Label
	Public WithEvents lblGroup As System.Windows.Forms.Label
	Public WithEvents fraGeneral As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdHelp = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.tabMainTab = New System.Windows.Forms.TabControl
		Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
		Me.fraGeneral = New System.Windows.Forms.GroupBox
		Me.cboCompany = New System.Windows.Forms.ComboBox
		Me.cboPaymentGroup = New System.Windows.Forms.ComboBox
		Me.lblCompany = New System.Windows.Forms.Label
		Me.lblGroup = New System.Windows.Forms.Label
		Me.tabMainTab.SuspendLayout()
		Me._tabMainTab_TabPage0.SuspendLayout()
		Me.fraGeneral.SuspendLayout()
		Me.SuspendLayout()
		' 
		' cmdHelp
		' 
		Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
		Me.cmdHelp.CausesValidation = True
		Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdHelp.Enabled = True
		Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdHelp.Location = New System.Drawing.Point(472, 192)
		Me.cmdHelp.Name = "cmdHelp"
		Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.cmdHelp.TabIndex = 2
		Me.cmdHelp.TabStop = True
		Me.cmdHelp.Text = "&Help"
		Me.cmdHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(392, 192)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 1
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(312, 192)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 0
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tabMainTab
		' 
		Me.tabMainTab.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabMainTab.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
		Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMainTab.ItemSize = New System.Drawing.Size(106, 18)
		Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
		Me.tabMainTab.Multiline = True
		Me.tabMainTab.Name = "tabMainTab"
		Me.tabMainTab.Size = New System.Drawing.Size(541, 181)
		Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMainTab.TabIndex = 3
		Me.tabMainTab.TabStop = False
		' 
		' _tabMainTab_TabPage0
		' 
		Me._tabMainTab_TabPage0.Controls.Add(Me.fraGeneral)
		Me._tabMainTab_TabPage0.Text = "&1 - General"
		' 
		' fraGeneral
		' 
		Me.fraGeneral.BackColor = System.Drawing.SystemColors.Control
		Me.fraGeneral.Controls.Add(Me.cboCompany)
		Me.fraGeneral.Controls.Add(Me.cboPaymentGroup)
		Me.fraGeneral.Controls.Add(Me.lblCompany)
		Me.fraGeneral.Controls.Add(Me.lblGroup)
		Me.fraGeneral.Enabled = True
		Me.fraGeneral.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraGeneral.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraGeneral.Location = New System.Drawing.Point(16, 12)
		Me.fraGeneral.Name = "fraGeneral"
		Me.fraGeneral.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraGeneral.Size = New System.Drawing.Size(457, 105)
		Me.fraGeneral.TabIndex = 4
		Me.fraGeneral.Visible = True
		' 
		' cboCompany
		' 
		Me.cboCompany.BackColor = System.Drawing.SystemColors.Window
		Me.cboCompany.CausesValidation = True
		Me.cboCompany.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboCompany.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown
		Me.cboCompany.Enabled = True
		Me.cboCompany.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboCompany.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboCompany.IntegralHeight = True
		Me.cboCompany.Location = New System.Drawing.Point(144, 56)
		Me.cboCompany.Name = "cboCompany"
		Me.cboCompany.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboCompany.Size = New System.Drawing.Size(153, 21)
		Me.cboCompany.Sorted = False
		Me.cboCompany.TabIndex = 8
		Me.cboCompany.TabStop = True
		Me.cboCompany.Text = " "
		Me.cboCompany.Visible = True
		' 
		' cboPaymentGroup
		' 
		Me.cboPaymentGroup.BackColor = System.Drawing.SystemColors.Window
		Me.cboPaymentGroup.CausesValidation = True
		Me.cboPaymentGroup.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboPaymentGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown
		Me.cboPaymentGroup.Enabled = True
		Me.cboPaymentGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboPaymentGroup.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboPaymentGroup.IntegralHeight = True
		Me.cboPaymentGroup.Location = New System.Drawing.Point(144, 32)
		Me.cboPaymentGroup.Name = "cboPaymentGroup"
		Me.cboPaymentGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboPaymentGroup.Size = New System.Drawing.Size(153, 21)
		Me.cboPaymentGroup.Sorted = False
		Me.cboPaymentGroup.TabIndex = 7
		Me.cboPaymentGroup.TabStop = True
		Me.cboPaymentGroup.Text = " "
		Me.cboPaymentGroup.Visible = True
		' 
		' lblCompany
		' 
		Me.lblCompany.AutoSize = False
		Me.lblCompany.BackColor = System.Drawing.SystemColors.Control
		Me.lblCompany.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCompany.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCompany.Enabled = True
		Me.lblCompany.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCompany.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCompany.Location = New System.Drawing.Point(16, 56)
		Me.lblCompany.Name = "lblCompany"
		Me.lblCompany.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCompany.Size = New System.Drawing.Size(89, 17)
		Me.lblCompany.TabIndex = 6
		Me.lblCompany.Text = "Company:"
		Me.lblCompany.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCompany.UseMnemonic = True
		Me.lblCompany.Visible = True
		' 
		' lblGroup
		' 
		Me.lblGroup.AutoSize = False
		Me.lblGroup.BackColor = System.Drawing.SystemColors.Control
		Me.lblGroup.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblGroup.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblGroup.Enabled = True
		Me.lblGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblGroup.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblGroup.Location = New System.Drawing.Point(16, 32)
		Me.lblGroup.Name = "lblGroup"
		Me.lblGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblGroup.Size = New System.Drawing.Size(121, 17)
		Me.lblGroup.TabIndex = 5
		Me.lblGroup.Text = "Payment Group:"
		Me.lblGroup.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblGroup.UseMnemonic = True
		Me.lblGroup.Visible = True
		' 
		' frmInterface
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(553, 233)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdHelp)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.tabMainTab)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
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
		Me.Text = "Payment Group"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMainTab, 1)
		Me.tabMainTab.ResumeLayout(False)
		Me._tabMainTab_TabPage0.ResumeLayout(False)
		Me.fraGeneral.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class