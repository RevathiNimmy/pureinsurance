Imports Newtone.ImageKit
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChildTIF
#Region "Windows Form Designer generated code "
    Private objfrmParentMDI As New iDOCViewer.frmParentMDI
    Public Sub New()
        MyBase.New()
        isInitializingComponent = True
        InitializeComponent()
        isInitializingComponent = False
        'This form is an MDI child.
        'This code simulates the VB6 
        ' functionality of automatically
        ' loading and showing an MDI
        ' child's parent.
        Me.MdiParent = objfrmParentMDI
        objfrmParentMDI.Show()
        'The MDI form in the VB6 project had its
        'AutoShowChildren property set to True
        'To simulate the VB6 behavior, we need to
        'automatically Show the form whenever it
        'is loaded.  If you do not want this behavior
        'then delete the following line of code

        Me.Show()
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
    Public WithEvents IkThumb1 As Win.Thumbnail
    'Public WithEvents IkCommon1 As Object
    Public WithEvents ImageKit1 As Win.ImageKit
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    '<System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmChildTIF))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.IkThumb1 = New Newtone.ImageKit.Win.Thumbnail
        Me.ImageKit1 = New Newtone.ImageKit.Win.ImageKit
        Me.SuspendLayout()
        '
        'IkThumb1
        '
        Me.IkThumb1.Location = New System.Drawing.Point(0, 0)
        Me.IkThumb1.Name = "IkThumb1"
        Me.IkThumb1.Size = New System.Drawing.Size(145, 313)
        Me.IkThumb1.TabIndex = 0
        '
        'ImageKit1
        '
        Me.ImageKit1.AllowDrop = True
        Me.ImageKit1.BackColor = System.Drawing.Color.White
        Me.ImageKit1.DefaultMouseCursor = System.Windows.Forms.Cursors.Default
        Me.ImageKit1.DXFBlack = False
        Me.ImageKit1.Grad = Newtone.ImageKit.Win.LengthUnit.None
        Me.ImageKit1.GradBackgroundColor = System.Drawing.Color.FromArgb(CType(CType(236, Byte), Integer), CType(CType(233, Byte), Integer), CType(CType(216, Byte), Integer))
        Me.ImageKit1.GradColor = System.Drawing.Color.Black
        Me.ImageKit1.Grid = Newtone.ImageKit.Win.LengthUnit.None
        Me.ImageKit1.GridColor = System.Drawing.Color.Silver
        Me.ImageKit1.GridSpace = 10
        Me.ImageKit1.InvalidHatchPattern = Newtone.ImageKit.Win.HatchPattern.ShowHatch
        Me.ImageKit1.Location = New System.Drawing.Point(104, 0)
        Me.ImageKit1.MouseWheelDirection = Newtone.ImageKit.Win.WheelDirection.Vertical
        Me.ImageKit1.Name = "ImageKit1"
        Me.ImageKit1.Rect = New System.Drawing.Rectangle(0, 0, 0, 0)
        Me.ImageKit1.RectDrawRatio = 0
        Me.ImageKit1.RectMouseCursor = System.Windows.Forms.Cursors.SizeAll
        Me.ImageKit1.Refine1BitImage = True
        Me.ImageKit1.Size = New System.Drawing.Size(249, 281)
        Me.ImageKit1.TabIndex = 1
        Me.ImageKit1.ToolTip = Newtone.ImageKit.Win.LengthUnit.None
        Me.ImageKit1.TransparentBlue = CType(0, Byte)
        Me.ImageKit1.TransparentGreen = CType(0, Byte)
        Me.ImageKit1.TransparentRed = CType(0, Byte)
        '
        'frmChildTIF
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(392, 325)
        Me.Controls.Add(Me.IkThumb1)
        Me.Controls.Add(Me.ImageKit1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmChildTIF"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "TIF Document"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)

    End Sub
#End Region
End Class