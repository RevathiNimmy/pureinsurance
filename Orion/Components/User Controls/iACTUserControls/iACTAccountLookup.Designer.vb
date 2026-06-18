<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AccountLookup
#Region "Windows Form Designer generated code "
    'developer guide no 211
    'Friend Sub New()
    Public Sub New()
        MyBase.New()
        isInitializingComponent = True
        InitializeComponent()
        isInitializingComponent = False
        UserControl_InitProperties()
        UserControl_Initialize()
    End Sub
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
     Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
        If Disposing Then
            UserControl_Terminate()
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(Disposing)
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents cmdAccountLookup As System.Windows.Forms.Button
    Friend WithEvents txtAccountCode As System.Windows.Forms.TextBox
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AccountLookup))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdAccountLookup = New System.Windows.Forms.Button
        Me.txtAccountCode = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        ' 
        ' cmdAccountLookup
        ' 
        Me.cmdAccountLookup.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAccountLookup.CausesValidation = True
        Me.cmdAccountLookup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAccountLookup.Enabled = True
        Me.cmdAccountLookup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.cmdAccountLookup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAccountLookup.Location = New System.Drawing.Point(192, 0)
        Me.cmdAccountLookup.Name = "cmdAccountLookup"
        Me.cmdAccountLookup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAccountLookup.Size = New System.Drawing.Size(24, 19)
        Me.cmdAccountLookup.TabIndex = 1
        Me.cmdAccountLookup.TabStop = True
        Me.cmdAccountLookup.Text = "..."
        Me.cmdAccountLookup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.cmdAccountLookup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        ' 
        ' txtAccountCode
        ' 
        Me.txtAccountCode.AcceptsReturn = True
        Me.txtAccountCode.AutoSize = False
        Me.txtAccountCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccountCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtAccountCode.CausesValidation = True
        Me.txtAccountCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccountCode.Enabled = True
        Me.txtAccountCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.txtAccountCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccountCode.HideSelection = True
        Me.txtAccountCode.Location = New System.Drawing.Point(0, 0)
        Me.txtAccountCode.MaxLength = 60
        Me.txtAccountCode.Multiline = False
        Me.txtAccountCode.Name = "txtAccountCode"
        Me.txtAccountCode.ReadOnly = False
        Me.txtAccountCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccountCode.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.txtAccountCode.Size = New System.Drawing.Size(193, 19)
        Me.txtAccountCode.TabIndex = 0
        Me.txtAccountCode.TabStop = True
        Me.txtAccountCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtAccountCode.Visible = True
        ' 
        ' AccountLookup
        ' 
        Me.ClientSize = New System.Drawing.Size(218, 22)
        Me.Controls.Add(Me.cmdAccountLookup)
        Me.Controls.Add(Me.txtAccountCode)
        MyBase.Location = New System.Drawing.Point(0, 0)
        MyBase.Name = "AccountLookup"
        Me.ResumeLayout(False)
    End Sub
#End Region
#Region "Upgrade Support"
    <System.Runtime.InteropServices.ProgId("MouseUpEventArgs_NET.MouseUpEventArgs")> _
    Public NotInheritable Class MouseUpEventArgs
        Inherits System.EventArgs
        Public Button As Integer
        Public Shift As Integer
        Public x As Single
        Public y As Single
        Friend Sub New(ByRef Button As Integer, ByRef Shift As Integer, ByRef x As Single, ByRef y As Single)
            MyBase.New()
            Me.Button = Button
            Me.Shift = Shift
            Me.x = x
            Me.y = y
        End Sub
    End Class
    <System.Runtime.InteropServices.ProgId("MouseMoveEventArgs_NET.MouseMoveEventArgs")> _
    Public NotInheritable Class MouseMoveEventArgs
        Inherits System.EventArgs
        Public Button As Integer
        Public Shift As Integer
        Public x As Single
        Public y As Single
        Friend Sub New(ByRef Button As Integer, ByRef Shift As Integer, ByRef x As Single, ByRef y As Single)
            MyBase.New()
            Me.Button = Button
            Me.Shift = Shift
            Me.x = x
            Me.y = y
        End Sub
    End Class
    <System.Runtime.InteropServices.ProgId("MouseDownEventArgs_NET.MouseDownEventArgs")> _
    Public NotInheritable Class MouseDownEventArgs
        Inherits System.EventArgs
        Public Button As Integer
        Public Shift As Integer
        Public x As Single
        Public y As Single
        Friend Sub New(ByRef Button As Integer, ByRef Shift As Integer, ByRef x As Single, ByRef y As Single)
            MyBase.New()
            Me.Button = Button
            Me.Shift = Shift
            Me.x = x
            Me.y = y
        End Sub
    End Class
    <System.Runtime.InteropServices.ProgId("KeyUpEventArgs_NET.KeyUpEventArgs")> _
    Public NotInheritable Class KeyUpEventArgs
        Inherits System.EventArgs
        Public KeyCode As Integer
        Public Shift As Integer
        Friend Sub New(ByRef KeyCode As Integer, ByRef Shift As Integer)
            MyBase.New()
            Me.KeyCode = KeyCode
            Me.Shift = Shift
        End Sub
    End Class
    <System.Runtime.InteropServices.ProgId("KeyPressEventArgs_NET.KeyPressEventArgs")> _
    Public NotInheritable Class KeyPressEventArgs
        Inherits System.EventArgs
        Public KeyAscii As Integer
        Friend Sub New(ByRef KeyAscii As Integer)
            MyBase.New()
            Me.KeyAscii = KeyAscii
        End Sub
    End Class
    <System.Runtime.InteropServices.ProgId("KeyDownEventArgs_NET.KeyDownEventArgs")> _
    Public NotInheritable Class KeyDownEventArgs
        Inherits System.EventArgs
        Public KeyCode As Integer
        Public Shift As Integer
        Friend Sub New(ByRef KeyCode As Integer, ByRef Shift As Integer)
            MyBase.New()
            Me.KeyCode = KeyCode
            Me.Shift = Shift
        End Sub
    End Class
#End Region
End Class