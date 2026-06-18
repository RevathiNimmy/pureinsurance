<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFilterRenewal
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
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cboAgentCode As System.Windows.Forms.ComboBox
    Public WithEvents cboBranch As System.Windows.Forms.ComboBox
	Public WithEvents cboProduct As PMLookupControl.cboPMLookup
	Public WithEvents lblAgent As System.Windows.Forms.Label
	Public WithEvents lblBranch As System.Windows.Forms.Label
	Public WithEvents lblProductType As System.Windows.Forms.Label
	Public WithEvents lblRenewalDate As System.Windows.Forms.Label
	Public WithEvents fraFilter As System.Windows.Forms.GroupBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.fraFilter = New System.Windows.Forms.GroupBox
        Me.cboAgentCode = New System.Windows.Forms.ComboBox
        Me.dtpRenewalDate = New System.Windows.Forms.DateTimePicker
        Me.cboBranch = New System.Windows.Forms.ComboBox
        Me.cboProduct = New PMLookupControl.cboPMLookup
        Me.lblAgent = New System.Windows.Forms.Label
        Me.lblBranch = New System.Windows.Forms.Label
        Me.lblProductType = New System.Windows.Forms.Label
        Me.lblRenewalDate = New System.Windows.Forms.Label
        Me.fraFilter.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(335, 175)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 5
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(258, 175)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 4
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'fraFilter
        '
        Me.fraFilter.BackColor = System.Drawing.SystemColors.Control
        Me.fraFilter.Controls.Add(Me.cboAgentCode)
        Me.fraFilter.Controls.Add(Me.dtpRenewalDate)
        Me.fraFilter.Controls.Add(Me.cboBranch)
        Me.fraFilter.Controls.Add(Me.cboProduct)
        Me.fraFilter.Controls.Add(Me.lblAgent)
        Me.fraFilter.Controls.Add(Me.lblBranch)
        Me.fraFilter.Controls.Add(Me.lblProductType)
        Me.fraFilter.Controls.Add(Me.lblRenewalDate)
        Me.fraFilter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraFilter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFilter.Location = New System.Drawing.Point(6, 4)
        Me.fraFilter.Name = "fraFilter"
        Me.fraFilter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFilter.Size = New System.Drawing.Size(407, 167)
        Me.fraFilter.TabIndex = 0
        Me.fraFilter.TabStop = False
        Me.fraFilter.Text = "Filter Criteria"
        '
        'cboAgentCode
        '
        Me.cboAgentCode.BackColor = System.Drawing.SystemColors.Window
        Me.cboAgentCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboAgentCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAgentCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboAgentCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboAgentCode.Location = New System.Drawing.Point(95, 128)
        Me.cboAgentCode.Name = "cboAgentCode"
        Me.cboAgentCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboAgentCode.Size = New System.Drawing.Size(295, 21)
        Me.cboAgentCode.TabIndex = 3
        '
        'dtpRenewalDate
        '
        Me.dtpRenewalDate.Checked = False
        Me.dtpRenewalDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpRenewalDate.Location = New System.Drawing.Point(96, 23)
        Me.dtpRenewalDate.Name = "dtpRenewalDate"
        Me.dtpRenewalDate.ShowCheckBox = True
        Me.dtpRenewalDate.Size = New System.Drawing.Size(145, 21)
        Me.dtpRenewalDate.TabIndex = 9
        '
        'cboBranch
        '
        Me.cboBranch.BackColor = System.Drawing.SystemColors.Window
        Me.cboBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBranch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBranch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBranch.Location = New System.Drawing.Point(96, 94)
        Me.cboBranch.Name = "cboBranch"
        Me.cboBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBranch.Size = New System.Drawing.Size(295, 21)
        Me.cboBranch.TabIndex = 2
        '
        'cboProduct
        '
        Me.cboProduct.DefaultItemId = 0
        Me.cboProduct.FirstItem = ""
        Me.cboProduct.ItemId = 0
        Me.cboProduct.ListIndex = -1
        Me.cboProduct.Location = New System.Drawing.Point(96, 60)
        Me.cboProduct.Name = "cboProduct"
        Me.cboProduct.PMLookupProductFamily = 9
        Me.cboProduct.SingleItemId = 0
        Me.cboProduct.Size = New System.Drawing.Size(295, 21)
        Me.cboProduct.Sorted = True
        Me.cboProduct.TabIndex = 1
        Me.cboProduct.TableName = "Product"
        Me.cboProduct.ToolTipText = ""
        Me.cboProduct.WhereClause = ""
        '
        'lblAgent
        '
        Me.lblAgent.AutoSize = True
        Me.lblAgent.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgent.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgent.Location = New System.Drawing.Point(8, 132)
        Me.lblAgent.Name = "lblAgent"
        Me.lblAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgent.Size = New System.Drawing.Size(74, 13)
        Me.lblAgent.TabIndex = 10
        Me.lblAgent.Text = "Agent Code"
        '
        'lblBranch
        '
        Me.lblBranch.AutoSize = True
        Me.lblBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranch.Location = New System.Drawing.Point(8, 98)
        Me.lblBranch.Name = "lblBranch"
        Me.lblBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranch.Size = New System.Drawing.Size(81, 13)
        Me.lblBranch.TabIndex = 8
        Me.lblBranch.Text = "Branch Code"
        '
        'lblProductType
        '
        Me.lblProductType.AutoSize = True
        Me.lblProductType.BackColor = System.Drawing.SystemColors.Control
        Me.lblProductType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProductType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProductType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProductType.Location = New System.Drawing.Point(8, 64)
        Me.lblProductType.Name = "lblProductType"
        Me.lblProductType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProductType.Size = New System.Drawing.Size(82, 13)
        Me.lblProductType.TabIndex = 7
        Me.lblProductType.Text = "Product Type"
        '
        'lblRenewalDate
        '
        Me.lblRenewalDate.AutoSize = True
        Me.lblRenewalDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblRenewalDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRenewalDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRenewalDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRenewalDate.Location = New System.Drawing.Point(8, 27)
        Me.lblRenewalDate.Name = "lblRenewalDate"
        Me.lblRenewalDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRenewalDate.Size = New System.Drawing.Size(86, 13)
        Me.lblRenewalDate.TabIndex = 6
        Me.lblRenewalDate.Text = "Renewal Date"
        '
        'frmFilterRenewal
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(416, 200)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.fraFilter)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmFilterRenewal"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Filter Renewals"
        Me.fraFilter.ResumeLayout(False)
        Me.fraFilter.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents dtpRenewalDate As System.Windows.Forms.DateTimePicker
#End Region 
End Class