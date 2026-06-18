<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSelectPeril
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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblClaimNumber As System.Windows.Forms.Label
	Public WithEvents imglImages As System.Windows.Forms.ImageList
	Public WithEvents lvwSelectPeril As System.Windows.Forms.ListView
	Public WithEvents txtClaimNumber As System.Windows.Forms.TextBox
	Public WithEvents cmdSelect As System.Windows.Forms.Button
	Public WithEvents chkCloseClaim As System.Windows.Forms.CheckBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSelectPeril))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.tabMainTab = New System.Windows.Forms.TabControl
		Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
		Me.lblClaimNumber = New System.Windows.Forms.Label
		Me.imglImages = New System.Windows.Forms.ImageList
		Me.lvwSelectPeril = New System.Windows.Forms.ListView
		Me.txtClaimNumber = New System.Windows.Forms.TextBox
		Me.cmdSelect = New System.Windows.Forms.Button
		Me.chkCloseClaim = New System.Windows.Forms.CheckBox
		Me.tabMainTab.SuspendLayout()
		Me._tabMainTab_TabPage0.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(464, 328)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 4
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(380, 328)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 3
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tabMainTab
		' 
		Me.tabMainTab.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabMainTab.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
		Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMainTab.ItemSize = New System.Drawing.Size(175, 18)
		Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
		Me.tabMainTab.Multiline = True
		Me.tabMainTab.Name = "tabMainTab"
		Me.tabMainTab.Size = New System.Drawing.Size(533, 317)
		Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMainTab.TabIndex = 5
		Me.tabMainTab.TabStop = False
		' 
		' _tabMainTab_TabPage0
		' 
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblClaimNumber)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lvwSelectPeril)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtClaimNumber)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmdSelect)
		Me._tabMainTab_TabPage0.Controls.Add(Me.chkCloseClaim)
		Me._tabMainTab_TabPage0.Text = "&1 - General"
		' 
		' lblClaimNumber
		' 
		Me.lblClaimNumber.AutoSize = True
		Me.lblClaimNumber.BackColor = System.Drawing.SystemColors.Control
		Me.lblClaimNumber.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblClaimNumber.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblClaimNumber.Enabled = True
		Me.lblClaimNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblClaimNumber.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblClaimNumber.Location = New System.Drawing.Point(16, 19)
		Me.lblClaimNumber.Name = "lblClaimNumber"
		Me.lblClaimNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblClaimNumber.Size = New System.Drawing.Size(86, 17)
		Me.lblClaimNumber.TabIndex = 0
		Me.lblClaimNumber.Text = "C&laim number:"
		Me.lblClaimNumber.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblClaimNumber.UseMnemonic = True
		Me.lblClaimNumber.Visible = True
		' 
		' imglImages
		' 
		Me.imglImages.ImageSize = New System.Drawing.Size(16, 16)
		Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        'Me.imglImages.Key_0 = "FindImage"
        Me.imglImages.Images.SetKeyName(0, "FindImages")
        Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(255, 255, 255)
		' 
		' lvwSelectPeril
		' 
		Me.lvwSelectPeril.BackColor = System.Drawing.SystemColors.Window
		Me.lvwSelectPeril.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwSelectPeril.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwSelectPeril.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwSelectPeril.HideSelection = True
		Me.lvwSelectPeril.LabelEdit = False
		Me.lvwSelectPeril.LabelWrap = True
		Me.lvwSelectPeril.LargeImageList = imglImages
		Me.lvwSelectPeril.Location = New System.Drawing.Point(16, 44)
		Me.lvwSelectPeril.Name = "lvwSelectPeril"
		Me.lvwSelectPeril.Size = New System.Drawing.Size(425, 233)
		Me.lvwSelectPeril.SmallImageList = imglImages
		Me.lvwSelectPeril.TabIndex = 2
		Me.lvwSelectPeril.View = System.Windows.Forms.View.Details
		' 
		' txtClaimNumber
		' 
		Me.txtClaimNumber.AcceptsReturn = True
		Me.txtClaimNumber.AutoSize = False
		Me.txtClaimNumber.BackColor = System.Drawing.SystemColors.Control
		Me.txtClaimNumber.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtClaimNumber.CausesValidation = True
		Me.txtClaimNumber.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtClaimNumber.Enabled = False
		Me.txtClaimNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtClaimNumber.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtClaimNumber.HideSelection = True
		Me.txtClaimNumber.Location = New System.Drawing.Point(136, 16)
		Me.txtClaimNumber.MaxLength = 0
		Me.txtClaimNumber.Multiline = False
		Me.txtClaimNumber.Name = "txtClaimNumber"
		Me.txtClaimNumber.ReadOnly = True
		Me.txtClaimNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtClaimNumber.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtClaimNumber.Size = New System.Drawing.Size(153, 19)
		Me.txtClaimNumber.TabIndex = 1
		Me.txtClaimNumber.TabStop = True
		Me.txtClaimNumber.Text = "XYZ789"
		Me.txtClaimNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtClaimNumber.Visible = True
		' 
		' cmdSelect
		' 
		Me.cmdSelect.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSelect.CausesValidation = True
		Me.cmdSelect.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSelect.Enabled = True
		Me.cmdSelect.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdSelect.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSelect.Location = New System.Drawing.Point(448, 64)
		Me.cmdSelect.Name = "cmdSelect"
		Me.cmdSelect.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSelect.Size = New System.Drawing.Size(73, 22)
		Me.cmdSelect.TabIndex = 6
		Me.cmdSelect.TabStop = True
		Me.cmdSelect.Text = "&Select"
		Me.cmdSelect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' chkCloseClaim
		' 
		Me.chkCloseClaim.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkCloseClaim.BackColor = System.Drawing.SystemColors.Control
		Me.chkCloseClaim.CausesValidation = True
		Me.chkCloseClaim.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
		Me.chkCloseClaim.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkCloseClaim.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkCloseClaim.Enabled = True
		Me.chkCloseClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkCloseClaim.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkCloseClaim.Location = New System.Drawing.Point(324, 19)
		Me.chkCloseClaim.Name = "chkCloseClaim"
		Me.chkCloseClaim.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkCloseClaim.Size = New System.Drawing.Size(112, 16)
		Me.chkCloseClaim.TabIndex = 7
		Me.chkCloseClaim.TabStop = True
		Me.chkCloseClaim.Text = "Close Claim"
		Me.chkCloseClaim.Visible = False
		' 
		' frmSelectPeril
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(546, 361)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.tabMainTab)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmSelectPeril"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "Select Peril"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMainTab, 1)
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwSelectPeril, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tabMainTab.ResumeLayout(False)
		Me._tabMainTab_TabPage0.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class