<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTreaty
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents cboReinsuranceType As PMLookupControl.cboPMLookup
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents txtAgreementCode As System.Windows.Forms.TextBox
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Private WithEvents _lvwTreatyParty_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTreatyParty_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTreatyParty_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTreatyParty_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTreatyParty_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwTreatyParty As System.Windows.Forms.ListView
	Public WithEvents fraTreaty As System.Windows.Forms.GroupBox
	Public WithEvents dtpEffectiveDate As System.Windows.Forms.DateTimePicker
	Public WithEvents dtpExpiryDate As System.Windows.Forms.DateTimePicker
	Public WithEvents txtCode As System.Windows.Forms.TextBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cboReplacesTreaty As PMLookupControl.cboPMLookup
	Public WithEvents dtpReplacedEffectivedt As System.Windows.Forms.DateTimePicker
	Public WithEvents cboReplacedByTreaty As PMLookupControl.cboPMLookup
	Public WithEvents lblReplaced As System.Windows.Forms.Label
	Public WithEvents lblReplacedEffectiveDt As System.Windows.Forms.Label
    Public WithEvents txtTreatyLimit As System.Windows.Forms.TextBox
    Public WithEvents uctCurrency As UserControls.CurrencyLookup
    Public WithEvents txtReinstatements As System.Windows.Forms.TextBox
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents lblAgreementCode As System.Windows.Forms.Label
	Public WithEvents lblReinsuranceType As System.Windows.Forms.Label
	Public WithEvents lblReplacesTreaty As System.Windows.Forms.Label
	Public WithEvents lblExpiryDate As System.Windows.Forms.Label
	Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
	Public WithEvents lblCode As System.Windows.Forms.Label
    Public WithEvents lblTreatyLimit As System.Windows.Forms.Label
    Public WithEvents lblCurrency As System.Windows.Forms.Label
    Public WithEvents lblReinstatements As System.Windows.Forms.Label
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cboReinsuranceType = New PMLookupControl.cboPMLookup
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.txtAgreementCode = New System.Windows.Forms.TextBox
        Me.fraTreaty = New System.Windows.Forms.GroupBox
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.lvwTreatyParty = New System.Windows.Forms.ListView
        Me._lvwTreatyParty_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwTreatyParty_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwTreatyParty_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwTreatyParty_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwTreatyParty_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me.dtpEffectiveDate = New System.Windows.Forms.DateTimePicker
        Me.dtpExpiryDate = New System.Windows.Forms.DateTimePicker
        Me.txtCode = New System.Windows.Forms.TextBox
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cboReplacesTreaty = New PMLookupControl.cboPMLookup
        Me.dtpReplacedEffectivedt = New System.Windows.Forms.DateTimePicker
        Me.cboReplacedByTreaty = New PMLookupControl.cboPMLookup
        Me.txtTreatyLimit = New System.Windows.Forms.TextBox
        Me.uctCurrency = New UserControls.CurrencyLookup
        Me.txtReinstatements = New System.Windows.Forms.TextBox
        Me.lblReplaced = New System.Windows.Forms.Label
        Me.lblReplacedEffectiveDt = New System.Windows.Forms.Label
        Me.lblDescription = New System.Windows.Forms.Label
        Me.lblAgreementCode = New System.Windows.Forms.Label
        Me.lblReinsuranceType = New System.Windows.Forms.Label
        Me.lblReplacesTreaty = New System.Windows.Forms.Label
        Me.lblTreatyLimit = New System.Windows.Forms.Label
        Me.lblCurrency = New System.Windows.Forms.Label
        Me.lblReinstatements = New System.Windows.Forms.Label
        Me.lblExpiryDate = New System.Windows.Forms.Label
        Me.lblEffectiveDate = New System.Windows.Forms.Label
        Me.lblCode = New System.Windows.Forms.Label
        Me.fraTreaty.SuspendLayout()
        Me.SuspendLayout()
        '
        'cboReinsuranceType
        '
        Me.cboReinsuranceType.DefaultItemId = 0
        Me.cboReinsuranceType.FirstItem = ""
        Me.cboReinsuranceType.ItemId = 0
        Me.cboReinsuranceType.ListIndex = -1
        Me.cboReinsuranceType.Location = New System.Drawing.Point(176, 152)
        Me.cboReinsuranceType.Name = "cboReinsuranceType"
        Me.cboReinsuranceType.PMLookupProductFamily = 1
        Me.cboReinsuranceType.SingleItemId = 0
        Me.cboReinsuranceType.Size = New System.Drawing.Size(140, 21)
        Me.cboReinsuranceType.Sorted = True
        Me.cboReinsuranceType.TabIndex = 11
        Me.cboReinsuranceType.TableName = "reinsurance_type"
        Me.cboReinsuranceType.ToolTipText = ""
        Me.cboReinsuranceType.WhereClause = ""
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(176, 40)
        Me.txtDescription.MaxLength = 255
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(368, 20)
        Me.txtDescription.TabIndex = 3
        '
        'txtAgreementCode
        '
        Me.txtAgreementCode.AcceptsReturn = True
        Me.txtAgreementCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgreementCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgreementCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgreementCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgreementCode.Location = New System.Drawing.Point(176, 126)
        Me.txtAgreementCode.MaxLength = 255
        Me.txtAgreementCode.Name = "txtAgreementCode"
        Me.txtAgreementCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgreementCode.Size = New System.Drawing.Size(240, 20)
        Me.txtAgreementCode.TabIndex = 9
        '
        'fraTreaty
        '
        Me.fraTreaty.BackColor = System.Drawing.SystemColors.Control
        Me.fraTreaty.Controls.Add(Me.cmdDelete)
        Me.fraTreaty.Controls.Add(Me.cmdEdit)
        Me.fraTreaty.Controls.Add(Me.cmdAdd)
        Me.fraTreaty.Controls.Add(Me.lvwTreatyParty)
        Me.fraTreaty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTreaty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTreaty.Location = New System.Drawing.Point(8, 320)
        Me.fraTreaty.Name = "fraTreaty"
        Me.fraTreaty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTreaty.Size = New System.Drawing.Size(541, 213)
        Me.fraTreaty.TabIndex = 14
        Me.fraTreaty.TabStop = False
        Me.fraTreaty.Text = "Treaty Parties"
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(457, 178)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 18
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(377, 178)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 17
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(297, 178)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 16
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'lvwTreatyParty
        '
        Me.lvwTreatyParty.BackColor = System.Drawing.SystemColors.Window
        Me.lvwTreatyParty.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwTreatyParty.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwTreatyParty_ColumnHeader_1, Me._lvwTreatyParty_ColumnHeader_2, Me._lvwTreatyParty_ColumnHeader_3, Me._lvwTreatyParty_ColumnHeader_4, Me._lvwTreatyParty_ColumnHeader_5})
        Me.lvwTreatyParty.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwTreatyParty.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwTreatyParty.FullRowSelect = True
        Me.lvwTreatyParty.Location = New System.Drawing.Point(9, 18)
        Me.lvwTreatyParty.Name = "lvwTreatyParty"
        Me.lvwTreatyParty.Size = New System.Drawing.Size(521, 153)
        Me.lvwTreatyParty.TabIndex = 15
        Me.lvwTreatyParty.UseCompatibleStateImageBehavior = False
        Me.lvwTreatyParty.View = System.Windows.Forms.View.Details
        '
        '_lvwTreatyParty_ColumnHeader_1
        '
        Me._lvwTreatyParty_ColumnHeader_1.Text = "Reinsurer"
        Me._lvwTreatyParty_ColumnHeader_1.Width = 201
        '
        '_lvwTreatyParty_ColumnHeader_2
        '
        Me._lvwTreatyParty_ColumnHeader_2.Text = "Share %"
        Me._lvwTreatyParty_ColumnHeader_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwTreatyParty_ColumnHeader_2.Width = 97
        '
        '_lvwTreatyParty_ColumnHeader_3
        '
        Me._lvwTreatyParty_ColumnHeader_3.Text = "Comm %"
        Me._lvwTreatyParty_ColumnHeader_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwTreatyParty_ColumnHeader_3.Width = 97
        '
        '_lvwTreatyParty_ColumnHeader_4
        '
        Me._lvwTreatyParty_ColumnHeader_4.Text = "Tax Group"
        Me._lvwTreatyParty_ColumnHeader_4.Width = 121
        '
        '_lvwTreatyParty_ColumnHeader_5
        '
        Me._lvwTreatyParty_ColumnHeader_5.Text = "Domiciled?"
        Me._lvwTreatyParty_ColumnHeader_5.Width = 97
        '
        'dtpEffectiveDate
        '
        Me.dtpEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpEffectiveDate.Location = New System.Drawing.Point(176, 66)
        Me.dtpEffectiveDate.Name = "dtpEffectiveDate"
        Me.dtpEffectiveDate.Size = New System.Drawing.Size(140, 21)
        Me.dtpEffectiveDate.TabIndex = 5
        '
        'dtpExpiryDate
        '
        Me.dtpExpiryDate.Checked = False
        Me.dtpExpiryDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpExpiryDate.Location = New System.Drawing.Point(176, 96)
        Me.dtpExpiryDate.Name = "dtpExpiryDate"
        Me.dtpExpiryDate.ShowCheckBox = True
        Me.dtpExpiryDate.Size = New System.Drawing.Size(140, 21)
        Me.dtpExpiryDate.TabIndex = 7
        '
        'txtCode
        '
        Me.txtCode.AcceptsReturn = True
        Me.txtCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCode.Location = New System.Drawing.Point(176, 13)
        Me.txtCode.MaxLength = 10
        Me.txtCode.Name = "txtCode"
        Me.txtCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCode.Size = New System.Drawing.Size(140, 20)
        Me.txtCode.TabIndex = 1
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(476, 540)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 20
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
        Me.cmdOK.Location = New System.Drawing.Point(396, 540)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 19
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cboReplacesTreaty
        '
        Me.cboReplacesTreaty.DefaultItemId = 0
        Me.cboReplacesTreaty.FirstItem = ""
        Me.cboReplacesTreaty.ItemId = 0
        Me.cboReplacesTreaty.ListIndex = -1
        Me.cboReplacesTreaty.Location = New System.Drawing.Point(176, 181)
        Me.cboReplacesTreaty.Name = "cboReplacesTreaty"
        Me.cboReplacesTreaty.PMLookupProductFamily = 1
        Me.cboReplacesTreaty.SingleItemId = 0
        Me.cboReplacesTreaty.Size = New System.Drawing.Size(368, 21)
        Me.cboReplacesTreaty.Sorted = True
        Me.cboReplacesTreaty.TabIndex = 13
        Me.cboReplacesTreaty.TableName = "treaty"
        Me.cboReplacesTreaty.ToolTipText = ""
        Me.cboReplacesTreaty.WhereClause = ""
        '
        'dtpReplacedEffectivedt
        '
        Me.dtpReplacedEffectivedt.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpReplacedEffectivedt.Location = New System.Drawing.Point(176, 237)
        Me.dtpReplacedEffectivedt.Name = "dtpReplacedEffectivedt"
        Me.dtpReplacedEffectivedt.Size = New System.Drawing.Size(132, 21)
        Me.dtpReplacedEffectivedt.TabIndex = 21
        '
        'cboReplacedByTreaty
        '
        Me.cboReplacedByTreaty.DefaultItemId = 0
        Me.cboReplacedByTreaty.FirstItem = ""
        Me.cboReplacedByTreaty.ItemId = 0
        Me.cboReplacedByTreaty.ListIndex = -1
        Me.cboReplacedByTreaty.Location = New System.Drawing.Point(176, 209)
        Me.cboReplacedByTreaty.Name = "cboReplacedByTreaty"
        Me.cboReplacedByTreaty.PMLookupProductFamily = 1
        Me.cboReplacedByTreaty.SingleItemId = 0
        Me.cboReplacedByTreaty.Size = New System.Drawing.Size(368, 21)
        Me.cboReplacedByTreaty.Sorted = True
        Me.cboReplacedByTreaty.TabIndex = 22
        Me.cboReplacedByTreaty.TableName = "treaty"
        Me.cboReplacedByTreaty.ToolTipText = ""
        Me.cboReplacedByTreaty.WhereClause = ""

        'txtTreatyLimit
        '
        Me.txtTreatyLimit.AcceptsReturn = True
        Me.txtTreatyLimit.BackColor = System.Drawing.SystemColors.Window
        Me.txtTreatyLimit.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTreatyLimit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTreatyLimit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTreatyLimit.Location = New System.Drawing.Point(176, 265)
        Me.txtTreatyLimit.Name = "txtTreatyLimit"
        Me.txtTreatyLimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTreatyLimit.Size = New System.Drawing.Size(140, 20)
        Me.txtTreatyLimit.TabIndex = 25
        Me.txtTreatyLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'uctCurrency
        '
        Me.uctCurrency.CompanyId = 0
        Me.uctCurrency.CurrencyId = 0
        Me.uctCurrency.DefaultCurrencyId = 0
        Me.uctCurrency.FirstItem = ""
        Me.uctCurrency.Location = New System.Drawing.Point(404, 265)
        Me.uctCurrency.Name = "uctCurrency"
        Me.uctCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actCompanyCurrencies
        Me.uctCurrency.Size = New System.Drawing.Size(140, 21)
        Me.uctCurrency.TabIndex = 26
        Me.uctCurrency.ToolTipText = ""
        Me.uctCurrency.WhatsThisHelpID = 0
        '
        'txtReinstatements
        '
        Me.txtReinstatements.AcceptsReturn = True
        Me.txtReinstatements.BackColor = System.Drawing.SystemColors.Window
        Me.txtReinstatements.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtReinstatements.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReinstatements.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtReinstatements.Location = New System.Drawing.Point(176, 291)
        Me.txtReinstatements.Name = "txtReinstatements"
        Me.txtReinstatements.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtReinstatements.Size = New System.Drawing.Size(140, 20)
        Me.txtReinstatements.TabIndex = 27

        '
        'lblReplaced
        '
        Me.lblReplaced.AutoSize = True
        Me.lblReplaced.BackColor = System.Drawing.SystemColors.Control
        Me.lblReplaced.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReplaced.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReplaced.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReplaced.Location = New System.Drawing.Point(16, 214)
        Me.lblReplaced.Name = "lblReplaced"
        Me.lblReplaced.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReplaced.Size = New System.Drawing.Size(74, 13)
        Me.lblReplaced.TabIndex = 24
        Me.lblReplaced.Text = "Replaced By :"
        '
        'lblReplacedEffectiveDt
        '
        Me.lblReplacedEffectiveDt.AutoSize = True
        Me.lblReplacedEffectiveDt.BackColor = System.Drawing.SystemColors.Control
        Me.lblReplacedEffectiveDt.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReplacedEffectiveDt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReplacedEffectiveDt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReplacedEffectiveDt.Location = New System.Drawing.Point(16, 242)
        Me.lblReplacedEffectiveDt.Name = "lblReplacedEffectiveDt"
        Me.lblReplacedEffectiveDt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReplacedEffectiveDt.Size = New System.Drawing.Size(141, 13)
        Me.lblReplacedEffectiveDt.TabIndex = 23
        Me.lblReplacedEffectiveDt.Text = "Replaced by Effective Date:"

        'lblTreatyLimit
        '
        Me.lblTreatyLimit.AutoSize = True
        Me.lblTreatyLimit.BackColor = System.Drawing.SystemColors.Control
        Me.lblTreatyLimit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTreatyLimit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTreatyLimit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTreatyLimit.Location = New System.Drawing.Point(16, 268)
        Me.lblTreatyLimit.Name = "lblTreatyLimit"
        Me.lblTreatyLimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTreatyLimit.Size = New System.Drawing.Size(66, 13)
        Me.lblTreatyLimit.TabIndex = 28
        Me.lblTreatyLimit.Text = "Reinstatement Limit:"
        '
        'lblCurrency
        '
        Me.lblCurrency.AutoSize = True
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(336, 268)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(52, 13)
        Me.lblCurrency.TabIndex = 29
        Me.lblCurrency.Text = "Currency:"
        '
        'lblReinstatements
        '
        Me.lblReinstatements.AutoSize = True
        Me.lblReinstatements.BackColor = System.Drawing.SystemColors.Control
        Me.lblReinstatements.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReinstatements.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReinstatements.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReinstatements.Location = New System.Drawing.Point(16, 294)
        Me.lblReinstatements.Name = "lblReinstatements"
        Me.lblReinstatements.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReinstatements.Size = New System.Drawing.Size(82, 13)
        Me.lblReinstatements.TabIndex = 30
        Me.lblReinstatements.Text = "Reinstatements:"
        '

        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(16, 45)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(63, 13)
        Me.lblDescription.TabIndex = 2
        Me.lblDescription.Text = "Description:"
        '
        'lblAgreementCode
        '
        Me.lblAgreementCode.AutoSize = True
        Me.lblAgreementCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgreementCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgreementCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgreementCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgreementCode.Location = New System.Drawing.Point(16, 129)
        Me.lblAgreementCode.Name = "lblAgreementCode"
        Me.lblAgreementCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgreementCode.Size = New System.Drawing.Size(89, 13)
        Me.lblAgreementCode.TabIndex = 8
        Me.lblAgreementCode.Text = "Agreement Code:"
        '
        'lblReinsuranceType
        '
        Me.lblReinsuranceType.AutoSize = True
        Me.lblReinsuranceType.BackColor = System.Drawing.SystemColors.Control
        Me.lblReinsuranceType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReinsuranceType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReinsuranceType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReinsuranceType.Location = New System.Drawing.Point(16, 157)
        Me.lblReinsuranceType.Name = "lblReinsuranceType"
        Me.lblReinsuranceType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReinsuranceType.Size = New System.Drawing.Size(97, 13)
        Me.lblReinsuranceType.TabIndex = 10
        Me.lblReinsuranceType.Text = "Reinsurance Type:"
        '
        'lblReplacesTreaty
        '
        Me.lblReplacesTreaty.AutoSize = True
        Me.lblReplacesTreaty.BackColor = System.Drawing.SystemColors.Control
        Me.lblReplacesTreaty.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReplacesTreaty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReplacesTreaty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReplacesTreaty.Location = New System.Drawing.Point(16, 186)
        Me.lblReplacesTreaty.Name = "lblReplacesTreaty"
        Me.lblReplacesTreaty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReplacesTreaty.Size = New System.Drawing.Size(88, 13)
        Me.lblReplacesTreaty.TabIndex = 12
        Me.lblReplacesTreaty.Text = "Replaces Treaty:"
        '
        'lblExpiryDate
        '
        Me.lblExpiryDate.AutoSize = True
        Me.lblExpiryDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblExpiryDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExpiryDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExpiryDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExpiryDate.Location = New System.Drawing.Point(16, 101)
        Me.lblExpiryDate.Name = "lblExpiryDate"
        Me.lblExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExpiryDate.Size = New System.Drawing.Size(64, 13)
        Me.lblExpiryDate.TabIndex = 6
        Me.lblExpiryDate.Text = "Expiry Date:"
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.AutoSize = True
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(16, 73)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(78, 13)
        Me.lblEffectiveDate.TabIndex = 4
        Me.lblEffectiveDate.Text = "Effective Date:"
        '
        'lblCode
        '
        Me.lblCode.AutoSize = True
        Me.lblCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCode.Location = New System.Drawing.Point(16, 16)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCode.Size = New System.Drawing.Size(35, 13)
        Me.lblCode.TabIndex = 0
        Me.lblCode.Text = "Code:"
        '
        'frmTreaty
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(556, 568)
        Me.Controls.Add(Me.cboReinsuranceType)
        Me.Controls.Add(Me.txtDescription)
        Me.Controls.Add(Me.txtAgreementCode)
        Me.Controls.Add(Me.fraTreaty)
        Me.Controls.Add(Me.dtpEffectiveDate)
        Me.Controls.Add(Me.dtpExpiryDate)
        Me.Controls.Add(Me.txtCode)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cboReplacesTreaty)
        Me.Controls.Add(Me.dtpReplacedEffectivedt)
        Me.Controls.Add(Me.cboReplacedByTreaty)
        Me.Controls.Add(Me.txtTreatyLimit)
        Me.Controls.Add(Me.uctCurrency)
        Me.Controls.Add(Me.txtReinstatements)
        Me.Controls.Add(Me.lblReplaced)
        Me.Controls.Add(Me.lblReplacedEffectiveDt)
        Me.Controls.Add(Me.lblTreatyLimit)
        Me.Controls.Add(Me.lblCurrency)
        Me.Controls.Add(Me.lblReinstatements)
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me.lblAgreementCode)
        Me.Controls.Add(Me.lblReinsuranceType)
        Me.Controls.Add(Me.lblReplacesTreaty)
        Me.Controls.Add(Me.lblExpiryDate)
        Me.Controls.Add(Me.lblEffectiveDate)
        Me.Controls.Add(Me.lblCode)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmTreaty"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Treaty Maintenance"
        Me.fraTreaty.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class