<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BankAccount
#Region "Windows Form Designer generated code "
    'developer guide no.211
    'Friend Sub New()
    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
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
    Friend WithEvents cboLookup As System.Windows.Forms.ComboBox
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BankAccount))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cboLookup = New System.Windows.Forms.ComboBox
        Me.SuspendLayout()
        ' 
        ' cboLookup
        ' 
        Me.cboLookup.BackColor = System.Drawing.SystemColors.Window
        Me.cboLookup.CausesValidation = True
        Me.cboLookup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboLookup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLookup.Enabled = True
        Me.cboLookup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.cboLookup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboLookup.IntegralHeight = True
        Me.cboLookup.Location = New System.Drawing.Point(0, 0)
        Me.cboLookup.Name = "cboLookup"
        Me.cboLookup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboLookup.Size = New System.Drawing.Size(153, 21)
        Me.cboLookup.Sorted = False
        Me.cboLookup.TabIndex = 0
        Me.cboLookup.TabStop = True
        Me.cboLookup.Visible = True
        ' 
        ' BankAccount
        ' 
        Me.ClientSize = New System.Drawing.Size(153, 21)
        Me.Controls.Add(Me.cboLookup)
        MyBase.Location = New System.Drawing.Point(0, 0)
        MyBase.Name = "BankAccount"
        Me.ResumeLayout(False)
    End Sub
#End Region
End Class