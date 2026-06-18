<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
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
	Public WithEvents cmdAddFAC As System.Windows.Forms.Button
	Public WithEvents cmdAddTreaty As System.Windows.Forms.Button
	Public WithEvents cboRIBand As System.Windows.Forms.ComboBox
    Public WithEvents uctRI As cSIRRIControls.uctRiskRIControl
	Private WithEvents _tabRI_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents uctORI As cSIRRIControls.uctRiskRIControl
	Private WithEvents _tabRI_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents uctSummary As cSIRRIControls.uctRIModelControl
	Private WithEvents _tabRI_TabPage2 As System.Windows.Forms.TabPage
	Public WithEvents tabRI As System.Windows.Forms.TabControl
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents lblRIBand As System.Windows.Forms.Label
	Dim Private tabRIPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdAddFAC = New System.Windows.Forms.Button
        Me.cmdAddTreaty = New System.Windows.Forms.Button
        Me.cboRIBand = New System.Windows.Forms.ComboBox
        Me.tabRI = New System.Windows.Forms.TabControl
        Me._tabRI_TabPage0 = New System.Windows.Forms.TabPage
        Me.uctRI = New cSIRRIControls.uctRiskRIControl
        Me._tabRI_TabPage1 = New System.Windows.Forms.TabPage
        Me.uctORI = New cSIRRIControls.uctRiskRIControl
        Me._tabRI_TabPage2 = New System.Windows.Forms.TabPage
        Me.uctSummary = New cSIRRIControls.uctRIModelControl
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.lblRIBand = New System.Windows.Forms.Label
        Me.tabRI.SuspendLayout()
        Me._tabRI_TabPage0.SuspendLayout()
        Me._tabRI_TabPage1.SuspendLayout()
        Me._tabRI_TabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdAddFAC
        '
        Me.cmdAddFAC.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddFAC.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddFAC.Enabled = False
        Me.cmdAddFAC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddFAC.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddFAC.Location = New System.Drawing.Point(92, 391)
        Me.cmdAddFAC.Name = "cmdAddFAC"
        Me.cmdAddFAC.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddFAC.Size = New System.Drawing.Size(80, 22)
        Me.cmdAddFAC.TabIndex = 7
        Me.cmdAddFAC.Text = "Add &FAC"
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
        Me.cmdAddTreaty.Location = New System.Drawing.Point(6, 391)
        Me.cmdAddTreaty.Name = "cmdAddTreaty"
        Me.cmdAddTreaty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddTreaty.Size = New System.Drawing.Size(80, 22)
        Me.cmdAddTreaty.TabIndex = 6
        Me.cmdAddTreaty.Text = "Add &Treaty"
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
        Me.tabRI.ItemSize = New System.Drawing.Size(271, 18)
        Me.tabRI.Location = New System.Drawing.Point(6, 38)
        Me.tabRI.Multiline = True
        Me.tabRI.Name = "tabRI"
        Me.tabRI.SelectedIndex = 0
        Me.tabRI.Size = New System.Drawing.Size(821, 351)
        Me.tabRI.TabIndex = 2
        '
        '_tabRI_TabPage0
        '
        Me._tabRI_TabPage0.Controls.Add(Me.uctRI)
        Me._tabRI_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabRI_TabPage0.Name = "_tabRI_TabPage0"
        Me._tabRI_TabPage0.Size = New System.Drawing.Size(813, 325)
        Me._tabRI_TabPage0.TabIndex = 0
        Me._tabRI_TabPage0.Text = "Reinsurance"
        '
        'uctRI
        '
        Me.uctRI.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctRI.Location = New System.Drawing.Point(6, 6)
        Me.uctRI.Name = "uctRI"
        Me.uctRI.ReadOnly_Renamed = False
        Me.uctRI.Size = New System.Drawing.Size(805, 315)
        Me.uctRI.TabIndex = 3
        '
        '_tabRI_TabPage1
        '
        Me._tabRI_TabPage1.Controls.Add(Me.uctORI)
        Me._tabRI_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabRI_TabPage1.Name = "_tabRI_TabPage1"
        Me._tabRI_TabPage1.Size = New System.Drawing.Size(813, 325)
        Me._tabRI_TabPage1.TabIndex = 1
        Me._tabRI_TabPage1.Text = "Original Reinsurance"
        '
        'uctORI
        '
        Me.uctORI.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctORI.Location = New System.Drawing.Point(6, 6)
        Me.uctORI.Name = "uctORI"
        Me.uctORI.ReadOnly_Renamed = True
        Me.uctORI.Size = New System.Drawing.Size(805, 315)
        Me.uctORI.TabIndex = 4
        '
        '_tabRI_TabPage2
        '
        Me._tabRI_TabPage2.Controls.Add(Me.uctSummary)
        Me._tabRI_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabRI_TabPage2.Name = "_tabRI_TabPage2"
        Me._tabRI_TabPage2.Size = New System.Drawing.Size(813, 325)
        Me._tabRI_TabPage2.TabIndex = 2
        Me._tabRI_TabPage2.Text = "RI Model Summary"
        '
        'uctSummary
        '
        Me.uctSummary.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctSummary.Location = New System.Drawing.Point(6, 6)
        Me.uctSummary.Name = "uctSummary"
        Me.uctSummary.Size = New System.Drawing.Size(805, 315)
        Me.uctSummary.TabIndex = 5
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(657, 391)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(80, 22)
        Me.cmdOK.TabIndex = 8
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
        Me.cmdCancel.Location = New System.Drawing.Point(743, 391)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(80, 22)
        Me.cmdCancel.TabIndex = 9
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
        'frmInterface
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(829, 419)
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
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
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