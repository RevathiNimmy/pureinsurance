<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTaxDetail
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializeoptBasis()
		InitializeoptApplyTax()
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
    Public WithEvents lblRunningTotal As System.Windows.Forms.Label
    Public WithEvents lblPremium As System.Windows.Forms.Label
    Public WithEvents lblSumInsuredChange As System.Windows.Forms.Label
    Public WithEvents lblOriginalSumInsured As System.Windows.Forms.Label
    Public WithEvents lblSumInsured As System.Windows.Forms.Label
    Public WithEvents lblCode As System.Windows.Forms.Label
    Public WithEvents lblValue As System.Windows.Forms.Label
    Public WithEvents txtRunningTotal As System.Windows.Forms.TextBox
    Public WithEvents txtPremium As System.Windows.Forms.TextBox
    Public WithEvents txtSumInsuredChange As System.Windows.Forms.TextBox
    Public WithEvents txtOriginalSumInsured As System.Windows.Forms.TextBox
    Public WithEvents txtSumInsured As System.Windows.Forms.TextBox
    Public WithEvents txtCountry As System.Windows.Forms.TextBox
    Public WithEvents txtState As System.Windows.Forms.TextBox
    Public WithEvents txtCOB As System.Windows.Forms.TextBox
    Public WithEvents lblCountry As System.Windows.Forms.Label
    Public WithEvents lblState As System.Windows.Forms.Label
    Public WithEvents lblCOB As System.Windows.Forms.Label
    Public WithEvents fraFilters As System.Windows.Forms.GroupBox
    Public WithEvents lblApplyTax As System.Windows.Forms.Label
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Public WithEvents txtBasisValue As System.Windows.Forms.TextBox
    Public WithEvents txtValue As System.Windows.Forms.TextBox
    Public WithEvents txtPercentage As System.Windows.Forms.TextBox
    Public WithEvents chkIsValue As System.Windows.Forms.CheckBox
    Public WithEvents chkRounded As System.Windows.Forms.CheckBox
    Public WithEvents chkAllowCredit As System.Windows.Forms.CheckBox
	Public WithEvents lblPer As System.Windows.Forms.Label
	Public WithEvents lblPercentage As System.Windows.Forms.Label
	Public WithEvents lblCalcBasis As System.Windows.Forms.Label
	Public WithEvents lblOfSI As System.Windows.Forms.Label
	Public WithEvents fraCalculation As System.Windows.Forms.GroupBox
	Public WithEvents txtTaxBand As System.Windows.Forms.TextBox
	Public WithEvents txtTaxValue As System.Windows.Forms.TextBox
	Public WithEvents cmdDetailOK As System.Windows.Forms.Button
	Public WithEvents cmdDetailCancel As System.Windows.Forms.Button
	Public WithEvents chkSpread As System.Windows.Forms.CheckBox
	Public WithEvents chkIncludeIns As System.Windows.Forms.CheckBox
	Public WithEvents chkNotApplied As System.Windows.Forms.CheckBox
	Public WithEvents fraInstalment As System.Windows.Forms.GroupBox
	Private WithEvents _tabDetailTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabDetailTab As System.Windows.Forms.TabControl
    Public optApplyTax(3) As System.Windows.Forms.RadioButton
	Public optBasis(3) As System.Windows.Forms.RadioButton
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tabDetailTab = New System.Windows.Forms.TabControl()
        Me._tabDetailTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.lblRunningTotal = New System.Windows.Forms.Label()
        Me.lblPremium = New System.Windows.Forms.Label()
        Me.lblSumInsuredChange = New System.Windows.Forms.Label()
        Me.lblOriginalSumInsured = New System.Windows.Forms.Label()
        Me.lblSumInsured = New System.Windows.Forms.Label()
        Me.lblCode = New System.Windows.Forms.Label()
        Me.lblValue = New System.Windows.Forms.Label()
        Me.txtRunningTotal = New System.Windows.Forms.TextBox()
        Me.txtPremium = New System.Windows.Forms.TextBox()
        Me.txtSumInsuredChange = New System.Windows.Forms.TextBox()
        Me.txtOriginalSumInsured = New System.Windows.Forms.TextBox()
        Me.txtSumInsured = New System.Windows.Forms.TextBox()
        Me.fraFilters = New System.Windows.Forms.GroupBox()
        Me.txtCountry = New System.Windows.Forms.TextBox()
        Me.txtState = New System.Windows.Forms.TextBox()
        Me.txtCOB = New System.Windows.Forms.TextBox()
        Me.lblCountry = New System.Windows.Forms.Label()
        Me.lblState = New System.Windows.Forms.Label()
        Me.lblCOB = New System.Windows.Forms.Label()
        Me.fraCalculation = New System.Windows.Forms.GroupBox()
        Me.Frame1 = New System.Windows.Forms.GroupBox()
        Me._optApplyTax_3 = New System.Windows.Forms.RadioButton()
        Me._optApplyTax_2 = New System.Windows.Forms.RadioButton()
        Me._optApplyTax_1 = New System.Windows.Forms.RadioButton()
        Me._optApplyTax_0 = New System.Windows.Forms.RadioButton()
        Me.lblApplyTax = New System.Windows.Forms.Label()
        Me.txtBasisValue = New System.Windows.Forms.TextBox()
        Me._optBasis_0 = New System.Windows.Forms.RadioButton()
        Me.txtValue = New System.Windows.Forms.TextBox()
        Me._optBasis_1 = New System.Windows.Forms.RadioButton()
        Me.txtPercentage = New System.Windows.Forms.TextBox()
        Me.chkIsValue = New System.Windows.Forms.CheckBox()
        Me.chkRounded = New System.Windows.Forms.CheckBox()
        Me._optBasis_3 = New System.Windows.Forms.RadioButton()
        Me._optBasis_2 = New System.Windows.Forms.RadioButton()
        Me.chkAllowCredit = New System.Windows.Forms.CheckBox()
        Me.lblPer = New System.Windows.Forms.Label()
        Me.lblPercentage = New System.Windows.Forms.Label()
        Me.lblCalcBasis = New System.Windows.Forms.Label()
        Me.lblOfSI = New System.Windows.Forms.Label()
        Me.txtTaxBand = New System.Windows.Forms.TextBox()
        Me.txtTaxValue = New System.Windows.Forms.TextBox()
        Me.cmdDetailOK = New System.Windows.Forms.Button()
        Me.cmdDetailCancel = New System.Windows.Forms.Button()
        Me.fraInstalment = New System.Windows.Forms.GroupBox()
        Me.chkSpread = New System.Windows.Forms.CheckBox()
        Me.chkIncludeIns = New System.Windows.Forms.CheckBox()
        Me.chkNotApplied = New System.Windows.Forms.CheckBox()
        Me.tabDetailTab.SuspendLayout()
        Me._tabDetailTab_TabPage0.SuspendLayout()
        Me.fraFilters.SuspendLayout()
        Me.fraCalculation.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me.fraInstalment.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabDetailTab
        '
        Me.tabDetailTab.Controls.Add(Me._tabDetailTab_TabPage0)
        Me.tabDetailTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabDetailTab.ItemSize = New System.Drawing.Size(633, 18)
        Me.tabDetailTab.Location = New System.Drawing.Point(0, 0)
        Me.tabDetailTab.Multiline = True
        Me.tabDetailTab.Name = "tabDetailTab"
        Me.tabDetailTab.SelectedIndex = 0
        Me.tabDetailTab.Size = New System.Drawing.Size(638, 483)
        Me.tabDetailTab.TabIndex = 42
        Me.tabDetailTab.TabStop = False
        '
        '_tabDetailTab_TabPage0
        '
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblRunningTotal)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblPremium)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblSumInsuredChange)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblOriginalSumInsured)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblSumInsured)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblCode)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblValue)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtRunningTotal)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtPremium)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtSumInsuredChange)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtOriginalSumInsured)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtSumInsured)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.fraFilters)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.fraCalculation)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtTaxBand)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtTaxValue)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdDetailOK)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdDetailCancel)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.fraInstalment)
        Me._tabDetailTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabDetailTab_TabPage0.Name = "_tabDetailTab_TabPage0"
        Me._tabDetailTab_TabPage0.Size = New System.Drawing.Size(630, 457)
        Me._tabDetailTab_TabPage0.TabIndex = 0
        Me._tabDetailTab_TabPage0.Text = "Details"
        '
        'lblRunningTotal
        '
        Me.lblRunningTotal.AutoSize = True
        Me.lblRunningTotal.BackColor = System.Drawing.SystemColors.Control
        Me.lblRunningTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRunningTotal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRunningTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRunningTotal.Location = New System.Drawing.Point(352, 69)
        Me.lblRunningTotal.Name = "lblRunningTotal"
        Me.lblRunningTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRunningTotal.Size = New System.Drawing.Size(90, 13)
        Me.lblRunningTotal.TabIndex = 31
        Me.lblRunningTotal.Text = "Running Total:"
        Me.lblRunningTotal.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblPremium
        '
        Me.lblPremium.AutoSize = True
        Me.lblPremium.BackColor = System.Drawing.SystemColors.Control
        Me.lblPremium.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPremium.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPremium.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPremium.Location = New System.Drawing.Point(379, 42)
        Me.lblPremium.Name = "lblPremium"
        Me.lblPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPremium.Size = New System.Drawing.Size(63, 13)
        Me.lblPremium.TabIndex = 29
        Me.lblPremium.Text = "Premium:"
        Me.lblPremium.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblSumInsuredChange
        '
        Me.lblSumInsuredChange.AutoSize = True
        Me.lblSumInsuredChange.BackColor = System.Drawing.SystemColors.Control
        Me.lblSumInsuredChange.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSumInsuredChange.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSumInsuredChange.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSumInsuredChange.Location = New System.Drawing.Point(8, 96)
        Me.lblSumInsuredChange.Name = "lblSumInsuredChange"
        Me.lblSumInsuredChange.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSumInsuredChange.Size = New System.Drawing.Size(134, 13)
        Me.lblSumInsuredChange.TabIndex = 32
        Me.lblSumInsuredChange.Text = "Sum Insured Change:"
        Me.lblSumInsuredChange.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblOriginalSumInsured
        '
        Me.lblOriginalSumInsured.AutoSize = True
        Me.lblOriginalSumInsured.BackColor = System.Drawing.SystemColors.Control
        Me.lblOriginalSumInsured.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOriginalSumInsured.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOriginalSumInsured.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOriginalSumInsured.Location = New System.Drawing.Point(8, 69)
        Me.lblOriginalSumInsured.Name = "lblOriginalSumInsured"
        Me.lblOriginalSumInsured.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOriginalSumInsured.Size = New System.Drawing.Size(134, 13)
        Me.lblOriginalSumInsured.TabIndex = 30
        Me.lblOriginalSumInsured.Text = "Original Sum Insured:"
        Me.lblOriginalSumInsured.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblSumInsured
        '
        Me.lblSumInsured.AutoSize = True
        Me.lblSumInsured.BackColor = System.Drawing.SystemColors.Control
        Me.lblSumInsured.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSumInsured.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSumInsured.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSumInsured.Location = New System.Drawing.Point(56, 42)
        Me.lblSumInsured.Name = "lblSumInsured"
        Me.lblSumInsured.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSumInsured.Size = New System.Drawing.Size(86, 13)
        Me.lblSumInsured.TabIndex = 28
        Me.lblSumInsured.Text = "Sum Insured:"
        Me.lblSumInsured.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblCode
        '
        Me.lblCode.AutoSize = True
        Me.lblCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCode.Location = New System.Drawing.Point(76, 10)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCode.Size = New System.Drawing.Size(66, 13)
        Me.lblCode.TabIndex = 27
        Me.lblCode.Text = "Tax Band:"
        Me.lblCode.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblValue
        '
        Me.lblValue.AutoSize = True
        Me.lblValue.BackColor = System.Drawing.SystemColors.Control
        Me.lblValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblValue.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblValue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblValue.Location = New System.Drawing.Point(376, 397)
        Me.lblValue.Name = "lblValue"
        Me.lblValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblValue.Size = New System.Drawing.Size(81, 13)
        Me.lblValue.TabIndex = 41
        Me.lblValue.Text = "Tax Amount:"
        Me.lblValue.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtRunningTotal
        '
        Me.txtRunningTotal.AcceptsReturn = True
        Me.txtRunningTotal.BackColor = System.Drawing.SystemColors.Control
        Me.txtRunningTotal.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRunningTotal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRunningTotal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRunningTotal.Location = New System.Drawing.Point(444, 65)
        Me.txtRunningTotal.MaxLength = 0
        Me.txtRunningTotal.Name = "txtRunningTotal"
        Me.txtRunningTotal.ReadOnly = True
        Me.txtRunningTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRunningTotal.Size = New System.Drawing.Size(160, 21)
        Me.txtRunningTotal.TabIndex = 5
        '
        'txtPremium
        '
        Me.txtPremium.AcceptsReturn = True
        Me.txtPremium.BackColor = System.Drawing.SystemColors.Control
        Me.txtPremium.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPremium.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPremium.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPremium.Location = New System.Drawing.Point(444, 38)
        Me.txtPremium.MaxLength = 0
        Me.txtPremium.Name = "txtPremium"
        Me.txtPremium.ReadOnly = True
        Me.txtPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPremium.Size = New System.Drawing.Size(160, 21)
        Me.txtPremium.TabIndex = 4
        '
        'txtSumInsuredChange
        '
        Me.txtSumInsuredChange.AcceptsReturn = True
        Me.txtSumInsuredChange.BackColor = System.Drawing.SystemColors.Control
        Me.txtSumInsuredChange.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSumInsuredChange.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSumInsuredChange.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSumInsuredChange.Location = New System.Drawing.Point(144, 92)
        Me.txtSumInsuredChange.MaxLength = 0
        Me.txtSumInsuredChange.Name = "txtSumInsuredChange"
        Me.txtSumInsuredChange.ReadOnly = True
        Me.txtSumInsuredChange.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSumInsuredChange.Size = New System.Drawing.Size(160, 21)
        Me.txtSumInsuredChange.TabIndex = 3
        '
        'txtOriginalSumInsured
        '
        Me.txtOriginalSumInsured.AcceptsReturn = True
        Me.txtOriginalSumInsured.BackColor = System.Drawing.SystemColors.Control
        Me.txtOriginalSumInsured.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOriginalSumInsured.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOriginalSumInsured.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOriginalSumInsured.Location = New System.Drawing.Point(144, 65)
        Me.txtOriginalSumInsured.MaxLength = 0
        Me.txtOriginalSumInsured.Name = "txtOriginalSumInsured"
        Me.txtOriginalSumInsured.ReadOnly = True
        Me.txtOriginalSumInsured.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOriginalSumInsured.Size = New System.Drawing.Size(160, 21)
        Me.txtOriginalSumInsured.TabIndex = 2
        '
        'txtSumInsured
        '
        Me.txtSumInsured.AcceptsReturn = True
        Me.txtSumInsured.BackColor = System.Drawing.SystemColors.Control
        Me.txtSumInsured.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSumInsured.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSumInsured.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSumInsured.Location = New System.Drawing.Point(144, 38)
        Me.txtSumInsured.MaxLength = 0
        Me.txtSumInsured.Name = "txtSumInsured"
        Me.txtSumInsured.ReadOnly = True
        Me.txtSumInsured.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSumInsured.Size = New System.Drawing.Size(160, 21)
        Me.txtSumInsured.TabIndex = 1
        '
        'fraFilters
        '
        Me.fraFilters.BackColor = System.Drawing.SystemColors.Control
        Me.fraFilters.Controls.Add(Me.txtCountry)
        Me.fraFilters.Controls.Add(Me.txtState)
        Me.fraFilters.Controls.Add(Me.txtCOB)
        Me.fraFilters.Controls.Add(Me.lblCountry)
        Me.fraFilters.Controls.Add(Me.lblState)
        Me.fraFilters.Controls.Add(Me.lblCOB)
        Me.fraFilters.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraFilters.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFilters.Location = New System.Drawing.Point(8, 318)
        Me.fraFilters.Name = "fraFilters"
        Me.fraFilters.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFilters.Size = New System.Drawing.Size(615, 73)
        Me.fraFilters.TabIndex = 8
        Me.fraFilters.TabStop = False
        Me.fraFilters.Text = "Active Filters"
        '
        'txtCountry
        '
        Me.txtCountry.AcceptsReturn = True
        Me.txtCountry.BackColor = System.Drawing.SystemColors.Control
        Me.txtCountry.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCountry.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCountry.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCountry.Location = New System.Drawing.Point(138, 20)
        Me.txtCountry.MaxLength = 0
        Me.txtCountry.Name = "txtCountry"
        Me.txtCountry.ReadOnly = True
        Me.txtCountry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCountry.Size = New System.Drawing.Size(180, 21)
        Me.txtCountry.TabIndex = 21
        '
        'txtState
        '
        Me.txtState.AcceptsReturn = True
        Me.txtState.BackColor = System.Drawing.SystemColors.Control
        Me.txtState.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtState.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtState.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtState.Location = New System.Drawing.Point(418, 19)
        Me.txtState.MaxLength = 0
        Me.txtState.Name = "txtState"
        Me.txtState.ReadOnly = True
        Me.txtState.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtState.Size = New System.Drawing.Size(180, 21)
        Me.txtState.TabIndex = 23
        '
        'txtCOB
        '
        Me.txtCOB.AcceptsReturn = True
        Me.txtCOB.BackColor = System.Drawing.SystemColors.Control
        Me.txtCOB.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCOB.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCOB.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCOB.Location = New System.Drawing.Point(138, 45)
        Me.txtCOB.MaxLength = 0
        Me.txtCOB.Name = "txtCOB"
        Me.txtCOB.ReadOnly = True
        Me.txtCOB.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCOB.Size = New System.Drawing.Size(180, 21)
        Me.txtCOB.TabIndex = 22
        '
        'lblCountry
        '
        Me.lblCountry.AutoSize = True
        Me.lblCountry.BackColor = System.Drawing.SystemColors.Control
        Me.lblCountry.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCountry.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCountry.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCountry.Location = New System.Drawing.Point(78, 23)
        Me.lblCountry.Name = "lblCountry"
        Me.lblCountry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCountry.Size = New System.Drawing.Size(58, 13)
        Me.lblCountry.TabIndex = 38
        Me.lblCountry.Text = "Country:"
        Me.lblCountry.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblState
        '
        Me.lblState.AutoSize = True
        Me.lblState.BackColor = System.Drawing.SystemColors.Control
        Me.lblState.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblState.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblState.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblState.Location = New System.Drawing.Point(374, 22)
        Me.lblState.Name = "lblState"
        Me.lblState.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblState.Size = New System.Drawing.Size(42, 13)
        Me.lblState.TabIndex = 39
        Me.lblState.Text = "State:"
        Me.lblState.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblCOB
        '
        Me.lblCOB.AutoSize = True
        Me.lblCOB.BackColor = System.Drawing.SystemColors.Control
        Me.lblCOB.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCOB.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCOB.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCOB.Location = New System.Drawing.Point(24, 48)
        Me.lblCOB.Name = "lblCOB"
        Me.lblCOB.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCOB.Size = New System.Drawing.Size(112, 13)
        Me.lblCOB.TabIndex = 40
        Me.lblCOB.Text = "Class of Business:"
        Me.lblCOB.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'fraCalculation
        '
        Me.fraCalculation.BackColor = System.Drawing.SystemColors.Control
        Me.fraCalculation.Controls.Add(Me.Frame1)
        Me.fraCalculation.Controls.Add(Me.txtBasisValue)
        Me.fraCalculation.Controls.Add(Me._optBasis_0)
        Me.fraCalculation.Controls.Add(Me.txtValue)
        Me.fraCalculation.Controls.Add(Me._optBasis_1)
        Me.fraCalculation.Controls.Add(Me.txtPercentage)
        Me.fraCalculation.Controls.Add(Me.chkIsValue)
        Me.fraCalculation.Controls.Add(Me.chkRounded)
        Me.fraCalculation.Controls.Add(Me._optBasis_3)
        Me.fraCalculation.Controls.Add(Me._optBasis_2)
        Me.fraCalculation.Controls.Add(Me.chkAllowCredit)
        Me.fraCalculation.Controls.Add(Me.lblPer)
        Me.fraCalculation.Controls.Add(Me.lblPercentage)
        Me.fraCalculation.Controls.Add(Me.lblCalcBasis)
        Me.fraCalculation.Controls.Add(Me.lblOfSI)
        Me.fraCalculation.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCalculation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraCalculation.Location = New System.Drawing.Point(8, 116)
        Me.fraCalculation.Name = "fraCalculation"
        Me.fraCalculation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraCalculation.Size = New System.Drawing.Size(615, 157)
        Me.fraCalculation.TabIndex = 6
        Me.fraCalculation.TabStop = False
        Me.fraCalculation.Text = "Calculation"
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me._optApplyTax_3)
        Me.Frame1.Controls.Add(Me._optApplyTax_2)
        Me.Frame1.Controls.Add(Me._optApplyTax_1)
        Me.Frame1.Controls.Add(Me._optApplyTax_0)
        Me.Frame1.Controls.Add(Me.lblApplyTax)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(34, 37)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(566, 33)
        Me.Frame1.TabIndex = 10
        Me.Frame1.TabStop = False
        '
        '_optApplyTax_3
        '
        Me._optApplyTax_3.BackColor = System.Drawing.SystemColors.Control
        Me._optApplyTax_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._optApplyTax_3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optApplyTax_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optApplyTax_3.Location = New System.Drawing.Point(425, 11)
        Me._optApplyTax_3.Name = "_optApplyTax_3"
        Me._optApplyTax_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optApplyTax_3.Size = New System.Drawing.Size(135, 20)
        Me._optApplyTax_3.TabIndex = 48
        Me._optApplyTax_3.TabStop = True
        Me._optApplyTax_3.Text = "Inception date TPI"
        Me._optApplyTax_3.UseVisualStyleBackColor = False
        '
        '_optApplyTax_2
        '
        Me._optApplyTax_2.BackColor = System.Drawing.SystemColors.Control
        Me._optApplyTax_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optApplyTax_2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optApplyTax_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optApplyTax_2.Location = New System.Drawing.Point(314, 11)
        Me._optApplyTax_2.Name = "_optApplyTax_2"
        Me._optApplyTax_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optApplyTax_2.Size = New System.Drawing.Size(108, 20)
        Me._optApplyTax_2.TabIndex = 12
        Me._optApplyTax_2.TabStop = True
        Me._optApplyTax_2.Text = "Inception date"
        Me._optApplyTax_2.UseVisualStyleBackColor = False
        '
        '_optApplyTax_1
        '
        Me._optApplyTax_1.BackColor = System.Drawing.SystemColors.Control
        Me._optApplyTax_1.Checked = True
        Me._optApplyTax_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optApplyTax_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optApplyTax_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optApplyTax_1.Location = New System.Drawing.Point(210, 11)
        Me._optApplyTax_1.Name = "_optApplyTax_1"
        Me._optApplyTax_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optApplyTax_1.Size = New System.Drawing.Size(109, 20)
        Me._optApplyTax_1.TabIndex = 11
        Me._optApplyTax_1.TabStop = True
        Me._optApplyTax_1.Text = "Effective date"
        Me._optApplyTax_1.UseVisualStyleBackColor = False
        '
        '_optApplyTax_0
        '
        Me._optApplyTax_0.BackColor = System.Drawing.SystemColors.Control
        Me._optApplyTax_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optApplyTax_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optApplyTax_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optApplyTax_0.Location = New System.Drawing.Point(88, 11)
        Me._optApplyTax_0.Name = "_optApplyTax_0"
        Me._optApplyTax_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optApplyTax_0.Size = New System.Drawing.Size(124, 20)
        Me._optApplyTax_0.TabIndex = 10
        Me._optApplyTax_0.TabStop = True
        Me._optApplyTax_0.Text = "Transaction date"
        Me._optApplyTax_0.UseVisualStyleBackColor = False
        '
        'lblApplyTax
        '
        Me.lblApplyTax.AutoSize = True
        Me.lblApplyTax.BackColor = System.Drawing.SystemColors.Control
        Me.lblApplyTax.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblApplyTax.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblApplyTax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblApplyTax.Location = New System.Drawing.Point(4, 13)
        Me.lblApplyTax.Name = "lblApplyTax"
        Me.lblApplyTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblApplyTax.Size = New System.Drawing.Size(84, 13)
        Me.lblApplyTax.TabIndex = 47
        Me.lblApplyTax.Text = "Apply tax by:"
        Me.lblApplyTax.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtBasisValue
        '
        Me.txtBasisValue.AcceptsReturn = True
        Me.txtBasisValue.BackColor = System.Drawing.SystemColors.Window
        Me.txtBasisValue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBasisValue.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBasisValue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBasisValue.Location = New System.Drawing.Point(320, 102)
        Me.txtBasisValue.MaxLength = 0
        Me.txtBasisValue.Name = "txtBasisValue"
        Me.txtBasisValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBasisValue.Size = New System.Drawing.Size(140, 21)
        Me.txtBasisValue.TabIndex = 15
        '
        '_optBasis_0
        '
        Me._optBasis_0.BackColor = System.Drawing.SystemColors.Control
        Me._optBasis_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optBasis_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optBasis_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optBasis_0.Location = New System.Drawing.Point(137, 19)
        Me._optBasis_0.Name = "_optBasis_0"
        Me._optBasis_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optBasis_0.Size = New System.Drawing.Size(84, 15)
        Me._optBasis_0.TabIndex = 6
        Me._optBasis_0.TabStop = True
        Me._optBasis_0.Text = "Premium"
        Me._optBasis_0.UseVisualStyleBackColor = False
        '
        'txtValue
        '
        Me.txtValue.AcceptsReturn = True
        Me.txtValue.BackColor = System.Drawing.SystemColors.Window
        Me.txtValue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtValue.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtValue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtValue.Location = New System.Drawing.Point(136, 102)
        Me.txtValue.MaxLength = 0
        Me.txtValue.Name = "txtValue"
        Me.txtValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtValue.Size = New System.Drawing.Size(140, 21)
        Me.txtValue.TabIndex = 14
        '
        '_optBasis_1
        '
        Me._optBasis_1.BackColor = System.Drawing.SystemColors.Control
        Me._optBasis_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optBasis_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optBasis_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optBasis_1.Location = New System.Drawing.Point(227, 19)
        Me._optBasis_1.Name = "_optBasis_1"
        Me._optBasis_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optBasis_1.Size = New System.Drawing.Size(96, 15)
        Me._optBasis_1.TabIndex = 7
        Me._optBasis_1.TabStop = True
        Me._optBasis_1.Text = "Sum Insured"
        Me._optBasis_1.UseVisualStyleBackColor = False
        '
        'txtPercentage
        '
        Me.txtPercentage.AcceptsReturn = True
        Me.txtPercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtPercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPercentage.Location = New System.Drawing.Point(136, 102)
        Me.txtPercentage.MaxLength = 0
        Me.txtPercentage.Name = "txtPercentage"
        Me.txtPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPercentage.Size = New System.Drawing.Size(140, 20)
        Me.txtPercentage.TabIndex = 35
        '
        'chkIsValue
        '
        Me.chkIsValue.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsValue.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsValue.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsValue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsValue.Location = New System.Drawing.Point(74, 76)
        Me.chkIsValue.Name = "chkIsValue"
        Me.chkIsValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsValue.Size = New System.Drawing.Size(77, 15)
        Me.chkIsValue.TabIndex = 13
        Me.chkIsValue.Text = "Is Value?"
        Me.chkIsValue.UseVisualStyleBackColor = False
        '
        'chkRounded
        '
        Me.chkRounded.BackColor = System.Drawing.SystemColors.Control
        Me.chkRounded.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkRounded.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRounded.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRounded.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRounded.Location = New System.Drawing.Point(384, 134)
        Me.chkRounded.Name = "chkRounded"
        Me.chkRounded.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRounded.Size = New System.Drawing.Size(77, 15)
        Me.chkRounded.TabIndex = 17
        Me.chkRounded.Text = "Rounded?"
        Me.chkRounded.UseVisualStyleBackColor = False
        '
        '_optBasis_3
        '
        Me._optBasis_3.BackColor = System.Drawing.SystemColors.Control
        Me._optBasis_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._optBasis_3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optBasis_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optBasis_3.Location = New System.Drawing.Point(487, 18)
        Me._optBasis_3.Name = "_optBasis_3"
        Me._optBasis_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optBasis_3.Size = New System.Drawing.Size(118, 23)
        Me._optBasis_3.TabIndex = 9
        Me._optBasis_3.TabStop = True
        Me._optBasis_3.Text = "Running Total"
        Me._optBasis_3.UseVisualStyleBackColor = False
        '
        '_optBasis_2
        '
        Me._optBasis_2.BackColor = System.Drawing.SystemColors.Control
        Me._optBasis_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optBasis_2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optBasis_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optBasis_2.Location = New System.Drawing.Point(339, 21)
        Me._optBasis_2.Name = "_optBasis_2"
        Me._optBasis_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optBasis_2.Size = New System.Drawing.Size(143, 15)
        Me._optBasis_2.TabIndex = 8
        Me._optBasis_2.TabStop = True
        Me._optBasis_2.Text = "Sum Insured Change"
        Me._optBasis_2.UseVisualStyleBackColor = False
        '
        'chkAllowCredit
        '
        Me.chkAllowCredit.BackColor = System.Drawing.SystemColors.Control
        Me.chkAllowCredit.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAllowCredit.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAllowCredit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAllowCredit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAllowCredit.Location = New System.Drawing.Point(44, 134)
        Me.chkAllowCredit.Name = "chkAllowCredit"
        Me.chkAllowCredit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllowCredit.Size = New System.Drawing.Size(107, 15)
        Me.chkAllowCredit.TabIndex = 16
        Me.chkAllowCredit.Text = "Allow CR Tax?"
        Me.chkAllowCredit.UseVisualStyleBackColor = False
        '
        'lblPer
        '
        Me.lblPer.AutoSize = True
        Me.lblPer.BackColor = System.Drawing.SystemColors.Control
        Me.lblPer.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPer.Location = New System.Drawing.Point(293, 105)
        Me.lblPer.Name = "lblPer"
        Me.lblPer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPer.Size = New System.Drawing.Size(26, 13)
        Me.lblPer.TabIndex = 12
        Me.lblPer.Text = "per"
        Me.lblPer.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblPercentage
        '
        Me.lblPercentage.AutoSize = True
        Me.lblPercentage.BackColor = System.Drawing.SystemColors.Control
        Me.lblPercentage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPercentage.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPercentage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPercentage.Location = New System.Drawing.Point(92, 105)
        Me.lblPercentage.Name = "lblPercentage"
        Me.lblPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPercentage.Size = New System.Drawing.Size(38, 13)
        Me.lblPercentage.TabIndex = 14
        Me.lblPercentage.Text = "Rate:"
        Me.lblPercentage.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblCalcBasis
        '
        Me.lblCalcBasis.AutoSize = True
        Me.lblCalcBasis.BackColor = System.Drawing.SystemColors.Control
        Me.lblCalcBasis.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCalcBasis.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCalcBasis.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCalcBasis.Location = New System.Drawing.Point(26, 19)
        Me.lblCalcBasis.Name = "lblCalcBasis"
        Me.lblCalcBasis.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCalcBasis.Size = New System.Drawing.Size(109, 13)
        Me.lblCalcBasis.TabIndex = 33
        Me.lblCalcBasis.Text = "Calculation Basis:"
        Me.lblCalcBasis.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblOfSI
        '
        Me.lblOfSI.AutoSize = True
        Me.lblOfSI.BackColor = System.Drawing.SystemColors.Control
        Me.lblOfSI.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOfSI.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOfSI.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOfSI.Location = New System.Drawing.Point(469, 105)
        Me.lblOfSI.Name = "lblOfSI"
        Me.lblOfSI.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOfSI.Size = New System.Drawing.Size(96, 13)
        Me.lblOfSI.TabIndex = 37
        Me.lblOfSI.Text = "of Sum Insured"
        Me.lblOfSI.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtTaxBand
        '
        Me.txtTaxBand.AcceptsReturn = True
        Me.txtTaxBand.BackColor = System.Drawing.SystemColors.Control
        Me.txtTaxBand.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTaxBand.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTaxBand.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTaxBand.Location = New System.Drawing.Point(144, 7)
        Me.txtTaxBand.MaxLength = 0
        Me.txtTaxBand.Name = "txtTaxBand"
        Me.txtTaxBand.ReadOnly = True
        Me.txtTaxBand.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTaxBand.Size = New System.Drawing.Size(460, 21)
        Me.txtTaxBand.TabIndex = 0
        '
        'txtTaxValue
        '
        Me.txtTaxValue.AcceptsReturn = True
        Me.txtTaxValue.BackColor = System.Drawing.SystemColors.Control
        Me.txtTaxValue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTaxValue.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTaxValue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTaxValue.Location = New System.Drawing.Point(462, 394)
        Me.txtTaxValue.MaxLength = 0
        Me.txtTaxValue.Name = "txtTaxValue"
        Me.txtTaxValue.ReadOnly = True
        Me.txtTaxValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTaxValue.Size = New System.Drawing.Size(160, 21)
        Me.txtTaxValue.TabIndex = 24
        '
        'cmdDetailOK
        '
        Me.cmdDetailOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDetailOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDetailOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDetailOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDetailOK.Location = New System.Drawing.Point(470, 426)
        Me.cmdDetailOK.Name = "cmdDetailOK"
        Me.cmdDetailOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDetailOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdDetailOK.TabIndex = 25
        Me.cmdDetailOK.Text = "&OK"
        Me.cmdDetailOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDetailOK.UseVisualStyleBackColor = False
        '
        'cmdDetailCancel
        '
        Me.cmdDetailCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDetailCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDetailCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDetailCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDetailCancel.Location = New System.Drawing.Point(550, 426)
        Me.cmdDetailCancel.Name = "cmdDetailCancel"
        Me.cmdDetailCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDetailCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdDetailCancel.TabIndex = 26
        Me.cmdDetailCancel.Text = "&Cancel"
        Me.cmdDetailCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDetailCancel.UseVisualStyleBackColor = False
        '
        'fraInstalment
        '
        Me.fraInstalment.BackColor = System.Drawing.SystemColors.Control
        Me.fraInstalment.Controls.Add(Me.chkSpread)
        Me.fraInstalment.Controls.Add(Me.chkIncludeIns)
        Me.fraInstalment.Controls.Add(Me.chkNotApplied)
        Me.fraInstalment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraInstalment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraInstalment.Location = New System.Drawing.Point(8, 276)
        Me.fraInstalment.Name = "fraInstalment"
        Me.fraInstalment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraInstalment.Size = New System.Drawing.Size(617, 41)
        Me.fraInstalment.TabIndex = 7
        Me.fraInstalment.TabStop = False
        Me.fraInstalment.Text = "Instalment"
        '
        'chkSpread
        '
        Me.chkSpread.BackColor = System.Drawing.SystemColors.Control
        Me.chkSpread.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSpread.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSpread.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSpread.Location = New System.Drawing.Point(400, 16)
        Me.chkSpread.Name = "chkSpread"
        Me.chkSpread.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSpread.Size = New System.Drawing.Size(201, 21)
        Me.chkSpread.TabIndex = 20
        Me.chkSpread.Text = "Spread Tax across Instalment"
        Me.chkSpread.UseVisualStyleBackColor = False
        '
        'chkIncludeIns
        '
        Me.chkIncludeIns.BackColor = System.Drawing.SystemColors.Control
        Me.chkIncludeIns.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIncludeIns.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIncludeIns.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIncludeIns.Location = New System.Drawing.Point(200, 16)
        Me.chkIncludeIns.Name = "chkIncludeIns"
        Me.chkIncludeIns.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIncludeIns.Size = New System.Drawing.Size(193, 21)
        Me.chkIncludeIns.TabIndex = 19
        Me.chkIncludeIns.Text = "Include Tax in Instalment"
        Me.chkIncludeIns.UseVisualStyleBackColor = False
        '
        'chkNotApplied
        '
        Me.chkNotApplied.BackColor = System.Drawing.SystemColors.Control
        Me.chkNotApplied.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkNotApplied.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkNotApplied.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkNotApplied.Location = New System.Drawing.Point(8, 16)
        Me.chkNotApplied.Name = "chkNotApplied"
        Me.chkNotApplied.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkNotApplied.Size = New System.Drawing.Size(169, 21)
        Me.chkNotApplied.TabIndex = 18
        Me.chkNotApplied.Text = "    Is not applied to client"
        Me.chkNotApplied.UseVisualStyleBackColor = False
        '
        'frmTaxDetail
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(633, 476)
        Me.Controls.Add(Me.tabDetailTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 30)
        Me.Name = "frmTaxDetail"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Taxes"
        Me.tabDetailTab.ResumeLayout(False)
        Me._tabDetailTab_TabPage0.ResumeLayout(False)
        Me._tabDetailTab_TabPage0.PerformLayout()
        Me.fraFilters.ResumeLayout(False)
        Me.fraFilters.PerformLayout()
        Me.fraCalculation.ResumeLayout(False)
        Me.fraCalculation.PerformLayout()
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me.fraInstalment.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
	Sub InitializeoptBasis()
		Me.optBasis(0) = _optBasis_0
		Me.optBasis(1) = _optBasis_1
		Me.optBasis(3) = _optBasis_3
		Me.optBasis(2) = _optBasis_2
	End Sub
    Sub InitializeoptApplyTax()
        Me.optApplyTax(3) = _optApplyTax_3
        Me.optApplyTax(2) = _optApplyTax_2
        Me.optApplyTax(1) = _optApplyTax_1
        Me.optApplyTax(0) = _optApplyTax_0
    End Sub
    Public WithEvents _optApplyTax_2 As System.Windows.Forms.RadioButton
    Public WithEvents _optApplyTax_1 As System.Windows.Forms.RadioButton
    Public WithEvents _optApplyTax_0 As System.Windows.Forms.RadioButton
    Public WithEvents _optBasis_0 As System.Windows.Forms.RadioButton
    Public WithEvents _optBasis_1 As System.Windows.Forms.RadioButton
    Public WithEvents _optBasis_3 As System.Windows.Forms.RadioButton
    Public WithEvents _optBasis_2 As System.Windows.Forms.RadioButton
    Public WithEvents _optApplyTax_3 As System.Windows.Forms.RadioButton
#End Region 
End Class