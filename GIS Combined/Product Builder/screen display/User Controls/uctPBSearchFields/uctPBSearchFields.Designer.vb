<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctPBSearchField
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
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
	Friend WithEvents pctDots As System.Windows.Forms.PictureBox
	Friend WithEvents cboDate As System.Windows.Forms.DateTimePicker
	Friend WithEvents txtRiskIndex As System.Windows.Forms.TextBox
	Friend WithEvents cboGISDataModel As PMLookupControl.cboPMLookup
	Friend WithEvents lblgisdatamodel As System.Windows.Forms.Label
	Friend WithEvents lblRiskIndex As System.Windows.Forms.Label
	Friend WithEvents fraSearch As System.Windows.Forms.GroupBox
	Private WithEvents grdSearchCriteria As Artinsoft.Windows.Forms.ExtendedDataGridView
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctPBSearchField))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.pctDots = New System.Windows.Forms.PictureBox
        Me.fraSearch = New System.Windows.Forms.GroupBox
        Me.cboDate = New System.Windows.Forms.DateTimePicker
        Me.txtRiskIndex = New System.Windows.Forms.TextBox
        Me.cboGISDataModel = New PMLookupControl.cboPMLookup
        Me.grdSearchCriteria = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me.lblgisdatamodel = New System.Windows.Forms.Label
        Me.lblRiskIndex = New System.Windows.Forms.Label
        CType(Me.pctDots, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraSearch.SuspendLayout()
        CType(Me.grdSearchCriteria, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pctDots
        '
        Me.pctDots.BackColor = System.Drawing.SystemColors.Control
        Me.pctDots.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pctDots.Cursor = System.Windows.Forms.Cursors.Default
        Me.pctDots.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pctDots.Image = CType(resources.GetObject("pctDots.Image"), System.Drawing.Image)
        Me.pctDots.Location = New System.Drawing.Point(312, 104)
        Me.pctDots.Name = "pctDots"
        Me.pctDots.Size = New System.Drawing.Size(25, 25)
        Me.pctDots.TabIndex = 0
        Me.pctDots.Visible = False
        '
        'fraSearch
        '
        Me.fraSearch.BackColor = System.Drawing.SystemColors.Control
        Me.fraSearch.Controls.Add(Me.cboDate)
        Me.fraSearch.Controls.Add(Me.txtRiskIndex)
        Me.fraSearch.Controls.Add(Me.cboGISDataModel)
        Me.fraSearch.Controls.Add(Me.grdSearchCriteria)
        Me.fraSearch.Controls.Add(Me.lblgisdatamodel)
        Me.fraSearch.Controls.Add(Me.lblRiskIndex)
        Me.fraSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraSearch.Location = New System.Drawing.Point(0, 0)
        Me.fraSearch.Name = "fraSearch"
        Me.fraSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraSearch.Size = New System.Drawing.Size(353, 129)
        Me.fraSearch.TabIndex = 1
        Me.fraSearch.TabStop = False
        Me.fraSearch.Text = "Search Criteria"
        '
        'cboDate
        '
        Me.cboDate.Location = New System.Drawing.Point(160, 80)
        Me.cboDate.Name = "cboDate"
        Me.cboDate.Size = New System.Drawing.Size(121, 20)
        Me.cboDate.TabIndex = 7
        Me.cboDate.Visible = False
        '
        'txtRiskIndex
        '
        Me.txtRiskIndex.AcceptsReturn = True
        Me.txtRiskIndex.BackColor = System.Drawing.SystemColors.Window
        Me.txtRiskIndex.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRiskIndex.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRiskIndex.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRiskIndex.Location = New System.Drawing.Point(88, 48)
        Me.txtRiskIndex.MaxLength = 0
        Me.txtRiskIndex.Name = "txtRiskIndex"
        Me.txtRiskIndex.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRiskIndex.Size = New System.Drawing.Size(257, 21)
        Me.txtRiskIndex.TabIndex = 4
        '
        'cboGISDataModel
        '
        Me.cboGISDataModel.DefaultItemId = 0
        Me.cboGISDataModel.FirstItem = ""
        Me.cboGISDataModel.ItemId = 0
        Me.cboGISDataModel.ListIndex = -1
        Me.cboGISDataModel.Location = New System.Drawing.Point(88, 16)
        Me.cboGISDataModel.Name = "cboGISDataModel"
        Me.cboGISDataModel.PMLookupProductFamily = 1
        Me.cboGISDataModel.SingleItemId = 0
        Me.cboGISDataModel.Size = New System.Drawing.Size(257, 21)
        Me.cboGISDataModel.Sorted = True
        Me.cboGISDataModel.TabIndex = 3
        Me.cboGISDataModel.TableName = "GIS_data_model"
        Me.cboGISDataModel.ToolTipText = ""
        Me.cboGISDataModel.WhereClause = "GIS_data_model_type_id = 5"
        '
        'grdSearchCriteria
        '
        Me.grdSearchCriteria.AllowBigSelection = False
        Me.grdSearchCriteria.AllowRowSelection = False
        Me.grdSearchCriteria.AllowUserToAddRows = False
        Me.grdSearchCriteria.AllowUserToDeleteRows = False
        Me.grdSearchCriteria.AllowUserToResizeRows = False
        Me.grdSearchCriteria.AlternatingRows = False
        Me.grdSearchCriteria.BackColorFixed = System.Drawing.Color.Empty
        Me.grdSearchCriteria.ColumnsCount = 0
        Me.grdSearchCriteria.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me.grdSearchCriteria.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.grdSearchCriteria.EvenStyle = DataGridViewCellStyle1
        Me.grdSearchCriteria.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me.grdSearchCriteria.FixedColumns = -1
        Me.grdSearchCriteria.FixedRows = -1
        Me.grdSearchCriteria.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me.grdSearchCriteria.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grdSearchCriteria.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me.grdSearchCriteria.GridLineWidth = 0
        Me.grdSearchCriteria.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me.grdSearchCriteria.Location = New System.Drawing.Point(8, 40)
        Me.grdSearchCriteria.MultiSelect = False
        Me.grdSearchCriteria.Name = "grdSearchCriteria"
        Me.grdSearchCriteria.OddStyle = DataGridViewCellStyle2
        Me.grdSearchCriteria.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.grdSearchCriteria.RowHeightMin = 0
        Me.grdSearchCriteria.RowsCount = 0
        Me.grdSearchCriteria.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me.grdSearchCriteria.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.grdSearchCriteria.SelectedStyle = Nothing
        Me.grdSearchCriteria.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.grdSearchCriteria.SelLength = -1
        Me.grdSearchCriteria.SelStart = -1
        Me.grdSearchCriteria.Size = New System.Drawing.Size(337, 81)
        Me.grdSearchCriteria.TabIndex = 2
        Me.grdSearchCriteria.ToolTipText = ""
        Me.grdSearchCriteria.Visible = False
        '
        'lblgisdatamodel
        '
        Me.lblgisdatamodel.AutoSize = True
        Me.lblgisdatamodel.BackColor = System.Drawing.SystemColors.Control
        Me.lblgisdatamodel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblgisdatamodel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblgisdatamodel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblgisdatamodel.Location = New System.Drawing.Point(8, 16)
        Me.lblgisdatamodel.Name = "lblgisdatamodel"
        Me.lblgisdatamodel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblgisdatamodel.Size = New System.Drawing.Size(62, 13)
        Me.lblgisdatamodel.TabIndex = 6
        Me.lblgisdatamodel.Text = "Data Model"
        '
        'lblRiskIndex
        '
        Me.lblRiskIndex.AutoSize = True
        Me.lblRiskIndex.BackColor = System.Drawing.SystemColors.Control
        Me.lblRiskIndex.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRiskIndex.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRiskIndex.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRiskIndex.Location = New System.Drawing.Point(8, 48)
        Me.lblRiskIndex.Name = "lblRiskIndex"
        Me.lblRiskIndex.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRiskIndex.Size = New System.Drawing.Size(57, 13)
        Me.lblRiskIndex.TabIndex = 5
        Me.lblRiskIndex.Text = "Risk Index"
        '
        'uctPBSearchField
        '
        Me.Controls.Add(Me.pctDots)
        Me.Controls.Add(Me.fraSearch)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctPBSearchField"
        Me.Size = New System.Drawing.Size(356, 130)
        CType(Me.pctDots, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraSearch.ResumeLayout(False)
        Me.fraSearch.PerformLayout()
        CType(Me.grdSearchCriteria, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
#Region "Upgrade Support"
	<System.Runtime.InteropServices.ProgId("SearchFieldEditedEventArgs_NET.SearchFieldEditedEventArgs")> _
	Public NotInheritable Class SearchFieldEditedEventArgs
		Inherits System.EventArgs
		Public vRiskIndex As Object
		Public Sub New(ByRef vRiskIndex As Object)
			MyBase.New()
			Me.vRiskIndex = vRiskIndex
		End Sub
	End Class
#End Region 
End Class