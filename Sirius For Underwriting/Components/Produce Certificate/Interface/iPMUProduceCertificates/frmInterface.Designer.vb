<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializemnuRestart()
		InitializemnuProcess()
		InitializelblTickParent()
		InitializelblTickChild()
		InitializelblStepParent()
		InitializelblStepChild()
		InitializeimgTickParent()
		InitializeimgTickChild()
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
	Private WithEvents _mnuRestart_1 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuSep1 As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuExit As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuProcess_1 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	Public WithEvents lblStepLabel As System.Windows.Forms.Label
	Public WithEvents frameStepLabel As System.Windows.Forms.Panel
	Public WithEvents optBulkCertificate As System.Windows.Forms.RadioButton
	Public WithEvents optCredentialCertificate As System.Windows.Forms.RadioButton
	Public WithEvents optNonCredentialCertificate As System.Windows.Forms.RadioButton
	Public WithEvents FrameCertificates As System.Windows.Forms.Panel
	Public WithEvents optPolicy As System.Windows.Forms.RadioButton
	Public WithEvents cmdBrowsePolicy As System.Windows.Forms.Button
	Public WithEvents txtPolicyRef As System.Windows.Forms.TextBox
	Public WithEvents optClient As System.Windows.Forms.RadioButton
	Public WithEvents cmdBrowseClient As System.Windows.Forms.Button
	Public WithEvents txtClientName As System.Windows.Forms.TextBox
	Public WithEvents FrameClient As System.Windows.Forms.Panel
	Public WithEvents txtCertHolderName As System.Windows.Forms.TextBox
	Public WithEvents cmdBrowseCertHolder As System.Windows.Forms.Button
	Public WithEvents lblCertHolder As System.Windows.Forms.Label
	Public WithEvents FrameCertHolder As System.Windows.Forms.Panel
	Public WithEvents txtAncillaryName As System.Windows.Forms.TextBox
	Public WithEvents chkNonScheduledAncillary As System.Windows.Forms.CheckBox
	Public WithEvents lblAnicillaryName As System.Windows.Forms.Label
	Public WithEvents FrameAncillary As System.Windows.Forms.GroupBox
	Public WithEvents lvwPolicies As System.Windows.Forms.ListView
	Public WithEvents txtComment As System.Windows.Forms.TextBox
	Public WithEvents lblComment As System.Windows.Forms.Label
	Public WithEvents frameComment As System.Windows.Forms.Panel
	Public WithEvents FrameStepOptions As System.Windows.Forms.GroupBox
	Public WithEvents lblStepHeading As System.Windows.Forms.Label
	Public WithEvents FrameDetails As System.Windows.Forms.GroupBox
	Public WithEvents ImageListTick As System.Windows.Forms.ImageList
	Public WithEvents Image1 As System.Windows.Forms.PictureBox
	Private WithEvents _imgTickChild_0 As System.Windows.Forms.PictureBox
	Private WithEvents _imgTickParent_0 As System.Windows.Forms.PictureBox
	Private WithEvents _lblTickChild_0 As System.Windows.Forms.Label
	Private WithEvents _lblStepChild_0 As System.Windows.Forms.Label
	Private WithEvents _lblStepParent_0 As System.Windows.Forms.Label
	Private WithEvents _lblTickParent_0 As System.Windows.Forms.Label
	Public WithEvents lbl_StepsCovered As System.Windows.Forms.Label
	Public WithEvents FrameStepsCovered As System.Windows.Forms.GroupBox
	Public WithEvents cmdFinish As System.Windows.Forms.Button
	Public WithEvents cmdLossRunPrint As System.Windows.Forms.Button
	Public WithEvents cmdBack As System.Windows.Forms.Button
	Public WithEvents cmdNext As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents FrameNavigation As System.Windows.Forms.GroupBox
	Public imgTickChild(0) As System.Windows.Forms.PictureBox
	Public imgTickParent(0) As System.Windows.Forms.PictureBox
	Public lblStepChild(0) As System.Windows.Forms.Label
	Public lblStepParent(0) As System.Windows.Forms.Label
	Public lblTickChild(0) As System.Windows.Forms.Label
	Public lblTickParent(0) As System.Windows.Forms.Label
	Public mnuProcess(1) As System.Windows.Forms.ToolStripMenuItem
	Public mnuRestart(1) As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip()
        Me._mnuProcess_1 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuRestart_1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSep1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.FrameDetails = New System.Windows.Forms.GroupBox()
        Me.frameStepLabel = New System.Windows.Forms.Panel()
        Me.lblStepLabel = New System.Windows.Forms.Label()
        Me.FrameStepOptions = New System.Windows.Forms.GroupBox()
        Me.FrameCertificates = New System.Windows.Forms.Panel()
        Me.optBulkCertificate = New System.Windows.Forms.RadioButton()
        Me.optCredentialCertificate = New System.Windows.Forms.RadioButton()
        Me.optNonCredentialCertificate = New System.Windows.Forms.RadioButton()
        Me.FrameClient = New System.Windows.Forms.Panel()
        Me.optPolicy = New System.Windows.Forms.RadioButton()
        Me.cmdBrowsePolicy = New System.Windows.Forms.Button()
        Me.txtPolicyRef = New System.Windows.Forms.TextBox()
        Me.optClient = New System.Windows.Forms.RadioButton()
        Me.cmdBrowseClient = New System.Windows.Forms.Button()
        Me.txtClientName = New System.Windows.Forms.TextBox()
        Me.FrameCertHolder = New System.Windows.Forms.Panel()
        Me.txtCertHolderName = New System.Windows.Forms.TextBox()
        Me.cmdBrowseCertHolder = New System.Windows.Forms.Button()
        Me.lblCertHolder = New System.Windows.Forms.Label()
        Me.FrameAncillary = New System.Windows.Forms.GroupBox()
        Me.txtAncillaryName = New System.Windows.Forms.TextBox()
        Me.chkNonScheduledAncillary = New System.Windows.Forms.CheckBox()
        Me.lblAnicillaryName = New System.Windows.Forms.Label()
        Me.lvwPolicies = New System.Windows.Forms.ListView()
        Me.frameComment = New System.Windows.Forms.Panel()
        Me.txtComment = New System.Windows.Forms.TextBox()
        Me.lblComment = New System.Windows.Forms.Label()
        Me.lblStepHeading = New System.Windows.Forms.Label()
        Me.FrameStepsCovered = New System.Windows.Forms.GroupBox()
        Me.Image1 = New System.Windows.Forms.PictureBox()
        Me._imgTickChild_0 = New System.Windows.Forms.PictureBox()
        Me._imgTickParent_0 = New System.Windows.Forms.PictureBox()
        Me._lblTickChild_0 = New System.Windows.Forms.Label()
        Me._lblStepChild_0 = New System.Windows.Forms.Label()
        Me._lblStepParent_0 = New System.Windows.Forms.Label()
        Me._lblTickParent_0 = New System.Windows.Forms.Label()
        Me.lbl_StepsCovered = New System.Windows.Forms.Label()
        Me.ImageListTick = New System.Windows.Forms.ImageList(Me.components)
        Me.FrameNavigation = New System.Windows.Forms.GroupBox()
        Me.cmdFinish = New System.Windows.Forms.Button()
        Me.cmdLossRunPrint = New System.Windows.Forms.Button()
        Me.cmdBack = New System.Windows.Forms.Button()
        Me.cmdNext = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.LblLoss = New System.Windows.Forms.Label()
        Me.CboNumberOfYear = New System.Windows.Forms.ComboBox()
        Me.MainMenu1.SuspendLayout
        Me.FrameDetails.SuspendLayout
        Me.frameStepLabel.SuspendLayout
        Me.FrameStepOptions.SuspendLayout
        Me.FrameCertificates.SuspendLayout
        Me.FrameClient.SuspendLayout
        Me.FrameCertHolder.SuspendLayout
        Me.FrameAncillary.SuspendLayout
        Me.frameComment.SuspendLayout
        Me.FrameStepsCovered.SuspendLayout
        CType(Me.Image1,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me._imgTickChild_0,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me._imgTickParent_0,System.ComponentModel.ISupportInitialize).BeginInit
        Me.FrameNavigation.SuspendLayout
        CType(Me.listViewHelper1,System.ComponentModel.ISupportInitialize).BeginInit
        Me.SuspendLayout
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._mnuProcess_1})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(736, 24)
        Me.MainMenu1.TabIndex = 10
        '
        '_mnuProcess_1
        '
        Me._mnuProcess_1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._mnuRestart_1, Me.mnuSep1, Me.mnuExit})
        Me._mnuProcess_1.Name = "_mnuProcess_1"
        Me._mnuProcess_1.Size = New System.Drawing.Size(59, 20)
        Me._mnuProcess_1.Text = "&Process"
        '
        '_mnuRestart_1
        '
        Me._mnuRestart_1.Name = "_mnuRestart_1"
        Me._mnuRestart_1.Size = New System.Drawing.Size(110, 22)
        Me._mnuRestart_1.Text = "&Restart"
        '
        'mnuSep1
        '
        Me.mnuSep1.Name = "mnuSep1"
        Me.mnuSep1.Size = New System.Drawing.Size(107, 6)
        '
        'mnuExit
        '
        Me.mnuExit.Name = "mnuExit"
        Me.mnuExit.Size = New System.Drawing.Size(110, 22)
        Me.mnuExit.Text = "E&xit"
        '
        'FrameDetails
        '
        Me.FrameDetails.BackColor = System.Drawing.SystemColors.Control
        Me.FrameDetails.Controls.Add(Me.frameStepLabel)
        Me.FrameDetails.Controls.Add(Me.FrameStepOptions)
        Me.FrameDetails.Controls.Add(Me.lblStepHeading)
        Me.FrameDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.FrameDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FrameDetails.Location = New System.Drawing.Point(232, 20)
        Me.FrameDetails.Name = "FrameDetails"
        Me.FrameDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FrameDetails.Size = New System.Drawing.Size(501, 259)
        Me.FrameDetails.TabIndex = 0
        Me.FrameDetails.TabStop = false
        '
        'frameStepLabel
        '
        Me.frameStepLabel.BackColor = System.Drawing.SystemColors.Control
        Me.frameStepLabel.Controls.Add(Me.lblStepLabel)
        Me.frameStepLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.frameStepLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.frameStepLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frameStepLabel.Location = New System.Drawing.Point(2, 30)
        Me.frameStepLabel.Name = "frameStepLabel"
        Me.frameStepLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frameStepLabel.Size = New System.Drawing.Size(489, 47)
        Me.frameStepLabel.TabIndex = 7
        '
        'lblStepLabel
        '
        Me.lblStepLabel.BackColor = System.Drawing.SystemColors.Control
        Me.lblStepLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStepLabel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblStepLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStepLabel.Location = New System.Drawing.Point(10, 8)
        Me.lblStepLabel.Name = "lblStepLabel"
        Me.lblStepLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStepLabel.Size = New System.Drawing.Size(475, 39)
        Me.lblStepLabel.TabIndex = 8
        Me.lblStepLabel.Text = "This step will allow you to configure the wizard to produce Loss History Letter a"& _ 
    "nd Certificate, Bulk Certificate or Certificate only. Just select from the optio"& _ 
    "ns specified below."
        '
        'FrameStepOptions
        '
        Me.FrameStepOptions.BackColor = System.Drawing.SystemColors.Control
        Me.FrameStepOptions.Controls.Add(Me.FrameClient)
        Me.FrameStepOptions.Controls.Add(Me.FrameCertificates)
        Me.FrameStepOptions.Controls.Add(Me.FrameCertHolder)
        Me.FrameStepOptions.Controls.Add(Me.FrameAncillary)
        Me.FrameStepOptions.Controls.Add(Me.lvwPolicies)
        Me.FrameStepOptions.Controls.Add(Me.frameComment)
        Me.FrameStepOptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.FrameStepOptions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FrameStepOptions.Location = New System.Drawing.Point(5, 83)
        Me.FrameStepOptions.Name = "FrameStepOptions"
        Me.FrameStepOptions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FrameStepOptions.Size = New System.Drawing.Size(493, 169)
        Me.FrameStepOptions.TabIndex = 16
        Me.FrameStepOptions.TabStop = false
        '
        'FrameCertificates
        '
        Me.FrameCertificates.BackColor = System.Drawing.SystemColors.Control
        Me.FrameCertificates.Controls.Add(Me.optBulkCertificate)
        Me.FrameCertificates.Controls.Add(Me.optCredentialCertificate)
        Me.FrameCertificates.Controls.Add(Me.optNonCredentialCertificate)
        Me.FrameCertificates.Cursor = System.Windows.Forms.Cursors.Default
        Me.FrameCertificates.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.FrameCertificates.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FrameCertificates.Location = New System.Drawing.Point(4, 10)
        Me.FrameCertificates.Name = "FrameCertificates"
        Me.FrameCertificates.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FrameCertificates.Size = New System.Drawing.Size(485, 93)
        Me.FrameCertificates.TabIndex = 36
        '
        'optBulkCertificate
        '
        Me.optBulkCertificate.BackColor = System.Drawing.SystemColors.Control
        Me.optBulkCertificate.Cursor = System.Windows.Forms.Cursors.Default
        Me.optBulkCertificate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.optBulkCertificate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optBulkCertificate.Location = New System.Drawing.Point(28, 62)
        Me.optBulkCertificate.Name = "optBulkCertificate"
        Me.optBulkCertificate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optBulkCertificate.Size = New System.Drawing.Size(365, 29)
        Me.optBulkCertificate.TabIndex = 39
        Me.optBulkCertificate.TabStop = true
        Me.optBulkCertificate.Text = "Do you want to produce Bulk Certificates?"
        Me.optBulkCertificate.UseVisualStyleBackColor = false
        '
        'optCredentialCertificate
        '
        Me.optCredentialCertificate.BackColor = System.Drawing.SystemColors.Control
        Me.optCredentialCertificate.Cursor = System.Windows.Forms.Cursors.Default
        Me.optCredentialCertificate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.optCredentialCertificate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optCredentialCertificate.Location = New System.Drawing.Point(28, 8)
        Me.optCredentialCertificate.Name = "optCredentialCertificate"
        Me.optCredentialCertificate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optCredentialCertificate.Size = New System.Drawing.Size(365, 27)
        Me.optCredentialCertificate.TabIndex = 38
        Me.optCredentialCertificate.TabStop = true
        Me.optCredentialCertificate.Tag = "Produce Loss History Letter"
        Me.optCredentialCertificate.Text = "Do you want to produce Certificate and Loss History Letter?"
        Me.optCredentialCertificate.UseVisualStyleBackColor = false
        '
        'optNonCredentialCertificate
        '
        Me.optNonCredentialCertificate.BackColor = System.Drawing.SystemColors.Control
        Me.optNonCredentialCertificate.Cursor = System.Windows.Forms.Cursors.Default
        Me.optNonCredentialCertificate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.optNonCredentialCertificate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optNonCredentialCertificate.Location = New System.Drawing.Point(28, 35)
        Me.optNonCredentialCertificate.Name = "optNonCredentialCertificate"
        Me.optNonCredentialCertificate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optNonCredentialCertificate.Size = New System.Drawing.Size(365, 27)
        Me.optNonCredentialCertificate.TabIndex = 37
        Me.optNonCredentialCertificate.TabStop = true
        Me.optNonCredentialCertificate.Text = "Do you want to produce Certificate Only?"
        Me.optNonCredentialCertificate.UseVisualStyleBackColor = false
        '
        'FrameClient
        '
        Me.FrameClient.BackColor = System.Drawing.SystemColors.Control
        Me.FrameClient.Controls.Add(Me.LblLoss)
        Me.FrameClient.Controls.Add(Me.CboNumberOfYear)
        Me.FrameClient.Controls.Add(Me.optPolicy)
        Me.FrameClient.Controls.Add(Me.cmdBrowsePolicy)
        Me.FrameClient.Controls.Add(Me.txtPolicyRef)
        Me.FrameClient.Controls.Add(Me.optClient)
        Me.FrameClient.Controls.Add(Me.cmdBrowseClient)
        Me.FrameClient.Controls.Add(Me.txtClientName)
        Me.FrameClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.FrameClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.FrameClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FrameClient.Location = New System.Drawing.Point(4, 15)
        Me.FrameClient.Name = "FrameClient"
        Me.FrameClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FrameClient.Size = New System.Drawing.Size(484, 82)
        Me.FrameClient.TabIndex = 29
        Me.FrameClient.Visible = false
        '
        'optPolicy
        '
        Me.optPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.optPolicy.Cursor = System.Windows.Forms.Cursors.Default
        Me.optPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.optPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optPolicy.Location = New System.Drawing.Point(28, 31)
        Me.optPolicy.Name = "optPolicy"
        Me.optPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optPolicy.Size = New System.Drawing.Size(60, 21)
        Me.optPolicy.TabIndex = 35
        Me.optPolicy.TabStop = true
        Me.optPolicy.Text = "Policy"
        Me.optPolicy.UseVisualStyleBackColor = false
        '
        'cmdBrowsePolicy
        '
        Me.cmdBrowsePolicy.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBrowsePolicy.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBrowsePolicy.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdBrowsePolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBrowsePolicy.Location = New System.Drawing.Point(366, 31)
        Me.cmdBrowsePolicy.Name = "cmdBrowsePolicy"
        Me.cmdBrowsePolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBrowsePolicy.Size = New System.Drawing.Size(33, 21)
        Me.cmdBrowsePolicy.TabIndex = 34
        Me.cmdBrowsePolicy.Text = "..."
        Me.cmdBrowsePolicy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdBrowsePolicy.UseVisualStyleBackColor = false
        '
        'txtPolicyRef
        '
        Me.txtPolicyRef.AcceptsReturn = true
        Me.txtPolicyRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolicyRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicyRef.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtPolicyRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicyRef.Location = New System.Drawing.Point(122, 31)
        Me.txtPolicyRef.MaxLength = 0
        Me.txtPolicyRef.Name = "txtPolicyRef"
        Me.txtPolicyRef.ReadOnly = true
        Me.txtPolicyRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicyRef.Size = New System.Drawing.Size(241, 21)
        Me.txtPolicyRef.TabIndex = 33
        '
        'optClient
        '
        Me.optClient.BackColor = System.Drawing.SystemColors.Control
        Me.optClient.Checked = true
        Me.optClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.optClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.optClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optClient.Location = New System.Drawing.Point(28, 8)
        Me.optClient.Name = "optClient"
        Me.optClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optClient.Size = New System.Drawing.Size(60, 21)
        Me.optClient.TabIndex = 32
        Me.optClient.TabStop = true
        Me.optClient.Text = "Client"
        Me.optClient.UseVisualStyleBackColor = false
        '
        'cmdBrowseClient
        '
        Me.cmdBrowseClient.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBrowseClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBrowseClient.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdBrowseClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBrowseClient.Location = New System.Drawing.Point(366, 8)
        Me.cmdBrowseClient.Name = "cmdBrowseClient"
        Me.cmdBrowseClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBrowseClient.Size = New System.Drawing.Size(33, 21)
        Me.cmdBrowseClient.TabIndex = 31
        Me.cmdBrowseClient.Text = "..."
        Me.cmdBrowseClient.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdBrowseClient.UseVisualStyleBackColor = false
        '
        'txtClientName
        '
        Me.txtClientName.AcceptsReturn = true
        Me.txtClientName.BackColor = System.Drawing.SystemColors.Window
        Me.txtClientName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClientName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtClientName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClientName.Location = New System.Drawing.Point(122, 8)
        Me.txtClientName.MaxLength = 0
        Me.txtClientName.Name = "txtClientName"
        Me.txtClientName.ReadOnly = true
        Me.txtClientName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClientName.Size = New System.Drawing.Size(241, 21)
        Me.txtClientName.TabIndex = 30
        '
        'FrameCertHolder
        '
        Me.FrameCertHolder.BackColor = System.Drawing.SystemColors.Control
        Me.FrameCertHolder.Controls.Add(Me.txtCertHolderName)
        Me.FrameCertHolder.Controls.Add(Me.cmdBrowseCertHolder)
        Me.FrameCertHolder.Controls.Add(Me.lblCertHolder)
        Me.FrameCertHolder.Cursor = System.Windows.Forms.Cursors.Default
        Me.FrameCertHolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.FrameCertHolder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FrameCertHolder.Location = New System.Drawing.Point(4, 14)
        Me.FrameCertHolder.Name = "FrameCertHolder"
        Me.FrameCertHolder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FrameCertHolder.Size = New System.Drawing.Size(479, 35)
        Me.FrameCertHolder.TabIndex = 25
        Me.FrameCertHolder.Visible = false
        '
        'txtCertHolderName
        '
        Me.txtCertHolderName.AcceptsReturn = true
        Me.txtCertHolderName.BackColor = System.Drawing.SystemColors.Window
        Me.txtCertHolderName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCertHolderName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtCertHolderName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCertHolderName.Location = New System.Drawing.Point(190, 5)
        Me.txtCertHolderName.MaxLength = 0
        Me.txtCertHolderName.Name = "txtCertHolderName"
        Me.txtCertHolderName.ReadOnly = true
        Me.txtCertHolderName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCertHolderName.Size = New System.Drawing.Size(172, 21)
        Me.txtCertHolderName.TabIndex = 27
        '
        'cmdBrowseCertHolder
        '
        Me.cmdBrowseCertHolder.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBrowseCertHolder.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBrowseCertHolder.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdBrowseCertHolder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBrowseCertHolder.Location = New System.Drawing.Point(366, 6)
        Me.cmdBrowseCertHolder.Name = "cmdBrowseCertHolder"
        Me.cmdBrowseCertHolder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBrowseCertHolder.Size = New System.Drawing.Size(33, 21)
        Me.cmdBrowseCertHolder.TabIndex = 26
        Me.cmdBrowseCertHolder.Text = "..."
        Me.cmdBrowseCertHolder.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdBrowseCertHolder.UseVisualStyleBackColor = false
        '
        'lblCertHolder
        '
        Me.lblCertHolder.BackColor = System.Drawing.SystemColors.Control
        Me.lblCertHolder.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCertHolder.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblCertHolder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCertHolder.Location = New System.Drawing.Point(8, 8)
        Me.lblCertHolder.Name = "lblCertHolder"
        Me.lblCertHolder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCertHolder.Size = New System.Drawing.Size(176, 17)
        Me.lblCertHolder.TabIndex = 28
        Me.lblCertHolder.Text = "Select a Certificate Holder :"
        '
        'FrameAncillary
        '
        Me.FrameAncillary.BackColor = System.Drawing.SystemColors.Control
        Me.FrameAncillary.Controls.Add(Me.txtAncillaryName)
        Me.FrameAncillary.Controls.Add(Me.chkNonScheduledAncillary)
        Me.FrameAncillary.Controls.Add(Me.lblAnicillaryName)
        Me.FrameAncillary.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.FrameAncillary.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FrameAncillary.Location = New System.Drawing.Point(4, 9)
        Me.FrameAncillary.Name = "FrameAncillary"
        Me.FrameAncillary.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FrameAncillary.Size = New System.Drawing.Size(484, 51)
        Me.FrameAncillary.TabIndex = 17
        Me.FrameAncillary.TabStop = false
        Me.FrameAncillary.Visible = false
        '
        'txtAncillaryName
        '
        Me.txtAncillaryName.AcceptsReturn = true
        Me.txtAncillaryName.BackColor = System.Drawing.SystemColors.Window
        Me.txtAncillaryName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAncillaryName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtAncillaryName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAncillaryName.Location = New System.Drawing.Point(178, 22)
        Me.txtAncillaryName.MaxLength = 0
        Me.txtAncillaryName.Name = "txtAncillaryName"
        Me.txtAncillaryName.ReadOnly = true
        Me.txtAncillaryName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAncillaryName.Size = New System.Drawing.Size(219, 20)
        Me.txtAncillaryName.TabIndex = 19
        '
        'chkNonScheduledAncillary
        '
        Me.chkNonScheduledAncillary.BackColor = System.Drawing.SystemColors.Control
        Me.chkNonScheduledAncillary.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkNonScheduledAncillary.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkNonScheduledAncillary.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkNonScheduledAncillary.Location = New System.Drawing.Point(10, 1)
        Me.chkNonScheduledAncillary.Name = "chkNonScheduledAncillary"
        Me.chkNonScheduledAncillary.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkNonScheduledAncillary.Size = New System.Drawing.Size(303, 22)
        Me.chkNonScheduledAncillary.TabIndex = 18
        Me.chkNonScheduledAncillary.Text = "Is this for a Non-Scheduled Ancillary Personnel ?"
        Me.chkNonScheduledAncillary.UseVisualStyleBackColor = false
        '
        'lblAnicillaryName
        '
        Me.lblAnicillaryName.BackColor = System.Drawing.SystemColors.Control
        Me.lblAnicillaryName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAnicillaryName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblAnicillaryName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAnicillaryName.Location = New System.Drawing.Point(8, 26)
        Me.lblAnicillaryName.Name = "lblAnicillaryName"
        Me.lblAnicillaryName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAnicillaryName.Size = New System.Drawing.Size(164, 17)
        Me.lblAnicillaryName.TabIndex = 20
        Me.lblAnicillaryName.Text = "Ancillary Personal Name :"
        '
        'lvwPolicies
        '
        Me.lvwPolicies.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwPolicies, "")
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwPolicies, false)
        Me.lvwPolicies.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lvwPolicies.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwPolicies.FullRowSelect = true
        Me.lvwPolicies.HideSelection = false
        Me.listViewHelper1.SetItemClickMethod(Me.lvwPolicies, "")
        Me.lvwPolicies.LabelWrap = false
        Me.listViewHelper1.SetLargeIcons(Me.lvwPolicies, "")
        Me.lvwPolicies.Location = New System.Drawing.Point(2, 8)
        Me.lvwPolicies.MultiSelect = false
        Me.lvwPolicies.Name = "lvwPolicies"
        Me.lvwPolicies.Size = New System.Drawing.Size(488, 158)
        Me.listViewHelper1.SetSmallIcons(Me.lvwPolicies, "")
        Me.listViewHelper1.SetSorted(Me.lvwPolicies, false)
        Me.listViewHelper1.SetSortKey(Me.lvwPolicies, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwPolicies, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwPolicies.TabIndex = 21
        Me.lvwPolicies.UseCompatibleStateImageBehavior = false
        Me.lvwPolicies.View = System.Windows.Forms.View.Details
        Me.lvwPolicies.Visible = false
        '
        'frameComment
        '
        Me.frameComment.BackColor = System.Drawing.SystemColors.Control
        Me.frameComment.Controls.Add(Me.txtComment)
        Me.frameComment.Controls.Add(Me.lblComment)
        Me.frameComment.Cursor = System.Windows.Forms.Cursors.Default
        Me.frameComment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.frameComment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frameComment.Location = New System.Drawing.Point(4, 12)
        Me.frameComment.Name = "frameComment"
        Me.frameComment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frameComment.Size = New System.Drawing.Size(485, 131)
        Me.frameComment.TabIndex = 22
        Me.frameComment.Visible = false
        '
        'txtComment
        '
        Me.txtComment.AcceptsReturn = true
        Me.txtComment.BackColor = System.Drawing.SystemColors.HighlightText
        Me.txtComment.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtComment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtComment.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtComment.Location = New System.Drawing.Point(2, 28)
        Me.txtComment.MaxLength = 255
        Me.txtComment.Multiline = true
        Me.txtComment.Name = "txtComment"
        Me.txtComment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtComment.Size = New System.Drawing.Size(483, 99)
        Me.txtComment.TabIndex = 23
        '
        'lblComment
        '
        Me.lblComment.BackColor = System.Drawing.SystemColors.Control
        Me.lblComment.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblComment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblComment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblComment.Location = New System.Drawing.Point(28, 8)
        Me.lblComment.Name = "lblComment"
        Me.lblComment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblComment.Size = New System.Drawing.Size(149, 17)
        Me.lblComment.TabIndex = 24
        Me.lblComment.Text = "Insert Comment"
        '
        'lblStepHeading
        '
        Me.lblStepHeading.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.lblStepHeading.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStepHeading.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblStepHeading.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.lblStepHeading.Location = New System.Drawing.Point(4, 10)
        Me.lblStepHeading.Name = "lblStepHeading"
        Me.lblStepHeading.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStepHeading.Size = New System.Drawing.Size(491, 17)
        Me.lblStepHeading.TabIndex = 2
        Me.lblStepHeading.Text = "STEPS DESCRIPTION"
        Me.lblStepHeading.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'FrameStepsCovered
        '
        Me.FrameStepsCovered.BackColor = System.Drawing.SystemColors.Control
        Me.FrameStepsCovered.Controls.Add(Me.Image1)
        Me.FrameStepsCovered.Controls.Add(Me._imgTickChild_0)
        Me.FrameStepsCovered.Controls.Add(Me._imgTickParent_0)
        Me.FrameStepsCovered.Controls.Add(Me._lblTickChild_0)
        Me.FrameStepsCovered.Controls.Add(Me._lblStepChild_0)
        Me.FrameStepsCovered.Controls.Add(Me._lblStepParent_0)
        Me.FrameStepsCovered.Controls.Add(Me._lblTickParent_0)
        Me.FrameStepsCovered.Controls.Add(Me.lbl_StepsCovered)
        Me.FrameStepsCovered.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.FrameStepsCovered.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FrameStepsCovered.Location = New System.Drawing.Point(2, 19)
        Me.FrameStepsCovered.Name = "FrameStepsCovered"
        Me.FrameStepsCovered.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FrameStepsCovered.Size = New System.Drawing.Size(229, 295)
        Me.FrameStepsCovered.TabIndex = 9
        Me.FrameStepsCovered.TabStop = false
        '
        'Image1
        '
        Me.Image1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Image1.Image = CType(resources.GetObject("Image1.Image"),System.Drawing.Image)
        Me.Image1.Location = New System.Drawing.Point(30, 168)
        Me.Image1.Name = "Image1"
        Me.Image1.Size = New System.Drawing.Size(14, 13)
        Me.Image1.TabIndex = 0
        Me.Image1.TabStop = false
        Me.Image1.Visible = false
        '
        '_imgTickChild_0
        '
        Me._imgTickChild_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._imgTickChild_0.Location = New System.Drawing.Point(34, 62)
        Me._imgTickChild_0.Name = "_imgTickChild_0"
        Me._imgTickChild_0.Size = New System.Drawing.Size(13, 15)
        Me._imgTickChild_0.TabIndex = 1
        Me._imgTickChild_0.TabStop = false
        Me._imgTickChild_0.Visible = false
        '
        '_imgTickParent_0
        '
        Me._imgTickParent_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._imgTickParent_0.Location = New System.Drawing.Point(12, 42)
        Me._imgTickParent_0.Name = "_imgTickParent_0"
        Me._imgTickParent_0.Size = New System.Drawing.Size(13, 15)
        Me._imgTickParent_0.TabIndex = 2
        Me._imgTickParent_0.TabStop = false
        Me._imgTickParent_0.Visible = false
        '
        '_lblTickChild_0
        '
        Me._lblTickChild_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblTickChild_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblTickChild_0.Font = New System.Drawing.Font("Wingdings 2", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2,Byte))
        Me._lblTickChild_0.ForeColor = System.Drawing.SystemColors.InfoText
        Me._lblTickChild_0.Location = New System.Drawing.Point(34, 60)
        Me._lblTickChild_0.Name = "_lblTickChild_0"
        Me._lblTickChild_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblTickChild_0.Size = New System.Drawing.Size(13, 15)
        Me._lblTickChild_0.TabIndex = 13
        Me._lblTickChild_0.Text = "P"
        Me._lblTickChild_0.Visible = false
        '
        '_lblStepChild_0
        '
        Me._lblStepChild_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblStepChild_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblStepChild_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me._lblStepChild_0.ForeColor = System.Drawing.SystemColors.InfoText
        Me._lblStepChild_0.Location = New System.Drawing.Point(54, 62)
        Me._lblStepChild_0.Name = "_lblStepChild_0"
        Me._lblStepChild_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblStepChild_0.Size = New System.Drawing.Size(131, 15)
        Me._lblStepChild_0.TabIndex = 12
        '
        '_lblStepParent_0
        '
        Me._lblStepParent_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblStepParent_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblStepParent_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me._lblStepParent_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblStepParent_0.Location = New System.Drawing.Point(32, 42)
        Me._lblStepParent_0.Name = "_lblStepParent_0"
        Me._lblStepParent_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblStepParent_0.Size = New System.Drawing.Size(169, 15)
        Me._lblStepParent_0.TabIndex = 14
        Me._lblStepParent_0.Text = "Produce Credential Certificates"
        Me._lblStepParent_0.Visible = false
        '
        '_lblTickParent_0
        '
        Me._lblTickParent_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblTickParent_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblTickParent_0.Font = New System.Drawing.Font("Wingdings 2", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2,Byte))
        Me._lblTickParent_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblTickParent_0.Location = New System.Drawing.Point(12, 40)
        Me._lblTickParent_0.Name = "_lblTickParent_0"
        Me._lblTickParent_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblTickParent_0.Size = New System.Drawing.Size(13, 15)
        Me._lblTickParent_0.TabIndex = 11
        Me._lblTickParent_0.Text = "P"
        Me._lblTickParent_0.Visible = false
        '
        'lbl_StepsCovered
        '
        Me.lbl_StepsCovered.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.lbl_StepsCovered.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbl_StepsCovered.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lbl_StepsCovered.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.lbl_StepsCovered.Location = New System.Drawing.Point(4, 10)
        Me.lbl_StepsCovered.Name = "lbl_StepsCovered"
        Me.lbl_StepsCovered.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbl_StepsCovered.Size = New System.Drawing.Size(221, 17)
        Me.lbl_StepsCovered.TabIndex = 10
        Me.lbl_StepsCovered.Text = "STEPS COVERED"
        Me.lbl_StepsCovered.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'ImageListTick
        '
        Me.ImageListTick.ImageStream = CType(resources.GetObject("ImageListTick.ImageStream"),System.Windows.Forms.ImageListStreamer)
        Me.ImageListTick.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192,Byte),Integer), CType(CType(192,Byte),Integer), CType(CType(192,Byte),Integer))
        Me.ImageListTick.Images.SetKeyName(0, "")
        Me.ImageListTick.Images.SetKeyName(1, "")
        Me.ImageListTick.Images.SetKeyName(2, "")
        '
        'FrameNavigation
        '
        Me.FrameNavigation.BackColor = System.Drawing.SystemColors.Control
        Me.FrameNavigation.Controls.Add(Me.cmdFinish)
        Me.FrameNavigation.Controls.Add(Me.cmdLossRunPrint)
        Me.FrameNavigation.Controls.Add(Me.cmdBack)
        Me.FrameNavigation.Controls.Add(Me.cmdNext)
        Me.FrameNavigation.Controls.Add(Me.cmdExit)
        Me.FrameNavigation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.FrameNavigation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FrameNavigation.Location = New System.Drawing.Point(232, 274)
        Me.FrameNavigation.Name = "FrameNavigation"
        Me.FrameNavigation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FrameNavigation.Size = New System.Drawing.Size(501, 41)
        Me.FrameNavigation.TabIndex = 1
        Me.FrameNavigation.TabStop = false
        '
        'cmdFinish
        '
        Me.cmdFinish.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFinish.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFinish.Enabled = false
        Me.cmdFinish.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdFinish.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFinish.Location = New System.Drawing.Point(283, 12)
        Me.cmdFinish.Name = "cmdFinish"
        Me.cmdFinish.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFinish.Size = New System.Drawing.Size(137, 25)
        Me.cmdFinish.TabIndex = 3
        Me.cmdFinish.Text = "&Cert/Loss Run Print"
        Me.cmdFinish.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFinish.UseVisualStyleBackColor = false
        '
        'cmdLossRunPrint
        '
        Me.cmdLossRunPrint.BackColor = System.Drawing.SystemColors.Control
        Me.cmdLossRunPrint.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdLossRunPrint.Enabled = false
        Me.cmdLossRunPrint.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdLossRunPrint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdLossRunPrint.Location = New System.Drawing.Point(186, 12)
        Me.cmdLossRunPrint.Name = "cmdLossRunPrint"
        Me.cmdLossRunPrint.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdLossRunPrint.Size = New System.Drawing.Size(97, 25)
        Me.cmdLossRunPrint.TabIndex = 6
        Me.cmdLossRunPrint.Text = "&Loss Run Print"
        Me.cmdLossRunPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdLossRunPrint.UseVisualStyleBackColor = false
        '
        'cmdBack
        '
        Me.cmdBack.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBack.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBack.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdBack.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBack.Location = New System.Drawing.Point(8, 12)
        Me.cmdBack.Name = "cmdBack"
        Me.cmdBack.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBack.Size = New System.Drawing.Size(89, 25)
        Me.cmdBack.TabIndex = 5
        Me.cmdBack.Text = "&Back"
        Me.cmdBack.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdBack.UseVisualStyleBackColor = false
        '
        'cmdNext
        '
        Me.cmdNext.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNext.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNext.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdNext.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNext.Location = New System.Drawing.Point(97, 12)
        Me.cmdNext.Name = "cmdNext"
        Me.cmdNext.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNext.Size = New System.Drawing.Size(89, 25)
        Me.cmdNext.TabIndex = 4
        Me.cmdNext.Text = "&Next"
        Me.cmdNext.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNext.UseVisualStyleBackColor = false
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Location = New System.Drawing.Point(404, 12)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(89, 25)
        Me.cmdExit.TabIndex = 15
        Me.cmdExit.Text = "&Exit"
        Me.cmdExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdExit.UseVisualStyleBackColor = false
        '
        'LblLoss
        '
        Me.LblLoss.AutoSize = true
        Me.LblLoss.Location = New System.Drawing.Point(28, 65)
        Me.LblLoss.Name = "LblLoss"
        Me.LblLoss.Size = New System.Drawing.Size(149, 13)
        Me.LblLoss.TabIndex = 119
        Me.LblLoss.Text = "Number of Years for Loss Run"
        '
        'CboNumberOfYear
        '
        Me.CboNumberOfYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboNumberOfYear.FormattingEnabled = true
        Me.CboNumberOfYear.Items.AddRange(New Object() {"All", "5", "10"})
        Me.CboNumberOfYear.Location = New System.Drawing.Point(227, 57)
        Me.CboNumberOfYear.Name = "CboNumberOfYear"
        Me.CboNumberOfYear.Size = New System.Drawing.Size(136, 21)
        Me.CboNumberOfYear.TabIndex = 118
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(736, 317)
        Me.Controls.Add(Me.FrameDetails)
        Me.Controls.Add(Me.FrameStepsCovered)
        Me.Controls.Add(Me.FrameNavigation)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 29)
        Me.MaximizeBox = false
        Me.MinimizeBox = false
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Produce Loss History Letter and Certificate"
        Me.MainMenu1.ResumeLayout(false)
        Me.MainMenu1.PerformLayout
        Me.FrameDetails.ResumeLayout(false)
        Me.frameStepLabel.ResumeLayout(false)
        Me.FrameStepOptions.ResumeLayout(false)
        Me.FrameCertificates.ResumeLayout(false)
        Me.FrameClient.ResumeLayout(false)
        Me.FrameClient.PerformLayout
        Me.FrameCertHolder.ResumeLayout(false)
        Me.FrameCertHolder.PerformLayout
        Me.FrameAncillary.ResumeLayout(false)
        Me.FrameAncillary.PerformLayout
        Me.frameComment.ResumeLayout(false)
        Me.frameComment.PerformLayout
        Me.FrameStepsCovered.ResumeLayout(false)
        CType(Me.Image1,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me._imgTickChild_0,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me._imgTickParent_0,System.ComponentModel.ISupportInitialize).EndInit
        Me.FrameNavigation.ResumeLayout(false)
        CType(Me.listViewHelper1,System.ComponentModel.ISupportInitialize).EndInit
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub
	Sub InitializemnuRestart()
		Me.mnuRestart(1) = _mnuRestart_1
	End Sub
	Sub InitializemnuProcess()
		Me.mnuProcess(1) = _mnuProcess_1
	End Sub
	Sub InitializelblTickParent()
		Me.lblTickParent(0) = _lblTickParent_0
	End Sub
	Sub InitializelblTickChild()
		Me.lblTickChild(0) = _lblTickChild_0
	End Sub
	Sub InitializelblStepParent()
		Me.lblStepParent(0) = _lblStepParent_0
	End Sub
	Sub InitializelblStepChild()
		Me.lblStepChild(0) = _lblStepChild_0
	End Sub
	Sub InitializeimgTickParent()
		Me.imgTickParent(0) = _imgTickParent_0
	End Sub
	Sub InitializeimgTickChild()
		Me.imgTickChild(0) = _imgTickChild_0
	End Sub
 Friend WithEvents LblLoss As System.Windows.Forms.Label
 Friend WithEvents CboNumberOfYear As System.Windows.Forms.ComboBox
#End Region 
End Class