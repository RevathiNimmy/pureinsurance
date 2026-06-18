<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
	End Sub
	Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
		Dispose(True)
	End Sub
	Dim fTerminateCalled_Form_Terminate_Renamed As Boolean
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
	 Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not fTerminateCalled_Form_Terminate_Renamed Then
				fTerminateCalled_Form_Terminate_Renamed = True
				Form_Terminate_Renamed()
			End If
			If Not (components Is Nothing) Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents cboWriteOffReasonId As System.Windows.Forms.ComboBox
	Public WithEvents cmdRemove As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblAllocatedTotal As System.Windows.Forms.Label
	Public WithEvents Line2 As System.Windows.Forms.Label
	Public WithEvents line1 As System.Windows.Forms.Label
	Public WithEvents lblWriteOffReasonID As System.Windows.Forms.Label
	Public WithEvents lblBal As System.Windows.Forms.Label
	Public WithEvents lblCurrencyTotal As System.Windows.Forms.Label
	Public WithEvents lblWriteOffTotal As System.Windows.Forms.Label
	Public WithEvents lblBalance As System.Windows.Forms.Label
	Public WithEvents lineBottom As System.Windows.Forms.Label
	Public WithEvents lblTotals As System.Windows.Forms.Label
	Public WithEvents lblOSTotal As System.Windows.Forms.Label
	Public WithEvents lblBaseCurrency As System.Windows.Forms.Label
	Public WithEvents lblTransactionCurrency As System.Windows.Forms.Label
    Private WithEvents tdbGrid As Artinsoft.Windows.Forms.ExtendedDataGridView
    'Private WithEvents tdbGrid As Artinsoft.VB6.Gui.Splits
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cboWriteOffReasonId = New System.Windows.Forms.ComboBox
        Me.cmdRemove = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.lblAllocatedTotal = New System.Windows.Forms.Label
        Me.Line2 = New System.Windows.Forms.Label
        Me.line1 = New System.Windows.Forms.Label
        Me.lblWriteOffReasonID = New System.Windows.Forms.Label
        Me.lblBal = New System.Windows.Forms.Label
        Me.lblCurrencyTotal = New System.Windows.Forms.Label
        Me.lblWriteOffTotal = New System.Windows.Forms.Label
        Me.lblBalance = New System.Windows.Forms.Label
        Me.lineBottom = New System.Windows.Forms.Label
        Me.lblTotals = New System.Windows.Forms.Label
        Me.lblOSTotal = New System.Windows.Forms.Label
        Me.lblBaseCurrency = New System.Windows.Forms.Label
        Me.lblTransactionCurrency = New System.Windows.Forms.Label
        Me.tdbGrid = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        CType(Me.tdbGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cboWriteOffReasonId
        '
        Me.cboWriteOffReasonId.BackColor = System.Drawing.SystemColors.Window
        Me.cboWriteOffReasonId.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboWriteOffReasonId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboWriteOffReasonId.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboWriteOffReasonId.Location = New System.Drawing.Point(528, 304)
        Me.cboWriteOffReasonId.Name = "cboWriteOffReasonId"
        Me.cboWriteOffReasonId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboWriteOffReasonId.Size = New System.Drawing.Size(169, 21)
        Me.cboWriteOffReasonId.TabIndex = 15
        '
        'cmdRemove
        '
        Me.cmdRemove.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRemove.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRemove.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRemove.Location = New System.Drawing.Point(88, 254)
        Me.cmdRemove.Name = "cmdRemove"
        Me.cmdRemove.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRemove.Size = New System.Drawing.Size(73, 22)
        Me.cmdRemove.TabIndex = 12
        Me.cmdRemove.TabStop = False
        Me.cmdRemove.Text = "&Remove"
        Me.cmdRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRemove.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(8, 254)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 11
        Me.cmdAdd.TabStop = False
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(643, 342)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 4
        Me.cmdCancel.TabStop = False
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
        Me.cmdOK.Location = New System.Drawing.Point(564, 342)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 3
        Me.cmdOK.TabStop = False
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'lblAllocatedTotal
        '
        Me.lblAllocatedTotal.BackColor = System.Drawing.SystemColors.Control
        Me.lblAllocatedTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblAllocatedTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAllocatedTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAllocatedTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAllocatedTotal.Location = New System.Drawing.Point(426, 252)
        Me.lblAllocatedTotal.Name = "lblAllocatedTotal"
        Me.lblAllocatedTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAllocatedTotal.Size = New System.Drawing.Size(90, 19)
        Me.lblAllocatedTotal.TabIndex = 14
        Me.lblAllocatedTotal.Text = "Totals"
        Me.lblAllocatedTotal.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Line2
        '
        Me.Line2.BackColor = System.Drawing.SystemColors.WindowText
        Me.Line2.Location = New System.Drawing.Point(384, 15)
        Me.Line2.Name = "Line2"
        Me.Line2.Size = New System.Drawing.Size(53, 1)
        Me.Line2.TabIndex = 16
        '
        'line1
        '
        Me.line1.BackColor = System.Drawing.SystemColors.WindowText
        Me.line1.Location = New System.Drawing.Point(144, 15)
        Me.line1.Name = "line1"
        Me.line1.Size = New System.Drawing.Size(74, 1)
        Me.line1.TabIndex = 17
        '
        'lblWriteOffReasonID
        '
        Me.lblWriteOffReasonID.AutoSize = True
        Me.lblWriteOffReasonID.BackColor = System.Drawing.SystemColors.Control
        Me.lblWriteOffReasonID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblWriteOffReasonID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWriteOffReasonID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblWriteOffReasonID.Location = New System.Drawing.Point(421, 304)
        Me.lblWriteOffReasonID.Name = "lblWriteOffReasonID"
        Me.lblWriteOffReasonID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblWriteOffReasonID.Size = New System.Drawing.Size(92, 13)
        Me.lblWriteOffReasonID.TabIndex = 13
        Me.lblWriteOffReasonID.Text = "Write Off Reason:"
        Me.lblWriteOffReasonID.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblBal
        '
        Me.lblBal.AutoSize = True
        Me.lblBal.BackColor = System.Drawing.SystemColors.Control
        Me.lblBal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBal.Location = New System.Drawing.Point(551, 278)
        Me.lblBal.Name = "lblBal"
        Me.lblBal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBal.Size = New System.Drawing.Size(62, 13)
        Me.lblBal.TabIndex = 10
        Me.lblBal.Text = "Balance:"
        Me.lblBal.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblCurrencyTotal
        '
        Me.lblCurrencyTotal.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrencyTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblCurrencyTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrencyTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrencyTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrencyTotal.Location = New System.Drawing.Point(610, 252)
        Me.lblCurrencyTotal.Name = "lblCurrencyTotal"
        Me.lblCurrencyTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrencyTotal.Size = New System.Drawing.Size(90, 19)
        Me.lblCurrencyTotal.TabIndex = 9
        Me.lblCurrencyTotal.Text = "Totals"
        Me.lblCurrencyTotal.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblWriteOffTotal
        '
        Me.lblWriteOffTotal.BackColor = System.Drawing.SystemColors.Control
        Me.lblWriteOffTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWriteOffTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblWriteOffTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWriteOffTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblWriteOffTotal.Location = New System.Drawing.Point(518, 252)
        Me.lblWriteOffTotal.Name = "lblWriteOffTotal"
        Me.lblWriteOffTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblWriteOffTotal.Size = New System.Drawing.Size(90, 19)
        Me.lblWriteOffTotal.TabIndex = 8
        Me.lblWriteOffTotal.Text = "Totals"
        Me.lblWriteOffTotal.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblBalance
        '
        Me.lblBalance.BackColor = System.Drawing.SystemColors.Control
        Me.lblBalance.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblBalance.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBalance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBalance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBalance.Location = New System.Drawing.Point(610, 276)
        Me.lblBalance.Name = "lblBalance"
        Me.lblBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBalance.Size = New System.Drawing.Size(90, 19)
        Me.lblBalance.TabIndex = 7
        Me.lblBalance.Text = "Totals"
        Me.lblBalance.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lineBottom
        '
        Me.lineBottom.BackColor = System.Drawing.SystemColors.WindowText
        Me.lineBottom.Location = New System.Drawing.Point(8, 332)
        Me.lineBottom.Name = "lineBottom"
        Me.lineBottom.Size = New System.Drawing.Size(708, 1)
        Me.lineBottom.TabIndex = 18
        '
        'lblTotals
        '
        Me.lblTotals.AutoSize = True
        Me.lblTotals.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotals.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotals.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotals.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotals.Location = New System.Drawing.Point(292, 254)
        Me.lblTotals.Name = "lblTotals"
        Me.lblTotals.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotals.Size = New System.Drawing.Size(39, 13)
        Me.lblTotals.TabIndex = 6
        Me.lblTotals.Text = "Totals:"
        Me.lblTotals.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblOSTotal
        '
        Me.lblOSTotal.BackColor = System.Drawing.SystemColors.Control
        Me.lblOSTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblOSTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOSTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOSTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOSTotal.Location = New System.Drawing.Point(334, 252)
        Me.lblOSTotal.Name = "lblOSTotal"
        Me.lblOSTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOSTotal.Size = New System.Drawing.Size(90, 19)
        Me.lblOSTotal.TabIndex = 5
        Me.lblOSTotal.Text = "Totals"
        Me.lblOSTotal.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblBaseCurrency
        '
        Me.lblBaseCurrency.AutoSize = True
        Me.lblBaseCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblBaseCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBaseCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBaseCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBaseCurrency.Location = New System.Drawing.Point(260, 8)
        Me.lblBaseCurrency.Name = "lblBaseCurrency"
        Me.lblBaseCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBaseCurrency.Size = New System.Drawing.Size(76, 13)
        Me.lblBaseCurrency.TabIndex = 2
        Me.lblBaseCurrency.Text = "Base Currency"
        '
        'lblTransactionCurrency
        '
        Me.lblTransactionCurrency.AutoSize = True
        Me.lblTransactionCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblTransactionCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTransactionCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTransactionCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTransactionCurrency.Location = New System.Drawing.Point(10, 8)
        Me.lblTransactionCurrency.Name = "lblTransactionCurrency"
        Me.lblTransactionCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTransactionCurrency.Size = New System.Drawing.Size(116, 13)
        Me.lblTransactionCurrency.TabIndex = 1
        Me.lblTransactionCurrency.Text = "Transaction Currencies"
        '
        'tdbGrid
        '
        Me.tdbGrid.AllowBigSelection = False
        Me.tdbGrid.AllowRowSelection = False
        Me.tdbGrid.AllowUserToAddRows = False
        Me.tdbGrid.AllowUserToDeleteRows = False
        Me.tdbGrid.AllowUserToResizeColumns = False
        Me.tdbGrid.AllowUserToResizeRows = False
        Me.tdbGrid.AlternatingRows = False
        Me.tdbGrid.BackColorFixed = System.Drawing.Color.Empty
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.tdbGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.tdbGrid.ColumnsCount = 0
        Me.tdbGrid.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me.tdbGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tdbGrid.EvenStyle = DataGridViewCellStyle2
        Me.tdbGrid.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me.tdbGrid.FixedColumns = -1
        Me.tdbGrid.FixedRows = -1
        Me.tdbGrid.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me.tdbGrid.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tdbGrid.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me.tdbGrid.GridLineWidth = 0
        Me.tdbGrid.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me.tdbGrid.Location = New System.Drawing.Point(6, 28)
        Me.tdbGrid.MultiSelect = False
        Me.tdbGrid.Name = "tdbGrid"
        Me.tdbGrid.OddStyle = DataGridViewCellStyle3
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.tdbGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.tdbGrid.RowHeightMin = 0
        Me.tdbGrid.RowsCount = 0
        Me.tdbGrid.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me.tdbGrid.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.tdbGrid.SelectedStyle = Nothing
        Me.tdbGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.tdbGrid.SelLength = -1
        Me.tdbGrid.SelStart = -1
        Me.tdbGrid.Size = New System.Drawing.Size(707, 219)
        Me.tdbGrid.TabIndex = 0
        Me.tdbGrid.ToolTipText = ""
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(725, 371)
        Me.Controls.Add(Me.cboWriteOffReasonId)
        Me.Controls.Add(Me.cmdRemove)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tdbGrid)
        Me.Controls.Add(Me.lblAllocatedTotal)
        Me.Controls.Add(Me.Line2)
        Me.Controls.Add(Me.line1)
        Me.Controls.Add(Me.lblWriteOffReasonID)
        Me.Controls.Add(Me.lblBal)
        Me.Controls.Add(Me.lblCurrencyTotal)
        Me.Controls.Add(Me.lblWriteOffTotal)
        Me.Controls.Add(Me.lblBalance)
        Me.Controls.Add(Me.lineBottom)
        Me.Controls.Add(Me.lblTotals)
        Me.Controls.Add(Me.lblOSTotal)
        Me.Controls.Add(Me.lblBaseCurrency)
        Me.Controls.Add(Me.lblTransactionCurrency)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Allocation"
        CType(Me.tdbGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class