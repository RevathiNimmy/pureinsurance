Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'developer guide no. 129
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
    ' SP 011298 - changes to support new business roadmap
    ' SP 130199 - Change for Tim
    ' CJB210405 - PN13921 Added support for PartyAgentCnt & PartySourceID
    '             Agent Type to be included - WR22 : Agent Payments
    ' ***************************************************************** '

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lPMAuthorityLevel As String = ""
    Private m_lStatus As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lPartyCnt As Integer
    Private m_iCurrencyID As Integer
    Private m_sShortName As String = ""
    Private m_sLongName As String = ""
    Private m_sResolvedName As String = ""
    Private m_iAgentOnly As Integer
    Private m_bIntroducerOnly As Boolean
    Private m_sSpecialParty As String = ""
    Private m_lPartyUIK As Integer
    Private m_sInsuranceRef As String = ""
    Private m_sSelectedPartyType As String = ""
    'DC160703 -ISS5384
    Private m_lPartySourceId As Integer
    Private m_lPartyAgentCnt As Integer 'PN13921

    ' Alix - 07/11/2002
    Private m_bAllowAddressSelection As Boolean
    Private m_lAddressCnt As Integer

    Private m_iNotEditable As Integer
    'eck090500
    Private m_vSourceArray(,) As Object

    'DM PN29952
    Private m_bExcludeMultiInsurer As Boolean

    'PN 15885 JT 25-10-04
    'Array for all sourceid even closed
    Private m_vSourceArrayIncludeClosedBranch As Object = Nothing

    Private m_bViewAuthority As Boolean '2005 Client Manager Security
    Private m_bEditAuthority As Boolean '2005 Client Manager Security
    Private m_bDeleteAuthority As Boolean '2005 Client Manager Security

    ' CTAF 260900
    Private m_vDateOfBirth As String = ""
    Private m_lSwiftPartyID As Integer

    'MSB 03/03/03
    Private m_vDateCancelled As String = ""

    ' CTAF 171100 - Swift Requirements
    Private m_vPartyType As String = ""
    Private m_vPostCode As String = ""
    Private m_vPartyStatus As String = ""
    Private m_vAddLine1 As String = ""
    Private m_vAddLine2 As String = ""
    Private m_vAddLine3 As String = ""
    Private m_vAddLine4 As String = ""
    'DC260202
    Private m_vNavStep As String = ""
    ' PW190802
    Private m_bSuppressSubAgents As Boolean

    Private m_bIsInTransferMode As Boolean

    Private m_iSourceID As Integer 'Agent Filtering

    ' {* USER DEFINED CODE (End) *}

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Member to hold instance of form interface
    Private m_ofrmInterface As iPMBFindParty.frmInterface

    ' CF 261099
    ' Do we want to show frmParties only?
    Private m_bPartyTypeOnly As Boolean
    ' Party type to return
    Private m_sPartyType As String = ""

    Private m_bDeleteMode As Boolean

    'sj 3/11/99 - start
    Private m_lInvariantKey As Integer

    'MSS200901 - Added for Merge
    Private m_bIgnoreDriversAndWitnesses As Boolean
    'MSS200901 - Merge end

    'sj 06/06/2002 - start
    Private m_lUserAgentCnt As Integer
    'sj 06/06/2002 - end
    'sj 02/07/2002 - start
    Private m_lUserInsurerCnt As Integer
    Private m_bRestrictInsurerAccess As Boolean
    'sj 02/07/2002 - end
    ' NeedFullAddress
    Private m_bNeedFullAddress As Boolean

    Private m_vValidPartyTypesArray() As Object = Nothing

    'MKW080104 PN9424 Include Complaint in FSA reasons
    Private m_iIsComplaint As Integer

    ' SET 08/06/2004 ISS11882
    Private m_bDPARequired As Boolean

    'RKS 141004 PN13238 & PN14838
    Private m_bIncludeClosedBranches As Boolean
    'JT PN-13238 For holding whether the Chkbox is checked or not
    Private m_bIsIncludeClosedBrancheChecked As Boolean

    'DC101204
    Private m_vAgentTypes As Object = Nothing
    Private m_sAgencyOrunderwriting As String = ""

    Private m_bIgnoreDPAQuestions As Boolean
    Private m_bEnableNewParty As Boolean

    ' RDC 20050901
    Private m_bSuppressCancelledAgents As Boolean

    'MKW 150606
    Private m_lCountryId As Integer
    Private m_bRiskTransfer As Boolean
    'PYV DM
    Private m_iBureauAccount As Integer
    ' Gaurav
    Private m_bIsRetained As Boolean
    Private m_iRetainedValue As Integer

    Private m_sReinsuranceTypeArray As String = ""

    ' R.Griffiths 2006-10-16 (Plus One)
    Private m_sPartyTelephonePrefix As String = ""
    Private m_sPartyTelephoneNumber As String = ""
    Private m_sPartyAutoSearch As String = ""

    Private m_lKeepOnTop As Integer
    Private m_vBrokerArray As Object = Nothing
    Private m_bAllowAgentSearch As Boolean

    Private m_bSkipFindParty As Boolean

    'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.3.1.1)
    Private m_bDisableWildcardSearchOption As Boolean
    Private m_bEnablePartialWildcardSearchOption As Boolean
    Private Const kSystemOptionDisableWildcardSearch As Integer = 5065
    Private Const kSystemOptionEnablePartialWildcardSearch As Integer = 5066
    'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.3.1.1)

    Private m_bSearchPartyWithShortCode As Boolean
    'Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
    Private m_vTreatyPartiesBrokerParticipantForDisplay As Object  'E005
    Public Property TreatyPartiesBrokerParticipantsForDisplay() As Object
        Get
            Return m_vTreatyPartiesBrokerParticipantForDisplay
        End Get
        Set(ByVal Value As Object)
            m_vTreatyPartiesBrokerParticipantForDisplay = Value
        End Set
    End Property

    'SAGICOR WPR14
    Private m_lCommissionlevel As Integer
    Public WriteOnly Property CommissionLevel() As Integer
        Set(ByVal Value As Integer)
            m_lCommissionlevel = Value
        End Set
    End Property

    Public Property SkipFindParty() As Boolean
        Get
            Return m_bSkipFindParty
        End Get
        Set(ByVal Value As Boolean)
            m_bSkipFindParty = Value
        End Set
    End Property

    ' Property desined to find that whether we have inputed any retained filter
    ' on the query
    Public Property IsRetained() As Boolean
        Get
            Return m_bIsRetained
        End Get
        Set(ByVal Value As Boolean)
            m_bIsRetained = Value
        End Set
    End Property

    ' Property used to append all the reinsurance types and make generic
    ' search criteria for reinsurance type

    Public Property ReinsuranceTypeArray() As String
        Get
            Return m_sReinsuranceTypeArray
        End Get
        Set(ByVal Value As String)
            If m_sReinsuranceTypeArray = "" Then
                m_sReinsuranceTypeArray = "('" & Value & "')"
            Else
                m_sReinsuranceTypeArray = m_sReinsuranceTypeArray.Substring(0, m_sReinsuranceTypeArray.Length - 1)
                m_sReinsuranceTypeArray = m_sReinsuranceTypeArray & ",'" & Value & "')"
            End If
        End Set
    End Property

    Public Property RetainedValue() As Integer
        Get
            Return m_iRetainedValue
        End Get
        Set(ByVal Value As Integer)
            m_iRetainedValue = Value
        End Set
    End Property

    ' RDC 20050901
    Public WriteOnly Property SuppressCancelledAgents() As Boolean
        Set(ByVal Value As Boolean)
            m_bSuppressCancelledAgents = Value
        End Set
    End Property

    Public Property IgnoreDPAQuestions() As Boolean
        Get
            Return m_bIgnoreDPAQuestions
        End Get
        Set(ByVal Value As Boolean)
            m_bIgnoreDPAQuestions = Value
        End Set
    End Property
    'RKS 141004 PN13238 & PN14838
    'RKS 141004 PN13238 & PN14838
    Public Property IncludeClosedBranches() As Boolean
        Get
            Return m_bIncludeClosedBranches
        End Get
        Set(ByVal Value As Boolean)
            m_bIncludeClosedBranches = Value
        End Set
    End Property

    Public WriteOnly Property IsComplaint() As Integer
        Set(ByVal Value As Integer)
            m_iIsComplaint = Value
        End Set
    End Property

    Public WriteOnly Property ValidPartyTypesArray() As Object()
        Set(ByVal Value As Object())
            'CJR 20/1/2003 Added for IAG.  Alows the client app. to specify a list of valid parties
            m_vValidPartyTypesArray = Value
        End Set
    End Property

    ' PW190802 allow to suppress sub agents
    Public WriteOnly Property SuppressSubAgents() As Boolean
        Set(ByVal Value As Boolean)
            m_bSuppressSubAgents = Value
        End Set
    End Property

    Public WriteOnly Property IsInTransferMode() As Boolean
        Set(ByVal Value As Boolean)
            m_bIsInTransferMode = Value
        End Set
    End Property

    'sj 03/07/2002 - start
    Public ReadOnly Property RestrictInsurerAccess() As Boolean
        Get
            Return m_bRestrictInsurerAccess
        End Get
    End Property
    Public ReadOnly Property UserInsurerCnt() As Integer
        Get
            Return m_lUserInsurerCnt
        End Get
    End Property
    'sj 03/07/2002 - end
    Public Property NeedFullAddress() As Boolean
        Get
            Return m_bNeedFullAddress
        End Get
        Set(ByVal Value As Boolean)
            m_bNeedFullAddress = Value
        End Set
    End Property

    Public Property InvariantKey() As Integer
        Get
            Return m_lInvariantKey
        End Get
        Set(ByVal Value As Integer)
            m_lInvariantKey = Value
        End Set
    End Property
    ' PRIVATE Data Members (End)
    Public WriteOnly Property AllowAddressSelection() As Boolean
        Set(ByVal Value As Boolean)
            m_bAllowAddressSelection = Value
        End Set
    End Property

    'Agent type to be included - WR22 : Agent Payments
    Public WriteOnly Property AllowAgentSearch() As Boolean
        Set(ByVal Value As Boolean)
            m_bAllowAgentSearch = Value
        End Set
    End Property

    Public Property AddressCnt() As Integer
        Get
            Return m_lAddressCnt
        End Get
        Set(ByVal Value As Integer)
            m_lAddressCnt = Value
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
    Public Property CurrencyID() As Integer
        Get
            Return m_iCurrencyID
        End Get
        Set(ByVal Value As Integer)
            m_iCurrencyID = Value
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
    Public ReadOnly Property ResolvedName() As String
        Get
            Return m_sResolvedName
        End Get
    End Property


    Public WriteOnly Property AgentOnly() As Integer
        Set(ByVal Value As Integer)
            m_iAgentOnly = Value
        End Set
    End Property

    Public WriteOnly Property SpecialParty() As String
        Set(ByVal Value As String)
            m_sSpecialParty = Value
        End Set
    End Property

    Public WriteOnly Property RiskTransfer() As Boolean
        Set(ByVal Value As Boolean)
            m_bRiskTransfer = Value
        End Set
    End Property

    Public ReadOnly Property PartyUIK() As Integer
        Get
            Return m_lPartyUIK
        End Get
    End Property

    ' TF140199
    Public WriteOnly Property InsuranceRef() As String
        Set(ByVal Value As String)
            m_sInsuranceRef = Value
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

    Public WriteOnly Property EnableNewParty() As Boolean
        Set(ByVal Value As Boolean)
            m_bEnableNewParty = Value
        End Set
    End Property

    Public ReadOnly Property PartyType() As String
        Get
            Return m_sSelectedPartyType
        End Get
    End Property

    Public ReadOnly Property PartyTypeDesc() As String
        Get
            Return m_vPartyType
        End Get
    End Property

    Public Property SwiftPartyID() As Integer
        Get
            Return m_lSwiftPartyID
        End Get
        Set(ByVal Value As Integer)
            m_lSwiftPartyID = Value
        End Set
    End Property

    'MSS200901 - Added for merge
    Public Property IgnoreDriversAndWitnesses() As Boolean
        Get
            Return m_bIgnoreDriversAndWitnesses
        End Get
        Set(ByVal Value As Boolean)
            m_bIgnoreDriversAndWitnesses = Value
        End Set
    End Property
    'MSS200901 - Merge End
    'MSB 03/03/2003
    Public Property DateCancelled() As String
        Get
            Return m_vDateCancelled
        End Get
        Set(ByVal Value As String)

            m_vDateCancelled = CStr(Value)
        End Set
    End Property
    'DC160703 -ISS5384 -start
    Public Property PartySourceId() As Integer
        Get
            Return m_lPartySourceId
        End Get
        Set(ByVal Value As Integer)
            m_lPartySourceId = Value
        End Set
    End Property

    Public Property PartyAgentCnt() As Integer
        Get
            Return m_lPartyAgentCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyAgentCnt = Value
        End Set
    End Property
    Public WriteOnly Property ExcludeMultiInsurer() As Boolean
        Set(ByVal Value As Boolean)
            m_bExcludeMultiInsurer = Value
        End Set
    End Property

    Public WriteOnly Property IntroducerOnly() As Boolean
        Set(ByVal Value As Boolean)
            m_bIntroducerOnly = Value
        End Set
    End Property
    Public Property SearchPartyWithShortCode() As Boolean
        Get
            Return m_bSearchPartyWithShortCode
        End Get
        Set(ByVal Value As Boolean)
            m_bSearchPartyWithShortCode = Value
        End Set
    End Property

    ''Agent Filtering
    Public Property BranchID() As Integer
        Get
            Return m_iSourceID
        End Get
        Set(ByVal Value As Integer)
            m_iSourceID = Value
        End Set
    End Property

    Public ReadOnly Property BrokerArray() As Object
        Get
            Return m_vBrokerArray
        End Get
    End Property
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
        Dim sMessage As String = String.Empty
        Dim sTitle As String = String.Empty
        Dim sHelpFile As String = String.Empty
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
            Dim temp_g_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bSIRFindParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
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

            'CMG/PB Bug fix 315
            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oBackofficelink As Object

            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBackofficelink, "bBackOfficeLink.bBOLink", "CLIENTMANAGER")
            g_oBackofficelink = temp_g_oBackofficelink

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

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oClaimBusiness As Object = Nothing
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

            g_bGenericConnectionStatus = g_oObjectManager.GenericConnectionStatus

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

            ' Default to showing the normal interface
            m_bPartyTypeOnly = False

            'RKS 141004 PN13238 & PN14838
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
    ''Agent Filtering

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
                If g_iRefCounter > 0 Then Exit Sub
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
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a valid array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Default Full_Address to false
            m_bNeedFullAddress = False

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.

                ' {* USER DEFINED CODE (Begin) *}
                'SP011298 - changes to support new business roadmap


                Select Case Convert.ToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()

                    Case PMNavKeyConst.PMKeyNameAllowAddressSelection
                        m_bAllowAddressSelection = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNamePartyCnt
                        m_lPartyCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameShortName
                        m_sShortName = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case "agent_only"
                        m_iAgentOnly = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case "special_party"
                        m_sSpecialParty = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameInsReference
                        m_sInsuranceRef = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNamePolicyNo
                        m_sInsuranceRef = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))


                    Case PMNavKeyConst.PMKeyNamePartiesOnly
                        m_bPartyTypeOnly = (CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)).ToUpper() = "YES")


                    Case PMNavKeyConst.PMKeyNameIncludeClosedBranches
                        m_bIncludeClosedBranches = (CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)).ToUpper() = "YES")

                    Case "delete_mode"
                        m_bDeleteMode = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case "swift_party_id"
                        m_lSwiftPartyID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNamePartyType
                        m_vPartyType = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameStatus
                        m_vPartyStatus = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNamePartyLongName
                        m_sLongName = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameDOB
                        If Information.IsDate(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) Then
                            m_vDateOfBirth = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If

                    Case PMNavKeyConst.PMKeyNameAddLine1
                        m_vAddLine1 = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNamePostCode
                        m_vPostCode = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameNavStep
                        m_vNavStep = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameSourceId
                        m_lPartySourceId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameIncludeComplaints
                        m_iIsComplaint = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameAskDPAQuestions
                        m_bDPARequired = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) = 1

                    Case "risk_transfer_agreement"
                        m_bRiskTransfer = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNamePartyTelephonePrefix
                        m_sPartyTelephonePrefix = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNamePartyTelephoneNumber
                        m_sPartyTelephoneNumber = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNamePartyAutoSearch
                        m_sPartyAutoSearch = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameKeepWindowOnTop

                        m_lKeepOnTop = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyBureauAccount

                        m_iBureauAccount = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case "include_agent"

                        m_bAllowAgentSearch = gPMFunctions.ToSafeBoolean(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    Case CStr(m_lCommissionlevel)
                        m_lCommissionlevel = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                End Select

                ' {* USER DEFINED CODE (End) *}

            Next lRow

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim iMax As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            'SP041298 - Add 'client_code' key for shotname to support gemini.
            ' CF 261099 - We need party type or not?
            ' CF 180800 - Not needed now
            ' CTAF 171100 - Did the iMax thing, and the iAddress part too
            ' TF281100 - Not needed as PartyType returned in both cases
            'DC140201 added branch id
            'DC260202 added resolved name
            'DC1501003 -PN7449
            'iMax% = 21
            'iMax% = 22
            'JT PN-13238 One more variable need to be passed for IncludeBranchchecked
            iMax = 25

            ReDim vKeyArray(1, iMax)

            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lPartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameShortName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_sShortName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameLongName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_sLongName
            'SP011298 - Amend keys to support roadmap

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameAgentCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_lPartyAgentCnt 'PN13921

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameClientCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_lPartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNameClientUIK

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_lPartyUIK

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.PMKeyNameClientName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = m_sLongName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = PMNavKeyConst.PMKeyNameClientCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = m_sShortName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = "agent_only"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = m_iAgentOnly

            ' CTAF 260900 - For Work Manager

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNameTaskCustomer

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = m_sLongName

            ' CTAF These keys need to go into PMNavKeyConst when its
            '      checked back in

            ' CTAF 260900 - Date of birth

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 10) = "date_of_birth"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 10) = m_vDateOfBirth

            ' CTAF 260900 - Swift Party ID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 11) = "swift_party_id"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 11) = m_lSwiftPartyID

            ' Type - amended TF281100
            If Not m_bPartyTypeOnly Then

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 12) = PMNavKeyConst.PMKeyNamePartyType

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 12) = m_vPartyType
            Else

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 12) = PMNavKeyConst.PMKeyNamePartyType

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 12) = m_sPartyType
            End If

            ' Status

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 13) = PMNavKeyConst.PMKeyNameStatus

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 13) = m_vPartyStatus

            ' Addresses

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 14) = PMNavKeyConst.PMKeyNameAddLine1

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 15) = PMNavKeyConst.PMKeyNameAddLine2

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 16) = PMNavKeyConst.PMKeyNameAddLine3

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 17) = PMNavKeyConst.PMKeyNameAddLine4

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 18) = PMNavKeyConst.PMKeyNamePostCode
            'DC140201 added for New Business Roadmap

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 19) = PMNavKeyConst.ACTKeyNameBranchID
            'DC260202

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 20) = PMNavKeyConst.PMKeyNamePartyResolvedName


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 14) = m_vAddLine1

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 15) = m_vAddLine2

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 16) = m_vAddLine3

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 17) = m_vAddLine4

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 18) = m_vPostCode
            'DC140201 added for New Business Roadmap

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 19) = g_iSourceID
            'DC260202

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 20) = m_sResolvedName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 21) = PMNavKeyConst.PMKeyNameAddressCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 21) = m_lAddressCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 22) = PMNavKeyConst.PMKeyNameHandlerType

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 22) = m_sSelectedPartyType


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 23) = PMNavKeyConst.PMKeyNameIsIncludeClosedBrancheChecked

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 23) = m_bIsIncludeClosedBrancheChecked


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 24) = PMNavKeyConst.PMKeyNamePartySourceId 'PN13921

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 24) = m_lPartySourceId 'PN13921


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 25) = PMNavKeyConst.PMKeyNameCountryId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 25) = m_lCountryId

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary (Standard Method)
    '
    ' Description: Stores all of the summary array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the summary array with the number of
            ' items needed to be returned.
            ' Note: Remember arrays are zero based.
            'SP011298 - Changes to support roadmap (if nowt to do return vSummarryArray="")
            ReDim vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0)

            ' Assign the key array with the parameter members.
            ' TF180199 - Set header to appropriate party type

            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummHeading, 0) = m_sSelectedPartyType

            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0) = m_sLongName

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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
                m_iTask = CInt(vTask)
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
    Private Function ProcessParties() As Integer

        Dim result As Integer = 0
        Dim oParties As frmParties



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get a new instance of frmParties
        oParties = New frmParties()
        ' Load it
        ' Show it
        oParties.ShowDialog()

        If oParties.Status = gPMConstants.PMEReturnCode.PMCancel Then
            ' Unload the object
            oParties.Close()
            ' Remove from memory
            oParties = Nothing
            ' Return False
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the party type
        m_sPartyType = oParties.PartyType

        ' Unload it
        oParties.Close()
        oParties = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer
        Dim result As Integer = 0
        Dim sValue As String = ""
        Const kMethodName As String = "Start"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim bRestrictedInsurerAccess As Boolean
            Dim sPartyTypeCode As String = ""

            m_bViewAuthority = True ' Client Manager Security
            m_bEditAuthority = True ' Client Manager Security
            m_bDeleteAuthority = True ' Client Manager Security

            ' Get System Option for Disable Wildcard Search
            m_lReturn = iPMFunc.GetSystemOption(kSystemOptionDisableWildcardSearch, sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOption for DisableWildcardSearch Failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If
            m_bDisableWildcardSearchOption = (sValue = "1")

            ' Get System Option for m_bEnablePartialWildcardSearchOption
            m_lReturn = iPMFunc.GetSystemOption(kSystemOptionEnablePartialWildcardSearch, sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOption for EnablePartialWildcardSearch Failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If
            m_bEnablePartialWildcardSearchOption = (sValue = "1")

            If m_bPartyTypeOnly Then
                m_lReturn = ProcessParties()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                Return result
                ' Client Manager Security
            Else
                m_lReturn = CheckSecurity(r_bViewAuthority:=m_bViewAuthority, r_bEditAuthority:=m_bEditAuthority, r_bDeleteAuthority:=m_bDeleteAuthority)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            m_lReturn = GetValidSources()
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            'Check to see if restricted insurer access is turned on
            m_lReturn = PartyFunc.GetHiddenOptions(v_lSourceId:=g_iSourceID, r_vRestrictedInsurerAccess:=bRestrictedInsurerAccess)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetHiddenOptions Failed for source_id = " & g_iSourceID, vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If bRestrictedInsurerAccess And m_lUserInsurerCnt > 0 Then

                'Restricted insurer access is turned on so check the party_cnt
                'from the pmuser table to see if it points to an insurer and
                'if it does then set restricted insurer access on

                m_lReturn = g_oBusiness.RestrictInsurerAccess(v_lUserInsurerCnt:=m_lUserInsurerCnt, r_bRestrictInsurerAccess:=m_bRestrictInsurerAccess)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="g_oBusiness.RestrictInsurerAccess Failed for party_cnt = " & m_lUserInsurerCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If m_bRestrictInsurerAccess Then
                    'We do nto want to filter by agent
                    m_lUserAgentCnt = 0
                End If
            End If

            m_sAgencyOrunderwriting = g_oBusiness.UnderwritingOrAgency

            ' Check if the PartyCnt is greater than zero. If so,
            ' there is no need to proceed with the interface. We
            ' can therefore return straight back out.
            If (m_lPartyCnt > 0) And (m_lSwiftPartyID = 0) Then
                ' ID is greater than zero.
                m_lStatus = gPMConstants.PMEReturnCode.PMOK

            Else
                ' Starts the interface processing.
                m_lReturn = ProcessInterface()

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

    ' ***************************************************************** '
    ' Name: GetDetailsFromPMBArray
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function GetDetailsFromPMBArray(ByVal v_lInvariantKey As Integer, Optional ByRef r_sClientCode As String = "", Optional ByRef r_sClientName As String = "", Optional ByRef r_sAddress1 As String = "", Optional ByRef r_sAddress2 As String = "", Optional ByRef r_sAddress3 As String = "", Optional ByRef r_sAddress4 As String = "", Optional ByRef r_sPostCode As String = "", Optional ByRef r_sPortfolio As String = "", Optional ByRef r_sCustomerId As String = "", Optional ByRef r_sStatus As String = "", Optional ByRef r_sTelNo As String = "", Optional ByRef r_sAltTelNo As String = "", Optional ByRef r_sDOB As String = "", Optional ByRef r_sSex As String = "", Optional ByRef r_sMarried As String = "", Optional ByRef r_sChildren As String = "") As Integer

        Dim result As Integer = 0

        Try
            Return g_oBusiness.GetDetailsFromPMBArray(v_lInvariantKey:=v_lInvariantKey, r_sClientCode:=r_sClientCode, r_sClientName:=r_sClientName, r_sAddress1:=r_sAddress1, r_sAddress2:=r_sAddress2, r_sAddress3:=r_sAddress3, r_sAddress4:=r_sAddress4, r_sPostCode:=r_sPostCode, r_sPortfolio:=r_sPortfolio, r_sCustomerId:=r_sCustomerId, r_sStatus:=r_sStatus, r_sTelNo:=r_sTelNo, r_sAltTelNo:=r_sAltTelNo, r_sDOB:=r_sDOB, r_sSex:=r_sSex, r_sMarried:=r_sMarried, r_sChildren:=r_sChildren)

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetailsFromPMBArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetailsFromPMBArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetID (Standard Method)
    '
    ' Description: Gets the ID for the search parameter from the
    '              business object.
    '
    ' ***************************************************************** '
    Public Function GetID(ByRef lId As Integer, Optional ByRef vShortName As Object = Nothing, Optional ByRef vName As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the ID from the busines object.

            m_lReturn = g_oBusiness.GetID(lId:=lId, vShortName:=vShortName, vName:=vName)

            ' Check for errors (PMNotFound=OK)
            If (m_lReturn = gPMConstants.PMEReturnCode.PMFalse) Or (m_lReturn = gPMConstants.PMEReturnCode.PMError) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the ID from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetID")
                Return result
            End If
            ' Return the value.
            Return m_lReturn

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the ID from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    'eck090500
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

        If m_lKeepOnTop = 1 Then
            'developer guide no. 69
            m_lReturn = iPMFunc.SetWindowPlacement(m_ofrmInterface.Handle.ToInt32(), True)
        End If

        If m_bSearchPartyWithShortCode Then
            m_ofrmInterface.txtShortName.Text = m_sShortName
            m_ofrmInterface.CallFindNow()
            m_ofrmInterface.CalllvwSearchDetailsClick()
            m_lReturn = ShowInterface(lDisplayState:=FormShowConstants.Modal)
        Else
            If Not m_bSkipFindParty Then
                ' Display the interface.
                m_lReturn = ShowInterface(lDisplayState:=FormShowConstants.Modal)
            Else
                m_ofrmInterface.txtShortName.Text = m_sShortName
                m_ofrmInterface.CallFindNow()
                m_ofrmInterface.CallOK()
            End If
        End If
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

        m_ofrmInterface = New iPMBFindParty.frmInterface()

        ' Assign the parameters to the interface properties.
        With m_ofrmInterface
            .EnableNewParty = m_bEnableNewParty
            .CallingAppName = m_sCallingAppName
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            ' {* USER DEFINED CODE (Begin) *}
            .ShortName = m_sShortName
            .AgentOnly = m_iAgentOnly
            .SpecialParty = m_sSpecialParty
            .InsuranceRef = m_sInsuranceRef
            .NotEditable = m_iNotEditable
            .DeleteMode = m_bDeleteMode
            If m_sSpecialParty.Length > 1 Then
                .PartyOther = (m_sSpecialParty.StartsWith("OT"))
            End If

            .SourceArray = m_vSourceArray
            'Passing array which will be used when chkbox IncludeClosedBranches is selected

            .SourceArrayIncludeClosedBranch = m_vSourceArrayIncludeClosedBranch
            .SwiftPartyID = m_lSwiftPartyID
            .PartyType = m_vPartyType
            .AddressLine1 = m_vAddLine1
            .Postcode = m_vPostCode
            .PartyStatus = m_vPartyStatus
            .LongName = m_sLongName
            .DateOfBirth = m_vDateOfBirth
            .IgnoreDriversAndWitnesses = m_bIgnoreDriversAndWitnesses
            .NavStep = m_vNavStep
            .UserAgentCnt = m_lUserAgentCnt
            .RestrictInsurerAccess = m_bRestrictInsurerAccess
            ' PW190802 allow to suppress sub agents
            .SuppressSubAgents = m_bSuppressSubAgents
            .IsInTransferMode = m_bIsInTransferMode
            .AllowAddressSelection = m_bAllowAddressSelection
            .AddressCnt = m_lAddressCnt
            .ValidPartyTypesArray = m_vValidPartyTypesArray
            .DateCancelled = m_vDateCancelled
            .PartySourceId = m_lPartySourceId
            .IsComplaint = m_iIsComplaint
            .DPARequired = m_bDPARequired
            .IncludeClosedBranches = m_bIncludeClosedBranches
            .AgentTypes = m_vAgentTypes
            .IgnoreDPAQuestions = m_bIgnoreDPAQuestions
            .SuppressCancelledAgents = m_bSuppressCancelledAgents
            .RiskTransfer = m_bRiskTransfer
            .BranchID = m_iSourceID ''Agent Filtering
            .AllowAgentSearch = m_bAllowAgentSearch
            .TelephoneAreaCode = m_sPartyTelephonePrefix
            .TelephoneNumber = m_sPartyTelephoneNumber

            If (Not String.IsNullOrEmpty(m_sPartyAutoSearch)) Then
                If m_sPartyAutoSearch.Substring(0, 1).ToLower() = "y" Then
                    .DoAutoSearch()
                End If
            End If

            .IsIntroducerOnly = m_bIntroducerOnly
            .ExcludeMultiInsurer = m_bExcludeMultiInsurer 'PN29952

            .IsRetained = m_bIsRetained
            .RetainedValue = m_iRetainedValue
            .ReinsuranceTypeArray = m_sReinsuranceTypeArray
            .BureauAccount = m_iBureauAccount
            .ViewAuthority = m_bViewAuthority ' Client Manager Security
            .EditAuthority = m_bEditAuthority ' Client Manager Security
            .DeleteAuthority = m_bDeleteAuthority ' Client Manager Security
            .DisableWildcardSearchOption = m_bDisableWildcardSearchOption
            .EnablePartialWildcardSearchOption = m_bEnablePartialWildcardSearchOption
            .CommissionLevel = m_lCommissionlevel
            .TreatyPartiesBrokerParticipantsForDisplay = m_vTreatyPartiesBrokerParticipantForDisplay
            .CurrencyId = m_iCurrencyID
        End With

        ' Load the instance of the interface into memory.
        ' Check if we have had an error so far.
        If m_ofrmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = m_ofrmInterface.ErrorNumber
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name:         ShowAddressInterface
    ' Description:  -
    ' Author:       Alix Bergeret
    ' Date:         07/11/2002
    ' ***************************************************************** '
    Public Function ShowAddressInterface() As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim objItem As ListViewItem
        Dim lAddressCnt_Temp As Integer
        Dim bOtherThanCorrAddress As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get data from business layer
            m_lReturn = g_oBusiness.GetMultipleAddresses(m_lPartyCnt, vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            Else

                ' Load form in memory
                'developer guide no. 69
                Dim frmMultiAddresses As frmMultiAddresses = New frmMultiAddresses

                ' Loop throught addresses
                bOtherThanCorrAddress = False

                For iIndex As Integer = 0 To vResultArray.GetUpperBound(1)

                    ' Populate it

                    If CDbl(vResultArray(7, iIndex)) <> 4 Then
                        bOtherThanCorrAddress = True
                    Else
                        ' keep correspondance address cnt

                        lAddressCnt_Temp = CInt(vResultArray(0, iIndex))
                    End If


                    objItem = frmMultiAddresses.lvwAddresses.Items.Insert(0, CStr(vResultArray(2, iIndex)))

                    objItem.Tag = CStr(vResultArray(0, iIndex)).Trim()

                    ListViewHelper.GetListViewSubItem(objItem, 1).Text = CStr(vResultArray(3, iIndex))

                    ListViewHelper.GetListViewSubItem(objItem, 2).Text = CStr(vResultArray(4, iIndex))

                    ListViewHelper.GetListViewSubItem(objItem, 3).Text = CStr(vResultArray(5, iIndex))

                    ListViewHelper.GetListViewSubItem(objItem, 4).Text = CStr(vResultArray(6, iIndex))

                    ' select address if its id was passed it
                    If m_lAddressCnt <> 0 Then
                        If CStr(m_lAddressCnt).Trim() = Convert.ToString(objItem.Tag) Then
                            objItem.Selected = True
                        End If
                    End If

                Next iIndex

                If bOtherThanCorrAddress Then

                    ' Show form
                    frmMultiAddresses.ShowDialog()

                    ' Form has been OKed or Canceled
                    If Convert.ToString(frmMultiAddresses.lvwAddresses.Tag) = "True" Then
                        m_lAddressCnt = Convert.ToString(frmMultiAddresses.lvwAddresses.FocusedItem.Tag)
                    Else
                        m_lAddressCnt = lAddressCnt_Temp
                    End If

                Else
                    ' No branch address, return correspondance address
                    m_lAddressCnt = lAddressCnt_Temp
                End If

                ' Unload form
                frmMultiAddresses.Close()

                objItem = Nothing
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display address interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowAddressInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
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
            m_lPartyCnt = .PartyCnt
            m_sShortName = .ShortName
            m_sLongName = .LongName
            m_sResolvedName = .ResolvedName
            m_lPartyUIK = .PartyUIK
            m_sSelectedPartyType = .SelectedPartyType
            m_iAgentOnly = .AgentOnly
            m_lInvariantKey = .InvariantKey
            m_lSwiftPartyID = .SwiftPartyID
            m_vDateOfBirth = .DateOfBirth
            m_vPartyType = .PartyType
            m_vPartyStatus = .PartyStatus
            m_vAddLine1 = .AddressLine1
            m_vAddLine2 = .AddressLine2
            m_vAddLine3 = .AddressLine3
            m_vAddLine4 = .AddressLine4
            m_vPostCode = .Postcode
            m_lCountryId = .CountryId 'MKW150606
            m_lAddressCnt = .AddressCnt
            m_vDateCancelled = .DateCancelled
            m_bIsIncludeClosedBrancheChecked = .IsIncludeClosedBranchChecked
            m_lPartyAgentCnt = .PartyAgentCnt 'PN13921
            m_lPartySourceId = .PartySourceId 'PN13921
            m_vBrokerArray = .BrokerArray
            m_iCurrencyID = .CurrencyId
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

        If m_lKeepOnTop = 1 Then
            m_lReturn = iPMFunc.SetWindowPlacement(m_ofrmInterface.Handle.ToInt32(), True)
        End If

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
        Try
            g_iRefCounter += 1

        Catch excep As System.Exception
            ' Log Error Message
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", excep:=excep)

            Exit Sub

        End Try
    End Sub

    Protected Overrides Sub Finalize()
        g_iRefCounter -= 1
        Dispose(False)
    End Sub

    Public Function FSACustomerVal(ByVal lPartyCnt As Integer, ByVal sPartyType As String, ByRef r_bProceed As Boolean) As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If sPartyType = "P" Then
                sPartyType = "PC"
            End If

            If sPartyType = "C" Then
                sPartyType = "CC"
            End If

            m_lReturn = FSACustomerValidate(lPartyCnt, sPartyType, m_iIsComplaint, r_bProceed)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed FSA Customer Validate.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPartyInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="FSACustomerVal", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result
            End Select

        Finally

        End Try
        Return result
    End Function
    ' PRIVATE Methods (Begin)
    'eck090500
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
        'David Kyle Thing
        'Call PMUser to get the Sources
        ' Get an instance of the business object via
        ' the public object manager.
        'developer guide no. 9(guide)
        g_oObjectManager = New bObjectManager.ObjectManager()
        result = g_oObjectManager.Initialise(ACApp)
        Dim temp_g_oPMUser As Object = Nothing
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
        'eck150600
        If m_sSpecialParty = "OT" Then

            m_lReturn = g_oPMUser.GetUserSources(r_vSourceArray:=m_vSourceArray, v_vUserID:=g_iUserID)
            m_lReturn = g_oPMUser.GetUserSources(r_vSourceArray:=m_vSourceArrayIncludeClosedBranch, v_vUserID:=g_iUserID, v_bIncludeDeletedSources:=True)
        ElseIf m_sSpecialParty = "AG" Or m_sSpecialParty = "UB" Then

            m_lReturn = g_oPMUser.GetUserSources(r_vSourceArray:=m_vSourceArray, v_vUserID:=g_iUserID)
            m_lReturn = g_oPMUser.GetUserSources(r_vSourceArray:=m_vSourceArrayIncludeClosedBranch, v_vUserID:=g_iUserID, v_bIncludeDeletedSources:=True)
            ''Agent Filtering
        ElseIf m_sSpecialParty <> "" Then

            m_lReturn = g_oPMUser.GetAllSources(r_vSourceArray:=m_vSourceArray)

            m_lReturn = g_oPMUser.GetAllSources(r_vSourceArray:=m_vSourceArrayIncludeClosedBranch, v_bIncludeDeletedSources:=True)
        Else

            m_lReturn = g_oPMUser.GetUserSources(r_vSourceArray:=m_vSourceArray, v_vUserID:=g_iUserID)

            'One more Array for holding data which includes even Closed branch
            m_lReturn = g_oPMUser.GetUserSources(r_vSourceArray:=m_vSourceArrayIncludeClosedBranch, v_vUserID:=g_iUserID, v_bIncludeDeletedSources:=True)

        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get valid sources", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources", excep:=New Exception(Information.Err().Description))

            Return result
        End If

        'sj 06/06/2002 - start
        'Get the party_cnt field for this user
        'If it is not zero then this is the agent for this user and the user will
        'only be allowed to look at clients which are allocated against this agent
        If m_sSpecialParty = "" Then

            m_lReturn = g_oPMUser.GetDetails(vUserId:=g_iUserID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="g_oPMUser.GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources")

                Return result
            End If

            g_oPMUser.CurrentRecord = 0
            m_lReturn = g_oPMUser.GetNext(vPartyCnt:=m_lUserAgentCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="g_oPMUser.GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources")

                Return result
            End If
            m_lUserInsurerCnt = m_lUserAgentCnt
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

    Public Function GetAgentTypes() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = g_oBusiness.GetVisibleAgentTypes(r_vVisibleAgentTypes:=m_vAgentTypes)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed Getting Party Third Party Types", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAgentTypes", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If
            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyAgentTypes", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result
            End Select

        Finally


        End Try
        Return result

    End Function
End Class

