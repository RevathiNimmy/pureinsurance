<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMapFolder
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
	Public WithEvents lblFullPath As System.Windows.Forms.Label
	Public WithEvents lblPath As System.Windows.Forms.Label
	Public WithEvents lblName As System.Windows.Forms.Label
	Public WithEvents lblReportCode As System.Windows.Forms.Label
	Public WithEvents lblTotallingIs As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents txtMapName As System.Windows.Forms.TextBox
	Public WithEvents txtreportMapId As System.Windows.Forms.TextBox
	Public WithEvents cboTotallingType As System.Windows.Forms.ComboBox
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMain As System.Windows.Forms.TabControl
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tabMain = New System.Windows.Forms.TabControl
        Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblFullPath = New System.Windows.Forms.Label
        Me.lblPath = New System.Windows.Forms.Label
        Me.lblName = New System.Windows.Forms.Label
        Me.lblReportCode = New System.Windows.Forms.Label
        Me.lblTotallingIs = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtMapName = New System.Windows.Forms.TextBox
        Me.txtreportMapId = New System.Windows.Forms.TextBox
        Me.cboTotallingType = New System.Windows.Forms.ComboBox
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMain.SuspendLayout()
        Me._tabMain_TabPage0.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me._tabMain_TabPage0)
        Me.tabMain.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMain.ItemSize = New System.Drawing.Size(326, 18)
        Me.tabMain.Location = New System.Drawing.Point(4, 4)
        Me.tabMain.Multiline = True
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(331, 203)
        Me.tabMain.TabIndex = 3
        '
        '_tabMain_TabPage0
        '
        Me._tabMain_TabPage0.Controls.Add(Me.lblFullPath)
        Me._tabMain_TabPage0.Controls.Add(Me.lblPath)
        Me._tabMain_TabPage0.Controls.Add(Me.lblName)
        Me._tabMain_TabPage0.Controls.Add(Me.lblReportCode)
        Me._tabMain_TabPage0.Controls.Add(Me.lblTotallingIs)
        Me._tabMain_TabPage0.Controls.Add(Me.Label1)
        Me._tabMain_TabPage0.Controls.Add(Me.txtMapName)
        Me._tabMain_TabPage0.Controls.Add(Me.txtreportMapId)
        Me._tabMain_TabPage0.Controls.Add(Me.cboTotallingType)
        Me._tabMain_TabPage0.Controls.Add(Me.txtDescription)
        Me._tabMain_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage0.Name = "_tabMain_TabPage0"
        Me._tabMain_TabPage0.Size = New System.Drawing.Size(323, 177)
        Me._tabMain_TabPage0.TabIndex = 0
        Me._tabMain_TabPage0.Text = "Details"
        Me._tabMain_TabPage0.UseVisualStyleBackColor = True
        '
        'lblFullPath
        '
        Me.lblFullPath.BackColor = System.Drawing.SystemColors.Control
        Me.lblFullPath.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblFullPath.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFullPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFullPath.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFullPath.Location = New System.Drawing.Point(100, 41)
        Me.lblFullPath.Name = "lblFullPath"
        Me.lblFullPath.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFullPath.Size = New System.Drawing.Size(215, 45)
        Me.lblFullPath.TabIndex = 4
        '
        'lblPath
        '
        Me.lblPath.AutoSize = True
        Me.lblPath.BackColor = System.Drawing.SystemColors.Control
        Me.lblPath.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPath.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPath.Location = New System.Drawing.Point(12, 45)
        Me.lblPath.Name = "lblPath"
        Me.lblPath.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPath.Size = New System.Drawing.Size(32, 13)
        Me.lblPath.TabIndex = 5
        Me.lblPath.Text = "Path:"
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.BackColor = System.Drawing.SystemColors.Control
        Me.lblName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblName.Location = New System.Drawing.Point(12, 18)
        Me.lblName.Name = "lblName"
        Me.lblName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblName.Size = New System.Drawing.Size(38, 13)
        Me.lblName.TabIndex = 6
        Me.lblName.Text = "&Name:"
        '
        'lblReportCode
        '
        Me.lblReportCode.AutoSize = True
        Me.lblReportCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblReportCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReportCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReportCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReportCode.Location = New System.Drawing.Point(12, 151)
        Me.lblReportCode.Name = "lblReportCode"
        Me.lblReportCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReportCode.Size = New System.Drawing.Size(64, 13)
        Me.lblReportCode.TabIndex = 7
        Me.lblReportCode.Text = "Report Seq:"
        '
        'lblTotallingIs
        '
        Me.lblTotallingIs.AutoSize = True
        Me.lblTotallingIs.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotallingIs.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotallingIs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotallingIs.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotallingIs.Location = New System.Drawing.Point(12, 96)
        Me.lblTotallingIs.Name = "lblTotallingIs"
        Me.lblTotallingIs.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotallingIs.Size = New System.Drawing.Size(77, 13)
        Me.lblTotallingIs.TabIndex = 9
        Me.lblTotallingIs.Text = "Account Type:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(12, 124)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(72, 13)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Total Header:"
        '
        'txtMapName
        '
        Me.txtMapName.AcceptsReturn = True
        Me.txtMapName.BackColor = System.Drawing.SystemColors.Window
        Me.txtMapName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMapName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMapName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMapName.Location = New System.Drawing.Point(100, 14)
        Me.txtMapName.MaxLength = 60
        Me.txtMapName.Name = "txtMapName"
        Me.txtMapName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMapName.Size = New System.Drawing.Size(215, 20)
        Me.txtMapName.TabIndex = 0
        '
        'txtreportMapId
        '
        Me.txtreportMapId.AcceptsReturn = True
        Me.txtreportMapId.BackColor = System.Drawing.SystemColors.Window
        Me.txtreportMapId.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtreportMapId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtreportMapId.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtreportMapId.Location = New System.Drawing.Point(100, 148)
        Me.txtreportMapId.MaxLength = 0
        Me.txtreportMapId.Name = "txtreportMapId"
        Me.txtreportMapId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtreportMapId.Size = New System.Drawing.Size(111, 20)
        Me.txtreportMapId.TabIndex = 8
        Me.txtreportMapId.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cboTotallingType
        '
        Me.cboTotallingType.BackColor = System.Drawing.SystemColors.Window
        Me.cboTotallingType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTotallingType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTotallingType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTotallingType.Location = New System.Drawing.Point(100, 92)
        Me.cboTotallingType.Name = "cboTotallingType"
        Me.cboTotallingType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTotallingType.Size = New System.Drawing.Size(215, 21)
        Me.cboTotallingType.TabIndex = 10
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(100, 120)
        Me.txtDescription.MaxLength = 0
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(215, 20)
        Me.txtDescription.TabIndex = 12
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(257, 208)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(74, 23)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(177, 208)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(74, 23)
        Me.cmdOK.TabIndex = 1
        Me.cmdOK.Text = "OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'frmMapFolder
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(337, 237)
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMapFolder"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Map Folder"
        Me.tabMain.ResumeLayout(False)
        Me._tabMain_TabPage0.ResumeLayout(False)
        Me._tabMain_TabPage0.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class