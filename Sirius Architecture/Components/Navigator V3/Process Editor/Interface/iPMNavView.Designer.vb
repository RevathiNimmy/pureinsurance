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
	Private Sub Ctx_mnuGroup_Opening(ByVal sender As object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuGroup.Opening
		Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
		Ctx_mnuGroup.Items.Clear()
		'We are moving the submenus from original menu to the context menu before displaying it
		For	Each item As System.Windows.Forms.ToolStripItem In mnuGroup.DropDownItems
			list.Add(item)
		Next item
		For	Each item As System.Windows.Forms.ToolStripItem In list
			Ctx_mnuGroup.Items.Add(item)
		Next item
		e.Cancel = False
	End Sub
	Private Sub Ctx_mnuGroup_Closing(ByVal sender As object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuGroup.Closing
		Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
		'We are moving the submenus the context menu back to the original menu after displaying
		For	Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuGroup.Items
			list.Add(item)
		Next item
		For	Each item As System.Windows.Forms.ToolStripItem In list
			mnuGroup.DropDownItems.Add(item)
		Next item
	End Sub
	Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
		Dispose(True)
	End Sub
    'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents mnuEditKeys As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuEditCopy As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuEditEnforce As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuStepComponents As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuEdit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuKeys As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuNewItem As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGroupDelete As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGroupComponent As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGroup As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	Private WithEvents _tlbToolBar_Button1 As System.Windows.Forms.ToolStripButton
	Private WithEvents _tlbToolBar_Button2 As System.Windows.Forms.ToolStripButton
	Private WithEvents _tlbToolBar_Button3 As System.Windows.Forms.ToolStripButton
	Private WithEvents _tlbToolBar_Button4 As System.Windows.Forms.ToolStripButton
	Private WithEvents _tlbToolBar_Button5 As System.Windows.Forms.ToolStripButton
	Private WithEvents _tlbToolBar_Button6 As System.Windows.Forms.ToolStripButton
	Private WithEvents _tlbToolBar_Button7 As System.Windows.Forms.ToolStripButton
	Private WithEvents _tlbToolBar_Button8 As System.Windows.Forms.ToolStripButton
	Private WithEvents _tlbToolBar_Button9 As System.Windows.Forms.ToolStripButton
	Private WithEvents _tlbToolBar_Button10 As System.Windows.Forms.ToolStripButton
	Private WithEvents _tlbToolBar_Button11 As System.Windows.Forms.ToolStripButton
	Public WithEvents tlbToolBar As System.Windows.Forms.ToolStrip
	Public WithEvents chkIsDeleted As System.Windows.Forms.CheckBox
	Public WithEvents cboGroupDescription As System.Windows.Forms.ComboBox
	Public WithEvents cboGroups As System.Windows.Forms.ComboBox
	Public WithEvents LinSliderBar As System.Windows.Forms.Label
	Public WithEvents panSliderBar As System.Windows.Forms.Panel
	Public WithEvents cmdApply As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents treMainData As System.Windows.Forms.TreeView
	Public WithEvents cboPMNav_Steppmnav_map_id As System.Windows.Forms.ComboBox
	Public WithEvents cboPMNav_Steptask As System.Windows.Forms.ComboBox
	Public WithEvents chkPMNav_Stepis_logged As System.Windows.Forms.CheckBox
	Public WithEvents txtPMNav_Stepdescription As System.Windows.Forms.TextBox
	Public WithEvents cboPMNav_Stepnavigate_status As System.Windows.Forms.ComboBox
	Public WithEvents cboPMNav_Stepsub_nav_map_id As System.Windows.Forms.ComboBox
	Public WithEvents cboPMNav_Stepok_action As System.Windows.Forms.ComboBox
	Public WithEvents cboPMNav_Stepcancel_action As System.Windows.Forms.ComboBox
	Public WithEvents chkPMNav_Stepis_hidden As System.Windows.Forms.CheckBox
	Public WithEvents cboPMNav_Steppmnav_component_id As System.Windows.Forms.ComboBox
	Public WithEvents txtPMNav_Stepcaption_id As System.Windows.Forms.TextBox
	Public WithEvents cboPMNav_Stepok_nav_process_id As System.Windows.Forms.ComboBox
	Public WithEvents txtPMNav_Stepok_no_of_steps As System.Windows.Forms.TextBox
	Public WithEvents cboPMNav_Stepcancel_nav_process_id As System.Windows.Forms.ComboBox
	Public WithEvents txtPMNav_Stepcancel_no_of_steps As System.Windows.Forms.TextBox
	Public WithEvents lblStepDetails As System.Windows.Forms.Label
	Public WithEvents lblMap As System.Windows.Forms.Label
	Public WithEvents lblSComponent As System.Windows.Forms.Label
	Public WithEvents lblSTask As System.Windows.Forms.Label
	Public WithEvents lblSDescription As System.Windows.Forms.Label
	Public WithEvents lblSNavStatus As System.Windows.Forms.Label
	Public WithEvents lblSSubMap As System.Windows.Forms.Label
	Public WithEvents lblSAction As System.Windows.Forms.Label
	Public WithEvents lblSOK As System.Windows.Forms.Label
	Public WithEvents lblSCancel As System.Windows.Forms.Label
	Public WithEvents fraStep As System.Windows.Forms.PictureBox
	Public WithEvents cboPMNav_Processis_user_driven As System.Windows.Forms.ComboBox
	Public WithEvents cboPMNav_Processtransaction_type_id As System.Windows.Forms.ComboBox
	Public WithEvents txtPMNav_Processeffective_date As System.Windows.Forms.TextBox
	Public WithEvents chkPMNav_Processis_logged As System.Windows.Forms.CheckBox
	Public WithEvents cboPMNav_Processpmproduct_id As System.Windows.Forms.ComboBox
	Public WithEvents cboPMNav_Processpmproc_lock_group_id As System.Windows.Forms.ComboBox
	Public WithEvents cboPMNav_Processprocess_mode As System.Windows.Forms.ComboBox
	Public WithEvents txtPMNav_Processcode As System.Windows.Forms.TextBox
	Public WithEvents cboPMNav_Processstart_nav_map_id As System.Windows.Forms.ComboBox
	Public WithEvents txtPMNav_Processdescription As System.Windows.Forms.TextBox
	Public WithEvents txtPMNav_Processcaption_id As System.Windows.Forms.TextBox
	Public WithEvents lblPNavMode As System.Windows.Forms.Label
	Public WithEvents lblPEffectiveDate As System.Windows.Forms.Label
	Public WithEvents lblPTransType As System.Windows.Forms.Label
	Public WithEvents lblPDescription As System.Windows.Forms.Label
	Public WithEvents lblPStartNavMap As System.Windows.Forms.Label
	Public WithEvents lblPProduct As System.Windows.Forms.Label
	Public WithEvents lblPProcessLockGroup As System.Windows.Forms.Label
	Public WithEvents lblPProcessMode As System.Windows.Forms.Label
	Public WithEvents lblPCode As System.Windows.Forms.Label
	Public WithEvents lblProcessDetails As System.Windows.Forms.Label
	Public WithEvents fraProcess As System.Windows.Forms.PictureBox
	Public WithEvents fraBlank As System.Windows.Forms.PictureBox
	Public WithEvents txtPMNav_Componentdescription As System.Windows.Forms.TextBox
	Public WithEvents txtPMNav_Componentcaption_id As System.Windows.Forms.TextBox
	Public WithEvents chkPMNav_Componentis_server_side As System.Windows.Forms.CheckBox
	Public WithEvents txtPMNav_Componentclass_name As System.Windows.Forms.TextBox
	Public WithEvents txtPMNav_Componentobject_name As System.Windows.Forms.TextBox
	Public WithEvents txtPMNav_Componenteffective_date As System.Windows.Forms.TextBox
	Public WithEvents cboPMNav_Componentnav_component_type As System.Windows.Forms.ComboBox
	Public WithEvents lblComponentDetails As System.Windows.Forms.Label
	Public WithEvents lblCClassName As System.Windows.Forms.Label
	Public WithEvents lblCObjectName As System.Windows.Forms.Label
	Public WithEvents lblCDescription As System.Windows.Forms.Label
	Public WithEvents lblCType As System.Windows.Forms.Label
	Public WithEvents lblCEffectiveDate As System.Windows.Forms.Label
	Public WithEvents fraComponent As System.Windows.Forms.PictureBox
	Public WithEvents txtPMNav_MapDescription As System.Windows.Forms.TextBox
	Public WithEvents txtPMNav_Mapcaption_id As System.Windows.Forms.TextBox
	Public WithEvents txtPMNav_Mapcode As System.Windows.Forms.TextBox
	Public WithEvents chkPMNav_Mapis_start_map As System.Windows.Forms.CheckBox
	Public WithEvents txtPMNav_Mapeffective_date As System.Windows.Forms.TextBox
	Public WithEvents lblMapDetails As System.Windows.Forms.Label
	Public WithEvents lblMCode As System.Windows.Forms.Label
	Public WithEvents lblMDescription As System.Windows.Forms.Label
	Public WithEvents lblMEffectiveDate As System.Windows.Forms.Label
	Public WithEvents fraMap As System.Windows.Forms.PictureBox
	Public WithEvents imgTreeImages As System.Windows.Forms.ImageList
	Public WithEvents linMenuLine1 As System.Windows.Forms.Label
	Public WithEvents linMenuLine2 As System.Windows.Forms.Label
	Public WithEvents imgImages As System.Windows.Forms.ImageList
	Public WithEvents Ctx_mnuGroup As System.Windows.Forms.ContextMenuStrip
	Private WithEvents listBoxComboBoxHelper1 As Artinsoft.VB6.Gui.ListControlHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.MainMenu1 = New System.Windows.Forms.MenuStrip
		Me.mnuEdit = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuEditKeys = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuEditCopy = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuEditEnforce = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuStepComponents = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuGroup = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuKeys = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuNewItem = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuGroupDelete = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuGroupComponent = New System.Windows.Forms.ToolStripMenuItem
		Me.tlbToolBar = New System.Windows.Forms.ToolStrip
		Me._tlbToolBar_Button1 = New System.Windows.Forms.ToolStripButton
		Me._tlbToolBar_Button2 = New System.Windows.Forms.ToolStripButton
		Me._tlbToolBar_Button3 = New System.Windows.Forms.ToolStripButton
		Me._tlbToolBar_Button4 = New System.Windows.Forms.ToolStripButton
		Me._tlbToolBar_Button5 = New System.Windows.Forms.ToolStripButton
		Me._tlbToolBar_Button6 = New System.Windows.Forms.ToolStripButton
		Me._tlbToolBar_Button7 = New System.Windows.Forms.ToolStripButton
		Me._tlbToolBar_Button8 = New System.Windows.Forms.ToolStripButton
		Me._tlbToolBar_Button9 = New System.Windows.Forms.ToolStripButton
		Me._tlbToolBar_Button10 = New System.Windows.Forms.ToolStripButton
		Me._tlbToolBar_Button11 = New System.Windows.Forms.ToolStripButton
		Me.chkIsDeleted = New System.Windows.Forms.CheckBox
		Me.cboGroupDescription = New System.Windows.Forms.ComboBox
		Me.cboGroups = New System.Windows.Forms.ComboBox
		Me.panSliderBar = New System.Windows.Forms.Panel
		Me.LinSliderBar = New System.Windows.Forms.Label
		Me.cmdApply = New System.Windows.Forms.Button
		Me.cmdExit = New System.Windows.Forms.Button
		Me.treMainData = New System.Windows.Forms.TreeView
		Me.fraStep = New System.Windows.Forms.PictureBox
		Me.cboPMNav_Steppmnav_map_id = New System.Windows.Forms.ComboBox
		Me.cboPMNav_Steptask = New System.Windows.Forms.ComboBox
		Me.chkPMNav_Stepis_logged = New System.Windows.Forms.CheckBox
		Me.txtPMNav_Stepdescription = New System.Windows.Forms.TextBox
		Me.cboPMNav_Stepnavigate_status = New System.Windows.Forms.ComboBox
		Me.cboPMNav_Stepsub_nav_map_id = New System.Windows.Forms.ComboBox
		Me.cboPMNav_Stepok_action = New System.Windows.Forms.ComboBox
		Me.cboPMNav_Stepcancel_action = New System.Windows.Forms.ComboBox
		Me.chkPMNav_Stepis_hidden = New System.Windows.Forms.CheckBox
		Me.cboPMNav_Steppmnav_component_id = New System.Windows.Forms.ComboBox
		Me.txtPMNav_Stepcaption_id = New System.Windows.Forms.TextBox
		Me.cboPMNav_Stepok_nav_process_id = New System.Windows.Forms.ComboBox
		Me.txtPMNav_Stepok_no_of_steps = New System.Windows.Forms.TextBox
		Me.cboPMNav_Stepcancel_nav_process_id = New System.Windows.Forms.ComboBox
		Me.txtPMNav_Stepcancel_no_of_steps = New System.Windows.Forms.TextBox
		Me.lblStepDetails = New System.Windows.Forms.Label
		Me.lblMap = New System.Windows.Forms.Label
		Me.lblSComponent = New System.Windows.Forms.Label
		Me.lblSTask = New System.Windows.Forms.Label
		Me.lblSDescription = New System.Windows.Forms.Label
		Me.lblSNavStatus = New System.Windows.Forms.Label
		Me.lblSSubMap = New System.Windows.Forms.Label
		Me.lblSAction = New System.Windows.Forms.Label
		Me.lblSOK = New System.Windows.Forms.Label
		Me.lblSCancel = New System.Windows.Forms.Label
		Me.fraProcess = New System.Windows.Forms.PictureBox
		Me.cboPMNav_Processis_user_driven = New System.Windows.Forms.ComboBox
		Me.cboPMNav_Processtransaction_type_id = New System.Windows.Forms.ComboBox
		Me.txtPMNav_Processeffective_date = New System.Windows.Forms.TextBox
		Me.chkPMNav_Processis_logged = New System.Windows.Forms.CheckBox
		Me.cboPMNav_Processpmproduct_id = New System.Windows.Forms.ComboBox
		Me.cboPMNav_Processpmproc_lock_group_id = New System.Windows.Forms.ComboBox
		Me.cboPMNav_Processprocess_mode = New System.Windows.Forms.ComboBox
		Me.txtPMNav_Processcode = New System.Windows.Forms.TextBox
		Me.cboPMNav_Processstart_nav_map_id = New System.Windows.Forms.ComboBox
		Me.txtPMNav_Processdescription = New System.Windows.Forms.TextBox
		Me.txtPMNav_Processcaption_id = New System.Windows.Forms.TextBox
		Me.lblPNavMode = New System.Windows.Forms.Label
		Me.lblPEffectiveDate = New System.Windows.Forms.Label
		Me.lblPTransType = New System.Windows.Forms.Label
		Me.lblPDescription = New System.Windows.Forms.Label
		Me.lblPStartNavMap = New System.Windows.Forms.Label
		Me.lblPProduct = New System.Windows.Forms.Label
		Me.lblPProcessLockGroup = New System.Windows.Forms.Label
		Me.lblPProcessMode = New System.Windows.Forms.Label
		Me.lblPCode = New System.Windows.Forms.Label
		Me.lblProcessDetails = New System.Windows.Forms.Label
		Me.fraBlank = New System.Windows.Forms.PictureBox
		Me.fraComponent = New System.Windows.Forms.PictureBox
		Me.txtPMNav_Componentdescription = New System.Windows.Forms.TextBox
		Me.txtPMNav_Componentcaption_id = New System.Windows.Forms.TextBox
		Me.chkPMNav_Componentis_server_side = New System.Windows.Forms.CheckBox
		Me.txtPMNav_Componentclass_name = New System.Windows.Forms.TextBox
		Me.txtPMNav_Componentobject_name = New System.Windows.Forms.TextBox
		Me.txtPMNav_Componenteffective_date = New System.Windows.Forms.TextBox
		Me.cboPMNav_Componentnav_component_type = New System.Windows.Forms.ComboBox
		Me.lblComponentDetails = New System.Windows.Forms.Label
		Me.lblCClassName = New System.Windows.Forms.Label
		Me.lblCObjectName = New System.Windows.Forms.Label
		Me.lblCDescription = New System.Windows.Forms.Label
		Me.lblCType = New System.Windows.Forms.Label
		Me.lblCEffectiveDate = New System.Windows.Forms.Label
		Me.fraMap = New System.Windows.Forms.PictureBox
		Me.txtPMNav_MapDescription = New System.Windows.Forms.TextBox
		Me.txtPMNav_Mapcaption_id = New System.Windows.Forms.TextBox
		Me.txtPMNav_Mapcode = New System.Windows.Forms.TextBox
		Me.chkPMNav_Mapis_start_map = New System.Windows.Forms.CheckBox
		Me.txtPMNav_Mapeffective_date = New System.Windows.Forms.TextBox
		Me.lblMapDetails = New System.Windows.Forms.Label
		Me.lblMCode = New System.Windows.Forms.Label
		Me.lblMDescription = New System.Windows.Forms.Label
		Me.lblMEffectiveDate = New System.Windows.Forms.Label
		Me.imgTreeImages = New System.Windows.Forms.ImageList
		Me.linMenuLine1 = New System.Windows.Forms.Label
		Me.linMenuLine2 = New System.Windows.Forms.Label
		Me.imgImages = New System.Windows.Forms.ImageList
		Me.tlbToolBar.SuspendLayout()
		Me.panSliderBar.SuspendLayout()
		Me.fraStep.SuspendLayout()
		Me.fraProcess.SuspendLayout()
		Me.fraComponent.SuspendLayout()
		Me.fraMap.SuspendLayout()
		Me.SuspendLayout()
		'Ctx_mnuGroup
		Me.Ctx_mnuGroup = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.Ctx_mnuGroup.Size = New System.Drawing.Size(153, 26)
		Me.listBoxComboBoxHelper1 = New Artinsoft.VB6.Gui.ListControlHelper(Me.components)
		CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' MainMenu1
		' 
		Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuEdit, Me.mnuGroup})
		' 
		' mnuEdit
		' 
		Me.mnuEdit.Available = True
		Me.mnuEdit.Checked = False
		Me.mnuEdit.Enabled = True
		Me.mnuEdit.Name = "mnuEdit"
		Me.mnuEdit.Text = "&Options"
		Me.mnuEdit.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuEditKeys, Me.mnuEditCopy, Me.mnuEditEnforce, Me.mnuStepComponents})
		' 
		' mnuEditKeys
		' 
		Me.mnuEditKeys.Available = True
		Me.mnuEditKeys.Checked = False
		Me.mnuEditKeys.Enabled = True
		Me.mnuEditKeys.Name = "mnuEditKeys"
		Me.mnuEditKeys.Text = "&Keys"
		' 
		' mnuEditCopy
		' 
		Me.mnuEditCopy.Available = True
		Me.mnuEditCopy.Checked = False
		Me.mnuEditCopy.Enabled = True
		Me.mnuEditCopy.Name = "mnuEditCopy"
		Me.mnuEditCopy.Text = "&Copy"
		' 
		' mnuEditEnforce
		' 
		Me.mnuEditEnforce.Available = True
		Me.mnuEditEnforce.Checked = True
		Me.mnuEditEnforce.Enabled = True
		Me.mnuEditEnforce.Name = "mnuEditEnforce"
		Me.mnuEditEnforce.Text = "&Enforce Key Rules"
		' 
		' mnuStepComponents
		' 
		Me.mnuStepComponents.Available = True
		Me.mnuStepComponents.Checked = False
		Me.mnuStepComponents.Enabled = True
		Me.mnuStepComponents.Name = "mnuStepComponents"
		Me.mnuStepComponents.Text = "&Show Step Components"
		' 
		' mnuGroup
		' 
		Me.mnuGroup.Available = False
		Me.mnuGroup.Checked = False
		Me.mnuGroup.Enabled = True
		Me.mnuGroup.Name = "mnuGroup"
		Me.mnuGroup.Text = "&Group"
		Me.mnuGroup.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuKeys, Me.mnuNewItem, Me.mnuGroupDelete, Me.mnuGroupComponent})
		' 
		' mnuKeys
		' 
		Me.mnuKeys.Available = True
		Me.mnuKeys.Checked = False
		Me.mnuKeys.Enabled = True
		Me.mnuKeys.Name = "mnuKeys"
		Me.mnuKeys.Text = "&Keys"
		' 
		' mnuNewItem
		' 
		Me.mnuNewItem.Available = True
		Me.mnuNewItem.Checked = False
		Me.mnuNewItem.Enabled = True
		Me.mnuNewItem.Name = "mnuNewItem"
		Me.mnuNewItem.Text = "&New"
		' 
		' mnuGroupDelete
		' 
		Me.mnuGroupDelete.Available = True
		Me.mnuGroupDelete.Checked = False
		Me.mnuGroupDelete.Enabled = True
		Me.mnuGroupDelete.Name = "mnuGroupDelete"
		Me.mnuGroupDelete.Text = "&Delete"
		' 
		' mnuGroupComponent
		' 
		Me.mnuGroupComponent.Available = False
		Me.mnuGroupComponent.Checked = False
		Me.mnuGroupComponent.Enabled = True
		Me.mnuGroupComponent.Name = "mnuGroupComponent"
		Me.mnuGroupComponent.Text = "&Component"
		' 
		' tlbToolBar
		' 
		Me.tlbToolBar.Dock = System.Windows.Forms.DockStyle.None
		Me.tlbToolBar.ImageList = imgImages
		Me.tlbToolBar.Location = New System.Drawing.Point(0, 28)
		Me.tlbToolBar.Name = "tlbToolBar"
		Me.tlbToolBar.ShowItemToolTips = True
		Me.tlbToolBar.Size = New System.Drawing.Size(593, 26)
		Me.tlbToolBar.TabIndex = 77
		Me.tlbToolBar.Items.Add(Me._tlbToolBar_Button1)
		Me.tlbToolBar.Items.Add(Me._tlbToolBar_Button2)
		Me.tlbToolBar.Items.Add(Me._tlbToolBar_Button3)
		Me.tlbToolBar.Items.Add(Me._tlbToolBar_Button4)
		Me.tlbToolBar.Items.Add(Me._tlbToolBar_Button5)
		Me.tlbToolBar.Items.Add(Me._tlbToolBar_Button6)
		Me.tlbToolBar.Items.Add(Me._tlbToolBar_Button7)
		Me.tlbToolBar.Items.Add(Me._tlbToolBar_Button8)
		Me.tlbToolBar.Items.Add(Me._tlbToolBar_Button9)
		Me.tlbToolBar.Items.Add(Me._tlbToolBar_Button10)
		Me.tlbToolBar.Items.Add(Me._tlbToolBar_Button11)
		' 
		' _tlbToolBar_Button1
		' 
		Me._tlbToolBar_Button1.AutoSize = False
		Me._tlbToolBar_Button1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._tlbToolBar_Button1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._tlbToolBar_Button1.Name = ""
		Me._tlbToolBar_Button1.Size = New System.Drawing.Size(24, 22)
		Me._tlbToolBar_Button1.Tag = ""
		Me._tlbToolBar_Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._tlbToolBar_Button1.Visible = False
		' 
		' _tlbToolBar_Button2
		' 
		Me._tlbToolBar_Button2.AutoSize = False
		Me._tlbToolBar_Button2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._tlbToolBar_Button2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._tlbToolBar_Button2.Name = ""
		Me._tlbToolBar_Button2.Size = New System.Drawing.Size(24, 22)
		Me._tlbToolBar_Button2.Tag = ""
		Me._tlbToolBar_Button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._tlbToolBar_Button2.Visible = False
		' 
		' _tlbToolBar_Button3
		' 
		Me._tlbToolBar_Button3.AutoSize = False
		Me._tlbToolBar_Button3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._tlbToolBar_Button3.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._tlbToolBar_Button3.Name = ""
		Me._tlbToolBar_Button3.Size = New System.Drawing.Size(24, 22)
		Me._tlbToolBar_Button3.Tag = ""
		Me._tlbToolBar_Button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._tlbToolBar_Button3.Visible = False
		' 
		' _tlbToolBar_Button4
		' 
		Me._tlbToolBar_Button4.AutoSize = False
		Me._tlbToolBar_Button4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._tlbToolBar_Button4.ImageKey = "Component"
		Me._tlbToolBar_Button4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._tlbToolBar_Button4.Name = ""
		Me._tlbToolBar_Button4.Size = New System.Drawing.Size(24, 22)
		Me._tlbToolBar_Button4.Tag = ""
		Me._tlbToolBar_Button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._tlbToolBar_Button4.ToolTipText = "New Component"
		' 
		' _tlbToolBar_Button5
		' 
		Me._tlbToolBar_Button5.AutoSize = False
		Me._tlbToolBar_Button5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._tlbToolBar_Button5.ImageKey = "Process"
		Me._tlbToolBar_Button5.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._tlbToolBar_Button5.Name = ""
		Me._tlbToolBar_Button5.Size = New System.Drawing.Size(24, 22)
		Me._tlbToolBar_Button5.Tag = ""
		Me._tlbToolBar_Button5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._tlbToolBar_Button5.ToolTipText = "New Process"
		' 
		' _tlbToolBar_Button6
		' 
		Me._tlbToolBar_Button6.AutoSize = False
		Me._tlbToolBar_Button6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._tlbToolBar_Button6.ImageKey = "Map"
		Me._tlbToolBar_Button6.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._tlbToolBar_Button6.Name = ""
		Me._tlbToolBar_Button6.Size = New System.Drawing.Size(24, 22)
		Me._tlbToolBar_Button6.Tag = ""
		Me._tlbToolBar_Button6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._tlbToolBar_Button6.ToolTipText = "New Map"
		' 
		' _tlbToolBar_Button7
		' 
		Me._tlbToolBar_Button7.AutoSize = False
		Me._tlbToolBar_Button7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._tlbToolBar_Button7.ImageKey = "Step"
		Me._tlbToolBar_Button7.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._tlbToolBar_Button7.Name = ""
		Me._tlbToolBar_Button7.Size = New System.Drawing.Size(24, 22)
		Me._tlbToolBar_Button7.Tag = ""
		Me._tlbToolBar_Button7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._tlbToolBar_Button7.ToolTipText = "New Step"
		' 
		' _tlbToolBar_Button8
		' 
		Me._tlbToolBar_Button8.AutoSize = False
		Me._tlbToolBar_Button8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._tlbToolBar_Button8.ImageKey = "Keys"
		Me._tlbToolBar_Button8.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._tlbToolBar_Button8.Name = ""
		Me._tlbToolBar_Button8.Size = New System.Drawing.Size(24, 22)
		Me._tlbToolBar_Button8.Tag = ""
		Me._tlbToolBar_Button8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._tlbToolBar_Button8.ToolTipText = "Get/Set Keys"
		' 
		' _tlbToolBar_Button9
		' 
		Me._tlbToolBar_Button9.AutoSize = False
		Me._tlbToolBar_Button9.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._tlbToolBar_Button9.ImageKey = "Delete"
		Me._tlbToolBar_Button9.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._tlbToolBar_Button9.Name = ""
		Me._tlbToolBar_Button9.Size = New System.Drawing.Size(24, 22)
		Me._tlbToolBar_Button9.Tag = ""
		Me._tlbToolBar_Button9.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._tlbToolBar_Button9.ToolTipText = "Delete"
		' 
		' _tlbToolBar_Button10
		' 
		Me._tlbToolBar_Button10.AutoSize = False
		Me._tlbToolBar_Button10.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._tlbToolBar_Button10.ImageKey = "Copy"
		Me._tlbToolBar_Button10.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._tlbToolBar_Button10.Name = ""
		Me._tlbToolBar_Button10.Size = New System.Drawing.Size(24, 22)
		Me._tlbToolBar_Button10.Tag = ""
		Me._tlbToolBar_Button10.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._tlbToolBar_Button10.ToolTipText = "Copy"
		' 
		' _tlbToolBar_Button11
		' 
		Me._tlbToolBar_Button11.AutoSize = False
		Me._tlbToolBar_Button11.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._tlbToolBar_Button11.ImageKey = "Print"
		Me._tlbToolBar_Button11.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._tlbToolBar_Button11.Name = ""
		Me._tlbToolBar_Button11.Size = New System.Drawing.Size(24, 22)
		Me._tlbToolBar_Button11.Tag = ""
		Me._tlbToolBar_Button11.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._tlbToolBar_Button11.ToolTipText = "Print"
		' 
		' chkIsDeleted
		' 
		Me.chkIsDeleted.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkIsDeleted.BackColor = System.Drawing.SystemColors.Control
		Me.chkIsDeleted.CausesValidation = True
		Me.chkIsDeleted.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkIsDeleted.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkIsDeleted.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkIsDeleted.Enabled = True
		Me.chkIsDeleted.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkIsDeleted.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkIsDeleted.Location = New System.Drawing.Point(200, 392)
		Me.chkIsDeleted.Name = "chkIsDeleted"
		Me.chkIsDeleted.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkIsDeleted.Size = New System.Drawing.Size(89, 17)
		Me.chkIsDeleted.TabIndex = 78
		Me.chkIsDeleted.TabStop = True
		Me.chkIsDeleted.Text = "IsDeleted"
		Me.chkIsDeleted.Visible = False
		' 
		' cboGroupDescription
		' 
		Me.cboGroupDescription.BackColor = System.Drawing.SystemColors.Window
		Me.cboGroupDescription.CausesValidation = True
		Me.cboGroupDescription.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboGroupDescription.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboGroupDescription.Enabled = True
		Me.cboGroupDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboGroupDescription.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboGroupDescription.IntegralHeight = True
		Me.cboGroupDescription.Location = New System.Drawing.Point(200, 56)
		Me.cboGroupDescription.Name = "cboGroupDescription"
		Me.cboGroupDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboGroupDescription.Size = New System.Drawing.Size(401, 21)
		Me.cboGroupDescription.Sorted = False
		Me.cboGroupDescription.TabIndex = 43
		Me.cboGroupDescription.TabStop = False
		Me.cboGroupDescription.Visible = True
		' 
		' cboGroups
		' 
		Me.cboGroups.BackColor = System.Drawing.SystemColors.Window
		Me.cboGroups.CausesValidation = True
		Me.cboGroups.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboGroups.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboGroups.Enabled = True
		Me.cboGroups.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboGroups.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboGroups.IntegralHeight = True
		Me.cboGroups.Location = New System.Drawing.Point(0, 56)
		Me.cboGroups.Name = "cboGroups"
		Me.cboGroups.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboGroups.Size = New System.Drawing.Size(193, 21)
		Me.cboGroups.Sorted = False
		Me.cboGroups.TabIndex = 42
		Me.cboGroups.TabStop = False
		Me.cboGroups.Visible = True
		Me.cboGroups.Items.AddRange(New Object(){"Components", "Processes", "Maps"})
		' 
		' panSliderBar
		' 
		Me.panSliderBar.BackColor = System.Drawing.SystemColors.Control
		Me.panSliderBar.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.panSliderBar.Controls.Add(Me.LinSliderBar)
		Me.panSliderBar.Cursor = System.Windows.Forms.Cursors.SizeWE
		Me.panSliderBar.Enabled = True
		Me.panSliderBar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.panSliderBar.ForeColor = System.Drawing.SystemColors.ControlText
		Me.panSliderBar.Location = New System.Drawing.Point(196, 80)
		Me.panSliderBar.Name = "panSliderBar"
		Me.panSliderBar.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.panSliderBar.Size = New System.Drawing.Size(4, 299)
		Me.panSliderBar.TabIndex = 41
		Me.panSliderBar.Visible = True
		' 
		' LinSliderBar
		' 
		Me.LinSliderBar.BackColor = System.Drawing.SystemColors.InactiveBorder
		Me.LinSliderBar.Location = New System.Drawing.Point(0, 0)
		Me.LinSliderBar.Name = "LinSliderBar"
		Me.LinSliderBar.Size = New System.Drawing.Size(1, 392)
		Me.LinSliderBar.Visible = True
		' 
		' cmdApply
		' 
		Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
		Me.cmdApply.CausesValidation = True
		Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdApply.Enabled = False
		Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdApply.Location = New System.Drawing.Point(528, 392)
		Me.cmdApply.Name = "cmdApply"
		Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdApply.Size = New System.Drawing.Size(73, 22)
		Me.cmdApply.TabIndex = 40
		Me.cmdApply.TabStop = False
		Me.cmdApply.Text = "&Apply"
		Me.cmdApply.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdExit
		' 
		Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdExit.CausesValidation = True
		Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdExit.Enabled = True
		Me.cmdExit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdExit.Location = New System.Drawing.Point(448, 392)
		Me.cmdExit.Name = "cmdExit"
		Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdExit.Size = New System.Drawing.Size(73, 22)
		Me.cmdExit.TabIndex = 39
		Me.cmdExit.TabStop = False
		Me.cmdExit.Text = "&Exit"
		Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' treMainData
		' 
		Me.treMainData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.treMainData.CausesValidation = True
		Me.treMainData.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.treMainData.HideSelection = False
		Me.treMainData.ImageList = imgTreeImages
		Me.treMainData.Indent = 30
		Me.treMainData.LabelEdit = False
		Me.treMainData.LabelEdit = True
		Me.treMainData.Location = New System.Drawing.Point(0, 80)
		Me.treMainData.Name = "treMainData"
		Me.treMainData.Size = New System.Drawing.Size(195, 307)
		Me.treMainData.TabIndex = 0
		Me.treMainData.TabStop = False
		' 
		' fraStep
		' 
		Me.fraStep.BackColor = System.Drawing.SystemColors.Control
		Me.fraStep.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.fraStep.CausesValidation = True
		Me.fraStep.Controls.Add(Me.cboPMNav_Steppmnav_map_id)
		Me.fraStep.Controls.Add(Me.cboPMNav_Steptask)
		Me.fraStep.Controls.Add(Me.chkPMNav_Stepis_logged)
		Me.fraStep.Controls.Add(Me.txtPMNav_Stepdescription)
		Me.fraStep.Controls.Add(Me.cboPMNav_Stepnavigate_status)
		Me.fraStep.Controls.Add(Me.cboPMNav_Stepsub_nav_map_id)
		Me.fraStep.Controls.Add(Me.cboPMNav_Stepok_action)
		Me.fraStep.Controls.Add(Me.cboPMNav_Stepcancel_action)
		Me.fraStep.Controls.Add(Me.chkPMNav_Stepis_hidden)
		Me.fraStep.Controls.Add(Me.cboPMNav_Steppmnav_component_id)
		Me.fraStep.Controls.Add(Me.txtPMNav_Stepcaption_id)
		Me.fraStep.Controls.Add(Me.cboPMNav_Stepok_nav_process_id)
		Me.fraStep.Controls.Add(Me.txtPMNav_Stepok_no_of_steps)
		Me.fraStep.Controls.Add(Me.cboPMNav_Stepcancel_nav_process_id)
		Me.fraStep.Controls.Add(Me.txtPMNav_Stepcancel_no_of_steps)
		Me.fraStep.Controls.Add(Me.lblStepDetails)
		Me.fraStep.Controls.Add(Me.lblMap)
		Me.fraStep.Controls.Add(Me.lblSComponent)
		Me.fraStep.Controls.Add(Me.lblSTask)
		Me.fraStep.Controls.Add(Me.lblSDescription)
		Me.fraStep.Controls.Add(Me.lblSNavStatus)
		Me.fraStep.Controls.Add(Me.lblSSubMap)
		Me.fraStep.Controls.Add(Me.lblSAction)
		Me.fraStep.Controls.Add(Me.lblSOK)
		Me.fraStep.Controls.Add(Me.lblSCancel)
		Me.fraStep.Cursor = System.Windows.Forms.Cursors.Default
		Me.fraStep.Dock = System.Windows.Forms.DockStyle.None
		Me.fraStep.Enabled = True
		Me.fraStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraStep.Location = New System.Drawing.Point(200, 80)
		Me.fraStep.Name = "fraStep"
		Me.fraStep.Size = New System.Drawing.Size(401, 307)
		Me.fraStep.TabIndex = 53
		Me.fraStep.TabStop = True
		Me.fraStep.Visible = False
		' 
		' cboPMNav_Steppmnav_map_id
		' 
		Me.cboPMNav_Steppmnav_map_id.BackColor = System.Drawing.SystemColors.Window
		Me.cboPMNav_Steppmnav_map_id.CausesValidation = True
		Me.cboPMNav_Steppmnav_map_id.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboPMNav_Steppmnav_map_id.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboPMNav_Steppmnav_map_id.Enabled = True
		Me.cboPMNav_Steppmnav_map_id.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboPMNav_Steppmnav_map_id.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboPMNav_Steppmnav_map_id.IntegralHeight = True
		Me.cboPMNav_Steppmnav_map_id.Location = New System.Drawing.Point(80, 32)
		Me.cboPMNav_Steppmnav_map_id.Name = "cboPMNav_Steppmnav_map_id"
		Me.cboPMNav_Steppmnav_map_id.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboPMNav_Steppmnav_map_id.Size = New System.Drawing.Size(313, 21)
		Me.cboPMNav_Steppmnav_map_id.Sorted = False
		Me.cboPMNav_Steppmnav_map_id.TabIndex = 13
		Me.cboPMNav_Steppmnav_map_id.TabStop = True
		Me.cboPMNav_Steppmnav_map_id.Tag = "Map"
		Me.cboPMNav_Steppmnav_map_id.Visible = True
		Me.cboPMNav_Steppmnav_map_id.Items.AddRange(New Object(){"Components", "Processes", "Maps", "Steps", "RoadMap"})
		' 
		' cboPMNav_Steptask
		' 
		Me.cboPMNav_Steptask.BackColor = System.Drawing.SystemColors.Window
		Me.cboPMNav_Steptask.CausesValidation = True
		Me.cboPMNav_Steptask.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboPMNav_Steptask.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboPMNav_Steptask.Enabled = True
		Me.cboPMNav_Steptask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboPMNav_Steptask.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboPMNav_Steptask.IntegralHeight = True
		Me.cboPMNav_Steptask.Location = New System.Drawing.Point(80, 160)
		Me.cboPMNav_Steptask.Name = "cboPMNav_Steptask"
		Me.cboPMNav_Steptask.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboPMNav_Steptask.Size = New System.Drawing.Size(241, 21)
		Me.cboPMNav_Steptask.Sorted = False
		Me.cboPMNav_Steptask.TabIndex = 17
		Me.cboPMNav_Steptask.TabStop = True
		Me.cboPMNav_Steptask.Tag = "Task"
		Me.cboPMNav_Steptask.Visible = True
		' 
		' chkPMNav_Stepis_logged
		' 
		Me.chkPMNav_Stepis_logged.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkPMNav_Stepis_logged.BackColor = System.Drawing.SystemColors.Control
		Me.chkPMNav_Stepis_logged.CausesValidation = True
		Me.chkPMNav_Stepis_logged.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkPMNav_Stepis_logged.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkPMNav_Stepis_logged.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkPMNav_Stepis_logged.Enabled = True
		Me.chkPMNav_Stepis_logged.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkPMNav_Stepis_logged.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkPMNav_Stepis_logged.Location = New System.Drawing.Point(336, 192)
		Me.chkPMNav_Stepis_logged.Name = "chkPMNav_Stepis_logged"
		Me.chkPMNav_Stepis_logged.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkPMNav_Stepis_logged.Size = New System.Drawing.Size(57, 17)
		Me.chkPMNav_Stepis_logged.TabIndex = 20
		Me.chkPMNav_Stepis_logged.TabStop = True
		Me.chkPMNav_Stepis_logged.Tag = "Logged"
		Me.chkPMNav_Stepis_logged.Text = "Logged"
		Me.chkPMNav_Stepis_logged.Visible = True
		' 
		' txtPMNav_Stepdescription
		' 
		Me.txtPMNav_Stepdescription.AcceptsReturn = True
		Me.txtPMNav_Stepdescription.AutoSize = False
		Me.txtPMNav_Stepdescription.BackColor = System.Drawing.SystemColors.Window
		Me.txtPMNav_Stepdescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPMNav_Stepdescription.CausesValidation = True
		Me.txtPMNav_Stepdescription.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPMNav_Stepdescription.Enabled = True
		Me.txtPMNav_Stepdescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPMNav_Stepdescription.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPMNav_Stepdescription.HideSelection = True
		Me.txtPMNav_Stepdescription.Location = New System.Drawing.Point(80, 128)
		Me.txtPMNav_Stepdescription.MaxLength = 0
		Me.txtPMNav_Stepdescription.Multiline = False
		Me.txtPMNav_Stepdescription.Name = "txtPMNav_Stepdescription"
		Me.txtPMNav_Stepdescription.ReadOnly = False
		Me.txtPMNav_Stepdescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPMNav_Stepdescription.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPMNav_Stepdescription.Size = New System.Drawing.Size(313, 19)
		Me.txtPMNav_Stepdescription.TabIndex = 16
		Me.txtPMNav_Stepdescription.TabStop = True
		Me.txtPMNav_Stepdescription.Tag = "Desc"
		Me.txtPMNav_Stepdescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPMNav_Stepdescription.Visible = True
		' 
		' cboPMNav_Stepnavigate_status
		' 
		Me.cboPMNav_Stepnavigate_status.BackColor = System.Drawing.SystemColors.Window
		Me.cboPMNav_Stepnavigate_status.CausesValidation = True
		Me.cboPMNav_Stepnavigate_status.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboPMNav_Stepnavigate_status.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboPMNav_Stepnavigate_status.Enabled = True
		Me.cboPMNav_Stepnavigate_status.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboPMNav_Stepnavigate_status.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboPMNav_Stepnavigate_status.IntegralHeight = True
		Me.cboPMNav_Stepnavigate_status.Location = New System.Drawing.Point(80, 192)
		Me.cboPMNav_Stepnavigate_status.Name = "cboPMNav_Stepnavigate_status"
		Me.cboPMNav_Stepnavigate_status.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboPMNav_Stepnavigate_status.Size = New System.Drawing.Size(241, 21)
		Me.cboPMNav_Stepnavigate_status.Sorted = False
		Me.cboPMNav_Stepnavigate_status.TabIndex = 19
		Me.cboPMNav_Stepnavigate_status.TabStop = True
		Me.cboPMNav_Stepnavigate_status.Tag = "Status"
		Me.cboPMNav_Stepnavigate_status.Visible = True
		' 
		' cboPMNav_Stepsub_nav_map_id
		' 
		Me.cboPMNav_Stepsub_nav_map_id.BackColor = System.Drawing.SystemColors.Window
		Me.cboPMNav_Stepsub_nav_map_id.CausesValidation = True
		Me.cboPMNav_Stepsub_nav_map_id.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboPMNav_Stepsub_nav_map_id.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboPMNav_Stepsub_nav_map_id.Enabled = True
		Me.cboPMNav_Stepsub_nav_map_id.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboPMNav_Stepsub_nav_map_id.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboPMNav_Stepsub_nav_map_id.IntegralHeight = True
		Me.cboPMNav_Stepsub_nav_map_id.Location = New System.Drawing.Point(80, 96)
		Me.cboPMNav_Stepsub_nav_map_id.Name = "cboPMNav_Stepsub_nav_map_id"
		Me.cboPMNav_Stepsub_nav_map_id.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboPMNav_Stepsub_nav_map_id.Size = New System.Drawing.Size(313, 21)
		Me.cboPMNav_Stepsub_nav_map_id.Sorted = False
		Me.cboPMNav_Stepsub_nav_map_id.TabIndex = 15
		Me.cboPMNav_Stepsub_nav_map_id.TabStop = True
		Me.cboPMNav_Stepsub_nav_map_id.Tag = "Sub Map"
		Me.cboPMNav_Stepsub_nav_map_id.Visible = True
		' 
		' cboPMNav_Stepok_action
		' 
		Me.cboPMNav_Stepok_action.BackColor = System.Drawing.SystemColors.Window
		Me.cboPMNav_Stepok_action.CausesValidation = True
		Me.cboPMNav_Stepok_action.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboPMNav_Stepok_action.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboPMNav_Stepok_action.Enabled = True
		Me.cboPMNav_Stepok_action.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboPMNav_Stepok_action.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboPMNav_Stepok_action.IntegralHeight = True
		Me.cboPMNav_Stepok_action.Location = New System.Drawing.Point(80, 248)
		Me.cboPMNav_Stepok_action.Name = "cboPMNav_Stepok_action"
		Me.cboPMNav_Stepok_action.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboPMNav_Stepok_action.Size = New System.Drawing.Size(137, 21)
		Me.cboPMNav_Stepok_action.Sorted = False
		Me.cboPMNav_Stepok_action.TabIndex = 21
		Me.cboPMNav_Stepok_action.TabStop = True
		Me.cboPMNav_Stepok_action.Tag = "OK"
		Me.cboPMNav_Stepok_action.Visible = True
		' 
		' cboPMNav_Stepcancel_action
		' 
		Me.cboPMNav_Stepcancel_action.BackColor = System.Drawing.SystemColors.Window
		Me.cboPMNav_Stepcancel_action.CausesValidation = True
		Me.cboPMNav_Stepcancel_action.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboPMNav_Stepcancel_action.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboPMNav_Stepcancel_action.Enabled = True
		Me.cboPMNav_Stepcancel_action.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboPMNav_Stepcancel_action.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboPMNav_Stepcancel_action.IntegralHeight = True
		Me.cboPMNav_Stepcancel_action.Location = New System.Drawing.Point(80, 272)
		Me.cboPMNav_Stepcancel_action.Name = "cboPMNav_Stepcancel_action"
		Me.cboPMNav_Stepcancel_action.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboPMNav_Stepcancel_action.Size = New System.Drawing.Size(137, 21)
		Me.cboPMNav_Stepcancel_action.Sorted = False
		Me.cboPMNav_Stepcancel_action.TabIndex = 24
		Me.cboPMNav_Stepcancel_action.TabStop = True
		Me.cboPMNav_Stepcancel_action.Tag = "Cancel"
		Me.cboPMNav_Stepcancel_action.Visible = True
		' 
		' chkPMNav_Stepis_hidden
		' 
		Me.chkPMNav_Stepis_hidden.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkPMNav_Stepis_hidden.BackColor = System.Drawing.SystemColors.Control
		Me.chkPMNav_Stepis_hidden.CausesValidation = True
		Me.chkPMNav_Stepis_hidden.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkPMNav_Stepis_hidden.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkPMNav_Stepis_hidden.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkPMNav_Stepis_hidden.Enabled = True
		Me.chkPMNav_Stepis_hidden.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkPMNav_Stepis_hidden.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkPMNav_Stepis_hidden.Location = New System.Drawing.Point(336, 160)
		Me.chkPMNav_Stepis_hidden.Name = "chkPMNav_Stepis_hidden"
		Me.chkPMNav_Stepis_hidden.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkPMNav_Stepis_hidden.Size = New System.Drawing.Size(57, 17)
		Me.chkPMNav_Stepis_hidden.TabIndex = 18
		Me.chkPMNav_Stepis_hidden.TabStop = True
		Me.chkPMNav_Stepis_hidden.Tag = "Hidden"
		Me.chkPMNav_Stepis_hidden.Text = "Hidden"
		Me.chkPMNav_Stepis_hidden.Visible = True
		' 
		' cboPMNav_Steppmnav_component_id
		' 
		Me.cboPMNav_Steppmnav_component_id.BackColor = System.Drawing.SystemColors.Window
		Me.cboPMNav_Steppmnav_component_id.CausesValidation = True
		Me.cboPMNav_Steppmnav_component_id.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboPMNav_Steppmnav_component_id.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboPMNav_Steppmnav_component_id.Enabled = True
		Me.cboPMNav_Steppmnav_component_id.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboPMNav_Steppmnav_component_id.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboPMNav_Steppmnav_component_id.IntegralHeight = True
		Me.cboPMNav_Steppmnav_component_id.Location = New System.Drawing.Point(80, 64)
		Me.cboPMNav_Steppmnav_component_id.Name = "cboPMNav_Steppmnav_component_id"
		Me.cboPMNav_Steppmnav_component_id.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboPMNav_Steppmnav_component_id.Size = New System.Drawing.Size(313, 21)
		Me.cboPMNav_Steppmnav_component_id.Sorted = False
		Me.cboPMNav_Steppmnav_component_id.TabIndex = 14
		Me.cboPMNav_Steppmnav_component_id.TabStop = True
		Me.cboPMNav_Steppmnav_component_id.Tag = "Component"
		Me.cboPMNav_Steppmnav_component_id.Visible = True
		' 
		' txtPMNav_Stepcaption_id
		' 
		Me.txtPMNav_Stepcaption_id.AcceptsReturn = True
		Me.txtPMNav_Stepcaption_id.AutoSize = False
		Me.txtPMNav_Stepcaption_id.BackColor = System.Drawing.SystemColors.Window
		Me.txtPMNav_Stepcaption_id.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPMNav_Stepcaption_id.CausesValidation = True
		Me.txtPMNav_Stepcaption_id.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPMNav_Stepcaption_id.Enabled = True
		Me.txtPMNav_Stepcaption_id.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPMNav_Stepcaption_id.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPMNav_Stepcaption_id.HideSelection = True
		Me.txtPMNav_Stepcaption_id.Location = New System.Drawing.Point(80, 128)
		Me.txtPMNav_Stepcaption_id.MaxLength = 0
		Me.txtPMNav_Stepcaption_id.Multiline = False
		Me.txtPMNav_Stepcaption_id.Name = "txtPMNav_Stepcaption_id"
		Me.txtPMNav_Stepcaption_id.ReadOnly = False
		Me.txtPMNav_Stepcaption_id.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPMNav_Stepcaption_id.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPMNav_Stepcaption_id.Size = New System.Drawing.Size(313, 19)
		Me.txtPMNav_Stepcaption_id.TabIndex = 27
		Me.txtPMNav_Stepcaption_id.TabStop = True
		Me.txtPMNav_Stepcaption_id.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPMNav_Stepcaption_id.Visible = False
		' 
		' cboPMNav_Stepok_nav_process_id
		' 
		Me.cboPMNav_Stepok_nav_process_id.BackColor = System.Drawing.SystemColors.Window
		Me.cboPMNav_Stepok_nav_process_id.CausesValidation = True
		Me.cboPMNav_Stepok_nav_process_id.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboPMNav_Stepok_nav_process_id.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboPMNav_Stepok_nav_process_id.Enabled = True
		Me.cboPMNav_Stepok_nav_process_id.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboPMNav_Stepok_nav_process_id.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboPMNav_Stepok_nav_process_id.IntegralHeight = True
		Me.cboPMNav_Stepok_nav_process_id.Location = New System.Drawing.Point(224, 248)
		Me.cboPMNav_Stepok_nav_process_id.Name = "cboPMNav_Stepok_nav_process_id"
		Me.cboPMNav_Stepok_nav_process_id.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboPMNav_Stepok_nav_process_id.Size = New System.Drawing.Size(169, 21)
		Me.cboPMNav_Stepok_nav_process_id.Sorted = False
		Me.cboPMNav_Stepok_nav_process_id.TabIndex = 23
		Me.cboPMNav_Stepok_nav_process_id.TabStop = True
		Me.cboPMNav_Stepok_nav_process_id.Tag = "OK2"
		Me.cboPMNav_Stepok_nav_process_id.Visible = False
		' 
		' txtPMNav_Stepok_no_of_steps
		' 
		Me.txtPMNav_Stepok_no_of_steps.AcceptsReturn = True
		Me.txtPMNav_Stepok_no_of_steps.AutoSize = False
		Me.txtPMNav_Stepok_no_of_steps.BackColor = System.Drawing.SystemColors.Window
		Me.txtPMNav_Stepok_no_of_steps.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPMNav_Stepok_no_of_steps.CausesValidation = True
		Me.txtPMNav_Stepok_no_of_steps.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPMNav_Stepok_no_of_steps.Enabled = True
		Me.txtPMNav_Stepok_no_of_steps.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPMNav_Stepok_no_of_steps.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPMNav_Stepok_no_of_steps.HideSelection = True
		Me.txtPMNav_Stepok_no_of_steps.Location = New System.Drawing.Point(224, 248)
		Me.txtPMNav_Stepok_no_of_steps.MaxLength = 0
		Me.txtPMNav_Stepok_no_of_steps.Multiline = False
		Me.txtPMNav_Stepok_no_of_steps.Name = "txtPMNav_Stepok_no_of_steps"
		Me.txtPMNav_Stepok_no_of_steps.ReadOnly = False
		Me.txtPMNav_Stepok_no_of_steps.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPMNav_Stepok_no_of_steps.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPMNav_Stepok_no_of_steps.Size = New System.Drawing.Size(169, 19)
		Me.txtPMNav_Stepok_no_of_steps.TabIndex = 22
		Me.txtPMNav_Stepok_no_of_steps.TabStop = True
		Me.txtPMNav_Stepok_no_of_steps.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPMNav_Stepok_no_of_steps.Visible = False
		' 
		' cboPMNav_Stepcancel_nav_process_id
		' 
		Me.cboPMNav_Stepcancel_nav_process_id.BackColor = System.Drawing.SystemColors.Window
		Me.cboPMNav_Stepcancel_nav_process_id.CausesValidation = True
		Me.cboPMNav_Stepcancel_nav_process_id.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboPMNav_Stepcancel_nav_process_id.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboPMNav_Stepcancel_nav_process_id.Enabled = True
		Me.cboPMNav_Stepcancel_nav_process_id.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboPMNav_Stepcancel_nav_process_id.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboPMNav_Stepcancel_nav_process_id.IntegralHeight = True
		Me.cboPMNav_Stepcancel_nav_process_id.Location = New System.Drawing.Point(224, 272)
		Me.cboPMNav_Stepcancel_nav_process_id.Name = "cboPMNav_Stepcancel_nav_process_id"
		Me.cboPMNav_Stepcancel_nav_process_id.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboPMNav_Stepcancel_nav_process_id.Size = New System.Drawing.Size(169, 21)
		Me.cboPMNav_Stepcancel_nav_process_id.Sorted = False
		Me.cboPMNav_Stepcancel_nav_process_id.TabIndex = 26
		Me.cboPMNav_Stepcancel_nav_process_id.TabStop = True
		Me.cboPMNav_Stepcancel_nav_process_id.Tag = "Cancel2"
		Me.cboPMNav_Stepcancel_nav_process_id.Visible = False
		' 
		' txtPMNav_Stepcancel_no_of_steps
		' 
		Me.txtPMNav_Stepcancel_no_of_steps.AcceptsReturn = True
		Me.txtPMNav_Stepcancel_no_of_steps.AutoSize = False
		Me.txtPMNav_Stepcancel_no_of_steps.BackColor = System.Drawing.SystemColors.Window
		Me.txtPMNav_Stepcancel_no_of_steps.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPMNav_Stepcancel_no_of_steps.CausesValidation = True
		Me.txtPMNav_Stepcancel_no_of_steps.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPMNav_Stepcancel_no_of_steps.Enabled = True
		Me.txtPMNav_Stepcancel_no_of_steps.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPMNav_Stepcancel_no_of_steps.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPMNav_Stepcancel_no_of_steps.HideSelection = True
		Me.txtPMNav_Stepcancel_no_of_steps.Location = New System.Drawing.Point(224, 272)
		Me.txtPMNav_Stepcancel_no_of_steps.MaxLength = 0
		Me.txtPMNav_Stepcancel_no_of_steps.Multiline = False
		Me.txtPMNav_Stepcancel_no_of_steps.Name = "txtPMNav_Stepcancel_no_of_steps"
		Me.txtPMNav_Stepcancel_no_of_steps.ReadOnly = False
		Me.txtPMNav_Stepcancel_no_of_steps.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPMNav_Stepcancel_no_of_steps.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPMNav_Stepcancel_no_of_steps.Size = New System.Drawing.Size(167, 19)
		Me.txtPMNav_Stepcancel_no_of_steps.TabIndex = 25
		Me.txtPMNav_Stepcancel_no_of_steps.TabStop = True
		Me.txtPMNav_Stepcancel_no_of_steps.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPMNav_Stepcancel_no_of_steps.Visible = False
		' 
		' lblStepDetails
		' 
		Me.lblStepDetails.AutoSize = False
		Me.lblStepDetails.BackColor = System.Drawing.SystemColors.Control
		Me.lblStepDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblStepDetails.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblStepDetails.Enabled = True
		Me.lblStepDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblStepDetails.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblStepDetails.Location = New System.Drawing.Point(8, 8)
		Me.lblStepDetails.Name = "lblStepDetails"
		Me.lblStepDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblStepDetails.Size = New System.Drawing.Size(385, 17)
		Me.lblStepDetails.TabIndex = 74
		Me.lblStepDetails.Text = "Step Details"
		Me.lblStepDetails.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me.lblStepDetails.UseMnemonic = True
		Me.lblStepDetails.Visible = True
		' 
		' lblMap
		' 
		Me.lblMap.AutoSize = False
		Me.lblMap.BackColor = System.Drawing.SystemColors.Control
		Me.lblMap.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMap.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMap.Enabled = True
		Me.lblMap.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMap.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMap.Location = New System.Drawing.Point(16, 32)
		Me.lblMap.Name = "lblMap"
		Me.lblMap.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMap.Size = New System.Drawing.Size(89, 17)
		Me.lblMap.TabIndex = 72
		Me.lblMap.Text = "Map"
		Me.lblMap.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMap.UseMnemonic = True
		Me.lblMap.Visible = True
		' 
		' lblSComponent
		' 
		Me.lblSComponent.AutoSize = False
		Me.lblSComponent.BackColor = System.Drawing.SystemColors.Control
		Me.lblSComponent.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSComponent.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSComponent.Enabled = True
		Me.lblSComponent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSComponent.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSComponent.Location = New System.Drawing.Point(16, 64)
		Me.lblSComponent.Name = "lblSComponent"
		Me.lblSComponent.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSComponent.Size = New System.Drawing.Size(89, 17)
		Me.lblSComponent.TabIndex = 71
		Me.lblSComponent.Text = "Component"
		Me.lblSComponent.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblSComponent.UseMnemonic = True
		Me.lblSComponent.Visible = True
		' 
		' lblSTask
		' 
		Me.lblSTask.AutoSize = False
		Me.lblSTask.BackColor = System.Drawing.SystemColors.Control
		Me.lblSTask.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSTask.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSTask.Enabled = True
		Me.lblSTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSTask.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSTask.Location = New System.Drawing.Point(16, 160)
		Me.lblSTask.Name = "lblSTask"
		Me.lblSTask.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSTask.Size = New System.Drawing.Size(89, 17)
		Me.lblSTask.TabIndex = 60
		Me.lblSTask.Text = "Task"
		Me.lblSTask.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblSTask.UseMnemonic = True
		Me.lblSTask.Visible = True
		' 
		' lblSDescription
		' 
		Me.lblSDescription.AutoSize = False
		Me.lblSDescription.BackColor = System.Drawing.SystemColors.Control
		Me.lblSDescription.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSDescription.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSDescription.Enabled = True
		Me.lblSDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSDescription.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSDescription.Location = New System.Drawing.Point(16, 128)
		Me.lblSDescription.Name = "lblSDescription"
		Me.lblSDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSDescription.Size = New System.Drawing.Size(65, 17)
		Me.lblSDescription.TabIndex = 59
		Me.lblSDescription.Text = "Description"
		Me.lblSDescription.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblSDescription.UseMnemonic = True
		Me.lblSDescription.Visible = True
		' 
		' lblSNavStatus
		' 
		Me.lblSNavStatus.AutoSize = False
		Me.lblSNavStatus.BackColor = System.Drawing.SystemColors.Control
		Me.lblSNavStatus.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSNavStatus.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSNavStatus.Enabled = True
		Me.lblSNavStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSNavStatus.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSNavStatus.Location = New System.Drawing.Point(16, 192)
		Me.lblSNavStatus.Name = "lblSNavStatus"
		Me.lblSNavStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSNavStatus.Size = New System.Drawing.Size(89, 17)
		Me.lblSNavStatus.TabIndex = 58
		Me.lblSNavStatus.Text = "Nav. Status"
		Me.lblSNavStatus.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblSNavStatus.UseMnemonic = True
		Me.lblSNavStatus.Visible = True
		' 
		' lblSSubMap
		' 
		Me.lblSSubMap.AutoSize = False
		Me.lblSSubMap.BackColor = System.Drawing.SystemColors.Control
		Me.lblSSubMap.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSSubMap.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSSubMap.Enabled = True
		Me.lblSSubMap.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSSubMap.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSSubMap.Location = New System.Drawing.Point(16, 96)
		Me.lblSSubMap.Name = "lblSSubMap"
		Me.lblSSubMap.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSSubMap.Size = New System.Drawing.Size(89, 17)
		Me.lblSSubMap.TabIndex = 57
		Me.lblSSubMap.Text = "Sub Map"
		Me.lblSSubMap.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblSSubMap.UseMnemonic = True
		Me.lblSSubMap.Visible = True
		' 
		' lblSAction
		' 
		Me.lblSAction.AutoSize = False
		Me.lblSAction.BackColor = System.Drawing.SystemColors.Control
		Me.lblSAction.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSAction.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSAction.Enabled = True
		Me.lblSAction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSAction.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSAction.Location = New System.Drawing.Point(80, 232)
		Me.lblSAction.Name = "lblSAction"
		Me.lblSAction.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSAction.Size = New System.Drawing.Size(113, 17)
		Me.lblSAction.TabIndex = 56
		Me.lblSAction.Text = "Action"
		Me.lblSAction.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblSAction.UseMnemonic = True
		Me.lblSAction.Visible = True
		' 
		' lblSOK
		' 
		Me.lblSOK.AutoSize = False
		Me.lblSOK.BackColor = System.Drawing.SystemColors.Control
		Me.lblSOK.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSOK.Enabled = True
		Me.lblSOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSOK.Location = New System.Drawing.Point(16, 248)
		Me.lblSOK.Name = "lblSOK"
		Me.lblSOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSOK.Size = New System.Drawing.Size(41, 17)
		Me.lblSOK.TabIndex = 55
		Me.lblSOK.Text = "OK"
		Me.lblSOK.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblSOK.UseMnemonic = True
		Me.lblSOK.Visible = True
		' 
		' lblSCancel
		' 
		Me.lblSCancel.AutoSize = False
		Me.lblSCancel.BackColor = System.Drawing.SystemColors.Control
		Me.lblSCancel.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSCancel.Enabled = True
		Me.lblSCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSCancel.Location = New System.Drawing.Point(16, 272)
		Me.lblSCancel.Name = "lblSCancel"
		Me.lblSCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSCancel.Size = New System.Drawing.Size(41, 17)
		Me.lblSCancel.TabIndex = 54
		Me.lblSCancel.Text = "Cancel"
		Me.lblSCancel.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblSCancel.UseMnemonic = True
		Me.lblSCancel.Visible = True
		' 
		' fraProcess
		' 
		Me.fraProcess.BackColor = System.Drawing.SystemColors.Control
		Me.fraProcess.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.fraProcess.CausesValidation = True
		Me.fraProcess.Controls.Add(Me.cboPMNav_Processis_user_driven)
		Me.fraProcess.Controls.Add(Me.cboPMNav_Processtransaction_type_id)
		Me.fraProcess.Controls.Add(Me.txtPMNav_Processeffective_date)
		Me.fraProcess.Controls.Add(Me.chkPMNav_Processis_logged)
		Me.fraProcess.Controls.Add(Me.cboPMNav_Processpmproduct_id)
		Me.fraProcess.Controls.Add(Me.cboPMNav_Processpmproc_lock_group_id)
		Me.fraProcess.Controls.Add(Me.cboPMNav_Processprocess_mode)
		Me.fraProcess.Controls.Add(Me.txtPMNav_Processcode)
		Me.fraProcess.Controls.Add(Me.cboPMNav_Processstart_nav_map_id)
		Me.fraProcess.Controls.Add(Me.txtPMNav_Processdescription)
		Me.fraProcess.Controls.Add(Me.txtPMNav_Processcaption_id)
		Me.fraProcess.Controls.Add(Me.lblPNavMode)
		Me.fraProcess.Controls.Add(Me.lblPEffectiveDate)
		Me.fraProcess.Controls.Add(Me.lblPTransType)
		Me.fraProcess.Controls.Add(Me.lblPDescription)
		Me.fraProcess.Controls.Add(Me.lblPStartNavMap)
		Me.fraProcess.Controls.Add(Me.lblPProduct)
		Me.fraProcess.Controls.Add(Me.lblPProcessLockGroup)
		Me.fraProcess.Controls.Add(Me.lblPProcessMode)
		Me.fraProcess.Controls.Add(Me.lblPCode)
		Me.fraProcess.Controls.Add(Me.lblProcessDetails)
		Me.fraProcess.Cursor = System.Windows.Forms.Cursors.Default
		Me.fraProcess.Dock = System.Windows.Forms.DockStyle.None
		Me.fraProcess.Enabled = True
		Me.fraProcess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraProcess.Location = New System.Drawing.Point(200, 80)
		Me.fraProcess.Name = "fraProcess"
		Me.fraProcess.Size = New System.Drawing.Size(401, 307)
		Me.fraProcess.TabIndex = 44
		Me.fraProcess.TabStop = True
		Me.fraProcess.Visible = False
		' 
		' cboPMNav_Processis_user_driven
		' 
		Me.cboPMNav_Processis_user_driven.BackColor = System.Drawing.SystemColors.Window
		Me.cboPMNav_Processis_user_driven.CausesValidation = True
		Me.cboPMNav_Processis_user_driven.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboPMNav_Processis_user_driven.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboPMNav_Processis_user_driven.Enabled = True
		Me.cboPMNav_Processis_user_driven.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboPMNav_Processis_user_driven.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboPMNav_Processis_user_driven.IntegralHeight = True
		Me.cboPMNav_Processis_user_driven.Location = New System.Drawing.Point(112, 224)
		Me.cboPMNav_Processis_user_driven.Name = "cboPMNav_Processis_user_driven"
		Me.cboPMNav_Processis_user_driven.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboPMNav_Processis_user_driven.Size = New System.Drawing.Size(177, 21)
		Me.cboPMNav_Processis_user_driven.Sorted = False
		Me.cboPMNav_Processis_user_driven.TabIndex = 35
		Me.cboPMNav_Processis_user_driven.TabStop = True
		Me.cboPMNav_Processis_user_driven.Visible = True
		' 
		' cboPMNav_Processtransaction_type_id
		' 
		Me.cboPMNav_Processtransaction_type_id.BackColor = System.Drawing.SystemColors.Window
		Me.cboPMNav_Processtransaction_type_id.CausesValidation = True
		Me.cboPMNav_Processtransaction_type_id.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboPMNav_Processtransaction_type_id.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboPMNav_Processtransaction_type_id.Enabled = True
		Me.cboPMNav_Processtransaction_type_id.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboPMNav_Processtransaction_type_id.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboPMNav_Processtransaction_type_id.IntegralHeight = True
		Me.cboPMNav_Processtransaction_type_id.Location = New System.Drawing.Point(112, 160)
		Me.cboPMNav_Processtransaction_type_id.Name = "cboPMNav_Processtransaction_type_id"
		Me.cboPMNav_Processtransaction_type_id.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboPMNav_Processtransaction_type_id.Size = New System.Drawing.Size(265, 21)
		Me.cboPMNav_Processtransaction_type_id.Sorted = False
		Me.cboPMNav_Processtransaction_type_id.TabIndex = 33
		Me.cboPMNav_Processtransaction_type_id.TabStop = True
		Me.cboPMNav_Processtransaction_type_id.Visible = True
		' 
		' txtPMNav_Processeffective_date
		' 
		Me.txtPMNav_Processeffective_date.AcceptsReturn = True
		Me.txtPMNav_Processeffective_date.AutoSize = False
		Me.txtPMNav_Processeffective_date.BackColor = System.Drawing.SystemColors.Window
		Me.txtPMNav_Processeffective_date.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPMNav_Processeffective_date.CausesValidation = True
		Me.txtPMNav_Processeffective_date.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPMNav_Processeffective_date.Enabled = True
		Me.txtPMNav_Processeffective_date.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPMNav_Processeffective_date.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPMNav_Processeffective_date.HideSelection = True
		Me.txtPMNav_Processeffective_date.Location = New System.Drawing.Point(112, 288)
		Me.txtPMNav_Processeffective_date.MaxLength = 0
		Me.txtPMNav_Processeffective_date.Multiline = False
		Me.txtPMNav_Processeffective_date.Name = "txtPMNav_Processeffective_date"
		Me.txtPMNav_Processeffective_date.ReadOnly = False
		Me.txtPMNav_Processeffective_date.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPMNav_Processeffective_date.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPMNav_Processeffective_date.Size = New System.Drawing.Size(265, 19)
		Me.txtPMNav_Processeffective_date.TabIndex = 38
		Me.txtPMNav_Processeffective_date.TabStop = True
		Me.txtPMNav_Processeffective_date.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPMNav_Processeffective_date.Visible = True
		' 
		' chkPMNav_Processis_logged
		' 
		Me.chkPMNav_Processis_logged.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkPMNav_Processis_logged.BackColor = System.Drawing.SystemColors.Control
		Me.chkPMNav_Processis_logged.CausesValidation = True
		Me.chkPMNav_Processis_logged.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkPMNav_Processis_logged.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkPMNav_Processis_logged.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkPMNav_Processis_logged.Enabled = True
		Me.chkPMNav_Processis_logged.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkPMNav_Processis_logged.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkPMNav_Processis_logged.Location = New System.Drawing.Point(304, 224)
		Me.chkPMNav_Processis_logged.Name = "chkPMNav_Processis_logged"
		Me.chkPMNav_Processis_logged.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkPMNav_Processis_logged.Size = New System.Drawing.Size(73, 17)
		Me.chkPMNav_Processis_logged.TabIndex = 36
		Me.chkPMNav_Processis_logged.TabStop = True
		Me.chkPMNav_Processis_logged.Text = "Is Logged"
		Me.chkPMNav_Processis_logged.Visible = True
		' 
		' cboPMNav_Processpmproduct_id
		' 
		Me.cboPMNav_Processpmproduct_id.BackColor = System.Drawing.SystemColors.Window
		Me.cboPMNav_Processpmproduct_id.CausesValidation = True
		Me.cboPMNav_Processpmproduct_id.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboPMNav_Processpmproduct_id.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboPMNav_Processpmproduct_id.Enabled = True
		Me.cboPMNav_Processpmproduct_id.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboPMNav_Processpmproduct_id.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboPMNav_Processpmproduct_id.IntegralHeight = True
		Me.cboPMNav_Processpmproduct_id.Location = New System.Drawing.Point(112, 32)
		Me.cboPMNav_Processpmproduct_id.Name = "cboPMNav_Processpmproduct_id"
		Me.cboPMNav_Processpmproduct_id.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboPMNav_Processpmproduct_id.Size = New System.Drawing.Size(265, 21)
		Me.cboPMNav_Processpmproduct_id.Sorted = False
		Me.cboPMNav_Processpmproduct_id.TabIndex = 28
		Me.cboPMNav_Processpmproduct_id.TabStop = True
		Me.cboPMNav_Processpmproduct_id.Visible = True
		' 
		' cboPMNav_Processpmproc_lock_group_id
		' 
		Me.cboPMNav_Processpmproc_lock_group_id.BackColor = System.Drawing.SystemColors.Window
		Me.cboPMNav_Processpmproc_lock_group_id.CausesValidation = True
		Me.cboPMNav_Processpmproc_lock_group_id.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboPMNav_Processpmproc_lock_group_id.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboPMNav_Processpmproc_lock_group_id.Enabled = True
		Me.cboPMNav_Processpmproc_lock_group_id.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboPMNav_Processpmproc_lock_group_id.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboPMNav_Processpmproc_lock_group_id.IntegralHeight = True
		Me.cboPMNav_Processpmproc_lock_group_id.Location = New System.Drawing.Point(112, 128)
		Me.cboPMNav_Processpmproc_lock_group_id.Name = "cboPMNav_Processpmproc_lock_group_id"
		Me.cboPMNav_Processpmproc_lock_group_id.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboPMNav_Processpmproc_lock_group_id.Size = New System.Drawing.Size(265, 21)
		Me.cboPMNav_Processpmproc_lock_group_id.Sorted = False
		Me.cboPMNav_Processpmproc_lock_group_id.TabIndex = 32
		Me.cboPMNav_Processpmproc_lock_group_id.TabStop = True
		Me.cboPMNav_Processpmproc_lock_group_id.Visible = True
		' 
		' cboPMNav_Processprocess_mode
		' 
		Me.cboPMNav_Processprocess_mode.BackColor = System.Drawing.SystemColors.Window
		Me.cboPMNav_Processprocess_mode.CausesValidation = True
		Me.cboPMNav_Processprocess_mode.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboPMNav_Processprocess_mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboPMNav_Processprocess_mode.Enabled = True
		Me.cboPMNav_Processprocess_mode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboPMNav_Processprocess_mode.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboPMNav_Processprocess_mode.IntegralHeight = True
		Me.cboPMNav_Processprocess_mode.Location = New System.Drawing.Point(112, 192)
		Me.cboPMNav_Processprocess_mode.Name = "cboPMNav_Processprocess_mode"
		Me.cboPMNav_Processprocess_mode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboPMNav_Processprocess_mode.Size = New System.Drawing.Size(265, 21)
		Me.cboPMNav_Processprocess_mode.Sorted = False
		Me.cboPMNav_Processprocess_mode.TabIndex = 34
		Me.cboPMNav_Processprocess_mode.TabStop = True
		Me.cboPMNav_Processprocess_mode.Visible = True
		' 
		' txtPMNav_Processcode
		' 
		Me.txtPMNav_Processcode.AcceptsReturn = True
		Me.txtPMNav_Processcode.AutoSize = False
		Me.txtPMNav_Processcode.BackColor = System.Drawing.SystemColors.Window
		Me.txtPMNav_Processcode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPMNav_Processcode.CausesValidation = True
		Me.txtPMNav_Processcode.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPMNav_Processcode.Enabled = True
		Me.txtPMNav_Processcode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPMNav_Processcode.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPMNav_Processcode.HideSelection = True
		Me.txtPMNav_Processcode.Location = New System.Drawing.Point(112, 64)
		Me.txtPMNav_Processcode.MaxLength = 0
		Me.txtPMNav_Processcode.Multiline = False
		Me.txtPMNav_Processcode.Name = "txtPMNav_Processcode"
		Me.txtPMNav_Processcode.ReadOnly = False
		Me.txtPMNav_Processcode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPMNav_Processcode.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPMNav_Processcode.Size = New System.Drawing.Size(265, 19)
		Me.txtPMNav_Processcode.TabIndex = 29
		Me.txtPMNav_Processcode.TabStop = True
		Me.txtPMNav_Processcode.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPMNav_Processcode.Visible = True
		' 
		' cboPMNav_Processstart_nav_map_id
		' 
		Me.cboPMNav_Processstart_nav_map_id.BackColor = System.Drawing.SystemColors.Window
		Me.cboPMNav_Processstart_nav_map_id.CausesValidation = True
		Me.cboPMNav_Processstart_nav_map_id.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboPMNav_Processstart_nav_map_id.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboPMNav_Processstart_nav_map_id.Enabled = True
		Me.cboPMNav_Processstart_nav_map_id.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboPMNav_Processstart_nav_map_id.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboPMNav_Processstart_nav_map_id.IntegralHeight = True
		Me.cboPMNav_Processstart_nav_map_id.Location = New System.Drawing.Point(112, 256)
		Me.cboPMNav_Processstart_nav_map_id.Name = "cboPMNav_Processstart_nav_map_id"
		Me.cboPMNav_Processstart_nav_map_id.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboPMNav_Processstart_nav_map_id.Size = New System.Drawing.Size(265, 21)
		Me.cboPMNav_Processstart_nav_map_id.Sorted = False
		Me.cboPMNav_Processstart_nav_map_id.TabIndex = 37
		Me.cboPMNav_Processstart_nav_map_id.TabStop = True
		Me.cboPMNav_Processstart_nav_map_id.Visible = True
		' 
		' txtPMNav_Processdescription
		' 
		Me.txtPMNav_Processdescription.AcceptsReturn = True
		Me.txtPMNav_Processdescription.AutoSize = False
		Me.txtPMNav_Processdescription.BackColor = System.Drawing.SystemColors.Window
		Me.txtPMNav_Processdescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPMNav_Processdescription.CausesValidation = True
		Me.txtPMNav_Processdescription.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPMNav_Processdescription.Enabled = True
		Me.txtPMNav_Processdescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPMNav_Processdescription.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPMNav_Processdescription.HideSelection = True
		Me.txtPMNav_Processdescription.Location = New System.Drawing.Point(112, 96)
		Me.txtPMNav_Processdescription.MaxLength = 0
		Me.txtPMNav_Processdescription.Multiline = False
		Me.txtPMNav_Processdescription.Name = "txtPMNav_Processdescription"
		Me.txtPMNav_Processdescription.ReadOnly = False
		Me.txtPMNav_Processdescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPMNav_Processdescription.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPMNav_Processdescription.Size = New System.Drawing.Size(265, 19)
		Me.txtPMNav_Processdescription.TabIndex = 31
		Me.txtPMNav_Processdescription.TabStop = True
		Me.txtPMNav_Processdescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPMNav_Processdescription.Visible = True
		' 
		' txtPMNav_Processcaption_id
		' 
		Me.txtPMNav_Processcaption_id.AcceptsReturn = True
		Me.txtPMNav_Processcaption_id.AutoSize = False
		Me.txtPMNav_Processcaption_id.BackColor = System.Drawing.SystemColors.Window
		Me.txtPMNav_Processcaption_id.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPMNav_Processcaption_id.CausesValidation = True
		Me.txtPMNav_Processcaption_id.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPMNav_Processcaption_id.Enabled = True
		Me.txtPMNav_Processcaption_id.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPMNav_Processcaption_id.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPMNav_Processcaption_id.HideSelection = True
		Me.txtPMNav_Processcaption_id.Location = New System.Drawing.Point(112, 96)
		Me.txtPMNav_Processcaption_id.MaxLength = 0
		Me.txtPMNav_Processcaption_id.Multiline = False
		Me.txtPMNav_Processcaption_id.Name = "txtPMNav_Processcaption_id"
		Me.txtPMNav_Processcaption_id.ReadOnly = False
		Me.txtPMNav_Processcaption_id.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPMNav_Processcaption_id.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPMNav_Processcaption_id.Size = New System.Drawing.Size(265, 19)
		Me.txtPMNav_Processcaption_id.TabIndex = 30
		Me.txtPMNav_Processcaption_id.TabStop = True
		Me.txtPMNav_Processcaption_id.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPMNav_Processcaption_id.Visible = False
		' 
		' lblPNavMode
		' 
		Me.lblPNavMode.AutoSize = False
		Me.lblPNavMode.BackColor = System.Drawing.SystemColors.Control
		Me.lblPNavMode.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPNavMode.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPNavMode.Enabled = True
		Me.lblPNavMode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPNavMode.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPNavMode.Location = New System.Drawing.Point(16, 224)
		Me.lblPNavMode.Name = "lblPNavMode"
		Me.lblPNavMode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPNavMode.Size = New System.Drawing.Size(89, 17)
		Me.lblPNavMode.TabIndex = 79
		Me.lblPNavMode.Text = "Navigator Mode"
		Me.lblPNavMode.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPNavMode.UseMnemonic = True
		Me.lblPNavMode.Visible = True
		' 
		' lblPEffectiveDate
		' 
		Me.lblPEffectiveDate.AutoSize = False
		Me.lblPEffectiveDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblPEffectiveDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPEffectiveDate.Enabled = True
		Me.lblPEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPEffectiveDate.Location = New System.Drawing.Point(16, 288)
		Me.lblPEffectiveDate.Name = "lblPEffectiveDate"
		Me.lblPEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPEffectiveDate.Size = New System.Drawing.Size(89, 17)
		Me.lblPEffectiveDate.TabIndex = 52
		Me.lblPEffectiveDate.Text = "Effective Date"
		Me.lblPEffectiveDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPEffectiveDate.UseMnemonic = True
		Me.lblPEffectiveDate.Visible = True
		' 
		' lblPTransType
		' 
		Me.lblPTransType.AutoSize = False
		Me.lblPTransType.BackColor = System.Drawing.SystemColors.Control
		Me.lblPTransType.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPTransType.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPTransType.Enabled = True
		Me.lblPTransType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPTransType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPTransType.Location = New System.Drawing.Point(16, 160)
		Me.lblPTransType.Name = "lblPTransType"
		Me.lblPTransType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPTransType.Size = New System.Drawing.Size(89, 25)
		Me.lblPTransType.TabIndex = 51
		Me.lblPTransType.Text = "Transaction Type"
		Me.lblPTransType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPTransType.UseMnemonic = True
		Me.lblPTransType.Visible = True
		' 
		' lblPDescription
		' 
		Me.lblPDescription.AutoSize = False
		Me.lblPDescription.BackColor = System.Drawing.SystemColors.Control
		Me.lblPDescription.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPDescription.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPDescription.Enabled = True
		Me.lblPDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPDescription.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPDescription.Location = New System.Drawing.Point(16, 96)
		Me.lblPDescription.Name = "lblPDescription"
		Me.lblPDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPDescription.Size = New System.Drawing.Size(89, 17)
		Me.lblPDescription.TabIndex = 50
		Me.lblPDescription.Text = "Description"
		Me.lblPDescription.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPDescription.UseMnemonic = True
		Me.lblPDescription.Visible = True
		' 
		' lblPStartNavMap
		' 
		Me.lblPStartNavMap.AutoSize = False
		Me.lblPStartNavMap.BackColor = System.Drawing.SystemColors.Control
		Me.lblPStartNavMap.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPStartNavMap.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPStartNavMap.Enabled = True
		Me.lblPStartNavMap.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPStartNavMap.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPStartNavMap.Location = New System.Drawing.Point(16, 256)
		Me.lblPStartNavMap.Name = "lblPStartNavMap"
		Me.lblPStartNavMap.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPStartNavMap.Size = New System.Drawing.Size(89, 17)
		Me.lblPStartNavMap.TabIndex = 49
		Me.lblPStartNavMap.Text = "Start Map"
		Me.lblPStartNavMap.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPStartNavMap.UseMnemonic = True
		Me.lblPStartNavMap.Visible = True
		' 
		' lblPProduct
		' 
		Me.lblPProduct.AutoSize = False
		Me.lblPProduct.BackColor = System.Drawing.SystemColors.Control
		Me.lblPProduct.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPProduct.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPProduct.Enabled = True
		Me.lblPProduct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPProduct.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPProduct.Location = New System.Drawing.Point(16, 32)
		Me.lblPProduct.Name = "lblPProduct"
		Me.lblPProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPProduct.Size = New System.Drawing.Size(89, 17)
		Me.lblPProduct.TabIndex = 48
		Me.lblPProduct.Text = "Product"
		Me.lblPProduct.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPProduct.UseMnemonic = True
		Me.lblPProduct.Visible = True
		' 
		' lblPProcessLockGroup
		' 
		Me.lblPProcessLockGroup.AutoSize = False
		Me.lblPProcessLockGroup.BackColor = System.Drawing.SystemColors.Control
		Me.lblPProcessLockGroup.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPProcessLockGroup.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPProcessLockGroup.Enabled = True
		Me.lblPProcessLockGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPProcessLockGroup.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPProcessLockGroup.Location = New System.Drawing.Point(16, 128)
		Me.lblPProcessLockGroup.Name = "lblPProcessLockGroup"
		Me.lblPProcessLockGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPProcessLockGroup.Size = New System.Drawing.Size(89, 17)
		Me.lblPProcessLockGroup.TabIndex = 47
		Me.lblPProcessLockGroup.Text = "Lock Group"
		Me.lblPProcessLockGroup.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPProcessLockGroup.UseMnemonic = True
		Me.lblPProcessLockGroup.Visible = True
		' 
		' lblPProcessMode
		' 
		Me.lblPProcessMode.AutoSize = False
		Me.lblPProcessMode.BackColor = System.Drawing.SystemColors.Control
		Me.lblPProcessMode.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPProcessMode.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPProcessMode.Enabled = True
		Me.lblPProcessMode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPProcessMode.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPProcessMode.Location = New System.Drawing.Point(16, 192)
		Me.lblPProcessMode.Name = "lblPProcessMode"
		Me.lblPProcessMode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPProcessMode.Size = New System.Drawing.Size(81, 17)
		Me.lblPProcessMode.TabIndex = 46
		Me.lblPProcessMode.Text = "Process Mode"
		Me.lblPProcessMode.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPProcessMode.UseMnemonic = True
		Me.lblPProcessMode.Visible = True
		' 
		' lblPCode
		' 
		Me.lblPCode.AutoSize = False
		Me.lblPCode.BackColor = System.Drawing.SystemColors.Control
		Me.lblPCode.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPCode.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPCode.Enabled = True
		Me.lblPCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPCode.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPCode.Location = New System.Drawing.Point(16, 64)
		Me.lblPCode.Name = "lblPCode"
		Me.lblPCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPCode.Size = New System.Drawing.Size(89, 17)
		Me.lblPCode.TabIndex = 45
		Me.lblPCode.Text = "Code"
		Me.lblPCode.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPCode.UseMnemonic = True
		Me.lblPCode.Visible = True
		' 
		' lblProcessDetails
		' 
		Me.lblProcessDetails.AutoSize = False
		Me.lblProcessDetails.BackColor = System.Drawing.SystemColors.Control
		Me.lblProcessDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblProcessDetails.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblProcessDetails.Enabled = True
		Me.lblProcessDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblProcessDetails.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblProcessDetails.Location = New System.Drawing.Point(8, 8)
		Me.lblProcessDetails.Name = "lblProcessDetails"
		Me.lblProcessDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblProcessDetails.Size = New System.Drawing.Size(385, 17)
		Me.lblProcessDetails.TabIndex = 75
		Me.lblProcessDetails.Text = "Process Details"
		Me.lblProcessDetails.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me.lblProcessDetails.UseMnemonic = True
		Me.lblProcessDetails.Visible = True
		' 
		' fraBlank
		' 
		Me.fraBlank.BackColor = System.Drawing.SystemColors.Control
		Me.fraBlank.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.fraBlank.CausesValidation = True
		Me.fraBlank.Cursor = System.Windows.Forms.Cursors.Default
		Me.fraBlank.Dock = System.Windows.Forms.DockStyle.None
		Me.fraBlank.Enabled = True
		Me.fraBlank.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraBlank.Location = New System.Drawing.Point(200, 80)
		Me.fraBlank.Name = "fraBlank"
		Me.fraBlank.Size = New System.Drawing.Size(401, 307)
		Me.fraBlank.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.fraBlank.TabIndex = 80
		Me.fraBlank.TabStop = True
		Me.fraBlank.Visible = False
		' 
		' fraComponent
		' 
		Me.fraComponent.BackColor = System.Drawing.SystemColors.Control
		Me.fraComponent.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.fraComponent.CausesValidation = True
		Me.fraComponent.Controls.Add(Me.txtPMNav_Componentdescription)
		Me.fraComponent.Controls.Add(Me.txtPMNav_Componentcaption_id)
		Me.fraComponent.Controls.Add(Me.chkPMNav_Componentis_server_side)
		Me.fraComponent.Controls.Add(Me.txtPMNav_Componentclass_name)
		Me.fraComponent.Controls.Add(Me.txtPMNav_Componentobject_name)
		Me.fraComponent.Controls.Add(Me.txtPMNav_Componenteffective_date)
		Me.fraComponent.Controls.Add(Me.cboPMNav_Componentnav_component_type)
		Me.fraComponent.Controls.Add(Me.lblComponentDetails)
		Me.fraComponent.Controls.Add(Me.lblCClassName)
		Me.fraComponent.Controls.Add(Me.lblCObjectName)
		Me.fraComponent.Controls.Add(Me.lblCDescription)
		Me.fraComponent.Controls.Add(Me.lblCType)
		Me.fraComponent.Controls.Add(Me.lblCEffectiveDate)
		Me.fraComponent.Cursor = System.Windows.Forms.Cursors.Default
		Me.fraComponent.Dock = System.Windows.Forms.DockStyle.None
		Me.fraComponent.Enabled = True
		Me.fraComponent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraComponent.Location = New System.Drawing.Point(200, 80)
		Me.fraComponent.Name = "fraComponent"
		Me.fraComponent.Size = New System.Drawing.Size(401, 307)
		Me.fraComponent.TabIndex = 61
		Me.fraComponent.TabStop = True
		Me.fraComponent.Visible = False
		' 
		' txtPMNav_Componentdescription
		' 
		Me.txtPMNav_Componentdescription.AcceptsReturn = True
		Me.txtPMNav_Componentdescription.AutoSize = False
		Me.txtPMNav_Componentdescription.BackColor = System.Drawing.SystemColors.Window
		Me.txtPMNav_Componentdescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPMNav_Componentdescription.CausesValidation = True
		Me.txtPMNav_Componentdescription.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPMNav_Componentdescription.Enabled = True
		Me.txtPMNav_Componentdescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPMNav_Componentdescription.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPMNav_Componentdescription.HideSelection = True
		Me.txtPMNav_Componentdescription.Location = New System.Drawing.Point(104, 104)
		Me.txtPMNav_Componentdescription.MaxLength = 0
		Me.txtPMNav_Componentdescription.Multiline = False
		Me.txtPMNav_Componentdescription.Name = "txtPMNav_Componentdescription"
		Me.txtPMNav_Componentdescription.ReadOnly = False
		Me.txtPMNav_Componentdescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPMNav_Componentdescription.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPMNav_Componentdescription.Size = New System.Drawing.Size(281, 19)
		Me.txtPMNav_Componentdescription.TabIndex = 3
		Me.txtPMNav_Componentdescription.TabStop = True
		Me.txtPMNav_Componentdescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPMNav_Componentdescription.Visible = True
		' 
		' txtPMNav_Componentcaption_id
		' 
		Me.txtPMNav_Componentcaption_id.AcceptsReturn = True
		Me.txtPMNav_Componentcaption_id.AutoSize = False
		Me.txtPMNav_Componentcaption_id.BackColor = System.Drawing.SystemColors.Window
		Me.txtPMNav_Componentcaption_id.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPMNav_Componentcaption_id.CausesValidation = True
		Me.txtPMNav_Componentcaption_id.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPMNav_Componentcaption_id.Enabled = True
		Me.txtPMNav_Componentcaption_id.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPMNav_Componentcaption_id.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPMNav_Componentcaption_id.HideSelection = True
		Me.txtPMNav_Componentcaption_id.Location = New System.Drawing.Point(104, 104)
		Me.txtPMNav_Componentcaption_id.MaxLength = 0
		Me.txtPMNav_Componentcaption_id.Multiline = False
		Me.txtPMNav_Componentcaption_id.Name = "txtPMNav_Componentcaption_id"
		Me.txtPMNav_Componentcaption_id.ReadOnly = False
		Me.txtPMNav_Componentcaption_id.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPMNav_Componentcaption_id.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPMNav_Componentcaption_id.Size = New System.Drawing.Size(193, 19)
		Me.txtPMNav_Componentcaption_id.TabIndex = 4
		Me.txtPMNav_Componentcaption_id.TabStop = True
		Me.txtPMNav_Componentcaption_id.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPMNav_Componentcaption_id.Visible = False
		' 
		' chkPMNav_Componentis_server_side
		' 
		Me.chkPMNav_Componentis_server_side.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkPMNav_Componentis_server_side.BackColor = System.Drawing.SystemColors.Control
		Me.chkPMNav_Componentis_server_side.CausesValidation = True
		Me.chkPMNav_Componentis_server_side.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkPMNav_Componentis_server_side.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkPMNav_Componentis_server_side.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkPMNav_Componentis_server_side.Enabled = True
		Me.chkPMNav_Componentis_server_side.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkPMNav_Componentis_server_side.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkPMNav_Componentis_server_side.Location = New System.Drawing.Point(296, 136)
		Me.chkPMNav_Componentis_server_side.Name = "chkPMNav_Componentis_server_side"
		Me.chkPMNav_Componentis_server_side.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkPMNav_Componentis_server_side.Size = New System.Drawing.Size(89, 17)
		Me.chkPMNav_Componentis_server_side.TabIndex = 6
		Me.chkPMNav_Componentis_server_side.TabStop = True
		Me.chkPMNav_Componentis_server_side.Text = "Is Server Side"
		Me.chkPMNav_Componentis_server_side.Visible = True
		' 
		' txtPMNav_Componentclass_name
		' 
		Me.txtPMNav_Componentclass_name.AcceptsReturn = True
		Me.txtPMNav_Componentclass_name.AutoSize = False
		Me.txtPMNav_Componentclass_name.BackColor = System.Drawing.SystemColors.Window
		Me.txtPMNav_Componentclass_name.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPMNav_Componentclass_name.CausesValidation = True
		Me.txtPMNav_Componentclass_name.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPMNav_Componentclass_name.Enabled = True
		Me.txtPMNav_Componentclass_name.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPMNav_Componentclass_name.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPMNav_Componentclass_name.HideSelection = True
		Me.txtPMNav_Componentclass_name.Location = New System.Drawing.Point(104, 72)
		Me.txtPMNav_Componentclass_name.MaxLength = 0
		Me.txtPMNav_Componentclass_name.Multiline = False
		Me.txtPMNav_Componentclass_name.Name = "txtPMNav_Componentclass_name"
		Me.txtPMNav_Componentclass_name.ReadOnly = False
		Me.txtPMNav_Componentclass_name.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPMNav_Componentclass_name.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPMNav_Componentclass_name.Size = New System.Drawing.Size(281, 19)
		Me.txtPMNav_Componentclass_name.TabIndex = 2
		Me.txtPMNav_Componentclass_name.TabStop = True
		Me.txtPMNav_Componentclass_name.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPMNav_Componentclass_name.Visible = True
		' 
		' txtPMNav_Componentobject_name
		' 
		Me.txtPMNav_Componentobject_name.AcceptsReturn = True
		Me.txtPMNav_Componentobject_name.AutoSize = False
		Me.txtPMNav_Componentobject_name.BackColor = System.Drawing.SystemColors.Window
		Me.txtPMNav_Componentobject_name.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPMNav_Componentobject_name.CausesValidation = True
		Me.txtPMNav_Componentobject_name.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPMNav_Componentobject_name.Enabled = True
		Me.txtPMNav_Componentobject_name.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPMNav_Componentobject_name.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPMNav_Componentobject_name.HideSelection = True
		Me.txtPMNav_Componentobject_name.Location = New System.Drawing.Point(104, 40)
		Me.txtPMNav_Componentobject_name.MaxLength = 0
		Me.txtPMNav_Componentobject_name.Multiline = False
		Me.txtPMNav_Componentobject_name.Name = "txtPMNav_Componentobject_name"
		Me.txtPMNav_Componentobject_name.ReadOnly = False
		Me.txtPMNav_Componentobject_name.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPMNav_Componentobject_name.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPMNav_Componentobject_name.Size = New System.Drawing.Size(281, 19)
		Me.txtPMNav_Componentobject_name.TabIndex = 1
		Me.txtPMNav_Componentobject_name.TabStop = True
		Me.txtPMNav_Componentobject_name.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPMNav_Componentobject_name.Visible = True
		' 
		' txtPMNav_Componenteffective_date
		' 
		Me.txtPMNav_Componenteffective_date.AcceptsReturn = True
		Me.txtPMNav_Componenteffective_date.AutoSize = False
		Me.txtPMNav_Componenteffective_date.BackColor = System.Drawing.SystemColors.Window
		Me.txtPMNav_Componenteffective_date.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPMNav_Componenteffective_date.CausesValidation = True
		Me.txtPMNav_Componenteffective_date.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPMNav_Componenteffective_date.Enabled = True
		Me.txtPMNav_Componenteffective_date.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPMNav_Componenteffective_date.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPMNav_Componenteffective_date.HideSelection = True
		Me.txtPMNav_Componenteffective_date.Location = New System.Drawing.Point(104, 176)
		Me.txtPMNav_Componenteffective_date.MaxLength = 0
		Me.txtPMNav_Componenteffective_date.Multiline = False
		Me.txtPMNav_Componenteffective_date.Name = "txtPMNav_Componenteffective_date"
		Me.txtPMNav_Componenteffective_date.ReadOnly = False
		Me.txtPMNav_Componenteffective_date.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPMNav_Componenteffective_date.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPMNav_Componenteffective_date.Size = New System.Drawing.Size(281, 19)
		Me.txtPMNav_Componenteffective_date.TabIndex = 7
		Me.txtPMNav_Componenteffective_date.TabStop = True
		Me.txtPMNav_Componenteffective_date.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPMNav_Componenteffective_date.Visible = True
		' 
		' cboPMNav_Componentnav_component_type
		' 
		Me.cboPMNav_Componentnav_component_type.BackColor = System.Drawing.SystemColors.Window
		Me.cboPMNav_Componentnav_component_type.CausesValidation = True
		Me.cboPMNav_Componentnav_component_type.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboPMNav_Componentnav_component_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboPMNav_Componentnav_component_type.Enabled = True
		Me.cboPMNav_Componentnav_component_type.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboPMNav_Componentnav_component_type.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboPMNav_Componentnav_component_type.IntegralHeight = True
		Me.cboPMNav_Componentnav_component_type.Location = New System.Drawing.Point(104, 136)
		Me.cboPMNav_Componentnav_component_type.Name = "cboPMNav_Componentnav_component_type"
		Me.cboPMNav_Componentnav_component_type.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboPMNav_Componentnav_component_type.Size = New System.Drawing.Size(185, 21)
		Me.cboPMNav_Componentnav_component_type.Sorted = False
		Me.cboPMNav_Componentnav_component_type.TabIndex = 5
		Me.cboPMNav_Componentnav_component_type.TabStop = True
		Me.cboPMNav_Componentnav_component_type.Visible = True
		' 
		' lblComponentDetails
		' 
		Me.lblComponentDetails.AutoSize = False
		Me.lblComponentDetails.BackColor = System.Drawing.SystemColors.Control
		Me.lblComponentDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblComponentDetails.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblComponentDetails.Enabled = True
		Me.lblComponentDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblComponentDetails.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblComponentDetails.Location = New System.Drawing.Point(8, 8)
		Me.lblComponentDetails.Name = "lblComponentDetails"
		Me.lblComponentDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblComponentDetails.Size = New System.Drawing.Size(385, 17)
		Me.lblComponentDetails.TabIndex = 76
		Me.lblComponentDetails.Text = "Component Details"
		Me.lblComponentDetails.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me.lblComponentDetails.UseMnemonic = True
		Me.lblComponentDetails.Visible = True
		' 
		' lblCClassName
		' 
		Me.lblCClassName.AutoSize = False
		Me.lblCClassName.BackColor = System.Drawing.SystemColors.Control
		Me.lblCClassName.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCClassName.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCClassName.Enabled = True
		Me.lblCClassName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCClassName.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCClassName.Location = New System.Drawing.Point(16, 72)
		Me.lblCClassName.Name = "lblCClassName"
		Me.lblCClassName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCClassName.Size = New System.Drawing.Size(89, 17)
		Me.lblCClassName.TabIndex = 66
		Me.lblCClassName.Text = "Class Name"
		Me.lblCClassName.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCClassName.UseMnemonic = True
		Me.lblCClassName.Visible = True
		' 
		' lblCObjectName
		' 
		Me.lblCObjectName.AutoSize = False
		Me.lblCObjectName.BackColor = System.Drawing.SystemColors.Control
		Me.lblCObjectName.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCObjectName.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCObjectName.Enabled = True
		Me.lblCObjectName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCObjectName.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCObjectName.Location = New System.Drawing.Point(16, 40)
		Me.lblCObjectName.Name = "lblCObjectName"
		Me.lblCObjectName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCObjectName.Size = New System.Drawing.Size(89, 17)
		Me.lblCObjectName.TabIndex = 65
		Me.lblCObjectName.Text = "Object Name"
		Me.lblCObjectName.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCObjectName.UseMnemonic = True
		Me.lblCObjectName.Visible = True
		' 
		' lblCDescription
		' 
		Me.lblCDescription.AutoSize = False
		Me.lblCDescription.BackColor = System.Drawing.SystemColors.Control
		Me.lblCDescription.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCDescription.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCDescription.Enabled = True
		Me.lblCDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCDescription.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCDescription.Location = New System.Drawing.Point(16, 104)
		Me.lblCDescription.Name = "lblCDescription"
		Me.lblCDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCDescription.Size = New System.Drawing.Size(89, 17)
		Me.lblCDescription.TabIndex = 64
		Me.lblCDescription.Text = "Description"
		Me.lblCDescription.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCDescription.UseMnemonic = True
		Me.lblCDescription.Visible = True
		' 
		' lblCType
		' 
		Me.lblCType.AutoSize = False
		Me.lblCType.BackColor = System.Drawing.SystemColors.Control
		Me.lblCType.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCType.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCType.Enabled = True
		Me.lblCType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCType.Location = New System.Drawing.Point(16, 136)
		Me.lblCType.Name = "lblCType"
		Me.lblCType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCType.Size = New System.Drawing.Size(89, 25)
		Me.lblCType.TabIndex = 63
		Me.lblCType.Text = "Type"
		Me.lblCType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCType.UseMnemonic = True
		Me.lblCType.Visible = True
		' 
		' lblCEffectiveDate
		' 
		Me.lblCEffectiveDate.AutoSize = False
		Me.lblCEffectiveDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblCEffectiveDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCEffectiveDate.Enabled = True
		Me.lblCEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCEffectiveDate.Location = New System.Drawing.Point(16, 176)
		Me.lblCEffectiveDate.Name = "lblCEffectiveDate"
		Me.lblCEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCEffectiveDate.Size = New System.Drawing.Size(89, 17)
		Me.lblCEffectiveDate.TabIndex = 62
		Me.lblCEffectiveDate.Text = "Effective Date"
		Me.lblCEffectiveDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCEffectiveDate.UseMnemonic = True
		Me.lblCEffectiveDate.Visible = True
		' 
		' fraMap
		' 
		Me.fraMap.BackColor = System.Drawing.SystemColors.Control
		Me.fraMap.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.fraMap.CausesValidation = True
		Me.fraMap.Controls.Add(Me.txtPMNav_MapDescription)
		Me.fraMap.Controls.Add(Me.txtPMNav_Mapcaption_id)
		Me.fraMap.Controls.Add(Me.txtPMNav_Mapcode)
		Me.fraMap.Controls.Add(Me.chkPMNav_Mapis_start_map)
		Me.fraMap.Controls.Add(Me.txtPMNav_Mapeffective_date)
		Me.fraMap.Controls.Add(Me.lblMapDetails)
		Me.fraMap.Controls.Add(Me.lblMCode)
		Me.fraMap.Controls.Add(Me.lblMDescription)
		Me.fraMap.Controls.Add(Me.lblMEffectiveDate)
		Me.fraMap.Cursor = System.Windows.Forms.Cursors.Default
		Me.fraMap.Dock = System.Windows.Forms.DockStyle.None
		Me.fraMap.Enabled = True
		Me.fraMap.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraMap.Location = New System.Drawing.Point(200, 80)
		Me.fraMap.Name = "fraMap"
		Me.fraMap.Size = New System.Drawing.Size(401, 307)
		Me.fraMap.TabIndex = 67
		Me.fraMap.TabStop = True
		Me.fraMap.Visible = False
		' 
		' txtPMNav_MapDescription
		' 
		Me.txtPMNav_MapDescription.AcceptsReturn = True
		Me.txtPMNav_MapDescription.AutoSize = False
		Me.txtPMNav_MapDescription.BackColor = System.Drawing.SystemColors.Window
		Me.txtPMNav_MapDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPMNav_MapDescription.CausesValidation = True
		Me.txtPMNav_MapDescription.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPMNav_MapDescription.Enabled = True
		Me.txtPMNav_MapDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPMNav_MapDescription.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPMNav_MapDescription.HideSelection = True
		Me.txtPMNav_MapDescription.Location = New System.Drawing.Point(96, 80)
		Me.txtPMNav_MapDescription.MaxLength = 0
		Me.txtPMNav_MapDescription.Multiline = False
		Me.txtPMNav_MapDescription.Name = "txtPMNav_MapDescription"
		Me.txtPMNav_MapDescription.ReadOnly = False
		Me.txtPMNav_MapDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPMNav_MapDescription.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPMNav_MapDescription.Size = New System.Drawing.Size(289, 19)
		Me.txtPMNav_MapDescription.TabIndex = 10
		Me.txtPMNav_MapDescription.TabStop = True
		Me.txtPMNav_MapDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPMNav_MapDescription.Visible = True
		' 
		' txtPMNav_Mapcaption_id
		' 
		Me.txtPMNav_Mapcaption_id.AcceptsReturn = True
		Me.txtPMNav_Mapcaption_id.AutoSize = False
		Me.txtPMNav_Mapcaption_id.BackColor = System.Drawing.SystemColors.Window
		Me.txtPMNav_Mapcaption_id.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPMNav_Mapcaption_id.CausesValidation = True
		Me.txtPMNav_Mapcaption_id.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPMNav_Mapcaption_id.Enabled = True
		Me.txtPMNav_Mapcaption_id.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPMNav_Mapcaption_id.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPMNav_Mapcaption_id.HideSelection = True
		Me.txtPMNav_Mapcaption_id.Location = New System.Drawing.Point(112, 80)
		Me.txtPMNav_Mapcaption_id.MaxLength = 0
		Me.txtPMNav_Mapcaption_id.Multiline = False
		Me.txtPMNav_Mapcaption_id.Name = "txtPMNav_Mapcaption_id"
		Me.txtPMNav_Mapcaption_id.ReadOnly = False
		Me.txtPMNav_Mapcaption_id.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPMNav_Mapcaption_id.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPMNav_Mapcaption_id.Size = New System.Drawing.Size(193, 19)
		Me.txtPMNav_Mapcaption_id.TabIndex = 9
		Me.txtPMNav_Mapcaption_id.TabStop = True
		Me.txtPMNav_Mapcaption_id.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPMNav_Mapcaption_id.Visible = False
		' 
		' txtPMNav_Mapcode
		' 
		Me.txtPMNav_Mapcode.AcceptsReturn = True
		Me.txtPMNav_Mapcode.AutoSize = False
		Me.txtPMNav_Mapcode.BackColor = System.Drawing.SystemColors.Window
		Me.txtPMNav_Mapcode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPMNav_Mapcode.CausesValidation = True
		Me.txtPMNav_Mapcode.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPMNav_Mapcode.Enabled = True
		Me.txtPMNav_Mapcode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPMNav_Mapcode.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPMNav_Mapcode.HideSelection = True
		Me.txtPMNav_Mapcode.Location = New System.Drawing.Point(96, 40)
		Me.txtPMNav_Mapcode.MaxLength = 0
		Me.txtPMNav_Mapcode.Multiline = False
		Me.txtPMNav_Mapcode.Name = "txtPMNav_Mapcode"
		Me.txtPMNav_Mapcode.ReadOnly = False
		Me.txtPMNav_Mapcode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPMNav_Mapcode.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPMNav_Mapcode.Size = New System.Drawing.Size(289, 19)
		Me.txtPMNav_Mapcode.TabIndex = 8
		Me.txtPMNav_Mapcode.TabStop = True
		Me.txtPMNav_Mapcode.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPMNav_Mapcode.Visible = True
		' 
		' chkPMNav_Mapis_start_map
		' 
		Me.chkPMNav_Mapis_start_map.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkPMNav_Mapis_start_map.BackColor = System.Drawing.SystemColors.Control
		Me.chkPMNav_Mapis_start_map.CausesValidation = True
		Me.chkPMNav_Mapis_start_map.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkPMNav_Mapis_start_map.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkPMNav_Mapis_start_map.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkPMNav_Mapis_start_map.Enabled = False
		Me.chkPMNav_Mapis_start_map.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkPMNav_Mapis_start_map.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkPMNav_Mapis_start_map.Location = New System.Drawing.Point(96, 120)
		Me.chkPMNav_Mapis_start_map.Name = "chkPMNav_Mapis_start_map"
		Me.chkPMNav_Mapis_start_map.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkPMNav_Mapis_start_map.Size = New System.Drawing.Size(89, 17)
		Me.chkPMNav_Mapis_start_map.TabIndex = 11
		Me.chkPMNav_Mapis_start_map.TabStop = True
		Me.chkPMNav_Mapis_start_map.Text = "Is Start Map"
		Me.chkPMNav_Mapis_start_map.Visible = True
		' 
		' txtPMNav_Mapeffective_date
		' 
		Me.txtPMNav_Mapeffective_date.AcceptsReturn = True
		Me.txtPMNav_Mapeffective_date.AutoSize = False
		Me.txtPMNav_Mapeffective_date.BackColor = System.Drawing.SystemColors.Window
		Me.txtPMNav_Mapeffective_date.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPMNav_Mapeffective_date.CausesValidation = True
		Me.txtPMNav_Mapeffective_date.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPMNav_Mapeffective_date.Enabled = True
		Me.txtPMNav_Mapeffective_date.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPMNav_Mapeffective_date.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPMNav_Mapeffective_date.HideSelection = True
		Me.txtPMNav_Mapeffective_date.Location = New System.Drawing.Point(96, 152)
		Me.txtPMNav_Mapeffective_date.MaxLength = 0
		Me.txtPMNav_Mapeffective_date.Multiline = False
		Me.txtPMNav_Mapeffective_date.Name = "txtPMNav_Mapeffective_date"
		Me.txtPMNav_Mapeffective_date.ReadOnly = False
		Me.txtPMNav_Mapeffective_date.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPMNav_Mapeffective_date.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPMNav_Mapeffective_date.Size = New System.Drawing.Size(289, 19)
		Me.txtPMNav_Mapeffective_date.TabIndex = 12
		Me.txtPMNav_Mapeffective_date.TabStop = True
		Me.txtPMNav_Mapeffective_date.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPMNav_Mapeffective_date.Visible = True
		' 
		' lblMapDetails
		' 
		Me.lblMapDetails.AutoSize = False
		Me.lblMapDetails.BackColor = System.Drawing.SystemColors.Control
		Me.lblMapDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMapDetails.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMapDetails.Enabled = True
		Me.lblMapDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMapDetails.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMapDetails.Location = New System.Drawing.Point(8, 9)
		Me.lblMapDetails.Name = "lblMapDetails"
		Me.lblMapDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMapDetails.Size = New System.Drawing.Size(385, 17)
		Me.lblMapDetails.TabIndex = 73
		Me.lblMapDetails.Text = "Map Details"
		Me.lblMapDetails.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me.lblMapDetails.UseMnemonic = True
		Me.lblMapDetails.Visible = True
		' 
		' lblMCode
		' 
		Me.lblMCode.AutoSize = False
		Me.lblMCode.BackColor = System.Drawing.SystemColors.Control
		Me.lblMCode.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMCode.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMCode.Enabled = True
		Me.lblMCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMCode.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMCode.Location = New System.Drawing.Point(16, 40)
		Me.lblMCode.Name = "lblMCode"
		Me.lblMCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMCode.Size = New System.Drawing.Size(89, 17)
		Me.lblMCode.TabIndex = 70
		Me.lblMCode.Text = "Code"
		Me.lblMCode.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMCode.UseMnemonic = True
		Me.lblMCode.Visible = True
		' 
		' lblMDescription
		' 
		Me.lblMDescription.AutoSize = False
		Me.lblMDescription.BackColor = System.Drawing.SystemColors.Control
		Me.lblMDescription.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMDescription.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMDescription.Enabled = True
		Me.lblMDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMDescription.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMDescription.Location = New System.Drawing.Point(16, 80)
		Me.lblMDescription.Name = "lblMDescription"
		Me.lblMDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMDescription.Size = New System.Drawing.Size(89, 17)
		Me.lblMDescription.TabIndex = 69
		Me.lblMDescription.Text = "Description"
		Me.lblMDescription.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMDescription.UseMnemonic = True
		Me.lblMDescription.Visible = True
		' 
		' lblMEffectiveDate
		' 
		Me.lblMEffectiveDate.AutoSize = False
		Me.lblMEffectiveDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblMEffectiveDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMEffectiveDate.Enabled = True
		Me.lblMEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMEffectiveDate.Location = New System.Drawing.Point(16, 152)
		Me.lblMEffectiveDate.Name = "lblMEffectiveDate"
		Me.lblMEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMEffectiveDate.Size = New System.Drawing.Size(89, 17)
		Me.lblMEffectiveDate.TabIndex = 68
		Me.lblMEffectiveDate.Text = "Effective Date"
		Me.lblMEffectiveDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMEffectiveDate.UseMnemonic = True
		Me.lblMEffectiveDate.Visible = True
		' 
		' imgTreeImages
		' 
		Me.imgTreeImages.ImageSize = New System.Drawing.Size(16, 16)
		Me.imgTreeImages.ImageStream = CType(resources.GetObject("imgTreeImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.imgTreeImages.Images.SetKeyName(0, "Root")
		Me.imgTreeImages.Images.SetKeyName(1, "Process")
		Me.imgTreeImages.Images.SetKeyName(2, "StepFind")
		Me.imgTreeImages.Images.SetKeyName(3, "Component")
		Me.imgTreeImages.Images.SetKeyName(4, "StepNoForm")
		Me.imgTreeImages.Images.SetKeyName(5, "Map")
		Me.imgTreeImages.Images.SetKeyName(6, "StepDataForm")
		Me.imgTreeImages.Images.SetKeyName(7, "StepDecision")
		Me.imgTreeImages.Images.SetKeyName(8, "StepSubMap")
		' 
		' linMenuLine1
		' 
		Me.linMenuLine1.BackColor = System.Drawing.SystemColors.ControlDark
		Me.linMenuLine1.Location = New System.Drawing.Point(0, 24)
		Me.linMenuLine1.Name = "linMenuLine1"
		Me.linMenuLine1.Size = New System.Drawing.Size(604, 1)
		Me.linMenuLine1.Visible = True
		' 
		' linMenuLine2
		' 
		Me.linMenuLine2.BackColor = System.Drawing.SystemColors.ControlLight
		Me.linMenuLine2.Location = New System.Drawing.Point(0, 24)
		Me.linMenuLine2.Name = "linMenuLine2"
		Me.linMenuLine2.Size = New System.Drawing.Size(604, 1)
		Me.linMenuLine2.Visible = True
		' 
		' imgImages
		' 
		Me.imgImages.ImageSize = New System.Drawing.Size(16, 16)
		Me.imgImages.ImageStream = CType(resources.GetObject("imgImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.imgImages.Images.SetKeyName(0, "Root")
		Me.imgImages.Images.SetKeyName(1, "Component")
		Me.imgImages.Images.SetKeyName(2, "Map")
		Me.imgImages.Images.SetKeyName(3, "Process")
		Me.imgImages.Images.SetKeyName(4, "Step")
		Me.imgImages.Images.SetKeyName(5, "Keys")
		Me.imgImages.Images.SetKeyName(6, "Delete")
		Me.imgImages.Images.SetKeyName(7, "Copy")
		Me.imgImages.Images.SetKeyName(8, "Print")
		' 
		' frmInterface
		' 
		Me.Controls.Add(Me.tlbToolBar)
		Me.Controls.Add(Me.chkIsDeleted)
		Me.Controls.Add(Me.cboGroupDescription)
		Me.Controls.Add(Me.cboGroups)
		Me.Controls.Add(Me.panSliderBar)
		Me.Controls.Add(Me.cmdApply)
		Me.Controls.Add(Me.cmdExit)
		Me.Controls.Add(Me.treMainData)
		Me.Controls.Add(Me.fraStep)
		Me.Controls.Add(Me.fraProcess)
		Me.Controls.Add(Me.fraBlank)
		Me.Controls.Add(Me.fraComponent)
		Me.Controls.Add(Me.fraMap)
		Me.Controls.Add(Me.linMenuLine1)
		Me.Controls.Add(Me.linMenuLine2)
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(603, 420)
		Me.ControlBox = True
		Me.Controls.Add(MainMenu1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmInterface.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 16425)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Navigator Process Editor"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.listBoxComboBoxHelper1.SetItemData(Me.cboGroups, New Integer(){0, 0, 0})
		Me.listBoxComboBoxHelper1.SetItemData(Me.cboPMNav_Steppmnav_map_id, New Integer(){0, 0, 0, 0, 0})
		CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tlbToolBar.ResumeLayout(False)
		Me.panSliderBar.ResumeLayout(False)
		Me.fraStep.ResumeLayout(False)
		Me.fraProcess.ResumeLayout(False)
		Me.fraComponent.ResumeLayout(False)
		Me.fraMap.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class