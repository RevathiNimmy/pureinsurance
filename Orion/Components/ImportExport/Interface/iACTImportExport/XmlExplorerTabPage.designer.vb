Partial Class XmlExplorerTabPage
    Inherits System.Windows.Forms.TabPage

    <System.Diagnostics.DebuggerNonUserCode()> _
    Public Sub New(ByVal container As System.ComponentModel.IContainer)
        MyClass.New()

        'Required for Windows.Forms Class Composition Designer support
        If (container IsNot Nothing) Then
            container.Add(Me)
        End If

    End Sub

    '<System.Diagnostics.DebuggerNonUserCode()> _
    'Public Sub New()
    '    MyBase.New()

    '    'This call is required by the Component Designer.
    '    InitializeComponent()

    'End Sub

    'Component overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer = Nothing

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.
    'Do not modify it using the code editor.

    Private Sub InitializeComponent()
        Me.xmlTreeView = New XPathNavigatorTreeView
        '
        'xmlTreeView
        '
        Me.xmlTreeView = New XPathNavigatorTreeView()
        MyBase.SuspendLayout()
        Me.xmlTreeView.Dock = DockStyle.Fill
        Me.xmlTreeView.HideSelection = False
        Me.xmlTreeView.LineColor = Color.Empty
        Me.xmlTreeView.Location = New Point(0, 0)
        Me.xmlTreeView.Name = "xmlTreeView"
        Me.xmlTreeView.Size = New Size(&H79, &H61)
        Me.xmlTreeView.TabIndex = 0
        Me.xmlTreeView.Navigator = Nothing
        MyBase.Controls.Add(Me.xmlTreeView)
        MyBase.ResumeLayout(False)
    End Sub
    Friend WithEvents xmlTreeView As XPathNavigatorTreeView

End Class
