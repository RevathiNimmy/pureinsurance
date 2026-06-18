<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False

		InitializetxtOccursPer()
		InitializeoptOccurs()
		InitializelblOccursPer()
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
    Public WithEvents pctDots As System.Windows.Forms.PictureBox
    Public WithEvents cmdNew As System.Windows.Forms.Button
    Public WithEvents cmdApplyNew As System.Windows.Forms.Button
    Private WithEvents _staStatusBar_Panel1 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents staStatusBar As System.Windows.Forms.StatusStrip
    Public WithEvents cmdPaste As System.Windows.Forms.Button
    Public WithEvents cmdRemove As System.Windows.Forms.Button
    Public WithEvents cmdAdd As System.Windows.Forms.Button
    Public WithEvents lblDocBalance As System.Windows.Forms.Label
    Public WithEvents panDocBalance As System.Windows.Forms.Label
    Public WithEvents Frame3 As System.Windows.Forms.GroupBox
    Private WithEvents _optOccurs_0 As System.Windows.Forms.RadioButton
    Private WithEvents _optOccurs_1 As System.Windows.Forms.RadioButton
    Private WithEvents _optOccurs_2 As System.Windows.Forms.RadioButton
    Public WithEvents dtpReverseDate As System.Windows.Forms.DateTimePicker
    Public WithEvents lblReversesOn As System.Windows.Forms.Label
    Public WithEvents lblOccurs As System.Windows.Forms.Label
    Public WithEvents lblTimes As System.Windows.Forms.Label
    Private WithEvents _lblOccursPer_0 As System.Windows.Forms.Label
    Private WithEvents _lblOccursPer_1 As System.Windows.Forms.Label
    Private WithEvents _lblOccursPer_2 As System.Windows.Forms.Label
    Public WithEvents Frame2 As System.Windows.Forms.GroupBox
    Public WithEvents cboBranches As System.Windows.Forms.ComboBox
    Public WithEvents txtComment As System.Windows.Forms.TextBox
    Public WithEvents cmbDocumentType As UserControls.TypeTable
    Public WithEvents dtpDocumentDate As System.Windows.Forms.DateTimePicker
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents lblComment As System.Windows.Forms.Label
    Public WithEvents lblDocumentDate As System.Windows.Forms.Label
    Public WithEvents lblDocumentType As System.Windows.Forms.Label
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public lblOccursPer(2) As System.Windows.Forms.Label
    Public optOccurs(2) As System.Windows.Forms.RadioButton
    Public txtOccursPer(2) As System.Windows.Forms.NumericUpDown
    'Public udOccursPer(2) As AxComCtl2.AxUpDown
    Private WithEvents tdgTransactions As Artinsoft.Windows.Forms.ExtendedDataGridView
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    '<System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.pctDots = New System.Windows.Forms.PictureBox
        Me.cmdNew = New System.Windows.Forms.Button
        Me.cmdApplyNew = New System.Windows.Forms.Button
        Me.staStatusBar = New System.Windows.Forms.StatusStrip
        Me._staStatusBar_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.Frame3 = New System.Windows.Forms.GroupBox
        Me.cmdPaste = New System.Windows.Forms.Button
        Me.cmdRemove = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.tdgTransactions = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me.lblDocBalance = New System.Windows.Forms.Label
        Me.panDocBalance = New System.Windows.Forms.Label
        Me.Frame2 = New System.Windows.Forms.GroupBox
        Me._txtOccursPer_2 = New System.Windows.Forms.NumericUpDown
        Me._txtOccursPer_1 = New System.Windows.Forms.NumericUpDown
        Me._txtOccursPer_0 = New System.Windows.Forms.NumericUpDown
        Me.txtOccurs = New System.Windows.Forms.NumericUpDown
        Me._optOccurs_0 = New System.Windows.Forms.RadioButton
        Me._optOccurs_1 = New System.Windows.Forms.RadioButton
        Me._optOccurs_2 = New System.Windows.Forms.RadioButton
        Me.dtpReverseDate = New System.Windows.Forms.DateTimePicker
        Me.lblReversesOn = New System.Windows.Forms.Label
        Me.lblOccurs = New System.Windows.Forms.Label
        Me.lblTimes = New System.Windows.Forms.Label
        Me._lblOccursPer_0 = New System.Windows.Forms.Label
        Me._lblOccursPer_1 = New System.Windows.Forms.Label
        Me._lblOccursPer_2 = New System.Windows.Forms.Label
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.cboBranches = New System.Windows.Forms.ComboBox
        Me.txtComment = New System.Windows.Forms.TextBox
        Me.cmbDocumentType = New UserControls.TypeTable
        Me.dtpDocumentDate = New System.Windows.Forms.DateTimePicker
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblComment = New System.Windows.Forms.Label
        Me.lblDocumentDate = New System.Windows.Forms.Label
        Me.lblDocumentType = New System.Windows.Forms.Label
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        CType(Me.pctDots, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.staStatusBar.SuspendLayout()
        Me.Frame3.SuspendLayout()
        CType(Me.tdgTransactions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Frame2.SuspendLayout()
        CType(Me._txtOccursPer_2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._txtOccursPer_1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._txtOccursPer_0, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtOccurs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Frame1.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pctDots
        '
        Me.pctDots.BackColor = System.Drawing.SystemColors.Control
        Me.pctDots.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pctDots.Cursor = System.Windows.Forms.Cursors.Default
        Me.pctDots.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pctDots.Image = CType(resources.GetObject("pctDots.Image"), System.Drawing.Image)
        Me.pctDots.Location = New System.Drawing.Point(128, 448)
        Me.pctDots.Name = "pctDots"
        Me.pctDots.Size = New System.Drawing.Size(25, 25)
        Me.pctDots.TabIndex = 40
        Me.pctDots.TabStop = False
        Me.pctDots.Visible = False
        '
        'cmdNew
        '
        Me.cmdNew.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNew.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNew.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNew.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNew.Location = New System.Drawing.Point(600, 448)
        Me.cmdNew.Name = "cmdNew"
        Me.cmdNew.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNew.Size = New System.Drawing.Size(73, 22)
        Me.cmdNew.TabIndex = 23
        Me.cmdNew.Text = "&New"
        Me.cmdNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNew.UseVisualStyleBackColor = False
        '
        'cmdApplyNew
        '
        Me.cmdApplyNew.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApplyNew.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApplyNew.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApplyNew.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApplyNew.Location = New System.Drawing.Point(480, 448)
        Me.cmdApplyNew.Name = "cmdApplyNew"
        Me.cmdApplyNew.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApplyNew.Size = New System.Drawing.Size(113, 22)
        Me.cmdApplyNew.TabIndex = 22
        Me.cmdApplyNew.Text = "&Apply and New"
        Me.cmdApplyNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdApplyNew.UseVisualStyleBackColor = False
        '
        'staStatusBar
        '
        Me.staStatusBar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.staStatusBar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._staStatusBar_Panel1})
        Me.staStatusBar.Location = New System.Drawing.Point(0, 481)
        Me.staStatusBar.Name = "staStatusBar"
        Me.staStatusBar.ShowItemToolTips = True
        Me.staStatusBar.Size = New System.Drawing.Size(759, 22)
        Me.staStatusBar.TabIndex = 38
        '
        '_staStatusBar_Panel1
        '
        Me._staStatusBar_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._staStatusBar_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._staStatusBar_Panel1.DoubleClickEnabled = True
        Me._staStatusBar_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._staStatusBar_Panel1.Name = "_staStatusBar_Panel1"
        Me._staStatusBar_Panel1.Size = New System.Drawing.Size(4, 22)
        Me._staStatusBar_Panel1.Tag = ""
        Me._staStatusBar_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Frame3
        '
        Me.Frame3.BackColor = System.Drawing.SystemColors.Control
        Me.Frame3.Controls.Add(Me.cmdPaste)
        Me.Frame3.Controls.Add(Me.cmdRemove)
        Me.Frame3.Controls.Add(Me.cmdAdd)
        Me.Frame3.Controls.Add(Me.tdgTransactions)
        Me.Frame3.Controls.Add(Me.lblDocBalance)
        Me.Frame3.Controls.Add(Me.panDocBalance)
        Me.Frame3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame3.Location = New System.Drawing.Point(8, 248)
        Me.Frame3.Name = "Frame3"
        Me.Frame3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame3.Size = New System.Drawing.Size(745, 193)
        Me.Frame3.TabIndex = 37
        Me.Frame3.TabStop = False
        Me.Frame3.Text = "Transactions"
        '
        'cmdPaste
        '
        Me.cmdPaste.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPaste.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPaste.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPaste.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPaste.Location = New System.Drawing.Point(168, 160)
        Me.cmdPaste.Name = "cmdPaste"
        Me.cmdPaste.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPaste.Size = New System.Drawing.Size(73, 22)
        Me.cmdPaste.TabIndex = 19
        Me.cmdPaste.Text = "&Paste"
        Me.cmdPaste.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPaste.UseVisualStyleBackColor = False
        '
        'cmdRemove
        '
        Me.cmdRemove.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRemove.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRemove.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRemove.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRemove.Location = New System.Drawing.Point(88, 160)
        Me.cmdRemove.Name = "cmdRemove"
        Me.cmdRemove.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRemove.Size = New System.Drawing.Size(73, 22)
        Me.cmdRemove.TabIndex = 18
        Me.cmdRemove.Text = "&Remove"
        Me.cmdRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRemove.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(8, 160)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 17
        Me.cmdAdd.Text = "A&dd"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'tdgTransactions
        '
        Me.tdgTransactions.AllowBigSelection = False
        Me.tdgTransactions.AllowRowSelection = False
        Me.tdgTransactions.AllowUserToAddRows = False
        Me.tdgTransactions.AlternatingRows = False
        Me.tdgTransactions.BackColorFixed = System.Drawing.Color.Empty
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.tdgTransactions.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.tdgTransactions.ColumnsCount = 0
        Me.tdgTransactions.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me.tdgTransactions.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.tdgTransactions.EvenStyle = DataGridViewCellStyle2
        Me.tdgTransactions.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me.tdgTransactions.FixedColumns = -1
        Me.tdgTransactions.FixedRows = -1
        Me.tdgTransactions.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me.tdgTransactions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tdgTransactions.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me.tdgTransactions.GridLineWidth = 0
        Me.tdgTransactions.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me.tdgTransactions.Location = New System.Drawing.Point(8, 19)
        Me.tdgTransactions.Name = "tdgTransactions"
        Me.tdgTransactions.OddStyle = DataGridViewCellStyle3
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.tdgTransactions.RowHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.tdgTransactions.RowHeightMin = 0
        Me.tdgTransactions.RowsCount = 0
        Me.tdgTransactions.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me.tdgTransactions.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.tdgTransactions.SelectedStyle = Nothing
        Me.tdgTransactions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.tdgTransactions.SelLength = -1
        Me.tdgTransactions.SelStart = -1
        Me.tdgTransactions.Size = New System.Drawing.Size(729, 134)
        Me.tdgTransactions.TabIndex = 16
        Me.tdgTransactions.ToolTipText = ""
        '
        'lblDocBalance
        '
        Me.lblDocBalance.BackColor = System.Drawing.SystemColors.Control
        Me.lblDocBalance.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDocBalance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocBalance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDocBalance.Location = New System.Drawing.Point(486, 163)
        Me.lblDocBalance.Name = "lblDocBalance"
        Me.lblDocBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDocBalance.Size = New System.Drawing.Size(123, 17)
        Me.lblDocBalance.TabIndex = 39
        Me.lblDocBalance.Text = "Document Balance:"
        '
        'panDocBalance
        '
        Me.panDocBalance.BackColor = System.Drawing.SystemColors.Control
        Me.panDocBalance.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panDocBalance.Cursor = System.Windows.Forms.Cursors.Default
        Me.panDocBalance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panDocBalance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.panDocBalance.Location = New System.Drawing.Point(620, 160)
        Me.panDocBalance.Name = "panDocBalance"
        Me.panDocBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panDocBalance.Size = New System.Drawing.Size(117, 21)
        Me.panDocBalance.TabIndex = 20
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me._txtOccursPer_2)
        Me.Frame2.Controls.Add(Me._txtOccursPer_1)
        Me.Frame2.Controls.Add(Me._txtOccursPer_0)
        Me.Frame2.Controls.Add(Me.txtOccurs)
        Me.Frame2.Controls.Add(Me._optOccurs_0)
        Me.Frame2.Controls.Add(Me._optOccurs_1)
        Me.Frame2.Controls.Add(Me._optOccurs_2)
        Me.Frame2.Controls.Add(Me.dtpReverseDate)
        Me.Frame2.Controls.Add(Me.lblReversesOn)
        Me.Frame2.Controls.Add(Me.lblOccurs)
        Me.Frame2.Controls.Add(Me.lblTimes)
        Me.Frame2.Controls.Add(Me._lblOccursPer_0)
        Me.Frame2.Controls.Add(Me._lblOccursPer_1)
        Me.Frame2.Controls.Add(Me._lblOccursPer_2)
        Me.Frame2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(8, 136)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(745, 97)
        Me.Frame2.TabIndex = 30
        Me.Frame2.TabStop = False
        Me.Frame2.Text = "Recurring"
        '
        '_txtOccursPer_2
        '
        Me._txtOccursPer_2.Location = New System.Drawing.Point(537, 73)
        Me._txtOccursPer_2.Maximum = New Decimal(New Integer() {31, 0, 0, 0})
        Me._txtOccursPer_2.Name = "_txtOccursPer_2"
        Me._txtOccursPer_2.Size = New System.Drawing.Size(35, 20)
        Me._txtOccursPer_2.TabIndex = 40
        '
        '_txtOccursPer_1
        '
        Me._txtOccursPer_1.Location = New System.Drawing.Point(537, 46)
        Me._txtOccursPer_1.Maximum = New Decimal(New Integer() {31, 0, 0, 0})
        Me._txtOccursPer_1.Name = "_txtOccursPer_1"
        Me._txtOccursPer_1.Size = New System.Drawing.Size(35, 20)
        Me._txtOccursPer_1.TabIndex = 39
        '
        '_txtOccursPer_0
        '
        Me._txtOccursPer_0.InterceptArrowKeys = False
        Me._txtOccursPer_0.Location = New System.Drawing.Point(537, 20)
        Me._txtOccursPer_0.Maximum = New Decimal(New Integer() {366, 0, 0, 0})
        Me._txtOccursPer_0.Name = "_txtOccursPer_0"
        Me._txtOccursPer_0.Size = New System.Drawing.Size(35, 20)
        Me._txtOccursPer_0.TabIndex = 38
        '
        'txtOccurs
        '
        Me.txtOccurs.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.txtOccurs.Location = New System.Drawing.Point(104, 64)
        Me.txtOccurs.Maximum = New Decimal(New Integer() {12, 0, 0, 0})
        Me.txtOccurs.Name = "txtOccurs"
        Me.txtOccurs.Size = New System.Drawing.Size(41, 20)
        Me.txtOccurs.TabIndex = 37
        Me.txtOccurs.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        '_optOccurs_0
        '
        Me._optOccurs_0.BackColor = System.Drawing.SystemColors.Control
        Me._optOccurs_0.Checked = True
        Me._optOccurs_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optOccurs_0.Enabled = False
        Me._optOccurs_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optOccurs_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optOccurs_0.Location = New System.Drawing.Point(400, 21)
        Me._optOccurs_0.Name = "_optOccurs_0"
        Me._optOccurs_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optOccurs_0.Size = New System.Drawing.Size(145, 18)
        Me._optOccurs_0.TabIndex = 7
        Me._optOccurs_0.TabStop = True
        Me._optOccurs_0.Text = "Per Period on day"
        Me._optOccurs_0.UseVisualStyleBackColor = False
        '
        '_optOccurs_1
        '
        Me._optOccurs_1.BackColor = System.Drawing.SystemColors.Control
        Me._optOccurs_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optOccurs_1.Enabled = False
        Me._optOccurs_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optOccurs_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optOccurs_1.Location = New System.Drawing.Point(400, 45)
        Me._optOccurs_1.Name = "_optOccurs_1"
        Me._optOccurs_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optOccurs_1.Size = New System.Drawing.Size(145, 18)
        Me._optOccurs_1.TabIndex = 10
        Me._optOccurs_1.TabStop = True
        Me._optOccurs_1.Text = "Per Month on day"
        Me._optOccurs_1.UseVisualStyleBackColor = False
        '
        '_optOccurs_2
        '
        Me._optOccurs_2.BackColor = System.Drawing.SystemColors.Control
        Me._optOccurs_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optOccurs_2.Enabled = False
        Me._optOccurs_2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optOccurs_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optOccurs_2.Location = New System.Drawing.Point(400, 69)
        Me._optOccurs_2.Name = "_optOccurs_2"
        Me._optOccurs_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optOccurs_2.Size = New System.Drawing.Size(145, 22)
        Me._optOccurs_2.TabIndex = 13
        Me._optOccurs_2.TabStop = True
        Me._optOccurs_2.Text = "Per Quarter on day"
        Me._optOccurs_2.UseVisualStyleBackColor = False
        '
        'dtpReverseDate
        '
        Me.dtpReverseDate.Location = New System.Drawing.Point(104, 24)
        Me.dtpReverseDate.Name = "dtpReverseDate"
        Me.dtpReverseDate.Size = New System.Drawing.Size(146, 20)
        Me.dtpReverseDate.TabIndex = 4
        '
        'lblReversesOn
        '
        Me.lblReversesOn.AutoSize = True
        Me.lblReversesOn.BackColor = System.Drawing.SystemColors.Control
        Me.lblReversesOn.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReversesOn.Enabled = False
        Me.lblReversesOn.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReversesOn.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReversesOn.Location = New System.Drawing.Point(24, 28)
        Me.lblReversesOn.Name = "lblReversesOn"
        Me.lblReversesOn.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReversesOn.Size = New System.Drawing.Size(83, 13)
        Me.lblReversesOn.TabIndex = 36
        Me.lblReversesOn.Text = "Reverses on:"
        '
        'lblOccurs
        '
        Me.lblOccurs.AutoSize = True
        Me.lblOccurs.BackColor = System.Drawing.SystemColors.Control
        Me.lblOccurs.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOccurs.Enabled = False
        Me.lblOccurs.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOccurs.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOccurs.Location = New System.Drawing.Point(24, 64)
        Me.lblOccurs.Name = "lblOccurs"
        Me.lblOccurs.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOccurs.Size = New System.Drawing.Size(51, 13)
        Me.lblOccurs.TabIndex = 35
        Me.lblOccurs.Text = "Occurs:"
        '
        'lblTimes
        '
        Me.lblTimes.AutoSize = True
        Me.lblTimes.BackColor = System.Drawing.SystemColors.Control
        Me.lblTimes.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTimes.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTimes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTimes.Location = New System.Drawing.Point(152, 64)
        Me.lblTimes.Name = "lblTimes"
        Me.lblTimes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTimes.Size = New System.Drawing.Size(48, 13)
        Me.lblTimes.TabIndex = 34
        Me.lblTimes.Text = "time(s)"
        '
        '_lblOccursPer_0
        '
        Me._lblOccursPer_0.AutoSize = True
        Me._lblOccursPer_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblOccursPer_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblOccursPer_0.Enabled = False
        Me._lblOccursPer_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblOccursPer_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblOccursPer_0.Location = New System.Drawing.Point(600, 21)
        Me._lblOccursPer_0.Name = "_lblOccursPer_0"
        Me._lblOccursPer_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblOccursPer_0.Size = New System.Drawing.Size(62, 13)
        Me._lblOccursPer_0.TabIndex = 33
        Me._lblOccursPer_0.Text = "of period."
        '
        '_lblOccursPer_1
        '
        Me._lblOccursPer_1.AutoSize = True
        Me._lblOccursPer_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblOccursPer_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblOccursPer_1.Enabled = False
        Me._lblOccursPer_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblOccursPer_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblOccursPer_1.Location = New System.Drawing.Point(600, 45)
        Me._lblOccursPer_1.Name = "_lblOccursPer_1"
        Me._lblOccursPer_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblOccursPer_1.Size = New System.Drawing.Size(62, 13)
        Me._lblOccursPer_1.TabIndex = 32
        Me._lblOccursPer_1.Text = "of month."
        '
        '_lblOccursPer_2
        '
        Me._lblOccursPer_2.AutoSize = True
        Me._lblOccursPer_2.BackColor = System.Drawing.SystemColors.Control
        Me._lblOccursPer_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblOccursPer_2.Enabled = False
        Me._lblOccursPer_2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblOccursPer_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblOccursPer_2.Location = New System.Drawing.Point(600, 69)
        Me._lblOccursPer_2.Name = "_lblOccursPer_2"
        Me._lblOccursPer_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblOccursPer_2.Size = New System.Drawing.Size(122, 13)
        Me._lblOccursPer_2.TabIndex = 31
        Me._lblOccursPer_2.Text = "of every 3rd month."
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.cboBranches)
        Me.Frame1.Controls.Add(Me.txtComment)
        Me.Frame1.Controls.Add(Me.cmbDocumentType)
        Me.Frame1.Controls.Add(Me.dtpDocumentDate)
        Me.Frame1.Controls.Add(Me.Label1)
        Me.Frame1.Controls.Add(Me.lblComment)
        Me.Frame1.Controls.Add(Me.lblDocumentDate)
        Me.Frame1.Controls.Add(Me.lblDocumentType)
        Me.Frame1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(8, 8)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(745, 113)
        Me.Frame1.TabIndex = 25
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Document"
        '
        'cboBranches
        '
        Me.cboBranches.BackColor = System.Drawing.SystemColors.Window
        Me.cboBranches.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBranches.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBranches.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBranches.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBranches.Location = New System.Drawing.Point(104, 48)
        Me.cboBranches.Name = "cboBranches"
        Me.cboBranches.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBranches.Size = New System.Drawing.Size(201, 21)
        Me.cboBranches.TabIndex = 1
        '
        'txtComment
        '
        Me.txtComment.BackColor = System.Drawing.SystemColors.Window
        Me.txtComment.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtComment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtComment.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtComment.Location = New System.Drawing.Point(400, 16)
        Me.txtComment.MaxLength = 0
        Me.txtComment.Multiline = True
        Me.txtComment.Name = "txtComment"
        Me.txtComment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtComment.Size = New System.Drawing.Size(329, 59)
        Me.txtComment.TabIndex = 3
        '
        'cmbDocumentType
        '
        Me.cmbDocumentType.BackStyle = 0
        Me.cmbDocumentType.BorderStyle = 0
        Me.cmbDocumentType.DefaultItemId = 0
        Me.cmbDocumentType.ItemCode = ""
        Me.cmbDocumentType.ItemId = 0
        Me.cmbDocumentType.ListIndex = -1
        Me.cmbDocumentType.Location = New System.Drawing.Point(104, 16)
        Me.cmbDocumentType.Name = "cmbDocumentType"
        Me.cmbDocumentType.Size = New System.Drawing.Size(201, 21)
        Me.cmbDocumentType.Sorted = True
        Me.cmbDocumentType.TabIndex = 0
        Me.cmbDocumentType.ToolTipText = ""
        Me.cmbDocumentType.WhatsThisHelpID = 0
        '
        'dtpDocumentDate
        '
        Me.dtpDocumentDate.Location = New System.Drawing.Point(104, 80)
        Me.dtpDocumentDate.Name = "dtpDocumentDate"
        Me.dtpDocumentDate.Size = New System.Drawing.Size(146, 21)
        Me.dtpDocumentDate.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(24, 50)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(73, 17)
        Me.Label1.TabIndex = 29
        Me.Label1.Text = "Branch:"
        '
        'lblComment
        '
        Me.lblComment.BackColor = System.Drawing.SystemColors.Control
        Me.lblComment.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblComment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblComment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblComment.Location = New System.Drawing.Point(328, 17)
        Me.lblComment.Name = "lblComment"
        Me.lblComment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblComment.Size = New System.Drawing.Size(66, 17)
        Me.lblComment.TabIndex = 28
        Me.lblComment.Text = "Comment:"
        '
        'lblDocumentDate
        '
        Me.lblDocumentDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblDocumentDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDocumentDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocumentDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDocumentDate.Location = New System.Drawing.Point(24, 82)
        Me.lblDocumentDate.Name = "lblDocumentDate"
        Me.lblDocumentDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDocumentDate.Size = New System.Drawing.Size(73, 17)
        Me.lblDocumentDate.TabIndex = 27
        Me.lblDocumentDate.Text = "Date:"
        '
        'lblDocumentType
        '
        Me.lblDocumentType.BackColor = System.Drawing.SystemColors.Control
        Me.lblDocumentType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDocumentType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocumentType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDocumentType.Location = New System.Drawing.Point(24, 18)
        Me.lblDocumentType.Name = "lblDocumentType"
        Me.lblDocumentType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDocumentType.Size = New System.Drawing.Size(90, 17)
        Me.lblDocumentType.TabIndex = 26
        Me.lblDocumentType.Text = "Type:"
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(400, 448)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 21
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(680, 448)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 24
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(759, 503)
        Me.Controls.Add(Me.pctDots)
        Me.Controls.Add(Me.cmdNew)
        Me.Controls.Add(Me.cmdApplyNew)
        Me.Controls.Add(Me.staStatusBar)
        Me.Controls.Add(Me.Frame3)
        Me.Controls.Add(Me.Frame2)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(4, 30)
        Me.MaximizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Journal Details"
        CType(Me.pctDots, System.ComponentModel.ISupportInitialize).EndInit()
        Me.staStatusBar.ResumeLayout(False)
        Me.staStatusBar.PerformLayout()
        Me.Frame3.ResumeLayout(False)
        CType(Me.tdgTransactions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Frame2.ResumeLayout(False)
        Me.Frame2.PerformLayout()
        CType(Me._txtOccursPer_2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._txtOccursPer_1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._txtOccursPer_0, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtOccurs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
 
    Sub InitializetxtOccursPer()
        Me.txtOccursPer(0) = _txtOccursPer_0
        Me.txtOccursPer(1) = _txtOccursPer_1
        Me.txtOccursPer(2) = _txtOccursPer_2
    End Sub
    Sub InitializeoptOccurs()
        Me.optOccurs(0) = _optOccurs_0
        Me.optOccurs(1) = _optOccurs_1
        Me.optOccurs(2) = _optOccurs_2
    End Sub
    Sub InitializelblOccursPer()
        Me.lblOccursPer(0) = _lblOccursPer_0
        Me.lblOccursPer(1) = _lblOccursPer_1
        Me.lblOccursPer(2) = _lblOccursPer_2
    End Sub
    Friend WithEvents _txtOccursPer_0 As System.Windows.Forms.NumericUpDown
    Friend WithEvents txtOccurs As System.Windows.Forms.NumericUpDown
    Friend WithEvents _txtOccursPer_2 As System.Windows.Forms.NumericUpDown
    Friend WithEvents _txtOccursPer_1 As System.Windows.Forms.NumericUpDown
#End Region
End Class