<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        isInitializingComponent = True
        InitializeComponent()
        isInitializingComponent = False
        InitializeoptBasis()
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
    Public WithEvents lblRITax As System.Windows.Forms.Label
    Private WithEvents _lvwRITax_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRITax_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRITax_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRITax_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRITax_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRITax_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRITax_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRITax_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRITax_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwRITax As System.Windows.Forms.ListView
    Public WithEvents cmdEdit As System.Windows.Forms.Button
    Public WithEvents txtDescription As System.Windows.Forms.TextBox
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents lblValue As System.Windows.Forms.Label
    Public WithEvents lblCode As System.Windows.Forms.Label
    Public WithEvents lblSumInsured As System.Windows.Forms.Label
    Public WithEvents lblOriginalSumInsured As System.Windows.Forms.Label
    Public WithEvents lblSumInsuredChange As System.Windows.Forms.Label
    Public WithEvents lblPremium As System.Windows.Forms.Label
    Public WithEvents lblRunningTotal As System.Windows.Forms.Label
    Public WithEvents cmdDetailCancel As System.Windows.Forms.Button
    Public WithEvents cmdDetailOK As System.Windows.Forms.Button
    Public WithEvents txtTaxValue As System.Windows.Forms.TextBox
    Public WithEvents txtTaxBand As System.Windows.Forms.TextBox
    Public WithEvents chkAllowCredit As System.Windows.Forms.CheckBox
    Private WithEvents _optBasis_2 As System.Windows.Forms.RadioButton
    Private WithEvents _optBasis_3 As System.Windows.Forms.RadioButton
    Public WithEvents chkRounded As System.Windows.Forms.CheckBox
    Public WithEvents chkIsValue As System.Windows.Forms.CheckBox
    Public WithEvents txtPercentage As System.Windows.Forms.TextBox
    Private WithEvents _optBasis_1 As System.Windows.Forms.RadioButton
    Public WithEvents txtValue As System.Windows.Forms.TextBox
    Private WithEvents _optBasis_0 As System.Windows.Forms.RadioButton
    Public WithEvents txtBasisValue As System.Windows.Forms.TextBox
    Public WithEvents lblOfSI As System.Windows.Forms.Label
    Public WithEvents lblCalcBasis As System.Windows.Forms.Label
    Public WithEvents lblPercentage As System.Windows.Forms.Label
    Public WithEvents lblPer As System.Windows.Forms.Label
    Public WithEvents fraCalculation As System.Windows.Forms.GroupBox
    Public WithEvents txtCOB As System.Windows.Forms.TextBox
    Public WithEvents txtState As System.Windows.Forms.TextBox
    Public WithEvents txtCountry As System.Windows.Forms.TextBox
    Public WithEvents lblCOB As System.Windows.Forms.Label
    Public WithEvents lblState As System.Windows.Forms.Label
    Public WithEvents lblCountry As System.Windows.Forms.Label
    Public WithEvents fraFilters As System.Windows.Forms.GroupBox
    Public WithEvents txtSumInsured As System.Windows.Forms.TextBox
    Public WithEvents txtOriginalSumInsured As System.Windows.Forms.TextBox
    Public WithEvents txtSumInsuredChange As System.Windows.Forms.TextBox
    Public WithEvents txtPremium As System.Windows.Forms.TextBox
    Public WithEvents txtRunningTotal As System.Windows.Forms.TextBox
    Private WithEvents _tabDetailTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents tabDetailTab As System.Windows.Forms.TabControl
    Public optBasis(3) As System.Windows.Forms.RadioButton
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblRITax = New System.Windows.Forms.Label
        Me.lvwRITax = New System.Windows.Forms.ListView
        Me._lvwRITax_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwRITax_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwRITax_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwRITax_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwRITax_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwRITax_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwRITax_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._lvwRITax_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
        Me._lvwRITax_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabDetailTab = New System.Windows.Forms.TabControl
        Me._tabDetailTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblValue = New System.Windows.Forms.Label
        Me.lblCode = New System.Windows.Forms.Label
        Me.lblSumInsured = New System.Windows.Forms.Label
        Me.lblOriginalSumInsured = New System.Windows.Forms.Label
        Me.lblSumInsuredChange = New System.Windows.Forms.Label
        Me.lblPremium = New System.Windows.Forms.Label
        Me.lblRunningTotal = New System.Windows.Forms.Label
        Me.cmdDetailCancel = New System.Windows.Forms.Button
        Me.cmdDetailOK = New System.Windows.Forms.Button
        Me.txtTaxValue = New System.Windows.Forms.TextBox
        Me.txtTaxBand = New System.Windows.Forms.TextBox
        Me.fraCalculation = New System.Windows.Forms.GroupBox
        Me.chkAllowCredit = New System.Windows.Forms.CheckBox
        Me._optBasis_2 = New System.Windows.Forms.RadioButton
        Me._optBasis_3 = New System.Windows.Forms.RadioButton
        Me.chkRounded = New System.Windows.Forms.CheckBox
        Me.chkIsValue = New System.Windows.Forms.CheckBox
        Me.txtPercentage = New System.Windows.Forms.TextBox
        Me._optBasis_1 = New System.Windows.Forms.RadioButton
        Me.txtValue = New System.Windows.Forms.TextBox
        Me._optBasis_0 = New System.Windows.Forms.RadioButton
        Me.txtBasisValue = New System.Windows.Forms.TextBox
        Me.lblOfSI = New System.Windows.Forms.Label
        Me.lblCalcBasis = New System.Windows.Forms.Label
        Me.lblPercentage = New System.Windows.Forms.Label
        Me.lblPer = New System.Windows.Forms.Label
        Me.fraFilters = New System.Windows.Forms.GroupBox
        Me.txtCOB = New System.Windows.Forms.TextBox
        Me.txtState = New System.Windows.Forms.TextBox
        Me.txtCountry = New System.Windows.Forms.TextBox
        Me.lblCOB = New System.Windows.Forms.Label
        Me.lblState = New System.Windows.Forms.Label
        Me.lblCountry = New System.Windows.Forms.Label
        Me.txtSumInsured = New System.Windows.Forms.TextBox
        Me.txtOriginalSumInsured = New System.Windows.Forms.TextBox
        Me.txtSumInsuredChange = New System.Windows.Forms.TextBox
        Me.txtPremium = New System.Windows.Forms.TextBox
        Me.txtRunningTotal = New System.Windows.Forms.TextBox
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.lvwRITax.SuspendLayout()
        Me.tabDetailTab.SuspendLayout()
        Me._tabDetailTab_TabPage0.SuspendLayout()
        Me.fraCalculation.SuspendLayout()
        Me.fraFilters.SuspendLayout()
        Me.SuspendLayout()
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        ' 
        ' tabMainTab
        ' 
        Me.tabMainTab.Alignment = System.Windows.Forms.TabAlignment.Top
        Me.tabMainTab.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.tabMainTab.ItemSize = New System.Drawing.Size(633, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(4, 4)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.Size = New System.Drawing.Size(638, 451)
        Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.tabMainTab.TabIndex = 9
        Me.tabMainTab.TabStop = False
        ' 
        ' _tabMainTab_TabPage0
        ' 
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblRITax)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwRITax)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdEdit)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDescription)
        Me._tabMainTab_TabPage0.Text = "Risk Tax"
        ' 
        ' lblRITax
        ' 
        Me.lblRITax.AutoSize = True
        Me.lblRITax.BackColor = System.Drawing.SystemColors.Control
        Me.lblRITax.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblRITax.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRITax.Enabled = True
        Me.lblRITax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.lblRITax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRITax.Location = New System.Drawing.Point(21, 17)
        Me.lblRITax.Name = "lblRITax"
        Me.lblRITax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRITax.Size = New System.Drawing.Size(114, 13)
        Me.lblRITax.TabIndex = 10
        Me.lblRITax.Text = "Risk/Insurance File:"
        Me.lblRITax.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblRITax.UseMnemonic = True
        Me.lblRITax.Visible = True
        ' 
        ' lvwRITax
        ' 
        Me.lvwRITax.BackColor = System.Drawing.SystemColors.Window
        Me.lvwRITax.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwRITax.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.lvwRITax.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwRITax.FullRowSelect = True
        Me.lvwRITax.HideSelection = True
        Me.lvwRITax.LabelEdit = False
        Me.lvwRITax.LabelWrap = True
        Me.lvwRITax.Location = New System.Drawing.Point(8, 46)
        Me.lvwRITax.Name = "lvwRITax"
        Me.lvwRITax.Size = New System.Drawing.Size(615, 338)
        Me.lvwRITax.TabIndex = 7
        Me.lvwRITax.View = System.Windows.Forms.View.Details
        Me.lvwRITax.Columns.Add(Me._lvwRITax_ColumnHeader_1)
        Me.lvwRITax.Columns.Add(Me._lvwRITax_ColumnHeader_2)
        Me.lvwRITax.Columns.Add(Me._lvwRITax_ColumnHeader_3)
        Me.lvwRITax.Columns.Add(Me._lvwRITax_ColumnHeader_4)
        Me.lvwRITax.Columns.Add(Me._lvwRITax_ColumnHeader_5)
        Me.lvwRITax.Columns.Add(Me._lvwRITax_ColumnHeader_6)
        Me.lvwRITax.Columns.Add(Me._lvwRITax_ColumnHeader_7)
        Me.lvwRITax.Columns.Add(Me._lvwRITax_ColumnHeader_8)
        Me.lvwRITax.Columns.Add(Me._lvwRITax_ColumnHeader_9)
        ' 
        ' _lvwRITax_ColumnHeader_1
        ' 
        Me._lvwRITax_ColumnHeader_1.Text = "Tax Group"
        Me._lvwRITax_ColumnHeader_1.Width = 201
        ' 
        ' _lvwRITax_ColumnHeader_2
        ' 
        Me._lvwRITax_ColumnHeader_2.Text = "Sequence"
        Me._lvwRITax_ColumnHeader_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwRITax_ColumnHeader_2.Width = 97
        ' 
        ' _lvwRITax_ColumnHeader_3
        ' 
        Me._lvwRITax_ColumnHeader_3.Text = "Tax Band"
        Me._lvwRITax_ColumnHeader_3.Width = 201
        ' 
        ' _lvwRITax_ColumnHeader_4
        ' 
        Me._lvwRITax_ColumnHeader_4.Text = "Tax Amount"
        Me._lvwRITax_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwRITax_ColumnHeader_4.Width = 97
        ' 
        ' _lvwRITax_ColumnHeader_5
        ' 
        Me._lvwRITax_ColumnHeader_5.Text = "Calculation Basis"
        Me._lvwRITax_ColumnHeader_5.Width = 201
        ' 
        ' _lvwRITax_ColumnHeader_6
        ' 
        Me._lvwRITax_ColumnHeader_6.Text = "Rate"
        Me._lvwRITax_ColumnHeader_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwRITax_ColumnHeader_6.Width = 97
        ' 
        ' _lvwRITax_ColumnHeader_7
        ' 
        Me._lvwRITax_ColumnHeader_7.Text = "Class of Business"
        Me._lvwRITax_ColumnHeader_7.Width = 97
        ' 
        ' _lvwRITax_ColumnHeader_8
        ' 
        Me._lvwRITax_ColumnHeader_8.Text = "Country"
        Me._lvwRITax_ColumnHeader_8.Width = 97
        ' 
        ' _lvwRITax_ColumnHeader_9
        ' 
        Me._lvwRITax_ColumnHeader_9.Text = "State"
        Me._lvwRITax_ColumnHeader_9.Width = 97
        ' 
        ' cmdEdit
        ' 
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.CausesValidation = True
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Enabled = True
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(550, 394)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 3
        Me.cmdEdit.TabStop = True
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        ' 
        ' txtDescription
        ' 
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.AutoSize = False
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Control
        Me.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtDescription.CausesValidation = True
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Enabled = True
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.HideSelection = True
        Me.txtDescription.Location = New System.Drawing.Point(144, 15)
        Me.txtDescription.MaxLength = 0
        Me.txtDescription.Multiline = False
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.ReadOnly = True
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.txtDescription.Size = New System.Drawing.Size(460, 21)
        Me.txtDescription.TabIndex = 14
        Me.txtDescription.TabStop = True
        Me.txtDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtDescription.Visible = True
        ' 
        ' cmdHelp
        ' 
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.CausesValidation = True
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Enabled = True
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(567, 461)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(71, 22)
        Me.cmdHelp.TabIndex = 2
        Me.cmdHelp.TabStop = True
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        ' 
        ' cmdCancel
        ' 
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.CausesValidation = True
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Enabled = True
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(487, 461)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.TabStop = True
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        ' 
        ' cmdOK
        ' 
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.CausesValidation = True
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Enabled = True
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(407, 461)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.TabStop = True
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        ' 
        ' tabDetailTab
        ' 
        Me.tabDetailTab.Alignment = System.Windows.Forms.TabAlignment.Top
        Me.tabDetailTab.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tabDetailTab.Controls.Add(Me._tabDetailTab_TabPage0)
        Me.tabDetailTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.tabDetailTab.ItemSize = New System.Drawing.Size(633, 18)
        Me.tabDetailTab.Location = New System.Drawing.Point(4, 4)
        Me.tabDetailTab.Multiline = True
        Me.tabDetailTab.Name = "tabDetailTab"
        Me.tabDetailTab.Size = New System.Drawing.Size(638, 451)
        Me.tabDetailTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.tabDetailTab.TabIndex = 8
        Me.tabDetailTab.TabStop = False
        ' 
        ' _tabDetailTab_TabPage0
        ' 
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblValue)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblCode)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblSumInsured)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblOriginalSumInsured)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblSumInsuredChange)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblPremium)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblRunningTotal)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdDetailCancel)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdDetailOK)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtTaxValue)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtTaxBand)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.fraCalculation)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.fraFilters)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtSumInsured)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtOriginalSumInsured)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtSumInsuredChange)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtPremium)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtRunningTotal)
        Me._tabDetailTab_TabPage0.Text = "Details"
        ' 
        ' lblValue
        ' 
        Me.lblValue.AutoSize = True
        Me.lblValue.BackColor = System.Drawing.SystemColors.Control
        Me.lblValue.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblValue.Enabled = True
        Me.lblValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.lblValue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblValue.Location = New System.Drawing.Point(377, 365)
        Me.lblValue.Name = "lblValue"
        Me.lblValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblValue.Size = New System.Drawing.Size(74, 13)
        Me.lblValue.TabIndex = 11
        Me.lblValue.Text = "Tax Amount:"
        Me.lblValue.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblValue.UseMnemonic = True
        Me.lblValue.Visible = True
        ' 
        ' lblCode
        ' 
        Me.lblCode.AutoSize = True
        Me.lblCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblCode.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCode.Enabled = True
        Me.lblCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.lblCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCode.Location = New System.Drawing.Point(76, 18)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCode.Size = New System.Drawing.Size(59, 13)
        Me.lblCode.TabIndex = 12
        Me.lblCode.Text = "Tax Band:"
        Me.lblCode.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblCode.UseMnemonic = True
        Me.lblCode.Visible = True
        ' 
        ' lblSumInsured
        ' 
        Me.lblSumInsured.AutoSize = True
        Me.lblSumInsured.BackColor = System.Drawing.SystemColors.Control
        Me.lblSumInsured.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblSumInsured.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSumInsured.Enabled = True
        Me.lblSumInsured.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.lblSumInsured.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSumInsured.Location = New System.Drawing.Point(56, 50)
        Me.lblSumInsured.Name = "lblSumInsured"
        Me.lblSumInsured.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSumInsured.Size = New System.Drawing.Size(79, 13)
        Me.lblSumInsured.TabIndex = 38
        Me.lblSumInsured.Text = "Sum Insured:"
        Me.lblSumInsured.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblSumInsured.UseMnemonic = True
        Me.lblSumInsured.Visible = True
        ' 
        ' lblOriginalSumInsured
        ' 
        Me.lblOriginalSumInsured.AutoSize = True
        Me.lblOriginalSumInsured.BackColor = System.Drawing.SystemColors.Control
        Me.lblOriginalSumInsured.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblOriginalSumInsured.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOriginalSumInsured.Enabled = True
        Me.lblOriginalSumInsured.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.lblOriginalSumInsured.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOriginalSumInsured.Location = New System.Drawing.Point(8, 77)
        Me.lblOriginalSumInsured.Name = "lblOriginalSumInsured"
        Me.lblOriginalSumInsured.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOriginalSumInsured.Size = New System.Drawing.Size(127, 13)
        Me.lblOriginalSumInsured.TabIndex = 40
        Me.lblOriginalSumInsured.Text = "Original Sum Insured:"
        Me.lblOriginalSumInsured.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblOriginalSumInsured.UseMnemonic = True
        Me.lblOriginalSumInsured.Visible = True
        ' 
        ' lblSumInsuredChange
        ' 
        Me.lblSumInsuredChange.AutoSize = True
        Me.lblSumInsuredChange.BackColor = System.Drawing.SystemColors.Control
        Me.lblSumInsuredChange.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblSumInsuredChange.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSumInsuredChange.Enabled = True
        Me.lblSumInsuredChange.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.lblSumInsuredChange.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSumInsuredChange.Location = New System.Drawing.Point(8, 104)
        Me.lblSumInsuredChange.Name = "lblSumInsuredChange"
        Me.lblSumInsuredChange.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSumInsuredChange.Size = New System.Drawing.Size(127, 13)
        Me.lblSumInsuredChange.TabIndex = 42
        Me.lblSumInsuredChange.Text = "Sum Insured Change:"
        Me.lblSumInsuredChange.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblSumInsuredChange.UseMnemonic = True
        Me.lblSumInsuredChange.Visible = True
        ' 
        ' lblPremium
        ' 
        Me.lblPremium.AutoSize = True
        Me.lblPremium.BackColor = System.Drawing.SystemColors.Control
        Me.lblPremium.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblPremium.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPremium.Enabled = True
        Me.lblPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.lblPremium.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPremium.Location = New System.Drawing.Point(379, 50)
        Me.lblPremium.Name = "lblPremium"
        Me.lblPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPremium.Size = New System.Drawing.Size(56, 13)
        Me.lblPremium.TabIndex = 44
        Me.lblPremium.Text = "Premium:"
        Me.lblPremium.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblPremium.UseMnemonic = True
        Me.lblPremium.Visible = True
        ' 
        ' lblRunningTotal
        ' 
        Me.lblRunningTotal.AutoSize = True
        Me.lblRunningTotal.BackColor = System.Drawing.SystemColors.Control
        Me.lblRunningTotal.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblRunningTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRunningTotal.Enabled = True
        Me.lblRunningTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.lblRunningTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRunningTotal.Location = New System.Drawing.Point(352, 77)
        Me.lblRunningTotal.Name = "lblRunningTotal"
        Me.lblRunningTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRunningTotal.Size = New System.Drawing.Size(83, 13)
        Me.lblRunningTotal.TabIndex = 46
        Me.lblRunningTotal.Text = "Running Total:"
        Me.lblRunningTotal.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblRunningTotal.UseMnemonic = True
        Me.lblRunningTotal.Visible = True
        ' 
        ' cmdDetailCancel
        ' 
        Me.cmdDetailCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDetailCancel.CausesValidation = True
        Me.cmdDetailCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDetailCancel.Enabled = True
        Me.cmdDetailCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.cmdDetailCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDetailCancel.Location = New System.Drawing.Point(550, 394)
        Me.cmdDetailCancel.Name = "cmdDetailCancel"
        Me.cmdDetailCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDetailCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdDetailCancel.TabIndex = 6
        Me.cmdDetailCancel.TabStop = True
        Me.cmdDetailCancel.Text = "&Cancel"
        Me.cmdDetailCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.cmdDetailCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        ' 
        ' cmdDetailOK
        ' 
        Me.cmdDetailOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDetailOK.CausesValidation = True
        Me.cmdDetailOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDetailOK.Enabled = True
        Me.cmdDetailOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.cmdDetailOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDetailOK.Location = New System.Drawing.Point(470, 394)
        Me.cmdDetailOK.Name = "cmdDetailOK"
        Me.cmdDetailOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDetailOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdDetailOK.TabIndex = 5
        Me.cmdDetailOK.TabStop = True
        Me.cmdDetailOK.Text = "&OK"
        Me.cmdDetailOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.cmdDetailOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        ' 
        ' txtTaxValue
        ' 
        Me.txtTaxValue.AcceptsReturn = True
        Me.txtTaxValue.AutoSize = False
        Me.txtTaxValue.BackColor = System.Drawing.SystemColors.Control
        Me.txtTaxValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtTaxValue.CausesValidation = True
        Me.txtTaxValue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTaxValue.Enabled = True
        Me.txtTaxValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.txtTaxValue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTaxValue.HideSelection = True
        Me.txtTaxValue.Location = New System.Drawing.Point(462, 362)
        Me.txtTaxValue.MaxLength = 0
        Me.txtTaxValue.Multiline = False
        Me.txtTaxValue.Name = "txtTaxValue"
        Me.txtTaxValue.ReadOnly = True
        Me.txtTaxValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTaxValue.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.txtTaxValue.Size = New System.Drawing.Size(160, 21)
        Me.txtTaxValue.TabIndex = 4
        Me.txtTaxValue.TabStop = True
        Me.txtTaxValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtTaxValue.Visible = True
        ' 
        ' txtTaxBand
        ' 
        Me.txtTaxBand.AcceptsReturn = True
        Me.txtTaxBand.AutoSize = False
        Me.txtTaxBand.BackColor = System.Drawing.SystemColors.Control
        Me.txtTaxBand.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtTaxBand.CausesValidation = True
        Me.txtTaxBand.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTaxBand.Enabled = True
        Me.txtTaxBand.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.txtTaxBand.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTaxBand.HideSelection = True
        Me.txtTaxBand.Location = New System.Drawing.Point(144, 15)
        Me.txtTaxBand.MaxLength = 0
        Me.txtTaxBand.Multiline = False
        Me.txtTaxBand.Name = "txtTaxBand"
        Me.txtTaxBand.ReadOnly = True
        Me.txtTaxBand.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTaxBand.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.txtTaxBand.Size = New System.Drawing.Size(460, 21)
        Me.txtTaxBand.TabIndex = 13
        Me.txtTaxBand.TabStop = True
        Me.txtTaxBand.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtTaxBand.Visible = True
        ' 
        ' fraCalculation
        ' 
        Me.fraCalculation.BackColor = System.Drawing.SystemColors.Control
        Me.fraCalculation.Controls.Add(Me.chkAllowCredit)
        Me.fraCalculation.Controls.Add(Me._optBasis_2)
        Me.fraCalculation.Controls.Add(Me._optBasis_3)
        Me.fraCalculation.Controls.Add(Me.chkRounded)
        Me.fraCalculation.Controls.Add(Me.chkIsValue)
        Me.fraCalculation.Controls.Add(Me.txtPercentage)
        Me.fraCalculation.Controls.Add(Me._optBasis_1)
        Me.fraCalculation.Controls.Add(Me.txtValue)
        Me.fraCalculation.Controls.Add(Me._optBasis_0)
        Me.fraCalculation.Controls.Add(Me.txtBasisValue)
        Me.fraCalculation.Controls.Add(Me.lblOfSI)
        Me.fraCalculation.Controls.Add(Me.lblCalcBasis)
        Me.fraCalculation.Controls.Add(Me.lblPercentage)
        Me.fraCalculation.Controls.Add(Me.lblPer)
        Me.fraCalculation.Enabled = True
        Me.fraCalculation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.fraCalculation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraCalculation.Location = New System.Drawing.Point(8, 132)
        Me.fraCalculation.Name = "fraCalculation"
        Me.fraCalculation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraCalculation.Size = New System.Drawing.Size(615, 133)
        Me.fraCalculation.TabIndex = 15
        Me.fraCalculation.Text = "Calculation"
        Me.fraCalculation.Visible = True
        ' 
        ' chkAllowCredit
        ' 
        Me.chkAllowCredit.Appearance = System.Windows.Forms.Appearance.Normal
        Me.chkAllowCredit.BackColor = System.Drawing.SystemColors.Control
        Me.chkAllowCredit.CausesValidation = True
        Me.chkAllowCredit.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAllowCredit.CheckState = System.Windows.Forms.CheckState.Unchecked
        Me.chkAllowCredit.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAllowCredit.Enabled = True
        Me.chkAllowCredit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.chkAllowCredit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAllowCredit.Location = New System.Drawing.Point(44, 102)
        Me.chkAllowCredit.Name = "chkAllowCredit"
        Me.chkAllowCredit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllowCredit.Size = New System.Drawing.Size(107, 15)
        Me.chkAllowCredit.TabIndex = 29
        Me.chkAllowCredit.TabStop = True
        Me.chkAllowCredit.Text = "Allow CR Tax?"
        Me.chkAllowCredit.Visible = True
        ' 
        ' _optBasis_2
        ' 
        Me._optBasis_2.Appearance = System.Windows.Forms.Appearance.Normal
        Me._optBasis_2.BackColor = System.Drawing.SystemColors.Control
        Me._optBasis_2.CausesValidation = True
        Me._optBasis_2.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me._optBasis_2.Checked = False
        Me._optBasis_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optBasis_2.Enabled = True
        Me._optBasis_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me._optBasis_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optBasis_2.Location = New System.Drawing.Point(340, 20)
        Me._optBasis_2.Name = "_optBasis_2"
        Me._optBasis_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optBasis_2.Size = New System.Drawing.Size(143, 15)
        Me._optBasis_2.TabIndex = 28
        Me._optBasis_2.TabStop = True
        Me._optBasis_2.Text = "Sum Insured Change"
        Me._optBasis_2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me._optBasis_2.Visible = True
        ' 
        ' _optBasis_3
        ' 
        Me._optBasis_3.Appearance = System.Windows.Forms.Appearance.Normal
        Me._optBasis_3.BackColor = System.Drawing.SystemColors.Control
        Me._optBasis_3.CausesValidation = True
        Me._optBasis_3.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me._optBasis_3.Checked = False
        Me._optBasis_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._optBasis_3.Enabled = True
        Me._optBasis_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me._optBasis_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optBasis_3.Location = New System.Drawing.Point(499, 20)
        Me._optBasis_3.Name = "_optBasis_3"
        Me._optBasis_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optBasis_3.Size = New System.Drawing.Size(99, 15)
        Me._optBasis_3.TabIndex = 27
        Me._optBasis_3.TabStop = True
        Me._optBasis_3.Text = "Running Total"
        Me._optBasis_3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me._optBasis_3.Visible = True
        ' 
        ' chkRounded
        ' 
        Me.chkRounded.Appearance = System.Windows.Forms.Appearance.Normal
        Me.chkRounded.BackColor = System.Drawing.SystemColors.Control
        Me.chkRounded.CausesValidation = True
        Me.chkRounded.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkRounded.CheckState = System.Windows.Forms.CheckState.Unchecked
        Me.chkRounded.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRounded.Enabled = True
        Me.chkRounded.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.chkRounded.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRounded.Location = New System.Drawing.Point(384, 102)
        Me.chkRounded.Name = "chkRounded"
        Me.chkRounded.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRounded.Size = New System.Drawing.Size(77, 15)
        Me.chkRounded.TabIndex = 26
        Me.chkRounded.TabStop = True
        Me.chkRounded.Text = "Rounded?"
        Me.chkRounded.Visible = True
        ' 
        ' chkIsValue
        ' 
        Me.chkIsValue.Appearance = System.Windows.Forms.Appearance.Normal
        Me.chkIsValue.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsValue.CausesValidation = True
        Me.chkIsValue.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsValue.CheckState = System.Windows.Forms.CheckState.Unchecked
        Me.chkIsValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsValue.Enabled = True
        Me.chkIsValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.chkIsValue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsValue.Location = New System.Drawing.Point(74, 44)
        Me.chkIsValue.Name = "chkIsValue"
        Me.chkIsValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsValue.Size = New System.Drawing.Size(77, 15)
        Me.chkIsValue.TabIndex = 25
        Me.chkIsValue.TabStop = True
        Me.chkIsValue.Text = "Is Value?"
        Me.chkIsValue.Visible = True
        ' 
        ' txtPercentage
        ' 
        Me.txtPercentage.AcceptsReturn = True
        Me.txtPercentage.AutoSize = False
        Me.txtPercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtPercentage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtPercentage.CausesValidation = True
        Me.txtPercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPercentage.Enabled = True
        Me.txtPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.txtPercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPercentage.HideSelection = True
        Me.txtPercentage.Location = New System.Drawing.Point(138, 70)
        Me.txtPercentage.MaxLength = 0
        Me.txtPercentage.Multiline = False
        Me.txtPercentage.Name = "txtPercentage"
        Me.txtPercentage.ReadOnly = False
        Me.txtPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPercentage.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.txtPercentage.Size = New System.Drawing.Size(140, 21)
        Me.txtPercentage.TabIndex = 23
        Me.txtPercentage.TabStop = True
        Me.txtPercentage.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtPercentage.Visible = True
        ' 
        ' _optBasis_1
        ' 
        Me._optBasis_1.Appearance = System.Windows.Forms.Appearance.Normal
        Me._optBasis_1.BackColor = System.Drawing.SystemColors.Control
        Me._optBasis_1.CausesValidation = True
        Me._optBasis_1.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me._optBasis_1.Checked = True
        Me._optBasis_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optBasis_1.Enabled = True
        Me._optBasis_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me._optBasis_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optBasis_1.Location = New System.Drawing.Point(227, 21)
        Me._optBasis_1.Name = "_optBasis_1"
        Me._optBasis_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optBasis_1.Size = New System.Drawing.Size(96, 14)
        Me._optBasis_1.TabIndex = 22
        Me._optBasis_1.TabStop = True
        Me._optBasis_1.Text = "Sum Insured"
        Me._optBasis_1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me._optBasis_1.Visible = True
        ' 
        ' txtValue
        ' 
        Me.txtValue.AcceptsReturn = True
        Me.txtValue.AutoSize = False
        Me.txtValue.BackColor = System.Drawing.SystemColors.Window
        Me.txtValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtValue.CausesValidation = True
        Me.txtValue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtValue.Enabled = True
        Me.txtValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.txtValue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtValue.HideSelection = True
        Me.txtValue.Location = New System.Drawing.Point(138, 70)
        Me.txtValue.MaxLength = 0
        Me.txtValue.Multiline = False
        Me.txtValue.Name = "txtValue"
        Me.txtValue.ReadOnly = False
        Me.txtValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtValue.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.txtValue.Size = New System.Drawing.Size(140, 21)
        Me.txtValue.TabIndex = 18
        Me.txtValue.TabStop = True
        Me.txtValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtValue.Visible = True
        ' 
        ' _optBasis_0
        ' 
        Me._optBasis_0.Appearance = System.Windows.Forms.Appearance.Normal
        Me._optBasis_0.BackColor = System.Drawing.SystemColors.Control
        Me._optBasis_0.CausesValidation = True
        Me._optBasis_0.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me._optBasis_0.Checked = False
        Me._optBasis_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optBasis_0.Enabled = True
        Me._optBasis_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me._optBasis_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optBasis_0.Location = New System.Drawing.Point(137, 21)
        Me._optBasis_0.Name = "_optBasis_0"
        Me._optBasis_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optBasis_0.Size = New System.Drawing.Size(74, 14)
        Me._optBasis_0.TabIndex = 17
        Me._optBasis_0.TabStop = True
        Me._optBasis_0.Text = "Premium"
        Me._optBasis_0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me._optBasis_0.Visible = True
        ' 
        ' txtBasisValue
        ' 
        Me.txtBasisValue.AcceptsReturn = True
        Me.txtBasisValue.AutoSize = False
        Me.txtBasisValue.BackColor = System.Drawing.SystemColors.Window
        Me.txtBasisValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtBasisValue.CausesValidation = True
        Me.txtBasisValue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBasisValue.Enabled = True
        Me.txtBasisValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.txtBasisValue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBasisValue.HideSelection = True
        Me.txtBasisValue.Location = New System.Drawing.Point(320, 70)
        Me.txtBasisValue.MaxLength = 0
        Me.txtBasisValue.Multiline = False
        Me.txtBasisValue.Name = "txtBasisValue"
        Me.txtBasisValue.ReadOnly = False
        Me.txtBasisValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBasisValue.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.txtBasisValue.Size = New System.Drawing.Size(140, 21)
        Me.txtBasisValue.TabIndex = 16
        Me.txtBasisValue.TabStop = True
        Me.txtBasisValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtBasisValue.Visible = True
        ' 
        ' lblOfSI
        ' 
        Me.lblOfSI.AutoSize = True
        Me.lblOfSI.BackColor = System.Drawing.SystemColors.Control
        Me.lblOfSI.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblOfSI.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOfSI.Enabled = True
        Me.lblOfSI.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.lblOfSI.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOfSI.Location = New System.Drawing.Point(470, 73)
        Me.lblOfSI.Name = "lblOfSI"
        Me.lblOfSI.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOfSI.Size = New System.Drawing.Size(89, 13)
        Me.lblOfSI.TabIndex = 24
        Me.lblOfSI.Text = "of Sum Insured"
        Me.lblOfSI.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lblOfSI.UseMnemonic = True
        Me.lblOfSI.Visible = True
        ' 
        ' lblCalcBasis
        ' 
        Me.lblCalcBasis.AutoSize = True
        Me.lblCalcBasis.BackColor = System.Drawing.SystemColors.Control
        Me.lblCalcBasis.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblCalcBasis.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCalcBasis.Enabled = True
        Me.lblCalcBasis.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.lblCalcBasis.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCalcBasis.Location = New System.Drawing.Point(26, 19)
        Me.lblCalcBasis.Name = "lblCalcBasis"
        Me.lblCalcBasis.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCalcBasis.Size = New System.Drawing.Size(102, 13)
        Me.lblCalcBasis.TabIndex = 21
        Me.lblCalcBasis.Text = "Calculation Basis:"
        Me.lblCalcBasis.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblCalcBasis.UseMnemonic = True
        Me.lblCalcBasis.Visible = True
        ' 
        ' lblPercentage
        ' 
        Me.lblPercentage.AutoSize = True
        Me.lblPercentage.BackColor = System.Drawing.SystemColors.Control
        Me.lblPercentage.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblPercentage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPercentage.Enabled = True
        Me.lblPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.lblPercentage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPercentage.Location = New System.Drawing.Point(97, 73)
        Me.lblPercentage.Name = "lblPercentage"
        Me.lblPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPercentage.Size = New System.Drawing.Size(31, 13)
        Me.lblPercentage.TabIndex = 20
        Me.lblPercentage.Text = "Rate:"
        Me.lblPercentage.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblPercentage.UseMnemonic = True
        Me.lblPercentage.Visible = True
        ' 
        ' lblPer
        ' 
        Me.lblPer.AutoSize = True
        Me.lblPer.BackColor = System.Drawing.SystemColors.Control
        Me.lblPer.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblPer.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPer.Enabled = True
        Me.lblPer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.lblPer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPer.Location = New System.Drawing.Point(291, 73)
        Me.lblPer.Name = "lblPer"
        Me.lblPer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPer.Size = New System.Drawing.Size(19, 13)
        Me.lblPer.TabIndex = 19
        Me.lblPer.Text = "per"
        Me.lblPer.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblPer.UseMnemonic = True
        Me.lblPer.Visible = True
        ' 
        ' fraFilters
        ' 
        Me.fraFilters.BackColor = System.Drawing.SystemColors.Control
        Me.fraFilters.Controls.Add(Me.txtCOB)
        Me.fraFilters.Controls.Add(Me.txtState)
        Me.fraFilters.Controls.Add(Me.txtCountry)
        Me.fraFilters.Controls.Add(Me.lblCOB)
        Me.fraFilters.Controls.Add(Me.lblState)
        Me.fraFilters.Controls.Add(Me.lblCountry)
        Me.fraFilters.Enabled = True
        Me.fraFilters.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.fraFilters.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFilters.Location = New System.Drawing.Point(8, 270)
        Me.fraFilters.Name = "fraFilters"
        Me.fraFilters.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFilters.Size = New System.Drawing.Size(615, 81)
        Me.fraFilters.TabIndex = 30
        Me.fraFilters.Text = "Active Filters"
        Me.fraFilters.Visible = True
        ' 
        ' txtCOB
        ' 
        Me.txtCOB.AcceptsReturn = True
        Me.txtCOB.AutoSize = False
        Me.txtCOB.BackColor = System.Drawing.SystemColors.Control
        Me.txtCOB.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtCOB.CausesValidation = True
        Me.txtCOB.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCOB.Enabled = True
        Me.txtCOB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.txtCOB.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCOB.HideSelection = True
        Me.txtCOB.Location = New System.Drawing.Point(138, 45)
        Me.txtCOB.MaxLength = 0
        Me.txtCOB.Multiline = False
        Me.txtCOB.Name = "txtCOB"
        Me.txtCOB.ReadOnly = True
        Me.txtCOB.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCOB.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.txtCOB.Size = New System.Drawing.Size(180, 21)
        Me.txtCOB.TabIndex = 35
        Me.txtCOB.TabStop = True
        Me.txtCOB.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtCOB.Visible = True
        ' 
        ' txtState
        ' 
        Me.txtState.AcceptsReturn = True
        Me.txtState.AutoSize = False
        Me.txtState.BackColor = System.Drawing.SystemColors.Control
        Me.txtState.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtState.CausesValidation = True
        Me.txtState.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtState.Enabled = True
        Me.txtState.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.txtState.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtState.HideSelection = True
        Me.txtState.Location = New System.Drawing.Point(418, 19)
        Me.txtState.MaxLength = 0
        Me.txtState.Multiline = False
        Me.txtState.Name = "txtState"
        Me.txtState.ReadOnly = True
        Me.txtState.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtState.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.txtState.Size = New System.Drawing.Size(180, 21)
        Me.txtState.TabIndex = 33
        Me.txtState.TabStop = True
        Me.txtState.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtState.Visible = True
        ' 
        ' txtCountry
        ' 
        Me.txtCountry.AcceptsReturn = True
        Me.txtCountry.AutoSize = False
        Me.txtCountry.BackColor = System.Drawing.SystemColors.Control
        Me.txtCountry.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtCountry.CausesValidation = True
        Me.txtCountry.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCountry.Enabled = True
        Me.txtCountry.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.txtCountry.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCountry.HideSelection = True
        Me.txtCountry.Location = New System.Drawing.Point(138, 20)
        Me.txtCountry.MaxLength = 0
        Me.txtCountry.Multiline = False
        Me.txtCountry.Name = "txtCountry"
        Me.txtCountry.ReadOnly = True
        Me.txtCountry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCountry.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.txtCountry.Size = New System.Drawing.Size(180, 21)
        Me.txtCountry.TabIndex = 31
        Me.txtCountry.TabStop = True
        Me.txtCountry.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtCountry.Visible = True
        ' 
        ' lblCOB
        ' 
        Me.lblCOB.AutoSize = True
        Me.lblCOB.BackColor = System.Drawing.SystemColors.Control
        Me.lblCOB.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblCOB.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCOB.Enabled = True
        Me.lblCOB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.lblCOB.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCOB.Location = New System.Drawing.Point(24, 48)
        Me.lblCOB.Name = "lblCOB"
        Me.lblCOB.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCOB.Size = New System.Drawing.Size(105, 13)
        Me.lblCOB.TabIndex = 36
        Me.lblCOB.Text = "Class of Business:"
        Me.lblCOB.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblCOB.UseMnemonic = True
        Me.lblCOB.Visible = True
        ' 
        ' lblState
        ' 
        Me.lblState.AutoSize = True
        Me.lblState.BackColor = System.Drawing.SystemColors.Control
        Me.lblState.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblState.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblState.Enabled = True
        Me.lblState.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.lblState.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblState.Location = New System.Drawing.Point(374, 22)
        Me.lblState.Name = "lblState"
        Me.lblState.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblState.Size = New System.Drawing.Size(35, 13)
        Me.lblState.TabIndex = 34
        Me.lblState.Text = "State:"
        Me.lblState.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblState.UseMnemonic = True
        Me.lblState.Visible = True
        ' 
        ' lblCountry
        ' 
        Me.lblCountry.AutoSize = True
        Me.lblCountry.BackColor = System.Drawing.SystemColors.Control
        Me.lblCountry.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblCountry.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCountry.Enabled = True
        Me.lblCountry.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.lblCountry.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCountry.Location = New System.Drawing.Point(78, 23)
        Me.lblCountry.Name = "lblCountry"
        Me.lblCountry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCountry.Size = New System.Drawing.Size(51, 13)
        Me.lblCountry.TabIndex = 32
        Me.lblCountry.Text = "Country:"
        Me.lblCountry.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblCountry.UseMnemonic = True
        Me.lblCountry.Visible = True
        ' 
        ' txtSumInsured
        ' 
        Me.txtSumInsured.AcceptsReturn = True
        Me.txtSumInsured.AutoSize = False
        Me.txtSumInsured.BackColor = System.Drawing.SystemColors.Control
        Me.txtSumInsured.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtSumInsured.CausesValidation = True
        Me.txtSumInsured.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSumInsured.Enabled = True
        Me.txtSumInsured.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.txtSumInsured.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSumInsured.HideSelection = True
        Me.txtSumInsured.Location = New System.Drawing.Point(144, 46)
        Me.txtSumInsured.MaxLength = 0
        Me.txtSumInsured.Multiline = False
        Me.txtSumInsured.Name = "txtSumInsured"
        Me.txtSumInsured.ReadOnly = True
        Me.txtSumInsured.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSumInsured.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.txtSumInsured.Size = New System.Drawing.Size(160, 21)
        Me.txtSumInsured.TabIndex = 37
        Me.txtSumInsured.TabStop = True
        Me.txtSumInsured.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtSumInsured.Visible = True
        ' 
        ' txtOriginalSumInsured
        ' 
        Me.txtOriginalSumInsured.AcceptsReturn = True
        Me.txtOriginalSumInsured.AutoSize = False
        Me.txtOriginalSumInsured.BackColor = System.Drawing.SystemColors.Control
        Me.txtOriginalSumInsured.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtOriginalSumInsured.CausesValidation = True
        Me.txtOriginalSumInsured.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOriginalSumInsured.Enabled = True
        Me.txtOriginalSumInsured.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.txtOriginalSumInsured.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOriginalSumInsured.HideSelection = True
        Me.txtOriginalSumInsured.Location = New System.Drawing.Point(144, 73)
        Me.txtOriginalSumInsured.MaxLength = 0
        Me.txtOriginalSumInsured.Multiline = False
        Me.txtOriginalSumInsured.Name = "txtOriginalSumInsured"
        Me.txtOriginalSumInsured.ReadOnly = True
        Me.txtOriginalSumInsured.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOriginalSumInsured.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.txtOriginalSumInsured.Size = New System.Drawing.Size(160, 21)
        Me.txtOriginalSumInsured.TabIndex = 39
        Me.txtOriginalSumInsured.TabStop = True
        Me.txtOriginalSumInsured.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtOriginalSumInsured.Visible = True
        ' 
        ' txtSumInsuredChange
        ' 
        Me.txtSumInsuredChange.AcceptsReturn = True
        Me.txtSumInsuredChange.AutoSize = False
        Me.txtSumInsuredChange.BackColor = System.Drawing.SystemColors.Control
        Me.txtSumInsuredChange.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtSumInsuredChange.CausesValidation = True
        Me.txtSumInsuredChange.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSumInsuredChange.Enabled = True
        Me.txtSumInsuredChange.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.txtSumInsuredChange.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSumInsuredChange.HideSelection = True
        Me.txtSumInsuredChange.Location = New System.Drawing.Point(144, 100)
        Me.txtSumInsuredChange.MaxLength = 0
        Me.txtSumInsuredChange.Multiline = False
        Me.txtSumInsuredChange.Name = "txtSumInsuredChange"
        Me.txtSumInsuredChange.ReadOnly = True
        Me.txtSumInsuredChange.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSumInsuredChange.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.txtSumInsuredChange.Size = New System.Drawing.Size(160, 21)
        Me.txtSumInsuredChange.TabIndex = 41
        Me.txtSumInsuredChange.TabStop = True
        Me.txtSumInsuredChange.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtSumInsuredChange.Visible = True
        ' 
        ' txtPremium
        ' 
        Me.txtPremium.AcceptsReturn = True
        Me.txtPremium.AutoSize = False
        Me.txtPremium.BackColor = System.Drawing.SystemColors.Control
        Me.txtPremium.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtPremium.CausesValidation = True
        Me.txtPremium.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPremium.Enabled = True
        Me.txtPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.txtPremium.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPremium.HideSelection = True
        Me.txtPremium.Location = New System.Drawing.Point(444, 46)
        Me.txtPremium.MaxLength = 0
        Me.txtPremium.Multiline = False
        Me.txtPremium.Name = "txtPremium"
        Me.txtPremium.ReadOnly = True
        Me.txtPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPremium.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.txtPremium.Size = New System.Drawing.Size(160, 21)
        Me.txtPremium.TabIndex = 43
        Me.txtPremium.TabStop = True
        Me.txtPremium.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtPremium.Visible = True
        ' 
        ' txtRunningTotal
        ' 
        Me.txtRunningTotal.AcceptsReturn = True
        Me.txtRunningTotal.AutoSize = False
        Me.txtRunningTotal.BackColor = System.Drawing.SystemColors.Control
        Me.txtRunningTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtRunningTotal.CausesValidation = True
        Me.txtRunningTotal.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRunningTotal.Enabled = True
        Me.txtRunningTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.txtRunningTotal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRunningTotal.HideSelection = True
        Me.txtRunningTotal.Location = New System.Drawing.Point(444, 73)
        Me.txtRunningTotal.MaxLength = 0
        Me.txtRunningTotal.Multiline = False
        Me.txtRunningTotal.Name = "txtRunningTotal"
        Me.txtRunningTotal.ReadOnly = True
        Me.txtRunningTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRunningTotal.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.txtRunningTotal.Size = New System.Drawing.Size(160, 21)
        Me.txtRunningTotal.TabIndex = 45
        Me.txtRunningTotal.TabStop = True
        Me.txtRunningTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtRunningTotal.Visible = True
        ' 
        ' frmInterface
        ' 
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(643, 490)
        Me.ControlBox = True
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabDetailTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Enabled = True
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.HelpButton = False
        Me.KeyPreview = False
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
        Me.Text = "Tax Band Rates"
        Me.WindowState = System.Windows.Forms.FormWindowState.Normal
        Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMainTab, 1)
        Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabDetailTab, 1)
        Me.listViewHelper1.SetItemClickMethod(Me.lvwRITax, "lvwRITax_ItemClick")
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwRITax, True)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.lvwRITax.ResumeLayout(False)
        Me.tabDetailTab.ResumeLayout(False)
        Me._tabDetailTab_TabPage0.ResumeLayout(False)
        Me.fraCalculation.ResumeLayout(False)
        Me.fraFilters.ResumeLayout(False)
        Me.ResumeLayout(False)
    End Sub
    Sub InitializeoptBasis()
        Me.optBasis(2) = _optBasis_2
        Me.optBasis(3) = _optBasis_3
        Me.optBasis(1) = _optBasis_1
        Me.optBasis(0) = _optBasis_0
    End Sub
#End Region
End Class