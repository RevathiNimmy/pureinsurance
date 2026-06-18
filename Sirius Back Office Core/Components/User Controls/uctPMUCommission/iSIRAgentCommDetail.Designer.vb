Imports PMLookupControl

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDetail
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
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents lblAgant As System.Windows.Forms.Label
    Public WithEvents lblAgentType As System.Windows.Forms.Label
    Public WithEvents lblPremium As System.Windows.Forms.Label
    Public WithEvents lblCommissionBand As System.Windows.Forms.Label
    Public WithEvents lblRiskType As System.Windows.Forms.Label
    Public WithEvents lblCommissionRate As System.Windows.Forms.Label
    Public WithEvents lblCommissionValue As System.Windows.Forms.Label
    Public WithEvents lblTaxGroup As System.Windows.Forms.Label
    Public WithEvents lblTaxValue As System.Windows.Forms.Label
    Public WithEvents lblOverrideReason As System.Windows.Forms.Label
    Public WithEvents cboTaxGroup As PMLookupControl.cboPMLookup
    Public WithEvents cboAgent As System.Windows.Forms.ComboBox
    Public WithEvents txtPremium As System.Windows.Forms.TextBox
    Public WithEvents txtCommissionrate As System.Windows.Forms.TextBox
    Public WithEvents txtCommissionvalue As System.Windows.Forms.TextBox
    Public WithEvents cboRiskType As PMLookupControl.cboPMLookup
    Public WithEvents cboCommissionBand As PMLookupControl.cboPMLookup
    Public WithEvents cboPartyAgentType As PMLookupControl.cboPMLookup
    Public WithEvents txtTaxValue As System.Windows.Forms.TextBox
    Public WithEvents txtOverrideReason As System.Windows.Forms.TextBox
    Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents SSTab1 As System.Windows.Forms.TabControl
    Private WithEvents _StatusBar1_Panel1 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents StatusBar1 As System.Windows.Forms.StatusStrip
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDetail))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblAgant = New System.Windows.Forms.Label
        Me.lblAgentType = New System.Windows.Forms.Label
        Me.lblPremium = New System.Windows.Forms.Label
        Me.lblCommissionBand = New System.Windows.Forms.Label
        Me.lblRiskType = New System.Windows.Forms.Label
        Me.lblCommissionRate = New System.Windows.Forms.Label
        Me.lblCommissionValue = New System.Windows.Forms.Label
        Me.lblTaxGroup = New System.Windows.Forms.Label
        Me.lblTaxValue = New System.Windows.Forms.Label
        Me.lblOverrideReason = New System.Windows.Forms.Label
        Me.cboTaxGroup = New PMLookupControl.cboPMLookup
        Me.cboAgent = New System.Windows.Forms.ComboBox
        Me.txtPremium = New System.Windows.Forms.TextBox
        Me.txtCommissionrate = New System.Windows.Forms.TextBox
        Me.txtCommissionvalue = New System.Windows.Forms.TextBox
        Me.cboRiskType = New PMLookupControl.cboPMLookup
        Me.cboCommissionBand = New PMLookupControl.cboPMLookup
        Me.cboPartyAgentType = New PMLookupControl.cboPMLookup
        Me.txtTaxValue = New System.Windows.Forms.TextBox
        Me.txtOverrideReason = New System.Windows.Forms.TextBox
        Me.StatusBar1 = New System.Windows.Forms.StatusStrip
        Me._StatusBar1_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me._StatusBar1_Panel2 = New System.Windows.Forms.ToolStripStatusLabel
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me.StatusBar1.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(402, 262)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 23)
        Me.cmdOK.TabIndex = 19
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(480, 262)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 23)
        Me.cmdCancel.TabIndex = 20
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(544, 18)
        Me.SSTab1.Location = New System.Drawing.Point(8, 8)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(549, 249)
        Me.SSTab1.TabIndex = 0
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.lblAgant)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblAgentType)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblPremium)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblCommissionBand)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblRiskType)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblCommissionRate)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblCommissionValue)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblTaxGroup)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblTaxValue)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblOverrideReason)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboTaxGroup)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboAgent)
        Me._SSTab1_TabPage0.Controls.Add(Me.txtPremium)
        Me._SSTab1_TabPage0.Controls.Add(Me.txtCommissionrate)
        Me._SSTab1_TabPage0.Controls.Add(Me.txtCommissionvalue)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboRiskType)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboCommissionBand)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboPartyAgentType)
        Me._SSTab1_TabPage0.Controls.Add(Me.txtTaxValue)
        Me._SSTab1_TabPage0.Controls.Add(Me.txtOverrideReason)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(541, 223)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "1 - General"
        Me._SSTab1_TabPage0.UseVisualStyleBackColor = True
        '
        'lblAgant
        '
        Me.lblAgant.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgant.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgant.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgant.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgant.Location = New System.Drawing.Point(8, 31)
        Me.lblAgant.Name = "lblAgant"
        Me.lblAgant.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgant.Size = New System.Drawing.Size(70, 16)
        Me.lblAgant.TabIndex = 2
        Me.lblAgant.Text = "Agent:"
        '
        'lblAgentType
        '
        Me.lblAgentType.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgentType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgentType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgentType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgentType.Location = New System.Drawing.Point(8, 61)
        Me.lblAgentType.Name = "lblAgentType"
        Me.lblAgentType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgentType.Size = New System.Drawing.Size(61, 18)
        Me.lblAgentType.TabIndex = 6
        Me.lblAgentType.Text = "Agent type:"
        '
        'lblPremium
        '
        Me.lblPremium.BackColor = System.Drawing.SystemColors.Control
        Me.lblPremium.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPremium.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPremium.Location = New System.Drawing.Point(8, 95)
        Me.lblPremium.Name = "lblPremium"
        Me.lblPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPremium.Size = New System.Drawing.Size(105, 17)
        Me.lblPremium.TabIndex = 10
        Me.lblPremium.Text = "Premium:"
        '
        'lblCommissionBand
        '
        Me.lblCommissionBand.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommissionBand.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommissionBand.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCommissionBand.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommissionBand.Location = New System.Drawing.Point(270, 63)
        Me.lblCommissionBand.Name = "lblCommissionBand"
        Me.lblCommissionBand.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommissionBand.Size = New System.Drawing.Size(106, 16)
        Me.lblCommissionBand.TabIndex = 8
        Me.lblCommissionBand.Text = "Commission band:"
        '
        'lblRiskType
        '
        Me.lblRiskType.BackColor = System.Drawing.SystemColors.Control
        Me.lblRiskType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRiskType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRiskType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRiskType.Location = New System.Drawing.Point(270, 29)
        Me.lblRiskType.Name = "lblRiskType"
        Me.lblRiskType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRiskType.Size = New System.Drawing.Size(85, 18)
        Me.lblRiskType.TabIndex = 4
        Me.lblRiskType.Text = "Risk type:"
        '
        'lblCommissionRate
        '
        Me.lblCommissionRate.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommissionRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommissionRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCommissionRate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommissionRate.Location = New System.Drawing.Point(8, 127)
        Me.lblCommissionRate.Name = "lblCommissionRate"
        Me.lblCommissionRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommissionRate.Size = New System.Drawing.Size(105, 17)
        Me.lblCommissionRate.TabIndex = 12
        Me.lblCommissionRate.Text = "Commission %:"
        '
        'lblCommissionValue
        '
        Me.lblCommissionValue.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommissionValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommissionValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCommissionValue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommissionValue.Location = New System.Drawing.Point(270, 127)
        Me.lblCommissionValue.Name = "lblCommissionValue"
        Me.lblCommissionValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommissionValue.Size = New System.Drawing.Size(113, 17)
        Me.lblCommissionValue.TabIndex = 14
        Me.lblCommissionValue.Text = "Commission value:"
        '
        'lblTaxGroup
        '
        Me.lblTaxGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxGroup.Location = New System.Drawing.Point(10, 159)
        Me.lblTaxGroup.Name = "lblTaxGroup"
        Me.lblTaxGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxGroup.Size = New System.Drawing.Size(68, 17)
        Me.lblTaxGroup.TabIndex = 16
        Me.lblTaxGroup.Text = "Tax Group:"
        '
        'lblTaxValue
        '
        Me.lblTaxValue.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxValue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxValue.Location = New System.Drawing.Point(270, 159)
        Me.lblTaxValue.Name = "lblTaxValue"
        Me.lblTaxValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxValue.Size = New System.Drawing.Size(113, 17)
        Me.lblTaxValue.TabIndex = 18
        Me.lblTaxValue.Text = "Tax value:"
        '
        'lblOverrideReason
        '
        Me.lblOverrideReason.AutoSize = True
        Me.lblOverrideReason.BackColor = System.Drawing.SystemColors.Control
        Me.lblOverrideReason.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOverrideReason.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOverrideReason.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOverrideReason.Location = New System.Drawing.Point(10, 189)
        Me.lblOverrideReason.Name = "lblOverrideReason"
        Me.lblOverrideReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOverrideReason.Size = New System.Drawing.Size(90, 13)
        Me.lblOverrideReason.TabIndex = 23
        Me.lblOverrideReason.Text = "Override Reason:"
        '
        'cboTaxGroup
        '
        Me.cboTaxGroup.DefaultItemId = 0
        Me.cboTaxGroup.Enabled = False
        Me.cboTaxGroup.ItemId = 0
        Me.cboTaxGroup.ListIndex = -1
        Me.cboTaxGroup.Location = New System.Drawing.Point(112, 156)
        Me.cboTaxGroup.Name = "cboTaxGroup"
        Me.cboTaxGroup.PMLookupProductFamily = 1
        Me.cboTaxGroup.SingleItemId = 0
        Me.cboTaxGroup.Size = New System.Drawing.Size(153, 21)
        Me.cboTaxGroup.Sorted = True
        Me.cboTaxGroup.TabIndex = 15
        Me.cboTaxGroup.TableName = "Tax_Group"
        Me.cboTaxGroup.ToolTipText = ""
        Me.cboTaxGroup.WhereClause = resources.GetString("cboTaxGroup.WhereClause")
        '
        'cboAgent
        '
        Me.cboAgent.BackColor = System.Drawing.SystemColors.Window
        Me.cboAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboAgent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAgent.Enabled = False
        Me.cboAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboAgent.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboAgent.Location = New System.Drawing.Point(112, 26)
        Me.cboAgent.Name = "cboAgent"
        Me.cboAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboAgent.Size = New System.Drawing.Size(153, 21)
        Me.cboAgent.TabIndex = 1
        '
        'txtPremium
        '
        Me.txtPremium.AcceptsReturn = True
        Me.txtPremium.BackColor = System.Drawing.SystemColors.Window
        Me.txtPremium.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPremium.Enabled = False
        Me.txtPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPremium.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPremium.Location = New System.Drawing.Point(112, 92)
        Me.txtPremium.MaxLength = 0
        Me.txtPremium.Name = "txtPremium"
        Me.txtPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPremium.Size = New System.Drawing.Size(153, 20)
        Me.txtPremium.TabIndex = 9
        '
        'txtCommissionrate
        '
        Me.txtCommissionrate.AcceptsReturn = True
        Me.txtCommissionrate.BackColor = System.Drawing.SystemColors.Window
        Me.txtCommissionrate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCommissionrate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCommissionrate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCommissionrate.Location = New System.Drawing.Point(112, 124)
        Me.txtCommissionrate.MaxLength = 14
        Me.txtCommissionrate.Name = "txtCommissionrate"
        Me.txtCommissionrate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCommissionrate.Size = New System.Drawing.Size(153, 20)
        Me.txtCommissionrate.TabIndex = 11
        '
        'txtCommissionvalue
        '
        Me.txtCommissionvalue.AcceptsReturn = True
        Me.txtCommissionvalue.BackColor = System.Drawing.SystemColors.Window
        Me.txtCommissionvalue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCommissionvalue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCommissionvalue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCommissionvalue.Location = New System.Drawing.Point(382, 124)
        Me.txtCommissionvalue.MaxLength = 0
        Me.txtCommissionvalue.Name = "txtCommissionvalue"
        Me.txtCommissionvalue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCommissionvalue.Size = New System.Drawing.Size(153, 20)
        Me.txtCommissionvalue.TabIndex = 13
        '
        'cboRiskType
        '
        Me.cboRiskType.DefaultItemId = 0
        Me.cboRiskType.Enabled = False
        Me.cboRiskType.ItemId = 0
        Me.cboRiskType.ListIndex = -1
        Me.cboRiskType.Location = New System.Drawing.Point(382, 26)
        Me.cboRiskType.Name = "cboRiskType"
        Me.cboRiskType.PMLookupProductFamily = 1
        Me.cboRiskType.SingleItemId = 0
        Me.cboRiskType.Size = New System.Drawing.Size(153, 21)
        Me.cboRiskType.Sorted = True
        Me.cboRiskType.TabIndex = 3
        Me.cboRiskType.TableName = "Risk_Type"
        Me.cboRiskType.ToolTipText = ""
        Me.cboRiskType.WhereClause = ""
        '
        'cboCommissionBand
        '
        Me.cboCommissionBand.DefaultItemId = 0
        Me.cboCommissionBand.Enabled = False
        Me.cboCommissionBand.ItemId = 0
        Me.cboCommissionBand.ListIndex = -1
        Me.cboCommissionBand.Location = New System.Drawing.Point(382, 58)
        Me.cboCommissionBand.Name = "cboCommissionBand"
        Me.cboCommissionBand.PMLookupProductFamily = 1
        Me.cboCommissionBand.SingleItemId = 0
        Me.cboCommissionBand.Size = New System.Drawing.Size(153, 21)
        Me.cboCommissionBand.Sorted = True
        Me.cboCommissionBand.TabIndex = 7
        Me.cboCommissionBand.TableName = "Commission_Band"
        Me.cboCommissionBand.ToolTipText = ""
        Me.cboCommissionBand.WhereClause = ""
        '
        'cboPartyAgentType
        '
        Me.cboPartyAgentType.DefaultItemId = 0
        Me.cboPartyAgentType.Enabled = False
        Me.cboPartyAgentType.ItemId = 0
        Me.cboPartyAgentType.ListIndex = -1
        Me.cboPartyAgentType.Location = New System.Drawing.Point(112, 58)
        Me.cboPartyAgentType.Name = "cboPartyAgentType"
        Me.cboPartyAgentType.PMLookupProductFamily = 1
        Me.cboPartyAgentType.SingleItemId = 0
        Me.cboPartyAgentType.Size = New System.Drawing.Size(153, 21)
        Me.cboPartyAgentType.Sorted = True
        Me.cboPartyAgentType.TabIndex = 5
        Me.cboPartyAgentType.TableName = "Party_Agent_Type"
        Me.cboPartyAgentType.ToolTipText = ""
        Me.cboPartyAgentType.WhereClause = ""
        '
        'txtTaxValue
        '
        Me.txtTaxValue.AcceptsReturn = True
        Me.txtTaxValue.BackColor = System.Drawing.SystemColors.Window
        Me.txtTaxValue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTaxValue.Enabled = False
        Me.txtTaxValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTaxValue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTaxValue.Location = New System.Drawing.Point(382, 156)
        Me.txtTaxValue.MaxLength = 0
        Me.txtTaxValue.Name = "txtTaxValue"
        Me.txtTaxValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTaxValue.Size = New System.Drawing.Size(153, 20)
        Me.txtTaxValue.TabIndex = 17
        '
        'txtOverrideReason
        '
        Me.txtOverrideReason.AcceptsReturn = True
        Me.txtOverrideReason.BackColor = System.Drawing.SystemColors.Window
        Me.txtOverrideReason.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOverrideReason.Enabled = False
        Me.txtOverrideReason.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOverrideReason.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOverrideReason.Location = New System.Drawing.Point(112, 186)
        Me.txtOverrideReason.MaxLength = 256
        Me.txtOverrideReason.Name = "txtOverrideReason"
        Me.txtOverrideReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOverrideReason.Size = New System.Drawing.Size(423, 20)
        Me.txtOverrideReason.TabIndex = 22
        '
        'StatusBar1
        '
        Me.StatusBar1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusBar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._StatusBar1_Panel1, Me._StatusBar1_Panel2})
        Me.StatusBar1.Location = New System.Drawing.Point(0, 293)
        Me.StatusBar1.Name = "StatusBar1"
        Me.StatusBar1.ShowItemToolTips = True
        Me.StatusBar1.Size = New System.Drawing.Size(559, 22)
        Me.StatusBar1.SizingGrip = False
        Me.StatusBar1.TabIndex = 21
        '
        '_StatusBar1_Panel1
        '
        Me._StatusBar1_Panel1.AutoSize = False
        Me._StatusBar1_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._StatusBar1_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._StatusBar1_Panel1.DoubleClickEnabled = True
        Me._StatusBar1_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._StatusBar1_Panel1.Name = "_StatusBar1_Panel1"
        Me._StatusBar1_Panel1.Size = New System.Drawing.Size(280, 22)
        Me._StatusBar1_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_StatusBar1_Panel2
        '
        Me._StatusBar1_Panel2.AutoSize = False
        Me._StatusBar1_Panel2.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._StatusBar1_Panel2.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._StatusBar1_Panel2.DoubleClickEnabled = True
        Me._StatusBar1_Panel2.Margin = New System.Windows.Forms.Padding(0)
        Me._StatusBar1_Panel2.Name = "_StatusBar1_Panel2"
        Me._StatusBar1_Panel2.Size = New System.Drawing.Size(280, 22)
        Me._StatusBar1_Panel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'frmDetail
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(559, 315)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.SSTab1)
        Me.Controls.Add(Me.StatusBar1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmDetail"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Agent Commission Details"
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me._SSTab1_TabPage0.PerformLayout()
        Me.StatusBar1.ResumeLayout(False)
        Me.StatusBar1.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()
        FillCombo()
    End Sub
    Friend WithEvents _StatusBar1_Panel2 As System.Windows.Forms.ToolStripStatusLabel
#End Region
End Class