<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctCLMListReceiptsC
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		UserControl_InitProperties()
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
	Friend WithEvents uctAnchor As uSIRCommonControls.uctAnchor
	Friend WithEvents cmdViewReceipts As System.Windows.Forms.Button
	Friend WithEvents lvwReceipts As System.Windows.Forms.ListView
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctCLMListReceiptsC))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.uctAnchor = New uSIRCommonControls.uctAnchor
		Me.cmdViewReceipts = New System.Windows.Forms.Button
		Me.lvwReceipts = New System.Windows.Forms.ListView
		Me.SuspendLayout()
        ' 
		' uctAnchor
		' 
		Me.uctAnchor.Location = New System.Drawing.Point(128, 248)
		Me.uctAnchor.Name = "uctAnchor"
		' 
		' cmdViewReceipts
		' 
		Me.cmdViewReceipts.BackColor = System.Drawing.SystemColors.Control
		Me.cmdViewReceipts.CausesValidation = True
		Me.cmdViewReceipts.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdViewReceipts.Enabled = True
		Me.cmdViewReceipts.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdViewReceipts.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdViewReceipts.Location = New System.Drawing.Point(552, 256)
		Me.cmdViewReceipts.Name = "cmdViewReceipts"
		Me.cmdViewReceipts.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdViewReceipts.Size = New System.Drawing.Size(100, 21)
		Me.cmdViewReceipts.TabIndex = 0
		Me.cmdViewReceipts.TabStop = True
		Me.cmdViewReceipts.Text = "&View Receipt"
		Me.cmdViewReceipts.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdViewReceipts.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lvwReceipts
		' 
		Me.lvwReceipts.BackColor = System.Drawing.SystemColors.Window
		Me.lvwReceipts.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lvwReceipts.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwReceipts.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwReceipts.FullRowSelect = True
		Me.lvwReceipts.GridLines = True
		Me.lvwReceipts.HideSelection = True
		Me.lvwReceipts.LabelEdit = False
		Me.lvwReceipts.LabelWrap = True
		Me.lvwReceipts.Location = New System.Drawing.Point(8, 8)
		Me.lvwReceipts.Name = "lvwReceipts"
		Me.lvwReceipts.Size = New System.Drawing.Size(711, 247)
		Me.lvwReceipts.TabIndex = 1
		Me.lvwReceipts.View = System.Windows.Forms.View.Details
		' 
		' uctCLMListReceiptsC
		' 
		Me.ClientSize = New System.Drawing.Size(728, 279)
		Me.Controls.Add(Me.uctAnchor)
		Me.Controls.Add(Me.cmdViewReceipts)
		Me.Controls.Add(Me.lvwReceipts)
		MyBase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		MyBase.Location = New System.Drawing.Point(0, 0)
		MyBase.Name = "uctCLMListReceiptsC"
        Me.ResumeLayout(False)
	End Sub
#End Region 
End Class