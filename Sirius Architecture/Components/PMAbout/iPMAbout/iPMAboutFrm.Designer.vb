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
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdSysInfo As System.Windows.Forms.Button
    Public WithEvents lblAddress As System.Windows.Forms.Label
    Public WithEvents lblDisclaimer As System.Windows.Forms.Label
    Public WithEvents lblCopyRight As System.Windows.Forms.Label
    Public Line1(1) As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdSysInfo = New System.Windows.Forms.Button
        Me.lblAddress = New System.Windows.Forms.Label
        Me.lblDisclaimer = New System.Windows.Forms.Label
        Me.lblCopyRight = New System.Windows.Forms.Label
        Me.lblVersion = New System.Windows.Forms.Label
        Me.lblPure = New System.Windows.Forms.Label
        Me.imgLogo = New System.Windows.Forms.PictureBox
        Me.cmdSupport = New System.Windows.Forms.Button
        CType(Me.imgLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdOK.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(272, 275)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(103, 25)
        Me.cmdOK.TabIndex = 2
        Me.cmdOK.Tag = "OK"
        Me.cmdOK.Text = "OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdSysInfo
        '
        Me.cmdSysInfo.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSysInfo.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSysInfo.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSysInfo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSysInfo.Location = New System.Drawing.Point(490, 275)
        Me.cmdSysInfo.Name = "cmdSysInfo"
        Me.cmdSysInfo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSysInfo.Size = New System.Drawing.Size(103, 25)
        Me.cmdSysInfo.TabIndex = 1
        Me.cmdSysInfo.Tag = "&System Info..."
        Me.cmdSysInfo.Text = "&System Info..."
        Me.cmdSysInfo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSysInfo.UseVisualStyleBackColor = False
        '
        'lblAddress
        '
        Me.lblAddress.AutoSize = True
        Me.lblAddress.BackColor = System.Drawing.SystemColors.Window
        Me.lblAddress.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddress.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddress.Location = New System.Drawing.Point(348, 39)
        Me.lblAddress.Name = "lblAddress"
        Me.lblAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddress.Size = New System.Drawing.Size(103, 15)
        Me.lblAddress.TabIndex = 10
        Me.lblAddress.Text = "Address goes here"
        '
        'lblDisclaimer
        '
        Me.lblDisclaimer.BackColor = System.Drawing.SystemColors.Window
        Me.lblDisclaimer.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDisclaimer.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDisclaimer.ForeColor = System.Drawing.Color.Black
        Me.lblDisclaimer.Location = New System.Drawing.Point(348, 167)
        Me.lblDisclaimer.Name = "lblDisclaimer"
        Me.lblDisclaimer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDisclaimer.Size = New System.Drawing.Size(245, 94)
        Me.lblDisclaimer.TabIndex = 7
        Me.lblDisclaimer.Text = resources.GetString("lblDisclaimer.Text")
        '
        'lblCopyRight
        '
        Me.lblCopyRight.BackColor = System.Drawing.SystemColors.Window
        Me.lblCopyRight.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCopyRight.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCopyRight.ForeColor = System.Drawing.Color.Black
        Me.lblCopyRight.Location = New System.Drawing.Point(348, 9)
        Me.lblCopyRight.Name = "lblCopyRight"
        Me.lblCopyRight.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCopyRight.Size = New System.Drawing.Size(253, 19)
        Me.lblCopyRight.TabIndex = 4
        Me.lblCopyRight.Tag = "App Description"
        Me.lblCopyRight.Text = "Copyright goes here"
        '
        'lblVersion
        '
        Me.lblVersion.BackColor = System.Drawing.SystemColors.Window
        Me.lblVersion.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblVersion.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVersion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblVersion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblVersion.Location = New System.Drawing.Point(12, 244)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblVersion.Size = New System.Drawing.Size(285, 17)
        Me.lblVersion.TabIndex = 16
        Me.lblVersion.Text = "Client Software Version Unavailable"
        Me.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblPure
        '
        Me.lblPure.AutoSize = True
        Me.lblPure.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPure.Location = New System.Drawing.Point(5, 199)
        Me.lblPure.Name = "lblPure"
        Me.lblPure.Size = New System.Drawing.Size(241, 45)
        Me.lblPure.TabIndex = 17
        Me.lblPure.Text = "Pure Insurance"
        '
        'imgLogo
        '
        Me.imgLogo.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgLogo.Image = CType(resources.GetObject("imgLogo.Image"), System.Drawing.Image)
        Me.imgLogo.Location = New System.Drawing.Point(-18, -28)
        Me.imgLogo.Name = "imgLogo"
        Me.imgLogo.Size = New System.Drawing.Size(367, 249)
        Me.imgLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.imgLogo.TabIndex = 15
        Me.imgLogo.TabStop = False
        '
        'cmdSupport
        '
        Me.cmdSupport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSupport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSupport.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSupport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSupport.Location = New System.Drawing.Point(381, 275)
        Me.cmdSupport.Name = "cmdSupport"
        Me.cmdSupport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSupport.Size = New System.Drawing.Size(103, 25)
        Me.cmdSupport.TabIndex = 18
        Me.cmdSupport.Tag = ""
        Me.cmdSupport.Text = "SSP Support"
        Me.cmdSupport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSupport.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.CancelButton = Me.cmdOK
        Me.ClientSize = New System.Drawing.Size(605, 312)
        Me.Controls.Add(Me.cmdSupport)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.lblPure)
        Me.Controls.Add(Me.imgLogo)
        Me.Controls.Add(Me.cmdSysInfo)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.lblDisclaimer)
        Me.Controls.Add(Me.lblAddress)
        Me.Controls.Add(Me.lblCopyRight)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = "About Viewstation"
        Me.Text = "About..."
        CType(Me.imgLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents lblPure As System.Windows.Forms.Label
    Public WithEvents imgLogo As System.Windows.Forms.PictureBox
    Public WithEvents cmdSupport As System.Windows.Forms.Button
#End Region 
End Class