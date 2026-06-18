<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctCLMVersions
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
	 Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			UserControl_Terminate()
			If Not (components Is Nothing) Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
	Friend WithEvents tvwClaims As System.Windows.Forms.TreeView
	Friend WithEvents lvwClaimVersions As System.Windows.Forms.ListView
	Friend WithEvents fraClaimVersions As System.Windows.Forms.GroupBox
	Friend WithEvents cmdViewCase As System.Windows.Forms.Button
	Friend WithEvents txtInsuranceRef As System.Windows.Forms.TextBox
	Friend WithEvents txtCaseNumber As System.Windows.Forms.TextBox
	Friend WithEvents lblPolicyNumber As System.Windows.Forms.Label
	Friend WithEvents lblCaseNumber As System.Windows.Forms.Label
	Friend WithEvents fraClaimInformation As System.Windows.Forms.GroupBox
	Friend WithEvents fraMain As System.Windows.Forms.GroupBox
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctCLMVersions))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.fraMain = New System.Windows.Forms.GroupBox
        Me.tvwClaims = New System.Windows.Forms.TreeView
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.fraClaimVersions = New System.Windows.Forms.GroupBox
        Me.lvwClaimVersions = New System.Windows.Forms.ListView
        Me.fraClaimInformation = New System.Windows.Forms.GroupBox
        Me.cmdViewCase = New System.Windows.Forms.Button
        Me.txtInsuranceRef = New System.Windows.Forms.TextBox
        Me.txtCaseNumber = New System.Windows.Forms.TextBox
        Me.lblPolicyNumber = New System.Windows.Forms.Label
        Me.lblCaseNumber = New System.Windows.Forms.Label
        Me.fraMain.SuspendLayout()
        Me.fraClaimVersions.SuspendLayout()
        Me.fraClaimInformation.SuspendLayout()
        Me.SuspendLayout()
        '
        'fraMain
        '
        Me.fraMain.BackColor = System.Drawing.SystemColors.Control
        Me.fraMain.Controls.Add(Me.tvwClaims)
        Me.fraMain.Controls.Add(Me.fraClaimVersions)
        Me.fraMain.Controls.Add(Me.fraClaimInformation)
        Me.fraMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraMain.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraMain.Location = New System.Drawing.Point(0, 0)
        Me.fraMain.Name = "fraMain"
        Me.fraMain.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraMain.Size = New System.Drawing.Size(745, 425)
        Me.fraMain.TabIndex = 0
        Me.fraMain.TabStop = False
        '
        'tvwClaims
        '
        Me.tvwClaims.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tvwClaims.ImageIndex = 1
        Me.tvwClaims.ImageList = Me.ImageList1
        Me.tvwClaims.LabelEdit = True
        Me.tvwClaims.Location = New System.Drawing.Point(8, 22)
        Me.tvwClaims.Name = "tvwClaims"
        Me.tvwClaims.SelectedImageIndex = 1
        Me.tvwClaims.ShowRootLines = False
        Me.tvwClaims.Size = New System.Drawing.Size(185, 393)
        Me.tvwClaims.TabIndex = 8
        Me.tvwClaims.LabelEdit = False
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "open")
        Me.ImageList1.Images.SetKeyName(1, "closed")
        Me.ImageList1.Images.SetKeyName(2, "selected")
        '
        'fraClaimVersions
        '
        Me.fraClaimVersions.BackColor = System.Drawing.SystemColors.Control
        Me.fraClaimVersions.Controls.Add(Me.lvwClaimVersions)
        Me.fraClaimVersions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraClaimVersions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraClaimVersions.Location = New System.Drawing.Point(200, 119)
        Me.fraClaimVersions.Name = "fraClaimVersions"
        Me.fraClaimVersions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraClaimVersions.Size = New System.Drawing.Size(537, 299)
        Me.fraClaimVersions.TabIndex = 6
        Me.fraClaimVersions.TabStop = False
        Me.fraClaimVersions.Text = "Claims Versions"
        '
        'lvwClaimVersions
        '
        Me.lvwClaimVersions.BackColor = System.Drawing.SystemColors.Window
        Me.lvwClaimVersions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwClaimVersions.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwClaimVersions.FullRowSelect = True
        Me.lvwClaimVersions.GridLines = True
        Me.lvwClaimVersions.HideSelection = False
        Me.lvwClaimVersions.Location = New System.Drawing.Point(8, 16)
        Me.lvwClaimVersions.Name = "lvwClaimVersions"
        Me.lvwClaimVersions.Size = New System.Drawing.Size(521, 313)
        Me.lvwClaimVersions.TabIndex = 7
        Me.lvwClaimVersions.UseCompatibleStateImageBehavior = False
        Me.lvwClaimVersions.View = System.Windows.Forms.View.Details
        '
        'fraClaimInformation
        '
        Me.fraClaimInformation.BackColor = System.Drawing.SystemColors.Control
        Me.fraClaimInformation.Controls.Add(Me.cmdViewCase)
        Me.fraClaimInformation.Controls.Add(Me.txtInsuranceRef)
        Me.fraClaimInformation.Controls.Add(Me.txtCaseNumber)
        Me.fraClaimInformation.Controls.Add(Me.lblPolicyNumber)
        Me.fraClaimInformation.Controls.Add(Me.lblCaseNumber)
        Me.fraClaimInformation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraClaimInformation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraClaimInformation.Location = New System.Drawing.Point(200, 16)
        Me.fraClaimInformation.Name = "fraClaimInformation"
        Me.fraClaimInformation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraClaimInformation.Size = New System.Drawing.Size(537, 97)
        Me.fraClaimInformation.TabIndex = 1
        Me.fraClaimInformation.TabStop = False
        Me.fraClaimInformation.Text = "Claim Information"
        '
        'cmdViewCase
        '
        Me.cmdViewCase.BackColor = System.Drawing.SystemColors.Control
        Me.cmdViewCase.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdViewCase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdViewCase.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdViewCase.Location = New System.Drawing.Point(449, 58)
        Me.cmdViewCase.Name = "cmdViewCase"
        Me.cmdViewCase.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdViewCase.Size = New System.Drawing.Size(81, 25)
        Me.cmdViewCase.TabIndex = 9
        Me.cmdViewCase.Text = "View Case"
        Me.cmdViewCase.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdViewCase.UseVisualStyleBackColor = False
        Me.cmdViewCase.Visible = False
        '
        'txtInsuranceRef
        '
        Me.txtInsuranceRef.AcceptsReturn = True
        Me.txtInsuranceRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtInsuranceRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInsuranceRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInsuranceRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInsuranceRef.Location = New System.Drawing.Point(104, 62)
        Me.txtInsuranceRef.MaxLength = 0
        Me.txtInsuranceRef.Name = "txtInsuranceRef"
        Me.txtInsuranceRef.ReadOnly = True
        Me.txtInsuranceRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInsuranceRef.Size = New System.Drawing.Size(134, 20)
        Me.txtInsuranceRef.TabIndex = 5
        '
        'txtCaseNumber
        '
        Me.txtCaseNumber.AcceptsReturn = True
        Me.txtCaseNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtCaseNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCaseNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCaseNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCaseNumber.Location = New System.Drawing.Point(310, 60)
        Me.txtCaseNumber.MaxLength = 0
        Me.txtCaseNumber.Name = "txtCaseNumber"
        Me.txtCaseNumber.ReadOnly = True
        Me.txtCaseNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCaseNumber.Size = New System.Drawing.Size(134, 20)
        Me.txtCaseNumber.TabIndex = 4
        Me.txtCaseNumber.Visible = False
        '
        'lblPolicyNumber
        '
        Me.lblPolicyNumber.AutoSize = True
        Me.lblPolicyNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyNumber.Location = New System.Drawing.Point(12, 67)
        Me.lblPolicyNumber.Name = "lblPolicyNumber"
        Me.lblPolicyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyNumber.Size = New System.Drawing.Size(78, 13)
        Me.lblPolicyNumber.TabIndex = 3
        Me.lblPolicyNumber.Text = "Policy Number:"
        '
        'lblCaseNumber
        '
        Me.lblCaseNumber.AutoSize = True
        Me.lblCaseNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblCaseNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCaseNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCaseNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCaseNumber.Location = New System.Drawing.Point(256, 65)
        Me.lblCaseNumber.Name = "lblCaseNumber"
        Me.lblCaseNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCaseNumber.Size = New System.Drawing.Size(34, 13)
        Me.lblCaseNumber.TabIndex = 2
        Me.lblCaseNumber.Text = "Case:"
        Me.lblCaseNumber.Visible = False
        '
        'uctCLMVersions
        '
        Me.Controls.Add(Me.fraMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctCLMVersions"
        Me.Size = New System.Drawing.Size(749, 430)
        Me.fraMain.ResumeLayout(False)
        Me.fraClaimVersions.ResumeLayout(False)
        Me.fraClaimInformation.ResumeLayout(False)
        Me.fraClaimInformation.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class