<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctCLMPerilRT
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Friend WithEvents cmdPerilAdd As System.Windows.Forms.Button
	Friend WithEvents cmdPerilEdit As System.Windows.Forms.Button
	Friend WithEvents cmdPerilDelete As System.Windows.Forms.Button
	Friend WithEvents fraButtons As System.Windows.Forms.Panel
	Friend WithEvents lvwPerils As System.Windows.Forms.ListView
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctCLMPerilRT))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.fraButtons = New System.Windows.Forms.Panel
		Me.cmdPerilAdd = New System.Windows.Forms.Button
		Me.cmdPerilEdit = New System.Windows.Forms.Button
		Me.cmdPerilDelete = New System.Windows.Forms.Button
		Me.lvwPerils = New System.Windows.Forms.ListView
		Me.fraButtons.SuspendLayout()
		Me.SuspendLayout()
        ' 
		' fraButtons
		' 
		Me.fraButtons.BackColor = System.Drawing.SystemColors.Control
		Me.fraButtons.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.fraButtons.Controls.Add(Me.cmdPerilAdd)
		Me.fraButtons.Controls.Add(Me.cmdPerilEdit)
		Me.fraButtons.Controls.Add(Me.cmdPerilDelete)
		Me.fraButtons.Cursor = System.Windows.Forms.Cursors.Default
		Me.fraButtons.Enabled = True
		Me.fraButtons.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraButtons.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraButtons.Location = New System.Drawing.Point(549, 1)
		Me.fraButtons.Name = "fraButtons"
		Me.fraButtons.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraButtons.Size = New System.Drawing.Size(85, 95)
		Me.fraButtons.TabIndex = 0
		Me.fraButtons.Text = "Frame1"
		Me.fraButtons.Visible = True
		' 
		' cmdPerilAdd
		' 
		Me.cmdPerilAdd.BackColor = System.Drawing.SystemColors.Control
		Me.cmdPerilAdd.CausesValidation = True
		Me.cmdPerilAdd.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdPerilAdd.Enabled = True
		Me.cmdPerilAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdPerilAdd.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdPerilAdd.Location = New System.Drawing.Point(6, 1)
		Me.cmdPerilAdd.Name = "cmdPerilAdd"
		Me.cmdPerilAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdPerilAdd.Size = New System.Drawing.Size(73, 22)
		Me.cmdPerilAdd.TabIndex = 3
		Me.cmdPerilAdd.TabStop = True
		Me.cmdPerilAdd.Text = "&Add"
		Me.cmdPerilAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdPerilAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdPerilEdit
		' 
		Me.cmdPerilEdit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdPerilEdit.CausesValidation = True
		Me.cmdPerilEdit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdPerilEdit.Enabled = False
		Me.cmdPerilEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdPerilEdit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdPerilEdit.Location = New System.Drawing.Point(6, 31)
		Me.cmdPerilEdit.Name = "cmdPerilEdit"
		Me.cmdPerilEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdPerilEdit.Size = New System.Drawing.Size(73, 22)
		Me.cmdPerilEdit.TabIndex = 2
		Me.cmdPerilEdit.TabStop = True
		Me.cmdPerilEdit.Text = "&Edit"
		Me.cmdPerilEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdPerilEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdPerilDelete
		' 
		Me.cmdPerilDelete.BackColor = System.Drawing.SystemColors.Control
		Me.cmdPerilDelete.CausesValidation = True
		Me.cmdPerilDelete.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdPerilDelete.Enabled = False
		Me.cmdPerilDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdPerilDelete.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdPerilDelete.Location = New System.Drawing.Point(6, 61)
		Me.cmdPerilDelete.Name = "cmdPerilDelete"
		Me.cmdPerilDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdPerilDelete.Size = New System.Drawing.Size(73, 22)
		Me.cmdPerilDelete.TabIndex = 1
		Me.cmdPerilDelete.TabStop = True
		Me.cmdPerilDelete.Text = "&Delete"
		Me.cmdPerilDelete.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdPerilDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lvwPerils
		' 
		Me.lvwPerils.BackColor = System.Drawing.SystemColors.Window
		Me.lvwPerils.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lvwPerils.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwPerils.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwPerils.FullRowSelect = True
		Me.lvwPerils.HideSelection = True
		Me.lvwPerils.LabelEdit = False
		Me.lvwPerils.LabelWrap = True
		Me.lvwPerils.Location = New System.Drawing.Point(0, 0)
		Me.lvwPerils.Name = "lvwPerils"
		Me.lvwPerils.Size = New System.Drawing.Size(545, 225)
		Me.lvwPerils.TabIndex = 4
		Me.lvwPerils.View = System.Windows.Forms.View.Details
		' 
		' uctCLMPerilRT
		' 
		Me.ClientSize = New System.Drawing.Size(636, 224)
		Me.Controls.Add(Me.fraButtons)
		Me.Controls.Add(Me.lvwPerils)
		MyBase.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		MyBase.Location = New System.Drawing.Point(0, 0)
		MyBase.Name = "uctCLMPerilRT"
        Me.fraButtons.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
#Region "Upgrade Support"
	<System.Runtime.InteropServices.ProgId("OnControlGotFocusEventArgs_NET.OnControlGotFocusEventArgs")> _
	Public NotInheritable Class OnControlGotFocusEventArgs
		Inherits System.EventArgs
		Public sControlName As String = ""
		Public Sub New(ByRef sControlName As String)
			MyBase.New()
			Me.sControlName = sControlName
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("DeleteClickEventArgs_NET.DeleteClickEventArgs")> _
	Public NotInheritable Class DeleteClickEventArgs
		Inherits System.EventArgs
		Public vKeyArray As Object
		Public Sub New(ByRef vKeyArray As Object)
			MyBase.New()
			Me.vKeyArray = vKeyArray
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("EditClickEventArgs_NET.EditClickEventArgs")> _
	Public NotInheritable Class EditClickEventArgs
		Inherits System.EventArgs
		Public vKeyArray As Object
		Public Sub New(ByRef vKeyArray As Object)
			MyBase.New()
			Me.vKeyArray = vKeyArray
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("AddClickEventArgs_NET.AddClickEventArgs")> _
	Public NotInheritable Class AddClickEventArgs
		Inherits System.EventArgs
		Public vKeyArray As Object
		Public Sub New(ByRef vKeyArray As Object)
			MyBase.New()
			Me.vKeyArray = vKeyArray
		End Sub
	End Class
#End Region 
End Class