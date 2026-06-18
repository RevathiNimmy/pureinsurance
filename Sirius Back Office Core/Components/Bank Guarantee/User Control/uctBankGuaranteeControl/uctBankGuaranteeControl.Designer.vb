<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctBankGuarenteeControl
#Region "Windows Form Designer generated code "
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
			If Not (components Is Nothing) Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Friend WithEvents cmdView As System.Windows.Forms.Button
	Friend WithEvents cmdInvoke As System.Windows.Forms.Button
	Friend WithEvents txtPartyName As System.Windows.Forms.TextBox
	Friend WithEvents txtPartyCode As System.Windows.Forms.TextBox
	Friend WithEvents cmdBGDelete As System.Windows.Forms.Button
	Friend WithEvents cmdBGEdit As System.Windows.Forms.Button
	Friend WithEvents cmdBGAdd As System.Windows.Forms.Button
	Friend WithEvents uctAnchor As uSIRCommonControls.uctAnchor
	Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
	Friend WithEvents lvwBGList As System.Windows.Forms.ListView
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents lblPartyCode As System.Windows.Forms.Label
	Friend WithEvents fraBankDetails As System.Windows.Forms.GroupBox
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctBankGuarenteeControl))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.fraBankDetails = New System.Windows.Forms.GroupBox
        Me.cmdView = New System.Windows.Forms.Button
        Me.cmdInvoke = New System.Windows.Forms.Button
        Me.txtPartyName = New System.Windows.Forms.TextBox
        Me.txtPartyCode = New System.Windows.Forms.TextBox
        Me.cmdBGDelete = New System.Windows.Forms.Button
        Me.cmdBGEdit = New System.Windows.Forms.Button
        Me.cmdBGAdd = New System.Windows.Forms.Button
        Me.uctAnchor = New uSIRCommonControls.uctAnchor
        Me.lvwBGList = New System.Windows.Forms.ListView
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblPartyCode = New System.Windows.Forms.Label
        Me.fraBankDetails.SuspendLayout()
        Me.SuspendLayout()
        '
        'fraBankDetails
        '
        Me.fraBankDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraBankDetails.Controls.Add(Me.cmdView)
        Me.fraBankDetails.Controls.Add(Me.cmdInvoke)
        Me.fraBankDetails.Controls.Add(Me.txtPartyName)
        Me.fraBankDetails.Controls.Add(Me.txtPartyCode)
        Me.fraBankDetails.Controls.Add(Me.cmdBGDelete)
        Me.fraBankDetails.Controls.Add(Me.cmdBGEdit)
        Me.fraBankDetails.Controls.Add(Me.cmdBGAdd)
        Me.fraBankDetails.Controls.Add(Me.uctAnchor)
        Me.fraBankDetails.Controls.Add(Me.lvwBGList)
        Me.fraBankDetails.Controls.Add(Me.Label1)
        Me.fraBankDetails.Controls.Add(Me.lblPartyCode)
        Me.fraBankDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraBankDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraBankDetails.Location = New System.Drawing.Point(0, 0)
        Me.fraBankDetails.Name = "fraBankDetails"
        Me.fraBankDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraBankDetails.Size = New System.Drawing.Size(749, 451)
        Me.fraBankDetails.TabIndex = 0
        Me.fraBankDetails.TabStop = False
        Me.fraBankDetails.Text = "Bank Guarentee"
        '
        'cmdView
        '
        Me.cmdView.BackColor = System.Drawing.SystemColors.Control
        Me.cmdView.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdView.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdView.Location = New System.Drawing.Point(300, 420)
        Me.cmdView.Name = "cmdView"
        Me.cmdView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdView.Size = New System.Drawing.Size(95, 25)
        Me.cmdView.TabIndex = 10
        Me.cmdView.Text = "&View"
        Me.cmdView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdView.UseVisualStyleBackColor = False
        '
        'cmdInvoke
        '
        Me.cmdInvoke.BackColor = System.Drawing.SystemColors.Control
        Me.cmdInvoke.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdInvoke.Enabled = False
        Me.cmdInvoke.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdInvoke.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdInvoke.Location = New System.Drawing.Point(632, 420)
        Me.cmdInvoke.Name = "cmdInvoke"
        Me.cmdInvoke.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdInvoke.Size = New System.Drawing.Size(95, 25)
        Me.cmdInvoke.TabIndex = 9
        Me.cmdInvoke.Text = "&Invoke "
        Me.cmdInvoke.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdInvoke.UseVisualStyleBackColor = False
        '
        'txtPartyName
        '
        Me.txtPartyName.AcceptsReturn = True
        Me.txtPartyName.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtPartyName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPartyName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPartyName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPartyName.Location = New System.Drawing.Point(298, 26)
        Me.txtPartyName.MaxLength = 0
        Me.txtPartyName.Name = "txtPartyName"
        Me.txtPartyName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPartyName.Size = New System.Drawing.Size(265, 20)
        Me.txtPartyName.TabIndex = 8
        '
        'txtPartyCode
        '
        Me.txtPartyCode.AcceptsReturn = True
        Me.txtPartyCode.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtPartyCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPartyCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPartyCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPartyCode.Location = New System.Drawing.Point(80, 26)
        Me.txtPartyCode.MaxLength = 0
        Me.txtPartyCode.Name = "txtPartyCode"
        Me.txtPartyCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPartyCode.Size = New System.Drawing.Size(129, 20)
        Me.txtPartyCode.TabIndex = 6
        '
        'cmdBGDelete
        '
        Me.cmdBGDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBGDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBGDelete.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBGDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBGDelete.Location = New System.Drawing.Point(202, 420)
        Me.cmdBGDelete.Name = "cmdBGDelete"
        Me.cmdBGDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBGDelete.Size = New System.Drawing.Size(95, 25)
        Me.cmdBGDelete.TabIndex = 3
        Me.cmdBGDelete.Text = "&Delete"
        Me.cmdBGDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdBGDelete.UseVisualStyleBackColor = False
        '
        'cmdBGEdit
        '
        Me.cmdBGEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBGEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBGEdit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBGEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBGEdit.Location = New System.Drawing.Point(104, 420)
        Me.cmdBGEdit.Name = "cmdBGEdit"
        Me.cmdBGEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBGEdit.Size = New System.Drawing.Size(95, 25)
        Me.cmdBGEdit.TabIndex = 2
        Me.cmdBGEdit.Text = "&Edit"
        Me.cmdBGEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdBGEdit.UseVisualStyleBackColor = False
        '
        'cmdBGAdd
        '
        Me.cmdBGAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBGAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBGAdd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBGAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBGAdd.Location = New System.Drawing.Point(6, 420)
        Me.cmdBGAdd.Name = "cmdBGAdd"
        Me.cmdBGAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBGAdd.Size = New System.Drawing.Size(95, 25)
        Me.cmdBGAdd.TabIndex = 1
        Me.cmdBGAdd.Text = "&Add"
        Me.cmdBGAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdBGAdd.UseVisualStyleBackColor = False
        '
        'uctAnchor
        '
        Me.uctAnchor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctAnchor.Location = New System.Drawing.Point(486, 322)
        Me.uctAnchor.Name = "uctAnchor"
        Me.uctAnchor.Size = New System.Drawing.Size(44, 16)
        Me.uctAnchor.TabIndex = 11
        Me.uctAnchor.Visible = False
        '
        'lvwBGList
        '
        Me.lvwBGList.BackColor = System.Drawing.SystemColors.Window
        Me.lvwBGList.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwBGList.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwBGList.FullRowSelect = True
        Me.lvwBGList.HideSelection = False
        Me.lvwBGList.LargeImageList = Me.ImageList1
        Me.lvwBGList.Location = New System.Drawing.Point(4, 54)
        Me.lvwBGList.Name = "lvwBGList"
        Me.lvwBGList.Size = New System.Drawing.Size(739, 363)
        Me.lvwBGList.SmallImageList = Me.ImageList1
        Me.lvwBGList.TabIndex = 4
        Me.lvwBGList.UseCompatibleStateImageBehavior = False
        Me.lvwBGList.View = System.Windows.Forms.View.Details
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "add")
        Me.ImageList1.Images.SetKeyName(1, "history")
        Me.ImageList1.Images.SetKeyName(2, "edited")
        Me.ImageList1.Images.SetKeyName(3, "delete")
        Me.ImageList1.Images.SetKeyName(4, "saved")
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(228, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(68, 13)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Party Name :"
        '
        'lblPartyCode
        '
        Me.lblPartyCode.AutoSize = True
        Me.lblPartyCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblPartyCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPartyCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartyCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPartyCode.Location = New System.Drawing.Point(14, 28)
        Me.lblPartyCode.Name = "lblPartyCode"
        Me.lblPartyCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPartyCode.Size = New System.Drawing.Size(65, 13)
        Me.lblPartyCode.TabIndex = 5
        Me.lblPartyCode.Text = "Party Code :"
        '
        'uctBankGuarenteeControl
        '
        Me.Controls.Add(Me.fraBankDetails)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctBankGuarenteeControl"
        Me.Size = New System.Drawing.Size(751, 452)
        Me.fraBankDetails.ResumeLayout(False)
        Me.fraBankDetails.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region 
#Region "Upgrade Support"
	<System.Runtime.InteropServices.ProgId("RefreshBGDetailsEventArgs_NET.RefreshBGDetailsEventArgs")> _
	Public NotInheritable Class RefreshBGDetailsEventArgs
		Inherits System.EventArgs
		Public vGuaranteeDetails As Object
		Public Sub New(ByRef vGuaranteeDetails As Object)
			MyBase.New()
			Me.vGuaranteeDetails = vGuaranteeDetails
		End Sub
	End Class
#End Region 
End Class