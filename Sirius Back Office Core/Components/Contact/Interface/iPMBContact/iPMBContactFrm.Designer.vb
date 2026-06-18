<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializecmdNext()
		tabMainTabPreviousTab = tabMainTab.SelectedIndex
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
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents imgIcon As System.Windows.Forms.PictureBox
	Private WithEvents _cmdNext_0 As System.Windows.Forms.Button
	Public WithEvents pnlConReference As System.Windows.Forms.Panel
	Public WithEvents pnlConPostcode As System.Windows.Forms.Panel
	Public WithEvents lblAdPostcode As System.Windows.Forms.Label
	Public WithEvents lbAdReference As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents txtExtension As System.Windows.Forms.TextBox
	Public WithEvents txtNumber As System.Windows.Forms.TextBox
	Public WithEvents txtAreaCode As System.Windows.Forms.TextBox
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents cmbContactType As System.Windows.Forms.ComboBox
	Public WithEvents lblExtension As System.Windows.Forms.Label
	Public WithEvents lblNumber As System.Windows.Forms.Label
	Public WithEvents lblAreaCode As System.Windows.Forms.Label
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents lblContactType As System.Windows.Forms.Label
	Public WithEvents Frame2 As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public cmdNext(0) As System.Windows.Forms.Button
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.imgIcon = New System.Windows.Forms.PictureBox
        Me._cmdNext_0 = New System.Windows.Forms.Button
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.pnlConReference = New System.Windows.Forms.Panel
        Me.lpnlConReference = New System.Windows.Forms.Label
        Me.pnlConPostcode = New System.Windows.Forms.Panel
        Me.lpnlConPostcode = New System.Windows.Forms.Label
        Me.lblAdPostcode = New System.Windows.Forms.Label
        Me.lbAdReference = New System.Windows.Forms.Label
        Me.Frame2 = New System.Windows.Forms.GroupBox
        Me.txtExtension = New System.Windows.Forms.TextBox
        Me.txtNumber = New System.Windows.Forms.TextBox
        Me.txtAreaCode = New System.Windows.Forms.TextBox
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.cmbContactType = New System.Windows.Forms.ComboBox
        Me.lblExtension = New System.Windows.Forms.Label
        Me.lblNumber = New System.Windows.Forms.Label
        Me.lblAreaCode = New System.Windows.Forms.Label
        Me.lblDescription = New System.Windows.Forms.Label
        Me.lblContactType = New System.Windows.Forms.Label
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Frame1.SuspendLayout()
        Me.pnlConReference.SuspendLayout()
        Me.pnlConPostcode.SuspendLayout()
        Me.Frame2.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(536, 277)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 7
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(456, 277)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 6
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(376, 277)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 5
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(119, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(605, 268)
        Me.tabMainTab.TabIndex = 8
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.imgIcon)
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdNext_0)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Frame1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Frame2)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(597, 242)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Contact"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'imgIcon
        '
        Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon.Image = CType(resources.GetObject("imgIcon.Image"), System.Drawing.Image)
        Me.imgIcon.Location = New System.Drawing.Point(552, 20)
        Me.imgIcon.Name = "imgIcon"
        Me.imgIcon.Size = New System.Drawing.Size(32, 32)
        Me.imgIcon.TabIndex = 0
        Me.imgIcon.TabStop = False
        '
        '_cmdNext_0
        '
        Me._cmdNext_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_0.Location = New System.Drawing.Point(318, 292)
        Me._cmdNext_0.Name = "_cmdNext_0"
        Me._cmdNext_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_0.Size = New System.Drawing.Size(22, 19)
        Me._cmdNext_0.TabIndex = 9
        Me._cmdNext_0.TabStop = False
        Me._cmdNext_0.Text = ">>"
        Me._cmdNext_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_0.UseVisualStyleBackColor = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.pnlConReference)
        Me.Frame1.Controls.Add(Me.pnlConPostcode)
        Me.Frame1.Controls.Add(Me.lblAdPostcode)
        Me.Frame1.Controls.Add(Me.lbAdReference)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(16, 12)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(521, 47)
        Me.Frame1.TabIndex = 10
        Me.Frame1.TabStop = False
        '
        'pnlConReference
        '
        Me.pnlConReference.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlConReference.Controls.Add(Me.lpnlConReference)
        Me.pnlConReference.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlConReference.Location = New System.Drawing.Point(103, 18)
        Me.pnlConReference.Name = "pnlConReference"
        Me.pnlConReference.Size = New System.Drawing.Size(153, 19)
        Me.pnlConReference.TabIndex = 19
        '
        'lpnlConReference
        '
        Me.lpnlConReference.AutoSize = True
        Me.lpnlConReference.Location = New System.Drawing.Point(1, 0)
        Me.lpnlConReference.Name = "lpnlConReference"
        Me.lpnlConReference.Size = New System.Drawing.Size(0, 13)
        Me.lpnlConReference.TabIndex = 0
        '
        'pnlConPostcode
        '
        Me.pnlConPostcode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlConPostcode.Controls.Add(Me.lpnlConPostcode)
        Me.pnlConPostcode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlConPostcode.Location = New System.Drawing.Point(419, 17)
        Me.pnlConPostcode.Name = "pnlConPostcode"
        Me.pnlConPostcode.Size = New System.Drawing.Size(81, 19)
        Me.pnlConPostcode.TabIndex = 20
        '
        'lpnlConPostcode
        '
        Me.lpnlConPostcode.AutoSize = True
        Me.lpnlConPostcode.Location = New System.Drawing.Point(1, 1)
        Me.lpnlConPostcode.Name = "lpnlConPostcode"
        Me.lpnlConPostcode.Size = New System.Drawing.Size(0, 13)
        Me.lpnlConPostcode.TabIndex = 0
        '
        'lblAdPostcode
        '
        Me.lblAdPostcode.AutoSize = True
        Me.lblAdPostcode.BackColor = System.Drawing.SystemColors.Control
        Me.lblAdPostcode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAdPostcode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAdPostcode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAdPostcode.Location = New System.Drawing.Point(332, 20)
        Me.lblAdPostcode.Name = "lblAdPostcode"
        Me.lblAdPostcode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAdPostcode.Size = New System.Drawing.Size(55, 13)
        Me.lblAdPostcode.TabIndex = 13
        Me.lblAdPostcode.Text = "Postcode:"
        '
        'lbAdReference
        '
        Me.lbAdReference.BackColor = System.Drawing.SystemColors.Control
        Me.lbAdReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbAdReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbAdReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbAdReference.Location = New System.Drawing.Point(16, 20)
        Me.lbAdReference.Name = "lbAdReference"
        Me.lbAdReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbAdReference.Size = New System.Drawing.Size(77, 13)
        Me.lbAdReference.TabIndex = 12
        Me.lbAdReference.Text = "Reference:"
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.txtExtension)
        Me.Frame2.Controls.Add(Me.txtNumber)
        Me.Frame2.Controls.Add(Me.txtAreaCode)
        Me.Frame2.Controls.Add(Me.txtDescription)
        Me.Frame2.Controls.Add(Me.cmbContactType)
        Me.Frame2.Controls.Add(Me.lblExtension)
        Me.Frame2.Controls.Add(Me.lblNumber)
        Me.Frame2.Controls.Add(Me.lblAreaCode)
        Me.Frame2.Controls.Add(Me.lblDescription)
        Me.Frame2.Controls.Add(Me.lblContactType)
        Me.Frame2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(16, 76)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(569, 129)
        Me.Frame2.TabIndex = 11
        Me.Frame2.TabStop = False
        '
        'txtExtension
        '
        Me.txtExtension.AcceptsReturn = True
        Me.txtExtension.BackColor = System.Drawing.SystemColors.Window
        Me.txtExtension.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtExtension.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExtension.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtExtension.Location = New System.Drawing.Point(296, 96)
        Me.txtExtension.MaxLength = 6
        Me.txtExtension.Name = "txtExtension"
        Me.txtExtension.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtExtension.Size = New System.Drawing.Size(57, 20)
        Me.txtExtension.TabIndex = 4
        '
        'txtNumber
        '
        Me.txtNumber.AcceptsReturn = True
        Me.txtNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNumber.Location = New System.Drawing.Point(88, 96)
        Me.txtNumber.MaxLength = 255
        Me.txtNumber.Name = "txtNumber"
        Me.txtNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNumber.Size = New System.Drawing.Size(193, 20)
        Me.txtNumber.TabIndex = 3
        '
        'txtAreaCode
        '
        Me.txtAreaCode.AcceptsReturn = True
        Me.txtAreaCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtAreaCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAreaCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAreaCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAreaCode.Location = New System.Drawing.Point(16, 96)
        Me.txtAreaCode.MaxLength = 10
        Me.txtAreaCode.Name = "txtAreaCode"
        Me.txtAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAreaCode.Size = New System.Drawing.Size(57, 20)
        Me.txtAreaCode.TabIndex = 2
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(296, 32)
        Me.txtDescription.MaxLength = 0
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(257, 34)
        Me.txtDescription.TabIndex = 1
        '
        'cmbContactType
        '
        Me.cmbContactType.BackColor = System.Drawing.SystemColors.Window
        Me.cmbContactType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbContactType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbContactType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbContactType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbContactType.Location = New System.Drawing.Point(16, 32)
        Me.cmbContactType.Name = "cmbContactType"
        Me.cmbContactType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbContactType.Size = New System.Drawing.Size(217, 21)
        Me.cmbContactType.TabIndex = 0
        '
        'lblExtension
        '
        Me.lblExtension.BackColor = System.Drawing.SystemColors.Control
        Me.lblExtension.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExtension.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExtension.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExtension.Location = New System.Drawing.Point(296, 80)
        Me.lblExtension.Name = "lblExtension"
        Me.lblExtension.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExtension.Size = New System.Drawing.Size(81, 17)
        Me.lblExtension.TabIndex = 18
        Me.lblExtension.Text = "Extension:"
        '
        'lblNumber
        '
        Me.lblNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNumber.Location = New System.Drawing.Point(88, 80)
        Me.lblNumber.Name = "lblNumber"
        Me.lblNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNumber.Size = New System.Drawing.Size(81, 17)
        Me.lblNumber.TabIndex = 17
        Me.lblNumber.Text = "Number:"
        '
        'lblAreaCode
        '
        Me.lblAreaCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblAreaCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAreaCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAreaCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAreaCode.Location = New System.Drawing.Point(16, 80)
        Me.lblAreaCode.Name = "lblAreaCode"
        Me.lblAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAreaCode.Size = New System.Drawing.Size(81, 17)
        Me.lblAreaCode.TabIndex = 16
        Me.lblAreaCode.Text = "Areacode:"
        '
        'lblDescription
        '
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(296, 16)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(81, 17)
        Me.lblDescription.TabIndex = 15
        Me.lblDescription.Text = "Description:"
        '
        'lblContactType
        '
        Me.lblContactType.BackColor = System.Drawing.SystemColors.Control
        Me.lblContactType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblContactType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblContactType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblContactType.Location = New System.Drawing.Point(16, 16)
        Me.lblContactType.Name = "lblContactType"
        Me.lblContactType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblContactType.Size = New System.Drawing.Size(81, 17)
        Me.lblContactType.TabIndex = 14
        Me.lblContactType.Text = "Contact "
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(621, 305)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(204, 164)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Contact"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me.pnlConReference.ResumeLayout(False)
        Me.pnlConReference.PerformLayout()
        Me.pnlConPostcode.ResumeLayout(False)
        Me.pnlConPostcode.PerformLayout()
        Me.Frame2.ResumeLayout(False)
        Me.Frame2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Sub InitializecmdNext()
        Me.cmdNext(0) = _cmdNext_0
    End Sub
    Friend WithEvents lpnlConReference As System.Windows.Forms.Label
    Friend WithEvents lpnlConPostcode As System.Windows.Forms.Label
#End Region 
End Class