Option Strict Off
Option Explicit On
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
    ' Date: 07/10/1998
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History: TF071098 - Created from iFindInsurance
    ' ***************************************************************** '
    'DEEPAK_COMMENT: Replaced iPMFunc.GetResData with GetResData in the whole document

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"


    Private m_sCallingAppName As String = ""
    Private m_sPMAuthorityLevel As String = ""
    Private m_iStatus As Integer

    Private m_iTask As Integer
    Private m_iNavigate As Integer
    Private m_iProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_iPartyCnt As Integer
    Private m_sShortName As String = ""
    Private m_sLongName As String = ""
    Private m_sResolvedName As String = ""
    Private m_iAgentOnly As Integer
    Private m_bIntroducerOnly As Boolean
    Private m_sSpecialParty As String = ""
    Private m_iPartyUIK As Integer
    Private m_sInsuranceRef As String = ""
    Private m_sSelectedPartyType As String = ""
    Private m_iPartySourceId As Integer
    Private m_iPartyAgentCnt As Integer 'PN13921
    Private m_bAllowAddressSelection As Boolean
    Private m_iAddressCnt As Integer
    Private m_iNotEditable As Integer
    Private m_vSourceArray(,) As Object
    Private m_bExcludeMultiInsurer As Boolean


    'Array for all sourceid even closed
    Private m_vSourceArrayIncludeClosedBranch As Object

    Private m_bViewAuthority As Boolean '2005 Client Manager Security
    Private m_bEditAuthority As Boolean '2005 Client Manager Security
    Private m_bDeleteAuthority As Boolean '2005 Client Manager Security

    Private m_vDateOfBirth As String = ""
    Private m_iSwiftPartyID As Integer
    Private m_vDateCancelled As String = ""
    Private m_vPartyType As String = ""
    Private m_vPostCode As String = ""
    Private m_vPartyStatus As String = ""
    Private m_vAddLine1 As String = ""
    Private m_vAddLine2 As String = ""
    Private m_vAddLine3 As String = ""
    Private m_vAddLine4 As String = ""
    Private m_vNavStep As String = ""
    Private m_bSuppressSubAgents As Boolean

    Private m_bIsInTransferMode As Boolean

    Private m_iSourceID As Integer 'Agent Filtering

    ' Stores the return value for the a
    ' function call.
    Private m_iReturn As Integer

    ' Member to hold instance of form interface
    Private objfrmInterface As iACTAgentSelect.frmInterface

    Private m_bPartyTypeOnly As Boolean
    ' Party type to return
    Private m_sPartyType As String = ""

    Private m_bDeleteMode As Boolean

    Private m_iInvariantKey As Integer
    Private m_bIgnoreDriversAndWitnesses As Boolean
    Private m_iUserAgentCnt As Integer
    Private m_iUserInsurerCnt As Integer
    Private m_bRestrictInsurerAccess As Boolean
    Private m_bNeedFullAddress As Boolean
    Private m_vValidPartyTypesArray() As Object
    Private m_iIsComplaint As Integer
    Private m_bDPARequired As Boolean
    Private m_bIncludeClosedBranches As Boolean
    Private m_bIsIncludeClosedBrancheChecked As Boolean
    Private m_vAgentTypes As Object
    Private m_sAgencyOrunderwriting As String = ""
    Private m_bIgnoreDPAQuestions As Boolean
    Private m_bEnableNewParty As Boolean
    Private m_bSuppressCancelledAgents As Boolean
    Private m_iCountryId As Integer
    Private m_bRiskTransfer As Boolean
    Private m_iBureauAccount As Integer
    Private m_bIsRetained As Boolean
    Private m_iRetainedValue As Integer
    Private m_sReinsuranceTypeArray As String = ""
    Private m_sPartyTelephonePrefix As String = ""
    Private m_sPartyTelephoneNumber As String = ""
    Private m_sPartyAutoSearch As String = ""
    Private m_iKeepOnTop As Integer
    Private m_vBrokerArray As Object
    Private m_bAllowAgentSearch As Boolean
    Private m_bSkipFindParty As Boolean
    Private m_bDisableWildcardSearchOption As Boolean
    Private m_bEnablePartialWildcardSearchOption As Boolean
    Private Const kSystemOptionDisableWildcardSearch As Integer = 5065
    Private Const kSystemOptionEnablePartialWildcardSearch As Integer = 5066
    Private m_bSearchPartyWithShortCode As Boolean
    Private m_iCommissionlevel As Integer
    Public WriteOnly Property CommissionLevel() As Integer
        Set(ByVal Value As Integer)
            m_iCommissionlevel = Value
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

    ' Gaurav
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
            Return m_iUserInsurerCnt
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
            Return m_iInvariantKey
        End Get
        Set(ByVal Value As Integer)
            m_iInvariantKey = Value
        End Set
    End Property
    'sj 3/11/99 - end
    ' PRIVATE Data Members (End)

    ' Alix - 07/11/2002
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
            Return m_iAddressCnt
        End Get
        Set(ByVal Value As Integer)
            m_iAddressCnt = Value
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
            m_sPMAuthorityLevel = CStr(Value)
        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_iStatus

        End Get
    End Property
    ' {* USER DEFINED CODE (Begin) *}
    Public Property PartyCnt() As Integer
        Get

            Return m_iPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_iPartyCnt = Value

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

            Return m_iPartyUIK

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
            Return m_iSwiftPartyID
        End Get
        Set(ByVal Value As Integer)
            m_iSwiftPartyID = Value
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

            Return m_iPartySourceId

        End Get
        Set(ByVal Value As Integer)

            m_iPartySourceId = Value

        End Set
    End Property

    Public Property PartyAgentCnt() As Integer
        Get 'PN13921
            Return m_iPartyAgentCnt
        End Get
        Set(ByVal Value As Integer) 'PN13921
            m_iPartyAgentCnt = Value
        End Set
    End Property
    Public WriteOnly Property ExcludeMultiInsurer() As Boolean
        Set(ByVal Value As Boolean) 'PN29952
            m_bExcludeMultiInsurer = Value
        End Set
    End Property

    Public WriteOnly Property IntroducerOnly() As Boolean
        Set(ByVal Value As Boolean)
            m_bIntroducerOnly = Value
        End Set
    End Property

    'Start - Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling

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

    'Public Property Get ProductID() As Long
    '
    '    ProductID = m_lProductID&
    '
    'End Property
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
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            MainModule.g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_iReturn = MainModule.g_oObjectManager.Initialise(MainModule.ACApp)

            ' Check for errors.
            If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                MainModule.g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With MainModule.g_oObjectManager
                MainModule.g_iLanguageID = .LanguageID
                MainModule.g_iSourceID = .SourceID
                'eck220500
                g_iUserID = .UserID
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_iNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_iProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Initialise the Insurance Holder Count in case it isn't set

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oBusiness As Object
            m_iReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bSIRFindParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oBusiness = temp_g_oBusiness

            ' Check for errors.
            If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.ACBusinessFailTitle, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.ACBusinessFail, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            End If

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
                If Not (MainModule.g_oBusiness Is Nothing) Then
                    MainModule.g_oBusiness.Dispose()
                    MainModule.g_oBusiness = Nothing
                End If
                If MainModule.g_oObjectManager IsNot Nothing Then
                    MainModule.g_oObjectManager.Dispose()
                    MainModule.g_oObjectManager = Nothing
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
    ' Edit History  :
    ' RAM20040226   : Added the PMKeyNameRunMode Key (Set from uctListEventControl.ocx)
    '                  Ref. PN Issue 10592
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
            m_bSavedata = False
            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.

                ' {* USER DEFINED CODE (Begin) *}
                'SP011298 - changes to support new business roadmap


                Select Case Convert.ToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    'MEvans : 13-12-2002 : 202
                    Case "autosearch"
                        m_bSavedata = True
                    Case PMNavKeyConst.PMKeyNameAllowAddressSelection

                        m_bAllowAddressSelection = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        'End MEvans : 13-12-2002 : 202

                    Case PMNavKeyConst.PMKeyNamePartyCnt

                        m_iPartyCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameShortName

                        m_sShortName = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case "agent_only"

                        m_iAgentOnly = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        'ECK 270499
                    Case "special_party"

                        m_sSpecialParty = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        ' TF140199
                    Case PMNavKeyConst.PMKeyNameInsReference

                        m_sInsuranceRef = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNamePolicyNo

                        m_sInsuranceRef = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        ' CF261099
                    Case PMNavKeyConst.PMKeyNamePartiesOnly

                        m_bPartyTypeOnly = (CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)).ToUpper() = "YES")

                        'RKS 141004 PN13238 & PN14838
                    Case PMNavKeyConst.PMKeyNameIncludeClosedBranches

                        m_bIncludeClosedBranches = (CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)).ToUpper() = "YES")
                        'Tomo140100
                    Case "delete_mode"

                        m_bDeleteMode = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        ' CTAF 260900
                    Case "swift_party_id"

                        m_iSwiftPartyID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        ' CTAF 171100 - Swift requirement
                    Case PMNavKeyConst.PMKeyNamePartyType

                        m_vPartyType = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        ' CTAF 171100 - Swift requirement
                    Case PMNavKeyConst.PMKeyNameStatus
                        m_vPartyStatus = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        ' CTAF 171100 - Swift requirement
                    Case PMNavKeyConst.PMKeyNamePartyLongName

                        m_sLongName = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        ' CTAF 171100 - Swift requirement
                    Case PMNavKeyConst.PMKeyNameDOB
                        If Information.IsDate(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) Then

                            m_vDateOfBirth = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If

                        ' CTAF 171100 - Swift requirement
                    Case PMNavKeyConst.PMKeyNameAddLine1

                        m_vAddLine1 = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        ' CTAF 171100 - Swift requirement
                    Case PMNavKeyConst.PMKeyNamePostCode

                        m_vPostCode = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        'DC260202
                    Case PMNavKeyConst.PMKeyNameNavStep

                        m_vNavStep = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        'DC160703 -ISS5384
                    Case PMNavKeyConst.PMKeyNameSourceId

                        m_iPartySourceId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        'MKW080104 PN9424 Include Complaints FSA reason.
                    Case PMNavKeyConst.PMKeyNameIncludeComplaints

                        m_iIsComplaint = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        ' SET 08/06/2004 ISS11882
                    Case PMNavKeyConst.PMKeyNameAskDPAQuestions

                        m_bDPARequired = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) = 1

                    Case "risk_transfer_agreement"

                        m_bRiskTransfer = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        ' R.Griffiths 2006-10-16 (Plus One)
                    Case PMNavKeyConst.PMKeyNamePartyTelephonePrefix

                        m_sPartyTelephonePrefix = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNamePartyTelephoneNumber

                        m_sPartyTelephoneNumber = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNamePartyAutoSearch

                        m_sPartyAutoSearch = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        ' R.Griffiths End

                    Case PMNavKeyConst.PMKeyNameKeepWindowOnTop

                        m_iKeepOnTop = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyBureauAccount

                        m_iBureauAccount = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case "include_agent"

                        m_bAllowAgentSearch = gPMFunctions.ToSafeBoolean(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    Case CStr(m_iCommissionlevel)
                        'UPGRADE_WARNING: Couldn't resolve default property of object vKeyArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        m_iCommissionlevel = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                End Select

                ' {* USER DEFINED CODE (End) *}

            Next lRow

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="SetKeys", r_lFunctionReturn:=result, excep:=excep)
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

            'iMax = 25
            iMax = 0

            ReDim vKeyArray(1, iMax)
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = AgentSelect
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_vselecteditemArray










            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 14) = PMNavKeyConst.PMKeyNameAddLine1

            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 15) = PMNavKeyConst.PMKeyNameAddLine2

            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 16) = PMNavKeyConst.PMKeyNameAddLine3

            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 17) = PMNavKeyConst.PMKeyNameAddLine4

            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 18) = PMNavKeyConst.PMKeyNamePostCode
            ''DC140201 added for New Business Roadmap

            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 19) = PMNavKeyConst.ACTKeyNameBranchID
            ''DC260202

            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 20) = PMNavKeyConst.PMKeyNamePartyResolvedName


            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 14) = m_vAddLine1

            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 15) = m_vAddLine2

            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 16) = m_vAddLine3

            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 17) = m_vAddLine4

            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 18) = m_vPostCode
            ''DC140201 added for New Business Roadmap

            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 19) = g_iSourceID
            ''DC260202

            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 20) = m_sResolvedName

            ''MEvans : 13-12-2002 : 202

            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 21) = PMNavKeyConst.PMKeyNameAddressCnt

            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 21) = m_lAddressCnt
            ''MEvans : 13-12-2002 : 202

            ''DC151003 -PN7449

            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 22) = PMNavKeyConst.PMKeyNameHandlerType

            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 22) = m_sSelectedPartyType

            ''JT PN-13238 28-10-2004

            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 23) = PMNavKeyConst.PMKeyNameIsIncludeClosedBrancheChecked

            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 23) = m_bIsIncludeClosedBrancheChecked


            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 24) = PMNavKeyConst.PMKeyNamePartySourceId 'PN13921

            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 24) = m_lPartySourceId 'PN13921

            ''MKW 150606

            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 25) = PMNavKeyConst.PMKeyNameCountryId

            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 25) = m_lCountryId

            ' CF261099 - Return party type if needed
            ' TF281100 - not needed
            '    If (m_bPartyTypeOnly = True) Then
            '        vKeyArray(PMKeyName, iMax) = PMKeyNamePartyType
            '        vKeyArray(PMKeyValue, iMax) = m_sPartyType$
            '    End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="GetKeys", r_lFunctionReturn:=result, excep:=excep)
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
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="GetSummary", r_lFunctionReturn:=result, excep:=excep)
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

                m_iNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_iProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            ' Set the process modes for the business object.
            If MainModule.g_oBusiness Is Nothing Then

                m_iReturn = g_oBusiness.SetProcessModes(vTask:=vTask, vNavigate:=vNavigate, vProcessMode:=vProcessMode, vTransactionType:=vTransactionType, vEffectiveDate:=vEffectiveDate)

                ' Check for errors.
                If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.

                    ' Log Error Message
                    iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="SetProcessModes", r_lFunctionReturn:=result)
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="SetProcessModes", r_lFunctionReturn:=result, excep:=excep)
            Return result

        End Try
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

            Dim sPartyTypeCode As String = ""


            ' Get System Option for Disable Wildcard Search
            m_iReturn = iPMFunc.GetSystemOption(kSystemOptionDisableWildcardSearch, sValue)
            If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOption for DisableWildcardSearch Failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If
            m_bDisableWildcardSearchOption = (sValue = "1")

            ' Get System Option for m_bEnablePartialWildcardSearchOption
            m_iReturn = iPMFunc.GetSystemOption(kSystemOptionEnablePartialWildcardSearch, sValue)
            If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOption for EnablePartialWildcardSearch Failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If
            m_bEnablePartialWildcardSearchOption = (sValue = "1")
            'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.3.1.2)

            m_iReturn = GetValidSources()
            ' Check for errors.
            If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            'eck090500

            ' CTAF 090401 - If SwiftpartyID <> 0 then show the interface

            m_sAgencyOrunderwriting = g_oBusiness.UnderwritingOrAgency


            m_iReturn = ProcessInterface()

            ' Check for errors.
            If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="Start", r_lFunctionReturn:=result, excep:=excep)
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
    Public Function GetID() As Integer

        Dim result As Integer = 0

        Try


            ' Get the ID from the busines object.
            ' TF031298 - This function does not exist on Business
            '    m_iReturn& = g_oBusiness.GetID( _
            'vSearch:=CVar(m_sInsReference$), _
            'vID:=CVar(m_lInsFileCnt&))

            ' Check for errors
            '    If (m_iReturn& = PMFalse Or _
            ''    m_iReturn& = PMError) Then
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="Failed to get the ID from the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GetID"
            '    End If

            ' Return the value.
            '    GetID = m_iReturn&

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="GetID", r_lFunctionReturn:=result, excep:=excep)
            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

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
        m_iReturn = CType(LoadInterface(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Display the interface.
        m_iReturn = CType(ShowInterface(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'Check if the interface is already open
            If m_iReturn = gPMConstants.PMEReturnCode.PMMoveStatusBack Then
                Return 400
            Else
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        'developer guide no. 51

        m_sShortName = objfrmInterface.ShortName
        m_sLongName = objfrmInterface.LongName

        ' Destroy the interface from memory.
        m_iReturn = CType(UnLoadInterface(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
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
    ' Edit History  :
    ' RAM20040226   : PN Issue 10592 Changes
    ' ***************************************************************** '
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        objfrmInterface = New iACTAgentSelect.frmInterface
        ' Assign the parameters to the interface properties.
        'developer guide no. 51
        With objfrmInterface
            .CallingAppName = m_sCallingAppName
            .Navigate = m_iNavigate
            .ProcessMode = m_iProcessMode
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
            'developer guide no. 24
            .SourceArray = m_vSourceArray
            'PN-15885   JT    25-10-2004
            'Passing array which will be used when chkbox IncludeClosedBranches is selected


            'developer guide no. 24
            .SourceArrayIncludeClosedBranch = m_vSourceArrayIncludeClosedBranch
            .SwiftPartyID = m_iSwiftPartyID
            .PartyType = m_vPartyType
            .AddressLine1 = m_vAddLine1
            .Postcode = m_vPostCode
            .PartyStatus = m_vPartyStatus
            .LongName = m_sLongName
            .DateOfBirth = m_vDateOfBirth
            .IgnoreDriversAndWitnesses = m_bIgnoreDriversAndWitnesses
            .NavStep = m_vNavStep
            .UserAgentCnt = m_iUserAgentCnt
            .RestrictInsurerAccess = m_bRestrictInsurerAccess
            ' PW190802 allow to suppress sub agents
            .SuppressSubAgents = m_bSuppressSubAgents
            .IsInTransferMode = m_bIsInTransferMode
            ' Alix - 07/11/2002
            .AllowAddressSelection = m_bAllowAddressSelection
            .AddressCnt = m_iAddressCnt
            'developer guide no. 24
            .ValidPartyTypesArray = m_vValidPartyTypesArray

            'MSB 03/03/2003
            .DateCancelled = m_vDateCancelled

            'DC160703 -ISS5384
            .PartySourceId = m_iPartySourceId

            'MKW080104 PN9424 Include Complaint in FSA reasons
            .IsComplaint = m_iIsComplaint

            ' SET 08/06/2004 ISS11882
            .DPARequired = m_bDPARequired

            'RKS 141004 PN13238 & PN14838
            .IncludeClosedBranches = m_bIncludeClosedBranches

            'DC101204


            'developer guide no.24
            .AgentTypes = m_vAgentTypes
            .IgnoreDPAQuestions = m_bIgnoreDPAQuestions
            .SuppressCancelledAgents = m_bSuppressCancelledAgents
            .RiskTransfer = m_bRiskTransfer
            .BranchID = m_iSourceID ''Agent Filtering
            .AllowAgentSearch = m_bAllowAgentSearch
            ' R.Griffiths 2006-10-16 (Plus One)
            .TelephoneAreaCode = m_sPartyTelephonePrefix
            .TelephoneNumber = m_sPartyTelephoneNumber
            ' R.Griffiths End
            .IsIntroducerOnly = m_bIntroducerOnly
            .ExcludeMultiInsurer = m_bExcludeMultiInsurer 'PN29952

            .IsRetained = m_bIsRetained
            .RetainedValue = m_iRetainedValue
            .ReinsuranceTypeArray = m_sReinsuranceTypeArray
            .BureauAccount = m_iBureauAccount
            .ViewAuthority = m_bViewAuthority ' Client Manager Security
            .EditAuthority = m_bEditAuthority ' Client Manager Security
            .DeleteAuthority = m_bDeleteAuthority ' Client Manager Security

            'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.3.1.3)
            .DisableWildcardSearchOption = m_bDisableWildcardSearchOption
            .EnablePartialWildcardSearchOption = m_bEnablePartialWildcardSearchOption
            'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.3.1.3)
            'SAGICOR WPR14
            .CommissionLevel = m_iCommissionlevel

        End With

        ' Load the instance of the interface into memory.
        'developer guide no.  commented refer guide no. 51 
        'Dim tempLoadForm As frmInterface = frmInterface

        ' Check if we have had an error so far.
        'developer guide no. 51 
        If objfrmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = objfrmInterface.ErrorNumber
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
        'developer guide no. 51
        With objfrmInterface
            m_iStatus = .Status

            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}
        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        'developer guide no. 51

        objfrmInterface.Close()
        objfrmInterface = Nothing


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
        'Modified by Sumeet Singh on 6/1/2010 6:49:48 PM commented the code because it has to be global so that the objfrmInterface object is available to all functions.
        'objfrmInterface = New frmInterface
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'For Each frm As Object In Application.OpenForms
            '    If Not frm Is Nothing Then
            '        If frm.Text = "Find: Insurance File" OrElse frm.GetType().ToString() = objfrmInterface.GetType().ToString() Then
            '            Return 400
            '        End If
            '    End If
            'Next
            ' Display the interface.
            ' developer guide no. 51
            VB6.ShowForm(objfrmInterface, lDisplayState)

            If lDisplayState = FormShowConstants.Modal Then
                ' Check for any form errors.
                'developer guide no. 51
                If objfrmInterface.ErrorNumber <> 0 Then
                    result = objfrmInterface.ErrorNumber
                End If
            End If

            Return result

        Catch excep As System.Exception
            'Check if the interface is already open
            If Information.Err().Number = 400 Then
                Return 400
            Else
                result = gPMConstants.PMEReturnCode.PMError
            End If

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="ShowInterface", r_lFunctionReturn:=result, excep:=excep)
            Return result

        End Try
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

    Private Function GetValidSources() As Integer
        Dim result As Integer = 0
        Dim sAgencyOrUnderwriting As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue
        'David Kyle Thing
        'Call PMUser to get the Sources
        ' Get an instance of the business object via
        ' the public object manager.

        Dim temp_g_oPMUser As Object
        m_iReturn = g_oObjectManager.GetInstance(temp_g_oPMUser, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        g_oPMUser = temp_g_oPMUser

        '    ' Check for errors.
        If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get an instance of the business object.
            result = gPMConstants.PMEReturnCode.PMFalse

            '        ' Display error stating the problem.

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bPMUser.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources", excep:=New Exception(Information.Err().Description))

            Return result
        End If

        m_iReturn = iPMFunc.getUnderwritingOrAgency(r_vUnderwriting:=sAgencyOrUnderwriting)
        'eck150600

        m_iReturn = g_oPMUser.GetUserSources(r_vSourceArray:=m_vSourceArray, v_vUserID:=g_iUserID)

        m_iReturn = g_oPMUser.GetUserSources(r_vSourceArray:=m_vSourceArrayIncludeClosedBranch, v_vUserID:=g_iUserID, v_bIncludeDeletedSources:=True)

        If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get valid sources", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources", excep:=New Exception(Information.Err().Description))

            Return result
        End If

        '    ' Remove instance of PMUser
        If Not (g_oPMUser Is Nothing) Then

            'm_iReturn = g_oPMUser.Terminate()
            g_oPMUser.Dispose()
            g_oPMUser = Nothing
        End If

        Return result

    End Function
End Class