<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctClaimRIControlRI2007
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
        Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle17 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle18 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle19 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.grdRI = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me.colPlacement = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colRetained = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colDefault = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colThis = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colLowerLimit = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colUpperLimit = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colSumInsured = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colRTD = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTR = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colPTD = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colThisPayment = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colReTD = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colBalance = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colIncurredTD = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colAgreement = New System.Windows.Forms.DataGridViewTextBoxColumn()
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
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.grdRI.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.grdRI.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.grdRI.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colPlacement, Me.colName, Me.colRetained, Me.colDefault, Me.colThis, Me.colLowerLimit, Me.colUpperLimit, Me.colSumInsured, Me.colRTD, Me.colTR, Me.colPTD, Me.colThisPayment, Me.colReTD, Me.colBalance, Me.colIncurredTD, Me.colAgreement})
        Me.grdRI.ColumnsCount = 16
        Me.grdRI.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        DataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.ButtonFace
        DataGridViewCellStyle16.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.grdRI.DefaultCellStyle = DataGridViewCellStyle16
        Me.grdRI.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        DataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.grdRI.EvenStyle = DataGridViewCellStyle17
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
        DataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.grdRI.OddStyle = DataGridViewCellStyle18
        DataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle19.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle19.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle19.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.grdRI.RowHeadersDefaultCellStyle = DataGridViewCellStyle19
        Me.grdRI.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.grdRI.RowHeightMin = 0
        Me.grdRI.RowsCount = 0
        Me.grdRI.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me.grdRI.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.grdRI.SelectedStyle = Nothing
        Me.grdRI.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.grdRI.SelLength = -1
        Me.grdRI.SelStart = -1
        Me.grdRI.Size = New System.Drawing.Size(761, 335)
        Me.grdRI.TabIndex = 0
        Me.grdRI.ToolTipText = ""
        '
        'colPlacement
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.colPlacement.DefaultCellStyle = DataGridViewCellStyle2
        Me.colPlacement.HeaderText = "Placement"
        Me.colPlacement.Name = "colPlacement"
        Me.colPlacement.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colPlacement.Width = 85
        '
        'colName
        '
        Me.colName.HeaderText = "Name"
        Me.colName.Name = "colName"
        Me.colName.ReadOnly = True
        Me.colName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colRetained
        '
        DataGridViewCellStyle3.Format = "0.00%"
        Me.colRetained.DefaultCellStyle = DataGridViewCellStyle3
        Me.colRetained.HeaderText = "Retained %"
        Me.colRetained.Name = "colRetained"
        Me.colRetained.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colRetained.Width = 90
        '
        'colDefault
        '
        DataGridViewCellStyle4.Format = "0.00%"
        Me.colDefault.DefaultCellStyle = DataGridViewCellStyle4
        Me.colDefault.HeaderText = "Default %"
        Me.colDefault.Name = "colDefault"
        Me.colDefault.ReadOnly = True
        Me.colDefault.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colDefault.Width = 70
        '
        'colThis
        '
        DataGridViewCellStyle5.Format = "0.00%"
        Me.colThis.DefaultCellStyle = DataGridViewCellStyle5
        Me.colThis.HeaderText = "This %"
        Me.colThis.Name = "colThis"
        Me.colThis.ReadOnly = True
        Me.colThis.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colThis.Width = 70
        '
        'colLowerLimit
        '
        DataGridViewCellStyle6.Format = "N2"
        Me.colLowerLimit.DefaultCellStyle = DataGridViewCellStyle6
        Me.colLowerLimit.HeaderText = "Lower Limit"
        Me.colLowerLimit.Name = "colLowerLimit"
        Me.colLowerLimit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colLowerLimit.Width = 80
        '
        'colUpperLimit
        '
        DataGridViewCellStyle7.Format = "N2"
        Me.colUpperLimit.DefaultCellStyle = DataGridViewCellStyle7
        Me.colUpperLimit.HeaderText = "Upper Limit"
        Me.colUpperLimit.Name = "colUpperLimit"
        Me.colUpperLimit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colUpperLimit.Width = 80
        '
        'colSumInsured
        '
        DataGridViewCellStyle8.Format = "N2"
        Me.colSumInsured.DefaultCellStyle = DataGridViewCellStyle8
        Me.colSumInsured.HeaderText = "RI Sum Insured / MPL"
        Me.colSumInsured.Name = "colSumInsured"
        Me.colSumInsured.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colSumInsured.Width = 140
        '
        'colRTD
        '
        DataGridViewCellStyle9.Format = "N2"
        Me.colRTD.DefaultCellStyle = DataGridViewCellStyle9
        Me.colRTD.HeaderText = "Reserve To Date"
        Me.colRTD.Name = "colRTD"
        Me.colRTD.ReadOnly = True
        Me.colRTD.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colRTD.Width = 120
        '
        'colTR
        '
        DataGridViewCellStyle10.Format = "N2"
        Me.colTR.DefaultCellStyle = DataGridViewCellStyle10
        Me.colTR.HeaderText = "This Reserve"
        Me.colTR.Name = "colTR"
        Me.colTR.ReadOnly = True
        Me.colTR.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colPTD
        '
        DataGridViewCellStyle11.Format = "N2"
        Me.colPTD.DefaultCellStyle = DataGridViewCellStyle11
        Me.colPTD.HeaderText = "Payment To Date"
        Me.colPTD.Name = "colPTD"
        Me.colPTD.ReadOnly = True
        Me.colPTD.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colPTD.Width = 125
        '
        'colThisPayment
        '
        DataGridViewCellStyle12.Format = "N2"
        Me.colThisPayment.DefaultCellStyle = DataGridViewCellStyle12
        Me.colThisPayment.HeaderText = "This Payment"
        Me.colThisPayment.Name = "colThisPayment"
        Me.colThisPayment.ReadOnly = True
        Me.colThisPayment.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colThisPayment.Width = 110
        '
        'colReTD
        '
        DataGridViewCellStyle13.Format = "N2"
        Me.colReTD.DefaultCellStyle = DataGridViewCellStyle13
        Me.colReTD.HeaderText = "Recovered To Date"
        Me.colReTD.Name = "colReTD"
        Me.colReTD.ReadOnly = True
        Me.colReTD.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colReTD.Width = 130
        '
        'colBalance
        '
        DataGridViewCellStyle14.Format = "N2"
        Me.colBalance.DefaultCellStyle = DataGridViewCellStyle14
        Me.colBalance.HeaderText = "Balance"
        Me.colBalance.Name = "colBalance"
        Me.colBalance.ReadOnly = True
        Me.colBalance.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colBalance.Width = 80
        '
        'colIncurredTD
        '
        DataGridViewCellStyle15.Format = "N2"
        DataGridViewCellStyle15.NullValue = Nothing
        Me.colIncurredTD.DefaultCellStyle = DataGridViewCellStyle15
        Me.colIncurredTD.HeaderText = "Incurred To Date"
        Me.colIncurredTD.Name = "colIncurredTD"
        Me.colIncurredTD.ReadOnly = True
        Me.colIncurredTD.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colIncurredTD.Width = 130
        '
        'colAgreement
        '
        Me.colAgreement.HeaderText = "Agreement"
        Me.colAgreement.Name = "colAgreement"
        Me.colAgreement.ReadOnly = True
        Me.colAgreement.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colAgreement.Width = 80
        '
        'uctClaimRIControlRI2007
        '
        Me.Controls.Add(Me.grdRI)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctClaimRIControlRI2007"
        Me.Size = New System.Drawing.Size(765, 343)
        CType(Me.grdRI, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents colPlacement As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colRetained As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colDefault As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colThis As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colLowerLimit As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colUpperLimit As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colSumInsured As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colRTD As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colTR As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colPTD As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colThisPayment As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colReTD As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colBalance As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colIncurredTD As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colAgreement As System.Windows.Forms.DataGridViewTextBoxColumn
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
#End Region 
End Class
