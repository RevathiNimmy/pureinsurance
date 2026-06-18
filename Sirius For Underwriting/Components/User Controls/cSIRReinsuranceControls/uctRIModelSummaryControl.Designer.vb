<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctRIModelControl
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Friend WithEvents imgIcons As System.Windows.Forms.ImageList
	Friend WithEvents trvRIModel As System.Windows.Forms.TreeView
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctRIModelControl))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.imgIcons = New System.Windows.Forms.ImageList
		Me.trvRIModel = New System.Windows.Forms.TreeView
		Me.SuspendLayout()
		' 
		' imgIcons
		' 
		Me.imgIcons.ImageSize = New System.Drawing.Size(16, 16)
		Me.imgIcons.ImageStream = CType(resources.GetObject("imgIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.imgIcons.TransparentColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.imgIcons.Images.SetKeyName(0, "ClosedFolder")
		Me.imgIcons.Images.SetKeyName(1, "OpenFolder")
		Me.imgIcons.Images.SetKeyName(2, "Priority")
		Me.imgIcons.Images.SetKeyName(3, "Treaty")
		Me.imgIcons.Images.SetKeyName(4, "Party")
		Me.imgIcons.Images.SetKeyName(5, "Notes")
		Me.imgIcons.Images.SetKeyName(6, "Note")
		' 
		' trvRIModel
		' 
		Me.trvRIModel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.trvRIModel.CausesValidation = True
		Me.trvRIModel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.trvRIModel.ImageList = imgIcons
		Me.trvRIModel.Indent = 0
		Me.trvRIModel.LabelEdit = False
		Me.trvRIModel.LabelEdit = True
		Me.trvRIModel.Location = New System.Drawing.Point(12, 16)
		Me.trvRIModel.Name = "trvRIModel"
		Me.trvRIModel.Size = New System.Drawing.Size(325, 233)
		Me.trvRIModel.TabIndex = 0
		' 
		' uctRIModelControl
		' 
		Me.ClientSize = New System.Drawing.Size(395, 276)
		Me.Controls.Add(Me.trvRIModel)
		MyBase.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		MyBase.Location = New System.Drawing.Point(0, 0)
		MyBase.Name = "uctRIModelControl"
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class