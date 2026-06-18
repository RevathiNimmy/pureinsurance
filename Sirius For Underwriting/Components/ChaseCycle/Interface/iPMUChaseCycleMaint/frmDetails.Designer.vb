<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDetails
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializetxtHelper()
		InitializefraCashDrawer()
		lvwSteps_InitializeColumnKeys()
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
    Public WithEvents cmdApply As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public fraCashDrawer(0) As System.Windows.Forms.GroupBox
    Public txtHelper(2) As System.Windows.Forms.TextBox
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    Private WithEvents listBoxHelper1 As Artinsoft.VB6.Gui.ListBoxHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdApply = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.lvwSteps = New System.Windows.Forms.ListView
        Me._lvwSteps_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwSteps_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwSteps_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwSteps_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._lvwSteps_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
        Me.listBoxHelper1 = New Artinsoft.VB6.Gui.ListBoxHelper(Me.components)
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.cboGISProperty = New System.Windows.Forms.ComboBox
        Me.cboUDLChaseCycle = New System.Windows.Forms.ComboBox
        Me.chkCancelledOnly = New System.Windows.Forms.CheckBox
        Me.chkIncludeCancelled = New System.Windows.Forms.CheckBox
        Me.cboGISDataModel = New PMLookupControl.cboPMLookup
        Me.chkUseGreaterTransEffDate = New System.Windows.Forms.CheckBox
        Me.lblGisDataModel = New System.Windows.Forms.Label
        Me.lblDescription = New System.Windows.Forms.Label
        Me.lblPMLookupSource = New System.Windows.Forms.Label
        Me.lblProcessingDays = New System.Windows.Forms.Label
        Me._txtHelper_1 = New System.Windows.Forms.TextBox
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.txtProcessingDays = New System.Windows.Forms.TextBox
        Me.cboProduct = New PMLookupControl.cboPMLookup
        Me.chkActive = New System.Windows.Forms.CheckBox
        Me.cboPMLookupSource = New PMLookupControl.cboPMLookup
        Me._fraCashDrawer_0 = New System.Windows.Forms.GroupBox
        Me.cmdAddStep = New System.Windows.Forms.Button
        Me.cmdEditStep = New System.Windows.Forms.Button
        Me.cmdDeleteStep = New System.Windows.Forms.Button
        Me.chkUseEffectiveDate = New System.Windows.Forms.CheckBox
        Me._txtHelper_2 = New System.Windows.Forms.TextBox
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me.lblUDLChaseCycle = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblGISProperty = New System.Windows.Forms.Label
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me._fraCashDrawer_0.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApply.Enabled = False
        Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Location = New System.Drawing.Point(505, 467)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApply.Size = New System.Drawing.Size(73, 22)
        Me.cmdApply.TabIndex = 30
        Me.cmdApply.Tag = "CAP;208"
        Me.cmdApply.Text = "Apply"
        Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdApply.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(585, 467)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 31
        Me.cmdCancel.Tag = "CAP;210"
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(425, 467)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 29
        Me.cmdOK.Tag = "CAP;209"
        Me.cmdOK.Text = "OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'lvwSteps
        '
        Me.lvwSteps.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwSteps, "")
        Me.lvwSteps.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSteps_ColumnHeader_1, Me._lvwSteps_ColumnHeader_2, Me._lvwSteps_ColumnHeader_6, Me._lvwSteps_ColumnHeader_7, Me._lvwSteps_ColumnHeader_8})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwSteps, True)
        Me.lvwSteps.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSteps.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSteps.FullRowSelect = True
        Me.lvwSteps.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwSteps, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwSteps, "")
        Me.lvwSteps.Location = New System.Drawing.Point(8, 16)
        Me.lvwSteps.Name = "lvwSteps"
        Me.lvwSteps.Size = New System.Drawing.Size(601, 173)
        Me.listViewHelper1.SetSmallIcons(Me.lvwSteps, "")
        Me.listViewHelper1.SetSorted(Me.lvwSteps, False)
        Me.listViewHelper1.SetSortKey(Me.lvwSteps, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwSteps, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwSteps.TabIndex = 16
        Me.lvwSteps.Tag = "CAP;410"
        Me.lvwSteps.UseCompatibleStateImageBehavior = False
        Me.lvwSteps.View = System.Windows.Forms.View.Details
        '
        '_lvwSteps_ColumnHeader_1
        '
        Me._lvwSteps_ColumnHeader_1.Tag = ""
        Me._lvwSteps_ColumnHeader_1.Text = "Step Number"
        Me._lvwSteps_ColumnHeader_1.Width = 74
        '
        '_lvwSteps_ColumnHeader_2
        '
        Me._lvwSteps_ColumnHeader_2.Tag = ""
        Me._lvwSteps_ColumnHeader_2.Text = "Elapsed Days"
        Me._lvwSteps_ColumnHeader_2.Width = 74
        '
        '_lvwSteps_ColumnHeader_6
        '
        Me._lvwSteps_ColumnHeader_6.Tag = ""
        Me._lvwSteps_ColumnHeader_6.Text = "Next Step"
        Me._lvwSteps_ColumnHeader_6.Width = 55
        '
        '_lvwSteps_ColumnHeader_7
        '
        Me._lvwSteps_ColumnHeader_7.Tag = ""
        Me._lvwSteps_ColumnHeader_7.Text = "Auto-Cancel"
        Me._lvwSteps_ColumnHeader_7.Width = 67
        '
        '_lvwSteps_ColumnHeader_8
        '
        Me._lvwSteps_ColumnHeader_8.Tag = "HIDDEN"
        Me._lvwSteps_ColumnHeader_8.Text = "{Step ID"
        Me._lvwSteps_ColumnHeader_8.Width = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblGISProperty)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboGISProperty)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboUDLChaseCycle)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkCancelledOnly)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkIncludeCancelled)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboGISDataModel)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkUseGreaterTransEffDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblGisDataModel)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPMLookupSource)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblProcessingDays)
        Me._tabMainTab_TabPage0.Controls.Add(Me._txtHelper_1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtProcessingDays)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblUDLChaseCycle)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Label1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboProduct)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkActive)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboPMLookupSource)
        Me._tabMainTab_TabPage0.Controls.Add(Me._fraCashDrawer_0)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkUseEffectiveDate)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(645, 427)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Detail"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'cboGISProperty
        '
        Me.cboGISProperty.FormattingEnabled = True
        Me.cboGISProperty.Location = New System.Drawing.Point(417, 70)
        Me.cboGISProperty.Name = "cboGISProperty"
        Me.cboGISProperty.Size = New System.Drawing.Size(155, 21)
        Me.cboGISProperty.TabIndex = 9
        '
        'cboUDLChaseCycle
        '
        Me.cboUDLChaseCycle.FormattingEnabled = True
        Me.cboUDLChaseCycle.Location = New System.Drawing.Point(417, 96)
        Me.cboUDLChaseCycle.Name = "cboUDLChaseCycle"
        Me.cboUDLChaseCycle.Size = New System.Drawing.Size(155, 21)
        Me.cboUDLChaseCycle.TabIndex = 10
        '
        'chkCancelledOnly
        '
        Me.chkCancelledOnly.AutoSize = True
        Me.chkCancelledOnly.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.chkCancelledOnly.Location = New System.Drawing.Point(346, 171)
        Me.chkCancelledOnly.Name = "chkCancelledOnly"
        Me.chkCancelledOnly.Size = New System.Drawing.Size(136, 17)
        Me.chkCancelledOnly.TabIndex = 36
        Me.chkCancelledOnly.Text = "Cancelled Policies Only"
        Me.chkCancelledOnly.UseVisualStyleBackColor = True
        '
        'chkIncludeCancelled
        '
        Me.chkIncludeCancelled.AutoSize = True
        Me.chkIncludeCancelled.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.chkIncludeCancelled.Location = New System.Drawing.Point(136, 171)
        Me.chkIncludeCancelled.Name = "chkIncludeCancelled"
        Me.chkIncludeCancelled.Size = New System.Drawing.Size(150, 17)
        Me.chkIncludeCancelled.TabIndex = 13
        Me.chkIncludeCancelled.Text = "Include Cancelled Policies"
        Me.chkIncludeCancelled.UseVisualStyleBackColor = True
        '
        'cboGISDataModel
        '
        Me.cboGISDataModel.DefaultItemId = 0
        Me.cboGISDataModel.FirstItem = ""
        Me.cboGISDataModel.ItemId = 0
        Me.cboGISDataModel.ListIndex = -1
        Me.cboGISDataModel.Location = New System.Drawing.Point(136, 71)
        Me.cboGISDataModel.Name = "cboGISDataModel"
        Me.cboGISDataModel.PMLookupProductFamily = 1
        Me.cboGISDataModel.SingleItemId = 0
        Me.cboGISDataModel.Size = New System.Drawing.Size(161, 21)
        Me.cboGISDataModel.Sorted = True
        Me.cboGISDataModel.TabIndex = 7
        Me.cboGISDataModel.TableName = "Gis_Data_Model"
        Me.cboGISDataModel.Tag = "F;M;"
        Me.cboGISDataModel.ToolTipText = ""
        Me.cboGISDataModel.WhereClause = ""
        '
        'chkUseGreaterTransEffDate
        '
        Me.chkUseGreaterTransEffDate.BackColor = System.Drawing.SystemColors.Control
        Me.chkUseGreaterTransEffDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkUseGreaterTransEffDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUseGreaterTransEffDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkUseGreaterTransEffDate.Location = New System.Drawing.Point(346, 141)
        Me.chkUseGreaterTransEffDate.Name = "chkUseGreaterTransEffDate"
        Me.chkUseGreaterTransEffDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkUseGreaterTransEffDate.Size = New System.Drawing.Size(287, 30)
        Me.chkUseGreaterTransEffDate.TabIndex = 12
        Me.chkUseGreaterTransEffDate.Tag = "CAP;729"
        Me.chkUseGreaterTransEffDate.Text = "Use the greater of Transaction or Policy Effective Date"
        Me.chkUseGreaterTransEffDate.UseVisualStyleBackColor = False
        Me.chkUseGreaterTransEffDate.Visible = False
        '
        'lblGisDataModel
        '
        Me.lblGisDataModel.AutoSize = True
        Me.lblGisDataModel.BackColor = System.Drawing.SystemColors.Control
        Me.lblGisDataModel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGisDataModel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGisDataModel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGisDataModel.Location = New System.Drawing.Point(8, 74)
        Me.lblGisDataModel.Name = "lblGisDataModel"
        Me.lblGisDataModel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGisDataModel.Size = New System.Drawing.Size(83, 13)
        Me.lblGisDataModel.TabIndex = 6
        Me.lblGisDataModel.Tag = "CAP;151"
        Me.lblGisDataModel.Text = "GIS Data Model"
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(8, 20)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(60, 13)
        Me.lblDescription.TabIndex = 1
        Me.lblDescription.Tag = "CAP;150"
        Me.lblDescription.Text = "Description"
        '
        'lblPMLookupSource
        '
        Me.lblPMLookupSource.AutoSize = True
        Me.lblPMLookupSource.BackColor = System.Drawing.SystemColors.Control
        Me.lblPMLookupSource.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPMLookupSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPMLookupSource.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPMLookupSource.Location = New System.Drawing.Point(8, 47)
        Me.lblPMLookupSource.Name = "lblPMLookupSource"
        Me.lblPMLookupSource.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPMLookupSource.Size = New System.Drawing.Size(41, 13)
        Me.lblPMLookupSource.TabIndex = 3
        Me.lblPMLookupSource.Tag = "CAP;502"
        Me.lblPMLookupSource.Text = "Branch"
        '
        'lblProcessingDays
        '
        Me.lblProcessingDays.AutoSize = True
        Me.lblProcessingDays.BackColor = System.Drawing.SystemColors.Control
        Me.lblProcessingDays.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProcessingDays.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProcessingDays.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProcessingDays.Location = New System.Drawing.Point(8, 101)
        Me.lblProcessingDays.Name = "lblProcessingDays"
        Me.lblProcessingDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProcessingDays.Size = New System.Drawing.Size(86, 13)
        Me.lblProcessingDays.TabIndex = 8
        Me.lblProcessingDays.Tag = "CAP;529"
        Me.lblProcessingDays.Text = "Processing Days"
        '
        '_txtHelper_1
        '
        Me._txtHelper_1.AcceptsReturn = True
        Me._txtHelper_1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me._txtHelper_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me._txtHelper_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtHelper_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtHelper_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtHelper_1.Location = New System.Drawing.Point(408, -20)
        Me._txtHelper_1.MaxLength = 0
        Me._txtHelper_1.Multiline = True
        Me._txtHelper_1.Name = "_txtHelper_1"
        Me._txtHelper_1.ReadOnly = True
        Me._txtHelper_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtHelper_1.Size = New System.Drawing.Size(201, 57)
        Me._txtHelper_1.TabIndex = 32
        Me._txtHelper_1.Visible = False
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(136, 17)
        Me.txtDescription.MaxLength = 50
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(257, 20)
        Me.txtDescription.TabIndex = 2
        Me.txtDescription.Tag = "F;M;"
        '
        'txtProcessingDays
        '
        Me.txtProcessingDays.AcceptsReturn = True
        Me.txtProcessingDays.BackColor = System.Drawing.SystemColors.Window
        Me.txtProcessingDays.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtProcessingDays.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtProcessingDays.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtProcessingDays.Location = New System.Drawing.Point(136, 98)
        Me.txtProcessingDays.MaxLength = 4
        Me.txtProcessingDays.Name = "txtProcessingDays"
        Me.txtProcessingDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtProcessingDays.Size = New System.Drawing.Size(81, 20)
        Me.txtProcessingDays.TabIndex = 9
        Me.txtProcessingDays.Tag = "F;M;"
        '
        'cboProduct
        '
        Me.cboProduct.DefaultItemId = 0
        Me.cboProduct.FirstItem = ""
        Me.cboProduct.ItemId = 0
        Me.cboProduct.ListIndex = -1
        Me.cboProduct.Location = New System.Drawing.Point(417, 43)
        Me.cboProduct.Name = "cboProduct"
        Me.cboProduct.PMLookupProductFamily = 1
        Me.cboProduct.SingleItemId = 0
        Me.cboProduct.Size = New System.Drawing.Size(155, 21)
        Me.cboProduct.Sorted = True
        Me.cboProduct.TabIndex = 5
        Me.cboProduct.TableName = "Product"
        Me.cboProduct.Tag = "F;"
        Me.cboProduct.ToolTipText = ""
        Me.cboProduct.WhereClause = ""
        '
        'chkActive
        '
        Me.chkActive.BackColor = System.Drawing.SystemColors.Control
        Me.chkActive.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkActive.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkActive.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkActive.Location = New System.Drawing.Point(136, 124)
        Me.chkActive.Name = "chkActive"
        Me.chkActive.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkActive.Size = New System.Drawing.Size(117, 17)
        Me.chkActive.TabIndex = 10
        Me.chkActive.Tag = "CAP;153"
        Me.chkActive.Text = "Active"
        Me.chkActive.UseVisualStyleBackColor = False
        '
        'cboPMLookupSource
        '
        Me.cboPMLookupSource.DefaultItemId = 0
        Me.cboPMLookupSource.FirstItem = ""
        Me.cboPMLookupSource.ItemId = 0
        Me.cboPMLookupSource.ListIndex = -1
        Me.cboPMLookupSource.Location = New System.Drawing.Point(136, 43)
        Me.cboPMLookupSource.Name = "cboPMLookupSource"
        Me.cboPMLookupSource.PMLookupProductFamily = 1
        Me.cboPMLookupSource.SingleItemId = 0
        Me.cboPMLookupSource.Size = New System.Drawing.Size(161, 21)
        Me.cboPMLookupSource.Sorted = True
        Me.cboPMLookupSource.TabIndex = 4
        Me.cboPMLookupSource.TableName = "Source"
        Me.cboPMLookupSource.Tag = "F;M;"
        Me.cboPMLookupSource.ToolTipText = ""
        Me.cboPMLookupSource.WhereClause = ""
        '
        '_fraCashDrawer_0
        '
        Me._fraCashDrawer_0.BackColor = System.Drawing.SystemColors.Control
        Me._fraCashDrawer_0.Controls.Add(Me.cmdAddStep)
        Me._fraCashDrawer_0.Controls.Add(Me.cmdEditStep)
        Me._fraCashDrawer_0.Controls.Add(Me.cmdDeleteStep)
        Me._fraCashDrawer_0.Controls.Add(Me.lvwSteps)
        Me._fraCashDrawer_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraCashDrawer_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraCashDrawer_0.Location = New System.Drawing.Point(16, 191)
        Me._fraCashDrawer_0.Name = "_fraCashDrawer_0"
        Me._fraCashDrawer_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraCashDrawer_0.Size = New System.Drawing.Size(617, 229)
        Me._fraCashDrawer_0.TabIndex = 15
        Me._fraCashDrawer_0.TabStop = False
        Me._fraCashDrawer_0.Tag = "CAP;315"
        Me._fraCashDrawer_0.Text = "Steps"
        '
        'cmdAddStep
        '
        Me.cmdAddStep.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddStep.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddStep.Enabled = False
        Me.cmdAddStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddStep.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddStep.Location = New System.Drawing.Point(8, 196)
        Me.cmdAddStep.Name = "cmdAddStep"
        Me.cmdAddStep.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddStep.Size = New System.Drawing.Size(57, 22)
        Me.cmdAddStep.TabIndex = 17
        Me.cmdAddStep.Tag = "CAP;203"
        Me.cmdAddStep.Text = "Add"
        Me.cmdAddStep.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddStep.UseVisualStyleBackColor = False
        '
        'cmdEditStep
        '
        Me.cmdEditStep.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditStep.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditStep.Enabled = False
        Me.cmdEditStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditStep.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditStep.Location = New System.Drawing.Point(72, 196)
        Me.cmdEditStep.Name = "cmdEditStep"
        Me.cmdEditStep.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditStep.Size = New System.Drawing.Size(57, 22)
        Me.cmdEditStep.TabIndex = 18
        Me.cmdEditStep.Tag = "CAP;205"
        Me.cmdEditStep.Text = "Edit"
        Me.cmdEditStep.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditStep.UseVisualStyleBackColor = False
        '
        'cmdDeleteStep
        '
        Me.cmdDeleteStep.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteStep.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteStep.Enabled = False
        Me.cmdDeleteStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteStep.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteStep.Location = New System.Drawing.Point(136, 196)
        Me.cmdDeleteStep.Name = "cmdDeleteStep"
        Me.cmdDeleteStep.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteStep.Size = New System.Drawing.Size(57, 22)
        Me.cmdDeleteStep.TabIndex = 19
        Me.cmdDeleteStep.Tag = "CAP;204"
        Me.cmdDeleteStep.Text = "Delete"
        Me.cmdDeleteStep.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteStep.UseVisualStyleBackColor = False
        '
        'chkUseEffectiveDate
        '
        Me.chkUseEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.chkUseEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkUseEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUseEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkUseEffectiveDate.Location = New System.Drawing.Point(136, 148)
        Me.chkUseEffectiveDate.Name = "chkUseEffectiveDate"
        Me.chkUseEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkUseEffectiveDate.Size = New System.Drawing.Size(161, 17)
        Me.chkUseEffectiveDate.TabIndex = 11
        Me.chkUseEffectiveDate.Tag = "CAP;154"
        Me.chkUseEffectiveDate.Text = "Use Effective Date"
        Me.chkUseEffectiveDate.UseVisualStyleBackColor = False
        '
        '_txtHelper_2
        '
        Me._txtHelper_2.AcceptsReturn = True
        Me._txtHelper_2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me._txtHelper_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me._txtHelper_2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtHelper_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtHelper_2.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtHelper_2.Location = New System.Drawing.Point(294, 463)
        Me._txtHelper_2.MaxLength = 0
        Me._txtHelper_2.Multiline = True
        Me._txtHelper_2.Name = "_txtHelper_2"
        Me._txtHelper_2.ReadOnly = True
        Me._txtHelper_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtHelper_2.Size = New System.Drawing.Size(125, 33)
        Me._txtHelper_2.TabIndex = 33
        Me._txtHelper_2.Visible = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(128, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(653, 453)
        Me.tabMainTab.TabIndex = 0
        '
        'lblUDLChaseCycle
        '
        Me.lblUDLChaseCycle.AutoSize = True
        Me.lblUDLChaseCycle.BackColor = System.Drawing.SystemColors.Control
        Me.lblUDLChaseCycle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUDLChaseCycle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUDLChaseCycle.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUDLChaseCycle.Location = New System.Drawing.Point(303, 98)
        Me.lblUDLChaseCycle.Name = "lblUDLChaseCycle"
        Me.lblUDLChaseCycle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUDLChaseCycle.Size = New System.Drawing.Size(117, 13)
        Me.lblUDLChaseCycle.TabIndex = 13
        Me.lblUDLChaseCycle.Tag = "CAP;152"
        Me.lblUDLChaseCycle.Text = "Chase Cycle Status"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(303, 47)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(51, 13)
        Me.Label1.TabIndex = 34
        Me.Label1.Tag = "CAP;155"
        Me.Label1.Text = "Product"
        '
        'lblGISProperty
        '
        Me.lblGISProperty.AutoSize = True
        Me.lblGISProperty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblGISProperty.Location = New System.Drawing.Point(303, 74)
        Me.lblGISProperty.Name = "lblGISProperty"
        Me.lblGISProperty.Size = New System.Drawing.Size(54, 13)
        Me.lblGISProperty.TabIndex = 37
        Me.lblGISProperty.Text = "Property"
        '
        'frmDetails
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(665, 496)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me._txtHelper_2)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDetails"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Add / Edit Chase Cycle Rule"
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me._fraCashDrawer_0.ResumeLayout(False)
        Me.tabMainTab.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializetxtHelper()
        Me.txtHelper(2) = _txtHelper_2
        Me.txtHelper(1) = _txtHelper_1
    End Sub
    Sub InitializefraCashDrawer()
        Me.fraCashDrawer(0) = _fraCashDrawer_0
    End Sub
    Sub lvwSteps_InitializeColumnKeys()
        Me._lvwSteps_ColumnHeader_1.Name = ""
        Me._lvwSteps_ColumnHeader_2.Name = ""
        'Me._lvwSteps_ColumnHeader_3.Name = ""
        'Me._lvwSteps_ColumnHeader_4.Name = ""
        'Me._lvwSteps_ColumnHeader_5.Name = ""
        'Me._lvwSteps_ColumnHeader_6.Name = ""
        Me._lvwSteps_ColumnHeader_7.Name = ""
        Me._lvwSteps_ColumnHeader_8.Name = ""
    End Sub
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents chkUseGreaterTransEffDate As System.Windows.Forms.CheckBox
    Public WithEvents lblGisDataModel As System.Windows.Forms.Label
    Public WithEvents lblDescription As System.Windows.Forms.Label
    Public WithEvents lblPMLookupSource As System.Windows.Forms.Label
    Public WithEvents lblProcessingDays As System.Windows.Forms.Label
    Private WithEvents _txtHelper_1 As System.Windows.Forms.TextBox
    Public WithEvents txtDescription As System.Windows.Forms.TextBox
    Public WithEvents txtProcessingDays As System.Windows.Forms.TextBox
    Public WithEvents cboProduct As PMLookupControl.cboPMLookup
    Public WithEvents chkActive As System.Windows.Forms.CheckBox
    Public WithEvents cboPMLookupSource As PMLookupControl.cboPMLookup
    Private WithEvents _fraCashDrawer_0 As System.Windows.Forms.GroupBox
    Public WithEvents cmdAddStep As System.Windows.Forms.Button
    Public WithEvents cmdEditStep As System.Windows.Forms.Button
    Public WithEvents cmdDeleteStep As System.Windows.Forms.Button
    Private WithEvents _txtHelper_2 As System.Windows.Forms.TextBox
    Public WithEvents lvwSteps As System.Windows.Forms.ListView
    Private WithEvents _lvwSteps_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSteps_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSteps_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSteps_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSteps_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
    Public WithEvents chkUseEffectiveDate As System.Windows.Forms.CheckBox
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public WithEvents cboGISDataModel As PMLookupControl.cboPMLookup
    Private WithEvents chkIncludeCancelled As System.Windows.Forms.CheckBox
    Private WithEvents chkCancelledOnly As System.Windows.Forms.CheckBox
    Friend WithEvents cboUDLChaseCycle As System.Windows.Forms.ComboBox
    Friend WithEvents cboGISProperty As System.Windows.Forms.ComboBox
    Public WithEvents lblUDLChaseCycle As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblGISProperty As System.Windows.Forms.Label
#End Region
End Class