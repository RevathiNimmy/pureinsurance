<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDetail
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		Form_Initialize_Renamed()
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
    Public WithEvents cboRatingSectionType As System.Windows.Forms.ComboBox
    Public WithEvents cboRateType As PMLookupControl.cboPMLookup
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Private WithEvents _StatusBar1_Panel1 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents StatusBar1 As System.Windows.Forms.StatusStrip
    Public WithEvents lblRatingSectiontype As System.Windows.Forms.Label
    Public WithEvents lblSumInsured As System.Windows.Forms.Label
    Public WithEvents lblRatetype As System.Windows.Forms.Label
    Public WithEvents lblRate As System.Windows.Forms.Label
    Public WithEvents lblOverflow As System.Windows.Forms.Label
    Public WithEvents lblDefinedCurrency As System.Windows.Forms.Label
    Public WithEvents lblTransactionCurrency As System.Windows.Forms.Label
    Public WithEvents lblThisPremium As System.Windows.Forms.Label
    Public WithEvents lblPremium As System.Windows.Forms.Label
    Public WithEvents lblState As System.Windows.Forms.Label
    Public WithEvents lblCountry As System.Windows.Forms.Label
    Public WithEvents lblOverrideReason As System.Windows.Forms.Label
    Public WithEvents lblEarningPattern As System.Windows.Forms.Label
    Public WithEvents cboEarningPattern As PMLookupControl.cboPMLookup
    Public WithEvents cboState As PMLookupControl.cboPMLookup
    Public WithEvents cboCountry As PMLookupControl.cboPMLookup
    Public WithEvents txtSumInsured As System.Windows.Forms.TextBox
    Public WithEvents txtRate As System.Windows.Forms.TextBox
    Public WithEvents txtPremium As System.Windows.Forms.TextBox
    Public WithEvents txtRate2 As System.Windows.Forms.TextBox
    Public WithEvents txtRate3 As System.Windows.Forms.TextBox
    Public WithEvents txtPremiumDefCurr As System.Windows.Forms.TextBox
    Public WithEvents txtThisPremiumDefCurr As System.Windows.Forms.TextBox
    Public WithEvents txtThisPremium As System.Windows.Forms.TextBox
    Public WithEvents txtOverrideReason As System.Windows.Forms.TextBox
    Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents SSTab1 As System.Windows.Forms.TabControl
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cboRatingSectionType = New System.Windows.Forms.ComboBox
        Me.cboRateType = New PMLookupControl.cboPMLookup
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.StatusBar1 = New System.Windows.Forms.StatusStrip
        Me._StatusBar1_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblRatingSectiontype = New System.Windows.Forms.Label
        Me.lblSumInsured = New System.Windows.Forms.Label
        Me.lblRatetype = New System.Windows.Forms.Label
        Me.lblRate = New System.Windows.Forms.Label
        Me.lblOverflow = New System.Windows.Forms.Label
        Me.lblDefinedCurrency = New System.Windows.Forms.Label
        Me.lblTransactionCurrency = New System.Windows.Forms.Label
        Me.lblThisPremium = New System.Windows.Forms.Label
        Me.lblPremium = New System.Windows.Forms.Label
        Me.lblState = New System.Windows.Forms.Label
        Me.lblCountry = New System.Windows.Forms.Label
        Me.lblOverrideReason = New System.Windows.Forms.Label
        Me.lblEarningPattern = New System.Windows.Forms.Label
        Me.cboEarningPattern = New PMLookupControl.cboPMLookup
        Me.cboState = New PMLookupControl.cboPMLookup
        Me.cboCountry = New PMLookupControl.cboPMLookup
        Me.txtSumInsured = New System.Windows.Forms.TextBox
        Me.txtRate = New System.Windows.Forms.TextBox
        Me.txtPremium = New System.Windows.Forms.TextBox
        Me.txtRate2 = New System.Windows.Forms.TextBox
        Me.txtRate3 = New System.Windows.Forms.TextBox
        Me.txtPremiumDefCurr = New System.Windows.Forms.TextBox
        Me.txtThisPremiumDefCurr = New System.Windows.Forms.TextBox
        Me.txtThisPremium = New System.Windows.Forms.TextBox
        Me.txtOverrideReason = New System.Windows.Forms.TextBox
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.StatusBar1.SuspendLayout()
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cboRatingSectionType
        '
        Me.cboRatingSectionType.BackColor = System.Drawing.SystemColors.Window
        Me.cboRatingSectionType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboRatingSectionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRatingSectionType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRatingSectionType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboRatingSectionType.Location = New System.Drawing.Point(139, 15)
        Me.cboRatingSectionType.Name = "cboRatingSectionType"
        Me.cboRatingSectionType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboRatingSectionType.Size = New System.Drawing.Size(337, 21)
        Me.cboRatingSectionType.TabIndex = 1
        '
        'cboRateType
        '
        Me.cboRateType.DefaultItemId = 0
        Me.cboRateType.FirstItem = ""
        Me.cboRateType.ItemId = 0
        Me.cboRateType.ListIndex = -1
        Me.cboRateType.Location = New System.Drawing.Point(139, 72)
        Me.cboRateType.Name = "cboRateType"
        Me.cboRateType.PMLookupProductFamily = 1
        Me.cboRateType.SingleItemId = 0
        Me.cboRateType.Size = New System.Drawing.Size(193, 21)
        Me.cboRateType.Sorted = True
        Me.cboRateType.TabIndex = 5
        Me.cboRateType.TableName = "Rate_type"
        Me.cboRateType.ToolTipText = ""
        Me.cboRateType.WhereClause = ""
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(320, 350)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 1
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(480, 350)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 3
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(400, 350)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'StatusBar1
        '
        Me.StatusBar1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusBar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._StatusBar1_Panel1})
        Me.StatusBar1.Location = New System.Drawing.Point(0, 378)
        Me.StatusBar1.Name = "StatusBar1"
        Me.StatusBar1.ShowItemToolTips = True
        Me.StatusBar1.Size = New System.Drawing.Size(591, 22)
        Me.StatusBar1.TabIndex = 4
        '
        '_StatusBar1_Panel1
        '
        Me._StatusBar1_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._StatusBar1_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._StatusBar1_Panel1.DoubleClickEnabled = True
        Me._StatusBar1_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._StatusBar1_Panel1.Name = "_StatusBar1_Panel1"
        Me._StatusBar1_Panel1.Size = New System.Drawing.Size(4, 22)
        Me._StatusBar1_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(548, 18)
        Me.SSTab1.Location = New System.Drawing.Point(4, 6)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(553, 341)
        Me.SSTab1.TabIndex = 0
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.lblRatingSectiontype)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblSumInsured)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblRatetype)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboRatingSectionType)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboRateType)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblRate)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblOverflow)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblDefinedCurrency)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblTransactionCurrency)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblThisPremium)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblPremium)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblState)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblCountry)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblOverrideReason)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblEarningPattern)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboEarningPattern)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboState)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboCountry)
        Me._SSTab1_TabPage0.Controls.Add(Me.txtSumInsured)
        Me._SSTab1_TabPage0.Controls.Add(Me.txtRate)
        Me._SSTab1_TabPage0.Controls.Add(Me.txtPremium)
        Me._SSTab1_TabPage0.Controls.Add(Me.txtRate2)
        Me._SSTab1_TabPage0.Controls.Add(Me.txtRate3)
        Me._SSTab1_TabPage0.Controls.Add(Me.txtPremiumDefCurr)
        Me._SSTab1_TabPage0.Controls.Add(Me.txtThisPremiumDefCurr)
        Me._SSTab1_TabPage0.Controls.Add(Me.txtThisPremium)
        Me._SSTab1_TabPage0.Controls.Add(Me.txtOverrideReason)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(545, 315)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "1 - General"
        Me._SSTab1_TabPage0.UseVisualStyleBackColor = True
        '
        'lblRatingSectiontype
        '
        Me.lblRatingSectiontype.AutoSize = True
        Me.lblRatingSectiontype.BackColor = System.Drawing.SystemColors.Control
        Me.lblRatingSectiontype.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRatingSectiontype.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRatingSectiontype.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRatingSectiontype.Location = New System.Drawing.Point(12, 18)
        Me.lblRatingSectiontype.Name = "lblRatingSectiontype"
        Me.lblRatingSectiontype.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRatingSectiontype.Size = New System.Drawing.Size(101, 13)
        Me.lblRatingSectiontype.TabIndex = 0
        Me.lblRatingSectiontype.Text = "Rating section type:"
        '
        'lblSumInsured
        '
        Me.lblSumInsured.AutoSize = True
        Me.lblSumInsured.BackColor = System.Drawing.SystemColors.Control
        Me.lblSumInsured.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSumInsured.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSumInsured.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSumInsured.Location = New System.Drawing.Point(12, 131)
        Me.lblSumInsured.Name = "lblSumInsured"
        Me.lblSumInsured.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSumInsured.Size = New System.Drawing.Size(68, 13)
        Me.lblSumInsured.TabIndex = 8
        Me.lblSumInsured.Text = "Sum insured:"
        '
        'lblRatetype
        '
        Me.lblRatetype.AutoSize = True
        Me.lblRatetype.BackColor = System.Drawing.SystemColors.Control
        Me.lblRatetype.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRatetype.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRatetype.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRatetype.Location = New System.Drawing.Point(12, 76)
        Me.lblRatetype.Name = "lblRatetype"
        Me.lblRatetype.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRatetype.Size = New System.Drawing.Size(56, 13)
        Me.lblRatetype.TabIndex = 4
        Me.lblRatetype.Text = "Rate type:"
        '
        'lblRate
        '
        Me.lblRate.AutoSize = True
        Me.lblRate.BackColor = System.Drawing.SystemColors.Control
        Me.lblRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRate.Location = New System.Drawing.Point(12, 101)
        Me.lblRate.Name = "lblRate"
        Me.lblRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRate.Size = New System.Drawing.Size(64, 13)
        Me.lblRate.TabIndex = 6
        Me.lblRate.Text = "Annual rate:"
        '
        'lblOverflow
        '
        Me.lblOverflow.BackColor = System.Drawing.SystemColors.Control
        Me.lblOverflow.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOverflow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOverflow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOverflow.Location = New System.Drawing.Point(386, 128)
        Me.lblOverflow.Name = "lblOverflow"
        Me.lblOverflow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOverflow.Size = New System.Drawing.Size(105, 17)
        Me.lblOverflow.TabIndex = 10
        '
        'lblDefinedCurrency
        '
        Me.lblDefinedCurrency.AutoSize = True
        Me.lblDefinedCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblDefinedCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDefinedCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDefinedCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDefinedCurrency.Location = New System.Drawing.Point(138, 156)
        Me.lblDefinedCurrency.Name = "lblDefinedCurrency"
        Me.lblDefinedCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDefinedCurrency.Size = New System.Drawing.Size(119, 13)
        Me.lblDefinedCurrency.TabIndex = 11
        Me.lblDefinedCurrency.Text = "Defined Currency (XXX)"
        '
        'lblTransactionCurrency
        '
        Me.lblTransactionCurrency.AutoSize = True
        Me.lblTransactionCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblTransactionCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTransactionCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTransactionCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTransactionCurrency.Location = New System.Drawing.Point(338, 156)
        Me.lblTransactionCurrency.Name = "lblTransactionCurrency"
        Me.lblTransactionCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTransactionCurrency.Size = New System.Drawing.Size(138, 13)
        Me.lblTransactionCurrency.TabIndex = 12
        Me.lblTransactionCurrency.Text = "Transaction Currency (XXX)"
        '
        'lblThisPremium
        '
        Me.lblThisPremium.AutoSize = True
        Me.lblThisPremium.BackColor = System.Drawing.SystemColors.Control
        Me.lblThisPremium.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblThisPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblThisPremium.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblThisPremium.Location = New System.Drawing.Point(12, 203)
        Me.lblThisPremium.Name = "lblThisPremium"
        Me.lblThisPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblThisPremium.Size = New System.Drawing.Size(73, 13)
        Me.lblThisPremium.TabIndex = 16
        Me.lblThisPremium.Text = "This Premium:"
        '
        'lblPremium
        '
        Me.lblPremium.AutoSize = True
        Me.lblPremium.BackColor = System.Drawing.SystemColors.Control
        Me.lblPremium.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPremium.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPremium.Location = New System.Drawing.Point(12, 179)
        Me.lblPremium.Name = "lblPremium"
        Me.lblPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPremium.Size = New System.Drawing.Size(86, 13)
        Me.lblPremium.TabIndex = 13
        Me.lblPremium.Text = "Annual Premium:"
        '
        'lblState
        '
        Me.lblState.AutoSize = True
        Me.lblState.BackColor = System.Drawing.SystemColors.Control
        Me.lblState.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblState.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblState.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblState.Location = New System.Drawing.Point(12, 260)
        Me.lblState.Name = "lblState"
        Me.lblState.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblState.Size = New System.Drawing.Size(35, 13)
        Me.lblState.TabIndex = 21
        Me.lblState.Text = "State:"
        '
        'lblCountry
        '
        Me.lblCountry.AutoSize = True
        Me.lblCountry.BackColor = System.Drawing.SystemColors.Control
        Me.lblCountry.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCountry.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCountry.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCountry.Location = New System.Drawing.Point(12, 234)
        Me.lblCountry.Name = "lblCountry"
        Me.lblCountry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCountry.Size = New System.Drawing.Size(46, 13)
        Me.lblCountry.TabIndex = 19
        Me.lblCountry.Text = "Country:"
        '
        'lblOverrideReason
        '
        Me.lblOverrideReason.AutoSize = True
        Me.lblOverrideReason.BackColor = System.Drawing.SystemColors.Control
        Me.lblOverrideReason.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOverrideReason.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOverrideReason.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOverrideReason.Location = New System.Drawing.Point(12, 285)
        Me.lblOverrideReason.Name = "lblOverrideReason"
        Me.lblOverrideReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOverrideReason.Size = New System.Drawing.Size(90, 13)
        Me.lblOverrideReason.TabIndex = 23
        Me.lblOverrideReason.Text = "Override Reason:"
        '
        'lblEarningPattern
        '
        Me.lblEarningPattern.AutoSize = True
        Me.lblEarningPattern.BackColor = System.Drawing.SystemColors.Control
        Me.lblEarningPattern.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEarningPattern.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEarningPattern.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEarningPattern.Location = New System.Drawing.Point(12, 48)
        Me.lblEarningPattern.Name = "lblEarningPattern"
        Me.lblEarningPattern.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEarningPattern.Size = New System.Drawing.Size(83, 13)
        Me.lblEarningPattern.TabIndex = 2
        Me.lblEarningPattern.Text = "Earning Pattern:"
        '
        'cboEarningPattern
        '
        Me.cboEarningPattern.DefaultItemId = 0
        Me.cboEarningPattern.FirstItem = ""
        Me.cboEarningPattern.ItemId = 0
        Me.cboEarningPattern.ListIndex = -1
        Me.cboEarningPattern.Location = New System.Drawing.Point(139, 44)
        Me.cboEarningPattern.Name = "cboEarningPattern"
        Me.cboEarningPattern.PMLookupProductFamily = 1
        Me.cboEarningPattern.SingleItemId = 0
        Me.cboEarningPattern.Size = New System.Drawing.Size(337, 21)
        Me.cboEarningPattern.Sorted = True
        Me.cboEarningPattern.TabIndex = 3
        Me.cboEarningPattern.TableName = "Earning_Pattern"
        Me.cboEarningPattern.ToolTipText = ""
        Me.cboEarningPattern.WhereClause = ""
        '
        'cboState
        '
        Me.cboState.DefaultItemId = 0
        Me.cboState.FirstItem = ""
        Me.cboState.ItemId = 0
        Me.cboState.ListIndex = -1
        Me.cboState.Location = New System.Drawing.Point(138, 256)
        Me.cboState.Name = "cboState"
        Me.cboState.PMLookupProductFamily = 1
        Me.cboState.SingleItemId = 0
        Me.cboState.Size = New System.Drawing.Size(193, 21)
        Me.cboState.Sorted = True
        Me.cboState.TabIndex = 22
        Me.cboState.TableName = "State"
        Me.cboState.ToolTipText = ""
        Me.cboState.WhereClause = "country_id = 0"
        '
        'cboCountry
        '
        Me.cboCountry.DefaultItemId = 0
        Me.cboCountry.FirstItem = ""
        Me.cboCountry.ItemId = 0
        Me.cboCountry.ListIndex = -1
        Me.cboCountry.Location = New System.Drawing.Point(138, 230)
        Me.cboCountry.Name = "cboCountry"
        Me.cboCountry.PMLookupProductFamily = 1
        Me.cboCountry.SingleItemId = 0
        Me.cboCountry.Size = New System.Drawing.Size(193, 21)
        Me.cboCountry.Sorted = True
        Me.cboCountry.TabIndex = 20
        Me.cboCountry.TableName = "Country"
        Me.cboCountry.ToolTipText = ""
        Me.cboCountry.WhereClause = ""
        '
        'txtSumInsured
        '
        Me.txtSumInsured.AcceptsReturn = True
        Me.txtSumInsured.BackColor = System.Drawing.SystemColors.Window
        Me.txtSumInsured.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSumInsured.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSumInsured.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSumInsured.Location = New System.Drawing.Point(138, 128)
        Me.txtSumInsured.MaxLength = 10
        Me.txtSumInsured.Name = "txtSumInsured"
        Me.txtSumInsured.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSumInsured.Size = New System.Drawing.Size(193, 20)
        Me.txtSumInsured.TabIndex = 9
        '
        'txtRate
        '
        Me.txtRate.AcceptsReturn = True
        Me.txtRate.BackColor = System.Drawing.SystemColors.Window
        Me.txtRate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRate.Location = New System.Drawing.Point(138, 98)
        Me.txtRate.MaxLength = 0
        Me.txtRate.Name = "txtRate"
        Me.txtRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRate.Size = New System.Drawing.Size(193, 20)
        Me.txtRate.TabIndex = 7
        '
        'txtPremium
        '
        Me.txtPremium.AcceptsReturn = True
        Me.txtPremium.BackColor = System.Drawing.SystemColors.Window
        Me.txtPremium.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPremium.Enabled = False
        Me.txtPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPremium.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPremium.Location = New System.Drawing.Point(338, 176)
        Me.txtPremium.MaxLength = 0
        Me.txtPremium.Name = "txtPremium"
        Me.txtPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPremium.Size = New System.Drawing.Size(193, 20)
        Me.txtPremium.TabIndex = 15
        '
        'txtRate2
        '
        Me.txtRate2.AcceptsReturn = True
        Me.txtRate2.BackColor = System.Drawing.SystemColors.Window
        Me.txtRate2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRate2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRate2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRate2.Location = New System.Drawing.Point(138, 98)
        Me.txtRate2.MaxLength = 0
        Me.txtRate2.Name = "txtRate2"
        Me.txtRate2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRate2.Size = New System.Drawing.Size(193, 20)
        Me.txtRate2.TabIndex = 4
        '
        'txtRate3
        '
        Me.txtRate3.AcceptsReturn = True
        Me.txtRate3.BackColor = System.Drawing.SystemColors.Window
        Me.txtRate3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRate3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRate3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRate3.Location = New System.Drawing.Point(138, 98)
        Me.txtRate3.MaxLength = 0
        Me.txtRate3.Name = "txtRate3"
        Me.txtRate3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRate3.Size = New System.Drawing.Size(193, 20)
        Me.txtRate3.TabIndex = 5
        '
        'txtPremiumDefCurr
        '
        Me.txtPremiumDefCurr.AcceptsReturn = True
        Me.txtPremiumDefCurr.BackColor = System.Drawing.SystemColors.Window
        Me.txtPremiumDefCurr.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPremiumDefCurr.Enabled = False
        Me.txtPremiumDefCurr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPremiumDefCurr.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPremiumDefCurr.Location = New System.Drawing.Point(138, 176)
        Me.txtPremiumDefCurr.MaxLength = 0
        Me.txtPremiumDefCurr.Name = "txtPremiumDefCurr"
        Me.txtPremiumDefCurr.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPremiumDefCurr.Size = New System.Drawing.Size(193, 20)
        Me.txtPremiumDefCurr.TabIndex = 14
        '
        'txtThisPremiumDefCurr
        '
        Me.txtThisPremiumDefCurr.AcceptsReturn = True
        Me.txtThisPremiumDefCurr.BackColor = System.Drawing.SystemColors.Window
        Me.txtThisPremiumDefCurr.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtThisPremiumDefCurr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThisPremiumDefCurr.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtThisPremiumDefCurr.Location = New System.Drawing.Point(138, 200)
        Me.txtThisPremiumDefCurr.MaxLength = 0
        Me.txtThisPremiumDefCurr.Name = "txtThisPremiumDefCurr"
        Me.txtThisPremiumDefCurr.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtThisPremiumDefCurr.Size = New System.Drawing.Size(193, 20)
        Me.txtThisPremiumDefCurr.TabIndex = 17
        '
        'txtThisPremium
        '
        Me.txtThisPremium.AcceptsReturn = True
        Me.txtThisPremium.BackColor = System.Drawing.SystemColors.Window
        Me.txtThisPremium.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtThisPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThisPremium.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtThisPremium.Location = New System.Drawing.Point(338, 200)
        Me.txtThisPremium.MaxLength = 0
        Me.txtThisPremium.Name = "txtThisPremium"
        Me.txtThisPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtThisPremium.Size = New System.Drawing.Size(193, 20)
        Me.txtThisPremium.TabIndex = 18
        '
        'txtOverrideReason
        '
        Me.txtOverrideReason.AcceptsReturn = True
        Me.txtOverrideReason.BackColor = System.Drawing.SystemColors.Window
        Me.txtOverrideReason.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOverrideReason.Enabled = False
        Me.txtOverrideReason.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOverrideReason.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOverrideReason.Location = New System.Drawing.Point(138, 282)
        Me.txtOverrideReason.MaxLength = 256
        Me.txtOverrideReason.Name = "txtOverrideReason"
        Me.txtOverrideReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOverrideReason.Size = New System.Drawing.Size(337, 20)
        Me.txtOverrideReason.TabIndex = 24
        '
        'frmDetail
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(591, 400)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.StatusBar1)
        Me.Controls.Add(Me.SSTab1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDetail"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Rating Section Type"
        Me.StatusBar1.ResumeLayout(False)
        Me.StatusBar1.PerformLayout()
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me._SSTab1_TabPage0.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region
End Class