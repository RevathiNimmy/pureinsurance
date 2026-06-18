<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDocumentLinkEdit
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents chkSAM As System.Windows.Forms.CheckBox
	Public WithEvents chkBO As System.Windows.Forms.CheckBox
	Public WithEvents OptUserChoice As System.Windows.Forms.RadioButton
	Public WithEvents OptPrinter As System.Windows.Forms.RadioButton
	Public WithEvents OptSpooler As System.Windows.Forms.RadioButton
	Public WithEvents cboDocumentType As System.Windows.Forms.ComboBox
	Public WithEvents cboProcess As System.Windows.Forms.ComboBox
	Public WithEvents cmdDeSelectDoc As System.Windows.Forms.Button
	Public WithEvents cmdSelectDoc As System.Windows.Forms.Button
	Public WithEvents chkOffice As System.Windows.Forms.CheckBox
	Public WithEvents chkAgent As System.Windows.Forms.CheckBox
	Public WithEvents chkClient As System.Windows.Forms.CheckBox
	Public WithEvents chkDefault As System.Windows.Forms.CheckBox
	Public WithEvents txtDocument As System.Windows.Forms.TextBox
	Public WithEvents cboBranch As PMLookupControl.cboPMLookup
	Public WithEvents lblFor As System.Windows.Forms.Label
	Public WithEvents lblDocumetType As System.Windows.Forms.Label
	Public WithEvents lblSendTo As System.Windows.Forms.Label
	Public WithEvents lblDocument As System.Windows.Forms.Label
	Public WithEvents lblBranch As System.Windows.Forms.Label
	Public WithEvents lblProcess As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOk = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.chkSAM = New System.Windows.Forms.CheckBox
        Me.chkBO = New System.Windows.Forms.CheckBox
        Me.OptUserChoice = New System.Windows.Forms.RadioButton
        Me.OptPrinter = New System.Windows.Forms.RadioButton
        Me.OptSpooler = New System.Windows.Forms.RadioButton
        Me.cboDocumentType = New System.Windows.Forms.ComboBox
        Me.cboProcess = New System.Windows.Forms.ComboBox
        Me.cmdDeSelectDoc = New System.Windows.Forms.Button
        Me.cmdSelectDoc = New System.Windows.Forms.Button
        Me.chkOffice = New System.Windows.Forms.CheckBox
        Me.chkAgent = New System.Windows.Forms.CheckBox
        Me.chkClient = New System.Windows.Forms.CheckBox
        Me.chkDefault = New System.Windows.Forms.CheckBox
        Me.txtDocument = New System.Windows.Forms.TextBox
        Me.cboBranch = New PMLookupControl.cboPMLookup
        Me.lblFor = New System.Windows.Forms.Label
        Me.lblDocumetType = New System.Windows.Forms.Label
        Me.lblSendTo = New System.Windows.Forms.Label
        Me.lblDocument = New System.Windows.Forms.Label
        Me.lblBranch = New System.Windows.Forms.Label
        Me.lblProcess = New System.Windows.Forms.Label
        Me.Frame1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdOk
        '
        Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOk.Location = New System.Drawing.Point(222, 278)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(65, 23)
        Me.cmdOk.TabIndex = 15
        Me.cmdOk.Text = "&OK"
        Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOk.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(293, 278)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(66, 22)
        Me.cmdCancel.TabIndex = 14
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.chkSAM)
        Me.Frame1.Controls.Add(Me.chkBO)
        Me.Frame1.Controls.Add(Me.OptUserChoice)
        Me.Frame1.Controls.Add(Me.OptPrinter)
        Me.Frame1.Controls.Add(Me.OptSpooler)
        Me.Frame1.Controls.Add(Me.cboDocumentType)
        Me.Frame1.Controls.Add(Me.cboProcess)
        Me.Frame1.Controls.Add(Me.cmdDeSelectDoc)
        Me.Frame1.Controls.Add(Me.cmdSelectDoc)
        Me.Frame1.Controls.Add(Me.chkOffice)
        Me.Frame1.Controls.Add(Me.chkAgent)
        Me.Frame1.Controls.Add(Me.chkClient)
        Me.Frame1.Controls.Add(Me.chkDefault)
        Me.Frame1.Controls.Add(Me.txtDocument)
        Me.Frame1.Controls.Add(Me.cboBranch)
        Me.Frame1.Controls.Add(Me.lblFor)
        Me.Frame1.Controls.Add(Me.lblDocumetType)
        Me.Frame1.Controls.Add(Me.lblSendTo)
        Me.Frame1.Controls.Add(Me.lblDocument)
        Me.Frame1.Controls.Add(Me.lblBranch)
        Me.Frame1.Controls.Add(Me.lblProcess)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(5, 6)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(356, 264)
        Me.Frame1.TabIndex = 0
        Me.Frame1.TabStop = False
        '
        'chkSAM
        '
        Me.chkSAM.BackColor = System.Drawing.SystemColors.Control
        Me.chkSAM.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSAM.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSAM.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSAM.Location = New System.Drawing.Point(100, 232)
        Me.chkSAM.Name = "chkSAM"
        Me.chkSAM.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSAM.Size = New System.Drawing.Size(214, 19)
        Me.chkSAM.TabIndex = 23
        Me.chkSAM.Text = "Generate through SAM"
        Me.chkSAM.UseVisualStyleBackColor = False
        '
        'chkBO
        '
        Me.chkBO.BackColor = System.Drawing.SystemColors.Control
        Me.chkBO.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkBO.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkBO.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkBO.Location = New System.Drawing.Point(100, 208)
        Me.chkBO.Name = "chkBO"
        Me.chkBO.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkBO.Size = New System.Drawing.Size(214, 19)
        Me.chkBO.TabIndex = 22
        Me.chkBO.Text = "Generate through Back Office"
        Me.chkBO.UseVisualStyleBackColor = False
        '
        'OptUserChoice
        '
        Me.OptUserChoice.BackColor = System.Drawing.SystemColors.Control
        Me.OptUserChoice.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptUserChoice.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptUserChoice.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptUserChoice.Location = New System.Drawing.Point(254, 178)
        Me.OptUserChoice.Name = "OptUserChoice"
        Me.OptUserChoice.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptUserChoice.Size = New System.Drawing.Size(89, 19)
        Me.OptUserChoice.TabIndex = 21
        Me.OptUserChoice.TabStop = True
        Me.OptUserChoice.Text = "User Choice"
        Me.OptUserChoice.UseVisualStyleBackColor = False
        '
        'OptPrinter
        '
        Me.OptPrinter.BackColor = System.Drawing.SystemColors.Control
        Me.OptPrinter.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptPrinter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptPrinter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptPrinter.Location = New System.Drawing.Point(98, 178)
        Me.OptPrinter.Name = "OptPrinter"
        Me.OptPrinter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptPrinter.Size = New System.Drawing.Size(59, 19)
        Me.OptPrinter.TabIndex = 20
        Me.OptPrinter.TabStop = True
        Me.OptPrinter.Text = "Printer"
        Me.OptPrinter.UseVisualStyleBackColor = False
        '
        'OptSpooler
        '
        Me.OptSpooler.BackColor = System.Drawing.SystemColors.Control
        Me.OptSpooler.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptSpooler.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptSpooler.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptSpooler.Location = New System.Drawing.Point(176, 178)
        Me.OptSpooler.Name = "OptSpooler"
        Me.OptSpooler.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptSpooler.Size = New System.Drawing.Size(65, 19)
        Me.OptSpooler.TabIndex = 19
        Me.OptSpooler.TabStop = True
        Me.OptSpooler.Text = "Spooler"
        Me.OptSpooler.UseVisualStyleBackColor = False
        '
        'cboDocumentType
        '
        Me.cboDocumentType.BackColor = System.Drawing.SystemColors.Window
        Me.cboDocumentType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboDocumentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDocumentType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboDocumentType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboDocumentType.Location = New System.Drawing.Point(104, 46)
        Me.cboDocumentType.Name = "cboDocumentType"
        Me.cboDocumentType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboDocumentType.Size = New System.Drawing.Size(176, 21)
        Me.cboDocumentType.TabIndex = 18
        '
        'cboProcess
        '
        Me.cboProcess.BackColor = System.Drawing.SystemColors.Window
        Me.cboProcess.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboProcess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboProcess.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboProcess.Location = New System.Drawing.Point(104, 14)
        Me.cboProcess.Name = "cboProcess"
        Me.cboProcess.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboProcess.Size = New System.Drawing.Size(176, 21)
        Me.cboProcess.TabIndex = 17
        '
        'cmdDeSelectDoc
        '
        Me.cmdDeSelectDoc.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeSelectDoc.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeSelectDoc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeSelectDoc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeSelectDoc.Location = New System.Drawing.Point(315, 112)
        Me.cmdDeSelectDoc.Name = "cmdDeSelectDoc"
        Me.cmdDeSelectDoc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeSelectDoc.Size = New System.Drawing.Size(36, 21)
        Me.cmdDeSelectDoc.TabIndex = 16
        Me.cmdDeSelectDoc.Text = "[ X ]"
        Me.cmdDeSelectDoc.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeSelectDoc.UseVisualStyleBackColor = False
        '
        'cmdSelectDoc
        '
        Me.cmdSelectDoc.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelectDoc.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelectDoc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSelectDoc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelectDoc.Location = New System.Drawing.Point(280, 112)
        Me.cmdSelectDoc.Name = "cmdSelectDoc"
        Me.cmdSelectDoc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelectDoc.Size = New System.Drawing.Size(34, 21)
        Me.cmdSelectDoc.TabIndex = 6
        Me.cmdSelectDoc.Text = "[...]"
        Me.cmdSelectDoc.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSelectDoc.UseVisualStyleBackColor = False
        '
        'chkOffice
        '
        Me.chkOffice.BackColor = System.Drawing.SystemColors.Control
        Me.chkOffice.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOffice.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOffice.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOffice.Location = New System.Drawing.Point(282, 146)
        Me.chkOffice.Name = "chkOffice"
        Me.chkOffice.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOffice.Size = New System.Drawing.Size(58, 19)
        Me.chkOffice.TabIndex = 5
        Me.chkOffice.Text = "Office"
        Me.chkOffice.UseVisualStyleBackColor = False
        '
        'chkAgent
        '
        Me.chkAgent.BackColor = System.Drawing.SystemColors.Control
        Me.chkAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAgent.Location = New System.Drawing.Point(222, 146)
        Me.chkAgent.Name = "chkAgent"
        Me.chkAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAgent.Size = New System.Drawing.Size(60, 19)
        Me.chkAgent.TabIndex = 4
        Me.chkAgent.Text = "Agent"
        Me.chkAgent.UseVisualStyleBackColor = False
        '
        'chkClient
        '
        Me.chkClient.BackColor = System.Drawing.SystemColors.Control
        Me.chkClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkClient.Location = New System.Drawing.Point(166, 146)
        Me.chkClient.Name = "chkClient"
        Me.chkClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkClient.Size = New System.Drawing.Size(57, 19)
        Me.chkClient.TabIndex = 3
        Me.chkClient.Text = "Client"
        Me.chkClient.UseVisualStyleBackColor = False
        '
        'chkDefault
        '
        Me.chkDefault.BackColor = System.Drawing.SystemColors.Control
        Me.chkDefault.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDefault.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDefault.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDefault.Location = New System.Drawing.Point(99, 146)
        Me.chkDefault.Name = "chkDefault"
        Me.chkDefault.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDefault.Size = New System.Drawing.Size(66, 19)
        Me.chkDefault.TabIndex = 2
        Me.chkDefault.Text = "Default"
        Me.chkDefault.UseVisualStyleBackColor = False
        '
        'txtDocument
        '
        Me.txtDocument.AcceptsReturn = True
        Me.txtDocument.BackColor = System.Drawing.SystemColors.Window
        Me.txtDocument.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDocument.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDocument.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDocument.Location = New System.Drawing.Point(104, 112)
        Me.txtDocument.MaxLength = 0
        Me.txtDocument.Name = "txtDocument"
        Me.txtDocument.ReadOnly = True
        Me.txtDocument.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDocument.Size = New System.Drawing.Size(174, 21)
        Me.txtDocument.TabIndex = 1
        '
        'cboBranch
        '
        Me.cboBranch.DefaultItemId = 0
        Me.cboBranch.FirstItem = ""
        Me.cboBranch.ItemId = 0
        Me.cboBranch.ListIndex = -1
        Me.cboBranch.Location = New System.Drawing.Point(104, 78)
        Me.cboBranch.Name = "cboBranch"
        Me.cboBranch.PMLookupProductFamily = 1
        Me.cboBranch.SingleItemId = 0
        Me.cboBranch.Size = New System.Drawing.Size(176, 21)
        Me.cboBranch.Sorted = True
        Me.cboBranch.TabIndex = 7
        Me.cboBranch.TableName = "Source"
        Me.cboBranch.ToolTipText = ""
        Me.cboBranch.WhereClause = ""
        '
        'lblFor
        '
        Me.lblFor.BackColor = System.Drawing.SystemColors.Control
        Me.lblFor.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFor.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFor.Location = New System.Drawing.Point(7, 146)
        Me.lblFor.Name = "lblFor"
        Me.lblFor.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFor.Size = New System.Drawing.Size(91, 18)
        Me.lblFor.TabIndex = 13
        Me.lblFor.Text = "For:"
        '
        'lblDocumetType
        '
        Me.lblDocumetType.BackColor = System.Drawing.SystemColors.Control
        Me.lblDocumetType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDocumetType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocumetType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDocumetType.Location = New System.Drawing.Point(7, 47)
        Me.lblDocumetType.Name = "lblDocumetType"
        Me.lblDocumetType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDocumetType.Size = New System.Drawing.Size(95, 18)
        Me.lblDocumetType.TabIndex = 12
        Me.lblDocumetType.Text = "Document Type:"
        '
        'lblSendTo
        '
        Me.lblSendTo.BackColor = System.Drawing.SystemColors.Control
        Me.lblSendTo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSendTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSendTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSendTo.Location = New System.Drawing.Point(7, 179)
        Me.lblSendTo.Name = "lblSendTo"
        Me.lblSendTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSendTo.Size = New System.Drawing.Size(90, 18)
        Me.lblSendTo.TabIndex = 11
        Me.lblSendTo.Text = "Send to:"
        '
        'lblDocument
        '
        Me.lblDocument.BackColor = System.Drawing.SystemColors.Control
        Me.lblDocument.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDocument.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocument.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDocument.Location = New System.Drawing.Point(7, 114)
        Me.lblDocument.Name = "lblDocument"
        Me.lblDocument.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDocument.Size = New System.Drawing.Size(91, 18)
        Me.lblDocument.TabIndex = 10
        Me.lblDocument.Text = "Document:"
        '
        'lblBranch
        '
        Me.lblBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranch.Location = New System.Drawing.Point(7, 79)
        Me.lblBranch.Name = "lblBranch"
        Me.lblBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranch.Size = New System.Drawing.Size(91, 18)
        Me.lblBranch.TabIndex = 9
        Me.lblBranch.Text = "Branch:"
        '
        'lblProcess
        '
        Me.lblProcess.BackColor = System.Drawing.SystemColors.Control
        Me.lblProcess.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProcess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProcess.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProcess.Location = New System.Drawing.Point(7, 15)
        Me.lblProcess.Name = "lblProcess"
        Me.lblProcess.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProcess.Size = New System.Drawing.Size(91, 18)
        Me.lblProcess.TabIndex = 8
        Me.lblProcess.Text = "Process:"
        '
        'frmDocumentLinkEdit
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(366, 310)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.Frame1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDocumentLinkEdit"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Document Link Reference"
        Me.Frame1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class