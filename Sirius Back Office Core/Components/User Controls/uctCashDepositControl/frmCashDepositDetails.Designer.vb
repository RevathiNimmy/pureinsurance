<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCashDepositDetails
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
	Public WithEvents cmdAddTask As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdApply As System.Windows.Forms.Button
	Public WithEvents chkSinglePolicyLock As System.Windows.Forms.CheckBox
	Public WithEvents txtCDNumber As System.Windows.Forms.TextBox
	Public WithEvents PickListProducts As uctPickList.PickList
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents PickListBranches As uctPickList.PickList
	Public WithEvents Frame3 As System.Windows.Forms.GroupBox
	Public WithEvents lblCDNumber As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCashDepositDetails))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdAddTask = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdApply = New System.Windows.Forms.Button
        Me.chkSinglePolicyLock = New System.Windows.Forms.CheckBox
        Me.txtCDNumber = New System.Windows.Forms.TextBox
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.Frame3 = New System.Windows.Forms.GroupBox
        Me.lblCDNumber = New System.Windows.Forms.Label
        Me.PickListProducts = New uctPickList.PickList
        Me.PickListBranches = New uctPickList.PickList
        Me.Frame1.SuspendLayout()
        Me.Frame3.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdAddTask
        '
        Me.cmdAddTask.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddTask.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddTask.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddTask.Location = New System.Drawing.Point(352, 632)
        Me.cmdAddTask.Name = "cmdAddTask"
        Me.cmdAddTask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddTask.Size = New System.Drawing.Size(79, 25)
        Me.cmdAddTask.TabIndex = 10
        Me.cmdAddTask.Text = "Add &Task"
        Me.cmdAddTask.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddTask.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(616, 632)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(79, 25)
        Me.cmdCancel.TabIndex = 9
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(528, 632)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(79, 25)
        Me.cmdOK.TabIndex = 8
        Me.cmdOK.Text = "&Ok"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Location = New System.Drawing.Point(440, 632)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApply.Size = New System.Drawing.Size(79, 25)
        Me.cmdApply.TabIndex = 7
        Me.cmdApply.Text = "&Apply"
        Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdApply.UseVisualStyleBackColor = False
        '
        'chkSinglePolicyLock
        '
        Me.chkSinglePolicyLock.BackColor = System.Drawing.SystemColors.Control
        Me.chkSinglePolicyLock.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkSinglePolicyLock.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSinglePolicyLock.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSinglePolicyLock.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSinglePolicyLock.Location = New System.Drawing.Point(576, 0)
        Me.chkSinglePolicyLock.Name = "chkSinglePolicyLock"
        Me.chkSinglePolicyLock.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSinglePolicyLock.Size = New System.Drawing.Size(129, 25)
        Me.chkSinglePolicyLock.TabIndex = 6
        Me.chkSinglePolicyLock.Text = "Single Policy Lock"
        Me.chkSinglePolicyLock.UseVisualStyleBackColor = False
        '
        'txtCDNumber
        '
        Me.txtCDNumber.AcceptsReturn = True
        Me.txtCDNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtCDNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCDNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCDNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCDNumber.Location = New System.Drawing.Point(120, 8)
        Me.txtCDNumber.MaxLength = 0
        Me.txtCDNumber.Name = "txtCDNumber"
        Me.txtCDNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCDNumber.Size = New System.Drawing.Size(185, 19)
        Me.txtCDNumber.TabIndex = 5
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.PickListProducts)
        Me.Frame1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(8, 32)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(697, 281)
        Me.Frame1.TabIndex = 2
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Select Products"
        '
        'Frame3
        '
        Me.Frame3.BackColor = System.Drawing.SystemColors.Control
        Me.Frame3.Controls.Add(Me.PickListBranches)
        Me.Frame3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame3.Location = New System.Drawing.Point(8, 320)
        Me.Frame3.Name = "Frame3"
        Me.Frame3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame3.Size = New System.Drawing.Size(689, 297)
        Me.Frame3.TabIndex = 0
        Me.Frame3.TabStop = False
        Me.Frame3.Text = "Select Branches"
        '
        'lblCDNumber
        '
        Me.lblCDNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblCDNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCDNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCDNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCDNumber.Location = New System.Drawing.Point(8, 8)
        Me.lblCDNumber.Name = "lblCDNumber"
        Me.lblCDNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCDNumber.Size = New System.Drawing.Size(105, 25)
        Me.lblCDNumber.TabIndex = 4
        Me.lblCDNumber.Text = "CD Number"
        '
        'PickListProducts
        '
        Me.PickListProducts.AvailableCaption = "Searched Products "
        Me.PickListProducts.BusinessObject = "bSIRCashDeposit.Business"
        Me.PickListProducts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PickListProducts.ForeignKeys = CType(resources.GetObject("PickListProducts.ForeignKeys"), Microsoft.VisualBasic.Collection)
        Me.PickListProducts.IsSearchable = True
        Me.PickListProducts.Location = New System.Drawing.Point(2, 20)
        Me.PickListProducts.Name = "PickListProducts"
        Me.PickListProducts.PickListType = ""
        Me.PickListProducts.Size = New System.Drawing.Size(693, 259)
        Me.PickListProducts.TabIndex = 3
        '
        'PickListBranches
        '
        Me.PickListBranches.AvailableCaption = "Searched Branches"
        Me.PickListBranches.BusinessObject = "bSIRCashDeposit.Business"
        Me.PickListBranches.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PickListBranches.ForeignKeys = CType(resources.GetObject("PickListBranches.ForeignKeys"), Microsoft.VisualBasic.Collection)
        Me.PickListBranches.IsSearchable = True
        Me.PickListBranches.Location = New System.Drawing.Point(2, 12)
        Me.PickListBranches.Name = "PickListBranches"
        Me.PickListBranches.PickListType = ""
        Me.PickListBranches.Size = New System.Drawing.Size(685, 283)
        Me.PickListBranches.TabIndex = 1
        '
        'frmCashDepositDetails
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(708, 662)
        Me.Controls.Add(Me.cmdAddTask)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.chkSinglePolicyLock)
        Me.Controls.Add(Me.txtCDNumber)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.Frame3)
        Me.Controls.Add(Me.lblCDNumber)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCashDepositDetails"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Cash Deposit Account Setup"
        Me.Frame1.ResumeLayout(False)
        Me.Frame3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class