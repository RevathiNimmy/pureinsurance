<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctPMAddressControl
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		UserControl_InitProperties()
	End Sub
    'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Friend WithEvents txtAddress2 As System.Windows.Forms.TextBox
	Friend WithEvents txtAddress1 As System.Windows.Forms.TextBox
	Friend WithEvents cboCountry As PMLookupControl.cboPMLookup
	Friend WithEvents cmdSearch As System.Windows.Forms.Button
	Friend WithEvents txtPostCode As System.Windows.Forms.TextBox
	Friend WithEvents cmdDelete As System.Windows.Forms.Button
	Friend WithEvents txtAddress3 As System.Windows.Forms.TextBox
	Friend WithEvents txtAddress4 As System.Windows.Forms.TextBox
	Friend WithEvents cboState As PMLookupControl.cboPMLookup
	Friend WithEvents lblStateDesc As System.Windows.Forms.Label
	Friend WithEvents lblCountry As System.Windows.Forms.Label
	Friend WithEvents lblPostCode As System.Windows.Forms.Label
	Friend WithEvents lblAddress1 As System.Windows.Forms.Label
	Friend WithEvents lblAddress2 As System.Windows.Forms.Label
	Friend WithEvents lblAddress3 As System.Windows.Forms.Label
	Friend WithEvents lblAddress4 As System.Windows.Forms.Label
	Friend WithEvents fraAddress As System.Windows.Forms.GroupBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdSearch = New System.Windows.Forms.Button
        Me.fraAddress = New System.Windows.Forms.GroupBox
        Me.txtAddress4 = New System.Windows.Forms.TextBox
        Me.txtAddress2 = New System.Windows.Forms.TextBox
        Me.txtAddress1 = New System.Windows.Forms.TextBox
        Me.cboCountry = New PMLookupControl.cboPMLookup
        Me.txtPostCode = New System.Windows.Forms.TextBox
        Me.txtAddress3 = New System.Windows.Forms.TextBox
        Me.cboState = New PMLookupControl.cboPMLookup
        Me.lblStateDesc = New System.Windows.Forms.Label
        Me.lblCountry = New System.Windows.Forms.Label
        Me.lblPostCode = New System.Windows.Forms.Label
        Me.lblAddress1 = New System.Windows.Forms.Label
        Me.lblAddress2 = New System.Windows.Forms.Label
        Me.lblAddress3 = New System.Windows.Forms.Label
        Me.lblAddress4 = New System.Windows.Forms.Label
        Me.fraAddress.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(318, 102)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(24, 19)
        Me.cmdDelete.TabIndex = 9
        Me.cmdDelete.Text = "X"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdDelete, "Clear")
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdSearch
        '
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSearch.Location = New System.Drawing.Point(290, 102)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSearch.Size = New System.Drawing.Size(24, 19)
        Me.cmdSearch.TabIndex = 8
        Me.cmdSearch.Text = ".."
        Me.cmdSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdSearch, "Search")
        Me.cmdSearch.UseVisualStyleBackColor = False
        '
        'fraAddress
        '
        Me.fraAddress.BackColor = System.Drawing.SystemColors.Control
        Me.fraAddress.Controls.Add(Me.txtAddress4)
        Me.fraAddress.Controls.Add(Me.txtAddress3)
        Me.fraAddress.Controls.Add(Me.txtAddress2)
        Me.fraAddress.Controls.Add(Me.txtAddress1)
        Me.fraAddress.Controls.Add(Me.cboCountry)
        Me.fraAddress.Controls.Add(Me.cmdSearch)
        Me.fraAddress.Controls.Add(Me.txtPostCode)
        Me.fraAddress.Controls.Add(Me.cmdDelete)
        Me.fraAddress.Controls.Add(Me.cboState)
        Me.fraAddress.Controls.Add(Me.lblStateDesc)
        Me.fraAddress.Controls.Add(Me.lblCountry)
        Me.fraAddress.Controls.Add(Me.lblPostCode)
        Me.fraAddress.Controls.Add(Me.lblAddress1)
        Me.fraAddress.Controls.Add(Me.lblAddress2)
        Me.fraAddress.Controls.Add(Me.lblAddress3)
        Me.fraAddress.Controls.Add(Me.lblAddress4)
        Me.fraAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAddress.Location = New System.Drawing.Point(0, 0)
        Me.fraAddress.Name = "fraAddress"
        Me.fraAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAddress.Size = New System.Drawing.Size(353, 153)
        Me.fraAddress.TabIndex = 0
        Me.fraAddress.TabStop = False
        '
        'txtAddress4
        '
        Me.txtAddress4.AcceptsReturn = True
        Me.txtAddress4.BackColor = System.Drawing.SystemColors.Window
        Me.txtAddress4.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAddress4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAddress4.Location = New System.Drawing.Point(135, 80)
        Me.txtAddress4.MaxLength = 0
        Me.txtAddress4.Name = "txtAddress4"
        Me.txtAddress4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAddress4.Size = New System.Drawing.Size(208, 20)
        Me.txtAddress4.TabIndex = 5
        '
        'txtAddress2
        '
        Me.txtAddress2.AcceptsReturn = True
        Me.txtAddress2.BackColor = System.Drawing.SystemColors.Window
        Me.txtAddress2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAddress2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAddress2.Location = New System.Drawing.Point(135, 37)
        Me.txtAddress2.MaxLength = 60
        Me.txtAddress2.Name = "txtAddress2"
        Me.txtAddress2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAddress2.Size = New System.Drawing.Size(208, 20)
        Me.txtAddress2.TabIndex = 2
        '
        'txtAddress1
        '
        Me.txtAddress1.AcceptsReturn = True
        Me.txtAddress1.BackColor = System.Drawing.SystemColors.Window
        Me.txtAddress1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAddress1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAddress1.Location = New System.Drawing.Point(135, 16)
        Me.txtAddress1.MaxLength = 60
        Me.txtAddress1.Name = "txtAddress1"
        Me.txtAddress1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAddress1.Size = New System.Drawing.Size(208, 20)
        Me.txtAddress1.TabIndex = 1
        '
        'cboCountry
        '
        Me.cboCountry.DefaultItemId = 0
        Me.cboCountry.FirstItem = ""
        Me.cboCountry.ItemId = 0
        Me.cboCountry.ListIndex = -1
        Me.cboCountry.Location = New System.Drawing.Point(135, 123)
        Me.cboCountry.Name = "cboCountry"
        Me.cboCountry.PMLookupProductFamily = 1
        Me.cboCountry.SingleItemId = 0
        Me.cboCountry.Size = New System.Drawing.Size(209, 21)
        Me.cboCountry.Sorted = True
        Me.cboCountry.TabIndex = 7
        Me.cboCountry.TableName = "Country"
        Me.cboCountry.ToolTipText = ""
        Me.cboCountry.WhereClause = "Is_deleted = 0"
        '
        'txtPostCode
        '
        Me.txtPostCode.AcceptsReturn = True
        Me.txtPostCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtPostCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPostCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPostCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPostCode.Location = New System.Drawing.Point(135, 102)
        Me.txtPostCode.MaxLength = 20
        Me.txtPostCode.Name = "txtPostCode"
        Me.txtPostCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPostCode.Size = New System.Drawing.Size(150, 20)
        Me.txtPostCode.TabIndex = 6
        '
        'txtAddress3
        '
        Me.txtAddress3.AcceptsReturn = True
        Me.txtAddress3.BackColor = System.Drawing.SystemColors.Window
        Me.txtAddress3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAddress3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAddress3.Location = New System.Drawing.Point(135, 56)
        Me.txtAddress3.MaxLength = 60
        Me.txtAddress3.Name = "txtAddress3"
        Me.txtAddress3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAddress3.Size = New System.Drawing.Size(208, 20)
        Me.txtAddress3.TabIndex = 3
        '
        'cboState
        '
        Me.cboState.DefaultItemId = 0
        Me.cboState.FirstItem = ""
        Me.cboState.ItemId = 0
        Me.cboState.ListIndex = -1
        Me.cboState.Location = New System.Drawing.Point(135, 79)
        Me.cboState.Name = "cboState"
        Me.cboState.PMLookupProductFamily = 1
        Me.cboState.SingleItemId = 0
        Me.cboState.Size = New System.Drawing.Size(95, 21)
        Me.cboState.Sorted = True
        Me.cboState.TabIndex = 4
        Me.cboState.TableName = "State"
        Me.cboState.ToolTipText = ""
        Me.cboState.WhereClause = "is_deleted = 0"
        '
        'lblStateDesc
        '
        Me.lblStateDesc.BackColor = System.Drawing.SystemColors.Control
        Me.lblStateDesc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblStateDesc.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStateDesc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStateDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStateDesc.Location = New System.Drawing.Point(232, 79)
        Me.lblStateDesc.Name = "lblStateDesc"
        Me.lblStateDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStateDesc.Size = New System.Drawing.Size(111, 21)
        Me.lblStateDesc.TabIndex = 16
        '
        'lblCountry
        '
        Me.lblCountry.AutoSize = True
        Me.lblCountry.BackColor = System.Drawing.SystemColors.Control
        Me.lblCountry.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCountry.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCountry.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCountry.Location = New System.Drawing.Point(12, 127)
        Me.lblCountry.Name = "lblCountry"
        Me.lblCountry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCountry.Size = New System.Drawing.Size(46, 13)
        Me.lblCountry.TabIndex = 15
        Me.lblCountry.Text = "Country:"
        '
        'lblPostCode
        '
        Me.lblPostCode.AutoSize = True
        Me.lblPostCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblPostCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPostCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPostCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPostCode.Location = New System.Drawing.Point(12, 105)
        Me.lblPostCode.Name = "lblPostCode"
        Me.lblPostCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPostCode.Size = New System.Drawing.Size(55, 13)
        Me.lblPostCode.TabIndex = 14
        Me.lblPostCode.Text = "Postcode:"
        '
        'lblAddress1
        '
        Me.lblAddress1.AutoSize = True
        Me.lblAddress1.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddress1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddress1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddress1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddress1.Location = New System.Drawing.Point(12, 19)
        Me.lblAddress1.Name = "lblAddress1"
        Me.lblAddress1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddress1.Size = New System.Drawing.Size(94, 13)
        Me.lblAddress1.TabIndex = 13
        Me.lblAddress1.Text = "No. && street name:"
        '
        'lblAddress2
        '
        Me.lblAddress2.AutoSize = True
        Me.lblAddress2.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddress2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddress2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddress2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddress2.Location = New System.Drawing.Point(12, 40)
        Me.lblAddress2.Name = "lblAddress2"
        Me.lblAddress2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddress2.Size = New System.Drawing.Size(46, 13)
        Me.lblAddress2.TabIndex = 12
        Me.lblAddress2.Text = "Locality:"
        '
        'lblAddress3
        '
        Me.lblAddress3.AutoSize = True
        Me.lblAddress3.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddress3.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddress3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddress3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddress3.Location = New System.Drawing.Point(12, 61)
        Me.lblAddress3.Name = "lblAddress3"
        Me.lblAddress3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddress3.Size = New System.Drawing.Size(37, 13)
        Me.lblAddress3.TabIndex = 11
        Me.lblAddress3.Text = "Town:"
        '
        'lblAddress4
        '
        Me.lblAddress4.AutoSize = True
        Me.lblAddress4.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddress4.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddress4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddress4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddress4.Location = New System.Drawing.Point(12, 83)
        Me.lblAddress4.Name = "lblAddress4"
        Me.lblAddress4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddress4.Size = New System.Drawing.Size(43, 13)
        Me.lblAddress4.TabIndex = 10
        Me.lblAddress4.Text = "County:"
        '
        'uctPMAddressControl
        '
        Me.Controls.Add(Me.fraAddress)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctPMAddressControl"
        Me.Size = New System.Drawing.Size(354, 167)
        Me.fraAddress.ResumeLayout(False)
        Me.fraAddress.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class