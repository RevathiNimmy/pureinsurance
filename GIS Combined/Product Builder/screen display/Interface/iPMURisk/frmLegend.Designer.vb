<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLegend
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
	Public WithEvents lblPurchase As System.Windows.Forms.Label
	Public WithEvents lblPostQuote As System.Windows.Forms.Label
	Public WithEvents lblPreQuote As System.Windows.Forms.Label
	Public WithEvents lblNonMandatory As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
       Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLegend))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.lblPurchase = New System.Windows.Forms.Label
		Me.lblPostQuote = New System.Windows.Forms.Label
		Me.lblPreQuote = New System.Windows.Forms.Label
		Me.lblNonMandatory = New System.Windows.Forms.Label
		Me.SuspendLayout()
        '
        ' lblPurchase
		' 
		Me.lblPurchase.AutoSize = False
		Me.lblPurchase.BackColor = System.Drawing.SystemColors.Control
		Me.lblPurchase.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPurchase.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPurchase.Enabled = True
        Me.lblPurchase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.lblPurchase.ForeColor = System.Drawing.SystemColors.ControlDark
        Me.lblPurchase.Location = New System.Drawing.Point(8, 112)
        Me.lblPurchase.Name = "lblPurchase"
        Me.lblPurchase.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPurchase.Size = New System.Drawing.Size(145, 17)
        Me.lblPurchase.TabIndex = 3
        Me.lblPurchase.Text = "Mandatory at purchase"
        Me.lblPurchase.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPurchase.UseMnemonic = True
		Me.lblPurchase.Visible = True
		' 
		' lblPostQuote
		' 
		Me.lblPostQuote.AutoSize = False
		Me.lblPostQuote.BackColor = System.Drawing.SystemColors.Control
		Me.lblPostQuote.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPostQuote.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPostQuote.Enabled = True
		Me.lblPostQuote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPostQuote.ForeColor = System.Drawing.SystemColors.ControlDark
        Me.lblPostQuote.Location = New System.Drawing.Point(8, 80)
        Me.lblPostQuote.Name = "lblPostQuote"
        Me.lblPostQuote.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPostQuote.Size = New System.Drawing.Size(145, 17)
        Me.lblPostQuote.TabIndex = 2
        Me.lblPostQuote.Text = "Mandatory at post-quote"
        Me.lblPostQuote.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPostQuote.UseMnemonic = True
		Me.lblPostQuote.Visible = True
		' 
		' lblPreQuote
		' 
		Me.lblPreQuote.AutoSize = False
		Me.lblPreQuote.BackColor = System.Drawing.SystemColors.Control
		Me.lblPreQuote.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPreQuote.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPreQuote.Enabled = True
		Me.lblPreQuote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPreQuote.ForeColor = System.Drawing.SystemColors.Highlight
		Me.lblPreQuote.Location = New System.Drawing.Point(8, 48)
		Me.lblPreQuote.Name = "lblPreQuote"
		Me.lblPreQuote.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPreQuote.Size = New System.Drawing.Size(145, 17)
		Me.lblPreQuote.TabIndex = 1
		Me.lblPreQuote.Text = "Mandatory at pre-quote"
		Me.lblPreQuote.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPreQuote.UseMnemonic = True
		Me.lblPreQuote.Visible = True
		' 
		' lblNonMandatory
		' 
		Me.lblNonMandatory.AutoSize = False
		Me.lblNonMandatory.BackColor = System.Drawing.SystemColors.Control
		Me.lblNonMandatory.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblNonMandatory.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblNonMandatory.Enabled = True
		Me.lblNonMandatory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblNonMandatory.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblNonMandatory.Location = New System.Drawing.Point(8, 16)
		Me.lblNonMandatory.Name = "lblNonMandatory"
		Me.lblNonMandatory.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblNonMandatory.Size = New System.Drawing.Size(145, 17)
		Me.lblNonMandatory.TabIndex = 0
		Me.lblNonMandatory.Text = "Not Mandatory"
		Me.lblNonMandatory.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblNonMandatory.UseMnemonic = True
		Me.lblNonMandatory.Visible = True
		' 
		' frmLegend
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(159, 149)
		Me.ControlBox = True
		Me.Controls.Add(Me.lblPurchase)
		Me.Controls.Add(Me.lblPostQuote)
		Me.Controls.Add(Me.lblPreQuote)
		Me.Controls.Add(Me.lblNonMandatory)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmLegend"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Legend"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class