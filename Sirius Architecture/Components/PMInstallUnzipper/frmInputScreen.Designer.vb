<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInputScreen
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
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
	Public WithEvents ImageList1 As System.Windows.Forms.ImageList
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents lblMessage As System.Windows.Forms.Label
	Public WithEvents lvwDiskList As System.Windows.Forms.ListView
	Public WithEvents txtFolderPath As System.Windows.Forms.TextBox
	Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents SSTab1 As System.Windows.Forms.TabControl
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInputScreen))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.ImageList1 = New System.Windows.Forms.ImageList
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOk = New System.Windows.Forms.Button
		Me.SSTab1 = New System.Windows.Forms.TabControl
		Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
		Me.lblMessage = New System.Windows.Forms.Label
		Me.lvwDiskList = New System.Windows.Forms.ListView
		Me.txtFolderPath = New System.Windows.Forms.TextBox
		Me.SSTab1.SuspendLayout()
		Me._SSTab1_TabPage0.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' ImageList1
		' 
		Me.ImageList1.ImageSize = New System.Drawing.Size(16, 16)
		Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.ImageList1.Images.SetKeyName(0, "Local")
		Me.ImageList1.Images.SetKeyName(1, "Network")
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(258, 232)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(61, 21)
		Me.cmdCancel.TabIndex = 3
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOk
		' 
		Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOk.CausesValidation = True
		Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOk.Enabled = True
		Me.cmdOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOk.Location = New System.Drawing.Point(194, 232)
		Me.cmdOk.Name = "cmdOk"
		Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOk.Size = New System.Drawing.Size(61, 21)
		Me.cmdOk.TabIndex = 1
		Me.cmdOk.TabStop = True
		Me.cmdOk.Text = "&Ok"
		Me.cmdOk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' SSTab1
		' 
		Me.SSTab1.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.SSTab1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
		Me.SSTab1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.SSTab1.ItemSize = New System.Drawing.Size(102, 18)
		Me.SSTab1.Location = New System.Drawing.Point(8, 8)
		Me.SSTab1.Multiline = True
		Me.SSTab1.Name = "SSTab1"
		Me.SSTab1.Size = New System.Drawing.Size(315, 221)
		Me.SSTab1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.SSTab1.TabIndex = 0
		' 
		' _SSTab1_TabPage0
		' 
		Me._SSTab1_TabPage0.Controls.Add(Me.lblMessage)
		Me._SSTab1_TabPage0.Controls.Add(Me.lvwDiskList)
		Me._SSTab1_TabPage0.Controls.Add(Me.txtFolderPath)
		Me._SSTab1_TabPage0.Text = "Select share path"
		' 
		' lblMessage
		' 
		Me.lblMessage.AutoSize = False
		Me.lblMessage.BackColor = System.Drawing.SystemColors.Control
		Me.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMessage.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMessage.Enabled = True
		Me.lblMessage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMessage.Location = New System.Drawing.Point(13, 16)
		Me.lblMessage.Name = "lblMessage"
		Me.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMessage.Size = New System.Drawing.Size(283, 33)
		Me.lblMessage.TabIndex = 2
		Me.lblMessage.Text = "lblMessage"
		Me.lblMessage.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMessage.UseMnemonic = True
		Me.lblMessage.Visible = True
		' 
		' lvwDiskList
		' 
		Me.lvwDiskList.BackColor = System.Drawing.SystemColors.Window
		Me.lvwDiskList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwDiskList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwDiskList.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwDiskList.FullRowSelect = True
		Me.lvwDiskList.HideSelection = True
		Me.lvwDiskList.LabelEdit = True
		Me.lvwDiskList.LabelWrap = True
		Me.lvwDiskList.LargeImageList = ImageList1
		Me.lvwDiskList.Location = New System.Drawing.Point(12, 76)
		Me.lvwDiskList.Name = "lvwDiskList"
		Me.lvwDiskList.Size = New System.Drawing.Size(285, 105)
		Me.lvwDiskList.SmallImageList = ImageList1
		Me.lvwDiskList.TabIndex = 4
		' 
		' txtFolderPath
		' 
		Me.txtFolderPath.AcceptsReturn = True
		Me.txtFolderPath.AutoSize = False
		Me.txtFolderPath.BackColor = System.Drawing.SystemColors.Window
		Me.txtFolderPath.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtFolderPath.CausesValidation = True
		Me.txtFolderPath.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtFolderPath.Enabled = True
		Me.txtFolderPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtFolderPath.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtFolderPath.HideSelection = True
		Me.txtFolderPath.Location = New System.Drawing.Point(13, 56)
		Me.txtFolderPath.MaxLength = 0
		Me.txtFolderPath.Multiline = False
		Me.txtFolderPath.Name = "txtFolderPath"
		Me.txtFolderPath.ReadOnly = False
		Me.txtFolderPath.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtFolderPath.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtFolderPath.Size = New System.Drawing.Size(283, 19)
		Me.txtFolderPath.TabIndex = 5
		Me.txtFolderPath.TabStop = True
		Me.txtFolderPath.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtFolderPath.Visible = True
		' 
		' frmInputScreen
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(324, 260)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOk)
		Me.Controls.Add(Me.SSTab1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmInputScreen.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmInputScreen"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "PMInstallUnzipper"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.SSTab1, 1)
		Me.listViewHelper1.SetItemClickMethod(Me.lvwDiskList, "lvwDiskList_ItemClick")
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwDiskList, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.SSTab1.ResumeLayout(False)
		Me._SSTab1_TabPage0.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class