<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDetails
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
	Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
		Dispose(True)
	End Sub
    'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents lblSuppressed As System.Windows.Forms.Label
	Public WithEvents lblAvailableDocs As System.Windows.Forms.Label
	Public WithEvents cmdRemoveDocs As System.Windows.Forms.Button
	Public WithEvents cmdAddDocs As System.Windows.Forms.Button
	Public WithEvents lstDocsToChoose As System.Windows.Forms.ListBox
	Public WithEvents lstDocsChosen As System.Windows.Forms.ListBox
	Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMain As System.Windows.Forms.TabControl
	Private WithEvents listBoxHelper1 As Artinsoft.VB6.Gui.ListBoxHelper
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDetails))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.tabMain = New System.Windows.Forms.TabControl
        Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblSuppressed = New System.Windows.Forms.Label
        Me.lblAvailableDocs = New System.Windows.Forms.Label
        Me.cmdRemoveDocs = New System.Windows.Forms.Button
        Me.cmdAddDocs = New System.Windows.Forms.Button
        Me.lstDocsToChoose = New System.Windows.Forms.ListBox
        Me.lstDocsChosen = New System.Windows.Forms.ListBox
        Me.listBoxHelper1 = New Artinsoft.VB6.Gui.ListBoxHelper(Me.components)
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMain.SuspendLayout()
        Me._tabMain_TabPage0.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(384, 296)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(464, 296)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me._tabMain_TabPage0)
        Me.tabMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMain.ItemSize = New System.Drawing.Size(170, 18)
        Me.tabMain.Location = New System.Drawing.Point(8, 8)
        Me.tabMain.Multiline = True
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(517, 285)
        Me.tabMain.TabIndex = 2
        '
        '_tabMain_TabPage0
        '
        Me._tabMain_TabPage0.Controls.Add(Me.lblSuppressed)
        Me._tabMain_TabPage0.Controls.Add(Me.lblAvailableDocs)
        Me._tabMain_TabPage0.Controls.Add(Me.cmdRemoveDocs)
        Me._tabMain_TabPage0.Controls.Add(Me.cmdAddDocs)
        Me._tabMain_TabPage0.Controls.Add(Me.lstDocsToChoose)
        Me._tabMain_TabPage0.Controls.Add(Me.lstDocsChosen)
        Me._tabMain_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage0.Name = "_tabMain_TabPage0"
        Me._tabMain_TabPage0.Size = New System.Drawing.Size(509, 259)
        Me._tabMain_TabPage0.TabIndex = 0
        Me._tabMain_TabPage0.Text = "1 - Details"
        '
        'lblSuppressed
        '
        Me.lblSuppressed.AutoSize = True
        Me.lblSuppressed.BackColor = System.Drawing.SystemColors.Control
        Me.lblSuppressed.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSuppressed.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSuppressed.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSuppressed.Location = New System.Drawing.Point(288, 12)
        Me.lblSuppressed.Name = "lblSuppressed"
        Me.lblSuppressed.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSuppressed.Size = New System.Drawing.Size(123, 13)
        Me.lblSuppressed.TabIndex = 7
        Me.lblSuppressed.Text = "Suppressed Documents:"
        '
        'lblAvailableDocs
        '
        Me.lblAvailableDocs.AutoSize = True
        Me.lblAvailableDocs.BackColor = System.Drawing.SystemColors.Control
        Me.lblAvailableDocs.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAvailableDocs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAvailableDocs.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAvailableDocs.Location = New System.Drawing.Point(8, 12)
        Me.lblAvailableDocs.Name = "lblAvailableDocs"
        Me.lblAvailableDocs.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAvailableDocs.Size = New System.Drawing.Size(110, 13)
        Me.lblAvailableDocs.TabIndex = 8
        Me.lblAvailableDocs.Text = "Available Documents:"
        '
        'cmdRemoveDocs
        '
        Me.cmdRemoveDocs.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRemoveDocs.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRemoveDocs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRemoveDocs.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRemoveDocs.Location = New System.Drawing.Point(232, 68)
        Me.cmdRemoveDocs.Name = "cmdRemoveDocs"
        Me.cmdRemoveDocs.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRemoveDocs.Size = New System.Drawing.Size(41, 41)
        Me.cmdRemoveDocs.TabIndex = 3
        Me.cmdRemoveDocs.Text = ">"
        Me.cmdRemoveDocs.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRemoveDocs.UseVisualStyleBackColor = False
        '
        'cmdAddDocs
        '
        Me.cmdAddDocs.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddDocs.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddDocs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddDocs.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddDocs.Location = New System.Drawing.Point(232, 124)
        Me.cmdAddDocs.Name = "cmdAddDocs"
        Me.cmdAddDocs.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddDocs.Size = New System.Drawing.Size(41, 41)
        Me.cmdAddDocs.TabIndex = 4
        Me.cmdAddDocs.Text = "<"
        Me.cmdAddDocs.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddDocs.UseVisualStyleBackColor = False
        '
        'lstDocsToChoose
        '
        Me.lstDocsToChoose.BackColor = System.Drawing.SystemColors.Window
        Me.lstDocsToChoose.Cursor = System.Windows.Forms.Cursors.Default
        Me.lstDocsToChoose.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.lstDocsToChoose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstDocsToChoose.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lstDocsToChoose.Location = New System.Drawing.Point(288, 28)
        Me.lstDocsToChoose.Name = "lstDocsToChoose"
        Me.lstDocsToChoose.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.listBoxHelper1.SetSelectionMode(Me.lstDocsToChoose, System.Windows.Forms.SelectionMode.MultiExtended)
        Me.lstDocsToChoose.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstDocsToChoose.Size = New System.Drawing.Size(209, 186)
        Me.lstDocsToChoose.TabIndex = 5
        '
        'lstDocsChosen
        '
        Me.lstDocsChosen.BackColor = System.Drawing.SystemColors.Window
        Me.lstDocsChosen.Cursor = System.Windows.Forms.Cursors.Default
        Me.lstDocsChosen.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.lstDocsChosen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstDocsChosen.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lstDocsChosen.Location = New System.Drawing.Point(8, 28)
        Me.lstDocsChosen.Name = "lstDocsChosen"
        Me.lstDocsChosen.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.listBoxHelper1.SetSelectionMode(Me.lstDocsChosen, System.Windows.Forms.SelectionMode.MultiExtended)
        Me.lstDocsChosen.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstDocsChosen.Size = New System.Drawing.Size(209, 199)
        Me.lstDocsChosen.TabIndex = 6
        '
        'frmDetails
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(545, 328)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.tabMain)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDetails"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Relationship"
        Me.tabMain.ResumeLayout(False)
        Me._tabMain_TabPage0.ResumeLayout(False)
        Me._tabMain_TabPage0.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class