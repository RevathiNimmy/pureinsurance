<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRIPortfolioTransfer
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
	Public WithEvents txtDate As System.Windows.Forms.DateTimePicker
	Public WithEvents cboProducts As PMLookupControl.cboPMLookup
	Public WithEvents lblTransferDate As System.Windows.Forms.Label
	Public WithEvents lblProduct As System.Windows.Forms.Label
	Public WithEvents fmeSelectPolicy As System.Windows.Forms.GroupBox
	Public WithEvents txtClientName As System.Windows.Forms.TextBox
	Public WithEvents txtPolicyNumber As System.Windows.Forms.TextBox
	Public WithEvents txtClientCode As System.Windows.Forms.TextBox
	Public WithEvents lblClientName As System.Windows.Forms.Label
	Public WithEvents lblPolicyNumber As System.Windows.Forms.Label
	Public WithEvents lblClientCode As System.Windows.Forms.Label
	Public WithEvents fmeCurrentPolicy As System.Windows.Forms.GroupBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdTransfer As System.Windows.Forms.Button
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRIPortfolioTransfer))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.fmeSelectPolicy = New System.Windows.Forms.GroupBox()
        Me.txtDate = New System.Windows.Forms.DateTimePicker()
        Me.cboProducts = New PMLookupControl.cboPMLookup()
        Me.lblTransferDate = New System.Windows.Forms.Label()
        Me.lblProduct = New System.Windows.Forms.Label()
        Me.fmeCurrentPolicy = New System.Windows.Forms.GroupBox()
        Me.txtClientName = New System.Windows.Forms.TextBox()
        Me.txtPolicyNumber = New System.Windows.Forms.TextBox()
        Me.txtClientCode = New System.Windows.Forms.TextBox()
        Me.lblClientName = New System.Windows.Forms.Label()
        Me.lblPolicyNumber = New System.Windows.Forms.Label()
        Me.lblClientCode = New System.Windows.Forms.Label()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdTransfer = New System.Windows.Forms.Button()
        Me.sbrStatus = New System.Windows.Forms.StatusStrip()
        Me._sbrStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.fmeSelectPolicy.SuspendLayout()
        Me.fmeCurrentPolicy.SuspendLayout()
        Me.sbrStatus.SuspendLayout()
        Me.SuspendLayout()
        '
        'fmeSelectPolicy
        '
        Me.fmeSelectPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.fmeSelectPolicy.Controls.Add(Me.txtDate)
        Me.fmeSelectPolicy.Controls.Add(Me.cboProducts)
        Me.fmeSelectPolicy.Controls.Add(Me.lblTransferDate)
        Me.fmeSelectPolicy.Controls.Add(Me.lblProduct)
        Me.fmeSelectPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fmeSelectPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fmeSelectPolicy.Location = New System.Drawing.Point(8, 8)
        Me.fmeSelectPolicy.Name = "fmeSelectPolicy"
        Me.fmeSelectPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fmeSelectPolicy.Size = New System.Drawing.Size(377, 97)
        Me.fmeSelectPolicy.TabIndex = 10
        Me.fmeSelectPolicy.TabStop = False
        Me.fmeSelectPolicy.Text = "Select Policies For Batch Transfer"
        '
        'txtDate
        '
        Me.txtDate.Location = New System.Drawing.Point(120, 56)
        Me.txtDate.Name = "txtDate"
        Me.txtDate.Size = New System.Drawing.Size(129, 20)
        Me.txtDate.TabIndex = 14
        '
        'cboProducts
        '
        Me.cboProducts.DefaultItemId = 0
        Me.cboProducts.FirstItem = ""
        Me.cboProducts.ItemId = 0
        Me.cboProducts.ListIndex = -1
        Me.cboProducts.Location = New System.Drawing.Point(120, 32)
        Me.cboProducts.Name = "cboProducts"
        Me.cboProducts.PMLookupProductFamily = 1
        Me.cboProducts.SingleItemId = 0
        Me.cboProducts.Size = New System.Drawing.Size(153, 21)
        Me.cboProducts.Sorted = True
        Me.cboProducts.TabIndex = 13
        Me.cboProducts.TableName = "product"
        Me.cboProducts.ToolTipText = ""
        Me.cboProducts.WhereClause = ""
        '
        'lblTransferDate
        '
        Me.lblTransferDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblTransferDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTransferDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTransferDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTransferDate.Location = New System.Drawing.Point(16, 56)
        Me.lblTransferDate.Name = "lblTransferDate"
        Me.lblTransferDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTransferDate.Size = New System.Drawing.Size(100, 19)
        Me.lblTransferDate.TabIndex = 12
        Me.lblTransferDate.Text = "Transfer Date:"
        '
        'lblProduct
        '
        Me.lblProduct.BackColor = System.Drawing.SystemColors.Control
        Me.lblProduct.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProduct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProduct.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProduct.Location = New System.Drawing.Point(16, 32)
        Me.lblProduct.Name = "lblProduct"
        Me.lblProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProduct.Size = New System.Drawing.Size(100, 19)
        Me.lblProduct.TabIndex = 11
        Me.lblProduct.Text = "Product:"
        '
        'fmeCurrentPolicy
        '
        Me.fmeCurrentPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.fmeCurrentPolicy.Controls.Add(Me.txtClientName)
        Me.fmeCurrentPolicy.Controls.Add(Me.txtPolicyNumber)
        Me.fmeCurrentPolicy.Controls.Add(Me.txtClientCode)
        Me.fmeCurrentPolicy.Controls.Add(Me.lblClientName)
        Me.fmeCurrentPolicy.Controls.Add(Me.lblPolicyNumber)
        Me.fmeCurrentPolicy.Controls.Add(Me.lblClientCode)
        Me.fmeCurrentPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fmeCurrentPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fmeCurrentPolicy.Location = New System.Drawing.Point(8, 107)
        Me.fmeCurrentPolicy.Name = "fmeCurrentPolicy"
        Me.fmeCurrentPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fmeCurrentPolicy.Size = New System.Drawing.Size(377, 121)
        Me.fmeCurrentPolicy.TabIndex = 6
        Me.fmeCurrentPolicy.TabStop = False
        Me.fmeCurrentPolicy.Text = "Current Policy"
        '
        'txtClientName
        '
        Me.txtClientName.AcceptsReturn = True
        Me.txtClientName.BackColor = System.Drawing.SystemColors.Control
        Me.txtClientName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClientName.Enabled = False
        Me.txtClientName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClientName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClientName.Location = New System.Drawing.Point(111, 86)
        Me.txtClientName.MaxLength = 0
        Me.txtClientName.Name = "txtClientName"
        Me.txtClientName.ReadOnly = True
        Me.txtClientName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClientName.Size = New System.Drawing.Size(249, 20)
        Me.txtClientName.TabIndex = 5
        '
        'txtPolicyNumber
        '
        Me.txtPolicyNumber.AcceptsReturn = True
        Me.txtPolicyNumber.BackColor = System.Drawing.SystemColors.Control
        Me.txtPolicyNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicyNumber.Enabled = False
        Me.txtPolicyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicyNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicyNumber.Location = New System.Drawing.Point(111, 33)
        Me.txtPolicyNumber.MaxLength = 0
        Me.txtPolicyNumber.Name = "txtPolicyNumber"
        Me.txtPolicyNumber.ReadOnly = True
        Me.txtPolicyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicyNumber.Size = New System.Drawing.Size(249, 20)
        Me.txtPolicyNumber.TabIndex = 1
        '
        'txtClientCode
        '
        Me.txtClientCode.AcceptsReturn = True
        Me.txtClientCode.BackColor = System.Drawing.SystemColors.Control
        Me.txtClientCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClientCode.Enabled = False
        Me.txtClientCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClientCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClientCode.Location = New System.Drawing.Point(111, 59)
        Me.txtClientCode.MaxLength = 0
        Me.txtClientCode.Name = "txtClientCode"
        Me.txtClientCode.ReadOnly = True
        Me.txtClientCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClientCode.Size = New System.Drawing.Size(249, 20)
        Me.txtClientCode.TabIndex = 3
        '
        'lblClientName
        '
        Me.lblClientName.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientName.Location = New System.Drawing.Point(12, 88)
        Me.lblClientName.Name = "lblClientName"
        Me.lblClientName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientName.Size = New System.Drawing.Size(100, 19)
        Me.lblClientName.TabIndex = 4
        Me.lblClientName.Text = "Client Name:"
        '
        'lblPolicyNumber
        '
        Me.lblPolicyNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyNumber.Location = New System.Drawing.Point(12, 34)
        Me.lblPolicyNumber.Name = "lblPolicyNumber"
        Me.lblPolicyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyNumber.Size = New System.Drawing.Size(100, 19)
        Me.lblPolicyNumber.TabIndex = 0
        Me.lblPolicyNumber.Text = "Policy Number:"
        '
        'lblClientCode
        '
        Me.lblClientCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientCode.Location = New System.Drawing.Point(12, 61)
        Me.lblClientCode.Name = "lblClientCode"
        Me.lblClientCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientCode.Size = New System.Drawing.Size(100, 19)
        Me.lblClientCode.TabIndex = 2
        Me.lblClientCode.Text = "Client Code:"
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(310, 234)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(76, 23)
        Me.cmdCancel.TabIndex = 8
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdTransfer
        '
        Me.cmdTransfer.BackColor = System.Drawing.SystemColors.Control
        Me.cmdTransfer.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdTransfer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTransfer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdTransfer.Location = New System.Drawing.Point(232, 234)
        Me.cmdTransfer.Name = "cmdTransfer"
        Me.cmdTransfer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdTransfer.Size = New System.Drawing.Size(73, 23)
        Me.cmdTransfer.TabIndex = 7
        Me.cmdTransfer.Text = "&Transfer"
        Me.cmdTransfer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdTransfer.UseVisualStyleBackColor = False
        '
        'sbrStatus
        '
        Me.sbrStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._sbrStatus_Panel1})
        Me.sbrStatus.Location = New System.Drawing.Point(0, 260)
        Me.sbrStatus.Name = "sbrStatus"
        Me.sbrStatus.Size = New System.Drawing.Size(395, 22)
        Me.sbrStatus.TabIndex = 11
        Me.sbrStatus.Text = "StatusStrip1"
        '
        '_sbrStatus_Panel1
        '
        Me._sbrStatus_Panel1.Name = "_sbrStatus_Panel1"
        Me._sbrStatus_Panel1.Size = New System.Drawing.Size(0, 17)
        '
        'frmRIPortfolioTransfer
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(395, 282)
        Me.Controls.Add(Me.sbrStatus)
        Me.Controls.Add(Me.fmeSelectPolicy)
        Me.Controls.Add(Me.fmeCurrentPolicy)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdTransfer)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(15, 28)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmRIPortfolioTransfer"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "RI Portfolio Transfer"
        Me.fmeSelectPolicy.ResumeLayout(False)
        Me.fmeCurrentPolicy.ResumeLayout(False)
        Me.fmeCurrentPolicy.PerformLayout()
        Me.sbrStatus.ResumeLayout(False)
        Me.sbrStatus.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents sbrStatus As System.Windows.Forms.StatusStrip
    Friend WithEvents _sbrStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
#End Region 
End Class