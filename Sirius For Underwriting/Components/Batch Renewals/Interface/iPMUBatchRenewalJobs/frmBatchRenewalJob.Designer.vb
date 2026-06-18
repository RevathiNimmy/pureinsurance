<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBatchRenewalJob
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
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents cmdApply As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents uctCreditCard2 As System.Windows.Forms.PictureBox
	Public WithEvents uctPMAddressControl4 As System.Windows.Forms.PictureBox
	Public WithEvents Check2 As System.Windows.Forms.CheckBox
	Public WithEvents Frame2 As System.Windows.Forms.GroupBox
	Public WithEvents Command3 As System.Windows.Forms.Button
	Public WithEvents Command2 As System.Windows.Forms.Button
	Public WithEvents Command1 As System.Windows.Forms.Button
	Public WithEvents uctPMAddressCC As System.Windows.Forms.PictureBox
	Public WithEvents chkIsRegistered As System.Windows.Forms.CheckBox
	Public WithEvents uctCreditCardDetails As System.Windows.Forms.PictureBox
	Public WithEvents Frame5 As System.Windows.Forms.GroupBox
	Public WithEvents lblJobCode As System.Windows.Forms.Label
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents lblJobType As System.Windows.Forms.Label
	Public WithEvents lblSAMServer As System.Windows.Forms.Label
	Public WithEvents lblDaysBeforeRenewalDate As System.Windows.Forms.Label
	Public WithEvents cboJobType As PMLookupControl.cboPMLookup
	Public WithEvents txtJobCode As System.Windows.Forms.TextBox
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents chkIsActive As System.Windows.Forms.CheckBox
	Public WithEvents txtSAMServer As System.Windows.Forms.TextBox
	Public WithEvents txtDaysBeforeRenewalDate As System.Windows.Forms.TextBox
	Public WithEvents cmdCalculate As System.Windows.Forms.Button
	Public WithEvents lblCalculate As System.Windows.Forms.Label
	Public WithEvents fraCurrentConfigurationResults As System.Windows.Forms.GroupBox
	Private WithEvents _SSBatchRenewalJobs_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents chkIncludeDirectPolicies As System.Windows.Forms.CheckBox
	Public WithEvents lvwAgents As System.Windows.Forms.ListView
	Public WithEvents cmdRemoveProduct As System.Windows.Forms.Button
	Public WithEvents cmdAddProduct As System.Windows.Forms.Button
	Public WithEvents optAllAgents As System.Windows.Forms.RadioButton
	Public WithEvents optSelectedAgents As System.Windows.Forms.RadioButton
	Public WithEvents fraAgents As System.Windows.Forms.GroupBox
	Public WithEvents uctPickListProducts As uctPickList.PickList
	Public WithEvents fraProducts As System.Windows.Forms.GroupBox
	Private WithEvents _SSBatchRenewalJobs_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents uctPickListBranches As uctPickList.PickList
	Public WithEvents fraBranches As System.Windows.Forms.GroupBox
	Public WithEvents optNotRequired As System.Windows.Forms.RadioButton
	Public WithEvents optSpool As System.Windows.Forms.RadioButton
	Public WithEvents optPrint As System.Windows.Forms.RadioButton
	Public WithEvents fraPrinting As System.Windows.Forms.GroupBox
	Public WithEvents optPolicyNumber As System.Windows.Forms.RadioButton
	Public WithEvents optClient As System.Windows.Forms.RadioButton
	Public WithEvents fraSortOrder As System.Windows.Forms.GroupBox
	Public WithEvents fraRenewalReportDocuments As System.Windows.Forms.GroupBox
	Private WithEvents _SSBatchRenewalJobs_TabPage2 As System.Windows.Forms.TabPage
	Public WithEvents SSBatchRenewalJobs As System.Windows.Forms.TabControl
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBatchRenewalJob))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOk = New System.Windows.Forms.Button()
        Me.cmdApply = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.SSBatchRenewalJobs = New System.Windows.Forms.TabControl()
        Me._SSBatchRenewalJobs_TabPage0 = New System.Windows.Forms.TabPage()
        Me.lblJobCode = New System.Windows.Forms.Label()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.lblJobType = New System.Windows.Forms.Label()
        Me.lblSAMServer = New System.Windows.Forms.Label()
        Me.lblDaysBeforeRenewalDate = New System.Windows.Forms.Label()
        Me.cboJobType = New PMLookupControl.cboPMLookup()
        Me.txtJobCode = New System.Windows.Forms.TextBox()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.chkIsActive = New System.Windows.Forms.CheckBox()
        Me.txtSAMServer = New System.Windows.Forms.TextBox()
        Me.txtDaysBeforeRenewalDate = New System.Windows.Forms.TextBox()
        Me.fraCurrentConfigurationResults = New System.Windows.Forms.GroupBox()
        Me.cmdCalculate = New System.Windows.Forms.Button()
        Me.lblCalculate = New System.Windows.Forms.Label()
        Me._SSBatchRenewalJobs_TabPage1 = New System.Windows.Forms.TabPage()
        Me.fraAgents = New System.Windows.Forms.GroupBox()
        Me.chkIncludeDirectPolicies = New System.Windows.Forms.CheckBox()
        Me.lvwAgents = New System.Windows.Forms.ListView()
        Me.cmdRemoveProduct = New System.Windows.Forms.Button()
        Me.cmdAddProduct = New System.Windows.Forms.Button()
        Me.optAllAgents = New System.Windows.Forms.RadioButton()
        Me.optSelectedAgents = New System.Windows.Forms.RadioButton()
        Me.fraProducts = New System.Windows.Forms.GroupBox()
        Me.uctPickListProducts = New uctPickList.PickList()
        Me._SSBatchRenewalJobs_TabPage2 = New System.Windows.Forms.TabPage()
        Me.fraBranches = New System.Windows.Forms.GroupBox()
        Me.uctPickListBranches = New uctPickList.PickList()
        Me.fraRenewalReportDocuments = New System.Windows.Forms.GroupBox()
        Me.fraPrinting = New System.Windows.Forms.GroupBox()
        Me.optNotRequired = New System.Windows.Forms.RadioButton()
        Me.optSpool = New System.Windows.Forms.RadioButton()
        Me.optPrint = New System.Windows.Forms.RadioButton()
        Me.fraSortOrder = New System.Windows.Forms.GroupBox()
        Me.optPolicyNumber = New System.Windows.Forms.RadioButton()
        Me.optClient = New System.Windows.Forms.RadioButton()
        Me.Frame2 = New System.Windows.Forms.GroupBox()
        Me.uctCreditCard2 = New System.Windows.Forms.PictureBox()
        Me.uctPMAddressControl4 = New System.Windows.Forms.PictureBox()
        Me.Check2 = New System.Windows.Forms.CheckBox()
        Me.Command3 = New System.Windows.Forms.Button()
        Me.Command2 = New System.Windows.Forms.Button()
        Me.Command1 = New System.Windows.Forms.Button()
        Me.Frame5 = New System.Windows.Forms.GroupBox()
        Me.uctPMAddressCC = New System.Windows.Forms.PictureBox()
        Me.chkIsRegistered = New System.Windows.Forms.CheckBox()
        Me.uctCreditCardDetails = New System.Windows.Forms.PictureBox()
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.chkRunExtendedRule = New System.Windows.Forms.CheckBox()
        Me.SSBatchRenewalJobs.SuspendLayout()
        Me._SSBatchRenewalJobs_TabPage0.SuspendLayout()
        Me.fraCurrentConfigurationResults.SuspendLayout()
        Me._SSBatchRenewalJobs_TabPage1.SuspendLayout()
        Me.fraAgents.SuspendLayout()
        Me.fraProducts.SuspendLayout()
        Me._SSBatchRenewalJobs_TabPage2.SuspendLayout()
        Me.fraBranches.SuspendLayout()
        Me.fraRenewalReportDocuments.SuspendLayout()
        Me.fraPrinting.SuspendLayout()
        Me.fraSortOrder.SuspendLayout()
        Me.Frame2.SuspendLayout()
        CType(Me.uctCreditCard2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uctPMAddressControl4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Frame5.SuspendLayout()
        CType(Me.uctPMAddressCC, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uctCreditCardDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdOk
        '
        Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOk.Location = New System.Drawing.Point(444, 464)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(73, 22)
        Me.cmdOk.TabIndex = 6
        Me.cmdOk.Text = "&OK"
        Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOk.UseVisualStyleBackColor = False
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Location = New System.Drawing.Point(522, 464)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApply.Size = New System.Drawing.Size(73, 22)
        Me.cmdApply.TabIndex = 7
        Me.cmdApply.Text = "&Apply"
        Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdApply.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(600, 464)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 8
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'SSBatchRenewalJobs
        '
        Me.SSBatchRenewalJobs.Controls.Add(Me._SSBatchRenewalJobs_TabPage0)
        Me.SSBatchRenewalJobs.Controls.Add(Me._SSBatchRenewalJobs_TabPage1)
        Me.SSBatchRenewalJobs.Controls.Add(Me._SSBatchRenewalJobs_TabPage2)
        Me.SSBatchRenewalJobs.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSBatchRenewalJobs.ItemSize = New System.Drawing.Size(220, 18)
        Me.SSBatchRenewalJobs.Location = New System.Drawing.Point(8, 8)
        Me.SSBatchRenewalJobs.Multiline = True
        Me.SSBatchRenewalJobs.Name = "SSBatchRenewalJobs"
        Me.SSBatchRenewalJobs.SelectedIndex = 0
        Me.SSBatchRenewalJobs.Size = New System.Drawing.Size(669, 453)
        Me.SSBatchRenewalJobs.TabIndex = 22
        '
        '_SSBatchRenewalJobs_TabPage0
        '
        Me._SSBatchRenewalJobs_TabPage0.Controls.Add(Me.chkRunExtendedRule)
        Me._SSBatchRenewalJobs_TabPage0.Controls.Add(Me.lblJobCode)
        Me._SSBatchRenewalJobs_TabPage0.Controls.Add(Me.lblDescription)
        Me._SSBatchRenewalJobs_TabPage0.Controls.Add(Me.lblJobType)
        Me._SSBatchRenewalJobs_TabPage0.Controls.Add(Me.lblSAMServer)
        Me._SSBatchRenewalJobs_TabPage0.Controls.Add(Me.lblDaysBeforeRenewalDate)
        Me._SSBatchRenewalJobs_TabPage0.Controls.Add(Me.cboJobType)
        Me._SSBatchRenewalJobs_TabPage0.Controls.Add(Me.txtJobCode)
        Me._SSBatchRenewalJobs_TabPage0.Controls.Add(Me.txtDescription)
        Me._SSBatchRenewalJobs_TabPage0.Controls.Add(Me.chkIsActive)
        Me._SSBatchRenewalJobs_TabPage0.Controls.Add(Me.txtSAMServer)
        Me._SSBatchRenewalJobs_TabPage0.Controls.Add(Me.txtDaysBeforeRenewalDate)
        Me._SSBatchRenewalJobs_TabPage0.Controls.Add(Me.fraCurrentConfigurationResults)
        Me._SSBatchRenewalJobs_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSBatchRenewalJobs_TabPage0.Name = "_SSBatchRenewalJobs_TabPage0"
        Me._SSBatchRenewalJobs_TabPage0.Size = New System.Drawing.Size(661, 427)
        Me._SSBatchRenewalJobs_TabPage0.TabIndex = 0
        Me._SSBatchRenewalJobs_TabPage0.Text = "1 - General"
        '
        'lblJobCode
        '
        Me.lblJobCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblJobCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblJobCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblJobCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblJobCode.Location = New System.Drawing.Point(16, 18)
        Me.lblJobCode.Name = "lblJobCode"
        Me.lblJobCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblJobCode.Size = New System.Drawing.Size(75, 17)
        Me.lblJobCode.TabIndex = 34
        Me.lblJobCode.Text = "Job Code:"
        '
        'lblDescription
        '
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(252, 18)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(85, 17)
        Me.lblDescription.TabIndex = 35
        Me.lblDescription.Text = "Description:"
        '
        'lblJobType
        '
        Me.lblJobType.BackColor = System.Drawing.SystemColors.Control
        Me.lblJobType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblJobType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblJobType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblJobType.Location = New System.Drawing.Point(16, 46)
        Me.lblJobType.Name = "lblJobType"
        Me.lblJobType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblJobType.Size = New System.Drawing.Size(75, 19)
        Me.lblJobType.TabIndex = 36
        Me.lblJobType.Text = "Job Type:"
        '
        'lblSAMServer
        '
        Me.lblSAMServer.BackColor = System.Drawing.SystemColors.Control
        Me.lblSAMServer.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSAMServer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSAMServer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSAMServer.Location = New System.Drawing.Point(16, 76)
        Me.lblSAMServer.Name = "lblSAMServer"
        Me.lblSAMServer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSAMServer.Size = New System.Drawing.Size(90, 19)
        Me.lblSAMServer.TabIndex = 37
        Me.lblSAMServer.Text = "SAM Server:"
        '
        'lblDaysBeforeRenewalDate
        '
        Me.lblDaysBeforeRenewalDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblDaysBeforeRenewalDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDaysBeforeRenewalDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDaysBeforeRenewalDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDaysBeforeRenewalDate.Location = New System.Drawing.Point(16, 100)
        Me.lblDaysBeforeRenewalDate.Name = "lblDaysBeforeRenewalDate"
        Me.lblDaysBeforeRenewalDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDaysBeforeRenewalDate.Size = New System.Drawing.Size(121, 35)
        Me.lblDaysBeforeRenewalDate.TabIndex = 38
        Me.lblDaysBeforeRenewalDate.Text = "Days before Renewal Date:"
        '
        'cboJobType
        '
        Me.cboJobType.DefaultItemId = 0
        Me.cboJobType.FirstItem = ""
        Me.cboJobType.ItemId = 0
        Me.cboJobType.ListIndex = -1
        Me.cboJobType.Location = New System.Drawing.Point(136, 44)
        Me.cboJobType.Name = "cboJobType"
        Me.cboJobType.PMLookupProductFamily = 1
        Me.cboJobType.SingleItemId = 0
        Me.cboJobType.Size = New System.Drawing.Size(211, 21)
        Me.cboJobType.Sorted = True
        Me.cboJobType.TabIndex = 2
        Me.cboJobType.TableName = "Batch_Renewal_Job_Type"
        Me.cboJobType.ToolTipText = ""
        Me.cboJobType.WhereClause = ""
        '
        'txtJobCode
        '
        Me.txtJobCode.AcceptsReturn = True
        Me.txtJobCode.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtJobCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtJobCode.Enabled = False
        Me.txtJobCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtJobCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtJobCode.Location = New System.Drawing.Point(136, 16)
        Me.txtJobCode.MaxLength = 20
        Me.txtJobCode.Name = "txtJobCode"
        Me.txtJobCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtJobCode.Size = New System.Drawing.Size(107, 20)
        Me.txtJobCode.TabIndex = 0
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(346, 16)
        Me.txtDescription.MaxLength = 255
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(295, 20)
        Me.txtDescription.TabIndex = 1
        '
        'chkIsActive
        '
        Me.chkIsActive.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsActive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsActive.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsActive.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsActive.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsActive.Location = New System.Drawing.Point(14, 132)
        Me.chkIsActive.Name = "chkIsActive"
        Me.chkIsActive.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsActive.Size = New System.Drawing.Size(136, 17)
        Me.chkIsActive.TabIndex = 5
        Me.chkIsActive.Text = "Is Active"
        Me.chkIsActive.UseVisualStyleBackColor = False
        '
        'txtSAMServer
        '
        Me.txtSAMServer.AcceptsReturn = True
        Me.txtSAMServer.BackColor = System.Drawing.SystemColors.Window
        Me.txtSAMServer.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSAMServer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSAMServer.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSAMServer.Location = New System.Drawing.Point(136, 74)
        Me.txtSAMServer.MaxLength = 500
        Me.txtSAMServer.Name = "txtSAMServer"
        Me.txtSAMServer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSAMServer.Size = New System.Drawing.Size(503, 20)
        Me.txtSAMServer.TabIndex = 3
        '
        'txtDaysBeforeRenewalDate
        '
        Me.txtDaysBeforeRenewalDate.AcceptsReturn = True
        Me.txtDaysBeforeRenewalDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtDaysBeforeRenewalDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDaysBeforeRenewalDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDaysBeforeRenewalDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDaysBeforeRenewalDate.Location = New System.Drawing.Point(136, 102)
        Me.txtDaysBeforeRenewalDate.MaxLength = 3
        Me.txtDaysBeforeRenewalDate.Name = "txtDaysBeforeRenewalDate"
        Me.txtDaysBeforeRenewalDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDaysBeforeRenewalDate.Size = New System.Drawing.Size(91, 20)
        Me.txtDaysBeforeRenewalDate.TabIndex = 4
        '
        'fraCurrentConfigurationResults
        '
        Me.fraCurrentConfigurationResults.BackColor = System.Drawing.SystemColors.Control
        Me.fraCurrentConfigurationResults.Controls.Add(Me.cmdCalculate)
        Me.fraCurrentConfigurationResults.Controls.Add(Me.lblCalculate)
        Me.fraCurrentConfigurationResults.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCurrentConfigurationResults.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraCurrentConfigurationResults.Location = New System.Drawing.Point(12, 162)
        Me.fraCurrentConfigurationResults.Name = "fraCurrentConfigurationResults"
        Me.fraCurrentConfigurationResults.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraCurrentConfigurationResults.Size = New System.Drawing.Size(643, 251)
        Me.fraCurrentConfigurationResults.TabIndex = 39
        Me.fraCurrentConfigurationResults.TabStop = False
        Me.fraCurrentConfigurationResults.Text = "Current Configuration Results"
        '
        'cmdCalculate
        '
        Me.cmdCalculate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCalculate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCalculate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCalculate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCalculate.Location = New System.Drawing.Point(542, 220)
        Me.cmdCalculate.Name = "cmdCalculate"
        Me.cmdCalculate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCalculate.Size = New System.Drawing.Size(91, 24)
        Me.cmdCalculate.TabIndex = 9
        Me.cmdCalculate.Text = "Ca&lculate"
        Me.cmdCalculate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCalculate.UseVisualStyleBackColor = False
        '
        'lblCalculate
        '
        Me.lblCalculate.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblCalculate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblCalculate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCalculate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCalculate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCalculate.Location = New System.Drawing.Point(10, 26)
        Me.lblCalculate.Name = "lblCalculate"
        Me.lblCalculate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCalculate.Size = New System.Drawing.Size(624, 187)
        Me.lblCalculate.TabIndex = 46
        '
        '_SSBatchRenewalJobs_TabPage1
        '
        Me._SSBatchRenewalJobs_TabPage1.Controls.Add(Me.fraAgents)
        Me._SSBatchRenewalJobs_TabPage1.Controls.Add(Me.fraProducts)
        Me._SSBatchRenewalJobs_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._SSBatchRenewalJobs_TabPage1.Name = "_SSBatchRenewalJobs_TabPage1"
        Me._SSBatchRenewalJobs_TabPage1.Size = New System.Drawing.Size(661, 427)
        Me._SSBatchRenewalJobs_TabPage1.TabIndex = 1
        Me._SSBatchRenewalJobs_TabPage1.Text = "2 - Product && Agent"
        '
        'fraAgents
        '
        Me.fraAgents.BackColor = System.Drawing.SystemColors.Control
        Me.fraAgents.Controls.Add(Me.chkIncludeDirectPolicies)
        Me.fraAgents.Controls.Add(Me.lvwAgents)
        Me.fraAgents.Controls.Add(Me.cmdRemoveProduct)
        Me.fraAgents.Controls.Add(Me.cmdAddProduct)
        Me.fraAgents.Controls.Add(Me.optAllAgents)
        Me.fraAgents.Controls.Add(Me.optSelectedAgents)
        Me.fraAgents.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAgents.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAgents.Location = New System.Drawing.Point(10, 226)
        Me.fraAgents.Name = "fraAgents"
        Me.fraAgents.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAgents.Size = New System.Drawing.Size(645, 197)
        Me.fraAgents.TabIndex = 41
        Me.fraAgents.TabStop = False
        Me.fraAgents.Text = "Agents"
        '
        'chkIncludeDirectPolicies
        '
        Me.chkIncludeDirectPolicies.BackColor = System.Drawing.SystemColors.Control
        Me.chkIncludeDirectPolicies.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIncludeDirectPolicies.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIncludeDirectPolicies.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIncludeDirectPolicies.Location = New System.Drawing.Point(364, 20)
        Me.chkIncludeDirectPolicies.Name = "chkIncludeDirectPolicies"
        Me.chkIncludeDirectPolicies.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIncludeDirectPolicies.Size = New System.Drawing.Size(174, 17)
        Me.chkIncludeDirectPolicies.TabIndex = 15
        Me.chkIncludeDirectPolicies.Text = "Include Direct Policies"
        Me.chkIncludeDirectPolicies.UseVisualStyleBackColor = False
        '
        'lvwAgents
        '
        Me.lvwAgents.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwAgents, "")
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwAgents, True)
        Me.lvwAgents.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwAgents.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me.lvwAgents, "")
        Me.lvwAgents.LabelEdit = True
        Me.listViewHelper1.SetLargeIcons(Me.lvwAgents, "")
        Me.lvwAgents.Location = New System.Drawing.Point(12, 40)
        Me.lvwAgents.Name = "lvwAgents"
        Me.lvwAgents.Size = New System.Drawing.Size(619, 125)
        Me.listViewHelper1.SetSmallIcons(Me.lvwAgents, "")
        Me.listViewHelper1.SetSorted(Me.lvwAgents, False)
        Me.listViewHelper1.SetSortKey(Me.lvwAgents, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwAgents, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwAgents.TabIndex = 47
        Me.lvwAgents.UseCompatibleStateImageBehavior = False
        '
        'cmdRemoveProduct
        '
        Me.cmdRemoveProduct.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRemoveProduct.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRemoveProduct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRemoveProduct.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRemoveProduct.Location = New System.Drawing.Point(564, 168)
        Me.cmdRemoveProduct.Name = "cmdRemoveProduct"
        Me.cmdRemoveProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRemoveProduct.Size = New System.Drawing.Size(73, 22)
        Me.cmdRemoveProduct.TabIndex = 14
        Me.cmdRemoveProduct.Text = "&Remove"
        Me.cmdRemoveProduct.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRemoveProduct.UseVisualStyleBackColor = False
        '
        'cmdAddProduct
        '
        Me.cmdAddProduct.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddProduct.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddProduct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddProduct.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddProduct.Location = New System.Drawing.Point(484, 168)
        Me.cmdAddProduct.Name = "cmdAddProduct"
        Me.cmdAddProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddProduct.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddProduct.TabIndex = 13
        Me.cmdAddProduct.Text = "&Add"
        Me.cmdAddProduct.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddProduct.UseVisualStyleBackColor = False
        '
        'optAllAgents
        '
        Me.optAllAgents.BackColor = System.Drawing.SystemColors.Control
        Me.optAllAgents.Checked = True
        Me.optAllAgents.Cursor = System.Windows.Forms.Cursors.Default
        Me.optAllAgents.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optAllAgents.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optAllAgents.Location = New System.Drawing.Point(10, 14)
        Me.optAllAgents.Name = "optAllAgents"
        Me.optAllAgents.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optAllAgents.Size = New System.Drawing.Size(145, 27)
        Me.optAllAgents.TabIndex = 11
        Me.optAllAgents.TabStop = True
        Me.optAllAgents.Text = "All Agents"
        Me.optAllAgents.UseVisualStyleBackColor = False
        '
        'optSelectedAgents
        '
        Me.optSelectedAgents.BackColor = System.Drawing.SystemColors.Control
        Me.optSelectedAgents.Cursor = System.Windows.Forms.Cursors.Default
        Me.optSelectedAgents.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optSelectedAgents.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelectedAgents.Location = New System.Drawing.Point(164, 14)
        Me.optSelectedAgents.Name = "optSelectedAgents"
        Me.optSelectedAgents.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optSelectedAgents.Size = New System.Drawing.Size(145, 27)
        Me.optSelectedAgents.TabIndex = 12
        Me.optSelectedAgents.TabStop = True
        Me.optSelectedAgents.Text = "Selected Agents"
        Me.optSelectedAgents.UseVisualStyleBackColor = False
        '
        'fraProducts
        '
        Me.fraProducts.BackColor = System.Drawing.SystemColors.Control
        Me.fraProducts.Controls.Add(Me.uctPickListProducts)
        Me.fraProducts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraProducts.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraProducts.Location = New System.Drawing.Point(8, 6)
        Me.fraProducts.Name = "fraProducts"
        Me.fraProducts.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraProducts.Size = New System.Drawing.Size(647, 219)
        Me.fraProducts.TabIndex = 40
        Me.fraProducts.TabStop = False
        Me.fraProducts.Text = "Products"
        '
        'uctPickListProducts
        '
        Me.uctPickListProducts.AvailableCaption = ""
        Me.uctPickListProducts.BusinessObject = "bSIRBatchRenewalJobs.Business"
        Me.uctPickListProducts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPickListProducts.ForeignKeys = CType(resources.GetObject("uctPickListProducts.ForeignKeys"), Microsoft.VisualBasic.Collection)
        Me.uctPickListProducts.IsSearchable = False
        Me.uctPickListProducts.Location = New System.Drawing.Point(6, 16)
        Me.uctPickListProducts.Name = "uctPickListProducts"
        Me.uctPickListProducts.PickListType = "Batch_Renewal_Job_Products"
        Me.uctPickListProducts.Size = New System.Drawing.Size(633, 195)
        Me.uctPickListProducts.TabIndex = 10
        '
        '_SSBatchRenewalJobs_TabPage2
        '
        Me._SSBatchRenewalJobs_TabPage2.Controls.Add(Me.fraBranches)
        Me._SSBatchRenewalJobs_TabPage2.Controls.Add(Me.fraRenewalReportDocuments)
        Me._SSBatchRenewalJobs_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._SSBatchRenewalJobs_TabPage2.Name = "_SSBatchRenewalJobs_TabPage2"
        Me._SSBatchRenewalJobs_TabPage2.Size = New System.Drawing.Size(661, 427)
        Me._SSBatchRenewalJobs_TabPage2.TabIndex = 2
        Me._SSBatchRenewalJobs_TabPage2.Text = "3 - Branch and Documents"
        '
        'fraBranches
        '
        Me.fraBranches.BackColor = System.Drawing.SystemColors.Control
        Me.fraBranches.Controls.Add(Me.uctPickListBranches)
        Me.fraBranches.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraBranches.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraBranches.Location = New System.Drawing.Point(8, 6)
        Me.fraBranches.Name = "fraBranches"
        Me.fraBranches.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraBranches.Size = New System.Drawing.Size(647, 219)
        Me.fraBranches.TabIndex = 42
        Me.fraBranches.TabStop = False
        Me.fraBranches.Text = "Branches"
        '
        'uctPickListBranches
        '
        Me.uctPickListBranches.AvailableCaption = ""
        Me.uctPickListBranches.BusinessObject = "bSIRBatchRenewalJobs.Business"
        Me.uctPickListBranches.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPickListBranches.ForeignKeys = CType(resources.GetObject("uctPickListBranches.ForeignKeys"), Microsoft.VisualBasic.Collection)
        Me.uctPickListBranches.IsSearchable = False
        Me.uctPickListBranches.Location = New System.Drawing.Point(6, 16)
        Me.uctPickListBranches.Name = "uctPickListBranches"
        Me.uctPickListBranches.PickListType = "Batch_Renewal_Job_Branches"
        Me.uctPickListBranches.Size = New System.Drawing.Size(633, 195)
        Me.uctPickListBranches.TabIndex = 16
        '
        'fraRenewalReportDocuments
        '
        Me.fraRenewalReportDocuments.BackColor = System.Drawing.SystemColors.Control
        Me.fraRenewalReportDocuments.Controls.Add(Me.fraPrinting)
        Me.fraRenewalReportDocuments.Controls.Add(Me.fraSortOrder)
        Me.fraRenewalReportDocuments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraRenewalReportDocuments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraRenewalReportDocuments.Location = New System.Drawing.Point(8, 228)
        Me.fraRenewalReportDocuments.Name = "fraRenewalReportDocuments"
        Me.fraRenewalReportDocuments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraRenewalReportDocuments.Size = New System.Drawing.Size(647, 193)
        Me.fraRenewalReportDocuments.TabIndex = 43
        Me.fraRenewalReportDocuments.TabStop = False
        Me.fraRenewalReportDocuments.Text = "Renewal Reports/Documents"
        '
        'fraPrinting
        '
        Me.fraPrinting.BackColor = System.Drawing.SystemColors.Control
        Me.fraPrinting.Controls.Add(Me.optNotRequired)
        Me.fraPrinting.Controls.Add(Me.optSpool)
        Me.fraPrinting.Controls.Add(Me.optPrint)
        Me.fraPrinting.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPrinting.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPrinting.Location = New System.Drawing.Point(18, 20)
        Me.fraPrinting.Name = "fraPrinting"
        Me.fraPrinting.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPrinting.Size = New System.Drawing.Size(613, 73)
        Me.fraPrinting.TabIndex = 45
        Me.fraPrinting.TabStop = False
        Me.fraPrinting.Text = "Printing"
        '
        'optNotRequired
        '
        Me.optNotRequired.BackColor = System.Drawing.SystemColors.Control
        Me.optNotRequired.Cursor = System.Windows.Forms.Cursors.Default
        Me.optNotRequired.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optNotRequired.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optNotRequired.Location = New System.Drawing.Point(440, 26)
        Me.optNotRequired.Name = "optNotRequired"
        Me.optNotRequired.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optNotRequired.Size = New System.Drawing.Size(129, 27)
        Me.optNotRequired.TabIndex = 19
        Me.optNotRequired.TabStop = True
        Me.optNotRequired.Text = "Not Required"
        Me.optNotRequired.UseVisualStyleBackColor = False
        '
        'optSpool
        '
        Me.optSpool.BackColor = System.Drawing.SystemColors.Control
        Me.optSpool.Cursor = System.Windows.Forms.Cursors.Default
        Me.optSpool.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optSpool.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSpool.Location = New System.Drawing.Point(229, 26)
        Me.optSpool.Name = "optSpool"
        Me.optSpool.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optSpool.Size = New System.Drawing.Size(129, 27)
        Me.optSpool.TabIndex = 18
        Me.optSpool.TabStop = True
        Me.optSpool.Text = "Spool"
        Me.optSpool.UseVisualStyleBackColor = False
        '
        'optPrint
        '
        Me.optPrint.BackColor = System.Drawing.SystemColors.Control
        Me.optPrint.Checked = True
        Me.optPrint.Cursor = System.Windows.Forms.Cursors.Default
        Me.optPrint.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optPrint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optPrint.Location = New System.Drawing.Point(18, 26)
        Me.optPrint.Name = "optPrint"
        Me.optPrint.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optPrint.Size = New System.Drawing.Size(129, 27)
        Me.optPrint.TabIndex = 17
        Me.optPrint.TabStop = True
        Me.optPrint.Text = "Print"
        Me.optPrint.UseVisualStyleBackColor = False
        '
        'fraSortOrder
        '
        Me.fraSortOrder.BackColor = System.Drawing.SystemColors.Control
        Me.fraSortOrder.Controls.Add(Me.optPolicyNumber)
        Me.fraSortOrder.Controls.Add(Me.optClient)
        Me.fraSortOrder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraSortOrder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraSortOrder.Location = New System.Drawing.Point(20, 104)
        Me.fraSortOrder.Name = "fraSortOrder"
        Me.fraSortOrder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraSortOrder.Size = New System.Drawing.Size(613, 73)
        Me.fraSortOrder.TabIndex = 44
        Me.fraSortOrder.TabStop = False
        Me.fraSortOrder.Text = "Sort Order"
        '
        'optPolicyNumber
        '
        Me.optPolicyNumber.BackColor = System.Drawing.SystemColors.Control
        Me.optPolicyNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.optPolicyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optPolicyNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optPolicyNumber.Location = New System.Drawing.Point(226, 24)
        Me.optPolicyNumber.Name = "optPolicyNumber"
        Me.optPolicyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optPolicyNumber.Size = New System.Drawing.Size(147, 27)
        Me.optPolicyNumber.TabIndex = 21
        Me.optPolicyNumber.TabStop = True
        Me.optPolicyNumber.Text = "Policy Number"
        Me.optPolicyNumber.UseVisualStyleBackColor = False
        '
        'optClient
        '
        Me.optClient.BackColor = System.Drawing.SystemColors.Control
        Me.optClient.Checked = True
        Me.optClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.optClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optClient.Location = New System.Drawing.Point(18, 24)
        Me.optClient.Name = "optClient"
        Me.optClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optClient.Size = New System.Drawing.Size(147, 27)
        Me.optClient.TabIndex = 20
        Me.optClient.TabStop = True
        Me.optClient.Text = "Client"
        Me.optClient.UseVisualStyleBackColor = False
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.uctCreditCard2)
        Me.Frame2.Controls.Add(Me.uctPMAddressControl4)
        Me.Frame2.Controls.Add(Me.Check2)
        Me.Frame2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(-4986, 30)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(937, 163)
        Me.Frame2.TabIndex = 30
        Me.Frame2.TabStop = False
        Me.Frame2.Text = "Credit/Debit Card Details"
        '
        'uctCreditCard2
        '
        Me.uctCreditCard2.BackColor = System.Drawing.SystemColors.Control
        Me.uctCreditCard2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.uctCreditCard2.Cursor = System.Windows.Forms.Cursors.Default
        Me.uctCreditCard2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctCreditCard2.Location = New System.Drawing.Point(8, 20)
        Me.uctCreditCard2.Name = "uctCreditCard2"
        Me.uctCreditCard2.Size = New System.Drawing.Size(555, 135)
        Me.uctCreditCard2.TabIndex = 33
        Me.uctCreditCard2.TabStop = False
        '
        'uctPMAddressControl4
        '
        Me.uctPMAddressControl4.BackColor = System.Drawing.Color.Red
        Me.uctPMAddressControl4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.uctPMAddressControl4.Cursor = System.Windows.Forms.Cursors.Default
        Me.uctPMAddressControl4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMAddressControl4.Location = New System.Drawing.Point(0, 0)
        Me.uctPMAddressControl4.Name = "uctPMAddressControl4"
        Me.uctPMAddressControl4.Size = New System.Drawing.Size(67, 67)
        Me.uctPMAddressControl4.TabIndex = 32
        Me.uctPMAddressControl4.TabStop = False
        '
        'Check2
        '
        Me.Check2.BackColor = System.Drawing.SystemColors.Control
        Me.Check2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Check2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Check2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Check2.Location = New System.Drawing.Point(570, 18)
        Me.Check2.Name = "Check2"
        Me.Check2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Check2.Size = New System.Drawing.Size(127, 17)
        Me.Check2.TabIndex = 31
        Me.Check2.Text = "Registered CardHolder"
        Me.Check2.UseVisualStyleBackColor = False
        '
        'Command3
        '
        Me.Command3.BackColor = System.Drawing.SystemColors.Control
        Me.Command3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Command3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Command3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Command3.Location = New System.Drawing.Point(-4318, 202)
        Me.Command3.Name = "Command3"
        Me.Command3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Command3.Size = New System.Drawing.Size(85, 25)
        Me.Command3.TabIndex = 29
        Me.Command3.Text = "Apply"
        Me.Command3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Command3.UseVisualStyleBackColor = False
        '
        'Command2
        '
        Me.Command2.BackColor = System.Drawing.SystemColors.Control
        Me.Command2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Command2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Command2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Command2.Location = New System.Drawing.Point(-4226, 202)
        Me.Command2.Name = "Command2"
        Me.Command2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Command2.Size = New System.Drawing.Size(85, 25)
        Me.Command2.TabIndex = 28
        Me.Command2.Text = "Ok"
        Me.Command2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Command2.UseVisualStyleBackColor = False
        '
        'Command1
        '
        Me.Command1.BackColor = System.Drawing.SystemColors.Control
        Me.Command1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Command1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Command1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Command1.Location = New System.Drawing.Point(-4134, 202)
        Me.Command1.Name = "Command1"
        Me.Command1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Command1.Size = New System.Drawing.Size(85, 25)
        Me.Command1.TabIndex = 27
        Me.Command1.Text = "Cancel"
        Me.Command1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Command1.UseVisualStyleBackColor = False
        '
        'Frame5
        '
        Me.Frame5.BackColor = System.Drawing.SystemColors.Control
        Me.Frame5.Controls.Add(Me.uctPMAddressCC)
        Me.Frame5.Controls.Add(Me.chkIsRegistered)
        Me.Frame5.Controls.Add(Me.uctCreditCardDetails)
        Me.Frame5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame5.Location = New System.Drawing.Point(-4994, 34)
        Me.Frame5.Name = "Frame5"
        Me.Frame5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame5.Size = New System.Drawing.Size(937, 165)
        Me.Frame5.TabIndex = 23
        Me.Frame5.TabStop = False
        Me.Frame5.Text = "Credit/Debit Card Details"
        '
        'uctPMAddressCC
        '
        Me.uctPMAddressCC.BackColor = System.Drawing.Color.Red
        Me.uctPMAddressCC.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.uctPMAddressCC.Cursor = System.Windows.Forms.Cursors.Default
        Me.uctPMAddressCC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMAddressCC.Location = New System.Drawing.Point(0, 0)
        Me.uctPMAddressCC.Name = "uctPMAddressCC"
        Me.uctPMAddressCC.Size = New System.Drawing.Size(67, 67)
        Me.uctPMAddressCC.TabIndex = 26
        Me.uctPMAddressCC.TabStop = False
        '
        'chkIsRegistered
        '
        Me.chkIsRegistered.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsRegistered.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsRegistered.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsRegistered.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsRegistered.Location = New System.Drawing.Point(570, 8)
        Me.chkIsRegistered.Name = "chkIsRegistered"
        Me.chkIsRegistered.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsRegistered.Size = New System.Drawing.Size(151, 17)
        Me.chkIsRegistered.TabIndex = 25
        Me.chkIsRegistered.Text = "Registered CardHolder"
        Me.chkIsRegistered.UseVisualStyleBackColor = False
        '
        'uctCreditCardDetails
        '
        Me.uctCreditCardDetails.BackColor = System.Drawing.Color.Red
        Me.uctCreditCardDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.uctCreditCardDetails.Cursor = System.Windows.Forms.Cursors.Default
        Me.uctCreditCardDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctCreditCardDetails.Location = New System.Drawing.Point(0, 0)
        Me.uctCreditCardDetails.Name = "uctCreditCardDetails"
        Me.uctCreditCardDetails.Size = New System.Drawing.Size(67, 67)
        Me.uctCreditCardDetails.TabIndex = 24
        Me.uctCreditCardDetails.TabStop = False
        '
        'chkRunExtendedRule
        '
        Me.chkRunExtendedRule.BackColor = System.Drawing.SystemColors.Control
        Me.chkRunExtendedRule.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkRunExtendedRule.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRunExtendedRule.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRunExtendedRule.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRunExtendedRule.Location = New System.Drawing.Point(395, 132)
        Me.chkRunExtendedRule.Name = "chkRunExtendedRule"
        Me.chkRunExtendedRule.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRunExtendedRule.Size = New System.Drawing.Size(132, 17)
        Me.chkRunExtendedRule.TabIndex = 40
        Me.chkRunExtendedRule.Text = "Extended Rule"
        Me.chkRunExtendedRule.UseVisualStyleBackColor = False
        '
        'frmBatchRenewalJob
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(680, 492)
        Me.Controls.Add(Me.Frame2)
        Me.Controls.Add(Me.Command3)
        Me.Controls.Add(Me.Command2)
        Me.Controls.Add(Me.Command1)
        Me.Controls.Add(Me.Frame5)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.SSBatchRenewalJobs)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 29)
        Me.MaximizeBox = False
        Me.Name = "frmBatchRenewalJob"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.Text = "Batch Renewal Job Configuration"
        Me.SSBatchRenewalJobs.ResumeLayout(False)
        Me._SSBatchRenewalJobs_TabPage0.ResumeLayout(False)
        Me._SSBatchRenewalJobs_TabPage0.PerformLayout()
        Me.fraCurrentConfigurationResults.ResumeLayout(False)
        Me._SSBatchRenewalJobs_TabPage1.ResumeLayout(False)
        Me.fraAgents.ResumeLayout(False)
        Me.fraProducts.ResumeLayout(False)
        Me._SSBatchRenewalJobs_TabPage2.ResumeLayout(False)
        Me.fraBranches.ResumeLayout(False)
        Me.fraRenewalReportDocuments.ResumeLayout(False)
        Me.fraPrinting.ResumeLayout(False)
        Me.fraSortOrder.ResumeLayout(False)
        Me.Frame2.ResumeLayout(False)
        CType(Me.uctCreditCard2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uctPMAddressControl4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Frame5.ResumeLayout(False)
        CType(Me.uctPMAddressCC, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uctCreditCardDetails, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents chkRunExtendedRule As System.Windows.Forms.CheckBox
#End Region
End Class