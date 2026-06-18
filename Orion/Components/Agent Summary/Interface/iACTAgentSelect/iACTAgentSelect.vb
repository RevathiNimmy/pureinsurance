Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Windows.Forms
Imports System.Globalization
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 07/10/1998
    '
    ' Description: Main interface.
    '
    ' Edit History: TF071098 - Created from iFindInsurance
    ' ED 05082002 : Code added to search SBO Policy based on the Front Office
    '               data based on the registry setting, whethere Carole Nash
    '               Search is activated
    ' ***************************************************************** '
    'DEEPAK_COMMENT: Replaced iPMFunc.GetResData with GetResData in the whole document
    Private Const vbFormCode As Integer = 0
    Public Event SourceIdChange()
    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

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
    Private m_sAddressLine1 As String = ""
    Private m_sAddressLine2 As String = ""
    Private m_sAddressLine3 As String = ""
    Private m_sAddressLine4 As String = ""
    Private m_sPostalCode As String = ""
    Private m_sTelAreaCode As String = ""
    Private m_sTelNumber As String = ""
    Private m_sInsuranceRef As String = ""
    Private m_sSelectedPartyType As String = ""
    Private m_sStructure As String = ""
    Private m_bPartyOther As Boolean
    Private m_iPartySourceId As Integer
    Private m_bAllowAddressSelection As Boolean
    Private m_lAddressCnt As Integer
    Private m_vDateCancelled As String = ""
    Private m_sFileCode As String = ""
    Private m_vDateOfBirth As String = ""
    Private m_iSwiftPartyID As Integer
    Private m_bDOBEnabled As Boolean
    Private m_vFullAddress As Object
    Private m_iNotEditable As Integer

    Private m_bDeleteMode As Boolean
    Private m_vSourceArray(,) As Object
    'Array for holding data for include Closed branch as well
    Private m_vSourceArrayIncludeClosedBranch As Object
    'Var for Checking that whether Include Closed Branch Checkbox is Checked or Not
    Private m_bIsIncludeClosedBranchChecked As Boolean
    Private m_bSwiftEnabled As Boolean
    Private m_iAllowConsolidatedCommission As Integer
    Private m_oGeneral As iACTAgentSelect.General
    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookUpDetails(,) As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Declare an instance of the Lock object.
    Private m_oPMLock As Object

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the search data from the business object.
    Public m_vSearchData As Object
    ' PRIVATE Data Members (End)
    'sj 3/11/99 - start
    Private m_iInvariantKey As Integer

    'TN20000918
    Private m_sAgencyOrunderwriting As String = ""

    Private m_bSpecialPartyQuery As Boolean 'CT 10/08/00

    'MSS200901 - Merge Start
    Private m_bIgnoreDriversAndWitnesses As Boolean
    Private m_vOtherPartyTypes As Object
    'MSS200901 - Merge End

    ' PartyType
    Private m_sPartyType As String = ""
    ' PartyLongName
    Private m_sPartyLongName As String = ""
    ' PartyStatus
    Private m_sPartyStatus As String = ""
    Private m_vNavStep As String = ""
    'JMK 18/10/2001
    Private m_sUnderwritingType As String = ""
    'sj 06/06/2002 - start
    Private m_bIsNRMA As Boolean
    'sj 02/07/2002 - start
    Private m_bAONAffinity As Boolean
    Private m_bRestrictInsurerAccess As Boolean
    ' UserAgentCnt
    Private m_lUserAgentCnt As Integer
    ' PW190802
    Private m_bSuppressSubAgents As Boolean

    Private m_bIsInTransferMode As Boolean

    Private m_vValidPartyTypesArray() As Object
    Private m_iIsComplaint As Integer

    ' SET 07/06/2004 ISS11882
    Private m_bDPARequired As Boolean
    Private m_bDPAIsActive As Boolean
    Private m_bDPAIsEnforced As Boolean 'FSA Phase 3.1

    'RKS 141004 PN13238 & PN14838
    Private m_bIncludeClosedBranches As Boolean

    'DC101204
    Private m_vAgentTypes As Object

    'DM22082006 PN29952
    Private m_bExcludeMultiInsurer As Boolean

    Private m_iSourceID As Integer ''Agent Filtering

    'MKW PN11249
    Private m_bIgnoreDPAQuestions As Boolean
    Private m_lPartyAgentCnt As Integer 'PN13921
    Private m_bEnableNewParty As Boolean

    Private m_bViewAuthority As Boolean '2005 Client manager Security
    Private m_bEditAuthority As Boolean '2005 Client manager Security
    Private m_bDeleteAuthority As Boolean '2005 Client Manager Security

    Private m_bSystemOptionClientBlacklistingInForce As Boolean
    Private m_bSuppressCancelledAgents As Boolean
    Private m_lCountryId As Integer
    Private m_bRiskTransfer As Boolean
    Private m_iBureauAccount As Integer
    Private m_bIsRetained As Boolean
    Private m_iRetainedValue As Integer
    Private m_sReinsuranceTypeArray As Object
    Private m_bHasAccess As Boolean
    Public m_bAutoSearch As Boolean
    Private m_bIsRiBroker As Boolean
    Private m_vBrokerArray As Object
    Private m_bSystemOptionEnhanceFilterscreens As Boolean

    'Party View
    Private m_bIsViewOnlyClientManager As Boolean
    Private m_bIsViewOnlyAgentMaintenance As Boolean
    Private m_bIsViewOnlyAccountHandlerMaintenance As Boolean
    Private m_bIsViewOnlyAccountExecutiveMaintenance As Boolean
    Private m_bIsViewOnlyInsurerMaintenance As Boolean
    Private m_bIsViewOnlyOtherPartyMaintenance As Boolean
    Private m_bAllowAgentSearch As Boolean

    Private m_bDisableWildcardSearchOption As Boolean
    Private m_bEnablePartialWildcardSearchOption As Boolean
    Private Const sFindCashDeptMaintenance As String = "iSIRFindCashDeptMaintenance"

    Private hScrollValue As Integer = 0
    Const LVM_FIRST As Int32 = &H1000
    Const LVM_SCROLL As Int32 = LVM_FIRST + 20
    Const SBS_HORZ As Integer = 0
    'SAGICOR WPR14
    Private m_iCommissionlevel As Integer
    Public WriteOnly Property CommissionLevel() As Integer
        Set(ByVal Value As Integer)
            m_iCommissionlevel = Value
        End Set
    End Property

    Public Property DisableWildcardSearchOption() As Boolean
        Get
            Return m_bDisableWildcardSearchOption
        End Get
        Set(ByVal Value As Boolean)
            m_bDisableWildcardSearchOption = Value
        End Set
    End Property


    Public Property EnablePartialWildcardSearchOption() As Boolean
        Get
            Return m_bEnablePartialWildcardSearchOption
        End Get
        Set(ByVal Value As Boolean)
            m_bEnablePartialWildcardSearchOption = Value
        End Set
    End Property

    Public Property IsRetained() As Boolean
        Get
            Return m_bIsRetained
        End Get
        Set(ByVal Value As Boolean)
            m_bIsRetained = Value
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

    ' Property used to append all the reinsurance types and make generic
    ' search criteria for reinsurance type

    Public Property ReinsuranceTypeArray() As String
        Get
            Return m_sReinsuranceTypeArray
        End Get
        Set(ByVal Value As String)
            m_sReinsuranceTypeArray = Value
        End Set
    End Property

    Public WriteOnly Property SuppressCancelledAgents() As Boolean
        Set(ByVal Value As Boolean)
            m_bSuppressCancelledAgents = Value
        End Set
    End Property

    Public WriteOnly Property EnableNewParty() As Boolean
        Set(ByVal Value As Boolean)
            m_bEnableNewParty = Value
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
    'JT PN13238
    'JT PN13238
    Public Property IsIncludeClosedBranchChecked() As Boolean
        Get
            Return m_bIsIncludeClosedBranchChecked
        End Get
        Set(ByVal Value As Boolean)
            m_bIsIncludeClosedBranchChecked = Value
        End Set
    End Property


    Public Property DPARequired() As Boolean
        Get
            Return m_bDPARequired
        End Get
        Set(ByVal Value As Boolean)
            m_bDPARequired = Value
        End Set
    End Property
    Public WriteOnly Property BureauAccount() As Integer
        Set(ByVal Value As Integer)
            m_iBureauAccount = Value
        End Set
    End Property

    Public WriteOnly Property IsIntroducerOnly() As Boolean
        Set(ByVal Value As Boolean)
            m_bIntroducerOnly = Value
        End Set
    End Property

    'Agent type to be included - WR22 : Agent Payments
    Public WriteOnly Property AllowAgentSearch() As Boolean
        Set(ByVal Value As Boolean)
            m_bAllowAgentSearch = Value
        End Set
    End Property

    'PN 53331 START
    Public Property TelephoneAreaCode() As String
        Get
            Return m_sTelAreaCode
        End Get
        Set(ByVal Value As String)
            m_sTelAreaCode = Value
        End Set
    End Property
    Public Property TelephoneNumber() As String
        Get
            Return m_sTelNumber
        End Get
        Set(ByVal Value As String)
            m_sTelNumber = Value
        End Set
    End Property

    Public WriteOnly Property IsComplaint() As Integer
        Set(ByVal Value As Integer)
            m_iIsComplaint = Value
        End Set
    End Property
    'MKW080104 PN9424 Include Complaint in FSA reasons END

    Public WriteOnly Property ValidPartyTypesArray() As Object()
        Set(ByVal Value As Object())
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

    Public WriteOnly Property UserAgentCnt() As Integer
        Set(ByVal Value As Integer)
            m_lUserAgentCnt = Value
        End Set
    End Property
    'sj 06/06/2002 - end

    'sj 03/07/2002 - start
    Public WriteOnly Property RestrictInsurerAccess() As Boolean
        Set(ByVal Value As Boolean)
            m_bRestrictInsurerAccess = Value
        End Set
    End Property
    'sj 03/07/2002 - end

    Public Property AddressCnt() As Integer
        Get
            Return m_lAddressCnt
        End Get
        Set(ByVal Value As Integer)
            m_lAddressCnt = Value
        End Set
    End Property

    Public Property AddressLine1() As String
        Get
            Return m_sAddressLine1
        End Get
        Set(ByVal Value As String)
            m_sAddressLine1 = Value
        End Set
    End Property
    Public Property AddressLine2() As String
        Get
            Return m_sAddressLine2
        End Get
        Set(ByVal Value As String)
            m_sAddressLine2 = Value
        End Set
    End Property
    Public Property AddressLine3() As String
        Get
            Return m_sAddressLine3
        End Get
        Set(ByVal Value As String)
            m_sAddressLine3 = Value
        End Set
    End Property
    Public Property AddressLine4() As String
        Get
            Return m_sAddressLine4
        End Get
        Set(ByVal Value As String)
            m_sAddressLine4 = Value
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
    Public Property Postcode() As String
        Get
            Return m_sPostalCode
        End Get
        Set(ByVal Value As String)
            m_sPostalCode = Value
        End Set
    End Property
    Public Property PartyStatus() As String
        Get
            Return m_sPartyStatus
        End Get
        Set(ByVal Value As String)
            m_sPartyStatus = Value
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
    'DC260202 -start
    Public Property NavStep() As String
        Get
            Return m_vNavStep
        End Get
        Set(ByVal Value As String)

            m_vNavStep = CStr(Value)
        End Set
    End Property
    'DC260202 -end
    ' CTAF 190900 - Start
    Public Property FileCode() As String
        Get
            Return m_sFileCode
        End Get
        Set(ByVal Value As String)
            m_sFileCode = Value
        End Set
    End Property
    ' CTAF 190900 - End

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

    ' Alix - 07/11/2002
    Public WriteOnly Property AllowAddressSelection() As Boolean
        Set(ByVal Value As Boolean)
            m_bAllowAddressSelection = Value
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

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_iNavigate = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_iProcessMode = Value

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
    Public ReadOnly Property PartyCnt() As Integer
        Get

            Return m_iPartyCnt

        End Get
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
    'DC160703 -ISS5384 -end

    Public Property PartyAgentCnt() As Integer
        Get 'PN13921
            Return m_lPartyAgentCnt
        End Get
        Set(ByVal Value As Integer) 'PN13921
            m_lPartyAgentCnt = Value
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


    Public Property LongName() As String
        Get

            Return m_sLongName

        End Get
        Set(ByVal Value As String)

            m_sLongName = Value

        End Set
    End Property


    Public ReadOnly Property ResolvedName() As String
        Get

            Return m_sResolvedName

        End Get
    End Property


    Public Property AgentOnly() As Integer
        Get

            Return m_iAgentOnly

        End Get
        Set(ByVal Value As Integer)
            'SP011298
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

    ' TF180199
    Public ReadOnly Property SelectedPartyType() As String
        Get

            Return m_sSelectedPartyType

        End Get
    End Property


    Public Property NotEditable() As Integer
        Get

            Return m_iNotEditable

        End Get
        Set(ByVal Value As Integer)

            m_iNotEditable = Value

        End Set
    End Property


    Public Property DeleteMode() As Boolean
        Get

            Return m_bDeleteMode

        End Get
        Set(ByVal Value As Boolean)

            m_bDeleteMode = Value

        End Set
    End Property
    'eck040500
    'developer guide no. 17(Guide)
    Public WriteOnly Property SourceArray() As Object(,)
        Set(ByVal Value As Object(,))
            ' Set the valid sources for the user
            m_vSourceArray = Value
        End Set
    End Property
    'JT PN-15885
    Public WriteOnly Property SourceArrayIncludeClosedBranch() As Object
        Set(ByVal Value As Object)
            ' Set the valid sources for the user


            m_vSourceArrayIncludeClosedBranch = Value
        End Set
    End Property


    Public Property DateOfBirth() As String
        Get
            Return m_vDateOfBirth
        End Get
        Set(ByVal Value As String)

            m_vDateOfBirth = CStr(Value)
        End Set
    End Property

    Public Property SwiftPartyID() As Integer
        Get
            Return m_iSwiftPartyID
        End Get
        Set(ByVal Value As Integer)
            m_iSwiftPartyID = Value
        End Set
    End Property
    'MSS200901 - Merge Start
    Public Property IgnoreDriversAndWitnesses() As Boolean
        Get
            Return m_bIgnoreDriversAndWitnesses
        End Get
        Set(ByVal Value As Boolean)
            m_bIgnoreDriversAndWitnesses = Value
        End Set
    End Property
    'MSS200901 - Merge End
    'DC101204
    Public Property AgentTypes() As Object
        Get
            Return m_vAgentTypes
        End Get
        Set(ByVal Value As Object)


            m_vAgentTypes = Value
        End Set
    End Property
    '2005 Client Manager Security
    Public WriteOnly Property ViewAuthority() As Boolean
        Set(ByVal Value As Boolean)
            m_bViewAuthority = Value
        End Set
    End Property
    Public WriteOnly Property EditAuthority() As Boolean
        Set(ByVal Value As Boolean)
            m_bEditAuthority = Value
        End Set
    End Property
    Public WriteOnly Property DeleteAuthority() As Boolean
        Set(ByVal Value As Boolean)
            m_bDeleteAuthority = Value
        End Set
    End Property
    ' PRIVATE Property Procedures (End)
    Public Property DateCancelled() As String
        Get
            Return m_vDateCancelled
        End Get
        Set(ByVal Value As String)

            m_vDateCancelled = CStr(Value)
        End Set
    End Property
    Public WriteOnly Property ExcludeMultiInsurer() As Boolean
        Set(ByVal Value As Boolean) 'PN29952
            m_bExcludeMultiInsurer = Value
        End Set
    End Property
    ' PRIVATE Events (End)
    'RWH(04/07/2000) RSAIB Process 007 - New property.

    Public Property PartyOther() As Boolean
        Get
            Return m_bPartyOther
        End Get
        Set(ByVal Value As Boolean)
            m_bPartyOther = Value
        End Set
    End Property

    'MKW 150606 START
    Public Property CountryId() As Integer
        Get
            Return m_lCountryId
        End Get
        Set(ByVal Value As Integer)
            m_lCountryId = Value
        End Set
    End Property
    'MKW 150606 END
    ''Agent Filtering

    Public Property BranchID() As Integer
        Get
            Return m_iSourceID
        End Get
        Set(ByVal Value As Integer)

            m_iSourceID = CInt(Value)
        End Set
    End Property

    Public ReadOnly Property IsRIBroker() As Boolean
        Get
            Return m_bIsRiBroker
        End Get
    End Property

    Public ReadOnly Property BrokerArray() As Object
        Get
            Return m_vBrokerArray
        End Get
    End Property

    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    ' Edit History  :
    ' RAM20040226   : PN Issue 10592 Changes
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Dim sPartyTypeOther As String = ""
        Dim lAccountID As Integer
        Dim vCnt, vName, vDateCancelled As Object
        Dim vResolvedName As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display a searching message.
            DisplayStatusSearching()

            ' Disable parts of the interface while
            ' a search is in progress.
            m_lReturn = DisableInterface(bDisable:=True)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' The user has typed in a code so we need the account_id
            If TxtAgentCode.Text <> "" Then
                m_lReturn = GetID(lId:=lAccountID, vShortName:=TxtAgentCode.Text)

                If lAccountID = 0 Or lAccountID = -1 Then
                    m_lReturn = SelectParty(vPartyCnt:=vCnt, vName:=vName, vShortName:=TxtAgentCode.Text, vResolvedName:=vResolvedName, vSpecialParty:="AG", bSuppressSubAgents:=True, vDateCancelled:=vDateCancelled)
                End If

            End If

            If IsRetained Then

                g_oBusiness.RetainedValue = RetainedValue

                g_oBusiness.IsRetained = True
            End If

            If ReinsuranceTypeArray <> "" Then

                g_oBusiness.ReinsuranceTypeArray = ReinsuranceTypeArray
            End If

            g_oBusiness.SuppressCancelledAgents = m_bSuppressCancelledAgents



            m_bSpecialPartyQuery = True 'CT 10/08/00   set form flag to say which query called

            'DC160703 -ISS5384 -pass party source id

            g_oBusiness.PartySourceId = m_iPartySourceId
            If m_sAgencyOrunderwriting = "U" And CheckExcludeBrokerssettledNeofComm.CheckState = CheckState.Checked Then
                'ignore searching on valid SourceArray
                m_lReturn = g_oBusiness.SearchAgent(r_vResultArray:=m_vSearchData, vShortname:=TxtAgentCode.Text, vName:=TxtAgentName.Text, vpartyAgentDesc:=cmbAgentType.Text, vCurrCode:=cboCurrency.Text, vSubBraDesc:=cmbBranch.ItemCaption, vIsGrossAgent:=1)
            Else
                m_lReturn = g_oBusiness.SearchAgent(r_vResultArray:=m_vSearchData, vShortname:=TxtAgentCode.Text, vName:=TxtAgentName.Text, vpartyAgentDesc:=cmbAgentType.Text, vCurrCode:=cboCurrency.Text, vSubBraDesc:=cmbBranch.ItemCaption, vIsGrossAgent:=0)

            End If

            ' Check the return values.
            Select Case (m_lReturn)
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Found search details.
                Case gPMConstants.PMEReturnCode.PMNotFound
                    ' No search details found.
                Case Else
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="GetBusiness", r_lFunctionReturn:=result)
                    Return result
            End Select

            ' PWF 11/09/2002 - Moved to DataToInterface
            ' Display the number of item found message.
            '    DisplayStatusFound

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="GetBusiness", r_lFunctionReturn:=result, excep:=excep)
            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ProcessFindParty
    '
    ' Description: Process the Party lookup component.
    '
    ' ***************************************************************** '

    ' SB 31/03/98 defect 37
    ' function added to enable find short name to work.

    'Private Function ProcessFindParty() As Integer
    '	Dim result As Integer = 0
    '	Dim iPMBFindParty As Object

    '	'TF031298 - changed from SB 31/03/98 Defect 37


    '       Dim oFindParty As Object

    '	Try 

    '		result = gPMConstants.PMEReturnCode.PMTrue

    '		' Set the mouse pointer.
    '		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


    '		'PN-71681(Sushil Kumar)
    '		If g_oObjectManager Is Nothing Then
    '			g_oObjectManager = New bObjectManager.ObjectManager()

    '			' Call the initialise method.
    '			m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
    '		End If



    '		' Create Find Party object

    '		Dim temp_oFindParty As Object
    '		m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_oFindParty, "iPMBFindParty.Interface_Renamed", gPMConstants.PMGetLocalInterface)
    '		oFindParty = temp_oFindParty

    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
    '			result = gPMConstants.PMEReturnCode.PMFalse
    '			oFindParty = Nothing
    '			Return result
    '		End If

    '		' Set the process modes.

    '		m_lReturn = oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

    '		' Check for errors.
    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			' carry on - let FindParty use defaults
    '		End If

    '		' Set the properties.

    '		oFindParty.CallingAppName = m_sCallingAppName

    '           oFindParty.ShortName = TxtAgentCode.Text.Trim()


    '		m_lReturn = oFindParty.Start()
    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
    '			Return gPMConstants.PMEReturnCode.PMFalse
    '		End If

    '		'Retrieve Party properties

    '		If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then
    '			With oFindParty

    '                   TxtAgentCode.Text = .ShortName.Trim()

    '				m_sShortName = .ShortName.Trim()

    '				m_lInsHolderCnt = .PartyCnt

    '				m_sLongName = .LongName.Trim()
    '			End With
    '		Else
    '			result = gPMConstants.PMEReturnCode.PMFalse
    '		End If

    '		' Destroy Find Party object

    '		m_lReturn = oFindParty.Terminate()
    '		oFindParty = Nothing

    '		' Set the mouse pointer.
    '		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

    '		Return result

    '	Catch excep As System.Exception




    '		result = gPMConstants.PMEReturnCode.PMError

    '		' Log Error.
    '		iPMFunc.LogError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process find party", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFindParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

    '		Return result

    '	End Try
    'End Function

    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    '
    ' ***************************************************************** '
    Public Function DataToInterface() As Integer


        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim icoFindImage As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Clear the search details.
            lvwSearchDetails.Items.Clear()

            ' Check that search details are valid before
            ' continuing.
            If Not Information.IsArray(m_vSearchData) Then
                ' PWF 11/09/2002 - Moved from GetBusiness
                ' Reset the number of items found message.
                DisplayStatusFound()

                Return result
            End If

            'sj 22/08/2002 - end

            ' Assign the details to the interface.

            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
                'PN29952
                If m_bExcludeMultiInsurer Then

                    If CStr(m_vSearchData(ACIShortName, lRow)).Trim().ToUpper() = "MULTI" Then

                        If lRow = m_vSearchData.GetUpperBound(1) Then Exit For
                        lRow += 1
                    End If

                    If CStr(m_vSearchData(ACIShortName, lRow)).Trim().ToUpper() = "MULTILM" Then

                        If lRow = m_vSearchData.GetUpperBound(1) Then Exit For
                        lRow += 1
                    End If
                End If
                'JAS(CMG) 03/09/02 - Added second image for icon
                'deepak_todo:
                oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(ACIShortName, lRow)).Trim(), 0)

                ' Assign details to other the columns 
                ' Column 2 Long Name
                ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnClientCode - 1).Text = CStr(m_vSearchData(ACIPartyType, lRow)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnName - 1).Text = CStr(m_vSearchData(ACIShortName, lRow)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnSurname - 1).Text = CStr(m_vSearchData(ACILongName, lRow)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnForename - 1).Text = CStr(m_vSearchData(ACIAddress1, lRow)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnAddressLine1 - 1).Text = CStr(m_vSearchData(ACIPostalCode, lRow)).Trim()
                oListItem.Tag = CStr(lRow)

                ' Refresh the first X amount of rows, to
                ' allow the user to see the results instantly.
                'Modified,add a line to make handle of listview,to help in selected item
                Dim iPtr As IntPtr = lvwSearchDetails.Handle

                If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                    ' Select the first item.
                    lvwSearchDetails.Items.Item(0).Selected = True

                    ' Refresh the initial results.
                    lvwSearchDetails.Refresh()
                End If
                'End If
            Next lRow

            ' Select the first item.
            If lvwSearchDetails.Items.Count > 0 Then

                lvwSearchDetails.Items.Item(0).Selected = True

                ' Alix - 07/11/2002

                'developer guide no. 131(Guide)
                If Not lvwSearchDetails.FocusedItem Is Nothing Then
                    m_iPartyCnt = CInt(m_vSearchData(ACIPartyCnt, Convert.ToString(lvwSearchDetails.FocusedItem.Tag)))
                End If
                ' Enable the interface now that the search has completed.
                m_lReturn = DisableInterface(bDisable:=False)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' PWF 11/09/2002 - Moved from GetBusiness
            ' Display the number of items found message.
            DisplayStatusFound()
            If lvwSearchDetails.Items.Count > 0 Then
                cmdSelectAll.Enabled = True
                cmdCancel.Enabled = True
            Else
                cmdCancel.Enabled = True
                cmdSelectAll.Enabled = False
            End If

            Return result


        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="DataToInterface", r_lFunctionReturn:=result, excep:=excep)
            Return result


            Return result
        End Try
    End Function




    ' ***************************************************************** '
    ' Name: DataToProperties
    '
    ' Description: Updates the property member from the search data
    '              storage.
    '
    ' ***************************************************************** '
    Public Function DataToProperties() As Integer

        Dim result As Integer = 0
        Dim lSelectedItem As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the selected item's tag, so we can use this
            ' as the index to the search data storage details.

            If IsNothing(lvwSearchDetails.FocusedItem) Then
                lSelectedItem = 0
            Else
                lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)
            End If
            ' lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)

            ' Update the property members.

            ' {* USER DEFINED CODE (Begin) *}


            m_iPartyCnt = CInt(m_vSearchData(ACIPartyCnt, lSelectedItem))

            m_sShortName = CStr(m_vSearchData(ACIPartyType, lSelectedItem)).Trim()

            m_sLongName = CStr(m_vSearchData(ACIShortName, lSelectedItem)).Trim()
            m_sResolvedName = CStr(m_vSearchData(ACILongName, lSelectedItem)).Trim()


            m_sFileCode = CStr(m_vSearchData(ACIPostalCode, lSelectedItem)).Trim()


            'Dim dbNumericTemp As Double
            'If Double.TryParse(CStr(m_vSearchData(ACIAgentCnt, lSelectedItem)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

            '    m_lPartyAgentCnt = CInt(m_vSearchData(ACIAgentCnt, lSelectedItem)) 'PN13921
            'End If


            'Dim dbNumericTemp2 As Double
            'If Double.TryParse(CStr(m_vSearchData(ACISourceID, lSelectedItem)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

            '    m_lPartySourceId = CInt(m_vSearchData(ACISourceID, lSelectedItem)) 'PN13921
            'End If

            'sj 3/11/99
            'bExistsOnSirius = True

            'If (Not (m_bSpecialPartyQuery)) And m_vSearchData.GetUpperBound(0) > 10 Then 'CT 10/08/00 added criteria that m_vSearchData must come from SearchByQuery procedure in BSirFindParty, not from SearchSpecialPartyByQuery

            '    Dim dbNumericTemp3 As Double
            '    If Double.TryParse(CStr(m_vSearchData(ACIInvariantKey, lSelectedItem)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then

            '        m_lInvariantKey = CInt(m_vSearchData(ACIInvariantKey, lSelectedItem))
            '    End If

            '    If CStr(m_vSearchData(ACISource, lSelectedItem)).Trim() = "Broking" Then
            '        bExistsOnSirius = False
            '    End If
            'End If
            'sj 3/11/99

            If m_sLongName = "" Then
                m_sLongName = m_sShortName
            End If

            ' MSB 03/03/03 - Start

            'If m_vSearchData.GetUpperBound(0) >= ACIPartyDateCancelled Then

            '    If CStr(m_vSearchData(ACIPartyDateCancelled, lSelectedItem)).Trim() <> "" Then

            '        m_vDateCancelled = CStr(m_vSearchData(ACIPartyDateCancelled, lSelectedItem))
            '    Else
            '        m_vDateCancelled = ""
            '    End If
            'End If
            ' MSB 03/03/03 - End


            ''m_sSelectedPartyType = CStr(m_vSearchData(ACIPartyTypeCode, lSelectedItem)).Trim()
            'TMP

            'm_iAllowConsolidatedCommission = Conversion.Val(CStr(m_vSearchData(ACIAllowConsolidatedCommission, lSelectedItem)))

            'Select Case m_sSelectedPartyType
            '    Case PMBConst.PMBPartyTypePersonalClient
            '        m_iAgentOnly = 1
            '    Case PMBConst.PMBPartyTypeCorporateClient
            '        m_iAgentOnly = 2
            '    Case PMBConst.PMBPartyTypeGroupClient
            '        m_iAgentOnly = 3
            '    Case PMBConst.PMBPartyTypeAgent
            m_iAgentOnly = 4
            'End Select
            'sj 23/09/2002 - end


            'If bExistsOnSirius Then
            '    '    If Trim$(m_vSearchData(ACISource, lSelectedItem&)) = "Sirius" _
            '    ''    Or Trim$(m_vSearchData(ACISource, lSelectedItem&)) = "Both" Then

            '    'Calculate the combined UIK

            '    iSourceID = CInt(m_vSearchData(ACISourceID, lSelectedItem))

            '    lPartyID = CInt(m_vSearchData(ACIPartyID, lSelectedItem))

            '    '   SJP 04072002 - Account Key is now = Party Count
            '    '       Still passed into CalcCombinedKey but should just be
            '    '           returned.
            '    m_lPartyUIK = m_lPartyCnt

            '    m_lReturn = g_oBusiness.CalcCombinedKey(v_lSourceId:=iSourceID, v_lKeyID:=lPartyID, r_lCombinedKeyID:=m_lPartyUIK)

            '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '        result = gPMConstants.PMEReturnCode.PMFalse
            '        iPMFunc.LogError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the Party UIK.", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            '        Return result
            '    End If

            '    'TF030399 - Display warning if no valid agent agreement
            '    ' (MBP UAT Bug 105)

            '    m_lReturn = g_oBusiness.CheckAgencyAgreement(v_lPartyCnt:=m_lPartyCnt, r_bIsAgent:=bIsAgent, r_bIsAgencyAgreementValid:=bIsAgencyAgreementValid)

            '    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (Not bIsAgent) Then
            '        Return result
            '    End If


            '    If Not bIsAgencyAgreementValid Then


            '        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNoAgencyAgreementTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            '        sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNoAgencyAgreement, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            '        m_lReturn = MessageBox.Show(sMsg, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)

            '    End If

            'End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="DataToProperties", r_lFunctionReturn:=result, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    Public Function DisplayLookupDetails() As Integer

        ' TF131298
        ' Not used at present, but leave in as lookup boxes suppressed, not deleted

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            m_lReturn = CType(GetLookupValues(), gPMConstants.PMEReturnCode)
            ''cmbSubBranch.ItemData.Clear()
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ''m_lReturn = GetSubBranchDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Get all of the lookup details.

            ' {* USER DEFINED CODE (Begin) *}


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="DisplayLookupDetails", r_lFunctionReturn:=result, excep:=excep)
            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: PropertiesToInterface
    '
    ' Description: Updates the interface details from the property
    '              members.
    '
    ' ***************************************************************** '
    Private Function PropertiesToInterface() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' {* USER DEFINED CODE (Begin) *}

            TxtAgentCode.Text = m_sShortName.Trim()
            TxtAgentName.Text = m_sLongName.Trim()

            ' {* USER DEFINED CODE (End) *}


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="PropertiesToInterface", r_lFunctionReturn:=result, excep:=excep)
            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: ValidateVehicle
    '
    ' Description: Validates the interface vehicle lookup.
    '
    ' ***************************************************************** '
    Private Function ValidateVehicle() As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim sRegistration As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sRegistration = TxtAgentName.Text.Trim()


            lReturn = g_oBusiness.FindLikeVehicle(sRegistration:=sRegistration, vResultArray:=m_vSearchData)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="ValidateVehicle", r_lFunctionReturn:=result, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ValidateIndex
    '
    ' Description: Validates the interface index.
    '
    ' ***************************************************************** '


    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Center the interface.
            iPMFunc.CenterForm(Me)

            If Information.IsArray(m_vAgentTypes) Then

                cmbAgentType.Items.Clear()
                Dim cmbAgentType_NewIndex As Integer = -1
                cmbAgentType_NewIndex = 0
                cmbAgentType.Items.Insert(cmbAgentType_NewIndex, "")

                For iCount As Integer = m_vAgentTypes.GetLowerBound(1) To m_vAgentTypes.GetUpperBound(1)
                    'PN 18683 : Used ItemData property instead of ListIndex for cmbAgentType
                    If CStr(m_vAgentTypes(0, iCount)) = "AGENT" Then
                        cmbAgentType_NewIndex = cmbAgentType.Items.Add(gSIRLibrary.SIRPartyTypeAgentText)
                        VB6.SetItemData(cmbAgentType, cmbAgentType_NewIndex, CInt(m_vAgentTypes(1, iCount)))
                    End If
                    If CStr(m_vAgentTypes(0, iCount)) = "SUB AGENT" Then
                        cmbAgentType_NewIndex = cmbAgentType.Items.Add(PMBConst.PMBAgentTypeSubAgentText)
                        VB6.SetItemData(cmbAgentType, cmbAgentType_NewIndex, CInt(m_vAgentTypes(1, iCount)))
                    End If
                    If CStr(m_vAgentTypes(0, iCount)) = "INTRODUCER" Then
                        cmbAgentType_NewIndex = cmbAgentType.Items.Add(PMBConst.PMBAgentTypeIntroducerText)
                        VB6.SetItemData(cmbAgentType, cmbAgentType_NewIndex, CInt(m_vAgentTypes(1, iCount)))
                    End If
                Next iCount
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.
            Select Case (m_iNavigate)
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                    cmdSelectAll.Visible = True
                    cmdSelectAll.Enabled = True

                Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
                    cmdSelectAll.Visible = True
                    cmdSelectAll.Enabled = True

                Case Else

                    cmdSelectAll.Visible = True
            End Select
            If lvwSearchDetails.Items.Count > 0 Then
                cmdSelectAll.Visible = True
            Else
                cmdSelectAll.Enabled = False
            End If
            ' Position View control

            ' Disable until a policy is selected
            ' cmdView.Enabled = False
            ' AJM 14/08/00 - do not show view button at all.

            ' Update the interface details with the
            ' property members.
            m_lReturn = CType(PropertiesToInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If (m_sSpecialParty = PMBConst.PMBPartyTypeAgent) Or (m_sSpecialParty = PMBConst.PMBPartyTypeSubAgent) Or (m_sSpecialParty = PMBConst.PMBPartyTypeCommissionAccount) Or (m_sSpecialParty = PMBConst.PMBPartyTypeIntermediary) Then
                'KB PN Issue 1929
                'These types apply to Underwriting only so add switch

                If m_sSpecialParty = PMBConst.PMBPartyTypeSubAgent Then
                    cmbAgentType.Items.Insert(0, "")
                    cmbAgentType.Items.Insert(1, "")
                    cmbAgentType.Items.Insert(2, PMBConst.PMBAgentTypeSubAgentText)
                ElseIf m_sSpecialParty = PMBConst.PMBPartyTypeCommissionAccount Then
                    cmbAgentType.Items.Insert(0, "")
                    cmbAgentType.Items.Insert(1, "")
                    cmbAgentType.Items.Insert(2, "")
                    cmbAgentType.Items.Insert(3, PMBConst.PMBAgentTypeCommAccountText)
                ElseIf m_sSpecialParty = PMBConst.PMBPartyTypeIntermediary Then
                    cmbAgentType.Items.Insert(0, "")
                    cmbAgentType.Items.Insert(1, "")
                    cmbAgentType.Items.Insert(2, "")
                    cmbAgentType.Items.Insert(3, "")
                    cmbAgentType.Items.Insert(4, PMBConst.PMBAgentTypeIntermediaryText)
                Else
                    cmbAgentType.Items.Insert(0, "(ALL)")
                    cmbAgentType.Items.Insert(1, PMBConst.PMBAgentTypeBrokerText)
                    If Not (m_bSuppressSubAgents) Then
                        cmbAgentType.Items.Insert(2, PMBConst.PMBAgentTypeSubAgentText)
                        cmbAgentType.Items.Insert(3, PMBConst.PMBAgentTypeCommAccountText)
                        cmbAgentType.Items.Insert(4, PMBConst.PMBAgentTypeIntermediaryText)
                    Else
                        cmbAgentType.Items.Insert(2, PMBConst.PMBAgentTypeCommAccountText)
                        cmbAgentType.Items.Insert(3, PMBConst.PMBAgentTypeIntermediaryText)
                    End If
                    cmbAgentType.SelectedIndex = 0
                End If
            End If

            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwSearchDetails.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the column widths for the search list.
            'lvwSearchDetails.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(1600))
            'lvwSearchDetails.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(900))
            ''RWH(25/05/01) Change size of header for wider content in mode 1.
            ''If m_lFindMode = 0 Then
            ''    lvwSearchDetails.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(850))
            ''Else
            'lvwSearchDetails.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(1200))
            ' ''End If
            'lvwSearchDetails.Columns.Item(3).Width = CInt(VB6.TwipsToPixelsX(1800))


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="SetInterfaceDefaults", r_lFunctionReturn:=result, excep:=excep)
            Return result

        End Try
    End Function

    Private Function SetDefaultCurrencyOptions(ByVal v_iSourceId As Integer, ByVal v_iCurrencyID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetDefaultCurrencyOptions"

        Try
            Dim iBaseCurrencyID As Integer


            result = gPMConstants.PMEReturnCode.PMTrue

            'Check for Multicurrency Banking Option

            'Check for Valid source and currency ID
            If (v_iSourceId < 1) Or (v_iCurrencyID < 1) Then
                Return result
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Fails to get base currency for source id " & v_iSourceId, gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            Return result

        Finally

        End Try
        Return result
    End Function
    ' ***************************************************************** '
    ' Name: DisableInterface
    '
    ' Description: Disables parts of the interface while a search is
    '              in progress.
    '
    ' ***************************************************************** '
    Private Function DisableInterface(ByRef bDisable As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdOk.Enabled = Not bDisable
            cmdCancel.Enabled = False
            cmdSelectAll.Enabled = False

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="DisableInterface", r_lFunctionReturn:=result, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    Private Function GetLookupValues() As Integer

        ' TF131298
        ' Not used at present, but leave in as lookup boxes suppressed, not deleted

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Gets all of the lookup values.

            ' Get all of the lookup values with the correct
            ' effective date.

            m_lReturn = g_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookUpDetails)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="GetLookupValues", r_lFunctionReturn:=result, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer

        ' TF131298
        ' Not used at present, but leave in as lookup boxes suppressed, not deleted

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        ' Lookup detail contants.
        Const ACDetailKey As Integer = 0
        Const ACDetailDesc As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            bFoundMatch = False

            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.
                If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next lRow

            ' Check if there has been a table match.
            If Not bFoundMatch Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="GetLookupDetails", r_lFunctionReturn:=result)
                Return result
            End If

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.

            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
                ' Add the details to the control.

                'NIIT - Replaced with the Migrated code 1144 
                ReflectionHelper.Invoke(ctlLookup, "AddItem", New Object() {m_vLookUpDetails(ACDetailDesc, lCntr)})
                'ctlLookup.AddItem(m_vLookupDetails(ACDetailDesc, lCntr))


                'NIIT - Replaced with the Migrated code 1144 

                'ctlLookup.ItemData(ctlLookup.NewIndex) = m_vLookupDetails(ACDetailKey, lCntr)
                ReflectionHelper.SetMember(ctlLookup, "ItemData", New Object() {ReflectionHelper.GetMember(ctlLookup, "NewIndex")}, m_vLookUpDetails(ACDetailKey, lCntr))
                ' Check if this is the selected index.
                If m_vLookupValues(ACValueID, lRow).Equals(m_vLookUpDetails(ACDetailKey, lCntr)) Then


                    'NIIT - Replaced with the Migrated code 1144 
                    'ctlLookup.ListIndex = ctlLookup.NewIndex
                    ReflectionHelper.SetMember(ctlLookup, "ListIndex", ReflectionHelper.GetMember(ctlLookup, "NewIndex"))
                End If
            Next lCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to zero.
            If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then

                'NIIT - Replaced with the Migrated code 1144 
                'ctlLookup.ListIndex = 0
                ReflectionHelper.SetMember(ctlLookup, "ListIndex", 0)
            End If
            '' PW110702 - Get sub branches
            'm_lReturn = PartyFunc.GetSubBranchDetails(r_oSubBranch:=cboSubBranch, r_oBranch:=uctBranch, r_oBusiness:=m_oBusiness, v_lSubBranchId:=m_vSubBranch)
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    Return gPMConstants.PMEReturnCode.PMFalse
            'End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="GetLookupDetails", r_lFunctionReturn:=result, excep:=excep)
            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: DisplayStatusSearching
    '
    ' Description: Display the status searching message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusSearching()

        Static sMessage As String = ""
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            _stbStatus_Panel1.Text = " " & sMessage
            Application.DoEvents()

        Catch excep As System.Exception
            ' Error Section.
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="DisplayStatusSearching", r_lFunctionReturn:=result, excep:=excep)
            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: DisplayStatusFound
    '
    ' Description: Display the status found message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusFound()

        Static sMessage As String = ""
        Dim lItemsFound As Integer
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            ' Store the total of item found.
            If Not Information.IsArray(m_vSearchData) Then
                lItemsFound = 0
            Else
                lItemsFound = (m_vSearchData.GetUpperBound(1) + 1)
            End If

            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            'developer guide no. 168
            _stbStatus_Panel1.Text = " " & lItemsFound & " " & sMessage

        Catch excep As System.Exception
            ' Error Section.

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="DisplayStatusFound", r_lFunctionReturn:=result, excep:=excep)
            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: CheckMandatory
    '
    ' Description: Check if all mandatory fields have been entered in
    '              order for the search to proceed.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Check all fields for data.

            '' If txtInsReference.Text.Trim() <> "" Then
            Return gPMConstants.PMEReturnCode.PMTrue
            ''End If

            If TxtAgentName.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If TxtAgentName.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="CheckMandatory", r_lFunctionReturn:=result, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ResizeInterface
    '
    ' Description: Resizes the interface controls.
    '
    ' ***************************************************************** '
    Private Function ResizeInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdFindNow.Left = Me.Width - VB6.TwipsToPixelsX(1455)
            cmdNewSearch.Left = Me.Width - VB6.TwipsToPixelsX(1455)

            ImgImage.Left = Me.Width - VB6.TwipsToPixelsX(1085)

            tabMainTab.Width = Me.Width - VB6.TwipsToPixelsX(1670)

            lvwSearchDetails.Width = Me.Width - VB6.TwipsToPixelsX(360)
            lvwSearchDetails.Height = Me.Height - VB6.TwipsToPixelsY(4000)


            cmdCancel.Left = Me.Width - VB6.TwipsToPixelsX(2535)
            cmdCancel.Top = Me.Height - VB6.TwipsToPixelsY(1110)


            ' AJM 14/08/00 - View button not required.
            'cmdView.Top = Me.Height - 1110

            'cmdNew.Top = Me.Height - 1110
            'cmdEdit.Top = Me.Height - 1110

            If cmdSelectAll.Visible Then
                cmdSelectAll.Top = Me.Height - VB6.TwipsToPixelsY(1110)
            End If

            Return result

        Catch



            ' Error Section.


            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function
    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.
        Dim sValue As String = ""

        Try
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iACTAgentSelect.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If
            ' Get System Option for Client BlackListing
            m_lReturn = iPMFunc.GetSystemOption(kSystemOptionClientBlacklistingInForce, sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="Initialise", r_lFunctionReturn:=m_lErrorNumber)
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If
            m_bSystemOptionEnhanceFilterscreens = (sValue = "1")

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel


            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception
            ' Error Section
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="Form_Initialise", r_lFunctionReturn:=m_lErrorNumber, excep:=excep)
            Exit Sub

        End Try

    End Sub
    ' PUBLIC Methods (Begin)
    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}
            'SP090998



            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    Return gPMConstants.PMEReturnCode.PMFalse
            'End If
            ''m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=uctBranch, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    Return gPMConstants.PMEReturnCode.PMFalse
            'End If




            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="SetFieldValidation", r_lFunctionReturn:=result, excep:=excep)
            Return result

        End Try
    End Function

    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try
            iPMFunc.ShowFormInTaskBar_Detach()
            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_sAgencyOrunderwriting = g_oBusiness.UnderwritingOrAgency

            'JMK 18/10/2001 - get second hidden option

            m_lReturn = SetFieldValidation()

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If
            Me.cmbBranch.FirstItem = "(ALL)"
            Me.cboCurrency.FirstItem = "(ALL)"
            If CheckMandatory() <> gPMConstants.PMEReturnCode.PMTrue Then
                ' No supplied data so cannot
                ' continue with the search.

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' {* USER DEFINED CODE (End) *}

            ' Gets the interface details to be displayed.
            m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            If m_bSavedata = True Then
                cmdOk_Click(eventSender, eventArgs)
            End If
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception
            ' Error Section
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="Form_Load", r_lFunctionReturn:=m_lErrorNumber, excep:=excep)
            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()

         

            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception
            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="Form_QueryUnload", r_lFunctionReturn:=m_lErrorNumber, excep:=excep)
            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub


    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        Try

            m_lReturn = CType(ResizeInterface(), gPMConstants.PMEReturnCode)

        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub


    ''''''''''''''



    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception
            ' Error Section.
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="cmdCancel_Click", r_lFunctionReturn:=result, excep:=excep)
            Exit Sub
        End Try

    End Sub

    Private Sub cmdHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHelp.Click
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID)

    End Sub

    Private Sub cmdOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOk.Click
        Dim vFileName As Object
        Dim lSelectedItem As Integer
        Dim sValue As String = ""
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        ' Click event of the OK button.
        Try
            m_vselecteditemArray = Nothing
            ReDim Preserve m_vselecteditemArray(lvwSearchDetails.SelectedItems.Count - 1)

            Dim i As Integer = 0
            'MKW PN18459 Select item selected in list.
            For n As Integer = 0 To lvwSearchDetails.Items.Count - 1
                If lvwSearchDetails.Items(n).Selected = True Then
                    m_vselecteditemArray(i) = m_vSearchData(0, n)
                    i += 1
                End If
            Next

            If IsNothing(lvwSearchDetails.FocusedItem) Then
                lSelectedItem = 0
            Else
                lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)
            End If


            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

            If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If
            ' Construct the recent filename
            'MKR 29/09/2004 PN 6021 'We are now using '|' as a delimiter rather than ','
            'developer guide no. 131
            If Not m_sSelectedPartyType Is Nothing AndAlso m_sSelectedPartyType <> String.Empty Then
                vFileName = m_sShortName & "|" & m_sLongName & "|" & CStr(m_iPartyCnt) & m_sSelectedPartyType.Substring(0, 1)
            Else
                vFileName = m_sShortName & "|" & m_sLongName & "|" & CStr(m_iPartyCnt)
            End If

            ' Add it
            m_lReturn = AddRecentFile(v_vFileName:=vFileName)

            ' Save the recent files
            m_lReturn = SaveRecentFiles()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If
            If m_bSavedata = True Then
                Me.Close()
            End If

        Catch excep As System.Exception
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="cmdOK_Click", r_lFunctionReturn:=result, excep:=excep)
            Exit Sub
        End Try
    End Sub

    ' ***************************************************************** '
    '
    ' Name: AddRecentFile
    '
    ' Description: Adds a file to the recent file menu
    '
    ' History: 26/06/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function AddRecentFile(ByVal v_vFileName As String) As Integer

        Dim result As Integer = 0
        Dim sShortName As String = ""
        Dim iTemp, iFound As Integer

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".AddRecentFile")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' CTAF 270601 - If its a special party then don't add it
            If (m_sSelectedPartyType <> gSIRLibrary.SIRPartyTypePersonalClient) And (m_sSelectedPartyType <> gSIRLibrary.SIRPartyTypeCorporateClient) And (m_sSelectedPartyType <> gSIRLibrary.SIRPartyTypeGroupClient) Then
                Return result
            End If

            iFound = 0

            ' Check its not already in the list
            For iLoop1 As Integer = 1 To 5
                'DJM 09/09/2003 : Just check on client code as party_type may have changed.
                'MKR 29/09/2004 PN 6021 'We are now using '|' as a delimiter rather than ','
                'If mnuRecentFile(iLoop1).Text = v_vFileName.Substring(0, v_vFileName.IndexOf("|"c)) Then
                '    'If (mnuRecentFile(iLoop1).Tag = v_vFileName) Then
                '    iFound = iLoop1
                '    Exit For
                'End If
            Next iLoop1

            If iFound = 0 Then
                iFound = 5
            End If

            ' Shuffle 1 to Found down to 2 to Found + 1
            For iLoop1 As Integer = iFound - 1 To 1 Step -1
                'mnuRecentFile(iLoop1 + 1).Text = mnuRecentFile(iLoop1).Text
                'mnuRecentFile(iLoop1 + 1).Available = mnuRecentFile(iLoop1).Available
                'mnuRecentFile(iLoop1 + 1).Tag = Convert.ToString(mnuRecentFile(iLoop1).Tag)
            Next iLoop1

            ' Add the new entry
            iTemp = (v_vFileName.IndexOf("|"c) + 1)
            sShortName = v_vFileName.Substring(0, iTemp - 1)

            ' If there is an ampersand then double it up so that it displays  PN17033
            iTemp = (sShortName.IndexOf("&"c) + 1)
            If iTemp > 0 Then
                sShortName = sShortName.Substring(0, iTemp - 1) & "&&" & Mid(sShortName, iTemp + 1, sShortName.Length - iTemp)
            End If

            'mnuRecentFile(1).Text = sShortName
            'mnuRecentFile(1).Tag = v_vFileName
            'mnuRecentFile(1).Available = True

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".AddRecentFile")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".AddRecentFile")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="AddRecentFile", r_lFunctionReturn:=result, excep:=excep)
            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: SaveRecentFiles
    '
    ' Description:
    '
    ' Recent File Functions : LoadRecentFiles
    '                         SaveRecentFiles
    '                         AddRecentFile
    '
    ' These can be taken wholesale and used in other programs
    ' (like ClientManager)
    '
    ' History: 26/06/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function SaveRecentFiles() As Integer

        Dim result As Integer = 0
        Dim sKey, sValue As String

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".SaveRecentFiles")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iLoop1 As Integer = 1 To 5

                sKey = "RecentFile" & iLoop1

                'If mnuRecentFile(iLoop1).Available Then
                '    sValue = Convert.ToString(mnuRecentFile(iLoop1).Tag)

                '    ' If we have 2 "&" (for display purposes) then replace with 1   PN17033
                '    sValue = sValue.Replace("&&", "&")

                'Else
                'sValue = ""
                'End If

                m_lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=sKey, v_sSettingValue:=sValue, v_sSubKey:="RecentFiles")
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next iLoop1

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".SaveRecentFiles")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".SaveRecentFiles")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="SaveRecentFiles", r_lFunctionReturn:=result, excep:=excep)
            Return result

        End Try
    End Function
    Private Sub cmdFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFindNow.Click
        Dim sWildcardErrorMessage As String = ""
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Input search criteria in at least one of Code or Name
            Dim sMsg As String = ""
            If m_bSystemOptionEnhanceFilterscreens Then

                If TxtAgentCode.Text.Trim() = "" And TxtAgentName.Text.Trim() = "" And cmbBranch.Text.Trim() = "" And cmbAgentType.Text.Trim() = "" Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    sMsg = "Please Enter at least one search criteria " & _
                           Strings.Chr(13) & Strings.Chr(10) & "(either " & lblAgentCode.Text.Replace(":", "") & _
                           " or " & lblName.Text.Replace(":", "")

                    If SSTabHelper.GetTabVisible(tabMainTab, 2) Then
                        sMsg = sMsg & " or " & lblBranch.Text.Replace("...", "")
                    End If
                    If SSTabHelper.GetTabVisible(tabMainTab, 3) Then
                        sMsg = sMsg & " or " & lblAgentType.Text.Replace(":", "")
                    End If

                    sMsg = sMsg & ")"
                    MessageBox.Show(sMsg, "Find Party", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    TxtAgentCode.Focus()
                    Exit Sub
                End If

            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=TxtAgentCode.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Party")
                TxtAgentCode.Focus()
                Exit Sub

            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=TxtAgentName.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Party")
                TxtAgentName.Focus()
                Exit Sub

            End If

            ' Get the interface details from the business object.
            m_lReturn = GetBusiness()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the details.
            End If

            ' Assign the details from the search data storage
            ' to the interface.
            m_lReturn = DataToInterface()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the details.
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
            End If

            If lvwSearchDetails.Items.Count > 0 Then
                VB6.SetDefault(cmdFindNow, False)
                VB6.SetDefault(cmdOk, False)
                cmdSelectAll.Enabled = True
                cmdCancel.Enabled = True
            End If

            If lvwSearchDetails.Visible Then
                ' Set the focus.
                lvwSearchDetails.Focus()
            End If
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If (lvwSearchDetails.Items.Count > 0) Then
                lvwSearchDetails.Items(0).Selected = True
                lvwSearchDetails.Items(0).Focused = True
            End If


        Catch excep As System.Exception
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="CmdFindNow_Click", r_lFunctionReturn:=result, excep:=excep)
            Exit Sub

        End Try
    End Sub

    Private Sub cmdNewSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNewSearch.Click

        ' Click event of the New Search button.
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            ' Clear the interface details.
            m_lReturn = ClearInterface(bConfirm:=True)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to clear the interface details.
            End If

        Catch excep As System.Exception
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="cmdNewSearch_Click", r_lFunctionReturn:=result, excep:=excep)
            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: ClearInterface
    '
    ' Description: Clears all of the interface details for a new
    '              search.
    '
    ' ***************************************************************** '
    Private Function ClearInterface(ByRef bConfirm As Boolean) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the search list details.
            lvwSearchDetails.Items.Clear()

            ' Clear the search status bar.
            'developer guide no. 168
            _stbStatus_Panel1.Text = ""

            ' {* USER DEFINED CODE (Begin) *}

            'RKS 141004 PN13238 & PN14838
            CheckExcludeBrokerssettledNeofComm.CheckState = CheckState.Unchecked
            TxtAgentCode.Text = ""
            TxtAgentName.Text = ""
            cmbAgentType.SelectedIndex = 0
            cboCurrency.ListIndex = 0
            cmbBranch.ItemId = 0

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

            m_lReturn = DisableInterface(bDisable:=True)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="ClearInterface", r_lFunctionReturn:=result, excep:=excep)
            Return result

        End Try
    End Function


    Private Sub cmdAgentLookup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAgentLookup.Click

        Dim vCnt, vName, vShortName As Object
        Dim vResolvedName As String = ""
        Dim dDefaultDate As Date
        Dim vDateCancelled As Object
        Dim dTransactionDate As Date
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            vShortName = TxtAgentCode.Text
            m_lReturn = SelectParty(vPartyCnt:=vCnt, vName:=vName, vShortName:=vShortName, vResolvedName:=vResolvedName, vSpecialParty:="AG", bSuppressSubAgents:=True, vDateCancelled:=vDateCancelled)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            dDefaultDate = ToSafeDate("29/12/1899", #12/29/1899#)

            dTransactionDate = DateTime.Today

            If (ToSafeDate(vDateCancelled, #12/29/1899#) <= dTransactionDate) And (ToSafeDate(vDateCancelled, #12/29/1899#) <> dDefaultDate) Then
                System.Windows.Forms.MessageBox.Show("Agency cancelled - no new transactions can be placed through this " & _
                                                     "agent", "Agency Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Error)
                TxtAgentCode.Text = ""


                If m_sUnderwritingType = "U" Then
                    ' Removes associated sub-agents
                    If lvwSearchDetails.Items.Count > 0 Then

                        ' lvwAgents has all the sub agents.
                        For iIndex As Integer = 1 To lvwSearchDetails.Items.Count
                            If lvwSearchDetails.Items.Count >= iIndex Then
                                ' Check for the associated sub agents.
                                lvwSearchDetails.Items.RemoveAt(iIndex - 1)
                                lvwSearchDetails.Refresh()
                            End If
                        Next

                        ' Clears the array as no associated sub agents is in the lvwAgents.

                    End If
                End If

                Exit Sub
            End If

            'save the count in the tag and update controls

            TxtAgentCode.Tag = CStr(vCnt)

            If vShortName = "" Then
                TxtAgentCode.Text = CStr(vName)
            Else
                TxtAgentCode.Text = vShortName
            End If
            TxtAgentName.Text = vResolvedName

            If m_sUnderwritingType = "U" Then
                ' Removes associated sub-agents
                If lvwSearchDetails.Items.Count > 0 Then

                    ' m_vGetAssociatedSubAgent array has the list of associated sub agents.
                    ' lvwAgents has all the sub agents.
                    For iIndex As Integer = 1 To lvwSearchDetails.Items.Count
                        If lvwSearchDetails.Items.Count >= iIndex Then
                            ' Check for the associated sub agents.
                            lvwSearchDetails.Items.RemoveAt(iIndex - 1)
                            lvwSearchDetails.Refresh()
                        End If
                    Next

                    System.Windows.Forms.MessageBox.Show("Lead Agent has been changed - any associated Sub-Agents will" & _
                                                         " also be removed", "Policy", MessageBoxButtons.OK)
                End If

            End If

        Catch excep As System.Exception
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="cmdAgentCode_Click", r_lFunctionReturn:=result, excep:=excep)
            Exit Sub

        End Try
    End Sub


    '**************************************************************************
    '
    ' Name    : IsInListView
    ' Desc    : return PMTrue if in list view
    ' History : 18/08/2000 Tinny (Created)
    '
    '**************************************************************************
    Public Function IsInListView(ByVal v_vKeyID As Integer, ByRef r_oListView As ListView) As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMFalse
            For lCount As Integer = 1 To r_oListView.Items.Count

                ' Added the cLng Conversion, since the v_vKeyID is a numeric,
                ' when compared with Tag property so it always mismatches
                If Convert.ToString(r_oListView.Items.Item(lCount - 1).Tag) = v_vKeyID Then
                    result = gPMConstants.PMEReturnCode.PMTrue
                    Exit For
                End If
            Next

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="IsInListView", r_lFunctionReturn:=result, excep:=excep)
            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: SelectParty
    '
    ' Description: Call Find Party component to choose a party
    '
    ' PW190802 - allow to suppress sub agents
    ' ***************************************************************** '
    Private Function SelectParty(ByRef vPartyCnt As Integer, ByRef vShortName As String, Optional ByRef vName As String = "", Optional ByRef vSpecialParty As String = "", Optional ByRef vResolvedName As String = "", Optional ByRef vAddress1 As String = "", Optional ByRef bSuppressSubAgents As Boolean = False, Optional ByRef vDateCancelled As Object = Nothing) As Integer 'CT 19/07/00 added vResolvedName parameter
        Dim result As Integer = 0
        Dim oFindParty As iPMBFindParty.Interface_Renamed
        Dim vKeyArray(,) As Object
        Dim lLower, lUpper As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oFindParty = New iPMBFindParty.Interface_Renamed

            oFindParty.BranchID = m_iSourceID
            m_lErrorNumber = CType(oFindParty, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If IsNothing(vShortName) Then
                vShortName = ""
            End If
            oFindParty.CallingAppName = ACApp

            'SD 31/07/2002
            m_lErrorNumber = oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:=m_sTransactionType, vEffectiveDate:=DateTime.Now)

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set appropriate key if agent only


            If (Not Information.IsNothing(vSpecialParty)) And (Not String.IsNullOrEmpty(vSpecialParty)) Then

                ReDim vKeyArray(1, 1)

                vKeyArray(0, 0) = "special_party"
                vKeyArray(1, 0) = vSpecialParty
                vKeyArray(0, 1) = "shortname"
                vKeyArray(1, 1) = vShortName

                m_lErrorNumber = oFindParty.SetKeys(vKeyArray)

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    oFindParty = Nothing
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If (vSpecialParty = "AG") Or (vSpecialParty = "UB") Or (vSpecialParty = "AH") Then
                    oFindParty.NotEditable = 1
                End If

                ' PW190802 - suppress sub agents if applicable
                oFindParty.SuppressSubAgents = bSuppressSubAgents
            End If

            m_lErrorNumber = oFindParty.Start()

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then

                vPartyCnt = oFindParty.PartyCnt
                vShortName = oFindParty.ShortName

                vDateCancelled = oFindParty.DateCancelled


                If Not Information.IsNothing(vName) Then
                    vName = oFindParty.LongName
                End If
                'TN20000823 - fix CT

                If Not Information.IsNothing(vResolvedName) Then
                    vResolvedName = oFindParty.ResolvedName
                End If

                ' PWF20020624 - Associated Clients SQL - SBO 1.9 - 094 Customer Centricity (START)
                ' Return address line1 if requested

                If Not Information.IsNothing(vAddress1) Then
                    ' Get the key array (only place it's stored)
                    m_lReturn = oFindParty.GetKeys(vKeyArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Walk the array to find the value

                    lLower = vKeyArray.GetLowerBound(1)

                    lUpper = vKeyArray.GetUpperBound(1)
                    For lCount As Integer = lLower To lUpper

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lCount)) = PMNavKeyConst.PMKeyNameAddLine1 Then

                            vAddress1 = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lCount))
                            Exit For
                        End If
                    Next
                End If

            Else
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oFindParty.Dispose()

            oFindParty = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="SelectParty", r_lFunctionReturn:=result, excep:=excep)
            Return result

        End Try
    End Function

    Private Sub TxtAgentCode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtAgentCode.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)
    End Sub

    Private Sub TxtAgentCode_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtAgentCode.Enter
        iPMFunc.SelectText(TxtAgentCode)

        ' Change the default button.
        VB6.SetDefault(cmdFindNow, True)
    End Sub

    Private Sub lvwSearchDetails_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwSearchDetails.Enter
        ' GotFocus Event for the search details
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            ' Unset any default buttons so can
            VB6.SetDefault(cmdFindNow, False)
            VB6.SetDefault(cmdOk, False)

        Catch excep As System.Exception
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="lvwSearchDetails_GotFocus", r_lFunctionReturn:=result, excep:=excep)
            Exit Sub

        End Try
    End Sub

    Private Sub lvwSearchDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwSearchDetails.Click
        Dim sShortName As String = ""
        Dim iRow As Integer

        If lvwSearchDetails.Items.Count > 0 Then
            sShortName = lvwSearchDetails.SelectedItems(0).Text

            ' Ram 22-03-2001
            ' To Show the proper telephone number we have to display the
            ' details of the selected row rather than checking the short name
            ' Since a Party can have multiple Telephone Number
            'Developer Guide no. 1
            'iRow = Convert.ToString(lvwSearchDetails.FocusedItem.Tag)
            iRow = Convert.ToString(lvwSearchDetails.SelectedItems(0).Tag)

            ' Alix - 07/11/2002

            m_iPartyCnt = CInt(m_vSearchData(ACIPartyCnt, iRow))

            TxtAgentCode.Text = CStr(m_vSearchData(ACIPartyType, iRow)).Trim()

            TxtAgentName.Text = CStr(m_vSearchData(ACIShortName, iRow)).Trim()

            ' CTAF 260900

            For n As Integer = 0 To cmbAgentType.Items.Count - 1
                cmbAgentType.SelectedIndex = n
                If cmbAgentType.Text.Replace(" ", "").Replace("-", "").ToUpper() = CStr(m_vSearchData(ACILongName, iRow)).Replace(" ", "").Replace("-", "").ToUpper() Then
                    Exit For
                End If
            Next

            For n As Integer = 0 To cboCurrency.ListCount - 1
                cboCurrency.ListIndex = n
                If (cboCurrency.Text).Substring(0, 3).Replace(" ", "").Replace("-", "").ToUpper() = CStr(m_vSearchData(ACIAddress1, iRow)).Replace(" ", "").Replace("-", "").ToUpper() Then
                    Exit For
                End If
            Next

            For n As Integer = 0 To cmbBranch.ListCount - 1
                cmbBranch.ItemId = n
                If cmbBranch.ItemCaption.Replace(" ", "").Replace("-", "").ToUpper() = CStr(m_vSearchData(ACIPostalCode, iRow)).Replace(" ", "").Replace("-", "").ToUpper() Then
                    Exit For
                End If
            Next

            VB6.SetDefault(cmdOk, True)
            cmdOk.Visible = True
            cmdOk.Enabled = True
            'If we're not in a special party, now we can edit
            '        If (m_sSpecialParty = "") Then
            cmdCancel.Enabled = True
            'sj 06/06/2002 - start
            cmdSelectAll.Visible = True
            cmdSelectAll.Enabled = True
            'sj 06/06/2002 - end
            '        End If
        End If
    End Sub

    Private Sub cmdSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelectAll.Click
        For n As Integer = 0 To lvwSearchDetails.Items.Count - 1
            lvwSearchDetails.Items(n).Selected = True
            lvwSearchDetails.Items(n).Focused = True
        Next

    End Sub
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

    Private Sub lvwSearchDetails_ColumnClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvwSearchDetails.ColumnClick
        Dim ColumnHeader As System.Windows.Forms.ColumnHeader = lvwSearchDetails.Columns(e.Column)
        Try

            ' If new column reset sort order
            If ListViewHelper.GetSortKeyProperty(lvwSearchDetails) <> ColumnHeader.Index Then
                lvwSearchDetails.Sorting = System.Windows.Forms.SortOrder.Descending
            End If

            ' All column are String columns
            ListViewHelper.SetSortedProperty(lvwSearchDetails, False)
            ListViewHelper.SetSortKeyProperty(lvwSearchDetails, ColumnHeader.Index)
            If ListViewHelper.GetSortOrderProperty(lvwSearchDetails) = SortOrder.Ascending Then
                ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Descending)
            Else
                ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Ascending)
            End If
            ListViewHelper.SetSortedProperty(lvwSearchDetails, True)


        Catch ex As Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_ColumnClick", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Finally
        End Try
    End Sub
End Class
