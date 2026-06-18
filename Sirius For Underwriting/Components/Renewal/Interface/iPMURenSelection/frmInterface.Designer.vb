<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
        'This call is required by the Windows Form Designer.
        Me.KeyPreview = True
		InitializeComponent()
		InitializeoptPrint()
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
    Public WithEvents cmdRePrint As System.Windows.Forms.Button
    Public WithEvents lblProductCode As System.Windows.Forms.Label
    Public WithEvents lblRenewalDate As System.Windows.Forms.Label
    Public WithEvents lblSource As System.Windows.Forms.Label
    Public WithEvents lblStartDate As System.Windows.Forms.Label
    Public WithEvents lblStartDateDesc As System.Windows.Forms.Label
    Public WithEvents dtEndDate As System.Windows.Forms.DateTimePicker
    Public WithEvents cboProductCode As System.Windows.Forms.ComboBox
    Public WithEvents cboSource As System.Windows.Forms.ComboBox
    Private WithEvents _optPrint_1 As System.Windows.Forms.RadioButton
    Private WithEvents _optPrint_0 As System.Windows.Forms.RadioButton
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents lblPolicyRef As System.Windows.Forms.Label
    Public WithEvents txtPolicyRef As System.Windows.Forms.TextBox
    Public WithEvents cmdSelectPolicy As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public optPrint(1) As System.Windows.Forms.RadioButton
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.cmdRePrint = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblProductCode = New System.Windows.Forms.Label
        Me.lblRenewalDate = New System.Windows.Forms.Label
        Me.lblSource = New System.Windows.Forms.Label
        Me.lblStartDate = New System.Windows.Forms.Label
        Me.lblStartDateDesc = New System.Windows.Forms.Label
        Me.dtEndDate = New System.Windows.Forms.DateTimePicker
        Me.dtStartDate = New System.Windows.Forms.DateTimePicker
        Me.cboProductCode = New System.Windows.Forms.ComboBox
        Me.cboSource = New System.Windows.Forms.ComboBox
        Me._optPrint_1 = New System.Windows.Forms.RadioButton
        Me._optPrint_0 = New System.Windows.Forms.RadioButton
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me.lblPolicyRef = New System.Windows.Forms.Label
        Me.txtPolicyRef = New System.Windows.Forms.TextBox
        Me.cmdSelectPolicy = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.Message = New System.Windows.Forms.ToolStripStatusLabel
        Me.Policy = New System.Windows.Forms.ToolStripStatusLabel
        Me.Count = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.StatusBar1 = New System.Windows.Forms.StatusStrip
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me.StatusBar1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdRePrint
        '
        Me.cmdRePrint.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRePrint.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRePrint.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRePrint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRePrint.Location = New System.Drawing.Point(6, 193)
        Me.cmdRePrint.Name = "cmdRePrint"
        Me.cmdRePrint.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRePrint.Size = New System.Drawing.Size(73, 22)
        Me.cmdRePrint.TabIndex = 11
        Me.cmdRePrint.Text = "RePrint"
        Me.cmdRePrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRePrint.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage1)
        Me.tabMainTab.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(255, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(6, 9)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(517, 181)
        Me.tabMainTab.TabIndex = 4
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblProductCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblRenewalDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblSource)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblStartDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblStartDateDesc)
        Me._tabMainTab_TabPage0.Controls.Add(Me.dtEndDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.dtStartDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboProductCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboSource)
        Me._tabMainTab_TabPage0.Controls.Add(Me._optPrint_1)
        Me._tabMainTab_TabPage0.Controls.Add(Me._optPrint_0)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(509, 155)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - By Date/Product"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'lblProductCode
        '
        Me.lblProductCode.AutoSize = True
        Me.lblProductCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblProductCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProductCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProductCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProductCode.Location = New System.Drawing.Point(15, 70)
        Me.lblProductCode.Name = "lblProductCode"
        Me.lblProductCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProductCode.Size = New System.Drawing.Size(97, 13)
        Me.lblProductCode.TabIndex = 5
        Me.lblProductCode.Text = "Product Code:"
        '
        'lblRenewalDate
        '
        Me.lblRenewalDate.AutoSize = True
        Me.lblRenewalDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblRenewalDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRenewalDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRenewalDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRenewalDate.Location = New System.Drawing.Point(15, 45)
        Me.lblRenewalDate.Name = "lblRenewalDate"
        Me.lblRenewalDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRenewalDate.Size = New System.Drawing.Size(69, 13)
        Me.lblRenewalDate.TabIndex = 6
        Me.lblRenewalDate.Text = "End Date:"
        Me.lblRenewalDate.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblSource
        '
        Me.lblSource.AutoSize = True
        Me.lblSource.BackColor = System.Drawing.SystemColors.Control
        Me.lblSource.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSource.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSource.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSource.Location = New System.Drawing.Point(15, 95)
        Me.lblSource.Name = "lblSource"
        Me.lblSource.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSource.Size = New System.Drawing.Size(96, 13)
        Me.lblSource.TabIndex = 13
        Me.lblSource.Text = "Branch Code :"
        '
        'lblStartDate
        '
        Me.lblStartDate.AutoSize = True
        Me.lblStartDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblStartDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStartDate.Enabled = False
        Me.lblStartDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStartDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStartDate.Location = New System.Drawing.Point(15, 19)
        Me.lblStartDate.Name = "lblStartDate"
        Me.lblStartDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStartDate.Size = New System.Drawing.Size(77, 13)
        Me.lblStartDate.TabIndex = 15
        Me.lblStartDate.Text = "Start Date:"
        Me.lblStartDate.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblStartDateDesc
        '
        Me.lblStartDateDesc.AutoSize = True
        Me.lblStartDateDesc.BackColor = System.Drawing.SystemColors.Control
        Me.lblStartDateDesc.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStartDateDesc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStartDateDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStartDateDesc.Location = New System.Drawing.Point(298, 45)
        Me.lblStartDateDesc.Name = "lblStartDateDesc"
        Me.lblStartDateDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStartDateDesc.Size = New System.Drawing.Size(142, 13)
        Me.lblStartDateDesc.TabIndex = 17
        Me.lblStartDateDesc.Text = "(using product renewal days)"
        '
        'dtEndDate
        '
        Me.dtEndDate.CustomFormat = ""
        Me.dtEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtEndDate.Location = New System.Drawing.Point(128, 40)
        Me.dtEndDate.Name = "dtEndDate"
        Me.dtEndDate.Size = New System.Drawing.Size(156, 20)
        Me.dtEndDate.TabIndex = 16
        '
        'dtStartDate
        '
        Me.dtStartDate.Checked = False
        Me.dtStartDate.CustomFormat = ""
        Me.dtStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtStartDate.Location = New System.Drawing.Point(128, 14)
        Me.dtStartDate.Name = "dtStartDate"
        Me.dtStartDate.ShowCheckBox = True
        Me.dtStartDate.Size = New System.Drawing.Size(156, 20)
        Me.dtStartDate.TabIndex = 14
        '
        'cboProductCode
        '
        Me.cboProductCode.BackColor = System.Drawing.SystemColors.Window
        Me.cboProductCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboProductCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboProductCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboProductCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboProductCode.Location = New System.Drawing.Point(128, 66)
        Me.cboProductCode.Name = "cboProductCode"
        Me.cboProductCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboProductCode.Size = New System.Drawing.Size(357, 21)
        Me.cboProductCode.TabIndex = 0
        '
        'cboSource
        '
        Me.cboSource.BackColor = System.Drawing.SystemColors.Window
        Me.cboSource.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSource.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSource.Location = New System.Drawing.Point(128, 91)
        Me.cboSource.Name = "cboSource"
        Me.cboSource.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSource.Size = New System.Drawing.Size(357, 21)
        Me.cboSource.TabIndex = 12
        '
        '_optPrint_1
        '
        Me._optPrint_1.BackColor = System.Drawing.SystemColors.Control
        Me._optPrint_1.Checked = True
        Me._optPrint_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optPrint_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optPrint_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optPrint_1.Location = New System.Drawing.Point(224, 123)
        Me._optPrint_1.Name = "_optPrint_1"
        Me._optPrint_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optPrint_1.Size = New System.Drawing.Size(79, 15)
        Me._optPrint_1.TabIndex = 18
        Me._optPrint_1.TabStop = True
        Me._optPrint_1.Text = "Preview"
        Me._optPrint_1.UseVisualStyleBackColor = False
        '
        '_optPrint_0
        '
        Me._optPrint_0.BackColor = System.Drawing.SystemColors.Control
        Me._optPrint_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optPrint_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optPrint_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optPrint_0.Location = New System.Drawing.Point(128, 123)
        Me._optPrint_0.Name = "_optPrint_0"
        Me._optPrint_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optPrint_0.Size = New System.Drawing.Size(79, 15)
        Me._optPrint_0.TabIndex = 19
        Me._optPrint_0.TabStop = True
        Me._optPrint_0.Text = "Print"
        Me._optPrint_0.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblPolicyRef)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtPolicyRef)
        Me._tabMainTab_TabPage1.Controls.Add(Me.cmdSelectPolicy)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(509, 155)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "2 - By Policy"
        Me._tabMainTab_TabPage1.UseVisualStyleBackColor = True
        '
        'lblPolicyRef
        '
        Me.lblPolicyRef.AutoSize = True
        Me.lblPolicyRef.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyRef.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyRef.Location = New System.Drawing.Point(15, 19)
        Me.lblPolicyRef.Name = "lblPolicyRef"
        Me.lblPolicyRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyRef.Size = New System.Drawing.Size(79, 13)
        Me.lblPolicyRef.TabIndex = 9
        Me.lblPolicyRef.Text = "Policy Ref :"
        Me.lblPolicyRef.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtPolicyRef
        '
        Me.txtPolicyRef.AcceptsReturn = True
        Me.txtPolicyRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolicyRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicyRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicyRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicyRef.Location = New System.Drawing.Point(128, 14)
        Me.txtPolicyRef.MaxLength = 0
        Me.txtPolicyRef.Name = "txtPolicyRef"
        Me.txtPolicyRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicyRef.Size = New System.Drawing.Size(149, 20)
        Me.txtPolicyRef.TabIndex = 8
        '
        'cmdSelectPolicy
        '
        Me.cmdSelectPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelectPolicy.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelectPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSelectPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelectPolicy.Location = New System.Drawing.Point(278, 14)
        Me.cmdSelectPolicy.Name = "cmdSelectPolicy"
        Me.cmdSelectPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelectPolicy.Size = New System.Drawing.Size(22, 19)
        Me.cmdSelectPolicy.TabIndex = 10
        Me.cmdSelectPolicy.Text = "..."
        Me.cmdSelectPolicy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSelectPolicy.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(446, 193)
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
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(366, 193)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 2
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
        Me.cmdOK.Location = New System.Drawing.Point(286, 193)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 1
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'Message
        '
        Me.Message.AutoSize = False
        Me.Message.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.Message.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.Message.DoubleClickEnabled = True
        Me.Message.Margin = New System.Windows.Forms.Padding(0)
        Me.Message.Name = "Message"
        Me.Message.Size = New System.Drawing.Size(230, 22)
        Me.Message.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Policy
        '
        Me.Policy.AutoSize = False
        Me.Policy.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.Policy.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.Policy.DoubleClickEnabled = True
        Me.Policy.Margin = New System.Windows.Forms.Padding(0)
        Me.Policy.Name = "Policy"
        Me.Policy.Size = New System.Drawing.Size(150, 22)
        Me.Policy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Count
        '
        Me.Count.AutoSize = False
        Me.Count.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.Count.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.Count.DoubleClickEnabled = True
        Me.Count.Margin = New System.Windows.Forms.Padding(0)
        Me.Count.Name = "Count"
        Me.Count.Size = New System.Drawing.Size(81, 22)
        Me.Count.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(111, 13)
        Me.ToolStripStatusLabel1.Text = "ToolStripStatusLabel1"
        '
        'StatusBar1
        '
        Me.StatusBar1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusBar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Message, Me.Policy, Me.Count, Me.ToolStripStatusLabel1})
        Me.StatusBar1.Location = New System.Drawing.Point(0, 222)
        Me.StatusBar1.Name = "StatusBar1"
        Me.StatusBar1.ShowItemToolTips = True
        Me.StatusBar1.Size = New System.Drawing.Size(530, 22)
        Me.StatusBar1.TabIndex = 7
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(530, 244)
        Me.Controls.Add(Me.cmdRePrint)
        Me.Controls.Add(Me.StatusBar1)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Renewal Selection"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me._tabMainTab_TabPage1.PerformLayout()
        Me.StatusBar1.ResumeLayout(False)
        Me.StatusBar1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializeoptPrint()
        Me.optPrint(0) = _optPrint_0
        Me.optPrint(1) = _optPrint_1
    End Sub
    Public WithEvents dtStartDate As System.Windows.Forms.DateTimePicker
    Public WithEvents Message As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents Policy As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents Count As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents StatusBar1 As System.Windows.Forms.StatusStrip
#End Region
End Class