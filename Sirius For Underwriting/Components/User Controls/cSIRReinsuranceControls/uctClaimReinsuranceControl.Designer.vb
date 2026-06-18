<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctClaimRIControl
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
    Private WithEvents grdRI As Artinsoft.Windows.Forms.ExtendedDataGridView
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.grdRI = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colDef = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colThis = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colSI = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colRTD = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colTR = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colPTD = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colTP = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colBal = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colAgr = New System.Windows.Forms.DataGridViewTextBoxColumn
        CType(Me.grdRI, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grdRI
        '
        Me.grdRI.AllowBigSelection = False
        Me.grdRI.AllowRowSelection = False
        Me.grdRI.AllowUserToAddRows = False
        Me.grdRI.AlternatingRows = False
        Me.grdRI.AllowUserToDeleteRows = False
        Me.grdRI.BackColorFixed = System.Drawing.Color.Empty
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
           Me.grdRI.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.grdRI.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colName, Me.colDef, Me.colThis, Me.colSI, Me.colRTD, Me.colTR, Me.colPTD, Me.colTP, Me.colBal, Me.colAgr})
        Me.grdRI.ColumnsCount = 10
        Me.grdRI.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.ButtonFace
        DataGridViewCellStyle12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.grdRI.DefaultCellStyle = DataGridViewCellStyle12
        Me.grdRI.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.grdRI.EvenStyle = DataGridViewCellStyle13
        Me.grdRI.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me.grdRI.FixedColumns = -1
        Me.grdRI.FixedRows = -1
        Me.grdRI.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me.grdRI.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grdRI.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me.grdRI.GridLineWidth = 0
        Me.grdRI.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me.grdRI.Location = New System.Drawing.Point(0, 0)
        Me.grdRI.Name = "grdRI"
        Me.grdRI.OddStyle = DataGridViewCellStyle14
        DataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle15.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.grdRI.RowHeadersDefaultCellStyle = DataGridViewCellStyle15
        Me.grdRI.RowHeightMin = 0
        Me.grdRI.RowsCount = 0
        Me.grdRI.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me.grdRI.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.grdRI.SelectedStyle = Nothing
        Me.grdRI.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.grdRI.SelLength = -1
        Me.grdRI.SelStart = -1
        Me.grdRI.Size = New System.Drawing.Size(741, 327)
        Me.grdRI.TabIndex = 0
        Me.grdRI.ToolTipText = ""
        '
        'colName
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.colName.DefaultCellStyle = DataGridViewCellStyle2
        Me.colName.HeaderText = "Name"
        Me.colName.Name = "colName"
        Me.colName.ReadOnly = True
        Me.colName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colName.Width = 150
        '
        'colDef
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle3.Format = "0.00%"
        DataGridViewCellStyle3.NullValue = Nothing
        Me.colDef.DefaultCellStyle = DataGridViewCellStyle3
        Me.colDef.HeaderText = "Default%"
        Me.colDef.Name = "colDef"
        Me.colDef.ReadOnly = True
        Me.colDef.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colThis
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle4.Format = "0.00%"
        DataGridViewCellStyle4.NullValue = Nothing
        Me.colThis.DefaultCellStyle = DataGridViewCellStyle4
        Me.colThis.HeaderText = "This%"
        Me.colThis.Name = "colThis"
        Me.colThis.ReadOnly = True
        Me.colThis.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colSI
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle5.Format = "N2"
        DataGridViewCellStyle5.NullValue = Nothing
        Me.colSI.DefaultCellStyle = DataGridViewCellStyle5
        Me.colSI.HeaderText = "Sum Insured"
        Me.colSI.Name = "colSI"
        Me.colSI.ReadOnly = True
        Me.colSI.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colRTD
        '
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle6.Format = "N2"
        DataGridViewCellStyle6.NullValue = Nothing
        Me.colRTD.DefaultCellStyle = DataGridViewCellStyle6
        Me.colRTD.HeaderText = "Reserve To Date"
        Me.colRTD.Name = "colRTD"
        Me.colRTD.ReadOnly = True
        Me.colRTD.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colTR
        '
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle7.Format = "N2"
        DataGridViewCellStyle7.NullValue = Nothing
        Me.colTR.DefaultCellStyle = DataGridViewCellStyle7
        Me.colTR.HeaderText = "This Reserve"
        Me.colTR.Name = "colTR"
        Me.colTR.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colPTD
        '
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle8.Format = "N2"
        DataGridViewCellStyle8.NullValue = Nothing
        Me.colPTD.DefaultCellStyle = DataGridViewCellStyle8
        Me.colPTD.HeaderText = "Payment To Date"
        Me.colPTD.Name = "colPTD"
        Me.colPTD.ReadOnly = True
        Me.colPTD.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colTP
        '
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle9.Format = "N2"
        DataGridViewCellStyle9.NullValue = Nothing
        Me.colTP.DefaultCellStyle = DataGridViewCellStyle9
        Me.colTP.HeaderText = "This Payment"
        Me.colTP.Name = "colTP"
        Me.colTP.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colBal
        '
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle10.Format = "N2"
        DataGridViewCellStyle10.NullValue = Nothing
        Me.colBal.DefaultCellStyle = DataGridViewCellStyle10
        Me.colBal.HeaderText = "Balance"
        Me.colBal.Name = "colBal"
        Me.colBal.ReadOnly = True
        Me.colBal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colAgr
        '
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.colAgr.DefaultCellStyle = DataGridViewCellStyle11
        Me.colAgr.HeaderText = "Agreement"
        Me.colAgr.Name = "colAgr"
        Me.colAgr.ReadOnly = True
        Me.colAgr.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'uctClaimRIControl
        '
        Me.Controls.Add(Me.grdRI)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctClaimRIControl"
        Me.Size = New System.Drawing.Size(765, 343)
        CType(Me.grdRI, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents colName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colDef As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colThis As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colSI As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colRTD As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colTR As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colPTD As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colTP As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colBal As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colAgr As System.Windows.Forms.DataGridViewTextBoxColumn
#End Region
End Class
