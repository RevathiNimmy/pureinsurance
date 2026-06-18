<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDummy
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
    'TODO check at run time
    'Public WithEvents MAPIMessages1 As AxMSMAPI.AxMAPIMessages
    'Public WithEvents MAPISession1 As AxMSMAPI.AxMAPISession
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDummy))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MAPIMessages1 = New AxMSMAPI.AxMAPIMessages
        Me.MAPISession1 = New AxMSMAPI.AxMAPISession
        CType(Me.MAPIMessages1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MAPISession1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MAPIMessages1
        '
        Me.MAPIMessages1.Enabled = True
        Me.MAPIMessages1.Location = New System.Drawing.Point(48, 16)
        Me.MAPIMessages1.Name = "MAPIMessages1"
        Me.MAPIMessages1.OcxState = CType(resources.GetObject("MAPIMessages1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.MAPIMessages1.Size = New System.Drawing.Size(38, 38)
        Me.MAPIMessages1.TabIndex = 0
        '
        'MAPISession1
        '
        Me.MAPISession1.Enabled = True
        Me.MAPISession1.Location = New System.Drawing.Point(92, 16)
        Me.MAPISession1.Name = "MAPISession1"
        Me.MAPISession1.OcxState = CType(resources.GetObject("MAPISession1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.MAPISession1.Size = New System.Drawing.Size(38, 38)
        Me.MAPISession1.TabIndex = 1
        '
        'frmDummy
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(202, 57)
        Me.Controls.Add(Me.MAPISession1)
        Me.Controls.Add(Me.MAPIMessages1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmDummy"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Form1"
        CType(Me.MAPIMessages1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MAPISession1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents MAPIMessages1 As AxMSMAPI.AxMAPIMessages
    Friend WithEvents MAPISession1 As AxMSMAPI.AxMAPISession
#End Region 
End Class