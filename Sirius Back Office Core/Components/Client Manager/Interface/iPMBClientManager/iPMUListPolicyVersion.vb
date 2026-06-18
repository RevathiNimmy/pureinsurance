Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmListPolicyVersion
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmListPolicyVersion
	'
	' Date: 23/06/1998
	'
	' Description: Main interface.
	'
	' Edit History: TF031298 - Menu & Toolbar activity
	' CJB 230305 PN19733 - New InsuranceFileRef public property procedure to support
	'            showing the pol. no. in the title bar (in Form_Activate).
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmListPolicyVersion"
    'developer guide no. 7
    Private Const vbFormCode As Integer = 0
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	' PRIVATE Data Members (Begin)
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	
	Private m_iTask As gPMConstants.PMEComponentAction
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	' {* USER DEFINED CODE (Begin) *}
	Private m_iIndex As Integer
	
	Private m_lInsuranceFileCnt As Integer
	Private m_lInsuranceFolderCnt As Integer
	Private m_sInsuranceFileRef As String = "" 'PN19733
	Private m_lPartyCnt As Integer
	Private m_sShortName As String = ""
	Private m_sPartyType As String = ""
	Private m_sFooter As String = ""
	' {* USER DEFINED CODE (End) *}
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	' Stores the details from the business object.
	
	' {* USER DEFINED CODE (Begin) *}
	Private Const vbScrollBars As String = "0x80000000"
	Private Const vbDesktop As String = "0x80000001"
	Private Const vbActiveTitleBar As String = "0x80000002"
	Private Const vbInactiveTitleBar As String = "0x80000003"
	Private Const vbMenuBar As String = "0x80000004"
	Private Const vbWindowBackground As String = "H00FFFFC0"
	Private Const vbWindowFrame As String = "0x80000006"
	Private Const vbMenuText As String = "0x80000007"
	Private Const vbWindowText As String = "0x80000008"
	Private Const vbTitleBarText As String = "0x80000009"
	Private Const vbActiveBorder As String = "0x8000000A"
	Private Const vbInactiveBorder As String = "0x8000000B"
	Private Const vbApplicationWorkspace As String = "0x8000000C"
	Private Const vbHighlight As String = "0x8000000D"
	Private Const vbHighlightText As String = "0x8000000E"
	Private Const vbButtonFace As String = "0x8000000F"
	Private Const vbButtonShadow As String = "0x80000010"
	Private Const vbGrayText As String = "0x80000011"
	Private Const vbButtonText As String = "0x80000012"
	Private Const vbInactiveCaptionText As String = "0x80000013"
	Private Const vb3DHighlight As String = "0x80000014"
	Private Const vb3DDKShadow As String = "0x80000015"
	Private Const vb3DLight As String = "0x80000016"
	Private Const vbInfoText As String = "0x80000017"
	Private Const vbInfoBackground As String = "0x80000018"
	
	'Are we closing this to edit, or just closing
	Private m_bEditing As Boolean
	
	' {* USER DEFINED CODE (End) *}
	' PRIVATE Data Members (End)
	
    Private objCM As MainModule
    Public WriteOnly Property ModuleClass() As MainModule
        Set(ByVal value As MainModule)
            objCM = value
        End Set
    End Property
	' PUBLIC Property Procedures (Begin)
	
	Public ReadOnly Property ErrorNumber() As Integer
		Get
			
			' Standard Property.
			
			' Return any error number that might have
			' occurred on the interface.
			Return m_lErrorNumber
			
		End Get
	End Property
	
	Public WriteOnly Property CallingAppName() As String
		Set(ByVal Value As String)
			
			' Standard Property.
			
			' Set the calling application name.
			m_sCallingAppName = Value
			
		End Set
	End Property
	
	
	' {* USER DEFINED CODE (End) *}
	
	' PUBLIC Property Procedures (End)
	
	
	' PRIVATE Property Procedures (Begin)
	
	Public Property Status() As Integer
		Get
			
			' Standard Property.
			
			' Return the interface exit status.
			Return m_lStatus
			
		End Get
		Set(ByVal Value As Integer)
			
			' Standard Property.
			
			' Set the interface exit status.
			m_lStatus = Value
			
		End Set
	End Property
	
	Public Property Task() As Integer
		Get
			
			' Return the objects task.
			Return m_iTask
			
		End Get
		Set(ByVal Value As Integer)
			
			' Standard Property.
			
			' Set the objects task.
			m_iTask = Value
			
		End Set
	End Property
	Public Property Index() As Integer
		Get
			
			' Return the objects task.
			Return m_iIndex
			
		End Get
		Set(ByVal Value As Integer)
			
			' Standard Property.
			
			' Set the objects task.
			m_iIndex = Value
			
		End Set
	End Property
	
	Public WriteOnly Property Navigate() As Integer
		Set(ByVal Value As Integer)
			
			' Standard Property.
			
			' Set the navigate flag.
			m_lNavigate = Value
			
		End Set
	End Property
	
	Public WriteOnly Property ProcessMode() As Integer
		Set(ByVal Value As Integer)
			
			' Standard Property.
			
			' Set the process mode.
			m_lProcessMode = Value
			
		End Set
	End Property
	
	Public WriteOnly Property TransactionType() As String
		Set(ByVal Value As String)
			
			' Standard Property.
			
			' Set the type of business.
			m_sTransactionType = Value
			
		End Set
	End Property
	
	Public WriteOnly Property EffectiveDate() As Date
		Set(ByVal Value As Date)
			
			' Standard Property.
			
			' Set the effective date.
			m_dtEffectiveDate = Value
			
		End Set
	End Property
	
	' {* USER DEFINED CODE (Begin) *}
	Public WriteOnly Property InsuranceFileCnt() As Integer
		Set(ByVal Value As Integer)
			m_lInsuranceFileCnt = Value
		End Set
	End Property
	
	Public WriteOnly Property InsuranceFolderCnt() As Integer
		Set(ByVal Value As Integer)
			m_lInsuranceFolderCnt = Value
		End Set
	End Property
	
	Public Property PartyCnt() As Integer
		Get
			Return m_lPartyCnt
		End Get
		Set(ByVal Value As Integer)
			m_lPartyCnt = Value
		End Set
	End Property
	
	Public Property ShortName() As String
		Get
			Return m_sShortName
		End Get
		Set(ByVal Value As String)
			m_sShortName = Value
		End Set
	End Property
	
	Public Property InsuranceFileRef() As String
		Get ' PN19733
			Return m_sInsuranceFileRef
		End Get
		Set(ByVal Value As String) 'PN19733
			m_sInsuranceFileRef = Value
		End Set
	End Property
	
	Public WriteOnly Property PartyType() As String
		Set(ByVal Value As String)
			m_sPartyType = Value
		End Set
	End Property
	
	Public Property Footer() As String
		Get
			Return m_sFooter
		End Get
		Set(ByVal Value As String)
			m_sFooter = Value
		End Set
	End Property
	' PRIVATE Property Procedures (End)
	
	
	' PUBLIC Methods (Begin)
	
	Public Function LoadInterface() As gPMConstants.PMEReturnCode
		
		
		Return gPMConstants.PMEReturnCode.PMTrue
		
		
		
		' Error Section
		
		' Log Error.
		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Exit Function
		
	End Function
	
	Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
		' Fire up the help screen
        '        m_lReturn& = ShowHelp(dlgHelp,objCM. ScreenHelpID)
		' Click event of the Cancel button.
		
		Try 
			
			' Process the next set of actions depending
			' upon the interface task etc.
			m_lReturn = uctPMUListPolicy1.ShowHelpScreen()
			
			' Check the return value.
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Help command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdHelp_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		Dim iLen As Integer
		
		' Click event of the OK button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMOK
			
			m_lReturn = uctPMUListPolicy1.OKClick()
			
			' Check the return value.
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				' Everything OK, so we can hide the interface.
                Me.Close()
				
			End If
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		' Click event of the Cancel button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			' Process the next set of actions depending
			' upon the interface task etc.
			m_lReturn = uctPMUListPolicy1.CancelClick()
			
			' Check the return value.
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				' Everything OK, so we can hide the interface.
				Me.Close()
			End If
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' PRIVATE Events (End)
	
	Private Sub frmListPolicyVersion_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			
            objCM.m_ofrmMDI.StatusBar1.Items.Item(0).Text = Me.Footer
            objCM.m_ofrmMDI.StatusBar1.Items.Item(1).Text = CStr(Me.PartyCnt)
            objCM.m_ofrmMDI.StatusBar1.Items.Item(2).Text = Me.ShortName
            objCM.m_ofrmMDI.Text = "[" & Me.ShortName.Trim() & "] Sirius Client Manager"
            Me.Text = "Policy List Version : [" & Me.InsuranceFileRef.Trim() & "]" ' PN19733

            m_lReturn = CType(objCM.SetToolbar(v_sFormName:=Me.Name), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Problem Setting Toolbar keys", Application.ProductName)
            End If

        End If
    End Sub

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error Section
        '
        'm_lErrorNumber = gPMConstants.PMEReturnCode.PMError
        '
        ' Log Error.
        'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub


    Private Sub frmListPolicyVersion_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim lStatus As Integer
        Dim sTemp As String = ""

        '
        '
        '        ' Forms load event.
        '
        Try

            m_bEditing = False

            If Me.PartyCnt <> 0 Then
                m_iTask = gPMConstants.PMEComponentAction.PMEdit
            Else
                m_iTask = gPMConstants.PMEComponentAction.PMAdd
            End If

            Me.Height = VB6.TwipsToPixelsY(6285)
            Me.Width = VB6.TwipsToPixelsX(9045)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            With uctPMUListPolicy1
                .Task = m_iTask
                .Status = gPMConstants.PMEReturnCode.PMTrue
                .TransactionType = ""
                .EffectiveDate = DateTime.Today
                .ProcessMode = 0
            End With

            m_lReturn = CType(uctPMUListPolicy1, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise user control.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub

            End If

            uctPMUListPolicy1.InsFileCnt = m_lInsuranceFileCnt
            uctPMUListPolicy1.InsuranceFolderCnt = m_lInsuranceFolderCnt
            uctPMUListPolicy1.ShortName = m_sShortName

            m_lReturn = uctPMUListPolicy1.LoadControl()

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the user control.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub

            End If

            m_lReturn = uctPMUListPolicy1.GetPolicies()

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get business details.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub

            End If

            lStatus = uctPMUListPolicy1.Status

            ' Get the number of recent files
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="MaxMRU", r_sSettingValue:=sTemp), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to read registry settings.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                ' Default to 4
                sTemp = "4"
            End If

            ' Convert the result back to an integer
            objCM.g_iMaxRecent = CInt(sTemp)

            ' Load the menus
            For iLoop1 As Integer = 2 To objCM.g_iMaxRecent
                ContainerHelper.LoadControl(Me, "mnuRecentFile", iLoop1)
                With mnuRecentFile(iLoop1)
                    .Text = "RecentFile" & iLoop1
                    .Available = False
                End With
            Next iLoop1

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmListPolicyVersion_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Dim iCount As Integer

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = uctPMUListPolicy1.CancelClick()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Cancel = 1
                    'developer guide no. 7
                    eventArgs.Cancel = True
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If

            End If

            'Flag the document as closed with the MDI Form
            objCM.FState(Index).Deleted = True

            'Don't reset these if editing, as the other form is already activated and
            'so won't be updating them...
            If Not m_bEditing Then

                objCM.m_ofrmMDI.StatusBar1.Items.Item(0).Text = ""
                objCM.m_ofrmMDI.StatusBar1.Items.Item(1).Text = ""
                objCM.m_ofrmMDI.StatusBar1.Items.Item(2).Text = ""
                objCM.m_ofrmMDI.Text = "Sirius Client Manager"

            End If

            iCount = 0

            For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                If Application.OpenForms.Item(iLoop1).Name <> Me.Name Then
                    If Application.OpenForms.Item(iLoop1).Name <> objCM.m_ofrmMDI.Name Then
                        iCount += 1
                    End If
                End If
            Next iLoop1

            If iCount = 0 Then
                ' Update

                m_lReturn = objCM.g_oCMManager.ImEmpty(v_lPartyCnt:=m_lPartyCnt)
                m_lReturn = CType(objCM.SetToolbar(v_sFormName:=objCM.m_ofrmMDI.Name), gPMConstants.PMEReturnCode)
            End If

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub
	
	Private isInitializingComponent As Boolean
	Private Sub frmListPolicyVersion_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		If isInitializingComponent Then
			Exit Sub
		End If
		
		If Me.WindowState <> FormWindowState.Minimized Then
			Me.Height = VB6.TwipsToPixelsY(6285)
			Me.Width = VB6.TwipsToPixelsX(9045)
		End If
		
	End Sub
	
	Private Sub frmListPolicyVersion_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		
		' Show the current form instance as deleted
        objCM.FState(CInt(Convert.ToString(Me.Tag))).Deleted = True
		
		' Hide the toolbar edit buttons if no notepad windows exist.
        If Not objCM.AnyPadsLeft() Then

            objCM.gToolsHidden = True
            ' Call the recent file list procedure
            objCM.GetRecentFiles()
        End If
		
	End Sub
	
	Public Sub mnuClientExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuClientExit.Click
		'CT 31/08/00 bugfix 379
		cmdCancel.Focus()
		cmdCancel_Click(cmdCancel, New EventArgs())
	End Sub
	
	Public Sub mnuHelpAbout_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuHelpAbout.Click
		
        m_lReturn = CType(objCM.ShowSBOAbout(), gPMConstants.PMEReturnCode)
		
	End Sub
	
	Public Sub mnuRecentFile_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _mnuRecentFile_0.Click, _mnuRecentFile_1.Click
		Dim Index As Integer = Array.IndexOf(mnuRecentFile, eventSender)
		
        m_lReturn = CType(objCM.ShowRecentFile(iIndex:=Index, r_oForm:=Me), gPMConstants.PMEReturnCode)
		
	End Sub
	
	' ***************************************************************** '
	' Name: Refresh
	'
	' Description: Refreshes the data on the form
	'
	' ***************************************************************** '
	Public Function RefreshList() As Integer
		
		Dim result As Integer = 0
		Try 
			
			' Call GetPolicies on the user control
			
			Return uctPMUListPolicy1.GetPolicies()
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RefreshList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SwitchTo
	'
	' Description: Switches focus to this form.
	'
	' ***************************************************************** '
	Public Function SwitchTo() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Set the focus
			Me.Activate()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SwitchTo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SwitchTo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
    Private Sub uctPMUListPolicy1_lvwSearchDetailsDblClick(ByVal Sender As Object, ByVal e As PMUPolicyVersion.uctPMUListPolicy.lvwSearchDetailsDblClickEventArgs) Handles uctPMUListPolicy1.lvwSearchDetailsDblClick

        Try

            'm_lReturn = ShowPolicySummary(v_lPartyCnt:=m_lInsHolderCnt, v_sPartyType:=m_sPartyType, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsFileCnt:=m_lInsFileCnt, v_sShortName:=m_sShortName.Trim(), v_sInsReference:=m_sInsReference.Trim(), v_lPolicyTypeId:=m_lPolicyTypeID)
            m_lReturn = objCM.ShowPolicySummary(v_lPartyCnt:=e.m_lInsHolderCnt, v_sPartyType:=m_sPartyType, v_lInsuranceFolderCnt:=e.m_lInsuranceFolderCnt, v_lInsFileCnt:=e.m_lInsFileCnt, v_sShortName:=e.m_sShortName.Trim(), v_sInsReference:=e.m_sInsReference.Trim(), v_lPolicyTypeId:=e.m_lPolicyTypeID)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub
End Class
