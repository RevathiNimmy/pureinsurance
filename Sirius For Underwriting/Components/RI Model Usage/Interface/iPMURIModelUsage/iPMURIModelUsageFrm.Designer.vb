<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents lblRIBand As System.Windows.Forms.Label
	Public WithEvents lblRIModel As System.Windows.Forms.Label
	Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
	Public WithEvents lblExpiryDate As System.Windows.Forms.Label
	Public WithEvents lblPortfolio As System.Windows.Forms.Label
	Public WithEvents cboRIBand As PMLookupControl.cboPMLookup
	Public WithEvents cmdDetailOK As System.Windows.Forms.Button
	Public WithEvents cmdDetailCancel As System.Windows.Forms.Button
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents txtEffectiveDate As System.Windows.Forms.TextBox
	Public WithEvents chkIsDeleted As System.Windows.Forms.CheckBox
	Public WithEvents cboRIModel As System.Windows.Forms.ComboBox
	Public WithEvents txtExpiryDate As System.Windows.Forms.TextBox
	Public WithEvents cboPortfolioRIModel As System.Windows.Forms.ComboBox
	Private WithEvents _tabDetailTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabDetailTab As System.Windows.Forms.TabControl
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblModelUsage As System.Windows.Forms.Label
	Public WithEvents lblModelUsageValue As System.Windows.Forms.Label
	Private WithEvents _lvwRIModelUsage_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRIModelUsage_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRIModelUsage_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRIModelUsage_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRIModelUsage_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRIModelUsage_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRIModelUsage_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwRIModelUsage As System.Windows.Forms.ListView
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public WithEvents ImageList1 As System.Windows.Forms.ImageList
    'TODOLIST-Commented the listviewhelper as it was conflicting with icon displai in listview)
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tabDetailTab = New System.Windows.Forms.TabControl
        Me._tabDetailTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblDescription = New System.Windows.Forms.Label
        Me.lblRIBand = New System.Windows.Forms.Label
        Me.lblRIModel = New System.Windows.Forms.Label
        Me.lblEffectiveDate = New System.Windows.Forms.Label
        Me.lblExpiryDate = New System.Windows.Forms.Label
        Me.lblPortfolio = New System.Windows.Forms.Label
        Me.cboRIBand = New PMLookupControl.cboPMLookup
        Me.cmdDetailOK = New System.Windows.Forms.Button
        Me.cmdDetailCancel = New System.Windows.Forms.Button
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.txtEffectiveDate = New System.Windows.Forms.TextBox
        Me.chkIsDeleted = New System.Windows.Forms.CheckBox
        Me.cboRIModel = New System.Windows.Forms.ComboBox
        Me.txtExpiryDate = New System.Windows.Forms.TextBox
        Me.cboPortfolioRIModel = New System.Windows.Forms.ComboBox
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblModelUsage = New System.Windows.Forms.Label
        Me.lblModelUsageValue = New System.Windows.Forms.Label
        Me.lvwRIModelUsage = New System.Windows.Forms.ListView
        Me._lvwRIModelUsage_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwRIModelUsage_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwRIModelUsage_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwRIModelUsage_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwRIModelUsage_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwRIModelUsage_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwRIModelUsage_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.tabDetailTab.SuspendLayout()
        Me._tabDetailTab_TabPage0.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabDetailTab
        '
        Me.tabDetailTab.Controls.Add(Me._tabDetailTab_TabPage0)
        Me.tabDetailTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabDetailTab.ItemSize = New System.Drawing.Size(600, 19)
        Me.tabDetailTab.Location = New System.Drawing.Point(88, -24)
        Me.tabDetailTab.Multiline = True
        Me.tabDetailTab.Name = "tabDetailTab"
        Me.tabDetailTab.SelectedIndex = 0
        Me.tabDetailTab.Size = New System.Drawing.Size(605, 277)
        Me.tabDetailTab.TabIndex = 11
        Me.tabDetailTab.Visible = False
        '
        '_tabDetailTab_TabPage0
        '
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblDescription)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblRIBand)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblRIModel)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblEffectiveDate)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblExpiryDate)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblPortfolio)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cboRIBand)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdDetailOK)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdDetailCancel)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtDescription)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtEffectiveDate)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.chkIsDeleted)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cboRIModel)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtExpiryDate)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cboPortfolioRIModel)
        Me._tabDetailTab_TabPage0.Location = New System.Drawing.Point(4, 23)
        Me._tabDetailTab_TabPage0.Name = "_tabDetailTab_TabPage0"
        Me._tabDetailTab_TabPage0.Size = New System.Drawing.Size(597, 250)
        Me._tabDetailTab_TabPage0.TabIndex = 0
        Me._tabDetailTab_TabPage0.Text = "&2 - Model Usage"
        '
        'lblDescription
        '
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(13, 66)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(145, 17)
        Me.lblDescription.TabIndex = 14
        Me.lblDescription.Text = "Description:"
        '
        'lblRIBand
        '
        Me.lblRIBand.BackColor = System.Drawing.SystemColors.Control
        Me.lblRIBand.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRIBand.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRIBand.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRIBand.Location = New System.Drawing.Point(13, 18)
        Me.lblRIBand.Name = "lblRIBand"
        Me.lblRIBand.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRIBand.Size = New System.Drawing.Size(145, 17)
        Me.lblRIBand.TabIndex = 15
        Me.lblRIBand.Text = "Reinsurance Band:"
        '
        'lblRIModel
        '
        Me.lblRIModel.BackColor = System.Drawing.SystemColors.Control
        Me.lblRIModel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRIModel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRIModel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRIModel.Location = New System.Drawing.Point(13, 42)
        Me.lblRIModel.Name = "lblRIModel"
        Me.lblRIModel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRIModel.Size = New System.Drawing.Size(145, 17)
        Me.lblRIModel.TabIndex = 16
        Me.lblRIModel.Text = "Reinsurance Model:"
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(13, 154)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(145, 17)
        Me.lblEffectiveDate.TabIndex = 17
        Me.lblEffectiveDate.Text = "Effective Date:"
        '
        'lblExpiryDate
        '
        Me.lblExpiryDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblExpiryDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExpiryDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExpiryDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExpiryDate.Location = New System.Drawing.Point(13, 178)
        Me.lblExpiryDate.Name = "lblExpiryDate"
        Me.lblExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExpiryDate.Size = New System.Drawing.Size(145, 17)
        Me.lblExpiryDate.TabIndex = 22
        Me.lblExpiryDate.Text = "Expiry Date:"
        '
        'lblPortfolio
        '
        Me.lblPortfolio.BackColor = System.Drawing.SystemColors.Control
        Me.lblPortfolio.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPortfolio.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPortfolio.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPortfolio.Location = New System.Drawing.Point(13, 202)
        Me.lblPortfolio.Name = "lblPortfolio"
        Me.lblPortfolio.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPortfolio.Size = New System.Drawing.Size(145, 17)
        Me.lblPortfolio.TabIndex = 24
        Me.lblPortfolio.Text = "Portfolio Transfer from:"
        '
        'cboRIBand
        '
        Me.cboRIBand.DefaultItemId = 0
        Me.cboRIBand.FirstItem = ""
        Me.cboRIBand.ItemId = 0
        Me.cboRIBand.ListIndex = -1
        Me.cboRIBand.Location = New System.Drawing.Point(158, 14)
        Me.cboRIBand.Name = "cboRIBand"
        Me.cboRIBand.PMLookupProductFamily = 1
        Me.cboRIBand.SingleItemId = 0
        Me.cboRIBand.Size = New System.Drawing.Size(240, 21)
        Me.cboRIBand.Sorted = True
        Me.cboRIBand.TabIndex = 25
        Me.cboRIBand.TableName = "ri_band"
        Me.cboRIBand.ToolTipText = ""
        Me.cboRIBand.WhereClause = ""
        '
        'cmdDetailOK
        '
        Me.cmdDetailOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDetailOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDetailOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDetailOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDetailOK.Location = New System.Drawing.Point(435, 219)
        Me.cmdDetailOK.Name = "cmdDetailOK"
        Me.cmdDetailOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDetailOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdDetailOK.TabIndex = 6
        Me.cmdDetailOK.Text = "OK"
        Me.cmdDetailOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDetailOK.UseVisualStyleBackColor = False
        '
        'cmdDetailCancel
        '
        Me.cmdDetailCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDetailCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDetailCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDetailCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDetailCancel.Location = New System.Drawing.Point(515, 219)
        Me.cmdDetailCancel.Name = "cmdDetailCancel"
        Me.cmdDetailCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDetailCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdDetailCancel.TabIndex = 7
        Me.cmdDetailCancel.Text = "Cancel"
        Me.cmdDetailCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDetailCancel.UseVisualStyleBackColor = False
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(158, 62)
        Me.txtDescription.MaxLength = 0
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDescription.Size = New System.Drawing.Size(240, 63)
        Me.txtDescription.TabIndex = 2
        '
        'txtEffectiveDate
        '
        Me.txtEffectiveDate.AcceptsReturn = True
        Me.txtEffectiveDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtEffectiveDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEffectiveDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEffectiveDate.Location = New System.Drawing.Point(158, 148)
        Me.txtEffectiveDate.MaxLength = 0
        Me.txtEffectiveDate.Name = "txtEffectiveDate"
        Me.txtEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEffectiveDate.Size = New System.Drawing.Size(160, 20)
        Me.txtEffectiveDate.TabIndex = 4
        '
        'chkIsDeleted
        '
        Me.chkIsDeleted.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsDeleted.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsDeleted.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsDeleted.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsDeleted.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsDeleted.Location = New System.Drawing.Point(13, 126)
        Me.chkIsDeleted.Name = "chkIsDeleted"
        Me.chkIsDeleted.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsDeleted.Size = New System.Drawing.Size(158, 21)
        Me.chkIsDeleted.TabIndex = 3
        Me.chkIsDeleted.Text = "Is deleted:"
        Me.chkIsDeleted.UseVisualStyleBackColor = False
        '
        'cboRIModel
        '
        Me.cboRIModel.BackColor = System.Drawing.SystemColors.Window
        Me.cboRIModel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboRIModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRIModel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRIModel.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboRIModel.Location = New System.Drawing.Point(158, 38)
        Me.cboRIModel.Name = "cboRIModel"
        Me.cboRIModel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboRIModel.Size = New System.Drawing.Size(240, 21)
        Me.cboRIModel.TabIndex = 21
        '
        'txtExpiryDate
        '
        Me.txtExpiryDate.AcceptsReturn = True
        Me.txtExpiryDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtExpiryDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtExpiryDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExpiryDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtExpiryDate.Location = New System.Drawing.Point(158, 172)
        Me.txtExpiryDate.MaxLength = 0
        Me.txtExpiryDate.Name = "txtExpiryDate"
        Me.txtExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtExpiryDate.Size = New System.Drawing.Size(160, 20)
        Me.txtExpiryDate.TabIndex = 5
        '
        'cboPortfolioRIModel
        '
        Me.cboPortfolioRIModel.BackColor = System.Drawing.SystemColors.Window
        Me.cboPortfolioRIModel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPortfolioRIModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPortfolioRIModel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPortfolioRIModel.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPortfolioRIModel.Location = New System.Drawing.Point(158, 196)
        Me.cboPortfolioRIModel.Name = "cboPortfolioRIModel"
        Me.cboPortfolioRIModel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPortfolioRIModel.Size = New System.Drawing.Size(240, 21)
        Me.cboPortfolioRIModel.TabIndex = 23
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(8, 291)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 10
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
        Me.cmdCancel.Location = New System.Drawing.Point(536, 291)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 9
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
        Me.cmdOK.Location = New System.Drawing.Point(456, 291)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 8
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(600, 19)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(605, 278)
        Me.tabMainTab.TabIndex = 12
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblModelUsage)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblModelUsageValue)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwRIModelUsage)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdDelete)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdEdit)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAdd)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 23)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(597, 251)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - RI Model Usage"
        '
        'lblModelUsage
        '
        Me.lblModelUsage.BackColor = System.Drawing.SystemColors.Control
        Me.lblModelUsage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblModelUsage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblModelUsage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblModelUsage.Location = New System.Drawing.Point(15, 16)
        Me.lblModelUsage.Name = "lblModelUsage"
        Me.lblModelUsage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblModelUsage.Size = New System.Drawing.Size(98, 19)
        Me.lblModelUsage.TabIndex = 13
        Me.lblModelUsage.Text = "Risk Type:"
        '
        'lblModelUsageValue
        '
        Me.lblModelUsageValue.BackColor = System.Drawing.SystemColors.Control
        Me.lblModelUsageValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblModelUsageValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblModelUsageValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblModelUsageValue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblModelUsageValue.Location = New System.Drawing.Point(124, 14)
        Me.lblModelUsageValue.Name = "lblModelUsageValue"
        Me.lblModelUsageValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblModelUsageValue.Size = New System.Drawing.Size(463, 21)
        Me.lblModelUsageValue.TabIndex = 18
        '
        'lvwRIModelUsage
        '
        Me.lvwRIModelUsage.BackColor = System.Drawing.SystemColors.Window
        Me.lvwRIModelUsage.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwRIModelUsage.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwRIModelUsage_ColumnHeader_1, Me._lvwRIModelUsage_ColumnHeader_2, Me._lvwRIModelUsage_ColumnHeader_3, Me._lvwRIModelUsage_ColumnHeader_4, Me._lvwRIModelUsage_ColumnHeader_5, Me._lvwRIModelUsage_ColumnHeader_6, Me._lvwRIModelUsage_ColumnHeader_7})
        Me.lvwRIModelUsage.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwRIModelUsage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwRIModelUsage.FullRowSelect = True
        Me.lvwRIModelUsage.LargeImageList = Me.ImageList1
        Me.lvwRIModelUsage.Location = New System.Drawing.Point(12, 46)
        Me.lvwRIModelUsage.Name = "lvwRIModelUsage"
        Me.lvwRIModelUsage.Size = New System.Drawing.Size(576, 163)
        Me.lvwRIModelUsage.SmallImageList = Me.ImageList1
        Me.lvwRIModelUsage.TabIndex = 19
        Me.lvwRIModelUsage.UseCompatibleStateImageBehavior = False
        Me.lvwRIModelUsage.View = System.Windows.Forms.View.Details
        '
        '_lvwRIModelUsage_ColumnHeader_1
        '
        Me._lvwRIModelUsage_ColumnHeader_1.Text = "Model"
        Me._lvwRIModelUsage_ColumnHeader_1.Width = 201
        '
        '_lvwRIModelUsage_ColumnHeader_2
        '
        Me._lvwRIModelUsage_ColumnHeader_2.Text = "Band"
        Me._lvwRIModelUsage_ColumnHeader_2.Width = 201
        '
        '_lvwRIModelUsage_ColumnHeader_3
        '
        Me._lvwRIModelUsage_ColumnHeader_3.Text = "Description"
        Me._lvwRIModelUsage_ColumnHeader_3.Width = 481
        '
        '_lvwRIModelUsage_ColumnHeader_4
        '
        Me._lvwRIModelUsage_ColumnHeader_4.Text = "Is Deleted"
        Me._lvwRIModelUsage_ColumnHeader_4.Width = 97
        '
        '_lvwRIModelUsage_ColumnHeader_5
        '
        Me._lvwRIModelUsage_ColumnHeader_5.Text = "Effective Date"
        Me._lvwRIModelUsage_ColumnHeader_5.Width = 97
        '
        '_lvwRIModelUsage_ColumnHeader_6
        '
        Me._lvwRIModelUsage_ColumnHeader_6.Text = "Expiry Date"
        Me._lvwRIModelUsage_ColumnHeader_6.Width = 97
        '
        '_lvwRIModelUsage_ColumnHeader_7
        '
        Me._lvwRIModelUsage_ColumnHeader_7.Text = "Transfer From"
        Me._lvwRIModelUsage_ColumnHeader_7.Width = 97
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "FindImage")
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(172, 219)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 1
        Me.cmdDelete.Text = "Dele&te"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(92, 219)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 0
        Me.cmdEdit.Text = "&Edit..."
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(12, 219)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 20
        Me.cmdAdd.Text = "A&dd..."
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(694, 322)
        Me.Controls.Add(Me.tabDetailTab)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Reinsurance Model Usage"
        Me.tabDetailTab.ResumeLayout(False)
        Me._tabDetailTab_TabPage0.ResumeLayout(False)
        Me._tabDetailTab_TabPage0.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class