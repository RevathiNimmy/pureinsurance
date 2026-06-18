<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctCLMListPaymentsC
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
	Friend WithEvents cmdViewPayment As System.Windows.Forms.Button
    Friend WithEvents lvwPayments As System.Windows.Forms.ListView
    'TODOLIST-Commented the listviewhelper as it was conflicting with icon displai in listview)
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.uctAnchor = New uSIRCommonControls.uctAnchor
        Me.cmdViewPayment = New System.Windows.Forms.Button
        Me.lvwPayments = New System.Windows.Forms.ListView
        Me.SuspendLayout()
        '
        'uctAnchor
        '
        Me.uctAnchor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctAnchor.Location = New System.Drawing.Point(154, 260)
        Me.uctAnchor.Name = "uctAnchor"
        Me.uctAnchor.Size = New System.Drawing.Size(44, 16)
        Me.uctAnchor.TabIndex = 0
        Me.uctAnchor.Visible = False
        '
        'cmdViewPayment
        '
        Me.cmdViewPayment.BackColor = System.Drawing.SystemColors.Control
        Me.cmdViewPayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdViewPayment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdViewPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdViewPayment.Location = New System.Drawing.Point(22, 256)
        Me.cmdViewPayment.Name = "cmdViewPayment"
        Me.cmdViewPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdViewPayment.Size = New System.Drawing.Size(104, 21)
        Me.cmdViewPayment.TabIndex = 0
        Me.cmdViewPayment.Text = "&View Payment"
        Me.cmdViewPayment.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdViewPayment.UseVisualStyleBackColor = False
        '
        'lvwPayments
        '
        Me.lvwPayments.BackColor = System.Drawing.SystemColors.Window
        Me.lvwPayments.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwPayments.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwPayments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwPayments.FullRowSelect = True
        Me.lvwPayments.GridLines = True
        Me.lvwPayments.Location = New System.Drawing.Point(8, 8)
        Me.lvwPayments.Name = "lvwPayments"
        Me.lvwPayments.Size = New System.Drawing.Size(711, 242)
        Me.lvwPayments.TabIndex = 1
        Me.lvwPayments.UseCompatibleStateImageBehavior = False
        Me.lvwPayments.View = System.Windows.Forms.View.Details
        '
        'uctCLMListPaymentsC
        '
        Me.Controls.Add(Me.uctAnchor)
        Me.Controls.Add(Me.cmdViewPayment)
        Me.Controls.Add(Me.lvwPayments)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctCLMListPaymentsC"
        Me.Size = New System.Drawing.Size(728, 279)
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class