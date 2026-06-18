<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDetail
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializeoptGenVal()
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
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents chkIsReadOnly As System.Windows.Forms.CheckBox
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents txtScheme As System.Windows.Forms.TextBox
	Public WithEvents cboBusinessType As PMLookupControl.cboPMLookup
	Public WithEvents cboPartyType As PMLookupControl.cboPMLookup
	Public WithEvents txtSchemeCode As System.Windows.Forms.TextBox
	Public WithEvents lblPartyType As System.Windows.Forms.Label
	Public WithEvents lblSchemeCode As System.Windows.Forms.Label
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents lblScheme As System.Windows.Forms.Label
	Public WithEvents lblBusinessType As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents chkResetNumber As System.Windows.Forms.CheckBox
	Public WithEvents chkIsResetDaily As System.Windows.Forms.CheckBox
	Public WithEvents txtFixedCode As System.Windows.Forms.TextBox
	Public WithEvents txtMask As System.Windows.Forms.TextBox
	Public WithEvents txtStep As System.Windows.Forms.TextBox
	Public WithEvents txtHighestNo As System.Windows.Forms.TextBox
	Public WithEvents txtNextNo As System.Windows.Forms.TextBox
	Public WithEvents lblFixedCode As System.Windows.Forms.Label
	Public WithEvents lblMask As System.Windows.Forms.Label
	Public WithEvents lblStep As System.Windows.Forms.Label
	Public WithEvents lblHighestNo As System.Windows.Forms.Label
	Public WithEvents lblNextNo As System.Windows.Forms.Label
	Public WithEvents Frame2 As System.Windows.Forms.GroupBox
	Public WithEvents chkReuse As System.Windows.Forms.CheckBox
	Private WithEvents _optGenVal_1 As System.Windows.Forms.RadioButton
	Private WithEvents _optGenVal_0 As System.Windows.Forms.RadioButton
	Public WithEvents Frame3 As System.Windows.Forms.GroupBox
	Public WithEvents txtAllocateNext As System.Windows.Forms.TextBox
	Public WithEvents lblAllocateNext As System.Windows.Forms.Label
	Public WithEvents Frame5 As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public optGenVal(1) As System.Windows.Forms.RadioButton
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.cmdNavigate = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.chkIsReadOnly = New System.Windows.Forms.CheckBox
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.txtScheme = New System.Windows.Forms.TextBox
        Me.cboBusinessType = New PMLookupControl.cboPMLookup
        Me.cboPartyType = New PMLookupControl.cboPMLookup
        Me.txtSchemeCode = New System.Windows.Forms.TextBox
        Me.lblPartyType = New System.Windows.Forms.Label
        Me.lblSchemeCode = New System.Windows.Forms.Label
        Me.lblDescription = New System.Windows.Forms.Label
        Me.lblScheme = New System.Windows.Forms.Label
        Me.lblBusinessType = New System.Windows.Forms.Label
        Me.Frame2 = New System.Windows.Forms.GroupBox
        Me.chkResetNumber = New System.Windows.Forms.CheckBox
        Me.chkIsResetDaily = New System.Windows.Forms.CheckBox
        Me.txtFixedCode = New System.Windows.Forms.TextBox
        Me.txtMask = New System.Windows.Forms.TextBox
        Me.txtStep = New System.Windows.Forms.TextBox
        Me.txtHighestNo = New System.Windows.Forms.TextBox
        Me.txtNextNo = New System.Windows.Forms.TextBox
        Me.lblFixedCode = New System.Windows.Forms.Label
        Me.lblMask = New System.Windows.Forms.Label
        Me.lblStep = New System.Windows.Forms.Label
        Me.lblHighestNo = New System.Windows.Forms.Label
        Me.lblNextNo = New System.Windows.Forms.Label
        Me.Frame3 = New System.Windows.Forms.GroupBox
        Me.chkReuse = New System.Windows.Forms.CheckBox
        Me._optGenVal_1 = New System.Windows.Forms.RadioButton
        Me._optGenVal_0 = New System.Windows.Forms.RadioButton
        Me.Frame5 = New System.Windows.Forms.GroupBox
        Me.txtAllocateNext = New System.Windows.Forms.TextBox
        Me.lblAllocateNext = New System.Windows.Forms.Label
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me.Frame2.SuspendLayout()
        Me.Frame3.SuspendLayout()
        Me.Frame5.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(4, 361)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 0
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(394, 361)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
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
        Me.cmdCancel.Location = New System.Drawing.Point(476, 361)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 20
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(556, 361)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 20
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(125, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(4, 2)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(635, 353)
        Me.tabMainTab.TabIndex = 20
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.Frame1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Frame2)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Frame3)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Frame5)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(627, 327)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Scheme Details"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.chkIsReadOnly)
        Me.Frame1.Controls.Add(Me.txtDescription)
        Me.Frame1.Controls.Add(Me.txtScheme)
        Me.Frame1.Controls.Add(Me.cboBusinessType)
        Me.Frame1.Controls.Add(Me.cboPartyType)
        Me.Frame1.Controls.Add(Me.txtSchemeCode)
        Me.Frame1.Controls.Add(Me.lblPartyType)
        Me.Frame1.Controls.Add(Me.lblSchemeCode)
        Me.Frame1.Controls.Add(Me.lblDescription)
        Me.Frame1.Controls.Add(Me.lblScheme)
        Me.Frame1.Controls.Add(Me.lblBusinessType)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(7, 56)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(613, 118)
        Me.Frame1.TabIndex = 4
        Me.Frame1.TabStop = False
        '
        'chkIsReadOnly
        '
        Me.chkIsReadOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsReadOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsReadOnly.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsReadOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsReadOnly.Location = New System.Drawing.Point(292, 40)
        Me.chkIsReadOnly.Name = "chkIsReadOnly"
        Me.chkIsReadOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsReadOnly.Size = New System.Drawing.Size(149, 23)
        Me.chkIsReadOnly.TabIndex = 8
        Me.chkIsReadOnly.Text = "Is Read Only"
        Me.chkIsReadOnly.UseVisualStyleBackColor = False
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(120, 88)
        Me.txtDescription.MaxLength = 100
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(387, 20)
        Me.txtDescription.TabIndex = 10
        '
        'txtScheme
        '
        Me.txtScheme.AcceptsReturn = True
        Me.txtScheme.BackColor = System.Drawing.SystemColors.Window
        Me.txtScheme.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtScheme.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtScheme.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtScheme.Location = New System.Drawing.Point(546, 14)
        Me.txtScheme.MaxLength = 3
        Me.txtScheme.Name = "txtScheme"
        Me.txtScheme.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtScheme.Size = New System.Drawing.Size(47, 20)
        Me.txtScheme.TabIndex = 6
        '
        'cboBusinessType
        '
        Me.cboBusinessType.DefaultItemId = 0
        Me.cboBusinessType.FirstItem = ""
        Me.cboBusinessType.ItemId = 0
        Me.cboBusinessType.ListIndex = -1
        Me.cboBusinessType.Location = New System.Drawing.Point(120, 14)
        Me.cboBusinessType.Name = "cboBusinessType"
        Me.cboBusinessType.PMLookupProductFamily = 9
        Me.cboBusinessType.SingleItemId = 0
        Me.cboBusinessType.Size = New System.Drawing.Size(151, 21)
        Me.cboBusinessType.Sorted = True
        Me.cboBusinessType.TabIndex = 5
        Me.cboBusinessType.TableName = "numbering_scheme_type"
        Me.cboBusinessType.ToolTipText = ""
        Me.cboBusinessType.WhereClause = "code <> 'CLIENT'"
        '
        'cboPartyType
        '
        Me.cboPartyType.DefaultItemId = 0
        Me.cboPartyType.FirstItem = ""
        Me.cboPartyType.ItemId = 0
        Me.cboPartyType.ListIndex = -1
        Me.cboPartyType.Location = New System.Drawing.Point(120, 41)
        Me.cboPartyType.Name = "cboPartyType"
        Me.cboPartyType.PMLookupProductFamily = 0
        Me.cboPartyType.SingleItemId = 0
        Me.cboPartyType.Size = New System.Drawing.Size(151, 21)
        Me.cboPartyType.Sorted = True
        Me.cboPartyType.TabIndex = 7
        Me.cboPartyType.TableName = "Party_Type"
        Me.cboPartyType.ToolTipText = ""
        Me.cboPartyType.WhereClause = "is_on_numbering_scheme=1"
        '
        'txtSchemeCode
        '
        Me.txtSchemeCode.AcceptsReturn = True
        Me.txtSchemeCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtSchemeCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSchemeCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSchemeCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSchemeCode.Location = New System.Drawing.Point(120, 64)
        Me.txtSchemeCode.MaxLength = 10
        Me.txtSchemeCode.Name = "txtSchemeCode"
        Me.txtSchemeCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSchemeCode.Size = New System.Drawing.Size(151, 20)
        Me.txtSchemeCode.TabIndex = 9
        '
        'lblPartyType
        '
        Me.lblPartyType.BackColor = System.Drawing.SystemColors.Control
        Me.lblPartyType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPartyType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartyType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPartyType.Location = New System.Drawing.Point(12, 43)
        Me.lblPartyType.Name = "lblPartyType"
        Me.lblPartyType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPartyType.Size = New System.Drawing.Size(119, 17)
        Me.lblPartyType.TabIndex = 35
        Me.lblPartyType.Text = "Party Type:"
        '
        'lblSchemeCode
        '
        Me.lblSchemeCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblSchemeCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSchemeCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSchemeCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSchemeCode.Location = New System.Drawing.Point(12, 68)
        Me.lblSchemeCode.Name = "lblSchemeCode"
        Me.lblSchemeCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSchemeCode.Size = New System.Drawing.Size(119, 17)
        Me.lblSchemeCode.TabIndex = 34
        Me.lblSchemeCode.Text = "Code:"
        '
        'lblDescription
        '
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(12, 91)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(77, 17)
        Me.lblDescription.TabIndex = 33
        Me.lblDescription.Text = "Description:"
        '
        'lblScheme
        '
        Me.lblScheme.BackColor = System.Drawing.SystemColors.Control
        Me.lblScheme.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblScheme.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblScheme.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblScheme.Location = New System.Drawing.Point(412, 17)
        Me.lblScheme.Name = "lblScheme"
        Me.lblScheme.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblScheme.Size = New System.Drawing.Size(133, 17)
        Me.lblScheme.TabIndex = 23
        Me.lblScheme.Text = "Numbering scheme:"
        '
        'lblBusinessType
        '
        Me.lblBusinessType.BackColor = System.Drawing.SystemColors.Control
        Me.lblBusinessType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBusinessType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBusinessType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBusinessType.Location = New System.Drawing.Point(12, 17)
        Me.lblBusinessType.Name = "lblBusinessType"
        Me.lblBusinessType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBusinessType.Size = New System.Drawing.Size(119, 17)
        Me.lblBusinessType.TabIndex = 22
        Me.lblBusinessType.Text = "Business type:"
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.chkResetNumber)
        Me.Frame2.Controls.Add(Me.chkIsResetDaily)
        Me.Frame2.Controls.Add(Me.txtFixedCode)
        Me.Frame2.Controls.Add(Me.txtMask)
        Me.Frame2.Controls.Add(Me.txtStep)
        Me.Frame2.Controls.Add(Me.txtHighestNo)
        Me.Frame2.Controls.Add(Me.txtNextNo)
        Me.Frame2.Controls.Add(Me.lblFixedCode)
        Me.Frame2.Controls.Add(Me.lblMask)
        Me.Frame2.Controls.Add(Me.lblStep)
        Me.Frame2.Controls.Add(Me.lblHighestNo)
        Me.Frame2.Controls.Add(Me.lblNextNo)
        Me.Frame2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(7, 180)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(613, 89)
        Me.Frame2.TabIndex = 11
        Me.Frame2.TabStop = False
        '
        'chkResetNumber
        '
        Me.chkResetNumber.BackColor = System.Drawing.SystemColors.Control
        Me.chkResetNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkResetNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkResetNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkResetNumber.Location = New System.Drawing.Point(286, 66)
        Me.chkResetNumber.Name = "chkResetNumber"
        Me.chkResetNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkResetNumber.Size = New System.Drawing.Size(270, 21)
        Me.chkResetNumber.TabIndex = 22
        Me.chkResetNumber.Text = "Reset number on financial year change"
        Me.chkResetNumber.UseVisualStyleBackColor = False
        '
        'chkIsResetDaily
        '
        Me.chkIsResetDaily.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsResetDaily.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsResetDaily.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsResetDaily.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsResetDaily.Location = New System.Drawing.Point(12, 66)
        Me.chkIsResetDaily.Name = "chkIsResetDaily"
        Me.chkIsResetDaily.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsResetDaily.Size = New System.Drawing.Size(261, 21)
        Me.chkIsResetDaily.TabIndex = 21
        Me.chkIsResetDaily.Text = "Reset number daily"
        Me.chkIsResetDaily.UseVisualStyleBackColor = False
        '
        'txtFixedCode
        '
        Me.txtFixedCode.AcceptsReturn = True
        Me.txtFixedCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtFixedCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFixedCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFixedCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFixedCode.Location = New System.Drawing.Point(406, 16)
        Me.txtFixedCode.MaxLength = 20
        Me.txtFixedCode.Name = "txtFixedCode"
        Me.txtFixedCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFixedCode.Size = New System.Drawing.Size(81, 20)
        Me.txtFixedCode.TabIndex = 13
        '
        'txtMask
        '
        Me.txtMask.AcceptsReturn = True
        Me.txtMask.BackColor = System.Drawing.SystemColors.Window
        Me.txtMask.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMask.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMask.Location = New System.Drawing.Point(120, 16)
        Me.txtMask.MaxLength = 20
        Me.txtMask.Name = "txtMask"
        Me.txtMask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMask.Size = New System.Drawing.Size(161, 20)
        Me.txtMask.TabIndex = 12
        '
        'txtStep
        '
        Me.txtStep.AcceptsReturn = True
        Me.txtStep.BackColor = System.Drawing.SystemColors.Window
        Me.txtStep.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStep.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStep.Location = New System.Drawing.Point(538, 42)
        Me.txtStep.MaxLength = 9
        Me.txtStep.Name = "txtStep"
        Me.txtStep.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStep.Size = New System.Drawing.Size(55, 20)
        Me.txtStep.TabIndex = 16
        '
        'txtHighestNo
        '
        Me.txtHighestNo.AcceptsReturn = True
        Me.txtHighestNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtHighestNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHighestNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHighestNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtHighestNo.Location = New System.Drawing.Point(406, 42)
        Me.txtHighestNo.MaxLength = 9
        Me.txtHighestNo.Name = "txtHighestNo"
        Me.txtHighestNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHighestNo.Size = New System.Drawing.Size(81, 20)
        Me.txtHighestNo.TabIndex = 15
        '
        'txtNextNo
        '
        Me.txtNextNo.AcceptsReturn = True
        Me.txtNextNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtNextNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNextNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNextNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNextNo.Location = New System.Drawing.Point(120, 42)
        Me.txtNextNo.MaxLength = 9
        Me.txtNextNo.Name = "txtNextNo"
        Me.txtNextNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNextNo.Size = New System.Drawing.Size(97, 20)
        Me.txtNextNo.TabIndex = 14
        '
        'lblFixedCode
        '
        Me.lblFixedCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblFixedCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFixedCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFixedCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFixedCode.Location = New System.Drawing.Point(290, 19)
        Me.lblFixedCode.Name = "lblFixedCode"
        Me.lblFixedCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFixedCode.Size = New System.Drawing.Size(97, 17)
        Me.lblFixedCode.TabIndex = 32
        Me.lblFixedCode.Text = "Fixed code:"
        '
        'lblMask
        '
        Me.lblMask.BackColor = System.Drawing.SystemColors.Control
        Me.lblMask.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMask.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMask.Location = New System.Drawing.Point(12, 19)
        Me.lblMask.Name = "lblMask"
        Me.lblMask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMask.Size = New System.Drawing.Size(97, 17)
        Me.lblMask.TabIndex = 31
        Me.lblMask.Text = "Mask code:"
        '
        'lblStep
        '
        Me.lblStep.BackColor = System.Drawing.SystemColors.Control
        Me.lblStep.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStep.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStep.Location = New System.Drawing.Point(496, 45)
        Me.lblStep.Name = "lblStep"
        Me.lblStep.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStep.Size = New System.Drawing.Size(37, 17)
        Me.lblStep.TabIndex = 27
        Me.lblStep.Text = "Step:"
        '
        'lblHighestNo
        '
        Me.lblHighestNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblHighestNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblHighestNo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHighestNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblHighestNo.Location = New System.Drawing.Point(290, 45)
        Me.lblHighestNo.Name = "lblHighestNo"
        Me.lblHighestNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblHighestNo.Size = New System.Drawing.Size(116, 18)
        Me.lblHighestNo.TabIndex = 26
        Me.lblHighestNo.Text = "Highest number:"
        '
        'lblNextNo
        '
        Me.lblNextNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblNextNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNextNo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNextNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNextNo.Location = New System.Drawing.Point(12, 45)
        Me.lblNextNo.Name = "lblNextNo"
        Me.lblNextNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNextNo.Size = New System.Drawing.Size(97, 17)
        Me.lblNextNo.TabIndex = 25
        Me.lblNextNo.Text = "Next number:"
        '
        'Frame3
        '
        Me.Frame3.BackColor = System.Drawing.SystemColors.Control
        Me.Frame3.Controls.Add(Me.chkReuse)
        Me.Frame3.Controls.Add(Me._optGenVal_1)
        Me.Frame3.Controls.Add(Me._optGenVal_0)
        Me.Frame3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame3.Location = New System.Drawing.Point(8, 3)
        Me.Frame3.Name = "Frame3"
        Me.Frame3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame3.Size = New System.Drawing.Size(613, 47)
        Me.Frame3.TabIndex = 0
        Me.Frame3.TabStop = False
        '
        'chkReuse
        '
        Me.chkReuse.BackColor = System.Drawing.SystemColors.Control
        Me.chkReuse.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkReuse.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkReuse.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.chkReuse.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkReuse.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkReuse.Location = New System.Drawing.Point(330, 14)
        Me.chkReuse.Name = "chkReuse"
        Me.chkReuse.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkReuse.Size = New System.Drawing.Size(176, 23)
        Me.chkReuse.TabIndex = 3
        Me.chkReuse.Text = "Reuse abandoned numbers"
        Me.chkReuse.UseVisualStyleBackColor = False
        '
        '_optGenVal_1
        '
        Me._optGenVal_1.BackColor = System.Drawing.SystemColors.Control
        Me._optGenVal_1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._optGenVal_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optGenVal_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optGenVal_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optGenVal_1.Location = New System.Drawing.Point(158, 16)
        Me._optGenVal_1.Name = "_optGenVal_1"
        Me._optGenVal_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optGenVal_1.Size = New System.Drawing.Size(131, 17)
        Me._optGenVal_1.TabIndex = 2
        Me._optGenVal_1.Text = "Validated"
        Me._optGenVal_1.UseVisualStyleBackColor = False
        '
        '_optGenVal_0
        '
        Me._optGenVal_0.BackColor = System.Drawing.SystemColors.Control
        Me._optGenVal_0.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._optGenVal_0.Checked = True
        Me._optGenVal_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optGenVal_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optGenVal_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optGenVal_0.Location = New System.Drawing.Point(6, 16)
        Me._optGenVal_0.Name = "_optGenVal_0"
        Me._optGenVal_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optGenVal_0.Size = New System.Drawing.Size(131, 17)
        Me._optGenVal_0.TabIndex = 1
        Me._optGenVal_0.TabStop = True
        Me._optGenVal_0.Text = "Generated"
        Me._optGenVal_0.UseVisualStyleBackColor = False
        '
        'Frame5
        '
        Me.Frame5.BackColor = System.Drawing.SystemColors.Control
        Me.Frame5.Controls.Add(Me.txtAllocateNext)
        Me.Frame5.Controls.Add(Me.lblAllocateNext)
        Me.Frame5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame5.Location = New System.Drawing.Point(8, 275)
        Me.Frame5.Name = "Frame5"
        Me.Frame5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame5.Size = New System.Drawing.Size(613, 49)
        Me.Frame5.TabIndex = 17
        Me.Frame5.TabStop = False
        '
        'txtAllocateNext
        '
        Me.txtAllocateNext.AcceptsReturn = True
        Me.txtAllocateNext.BackColor = System.Drawing.SystemColors.Window
        Me.txtAllocateNext.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAllocateNext.Enabled = False
        Me.txtAllocateNext.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAllocateNext.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAllocateNext.Location = New System.Drawing.Point(200, 18)
        Me.txtAllocateNext.MaxLength = 0
        Me.txtAllocateNext.Name = "txtAllocateNext"
        Me.txtAllocateNext.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAllocateNext.Size = New System.Drawing.Size(205, 20)
        Me.txtAllocateNext.TabIndex = 18
        Me.txtAllocateNext.TabStop = False
        '
        'lblAllocateNext
        '
        Me.lblAllocateNext.BackColor = System.Drawing.SystemColors.Control
        Me.lblAllocateNext.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAllocateNext.Enabled = False
        Me.lblAllocateNext.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAllocateNext.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAllocateNext.Location = New System.Drawing.Point(12, 21)
        Me.lblAllocateNext.Name = "lblAllocateNext"
        Me.lblAllocateNext.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAllocateNext.Size = New System.Drawing.Size(187, 17)
        Me.lblAllocateNext.TabIndex = 30
        Me.lblAllocateNext.Text = "Next number to be allocated:"
        '
        'frmDetail
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(640, 395)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDetail"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Policy Numbering Schemes "
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me.Frame2.ResumeLayout(False)
        Me.Frame2.PerformLayout()
        Me.Frame3.ResumeLayout(False)
        Me.Frame5.ResumeLayout(False)
        Me.Frame5.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
	Sub InitializeoptGenVal()
		Me.optGenVal(1) = _optGenVal_1
		Me.optGenVal(0) = _optGenVal_0
	End Sub
#End Region 
End Class