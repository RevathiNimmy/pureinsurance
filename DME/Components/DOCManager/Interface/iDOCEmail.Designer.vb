<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEmail
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
   
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents txtTo As System.Windows.Forms.TextBox
	Public WithEvents txtSubject As System.Windows.Forms.TextBox
	Public WithEvents txtNote As System.Windows.Forms.TextBox
	Public WithEvents cmdSend As System.Windows.Forms.Button
	Public WithEvents txtFile As System.Windows.Forms.TextBox
	Public WithEvents cmdTo As System.Windows.Forms.Button
	Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents SSTab1 As System.Windows.Forms.TabControl
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEmail))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.cmdExit = New System.Windows.Forms.Button
        Me.txtTo = New System.Windows.Forms.TextBox
        Me.txtSubject = New System.Windows.Forms.TextBox
        Me.txtNote = New System.Windows.Forms.TextBox
        Me.cmdSend = New System.Windows.Forms.Button
        Me.txtFile = New System.Windows.Forms.TextBox
        Me.cmdTo = New System.Windows.Forms.Button
        'Me.AxMAPIMessages1 = New AxMSMAPI.AxMAPIMessages
        'Me.MAPIMessages1 = New AxMSMAPI.AxMAPIMessages
        'Me.MAPISession1 = New AxMSMAPI.AxMAPISession
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        'CType(Me.AxMAPIMessages1, System.ComponentModel.ISupportInitialize).BeginInit()
        'CType(Me.MAPIMessages1, System.ComponentModel.ISupportInitialize).BeginInit()
        'CType(Me.MAPISession1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(154, 18)
        Me.SSTab1.Location = New System.Drawing.Point(8, 16)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(469, 397)
        Me.SSTab1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.SSTab1.TabIndex = 0
        Me.SSTab1.TabStop = False
        '
        '_SSTab1_TabPage0
        '
        ' Me._SSTab1_TabPage0.Controls.Add(Me.AxMAPIMessages1)
        Me._SSTab1_TabPage0.Controls.Add(Me.Label1)
        Me._SSTab1_TabPage0.Controls.Add(Me.Label2)
        Me._SSTab1_TabPage0.Controls.Add(Me.Label4)
        Me._SSTab1_TabPage0.Controls.Add(Me.cmdExit)
        Me._SSTab1_TabPage0.Controls.Add(Me.txtTo)
        Me._SSTab1_TabPage0.Controls.Add(Me.txtSubject)
        Me._SSTab1_TabPage0.Controls.Add(Me.txtNote)
        Me._SSTab1_TabPage0.Controls.Add(Me.cmdSend)
        Me._SSTab1_TabPage0.Controls.Add(Me.txtFile)
        Me._SSTab1_TabPage0.Controls.Add(Me.cmdTo)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(461, 371)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "1 -Email"
        Me._SSTab1_TabPage0.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(16, 76)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(49, 17)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "To:"
        Me.Label1.Visible = False
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(16, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(51, 17)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Subject:"
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(16, 60)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(73, 17)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Attachment:"
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Location = New System.Drawing.Point(376, 340)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(73, 25)
        Me.cmdExit.TabIndex = 6
        Me.cmdExit.Text = "&Exit"
        Me.cmdExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'txtTo
        '
        Me.txtTo.AcceptsReturn = True
        Me.txtTo.BackColor = System.Drawing.SystemColors.Window
        Me.txtTo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTo.Location = New System.Drawing.Point(104, 12)
        Me.txtTo.MaxLength = 0
        Me.txtTo.Name = "txtTo"
        Me.txtTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTo.Size = New System.Drawing.Size(345, 20)
        Me.txtTo.TabIndex = 1
        '
        'txtSubject
        '
        Me.txtSubject.AcceptsReturn = True
        Me.txtSubject.BackColor = System.Drawing.SystemColors.Window
        Me.txtSubject.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSubject.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSubject.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSubject.Location = New System.Drawing.Point(104, 36)
        Me.txtSubject.MaxLength = 0
        Me.txtSubject.Name = "txtSubject"
        Me.txtSubject.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSubject.Size = New System.Drawing.Size(345, 20)
        Me.txtSubject.TabIndex = 2
        '
        'txtNote
        '
        Me.txtNote.AcceptsReturn = True
        Me.txtNote.BackColor = System.Drawing.SystemColors.Window
        Me.txtNote.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNote.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNote.Location = New System.Drawing.Point(16, 92)
        Me.txtNote.MaxLength = 0
        Me.txtNote.Multiline = True
        Me.txtNote.Name = "txtNote"
        Me.txtNote.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNote.Size = New System.Drawing.Size(433, 233)
        Me.txtNote.TabIndex = 4
        '
        'cmdSend
        '
        Me.cmdSend.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSend.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSend.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSend.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSend.Location = New System.Drawing.Point(280, 340)
        Me.cmdSend.Name = "cmdSend"
        Me.cmdSend.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSend.Size = New System.Drawing.Size(81, 25)
        Me.cmdSend.TabIndex = 5
        Me.cmdSend.Text = "&Send"
        Me.cmdSend.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSend.UseVisualStyleBackColor = False
        '
        'txtFile
        '
        Me.txtFile.AcceptsReturn = True
        Me.txtFile.BackColor = System.Drawing.SystemColors.Window
        Me.txtFile.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFile.Enabled = False
        Me.txtFile.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFile.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFile.Location = New System.Drawing.Point(104, 60)
        Me.txtFile.MaxLength = 0
        Me.txtFile.Name = "txtFile"
        Me.txtFile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFile.Size = New System.Drawing.Size(345, 21)
        Me.txtFile.TabIndex = 3
        Me.txtFile.Text = " "
        '
        'cmdTo
        '
        Me.cmdTo.BackColor = System.Drawing.SystemColors.Control
        Me.cmdTo.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdTo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdTo.Location = New System.Drawing.Point(16, 12)
        Me.cmdTo.Name = "cmdTo"
        Me.cmdTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdTo.Size = New System.Drawing.Size(49, 25)
        Me.cmdTo.TabIndex = 10
        Me.cmdTo.Text = "&To"
        Me.cmdTo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdTo.UseVisualStyleBackColor = False
        '
        'AxMAPIMessages1
        '
        'Me.AxMAPIMessages1.Enabled = True
        'Me.AxMAPIMessages1.Location = New System.Drawing.Point(19, 361)
        'Me.AxMAPIMessages1.Name = "AxMAPIMessages1"
        'Me.AxMAPIMessages1.OcxState = CType(resources.GetObject("AxMAPIMessages1.OcxState"), System.Windows.Forms.AxHost.State)
        'Me.AxMAPIMessages1.Size = New System.Drawing.Size(38, 38)
        'Me.AxMAPIMessages1.TabIndex = 1
        '
        'MAPIMessages1
        '
        'Me.MAPIMessages1.Enabled = True
        'Me.MAPIMessages1.Location = New System.Drawing.Point(155, 416)
        'Me.MAPIMessages1.Name = "MAPIMessages1"
        'Me.MAPIMessages1.OcxState = CType(resources.GetObject("MAPIMessages1.OcxState"), System.Windows.Forms.AxHost.State)
        'Me.MAPIMessages1.Size = New System.Drawing.Size(38, 38)
        'Me.MAPIMessages1.TabIndex = 1
        '
        'MAPISession1
        '
        'Me.MAPISession1.Enabled = True
        'Me.MAPISession1.Location = New System.Drawing.Point(100, 415)
        'Me.MAPISession1.Name = "MAPISession1"
        'Me.MAPISession1.OcxState = CType(resources.GetObject("MAPISession1.OcxState"), System.Windows.Forms.AxHost.State)
        'Me.MAPISession1.Size = New System.Drawing.Size(38, 38)
        'Me.MAPISession1.TabIndex = 2
        '
        'frmEmail
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(500, 441)
        'Me.Controls.Add(Me.MAPISession1)
        'Me.Controls.Add(Me.MAPIMessages1)
        Me.Controls.Add(Me.SSTab1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmEmail"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Documaster Email"
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me._SSTab1_TabPage0.PerformLayout()
        'CType(Me.AxMAPIMessages1, System.ComponentModel.ISupportInitialize).EndInit()
        'CType(Me.MAPIMessages1, System.ComponentModel.ISupportInitialize).EndInit()
        'CType(Me.MAPISession1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    'Friend WithEvents AxMAPIMessages1 As AxMSMAPI.AxMAPIMessages
    'Friend WithEvents MAPIMessages1 As AxMSMAPI.AxMAPIMessages
    'Friend WithEvents MAPISession1 As AxMSMAPI.AxMAPISession
#End Region
End Class