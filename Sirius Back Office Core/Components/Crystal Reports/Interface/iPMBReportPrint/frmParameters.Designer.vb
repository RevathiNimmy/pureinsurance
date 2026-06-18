<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmParameters
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializelvwIncluded()
		InitializelvwExcluded()
		InitializelblParameterType()
		InitializelblParameterName()
		InitializelblIncluded()
		InitializelblExcluded()
		InitializefraType()
		InitializecmdParameterValues()
		InitializecmdDeleteExcluded()
		InitializecmdDeleteAllExcluded()
		InitializecmdAddIncluded()
		InitializecmdAddAllIncluded()
		InitializechkGroupBy()
		InitializecboParameterValues()
        InitializeDTParameters()
        InitializeListBox()
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
    Public WithEvents cboFrequency As PMLookupControl.cboPMLookup
    Private WithEvents _DTParameters_13 As System.Windows.Forms.DateTimePicker
    Private WithEvents _DTParameters_12 As System.Windows.Forms.DateTimePicker
    Private WithEvents _DTParameters_11 As System.Windows.Forms.DateTimePicker
    Private WithEvents _DTParameters_10 As System.Windows.Forms.DateTimePicker
    Private WithEvents _DTParameters_9 As System.Windows.Forms.DateTimePicker
    Private WithEvents _DTParameters_8 As System.Windows.Forms.DateTimePicker
    Private WithEvents _DTParameters_7 As System.Windows.Forms.DateTimePicker
    Private WithEvents _DTParameters_6 As System.Windows.Forms.DateTimePicker
    Private WithEvents _DTParameters_5 As System.Windows.Forms.DateTimePicker
    Private WithEvents _DTParameters_4 As System.Windows.Forms.DateTimePicker
    Private WithEvents _DTParameters_3 As System.Windows.Forms.DateTimePicker
    Private WithEvents _chkGroupBy_13 As System.Windows.Forms.CheckBox
    Private WithEvents _cmdParameterValues_13 As System.Windows.Forms.Button
    Private WithEvents _chkGroupBy_12 As System.Windows.Forms.CheckBox
    Private WithEvents _cmdParameterValues_12 As System.Windows.Forms.Button
    Private WithEvents _cmdParameterValues_11 As System.Windows.Forms.Button
    Private WithEvents _chkGroupBy_11 As System.Windows.Forms.CheckBox
    Private WithEvents _cboParameterValues_11 As System.Windows.Forms.ComboBox
    Private WithEvents _chkGroupBy_10 As System.Windows.Forms.CheckBox
    Private WithEvents _chkGroupBy_9 As System.Windows.Forms.CheckBox
    Private WithEvents _chkGroupBy_8 As System.Windows.Forms.CheckBox
    Private WithEvents _chkGroupBy_7 As System.Windows.Forms.CheckBox
    Private WithEvents _chkGroupBy_6 As System.Windows.Forms.CheckBox
    Private WithEvents _chkGroupBy_5 As System.Windows.Forms.CheckBox
    Private WithEvents _chkGroupBy_4 As System.Windows.Forms.CheckBox
    Private WithEvents _chkGroupBy_3 As System.Windows.Forms.CheckBox
    Private WithEvents _chkGroupBy_2 As System.Windows.Forms.CheckBox
    Private WithEvents _chkGroupBy_1 As System.Windows.Forms.CheckBox
    Private WithEvents _chkGroupBy_0 As System.Windows.Forms.CheckBox
    Private WithEvents _cmdParameterValues_10 As System.Windows.Forms.Button
    Private WithEvents _cmdParameterValues_9 As System.Windows.Forms.Button
    Private WithEvents _cmdParameterValues_8 As System.Windows.Forms.Button
    Private WithEvents _cmdParameterValues_7 As System.Windows.Forms.Button
    Private WithEvents _cmdParameterValues_0 As System.Windows.Forms.Button
    Private WithEvents _cmdParameterValues_1 As System.Windows.Forms.Button
    Private WithEvents _cmdParameterValues_6 As System.Windows.Forms.Button
    Private WithEvents _cmdParameterValues_5 As System.Windows.Forms.Button
    Private WithEvents _cmdParameterValues_4 As System.Windows.Forms.Button
    Private WithEvents _cmdParameterValues_3 As System.Windows.Forms.Button
    Private WithEvents _cmdParameterValues_2 As System.Windows.Forms.Button
    Private WithEvents _cboParameterValues_6 As System.Windows.Forms.ComboBox
    Private WithEvents _cboParameterValues_5 As System.Windows.Forms.ComboBox
    Private WithEvents _cboParameterValues_4 As System.Windows.Forms.ComboBox
    Private WithEvents _cboParameterValues_3 As System.Windows.Forms.ComboBox
    Private WithEvents _cboParameterValues_2 As System.Windows.Forms.ComboBox
    Private WithEvents _cboParameterValues_1 As System.Windows.Forms.ComboBox
    Private WithEvents _cboParameterValues_0 As System.Windows.Forms.ComboBox
    Private WithEvents _cboParameterValues_7 As System.Windows.Forms.ComboBox
    Private WithEvents _cboParameterValues_8 As System.Windows.Forms.ComboBox
    Private WithEvents _cboParameterValues_9 As System.Windows.Forms.ComboBox
    Private WithEvents _cboParameterValues_10 As System.Windows.Forms.ComboBox
    Private WithEvents _cboParameterValues_12 As System.Windows.Forms.ComboBox
    Private WithEvents _cboParameterValues_13 As System.Windows.Forms.ComboBox
    Public WithEvents lblFrequencyType As System.Windows.Forms.Label
    Public WithEvents lblFrequencyPrompt As System.Windows.Forms.Label
    Private WithEvents _lblParameterName_13 As System.Windows.Forms.Label
    Private WithEvents _lblParameterType_13 As System.Windows.Forms.Label
    Private WithEvents _lblParameterName_12 As System.Windows.Forms.Label
    Private WithEvents _lblParameterType_12 As System.Windows.Forms.Label
    Private WithEvents _lblParameterType_11 As System.Windows.Forms.Label
    Private WithEvents _lblParameterName_11 As System.Windows.Forms.Label
    Public WithEvents lblGroupBy As System.Windows.Forms.Label
    Private WithEvents _lblParameterName_10 As System.Windows.Forms.Label
    Private WithEvents _lblParameterType_10 As System.Windows.Forms.Label
    Private WithEvents _lblParameterName_9 As System.Windows.Forms.Label
    Private WithEvents _lblParameterType_9 As System.Windows.Forms.Label
    Private WithEvents _lblParameterName_8 As System.Windows.Forms.Label
    Private WithEvents _lblParameterType_8 As System.Windows.Forms.Label
    Private WithEvents _lblParameterName_7 As System.Windows.Forms.Label
    Private WithEvents _lblParameterType_7 As System.Windows.Forms.Label
    Private WithEvents _lblParameterName_0 As System.Windows.Forms.Label
    Private WithEvents _lblParameterName_1 As System.Windows.Forms.Label
    Private WithEvents _lblParameterName_2 As System.Windows.Forms.Label
    Private WithEvents _lblParameterName_3 As System.Windows.Forms.Label
    Private WithEvents _lblParameterName_4 As System.Windows.Forms.Label
    Private WithEvents _lblParameterType_0 As System.Windows.Forms.Label
    Private WithEvents _lblParameterType_1 As System.Windows.Forms.Label
    Private WithEvents _lblParameterType_2 As System.Windows.Forms.Label
    Private WithEvents _lblParameterType_3 As System.Windows.Forms.Label
    Private WithEvents _lblParameterType_4 As System.Windows.Forms.Label
    Private WithEvents _lblParameterType_5 As System.Windows.Forms.Label
    Private WithEvents _lblParameterName_5 As System.Windows.Forms.Label
    Private WithEvents _lblParameterName_6 As System.Windows.Forms.Label
    Private WithEvents _lblParameterType_6 As System.Windows.Forms.Label
    Public WithEvents fraParameters As System.Windows.Forms.GroupBox
    Private WithEvents _cmdDeleteExcluded_0 As System.Windows.Forms.Button
    Private WithEvents _cmdAddIncluded_0 As System.Windows.Forms.Button
    Private WithEvents _cmdAddAllIncluded_0 As System.Windows.Forms.Button
    Private WithEvents _cmdDeleteAllExcluded_0 As System.Windows.Forms.Button
    Private WithEvents _lvwIncluded_0_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwIncluded_0 As System.Windows.Forms.ListView
    Private WithEvents _lvwExcluded_0_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwExcluded_0 As System.Windows.Forms.ListView
    Private WithEvents _lblIncluded_0 As System.Windows.Forms.Label
    Private WithEvents _lblExcluded_0 As System.Windows.Forms.Label
    Private WithEvents _fraType_0 As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents ImageList1 As System.Windows.Forms.ImageList
    Public WithEvents cmdAddToScheduler As System.Windows.Forms.Button
    Public DTParameters(13) As System.Windows.Forms.DateTimePicker
    Public List1(13) As System.Windows.Forms.ListBox
    Public cboParameterValues(13) As System.Windows.Forms.ComboBox
    Public chkGroupBy(13) As System.Windows.Forms.CheckBox
    Public cmdAddAllIncluded(0) As System.Windows.Forms.Button
    Public cmdAddIncluded(0) As System.Windows.Forms.Button
    Public cmdDeleteAllExcluded(0) As System.Windows.Forms.Button
    Public cmdDeleteExcluded(0) As System.Windows.Forms.Button
    Public cmdParameterValues(13) As System.Windows.Forms.Button
    Public fraType(0) As System.Windows.Forms.GroupBox
    Public lblExcluded(0) As System.Windows.Forms.Label
    Public lblIncluded(0) As System.Windows.Forms.Label
    Public lblParameterName(13) As System.Windows.Forms.Label
    Public lblParameterType(13) As System.Windows.Forms.Label
    Public lvwExcluded(0) As System.Windows.Forms.ListView
    Public lvwIncluded(0) As System.Windows.Forms.ListView

    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmParameters))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me._cmdDeleteExcluded_0 = New System.Windows.Forms.Button
        Me._cmdAddIncluded_0 = New System.Windows.Forms.Button
        Me._cmdAddAllIncluded_0 = New System.Windows.Forms.Button
        Me._cmdDeleteAllExcluded_0 = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.fraParameters = New System.Windows.Forms.GroupBox
        Me.ListBox13 = New System.Windows.Forms.ListBox
        Me.ListBox12 = New System.Windows.Forms.ListBox
        Me.ListBox11 = New System.Windows.Forms.ListBox
        Me.ListBox10 = New System.Windows.Forms.ListBox
        Me.ListBox9 = New System.Windows.Forms.ListBox
        Me.ListBox8 = New System.Windows.Forms.ListBox
        Me.ListBox7 = New System.Windows.Forms.ListBox
        Me.ListBox6 = New System.Windows.Forms.ListBox
        Me.ListBox5 = New System.Windows.Forms.ListBox
        Me.ListBox4 = New System.Windows.Forms.ListBox
        Me.ListBox3 = New System.Windows.Forms.ListBox
        Me.ListBox2 = New System.Windows.Forms.ListBox
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.ListBox0 = New System.Windows.Forms.ListBox
        Me.cboFrequency = New PMLookupControl.cboPMLookup
        Me._DTParameters_13 = New System.Windows.Forms.DateTimePicker
        Me._DTParameters_12 = New System.Windows.Forms.DateTimePicker
        Me._DTParameters_11 = New System.Windows.Forms.DateTimePicker
        Me._DTParameters_10 = New System.Windows.Forms.DateTimePicker
        Me._DTParameters_9 = New System.Windows.Forms.DateTimePicker
        Me._DTParameters_8 = New System.Windows.Forms.DateTimePicker
        Me._DTParameters_7 = New System.Windows.Forms.DateTimePicker
        Me._DTParameters_6 = New System.Windows.Forms.DateTimePicker
        Me._DTParameters_5 = New System.Windows.Forms.DateTimePicker
        Me._DTParameters_4 = New System.Windows.Forms.DateTimePicker
        Me._DTParameters_3 = New System.Windows.Forms.DateTimePicker
        Me._DTParameters_2 = New System.Windows.Forms.DateTimePicker
        Me._DTParameters_1 = New System.Windows.Forms.DateTimePicker
        Me._DTParameters_0 = New System.Windows.Forms.DateTimePicker
        Me._chkGroupBy_13 = New System.Windows.Forms.CheckBox
        Me._cmdParameterValues_13 = New System.Windows.Forms.Button
        Me._chkGroupBy_12 = New System.Windows.Forms.CheckBox
        Me._cmdParameterValues_12 = New System.Windows.Forms.Button
        Me._cmdParameterValues_11 = New System.Windows.Forms.Button
        Me._chkGroupBy_11 = New System.Windows.Forms.CheckBox
        Me._cboParameterValues_11 = New System.Windows.Forms.ComboBox
        Me._chkGroupBy_10 = New System.Windows.Forms.CheckBox
        Me._chkGroupBy_9 = New System.Windows.Forms.CheckBox
        Me._chkGroupBy_8 = New System.Windows.Forms.CheckBox
        Me._chkGroupBy_7 = New System.Windows.Forms.CheckBox
        Me._chkGroupBy_6 = New System.Windows.Forms.CheckBox
        Me._chkGroupBy_5 = New System.Windows.Forms.CheckBox
        Me._chkGroupBy_4 = New System.Windows.Forms.CheckBox
        Me._chkGroupBy_3 = New System.Windows.Forms.CheckBox
        Me._chkGroupBy_2 = New System.Windows.Forms.CheckBox
        Me._chkGroupBy_1 = New System.Windows.Forms.CheckBox
        Me._chkGroupBy_0 = New System.Windows.Forms.CheckBox
        Me._cmdParameterValues_10 = New System.Windows.Forms.Button
        Me._cmdParameterValues_9 = New System.Windows.Forms.Button
        Me._cmdParameterValues_8 = New System.Windows.Forms.Button
        Me._cmdParameterValues_7 = New System.Windows.Forms.Button
        Me._cmdParameterValues_0 = New System.Windows.Forms.Button
        Me._cmdParameterValues_1 = New System.Windows.Forms.Button
        Me._cmdParameterValues_6 = New System.Windows.Forms.Button
        Me._cmdParameterValues_5 = New System.Windows.Forms.Button
        Me._cmdParameterValues_4 = New System.Windows.Forms.Button
        Me._cmdParameterValues_3 = New System.Windows.Forms.Button
        Me._cmdParameterValues_2 = New System.Windows.Forms.Button
        Me._cboParameterValues_6 = New System.Windows.Forms.ComboBox
        Me._cboParameterValues_5 = New System.Windows.Forms.ComboBox
        Me._cboParameterValues_4 = New System.Windows.Forms.ComboBox
        Me._cboParameterValues_3 = New System.Windows.Forms.ComboBox
        Me._cboParameterValues_2 = New System.Windows.Forms.ComboBox
        Me._cboParameterValues_1 = New System.Windows.Forms.ComboBox
        Me._cboParameterValues_0 = New System.Windows.Forms.ComboBox
        Me._cboParameterValues_7 = New System.Windows.Forms.ComboBox
        Me._cboParameterValues_8 = New System.Windows.Forms.ComboBox
        Me._cboParameterValues_9 = New System.Windows.Forms.ComboBox
        Me._cboParameterValues_10 = New System.Windows.Forms.ComboBox
        Me._cboParameterValues_12 = New System.Windows.Forms.ComboBox
        Me._cboParameterValues_13 = New System.Windows.Forms.ComboBox
        Me.lblFrequencyType = New System.Windows.Forms.Label
        Me.lblFrequencyPrompt = New System.Windows.Forms.Label
        Me._lblParameterName_13 = New System.Windows.Forms.Label
        Me._lblParameterType_13 = New System.Windows.Forms.Label
        Me._lblParameterName_12 = New System.Windows.Forms.Label
        Me._lblParameterType_12 = New System.Windows.Forms.Label
        Me._lblParameterType_11 = New System.Windows.Forms.Label
        Me._lblParameterName_11 = New System.Windows.Forms.Label
        Me.lblGroupBy = New System.Windows.Forms.Label
        Me._lblParameterName_10 = New System.Windows.Forms.Label
        Me._lblParameterType_10 = New System.Windows.Forms.Label
        Me._lblParameterName_9 = New System.Windows.Forms.Label
        Me._lblParameterType_9 = New System.Windows.Forms.Label
        Me._lblParameterName_8 = New System.Windows.Forms.Label
        Me._lblParameterType_8 = New System.Windows.Forms.Label
        Me._lblParameterName_7 = New System.Windows.Forms.Label
        Me._lblParameterType_7 = New System.Windows.Forms.Label
        Me._lblParameterName_0 = New System.Windows.Forms.Label
        Me._lblParameterName_1 = New System.Windows.Forms.Label
        Me._lblParameterName_2 = New System.Windows.Forms.Label
        Me._lblParameterName_3 = New System.Windows.Forms.Label
        Me._lblParameterName_4 = New System.Windows.Forms.Label
        Me._lblParameterType_0 = New System.Windows.Forms.Label
        Me._lblParameterType_1 = New System.Windows.Forms.Label
        Me._lblParameterType_2 = New System.Windows.Forms.Label
        Me._lblParameterType_3 = New System.Windows.Forms.Label
        Me._lblParameterType_4 = New System.Windows.Forms.Label
        Me._lblParameterType_5 = New System.Windows.Forms.Label
        Me._lblParameterName_5 = New System.Windows.Forms.Label
        Me._lblParameterName_6 = New System.Windows.Forms.Label
        Me._lblParameterType_6 = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me._fraType_0 = New System.Windows.Forms.GroupBox
        Me._lvwIncluded_0 = New System.Windows.Forms.ListView
        Me._lvwIncluded_0_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwExcluded_0 = New System.Windows.Forms.ListView
        Me._lvwExcluded_0_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lblIncluded_0 = New System.Windows.Forms.Label
        Me._lblExcluded_0 = New System.Windows.Forms.Label
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdAddToScheduler = New System.Windows.Forms.Button
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.fraParameters.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me._fraType_0.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_cmdDeleteExcluded_0
        '
        Me._cmdDeleteExcluded_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdDeleteExcluded_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdDeleteExcluded_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdDeleteExcluded_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdDeleteExcluded_0.Location = New System.Drawing.Point(264, 392)
        Me._cmdDeleteExcluded_0.Name = "_cmdDeleteExcluded_0"
        Me._cmdDeleteExcluded_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdDeleteExcluded_0.Size = New System.Drawing.Size(49, 22)
        Me._cmdDeleteExcluded_0.TabIndex = 69
        Me._cmdDeleteExcluded_0.Text = "&<--"
        Me._cmdDeleteExcluded_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me._cmdDeleteExcluded_0, "Unselect chosen task groups")
        Me._cmdDeleteExcluded_0.UseVisualStyleBackColor = False
        '
        '_cmdAddIncluded_0
        '
        Me._cmdAddIncluded_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdAddIncluded_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdAddIncluded_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdAddIncluded_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdAddIncluded_0.Location = New System.Drawing.Point(264, 40)
        Me._cmdAddIncluded_0.Name = "_cmdAddIncluded_0"
        Me._cmdAddIncluded_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdAddIncluded_0.Size = New System.Drawing.Size(49, 22)
        Me._cmdAddIncluded_0.TabIndex = 68
        Me._cmdAddIncluded_0.Text = "--&>"
        Me._cmdAddIncluded_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me._cmdAddIncluded_0, "Select all available task groups")
        Me._cmdAddIncluded_0.UseVisualStyleBackColor = False
        '
        '_cmdAddAllIncluded_0
        '
        Me._cmdAddAllIncluded_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdAddAllIncluded_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdAddAllIncluded_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdAddAllIncluded_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdAddAllIncluded_0.Location = New System.Drawing.Point(264, 72)
        Me._cmdAddAllIncluded_0.Name = "_cmdAddAllIncluded_0"
        Me._cmdAddAllIncluded_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdAddAllIncluded_0.Size = New System.Drawing.Size(49, 22)
        Me._cmdAddAllIncluded_0.TabIndex = 67
        Me._cmdAddAllIncluded_0.Text = "->>"
        Me._cmdAddAllIncluded_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me._cmdAddAllIncluded_0, "Select all available task groups")
        Me._cmdAddAllIncluded_0.UseVisualStyleBackColor = False
        '
        '_cmdDeleteAllExcluded_0
        '
        Me._cmdDeleteAllExcluded_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdDeleteAllExcluded_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdDeleteAllExcluded_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdDeleteAllExcluded_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdDeleteAllExcluded_0.Location = New System.Drawing.Point(264, 425)
        Me._cmdDeleteAllExcluded_0.Name = "_cmdDeleteAllExcluded_0"
        Me._cmdDeleteAllExcluded_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdDeleteAllExcluded_0.Size = New System.Drawing.Size(49, 22)
        Me._cmdDeleteAllExcluded_0.TabIndex = 66
        Me._cmdDeleteAllExcluded_0.Text = "<<-"
        Me._cmdDeleteAllExcluded_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me._cmdDeleteAllExcluded_0, "Unselect all task groups")
        Me._cmdDeleteAllExcluded_0.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage1)
        Me.tabMainTab.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(110, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(671, 565)
        Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.tabMainTab.TabIndex = 2
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraParameters)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(663, 539)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Parameters"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'fraParameters
        '
        Me.fraParameters.BackColor = System.Drawing.SystemColors.Control
        Me.fraParameters.Controls.Add(Me.ListBox13)
        Me.fraParameters.Controls.Add(Me.ListBox12)
        Me.fraParameters.Controls.Add(Me.ListBox11)
        Me.fraParameters.Controls.Add(Me.ListBox10)
        Me.fraParameters.Controls.Add(Me.ListBox9)
        Me.fraParameters.Controls.Add(Me.ListBox8)
        Me.fraParameters.Controls.Add(Me.ListBox7)
        Me.fraParameters.Controls.Add(Me.ListBox6)
        Me.fraParameters.Controls.Add(Me.ListBox5)
        Me.fraParameters.Controls.Add(Me.ListBox4)
        Me.fraParameters.Controls.Add(Me.ListBox3)
        Me.fraParameters.Controls.Add(Me.ListBox2)
        Me.fraParameters.Controls.Add(Me.ListBox1)
        Me.fraParameters.Controls.Add(Me.ListBox0)
        Me.fraParameters.Controls.Add(Me.cboFrequency)
        Me.fraParameters.Controls.Add(Me._DTParameters_13)
        Me.fraParameters.Controls.Add(Me._DTParameters_12)
        Me.fraParameters.Controls.Add(Me._DTParameters_11)
        Me.fraParameters.Controls.Add(Me._DTParameters_10)
        Me.fraParameters.Controls.Add(Me._DTParameters_9)
        Me.fraParameters.Controls.Add(Me._DTParameters_8)
        Me.fraParameters.Controls.Add(Me._DTParameters_7)
        Me.fraParameters.Controls.Add(Me._DTParameters_6)
        Me.fraParameters.Controls.Add(Me._DTParameters_5)
        Me.fraParameters.Controls.Add(Me._DTParameters_4)
        Me.fraParameters.Controls.Add(Me._DTParameters_3)
        Me.fraParameters.Controls.Add(Me._DTParameters_2)
        Me.fraParameters.Controls.Add(Me._DTParameters_1)
        Me.fraParameters.Controls.Add(Me._DTParameters_0)
        Me.fraParameters.Controls.Add(Me._chkGroupBy_13)
        Me.fraParameters.Controls.Add(Me._cmdParameterValues_13)
        Me.fraParameters.Controls.Add(Me._chkGroupBy_12)
        Me.fraParameters.Controls.Add(Me._cmdParameterValues_12)
        Me.fraParameters.Controls.Add(Me._cmdParameterValues_11)
        Me.fraParameters.Controls.Add(Me._chkGroupBy_11)
        Me.fraParameters.Controls.Add(Me._cboParameterValues_11)
        Me.fraParameters.Controls.Add(Me._chkGroupBy_10)
        Me.fraParameters.Controls.Add(Me._chkGroupBy_9)
        Me.fraParameters.Controls.Add(Me._chkGroupBy_8)
        Me.fraParameters.Controls.Add(Me._chkGroupBy_7)
        Me.fraParameters.Controls.Add(Me._chkGroupBy_6)
        Me.fraParameters.Controls.Add(Me._chkGroupBy_5)
        Me.fraParameters.Controls.Add(Me._chkGroupBy_4)
        Me.fraParameters.Controls.Add(Me._chkGroupBy_3)
        Me.fraParameters.Controls.Add(Me._chkGroupBy_2)
        Me.fraParameters.Controls.Add(Me._chkGroupBy_1)
        Me.fraParameters.Controls.Add(Me._chkGroupBy_0)
        Me.fraParameters.Controls.Add(Me._cmdParameterValues_10)
        Me.fraParameters.Controls.Add(Me._cmdParameterValues_9)
        Me.fraParameters.Controls.Add(Me._cmdParameterValues_8)
        Me.fraParameters.Controls.Add(Me._cmdParameterValues_7)
        Me.fraParameters.Controls.Add(Me._cmdParameterValues_0)
        Me.fraParameters.Controls.Add(Me._cmdParameterValues_1)
        Me.fraParameters.Controls.Add(Me._cmdParameterValues_6)
        Me.fraParameters.Controls.Add(Me._cmdParameterValues_5)
        Me.fraParameters.Controls.Add(Me._cmdParameterValues_4)
        Me.fraParameters.Controls.Add(Me._cmdParameterValues_3)
        Me.fraParameters.Controls.Add(Me._cmdParameterValues_2)
        Me.fraParameters.Controls.Add(Me._cboParameterValues_6)
        Me.fraParameters.Controls.Add(Me._cboParameterValues_5)
        Me.fraParameters.Controls.Add(Me._cboParameterValues_4)
        Me.fraParameters.Controls.Add(Me._cboParameterValues_3)
        Me.fraParameters.Controls.Add(Me._cboParameterValues_2)
        Me.fraParameters.Controls.Add(Me._cboParameterValues_1)
        Me.fraParameters.Controls.Add(Me._cboParameterValues_0)
        Me.fraParameters.Controls.Add(Me._cboParameterValues_7)
        Me.fraParameters.Controls.Add(Me._cboParameterValues_8)
        Me.fraParameters.Controls.Add(Me._cboParameterValues_9)
        Me.fraParameters.Controls.Add(Me._cboParameterValues_10)
        Me.fraParameters.Controls.Add(Me._cboParameterValues_12)
        Me.fraParameters.Controls.Add(Me._cboParameterValues_13)
        Me.fraParameters.Controls.Add(Me.lblFrequencyType)
        Me.fraParameters.Controls.Add(Me.lblFrequencyPrompt)
        Me.fraParameters.Controls.Add(Me._lblParameterName_13)
        Me.fraParameters.Controls.Add(Me._lblParameterType_13)
        Me.fraParameters.Controls.Add(Me._lblParameterName_12)
        Me.fraParameters.Controls.Add(Me._lblParameterType_12)
        Me.fraParameters.Controls.Add(Me._lblParameterType_11)
        Me.fraParameters.Controls.Add(Me._lblParameterName_11)
        Me.fraParameters.Controls.Add(Me.lblGroupBy)
        Me.fraParameters.Controls.Add(Me._lblParameterName_10)
        Me.fraParameters.Controls.Add(Me._lblParameterType_10)
        Me.fraParameters.Controls.Add(Me._lblParameterName_9)
        Me.fraParameters.Controls.Add(Me._lblParameterType_9)
        Me.fraParameters.Controls.Add(Me._lblParameterName_8)
        Me.fraParameters.Controls.Add(Me._lblParameterType_8)
        Me.fraParameters.Controls.Add(Me._lblParameterName_7)
        Me.fraParameters.Controls.Add(Me._lblParameterType_7)
        Me.fraParameters.Controls.Add(Me._lblParameterName_0)
        Me.fraParameters.Controls.Add(Me._lblParameterName_1)
        Me.fraParameters.Controls.Add(Me._lblParameterName_2)
        Me.fraParameters.Controls.Add(Me._lblParameterName_3)
        Me.fraParameters.Controls.Add(Me._lblParameterName_4)
        Me.fraParameters.Controls.Add(Me._lblParameterType_0)
        Me.fraParameters.Controls.Add(Me._lblParameterType_1)
        Me.fraParameters.Controls.Add(Me._lblParameterType_2)
        Me.fraParameters.Controls.Add(Me._lblParameterType_3)
        Me.fraParameters.Controls.Add(Me._lblParameterType_4)
        Me.fraParameters.Controls.Add(Me._lblParameterType_5)
        Me.fraParameters.Controls.Add(Me._lblParameterName_5)
        Me.fraParameters.Controls.Add(Me._lblParameterName_6)
        Me.fraParameters.Controls.Add(Me._lblParameterType_6)
        Me.fraParameters.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraParameters.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraParameters.Location = New System.Drawing.Point(8, 12)
        Me.fraParameters.Name = "fraParameters"
        Me.fraParameters.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraParameters.Size = New System.Drawing.Size(609, 513)
        Me.fraParameters.TabIndex = 3
        Me.fraParameters.TabStop = False
        Me.fraParameters.Text = "Enter Parameters Required to produce report"
        '
        'ListBox13
        '
        Me.ListBox13.FormattingEnabled = True
        Me.ListBox13.Location = New System.Drawing.Point(368, 472)
        Me.ListBox13.Name = "ListBox13"
        Me.ListBox13.Size = New System.Drawing.Size(171, 17)
        Me.ListBox13.TabIndex = 115
        Me.ListBox13.Visible = False
        '
        'ListBox12
        '
        Me.ListBox12.FormattingEnabled = True
        Me.ListBox12.Location = New System.Drawing.Point(368, 440)
        Me.ListBox12.Name = "ListBox12"
        Me.ListBox12.Size = New System.Drawing.Size(171, 17)
        Me.ListBox12.TabIndex = 114
        Me.ListBox12.Visible = False
        '
        'ListBox11
        '
        Me.ListBox11.FormattingEnabled = True
        Me.ListBox11.Location = New System.Drawing.Point(368, 408)
        Me.ListBox11.Name = "ListBox11"
        Me.ListBox11.Size = New System.Drawing.Size(171, 17)
        Me.ListBox11.TabIndex = 113
        Me.ListBox11.Visible = False
        '
        'ListBox10
        '
        Me.ListBox10.FormattingEnabled = True
        Me.ListBox10.Location = New System.Drawing.Point(368, 376)
        Me.ListBox10.Name = "ListBox10"
        Me.ListBox10.Size = New System.Drawing.Size(171, 17)
        Me.ListBox10.TabIndex = 112
        Me.ListBox10.Visible = False
        '
        'ListBox9
        '
        Me.ListBox9.FormattingEnabled = True
        Me.ListBox9.Location = New System.Drawing.Point(368, 344)
        Me.ListBox9.Name = "ListBox9"
        Me.ListBox9.Size = New System.Drawing.Size(171, 17)
        Me.ListBox9.TabIndex = 111
        Me.ListBox9.Visible = False
        '
        'ListBox8
        '
        Me.ListBox8.FormattingEnabled = True
        Me.ListBox8.Location = New System.Drawing.Point(368, 312)
        Me.ListBox8.Name = "ListBox8"
        Me.ListBox8.Size = New System.Drawing.Size(171, 17)
        Me.ListBox8.TabIndex = 110
        Me.ListBox8.Visible = False
        '
        'ListBox7
        '
        Me.ListBox7.FormattingEnabled = True
        Me.ListBox7.Location = New System.Drawing.Point(368, 280)
        Me.ListBox7.Name = "ListBox7"
        Me.ListBox7.Size = New System.Drawing.Size(171, 17)
        Me.ListBox7.TabIndex = 109
        Me.ListBox7.Visible = False
        '
        'ListBox6
        '
        Me.ListBox6.FormattingEnabled = True
        Me.ListBox6.Location = New System.Drawing.Point(368, 248)
        Me.ListBox6.Name = "ListBox6"
        Me.ListBox6.Size = New System.Drawing.Size(171, 17)
        Me.ListBox6.TabIndex = 108
        Me.ListBox6.Visible = False
        '
        'ListBox5
        '
        Me.ListBox5.FormattingEnabled = True
        Me.ListBox5.Location = New System.Drawing.Point(368, 216)
        Me.ListBox5.Name = "ListBox5"
        Me.ListBox5.Size = New System.Drawing.Size(171, 17)
        Me.ListBox5.TabIndex = 107
        Me.ListBox5.Visible = False
        '
        'ListBox4
        '
        Me.ListBox4.FormattingEnabled = True
        Me.ListBox4.Location = New System.Drawing.Point(368, 184)
        Me.ListBox4.Name = "ListBox4"
        Me.ListBox4.Size = New System.Drawing.Size(171, 17)
        Me.ListBox4.TabIndex = 106
        Me.ListBox4.Visible = False
        '
        'ListBox3
        '
        Me.ListBox3.FormattingEnabled = True
        Me.ListBox3.Location = New System.Drawing.Point(368, 152)
        Me.ListBox3.Name = "ListBox3"
        Me.ListBox3.Size = New System.Drawing.Size(171, 17)
        Me.ListBox3.TabIndex = 105
        Me.ListBox3.Visible = False
        '
        'ListBox2
        '
        Me.ListBox2.FormattingEnabled = True
        Me.ListBox2.Location = New System.Drawing.Point(368, 122)
        Me.ListBox2.Name = "ListBox2"
        Me.ListBox2.Size = New System.Drawing.Size(171, 17)
        Me.ListBox2.TabIndex = 104
        Me.ListBox2.Visible = False
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(368, 88)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(171, 17)
        Me.ListBox1.TabIndex = 103
        Me.ListBox1.Visible = False
        '
        'ListBox0
        '
        Me.ListBox0.FormattingEnabled = True
        Me.ListBox0.Location = New System.Drawing.Point(368, 56)
        Me.ListBox0.Name = "ListBox0"
        Me.ListBox0.Size = New System.Drawing.Size(171, 17)
        Me.ListBox0.TabIndex = 102
        Me.ListBox0.Visible = False
        '
        'cboFrequency
        '
        Me.cboFrequency.DefaultItemId = 0
        Me.cboFrequency.FirstItem = ""
        Me.cboFrequency.ItemId = 0
        Me.cboFrequency.ListIndex = -1
        Me.cboFrequency.Location = New System.Drawing.Point(368, 24)
        Me.cboFrequency.Name = "cboFrequency"
        Me.cboFrequency.PMLookupProductFamily = 1
        Me.cboFrequency.SingleItemId = 0
        Me.cboFrequency.Size = New System.Drawing.Size(169, 21)
        Me.cboFrequency.Sorted = True
        Me.cboFrequency.TabIndex = 101
        Me.cboFrequency.TableName = "Scheduled_Report_Frequency"
        Me.cboFrequency.ToolTipText = ""
        Me.cboFrequency.Visible = False
        Me.cboFrequency.WhereClause = ""
        '
        '_DTParameters_13
        '
        Me._DTParameters_13.Location = New System.Drawing.Point(368, 472)
        Me._DTParameters_13.Name = "_DTParameters_13"
        Me._DTParameters_13.Size = New System.Drawing.Size(169, 20)
        Me._DTParameters_13.TabIndex = 97
        Me._DTParameters_13.Visible = False
        '
        '_DTParameters_12
        '
        Me._DTParameters_12.Location = New System.Drawing.Point(368, 440)
        Me._DTParameters_12.Name = "_DTParameters_12"
        Me._DTParameters_12.Size = New System.Drawing.Size(169, 20)
        Me._DTParameters_12.TabIndex = 96
        Me._DTParameters_12.Visible = False
        '
        '_DTParameters_11
        '
        Me._DTParameters_11.Location = New System.Drawing.Point(368, 408)
        Me._DTParameters_11.Name = "_DTParameters_11"
        Me._DTParameters_11.Size = New System.Drawing.Size(169, 20)
        Me._DTParameters_11.TabIndex = 95
        Me._DTParameters_11.Visible = False
        '
        '_DTParameters_10
        '
        Me._DTParameters_10.Location = New System.Drawing.Point(368, 376)
        Me._DTParameters_10.Name = "_DTParameters_10"
        Me._DTParameters_10.Size = New System.Drawing.Size(169, 20)
        Me._DTParameters_10.TabIndex = 94
        Me._DTParameters_10.Visible = False
        '
        '_DTParameters_9
        '
        Me._DTParameters_9.Location = New System.Drawing.Point(368, 344)
        Me._DTParameters_9.Name = "_DTParameters_9"
        Me._DTParameters_9.Size = New System.Drawing.Size(169, 20)
        Me._DTParameters_9.TabIndex = 93
        Me._DTParameters_9.Visible = False
        '
        '_DTParameters_8
        '
        Me._DTParameters_8.Location = New System.Drawing.Point(368, 312)
        Me._DTParameters_8.Name = "_DTParameters_8"
        Me._DTParameters_8.Size = New System.Drawing.Size(169, 20)
        Me._DTParameters_8.TabIndex = 92
        Me._DTParameters_8.Visible = False
        '
        '_DTParameters_7
        '
        Me._DTParameters_7.Location = New System.Drawing.Point(368, 280)
        Me._DTParameters_7.Name = "_DTParameters_7"
        Me._DTParameters_7.Size = New System.Drawing.Size(169, 20)
        Me._DTParameters_7.TabIndex = 91
        Me._DTParameters_7.Visible = False
        '
        '_DTParameters_6
        '
        Me._DTParameters_6.Location = New System.Drawing.Point(368, 248)
        Me._DTParameters_6.Name = "_DTParameters_6"
        Me._DTParameters_6.Size = New System.Drawing.Size(169, 20)
        Me._DTParameters_6.TabIndex = 90
        Me._DTParameters_6.Visible = False
        '
        '_DTParameters_5
        '
        Me._DTParameters_5.Location = New System.Drawing.Point(368, 216)
        Me._DTParameters_5.Name = "_DTParameters_5"
        Me._DTParameters_5.Size = New System.Drawing.Size(169, 20)
        Me._DTParameters_5.TabIndex = 89
        Me._DTParameters_5.Visible = False
        '
        '_DTParameters_4
        '
        Me._DTParameters_4.Location = New System.Drawing.Point(368, 184)
        Me._DTParameters_4.Name = "_DTParameters_4"
        Me._DTParameters_4.Size = New System.Drawing.Size(169, 20)
        Me._DTParameters_4.TabIndex = 88
        Me._DTParameters_4.Visible = False
        '
        '_DTParameters_3
        '
        Me._DTParameters_3.Location = New System.Drawing.Point(368, 152)
        Me._DTParameters_3.Name = "_DTParameters_3"
        Me._DTParameters_3.Size = New System.Drawing.Size(169, 20)
        Me._DTParameters_3.TabIndex = 87
        Me._DTParameters_3.Visible = False
        '
        '_DTParameters_2
        '
        Me._DTParameters_2.Location = New System.Drawing.Point(368, 120)
        Me._DTParameters_2.Name = "_DTParameters_2"
        Me._DTParameters_2.Size = New System.Drawing.Size(169, 20)
        Me._DTParameters_2.TabIndex = 86
        Me._DTParameters_2.Visible = False
        '
        '_DTParameters_1
        '
        Me._DTParameters_1.Location = New System.Drawing.Point(368, 88)
        Me._DTParameters_1.Name = "_DTParameters_1"
        Me._DTParameters_1.Size = New System.Drawing.Size(169, 20)
        Me._DTParameters_1.TabIndex = 85
        Me._DTParameters_1.Visible = False
        '
        '_DTParameters_0
        '
        Me._DTParameters_0.Location = New System.Drawing.Point(368, 58)
        Me._DTParameters_0.Name = "_DTParameters_0"
        Me._DTParameters_0.Size = New System.Drawing.Size(169, 20)
        Me._DTParameters_0.TabIndex = 84
        Me._DTParameters_0.Visible = False
        '
        '_chkGroupBy_13
        '
        Me._chkGroupBy_13.BackColor = System.Drawing.SystemColors.Control
        Me._chkGroupBy_13.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkGroupBy_13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkGroupBy_13.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkGroupBy_13.Location = New System.Drawing.Point(552, 472)
        Me._chkGroupBy_13.Name = "_chkGroupBy_13"
        Me._chkGroupBy_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkGroupBy_13.Size = New System.Drawing.Size(17, 17)
        Me._chkGroupBy_13.TabIndex = 80
        Me._chkGroupBy_13.UseVisualStyleBackColor = False
        Me._chkGroupBy_13.Visible = False
        '
        '_cmdParameterValues_13
        '
        Me._cmdParameterValues_13.BackColor = System.Drawing.SystemColors.Control
        Me._cmdParameterValues_13.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdParameterValues_13.Enabled = False
        Me._cmdParameterValues_13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdParameterValues_13.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdParameterValues_13.Location = New System.Drawing.Point(518, 474)
        Me._cmdParameterValues_13.Name = "_cmdParameterValues_13"
        Me._cmdParameterValues_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdParameterValues_13.Size = New System.Drawing.Size(17, 18)
        Me._cmdParameterValues_13.TabIndex = 79
        Me._cmdParameterValues_13.Text = ".."
        Me._cmdParameterValues_13.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdParameterValues_13.UseVisualStyleBackColor = False
        Me._cmdParameterValues_13.Visible = False
        '
        '_chkGroupBy_12
        '
        Me._chkGroupBy_12.BackColor = System.Drawing.SystemColors.Control
        Me._chkGroupBy_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkGroupBy_12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkGroupBy_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkGroupBy_12.Location = New System.Drawing.Point(552, 440)
        Me._chkGroupBy_12.Name = "_chkGroupBy_12"
        Me._chkGroupBy_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkGroupBy_12.Size = New System.Drawing.Size(17, 17)
        Me._chkGroupBy_12.TabIndex = 75
        Me._chkGroupBy_12.UseVisualStyleBackColor = False
        Me._chkGroupBy_12.Visible = False
        '
        '_cmdParameterValues_12
        '
        Me._cmdParameterValues_12.BackColor = System.Drawing.SystemColors.Control
        Me._cmdParameterValues_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdParameterValues_12.Enabled = False
        Me._cmdParameterValues_12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdParameterValues_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdParameterValues_12.Location = New System.Drawing.Point(518, 442)
        Me._cmdParameterValues_12.Name = "_cmdParameterValues_12"
        Me._cmdParameterValues_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdParameterValues_12.Size = New System.Drawing.Size(17, 18)
        Me._cmdParameterValues_12.TabIndex = 74
        Me._cmdParameterValues_12.Text = ".."
        Me._cmdParameterValues_12.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdParameterValues_12.UseVisualStyleBackColor = False
        Me._cmdParameterValues_12.Visible = False
        '
        '_cmdParameterValues_11
        '
        Me._cmdParameterValues_11.BackColor = System.Drawing.SystemColors.Control
        Me._cmdParameterValues_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdParameterValues_11.Enabled = False
        Me._cmdParameterValues_11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdParameterValues_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdParameterValues_11.Location = New System.Drawing.Point(518, 410)
        Me._cmdParameterValues_11.Name = "_cmdParameterValues_11"
        Me._cmdParameterValues_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdParameterValues_11.Size = New System.Drawing.Size(17, 18)
        Me._cmdParameterValues_11.TabIndex = 64
        Me._cmdParameterValues_11.Text = ".."
        Me._cmdParameterValues_11.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdParameterValues_11.UseVisualStyleBackColor = False
        Me._cmdParameterValues_11.Visible = False
        '
        '_chkGroupBy_11
        '
        Me._chkGroupBy_11.BackColor = System.Drawing.SystemColors.Control
        Me._chkGroupBy_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkGroupBy_11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkGroupBy_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkGroupBy_11.Location = New System.Drawing.Point(552, 408)
        Me._chkGroupBy_11.Name = "_chkGroupBy_11"
        Me._chkGroupBy_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkGroupBy_11.Size = New System.Drawing.Size(17, 17)
        Me._chkGroupBy_11.TabIndex = 63
        Me._chkGroupBy_11.UseVisualStyleBackColor = False
        Me._chkGroupBy_11.Visible = False
        '
        '_cboParameterValues_11
        '
        Me._cboParameterValues_11.BackColor = System.Drawing.SystemColors.Window
        Me._cboParameterValues_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboParameterValues_11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboParameterValues_11.ForeColor = System.Drawing.SystemColors.WindowText
        Me._cboParameterValues_11.Location = New System.Drawing.Point(368, 408)
        Me._cboParameterValues_11.Name = "_cboParameterValues_11"
        Me._cboParameterValues_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboParameterValues_11.Size = New System.Drawing.Size(169, 21)
        Me._cboParameterValues_11.TabIndex = 62
        Me._cboParameterValues_11.Text = "Combo1"
        '
        '_chkGroupBy_10
        '
        Me._chkGroupBy_10.BackColor = System.Drawing.SystemColors.Control
        Me._chkGroupBy_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkGroupBy_10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkGroupBy_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkGroupBy_10.Location = New System.Drawing.Point(552, 376)
        Me._chkGroupBy_10.Name = "_chkGroupBy_10"
        Me._chkGroupBy_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkGroupBy_10.Size = New System.Drawing.Size(17, 17)
        Me._chkGroupBy_10.TabIndex = 58
        Me._chkGroupBy_10.UseVisualStyleBackColor = False
        Me._chkGroupBy_10.Visible = False
        '
        '_chkGroupBy_9
        '
        Me._chkGroupBy_9.BackColor = System.Drawing.SystemColors.Control
        Me._chkGroupBy_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkGroupBy_9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkGroupBy_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkGroupBy_9.Location = New System.Drawing.Point(552, 336)
        Me._chkGroupBy_9.Name = "_chkGroupBy_9"
        Me._chkGroupBy_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkGroupBy_9.Size = New System.Drawing.Size(17, 33)
        Me._chkGroupBy_9.TabIndex = 57
        Me._chkGroupBy_9.UseVisualStyleBackColor = False
        Me._chkGroupBy_9.Visible = False
        '
        '_chkGroupBy_8
        '
        Me._chkGroupBy_8.BackColor = System.Drawing.SystemColors.Control
        Me._chkGroupBy_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkGroupBy_8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkGroupBy_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkGroupBy_8.Location = New System.Drawing.Point(552, 304)
        Me._chkGroupBy_8.Name = "_chkGroupBy_8"
        Me._chkGroupBy_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkGroupBy_8.Size = New System.Drawing.Size(17, 33)
        Me._chkGroupBy_8.TabIndex = 56
        Me._chkGroupBy_8.UseVisualStyleBackColor = False
        Me._chkGroupBy_8.Visible = False
        '
        '_chkGroupBy_7
        '
        Me._chkGroupBy_7.BackColor = System.Drawing.SystemColors.Control
        Me._chkGroupBy_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkGroupBy_7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkGroupBy_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkGroupBy_7.Location = New System.Drawing.Point(552, 272)
        Me._chkGroupBy_7.Name = "_chkGroupBy_7"
        Me._chkGroupBy_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkGroupBy_7.Size = New System.Drawing.Size(17, 33)
        Me._chkGroupBy_7.TabIndex = 55
        Me._chkGroupBy_7.UseVisualStyleBackColor = False
        Me._chkGroupBy_7.Visible = False
        '
        '_chkGroupBy_6
        '
        Me._chkGroupBy_6.BackColor = System.Drawing.SystemColors.Control
        Me._chkGroupBy_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkGroupBy_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkGroupBy_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkGroupBy_6.Location = New System.Drawing.Point(552, 240)
        Me._chkGroupBy_6.Name = "_chkGroupBy_6"
        Me._chkGroupBy_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkGroupBy_6.Size = New System.Drawing.Size(17, 33)
        Me._chkGroupBy_6.TabIndex = 54
        Me._chkGroupBy_6.UseVisualStyleBackColor = False
        Me._chkGroupBy_6.Visible = False
        '
        '_chkGroupBy_5
        '
        Me._chkGroupBy_5.BackColor = System.Drawing.SystemColors.Control
        Me._chkGroupBy_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkGroupBy_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkGroupBy_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkGroupBy_5.Location = New System.Drawing.Point(552, 208)
        Me._chkGroupBy_5.Name = "_chkGroupBy_5"
        Me._chkGroupBy_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkGroupBy_5.Size = New System.Drawing.Size(17, 33)
        Me._chkGroupBy_5.TabIndex = 53
        Me._chkGroupBy_5.UseVisualStyleBackColor = False
        Me._chkGroupBy_5.Visible = False
        '
        '_chkGroupBy_4
        '
        Me._chkGroupBy_4.BackColor = System.Drawing.SystemColors.Control
        Me._chkGroupBy_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkGroupBy_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkGroupBy_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkGroupBy_4.Location = New System.Drawing.Point(552, 176)
        Me._chkGroupBy_4.Name = "_chkGroupBy_4"
        Me._chkGroupBy_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkGroupBy_4.Size = New System.Drawing.Size(17, 33)
        Me._chkGroupBy_4.TabIndex = 52
        Me._chkGroupBy_4.UseVisualStyleBackColor = False
        Me._chkGroupBy_4.Visible = False
        '
        '_chkGroupBy_3
        '
        Me._chkGroupBy_3.BackColor = System.Drawing.SystemColors.Control
        Me._chkGroupBy_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkGroupBy_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkGroupBy_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkGroupBy_3.Location = New System.Drawing.Point(552, 144)
        Me._chkGroupBy_3.Name = "_chkGroupBy_3"
        Me._chkGroupBy_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkGroupBy_3.Size = New System.Drawing.Size(17, 33)
        Me._chkGroupBy_3.TabIndex = 51
        Me._chkGroupBy_3.UseVisualStyleBackColor = False
        Me._chkGroupBy_3.Visible = False
        '
        '_chkGroupBy_2
        '
        Me._chkGroupBy_2.BackColor = System.Drawing.SystemColors.Control
        Me._chkGroupBy_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkGroupBy_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkGroupBy_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkGroupBy_2.Location = New System.Drawing.Point(552, 112)
        Me._chkGroupBy_2.Name = "_chkGroupBy_2"
        Me._chkGroupBy_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkGroupBy_2.Size = New System.Drawing.Size(17, 33)
        Me._chkGroupBy_2.TabIndex = 50
        Me._chkGroupBy_2.UseVisualStyleBackColor = False
        Me._chkGroupBy_2.Visible = False
        '
        '_chkGroupBy_1
        '
        Me._chkGroupBy_1.BackColor = System.Drawing.SystemColors.Control
        Me._chkGroupBy_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkGroupBy_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkGroupBy_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkGroupBy_1.Location = New System.Drawing.Point(552, 80)
        Me._chkGroupBy_1.Name = "_chkGroupBy_1"
        Me._chkGroupBy_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkGroupBy_1.Size = New System.Drawing.Size(17, 33)
        Me._chkGroupBy_1.TabIndex = 49
        Me._chkGroupBy_1.UseVisualStyleBackColor = False
        Me._chkGroupBy_1.Visible = False
        '
        '_chkGroupBy_0
        '
        Me._chkGroupBy_0.BackColor = System.Drawing.SystemColors.Control
        Me._chkGroupBy_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkGroupBy_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkGroupBy_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkGroupBy_0.Location = New System.Drawing.Point(552, 56)
        Me._chkGroupBy_0.Name = "_chkGroupBy_0"
        Me._chkGroupBy_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkGroupBy_0.Size = New System.Drawing.Size(17, 17)
        Me._chkGroupBy_0.TabIndex = 48
        Me._chkGroupBy_0.UseVisualStyleBackColor = False
        Me._chkGroupBy_0.Visible = False
        '
        '_cmdParameterValues_10
        '
        Me._cmdParameterValues_10.BackColor = System.Drawing.SystemColors.Control
        Me._cmdParameterValues_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdParameterValues_10.Enabled = False
        Me._cmdParameterValues_10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdParameterValues_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdParameterValues_10.Location = New System.Drawing.Point(518, 378)
        Me._cmdParameterValues_10.Name = "_cmdParameterValues_10"
        Me._cmdParameterValues_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdParameterValues_10.Size = New System.Drawing.Size(17, 18)
        Me._cmdParameterValues_10.TabIndex = 44
        Me._cmdParameterValues_10.Text = ".."
        Me._cmdParameterValues_10.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdParameterValues_10.UseVisualStyleBackColor = False
        Me._cmdParameterValues_10.Visible = False
        '
        '_cmdParameterValues_9
        '
        Me._cmdParameterValues_9.BackColor = System.Drawing.SystemColors.Control
        Me._cmdParameterValues_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdParameterValues_9.Enabled = False
        Me._cmdParameterValues_9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdParameterValues_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdParameterValues_9.Location = New System.Drawing.Point(518, 346)
        Me._cmdParameterValues_9.Name = "_cmdParameterValues_9"
        Me._cmdParameterValues_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdParameterValues_9.Size = New System.Drawing.Size(17, 18)
        Me._cmdParameterValues_9.TabIndex = 40
        Me._cmdParameterValues_9.Text = ".."
        Me._cmdParameterValues_9.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdParameterValues_9.UseVisualStyleBackColor = False
        Me._cmdParameterValues_9.Visible = False
        '
        '_cmdParameterValues_8
        '
        Me._cmdParameterValues_8.BackColor = System.Drawing.SystemColors.Control
        Me._cmdParameterValues_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdParameterValues_8.Enabled = False
        Me._cmdParameterValues_8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdParameterValues_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdParameterValues_8.Location = New System.Drawing.Point(518, 314)
        Me._cmdParameterValues_8.Name = "_cmdParameterValues_8"
        Me._cmdParameterValues_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdParameterValues_8.Size = New System.Drawing.Size(17, 18)
        Me._cmdParameterValues_8.TabIndex = 36
        Me._cmdParameterValues_8.Text = ".."
        Me._cmdParameterValues_8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdParameterValues_8.UseVisualStyleBackColor = False
        Me._cmdParameterValues_8.Visible = False
        '
        '_cmdParameterValues_7
        '
        Me._cmdParameterValues_7.BackColor = System.Drawing.SystemColors.Control
        Me._cmdParameterValues_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdParameterValues_7.Enabled = False
        Me._cmdParameterValues_7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdParameterValues_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdParameterValues_7.Location = New System.Drawing.Point(518, 282)
        Me._cmdParameterValues_7.Name = "_cmdParameterValues_7"
        Me._cmdParameterValues_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdParameterValues_7.Size = New System.Drawing.Size(17, 18)
        Me._cmdParameterValues_7.TabIndex = 32
        Me._cmdParameterValues_7.Text = ".."
        Me._cmdParameterValues_7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdParameterValues_7.UseVisualStyleBackColor = False
        Me._cmdParameterValues_7.Visible = False
        '
        '_cmdParameterValues_0
        '
        Me._cmdParameterValues_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdParameterValues_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdParameterValues_0.Enabled = False
        Me._cmdParameterValues_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdParameterValues_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdParameterValues_0.Location = New System.Drawing.Point(518, 58)
        Me._cmdParameterValues_0.Name = "_cmdParameterValues_0"
        Me._cmdParameterValues_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdParameterValues_0.Size = New System.Drawing.Size(17, 18)
        Me._cmdParameterValues_0.TabIndex = 10
        Me._cmdParameterValues_0.Text = ".."
        Me._cmdParameterValues_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdParameterValues_0.UseVisualStyleBackColor = False
        Me._cmdParameterValues_0.Visible = False
        '
        '_cmdParameterValues_1
        '
        Me._cmdParameterValues_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdParameterValues_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdParameterValues_1.Enabled = False
        Me._cmdParameterValues_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdParameterValues_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdParameterValues_1.Location = New System.Drawing.Point(518, 90)
        Me._cmdParameterValues_1.Name = "_cmdParameterValues_1"
        Me._cmdParameterValues_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdParameterValues_1.Size = New System.Drawing.Size(17, 18)
        Me._cmdParameterValues_1.TabIndex = 9
        Me._cmdParameterValues_1.Text = ".."
        Me._cmdParameterValues_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdParameterValues_1.UseVisualStyleBackColor = False
        Me._cmdParameterValues_1.Visible = False
        '
        '_cmdParameterValues_6
        '
        Me._cmdParameterValues_6.BackColor = System.Drawing.SystemColors.Control
        Me._cmdParameterValues_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdParameterValues_6.Enabled = False
        Me._cmdParameterValues_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdParameterValues_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdParameterValues_6.Location = New System.Drawing.Point(518, 250)
        Me._cmdParameterValues_6.Name = "_cmdParameterValues_6"
        Me._cmdParameterValues_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdParameterValues_6.Size = New System.Drawing.Size(17, 18)
        Me._cmdParameterValues_6.TabIndex = 8
        Me._cmdParameterValues_6.Text = ".."
        Me._cmdParameterValues_6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdParameterValues_6.UseVisualStyleBackColor = False
        Me._cmdParameterValues_6.Visible = False
        '
        '_cmdParameterValues_5
        '
        Me._cmdParameterValues_5.BackColor = System.Drawing.SystemColors.Control
        Me._cmdParameterValues_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdParameterValues_5.Enabled = False
        Me._cmdParameterValues_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdParameterValues_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdParameterValues_5.Location = New System.Drawing.Point(518, 218)
        Me._cmdParameterValues_5.Name = "_cmdParameterValues_5"
        Me._cmdParameterValues_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdParameterValues_5.Size = New System.Drawing.Size(17, 18)
        Me._cmdParameterValues_5.TabIndex = 7
        Me._cmdParameterValues_5.Text = ".."
        Me._cmdParameterValues_5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdParameterValues_5.UseVisualStyleBackColor = False
        Me._cmdParameterValues_5.Visible = False
        '
        '_cmdParameterValues_4
        '
        Me._cmdParameterValues_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdParameterValues_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdParameterValues_4.Enabled = False
        Me._cmdParameterValues_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdParameterValues_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdParameterValues_4.Location = New System.Drawing.Point(518, 186)
        Me._cmdParameterValues_4.Name = "_cmdParameterValues_4"
        Me._cmdParameterValues_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdParameterValues_4.Size = New System.Drawing.Size(17, 18)
        Me._cmdParameterValues_4.TabIndex = 6
        Me._cmdParameterValues_4.Text = ".."
        Me._cmdParameterValues_4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdParameterValues_4.UseVisualStyleBackColor = False
        Me._cmdParameterValues_4.Visible = False
        '
        '_cmdParameterValues_3
        '
        Me._cmdParameterValues_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdParameterValues_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdParameterValues_3.Enabled = False
        Me._cmdParameterValues_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdParameterValues_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdParameterValues_3.Location = New System.Drawing.Point(518, 154)
        Me._cmdParameterValues_3.Name = "_cmdParameterValues_3"
        Me._cmdParameterValues_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdParameterValues_3.Size = New System.Drawing.Size(17, 18)
        Me._cmdParameterValues_3.TabIndex = 5
        Me._cmdParameterValues_3.Text = ".."
        Me._cmdParameterValues_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdParameterValues_3.UseVisualStyleBackColor = False
        Me._cmdParameterValues_3.Visible = False
        '
        '_cmdParameterValues_2
        '
        Me._cmdParameterValues_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdParameterValues_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdParameterValues_2.Enabled = False
        Me._cmdParameterValues_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdParameterValues_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdParameterValues_2.Location = New System.Drawing.Point(518, 122)
        Me._cmdParameterValues_2.Name = "_cmdParameterValues_2"
        Me._cmdParameterValues_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdParameterValues_2.Size = New System.Drawing.Size(17, 18)
        Me._cmdParameterValues_2.TabIndex = 4
        Me._cmdParameterValues_2.Text = ".."
        Me._cmdParameterValues_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdParameterValues_2.UseVisualStyleBackColor = False
        Me._cmdParameterValues_2.Visible = False
        '
        '_cboParameterValues_6
        '
        Me._cboParameterValues_6.BackColor = System.Drawing.SystemColors.Window
        Me._cboParameterValues_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboParameterValues_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboParameterValues_6.ForeColor = System.Drawing.SystemColors.WindowText
        Me._cboParameterValues_6.Location = New System.Drawing.Point(368, 248)
        Me._cboParameterValues_6.Name = "_cboParameterValues_6"
        Me._cboParameterValues_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboParameterValues_6.Size = New System.Drawing.Size(169, 21)
        Me._cboParameterValues_6.TabIndex = 11
        Me._cboParameterValues_6.Text = "Combo1"
        '
        '_cboParameterValues_5
        '
        Me._cboParameterValues_5.BackColor = System.Drawing.SystemColors.Window
        Me._cboParameterValues_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboParameterValues_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboParameterValues_5.ForeColor = System.Drawing.SystemColors.WindowText
        Me._cboParameterValues_5.Location = New System.Drawing.Point(368, 216)
        Me._cboParameterValues_5.Name = "_cboParameterValues_5"
        Me._cboParameterValues_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboParameterValues_5.Size = New System.Drawing.Size(169, 21)
        Me._cboParameterValues_5.TabIndex = 12
        Me._cboParameterValues_5.Text = "Combo1"
        '
        '_cboParameterValues_4
        '
        Me._cboParameterValues_4.BackColor = System.Drawing.SystemColors.Window
        Me._cboParameterValues_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboParameterValues_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboParameterValues_4.ForeColor = System.Drawing.SystemColors.WindowText
        Me._cboParameterValues_4.Location = New System.Drawing.Point(368, 184)
        Me._cboParameterValues_4.Name = "_cboParameterValues_4"
        Me._cboParameterValues_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboParameterValues_4.Size = New System.Drawing.Size(169, 21)
        Me._cboParameterValues_4.TabIndex = 13
        Me._cboParameterValues_4.Text = "Combo1"
        '
        '_cboParameterValues_3
        '
        Me._cboParameterValues_3.BackColor = System.Drawing.SystemColors.Window
        Me._cboParameterValues_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboParameterValues_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboParameterValues_3.ForeColor = System.Drawing.SystemColors.WindowText
        Me._cboParameterValues_3.Location = New System.Drawing.Point(368, 152)
        Me._cboParameterValues_3.Name = "_cboParameterValues_3"
        Me._cboParameterValues_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboParameterValues_3.Size = New System.Drawing.Size(169, 21)
        Me._cboParameterValues_3.TabIndex = 14
        Me._cboParameterValues_3.Text = "Combo1"
        '
        '_cboParameterValues_2
        '
        Me._cboParameterValues_2.BackColor = System.Drawing.SystemColors.Window
        Me._cboParameterValues_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboParameterValues_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboParameterValues_2.ForeColor = System.Drawing.SystemColors.WindowText
        Me._cboParameterValues_2.Location = New System.Drawing.Point(368, 120)
        Me._cboParameterValues_2.Name = "_cboParameterValues_2"
        Me._cboParameterValues_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboParameterValues_2.Size = New System.Drawing.Size(169, 21)
        Me._cboParameterValues_2.TabIndex = 15
        Me._cboParameterValues_2.Text = "Combo1"
        '
        '_cboParameterValues_1
        '
        Me._cboParameterValues_1.BackColor = System.Drawing.SystemColors.Window
        Me._cboParameterValues_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboParameterValues_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboParameterValues_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me._cboParameterValues_1.Location = New System.Drawing.Point(368, 88)
        Me._cboParameterValues_1.Name = "_cboParameterValues_1"
        Me._cboParameterValues_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboParameterValues_1.Size = New System.Drawing.Size(169, 21)
        Me._cboParameterValues_1.TabIndex = 16
        Me._cboParameterValues_1.Text = "Combo1"
        '
        '_cboParameterValues_0
        '
        Me._cboParameterValues_0.BackColor = System.Drawing.SystemColors.Window
        Me._cboParameterValues_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboParameterValues_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboParameterValues_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._cboParameterValues_0.Location = New System.Drawing.Point(368, 56)
        Me._cboParameterValues_0.Name = "_cboParameterValues_0"
        Me._cboParameterValues_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboParameterValues_0.Size = New System.Drawing.Size(169, 21)
        Me._cboParameterValues_0.TabIndex = 17
        Me._cboParameterValues_0.Text = "Combo1"
        '
        '_cboParameterValues_7
        '
        Me._cboParameterValues_7.BackColor = System.Drawing.SystemColors.Window
        Me._cboParameterValues_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboParameterValues_7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboParameterValues_7.ForeColor = System.Drawing.SystemColors.WindowText
        Me._cboParameterValues_7.Location = New System.Drawing.Point(368, 280)
        Me._cboParameterValues_7.Name = "_cboParameterValues_7"
        Me._cboParameterValues_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboParameterValues_7.Size = New System.Drawing.Size(169, 21)
        Me._cboParameterValues_7.TabIndex = 33
        Me._cboParameterValues_7.Text = "Combo1"
        '
        '_cboParameterValues_8
        '
        Me._cboParameterValues_8.BackColor = System.Drawing.SystemColors.Window
        Me._cboParameterValues_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboParameterValues_8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboParameterValues_8.ForeColor = System.Drawing.SystemColors.WindowText
        Me._cboParameterValues_8.Location = New System.Drawing.Point(368, 312)
        Me._cboParameterValues_8.Name = "_cboParameterValues_8"
        Me._cboParameterValues_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboParameterValues_8.Size = New System.Drawing.Size(169, 21)
        Me._cboParameterValues_8.TabIndex = 37
        Me._cboParameterValues_8.Text = "Combo1"
        '
        '_cboParameterValues_9
        '
        Me._cboParameterValues_9.BackColor = System.Drawing.SystemColors.Window
        Me._cboParameterValues_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboParameterValues_9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboParameterValues_9.ForeColor = System.Drawing.SystemColors.WindowText
        Me._cboParameterValues_9.Location = New System.Drawing.Point(368, 344)
        Me._cboParameterValues_9.Name = "_cboParameterValues_9"
        Me._cboParameterValues_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboParameterValues_9.Size = New System.Drawing.Size(169, 21)
        Me._cboParameterValues_9.TabIndex = 41
        Me._cboParameterValues_9.Text = "Combo1"
        '
        '_cboParameterValues_10
        '
        Me._cboParameterValues_10.BackColor = System.Drawing.SystemColors.Window
        Me._cboParameterValues_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboParameterValues_10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboParameterValues_10.ForeColor = System.Drawing.SystemColors.WindowText
        Me._cboParameterValues_10.Location = New System.Drawing.Point(368, 376)
        Me._cboParameterValues_10.Name = "_cboParameterValues_10"
        Me._cboParameterValues_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboParameterValues_10.Size = New System.Drawing.Size(169, 21)
        Me._cboParameterValues_10.TabIndex = 45
        Me._cboParameterValues_10.Text = "Combo1"
        '
        '_cboParameterValues_12
        '
        Me._cboParameterValues_12.BackColor = System.Drawing.SystemColors.Window
        Me._cboParameterValues_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboParameterValues_12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboParameterValues_12.ForeColor = System.Drawing.SystemColors.WindowText
        Me._cboParameterValues_12.Location = New System.Drawing.Point(368, 440)
        Me._cboParameterValues_12.Name = "_cboParameterValues_12"
        Me._cboParameterValues_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboParameterValues_12.Size = New System.Drawing.Size(169, 21)
        Me._cboParameterValues_12.TabIndex = 76
        Me._cboParameterValues_12.Text = "Combo1"
        '
        '_cboParameterValues_13
        '
        Me._cboParameterValues_13.BackColor = System.Drawing.SystemColors.Window
        Me._cboParameterValues_13.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboParameterValues_13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboParameterValues_13.ForeColor = System.Drawing.SystemColors.WindowText
        Me._cboParameterValues_13.Location = New System.Drawing.Point(368, 472)
        Me._cboParameterValues_13.Name = "_cboParameterValues_13"
        Me._cboParameterValues_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboParameterValues_13.Size = New System.Drawing.Size(169, 21)
        Me._cboParameterValues_13.TabIndex = 81
        Me._cboParameterValues_13.Text = "Combo1"
        '
        'lblFrequencyType
        '
        Me.lblFrequencyType.AutoSize = True
        Me.lblFrequencyType.BackColor = System.Drawing.SystemColors.Control
        Me.lblFrequencyType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFrequencyType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFrequencyType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFrequencyType.Location = New System.Drawing.Point(288, 24)
        Me.lblFrequencyType.Name = "lblFrequencyType"
        Me.lblFrequencyType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFrequencyType.Size = New System.Drawing.Size(34, 13)
        Me.lblFrequencyType.TabIndex = 100
        Me.lblFrequencyType.Text = "(Text)"
        '
        'lblFrequencyPrompt
        '
        Me.lblFrequencyPrompt.AutoSize = True
        Me.lblFrequencyPrompt.BackColor = System.Drawing.SystemColors.Control
        Me.lblFrequencyPrompt.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFrequencyPrompt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFrequencyPrompt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFrequencyPrompt.Location = New System.Drawing.Point(24, 24)
        Me.lblFrequencyPrompt.Name = "lblFrequencyPrompt"
        Me.lblFrequencyPrompt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFrequencyPrompt.Size = New System.Drawing.Size(60, 13)
        Me.lblFrequencyPrompt.TabIndex = 99
        Me.lblFrequencyPrompt.Text = "Frequency:"
        '
        '_lblParameterName_13
        '
        Me._lblParameterName_13.AutoSize = True
        Me._lblParameterName_13.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterName_13.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterName_13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterName_13.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterName_13.Location = New System.Drawing.Point(24, 472)
        Me._lblParameterName_13.Name = "_lblParameterName_13"
        Me._lblParameterName_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterName_13.Size = New System.Drawing.Size(45, 13)
        Me._lblParameterName_13.TabIndex = 83
        Me._lblParameterName_13.Text = "Label14"
        '
        '_lblParameterType_13
        '
        Me._lblParameterType_13.AutoSize = True
        Me._lblParameterType_13.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterType_13.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterType_13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterType_13.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterType_13.Location = New System.Drawing.Point(288, 472)
        Me._lblParameterType_13.Name = "_lblParameterType_13"
        Me._lblParameterType_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterType_13.Size = New System.Drawing.Size(39, 13)
        Me._lblParameterType_13.TabIndex = 82
        Me._lblParameterType_13.Text = "Label1"
        '
        '_lblParameterName_12
        '
        Me._lblParameterName_12.AutoSize = True
        Me._lblParameterName_12.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterName_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterName_12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterName_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterName_12.Location = New System.Drawing.Point(24, 440)
        Me._lblParameterName_12.Name = "_lblParameterName_12"
        Me._lblParameterName_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterName_12.Size = New System.Drawing.Size(45, 13)
        Me._lblParameterName_12.TabIndex = 78
        Me._lblParameterName_12.Text = "Label13"
        '
        '_lblParameterType_12
        '
        Me._lblParameterType_12.AutoSize = True
        Me._lblParameterType_12.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterType_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterType_12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterType_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterType_12.Location = New System.Drawing.Point(288, 440)
        Me._lblParameterType_12.Name = "_lblParameterType_12"
        Me._lblParameterType_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterType_12.Size = New System.Drawing.Size(39, 13)
        Me._lblParameterType_12.TabIndex = 77
        Me._lblParameterType_12.Text = "Label1"
        '
        '_lblParameterType_11
        '
        Me._lblParameterType_11.AutoSize = True
        Me._lblParameterType_11.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterType_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterType_11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterType_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterType_11.Location = New System.Drawing.Point(288, 408)
        Me._lblParameterType_11.Name = "_lblParameterType_11"
        Me._lblParameterType_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterType_11.Size = New System.Drawing.Size(39, 13)
        Me._lblParameterType_11.TabIndex = 61
        Me._lblParameterType_11.Text = "Label1"
        '
        '_lblParameterName_11
        '
        Me._lblParameterName_11.AutoSize = True
        Me._lblParameterName_11.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterName_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterName_11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterName_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterName_11.Location = New System.Drawing.Point(24, 408)
        Me._lblParameterName_11.Name = "_lblParameterName_11"
        Me._lblParameterName_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterName_11.Size = New System.Drawing.Size(45, 13)
        Me._lblParameterName_11.TabIndex = 60
        Me._lblParameterName_11.Text = "Label12"
        '
        'lblGroupBy
        '
        Me.lblGroupBy.AutoSize = True
        Me.lblGroupBy.BackColor = System.Drawing.SystemColors.Control
        Me.lblGroupBy.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGroupBy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGroupBy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGroupBy.Location = New System.Drawing.Point(544, 40)
        Me.lblGroupBy.Name = "lblGroupBy"
        Me.lblGroupBy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGroupBy.Size = New System.Drawing.Size(51, 13)
        Me.lblGroupBy.TabIndex = 59
        Me.lblGroupBy.Text = "Group By"
        Me.lblGroupBy.Visible = False
        '
        '_lblParameterName_10
        '
        Me._lblParameterName_10.AutoSize = True
        Me._lblParameterName_10.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterName_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterName_10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterName_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterName_10.Location = New System.Drawing.Point(24, 376)
        Me._lblParameterName_10.Name = "_lblParameterName_10"
        Me._lblParameterName_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterName_10.Size = New System.Drawing.Size(45, 13)
        Me._lblParameterName_10.TabIndex = 47
        Me._lblParameterName_10.Text = "Label11"
        '
        '_lblParameterType_10
        '
        Me._lblParameterType_10.AutoSize = True
        Me._lblParameterType_10.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterType_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterType_10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterType_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterType_10.Location = New System.Drawing.Point(288, 376)
        Me._lblParameterType_10.Name = "_lblParameterType_10"
        Me._lblParameterType_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterType_10.Size = New System.Drawing.Size(39, 13)
        Me._lblParameterType_10.TabIndex = 46
        Me._lblParameterType_10.Text = "Label1"
        '
        '_lblParameterName_9
        '
        Me._lblParameterName_9.AutoSize = True
        Me._lblParameterName_9.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterName_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterName_9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterName_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterName_9.Location = New System.Drawing.Point(24, 344)
        Me._lblParameterName_9.Name = "_lblParameterName_9"
        Me._lblParameterName_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterName_9.Size = New System.Drawing.Size(45, 13)
        Me._lblParameterName_9.TabIndex = 43
        Me._lblParameterName_9.Text = "Label10"
        '
        '_lblParameterType_9
        '
        Me._lblParameterType_9.AutoSize = True
        Me._lblParameterType_9.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterType_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterType_9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterType_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterType_9.Location = New System.Drawing.Point(288, 344)
        Me._lblParameterType_9.Name = "_lblParameterType_9"
        Me._lblParameterType_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterType_9.Size = New System.Drawing.Size(39, 13)
        Me._lblParameterType_9.TabIndex = 42
        Me._lblParameterType_9.Text = "Label1"
        '
        '_lblParameterName_8
        '
        Me._lblParameterName_8.AutoSize = True
        Me._lblParameterName_8.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterName_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterName_8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterName_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterName_8.Location = New System.Drawing.Point(24, 312)
        Me._lblParameterName_8.Name = "_lblParameterName_8"
        Me._lblParameterName_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterName_8.Size = New System.Drawing.Size(39, 13)
        Me._lblParameterName_8.TabIndex = 39
        Me._lblParameterName_8.Text = "Label9"
        '
        '_lblParameterType_8
        '
        Me._lblParameterType_8.AutoSize = True
        Me._lblParameterType_8.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterType_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterType_8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterType_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterType_8.Location = New System.Drawing.Point(288, 312)
        Me._lblParameterType_8.Name = "_lblParameterType_8"
        Me._lblParameterType_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterType_8.Size = New System.Drawing.Size(39, 13)
        Me._lblParameterType_8.TabIndex = 38
        Me._lblParameterType_8.Text = "Label1"
        '
        '_lblParameterName_7
        '
        Me._lblParameterName_7.AutoSize = True
        Me._lblParameterName_7.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterName_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterName_7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterName_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterName_7.Location = New System.Drawing.Point(24, 280)
        Me._lblParameterName_7.Name = "_lblParameterName_7"
        Me._lblParameterName_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterName_7.Size = New System.Drawing.Size(39, 13)
        Me._lblParameterName_7.TabIndex = 35
        Me._lblParameterName_7.Text = "Label8"
        '
        '_lblParameterType_7
        '
        Me._lblParameterType_7.AutoSize = True
        Me._lblParameterType_7.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterType_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterType_7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterType_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterType_7.Location = New System.Drawing.Point(288, 280)
        Me._lblParameterType_7.Name = "_lblParameterType_7"
        Me._lblParameterType_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterType_7.Size = New System.Drawing.Size(39, 13)
        Me._lblParameterType_7.TabIndex = 34
        Me._lblParameterType_7.Text = "Label1"
        '
        '_lblParameterName_0
        '
        Me._lblParameterName_0.AutoSize = True
        Me._lblParameterName_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterName_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterName_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterName_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterName_0.Location = New System.Drawing.Point(24, 56)
        Me._lblParameterName_0.Name = "_lblParameterName_0"
        Me._lblParameterName_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterName_0.Size = New System.Drawing.Size(39, 13)
        Me._lblParameterName_0.TabIndex = 31
        Me._lblParameterName_0.Text = "Label1"
        '
        '_lblParameterName_1
        '
        Me._lblParameterName_1.AutoSize = True
        Me._lblParameterName_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterName_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterName_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterName_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterName_1.Location = New System.Drawing.Point(24, 88)
        Me._lblParameterName_1.Name = "_lblParameterName_1"
        Me._lblParameterName_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterName_1.Size = New System.Drawing.Size(39, 13)
        Me._lblParameterName_1.TabIndex = 30
        Me._lblParameterName_1.Text = "Label2"
        '
        '_lblParameterName_2
        '
        Me._lblParameterName_2.AutoSize = True
        Me._lblParameterName_2.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterName_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterName_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterName_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterName_2.Location = New System.Drawing.Point(24, 120)
        Me._lblParameterName_2.Name = "_lblParameterName_2"
        Me._lblParameterName_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterName_2.Size = New System.Drawing.Size(39, 13)
        Me._lblParameterName_2.TabIndex = 29
        Me._lblParameterName_2.Text = "Label3"
        '
        '_lblParameterName_3
        '
        Me._lblParameterName_3.AutoSize = True
        Me._lblParameterName_3.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterName_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterName_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterName_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterName_3.Location = New System.Drawing.Point(24, 152)
        Me._lblParameterName_3.Name = "_lblParameterName_3"
        Me._lblParameterName_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterName_3.Size = New System.Drawing.Size(39, 13)
        Me._lblParameterName_3.TabIndex = 28
        Me._lblParameterName_3.Text = "Label4"
        '
        '_lblParameterName_4
        '
        Me._lblParameterName_4.AutoSize = True
        Me._lblParameterName_4.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterName_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterName_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterName_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterName_4.Location = New System.Drawing.Point(24, 184)
        Me._lblParameterName_4.Name = "_lblParameterName_4"
        Me._lblParameterName_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterName_4.Size = New System.Drawing.Size(39, 13)
        Me._lblParameterName_4.TabIndex = 27
        Me._lblParameterName_4.Text = "Label5"
        '
        '_lblParameterType_0
        '
        Me._lblParameterType_0.AutoSize = True
        Me._lblParameterType_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterType_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterType_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterType_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterType_0.Location = New System.Drawing.Point(288, 56)
        Me._lblParameterType_0.Name = "_lblParameterType_0"
        Me._lblParameterType_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterType_0.Size = New System.Drawing.Size(39, 13)
        Me._lblParameterType_0.TabIndex = 26
        Me._lblParameterType_0.Text = "Label1"
        '
        '_lblParameterType_1
        '
        Me._lblParameterType_1.AutoSize = True
        Me._lblParameterType_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterType_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterType_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterType_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterType_1.Location = New System.Drawing.Point(288, 88)
        Me._lblParameterType_1.Name = "_lblParameterType_1"
        Me._lblParameterType_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterType_1.Size = New System.Drawing.Size(39, 13)
        Me._lblParameterType_1.TabIndex = 25
        Me._lblParameterType_1.Text = "Label1"
        '
        '_lblParameterType_2
        '
        Me._lblParameterType_2.AutoSize = True
        Me._lblParameterType_2.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterType_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterType_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterType_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterType_2.Location = New System.Drawing.Point(288, 120)
        Me._lblParameterType_2.Name = "_lblParameterType_2"
        Me._lblParameterType_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterType_2.Size = New System.Drawing.Size(39, 13)
        Me._lblParameterType_2.TabIndex = 24
        Me._lblParameterType_2.Text = "Label1"
        '
        '_lblParameterType_3
        '
        Me._lblParameterType_3.AutoSize = True
        Me._lblParameterType_3.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterType_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterType_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterType_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterType_3.Location = New System.Drawing.Point(288, 152)
        Me._lblParameterType_3.Name = "_lblParameterType_3"
        Me._lblParameterType_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterType_3.Size = New System.Drawing.Size(39, 13)
        Me._lblParameterType_3.TabIndex = 23
        Me._lblParameterType_3.Text = "Label1"
        '
        '_lblParameterType_4
        '
        Me._lblParameterType_4.AutoSize = True
        Me._lblParameterType_4.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterType_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterType_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterType_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterType_4.Location = New System.Drawing.Point(288, 184)
        Me._lblParameterType_4.Name = "_lblParameterType_4"
        Me._lblParameterType_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterType_4.Size = New System.Drawing.Size(39, 13)
        Me._lblParameterType_4.TabIndex = 22
        Me._lblParameterType_4.Text = "Label1"
        '
        '_lblParameterType_5
        '
        Me._lblParameterType_5.AutoSize = True
        Me._lblParameterType_5.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterType_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterType_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterType_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterType_5.Location = New System.Drawing.Point(288, 216)
        Me._lblParameterType_5.Name = "_lblParameterType_5"
        Me._lblParameterType_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterType_5.Size = New System.Drawing.Size(39, 13)
        Me._lblParameterType_5.TabIndex = 21
        Me._lblParameterType_5.Text = "Label1"
        '
        '_lblParameterName_5
        '
        Me._lblParameterName_5.AutoSize = True
        Me._lblParameterName_5.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterName_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterName_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterName_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterName_5.Location = New System.Drawing.Point(24, 216)
        Me._lblParameterName_5.Name = "_lblParameterName_5"
        Me._lblParameterName_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterName_5.Size = New System.Drawing.Size(39, 13)
        Me._lblParameterName_5.TabIndex = 20
        Me._lblParameterName_5.Text = "Label6"
        '
        '_lblParameterName_6
        '
        Me._lblParameterName_6.AutoSize = True
        Me._lblParameterName_6.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterName_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterName_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterName_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterName_6.Location = New System.Drawing.Point(24, 248)
        Me._lblParameterName_6.Name = "_lblParameterName_6"
        Me._lblParameterName_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterName_6.Size = New System.Drawing.Size(39, 13)
        Me._lblParameterName_6.TabIndex = 19
        Me._lblParameterName_6.Text = "Label7"
        '
        '_lblParameterType_6
        '
        Me._lblParameterType_6.AutoSize = True
        Me._lblParameterType_6.BackColor = System.Drawing.SystemColors.Control
        Me._lblParameterType_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblParameterType_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblParameterType_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblParameterType_6.Location = New System.Drawing.Point(288, 248)
        Me._lblParameterType_6.Name = "_lblParameterType_6"
        Me._lblParameterType_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblParameterType_6.Size = New System.Drawing.Size(39, 13)
        Me._lblParameterType_6.TabIndex = 18
        Me._lblParameterType_6.Text = "Label1"
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me._fraType_0)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(663, 539)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "Tab 2"
        Me._tabMainTab_TabPage1.UseVisualStyleBackColor = True
        '
        '_fraType_0
        '
        Me._fraType_0.BackColor = System.Drawing.SystemColors.Control
        Me._fraType_0.Controls.Add(Me._cmdDeleteExcluded_0)
        Me._fraType_0.Controls.Add(Me._cmdAddIncluded_0)
        Me._fraType_0.Controls.Add(Me._cmdAddAllIncluded_0)
        Me._fraType_0.Controls.Add(Me._cmdDeleteAllExcluded_0)
        Me._fraType_0.Controls.Add(Me._lvwIncluded_0)
        Me._fraType_0.Controls.Add(Me._lvwExcluded_0)
        Me._fraType_0.Controls.Add(Me._lblIncluded_0)
        Me._fraType_0.Controls.Add(Me._lblExcluded_0)
        Me._fraType_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraType_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraType_0.Location = New System.Drawing.Point(8, 12)
        Me._fraType_0.Name = "_fraType_0"
        Me._fraType_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraType_0.Size = New System.Drawing.Size(593, 481)
        Me._fraType_0.TabIndex = 65
        Me._fraType_0.TabStop = False
        Me._fraType_0.Text = "Type"
        '
        '_lvwIncluded_0
        '
        Me._lvwIncluded_0.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me._lvwIncluded_0, "")
        Me._lvwIncluded_0.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwIncluded_0_ColumnHeader_1})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me._lvwIncluded_0, False)
        Me._lvwIncluded_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lvwIncluded_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me._lvwIncluded_0, "")
        Me.listViewHelper1.SetLargeIcons(Me._lvwIncluded_0, "")
        Me._lvwIncluded_0.Location = New System.Drawing.Point(8, 40)
        Me._lvwIncluded_0.Name = "_lvwIncluded_0"
        Me._lvwIncluded_0.Size = New System.Drawing.Size(233, 407)
        Me.listViewHelper1.SetSmallIcons(Me._lvwIncluded_0, "")
        Me.listViewHelper1.SetSorted(Me._lvwIncluded_0, True)
        Me._lvwIncluded_0.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.listViewHelper1.SetSortKey(Me._lvwIncluded_0, 0)
        Me.listViewHelper1.SetSortOrder(Me._lvwIncluded_0, System.Windows.Forms.SortOrder.Ascending)
        Me._lvwIncluded_0.TabIndex = 72
        Me._lvwIncluded_0.UseCompatibleStateImageBehavior = False
        Me._lvwIncluded_0.View = System.Windows.Forms.View.Details
        '
        '_lvwIncluded_0_ColumnHeader_1
        '
        Me._lvwIncluded_0_ColumnHeader_1.Width = 234
        '
        '_lvwExcluded_0
        '
        Me._lvwExcluded_0.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me._lvwExcluded_0, "")
        Me._lvwExcluded_0.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwExcluded_0_ColumnHeader_1})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me._lvwExcluded_0, False)
        Me._lvwExcluded_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lvwExcluded_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me._lvwExcluded_0, "")
        Me.listViewHelper1.SetLargeIcons(Me._lvwExcluded_0, "")
        Me._lvwExcluded_0.Location = New System.Drawing.Point(336, 40)
        Me._lvwExcluded_0.Name = "_lvwExcluded_0"
        Me._lvwExcluded_0.Size = New System.Drawing.Size(241, 407)
        Me.listViewHelper1.SetSmallIcons(Me._lvwExcluded_0, "")
        Me.listViewHelper1.SetSorted(Me._lvwExcluded_0, True)
        Me._lvwExcluded_0.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.listViewHelper1.SetSortKey(Me._lvwExcluded_0, 0)
        Me.listViewHelper1.SetSortOrder(Me._lvwExcluded_0, System.Windows.Forms.SortOrder.Ascending)
        Me._lvwExcluded_0.TabIndex = 73
        Me._lvwExcluded_0.UseCompatibleStateImageBehavior = False
        Me._lvwExcluded_0.View = System.Windows.Forms.View.Details
        '
        '_lvwExcluded_0_ColumnHeader_1
        '
        Me._lvwExcluded_0_ColumnHeader_1.Width = 234
        '
        '_lblIncluded_0
        '
        Me._lblIncluded_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblIncluded_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblIncluded_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblIncluded_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblIncluded_0.Location = New System.Drawing.Point(8, 24)
        Me._lblIncluded_0.Name = "_lblIncluded_0"
        Me._lblIncluded_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblIncluded_0.Size = New System.Drawing.Size(89, 17)
        Me._lblIncluded_0.TabIndex = 71
        Me._lblIncluded_0.Text = "Included:"
        '
        '_lblExcluded_0
        '
        Me._lblExcluded_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblExcluded_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblExcluded_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblExcluded_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblExcluded_0.Location = New System.Drawing.Point(336, 24)
        Me._lblExcluded_0.Name = "_lblExcluded_0"
        Me._lblExcluded_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblExcluded_0.Size = New System.Drawing.Size(89, 17)
        Me._lblExcluded_0.TabIndex = 70
        Me._lblExcluded_0.Text = "Excluded:"
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(600, 576)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 1
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
        Me.cmdOK.Location = New System.Drawing.Point(520, 576)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "Risk")
        Me.ImageList1.Images.SetKeyName(1, "")
        Me.ImageList1.Images.SetKeyName(2, "")
        Me.ImageList1.Images.SetKeyName(3, "")
        Me.ImageList1.Images.SetKeyName(4, "")
        '
        'cmdAddToScheduler
        '
        Me.cmdAddToScheduler.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddToScheduler.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddToScheduler.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddToScheduler.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddToScheduler.Location = New System.Drawing.Point(456, 576)
        Me.cmdAddToScheduler.Name = "cmdAddToScheduler"
        Me.cmdAddToScheduler.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddToScheduler.Size = New System.Drawing.Size(137, 22)
        Me.cmdAddToScheduler.TabIndex = 98
        Me.cmdAddToScheduler.Text = "&Add To Scheduler"
        Me.cmdAddToScheduler.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddToScheduler.UseVisualStyleBackColor = False
        Me.cmdAddToScheduler.Visible = False
        '
        'frmParameters
        '
        Me.AcceptButton = Me.cmdAddToScheduler
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(680, 604)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdAddToScheduler)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmParameters"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Sirius Reports"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.fraParameters.ResumeLayout(False)
        Me.fraParameters.PerformLayout()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me._fraType_0.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Sub InitializelvwIncluded()
        Me.lvwIncluded(0) = _lvwIncluded_0
    End Sub
    Sub InitializelvwExcluded()
        Me.lvwExcluded(0) = _lvwExcluded_0
    End Sub
    Sub InitializelblParameterType()
        Me.lblParameterType(13) = _lblParameterType_13
        Me.lblParameterType(12) = _lblParameterType_12
        Me.lblParameterType(11) = _lblParameterType_11
        Me.lblParameterType(10) = _lblParameterType_10
        Me.lblParameterType(9) = _lblParameterType_9
        Me.lblParameterType(8) = _lblParameterType_8
        Me.lblParameterType(7) = _lblParameterType_7
        Me.lblParameterType(0) = _lblParameterType_0
        Me.lblParameterType(1) = _lblParameterType_1
        Me.lblParameterType(2) = _lblParameterType_2
        Me.lblParameterType(3) = _lblParameterType_3
        Me.lblParameterType(4) = _lblParameterType_4
        Me.lblParameterType(5) = _lblParameterType_5
        Me.lblParameterType(6) = _lblParameterType_6
    End Sub
    Sub InitializelblParameterName()
        Me.lblParameterName(13) = _lblParameterName_13
        Me.lblParameterName(12) = _lblParameterName_12
        Me.lblParameterName(11) = _lblParameterName_11
        Me.lblParameterName(10) = _lblParameterName_10
        Me.lblParameterName(9) = _lblParameterName_9
        Me.lblParameterName(8) = _lblParameterName_8
        Me.lblParameterName(7) = _lblParameterName_7
        Me.lblParameterName(0) = _lblParameterName_0
        Me.lblParameterName(1) = _lblParameterName_1
        Me.lblParameterName(2) = _lblParameterName_2
        Me.lblParameterName(3) = _lblParameterName_3
        Me.lblParameterName(4) = _lblParameterName_4
        Me.lblParameterName(5) = _lblParameterName_5
        Me.lblParameterName(6) = _lblParameterName_6
    End Sub
    Sub InitializelblIncluded()
        Me.lblIncluded(0) = _lblIncluded_0
    End Sub
    Sub InitializelblExcluded()
        Me.lblExcluded(0) = _lblExcluded_0
    End Sub
    Sub InitializefraType()
        Me.fraType(0) = _fraType_0
    End Sub
    Sub InitializecmdParameterValues()
        Me.cmdParameterValues(13) = _cmdParameterValues_13
        Me.cmdParameterValues(12) = _cmdParameterValues_12
        Me.cmdParameterValues(11) = _cmdParameterValues_11
        Me.cmdParameterValues(10) = _cmdParameterValues_10
        Me.cmdParameterValues(9) = _cmdParameterValues_9
        Me.cmdParameterValues(8) = _cmdParameterValues_8
        Me.cmdParameterValues(7) = _cmdParameterValues_7
        Me.cmdParameterValues(0) = _cmdParameterValues_0
        Me.cmdParameterValues(1) = _cmdParameterValues_1
        Me.cmdParameterValues(6) = _cmdParameterValues_6
        Me.cmdParameterValues(5) = _cmdParameterValues_5
        Me.cmdParameterValues(4) = _cmdParameterValues_4
        Me.cmdParameterValues(3) = _cmdParameterValues_3
        Me.cmdParameterValues(2) = _cmdParameterValues_2
    End Sub
    Sub InitializecmdDeleteExcluded()
        Me.cmdDeleteExcluded(0) = _cmdDeleteExcluded_0
    End Sub
    Sub InitializecmdDeleteAllExcluded()
        Me.cmdDeleteAllExcluded(0) = _cmdDeleteAllExcluded_0
    End Sub
    Sub InitializecmdAddIncluded()
        Me.cmdAddIncluded(0) = _cmdAddIncluded_0
    End Sub
    Sub InitializecmdAddAllIncluded()
        Me.cmdAddAllIncluded(0) = _cmdAddAllIncluded_0
    End Sub
    Sub InitializechkGroupBy()
        Me.chkGroupBy(13) = _chkGroupBy_13
        Me.chkGroupBy(12) = _chkGroupBy_12
        Me.chkGroupBy(11) = _chkGroupBy_11
        Me.chkGroupBy(10) = _chkGroupBy_10
        Me.chkGroupBy(9) = _chkGroupBy_9
        Me.chkGroupBy(8) = _chkGroupBy_8
        Me.chkGroupBy(7) = _chkGroupBy_7
        Me.chkGroupBy(6) = _chkGroupBy_6
        Me.chkGroupBy(5) = _chkGroupBy_5
        Me.chkGroupBy(4) = _chkGroupBy_4
        Me.chkGroupBy(3) = _chkGroupBy_3
        Me.chkGroupBy(2) = _chkGroupBy_2
        Me.chkGroupBy(1) = _chkGroupBy_1
        Me.chkGroupBy(0) = _chkGroupBy_0
    End Sub
    Sub InitializecboParameterValues()
        Me.cboParameterValues(11) = _cboParameterValues_11
        Me.cboParameterValues(6) = _cboParameterValues_6
        Me.cboParameterValues(5) = _cboParameterValues_5
        Me.cboParameterValues(4) = _cboParameterValues_4
        Me.cboParameterValues(3) = _cboParameterValues_3
        Me.cboParameterValues(2) = _cboParameterValues_2
        Me.cboParameterValues(1) = _cboParameterValues_1
        Me.cboParameterValues(0) = _cboParameterValues_0
        Me.cboParameterValues(7) = _cboParameterValues_7
        Me.cboParameterValues(8) = _cboParameterValues_8
        Me.cboParameterValues(9) = _cboParameterValues_9
        Me.cboParameterValues(10) = _cboParameterValues_10
        Me.cboParameterValues(12) = _cboParameterValues_12
        Me.cboParameterValues(13) = _cboParameterValues_13
    End Sub
    Sub InitializeDTParameters()
        Me.DTParameters(13) = _DTParameters_13
        Me.DTParameters(12) = _DTParameters_12
        Me.DTParameters(11) = _DTParameters_11
        Me.DTParameters(10) = _DTParameters_10
        Me.DTParameters(9) = _DTParameters_9
        Me.DTParameters(8) = _DTParameters_8
        Me.DTParameters(7) = _DTParameters_7
        Me.DTParameters(6) = _DTParameters_6
        Me.DTParameters(5) = _DTParameters_5
        Me.DTParameters(4) = _DTParameters_4
        Me.DTParameters(3) = _DTParameters_3
        Me.DTParameters(2) = _DTParameters_2
        Me.DTParameters(1) = _DTParameters_1
        Me.DTParameters(0) = _DTParameters_0
    End Sub

    Sub InitializeListBox()
        Me.List1(13) = ListBox13
        Me.List1(12) = ListBox12
        Me.List1(11) = ListBox11
        Me.List1(10) = ListBox10
        Me.List1(9) = ListBox9
        Me.List1(8) = ListBox8
        Me.List1(7) = ListBox7
        Me.List1(6) = ListBox6
        Me.List1(5) = ListBox5
        Me.List1(4) = ListBox4
        Me.List1(3) = ListBox3
        Me.List1(2) = ListBox2
        Me.List1(1) = ListBox1
        Me.List1(0) = ListBox0
    End Sub

    Public WithEvents _DTParameters_2 As System.Windows.Forms.DateTimePicker
    Public WithEvents _DTParameters_1 As System.Windows.Forms.DateTimePicker
    Public WithEvents _DTParameters_0 As System.Windows.Forms.DateTimePicker
    Public WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Friend WithEvents ListBox0 As System.Windows.Forms.ListBox
    Friend WithEvents ListBox3 As System.Windows.Forms.ListBox
    Friend WithEvents ListBox2 As System.Windows.Forms.ListBox
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents ListBox13 As System.Windows.Forms.ListBox
    Friend WithEvents ListBox12 As System.Windows.Forms.ListBox
    Friend WithEvents ListBox11 As System.Windows.Forms.ListBox
    Friend WithEvents ListBox10 As System.Windows.Forms.ListBox
    Friend WithEvents ListBox9 As System.Windows.Forms.ListBox
    Friend WithEvents ListBox8 As System.Windows.Forms.ListBox
    Friend WithEvents ListBox7 As System.Windows.Forms.ListBox
    Friend WithEvents ListBox6 As System.Windows.Forms.ListBox
    Friend WithEvents ListBox5 As System.Windows.Forms.ListBox
    Friend WithEvents ListBox4 As System.Windows.Forms.ListBox
#End Region
End Class