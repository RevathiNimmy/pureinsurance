<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctCLMReserve
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwCoinsurers_InitializeColumnKeys()
		UserControl_InitProperties()
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Friend WithEvents txtFormatPercentage As System.Windows.Forms.TextBox
	Friend WithEvents txtTotalAllocated As System.Windows.Forms.TextBox
	Friend WithEvents txtPercentageAllocated As System.Windows.Forms.TextBox
	Friend WithEvents _lvwCoinsurers_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwCoinsurers_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwCoinsurers_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwCoinsurers_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lvwCoinsurers As System.Windows.Forms.ListView
	Friend WithEvents lblTotalAllocated As System.Windows.Forms.Label
	Friend WithEvents lblPercentageAllocated As System.Windows.Forms.Label
	Friend WithEvents fraCoInsurers As System.Windows.Forms.GroupBox
    Friend WithEvents lstviewReserve As System.Windows.Forms.ListView
    Friend WithEvents fraReserveDetails As System.Windows.Forms.GroupBox
    'TODOLIST-Commented the listviewhelper as it was conflicting with icon display in listview
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.fraCoInsurers = New System.Windows.Forms.GroupBox
        Me.txtFormatPercentage = New System.Windows.Forms.TextBox
        Me.txtTotalAllocated = New System.Windows.Forms.TextBox
        Me.txtPercentageAllocated = New System.Windows.Forms.TextBox
        Me.lvwCoinsurers = New System.Windows.Forms.ListView
        Me._lvwCoinsurers_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwCoinsurers_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwCoinsurers_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwCoinsurers_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me.lblTotalAllocated = New System.Windows.Forms.Label
        Me.lblPercentageAllocated = New System.Windows.Forms.Label
        Me.fraReserveDetails = New System.Windows.Forms.GroupBox
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.lstviewReserve = New System.Windows.Forms.ListView
        Me.fraCoInsurers.SuspendLayout()
        Me.fraReserveDetails.SuspendLayout()
        Me.SuspendLayout()
        '
        'fraCoInsurers
        '
        Me.fraCoInsurers.BackColor = System.Drawing.SystemColors.Control
        Me.fraCoInsurers.Controls.Add(Me.txtFormatPercentage)
        Me.fraCoInsurers.Controls.Add(Me.txtTotalAllocated)
        Me.fraCoInsurers.Controls.Add(Me.txtPercentageAllocated)
        Me.fraCoInsurers.Controls.Add(Me.lvwCoinsurers)
        Me.fraCoInsurers.Controls.Add(Me.lblTotalAllocated)
        Me.fraCoInsurers.Controls.Add(Me.lblPercentageAllocated)
        Me.fraCoInsurers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCoInsurers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraCoInsurers.Location = New System.Drawing.Point(0, 200)
        Me.fraCoInsurers.Name = "fraCoInsurers"
        Me.fraCoInsurers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraCoInsurers.Size = New System.Drawing.Size(737, 177)
        Me.fraCoInsurers.TabIndex = 3
        Me.fraCoInsurers.TabStop = False
        Me.fraCoInsurers.Text = "Co-Insurer Details"
        '
        'txtFormatPercentage
        '
        Me.txtFormatPercentage.AcceptsReturn = True
        Me.txtFormatPercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtFormatPercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFormatPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFormatPercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFormatPercentage.Location = New System.Drawing.Point(416, 16)
        Me.txtFormatPercentage.MaxLength = 0
        Me.txtFormatPercentage.Name = "txtFormatPercentage"
        Me.txtFormatPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFormatPercentage.Size = New System.Drawing.Size(113, 20)
        Me.txtFormatPercentage.TabIndex = 9
        Me.txtFormatPercentage.Visible = False
        '
        'txtTotalAllocated
        '
        Me.txtTotalAllocated.AcceptsReturn = True
        Me.txtTotalAllocated.BackColor = System.Drawing.SystemColors.Window
        Me.txtTotalAllocated.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalAllocated.Enabled = False
        Me.txtTotalAllocated.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalAllocated.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalAllocated.Location = New System.Drawing.Point(256, 16)
        Me.txtTotalAllocated.MaxLength = 0
        Me.txtTotalAllocated.Name = "txtTotalAllocated"
        Me.txtTotalAllocated.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalAllocated.Size = New System.Drawing.Size(105, 20)
        Me.txtTotalAllocated.TabIndex = 8
        Me.txtTotalAllocated.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtPercentageAllocated
        '
        Me.txtPercentageAllocated.AcceptsReturn = True
        Me.txtPercentageAllocated.BackColor = System.Drawing.SystemColors.Window
        Me.txtPercentageAllocated.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPercentageAllocated.Enabled = False
        Me.txtPercentageAllocated.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPercentageAllocated.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPercentageAllocated.Location = New System.Drawing.Point(88, 16)
        Me.txtPercentageAllocated.MaxLength = 0
        Me.txtPercentageAllocated.Name = "txtPercentageAllocated"
        Me.txtPercentageAllocated.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPercentageAllocated.Size = New System.Drawing.Size(65, 20)
        Me.txtPercentageAllocated.TabIndex = 7
        Me.txtPercentageAllocated.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lvwCoinsurers
        '
        Me.lvwCoinsurers.BackColor = System.Drawing.SystemColors.Window
        Me.lvwCoinsurers.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwCoinsurers_ColumnHeader_1, Me._lvwCoinsurers_ColumnHeader_2, Me._lvwCoinsurers_ColumnHeader_3, Me._lvwCoinsurers_ColumnHeader_4})
        Me.lvwCoinsurers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwCoinsurers.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwCoinsurers.Location = New System.Drawing.Point(8, 48)
        Me.lvwCoinsurers.Name = "lvwCoinsurers"
        Me.lvwCoinsurers.Size = New System.Drawing.Size(705, 121)
        Me.lvwCoinsurers.TabIndex = 4
        Me.lvwCoinsurers.UseCompatibleStateImageBehavior = False
        Me.lvwCoinsurers.View = System.Windows.Forms.View.Details
        '
        '_lvwCoinsurers_ColumnHeader_1
        '
        Me._lvwCoinsurers_ColumnHeader_1.Tag = ""
        Me._lvwCoinsurers_ColumnHeader_1.Text = "Insurer"
        Me._lvwCoinsurers_ColumnHeader_1.Width = 167
        '
        '_lvwCoinsurers_ColumnHeader_2
        '
        Me._lvwCoinsurers_ColumnHeader_2.Tag = ""
        Me._lvwCoinsurers_ColumnHeader_2.Text = "% of Reserve"
        Me._lvwCoinsurers_ColumnHeader_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwCoinsurers_ColumnHeader_2.Width = 67
        '
        '_lvwCoinsurers_ColumnHeader_3
        '
        Me._lvwCoinsurers_ColumnHeader_3.Tag = ""
        Me._lvwCoinsurers_ColumnHeader_3.Text = "Total Reserve"
        Me._lvwCoinsurers_ColumnHeader_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwCoinsurers_ColumnHeader_3.Width = 67
        '
        '_lvwCoinsurers_ColumnHeader_4
        '
        Me._lvwCoinsurers_ColumnHeader_4.Tag = ""
        Me._lvwCoinsurers_ColumnHeader_4.Text = "Total This Reserve Allocated"
        Me._lvwCoinsurers_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwCoinsurers_ColumnHeader_4.Width = 134
        '
        'lblTotalAllocated
        '
        Me.lblTotalAllocated.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalAllocated.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalAllocated.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalAllocated.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalAllocated.Location = New System.Drawing.Point(168, 24)
        Me.lblTotalAllocated.Name = "lblTotalAllocated"
        Me.lblTotalAllocated.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalAllocated.Size = New System.Drawing.Size(97, 17)
        Me.lblTotalAllocated.TabIndex = 6
        Me.lblTotalAllocated.Text = "Total Allocated:"
        '
        'lblPercentageAllocated
        '
        Me.lblPercentageAllocated.BackColor = System.Drawing.SystemColors.Control
        Me.lblPercentageAllocated.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPercentageAllocated.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPercentageAllocated.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPercentageAllocated.Location = New System.Drawing.Point(16, 24)
        Me.lblPercentageAllocated.Name = "lblPercentageAllocated"
        Me.lblPercentageAllocated.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPercentageAllocated.Size = New System.Drawing.Size(73, 17)
        Me.lblPercentageAllocated.TabIndex = 5
        Me.lblPercentageAllocated.Text = "% Allocated:"
        '
        'fraReserveDetails
        '
        Me.fraReserveDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraReserveDetails.Controls.Add(Me.cmdEdit)
        Me.fraReserveDetails.Controls.Add(Me.lstviewReserve)
        Me.fraReserveDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraReserveDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraReserveDetails.Location = New System.Drawing.Point(0, 0)
        Me.fraReserveDetails.Name = "fraReserveDetails"
        Me.fraReserveDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraReserveDetails.Size = New System.Drawing.Size(737, 193)
        Me.fraReserveDetails.TabIndex = 0
        Me.fraReserveDetails.TabStop = False
        Me.fraReserveDetails.Text = "Reserve Details"
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(656, 8)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 1
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'lstviewReserve
        '
        Me.lstviewReserve.BackColor = System.Drawing.SystemColors.Window
        Me.lstviewReserve.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstviewReserve.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lstviewReserve.GridLines = True
        Me.lstviewReserve.Location = New System.Drawing.Point(8, 16)
        Me.lstviewReserve.Name = "lstviewReserve"
        Me.lstviewReserve.Size = New System.Drawing.Size(641, 169)
        Me.lstviewReserve.TabIndex = 2
        Me.lstviewReserve.UseCompatibleStateImageBehavior = False
        Me.lstviewReserve.View = System.Windows.Forms.View.Details
        '
        'uctCLMReserve
        '
        Me.Controls.Add(Me.fraCoInsurers)
        Me.Controls.Add(Me.fraReserveDetails)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctCLMReserve"
        Me.Size = New System.Drawing.Size(740, 385)
        Me.fraCoInsurers.ResumeLayout(False)
        Me.fraCoInsurers.PerformLayout()
        Me.fraReserveDetails.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
	Sub lvwCoinsurers_InitializeColumnKeys()
		Me._lvwCoinsurers_ColumnHeader_1.Name = ""
		Me._lvwCoinsurers_ColumnHeader_2.Name = ""
		Me._lvwCoinsurers_ColumnHeader_3.Name = ""
		Me._lvwCoinsurers_ColumnHeader_4.Name = ""
    End Sub
    Public WithEvents cmdEdit As System.Windows.Forms.Button
#End Region 
#Region "Upgrade Support"
	<System.Runtime.InteropServices.ProgId("DataHasChangedEventArgs_NET.DataHasChangedEventArgs")> _
	Public NotInheritable Class DataHasChangedEventArgs
		Inherits System.EventArgs
        Public NewData As Object
        Public Sub New(ByRef NewData As Object)
            MyBase.New()
            Me.NewData = NewData
        End Sub
	End Class
#End Region 
End Class