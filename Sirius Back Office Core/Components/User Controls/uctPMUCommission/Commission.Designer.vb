<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Commission
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
	Friend WithEvents txtLeadAgentTotalCommission As System.Windows.Forms.TextBox
	Friend WithEvents txtTotalNetCommission As System.Windows.Forms.TextBox
	Friend WithEvents txtSubAgentNet As System.Windows.Forms.TextBox
	Friend WithEvents txtLeadAgentNet As System.Windows.Forms.TextBox
	Friend WithEvents txtTotalTax As System.Windows.Forms.TextBox
	Friend WithEvents txtSubAgentTotalTax As System.Windows.Forms.TextBox
	Friend WithEvents txtLeadAgentTotalTax As System.Windows.Forms.TextBox
	Friend WithEvents txtTotalComm As System.Windows.Forms.TextBox
	Friend WithEvents txtSubAgentTotalCommission As System.Windows.Forms.TextBox
	Friend WithEvents lblTotalComm As System.Windows.Forms.Label
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents lblAgent As System.Windows.Forms.Label
	Friend WithEvents lblTotal As System.Windows.Forms.Label
	Friend WithEvents lblSubAgent As System.Windows.Forms.Label
	Friend WithEvents Line1 As System.Windows.Forms.Label
	Friend WithEvents lblTotalNetCommision As System.Windows.Forms.Label
	Friend WithEvents lblTotalTax As System.Windows.Forms.Label
	Friend WithEvents frmAgentTotal As System.Windows.Forms.GroupBox
	Friend WithEvents cmdEdit As System.Windows.Forms.Button
	Friend WithEvents lvwAgentCommission As System.Windows.Forms.ListView
	Friend WithEvents lblTaxAmendedStatus As System.Windows.Forms.Label
	Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblStatus As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.frmAgentTotal = New System.Windows.Forms.GroupBox
        Me.txtLeadAgentTotalCommission = New System.Windows.Forms.TextBox
        Me.txtTotalNetCommission = New System.Windows.Forms.TextBox
        Me.txtSubAgentNet = New System.Windows.Forms.TextBox
        Me.txtLeadAgentNet = New System.Windows.Forms.TextBox
        Me.txtTotalTax = New System.Windows.Forms.TextBox
        Me.txtSubAgentTotalTax = New System.Windows.Forms.TextBox
        Me.txtLeadAgentTotalTax = New System.Windows.Forms.TextBox
        Me.txtTotalComm = New System.Windows.Forms.TextBox
        Me.txtSubAgentTotalCommission = New System.Windows.Forms.TextBox
        Me.lblTotalComm = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblAgent = New System.Windows.Forms.Label
        Me.lblTotal = New System.Windows.Forms.Label
        Me.lblSubAgent = New System.Windows.Forms.Label
        Me.Line1 = New System.Windows.Forms.Label
        Me.lblTotalNetCommision = New System.Windows.Forms.Label
        Me.lblTotalTax = New System.Windows.Forms.Label
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.lvwAgentCommission = New System.Windows.Forms.ListView
        Me.lblTaxAmendedStatus = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblStatus = New System.Windows.Forms.Label
        Me.frmAgentTotal.SuspendLayout()
        Me.SuspendLayout()
        '
        'frmAgentTotal
        '
        Me.frmAgentTotal.BackColor = System.Drawing.SystemColors.Control
        Me.frmAgentTotal.Controls.Add(Me.txtLeadAgentTotalCommission)
        Me.frmAgentTotal.Controls.Add(Me.txtTotalNetCommission)
        Me.frmAgentTotal.Controls.Add(Me.txtSubAgentNet)
        Me.frmAgentTotal.Controls.Add(Me.txtLeadAgentNet)
        Me.frmAgentTotal.Controls.Add(Me.txtTotalTax)
        Me.frmAgentTotal.Controls.Add(Me.txtSubAgentTotalTax)
        Me.frmAgentTotal.Controls.Add(Me.txtLeadAgentTotalTax)
        Me.frmAgentTotal.Controls.Add(Me.txtTotalComm)
        Me.frmAgentTotal.Controls.Add(Me.txtSubAgentTotalCommission)
        Me.frmAgentTotal.Controls.Add(Me.lblTotalComm)
        Me.frmAgentTotal.Controls.Add(Me.Label1)
        Me.frmAgentTotal.Controls.Add(Me.lblAgent)
        Me.frmAgentTotal.Controls.Add(Me.lblTotal)
        Me.frmAgentTotal.Controls.Add(Me.lblSubAgent)
        Me.frmAgentTotal.Controls.Add(Me.Line1)
        Me.frmAgentTotal.Controls.Add(Me.lblTotalNetCommision)
        Me.frmAgentTotal.Controls.Add(Me.lblTotalTax)
        Me.frmAgentTotal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frmAgentTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frmAgentTotal.Location = New System.Drawing.Point(0, 120)
        Me.frmAgentTotal.Name = "frmAgentTotal"
        Me.frmAgentTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frmAgentTotal.Size = New System.Drawing.Size(681, 89)
        Me.frmAgentTotal.TabIndex = 1
        Me.frmAgentTotal.TabStop = False
        Me.frmAgentTotal.Text = "Agent Totals"
        '
        'txtLeadAgentTotalCommission
        '
        Me.txtLeadAgentTotalCommission.AcceptsReturn = True
        Me.txtLeadAgentTotalCommission.BackColor = System.Drawing.SystemColors.Control
        Me.txtLeadAgentTotalCommission.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtLeadAgentTotalCommission.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLeadAgentTotalCommission.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLeadAgentTotalCommission.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLeadAgentTotalCommission.Location = New System.Drawing.Point(128, 40)
        Me.txtLeadAgentTotalCommission.MaxLength = 0
        Me.txtLeadAgentTotalCommission.Name = "txtLeadAgentTotalCommission"
        Me.txtLeadAgentTotalCommission.ReadOnly = True
        Me.txtLeadAgentTotalCommission.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLeadAgentTotalCommission.Size = New System.Drawing.Size(167, 14)
        Me.txtLeadAgentTotalCommission.TabIndex = 6
        Me.txtLeadAgentTotalCommission.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTotalNetCommission
        '
        Me.txtTotalNetCommission.AcceptsReturn = True
        Me.txtTotalNetCommission.BackColor = System.Drawing.SystemColors.Control
        Me.txtTotalNetCommission.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtTotalNetCommission.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalNetCommission.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalNetCommission.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalNetCommission.Location = New System.Drawing.Point(496, 96)
        Me.txtTotalNetCommission.MaxLength = 0
        Me.txtTotalNetCommission.Name = "txtTotalNetCommission"
        Me.txtTotalNetCommission.ReadOnly = True
        Me.txtTotalNetCommission.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalNetCommission.Size = New System.Drawing.Size(183, 14)
        Me.txtTotalNetCommission.TabIndex = 16
        Me.txtTotalNetCommission.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtTotalNetCommission.Visible = False
        '
        'txtSubAgentNet
        '
        Me.txtSubAgentNet.AcceptsReturn = True
        Me.txtSubAgentNet.BackColor = System.Drawing.SystemColors.Control
        Me.txtSubAgentNet.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtSubAgentNet.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSubAgentNet.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSubAgentNet.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSubAgentNet.Location = New System.Drawing.Point(496, 60)
        Me.txtSubAgentNet.MaxLength = 0
        Me.txtSubAgentNet.Name = "txtSubAgentNet"
        Me.txtSubAgentNet.ReadOnly = True
        Me.txtSubAgentNet.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSubAgentNet.Size = New System.Drawing.Size(183, 14)
        Me.txtSubAgentNet.TabIndex = 12
        Me.txtSubAgentNet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtLeadAgentNet
        '
        Me.txtLeadAgentNet.AcceptsReturn = True
        Me.txtLeadAgentNet.BackColor = System.Drawing.SystemColors.Control
        Me.txtLeadAgentNet.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtLeadAgentNet.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLeadAgentNet.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLeadAgentNet.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLeadAgentNet.Location = New System.Drawing.Point(496, 40)
        Me.txtLeadAgentNet.MaxLength = 0
        Me.txtLeadAgentNet.Name = "txtLeadAgentNet"
        Me.txtLeadAgentNet.ReadOnly = True
        Me.txtLeadAgentNet.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLeadAgentNet.Size = New System.Drawing.Size(183, 14)
        Me.txtLeadAgentNet.TabIndex = 8
        Me.txtLeadAgentNet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTotalTax
        '
        Me.txtTotalTax.AcceptsReturn = True
        Me.txtTotalTax.BackColor = System.Drawing.SystemColors.Control
        Me.txtTotalTax.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtTotalTax.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalTax.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalTax.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalTax.Location = New System.Drawing.Point(304, 96)
        Me.txtTotalTax.MaxLength = 0
        Me.txtTotalTax.Name = "txtTotalTax"
        Me.txtTotalTax.ReadOnly = True
        Me.txtTotalTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalTax.Size = New System.Drawing.Size(183, 14)
        Me.txtTotalTax.TabIndex = 15
        Me.txtTotalTax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtTotalTax.Visible = False
        '
        'txtSubAgentTotalTax
        '
        Me.txtSubAgentTotalTax.AcceptsReturn = True
        Me.txtSubAgentTotalTax.BackColor = System.Drawing.SystemColors.Control
        Me.txtSubAgentTotalTax.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtSubAgentTotalTax.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSubAgentTotalTax.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSubAgentTotalTax.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSubAgentTotalTax.Location = New System.Drawing.Point(304, 60)
        Me.txtSubAgentTotalTax.MaxLength = 0
        Me.txtSubAgentTotalTax.Name = "txtSubAgentTotalTax"
        Me.txtSubAgentTotalTax.ReadOnly = True
        Me.txtSubAgentTotalTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSubAgentTotalTax.Size = New System.Drawing.Size(183, 14)
        Me.txtSubAgentTotalTax.TabIndex = 11
        Me.txtSubAgentTotalTax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtLeadAgentTotalTax
        '
        Me.txtLeadAgentTotalTax.AcceptsReturn = True
        Me.txtLeadAgentTotalTax.BackColor = System.Drawing.SystemColors.Control
        Me.txtLeadAgentTotalTax.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtLeadAgentTotalTax.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLeadAgentTotalTax.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLeadAgentTotalTax.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLeadAgentTotalTax.Location = New System.Drawing.Point(304, 40)
        Me.txtLeadAgentTotalTax.MaxLength = 0
        Me.txtLeadAgentTotalTax.Name = "txtLeadAgentTotalTax"
        Me.txtLeadAgentTotalTax.ReadOnly = True
        Me.txtLeadAgentTotalTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLeadAgentTotalTax.Size = New System.Drawing.Size(183, 14)
        Me.txtLeadAgentTotalTax.TabIndex = 7
        Me.txtLeadAgentTotalTax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTotalComm
        '
        Me.txtTotalComm.AcceptsReturn = True
        Me.txtTotalComm.BackColor = System.Drawing.SystemColors.Control
        Me.txtTotalComm.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtTotalComm.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalComm.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalComm.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalComm.Location = New System.Drawing.Point(128, 96)
        Me.txtTotalComm.MaxLength = 0
        Me.txtTotalComm.Name = "txtTotalComm"
        Me.txtTotalComm.ReadOnly = True
        Me.txtTotalComm.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalComm.Size = New System.Drawing.Size(167, 14)
        Me.txtTotalComm.TabIndex = 14
        Me.txtTotalComm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtTotalComm.Visible = False
        '
        'txtSubAgentTotalCommission
        '
        Me.txtSubAgentTotalCommission.AcceptsReturn = True
        Me.txtSubAgentTotalCommission.BackColor = System.Drawing.SystemColors.Control
        Me.txtSubAgentTotalCommission.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtSubAgentTotalCommission.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSubAgentTotalCommission.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSubAgentTotalCommission.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSubAgentTotalCommission.Location = New System.Drawing.Point(128, 60)
        Me.txtSubAgentTotalCommission.MaxLength = 0
        Me.txtSubAgentTotalCommission.Name = "txtSubAgentTotalCommission"
        Me.txtSubAgentTotalCommission.ReadOnly = True
        Me.txtSubAgentTotalCommission.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSubAgentTotalCommission.Size = New System.Drawing.Size(167, 14)
        Me.txtSubAgentTotalCommission.TabIndex = 10
        Me.txtSubAgentTotalCommission.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblTotalComm
        '
        Me.lblTotalComm.AutoSize = True
        Me.lblTotalComm.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalComm.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalComm.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalComm.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalComm.Location = New System.Drawing.Point(193, 16)
        Me.lblTotalComm.Name = "lblTotalComm"
        Me.lblTotalComm.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalComm.Size = New System.Drawing.Size(109, 13)
        Me.lblTotalComm.TabIndex = 1
        Me.lblTotalComm.Text = "Total Commission"
        Me.lblTotalComm.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(16, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(72, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Agent Type"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblAgent
        '
        Me.lblAgent.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgent.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgent.Location = New System.Drawing.Point(16, 42)
        Me.lblAgent.Name = "lblAgent"
        Me.lblAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgent.Size = New System.Drawing.Size(89, 17)
        Me.lblAgent.TabIndex = 5
        Me.lblAgent.Text = "Lead Agent "
        '
        'lblTotal
        '
        Me.lblTotal.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotal.Location = New System.Drawing.Point(16, 96)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotal.Size = New System.Drawing.Size(89, 17)
        Me.lblTotal.TabIndex = 13
        Me.lblTotal.Text = "Total "
        Me.lblTotal.Visible = False
        '
        'lblSubAgent
        '
        Me.lblSubAgent.BackColor = System.Drawing.SystemColors.Control
        Me.lblSubAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSubAgent.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubAgent.Location = New System.Drawing.Point(16, 62)
        Me.lblSubAgent.Name = "lblSubAgent"
        Me.lblSubAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSubAgent.Size = New System.Drawing.Size(89, 17)
        Me.lblSubAgent.TabIndex = 9
        Me.lblSubAgent.Text = "Sub Agent"
        '
        'Line1
        '
        Me.Line1.BackColor = System.Drawing.SystemColors.WindowText
        Me.Line1.Location = New System.Drawing.Point(6, 36)
        Me.Line1.Name = "Line1"
        Me.Line1.Size = New System.Drawing.Size(672, 1)
        Me.Line1.TabIndex = 4
        '
        'lblTotalNetCommision
        '
        Me.lblTotalNetCommision.AutoSize = True
        Me.lblTotalNetCommision.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalNetCommision.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalNetCommision.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalNetCommision.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalNetCommision.Location = New System.Drawing.Point(573, 16)
        Me.lblTotalNetCommision.Name = "lblTotalNetCommision"
        Me.lblTotalNetCommision.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalNetCommision.Size = New System.Drawing.Size(113, 13)
        Me.lblTotalNetCommision.TabIndex = 3
        Me.lblTotalNetCommision.Text = "Total Net Premium"
        Me.lblTotalNetCommision.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblTotalTax
        '
        Me.lblTotalTax.AutoSize = True
        Me.lblTotalTax.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalTax.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalTax.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalTax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalTax.Location = New System.Drawing.Point(434, 16)
        Me.lblTotalTax.Name = "lblTotalTax"
        Me.lblTotalTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalTax.Size = New System.Drawing.Size(60, 13)
        Me.lblTotalTax.TabIndex = 2
        Me.lblTotalTax.Text = "Total Tax"
        Me.lblTotalTax.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(0, 216)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 23)
        Me.cmdEdit.TabIndex = 2
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'lvwAgentCommission
        '
        Me.lvwAgentCommission.BackColor = System.Drawing.SystemColors.Window
        Me.lvwAgentCommission.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwAgentCommission.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwAgentCommission.Location = New System.Drawing.Point(0, 0)
        Me.lvwAgentCommission.Name = "lvwAgentCommission"
        Me.lvwAgentCommission.Size = New System.Drawing.Size(687, 113)
        Me.lvwAgentCommission.TabIndex = 0
        Me.lvwAgentCommission.UseCompatibleStateImageBehavior = False
        Me.lvwAgentCommission.View = System.Windows.Forms.View.Details
        '
        'lblTaxAmendedStatus
        '
        Me.lblTaxAmendedStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxAmendedStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxAmendedStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxAmendedStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxAmendedStatus.Location = New System.Drawing.Point(392, 220)
        Me.lblTaxAmendedStatus.Name = "lblTaxAmendedStatus"
        Me.lblTaxAmendedStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxAmendedStatus.Size = New System.Drawing.Size(279, 13)
        Me.lblTaxAmendedStatus.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(56, 720)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(319, 13)
        Me.Label2.TabIndex = 20
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatus.Location = New System.Drawing.Point(82, 220)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatus.Size = New System.Drawing.Size(296, 13)
        Me.lblStatus.TabIndex = 3
        '
        'Commission
        '
        Me.Controls.Add(Me.frmAgentTotal)
        Me.Controls.Add(Me.cmdEdit)
        Me.Controls.Add(Me.lvwAgentCommission)
        Me.Controls.Add(Me.lblTaxAmendedStatus)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblStatus)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "Commission"
        Me.Size = New System.Drawing.Size(693, 247)
        Me.frmAgentTotal.ResumeLayout(False)
        Me.frmAgentTotal.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class