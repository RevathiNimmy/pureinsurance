<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
	Public WithEvents cmdSend As System.Windows.Forms.Button
	Public WithEvents txtFile As System.Windows.Forms.TextBox
	Public WithEvents txtNote As System.Windows.Forms.TextBox
	Public WithEvents txtSubject As System.Windows.Forms.TextBox
	Public WithEvents txtTo As System.Windows.Forms.TextBox
	Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdSend = New System.Windows.Forms.Button
		Me.txtFile = New System.Windows.Forms.TextBox
		Me.txtNote = New System.Windows.Forms.TextBox
		Me.txtSubject = New System.Windows.Forms.TextBox
		Me.txtTo = New System.Windows.Forms.TextBox
		Me.Label4 = New System.Windows.Forms.Label
		Me.Label3 = New System.Windows.Forms.Label
		Me.Label2 = New System.Windows.Forms.Label
		Me.Label1 = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' cmdSend
		' 
		Me.cmdSend.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSend.CausesValidation = True
		Me.cmdSend.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSend.Enabled = True
		Me.cmdSend.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdSend.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSend.Location = New System.Drawing.Point(288, 192)
		Me.cmdSend.Name = "cmdSend"
		Me.cmdSend.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSend.Size = New System.Drawing.Size(81, 25)
		Me.cmdSend.TabIndex = 8
		Me.cmdSend.TabStop = True
		Me.cmdSend.Text = "&Send"
		Me.cmdSend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSend.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtFile
		' 
		Me.txtFile.AcceptsReturn = True
		Me.txtFile.AutoSize = False
		Me.txtFile.BackColor = System.Drawing.SystemColors.Window
		Me.txtFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtFile.CausesValidation = True
		Me.txtFile.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtFile.Enabled = True
		Me.txtFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtFile.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtFile.HideSelection = True
		Me.txtFile.Location = New System.Drawing.Point(64, 144)
		Me.txtFile.MaxLength = 0
		Me.txtFile.Multiline = False
		Me.txtFile.Name = "txtFile"
		Me.txtFile.ReadOnly = False
		Me.txtFile.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtFile.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtFile.Size = New System.Drawing.Size(305, 25)
		Me.txtFile.TabIndex = 6
		Me.txtFile.TabStop = True
		Me.txtFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtFile.Visible = True
		' 
		' txtNote
		' 
		Me.txtNote.AcceptsReturn = True
		Me.txtNote.AutoSize = False
		Me.txtNote.BackColor = System.Drawing.SystemColors.Window
		Me.txtNote.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtNote.CausesValidation = True
		Me.txtNote.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtNote.Enabled = True
		Me.txtNote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtNote.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtNote.HideSelection = True
		Me.txtNote.Location = New System.Drawing.Point(64, 72)
		Me.txtNote.MaxLength = 0
		Me.txtNote.Multiline = False
		Me.txtNote.Name = "txtNote"
		Me.txtNote.ReadOnly = False
		Me.txtNote.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtNote.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtNote.Size = New System.Drawing.Size(305, 65)
		Me.txtNote.TabIndex = 4
		Me.txtNote.TabStop = True
		Me.txtNote.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtNote.Visible = True
		' 
		' txtSubject
		' 
		Me.txtSubject.AcceptsReturn = True
		Me.txtSubject.AutoSize = False
		Me.txtSubject.BackColor = System.Drawing.SystemColors.Window
		Me.txtSubject.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtSubject.CausesValidation = True
		Me.txtSubject.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtSubject.Enabled = True
		Me.txtSubject.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtSubject.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtSubject.HideSelection = True
		Me.txtSubject.Location = New System.Drawing.Point(64, 40)
		Me.txtSubject.MaxLength = 0
		Me.txtSubject.Multiline = False
		Me.txtSubject.Name = "txtSubject"
		Me.txtSubject.ReadOnly = False
		Me.txtSubject.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtSubject.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtSubject.Size = New System.Drawing.Size(305, 25)
		Me.txtSubject.TabIndex = 2
		Me.txtSubject.TabStop = True
		Me.txtSubject.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtSubject.Visible = True
		' 
		' txtTo
		' 
		Me.txtTo.AcceptsReturn = True
		Me.txtTo.AutoSize = False
		Me.txtTo.BackColor = System.Drawing.SystemColors.Window
		Me.txtTo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtTo.CausesValidation = True
		Me.txtTo.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtTo.Enabled = True
		Me.txtTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtTo.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtTo.HideSelection = True
		Me.txtTo.Location = New System.Drawing.Point(64, 8)
		Me.txtTo.MaxLength = 0
		Me.txtTo.Multiline = False
		Me.txtTo.Name = "txtTo"
		Me.txtTo.ReadOnly = False
		Me.txtTo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTo.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTo.Size = New System.Drawing.Size(305, 25)
		Me.txtTo.TabIndex = 0
		Me.txtTo.TabStop = True
		Me.txtTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTo.Visible = True
		' 
		' Label4
		' 
		Me.Label4.AutoSize = False
		Me.Label4.BackColor = System.Drawing.SystemColors.Control
		Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label4.Enabled = True
		Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label4.Location = New System.Drawing.Point(8, 144)
		Me.Label4.Name = "Label4"
		Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label4.Size = New System.Drawing.Size(49, 25)
		Me.Label4.TabIndex = 7
		Me.Label4.Text = "File"
		Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label4.UseMnemonic = True
		Me.Label4.Visible = True
		' 
		' Label3
		' 
		Me.Label3.AutoSize = False
		Me.Label3.BackColor = System.Drawing.SystemColors.Control
		Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label3.Enabled = True
		Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label3.Location = New System.Drawing.Point(8, 72)
		Me.Label3.Name = "Label3"
		Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label3.Size = New System.Drawing.Size(49, 25)
		Me.Label3.TabIndex = 5
		Me.Label3.Text = "Text"
		Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label3.UseMnemonic = True
		Me.Label3.Visible = True
		' 
		' Label2
		' 
		Me.Label2.AutoSize = False
		Me.Label2.BackColor = System.Drawing.SystemColors.Control
		Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label2.Enabled = True
		Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label2.Location = New System.Drawing.Point(8, 40)
		Me.Label2.Name = "Label2"
		Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label2.Size = New System.Drawing.Size(49, 25)
		Me.Label2.TabIndex = 3
		Me.Label2.Text = "Subject"
		Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label2.UseMnemonic = True
		Me.Label2.Visible = True
		' 
		' Label1
		' 
		Me.Label1.AutoSize = False
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.Enabled = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Location = New System.Drawing.Point(8, 8)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(49, 25)
		Me.Label1.TabIndex = 1
		Me.Label1.Text = "To"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' Form1
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(381, 228)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdSend)
		Me.Controls.Add(Me.txtFile)
		Me.Controls.Add(Me.txtNote)
		Me.Controls.Add(Me.txtSubject)
		Me.Controls.Add(Me.txtTo)
		Me.Controls.Add(Me.Label4)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.Label1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "Form1"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "Form1"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class