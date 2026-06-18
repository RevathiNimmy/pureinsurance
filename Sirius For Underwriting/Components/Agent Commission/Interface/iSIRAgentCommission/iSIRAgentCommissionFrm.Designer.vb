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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblPolicyref As System.Windows.Forms.Label
	Public WithEvents lblPolicyHolderShort As System.Windows.Forms.Label
	Public WithEvents panPolicyHolder As System.Windows.Forms.Label
	Public WithEvents panPolicyRef As System.Windows.Forms.Label
	Public WithEvents panPolicyDesc As System.Windows.Forms.Label
	Public WithEvents panPolicyHolderFull As System.Windows.Forms.Label
	Public WithEvents uctCommission As uctPMUCommission.Commission
	Private WithEvents _frmInterface_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents frmInterface_frmInterface As System.Windows.Forms.TabControl
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
		Me.frmInterface_frmInterface = New System.Windows.Forms.TabControl
		Me._frmInterface_TabPage0 = New System.Windows.Forms.TabPage
		Me.lblPolicyref = New System.Windows.Forms.Label
		Me.lblPolicyHolderShort = New System.Windows.Forms.Label
		Me.panPolicyHolder = New System.Windows.Forms.Label
		Me.panPolicyRef = New System.Windows.Forms.Label
		Me.panPolicyDesc = New System.Windows.Forms.Label
		Me.panPolicyHolderFull = New System.Windows.Forms.Label
		Me.uctCommission = New uctPMUCommission.Commission
		Me.frmInterface_frmInterface.SuspendLayout()
		Me._frmInterface_TabPage0.SuspendLayout()
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
		Me.cmdCancel.Location = New System.Drawing.Point(684, 410)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(75, 24)
		Me.cmdCancel.TabIndex = 1
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
		Me.cmdOK.Location = New System.Drawing.Point(604, 410)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(75, 24)
		Me.cmdOK.TabIndex = 0
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' frmInterface_frmInterface
		' 
		Me.frmInterface_frmInterface.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.frmInterface_frmInterface.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.frmInterface_frmInterface.Controls.Add(Me._frmInterface_TabPage0)
		Me.frmInterface_frmInterface.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.frmInterface_frmInterface.ItemSize = New System.Drawing.Size(149, 18)
		Me.frmInterface_frmInterface.Location = New System.Drawing.Point(6, 8)
		Me.frmInterface_frmInterface.Multiline = True
		Me.frmInterface_frmInterface.Name = "frmInterface_frmInterface"
		Me.frmInterface_frmInterface.Size = New System.Drawing.Size(757, 401)
		Me.frmInterface_frmInterface.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.frmInterface_frmInterface.TabIndex = 2
		' 
		' _frmInterface_TabPage0
		' 
		Me._frmInterface_TabPage0.Controls.Add(Me.lblPolicyref)
		Me._frmInterface_TabPage0.Controls.Add(Me.lblPolicyHolderShort)
		Me._frmInterface_TabPage0.Controls.Add(Me.panPolicyHolder)
		Me._frmInterface_TabPage0.Controls.Add(Me.panPolicyRef)
		Me._frmInterface_TabPage0.Controls.Add(Me.panPolicyDesc)
		Me._frmInterface_TabPage0.Controls.Add(Me.panPolicyHolderFull)
		Me._frmInterface_TabPage0.Controls.Add(Me.uctCommission)
		Me._frmInterface_TabPage0.Text = "&1 - General"
		' 
		' lblPolicyref
		' 
		Me.lblPolicyref.AutoSize = False
		Me.lblPolicyref.BackColor = System.Drawing.SystemColors.Control
		Me.lblPolicyref.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPolicyref.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPolicyref.Enabled = True
		Me.lblPolicyref.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPolicyref.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPolicyref.Location = New System.Drawing.Point(12, 41)
		Me.lblPolicyref.Name = "lblPolicyref"
		Me.lblPolicyref.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPolicyref.Size = New System.Drawing.Size(81, 17)
		Me.lblPolicyref.TabIndex = 3
		Me.lblPolicyref.Text = "Policy Ref:"
		Me.lblPolicyref.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPolicyref.UseMnemonic = True
		Me.lblPolicyref.Visible = True
		' 
		' lblPolicyHolderShort
		' 
		Me.lblPolicyHolderShort.AutoSize = False
		Me.lblPolicyHolderShort.BackColor = System.Drawing.SystemColors.Control
		Me.lblPolicyHolderShort.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPolicyHolderShort.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPolicyHolderShort.Enabled = True
		Me.lblPolicyHolderShort.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPolicyHolderShort.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPolicyHolderShort.Location = New System.Drawing.Point(12, 15)
		Me.lblPolicyHolderShort.Name = "lblPolicyHolderShort"
		Me.lblPolicyHolderShort.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPolicyHolderShort.Size = New System.Drawing.Size(81, 17)
		Me.lblPolicyHolderShort.TabIndex = 4
		Me.lblPolicyHolderShort.Text = "Policy Holder:"
		Me.lblPolicyHolderShort.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPolicyHolderShort.UseMnemonic = True
		Me.lblPolicyHolderShort.Visible = True
		' 
		' panPolicyHolder
		' 
		Me.panPolicyHolder.AutoSize = False
		Me.panPolicyHolder.BackColor = System.Drawing.SystemColors.Control
		Me.panPolicyHolder.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.panPolicyHolder.Cursor = System.Windows.Forms.Cursors.Default
		Me.panPolicyHolder.Enabled = True
		Me.panPolicyHolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.panPolicyHolder.ForeColor = System.Drawing.SystemColors.ControlText
		Me.panPolicyHolder.Location = New System.Drawing.Point(96, 14)
		Me.panPolicyHolder.Name = "panPolicyHolder"
		Me.panPolicyHolder.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.panPolicyHolder.Size = New System.Drawing.Size(151, 19)
		Me.panPolicyHolder.TabIndex = 5
		Me.panPolicyHolder.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.panPolicyHolder.UseMnemonic = True
		Me.panPolicyHolder.Visible = True
		' 
		' panPolicyRef
		' 
		Me.panPolicyRef.AutoSize = False
		Me.panPolicyRef.BackColor = System.Drawing.SystemColors.Control
		Me.panPolicyRef.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.panPolicyRef.Cursor = System.Windows.Forms.Cursors.Default
		Me.panPolicyRef.Enabled = True
		Me.panPolicyRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.panPolicyRef.ForeColor = System.Drawing.SystemColors.ControlText
		Me.panPolicyRef.Location = New System.Drawing.Point(96, 38)
		Me.panPolicyRef.Name = "panPolicyRef"
		Me.panPolicyRef.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.panPolicyRef.Size = New System.Drawing.Size(151, 19)
		Me.panPolicyRef.TabIndex = 6
		Me.panPolicyRef.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.panPolicyRef.UseMnemonic = True
		Me.panPolicyRef.Visible = True
		' 
		' panPolicyDesc
		' 
		Me.panPolicyDesc.AutoSize = False
		Me.panPolicyDesc.BackColor = System.Drawing.SystemColors.Control
		Me.panPolicyDesc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.panPolicyDesc.Cursor = System.Windows.Forms.Cursors.Default
		Me.panPolicyDesc.Enabled = True
		Me.panPolicyDesc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.panPolicyDesc.ForeColor = System.Drawing.SystemColors.ControlText
		Me.panPolicyDesc.Location = New System.Drawing.Point(250, 38)
		Me.panPolicyDesc.Name = "panPolicyDesc"
		Me.panPolicyDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.panPolicyDesc.Size = New System.Drawing.Size(489, 19)
		Me.panPolicyDesc.TabIndex = 7
		Me.panPolicyDesc.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.panPolicyDesc.UseMnemonic = True
		Me.panPolicyDesc.Visible = True
		' 
		' panPolicyHolderFull
		' 
		Me.panPolicyHolderFull.AutoSize = False
		Me.panPolicyHolderFull.BackColor = System.Drawing.SystemColors.Control
		Me.panPolicyHolderFull.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.panPolicyHolderFull.Cursor = System.Windows.Forms.Cursors.Default
		Me.panPolicyHolderFull.Enabled = True
		Me.panPolicyHolderFull.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.panPolicyHolderFull.ForeColor = System.Drawing.SystemColors.ControlText
		Me.panPolicyHolderFull.Location = New System.Drawing.Point(250, 14)
		Me.panPolicyHolderFull.Name = "panPolicyHolderFull"
		Me.panPolicyHolderFull.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.panPolicyHolderFull.Size = New System.Drawing.Size(489, 19)
		Me.panPolicyHolderFull.TabIndex = 8
		Me.panPolicyHolderFull.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.panPolicyHolderFull.UseMnemonic = True
		Me.panPolicyHolderFull.Visible = True
		' 
		' uctCommission
		' 
		Me.uctCommission.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.uctCommission.Location = New System.Drawing.Point(12, 66)
		Me.uctCommission.Name = "uctCommission"
		Me.uctCommission.Size = New System.Drawing.Size(729, 297)
		Me.uctCommission.TabIndex = 9
		' 
		' frmInterface
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(766, 440)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.frmInterface_frmInterface)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "Agent Commission"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.frmInterface_frmInterface, 1)
		Me.frmInterface_frmInterface.ResumeLayout(False)
		Me._frmInterface_TabPage0.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class