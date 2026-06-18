<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmImport
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
	Public WithEvents lblFile As System.Windows.Forms.Label
	Public WithEvents lblListType As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents txtfile As System.Windows.Forms.TextBox
	Public WithEvents cboListType As System.Windows.Forms.ComboBox
	Public WithEvents cmdBrowse As System.Windows.Forms.Button
	Public WithEvents cboListVersion As System.Windows.Forms.ComboBox
	Public WithEvents txtEffectiveDate As System.Windows.Forms.TextBox
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents SSTab1 As System.Windows.Forms.TabControl
	Public WithEvents cmdView As System.Windows.Forms.Button
	Public WithEvents cmdImport As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public CommonDialog1Open As System.Windows.Forms.OpenFileDialog
	Public WithEvents ProgressBar As System.Windows.Forms.ProgressBar
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmImport))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me.optUpdateList = New System.Windows.Forms.RadioButton
        Me.optNewList = New System.Windows.Forms.RadioButton
        Me.lblFile = New System.Windows.Forms.Label
        Me.lblListType = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.lblDescription = New System.Windows.Forms.Label
        Me.txtfile = New System.Windows.Forms.TextBox
        Me.cboListType = New System.Windows.Forms.ComboBox
        Me.cmdBrowse = New System.Windows.Forms.Button
        Me.cboListVersion = New System.Windows.Forms.ComboBox
        Me.txtEffectiveDate = New System.Windows.Forms.TextBox
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.cmdView = New System.Windows.Forms.Button
        Me.cmdImport = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.CommonDialog1Open = New System.Windows.Forms.OpenFileDialog
        Me.ProgressBar = New System.Windows.Forms.ProgressBar
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me.SuspendLayout()
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(504, 18)
        Me.SSTab1.Location = New System.Drawing.Point(8, 8)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(509, 213)
        Me.SSTab1.TabIndex = 3
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.optUpdateList)
        Me._SSTab1_TabPage0.Controls.Add(Me.optNewList)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblFile)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblListType)
        Me._SSTab1_TabPage0.Controls.Add(Me.Label3)
        Me._SSTab1_TabPage0.Controls.Add(Me.Label4)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblDescription)
        Me._SSTab1_TabPage0.Controls.Add(Me.txtfile)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboListType)
        Me._SSTab1_TabPage0.Controls.Add(Me.cmdBrowse)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboListVersion)
        Me._SSTab1_TabPage0.Controls.Add(Me.txtEffectiveDate)
        Me._SSTab1_TabPage0.Controls.Add(Me.txtDescription)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(501, 187)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "Import File"
        '
        'optUpdateList
        '
        Me.optUpdateList.AutoSize = True
        Me.optUpdateList.Location = New System.Drawing.Point(280, 167)
        Me.optUpdateList.Name = "optUpdateList"
        Me.optUpdateList.Size = New System.Drawing.Size(88, 17)
        Me.optUpdateList.TabIndex = 16
        Me.optUpdateList.TabStop = True
        Me.optUpdateList.Text = "Update List"
        Me.optUpdateList.UseVisualStyleBackColor = True
        '
        'optNewList
        '
        Me.optNewList.AutoSize = True
        Me.optNewList.Checked = True
        Me.optNewList.Location = New System.Drawing.Point(136, 167)
        Me.optNewList.Name = "optNewList"
        Me.optNewList.Size = New System.Drawing.Size(72, 17)
        Me.optNewList.TabIndex = 15
        Me.optNewList.TabStop = True
        Me.optNewList.Text = "New List"
        Me.optNewList.UseVisualStyleBackColor = True
        '
        'lblFile
        '
        Me.lblFile.AutoSize = True
        Me.lblFile.BackColor = System.Drawing.SystemColors.Control
        Me.lblFile.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFile.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFile.Location = New System.Drawing.Point(24, 135)
        Me.lblFile.Name = "lblFile"
        Me.lblFile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFile.Size = New System.Drawing.Size(86, 13)
        Me.lblFile.TabIndex = 4
        Me.lblFile.Text = "Import File Name"
        '
        'lblListType
        '
        Me.lblListType.AutoSize = True
        Me.lblListType.BackColor = System.Drawing.SystemColors.Control
        Me.lblListType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblListType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblListType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblListType.Location = New System.Drawing.Point(24, 32)
        Me.lblListType.Name = "lblListType"
        Me.lblListType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblListType.Size = New System.Drawing.Size(50, 13)
        Me.lblListType.TabIndex = 5
        Me.lblListType.Text = "List Type"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(24, 80)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(61, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "List Version"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(24, 103)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(75, 13)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Effective Date"
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(24, 55)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(60, 13)
        Me.lblDescription.TabIndex = 14
        Me.lblDescription.Text = "Description"
        '
        'txtfile
        '
        Me.txtfile.AcceptsReturn = True
        Me.txtfile.BackColor = System.Drawing.SystemColors.Window
        Me.txtfile.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtfile.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtfile.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtfile.Location = New System.Drawing.Point(136, 132)
        Me.txtfile.MaxLength = 0
        Me.txtfile.Name = "txtfile"
        Me.txtfile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtfile.Size = New System.Drawing.Size(313, 20)
        Me.txtfile.TabIndex = 8
        '
        'cboListType
        '
        Me.cboListType.BackColor = System.Drawing.SystemColors.Window
        Me.cboListType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboListType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboListType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboListType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboListType.Location = New System.Drawing.Point(136, 28)
        Me.cboListType.Name = "cboListType"
        Me.cboListType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboListType.Size = New System.Drawing.Size(153, 21)
        Me.cboListType.TabIndex = 9
        '
        'cmdBrowse
        '
        Me.cmdBrowse.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBrowse.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBrowse.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBrowse.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBrowse.Location = New System.Drawing.Point(456, 132)
        Me.cmdBrowse.Name = "cmdBrowse"
        Me.cmdBrowse.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBrowse.Size = New System.Drawing.Size(25, 22)
        Me.cmdBrowse.TabIndex = 10
        Me.cmdBrowse.Text = "..."
        Me.cmdBrowse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdBrowse.UseVisualStyleBackColor = False
        '
        'cboListVersion
        '
        Me.cboListVersion.BackColor = System.Drawing.SystemColors.Window
        Me.cboListVersion.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboListVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboListVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboListVersion.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboListVersion.Location = New System.Drawing.Point(136, 76)
        Me.cboListVersion.Name = "cboListVersion"
        Me.cboListVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboListVersion.Size = New System.Drawing.Size(153, 21)
        Me.cboListVersion.TabIndex = 11
        '
        'txtEffectiveDate
        '
        Me.txtEffectiveDate.AcceptsReturn = True
        Me.txtEffectiveDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtEffectiveDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEffectiveDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEffectiveDate.Location = New System.Drawing.Point(136, 100)
        Me.txtEffectiveDate.MaxLength = 0
        Me.txtEffectiveDate.Name = "txtEffectiveDate"
        Me.txtEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEffectiveDate.Size = New System.Drawing.Size(153, 20)
        Me.txtEffectiveDate.TabIndex = 12
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(136, 52)
        Me.txtDescription.MaxLength = 0
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(313, 20)
        Me.txtDescription.TabIndex = 13
        '
        'cmdView
        '
        Me.cmdView.BackColor = System.Drawing.SystemColors.Control
        Me.cmdView.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdView.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdView.Location = New System.Drawing.Point(352, 232)
        Me.cmdView.Name = "cmdView"
        Me.cmdView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdView.Size = New System.Drawing.Size(73, 22)
        Me.cmdView.TabIndex = 2
        Me.cmdView.Text = "View"
        Me.cmdView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdView.UseVisualStyleBackColor = False
        '
        'cmdImport
        '
        Me.cmdImport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdImport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdImport.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdImport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdImport.Location = New System.Drawing.Point(264, 232)
        Me.cmdImport.Name = "cmdImport"
        Me.cmdImport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdImport.Size = New System.Drawing.Size(73, 22)
        Me.cmdImport.TabIndex = 1
        Me.cmdImport.Text = "Import"
        Me.cmdImport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdImport.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(440, 232)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 0
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'ProgressBar
        '
        Me.ProgressBar.Location = New System.Drawing.Point(8, 233)
        Me.ProgressBar.Name = "ProgressBar"
        Me.ProgressBar.Size = New System.Drawing.Size(252, 19)
        Me.ProgressBar.TabIndex = 15
        Me.ProgressBar.Visible = False
        '
        'FrmImport
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(518, 263)
        Me.Controls.Add(Me.SSTab1)
        Me.Controls.Add(Me.cmdView)
        Me.Controls.Add(Me.cmdImport)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.ProgressBar)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmImport"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Import List"
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me._SSTab1_TabPage0.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents optUpdateList As System.Windows.Forms.RadioButton
    Friend WithEvents optNewList As System.Windows.Forms.RadioButton
#End Region 
End Class
