Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'Developer Guide no. 129    
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 17/02/1997
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lPMAuthorityLevel As String = ""
    Private m_lStatus As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lPartyCnt As Integer
    Private m_sShortName As String = ""
    Private m_sLongName As String = ""
    Private m_lAddressCnt As Integer

    Private m_iNotEditable As Integer
    Private m_vSourceArray As Object
    Private m_vSourceArrayIncludeClosedBranch As Object

    Private m_bViewAuthority As Boolean '2005 Client Manager Security
    Private m_bEditAuthority As Boolean '2005 Client Manager Security
    Private m_bDeleteAuthority As Boolean '2005 Client Manager Security

    Private m_vPartyType As Object
    Private m_vPartyStatus As Object
    Private m_vNavStep As Object
    ' {* USER DEFINED CODE (End) *}

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Member to hold instance of form interface
    Private m_ofrmInterface As iPMUFindRIParty.frmInterface
    Private m_ofrmBrokerInterface As iPMUFindRIParty.frmRIBrokerParticipants

    Private m_bDeleteMode As Boolean

    Private m_bIncludeClosedBranches As Boolean
    Private m_bIsIncludeClosedBrancheChecked As Boolean
    Private m_cUpperLimit As Decimal
    Private m_cLowerLimit As Decimal
    Private m_lGroupingId As Integer
    Private bIsFAx As Boolean
    Private m_dRetained_percent As Double
    Private m_dParticipation_percent As Double
    Private m_dComm_percent As Double
    Private m_cSumInsured As Decimal
    Private m_cTotalSumInsured As Decimal
    Private m_cPremium As Decimal
    Private m_cPremiumTax As Decimal
    Private m_cCommission As Decimal
    Private m_cCommTax As Decimal
    Private m_lRiskId As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_lRiArrangement_id As Integer
    'Developer Guide No.101
    Private m_vExistingLimits As Object
    Private m_lRiArrangementLineId As Integer
    Private vParticipantArray() As Object
    Private vBrokerArray(,) As Object
    Private m_lClaimId As Integer
    Private m_bFACPropExists As Boolean
    Private m_sLineAddMode As String = ""
    Private m_vAddedFindRIPartyLines As Object 'PN44646
    Private m_sAgreementCode As String = "" 'Sankar - PN 50348
    Private m_bAddParticipantsFromTreaty As Boolean
    Private m_vTreatyPartiesBrokerParticipantForDisplay(,) As Object

    Private m_cGrossPremium As Decimal

    Public Property AddParticipantsFromTreaty() As Boolean
        Get
            Return m_bAddParticipantsFromTreaty
        End Get
        Set(ByVal Value As Boolean)
            m_bAddParticipantsFromTreaty = Value
        End Set
    End Property

    Public Property TreatyPartiesBrokerParticipantsForDisplay() As Object
        Get
            Return m_vTreatyPartiesBrokerParticipantForDisplay
        End Get
        Set(ByVal Value As Object)
            m_vTreatyPartiesBrokerParticipantForDisplay = Value
        End Set
    End Property
    Public Property AgreementCode() As String
        Get
            Return m_sAgreementCode
        End Get
        Set(ByVal Value As String)
            m_sAgreementCode = Value
        End Set
    End Property
    'PN44646
    'PN44646
    Public Property AddedFindRIPartyLines() As Object
        Get
            Return m_vAddedFindRIPartyLines
        End Get
        Set(ByVal Value As Object)


            m_vAddedFindRIPartyLines = Value
        End Set
    End Property
    'Claim

    Public Property ClaimId() As Integer
        Get
            Return m_lClaimId
        End Get
        Set(ByVal Value As Integer)
            m_lClaimId = Value
        End Set
    End Property

    'Developer Guide no.101
    Public Property ExistingLimits() As Object
        Get
            Return m_vExistingLimits
        End Get
        Set(ByVal Value As Object)

            m_vExistingLimits = Value
        End Set
    End Property



    Public Property IncludeClosedBranches() As Boolean
        Get
            Return m_bIncludeClosedBranches
        End Get
        Set(ByVal Value As Boolean)
            m_bIncludeClosedBranches = Value
        End Set
    End Property


    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = CStr(Value)
        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
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

    Public Property ShortName() As String
        Get

            Return m_sShortName

        End Get
        Set(ByVal Value As String)

            m_sShortName = Value

        End Set
    End Property

    Public ReadOnly Property LongName() As String
        Get

            Return m_sLongName

        End Get
    End Property
    Public ReadOnly Property Task() As Integer
        Get
            ' Return the task.
            Return m_iTask
        End Get
    End Property


    Public Property UpperLimit() As Decimal
        Get
            Return m_cUpperLimit
        End Get
        Set(ByVal Value As Decimal)
            m_cUpperLimit = Value
        End Set
    End Property


    Public Property LowerLimit() As Decimal
        Get
            Return m_cLowerLimit
        End Get
        Set(ByVal Value As Decimal)
            m_cLowerLimit = Value
        End Set
    End Property

    Public Property GroupingId() As Integer
        Get
            Return m_lGroupingId
        End Get
        Set(ByVal Value As Integer)
            m_lGroupingId = Value
        End Set
    End Property


    Public Property IsFAX() As Boolean
        Get
            Return bIsFAx
        End Get
        Set(ByVal Value As Boolean)
            bIsFAx = Value
        End Set
    End Property


    Public Property NotEditable() As Integer
        Get

            Return m_iNotEditable

        End Get
        Set(ByVal Value As Integer)
            m_iNotEditable = Value
        End Set
    End Property


    Public Property RiskId() As Integer
        Get
            Return m_lRiskId
        End Get
        Set(ByVal Value As Integer)
            m_lRiskId = Value
        End Set
    End Property
    Public Property TotalSumInsured() As Decimal
        Get
            Return m_cTotalSumInsured
        End Get
        Set(ByVal Value As Decimal)
            m_cTotalSumInsured = Value
        End Set
    End Property


    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public WriteOnly Property Ri_Arrangement_id() As Integer
        Set(ByVal Value As Integer)
            m_lRiArrangement_id = Value
        End Set
    End Property

    Public ReadOnly Property Retained_Percent() As Double
        Get
            Return m_dRetained_percent
        End Get
    End Property

    Public ReadOnly Property SumInsured() As Decimal
        Get
            Return m_cSumInsured
        End Get
    End Property

    Public ReadOnly Property CommTax() As Decimal
        Get
            Return m_cCommTax
        End Get
    End Property

    Public ReadOnly Property Commission() As Decimal
        Get
            Return m_cCommission
        End Get
    End Property

    Public ReadOnly Property Premium() As Decimal
        Get
            Return m_cPremium
        End Get
    End Property

    Public ReadOnly Property PremiumTax() As Decimal
        Get
            Return m_cPremiumTax
        End Get
    End Property

    Public ReadOnly Property Comm_percent() As Double
        Get
            Return m_dComm_percent
        End Get
    End Property


    Public Property RI_Arrangement_Line_Id() As Integer
        Get
            Return m_lRiArrangementLineId
        End Get
        Set(ByVal Value As Integer)
            m_lRiArrangementLineId = Value
        End Set
    End Property


    Public Property BrokerArray() As Object
        Get
            Return VB6.CopyArray(vBrokerArray)
        End Get
        Set(ByVal Value As Object)
            vBrokerArray = Value
        End Set
    End Property



    Public Property FACPropExists() As Boolean
        Get
            Return m_bFACPropExists
        End Get
        Set(ByVal Value As Boolean)
            m_bFACPropExists = Value
        End Set
    End Property

    Public WriteOnly Property AddMode() As String
        Set(ByVal Value As String)
            m_sLineAddMode = Value
        End Set
    End Property

    Public Property GrossPremium() As Decimal
        Get
            Return m_cGrossPremium
        End Get
        Set(ByVal value As Decimal)
            m_cGrossPremium = value
        End Set
    End Property

    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Dim sMessage, sTitle, sHelpFile As String
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_iUserID = .UserID
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bSIRFindRIParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oBusiness = temp_g_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            End If

            '    'CMG/PB Bug fix 315
            '    ' Get an instance of the business object via
            '    ' the public object manager.
            '    m_lReturn& = g_oObjectManager.GetInstance( _
            ''        oObject:=g_oBackofficelink, _
            ''        sClassName:="bBackofficelink.bBOlink", _
            ''        vInstanceManager:="Clientmanager")
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        ' Failed to get an instance of the business object.
            '        Initialise = PMFalse
            '
            '        ' Display error stating the problem.
            '
            '        ' Get description from the resource file.
            '        sTitle$ = iPMFunc.GetResData( _
            ''            iLangID:=g_iLanguageID%, _
            ''            lId:=ACBusinessFailTitle, _
            ''            iDataType:=PMResString)
            '
            '        sMessage$ = iPMFunc.GetResData( _
            ''            iLangID:=g_iLanguageID%, _
            ''            lId:=ACBusinessFail, _
            ''            iDataType:=PMResString)
            '
            '        ' Display message.
            '        MsgBox sMessage$, vbCritical, sTitle$
            '
            '        Exit Function
            '    End If


            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oClaimBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oClaimBusiness, "bCLMFindClaim.Business", vInstanceManager:="ClientManager")
            g_oClaimBusiness = temp_g_oClaimBusiness


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            End If
            'End CMG

            'sj 3/11/99 - start
            g_bGenericConnectionStatus = g_oObjectManager.GenericConnectionStatus
            'sj 3/11/99 - end

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = PMProductFamily
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            'Find out from the registry where the Help File is
            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrieve Helpfile", Application.ProductName)
                Return result
            End If

            If sHelpFile <> "" Then
                'TODO
                'App.HelpFile = sHelpFile
            End If

            ' Default to showing the normal interface

            'Default to show IncludeClosedBranch checkbox
            m_bIncludeClosedBranches = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If g_oBusiness IsNot Nothing Then
                    g_oBusiness.Dispose()
                    g_oBusiness = Nothing
                End If
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' ***************************************************************** '
    'Public Function SetKeys(vKeyArray(,) As(,) Variant ) As Long
    '
    'Dim lRow As Long
    '
    '    On Error GoTo Err_SetKeys
    '
    '    SetKeys = PMTrue
    '
    '    ' Check we have a valid array.
    '    If (IsArray(vKeyArray) = False) Then
    '        SetKeys = PMFalse
    '        Exit Function
    '    End If
    '
    '    ' Default Full_Address to false
    '    m_bNeedFullAddress = False
    '
    '    ' Step through the key array.
    '    For lRow& = LBound(vKeyArray, 2) To UBound(vKeyArray, 2)
    '        ' Assign the parameter member with the
    '        ' correct key array item.
    '
    '        ' {* USER DEFINED CODE (Begin) *}
    '        'SP011298 - changes to support new business roadmap
    '        Select Case Trim$(CStr(vKeyArray(PMKeyName, lRow&)))
    '
    '            'MEvans : 13-12-2002 : 202
    '            Case PMKeyNameAllowAddressSelection
    '                m_bAllowAddressSelection = CBool(vKeyArray(PMKeyValue, lRow))
    '            'End MEvans : 13-12-2002 : 202
    '
    '            Case PMKeyNamePartyCnt
    '                m_lPartyCnt& = CLng(vKeyArray(PMKeyValue, lRow&))
    '
    '            Case PMKeyNameShortName
    '                m_sShortName$ = CStr(vKeyArray(PMKeyValue, lRow&))
    '
    '            Case "agent_only"
    '                m_iAgentOnly% = CInt(vKeyArray(PMKeyValue, lRow&))
    '            'ECK 270499
    '            Case "special_party"
    '                m_sSpecialParty = CStr(vKeyArray(PMKeyValue, lRow&))
    '            ' TF140199
    '            Case PMKeyNameInsReference
    '                m_sInsuranceRef$ = CStr(vKeyArray(PMKeyValue, lRow&))
    '
    '            Case PMKeyNamePolicyNo
    '                m_sInsuranceRef$ = CStr(vKeyArray(PMKeyValue, lRow&))
    '
    '            ' CF261099
    '            Case PMKeyNamePartiesOnly
    '                If (UCase$(CStr(vKeyArray(PMKeyValue, lRow&))) = "YES") Then
    '                    m_bPartyTypeOnly = True
    '                Else
    '                    m_bPartyTypeOnly = False
    '                End If
    '
    '            'RKS 141004 PN13238 & PN14838
    '            Case PMKeyNameIncludeClosedBranches
    '                If (UCase$(CStr(vKeyArray(PMKeyValue, lRow&))) = "YES") Then
    '                    m_bIncludeClosedBranches = True
    '                Else
    '                    m_bIncludeClosedBranches = False
    '                End If
    ''Tomo140100
    '            Case "delete_mode"
    '                m_bDeleteMode = CBool(vKeyArray(PMKeyValue, lRow&))
    '
    '            ' CTAF 260900
    '            Case "swift_party_id"
    '                m_lSwiftPartyID = CLng(vKeyArray(PMKeyValue, lRow&))
    '
    '            ' CTAF 171100 - Swift requirement
    '            Case PMKeyNamePartyType
    '                m_vPartyType = vKeyArray(PMKeyValue, lRow&)
    '
    '            ' CTAF 171100 - Swift requirement
    '            Case PMKeyNameStatus
    '                m_vPartyStatus = CVar(vKeyArray(PMKeyValue, lRow&))
    '
    '            ' CTAF 171100 - Swift requirement
    '            Case PMKeyNamePartyLongName
    '                m_sLongName$ = CStr(vKeyArray(PMKeyValue, lRow&))
    '
    '            ' CTAF 171100 - Swift requirement
    '            Case PMKeyNameDOB
    '                If (IsDate(vKeyArray(PMKeyValue, lRow&)) = True) Then
    '                    m_vDateOfBirth = vKeyArray(PMKeyValue, lRow&)
    '                End If
    '
    '            ' CTAF 171100 - Swift requirement
    '            Case PMKeyNameAddLine1
    '                m_vAddLine1 = vKeyArray(PMKeyValue, lRow&)
    '
    '            ' CTAF 171100 - Swift requirement
    '            Case PMKeyNamePostCode
    '                m_vPostCode = vKeyArray(PMKeyValue, lRow&)
    '            'DC260202
    '            Case PMKeyNameNavStep
    '                m_vNavStep = vKeyArray(PMKeyValue, lRow&)
    '            'DC160703 -ISS5384
    '            Case PMKeyNameSourceId
    '                m_lPartySourceId = vKeyArray(PMKeyValue, lRow&)
    '
    '            'MKW080104 PN9424 Include Complaints FSA reason.
    '            Case PMKeyNameIncludeComplaints
    '                m_iIsComplaint = CInt(vKeyArray(PMKeyValue, lRow&))
    '
    '            ' SET 08/06/2004 ISS11882
    '            Case PMKeyNameAskDPAQuestions
    '                If CInt(vKeyArray(PMKeyValue, lRow&)) = 1 Then
    '                    m_bDPARequired = True
    '                Else
    '                    m_bDPARequired = False
    '                End If
    '
    '            Case "risk_transfer_agreement"
    '                m_bRiskTransfer = CBool(vKeyArray(PMKeyValue, lRow&))
    '
    '        End Select
    '
    '        ' {* USER DEFINED CODE (End) *}
    '
    '    Next lRow&
    '
    '    Exit Function
    '
    '
    'Err_SetKeys:
    '
    '    SetKeys = PMError
    '
    '    ' Log Error Message
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="SetKeys Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="SetKeys", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function
    '
    '' ***************************************************************** '
    '' Name: GetKeys (Standard Method)
    ''
    '' Description: Stores all of the key array with the parameter
    ''              members.
    ''
    '' ***************************************************************** '
    'Public Function GetKeys(vKeyArray(,) As(,) Variant ) As Long
    '
    'Dim lRow As Long
    'Dim iMax As Integer
    '
    '    On Error GoTo Err_GetKeys
    '
    '    GetKeys = PMTrue
    '
    '    ' {* USER DEFINED CODE (Begin) *}
    '
    '    ' Initialise the key array with the number of
    '    ' keys needed to be returned.
    '    ' Note: Remember arrays are zero based.
    '    'SP041298 - Add 'client_code' key for shotname to support gemini.
    '    ' CF 261099 - We need party type or not?
    '    ' CF 180800 - Not needed now
    '    ' CTAF 171100 - Did the iMax thing, and the iAddress part too
    '    ' TF281100 - Not needed as PartyType returned in both cases
    '    'DC140201 added branch id
    '    'DC260202 added resolved name
    '    'DC1501003 -PN7449
    '    'iMax% = 21
    '    'iMax% = 22
    '    'JT PN-13238 One more variable need to be passed for IncludeBranchchecked
    '     iMax% = 25
    ''    If (m_bPartyTypeOnly = True) Then
    ''        iMax% = 19
    ''    Else
    ''        iMax% = 18
    ''    End If
    '
    '    ReDim vKeyArray(1, iMax)
    '
    '    ' Assign the key array with the parameter members.
    '    vKeyArray(PMKeyName, 0) = PMKeyNamePartyCnt
    '    vKeyArray(PMKeyValue, 0) = m_lPartyCnt&
    '    vKeyArray(PMKeyName, 1) = PMKeyNameShortName
    '    vKeyArray(PMKeyValue, 1) = m_sShortName$
    '    vKeyArray(PMKeyName, 2) = PMKeyNameLongName
    '    vKeyArray(PMKeyValue, 2) = m_sLongName$
    '    'SP011298 - Amend keys to support roadmap
    '    vKeyArray(PMKeyName, 3) = PMKeyNameAgentCnt
    '    vKeyArray(PMKeyValue, 3) = m_lPartyAgentCnt     'PN13921
    '    vKeyArray(PMKeyName, 4) = PMKeyNameClientCnt
    '    vKeyArray(PMKeyValue, 4) = m_lPartyCnt&
    '    vKeyArray(PMKeyName, 5) = PMKeyNameClientUIK
    '    vKeyArray(PMKeyValue, 5) = m_lPartyUIK&
    '    vKeyArray(PMKeyName, 6) = PMKeyNameClientName
    '    vKeyArray(PMKeyValue, 6) = m_sLongName$
    '    vKeyArray(PMKeyName, 7) = PMKeyNameClientCode
    '    vKeyArray(PMKeyValue, 7) = m_sShortName$
    '    vKeyArray(PMKeyName, 8) = "agent_only"
    '    vKeyArray(PMKeyValue, 8) = m_iAgentOnly%
    '
    '    ' CTAF 260900 - For Work Manager
    '    vKeyArray(PMKeyName, 9) = PMKeyNameTaskCustomer
    '    vKeyArray(PMKeyValue, 9) = m_sLongName$
    '
    '    ' CTAF These keys need to go into PMNavKeyConst when its
    '    '      checked back in
    '
    '    ' CTAF 260900 - Date of birth
    '    vKeyArray(PMKeyName, 10) = "date_of_birth"
    '    vKeyArray(PMKeyValue, 10) = m_vDateOfBirth
    '
    '    ' CTAF 260900 - Swift Party ID
    '    vKeyArray(PMKeyName, 11) = "swift_party_id"
    '    vKeyArray(PMKeyValue, 11) = m_lSwiftPartyID&
    '
    '    ' Type - amended TF281100
    '    If (m_bPartyTypeOnly = False) Then
    '        vKeyArray(PMKeyName, 12) = PMKeyNamePartyType
    '        vKeyArray(PMKeyValue, 12) = m_vPartyType
    '    Else
    '        vKeyArray(PMKeyName, 12) = PMKeyNamePartyType
    '        vKeyArray(PMKeyValue, 12) = m_sPartyType$
    '    End If
    '
    '
    '
    '    ' Status
    '    vKeyArray(PMKeyName, 13) = PMKeyNameStatus
    '    vKeyArray(PMKeyValue, 13) = m_vPartyStatus
    '
    '    ' Addresses
    '    vKeyArray(PMKeyName, 14) = PMKeyNameAddLine1
    '    vKeyArray(PMKeyName, 15) = PMKeyNameAddLine2
    '    vKeyArray(PMKeyName, 16) = PMKeyNameAddLine3
    '    vKeyArray(PMKeyName, 17) = PMKeyNameAddLine4
    '    vKeyArray(PMKeyName, 18) = PMKeyNamePostCode
    '    'DC140201 added for New Business Roadmap
    '    vKeyArray(PMKeyName, 19) = ACTKeyNameBranchID
    '    'DC260202
    '    vKeyArray(PMKeyName, 20) = PMKeyNamePartyResolvedName
    '
    '    vKeyArray(PMKeyValue, 14) = m_vAddLine1
    '    vKeyArray(PMKeyValue, 15) = m_vAddLine2
    '    vKeyArray(PMKeyValue, 16) = m_vAddLine3
    '    vKeyArray(PMKeyValue, 17) = m_vAddLine4
    '    vKeyArray(PMKeyValue, 18) = m_vPostCode
    '    'DC140201 added for New Business Roadmap
    '    vKeyArray(PMKeyValue, 19) = g_iSourceID
    '    'DC260202
    '    vKeyArray(PMKeyValue, 20) = m_sResolvedName
    '
    '    'MEvans : 13-12-2002 : 202
    '    vKeyArray(PMKeyName, 21) = PMKeyNameAddressCnt
    '    vKeyArray(PMKeyValue, 21) = m_lAddressCnt
    '    'MEvans : 13-12-2002 : 202
    '
    '    'DC151003 -PN7449
    '    vKeyArray(PMKeyName, 22) = PMKeyNameHandlerType
    '    vKeyArray(PMKeyValue, 22) = m_sSelectedPartyType
    '
    '    'JT PN-13238 28-10-2004
    '    vKeyArray(PMKeyName, 23) = PMKeyNameIsIncludeClosedBrancheChecked
    '    vKeyArray(PMKeyValue, 23) = m_bIsIncludeClosedBrancheChecked
    '
    '    vKeyArray(PMKeyName, 24) = PMKeyNamePartySourceId                   'PN13921
    '    vKeyArray(PMKeyValue, 24) = m_lPartySourceId                        'PN13921
    '
    '    'MKW 150606
    '    vKeyArray(PMKeyName, 24) = PMKeyNameCountryId
    '    vKeyArray(PMKeyValue, 24) = m_lCountryId
    '
    '    ' CF261099 - Return party type if needed
    '    ' TF281100 - not needed
    ''    If (m_bPartyTypeOnly = True) Then
    ''        vKeyArray(PMKeyName, iMax) = PMKeyNamePartyType
    ''        vKeyArray(PMKeyValue, iMax) = m_sPartyType$
    ''    End If
    '
    '    ' {* USER DEFINED CODE (End) *}
    '
    '    Exit Function
    '
    'Err_GetKeys:
    '
    '    GetKeys = PMError
    '
    '    ' Log Error Message
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="GetKeys Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="GetKeys", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function

    ' ***************************************************************** '
    ' Name: GetSummary (Standard Method)
    '
    '' Description: Stores all of the summary array with the parameter
    ''              members.
    ''
    '' ***************************************************************** '
    'Public Function GetSummary(vSummaryArray As Variant) As Long
    '
    'Dim lRow As Long
    '
    '    On Error GoTo Err_GetSummary
    '
    '    GetSummary = PMTrue
    '
    '    ' {* USER DEFINED CODE (Begin) *}
    '
    '    ' Initialise the summary array with the number of
    '    ' items needed to be returned.
    '    ' Note: Remember arrays are zero based.
    '    'SP011298 - Changes to support roadmap (if nowt to do return vSummarryArray="")
    '    ReDim vSummaryArray(PMNavSummValue, 0)
    '
    '    ' Assign the key array with the parameter members.
    '    ' TF180199 - Set header to appropriate party type
    '    vSummaryArray(PMNavSummHeading, 0) = m_sSelectedPartyType$
    '    vSummaryArray(PMNavSummValue, 0) = m_sLongName$
    '
    '    ' {* USER DEFINED CODE (End) *}
    '
    '    Exit Function
    '
    '
    'Err_GetSummary:
    '
    '    GetSummary = PMError
    '
    '    ' Log Error Message
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="GetSummary Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="GetSummary", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CType(CInt(vTask), gPMConstants.PMEComponentAction)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            ' Set the process modes for the business object.
            If Not (g_oBusiness Is Nothing) Then

                m_lReturn = g_oBusiness.SetProcessModes(vTask:=vTask, vNavigate:=vNavigate, vProcessMode:=vProcessMode, vTransactionType:=vTransactionType, vEffectiveDate:=vEffectiveDate)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes")

                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessParties
    '
    ' Description: Shows the parties form and gets the party type
    '
    ' History: 26/10/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    'Private Function ProcessParties() As Long
    '
    'Dim oParties As frmParties
    '
    '    On Error GoTo Err_ProcessParties
    '
    '    ProcessParties = PMTrue
    '
    '    ' Get a new instance of frmParties
    '    Set oParties = New frmParties
    '
    '    ' Load it
    '    Load oParties
    '
    '    ' Show it
    '    oParties.Show vbModal
    '
    '    If (oParties.Status = PMCancel) Then
    '        ' Unload the object
    '        Unload oParties
    '        ' Remove from memory
    '        Set oParties = Nothing
    '        ' Return False
    '        ProcessParties = PMFalse
    '        Exit Function
    '    End If
    '
    '    ' Get the party type
    '    m_sPartyType = oParties.PartyType
    '
    '    ' Unload it
    '    Unload oParties
    '
    '    Set oParties = Nothing
    '
    '    Exit Function
    '
    'Err_ProcessParties:
    '
    '    ProcessParties = PMError
    '
    '    ' Log Error Message
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="ProcessParties Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="ProcessParties", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function


    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sPartyTypeCode As String = ""

            m_lReturn = GetValidSources()
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check if the PartyCnt is greater than zero. If so,
            ' there is no need to proceed with the interface. We
            ' can therefore return straight back out.
            If m_lPartyCnt > 0 Then
                ' ID is greater than zero.
                m_lStatus = gPMConstants.PMEReturnCode.PMOK
                If Not bIsFAx Then
                    m_lReturn = ProcessBrokerInterface()
                End If
                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            Else

                If bIsFAx Then
                    ' Starts the interface processing.
                    m_lReturn = ProcessInterface()
                Else
                    m_lReturn = ProcessBrokerInterface()
                End If
                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: ProcessInterface (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the
    '              interface.
    '
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load the interface into memory.
        m_lReturn = LoadInterface()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        ' Display the interface.
        m_lReturn = ShowInterface(lDisplayState:=FormShowConstants.Modal)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to display the inteface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Destroy the interface from memory.
        m_lReturn = UnLoadInterface()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to unload the interface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: LoadInterface (Standard Method)
    '
    ' Description: Loads the instance of the interface into memory and
    '              passes the parameters in.
    '
    ' ***************************************************************** '
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_ofrmInterface = New iPMUFindRIParty.frmInterface()


        ' Assign the parameters to the interface properties.
        With m_ofrmInterface
            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            ' {* USER DEFINED CODE (Begin) *}
            .ShortName = m_sShortName
            .NotEditable = m_iNotEditable
            .DeleteMode = m_bDeleteMode


            'Developer Guide no. 24
            .SourceArray = m_vSourceArray


            'Developer Guide no. 24
            .SourceArrayIncludeClosedBranch = m_vSourceArrayIncludeClosedBranch
            .LongName = m_sLongName
            .IncludeClosedBranches = m_bIncludeClosedBranches
            .IsFAX = bIsFAx
            .RiskId = m_lRiskId
            .InsuranceFileCnt = m_lInsuranceFileCnt
            .RIArrangementid = m_lRiArrangement_id
            .GroupingId = m_lGroupingId
            .UpperLimit = m_cUpperLimit
            .LowerLimit = m_cLowerLimit
            .ExistingLimits = m_vExistingLimits
            .ClaimId = m_lClaimId
            .TotalSumInsured = m_cTotalSumInsured
            .FACPropExists = m_bFACPropExists
            .AddMode = m_sLineAddMode
            .GrossPremium = m_cGrossPremium

            .AddedFindRIPartyLines = m_vAddedFindRIPartyLines
        End With

        ' Load the instance of the interface into memory.

        'Developer Guide no. 68
        'Load(m_ofrmInterface)

        ' Check if we have had an error so far.
        If m_ofrmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = m_ofrmInterface.ErrorNumber
        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: UnLoadInterface (Standard Method)
    '
    ' Description: Unloads the instance of the interface from memory.
    '
    ' ***************************************************************** '
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.
        With m_ofrmInterface
            m_lStatus = .Status

            ' {* USER DEFINED CODE (Begin) *}

            m_lPartyCnt = .PartyCnt
            m_sShortName = .ShortName
            m_cUpperLimit = .UpperLimit
            m_cLowerLimit = .LowerLimit
            m_dRetained_percent = .Retained_Percent
            m_cPremium = .Premium
            m_cPremiumTax = .PremiumTax
            m_cCommission = .Commission
            m_dComm_percent = .Comm_percent
            m_cCommTax = .CommTax
            m_cSumInsured = .SumInsured
            m_lGroupingId = .GroupingId
            m_bIsIncludeClosedBrancheChecked = .IsIncludeClosedBranchChecked


            m_vAddedFindRIPartyLines = .AddedFindRIPartyLines 'PN44646
            m_sAgreementCode = .AgreementCode ' Sankar - PN 50348
            ' {* USER DEFINED CODE (End) *}
        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        m_ofrmInterface.Close()
        m_ofrmInterface = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ShowInterface (Standard Method)
    '
    ' Description: Displays the instance of the interface using the
    '              display state.
    '
    ' ***************************************************************** '
    Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Display the interface.
        VB6.ShowForm(m_ofrmInterface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            If m_ofrmInterface.ErrorNumber <> 0 Then
                result = m_ofrmInterface.ErrorNumber
            End If
        End If

        Return result

    End Function
    'PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: GetValidSources (Standard Method)
    '
    ' Description: Calls the appropriate methods to get the Sources
    '              which the the current user can access
    '
    ' ***************************************************************** '
    Private Function GetValidSources() As Integer
        Dim result As Integer = 0
        Dim sAgencyOrUnderwriting As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Call PMUser to get the Sources
        ' Get an instance of the business object via
        ' the public object manager.
        Dim temp_g_oPMUser As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_g_oPMUser, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        g_oPMUser = temp_g_oPMUser

        '    ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get an instance of the business object.
            result = gPMConstants.PMEReturnCode.PMFalse

            '        ' Display error stating the problem.

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bPMUser.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources", excep:=New Exception(Information.Err().Description))

            Return result
        End If

        m_lReturn = iPMFunc.getUnderwritingOrAgency(r_vUnderwriting:=sAgencyOrUnderwriting)


        m_lReturn = g_oPMUser.GetUserSources(r_vSourceArray:=m_vSourceArray, v_vUserID:=g_iUserID)

        m_lReturn = g_oPMUser.GetUserSources(r_vSourceArray:=m_vSourceArrayIncludeClosedBranch, v_vUserID:=g_iUserID, v_bIncludeDeletedSources:=True)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get valid sources", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources", excep:=New Exception(Information.Err().Description))

            Return result
        End If

        '    ' Remove instance of PMUser
        If Not (g_oPMUser Is Nothing) Then

            g_oPMUser.Dispose()
            g_oPMUser = Nothing
        End If

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: CheckSecurity (Standard Method)
    '
    ' Description: Check whether the user has authority to view clients
    ' History:     2005 Client Security  20/04/2005
    '
    ' ***************************************************************** '
    Public Function CheckSecurity(ByRef r_bViewAuthority As Boolean, ByRef r_bEditAuthority As Boolean, ByRef r_bDeleteAuthority As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            r_bViewAuthority = True
            r_bEditAuthority = True



            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckSecurity", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function ProcessBrokerInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load the interface into memory.
        m_lReturn = LoadBrokerInterface()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        ' Display the interface.
        m_lReturn = ShowBrokerInterface(lDisplayState:=FormShowConstants.Modal)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to display the inteface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Destroy the interface from memory.
        m_lReturn = UnLoadBrokerInterface()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to unload the interface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: LoadBrokerInterface (Standard Method)
    '
    ' Description: Loads the instance of the interface into memory and
    '              passes the parameters in.
    '
    ' ***************************************************************** '
    Private Function LoadBrokerInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_ofrmBrokerInterface = New iPMUFindRIParty.frmRIBrokerParticipants()

        ' Assign the parameters to the interface properties.
        With m_ofrmBrokerInterface
            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            '        .Navigate = m_lNavigate&
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            ' {* USER DEFINED CODE (Begin) *}
            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                .Action = 2
            ElseIf m_iTask = gPMConstants.PMEComponentAction.PMView Then
                .Action = 3
            End If
            .RiArrangementLineID = m_lRiArrangementLineId


            'Developer Guide no. 24
            .SourceArray = m_vSourceArray


            'Developer Guide no. 24
            .SourceArrayIncludeClosedBranch = m_vSourceArrayIncludeClosedBranch
            .IsFAX = bIsFAx
            .PartyCnt = m_lPartyCnt
            .BrokerArray = VB6.CopyArray(vBrokerArray)
            .AddParticipantsFromTreaty = m_bAddParticipantsFromTreaty    'E005 Santosh Singh
            .TreatyPartiesBrokerParticipantsForDisplay = m_vTreatyPartiesBrokerParticipantForDisplay  'E005
        End With

        ' Load the instance of the interface into memory.

        'Developer Guide no. 68
        'Load(m_ofrmBrokerInterface)

        ' Check if we have had an error so far.
        If m_ofrmBrokerInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = m_ofrmBrokerInterface.ErrorNumber
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ShowBrokerInterface (Standard Method)
    '
    ' Description: Displays the instance of the interface using the
    '              display state.
    '
    ' ***************************************************************** '
    Private Function ShowBrokerInterface(ByRef lDisplayState As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Display the interface.
        VB6.ShowForm(m_ofrmBrokerInterface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            If m_ofrmBrokerInterface.ErrorNumber <> 0 Then
                result = m_ofrmBrokerInterface.ErrorNumber
            End If
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UnLoadBrokerInterface (Standard Method)
    '
    ' Description: Unloads the instance of the interface from memory.
    '
    ' ***************************************************************** '
    Private Function UnLoadBrokerInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.
        With m_ofrmBrokerInterface
            m_lStatus = .Status
            vBrokerArray = .BrokerArray
            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}
        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        m_ofrmBrokerInterface.Close()
        m_ofrmBrokerInterface = Nothing

        Return result

    End Function
    Private Shared _DefaultInstance As Interface_Renamed = Nothing
    Public Shared ReadOnly Property DefaultInstance() As Interface_Renamed
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New Interface_Renamed
            End If
            Return _DefaultInstance
        End Get
    End Property
End Class

