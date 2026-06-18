<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDetails
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializetxtBox()
		InitializelblBox()
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
	Private WithEvents _txtBox_1 As System.Windows.Forms.TextBox
	Private WithEvents _txtBox_0 As System.Windows.Forms.TextBox
	Private WithEvents _txtBox_2 As System.Windows.Forms.TextBox
	Public WithEvents cmbBox As System.Windows.Forms.ComboBox
	Private WithEvents _lblBox_3 As System.Windows.Forms.Label
	Private WithEvents _lblBox_0 As System.Windows.Forms.Label
	Private WithEvents _lblBox_1 As System.Windows.Forms.Label
	Private WithEvents _lblBox_2 As System.Windows.Forms.Label
	Public WithEvents fraFrame As System.Windows.Forms.GroupBox
	Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents SSTab1 As System.Windows.Forms.TabControl
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public lblBox(3) As System.Windows.Forms.Label
	Public txtBox(2) As System.Windows.Forms.TextBox
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me.fraFrame = New System.Windows.Forms.GroupBox
        Me._txtBox_1 = New System.Windows.Forms.TextBox
        Me._txtBox_0 = New System.Windows.Forms.TextBox
        Me._txtBox_2 = New System.Windows.Forms.TextBox
        Me.cmbBox = New System.Windows.Forms.ComboBox
        Me._lblBox_3 = New System.Windows.Forms.Label
        Me._lblBox_0 = New System.Windows.Forms.Label
        Me._lblBox_1 = New System.Windows.Forms.Label
        Me._lblBox_2 = New System.Windows.Forms.Label
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOk = New System.Windows.Forms.Button
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me.fraFrame.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(119, 18)
        Me.SSTab1.Location = New System.Drawing.Point(8, 8)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(365, 213)
        Me.SSTab1.TabIndex = 2
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.fraFrame)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(357, 187)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "&1 - Details"
        '
        'fraFrame
        '
        Me.fraFrame.BackColor = System.Drawing.SystemColors.Control
        Me.fraFrame.Controls.Add(Me._txtBox_1)
        Me.fraFrame.Controls.Add(Me._txtBox_0)
        Me.fraFrame.Controls.Add(Me._txtBox_2)
        Me.fraFrame.Controls.Add(Me.cmbBox)
        Me.fraFrame.Controls.Add(Me._lblBox_3)
        Me.fraFrame.Controls.Add(Me._lblBox_0)
        Me.fraFrame.Controls.Add(Me._lblBox_1)
        Me.fraFrame.Controls.Add(Me._lblBox_2)
        Me.fraFrame.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraFrame.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFrame.Location = New System.Drawing.Point(16, 12)
        Me.fraFrame.Name = "fraFrame"
        Me.fraFrame.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFrame.Size = New System.Drawing.Size(329, 161)
        Me.fraFrame.TabIndex = 3
        Me.fraFrame.TabStop = False
        Me.fraFrame.Text = "Coinsurance Recoveries Details"
        '
        '_txtBox_1
        '
        Me._txtBox_1.AcceptsReturn = True
        Me._txtBox_1.BackColor = System.Drawing.SystemColors.Window
        Me._txtBox_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtBox_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtBox_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtBox_1.Location = New System.Drawing.Point(160, 96)
        Me._txtBox_1.MaxLength = 0
        Me._txtBox_1.Name = "_txtBox_1"
        Me._txtBox_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtBox_1.Size = New System.Drawing.Size(153, 20)
        Me._txtBox_1.TabIndex = 7
        '
        '_txtBox_0
        '
        Me._txtBox_0.AcceptsReturn = True
        Me._txtBox_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtBox_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtBox_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtBox_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtBox_0.Location = New System.Drawing.Point(160, 64)
        Me._txtBox_0.MaxLength = 0
        Me._txtBox_0.Name = "_txtBox_0"
        Me._txtBox_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtBox_0.Size = New System.Drawing.Size(153, 20)
        Me._txtBox_0.TabIndex = 6
        '
        '_txtBox_2
        '
        Me._txtBox_2.AcceptsReturn = True
        Me._txtBox_2.BackColor = System.Drawing.SystemColors.Window
        Me._txtBox_2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtBox_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtBox_2.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtBox_2.Location = New System.Drawing.Point(160, 128)
        Me._txtBox_2.MaxLength = 0
        Me._txtBox_2.Name = "_txtBox_2"
        Me._txtBox_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtBox_2.Size = New System.Drawing.Size(153, 20)
        Me._txtBox_2.TabIndex = 5
        '
        'cmbBox
        '
        Me.cmbBox.BackColor = System.Drawing.SystemColors.Window
        Me.cmbBox.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbBox.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbBox.Location = New System.Drawing.Point(160, 32)
        Me.cmbBox.Name = "cmbBox"
        Me.cmbBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbBox.Size = New System.Drawing.Size(153, 21)
        Me.cmbBox.TabIndex = 4
        '
        '_lblBox_3
        '
        Me._lblBox_3.BackColor = System.Drawing.SystemColors.Control
        Me._lblBox_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblBox_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblBox_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblBox_3.Location = New System.Drawing.Point(8, 35)
        Me._lblBox_3.Name = "_lblBox_3"
        Me._lblBox_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblBox_3.Size = New System.Drawing.Size(137, 17)
        Me._lblBox_3.TabIndex = 11
        Me._lblBox_3.Text = "Party Name :"
        '
        '_lblBox_0
        '
        Me._lblBox_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblBox_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblBox_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblBox_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblBox_0.Location = New System.Drawing.Point(8, 67)
        Me._lblBox_0.Name = "_lblBox_0"
        Me._lblBox_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblBox_0.Size = New System.Drawing.Size(137, 17)
        Me._lblBox_0.TabIndex = 10
        Me._lblBox_0.Text = "Share Percentage :"
        '
        '_lblBox_1
        '
        Me._lblBox_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblBox_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblBox_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblBox_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblBox_1.Location = New System.Drawing.Point(8, 99)
        Me._lblBox_1.Name = "_lblBox_1"
        Me._lblBox_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblBox_1.Size = New System.Drawing.Size(137, 17)
        Me._lblBox_1.TabIndex = 9
        Me._lblBox_1.Text = "Current Share Value :"
        '
        '_lblBox_2
        '
        Me._lblBox_2.BackColor = System.Drawing.SystemColors.Control
        Me._lblBox_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblBox_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblBox_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblBox_2.Location = New System.Drawing.Point(8, 131)
        Me._lblBox_2.Name = "_lblBox_2"
        Me._lblBox_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblBox_2.Size = New System.Drawing.Size(137, 17)
        Me._lblBox_2.TabIndex = 8
        Me._lblBox_2.Text = "New Share Value :"
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(296, 224)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.TabStop = False
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOk
        '
        Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOk.Location = New System.Drawing.Point(216, 224)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(73, 22)
        Me.cmdOk.TabIndex = 0
        Me.cmdOk.TabStop = False
        Me.cmdOk.Text = "&OK"
        Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOk.UseVisualStyleBackColor = False
        '
        'frmDetails
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(374, 251)
        Me.Controls.Add(Me.SSTab1)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDetails"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Coinsurance Recoveries Details"
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me.fraFrame.ResumeLayout(False)
        Me.fraFrame.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
	Sub InitializetxtBox()
		Me.txtBox(1) = _txtBox_1
		Me.txtBox(0) = _txtBox_0
		Me.txtBox(2) = _txtBox_2
	End Sub
	Sub InitializelblBox()
		Me.lblBox(3) = _lblBox_3
		Me.lblBox(0) = _lblBox_0
		Me.lblBox(1) = _lblBox_1
		Me.lblBox(2) = _lblBox_2
	End Sub
#End Region 
End Class