<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializecmdButton()
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
	Private WithEvents _cmdButton_2 As System.Windows.Forms.Button
	Private WithEvents _cmdButton_1 As System.Windows.Forms.Button
	Private WithEvents _cmdButton_0 As System.Windows.Forms.Button
	Public WithEvents lblClaimNumber As System.Windows.Forms.Label
	Public WithEvents lblCoinsuranceTreatment As System.Windows.Forms.Label
	Public WithEvents txtClaimNumber As System.Windows.Forms.TextBox
	Public WithEvents lstView As System.Windows.Forms.ListView
	Private WithEvents _cmdButton_5 As System.Windows.Forms.Button
	Private WithEvents _cmdButton_4 As System.Windows.Forms.Button
	Private WithEvents _cmdButton_3 As System.Windows.Forms.Button
	Public WithEvents txtTotalNewShareValue As System.Windows.Forms.TextBox
	Public WithEvents txtTotalCurrentShareValue As System.Windows.Forms.TextBox
	Public WithEvents txtTotalSharePercentage As System.Windows.Forms.TextBox
	Public WithEvents lblTotalNewShareValue As System.Windows.Forms.Label
	Public WithEvents lblTotalCurrentShareValue As System.Windows.Forms.Label
	Public WithEvents lblTotalShare As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents cmbBox As System.Windows.Forms.ComboBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public cmdButton(5) As System.Windows.Forms.Button
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me._cmdButton_2 = New System.Windows.Forms.Button
        Me._cmdButton_1 = New System.Windows.Forms.Button
        Me._cmdButton_0 = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblClaimNumber = New System.Windows.Forms.Label
        Me.lblCoinsuranceTreatment = New System.Windows.Forms.Label
        Me.txtClaimNumber = New System.Windows.Forms.TextBox
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.lstView = New System.Windows.Forms.ListView
        Me._cmdButton_5 = New System.Windows.Forms.Button
        Me._cmdButton_4 = New System.Windows.Forms.Button
        Me._cmdButton_3 = New System.Windows.Forms.Button
        Me.txtTotalNewShareValue = New System.Windows.Forms.TextBox
        Me.txtTotalCurrentShareValue = New System.Windows.Forms.TextBox
        Me.txtTotalSharePercentage = New System.Windows.Forms.TextBox
        Me.lblTotalNewShareValue = New System.Windows.Forms.Label
        Me.lblTotalCurrentShareValue = New System.Windows.Forms.Label
        Me.lblTotalShare = New System.Windows.Forms.Label
        Me.cmbBox = New System.Windows.Forms.ComboBox
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.Frame1.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_cmdButton_2
        '
        Me._cmdButton_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdButton_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdButton_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdButton_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdButton_2.Location = New System.Drawing.Point(432, 408)
        Me._cmdButton_2.Name = "_cmdButton_2"
        Me._cmdButton_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdButton_2.Size = New System.Drawing.Size(73, 22)
        Me._cmdButton_2.TabIndex = 2
        Me._cmdButton_2.Text = "&Help"
        Me._cmdButton_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdButton_2.UseVisualStyleBackColor = False
        '
        '_cmdButton_1
        '
        Me._cmdButton_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdButton_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdButton_1.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me._cmdButton_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdButton_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdButton_1.Location = New System.Drawing.Point(352, 408)
        Me._cmdButton_1.Name = "_cmdButton_1"
        Me._cmdButton_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdButton_1.Size = New System.Drawing.Size(73, 22)
        Me._cmdButton_1.TabIndex = 1
        Me._cmdButton_1.Text = "&Cancel"
        Me._cmdButton_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdButton_1.UseVisualStyleBackColor = False
        '
        '_cmdButton_0
        '
        Me._cmdButton_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdButton_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdButton_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdButton_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdButton_0.Location = New System.Drawing.Point(270, 408)
        Me._cmdButton_0.Name = "_cmdButton_0"
        Me._cmdButton_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdButton_0.Size = New System.Drawing.Size(73, 22)
        Me._cmdButton_0.TabIndex = 0
        Me._cmdButton_0.Text = "&OK"
        Me._cmdButton_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdButton_0.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(98, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(501, 397)
        Me.tabMainTab.TabIndex = 3
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblClaimNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCoinsuranceTreatment)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtClaimNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Frame1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmbBox)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(493, 371)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "&1 - General"
        '
        'lblClaimNumber
        '
        Me.lblClaimNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblClaimNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClaimNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClaimNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClaimNumber.Location = New System.Drawing.Point(8, 23)
        Me.lblClaimNumber.Name = "lblClaimNumber"
        Me.lblClaimNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClaimNumber.Size = New System.Drawing.Size(129, 17)
        Me.lblClaimNumber.TabIndex = 5
        Me.lblClaimNumber.Text = "Claim Number :"
        '
        'lblCoinsuranceTreatment
        '
        Me.lblCoinsuranceTreatment.BackColor = System.Drawing.SystemColors.Control
        Me.lblCoinsuranceTreatment.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCoinsuranceTreatment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCoinsuranceTreatment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCoinsuranceTreatment.Location = New System.Drawing.Point(8, 55)
        Me.lblCoinsuranceTreatment.Name = "lblCoinsuranceTreatment"
        Me.lblCoinsuranceTreatment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCoinsuranceTreatment.Size = New System.Drawing.Size(153, 17)
        Me.lblCoinsuranceTreatment.TabIndex = 17
        Me.lblCoinsuranceTreatment.Text = "Coinsurance Treatment :"
        '
        'txtClaimNumber
        '
        Me.txtClaimNumber.AcceptsReturn = True
        Me.txtClaimNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtClaimNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClaimNumber.Enabled = False
        Me.txtClaimNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClaimNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClaimNumber.Location = New System.Drawing.Point(176, 20)
        Me.txtClaimNumber.MaxLength = 0
        Me.txtClaimNumber.Name = "txtClaimNumber"
        Me.txtClaimNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClaimNumber.Size = New System.Drawing.Size(217, 19)
        Me.txtClaimNumber.TabIndex = 4
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.lstView)
        Me.Frame1.Controls.Add(Me._cmdButton_5)
        Me.Frame1.Controls.Add(Me._cmdButton_4)
        Me.Frame1.Controls.Add(Me._cmdButton_3)
        Me.Frame1.Controls.Add(Me.txtTotalNewShareValue)
        Me.Frame1.Controls.Add(Me.txtTotalCurrentShareValue)
        Me.Frame1.Controls.Add(Me.txtTotalSharePercentage)
        Me.Frame1.Controls.Add(Me.lblTotalNewShareValue)
        Me.Frame1.Controls.Add(Me.lblTotalCurrentShareValue)
        Me.Frame1.Controls.Add(Me.lblTotalShare)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(8, 92)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(481, 265)
        Me.Frame1.TabIndex = 6
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Coinsurance Recoveries"
        '
        'lstView
        '
        Me.lstView.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lstView, "")
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lstView, True)
        Me.lstView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstView.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lstView.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lstView, "")
        Me.listViewHelper1.SetLargeIcons(Me.lstView, "")
        Me.lstView.Location = New System.Drawing.Point(8, 80)
        Me.lstView.Name = "lstView"
        Me.lstView.Size = New System.Drawing.Size(465, 145)
        Me.listViewHelper1.SetSmallIcons(Me.lstView, "")
        Me.listViewHelper1.SetSorted(Me.lstView, False)
        Me.listViewHelper1.SetSortKey(Me.lstView, 0)
        Me.listViewHelper1.SetSortOrder(Me.lstView, System.Windows.Forms.SortOrder.Ascending)
        Me.lstView.TabIndex = 18
        Me.lstView.UseCompatibleStateImageBehavior = False
        Me.lstView.View = System.Windows.Forms.View.Details
        '
        '_cmdButton_5
        '
        Me._cmdButton_5.BackColor = System.Drawing.SystemColors.Control
        Me._cmdButton_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdButton_5.Enabled = False
        Me._cmdButton_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdButton_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdButton_5.Location = New System.Drawing.Point(400, 232)
        Me._cmdButton_5.Name = "_cmdButton_5"
        Me._cmdButton_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdButton_5.Size = New System.Drawing.Size(73, 22)
        Me._cmdButton_5.TabIndex = 15
        Me._cmdButton_5.Text = "&Delete"
        Me._cmdButton_5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdButton_5.UseVisualStyleBackColor = False
        '
        '_cmdButton_4
        '
        Me._cmdButton_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdButton_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdButton_4.Enabled = False
        Me._cmdButton_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdButton_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdButton_4.Location = New System.Drawing.Point(320, 232)
        Me._cmdButton_4.Name = "_cmdButton_4"
        Me._cmdButton_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdButton_4.Size = New System.Drawing.Size(73, 22)
        Me._cmdButton_4.TabIndex = 14
        Me._cmdButton_4.Text = "&Edit"
        Me._cmdButton_4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdButton_4.UseVisualStyleBackColor = False
        '
        '_cmdButton_3
        '
        Me._cmdButton_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdButton_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdButton_3.Enabled = False
        Me._cmdButton_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdButton_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdButton_3.Location = New System.Drawing.Point(240, 232)
        Me._cmdButton_3.Name = "_cmdButton_3"
        Me._cmdButton_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdButton_3.Size = New System.Drawing.Size(73, 22)
        Me._cmdButton_3.TabIndex = 13
        Me._cmdButton_3.Text = "&Add"
        Me._cmdButton_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdButton_3.UseVisualStyleBackColor = False
        '
        'txtTotalNewShareValue
        '
        Me.txtTotalNewShareValue.AcceptsReturn = True
        Me.txtTotalNewShareValue.BackColor = System.Drawing.SystemColors.Window
        Me.txtTotalNewShareValue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalNewShareValue.Enabled = False
        Me.txtTotalNewShareValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalNewShareValue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalNewShareValue.Location = New System.Drawing.Point(352, 56)
        Me.txtTotalNewShareValue.MaxLength = 0
        Me.txtTotalNewShareValue.Name = "txtTotalNewShareValue"
        Me.txtTotalNewShareValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalNewShareValue.Size = New System.Drawing.Size(113, 19)
        Me.txtTotalNewShareValue.TabIndex = 9
        Me.txtTotalNewShareValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTotalCurrentShareValue
        '
        Me.txtTotalCurrentShareValue.AcceptsReturn = True
        Me.txtTotalCurrentShareValue.BackColor = System.Drawing.SystemColors.Window
        Me.txtTotalCurrentShareValue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalCurrentShareValue.Enabled = False
        Me.txtTotalCurrentShareValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalCurrentShareValue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalCurrentShareValue.Location = New System.Drawing.Point(352, 24)
        Me.txtTotalCurrentShareValue.MaxLength = 0
        Me.txtTotalCurrentShareValue.Name = "txtTotalCurrentShareValue"
        Me.txtTotalCurrentShareValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalCurrentShareValue.Size = New System.Drawing.Size(113, 19)
        Me.txtTotalCurrentShareValue.TabIndex = 8
        Me.txtTotalCurrentShareValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTotalSharePercentage
        '
        Me.txtTotalSharePercentage.AcceptsReturn = True
        Me.txtTotalSharePercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtTotalSharePercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalSharePercentage.Enabled = False
        Me.txtTotalSharePercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalSharePercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalSharePercentage.Location = New System.Drawing.Point(112, 24)
        Me.txtTotalSharePercentage.MaxLength = 0
        Me.txtTotalSharePercentage.Name = "txtTotalSharePercentage"
        Me.txtTotalSharePercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalSharePercentage.Size = New System.Drawing.Size(57, 19)
        Me.txtTotalSharePercentage.TabIndex = 7
        '
        'lblTotalNewShareValue
        '
        Me.lblTotalNewShareValue.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalNewShareValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalNewShareValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalNewShareValue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalNewShareValue.Location = New System.Drawing.Point(176, 59)
        Me.lblTotalNewShareValue.Name = "lblTotalNewShareValue"
        Me.lblTotalNewShareValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalNewShareValue.Size = New System.Drawing.Size(169, 17)
        Me.lblTotalNewShareValue.TabIndex = 12
        Me.lblTotalNewShareValue.Text = "Total New Share Value :"
        '
        'lblTotalCurrentShareValue
        '
        Me.lblTotalCurrentShareValue.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalCurrentShareValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalCurrentShareValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalCurrentShareValue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalCurrentShareValue.Location = New System.Drawing.Point(176, 27)
        Me.lblTotalCurrentShareValue.Name = "lblTotalCurrentShareValue"
        Me.lblTotalCurrentShareValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalCurrentShareValue.Size = New System.Drawing.Size(169, 17)
        Me.lblTotalCurrentShareValue.TabIndex = 11
        Me.lblTotalCurrentShareValue.Text = "Total Current Share Value :"
        '
        'lblTotalShare
        '
        Me.lblTotalShare.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalShare.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalShare.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalShare.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalShare.Location = New System.Drawing.Point(8, 27)
        Me.lblTotalShare.Name = "lblTotalShare"
        Me.lblTotalShare.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalShare.Size = New System.Drawing.Size(153, 17)
        Me.lblTotalShare.TabIndex = 10
        Me.lblTotalShare.Text = "Total Share (%) :"
        '
        'cmbBox
        '
        Me.cmbBox.BackColor = System.Drawing.SystemColors.Window
        Me.cmbBox.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbBox.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbBox.Location = New System.Drawing.Point(176, 52)
        Me.cmbBox.Name = "cmbBox"
        Me.cmbBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbBox.Size = New System.Drawing.Size(217, 21)
        Me.cmbBox.TabIndex = 16
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me._cmdButton_1
        Me.ClientSize = New System.Drawing.Size(511, 434)
        Me.Controls.Add(Me._cmdButton_2)
        Me.Controls.Add(Me._cmdButton_1)
        Me.Controls.Add(Me._cmdButton_0)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.Frame1.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
	Sub InitializecmdButton()
		Me.cmdButton(2) = _cmdButton_2
		Me.cmdButton(1) = _cmdButton_1
		Me.cmdButton(0) = _cmdButton_0
		Me.cmdButton(5) = _cmdButton_5
		Me.cmdButton(4) = _cmdButton_4
		Me.cmdButton(3) = _cmdButton_3
	End Sub
#End Region 
End Class