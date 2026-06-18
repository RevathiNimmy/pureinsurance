<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
        MyBase.New()
        Me.KeyPreview = True
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
    
    Public WithEvents cryManualRenewal As Object
	Public WithEvents lblProductCode As System.Windows.Forms.Label
	Public WithEvents lblRenewalDate As System.Windows.Forms.Label
	Public WithEvents lblBranchCode As System.Windows.Forms.Label
	Public WithEvents txtRenewalDate As System.Windows.Forms.TextBox
	Public WithEvents cboProductCode As System.Windows.Forms.ComboBox
	Public WithEvents chkPreview As System.Windows.Forms.CheckBox
	Public WithEvents chkPrint As System.Windows.Forms.CheckBox
	Public WithEvents cmdRePrint As System.Windows.Forms.Button
	Public WithEvents cboBranchCode As System.Windows.Forms.ComboBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
    
    Public WithEvents cryAutoRenewal As Object
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblProductCode = New System.Windows.Forms.Label
        Me.lblRenewalDate = New System.Windows.Forms.Label
        Me.lblBranchCode = New System.Windows.Forms.Label
        Me.txtRenewalDate = New System.Windows.Forms.TextBox
        Me.cboProductCode = New System.Windows.Forms.ComboBox
        Me.chkPreview = New System.Windows.Forms.CheckBox
        Me.chkPrint = New System.Windows.Forms.CheckBox
        Me.cmdRePrint = New System.Windows.Forms.Button
        Me.cboBranchCode = New System.Windows.Forms.ComboBox
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(426, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(431, 178)
        Me.tabMainTab.TabIndex = 7
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblProductCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblRenewalDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblBranchCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtRenewalDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboProductCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkPreview)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkPrint)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdRePrint)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboBranchCode)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(423, 152)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1-Renewal Pre-list"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'lblProductCode
        '
        Me.lblProductCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblProductCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProductCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProductCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProductCode.Location = New System.Drawing.Point(24, 52)
        Me.lblProductCode.Name = "lblProductCode"
        Me.lblProductCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProductCode.Size = New System.Drawing.Size(94, 17)
        Me.lblProductCode.TabIndex = 8
        Me.lblProductCode.Text = "Product Code:"
        '
        'lblRenewalDate
        '
        Me.lblRenewalDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblRenewalDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRenewalDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRenewalDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRenewalDate.Location = New System.Drawing.Point(24, 19)
        Me.lblRenewalDate.Name = "lblRenewalDate"
        Me.lblRenewalDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRenewalDate.Size = New System.Drawing.Size(99, 17)
        Me.lblRenewalDate.TabIndex = 9
        Me.lblRenewalDate.Text = "Renewal Date:"
        '
        'lblBranchCode
        '
        Me.lblBranchCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranchCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranchCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranchCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranchCode.Location = New System.Drawing.Point(24, 84)
        Me.lblBranchCode.Name = "lblBranchCode"
        Me.lblBranchCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranchCode.Size = New System.Drawing.Size(94, 17)
        Me.lblBranchCode.TabIndex = 11
        Me.lblBranchCode.Text = "Branch Code:"
        '
        'txtRenewalDate
        '
        Me.txtRenewalDate.AcceptsReturn = True
        Me.txtRenewalDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtRenewalDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRenewalDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRenewalDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRenewalDate.Location = New System.Drawing.Point(123, 16)
        Me.txtRenewalDate.MaxLength = 0
        Me.txtRenewalDate.Name = "txtRenewalDate"
        Me.txtRenewalDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRenewalDate.Size = New System.Drawing.Size(149, 20)
        Me.txtRenewalDate.TabIndex = 0
        '
        'cboProductCode
        '
        Me.cboProductCode.BackColor = System.Drawing.SystemColors.Window
        Me.cboProductCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboProductCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboProductCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboProductCode.Location = New System.Drawing.Point(120, 48)
        Me.cboProductCode.Name = "cboProductCode"
        Me.cboProductCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboProductCode.Size = New System.Drawing.Size(289, 21)
        Me.cboProductCode.TabIndex = 1
        '
        'chkPreview
        '
        Me.chkPreview.BackColor = System.Drawing.SystemColors.Control
        Me.chkPreview.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkPreview.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPreview.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPreview.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPreview.Location = New System.Drawing.Point(24, 124)
        Me.chkPreview.Name = "chkPreview"
        Me.chkPreview.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPreview.Size = New System.Drawing.Size(73, 17)
        Me.chkPreview.TabIndex = 2
        Me.chkPreview.Text = "View"
        Me.chkPreview.UseVisualStyleBackColor = False
        '
        'chkPrint
        '
        Me.chkPrint.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrint.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkPrint.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPrint.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrint.Location = New System.Drawing.Point(120, 124)
        Me.chkPrint.Name = "chkPrint"
        Me.chkPrint.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPrint.Size = New System.Drawing.Size(73, 17)
        Me.chkPrint.TabIndex = 3
        Me.chkPrint.Text = "Print"
        Me.chkPrint.UseVisualStyleBackColor = False
        '
        'cmdRePrint
        '
        Me.cmdRePrint.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRePrint.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRePrint.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRePrint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRePrint.Location = New System.Drawing.Point(336, 124)
        Me.cmdRePrint.Name = "cmdRePrint"
        Me.cmdRePrint.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRePrint.Size = New System.Drawing.Size(73, 22)
        Me.cmdRePrint.TabIndex = 10
        Me.cmdRePrint.Text = "RePrint"
        Me.cmdRePrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRePrint.UseVisualStyleBackColor = False
        '
        'cboBranchCode
        '
        Me.cboBranchCode.BackColor = System.Drawing.SystemColors.Window
        Me.cboBranchCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBranchCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBranchCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBranchCode.Location = New System.Drawing.Point(120, 84)
        Me.cboBranchCode.Name = "cboBranchCode"
        Me.cboBranchCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBranchCode.Size = New System.Drawing.Size(289, 21)
        Me.cboBranchCode.TabIndex = 12
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(364, 200)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 6
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
        Me.cmdCancel.Location = New System.Drawing.Point(284, 200)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 5
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
        Me.cmdOK.Location = New System.Drawing.Point(204, 200)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 4
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(444, 230)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Renewal Pre-list"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class