<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmList
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
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public WithEvents cmdNavigate As System.Windows.Forms.Button
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents imgIcon As System.Windows.Forms.PictureBox
    Public WithEvents lblCashTotal As System.Windows.Forms.Label
    Public WithEvents pnlTotal As System.Windows.Forms.Label
    Public WithEvents imglImages As System.Windows.Forms.ImageList
    Private WithEvents _lvwListDetails_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwListDetails_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwListDetails_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwListDetails_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwListDetails_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwListDetails_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwListDetails As System.Windows.Forms.ListView
    Public WithEvents cmdEdit As System.Windows.Forms.Button
    Public WithEvents cmdRemove As System.Windows.Forms.Button
    Public WithEvents cmdAdd As System.Windows.Forms.Button
    Public WithEvents cmdAllocate As System.Windows.Forms.Button
    Public WithEvents cmdPost As System.Windows.Forms.Button
    Public WithEvents cmdReverse As System.Windows.Forms.Button
    Public WithEvents cmdView As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmList))
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
        Me.chkSplitReceipt = New System.Windows.Forms.CheckBox
        Me.chkAutoAllocateIfAble = New System.Windows.Forms.CheckBox
        Me.cmdReverse = New System.Windows.Forms.Button
        Me.cmdView = New System.Windows.Forms.Button
        Me.imgIcon = New System.Windows.Forms.PictureBox
        Me.lblCashTotal = New System.Windows.Forms.Label
        Me.pnlTotal = New System.Windows.Forms.Label
        Me.lvwListDetails = New System.Windows.Forms.ListView
        Me._lvwListDetails_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwListDetails_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwListDetails_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwListDetails_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwListDetails_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwListDetails_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwListDetails_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me.imglImages = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdRemove = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdAllocate = New System.Windows.Forms.Button
        Me.cmdPost = New System.Windows.Forms.Button
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(8, 376)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 7
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        Me.cmdNavigate.Visible = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(536, 376)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 6
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
        Me.cmdCancel.Location = New System.Drawing.Point(456, 376)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 5
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
        Me.cmdOK.Location = New System.Drawing.Point(376, 376)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 4
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(299, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(605, 365)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkSplitReceipt)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkAutoAllocateIfAble)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdReverse)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdView)
        Me._tabMainTab_TabPage0.Controls.Add(Me.imgIcon)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCashTotal)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pnlTotal)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwListDetails)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdEdit)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdRemove)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAdd)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAllocate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdPost)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(597, 339)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Items"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'chkSplitReceipt
        '
        Me.chkSplitReceipt.AutoSize = True
        Me.chkSplitReceipt.Location = New System.Drawing.Point(196, 19)
        Me.chkSplitReceipt.Name = "chkSplitReceipt"
        Me.chkSplitReceipt.Size = New System.Drawing.Size(107, 17)
        Me.chkSplitReceipt.TabIndex = 17
        Me.chkSplitReceipt.Text = "Split Receipt ?"
        Me.chkSplitReceipt.UseVisualStyleBackColor = True
        Me.chkSplitReceipt.Visible = False
        '
        'chkAutoAllocateIfAble
        '
        Me.chkAutoAllocateIfAble.AutoSize = True
        Me.chkAutoAllocateIfAble.Location = New System.Drawing.Point(16, 19)
        Me.chkAutoAllocateIfAble.Name = "chkAutoAllocateIfAble"
        Me.chkAutoAllocateIfAble.Size = New System.Drawing.Size(143, 17)
        Me.chkAutoAllocateIfAble.TabIndex = 16
        Me.chkAutoAllocateIfAble.Text = "Auto Allocate If Able"
        Me.chkAutoAllocateIfAble.UseVisualStyleBackColor = True
        '
        'cmdReverse
        '
        Me.cmdReverse.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReverse.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReverse.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReverse.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReverse.Location = New System.Drawing.Point(176, 316)
        Me.cmdReverse.Name = "cmdReverse"
        Me.cmdReverse.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReverse.Size = New System.Drawing.Size(71, 22)
        Me.cmdReverse.TabIndex = 12
        Me.cmdReverse.Text = "&Reverse"
        Me.cmdReverse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdReverse.UseVisualStyleBackColor = False
        '
        'cmdView
        '
        Me.cmdView.BackColor = System.Drawing.SystemColors.Control
        Me.cmdView.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdView.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdView.Location = New System.Drawing.Point(96, 316)
        Me.cmdView.Name = "cmdView"
        Me.cmdView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdView.Size = New System.Drawing.Size(71, 22)
        Me.cmdView.TabIndex = 13
        Me.cmdView.Text = "View"
        Me.cmdView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdView.UseVisualStyleBackColor = False
        '
        'imgIcon
        '
        Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon.Image = CType(resources.GetObject("imgIcon.Image"), System.Drawing.Image)
        Me.imgIcon.Location = New System.Drawing.Point(552, 4)
        Me.imgIcon.Name = "imgIcon"
        Me.imgIcon.Size = New System.Drawing.Size(32, 32)
        Me.imgIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.imgIcon.TabIndex = 0
        Me.imgIcon.TabStop = False
        '
        'lblCashTotal
        '
        Me.lblCashTotal.BackColor = System.Drawing.SystemColors.Control
        Me.lblCashTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCashTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCashTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCashTotal.Location = New System.Drawing.Point(432, 301)
        Me.lblCashTotal.Name = "lblCashTotal"
        Me.lblCashTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCashTotal.Size = New System.Drawing.Size(34, 18)
        Me.lblCashTotal.TabIndex = 11
        Me.lblCashTotal.Text = "Total:"
        '
        'pnlTotal
        '
        Me.pnlTotal.BackColor = System.Drawing.SystemColors.Control
        Me.pnlTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.pnlTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlTotal.Location = New System.Drawing.Point(472, 300)
        Me.pnlTotal.Name = "pnlTotal"
        Me.pnlTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlTotal.Size = New System.Drawing.Size(113, 19)
        Me.pnlTotal.TabIndex = 14
        '
        'lvwListDetails
        '
        Me.lvwListDetails.BackColor = System.Drawing.SystemColors.Window
        Me.lvwListDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwListDetails.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwListDetails_ColumnHeader_1, Me._lvwListDetails_ColumnHeader_2, Me._lvwListDetails_ColumnHeader_3, Me._lvwListDetails_ColumnHeader_4, Me._lvwListDetails_ColumnHeader_5, Me._lvwListDetails_ColumnHeader_6, Me._lvwListDetails_ColumnHeader_7})
        Me.lvwListDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwListDetails.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwListDetails.FullRowSelect = True
        Me.lvwListDetails.HideSelection = False
        Me.lvwListDetails.Location = New System.Drawing.Point(16, 44)
        Me.lvwListDetails.MultiSelect = False
        Me.lvwListDetails.Name = "lvwListDetails"
        Me.lvwListDetails.Size = New System.Drawing.Size(569, 249)
        Me.lvwListDetails.SmallImageList = Me.imglImages
        Me.lvwListDetails.TabIndex = 0
        Me.lvwListDetails.UseCompatibleStateImageBehavior = False
        Me.lvwListDetails.View = System.Windows.Forms.View.Details
        '
        '_lvwListDetails_ColumnHeader_1
        '
        Me._lvwListDetails_ColumnHeader_1.Text = "Media Reference"
        Me._lvwListDetails_ColumnHeader_1.Width = 291
        '
        '_lvwListDetails_ColumnHeader_2
        '
        Me._lvwListDetails_ColumnHeader_2.DisplayIndex = 2
        Me._lvwListDetails_ColumnHeader_2.Text = "Media Type"
        Me._lvwListDetails_ColumnHeader_2.Width = 97
        '
        '_lvwListDetails_ColumnHeader_3
        '
        Me._lvwListDetails_ColumnHeader_3.DisplayIndex = 3
        Me._lvwListDetails_ColumnHeader_3.Text = "Amount"
        Me._lvwListDetails_ColumnHeader_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwListDetails_ColumnHeader_3.Width = 97
        '
        '_lvwListDetails_ColumnHeader_4
        '
        Me._lvwListDetails_ColumnHeader_4.DisplayIndex = 4
        Me._lvwListDetails_ColumnHeader_4.Text = "Account"
        Me._lvwListDetails_ColumnHeader_4.Width = 97
        '
        '_lvwListDetails_ColumnHeader_5
        '
        Me._lvwListDetails_ColumnHeader_5.DisplayIndex = 5
        Me._lvwListDetails_ColumnHeader_5.Text = "Status"
        Me._lvwListDetails_ColumnHeader_5.Width = 97
        '
        '_lvwListDetails_ColumnHeader_6
        '
        Me._lvwListDetails_ColumnHeader_6.DisplayIndex = 6
        Me._lvwListDetails_ColumnHeader_6.Text = "Letter ?"
        Me._lvwListDetails_ColumnHeader_6.Width = 97
        '
        '_lvwListDetails_ColumnHeader_7
        '
        Me._lvwListDetails_ColumnHeader_7.DisplayIndex = 1
        Me._lvwListDetails_ColumnHeader_7.Text = "Type"
        '
        'imglImages
        '
        Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.imglImages.Images.SetKeyName(0, "CashListImage")
        Me.imglImages.Images.SetKeyName(1, "PFListImage")
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(96, 300)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(71, 22)
        Me.cmdEdit.TabIndex = 3
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdRemove
        '
        Me.cmdRemove.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRemove.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRemove.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRemove.Location = New System.Drawing.Point(176, 300)
        Me.cmdRemove.Name = "cmdRemove"
        Me.cmdRemove.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRemove.Size = New System.Drawing.Size(71, 22)
        Me.cmdRemove.TabIndex = 2
        Me.cmdRemove.Text = "&Remove"
        Me.cmdRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRemove.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(16, 300)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(71, 22)
        Me.cmdAdd.TabIndex = 0
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdAllocate
        '
        Me.cmdAllocate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAllocate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAllocate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAllocate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAllocate.Location = New System.Drawing.Point(256, 300)
        Me.cmdAllocate.Name = "cmdAllocate"
        Me.cmdAllocate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAllocate.Size = New System.Drawing.Size(71, 22)
        Me.cmdAllocate.TabIndex = 9
        Me.cmdAllocate.Text = "Alloca&te"
        Me.cmdAllocate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAllocate.UseVisualStyleBackColor = False
        '
        'cmdPost
        '
        Me.cmdPost.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPost.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPost.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPost.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPost.Location = New System.Drawing.Point(336, 300)
        Me.cmdPost.Name = "cmdPost"
        Me.cmdPost.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPost.Size = New System.Drawing.Size(71, 22)
        Me.cmdPost.TabIndex = 10
        Me.cmdPost.Text = "&Post"
        Me.cmdPost.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPost.UseVisualStyleBackColor = False
        '
        'frmList
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(618, 403)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(183, 210)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmList"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Cash List Items"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents chkAutoAllocateIfAble As System.Windows.Forms.CheckBox
    Friend WithEvents chkSplitReceipt As System.Windows.Forms.CheckBox
    Private WithEvents _lvwListDetails_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
#End Region
End Class