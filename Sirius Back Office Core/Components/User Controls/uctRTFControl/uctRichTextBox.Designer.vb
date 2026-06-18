<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctRichTextBox
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        InitializeilsToolbar()
        UserControl_InitProperties()
        UserControl_Initialize()
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
    Friend WithEvents _ilsToolbar_0 As System.Windows.Forms.ImageList
    Public CommonDialog1Font As System.Windows.Forms.FontDialog
    Public CommonDialog1Color As System.Windows.Forms.ColorDialog
    Friend WithEvents rtfEdit As System.Windows.Forms.RichTextBox
    Friend WithEvents butPrint As System.Windows.Forms.ToolStripButton
    Friend WithEvents butPreview As System.Windows.Forms.ToolStripButton
    Friend WithEvents butNew As System.Windows.Forms.ToolStripButton
    Friend WithEvents butSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents butOpen As System.Windows.Forms.ToolStripButton
    Friend WithEvents _tlbMain_Button6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents butCut As System.Windows.Forms.ToolStripButton
    Friend WithEvents butCopy As System.Windows.Forms.ToolStripButton
    Friend WithEvents butPaste As System.Windows.Forms.ToolStripButton
    Friend WithEvents _tlbMain_Button10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents butUndo As System.Windows.Forms.ToolStripButton
    Friend WithEvents butRedo As System.Windows.Forms.ToolStripButton
    Friend WithEvents _tlbMain_Button13 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents butBold As System.Windows.Forms.ToolStripButton
    Friend WithEvents butItalic As System.Windows.Forms.ToolStripButton
    Friend WithEvents butUnderline As System.Windows.Forms.ToolStripButton
    Friend WithEvents butStrikeThru As System.Windows.Forms.ToolStripButton
    Friend WithEvents _tlbMain_Button18 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents butColor As System.Windows.Forms.ToolStripButton
    Friend WithEvents butFont As System.Windows.Forms.ToolStripButton
    Friend WithEvents _tlbMain_Button21 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents butLeft As System.Windows.Forms.ToolStripButton
    Friend WithEvents butCenter As System.Windows.Forms.ToolStripButton
    Friend WithEvents butRight As System.Windows.Forms.ToolStripButton
    Friend WithEvents _tlbMain_Button25 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents butBullet As System.Windows.Forms.ToolStripButton
    Friend WithEvents butDecreaseIndent As System.Windows.Forms.ToolStripButton
    Friend WithEvents butIncreaseIndent As System.Windows.Forms.ToolStripButton
    Friend WithEvents tlbMain As System.Windows.Forms.ToolStrip
    Friend ilsToolbar(0) As System.Windows.Forms.ImageList
    Friend WithEvents sad As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ss As System.Windows.Forms.ToolStripMenuItem
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctRichTextBox))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me._ilsToolbar_0 = New System.Windows.Forms.ImageList(Me.components)
        Me.CommonDialog1Font = New System.Windows.Forms.FontDialog
        Me.CommonDialog1Color = New System.Windows.Forms.ColorDialog
        Me.rtfEdit = New System.Windows.Forms.RichTextBox
        Me.tlbMain = New System.Windows.Forms.ToolStrip
        Me.butPrint = New System.Windows.Forms.ToolStripButton
        Me.butPreview = New System.Windows.Forms.ToolStripButton
        Me.butNew = New System.Windows.Forms.ToolStripButton
        Me.butSave = New System.Windows.Forms.ToolStripButton
        Me.butOpen = New System.Windows.Forms.ToolStripButton
        Me._tlbMain_Button6 = New System.Windows.Forms.ToolStripSeparator
        Me.butCut = New System.Windows.Forms.ToolStripButton
        Me.butCopy = New System.Windows.Forms.ToolStripButton
        Me.butPaste = New System.Windows.Forms.ToolStripButton
        Me._tlbMain_Button10 = New System.Windows.Forms.ToolStripSeparator
        Me.butUndo = New System.Windows.Forms.ToolStripButton
        Me.butRedo = New System.Windows.Forms.ToolStripButton
        Me._tlbMain_Button13 = New System.Windows.Forms.ToolStripSeparator
        Me.butBold = New System.Windows.Forms.ToolStripButton
        Me.butItalic = New System.Windows.Forms.ToolStripButton
        Me.butUnderline = New System.Windows.Forms.ToolStripButton
        Me.butStrikeThru = New System.Windows.Forms.ToolStripButton
        Me._tlbMain_Button18 = New System.Windows.Forms.ToolStripSeparator
        Me.butColor = New System.Windows.Forms.ToolStripButton
        Me.butFont = New System.Windows.Forms.ToolStripButton
        Me._tlbMain_Button21 = New System.Windows.Forms.ToolStripSeparator
        Me.butLeft = New System.Windows.Forms.ToolStripButton
        Me.butCenter = New System.Windows.Forms.ToolStripButton
        Me.butRight = New System.Windows.Forms.ToolStripButton
        Me._tlbMain_Button25 = New System.Windows.Forms.ToolStripSeparator
        Me.butBullet = New System.Windows.Forms.ToolStripButton
        Me.butDecreaseIndent = New System.Windows.Forms.ToolStripButton
        Me.butIncreaseIndent = New System.Windows.Forms.ToolStripButton
        Me.sad = New System.Windows.Forms.ToolStripMenuItem
        Me.ss = New System.Windows.Forms.ToolStripMenuItem
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument
        Me.PrintPreviewDialog1 = New System.Windows.Forms.PrintPreviewDialog
        Me.tlbMain.SuspendLayout()
        Me.SuspendLayout()
        '
        '_ilsToolbar_0
        '
        Me._ilsToolbar_0.ImageStream = CType(resources.GetObject("_ilsToolbar_0.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me._ilsToolbar_0.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me._ilsToolbar_0.Images.SetKeyName(0, "butNew")
        Me._ilsToolbar_0.Images.SetKeyName(1, "butFont")
        Me._ilsToolbar_0.Images.SetKeyName(2, "butOpen")
        Me._ilsToolbar_0.Images.SetKeyName(3, "butSave")
        Me._ilsToolbar_0.Images.SetKeyName(4, "butCut")
        Me._ilsToolbar_0.Images.SetKeyName(5, "butCopy")
        Me._ilsToolbar_0.Images.SetKeyName(6, "butPaste")
        Me._ilsToolbar_0.Images.SetKeyName(7, "butFind")
        Me._ilsToolbar_0.Images.SetKeyName(8, "butUndo")
        Me._ilsToolbar_0.Images.SetKeyName(9, "butRedo")
        Me._ilsToolbar_0.Images.SetKeyName(10, "butHelp")
        Me._ilsToolbar_0.Images.SetKeyName(11, "butBold")
        Me._ilsToolbar_0.Images.SetKeyName(12, "butItalic")
        Me._ilsToolbar_0.Images.SetKeyName(13, "butUnderline")
        Me._ilsToolbar_0.Images.SetKeyName(14, "butStrikeThru")
        Me._ilsToolbar_0.Images.SetKeyName(15, "butColor")
        Me._ilsToolbar_0.Images.SetKeyName(16, "butLeft")
        Me._ilsToolbar_0.Images.SetKeyName(17, "butCenter")
        Me._ilsToolbar_0.Images.SetKeyName(18, "butRight")
        Me._ilsToolbar_0.Images.SetKeyName(19, "butDecreaseIndent")
        Me._ilsToolbar_0.Images.SetKeyName(20, "butIncreaseIndent")
        Me._ilsToolbar_0.Images.SetKeyName(21, "butBullet")
        Me._ilsToolbar_0.Images.SetKeyName(22, "butPreview")
        Me._ilsToolbar_0.Images.SetKeyName(23, "butPrint")
        '
        'rtfEdit
        '
        Me.rtfEdit.AutoWordSelection = True
        Me.rtfEdit.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.rtfEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.rtfEdit.BulletIndent = 25
        Me.rtfEdit.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtfEdit.HideSelection = False
        Me.rtfEdit.Location = New System.Drawing.Point(26, 58)
        Me.rtfEdit.Name = "rtfEdit"
        Me.rtfEdit.Size = New System.Drawing.Size(321, 147)
        Me.rtfEdit.TabIndex = 0
        Me.rtfEdit.Text = ""
        '
        'tlbMain
        '
        Me.tlbMain.ImageList = Me._ilsToolbar_0
        Me.tlbMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.butPrint, Me.butPreview, Me.butNew, Me.butSave, Me.butOpen, Me._tlbMain_Button6, Me.butCut, Me.butCopy, Me.butPaste, Me._tlbMain_Button10, Me.butUndo, Me.butRedo, Me._tlbMain_Button13, Me.butBold, Me.butItalic, Me.butUnderline, Me.butStrikeThru, Me._tlbMain_Button18, Me.butColor, Me.butFont, Me._tlbMain_Button21, Me.butLeft, Me.butCenter, Me.butRight, Me._tlbMain_Button25, Me.butBullet, Me.butDecreaseIndent, Me.butIncreaseIndent})
        Me.tlbMain.Location = New System.Drawing.Point(0, 0)
        Me.tlbMain.Name = "tlbMain"
        Me.tlbMain.Size = New System.Drawing.Size(484, 25)
        Me.tlbMain.TabIndex = 1
        '
        'butPrint
        '
        Me.butPrint.AutoSize = False
        Me.butPrint.ImageKey = "butPrint"
        Me.butPrint.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.butPrint.Name = "butPrint"
        Me.butPrint.Size = New System.Drawing.Size(24, 22)
        Me.butPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.butPrint.ToolTipText = "Print"
        '
        'butPreview
        '
        Me.butPreview.AutoSize = False
        Me.butPreview.ImageKey = "butPreview"
        Me.butPreview.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.butPreview.Name = "butPreview"
        Me.butPreview.Size = New System.Drawing.Size(24, 22)
        Me.butPreview.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.butPreview.ToolTipText = "Print Preview"
        '
        'butNew
        '
        Me.butNew.AutoSize = False
        Me.butNew.ImageKey = "butNew"
        Me.butNew.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.butNew.Name = "butNew"
        Me.butNew.Size = New System.Drawing.Size(24, 22)
        Me.butNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.butNew.ToolTipText = "New"
        Me.butNew.Visible = False
        '
        'butSave
        '
        Me.butSave.AutoSize = False
        Me.butSave.ImageKey = "butSave"
        Me.butSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.butSave.Name = "butSave"
        Me.butSave.Size = New System.Drawing.Size(24, 22)
        Me.butSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.butSave.ToolTipText = "Save"
        Me.butSave.Visible = False
        '
        'butOpen
        '
        Me.butOpen.AutoSize = False
        Me.butOpen.ImageKey = "butOpen"
        Me.butOpen.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.butOpen.Name = "butOpen"
        Me.butOpen.Size = New System.Drawing.Size(24, 22)
        Me.butOpen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.butOpen.ToolTipText = "Open a file"
        Me.butOpen.Visible = False
        '
        '_tlbMain_Button6
        '
        Me._tlbMain_Button6.AutoSize = False
        Me._tlbMain_Button6.Name = "_tlbMain_Button6"
        Me._tlbMain_Button6.Size = New System.Drawing.Size(6, 22)
        '
        'butCut
        '
        Me.butCut.AutoSize = False
        Me.butCut.ImageKey = "butCut"
        Me.butCut.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.butCut.Name = "butCut"
        Me.butCut.Size = New System.Drawing.Size(24, 22)
        Me.butCut.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.butCut.ToolTipText = "Cut"
        '
        'butCopy
        '
        Me.butCopy.AutoSize = False
        Me.butCopy.ImageKey = "butCopy"
        Me.butCopy.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.butCopy.Name = "butCopy"
        Me.butCopy.Size = New System.Drawing.Size(24, 22)
        Me.butCopy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.butCopy.ToolTipText = "Copy"
        '
        'butPaste
        '
        Me.butPaste.AutoSize = False
        Me.butPaste.ImageKey = "butPaste"
        Me.butPaste.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.butPaste.Name = "butPaste"
        Me.butPaste.Size = New System.Drawing.Size(24, 22)
        Me.butPaste.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.butPaste.ToolTipText = "Paste"
        '
        '_tlbMain_Button10
        '
        Me._tlbMain_Button10.AutoSize = False
        Me._tlbMain_Button10.Name = "_tlbMain_Button10"
        Me._tlbMain_Button10.Size = New System.Drawing.Size(6, 22)
        '
        'butUndo
        '
        Me.butUndo.AutoSize = False
        Me.butUndo.ImageKey = "butUndo"
        Me.butUndo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.butUndo.Name = "butUndo"
        Me.butUndo.Size = New System.Drawing.Size(24, 22)
        Me.butUndo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.butUndo.ToolTipText = "Undo"
        '
        'butRedo
        '
        Me.butRedo.AutoSize = False
        Me.butRedo.ImageKey = "butRedo"
        Me.butRedo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.butRedo.Name = "butRedo"
        Me.butRedo.Size = New System.Drawing.Size(24, 22)
        Me.butRedo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.butRedo.ToolTipText = "Redo"
        '
        '_tlbMain_Button13
        '
        Me._tlbMain_Button13.AutoSize = False
        Me._tlbMain_Button13.Name = "_tlbMain_Button13"
        Me._tlbMain_Button13.Size = New System.Drawing.Size(6, 22)
        '
        'butBold
        '
        Me.butBold.AutoSize = False
        Me.butBold.ImageKey = "butBold"
        Me.butBold.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.butBold.Name = "butBold"
        Me.butBold.Size = New System.Drawing.Size(24, 22)
        Me.butBold.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.butBold.ToolTipText = "Bold"
        '
        'butItalic
        '
        Me.butItalic.AutoSize = False
        Me.butItalic.ImageKey = "butItalic"
        Me.butItalic.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.butItalic.Name = "butItalic"
        Me.butItalic.Size = New System.Drawing.Size(24, 22)
        Me.butItalic.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.butItalic.ToolTipText = "Italic"
        '
        'butUnderline
        '
        Me.butUnderline.AutoSize = False
        Me.butUnderline.ImageKey = "butUnderline"
        Me.butUnderline.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.butUnderline.Name = "butUnderline"
        Me.butUnderline.Size = New System.Drawing.Size(24, 22)
        Me.butUnderline.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.butUnderline.ToolTipText = "Underline"
        '
        'butStrikeThru
        '
        Me.butStrikeThru.AutoSize = False
        Me.butStrikeThru.ImageKey = "butStrikeThru"
        Me.butStrikeThru.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.butStrikeThru.Name = "butStrikeThru"
        Me.butStrikeThru.Size = New System.Drawing.Size(24, 22)
        Me.butStrikeThru.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.butStrikeThru.ToolTipText = "StrikeThru"
        Me.butStrikeThru.Visible = False
        '
        '_tlbMain_Button18
        '
        Me._tlbMain_Button18.AutoSize = False
        Me._tlbMain_Button18.Name = "_tlbMain_Button18"
        Me._tlbMain_Button18.Size = New System.Drawing.Size(6, 22)
        '
        'butColor
        '
        Me.butColor.AutoSize = False
        Me.butColor.ImageKey = "butColor"
        Me.butColor.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.butColor.Name = "butColor"
        Me.butColor.Size = New System.Drawing.Size(24, 22)
        Me.butColor.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.butColor.ToolTipText = "Font Color"
        Me.butColor.Visible = False
        '
        'butFont
        '
        Me.butFont.AutoSize = False
        Me.butFont.ImageKey = "butFont"
        Me.butFont.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.butFont.Name = "butFont"
        Me.butFont.Size = New System.Drawing.Size(24, 22)
        Me.butFont.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.butFont.ToolTipText = "Font type and Size"
        '
        '_tlbMain_Button21
        '
        Me._tlbMain_Button21.AutoSize = False
        Me._tlbMain_Button21.Name = "_tlbMain_Button21"
        Me._tlbMain_Button21.Size = New System.Drawing.Size(6, 22)
        '
        'butLeft
        '
        Me.butLeft.AutoSize = False
        Me.butLeft.ImageKey = "butLeft"
        Me.butLeft.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.butLeft.Name = "butLeft"
        Me.butLeft.Size = New System.Drawing.Size(24, 22)
        Me.butLeft.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.butLeft.ToolTipText = "Align Left"
        '
        'butCenter
        '
        Me.butCenter.AutoSize = False
        Me.butCenter.ImageKey = "butCenter"
        Me.butCenter.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.butCenter.Name = "butCenter"
        Me.butCenter.Size = New System.Drawing.Size(24, 22)
        Me.butCenter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.butCenter.ToolTipText = "Center"
        '
        'butRight
        '
        Me.butRight.AutoSize = False
        Me.butRight.ImageKey = "butRight"
        Me.butRight.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.butRight.Name = "butRight"
        Me.butRight.Size = New System.Drawing.Size(24, 22)
        Me.butRight.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.butRight.ToolTipText = "Align Right"
        '
        '_tlbMain_Button25
        '
        Me._tlbMain_Button25.AutoSize = False
        Me._tlbMain_Button25.Name = "_tlbMain_Button25"
        Me._tlbMain_Button25.Size = New System.Drawing.Size(6, 22)
        '
        'butBullet
        '
        Me.butBullet.AutoSize = False
        Me.butBullet.ImageKey = "butBullet"
        Me.butBullet.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.butBullet.Name = "butBullet"
        Me.butBullet.Size = New System.Drawing.Size(24, 22)
        Me.butBullet.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.butBullet.ToolTipText = "Bullets"
        '
        'butDecreaseIndent
        '
        Me.butDecreaseIndent.AutoSize = False
        Me.butDecreaseIndent.ImageKey = "butDecreaseIndent"
        Me.butDecreaseIndent.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.butDecreaseIndent.Name = "butDecreaseIndent"
        Me.butDecreaseIndent.Size = New System.Drawing.Size(24, 22)
        Me.butDecreaseIndent.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.butDecreaseIndent.ToolTipText = "Decrease Indent"
        '
        'butIncreaseIndent
        '
        Me.butIncreaseIndent.AutoSize = False
        Me.butIncreaseIndent.ImageKey = "butIncreaseIndent"
        Me.butIncreaseIndent.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.butIncreaseIndent.Name = "butIncreaseIndent"
        Me.butIncreaseIndent.Size = New System.Drawing.Size(24, 22)
        Me.butIncreaseIndent.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.butIncreaseIndent.ToolTipText = "Increase Indent"
        '
        'sad
        '
        Me.sad.Name = "sad"
        Me.sad.Size = New System.Drawing.Size(32, 19)
        Me.sad.Text = "gsdfgsdfg"
        '
        'ss
        '
        Me.ss.Name = "ss"
        Me.ss.Size = New System.Drawing.Size(32, 19)
        Me.ss.Text = "ee"
        '
        'PrintDialog1
        '
        Me.PrintDialog1.Document = Me.PrintDocument1
        Me.PrintDialog1.UseEXDialog = True
        '
        'PrintDocument1
        '
        '
        'PrintPreviewDialog1
        '
        Me.PrintPreviewDialog1.AutoScrollMargin = New System.Drawing.Size(0, 0)
        Me.PrintPreviewDialog1.AutoScrollMinSize = New System.Drawing.Size(0, 0)
        Me.PrintPreviewDialog1.ClientSize = New System.Drawing.Size(400, 300)
        Me.PrintPreviewDialog1.Document = Me.PrintDocument1
        Me.PrintPreviewDialog1.Enabled = True
        Me.PrintPreviewDialog1.Icon = CType(resources.GetObject("PrintPreviewDialog1.Icon"), System.Drawing.Icon)
        Me.PrintPreviewDialog1.Name = "PrintPreviewDialog1"
        Me.PrintPreviewDialog1.Visible = False
        '
        'uctRichTextBox
        '
        Me.Controls.Add(Me.rtfEdit)
        Me.Controls.Add(Me.tlbMain)
        Me.Name = "uctRichTextBox"
        Me.Size = New System.Drawing.Size(484, 268)
        Me.tlbMain.ResumeLayout(False)
        Me.tlbMain.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializeilsToolbar()
        Me.ilsToolbar(0) = _ilsToolbar_0
    End Sub
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents PrintPreviewDialog1 As System.Windows.Forms.PrintPreviewDialog
#End Region
End Class
