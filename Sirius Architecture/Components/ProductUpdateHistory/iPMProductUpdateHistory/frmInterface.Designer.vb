<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
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
	Public WithEvents mnuFileExit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuViewLarge As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuViewSmall As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuViewDetails As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuView As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents lblListView As System.Windows.Forms.Label
	Public WithEvents tvwProduct As System.Windows.Forms.TreeView
	Public WithEvents lvwProduct As System.Windows.Forms.ListView
	Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents SSTab1 As System.Windows.Forms.TabControl
	Public WithEvents ImgList32 As System.Windows.Forms.ImageList
	Public WithEvents ImgList16 As System.Windows.Forms.ImageList
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFileExit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuView = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuViewLarge = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuViewSmall = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuViewDetails = New System.Windows.Forms.ToolStripMenuItem
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdOk = New System.Windows.Forms.Button
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblListView = New System.Windows.Forms.Label
        Me.tvwProduct = New System.Windows.Forms.TreeView
        Me.ImgList16 = New System.Windows.Forms.ImageList(Me.components)
        Me.lvwProduct = New System.Windows.Forms.ListView
        Me.ImgList32 = New System.Windows.Forms.ImageList(Me.components)
        Me.MainMenu1.SuspendLayout()
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuView})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(645, 24)
        Me.MainMenu1.TabIndex = 5
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFileExit})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(35, 20)
        Me.mnuFile.Text = "&File"
        '
        'mnuFileExit
        '
        Me.mnuFileExit.Name = "mnuFileExit"
        Me.mnuFileExit.Size = New System.Drawing.Size(92, 22)
        Me.mnuFileExit.Text = "&Exit"
        '
        'mnuView
        '
        Me.mnuView.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuViewLarge, Me.mnuViewSmall, Me.mnuViewDetails})
        Me.mnuView.Name = "mnuView"
        Me.mnuView.Size = New System.Drawing.Size(41, 20)
        Me.mnuView.Text = "&View"
        '
        'mnuViewLarge
        '
        Me.mnuViewLarge.Name = "mnuViewLarge"
        Me.mnuViewLarge.Size = New System.Drawing.Size(128, 22)
        Me.mnuViewLarge.Text = "Large icons"
        '
        'mnuViewSmall
        '
        Me.mnuViewSmall.Name = "mnuViewSmall"
        Me.mnuViewSmall.Size = New System.Drawing.Size(128, 22)
        Me.mnuViewSmall.Text = "Small icons"
        '
        'mnuViewDetails
        '
        Me.mnuViewDetails.Name = "mnuViewDetails"
        Me.mnuViewDetails.Size = New System.Drawing.Size(128, 22)
        Me.mnuViewDetails.Text = "Details"
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(566, 352)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 25)
        Me.cmdHelp.TabIndex = 4
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdOk
        '
        Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOk.Location = New System.Drawing.Point(488, 352)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(73, 25)
        Me.cmdOk.TabIndex = 3
        Me.cmdOk.Text = "&Ok"
        Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOk.UseVisualStyleBackColor = False
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(209, 18)
        Me.SSTab1.Location = New System.Drawing.Point(8, 32)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(635, 317)
        Me.SSTab1.TabIndex = 0
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.Label1)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblListView)
        Me._SSTab1_TabPage0.Controls.Add(Me.tvwProduct)
        Me._SSTab1_TabPage0.Controls.Add(Me.lvwProduct)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(627, 291)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "Update History"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(12, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(157, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Products"
        '
        'lblListView
        '
        Me.lblListView.BackColor = System.Drawing.SystemColors.Control
        Me.lblListView.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblListView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblListView.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblListView.Location = New System.Drawing.Point(172, 16)
        Me.lblListView.Name = "lblListView"
        Me.lblListView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblListView.Size = New System.Drawing.Size(209, 13)
        Me.lblListView.TabIndex = 2
        Me.lblListView.Text = "Updates"
        '
        'tvwProduct
        '
        Me.tvwProduct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tvwProduct.ImageIndex = 0
        Me.tvwProduct.ImageList = Me.ImgList16
        Me.tvwProduct.Indent = 36
        Me.tvwProduct.LabelEdit = True
        Me.tvwProduct.Location = New System.Drawing.Point(12, 28)
        Me.tvwProduct.Name = "tvwProduct"
        Me.tvwProduct.SelectedImageIndex = 0
        Me.tvwProduct.Size = New System.Drawing.Size(157, 253)
        Me.tvwProduct.TabIndex = 5
        '
        'ImgList16
        '
        Me.ImgList16.ImageStream = CType(resources.GetObject("ImgList16.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImgList16.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImgList16.Images.SetKeyName(0, "COMPUTER")
        Me.ImgList16.Images.SetKeyName(1, "PRODUCT")
        Me.ImgList16.Images.SetKeyName(2, "NOTE")
        '
        'lvwProduct
        '
        Me.lvwProduct.BackColor = System.Drawing.SystemColors.Window
        Me.lvwProduct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwProduct.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwProduct.HideSelection = False
        Me.lvwProduct.LabelEdit = True
        Me.lvwProduct.LargeImageList = Me.ImgList32
        Me.lvwProduct.Location = New System.Drawing.Point(172, 28)
        Me.lvwProduct.Name = "lvwProduct"
        Me.lvwProduct.Size = New System.Drawing.Size(445, 253)
        Me.lvwProduct.SmallImageList = Me.ImgList16
        Me.lvwProduct.TabIndex = 6
        Me.lvwProduct.UseCompatibleStateImageBehavior = False
        Me.lvwProduct.View = System.Windows.Forms.View.Details
        '
        'ImgList32
        '
        Me.ImgList32.ImageStream = CType(resources.GetObject("ImgList32.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImgList32.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImgList32.Images.SetKeyName(0, "COMPUTER")
        Me.ImgList32.Images.SetKeyName(1, "PRODUCT")
        Me.ImgList32.Images.SetKeyName(2, "NOTE")
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(645, 385)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.SSTab1)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 42)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Product Update History Viewer"
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class