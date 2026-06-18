<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctCLMPayment
#Region "Windows Form Designer generated code "
	Friend Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		UserControl_InitProperties()
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Friend WithEvents cmdHistory As System.Windows.Forms.Button
	Friend WithEvents cmdEdit As System.Windows.Forms.Button
	Friend WithEvents lstviewPayment As System.Windows.Forms.ListView
	Friend WithEvents fraPaymentDetails As System.Windows.Forms.Panel
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctCLMPayment))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdHistory = New System.Windows.Forms.Button
		Me.fraPaymentDetails = New System.Windows.Forms.Panel
		Me.cmdEdit = New System.Windows.Forms.Button
		Me.lstviewPayment = New System.Windows.Forms.ListView
		Me.fraPaymentDetails.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdHistory
		' 
		Me.cmdHistory.BackColor = System.Drawing.SystemColors.Control
		Me.cmdHistory.CausesValidation = True
		Me.cmdHistory.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdHistory.Enabled = False
		Me.cmdHistory.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdHistory.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdHistory.Location = New System.Drawing.Point(656, 36)
		Me.cmdHistory.Name = "cmdHistory"
		Me.cmdHistory.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHistory.Size = New System.Drawing.Size(73, 22)
		Me.cmdHistory.TabIndex = 3
		Me.cmdHistory.TabStop = True
		Me.cmdHistory.Text = "History"
		Me.cmdHistory.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdHistory.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' fraPaymentDetails
		' 
		Me.fraPaymentDetails.BackColor = System.Drawing.SystemColors.Control
		Me.fraPaymentDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.fraPaymentDetails.Controls.Add(Me.cmdEdit)
		Me.fraPaymentDetails.Controls.Add(Me.lstviewPayment)
		Me.fraPaymentDetails.Cursor = System.Windows.Forms.Cursors.Default
		Me.fraPaymentDetails.Enabled = True
		Me.fraPaymentDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraPaymentDetails.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraPaymentDetails.Location = New System.Drawing.Point(0, 0)
		Me.fraPaymentDetails.Name = "fraPaymentDetails"
		Me.fraPaymentDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraPaymentDetails.Size = New System.Drawing.Size(737, 377)
		Me.fraPaymentDetails.TabIndex = 0
		Me.fraPaymentDetails.Text = "Payment"
		Me.fraPaymentDetails.Visible = True
		' 
		' cmdEdit
		' 
		Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdEdit.CausesValidation = True
		Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdEdit.Enabled = False
		Me.cmdEdit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdEdit.Location = New System.Drawing.Point(656, 8)
		Me.cmdEdit.Name = "cmdEdit"
		Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
		Me.cmdEdit.TabIndex = 2
		Me.cmdEdit.TabStop = True
		Me.cmdEdit.Text = "{Edit}"
		Me.cmdEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.cmdEdit.Visible = False
		' 
		' lstviewPayment
		' 
		Me.lstviewPayment.BackColor = System.Drawing.SystemColors.Window
		Me.lstviewPayment.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lstviewPayment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lstviewPayment.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lstviewPayment.HideSelection = True
		Me.lstviewPayment.LabelEdit = False
		Me.lstviewPayment.LabelWrap = True
		Me.lstviewPayment.Location = New System.Drawing.Point(8, 8)
		Me.lstviewPayment.Name = "lstviewPayment"
		Me.lstviewPayment.Size = New System.Drawing.Size(641, 369)
		Me.lstviewPayment.TabIndex = 1
		Me.lstviewPayment.View = System.Windows.Forms.View.Details
		' 
		' uctCLMPayment
		' 
		Me.ClientSize = New System.Drawing.Size(740, 384)
		Me.Controls.Add(Me.cmdHistory)
		Me.Controls.Add(Me.fraPaymentDetails)
		MyBase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		MyBase.Location = New System.Drawing.Point(0, 0)
		MyBase.Name = "uctCLMPayment"
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lstviewpayment, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.fraPaymentDetails.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
#Region "Upgrade Support"
	<System.Runtime.InteropServices.ProgId("DataHasChangedEventArgs_NET.DataHasChangedEventArgs")> _
	Public NotInheritable Class DataHasChangedEventArgs
		Inherits System.EventArgs
        Public NewData(,) As Object
        Friend Sub New(ByRef NewData(,) As Object)
            MyBase.New()
            Me.NewData = NewData
        End Sub
	End Class
#End Region 
End Class