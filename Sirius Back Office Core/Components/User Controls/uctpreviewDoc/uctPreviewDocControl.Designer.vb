<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctPreviewDocControl
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents HScroll1 As System.Windows.Forms.HScrollBar
    Friend WithEvents VScroll1 As System.Windows.Forms.VScrollBar
    Friend WithEvents pctView As System.Windows.Forms.PictureBox
    Friend WithEvents pctContainer As System.Windows.Forms.PictureBox
    Friend WithEvents pctTemp As System.Windows.Forms.PictureBox
    Friend WithEvents pctThumbnails As System.Windows.Forms.PictureBox
    Friend WithEvents pctSplitter As System.Windows.Forms.PictureBox
    Friend WithEvents VScroll2 As System.Windows.Forms.VScrollBar
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.HScroll1 = New System.Windows.Forms.HScrollBar
        Me.VScroll1 = New System.Windows.Forms.VScrollBar
        Me.pctContainer = New System.Windows.Forms.PictureBox
        Me.pctView = New System.Windows.Forms.PictureBox
        Me.pctThumbnails = New System.Windows.Forms.PictureBox
        Me.pctTemp = New System.Windows.Forms.PictureBox
        Me.pctSplitter = New System.Windows.Forms.PictureBox
        Me.VScroll2 = New System.Windows.Forms.VScrollBar
        CType(Me.pctContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pctContainer.SuspendLayout()
        CType(Me.pctView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pctThumbnails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pctThumbnails.SuspendLayout()
        CType(Me.pctTemp, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pctSplitter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'HScroll1
        '
        Me.HScroll1.Cursor = System.Windows.Forms.Cursors.Default
        Me.HScroll1.LargeChange = 1
        Me.HScroll1.Location = New System.Drawing.Point(264, 192)
        Me.HScroll1.Maximum = 32767
        Me.HScroll1.Name = "HScroll1"
        Me.HScroll1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HScroll1.Size = New System.Drawing.Size(193, 17)
        Me.HScroll1.TabIndex = 7
        Me.HScroll1.TabStop = True
        '
        'VScroll1
        '
        Me.VScroll1.Cursor = System.Windows.Forms.Cursors.Default
        Me.VScroll1.LargeChange = 1
        Me.VScroll1.Location = New System.Drawing.Point(456, 48)
        Me.VScroll1.Maximum = 32767
        Me.VScroll1.Name = "VScroll1"
        Me.VScroll1.Size = New System.Drawing.Size(17, 145)
        Me.VScroll1.TabIndex = 6
        Me.VScroll1.TabStop = True
        '
        'pctContainer
        '
        Me.pctContainer.BackColor = System.Drawing.SystemColors.Control
        Me.pctContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pctContainer.Controls.Add(Me.pctView)
        Me.pctContainer.Cursor = System.Windows.Forms.Cursors.Default
        Me.pctContainer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pctContainer.Location = New System.Drawing.Point(248, 16)
        Me.pctContainer.Name = "pctContainer"
        Me.pctContainer.Size = New System.Drawing.Size(177, 169)
        Me.pctContainer.TabIndex = 4
        Me.pctContainer.TabStop = False
        '
        'pctView
        '
        Me.pctView.BackColor = System.Drawing.SystemColors.Control
        Me.pctView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pctView.Cursor = System.Windows.Forms.Cursors.Default
        Me.pctView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pctView.Location = New System.Drawing.Point(24, 24)
        Me.pctView.Name = "pctView"
        Me.pctView.Size = New System.Drawing.Size(113, 105)
        Me.pctView.TabIndex = 5
        Me.pctView.TabStop = False
        '
        'pctThumbnails
        '
        Me.pctThumbnails.BackColor = System.Drawing.SystemColors.Control
        Me.pctThumbnails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pctThumbnails.Controls.Add(Me.pctTemp)
        Me.pctThumbnails.Cursor = System.Windows.Forms.Cursors.Default
        Me.pctThumbnails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pctThumbnails.Location = New System.Drawing.Point(0, 8)
        Me.pctThumbnails.Name = "pctThumbnails"
        Me.pctThumbnails.Size = New System.Drawing.Size(89, 225)
        Me.pctThumbnails.TabIndex = 2
        Me.pctThumbnails.TabStop = False
        '
        'pctTemp
        '
        Me.pctTemp.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.pctTemp.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pctTemp.Cursor = System.Windows.Forms.Cursors.Default
        Me.pctTemp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pctTemp.Location = New System.Drawing.Point(8, 8)
        Me.pctTemp.Name = "pctTemp"
        Me.pctTemp.Size = New System.Drawing.Size(81, 73)
        Me.pctTemp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pctTemp.TabIndex = 3
        Me.pctTemp.TabStop = False
        '
        'pctSplitter
        '
        Me.pctSplitter.BackColor = System.Drawing.SystemColors.Control
        Me.pctSplitter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pctSplitter.Cursor = System.Windows.Forms.Cursors.Default
        Me.pctSplitter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pctSplitter.Location = New System.Drawing.Point(112, 0)
        Me.pctSplitter.Name = "pctSplitter"
        Me.pctSplitter.Size = New System.Drawing.Size(9, 257)
        Me.pctSplitter.TabIndex = 1
        Me.pctSplitter.TabStop = False
        '
        'VScroll2
        '
        Me.VScroll2.Cursor = System.Windows.Forms.Cursors.Default
        Me.VScroll2.LargeChange = 1
        Me.VScroll2.Location = New System.Drawing.Point(88, 8)
        Me.VScroll2.Maximum = 32767
        Me.VScroll2.Name = "VScroll2"
        Me.VScroll2.Size = New System.Drawing.Size(17, 233)
        Me.VScroll2.TabIndex = 0
        Me.VScroll2.TabStop = True
        '
        'uctPreviewDocControl
        '
        Me.Controls.Add(Me.HScroll1)
        Me.Controls.Add(Me.VScroll1)
        Me.Controls.Add(Me.pctContainer)
        Me.Controls.Add(Me.pctThumbnails)
        Me.Controls.Add(Me.pctSplitter)
        Me.Controls.Add(Me.VScroll2)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctPreviewDocControl"
        Me.Size = New System.Drawing.Size(491, 265)
        CType(Me.pctContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pctContainer.ResumeLayout(False)
        CType(Me.pctView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pctThumbnails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pctThumbnails.ResumeLayout(False)
        CType(Me.pctTemp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pctSplitter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region
End Class