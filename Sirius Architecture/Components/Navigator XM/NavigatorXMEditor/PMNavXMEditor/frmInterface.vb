Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.IO
Imports System.Windows.Forms
Imports System.Xml
Imports SharedFiles
Imports System.Runtime.ExceptionServices
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	'
	' History:
	' CJB 060106 PN26730 Hid the Tools menu option as its only submenu item (update log) has
	'            not been developed fully yet (shows log but nothing yet adds to log!)
	'
	
	Private m_lStatus As gPMConstants.PMEReturnCode
	Private m_lReturn As Integer
	Private m_bRet As Boolean
	
	' map edit flag
	Private m_bDirty As Boolean
	Private m_bElDirty As Boolean
	Private m_bElementEdit As Boolean
	
	Private m_lNavXMProcessID As Integer
	Private m_lNavXMProcessVersionID As Integer
	
	' unique Element ID counter
	Private m_lMaxElementID As Integer
	
	Private m_sLastNodeKey As String = ""
	
	Private m_bExpanded As Boolean
	
	' Navigator XM roadmap path
	Private m_sNavFilePath As String = ""
	Private m_sFilename As String = ""
	
	Private m_sTestFilename As String = ""
	
	' roadmap temp storage
	Private m_sXML As String = ""
	
	Private sDescription As String = ""
	' used to store data for document template keys
	Private m_vDocTemplate( ,  ) As Object
	Private m_vDocTempList( ,  ) As Object
	
	' doc template array constants
	Private Const DOC_TEMPLATE_ID As Integer = 0 ' element id
	Private Const DOC_TEMPLATE_CODE As Integer = 1 ' template code
	Private Const DOC_TEMPLATE_TYPE As Integer = 2 ' type code
	Private Const DOC_TEMPLATE_DESC As Integer = 3 ' description
	
	' roadmap storage
	Private m_oXMLDOM As XmlDocument
	
	' action lists for OK and Cancel
	Private m_sActions( ,  ) As String
	
	' element types
	Private m_typMap As MainModule.Map = MainModule.Map.CreateInstance()
	Private m_typStep As MainModule.Step_Renamed = MainModule.Step_Renamed.CreateInstance()
	Private m_typSubmap As MainModule.Submap = MainModule.Submap.CreateInstance()
	Private m_typKey As MainModule.Key = MainModule.Key.CreateInstance()
	
	' current element
	Private m_oElementDOM As XmlElement
	
	' bPMNavXMEditor

    Private m_oBusiness As bPMNavXMEditor.Business

    ' for testing via Navigator XM
    Private WithEvents m_oNavigatorXM As iPMNavigatorXM.Interface_Renamed

    ' webdings characters - up/down arrows on resume step button
    Private Const CHARACTER_CURSOR_DOWN As String = "6"
    Private Const CHARACTER_CURSOR_UP As String = "5"

    ' attribute array, from PMNavXM_Available_Step
    Private Const STEP_ATTRIB_CODE As Integer = 0
    Private Const STEP_ATTRIB_DESCRIPTION As Integer = 1
    Private Const STEP_ATTRIB_COMPONENT As Integer = 2
    Private Const STEP_ATTRIB_TYPE As Integer = 3
    Private Const STEP_ATTRIB_CANCELACTION As Integer = 4
    Private Const STEP_ATTRIB_OKACTION As Integer = 5
    Private Const STEP_ATTRIB_OKSTEPS As Integer = 6
    Private Const STEP_ATTRIB_CANCELSTEPS As Integer = 7
    Private Const STEP_ATTRIB_COMPONENTACTION As Integer = 8
    Private Const STEP_ATTRIB_SERVERSIDE As Integer = 9
    Private Const STEP_ATTRIB_CREATEWMTASK As Integer = 10
    Private Const STEP_ATTRIB_RESUMESTEP As Integer = 11
    Private Const STEP_ATTRIB_CORE As Integer = 12
    Private Const STEP_ATTRIB_SUBMAP As Integer = 13
    Private Const STEP_ATTRIB_OKNEWROADMAP As Integer = 14
    Private Const STEP_ATTRIB_CANCELNEWROADMAP As Integer = 15

    ' key array used in roadmap validation
    Private Const KEYARRAY_PARENT As Integer = 0
    Private Const KEYARRAY_NAME As Integer = 1
    Private Const KEYARRAY_VALUE As Integer = 2

    Private Const ACClass As String = "frmInterface"

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Initialise the form
    '
    ' ***************************************************************** '
    Public Function Initialise() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try


            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' roadmap edited flag
            m_bDirty = False

            ' Get an instance of the business object via object manager
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bPMNavXMEditor.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of object manager, or
                ' user cancelled login
                Return result
            End If

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' get the path to the nav xml files
            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="NavigatorXMPath", r_sSettingValue:=m_sNavFilePath)

            m_sNavFilePath = m_sNavFilePath.Trim()

            If m_sNavFilePath = "" Then
                ' select failed
                MessageBox.Show("Failed to get path for NavigatorXM roadmap files", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            End If

            ' make sure it's got \ on the end
            If m_sNavFilePath.Substring(m_sNavFilePath.Length - 1) <> FILENAME_SEPARATOR Then
                m_sNavFilePath = m_sNavFilePath & FILENAME_SEPARATOR
            End If

            ' Set the mouse pointer to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Load (Standard Method)
    '
    ' Description: Load the form details
    '
    ' ***************************************************************** '
    Public Function Load_Renamed() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try


            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' build combo lists
            m_lReturn = BuildComboLists()
            m_bElDirty = False

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Load failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ShowForm (Standard Method)
    '
    ' Description: Show the form using the display state passed
    '
    ' ***************************************************************** '
    Public Function ShowForm(ByRef lDisplayState As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Show the the form, allow user input etc.
            VB6.ShowForm(Me, lDisplayState)

            Return result

        Catch excep As System.Exception



            ' Error Section
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowForm failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Dim oButton As ToolStripButton

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If g_lUserMode <> USER_MODE_ADMIN Then
                mnuFileOpenProcess.Available = False
            End If

            lblSplitter.Cursor = Cursors.SizeWE ' resize, W-E
            lblSplitter.Top = tvwMap.Top ' + 15

            ' hide attribute frames
            fmeMap.Visible = False
            fmeStep.Visible = False
            fmeSubmap.Visible = False
            fmeKey.Visible = False

            ' hide their captions
            fmeMap.Text = ""
            fmeStep.Text = ""
            fmeSubmap.Text = ""
            fmeKey.Text = ""

            ' frames enabled under particular circumstances:
            ' Sirius user - all elements
            ' Customer - non-core elements only
            fmeMap.Enabled = False
            fmeSubmap.Enabled = False
            fmeKey.Enabled = False

            ' frame positions
            fmeMap.Top = 0
            fmeMap.Left = 0

            fmeStep.Top = 0
            fmeStep.Left = 0

            fmeSubmap.Top = 0
            fmeSubmap.Left = 0

            fmeKey.Top = 0
            fmeKey.Left = 0

            ' popup menus
            mnuMap.Available = False
            mnuStep.Available = False
            mnuSubmap.Available = False
            mnuKey.Available = False

            ' toolbar buttons
            oButton = New ToolStripButton()
            tbToolbar.Items.Add(oButton)
            oButton.Name = "Open"
            oButton.ImageKey = "Open"
            oButton.ToolTipText = "Open task roadmap"

            oButton = New ToolStripButton()
            tbToolbar.Items.Add(oButton)
            oButton.Name = "Save"
            oButton.ImageKey = "Save"
            oButton.ToolTipText = "Save roadmap"

            'oButton = New ToolStripSeparator()
            oButton = New ToolStripButton()
            tbToolbar.Items.Add(oButton)
            oButton.Name = "sep1"

            oButton = New ToolStripButton()
            tbToolbar.Items.Add(oButton)
            oButton.Name = "Delete"
            oButton.ImageKey = "Delete"
            oButton.ToolTipText = "Delete element"

            'oButton = New ToolStripSeparator()
            oButton = New ToolStripButton()
            tbToolbar.Items.Add(oButton)
            oButton.Name = "sep2"

            oButton = New ToolStripButton()
            tbToolbar.Items.Add(oButton)
            oButton.Name = "MoveUp"
            oButton.ImageKey = "MoveUp"
            oButton.ToolTipText = "Move step up"

            oButton = New ToolStripButton()
            tbToolbar.Items.Add(oButton)
            oButton.Name = "MoveDown"
            oButton.ImageKey = "MoveDown"
            oButton.ToolTipText = "Move step down"

            StatusMsg("Ready")

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            m_vDocTemplate = Nothing
            m_vDocTempList = Nothing

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetInterfaceDefaults failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Sub cboDiaryTaskGroupId_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboDiaryTaskGroupId.SelectedIndexChanged

        If cboDiaryTaskGroupId.SelectedIndex > 0 Then
            txtKeyValue.Text = CStr(VB6.GetItemData(cboDiaryTaskGroupId, cboDiaryTaskGroupId.SelectedIndex))
            cmdKeySet.Enabled = True
            m_bElDirty = True
        End If

    End Sub

    Private Sub cboDiaryTaskId_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboDiaryTaskId.SelectedIndexChanged

        If cboDiaryTaskId.SelectedIndex > 0 Then
            txtKeyValue.Text = CStr(VB6.GetItemData(cboDiaryTaskId, cboDiaryTaskId.SelectedIndex))
            cmdKeySet.Enabled = True
            m_bElDirty = True
        End If

    End Sub

    Private Sub cboDiaryUserGroupId_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboDiaryUserGroupId.SelectedIndexChanged

        If cboDiaryUserGroupId.SelectedIndex <> -1 Then
            txtKeyValue.Text = CStr(VB6.GetItemData(cboDiaryUserGroupId, cboDiaryUserGroupId.SelectedIndex))
            cmdKeySet.Enabled = True
            m_bElDirty = True
        End If

    End Sub

    Private Sub cboDiaryUserId_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboDiaryUserId.SelectedIndexChanged

        If cboDiaryUserId.SelectedIndex <> -1 Then
            txtKeyValue.Text = CStr(VB6.GetItemData(cboDiaryUserId, cboDiaryUserId.SelectedIndex))
            cmdKeySet.Enabled = True
            m_bElDirty = True
        End If

    End Sub

    Private Sub cboDiaryWMSteps_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboDiaryWMSteps.SelectedIndexChanged

        If cboDiaryWMSteps.SelectedIndex <> -1 Then
            txtKeyValue.Text = cboDiaryWMSteps.Text.Trim()
            cmdKeySet.Enabled = True
            m_bElDirty = True
        End If

    End Sub

    Private Sub cboDocTempMode_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboDocTempMode.SelectedIndexChanged

        If cboDocTempMode.SelectedIndex <> -1 Then
            txtKeyValue.Text = CStr(VB6.GetItemData(cboDocTempMode, cboDocTempMode.SelectedIndex))
            cmdKeySet.Enabled = True
            m_bElDirty = True
        End If

    End Sub

    Private Sub cmdKeyValueBrowse_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdKeyValueBrowse.Click
        m_lReturn = GetKeyValue()
    End Sub

    Private Sub Form_Initialize_Renamed()

        ' enable form in taskbar
        iPMFunc.ShowFormInTaskBar_Attach()

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' enable form in taskbar
        iPMFunc.ShowFormInTaskBar_Detach()

    End Sub

    Private Sub lblSplitter_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lblSplitter.MouseMove
        Dim iButton As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim XPos As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        m_lReturn = SplitterMove(iButton, XPos)
    End Sub

    Public Sub mnuFilePromote_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFilePromote.Click
        m_lReturn = PromoteVersion()
    End Sub

    Public Sub mnuFileTest_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileTest.Click
        m_lReturn = TestRunRoadmap()
    End Sub

    Public Sub mnuFileTransferExport_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileTransferExport.Click
        m_lReturn = ExportRoadmap()
    End Sub

    Public Sub mnuFileTransferImport_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileTransferImport.Click
        m_lReturn = ImportRoadmap()
    End Sub

    Public Sub mnuHelpAbout_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuHelpAbout.Click

        Dim oPMAbout As iPMAbout.Interface_Renamed

        Dim sTitle, sVersionNumber, sVersionDate, sComponent As String
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sVersion, sRelease, sSiriusType, sInstallDate As String

        Try

            ' Set the application title
            sTitle = "Sirius Navigator Toolkit"
            sComponent = "Navigator Roadmap Editor"

            ' get Sirius version info
            lReturn = CType(gPMFunctions.GetSiriusVersion(sVersion, sRelease, sSiriusType, sInstallDate), gPMConstants.PMEReturnCode)

            ' Set the version number and date
            If sSiriusType = "" Then
                sVersionNumber = "Sirius Architecture v" & sVersion & ", sr" & sRelease
            Else
                sVersionNumber = "Sirius for " & sSiriusType & " v" & sVersion & ", sr" & sRelease
            End If

            sVersionDate = sInstallDate

            ' Create the object
            oPMAbout = New iPMAbout.Interface_Renamed()

            ' Initialise it. No parameters
            lReturn = CType(oPMAbout, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            ' Display the about screen modally
            lReturn = oPMAbout.Show(sTitle:=sTitle, sVersionNumber:=sVersionNumber, sVersionDate:=sVersionDate, sComponent:=sComponent)

            ' Terminate it, and...
		oPMAbout.Dispose()

            ' ...remove it from memory
            oPMAbout = Nothing

        Catch



            Exit Sub
        End Try


    End Sub

    Public Sub mnuKeyDeleteKey_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuKeyDeleteKey.Click
        m_lReturn = DeleteElement(sType:="Key", sName:="Name")
    End Sub

    Public Sub mnuStepAddStep_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuStepAddStep.Click
        m_lReturn = StepAddStep()
    End Sub

    Public Sub mnuFileExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileExit.Click

        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        Me.Close()

    End Sub

    Public Sub mnuFileOpenProcess_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileOpenProcess.Click
        m_lReturn = OpenRoadmapByProcess()
    End Sub

    Public Sub mnuFileOpenTask_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileOpenTask.Click
        m_lReturn = OpenRoadmapByTask()
    End Sub

    Public Sub mnuFileSave_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileSave.Click
        m_lReturn = FileSave()
    End Sub

    Public Sub mnuMapAddStep_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuMapAddStep.Click
        m_lReturn = MapAddStep()
    End Sub

    Public Sub mnuStepAddBlankStep_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuStepAddBlankStep.Click
        m_lReturn = StepAddBlankStep()
    End Sub

    Public Sub mnuStepAddKey_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuStepAddKey.Click
        m_lReturn = StepAddKey()
    End Sub

    Public Sub mnuStepAddSubmap_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuStepAddSubmap.Click
        m_lReturn = StepAddSubmap()
    End Sub

    Public Sub mnuStepDeleteStep_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuStepDeleteStep.Click
        m_lReturn = DeleteElement(sType:="Step", sName:="Description")
    End Sub

    Public Sub mnuStepMoveDown_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuStepMoveDown.Click
        m_lReturn = StepMoveDown()
    End Sub

    Public Sub mnuStepMoveUp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuStepMoveUp.Click
        m_lReturn = StepMoveUp()
    End Sub

    Public Sub mnuSubmapAddBlankStep_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuSubmapAddBlankStep.Click
        m_lReturn = SubmapAddBlankStep()
    End Sub

    Public Sub mnuSubmapDeleteSubmap_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuSubmapDeleteSubmap.Click
        m_lReturn = DeleteElement(sType:="Submap", sName:="Description")
    End Sub

    Private Sub cboCancelAction_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCancelAction.SelectedIndexChanged

        ' enable/disable CancelSteps depending on value of CancelAction
        Select Case cboCancelAction.Text
            Case "Back X", "Forward X"
                ' steps are only used with these
                txtCancelSteps.Enabled = True
            Case Else
                ' do nothing
        End Select

    End Sub

    Private Sub cboOKAction_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboOKAction.SelectedIndexChanged

        ' enable/disable OKSteps depending on value of OKAction
        Select Case cboOKAction.Text
            Case "Back X", "Forward X"
                ' steps are only used with these
                txtOKSteps.Enabled = True
            Case Else
                ' do nothing
        End Select

    End Sub

    Private Sub cmdKeySet_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdKeySet.Click
        m_lReturn = SaveKeyAttributes(oElement:=m_oElementDOM)

    End Sub

    Private Sub cmdMapSet_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdMapSet.Click
        m_lReturn = SaveMapAttributes()
    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (cmdOk_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cmdOk_Click()
    '
    'm_lStatus = gPMConstants.PMEReturnCode.PMOk
    '
    'Me.Close()
    '
    'End Sub

    'UPGRADE_NOTE: (7001) The following declaration (cmdCancel_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cmdCancel_Click()
    '
    'm_lStatus = gPMConstants.PMEReturnCode.PMCancel
    '
    'Me.Close()
    '
    'End Sub

    Private Sub cmdShowResumeView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdShowResumeView.Click
        m_lReturn = ShowResumeView()
    End Sub

    Private Sub cmdStepSet_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdStepSet.Click
        m_lReturn = SaveStepAttributes(oElement:=m_oElementDOM)
    End Sub

    Private Sub cmdSubmapSet_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSubmapSet.Click
        m_lReturn = SaveSubmapAttributes(oElement:=m_oElementDOM)
    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (mnuToolsOptions_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub mnuToolsOptions_Click()
    'm_lReturn = ShowOptions()
    'End Sub

    Public Sub mnuToolsUpdateLog_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuToolsUpdateLog.Click
        m_lReturn = ToolsUpdateLog()
    End Sub

    Private Sub tbToolbar_ButtonClick(ByVal eventSender As Object, ByVal eventArgs As ToolStripItemClickedEventArgs) Handles tbToolbar.ItemClicked
        Dim Button As ToolStripItem = CType(eventSender, ToolStripItem)

        ' select action based on selected icon
        Select Case Button.Name
            Case "Open"
                m_lReturn = OpenRoadmapByTask()
            Case "Save"
                m_lReturn = FileSave()
                '        Case "Add"
                '            m_lReturn = ToolbarAdd()
            Case "Delete"
                m_lReturn = ToolbarDelete()
            Case "MoveUp"
                m_lReturn = StepMoveUp()
            Case "MoveDown"
                m_lReturn = StepMoveDown()
        End Select

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub txtDescription_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        If txtDescription.Text.Trim() <> sDescription Then
            cmdStepSet.Enabled = True
        End If
    End Sub

    Private Sub txtDescription_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Enter

        sDescription = txtDescription.Text.Trim()
    End Sub
    Private Sub txtDescription_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtDescription.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)

        m_bElDirty = True

        cmdStepSet.Enabled = True

        If KeyAscii = CInt(Keys.Return) Then
            m_lReturn = SaveStepAttributes(oElement:=m_oElementDOM)
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtKeyName_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtKeyName.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)

        cmdKeySet.Enabled = True

        If KeyAscii = CInt(Keys.Return) Then
            m_lReturn = SaveKeyAttributes(oElement:=m_oElementDOM)
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtKeyValue_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles txtKeyValue.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim strKeyvalue As String = gPMFunctions.ToSafeString(txtKeyValue.Text.Trim())
        If strKeyvalue <> "" Then
            If eventArgs.KeyCode = Keys.Delete Then
                m_bElDirty = True
                cmdKeySet.Enabled = True
            End If
        End If

    End Sub

    Private Sub txtKeyValue_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtKeyValue.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)

        m_bElDirty = True

        cmdKeySet.Enabled = True

        If KeyAscii = CInt(Keys.Return) And txtKeyName.Text <> "TEXT" Then
            m_lReturn = SaveKeyAttributes(oElement:=m_oElementDOM)
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtResumeStep_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtResumeStep.Click
        m_lReturn = ShowResumeView()
    End Sub

    Private Sub txtResumeStep_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles txtResumeStep.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        ' stop user typing stuff in
        KeyCode = 0
    End Sub

    Private Sub txtResumeStep_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtResumeStep.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        ' stop user typing stuff in
        KeyAscii = 0
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub tvwMap_AfterSelect(ByVal eventSender As Object, ByVal eventArgs As TreeViewEventArgs) Handles tvwMap.AfterSelect
        Dim Node As TreeNode = eventArgs.Node
        m_lReturn = MapNodeClick(oNode:=Node)
    End Sub

    Private Sub tvwResumeStep_AfterSelect(ByVal eventSender As Object, ByVal eventArgs As TreeViewEventArgs) Handles tvwResumeStep.AfterSelect
        Dim Node As TreeNode = eventArgs.Node
        m_lReturn = ResumeStepNodeClick(oNode:=Node)
    End Sub

    Private Sub tvwMap_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles tvwMap.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        m_lReturn = MapMouseUp(iButton:=Button)
    End Sub

    ' ***************************************************************** '
    ' Name: ShowResumeView
    '
    ' Description: show or hide the resumestep treeview
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function ShowResumeView() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' switch treeview off/on
            ' this tv is used to select the resume step in the current step
            If Not (tvwResumeStep.Visible) Then
                tvwResumeStep.Height = VB6.TwipsToPixelsY(1215)
                m_lReturn = BuildTreeView(tvwTreeView:=tvwResumeStep)
                ' show the map
                cmdShowResumeView.Text = CHARACTER_CURSOR_UP
                tvwResumeStep.Enabled = True
                tvwResumeStep.Visible = True
            Else
                ' hide the map
                cmdShowResumeView.Text = CHARACTER_CURSOR_DOWN
                tvwResumeStep.Enabled = False
                tvwResumeStep.Visible = False
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowResumeView failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowResumeView", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ResumeStepNodeClick
    '
    ' Description: get description of selected resumestep node
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function ResumeStepNodeClick(ByVal oNode As TreeNode) As Integer

        Dim result As Integer = 0
        Dim oElement As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            txtResumeStep.Text = ""

            If (oNode.Name = m_typStep.ElementID) Or (oNode.Name = TREEVIEW_NODE_ROOT) Then
                Return result
            End If


            'TODO: Find the appropriate replacement of nodeFromID property and uncomment the below code
            'oElement = m_oXMLDOM.nodeFromID(oNode.Name)

            If oElement Is Nothing Then
                Return result
            End If

            If oElement.LocalName.ToUpper() = ELEMENT_BASENAME_SUBMAP Then
                Return result
            End If


            txtResumeStep.Text = CStr(oElement.GetAttribute("Description"))


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ResumeStepNodeClick failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ResumeStepNodeClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' confirm action
        m_lReturn = CheckMapStatus()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Cancel = 1
        End If

        eventArgs.Cancel = Cancel <> 0
    End Sub

    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If
        m_lReturn = FormResize()
    End Sub

    Private Sub txtSubmapCode_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtSubmapCode.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)

        cmdSubmapSet.Enabled = True

        If KeyAscii = CInt(Keys.Return) Then
            m_lReturn = SaveSubmapAttributes(oElement:=m_oElementDOM)
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtSubmapDescription_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtSubmapDescription.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)

        cmdSubmapSet.Enabled = True

        If KeyAscii = CInt(Keys.Return) Then
            m_lReturn = SaveSubmapAttributes(oElement:=m_oElementDOM)
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    ' ***************************************************************** '
    ' Name: FormResize
    '
    ' Description:
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function FormResize() As Integer

        Dim result As Integer = 0
        Dim lSBheight As Integer

        ' minimum form dimensions
        Const MIN_WIDTH As Integer = 7030
        Const MIN_HEIGHT As Integer = 5100

        'Const GAP_MEDIUM As Integer = 120      ''Unused Local Variable
        'Const GAP_LARGE As Integer = 240       ''Unused Local Variable

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' no need if its been minimised
            If Me.WindowState = FormWindowState.Minimized Then
                Return result
            End If

            lSBheight = CInt(VB6.PixelsToTwipsY(sbStatus.Height))

            ' minimum sizes
            If VB6.PixelsToTwipsX(Me.Width) < MIN_WIDTH Then
                Me.Width = VB6.TwipsToPixelsX(MIN_WIDTH)
            End If

            If VB6.PixelsToTwipsY(Me.Height) < MIN_HEIGHT Then
                Me.Height = VB6.TwipsToPixelsY(MIN_HEIGHT)
            End If

            ' check against position of the splitter bar
            If VB6.PixelsToTwipsX(Me.Width) < VB6.PixelsToTwipsX(lblSplitter.Left) + 480 Then
                Me.Width = lblSplitter.Left + VB6.TwipsToPixelsX(480)
            End If

            ' background pic simply provides border under toolbar
            picBG.Width = Me.ClientRectangle.Width + VB6.TwipsToPixelsX(120)
            picBG.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - lSBheight - 390)

            ' form buttons
            '    cmdCancel.Left = Me.ScaleWidth - cmdCancel.Width - 60
            '    cmdOk.Left = cmdCancel.Left - cmdOk.Width - GAP_MEDIUM

            '    cmdCancel.Top = Me.ScaleHeight - cmdCancel.Height - lSBheight - 180
            '    cmdOk.Top = Me.ScaleHeight - cmdOk.Height - lSBheight - 180

            ' treeview
            tvwMap.Height = picBG.Height - VB6.TwipsToPixelsY(270) '840

            ' main frame
            fmeAttrib.Height = picBG.Height - VB6.TwipsToPixelsY(180) '750
            fmeAttrib.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(picBG.Width) - VB6.PixelsToTwipsX(fmeAttrib.Left) - 180)

            ' splitter bar
            lblSplitter.Height = tvwMap.Height - VB6.TwipsToPixelsY(15)

            ' sub-frames
            fmeMap.Width = fmeAttrib.Width
            fmeMap.Height = fmeAttrib.Height

            fmeStep.Width = fmeAttrib.Width
            fmeStep.Height = fmeAttrib.Height

            fmeSubmap.Width = fmeAttrib.Width
            fmeSubmap.Height = fmeAttrib.Height

            fmeKey.Width = fmeAttrib.Width
            fmeKey.Height = fmeAttrib.Height


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormResize failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormResize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BuildComboLists
    '
    ' Description: OKaction and CancelAction combos
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function BuildComboLists() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' acions list for cboOKAction and cboCancelAction combos
            ReDim m_sActions(1, 8)
            m_sActions(0, 0) = gPMConstants.PMNavActionStartProcess
            m_sActions(1, 0) = "Start process"
            m_sActions(0, 1) = gPMConstants.PMNavActionRepeatMap
            m_sActions(1, 1) = "Repeat map"
            m_sActions(0, 2) = gPMConstants.PMNavActionForwardX
            m_sActions(1, 2) = "Forward X"
            m_sActions(0, 3) = gPMConstants.PMNavActionForwardOne
            m_sActions(1, 3) = "Forward one"
            m_sActions(0, 4) = gPMConstants.PMNavActionExitMap
            m_sActions(1, 4) = "Exit map"
            m_sActions(0, 5) = gPMConstants.PMNavActionCompleteProcess
            m_sActions(1, 5) = "Complete process"
            m_sActions(0, 6) = gPMConstants.PMNavActionAbortProcess
            m_sActions(1, 6) = "Abort process"
            m_sActions(0, 7) = gPMConstants.PMNavActionBackX
            m_sActions(1, 7) = "Back X"
            m_sActions(0, 8) = gPMConstants.PMNavActionBackOne
            m_sActions(1, 8) = "Back one"

            ' load the combos
            For lLoop As Integer = m_sActions.GetLowerBound(1) To m_sActions.GetUpperBound(1)
                cboOKAction.Items.Add(m_sActions(1, lLoop))
                cboCancelAction.Items.Add(m_sActions(1, lLoop))
            Next

            cboDocTempMode.Items.Clear()
            Dim cboDocTempMode_NewIndex As Integer = -1
            cboDocTempMode_NewIndex = cboDocTempMode.Items.Add("Preview Document")
            VB6.SetItemData(cboDocTempMode, cboDocTempMode_NewIndex, 1)
            cboDocTempMode_NewIndex = cboDocTempMode.Items.Add("Automatically Print & Archive Documents")
            VB6.SetItemData(cboDocTempMode, cboDocTempMode_NewIndex, 3)
            cboDocTempMode_NewIndex = cboDocTempMode.Items.Add("Automatically Spool Documents")
            VB6.SetItemData(cboDocTempMode, cboDocTempMode_NewIndex, 13)
            cboDocTempMode_NewIndex = cboDocTempMode.Items.Add("Automatically Archive Documents")
            VB6.SetItemData(cboDocTempMode, cboDocTempMode_NewIndex, 8)
            cboDocTempMode_NewIndex = cboDocTempMode.Items.Add("Automatically Print Documents")
            VB6.SetItemData(cboDocTempMode, cboDocTempMode_NewIndex, 9)
            cboDocTempMode_NewIndex = cboDocTempMode.Items.Add("Automatically Print, Spool & Archive Documents")
            VB6.SetItemData(cboDocTempMode, cboDocTempMode_NewIndex, 10)
            cboDocTempMode_NewIndex = cboDocTempMode.Items.Add("Automatically Print & Spool Documents")
            VB6.SetItemData(cboDocTempMode, cboDocTempMode_NewIndex, 11)
            cboDocTempMode_NewIndex = cboDocTempMode.Items.Add("Automatically Spool & Archive Documents")
            VB6.SetItemData(cboDocTempMode, cboDocTempMode_NewIndex, 12)
            cboDocTempMode.SelectedIndex = 0

            cboDiaryWMSteps.Items.Clear()
            cboDiaryWMSteps.Items.Add("WMNB1")
            cboDiaryWMSteps.Items.Add("WMNB2")
            cboDiaryWMSteps.Items.Add("WMNB3")
            cboDiaryWMSteps.Items.Add("WMNB4")
            cboDiaryWMSteps.Items.Add("WMMTA1")
            cboDiaryWMSteps.Items.Add("WMMTA2")
            cboDiaryWMSteps.Items.Add("WMMTA3")
            cboDiaryWMSteps.Items.Add("WMREN1")
            cboDiaryWMSteps.Items.Add("WMREN2")
            cboDiaryWMSteps.Items.Add("WMREN3")
            cboDiaryWMSteps.Items.Add("WMREN4")
            cboDiaryWMSteps.Items.Add("WMREN5")
            cboDiaryWMSteps.Items.Add("WMREN6")

            'Filling TaskGroup Combo
            m_lReturn = GetLookupDetails(sLookupTable:="PMWRK_TASK", ctlLookup:=cboDiaryTaskId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Filling TaskGroup Combo
            m_lReturn = GetLookupDetails(sLookupTable:="PMWRK_TASK_GROUP", ctlLookup:=cboDiaryTaskGroupId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Filling UserGroup Combo
            m_lReturn = GetLookupDetails(sLookupTable:="PMUSER_GROUP", ctlLookup:=cboDiaryUserGroupId)

            'Filling User Combo
            m_lReturn = GetAllEffectiveUsers()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BuildComboLists failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildComboLists", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: OpenRoadmapByProcess
    '
    ' Description: get NAVXM roadmap via PMNavXMProcess
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function OpenRoadmapByProcess() As Integer

        Dim result As Integer = 0
        Dim lStatus As gPMConstants.PMEReturnCode
        Dim lNavXMProcessID As Integer
        Dim oForm As frmProcesses

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If m_bDirty Then
                ' map has been changed
                m_lReturn = MessageBox.Show("Current map has not be saved." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Continue and lose changes?", ACApp, MessageBoxButtons.OKCancel, MessageBoxIcon.Question)

                If m_lReturn <> System.Windows.Forms.DialogResult.OK Then
                    Return result
                End If
            End If

            ' run the form to get roadmap direct from pmnavxm_process
            oForm = New frmProcesses()

            With oForm
                m_lReturn = .Initialise()

                .Business = m_oBusiness

                m_lReturn = .Load_Renamed()

                m_lReturn = .ShowForm(lDisplayState:=FormShowConstants.Modal)

                lStatus = .Status
                lNavXMProcessID = .NavXMProcessID
            End With

            oForm.Close()

            oForm = Nothing

            If lStatus <> gPMConstants.PMEReturnCode.PMOK Then
                ' failed, or nothing selected
                Return result
            End If

            ' get the roadmap
            m_lReturn = LoadXMLData(lNavXMProcessID:=lNavXMProcessID, lNavXMProcessVersionID:=m_lNavXMProcessVersionID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' more stuff if required
            StatusMsg("Roadmap loaded")


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OpenRoadmapByProcess failed", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenRoadmapByProcess", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: OpenRoadmapByTask
    '
    ' Description: get roadmap via PMWrk_Task
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function OpenRoadmapByTask() As Integer

        Dim result As Integer = 0
        Dim lStatus As gPMConstants.PMEReturnCode
        Dim lNavXMProcessID, lNavXMProcessVersionID As Integer
        Dim oForm As frmSelectRoadmap

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If m_bDirty Then
                m_lReturn = MessageBox.Show("Current map has not be saved." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Continue and lose changes?", ACApp, MessageBoxButtons.OKCancel, MessageBoxIcon.Question)

                If m_lReturn <> System.Windows.Forms.DialogResult.OK Then
                    Return result
                End If
            End If

            ' run the form to get roadmap via pmwrk_task
            oForm = New frmSelectRoadmap()

            With oForm
                m_lReturn = .Initialise()

                .Business = m_oBusiness

                m_lReturn = .Load_Renamed()

                m_lReturn = .ShowForm(lDisplayState:=FormShowConstants.Modal)

                lStatus = .Status
                lNavXMProcessID = .ProcessID
                lNavXMProcessVersionID = .ProcessVersionID
            End With

            oForm.Close()

            oForm = Nothing

            If lStatus <> gPMConstants.PMEReturnCode.PMOK Then
                ' failed, or nothing selected
                Return result
            End If

            ' get the roadmap
            m_lReturn = LoadXMLData(lNavXMProcessID:=lNavXMProcessID, lNavXMProcessVersionID:=lNavXMProcessVersionID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            StatusMsg("Roadmap loaded")


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OpenRoadmapByTask failed", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenRoadmapByTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LoadXMLData
    '
    ' Description: From NavXMProcessID, load XML from database into DOM
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function LoadXMLData(ByVal lNavXMProcessID As Integer, ByVal lNavXMProcessVersionID As Integer) As Integer

        Dim result As Integer = 0
        Dim lFH, lVersion As Integer
        Dim sFilename, sElementID, sError As String
        Dim oError As Exception

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' tells the form that the loaded roadmap has not been edited
            m_bDirty = False

            ' load xml
            If lNavXMProcessVersionID <= 0 Then


                m_lReturn = m_oBusiness.GetXMLFromPrimary(lNavXMProcessID:=lNavXMProcessID, sFilename:=sFilename, sXML:=m_sXML)
            Else


                m_lReturn = m_oBusiness.GetXMLFromSecondary(lNavXMProcessVersionID:=lNavXMProcessVersionID, lVersionNumber:=lVersion, sFilename:=sFilename, sXML:=m_sXML)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                MessageBox.Show("Failed to get roadmap from the database", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Return result

            End If

            m_sFilename = sFilename

            m_lNavXMProcessID = lNavXMProcessID
            m_lNavXMProcessVersionID = lNavXMProcessVersionID

            sFilename = m_sNavFilePath & "temp" & (CStr(DateTime.Now.TimeOfDay.TotalSeconds * 100)) & ".xml"

            lFH = FileSystem.FreeFile()

            FileSystem.FileOpen(lFH, sFilename, OpenMode.Output)
            FileSystem.PrintLine(lFH, m_sXML)
            FileSystem.FileClose(lFH)

            m_oXMLDOM = New XmlDocument()

            'TODO: Find the appropriate replacement of validateOnParse property and uncomment the below code
            ' m_oXMLDOM.validateOnParse = False
            m_oXMLDOM.PreserveWhitespace = False
            ' for IDs

            'TODO: Find the appropriate replacement of resolveExternals property and uncomment the below code
            ' m_oXMLDOM.resolveExternals = False

            ' load roadmap from temporary file
            Dim temp_xml_result As Boolean
            Try
                m_oXMLDOM.Load(sFilename)
                temp_xml_result = True

            Catch parseError As System.Exception
                temp_xml_result = False
            End Try
            m_bRet = temp_xml_result

            ' get rid of the temp file
            File.Delete(sFilename)

            If Not m_bRet Then

                ' failed to load the roadmap

                'TODO: Find the appropriate replacement of parseError property and uncomment the below code
                'oError = m_oXMLDOM.parseError

                ' details from error object

                
                sError = sError & "Line: " & CStr(CType(oError, XmlException).LineNumber) & ", "
                sError = sError & "LinePos: " & CStr(CType(oError, XmlException).LinePosition) & ", "
                sError = sError & "Reason: " & oError.Message

                oError = Nothing
                m_oXMLDOM = Nothing

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sError, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadXMLData", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                MessageBox.Show("Failed to load roadmap." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "See Sirius log for details", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Return result

            End If

            ' map attributes
            m_lReturn = GetMapDetails()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to get map details", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            End If


            sElementID = Mid(CStr(m_oXMLDOM.DocumentElement.GetAttribute("ElementID")), ELEMENT_ID_PREFIX.Length + 1)

            m_lMaxElementID = CInt(sElementID)

            ' build the main treeview
            m_lReturn = BuildTreeView(tvwMap)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_oXMLDOM = Nothing

                MessageBox.Show("Failed to build treeview from the DOM", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Return result

            End If

            fmeMap.Visible = True


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            m_oXMLDOM = Nothing

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadXMLData failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadXMLData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BuildTreeView
    '
    ' Description: build the treeview from the DOM. There are two in
    '              the interface - left-panel, and resume step
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function BuildTreeView(ByRef tvwTreeView As TreeView, Optional ByRef sKey As String = "", Optional ByRef bExpanded As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim sDesc As String = ""
        Dim oNodeTV As TreeNode
        Dim oChildNodeDOM As XmlNode

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_vDocTemplate = Nothing
            m_vDocTempList = Nothing

            tvwTreeView.Nodes.Clear()

            ' root node

            sDesc = CStr(m_oXMLDOM.DocumentElement.GetAttribute("RoadmapName"))

            oNodeTV = tvwTreeView.Nodes.Add(TREEVIEW_NODE_ROOT, sDesc, "Roadmap")

            oNodeTV.Expand()

            ' get all child objects recursively
            For Each oChildNodeDOM2 As XmlNode In m_oXMLDOM.ChildNodes
                oChildNodeDOM = oChildNodeDOM2
                m_lReturn = GetChildNodes(tvwTreeView:=tvwTreeView, oCurrNodeTV:=oNodeTV, oNodeDOM:=oChildNodeDOM, sKey:=sKey, bExpanded:=bExpanded)
            Next oChildNodeDOM2

            oNodeTV = Nothing
            oChildNodeDOM = Nothing


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BuildTreeView failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildTreeView", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetChildNodes
    '
    ' Description: called recursively to get all DOM child objects
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function GetChildNodes(ByRef tvwTreeView As TreeView, ByVal oCurrNodeTV As TreeNode, ByVal oNodeDOM As XmlNode, ByVal sKey As String, ByVal bExpanded As Boolean) As Integer

        Dim result As Integer = 0
        Dim bBold As Boolean
        Dim lElementID As Integer
        Dim sDesc, sImage, sElementID As String



        Dim oNodeTV As TreeNode

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            For Each oChildDOM As XmlNode In oNodeDOM.ChildNodes

                bBold = False

                If Not (oChildDOM.LocalName = "") Then
                    ' it's not a comment, so what type of element is it?
                    Select Case oChildDOM.LocalName.ToUpper()
                        Case ELEMENT_BASENAME_KEY
                            sImage = "Key"

                            sDesc = oChildDOM.Attributes("Name").ToString()

                        Case ELEMENT_BASENAME_SUBMAP
                            sImage = "SubMap"

                            sDesc = oChildDOM.Attributes("Description").ToString()
                            If tvwTreeView.Name = TREEVIEW_RESUMESTEP Then
                                ' add comment for resume step treeview
                                sDesc = sDesc & " (submap)"
                            End If

                        Case Else
                            sImage = "Process"

                            sDesc = oChildDOM.Attributes("Description").ToString()

                            If oChildDOM.Attributes("Core").ToString() = 0 Then
                                bBold = True
                            End If

                    End Select

                    ' set icon used in treeview

                    Select Case oChildDOM.Attributes("Type").ToString()
                        Case gPMConstants.PMNavComponentFindForm
                            sImage = ACIconFindForm
                        Case gPMConstants.PMNavComponentDataForm
                            sImage = ACIconDataForm
                        Case gPMConstants.PMNavComponentDecisionForm
                            sImage = ACIconQuestion
                        Case gPMConstants.PMNavComponentBusinessObject
                            sImage = ACIconBusiness
                        Case PMNavComponentPrintObject
                            sImage = ACIconPrint
                        Case gPMConstants.PMNavComponentDiary
                            sImage = ACIconDiary
                        Case gPMConstants.PMNavComponentEditText
                            sImage = ACIconEditText
                        Case gPMConstants.PMNavComponentRaiseEvent
                            sImage = ACIconRaiseEvent
                        Case gPMConstants.PMNavComponentStandardLetter
                            sImage = ACIconStandardLetter
                        Case gPMConstants.PMNavComponentLaunchEXE
                            sImage = ACIconLaunchEXE
                        Case gPMConstants.PMNavComponentUserComponent
                            sImage = ACIconUserComponent
                        Case Else
                            sImage = ACIconDataForm
                    End Select

                    ' override for keys and submaps
                    Select Case oChildDOM.LocalName
                        Case ELEMENT_BASENAME_SUBMAP
                            sImage = "SubMap"
                        Case ELEMENT_BASENAME_KEY
                            sImage = "Key"
                    End Select

                    ' unique key

                    sElementID = oChildDOM.Attributes("ElementID").ToString().ToUpper()
                    ' search for the current maximum element ID
                    lElementID = CInt(Mid(sElementID, ELEMENT_ID_PREFIX.Length + 1))

                    ' set counter if appropriate. This variable will be used to
                    ' create new element IDs when user adds new steps
                    If lElementID > m_lMaxElementID Then
                        m_lMaxElementID = lElementID
                    End If

                    ' tvwResumeStep is used to select resume step.
                    If tvwTreeView.Name = TREEVIEW_RESUMESTEP And oChildDOM.LocalName.ToUpper() = ELEMENT_BASENAME_KEY Then
                        ' don't show key keys on Resume Step selector
                    Else
                        oNodeTV = tvwTreeView.Nodes.Find(oCurrNodeTV.Name, True)(0).Nodes.Add(sElementID, sDesc, sImage)


                        'TODO: Find the appropriate replacement of Bold property and uncomment the below code
                        'oNodeTV.Bold = bBold

                        If sKey <> "" And sKey = oNodeTV.Name And tvwTreeView.Name = TREEVIEW_MAP Then

                            oNodeTV.Checked = True
                            oNodeTV.EnsureVisible()

                            'developer guide no.201
                            If (bExpanded) Then
                                oNodeTV.Expand()
                            Else
                                oNodeTV.Collapse()
                            End If
                            m_lReturn = MapNodeClick(oNode:=oNodeTV)
                            tvwMap.Focus()
                        End If


                    End If

                    ' does this child have any children?
                    m_lReturn = GetChildNodes(tvwTreeView:=tvwTreeView, oCurrNodeTV:=oNodeTV, oNodeDOM:=oChildDOM, sKey:=sKey, bExpanded:=bExpanded)
                End If

            Next oChildDOM


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            Select Case Information.Err().Number
                Case ERROR_LET_PROCEDURE_NOT_DEFINED
                    ' this error is expected for some elements
                    Return gPMConstants.PMEReturnCode.PMTrue
            End Select

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetChildNodes failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetChildNodes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FileSave
    '
    ' Description: save the roadmap
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function FileSave() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If Not m_bDirty Then
                MessageBox.Show("Nothing to save. " & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Map has not been edited.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)

                Return result
            End If

            m_lReturn = SaveRoadmap(bShowDiag:=True)

            If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                Return result
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to save the roadmap", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FileSave failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FileSave", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SaveRoadmap
    '
    ' Description:
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function SaveRoadmap(ByVal bShowDiag As Boolean) As Integer

        Dim result As Integer = 0
        Dim lVersion As Integer
        Dim sOldCode, sOldDesc, sCode, sDesc As String

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' check mandatory parameters have been set
            m_lReturn = ValidateRoadmap()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If m_sFilename.ToUpper() = BLANK_ROADMAP_FILENAME Then
                ' admin user selected blank.xml file, which is an empty map.
                ' This needs to be saved under a new name, then 'attached' to the required WM task
                m_lReturn = SaveNewRoadmap()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Roadmap has not been saved.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                    Return result
                End If
            End If


            lVersion = CInt(m_oXMLDOM.DocumentElement.GetAttribute("Version")) + 1

            m_oXMLDOM.DocumentElement.SetAttribute("Version", CStr(lVersion))

            If g_lUserMode = USER_MODE_ADMIN Then
                ' Sirius user
                If m_lNavXMProcessVersionID = ID_NO_VALUE Then
                    ' core map
                    m_lReturn = SaveDocument(lVersion:=lVersion, lUserType:=USER_MODE_ADMIN)
                Else
                    ' user map - 'save' or 'save as'
                    m_lReturn = GetVersionDetails(sCode:=sCode, sDesc:=sDesc)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to get version details", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                        Return result
                    End If

                    sOldCode = sCode
                    sOldDesc = sDesc

                    If bShowDiag Then
                        m_lReturn = UserInputDescription(lMode:=MSG_MODE_OLDMAP, sCode:=sCode, sDesc:=sDesc)
                    End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If

                    If sOldCode <> sCode Or sOldDesc <> sDesc Then
                        m_lReturn = SaveRoadmapVersion(bNew:=True, sCode:=sCode, sDesc:=sDesc)
                    Else
                        m_lReturn = SaveRoadmapVersion(bNew:=False, sCode:=sCode, sDesc:=sDesc)
                    End If

                End If
            Else
                ' customer
                If m_lNavXMProcessVersionID = ID_NO_VALUE Then
                    ' core map, so save as a copy

                    If bShowDiag Then
                        m_lReturn = UserInputDescription(lMode:=MSG_MODE_NEWMAP, sCode:=sCode, sDesc:=sDesc)
                    End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMOK Then
                        MessageBox.Show("Failed to get description for roadmap copy", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                        Return result
                    End If

                    m_lReturn = SaveRoadmapVersion(bNew:=True, sCode:=sCode, sDesc:=sDesc)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to save new roadmap", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                        Return result
                    End If
                Else
                    ' user map, so 'save', or 'save as'
                    m_lReturn = GetVersionDetails(sCode:=sCode, sDesc:=sDesc)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to get version details", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                        Return result
                    End If

                    sOldCode = sCode
                    sOldDesc = sDesc

                    If bShowDiag Then
                        m_lReturn = UserInputDescription(lMode:=MSG_MODE_OLDMAP, sCode:=sCode, sDesc:=sDesc)
                    End If


                    If m_lReturn <> gPMConstants.PMEReturnCode.PMOK Then
                        Return m_lReturn
                    End If

                    If sOldCode <> sCode Or sOldDesc <> sDesc Then
                        m_lReturn = SaveRoadmapVersion(bNew:=True, sCode:=sCode, sDesc:=sDesc)
                    Else
                        m_lReturn = SaveRoadmapVersion(bNew:=False, sCode:=sCode, sDesc:=sDesc)
                    End If
                End If

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_bDirty = False

            m_lReturn = GetMapDetails()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                tvwMap.Nodes.Clear()

                MessageBox.Show("Failed to get the map attributes", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Return result
            End If

            m_lReturn = BuildTreeView(tvwTreeView:=tvwMap)

            StatusMsg("Roadmap saved")


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveRoadmap failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveRoadmap", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SaveNewRoadmap
    '
    ' Description: save roadmap created from blank.xml
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function SaveNewRoadmap() As Integer

        Dim result As Integer = 0
        Dim lNavXMProcessID As Integer
        Dim sFilename As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            'TODO: Find the appropriate replacement of CancelError property and uncomment the below code
            'CommonDialog1Open.CancelError = True
            CommonDialog1Open.FileName = m_sNavFilePath & "newroadmap.xml"
            CommonDialog1Save.FileName = m_sNavFilePath & "newroadmap.xml"

            CommonDialog1Open.Filter = "Navigator XM files (*" & FILENAME_SUFFIX_STANDARD & ")|*" & FILENAME_SUFFIX_STANDARD & "|All files (*.*)|*.*"
            CommonDialog1Save.Filter = "Navigator XM files (*" & FILENAME_SUFFIX_STANDARD & ")|*" & FILENAME_SUFFIX_STANDARD & "|All files (*.*)|*.*"

            m_bRet = True

            Do While m_bRet

                CommonDialog1Save.ShowDialog()
                CommonDialog1Open.FileName = CommonDialog1Save.FileName

                sFilename = CommonDialog1Open.FileName

                m_lReturn = CheckFileExists(sFilename, m_bRet)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to check if file already exists", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                    Return result
                End If

                If m_bRet Then
                    MessageBox.Show("Not permitted: roadmap filename already exists", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

            Loop

            m_sFilename = sFilename


            m_lReturn = m_oBusiness.CreateNewRoadmap(lNavXMProcessID:=lNavXMProcessID, sFilename:=sFilename)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lNavXMProcessID = lNavXMProcessID


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            If Information.Err().Number = ERROR_CANCEL_SELECTED Then
                ' user cancelled the export, so don't display error message

                Return gPMConstants.PMEReturnCode.PMError
            End If

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveNewRoadmap failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveNewRoadmap", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AdminSaveDocument
    '
    ' Description: sirius save
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (AdminSaveDocument) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function AdminSaveDocument(ByVal lVersion As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'm_lReturn = SaveDocument(lVersion:=lVersion, lUserType:=ROADMAP_CORE_ADMIN)
    '
    '
    'Return m_lReturn
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AdminSaveDocument failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AdminSaveDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: UserSaveDocument
    '
    ' Description: user save
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (UserSaveDocument) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function UserSaveDocument(ByVal lVersion As Integer, ByVal sDesc As String) As Integer
    '
    'Dim result As Integer = 0
    'Dim sCoreFilename As String = ""
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' instead of saving the file to disk, save it to a new row in the process_version table
    '
    'm_lReturn = m_oBusiness.SaveRoadmapVersion(lPMNavXMProcessID:=m_lNavXMProcessID, lVersion:=lVersion, sDesc:=sDesc, sXML:=m_oXMLDOM.InnerXml, lNavXMProcessVersionID:=m_lNavXMProcessVersionID)
    '
    '
    'Return m_lReturn
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UserSaveDocument failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UserSaveDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: SaveDocument
    '
    ' Description: write the xml file to HD and database
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function SaveDocument(ByVal lVersion As Integer, ByVal lUserType As Integer) As Integer

        Dim result As Integer = 0
        Dim sCoreFilename As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' save to disk
            m_lReturn = WriteXMLFile()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to write roadmap to file", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Return result
            End If

            ' save to database
            m_lReturn = SaveXMLData(lVersion:=lVersion, lCore:=lUserType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to save roadmap to database", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveDocument failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SaveXMLData
    '
    ' Description: write XML to database
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function SaveXMLData(ByVal lVersion As Integer, ByVal lCore As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            m_lReturn = m_oBusiness.SaveXMLData(lNavXMProcessID:=m_lNavXMProcessID, lVersion:=lVersion, lCore:=lCore, sXML:=m_oXMLDOM.InnerXml)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to save roadmap to database", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveXMLData failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveXMLData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: WriteXMLFile
    '
    ' Description: write XML to HD
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function WriteXMLFile() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_oXMLDOM.Save(m_sNavFilePath & m_sFilename)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="WriteXMLFile failed", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteXMLFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteElement
    '
    ' Description: remove element and rebuild the treeview
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function DeleteElement(ByVal sType As String, ByVal sName As String) As Integer

        Dim result As Integer = 0
        Dim oParent, oOldElement As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            m_lReturn = MessageBox.Show("Ok to delete " & sType & " '" & CStr(m_oElementDOM.GetAttribute(sName)) & "'?", ACApp, MessageBoxButtons.OKCancel, MessageBoxIcon.Question)

            If m_lReturn <> System.Windows.Forms.DialogResult.OK Then
                Return result
            End If

            oParent = m_oElementDOM.ParentNode

            ' Get the previous sibling
            oOldElement = m_oElementDOM.PreviousSibling

            If Not (oOldElement Is Nothing) Then

                If CStr(m_oElementDOM.GetAttribute("OKAction")) = gPMConstants.PMNavActionCompleteProcess Then
                    oOldElement.SetAttribute("OKAction", gPMConstants.PMNavActionCompleteProcess)
                End If
            End If


            m_oElementDOM = oParent.RemoveChild(m_oElementDOM)

            m_oElementDOM = Nothing

            m_bDirty = True

            m_lReturn = BuildTreeView(tvwTreeView:=tvwMap)

            StatusMsg(sType & " deleted")


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteElement failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteElement", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: MapAddStep
    '
    ' Description: add a blank roadmap step
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function MapAddStep() As Integer

        Dim result As Integer = 0
        Dim oNewElement As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            oNewElement = m_oXMLDOM.CreateElement(ELEMENT_BASENAME_STEP)

            ' add its attributes
            oNewElement.SetAttribute("Description", "NEW STEP")
            oNewElement.SetAttribute("Component", "")
            oNewElement.SetAttribute("Type", "")
            oNewElement.SetAttribute("CancelAction", gPMConstants.PMNavActionAbortProcess)
            oNewElement.SetAttribute("OKAction", gPMConstants.PMNavActionForwardOne)
            oNewElement.SetAttribute("OKSteps", "0")
            oNewElement.SetAttribute("CancelSteps", "0")
            oNewElement.SetAttribute("ComponentAction", "0")
            oNewElement.SetAttribute("ServerSide", "False")
            oNewElement.SetAttribute("CreateWMTask", "False")
            oNewElement.SetAttribute("ResumeStep", "")
            oNewElement.SetAttribute("Core", "1")
            oNewElement.SetAttribute("Submap", "")

            ' get unique element ID
            m_lReturn = SetElementID(oElement:=oNewElement)

            oNewElement.SetAttribute("OKNewRoadmap", "")
            oNewElement.SetAttribute("CancelNewRoadmap", "")


            oNewElement = m_oXMLDOM.DocumentElement.AppendChild(oNewElement)

            m_bDirty = True

            m_lReturn = BuildTreeView(tvwTreeView:=tvwMap)

            StatusMsg("Step added")


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MapAddStep failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MapAddStep", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: StepAddBlankStep
    '
    ' Description:
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function StepAddBlankStep() As Integer

        Dim result As Integer = 0
        Dim oElement, oNewElement, oParent As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' create the new element
            oNewElement = m_oXMLDOM.CreateElement(ELEMENT_BASENAME_STEP)

            ' add its attributes
            oNewElement.SetAttribute("Description", "NEW STEP")
            oNewElement.SetAttribute("Component", "")
            oNewElement.SetAttribute("Type", "")
            oNewElement.SetAttribute("CancelAction", gPMConstants.PMNavActionAbortProcess)
            oNewElement.SetAttribute("OKAction", gPMConstants.PMNavActionForwardOne)
            oNewElement.SetAttribute("OKSteps", "0")
            oNewElement.SetAttribute("CancelSteps", "0")
            oNewElement.SetAttribute("ComponentAction", "0")
            oNewElement.SetAttribute("ServerSide", "False")
            oNewElement.SetAttribute("CreateWMTask", "False")
            oNewElement.SetAttribute("ResumeStep", "")
            oNewElement.SetAttribute("Core", "1")
            oNewElement.SetAttribute("Submap", "")

            ' get unique element ID
            m_lReturn = SetElementID(oElement:=oNewElement)

            oNewElement.SetAttribute("OKNewRoadmap", "")
            oNewElement.SetAttribute("CancelNewRoadmap", "")

            ' select correct position to add the element
            oElement = m_oElementDOM.NextSibling
            oParent = m_oElementDOM.ParentNode

            If oElement Is Nothing Then
                ' add new element to end of parents' children

                oElement = oParent.AppendChild(oNewElement)
            Else
                ' current element was not last, so add after current element

                oElement = oParent.InsertBefore(oNewElement, oElement)
            End If

            m_bDirty = True

            m_lReturn = BuildTreeView(tvwTreeView:=tvwMap)

            StatusMsg("Step added")


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StepAddBlankStep failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StepAddBlankStep", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: StepAddKey
    '
    ' Description: add new key
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function StepAddKey() As Integer

        Dim result As Integer = 0
        Dim oNewElement As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' create the new element
            oNewElement = m_oXMLDOM.CreateElement(ELEMENT_BASENAME_KEY)

            ' add its attributes
            oNewElement.SetAttribute("Name", "NEW KEY")
            oNewElement.SetAttribute("Value", "")

            ' get unique element ID
            m_lReturn = SetElementID(oElement:=oNewElement)

            ' add new element to end of parents' children

            oNewElement = m_oElementDOM.AppendChild(oNewElement)

            m_bDirty = True

            m_lReturn = BuildTreeView(tvwTreeView:=tvwMap)

            StatusMsg("Key added")


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StepAddKey failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StepAddKey", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: StepAddStep
    '
    ' Description: add secondary step
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function StepAddStep() As Integer

        Dim result As Integer = 0
        Dim lStatus As gPMConstants.PMEReturnCode
        Dim lAvailableStepID As Integer
        Dim oForm As frmAvailableSteps

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' get required step
            oForm = New frmAvailableSteps()

            With oForm
                m_lReturn = .Initialise()

                .Business = m_oBusiness

                m_lReturn = .Load_Renamed()

                m_lReturn = .ShowForm(lDisplayState:=FormShowConstants.Modal)

                lStatus = .Status
                lAvailableStepID = .AvailableStepID
            End With

            oForm.Close()

            oForm = Nothing

            If lStatus <> gPMConstants.PMEReturnCode.PMOK Then
                Return result
            End If

            m_lReturn = AddAvailableStep(lAvailableStepID:=lAvailableStepID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_bDirty = True

            ' more stuff here, if required
            StatusMsg("Step added")


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StepAddStep failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StepAddStep", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: StepAddSubmap
    '
    ' Description: add submap
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function StepAddSubmap() As Integer

        Dim result As Integer = 0
        Dim oNewElement As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' create the new element
            oNewElement = m_oXMLDOM.CreateElement(ELEMENT_BASENAME_SUBMAP)

            ' add its attributes
            oNewElement.SetAttribute("Code", "NEW SUBMAP")
            oNewElement.SetAttribute("Description", "NEW SUBMAP")

            ' get unique element ID
            m_lReturn = SetElementID(oElement:=oNewElement)

            ' add new element to end of parents' children

            oNewElement = m_oElementDOM.AppendChild(oNewElement)

            m_bDirty = True

            m_lReturn = BuildTreeView(tvwTreeView:=tvwMap)

            StatusMsg("Submap added")


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StepAddSubmap failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StepAddSubmap", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: StepMoveDown
    '
    ' Description: move step down one place
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function StepMoveDown() As Integer

        Dim result As Integer = 0
        Dim sKey, sCore, sName As String
        Dim oNewElement, oElement, oParent As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If m_oElementDOM Is Nothing Then
                Return result
            End If

            If tvwMap.SelectedNode Is Nothing Then
                ' nothing selected on the tree view
                Return result
            End If

            m_bExpanded = tvwMap.SelectedNode.IsExpanded

            sName = m_oElementDOM.LocalName

            If sName <> ELEMENT_BASENAME_STEP Then
                Return result
            End If


            sKey = CStr(m_oElementDOM.GetAttribute("ElementID"))


            sCore = CStr(m_oElementDOM.GetAttribute("Core"))

            If sCore = "1" Then
                Return result
            End If

            oElement = m_oElementDOM.NextSibling

            If oElement Is Nothing Then
                ' nowt, 'cos it's already the last child of the parent
            Else
                oNewElement = oElement.CloneNode(True)
                oParent = m_oElementDOM.ParentNode

                oElement = oParent.RemoveChild(oElement)

                oNewElement = oParent.InsertBefore(oNewElement, m_oElementDOM)
            End If

            m_bDirty = True

            m_lReturn = BuildTreeView(tvwTreeView:=tvwMap, sKey:=sKey, bExpanded:=m_bExpanded)

            StatusMsg("Step moved down")


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StepMoveDown failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StepMoveDown", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: StepMoveUp
    '
    ' Description: move step up one place
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function StepMoveUp() As Integer

        Dim result As Integer = 0
        Dim sKey, sCore, sName As String
        Dim oNewElement As XmlElement
        Dim oElement As XmlNode
        Dim oParent As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If m_oElementDOM Is Nothing Then
                ' nothing selected
                Return result
            End If

            If tvwMap.SelectedNode Is Nothing Then
                ' nothing selected on the tree view
                Return result
            End If

            m_bExpanded = tvwMap.SelectedNode.IsExpanded

            sName = m_oElementDOM.LocalName

            If sName <> ELEMENT_BASENAME_STEP Then
                ' not a step element
                Return result
            End If


            sKey = CStr(m_oElementDOM.GetAttribute("ElementID"))


            sCore = CStr(m_oElementDOM.GetAttribute("Core"))

            If g_lUserMode = USER_MODE_USER And sCore = "1" Then
                ' users can't move
                Return result
            End If

            oElement = m_oElementDOM.PreviousSibling

            If oElement Is Nothing Then
                ' it's already the first child of the parent
            Else
                If oElement.LocalName = "" Then
                    ' it's a comment, do nothing
                    Return result
                End If

                oNewElement = m_oElementDOM.CloneNode(True)
                oParent = m_oElementDOM.ParentNode

                m_oElementDOM = oParent.RemoveChild(m_oElementDOM)

                oNewElement = oParent.InsertBefore(oNewElement, oElement)
            End If

            m_bDirty = True

            m_lReturn = BuildTreeView(tvwTreeView:=tvwMap, sKey:=sKey, bExpanded:=m_bExpanded)

            StatusMsg("Step moved up")


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StepMoveUp failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StepMoveUp", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SubmapAddBlankStep
    '
    ' Description: add blank step to submap
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function SubmapAddBlankStep() As Integer

        Dim result As Integer = 0
        Dim oNewElement As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' create the new element
            oNewElement = m_oXMLDOM.CreateElement(ELEMENT_BASENAME_STEP)

            ' add its attributes
            oNewElement.SetAttribute("Description", "NEW STEP")
            oNewElement.SetAttribute("Component", "")
            oNewElement.SetAttribute("Type", "")
            oNewElement.SetAttribute("CancelAction", gPMConstants.PMNavActionAbortProcess)
            oNewElement.SetAttribute("OKAction", gPMConstants.PMNavActionForwardOne)
            oNewElement.SetAttribute("OKSteps", "0")
            oNewElement.SetAttribute("CancelSteps", "0")
            oNewElement.SetAttribute("ComponentAction", "0")
            oNewElement.SetAttribute("ServerSide", "False")
            oNewElement.SetAttribute("CreateWMTask", "False")
            oNewElement.SetAttribute("ResumeStep", "")
            oNewElement.SetAttribute("Core", "1")
            oNewElement.SetAttribute("Submap", "")

            ' get unique element ID
            m_lReturn = SetElementID(oElement:=oNewElement)

            oNewElement.SetAttribute("OKNewRoadmap", "")
            oNewElement.SetAttribute("CancelNewRoadmap", "")


            oNewElement = m_oElementDOM.AppendChild(oNewElement)

            m_bDirty = True

            m_lReturn = BuildTreeView(tvwTreeView:=tvwMap)

            StatusMsg("Step added")


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SubmapAddBlankStep failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SubmapAddBlankStep", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: MapMouseUp
    '
    ' Description: show map right-click popup menu
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function MapMouseUp(ByVal iButton As Integer) As Integer

        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If m_lMaxElementID = 0 Then
                Return result
            End If

            If iButton And MouseButtonConstants.RightButton Then
                ' right-click menu
                If m_oElementDOM Is Nothing Then
                    'it's the map
                    If g_lUserMode = USER_MODE_ADMIN Then
                        Ctx_mnuMap.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
                    End If
                Else
                    Select Case m_oElementDOM.LocalName.ToUpper()
                        Case ELEMENT_BASENAME_STEP
                            m_lReturn = ShowStepMenu()
                        Case ELEMENT_BASENAME_SUBMAP
                            If g_lUserMode = USER_MODE_ADMIN Then
                                Ctx_mnuSubmap.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
                            End If
                        Case ELEMENT_BASENAME_KEY
                            ' no menu for keys
                            If g_lUserMode = USER_MODE_ADMIN Then
                                Ctx_mnuKey.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
                            End If
                    End Select
                End If

            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MapMouseUp failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MapMouseUp", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ShowStepMenu
    '
    ' Description: show step right-click popup menu
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function ShowStepMenu() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If g_lUserMode = USER_MODE_USER Then

                If CStr(m_oElementDOM.GetAttribute("Core")) = CStr(ROADMAP_CORE_ADMIN) Then
                    mnuStepAddStep.Available = True
                    mnuStepAddBlankStep.Available = False
                    mnuStepAddSubmap.Available = False
                    mnuStepAddKey.Available = False
                    mnuStepMoveUp.Available = False
                    mnuStepMoveDown.Available = False
                    mnuStepDeleteStep.Available = False
                Else
                    mnuStepAddStep.Available = True
                    mnuStepAddBlankStep.Available = False
                    mnuStepAddSubmap.Available = False
                    mnuStepAddKey.Available = False
                    mnuStepMoveUp.Available = True
                    mnuStepMoveDown.Available = True
                    mnuStepDeleteStep.Available = True
                    mnuStepAddStep.Available = True
                End If
            Else
                mnuStepAddStep.Available = True
                mnuStepAddBlankStep.Available = True
                mnuStepAddSubmap.Available = True
                mnuStepAddKey.Available = True
                mnuStepMoveUp.Available = True
                mnuStepMoveDown.Available = True
                mnuStepDeleteStep.Available = True
                mnuStepAddStep.Available = True
            End If

            Ctx_mnuStep.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowStepMenu failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowStepMenu", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: MapNodeClick
    '
    ' Description: ensure screen properties updated for selected node
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function MapNodeClick(ByVal oNode As TreeNode) As Integer

        Dim result As Integer = 0
        Dim typMap As MainModule.Map = MainModule.Map.CreateInstance()
        Dim typStep As MainModule.Step_Renamed = MainModule.Step_Renamed.CreateInstance()
        Dim typSubmap As MainModule.Submap = MainModule.Submap.CreateInstance()
        Dim typKey As MainModule.Key = MainModule.Key.CreateInstance()
        Dim oParent, oLastElement As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            oLastElement = m_oElementDOM

            'TODO: Find the appropriate replacement of nodeFromID property and uncomment the below code
            'm_oElementDOM = m_oXMLDOM.nodeFromID(oNode.Name)

            If m_sLastNodeKey <> "" And m_sLastNodeKey <> oNode.Name Then
                ' switched nodes
                If m_bElDirty Then
                    m_bElDirty = False
                    m_lReturn = MessageBox.Show("Element details have changed. Save?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                    If m_lReturn = System.Windows.Forms.DialogResult.Yes Then
                        m_lReturn = SaveElementAttributes(oElement:=oLastElement)
                    End If
                End If
            End If

            fmeStep.Visible = False
            fmeSubmap.Visible = False
            fmeKey.Visible = False

            If m_oElementDOM Is Nothing Then
                m_lReturn = GetMapDetails()
                Return result
            End If

            fmeMap.Visible = False

            Select Case m_oElementDOM.LocalName
                Case ELEMENT_BASENAME_STEP
                    m_lReturn = GetStepDetails()
                    m_lReturn = CheckMode(fmeFrame:=fmeStep, oElement:=m_oElementDOM)
                    fmeStep.Visible = True
                Case ELEMENT_BASENAME_SUBMAP
                    m_lReturn = GetSubmapDetails()
                    m_lReturn = CheckMode(fmeFrame:=fmeSubmap, oElement:=m_oElementDOM)
                    fmeSubmap.Visible = True
                Case ELEMENT_BASENAME_KEY
                    m_lReturn = GetKeyDetails()
                    oParent = m_oElementDOM.ParentNode
                    m_lReturn = CheckMode(fmeFrame:=fmeKey, oElement:=oParent)
                    fmeKey.Visible = True
                Case Else
                    ' didn't work
                    m_oElementDOM = Nothing
                    MessageBox.Show("Failed to get element details", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                    Return result
            End Select

            m_sLastNodeKey = oNode.Name


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MapNodeClick failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MapNodeClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckMode
    '
    ' Description: check if sirius or user mode and enable/disable
    '              frame appropriately
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function CheckMode(ByRef fmeFrame As GroupBox, ByVal oElement As XmlElement) As Integer

        Dim result As Integer = 0
        Dim vCore As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            vCore = CInt(oElement.GetAttribute("Core"))

            If g_lUserMode = USER_MODE_ADMIN Then
                fmeFrame.Enabled = True

            Else
                fmeFrame.Enabled = vCore = ROADMAP_CORE_USER
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMode failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetMapDetails
    '
    ' Description: display map properties
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function GetMapDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' map attributes

            m_typMap.AutoClose = CStr(m_oXMLDOM.DocumentElement.GetAttribute("AutoClose"))

            m_typMap.ImageURL = CStr(m_oXMLDOM.DocumentElement.GetAttribute("ImageURL"))

            m_typMap.NavigatorDriven = CStr(m_oXMLDOM.DocumentElement.GetAttribute("NavigatorDriven"))

            m_typMap.ProcessMode = CStr(m_oXMLDOM.DocumentElement.GetAttribute("ProcessMode"))

            m_typMap.RoadmapName = CStr(m_oXMLDOM.DocumentElement.GetAttribute("RoadmapName"))

            m_typMap.Title = CStr(m_oXMLDOM.DocumentElement.GetAttribute("Title"))

            m_typMap.TransactionType = CStr(m_oXMLDOM.DocumentElement.GetAttribute("TransactionType"))

            m_typMap.WMTaskCode = CStr(m_oXMLDOM.DocumentElement.GetAttribute("WMTaskCode"))

            m_typMap.WMTaskDescription = CStr(m_oXMLDOM.DocumentElement.GetAttribute("WMTaskDescription"))

            m_typMap.Core = CStr(m_oXMLDOM.DocumentElement.GetAttribute("Core"))

            m_typMap.Version = CStr(m_oXMLDOM.DocumentElement.GetAttribute("Version"))

            m_typMap.ElementID = CStr(m_oXMLDOM.DocumentElement.GetAttribute("ElementID"))

            ' to form
            txtMapAutoClose.Text = m_typMap.AutoClose
            txtMapImageURL.Text = m_typMap.ImageURL
            txtMapNavigatorDriven.Text = m_typMap.NavigatorDriven
            txtMapProcessMode.Text = m_typMap.ProcessMode
            txtMapRoadmapName.Text = m_typMap.RoadmapName
            txtMapTitle.Text = m_typMap.Title
            txtMapTransactionType.Text = m_typMap.TransactionType
            txtMapWMTaskCode.Text = m_typMap.WMTaskCode
            txtMapWMTaskDescription.Text = m_typMap.WMTaskDescription
            txtMapVersion.Text = m_typMap.Version
            txtMapElementID.Text = m_typMap.ElementID

            If m_typMap.Core = CStr(ROADMAP_CORE_ADMIN) Then
                chkMapCore.CheckState = CheckState.Checked
            Else
                chkMapCore.CheckState = CheckState.Unchecked
            End If

            cmdMapSet.Enabled = False

            ' which parms can be edited?
            If g_lUserMode = USER_MODE_USER Then
                ' objects to lock down
                fmeMap.Enabled = False

                m_lReturn = SetMapPropertiesUser()

            Else
                ' objects to unlock
                fmeMap.Enabled = True

                m_lReturn = SetMapPropertiesAdmin()

            End If

            fmeMap.Visible = True


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMapDetails failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMapDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetMapPropertiesUser
    '
    ' Description: map display, user mode
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function SetMapPropertiesUser() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            lblMapProcessMode.Visible = False
            lblMapNavigatorDriven.Visible = False
            lblMapVersion.Visible = False
            lblMapImageURL.Visible = False
            lblMapElementID.Visible = False
            lblMapWMTaskCode.Visible = False
            lblMapWMTaskDescription.Visible = False
            lblMapTransactionType.Visible = False
            lblMapCore.Visible = False
            lblMapAutoClose.Visible = False

            txtMapProcessMode.Visible = False
            txtMapNavigatorDriven.Visible = False
            txtMapVersion.Visible = False
            txtMapImageURL.Visible = False
            txtMapElementID.Visible = False
            txtMapWMTaskCode.Visible = False
            txtMapWMTaskDescription.Visible = False
            txtMapTransactionType.Visible = False
            chkMapCore.Visible = False
            txtMapAutoClose.Visible = False

            lblMapRoadmapName.Enabled = False
            lblMapTitle.Enabled = False
            txtMapRoadmapName.Enabled = False
            txtMapTitle.Enabled = False
            cmdMapSet.Enabled = False

            cmdMapSet.Top = VB6.TwipsToPixelsY(1680)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: SetMapPropertiesAdmin
    '
    ' Description: map display, sirius mode
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function SetMapPropertiesAdmin() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            lblMapProcessMode.Visible = True
            lblMapNavigatorDriven.Visible = True
            lblMapVersion.Visible = True
            lblMapImageURL.Visible = True
            lblMapElementID.Visible = True
            lblMapWMTaskCode.Visible = True
            lblMapWMTaskDescription.Visible = True
            lblMapTransactionType.Visible = True
            lblMapCore.Visible = True
            lblMapAutoClose.Visible = True

            txtMapProcessMode.Visible = True
            txtMapNavigatorDriven.Visible = True
            txtMapVersion.Visible = True
            txtMapImageURL.Visible = True
            txtMapElementID.Visible = True
            txtMapWMTaskCode.Visible = True
            txtMapWMTaskDescription.Visible = True
            txtMapTransactionType.Visible = True
            chkMapCore.Visible = True
            txtMapAutoClose.Visible = True

            lblMapRoadmapName.Enabled = True
            lblMapTitle.Enabled = True
            txtMapRoadmapName.Enabled = True
            txtMapTitle.Enabled = True
            cmdMapSet.Enabled = True

            cmdMapSet.Top = VB6.TwipsToPixelsY(3420)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: GetStepDetails
    '
    ' Description: display step properties
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function GetStepDetails() As Integer

        Dim result As Integer = 0
        Dim sAction As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' step attributes

            m_typStep.CancelAction = CStr(m_oElementDOM.GetAttribute("CancelAction"))

            m_typStep.CancelSteps = CStr(m_oElementDOM.GetAttribute("CancelSteps"))

            m_typStep.Component = CStr(m_oElementDOM.GetAttribute("Component"))

            m_typStep.ComponentAction = CStr(m_oElementDOM.GetAttribute("ComponentAction"))

            m_typStep.CreateWMTask = CStr(m_oElementDOM.GetAttribute("CreateWMTask"))

            m_typStep.Description = CStr(m_oElementDOM.GetAttribute("Description"))

            m_typStep.OKAction = CStr(m_oElementDOM.GetAttribute("OKAction"))

            m_typStep.OKSteps = CStr(m_oElementDOM.GetAttribute("OKSteps"))

            m_typStep.ElementID = CStr(m_oElementDOM.GetAttribute("ElementID"))

            m_typStep.ResumeStep = CStr(m_oElementDOM.GetAttribute("ResumeStep"))

            m_typStep.ServerSide = CStr(m_oElementDOM.GetAttribute("ServerSide"))

            m_typStep.Submap = CStr(m_oElementDOM.GetAttribute("Submap"))

            m_typStep.Type = CStr(m_oElementDOM.GetAttribute("Type"))

            m_typStep.Core = CStr(m_oElementDOM.GetAttribute("Core"))

            m_typStep.OKNewRoadmap = CStr(m_oElementDOM.GetAttribute("OKNewRoadmap"))

            m_typStep.CancelNewRoadmap = CStr(m_oElementDOM.GetAttribute("CancelNewRoadmap"))

            ' to form
            sAction = m_typStep.CancelAction
            m_lReturn = SetComboAction(cboCancelAction, sAction)

            txtCancelSteps.Text = m_typStep.CancelSteps
            txtComponent.Text = m_typStep.Component
            txtComponentAction.Text = m_typStep.ComponentAction

            m_lReturn = SetCheckBox(chkCreateWMTask, m_typStep.CreateWMTask)

            txtDescription.Text = m_typStep.Description

            sAction = m_typStep.OKAction
            m_lReturn = SetComboAction(cboOKAction, sAction)

            txtOKSteps.Text = m_typStep.OKSteps
            txtStepElementID.Text = m_typStep.ElementID
            txtResumeStep.Text = m_typStep.ResumeStep
            txtOKNewRoadmap.Text = m_typStep.OKNewRoadmap
            txtCancelNewRoadmap.Text = m_typStep.CancelNewRoadmap

            m_lReturn = SetCheckBox(chkServerSide, m_typStep.ServerSide)

            txtSubmap.Text = m_typStep.Submap
            txtStepType.Text = m_typStep.Type

            If m_typStep.Core = CStr(ROADMAP_CORE_ADMIN) Then
                chkStepCore.CheckState = CheckState.Checked
            Else
                chkStepCore.CheckState = CheckState.Unchecked
            End If

            cmdShowResumeView.Text = CHARACTER_CURSOR_DOWN
            tvwResumeStep.Enabled = False
            tvwResumeStep.Visible = False

            cmdStepSet.Enabled = False

            ' which parms can be edited?
            If g_lUserMode = USER_MODE_USER Then
                ' objects to lock down
                If m_typStep.Core = CStr(ROADMAP_CORE_ADMIN) Then
                    ' lock everything down
                    fmeStep.Enabled = False
                Else
                    ' lock only core items dowm
                    fmeStep.Enabled = True

                End If

                m_lReturn = SetStepPropertiesUser(m_typStep.Core)

            Else
                ' objects to unlock
                fmeStep.Enabled = True

                m_lReturn = SetStepPropertiesAdmin()

            End If

            If m_typStep.Description = "SCREENTEXT" Then
                chkCreateWMTask.CheckState = CheckState.Unchecked
                chkCreateWMTask.Visible = False
                lblCreateWMTask.Visible = False
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStepDetails failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStepDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetStepPropertiesUser
    '
    ' Description: step display, user mode
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function SetStepPropertiesUser(ByVal sCore As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            cboCancelAction.Enabled = False
            txtCancelSteps.Enabled = False
            txtComponent.Enabled = False
            txtComponentAction.Enabled = False

            If sCore = "1" Then
                chkCreateWMTask.Enabled = False
                txtDescription.Enabled = False
            Else
                chkCreateWMTask.Enabled = True
                txtDescription.Enabled = True
            End If

            cboOKAction.Enabled = False
            txtOKSteps.Enabled = False
            txtStepElementID.Enabled = False
            txtOKNewRoadmap.Enabled = False
            txtCancelNewRoadmap.Enabled = False
            txtResumeStep.Enabled = False
            chkServerSide.Enabled = False
            txtSubmap.Enabled = False
            txtStepType.Enabled = False
            chkStepCore.Enabled = False

            lblStepCore.Visible = False
            lblStepElementID.Visible = False
            lblComponent.Visible = False
            lblComponentAction.Visible = False
            lblOKAction.Visible = False
            lblOkSteps.Visible = False
            lblServerSide.Visible = False
            lblCancelAction.Visible = False
            lblCancelSteps.Visible = False
            lblStepType.Visible = False
            lblSubmap.Visible = False
            lblResumeStep.Visible = False
            lblStepOKNewRoadmap.Visible = False
            lblStepCancelNewRoadmap.Visible = False
            lblCreateWMTask.Visible = True
            cmdShowResumeView.Visible = False

            cboCancelAction.Visible = False
            txtCancelSteps.Visible = False
            txtComponent.Visible = False
            txtComponentAction.Visible = False

            chkCreateWMTask.Visible = True
            txtDescription.Visible = True

            If sCore = "1" Then
                lblDescription.Enabled = False
                lblCreateWMTask.Enabled = False
            Else
                lblDescription.Enabled = True
                lblCreateWMTask.Enabled = True
            End If

            cboOKAction.Visible = False
            txtOKSteps.Visible = False
            txtStepElementID.Visible = False
            txtOKNewRoadmap.Visible = False
            txtCancelNewRoadmap.Visible = False
            txtResumeStep.Visible = False
            chkServerSide.Visible = False
            txtSubmap.Visible = False
            txtStepType.Visible = False
            chkStepCore.Visible = False

            cmdStepSet.Top = VB6.TwipsToPixelsY(1680) '1080


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: SetStepPropertiesAdmin
    '
    ' Description: step display, sirius mode
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function SetStepPropertiesAdmin() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            cboCancelAction.Enabled = True
            txtCancelSteps.Enabled = True
            txtComponent.Enabled = True
            txtComponentAction.Enabled = True
            chkCreateWMTask.Enabled = True
            txtDescription.Enabled = True
            cboOKAction.Enabled = True
            txtOKSteps.Enabled = True
            txtStepElementID.Enabled = True
            txtOKNewRoadmap.Enabled = True
            txtCancelNewRoadmap.Enabled = True
            txtResumeStep.Enabled = True
            chkServerSide.Enabled = True
            txtSubmap.Enabled = True
            txtStepType.Enabled = True
            chkStepCore.Enabled = True

            lblStepCore.Visible = True
            lblStepElementID.Visible = True
            lblComponent.Visible = True
            lblComponentAction.Visible = True
            lblOKAction.Visible = True
            lblOkSteps.Visible = True
            lblServerSide.Visible = True
            lblCancelAction.Visible = True
            lblCancelSteps.Visible = True
            lblStepType.Visible = True
            lblSubmap.Visible = True
            lblResumeStep.Visible = True
            lblStepOKNewRoadmap.Visible = True
            lblStepCancelNewRoadmap.Visible = True
            lblCreateWMTask.Visible = True
            cmdShowResumeView.Visible = True

            cboCancelAction.Visible = True
            txtCancelSteps.Visible = True
            txtComponent.Visible = True
            txtComponentAction.Visible = True
            chkCreateWMTask.Visible = True
            txtDescription.Visible = True
            cboOKAction.Visible = True
            txtOKSteps.Visible = True
            txtStepElementID.Visible = True
            txtOKNewRoadmap.Visible = True
            txtCancelNewRoadmap.Visible = True
            txtResumeStep.Visible = True
            chkServerSide.Visible = True
            txtSubmap.Visible = True
            txtStepType.Visible = True
            chkStepCore.Visible = True

            cmdStepSet.Top = VB6.TwipsToPixelsY(4020)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: GetSubmapDetails
    '
    ' Description: display submap properties
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function GetSubmapDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            m_typSubmap.Code = CStr(m_oElementDOM.GetAttribute("Code"))

            m_typSubmap.Description = CStr(m_oElementDOM.GetAttribute("Description"))

            m_typSubmap.ElementID = CStr(m_oElementDOM.GetAttribute("ElementID"))

            txtSubmapCode.Text = m_typSubmap.Code
            txtSubmapDescription.Text = m_typSubmap.Description
            txtSubmapElementID.Text = m_typSubmap.ElementID

            cmdSubmapSet.Enabled = False

            ' which parms can be edited?
            If g_lUserMode = USER_MODE_USER Then
                ' objects to lock down
                fmeSubmap.Enabled = False
                lblSubmapElementID.Visible = False
                txtSubmapElementID.Visible = False
            Else
                ' objects to unlock
                fmeSubmap.Enabled = True
                lblSubmapElementID.Visible = True
                txtSubmapElementID.Visible = True
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSubmapDetails failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubmapDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeyDetails
    '
    ' Description: display key properties
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function GetKeyDetails() As Integer

        Dim result As Integer = 0
        Dim sCode, sType, sDesc, sCore As String
        Dim oParent As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            m_typKey.ElementID = CStr(m_oElementDOM.GetAttribute("ElementID"))

            m_typKey.Name = CStr(m_oElementDOM.GetAttribute("Name"))

            m_typKey.Value = CStr(m_oElementDOM.GetAttribute("Value"))

            oParent = m_oElementDOM.ParentNode

            sCore = CStr(oParent.GetAttribute("Core"))

            txtKeyElementID.Text = m_typKey.ElementID
            txtKeyName.Text = m_typKey.Name
            txtKeyValue.Text = m_typKey.Value

            Select Case txtKeyName.Text
                Case "ScriptFilename", "ScriptStartMethod", "LaunchExeFile", "document_template_id"
                    cmdKeyValueBrowse.Enabled = True
                    cmdKeyValueBrowse.Visible = True
                    cmdKeyValueBrowse.Top = VB6.TwipsToPixelsY(960)
                    txtKeyValue.ReadOnly = False
                Case Else
                    cmdKeyValueBrowse.Enabled = False
                    cmdKeyValueBrowse.Visible = False
                    cboDocTempMode.Visible = False
                    cboDocTempMode.Enabled = False
                    txtKeyValue.ReadOnly = False
            End Select

            ' which parms can be edited?
            If g_lUserMode = USER_MODE_USER Then
                ' objects to lock down
                If sCore = CStr(ROADMAP_CORE_ADMIN) Then
                    fmeKey.Enabled = False

                    lblKeyName.Enabled = False
                    lblKeyValue.Enabled = False
                    txtKeyName.Enabled = False
                    txtKeyValue.Enabled = False

                    cmdKeyValueBrowse.Enabled = False
                    cmdKeySet.Enabled = False

                Else
                    fmeKey.Enabled = True

                    lblKeyName.Enabled = False
                    lblKeyValue.Enabled = True
                    txtKeyElementID.Enabled = False
                    txtKeyName.Enabled = False
                    txtKeyValue.Enabled = True

                    cmdKeyValueBrowse.Enabled = True
                    cmdKeySet.Enabled = False

                End If

                lblKeyElementID.Visible = False
                txtKeyElementID.Visible = False


                If CStr(m_oElementDOM.GetAttribute("Name")) = "doc_template_mode" Then
                    cboDocTempMode.Visible = True
                    cboDocTempMode.Enabled = True
                    lblCaption.Visible = True
                    lblCaption.Text = "Document Template Mode"

                    txtKeyValue.Enabled = False
                    cboDiaryUserGroupId.Visible = False
                    cboDiaryUserId.Visible = False
                    cboDiaryTaskGroupId.Visible = False
                    cboDiaryTaskId.Visible = False
                    cboDiaryWMSteps.Visible = False
                    lblTempDesc.Visible = False
                    txtDocTempType.Visible = False
                    lblKeyTempCode.Visible = False
                    txtDocTemplate.Visible = False
                    cmdKeyValueBrowse.Visible = False

                    If txtKeyValue.Text.Trim() <> "" Then
                        For iRow As Integer = 0 To cboDocTempMode.Items.Count - 1
                            If VB6.GetItemData(cboDocTempMode, iRow) = Conversion.Val(txtKeyValue.Text) Then
                                cboDocTempMode.SelectedIndex = iRow
                                Exit For
                            End If
                        Next iRow
                    End If

                ElseIf CStr(m_oElementDOM.GetAttribute("Name")) = "document_template_id" Then
                    lblTempDesc.Visible = True
                    txtDocTempType.Visible = True
                    lblKeyTempCode.Visible = True
                    txtDocTemplate.Visible = True
                    cmdKeyValueBrowse.Top = VB6.TwipsToPixelsY(2160)
                    txtKeyValue.Height = VB6.TwipsToPixelsY(285)

                    txtKeyValue.Enabled = False
                    cboDocTempMode.Visible = False
                    cboDiaryUserGroupId.Visible = False
                    cboDiaryUserId.Visible = False
                    cboDiaryTaskGroupId.Visible = False
                    cboDiaryTaskId.Visible = False
                    cboDiaryWMSteps.Visible = False
                    lblCaption.Visible = False

                    m_lReturn = GetDocTempDetails(CStr(gPMFunctions.ToSafeLong(txtKeyValue.Text)), sCode, sType, sDesc)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    txtDocTempType.Text = sType.Trim()
                    txtDocTemplate.Text = sDesc.Trim()

                ElseIf CStr(m_oElementDOM.GetAttribute("Name")) = "DiaryNXMStep" Then

                    txtKeyValue.Enabled = False
                    cboDocTempMode.Visible = False
                    cboDiaryUserGroupId.Visible = False
                    cboDiaryUserId.Visible = False
                    cboDiaryTaskGroupId.Visible = False
                    cboDiaryTaskId.Visible = False
                    cboDiaryWMSteps.Visible = False
                    lblCaption.Visible = False
                    lblTempDesc.Visible = False
                    txtDocTempType.Visible = False
                    lblKeyTempCode.Visible = False
                    txtDocTemplate.Visible = False
                    cmdKeyValueBrowse.Visible = False

                ElseIf CStr(m_oElementDOM.GetAttribute("Name")) = "DiaryUserGroupId" Then
                    lblCaption.Visible = True
                    lblCaption.Text = "Diary User Group Id"
                    cboDiaryUserGroupId.Visible = True
                    cboDiaryUserGroupId.Enabled = True

                    txtKeyValue.Enabled = False
                    cboDocTempMode.Visible = False
                    cboDiaryUserId.Visible = False
                    cboDiaryTaskGroupId.Visible = False
                    cboDiaryTaskId.Visible = False
                    cboDiaryWMSteps.Visible = False
                    lblTempDesc.Visible = False
                    txtDocTempType.Visible = False
                    lblKeyTempCode.Visible = False
                    txtDocTemplate.Visible = False
                    cmdKeyValueBrowse.Visible = False

                    If txtKeyValue.Text.Trim() <> "" Then
                        For iRow As Integer = 0 To cboDiaryUserGroupId.Items.Count - 1
                            If VB6.GetItemData(cboDiaryUserGroupId, iRow) = Conversion.Val(txtKeyValue.Text) Then
                                cboDiaryUserGroupId.SelectedIndex = iRow
                                Exit For
                            End If
                        Next iRow
                    End If

                ElseIf CStr(m_oElementDOM.GetAttribute("Name")) = "DiaryUserId" Then
                    lblCaption.Visible = True
                    lblCaption.Text = "Diary User Id"
                    cboDiaryUserId.Visible = True
                    cboDiaryUserId.Enabled = True

                    txtKeyValue.Enabled = False
                    cboDocTempMode.Visible = False
                    cboDiaryUserGroupId.Visible = False
                    cboDiaryTaskGroupId.Visible = False
                    cboDiaryTaskId.Visible = False
                    cboDiaryWMSteps.Visible = False
                    lblTempDesc.Visible = False
                    txtDocTempType.Visible = False
                    lblKeyTempCode.Visible = False
                    txtDocTemplate.Visible = False
                    cmdKeyValueBrowse.Visible = False

                    If txtKeyValue.Text.Trim() <> "" Then
                        For iRow As Integer = 0 To cboDiaryUserId.Items.Count - 1
                            If VB6.GetItemData(cboDiaryUserId, iRow) = Conversion.Val(txtKeyValue.Text) Then
                                cboDiaryUserId.SelectedIndex = iRow
                                Exit For
                            End If
                        Next iRow
                    End If

                ElseIf CStr(m_oElementDOM.GetAttribute("Name")) = "DiaryTaskGroupId" Then
                    lblCaption.Visible = True
                    lblCaption.Text = "Diary Task Group Id"
                    cboDiaryTaskGroupId.Visible = True
                    cboDiaryTaskGroupId.Enabled = True

                    txtKeyValue.Enabled = False
                    cboDocTempMode.Visible = False
                    cboDiaryUserGroupId.Visible = False
                    cboDiaryUserId.Visible = False
                    cboDiaryTaskId.Visible = False
                    cboDiaryWMSteps.Visible = False
                    lblTempDesc.Visible = False
                    txtDocTempType.Visible = False
                    lblKeyTempCode.Visible = False
                    txtDocTemplate.Visible = False
                    cmdKeyValueBrowse.Visible = False

                    If txtKeyValue.Text.Trim() <> "" Then
                        For iRow As Integer = 0 To cboDiaryTaskGroupId.Items.Count - 1
                            If VB6.GetItemData(cboDiaryTaskGroupId, iRow) = Conversion.Val(txtKeyValue.Text) Then
                                cboDiaryTaskGroupId.SelectedIndex = iRow
                                Exit For
                            End If
                        Next iRow
                    End If

                ElseIf CStr(m_oElementDOM.GetAttribute("Name")) = "DiaryTaskId" Then
                    lblCaption.Visible = True
                    lblCaption.Text = "Diary Task Id"
                    cboDiaryTaskId.Visible = True
                    cboDiaryTaskId.Enabled = True

                    txtKeyValue.Enabled = False
                    cboDocTempMode.Visible = False
                    cboDiaryUserGroupId.Visible = False
                    cboDiaryUserId.Visible = False
                    cboDiaryTaskGroupId.Visible = False
                    cboDiaryWMSteps.Visible = False
                    lblTempDesc.Visible = False
                    txtDocTempType.Visible = False
                    lblKeyTempCode.Visible = False
                    txtDocTemplate.Visible = False
                    cmdKeyValueBrowse.Visible = False

                    If txtKeyValue.Text.Trim() <> "" Then
                        For iRow As Integer = 0 To cboDiaryTaskId.Items.Count - 1
                            If VB6.GetItemData(cboDiaryTaskId, iRow) = Conversion.Val(txtKeyValue.Text) Then
                                cboDiaryTaskId.SelectedIndex = iRow
                                Exit For
                            End If
                        Next iRow
                    End If

                ElseIf CStr(m_oElementDOM.GetAttribute("Name")) = "DiaryWMStep" Then
                    lblCaption.Visible = True
                    lblCaption.Text = "Diary WMStep"
                    cboDiaryWMSteps.Visible = True
                    cboDiaryWMSteps.Enabled = True

                    txtKeyValue.Enabled = False
                    cboDocTempMode.Visible = False
                    cboDiaryUserGroupId.Visible = False
                    cboDiaryUserId.Visible = False
                    cboDiaryTaskGroupId.Visible = False
                    cboDiaryTaskId.Visible = False
                    lblTempDesc.Visible = False
                    txtDocTempType.Visible = False
                    lblKeyTempCode.Visible = False
                    txtDocTemplate.Visible = False
                    cmdKeyValueBrowse.Visible = False

                    If txtKeyValue.Text.Trim() <> "" Then
                        For iRow As Integer = 0 To cboDiaryWMSteps.Items.Count - 1
                            If VB6.GetItemString(cboDiaryWMSteps, iRow) = txtKeyValue.Text.Trim() Then
                                cboDiaryWMSteps.SelectedIndex = iRow
                                Exit For
                            End If
                        Next iRow
                    End If

                Else
                    cboDocTempMode.Visible = False
                    cboDiaryUserGroupId.Visible = False
                    cboDiaryUserId.Visible = False
                    cboDiaryTaskGroupId.Visible = False
                    cboDiaryTaskId.Visible = False
                    cboDiaryWMSteps.Visible = False
                    lblCaption.Visible = False
                    lblTempDesc.Visible = False
                    txtDocTempType.Visible = False
                    lblKeyTempCode.Visible = False
                    txtDocTemplate.Visible = False
                End If
            Else
                ' objects to unlock
                fmeKey.Enabled = True

                txtKeyElementID.Enabled = True
                txtKeyName.Enabled = True
                txtKeyValue.Enabled = True

                lblKeyElementID.Visible = True
                txtKeyElementID.Visible = True
            End If

            'Resize the text box for Screen text

            If CStr(m_oElementDOM.GetAttribute("Name")) = "TEXT" Then
                txtKeyValue.Height = VB6.TwipsToPixelsY(1500)
            Else
                txtKeyValue.Height = VB6.TwipsToPixelsY(285)
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeyDetails failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeyDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetCheckBox
    '
    ' Description: set value of checkbox, accepts true/false & 0/1
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function SetCheckBox(ByRef oCheck As CheckBox, ByVal sValue As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            Select Case sValue.ToUpper()
                Case "TRUE", "1"
                    oCheck.CheckState = CheckState.Checked
                Case "FALSE", "0"
                    oCheck.CheckState = CheckState.Unchecked
                Case Else
                    Return result
            End Select


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetCheckBox failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetCheckBox", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetComboAction
    '
    ' Description: display appropriate description for code
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function SetComboAction(ByRef oCombo As ComboBox, ByVal sAction As String) As Integer

        Dim result As Integer = 0
        Dim bFound As Boolean

        Dim oName As Label
        Dim oValue As TextBox

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            bFound = False

            For lLoop As Integer = m_sActions.GetLowerBound(1) To m_sActions.GetUpperBound(1)

                If sAction = m_sActions(0, lLoop) Then
                    oCombo.SelectedIndex = lLoop
                    bFound = True
                    Exit For
                End If

            Next

            If Not (bFound) Then
                Return result
            End If

            Select Case oCombo.Name
                Case "cboOKAction"
                    oName = lblOkSteps
                    oValue = txtOKSteps
                Case "cboCancelAction"
                    oName = lblCancelSteps
                    oValue = txtCancelSteps
            End Select

            Select Case sAction
                Case gPMConstants.PMNavActionForwardX, gPMConstants.PMNavActionBackX
                    oName.Enabled = True
                    oValue.Enabled = True
                Case Else
                    oName.Enabled = False
                    oValue.Enabled = False
            End Select


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetComboAction failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetComboAction", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddAvailableStep
    '
    ' Description: adds a new step below the current one
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function AddAvailableStep(ByVal lAvailableStepID As Integer) As Integer

        Dim result As Integer = 0
        Dim vKeys, vDetails As Object
        Dim oNewElement, oElement, oParent As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' step details

            m_lReturn = m_oBusiness.GetAvailableStepDetails(lAvailableStepID:=lAvailableStepID, vStepDetails:=vDetails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' step key details. These will be the default keys required by the step

            m_lReturn = m_oBusiness.GetDefaultKeys(lAvailableStepID:=lAvailableStepID, vKeys:=vKeys)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                Return result
            End If

            ' create the new element
            oNewElement = m_oXMLDOM.CreateElement(ELEMENT_BASENAME_STEP)

            oElement = m_oElementDOM.NextSibling
            oParent = m_oElementDOM.ParentNode

            ' add its attributes
            oNewElement.SetAttribute("Description", vDetails(STEP_ATTRIB_DESCRIPTION, 0))
            oNewElement.SetAttribute("Core", "0")
            oNewElement.SetAttribute("Type", vDetails(STEP_ATTRIB_TYPE, 0))
            oNewElement.SetAttribute("Component", vDetails(STEP_ATTRIB_COMPONENT, 0))
            oNewElement.SetAttribute("CancelAction", vDetails(STEP_ATTRIB_CANCELACTION, 0))
            If oElement Is Nothing Then
                oNewElement.SetAttribute("OKAction", gPMConstants.PMNavActionCompleteProcess)
            Else
                oNewElement.SetAttribute("OKAction", vDetails(STEP_ATTRIB_OKACTION, 0))
            End If
            oNewElement.SetAttribute("OKSteps", vDetails(STEP_ATTRIB_OKSTEPS, 0))
            oNewElement.SetAttribute("CancelSteps", vDetails(STEP_ATTRIB_CANCELSTEPS, 0))
            oNewElement.SetAttribute("ResumeStep", vDetails(STEP_ATTRIB_RESUMESTEP, 0))
            oNewElement.SetAttribute("ComponentAction", vDetails(STEP_ATTRIB_COMPONENTACTION, 0))
            oNewElement.SetAttribute("ServerSide", vDetails(STEP_ATTRIB_SERVERSIDE, 0))
            oNewElement.SetAttribute("CreateWMTask", vDetails(STEP_ATTRIB_CREATEWMTASK, 0))
            oNewElement.SetAttribute("Submap", vDetails(STEP_ATTRIB_SUBMAP, 0))
            oNewElement.SetAttribute("OKNewRoadmap", vDetails(STEP_ATTRIB_OKNEWROADMAP, 0))
            oNewElement.SetAttribute("CancelNewRoadmap", vDetails(STEP_ATTRIB_CANCELNEWROADMAP, 0))

            ' get unique element ID
            m_lReturn = SetElementID(oElement:=oNewElement)

            If oElement Is Nothing Then
                ' add new element to end of parents' children
                m_oElementDOM.SetAttribute("OKAction", gPMConstants.PMNavActionForwardOne)

                oElement = oParent.AppendChild(oNewElement)
            Else
                ' current element was not last, so add after current element

                oElement = oParent.InsertBefore(oNewElement, oElement)
            End If

            ' add keys if required
            If Not Information.IsArray(vKeys) Then
                ' no keys to add
                m_lReturn = BuildTreeView(tvwTreeView:=tvwMap)
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            ' each key

            For lLoop As Integer = vKeys.GetLowerBound(1) To vKeys.GetUpperBound(1)
                oNewElement = m_oXMLDOM.CreateElement(ELEMENT_BASENAME_KEY)



                If CStr(vDetails(STEP_ATTRIB_DESCRIPTION, 0)) = "DIARY" And CStr(vKeys(0, lLoop)) = "DiaryNXMStep" Then
                    oNewElement.SetAttribute("Name", vKeys(0, lLoop))
                    oNewElement.SetAttribute("Value", "WMTASK")
                Else
                    oNewElement.SetAttribute("Name", vKeys(0, lLoop))
                    oNewElement.SetAttribute("Value", "")
                End If

                m_lReturn = SetElementID(oNewElement)


                oNewElement = oElement.AppendChild(oNewElement)

            Next

            ' refresh the form's treeview
            m_lReturn = BuildTreeView(tvwTreeView:=tvwMap)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAvailableStep failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddAvailableStep", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetElementID
    '
    ' Description: add ElementID with next unique number
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function SetElementID(ByRef oElement As XmlElement) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_lMaxElementID += 1

            oElement.SetAttribute("ElementID", ELEMENT_ID_PREFIX & CStr(m_lMaxElementID))


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetElementID failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetElementID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckMapStatus
    '
    ' Description: Check status of map and if it's okay to exit
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function CheckMapStatus() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If m_bDirty Then
                m_lReturn = MessageBox.Show("Map has been edited and not saved." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Save it now?", ACApp, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)

                If m_lReturn = System.Windows.Forms.DialogResult.Cancel Then
                    Return result
                End If

                If m_lReturn = System.Windows.Forms.DialogResult.Yes Then
                    m_lReturn = SaveRoadmap(bShowDiag:=True)
                End If
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMapStatus failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMapStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: StatusMsg
    '
    ' Description: display message in status box at foot of form
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Sub StatusMsg(ByVal sMsg As String)

        sbStatus.Items.Item(0).Text = sMsg

    End Sub

    ' ***************************************************************** '
    ' Name: SaveElementAttributes
    '
    ' Description: when user clicks another element, ensure that changed
    '              properties are saved
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function SaveElementAttributes(ByVal oElement As XmlElement) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If oElement Is Nothing Then
                ' save the map attributes, although it could be a deleted step
                m_lReturn = SaveMapAttributes()
            Else
                Select Case oElement.LocalName.ToUpper()
                    Case ELEMENT_BASENAME_SUBMAP
                        m_lReturn = SaveSubmapAttributes(oElement:=oElement)
                    Case ELEMENT_BASENAME_STEP
                        m_lReturn = SaveStepAttributes(oElement:=oElement)
                    Case ELEMENT_BASENAME_KEY
                        m_lReturn = SaveKeyAttributes(oElement:=oElement)
                    Case Else
                        ' unknown element
                        m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                End Select
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to save element attributes", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveElementAttributes failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveElementAttributes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SaveMapAttributes
    '
    ' Description: update map attributes in memory
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function SaveMapAttributes() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            tvwMap.Nodes.Item(TREEVIEW_NODE_ROOT).Text = txtMapRoadmapName.Text

            m_oXMLDOM.DocumentElement.SetAttribute("AutoClose", txtMapAutoClose.Text)
            m_oXMLDOM.DocumentElement.SetAttribute("ImageURL", txtMapImageURL.Text)
            m_oXMLDOM.DocumentElement.SetAttribute("NavigatorDriven", txtMapNavigatorDriven.Text)
            m_oXMLDOM.DocumentElement.SetAttribute("ProcessMode", txtMapProcessMode.Text)
            m_oXMLDOM.DocumentElement.SetAttribute("RoadmapName", txtMapRoadmapName.Text)
            m_oXMLDOM.DocumentElement.SetAttribute("Title", txtMapTitle.Text)
            m_oXMLDOM.DocumentElement.SetAttribute("TransactionType", txtMapTransactionType.Text)
            m_oXMLDOM.DocumentElement.SetAttribute("WMTaskCode", txtMapWMTaskCode.Text)
            m_oXMLDOM.DocumentElement.SetAttribute("WMTaskDescription", txtMapWMTaskDescription.Text)
            m_oXMLDOM.DocumentElement.SetAttribute("Version", txtMapVersion.Text)
            m_oXMLDOM.DocumentElement.SetAttribute("Core", chkMapCore.CheckState)
            m_oXMLDOM.DocumentElement.SetAttribute("ElementID", txtMapElementID.Text)

            m_bElDirty = False
            m_bDirty = True

            StatusMsg("Map attributes saved")

            cmdMapSet.Enabled = False


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveMapAttributes failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveMapAttributes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SaveSubmapAttributes
    '
    ' Description: update submap attributes in memory
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function SaveSubmapAttributes(ByVal oElement As XmlElement) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            tvwMap.Nodes.Item(m_sLastNodeKey).Text = txtSubmapDescription.Text

            oElement.SetAttribute("Code", txtSubmapCode.Text)
            oElement.SetAttribute("Description", txtSubmapDescription.Text)
            oElement.SetAttribute("ElementID", txtSubmapElementID.Text)

            m_bElDirty = False
            m_bDirty = True

            StatusMsg("Submap attributes saved")

            cmdSubmapSet.Enabled = False


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveSubmapAttributes failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveSubmapAttributes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SaveStepAttributes
    '
    ' Description: update step attributes in memory
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function SaveStepAttributes(ByRef oElement As XmlElement) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            tvwMap.Nodes.Item(m_sLastNodeKey).Text = txtDescription.Text

            oElement.SetAttribute("CancelAction", GetActionComboCode(cboCancelAction))
            oElement.SetAttribute("CancelSteps", txtCancelSteps.Text)
            oElement.SetAttribute("Component", txtComponent.Text)
            oElement.SetAttribute("ComponentAction", txtComponentAction.Text)
            oElement.SetAttribute("CreateWMTask", chkCreateWMTask.CheckState)
            oElement.SetAttribute("Description", txtDescription.Text)
            oElement.SetAttribute("OKAction", GetActionComboCode(cboOKAction))
            oElement.SetAttribute("OKSteps", txtOKSteps.Text)
            oElement.SetAttribute("OKNewRoadmap", txtOKNewRoadmap.Text)
            oElement.SetAttribute("CancelNewRoadmap", txtCancelNewRoadmap.Text)
            oElement.SetAttribute("ResumeStep", txtResumeStep.Text)
            oElement.SetAttribute("ServerSide", chkServerSide.CheckState)
            oElement.SetAttribute("Submap", txtSubmap.Text)
            oElement.SetAttribute("Type", txtStepType.Text)
            oElement.SetAttribute("ElementID", txtStepElementID.Text)
            oElement.SetAttribute("Core", chkStepCore.CheckState)

            m_bElDirty = False
            m_bDirty = True

            StatusMsg("Step attributes saved")

            cmdStepSet.Enabled = False


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveStepAttributes failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveStepAttributes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SaveKeyAttributes
    '
    ' Description:  update key attributes in memory
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function SaveKeyAttributes(ByVal oElement As XmlElement) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            tvwMap.Nodes.Item(m_sLastNodeKey).Text = txtKeyName.Text

            oElement.SetAttribute("Name", txtKeyName.Text)
            oElement.SetAttribute("Value", txtKeyValue.Text)
            oElement.SetAttribute("ElementID", txtKeyElementID.Text)

            m_bElDirty = False
            m_bDirty = True

            StatusMsg("Key attributes saved")

            cmdKeySet.Enabled = False


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveKeyAttributes failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveKeyAttributes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetActionComboCode
    '
    ' Description: get user-friendly version of step action code
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function GetActionComboCode(ByRef oCombo As ComboBox) As String

        Dim result As String = String.Empty

        Try

            result = ""

            For lLoop As Integer = m_sActions.GetLowerBound(1) To m_sActions.GetUpperBound(1)
                If m_sActions(1, lLoop) = oCombo.Text Then
                    result = m_sActions(0, lLoop)
                    Exit For
                End If
            Next

            Return result

        Catch excep As System.Exception



            result = ""

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetActionComboCode failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetActionComboCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ExportRoadmap
    '
    ' Description: write roadmap out to text file
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function ExportRoadmap() As Integer

        Dim result As Integer = 0
        Dim lFH As Integer
        Dim lRepeat As DialogResult
        Dim sCode, sDesc, sExportFilename As String

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If m_lNavXMProcessID = 0 Then
                MessageBox.Show("Nothing available to export", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)

                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            ' users cannot export a sirius roadmap
            If g_lUserMode = USER_MODE_USER And m_lNavXMProcessVersionID <= 0 Then
                MessageBox.Show("This is a core (Sirius) roadmap and cannot be exported." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Create and export a copy of the roadmap.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            sCode = ""
            sDesc = ""

            ' sort out the default export name for this roadmap
            If m_lNavXMProcessVersionID <= 0 Then
                sExportFilename = m_sFilename.Substring(0, m_sFilename.IndexOf("."c)) & FILENAME_EXTENSION_EXPORT & FILENAME_SUFFIX_EXPORT
            Else
                m_lReturn = GetVersionDetails(sCode:=sCode, sDesc:=sDesc)

                sExportFilename = m_sFilename.Substring(0, m_sFilename.IndexOf("."c)) & "_" & sCode & FILENAME_EXTENSION_EXPORT & FILENAME_SUFFIX_EXPORT
            End If

            ' call the common save dialogue
            CommonDialog1Open.FileName = m_sNavFilePath & sExportFilename
            CommonDialog1Save.FileName = m_sNavFilePath & sExportFilename


            'TODO: Find the appropriate replacement of CancelError property and uncomment the below code
            'CommonDialog1Open.CancelError = True

            CommonDialog1Open.Filter = "Navigator XM Export files (*" & _
                                       FILENAME_SUFFIX_EXPORT & ")|*" & FILENAME_SUFFIX_EXPORT & "|All files (*.*)|*.*"
            CommonDialog1Save.Filter = "Navigator XM Export files (*" & _
                                       FILENAME_SUFFIX_EXPORT & ")|*" & FILENAME_SUFFIX_EXPORT & "|All files (*.*)|*.*"

            lRepeat = System.Windows.Forms.DialogResult.No

            Do While lRepeat = System.Windows.Forms.DialogResult.No

                ' repeat until a unique filename is entered, or the user cancels
                CommonDialog1Save.ShowDialog()
                CommonDialog1Open.FileName = CommonDialog1Save.FileName

                sExportFilename = CommonDialog1Open.FileName

                m_lReturn = CheckFileExists(sFilename:=sExportFilename, bExists:=m_bRet)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to check if export filename already exists", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                    Return result
                End If

                If m_bRet Then
                    lRepeat = MessageBox.Show("File already exists: " & sExportFilename & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Ok to overwrite this file?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                Else
                    lRepeat = System.Windows.Forms.DialogResult.Yes
                End If

            Loop

            ' make sure that the roadmap file is updated. This
            ' ensures that the exported map matches the one on disk
            m_lReturn = SaveRoadmap(bShowDiag:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to save roadmap", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Return result
            End If

            lFH = FileSystem.FreeFile()
            FileSystem.FileOpen(lFH, sExportFilename, OpenMode.Output)

            FileSystem.PrintLine(lFH, EXPORT_INFO_COMPANY_NAME)
            FileSystem.PrintLine(lFH, EXPORT_INFO_TITLE)
            FileSystem.PrintLine(lFH, "Exported on " & DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss"))
            FileSystem.PrintLine(lFH, "Roadmap: " & m_sFilename)
            FileSystem.PrintLine(lFH, "Code: " & sCode)
            FileSystem.PrintLine(lFH, "Description: " & sDesc)
            FileSystem.PrintLine(lFH, m_oXMLDOM.InnerXml)

            FileSystem.FileClose(lFH)

            StatusMsg("Roadmap exported successfully")


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            If Information.Err().Number = ERROR_CANCEL_SELECTED Then
                ' user cancelled the export

                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExportRoadmap failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExportRoadmap", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ImportRoadmap
    '
    ' Description: load roadmap from import file. Creates new copy if
    '              custom map from source system
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function ImportRoadmap() As Integer

        Dim result As Integer = 0
        Dim lFH, lNavXMProcessID, lCore, lVersion As Integer
        Dim sXML, sTemp, sCode, sDesc, sRoadmapFilename, sImportFilename As String

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = MessageBox.Show("Import process will load the imported roadmap into the editor." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Ok to proceed?", ACApp, MessageBoxButtons.OKCancel, MessageBoxIcon.Question)

            If m_lReturn <> System.Windows.Forms.DialogResult.OK Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If


            m_lReturn = GetOpenFilename("Navigator XM Export files (*" & FILENAME_SUFFIX_EXPORT & ")|*" & FILENAME_SUFFIX_EXPORT & "|All files (*.*)|*.*", sImportFilename)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' load the selected file
            lFH = FileSystem.FreeFile()

            FileSystem.FileOpen(lFH, sImportFilename, OpenMode.Input)

            ' first three lines contain header info - nothing important
            sTemp = FileSystem.LineInput(lFH)
            sTemp = FileSystem.LineInput(lFH)
            sTemp = FileSystem.LineInput(lFH)

            ' target roadmap filename
            sTemp = FileSystem.LineInput(lFH)
            sRoadmapFilename = Mid(sTemp, (sTemp.IndexOf(": ") + 1) + 1).Trim()

            ' custom map code
            sTemp = FileSystem.LineInput(lFH)
            sCode = Mid(sTemp, (sTemp.IndexOf(": ") + 1) + 1).Trim()

            ' custom map description
            sTemp = FileSystem.LineInput(lFH)
            sDesc = Mid(sTemp, (sTemp.IndexOf(": ") + 1) + 1).Trim()

            ' if code and description are not empty, the map is a custom job

            ' load the rest of the file, which contains the XML of the exported file
            sXML = ""

            Do While Not FileSystem.EOF(lFH)
                sTemp = FileSystem.LineInput(lFH)
                sXML = sXML & sTemp & Strings.Chr(13) & Strings.Chr(10)
            Loop

            FileSystem.FileClose(lFH)

            ' strip last CR LF
            sXML = sXML.Substring(0, sXML.Length - 2)

            ' does the file exist in PMNavXM_Process?
            m_lReturn = CheckRoadmapProcessExists(sRoadmapFilename:=sRoadmapFilename, lNavXMProcessID:=lNavXMProcessID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to check if roadmap name valid", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Return result
            End If

            If lNavXMProcessID = ID_NO_VALUE And sCode = "" And sDesc = "" Then
                ' file does not exist in PMNavXM_Process
                MessageBox.Show("Roadmap name is not valid." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "It does not have an associated roadmap process.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Return result
            End If

            ' load up the XML into a DOM
            m_oXMLDOM = New XmlDocument()


            'TODO: Find the appropriate replacement of validateOnParse property and uncomment the below code
            ' m_oXMLDOM.validateOnParse = False
            m_oXMLDOM.PreserveWhitespace = False
            ' for IDs

            'TODO: Find the appropriate replacement of resolveExternals property and uncomment the below code
            ' m_oXMLDOM.resolveExternals = False

            ' load roadmap from string
            Try
                m_oXMLDOM.LoadXml(sXML)
                m_bRet = True

            Catch parseError As System.Exception
                m_bRet = False
                ' that didn't work
                MessageBox.Show("Failed to load XML into DOM document", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Return result
            End Try



            lCore = CInt(m_oXMLDOM.DocumentElement.GetAttribute("Core"))

            lVersion = CInt(m_oXMLDOM.DocumentElement.GetAttribute("Version"))

            If sCode = "" And sDesc = "" Then
                ' map is core
                m_lReturn = ImportCoreRoadmap(lNavXMProcessID, lCore, lVersion, sRoadmapFilename)
            Else
                ' map is custom
                m_lReturn = ImportCustomRoadmap(lNavXMProcessID, lCore, lVersion, sRoadmapFilename, sCode, sDesc)
            End If

            ' load the new roadmap into the editor, so the user can view it
            m_lReturn = LoadXMLData(lNavXMProcessID:=lNavXMProcessID, lNavXMProcessVersionID:=m_lNavXMProcessVersionID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to load the roadmap into the editor", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Return result
            End If

            StatusMsg("Roadmap imported successfully")


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImportRoadmap failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportRoadmap", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ImportCoreRoadmap
    '
    ' Description: loads core map from file, overwriting core map
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function ImportCoreRoadmap(ByVal lNavXMProcessID As Integer, ByVal lCore As Integer, ByVal lVersion As Integer, ByVal sRoadmapFilename As String) As Integer

        Dim result As Integer = 0
        Dim lFH As Integer
        Dim sXML As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sXML = m_oXMLDOM.InnerXml

            ' save the imported file
            lFH = FileSystem.FreeFile()

            FileSystem.FileOpen(lFH, m_sNavFilePath & sRoadmapFilename, OpenMode.Output)
            FileSystem.PrintLine(lFH, sXML)
            FileSystem.FileClose(lFH)

            ' update PMNavXM_Process

            m_lReturn = m_oBusiness.SaveXMLData(lNavXMProcessID:=lNavXMProcessID, lVersion:=lVersion, lCore:=lCore, sXML:=sXML)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' bombed
                MessageBox.Show("Failed to update the XML definition", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImportCoreRoadmap failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportCoreRoadmap", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ImportCustomRoadmap
    '
    ' Description: load custom export
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function ImportCustomRoadmap(ByVal lNavXMProcessID As Integer, ByVal lCore As Integer, ByVal lVersion As Integer, ByVal sRoadmapFilename As String, ByVal sCode As String, ByVal sDesc As String) As Integer

        Dim result As Integer = 0
        Dim bNew As Boolean
        Dim lNavXMProcessVersionID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            m_lReturn = m_oBusiness.GetProcessVersionID(sCode:=sCode, sDesc:=sDesc, lNavXMProcessID:=lNavXMProcessID, lNavXMProcessVersionID:=lNavXMProcessVersionID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lNavXMProcessID = lNavXMProcessID
            m_lNavXMProcessVersionID = lNavXMProcessVersionID

            If lNavXMProcessVersionID <> ID_NO_VALUE Then
                ' save
                bNew = False
            Else
                ' save as
                bNew = True
            End If


            Return SaveRoadmapVersion(bNew:=bNew, sCode:=sCode, sDesc:=sDesc)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImportCustomRoadmap failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportCustomRoadmap", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckRoadmapProcessExists
    '
    ' Description: check PMNavXM_Process to see if the supplied roadmap
    '              is assigned to a nav process
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function CheckRoadmapProcessExists(ByVal sRoadmapFilename As String, ByRef lNavXMProcessID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' call the business

            m_lReturn = m_oBusiness.CheckRoadmapProcessExists(sRoadmapFilename:=sRoadmapFilename, lNavXMProcessID:=lNavXMProcessID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckRoadmapProcessExists failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckRoadmapProcessExists", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDocumentTemplate
    '
    ' Description: for standard letter step
    '
    ' ***************************************************************** '
    Private Function GetDocumentTemplate(ByRef r_lDocumentTemplateId As Integer, ByRef r_sDocumentType As String, ByRef r_sDocumentTemplate As String) As Integer

        Dim result As Integer = 0
        Dim oDocTemplate As iPMBFindDocTemplate.Interface_Renamed

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oDocTemplate = New iPMBFindDocTemplate.Interface_Renamed()

            m_lReturn = CType(oDocTemplate, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oDocTemplate = Nothing
                Return result
            End If

            oDocTemplate.CallingAppName = ACApp

            m_lReturn = oDocTemplate.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oDocTemplate = Nothing
                Return result
            End If

            oDocTemplate.Mode = 1

            m_lReturn = oDocTemplate.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oDocTemplate = Nothing
                Return result
            End If

            r_lDocumentTemplateId = oDocTemplate.DocumentTemplateId
            r_sDocumentTemplate = oDocTemplate.DocumentTemplateDescription
            r_sDocumentType = oDocTemplate.DocumentTypeDescription

            oDocTemplate.Dispose()
            oDocTemplate = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocumentTemplate failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentTemplate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetKeyValue
    '
    ' Description: get key value, based on type of step
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function GetKeyValue() As Integer

        Dim result As Integer = 0
        Dim sKeyName, sFilename, sScriptStartMethod As String
        Dim lDocumentTemplateId As Integer
        Dim sDocumentType, sDocumentTemplate As String

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sKeyName = txtKeyName.Text

            Select Case sKeyName
                Case "ScriptFilename"
                    ' User Component (script)
                    m_lReturn = GetOpenFilename("VB Script files (*.vbs)|*.vbs|All files (*.*)|*.*", sFilename)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If

                    If sFilename <> "" Then
                        m_bElDirty = True
                        txtKeyValue.Text = sFilename
                    End If
                Case "ScriptStartMethod"
                    ' User Component (script)
                    m_lReturn = GetScriptStartMethod(sScriptStartMethod)
                Case "LaunchExeFile"
                    ' Launch EXE
                    m_lReturn = GetOpenFilename("All files (*.*)|*.*", sFilename)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If

                    If sFilename <> "" Then
                        m_bElDirty = True
                        txtKeyValue.Text = sFilename
                    End If
                Case "document_template_id"
                    m_lReturn = GetDocumentTemplate(r_lDocumentTemplateId:=lDocumentTemplateId, r_sDocumentType:=sDocumentType, r_sDocumentTemplate:=sDocumentTemplate)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If

                    txtKeyValue.Text = CStr(lDocumentTemplateId)
                    txtDocTemplate.Text = sDocumentTemplate.Trim()
                    txtDocTempType.Text = sDocumentType.Trim()
                    If txtKeyValue.Text <> "" Then
                        m_bElDirty = True
                    End If
                Case Else
                    Return gPMConstants.PMEReturnCode.PMTrue
            End Select

            cmdKeySet.Enabled = True


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeyValue failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeyValue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetScriptStartMethod
    '
    ' Description: load script and see what methods are available, so
    '              that user can select the start method
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function GetScriptStartMethod(ByRef sScriptStartMethod As String) As Integer

        Dim result As Integer = 0
        Dim sCode As String = ""

        Dim sScriptFilename As String = ""
        Dim oParent As XmlElement

        Dim oForm As frmScriptProcs

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            oParent = m_oElementDOM.ParentNode

            sScriptFilename = ""

            For Each oFilename As XmlElement In oParent.ChildNodes

                If CStr(oFilename.GetAttribute("Name")) = "ScriptFilename" Then

                    sScriptFilename = CStr(oFilename.GetAttribute("Value"))
                End If
            Next oFilename

            If sScriptFilename = "" Then
                MessageBox.Show("Filename not set", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return result
            End If

            m_lReturn = CheckFileExists(sScriptFilename, m_bRet)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to check if script file exists", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return result
            End If

            If Not m_bRet Then
                MessageBox.Show("Script file does not exist", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return result
            End If

            oForm = New frmScriptProcs()

            With oForm
                .ScriptFilename = sScriptFilename

                m_lReturn = .Initialise()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                m_lReturn = .Load_Renamed()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                .ShowDialog()

                m_lStatus = .Status
                sScriptStartMethod = .ProcedureName
            End With

            oForm = Nothing

            If m_lStatus <> gPMConstants.PMEReturnCode.PMOK Then
                Return result
            End If

            m_bElDirty = True
            txtKeyValue.Text = sScriptStartMethod


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeyValue failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeyValue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetOpenFilename
    '
    ' Description: select file for load via common dialog
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function GetOpenFilename(ByVal sFilter As String, ByRef sFilename As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sFilename = ""

            ' error if user cancels dialogue

            'TODO: Find the appropriate replacement of CancelError property and uncomment the below code
            'CommonDialog1Open.CancelError = True

            ' file filter for export files

            CommonDialog1Open.Filter = sFilter
            CommonDialog1Save.Filter = sFilter

            m_bRet = False

            Do While Not m_bRet

                ' loop until the user enters a file that exists, or cancels
                CommonDialog1Open.ShowDialog()
                CommonDialog1Save.FileName = CommonDialog1Open.FileName

                sFilename = CommonDialog1Open.FileName

                m_lReturn = CheckFileExists(sFilename:=sFilename, bExists:=m_bRet)

            Loop


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            If Information.Err().Number = ERROR_CANCEL_SELECTED Then
                ' user cancelled the dialog
                Return result
            End If

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOpenFilename failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOpenFilename", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UserInputDescription
    '
    ' Description: code and description for new/existing custom map
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function UserInputDescription(ByVal lMode As Integer, ByRef sCode As String, ByRef sDesc As String) As Integer

        Dim result As Integer = 0
        Dim vCodes As Object
        Dim oForm As frmDescription

        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            m_lReturn = m_oBusiness.GetVersionCodes(lMode:=lMode, vCodes:=vCodes)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to get existing version codes from PMNavXM_Process_Version", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Return result
            End If

            oForm = New frmDescription()

            With oForm
                .Mode = lMode
                .Code = sCode
                .Desc = sDesc

                .VersionCodes = vCodes

                .ShowDialog()

                m_lStatus = .Status

                sCode = .Code
                sDesc = .Desc
            End With

            oForm = Nothing


            Return m_lStatus

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UserInputDescription failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UserInputDescription", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetVersionDetails
    '
    ' Description: get code and description for a custom map
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function GetVersionDetails(ByRef sCode As String, ByRef sDesc As String) As Integer

        Dim result As Integer = 0
        Try



            Return m_oBusiness.GetVersionDetails(lNavXMProcessVersionID:=m_lNavXMProcessVersionID, sCode:=sCode, sDesc:=sDesc)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetVersionDetails failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVersionDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SaveRoadmapVersion
    '
    ' Description: save a new custom version of a roadmap
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function SaveRoadmapVersion(ByVal bNew As Boolean, ByVal sCode As String, ByVal sDesc As String) As Integer

        Dim result As Integer = 0
        Dim lPVID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If bNew Then
                lPVID = ID_NO_VALUE
            Else
                lPVID = m_lNavXMProcessVersionID
            End If


            m_lReturn = m_oBusiness.SaveRoadMapVersion(lNavXMProcessID:=m_lNavXMProcessID, lNavXMProcessVersionID:=lPVID, sCode:=sCode, sDesc:=sDesc, sXML:=m_oXMLDOM.InnerXml)

            If bNew Then
                m_lNavXMProcessVersionID = lPVID
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveRoadmapVersion failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveRoadmapVersion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PromoteVersion
    '
    ' Description: create a new Work Manager task with associated map
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function PromoteVersion() As Integer

        Dim result As Integer = 0
        Dim bExists As Boolean
        Dim sCode, sDesc As String
        Dim vTaskCodes As Object
        Dim sXML As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' anything loaded?
            If m_lNavXMProcessID <= 0 Then
                MessageBox.Show("No roadmap loaded", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)

                Return result
            End If

            ' users can only promote a user roadmap
            If m_lNavXMProcessVersionID <= 0 And g_lUserMode = USER_MODE_USER Then
                MessageBox.Show("This is a core roadmap, and cannot be promoted", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)

                Return result
            End If

            m_lReturn = MessageBox.Show("Promote the currently loaded roadmap?" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "A new Work Manager Task will be created", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If m_lReturn <> System.Windows.Forms.DialogResult.Yes Then
                Return result
            End If

            If m_bDirty Then

                ' save the roadmap if it has been edited since it was loaded
                m_lReturn = SaveRoadmap(bShowDiag:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to save the roadmap", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                    Return result
                End If

            End If


            m_lReturn = m_oBusiness.GetTaskCodes(vTaskCodes:=vTaskCodes)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to get task codes", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return result
            End If

            bExists = True

            Do While bExists
                ' get code and description for this task
                m_lReturn = UserInputDescription(lMode:=MSG_MODE_NEWTASK, sCode:=sCode, sDesc:=sDesc)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMOK Then
                    Return result
                End If

                sCode = sCode.Replace("'", "")
                sDesc = sDesc.Replace("'", "''")

                bExists = False


                For lLoop As Integer = vTaskCodes.GetLowerBound(1) To vTaskCodes.GetUpperBound(1)

                    If CStr(vTaskCodes(0, lLoop)).Trim().ToUpper() = sCode.Trim().ToUpper() Then
                        MessageBox.Show("Task code already exists. A unique code must be specified", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        bExists = True
                        Exit For
                    End If
                Next

            Loop

            'Update new TaskCode and Description into XML
            sXML = m_oXMLDOM.InnerXml
            m_lReturn = UpdateTaskCode(sXML:=sXML, sCode:=sCode, sDesc:=sDesc)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to update TaskCode in XML.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Return result
            End If


            m_lReturn = m_oBusiness.PromoteVersion(lNavXMProcessID:=m_lNavXMProcessID, lNavXMProcessVersionID:=m_lNavXMProcessVersionID, sCode:=sCode, sDesc:=sDesc, sXML:=sXML, sNavFilePath:=m_sNavFilePath)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to promote the roadmap", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Return result
            End If



            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PromoteVersion failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PromoteVersion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ******************************************************************
    ' Name: UpdateTaskCode
    ' Description: Update WMTaskCode & WMTaskDescription in promoted XML
    ' ******************************************************************
    Private Function UpdateTaskCode(ByRef sXML As String, ByVal sCode As String, ByVal sDesc As String) As Integer

        Dim result As Integer = 0
        Dim sStrtXML, sEndXML As String
        Dim oXMLDOM As XmlDocument

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oXMLDOM = New XmlDocument()

            'Extract XML text before the Map Element
            sStrtXML = Mid(sXML, 1, sXML.IndexOf("<MAP"))

            'Extract XML text From the Map Element
            sEndXML = Mid(sXML, sXML.IndexOf("<MAP") + 1)

            'Load XML object
            Try
                oXMLDOM.LoadXml(sEndXML)

            Catch
            End Try

            'Set new TaskCode,Description and Version No.
            oXMLDOM.DocumentElement.SetAttribute("WMTaskCode", sCode)
            oXMLDOM.DocumentElement.SetAttribute("WMTaskDescription", sDesc)
            oXMLDOM.DocumentElement.SetAttribute("ImageURL", oXMLDOM.DocumentElement.GetAttribute("ImageURL"))
            oXMLDOM.DocumentElement.SetAttribute("TransactionType", oXMLDOM.DocumentElement.GetAttribute("TransactionType"))
            oXMLDOM.DocumentElement.SetAttribute("ProcessMode", oXMLDOM.DocumentElement.GetAttribute("ProcessMode"))
            oXMLDOM.DocumentElement.SetAttribute("AutoClose", oXMLDOM.DocumentElement.GetAttribute("AutoClose"))
            oXMLDOM.DocumentElement.SetAttribute("NavigatorDriven", oXMLDOM.DocumentElement.GetAttribute("NavigatorDriven"))
            oXMLDOM.DocumentElement.SetAttribute("Title", oXMLDOM.DocumentElement.GetAttribute("Title"))
            oXMLDOM.DocumentElement.SetAttribute("RoadmapName", oXMLDOM.DocumentElement.GetAttribute("RoadmapName"))
            oXMLDOM.DocumentElement.SetAttribute("Core", oXMLDOM.DocumentElement.GetAttribute("Core"))
            oXMLDOM.DocumentElement.SetAttribute("ElementID", oXMLDOM.DocumentElement.GetAttribute("ElementID"))
            oXMLDOM.DocumentElement.SetAttribute("Version", 1)

            sXML = sStrtXML & oXMLDOM.InnerXml

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateTaskCode failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTaskCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: TestRunRoadmap
    '
    ' Description: run the roadmap through NavigatorXM
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function TestRunRoadmap() As Integer

        Dim result As Integer = 0
        Dim lFH As Integer
        Dim sFilename As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If m_lNavXMProcessID <= 0 Then
                Return result
            End If

            m_lReturn = MessageBox.Show("The loaded roadmap will be run via Navigator XM" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Ok to run the roadmap?", ACApp, MessageBoxButtons.OKCancel, MessageBoxIcon.Question)

            If m_lReturn <> System.Windows.Forms.DialogResult.OK Then
                Return result
            End If

            m_sTestFilename = "temp" & (CStr(DateTime.Now.TimeOfDay.TotalSeconds * 100)) & ".xml"

            lFH = FileSystem.FreeFile()

            FileSystem.FileOpen(lFH, m_sNavFilePath & m_sTestFilename, OpenMode.Output)
            FileSystem.PrintLine(lFH, m_oXMLDOM.InnerXml)
            FileSystem.FileClose(lFH)

            Me.Enabled = False

            m_oNavigatorXM = New iPMNavigatorXM.Interface_Renamed()

            With m_oNavigatorXM
                .XMLFileName = m_sTestFilename
                m_lReturn = .Initialise()
                m_lReturn = .Start()
            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TestRunRoadmap failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TestRunRoadmap", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: m_oNavigatorXM_NavigatorClose
    '
    ' Description: terminate the Navigator after test-running a map
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Sub m_oNavigatorXM_NavigatorClose() Handles m_oNavigatorXM.NavigatorClose

        Me.Enabled = True

		m_oNavigatorXM.Dispose()

        m_oNavigatorXM = Nothing

        ' delete the file used by the roadmap test run
        If m_sTestFilename.Substring(0, 4).ToLower() = "temp" Then
            File.Delete(m_sNavFilePath & m_sTestFilename)
        End If

    End Sub

    ' ***************************************************************** '
    ' Name: ValidateRoadmap
    '
    ' Description: check roadmap properties are valid
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function ValidateRoadmap() As Integer

        Dim result As Integer = 0
        Dim bExists As Boolean
        Dim sKeyParent, sKeyName, sKeyValue, sNextKeyValue, sErrorDetails As String
        Dim vKeys(,) As Object

        result = gPMConstants.PMEReturnCode.PMFalse

        ' get all the keys
        m_lReturn = FindKeys(vKeys)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' probablt getting the keys
            Return result
        End If

        For lLoop As Integer = vKeys.GetLowerBound(1) To vKeys.GetUpperBound(1)

            ' get key attributes

            sKeyParent = CStr(vKeys(KEYARRAY_PARENT, lLoop))

            sKeyName = CStr(vKeys(KEYARRAY_NAME, lLoop))

            sKeyValue = CStr(vKeys(KEYARRAY_VALUE, lLoop))

            Select Case sKeyName
                Case "ScriptFilename"
                    ' scriptfilename and script start method

                    sNextKeyValue = CStr(vKeys(KEYARRAY_VALUE, lLoop + 1))

                    sErrorDetails = Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                                    "Step name: " & sKeyParent & Strings.Chr(13) & Strings.Chr(10) & _
                                    "Filename: " & sKeyValue & Strings.Chr(13) & Strings.Chr(10) & _
                                    "StartMethod: " & sNextKeyValue

                    m_lReturn = CheckFileExists(sFilename:=sKeyValue, bExists:=bExists)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to check if script file exists:" & sErrorDetails, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return result
                    End If

                    If Not bExists Then
                        m_lReturn = MessageBox.Show("Warning: script file does not exist." & sErrorDetails & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Ok to save the roadmap?", ACApp, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)

                        If m_lReturn <> System.Windows.Forms.DialogResult.OK Then
                            Return result
                        End If

                    End If

                    If bExists Then
                        m_lReturn = ValidateScript(sFilename:=sKeyValue, sStartMethod:=sNextKeyValue)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = MessageBox.Show("Warning: script's start method does not exist." & sErrorDetails & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Ok to save the roadmap?", ACApp, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)

                            If m_lReturn <> System.Windows.Forms.DialogResult.OK Then
                                Return gPMConstants.PMEReturnCode.PMCancel
                            End If
                        End If

                    End If

                Case "LaunchExeFile"
                    ' exe filename
                    m_lReturn = CheckFileExists(sFilename:=sKeyValue, bExists:=bExists)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to check if LaunchEXE file exists: " & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                                        "Step name: " & sKeyParent & Strings.Chr(13) & Strings.Chr(10) & _
                                        "Filename: " & sKeyValue, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                        Return result
                    End If

                    If Not bExists Then
                        m_lReturn = MessageBox.Show("LaunchEXE file does not exist: " & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                                    "Step name: " & sKeyParent & Strings.Chr(13) & Strings.Chr(10) & _
                                    "Filename: " & sKeyValue & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Ok to save the roadmap?", ACApp, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)

                        If m_lReturn <> System.Windows.Forms.DialogResult.OK Then
                            Return gPMConstants.PMEReturnCode.PMCancel
                        End If
                    End If

            End Select
        Next


        Return gPMConstants.PMEReturnCode.PMTrue



        result = gPMConstants.PMEReturnCode.PMError

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateRoadmap failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateRoadmap", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ValidateScript
    '
    ' Description: check script loads okay and check that specified
    '              start method exists
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
	<HandleProcessCorruptedStateExceptions>
    Private Function ValidateScript(ByVal sFilename As String, ByVal sStartMethod As String) As Integer

        Dim result As Integer = 0
        Dim lFH As Integer
        Dim bFound As Boolean
        Dim sCode As String = ""
        Dim oProc As MSScriptControl.IScriptProcedure
        Dim oProcs As MSScriptControl.IScriptProcedureCollection
        Dim oMSScriptControl As MSScriptControl.ScriptControl

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            oMSScriptControl = New MSScriptControl.ScriptControl()

            ' get the file
            lFH = FileSystem.FreeFile()

            FileSystem.FileOpen(lFH, sFilename, OpenMode.Input)

            sCode = FileSystem.InputString(lFH, FileSystem.LOF(lFH)).Trim()

            FileSystem.FileClose(lFH)

            ' is it empty?
            If sCode = "" Then
                Return result
            End If

            oMSScriptControl.Language = "VBScript"
            oMSScriptControl.AddCode(sCode)

            oProcs = oMSScriptControl.Procedures

            oMSScriptControl = Nothing

            ' check that the specified start method exists
            For Each oProc2 As MSScriptControl.IScriptProcedure In oProcs
                oProc = oProc2

                If Not oProc.HasReturnValue Then
                    If oProc.Name.ToUpper() = sStartMethod.ToUpper() Then
                        bFound = True
                        Exit For
                    End If
                End If
            Next oProc2

            oProc = Nothing
            oProcs = Nothing

            If Not bFound Then
                ' didn't find the start method specified in the key
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateScript failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateScript", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FindKeys
    '
    ' Description: get all the keys in the roadmap
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function FindKeys(ByRef vKeys(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' the top-level nodes
            For Each oChildNodeDOM As XmlNode In m_oXMLDOM.ChildNodes
                ' get child nodes
                m_lReturn = WalkDOMNodes(oNodeDOM:=oChildNodeDOM, vKeyArray:=vKeys)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

            Next oChildNodeDOM


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindKeys failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: WalkTheDinosaur
    '
    ' Description: check nodes and subnodes. Add key properties to array
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function WalkDOMNodes(ByVal oNodeDOM As XmlNode, ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Static lCount As Integer
        Dim oParent As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            For Each oChildDOM As XmlNode In oNodeDOM.ChildNodes

                ' only interested in KEY elements
                If oChildDOM.LocalName.ToUpper() = ELEMENT_BASENAME_KEY Then

                    ' add the details to the array
                    ReDim Preserve vKeyArray(2, lCount)
                    oParent = oChildDOM.ParentNode



                    vKeyArray(0, lCount) = oParent.GetAttribute("Description")

                    vKeyArray(1, lCount) = oChildDOM.Attributes("Name").ToString()

                    vKeyArray(2, lCount) = oChildDOM.Attributes("Value").ToString()

                    lCount += 1

                End If

                ' does this child have any children?
                m_lReturn = WalkDOMNodes(oNodeDOM:=oChildDOM, vKeyArray:=vKeyArray)

            Next oChildDOM


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            Select Case Information.Err().Number
                Case ERROR_LET_PROCEDURE_NOT_DEFINED
                    ' this error is expected for some elements
                    Return gPMConstants.PMEReturnCode.PMTrue
            End Select

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="WalkDOMNodes failed", vApp:=ACApp, vClass:=ACClass, vMethod:="WalkDOMNodes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ShowOptions
    '
    ' Description: Display the options dialogue
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function ShowOptions() As Integer

        Dim result As Integer = 0
        Dim oOptions As frmOptions

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            oOptions = New frmOptions()

            With oOptions
                m_lReturn = .Initialise()

                .Business = m_oBusiness

                m_lReturn = .Load_Renamed()

                m_lReturn = .ShowForm(lDisplayState:=FormShowConstants.Modal)

                m_lStatus = .Status
            End With

            oOptions.Close()

            oOptions = Nothing

            If m_lStatus <> gPMConstants.PMEReturnCode.PMOK Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowOptions failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowOptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateDocTemplateList
    '
    ' Description: maintains data for standard letter steps
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    ' update doc template details, or add of missing
    'UPGRADE_NOTE: (7001) The following declaration (UpdateDocTemplateList) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function UpdateDocTemplateList(ByVal sElementID As String, ByVal sTempCode As String, ByVal sTypeCode As String, ByVal sTempDesc As String) As Integer
    '
    'Dim result As Integer = 0
    'Dim bFound As Boolean
    'Dim lPos As Integer
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'If Information.IsArray(m_vDocTemplate) Then
    'For 'lPos = m_vDocTemplate.GetLowerBound(1) To m_vDocTemplate.GetUpperBound(1)
    'If CStr(m_vDocTemplate(DOC_TEMPLATE_ID, lPos)) = sElementID Then
    'bFound = True
    'Exit For
    'End If
    'Next 
    '
    'If Not bFound Then
    'lPos = m_vDocTemplate.GetUpperBound(1) + 1
    ''ReDim Preserve m_vDocTemplate(3, lPos)
    'End If
    '
    'm_vDocTemplate(DOC_TEMPLATE_ID, lPos) = sElementID
    'm_vDocTemplate(DOC_TEMPLATE_CODE, lPos) = sTempCode
    'm_vDocTemplate(DOC_TEMPLATE_TYPE, lPos) = sTypeCode
    'm_vDocTemplate(DOC_TEMPLATE_DESC, lPos) = sTempDesc
    'Else
    ''ReDim m_vDocTemplate(3, 0)
    'm_vDocTemplate(DOC_TEMPLATE_ID, 0) = sElementID.Trim()
    'm_vDocTemplate(DOC_TEMPLATE_CODE, 0) = sTempCode.Trim()
    'm_vDocTemplate(DOC_TEMPLATE_TYPE, 0) = sTypeCode.Trim()
    'm_vDocTemplate(DOC_TEMPLATE_DESC, 0) = sTempDesc.Trim()
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    'End If
    '
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateDocTemplateList failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDocTemplateList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetDocTempDetails
    '
    ' Description: locates document template data for given template ID
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function GetDocTempDetails(ByVal v_lDocTemIdID As String, ByRef r_sCode As String, ByRef r_sType As String, ByRef r_sDesc As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_vDocTempList = Nothing


            m_lReturn = m_oBusiness.GetDocumentTemplateDesc(v_lDocTemIdID, m_vDocTempList)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(m_vDocTempList) Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            r_sCode = CStr(m_vDocTempList(DOC_TEMPLATE_CODE, 0))
            r_sDesc = CStr(m_vDocTempList(DOC_TEMPLATE_DESC, 0))
            r_sType = CStr(m_vDocTempList(DOC_TEMPLATE_TYPE, 0))


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocTempDetails failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocTempDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: ShowOptions
    '
    ' Description: gets template info for given ElementID
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetTemplateInfo) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetTemplateInfo(ByVal sElementID As String, ByRef sCode As String, ByRef sType As String, ByRef sDesc As String) As Integer
    '
    'Dim result As Integer = 0
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'If Not Information.IsArray(m_vDocTemplate) Then
    'Return gPMConstants.PMEReturnCode.PMTrue
    'End If
    '
    'For 'lLoop As Integer = m_vDocTemplate.GetLowerBound(1) To m_vDocTemplate.GetUpperBound(1)
    'If CStr(m_vDocTemplate(DOC_TEMPLATE_ID, lLoop)) = sElementID Then
    'sCode = CStr(m_vDocTemplate(DOC_TEMPLATE_CODE, lLoop))
    'sType = CStr(m_vDocTemplate(DOC_TEMPLATE_TYPE, lLoop))
    'sDesc = CStr(m_vDocTemplate(DOC_TEMPLATE_DESC, lLoop))
    'Exit For
    'End If
    'Next 
    '
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTemplateInfo failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTemplateInfo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: SplitterMove
    '
    ' Description: Splitter sits between treeview and attribute
    '              display. Move the splitter when dragged, and resize
    '              the controls left and right of it.
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function SplitterMove(ByVal iButton As Integer, ByVal PosX As Single) As Integer

        Dim result As Integer = 0

        Dim NewX As Single

        ' used to calc difference between splitter bar's original and current position
        Static StartX As Single

        ' controls left and right of the splitter
        Dim oLeft As TreeView
        Dim oRight As GroupBox

        ' minimum width of left control
        Const POS_MINIMUM As Integer = 180
        Const VALUE_ZERO As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' check if left button pressed, else reset position
            If iButton <> MouseButtonConstants.LeftButton Then
                StartX = VALUE_ZERO
                Return result
            End If

            ' get x position of pointer relative to control
            If StartX = VALUE_ZERO Then
                StartX = PosX
            End If

            ' controls left and right of the splitter
            oLeft = tvwMap
            oRight = fmeAttrib

            ' new position of pointer
            NewX = VB6.PixelsToTwipsX(lblSplitter.Left) + PosX - StartX

            If NewX < POS_MINIMUM Then
                NewX = POS_MINIMUM
            End If

            ' can't be > width of container
            If NewX + VB6.PixelsToTwipsX(lblSplitter.Width) + POS_MINIMUM > VB6.PixelsToTwipsX(Me.ClientRectangle.Width) Then
                Return result
            End If

            ' move splitter bar
            lblSplitter.Left = VB6.TwipsToPixelsX(NewX)

            ' resize left and right controls
            oLeft.Width = lblSplitter.Left - VB6.TwipsToPixelsX(30)
            oRight.Left = lblSplitter.Left + lblSplitter.Width
            oRight.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(picBG.Width) - VB6.PixelsToTwipsX(oRight.Left) - 180)

            ' resize the property frames
            fmeKey.Width = oRight.Width
            fmeMap.Width = oRight.Width
            fmeSubmap.Width = oRight.Width
            fmeStep.Width = oRight.Width


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SplitterMove failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SplitterMove", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ToolbarDelete
    '
    ' Description: delete current element when toolbar delete clicked.
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Private Function ToolbarDelete() As Integer

        Dim result As Integer = 0
        Dim sCore, sName As String
        Dim oNode, oParent As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If m_oElementDOM Is Nothing Then
                ' nothing selected
                Return result
            End If

            sName = m_oElementDOM.LocalName

            ' not for map root
            If sName = ELEMENT_BASENAME_MAP Then
                Return result
            End If

            ' parent has the "Core" attribute if a key or submap
            If sName = ELEMENT_BASENAME_KEY Or sName = ELEMENT_BASENAME_SUBMAP Then
                oNode = m_oElementDOM.ParentNode
            Else
                oNode = m_oElementDOM
            End If


            sCore = CStr(oNode.GetAttribute("Core"))

            ' users cannot delete core elements
            If g_lUserMode = USER_MODE_USER And sCore = CStr(ROADMAP_CORE_ADMIN) Then
                Return result
            End If


            sName = CStr(oNode.GetAttribute("Description"))

            ' confirm delete
            m_lReturn = MessageBox.Show("Ok to delete element '" & sName & "' from the roadmap?", ACApp, MessageBoxButtons.OKCancel, MessageBoxIcon.Question)

            If m_lReturn <> System.Windows.Forms.DialogResult.OK Then
                Return result
            End If

            ' element deleted via parent
            oParent = oNode.ParentNode

            ' thank you and goodnight

            oNode = oParent.RemoveChild(oNode)

            ' zap the objects
            oNode = Nothing
            oParent = Nothing
            m_oElementDOM = Nothing

            ' map modified
            m_bDirty = True

            m_lReturn = BuildTreeView(tvwTreeView:=tvwMap)

            StatusMsg(sName & " deleted")


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ToolbarDelete failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ToolbarDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    'UPGRADE_NOTE: (7001) The following declaration (ToolbarAdd) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ToolbarAdd() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ToolbarAdd failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ToolbarAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: ToolsUpdateLog
    '
    ' Description: display update history i.e. those core roadmaps
    '              updated by installer
    '
    ' History: RDC 15082003 created
    ' ***************************************************************** '
    Private Function ToolsUpdateLog() As Integer

        Dim result As Integer = 0
        Dim lStatus As Integer
        Dim oForm As frmUpdateLog

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            oForm = New frmUpdateLog()

            With oForm
                m_lReturn = .Initialise()

                .Business = m_oBusiness

                m_lReturn = .Load_Renamed()

                m_lReturn = .ShowForm(lDisplayState:=FormShowConstants.Modal)

                lStatus = .Status
            End With

            oForm.Close()

            oForm = Nothing


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ToolsUpdateLog failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ToolsUpdateLog", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Get the values from Lookup tables
    ' ***************************************************************** '

    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As ComboBox) As Integer
        Dim result As Integer = 0
        Dim oLookUp As bPMLookup.Business
        Dim vLookupValues, vLookupDetails As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If sLookupTable = "" Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the lookup values.

            ctlLookup.Items.Clear()

            oLookUp = New bPMLookup.Business()
            m_lReturn = CType(oLookUp, SSP.S4I.Interfaces.IBusiness).Initialise(sUsername:=g_oObjectManager.UserName, sPassword:=g_oObjectManager.Password, iUserID:=g_oObjectManager.UserID, iSourceID:=g_oObjectManager.SourceID, iLanguageID:=g_oObjectManager.LanguageID, iCurrencyID:=g_oObjectManager.CurrencyID, iLogLevel:=g_oObjectManager.LogLevel, sCallingAppName:=ACClass)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oLookUp.Dispose()
                oLookUp = Nothing
                Return result
            End If

            ' Reset Result Array

            vLookupDetails = Nothing
            ' Reset Table Array

            vLookupValues = Nothing

            ReDim vLookupValues(3, 0)

            vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = sLookupTable

            vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""


            m_lReturn = oLookUp.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=vLookupValues, iLanguageID:=g_iLanguageID, dtEffectiveDate:=DateTime.Now, vResultArray:=vLookupDetails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oLookUp.Dispose()
                oLookUp = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")

                Return result
            End If

            If Not Information.IsArray(vLookupDetails) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            For iRow As Integer = vLookupDetails.GetLowerBound(1) To vLookupDetails.GetUpperBound(1)

                ctlLookup.Items.Add(New VB6.ListBoxItem(vLookupDetails(1, iRow)))

            Next iRow


            oLookUp.Dispose()
            oLookUp = Nothing

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAllEffectiveUsers
    '
    ' Description: Get all effective users.
    ' ***************************************************************** '
    Private Function GetAllEffectiveUsers() As Integer
        Dim result As Integer = 0
        Dim oUser As bPMUserGroup.Lookup
        Dim vUserDetails As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oUser = New bPMUserGroup.Lookup()
            m_lReturn = CType(oUser, SSP.S4I.Interfaces.IBusiness).Initialise(sUsername:=g_oObjectManager.UserName, sPassword:=g_oObjectManager.Password, iUserID:=g_oObjectManager.UserID, iSourceID:=g_oObjectManager.SourceID, iLanguageID:=g_oObjectManager.LanguageID, iCurrencyID:=g_oObjectManager.CurrencyID, iLogLevel:=g_oObjectManager.LogLevel, sCallingAppName:=ACClass)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oUser.Dispose()
                oUser = Nothing
                Return result
            End If


            vUserDetails = Nothing

            m_lReturn = oUser.GetAllEffectiveUsers(v_dtEffectiveDate:=DateTime.Now, r_vAllUsersArray:=vUserDetails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oUser.Dispose()
                oUser = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllEffectiveUsers")

                Return result
            End If

            If Not Information.IsArray(vUserDetails) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            For iRow As Integer = vUserDetails.GetLowerBound(1) To vUserDetails.GetUpperBound(1)
                Dim cboDiaryUserId_NewIndex As Integer = -1

                cboDiaryUserId_NewIndex = cboDiaryUserId.Items.Add(CStr(vUserDetails(1, iRow)))

                VB6.SetItemData(cboDiaryUserId, cboDiaryUserId_NewIndex, gPMFunctions.ToSafeLong(CStr(vUserDetails(0, iRow))))
            Next iRow


            oUser.Dispose()
            oUser = Nothing

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all users", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllEffectiveUsers", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
