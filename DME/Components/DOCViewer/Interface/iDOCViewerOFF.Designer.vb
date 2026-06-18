<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChildOFF
    Private objfrmParentMDI As New iDOCViewer.frmParentMDI
#Region "Windows Form Designer generated code "
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
        'VB6.ShowForm(objfrmParentMDI, Modal:=VB6.FormShowConstants.Modal)
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
        GC.Collect()
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    'Public WithEvents FCOffice As AxDSOFramer.AxFramerControl
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmChildOFF))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        'Me.FCOffice = New AxDSOFramer.AxFramerControl
        'CType(Me.FCOffice, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'FCOffice
        '
        'Me.FCOffice.Enabled = True
        'Me.FCOffice.Location = New System.Drawing.Point(-1, 0)
        'Me.FCOffice.Name = "FCOffice"
        'Me.FCOffice.OcxState = CType(resources.GetObject("FCOffice.OcxState"), System.Windows.Forms.AxHost.State)
        'Me.FCOffice.Size = New System.Drawing.Size(597, 476)
        'Me.FCOffice.TabIndex = 0
        '
        'frmChildOFF
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(596, 476)
        'Me.Controls.Add(Me.FCOffice)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmChildOFF"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Office Document Viewer"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        'CType(Me.FCOffice, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    'Friend WithEvents FCOffice As AxDSOFramer.AxFramerControl

#End Region
End Class