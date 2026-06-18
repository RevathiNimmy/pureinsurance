<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializeudOccursPer()
		InitializetxtOccursPer()
		InitializeoptOccurs()
		InitializelblOccursPer()
		tabMainTabPreviousTab = tabMainTab.SelectedIndex
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
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblComment As System.Windows.Forms.Label
	Public WithEvents lblDocumentDate As System.Windows.Forms.Label
	Public WithEvents lblDocumentRef As System.Windows.Forms.Label
	Public WithEvents imgIcon As System.Windows.Forms.PictureBox
	Public WithEvents lblDocumentType As System.Windows.Forms.Label
	Public WithEvents lblReversesOn As System.Windows.Forms.Label
	Public WithEvents lblOccurs As System.Windows.Forms.Label
	Public WithEvents lblTimes As System.Windows.Forms.Label
	Private WithEvents _lblOccursPer_0 As System.Windows.Forms.Label
	Private WithEvents _lblOccursPer_1 As System.Windows.Forms.Label
	Private WithEvents _lblOccursPer_2 As System.Windows.Forms.Label
	Public WithEvents cmbDocumentType As UserControls.TypeTable
	Public WithEvents txtComment As System.Windows.Forms.TextBox
	Public WithEvents txtDocumentDate As System.Windows.Forms.TextBox
	Public WithEvents txtDocumentRef As System.Windows.Forms.TextBox
	Public WithEvents txtReverseDate As System.Windows.Forms.TextBox
	Public WithEvents txtOccurs As System.Windows.Forms.TextBox
	Public WithEvents udOccurs As AxComCtl2.AxUpDown
	Private WithEvents _optOccurs_0 As System.Windows.Forms.RadioButton
	Private WithEvents _optOccurs_1 As System.Windows.Forms.RadioButton
	Private WithEvents _optOccurs_2 As System.Windows.Forms.RadioButton
	Private WithEvents _txtOccursPer_0 As System.Windows.Forms.TextBox
	Private WithEvents _txtOccursPer_1 As System.Windows.Forms.TextBox
	Private WithEvents _txtOccursPer_2 As System.Windows.Forms.TextBox
	Private WithEvents _udOccursPer_0 As AxComCtl2.AxUpDown
	Private WithEvents _udOccursPer_1 As AxComCtl2.AxUpDown
	Private WithEvents _udOccursPer_2 As AxComCtl2.AxUpDown
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public lblOccursPer(2) As System.Windows.Forms.Label
	Public optOccurs(2) As System.Windows.Forms.RadioButton
	Public txtOccursPer(2) As System.Windows.Forms.TextBox
	Public udOccursPer(2) As AxComCtl2.AxUpDown
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
		Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
		Me.dlgHelpFont = New System.Windows.Forms.FontDialog
		Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
		Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
		Me.cmdNavigate = New System.Windows.Forms.Button
		Me.cmdHelp = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.tabMainTab = New System.Windows.Forms.TabControl
		Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
		Me.lblComment = New System.Windows.Forms.Label
		Me.lblDocumentDate = New System.Windows.Forms.Label
		Me.lblDocumentRef = New System.Windows.Forms.Label
		Me.imgIcon = New System.Windows.Forms.PictureBox
		Me.lblDocumentType = New System.Windows.Forms.Label
		Me.lblReversesOn = New System.Windows.Forms.Label
		Me.lblOccurs = New System.Windows.Forms.Label
		Me.lblTimes = New System.Windows.Forms.Label
		Me._lblOccursPer_0 = New System.Windows.Forms.Label
		Me._lblOccursPer_1 = New System.Windows.Forms.Label
		Me._lblOccursPer_2 = New System.Windows.Forms.Label
		Me.cmbDocumentType = New UserControls.TypeTable
		Me.txtComment = New System.Windows.Forms.TextBox
		Me.txtDocumentDate = New System.Windows.Forms.TextBox
		Me.txtDocumentRef = New System.Windows.Forms.TextBox
		Me.txtReverseDate = New System.Windows.Forms.TextBox
		Me.txtOccurs = New System.Windows.Forms.TextBox
		Me.udOccurs = New AxComCtl2.AxUpDown
		Me._optOccurs_0 = New System.Windows.Forms.RadioButton
		Me._optOccurs_1 = New System.Windows.Forms.RadioButton
		Me._optOccurs_2 = New System.Windows.Forms.RadioButton
		Me._txtOccursPer_0 = New System.Windows.Forms.TextBox
		Me._txtOccursPer_1 = New System.Windows.Forms.TextBox
		Me._txtOccursPer_2 = New System.Windows.Forms.TextBox
		Me._udOccursPer_0 = New AxComCtl2.AxUpDown
		Me._udOccursPer_1 = New AxComCtl2.AxUpDown
		Me._udOccursPer_2 = New AxComCtl2.AxUpDown
		CType(Me.udOccurs, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me._udOccursPer_0, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me._udOccursPer_1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me._udOccursPer_2, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.tabMainTab.SuspendLayout()
		Me._tabMainTab_TabPage0.SuspendLayout()
		Me.SuspendLayout()
		' 
		' cmdNavigate
		' 
		Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
		Me.cmdNavigate.CausesValidation = True
		Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdNavigate.Enabled = True
		Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNavigate.Location = New System.Drawing.Point(8, 336)
		Me.cmdNavigate.Name = "cmdNavigate"
		Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
		Me.cmdNavigate.TabIndex = 16
		Me.cmdNavigate.TabStop = True
		Me.cmdNavigate.Text = "&Navigate"
		Me.cmdNavigate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.cmdNavigate.Visible = False
		' 
		' cmdHelp
		' 
		Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
		Me.cmdHelp.CausesValidation = True
		Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdHelp.Enabled = True
		Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdHelp.Location = New System.Drawing.Point(336, 336)
		Me.cmdHelp.Name = "cmdHelp"
		Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.cmdHelp.TabIndex = 15
		Me.cmdHelp.TabStop = True
		Me.cmdHelp.Text = "&Help"
		Me.cmdHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(256, 336)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 14
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
		Me.cmdOK.Location = New System.Drawing.Point(176, 336)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 13
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
		Me.tabMainTab.ItemSize = New System.Drawing.Size(79, 18)
		Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
		Me.tabMainTab.Multiline = True
		Me.tabMainTab.Name = "tabMainTab"
		Me.tabMainTab.Size = New System.Drawing.Size(405, 325)
		Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMainTab.TabIndex = 17
		' 
		' _tabMainTab_TabPage0
		' 
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblComment)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblDocumentDate)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblDocumentRef)
		Me._tabMainTab_TabPage0.Controls.Add(Me.imgIcon)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblDocumentType)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblReversesOn)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblOccurs)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblTimes)
		Me._tabMainTab_TabPage0.Controls.Add(Me._lblOccursPer_0)
		Me._tabMainTab_TabPage0.Controls.Add(Me._lblOccursPer_1)
		Me._tabMainTab_TabPage0.Controls.Add(Me._lblOccursPer_2)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmbDocumentType)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtComment)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtDocumentDate)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtDocumentRef)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtReverseDate)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtOccurs)
		Me._tabMainTab_TabPage0.Controls.Add(Me.udOccurs)
		Me._tabMainTab_TabPage0.Controls.Add(Me._optOccurs_0)
		Me._tabMainTab_TabPage0.Controls.Add(Me._optOccurs_1)
		Me._tabMainTab_TabPage0.Controls.Add(Me._optOccurs_2)
		Me._tabMainTab_TabPage0.Controls.Add(Me._txtOccursPer_0)
		Me._tabMainTab_TabPage0.Controls.Add(Me._txtOccursPer_1)
		Me._tabMainTab_TabPage0.Controls.Add(Me._txtOccursPer_2)
		Me._tabMainTab_TabPage0.Controls.Add(Me._udOccursPer_0)
		Me._tabMainTab_TabPage0.Controls.Add(Me._udOccursPer_1)
		Me._tabMainTab_TabPage0.Controls.Add(Me._udOccursPer_2)
		Me._tabMainTab_TabPage0.Text = "&1 - General"
		' 
		' lblComment
		' 
		Me.lblComment.AutoSize = False
		Me.lblComment.BackColor = System.Drawing.SystemColors.Control
		Me.lblComment.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblComment.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblComment.Enabled = True
		Me.lblComment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblComment.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblComment.Location = New System.Drawing.Point(8, 109)
		Me.lblComment.Name = "lblComment"
		Me.lblComment.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblComment.Size = New System.Drawing.Size(73, 17)
		Me.lblComment.TabIndex = 6
		Me.lblComment.Text = "Comment:"
		Me.lblComment.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblComment.UseMnemonic = True
		Me.lblComment.Visible = True
		' 
		' lblDocumentDate
		' 
		Me.lblDocumentDate.AutoSize = False
		Me.lblDocumentDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblDocumentDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDocumentDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDocumentDate.Enabled = True
		Me.lblDocumentDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDocumentDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDocumentDate.Location = New System.Drawing.Point(8, 77)
		Me.lblDocumentDate.Name = "lblDocumentDate"
		Me.lblDocumentDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDocumentDate.Size = New System.Drawing.Size(73, 17)
		Me.lblDocumentDate.TabIndex = 4
		Me.lblDocumentDate.Text = "Date:"
		Me.lblDocumentDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDocumentDate.UseMnemonic = True
		Me.lblDocumentDate.Visible = True
		' 
		' lblDocumentRef
		' 
		Me.lblDocumentRef.AutoSize = False
		Me.lblDocumentRef.BackColor = System.Drawing.SystemColors.Control
		Me.lblDocumentRef.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDocumentRef.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDocumentRef.Enabled = True
		Me.lblDocumentRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDocumentRef.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDocumentRef.Location = New System.Drawing.Point(8, 13)
		Me.lblDocumentRef.Name = "lblDocumentRef"
		Me.lblDocumentRef.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDocumentRef.Size = New System.Drawing.Size(73, 17)
		Me.lblDocumentRef.TabIndex = 2
		Me.lblDocumentRef.Text = "Reference:"
		Me.lblDocumentRef.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDocumentRef.UseMnemonic = True
		Me.lblDocumentRef.Visible = False
		' 
		' imgIcon
		' 
		Me.imgIcon.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
		Me.imgIcon.Enabled = True
		Me.imgIcon.Image = CType(resources.GetObject("imgIcon.Image"), System.Drawing.Image)
		Me.imgIcon.Location = New System.Drawing.Point(352, 12)
		Me.imgIcon.Name = "imgIcon"
		Me.imgIcon.Size = New System.Drawing.Size(32, 32)
		Me.imgIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.imgIcon.Visible = True
		' 
		' lblDocumentType
		' 
		Me.lblDocumentType.AutoSize = False
		Me.lblDocumentType.BackColor = System.Drawing.SystemColors.Control
		Me.lblDocumentType.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDocumentType.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDocumentType.Enabled = True
		Me.lblDocumentType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDocumentType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDocumentType.Location = New System.Drawing.Point(8, 46)
		Me.lblDocumentType.Name = "lblDocumentType"
		Me.lblDocumentType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDocumentType.Size = New System.Drawing.Size(73, 17)
		Me.lblDocumentType.TabIndex = 0
		Me.lblDocumentType.Text = "Type:"
		Me.lblDocumentType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDocumentType.UseMnemonic = True
		Me.lblDocumentType.Visible = True
		' 
		' lblReversesOn
		' 
		Me.lblReversesOn.AutoSize = True
		Me.lblReversesOn.BackColor = System.Drawing.SystemColors.Control
		Me.lblReversesOn.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblReversesOn.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblReversesOn.Enabled = False
		Me.lblReversesOn.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblReversesOn.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblReversesOn.Location = New System.Drawing.Point(8, 143)
		Me.lblReversesOn.Name = "lblReversesOn"
		Me.lblReversesOn.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblReversesOn.Size = New System.Drawing.Size(76, 13)
		Me.lblReversesOn.TabIndex = 8
		Me.lblReversesOn.Text = "Reverses on:"
		Me.lblReversesOn.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblReversesOn.UseMnemonic = True
		Me.lblReversesOn.Visible = True
		' 
		' lblOccurs
		' 
		Me.lblOccurs.AutoSize = True
		Me.lblOccurs.BackColor = System.Drawing.SystemColors.Control
		Me.lblOccurs.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblOccurs.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblOccurs.Enabled = False
		Me.lblOccurs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblOccurs.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblOccurs.Location = New System.Drawing.Point(8, 183)
		Me.lblOccurs.Name = "lblOccurs"
		Me.lblOccurs.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblOccurs.Size = New System.Drawing.Size(44, 13)
		Me.lblOccurs.TabIndex = 10
		Me.lblOccurs.Text = "Occurs:"
		Me.lblOccurs.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblOccurs.UseMnemonic = True
		Me.lblOccurs.Visible = True
		' 
		' lblTimes
		' 
		Me.lblTimes.AutoSize = True
		Me.lblTimes.BackColor = System.Drawing.SystemColors.Control
		Me.lblTimes.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTimes.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTimes.Enabled = True
		Me.lblTimes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTimes.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTimes.Location = New System.Drawing.Point(136, 183)
		Me.lblTimes.Name = "lblTimes"
		Me.lblTimes.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTimes.Size = New System.Drawing.Size(41, 13)
		Me.lblTimes.TabIndex = 18
		Me.lblTimes.Text = "time(s)"
		Me.lblTimes.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTimes.UseMnemonic = True
		Me.lblTimes.Visible = True
		' 
		' _lblOccursPer_0
		' 
		Me._lblOccursPer_0.AutoSize = True
		Me._lblOccursPer_0.BackColor = System.Drawing.SystemColors.Control
		Me._lblOccursPer_0.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblOccursPer_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblOccursPer_0.Enabled = False
		Me._lblOccursPer_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._lblOccursPer_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblOccursPer_0.Location = New System.Drawing.Point(272, 220)
		Me._lblOccursPer_0.Name = "_lblOccursPer_0"
		Me._lblOccursPer_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblOccursPer_0.Size = New System.Drawing.Size(55, 13)
		Me._lblOccursPer_0.TabIndex = 28
		Me._lblOccursPer_0.Text = "of period."
		Me._lblOccursPer_0.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblOccursPer_0.UseMnemonic = True
		Me._lblOccursPer_0.Visible = True
		' 
		' _lblOccursPer_1
		' 
		Me._lblOccursPer_1.AutoSize = True
		Me._lblOccursPer_1.BackColor = System.Drawing.SystemColors.Control
		Me._lblOccursPer_1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblOccursPer_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblOccursPer_1.Enabled = False
		Me._lblOccursPer_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._lblOccursPer_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblOccursPer_1.Location = New System.Drawing.Point(272, 244)
		Me._lblOccursPer_1.Name = "_lblOccursPer_1"
		Me._lblOccursPer_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblOccursPer_1.Size = New System.Drawing.Size(55, 13)
		Me._lblOccursPer_1.TabIndex = 29
		Me._lblOccursPer_1.Text = "of month."
		Me._lblOccursPer_1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblOccursPer_1.UseMnemonic = True
		Me._lblOccursPer_1.Visible = True
		' 
		' _lblOccursPer_2
		' 
		Me._lblOccursPer_2.AutoSize = True
		Me._lblOccursPer_2.BackColor = System.Drawing.SystemColors.Control
		Me._lblOccursPer_2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblOccursPer_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblOccursPer_2.Enabled = False
		Me._lblOccursPer_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._lblOccursPer_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblOccursPer_2.Location = New System.Drawing.Point(272, 268)
		Me._lblOccursPer_2.Name = "_lblOccursPer_2"
		Me._lblOccursPer_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblOccursPer_2.Size = New System.Drawing.Size(115, 13)
		Me._lblOccursPer_2.TabIndex = 30
		Me._lblOccursPer_2.Text = "of every 3rd month."
		Me._lblOccursPer_2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblOccursPer_2.UseMnemonic = True
		Me._lblOccursPer_2.Visible = True
		' 
		' cmbDocumentType
		' 
		Me.cmbDocumentType.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmbDocumentType.Location = New System.Drawing.Point(88, 44)
		Me.cmbDocumentType.Name = "cmbDocumentType"
		Me.cmbDocumentType.Size = New System.Drawing.Size(169, 21)
		Me.cmbDocumentType.Sorted = True
		Me.cmbDocumentType.TabIndex = 1
		Me.cmbDocumentType.WhatsThisHelpID = 19006
		' 
		' txtComment
		' 
		Me.txtComment.AcceptsReturn = True
		Me.txtComment.AutoSize = False
		Me.txtComment.BackColor = System.Drawing.SystemColors.Window
		Me.txtComment.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtComment.CausesValidation = True
		Me.txtComment.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtComment.Enabled = True
		Me.txtComment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtComment.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtComment.HideSelection = True
		Me.txtComment.Location = New System.Drawing.Point(88, 108)
		Me.txtComment.MaxLength = 60
		Me.txtComment.Multiline = False
		Me.txtComment.Name = "txtComment"
		Me.txtComment.ReadOnly = False
		Me.txtComment.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtComment.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtComment.Size = New System.Drawing.Size(241, 19)
		Me.txtComment.TabIndex = 7
		Me.txtComment.TabStop = True
		Me.txtComment.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtComment.Visible = True
		' 
		' txtDocumentDate
		' 
		Me.txtDocumentDate.AcceptsReturn = True
		Me.txtDocumentDate.AutoSize = False
		Me.txtDocumentDate.BackColor = System.Drawing.SystemColors.Window
		Me.txtDocumentDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDocumentDate.CausesValidation = True
		Me.txtDocumentDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDocumentDate.Enabled = True
		Me.txtDocumentDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDocumentDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDocumentDate.HideSelection = True
		Me.txtDocumentDate.Location = New System.Drawing.Point(88, 76)
		Me.txtDocumentDate.MaxLength = 0
		Me.txtDocumentDate.Multiline = False
		Me.txtDocumentDate.Name = "txtDocumentDate"
		Me.txtDocumentDate.ReadOnly = False
		Me.txtDocumentDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDocumentDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDocumentDate.Size = New System.Drawing.Size(121, 19)
		Me.txtDocumentDate.TabIndex = 5
		Me.txtDocumentDate.TabStop = True
		Me.txtDocumentDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDocumentDate.Visible = True
		' 
		' txtDocumentRef
		' 
		Me.txtDocumentRef.AcceptsReturn = True
		Me.txtDocumentRef.AutoSize = False
		Me.txtDocumentRef.BackColor = System.Drawing.SystemColors.Window
		Me.txtDocumentRef.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDocumentRef.CausesValidation = True
		Me.txtDocumentRef.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDocumentRef.Enabled = False
		Me.txtDocumentRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDocumentRef.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDocumentRef.HideSelection = True
		Me.txtDocumentRef.Location = New System.Drawing.Point(88, 12)
		Me.txtDocumentRef.MaxLength = 0
		Me.txtDocumentRef.Multiline = False
		Me.txtDocumentRef.Name = "txtDocumentRef"
		Me.txtDocumentRef.ReadOnly = False
		Me.txtDocumentRef.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDocumentRef.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDocumentRef.Size = New System.Drawing.Size(169, 19)
		Me.txtDocumentRef.TabIndex = 3
		Me.txtDocumentRef.TabStop = True
		Me.txtDocumentRef.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDocumentRef.Visible = False
		' 
		' txtReverseDate
		' 
		Me.txtReverseDate.AcceptsReturn = True
		Me.txtReverseDate.AutoSize = False
		Me.txtReverseDate.BackColor = System.Drawing.SystemColors.Window
		Me.txtReverseDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtReverseDate.CausesValidation = True
		Me.txtReverseDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtReverseDate.Enabled = False
		Me.txtReverseDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtReverseDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtReverseDate.HideSelection = True
		Me.txtReverseDate.Location = New System.Drawing.Point(88, 140)
		Me.txtReverseDate.MaxLength = 0
		Me.txtReverseDate.Multiline = False
		Me.txtReverseDate.Name = "txtReverseDate"
		Me.txtReverseDate.ReadOnly = False
		Me.txtReverseDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtReverseDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtReverseDate.Size = New System.Drawing.Size(121, 19)
		Me.txtReverseDate.TabIndex = 9
		Me.txtReverseDate.TabStop = True
		Me.txtReverseDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtReverseDate.Visible = True
		' 
		' txtOccurs
		' 
		Me.txtOccurs.AcceptsReturn = True
		Me.txtOccurs.AutoSize = False
		Me.txtOccurs.BackColor = System.Drawing.SystemColors.Window
		Me.txtOccurs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtOccurs.CausesValidation = True
		Me.txtOccurs.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtOccurs.Enabled = False
		Me.txtOccurs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtOccurs.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtOccurs.HideSelection = True
		Me.txtOccurs.Location = New System.Drawing.Point(88, 180)
		Me.txtOccurs.MaxLength = 0
		Me.txtOccurs.Multiline = False
		Me.txtOccurs.Name = "txtOccurs"
		Me.txtOccurs.ReadOnly = True
		Me.txtOccurs.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtOccurs.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtOccurs.Size = New System.Drawing.Size(25, 19)
		Me.txtOccurs.TabIndex = 11
		Me.txtOccurs.TabStop = True
		Me.txtOccurs.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtOccurs.Visible = True
		' 
		' udOccurs
		' 
		Me.udOccurs.Location = New System.Drawing.Point(112, 180)
		Me.udOccurs.Name = "udOccurs"
		Me.udOccurs.OcxState = CType(resources.GetObject("udOccurs.OcxState"), System.Windows.Forms.AxHost.State)
		Me.udOccurs.Size = New System.Drawing.Size(17, 19)
		Me.udOccurs.TabIndex = 12
		' 
		' _optOccurs_0
		' 
		Me._optOccurs_0.Appearance = System.Windows.Forms.Appearance.Normal
		Me._optOccurs_0.BackColor = System.Drawing.SystemColors.Control
		Me._optOccurs_0.CausesValidation = True
		Me._optOccurs_0.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optOccurs_0.Checked = True
		Me._optOccurs_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._optOccurs_0.Enabled = False
		Me._optOccurs_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._optOccurs_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me._optOccurs_0.Location = New System.Drawing.Point(88, 220)
		Me._optOccurs_0.Name = "_optOccurs_0"
		Me._optOccurs_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optOccurs_0.Size = New System.Drawing.Size(145, 13)
		Me._optOccurs_0.TabIndex = 19
		Me._optOccurs_0.TabStop = True
		Me._optOccurs_0.Text = "Per Period on day"
		Me._optOccurs_0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optOccurs_0.Visible = True
		' 
		' _optOccurs_1
		' 
		Me._optOccurs_1.Appearance = System.Windows.Forms.Appearance.Normal
		Me._optOccurs_1.BackColor = System.Drawing.SystemColors.Control
		Me._optOccurs_1.CausesValidation = True
		Me._optOccurs_1.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optOccurs_1.Checked = False
		Me._optOccurs_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._optOccurs_1.Enabled = False
		Me._optOccurs_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._optOccurs_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._optOccurs_1.Location = New System.Drawing.Point(88, 244)
		Me._optOccurs_1.Name = "_optOccurs_1"
		Me._optOccurs_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optOccurs_1.Size = New System.Drawing.Size(129, 13)
		Me._optOccurs_1.TabIndex = 20
		Me._optOccurs_1.TabStop = True
		Me._optOccurs_1.Text = "Per Month on day"
		Me._optOccurs_1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optOccurs_1.Visible = True
		' 
		' _optOccurs_2
		' 
		Me._optOccurs_2.Appearance = System.Windows.Forms.Appearance.Normal
		Me._optOccurs_2.BackColor = System.Drawing.SystemColors.Control
		Me._optOccurs_2.CausesValidation = True
		Me._optOccurs_2.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optOccurs_2.Checked = False
		Me._optOccurs_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._optOccurs_2.Enabled = False
		Me._optOccurs_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._optOccurs_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me._optOccurs_2.Location = New System.Drawing.Point(88, 268)
		Me._optOccurs_2.Name = "_optOccurs_2"
		Me._optOccurs_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optOccurs_2.Size = New System.Drawing.Size(145, 13)
		Me._optOccurs_2.TabIndex = 21
		Me._optOccurs_2.TabStop = True
		Me._optOccurs_2.Text = "Per Quarter on day"
		Me._optOccurs_2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optOccurs_2.Visible = True
		' 
		' _txtOccursPer_0
		' 
		Me._txtOccursPer_0.AcceptsReturn = True
		Me._txtOccursPer_0.AutoSize = False
		Me._txtOccursPer_0.BackColor = System.Drawing.SystemColors.Window
		Me._txtOccursPer_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._txtOccursPer_0.CausesValidation = True
		Me._txtOccursPer_0.Cursor = System.Windows.Forms.Cursors.IBeam
		Me._txtOccursPer_0.Enabled = False
		Me._txtOccursPer_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._txtOccursPer_0.ForeColor = System.Drawing.SystemColors.WindowText
		Me._txtOccursPer_0.HideSelection = True
		Me._txtOccursPer_0.Location = New System.Drawing.Point(224, 217)
		Me._txtOccursPer_0.MaxLength = 0
		Me._txtOccursPer_0.Multiline = False
		Me._txtOccursPer_0.Name = "_txtOccursPer_0"
		Me._txtOccursPer_0.ReadOnly = True
		Me._txtOccursPer_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._txtOccursPer_0.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me._txtOccursPer_0.Size = New System.Drawing.Size(25, 19)
		Me._txtOccursPer_0.TabIndex = 22
		Me._txtOccursPer_0.TabStop = True
		Me._txtOccursPer_0.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me._txtOccursPer_0.Visible = True
		' 
		' _txtOccursPer_1
		' 
		Me._txtOccursPer_1.AcceptsReturn = True
		Me._txtOccursPer_1.AutoSize = False
		Me._txtOccursPer_1.BackColor = System.Drawing.SystemColors.Window
		Me._txtOccursPer_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._txtOccursPer_1.CausesValidation = True
		Me._txtOccursPer_1.Cursor = System.Windows.Forms.Cursors.IBeam
		Me._txtOccursPer_1.Enabled = False
		Me._txtOccursPer_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._txtOccursPer_1.ForeColor = System.Drawing.SystemColors.WindowText
		Me._txtOccursPer_1.HideSelection = True
		Me._txtOccursPer_1.Location = New System.Drawing.Point(224, 241)
		Me._txtOccursPer_1.MaxLength = 0
		Me._txtOccursPer_1.Multiline = False
		Me._txtOccursPer_1.Name = "_txtOccursPer_1"
		Me._txtOccursPer_1.ReadOnly = True
		Me._txtOccursPer_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._txtOccursPer_1.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me._txtOccursPer_1.Size = New System.Drawing.Size(25, 19)
		Me._txtOccursPer_1.TabIndex = 23
		Me._txtOccursPer_1.TabStop = True
		Me._txtOccursPer_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me._txtOccursPer_1.Visible = True
		' 
		' _txtOccursPer_2
		' 
		Me._txtOccursPer_2.AcceptsReturn = True
		Me._txtOccursPer_2.AutoSize = False
		Me._txtOccursPer_2.BackColor = System.Drawing.SystemColors.Window
		Me._txtOccursPer_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._txtOccursPer_2.CausesValidation = True
		Me._txtOccursPer_2.Cursor = System.Windows.Forms.Cursors.IBeam
		Me._txtOccursPer_2.Enabled = False
		Me._txtOccursPer_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._txtOccursPer_2.ForeColor = System.Drawing.SystemColors.WindowText
		Me._txtOccursPer_2.HideSelection = True
		Me._txtOccursPer_2.Location = New System.Drawing.Point(224, 265)
		Me._txtOccursPer_2.MaxLength = 0
		Me._txtOccursPer_2.Multiline = False
		Me._txtOccursPer_2.Name = "_txtOccursPer_2"
		Me._txtOccursPer_2.ReadOnly = True
		Me._txtOccursPer_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._txtOccursPer_2.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me._txtOccursPer_2.Size = New System.Drawing.Size(25, 19)
		Me._txtOccursPer_2.TabIndex = 24
		Me._txtOccursPer_2.TabStop = True
		Me._txtOccursPer_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me._txtOccursPer_2.Visible = True
		' 
		' _udOccursPer_0
		' 
		Me._udOccursPer_0.Location = New System.Drawing.Point(248, 217)
		Me._udOccursPer_0.Name = "_udOccursPer_0"
		Me._udOccursPer_0.OcxState = CType(resources.GetObject("_udOccursPer_0.OcxState"), System.Windows.Forms.AxHost.State)
		Me._udOccursPer_0.Size = New System.Drawing.Size(16, 19)
		Me._udOccursPer_0.TabIndex = 25
		' 
		' _udOccursPer_1
		' 
		Me._udOccursPer_1.Location = New System.Drawing.Point(248, 241)
		Me._udOccursPer_1.Name = "_udOccursPer_1"
		Me._udOccursPer_1.OcxState = CType(resources.GetObject("_udOccursPer_1.OcxState"), System.Windows.Forms.AxHost.State)
		Me._udOccursPer_1.Size = New System.Drawing.Size(16, 19)
		Me._udOccursPer_1.TabIndex = 26
		' 
		' _udOccursPer_2
		' 
		Me._udOccursPer_2.Location = New System.Drawing.Point(248, 265)
		Me._udOccursPer_2.Name = "_udOccursPer_2"
		Me._udOccursPer_2.OcxState = CType(resources.GetObject("_udOccursPer_2.OcxState"), System.Windows.Forms.AxHost.State)
		Me._udOccursPer_2.Size = New System.Drawing.Size(16, 19)
		Me._udOccursPer_2.TabIndex = 27
		' 
		' frmInterface
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(417, 365)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdNavigate)
		Me.Controls.Add(Me.cmdHelp)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.tabMainTab)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = True
		Me.Icon = CType(resources.GetObject("frmInterface.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(203, 163)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Document"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMainTab, 1)
		CType(Me.udOccurs, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me._udOccursPer_0, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me._udOccursPer_1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me._udOccursPer_2, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tabMainTab.ResumeLayout(False)
		Me._tabMainTab_TabPage0.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub InitializeudOccursPer()
		Me.udOccursPer(2) = _udOccursPer_2
		Me.udOccursPer(1) = _udOccursPer_1
		Me.udOccursPer(0) = _udOccursPer_0
	End Sub
	Sub InitializetxtOccursPer()
		Me.txtOccursPer(2) = _txtOccursPer_2
		Me.txtOccursPer(1) = _txtOccursPer_1
		Me.txtOccursPer(0) = _txtOccursPer_0
	End Sub
	Sub InitializeoptOccurs()
		Me.optOccurs(2) = _optOccurs_2
		Me.optOccurs(1) = _optOccurs_1
		Me.optOccurs(0) = _optOccurs_0
	End Sub
	Sub InitializelblOccursPer()
		Me.lblOccursPer(2) = _lblOccursPer_2
		Me.lblOccursPer(1) = _lblOccursPer_1
		Me.lblOccursPer(0) = _lblOccursPer_0
	End Sub
#End Region 
End Class