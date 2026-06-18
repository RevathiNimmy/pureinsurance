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
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdView As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdAddTreatyXOL As System.Windows.Forms.Button
	Public WithEvents cmdAddFacXOL As System.Windows.Forms.Button
	Public WithEvents cmdAddFAC As System.Windows.Forms.Button
	Public WithEvents cmdAddTreaty As System.Windows.Forms.Button
	Public WithEvents cboRIBand As System.Windows.Forms.ComboBox
	Public WithEvents uctRI As cSIRRIControls.uctRiskRIControlRI2007
	Private WithEvents _tabRI_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents uctORI As cSIRRIControls.uctRiskRIControlRI2007
	Private WithEvents _tabRI_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents uctSummary As cSIRRIControls.uctRIModelControl
	Private WithEvents _tabRI_TabPage2 As System.Windows.Forms.TabPage
	Public WithEvents tabRI As System.Windows.Forms.TabControl
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents lblRIBand As System.Windows.Forms.Label
    Public WithEvents cboRIVersionType As System.Windows.Forms.ComboBox
    Public WithEvents txtEffectiveDate As System.Windows.Forms.TextBox
	Dim Private tabRIPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterfaceRI2007))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdView = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdAddTreatyXOL = New System.Windows.Forms.Button
        Me.cmdAddFacXOL = New System.Windows.Forms.Button
        Me.cmdAddFAC = New System.Windows.Forms.Button
        Me.cmdAddTreaty = New System.Windows.Forms.Button
        Me.cboRIBand = New System.Windows.Forms.ComboBox
        Me.tabRI = New System.Windows.Forms.TabControl
        Me._tabRI_TabPage0 = New System.Windows.Forms.TabPage
        Me.uctRI = New cSIRRIControls.uctRiskRIControlRI2007
        Me._tabRI_TabPage1 = New System.Windows.Forms.TabPage
        Me.uctORI = New cSIRRIControls.uctRiskRIControlRI2007
        Me._tabRI_TabPage2 = New System.Windows.Forms.TabPage
        Me.uctSummary = New cSIRRIControls.uctRIModelControl
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.lblRIBand = New System.Windows.Forms.Label
        Me.cboRIVersionType = New System.Windows.Forms.ComboBox
        Me.txtEffectiveDate = New System.Windows.Forms.TextBox
        Me.tabRI.SuspendLayout()
        Me._tabRI_TabPage0.SuspendLayout()
        Me._tabRI_TabPage1.SuspendLayout()
        Me._tabRI_TabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Enabled = False
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(604, 392)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(86, 22)
        Me.cmdDelete.TabIndex = 12
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdView
        '
        Me.cmdView.BackColor = System.Drawing.SystemColors.Control
        Me.cmdView.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdView.Enabled = False
        Me.cmdView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdView.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdView.Location = New System.Drawing.Point(428, 392)
        Me.cmdView.Name = "cmdView"
        Me.cmdView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdView.Size = New System.Drawing.Size(86, 22)
        Me.cmdView.TabIndex = 11
        Me.cmdView.Text = "&View"
        Me.cmdView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdView.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Enabled = False
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(516, 392)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(86, 22)
        Me.cmdEdit.TabIndex = 10
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdAddTreatyXOL
        '
        Me.cmdAddTreatyXOL.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddTreatyXOL.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddTreatyXOL.Enabled = False
        Me.cmdAddTreatyXOL.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddTreatyXOL.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddTreatyXOL.Location = New System.Drawing.Point(220, 392)
        Me.cmdAddTreatyXOL.Name = "cmdAddTreatyXOL"
        Me.cmdAddTreatyXOL.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddTreatyXOL.Size = New System.Drawing.Size(102, 22)
        Me.cmdAddTreatyXOL.TabIndex = 9
        Me.cmdAddTreatyXOL.Text = "Add &XOL Treaty"
        Me.cmdAddTreatyXOL.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddTreatyXOL.UseVisualStyleBackColor = False
        '
        'cmdAddFacXOL
        '
        Me.cmdAddFacXOL.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddFacXOL.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddFacXOL.Enabled = False
        Me.cmdAddFacXOL.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddFacXOL.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddFacXOL.Location = New System.Drawing.Point(324, 392)
        Me.cmdAddFacXOL.Name = "cmdAddFacXOL"
        Me.cmdAddFacXOL.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddFacXOL.Size = New System.Drawing.Size(102, 22)
        Me.cmdAddFacXOL.TabIndex = 8
        Me.cmdAddFacXOL.Text = "Add FAC XO&L"
        Me.cmdAddFacXOL.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddFacXOL.UseVisualStyleBackColor = False
        '
        'cmdAddFAC
        '
        Me.cmdAddFAC.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddFAC.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddFAC.Enabled = False
        Me.cmdAddFAC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddFAC.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddFAC.Location = New System.Drawing.Point(116, 392)
        Me.cmdAddFAC.Name = "cmdAddFAC"
        Me.cmdAddFAC.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddFAC.Size = New System.Drawing.Size(102, 22)
        Me.cmdAddFAC.TabIndex = 7
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
        Me.cmdAddTreaty.Location = New System.Drawing.Point(8, 392)
        Me.cmdAddTreaty.Name = "cmdAddTreaty"
        Me.cmdAddTreaty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddTreaty.Size = New System.Drawing.Size(106, 22)
        Me.cmdAddTreaty.TabIndex = 6
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
        Me.cboRIBand.TabIndex = 1
        '
        'tabRI
        '
        Me.tabRI.Controls.Add(Me._tabRI_TabPage0)
        Me.tabRI.Controls.Add(Me._tabRI_TabPage1)
        Me.tabRI.Controls.Add(Me._tabRI_TabPage2)
        Me.tabRI.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabRI.ItemSize = New System.Drawing.Size(324, 18)
        Me.tabRI.Location = New System.Drawing.Point(6, 38)
        Me.tabRI.Multiline = True
        Me.tabRI.Name = "tabRI"
        Me.tabRI.SelectedIndex = 0
        Me.tabRI.Size = New System.Drawing.Size(981, 351)
        Me.tabRI.TabIndex = 2
        '
        '_tabRI_TabPage0
        '
        Me._tabRI_TabPage0.Controls.Add(Me.uctRI)
        Me._tabRI_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabRI_TabPage0.Name = "_tabRI_TabPage0"
        Me._tabRI_TabPage0.Size = New System.Drawing.Size(973, 325)
        Me._tabRI_TabPage0.TabIndex = 0
        Me._tabRI_TabPage0.Text = "Reinsurance"
        '
        'uctRI
        '
        Me.uctRI.DeletedRIArragementIds = Nothing
        Me.uctRI.ExistingLimits = Nothing
        Me.uctRI.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctRI.IsDirty = False
        Me.uctRI.Location = New System.Drawing.Point(8, 12)
        Me.uctRI.Name = "uctRI"
        Me.uctRI.ReadOnly_Renamed = False
        Me.uctRI.RowCountFAC = 0
        Me.uctRI.RowCountRetained = 0
        Me.uctRI.RowCountTreaty = 0
        Me.uctRI.SelectedRIType = ""
        Me.uctRI.SelectedRow = 0
        Me.uctRI.SelRIArrangementLine = 0
        Me.uctRI.Size = New System.Drawing.Size(957, 307)
        Me.uctRI.TabIndex = 14
        '
        '_tabRI_TabPage1
        '
        Me._tabRI_TabPage1.Controls.Add(Me.uctORI)
        Me._tabRI_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabRI_TabPage1.Name = "_tabRI_TabPage1"
        Me._tabRI_TabPage1.Size = New System.Drawing.Size(973, 325)
        Me._tabRI_TabPage1.TabIndex = 1
        Me._tabRI_TabPage1.Text = "Original Reinsurance"
        '
        'uctORI
        '
        Me.uctORI.DeletedRIArragementIds = Nothing
        Me.uctORI.ExistingLimits = Nothing
        Me.uctORI.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctORI.IsDirty = False
        Me.uctORI.Location = New System.Drawing.Point(4, 4)
        Me.uctORI.Name = "uctORI"
        Me.uctORI.ReadOnly_Renamed = True
        Me.uctORI.RowCountFAC = 0
        Me.uctORI.RowCountRetained = 0
        Me.uctORI.RowCountTreaty = 0
        Me.uctORI.SelectedRIType = ""
        Me.uctORI.SelectedRow = 0
        Me.uctORI.SelRIArrangementLine = 0
        Me.uctORI.Size = New System.Drawing.Size(969, 319)
        Me.uctORI.TabIndex = 13
        '
        '_tabRI_TabPage2
        '
        Me._tabRI_TabPage2.Controls.Add(Me.uctSummary)
        Me._tabRI_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabRI_TabPage2.Name = "_tabRI_TabPage2"
        Me._tabRI_TabPage2.Size = New System.Drawing.Size(973, 325)
        Me._tabRI_TabPage2.TabIndex = 2
        Me._tabRI_TabPage2.Text = "RI Model Summary"
        '
        'uctSummary
        '
        Me.uctSummary.FilterType = 0
        Me.uctSummary.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctSummary.Location = New System.Drawing.Point(6, 6)
        Me.uctSummary.Name = "uctSummary"
        Me.uctSummary.RIArrangementID = 0
        Me.uctSummary.Size = New System.Drawing.Size(967, 315)
        Me.uctSummary.TabIndex = 3
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(817, 392)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(80, 22)
        Me.cmdOK.TabIndex = 4
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(903, 392)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(80, 22)
        Me.cmdCancel.TabIndex = 5
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
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
        Me.lblRIBand.TabIndex = 0
        Me.lblRIBand.Text = "Reinsurance Band:"
        '
        'cboRIVersionType
        '
        Me.cboRIVersionType.BackColor = System.Drawing.SystemColors.Window
        Me.cboRIVersionType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboRIVersionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRIVersionType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRIVersionType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboRIVersionType.Location = New System.Drawing.Point(516, 9)
        Me.cboRIVersionType.Name = "cboRIVersionType"
        Me.cboRIVersionType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboRIVersionType.Size = New System.Drawing.Size(240, 21)
        Me.cboRIVersionType.TabIndex = 13
        '
        'txtEffectiveDate
        '
        Me.txtEffectiveDate.Location = New System.Drawing.Point(786, 9)
        Me.txtEffectiveDate.Name = "txtEffectiveDate"
        Me.txtEffectiveDate.Size = New System.Drawing.Size(170, 21)
        Me.txtEffectiveDate.TabIndex = 14
        '
        'frmInterfaceRI2007
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(987, 419)
        Me.Controls.Add(Me.txtEffectiveDate)
        Me.Controls.Add(Me.cboRIVersionType)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.cmdView)
        Me.Controls.Add(Me.cmdEdit)
        Me.Controls.Add(Me.cmdAddTreatyXOL)
        Me.Controls.Add(Me.cmdAddFacXOL)
        Me.Controls.Add(Me.cmdAddFAC)
        Me.Controls.Add(Me.cmdAddTreaty)
        Me.Controls.Add(Me.cboRIBand)
        Me.Controls.Add(Me.tabRI)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.lblRIBand)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(146, 23)
        Me.Name = "frmInterfaceRI2007"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Risk Reinsurance"
        Me.tabRI.ResumeLayout(False)
        Me._tabRI_TabPage0.ResumeLayout(False)
        Me._tabRI_TabPage1.ResumeLayout(False)
        Me._tabRI_TabPage2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    
#End Region 
End Class