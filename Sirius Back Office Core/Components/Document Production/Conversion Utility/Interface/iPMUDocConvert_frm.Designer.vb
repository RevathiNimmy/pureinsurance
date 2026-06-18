<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class iPMUDocConvert_frm
#Region "Windows Form Designer generated code "
    <System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        Form_Initialize_renamed()
    End Sub
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
        If Disposing Then
            If Not components Is Nothing Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(Disposing)
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents List1 As System.Windows.Forms.ListBox
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Public WithEvents ProgressBar1 As AxComctlLib.AxProgressBar
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdConvert As System.Windows.Forms.Button
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(iPMUDocConvert_frm))
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.List1 = New System.Windows.Forms.ListBox
        Me.ProgressBar1 = New AxComctlLib.AxProgressBar
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdConvert = New System.Windows.Forms.Button
        Me.Frame1.SuspendLayout()
        Me.SuspendLayout()
        Me.ToolTip1.Active = True
        CType(Me.ProgressBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Text = "Document Conversion Utility"
        Me.ClientSize = New System.Drawing.Size(497, 335)
        Me.Location = New System.Drawing.Point(4, 23)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.ControlBox = True
        Me.Enabled = True
        Me.KeyPreview = False
        Me.MaximizeBox = True
        Me.MinimizeBox = True
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = True
        Me.HelpButton = False
        Me.WindowState = System.Windows.Forms.FormWindowState.Normal
        Me.Name = "frmInterface"
        Me.Frame1.Text = "Templates on System"
        Me.Frame1.Size = New System.Drawing.Size(496, 277)
        Me.Frame1.Location = New System.Drawing.Point(0, 0)
        Me.Frame1.TabIndex = 3
        Me.Frame1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Enabled = True
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Visible = True
        Me.Frame1.Name = "Frame1"
        Me.List1.Size = New System.Drawing.Size(478, 254)
        Me.List1.Location = New System.Drawing.Point(9, 18)
        Me.List1.TabIndex = 4
        Me.List1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.List1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.List1.BackColor = System.Drawing.SystemColors.Window
        Me.List1.CausesValidation = True
        Me.List1.Enabled = True
        Me.List1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.List1.IntegralHeight = True
        Me.List1.Cursor = System.Windows.Forms.Cursors.Default
        Me.List1.SelectionMode = System.Windows.Forms.SelectionMode.One
        Me.List1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.List1.Sorted = False
        Me.List1.TabStop = True
        Me.List1.Visible = True
        Me.List1.MultiColumn = False
        Me.List1.Name = "List1"
        ProgressBar1.OcxState = CType(resources.GetObject("ProgressBar1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.ProgressBar1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ProgressBar1.Size = New System.Drawing.Size(497, 19)
        Me.ProgressBar1.Location = New System.Drawing.Point(0, 316)
        Me.ProgressBar1.TabIndex = 2
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.cmdExit.Text = "Exit"
        Me.cmdExit.Size = New System.Drawing.Size(97, 28)
        Me.cmdExit.Location = New System.Drawing.Point(393, 282)
        Me.cmdExit.TabIndex = 1
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.CausesValidation = True
        Me.cmdExit.Enabled = True
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.TabStop = True
        Me.cmdExit.Name = "cmdExit"
        Me.cmdConvert.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.cmdConvert.Text = "Start Conversion"
        Me.cmdConvert.Size = New System.Drawing.Size(97, 28)
        Me.cmdConvert.Location = New System.Drawing.Point(294, 282)
        Me.cmdConvert.TabIndex = 0
        Me.cmdConvert.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdConvert.BackColor = System.Drawing.SystemColors.Control
        Me.cmdConvert.CausesValidation = True
        Me.cmdConvert.Enabled = True
        Me.cmdConvert.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdConvert.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdConvert.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdConvert.TabStop = True
        Me.cmdConvert.Name = "cmdConvert"
        Me.Controls.Add(Frame1)
        Me.Controls.Add(ProgressBar1)
        Me.Controls.Add(cmdExit)
        Me.Controls.Add(cmdConvert)
        Me.Frame1.Controls.Add(List1)
        CType(Me.ProgressBar1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Frame1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub
#End Region
End Class