<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBrowser
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
        Me.MdiParent = objfrmParentMDI 'iDOCViewer.frmParentMDI
        objfrmParentMDI.Show()
        'The MDI form in the VB6 project had its
        'AutoShowChildren property set to True
        'To simulate the VB6 behavior, we need to
        'automatically Show the form whenever it
        'is loaded.  If you do not want this behavior
        'then delete the following line of code

        Me.Show()
    End Sub
    Private Sub Ctx_frmParentMDI_mnuView_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_frmParentMDI_mnuView.Opening
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        Ctx_frmParentMDI_mnuView.Items.Clear()
        'We are moving the submenus from original menu to the context menu before displaying it
        For Each item As System.Windows.Forms.ToolStripItem In objfrmParentMDI.mnuView.DropDownItems
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            Ctx_frmParentMDI_mnuView.Items.Add(item)
        Next item
        e.Cancel = False
    End Sub
    Private Sub Ctx_frmParentMDI_mnuView_Closing(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_frmParentMDI_mnuView.Closing
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        'We are moving the submenus the context menu back to the original menu after displaying
        For Each item As System.Windows.Forms.ToolStripItem In Ctx_frmParentMDI_mnuView.Items
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            objfrmParentMDI.mnuView.DropDownItems.Add(item)
        Next item
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
    Public WithEvents timTimer As System.Windows.Forms.Timer
    Public WithEvents brwWebBrowser As System.Windows.Forms.WebBrowser
    Public WithEvents imlIcons As System.Windows.Forms.ImageList
    Public WithEvents Ctx_frmParentMDI_mnuView As System.Windows.Forms.ContextMenuStrip
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBrowser))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.timTimer = New System.Windows.Forms.Timer(Me.components)
        Me.brwWebBrowser = New System.Windows.Forms.WebBrowser
        Me.imlIcons = New System.Windows.Forms.ImageList(Me.components)
        Me.Ctx_frmParentMDI_mnuView = New System.Windows.Forms.ContextMenuStrip(Me.components)

        Me.SuspendLayout()
        '
        'timTimer
        '
        Me.timTimer.Interval = 5
        '
        'brwWebBrowser
        '
        Me.brwWebBrowser.Location = New System.Drawing.Point(2, 2)
        Me.brwWebBrowser.Name = "brwWebBrowser"
        Me.brwWebBrowser.Size = New System.Drawing.Size(360, 249)
        Me.brwWebBrowser.TabIndex = 0
        '
        'imlIcons
        '
        Me.imlIcons.ImageStream = CType(resources.GetObject("imlIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imlIcons.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Object), Integer), CType(CType(192, Object), Integer), CType(CType(192, Object), Integer))
        Me.imlIcons.Images.SetKeyName(0, "")
        Me.imlIcons.Images.SetKeyName(1, "")
        Me.imlIcons.Images.SetKeyName(2, "")
        Me.imlIcons.Images.SetKeyName(3, "")
        Me.imlIcons.Images.SetKeyName(4, "")
        Me.imlIcons.Images.SetKeyName(5, "")
        '
        'Ctx_frmParentMDI_mnuView
        '
        Me.Ctx_frmParentMDI_mnuView.Name = "Ctx_frmParentMDI_mnuView"
        Me.Ctx_frmParentMDI_mnuView.Size = New System.Drawing.Size(61, 4)
        '


        '
        'frmBrowser
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(436, 342)
        Me.Controls.Add(Me.brwWebBrowser)

        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Object))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(204, 223)
        Me.Name = "frmBrowser"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized

        Me.ResumeLayout(False)

    End Sub

#End Region
End Class