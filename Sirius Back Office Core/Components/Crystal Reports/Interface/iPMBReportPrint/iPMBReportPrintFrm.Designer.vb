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
	Public WithEvents lstReports As System.Windows.Forms.ListBox
	Public WithEvents cmdPrint As System.Windows.Forms.Button
	Public WithEvents cmdPreview As System.Windows.Forms.Button
	Public WithEvents lstDirectories As System.Windows.Forms.ListBox
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents lblCategory As System.Windows.Forms.Label
	Public WithEvents lblReports As System.Windows.Forms.Label
	Private WithEvents listBoxHelper1 As Artinsoft.VB6.Gui.ListBoxHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.lstReports = New System.Windows.Forms.ListBox
        Me.cmdPrint = New System.Windows.Forms.Button
        Me.cmdPreview = New System.Windows.Forms.Button
        Me.lstDirectories = New System.Windows.Forms.ListBox
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.lblCategory = New System.Windows.Forms.Label
        Me.lblReports = New System.Windows.Forms.Label
        Me.listBoxHelper1 = New Artinsoft.VB6.Gui.ListBoxHelper(Me.components)
        Me.SuspendLayout()
        '
        'lstReports
        '
        Me.lstReports.BackColor = System.Drawing.SystemColors.Window
        Me.lstReports.Cursor = System.Windows.Forms.Cursors.Default
        Me.lstReports.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstReports.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lstReports.Location = New System.Drawing.Point(8, 120)
        Me.lstReports.Name = "lstReports"
        Me.lstReports.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.listBoxHelper1.SetSelectionMode(Me.lstReports, System.Windows.Forms.SelectionMode.One)
        Me.lstReports.Size = New System.Drawing.Size(337, 160)
        Me.lstReports.Sorted = True
        Me.lstReports.TabIndex = 5
        '
        'cmdPrint
        '
        Me.cmdPrint.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPrint.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPrint.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPrint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPrint.Location = New System.Drawing.Point(192, 288)
        Me.cmdPrint.Name = "cmdPrint"
        Me.cmdPrint.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPrint.Size = New System.Drawing.Size(73, 22)
        Me.cmdPrint.TabIndex = 4
        Me.cmdPrint.Text = "&Print"
        Me.cmdPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPrint.UseVisualStyleBackColor = False
        '
        'cmdPreview
        '
        Me.cmdPreview.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPreview.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPreview.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPreview.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPreview.Location = New System.Drawing.Point(112, 288)
        Me.cmdPreview.Name = "cmdPreview"
        Me.cmdPreview.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPreview.Size = New System.Drawing.Size(73, 22)
        Me.cmdPreview.TabIndex = 3
        Me.cmdPreview.Text = "Pre&view"
        Me.cmdPreview.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPreview.UseVisualStyleBackColor = False
        '
        'lstDirectories
        '
        Me.lstDirectories.BackColor = System.Drawing.SystemColors.Window
        Me.lstDirectories.Cursor = System.Windows.Forms.Cursors.Default
        Me.lstDirectories.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstDirectories.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lstDirectories.Location = New System.Drawing.Point(8, 24)
        Me.lstDirectories.Name = "lstDirectories"
        Me.lstDirectories.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.listBoxHelper1.SetSelectionMode(Me.lstDirectories, System.Windows.Forms.SelectionMode.One)
        Me.lstDirectories.Size = New System.Drawing.Size(337, 69)
        Me.lstDirectories.Sorted = True
        Me.lstDirectories.TabIndex = 2
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(8, 288)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 1
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(272, 288)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 0
        Me.cmdCancel.Text = "&Close"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'lblCategory
        '
        Me.lblCategory.AutoSize = True
        Me.lblCategory.BackColor = System.Drawing.SystemColors.Control
        Me.lblCategory.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCategory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCategory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCategory.Location = New System.Drawing.Point(8, 8)
        Me.lblCategory.Name = "lblCategory"
        Me.lblCategory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCategory.Size = New System.Drawing.Size(52, 13)
        Me.lblCategory.TabIndex = 7
        Me.lblCategory.Text = "Category:"
        '
        'lblReports
        '
        Me.lblReports.AutoSize = True
        Me.lblReports.BackColor = System.Drawing.SystemColors.Control
        Me.lblReports.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReports.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReports.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReports.Location = New System.Drawing.Point(8, 104)
        Me.lblReports.Name = "lblReports"
        Me.lblReports.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReports.Size = New System.Drawing.Size(93, 13)
        Me.lblReports.TabIndex = 6
        Me.lblReports.Text = "Available Reports:"
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(354, 318)
        Me.Controls.Add(Me.lstReports)
        Me.Controls.Add(Me.cmdPrint)
        Me.Controls.Add(Me.cmdPreview)
        Me.Controls.Add(Me.lstDirectories)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.lblCategory)
        Me.Controls.Add(Me.lblReports)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Sirius Reports"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class