<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSplash
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents lblMsg As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Image1 As System.Windows.Forms.PictureBox
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents lblVersion As System.Windows.Forms.Label
	Public WithEvents lblDate As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents pnlMain As System.Windows.Forms.Panel
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSplash))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.pnlMain = New System.Windows.Forms.Panel
		Me.lblMsg = New System.Windows.Forms.Label
		Me.Label1 = New System.Windows.Forms.Label
		Me.Image1 = New System.Windows.Forms.PictureBox
		Me.Label2 = New System.Windows.Forms.Label
		Me.lblVersion = New System.Windows.Forms.Label
		Me.lblDate = New System.Windows.Forms.Label
		Me.Label3 = New System.Windows.Forms.Label
		Me.pnlMain.SuspendLayout()
		Me.SuspendLayout()
		' 
		' pnlMain
		' 
		Me.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.pnlMain.Controls.Add(Me.lblMsg)
		Me.pnlMain.Controls.Add(Me.Label1)
		Me.pnlMain.Controls.Add(Me.Image1)
		Me.pnlMain.Controls.Add(Me.Label2)
		Me.pnlMain.Controls.Add(Me.lblVersion)
		Me.pnlMain.Controls.Add(Me.lblDate)
		Me.pnlMain.Controls.Add(Me.Label3)
		Me.pnlMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pnlMain.Location = New System.Drawing.Point(0, 0)
		Me.pnlMain.Name = "pnlMain"
		Me.pnlMain.Size = New System.Drawing.Size(313, 201)
		Me.pnlMain.TabIndex = 0
		' 
		' lblMsg
		' 
		Me.lblMsg.AutoSize = False
		Me.lblMsg.BackColor = System.Drawing.SystemColors.Control
		Me.lblMsg.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMsg.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMsg.Enabled = True
		Me.lblMsg.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMsg.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMsg.Location = New System.Drawing.Point(52, 136)
		Me.lblMsg.Name = "lblMsg"
		Me.lblMsg.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMsg.Size = New System.Drawing.Size(249, 17)
		Me.lblMsg.TabIndex = 6
		Me.lblMsg.Text = "lblMsg"
		Me.lblMsg.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMsg.UseMnemonic = True
		Me.lblMsg.Visible = True
		' 
		' Label1
		' 
		Me.Label1.AutoSize = False
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.Enabled = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Location = New System.Drawing.Point(52, 32)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(233, 21)
		Me.Label1.TabIndex = 5
		Me.Label1.Text = "for Documaster Enterprise "
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' Image1
		' 
		Me.Image1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Image1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Image1.Enabled = True
		Me.Image1.Image = CType(resources.GetObject("Image1.Image"), System.Drawing.Image)
		Me.Image1.Location = New System.Drawing.Point(12, 12)
		Me.Image1.Name = "Image1"
		Me.Image1.Size = New System.Drawing.Size(32, 32)
		Me.Image1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.Image1.Visible = True
		' 
		' Label2
		' 
		Me.Label2.AutoSize = False
		Me.Label2.BackColor = System.Drawing.SystemColors.Control
		Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label2.Enabled = True
		Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 13.5!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label2.Location = New System.Drawing.Point(52, 12)
		Me.Label2.Name = "Label2"
		Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label2.Size = New System.Drawing.Size(109, 21)
		Me.Label2.TabIndex = 4
		Me.Label2.Text = "Harmoniser"
		Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label2.UseMnemonic = True
		Me.Label2.Visible = True
		' 
		' lblVersion
		' 
		Me.lblVersion.AutoSize = False
		Me.lblVersion.BackColor = System.Drawing.SystemColors.Control
		Me.lblVersion.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblVersion.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblVersion.Enabled = True
		Me.lblVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblVersion.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblVersion.Location = New System.Drawing.Point(52, 72)
		Me.lblVersion.Name = "lblVersion"
		Me.lblVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblVersion.Size = New System.Drawing.Size(245, 17)
		Me.lblVersion.TabIndex = 3
		Me.lblVersion.Text = "version"
		Me.lblVersion.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblVersion.UseMnemonic = True
		Me.lblVersion.Visible = True
		' 
		' lblDate
		' 
		Me.lblDate.AutoSize = False
		Me.lblDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDate.Enabled = True
		Me.lblDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDate.Location = New System.Drawing.Point(52, 92)
		Me.lblDate.Name = "lblDate"
		Me.lblDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDate.Size = New System.Drawing.Size(245, 21)
		Me.lblDate.TabIndex = 2
		Me.lblDate.Text = "Date"
		Me.lblDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDate.UseMnemonic = True
		Me.lblDate.Visible = True
		' 
		' Label3
		' 
		Me.Label3.AutoSize = False
		Me.Label3.BackColor = System.Drawing.SystemColors.Control
		Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label3.Enabled = True
		Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label3.Location = New System.Drawing.Point(164, 180)
		Me.Label3.Name = "Label3"
		Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label3.Size = New System.Drawing.Size(137, 17)
		Me.Label3.TabIndex = 1
		Me.Label3.Text = "Sirius Financial Solutions Plc"
		Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.Label3.UseMnemonic = True
		Me.Label3.Visible = True
		' 
		' frmSplash
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(369, 269)
		Me.ControlBox = True
		Me.Controls.Add(Me.pnlMain)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(0, 0)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmSplash"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.pnlMain.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class