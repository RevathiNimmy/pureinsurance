<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cboCurrency As PMLookupControl.cboPMLookup
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	Public WithEvents lblText2 As System.Windows.Forms.Label
	Public WithEvents lblText1 As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cboCurrency = New PMLookupControl.cboPMLookup
		Me.lblCurrency = New System.Windows.Forms.Label
		Me.lblText2 = New System.Windows.Forms.Label
		Me.lblText1 = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(256, 160)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 5
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(176, 160)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 4
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cboCurrency
		' 
		Me.cboCurrency.Location = New System.Drawing.Point(112, 120)
		Me.cboCurrency.Name = "cboCurrency"
		Me.cboCurrency.Size = New System.Drawing.Size(177, 21)
		Me.cboCurrency.TabIndex = 3
        'Developer Guide No.77
        'Me.cboCurrency.Table = "Currency"
        Me.cboCurrency.TableName = "Currency"
		' 
		' lblCurrency
		' 
		Me.lblCurrency.AutoSize = False
		Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
		Me.lblCurrency.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCurrency.Enabled = True
		Me.lblCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCurrency.Location = New System.Drawing.Point(8, 120)
		Me.lblCurrency.Name = "lblCurrency"
		Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCurrency.Size = New System.Drawing.Size(97, 17)
		Me.lblCurrency.TabIndex = 2
		Me.lblCurrency.Text = "Currency:"
		Me.lblCurrency.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblCurrency.UseMnemonic = True
		Me.lblCurrency.Visible = True
		' 
		' lblText2
		' 
		Me.lblText2.AutoSize = False
		Me.lblText2.BackColor = System.Drawing.SystemColors.Control
		Me.lblText2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblText2.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblText2.Enabled = True
		Me.lblText2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblText2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblText2.Location = New System.Drawing.Point(8, 56)
		Me.lblText2.Name = "lblText2"
		Me.lblText2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblText2.Size = New System.Drawing.Size(321, 41)
		Me.lblText2.TabIndex = 1
		Me.lblText2.Text = "WARNING: You can only set the system currency when Sirius is installed and there have been no transactions raised. Once transactions have been posted, the system currency is read-only."
		Me.lblText2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblText2.UseMnemonic = True
		Me.lblText2.Visible = True
		' 
		' lblText1
		' 
		Me.lblText1.AutoSize = False
		Me.lblText1.BackColor = System.Drawing.SystemColors.Control
		Me.lblText1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblText1.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblText1.Enabled = True
		Me.lblText1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblText1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblText1.Location = New System.Drawing.Point(8, 16)
		Me.lblText1.Name = "lblText1"
		Me.lblText1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblText1.Size = New System.Drawing.Size(321, 33)
		Me.lblText1.TabIndex = 0
		Me.lblText1.Text = "The system currency is used across all branches for consolidating transactions using a single currency."
		Me.lblText1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblText1.UseMnemonic = True
		Me.lblText1.Visible = True
		' 
		' frmInterface
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(337, 189)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cboCurrency)
		Me.Controls.Add(Me.lblCurrency)
		Me.Controls.Add(Me.lblText2)
		Me.Controls.Add(Me.lblText1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmInterface.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "System Currency"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class