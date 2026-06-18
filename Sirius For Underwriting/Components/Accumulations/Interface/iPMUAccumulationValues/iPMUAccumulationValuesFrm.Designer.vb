<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAccumulationValues
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
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
	Public WithEvents cmdSinglePolicy As System.Windows.Forms.Button
	Public WithEvents cmdAllValues As System.Windows.Forms.Button
	Public WithEvents Label1 As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAccumulationValues))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdSinglePolicy = New System.Windows.Forms.Button
		Me.cmdAllValues = New System.Windows.Forms.Button
		Me.Label1 = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' cmdSinglePolicy
		' 
		Me.cmdSinglePolicy.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSinglePolicy.CausesValidation = True
		Me.cmdSinglePolicy.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSinglePolicy.Enabled = True
		Me.cmdSinglePolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdSinglePolicy.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSinglePolicy.Location = New System.Drawing.Point(152, 64)
		Me.cmdSinglePolicy.Name = "cmdSinglePolicy"
		Me.cmdSinglePolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSinglePolicy.Size = New System.Drawing.Size(89, 25)
		Me.cmdSinglePolicy.TabIndex = 1
		Me.cmdSinglePolicy.TabStop = True
		Me.cmdSinglePolicy.Text = "Single Policy"
		Me.cmdSinglePolicy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSinglePolicy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdAllValues
		' 
		Me.cmdAllValues.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAllValues.CausesValidation = True
		Me.cmdAllValues.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAllValues.Enabled = True
		Me.cmdAllValues.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAllValues.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAllValues.Location = New System.Drawing.Point(48, 64)
		Me.cmdAllValues.Name = "cmdAllValues"
		Me.cmdAllValues.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAllValues.Size = New System.Drawing.Size(89, 25)
		Me.cmdAllValues.TabIndex = 0
		Me.cmdAllValues.TabStop = True
		Me.cmdAllValues.Text = "All Values"
		Me.cmdAllValues.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAllValues.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' Label1
		' 
		Me.Label1.AutoSize = False
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.Enabled = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Location = New System.Drawing.Point(8, 8)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(289, 33)
		Me.Label1.TabIndex = 2
		Me.Label1.Text = "Do you wish to recreate all accumulation values or just those for a single policy?"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' frmAccumulationValues
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(303, 113)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdSinglePolicy)
		Me.Controls.Add(Me.cmdAllValues)
		Me.Controls.Add(Me.Label1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmAccumulationValues"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Generate Accumulation Values"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class