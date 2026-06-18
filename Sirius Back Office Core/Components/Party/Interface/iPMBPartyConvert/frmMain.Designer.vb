<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwParties_InitializeColumnKeys()
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
	Public WithEvents cmdConvert As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdRemove As System.Windows.Forms.Button
	Private WithEvents _lvwParties_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwParties_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwParties_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwParties_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwParties As System.Windows.Forms.ListView
	Public WithEvents fraParties As System.Windows.Forms.GroupBox
    Public WithEvents imlParty As System.Windows.Forms.ImageList
    'Commented as not needed to get the image in the listview
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdConvert = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdRemove = New System.Windows.Forms.Button
        Me.fraParties = New System.Windows.Forms.GroupBox
        Me.lvwParties = New System.Windows.Forms.ListView
        Me._lvwParties_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwParties_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwParties_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwParties_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me.imlParty = New System.Windows.Forms.ImageList(Me.components)
        Me.fraParties.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdConvert
        '
        Me.cmdConvert.BackColor = System.Drawing.SystemColors.Control
        Me.cmdConvert.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdConvert.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdConvert.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdConvert.Location = New System.Drawing.Point(288, 248)
        Me.cmdConvert.Name = "cmdConvert"
        Me.cmdConvert.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdConvert.Size = New System.Drawing.Size(73, 22)
        Me.cmdConvert.TabIndex = 6
        Me.cmdConvert.Text = "C&onvert"
        Me.cmdConvert.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdConvert.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(368, 248)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 5
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(448, 248)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 4
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(8, 248)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 3
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdRemove
        '
        Me.cmdRemove.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRemove.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRemove.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRemove.Location = New System.Drawing.Point(88, 248)
        Me.cmdRemove.Name = "cmdRemove"
        Me.cmdRemove.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRemove.Size = New System.Drawing.Size(73, 22)
        Me.cmdRemove.TabIndex = 2
        Me.cmdRemove.Text = "&Remove"
        Me.cmdRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRemove.UseVisualStyleBackColor = False
        '
        'fraParties
        '
        Me.fraParties.BackColor = System.Drawing.SystemColors.Control
        Me.fraParties.Controls.Add(Me.lvwParties)
        Me.fraParties.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraParties.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraParties.Location = New System.Drawing.Point(8, 8)
        Me.fraParties.Name = "fraParties"
        Me.fraParties.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraParties.Size = New System.Drawing.Size(513, 233)
        Me.fraParties.TabIndex = 0
        Me.fraParties.TabStop = False
        Me.fraParties.Text = "Parties"
        '
        'lvwParties
        '
        Me.lvwParties.BackColor = System.Drawing.SystemColors.Window
        Me.lvwParties.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwParties_ColumnHeader_1, Me._lvwParties_ColumnHeader_2, Me._lvwParties_ColumnHeader_3, Me._lvwParties_ColumnHeader_4})
        Me.lvwParties.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwParties.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwParties.FullRowSelect = True
        Me.lvwParties.LargeImageList = Me.imlParty
        Me.lvwParties.Location = New System.Drawing.Point(8, 16)
        Me.lvwParties.MultiSelect = False
        Me.lvwParties.Name = "lvwParties"
        Me.lvwParties.Size = New System.Drawing.Size(497, 209)
        Me.lvwParties.SmallImageList = Me.imlParty
        Me.lvwParties.TabIndex = 1
        Me.lvwParties.UseCompatibleStateImageBehavior = False
        Me.lvwParties.View = System.Windows.Forms.View.Details
        '
        '_lvwParties_ColumnHeader_1
        '
        Me._lvwParties_ColumnHeader_1.Tag = ""
        Me._lvwParties_ColumnHeader_1.Text = "Short Name"
        Me._lvwParties_ColumnHeader_1.Width = 97
        '
        '_lvwParties_ColumnHeader_2
        '
        Me._lvwParties_ColumnHeader_2.Tag = ""
        Me._lvwParties_ColumnHeader_2.Text = "Name"
        Me._lvwParties_ColumnHeader_2.Width = 97
        '
        '_lvwParties_ColumnHeader_3
        '
        Me._lvwParties_ColumnHeader_3.Tag = ""
        Me._lvwParties_ColumnHeader_3.Text = "Party Type"
        Me._lvwParties_ColumnHeader_3.Width = 97
        '
        '_lvwParties_ColumnHeader_4
        '
        Me._lvwParties_ColumnHeader_4.Tag = ""
        Me._lvwParties_ColumnHeader_4.Text = "Convert to"
        Me._lvwParties_ColumnHeader_4.Width = 97
        '
        'imlParty
        '
        Me.imlParty.ImageStream = CType(resources.GetObject("imlParty.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imlParty.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.imlParty.Images.SetKeyName(0, "folder")
        '
        'frmMain
        '
        Me.AcceptButton = Me.cmdConvert
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(531, 277)
        Me.Controls.Add(Me.cmdConvert)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.cmdRemove)
        Me.Controls.Add(Me.fraParties)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMain"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Party Conversion"
        Me.fraParties.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
	Sub lvwParties_InitializeColumnKeys()
		Me._lvwParties_ColumnHeader_1.Name = ""
		Me._lvwParties_ColumnHeader_2.Name = ""
		Me._lvwParties_ColumnHeader_3.Name = ""
		Me._lvwParties_ColumnHeader_4.Name = ""
	End Sub
#End Region 
End Class