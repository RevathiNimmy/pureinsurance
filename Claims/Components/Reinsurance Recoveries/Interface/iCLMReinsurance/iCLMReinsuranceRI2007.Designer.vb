<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterfaceRI2007
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		tabRIPreviousTab = tabRI.SelectedIndex
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
	Public WithEvents txtRI_version As System.Windows.Forms.TextBox
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdView As System.Windows.Forms.Button
	Public WithEvents cmdAddFACXOL As System.Windows.Forms.Button
	Public WithEvents cmdAddXOLTreaty As System.Windows.Forms.Button
	Public WithEvents cmdAddFAC As System.Windows.Forms.Button
	Public WithEvents cmdAddTreaty As System.Windows.Forms.Button
	Public WithEvents cboRIBand As System.Windows.Forms.ComboBox
	Public WithEvents uctRI As cSIRRIControls.uctClaimRIControlRI2007
	Private WithEvents _tabRI_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents uctSummary As cSIRRIControls.uctRIModelControl
	Private WithEvents _tabRI_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents tabRI As System.Windows.Forms.TabControl
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents lblReinsurance_version As System.Windows.Forms.Label
	Public WithEvents lblRIBand As System.Windows.Forms.Label
	Dim Private tabRIPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterfaceRI2007))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtRI_version = New System.Windows.Forms.TextBox
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdView = New System.Windows.Forms.Button
        Me.cmdAddFACXOL = New System.Windows.Forms.Button
        Me.cmdAddXOLTreaty = New System.Windows.Forms.Button
        Me.cmdAddFAC = New System.Windows.Forms.Button
        Me.cmdAddTreaty = New System.Windows.Forms.Button
        Me.cboRIBand = New System.Windows.Forms.ComboBox
        Me.tabRI = New System.Windows.Forms.TabControl
        Me._tabRI_TabPage0 = New System.Windows.Forms.TabPage
        Me.uctRI = New cSIRRIControls.uctClaimRIControlRI2007
        Me._tabRI_TabPage1 = New System.Windows.Forms.TabPage
        Me.uctSummary = New cSIRRIControls.uctRIModelControl
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.lblReinsurance_version = New System.Windows.Forms.Label
        Me.lblRIBand = New System.Windows.Forms.Label
        Me.tabRI.SuspendLayout()
        Me._tabRI_TabPage0.SuspendLayout()
        Me._tabRI_TabPage1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtRI_version
        '
        Me.txtRI_version.AcceptsReturn = True
        Me.txtRI_version.BackColor = System.Drawing.SystemColors.Window
        Me.txtRI_version.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRI_version.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRI_version.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRI_version.Location = New System.Drawing.Point(944, 8)
        Me.txtRI_version.MaxLength = 0
        Me.txtRI_version.Name = "txtRI_version"
        Me.txtRI_version.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRI_version.Size = New System.Drawing.Size(41, 20)
        Me.txtRI_version.TabIndex = 15
        Me.txtRI_version.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Enabled = False
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(592, 400)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(80, 22)
        Me.cmdDelete.TabIndex = 13
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Enabled = False
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(509, 400)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(80, 22)
        Me.cmdEdit.TabIndex = 12
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdView
        '
        Me.cmdView.BackColor = System.Drawing.SystemColors.Control
        Me.cmdView.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdView.Enabled = False
        Me.cmdView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdView.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdView.Location = New System.Drawing.Point(426, 400)
        Me.cmdView.Name = "cmdView"
        Me.cmdView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdView.Size = New System.Drawing.Size(80, 22)
        Me.cmdView.TabIndex = 11
        Me.cmdView.Text = "&View"
        Me.cmdView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdView.UseVisualStyleBackColor = False
        '
        'cmdAddFACXOL
        '
        Me.cmdAddFACXOL.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddFACXOL.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddFACXOL.Enabled = False
        Me.cmdAddFACXOL.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddFACXOL.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddFACXOL.Location = New System.Drawing.Point(323, 400)
        Me.cmdAddFACXOL.Name = "cmdAddFACXOL"
        Me.cmdAddFACXOL.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddFACXOL.Size = New System.Drawing.Size(100, 22)
        Me.cmdAddFACXOL.TabIndex = 10
        Me.cmdAddFACXOL.Text = "Add FAC XO&L"
        Me.cmdAddFACXOL.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddFACXOL.UseVisualStyleBackColor = False
        '
        'cmdAddXOLTreaty
        '
        Me.cmdAddXOLTreaty.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddXOLTreaty.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddXOLTreaty.Enabled = False
        Me.cmdAddXOLTreaty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddXOLTreaty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddXOLTreaty.Location = New System.Drawing.Point(220, 400)
        Me.cmdAddXOLTreaty.Name = "cmdAddXOLTreaty"
        Me.cmdAddXOLTreaty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddXOLTreaty.Size = New System.Drawing.Size(100, 22)
        Me.cmdAddXOLTreaty.TabIndex = 9
        Me.cmdAddXOLTreaty.Text = "Add &XOL Treaty"
        Me.cmdAddXOLTreaty.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddXOLTreaty.UseVisualStyleBackColor = False
        '
        'cmdAddFAC
        '
        Me.cmdAddFAC.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddFAC.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddFAC.Enabled = False
        Me.cmdAddFAC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddFAC.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddFAC.Location = New System.Drawing.Point(113, 400)
        Me.cmdAddFAC.Name = "cmdAddFAC"
        Me.cmdAddFAC.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddFAC.Size = New System.Drawing.Size(104, 22)
        Me.cmdAddFAC.TabIndex = 5
        Me.cmdAddFAC.Text = "Add Prop &FAC"
        Me.cmdAddFAC.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddFAC.UseVisualStyleBackColor = False
        '
        'cmdAddTreaty
        '
        Me.cmdAddTreaty.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddTreaty.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddTreaty.Enabled = False
        Me.cmdAddTreaty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddTreaty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddTreaty.Location = New System.Drawing.Point(6, 400)
        Me.cmdAddTreaty.Name = "cmdAddTreaty"
        Me.cmdAddTreaty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddTreaty.Size = New System.Drawing.Size(104, 22)
        Me.cmdAddTreaty.TabIndex = 3
        Me.cmdAddTreaty.Text = "Add Prop &Treaty"
        Me.cmdAddTreaty.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddTreaty.UseVisualStyleBackColor = False
        '
        'cboRIBand
        '
        Me.cboRIBand.BackColor = System.Drawing.SystemColors.Window
        Me.cboRIBand.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboRIBand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRIBand.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRIBand.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboRIBand.Location = New System.Drawing.Point(140, 9)
        Me.cboRIBand.Name = "cboRIBand"
        Me.cboRIBand.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboRIBand.Size = New System.Drawing.Size(240, 21)
        Me.cboRIBand.TabIndex = 6
        '
        'tabRI
        '
        Me.tabRI.Controls.Add(Me._tabRI_TabPage0)
        Me.tabRI.Controls.Add(Me._tabRI_TabPage1)
        Me.tabRI.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabRI.ItemSize = New System.Drawing.Size(490, 18)
        Me.tabRI.Location = New System.Drawing.Point(6, 38)
        Me.tabRI.Multiline = True
        Me.tabRI.Name = "tabRI"
        Me.tabRI.SelectedIndex = 0
        Me.tabRI.Size = New System.Drawing.Size(987, 359)
        Me.tabRI.TabIndex = 2
        '
        '_tabRI_TabPage0
        '
        Me._tabRI_TabPage0.Controls.Add(Me.uctRI)
        Me._tabRI_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabRI_TabPage0.Name = "_tabRI_TabPage0"
        Me._tabRI_TabPage0.Size = New System.Drawing.Size(979, 333)
        Me._tabRI_TabPage0.TabIndex = 0
        Me._tabRI_TabPage0.Text = "Reinsurance"
        '
        'uctRI
        '
        Me.uctRI.DeletedRIArragementIds = Nothing
        Me.uctRI.Dock = System.Windows.Forms.DockStyle.Fill
        Me.uctRI.ExistingLimits = Nothing
        Me.uctRI.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctRI.IsDirty = False
        Me.uctRI.Location = New System.Drawing.Point(0, 0)
        Me.uctRI.Name = "uctRI"
        Me.uctRI.ReadOnly_Renamed = False
        Me.uctRI.RowCountFAC = 0
        Me.uctRI.RowCountRetained = 0
        Me.uctRI.RowCountTreaty = 0
        Me.uctRI.SelectedRIType = ""
        Me.uctRI.SelectedRow = 0
        Me.uctRI.SelRIArrangementLine = 0
        Me.uctRI.ShowPayments = True
        Me.uctRI.Size = New System.Drawing.Size(979, 333)
        Me.uctRI.TabIndex = 0
        Me.uctRI.TransactionType = ""
        '
        '_tabRI_TabPage1
        '
        Me._tabRI_TabPage1.Controls.Add(Me.uctSummary)
        Me._tabRI_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabRI_TabPage1.Name = "_tabRI_TabPage1"
        Me._tabRI_TabPage1.Size = New System.Drawing.Size(979, 333)
        Me._tabRI_TabPage1.TabIndex = 1
        Me._tabRI_TabPage1.Text = "RI Model Summary"
        '
        'uctSummary
        '
        Me.uctSummary.Dock = System.Windows.Forms.DockStyle.Fill
        Me.uctSummary.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctSummary.Location = New System.Drawing.Point(0, 0)
        Me.uctSummary.Name = "uctSummary"
        Me.uctSummary.Size = New System.Drawing.Size(979, 333)
        Me.uctSummary.TabIndex = 8
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(823, 399)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(80, 22)
        Me.cmdOK.TabIndex = 0
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
        Me.cmdCancel.Location = New System.Drawing.Point(909, 399)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(80, 22)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'lblReinsurance_version
        '
        Me.lblReinsurance_version.AutoSize = True
        Me.lblReinsurance_version.BackColor = System.Drawing.SystemColors.Control
        Me.lblReinsurance_version.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReinsurance_version.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReinsurance_version.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReinsurance_version.Location = New System.Drawing.Point(816, 13)
        Me.lblReinsurance_version.Name = "lblReinsurance_version"
        Me.lblReinsurance_version.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReinsurance_version.Size = New System.Drawing.Size(108, 13)
        Me.lblReinsurance_version.TabIndex = 14
        Me.lblReinsurance_version.Text = "Reinsurance Version:"
        '
        'lblRIBand
        '
        Me.lblRIBand.AutoSize = True
        Me.lblRIBand.BackColor = System.Drawing.SystemColors.Control
        Me.lblRIBand.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRIBand.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRIBand.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRIBand.Location = New System.Drawing.Point(14, 13)
        Me.lblRIBand.Name = "lblRIBand"
        Me.lblRIBand.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRIBand.Size = New System.Drawing.Size(98, 13)
        Me.lblRIBand.TabIndex = 4
        Me.lblRIBand.Text = "Reinsurance Band:"
        '
        'frmInterfaceRI2007
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(996, 427)
        Me.Controls.Add(Me.txtRI_version)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.cmdEdit)
        Me.Controls.Add(Me.cmdView)
        Me.Controls.Add(Me.cmdAddFACXOL)
        Me.Controls.Add(Me.cmdAddXOLTreaty)
        Me.Controls.Add(Me.cmdAddFAC)
        Me.Controls.Add(Me.cmdAddTreaty)
        Me.Controls.Add(Me.cboRIBand)
        Me.Controls.Add(Me.tabRI)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.lblReinsurance_version)
        Me.Controls.Add(Me.lblRIBand)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(146, 23)
        Me.Name = "frmInterfaceRI2007"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Claims Reinsurance"
        Me.tabRI.ResumeLayout(False)
        Me._tabRI_TabPage0.ResumeLayout(False)
        Me._tabRI_TabPage1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class