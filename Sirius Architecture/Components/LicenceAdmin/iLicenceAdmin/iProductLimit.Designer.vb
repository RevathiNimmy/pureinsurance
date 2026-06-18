<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProductLimit
#Region "Windows Form Designer generated code "
    Public XmlFiledata As String = ""
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
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtLicenceFileXml = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'txtLicenceFileXml
        '
        Me.txtLicenceFileXml.BackColor = System.Drawing.SystemColors.Window
        Me.txtLicenceFileXml.Location = New System.Drawing.Point(0, 0)
        Me.txtLicenceFileXml.Multiline = True
        Me.txtLicenceFileXml.Name = "txtLicenceFileXml"
        Me.txtLicenceFileXml.ReadOnly = True
        Me.txtLicenceFileXml.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtLicenceFileXml.Size = New System.Drawing.Size(797, 434)
        Me.txtLicenceFileXml.TabIndex = 0
        '
        'frmProductLimit
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(8, 17)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(798, 446)
        Me.Controls.Add(Me.txtLicenceFileXml)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(364, 341)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmProductLimit"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Licence File Details"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtLicenceFileXml As TextBox
#End Region
End Class