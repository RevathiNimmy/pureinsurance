<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLossScheduleType
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
	Public WithEvents lblLossScheduleType As System.Windows.Forms.Label
	Public WithEvents cboLossScheduleType As System.Windows.Forms.ComboBox
	Private WithEvents _tabLossScheduleType_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabLossScheduleType As System.Windows.Forms.TabControl
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLossScheduleType))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.tabLossScheduleType = New System.Windows.Forms.TabControl
		Me._tabLossScheduleType_TabPage0 = New System.Windows.Forms.TabPage
		Me.lblLossScheduleType = New System.Windows.Forms.Label
		Me.cboLossScheduleType = New System.Windows.Forms.ComboBox
		Me.tabLossScheduleType.SuspendLayout()
		Me._tabLossScheduleType_TabPage0.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(200, 112)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 1
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "Cancel"
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
		Me.cmdOK.Location = New System.Drawing.Point(0, 112)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 0
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "Ok"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tabLossScheduleType
		' 
		Me.tabLossScheduleType.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabLossScheduleType.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabLossScheduleType.Controls.Add(Me._tabLossScheduleType_TabPage0)
		Me.tabLossScheduleType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabLossScheduleType.ItemSize = New System.Drawing.Size(90, 18)
		Me.tabLossScheduleType.Location = New System.Drawing.Point(0, 0)
		Me.tabLossScheduleType.Multiline = True
		Me.tabLossScheduleType.Name = "tabLossScheduleType"
		Me.tabLossScheduleType.Size = New System.Drawing.Size(277, 109)
		Me.tabLossScheduleType.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabLossScheduleType.TabIndex = 2
		' 
		' _tabLossScheduleType_TabPage0
		' 
		Me._tabLossScheduleType_TabPage0.Controls.Add(Me.lblLossScheduleType)
		Me._tabLossScheduleType_TabPage0.Controls.Add(Me.cboLossScheduleType)
		Me._tabLossScheduleType_TabPage0.Text = "&1-Loss Schedule Type"
		' 
		' lblLossScheduleType
		' 
		Me.lblLossScheduleType.AutoSize = False
		Me.lblLossScheduleType.BackColor = System.Drawing.SystemColors.Control
		Me.lblLossScheduleType.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblLossScheduleType.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblLossScheduleType.Enabled = True
		Me.lblLossScheduleType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblLossScheduleType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLossScheduleType.Location = New System.Drawing.Point(7, 25)
		Me.lblLossScheduleType.Name = "lblLossScheduleType"
		Me.lblLossScheduleType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblLossScheduleType.Size = New System.Drawing.Size(101, 17)
		Me.lblLossScheduleType.TabIndex = 4
		Me.lblLossScheduleType.Text = "Loss Schedule Type"
		Me.lblLossScheduleType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblLossScheduleType.UseMnemonic = True
		Me.lblLossScheduleType.Visible = True
		' 
		' cboLossScheduleType
		' 
		Me.cboLossScheduleType.BackColor = System.Drawing.SystemColors.Window
		Me.cboLossScheduleType.CausesValidation = True
		Me.cboLossScheduleType.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboLossScheduleType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboLossScheduleType.Enabled = True
		Me.cboLossScheduleType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboLossScheduleType.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboLossScheduleType.IntegralHeight = True
		Me.cboLossScheduleType.Location = New System.Drawing.Point(120, 20)
		Me.cboLossScheduleType.Name = "cboLossScheduleType"
		Me.cboLossScheduleType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboLossScheduleType.Size = New System.Drawing.Size(135, 21)
		Me.cboLossScheduleType.Sorted = False
		Me.cboLossScheduleType.TabIndex = 3
		Me.cboLossScheduleType.TabStop = True
		Me.cboLossScheduleType.Visible = True
		' 
		' frmLossScheduleType
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(281, 142)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.tabLossScheduleType)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmLossScheduleType"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "Form1"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabLossScheduleType, 1)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tabLossScheduleType.ResumeLayout(False)
		Me._tabLossScheduleType_TabPage0.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class