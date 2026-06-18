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
	Private Sub Ctx_mnuMap_Opening(ByVal sender As object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuMap.Opening
		Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
		Ctx_mnuMap.Items.Clear()
		'We are moving the submenus from original menu to the context menu before displaying it
		For	Each item As System.Windows.Forms.ToolStripItem In mnuMap.DropDownItems
			list.Add(item)
		Next item
		For	Each item As System.Windows.Forms.ToolStripItem In list
			Ctx_mnuMap.Items.Add(item)
		Next item
		e.Cancel = False
	End Sub
	Private Sub Ctx_mnuMap_Closing(ByVal sender As object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuMap.Closing
		Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
		'We are moving the submenus the context menu back to the original menu after displaying
		For	Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuMap.Items
			list.Add(item)
		Next item
		For	Each item As System.Windows.Forms.ToolStripItem In list
			mnuMap.DropDownItems.Add(item)
		Next item
	End Sub
	Private Sub Ctx_mnuSubmap_Opening(ByVal sender As object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuSubmap.Opening
		Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
		Ctx_mnuSubmap.Items.Clear()
		'We are moving the submenus from original menu to the context menu before displaying it
		For	Each item As System.Windows.Forms.ToolStripItem In mnuSubmap.DropDownItems
			list.Add(item)
		Next item
		For	Each item As System.Windows.Forms.ToolStripItem In list
			Ctx_mnuSubmap.Items.Add(item)
		Next item
		e.Cancel = False
	End Sub
	Private Sub Ctx_mnuSubmap_Closing(ByVal sender As object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuSubmap.Closing
		Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
		'We are moving the submenus the context menu back to the original menu after displaying
		For	Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuSubmap.Items
			list.Add(item)
		Next item
		For	Each item As System.Windows.Forms.ToolStripItem In list
			mnuSubmap.DropDownItems.Add(item)
		Next item
	End Sub
	Private Sub Ctx_mnuKey_Opening(ByVal sender As object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuKey.Opening
		Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
		Ctx_mnuKey.Items.Clear()
		'We are moving the submenus from original menu to the context menu before displaying it
		For	Each item As System.Windows.Forms.ToolStripItem In mnuKey.DropDownItems
			list.Add(item)
		Next item
		For	Each item As System.Windows.Forms.ToolStripItem In list
			Ctx_mnuKey.Items.Add(item)
		Next item
		e.Cancel = False
	End Sub
	Private Sub Ctx_mnuKey_Closing(ByVal sender As object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuKey.Closing
		Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
		'We are moving the submenus the context menu back to the original menu after displaying
		For	Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuKey.Items
			list.Add(item)
		Next item
		For	Each item As System.Windows.Forms.ToolStripItem In list
			mnuKey.DropDownItems.Add(item)
		Next item
	End Sub
	Private Sub Ctx_mnuStep_Opening(ByVal sender As object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuStep.Opening
		Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
		Ctx_mnuStep.Items.Clear()
		'We are moving the submenus from original menu to the context menu before displaying it
		For	Each item As System.Windows.Forms.ToolStripItem In mnuStep.DropDownItems
			list.Add(item)
		Next item
		For	Each item As System.Windows.Forms.ToolStripItem In list
			Ctx_mnuStep.Items.Add(item)
		Next item
		e.Cancel = False
	End Sub
	Private Sub Ctx_mnuStep_Closing(ByVal sender As object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuStep.Closing
		Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
		'We are moving the submenus the context menu back to the original menu after displaying
		For	Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuStep.Items
			list.Add(item)
		Next item
		For	Each item As System.Windows.Forms.ToolStripItem In list
			mnuStep.DropDownItems.Add(item)
		Next item
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
	Public WithEvents mnuFileOpenTask As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFileOpenProcess As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFileOpen As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFileSave As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFileLine2 As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuFileTransferImport As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFileTransferExport As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFileTransfer As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFileTest As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFilePromote As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFileLine3 As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuFileExit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuToolsUpdateLog As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuTools As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuHelpAbout As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuMapAddStep As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuMap As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuStepAddStep As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuStepAddSubmap As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuStepAddBlankStep As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuStepAddKey As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuStepDeleteStep As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuStepMoveUp As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuStepMoveDown As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuStep As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuSubmapAddBlankStep As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuSubmapDeleteSubmap As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuSubmap As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuKeyDeleteKey As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuKey As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	Public WithEvents cmdKeyValueBrowse As System.Windows.Forms.Button
	Public WithEvents txtKeyElementID As System.Windows.Forms.TextBox
	Public WithEvents cmdKeySet As System.Windows.Forms.Button
	Public WithEvents txtKeyName As System.Windows.Forms.TextBox
	Public WithEvents txtKeyValue As System.Windows.Forms.TextBox
	Public WithEvents txtDocTemplate As System.Windows.Forms.TextBox
	Public WithEvents txtDocTempType As System.Windows.Forms.TextBox
	Public WithEvents cboDocTempMode As System.Windows.Forms.ComboBox
	Public WithEvents cboDiaryUserId As System.Windows.Forms.ComboBox
	Public WithEvents cboDiaryUserGroupId As System.Windows.Forms.ComboBox
	Public WithEvents cboDiaryTaskGroupId As System.Windows.Forms.ComboBox
	Public WithEvents cboDiaryTaskId As System.Windows.Forms.ComboBox
	Public WithEvents cboDiaryWMSteps As System.Windows.Forms.ComboBox
	Public WithEvents lblKeyElementID As System.Windows.Forms.Label
	Public WithEvents lblKeyValue As System.Windows.Forms.Label
	Public WithEvents lblKeyName As System.Windows.Forms.Label
	Public WithEvents lblKeyTempCode As System.Windows.Forms.Label
	Public WithEvents lblTempDesc As System.Windows.Forms.Label
	Public WithEvents lblCaption As System.Windows.Forms.Label
	Public WithEvents fmeKey As System.Windows.Forms.GroupBox
	Public WithEvents txtSubmapDescription As System.Windows.Forms.TextBox
	Public WithEvents txtSubmapCode As System.Windows.Forms.TextBox
	Public WithEvents cmdSubmapSet As System.Windows.Forms.Button
	Public WithEvents txtSubmapElementID As System.Windows.Forms.TextBox
	Public WithEvents lblSubmapDescription As System.Windows.Forms.Label
	Public WithEvents lblSubmapCode As System.Windows.Forms.Label
	Public WithEvents lblSubmapElementID As System.Windows.Forms.Label
	Public WithEvents fmeSubmap As System.Windows.Forms.GroupBox
	Public WithEvents tvwResumeStep As System.Windows.Forms.TreeView
	Public WithEvents cmdShowResumeView As System.Windows.Forms.Button
	Public WithEvents txtSubmap As System.Windows.Forms.TextBox
	Public WithEvents txtCancelSteps As System.Windows.Forms.TextBox
	Public WithEvents txtComponent As System.Windows.Forms.TextBox
	Public WithEvents txtComponentAction As System.Windows.Forms.TextBox
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents txtOKSteps As System.Windows.Forms.TextBox
	Public WithEvents txtResumeStep As System.Windows.Forms.TextBox
	Public WithEvents cboOKAction As System.Windows.Forms.ComboBox
	Public WithEvents cboCancelAction As System.Windows.Forms.ComboBox
	Public WithEvents chkCreateWMTask As System.Windows.Forms.CheckBox
	Public WithEvents chkServerSide As System.Windows.Forms.CheckBox
	Public WithEvents chkStepCore As System.Windows.Forms.CheckBox
	Public WithEvents cmdStepSet As System.Windows.Forms.Button
	Public WithEvents txtStepElementID As System.Windows.Forms.TextBox
	Public WithEvents txtStepType As System.Windows.Forms.TextBox
	Public WithEvents txtOKNewRoadmap As System.Windows.Forms.TextBox
	Public WithEvents txtCancelNewRoadmap As System.Windows.Forms.TextBox
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents lblCancelAction As System.Windows.Forms.Label
	Public WithEvents lblCancelSteps As System.Windows.Forms.Label
	Public WithEvents lblComponent As System.Windows.Forms.Label
	Public WithEvents lblComponentAction As System.Windows.Forms.Label
	Public WithEvents lblCreateWMTask As System.Windows.Forms.Label
	Public WithEvents lblOKAction As System.Windows.Forms.Label
	Public WithEvents lblOkSteps As System.Windows.Forms.Label
	Public WithEvents lblResumeStep As System.Windows.Forms.Label
	Public WithEvents lblServerSide As System.Windows.Forms.Label
	Public WithEvents lblSubmap As System.Windows.Forms.Label
	Public WithEvents lblStepCore As System.Windows.Forms.Label
	Public WithEvents lblStepElementID As System.Windows.Forms.Label
	Public WithEvents lblStepType As System.Windows.Forms.Label
	Public WithEvents lblStepOKNewRoadmap As System.Windows.Forms.Label
	Public WithEvents lblStepCancelNewRoadmap As System.Windows.Forms.Label
	Public WithEvents fmeStep As System.Windows.Forms.GroupBox
	Public WithEvents txtMapAutoClose As System.Windows.Forms.TextBox
	Public WithEvents txtMapImageURL As System.Windows.Forms.TextBox
	Public WithEvents txtMapNavigatorDriven As System.Windows.Forms.TextBox
	Public WithEvents txtMapProcessMode As System.Windows.Forms.TextBox
	Public WithEvents txtMapRoadmapName As System.Windows.Forms.TextBox
	Public WithEvents txtMapTitle As System.Windows.Forms.TextBox
	Public WithEvents txtMapTransactionType As System.Windows.Forms.TextBox
	Public WithEvents txtMapWMTaskCode As System.Windows.Forms.TextBox
	Public WithEvents txtMapWMTaskDescription As System.Windows.Forms.TextBox
	Public WithEvents chkMapCore As System.Windows.Forms.CheckBox
	Public WithEvents txtMapVersion As System.Windows.Forms.TextBox
	Public WithEvents cmdMapSet As System.Windows.Forms.Button
	Public WithEvents txtMapElementID As System.Windows.Forms.TextBox
	Public WithEvents lblMapAutoClose As System.Windows.Forms.Label
	Public WithEvents lblMapImageURL As System.Windows.Forms.Label
	Public WithEvents lblMapNavigatorDriven As System.Windows.Forms.Label
	Public WithEvents lblMapProcessMode As System.Windows.Forms.Label
	Public WithEvents lblMapRoadmapName As System.Windows.Forms.Label
	Public WithEvents lblMapTitle As System.Windows.Forms.Label
	Public WithEvents lblMapTransactionType As System.Windows.Forms.Label
	Public WithEvents lblMapWMTaskCode As System.Windows.Forms.Label
	Public WithEvents lblMapWMTaskDescription As System.Windows.Forms.Label
	Public WithEvents lblMapCore As System.Windows.Forms.Label
	Public WithEvents lblMapVersion As System.Windows.Forms.Label
	Public WithEvents lblMapElementID As System.Windows.Forms.Label
	Public WithEvents fmeMap As System.Windows.Forms.GroupBox
	Public WithEvents fmeAttrib As System.Windows.Forms.GroupBox
	Public WithEvents tvwMap As System.Windows.Forms.TreeView
	Public WithEvents lblSplitter As System.Windows.Forms.Label
	Public WithEvents picBG As System.Windows.Forms.PictureBox
	Public WithEvents tbToolbar As System.Windows.Forms.ToolStrip
	Public CommonDialog1Open As System.Windows.Forms.OpenFileDialog
	Public CommonDialog1Save As System.Windows.Forms.SaveFileDialog
	Private WithEvents _sbStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents sbStatus As System.Windows.Forms.StatusStrip
	Public WithEvents imlIcons As System.Windows.Forms.ImageList
	Public WithEvents imlToolbarIcons As System.Windows.Forms.ImageList
	Public WithEvents Ctx_mnuMap As System.Windows.Forms.ContextMenuStrip
	Public WithEvents Ctx_mnuSubmap As System.Windows.Forms.ContextMenuStrip
	Public WithEvents Ctx_mnuKey As System.Windows.Forms.ContextMenuStrip
	Public WithEvents Ctx_mnuStep As System.Windows.Forms.ContextMenuStrip
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.MainMenu1 = New System.Windows.Forms.MenuStrip
		Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFileOpen = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFileOpenTask = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFileOpenProcess = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFileSave = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFileLine2 = New System.Windows.Forms.ToolStripSeparator
		Me.mnuFileTransfer = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFileTransferImport = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFileTransferExport = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFileTest = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFilePromote = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFileLine3 = New System.Windows.Forms.ToolStripSeparator
		Me.mnuFileExit = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuTools = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuToolsUpdateLog = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuHelpAbout = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuMap = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuMapAddStep = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuStep = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuStepAddStep = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuStepAddSubmap = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuStepAddBlankStep = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuStepAddKey = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuStepDeleteStep = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuStepMoveUp = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuStepMoveDown = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuSubmap = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuSubmapAddBlankStep = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuSubmapDeleteSubmap = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuKey = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuKeyDeleteKey = New System.Windows.Forms.ToolStripMenuItem
		Me.picBG = New System.Windows.Forms.PictureBox
		Me.fmeAttrib = New System.Windows.Forms.GroupBox
		Me.fmeKey = New System.Windows.Forms.GroupBox
		Me.cmdKeyValueBrowse = New System.Windows.Forms.Button
		Me.txtKeyElementID = New System.Windows.Forms.TextBox
		Me.cmdKeySet = New System.Windows.Forms.Button
		Me.txtKeyName = New System.Windows.Forms.TextBox
		Me.txtKeyValue = New System.Windows.Forms.TextBox
		Me.txtDocTemplate = New System.Windows.Forms.TextBox
		Me.txtDocTempType = New System.Windows.Forms.TextBox
		Me.cboDocTempMode = New System.Windows.Forms.ComboBox
		Me.cboDiaryUserId = New System.Windows.Forms.ComboBox
		Me.cboDiaryUserGroupId = New System.Windows.Forms.ComboBox
		Me.cboDiaryTaskGroupId = New System.Windows.Forms.ComboBox
		Me.cboDiaryTaskId = New System.Windows.Forms.ComboBox
		Me.cboDiaryWMSteps = New System.Windows.Forms.ComboBox
		Me.lblKeyElementID = New System.Windows.Forms.Label
		Me.lblKeyValue = New System.Windows.Forms.Label
		Me.lblKeyName = New System.Windows.Forms.Label
		Me.lblKeyTempCode = New System.Windows.Forms.Label
		Me.lblTempDesc = New System.Windows.Forms.Label
		Me.lblCaption = New System.Windows.Forms.Label
		Me.fmeSubmap = New System.Windows.Forms.GroupBox
		Me.txtSubmapDescription = New System.Windows.Forms.TextBox
		Me.txtSubmapCode = New System.Windows.Forms.TextBox
		Me.cmdSubmapSet = New System.Windows.Forms.Button
		Me.txtSubmapElementID = New System.Windows.Forms.TextBox
		Me.lblSubmapDescription = New System.Windows.Forms.Label
		Me.lblSubmapCode = New System.Windows.Forms.Label
		Me.lblSubmapElementID = New System.Windows.Forms.Label
		Me.fmeStep = New System.Windows.Forms.GroupBox
		Me.tvwResumeStep = New System.Windows.Forms.TreeView
		Me.cmdShowResumeView = New System.Windows.Forms.Button
		Me.txtSubmap = New System.Windows.Forms.TextBox
		Me.txtCancelSteps = New System.Windows.Forms.TextBox
		Me.txtComponent = New System.Windows.Forms.TextBox
		Me.txtComponentAction = New System.Windows.Forms.TextBox
		Me.txtDescription = New System.Windows.Forms.TextBox
		Me.txtOKSteps = New System.Windows.Forms.TextBox
		Me.txtResumeStep = New System.Windows.Forms.TextBox
		Me.cboOKAction = New System.Windows.Forms.ComboBox
		Me.cboCancelAction = New System.Windows.Forms.ComboBox
		Me.chkCreateWMTask = New System.Windows.Forms.CheckBox
		Me.chkServerSide = New System.Windows.Forms.CheckBox
		Me.chkStepCore = New System.Windows.Forms.CheckBox
		Me.cmdStepSet = New System.Windows.Forms.Button
		Me.txtStepElementID = New System.Windows.Forms.TextBox
		Me.txtStepType = New System.Windows.Forms.TextBox
		Me.txtOKNewRoadmap = New System.Windows.Forms.TextBox
		Me.txtCancelNewRoadmap = New System.Windows.Forms.TextBox
		Me.lblDescription = New System.Windows.Forms.Label
		Me.lblCancelAction = New System.Windows.Forms.Label
		Me.lblCancelSteps = New System.Windows.Forms.Label
		Me.lblComponent = New System.Windows.Forms.Label
		Me.lblComponentAction = New System.Windows.Forms.Label
		Me.lblCreateWMTask = New System.Windows.Forms.Label
		Me.lblOKAction = New System.Windows.Forms.Label
		Me.lblOkSteps = New System.Windows.Forms.Label
		Me.lblResumeStep = New System.Windows.Forms.Label
		Me.lblServerSide = New System.Windows.Forms.Label
		Me.lblSubmap = New System.Windows.Forms.Label
		Me.lblStepCore = New System.Windows.Forms.Label
		Me.lblStepElementID = New System.Windows.Forms.Label
		Me.lblStepType = New System.Windows.Forms.Label
		Me.lblStepOKNewRoadmap = New System.Windows.Forms.Label
		Me.lblStepCancelNewRoadmap = New System.Windows.Forms.Label
		Me.fmeMap = New System.Windows.Forms.GroupBox
		Me.txtMapAutoClose = New System.Windows.Forms.TextBox
		Me.txtMapImageURL = New System.Windows.Forms.TextBox
		Me.txtMapNavigatorDriven = New System.Windows.Forms.TextBox
		Me.txtMapProcessMode = New System.Windows.Forms.TextBox
		Me.txtMapRoadmapName = New System.Windows.Forms.TextBox
		Me.txtMapTitle = New System.Windows.Forms.TextBox
		Me.txtMapTransactionType = New System.Windows.Forms.TextBox
		Me.txtMapWMTaskCode = New System.Windows.Forms.TextBox
		Me.txtMapWMTaskDescription = New System.Windows.Forms.TextBox
		Me.chkMapCore = New System.Windows.Forms.CheckBox
		Me.txtMapVersion = New System.Windows.Forms.TextBox
		Me.cmdMapSet = New System.Windows.Forms.Button
		Me.txtMapElementID = New System.Windows.Forms.TextBox
		Me.lblMapAutoClose = New System.Windows.Forms.Label
		Me.lblMapImageURL = New System.Windows.Forms.Label
		Me.lblMapNavigatorDriven = New System.Windows.Forms.Label
		Me.lblMapProcessMode = New System.Windows.Forms.Label
		Me.lblMapRoadmapName = New System.Windows.Forms.Label
		Me.lblMapTitle = New System.Windows.Forms.Label
		Me.lblMapTransactionType = New System.Windows.Forms.Label
		Me.lblMapWMTaskCode = New System.Windows.Forms.Label
		Me.lblMapWMTaskDescription = New System.Windows.Forms.Label
		Me.lblMapCore = New System.Windows.Forms.Label
		Me.lblMapVersion = New System.Windows.Forms.Label
		Me.lblMapElementID = New System.Windows.Forms.Label
		Me.tvwMap = New System.Windows.Forms.TreeView
		Me.lblSplitter = New System.Windows.Forms.Label
		Me.tbToolbar = New System.Windows.Forms.ToolStrip
		Me.CommonDialog1Open = New System.Windows.Forms.OpenFileDialog
		Me.CommonDialog1Save = New System.Windows.Forms.SaveFileDialog
		Me.sbStatus = New System.Windows.Forms.StatusStrip
		Me._sbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
		Me.imlIcons = New System.Windows.Forms.ImageList
		Me.imlToolbarIcons = New System.Windows.Forms.ImageList
		Me.picBG.SuspendLayout()
		Me.fmeAttrib.SuspendLayout()
		Me.fmeKey.SuspendLayout()
		Me.fmeSubmap.SuspendLayout()
		Me.fmeStep.SuspendLayout()
		Me.fmeMap.SuspendLayout()
		Me.sbStatus.SuspendLayout()
		Me.SuspendLayout()
		'Ctx_mnuMap
		Me.Ctx_mnuMap = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.Ctx_mnuMap.Size = New System.Drawing.Size(153, 26)
		'Ctx_mnuSubmap
		Me.Ctx_mnuSubmap = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.Ctx_mnuSubmap.Size = New System.Drawing.Size(153, 26)
		'Ctx_mnuKey
		Me.Ctx_mnuKey = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.Ctx_mnuKey.Size = New System.Drawing.Size(153, 26)
		'Ctx_mnuStep
		Me.Ctx_mnuStep = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.Ctx_mnuStep.Size = New System.Drawing.Size(153, 26)
		' 
		' MainMenu1
		' 
		Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuFile, Me.mnuTools, Me.mnuHelp, Me.mnuMap, Me.mnuStep, Me.mnuSubmap, Me.mnuKey})
		' 
		' mnuFile
		' 
		Me.mnuFile.Available = True
		Me.mnuFile.Checked = False
		Me.mnuFile.Enabled = True
		Me.mnuFile.Name = "mnuFile"
		Me.mnuFile.Text = "&File"
		Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuFileOpen, Me.mnuFileSave, Me.mnuFileLine2, Me.mnuFileTransfer, Me.mnuFileTest, Me.mnuFilePromote, Me.mnuFileLine3, Me.mnuFileExit})
		' 
		' mnuFileOpen
		' 
		Me.mnuFileOpen.Available = True
		Me.mnuFileOpen.Checked = False
		Me.mnuFileOpen.Enabled = True
		Me.mnuFileOpen.Name = "mnuFileOpen"
		Me.mnuFileOpen.Text = "&Open"
		Me.mnuFileOpen.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuFileOpenTask, Me.mnuFileOpenProcess})
		' 
		' mnuFileOpenTask
		' 
		Me.mnuFileOpenTask.Available = True
		Me.mnuFileOpenTask.Checked = False
		Me.mnuFileOpenTask.Enabled = True
		Me.mnuFileOpenTask.Name = "mnuFileOpenTask"
		Me.mnuFileOpenTask.Text = "By Task ..."
		' 
		' mnuFileOpenProcess
		' 
		Me.mnuFileOpenProcess.Available = True
		Me.mnuFileOpenProcess.Checked = False
		Me.mnuFileOpenProcess.Enabled = True
		Me.mnuFileOpenProcess.Name = "mnuFileOpenProcess"
		Me.mnuFileOpenProcess.Text = "By Process ..."
		' 
		' mnuFileSave
		' 
		Me.mnuFileSave.Available = True
		Me.mnuFileSave.Checked = False
		Me.mnuFileSave.Enabled = True
		Me.mnuFileSave.Name = "mnuFileSave"
		Me.mnuFileSave.Text = "&Save"
		' 
		' mnuFileLine2
		' 
		Me.mnuFileLine2.Available = True
		Me.mnuFileLine2.Enabled = True
		Me.mnuFileLine2.Name = "mnuFileLine2"
		' 
		' mnuFileTransfer
		' 
		Me.mnuFileTransfer.Available = True
		Me.mnuFileTransfer.Checked = False
		Me.mnuFileTransfer.Enabled = True
		Me.mnuFileTransfer.Name = "mnuFileTransfer"
		Me.mnuFileTransfer.Text = "Transfer"
		Me.mnuFileTransfer.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuFileTransferImport, Me.mnuFileTransferExport})
		' 
		' mnuFileTransferImport
		' 
		Me.mnuFileTransferImport.Available = True
		Me.mnuFileTransferImport.Checked = False
		Me.mnuFileTransferImport.Enabled = True
		Me.mnuFileTransferImport.Name = "mnuFileTransferImport"
		Me.mnuFileTransferImport.Text = "Import ..."
		' 
		' mnuFileTransferExport
		' 
		Me.mnuFileTransferExport.Available = True
		Me.mnuFileTransferExport.Checked = False
		Me.mnuFileTransferExport.Enabled = True
		Me.mnuFileTransferExport.Name = "mnuFileTransferExport"
		Me.mnuFileTransferExport.Text = "Export ..."
		' 
		' mnuFileTest
		' 
		Me.mnuFileTest.Available = True
		Me.mnuFileTest.Checked = False
		Me.mnuFileTest.Enabled = True
		Me.mnuFileTest.Name = "mnuFileTest"
		Me.mnuFileTest.Text = "Test run"
		' 
		' mnuFilePromote
		' 
		Me.mnuFilePromote.Available = True
		Me.mnuFilePromote.Checked = False
		Me.mnuFilePromote.Enabled = True
		Me.mnuFilePromote.Name = "mnuFilePromote"
		Me.mnuFilePromote.Text = "Promote ..."
		' 
		' mnuFileLine3
		' 
		Me.mnuFileLine3.Available = True
		Me.mnuFileLine3.Enabled = True
		Me.mnuFileLine3.Name = "mnuFileLine3"
		' 
		' mnuFileExit
		' 
		Me.mnuFileExit.Available = True
		Me.mnuFileExit.Checked = False
		Me.mnuFileExit.Enabled = True
		Me.mnuFileExit.Name = "mnuFileExit"
		Me.mnuFileExit.Text = "E&xit"
		' 
		' mnuTools
		' 
		Me.mnuTools.Available = False
		Me.mnuTools.Checked = False
		Me.mnuTools.Enabled = False
		Me.mnuTools.Name = "mnuTools"
		Me.mnuTools.Text = "&Tools"
		Me.mnuTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuToolsUpdateLog})
		' 
		' mnuToolsUpdateLog
		' 
		Me.mnuToolsUpdateLog.Available = True
		Me.mnuToolsUpdateLog.Checked = False
		Me.mnuToolsUpdateLog.Enabled = False
		Me.mnuToolsUpdateLog.Name = "mnuToolsUpdateLog"
		Me.mnuToolsUpdateLog.Text = "Update log"
		' 
		' mnuHelp
		' 
		Me.mnuHelp.Available = True
		Me.mnuHelp.Checked = False
		Me.mnuHelp.Enabled = True
		Me.mnuHelp.Name = "mnuHelp"
		Me.mnuHelp.Text = "&Help"
		Me.mnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuHelpAbout})
		' 
		' mnuHelpAbout
		' 
		Me.mnuHelpAbout.Available = True
		Me.mnuHelpAbout.Checked = False
		Me.mnuHelpAbout.Enabled = True
		Me.mnuHelpAbout.Name = "mnuHelpAbout"
		Me.mnuHelpAbout.Text = "&About PMNavXMEditor"
		' 
		' mnuMap
		' 
		Me.mnuMap.Available = True
		Me.mnuMap.Checked = False
		Me.mnuMap.Enabled = True
		Me.mnuMap.Name = "mnuMap"
		Me.mnuMap.Text = "Map"
		Me.mnuMap.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuMapAddStep})
		' 
		' mnuMapAddStep
		' 
		Me.mnuMapAddStep.Available = True
		Me.mnuMapAddStep.Checked = False
		Me.mnuMapAddStep.Enabled = True
		Me.mnuMapAddStep.Name = "mnuMapAddStep"
		Me.mnuMapAddStep.Text = "Add step"
		' 
		' mnuStep
		' 
		Me.mnuStep.Available = True
		Me.mnuStep.Checked = False
		Me.mnuStep.Enabled = True
		Me.mnuStep.Name = "mnuStep"
		Me.mnuStep.Text = "Step"
		Me.mnuStep.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuStepAddStep, Me.mnuStepAddSubmap, Me.mnuStepAddBlankStep, Me.mnuStepAddKey, Me.mnuStepDeleteStep, Me.mnuStepMoveUp, Me.mnuStepMoveDown})
		' 
		' mnuStepAddStep
		' 
		Me.mnuStepAddStep.Available = True
		Me.mnuStepAddStep.Checked = False
		Me.mnuStepAddStep.Enabled = True
		Me.mnuStepAddStep.Name = "mnuStepAddStep"
		Me.mnuStepAddStep.Text = "Add step"
		' 
		' mnuStepAddSubmap
		' 
		Me.mnuStepAddSubmap.Available = True
		Me.mnuStepAddSubmap.Checked = False
		Me.mnuStepAddSubmap.Enabled = True
		Me.mnuStepAddSubmap.Name = "mnuStepAddSubmap"
		Me.mnuStepAddSubmap.Text = "Add Submap"
		' 
		' mnuStepAddBlankStep
		' 
		Me.mnuStepAddBlankStep.Available = True
		Me.mnuStepAddBlankStep.Checked = False
		Me.mnuStepAddBlankStep.Enabled = True
		Me.mnuStepAddBlankStep.Name = "mnuStepAddBlankStep"
		Me.mnuStepAddBlankStep.Text = "Add blank step"
		' 
		' mnuStepAddKey
		' 
		Me.mnuStepAddKey.Available = True
		Me.mnuStepAddKey.Checked = False
		Me.mnuStepAddKey.Enabled = True
		Me.mnuStepAddKey.Name = "mnuStepAddKey"
		Me.mnuStepAddKey.Text = "Add key"
		' 
		' mnuStepDeleteStep
		' 
		Me.mnuStepDeleteStep.Available = True
		Me.mnuStepDeleteStep.Checked = False
		Me.mnuStepDeleteStep.Enabled = True
		Me.mnuStepDeleteStep.Name = "mnuStepDeleteStep"
		Me.mnuStepDeleteStep.Text = "Delete step"
		' 
		' mnuStepMoveUp
		' 
		Me.mnuStepMoveUp.Available = True
		Me.mnuStepMoveUp.Checked = False
		Me.mnuStepMoveUp.Enabled = True
		Me.mnuStepMoveUp.Name = "mnuStepMoveUp"
		Me.mnuStepMoveUp.Text = "Move up"
		' 
		' mnuStepMoveDown
		' 
		Me.mnuStepMoveDown.Available = True
		Me.mnuStepMoveDown.Checked = False
		Me.mnuStepMoveDown.Enabled = True
		Me.mnuStepMoveDown.Name = "mnuStepMoveDown"
		Me.mnuStepMoveDown.Text = "Move Down"
		' 
		' mnuSubmap
		' 
		Me.mnuSubmap.Available = True
		Me.mnuSubmap.Checked = False
		Me.mnuSubmap.Enabled = True
		Me.mnuSubmap.Name = "mnuSubmap"
		Me.mnuSubmap.Text = "Submap"
		Me.mnuSubmap.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuSubmapAddBlankStep, Me.mnuSubmapDeleteSubmap})
		' 
		' mnuSubmapAddBlankStep
		' 
		Me.mnuSubmapAddBlankStep.Available = True
		Me.mnuSubmapAddBlankStep.Checked = False
		Me.mnuSubmapAddBlankStep.Enabled = True
		Me.mnuSubmapAddBlankStep.Name = "mnuSubmapAddBlankStep"
		Me.mnuSubmapAddBlankStep.Text = "Add blank step"
		' 
		' mnuSubmapDeleteSubmap
		' 
		Me.mnuSubmapDeleteSubmap.Available = True
		Me.mnuSubmapDeleteSubmap.Checked = False
		Me.mnuSubmapDeleteSubmap.Enabled = True
		Me.mnuSubmapDeleteSubmap.Name = "mnuSubmapDeleteSubmap"
		Me.mnuSubmapDeleteSubmap.Text = "Delete submap"
		' 
		' mnuKey
		' 
		Me.mnuKey.Available = True
		Me.mnuKey.Checked = False
		Me.mnuKey.Enabled = True
		Me.mnuKey.Name = "mnuKey"
		Me.mnuKey.Text = "Key"
		Me.mnuKey.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuKeyDeleteKey})
		' 
		' mnuKeyDeleteKey
		' 
		Me.mnuKeyDeleteKey.Available = True
		Me.mnuKeyDeleteKey.Checked = False
		Me.mnuKeyDeleteKey.Enabled = True
		Me.mnuKeyDeleteKey.Name = "mnuKeyDeleteKey"
		Me.mnuKeyDeleteKey.Text = "Delete key"
		' 
		' picBG
		' 
		Me.picBG.BackColor = System.Drawing.SystemColors.Control
		Me.picBG.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.picBG.CausesValidation = True
		Me.picBG.Controls.Add(Me.fmeAttrib)
		Me.picBG.Controls.Add(Me.tvwMap)
		Me.picBG.Controls.Add(Me.lblSplitter)
		Me.picBG.Cursor = System.Windows.Forms.Cursors.Default
		Me.picBG.Dock = System.Windows.Forms.DockStyle.None
		Me.picBG.Enabled = True
		Me.picBG.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.picBG.Location = New System.Drawing.Point(-4, 52)
		Me.picBG.Name = "picBG"
		Me.picBG.Size = New System.Drawing.Size(689, 325)
		Me.picBG.TabIndex = 2
		Me.picBG.TabStop = True
		Me.picBG.Visible = True
		' 
		' fmeAttrib
		' 
		Me.fmeAttrib.BackColor = System.Drawing.SystemColors.Control
		Me.fmeAttrib.Controls.Add(Me.fmeKey)
		Me.fmeAttrib.Controls.Add(Me.fmeSubmap)
		Me.fmeAttrib.Controls.Add(Me.fmeStep)
		Me.fmeAttrib.Controls.Add(Me.fmeMap)
		Me.fmeAttrib.Enabled = True
		Me.fmeAttrib.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fmeAttrib.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fmeAttrib.Location = New System.Drawing.Point(224, 2)
		Me.fmeAttrib.Name = "fmeAttrib"
		Me.fmeAttrib.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fmeAttrib.Size = New System.Drawing.Size(453, 307)
		Me.fmeAttrib.TabIndex = 4
		Me.fmeAttrib.Visible = True
		' 
		' fmeKey
		' 
		Me.fmeKey.BackColor = System.Drawing.SystemColors.Control
		Me.fmeKey.Controls.Add(Me.cmdKeyValueBrowse)
		Me.fmeKey.Controls.Add(Me.txtKeyElementID)
		Me.fmeKey.Controls.Add(Me.cmdKeySet)
		Me.fmeKey.Controls.Add(Me.txtKeyName)
		Me.fmeKey.Controls.Add(Me.txtKeyValue)
		Me.fmeKey.Controls.Add(Me.txtDocTemplate)
		Me.fmeKey.Controls.Add(Me.txtDocTempType)
		Me.fmeKey.Controls.Add(Me.cboDocTempMode)
		Me.fmeKey.Controls.Add(Me.cboDiaryUserId)
		Me.fmeKey.Controls.Add(Me.cboDiaryUserGroupId)
		Me.fmeKey.Controls.Add(Me.cboDiaryTaskGroupId)
		Me.fmeKey.Controls.Add(Me.cboDiaryTaskId)
		Me.fmeKey.Controls.Add(Me.cboDiaryWMSteps)
		Me.fmeKey.Controls.Add(Me.lblKeyElementID)
		Me.fmeKey.Controls.Add(Me.lblKeyValue)
		Me.fmeKey.Controls.Add(Me.lblKeyName)
		Me.fmeKey.Controls.Add(Me.lblKeyTempCode)
		Me.fmeKey.Controls.Add(Me.lblTempDesc)
		Me.fmeKey.Controls.Add(Me.lblCaption)
		Me.fmeKey.Enabled = True
		Me.fmeKey.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fmeKey.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fmeKey.Location = New System.Drawing.Point(8, 12)
		Me.fmeKey.Name = "fmeKey"
		Me.fmeKey.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fmeKey.Size = New System.Drawing.Size(277, 225)
		Me.fmeKey.TabIndex = 76
		Me.fmeKey.Text = "Key"
		Me.fmeKey.Visible = True
		' 
		' cmdKeyValueBrowse
		' 
		Me.cmdKeyValueBrowse.BackColor = System.Drawing.SystemColors.Control
		Me.cmdKeyValueBrowse.CausesValidation = True
		Me.cmdKeyValueBrowse.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdKeyValueBrowse.Enabled = True
		Me.cmdKeyValueBrowse.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdKeyValueBrowse.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdKeyValueBrowse.Location = New System.Drawing.Point(200, 64)
		Me.cmdKeyValueBrowse.Name = "cmdKeyValueBrowse"
		Me.cmdKeyValueBrowse.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdKeyValueBrowse.Size = New System.Drawing.Size(21, 19)
		Me.cmdKeyValueBrowse.TabIndex = 89
		Me.cmdKeyValueBrowse.TabStop = True
		Me.cmdKeyValueBrowse.Text = "..."
		Me.cmdKeyValueBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdKeyValueBrowse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtKeyElementID
		' 
		Me.txtKeyElementID.AcceptsReturn = True
		Me.txtKeyElementID.AutoSize = False
		Me.txtKeyElementID.BackColor = System.Drawing.SystemColors.Window
		Me.txtKeyElementID.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtKeyElementID.CausesValidation = True
		Me.txtKeyElementID.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtKeyElementID.Enabled = True
		Me.txtKeyElementID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtKeyElementID.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtKeyElementID.HideSelection = True
		Me.txtKeyElementID.Location = New System.Drawing.Point(208, 28)
		Me.txtKeyElementID.MaxLength = 0
		Me.txtKeyElementID.Multiline = False
		Me.txtKeyElementID.Name = "txtKeyElementID"
		Me.txtKeyElementID.ReadOnly = False
		Me.txtKeyElementID.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtKeyElementID.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtKeyElementID.Size = New System.Drawing.Size(53, 19)
		Me.txtKeyElementID.TabIndex = 88
		Me.txtKeyElementID.TabStop = True
		Me.txtKeyElementID.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtKeyElementID.Visible = True
		' 
		' cmdKeySet
		' 
		Me.cmdKeySet.BackColor = System.Drawing.SystemColors.Control
		Me.cmdKeySet.CausesValidation = True
		Me.cmdKeySet.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdKeySet.Enabled = True
		Me.cmdKeySet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdKeySet.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdKeySet.Location = New System.Drawing.Point(64, 168)
		Me.cmdKeySet.Name = "cmdKeySet"
		Me.cmdKeySet.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdKeySet.Size = New System.Drawing.Size(53, 21)
		Me.cmdKeySet.TabIndex = 87
		Me.cmdKeySet.TabStop = True
		Me.cmdKeySet.Text = "Apply"
		Me.cmdKeySet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdKeySet.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtKeyName
		' 
		Me.txtKeyName.AcceptsReturn = True
		Me.txtKeyName.AutoSize = False
		Me.txtKeyName.BackColor = System.Drawing.SystemColors.Window
		Me.txtKeyName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtKeyName.CausesValidation = True
		Me.txtKeyName.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtKeyName.Enabled = True
		Me.txtKeyName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtKeyName.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtKeyName.HideSelection = True
		Me.txtKeyName.Location = New System.Drawing.Point(8, 28)
		Me.txtKeyName.MaxLength = 0
		Me.txtKeyName.Multiline = False
		Me.txtKeyName.Name = "txtKeyName"
		Me.txtKeyName.ReadOnly = False
		Me.txtKeyName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtKeyName.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtKeyName.Size = New System.Drawing.Size(189, 19)
		Me.txtKeyName.TabIndex = 86
		Me.txtKeyName.TabStop = True
		Me.txtKeyName.Tag = "lblKeyName"
		Me.txtKeyName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtKeyName.Visible = True
		' 
		' txtKeyValue
		' 
		Me.txtKeyValue.AcceptsReturn = True
		Me.txtKeyValue.AutoSize = False
		Me.txtKeyValue.BackColor = System.Drawing.SystemColors.Window
		Me.txtKeyValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtKeyValue.CausesValidation = True
		Me.txtKeyValue.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtKeyValue.Enabled = True
		Me.txtKeyValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtKeyValue.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtKeyValue.HideSelection = True
		Me.txtKeyValue.Location = New System.Drawing.Point(8, 64)
		Me.txtKeyValue.MaxLength = 0
		Me.txtKeyValue.Multiline = True
		Me.txtKeyValue.Name = "txtKeyValue"
		Me.txtKeyValue.ReadOnly = False
		Me.txtKeyValue.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtKeyValue.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtKeyValue.Size = New System.Drawing.Size(189, 19)
		Me.txtKeyValue.TabIndex = 85
		Me.txtKeyValue.TabStop = True
		Me.txtKeyValue.Tag = "lblKeyValue"
		Me.txtKeyValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtKeyValue.Visible = True
		' 
		' txtDocTemplate
		' 
		Me.txtDocTemplate.AcceptsReturn = True
		Me.txtDocTemplate.AutoSize = False
		Me.txtDocTemplate.BackColor = System.Drawing.SystemColors.Window
		Me.txtDocTemplate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDocTemplate.CausesValidation = True
		Me.txtDocTemplate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDocTemplate.Enabled = False
		Me.txtDocTemplate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDocTemplate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDocTemplate.HideSelection = True
		Me.txtDocTemplate.Location = New System.Drawing.Point(8, 144)
		Me.txtDocTemplate.MaxLength = 0
		Me.txtDocTemplate.Multiline = False
		Me.txtDocTemplate.Name = "txtDocTemplate"
		Me.txtDocTemplate.ReadOnly = False
		Me.txtDocTemplate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDocTemplate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDocTemplate.Size = New System.Drawing.Size(189, 19)
		Me.txtDocTemplate.TabIndex = 84
		Me.txtDocTemplate.TabStop = True
		Me.txtDocTemplate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDocTemplate.Visible = True
		' 
		' txtDocTempType
		' 
		Me.txtDocTempType.AcceptsReturn = True
		Me.txtDocTempType.AutoSize = False
		Me.txtDocTempType.BackColor = System.Drawing.SystemColors.Window
		Me.txtDocTempType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDocTempType.CausesValidation = True
		Me.txtDocTempType.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDocTempType.Enabled = False
		Me.txtDocTempType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDocTempType.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDocTempType.HideSelection = True
		Me.txtDocTempType.Location = New System.Drawing.Point(8, 112)
		Me.txtDocTempType.MaxLength = 0
		Me.txtDocTempType.Multiline = False
		Me.txtDocTempType.Name = "txtDocTempType"
		Me.txtDocTempType.ReadOnly = False
		Me.txtDocTempType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDocTempType.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDocTempType.Size = New System.Drawing.Size(189, 19)
		Me.txtDocTempType.TabIndex = 83
		Me.txtDocTempType.TabStop = True
		Me.txtDocTempType.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDocTempType.Visible = True
		' 
		' cboDocTempMode
		' 
		Me.cboDocTempMode.BackColor = System.Drawing.SystemColors.Window
		Me.cboDocTempMode.CausesValidation = True
		Me.cboDocTempMode.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboDocTempMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboDocTempMode.Enabled = True
		Me.cboDocTempMode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboDocTempMode.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboDocTempMode.IntegralHeight = True
		Me.cboDocTempMode.Location = New System.Drawing.Point(8, 104)
		Me.cboDocTempMode.Name = "cboDocTempMode"
		Me.cboDocTempMode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboDocTempMode.Size = New System.Drawing.Size(193, 21)
		Me.cboDocTempMode.Sorted = False
		Me.cboDocTempMode.TabIndex = 82
		Me.cboDocTempMode.TabStop = True
		Me.cboDocTempMode.Visible = True
		' 
		' cboDiaryUserId
		' 
		Me.cboDiaryUserId.BackColor = System.Drawing.SystemColors.Window
		Me.cboDiaryUserId.CausesValidation = True
		Me.cboDiaryUserId.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboDiaryUserId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboDiaryUserId.Enabled = True
		Me.cboDiaryUserId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboDiaryUserId.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboDiaryUserId.IntegralHeight = True
		Me.cboDiaryUserId.Location = New System.Drawing.Point(8, 104)
		Me.cboDiaryUserId.Name = "cboDiaryUserId"
		Me.cboDiaryUserId.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboDiaryUserId.Size = New System.Drawing.Size(193, 21)
		Me.cboDiaryUserId.Sorted = False
		Me.cboDiaryUserId.TabIndex = 81
		Me.cboDiaryUserId.TabStop = True
		Me.cboDiaryUserId.Visible = True
		' 
		' cboDiaryUserGroupId
		' 
		Me.cboDiaryUserGroupId.BackColor = System.Drawing.SystemColors.Window
		Me.cboDiaryUserGroupId.CausesValidation = True
		Me.cboDiaryUserGroupId.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboDiaryUserGroupId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboDiaryUserGroupId.Enabled = True
		Me.cboDiaryUserGroupId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboDiaryUserGroupId.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboDiaryUserGroupId.IntegralHeight = True
		Me.cboDiaryUserGroupId.Location = New System.Drawing.Point(8, 104)
		Me.cboDiaryUserGroupId.Name = "cboDiaryUserGroupId"
		Me.cboDiaryUserGroupId.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboDiaryUserGroupId.Size = New System.Drawing.Size(193, 21)
		Me.cboDiaryUserGroupId.Sorted = False
		Me.cboDiaryUserGroupId.TabIndex = 80
		Me.cboDiaryUserGroupId.TabStop = True
		Me.cboDiaryUserGroupId.Visible = True
		' 
		' cboDiaryTaskGroupId
		' 
		Me.cboDiaryTaskGroupId.BackColor = System.Drawing.SystemColors.Window
		Me.cboDiaryTaskGroupId.CausesValidation = True
		Me.cboDiaryTaskGroupId.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboDiaryTaskGroupId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboDiaryTaskGroupId.Enabled = True
		Me.cboDiaryTaskGroupId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboDiaryTaskGroupId.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboDiaryTaskGroupId.IntegralHeight = True
		Me.cboDiaryTaskGroupId.Location = New System.Drawing.Point(8, 104)
		Me.cboDiaryTaskGroupId.Name = "cboDiaryTaskGroupId"
		Me.cboDiaryTaskGroupId.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboDiaryTaskGroupId.Size = New System.Drawing.Size(193, 21)
		Me.cboDiaryTaskGroupId.Sorted = False
		Me.cboDiaryTaskGroupId.TabIndex = 79
		Me.cboDiaryTaskGroupId.TabStop = True
		Me.cboDiaryTaskGroupId.Visible = True
		' 
		' cboDiaryTaskId
		' 
		Me.cboDiaryTaskId.BackColor = System.Drawing.SystemColors.Window
		Me.cboDiaryTaskId.CausesValidation = True
		Me.cboDiaryTaskId.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboDiaryTaskId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboDiaryTaskId.Enabled = True
		Me.cboDiaryTaskId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboDiaryTaskId.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboDiaryTaskId.IntegralHeight = True
		Me.cboDiaryTaskId.Location = New System.Drawing.Point(8, 104)
		Me.cboDiaryTaskId.Name = "cboDiaryTaskId"
		Me.cboDiaryTaskId.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboDiaryTaskId.Size = New System.Drawing.Size(193, 21)
		Me.cboDiaryTaskId.Sorted = False
		Me.cboDiaryTaskId.TabIndex = 78
		Me.cboDiaryTaskId.TabStop = True
		Me.cboDiaryTaskId.Visible = True
		' 
		' cboDiaryWMSteps
		' 
		Me.cboDiaryWMSteps.BackColor = System.Drawing.SystemColors.Window
		Me.cboDiaryWMSteps.CausesValidation = True
		Me.cboDiaryWMSteps.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboDiaryWMSteps.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboDiaryWMSteps.Enabled = True
		Me.cboDiaryWMSteps.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboDiaryWMSteps.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboDiaryWMSteps.IntegralHeight = True
		Me.cboDiaryWMSteps.Location = New System.Drawing.Point(8, 104)
		Me.cboDiaryWMSteps.Name = "cboDiaryWMSteps"
		Me.cboDiaryWMSteps.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboDiaryWMSteps.Size = New System.Drawing.Size(193, 21)
		Me.cboDiaryWMSteps.Sorted = False
		Me.cboDiaryWMSteps.TabIndex = 77
		Me.cboDiaryWMSteps.TabStop = True
		Me.cboDiaryWMSteps.Visible = True
		' 
		' lblKeyElementID
		' 
		Me.lblKeyElementID.AutoSize = False
		Me.lblKeyElementID.BackColor = System.Drawing.SystemColors.Control
		Me.lblKeyElementID.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblKeyElementID.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblKeyElementID.Enabled = True
		Me.lblKeyElementID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblKeyElementID.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblKeyElementID.Location = New System.Drawing.Point(208, 16)
		Me.lblKeyElementID.Name = "lblKeyElementID"
		Me.lblKeyElementID.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblKeyElementID.Size = New System.Drawing.Size(57, 13)
		Me.lblKeyElementID.TabIndex = 95
		Me.lblKeyElementID.Text = "Element ID"
		Me.lblKeyElementID.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblKeyElementID.UseMnemonic = True
		Me.lblKeyElementID.Visible = True
		' 
		' lblKeyValue
		' 
		Me.lblKeyValue.AutoSize = False
		Me.lblKeyValue.BackColor = System.Drawing.SystemColors.Control
		Me.lblKeyValue.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblKeyValue.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblKeyValue.Enabled = True
		Me.lblKeyValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblKeyValue.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblKeyValue.Location = New System.Drawing.Point(8, 52)
		Me.lblKeyValue.Name = "lblKeyValue"
		Me.lblKeyValue.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblKeyValue.Size = New System.Drawing.Size(29, 13)
		Me.lblKeyValue.TabIndex = 94
		Me.lblKeyValue.Text = "Value"
		Me.lblKeyValue.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblKeyValue.UseMnemonic = True
		Me.lblKeyValue.Visible = True
		' 
		' lblKeyName
		' 
		Me.lblKeyName.AutoSize = False
		Me.lblKeyName.BackColor = System.Drawing.SystemColors.Control
		Me.lblKeyName.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblKeyName.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblKeyName.Enabled = True
		Me.lblKeyName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblKeyName.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblKeyName.Location = New System.Drawing.Point(8, 16)
		Me.lblKeyName.Name = "lblKeyName"
		Me.lblKeyName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblKeyName.Size = New System.Drawing.Size(33, 13)
		Me.lblKeyName.TabIndex = 93
		Me.lblKeyName.Text = "Name"
		Me.lblKeyName.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblKeyName.UseMnemonic = True
		Me.lblKeyName.Visible = True
		' 
		' lblKeyTempCode
		' 
		Me.lblKeyTempCode.AutoSize = False
		Me.lblKeyTempCode.BackColor = System.Drawing.SystemColors.Control
		Me.lblKeyTempCode.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblKeyTempCode.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblKeyTempCode.Enabled = True
		Me.lblKeyTempCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblKeyTempCode.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblKeyTempCode.Location = New System.Drawing.Point(8, 132)
		Me.lblKeyTempCode.Name = "lblKeyTempCode"
		Me.lblKeyTempCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblKeyTempCode.Size = New System.Drawing.Size(177, 13)
		Me.lblKeyTempCode.TabIndex = 92
		Me.lblKeyTempCode.Text = "Document Template"
		Me.lblKeyTempCode.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblKeyTempCode.UseMnemonic = True
		Me.lblKeyTempCode.Visible = True
		' 
		' lblTempDesc
		' 
		Me.lblTempDesc.AutoSize = False
		Me.lblTempDesc.BackColor = System.Drawing.SystemColors.Control
		Me.lblTempDesc.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTempDesc.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTempDesc.Enabled = True
		Me.lblTempDesc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTempDesc.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTempDesc.Location = New System.Drawing.Point(8, 100)
		Me.lblTempDesc.Name = "lblTempDesc"
		Me.lblTempDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTempDesc.Size = New System.Drawing.Size(177, 13)
		Me.lblTempDesc.TabIndex = 91
		Me.lblTempDesc.Text = "Document Type"
		Me.lblTempDesc.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTempDesc.UseMnemonic = True
		Me.lblTempDesc.Visible = True
		' 
		' lblCaption
		' 
		Me.lblCaption.AutoSize = False
		Me.lblCaption.BackColor = System.Drawing.SystemColors.Control
		Me.lblCaption.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCaption.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCaption.Enabled = True
		Me.lblCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCaption.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCaption.Location = New System.Drawing.Point(8, 92)
		Me.lblCaption.Name = "lblCaption"
		Me.lblCaption.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCaption.Size = New System.Drawing.Size(189, 13)
		Me.lblCaption.TabIndex = 90
		Me.lblCaption.Text = "Document Mode"
		Me.lblCaption.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCaption.UseMnemonic = True
		Me.lblCaption.Visible = True
		' 
		' fmeSubmap
		' 
		Me.fmeSubmap.BackColor = System.Drawing.SystemColors.Control
		Me.fmeSubmap.Controls.Add(Me.txtSubmapDescription)
		Me.fmeSubmap.Controls.Add(Me.txtSubmapCode)
		Me.fmeSubmap.Controls.Add(Me.cmdSubmapSet)
		Me.fmeSubmap.Controls.Add(Me.txtSubmapElementID)
		Me.fmeSubmap.Controls.Add(Me.lblSubmapDescription)
		Me.fmeSubmap.Controls.Add(Me.lblSubmapCode)
		Me.fmeSubmap.Controls.Add(Me.lblSubmapElementID)
		Me.fmeSubmap.Enabled = True
		Me.fmeSubmap.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fmeSubmap.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fmeSubmap.Location = New System.Drawing.Point(292, 12)
		Me.fmeSubmap.Name = "fmeSubmap"
		Me.fmeSubmap.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fmeSubmap.Size = New System.Drawing.Size(273, 165)
		Me.fmeSubmap.TabIndex = 67
		Me.fmeSubmap.Text = "Submap"
		Me.fmeSubmap.Visible = True
		' 
		' txtSubmapDescription
		' 
		Me.txtSubmapDescription.AcceptsReturn = True
		Me.txtSubmapDescription.AutoSize = False
		Me.txtSubmapDescription.BackColor = System.Drawing.SystemColors.Window
		Me.txtSubmapDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtSubmapDescription.CausesValidation = True
		Me.txtSubmapDescription.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtSubmapDescription.Enabled = True
		Me.txtSubmapDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtSubmapDescription.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtSubmapDescription.HideSelection = True
		Me.txtSubmapDescription.Location = New System.Drawing.Point(8, 76)
		Me.txtSubmapDescription.MaxLength = 0
		Me.txtSubmapDescription.Multiline = False
		Me.txtSubmapDescription.Name = "txtSubmapDescription"
		Me.txtSubmapDescription.ReadOnly = False
		Me.txtSubmapDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtSubmapDescription.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtSubmapDescription.Size = New System.Drawing.Size(189, 19)
		Me.txtSubmapDescription.TabIndex = 71
		Me.txtSubmapDescription.TabStop = True
		Me.txtSubmapDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtSubmapDescription.Visible = True
		' 
		' txtSubmapCode
		' 
		Me.txtSubmapCode.AcceptsReturn = True
		Me.txtSubmapCode.AutoSize = False
		Me.txtSubmapCode.BackColor = System.Drawing.SystemColors.Window
		Me.txtSubmapCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtSubmapCode.CausesValidation = True
		Me.txtSubmapCode.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtSubmapCode.Enabled = True
		Me.txtSubmapCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtSubmapCode.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtSubmapCode.HideSelection = True
		Me.txtSubmapCode.Location = New System.Drawing.Point(8, 36)
		Me.txtSubmapCode.MaxLength = 0
		Me.txtSubmapCode.Multiline = False
		Me.txtSubmapCode.Name = "txtSubmapCode"
		Me.txtSubmapCode.ReadOnly = False
		Me.txtSubmapCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtSubmapCode.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtSubmapCode.Size = New System.Drawing.Size(189, 19)
		Me.txtSubmapCode.TabIndex = 70
		Me.txtSubmapCode.TabStop = True
		Me.txtSubmapCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtSubmapCode.Visible = True
		' 
		' cmdSubmapSet
		' 
		Me.cmdSubmapSet.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSubmapSet.CausesValidation = True
		Me.cmdSubmapSet.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSubmapSet.Enabled = True
		Me.cmdSubmapSet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdSubmapSet.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSubmapSet.Location = New System.Drawing.Point(8, 116)
		Me.cmdSubmapSet.Name = "cmdSubmapSet"
		Me.cmdSubmapSet.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSubmapSet.Size = New System.Drawing.Size(53, 21)
		Me.cmdSubmapSet.TabIndex = 69
		Me.cmdSubmapSet.TabStop = True
		Me.cmdSubmapSet.Text = "Apply"
		Me.cmdSubmapSet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSubmapSet.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtSubmapElementID
		' 
		Me.txtSubmapElementID.AcceptsReturn = True
		Me.txtSubmapElementID.AutoSize = False
		Me.txtSubmapElementID.BackColor = System.Drawing.SystemColors.Window
		Me.txtSubmapElementID.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtSubmapElementID.CausesValidation = True
		Me.txtSubmapElementID.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtSubmapElementID.Enabled = True
		Me.txtSubmapElementID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtSubmapElementID.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtSubmapElementID.HideSelection = True
		Me.txtSubmapElementID.Location = New System.Drawing.Point(208, 36)
		Me.txtSubmapElementID.MaxLength = 0
		Me.txtSubmapElementID.Multiline = False
		Me.txtSubmapElementID.Name = "txtSubmapElementID"
		Me.txtSubmapElementID.ReadOnly = False
		Me.txtSubmapElementID.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtSubmapElementID.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtSubmapElementID.Size = New System.Drawing.Size(53, 19)
		Me.txtSubmapElementID.TabIndex = 68
		Me.txtSubmapElementID.TabStop = True
		Me.txtSubmapElementID.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtSubmapElementID.Visible = True
		' 
		' lblSubmapDescription
		' 
		Me.lblSubmapDescription.AutoSize = False
		Me.lblSubmapDescription.BackColor = System.Drawing.SystemColors.Control
		Me.lblSubmapDescription.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSubmapDescription.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSubmapDescription.Enabled = True
		Me.lblSubmapDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSubmapDescription.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSubmapDescription.Location = New System.Drawing.Point(8, 64)
		Me.lblSubmapDescription.Name = "lblSubmapDescription"
		Me.lblSubmapDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSubmapDescription.Size = New System.Drawing.Size(53, 13)
		Me.lblSubmapDescription.TabIndex = 74
		Me.lblSubmapDescription.Text = "Description"
		Me.lblSubmapDescription.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblSubmapDescription.UseMnemonic = True
		Me.lblSubmapDescription.Visible = True
		' 
		' lblSubmapCode
		' 
		Me.lblSubmapCode.AutoSize = False
		Me.lblSubmapCode.BackColor = System.Drawing.SystemColors.Control
		Me.lblSubmapCode.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSubmapCode.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSubmapCode.Enabled = True
		Me.lblSubmapCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSubmapCode.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSubmapCode.Location = New System.Drawing.Point(8, 24)
		Me.lblSubmapCode.Name = "lblSubmapCode"
		Me.lblSubmapCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSubmapCode.Size = New System.Drawing.Size(25, 13)
		Me.lblSubmapCode.TabIndex = 73
		Me.lblSubmapCode.Text = "Code"
		Me.lblSubmapCode.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblSubmapCode.UseMnemonic = True
		Me.lblSubmapCode.Visible = True
		' 
		' lblSubmapElementID
		' 
		Me.lblSubmapElementID.AutoSize = False
		Me.lblSubmapElementID.BackColor = System.Drawing.SystemColors.Control
		Me.lblSubmapElementID.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSubmapElementID.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSubmapElementID.Enabled = True
		Me.lblSubmapElementID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSubmapElementID.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSubmapElementID.Location = New System.Drawing.Point(208, 24)
		Me.lblSubmapElementID.Name = "lblSubmapElementID"
		Me.lblSubmapElementID.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSubmapElementID.Size = New System.Drawing.Size(57, 13)
		Me.lblSubmapElementID.TabIndex = 72
		Me.lblSubmapElementID.Text = "Element ID"
		Me.lblSubmapElementID.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblSubmapElementID.UseMnemonic = True
		Me.lblSubmapElementID.Visible = True
		' 
		' fmeStep
		' 
		Me.fmeStep.BackColor = System.Drawing.SystemColors.Control
		Me.fmeStep.Controls.Add(Me.tvwResumeStep)
		Me.fmeStep.Controls.Add(Me.cmdShowResumeView)
		Me.fmeStep.Controls.Add(Me.txtSubmap)
		Me.fmeStep.Controls.Add(Me.txtCancelSteps)
		Me.fmeStep.Controls.Add(Me.txtComponent)
		Me.fmeStep.Controls.Add(Me.txtComponentAction)
		Me.fmeStep.Controls.Add(Me.txtDescription)
		Me.fmeStep.Controls.Add(Me.txtOKSteps)
		Me.fmeStep.Controls.Add(Me.txtResumeStep)
		Me.fmeStep.Controls.Add(Me.cboOKAction)
		Me.fmeStep.Controls.Add(Me.cboCancelAction)
		Me.fmeStep.Controls.Add(Me.chkCreateWMTask)
		Me.fmeStep.Controls.Add(Me.chkServerSide)
		Me.fmeStep.Controls.Add(Me.chkStepCore)
		Me.fmeStep.Controls.Add(Me.cmdStepSet)
		Me.fmeStep.Controls.Add(Me.txtStepElementID)
		Me.fmeStep.Controls.Add(Me.txtStepType)
		Me.fmeStep.Controls.Add(Me.txtOKNewRoadmap)
		Me.fmeStep.Controls.Add(Me.txtCancelNewRoadmap)
		Me.fmeStep.Controls.Add(Me.lblDescription)
		Me.fmeStep.Controls.Add(Me.lblCancelAction)
		Me.fmeStep.Controls.Add(Me.lblCancelSteps)
		Me.fmeStep.Controls.Add(Me.lblComponent)
		Me.fmeStep.Controls.Add(Me.lblComponentAction)
		Me.fmeStep.Controls.Add(Me.lblCreateWMTask)
		Me.fmeStep.Controls.Add(Me.lblOKAction)
		Me.fmeStep.Controls.Add(Me.lblOkSteps)
		Me.fmeStep.Controls.Add(Me.lblResumeStep)
		Me.fmeStep.Controls.Add(Me.lblServerSide)
		Me.fmeStep.Controls.Add(Me.lblSubmap)
		Me.fmeStep.Controls.Add(Me.lblStepCore)
		Me.fmeStep.Controls.Add(Me.lblStepElementID)
		Me.fmeStep.Controls.Add(Me.lblStepType)
		Me.fmeStep.Controls.Add(Me.lblStepOKNewRoadmap)
		Me.fmeStep.Controls.Add(Me.lblStepCancelNewRoadmap)
		Me.fmeStep.Enabled = True
		Me.fmeStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fmeStep.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fmeStep.Location = New System.Drawing.Point(300, 176)
		Me.fmeStep.Name = "fmeStep"
		Me.fmeStep.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fmeStep.Size = New System.Drawing.Size(389, 401)
		Me.fmeStep.TabIndex = 5
		Me.fmeStep.Text = "Step"
		Me.fmeStep.Visible = True
		' 
		' tvwResumeStep
		' 
		Me.tvwResumeStep.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tvwResumeStep.CausesValidation = True
		Me.tvwResumeStep.Font = New System.Drawing.Font("Arial", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tvwResumeStep.ImageList = imlIcons
		Me.tvwResumeStep.Indent = 12
		Me.tvwResumeStep.LabelEdit = True
		Me.tvwResumeStep.Location = New System.Drawing.Point(156, 216)
		Me.tvwResumeStep.Name = "tvwResumeStep"
		Me.tvwResumeStep.Size = New System.Drawing.Size(221, 17)
		Me.tvwResumeStep.TabIndex = 6
		' 
		' cmdShowResumeView
		' 
		Me.cmdShowResumeView.BackColor = System.Drawing.SystemColors.Control
		Me.cmdShowResumeView.CausesValidation = True
		Me.cmdShowResumeView.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdShowResumeView.Enabled = True
		Me.cmdShowResumeView.Font = New System.Drawing.Font("Webdings", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 2)
		Me.cmdShowResumeView.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdShowResumeView.Location = New System.Drawing.Point(320, 200)
		Me.cmdShowResumeView.Name = "cmdShowResumeView"
		Me.cmdShowResumeView.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdShowResumeView.Size = New System.Drawing.Size(21, 18)
		Me.cmdShowResumeView.TabIndex = 24
		Me.cmdShowResumeView.TabStop = True
		Me.cmdShowResumeView.Text = "6"
		Me.cmdShowResumeView.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdShowResumeView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtSubmap
		' 
		Me.txtSubmap.AcceptsReturn = True
		Me.txtSubmap.AutoSize = False
		Me.txtSubmap.BackColor = System.Drawing.SystemColors.Window
		Me.txtSubmap.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtSubmap.CausesValidation = True
		Me.txtSubmap.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtSubmap.Enabled = True
		Me.txtSubmap.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtSubmap.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtSubmap.HideSelection = True
		Me.txtSubmap.Location = New System.Drawing.Point(8, 200)
		Me.txtSubmap.MaxLength = 0
		Me.txtSubmap.Multiline = False
		Me.txtSubmap.Name = "txtSubmap"
		Me.txtSubmap.ReadOnly = False
		Me.txtSubmap.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtSubmap.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtSubmap.Size = New System.Drawing.Size(137, 19)
		Me.txtSubmap.TabIndex = 23
		Me.txtSubmap.TabStop = True
		Me.txtSubmap.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtSubmap.Visible = True
		' 
		' txtCancelSteps
		' 
		Me.txtCancelSteps.AcceptsReturn = True
		Me.txtCancelSteps.AutoSize = False
		Me.txtCancelSteps.BackColor = System.Drawing.SystemColors.Window
		Me.txtCancelSteps.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtCancelSteps.CausesValidation = True
		Me.txtCancelSteps.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtCancelSteps.Enabled = True
		Me.txtCancelSteps.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtCancelSteps.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtCancelSteps.HideSelection = True
		Me.txtCancelSteps.Location = New System.Drawing.Point(156, 156)
		Me.txtCancelSteps.MaxLength = 0
		Me.txtCancelSteps.Multiline = False
		Me.txtCancelSteps.Name = "txtCancelSteps"
		Me.txtCancelSteps.ReadOnly = False
		Me.txtCancelSteps.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtCancelSteps.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtCancelSteps.Size = New System.Drawing.Size(41, 19)
		Me.txtCancelSteps.TabIndex = 22
		Me.txtCancelSteps.TabStop = True
		Me.txtCancelSteps.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtCancelSteps.Visible = True
		' 
		' txtComponent
		' 
		Me.txtComponent.AcceptsReturn = True
		Me.txtComponent.AutoSize = False
		Me.txtComponent.BackColor = System.Drawing.SystemColors.Window
		Me.txtComponent.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtComponent.CausesValidation = True
		Me.txtComponent.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtComponent.Enabled = True
		Me.txtComponent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtComponent.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtComponent.HideSelection = True
		Me.txtComponent.Location = New System.Drawing.Point(8, 76)
		Me.txtComponent.MaxLength = 0
		Me.txtComponent.Multiline = False
		Me.txtComponent.Name = "txtComponent"
		Me.txtComponent.ReadOnly = False
		Me.txtComponent.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtComponent.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtComponent.Size = New System.Drawing.Size(209, 19)
		Me.txtComponent.TabIndex = 21
		Me.txtComponent.TabStop = True
		Me.txtComponent.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtComponent.Visible = True
		' 
		' txtComponentAction
		' 
		Me.txtComponentAction.AcceptsReturn = True
		Me.txtComponentAction.AutoSize = False
		Me.txtComponentAction.BackColor = System.Drawing.SystemColors.Window
		Me.txtComponentAction.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtComponentAction.CausesValidation = True
		Me.txtComponentAction.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtComponentAction.Enabled = True
		Me.txtComponentAction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtComponentAction.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtComponentAction.HideSelection = True
		Me.txtComponentAction.Location = New System.Drawing.Point(232, 76)
		Me.txtComponentAction.MaxLength = 0
		Me.txtComponentAction.Multiline = False
		Me.txtComponentAction.Name = "txtComponentAction"
		Me.txtComponentAction.ReadOnly = False
		Me.txtComponentAction.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtComponentAction.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtComponentAction.Size = New System.Drawing.Size(85, 19)
		Me.txtComponentAction.TabIndex = 20
		Me.txtComponentAction.TabStop = True
		Me.txtComponentAction.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtComponentAction.Visible = True
		' 
		' txtDescription
		' 
		Me.txtDescription.AcceptsReturn = True
		Me.txtDescription.AutoSize = False
		Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
		Me.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDescription.CausesValidation = True
		Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDescription.Enabled = True
		Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDescription.HideSelection = True
		Me.txtDescription.Location = New System.Drawing.Point(8, 36)
		Me.txtDescription.MaxLength = 0
		Me.txtDescription.Multiline = False
		Me.txtDescription.Name = "txtDescription"
		Me.txtDescription.ReadOnly = False
		Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDescription.Size = New System.Drawing.Size(137, 19)
		Me.txtDescription.TabIndex = 19
		Me.txtDescription.TabStop = True
		Me.txtDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDescription.Visible = True
		' 
		' txtOKSteps
		' 
		Me.txtOKSteps.AcceptsReturn = True
		Me.txtOKSteps.AutoSize = False
		Me.txtOKSteps.BackColor = System.Drawing.SystemColors.Window
		Me.txtOKSteps.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtOKSteps.CausesValidation = True
		Me.txtOKSteps.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtOKSteps.Enabled = True
		Me.txtOKSteps.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtOKSteps.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtOKSteps.HideSelection = True
		Me.txtOKSteps.Location = New System.Drawing.Point(156, 116)
		Me.txtOKSteps.MaxLength = 0
		Me.txtOKSteps.Multiline = False
		Me.txtOKSteps.Name = "txtOKSteps"
		Me.txtOKSteps.ReadOnly = False
		Me.txtOKSteps.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtOKSteps.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtOKSteps.Size = New System.Drawing.Size(41, 19)
		Me.txtOKSteps.TabIndex = 18
		Me.txtOKSteps.TabStop = True
		Me.txtOKSteps.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtOKSteps.Visible = True
		' 
		' txtResumeStep
		' 
		Me.txtResumeStep.AcceptsReturn = True
		Me.txtResumeStep.AutoSize = False
		Me.txtResumeStep.BackColor = System.Drawing.SystemColors.Window
		Me.txtResumeStep.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtResumeStep.CausesValidation = True
		Me.txtResumeStep.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtResumeStep.Enabled = True
		Me.txtResumeStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtResumeStep.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtResumeStep.HideSelection = True
		Me.txtResumeStep.Location = New System.Drawing.Point(156, 200)
		Me.txtResumeStep.MaxLength = 0
		Me.txtResumeStep.Multiline = False
		Me.txtResumeStep.Name = "txtResumeStep"
		Me.txtResumeStep.ReadOnly = False
		Me.txtResumeStep.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtResumeStep.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtResumeStep.Size = New System.Drawing.Size(165, 19)
		Me.txtResumeStep.TabIndex = 17
		Me.txtResumeStep.TabStop = True
		Me.txtResumeStep.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtResumeStep.Visible = True
		' 
		' cboOKAction
		' 
		Me.cboOKAction.BackColor = System.Drawing.SystemColors.Window
		Me.cboOKAction.CausesValidation = True
		Me.cboOKAction.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboOKAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboOKAction.Enabled = True
		Me.cboOKAction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboOKAction.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboOKAction.IntegralHeight = True
		Me.cboOKAction.Location = New System.Drawing.Point(8, 116)
		Me.cboOKAction.Name = "cboOKAction"
		Me.cboOKAction.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboOKAction.Size = New System.Drawing.Size(137, 21)
		Me.cboOKAction.Sorted = False
		Me.cboOKAction.TabIndex = 16
		Me.cboOKAction.TabStop = True
		Me.cboOKAction.Visible = True
		' 
		' cboCancelAction
		' 
		Me.cboCancelAction.BackColor = System.Drawing.SystemColors.Window
		Me.cboCancelAction.CausesValidation = True
		Me.cboCancelAction.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboCancelAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboCancelAction.Enabled = True
		Me.cboCancelAction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboCancelAction.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboCancelAction.IntegralHeight = True
		Me.cboCancelAction.Location = New System.Drawing.Point(8, 156)
		Me.cboCancelAction.Name = "cboCancelAction"
		Me.cboCancelAction.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboCancelAction.Size = New System.Drawing.Size(137, 21)
		Me.cboCancelAction.Sorted = False
		Me.cboCancelAction.TabIndex = 15
		Me.cboCancelAction.TabStop = True
		Me.cboCancelAction.Visible = True
		' 
		' chkCreateWMTask
		' 
		Me.chkCreateWMTask.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkCreateWMTask.BackColor = System.Drawing.SystemColors.Control
		Me.chkCreateWMTask.CausesValidation = True
		Me.chkCreateWMTask.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkCreateWMTask.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkCreateWMTask.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkCreateWMTask.Enabled = True
		Me.chkCreateWMTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkCreateWMTask.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkCreateWMTask.Location = New System.Drawing.Point(156, 40)
		Me.chkCreateWMTask.Name = "chkCreateWMTask"
		Me.chkCreateWMTask.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkCreateWMTask.Size = New System.Drawing.Size(13, 13)
		Me.chkCreateWMTask.TabIndex = 14
		Me.chkCreateWMTask.TabStop = True
		Me.chkCreateWMTask.Text = ""
		Me.chkCreateWMTask.Visible = True
		' 
		' chkServerSide
		' 
		Me.chkServerSide.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkServerSide.BackColor = System.Drawing.SystemColors.Control
		Me.chkServerSide.CausesValidation = True
		Me.chkServerSide.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkServerSide.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkServerSide.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkServerSide.Enabled = True
		Me.chkServerSide.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkServerSide.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkServerSide.Location = New System.Drawing.Point(232, 120)
		Me.chkServerSide.Name = "chkServerSide"
		Me.chkServerSide.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkServerSide.Size = New System.Drawing.Size(13, 13)
		Me.chkServerSide.TabIndex = 13
		Me.chkServerSide.TabStop = True
		Me.chkServerSide.Text = ""
		Me.chkServerSide.Visible = True
		' 
		' chkStepCore
		' 
		Me.chkStepCore.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkStepCore.BackColor = System.Drawing.SystemColors.Control
		Me.chkStepCore.CausesValidation = True
		Me.chkStepCore.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkStepCore.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkStepCore.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkStepCore.Enabled = True
		Me.chkStepCore.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkStepCore.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkStepCore.Location = New System.Drawing.Point(244, 40)
		Me.chkStepCore.Name = "chkStepCore"
		Me.chkStepCore.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkStepCore.Size = New System.Drawing.Size(13, 13)
		Me.chkStepCore.TabIndex = 12
		Me.chkStepCore.TabStop = True
		Me.chkStepCore.Text = ""
		Me.chkStepCore.Visible = True
		' 
		' cmdStepSet
		' 
		Me.cmdStepSet.BackColor = System.Drawing.SystemColors.Control
		Me.cmdStepSet.CausesValidation = True
		Me.cmdStepSet.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdStepSet.Enabled = True
		Me.cmdStepSet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdStepSet.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdStepSet.Location = New System.Drawing.Point(8, 272)
		Me.cmdStepSet.Name = "cmdStepSet"
		Me.cmdStepSet.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdStepSet.Size = New System.Drawing.Size(53, 21)
		Me.cmdStepSet.TabIndex = 11
		Me.cmdStepSet.TabStop = True
		Me.cmdStepSet.Text = "Apply"
		Me.cmdStepSet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdStepSet.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtStepElementID
		' 
		Me.txtStepElementID.AcceptsReturn = True
		Me.txtStepElementID.AutoSize = False
		Me.txtStepElementID.BackColor = System.Drawing.SystemColors.Window
		Me.txtStepElementID.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtStepElementID.CausesValidation = True
		Me.txtStepElementID.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtStepElementID.Enabled = True
		Me.txtStepElementID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtStepElementID.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtStepElementID.HideSelection = True
		Me.txtStepElementID.Location = New System.Drawing.Point(288, 36)
		Me.txtStepElementID.MaxLength = 0
		Me.txtStepElementID.Multiline = False
		Me.txtStepElementID.Name = "txtStepElementID"
		Me.txtStepElementID.ReadOnly = False
		Me.txtStepElementID.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtStepElementID.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtStepElementID.Size = New System.Drawing.Size(53, 19)
		Me.txtStepElementID.TabIndex = 10
		Me.txtStepElementID.TabStop = True
		Me.txtStepElementID.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtStepElementID.Visible = True
		' 
		' txtStepType
		' 
		Me.txtStepType.AcceptsReturn = True
		Me.txtStepType.AutoSize = False
		Me.txtStepType.BackColor = System.Drawing.SystemColors.Window
		Me.txtStepType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtStepType.CausesValidation = True
		Me.txtStepType.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtStepType.Enabled = True
		Me.txtStepType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtStepType.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtStepType.HideSelection = True
		Me.txtStepType.Location = New System.Drawing.Point(232, 156)
		Me.txtStepType.MaxLength = 0
		Me.txtStepType.Multiline = False
		Me.txtStepType.Name = "txtStepType"
		Me.txtStepType.ReadOnly = False
		Me.txtStepType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtStepType.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtStepType.Size = New System.Drawing.Size(85, 19)
		Me.txtStepType.TabIndex = 9
		Me.txtStepType.TabStop = True
		Me.txtStepType.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtStepType.Visible = True
		' 
		' txtOKNewRoadmap
		' 
		Me.txtOKNewRoadmap.AcceptsReturn = True
		Me.txtOKNewRoadmap.AutoSize = False
		Me.txtOKNewRoadmap.BackColor = System.Drawing.SystemColors.Window
		Me.txtOKNewRoadmap.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtOKNewRoadmap.CausesValidation = True
		Me.txtOKNewRoadmap.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtOKNewRoadmap.Enabled = True
		Me.txtOKNewRoadmap.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtOKNewRoadmap.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtOKNewRoadmap.HideSelection = True
		Me.txtOKNewRoadmap.Location = New System.Drawing.Point(8, 244)
		Me.txtOKNewRoadmap.MaxLength = 0
		Me.txtOKNewRoadmap.Multiline = False
		Me.txtOKNewRoadmap.Name = "txtOKNewRoadmap"
		Me.txtOKNewRoadmap.ReadOnly = False
		Me.txtOKNewRoadmap.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtOKNewRoadmap.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtOKNewRoadmap.Size = New System.Drawing.Size(137, 19)
		Me.txtOKNewRoadmap.TabIndex = 8
		Me.txtOKNewRoadmap.TabStop = True
		Me.txtOKNewRoadmap.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtOKNewRoadmap.Visible = True
		' 
		' txtCancelNewRoadmap
		' 
		Me.txtCancelNewRoadmap.AcceptsReturn = True
		Me.txtCancelNewRoadmap.AutoSize = False
		Me.txtCancelNewRoadmap.BackColor = System.Drawing.SystemColors.Window
		Me.txtCancelNewRoadmap.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtCancelNewRoadmap.CausesValidation = True
		Me.txtCancelNewRoadmap.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtCancelNewRoadmap.Enabled = True
		Me.txtCancelNewRoadmap.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtCancelNewRoadmap.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtCancelNewRoadmap.HideSelection = True
		Me.txtCancelNewRoadmap.Location = New System.Drawing.Point(156, 244)
		Me.txtCancelNewRoadmap.MaxLength = 0
		Me.txtCancelNewRoadmap.Multiline = False
		Me.txtCancelNewRoadmap.Name = "txtCancelNewRoadmap"
		Me.txtCancelNewRoadmap.ReadOnly = False
		Me.txtCancelNewRoadmap.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtCancelNewRoadmap.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtCancelNewRoadmap.Size = New System.Drawing.Size(137, 19)
		Me.txtCancelNewRoadmap.TabIndex = 7
		Me.txtCancelNewRoadmap.TabStop = True
		Me.txtCancelNewRoadmap.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtCancelNewRoadmap.Visible = True
		' 
		' lblDescription
		' 
		Me.lblDescription.AutoSize = False
		Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
		Me.lblDescription.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDescription.Enabled = True
		Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDescription.Location = New System.Drawing.Point(8, 24)
		Me.lblDescription.Name = "lblDescription"
		Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDescription.Size = New System.Drawing.Size(85, 13)
		Me.lblDescription.TabIndex = 25
		Me.lblDescription.Text = "Description"
		Me.lblDescription.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDescription.UseMnemonic = True
		Me.lblDescription.Visible = True
		' 
		' lblCancelAction
		' 
		Me.lblCancelAction.AutoSize = False
		Me.lblCancelAction.BackColor = System.Drawing.SystemColors.Control
		Me.lblCancelAction.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCancelAction.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCancelAction.Enabled = True
		Me.lblCancelAction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCancelAction.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCancelAction.Location = New System.Drawing.Point(8, 144)
		Me.lblCancelAction.Name = "lblCancelAction"
		Me.lblCancelAction.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCancelAction.Size = New System.Drawing.Size(85, 13)
		Me.lblCancelAction.TabIndex = 40
		Me.lblCancelAction.Text = "Cancel Action"
		Me.lblCancelAction.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCancelAction.UseMnemonic = True
		Me.lblCancelAction.Visible = True
		' 
		' lblCancelSteps
		' 
		Me.lblCancelSteps.AutoSize = False
		Me.lblCancelSteps.BackColor = System.Drawing.SystemColors.Control
		Me.lblCancelSteps.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCancelSteps.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCancelSteps.Enabled = True
		Me.lblCancelSteps.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCancelSteps.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCancelSteps.Location = New System.Drawing.Point(156, 144)
		Me.lblCancelSteps.Name = "lblCancelSteps"
		Me.lblCancelSteps.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCancelSteps.Size = New System.Drawing.Size(73, 13)
		Me.lblCancelSteps.TabIndex = 39
		Me.lblCancelSteps.Text = "Cancel Steps"
		Me.lblCancelSteps.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCancelSteps.UseMnemonic = True
		Me.lblCancelSteps.Visible = True
		' 
		' lblComponent
		' 
		Me.lblComponent.AutoSize = False
		Me.lblComponent.BackColor = System.Drawing.SystemColors.Control
		Me.lblComponent.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblComponent.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblComponent.Enabled = True
		Me.lblComponent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblComponent.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblComponent.Location = New System.Drawing.Point(8, 64)
		Me.lblComponent.Name = "lblComponent"
		Me.lblComponent.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblComponent.Size = New System.Drawing.Size(85, 13)
		Me.lblComponent.TabIndex = 38
		Me.lblComponent.Text = "Component"
		Me.lblComponent.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblComponent.UseMnemonic = True
		Me.lblComponent.Visible = True
		' 
		' lblComponentAction
		' 
		Me.lblComponentAction.AutoSize = False
		Me.lblComponentAction.BackColor = System.Drawing.SystemColors.Control
		Me.lblComponentAction.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblComponentAction.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblComponentAction.Enabled = True
		Me.lblComponentAction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblComponentAction.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblComponentAction.Location = New System.Drawing.Point(232, 64)
		Me.lblComponentAction.Name = "lblComponentAction"
		Me.lblComponentAction.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblComponentAction.Size = New System.Drawing.Size(85, 13)
		Me.lblComponentAction.TabIndex = 37
		Me.lblComponentAction.Text = "ComponentAction"
		Me.lblComponentAction.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblComponentAction.UseMnemonic = True
		Me.lblComponentAction.Visible = True
		' 
		' lblCreateWMTask
		' 
		Me.lblCreateWMTask.AutoSize = False
		Me.lblCreateWMTask.BackColor = System.Drawing.SystemColors.Control
		Me.lblCreateWMTask.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCreateWMTask.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCreateWMTask.Enabled = True
		Me.lblCreateWMTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCreateWMTask.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCreateWMTask.Location = New System.Drawing.Point(156, 24)
		Me.lblCreateWMTask.Name = "lblCreateWMTask"
		Me.lblCreateWMTask.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCreateWMTask.Size = New System.Drawing.Size(77, 13)
		Me.lblCreateWMTask.TabIndex = 36
		Me.lblCreateWMTask.Text = "CreateWMTask"
		Me.lblCreateWMTask.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCreateWMTask.UseMnemonic = True
		Me.lblCreateWMTask.Visible = True
		' 
		' lblOKAction
		' 
		Me.lblOKAction.AutoSize = False
		Me.lblOKAction.BackColor = System.Drawing.SystemColors.Control
		Me.lblOKAction.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblOKAction.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblOKAction.Enabled = True
		Me.lblOKAction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblOKAction.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblOKAction.Location = New System.Drawing.Point(8, 104)
		Me.lblOKAction.Name = "lblOKAction"
		Me.lblOKAction.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblOKAction.Size = New System.Drawing.Size(85, 13)
		Me.lblOKAction.TabIndex = 35
		Me.lblOKAction.Text = "OK Action"
		Me.lblOKAction.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblOKAction.UseMnemonic = True
		Me.lblOKAction.Visible = True
		' 
		' lblOkSteps
		' 
		Me.lblOkSteps.AutoSize = False
		Me.lblOkSteps.BackColor = System.Drawing.SystemColors.Control
		Me.lblOkSteps.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblOkSteps.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblOkSteps.Enabled = True
		Me.lblOkSteps.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblOkSteps.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblOkSteps.Location = New System.Drawing.Point(156, 104)
		Me.lblOkSteps.Name = "lblOkSteps"
		Me.lblOkSteps.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblOkSteps.Size = New System.Drawing.Size(85, 13)
		Me.lblOkSteps.TabIndex = 34
		Me.lblOkSteps.Text = "OK Steps"
		Me.lblOkSteps.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblOkSteps.UseMnemonic = True
		Me.lblOkSteps.Visible = True
		' 
		' lblResumeStep
		' 
		Me.lblResumeStep.AutoSize = False
		Me.lblResumeStep.BackColor = System.Drawing.SystemColors.Control
		Me.lblResumeStep.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblResumeStep.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblResumeStep.Enabled = True
		Me.lblResumeStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblResumeStep.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblResumeStep.Location = New System.Drawing.Point(156, 188)
		Me.lblResumeStep.Name = "lblResumeStep"
		Me.lblResumeStep.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblResumeStep.Size = New System.Drawing.Size(65, 13)
		Me.lblResumeStep.TabIndex = 33
		Me.lblResumeStep.Text = "ResumeStep"
		Me.lblResumeStep.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblResumeStep.UseMnemonic = True
		Me.lblResumeStep.Visible = True
		' 
		' lblServerSide
		' 
		Me.lblServerSide.AutoSize = False
		Me.lblServerSide.BackColor = System.Drawing.SystemColors.Control
		Me.lblServerSide.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblServerSide.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblServerSide.Enabled = True
		Me.lblServerSide.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblServerSide.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblServerSide.Location = New System.Drawing.Point(232, 104)
		Me.lblServerSide.Name = "lblServerSide"
		Me.lblServerSide.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblServerSide.Size = New System.Drawing.Size(53, 13)
		Me.lblServerSide.TabIndex = 32
		Me.lblServerSide.Text = "Server-side"
		Me.lblServerSide.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblServerSide.UseMnemonic = True
		Me.lblServerSide.Visible = True
		' 
		' lblSubmap
		' 
		Me.lblSubmap.AutoSize = False
		Me.lblSubmap.BackColor = System.Drawing.SystemColors.Control
		Me.lblSubmap.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSubmap.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSubmap.Enabled = True
		Me.lblSubmap.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSubmap.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSubmap.Location = New System.Drawing.Point(8, 188)
		Me.lblSubmap.Name = "lblSubmap"
		Me.lblSubmap.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSubmap.Size = New System.Drawing.Size(85, 13)
		Me.lblSubmap.TabIndex = 31
		Me.lblSubmap.Text = "Submap"
		Me.lblSubmap.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblSubmap.UseMnemonic = True
		Me.lblSubmap.Visible = True
		' 
		' lblStepCore
		' 
		Me.lblStepCore.AutoSize = False
		Me.lblStepCore.BackColor = System.Drawing.SystemColors.Control
		Me.lblStepCore.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblStepCore.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblStepCore.Enabled = True
		Me.lblStepCore.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblStepCore.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblStepCore.Location = New System.Drawing.Point(244, 24)
		Me.lblStepCore.Name = "lblStepCore"
		Me.lblStepCore.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblStepCore.Size = New System.Drawing.Size(25, 13)
		Me.lblStepCore.TabIndex = 30
		Me.lblStepCore.Text = "Core"
		Me.lblStepCore.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblStepCore.UseMnemonic = True
		Me.lblStepCore.Visible = True
		' 
		' lblStepElementID
		' 
		Me.lblStepElementID.AutoSize = False
		Me.lblStepElementID.BackColor = System.Drawing.SystemColors.Control
		Me.lblStepElementID.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblStepElementID.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblStepElementID.Enabled = True
		Me.lblStepElementID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblStepElementID.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblStepElementID.Location = New System.Drawing.Point(288, 24)
		Me.lblStepElementID.Name = "lblStepElementID"
		Me.lblStepElementID.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblStepElementID.Size = New System.Drawing.Size(57, 13)
		Me.lblStepElementID.TabIndex = 29
		Me.lblStepElementID.Text = "Element ID"
		Me.lblStepElementID.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblStepElementID.UseMnemonic = True
		Me.lblStepElementID.Visible = True
		' 
		' lblStepType
		' 
		Me.lblStepType.AutoSize = False
		Me.lblStepType.BackColor = System.Drawing.SystemColors.Control
		Me.lblStepType.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblStepType.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblStepType.Enabled = True
		Me.lblStepType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblStepType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblStepType.Location = New System.Drawing.Point(232, 144)
		Me.lblStepType.Name = "lblStepType"
		Me.lblStepType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblStepType.Size = New System.Drawing.Size(29, 13)
		Me.lblStepType.TabIndex = 28
		Me.lblStepType.Text = "Type"
		Me.lblStepType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblStepType.UseMnemonic = True
		Me.lblStepType.Visible = True
		' 
		' lblStepOKNewRoadmap
		' 
		Me.lblStepOKNewRoadmap.AutoSize = False
		Me.lblStepOKNewRoadmap.BackColor = System.Drawing.SystemColors.Control
		Me.lblStepOKNewRoadmap.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblStepOKNewRoadmap.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblStepOKNewRoadmap.Enabled = True
		Me.lblStepOKNewRoadmap.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblStepOKNewRoadmap.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblStepOKNewRoadmap.Location = New System.Drawing.Point(8, 232)
		Me.lblStepOKNewRoadmap.Name = "lblStepOKNewRoadmap"
		Me.lblStepOKNewRoadmap.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblStepOKNewRoadmap.Size = New System.Drawing.Size(101, 13)
		Me.lblStepOKNewRoadmap.TabIndex = 27
		Me.lblStepOKNewRoadmap.Text = "Ok New Roadmap"
		Me.lblStepOKNewRoadmap.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblStepOKNewRoadmap.UseMnemonic = True
		Me.lblStepOKNewRoadmap.Visible = True
		' 
		' lblStepCancelNewRoadmap
		' 
		Me.lblStepCancelNewRoadmap.AutoSize = False
		Me.lblStepCancelNewRoadmap.BackColor = System.Drawing.SystemColors.Control
		Me.lblStepCancelNewRoadmap.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblStepCancelNewRoadmap.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblStepCancelNewRoadmap.Enabled = True
		Me.lblStepCancelNewRoadmap.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblStepCancelNewRoadmap.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblStepCancelNewRoadmap.Location = New System.Drawing.Point(156, 232)
		Me.lblStepCancelNewRoadmap.Name = "lblStepCancelNewRoadmap"
		Me.lblStepCancelNewRoadmap.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblStepCancelNewRoadmap.Size = New System.Drawing.Size(113, 13)
		Me.lblStepCancelNewRoadmap.TabIndex = 26
		Me.lblStepCancelNewRoadmap.Text = "Cancel New Roadmap"
		Me.lblStepCancelNewRoadmap.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblStepCancelNewRoadmap.UseMnemonic = True
		Me.lblStepCancelNewRoadmap.Visible = True
		' 
		' fmeMap
		' 
		Me.fmeMap.BackColor = System.Drawing.SystemColors.Control
		Me.fmeMap.Controls.Add(Me.txtMapAutoClose)
		Me.fmeMap.Controls.Add(Me.txtMapImageURL)
		Me.fmeMap.Controls.Add(Me.txtMapNavigatorDriven)
		Me.fmeMap.Controls.Add(Me.txtMapProcessMode)
		Me.fmeMap.Controls.Add(Me.txtMapRoadmapName)
		Me.fmeMap.Controls.Add(Me.txtMapTitle)
		Me.fmeMap.Controls.Add(Me.txtMapTransactionType)
		Me.fmeMap.Controls.Add(Me.txtMapWMTaskCode)
		Me.fmeMap.Controls.Add(Me.txtMapWMTaskDescription)
		Me.fmeMap.Controls.Add(Me.chkMapCore)
		Me.fmeMap.Controls.Add(Me.txtMapVersion)
		Me.fmeMap.Controls.Add(Me.cmdMapSet)
		Me.fmeMap.Controls.Add(Me.txtMapElementID)
		Me.fmeMap.Controls.Add(Me.lblMapAutoClose)
		Me.fmeMap.Controls.Add(Me.lblMapImageURL)
		Me.fmeMap.Controls.Add(Me.lblMapNavigatorDriven)
		Me.fmeMap.Controls.Add(Me.lblMapProcessMode)
		Me.fmeMap.Controls.Add(Me.lblMapRoadmapName)
		Me.fmeMap.Controls.Add(Me.lblMapTitle)
		Me.fmeMap.Controls.Add(Me.lblMapTransactionType)
		Me.fmeMap.Controls.Add(Me.lblMapWMTaskCode)
		Me.fmeMap.Controls.Add(Me.lblMapWMTaskDescription)
		Me.fmeMap.Controls.Add(Me.lblMapCore)
		Me.fmeMap.Controls.Add(Me.lblMapVersion)
		Me.fmeMap.Controls.Add(Me.lblMapElementID)
		Me.fmeMap.Enabled = True
		Me.fmeMap.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fmeMap.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fmeMap.Location = New System.Drawing.Point(20, 220)
		Me.fmeMap.Name = "fmeMap"
		Me.fmeMap.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fmeMap.Size = New System.Drawing.Size(313, 257)
		Me.fmeMap.TabIndex = 41
		Me.fmeMap.Text = "Map"
		Me.fmeMap.Visible = True
		' 
		' txtMapAutoClose
		' 
		Me.txtMapAutoClose.AcceptsReturn = True
		Me.txtMapAutoClose.AutoSize = False
		Me.txtMapAutoClose.BackColor = System.Drawing.SystemColors.Window
		Me.txtMapAutoClose.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtMapAutoClose.CausesValidation = True
		Me.txtMapAutoClose.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtMapAutoClose.Enabled = True
		Me.txtMapAutoClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtMapAutoClose.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtMapAutoClose.HideSelection = True
		Me.txtMapAutoClose.Location = New System.Drawing.Point(224, 200)
		Me.txtMapAutoClose.MaxLength = 0
		Me.txtMapAutoClose.Multiline = False
		Me.txtMapAutoClose.Name = "txtMapAutoClose"
		Me.txtMapAutoClose.ReadOnly = False
		Me.txtMapAutoClose.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtMapAutoClose.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtMapAutoClose.Size = New System.Drawing.Size(69, 19)
		Me.txtMapAutoClose.TabIndex = 54
		Me.txtMapAutoClose.TabStop = True
		Me.txtMapAutoClose.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtMapAutoClose.Visible = True
		' 
		' txtMapImageURL
		' 
		Me.txtMapImageURL.AcceptsReturn = True
		Me.txtMapImageURL.AutoSize = False
		Me.txtMapImageURL.BackColor = System.Drawing.SystemColors.Window
		Me.txtMapImageURL.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtMapImageURL.CausesValidation = True
		Me.txtMapImageURL.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtMapImageURL.Enabled = True
		Me.txtMapImageURL.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtMapImageURL.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtMapImageURL.HideSelection = True
		Me.txtMapImageURL.Location = New System.Drawing.Point(52, 116)
		Me.txtMapImageURL.MaxLength = 0
		Me.txtMapImageURL.Multiline = False
		Me.txtMapImageURL.Name = "txtMapImageURL"
		Me.txtMapImageURL.ReadOnly = False
		Me.txtMapImageURL.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtMapImageURL.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtMapImageURL.Size = New System.Drawing.Size(181, 19)
		Me.txtMapImageURL.TabIndex = 53
		Me.txtMapImageURL.TabStop = True
		Me.txtMapImageURL.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtMapImageURL.Visible = True
		' 
		' txtMapNavigatorDriven
		' 
		Me.txtMapNavigatorDriven.AcceptsReturn = True
		Me.txtMapNavigatorDriven.AutoSize = False
		Me.txtMapNavigatorDriven.BackColor = System.Drawing.SystemColors.Window
		Me.txtMapNavigatorDriven.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtMapNavigatorDriven.CausesValidation = True
		Me.txtMapNavigatorDriven.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtMapNavigatorDriven.Enabled = True
		Me.txtMapNavigatorDriven.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtMapNavigatorDriven.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtMapNavigatorDriven.HideSelection = True
		Me.txtMapNavigatorDriven.Location = New System.Drawing.Point(224, 76)
		Me.txtMapNavigatorDriven.MaxLength = 0
		Me.txtMapNavigatorDriven.Multiline = False
		Me.txtMapNavigatorDriven.Name = "txtMapNavigatorDriven"
		Me.txtMapNavigatorDriven.ReadOnly = False
		Me.txtMapNavigatorDriven.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtMapNavigatorDriven.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtMapNavigatorDriven.Size = New System.Drawing.Size(69, 19)
		Me.txtMapNavigatorDriven.TabIndex = 52
		Me.txtMapNavigatorDriven.TabStop = True
		Me.txtMapNavigatorDriven.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtMapNavigatorDriven.Visible = True
		' 
		' txtMapProcessMode
		' 
		Me.txtMapProcessMode.AcceptsReturn = True
		Me.txtMapProcessMode.AutoSize = False
		Me.txtMapProcessMode.BackColor = System.Drawing.SystemColors.Window
		Me.txtMapProcessMode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtMapProcessMode.CausesValidation = True
		Me.txtMapProcessMode.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtMapProcessMode.Enabled = True
		Me.txtMapProcessMode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtMapProcessMode.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtMapProcessMode.HideSelection = True
		Me.txtMapProcessMode.Location = New System.Drawing.Point(224, 36)
		Me.txtMapProcessMode.MaxLength = 0
		Me.txtMapProcessMode.Multiline = False
		Me.txtMapProcessMode.Name = "txtMapProcessMode"
		Me.txtMapProcessMode.ReadOnly = False
		Me.txtMapProcessMode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtMapProcessMode.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtMapProcessMode.Size = New System.Drawing.Size(69, 19)
		Me.txtMapProcessMode.TabIndex = 51
		Me.txtMapProcessMode.TabStop = True
		Me.txtMapProcessMode.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtMapProcessMode.Visible = True
		' 
		' txtMapRoadmapName
		' 
		Me.txtMapRoadmapName.AcceptsReturn = True
		Me.txtMapRoadmapName.AutoSize = False
		Me.txtMapRoadmapName.BackColor = System.Drawing.SystemColors.Window
		Me.txtMapRoadmapName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtMapRoadmapName.CausesValidation = True
		Me.txtMapRoadmapName.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtMapRoadmapName.Enabled = True
		Me.txtMapRoadmapName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtMapRoadmapName.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtMapRoadmapName.HideSelection = True
		Me.txtMapRoadmapName.Location = New System.Drawing.Point(8, 36)
		Me.txtMapRoadmapName.MaxLength = 0
		Me.txtMapRoadmapName.Multiline = False
		Me.txtMapRoadmapName.Name = "txtMapRoadmapName"
		Me.txtMapRoadmapName.ReadOnly = False
		Me.txtMapRoadmapName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtMapRoadmapName.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtMapRoadmapName.Size = New System.Drawing.Size(205, 19)
		Me.txtMapRoadmapName.TabIndex = 50
		Me.txtMapRoadmapName.TabStop = True
		Me.txtMapRoadmapName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtMapRoadmapName.Visible = True
		' 
		' txtMapTitle
		' 
		Me.txtMapTitle.AcceptsReturn = True
		Me.txtMapTitle.AutoSize = False
		Me.txtMapTitle.BackColor = System.Drawing.SystemColors.Window
		Me.txtMapTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtMapTitle.CausesValidation = True
		Me.txtMapTitle.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtMapTitle.Enabled = True
		Me.txtMapTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtMapTitle.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtMapTitle.HideSelection = True
		Me.txtMapTitle.Location = New System.Drawing.Point(8, 76)
		Me.txtMapTitle.MaxLength = 0
		Me.txtMapTitle.Multiline = False
		Me.txtMapTitle.Name = "txtMapTitle"
		Me.txtMapTitle.ReadOnly = False
		Me.txtMapTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtMapTitle.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtMapTitle.Size = New System.Drawing.Size(205, 19)
		Me.txtMapTitle.TabIndex = 49
		Me.txtMapTitle.TabStop = True
		Me.txtMapTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtMapTitle.Visible = True
		' 
		' txtMapTransactionType
		' 
		Me.txtMapTransactionType.AcceptsReturn = True
		Me.txtMapTransactionType.AutoSize = False
		Me.txtMapTransactionType.BackColor = System.Drawing.SystemColors.Window
		Me.txtMapTransactionType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtMapTransactionType.CausesValidation = True
		Me.txtMapTransactionType.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtMapTransactionType.Enabled = True
		Me.txtMapTransactionType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtMapTransactionType.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtMapTransactionType.HideSelection = True
		Me.txtMapTransactionType.Location = New System.Drawing.Point(8, 200)
		Me.txtMapTransactionType.MaxLength = 0
		Me.txtMapTransactionType.Multiline = False
		Me.txtMapTransactionType.Name = "txtMapTransactionType"
		Me.txtMapTransactionType.ReadOnly = False
		Me.txtMapTransactionType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtMapTransactionType.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtMapTransactionType.Size = New System.Drawing.Size(137, 19)
		Me.txtMapTransactionType.TabIndex = 48
		Me.txtMapTransactionType.TabStop = True
		Me.txtMapTransactionType.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtMapTransactionType.Visible = True
		' 
		' txtMapWMTaskCode
		' 
		Me.txtMapWMTaskCode.AcceptsReturn = True
		Me.txtMapWMTaskCode.AutoSize = False
		Me.txtMapWMTaskCode.BackColor = System.Drawing.SystemColors.Window
		Me.txtMapWMTaskCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtMapWMTaskCode.CausesValidation = True
		Me.txtMapWMTaskCode.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtMapWMTaskCode.Enabled = True
		Me.txtMapWMTaskCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtMapWMTaskCode.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtMapWMTaskCode.HideSelection = True
		Me.txtMapWMTaskCode.Location = New System.Drawing.Point(8, 156)
		Me.txtMapWMTaskCode.MaxLength = 0
		Me.txtMapWMTaskCode.Multiline = False
		Me.txtMapWMTaskCode.Name = "txtMapWMTaskCode"
		Me.txtMapWMTaskCode.ReadOnly = False
		Me.txtMapWMTaskCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtMapWMTaskCode.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtMapWMTaskCode.Size = New System.Drawing.Size(137, 19)
		Me.txtMapWMTaskCode.TabIndex = 47
		Me.txtMapWMTaskCode.TabStop = True
		Me.txtMapWMTaskCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtMapWMTaskCode.Visible = True
		' 
		' txtMapWMTaskDescription
		' 
		Me.txtMapWMTaskDescription.AcceptsReturn = True
		Me.txtMapWMTaskDescription.AutoSize = False
		Me.txtMapWMTaskDescription.BackColor = System.Drawing.SystemColors.Window
		Me.txtMapWMTaskDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtMapWMTaskDescription.CausesValidation = True
		Me.txtMapWMTaskDescription.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtMapWMTaskDescription.Enabled = True
		Me.txtMapWMTaskDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtMapWMTaskDescription.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtMapWMTaskDescription.HideSelection = True
		Me.txtMapWMTaskDescription.Location = New System.Drawing.Point(156, 156)
		Me.txtMapWMTaskDescription.MaxLength = 0
		Me.txtMapWMTaskDescription.Multiline = False
		Me.txtMapWMTaskDescription.Name = "txtMapWMTaskDescription"
		Me.txtMapWMTaskDescription.ReadOnly = False
		Me.txtMapWMTaskDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtMapWMTaskDescription.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtMapWMTaskDescription.Size = New System.Drawing.Size(137, 19)
		Me.txtMapWMTaskDescription.TabIndex = 46
		Me.txtMapWMTaskDescription.TabStop = True
		Me.txtMapWMTaskDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtMapWMTaskDescription.Visible = True
		' 
		' chkMapCore
		' 
		Me.chkMapCore.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkMapCore.BackColor = System.Drawing.SystemColors.Control
		Me.chkMapCore.CausesValidation = True
		Me.chkMapCore.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkMapCore.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkMapCore.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkMapCore.Enabled = True
		Me.chkMapCore.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkMapCore.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkMapCore.Location = New System.Drawing.Point(156, 204)
		Me.chkMapCore.Name = "chkMapCore"
		Me.chkMapCore.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkMapCore.Size = New System.Drawing.Size(17, 13)
		Me.chkMapCore.TabIndex = 45
		Me.chkMapCore.TabStop = True
		Me.chkMapCore.Text = ""
		Me.chkMapCore.Visible = True
		' 
		' txtMapVersion
		' 
		Me.txtMapVersion.AcceptsReturn = True
		Me.txtMapVersion.AutoSize = False
		Me.txtMapVersion.BackColor = System.Drawing.SystemColors.Window
		Me.txtMapVersion.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtMapVersion.CausesValidation = True
		Me.txtMapVersion.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtMapVersion.Enabled = True
		Me.txtMapVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtMapVersion.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtMapVersion.HideSelection = True
		Me.txtMapVersion.Location = New System.Drawing.Point(8, 116)
		Me.txtMapVersion.MaxLength = 0
		Me.txtMapVersion.Multiline = False
		Me.txtMapVersion.Name = "txtMapVersion"
		Me.txtMapVersion.ReadOnly = False
		Me.txtMapVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtMapVersion.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtMapVersion.Size = New System.Drawing.Size(29, 19)
		Me.txtMapVersion.TabIndex = 44
		Me.txtMapVersion.TabStop = True
		Me.txtMapVersion.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtMapVersion.Visible = True
		' 
		' cmdMapSet
		' 
		Me.cmdMapSet.BackColor = System.Drawing.SystemColors.Control
		Me.cmdMapSet.CausesValidation = True
		Me.cmdMapSet.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdMapSet.Enabled = True
		Me.cmdMapSet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdMapSet.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdMapSet.Location = New System.Drawing.Point(8, 224)
		Me.cmdMapSet.Name = "cmdMapSet"
		Me.cmdMapSet.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdMapSet.Size = New System.Drawing.Size(53, 21)
		Me.cmdMapSet.TabIndex = 43
		Me.cmdMapSet.TabStop = True
		Me.cmdMapSet.Text = "Apply"
		Me.cmdMapSet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdMapSet.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtMapElementID
		' 
		Me.txtMapElementID.AcceptsReturn = True
		Me.txtMapElementID.AutoSize = False
		Me.txtMapElementID.BackColor = System.Drawing.SystemColors.Window
		Me.txtMapElementID.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtMapElementID.CausesValidation = True
		Me.txtMapElementID.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtMapElementID.Enabled = True
		Me.txtMapElementID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtMapElementID.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtMapElementID.HideSelection = True
		Me.txtMapElementID.Location = New System.Drawing.Point(244, 116)
		Me.txtMapElementID.MaxLength = 0
		Me.txtMapElementID.Multiline = False
		Me.txtMapElementID.Name = "txtMapElementID"
		Me.txtMapElementID.ReadOnly = False
		Me.txtMapElementID.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtMapElementID.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtMapElementID.Size = New System.Drawing.Size(29, 19)
		Me.txtMapElementID.TabIndex = 42
		Me.txtMapElementID.TabStop = True
		Me.txtMapElementID.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtMapElementID.Visible = True
		' 
		' lblMapAutoClose
		' 
		Me.lblMapAutoClose.AutoSize = False
		Me.lblMapAutoClose.BackColor = System.Drawing.SystemColors.Control
		Me.lblMapAutoClose.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMapAutoClose.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMapAutoClose.Enabled = True
		Me.lblMapAutoClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMapAutoClose.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMapAutoClose.Location = New System.Drawing.Point(224, 188)
		Me.lblMapAutoClose.Name = "lblMapAutoClose"
		Me.lblMapAutoClose.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMapAutoClose.Size = New System.Drawing.Size(53, 13)
		Me.lblMapAutoClose.TabIndex = 66
		Me.lblMapAutoClose.Text = "Auto Close"
		Me.lblMapAutoClose.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMapAutoClose.UseMnemonic = True
		Me.lblMapAutoClose.Visible = True
		' 
		' lblMapImageURL
		' 
		Me.lblMapImageURL.AutoSize = False
		Me.lblMapImageURL.BackColor = System.Drawing.SystemColors.Control
		Me.lblMapImageURL.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMapImageURL.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMapImageURL.Enabled = True
		Me.lblMapImageURL.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMapImageURL.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMapImageURL.Location = New System.Drawing.Point(52, 104)
		Me.lblMapImageURL.Name = "lblMapImageURL"
		Me.lblMapImageURL.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMapImageURL.Size = New System.Drawing.Size(57, 13)
		Me.lblMapImageURL.TabIndex = 65
		Me.lblMapImageURL.Text = "Image URL"
		Me.lblMapImageURL.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMapImageURL.UseMnemonic = True
		Me.lblMapImageURL.Visible = True
		' 
		' lblMapNavigatorDriven
		' 
		Me.lblMapNavigatorDriven.AutoSize = False
		Me.lblMapNavigatorDriven.BackColor = System.Drawing.SystemColors.Control
		Me.lblMapNavigatorDriven.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMapNavigatorDriven.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMapNavigatorDriven.Enabled = True
		Me.lblMapNavigatorDriven.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMapNavigatorDriven.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMapNavigatorDriven.Location = New System.Drawing.Point(224, 64)
		Me.lblMapNavigatorDriven.Name = "lblMapNavigatorDriven"
		Me.lblMapNavigatorDriven.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMapNavigatorDriven.Size = New System.Drawing.Size(81, 13)
		Me.lblMapNavigatorDriven.TabIndex = 64
		Me.lblMapNavigatorDriven.Text = "Navigator driven"
		Me.lblMapNavigatorDriven.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMapNavigatorDriven.UseMnemonic = True
		Me.lblMapNavigatorDriven.Visible = True
		' 
		' lblMapProcessMode
		' 
		Me.lblMapProcessMode.AutoSize = False
		Me.lblMapProcessMode.BackColor = System.Drawing.SystemColors.Control
		Me.lblMapProcessMode.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMapProcessMode.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMapProcessMode.Enabled = True
		Me.lblMapProcessMode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMapProcessMode.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMapProcessMode.Location = New System.Drawing.Point(224, 24)
		Me.lblMapProcessMode.Name = "lblMapProcessMode"
		Me.lblMapProcessMode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMapProcessMode.Size = New System.Drawing.Size(69, 13)
		Me.lblMapProcessMode.TabIndex = 63
		Me.lblMapProcessMode.Text = "Process mode"
		Me.lblMapProcessMode.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMapProcessMode.UseMnemonic = True
		Me.lblMapProcessMode.Visible = True
		' 
		' lblMapRoadmapName
		' 
		Me.lblMapRoadmapName.AutoSize = False
		Me.lblMapRoadmapName.BackColor = System.Drawing.SystemColors.Control
		Me.lblMapRoadmapName.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMapRoadmapName.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMapRoadmapName.Enabled = True
		Me.lblMapRoadmapName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMapRoadmapName.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMapRoadmapName.Location = New System.Drawing.Point(8, 24)
		Me.lblMapRoadmapName.Name = "lblMapRoadmapName"
		Me.lblMapRoadmapName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMapRoadmapName.Size = New System.Drawing.Size(77, 13)
		Me.lblMapRoadmapName.TabIndex = 62
		Me.lblMapRoadmapName.Text = "Roadmap name"
		Me.lblMapRoadmapName.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMapRoadmapName.UseMnemonic = True
		Me.lblMapRoadmapName.Visible = True
		' 
		' lblMapTitle
		' 
		Me.lblMapTitle.AutoSize = False
		Me.lblMapTitle.BackColor = System.Drawing.SystemColors.Control
		Me.lblMapTitle.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMapTitle.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMapTitle.Enabled = True
		Me.lblMapTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMapTitle.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMapTitle.Location = New System.Drawing.Point(8, 64)
		Me.lblMapTitle.Name = "lblMapTitle"
		Me.lblMapTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMapTitle.Size = New System.Drawing.Size(37, 13)
		Me.lblMapTitle.TabIndex = 61
		Me.lblMapTitle.Text = "Title"
		Me.lblMapTitle.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMapTitle.UseMnemonic = True
		Me.lblMapTitle.Visible = True
		' 
		' lblMapTransactionType
		' 
		Me.lblMapTransactionType.AutoSize = False
		Me.lblMapTransactionType.BackColor = System.Drawing.SystemColors.Control
		Me.lblMapTransactionType.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMapTransactionType.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMapTransactionType.Enabled = True
		Me.lblMapTransactionType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMapTransactionType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMapTransactionType.Location = New System.Drawing.Point(8, 188)
		Me.lblMapTransactionType.Name = "lblMapTransactionType"
		Me.lblMapTransactionType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMapTransactionType.Size = New System.Drawing.Size(89, 13)
		Me.lblMapTransactionType.TabIndex = 60
		Me.lblMapTransactionType.Text = "Transaction type"
		Me.lblMapTransactionType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMapTransactionType.UseMnemonic = True
		Me.lblMapTransactionType.Visible = True
		' 
		' lblMapWMTaskCode
		' 
		Me.lblMapWMTaskCode.AutoSize = False
		Me.lblMapWMTaskCode.BackColor = System.Drawing.SystemColors.Control
		Me.lblMapWMTaskCode.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMapWMTaskCode.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMapWMTaskCode.Enabled = True
		Me.lblMapWMTaskCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMapWMTaskCode.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMapWMTaskCode.Location = New System.Drawing.Point(8, 144)
		Me.lblMapWMTaskCode.Name = "lblMapWMTaskCode"
		Me.lblMapWMTaskCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMapWMTaskCode.Size = New System.Drawing.Size(77, 13)
		Me.lblMapWMTaskCode.TabIndex = 59
		Me.lblMapWMTaskCode.Text = "WM Task Code"
		Me.lblMapWMTaskCode.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMapWMTaskCode.UseMnemonic = True
		Me.lblMapWMTaskCode.Visible = True
		' 
		' lblMapWMTaskDescription
		' 
		Me.lblMapWMTaskDescription.AutoSize = False
		Me.lblMapWMTaskDescription.BackColor = System.Drawing.SystemColors.Control
		Me.lblMapWMTaskDescription.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMapWMTaskDescription.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMapWMTaskDescription.Enabled = True
		Me.lblMapWMTaskDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMapWMTaskDescription.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMapWMTaskDescription.Location = New System.Drawing.Point(156, 144)
		Me.lblMapWMTaskDescription.Name = "lblMapWMTaskDescription"
		Me.lblMapWMTaskDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMapWMTaskDescription.Size = New System.Drawing.Size(105, 13)
		Me.lblMapWMTaskDescription.TabIndex = 58
		Me.lblMapWMTaskDescription.Text = "WM Task Description"
		Me.lblMapWMTaskDescription.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMapWMTaskDescription.UseMnemonic = True
		Me.lblMapWMTaskDescription.Visible = True
		' 
		' lblMapCore
		' 
		Me.lblMapCore.AutoSize = False
		Me.lblMapCore.BackColor = System.Drawing.SystemColors.Control
		Me.lblMapCore.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMapCore.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMapCore.Enabled = True
		Me.lblMapCore.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMapCore.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMapCore.Location = New System.Drawing.Point(156, 188)
		Me.lblMapCore.Name = "lblMapCore"
		Me.lblMapCore.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMapCore.Size = New System.Drawing.Size(29, 13)
		Me.lblMapCore.TabIndex = 57
		Me.lblMapCore.Text = "Core"
		Me.lblMapCore.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMapCore.UseMnemonic = True
		Me.lblMapCore.Visible = True
		' 
		' lblMapVersion
		' 
		Me.lblMapVersion.AutoSize = False
		Me.lblMapVersion.BackColor = System.Drawing.SystemColors.Control
		Me.lblMapVersion.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMapVersion.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMapVersion.Enabled = True
		Me.lblMapVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMapVersion.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMapVersion.Location = New System.Drawing.Point(8, 104)
		Me.lblMapVersion.Name = "lblMapVersion"
		Me.lblMapVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMapVersion.Size = New System.Drawing.Size(41, 13)
		Me.lblMapVersion.TabIndex = 56
		Me.lblMapVersion.Text = "Version"
		Me.lblMapVersion.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMapVersion.UseMnemonic = True
		Me.lblMapVersion.Visible = True
		' 
		' lblMapElementID
		' 
		Me.lblMapElementID.AutoSize = False
		Me.lblMapElementID.BackColor = System.Drawing.SystemColors.Control
		Me.lblMapElementID.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMapElementID.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMapElementID.Enabled = True
		Me.lblMapElementID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMapElementID.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMapElementID.Location = New System.Drawing.Point(244, 104)
		Me.lblMapElementID.Name = "lblMapElementID"
		Me.lblMapElementID.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMapElementID.Size = New System.Drawing.Size(53, 13)
		Me.lblMapElementID.TabIndex = 55
		Me.lblMapElementID.Text = "Element ID"
		Me.lblMapElementID.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMapElementID.UseMnemonic = True
		Me.lblMapElementID.Visible = True
		' 
		' tvwMap
		' 
		Me.tvwMap.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tvwMap.CausesValidation = True
		Me.tvwMap.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tvwMap.ImageList = imlIcons
		Me.tvwMap.Indent = 36
		Me.tvwMap.LabelEdit = True
		Me.tvwMap.Location = New System.Drawing.Point(8, 8)
		Me.tvwMap.Name = "tvwMap"
		Me.tvwMap.Size = New System.Drawing.Size(209, 93)
		Me.tvwMap.TabIndex = 3
		' 
		' lblSplitter
		' 
		Me.lblSplitter.AutoSize = False
		Me.lblSplitter.BackColor = System.Drawing.Color.Silver
		Me.lblSplitter.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSplitter.Cursor = System.Windows.Forms.Cursors.SizeWE
		Me.lblSplitter.Enabled = True
		Me.lblSplitter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSplitter.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSplitter.Location = New System.Drawing.Point(212, 24)
		Me.lblSplitter.Name = "lblSplitter"
		Me.lblSplitter.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSplitter.Size = New System.Drawing.Size(11, 249)
		Me.lblSplitter.TabIndex = 75
		Me.lblSplitter.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblSplitter.UseMnemonic = True
		Me.lblSplitter.Visible = True
		' 
		' tbToolbar
		' 
		Me.tbToolbar.Dock = System.Windows.Forms.DockStyle.Top
		Me.tbToolbar.ImageList = imlIcons
		Me.tbToolbar.Location = New System.Drawing.Point(0, 24)
		Me.tbToolbar.Name = "tbToolbar"
		Me.tbToolbar.ShowItemToolTips = True
		Me.tbToolbar.Size = New System.Drawing.Size(689, 28)
		Me.tbToolbar.TabIndex = 1
		' 
		' sbStatus
		' 
		Me.sbStatus.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.sbStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.sbStatus.Location = New System.Drawing.Point(0, 409)
		Me.sbStatus.Name = "sbStatus"
		Me.sbStatus.ShowItemToolTips = True
		Me.sbStatus.Size = New System.Drawing.Size(689, 17)
		Me.sbStatus.TabIndex = 0
		Me.sbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me._sbStatus_Panel1})
		' 
		' _sbStatus_Panel1
		' 
		Me._sbStatus_Panel1.AutoSize = True
		Me._sbStatus_Panel1.AutoSize = False
		Me._sbStatus_Panel1.BorderSides = CType(System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom, System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me._sbStatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
		Me._sbStatus_Panel1.DoubleClickEnabled = True
		Me._sbStatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
		Me._sbStatus_Panel1.Size = New System.Drawing.Size(671, 17)
		Me._sbStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._sbStatus_Panel1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
		' 
		' imlIcons
		' 
		Me.imlIcons.ImageSize = New System.Drawing.Size(16, 16)
		Me.imlIcons.ImageStream = CType(resources.GetObject("imlIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.imlIcons.TransparentColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.imlIcons.Images.SetKeyName(0, "")
		Me.imlIcons.Images.SetKeyName(1, "Delete")
		Me.imlIcons.Images.SetKeyName(2, "Add")
		Me.imlIcons.Images.SetKeyName(3, "MoveDown")
		Me.imlIcons.Images.SetKeyName(4, "MoveUp")
		Me.imlIcons.Images.SetKeyName(5, "Open")
		Me.imlIcons.Images.SetKeyName(6, "Save")
		Me.imlIcons.Images.SetKeyName(7, "Diary")
		Me.imlIcons.Images.SetKeyName(8, "EditText")
		Me.imlIcons.Images.SetKeyName(9, "StandardLetter")
		Me.imlIcons.Images.SetKeyName(10, "RaiseEvent")
		Me.imlIcons.Images.SetKeyName(11, "LaunchEXE")
		Me.imlIcons.Images.SetKeyName(12, "UserComponent")
		Me.imlIcons.Images.SetKeyName(13, "Roadmap")
		Me.imlIcons.Images.SetKeyName(14, "Process")
		Me.imlIcons.Images.SetKeyName(15, "StepFind")
		Me.imlIcons.Images.SetKeyName(16, "StepFindCross")
		Me.imlIcons.Images.SetKeyName(17, "StepFindTick")
		Me.imlIcons.Images.SetKeyName(18, "Key")
		Me.imlIcons.Images.SetKeyName(19, "StepFindGrey")
		Me.imlIcons.Images.SetKeyName(20, "StepNoForm")
		Me.imlIcons.Images.SetKeyName(21, "StepNoFormCross")
		Me.imlIcons.Images.SetKeyName(22, "StepNoFormGrey")
		Me.imlIcons.Images.SetKeyName(23, "StepNoFormTick")
		Me.imlIcons.Images.SetKeyName(24, "SubMap")
		Me.imlIcons.Images.SetKeyName(25, "SubMapGrey")
		Me.imlIcons.Images.SetKeyName(26, "StepDataForm")
		Me.imlIcons.Images.SetKeyName(27, "StepDataFormCross")
		Me.imlIcons.Images.SetKeyName(28, "StepDataFormGrey")
		Me.imlIcons.Images.SetKeyName(29, "StepDataFormTick")
		Me.imlIcons.Images.SetKeyName(30, "StepDecision")
		Me.imlIcons.Images.SetKeyName(31, "StepDecisionTick")
		Me.imlIcons.Images.SetKeyName(32, "StepDecisionCross")
		Me.imlIcons.Images.SetKeyName(33, "StepDecisionGrey")
		Me.imlIcons.Images.SetKeyName(34, "")
		Me.imlIcons.Images.SetKeyName(35, "StepPrint")
		' 
		' imlToolbarIcons
		' 
		Me.imlToolbarIcons.ImageSize = New System.Drawing.Size(16, 16)
		Me.imlToolbarIcons.ImageStream = CType(resources.GetObject("imlToolbarIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.imlToolbarIcons.TransparentColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.imlToolbarIcons.Images.SetKeyName(0, "Tab Center")
		Me.imlToolbarIcons.Images.SetKeyName(1, "Delete")
		Me.imlToolbarIcons.Images.SetKeyName(2, "Help")
		Me.imlToolbarIcons.Images.SetKeyName(3, "Open")
		Me.imlToolbarIcons.Images.SetKeyName(4, "Save")
		' 
		' frmInterface
		' 
		Me.Text = "PMNavXMEditor"
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.ClientSize = New System.Drawing.Size(689, 426)
		Me.ControlBox = True
		Me.Controls.Add(Me.picBG)
		Me.Controls.Add(MainMenu1)
		Me.Controls.Add(Me.tbToolbar)
		Me.Controls.Add(Me.sbStatus)
		Me.Enabled = True
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.MaximizeBox = True
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Icon = CType(resources.GetObject("frmInterface.Icon"), System.Drawing.Icon)
		Me.Location = New System.Drawing.Point(4, 42)
		Me.MinimizeBox = True
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Name = "frmInterface"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.picBG.ResumeLayout(False)
		Me.fmeAttrib.ResumeLayout(False)
		Me.fmeKey.ResumeLayout(False)
		Me.fmeSubmap.ResumeLayout(False)
		Me.fmeStep.ResumeLayout(False)
		Me.fmeMap.ResumeLayout(False)
		Me.sbStatus.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class