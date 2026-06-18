<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctRiskRIControl
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
    Private WithEvents grdRI As Artinsoft.Windows.Forms.ExtendedDataGridView
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.grdRI = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colDef = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colThisPer = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colSumIns = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colPrem = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTax = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colCommPer = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colComm = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colComTax = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colAgreement = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.grdRI, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grdRI
        '
        Me.grdRI.AllowBigSelection = False
        Me.grdRI.AllowRowSelection = False
        Me.grdRI.AllowUserToAddRows = False
        Me.grdRI.AllowUserToDeleteRows = False
        Me.grdRI.AlternatingRows = False
        Me.grdRI.AllowUserToDeleteRows = False
        Me.grdRI.BackColorFixed = System.Drawing.Color.Empty
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.grdRI.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.grdRI.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colName, Me.colDef, Me.colThisPer, Me.colSumIns, Me.colPrem, Me.colTax, Me.colCommPer, Me.colComm, Me.colComTax, Me.colAgreement})
        Me.grdRI.ColumnsCount = 10
        Me.grdRI.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.ButtonFace
        DataGridViewCellStyle11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.grdRI.DefaultCellStyle = DataGridViewCellStyle11
        Me.grdRI.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.grdRI.EvenStyle = DataGridViewCellStyle12
        Me.grdRI.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me.grdRI.FixedColumns = -1
        Me.grdRI.FixedRows = -1
        Me.grdRI.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me.grdRI.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grdRI.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me.grdRI.GridLineWidth = 0
        Me.grdRI.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me.grdRI.Location = New System.Drawing.Point(0, 0)
        Me.grdRI.MultiSelect = False
        Me.grdRI.Name = "grdRI"
        Me.grdRI.OddStyle = DataGridViewCellStyle13
        DataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle14.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.grdRI.RowHeadersDefaultCellStyle = DataGridViewCellStyle14
        Me.grdRI.RowHeightMin = 0
        Me.grdRI.RowsCount = 0
        Me.grdRI.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me.grdRI.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.grdRI.SelectedStyle = Nothing
        Me.grdRI.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.grdRI.SelLength = -1
        Me.grdRI.SelStart = -1
        Me.grdRI.ShowEditingIcon = False
        Me.grdRI.Size = New System.Drawing.Size(741, 327)
        Me.grdRI.TabIndex = 0
        Me.grdRI.ToolTipText = ""
        '
        'colName
        '
        Me.colName.HeaderText = "Name"
        Me.colName.Name = "colName"
        Me.colName.ReadOnly = True
        Me.colName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colDef
        '
        DataGridViewCellStyle2.Format = "0.0%"
        DataGridViewCellStyle2.NullValue = Nothing
        Me.colDef.DefaultCellStyle = DataGridViewCellStyle2
        Me.colDef.HeaderText = "Default %"
        Me.colDef.Name = "colDef"
        Me.colDef.ReadOnly = True
        Me.colDef.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colThisPer
        '
        DataGridViewCellStyle3.Format = "0.0%"
        DataGridViewCellStyle3.NullValue = Nothing
        Me.colThisPer.DefaultCellStyle = DataGridViewCellStyle3
        Me.colThisPer.HeaderText = "This %"
        Me.colThisPer.Name = "colThisPer"
        Me.colThisPer.ReadOnly = True
        Me.colThisPer.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colSumIns
        '
        DataGridViewCellStyle4.Format = "N2"
        DataGridViewCellStyle4.NullValue = Nothing
        Me.colSumIns.DefaultCellStyle = DataGridViewCellStyle4
        Me.colSumIns.HeaderText = "Sum Insured"
        Me.colSumIns.Name = "colSumIns"
        Me.colSumIns.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colPrem
        '
        DataGridViewCellStyle5.Format = "N2"
        DataGridViewCellStyle5.NullValue = Nothing
        Me.colPrem.DefaultCellStyle = DataGridViewCellStyle5
        Me.colPrem.HeaderText = "Premium"
        Me.colPrem.Name = "colPrem"
        Me.colPrem.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colTax
        '
        DataGridViewCellStyle6.Format = "N2"
        DataGridViewCellStyle6.NullValue = Nothing
        Me.colTax.DefaultCellStyle = DataGridViewCellStyle6
        Me.colTax.HeaderText = "Tax"
        Me.colTax.Name = "colTax"
        Me.colTax.ReadOnly = True
        Me.colTax.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colCommPer
        '
        DataGridViewCellStyle7.Format = "0.0%"
        DataGridViewCellStyle7.NullValue = Nothing
        Me.colCommPer.DefaultCellStyle = DataGridViewCellStyle7
        Me.colCommPer.HeaderText = "Comm %"
        Me.colCommPer.Name = "colCommPer"
        Me.colCommPer.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colComm
        '
        DataGridViewCellStyle8.Format = "N2"
        DataGridViewCellStyle8.NullValue = Nothing
        Me.colComm.DefaultCellStyle = DataGridViewCellStyle8
        Me.colComm.HeaderText = "Commission"
        Me.colComm.Name = "colComm"
        Me.colComm.ReadOnly = True
        Me.colComm.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colComTax
        '
        DataGridViewCellStyle9.Format = "N2"
        DataGridViewCellStyle9.NullValue = Nothing
        Me.colComTax.DefaultCellStyle = DataGridViewCellStyle9
        Me.colComTax.HeaderText = "Comm Tax"
        Me.colComTax.Name = "colComTax"
        Me.colComTax.ReadOnly = True
        Me.colComTax.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colAgreement
        '
        DataGridViewCellStyle10.NullValue = Nothing
        Me.colAgreement.DefaultCellStyle = DataGridViewCellStyle10
        Me.colAgreement.HeaderText = "Agreement"
        Me.colAgreement.Name = "colAgreement"
        Me.colAgreement.ReadOnly = True
        Me.colAgreement.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'uctRiskRIControl
        '
        Me.Controls.Add(Me.grdRI)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctRiskRIControl"
        Me.Size = New System.Drawing.Size(741, 327)
        CType(Me.grdRI, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents colName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colDef As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colThisPer As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colSumIns As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colPrem As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colTax As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colCommPer As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colComm As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colComTax As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colAgreement As System.Windows.Forms.DataGridViewTextBoxColumn
    Private WithEvents ToolTip1 As System.Windows.Forms.ToolTip
#End Region
#Region "Upgrade Support"
	<System.Runtime.InteropServices.ProgId("RecalculateFacTaxEventArgs_NET.RecalculateFacTaxEventArgs")> _
	Public NotInheritable Class RecalculateFacTaxEventArgs
		Inherits System.EventArgs
		Public lArrangementLineID As Integer
		Public lPartyCnt As Integer
		Public cPremium As Decimal
		Public cCommission As Decimal
		Public cPremiumTax As Decimal
		Public cCommTax As Decimal
		Public Sub New(ByVal lArrangementLineID As Integer, ByVal lPartyCnt As Integer, ByVal cPremium As Decimal, ByVal cCommission As Decimal, ByRef cPremiumTax As Decimal, ByRef cCommTax As Decimal)
			MyBase.New()
			Me.lArrangementLineID = lArrangementLineID
			Me.lPartyCnt = lPartyCnt
			Me.cPremium = cPremium
			Me.cCommission = cCommission
			Me.cPremiumTax = cPremiumTax
			Me.cCommTax = cCommTax
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("RecalculateTreatyTaxEventArgs_NET.RecalculateTreatyTaxEventArgs")> _
	Public NotInheritable Class RecalculateTreatyTaxEventArgs
		Inherits System.EventArgs
		Public lArrangementLineID As Integer
		Public lTreatyID As Integer
		Public cPremium As Decimal
		Public cCommission As Decimal
		Public cPremiumTax As Decimal
		Public cCommTax As Decimal
		Public Sub New(ByVal lArrangementLineID As Integer, ByVal lTreatyID As Integer, ByVal cPremium As Decimal, ByVal cCommission As Decimal, ByRef cPremiumTax As Decimal, ByRef cCommTax As Decimal)
			MyBase.New()
			Me.lArrangementLineID = lArrangementLineID
			Me.lTreatyID = lTreatyID
			Me.cPremium = cPremium
			Me.cCommission = cCommission
			Me.cPremiumTax = cPremiumTax
			Me.cCommTax = cCommTax
		End Sub
	End Class
#End Region 
End Class
