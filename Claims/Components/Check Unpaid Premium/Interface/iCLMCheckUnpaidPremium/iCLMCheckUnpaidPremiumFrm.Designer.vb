<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwTransactions_InitializeColumnKeys()
		lvwInstalments_InitializeColumnKeys()
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
	Public WithEvents chkOutstandingOnly As System.Windows.Forms.CheckBox
	Public WithEvents pnlClaimNumber As System.Windows.Forms.Panel
	Public WithEvents pnlClaimDate As System.Windows.Forms.Panel
	Public WithEvents pnlClient As System.Windows.Forms.Panel
	Public WithEvents pnlPolicyNumber As System.Windows.Forms.Panel
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdAbortPayment As System.Windows.Forms.Button
	Public WithEvents cmdContinuePayment As System.Windows.Forms.Button
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Private WithEvents _lvwInstalments_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwInstalments_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwInstalments_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwInstalments_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwInstalments_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwInstalments_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwInstalments_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwInstalments_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwInstalments_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwInstalments As System.Windows.Forms.ListView
	Private WithEvents _lvwTransactions_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTransactions_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTransactions_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTransactions_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTransactions_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTransactions_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTransactions_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwTransactions As System.Windows.Forms.ListView
	Public WithEvents PnlOverdueInstalments As System.Windows.Forms.Panel
	Public WithEvents LblOverdueInstalments As System.Windows.Forms.Label
	Public WithEvents lblClaimNumber As System.Windows.Forms.Label
	Public WithEvents lblClient As System.Windows.Forms.Label
	Public WithEvents lblClaimDate As System.Windows.Forms.Label
	Public WithEvents lblPolicyNumber As System.Windows.Forms.Label
    Public WithEvents imglImages As System.Windows.Forms.ImageList
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
        Me.chkOutstandingOnly = New System.Windows.Forms.CheckBox
        Me.pnlClaimNumber = New System.Windows.Forms.Panel
        Me.plblClaimNumber = New System.Windows.Forms.Label
        Me.pnlClaimDate = New System.Windows.Forms.Panel
        Me.plblClaimDate = New System.Windows.Forms.Label
        Me.pnlClient = New System.Windows.Forms.Panel
        Me.plblClient = New System.Windows.Forms.Label
        Me.pnlPolicyNumber = New System.Windows.Forms.Panel
        Me.plblPolicyNumber = New System.Windows.Forms.Label
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdAbortPayment = New System.Windows.Forms.Button
        Me.cmdContinuePayment = New System.Windows.Forms.Button
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.lvwInstalments = New System.Windows.Forms.ListView
        Me._lvwInstalments_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwInstalments_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwInstalments_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwInstalments_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwInstalments_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwInstalments_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwInstalments_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._lvwInstalments_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
        Me._lvwInstalments_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
        Me.imglImages = New System.Windows.Forms.ImageList(Me.components)
        Me.lvwTransactions = New System.Windows.Forms.ListView
        Me._lvwTransactions_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwTransactions_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwTransactions_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwTransactions_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwTransactions_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwTransactions_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwTransactions_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me.PnlOverdueInstalments = New System.Windows.Forms.Panel
        Me.pLblOverdueInstalments = New System.Windows.Forms.Label
        Me.LblOverdueInstalments = New System.Windows.Forms.Label
        Me.lblClaimNumber = New System.Windows.Forms.Label
        Me.lblClient = New System.Windows.Forms.Label
        Me.lblClaimDate = New System.Windows.Forms.Label
        Me.lblPolicyNumber = New System.Windows.Forms.Label
        Me.pnlClaimNumber.SuspendLayout()
        Me.pnlClaimDate.SuspendLayout()
        Me.pnlClient.SuspendLayout()
        Me.pnlPolicyNumber.SuspendLayout()
        Me.PnlOverdueInstalments.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkOutstandingOnly
        '
        Me.chkOutstandingOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkOutstandingOnly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOutstandingOnly.Checked = True
        Me.chkOutstandingOnly.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkOutstandingOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOutstandingOnly.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOutstandingOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOutstandingOnly.Location = New System.Drawing.Point(584, 8)
        Me.chkOutstandingOnly.Name = "chkOutstandingOnly"
        Me.chkOutstandingOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOutstandingOnly.Size = New System.Drawing.Size(87, 41)
        Me.chkOutstandingOnly.TabIndex = 2
        Me.chkOutstandingOnly.Text = "Show Outstanding Only"
        Me.chkOutstandingOnly.UseVisualStyleBackColor = False
        '
        'pnlClaimNumber
        '
        Me.pnlClaimNumber.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlClaimNumber.Controls.Add(Me.plblClaimNumber)
        Me.pnlClaimNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlClaimNumber.Location = New System.Drawing.Point(104, 32)
        Me.pnlClaimNumber.Name = "pnlClaimNumber"
        Me.pnlClaimNumber.Size = New System.Drawing.Size(177, 17)
        Me.pnlClaimNumber.TabIndex = 5
        '
        'plblClaimNumber
        '
        Me.plblClaimNumber.AutoSize = True
        Me.plblClaimNumber.Location = New System.Drawing.Point(2, -1)
        Me.plblClaimNumber.Name = "plblClaimNumber"
        Me.plblClaimNumber.Size = New System.Drawing.Size(0, 13)
        Me.plblClaimNumber.TabIndex = 0
        '
        'pnlClaimDate
        '
        Me.pnlClaimDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlClaimDate.Controls.Add(Me.plblClaimDate)
        Me.pnlClaimDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlClaimDate.Location = New System.Drawing.Point(392, 8)
        Me.pnlClaimDate.Name = "pnlClaimDate"
        Me.pnlClaimDate.Size = New System.Drawing.Size(177, 17)
        Me.pnlClaimDate.TabIndex = 9
        '
        'plblClaimDate
        '
        Me.plblClaimDate.AutoSize = True
        Me.plblClaimDate.Location = New System.Drawing.Point(1, -1)
        Me.plblClaimDate.Name = "plblClaimDate"
        Me.plblClaimDate.Size = New System.Drawing.Size(0, 13)
        Me.plblClaimDate.TabIndex = 0
        '
        'pnlClient
        '
        Me.pnlClient.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlClient.Controls.Add(Me.plblClient)
        Me.pnlClient.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlClient.Location = New System.Drawing.Point(392, 32)
        Me.pnlClient.Name = "pnlClient"
        Me.pnlClient.Size = New System.Drawing.Size(177, 17)
        Me.pnlClient.TabIndex = 11
        '
        'plblClient
        '
        Me.plblClient.AutoSize = True
        Me.plblClient.Location = New System.Drawing.Point(2, 0)
        Me.plblClient.Name = "plblClient"
        Me.plblClient.Size = New System.Drawing.Size(0, 13)
        Me.plblClient.TabIndex = 0
        '
        'pnlPolicyNumber
        '
        Me.pnlPolicyNumber.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlPolicyNumber.Controls.Add(Me.plblPolicyNumber)
        Me.pnlPolicyNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlPolicyNumber.Location = New System.Drawing.Point(104, 8)
        Me.pnlPolicyNumber.Name = "pnlPolicyNumber"
        Me.pnlPolicyNumber.Size = New System.Drawing.Size(177, 17)
        Me.pnlPolicyNumber.TabIndex = 6
        '
        'plblPolicyNumber
        '
        Me.plblPolicyNumber.AutoSize = True
        Me.plblPolicyNumber.Location = New System.Drawing.Point(2, 0)
        Me.plblPolicyNumber.Name = "plblPolicyNumber"
        Me.plblPolicyNumber.Size = New System.Drawing.Size(0, 13)
        Me.plblPolicyNumber.TabIndex = 0
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(600, 264)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 3
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdAbortPayment
        '
        Me.cmdAbortPayment.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAbortPayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAbortPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAbortPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAbortPayment.Location = New System.Drawing.Point(520, 264)
        Me.cmdAbortPayment.Name = "cmdAbortPayment"
        Me.cmdAbortPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAbortPayment.Size = New System.Drawing.Size(73, 22)
        Me.cmdAbortPayment.TabIndex = 1
        Me.cmdAbortPayment.Text = "Abort"
        Me.cmdAbortPayment.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAbortPayment.UseVisualStyleBackColor = False
        '
        'cmdContinuePayment
        '
        Me.cmdContinuePayment.BackColor = System.Drawing.SystemColors.Control
        Me.cmdContinuePayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdContinuePayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdContinuePayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdContinuePayment.Location = New System.Drawing.Point(440, 264)
        Me.cmdContinuePayment.Name = "cmdContinuePayment"
        Me.cmdContinuePayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdContinuePayment.Size = New System.Drawing.Size(73, 22)
        Me.cmdContinuePayment.TabIndex = 0
        Me.cmdContinuePayment.Text = "Continue"
        Me.cmdContinuePayment.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdContinuePayment.UseVisualStyleBackColor = False
        '
        'lvwInstalments
        '
        Me.lvwInstalments.BackColor = System.Drawing.SystemColors.Window
        Me.lvwInstalments.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwInstalments.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwInstalments_ColumnHeader_1, Me._lvwInstalments_ColumnHeader_2, Me._lvwInstalments_ColumnHeader_3, Me._lvwInstalments_ColumnHeader_4, Me._lvwInstalments_ColumnHeader_5, Me._lvwInstalments_ColumnHeader_6, Me._lvwInstalments_ColumnHeader_7, Me._lvwInstalments_ColumnHeader_8, Me._lvwInstalments_ColumnHeader_9})
        Me.lvwInstalments.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwInstalments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwInstalments.LargeImageList = Me.imglImages
        Me.lvwInstalments.Location = New System.Drawing.Point(8, 307)
        Me.lvwInstalments.Name = "lvwInstalments"
        Me.lvwInstalments.Size = New System.Drawing.Size(665, 145)
        Me.lvwInstalments.SmallImageList = Me.imglImages
        Me.lvwInstalments.TabIndex = 12
        Me.lvwInstalments.UseCompatibleStateImageBehavior = False
        Me.lvwInstalments.View = System.Windows.Forms.View.Details
        '
        '_lvwInstalments_ColumnHeader_1
        '
        Me._lvwInstalments_ColumnHeader_1.Tag = ""
        Me._lvwInstalments_ColumnHeader_1.Text = "Branch"
        Me._lvwInstalments_ColumnHeader_1.Width = 201
        '
        '_lvwInstalments_ColumnHeader_2
        '
        Me._lvwInstalments_ColumnHeader_2.Tag = ""
        Me._lvwInstalments_ColumnHeader_2.Text = "Account"
        Me._lvwInstalments_ColumnHeader_2.Width = 97
        '
        '_lvwInstalments_ColumnHeader_3
        '
        Me._lvwInstalments_ColumnHeader_3.Tag = ""
        Me._lvwInstalments_ColumnHeader_3.Text = "Doc Ref"
        Me._lvwInstalments_ColumnHeader_3.Width = 97
        '
        '_lvwInstalments_ColumnHeader_4
        '
        Me._lvwInstalments_ColumnHeader_4.Tag = ""
        Me._lvwInstalments_ColumnHeader_4.Text = "Instalment"
        Me._lvwInstalments_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwInstalments_ColumnHeader_4.Width = 97
        '
        '_lvwInstalments_ColumnHeader_5
        '
        Me._lvwInstalments_ColumnHeader_5.Tag = ""
        Me._lvwInstalments_ColumnHeader_5.Text = "Amount"
        Me._lvwInstalments_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwInstalments_ColumnHeader_5.Width = 97
        '
        '_lvwInstalments_ColumnHeader_6
        '
        Me._lvwInstalments_ColumnHeader_6.Tag = ""
        Me._lvwInstalments_ColumnHeader_6.Text = "Due Date"
        Me._lvwInstalments_ColumnHeader_6.Width = 97
        '
        '_lvwInstalments_ColumnHeader_7
        '
        Me._lvwInstalments_ColumnHeader_7.Tag = ""
        Me._lvwInstalments_ColumnHeader_7.Text = "Trans Code"
        Me._lvwInstalments_ColumnHeader_7.Width = 97
        '
        '_lvwInstalments_ColumnHeader_8
        '
        Me._lvwInstalments_ColumnHeader_8.Tag = ""
        Me._lvwInstalments_ColumnHeader_8.Text = "Status"
        Me._lvwInstalments_ColumnHeader_8.Width = 97
        '
        '_lvwInstalments_ColumnHeader_9
        '
        Me._lvwInstalments_ColumnHeader_9.Tag = ""
        Me._lvwInstalments_ColumnHeader_9.Text = "Posted Date"
        Me._lvwInstalments_ColumnHeader_9.Width = 97
        '
        'imglImages
        '
        Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.imglImages.Images.SetKeyName(0, "FindImage")
        '
        'lvwTransactions
        '
        Me.lvwTransactions.BackColor = System.Drawing.SystemColors.Window
        Me.lvwTransactions.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwTransactions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwTransactions_ColumnHeader_1, Me._lvwTransactions_ColumnHeader_2, Me._lvwTransactions_ColumnHeader_3, Me._lvwTransactions_ColumnHeader_4, Me._lvwTransactions_ColumnHeader_5, Me._lvwTransactions_ColumnHeader_6, Me._lvwTransactions_ColumnHeader_7})
        Me.lvwTransactions.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwTransactions.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwTransactions.LargeImageList = Me.imglImages
        Me.lvwTransactions.Location = New System.Drawing.Point(8, 96)
        Me.lvwTransactions.Name = "lvwTransactions"
        Me.lvwTransactions.Size = New System.Drawing.Size(665, 145)
        Me.lvwTransactions.SmallImageList = Me.imglImages
        Me.lvwTransactions.TabIndex = 13
        Me.lvwTransactions.TabStop = False
        Me.lvwTransactions.UseCompatibleStateImageBehavior = False
        Me.lvwTransactions.View = System.Windows.Forms.View.Details
        '
        '_lvwTransactions_ColumnHeader_1
        '
        Me._lvwTransactions_ColumnHeader_1.Tag = ""
        Me._lvwTransactions_ColumnHeader_1.Text = "Branch"
        Me._lvwTransactions_ColumnHeader_1.Width = 201
        '
        '_lvwTransactions_ColumnHeader_2
        '
        Me._lvwTransactions_ColumnHeader_2.Tag = ""
        Me._lvwTransactions_ColumnHeader_2.Text = "Account"
        Me._lvwTransactions_ColumnHeader_2.Width = 97
        '
        '_lvwTransactions_ColumnHeader_3
        '
        Me._lvwTransactions_ColumnHeader_3.Tag = ""
        Me._lvwTransactions_ColumnHeader_3.Text = "Doc Ref"
        Me._lvwTransactions_ColumnHeader_3.Width = 97
        '
        '_lvwTransactions_ColumnHeader_4
        '
        Me._lvwTransactions_ColumnHeader_4.Tag = ""
        Me._lvwTransactions_ColumnHeader_4.Text = "Trans Date"
        Me._lvwTransactions_ColumnHeader_4.Width = 97
        '
        '_lvwTransactions_ColumnHeader_5
        '
        Me._lvwTransactions_ColumnHeader_5.Tag = ""
        Me._lvwTransactions_ColumnHeader_5.Text = "Amount"
        Me._lvwTransactions_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwTransactions_ColumnHeader_5.Width = 97
        '
        '_lvwTransactions_ColumnHeader_6
        '
        Me._lvwTransactions_ColumnHeader_6.Tag = ""
        Me._lvwTransactions_ColumnHeader_6.Text = "OS Amount"
        Me._lvwTransactions_ColumnHeader_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwTransactions_ColumnHeader_6.Width = 97
        '
        '_lvwTransactions_ColumnHeader_7
        '
        Me._lvwTransactions_ColumnHeader_7.Tag = ""
        Me._lvwTransactions_ColumnHeader_7.Text = "Doc Type"
        Me._lvwTransactions_ColumnHeader_7.Width = 97
        '
        'PnlOverdueInstalments
        '
        Me.PnlOverdueInstalments.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PnlOverdueInstalments.Controls.Add(Me.pLblOverdueInstalments)
        Me.PnlOverdueInstalments.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PnlOverdueInstalments.Location = New System.Drawing.Point(104, 64)
        Me.PnlOverdueInstalments.Name = "PnlOverdueInstalments"
        Me.PnlOverdueInstalments.Size = New System.Drawing.Size(177, 17)
        Me.PnlOverdueInstalments.TabIndex = 14
        '
        'pLblOverdueInstalments
        '
        Me.pLblOverdueInstalments.AutoSize = True
        Me.pLblOverdueInstalments.Location = New System.Drawing.Point(2, 0)
        Me.pLblOverdueInstalments.Name = "pLblOverdueInstalments"
        Me.pLblOverdueInstalments.Size = New System.Drawing.Size(0, 13)
        Me.pLblOverdueInstalments.TabIndex = 0
        '
        'LblOverdueInstalments
        '
        Me.LblOverdueInstalments.BackColor = System.Drawing.SystemColors.Control
        Me.LblOverdueInstalments.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblOverdueInstalments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblOverdueInstalments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblOverdueInstalments.Location = New System.Drawing.Point(8, 56)
        Me.LblOverdueInstalments.Name = "LblOverdueInstalments"
        Me.LblOverdueInstalments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LblOverdueInstalments.Size = New System.Drawing.Size(89, 33)
        Me.LblOverdueInstalments.TabIndex = 15
        Me.LblOverdueInstalments.Text = "Overdue Instalments"
        '
        'lblClaimNumber
        '
        Me.lblClaimNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblClaimNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClaimNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClaimNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClaimNumber.Location = New System.Drawing.Point(8, 32)
        Me.lblClaimNumber.Name = "lblClaimNumber"
        Me.lblClaimNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClaimNumber.Size = New System.Drawing.Size(89, 17)
        Me.lblClaimNumber.TabIndex = 7
        Me.lblClaimNumber.Text = "Claim Number:"
        '
        'lblClient
        '
        Me.lblClient.BackColor = System.Drawing.SystemColors.Control
        Me.lblClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClient.Location = New System.Drawing.Point(304, 32)
        Me.lblClient.Name = "lblClient"
        Me.lblClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClient.Size = New System.Drawing.Size(72, 17)
        Me.lblClient.TabIndex = 10
        Me.lblClient.Text = "Client:"
        '
        'lblClaimDate
        '
        Me.lblClaimDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblClaimDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClaimDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClaimDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClaimDate.Location = New System.Drawing.Point(304, 8)
        Me.lblClaimDate.Name = "lblClaimDate"
        Me.lblClaimDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClaimDate.Size = New System.Drawing.Size(82, 17)
        Me.lblClaimDate.TabIndex = 8
        Me.lblClaimDate.Text = "Claim Date:"
        '
        'lblPolicyNumber
        '
        Me.lblPolicyNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyNumber.Location = New System.Drawing.Point(8, 8)
        Me.lblPolicyNumber.Name = "lblPolicyNumber"
        Me.lblPolicyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyNumber.Size = New System.Drawing.Size(90, 17)
        Me.lblPolicyNumber.TabIndex = 4
        Me.lblPolicyNumber.Text = "Policy Number:"
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(683, 303)
        Me.Controls.Add(Me.chkOutstandingOnly)
        Me.Controls.Add(Me.pnlClaimNumber)
        Me.Controls.Add(Me.pnlClaimDate)
        Me.Controls.Add(Me.pnlClient)
        Me.Controls.Add(Me.pnlPolicyNumber)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdAbortPayment)
        Me.Controls.Add(Me.cmdContinuePayment)
        Me.Controls.Add(Me.lvwInstalments)
        Me.Controls.Add(Me.lvwTransactions)
        Me.Controls.Add(Me.PnlOverdueInstalments)
        Me.Controls.Add(Me.LblOverdueInstalments)
        Me.Controls.Add(Me.lblClaimNumber)
        Me.Controls.Add(Me.lblClient)
        Me.Controls.Add(Me.lblClaimDate)
        Me.Controls.Add(Me.lblPolicyNumber)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(299, 243)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Unpaid Premium"
        Me.pnlClaimNumber.ResumeLayout(False)
        Me.pnlClaimNumber.PerformLayout()
        Me.pnlClaimDate.ResumeLayout(False)
        Me.pnlClaimDate.PerformLayout()
        Me.pnlClient.ResumeLayout(False)
        Me.pnlClient.PerformLayout()
        Me.pnlPolicyNumber.ResumeLayout(False)
        Me.pnlPolicyNumber.PerformLayout()
        Me.PnlOverdueInstalments.ResumeLayout(False)
        Me.PnlOverdueInstalments.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Sub lvwTransactions_InitializeColumnKeys()
        Me._lvwTransactions_ColumnHeader_1.Name = ""
        Me._lvwTransactions_ColumnHeader_2.Name = ""
        Me._lvwTransactions_ColumnHeader_3.Name = ""
        Me._lvwTransactions_ColumnHeader_4.Name = ""
        Me._lvwTransactions_ColumnHeader_5.Name = ""
        Me._lvwTransactions_ColumnHeader_6.Name = ""
        Me._lvwTransactions_ColumnHeader_7.Name = ""
    End Sub
    Sub lvwInstalments_InitializeColumnKeys()
        Me._lvwInstalments_ColumnHeader_1.Name = ""
        Me._lvwInstalments_ColumnHeader_2.Name = ""
        Me._lvwInstalments_ColumnHeader_3.Name = ""
        Me._lvwInstalments_ColumnHeader_4.Name = ""
        Me._lvwInstalments_ColumnHeader_5.Name = ""
        Me._lvwInstalments_ColumnHeader_6.Name = ""
        Me._lvwInstalments_ColumnHeader_7.Name = ""
        Me._lvwInstalments_ColumnHeader_8.Name = ""
        Me._lvwInstalments_ColumnHeader_9.Name = ""
    End Sub
    Friend WithEvents plblPolicyNumber As System.Windows.Forms.Label
    Friend WithEvents plblClaimNumber As System.Windows.Forms.Label
    Friend WithEvents pLblOverdueInstalments As System.Windows.Forms.Label
    Friend WithEvents plblClaimDate As System.Windows.Forms.Label
    Friend WithEvents plblClient As System.Windows.Forms.Label
#End Region 
End Class