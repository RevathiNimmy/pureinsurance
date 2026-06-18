<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSystemOptions
#Region "Windows Form Designer generated code "
	Public Sub New()
        MyBase.New()
        Me.KeyPreview = True
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		SSTab1PreviousTab = SSTab1.SelectedIndex
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
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents txtPMSupport As System.Windows.Forms.TextBox
	Public WithEvents lblPMSupportExplanation As System.Windows.Forms.Label
	Public WithEvents lblPMSupport As System.Windows.Forms.Label
	Public WithEvents panPMSupport As System.Windows.Forms.Panel
	Public WithEvents txtHomePage As System.Windows.Forms.TextBox
	Public WithEvents lblExplanation As System.Windows.Forms.Label
	Public WithEvents lblHomePage As System.Windows.Forms.Label
	Public WithEvents panWebHome As System.Windows.Forms.Panel
	Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents txtTabCaption As System.Windows.Forms.TextBox
	Public WithEvents lblWebTabExplanation As System.Windows.Forms.Label
	Public WithEvents lblTabCaption As System.Windows.Forms.Label
	Public WithEvents panWebTab As System.Windows.Forms.Panel
	Public WithEvents txtFormCaption As System.Windows.Forms.TextBox
	Public WithEvents lblMainFormExplanation As System.Windows.Forms.Label
	Public WithEvents lblFormCaption As System.Windows.Forms.Label
	Public WithEvents panMainForm As System.Windows.Forms.Panel
	Private WithEvents _SSTab1_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents SSTab1 As System.Windows.Forms.TabControl
	Dim Private SSTab1PreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOk = New System.Windows.Forms.Button
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me.panPMSupport = New System.Windows.Forms.Panel
        Me.txtPMSupport = New System.Windows.Forms.TextBox
        Me.lblPMSupportExplanation = New System.Windows.Forms.Label
        Me.lblPMSupport = New System.Windows.Forms.Label
        Me.panWebHome = New System.Windows.Forms.Panel
        Me.txtHomePage = New System.Windows.Forms.TextBox
        Me.lblExplanation = New System.Windows.Forms.Label
        Me.lblHomePage = New System.Windows.Forms.Label
        Me._SSTab1_TabPage1 = New System.Windows.Forms.TabPage
        Me.panWebTab = New System.Windows.Forms.Panel
        Me.txtTabCaption = New System.Windows.Forms.TextBox
        Me.lblWebTabExplanation = New System.Windows.Forms.Label
        Me.lblTabCaption = New System.Windows.Forms.Label
        Me.panMainForm = New System.Windows.Forms.Panel
        Me.txtFormCaption = New System.Windows.Forms.TextBox
        Me.lblMainFormExplanation = New System.Windows.Forms.Label
        Me.lblFormCaption = New System.Windows.Forms.Label
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me.panPMSupport.SuspendLayout()
        Me.panWebHome.SuspendLayout()
        Me._SSTab1_TabPage1.SuspendLayout()
        Me.panWebTab.SuspendLayout()
        Me.panMainForm.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(240, 224)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 2
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
        Me.cmdOk.Location = New System.Drawing.Point(160, 224)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(73, 22)
        Me.cmdOk.TabIndex = 1
        Me.cmdOk.Text = "&Ok"
        Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOk.UseVisualStyleBackColor = False
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage1)
        Me.SSTab1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(100, 18)
        Me.SSTab1.Location = New System.Drawing.Point(8, 8)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(309, 213)
        Me.SSTab1.TabIndex = 0
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.panPMSupport)
        Me._SSTab1_TabPage0.Controls.Add(Me.panWebHome)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(301, 187)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "Web Pages"
        '
        'panPMSupport
        '
        Me.panPMSupport.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panPMSupport.Controls.Add(Me.txtPMSupport)
        Me.panPMSupport.Controls.Add(Me.lblPMSupportExplanation)
        Me.panPMSupport.Controls.Add(Me.lblPMSupport)
        Me.panPMSupport.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panPMSupport.Location = New System.Drawing.Point(8, 100)
        Me.panPMSupport.Name = "panPMSupport"
        Me.panPMSupport.Size = New System.Drawing.Size(289, 81)
        Me.panPMSupport.TabIndex = 7
        '
        'txtPMSupport
        '
        Me.txtPMSupport.AcceptsReturn = True
        Me.txtPMSupport.BackColor = System.Drawing.SystemColors.Window
        Me.txtPMSupport.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPMSupport.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPMSupport.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPMSupport.Location = New System.Drawing.Point(80, 48)
        Me.txtPMSupport.MaxLength = 0
        Me.txtPMSupport.Name = "txtPMSupport"
        Me.txtPMSupport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPMSupport.Size = New System.Drawing.Size(201, 20)
        Me.txtPMSupport.TabIndex = 10
        '
        'lblPMSupportExplanation
        '
        Me.lblPMSupportExplanation.BackColor = System.Drawing.SystemColors.Control
        Me.lblPMSupportExplanation.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPMSupportExplanation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPMSupportExplanation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPMSupportExplanation.Location = New System.Drawing.Point(8, 8)
        Me.lblPMSupportExplanation.Name = "lblPMSupportExplanation"
        Me.lblPMSupportExplanation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPMSupportExplanation.Size = New System.Drawing.Size(265, 33)
        Me.lblPMSupportExplanation.TabIndex = 8
        Me.lblPMSupportExplanation.Text = "Enter the SSP Pure Web Page."
        '
        'lblPMSupport
        '
        Me.lblPMSupport.BackColor = System.Drawing.SystemColors.Control
        Me.lblPMSupport.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPMSupport.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPMSupport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPMSupport.Location = New System.Drawing.Point(8, 48)
        Me.lblPMSupport.Name = "lblPMSupport"
        Me.lblPMSupport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPMSupport.Size = New System.Drawing.Size(80, 20)
        Me.lblPMSupport.TabIndex = 9
        Me.lblPMSupport.Text = "Support Page:"
        '
        'panWebHome
        '
        Me.panWebHome.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panWebHome.Controls.Add(Me.txtHomePage)
        Me.panWebHome.Controls.Add(Me.lblExplanation)
        Me.panWebHome.Controls.Add(Me.lblHomePage)
        Me.panWebHome.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panWebHome.Location = New System.Drawing.Point(8, 12)
        Me.panWebHome.Name = "panWebHome"
        Me.panWebHome.Size = New System.Drawing.Size(289, 81)
        Me.panWebHome.TabIndex = 3
        '
        'txtHomePage
        '
        Me.txtHomePage.AcceptsReturn = True
        Me.txtHomePage.BackColor = System.Drawing.SystemColors.Window
        Me.txtHomePage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHomePage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHomePage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtHomePage.Location = New System.Drawing.Point(80, 48)
        Me.txtHomePage.MaxLength = 0
        Me.txtHomePage.Name = "txtHomePage"
        Me.txtHomePage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHomePage.Size = New System.Drawing.Size(201, 20)
        Me.txtHomePage.TabIndex = 6
        '
        'lblExplanation
        '
        Me.lblExplanation.BackColor = System.Drawing.SystemColors.Control
        Me.lblExplanation.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExplanation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExplanation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExplanation.Location = New System.Drawing.Point(8, 8)
        Me.lblExplanation.Name = "lblExplanation"
        Me.lblExplanation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExplanation.Size = New System.Drawing.Size(265, 33)
        Me.lblExplanation.TabIndex = 4
        Me.lblExplanation.Text = "Enter the Web Home Page. Clear the text box to disable the Web Tab for ALL users." & _
            ""
        '
        'lblHomePage
        '
        Me.lblHomePage.BackColor = System.Drawing.SystemColors.Control
        Me.lblHomePage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblHomePage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHomePage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblHomePage.Location = New System.Drawing.Point(8, 48)
        Me.lblHomePage.Name = "lblHomePage"
        Me.lblHomePage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblHomePage.Size = New System.Drawing.Size(73, 17)
        Me.lblHomePage.TabIndex = 5
        Me.lblHomePage.Text = "Home Page:"
        '
        '_SSTab1_TabPage1
        '
        Me._SSTab1_TabPage1.Controls.Add(Me.panWebTab)
        Me._SSTab1_TabPage1.Controls.Add(Me.panMainForm)
        Me._SSTab1_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage1.Name = "_SSTab1_TabPage1"
        Me._SSTab1_TabPage1.Size = New System.Drawing.Size(301, 187)
        Me._SSTab1_TabPage1.TabIndex = 1
        Me._SSTab1_TabPage1.Text = "Captions"
        '
        'panWebTab
        '
        Me.panWebTab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panWebTab.Controls.Add(Me.txtTabCaption)
        Me.panWebTab.Controls.Add(Me.lblWebTabExplanation)
        Me.panWebTab.Controls.Add(Me.lblTabCaption)
        Me.panWebTab.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panWebTab.Location = New System.Drawing.Point(8, 100)
        Me.panWebTab.Name = "panWebTab"
        Me.panWebTab.Size = New System.Drawing.Size(289, 81)
        Me.panWebTab.TabIndex = 15
        '
        'txtTabCaption
        '
        Me.txtTabCaption.AcceptsReturn = True
        Me.txtTabCaption.BackColor = System.Drawing.SystemColors.Window
        Me.txtTabCaption.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTabCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTabCaption.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTabCaption.Location = New System.Drawing.Point(80, 48)
        Me.txtTabCaption.MaxLength = 0
        Me.txtTabCaption.Name = "txtTabCaption"
        Me.txtTabCaption.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTabCaption.Size = New System.Drawing.Size(201, 20)
        Me.txtTabCaption.TabIndex = 18
        '
        'lblWebTabExplanation
        '
        Me.lblWebTabExplanation.BackColor = System.Drawing.SystemColors.Control
        Me.lblWebTabExplanation.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblWebTabExplanation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWebTabExplanation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblWebTabExplanation.Location = New System.Drawing.Point(8, 8)
        Me.lblWebTabExplanation.Name = "lblWebTabExplanation"
        Me.lblWebTabExplanation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblWebTabExplanation.Size = New System.Drawing.Size(265, 33)
        Me.lblWebTabExplanation.TabIndex = 16
        Me.lblWebTabExplanation.Text = "Enter the caption to be displayed on the Web Tab on Work Manager. The default is " & _
            "&&News."
        '
        'lblTabCaption
        '
        Me.lblTabCaption.BackColor = System.Drawing.SystemColors.Control
        Me.lblTabCaption.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTabCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTabCaption.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTabCaption.Location = New System.Drawing.Point(8, 48)
        Me.lblTabCaption.Name = "lblTabCaption"
        Me.lblTabCaption.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTabCaption.Size = New System.Drawing.Size(75, 17)
        Me.lblTabCaption.TabIndex = 17
        Me.lblTabCaption.Text = "Tab Caption:"
        '
        'panMainForm
        '
        Me.panMainForm.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panMainForm.Controls.Add(Me.txtFormCaption)
        Me.panMainForm.Controls.Add(Me.lblMainFormExplanation)
        Me.panMainForm.Controls.Add(Me.lblFormCaption)
        Me.panMainForm.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panMainForm.Location = New System.Drawing.Point(8, 12)
        Me.panMainForm.Name = "panMainForm"
        Me.panMainForm.Size = New System.Drawing.Size(289, 81)
        Me.panMainForm.TabIndex = 11
        '
        'txtFormCaption
        '
        Me.txtFormCaption.AcceptsReturn = True
        Me.txtFormCaption.BackColor = System.Drawing.SystemColors.Window
        Me.txtFormCaption.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFormCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFormCaption.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFormCaption.Location = New System.Drawing.Point(80, 48)
        Me.txtFormCaption.MaxLength = 0
        Me.txtFormCaption.Name = "txtFormCaption"
        Me.txtFormCaption.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFormCaption.Size = New System.Drawing.Size(201, 20)
        Me.txtFormCaption.TabIndex = 14
        '
        'lblMainFormExplanation
        '
        Me.lblMainFormExplanation.BackColor = System.Drawing.SystemColors.Control
        Me.lblMainFormExplanation.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMainFormExplanation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMainFormExplanation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMainFormExplanation.Location = New System.Drawing.Point(8, 8)
        Me.lblMainFormExplanation.Name = "lblMainFormExplanation"
        Me.lblMainFormExplanation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMainFormExplanation.Size = New System.Drawing.Size(265, 33)
        Me.lblMainFormExplanation.TabIndex = 12
        Me.lblMainFormExplanation.Text = "Enter the caption to be displayed on the Main Form. The default is SSP Pure: Work" & _
            " Manager."
        '
        'lblFormCaption
        '
        Me.lblFormCaption.BackColor = System.Drawing.SystemColors.Control
        Me.lblFormCaption.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFormCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFormCaption.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFormCaption.Location = New System.Drawing.Point(8, 48)
        Me.lblFormCaption.Name = "lblFormCaption"
        Me.lblFormCaption.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFormCaption.Size = New System.Drawing.Size(75, 17)
        Me.lblFormCaption.TabIndex = 13
        Me.lblFormCaption.Text = "Form Caption:"
        '
        'frmSystemOptions
        '
        Me.AcceptButton = Me.cmdOk
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(319, 255)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.SSTab1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSystemOptions"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.Text = "System Administrator Options"
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me.panPMSupport.ResumeLayout(False)
        Me.panPMSupport.PerformLayout()
        Me.panWebHome.ResumeLayout(False)
        Me.panWebHome.PerformLayout()
        Me._SSTab1_TabPage1.ResumeLayout(False)
        Me.panWebTab.ResumeLayout(False)
        Me.panWebTab.PerformLayout()
        Me.panMainForm.ResumeLayout(False)
        Me.panMainForm.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class