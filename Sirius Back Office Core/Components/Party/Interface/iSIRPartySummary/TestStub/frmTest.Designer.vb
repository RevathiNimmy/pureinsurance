<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTest
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
	Public WithEvents cmdSwitchTo As System.Windows.Forms.Button
	Public WithEvents cmdTerminate As System.Windows.Forms.Button
	Public WithEvents cboDisplayMode As System.Windows.Forms.ComboBox
	Public WithEvents txtPartyShortName As System.Windows.Forms.TextBox
	Public WithEvents txtPartyCnt As System.Windows.Forms.TextBox
	Public WithEvents cmtTest As System.Windows.Forms.Button
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Private WithEvents listBoxComboBoxHelper1 As Artinsoft.VB6.Gui.ListControlHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTest))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdSwitchTo = New System.Windows.Forms.Button
		Me.cmdTerminate = New System.Windows.Forms.Button
		Me.cboDisplayMode = New System.Windows.Forms.ComboBox
		Me.txtPartyShortName = New System.Windows.Forms.TextBox
		Me.txtPartyCnt = New System.Windows.Forms.TextBox
		Me.cmtTest = New System.Windows.Forms.Button
		Me.Label3 = New System.Windows.Forms.Label
		Me.Label2 = New System.Windows.Forms.Label
		Me.Label1 = New System.Windows.Forms.Label
		Me.SuspendLayout()
		Me.listBoxComboBoxHelper1 = New Artinsoft.VB6.Gui.ListControlHelper(Me.components)
		CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdSwitchTo
		' 
		Me.cmdSwitchTo.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSwitchTo.CausesValidation = True
		Me.cmdSwitchTo.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSwitchTo.Enabled = True
		Me.cmdSwitchTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdSwitchTo.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSwitchTo.Location = New System.Drawing.Point(120, 120)
		Me.cmdSwitchTo.Name = "cmdSwitchTo"
		Me.cmdSwitchTo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSwitchTo.Size = New System.Drawing.Size(89, 25)
		Me.cmdSwitchTo.TabIndex = 8
		Me.cmdSwitchTo.TabStop = True
		Me.cmdSwitchTo.Text = "Switch To"
		Me.cmdSwitchTo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSwitchTo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdTerminate
		' 
		Me.cmdTerminate.BackColor = System.Drawing.SystemColors.Control
		Me.cmdTerminate.CausesValidation = True
		Me.cmdTerminate.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdTerminate.Enabled = True
		Me.cmdTerminate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdTerminate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdTerminate.Location = New System.Drawing.Point(216, 120)
		Me.cmdTerminate.Name = "cmdTerminate"
		Me.cmdTerminate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdTerminate.Size = New System.Drawing.Size(89, 25)
		Me.cmdTerminate.TabIndex = 7
		Me.cmdTerminate.TabStop = True
		Me.cmdTerminate.Text = "Terminate"
		Me.cmdTerminate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdTerminate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cboDisplayMode
		' 
		Me.cboDisplayMode.BackColor = System.Drawing.SystemColors.Window
		Me.cboDisplayMode.CausesValidation = True
		Me.cboDisplayMode.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboDisplayMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboDisplayMode.Enabled = True
		Me.cboDisplayMode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboDisplayMode.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboDisplayMode.IntegralHeight = True
		Me.cboDisplayMode.Location = New System.Drawing.Point(120, 88)
		Me.cboDisplayMode.Name = "cboDisplayMode"
		Me.cboDisplayMode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboDisplayMode.Size = New System.Drawing.Size(177, 21)
		Me.cboDisplayMode.Sorted = False
		Me.cboDisplayMode.TabIndex = 6
		Me.cboDisplayMode.TabStop = True
		Me.cboDisplayMode.Visible = True
		Me.cboDisplayMode.Items.AddRange(New Object(){"Show Modal", "Show Modeless"})
		' 
		' txtPartyShortName
		' 
		Me.txtPartyShortName.AcceptsReturn = True
		Me.txtPartyShortName.AutoSize = False
		Me.txtPartyShortName.BackColor = System.Drawing.SystemColors.Window
		Me.txtPartyShortName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPartyShortName.CausesValidation = True
		Me.txtPartyShortName.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPartyShortName.Enabled = True
		Me.txtPartyShortName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPartyShortName.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPartyShortName.HideSelection = True
		Me.txtPartyShortName.Location = New System.Drawing.Point(120, 56)
		Me.txtPartyShortName.MaxLength = 0
		Me.txtPartyShortName.Multiline = False
		Me.txtPartyShortName.Name = "txtPartyShortName"
		Me.txtPartyShortName.ReadOnly = False
		Me.txtPartyShortName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPartyShortName.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPartyShortName.Size = New System.Drawing.Size(177, 25)
		Me.txtPartyShortName.TabIndex = 3
		Me.txtPartyShortName.TabStop = True
		Me.txtPartyShortName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPartyShortName.Visible = True
		' 
		' txtPartyCnt
		' 
		Me.txtPartyCnt.AcceptsReturn = True
		Me.txtPartyCnt.AutoSize = False
		Me.txtPartyCnt.BackColor = System.Drawing.SystemColors.Window
		Me.txtPartyCnt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPartyCnt.CausesValidation = True
		Me.txtPartyCnt.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPartyCnt.Enabled = True
		Me.txtPartyCnt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPartyCnt.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPartyCnt.HideSelection = True
		Me.txtPartyCnt.Location = New System.Drawing.Point(120, 24)
		Me.txtPartyCnt.MaxLength = 0
		Me.txtPartyCnt.Multiline = False
		Me.txtPartyCnt.Name = "txtPartyCnt"
		Me.txtPartyCnt.ReadOnly = False
		Me.txtPartyCnt.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPartyCnt.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPartyCnt.Size = New System.Drawing.Size(177, 25)
		Me.txtPartyCnt.TabIndex = 1
		Me.txtPartyCnt.TabStop = True
		Me.txtPartyCnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPartyCnt.Visible = True
		' 
		' cmtTest
		' 
		Me.cmtTest.BackColor = System.Drawing.SystemColors.Control
		Me.cmtTest.CausesValidation = True
		Me.cmtTest.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmtTest.Enabled = True
		Me.cmtTest.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmtTest.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmtTest.Location = New System.Drawing.Point(24, 120)
		Me.cmtTest.Name = "cmtTest"
		Me.cmtTest.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmtTest.Size = New System.Drawing.Size(89, 25)
		Me.cmtTest.TabIndex = 0
		Me.cmtTest.TabStop = True
		Me.cmtTest.Text = "Test It"
		Me.cmtTest.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmtTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
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
		Me.Label3.Location = New System.Drawing.Point(8, 88)
		Me.Label3.Name = "Label3"
		Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label3.Size = New System.Drawing.Size(105, 17)
		Me.Label3.TabIndex = 5
		Me.Label3.Text = "Display Mode"
		Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label3.UseMnemonic = True
		Me.Label3.Visible = True
		' 
		' Label2
		' 
		Me.Label2.AutoSize = False
		Me.Label2.BackColor = System.Drawing.SystemColors.Control
		Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label2.Enabled = True
		Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label2.Location = New System.Drawing.Point(8, 56)
		Me.Label2.Name = "Label2"
		Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label2.Size = New System.Drawing.Size(105, 17)
		Me.Label2.TabIndex = 4
		Me.Label2.Text = "Party Short Name"
		Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label2.UseMnemonic = True
		Me.Label2.Visible = True
		' 
		' Label1
		' 
		Me.Label1.AutoSize = False
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.Enabled = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Location = New System.Drawing.Point(8, 24)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(105, 17)
		Me.Label1.TabIndex = 2
		Me.Label1.Text = "Party Cnt"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' frmTest
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(312, 156)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdSwitchTo)
		Me.Controls.Add(Me.cmdTerminate)
		Me.Controls.Add(Me.cboDisplayMode)
		Me.Controls.Add(Me.txtPartyShortName)
		Me.Controls.Add(Me.txtPartyCnt)
		Me.Controls.Add(Me.cmtTest)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.Label1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmTest"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Test iSIRPartySummary.dll"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.listBoxComboBoxHelper1.SetItemData(Me.cboDisplayMode, New Integer(){1, 0})
		CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class