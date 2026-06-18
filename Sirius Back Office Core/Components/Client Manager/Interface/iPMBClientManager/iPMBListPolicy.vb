Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Imports uctListPolicyControl
Imports uctPMUPolicyExpCtl

Partial Friend Class frmListPolicy
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmListPolicy
    '
    ' Date: 23/06/1998
    '
    ' Description: Main interface.
    '
    ' Edit History: TF031298 - Menu & Toolbar activity
    '               SD27082002- Different control for broking vs underwriting
    '               CJB230305 - PN19733 Changed uctListPolicy1_lvwSearchDetailsDblClick to
    '                           pass extra pol. no. param to ShowPolicyListVersion to support
    '                           showing it in policy version list title bar.
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmListPolicy"
    'Developer Guide No. 7
    Private Const vbFormCode As Integer = 3
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
    ' {* USER DEFINED CODE (End) *}
    Private m_oInterface As Object

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Variables to store the lookup values/details.
    Private m_vLookupValues As Object
    Private m_vLookupDetails As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

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

    'JSB 08/06/01 - Flag to idenitify if we wat to call GII
    Private m_bDontCallGII As Boolean

    Private m_bFromEvent As Boolean
    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)

    Private m_lRiskCodeId As Integer
    Private m_lRiskGroupId As Integer
    Private m_lRiskGisScreenId As Integer
    Private m_lRiskTypeId As Integer

    Private m_bFormActivate As Boolean

    Private objCM As MainModule
    Public WriteOnly Property ModuleClass() As MainModule
        Set(ByVal value As MainModule)
            objCM = value
        End Set
    End Property

    Public Property RiskCodeId() As Integer
        Get

            Return m_lRiskCodeId

        End Get
        Set(ByVal Value As Integer)

            m_lRiskCodeId = Value

        End Set
    End Property

    Public Property RiskGroupId() As Integer
        Get

            Return m_lRiskGroupId

        End Get
        Set(ByVal Value As Integer)

            m_lRiskGroupId = Value

        End Set
    End Property
    Public Property RiskTypeId() As Integer
        Get

            Return m_lRiskTypeId

        End Get
        Set(ByVal Value As Integer)

            m_lRiskTypeId = Value

        End Set
    End Property
    Public Property ScreenId() As Integer
        Get

            Return m_lRiskGisScreenId

        End Get
        Set(ByVal Value As Integer)

            m_lRiskGisScreenId = Value

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

    Public Property FromEvent() As Boolean
        Get

            m_bFromEvent = m_bFromEvent

        End Get
        Set(ByVal Value As Boolean)

            m_bFromEvent = Value

        End Set
    End Property
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    Public Function LoadInterface() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        result = gPMConstants.PMEReturnCode.PMTrue
        '2005 Client Manager Security
        If Not objCM.g_bEditPolicyAuthority Then
            cmdAdd.Enabled = False
            cmdCopy.Enabled = False
        End If

        Return result



        ' Error Section

        ' Log Error.
        gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", excep:=New Exception(Information.Err().Description))

        Return result

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

    'Private Sub cmdEdit_Click()
    '
    '    'open another document
    '
    '    m_lReturn = OpenFile(vPartyCnt:=m_lPartyCnt, _
    ''                         vPartyShortName:=m_sShortName, _
    ''                         vPartyType:=m_sPartyType, _
    ''                         vPartyResolvedName:=m_sResolved)
    '
    '    m_bEditing = True
    '
    '    If (m_lReturn = PMTrue) Then
    '        Unload Me
    '    End If
    '
    'End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        'ECK 18/06/99
        Try

            'gp20020311 - Re-set RiskCnt as this is a new policy
            objcm.m_ofrmMDI.RiskCnt = 0
            objCM.g_bCallGII = True 'PN26814

            ' Call Toolbar Control function


            m_lReturn = CInt(objCM.ShowPolicyDetail(v_lPartyCnt:=m_lPartyCnt, v_sPartyType:=m_sPartyType, v_lInsuranceFolderCnt:=0, v_lInsFileCnt:=0, v_lInsuranceFileStructureId:=0, v_sShortName:=m_sShortName, v_sInsReference:="", v_bFromEvent:=False, v_lPolicyTypeId:=0, v_vGeminiPolicyStatus:=DBNull.Value))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

            'DC080404 PN11508 unload listpolicy to match when editting apolicy
            Me.Close()

        Catch


            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    ' ***************************************************************** '
    '
    ' Name: ShowOtherClientPolicy
    '
    ' Description: Loads up another clients policy in a new client manager
    '
    ' History: 15/08/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function ShowOtherClientPolicy(ByVal v_lPartyCnt As Integer, ByVal v_sShortName As String, ByVal v_sResolvedName As String, ByVal v_sPartyType As String, ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the party_cnt

            objCM.g_oCMManager.PartyCnt = v_lPartyCnt

            objCM.g_oCMManager.InsuranceFileCnt = v_lInsuranceFileCnt

            objCM.g_oCMManager.PartyShortName = v_sShortName

            objCM.g_oCMManager.PartyResolvedName = v_sResolvedName

            objCM.g_oCMManager.PartyType = v_sPartyType

            objCM.g_oCMManager.FromCopy = True

            ' Start the component up

            m_lReturn = objCM.g_oCMManager.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lPartyCnt", v_lPartyCnt)
            oDict.Add("v_lInsuranceFileCnt", v_lInsuranceFileCnt)
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowOtherClientPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowOtherClientPolicy", excep:=excep, oDicParms:=oDict)

            Return result




            Return result
        End Try
    End Function



    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        'm_lReturn = ShowHelp(cmdHelp,objCM. ScreenHelpID)
        ' Click event of the Cancel button.

        Try

            ' Process the next set of actions depending
            ' upon the interface task etc.

            'SD 27/08/2002 Customise form for broking or underwriting

            m_lReturn = uctPMUPolicyExplorer1.ShowHelpScreen(cmdHelp)


            ' Check the return value.

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            Dim sErrUniqueId As String = ""
            sErrUniqueId = GenerateUniqueSSPExceptionRef(gPMConstants.ERROR_NO_LENGTH)
			gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Help command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdHelp_Click", excep:=excep, sErrUniqueId:=sErrUniqueId)
            Exit Sub

        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'SD 27/08/2002 Customise form for broking or underwriting

            m_lReturn = uctPMUPolicyExplorer1.OKClick()


            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Close()

                If IsArray(objCM.g_vWarnings) = True Then
                    For m_iIndex = 0 To UBound(objCM.g_vWarnings, 2)
                        objCM.Document(objCM.g_vWarnings(0, m_iIndex)).Close()
                    Next m_iIndex
                    objCM.g_vWarnings = ""
                End If

                objCM.OpenWarnings(m_lPartyCnt)

            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", excep:=excep)

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

            'SD 27/08/2002 Customise form for broking or underwriting

            m_lReturn = uctPMUPolicyExplorer1.CancelClick()


            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Close()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' PRIVATE Events (End)

    Private Sub frmListPolicy_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender


            objcm.m_ofrmMDI.StatusBar1.Items.Item(0).Text = Me.Footer
            objcm.m_ofrmMDI.StatusBar1.Items.Item(1).Text = CStr(Me.PartyCnt)
            objcm.m_ofrmMDI.StatusBar1.Items.Item(2).Text = Me.ShortName
            objcm.m_ofrmMDI.Text = "[" & Me.ShortName.Trim() & "] Sirius Client Manager"
            Me.Text = "Policy List : [" & Me.ShortName.Trim() & "]"


            'sj 03/07/2002 - start
            If objCM.g_bRestrictInsurerAccess Then
                m_lReturn = objCM.SetRestrictedToolbar(v_sFormName:=Me.Name)
            Else
                m_lReturn = objCM.SetToolbar(v_sFormName:=Me.Name)
            End If
            'sj 03/07/2002 - end
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Problem Setting Toolbar keys", Application.ProductName)
            End If


            'SD 27/08/2002 Customise form for broking or underwriting

            uctListPolicy1.Visible = False
            uctPMUPolicyExplorer1.Visible = True
            If Not m_bFormActivate Then
                uctPMUPolicyExplorer1.Focus()
            End If


            'JSB 08/06/01 - set flag to true
            m_bDontCallGII = True

            m_bFormActivate = True

        End If
    End Sub


    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try

            m_bFormActivate = False
            WindowState = FormWindowState.Maximized
        Catch excep As System.Exception



            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", excep:=excep)

            Exit Sub

        End Try


    End Sub


    Private Sub frmListPolicy_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        'Me.Height = Me.Height - 60
        'Me.Width = Me.Width - 40

        Dim lStatus As Integer
        Dim sTemp As String = ""
        'eck 090903 PN6647

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

            'Me.Height = 6100
            'Me.Width = 9435

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Thinh Nguyen 15/04/2002 (start) - hide Add/Quote/Copy for underwriting

            cmdAdd.Visible = False
            cmdGII.Visible = False
            cmdCopy.Visible = False

            ' Russell G 22/08/2002
            ' Disable menu options too.
            mnuPolicyAdd.Enabled = False
            mnuPolicyCopy.Enabled = False


            'Thinh Nguyen 15/04/2002 (end) - hide Add/Quote/Copy for underwriting

            'SD 27/08/2002 START Customise form for broking or underwriting

            With uctPMUPolicyExplorer1
                .Task = m_iTask
                .Status = gPMConstants.PMEReturnCode.PMTrue
                .TransactionType = ""
                .EffectiveDate = DateTime.Today
                .ProcessMode = 0
                '.PartyCnt = m_lPartyCnt
            End With
            m_lReturn = uctPMUPolicyExplorer1.Initialise()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise user control.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", excep:=New Exception(Information.Err().Description))
                Exit Sub
            End If

            uctPMUPolicyExplorer1.ShortName = m_sShortName
            uctPMUPolicyExplorer1.InsHolderCnt = m_lPartyCnt

            m_lReturn = uctPMUPolicyExplorer1.LoadControl()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the user control.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", excep:=New Exception(Information.Err().Description))
                Exit Sub
            End If

            m_lReturn = uctPMUPolicyExplorer1.GetPolicies()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get business details.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", excep:=New Exception(Information.Err().Description))
                Exit Sub
            End If

            lStatus = uctPMUPolicyExplorer1.Status


            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Get the number of recent files
            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="MaxMRU", r_sSettingValue:=sTemp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to read registry settings.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", excep:=New Exception(Information.Err().Description))

                ' Default to 4
                sTemp = "4"
            End If

            ' Convert the result back to an integer
            objCM.g_iMaxRecent = CInt(sTemp)

            ' Load the menus
            '    For iLoop1% = 2 To g_iMaxRecent%
            '        Load mnuRecentFile(iLoop1%)
            '        With mnuRecentFile(iLoop1%)
            '            .Caption = "RecentFile" & CStr(iLoop1%)
            '            .Visible = False
            '        End With
            '    Next iLoop1%

            ' CTAF 170801 - Use objCM.LoadRecentFiles
            m_lReturn = objCM.LoadRecentFiles(r_oForm:=Me)

            'sj 03/07/2002 - start
            If objCM.g_bRestrictInsurerAccess Then
                cmdAdd.Visible = False
                cmdGII.Visible = False
                cmdCopy.Visible = False
                mnuPolicyAdd.Available = False
                mnuClientDelete.Available = False
                mnuPolicyCopy.Available = False
                mnuClientMove.Available = False
            End If
            'sj 03/07/2002 - end

        Catch excep As System.Exception



            ' Error Section.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", excep:=excep)

            Exit Sub

        End Try



    End Sub

    Private Sub frmListPolicy_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
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


                'SD 27/08/2002 Customise form for broking or underwriting
                m_lReturn = uctPMUPolicyExplorer1.CancelClick()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Cancel = 1
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

                objcm.m_ofrmMDI.StatusBar1.Items.Item(0).Text = ""
                objcm.m_ofrmMDI.StatusBar1.Items.Item(1).Text = ""
                objcm.m_ofrmMDI.StatusBar1.Items.Item(2).Text = ""
                objcm.m_ofrmMDI.Text = "Sirius Client Manager"

            End If

            iCount = 0

            For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                If Application.OpenForms.Item(iLoop1).Name <> Me.Name Then
                    If Application.OpenForms.Item(iLoop1).Name <> objcm.m_ofrmMDI.Name Then
                        iCount += 1
                    End If
                End If
            Next iLoop1

            If iCount = 0 Then
                ' Update

                m_lReturn = objCM.g_oCMManager.ImEmpty(v_lPartyCnt:=m_lPartyCnt)
                'sj 03/07/2002 - start
                If objCM.g_bRestrictInsurerAccess Then
                    m_lReturn = objCM.SetRestrictedToolbar(v_sFormName:=objcm.m_ofrmMDI.Name)
                Else
                    m_lReturn = objCM.SetToolbar(v_sFormName:=objcm.m_ofrmMDI.Name)
                End If
                'sj 03/07/2002 - end
            End If

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmListPolicy_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        If Me.WindowState <> FormWindowState.Minimized Then
            'move controls on form to match with new form size (keep same proportion as original form)
            uctPMUPolicyExplorer1.Width = Me.Width - VB6.TwipsToPixelsY(510)
            uctPMUPolicyExplorer1.Height = Me.Height - VB6.TwipsToPixelsY(1100)
            'uctPMUPolicyExplorer1.Width = Me.Width - 100
            'uctPMUPolicyExplorer1.Height = Me.Height - 300
            '                Me.cmdOK.Top = Me.Height - 900
            '                Me.cmdOK.Left = Me.Width - 3715
            'Me.cmdOK.Top = Me.Height - VB6.TwipsToPixelsY(850)
            'Me.cmdOK.Left = Me.Width - VB6.TwipsToPixelsX(2505)
            'Me.cmdCancel.Top = Me.Height - VB6.TwipsToPixelsY(850)
            'Me.cmdCancel.Left = Me.Width - VB6.TwipsToPixelsX(1305)
        End If

    End Sub

    Private Sub frmListPolicy_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        ' Show the current form instance as deleted
        objCM.FState(CInt(Convert.ToString(Me.Tag))).Deleted = True

        ' Hide the toolbar edit buttons if no notepad windows exist.
        If Not objCM.AnyPadsLeft() Then

            objCM.gToolsHidden = True
            ' Call the recent file list procedure
            'GetRecentFiles
            ' CTAF 170801 - Use objCM.LoadRecentFiles
            m_lReturn = objCM.LoadRecentFiles(r_oForm:=Me)

        End If

    End Sub

    Public Sub mnuClientExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuClientExit.Click
        'CT 31/08/00 bugfix 379
        cmdCancel.Focus()
        cmdCancel_Click(cmdCancel, New EventArgs())
    End Sub


    Public Sub mnuHelpAbout_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuHelpAbout.Click

        m_lReturn = objCM.ShowSBOAbout()

    End Sub

    Public Sub mnuPolicyAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPolicyAdd.Click
        'JSB 05/06/01
        'Set flag so we know to add a GII policy if the user select a GII process type
        objCM.g_bCallGII = True
        ' Call Toolbar Control function

        If objCM.ShowPolicyDetail(v_lPartyCnt:=m_lPartyCnt, v_sPartyType:=m_sPartyType, v_lInsuranceFolderCnt:=0, v_lInsFileCnt:=0, v_lInsuranceFileStructureId:=0, v_sShortName:=m_sShortName, v_sInsReference:="", v_bFromEvent:=False, v_lPolicyTypeId:=0, v_vGeminiPolicyStatus:=DBNull.Value) <> gPMConstants.PMEReturnCode.PMTrue Then
            'inform user that failed to add policy
            MessageBox.Show("Failed to added policy", "Client Manager", MessageBoxButtons.OK, MessageBoxIcon.Information)
            'Continue as not serious
            Exit Sub
        End If

    End Sub


    Public Sub mnuRecentFile_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _mnuRecentFile_0.Click, _mnuRecentFile_1.Click, _mnuRecentFile_2.Click, _mnuRecentFile_3.Click, _mnuRecentFile_4.Click, _mnuRecentFile_5.Click
        Dim Index As Integer = Array.IndexOf(mnuRecentFile, eventSender)

        m_lReturn = objcm.ShowRecentFile(iIndex:=Index, r_oForm:=Me)

    End Sub

    ' ***************************************************************** '
    ' Name: RefreshList
    '
    ' Description: Refreshes the data on the form
    '
    ' ***************************************************************** '
    Public Function RefreshList() As Integer
        Dim result As Integer = 0
        Try

            ' Call GetPolicies on the user control

            'SD 27/08/2002 Customise form for broking or underwriting


            Return uctPMUPolicyExplorer1.GetPolicies()

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RefreshList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshList", excep:=excep)

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
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SwitchTo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SwitchTo", excep:=excep)

            Return result

        End Try
    End Function

    'sj 30/08/2002 - start
    'Only enable the quotes button for Gemini Policies
    Private Sub uctListPolicy1_lvwSearchDetailsMouseDown(ByVal Sender As Object, ByVal e As uctListPolicy.lvwSearchDetailsMouseDownEventArgs) Handles uctListPolicy1.lvwSearchDetailsMouseDown
        cmdGII.Enabled = uctListPolicy1.m_vSearchData(22, e.m_lSelected - 1) = "Motor" Or uctListPolicy1.m_vSearchData(22, e.m_lSelected - 1) = "Household" Or uctListPolicy1.m_vSearchData(22, e.m_lSelected - 1) = "Commercial Vehicle"

        mnuPolicyCopy.Enabled = True
        cmdCopy.Enabled = True

    End Sub
    'sj 30/08/2002 - end



    Private Sub uctPMUPolicyExplorer1_lvwRisksClick(ByVal Sender As Object, ByVal e As uctPMUPolicyExplorer.lvwRisksClickEventArgs) Handles uctPMUPolicyExplorer1.lvwRisksClick
        m_lRiskCodeId = e.v_lRiskID
    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (uctPMUPolicyExplorer1_lvwRisksDblClick) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Uncommented the dead code.
    Private Sub uctPMUPolicyExplorer1_lvwRisksDblClick(ByVal Sender As Object, ByVal e As uctPMUPolicyExplorer.lvwRisksDblClickEventArgs) Handles uctPMUPolicyExplorer1.lvwRisksDblClick

        Try
            If WindowState = FormWindowState.Maximized Then

                Dim maximizedScreenSize As Size = New Size(Width - 25, Height - 25)

                WindowState = FormWindowState.Normal
                Size = maximizedScreenSize
            End If

            m_lReturn = objCM.ShowRiskUnderwriting(v_lInsuranceFileCnt:=e.v_lInsFileCnt, v_lRiskCodeId:=e.v_lRiskID, v_lRiskGisScreenId:=e.v_lRiskGisScreen, v_lRiskTypeId:=e.v_lRiskTypeId, v_bFromEvent:=Me.FromEvent, v_lIsReInsuranceAtRiskLevel:=e.v_lIsReInsuranceAtRiskLevel, v_lInsuranceFolderCnt:=e.v_lInsuranceFolderCnt, nPartyCnt:=m_lPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                Exit Sub
            End If
            '
        Catch

            Exit Sub
        End Try
        '
        '
        ''
    End Sub

    Private Sub uctPMUPolicyExplorer1_lvwVersionsDblClick(ByVal Sender As Object, ByVal e As uctPMUPolicyExplorer.lvwVersionsDblClickEventArgs) Handles uctPMUPolicyExplorer1.lvwVersionsDblClick

        Try
            If WindowState = FormWindowState.Maximized Then

                Dim maximizedScreenSize As Size = New Size(Width - 15, Height - 28)

                WindowState = FormWindowState.Normal
                Size = maximizedScreenSize
            End If

            m_lReturn = CInt(objcm.ShowPolicySummary(v_lPartyCnt:=e.m_lInsHolderCnt, v_sPartyType:=m_sPartyType, v_lInsuranceFolderCnt:=e.m_lInsuranceFolderCnt, v_lInsFileCnt:=e.m_lInsFileCnt, v_sShortName:=m_sShortName, v_sInsReference:=e.m_sInsReference, v_lPolicyTypeId:=e.m_lPolicyTypeID, v_lRiskCnt:=m_lRiskCodeId))


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    Private Sub uctListPolicy1_lvwSearchDetailsDblClick(ByVal Sender As Object, ByVal e As uctListPolicy.lvwSearchDetailsDblClickEventArgs) Handles uctListPolicy1.lvwSearchDetailsDblClick

        'ECK 15/06/99
        Try

            'TN20010227 - Start
            If e.m_lPolicyTypeId = PMBConst.PMBPolicyTypeUnderwriting Then
                'sj 30/08/2002 - start
                If uctListPolicy1.ItemsFound > 1 Then
                    m_lReturn = objCM.ShowPolicyListVersion(v_lInsuranceFolderCnt:=e.m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=e.m_lInsFileCnt, v_lPartyCnt:=e.m_lInsHolderCnt, v_sPartyType:=m_sPartyType, v_sShortName:=m_sShortName, v_sInsReference:=e.m_sInsReference) 'PN19733
                Else

                    m_lReturn = CInt(objcm.ShowPolicySummary(v_lPartyCnt:=e.m_lInsHolderCnt, v_sPartyType:=m_sPartyType, v_lInsuranceFolderCnt:=e.m_lInsuranceFolderCnt, v_lInsFileCnt:=e.m_lInsFileCnt, v_sShortName:=m_sShortName, v_sInsReference:=e.m_sInsReference, v_lPolicyTypeId:=e.m_lPolicyTypeId))
                End If

                '        m_lReturn& = objcm.ShowPolicyListVersion(v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, _
                ''                                                            v_lInsuranceFileCnt:=m_lInsFileCnt, _
                ''                                                            v_lPartyCnt:=m_lInsHolderCnt, _
                ''                                                            v_sPartyType:=m_sPartyType, _
                ''                                                            v_sShortName:=m_sShortName)
                '
                'sj 30/08/2002 - end
                ' PSA 22092000

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Continue as not serious
                    Exit Sub
                End If
                'JSB 08/06/01 - Added call to launch GII roadmap here, to avoid adding a new event to uctListPolicy
                '               control, which would have broke compatibility, and as I don't know where this control
                '               is used, it was simpler to do it like. Not the tidest way to do it, but what else is new
            Else
                If m_bDontCallGII Then
                    ' Call Toolbar Control function
                    'sj 30/08/2002 - start
                    'Show policy version for Broking
                    If uctListPolicy1.ItemsFound > 1 Then
                        m_lReturn = objCM.ShowPolicyListVersion(v_lInsuranceFolderCnt:=e.m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=e.m_lInsFileCnt, v_lPartyCnt:=e.m_lInsHolderCnt, v_sPartyType:=m_sPartyType, v_sShortName:=m_sShortName, v_sInsReference:=e.m_sInsReference) 'PN19733
                    Else

                        m_lReturn = CInt(objcm.ShowPolicySummary(v_lPartyCnt:=e.m_lInsHolderCnt, v_sPartyType:=m_sPartyType, v_lInsuranceFolderCnt:=e.m_lInsuranceFolderCnt, v_lInsFileCnt:=e.m_lInsFileCnt, v_sShortName:=m_sShortName, v_sInsReference:=e.m_sInsReference, v_lPolicyTypeId:=e.m_lPolicyTypeId))
                    End If

                    '            If (ShowPolicySummary(v_lPartyCnt:=m_lInsHolderCnt, _
                    ''                                  v_sPartyType:=m_sPartyType, _
                    ''                                  v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, _
                    ''                                  v_lInsFileCnt:=m_lInsFileCnt&, _
                    ''                                  v_sShortName:=m_sShortName$, _
                    ''                                  v_sInsReference:=m_sInsReference, _
                    ''                                  v_lPolicyTypeId:=m_lPolicyTypeID) <> PMTrue) Then
                    '                'Continue as not serious
                    '                Exit Sub
                    '            End If
                    'sj 30/08/2002 - end

                Else
                    'call GII
                    If objCM.CallGeminiII(v_lPartyCnt:=e.m_lInsHolderCnt, v_sPartyType:=m_sPartyType, v_lInsuranceFolderCnt:=e.m_lInsuranceFolderCnt, v_lInsFileCnt:=e.m_lInsFileCnt, v_sShortName:=m_sShortName, v_bFromEvent:=False, v_lPolicyTypeId:=e.m_lPolicyTypeId, v_lGIIPolicyStatus:=CInt(uctListPolicy1.GeminiPolicyStatus), v_vGIIPolicyNumber:=uctListPolicy1.InsReference) <> gPMConstants.PMEReturnCode.PMTrue Then

                        MessageBox.Show("Falied to Call Gemini II Process", "Client Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    End If

                End If
            End If

            e.m_lInsFileCnt = 0 'pkh 06/09/2002 - Reset variable otherwise the policy
            'doesn't show in the policy list

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    Private Sub frmListPolicy_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            DirectCast(uctListPolicy1.Controls("tabMainTab"), TabControl).SelectedIndex = 0
        End If
    End Sub
End Class
