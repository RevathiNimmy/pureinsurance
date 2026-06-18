<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSelectGISScreen
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
	Public WithEvents lblRiskType As System.Windows.Forms.Label
	Public WithEvents lblGISSCreen As System.Windows.Forms.Label
	Public WithEvents txtRiskType As System.Windows.Forms.TextBox
	Public WithEvents lstScreens As System.Windows.Forms.ComboBox
	Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents SSTab1 As System.Windows.Forms.TabControl
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblRiskType = New System.Windows.Forms.Label
        Me.lblGISSCreen = New System.Windows.Forms.Label
        Me.txtRiskType = New System.Windows.Forms.TextBox
        Me.lstScreens = New System.Windows.Forms.ComboBox
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me.SuspendLayout()
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(140, 18)
        Me.SSTab1.Location = New System.Drawing.Point(8, 6)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(427, 105)
        Me.SSTab1.TabIndex = 0
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.lblRiskType)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblGISSCreen)
        Me._SSTab1_TabPage0.Controls.Add(Me.txtRiskType)
        Me._SSTab1_TabPage0.Controls.Add(Me.lstScreens)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(419, 79)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "1 - Gis Screen"
        '
        'lblRiskType
        '
        Me.lblRiskType.BackColor = System.Drawing.SystemColors.Control
        Me.lblRiskType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRiskType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRiskType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRiskType.Location = New System.Drawing.Point(56, 20)
        Me.lblRiskType.Name = "lblRiskType"
        Me.lblRiskType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRiskType.Size = New System.Drawing.Size(75, 17)
        Me.lblRiskType.TabIndex = 0
        Me.lblRiskType.Text = "Risk Type"
        Me.lblRiskType.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblGISSCreen
        '
        Me.lblGISSCreen.BackColor = System.Drawing.SystemColors.Control
        Me.lblGISSCreen.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGISSCreen.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGISSCreen.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGISSCreen.Location = New System.Drawing.Point(16, 46)
        Me.lblGISSCreen.Name = "lblGISSCreen"
        Me.lblGISSCreen.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGISSCreen.Size = New System.Drawing.Size(115, 17)
        Me.lblGISSCreen.TabIndex = 2
        Me.lblGISSCreen.Text = "Claims GIS Screen"
        Me.lblGISSCreen.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtRiskType
        '
        Me.txtRiskType.AcceptsReturn = True
        Me.txtRiskType.BackColor = System.Drawing.SystemColors.Window
        Me.txtRiskType.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRiskType.Enabled = False
        Me.txtRiskType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRiskType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRiskType.Location = New System.Drawing.Point(142, 18)
        Me.txtRiskType.MaxLength = 0
        Me.txtRiskType.Name = "txtRiskType"
        Me.txtRiskType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRiskType.Size = New System.Drawing.Size(265, 21)
        Me.txtRiskType.TabIndex = 1
        '
        'lstScreens
        '
        Me.lstScreens.BackColor = System.Drawing.SystemColors.Window
        Me.lstScreens.Cursor = System.Windows.Forms.Cursors.Default
        Me.lstScreens.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstScreens.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lstScreens.Location = New System.Drawing.Point(142, 42)
        Me.lstScreens.Name = "lstScreens"
        Me.lstScreens.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lstScreens.Size = New System.Drawing.Size(265, 21)
        Me.lstScreens.TabIndex = 3
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(358, 112)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Enabled = False
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(278, 112)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 1
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'frmSelectGISScreen
        '
        Me.AcceptButton = Me.cmdCancel
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(435, 140)
        Me.Controls.Add(Me.SSTab1)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSelectGISScreen"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Select GIS Screen"
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me._SSTab1_TabPage0.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class