<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwSchemes_InitializeColumnKeys()
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
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdSchemeDelete As System.Windows.Forms.Button
	Private WithEvents _lvwSchemes_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSchemes_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSchemes_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSchemes_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSchemes_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSchemes_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwSchemes As System.Windows.Forms.ListView
	Public WithEvents cmdSchemeEdit As System.Windows.Forms.Button
	Public WithEvents cmdSchemeAdd As System.Windows.Forms.Button
	Public WithEvents fraSchemes As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents ImageList1 As System.Windows.Forms.ImageList
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    Private tabMainTabPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.cmdNavigate = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.fraSchemes = New System.Windows.Forms.GroupBox
        Me.cmdSchemeDelete = New System.Windows.Forms.Button
        Me.lvwSchemes = New System.Windows.Forms.ListView
        Me._lvwSchemes_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwSchemes_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwSchemes_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwSchemes_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwSchemes_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwSchemes_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdSchemeEdit = New System.Windows.Forms.Button
        Me.cmdSchemeAdd = New System.Windows.Forms.Button
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.fraSchemes.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(8, 440)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 0
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(632, 440)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 3
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
        Me.cmdCancel.Location = New System.Drawing.Point(552, 440)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 2
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
        Me.cmdOK.Location = New System.Drawing.Point(472, 440)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 1
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(138, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(6, 2)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(701, 437)
        Me.tabMainTab.TabIndex = 4
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraSchemes)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(693, 411)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Policy Numbering Schemes"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'fraSchemes
        '
        Me.fraSchemes.BackColor = System.Drawing.SystemColors.Control
        Me.fraSchemes.Controls.Add(Me.cmdSchemeDelete)
        Me.fraSchemes.Controls.Add(Me.lvwSchemes)
        Me.fraSchemes.Controls.Add(Me.cmdSchemeEdit)
        Me.fraSchemes.Controls.Add(Me.cmdSchemeAdd)
        Me.fraSchemes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraSchemes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraSchemes.Location = New System.Drawing.Point(8, 12)
        Me.fraSchemes.Name = "fraSchemes"
        Me.fraSchemes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraSchemes.Size = New System.Drawing.Size(681, 393)
        Me.fraSchemes.TabIndex = 5
        Me.fraSchemes.TabStop = False
        '
        'cmdSchemeDelete
        '
        Me.cmdSchemeDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSchemeDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSchemeDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSchemeDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSchemeDelete.Location = New System.Drawing.Point(92, 360)
        Me.cmdSchemeDelete.Name = "cmdSchemeDelete"
        Me.cmdSchemeDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSchemeDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdSchemeDelete.TabIndex = 9
        Me.cmdSchemeDelete.Text = "&Delete"
        Me.cmdSchemeDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSchemeDelete.UseVisualStyleBackColor = False
        '
        'lvwSchemes
        '
        Me.lvwSchemes.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSchemes.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwSchemes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSchemes_ColumnHeader_1, Me._lvwSchemes_ColumnHeader_2, Me._lvwSchemes_ColumnHeader_3, Me._lvwSchemes_ColumnHeader_4, Me._lvwSchemes_ColumnHeader_5, Me._lvwSchemes_ColumnHeader_6})
        Me.lvwSchemes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSchemes.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSchemes.FullRowSelect = True
        Me.lvwSchemes.HideSelection = False
        Me.lvwSchemes.LargeImageList = Me.ImageList1
        Me.lvwSchemes.Location = New System.Drawing.Point(8, 16)
        Me.lvwSchemes.MultiSelect = False
        Me.lvwSchemes.Name = "lvwSchemes"
        Me.lvwSchemes.Size = New System.Drawing.Size(665, 337)
        Me.lvwSchemes.SmallImageList = Me.ImageList1
        Me.lvwSchemes.TabIndex = 6
        Me.lvwSchemes.UseCompatibleStateImageBehavior = False
        Me.lvwSchemes.View = System.Windows.Forms.View.Details
        '
        '_lvwSchemes_ColumnHeader_1
        '
        Me._lvwSchemes_ColumnHeader_1.Tag = ""
        Me._lvwSchemes_ColumnHeader_1.Text = "Scheme ID"
        Me._lvwSchemes_ColumnHeader_1.Width = 80
        '
        '_lvwSchemes_ColumnHeader_2
        '
        Me._lvwSchemes_ColumnHeader_2.Tag = ""
        Me._lvwSchemes_ColumnHeader_2.Text = "Description"
        Me._lvwSchemes_ColumnHeader_2.Width = 200
        '
        '_lvwSchemes_ColumnHeader_3
        '
        Me._lvwSchemes_ColumnHeader_3.Tag = ""
        Me._lvwSchemes_ColumnHeader_3.Text = "Mask Code"
        Me._lvwSchemes_ColumnHeader_3.Width = 120
        '
        '_lvwSchemes_ColumnHeader_4
        '
        Me._lvwSchemes_ColumnHeader_4.Tag = ""
        Me._lvwSchemes_ColumnHeader_4.Text = "Fixed Number"
        Me._lvwSchemes_ColumnHeader_4.Width = 100
        '
        '_lvwSchemes_ColumnHeader_5
        '
        Me._lvwSchemes_ColumnHeader_5.Tag = ""
        Me._lvwSchemes_ColumnHeader_5.Text = "Scheme"
        Me._lvwSchemes_ColumnHeader_5.Width = 60
        '
        '_lvwSchemes_ColumnHeader_6
        '
        Me._lvwSchemes_ColumnHeader_6.Tag = ""
        Me._lvwSchemes_ColumnHeader_6.Text = "Type"
        Me._lvwSchemes_ColumnHeader_6.Width = 120
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.SystemColors.Window
        Me.ImageList1.Images.SetKeyName(0, "Textfile")
        '
        'cmdSchemeEdit
        '
        Me.cmdSchemeEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSchemeEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSchemeEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSchemeEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSchemeEdit.Location = New System.Drawing.Point(168, 360)
        Me.cmdSchemeEdit.Name = "cmdSchemeEdit"
        Me.cmdSchemeEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSchemeEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdSchemeEdit.TabIndex = 8
        Me.cmdSchemeEdit.Text = "Edit"
        Me.cmdSchemeEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSchemeEdit.UseVisualStyleBackColor = False
        '
        'cmdSchemeAdd
        '
        Me.cmdSchemeAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSchemeAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSchemeAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSchemeAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSchemeAdd.Location = New System.Drawing.Point(16, 360)
        Me.cmdSchemeAdd.Name = "cmdSchemeAdd"
        Me.cmdSchemeAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSchemeAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdSchemeAdd.TabIndex = 7
        Me.cmdSchemeAdd.Text = "Add"
        Me.cmdSchemeAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSchemeAdd.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(713, 469)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Policy Numbering Schemes (Maintenance Screen)"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.fraSchemes.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
	Sub lvwSchemes_InitializeColumnKeys()
		Me._lvwSchemes_ColumnHeader_1.Name = ""
		Me._lvwSchemes_ColumnHeader_2.Name = ""
		Me._lvwSchemes_ColumnHeader_3.Name = ""
		Me._lvwSchemes_ColumnHeader_4.Name = ""
		Me._lvwSchemes_ColumnHeader_5.Name = ""
		Me._lvwSchemes_ColumnHeader_6.Name = ""
	End Sub
#End Region 
End Class