<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBackdatedMTA
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
	Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
		Dispose(True)
	End Sub
	Dim fTerminateCalled_Form_Terminate_Renamed As Boolean
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
	 Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not fTerminateCalled_Form_Terminate_Renamed Then
				fTerminateCalled_Form_Terminate_Renamed = True
				Form_Terminate_Renamed()
			End If
			If Not (components Is Nothing) Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents txtPremiumTotal As System.Windows.Forms.TextBox
	Public WithEvents lvwBackdatePolicies As System.Windows.Forms.ListView
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents lblPremiumTotal As System.Windows.Forms.Label

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtPremiumTotal = New System.Windows.Forms.TextBox
        Me.lvwBackdatePolicies = New System.Windows.Forms.ListView
        Me.cmdOk = New System.Windows.Forms.Button
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.lblPremiumTotal = New System.Windows.Forms.Label
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdView = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtCommTotal = New System.Windows.Forms.TextBox
        Me.txtFeeTotal = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'txtPremiumTotal
        '
        Me.txtPremiumTotal.AcceptsReturn = True
        Me.txtPremiumTotal.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtPremiumTotal.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPremiumTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPremiumTotal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPremiumTotal.Location = New System.Drawing.Point(135, 234)
        Me.txtPremiumTotal.MaxLength = 0
        Me.txtPremiumTotal.Name = "txtPremiumTotal"
        Me.txtPremiumTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPremiumTotal.Size = New System.Drawing.Size(93, 20)
        Me.txtPremiumTotal.TabIndex = 3
        Me.txtPremiumTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lvwBackdatePolicies
        '
        Me.lvwBackdatePolicies.BackColor = System.Drawing.SystemColors.Window
        Me.lvwBackdatePolicies.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwBackdatePolicies.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwBackdatePolicies.FullRowSelect = True
        Me.lvwBackdatePolicies.Location = New System.Drawing.Point(16, 24)
        Me.lvwBackdatePolicies.Name = "lvwBackdatePolicies"
        Me.lvwBackdatePolicies.Size = New System.Drawing.Size(894, 202)
        Me.lvwBackdatePolicies.TabIndex = 2
        Me.lvwBackdatePolicies.UseCompatibleStateImageBehavior = False
        Me.lvwBackdatePolicies.View = System.Windows.Forms.View.Details
        '
        'cmdOk
        '
        Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOk.Location = New System.Drawing.Point(848, 232)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(66, 25)
        Me.cmdOk.TabIndex = 1
        Me.cmdOk.Text = "&OK"
        Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOk.UseVisualStyleBackColor = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(8, 8)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(908, 218)
        Me.Frame1.TabIndex = 0
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Affected Versions"
        '
        'lblPremiumTotal
        '
        Me.lblPremiumTotal.BackColor = System.Drawing.SystemColors.Control
        Me.lblPremiumTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPremiumTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPremiumTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPremiumTotal.Location = New System.Drawing.Point(5, 234)
        Me.lblPremiumTotal.Name = "lblPremiumTotal"
        Me.lblPremiumTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPremiumTotal.Size = New System.Drawing.Size(133, 17)
        Me.lblPremiumTotal.TabIndex = 4
        Me.lblPremiumTotal.Text = "Total Premium Adjustment"
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(776, 232)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(67, 25)
        Me.cmdEdit.TabIndex = 5
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdView
        '
        Me.cmdView.Location = New System.Drawing.Point(704, 232)
        Me.cmdView.Name = "cmdView"
        Me.cmdView.Size = New System.Drawing.Size(67, 25)
        Me.cmdView.TabIndex = 6
        Me.cmdView.Text = "&View"
        Me.cmdView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdView.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(233, 237)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(133, 13)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Total Commission Adjusted"
        '
        'txtCommTotal
        '
        Me.txtCommTotal.AcceptsReturn = True
        Me.txtCommTotal.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtCommTotal.Location = New System.Drawing.Point(367, 234)
        Me.txtCommTotal.Name = "txtCommTotal"
        Me.txtCommTotal.Size = New System.Drawing.Size(100, 20)
        Me.txtCommTotal.TabIndex = 8
        '
        'txtFeeTotal
        '
        Me.txtFeeTotal.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtFeeTotal.Location = New System.Drawing.Point(576, 235)
        Me.txtFeeTotal.Name = "txtFeeTotal"
        Me.txtFeeTotal.Size = New System.Drawing.Size(100, 20)
        Me.txtFeeTotal.TabIndex = 9
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(477, 238)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(96, 13)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Total Fee Adjusted"
        '
        'frmBackdatedMTA
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(928, 286)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtFeeTotal)
        Me.Controls.Add(Me.txtCommTotal)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmdView)
        Me.Controls.Add(Me.cmdEdit)
        Me.Controls.Add(Me.txtPremiumTotal)
        Me.Controls.Add(Me.lvwBackdatePolicies)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.lblPremiumTotal)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.MaximizeBox = False
        Me.Name = "frmBackdatedMTA"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Backdated MTA"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub
    Public WithEvents cmdEdit As System.Windows.Forms.Button
    Public WithEvents cmdView As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtCommTotal As System.Windows.Forms.TextBox
    Friend WithEvents txtFeeTotal As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
#End Region 
End Class