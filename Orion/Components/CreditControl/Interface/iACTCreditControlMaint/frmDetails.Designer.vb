<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
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
    Public WithEvents cmdApply As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents lblBusinessType As System.Windows.Forms.Label
    Public WithEvents lblDescription As System.Windows.Forms.Label
    Public WithEvents lblPMLookupSource As System.Windows.Forms.Label
    Public WithEvents lblProcessingDays As System.Windows.Forms.Label
    Public WithEvents lblPMLookupPFFrequency As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents cboProduct As PMLookupControl.cboPMLookup
    Public WithEvents cboPMLookupPFFrequency As PMLookupControl.cboPMLookup
    Public WithEvents txtDescription As System.Windows.Forms.TextBox
    Public WithEvents chkActive As System.Windows.Forms.CheckBox
    Public WithEvents cboPMLookupSource As PMLookupControl.cboPMLookup
    Private WithEvents _txtHelper_1 As System.Windows.Forms.TextBox
    Public WithEvents cmdAddStep As System.Windows.Forms.Button
    Public WithEvents cmdEditStep As System.Windows.Forms.Button
    Public WithEvents cmdDeleteStep As System.Windows.Forms.Button
    Private WithEvents _txtHelper_2 As System.Windows.Forms.TextBox
    Private WithEvents _lvwSteps_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSteps_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSteps_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSteps_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSteps_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSteps_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSteps_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSteps_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwSteps As System.Windows.Forms.ListView
    Private WithEvents _fraCashDrawer_0 As System.Windows.Forms.GroupBox
    Public WithEvents cboBusinessType As System.Windows.Forms.ComboBox
    Public WithEvents chkUseEffectiveDate As System.Windows.Forms.CheckBox
    Public WithEvents chkUseGreaterTransEffDate As System.Windows.Forms.CheckBox
    Public WithEvents txtProcessingDays As System.Windows.Forms.TextBox
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents cboPMLookupInstalmentResult As PMLookupControl.cboPMLookup
    Public WithEvents lblPMLookupInstalmentResult As System.Windows.Forms.Label
    Public WithEvents fraInstalmentScheme As System.Windows.Forms.GroupBox
    Public WithEvents chkUnpaid As System.Windows.Forms.CheckBox
    Public WithEvents chkPaid As System.Windows.Forms.CheckBox
    Public WithEvents fraPaidPostion As System.Windows.Forms.GroupBox
    Public WithEvents chklbInsuranceFileStatus As System.Windows.Forms.CheckedListBox
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Public WithEvents fraInsuranceFileFilters As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public fraCashDrawer(0) As System.Windows.Forms.GroupBox
    Public txtHelper(2) As System.Windows.Forms.TextBox
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    Private WithEvents listBoxHelper1 As Artinsoft.VB6.Gui.ListBoxHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdApply = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.chkUseInceptionDate = New System.Windows.Forms.CheckBox
        Me.chkRenewalPrintDate = New System.Windows.Forms.CheckBox
        Me.chkUseDueDate = New System.Windows.Forms.CheckBox
        Me.chkUseGreaterTransEffDate = New System.Windows.Forms.CheckBox
        Me.lblBusinessType = New System.Windows.Forms.Label
        Me.lblDescription = New System.Windows.Forms.Label
        Me.lblPMLookupSource = New System.Windows.Forms.Label
        Me.lblProcessingDays = New System.Windows.Forms.Label
        Me._txtHelper_1 = New System.Windows.Forms.TextBox
        Me.lblPMLookupPFFrequency = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.cboProduct = New PMLookupControl.cboPMLookup
        Me.cboPMLookupPFFrequency = New PMLookupControl.cboPMLookup
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.chkActive = New System.Windows.Forms.CheckBox
        Me.cboPMLookupSource = New PMLookupControl.cboPMLookup
        Me._fraCashDrawer_0 = New System.Windows.Forms.GroupBox
        Me.cmdAddStep = New System.Windows.Forms.Button
        Me.cmdEditStep = New System.Windows.Forms.Button
        Me.cmdDeleteStep = New System.Windows.Forms.Button
        Me._txtHelper_2 = New System.Windows.Forms.TextBox
        Me.lvwSteps = New System.Windows.Forms.ListView
        Me._lvwSteps_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwSteps_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwSteps_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwSteps_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwSteps_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwSteps_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwSteps_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._lvwSteps_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
        Me.cboBusinessType = New System.Windows.Forms.ComboBox
        Me.chkUseEffectiveDate = New System.Windows.Forms.CheckBox
        Me.txtProcessingDays = New System.Windows.Forms.TextBox
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me.fraInstalmentScheme = New System.Windows.Forms.GroupBox
        Me.cboPMLookupInstalmentResult = New PMLookupControl.cboPMLookup
        Me.lblPMLookupInstalmentResult = New System.Windows.Forms.Label
        Me.fraInsuranceFileFilters = New System.Windows.Forms.GroupBox
        Me.fraPaidPostion = New System.Windows.Forms.GroupBox
        Me.chkUnpaid = New System.Windows.Forms.CheckBox
        Me.chkPaid = New System.Windows.Forms.CheckBox
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.chklbInsuranceFileStatus = New System.Windows.Forms.CheckedListBox
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.listBoxHelper1 = New Artinsoft.VB6.Gui.ListBoxHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me._fraCashDrawer_0.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me.fraInstalmentScheme.SuspendLayout()
        Me.fraInsuranceFileFilters.SuspendLayout()
        Me.fraPaidPostion.SuspendLayout()
        Me.Frame1.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApply.Enabled = False
        Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Location = New System.Drawing.Point(504, 466)
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
        Me.cmdCancel.Location = New System.Drawing.Point(584, 466)
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
        Me.cmdOK.Location = New System.Drawing.Point(424, 466)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 29
        Me.cmdOK.Tag = "CAP;209"
        Me.cmdOK.Text = "OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage1)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(128, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(653, 452)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkUseInceptionDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkRenewalPrintDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkUseDueDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkUseGreaterTransEffDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblBusinessType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPMLookupSource)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblProcessingDays)
        Me._tabMainTab_TabPage0.Controls.Add(Me._txtHelper_1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPMLookupPFFrequency)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Label1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboProduct)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboPMLookupPFFrequency)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkActive)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboPMLookupSource)
        Me._tabMainTab_TabPage0.Controls.Add(Me._fraCashDrawer_0)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboBusinessType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkUseEffectiveDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtProcessingDays)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(645, 426)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Detail"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'chkUseInceptionDate
        '
        Me.chkUseInceptionDate.BackColor = System.Drawing.SystemColors.Control
        Me.chkUseInceptionDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkUseInceptionDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUseInceptionDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkUseInceptionDate.Location = New System.Drawing.Point(136, 171)
        Me.chkUseInceptionDate.Name = "chkUseInceptionDate"
        Me.chkUseInceptionDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkUseInceptionDate.Size = New System.Drawing.Size(202, 20)
        Me.chkUseInceptionDate.TabIndex = 36
        Me.chkUseInceptionDate.Tag = "CAP"
        Me.chkUseInceptionDate.Text = "Use Inception Date For Cancellation"
        Me.chkUseInceptionDate.UseVisualStyleBackColor = False
        '
        'chkRenewalPrintDate
        '
        Me.chkRenewalPrintDate.AutoSize = True
        Me.chkRenewalPrintDate.Location = New System.Drawing.Point(346, 148)
        Me.chkRenewalPrintDate.Name = "chkRenewalPrintDate"
        Me.chkRenewalPrintDate.Size = New System.Drawing.Size(160, 17)
        Me.chkRenewalPrintDate.TabIndex = 36
        Me.chkRenewalPrintDate.Text = "Use Renewal Print Date"
        Me.chkRenewalPrintDate.UseVisualStyleBackColor = True
        '
        'chkUseDueDate
        '
        Me.chkUseDueDate.BackColor = System.Drawing.SystemColors.Control
        Me.chkUseDueDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkUseDueDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUseDueDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkUseDueDate.Location = New System.Drawing.Point(346, 148)
        Me.chkUseDueDate.Name = "chkUseDueDate"
        Me.chkUseDueDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkUseDueDate.Size = New System.Drawing.Size(161, 17)
        Me.chkUseDueDate.TabIndex = 35
        Me.chkUseDueDate.Tag = "CAP;156"
        Me.chkUseDueDate.Text = "Use Due Date"
        Me.chkUseDueDate.UseVisualStyleBackColor = False
        '
        'chkUseGreaterTransEffDate
        '
        Me.chkUseGreaterTransEffDate.BackColor = System.Drawing.SystemColors.Control
        Me.chkUseGreaterTransEffDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkUseGreaterTransEffDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUseGreaterTransEffDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkUseGreaterTransEffDate.Location = New System.Drawing.Point(346, 117)
        Me.chkUseGreaterTransEffDate.Name = "chkUseGreaterTransEffDate"
        Me.chkUseGreaterTransEffDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkUseGreaterTransEffDate.Size = New System.Drawing.Size(287, 30)
        Me.chkUseGreaterTransEffDate.TabIndex = 12
        Me.chkUseGreaterTransEffDate.Tag = "CAP;729"
        Me.chkUseGreaterTransEffDate.Text = "Use the greater of Transaction or Policy Effective Date"
        Me.chkUseGreaterTransEffDate.UseVisualStyleBackColor = False
        Me.chkUseGreaterTransEffDate.Visible = False
        '
        'lblBusinessType
        '
        Me.lblBusinessType.AutoSize = True
        Me.lblBusinessType.BackColor = System.Drawing.SystemColors.Control
        Me.lblBusinessType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBusinessType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBusinessType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBusinessType.Location = New System.Drawing.Point(8, 74)
        Me.lblBusinessType.Name = "lblBusinessType"
        Me.lblBusinessType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBusinessType.Size = New System.Drawing.Size(52, 13)
        Me.lblBusinessType.TabIndex = 6
        Me.lblBusinessType.Tag = "CAP;151"
        Me.lblBusinessType.Text = "Bus Type"
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
        'lblPMLookupPFFrequency
        '
        Me.lblPMLookupPFFrequency.AutoSize = True
        Me.lblPMLookupPFFrequency.BackColor = System.Drawing.SystemColors.Control
        Me.lblPMLookupPFFrequency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPMLookupPFFrequency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPMLookupPFFrequency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPMLookupPFFrequency.Location = New System.Drawing.Point(312, 74)
        Me.lblPMLookupPFFrequency.Name = "lblPMLookupPFFrequency"
        Me.lblPMLookupPFFrequency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPMLookupPFFrequency.Size = New System.Drawing.Size(57, 13)
        Me.lblPMLookupPFFrequency.TabIndex = 13
        Me.lblPMLookupPFFrequency.Tag = "CAP;152"
        Me.lblPMLookupPFFrequency.Text = "Frequency"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(312, 48)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(44, 13)
        Me.Label1.TabIndex = 34
        Me.Label1.Tag = "CAP;155"
        Me.Label1.Text = "Product"
        '
        'cboProduct
        '
        Me.cboProduct.DefaultItemId = 0
        Me.cboProduct.FirstItem = ""
        Me.cboProduct.ItemId = 0
        Me.cboProduct.ListIndex = -1
        Me.cboProduct.Location = New System.Drawing.Point(408, 44)
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
        'cboPMLookupPFFrequency
        '
        Me.cboPMLookupPFFrequency.DefaultItemId = 0
        Me.cboPMLookupPFFrequency.FirstItem = ""
        Me.cboPMLookupPFFrequency.ItemId = 0
        Me.cboPMLookupPFFrequency.ListIndex = -1
        Me.cboPMLookupPFFrequency.Location = New System.Drawing.Point(408, 70)
        Me.cboPMLookupPFFrequency.Name = "cboPMLookupPFFrequency"
        Me.cboPMLookupPFFrequency.PMLookupProductFamily = 1
        Me.cboPMLookupPFFrequency.SingleItemId = 0
        Me.cboPMLookupPFFrequency.Size = New System.Drawing.Size(225, 21)
        Me.cboPMLookupPFFrequency.Sorted = True
        Me.cboPMLookupPFFrequency.TabIndex = 14
        Me.cboPMLookupPFFrequency.TableName = "pffrequency"
        Me.cboPMLookupPFFrequency.Tag = "F;"
        Me.cboPMLookupPFFrequency.ToolTipText = ""
        Me.cboPMLookupPFFrequency.WhereClause = ""
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
        Me.cboPMLookupSource.Size = New System.Drawing.Size(153, 21)
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
        Me._fraCashDrawer_0.Controls.Add(Me._txtHelper_2)
        Me._fraCashDrawer_0.Controls.Add(Me.lvwSteps)
        Me._fraCashDrawer_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraCashDrawer_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraCashDrawer_0.Location = New System.Drawing.Point(16, 190)
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
        '_txtHelper_2
        '
        Me._txtHelper_2.AcceptsReturn = True
        Me._txtHelper_2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me._txtHelper_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me._txtHelper_2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtHelper_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtHelper_2.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtHelper_2.Location = New System.Drawing.Point(476, 50)
        Me._txtHelper_2.MaxLength = 0
        Me._txtHelper_2.Multiline = True
        Me._txtHelper_2.Name = "_txtHelper_2"
        Me._txtHelper_2.ReadOnly = True
        Me._txtHelper_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtHelper_2.Size = New System.Drawing.Size(125, 41)
        Me._txtHelper_2.TabIndex = 33
        Me._txtHelper_2.Visible = False
        '
        'lvwSteps
        '
        Me.lvwSteps.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwSteps, "")
        Me.lvwSteps.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSteps_ColumnHeader_1, Me._lvwSteps_ColumnHeader_2, Me._lvwSteps_ColumnHeader_3, Me._lvwSteps_ColumnHeader_4, Me._lvwSteps_ColumnHeader_5, Me._lvwSteps_ColumnHeader_6, Me._lvwSteps_ColumnHeader_7, Me._lvwSteps_ColumnHeader_8})
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
        '_lvwSteps_ColumnHeader_3
        '
        Me._lvwSteps_ColumnHeader_3.Tag = ""
        Me._lvwSteps_ColumnHeader_3.Text = "Broker Days"
        Me._lvwSteps_ColumnHeader_3.Width = 67
        '
        '_lvwSteps_ColumnHeader_4
        '
        Me._lvwSteps_ColumnHeader_4.Tag = ""
        Me._lvwSteps_ColumnHeader_4.Text = "Policy Amt"
        Me._lvwSteps_ColumnHeader_4.Width = 61
        '
        '_lvwSteps_ColumnHeader_5
        '
        Me._lvwSteps_ColumnHeader_5.Tag = ""
        Me._lvwSteps_ColumnHeader_5.Text = "Account Amt"
        Me._lvwSteps_ColumnHeader_5.Width = 67
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
        'cboBusinessType
        '
        Me.cboBusinessType.BackColor = System.Drawing.SystemColors.Window
        Me.cboBusinessType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBusinessType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBusinessType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBusinessType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBusinessType.Location = New System.Drawing.Point(136, 70)
        Me.cboBusinessType.Name = "cboBusinessType"
        Me.cboBusinessType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBusinessType.Size = New System.Drawing.Size(153, 21)
        Me.cboBusinessType.TabIndex = 7
        Me.cboBusinessType.Tag = "F;M;"
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
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraInstalmentScheme)
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraInsuranceFileFilters)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(645, 426)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "2 - Instalment Import Configuration"
        Me._tabMainTab_TabPage1.UseVisualStyleBackColor = True
        '
        'fraInstalmentScheme
        '
        Me.fraInstalmentScheme.BackColor = System.Drawing.SystemColors.Control
        Me.fraInstalmentScheme.Controls.Add(Me.cboPMLookupInstalmentResult)
        Me.fraInstalmentScheme.Controls.Add(Me.lblPMLookupInstalmentResult)
        Me.fraInstalmentScheme.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraInstalmentScheme.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraInstalmentScheme.Location = New System.Drawing.Point(8, 12)
        Me.fraInstalmentScheme.Name = "fraInstalmentScheme"
        Me.fraInstalmentScheme.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraInstalmentScheme.Size = New System.Drawing.Size(633, 65)
        Me.fraInstalmentScheme.TabIndex = 20
        Me.fraInstalmentScheme.TabStop = False
        Me.fraInstalmentScheme.Text = "Instalment Details : This filter only applies to the instalment import rejection " &
            "process"
        '
        'cboPMLookupInstalmentResult
        '
        Me.cboPMLookupInstalmentResult.DefaultItemId = 0
        Me.cboPMLookupInstalmentResult.FirstItem = ""
        Me.cboPMLookupInstalmentResult.ItemId = 0
        Me.cboPMLookupInstalmentResult.ListIndex = -1
        Me.cboPMLookupInstalmentResult.Location = New System.Drawing.Point(160, 24)
        Me.cboPMLookupInstalmentResult.Name = "cboPMLookupInstalmentResult"
        Me.cboPMLookupInstalmentResult.PMLookupProductFamily = 1
        Me.cboPMLookupInstalmentResult.SingleItemId = 0
        Me.cboPMLookupInstalmentResult.Size = New System.Drawing.Size(337, 21)
        Me.cboPMLookupInstalmentResult.Sorted = True
        Me.cboPMLookupInstalmentResult.TabIndex = 22
        Me.cboPMLookupInstalmentResult.TableName = "pfInstalments_Result"
        Me.cboPMLookupInstalmentResult.ToolTipText = ""
        Me.cboPMLookupInstalmentResult.WhereClause = ""
        '
        'lblPMLookupInstalmentResult
        '
        Me.lblPMLookupInstalmentResult.AutoSize = True
        Me.lblPMLookupInstalmentResult.BackColor = System.Drawing.SystemColors.Control
        Me.lblPMLookupInstalmentResult.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPMLookupInstalmentResult.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPMLookupInstalmentResult.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPMLookupInstalmentResult.Location = New System.Drawing.Point(16, 28)
        Me.lblPMLookupInstalmentResult.Name = "lblPMLookupInstalmentResult"
        Me.lblPMLookupInstalmentResult.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPMLookupInstalmentResult.Size = New System.Drawing.Size(122, 13)
        Me.lblPMLookupInstalmentResult.TabIndex = 21
        Me.lblPMLookupInstalmentResult.Text = "Payment Failed Reason:"
        '
        'fraInsuranceFileFilters
        '
        Me.fraInsuranceFileFilters.BackColor = System.Drawing.SystemColors.Control
        Me.fraInsuranceFileFilters.Controls.Add(Me.fraPaidPostion)
        Me.fraInsuranceFileFilters.Controls.Add(Me.Frame1)
        Me.fraInsuranceFileFilters.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraInsuranceFileFilters.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraInsuranceFileFilters.Location = New System.Drawing.Point(8, 84)
        Me.fraInsuranceFileFilters.Name = "fraInsuranceFileFilters"
        Me.fraInsuranceFileFilters.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraInsuranceFileFilters.Size = New System.Drawing.Size(633, 273)
        Me.fraInsuranceFileFilters.TabIndex = 23
        Me.fraInsuranceFileFilters.TabStop = False
        Me.fraInsuranceFileFilters.Text = "Policy Detail Filters -  These filters only apply to the instalment import cancel" &
            "lation process"
        '
        'fraPaidPostion
        '
        Me.fraPaidPostion.BackColor = System.Drawing.SystemColors.Control
        Me.fraPaidPostion.Controls.Add(Me.chkUnpaid)
        Me.fraPaidPostion.Controls.Add(Me.chkPaid)
        Me.fraPaidPostion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPaidPostion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPaidPostion.Location = New System.Drawing.Point(8, 24)
        Me.fraPaidPostion.Name = "fraPaidPostion"
        Me.fraPaidPostion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPaidPostion.Size = New System.Drawing.Size(617, 57)
        Me.fraPaidPostion.TabIndex = 24
        Me.fraPaidPostion.TabStop = False
        Me.fraPaidPostion.Text = "Paid Position"
        '
        'chkUnpaid
        '
        Me.chkUnpaid.BackColor = System.Drawing.SystemColors.Control
        Me.chkUnpaid.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkUnpaid.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkUnpaid.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUnpaid.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkUnpaid.Location = New System.Drawing.Point(80, 16)
        Me.chkUnpaid.Name = "chkUnpaid"
        Me.chkUnpaid.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkUnpaid.Size = New System.Drawing.Size(65, 25)
        Me.chkUnpaid.TabIndex = 26
        Me.chkUnpaid.Text = "Unpaid"
        Me.chkUnpaid.UseVisualStyleBackColor = False
        '
        'chkPaid
        '
        Me.chkPaid.BackColor = System.Drawing.SystemColors.Control
        Me.chkPaid.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkPaid.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPaid.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPaid.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPaid.Location = New System.Drawing.Point(16, 16)
        Me.chkPaid.Name = "chkPaid"
        Me.chkPaid.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPaid.Size = New System.Drawing.Size(49, 25)
        Me.chkPaid.TabIndex = 25
        Me.chkPaid.Text = "Paid:"
        Me.chkPaid.UseVisualStyleBackColor = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.chklbInsuranceFileStatus)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(8, 88)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(617, 177)
        Me.Frame1.TabIndex = 27
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Policy Status Filters - Where no filters are selected a status of ""Live"" is assum" &
            "ed"
        '
        'chklbInsuranceFileStatus
        '
        Me.chklbInsuranceFileStatus.BackColor = System.Drawing.SystemColors.Window
        Me.chklbInsuranceFileStatus.CheckOnClick = True
        Me.chklbInsuranceFileStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.chklbInsuranceFileStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chklbInsuranceFileStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.chklbInsuranceFileStatus.Location = New System.Drawing.Point(8, 16)
        Me.chklbInsuranceFileStatus.Name = "chklbInsuranceFileStatus"
        Me.chklbInsuranceFileStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.listBoxHelper1.SetSelectionMode(Me.chklbInsuranceFileStatus, System.Windows.Forms.SelectionMode.One)
        Me.chklbInsuranceFileStatus.Size = New System.Drawing.Size(601, 124)
        Me.chklbInsuranceFileStatus.TabIndex = 28
        '
        'frmDetails
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(665, 495)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
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
        Me.Text = "Add / Edit Credit Control Rule"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me._fraCashDrawer_0.ResumeLayout(False)
        Me._fraCashDrawer_0.PerformLayout()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me.fraInstalmentScheme.ResumeLayout(False)
        Me.fraInstalmentScheme.PerformLayout()
        Me.fraInsuranceFileFilters.ResumeLayout(False)
        Me.fraPaidPostion.ResumeLayout(False)
        Me.Frame1.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

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
        Me._lvwSteps_ColumnHeader_3.Name = ""
        Me._lvwSteps_ColumnHeader_4.Name = ""
        Me._lvwSteps_ColumnHeader_5.Name = ""
        Me._lvwSteps_ColumnHeader_6.Name = ""
        Me._lvwSteps_ColumnHeader_7.Name = ""
        Me._lvwSteps_ColumnHeader_8.Name = ""
    End Sub
    Public WithEvents chkUseDueDate As System.Windows.Forms.CheckBox
    Public WithEvents chkUseInceptionDate As System.Windows.Forms.CheckBox
    Friend WithEvents chkRenewalPrintDate As System.Windows.Forms.CheckBox
#End Region
End Class