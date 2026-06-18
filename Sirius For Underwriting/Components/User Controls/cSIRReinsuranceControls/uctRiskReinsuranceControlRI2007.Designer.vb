<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctRiskRIControlRI2007
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
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
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
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.grdRI = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me.colPlacement = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colRetained = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colDefaultPer = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colThisPer = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colLowerLimit = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colUpperLimit = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colSumIns = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colPrem = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colTax = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colCommPer = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colCommisiion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colCommTax = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colAgreementCode = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colApproved = New System.Windows.Forms.DataGridViewTextBoxColumn
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
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.grdRI.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.grdRI.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colPlacement, Me.colName, Me.colRetained, Me.colDefaultPer, Me.colThisPer, Me.colLowerLimit, Me.colUpperLimit, Me.colSumIns, Me.colPrem, Me.colTax, Me.colCommPer, Me.colCommisiion, Me.colCommTax, Me.colAgreementCode, Me.colApproved})
        Me.grdRI.ColumnsCount = 15
        Me.grdRI.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.ButtonFace
        DataGridViewCellStyle13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.grdRI.DefaultCellStyle = DataGridViewCellStyle13
        Me.grdRI.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.grdRI.EvenStyle = DataGridViewCellStyle14
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
        Me.grdRI.OddStyle = DataGridViewCellStyle15
        DataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle16.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.grdRI.RowHeadersDefaultCellStyle = DataGridViewCellStyle16
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
        'colPlacement
        '
        Me.colPlacement.HeaderText = "Placement"
        Me.colPlacement.Name = "colPlacement"
        Me.colPlacement.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colPlacement.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        '
        'colName
        '
        Me.colName.HeaderText = "Name"
        Me.colName.Name = "colName"
        Me.colName.ReadOnly = True
        Me.colName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colName.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        '
        'colRetained
        '
        DataGridViewCellStyle2.Format = "0.00%"
        Me.colRetained.DefaultCellStyle = DataGridViewCellStyle2
        Me.colRetained.HeaderText = "Retained %"
        Me.colRetained.Name = "colRetained"
        Me.colRetained.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colDefaultPer
        '
        DataGridViewCellStyle3.Format = "0.00%"
        Me.colDefaultPer.DefaultCellStyle = DataGridViewCellStyle3
        Me.colDefaultPer.HeaderText = "Default %"
        Me.colDefaultPer.Name = "colDefaultPer"
        Me.colDefaultPer.ReadOnly = True
        Me.colDefaultPer.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colDefaultPer.Width = 110
        '
        'colThisPer
        '
        DataGridViewCellStyle4.Format = "N4"
        Me.colThisPer.DefaultCellStyle = DataGridViewCellStyle4
        Me.colThisPer.HeaderText = "This %"
        Me.colThisPer.Name = "colThisPer"
        Me.colThisPer.ReadOnly = True
        Me.colThisPer.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colLowerLimit
        '
        DataGridViewCellStyle5.Format = "N2"
        Me.colLowerLimit.DefaultCellStyle = DataGridViewCellStyle5
        Me.colLowerLimit.HeaderText = "Lower Limit"
        Me.colLowerLimit.Name = "colLowerLimit"
        Me.colLowerLimit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colUpperLimit
        '
        DataGridViewCellStyle6.Format = "N2"
        Me.colUpperLimit.DefaultCellStyle = DataGridViewCellStyle6
        Me.colUpperLimit.HeaderText = "Upper Limit"
        Me.colUpperLimit.Name = "colUpperLimit"
        Me.colUpperLimit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colSumIns
        '
        DataGridViewCellStyle7.Format = "N2"
        Me.colSumIns.DefaultCellStyle = DataGridViewCellStyle7
        Me.colSumIns.HeaderText = "RI Sum Insured / MPL"
        Me.colSumIns.Name = "colSumIns"
        Me.colSumIns.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colSumIns.Width = 140
        '
        'colPrem
        '
        DataGridViewCellStyle8.Format = "N2"
        Me.colPrem.DefaultCellStyle = DataGridViewCellStyle8
        Me.colPrem.HeaderText = "Premium"
        Me.colPrem.Name = "colPrem"
        Me.colPrem.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colTax
        '
        DataGridViewCellStyle9.Format = "N2"
        Me.colTax.DefaultCellStyle = DataGridViewCellStyle9
        Me.colTax.HeaderText = "Tax"
        Me.colTax.Name = "colTax"
        Me.colTax.ReadOnly = True
        Me.colTax.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colCommPer
        '
        DataGridViewCellStyle10.Format = "0.00%"
        Me.colCommPer.DefaultCellStyle = DataGridViewCellStyle10
        Me.colCommPer.HeaderText = "Comm %"
        Me.colCommPer.Name = "colCommPer"
        Me.colCommPer.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colCommisiion
        '
        DataGridViewCellStyle11.Format = "N2"
        Me.colCommisiion.DefaultCellStyle = DataGridViewCellStyle11
        Me.colCommisiion.HeaderText = "Commission"
        Me.colCommisiion.Name = "colCommisiion"
        Me.colCommisiion.ReadOnly = True
        Me.colCommisiion.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colCommTax
        '
        DataGridViewCellStyle12.Format = "N2"
        Me.colCommTax.DefaultCellStyle = DataGridViewCellStyle12
        Me.colCommTax.HeaderText = "Comm Tax"
        Me.colCommTax.Name = "colCommTax"
        Me.colCommTax.ReadOnly = True
        Me.colCommTax.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colAgreementCode
        '
        Me.colAgreementCode.HeaderText = "Agreement Code"
        Me.colAgreementCode.Name = "colAgreementCode"
        Me.colAgreementCode.ReadOnly = True
        Me.colAgreementCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colAgreementCode.Width = 120

        'colApproved
        '
        Me.colApproved.HeaderText = "Approved"
        Me.colApproved.Name = "colApproved"
        Me.colApproved.ReadOnly = True
        Me.colApproved.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colApproved.Width = 100
        '
        'uctRiskRIControlRI2007
        '
        Me.Controls.Add(Me.grdRI)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctRiskRIControlRI2007"
        Me.Size = New System.Drawing.Size(741, 327)
        CType(Me.grdRI, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents colPlacement As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colRetained As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colDefaultPer As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colThisPer As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colLowerLimit As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colUpperLimit As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colSumIns As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colPrem As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colTax As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colCommPer As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colCommisiion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colCommTax As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colAgreementCode As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colApproved As System.Windows.Forms.DataGridViewTextBoxColumn
#End Region
#Region "Upgrade Support"
	<System.Runtime.InteropServices.ProgId("ResetControlsEventArgs_NET.ResetControlsEventArgs")> _
	Public NotInheritable Class ResetControlsEventArgs
		Inherits System.EventArgs
		Public SelRIType As String = ""
		Public Sub New(ByVal SelRIType As String)
			MyBase.New()
			Me.SelRIType = SelRIType
		End Sub
	End Class
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
