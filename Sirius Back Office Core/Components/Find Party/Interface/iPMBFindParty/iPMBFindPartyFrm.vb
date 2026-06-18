Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Globalization
Imports System.Text
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Imports System.Runtime.InteropServices
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
        End If
    End Sub
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 17/02/1997
    '
    ' Description: Main interface.
    '
    ' Edit History:
    '
    ' CJB 27/06/2005 : PN21980 - Change SetInterfaceDefaults to check m_sSpecialParty has a value - Bug from u/w code.
    ' CJB 24/06/2005 : PN20059 - Changed cmdCancel_Click to reset m_lPartyCnt when exiting to prevent WM tasks being wrong
    ' CJB 21/06/2005 : PN20131 - Do not show File Code textbox as not applicable (in SetInterfaceDefaults)
    ' CJB 21/04/2005 : PN13921 Added support for PartyAgentCnt & PartySourceId
    ' CJB 18/04/2005 : PN17033 Changed LoadRecentFiles, mnuRecentFile_Click, AddRecentFile &
    '                  SaveRecentFiles to show "&" if there is one in the shortname.
    '                  Also changed LoadRecentFiles to function as Client Manager menu does
    '                  and not load duplicates and get short and resolved named from db in
    '                  case they have been updated.
    ' DJM 12/07/2002 : Removed DataToInterface call from GetBusiness to speed it up. It gets called after the function anyway.
    ' SJP 09/07/2002 : Added Claims to Interface - NRMA Process Sheet 179
    ' SJP 04/07/2002 : Account Key now party count to allow for more than 8 branches
    ' DJM 01/07/2002 : Added valid source array as parameter to client search functions.
    ' SP  01/12/1998 : changes to support new business roadmap
    ' TF  24/04/1998 : ProcessPartyInterface() added to activate refresh on
    '                  return to Find
    ' ??  17/02/1997 : Created
    ' ***************************************************************** '
    'developer guide no. 7
    Private Const vbFormCode As Integer = 0

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lPartyCnt As Integer
    Private m_sShortName As String = ""
    Private m_sLongName As String = ""
    Private m_sResolvedName As String = ""
    Private m_iAgentOnly As Integer
    Private m_bIntroducerOnly As Boolean
    Private m_sSpecialParty As String = ""
    Private m_lPartyUIK As Integer
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
    Private m_bPartyOther As Boolean 'RWH(04/07/2000) RSAIB Process 007
    'DC160703 -ISS5384
    Private m_lPartySourceId As Integer
    Private m_bAllowAddressSelection As Boolean
    Private m_lAddressCnt As Integer
    Private m_iCurrencyId As Integer
    'MSB - 03/03/2003
    Private m_vDateCancelled As String = ""

    ' CTAF 190800
    Private m_sFileCode As String = ""
    ' CTAF 260900
    Private m_vDateOfBirth As String = ""
    ' CTAF 260900 - SwiftPartyID
    Private m_lSwiftPartyID As Integer
    Private m_bDOBEnabled As Boolean

    ' Full Address
    Private m_vFullAddress As Object = Nothing

    Private m_iNotEditable As Integer

    Private m_bDeleteMode As Boolean
    'eck120500
    Private m_vSourceArray(,) As Object = Nothing
    'PN-15885   JT    25-10-2004
    'Array for holding data for include Closed branch as well
    Private m_vSourceArrayIncludeClosedBranch As Object = Nothing
    'JT PN-13238
    'Var for Checking that whether Include Closed Branch Checkbox is Checked or Not
    Private m_bIsIncludeClosedBranchChecked As Boolean
    ' CTAF 211100
    Private m_bSwiftEnabled As Boolean
    'TMP
    Private m_iAllowConsolidatedCommission As Integer

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMBFindParty.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object = Nothing

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object = Nothing
    Private m_vLookUpDetails(,) As Object = Nothing

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Declare an instance of the Lock object.
    Private m_oPMLock As Object = Nothing

    Private m_bAlreadyShownClosingValidation As Boolean

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the search data from the business object.
    Public m_vSearchData As Object = Nothing
    ' PRIVATE Data Members (End)
    'sj 3/11/99 - start
    Private m_lInvariantKey As Integer

    'TN20000918
    Private m_sAgencyOrunderwriting As String = ""
    Private m_bSpecialPartyQuery As Boolean 'CT 10/08/00
    'MSS200901 - Merge Start
    Private m_bIgnoreDriversAndWitnesses As Boolean
    Private m_vOtherPartyTypes As Object = Nothing
    'MSS200901 - Merge End

    ' PartyType
    Private m_sPartyType As String = ""
    ' PartyLongName
    Private m_sPartyLongName As String = ""
    ' PartyStatus
    Private m_sPartyStatus As String = ""
    'DC260202
    Private m_vNavStep As String = ""
    'JMK 18/10/2001
    Private m_sUnderwritingType As String = ""
    'sj 06/06/2002 - start
    Private m_bIsNRMA As Boolean
    'sj 02/07/2002 - start
    Private m_bAONAffinity As Boolean
    'sj 02/07/2002 - end
    'sj 03/07/2002 - start
    ' RestrictInsurerAccess
    Private m_bRestrictInsurerAccess As Boolean
    'sj 03/07/2002 - end
    ' UserAgentCnt
    Private m_lUserAgentCnt As Integer
    ' PW190802
    Private m_bSuppressSubAgents As Boolean
    Private m_bIsInTransferMode As Boolean

    Private m_vValidPartyTypesArray() As Object = Nothing

    'MKW080104 PN9424 Include Complaint in FSA reasons START
    Private m_iIsComplaint As Integer

    ' SET 07/06/2004 ISS11882
    Private m_bDPARequired As Boolean
    Private m_bDPAIsActive As Boolean
    Private m_bDPAIsEnforced As Boolean 'FSA Phase 3.1

    'RKS 141004 PN13238 & PN14838
    Private m_bIncludeClosedBranches As Boolean

    'DC101204
    Private m_vAgentTypes As Object = Nothing

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

    'MKW 150606
    Private m_lCountryId As Integer
    Private m_bRiskTransfer As Boolean

    'DM PVY
    Private m_iBureauAccount As Integer
    ' Gaurav
    Private m_bIsRetained As Boolean
    Private m_iRetainedValue As Integer
    'developer guide no. 101
    Private m_sReinsuranceTypeArray As Object = Nothing
    Private m_bHasAccess As Boolean

    ' R.Griffiths 2006-10-16 (Plus One)
    Public m_bAutoSearch As Boolean
    Private m_bIsRiBroker As Boolean
    Private m_vBrokerArray As Object = Nothing

    'PN38955 (RC)
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
    Private m_vTreatyPartiesBrokerParticipantForDisplay As Object
    Private m_lCommissionlevel As Integer
    Dim m_oAgentGroup As Object(,) = Nothing

    Public WriteOnly Property CommissionLevel() As Integer
        Set(ByVal Value As Integer)
            m_lCommissionlevel = Value
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
    Public Property CurrencyId() As Integer
        Get
            Return m_iCurrencyId
        End Get
        Set(ByVal Value As Integer)
            m_iCurrencyId = Value
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

    Public Property IncludeClosedBranches() As Boolean
        Get
            Return m_bIncludeClosedBranches
        End Get
        Set(ByVal Value As Boolean)
            m_bIncludeClosedBranches = Value
        End Set
    End Property

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

    Public WriteOnly Property ValidPartyTypesArray() As Object()
        Set(ByVal Value As Object())
            m_vValidPartyTypesArray = Value
        End Set
    End Property

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

    Public WriteOnly Property RestrictInsurerAccess() As Boolean
        Set(ByVal Value As Boolean)
            m_bRestrictInsurerAccess = Value
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
            Return m_lInvariantKey
        End Get
        Set(ByVal Value As Integer)
            m_lInvariantKey = Value
        End Set
    End Property

    Public Property NavStep() As String
        Get
            Return m_vNavStep
        End Get
        Set(ByVal Value As String)

            m_vNavStep = CStr(Value)
        End Set
    End Property

    Public Property FileCode() As String
        Get
            Return m_sFileCode
        End Get
        Set(ByVal Value As String)
            m_sFileCode = Value
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

    Public WriteOnly Property AllowAddressSelection() As Boolean
        Set(ByVal Value As Boolean)
            m_bAllowAddressSelection = Value
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

    Public ReadOnly Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
    End Property

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

    Public WriteOnly Property InsuranceRef() As String
        Set(ByVal Value As String)
            m_sInsuranceRef = Value
        End Set
    End Property

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

    Public WriteOnly Property SourceArray() As Object(,)
        Set(ByVal Value As Object(,))
            ' Set the valid sources for the user
            m_vSourceArray = Value
        End Set
    End Property

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
            Return m_lSwiftPartyID
        End Get
        Set(ByVal Value As Integer)
            m_lSwiftPartyID = Value
        End Set
    End Property

    Public Property IgnoreDriversAndWitnesses() As Boolean
        Get
            Return m_bIgnoreDriversAndWitnesses
        End Get
        Set(ByVal Value As Boolean)
            m_bIgnoreDriversAndWitnesses = Value
        End Set
    End Property

    Public Property AgentTypes() As Object
        Get
            Return m_vAgentTypes
        End Get
        Set(ByVal Value As Object)

            m_vAgentTypes = Value
        End Set
    End Property

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

    Public Property DateCancelled() As String
        Get
            Return m_vDateCancelled
        End Get
        Set(ByVal Value As String)
            m_vDateCancelled = CStr(Value)
        End Set
    End Property
    Public WriteOnly Property ExcludeMultiInsurer() As Boolean
        Set(ByVal Value As Boolean)
            m_bExcludeMultiInsurer = Value
        End Set
    End Property

    Public Property PartyOther() As Boolean
        Get
            Return m_bPartyOther
        End Get
        Set(ByVal Value As Boolean)
            m_bPartyOther = Value
        End Set
    End Property

    Public Property CountryId() As Integer
        Get
            Return m_lCountryId
        End Get
        Set(ByVal Value As Integer)
            m_lCountryId = Value
        End Set
    End Property

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

    Private Function GetDPASetting() As Integer
        Dim result As Integer = 0
        Try
            Dim sTemp As String = ""
            Dim vTemp As String = ""
            Dim lReturn As Integer
            Dim sValue As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue
            m_bDPAIsActive = False
            m_bDPAIsEnforced = False

            If m_sAgencyOrunderwriting <> "A" Then
                ' not used in underwriting systems
                Return result
            End If

            ' is the FSA module switched on
            lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableFSACompliance, v_vBranch:=g_iLanguageID, r_vUnderwriting:=vTemp)

            If Convert.IsDBNull(vTemp) Or IsNothing(vTemp) Then
                Return result
            ElseIf Conversion.Val(vTemp) <> 1 Then
                Return result
            End If

            ' only enabled for personal clients
            If m_sSpecialParty <> "" Then
                Return result
            End If

            m_bDPAIsActive = True

            'FSA Phase 3.1
            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=93, r_sOptionValue:=sValue)

            If sValue = "1" Then
                m_bDPAIsEnforced = True
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDPASetting Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDPASetting", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    Public Function DisplayLookupDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.
            m_lReturn = GetLookupValues()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Get all of the lookup details.
            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupSource, ctlLookup:=cmbBranch)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Gets all of the lookup values.
            ' Check the task.
            ' Get all of the lookup values.

            m_lReturn = g_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageId:=g_iLanguageID, vResultArray:=m_vLookUpDetails)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")

                Return result
            End If
            Return result

        Catch excep As System.Exception
            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    'developer guide no. change contol to combobox
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As ComboBox) As Integer

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
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")

                Return result
            End If

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.

            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
                ' Add the details to the control.
                Dim listindex As Integer = ctlLookup.Items.Add(New VB6.ListBoxItem((m_vLookUpDetails(ACDetailDesc, lCntr)), CInt(m_vLookUpDetails(ACDetailKey, lCntr))))

                ' Check if this is the selected index.
                If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
                    If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookUpDetails(ACDetailKey, lCntr)) Then
                        ctlLookup.SelectedIndex = listindex
                    End If
                End If

            Next lCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to zero.
            If CStr(m_vLookupValues(ACValueNumber, lRow)) = "" Then
                ctlLookup.SelectedIndex = 0
            End If

            Return result

        Catch excep As System.Exception
            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' GetBusiness
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBusiness() As Integer

        Dim nResult As Integer
        Dim sPartyTypeOther As String = ""
        Dim nBranchID As Integer
        Dim sAgentGroup As String = ""

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Display a searching message.
            DisplayStatusSearching()

            ' Disable parts of the interface while
            ' a search is in progress.
            nResult = DisableInterface(bDisable:=True)

            ' Check for errors
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return nResult
            End If

            If IsRetained Then
                g_oBusiness.RetainedValue = RetainedValue

                g_oBusiness.IsRetained = True
            End If

            If ReinsuranceTypeArray <> "" Then

                g_oBusiness.ReinsuranceTypeArray = ReinsuranceTypeArray
            End If

            g_oBusiness.SuppressCancelledAgents = m_bSuppressCancelledAgents

            If m_bPartyOther Then
                'Set client type using common part of party type code contained in constant
                'and unique numeric segment contained in combo.ItemData.
                If cmbOtherPartyType.SelectedIndex > 0 Then
                    sPartyTypeOther = CStr(m_vOtherPartyTypes(2, VB6.GetItemData(cmbOtherPartyType, cmbOtherPartyType.SelectedIndex)))
                Else
                    sPartyTypeOther = PMBConst.PMBPartyTypeOther
                End If

                m_lPartyCnt = 0

                g_oBusiness.PartyCnt = m_lPartyCnt
                m_bSpecialPartyQuery = False 'CT 10/08/00   set form flag to say which query called

                g_oBusiness.IgnoreDriversAndWitnesses = m_bIgnoreDriversAndWitnesses

                If chkIncludeClosedBranches.CheckState = CheckState.Checked Then
                    'ignore searching on valid SourceArray

                    nResult = g_oBusiness.SearchByQuery(r_lNumberOfRecords:=ACMaxSearchDetails,
                                                          r_vResultArray:=m_vSearchData,
                                                          v_vShortName:=txtShortName.Text,
                                                          v_vName:=txtLongName.Text,
                                                          v_vFileCode:=txtFileCode.Text,
                                                          v_vClientType:=sPartyTypeOther,
                                                          v_vAddress1:=txtAddress1.Text,
                                                          v_vPostalCode:=txtPostalCode.Text,
                                                          v_vAreaCode:=txtTelephoneCode.Text,
                                                          v_vNumber:=txtTelephone.Text,
                                                          v_vInsuranceRef:=txtInsReference.Text,
                                                          v_vDOB:=txtDOB.Text,
                                                          v_vSwiftPartyID:=m_lSwiftPartyID,
                                                          v_vValidSourceArray:=m_vSourceArrayIncludeClosedBranch,
                                                          v_vRiskIndex:=txtRiskIndex.Text,
                                                          v_vClaimNumber:=txtClaimNumber.Text,
                                                          bIncludeAgent:=m_bAllowAgentSearch,
                                                          v_vCaseNumber:=txtCaseNumber.Text)
                Else

                    nResult = g_oBusiness.SearchByQuery(r_lNumberOfRecords:=ACMaxSearchDetails,
                                                          r_vResultArray:=m_vSearchData,
                                                          v_vShortName:=txtShortName.Text,
                                                          v_vName:=txtLongName.Text,
                                                          v_vFileCode:=txtFileCode.Text,
                                                          v_vClientType:=sPartyTypeOther,
                                                          v_vAddress1:=txtAddress1.Text,
                                                          v_vPostalCode:=txtPostalCode.Text,
                                                          v_vAreaCode:=txtTelephoneCode.Text,
                                                          v_vNumber:=txtTelephone.Text,
                                                          v_vInsuranceRef:=txtInsReference.Text,
                                                          v_vDOB:=txtDOB.Text,
                                                          v_vSwiftPartyID:=m_lSwiftPartyID,
                                                          v_vValidSourceArray:=m_vSourceArray,
                                                          v_vRiskIndex:=txtRiskIndex.Text,
                                                          v_vClaimNumber:=txtClaimNumber.Text,
                                                          bIncludeAgent:=m_bAllowAgentSearch,
                                                          v_vCaseNumber:=txtCaseNumber.Text)
                End If

            ElseIf (m_sSpecialParty = "") Then

                m_lPartyCnt = 0

                g_oBusiness.PartyCnt = m_lPartyCnt

                m_bSpecialPartyQuery = False

                If txtRiskIndex.Text.Trim() <> "" Then
                    nResult = GetClientsByRiskIndex()
                Else
                    If chkIncludeClosedBranches.CheckState = CheckState.Checked Then
                        'ignore searching on valid SourceArray

                        nResult = g_oBusiness.SearchByQuery(r_lNumberOfRecords:=ACMaxSearchDetails,
                                                              r_vResultArray:=m_vSearchData,
                                                              v_vShortName:=txtShortName.Text,
                                                              v_vName:=txtLongName.Text,
                                                              v_vFileCode:=txtFileCode.Text,
                                                              v_vClientType:=cmbType.Text,
                                                              v_vStatusType:=cmbStatus.Text,
                                                              v_vAddress1:=txtAddress1.Text,
                                                              v_vPostalCode:=txtPostalCode.Text,
                                                              v_vAreaCode:=txtTelephoneCode.Text,
                                                              v_vNumber:=txtTelephone.Text,
                                                              v_vInsuranceRef:=txtInsReference.Text,
                                                              v_vDOB:=txtDOB.Text,
                                                              v_vSwiftPartyID:=m_lSwiftPartyID,
                                                              v_vAgentCnt:=m_lUserAgentCnt,
                                                              v_vValidSourceArray:=m_vSourceArrayIncludeClosedBranch,
                                                              v_vRiskIndex:=txtRiskIndex.Text,
                                                              v_vClaimNumber:=txtClaimNumber.Text,
                                                              bIncludeAgent:=m_bAllowAgentSearch,
                                                              v_vCaseNumber:=txtCaseNumber.Text)
                    Else

                        nResult = g_oBusiness.SearchByQuery(r_lNumberOfRecords:=ACMaxSearchDetails,
                                                              r_vResultArray:=m_vSearchData,
                                                              v_vShortName:=txtShortName.Text,
                                                              v_vName:=txtLongName.Text,
                                                              v_vFileCode:=txtFileCode.Text,
                                                              v_vClientType:=cmbType.Text,
                                                              v_vStatusType:=cmbStatus.Text,
                                                              v_vAddress1:=txtAddress1.Text,
                                                              v_vPostalCode:=txtPostalCode.Text,
                                                              v_vAreaCode:=txtTelephoneCode.Text,
                                                              v_vNumber:=txtTelephone.Text,
                                                              v_vInsuranceRef:=txtInsReference.Text,
                                                              v_vDOB:=txtDOB.Text,
                                                              v_vSwiftPartyID:=m_lSwiftPartyID,
                                                              v_vAgentCnt:=m_lUserAgentCnt,
                                                              v_vValidSourceArray:=m_vSourceArray,
                                                              v_vRiskIndex:=txtRiskIndex.Text,
                                                              v_vClaimNumber:=txtClaimNumber.Text,
                                                              bIncludeAgent:=m_bAllowAgentSearch,
                                                              v_vCaseNumber:=txtCaseNumber.Text)
                    End If
                End If
            Else

                m_bSpecialPartyQuery = True

                If cmbAgentGroup.Text <> "<ALL>" AndAlso Not String.IsNullOrEmpty(cmbAgentGroup.Text) Then
                    If m_oAgentGroup IsNot Nothing AndAlso IsArray(m_oAgentGroup) Then
                        For iCnt As Integer = 0 To UBound(m_oAgentGroup, 2)
                            If cmbAgentGroup.Text = Trim(m_oAgentGroup(3, iCnt)) Then
                                sAgentGroup = Trim(ToSafeString(m_oAgentGroup(2, iCnt)))
                            End If
                        Next iCnt
                    End If
                End If

                g_oBusiness.PartySourceId = m_lPartySourceId

                If cmbBranch.SelectedIndex >= 0 Then
                    nBranchID = VB6.GetItemData(cmbBranch, cmbBranch.SelectedIndex)
                Else
                    nBranchID = m_iSourceID
                End If

                'searching on closed branches also (implemented only
                'for underwriting)
                If m_sAgencyOrunderwriting = "U" And chkIncludeClosedBranches.CheckState = CheckState.Checked Then
                    'ignore searching on valid SourceArray
                    nResult = g_oBusiness.SearchSpecialPartyByQuery(r_lNumberOfRecords:=ACMaxSearchDetails, r_vResultArray:=m_vSearchData,
                                                                    v_vShortName:=txtShortName.Text, v_vName:=txtLongName.Text,
                                                                    v_vFileCode:=txtFileCode.Text, v_vClientType:=cmbType.Text,
                                                                    v_vAgentType:=cmbAgentType.Text, v_vStatusType:=cmbStatus.Text,
                                                                    v_vAddress1:=txtAddress1.Text, v_vPostalCode:=txtPostalCode.Text,
                                                                    v_vAreaCode:=txtTelephoneCode.Text, v_vNumber:=txtTelephone.Text,
                                                                    v_vInsuranceRef:=txtInsReference.Text, v_vValidSourceArray:=m_vSourceArrayIncludeClosedBranch,
                                                                    v_vBranch:=nBranchID, v_vActiveStatus:=VB6.GetItemData(cmbActiveStatus, cmbActiveStatus.SelectedIndex),
                                                                    v_bSuppressSubAgents:=m_bSuppressSubAgents, v_bIsInTransferMode:=m_bIsInTransferMode,
                                                                    v_lCommissionLevel:=m_lCommissionlevel, sAgentGroup:=sAgentGroup)
                Else
                    If m_sCallingAppName.ToUpper() = "UCTTHIRDPARTY" And m_sSpecialParty = "AG" Then
                        g_oBusiness.IgnoreViewableOnlyAgents = True
                    End If
                    If m_sSpecialParty = "AH" Then
                        nResult = g_oBusiness.SearchSpecialPartyByQuery(r_lNumberOfRecords:=ACMaxSearchDetails, r_vResultArray:=m_vSearchData, v_vShortName:=txtShortName.Text, v_vName:=txtLongName.Text, v_vFileCode:=txtFileCode.Text, v_vClientType:=cmbType.Text, v_vAgentType:=cmbAgentType.Text, v_vStatusType:=cmbStatus.Text, v_vAddress1:=txtAddress1.Text, v_vPostalCode:=txtPostalCode.Text, v_vAreaCode:=txtTelephoneCode.Text, v_vNumber:=txtTelephone.Text, v_vInsuranceRef:=txtInsReference.Text, v_vBranch:=nBranchID, v_vActiveStatus:=VB6.GetItemData(cmbActiveStatus, cmbActiveStatus.SelectedIndex), v_bSuppressSubAgents:=m_bSuppressSubAgents, v_bIsInTransferMode:=m_bIsInTransferMode, v_vRiskTransfer:=m_bRiskTransfer, v_vInsurerType:=VB6.GetItemString(cmbActiveStatus, cmbActiveStatus.SelectedIndex), v_lCommissionLevel:=m_lCommissionlevel)
                    Else
                        If cmbActiveStatus.SelectedIndex <> -1 Then
                            nResult = g_oBusiness.SearchSpecialPartyByQuery(r_lNumberOfRecords:=ACMaxSearchDetails, r_vResultArray:=m_vSearchData, v_vShortName:=txtShortName.Text, v_vName:=txtLongName.Text, v_vFileCode:=txtFileCode.Text, v_vClientType:=cmbType.Text, v_vAgentType:=cmbAgentType.Text, v_vStatusType:=cmbStatus.Text, v_vAddress1:=txtAddress1.Text, v_vPostalCode:=txtPostalCode.Text, v_vAreaCode:=txtTelephoneCode.Text, v_vNumber:=txtTelephone.Text, v_vInsuranceRef:=txtInsReference.Text, v_vValidSourceArray:=m_vSourceArray, v_vBranch:=nBranchID, v_vActiveStatus:=VB6.GetItemData(cmbActiveStatus, cmbActiveStatus.SelectedIndex), v_bSuppressSubAgents:=m_bSuppressSubAgents, v_bIsInTransferMode:=m_bIsInTransferMode, v_vRiskTransfer:=m_bRiskTransfer, v_vInsurerType:=VB6.GetItemString(cmbActiveStatus, cmbActiveStatus.SelectedIndex), v_lCommissionLevel:=m_lCommissionlevel, sAgentGroup:=sAgentGroup)
                        End If
                    End If
                End If
            End If

            ' Check the return values.
            Select Case (nResult)
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Found search details.
                Case gPMConstants.PMEReturnCode.PMNotFound
                    ' No search details found.
                Case Else
                    ' Failed to get details.
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return nResult
            End Select

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetClientsByRiskIndex
    '
    ' Description: Gets Claims by Risk
    '
    ' ***************************************************************** '
    Private Function GetClientsByRiskIndex() As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim sIndex As String = ""
        Dim vGISSearchDataArray As Object = Nothing
        Dim vClaimsData(,) As Object = Nothing
        Dim vResultData(,) As Object = Nothing
        Dim iFromRow, iMaxRow, iNumClaims As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sIndex = txtRiskIndex.Text.Trim()

            lReturn = g_oBackofficelink.FindLikeIndex(sIndex:=sIndex, lNumberOfRecords:=gPMConstants.PMAllRecords, vResultArray:=vGISSearchDataArray, sDataModelType:="CLAIM")

            'clear this array, were going to use it again
            'and definitely dont want to display it or append to it later

            m_vSearchData = Nothing

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to FindLikeIndex", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientsByRiskIndex")
                Return gPMConstants.PMEReturnCode.PMFalse

            Else
                ' We have the Insurance File Cnt
                lReturn = g_oClaimBusiness.GetMultiPolicyClaims(vGISSearchDataArray, vClaimsData, v_vSiriusProduct:=g_oBackofficelink.Sirius_Product, v_vClaimNumber:=txtClaimNumber.Text, v_vRegNumber:=txtRiskIndex.Text)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetMultiPolicyClaims failed to get Policy Details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientsByRiskIndex")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If NO Indexes were found return Not Found
                If Not Information.IsArray(vClaimsData) Then
                    result = gPMConstants.PMEReturnCode.PMNotFound
                Else
                    'We have claim number(s) now use these to get the clients
                    For iCounter1 As Integer = vClaimsData.GetLowerBound(1) To vClaimsData.GetUpperBound(1)

                        m_lReturn = g_oBusiness.SearchByQuery(r_lNumberOfRecords:=ACMaxSearchDetails, r_vResultArray:=vResultData, v_vClaimNumber:=vClaimsData(3, iCounter1))

                        ' Get the no of fields selected
                        iNumClaims = vResultData.GetUpperBound(0)
                        If Not Information.IsArray(m_vSearchData) Then
                            m_vSearchData = vResultData
                        Else
                            ' We alreay have some data and we have to merge it with new data
                            iFromRow = m_vSearchData.GetUpperBound(1)
                            iMaxRow = m_vSearchData.GetUpperBound(1) + vResultData.GetUpperBound(1) + 1
                            ReDim Preserve m_vSearchData(iNumClaims, iMaxRow)
                            For iCounter2 As Integer = vResultData.GetLowerBound(1) To vResultData.GetUpperBound(1)
                                iFromRow += 1
                                For iCounter3 As Integer = 0 To iNumClaims


                                    m_vSearchData(iCounter3, iFromRow) = vResultData(iCounter3, iCounter2)
                                Next iCounter3
                            Next iCounter2
                        End If
                    Next
                End If
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to validate index", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientsByRiskIndex", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

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

        Dim dDOB As Date
        'JAS(CMG) - Added second list image - FindImage2
        Dim icoFindImage As String = ""

        Const ACFindImage As String = "FindImage"
        Const ACFindImageDeletedAon As String = "FindImage2"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Update the interface details.

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

            If m_bDOBEnabled Then
                lvwSearchDetails.Columns.Item(g_kLvwColumnDateOfBirth - 1).Width = CInt(VB6.TwipsToPixelsX(1400))
            Else
                lvwSearchDetails.Columns.Item(g_kLvwColumnDateOfBirth - 1).Width = CInt(0)
            End If

            If m_sSpecialParty.Trim() = "" Then
                lvwSearchDetails.Columns.Item(g_kLvwColumnStatus - 1).Width = CInt(VB6.TwipsToPixelsX(700))
            Else
                lvwSearchDetails.Columns.Item(g_kLvwColumnStatus - 1).Width = CInt(0)
            End If

            ' Assign the details to the interface.

            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
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
                If m_vSearchData.GetUpperBound(0) >= ACIrecord_status Then

                    If CStr(m_vSearchData(ACIrecord_status, lRow)) = "aon_d" Then
                        icoFindImage = ACFindImageDeletedAon
                    Else
                        icoFindImage = ACFindImage
                    End If
                Else
                    icoFindImage = ACFindImage
                End If

                oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(ACIShortName, lRow)).Trim(), 0)

                ' Assign details to other the columns
                ' Column 2 Long Name

                ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnName - 1).Text = CStr(m_vSearchData(ACILongName, lRow)).Trim()

                If m_sSpecialParty.Trim() = "" Or m_sSpecialParty.Trim() = "PC" Then
                    ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnSurname - 1).Text = CStr(m_vSearchData(ACISurname, lRow)).Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnForename - 1).Text = CStr(m_vSearchData(ACIForename, lRow)).Trim()
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnSurname - 1).Text = ""
                    ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnForename - 1).Text = ""
                End If
                ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnAddressLine1 - 1).Text = CStr(m_vSearchData(ACIAddress1, lRow)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnAddressLine2 - 1).Text = CStr(m_vSearchData(ACIAddress2, lRow)).Trim()

                If m_sSpecialParty.Trim() = "" Or m_sSpecialParty.Trim() = "PC" Then
                    ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnAddressLine3 - 1).Text = CStr(m_vSearchData(ACIAddress3, lRow)).Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnAddressLine4 - 1).Text = CStr(m_vSearchData(ACIAddress4, lRow)).Trim()
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnAddressLine3 - 1).Text = ""
                    ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnAddressLine4 - 1).Text = ""
                End If
                ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnPostCode - 1).Text = CStr(m_vSearchData(ACIPostalCode, lRow)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnFileCode - 1).Text = CStr(m_vSearchData(ACIFileCode, lRow)).Trim()

                Dim sPartyTypeCode As String = String.Empty
                If m_bSpecialPartyQuery Then
                    sPartyTypeCode = CStr(m_vSearchData(ACIPartyTypeCode_SpecialParty, lRow)).Trim().ToUpper()
                Else
                    sPartyTypeCode = CStr(m_vSearchData(ACIPartyTypeCode, lRow)).Trim().ToUpper()
                End If

                If sPartyTypeCode = PMBConst.PMBPartyTypeAgent.ToUpper() Then
                    ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnType - 1).Text = CStr(m_vSearchData(ACIAgentType, lRow)).Trim()
                ElseIf CStr(m_vSearchData(ACIPartyType, lRow)).Trim() = PMBConst.PMBPartyTypeInsurerText Then

                    If CStr(m_vSearchData(8, lRow)) = "4" Then
                        ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnType - 1).Text = PMBConst.PMBPartyTypeCoinsurerText
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnType - 1).Text = PMBConst.PMBPartyTypeReinsurerText
                    End If
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnType - 1).Text = CStr(m_vSearchData(ACIPartyType, lRow)).Trim()
                End If

                If m_sStructure = gSIRLibrary.SIRPMBSolution Then
                    ' Column 8 Party Status Code
                    If CStr(m_vSearchData(ACIPartyType, lRow)).Trim().ToUpper() = "AGENT" Then
                        ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnStatus - 1).Text = CStr(m_vSearchData(ACIPartyType, lRow))
                    Else
                        If CStr(m_vSearchData(ACIPartyStatus, lRow)).Trim() = "1" Then
                            ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnStatus - 1).Text = "Prospect"
                        Else
                            ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnStatus - 1).Text = "Client"
                        End If
                    End If
                End If
                ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnSource - 1).Text = CStr(m_vSearchData(ACISourceName, lRow)).Trim()

                If (Not (m_bSpecialPartyQuery)) And m_vSearchData.GetUpperBound(0) > 10 Then 'CT 10/08/00 addded criteria that m_vSearchData must come from SearchByQuery procedure in BSirFindParty
                    If g_bGenericConnectionStatus Then
                        ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnSource - 1).Text = CStr(m_vSearchData(ACISource, lRow)).Trim()
                    End If
                End If
                If m_bDOBEnabled And (Not (m_bSpecialPartyQuery)) Then

                    If CStr(m_vSearchData(ACIDOB, lRow)) <> "" Then

                        dDOB = CDate(m_vSearchData(ACIDOB, lRow))
                        If Not (dDOB.Year = 1899 And dDOB.Month = 12 And DateAndTime.Day(dDOB) = 29) Then
                            ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnDateOfBirth - 1).Text = CStr(dDOB).Trim()
                        End If
                    End If
                End If

                If m_bSwiftEnabled Then
                    ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnSwiftLink - 1).Text = IIf(Conversion.Val(CStr(m_vSearchData(ACISwiftPartyID, lRow))) > 0, "Yes", "No")
                End If

                If m_sSpecialParty = PMBConst.PMBPartyTypeAgentGroup Then
                    ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnActiveStatus - 1).Text = CStr(m_vSearchData(ACIActiveStatus, lRow)).Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnBranch - 1).Text = CStr(m_vSearchData(ACIBranch, lRow)).Trim()
                End If

                ' Set the tag property with the index of
                ' the search data storage.
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

            Next lRow

            ' Select the first item.
            If lvwSearchDetails.Items.Count > 0 Then

                lvwSearchDetails.Items.Item(0).Selected = True
                If Not lvwSearchDetails.FocusedItem Is Nothing Then
                    m_lPartyCnt = CInt(m_vSearchData(ACIPartyCnt, Convert.ToString(lvwSearchDetails.FocusedItem.Tag)))
                End If
                ' Enable the interface now that the search has completed.
                m_lReturn = DisableInterface(bDisable:=False)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Display the number of items found message.
            DisplayStatusFound()

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
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
        Dim iSourceID As Integer
        Dim lPartyID As Integer
        Dim bIsAgent, bIsAgencyAgreementValid As Boolean
        Dim sTitle, sMsg As String
        Dim bExistsOnSirius As Boolean
        Dim vRIBrokerStatus As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the selected item's tag, so we can use this
            ' as the index to the search data storage details.

            If IsNothing(lvwSearchDetails.FocusedItem) Then
                lSelectedItem = 0
            Else
                lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)
            End If

            m_lPartyCnt = CInt(m_vSearchData(ACIPartyCnt, lSelectedItem))

            m_sShortName = CStr(m_vSearchData(ACIShortName, lSelectedItem)).Trim()

            m_sLongName = CStr(m_vSearchData(ACILongName, lSelectedItem)).Trim()

            m_sResolvedName = CStr(m_vSearchData(ACIResolvedName, lSelectedItem)).Trim()
            If m_sPartyType = "<ALL>" Or m_sPartyType = "Personal Client" Or m_sPartyType = "Corporate Client" Or m_sPartyType = "Group Client" Then
                m_iCurrencyId = CInt(m_vSearchData(ACICurrencyID, lSelectedItem))
            End If
            If CStr(m_vSearchData(ACIPartyType, lSelectedItem)).Trim() = "Insurer" Then

                m_bIsRiBroker = gPMFunctions.ToSafeBoolean(CStr(m_vSearchData(ACIIsRiBroker, lSelectedItem)))
            End If

            If m_bIsRiBroker And (m_sCallingAppName = "iPMUReinsurance2007" Or m_sCallingAppName = "iCLMReinsurance2007" Or m_sCallingAppName = "iPMUTreaty") Then
                ProcessRIBrokerInterface(vRIBrokerStatus)
                If vRIBrokerStatus = gPMConstants.PMEReturnCode.PMCancel Then
                    Return gPMConstants.PMEReturnCode.PMCancel
                End If
            End If
            m_sFileCode = CStr(m_vSearchData(ACIFileCode, lSelectedItem)).Trim()

            If CStr(m_vSearchData(ACIDOB, lSelectedItem)).Trim() <> "" Then

                m_vDateOfBirth = CStr(m_vSearchData(ACIDOB, lSelectedItem))
            Else
                m_vDateOfBirth = ""
            End If

            If CStr(m_vSearchData(ACIPartyType, lSelectedItem)).Trim() <> "" Then

                m_sPartyType = CStr(m_vSearchData(ACIPartyType, lSelectedItem))
            Else
                m_sPartyType = ""
            End If
            If CStr(m_vSearchData(ACIPartyStatus, lSelectedItem)) = "1" Then
                m_sPartyStatus = "Prospect"
            Else
                m_sPartyStatus = "Client"
            End If

            m_lSwiftPartyID = CInt(Conversion.Val(CStr(m_vSearchData(ACISwiftPartyID, lSelectedItem)).Trim()))

            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(m_vSearchData(ACIAgentCnt, lSelectedItem)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                m_lPartyAgentCnt = CInt(m_vSearchData(ACIAgentCnt, lSelectedItem)) 'PN13921
            End If

            Dim dbNumericTemp2 As Double
            If Double.TryParse(CStr(m_vSearchData(ACISourceID, lSelectedItem)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                m_lPartySourceId = CInt(m_vSearchData(ACISourceID, lSelectedItem)) 'PN13921
            End If

            bExistsOnSirius = True

            If (Not (m_bSpecialPartyQuery)) And m_vSearchData.GetUpperBound(0) > 10 Then 'CT 10/08/00 added criteria that m_vSearchData must come from SearchByQuery procedure in BSirFindParty, not from SearchSpecialPartyByQuery

                Dim dbNumericTemp3 As Double
                If Double.TryParse(CStr(m_vSearchData(ACIInvariantKey, lSelectedItem)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then

                    m_lInvariantKey = CInt(m_vSearchData(ACIInvariantKey, lSelectedItem))
                End If

                If CStr(m_vSearchData(ACISource, lSelectedItem)).Trim() = "Broking" Then
                    bExistsOnSirius = False
                End If
            End If

            If m_sLongName = "" Then
                m_sLongName = m_sShortName
            End If
            If m_bSpecialPartyQuery Then
                If m_vSearchData.GetUpperBound(0) >= ACIPartyDateCancelled_SpecialParty Then

                    If CStr(m_vSearchData(ACIPartyDateCancelled_SpecialParty, lSelectedItem)).Trim() <> "" Then

                        m_vDateCancelled = CStr(m_vSearchData(ACIPartyDateCancelled_SpecialParty, lSelectedItem))
                    Else
                        m_vDateCancelled = ""
                    End If
                End If
            Else
                If m_vSearchData.GetUpperBound(0) >= ACIPartyDateCancelled Then

                    If CStr(m_vSearchData(ACIPartyDateCancelled, lSelectedItem)).Trim() <> "" Then

                        m_vDateCancelled = CStr(m_vSearchData(ACIPartyDateCancelled, lSelectedItem))
                    Else
                        m_vDateCancelled = ""
                    End If
                End If
            End If

            If m_bSpecialPartyQuery Then
                m_sSelectedPartyType = CStr(m_vSearchData(ACIPartyTypeCode_SpecialParty, lSelectedItem)).Trim()
            Else
                m_sSelectedPartyType = CStr(m_vSearchData(ACIPartyTypeCode, lSelectedItem)).Trim()
            End If

            m_iAllowConsolidatedCommission = Conversion.Val(CStr(m_vSearchData(ACIAllowConsolidatedCommission, lSelectedItem)))

            Select Case m_sSelectedPartyType
                Case PMBConst.PMBPartyTypePersonalClient
                    m_iAgentOnly = 1
                Case PMBConst.PMBPartyTypeCorporateClient
                    m_iAgentOnly = 2
                Case PMBConst.PMBPartyTypeGroupClient
                    m_iAgentOnly = 3
                Case PMBConst.PMBPartyTypeAgent
                    m_iAgentOnly = 4
            End Select

            m_lReturn = g_oBusiness.GetFullAddress(v_lPartyCnt:=m_lPartyCnt, r_vAddress1:=m_sAddressLine1, r_vAddress2:=m_sAddressLine2, r_vAddress3:=m_sAddressLine3, r_vAddress4:=m_sAddressLine4, r_vPostalCode:=m_sPostalCode, r_vCountryID:=m_lCountryId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If bExistsOnSirius Then

                iSourceID = CInt(m_vSearchData(ACISourceID, lSelectedItem))

                lPartyID = CInt(m_vSearchData(ACIPartyID, lSelectedItem))

                m_lPartyUIK = m_lPartyCnt

                m_lReturn = g_oBusiness.CalcCombinedKey(v_lSourceId:=iSourceID, v_lKeyID:=lPartyID, r_lCombinedKeyID:=m_lPartyUIK)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the Party UIK.", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                m_lReturn = g_oBusiness.CheckAgencyAgreement(v_lPartyCnt:=m_lPartyCnt, r_bIsAgent:=bIsAgent, r_bIsAgencyAgreementValid:=bIsAgencyAgreementValid)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (Not bIsAgent) Then
                    Return result
                End If

                If Not bIsAgencyAgreementValid Then

                    sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNoAgencyAgreementTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNoAgencyAgreement, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    m_lReturn = MessageBox.Show(sMsg, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)

                End If

            End If
            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPolicyInfo
    '
    ' Description:  Instance FindInsurance to retrieve Policy reference
    '
    ' ***************************************************************** '
    Public Function GetPolicyInfo() As Integer
        Dim result As Integer = 0


        Dim oFindPolicy As iPMBFindInsurance.Interface_Renamed

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Find Insurance object
            Dim temp_oFindPolicy As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindPolicy, sClassName:="iPMBFindInsurance.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindPolicy = temp_oFindPolicy

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'iPMBFindInsurance.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyInfo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Set component properties and start interface
            oFindPolicy.CallingAppName = ACApp
            oFindPolicy.InsReference = txtInsReference.Text
            oFindPolicy.FindMode = 1 'PN 36697
            m_lReturn = oFindPolicy.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                'Start PN: 61332 - Check if the interface is already open
                If m_lReturn = 400 Then
                    MessageBox.Show("The Find Insurance form is already open. Cannot open another instance.", "iPMBFindParty")
                    m_lReturn = 400
                    'End PN: 61332
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process object 'iPMBFindInsurance.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyInfo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If
                Return result
            End If

            'Retrieve InsuranceRef and set as PolicyRef
            If oFindPolicy.Status <> gPMConstants.PMEReturnCode.PMCancel Then

                m_sInsuranceRef = oFindPolicy.InsReference
                'Display Policy Reference on form
                txtInsReference.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sInsuranceRef.Trim())
                ' Destroy Find Insurance object
                oFindPolicy.Dispose()
                oFindPolicy = Nothing
                ' Do search
                cmdFindNow_Click(cmdFindNow, New EventArgs())
            Else
                ' Destroy Find Insurance object

                oFindPolicy.Dispose()
                oFindPolicy = Nothing
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyInfoFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyInfo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DeleteParty
    '
    ' Description:
    '
    ' History: 14/01/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function DeleteParty() As Integer

        Dim result As Integer = 0
        Dim bOKToDelete As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            g_oBusiness.PartyCnt = PartyCnt

            bOKToDelete = g_oBusiness.OKToDelete()
            If Not bOKToDelete Then

                MessageBox.Show("Cannot delete -" & Strings.Chr(13) & Strings.Chr(10) & g_oBusiness.NoDeleteReasons, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            End If

            m_lReturn = LockParty()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Cannot delete -" & Strings.Chr(13) & Strings.Chr(10) & "Party locked by another user", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            End If

            m_lReturn = g_oBusiness.DeleteParty
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            m_lReturn = UnlockParty()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            MessageBox.Show("Client " & m_sShortName & " has successfully been deleted", "Delete Successful", MessageBoxButtons.OK)

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    '***************************************************************
    'Name: CheckQASNames
    '
    'Check registry for installation of QASNames
    '
    '
    '****************************************************************
    Private Function CheckQASNames() As Boolean
        Dim result As Boolean = False

        Try

            Dim oOptions As bSIROptions.Business
            Dim sValue As String = ""

            ' Get an instance of System Options
            Dim temp_oOptions As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oOptions, "bSIROptions.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oOptions = temp_oOptions
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            'get QAS option
            m_lReturn = oOptions.GetOption(iOptionNumber:=13, sValue:=sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            'is this the right option for QAS Names
            If Conversion.Val(sValue) = ACQASNames Then
                result = True
            End If

            Return result

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckQASNames Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckQASNames", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: LockParty
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function LockParty() As Integer

        Dim result As Integer = 0
        Dim sLockedBy As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = m_oPMLock.LockKey(sKeyName:="party_cnt", vKeyValue:=m_lPartyCnt, iUserID:=g_oObjectManager.UserID, sCurrentlyLockedBy:=sLockedBy)

            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    'OK

                Case gPMConstants.PMEReturnCode.PMFalse
                    'Locked or error
                    If sLockedBy = "ERROR" Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to lock record", vApp:=ACApp, vClass:=ACClass, vMethod:="LockParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    Else
                        result = gPMConstants.PMEReturnCode.PMFalse
                        MessageBox.Show("Party currently locked by " & sLockedBy &
                                        Strings.Chr(13) & Strings.Chr(10) & "Please try later", "Party Lock")
                        Return result
                    End If

                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock the party", vApp:=ACApp, vClass:=ACClass, vMethod:="LockParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result

            End Select

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UnlockParty
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function UnlockParty() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = m_oPMLock.UnLockKey(sKeyName:="party_cnt", vKeyValue:=m_lPartyCnt, iUserID:=g_oObjectManager.UserID)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to unlock the party", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result

            End If
            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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
            txtShortName.Text = m_sShortName.Trim()
            txtLongName.Text = m_sLongName.Trim()
            txtAddress1.Text = m_sAddressLine1.Trim()
            txtPostalCode.Text = m_sPostalCode.Trim()
            txtTelephoneCode.Text = m_sTelAreaCode.Trim()
            txtTelephone.Text = m_sTelNumber.Trim()
            txtInsReference.Text = m_sInsuranceRef.Trim()
            txtDOB.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, m_vDateOfBirth)

            Select Case m_sPartyType
                Case "<ALL>", "Personal Client", "Corporate Client", "Group Client"
                    cmbType.Text = m_sPartyType
                Case Else
                    ' Do nothing
            End Select

            Select Case m_sPartyStatus
                Case "Client", "Prospect"
                    cmbStatus.Text = m_sPartyStatus
                Case Else
                    ' Do nothing
            End Select
            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: HideColumns
    '
    ' Description: Hides columns. Leaves "v_iShowLeft" showing.
    '
    ' ***************************************************************** '
    Private Function HideColumns(ByVal v_iShowLeft As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iLoop1 As Integer = v_iShowLeft + 1 To lvwSearchDetails.Columns.Count
                lvwSearchDetails.Columns.Item(iLoop1 - 1).Width = CInt(0)
            Next iLoop1

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="HideColumns Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="HideColumns", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' SetInterfaceDefaults
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetInterfaceDefaults() As Integer

        Dim nResult As Integer
        Dim bRestrictedInsurerAccess As Boolean

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            Dim bIsSystemAdministrator As Boolean

            m_lReturn = g_oBusiness.GetStructure(sStructure:=m_sStructure)

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            m_lReturn = PartyFunc.GetHiddenOptions(v_lSourceId:=g_iSourceID, r_vIsNRMA:=m_bIsNRMA, r_vAONAffinity:=m_bAONAffinity)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.
            Select Case (m_lNavigate)
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = True

                Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
                    cmdNavigate.Visible = False
                    cmdNavigate.Enabled = False

                Case Else
                    cmdNavigate.Visible = False
            End Select

            If Not m_bPartyOther Then
                If (m_sSpecialParty.Trim() <> "") And (m_sSpecialParty <> PMBConst.PMBPartyTypeAgentGroup) Then 'PN21980
                    If Not Information.IsArray(m_vValidPartyTypesArray) Then
                        SetupValidPartyTypeArray()
                    End If
                End If
            End If

            If (m_sSpecialParty = PMBConst.PMBPartyTypeAgent) Or (m_sSpecialParty = PMBConst.PMBPartyTypeSubAgent) Or (m_sSpecialParty = PMBConst.PMBPartyTypeCommissionAccount) Or (m_sSpecialParty = PMBConst.PMBPartyTypeIntermediary) Then
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
                    cmbAgentType.Items.Insert(0, "<ALL>")
                    cmbAgentType.Items.Insert(1, PMBConst.PMBAgentTypeBrokerText)
                    If Not (m_bSuppressSubAgents) Then
                        cmbAgentType.Items.Insert(2, PMBConst.PMBAgentTypeSubAgentText)
                        cmbAgentType.Items.Insert(3, PMBConst.PMBAgentTypeCommAccountText)
                        cmbAgentType.Items.Insert(4, PMBConst.PMBAgentTypeIntermediaryText)
                    Else
                        cmbAgentType.Items.Insert(2, PMBConst.PMBAgentTypeCommAccountText)
                        cmbAgentType.Items.Insert(3, PMBConst.PMBAgentTypeIntermediaryText)
                    End If

                End If
            End If

            'Check for 'Other Party' before setting combos.

            'CJR 20/1/2003 Added for IAG.  Alows the client app. to specify a list of valid parties
            If Information.IsArray(m_vValidPartyTypesArray) Then
                cmbType.Items.Clear()
                For Each m_vValidPartyTypesArray_item As Object In m_vValidPartyTypesArray
                    cmbType.Items.Add(CStr(m_vValidPartyTypesArray_item))
                Next m_vValidPartyTypesArray_item
                cmbType.SelectedIndex = 0

            End If

            If Not m_bPartyOther Then

                If m_sSpecialParty = PMBConst.PMBPartyTypeAgentGroup Then
                    If Not Information.IsArray(m_vValidPartyTypesArray) Then
                        'Add Agent group into type for passing to business obj
                        cmbType.Items.Clear()
                        cmbType.Items.Insert(0, PMBConst.PMBPartyTypeAgentGroupText)
                        cmbType.SelectedIndex = 0
                    End If
                    'Branch is populated by lookup table
                    cmbBranch.Visible = True
                    cmbBranch.Enabled = True
                    cmbBranch.Items.Clear()
                    cmbBranch.Items.Insert(0, "<ALL>")

                Else

                    'Populate combo box with Party Types (does not use lookup table)
                    'Branch not used
                    cmbBranch.Visible = False
                    cmbBranch.Enabled = False

                    If Not Information.IsArray(m_vValidPartyTypesArray) Then
                        cmbType.Items.Clear()
                        cmbType.Items.Insert(0, "<ALL>")
                        cmbType.Items.Insert(1, PMBConst.PMBPartyTypePersonalClientText)
                        cmbType.Items.Insert(2, PMBConst.PMBPartyTypeCorporateClientText)
                        cmbType.Items.Insert(3, PMBConst.PMBPartyTypeGroupClientText)

                        If m_sStructure <> gSIRLibrary.SIRPMBSolution Then
                            cmbType.Items.Insert(4, PMBConst.PMBPartyTypeAgentText)
                        ElseIf m_bAllowAgentSearch Then
                            cmbType.Items.Insert(4, PMBConst.PMBPartyTypeAgentText)
                        End If

                    End If

                    If m_sStructure = gSIRLibrary.SIRPMBSolution Then
                        If m_sSpecialParty <> "" Then
                            If Not Information.IsArray(m_vValidPartyTypesArray) Then
                                cmbType.Items.Insert(4, PMBConst.PMBPartyTypeAgentText)
                                cmbType.Items.Insert(5, PMBConst.PMBPartyTypeConsultantText)
                                cmbType.Items.Insert(6, PMBConst.PMBPartyTypeAccountHandlerText)
                                cmbType.Items.Insert(7, PMBConst.PMBPartyTypeReinsurerText)
                                cmbType.Items.Insert(8, PMBConst.PMBPartyTypeBrokerText)
                                cmbType.Items.Insert(9, PMBConst.PMBPartyTypeFeeText)
                                cmbType.Items.Insert(10, PMBConst.PMBPartyTypeExtraText)
                                cmbType.Items.Insert(11, PMBConst.PMBPartyTypeDiscountText)
                                cmbType.Items.Insert(12, PMBConst.PMBPartyTypeCommissionAccount)
                                cmbType.Items.Insert(13, PMBConst.PMBPartyTypeFinanceProviderText)

                            End If
                        Else
                            cmdEdit.Enabled = False
                            cmdEdit.Visible = False
                        End If
                    End If
                End If

            Else

                'Other Party stuff.
                If Not Information.IsArray(m_vValidPartyTypesArray) Then
                    cmbType.Items.Add(PMBConst.PMBPartyTypeOtherText)
                End If

                cmbType.SelectedIndex = 0
                cmbType.Enabled = False
                cmbAgentType.Visible = False
                cmbStatus.Visible = False
                cmbOtherPartyType.Visible = True

                'Get Other Party Types and populate combo.
                PopulateOtherPartyTypeLookup()

                'cmbOtherPartyType.Enabled = False
                mnuRecentFiles.Available = False
                ResizeIfNoMenu()
            End If

            If m_iNotEditable = 1 Then
                cmdNew.Enabled = False
                cmdNew.Visible = False
                cmdEdit.Enabled = False
                cmdEdit.Visible = False
            End If

            Select Case m_sTransactionType
                Case "EDIT", "MTC", "NB", "MTR"
                    If m_sTransactionType = "NB" Then
                        If m_sSpecialParty = "AH" Then
                            cmdNew.Enabled = False
                            cmdNew.Visible = False
                        End If
                    Else
                        cmdNew.Enabled = False
                        cmdNew.Visible = False
                    End If
                Case "MTA"
                    If m_sSpecialParty <> "IN" Then
                        cmdNew.Enabled = False
                        cmdNew.Visible = False
                    End If
            End Select

            m_lReturn = GetDPASetting()

            If Not m_bDPAIsActive Then
                ' Hide DPA stuff
                chkDPARequired.Visible = False
                lblDPARequired.Visible = False
                chkDPARequired.CheckState = CheckState.Unchecked
            Else
                ' Display DPA stuff
                chkDPARequired.Visible = True
                lblDPARequired.Visible = True
                If m_bDPAIsEnforced Then
                    chkDPARequired.CheckState = CheckState.Checked
                    chkDPARequired.Enabled = False
                Else
                    chkDPARequired.CheckState = CheckState.Unchecked
                End If
            End If
            chkIncludeClosedBranches.CheckState = CheckState.Unchecked
            chkIncludeClosedBranches.Visible = m_bIncludeClosedBranches

            ' Set the column widths for the search list.
            'DC081101 increased from 1400 to 2000
            lvwSearchDetails.Columns.Item(g_kLvwColumnClientCode - 1).Width = CInt(VB6.TwipsToPixelsX(2350))
            lvwSearchDetails.Columns.Item(g_kLvwColumnName - 1).Width = CInt(VB6.TwipsToPixelsX(2000))
            If (m_sStructure = gSIRLibrary.SIRPMBSolution) And (m_sSpecialParty = "" Or m_sSpecialParty = "PC") Then
                lvwSearchDetails.Columns.Item(g_kLvwColumnSurname - 1).Width = CInt(VB6.TwipsToPixelsX(2000))
                lvwSearchDetails.Columns.Item(g_kLvwColumnForename - 1).Width = CInt(VB6.TwipsToPixelsX(2000))
            Else
                lvwSearchDetails.Columns.Item(g_kLvwColumnSurname - 1).Width = CInt(0)
                lvwSearchDetails.Columns.Item(g_kLvwColumnForename - 1).Width = CInt(0)
            End If
            lvwSearchDetails.Columns.Item(g_kLvwColumnAddressLine1 - 1).Width = CInt(VB6.TwipsToPixelsX(2000))
            lvwSearchDetails.Columns.Item(g_kLvwColumnAddressLine2 - 1).Width = CInt(VB6.TwipsToPixelsX(2000))
            If (m_sStructure = gSIRLibrary.SIRPMBSolution) And (m_sSpecialParty = "" Or m_sSpecialParty = "PC") Then
                lvwSearchDetails.Columns.Item(g_kLvwColumnAddressLine3 - 1).Width = CInt(VB6.TwipsToPixelsX(2000))
                lvwSearchDetails.Columns.Item(g_kLvwColumnAddressLine4 - 1).Width = CInt(VB6.TwipsToPixelsX(2000))
            Else
                lvwSearchDetails.Columns.Item(g_kLvwColumnAddressLine3 - 1).Width = CInt(0)
                lvwSearchDetails.Columns.Item(g_kLvwColumnAddressLine4 - 1).Width = CInt(0)
            End If
            lvwSearchDetails.Columns.Item(g_kLvwColumnPostCode - 1).Width = CInt(VB6.TwipsToPixelsX(1300))
            If (m_sStructure = gSIRLibrary.SIRPMBSolution) And (m_sSpecialParty = "") Then
                lvwSearchDetails.Columns.Item(g_kLvwColumnFileCode - 1).Width = CInt(VB6.TwipsToPixelsX(1300))
            Else
                lvwSearchDetails.Columns.Item(g_kLvwColumnFileCode - 1).Width = CInt(0)
            End If

            lvwSearchDetails.Columns.Item(g_kLvwColumnType - 1).Width = CInt(VB6.TwipsToPixelsX(1300))
            lvwSearchDetails.Columns.Item(g_kLvwColumnStatus - 1).Width = CInt(VB6.TwipsToPixelsX(700))

            If g_bGenericConnectionStatus Then
                lvwSearchDetails.Columns.Item(g_kLvwColumnSource - 1).Width = CInt(VB6.TwipsToPixelsX(700))
            Else
                lvwSearchDetails.Columns.Item(g_kLvwColumnSource - 1).Width = CInt(VB6.TwipsToPixelsX(1400))
            End If
            If (m_sStructure = gSIRLibrary.SIRPMBSolution) And (m_sSpecialParty = "") Then
                lvwSearchDetails.Columns.Item(g_kLvwColumnDateOfBirth - 1).Width = CInt(VB6.TwipsToPixelsX(1400))
            Else
                lvwSearchDetails.Columns.Item(g_kLvwColumnDateOfBirth - 1).Width = CInt(0)
            End If

            If (m_sStructure = gSIRLibrary.SIRPMBSolution) And (m_sSpecialParty = "") Then
                lvwSearchDetails.Columns.Item(g_kLvwColumnSwiftLink - 1).Width = CInt(VB6.TwipsToPixelsX(1400))
                m_bSwiftEnabled = True
            Else
                lvwSearchDetails.Columns.Item(g_kLvwColumnSwiftLink - 1).Width = CInt(0)
                m_bSwiftEnabled = False
            End If

            lvwSearchDetails.Columns.Item(g_kLvwColumnActiveStatus - 1).Width = CInt(0)
            lvwSearchDetails.Columns.Item(g_kLvwColumnBranch - 1).Width = CInt(0)

            If Not m_bPartyOther Then

                If m_iAgentOnly = 4 Then
                    'Only find agents (or whatever)
                    cmbType.SelectedIndex = 4
                    cmbType.Enabled = False
                Else
                    cmbType.Enabled = False
                    Select Case m_sSpecialParty

                        Case PMBConst.PMBPartyTypeAgentGroup
                            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindAgentGroupTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            lblShortName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListAgentGroupTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            lblType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListBranchTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                            'Sort Out Column Headers
                            'Use these column headers
                            lvwSearchDetails.Columns.Item(g_kLvwColumnClientCode - 1).Width = CInt(VB6.TwipsToPixelsX(2000))
                            lvwSearchDetails.Columns.Item(g_kLvwColumnClientCode - 1).Text = lblLongName.Text
                            lvwSearchDetails.Columns.Item(g_kLvwColumnName - 1).Width = CInt(VB6.TwipsToPixelsX(2000))
                            lvwSearchDetails.Columns.Item(g_kLvwColumnName - 1).Text = lblLongName.Text
                            lvwSearchDetails.Columns.Item(g_kLvwColumnActiveStatus - 1).Width = CInt(VB6.TwipsToPixelsX(2000))
                            lvwSearchDetails.Columns.Item(g_kLvwColumnActiveStatus - 1).Text = lblType.Text
                            lvwSearchDetails.Columns.Item(g_kLvwColumnBranch - 1).Width = CInt(VB6.TwipsToPixelsX(2000))
                            lvwSearchDetails.Columns.Item(g_kLvwColumnBranch - 1).Text = lblStatus.Text

                            'Hide the rest
                            lvwSearchDetails.Columns.Item(g_kLvwColumnAddressLine1 - 1).Width = CInt(0)
                            lvwSearchDetails.Columns.Item(g_kLvwColumnAddressLine2 - 1).Width = CInt(0)
                            lvwSearchDetails.Columns.Item(g_kLvwColumnAddressLine3 - 1).Width = CInt(0)
                            lvwSearchDetails.Columns.Item(g_kLvwColumnAddressLine4 - 1).Width = CInt(0)
                            lvwSearchDetails.Columns.Item(g_kLvwColumnPostCode - 1).Width = CInt(0)
                            lvwSearchDetails.Columns.Item(g_kLvwColumnFileCode - 1).Width = CInt(0)
                            lvwSearchDetails.Columns.Item(g_kLvwColumnType - 1).Width = CInt(0)
                            lvwSearchDetails.Columns.Item(g_kLvwColumnStatus - 1).Width = CInt(0)
                            lvwSearchDetails.Columns.Item(g_kLvwColumnSource - 1).Width = CInt(0)
                            lvwSearchDetails.Columns.Item(g_kLvwColumnDateOfBirth - 1).Width = CInt(0)
                            lvwSearchDetails.Columns.Item(g_kLvwColumnSwiftLink - 1).Width = CInt(0)
                            lvwSearchDetails.Columns.Item(g_kLvwColumnClientCode - 1).Text = lblShortName.Text

                            'Hide Claim tab
                            SSTabHelper.SetTabVisible(tabMainTab, 3, False)

                            m_lReturn = g_oBusiness.IsUserSystemAdministrator(r_bIsSystemAdministrator:=bIsSystemAdministrator)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            cmdUsers.Visible = bIsSystemAdministrator And NotEditable <> 1 And m_bAONAffinity

                        Case PMBConst.PMBPartyTypeAgent

                            lblAgentGroup.Visible = True
                            cmbAgentGroup.Visible = True

                            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindAgentTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            lblShortName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListAgentTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            lvwSearchDetails.Columns.Item(g_kLvwColumnClientCode - 1).Text = lblShortName.Text
                            cmbType.SelectedIndex = 0
                            cmbAgentType.SelectedIndex = 0
                            cmbAgentType.Visible = True
                            lblAgentType.Visible = True

                            m_lReturn = g_oBusiness.IsUserSystemAdministrator(r_bIsSystemAdministrator:=bIsSystemAdministrator)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            cmdUsers.Visible = bIsSystemAdministrator And NotEditable <> 1 And m_bAONAffinity

                        Case PMBConst.PMBPartyTypeSubAgent
                            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindAgentTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            lblShortName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListAgentTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            lvwSearchDetails.Columns.Item(g_kLvwColumnClientCode - 1).Text = lblShortName.Text
                            cmbType.SelectedIndex = 0
                            cmbAgentType.Visible = True
                            lblAgentType.Visible = True
                            cmbAgentType.SelectedIndex = 2
                            cmbAgentType.Enabled = False

                            m_lReturn = g_oBusiness.IsUserSystemAdministrator(r_bIsSystemAdministrator:=bIsSystemAdministrator)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            cmdUsers.Visible = bIsSystemAdministrator And NotEditable <> 1 And m_bAONAffinity

                        Case PMBConst.PMBPartyTypeConsultant

                            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindConsultantTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            lblShortName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListConsultantTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            lvwSearchDetails.Columns.Item(g_kLvwColumnClientCode - 1).Text = lblShortName.Text
                            m_lReturn = HideColumns(v_iShowLeft:=2)
                            cmbType.SelectedIndex = 0
                            txtFileCode.Visible = False
                            lblFileCode.Visible = False
                            cmbType.Top = txtFileCode.Top
                            lblType.Top = lblFileCode.Top

                        Case PMBConst.PMBPartyTypeAccountHandler

                            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindAccountHandlerTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            lblShortName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListAccountHandlerTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            lvwSearchDetails.Columns.Item(g_kLvwColumnClientCode - 1).Text = lblShortName.Text
                            m_lReturn = HideColumns(v_iShowLeft:=2)
                            cmbType.SelectedIndex = 0
                            txtFileCode.Visible = False
                            lblFileCode.Visible = False
                            cmbType.Top = txtFileCode.Top
                            lblType.Top = lblFileCode.Top

                        Case PMBConst.PMBPartyTypeInsurer
                            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindReinsurerTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            lblShortName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListReinsurerTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            lvwSearchDetails.Columns.Item(g_kLvwColumnClientCode - 1).Text = lblShortName.Text
                            cmbType.SelectedIndex = 0
                            m_lReturn = g_oBusiness.IsUserSystemAdministrator(r_bIsSystemAdministrator:=bIsSystemAdministrator)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            'Check to see if restricted insurer access is turned on
                            m_lReturn = PartyFunc.GetHiddenOptions(v_lSourceId:=g_iSourceID, r_vRestrictedInsurerAccess:=bRestrictedInsurerAccess)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetHiddenOptions Failed for source_id = " & g_iSourceID, vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            cmdUsers.Visible = bIsSystemAdministrator And bRestrictedInsurerAccess And m_bAONAffinity

                        Case PMBConst.PMBPartyTypeBroker
                            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindBrokerTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            lblShortName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListBrokerTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            lvwSearchDetails.Columns.Item(g_kLvwColumnClientCode - 1).Text = lblShortName.Text
                            cmbType.SelectedIndex = 0

                        Case PMBConst.PMBPartyTypeFee

                            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindFeeTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            lblShortName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListFeeTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            lvwSearchDetails.Columns.Item(g_kLvwColumnClientCode - 1).Text = lblShortName.Text
                            cmbType.SelectedIndex = 0
                            m_lReturn = HideColumns(v_iShowLeft:=2)

                            txtFileCode.Visible = False
                            lblFileCode.Visible = False
                            cmbType.Top = txtFileCode.Top
                            lblType.Top = lblFileCode.Top

                        Case PMBConst.PMBPartyTypeExtra
                            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindExtraTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            lblShortName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListExtraTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                            lvwSearchDetails.Columns.Item(g_kLvwColumnClientCode - 1).Text = lblShortName.Text

                            cmbType.SelectedIndex = 0

                            m_lReturn = HideColumns(v_iShowLeft:=2)
                            txtFileCode.Visible = False
                            lblFileCode.Visible = False
                            cmbType.Top = txtFileCode.Top
                            lblType.Top = lblFileCode.Top

                        Case PMBConst.PMBPartyTypeDiscount

                            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindDiscountTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            lblShortName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListDiscountTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                            lvwSearchDetails.Columns.Item(g_kLvwColumnClientCode - 1).Text = lblShortName.Text

                            cmbType.SelectedIndex = 0

                            m_lReturn = HideColumns(v_iShowLeft:=2)

                            txtFileCode.Visible = False
                            lblFileCode.Visible = False
                            cmbType.Top = txtFileCode.Top
                            lblType.Top = lblFileCode.Top

                        Case PMBConst.PMBPartyTypeCommissionAccount
                            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindCommissionAccountTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            lblShortName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListCommissionAccountTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                            lvwSearchDetails.Columns.Item(g_kLvwColumnClientCode - 1).Text = lblShortName.Text

                            m_lReturn = HideColumns(v_iShowLeft:=8)

                            cmbType.SelectedIndex = 0
                            cmbAgentType.Visible = True
                            lblAgentType.Visible = True

                            cmbAgentType.SelectedIndex = 3
                            cmbAgentType.Enabled = False

                        Case PMBConst.PMBPartyTypeFinanceProvider

                            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindFinanceProviderTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            lblShortName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListFinanceProviderTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                            lvwSearchDetails.Columns.Item(g_kLvwColumnClientCode - 1).Text = lblShortName.Text

                            m_lReturn = HideColumns(v_iShowLeft:=2)

                            cmbType.SelectedIndex = 0

                        Case PMBConst.PMBPartyTypeIntermediary
                            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindIntermediaryTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                            lblShortName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListIntermediaryTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                            lvwSearchDetails.Columns.Item(g_kLvwColumnClientCode - 1).Text = lblShortName.Text

                            cmbType.SelectedIndex = 0
                            cmbAgentType.Visible = True
                            cmbAgentType.SelectedIndex = 4
                            cmbAgentType.Enabled = False

                            m_lReturn = g_oBusiness.IsUserSystemAdministrator(r_bIsSystemAdministrator:=bIsSystemAdministrator)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            cmdUsers.Visible = bIsSystemAdministrator And NotEditable <> 1 And m_bAONAffinity

                        Case Else

                            cmbType.SelectedIndex = m_iAgentOnly
                            cmbType.Enabled = True

                    End Select

                End If

                'If we're looking for a special party, don't allow the last 2 tabs...
                If Not cmbType.Enabled Then

                    SSTabHelper.SetTabVisible(tabMainTab, 3, False)
                    SSTabHelper.SetTabVisible(tabMainTab, 2, False)
                    SSTabHelper.SetTabVisible(tabMainTab, 1, False)
                    'And don't show the menu
                    mnuRecentFiles.Available = False
                    ResizeIfNoMenu()
                    tabMainTab.Top = 5
                End If

                'CMG/PB 18072002 Add Find party agent group
                If m_sSpecialParty = PMBConst.PMBPartyTypeAgentGroup Then

                    cmbActiveStatus.Items.Clear()
                    Dim cmbActiveStatus_NewIndex As Integer = -1
                    cmbActiveStatus_NewIndex = 0
                    cmbActiveStatus.Items.Insert(cmbActiveStatus_NewIndex, "<ALL>")
                    VB6.SetItemData(cmbActiveStatus, cmbActiveStatus_NewIndex, 2)
                    cmbActiveStatus_NewIndex = 1
                    cmbActiveStatus.Items.Insert(cmbActiveStatus_NewIndex, "Active")
                    VB6.SetItemData(cmbActiveStatus, cmbActiveStatus_NewIndex, 1)
                    cmbActiveStatus_NewIndex = 2
                    cmbActiveStatus.Items.Insert(cmbActiveStatus_NewIndex, "InActive")
                    VB6.SetItemData(cmbActiveStatus, cmbActiveStatus_NewIndex, 0)

                    'Set defaults to 'all'
                    cmbActiveStatus.SelectedIndex = 0
                    cmbBranch.SelectedIndex = 0
                Else
                    cmbActiveStatus.Items.Clear()
                    'Deepak commented and replaced 
                    'cmbActiveStatus_NewIndex = 0
                    'cmbActiveStatus.Items.Insert(cmbActiveStatus_NewIndex, "<ALL>")
                    cmbActiveStatus.Items.Insert(0, "<ALL>")
                    cmbActiveStatus.SelectedIndex = 0
                    'Populate status combo box with Prospect Types (does not use lookup table)
                    cmbStatus.Items.Clear()
                    cmbStatus.Items.Insert(0, "<ALL>")
                    cmbStatus.Items.Insert(1, PMBConst.PMBProspectTypeClientText)
                    cmbStatus.Items.Insert(2, PMBConst.PMBProspectTypeProspectText)

                    'DC081101 set default to client and not 'all'
                    cmbStatus.SelectedIndex = 1
                End If

                cmbStatus.Enabled = (m_sSpecialParty = "")

                If (m_sStructure = gSIRLibrary.SIRPMBSolution) And (m_sSpecialParty = "") Then
                    cmbStatus.Visible = True
                    lblStatus.Visible = True
                    ' Show DOB
                    lblDOB.Visible = True
                    txtDOB.Visible = True

                ElseIf m_sSpecialParty = PMBConst.PMBPartyTypeAgentGroup Then
                    'Set up fields for Find Agent Group
                    'cmbType is used to hold the branch
                    cmbStatus.Visible = False
                    'Acts as active status label
                    lblStatus.Visible = True
                    cmbStatus.Enabled = False

                    cmbActiveStatus.Visible = True
                    cmbActiveStatus.Enabled = True

                    cmbAgentType.Visible = False
                    lblAgentType.Visible = False
                    cmbAgentType.Enabled = False

                    cmbBranch.Visible = True
                    cmbBranch.Enabled = True

                    cmbType.Visible = False
                    'Acts as branch label
                    lblType.Visible = True
                Else
                    cmbStatus.Visible = False
                    lblStatus.Visible = False
                    ' Hide DOB
                    lblDOB.Visible = False
                    txtDOB.Visible = False
                End If
            Else
                'If this is party type 'Other' then remove policy tab.
                SSTabHelper.SetTabVisible(tabMainTab, 2, False)
                lvwSearchDetails.Columns.Item(g_kLvwColumnClientCode - 1).Text = lblShortName.Text.Replace(":", "")

            End If

            Dim sValue As String
            m_lReturn = iPMFunc.GetSystemOption(5099, sValue)

            If sValue = "1" Then
                lblCaseNumber.Visible = True
                txtCaseNumber.Visible = True
            End If

            ' Update the interface details with the
            ' property members.
            m_lReturn = PropertiesToInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'sj 2/11/99 - start
            If m_sSpecialParty = "" Then
                If Not g_bGenericConnectionStatus Then
                    chkPM.CheckState = CheckState.Unchecked
                    chkPM.Enabled = False
                Else
                    chkPM.CheckState = CheckState.Checked
                    chkPM.Enabled = True
                End If
            Else
                chkPM.CheckState = CheckState.Unchecked
                chkPM.Enabled = False
            End If

            chkSirius.CheckState = CheckState.Checked

            If Not g_bGenericConnectionStatus Then
                chkPM.Visible = False
                chkSirius.Visible = False
            End If

            ' Set any other default values to the interface.
            lvwSearchDetails.FullRowSelect = True

            If m_vNavStep = "BASIC" Then
                cmdEdit.Enabled = False
                SSTabHelper.SetTabVisible(tabMainTab, 2, False)
                cmbStatus.Enabled = False
                cmbType.SelectedIndex = 1
                cmbType.Enabled = False
            End If

            If m_bRestrictInsurerAccess Then
                cmdNew.Visible = False
            End If

            If m_bEnableNewParty Then
                cmdNew.Enabled = True
                cmdNew.Visible = True
            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult
        End Try
    End Function
    ''' <summary>
    ''' ClearInterface
    ''' </summary>
    ''' <param name="bConfirm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ClearInterface(ByRef bConfirm As Boolean) As Integer

        Dim nResult As Integer
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the interface details.

            If bConfirm Then

                ' Check if the user still wishes to clear
                ' the interface.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display the message.
                iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                ' Check message result.
                If iMsgResult = System.Windows.Forms.DialogResult.No Then
                    ' Don't continue with the clear.
                    Return nResult
                End If
            End If

            ' Clear the search data array.
            m_vSearchData = Nothing

            ' Clear the search list details.
            lvwSearchDetails.Items.Clear()

            ' Clear the search status bar.

            _stbStatus_Panel1.Text = ""
            chkIncludeClosedBranches.CheckState = CheckState.Unchecked
            txtShortName.Text = ""
            txtLongName.Text = ""
            txtAddress1.Text = ""
            txtPostalCode.Text = ""
            txtTelephoneCode.Text = ""
            txtTelephone.Text = ""
            txtInsReference.Text = ""
            txtFileCode.Text = ""
            txtDOB.Text = ""
            m_lSwiftPartyID = 0 ' Reset this!!!
            txtClaimNumber.Text = ""
            txtRiskIndex.Text = ""
            txtCaseNumber.Text = ""

            If Not m_bPartyOther Then
                If m_iAgentOnly <> 0 Then
                    'Only find agents (or whatever)
                    cmbType.SelectedIndex = m_iAgentOnly
                Else
                    If cmbType.Items.Count < 2 Then
                        cmbType.SelectedIndex = 0
                    Else
                        Select Case m_sSpecialParty
                            Case PMBConst.PMBPartyTypeAgentGroup
                                cmbType.SelectedIndex = 0
                            Case PMBConst.PMBPartyTypeAgent, PMBConst.PMBPartyTypeSubAgent
                                cmbType.SelectedIndex = 4
                            Case PMBConst.PMBPartyTypeConsultant
                                cmbType.SelectedIndex = 5
                            Case PMBConst.PMBPartyTypeAccountHandler
                                cmbType.SelectedIndex = 6
                            Case PMBConst.PMBPartyTypeInsurer
                                cmbType.SelectedIndex = 7
                            Case PMBConst.PMBPartyTypeBroker
                                cmbType.SelectedIndex = 8
                            Case PMBConst.PMBPartyTypeFee
                                cmbType.SelectedIndex = 9
                            Case PMBConst.PMBPartyTypeExtra
                                cmbType.SelectedIndex = 10
                            Case PMBConst.PMBPartyTypeDiscount 'DN 06/01/03 ISS 1083
                                cmbType.SelectedIndex = 11
                            Case PMBConst.PMBPartyTypeCommissionAccount
                                cmbType.SelectedIndex = 12
                            Case PMBConst.PMBPartyTypeFinanceProvider 'Alix Bergeret - 28/01/2003 - Issue 1897
                                cmbType.SelectedIndex = 13
                            Case Else
                                cmbType.SelectedIndex = 0
                        End Select
                    End If
                End If
            Else
                ' ...just reset cmbOtherPartyType (if it's enabled)
                If cmbOtherPartyType.Enabled Then
                    cmbOtherPartyType.SelectedIndex = 0
                End If
            End If
            'JMK 22/08/2001 end
            If cmbStatus.Visible Then
                cmbStatus.SelectedIndex = 0
            End If
            If cmbAgentType.Visible Then
                cmbAgentType.SelectedIndex = 0
            End If
            'CMG/PB 23072002 Clear the new agent group controls
            If cmbActiveStatus.Visible Then
                cmbActiveStatus.SelectedIndex = 0
            End If

            If cmbBranch.Visible AndAlso cmbActiveStatus.Enabled Then
                cmbBranch.SelectedIndex = 0
            End If

            If cmbAgentGroup.Visible AndAlso cmbAgentGroup.Items.Count > 0 Then
                cmbAgentGroup.SelectedIndex = 0
            End If

            ' Set to the first tab.
            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

            ' Set focus to the search details.
            txtShortName.Focus()

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

            ' Disable parts of the interface, so the
            ' user can now only enter a new search
            m_lReturn = DisableInterface(bDisable:=True)

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetFirstLastControls
    '
    ' Description: Sets the first and last data entry controls for
    '              each tab to the control array, for use with the
    '              keyboard navigation.
    '
    ' ***************************************************************** '
    Private Function SetFirstLastControls() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            ReDim m_ctlTabFirstLast(1, 2)

            ' Set the first and last data entry controls for
            ' all of the tabs.

            m_ctlTabFirstLast(ACControlStart, 0) = txtShortName
            If cmbStatus.Visible Then
                m_ctlTabFirstLast(ACControlEnd, 0) = cmbStatus
            Else
                m_ctlTabFirstLast(ACControlEnd, 0) = cmbType
            End If

            m_ctlTabFirstLast(ACControlStart, 1) = txtAddress1
            m_ctlTabFirstLast(ACControlEnd, 1) = txtTelephone

            m_ctlTabFirstLast(ACControlStart, 2) = txtInsReference
            m_ctlTabFirstLast(ACControlEnd, 2) = txtInsReference


            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Dim iLanguageId As Integer
        'In all instances where the GetResData function is called the g_iLanguageID% is replaced with iLanguageId%

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            Dim sCaption As String = ""

            m_lReturn = gPMFunctions.GetUserIsAmericanLanguageID(iLanguageId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Display all language specific captions.

            If m_bPartyOther Then

                Me.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACFindOtherPartyTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                lblShortName.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACOtherPartyCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                lblStatus.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACOtherPartyType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Else

                Me.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                lblStatus.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                lblShortName.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACShortName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            End If

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &
                                  "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If

            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            If m_bDeleteMode Then
                cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Else
                cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdFindNow.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACFindNowButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdNewSearch.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACNewSearchButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            SSTabHelper.SetTabCaption(tabMainTab, 2, iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACTabTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdNew.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACNewButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchDetails.Columns.Item(g_kLvwColumnClientCode - 1).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchDetails.Columns.Item(g_kLvwColumnName - 1).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchDetails.Columns.Item(g_kLvwColumnSurname - 1).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitleSurname, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchDetails.Columns.Item(g_kLvwColumnForename - 1).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitleForename, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            If m_bIsNRMA Then
                lvwSearchDetails.Columns.Item(g_kLvwColumnAddressLine1 - 1).Text = "Property Name"
            Else
                lvwSearchDetails.Columns.Item(g_kLvwColumnAddressLine1 - 1).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            If m_bIsNRMA Then
                lvwSearchDetails.Columns.Item(g_kLvwColumnAddressLine2 - 1).Text = "Street/PO Box"
            Else
                lvwSearchDetails.Columns.Item(g_kLvwColumnAddressLine2 - 1).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitleAddressLine2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            If m_bIsNRMA Then
                lvwSearchDetails.Columns.Item(g_kLvwColumnAddressLine3 - 1).Text = "City"
            Else
                lvwSearchDetails.Columns.Item(g_kLvwColumnAddressLine3 - 1).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitleAddressLine3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            If m_bIsNRMA Then
                lvwSearchDetails.Columns.Item(g_kLvwColumnAddressLine4 - 1).Text = "State/Country"
            Else
                lvwSearchDetails.Columns.Item(g_kLvwColumnAddressLine4 - 1).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitleAddressLine4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If
            lvwSearchDetails.Columns.Item(g_kLvwColumnPostCode - 1).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            If m_bAONAffinity Then
                sCaption = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitleAON5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Else
                sCaption = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If
            lvwSearchDetails.Columns.Item(g_kLvwColumnFileCode - 1).Text = sCaption
            lblFileCode.Text = sCaption & ":"

            lvwSearchDetails.Columns.Item(g_kLvwColumnType - 1).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitle6, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchDetails.Columns.Item(g_kLvwColumnStatus - 1).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitle7, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchDetails.Columns.Item(g_kLvwColumnSource - 1).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitle8, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchDetails.Columns.Item(g_kLvwColumnDateOfBirth - 1).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitleDateOfBirth, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchDetails.Columns.Item(g_kLvwColumnSwiftLink - 1).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitleSwiftLink, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblLongName.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACLongName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblType.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            If m_bIsNRMA Then
                lblAddress1.Text = "Address:"
            Else
                lblAddress1.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACAddress1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            lblPostalCode.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACPostalCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblTelephone.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACTelephone, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdPolicyRefFind.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACInsReference, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            chkIncludeClosedBranches.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACIncludeClosedBranches, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
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

            cmdOK.Enabled = Not bDisable
            'If we're here we're searching.  Disable it until an item is clicked.
            cmdEdit.Enabled = False

            cmdUsers.Enabled = False

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

        Try

            ' Get message text if not already present.
            If sMessage = "" Then
                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.

            _stbStatus_Panel1.Text = " " & sMessage

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

        Try

            ' Store the total of item found.
            lItemsFound = lvwSearchDetails.Items.Count
            '
            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            _stbStatus_Panel1.Text = " " & lItemsFound & " " & sMessage

        Catch excep As System.Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            ' At least one field must be populated

            If txtShortName.Text.Trim() <> "" Then
                If txtShortName.Text.Trim().Length >= ACMinSearchLength Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            If txtLongName.Text.Trim() <> "" Then
                If txtLongName.Text.Trim().Length >= ACMinSearchLength Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            If txtFileCode.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If cmbType.Text.ToUpper() <> "<ALL>" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If cmbStatus.Text.ToUpper() <> "<ALL>" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtAddress1.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtPostalCode.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtTelephoneCode.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtTelephone.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtInsReference.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtDOB.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtRiskIndex.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtClaimNumber.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtCaseNumber.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check for mandatory fields", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        'Setting done for Menu visiblity.
        Dim rFactor As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Added by Sumeet (Works fine with RecentFiles as well as without it.)
            If mnuRecentFiles.Available Then
                rFactor = 1110
            Else
                rFactor = 1395
            End If
            cmdFindNow.Left = Me.Width - VB6.TwipsToPixelsX(1335)
            cmdNewSearch.Left = Me.Width - VB6.TwipsToPixelsX(1335)

            tabMainTab.Width = Me.Width - VB6.TwipsToPixelsX(1560)
            lvwSearchDetails.Width = Me.Width - VB6.TwipsToPixelsX(360)
            lvwSearchDetails.Height = Me.Height - VB6.TwipsToPixelsY(4800)
            cmdHelp.Left = Me.Width - VB6.TwipsToPixelsX(1335)
            cmdHelp.Top = Me.Height - VB6.TwipsToPixelsY(rFactor)
            cmdCancel.Left = Me.Width - VB6.TwipsToPixelsX(2535)
            cmdCancel.Top = Me.Height - VB6.TwipsToPixelsY(rFactor)
            cmdOK.Left = Me.Width - VB6.TwipsToPixelsX(3735)
            cmdOK.Top = Me.Height - VB6.TwipsToPixelsY(rFactor)
            cmdNew.Top = Me.Height - VB6.TwipsToPixelsY(rFactor)
            cmdEdit.Top = Me.Height - VB6.TwipsToPixelsY(rFactor)
            cmdUsers.Top = Me.Height - VB6.TwipsToPixelsY(rFactor)

            If cmdNavigate.Visible Then
                cmdNavigate.Top = Me.Height - VB6.TwipsToPixelsY(rFactor)
            End If
            'Added to resize the panel according to the status panel size.
            _stbStatus_Panel1.Width = stbStatus.Width - 26
            Return result

        Catch

            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: GetCompany (Standard Method)
    '
    ' Description: Gets valid Source ID's  and if nessessary displays selection
    '
    ' ***************************************************************** '
    Public Function GetCompany(ByRef m_iCompanyID As Integer) As Integer
        Dim result As Integer = 0
        Dim m_oBranch As iPMBBranch.Interface_Renamed
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_m_oBranch As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBranch, sClassName:="iPMBBranch.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oBranch = temp_m_oBranch

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oBranch.GetSource(iSourceID:=m_iCompanyID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oBranch.Dispose()
            m_oBranch = Nothing
            ' Check if we have had an error so far.

            Return result

        Catch excep As System.Exception
            ' Error Section
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Company", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCompany", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessPartyInterface(Private)
    '
    ' Description: Calls the appropriate Party Interface
    '
    ' ***************************************************************** '
    'eck120500
    Private Function ProcessPartyInterface(Optional ByVal v_sPartyType As String = "", Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_iTask As Integer = 0, Optional ByVal v_iSourceID As Integer = 0, Optional ByVal v_lIndex As Integer = 0) As Integer
        Dim result As Integer = 0

        Dim oParty As Object
        Dim sTmp As String = ""
        Dim vFileName As String = ""
        Dim bProceed As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Party
            If v_iTask <> gPMConstants.PMEComponentAction.PMAdd Then
                m_lReturn = FSACustomerValidate(v_lPartyCnt, v_sPartyType, m_iIsComplaint, bProceed)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed FSA Customer Validate.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPartyInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Throw New Exception()
                    Return result
                End If

                'User is not proceeding
                If Not bProceed Then
                    Return result
                End If
            End If

            If m_vNavStep <> "BASIC" Then

                ' Create the appropriate party interface
                Select Case v_sPartyType
                    Case PMBConst.PMBPartyTypePersonalClient, PMBConst.PMBPartyTypeAgent, PMBConst.PMBPartyTypeCorporateClient, PMBConst.PMBPartyTypeGroupClient, PMBConst.PMBPartyTypeAccountHandler, PMBConst.PMBPartyTypeInsurer, PMBConst.PMBPartyTypeExtra, PMBConst.PMBPartyTypeFinanceProvider, PMBConst.PMBPartyTypeAgentGroup
                        sTmp = v_sPartyType

                    Case PMBConst.PMBPartyTypeConsultant
                        sTmp = PMBConst.PMBPartyTypeAccountHandler

                    Case PMBConst.PMBPartyTypeExecutiveHandler
                        sTmp = PMBConst.PMBPartyTypeAccountHandler

                    Case PMBConst.PMBPartyTypeFee
                        sTmp = PMBConst.PMBPartyTypeExtra

                    Case PMBConst.PMBPartyTypeDiscount
                        sTmp = PMBConst.PMBPartyTypeExtra
                        '
                    Case Else
                        If v_sPartyType.Substring(0, 2) = PMBConst.PMBPartyTypeOther Then
                            sTmp = PMBConst.PMBPartyTypeOther
                        Else
                            result = gPMConstants.PMEReturnCode.PMFalse
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unknown Party Type - " & v_sPartyType, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPartyInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                            Throw New Exception()
                            Return result
                        End If
                End Select

                m_lReturn = g_oObjectManager.GetInstance(oObject:=oParty, sClassName:="iPMBParty" & sTmp & ".Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)

            Else

                Dim temp_oParty As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_oParty, sClassName:="iPMBPartyPCBasic.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                oParty = temp_oParty

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                If Not (oParty Is Nothing) Then

                    oParty.Dispose()
                    oParty = Nothing
                End If
                Throw New Exception()
            End If

            ' set the party cnt and process mode if editing

            If (v_iTask = gPMConstants.PMEComponentAction.PMEdit) Or (v_iTask = gPMConstants.PMEComponentAction.PMView) Then

                oParty.PartyCnt = v_lPartyCnt

                m_lReturn = oParty.SetProcessModes(vTask:=v_iTask)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    oParty.Dispose()
                    oParty = Nothing
                    Throw New Exception()
                End If
            End If

            'JDW for CNIC add names info
            'CJB 03/09/2002 CNIC change to ensure that if there is QAS failure that manually
            'entered details will still get transferred...Comment out check if address selected
            ' SET 03092002 - QAS functionality is currently only present for personal
            '                parties so the following code will be executed for them only


            If v_sPartyType = gSIRLibrary.SIRPartyTypePersonalClient Then

                oParty.QASSurname = m_sSurname

                oParty.QASForename = m_sForename

                oParty.QASInitial = m_sInitial

                oParty.QASTitle = m_sTitle

                oParty.QASNadd1 = m_oQASN.Add1

                oParty.QASNadd2 = m_oQASN.Add2

                oParty.QASNadd3 = m_oQASN.Add3

                oParty.QASNadd4 = m_oQASN.Add4

                oParty.QASNpostcode = m_oQASN.Postcode
            End If

            ' set the process mode if adding a new party
            If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                m_lReturn = oParty.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    oParty.Dispose()
                    oParty = Nothing
                    Throw New Exception()
                End If

                If m_vNavStep <> "BASIC" Then

                    oParty.SourceID = v_iSourceID
                    If v_sPartyType = PMBConst.PMBPartyTypePersonalClient Or v_sPartyType = PMBConst.PMBPartyTypeCorporateClient Or v_sPartyType = PMBConst.PMBPartyTypeGroupClient Then
                        oParty.SwiftPartyID = m_lSwiftPartyID
                    End If
                Else
                    v_sPartyType = "PC"
                End If
            End If

            If m_vNavStep <> "BASIC" Then

                If sTmp = PMBConst.PMBPartyTypeAccountHandler Then

                    oParty.HandlerType = v_sPartyType
                    m_sSelectedPartyType = v_sPartyType
                End If

                If sTmp = PMBConst.PMBPartyTypeExtra Or sTmp = PMBConst.PMBPartyTypeFee Or sTmp = PMBConst.PMBPartyTypeDiscount Or sTmp = PMBConst.PMBPartyTypeOther Then

                    oParty.PartyType = v_sPartyType
                End If

            End If

            ' Party View
            If (sTmp = PMBConst.PMBPartyTypeAgent) And (v_iTask = gPMConstants.PMEComponentAction.PMView) Then
                oParty.IsViewOnly = True
            End If

            ' start the object
            m_lReturn = oParty.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oParty.Dispose()
                oParty = Nothing
                Throw New Exception()
            End If

            If v_sPartyType = PMBConst.PMBPartyTypePersonalClient Or v_sPartyType = PMBConst.PMBPartyTypeCorporateClient Or v_sPartyType = PMBConst.PMBPartyTypeGroupClient Then ' MSS200901 - Added check. UW doesn't check this criteria

                If oParty.Status <> gPMConstants.PMEReturnCode.PMCancel Then

                    Select Case v_iTask
                        'Party View - PMView allowed
                        Case gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMView
                            'ECK 19/05/99 If Recent files chosen
                            If v_lIndex = 0 Then
                                'update the details in the listview - they may have changed
                                lvwSearchDetails.Items.Item(v_lIndex - 1).Text = oParty.ShortName

                                ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(v_lIndex - 1), 1).Text = oParty.Name

                                ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(v_lIndex - 1), 2).Text = oParty.AddressLine1

                                ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(v_lIndex - 1), 3).Text = oParty.mainpostcode
                            End If

                        Case gPMConstants.PMEComponentAction.PMAdd

                            ' Clear the interface details.
                            m_lReturn = ClearInterface(bConfirm:=False)

                            ' Check for errors.
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                ' Failed to clear the interface details.
                            End If

                            'Set the shortname field and do another search to populate with the new party

                            txtShortName.Text = oParty.ShortName

                            cmdFindNow_Click(cmdFindNow, New EventArgs())

                        Case Else

                            result = gPMConstants.PMEReturnCode.PMFalse
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unknown Party Type - " & v_sPartyType, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPartyInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                            oParty.Dispose()
                            oParty = Nothing
                            Return result

                    End Select
                End If
                If v_sPartyType = PMBConst.PMBPartyTypePersonalClient Or v_sPartyType = PMBConst.PMBPartyTypeCorporateClient Or v_sPartyType = PMBConst.PMBPartyTypeGroupClient Then
                    If v_iTask = gPMConstants.PMEComponentAction.PMEdit Then

                        vFileName = oParty.ShortName & "|" & oParty.Name & "|" & oParty.PartyCnt & v_sPartyType.Substring(0, 1)
                        AddRecentFile(vFileName)
                    Else

                        If oParty.Status <> gPMConstants.PMEReturnCode.PMCancel Then
                            vFileName = oParty.ShortName & "|" & oParty.Name & "|" & oParty.PartyCnt & v_sPartyType.Substring(0, 1)
                            AddRecentFile(vFileName)
                        End If
                    End If
                End If
            End If

            If sTmp = PMBConst.PMBPartyTypeAccountHandler Then
                m_sLongName = oParty.ResolvedName
            End If

            ' Destroy the object
            oParty.Dispose()
            oParty = Nothing

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process the Party Interface.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPartyInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            ' Clear the interface details.
            m_lReturn = ClearInterface(bConfirm:=False)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to clear the interface details.
            End If
            Return result

        End Try
    End Function
    Private Sub chkDPARequired_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkDPARequired.CheckStateChanged

        If DPARequired Then
            chkDPARequired.TabStop = False
            chkDPARequired.CheckState = CheckState.Checked
        Else
            chkDPARequired.TabStop = True
        End If

        If chkDPARequired.CheckState = CheckState.Checked Then
            lblDPARequired.Text = "Yes"
        Else
            lblDPARequired.Text = "No"
        End If
    End Sub

    ' PRIVATE Methods (End)

    Private Sub chkPM_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkPM.CheckStateChanged

        If chkPM.CheckState = CheckState.Unchecked Then
            chkSirius.CheckState = CheckState.Checked
        End If

    End Sub

    Private Sub chkSirius_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkSirius.CheckStateChanged

        If chkSirius.CheckState = CheckState.Unchecked Then
            chkPM.CheckState = CheckState.Checked
        End If

    End Sub

    Private Sub cmbStatus_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbStatus.SelectedIndexChanged
        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)
    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        If m_sAgencyOrunderwriting = "" Then
            m_sAgencyOrunderwriting = m_oBusiness.UnderwritingOrAuthority
        End If

        PMHelpFunc.g_sProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        m_lReturn = PMHelpFunc.ShowHelp(objCnt:=cmdHelp, lContextID:=UWScreenHelpID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        End If
    End Sub
    ' ***************************************************************** '
    '
    ' Name: cmdUsers_Click
    '
    ' Description:
    '
    ' History: 06/06/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Sub cmdUsers_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdUsers.Click

        Try

            Dim lPartyCnt, lRowId, lIndex As Integer
            Dim sAgentName As String = ""
            'adeveloper guide no. 69
            Dim frmUsers As New frmUsers
            ' Get id of the row that has been selected for an edit

            lRowId = Convert.ToString(lvwSearchDetails.FocusedItem.Tag)

            'Get the index
            lIndex = lvwSearchDetails.FocusedItem.Index + 1

            ' Get code and cnt for the selected row id

            lPartyCnt = CInt(m_vSearchData(ACIPartyCnt, lRowId))

            sAgentName = CStr(m_vSearchData(ACILongName, lRowId))

            With frmUsers
                m_lReturn = .InitialForm()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="frmUsers.InitialForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdUsers_Click")
                    Exit Sub
                End If

                .PartyCnt = lPartyCnt
                .AgentName = sAgentName

                m_lReturn = .PopListBox()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="frmUsers.PopListBox Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdUsers_Click")
                    Exit Sub
                End If

                m_lReturn = .ShowForm()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="frmUsers.ShowForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdUsers_Click")
                    Exit Sub
                End If
            End With

            frmUsers.Close()

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdUsers_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdUsers_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

        End Try

    End Sub


    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()
        Dim sValue As String = ""
        'Party View

        Dim oUserAuthorities As bACTUserAuthorities.Business

        ' Forms initialise event.
        Try

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMBFindParty.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            'Get bPMLock
            Dim temp_m_oPMLock As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oPMLock = temp_m_oPMLock

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                'Initialise = PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' get system options client blacklisting in force
            ' Get System Option for Client BlackListing
            m_lReturn = iPMFunc.GetSystemOption(kSystemOptionClientBlacklistingInForce, sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemOptionValue Failed for Client BlackListing In Force", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            m_bSystemOptionClientBlacklistingInForce = (sValue = "1")
            ' Get System Option "Enhance Filter screens for large datasets"
            m_lReturn = iPMFunc.GetSystemOption(kSystemOptionEnhanceFilterscreens, sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemOptionValue Failed for 'Enhance Filter screens for large datasets'", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            m_bSystemOptionEnhanceFilterscreens = (sValue = "1")

            'Party View
            Dim temp_oUserAuthorities As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oUserAuthorities, "bACTUserAuthorities.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oUserAuthorities = temp_oUserAuthorities

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bACTUserAuthorities.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialize", excep:=New Exception(Information.Err().Description))
                Exit Sub
            End If
            m_lReturn = oUserAuthorities.GetPartyViewOptions(v_lUserId:=g_oObjectManager.UserID, r_bIsViewOnlyClientManager:=m_bIsViewOnlyClientManager, r_bIsViewOnlyAgentMaintenance:=m_bIsViewOnlyAgentMaintenance, r_bIsViewOnlyAccountHandlerMaintenance:=m_bIsViewOnlyAccountHandlerMaintenance, r_bIsViewOnlyAccountExecutiveMaintenace:=m_bIsViewOnlyAccountExecutiveMaintenance, r_bIsViewOnlyInsurerMaintenance:=m_bIsViewOnlyInsurerMaintenance, r_bIsViewOnlyOtherPartyMaintenance:=m_bIsViewOnlyOtherPartyMaintenance)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Execute GetPartyViewOptions", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialize", excep:=New Exception(Information.Err().Description))
                Exit Sub
            End If

            'Terminate the object

            oUserAuthorities.Dispose()
            oUserAuthorities = Nothing

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ''' <summary>
    ''' frmInterface_Load
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim iCnt As Integer

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

            'TN20000918 - get hidden option

            m_sAgencyOrunderwriting = g_oBusiness.UnderwritingOrAgency

            'JMK 18/10/2001 - get second hidden option

            m_sUnderwritingType = g_oBusiness.UnderwritingType

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            m_lReturn = LoadRecentFiles()

            If m_sCallingAppName = sFindCashDeptMaintenance Then
                lvwSearchDetails_Click(Nothing, Nothing)
            End If

            If CheckMandatory() <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Inadequate data so cannot
                ' continue with the search.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' TF180199 - Don't display all agents if processing 'Find Agent'
            ' (still need to pre-populate if default Policy Ref is supplied)
            If m_iAgentOnly = 4 Then
                If m_sInsuranceRef = "" Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If
            End If

            If (m_bPartyOther) And (m_sSpecialParty.Length < 3) Then
                If m_bIsViewOnlyOtherPartyMaintenance Then
                    cmdNew.Enabled = False
                    cmdEdit.Text = "&View"
                End If
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            'DC281101 do not search if client (default) is given
            'DC101201 changed check to prevent 'client' auto listing

            ' Comment for Party View
            If (m_sSpecialParty = "") And (Not m_bAutoSearch) Then
                'If party is not a SpecialType (for Client)
                If m_bIsViewOnlyClientManager And m_sTransactionType = "" Then
                    cmdNew.Enabled = False
                End If
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            'Dont get data now if System Option Enhance Filter screens is checked
            If Not m_bSystemOptionEnhanceFilterscreens Then

                ' Gets the interface details to be displayed.
                m_lReturn = m_oGeneral.GetInterfaceDetails()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get the interface details.
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If

            End If

            If cmbType.Text = "Insurer" Then

                m_lReturn = g_oBusiness.CheckInsurerAccess(r_bHasAccess:=m_bHasAccess)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                End If
            Else
                m_bHasAccess = True
            End If
            If Not m_bHasAccess Then
                cmdNew.Enabled = False
                cmdEdit.Enabled = False
            End If

            If UCase(m_sSpecialParty) = PMBPartyTypeAgent Then
                m_lReturn = g_oBusiness.SearchSpecialPartyByQuery(
                    r_lNumberOfRecords:=ACMaxSearchDetails,
                    r_vResultArray:=m_oAgentGroup,
                    v_vShortName:="",
                    v_vName:="",
                    v_vFileCode:="",
                    v_vClientType:=PMBPartyTypeAgentGroupText,
                    v_vAgentType:="",
                    v_vStatusType:="",
                    v_vAddress1:="",
                    v_vPostalCode:="",
                    v_vAreaCode:="",
                    v_vNumber:="",
                    v_vInsuranceRef:="",
                    v_vValidSourceArray:=m_vSourceArray,
                    v_vBranch:=m_iSourceID,
                    v_vActiveStatus:=1,
                    v_bSuppressSubAgents:=m_bSuppressSubAgents,
                    v_bIsInTransferMode:=m_bIsInTransferMode,
                    v_vRiskTransfer:=m_bRiskTransfer,
                    v_vInsurerType:=0)

                cmbAgentGroup.Items.Clear()
                cmbAgentGroup.Items.Add("<ALL>")
                If m_oAgentGroup IsNot Nothing AndAlso Information.IsArray(m_oAgentGroup) Then
                    For iCnt = 0 To UBound(m_oAgentGroup, 2)
                        cmbAgentGroup.Items.Add(Trim(m_oAgentGroup(3, iCnt)))
                    Next iCnt
                End If
                cmbAgentGroup.SelectedIndex = 0
            End If

            'Party View
            'Check if the party is of SpecialType
            If m_sSpecialParty.Length > 0 Then
                Select Case m_sSpecialParty.ToUpper()
                    Case "AG"
                        If m_bIsViewOnlyAgentMaintenance Then
                            cmdNew.Enabled = False
                            cmdEdit.Text = "&View"
                        End If
                    Case "IN"
                        If m_bIsViewOnlyInsurerMaintenance Then
                            cmdNew.Enabled = False
                            cmdEdit.Text = "&View"
                        End If
                    Case "AH"
                        If m_bIsViewOnlyAccountHandlerMaintenance Then
                            cmdNew.Enabled = False
                            cmdEdit.Text = "&View"
                        End If
                    Case "CO"
                        If m_bIsViewOnlyAccountExecutiveMaintenance Then
                            cmdNew.Enabled = False
                            cmdEdit.Text = "&View"
                        End If
                    Case "OT"
                        If m_bIsViewOnlyOtherPartyMaintenance Then
                            cmdNew.Enabled = False
                            cmdEdit.Text = "&View"
                        End If
                End Select
            End If
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            If UnloadMode <> vbFormCode AndAlso Not m_bAlreadyShownClosingValidation Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    'developer guide no. 
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

            ' Destroy the instance of the lock object
            ' from memory.
            If Not (m_oPMLock Is Nothing) Then
                m_oPMLock = Nothing
            End If

            m_vOtherPartyTypes = ""

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown As Integer

        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With tabMainTab
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                        Else
                            ' Check we are not on the
                            ' first tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
                                ' Display the previous tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 1)
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) - 1)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) < (SSTabHelper.GetTabCount(tabMainTab) - 1) Then
                                ' Display the next tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) + 1)
                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If
                End Select
            End With
            'Developer Guide No 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
                tabMainTab.Focus()
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D2 Then
                tabMainTab.SelectedIndex = 1
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D3 Then
                tabMainTab.SelectedIndex = 2
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D4 Then
                tabMainTab.SelectedIndex = 3
            End If
        Catch
            Exit Sub
        End Try

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        Try
            m_lReturn = ResizeInterface()
        Catch

            Exit Sub
        End Try

    End Sub

    Private Sub lvwSearchDetails_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwSearchDetails.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        Dim iRow As Integer

        ' Ram 22-03-2001
        ' To Show the proper telephone number we have to display the
        ' details of the selected row rather than checking the short name
        ' Since a Party can have multiple Telephone Number
        If lvwSearchDetails.Items.Count > 0 Then
            If (eventArgs.KeyCode = Keys.Up) Or eventArgs.KeyCode = Keys.Down Then

                iRow = Convert.ToString(lvwSearchDetails.FocusedItem.Tag)

                txtShortName.Text = CStr(m_vSearchData(ACIShortName, iRow)).Trim()

                txtLongName.Text = CStr(m_vSearchData(ACILongName, iRow)).Trim()

                txtAddress1.Text = CStr(m_vSearchData(ACIAddress1, iRow)).Trim()

                txtPostalCode.Text = CStr(m_vSearchData(ACIPostalCode, iRow)).Trim()

                txtTelephoneCode.Text = CStr(m_vSearchData(ACITelAreaCode, iRow)).Trim()

                txtTelephone.Text = CStr(m_vSearchData(ACITelNumber, iRow)).Trim()
            End If
        End If
    End Sub

    Public Sub mnuRecentFile_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _mnuRecentFile_0.Click, _mnuRecentFile_1.Click, _mnuRecentFile_2.Click, _mnuRecentFile_3.Click, _mnuRecentFile_4.Click, _mnuRecentFile_5.Click
        Dim Index As Integer = Array.IndexOf(mnuRecentFile, eventSender)
        ClearInterface(bConfirm:=False)

        'MKR 29/09/2004 PN 6021 'We are now using '|' as a delimiter rather than ','

        Dim sTemp As String = Convert.ToString(mnuRecentFile(Index).Tag)
        Dim iTemp As Integer = (sTemp.IndexOf("|"c) + 1)
        Dim vPartyShortName As String = sTemp.Substring(0, iTemp - 1)

        ' If we have 2 "&" (for display purposes) then replace with 1   PN17033
        vPartyShortName = vPartyShortName.Replace("&&", "&")

        sTemp = sTemp.Substring(iTemp)
        iTemp = (sTemp.IndexOf("|"c) + 1)
        Dim vPartyResolvedName As String = sTemp.Substring(0, iTemp - 1)
        sTemp = sTemp.Substring(iTemp)
        'ECK 21/05/99 To cope with changes in registry information
        Dim vPartyType As String = sTemp.Substring(sTemp.Length - 1) & "C"
        'ECK 21/05/99 Changed to cope with different registry settings

        'RAG 23-10-01 This line moved from down below, cos otherwise you
        ' get a run-time error 5!
        Dim vPartyCnt As String = sTemp.Substring(0, sTemp.Length - 1)

        'MSS200901 - Added from UW
        Dim dbNumericTemp As Double
        If Not Double.TryParse(vPartyCnt.Substring(vPartyCnt.Length - 1), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            vPartyCnt = vPartyCnt.Substring(0, vPartyCnt.Length - 1)
        End If
        'MSS200901 - Merge End

        'vPartyCnt = Left$(sTemp, Len(sTemp) - 1)
        txtShortName.Text = vPartyShortName
        txtLongName.Text = ""
        txtAddress1.Text = ""
        txtPostalCode.Text = ""
        txtTelephoneCode.Text = ""
        txtTelephone.Text = ""

        m_lPartyCnt = CInt(vPartyCnt)

        cmdFindNow_Click(cmdFindNow, New EventArgs())

    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            With tabMainTab
                ' Set the default button.
                '        If (.Tab < cmdNext.Count) Then
                '            cmdNext(.Tab).Default = True
                '        Else
                '            cmdOK.Default = True
                '        End If

                ' Now I know this is crap, this goes against
                ' all my principles, but for some reason when
                ' using the mouse to select a tab the setfocus
                ' code below doesn't work. The cursor sticks,
                ' and you can't tab off. Therefore I've used
                ' this to get around the problem.
                Application.DoEvents()

                ' Set focus to the first control on the tab.
                If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                    m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                End If
            End With

        Catch
            tabMainTabPreviousTab = tabMainTab.SelectedIndex
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        'modified guide no 33
        'Dim vFileName As String = ""
        Dim vFileName As Object = Nothing
        Dim vResultArray(,) As Object = Nothing
        Dim objItem As ListViewItem
        Dim lAddressCnt_Temp As Integer
        Dim bOtherThanCorrAddress As Boolean

        Dim oParty As Object = Nothing
        Dim sMsg As String = String.Empty
        Dim sClassName As String = String.Empty
        Dim lSelectedItem As Integer
        Dim bPartyTypeIsKnown, bProceed As Boolean

        Dim sValue As String = ""
        Dim lAssociatedClients As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sServiceLevel As String = String.Empty
        Dim sPartyType As String = String.Empty

        ' Click event of the OK button.

        Try

            If IsNothing(lvwSearchDetails.FocusedItem) Then
                lSelectedItem = 0
            Else
                lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)
            End If
            'Check the service level for the selected client. 
            sPartyType = Convert.ToString(m_vSearchData(ACIPartyTypeCode, lSelectedItem)).Trim().ToUpper()
            If sPartyType = PMBConst.PMBPartyTypePersonalClient OrElse sPartyType = PMBConst.PMBPartyTypeGroupClient OrElse
                sPartyType = PMBConst.PMBPartyTypeCorporateClient Then
                sServiceLevel = Convert.ToString(m_vSearchData(ACIPartyServiceLevelCode, lSelectedItem)).Trim().ToUpper()
                Dim dResult As DialogResult
                If sServiceLevel = "RESTRICTED" Then
                    dResult = MessageBox.Show("Client is Restricted, do you want to continue?", "Service Level Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                    If dResult <> Windows.Forms.DialogResult.OK Then
                        Exit Sub
                    End If
                ElseIf sServiceLevel = "OBJECTED" Then
                    dResult = MessageBox.Show("Client has objected, do you want to continue?", "Service Level Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                    If dResult <> Windows.Forms.DialogResult.OK Then
                        Exit Sub
                    End If
                End If
            End If
            ' Blacklisting Client Validation
            lReturn = CType(ClientBlackListingValidation(lSelectedItem), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' SET 08/06/2004 ISS11882 - do we need to show the Data Protection Act questions
            If (m_bDPAIsActive And chkDPARequired.CheckState = CheckState.Checked) And (Not m_bIgnoreDPAQuestions) Then
                'DD 29/10/2003
                'If FSA compliance is enabled then check why the user is viewing the
                'Party

                If m_bSpecialPartyQuery Then
                    m_lReturn = FSACustomerValidate(m_lPartyCnt, CStr(m_vSearchData(ACIPartyTypeCode_SpecialParty, lSelectedItem)).Trim(), m_iIsComplaint, bProceed)
                Else
                    m_lReturn = FSACustomerValidate(m_lPartyCnt, CStr(m_vSearchData(ACIPartyTypeCode, lSelectedItem)).Trim(), m_iIsComplaint, bProceed)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed FSA Customer Validate.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPartyInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If

                'User is not proceeding
                If Not bProceed Then
                    Exit Sub
                End If

                ' PN 12587 - store this setting so that we can read it when using the recent files in CM
                m_lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="ShowFSA", v_sSettingValue:="1")

            Else

                ' PN 12587 - store this setting so that we can read it when using the recent files in CM
                m_lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="ShowFSA", v_sSettingValue:="0")

            End If

            ' Alix - 07/11/2002
            ' If flag set to true, look for multiple addresses
            If m_bAllowAddressSelection Then

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
                        frmMultiAddresses.Top = Me.Top + (Me.Height / 2) - (frmMultiAddresses.Height / 2)
                        frmMultiAddresses.Left = Me.Left + (Me.Width / 2) - (frmMultiAddresses.Width / 2)
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
            End If

            ' Alix - 27/05/2003 - Allow user to amend client details in MTA Roadmap
            If m_sTransactionType = "MTA" Then

                'DJM 04/03/2004
                'Only allow editting of client on multi-company if it belongs to the same branch as what the user is logged in on.
                bProceed = True
                m_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, CStr(g_iSourceID), sValue)
                If sValue = "1" Then

                    If CDbl(m_vSearchData(ACISourceID, lSelectedItem)) <> g_iSourceID Then
                        bProceed = False
                    End If
                End If

                If bProceed Then
                    ' Check party type
                    bPartyTypeIsKnown = True

                    If IsNothing(lvwSearchDetails.FocusedItem) Then
                        lSelectedItem = 0
                    Else
                        lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)
                    End If

                    If m_bSpecialPartyQuery Then
                        m_sSelectedPartyType = CStr(m_vSearchData(ACIPartyTypeCode_SpecialParty, lSelectedItem)).Trim()
                    Else
                        m_sSelectedPartyType = CStr(m_vSearchData(ACIPartyTypeCode, lSelectedItem)).Trim()
                    End If

                    Select Case m_sSelectedPartyType.Trim()
                        Case PMBConst.PMBPartyTypePersonalClient
                            sClassName = "iPMBPartyPC.Interface_Renamed"
                        Case PMBConst.PMBPartyTypeCorporateClient
                            sClassName = "iPMBPartyCC.Interface_Renamed"
                        Case PMBConst.PMBPartyTypeGroupClient
                            sClassName = "iPMBPartyGC.Interface_Renamed"
                            'PN24717
                        Case PMBConst.PMBPartyTypeAgent, PMBConst.PMBPartyTypeAccountHandler, PMBConst.PMBPartyTypeInsurer
                            bPartyTypeIsKnown = False
                        Case Else
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Party type is unknown.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOk_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                            bPartyTypeIsKnown = False
                    End Select

                    ' If we don't know the party type, we don't offer to edit
                    If bPartyTypeIsKnown Then

                        ' Ask user if he wants to amend client details

                        sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmendClientDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        If MessageBox.Show(sMsg, "Amend Client Details", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then

                            ' First we need to lock the party
                            m_lReturn = LockParty()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock party", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOk_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                                Exit Sub
                            End If

                            ' Then we get the object allowing party editing

                            m_lReturn = g_oObjectManager.GetInstance(oObject:=oParty, sClassName:=sClassName, vInstanceManager:=gPMConstants.PMGetLocalInterface)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object " & sClassName, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOk_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                                Exit Sub
                            End If

                            ' Init interface

                            m_lReturn = oParty.Initialise

                            ' Pass in party cnt

                            oParty.PartyCnt = m_lPartyCnt

                            ' Start Interface and wait

                            m_lReturn = oParty.Start()

                            ' terminate

                            oParty.Dispose()
                            oParty = Nothing
                        End If
                    End If
                End If
            End If
            ' /Alix

            ' Check and warn about associated clients
            If m_sTransactionType = "NB" Then
                ' Check party type

                If IsNothing(lvwSearchDetails.FocusedItem) Then
                    lSelectedItem = 0
                Else
                    lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)
                End If

                ' We should only process personal clients
                Dim sPartyTypeCode As String = String.Empty
                If m_bSpecialPartyQuery Then
                    sPartyTypeCode = CStr(m_vSearchData(ACIPartyTypeCode_SpecialParty, lSelectedItem)).Trim()
                Else
                    sPartyTypeCode = CStr(m_vSearchData(ACIPartyTypeCode, lSelectedItem)).Trim()
                End If

                If sPartyTypeCode = PMBConst.PMBPartyTypePersonalClient Then

                    ' Check system option
                    m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=1035, r_sOptionValue:=sValue)

                    ' Check if associated client switch is set
                    If sValue = "1" Then

                        ' Get count of associated clients

                        m_lReturn = g_oBusiness.CheckAssociatedClients(v_lPartyCnt:=m_lPartyCnt, r_lAssociatedClientCount:=lAssociatedClients)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", CheckAssociatedClients, Unable to check associated client count.")
                        End If

                        ' Check if we have associated clients
                        If lAssociatedClients > 0 Then
                            If MessageBox.Show("The insured has client associations. Do you wish to continue?", "Associated Clients", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                                Exit Sub
                            End If
                        End If
                    End If
                End If
            End If

            'JT PN-13238 Check if checkbox is checked or not
            IsIncludeClosedBranchChecked = chkIncludeClosedBranches.CheckState = CheckState.Checked
            ' Set the interface status.
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
                vFileName = m_sShortName & "|" & m_sLongName & "|" & CStr(m_lPartyCnt) & m_sSelectedPartyType.Substring(0, 1)
            Else
                vFileName = m_sShortName & "|" & m_sLongName & "|" & CStr(m_lPartyCnt)
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

        Catch excep As System.Exception
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

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                m_bAlreadyShownClosingValidation = True

                m_lPartyCnt = 0 'PN20059

                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click
        'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.3.2.2)
        Dim sWildcardErrorMessage As String = ""
        'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.3.2.2)

        ' Click event of the Cancel button.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'PN38955 (RC)
            'Input search criteria in at least one of Code or Name
            Dim sMsg As String = ""
            If m_bSystemOptionEnhanceFilterscreens Then

                If txtShortName.Text.Trim() = "" And txtLongName.Text.Trim() = "" And txtInsReference.Text.Trim() = "" And txtClaimNumber.Text.Trim() = "" Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    sMsg = "Please Enter at least one search criteria " &
                           Strings.Chr(13) & Strings.Chr(10) & "(either " & lblShortName.Text.Replace(":", "") &
                           " or " & lblLongName.Text.Replace(":", "")

                    If SSTabHelper.GetTabVisible(tabMainTab, 2) Then
                        sMsg = sMsg & " or " & cmdPolicyRefFind.Text.Replace("...", "")
                    End If
                    If SSTabHelper.GetTabVisible(tabMainTab, 3) Then
                        sMsg = sMsg & " or " & lblClaimNo.Text.Replace(":", "")
                    End If

                    sMsg = sMsg & ")"
                    MessageBox.Show(sMsg, "Find Party", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtShortName.Focus()
                    Exit Sub
                End If

            End If

            ' Tel number fields must be numeric

            Dim dbNumericTemp As Double
            If (txtTelephoneCode.Text.Trim() <> "") And Not Double.TryParse(txtTelephoneCode.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show("Telephone area code must be numeric", "Find Party")
                txtTelephoneCode.Focus()
                Exit Sub
            End If
            'Check wildcard searches

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtShortName.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Party")
                txtShortName.Focus()
                Exit Sub

            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtLongName.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Party")
                txtLongName.Focus()
                Exit Sub

            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtFileCode.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Party")
                txtFileCode.Focus()
                Exit Sub

            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtAddress1.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Party")
                txtAddress1.Focus()
                Exit Sub

            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtPostalCode.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Party")
                txtPostalCode.Focus()
                Exit Sub

            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtInsReference.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Party")
                txtInsReference.Focus()
                Exit Sub

            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtClaimNumber.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Party")
                txtClaimNumber.Focus()
                Exit Sub

            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtRiskIndex.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Party")
                txtRiskIndex.Focus()
                Exit Sub

            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtTelephone.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Party", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtTelephone.Focus()
                Exit Sub

            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtTelephoneCode.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Party", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtTelephoneCode.Focus()
                Exit Sub

            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtDOB.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Party", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtDOB.Focus()
                Exit Sub

            End If


            If m_bDisableWildcardSearchOption Then
                If txtRiskIndex.Text.Trim() = "" And txtClaimNumber.Text.Trim() = "" And txtInsReference.Text.Trim() = "" And txtPostalCode.Text.Trim() = "" And txtAddress1.Text.Trim() = "" And txtFileCode.Text.Trim() = "" And txtLongName.Text.Trim() = "" And txtShortName.Text.Trim() = "" And cmbType.SelectedIndex < 1 And txtTelephoneCode.Text.Trim() = "" And txtTelephone.Text.Trim() = "" And txtDOB.Text.Trim() = "" Then

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    MessageBox.Show("Please provide any search criteria.", "Find Party", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtShortName.Focus()
                    Exit Sub
                End If
            End If

            If g_bGenericConnectionStatus Then

                If (chkPM.CheckState = CheckState.Checked) And (chkSirius.CheckState = CheckState.Checked) Then
                    g_oBusiness.DataSource = PMSearchSiriusPMB
                Else
                    If chkPM.CheckState = CheckState.Checked Then
                        g_oBusiness.DataSource = PMSearchPMB
                    Else
                        g_oBusiness.DataSource = PMSearchSirius
                    End If
                End If

            Else
                g_oBusiness.DataSource = PMSearchSirius
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
                VB6.SetDefault(cmdOK, False)
                cmdEdit.Enabled = True
                cmdUsers.Enabled = True
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

            If Not m_vSearchData Is Nothing Then
                If lvwSearchDetails.SelectedItems.Count > 0 Then
                    m_lPartyCnt = Convert.ToInt64(m_vSearchData(ACIPartyCnt, lvwSearchDetails.SelectedItems(0).Tag))
                End If
            End If

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Find Now command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdFindNow_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

        End Try

    End Sub

    Private Sub cmdNewSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewSearch.Click

        ' Click event of the New Search button.

        Try

            ' Clear the interface details.
            m_lReturn = ClearInterface(bConfirm:=True)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to clear the interface details.
            End If

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the new search command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNewSearch_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNew.Click
        Dim sPartyType As String = ""
        Dim iSourceID As Integer
        Dim frmParties As New frmParties
        Dim frmQASFindName As New frmQASFindName
        ' Click event of the New Button.

        Try

            If (m_sSpecialParty = "") Or (m_bPartyOther) Then

                If m_vNavStep <> "BASIC" Then

                    If m_sSpecialParty.Length > 1 Then
                        frmParties.PartyOther = (m_sSpecialParty.Substring(0, 2) = "OT")
                    End If

                    ' Show form to select a party
                    frmParties.ShowDialog()

                    If frmParties.Status = gPMConstants.PMEReturnCode.PMCancel Then
                        frmParties.Close()
                        frmParties = Nothing

                        Exit Sub
                    End If

                    sPartyType = frmParties.PartyType.Trim()

                    frmParties.Close()
                    frmParties = Nothing
                    m_lReturn = GetCompany(m_iCompanyID:=iSourceID)

                End If
            Else
                If m_sSpecialParty = PMBConst.PMBPartyTypeSubAgent Then
                    sPartyType = PMBConst.PMBPartyTypeAgent
                Else
                    sPartyType = m_sSpecialParty
                End If
            End If
            ' Set the mouse pointer.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'JDW CNIC addition
            'check to see if QAS Names is in use
            m_bChosenAddress = gPMConstants.PMEReturnCode.PMFalse

            If CheckQASNames() Then

                'load form for QAS Names
                m_sPartyType4QAS = sPartyType

                'disable QAsNames for non clients
                If sPartyType = gSIRLibrary.SIRPartyTypePersonalClient Or sPartyType = gSIRLibrary.SIRPartyTypeCorporateClient Or sPartyType = gSIRLibrary.SIRPartyTypeGroupClient Then

                    frmQASFindName.ShowDialog()
                    If m_bQASCancel Then Exit Sub

                End If
            End If

            'Load up relevant interface for chosen party
            ' Process the Party Interface
            'eck120500 Pass source Id
            m_lReturn = ProcessPartyInterface(v_sPartyType:=sPartyType, v_iSourceID:=iSourceID, v_iTask:=gPMConstants.PMEComponentAction.PMAdd)

            ' Set the mouse pointer.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the New button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNew_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        Dim lRowId, lIndex As Integer
        Dim sPartyTypeText As String = ""
        Dim lPartyCnt As Integer

        ' Click event of the Edit Button.

        Try

            ' Get id of the row that has been selected for an edit
            lRowId = Convert.ToString(lvwSearchDetails.FocusedItem.Tag)

            'Get the index
            lIndex = lvwSearchDetails.FocusedItem.Index + 1

            ' Get code and cnt for the selected row id
            'DN 21/05/01 - Change conversion of array field to long instead of int

            lPartyCnt = CInt(m_vSearchData(ACIPartyCnt, lRowId))

            'Get Party Type
            m_lReturn = g_oBusiness.GetPartyType(lPartyCnt:=lPartyCnt, sPartyTypeText:=sPartyTypeText)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get party type", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            ' Process the Party Interface

            'Party View
            If cmdEdit.Text.ToUpper().EndsWith("VIEW") Then
                m_lReturn = ProcessPartyInterface(v_sPartyType:=sPartyTypeText, v_lPartyCnt:=lPartyCnt, v_iTask:=gPMConstants.PMEComponentAction.PMView, v_lIndex:=lIndex)
            Else
                m_lReturn = ProcessPartyInterface(v_sPartyType:=sPartyTypeText, v_lPartyCnt:=lPartyCnt, v_iTask:=gPMConstants.PMEComponentAction.PMEdit, v_lIndex:=lIndex)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Edit button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Enter

        ' GotFocus Event for the search details

        Try

            ' Unset any default buttons so can
            VB6.SetDefault(cmdFindNow, False)
            VB6.SetDefault(cmdOK, False)

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Leave

        ' LostFocus Event for the search details
        Try

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

        Catch excep As System.Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Click

        Dim sShortName As String = ""
        Dim iRow As Integer

        If lvwSearchDetails.Items.Count > 0 Then

            sShortName = lvwSearchDetails.SelectedItems(0).Text
            iRow = Convert.ToString(lvwSearchDetails.SelectedItems(0).Tag)

            ' Alix - 07/11/2002

            m_lPartyCnt = CInt(m_vSearchData(ACIPartyCnt, iRow))


            txtShortName.Text = CStr(m_vSearchData(ACIShortName, iRow)).Trim()

            txtLongName.Text = CStr(m_vSearchData(ACILongName, iRow)).Trim()

            txtAddress1.Text = CStr(m_vSearchData(ACIAddress1, iRow)).Trim()

            txtPostalCode.Text = CStr(m_vSearchData(ACIPostalCode, iRow)).Trim()

            txtTelephoneCode.Text = CStr(m_vSearchData(ACITelAreaCode, iRow)).Trim()

            txtTelephone.Text = CStr(m_vSearchData(ACITelNumber, iRow)).Trim()

            txtFileCode.Text = CStr(m_vSearchData(ACIFileCode, iRow)).Trim()

            If m_sPartyType = "<ALL>" Or m_sPartyType = "Personal Client" Or m_sPartyType = "Corporate Client" Or m_sPartyType = "Group Client" Then
                m_iCurrencyId = CInt(m_vSearchData(ACICurrencyID, iRow))
            End If

            Dim dtDOB As DateTime = DateTime.MinValue
            DateTime.TryParse(Convert.ToString(m_vSearchData(ACIDOB, iRow)).Trim, dtDOB)
            txtDOB.Text = IIf(dtDOB.Equals(DateTime.MinValue), String.Empty, dtDOB.ToShortDateString)

            'PSL 08/10/2002 Issue 863 update Agent Combo when selecting a row
            For n As Integer = 0 To cmbAgentType.Items.Count - 1
                cmbAgentType.SelectedIndex = n
                'MKW PN18388 Remove inconsistencies between table and defined constants
                'If cmbAgentType.Text = Trim(m_vSearchData(ACIAgentType, iRow)) Then

                If cmbAgentType.Text.Replace(" ", "").Replace("-", "").ToUpper() = CStr(m_vSearchData(ACIAgentType, iRow)).Replace(" ", "").Replace("-", "").ToUpper() Then
                    Exit For
                End If
            Next

            VB6.SetDefault(cmdOK, True)

            If m_bHasAccess Then
                cmdEdit.Enabled = True
            End If

            cmdUsers.Enabled = True

        End If

    End Sub

    Private Sub lvwSearchDetails_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.DoubleClick

        ' Double click event for the search details.

        Try

            'Issue No. 13511 Multiple errors when double clicking on empty Find Client list view
            If lvwSearchDetails.Items.Count > 0 Then
                ' CTAF 260601
                ' Call OK_Click. Its just the same code
                cmdOK_Click(cmdOK, New EventArgs())
            End If
        Catch excep As System.Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwSearchDetails.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        If KeyCode <> 13 Then
            VB6.SetDefault(cmdOK, False)
        End If
    End Sub

    Private Sub lvwSearchDetails_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles lvwSearchDetails.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)

        Dim sShortName As String = ""
        Dim iRow As Integer

        If KeyAscii = 13 Then
            If lvwSearchDetails.Items.Count > 0 Then
                sShortName = lvwSearchDetails.FocusedItem.Text

                ' To Show the proper telephone number we have to display the
                ' details of the selected row rather than checking the short name

                iRow = Convert.ToString(lvwSearchDetails.FocusedItem.Tag)

                txtShortName.Text = CStr(m_vSearchData(ACIShortName, iRow)).Trim()

                txtLongName.Text = CStr(m_vSearchData(ACILongName, iRow)).Trim()

                txtAddress1.Text = CStr(m_vSearchData(ACIAddress1, iRow)).Trim()

                txtPostalCode.Text = CStr(m_vSearchData(ACIPostalCode, iRow)).Trim()

                txtTelephoneCode.Text = CStr(m_vSearchData(ACITelAreaCode, iRow)).Trim()

                txtTelephone.Text = CStr(m_vSearchData(ACITelNumber, iRow)).Trim()

                VB6.SetDefault(cmdOK, True)

            End If
        End If

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub lvwSearchDetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchDetails.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwSearchDetails.Columns(eventArgs.Column)

        ' Column click event for the search details
        StoreHScrollValue()
        Try

            'With lvwSearchDetails
            '    ' If current sort column header is
            '    ' pressed.
            '    If ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwSearchDetails) Then
            '        ' Set sort order opposite of
            '        ' current direction.
            '        ListViewHelper.SetSortedProperty(lvwSearchDetails, True)
            '        ListViewHelper.SetSortedProperty(lvwSearchDetails, False)

            '        If ListViewHelper.GetSortOrderProperty(lvwSearchDetails) = SortOrder.Ascending Then
            '            ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Descending)
            '        Else
            '            ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Ascending)
            '        End If
            '        ListViewHelper.SetSortKeyProperty(lvwSearchDetails, ColumnHeader.Index + 1 - 1)
            '    Else
            '        ListViewHelper.SetSortedProperty(lvwSearchDetails, True)
            '        ' Sort by this column (ascending).
            '        ListViewHelper.SetSortedProperty(lvwSearchDetails, False)

            '        ' Turn off sorting so that the list
            '        ' is not sorted twice
            '        'ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Ascending)
            '        If ListViewHelper.GetSortOrderProperty(lvwSearchDetails) = SortOrder.Ascending Then
            '            ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Descending)
            '        Else
            '            ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Ascending)
            '        End If
            '        ListViewHelper.SetSortKeyProperty(lvwSearchDetails, ColumnHeader.Index + 1 - 1)

            '    End If
            'End With
            ListViewFunc.SortListView(lvwSearchDetails, eventArgs)
            RecoverHorizontalScroll()
        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

        End Try

    End Sub
    Private Sub txtCaseNumber_TextChanged(sender As Object, e As EventArgs) Handles txtCaseNumber.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)
    End Sub
    Private Sub txtClaimNumber_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtClaimNumber.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    Private Sub txtDOB_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDOB.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    Private Sub txtDOB_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDOB.Enter

        ' Do what form fields would do
        txtDOB.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=txtDOB.Text)

        ' Select it all
        txtDOB.SelectionStart = 0
        txtDOB.SelectionLength = Strings.Len(txtDOB.Text)

    End Sub

    Private Sub txtDOB_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDOB.Leave
        If txtDOB.Text.Contains("%") Then
        Else
            txtDOB.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=txtDOB.Text)
        End If
    End Sub

    Private Sub txtFileCode_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFileCode.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    Private Sub txtFileCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFileCode.Enter

        ' Highlight any text.
        iPMFunc.SelectText(txtFileCode)
        ' Change the default button.
        VB6.SetDefault(cmdFindNow, True)

    End Sub

    Private Sub txtInsReference_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInsReference.Enter

        ' Highlight any text.
        iPMFunc.SelectText(txtInsReference)

    End Sub

    Private Sub txtInsReference_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInsReference.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    Private Sub txtRiskIndex_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRiskIndex.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    Private Sub txtShortName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtShortName.Enter

        ' Highlight any text.
        iPMFunc.SelectText(txtShortName)

        ' Change the default button.
        VB6.SetDefault(cmdFindNow, True)

    End Sub

    Private Sub txtShortName_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtShortName.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        '    If (Trim$(txtShortName) <> "") Then
        '        txtLongName.Text = ""
        '        txtAddress1.Text = ""
        '        txtPostalCode.Text = ""
        '        txtTelephoneCode.Text = ""
        '        txtTelephone.Text = ""
        '        txtInsReference.Text = ""
        '        'Set party type to ALL
        '        If (m_iAgentOnly = 1) Then
        '            'Find agents only
        '            cmbType.ListIndex = 2
        '        Else
        '            cmbType.ListIndex = 0
        '        End If
        '    End If

        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    Private Sub txtLongName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLongName.Enter

        ' Highlight any text.
        iPMFunc.SelectText(txtLongName)

        ' Change the default button.
        VB6.SetDefault(cmdFindNow, True)

    End Sub

    Private Sub txtLongName_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLongName.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        '    If (Trim$(txtLongName) <> "") Then
        '        txtShortName.Text = ""
        '    End If

        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    ' ***************************************************************** '
    '
    ' Name: CheckEnableDOB
    '
    ' Description: Checks if ALL or Personal client is chosen
    '
    ' History: 26/09/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function CheckEnableDOB() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Select Case cmbType.Text
                Case PMBConst.PMBPartyTypePersonalClientText, "<ALL>"

                    ' Show the controls
                    txtDOB.Visible = True
                    lblDOB.Visible = True

                    m_bDOBEnabled = True

                Case Else

                    ' Hide the controls
                    txtDOB.Visible = False
                    txtDOB.Text = ""
                    lblDOB.Visible = False

                    m_bDOBEnabled = False

            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckEnableDOB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckEnableDOB", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub cmbType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbType.SelectedIndexChanged

        '    If (cmbType.ListIndex <> 0) Then
        '        txtShortName.Text = ""
        '    End If

        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

        'PN_72006 Start
        If cmbType.Text.ToUpper() = "AGENT" Then
            If cmbStatus.Items.Count < 0 Then
                cmbStatus.SelectedIndex = 0
            End If
            cmbStatus.Enabled = False
        Else
            cmbStatus.Enabled = True
        End If
        'PN_72006 End

        m_lReturn = CheckEnableDOB()

    End Sub

    Private Sub txtAddress1_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAddress1.Enter

        ' Highlight any text.
        iPMFunc.SelectText(txtAddress1)

    End Sub

    Private Sub txtAddress1_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAddress1.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        '    If (Trim$(txtAddress1) <> "") Then
        '        txtShortName.Text = ""
        '    End If

        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    Private Sub txtPostalCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPostalCode.Enter

        ' Highlight any text.
        iPMFunc.SelectText(txtPostalCode)

    End Sub

    Private Sub txtPostalCode_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPostalCode.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        '    If (Trim$(txtPostalCode) <> "") Then
        '        txtShortName.Text = ""
        '    End If

        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    Private Sub txtTelephoneCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTelephoneCode.Enter

        ' Highlight any text.
        iPMFunc.SelectText(txtTelephoneCode)

    End Sub

    Private Sub txtTelephoneCode_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTelephoneCode.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        '    If (Trim$(txtTelephoneCode) <> "") Then
        '        txtShortName.Text = ""
        '    End If

        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    Private Sub txtTelephone_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTelephone.Enter

        ' Highlight any text.
        iPMFunc.SelectText(txtTelephone)

    End Sub

    Private Sub txtTelephone_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTelephone.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        '    If (Trim$(txtTelephone) <> "") Then
        '        txtShortName.Text = ""
        '    End If

        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    Private Sub cmdPolicyRefFind_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPolicyRefFind.Click

        m_lReturn = GetPolicyInfo()

    End Sub

    Private Function PopulateOtherPartyTypeLookup() As Integer
        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim iRequiredListIndex As Integer
        Dim bSetListIndex As Boolean

        Const iDESCRIPTION As Integer = 3
        Const iCODE As Integer = 2


        Try


            m_lReturn = g_oBusiness.GetOtherPartyTypes(vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            cmbOtherPartyType.Items.Clear()
            Dim cmbOtherPartyType_NewIndex As Integer = -1
            cmbOtherPartyType_NewIndex = cmbOtherPartyType.Items.Add("<ALL>")
            VB6.SetItemData(cmbOtherPartyType, cmbOtherPartyType_NewIndex, 0)

            'MSS200901 - Added switch
            bSetListIndex = (m_sSpecialParty.Length > 2)
            'DC050402 -broking does as undwerwriting
            '    If m_sAgencyOrUnderwriting <> "U" Then
            '        For iPartyType = LBound(vResultArray, 2) To UBound(vResultArray, 2)
            '            cmbOtherPartyType.AddItem (vResultArray(iDESCRIPTION, iPartyType))
            '            cmbOtherPartyType.ItemData(cmbOtherPartyType.NewIndex) _
            ''                = CLng(Mid(vResultArray(iCODE, iPartyType), 3))
            '            If bSetListIndex Then
            '                If (cmbOtherPartyType.ItemData(cmbOtherPartyType.NewIndex) = CLng(Mid$(m_sSpecialParty, 3, 1))) Then
            '                    iRequiredListIndex = cmbOtherPartyType.NewIndex
            '                End If
            '            End If
            '        Next iPartyType
            '
            '    Else
            '        'MSB161101


            m_vOtherPartyTypes = vResultArray


            For iPartyType As Integer = m_vOtherPartyTypes.GetLowerBound(1) To m_vOtherPartyTypes.GetUpperBound(1)


                If m_bIgnoreDriversAndWitnesses Then

                    If CStr(m_vOtherPartyTypes(iCODE, iPartyType)).Trim() <> "OTDRIVER" Then

                        If CStr(m_vOtherPartyTypes(iCODE, iPartyType)).Trim() <> "OTWITNESS" Then

                            cmbOtherPartyType_NewIndex = cmbOtherPartyType.Items.Add(CStr(m_vOtherPartyTypes(iDESCRIPTION, iPartyType)))
                            VB6.SetItemData(cmbOtherPartyType, cmbOtherPartyType_NewIndex, iPartyType)
                            If bSetListIndex Then
                                If VB6.GetItemData(cmbOtherPartyType, cmbOtherPartyType_NewIndex) = iPartyType Then
                                    iRequiredListIndex = cmbOtherPartyType_NewIndex
                                End If
                            End If
                        End If
                    End If
                ElseIf (CStr(m_vOtherPartyTypes(iCODE, iPartyType)).Trim() = "OTHERPARTY") Then
                    ' alway ignore other party code
                Else

                    cmbOtherPartyType_NewIndex = cmbOtherPartyType.Items.Add(CStr(m_vOtherPartyTypes(iDESCRIPTION, iPartyType)))
                    VB6.SetItemData(cmbOtherPartyType, cmbOtherPartyType_NewIndex, iPartyType)
                    If bSetListIndex Then

                        If CStr(m_vOtherPartyTypes(iCODE, iPartyType)).Trim() = m_sSpecialParty Then
                            iRequiredListIndex = cmbOtherPartyType_NewIndex
                        End If
                    End If
                End If
            Next iPartyType
            '    End If
            'DC050402
            'MSS200901 - Merge End

            'MSS200901 - Replaced SFORB code with UW code.
            If iRequiredListIndex > 0 Then
                cmbOtherPartyType.SelectedIndex = iRequiredListIndex
                cmbOtherPartyType.Enabled = False
            Else
                cmbOtherPartyType.SelectedIndex = 0
                cmbOtherPartyType.Enabled = True
            End If
            'MSS200901 - Merge End

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to populate Party Types combo", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateOtherPartyTypeLookup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetFullAddress
    '
    ' Description:
    '
    ' History: 17/11/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetFullAddress) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetFullAddress(ByVal v_lPartyCnt As Integer, ByRef r_vAddress As Object) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '

    'm_lReturn = m_oBusiness.GetFullAddress(v_lPartyCnt:=v_lPartyCnt, r_vAddress:=r_vAddress)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get correspondance address for party : " & v_lPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetFullAddress", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    'Return result
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFullAddress Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFullAddress", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    '
    ' Name: LoadRecentFiles
    '
    ' Description: Written to use proper use of the registry
    '
    ' History: 26/06/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function LoadRecentFiles() As Integer

        Dim result As Integer = 0
        Dim iLoop1 As Integer
        Dim sKey As String = String.Empty
        Dim sShortName As String = String.Empty
        Dim iTemp As Integer
        Dim sFile As String = String.Empty
        Dim sTemp As String = String.Empty
        Dim sResolvedName As String = String.Empty
        Dim sNames As New StringBuilder
        Dim lPartyCnt As Integer

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".LoadRecentFiles")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise
            sNames = New StringBuilder("#")
            iLoop1 = 1
            sFile = ":-)"

            While (sFile <> "")

                ' Construct the key to check
                sKey = "RecentFile" & iLoop1

                m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=sKey, r_sSettingValue:=sFile, v_sSubKey:="RecentFiles")

                'MKR 29/09/2004 PN 6021 'We are now using '|' as a delimiter rather than ','
                iTemp = (sFile.IndexOf("|"c) + 1)

                ' Check its a valid record
                If iTemp > 0 Then

                    sTemp = Mid(sFile, (IIf(sFile = "" And "|" = "", 0, (sFile.LastIndexOf("|") + 1))) + 1)
                    lPartyCnt = CInt(sTemp.Substring(0, sTemp.Length - 1))

                    'Get party short and resolved names from database in case it has changed.
                    m_lReturn = GetName(lPartyCnt, sShortName, sResolvedName)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Do not show duplicate names.
                    If (sNames.ToString().IndexOf("#" & sShortName & "#") + 1) = 0 Then

                        sShortName = sFile.Substring(0, iTemp - 1)

                        sNames.Append(sShortName & "#")

                        ' If there is an ampersand then double it up so that it displays  PN17033
                        iTemp = (sShortName.IndexOf("&"c) + 1)
                        If iTemp > 0 Then
                            sShortName = sShortName.Substring(0, iTemp - 1) & "&&" & Mid(sShortName, iTemp + 1, sShortName.Length - iTemp)
                        End If
                        ''developer guide no.  added the empty string check	
                        mnuRecentFile(iLoop1).Available = False
                        If (sShortName.Trim.Length > 0) Then
                            mnuRecentFile(iLoop1).Text = sShortName
                            mnuRecentFile(iLoop1).Tag = sShortName & "|" & sResolvedName & "|" & CStr(lPartyCnt) & sFile.Substring(sFile.Length - 1)
                            mnuRecentFile(iLoop1).Available = True
                        End If
                    End If

                End If

                ' Increment to the next file
                iLoop1 += 1

            End While

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".LoadRecentFiles")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".LoadRecentFiles")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadRecentFiles Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadRecentFiles", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

                If mnuRecentFile(iLoop1).Available Then
                    sValue = Convert.ToString(mnuRecentFile(iLoop1).Tag)

                    ' If we have 2 "&" (for display purposes) then replace with 1   PN17033
                    sValue = sValue.Replace("&&", "&")

                Else
                    sValue = ""
                End If

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveRecentFiles Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveRecentFiles", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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
                If mnuRecentFile(iLoop1).Text = v_vFileName.Substring(0, v_vFileName.IndexOf("|"c)) Then
                    'If (mnuRecentFile(iLoop1).Tag = v_vFileName) Then
                    iFound = iLoop1
                    Exit For
                End If
            Next iLoop1

            If iFound = 0 Then
                iFound = 5
            End If

            ' Shuffle 1 to Found down to 2 to Found + 1
            For iLoop1 As Integer = iFound - 1 To 1 Step -1
                mnuRecentFile(iLoop1 + 1).Text = mnuRecentFile(iLoop1).Text
                mnuRecentFile(iLoop1 + 1).Available = mnuRecentFile(iLoop1).Available
                mnuRecentFile(iLoop1 + 1).Tag = Convert.ToString(mnuRecentFile(iLoop1).Tag)
            Next iLoop1

            ' Add the new entry
            iTemp = (v_vFileName.IndexOf("|"c) + 1)
            sShortName = v_vFileName.Substring(0, iTemp - 1)

            ' If there is an ampersand then double it up so that it displays  PN17033
            iTemp = (sShortName.IndexOf("&"c) + 1)
            If iTemp > 0 Then
                sShortName = sShortName.Substring(0, iTemp - 1) & "&&" & Mid(sShortName, iTemp + 1, sShortName.Length - iTemp)
            End If

            mnuRecentFile(1).Text = sShortName
            mnuRecentFile(1).Tag = v_vFileName
            mnuRecentFile(1).Available = True

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".AddRecentFile")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".AddRecentFile")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddRecentFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddRecentFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetupValidPartyTypeArray
    '
    ' Parameters: n/a
    '
    ' Description: If a special party has been specified then limit
    '                the returned parties to that specific party type
    '
    ' History:
    '           Created : MEvans : 22-06-2005 : PN21927
    ' ***************************************************************** '
    Private Function SetupValidPartyTypeArray() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupValidPartyTypeArray"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sPartyType As String = ""
        Dim vPartyTypeDetails As Object = Nothing
        Dim sSpecialParty As String = ""

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            sSpecialParty = m_sSpecialParty

            ' if the party is a type of agent then
            ' default the party type to "AG"
            If (m_sSpecialParty = PMBConst.PMBPartyTypeSubAgent) Or (m_sSpecialParty = PMBConst.PMBPartyTypeCommissionAccount) Or (m_sSpecialParty = PMBConst.PMBAgentTypeBrokerText) Or (m_sSpecialParty = PMBConst.PMBAgentTypeIntroducerText) Or (m_sSpecialParty = PMBConst.PMBPartyTypeIntermediary) Then
                sSpecialParty = "AG"
            Else
                sSpecialParty = m_sSpecialParty
            End If


            lReturn = g_oBusiness.GetPartyTypeByCode(v_sCode:=sSpecialParty, r_vResults:=vPartyTypeDetails)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetPartyTypeByCode", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Information.IsArray(vPartyTypeDetails) Then

                sPartyType = CStr(vPartyTypeDetails(0, 0))
            End If

            ReDim m_vValidPartyTypesArray(0)

            m_vValidPartyTypesArray(0) = sPartyType



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ClientBlackListingValidation
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 27-09-2005 : 358 - BlackList Clients
    ' ***************************************************************** '
    Private Function ClientBlackListingValidation(ByVal v_lSelectedItem As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ClientBlackListingValidation"

        Dim lReturn, lBlackListReasonId As Integer
        Dim sBlackListReasonDesc As String = ""
        Dim vBlackListReason As Object = Nothing
        Dim sPartyType As String = ""

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the selected party's type
            If m_bSpecialPartyQuery Then
                sPartyType = CStr(m_vSearchData(ACIPartyTypeCode_SpecialParty, v_lSelectedItem)).Trim()
            Else
                sPartyType = CStr(m_vSearchData(ACIPartyTypeCode, v_lSelectedItem)).Trim()
            End If

            ' if this is underwriting
            ' and client blacklisting is on
            ' and this is the MTA or NB roadmap
            ' and the selected party is a personal client (PartyType = "PC")
            If m_bSystemOptionClientBlacklistingInForce And (m_sTransactionType = "NB" Or m_sTransactionType = "MTA") And (sPartyType = gSIRLibrary.SIRPartyTypePersonalClient Or sPartyType = gSIRLibrary.SIRPartyTypeCorporateClient Or sPartyType = gSIRLibrary.SIRPartyTypeGroupClient) Then

                ' get the blacklisting reason id / description

                lReturn = g_oBusiness.GetClientBlackListingReason(v_lPartyCnt:=m_lPartyCnt, r_vResults:=vBlackListReason)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetClientBlackListingReason Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' if data has been returned
                If Information.IsArray(vBlackListReason) Then

                    lBlackListReasonId = gPMFunctions.ToSafeLong(CStr(vBlackListReason(0, 0)), 0)

                    sBlackListReasonDesc = CStr(vBlackListReason(1, 0))
                Else
                    ' there should always be some data
                    gPMFunctions.RaiseError(kMethodName, "GetClientBlackListingReason Failed to return any data", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' if the client is black listed (a reason exists)
                If lBlackListReasonId <> 0 Then

                    ' indicate this to the user - giving them the choice of
                    ' whether or not to continue...
                    lReturn = MessageBox.Show("WARNING: This client has been blacklisted for the " & Strings.Chr(13) & Strings.Chr(10) &
                              " following reason:" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &
                              sBlackListReasonDesc & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &
                          " Do you want to continue?", "WARNING: Blacklisted Client", MessageBoxButtons.YesNo)

                    If lReturn = System.Windows.Forms.DialogResult.No Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            Return result

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
        Return result
    End Function
    ''Agent Filtering

    ' R.Griffiths 2006-10-16 (Plus One)
    ' New method to auto search.
    Public Function DoAutoSearch() As Integer

        ' Gets the interface details to be displayed.
        m_lReturn = m_oGeneral.GetInterfaceDetails()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get the interface details.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Exit Function
        End If

        ' Set the mouse pointer to normal.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Exit Function

    End Function

    Public Function ProcessRIBrokerInterface(ByRef vStatus As Integer) As Integer
        Dim result As Integer = 0

        Dim oFindRIParty As iPMUFindRIParty.Interface_Renamed

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the find ri party component.
            Dim temp_oFindRIParty As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindRIParty, sClassName:="iPMUFindRIParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindRIParty = temp_oFindRIParty
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "Unable to get instance of iPMUFindRIParty.Interface")
            End If

            ' Set the property values.

            oFindRIParty.CallingAppName = ACApp

            oFindRIParty.IsFAX = False

            oFindRIParty.PartyCnt = m_lPartyCnt

            ' Set the process modes.
            If m_sCallingAppName = "iPMUTreaty" Then
                oFindRIParty.AddParticipantsFromTreaty = 1
                oFindRIParty.TreatyPartiesBrokerParticipantsForDisplay = m_vTreatyPartiesBrokerParticipantForDisplay
            End If

            m_lReturn = oFindRIParty.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oPartyIn.SetProcessModes", "Unable to set process modes on iPMBFindParty.Interface")
            End If


            m_lReturn = oFindRIParty.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oFindRIParty.Start", "Unable to start Find Party interface")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If oFindRIParty.Status = gPMConstants.PMEReturnCode.PMOK Then


                m_vBrokerArray = oFindRIParty.BrokerArray
            End If


            vStatus = oFindRIParty.Status


            oFindRIParty.Dispose()
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process the Broker Participant Interface.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRIBrokerInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            ' Clear the interface details.
            m_lReturn = ClearInterface(bConfirm:=False)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to clear the interface details.
            End If
            Return result

        End Try
    End Function

    Public Sub CallOK()
        cmdOK_Click(cmdOK, New EventArgs())
    End Sub

    'Start - Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
    Public Sub CalllvwSearchDetailsClick()
        lvwSearchDetails_Click(lvwSearchDetails, New EventArgs())
    End Sub
    'End - Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling

    Public Sub CallFindNow()
        cmdFindNow_Click(cmdFindNow, New EventArgs())
    End Sub
    'Added for resizing the form if the recent files is not visible.
    Public Sub ResizeIfNoMenu()
        MainMenu1.Visible = False
        tabMainTab.Top = 4
        lvwSearchDetails.Top -= 22
        cmdFindNow.Top -= 22
        cmdNewSearch.Top -= 22
        cmdNew.Top -= 22
        cmdEdit.Top -= 22
        cmdOK.Top -= 22
        cmdCancel.Top -= 22
        cmdHelp.Top -= 22
    End Sub
    <DllImport("user32.dll")>
    Private Shared Function GetScrollPos(ByVal hWnd As System.IntPtr, ByVal nBar As Integer) As Integer

    End Function
    <DllImport("user32.dll")>
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As Integer, ByVal lParam As Integer) As Boolean

    End Function
    <DllImport("user32.dll")>
    Private Shared Function LockWindowUpdate(ByVal Handle As IntPtr) As Boolean

    End Function
    'Store the horizontal scroll value.
    Private Sub StoreHScrollValue()
        hScrollValue = GetScrollPos(lvwSearchDetails.Handle, SBS_HORZ)
    End Sub
    'Recover the old scroll position
    Private Sub RecoverHorizontalScroll()
        LockWindowUpdate(lvwSearchDetails.Handle)
        'Calculate the value the scroll needs to scroll back.
        Dim dx As Integer = hScrollValue - GetScrollPos(lvwSearchDetails.Handle, SBS_HORZ)
        'Send the scroll message.
        Dim b As Boolean = SendMessage(lvwSearchDetails.Handle, LVM_SCROLL, dx, 0)
        LockWindowUpdate(IntPtr.Zero)

    End Sub

    Private Sub cmbAgentType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAgentType.SelectedIndexChanged

    End Sub
End Class
