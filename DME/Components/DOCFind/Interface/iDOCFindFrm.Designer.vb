<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		Form_Initialize_Renamed()
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
    Public WithEvents cmbMax As System.Windows.Forms.ComboBox
	Public WithEvents txtStartFolder As System.Windows.Forms.TextBox
	Public WithEvents txtDocName As System.Windows.Forms.TextBox
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdFindNow As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.cmbMax = New System.Windows.Forms.ComboBox
        Me.txtStartFolder = New System.Windows.Forms.TextBox
        Me.txtDocName = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdFindNow = New System.Windows.Forms.Button
        'Me.aniFind = New AxMSComCtl2.AxAnimation
        Me.Frame1.SuspendLayout()
        ' CType(Me.aniFind, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.cmbMax)
        Me.Frame1.Controls.Add(Me.txtStartFolder)
        Me.Frame1.Controls.Add(Me.txtDocName)
        Me.Frame1.Controls.Add(Me.Label3)
        Me.Frame1.Controls.Add(Me.Label2)
        Me.Frame1.Controls.Add(Me.Label1)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(8, 8)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(273, 110)
        Me.Frame1.TabIndex = 4
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Name and Location"
        '
        'cmbMax
        '
        Me.cmbMax.BackColor = System.Drawing.SystemColors.Window
        Me.cmbMax.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbMax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbMax.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbMax.Location = New System.Drawing.Point(96, 72)
        Me.cmbMax.Name = "cmbMax"
        Me.cmbMax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbMax.Size = New System.Drawing.Size(49, 21)
        Me.cmbMax.TabIndex = 10
        '
        'txtStartFolder
        '
        Me.txtStartFolder.AcceptsReturn = True
        Me.txtStartFolder.BackColor = System.Drawing.SystemColors.Control
        Me.txtStartFolder.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStartFolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStartFolder.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStartFolder.Location = New System.Drawing.Point(96, 48)
        Me.txtStartFolder.MaxLength = 0
        Me.txtStartFolder.Name = "txtStartFolder"
        Me.txtStartFolder.ReadOnly = True
        Me.txtStartFolder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStartFolder.Size = New System.Drawing.Size(161, 19)
        Me.txtStartFolder.TabIndex = 7
        '
        'txtDocName
        '
        Me.txtDocName.AcceptsReturn = True
        Me.txtDocName.BackColor = System.Drawing.SystemColors.Window
        Me.txtDocName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDocName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDocName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDocName.Location = New System.Drawing.Point(96, 24)
        Me.txtDocName.MaxLength = 6
        Me.txtDocName.Name = "txtDocName"
        Me.txtDocName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDocName.Size = New System.Drawing.Size(161, 19)
        Me.txtDocName.TabIndex = 0
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(16, 74)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(65, 25)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Max returned"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(16, 26)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(57, 17)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Named"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(16, 49)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(41, 25)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Look in"
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(288, 128)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 3
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(200, 128)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdFindNow
        '
        Me.cmdFindNow.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindNow.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindNow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindNow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindNow.Location = New System.Drawing.Point(112, 128)
        Me.cmdFindNow.Name = "cmdFindNow"
        Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindNow.Size = New System.Drawing.Size(73, 22)
        Me.cmdFindNow.TabIndex = 1
        Me.cmdFindNow.Text = "&Find Now"
        Me.cmdFindNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindNow.UseVisualStyleBackColor = False
        '
        'aniFind
        '
        'Me.aniFind.Location = New System.Drawing.Point(287, 32)
        'Me.aniFind.Name = "aniFind"
        'Me.aniFind.OcxState = CType(resources.GetObject("aniFind.OcxState"), System.Windows.Forms.AxHost.State)
        'Me.aniFind.Size = New System.Drawing.Size(75, 50)
        'Me.aniFind.TabIndex = 5
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(370, 159)
        'Me.Controls.Add(Me.aniFind)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdFindNow)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Find Document"
        Me.Frame1.ResumeLayout(False)
        'CType(Me.aniFind, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    ' Friend WithEvents aniFind As AxMSComCtl2.AxAnimation
#End Region
End Class