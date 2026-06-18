<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctClaimReserve
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		UserControl_Initialize()
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
	Friend WithEvents cmdEdit As System.Windows.Forms.Button
	Friend WithEvents lvwReserve As System.Windows.Forms.ListView
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctClaimReserve))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdEdit = New System.Windows.Forms.Button
		Me.lvwReserve = New System.Windows.Forms.ListView
		Me.SuspendLayout()
		' 
		' cmdEdit
		' 
		Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdEdit.CausesValidation = True
		Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdEdit.Enabled = True
		Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdEdit.Location = New System.Drawing.Point(446, 6)
		Me.cmdEdit.Name = "cmdEdit"
		Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdEdit.Size = New System.Drawing.Size(73, 23)
		Me.cmdEdit.TabIndex = 1
		Me.cmdEdit.TabStop = True
		Me.cmdEdit.Text = "Edit"
		Me.cmdEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lvwReserve
		' 
		Me.lvwReserve.BackColor = System.Drawing.SystemColors.Window
		Me.lvwReserve.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwReserve.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwReserve.HideSelection = True
		Me.lvwReserve.LabelEdit = False
		Me.lvwReserve.LabelWrap = True
		Me.lvwReserve.Location = New System.Drawing.Point(3, 2)
		Me.lvwReserve.Name = "lvwReserve"
		Me.lvwReserve.Size = New System.Drawing.Size(426, 216)
		Me.lvwReserve.TabIndex = 0
		Me.lvwReserve.View = System.Windows.Forms.View.Details
		' 
		' uctClaimReserve
		' 
		Me.ClientSize = New System.Drawing.Size(530, 256)
		Me.Controls.Add(Me.cmdEdit)
		Me.Controls.Add(Me.lvwReserve)
		MyBase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		MyBase.Location = New System.Drawing.Point(0, 0)
		MyBase.Name = "uctClaimReserve"
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class