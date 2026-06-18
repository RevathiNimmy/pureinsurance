<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTreatyParty
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
	Public WithEvents txtReinsurer As System.Windows.Forms.TextBox
	Public WithEvents chkIsDomiciledForTax As System.Windows.Forms.CheckBox
	Public WithEvents cboTaxGroup As PMLookupControl.cboPMLookup
	Public WithEvents lblTaxGroup As System.Windows.Forms.Label
	Public WithEvents fraTax As System.Windows.Forms.GroupBox
	Public WithEvents cmdReinsurer As System.Windows.Forms.Button
	Public WithEvents txtCommPercent As System.Windows.Forms.TextBox
	Public WithEvents txtSharePercent As System.Windows.Forms.TextBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblCommPercent As System.Windows.Forms.Label
	Public WithEvents lblSharePercent As System.Windows.Forms.Label
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtReinsurer = New System.Windows.Forms.TextBox()
        Me.fraTax = New System.Windows.Forms.GroupBox()
        Me.chkIsDomiciledForTax = New System.Windows.Forms.CheckBox()
        Me.cboTaxGroup = New PMLookupControl.cboPMLookup()
        Me.lblTaxGroup = New System.Windows.Forms.Label()
        Me.cmdReinsurer = New System.Windows.Forms.Button()
        Me.txtCommPercent = New System.Windows.Forms.TextBox()
        Me.txtSharePercent = New System.Windows.Forms.TextBox()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.lblCommPercent = New System.Windows.Forms.Label()
        Me.lblSharePercent = New System.Windows.Forms.Label()
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.chkIsReinsurerApproved = New System.Windows.Forms.CheckBox()
        Me.fraTax.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtReinsurer
        '
        Me.txtReinsurer.AcceptsReturn = True
        Me.txtReinsurer.BackColor = System.Drawing.SystemColors.Control
        Me.txtReinsurer.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtReinsurer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReinsurer.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtReinsurer.Location = New System.Drawing.Point(154, 12)
        Me.txtReinsurer.MaxLength = 0
        Me.txtReinsurer.Name = "txtReinsurer"
        Me.txtReinsurer.ReadOnly = True
        Me.txtReinsurer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtReinsurer.Size = New System.Drawing.Size(257, 20)
        Me.txtReinsurer.TabIndex = 1
        '
        'fraTax
        '
        Me.fraTax.BackColor = System.Drawing.SystemColors.Control
        Me.fraTax.Controls.Add(Me.chkIsDomiciledForTax)
        Me.fraTax.Controls.Add(Me.cboTaxGroup)
        Me.fraTax.Controls.Add(Me.lblTaxGroup)
        Me.fraTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTax.Location = New System.Drawing.Point(9, 121)
        Me.fraTax.Name = "fraTax"
        Me.fraTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTax.Size = New System.Drawing.Size(405, 81)
        Me.fraTax.TabIndex = 6
        Me.fraTax.TabStop = False
        Me.fraTax.Text = "Reinsurer Tax Details"
        '
        'chkIsDomiciledForTax
        '
        Me.chkIsDomiciledForTax.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsDomiciledForTax.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsDomiciledForTax.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsDomiciledForTax.Enabled = False
        Me.chkIsDomiciledForTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsDomiciledForTax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsDomiciledForTax.Location = New System.Drawing.Point(10, 52)
        Me.chkIsDomiciledForTax.Name = "chkIsDomiciledForTax"
        Me.chkIsDomiciledForTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsDomiciledForTax.Size = New System.Drawing.Size(148, 15)
        Me.chkIsDomiciledForTax.TabIndex = 5
        Me.chkIsDomiciledForTax.Text = "Is Domiciled for Tax:"
        Me.chkIsDomiciledForTax.UseVisualStyleBackColor = False
        '
        'cboTaxGroup
        '
        Me.cboTaxGroup.DefaultItemId = 0
        Me.cboTaxGroup.Enabled = False
        Me.cboTaxGroup.FirstItem = ""
        Me.cboTaxGroup.ItemId = 0
        Me.cboTaxGroup.ListIndex = -1
        Me.cboTaxGroup.Location = New System.Drawing.Point(145, 24)
        Me.cboTaxGroup.Name = "cboTaxGroup"
        Me.cboTaxGroup.PMLookupProductFamily = 1
        Me.cboTaxGroup.SingleItemId = 0
        Me.cboTaxGroup.Size = New System.Drawing.Size(247, 21)
        Me.cboTaxGroup.Sorted = True
        Me.cboTaxGroup.TabIndex = 5
        Me.cboTaxGroup.TableName = "tax_group"
        Me.cboTaxGroup.ToolTipText = ""
        Me.cboTaxGroup.WhereClause = ""
        '
        'lblTaxGroup
        '
        Me.lblTaxGroup.AutoSize = True
        Me.lblTaxGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxGroup.Enabled = False
        Me.lblTaxGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxGroup.Location = New System.Drawing.Point(12, 26)
        Me.lblTaxGroup.Name = "lblTaxGroup"
        Me.lblTaxGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxGroup.Size = New System.Drawing.Size(60, 13)
        Me.lblTaxGroup.TabIndex = 7
        Me.lblTaxGroup.Text = "Tax Group:"
        '
        'cmdReinsurer
        '
        Me.cmdReinsurer.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReinsurer.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReinsurer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReinsurer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReinsurer.Location = New System.Drawing.Point(16, 10)
        Me.cmdReinsurer.Name = "cmdReinsurer"
        Me.cmdReinsurer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReinsurer.Size = New System.Drawing.Size(96, 22)
        Me.cmdReinsurer.TabIndex = 0
        Me.cmdReinsurer.Text = "Reinsurer..."
        Me.cmdReinsurer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdReinsurer.UseVisualStyleBackColor = False
        '
        'txtCommPercent
        '
        Me.txtCommPercent.AcceptsReturn = True
        Me.txtCommPercent.BackColor = System.Drawing.SystemColors.Window
        Me.txtCommPercent.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCommPercent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCommPercent.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCommPercent.Location = New System.Drawing.Point(152, 84)
        Me.txtCommPercent.MaxLength = 20
        Me.txtCommPercent.Name = "txtCommPercent"
        Me.txtCommPercent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCommPercent.Size = New System.Drawing.Size(100, 20)
        Me.txtCommPercent.TabIndex = 4
        Me.txtCommPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtSharePercent
        '
        Me.txtSharePercent.AcceptsReturn = True
        Me.txtSharePercent.BackColor = System.Drawing.SystemColors.Window
        Me.txtSharePercent.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSharePercent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSharePercent.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSharePercent.Location = New System.Drawing.Point(154, 51)
        Me.txtSharePercent.MaxLength = 20
        Me.txtSharePercent.Name = "txtSharePercent"
        Me.txtSharePercent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSharePercent.Size = New System.Drawing.Size(100, 20)
        Me.txtSharePercent.TabIndex = 3
        Me.txtSharePercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(340, 211)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 8
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
        Me.cmdOK.Location = New System.Drawing.Point(260, 211)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 7
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'lblCommPercent
        '
        Me.lblCommPercent.AutoSize = True
        Me.lblCommPercent.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommPercent.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommPercent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCommPercent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommPercent.Location = New System.Drawing.Point(16, 87)
        Me.lblCommPercent.Name = "lblCommPercent"
        Me.lblCommPercent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommPercent.Size = New System.Drawing.Size(76, 13)
        Me.lblCommPercent.TabIndex = 4
        Me.lblCommPercent.Text = "Commission %:"
        '
        'lblSharePercent
        '
        Me.lblSharePercent.AutoSize = True
        Me.lblSharePercent.BackColor = System.Drawing.SystemColors.Control
        Me.lblSharePercent.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSharePercent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSharePercent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSharePercent.Location = New System.Drawing.Point(18, 54)
        Me.lblSharePercent.Name = "lblSharePercent"
        Me.lblSharePercent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSharePercent.Size = New System.Drawing.Size(49, 13)
        Me.lblSharePercent.TabIndex = 2
        Me.lblSharePercent.Text = "Share %:"
        '
        'chkIsReinsurerApproved
        '
        Me.chkIsReinsurerApproved.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsReinsurerApproved.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsReinsurerApproved.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsReinsurerApproved.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsReinsurerApproved.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsReinsurerApproved.Location = New System.Drawing.Point(312, 50)
        Me.chkIsReinsurerApproved.Name = "chkIsReinsurerApproved"
        Me.chkIsReinsurerApproved.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsReinsurerApproved.Size = New System.Drawing.Size(89, 23)
        Me.chkIsReinsurerApproved.TabIndex = 2
        Me.chkIsReinsurerApproved.Text = "Approved :"
        Me.chkIsReinsurerApproved.UseVisualStyleBackColor = False
        Me.chkIsReinsurerApproved.Visible = False
        '
        'frmTreatyParty
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(422, 272)
        Me.Controls.Add(Me.chkIsReinsurerApproved)
        Me.Controls.Add(Me.txtReinsurer)
        Me.Controls.Add(Me.fraTax)
        Me.Controls.Add(Me.cmdReinsurer)
        Me.Controls.Add(Me.txtCommPercent)
        Me.Controls.Add(Me.txtSharePercent)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.lblCommPercent)
        Me.Controls.Add(Me.lblSharePercent)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmTreatyParty"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Treaty Parties"
        Me.fraTax.ResumeLayout(False)
        Me.fraTax.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents chkIsReinsurerApproved As System.Windows.Forms.CheckBox
#End Region 
End Class