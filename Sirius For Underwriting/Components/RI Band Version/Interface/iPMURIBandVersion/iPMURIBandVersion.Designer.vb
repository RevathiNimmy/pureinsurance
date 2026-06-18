
Imports PMLookupControl.cboPMLookup
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmInterface

#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        isInitializingComponent = True
        InitializeComponent()
        isInitializingComponent = False
        InitializeoptRule()
        Form_Initialize_Renamed()
    End Sub
    Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
        Dispose(True)
    End Sub
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public WithEvents tabDetailTab As System.Windows.Forms.TabControl
    Public WithEvents lblRIGroup As System.Windows.Forms.Label
    Public WithEvents pnlRIBand As System.Windows.Forms.Label
    Public WithEvents cmdDelete As System.Windows.Forms.Button
    Public WithEvents cmdEdit As System.Windows.Forms.Button
    ' Public WithEvents cmdCopy As System.Windows.Forms.Button
    Private WithEvents _lvwRIBandVersion_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRIBandVersion_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRIBandsVersion_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRIBandVersion_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwRIBandVersion As System.Windows.Forms.ListView
    Public WithEvents cmdAdd As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog()
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog()
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog()
        Me.tabDetailTab = New System.Windows.Forms.TabControl()
        Me._tabDetailTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.cboXOLTreaty = New PMLookupControl.cboPMLookup()
        Me.cboPropRICal = New PMLookupControl.cboPMLookup()
        Me.cboDateForTreaty = New PMLookupControl.cboPMLookup()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblXOLTreaty = New System.Windows.Forms.Label()
        Me.lblDateForTreaty = New System.Windows.Forms.Label()
        Me.chkUseAnniversary = New System.Windows.Forms.CheckBox()
        Me.dtEffectiveDate = New System.Windows.Forms.DateTimePicker()
        Me.lblEffectiveDate = New System.Windows.Forms.Label()
        Me.lblUseAnniversary = New System.Windows.Forms.Label()
        Me.cmdDetailOK = New System.Windows.Forms.Button()
        Me.cmdDetailCancel = New System.Windows.Forms.Button()
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.lblRIGroup = New System.Windows.Forms.Label()
        Me.pnlRIBand = New System.Windows.Forms.Label()
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.cmdEdit = New System.Windows.Forms.Button()
        'Me.cmdCopy = New System.Windows.Forms.Button()
        Me.lvwRIBandVersion = New System.Windows.Forms.ListView()
        Me._lvwRIBandVersion_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRIBandVersion_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRIBandsVersion_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRIBandVersion_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRIBandVersion_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabDetailTab.SuspendLayout()
        Me._tabDetailTab_TabPage0.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(804, 432)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(110, 33)
        Me.cmdHelp.TabIndex = 7
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
        Me.cmdCancel.Location = New System.Drawing.Point(684, 432)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(110, 33)
        Me.cmdCancel.TabIndex = 5
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
        Me.cmdOK.Location = New System.Drawing.Point(564, 432)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(110, 33)
        Me.cmdOK.TabIndex = 4
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabDetailTab
        '
        Me.tabDetailTab.Controls.Add(Me._tabDetailTab_TabPage0)
        Me.tabDetailTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabDetailTab.ItemSize = New System.Drawing.Size(600, 18)
        Me.tabDetailTab.Location = New System.Drawing.Point(18, 12)
        Me.tabDetailTab.Multiline = True
        Me.tabDetailTab.Name = "tabDetailTab"
        Me.tabDetailTab.SelectedIndex = 0
        Me.tabDetailTab.Size = New System.Drawing.Size(908, 416)
        Me.tabDetailTab.TabIndex = 10
        '
        '_tabDetailTab_TabPage0
        '
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cboXOLTreaty)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cboPropRICal)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cboDateForTreaty)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.Label2)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblXOLTreaty)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblDateForTreaty)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.chkUseAnniversary)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.dtEffectiveDate)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblEffectiveDate)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblUseAnniversary)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdDetailOK)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdDetailCancel)
        Me._tabDetailTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabDetailTab_TabPage0.Name = "_tabDetailTab_TabPage0"
        Me._tabDetailTab_TabPage0.Size = New System.Drawing.Size(900, 390)
        Me._tabDetailTab_TabPage0.TabIndex = 0
        Me._tabDetailTab_TabPage0.Text = "2 - RI Band"
        Me._tabDetailTab_TabPage0.UseVisualStyleBackColor = True
        '
        'cboXOLTreaty
        '
        Me.cboXOLTreaty.DefaultItemId = 0
        Me.cboXOLTreaty.FirstItem = ""
        Me.cboXOLTreaty.ItemId = 0
        Me.cboXOLTreaty.ListIndex = -1
        Me.cboXOLTreaty.Location = New System.Drawing.Point(282, 128)
        Me.cboXOLTreaty.Name = "cboXOLTreaty"
        Me.cboXOLTreaty.PMLookupProductFamily = 9
        Me.cboXOLTreaty.SingleItemId = 0
        Me.cboXOLTreaty.Size = New System.Drawing.Size(349, 28)
        Me.cboXOLTreaty.SortColumnName = ""
        Me.cboXOLTreaty.Sorted = True
        Me.cboXOLTreaty.TabIndex = 6
        Me.cboXOLTreaty.TableName = "XOL_Treaty_To_Recover_From"
        Me.cboXOLTreaty.ToolTipText = ""
        Me.cboXOLTreaty.WhereClause = ""
        '
        'cboPropRICal
        '
        Me.cboPropRICal.DefaultItemId = 0
        Me.cboPropRICal.FirstItem = ""
        Me.cboPropRICal.ItemId = 0
        Me.cboPropRICal.ListIndex = -1
        Me.cboPropRICal.Location = New System.Drawing.Point(282, 162)
        Me.cboPropRICal.Name = "cboPropRICal"
        Me.cboPropRICal.PMLookupProductFamily = 9
        Me.cboPropRICal.SingleItemId = 0
        Me.cboPropRICal.Size = New System.Drawing.Size(349, 28)
        Me.cboPropRICal.SortColumnName = ""
        Me.cboPropRICal.Sorted = True
        Me.cboPropRICal.TabIndex = 6
        Me.cboPropRICal.TableName = "Proportional_RI_Calculation_Method"
        Me.cboPropRICal.ToolTipText = ""
        Me.cboPropRICal.WhereClause = ""
        '
        'cboDateForTreaty
        '
        Me.cboDateForTreaty.DefaultItemId = 0
        Me.cboDateForTreaty.FirstItem = ""
        Me.cboDateForTreaty.ItemId = 0
        Me.cboDateForTreaty.ListIndex = -1
        Me.cboDateForTreaty.Location = New System.Drawing.Point(282, 94)
        Me.cboDateForTreaty.Name = "cboDateForTreaty"
        Me.cboDateForTreaty.PMLookupProductFamily = 9
        Me.cboDateForTreaty.SingleItemId = 0
        Me.cboDateForTreaty.Size = New System.Drawing.Size(349, 28)
        Me.cboDateForTreaty.SortColumnName = ""
        Me.cboDateForTreaty.Sorted = True
        Me.cboDateForTreaty.TabIndex = 6
        Me.cboDateForTreaty.TableName = "Date_for_Treaty_XOL_Calculation"
        Me.cboDateForTreaty.ToolTipText = ""
        Me.cboDateForTreaty.WhereClause = ""
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(28, 180)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(231, 20)
        Me.Label2.TabIndex = 31
        Me.Label2.Text = "Proportional RI Cal Method:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblXOLTreaty
        '
        Me.lblXOLTreaty.AutoSize = True
        Me.lblXOLTreaty.BackColor = System.Drawing.SystemColors.Control
        Me.lblXOLTreaty.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblXOLTreaty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblXOLTreaty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblXOLTreaty.Location = New System.Drawing.Point(28, 146)
        Me.lblXOLTreaty.Name = "lblXOLTreaty"
        Me.lblXOLTreaty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblXOLTreaty.Size = New System.Drawing.Size(246, 20)
        Me.lblXOLTreaty.TabIndex = 30
        Me.lblXOLTreaty.Text = "XOL Treaty To Recover From:"
        Me.lblXOLTreaty.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblDateForTreaty
        '
        Me.lblDateForTreaty.AutoSize = True
        Me.lblDateForTreaty.BackColor = System.Drawing.SystemColors.Control
        Me.lblDateForTreaty.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDateForTreaty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateForTreaty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDateForTreaty.Location = New System.Drawing.Point(25, 104)
        Me.lblDateForTreaty.Name = "lblDateForTreaty"
        Me.lblDateForTreaty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDateForTreaty.Size = New System.Drawing.Size(265, 20)
        Me.lblDateForTreaty.TabIndex = 29
        Me.lblDateForTreaty.Text = "Date For Treaty Xol Calculation:"
        Me.lblDateForTreaty.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'chkUseAnniversary
        '
        Me.chkUseAnniversary.AutoSize = True
        Me.chkUseAnniversary.Location = New System.Drawing.Point(282, 215)
        Me.chkUseAnniversary.Name = "chkUseAnniversary"
        Me.chkUseAnniversary.Size = New System.Drawing.Size(22, 21)
        Me.chkUseAnniversary.TabIndex = 28
        Me.chkUseAnniversary.UseVisualStyleBackColor = True
        '
        'dtEffectiveDate
        '
        Me.dtEffectiveDate.Location = New System.Drawing.Point(282, 255)
        Me.dtEffectiveDate.Name = "dtEffectiveDate"
        Me.dtEffectiveDate.Size = New System.Drawing.Size(349, 28)
        Me.dtEffectiveDate.TabIndex = 27
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.AutoSize = True
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(28, 263)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(126, 20)
        Me.lblEffectiveDate.TabIndex = 25
        Me.lblEffectiveDate.Text = "Effective date:"
        Me.lblEffectiveDate.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblUseAnniversary
        '
        Me.lblUseAnniversary.AutoSize = True
        Me.lblUseAnniversary.BackColor = System.Drawing.SystemColors.Control
        Me.lblUseAnniversary.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUseAnniversary.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUseAnniversary.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUseAnniversary.Location = New System.Drawing.Point(28, 216)
        Me.lblUseAnniversary.Name = "lblUseAnniversary"
        Me.lblUseAnniversary.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUseAnniversary.Size = New System.Drawing.Size(230, 20)
        Me.lblUseAnniversary.TabIndex = 18
        Me.lblUseAnniversary.Text = "Use Anniversary Date For Tmp:"
        Me.lblUseAnniversary.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmdDetailOK
        '
        Me.cmdDetailOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDetailOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDetailOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDetailOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDetailOK.Location = New System.Drawing.Point(660, 333)
        Me.cmdDetailOK.Name = "cmdDetailOK"
        Me.cmdDetailOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDetailOK.Size = New System.Drawing.Size(110, 33)
        Me.cmdDetailOK.TabIndex = 8
        Me.cmdDetailOK.Text = "&OK"
        Me.cmdDetailOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDetailOK.UseVisualStyleBackColor = False
        '
        'cmdDetailCancel
        '
        Me.cmdDetailCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDetailCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDetailCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDetailCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDetailCancel.Location = New System.Drawing.Point(780, 333)
        Me.cmdDetailCancel.Name = "cmdDetailCancel"
        Me.cmdDetailCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDetailCancel.Size = New System.Drawing.Size(110, 33)
        Me.cmdDetailCancel.TabIndex = 9
        Me.cmdDetailCancel.Text = "&Cancel"
        Me.cmdDetailCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDetailCancel.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(600, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(12, 12)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(908, 416)
        Me.tabMainTab.TabIndex = 11
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblRIGroup)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pnlRIBand)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdDelete)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdEdit)
        'Me._tabMainTab_TabPage0.Controls.Add(Me.cmdCopy)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwRIBandVersion)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAdd)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(900, 390)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - RI Band Configuration"
        '
        'lblRIGroup
        '
        Me.lblRIGroup.AutoSize = True
        Me.lblRIGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblRIGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRIGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRIGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRIGroup.Location = New System.Drawing.Point(45, 24)
        Me.lblRIGroup.Name = "lblRIGroup"
        Me.lblRIGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRIGroup.Size = New System.Drawing.Size(72, 20)
        Me.lblRIGroup.TabIndex = 13
        Me.lblRIGroup.Text = "RI Band:"
        Me.lblRIGroup.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'pnlRIBand
        '
        Me.pnlRIBand.BackColor = System.Drawing.SystemColors.Control
        Me.pnlRIBand.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlRIBand.Cursor = System.Windows.Forms.Cursors.Default
        Me.pnlRIBand.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlRIBand.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlRIBand.Location = New System.Drawing.Point(159, 21)
        Me.pnlRIBand.Name = "pnlRIBand"
        Me.pnlRIBand.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlRIBand.Size = New System.Drawing.Size(719, 29)
        Me.pnlRIBand.TabIndex = 14
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(780, 333)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(110, 33)
        Me.cmdDelete.TabIndex = 3
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
        'Me.cmdEdit.Location = New System.Drawing.Point(424, 333)
        Me.cmdEdit.Location = New System.Drawing.Point(548, 333)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(110, 33)
        Me.cmdEdit.TabIndex = 1
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdCopy
        '
        'Me.cmdCopy.BackColor = System.Drawing.SystemColors.Control
        'Me.cmdCopy.Cursor = System.Windows.Forms.Cursors.Default
        'Me.cmdCopy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        'Me.cmdCopy.ForeColor = System.Drawing.SystemColors.ControlText
        'Me.cmdCopy.Location = New System.Drawing.Point(548, 333)
        'Me.cmdCopy.Name = "cmdCopy"
        'Me.cmdCopy.RightToLeft = System.Windows.Forms.RightToLeft.No
        'Me.cmdCopy.Size = New System.Drawing.Size(110, 33)
        'Me.cmdCopy.TabIndex = 15
        'Me.cmdCopy.Text = "&Copy"
        'Me.cmdCopy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        'Me.cmdCopy.UseVisualStyleBackColor = False
        '
        'lvwRIBandVersion
        '
        Me.lvwRIBandVersion.BackColor = System.Drawing.SystemColors.Window
        Me.lvwRIBandVersion.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwRIBandVersion, "")
        Me.lvwRIBandVersion.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwRIBandVersion_ColumnHeader_1, Me._lvwRIBandVersion_ColumnHeader_2, Me._lvwRIBandsVersion_ColumnHeader_3, Me._lvwRIBandVersion_ColumnHeader_4, Me._lvwRIBandVersion_ColumnHeader_5})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwRIBandVersion, True)
        Me.lvwRIBandVersion.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwRIBandVersion.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwRIBandVersion.FullRowSelect = True
        Me.lvwRIBandVersion.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwRIBandVersion, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwRIBandVersion, "")
        Me.lvwRIBandVersion.Location = New System.Drawing.Point(9, 60)
        Me.lvwRIBandVersion.Name = "lvwRIBandVersion"
        Me.lvwRIBandVersion.Size = New System.Drawing.Size(877, 260)
        Me.listViewHelper1.SetSmallIcons(Me.lvwRIBandVersion, "")
        Me.listViewHelper1.SetSorted(Me.lvwRIBandVersion, False)
        Me.listViewHelper1.SetSortKey(Me.lvwRIBandVersion, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwRIBandVersion, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwRIBandVersion.TabIndex = 0
        Me.lvwRIBandVersion.UseCompatibleStateImageBehavior = False
        Me.lvwRIBandVersion.View = System.Windows.Forms.View.Details
        '
        '_lvwRIBandVersion_ColumnHeader_1
        '
        Me._lvwRIBandVersion_ColumnHeader_1.Text = "Date for Treaty Xol Calculation"
        Me._lvwRIBandVersion_ColumnHeader_1.Width = 195
        '
        '_lvwRIBandVersion_ColumnHeader_2
        '
        Me._lvwRIBandVersion_ColumnHeader_2.Text = "Xol Treaty To Recover From"
        Me._lvwRIBandVersion_ColumnHeader_2.Width = 97
        '
        '_lvwRIBandsVersion_ColumnHeader_3
        '
        Me._lvwRIBandsVersion_ColumnHeader_3.Text = "Proportional RI Cal Method"
        Me._lvwRIBandsVersion_ColumnHeader_3.Width = 131
        '
        '_lvwRIBandVersion_ColumnHeader_4
        '
        Me._lvwRIBandVersion_ColumnHeader_4.Text = "Use Anniversary Date For Tmp"
        Me._lvwRIBandVersion_ColumnHeader_4.Width = 141
        '
        '_lvwRIBandVersion_ColumnHeader_5
        '
        Me._lvwRIBandVersion_ColumnHeader_5.Text = "Effective Date"
        Me._lvwRIBandVersion_ColumnHeader_5.Width = 141
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(660, 333)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(110, 33)
        Me.cmdAdd.TabIndex = 2
        Me.cmdAdd.Text = "&New"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(9, 21)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(928, 494)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabDetailTab)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "RI Band Configuration"
        Me.tabDetailTab.ResumeLayout(False)
        Me._tabDetailTab_TabPage0.ResumeLayout(False)
        Me._tabDetailTab_TabPage0.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Sub InitializeoptRule()
        'Me.optRule(2) = _optRule_2
        'Me.optRule(1) = _optRule_1
        'Me.optRule(0) = _optRule_0
    End Sub
    Friend WithEvents _lvwRIBandVersion_ColumnHeader_5 As ColumnHeader
    Private WithEvents _tabDetailTab_TabPage0 As TabPage
    Friend WithEvents chkUseAnniversary As CheckBox
    Friend WithEvents dtEffectiveDate As DateTimePicker
    Public WithEvents lblEffectiveDate As Label
    Public WithEvents lblUseAnniversary As Label
    Public WithEvents cmdDetailOK As Button
    Public WithEvents cmdDetailCancel As Button
    Public WithEvents lblDateForTreaty As Label
    Public WithEvents Label2 As Label
    Public WithEvents lblXOLTreaty As Label
    Private WithEvents cboXOLTreaty As PMLookupControl.cboPMLookup
    Private WithEvents cboPropRICal As PMLookupControl.cboPMLookup
    Private WithEvents cboDateForTreaty As PMLookupControl.cboPMLookup
#End Region
End Class