<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPaymentProcessed
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
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdPayIndividually As System.Windows.Forms.Button
	Public WithEvents cmdSelectAll As System.Windows.Forms.Button
	Public WithEvents cmdPayAsOne As System.Windows.Forms.Button
	Public WithEvents cmdSettleAll As System.Windows.Forms.Button
	Private WithEvents _lvwPaymentDesc_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwPaymentDesc_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwPaymentDesc_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwPaymentDesc As System.Windows.Forms.ListView
	Public WithEvents fraUnallocatedClaimPayments As System.Windows.Forms.GroupBox
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPaymentProcessed))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdOK = New System.Windows.Forms.Button
		Me.fraUnallocatedClaimPayments = New System.Windows.Forms.GroupBox
		Me.cmdPayIndividually = New System.Windows.Forms.Button
		Me.cmdSelectAll = New System.Windows.Forms.Button
		Me.cmdPayAsOne = New System.Windows.Forms.Button
		Me.cmdSettleAll = New System.Windows.Forms.Button
		Me.lvwPaymentDesc = New System.Windows.Forms.ListView
		Me._lvwPaymentDesc_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwPaymentDesc_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvwPaymentDesc_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me.fraUnallocatedClaimPayments.SuspendLayout()
		Me.lvwPaymentDesc.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(164, 184)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(81, 25)
		Me.cmdOK.TabIndex = 6
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' fraUnallocatedClaimPayments
		' 
		Me.fraUnallocatedClaimPayments.BackColor = System.Drawing.SystemColors.Control
		Me.fraUnallocatedClaimPayments.Controls.Add(Me.cmdPayIndividually)
		Me.fraUnallocatedClaimPayments.Controls.Add(Me.cmdSelectAll)
		Me.fraUnallocatedClaimPayments.Controls.Add(Me.cmdPayAsOne)
		Me.fraUnallocatedClaimPayments.Controls.Add(Me.cmdSettleAll)
		Me.fraUnallocatedClaimPayments.Controls.Add(Me.lvwPaymentDesc)
		Me.fraUnallocatedClaimPayments.Enabled = True
		Me.fraUnallocatedClaimPayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraUnallocatedClaimPayments.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraUnallocatedClaimPayments.Location = New System.Drawing.Point(0, 0)
		Me.fraUnallocatedClaimPayments.Name = "fraUnallocatedClaimPayments"
		Me.fraUnallocatedClaimPayments.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraUnallocatedClaimPayments.Size = New System.Drawing.Size(435, 177)
		Me.fraUnallocatedClaimPayments.TabIndex = 0
		Me.fraUnallocatedClaimPayments.Visible = True
		' 
		' cmdPayIndividually
		' 
		Me.cmdPayIndividually.BackColor = System.Drawing.SystemColors.Control
		Me.cmdPayIndividually.CausesValidation = True
		Me.cmdPayIndividually.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdPayIndividually.Enabled = True
		Me.cmdPayIndividually.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdPayIndividually.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdPayIndividually.Location = New System.Drawing.Point(8, 408)
		Me.cmdPayIndividually.Name = "cmdPayIndividually"
		Me.cmdPayIndividually.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdPayIndividually.Size = New System.Drawing.Size(147, 25)
		Me.cmdPayIndividually.TabIndex = 4
		Me.cmdPayIndividually.TabStop = True
		Me.cmdPayIndividually.Text = "Make &Multiple Payments"
		Me.cmdPayIndividually.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdPayIndividually.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdSelectAll
		' 
		Me.cmdSelectAll.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSelectAll.CausesValidation = True
		Me.cmdSelectAll.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSelectAll.Enabled = True
		Me.cmdSelectAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdSelectAll.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSelectAll.Location = New System.Drawing.Point(312, 408)
		Me.cmdSelectAll.Name = "cmdSelectAll"
		Me.cmdSelectAll.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSelectAll.Size = New System.Drawing.Size(145, 25)
		Me.cmdSelectAll.TabIndex = 3
		Me.cmdSelectAll.TabStop = True
		Me.cmdSelectAll.Text = "Select &All Payments"
		Me.cmdSelectAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSelectAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdPayAsOne
		' 
		Me.cmdPayAsOne.BackColor = System.Drawing.SystemColors.Control
		Me.cmdPayAsOne.CausesValidation = True
		Me.cmdPayAsOne.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdPayAsOne.Enabled = True
		Me.cmdPayAsOne.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdPayAsOne.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdPayAsOne.Location = New System.Drawing.Point(160, 408)
		Me.cmdPayAsOne.Name = "cmdPayAsOne"
		Me.cmdPayAsOne.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdPayAsOne.Size = New System.Drawing.Size(147, 25)
		Me.cmdPayAsOne.TabIndex = 2
		Me.cmdPayAsOne.TabStop = True
		Me.cmdPayAsOne.Text = "Make &Single Payment"
		Me.cmdPayAsOne.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdPayAsOne.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdSettleAll
		' 
		Me.cmdSettleAll.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSettleAll.CausesValidation = True
		Me.cmdSettleAll.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSettleAll.Enabled = True
		Me.cmdSettleAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdSettleAll.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSettleAll.Location = New System.Drawing.Point(462, 408)
		Me.cmdSettleAll.Name = "cmdSettleAll"
		Me.cmdSettleAll.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSettleAll.Size = New System.Drawing.Size(145, 25)
		Me.cmdSettleAll.TabIndex = 1
		Me.cmdSettleAll.TabStop = True
		Me.cmdSettleAll.Text = "S&ettle All Selected"
		Me.cmdSettleAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSettleAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lvwPaymentDesc
		' 
		Me.lvwPaymentDesc.BackColor = System.Drawing.SystemColors.Window
		Me.lvwPaymentDesc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwPaymentDesc.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwPaymentDesc.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwPaymentDesc.FullRowSelect = True
		Me.lvwPaymentDesc.GridLines = True
		Me.lvwPaymentDesc.HideSelection = True
		Me.lvwPaymentDesc.LabelEdit = False
		Me.lvwPaymentDesc.LabelWrap = True
		Me.lvwPaymentDesc.Location = New System.Drawing.Point(8, 16)
		Me.lvwPaymentDesc.MultiSelect = True
		Me.lvwPaymentDesc.Name = "lvwPaymentDesc"
		Me.lvwPaymentDesc.Size = New System.Drawing.Size(421, 153)
		Me.lvwPaymentDesc.TabIndex = 5
		Me.lvwPaymentDesc.View = System.Windows.Forms.View.Details
		Me.lvwPaymentDesc.Columns.Add(Me._lvwPaymentDesc_ColumnHeader_1)
		Me.lvwPaymentDesc.Columns.Add(Me._lvwPaymentDesc_ColumnHeader_2)
		Me.lvwPaymentDesc.Columns.Add(Me._lvwPaymentDesc_ColumnHeader_3)
		' 
		' _lvwPaymentDesc_ColumnHeader_1
		' 
		Me._lvwPaymentDesc_ColumnHeader_1.Text = "Media Type"
		Me._lvwPaymentDesc_ColumnHeader_1.Width = 134
		' 
		' _lvwPaymentDesc_ColumnHeader_2
		' 
		Me._lvwPaymentDesc_ColumnHeader_2.Text = "Total No. of Payments"
		Me._lvwPaymentDesc_ColumnHeader_2.Width = 147
		' 
		' _lvwPaymentDesc_ColumnHeader_3
		' 
		Me._lvwPaymentDesc_ColumnHeader_3.Text = "Payment Value"
		Me._lvwPaymentDesc_ColumnHeader_3.Width = 134
		' 
		' frmPaymentProcessed
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(438, 217)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.fraUnallocatedClaimPayments)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmPaymentProcessed"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Payments Processed"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ToolTip1.SetToolTip(Me.cmdPayIndividually, "Makes an individual payment for each item selected")
		Me.ToolTip1.SetToolTip(Me.cmdPayAsOne, "Make a single payment for all selected items")
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.fraUnallocatedClaimPayments.ResumeLayout(False)
		Me.lvwPaymentDesc.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class