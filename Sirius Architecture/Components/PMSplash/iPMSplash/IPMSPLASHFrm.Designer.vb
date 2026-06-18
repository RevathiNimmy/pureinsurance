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
	Public WithEvents lblPolicyMaster As System.Windows.Forms.Label
	Public WithEvents lblEmail As System.Windows.Forms.Label
	Public WithEvents lblAddress As System.Windows.Forms.Label
	Public WithEvents lblTelephone As System.Windows.Forms.Label
	Public WithEvents fraContact As System.Windows.Forms.GroupBox
	Public WithEvents lblTitleName As System.Windows.Forms.Label
	Public WithEvents fraTitle As System.Windows.Forms.GroupBox
	Public WithEvents picSplash As System.Windows.Forms.PictureBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.fraContact = New System.Windows.Forms.GroupBox
        Me.lblPolicyMaster = New System.Windows.Forms.Label
        Me.lblEmail = New System.Windows.Forms.Label
        Me.lblAddress = New System.Windows.Forms.Label
        Me.lblTelephone = New System.Windows.Forms.Label
        Me.fraTitle = New System.Windows.Forms.GroupBox
        Me.lblTitleName = New System.Windows.Forms.Label
        Me.picSplash = New System.Windows.Forms.PictureBox
        Me.fraContact.SuspendLayout()
        Me.fraTitle.SuspendLayout()
        CType(Me.picSplash, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'fraContact
        '
        Me.fraContact.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.fraContact.BackColor = System.Drawing.SystemColors.Control
        Me.fraContact.Controls.Add(Me.lblPolicyMaster)
        Me.fraContact.Controls.Add(Me.lblEmail)
        Me.fraContact.Controls.Add(Me.lblAddress)
        Me.fraContact.Controls.Add(Me.lblTelephone)
        Me.fraContact.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraContact.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraContact.Location = New System.Drawing.Point(8, 256)
        Me.fraContact.Name = "fraContact"
        Me.fraContact.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraContact.Size = New System.Drawing.Size(368, 109)
        Me.fraContact.TabIndex = 2
        Me.fraContact.TabStop = False
        '
        'lblPolicyMaster
        '
        Me.lblPolicyMaster.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblPolicyMaster.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyMaster.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyMaster.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyMaster.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyMaster.Location = New System.Drawing.Point(8, 16)
        Me.lblPolicyMaster.Name = "lblPolicyMaster"
        Me.lblPolicyMaster.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyMaster.Size = New System.Drawing.Size(340, 17)
        Me.lblPolicyMaster.TabIndex = 6
        Me.lblPolicyMaster.Text = "SSP Limited © "
        '
        'lblEmail
        '
        Me.lblEmail.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblEmail.BackColor = System.Drawing.SystemColors.Control
        Me.lblEmail.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEmail.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEmail.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEmail.Location = New System.Drawing.Point(8, 80)
        Me.lblEmail.Name = "lblEmail"
        Me.lblEmail.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEmail.Size = New System.Drawing.Size(355, 17)
        Me.lblEmail.TabIndex = 5
        Me.lblEmail.Text = "URL: http://www.ssp-uk.com/index.aspx    Email: info@ssp-worldwide.com"
        '
        'lblAddress
        '
        Me.lblAddress.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblAddress.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddress.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddress.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddress.Location = New System.Drawing.Point(8, 32)
        Me.lblAddress.Name = "lblAddress"
        Me.lblAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddress.Size = New System.Drawing.Size(354, 28)
        Me.lblAddress.TabIndex = 4
        Me.lblAddress.Text = "SSP Limited,  Fearnley Mill, Dean Clough, Halifax, West Yorkshire HX3 5AX, United" & _
            " Kingdom"
        '
        'lblTelephone
        '
        Me.lblTelephone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTelephone.BackColor = System.Drawing.SystemColors.Control
        Me.lblTelephone.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTelephone.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTelephone.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTelephone.Location = New System.Drawing.Point(8, 63)
        Me.lblTelephone.Name = "lblTelephone"
        Me.lblTelephone.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTelephone.Size = New System.Drawing.Size(354, 17)
        Me.lblTelephone.TabIndex = 3
        Me.lblTelephone.Text = "Tel: +44 (0)1422 330 022   Fax: +44 (0)1422 349 130"
        '
        'fraTitle
        '
        Me.fraTitle.BackColor = System.Drawing.SystemColors.Control
        Me.fraTitle.Controls.Add(Me.lblTitleName)
        Me.fraTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTitle.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTitle.Location = New System.Drawing.Point(8, 204)
        Me.fraTitle.Margin = New System.Windows.Forms.Padding(0)
        Me.fraTitle.Name = "fraTitle"
        Me.fraTitle.Padding = New System.Windows.Forms.Padding(0)
        Me.fraTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTitle.Size = New System.Drawing.Size(368, 51)
        Me.fraTitle.TabIndex = 1
        Me.fraTitle.TabStop = False
        '
        'lblTitleName
        '
        Me.lblTitleName.BackColor = System.Drawing.SystemColors.Control
        Me.lblTitleName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTitleName.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitleName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTitleName.Location = New System.Drawing.Point(8, 13)
        Me.lblTitleName.Name = "lblTitleName"
        Me.lblTitleName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTitleName.Size = New System.Drawing.Size(354, 32)
        Me.lblTitleName.TabIndex = 7
        Me.lblTitleName.Text = "Pure Insurance"
        Me.lblTitleName.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'picSplash
        '
        Me.picSplash.BackColor = System.Drawing.SystemColors.Window
        Me.picSplash.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.picSplash.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.picSplash.Cursor = System.Windows.Forms.Cursors.Default
        Me.picSplash.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.picSplash.Image = CType(resources.GetObject("picSplash.Image"), System.Drawing.Image)
        Me.picSplash.Location = New System.Drawing.Point(8, 8)
        Me.picSplash.Name = "picSplash"
        Me.picSplash.Size = New System.Drawing.Size(368, 190)
        Me.picSplash.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picSplash.TabIndex = 0
        Me.picSplash.TabStop = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(383, 372)
        Me.ControlBox = False
        Me.Controls.Add(Me.fraContact)
        Me.Controls.Add(Me.fraTitle)
        Me.Controls.Add(Me.picSplash)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TopMost = True
        Me.fraContact.ResumeLayout(False)
        Me.fraTitle.ResumeLayout(False)
        CType(Me.picSplash, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class