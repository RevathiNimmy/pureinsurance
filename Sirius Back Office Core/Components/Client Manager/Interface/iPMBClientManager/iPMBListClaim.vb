Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmListClaim
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmListClaim
	'
	' Date: 07/11/00
	'
	' Description: Main interface.
	'
	' Edit History: DC071100 - Menu & Toolbar activity
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
    Private Const ACClass As String = "frmListClaim"
    'Developer Guide No. 7
    Private Const vbFormCode As Integer = 0
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	' PRIVATE Data Members (Begin)
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As gPMConstants.PMEReturnCode
	Private m_lErrorNumber As Integer
	
	Private m_iTask As gPMConstants.PMEComponentAction
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	' {* USER DEFINED CODE (Begin) *}
	Private m_iIndex As Integer
	' {* USER DEFINED CODE (End) *}
	Private m_oInterface As Object
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Variables to store the lookup values/details.
	Private m_vLookupValues As Object
	Private m_vLookupDetails As Object
	
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

    Private m_sFooter As String = ""
    Private m_sPartyType As String = ""
    Private m_lPartyCnt As Integer
    Private m_sShortName As String = ""
    Private m_sMainPostCode As String = ""
    Private m_sSurName As String = ""
    Private m_iPartyTitleID As Integer
    Private m_sForeName As String = ""
    Private m_sInitials As String = ""
    Private m_iOccupationID As Integer
    Private m_dtDOB As Date
    Private m_lAgentCnt As Object
    Private m_sAgentRef As String = ""
    Private m_sAgentName As String = ""
    Private m_lEmployerCnt As Object
    Private m_sEmployerRef As String = ""
    Private m_vAddresses As Object
    Private m_vAddressTypes As Object
    Private m_vContacts As Object
    Private m_sAddressLine1 As String = ""
    Private m_sInsReference As String = ""
    'Extras for PMB
    Private m_vPersons As Object
    Private m_vPersonTypes As Object
    Private m_vPersonSex As Object
    Private m_sAssociateRef As String = ""
    Private m_sConsultantRef As String = ""
    Private m_sConsultantName As String = ""

    'Flags to indicate whether we need to check the employer/agent ids match
    'the employer/agent ref as user may change the reference directly
    Private m_bVerifyAgentCnt As Boolean
    Private m_bVerifyEmployerCnt As Boolean

    'Are we closing this to edit, or just closing
    Private m_bEditing As Boolean

    'Note the index in the lookup array of the main address
    Private m_iMainAddressIndex As Integer

    ' Declare an instance of the address interface.
    Private m_oAddress As Object

    ' Declare an instance of the contact interface.
    Private m_oContact As Object
    Private m_iClaimID As Integer
    Private m_iInsurancefileCnt, m_iInsuranceFolderCnt As Integer
    Private m_sClaimNo As String

    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)


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
	
	'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub Status(ByVal Value As Integer)
		'
		' Standard Property.
		'
		' Set the interface exit status.
		'm_lStatus = Value
		'
	'End Sub
	Public ReadOnly Property Status() As Integer
		Get
			
			' Standard Property.
			
			' Return the interface exit status.
			Return m_lStatus
			
		End Get
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
	
	Public Property PartyCnt() As Integer
		Get
			
			Return m_lPartyCnt
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lPartyCnt = Value
			
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
	Public Property ShortName() As String
		Get
			
			Return m_sShortName
			
		End Get
		Set(ByVal Value As String)
			
			m_sShortName = Value
			
		End Set
	End Property
	Public Property PartyType() As String
		Get
			
			Return m_sPartyType
			
		End Get
		Set(ByVal Value As String)
			
			m_sPartyType = Value
			
		End Set
	End Property
	Public Property InsReference() As String
		Get
			
			Return m_sInsReference
			
		End Get
		Set(ByVal Value As String)
			
			m_sInsReference = Value
			
		End Set
    End Property
    Public Property ClaimID() As Integer
        Get

            Return m_iClaimID

        End Get
        Set(ByVal Value As Integer)

            m_iClaimID = Value

        End Set
    End Property
    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_iInsurancefileCnt
        End Get
        Set(ByVal Value As Integer)
            m_iInsurancefileCnt = Value
        End Set
    End Property

    Public Property InsuranceFolderCnt() As Integer
        Get
            Return m_iInsuranceFolderCnt
        End Get
        Set(ByVal Value As Integer)
            m_iInsuranceFolderCnt = Value
        End Set
    End Property
    Public Property ClaimReference() As String
        Get

            Return m_sClientRef

        End Get
        Set(ByVal Value As String)

            m_sClientRef = Value

        End Set
    End Property


	' PRIVATE Property Procedures (End)
    Private objCM As MainModule
    Public WriteOnly Property ModuleClass() As MainModule
        Set(ByVal value As MainModule)
            objCM = value
        End Set
    End Property
	
	' PUBLIC Methods (Begin)
	
	Public Function LoadInterface() As gPMConstants.PMEReturnCode
		
		
		Return gPMConstants.PMEReturnCode.PMTrue
		
		
		
		' Error Section
		
		' Log Error.
		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Exit Function
		
	End Function
	
	' ***************************************************************** '
	' Name: DisableForm
	'
	' Description: Sets all of the interface details to the disable
	'              state passed.
	'
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (DisableForm) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function DisableForm(ByRef lDisabled As Integer) As Integer
		'
		'Dim result As Integer = 0
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			' Set all of the forms controls to the disable state.

			'For	Each ctlFormControl As Control In ContainerHelper.Controls(frmPartyPC)
				' Check the type of the control.
				'If TypeOf ctlFormControl Is TextBox Then
					'ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
				'ElseIf (TypeOf ctlFormControl Is ComboBox) Then 
					'ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
				'ElseIf (TypeOf ctlFormControl Is CheckBox) Then 
					'ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
					'ElseIf (TypeOf ctlFormControl Is SSOption) Then
					'    ctlFormControl.Enabled = Not lDisabled&
				'End If
			'Next ctlFormControl
			'
			'Return result
		'
		'Catch excep As System.Exception
			'
			'
			'
			' Error Section.
			'
			'result = gPMConstants.PMEReturnCode.PMError
			'
			' Log Error.
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the form disabling", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result
			'
		'End Try
	'End Function
	
	'UPGRADE_NOTE: (7001) The following declaration (cmdHelp_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub cmdHelp_Click()
		'
		' Click event of the Cancel button.
		'
		''UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
		'Try 
			'
			'    ' Process the next set of actions depending
			'    ' upon the interface task etc.
			'    m_lReturn& = uctListClaim1.ShowHelpScreen
			''
			'    ' Check the return value.
		'
		'Catch excep As System.Exception
			'
			'
			'
			' Error Section.
			'
			' Log Error.
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Help command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdHelp_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Exit Sub
			'
		'End Try
		'
	'End Sub
	
	' ***************************************************************** '
	' Name: ActionDisplayClaimsVersion
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : Date : Process ID
	' ***************************************************************** '
	Public Function ActionDisplayClaimsVersion(ByVal v_bCloseIfOk As Boolean) As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "ActionDisplayClaimsVersion"
		
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim lSubValue As Integer
		Dim iLen As Integer
		Dim lClaimID, lInsuranceFileCnt As Integer
		Dim sClaimNumber, sInsuranceRef As String
		Dim lRiskCnt As Integer
		Dim sClientShortname As String = ""
		Dim dtLossFromDate As Date
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		
		' get the selected claims version
		lReturn = uctCLMVersions.GetSelectedClaimsDetails(lClaimID, lInsuranceFileCnt, sClaimNumber, sInsuranceRef, lRiskCnt, sClientShortname, dtLossFromDate)
		
		' if the details have been successfully retrieved
		If lClaimID <> 0 Then
			
			' display the claim in view mode
            lReturn = CType(objCM.ShowClaim(sClaimNumber, lInsuranceFileCnt, lClaimID, lRiskCnt, dtLossFromDate, sClientShortname, sInsuranceRef), gPMConstants.PMEReturnCode)
			
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				gPMFunctions.RaiseError(kMethodName, "ShowClaim Failed", gPMConstants.PMELogLevel.PMLogError)
			Else
				If v_bCloseIfOk Then
					
					' Set the interface status.
					m_lStatus = gPMConstants.PMEReturnCode.PMOK
					
					Me.Close()
					
				End If
				
			End If
			
		Else
			MessageBox.Show("You must select a claim to continue", "Claim Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
		End If
		
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sUsername:=objCM.g_sUserName, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

		Finally
'        Return result
'        Resume
'        Return result
		End Try
		Return result
    End Function

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Const kMethodName As String = "cmdOK_Click"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lSubValue As Integer

        Try



        lReturn = CType(ActionDisplayClaimsVersion(v_bCloseIfOk:=True), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "ActionDisplayClaimsVersion Failed", gPMConstants.PMELogLevel.PMLogError)
        End If



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sUsername:=objCM.g_sUserName, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally

       
        End Try
    End Sub
	
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		' Click event of the Cancel button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			Me.Close()
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' PRIVATE Events (End)
	
	Private Sub frmListClaim_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			
            objCM.m_ofrmMDI.StatusBar1.Items.Item(0).Text = Me.Footer
            objCM.m_ofrmMDI.StatusBar1.Items.Item(1).Text = CStr(Me.PartyCnt)
            objCM.m_ofrmMDI.StatusBar1.Items.Item(2).Text = Me.ShortName
            objCM.m_ofrmMDI.Text = "[" & Me.ShortName.Trim() & "] Sirius Client Manager"
            Me.Text = "Claim List : [" & Me.ShortName.Trim() & "]"

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

    ' ***************************************************************** '
    ' Name: Form_Load
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 13-03-2006 : Claims Versioning Changes
    ' ***************************************************************** '

    Public Sub frmListClaim_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Const kMethodName As String = "Form_Load"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lSubValue As Integer
        Dim sTemp As String = ""

        Try



        m_bEditing = False

        If Me.PartyCnt <> 0 Then
            m_iTask = gPMConstants.PMEComponentAction.PMEdit
        Else
            m_iTask = gPMConstants.PMEComponentAction.PMAdd
        End If

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        ' initialse claim version user control
        'modified guide no. 97
        'lReturn = CType(uctCLMVersions, SSP.S4I.Interfaces.ILocalInterface).Initialise()
        lReturn = uctCLMVersions.Initialise()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "uctCLMVersions.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' set the processing modes for claim version user control
        lReturn = uctCLMVersions.SetProcessModes(vTask:=m_iTask, vProcessMode:=0, vTransactionType:="", vEffectiveDate:=DateTime.Today)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "uctCLMVersions.SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' set required lookup details
        uctCLMVersions.ShortName = m_sShortName
        uctCLMVersions.InsuranceRef = m_sInsReference
        uctCLMVersions.PartyCnt = m_lPartyCnt

        ' load the claim version user control
        'modified
        '  lReturn = uctCLMVersions.Load()
        lReturn = uctCLMVersions.Load_Renamed()

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "uctCLMVersions.Load Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        ' Get the number of recent files
        lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="MaxMRU", r_sSettingValue:=sTemp), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetPMRegSetting Failed", gPMConstants.PMELogLevel.PMLogError)
            ' Default to 4
            sTemp = "4"
        End If

        ' Convert the result back to an integer
        objCM.g_iMaxRecent = CInt(sTemp)

        ' Use objCM.LoadRecentFiles
        lReturn = CType(objCM.LoadRecentFiles(r_oForm:=Me), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "objCM.LoadRecentFiles Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CInt(""), excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally
     
        End Try
    End Sub

    'Private Sub Form_Load()
    '
    'Dim lStatus As Long
    'Dim sTemp As String
    'Dim iLoop1 As Integer
    '
    ''
    ''
    ''        ' Forms load event.
    ''
    '    On Error GoTo Err_FormLoad
    '
    '    m_bEditing = False
    '
    '    If (Me.PartyCnt <> 0) Then
    '        m_iTask = PMEdit
    '    Else
    '        m_iTask = PMAdd
    '    End If
    '
    '    'Me.Height = 6100
    '    'Me.Width = 9435
    '
    '    SetMousePointer PMMouseBusy
    '
    '    lReturn = uctCLMVersions.Initialise
    '
    '    lReturn = uctCLMVersions.SetProcessModes(vTask:=m_lTask, _
    ''                         vProcessMode:=0, _
    ''                         vTransactionType:="", _
    ''                         vEffectiveDate:=Date)
    '
    '    uctCLMVersions.ShortName = m_sShortName
    '    uctCLMVersions.InsuranceRef = m_sInsReference
    '
    '
    '    lReturn = uctCLMVersions.Load
    '
    ''    With uctListClaim1
    ''        .Task = m_iTask
    ''        .Status = PMTrue
    ''        .TransactionType = ""
    ''        .EffectiveDate = Date
    ''        .ProcessMode = 0
    ''    End With
    ''
    ''    m_lReturn = uctListClaim1.Initialise
    ''    If m_lReturn = PMFalse Then
    ''        SetMousePointer PMMouseNormal
    ''        ' Log Error.
    ''        LogMessage _
    '''            iType:=PMLogOnError, _
    '''            sMsg:="Failed to initialise user control.", _
    '''            vApp:=ACApp, _
    '''            vClass:=ACClass, _
    '''            vMethod:="Form_Load", _
    '''            vErrNo:=Err.Number, _
    '''            vErrDesc:=Err.Description
    ''        Exit Sub
    ''    End If
    ''
    ''    uctListClaim1.ShortName = m_sShortName$
    ''    uctListClaim1.InsReference = m_sInsReference$
    ''
    ''    m_lReturn = uctListClaim1.LoadControl
    ''    If m_lReturn = PMFalse Then
    ''        SetMousePointer PMMouseNormal
    ''        ' Log Error.
    ''        LogMessage _
    '''            iType:=PMLogOnError, _
    '''            sMsg:="Failed to load the user control.", _
    '''            vApp:=ACApp, _
    '''            vClass:=ACClass, _
    '''            vMethod:="Form_Load", _
    '''            vErrNo:=Err.Number, _
    '''            vErrDesc:=Err.Description
    ''        Exit Sub
    ''    End If
    ''
    ''    m_lReturn = uctListClaim1.GetClaims
    ''    If m_lReturn = PMFalse Then
    ''        SetMousePointer PMMouseNormal
    ''        ' Log Error.
    ''        LogMessage _
    '''            iType:=PMLogOnError, _
    '''            sMsg:="Failed to get business details.", _
    '''            vApp:=ACApp, _
    '''            vClass:=ACClass, _
    '''            vMethod:="Form_Load", _
    '''            vErrNo:=Err.Number, _
    '''            vErrDesc:=Err.Description
    ''        Exit Sub
    ''    End If
    ''
    ''    lStatus = uctListClaim1.Status
    '
    '    SetMousePointer PMMouseNormal
    '
    '    ' Get the number of recent files
    '    m_lReturn& = GetPMRegSetting(v_lPMERegSettingRoot:=pmeRSRLocalMachine, _
    ''                                 v_lPMEProductFamily:=pmePFSiriusSolutions, _
    ''                                 v_lPMERegSettingLevel:=pmeRSLClient, _
    ''                                 v_sSettingName:="MaxMRU", _
    ''                                 r_sSettingValue:=sTemp$)
    '    If (m_lReturn& <> PMTrue) Then
    '        ' Log Error.
    '        LogMessage _
    ''            iType:=PMLogOnError, _
    ''            sMsg:="Failed to read registry settings.", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="Form_Load", _
    ''            vErrNo:=Err.Number, _
    ''            vErrDesc:=Err.Description
    '
    '        ' Default to 4
    '        sTemp$ = "4"
    '    End If
    '
    '    ' Convert the result back to an integer
    '    g_iMaxRecent% = CInt(sTemp$)
    '
    '    ' Load the menus
    ''    For iLoop1% = 2 To g_iMaxRecent%
    ''        Load mnuRecentFile(iLoop1%)
    ''        With mnuRecentFile(iLoop1%)
    ''            .Caption = "RecentFile" & CStr(iLoop1%)
    ''            .Visible = False
    ''        End With
    ''    Next iLoop1%
    '
    '    ' CTAF 170801 - Use objCM.LoadRecentFiles
    '    m_lReturn& = objCM.LoadRecentFiles(r_oForm:=Me)
    '
    '    Exit Sub
    '
    'Err_FormLoad:
    '
    '    ' Error Section.
    '    m_lErrorNumber& = PMError
    '
    '    ' Log Error.
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Failed to load the form", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="Form_Load", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Sub
    '
    'End Sub

    Private Sub frmListClaim_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
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
            Dim m_sOpenedFormName As String = String.Empty

            For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                If Application.OpenForms.Item(iLoop1).Name <> Me.Name Then
                    If Application.OpenForms.Item(iLoop1).Name <> objCM.m_ofrmMDI.Name Then
                        m_sOpenedFormName = Application.OpenForms.Item(iLoop1).Name
                        iCount += 1
                    End If
                End If
            Next iLoop1

            If iCount = 0 Or (iCount = 1 And m_sOpenedFormName = "frmMain") Then
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
    Private Sub frmListClaim_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If


        Try

            If Me.WindowState <> FormWindowState.Minimized Then

                If VB6.PixelsToTwipsY(Me.Height) < 6285 Then
                    Me.Height = VB6.TwipsToPixelsY(6285)
                End If

                If VB6.PixelsToTwipsX(Me.Width) < 9045 Then
                    Me.Width = VB6.TwipsToPixelsX(9045)
                End If

                uctCLMVersions.Width = Me.Width - VB6.TwipsToPixelsX(300)
                uctCLMVersions.Height = Me.Height - VB6.TwipsToPixelsY(1000)

                cmdOK.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(uctCLMVersions.Top) + VB6.PixelsToTwipsY(uctCLMVersions.Height) + 20)
                cmdCancel.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(uctCLMVersions.Top) + VB6.PixelsToTwipsY(uctCLMVersions.Height) + 20)

                cmdCancel.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.Width) - VB6.PixelsToTwipsX(cmdCancel.Width) - 250)
                cmdOK.Left = cmdCancel.Left - VB6.TwipsToPixelsX(105) - cmdOK.Width
            End If

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Sub

    Private Sub frmListClaim_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        ' Show the current form instance as deleted
        objCM.FState(CInt(Convert.ToString(Me.Tag))).Deleted = True

        ' Hide the toolbar edit buttons if no notepad windows exist.
        If Not objCM.AnyPadsLeft() Then

            objCM.gToolsHidden = True
            ' Call the recent file list procedure
            'GetRecentFiles
            ' CTAF 170801 - Use objCM.LoadRecentFiles
            m_lReturn = CType(objCM.LoadRecentFiles(r_oForm:=Me), gPMConstants.PMEReturnCode)

        End If

    End Sub
	
	Public Sub mnuClientExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuClientExit.Click
		'CT 31/08/00 bugfix 379
		cmdCancel.Focus()
		cmdCancel_Click(cmdCancel, New EventArgs())
	End Sub
	
	Public Sub mnuDocumentationLetterWriting_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuDocumentationLetterWriting.Click
		
		Dim lClaimID, lInsuranceFileCnt, lInsuranceFolderCnt As Integer
		
		m_lReturn = uctCLMVersions.GetSelectedClaimsDetails(r_lClaimID:=lClaimID, r_lInsuranceFileCnt:=lInsuranceFileCnt, r_lInsuranceFolderCnt:=lInsuranceFolderCnt)
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			MessageBox.Show("Failed to get selected claim id", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
			Exit Sub
		End If
		
		If lClaimID = 0 Then
			MessageBox.Show("Please select claim from the list", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
			Exit Sub
		End If
		
        m_lReturn = CType(objCM.ProcessToolbar(v_iButton:=objCM.ACIButtonLetter, v_lPartyCnt:=m_lPartyCnt, v_lInsuranceFileCnt:=lInsuranceFileCnt, v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_lClaimCnt:=lClaimID), gPMConstants.PMEReturnCode)
		
	End Sub
	
	Public Sub mnuHelpAbout_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuHelpAbout.Click
		
        m_lReturn = CType(objCM.ShowSBOAbout(), gPMConstants.PMEReturnCode)
		
	End Sub
	
	Public Sub mnuRecentFile_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _mnuRecentFile_0.Click, _mnuRecentFile_1.Click, _mnuRecentFile_2.Click, _mnuRecentFile_3.Click, _mnuRecentFile_4.Click, _mnuRecentFile_5.Click
		Dim Index As Integer = Array.IndexOf(mnuRecentFile, eventSender)
		
        m_lReturn = CType(objcm.ShowRecentFile(iIndex:=Index, r_oForm:=Me), gPMConstants.PMEReturnCode)
		
	End Sub
	
	' ***************************************************************** '
	' Name: RefreshList
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 13-03-2006 : Claims Versioning Changes
	' ***************************************************************** '
	Public Function RefreshList() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "RefreshList"
		
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' Call refresh on the claim versions user control
		lReturn = uctCLMVersions.Refresh()
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "uctCLMVersions.Refresh Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
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
	
	'UPGRADE_NOTE: (7001) The following declaration (uctListClaim1_lvwSearchDetailsDblClick) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub uctListClaim1_lvwSearchDetailsDblClick(ByRef m_sClaimNumber As String, ByRef m_lPolicyId As Integer, ByRef m_lClaimId As Integer, ByRef m_lRiskTypeId As Integer, ByRef m_dtClaimDate As Date, ByRef m_sClientName As String)
		'
		'Try 
			'
    'm_lReturn = CType(objCM.ShowClaim(v_sClaimNumber:=m_sClaimNumber, v_lPolicyID:=m_lPolicyId, v_lClaimId:=m_lClaimId, v_lRiskTypeId:=m_lRiskTypeId, v_dtClaimDate:=m_dtClaimDate, v_sClientName:=m_sClientName, v_sInsReference:=m_sInsReference), gPMConstants.PMEReturnCode)
			'
			'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'Continue as not serious
				'Exit Sub
			'End If
		'
		'Catch 
			'
			'
			'
			'Continue as not serious
			'Exit Sub
		'End Try
		'
		'
	'End Sub
	
	Private Sub uctCLMVersions_DblClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles uctCLMVersions.DblClick
        ActionDisplayClaimsVersion(v_bCloseIfOk:=False)

	End Sub

    Public Function GetSelectedClaimsDetails(ByRef iClaimID As Integer, ByRef iInsuranceFileCnt As Integer, ByRef iInsuranceFolderCnt As Integer, ByRef sInsReference As String, ByRef sClaimNo As String) As Integer

        Dim result As Integer = 0
        Dim iReturn As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the selected claims version
            iReturn = uctCLMVersions.GetSelectedClaimsDetails(iClaimID, iInsuranceFileCnt)
            ClaimID = iClaimID
            InsuranceFileCnt = iInsuranceFileCnt
            ClaimReference = sClaimNo
            InsReference = sInsReference
            InsuranceFolderCnt = iInsuranceFolderCnt
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSelectedClaimsDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSelectedClaimsDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
