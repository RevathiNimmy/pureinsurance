Option Strict Off
Option Explicit On
Imports System.Globalization
Imports System.Text
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 20/12/2000
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to get the
    '              extra details when quoting.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 27/10/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************


    ' ************************************************
    ' Added to replace global variables 19/09/2003
    ' Username.

    ' Password.

    ' User ID

    ' Calling Application
    ' Source ID
    ' Language ID
    ' Currency ID
    ' LogLevel
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_lPMAuthorityLevel As Integer

    Private m_sGISDataModel As String = ""

    Private m_lPolicyBinderId As Integer
    Private m_lPolicyLinkId As Integer
    Private m_lLastPolicyLinkId As Integer = 0
    Private m_bGotPartyDetails As Boolean
    Private m_bGotTaskAssignmentSubDetails As Boolean
    Private m_bGotInsuranceFileDetails As Boolean
    Private m_bGotPartyAdditionalDetails As Boolean
    Private m_bGotRiskTypeDetails As Boolean

    Private m_lPartyCnt As Integer ' RAM20020812 : To Store the party_cnt
    Private m_sPartyName As Object
    Private m_sGender As Object
    Private m_dtDateOfBirth As Object
    Private m_lAddressCnt As Object
    Private m_lAreaId As Object
    Private m_sTitle As Object
    Private m_sForeName As Object
    Private m_sInitials As Object
    Private m_sSurname As Object
    Private m_sPartyType As Object
    Private m_sRiskTypeCode As Object
    Private m_sRiskTypeDescription As Object
    Private m_sWorkflowInformation As String = ""
    Private m_dtClaimLossDate As Object


    ' RAM20021016 - NRMA Changes - Sirius Process No 126 - Start
    Private m_vOccupationCode As Object ' As per Party Services Declaration
    Private m_vSecondaryOccupationCode As Object ' As per Party Services Declaration
    Private m_vContactDetailsArray As Object
    Private m_sPreferredContactType As String = ""
    Private m_sPreferredContactDetail As String = ""
    Private m_vConvictionDetailsArray As Object
    ' RAM20021016 - NRMA Changes - Sirius Process No 126 - End

    Private m_sProductBroking As Object
    Private m_sProductUnderwriting As Object

    Private m_lLeadInsurerCnt As Object
    Private m_sLeadAgent As Object
    Private m_sLeadAgentCode As Object
    ' CJB 100804 PN13723
    Private m_sSubAgentName As String = ""
    Private m_sSubAgentCode As String = ""
    Private m_vCoverStartDate As Object

    Private m_sTaskDescription As Object
    Private m_sTaskGroup As Object
    Private m_sTaskUserGroup As Object
    Private m_sTaskUser As Object
    Private m_dtTaskDueDate As Object
    Private m_iTaskUrgent As Object
    Private m_sTaskStatus As Object

    ' HG29052003 - Added ability to pass in work manager keys when raising work manager tasks
    Private m_sTaskCode As String = ""
    Private m_vKeyArray As Object

    Private m_iAccumulationUsedElsewhere As Object

    'TN20010716 - start
    Private m_lGetClaimInfo As Integer 'set to pmtrue to get info
    Private m_lInsuranceFileCnt As Integer
    Private m_vClaimYearToCheck As Object 'number of previous years to check
    Private m_vNumberOfClaim As Object
    Private m_vTotalIncurred As Object
    Private m_vLargestIncurred As Object
    Private m_vClaimDetails As Object
    'TN20010716 - end
    'ED16072002
    Private m_bUnderwriting As Boolean

    'GetTaskAssignmentSubDetails()
    Private m_sSource As String = ""
    Private m_sRenewalStopCodeClient As String = ""
    Private m_sRenewalStopCodePolicy As String = ""
    Private m_sAccountExecutiveShortName As String = ""
    Private m_sAccountHandlerShortName As String = ""
    Private m_bIsProductAutoRenewable As Boolean
    Private m_bIsReferredAtRenewal As Boolean
    Private m_sPolicyBranch As String = ""
    Private m_sPolicySubBranch As String = ""
    Private m_sPolicyCurrency As String = ""
    Private m_iPolicySourceID As Integer
    Private m_iPolicyCurrencyID As Integer
    Private m_lAccountID As Integer

    ' RAM20021120 : NRMA Project Process No 204 - Start
    Private m_sClaimsGISDataModel As String = ""
    Private m_lGISDataModelType As Integer
    Private m_lInsuranceFolderCnt As Integer
    Private m_lClaimCnt As Integer
    ' RAM20021120 : NRMA Project Process No 204 - End


    'GetPartyAdditionalData sets these
    Private m_sEmploymentStatusCode As String = ""
    Private m_sEmployerBusiness As String = ""
    Private m_sSecondaryEmploymentStatusCode As String = ""
    Private m_sSecondaryEmployerBusiness As String = ""
    Private m_sSalutation As String = ""
    Private m_sPaymentTermCode As String = ""
    Private m_vRenewalStopCode As Object
    Private m_vCorrespondenceType As Object
    Private m_sPartyBranch As String = ""
    Private m_sPartySubBranch As String = ""

    'sj 10/12/2002 - start
    ' EventTypeCode
    Private m_sEventTypeCode As String = ""
    ' EventDescription
    Private m_sEventDescription As String = ""
    'sj 10/12/2002 - end

    ' HG02062003 - Renwal Failure Reasons
    Private m_vRenwalFailureReasons As Object

    Private m_dtCancellationDate As Date

    ' HG14072003 - Pass in a reference to the data set control
    Private m_oDataSet As Object

    ' AMB 24-Oct-03: 1.8.6 MMM True Monthly Policies - add anniversay date
    Private m_dtAnniversaryDate As Date

    'added to allow list description to ABI code conversion
    Private m_obGISListManager As Object

    Private m_oPBRiskPolicyCurrency As bGISPMUExtras.PBRiskPolicyCurrency
    Private m_oRiskDataSet As Object 'The claim's policy risk data Engine
    Private m_sShortName As String = ""

    Private m_lProductDetailsInsuranceFile As Integer
    Private m_bIsTrueMonthlyPolicy As Boolean

    Private m_lGisSchemeId As Integer 'gis_scheme_id for scheme quotes

    Private m_lCachedGisSchemeId As Integer
    Private m_vCachedScheme As Object

    'MKW 150606
    Private m_lRiskCodeId As Integer
    Private m_lRiskGroupId As Integer
    Private m_lCountryId As Integer

    'PLICO14
    Private m_oPartyDataSet As Object 'The Party data Engine
    Private m_lPartyDataCnt As Integer
    Private m_lNumberOfFleetVehicles As Integer ' 2819

    Public Const kCLAIMSDETAILS_ClaimNumber As Integer = 0
    Public Const kCLAIMSDETAILS_LossDate As Integer = 1
    Public Const kCLAIMSDETAILS_TotalClaimReserve As Integer = 2
    Public Const kCLAIMSDETAILS_Description As Integer = 3
    Public Const kCLAIMSDETAILS_Status As Integer = 4
    Public Const kCLAIMSDETAILS_PrimaryCause As Integer = 5
    Public Const kCLAIMSDETAILS_SecondaryCause As Integer = 6
    Public Const kCLAIMSDETAILS_CatasthropheCode As Integer = 7
    Public Const kCLAIMSDETAILS_LastModifiedDate As Integer = 8
    Public Const kCLAIMSDETAILS_ArrayOfPerils As Integer = 9
    Public Const kCLAIMSDETAILS_ClaimID As Integer = 10
    Public Const kCLAIMSDETAILS_TransactionType As Integer = 11
    Public Const kCLAIMSDETAILS_RiskDescription As Integer = 12
    Public Const kCLAIMSDETAILS_FieldCount As Integer = kCLAIMSDETAILS_RiskDescription

    Public Const kPERIL_Description As Integer = 0
    Public Const kPERIL_IncurredReserve As Integer = 1
    Public Const kPERIL_PaidToDate As Integer = 2
    Public Const kPERIL_ArrayofReserve As Integer = 3
    Public Const kPERIL_PerilID As Integer = 4
    Public Const kPERIL_SumInsured As Integer = 5
    Public Const kPERIL_ArrayOfRecoveries As Integer = 6
    Public Const kPERILType_Code As Integer = 7
    Public Const kPERIL_FieldCount As Integer = kPERILType_Code


    Public Const kRESERVE_ReserveID As Integer = 0
    Public Const kRESERVE_Description As Integer = 1
    Public Const kRESERVE_InitialReserve As Integer = 2
    Public Const kRESERVE_RevisedReserve As Integer = 3
    Public Const kRESERVE_RevisionCnt As Integer = 4
    Public Const kRESERVE_ThisRevision As Integer = 5
    Public Const kRESERVE_ThisPayment As Integer = 6
    Public Const kRESERVE_PaidToDate As Integer = 7
    Public Const kRESERVE_IsExcess As Integer = 8
    Public Const kRESERVE_IsIndemnity As Integer = 9
    Public Const kRESERVE_IsExpense As Integer = 10
    Public Const kRESERVE_ReserveTypeID As Integer = 11
    Public Const kRESERVE_IsUpdated As Integer = 12
    Public Const kRESERVE_SumInsured As Integer = 13
    Public Const kRESERVE_FieldCount As Integer = kRESERVE_SumInsured

    Public Const kRESERVE_CP_ReserveID As Integer = 0
    Public Const kRESERVE_CP_Description As Integer = 1
    Public Const kRESERVE_CP_ThisPayment As Integer = 2
    Public Const kRESERVE_CP_TaxGroupCode As Integer = 3
    Public Const kRESERVE_CP_PayeeType As Integer = 4
    Public Const kRESERVE_CP_PayeeShortCode As Integer = 5
    Public Const kRESERVE_CP_MediaType As Integer = 6
    Public Const kRESERVE_CP_BankPaymentType As Integer = 7
    Public Const kRESERVE_CP_IsExcess As Integer = 8
    Public Const kRESERVE_CP_IsIndemnity As Integer = 9
    Public Const kRESERVE_CP_IsExpense As Integer = 10
    Public Const kRESERVE_CP_ReserveTypeID As Integer = 11
    Public Const kRESERVE_CP_CurrencyCode As Integer = 12
    Public Const kRESERVE_CP_MediaRef As Integer = 13
    Public Const kRESERVE_CP_ClaimPaymentToID As Integer = 14
    Public Const kRESERVE_CP_IsExGratia As Integer = 15
    Public Const kRESERVE_CP_FieldCount As Integer = kRESERVE_CP_IsExGratia

    Public Const kRECOVERY_ReserveID As Integer = 0
    Public Const kRECOVERY_ReserveType As Integer = 1
    Public Const kRECOVERY_IsSalvage As Integer = 2
    Public Const kRECOVERY_ThisReserve As Integer = 3
    Public Const kRECOVERY_IsUpdated As Integer = 4
    Public Const kRECOVERY_PartyTypeId As Integer = 5
    Public Const kRECOVERY_PartyKey As Integer = 6
    Public Const kRECOVERY_InitialReserve As Integer = 7
    Public Const kRECOVERY_RevisedReserve As Integer = 8
    Public Const kRECOVERY_FieldCount As Integer = kRECOVERY_RevisedReserve

    Private m_vPolicyDeductibles As Object
    Private m_vPolicyLimits As Object

    Public m_vClaimDetail_GIS As Object
    Dim m_vClaimsPerils As Object
    Private m_bPaymentExceedReserve As Boolean
    Private m_iProductId As Integer

    'RJ UIICWR59 - Work Manager Enhancement
    Private m_iIsTaskReview As Integer

    '--RFC-PLICO14 - Amit
    Private m_vManualDiscountPercentage As Object

    Public m_oClaimDetail_GIS As Object

    Public Property IsTaskReview() As Integer
        Get
            Return m_iIsTaskReview
        End Get
        Set(ByVal Value As Integer)
            m_iIsTaskReview = Value
        End Set
    End Property


    Public Property TransactionType() As String
        Get
            Return m_sTransactionType
        End Get
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property

    Public WriteOnly Property ClaimReservesUpdated() As Boolean
        Set(ByVal Value As Boolean)
            'THE PROPERTY IS KEPT SO THAT EXISTING USERS WONT CRASH THE SCRIPT
            'OBSOLETE
        End Set
    End Property

    Public WriteOnly Property ClaimPaymentUpdated() As Boolean
        Set(ByVal Value As Boolean)
            'THE PROPERTY IS KEPT SO THAT EXISTING USERS WONT CRASH THE SCRIPT
            'OBSOLETE
        End Set
    End Property

    Public ReadOnly Property CanPaymentExceedReserve() As Boolean
        Get
            If Not m_bPaymentExceedReserve Then
                m_lReturn = IsPaymentExceedReserve()
            End If

            Return m_bPaymentExceedReserve

        End Get
    End Property
    Public Property NumberOfFleetVehicles() As Integer  '2819
        Get
            If Not m_bGotInsuranceFileDetails Then
                m_lReturn = GetInsuranceFileDetails()
            End If
            Return m_lNumberOfFleetVehicles
        End Get
        Set(ByVal vNumberOfFleetVehicles As Integer)

            m_lNumberOfFleetVehicles = vNumberOfFleetVehicles

            m_lReturn = GetInsuranceFileCnt()

            AddParameterLite(m_oDatabase, "insurance_file_cnt", m_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)
            AddParameterLite(m_oDatabase, "number_of_fleet_vehicles", vNumberOfFleetVehicles, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.SQLAction(
                            sSQL:=ACUpdateNumberOfFleetVehiclesSQL,
                            sSQLName:=ACUpdateNumberOfFleetVehiclesName,
                            bStoredProcedure:=True)

            'If m_lReturn <> PMTrue Then
            '    NumberOfFleetVehicles = PMFalse
            'End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lNumberOfFleetVehicles = gPMConstants.PMEReturnCode.PMFalse

            End If
            'TODO   Make a call to the stored procedure spu_SIR_Update_Number_Of_Fleet_Vehicles to update the value in the database.


        End Set
    End Property


    Public ReadOnly Property ManualDiscountPercentage() As Object
        Get
            If Not m_bGotInsuranceFileDetails Then
                m_lReturn = GetInsuranceFileDetails()
            End If

            Return m_vManualDiscountPercentage
        End Get
    End Property


    Public ReadOnly Property IsTrueMonthlyPolicy() As Boolean
        Get

            ' get the product details for the insurance file
            GetProductDetails()

            Return m_bIsTrueMonthlyPolicy

        End Get
    End Property


    Public Property CancellationDate() As Date
        Get
            Return m_dtCancellationDate
        End Get
        Set(ByVal Value As Date)
            m_dtCancellationDate = Value
        End Set
    End Property
    ' RAM20030203 : NRMA Project Process No 426 - End

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    ' HG14072003 - Pass in a reference to the data set control
    Public WriteOnly Property DataSet() As Object
        Set(ByVal Value As Object)
            m_oDataSet = Value
        End Set
    End Property
    'sj 10/12/2002 - start
    Public WriteOnly Property EventDescription() As String
        Set(ByVal Value As String)
            m_sEventDescription = Value
        End Set
    End Property
    Public WriteOnly Property EventTypeCode() As String
        Set(ByVal Value As String)
            m_sEventTypeCode = Value
        End Set
    End Property
    'sj 10/12/2002 - end

    Public Property WorkflowInformation() As String
        Get
            Return m_sWorkflowInformation
        End Get
        Set(ByVal Value As String)
            m_sWorkflowInformation = Value
        End Set
    End Property

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

    Public ReadOnly Property IsProductAutoRenewable() As Boolean
        Get
            If Not m_bGotTaskAssignmentSubDetails Then
                m_lReturn = GetTaskAssignmentSubDetails()
            End If
            Return m_bIsProductAutoRenewable
        End Get
    End Property
    Public ReadOnly Property IsReferredAtRenewal() As Boolean
        Get
            If Not m_bGotTaskAssignmentSubDetails Then
                m_lReturn = GetTaskAssignmentSubDetails()
            End If
            Return m_bIsReferredAtRenewal
        End Get
    End Property

    Public ReadOnly Property BranchCodeFromPolicy() As String
        Get
            If Not m_bGotTaskAssignmentSubDetails Then
                m_lReturn = GetTaskAssignmentSubDetails()
            End If
            Return m_sSource
        End Get
    End Property

    Public ReadOnly Property RenewalStopCodeClient() As String
        Get
            If Not m_bGotTaskAssignmentSubDetails Then
                m_lReturn = GetTaskAssignmentSubDetails()
            End If
            Return m_sRenewalStopCodeClient
        End Get
    End Property
    Public ReadOnly Property RenewalStopCodePolicy() As String
        Get
            If Not m_bGotTaskAssignmentSubDetails Then
                m_lReturn = GetTaskAssignmentSubDetails()
            End If
            Return m_sRenewalStopCodePolicy
        End Get
    End Property
    Public ReadOnly Property AccountExecutiveShortName() As String
        Get
            If Not m_bGotTaskAssignmentSubDetails Then
                m_lReturn = GetTaskAssignmentSubDetails()
            End If
            Return m_sAccountExecutiveShortName
        End Get
    End Property

    Public ReadOnly Property AccountHandlerShortName() As String
        Get
            If Not m_bGotTaskAssignmentSubDetails Then
                m_lReturn = GetTaskAssignmentSubDetails()
            End If
            Return m_sAccountHandlerShortName
        End Get
    End Property
    Public ReadOnly Property PolicyBranch() As String
        Get
            If Not m_bGotTaskAssignmentSubDetails Then
                m_lReturn = GetTaskAssignmentSubDetails()
            End If
            Return m_sPolicyBranch
        End Get
    End Property
    Public ReadOnly Property PolicySubBranch() As String
        Get
            If Not m_bGotTaskAssignmentSubDetails Then
                m_lReturn = GetTaskAssignmentSubDetails()
            End If
            Return m_sPolicySubBranch
        End Get
    End Property

    Public ReadOnly Property RiskTypeCode() As String
        Get
            If Not m_bGotRiskTypeDetails Then
                m_lReturn = GetRiskTypeDetails()
            End If
            Return m_sRiskTypeCode
        End Get
    End Property
    Public ReadOnly Property RiskTypeDescription() As String
        Get
            If Not m_bGotRiskTypeDetails Then
                m_lReturn = GetRiskTypeDetails()
            End If
            Return m_sRiskTypeDescription
        End Get
    End Property


    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public WriteOnly Property GISDataModel() As String
        Set(ByVal Value As String)
            m_sGISDataModel = Value

            ' RAM20021202 : Added a method to fetch, the Data Model Type
            '               say, Risk or Claim
            '               Ref : NRMA Project Process No 204 - Start
            m_lReturn = GetGISDataModelType(v_sGISDataModelCode:=CStr(m_sGISDataModel), r_lGISDataModelTypeID:=m_lGISDataModelType)
            'RAM20021202 : NRMA Project Process No 204 - End

        End Set
    End Property

    Public WriteOnly Property PolicyLinkId() As Integer
        Set(ByVal Value As Integer)

            m_lPolicyLinkId = Value

            'N O T E : Policy Binder ID Value will be always equals to the Policy Link ID
            '           This is by design (AS PER ROB Courtney's Comments)
            m_lPolicyBinderId = Value

        End Set
    End Property

    ' RAM20020812 : Added the following property, if needed we can pass this
    '                property from QEM
    Public WriteOnly Property PartyCnt() As Integer
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property

    Public ReadOnly Property PartyName() As Object
        Get
            If Not m_bGotPartyDetails Then
                m_lReturn = GetPartyDetails()
            End If

            Return m_sPartyName
        End Get
    End Property

    Public ReadOnly Property PartyType() As Object
        Get
            If Not m_bGotPartyDetails Then
                m_lReturn = GetPartyDetails()
            End If

            Return m_sPartyType
        End Get
    End Property

    Public ReadOnly Property Gender() As Object
        Get
            If Not m_bGotPartyDetails Then
                m_lReturn = GetPartyDetails()
            End If

            Return m_sGender
        End Get
    End Property

    Public ReadOnly Property DateOfBirth() As Object
        Get
            If Not m_bGotPartyDetails Then
                m_lReturn = GetPartyDetails()
            End If

            Return m_dtDateOfBirth
        End Get
    End Property

    Public ReadOnly Property AddressCnt() As Object
        Get
            If Not m_bGotPartyDetails Then
                m_lReturn = GetPartyDetails()
            End If

            Return m_lAddressCnt
        End Get
    End Property

    Public ReadOnly Property AreaId() As Object
        Get
            If Not m_bGotPartyDetails Then
                m_lReturn = GetPartyDetails()
            End If

            Return m_lAreaId
        End Get
    End Property

    Public ReadOnly Property Title() As Object
        Get
            If Not m_bGotPartyDetails Then
                m_lReturn = GetPartyDetails()
            End If

            Return m_sTitle
        End Get
    End Property

    Public ReadOnly Property Forename() As Object
        Get
            If Not m_bGotPartyDetails Then
                m_lReturn = GetPartyDetails()
            End If

            Return m_sForeName
        End Get
    End Property

    Public ReadOnly Property Initials() As Object
        Get
            If Not m_bGotPartyDetails Then
                m_lReturn = GetPartyDetails()
            End If

            Return m_sInitials
        End Get
    End Property

    Public ReadOnly Property Surname() As Object
        Get
            If Not m_bGotPartyDetails Then
                m_lReturn = GetPartyDetails()
            End If

            Return m_sSurname
        End Get
    End Property

    ' RAM20021016 - NRMA Changes - Sirius Process No 126 - Start
    Public ReadOnly Property Occupation() As Object
        Get
            If Not m_bGotPartyDetails Then
                m_lReturn = GetPartyDetails()
            End If

            Return m_vOccupationCode
        End Get
    End Property

    Public ReadOnly Property SecondaryOccupation() As Object
        Get
            If Not m_bGotPartyDetails Then
                m_lReturn = GetPartyDetails()
            End If

            Return m_vSecondaryOccupationCode

        End Get
    End Property

    Public ReadOnly Property ContactDetails() As Object
        Get
            If Not m_bGotPartyDetails Then
                m_lReturn = GetPartyDetails()
            End If

            Return m_vContactDetailsArray

        End Get
    End Property

    Public ReadOnly Property PreferredCorrespondenceCode() As String
        Get
            If Not m_bGotPartyDetails Then
                m_lReturn = GetPartyDetails()
            End If

            Return m_sPreferredContactType

        End Get
    End Property

    Public ReadOnly Property PreferredCorrespondenceDetail() As String
        Get
            If Not m_bGotPartyDetails Then
                m_lReturn = GetPartyDetails()
            End If

            Return m_sPreferredContactDetail

        End Get
    End Property

    Public ReadOnly Property ConvictionDetails() As Object
        Get
            If Not m_bGotPartyDetails Then
                m_lReturn = GetPartyDetails()
            End If

            Return m_vConvictionDetailsArray

        End Get
    End Property
    ' RAM20021016 - NRMA Changes - Sirius Process No 126 - End

    Public ReadOnly Property ProductCode() As Object
        Get
            If m_bUnderwriting Then
                If Not m_bGotTaskAssignmentSubDetails Then
                    m_lReturn = GetTaskAssignmentSubDetails()
                End If
                Return m_sProductUnderwriting
            Else

                If Not m_bGotInsuranceFileDetails Then
                    m_lReturn = GetInsuranceFileDetails()
                End If
                Return m_sProductBroking
            End If
        End Get
    End Property

    Public ReadOnly Property LeadAgent() As Object
        Get
            If Not m_bGotInsuranceFileDetails Then
                m_lReturn = GetInsuranceFileDetails()
            End If

            Return m_sLeadAgent
        End Get
    End Property

    Public ReadOnly Property LeadAgentCode() As Object
        Get
            If Not m_bGotInsuranceFileDetails Then
                m_lReturn = GetInsuranceFileDetails()
            End If

            Return m_sLeadAgentCode
        End Get
    End Property

    ' CJB 100804 PN13723
    Public ReadOnly Property SubAgentName() As Object
        Get
            If Not m_bGotInsuranceFileDetails Then
                m_lReturn = GetInsuranceFileDetails()
            End If

            Return m_sSubAgentName
        End Get
    End Property

    ' CJB 100804 PN13723
    Public ReadOnly Property SubAgentCode() As Object
        Get
            If Not m_bGotInsuranceFileDetails Then
                m_lReturn = GetInsuranceFileDetails()
            End If

            Return m_sSubAgentCode
        End Get
    End Property
    Public ReadOnly Property CoverStartDate() As Object
        Get
            If Not m_bGotInsuranceFileDetails Then
                m_lReturn = GetInsuranceFileDetails()
            End If

            Return m_vCoverStartDate
        End Get
    End Property
    Public WriteOnly Property TaskDescription() As Object
        Set(ByVal Value As Object)
            m_sTaskDescription = Value
        End Set
    End Property

    Public WriteOnly Property TaskGroup() As Object
        Set(ByVal Value As Object)
            m_sTaskGroup = Value
        End Set
    End Property

    Public WriteOnly Property TaskUser() As Object
        Set(ByVal Value As Object)
            m_sTaskUser = Value
        End Set
    End Property

    Public WriteOnly Property TaskUserGroup() As Object
        Set(ByVal Value As Object)
            m_sTaskUserGroup = Value
        End Set
    End Property

    Public WriteOnly Property TaskDueDate() As Object
        Set(ByVal Value As Object)
            m_dtTaskDueDate = Value
        End Set
    End Property

    Public WriteOnly Property TaskUrgent() As Object
        Set(ByVal Value As Object)
            m_iTaskUrgent = Value
        End Set
    End Property


    Public Property TaskStatus() As Object
        Get
            Return m_sTaskStatus
        End Get
        Set(ByVal Value As Object)

            m_sTaskStatus = Value
        End Set
    End Property

    ' HG29052003 - Added ability to pass in work manager keys when raising work manager tasks
    ' This property allows the passing of the type of task to raise
    Public WriteOnly Property TaskCode() As Object
        Set(ByVal Value As Object)
            m_sTaskCode = Value
        End Set
    End Property

    'TN20010716 - start
    Public WriteOnly Property ClaimYearToCheck() As Object
        Set(ByVal Value As Object)


            m_vClaimYearToCheck = Value
            m_lGetClaimInfo = gPMConstants.PMEReturnCode.PMTrue
        End Set
    End Property

    Public ReadOnly Property GetClaimInformation() As Object
        Get
            Dim result As Object
            result = GetClaimInfo()
            m_lGetClaimInfo = gPMConstants.PMEReturnCode.PMTrue
            Return result
        End Get
    End Property

    Public ReadOnly Property NumberOfClaim() As Object
        Get
            If m_lGetClaimInfo = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = GetClaimInfo()
                m_lGetClaimInfo = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return m_vNumberOfClaim

        End Get
    End Property

    Public ReadOnly Property TotalIncurred() As Object
        Get
            If m_lGetClaimInfo = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = GetClaimInfo()
                m_lGetClaimInfo = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return m_vTotalIncurred

        End Get
    End Property

    Public ReadOnly Property LargestIncurred() As Object
        Get
            If m_lGetClaimInfo = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = GetClaimInfo()
                m_lGetClaimInfo = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return m_vLargestIncurred

        End Get
    End Property
    'TN20010716 - end

    Public ReadOnly Property ClaimDetails() As Object
        Get
            If m_lGetClaimInfo = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = GetClaimInfo()
                m_lGetClaimInfo = gPMConstants.PMEReturnCode.PMFalse
            End If
            Return DirectCast(m_vClaimDetails, Object(,)).Clone() ' VB6.CopyArray(m_vClaimDetails)
        End Get
    End Property


    Public ReadOnly Property IsUnderwriting() As Boolean
        Get
            Return m_bUnderwriting
        End Get
    End Property

    Public ReadOnly Property AccumulationUsedElsewhere() As Object
        Get

            m_lReturn = GetAccumulationUsedElsewhere()

            Return m_iAccumulationUsedElsewhere
        End Get
    End Property

    ' RAM20021120 : NRMA Project Process No 204 - Start
    Public WriteOnly Property ClaimsGISDataModel() As String
        Set(ByVal Value As String)

            m_sClaimsGISDataModel = Value

            m_lReturn = GetGISDataModelType(v_sGISDataModelCode:=m_sClaimsGISDataModel, r_lGISDataModelTypeID:=m_lGISDataModelType)

        End Set
    End Property

    ' RAM20021120 : NRMA Project Process No 204 - End
    Public ReadOnly Property PartyPreferredCorrespondence() As String
        Get
            If Not m_bGotPartyAdditionalDetails Then
                GetPartyAdditionalDetails()
            End If
            Return m_vCorrespondenceType
        End Get
    End Property
    Public ReadOnly Property PartyEmploymentStatus() As String
        Get
            If Not m_bGotPartyAdditionalDetails Then
                GetPartyAdditionalDetails()
            End If
            Return m_sEmploymentStatusCode
        End Get
    End Property
    Public ReadOnly Property PartyEmployerBusiness() As String
        Get
            If Not m_bGotPartyAdditionalDetails Then
                GetPartyAdditionalDetails()
            End If
            Return m_sEmployerBusiness
        End Get
    End Property
    Public ReadOnly Property PartySecondaryEmploymentStatus() As String
        Get
            If Not m_bGotPartyAdditionalDetails Then
                GetPartyAdditionalDetails()
            End If
            Return m_sSecondaryEmploymentStatusCode
        End Get
    End Property
    Public ReadOnly Property PartySecondaryEmployerBusiness() As String
        Get
            If Not m_bGotPartyAdditionalDetails Then
                GetPartyAdditionalDetails()
            End If
            Return m_sSecondaryEmployerBusiness
        End Get
    End Property
    Public ReadOnly Property PartySalutation() As String
        Get
            If Not m_bGotPartyAdditionalDetails Then
                GetPartyAdditionalDetails()
            End If
            Return m_sSalutation
        End Get
    End Property
    Public ReadOnly Property PartyPaymentTermCode() As String
        Get
            If Not m_bGotPartyAdditionalDetails Then
                GetPartyAdditionalDetails()
            End If
            Return m_sPaymentTermCode
        End Get
    End Property
    Public ReadOnly Property PartyRenewalStopCode() As String
        Get
            If Not m_bGotPartyAdditionalDetails Then
                GetPartyAdditionalDetails()
            End If

            Return CStr(m_vRenewalStopCode)
        End Get
    End Property
    Public ReadOnly Property PartyBranch() As String
        Get
            If Not m_bGotPartyAdditionalDetails Then
                GetPartyAdditionalDetails()
            End If
            Return m_sPartyBranch
        End Get
    End Property
    Public ReadOnly Property PartySubBranch() As String
        Get
            If Not m_bGotPartyAdditionalDetails Then
                GetPartyAdditionalDetails()
            End If
            Return m_sPartySubBranch
        End Get
    End Property
    Public ReadOnly Property IsWorkManagerTaskRequiredForRenewal() As Boolean
        Get

            Static lIsWorkManagerTaskRequiredForRenewal As gPMConstants.PMEReturnCode

            If lIsWorkManagerTaskRequiredForRenewal = gPMConstants.PMEReturnCode.PMFalse Then
                m_lReturn = CheckIfWorkManagerTaskRequired()
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    lIsWorkManagerTaskRequiredForRenewal = gPMConstants.PMEReturnCode.PMTrue
                Else
                    lIsWorkManagerTaskRequiredForRenewal = 2
                End If
            End If

            Return lIsWorkManagerTaskRequiredForRenewal = gPMConstants.PMEReturnCode.PMTrue

        End Get
    End Property

    'HG02062003 - Return renewal failure reasons back to the client
    Public ReadOnly Property RenewalFailureReasons() As Object
        Get
            m_lReturn = GetPreviousRenewalReasons()
            Return m_vRenwalFailureReasons
        End Get
    End Property

    Public ReadOnly Property AnniversaryDate() As Date
        Get
            ' AMB 24-Oct-03: 1.8.6 MMM True Monthly Policies - anniversary_date added

            If Not m_bGotInsuranceFileDetails Then
                m_lReturn = GetInsuranceFileDetails()
            End If

            Return m_dtAnniversaryDate

        End Get
    End Property

    Public ReadOnly Property Username() As String
        Get
            Return m_sUsername
        End Get
    End Property

    '20052005 CLG
    'Give access to the policy risk data of a claim.
    Public ReadOnly Property RiskDataEngine() As Object
        Get
            If m_oRiskDataSet Is Nothing Then
                If LoadRiskDataEngine() <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return Nothing
                End If
            End If
            Return m_oRiskDataSet.Risk
        End Get
    End Property

    'PLICO14 Give access to the Party data
    Public ReadOnly Property PartyDataEngine(ByVal lPartyCnt As Integer) As Object
        Get
            Dim result As Object = Nothing
            If m_oPartyDataSet Is Nothing Or m_lPartyDataCnt <> lPartyCnt Then
                If LoadPartyDataEngine(lPartyCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return Nothing
                End If
            End If
            result = m_oPartyDataSet.Risk
            m_lPartyDataCnt = lPartyCnt
            Return result
        End Get
    End Property

    Public ReadOnly Property PartyShortName() As String
        Get
            If Not m_bGotPartyAdditionalDetails Then
                GetPartyAdditionalDetails()
            End If
            Return m_sShortName.Trim()
        End Get
    End Property
    Public Property SchemeId() As Integer
        Get
            Return m_lGisSchemeId
        End Get
        Set(ByVal Value As Integer)
            m_lGisSchemeId = Value
        End Set
    End Property
    Public ReadOnly Property SchemeDetails() As Object
        Get

            If m_lGisSchemeId <> m_lCachedGisSchemeId Then

                m_vCachedScheme = GetSchemeDetails(m_lGisSchemeId)
                m_lCachedGisSchemeId = m_lGisSchemeId
            End If

            Return m_vCachedScheme

        End Get
    End Property

    Public ReadOnly Property PolicyDeductibles() As Object
        Get

            If Not m_bGotInsuranceFileDetails Then
                m_lReturn = GetInsuranceFileDetails()
            End If
            Return m_vPolicyDeductibles
        End Get
    End Property

    Public ReadOnly Property PolicyLimits() As Object
        Get

            If Not m_bGotInsuranceFileDetails Then
                m_lReturn = GetInsuranceFileDetails()
            End If
            Return m_vPolicyLimits

        End Get
    End Property


    Private Function IsPaymentExceedReserve() As Integer
        Dim result As Integer = 0
        Dim vResult As Object
        Const kMethodName As String = "IsPaymentExceedReserve"

        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = GetInsuranceFileCntForClaim()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        m_lReturn = GetValueFromTable("Insurance_File", "product_id", "Insurance_File_Cnt", gPMFunctions.ToSafeString(m_lInsuranceFileCnt), gPMConstants.PMEDataType.PMInteger, vResult)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "IsPaymentExceedReserve For Product ID Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_iProductId = gPMFunctions.ToSafeInteger(vResult)

        m_lReturn = GetValueFromTable("Product", "payment_cannot_exceed_reserve", "product_id", CStr(m_iProductId), gPMConstants.PMEDataType.PMInteger, vResult)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "IsPaymentExceedReserve For Payment Cannot Exceed Resreve Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_bPaymentExceedReserve = gPMFunctions.ToSafeBoolean(vResult)


        Return result
    End Function
    Public Function GetClaimCnt() As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            bPMAddParameter.AddParameterLite(m_oDatabase, "gis_policy_link_id", m_lPolicyLinkId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)
            result = m_oDatabase.SQLSelect(sSQL:=ACGetClaimCntSQL, sSQLName:=ACGetClaimCntName, bStoredProcedure:=ACGetAddressStored, vResultArray:=vResultArray, bKeepNulls:=True)

            If Informations.IsArray(vResultArray) Then

                m_lClaimCnt = ToSafeLong(vResultArray(0, 0))
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            gPMFunctions.LogMessageToFile(sUsername:=CStr(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimCnt failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:=CStr(result), excep:=excep)
            Return result


        End Try
    End Function



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
    Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Dim sValue As String = "U"
        'As Broker is no longer in use for this codeset so just pass the true for underwriter.
        m_bUnderwriting = True
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Set Username and Password
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            'Default the task variables
            m_sTaskGroup = "UNDER"
            m_sTaskUser = m_sUsername
            m_sTaskUserGroup = ""
            m_sTaskDescription = ""
            m_iTaskUrgent = 0
            m_dtTaskDueDate = DateTime.Today
            sValue = "U"

            'TN20010716 - start
            m_lGetClaimInfo = gPMConstants.PMEReturnCode.PMFalse
            m_bUnderwriting = True
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If m_oPBRiskPolicyCurrency IsNot Nothing Then
                    m_oPBRiskPolicyCurrency.Dispose()
                    m_oPBRiskPolicyCurrency = Nothing
                End If
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If

                If m_oRiskDataSet IsNot Nothing Then
                    m_oRiskDataSet.Dispose()
                    m_oRiskDataSet = Nothing
                End If
                If m_oPartyDataSet IsNot Nothing Then
                    m_oPartyDataSet.Dispose()
                    m_oPartyDataSet = Nothing
                End If
                m_obGISListManager = Nothing

            End If
        End If
        Me.disposedValue = True
    End Sub


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


            If Not Informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Informations.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSumInsured
    '
    ' Description: Get details from DB.
    '
    ' CG20021202 : 1522 Provide alternative to GetBODetails
    ' ***************************************************************** '
    Public Function GetSumInsured(ByRef sSumInsuredType As Object, ByRef cSumInsured As Object, ByRef cMaxSumInsured As Object, ByRef dRate As Object, ByRef cPremium As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vArray(,) As Object
        Dim dtDate As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            dRate = 0
            cPremium = 0
            cSumInsured = 0
            cMaxSumInsured = 0

            sSQL = "SELECT AVG(ISNULL(SI.rate,0))," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "MAX(ISNULL(SI.premium,0))" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "FROM " & m_sGISDataModel & "_sum_insured SI," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "sum_insured_type SIT" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "WHERE SI.sum_insured_type_id = SIT.sum_insured_type_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "AND SI.sum_insured IS NOT NULL " & Strings.ChrW(13) & Strings.ChrW(10) &
                   "AND SIT.code = '" & sSumInsuredType & "'" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "AND SI." & m_sGISDataModel & "_policy_binder_id = " & CStr(m_lPolicyBinderId)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetRateAndPremium", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vArray) Then
                Dim auxVar As Object = vArray(0, 0)


                If Not (Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar)) Then

                    dRate = vArray(0, 0)
                End If

                Dim auxVar_2 As Object = vArray(1, 0)


                If Not (Convert.IsDBNull(auxVar_2) Or Informations.IsNothing(auxVar_2)) Then

                    cPremium = vArray(1, 0)
                End If
            End If

            sSQL = "SELECT SUM(SI.sum_insured)," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "MAX (SI.sum_insured)" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "FROM " & m_sGISDataModel & "_sum_insured SI," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "sum_insured_type SIT" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "WHERE SI.sum_insured_type_id = SIT.sum_insured_type_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "AND SI.sum_insured IS NOT NULL " & Strings.ChrW(13) & Strings.ChrW(10) &
                   "AND SIT.code = '" & sSumInsuredType.Trim() & "'" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "AND SI.date_deleted < {date_deleted}" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "AND SI." & m_sGISDataModel & "_policy_binder_id = " & CStr(m_lPolicyBinderId)


            vArray = Nothing

            m_oDatabase.Parameters.Clear()

            dtDate = #1/1/1900#

            m_lReturn = m_oDatabase.Parameters.Add(sName:="date_deleted", vValue:=dtDate.ToString, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetSumInsured", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vArray) Then
                Dim auxVar_3 As Object = vArray(0, 0)


                If Not (Convert.IsDBNull(auxVar_3) Or Informations.IsNothing(auxVar_3)) Then

                    cSumInsured = vArray(0, 0)
                End If

                Dim auxVar_4 As Object = vArray(1, 0)


                If Not (Convert.IsDBNull(auxVar_4) Or Informations.IsNothing(auxVar_4)) Then

                    cMaxSumInsured = vArray(1, 0)
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetSumInsured Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="GetSumInsured", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetCodeAndIndicator
    '
    ' Description: Get details from DB.
    ' Edit History  :
    ' RAM20040608   : Bug fix for CQ5367
    '                 PMNotFound        : If iLookupValue is not found
    '                 Note: now uses the new stored procedure 'spu_GIS_Get_CodeAndIndicator'
    ' CJB20051214   : PN26388 Since lLookupValue is unique then have taken out recent change
    '                 to check on sLookupType too since customers systems may not have had
    '                 correct value for this but were working! This extra check gives us
    '                 nothing as this does not filter back to sirius log anyway!
    ' ***************************************************************** '
    Public Function GetCodeAndIndicator(ByRef sLookupType As Object, ByRef lLookupValue As Object, ByRef sCode As Object, ByRef sIndicator As Object) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sCode = ""
            sIndicator = ""


            If Object.Equals(lLookupValue, Nothing) Then
                Return result
            End If

            'A bit more validation, as you can't trust the writers of the scripts...

            If Convert.IsDBNull(lLookupValue) Or Informations.IsNothing(lLookupValue) Then
                Return result
            End If


            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(lLookupValue), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return result
            End If

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20040608 : Use Stored procedure, rather than Embedded SQL (To improve performance)
            '               Ref. CQ5367 - START
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            With m_oDatabase

                .Parameters.Clear()

                .Parameters.Add(sName:="LookupValue", vValue:=CStr(lLookupValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                m_lReturn = .SQLSelect(sSQL:=ACGetCodeAndIndicatorSQL, sSQLName:=ACGetCodeAndIndicatorName, bStoredProcedure:=ACGetCodeAndIndicatorStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20040608 : Use Stored procedure, rather than Embedded SQL (To improve performance)
            '               Ref. CQ5367 - END
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            If Informations.IsArray(vArray) Then
                Dim auxVar As Object = vArray(0, 0)


                If Not (Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar)) Then

                    sCode = vArray(0, 0)
                End If

                Dim auxVar_2 As Object = vArray(1, 0)


                If Not (Convert.IsDBNull(auxVar_2) Or Informations.IsNothing(auxVar_2)) Then

                    sIndicator = CStr(vArray(1, 0)).Trim()
                End If
            Else
                ' GIS User Def Header code doesn't match
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetCodeAndIndicator Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="GetCodeAndIndicator", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetIdFromCode
    '
    ' Description: Get details from DB.
    '
    ' ***************************************************************** '
    Public Function GetIdFromCode(ByRef sLookup As Object, ByRef sCode As Object, ByRef lId As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If String.IsNullOrEmpty(sLookup) Then
                Return result
            End If

            If sLookup.Trim() = "" Then
                Return result
            End If


            If String.IsNullOrEmpty(sCode) Then
                Return result
            End If

            If sCode.Trim() = "" Then
                Return result
            End If
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="sCode",
                    vValue:=Trim$(sCode),
                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                    iDataType:=gPMConstants.PMEDataType.PMString)
            If sLookup.StartsWith("0") Then
                sSQL = "spu_getUserDefDetailforID"
                m_lReturn = m_oDatabase.Parameters.Add(sName:="sHeaderID",
                    vValue:=Trim$(sLookup),
                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                    iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                sSQL = "spu_getUserDefDetail"

                m_lReturn = m_oDatabase.Parameters.Add(sName:="sHeaderCode",
                    vValue:=Trim$(sLookup),
                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                    iDataType:=gPMConstants.PMEDataType.PMString)
            End If
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetIdFromCode", bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vArray) Then
                Dim auxVar As Object = vArray(0, 0)
                If Not (Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar)) Then


                    lId = vArray(0, 0)
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetIdFromCode Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="GetIdFromCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Get The address details from the database.
    ''' </summary>
    ''' <param name="lAddressCnt"></param>
    ''' <param name="sAddressLine1"></param>
    ''' <param name="sAddressLine2"></param>
    ''' <param name="sAddressLine3"></param>
    ''' <param name="sAddressLine4"></param>
    ''' <param name="sAddressLine5"></param>
    ''' <param name="r_oAddressLine6"></param>
    ''' <param name="r_oAddressLine7"></param>
    ''' <param name="r_oAddressLine8"></param>
    ''' <param name="r_oAddressLine9"></param>
    ''' <param name="r_oAddressLine10"></param>
    ''' <param name="r_oAddressLine11"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAddress(ByRef lAddressCnt As Object, ByRef sAddressLine1 As Object,
                               ByRef sAddressLine2 As Object, ByRef sAddressLine3 As Object,
                               ByRef sAddressLine4 As Object, ByRef sAddressLine5 As Object,
                               Optional ByRef sAddressLine6 As String = "",
                               Optional ByRef sAddressLine7 As String = "",
                               Optional ByRef sAddressLine8 As String = "",
                               Optional ByRef sAddressLine9 As String = "",
                               Optional ByRef sAddressLine10 As String = "",
                               Optional ByRef sAddressLine11 As String = "") As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim aoArray(,) As Object

        Try

            sAddressLine1 = ""
            sAddressLine2 = ""
            sAddressLine3 = ""
            sAddressLine4 = ""
            sAddressLine5 = ""
            sAddressLine6 = ""
            sAddressLine7 = ""
            sAddressLine8 = ""
            sAddressLine9 = ""
            sAddressLine10 = ""
            sAddressLine11 = ""

            If Object.Equals(lAddressCnt, Nothing) Then
                Return nResult
            End If

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(lAddressCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return nResult
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="address_cnt", vValue:=CStr(lAddressCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAddressSQL, sSQLName:=ACGetAddressName, bStoredProcedure:=ACGetAddressStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=aoArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(aoArray) Then
                Dim auxVar As Object = aoArray(ACAddress1, 0)

                If Not (Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar)) Then

                    sAddressLine1 = CStr(aoArray(ACAddress1, 0)).Trim()
                End If

                Dim auxVar_2 As Object = aoArray(ACAddress2, 0)
                If Not (Convert.IsDBNull(auxVar_2) Or Informations.IsNothing(auxVar_2)) Then
                    sAddressLine2 = CStr(aoArray(ACAddress2, 0)).Trim()
                End If

                Dim auxVar_3 As Object = aoArray(ACAddress3, 0)

                If Not (Convert.IsDBNull(auxVar_3) Or Informations.IsNothing(auxVar_3)) Then

                    sAddressLine3 = CStr(aoArray(ACAddress3, 0)).Trim()
                End If

                Dim auxVar_4 As Object = aoArray(ACAddress4, 0)


                If Not (Convert.IsDBNull(auxVar_4) Or Informations.IsNothing(auxVar_4)) Then

                    sAddressLine4 = CStr(aoArray(ACAddress4, 0)).Trim()
                End If

                Dim auxVar_5 As Object = aoArray(ACPostalCode, 0)

                If Not (Convert.IsDBNull(auxVar_5) Or Informations.IsNothing(auxVar_5)) Then

                    sAddressLine5 = CStr(aoArray(ACPostalCode, 0)).Trim()
                End If
                Dim oAuxVar_6 As Object = aoArray.GetValue(kAcAddress5, 0)

                If Not (Convert.IsDBNull(oAuxVar_6) Or Informations.IsNothing(oAuxVar_6)) Then

                    sAddressLine6 = CStr(aoArray.GetValue(kAcAddress5, 0)).Trim()
                End If

                Dim oAuxVar_7 As Object = aoArray.GetValue(kAcAddress6, 0)

                If Not (Convert.IsDBNull(oAuxVar_7) Or Informations.IsNothing(oAuxVar_7)) Then

                    sAddressLine7 = CStr(aoArray.GetValue(kAcAddress6, 0)).Trim()
                End If

                Dim oAuxVar_8 As Object = aoArray.GetValue(kAcAddress7, 0)


                If Not (Convert.IsDBNull(oAuxVar_8) Or Informations.IsNothing(oAuxVar_8)) Then

                    sAddressLine8 = CStr(aoArray.GetValue(kAcAddress7, 0)).Trim()
                End If

                Dim oAuxVar_9 As Object = aoArray.GetValue(kAcAddress8, 0)

                If Not (Convert.IsDBNull(oAuxVar_9) Or Informations.IsNothing(oAuxVar_9)) Then

                    sAddressLine9 = CStr(aoArray.GetValue(kAcAddress8, 0)).Trim()
                End If

                Dim oAuxVar_10 As Object = aoArray.GetValue(kAcAddress9, 0)

                If Not (Convert.IsDBNull(oAuxVar_10) Or Informations.IsNothing(oAuxVar_10)) Then

                    sAddressLine10 = CStr(aoArray.GetValue(kAcAddress9, 0)).Trim()
                End If

                Dim oAuxVar_11 As Object = aoArray.GetValue(kAcAddress10, 0)


                If Not (Convert.IsDBNull(oAuxVar_11) Or Informations.IsNothing(oAuxVar_11)) Then

                    sAddressLine11 = CStr(aoArray.GetValue(kAcAddress10, 0)).Trim()
                End If
            End If

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAddress Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="GetAddress", excep:=excep)

            Return nResult
        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: IsSameScheme
    '
    ' Description: Scheme Id Comparison check new scheme with all of the
    '              Gis_scheme_ids of Gis_Schemes with the samescheme no
    '
    ' History: 25/07/2002 ED - Created.
    '
    ' ***************************************************************** '
    Public Function IsSameScheme(ByVal v_lOldSchemeId As Object, ByVal v_lNewSchemeId As Object, ByRef r_bIsSameScheme As Object) As Object


        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim vSchemeIdArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_bIsSameScheme = False

            If v_lOldSchemeId = -1 Then

                Return result
            End If

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            ' Old Scheme ID
            m_lReturn = m_oDatabase.Parameters.Add(sName:="OldSchemeId", vValue:=CStr(v_lOldSchemeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' New Scheme ID

            m_lReturn = m_oDatabase.Parameters.Add(sName:="NewSchemeId", vValue:=CStr(v_lNewSchemeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' Call the SP
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACIsSameSchemeSQL, sSQLName:=ACIsSameSchemeName, bStoredProcedure:=True, vResultArray:=vSchemeIdArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            If Not Informations.IsArray(vSchemeIdArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            Else

                If CInt(vSchemeIdArray(0, 0)) = 0 Then

                    r_bIsSameScheme = False
                Else
                    r_bIsSameScheme = True
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsSameScheme Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="IsSameScheme", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPartyDetails
    '
    ' Description:
    ' Edit History
    ' 09/04/2001 Tomo - Created.
    ' RAM20020807 : Changed the way to get the party Cnt
    ' RAM20021016 : Changed the code to use With Operator
    ' RAM20021016 : NRMA Changes - Sirius Process No 126
    ' ***************************************************************** '
    Private Function GetPartyDetails() As Integer

        Dim result As Integer = 0
        Dim oParty As bSIRParty.Services



        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = GetInsuranceFileDetails()

        ' RAM20020807 : Changed the Extras component for a property
        '               if we don't have the Party_Cnt then we get it from Policy Link ID
        If GetPartyCnt() = gPMConstants.PMEReturnCode.PMFalse Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Invalid Party_Cnt: lPartyCnt = " & m_lPartyCnt, vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="GetPartyDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        'We could use party services here...

        oParty = New bSIRParty.Services
        m_lReturn = oParty.Initialise(sUsername:=CStr(m_sUsername), sPassword:=CStr(m_sPassword), iUserID:=CInt(m_iUserID), iSourceID:=CInt(m_iSourceID), iLanguageID:=CInt(m_iLanguageID), iCurrencyID:=CInt(m_iCurrencyID), iLogLevel:=CInt(m_iLogLevel), sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        If oParty Is Nothing Then
            Return result
        End If

        With oParty


            .PartyCnt = m_lPartyCnt


            m_lReturn = .GetDetails

            m_sShortName = .Shortname


            m_sPartyName = .ResolvedName


            m_sGender = .GenderCode


            m_dtDateOfBirth = .DateOfBirth


            m_lAddressCnt = .AddressCnt

            If m_bUnderwriting Then


                m_lAreaId = .AreaId
            Else


                m_lAreaId = .AreaId
            End If



            m_sTitle = .PartyTitleCode


            m_sForeName = .Forename


            m_sInitials = .Initials


            m_sSurname = .Name


            m_sPartyType = .SolutionPartyType

            ' RAM20021016 - NRMA Changes - Sirius Process No 126 - Start


            m_vOccupationCode = .OccupationCode


            m_vSecondaryOccupationCode = .SecondaryOccupationCode


            m_vContactDetailsArray = .AllContactsArray

            m_sPreferredContactType = .PreferredContactType

            m_sPreferredContactDetail = .PreferredContactDetail


            m_vConvictionDetailsArray = .ConvictionsArray
            ' RAM20021016 - NRMA Changes - Sirius Process No 126 - Start

        End With


        oParty.Dispose()

        oParty = Nothing

        m_bGotPartyDetails = True

        Return result

    End Function

    ''' <summary>
    ''' GetInsuranceFileDetails
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetInsuranceFileDetails() As Integer

        Dim nResult As Integer = 0
        Dim sSQL As String = ""
        Dim vResults(,) As Object



        nResult = gPMConstants.PMEReturnCode.PMTrue

        'Get the insurance file cnt...
        If m_lGISDataModelType = 2 Then
            nResult = GetInsuranceFileCntForClaim()
        Else
            If m_lInsuranceFileCnt < 1 Then
                nResult = GetInsuranceFileCnt()
            End If
        End If

        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            Return nResult
        End If

        If m_lInsuranceFileCnt < 1 Then
            Return nResult
        End If

        ' Clear the parameters
        m_oDatabase.Parameters.Clear()

        ' Old Scheme ID
        m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CInt(m_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMError
        End If

        ' Call the SP
        m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_GetInsuranceFileDetails_Extras", sSQLName:="spu_GetInsuranceFileDetails_Extras", bStoredProcedure:=True, vResultArray:=vResults, bKeepNulls:=True)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMError
        End If
        If Not Informations.IsArray(vResults) Then
            nResult = gPMConstants.PMEReturnCode.PMNotFound
        End If

        If Informations.IsArray(vResults) Then

            m_sProductBroking = ToSafeString(vResults(0, 0))
            m_sLeadAgent = ToSafeString(vResults(1, 0))
            m_sLeadAgentCode = vResults(2, 0)
            m_lLeadInsurerCnt = vResults(3, 0)
            m_lPartyCnt = ToSafeInteger(vResults(4, 0))
            m_sSubAgentName = ToSafeString(vResults(5, 0))
            m_sSubAgentCode = ToSafeString(vResults(6, 0))
            m_vCoverStartDate = vResults(7, 0)
            m_dtAnniversaryDate = vResults(8, 0)

            m_lRiskCodeId = ToSafeInteger(vResults(9, 0))
            m_lRiskGroupId = ToSafeInteger(vResults(10, 0))
            m_lCountryId = ToSafeInteger(vResults(11, 0))
            m_vPolicyDeductibles = vResults(12, 0)
            m_vPolicyLimits = vResults(13, 0)

            m_lNumberOfFleetVehicles = 0
            m_vManualDiscountPercentage = ToSafeDecimal(vResults(14, 0))
        End If

        m_bGotInsuranceFileDetails = True

        Return nResult

    End Function

    ' ***************************************************************** '
    '
    ' Name: MatchPMToGIS
    '
    ' Description:
    '
    ' History: 09/04/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function MatchPMToGIS(ByRef sPMLookupType As Object, ByRef lPMLookupValue As Object, ByRef sGISLookupType As Object, ByRef lGISLookupValue As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT d.gis_user_def_detail_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "FROM " & sPMLookupType & " pm," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "gis_user_def_header h," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "gis_user_def_detail d" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "WHERE pm." & sPMLookupType & "_id = " & lPMLookupValue & Strings.ChrW(13) & Strings.ChrW(10) &
                   "AND h.code = '" & sGISLookupType & "'" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "AND h.GIS_user_def_header_id = d.GIS_user_def_header_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "AND d.code = pm.code" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "AND pm.is_deleted = 0" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "AND d.is_deleted = 0"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetpartyCnt", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vArray) Then


                lGISLookupValue = vArray(0, 0)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MatchPMToGIS Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="MatchPMToGIS", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' This Method is used to add the task in theTask Instance.
    ''' </summary>
    ''' <param name="iParentTaskId"></param>
    ''' <param name="bIsExternalItem"></param>
    ''' <param name="guidPMExternalItem"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddTaskToWorkManager(Optional ByVal iParentTaskId As Integer = 0,
                                         Optional ByVal bIsExternalItem As Boolean = False,
                                         Optional ByRef o_GuidPMExternalItem As String = "",
                                         Optional ByVal sExternalTaskCategoryCode As String = "",
                                         Optional ByVal sLockKeyName As String = "",
                                         Optional ByVal nLockKeyValue As Integer = 0,
                                         Optional ByVal nExternalTaskStatus As Integer = -1) As Integer
        ''here three prameter is not defined in TS(nExternalTaskCategoryId,sLockKeyName,nLockKeyValue)
        ''but final integration Decide to add or remove

        Dim nResult As Integer = 0
        Dim oDatabase As dPMDAO.Database

        Dim oWrkTaskInstance As bPMWrkTaskInstance.TaskControl
        'Dim oWrkTaskInstance As bPMWrkTaskInstance.TaskControl

        Dim sSQL As String = ""
        Dim vResultArray(,) As Object
        Dim lPMWrkTaskID As Integer
        Dim lPMWrkTaskGroupID As Integer
        Dim lPMUserGroupID As Integer
        Dim lPMUserID As Integer
        Dim sCustomer As String = ""
        Dim lPMWrkTaskInstanceCnt As Integer

        ' HG29052003
        Dim sTaskCode As String = ""

        'Why is this a const?
        Const PMTaskMemo As String = "MEMO"

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_sTaskStatus = ""

            If Not Informations.IsDate(m_dtTaskDueDate) Then
                m_sTaskStatus = "Invalid Date"
                Return nResult
            End If


            m_lReturn = gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oWrkTaskInstance = New bPMWrkTaskInstance.TaskControl
            m_lReturn = oWrkTaskInstance.Initialise(sUsername:=CStr(m_sUsername), sPassword:=CStr(m_sPassword), iUserID:=CInt(m_iUserID), iSourceID:=CInt(m_iSourceID), iLanguageID:=CInt(m_iLanguageID), iCurrencyID:=CInt(m_iCurrencyID), iLogLevel:=CInt(m_iLogLevel), sCallingAppName:=CStr(ACApp), vDatabase:=oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Get the task group id
            If m_sTaskGroup <> "" Then
                sSQL = "SELECT wtg.pmwrk_task_group_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                       "FROM pmwrk_task_group wtg" & Strings.ChrW(13) & Strings.ChrW(10) &
                       "WHERE wtg.code = {code}" & Strings.ChrW(13) & Strings.ChrW(10)

                ' Clear the database parameters
                oDatabase.Parameters.Clear()

                ' Add the user_id parameter
                m_lReturn = oDatabase.Parameters.Add(sName:="code", vValue:=m_sTaskGroup, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetTaskGroupID", bStoredProcedure:=False, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not Informations.IsArray(vResultArray) Then
                    m_sTaskStatus = "Cannot get task group id from code " & m_sTaskGroup
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                lPMWrkTaskGroupID = CInt(vResultArray(0, 0))
            Else
                m_sTaskStatus = "No task group assigned"
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'Get the user id
            If m_sTaskUser <> "" Then
                If m_sTaskUser = "All Users" Then
                    lPMUserID = 0
                Else
                    sSQL = "SELECT u.user_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                           "FROM pmuser u" & Strings.ChrW(13) & Strings.ChrW(10) &
                           "WHERE u.username = {username}" & Strings.ChrW(13) & Strings.ChrW(10)

                    ' Clear the database parameters
                    oDatabase.Parameters.Clear()

                    ' Add the user_id parameter
                    m_lReturn = oDatabase.Parameters.Add(sName:="username", vValue:=m_sTaskUser, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetUserID", bStoredProcedure:=False, vResultArray:=vResultArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If Not Informations.IsArray(vResultArray) Then
                        m_sTaskStatus = "Cannot get user id from code " & m_sTaskUser
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    lPMUserID = CInt(vResultArray(0, 0))
                End If
            Else
                lPMUserID = m_iUserID
            End If

            'Get the user group id
            If m_sTaskUserGroup <> "" Then
                sSQL = "SELECT ug.pmuser_group_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                       "FROM pmuser_group ug" & Strings.ChrW(13) & Strings.ChrW(10) &
                       "WHERE ug.code = {code}" & Strings.ChrW(13) & Strings.ChrW(10)

                ' Clear the database parameters
                oDatabase.Parameters.Clear()

                ' Add the user_id parameter
                m_lReturn = oDatabase.Parameters.Add(sName:="code", vValue:=m_sTaskUserGroup, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetUserGroupID", bStoredProcedure:=False, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not Informations.IsArray(vResultArray) Then
                    m_sTaskStatus = "Cannot get user group id from code " & m_sTaskUserGroup
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                lPMUserGroupID = CInt(vResultArray(0, 0))

            Else
                'get user group id
                sSQL = "SELECT ugu.pmuser_group_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                       "FROM pmuser_group_user ugu," & Strings.ChrW(13) & Strings.ChrW(10) &
                       "pmuser_group_activity uga" & Strings.ChrW(13) & Strings.ChrW(10) &
                       "WHERE ugu.user_id = {user_id}" & Strings.ChrW(13) & Strings.ChrW(10) &
                       "AND ugu.pmuser_group_id = uga.pmuser_group_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                       "AND uga.pmwrk_task_group_id = {pmwrk_task_group_id}"


                ' Clear the database parameters
                oDatabase.Parameters.Clear()

                ' Add the user_id parameter
                m_lReturn = oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(lPMUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = oDatabase.Parameters.Add(sName:="pmwrk_task_group_id", vValue:=CStr(lPMWrkTaskGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetGroupID", bStoredProcedure:=False, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not Informations.IsArray(vResultArray) Then
                    m_sTaskStatus = "Cannot match user with group"
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                lPMUserGroupID = CInt(vResultArray(0, 0))

            End If

            ' HG29052003 - Allow users to select the type of task they want to raise. Note that
            ' not supplying a task code will mean that this works as it always had - i.e. a MEMO
            ' task will be created.
            If m_sTaskCode = "" Then
                sTaskCode = PMTaskMemo
            Else
                ' Add the insurance file to the key array (this is not done in the script and is hence
                ' abstracted from the user)
                If m_lInsuranceFileCnt = 0 Then
                    GetInsuranceFileCnt()
                End If
                TaskAddKeys("insurance_file_cnt", m_lInsuranceFileCnt)
                sTaskCode = m_sTaskCode
            End If

            ' Get the task_id
            sSQL = "SELECT pmwrk_task_id FROM PMWrk_Task WHERE code = {code}"

            ' Clear the database parameters
            oDatabase.Parameters.Clear()

            ' HG29052003 - Changed Taskcode to no longer be hardcoded to MEMO
            m_lReturn = oDatabase.Parameters.Add(sName:="code", vValue:=sTaskCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetTaskID", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the task_id

            lPMWrkTaskID = CInt(vResultArray(0, 0))

            If sLockKeyName = "" Then
                sLockKeyName = LockName.InvalidValue
            End If

            'create task
            ' HG29052003 - Added key array to end also

            m_lReturn = oWrkTaskInstance.CreateNew(v_lPMWrkTaskGroupID:=lPMWrkTaskGroupID,
                                                  v_lPMWrkTaskID:=lPMWrkTaskID,
                                                  v_sCustomer:=PartyName,
                                                  v_dtTaskDueDate:=m_dtTaskDueDate,
                                                  v_lPMUserGroupID:=lPMUserGroupID,
                                                  v_sDescription:=m_sTaskDescription,
                                                  v_iTaskStatus:=gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew,
                                                  v_iIsUrgent:=m_iTaskUrgent,
                                                  r_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt,
                                                  v_iUserID:=lPMUserID,
                                                  v_vKeyArray:=m_vKeyArray,
                                                  v_iIsTaskReview:=m_iIsTaskReview,
                                                   nParentTaskId:=iParentTaskId,
                                                  bIsExternalWorkItem:=bIsExternalItem,
                                                  r_sGuidPMExternalItem:=o_GuidPMExternalItem,
                                                  sExternalTaskCategoryCode:=sExternalTaskCategoryCode,
                                                  sLockKeyName:=If(sLockKeyName = "", CStr(LockName.InvalidValue), sLockKeyName),
                                                  nLockKeyValue:=nLockKeyValue,
                                                  nExternalTaskStatus:=nExternalTaskStatus)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_sTaskStatus = "Failed to create task"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'close database
            m_lReturn = oDatabase.CloseDatabase()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oDatabase = Nothing


            oWrkTaskInstance.Dispose()
            oWrkTaskInstance = Nothing

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskToWorkManager Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    '
    ' Name: GetInsuranceFileCnt
    '
    ' Description: get insurance file cnt using policy link id
    '
    ' History: 16/07/2001 TN - Created.
    ' CG20021202 : Renamed the function from GetPolicyID To GetInsuranceFileCnt
    ' ***************************************************************** '
    Private Function GetInsuranceFileCnt() As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        result = gPMConstants.PMEReturnCode.PMTrue
        If m_lPolicyLinkId <> m_lLastPolicyLinkId Then

            m_lLastPolicyLinkId = m_lPolicyLinkId
            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=m_lPolicyLinkId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:="spu_GetPolicyDetail", sSQLName:=ACGetPolicyIDName, bStoredProcedure:=True, vResultArray:=vResultArray)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            m_lInsuranceFileCnt = CInt(vResultArray(0, 0))
            m_lPartyCnt = CInt(vResultArray(1, 0))
            m_lInsuranceFolderCnt = CInt(vResultArray(2, 0))
        End If


        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetClaimInfo
    '
    ' Description: get claim details and stored them in
    '              m_lNumberOfClaim, m_cTotalIncurred, m_clargestIncurred
    '
    ' History: 16/07/2001 TN - Created.
    '          See Product Builder Best Practise Guide for results for broking
    '
    ' ***************************************************************** '
    Private Function GetClaimInfo() As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'do we have enough info to query

            If Convert.IsDBNull(m_vClaimYearToCheck) Or Informations.IsNothing(m_vClaimYearToCheck) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'get insurance file count
            result = GetInsuranceFileCnt() ' CG20021202 : 1522 Provide alternative to GetBODetails

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_oDatabase.Parameters.Clear()

            Dim sSQL, sSQL2 As String
            Dim iRetVal As gPMConstants.PMEReturnCode
            Dim vClaimsUserDefinedFields, vClaimsPerils(,) As Object
            Dim vClaimPerilUserDefinedFields As Object
            If m_bUnderwriting Then

                result = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If


                result = m_oDatabase.Parameters.Add(sName:="claim_year_to_check", vValue:=CStr(CInt(m_vClaimYearToCheck)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If


                result = m_oDatabase.SQLSelect(sSQL:=ACGetClaimInfoSQL, sSQLName:=ACGetClaimInfoName, bStoredProcedure:=ACGetClaimInfoStored, vResultArray:=vResultArray, bKeepNulls:=True)

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                If Not Informations.IsArray(vResultArray) Then
                    Return result
                End If



                m_vNumberOfClaim = vResultArray(0, 0)


                m_vTotalIncurred = vResultArray(1, 0)


                m_vLargestIncurred = vResultArray(2, 0)

                Return result
            Else
                ' broking


                'step 1 get the claims records
                sSQL = "spu_get_claim_info_ex"
                sSQL2 = "get_claim_info_ex, step 1"
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "mode", 1, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "ID", m_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "claim_year_to_check", m_vClaimYearToCheck, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                result = m_oDatabase.SQLSelect(vResultArray:=vResultArray, sSQL:=sSQL, sSQLName:=sSQL2, bStoredProcedure:=True, bKeepNulls:=True)

                If Informations.IsArray(vResultArray) Then


                    For iClaimCount As Integer = 0 To vResultArray.GetUpperBound(1)


                        m_vNumberOfClaim = vResultArray.GetUpperBound(1) + 1
                        'step 2, get the claims user defined fields
                        sSQL = "spu_get_claim_info_ex"
                        sSQL2 = "get_claim_info_ex, step 2"
                        bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "mode", 2, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "ID", vResultArray(11, iClaimCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        result = m_oDatabase.SQLSelect(vResultArray:=vClaimsUserDefinedFields, sSQL:=sSQL, sSQLName:=sSQL2, bStoredProcedure:=True, bKeepNulls:=True)
                        If Informations.IsArray(vClaimsUserDefinedFields) Then


                            vResultArray(9, iClaimCount) = vClaimsUserDefinedFields
                        End If

                        'step 3, get the perils
                        sSQL = "spu_get_claim_info_ex"
                        sSQL2 = "get_claim_info_ex, step 3"
                        bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "mode", 3, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "ID", vResultArray(11, iClaimCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "underwriting", If(m_bUnderwriting, 1, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        result = m_oDatabase.SQLSelect(vResultArray:=vClaimsPerils, sSQL:=sSQL, sSQLName:=sSQL2, bStoredProcedure:=True, bKeepNulls:=True)
                        If Informations.IsArray(vClaimsPerils) Then

                            For iPerilCount As Integer = 0 To vClaimsPerils.GetUpperBound(1)

                                'calculate totals


                                vResultArray(2, iClaimCount) = CDbl(vResultArray(2, iClaimCount)) + CDbl(vClaimsPerils(1, CInt(iPerilCount)))

                                m_vTotalIncurred = CDbl(m_vTotalIncurred) + CDbl(vClaimsPerils(1, CInt(iPerilCount)))



                                If m_vLargestIncurred < vClaimsPerils(1, CInt(iPerilCount)) Then


                                    m_vLargestIncurred = vClaimsPerils(1, CInt(iPerilCount))
                                End If

                                'step 4, get the perils user defined fields
                                'get these and set into vClaimsPerils before adding vClaimsPerils to vResultArray
                                sSQL = "spu_get_claim_info_ex"
                                sSQL2 = "get_claim_info_ex, step 4"
                                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "mode", 4, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "ID", vClaimsPerils(4, CInt(iPerilCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                                result = m_oDatabase.SQLSelect(vResultArray:=vClaimPerilUserDefinedFields, sSQL:=sSQL, sSQLName:=sSQL2, bStoredProcedure:=True, bKeepNulls:=True)
                                If Informations.IsArray(vClaimPerilUserDefinedFields) Then


                                    vClaimsPerils(3, CInt(iPerilCount)) = vClaimPerilUserDefinedFields
                                Else


                                    vClaimsPerils(3, CInt(iPerilCount)) = DBNull.Value
                                End If
                            Next


                            vResultArray(10, iClaimCount) = vClaimsPerils
                        End If
                    Next
                End If

                m_vClaimDetails = vResultArray
                Return result
            End If
        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimInfo Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="GetClaimInfo", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

        'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetAccumulationUsedElsewhere
    '
    ' Description: Is the accumulation code on this risk in use on
    '              another policy?
    '
    ' History: 02/11/2001 Tom - Created.
    '
    ' ***************************************************************** '
    Private Function GetAccumulationUsedElsewhere() As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        m_iAccumulationUsedElsewhere = gPMConstants.PMEReturnCode.PMFalse

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(m_lPolicyLinkId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACAccumulationUsedElsewhereSQL, sSQLName:=ACAccumulationUsedElsewhereName, bStoredProcedure:=ACAccumulationUsedElsewhereStored, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        If Informations.IsArray(vResultArray) Then
            m_iAccumulationUsedElsewhere = gPMConstants.PMEReturnCode.PMTrue
            Return result
        End If

        Return result

    End Function

    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=cstr(ACApp), vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: GetPMLookupCode
    '
    ' Description: Get code from pmlookup given id and table
    '
    ' History: 29/11/2001 CLG - Created.
    '          16/07/2002 ED  - Merged from CNIC
    ' ***************************************************************** '
    Public Function GetPMLookupCode(ByVal v_sTableName As Object, ByVal v_lID As Object, ByRef r_vCode As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try


            sSQL = "SELECT code FROM " & v_sTableName & " WHERE " & v_sTableName & "_id = " & v_lID
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACGetCodeFromIDName, bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the code
            If m_oDatabase.Records.Count() <> 0 Then
                r_vCode = m_oDatabase.Records.Item(0).Fields(0)
            Else
                r_vCode = -1
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPMLookupCode Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="GetPMLookupCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetPMLookupRate
    '
    ' Description: See below
    '
    ' History: 29/11/2001 CLG - Created.
    '          13/06/2002 JES - Converted to Embedded SQL for speed
    '          16/07/2002 ED  - Merged from CNIC
    '
    ' ***************************************************************** '
    '
    ' Lookup up rate using upto 3 parameters
    '
    ' Public Function GetPMLookupRate(
    '    ByVal v_vSchemeId As Variant, _                scheme id as number or name
    '    ByVal v_sLookupName As Variant, _              lookup name
    '    ByVal v_vLookup1 As Variant, _                 lookup value 1
    '    ByVal v_vLookup2 As Variant, _                 lookup value 2 if needed
    '    ByVal v_vLookup3 As Variant, _                 lookup value 3 if needed
    '    ByRef r_iReturnValue As Variant, _             see below
    '    ByRef r_vRate As Variant, _                    rate value returned from lookup
    '    Optional ByVal v_dExecutionDate As Variant)    ovveriding execution date
    '
    ' r_iReturnValue
    '    extended return value from stored procedure
    '    0 = okay
    '    1 = scheme id is invalid
    '    2 = gis_rate_type and/or calculation name is invalid
    '    3 = rate not found
    '    10/20/30 = lookup 1/2/3 required but not provided
    '    11/21/31 = code not found for lookup 1/2/3
    '    12/22/32 = group not found for lookup 1/2/3 code
    '
    '
    ' ***************************************************************** '
    Public Function GetPMLookupRate(ByVal v_vSchemeId As Object, ByVal v_sLookupName As Object, ByVal v_vLookup1 As Object, ByVal v_vLookup2 As Object, ByVal v_vLookup3 As Object, ByRef r_iReturnValue As Object, ByRef r_vRate As Object, Optional ByVal v_dExecutionDate As Object = Nothing) As Integer

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            Dim sSQL As String = ""
            Dim vData As Object


            If Informations.IsNothing(v_dExecutionDate) Then
                v_dExecutionDate = DateTime.Today.ToString("yyyy-MM-dd")
            Else
                Dim TempDate As Date
                v_dExecutionDate = If(DateTime.TryParse(v_dExecutionDate, TempDate), TempDate.ToString("yyyy-MM-dd"), v_dExecutionDate)
            End If

            'converted to nasty embedded stuff as PMDAO is crawling
            sSQL = "EXEC spu_pmlookup_rate "
            sSQL = sSQL & v_vSchemeId & ","
            sSQL = sSQL & "'" & v_sLookupName & "',"
            sSQL = sSQL & "'" & v_dExecutionDate & "',"
            sSQL = sSQL & "'" & v_vLookup1 & "',"

            'lookups may be empty so substiture null

            If Convert.IsDBNull(v_vLookup2) Or Informations.IsNothing(v_vLookup2) Then
                sSQL = sSQL & " null ,"
            Else
                sSQL = sSQL & "'" & v_vLookup2 & "',"
            End If


            If Convert.IsDBNull(v_vLookup3) Or Informations.IsNothing(v_vLookup3) Then
                sSQL = sSQL & " null"
            Else
                sSQL = sSQL & "'" & v_vLookup3 & "'"
            End If

            'go for it
            m_lReturn = m_oDatabase.SQLSelect(sSQL, "Get rate", False, , vData)

            'check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'return values
            If Informations.IsArray(vData) Then

                r_iReturnValue = vData(0, 0)

                r_vRate = vData(1, 0)
            Else
                r_iReturnValue = 0
                r_vRate = 0
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPMLookupRate Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="GetPMLookupCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetPreviousRage
    '
    ' Description: get annual rate on rating section for previous risk
    '
    ' History:  20/03/2002 Thinh Nguyen - Created
    '           17/12/2003 Alix - Added r_vRateType parameter
    ' ***************************************************************** '
    Public Function GetPreviousRate(ByVal v_vGisPolicyLinkID As Object, ByVal v_vRatingSectionCode As Object, ByRef r_vRate As Object, Optional ByRef r_vRateType As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_vRate = CStr(0)

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="GisPolicyLinkID", vValue:=CStr(CInt(v_vGisPolicyLinkID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="RatingSectionCode", vValue:=CStr(v_vRatingSectionCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPrevRateSQL, sSQLName:=ACGetPrevRateName, bStoredProcedure:=ACGetPrevRateStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then

                If CStr(vResultArray(0, 0)) = "" Then
                    r_vRate = CStr(0)
                Else

                    r_vRate = CStr(vResultArray(0, 0))
                End If
                ' Alix - 17/12/2003 - PN8991

                If CStr(vResultArray(1, 0)) = "" Then
                    r_vRateType = CStr(0)
                Else

                    r_vRateType = CStr(vResultArray(1, 0))
                End If
                ' /Alix
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPreviousRate Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="GetPreviousRate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Class Name:   GetBODetails
    '
    ' Date:         30 April 2002
    '
    ' Description:  Provides policy details to the rules
    '
    ' Edit History:
    '
    ' JES 30 April 2002 Created
    ' ***************************************************************** '
    Public Function GetBODetails(ByRef r_vData As Object) As Boolean
        Dim result As Boolean = False
        Try

            Dim sSQL As String = ""
            Dim vData(,) As Object

            sSQL = "EXEC spu_getBODetails " & m_lPolicyLinkId

            'Execute as non sp (even though it is).
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetBODetails", bStoredProcedure:=False, vResultArray:=vData)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Informations.IsArray(vData) Then
                'oops
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            'pass


            r_vData = vData

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPreviousRate Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="GetPreviousRate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    ''' <summary>
    ''' generic Method to select UDL columns
    ''' </summary>
    ''' <param name="sTable"></param>
    ''' <param name="sFieldName"></param>
    ''' <param name="sCode"></param>
    ''' <param name="odtEffectiveDate"></param>
    ''' <param name="nVersion"></param>
    ''' <returns></returns>
    Public Function GetField(ByVal sTable As String, ByVal sFieldName As String, ByVal sCode As String,
                Optional ByVal odtEffectiveDate As Object = Nothing,
                        Optional ByVal nVersion As Integer = 0) As Object

        Const kMethodName As String = "GetField"
        Dim sSQL As String = ""
        Dim vData As Object
        Dim bValidUDL As Boolean

        Try
            If sTable.StartsWith("UDL_") Then
                If CheckForValidUDL(sTable, bValidUDL) <> PMEReturnCode.PMTrue Then
                    Throw New Exception
                End If
            End If

            If bValidUDL Then
                If odtEffectiveDate Is Nothing Then
                    If m_sTransactionType = kTRANSACTIONTYPE_OpenClaim Or m_sTransactionType = kTRANSACTIONTYPE_PayClaim Or
                        m_sTransactionType = kTRANSACTIONTYPE_MaintainClaim Then
                        m_lReturn = GetClaimCnt()
                        If m_lClaimCnt > 0 Then
                            m_lReturn = GetClaimDetails(m_lClaimCnt)
                            If m_lReturn <> PMEReturnCode.PMTrue Then
                                GetField = PMEReturnCode.PMFalse
                                Exit Function
                            End If
                            odtEffectiveDate = m_dtClaimLossDate
                        End If
                    Else
                        m_lReturn = GetInsuranceFileCnt()
                        If m_lInsuranceFileCnt > 0 Then
                            m_lReturn = GetInsuranceFileDetails()
                            If m_lReturn <> PMEReturnCode.PMTrue Then
                                GetField = PMEReturnCode.PMFalse
                                Exit Function
                            End If
                            odtEffectiveDate = m_vCoverStartDate
                        End If
                    End If
                End If

                sSQL = "spg_GetField__" & sTable & "__" & sFieldName
                m_oDatabase.Parameters.Clear()

                m_oDatabase.Parameters.Add(sName:="code", vValue:=sCode, iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMString)
                m_oDatabase.Parameters.Add(sName:="version", vValue:=nVersion, iDirection:=PMEParameterDirection.PMParamInput,
                                           iDataType:=PMEDataType.PMInteger)
                m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=ToSafeDate(odtEffectiveDate, DateTime.Now),
                                           iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMDate)
            Else

                sSQL = "spg_GetField__" & sTable & "__" & sFieldName
                m_oDatabase.Parameters.Clear()

                m_oDatabase.Parameters.Add(sName:="code", vValue:=sCode, iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMString)

            End If

            If m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=sSQL, bStoredProcedure:=True, bKeepNulls:=True,
                                   lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vData) <> PMEReturnCode.PMTrue Then

                'COULD NOT FIND SP, SO CREATE IT
                Dim sSQL2 As String = ""
                If Not sTable.ToLower.StartsWith("udl_") Then
                    sSQL2 = "CREATE PROCEDURE spg_GetField__" & sTable & "__" & sFieldName & Strings.ChrW(13) & Strings.ChrW(10) &
                            "@code varchar(50)" & Strings.ChrW(13) & Strings.ChrW(10) &
                            "as" & Strings.ChrW(13) & Strings.ChrW(10) &
                            "SELECT " & sFieldName & Strings.ChrW(13) & Strings.ChrW(10) &
                            " FROM " & sTable & Strings.ChrW(13) & Strings.ChrW(10) &
                            " WHERE code=@code" & Strings.ChrW(13) & Strings.ChrW(10) &
                            ""
                Else

                    ' if procedure exists with 1 parameter then drop the procedure and create with 3 parameter. 
                    m_oDatabase.Parameters.Clear()
                    m_oDatabase.Parameters.Add(sName:="sName", vValue:=sSQL, iDirection:=PMEParameterDirection.PMParamInput,iDataType:=PMEDataType.PMString)
                    m_oDatabase.Parameters.Add(sName:="bQuiet", vValue:=1, iDirection:=PMEParameterDirection.PMParamInput,iDataType:=PMEDataType.PMInteger)
                    m_oDatabase.SQLAction(sSQL:="DDLDropProcedure", sSQLName:="Drop", bStoredProcedure:=True)

                    m_oDatabase.Parameters.Clear()

                    sSQL2 = "CREATE PROCEDURE spg_GetField__" & sTable & "__" & sFieldName & vbCrLf &
                            "@code varchar(50)," & vbCrLf &
                            "@Effective_date datetime = NULL," & vbCrLf &
                            "@Version integer = 0" & vbCrLf &
                            "as" & vbCrLf &
                            "  IF @effective_Date IS NULL  SET @effective_Date = getdate() " & vbCrLf &
                            "  IF @version = 0  " & vbCrLf &
                            "  SELECT " & sFieldName & vbCrLf &
                            "  FROM " & sTable & vbCrLf &
                            "  WHERE code=@code" & vbCrLf &
                            "  and UDL_Version = (Select max(udl_version) FROM " & sTable & " Where effective_date<= @effective_date )" & vbCrLf &
                            "  AND effective_date<=@effective_date " & vbCrLf &
                            "  ELSE " & vbCrLf &
                            "  SELECT " & sFieldName & vbCrLf &
                            "  FROM " & sTable & vbCrLf &
                            "  WHERE code=@code and udl_version = @version " & vbCrLf &
                            "  AND effective_date<=@effective_date "
                End If

                If m_oDatabase.SQLAction(sSQL:=sSQL2, sSQLName:="Create", bStoredProcedure:=False) <> PMEReturnCode.PMTrue Then
                    Throw New Exception
                End If

                Return GetField(sTable, sFieldName, sCode, odtEffectiveDate, nVersion)

            End If

            If Informations.IsArray(vData) Then
                Return vData(0, 0)
            Else
                'Return blank string if no data 
                Return String.Empty
            End If

        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=CStr(m_sUsername),
                               iType:=PMELogLevel.PMLogError,
                               sMsg:="Method Failed!", vClass:=ACClass,
                               vMethod:=kMethodName,
                               excep:=ex)
            Return String.Empty
        End Try

    End Function


    Private Function GetTaskAssignmentSubDetails() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = GetInsuranceFileCnt()  ' CG20021202 : 1522 Provide alternative to GetBODetails

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyDetails Failed ", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="GetTaskAssignmentSubDetails")

            result = gPMConstants.PMEReturnCode.PMError
        End If

        Dim vArray(,) As Object

        m_oDatabase.Parameters.Clear()

        result = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACTaskAssignmentSubDetailsSQL, sSQLName:=ACTaskAssignmentSubDetailsName, bStoredProcedure:=ACTaskAssignmentSubDetailsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Informations.IsArray(vArray) Then


            m_sSource = CStr(vArray(0, 0))

            m_sProductUnderwriting = CStr(vArray(1, 0))

            m_sRenewalStopCodeClient = CStr(vArray(2, 0))

            m_sRenewalStopCodePolicy = CStr(vArray(3, 0))

            m_sAccountExecutiveShortName = CStr(vArray(4, 0))

            m_sAccountHandlerShortName = CStr(vArray(5, 0))

            If m_bUnderwriting Then

                m_bIsProductAutoRenewable = CBool(vArray(6, 0))

                m_bIsReferredAtRenewal = CBool(vArray(7, 0))
            End If


            m_sPolicyBranch = CStr(vArray(8, 0))

            m_sPolicySubBranch = CStr(vArray(9, 0))

            'Extra fields for use with DoCurrencyconversion function

            m_sPolicyCurrency = CStr(vArray(10, 0))

            m_iPolicySourceID = CInt(vArray(11, 0))

            m_iPolicyCurrencyID = CInt(vArray(12, 0))

            m_lAccountID = CInt(vArray(13, 0))
        End If


        m_bGotTaskAssignmentSubDetails = True
        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: GwetPartyAdditionalDetails
    '
    ' Description:
    '
    ' History: 22/11/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Private Function GetPartyAdditionalDetails() As Integer

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".GetPartyAdditionalDetails")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'working variables
            Dim vCorrespondenceTypes(,) As Object
            Dim sSQL As String = ""
            Dim oParty As bSIRPartyPC.Business
            Dim vResults As Object
            'oParty.GetNext returns these which we use indirectlty
            Dim vRenewalStopCodeId As Object
            Dim vCorrespondenceTypeId, lSubBranchId As Integer
            Dim vPartyCode As String = ""

            'oParty.GetNext could return these but as yet we don't need them
            'Dim m_sPartyTitleCode
            'Dim m_iPartySourceId
            'Dim m_lPartyId
            'Dim m_sInitials$
            'Dim m_sMaritalStatusCode
            'Dim m_lNumberOFChildren
            'Dim m_vNationalityId
            'Dim m_iMailshot
            'Dim m_iIsPetOwner
            'Dim m_sAccommodationTypeCode
            'Dim m_sShortName$
            'Dim m_sSurName$
            'Dim m_iIsAlsoAgent
            'Dim m_iIsProspect
            'Dim m_lAgentCnt
            'Dim m_lConsultantCnt
            'Dim m_sFileCode
            'Dim m_iCurrencyId
            'Dim m_sPaymentMethodCode
            'Dim m_lReminderTypeID
            'Dim m_iAreaId
            'Dim m_lServiceLevelId
            'Dim m_sCreditCardCode
            'Dim m_lCCJs
            'Dim m_vSeasonalGiftId
            'Dim m_vRenewalStopCodeId
            'Dim m_lSwiftPartyID&
            'Dim m_sLoyaltyNumber
            'Dim m_sAlternativeIdentifier
            'Dim m_sTradingName
            'Dim m_lSubBranchId
            'Dim vCurrencyId

            If GetPartyCnt() = gPMConstants.PMEReturnCode.PMFalse Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Invalid Party_Cnt: lPartyCnt = " & m_lPartyCnt, vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="GetPartyAdditionalDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If GetPartyCode(r_vPartyCode:=vPartyCode) = gPMConstants.PMEReturnCode.PMFalse Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Invalid Party_Cnt: lPartyCode = " & vPartyCode, vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="GetPartyAdditionalDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If
            Dim lBranchCode As Integer
            If vPartyCode.Trim() = "PC" Then

                oParty = New bSIRPartyPC.Business
                m_lReturn = oParty.Initialise(sUsername:=CStr(m_sUsername), sPassword:=CStr(m_sPassword), iUserID:=CInt(m_iUserID), iSourceID:=CInt(m_iSourceID), iLanguageID:=CInt(m_iLanguageID), iCurrencyID:=CInt(m_iCurrencyID), iLogLevel:=CInt(m_iLogLevel), sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)


                'get the additional party details we currently need

                m_lReturn = oParty.GetDetails(vPartyCnt:=m_lPartyCnt)

                m_lReturn = oParty.GetNext(vPartyCnt:=m_lPartyCnt, vEmploymentStatusCode:=m_sEmploymentStatusCode, vEmployerBusiness:=m_sEmployerBusiness, vSecondaryEmploymentStatusC:=m_sSecondaryEmploymentStatusCode, vSecondaryEmployerBusiness:=m_sSecondaryEmployerBusiness, vSalutation:=m_sSalutation, vPaymentTermCode:=m_sPaymentTermCode, vRenewalStopCodeId:=vRenewalStopCodeId, vCorrespondenceTypeId:=vCorrespondenceTypeId, vSubBranchId:=lSubBranchId, vShortname:=m_sShortName)


                'convert the m_vCorrespondenceTypeId into a string m_vCorrespondenceType

                m_lReturn = oParty.GetCorrespondenceTypes(vCorrespondenceTypes:=vCorrespondenceTypes)
                For lCount As Integer = 0 To vCorrespondenceTypes.GetUpperBound(1)
                    If CInt(vCorrespondenceTypes(0, lCount)) = vCorrespondenceTypeId Then
                        m_vCorrespondenceType = CStr(vCorrespondenceTypes(1, lCount))
                        lCount = vCorrespondenceTypes.GetUpperBound(1)
                    End If
                Next

                'convert the m_vRenewalStopCodeId into a string m_vRenewalStopCode
                sSQL = ACGetRenewalStopCodeCaptionIdSQL
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "renewal_stop_code_id", vRenewalStopCodeId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, v_bIgnoreIfBlank:=True)
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACGetRenewalStopCodeCaptionIdName, bStoredProcedure:=ACGetRenewalStopCodeCaptionIdStored, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)
                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And Informations.IsArray(vResults) Then
                        sSQL = ACGetCaptionDescSQL
                        bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "language_id", 1, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, v_bIgnoreIfBlank:=True)
                        bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "caption_id", vResults(0, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, v_bIgnoreIfBlank:=True)
                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACGetCaptionDescName, bStoredProcedure:=ACGetCaptionDescStored, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)
                            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then


                                m_vRenewalStopCode = vResults(0, 0)
                            End If
                        End If
                    End If
                End If

                'given the sub branch id we need to get the branch and sub branch name
                'get the sub branch caption id and the branch id
                sSQL = ACGetSubBranchDetailsSQL
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "sub_branch_id", lSubBranchId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, v_bIgnoreIfBlank:=True)
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACGetSubBranchDetailsName, bStoredProcedure:=ACGetSubBranchDetailsStored, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)
                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And Informations.IsArray(vResults) Then

                        lBranchCode = CInt(vResults(1, 0))
                        sSQL = ACGetCaptionDescSQL
                        bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "language_id", 1, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, v_bIgnoreIfBlank:=True)
                        bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "caption_id", vResults(0, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, v_bIgnoreIfBlank:=True)
                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACGetCaptionDescName, bStoredProcedure:=ACGetCaptionDescStored, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)
                            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And Informations.IsArray(vResults) Then

                                m_sPartySubBranch = CStr(vResults(0, 0))
                                'now get the branch details
                                sSQL = ACGetBranchCaptionIdSQL
                                bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "source_id", lBranchCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, v_bIgnoreIfBlank:=True)
                                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACGetBranchCaptionIdName, bStoredProcedure:=ACGetBranchCaptionIdStored, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)
                                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And Informations.IsArray(vResults) Then
                                        'now get the branch caption
                                        sSQL = ACGetCaptionDescSQL
                                        bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "language_id", 1, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, v_bIgnoreIfBlank:=True)
                                        bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "caption_id", vResults(0, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, v_bIgnoreIfBlank:=True)
                                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACGetCaptionDescName, bStoredProcedure:=ACGetCaptionDescStored, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)
                                            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                                                m_sPartyBranch = CStr(vResults(0, 0))
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            If vPartyCode.Trim() = "CC" Then

                bPMAddParameter.AddParameterLite(m_oDatabase, "party_cnt", m_lPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartySQL, sSQLName:=ACGetPartyName, bStoredProcedure:=ACGetPartyStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    If Informations.IsArray(vResults) Then

                        m_sShortName = CStr(vResults(6, 0))
                    End If
                End If

            End If
            'signal details have been cached
            m_bGotPartyAdditionalDetails = True

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".GetPartyAdditionalDetails")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".GetPartyAdditionalDetails")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyAdditionalDetails Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="GetPartyAdditionalDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetPartyCnt
    '
    ' Description: Derives PartyCnt from insurance_file table
    '
    ' History: 22/11/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Private Function GetPartyCnt() As Integer

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".GetPartyCnt")

        Dim sSQL As String = ""
        Dim vArray As Object

        Dim bStoredProcedure As Boolean

        result = PMEReturnCode.PMTrue

        'check if we already have it
        If m_lPartyCnt > 0 Then
            Return result
        End If

        ' Note : If the Party_Cnt = 0 then we have to get it from the Insurance_file table
        '        The insurance_file_cnt / insurance_folder_cnt is stored in the gis_policy_link table
        '        Insurance_file_cnt for SBO
        '        Insurance_folder_cnt for SFU

        ' Check we have a proper Poliy Link ID
        If m_lPolicyLinkId < 1 Then
            Return PMEReturnCode.PMFalse
        End If

        bStoredProcedure = False

        If CurrentClaimKey = 0 Then
            If m_lGISDataModelType = GISDMTypeParty Then
                AddParameterLite(m_oDatabase, "gis_policy_link_id", m_lPolicyLinkId, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)
                sSQL = "Spu_GIS_GetPartyCnt"
                bStoredProcedure = True
            Else

                sSQL = "SELECT INSF.insured_cnt " & Strings.ChrW(13) & Strings.ChrW(10) &
                       "FROM Insurance_file INSF, " & Strings.ChrW(13) & Strings.ChrW(10) &
                       "     gis_policy_link GPL" & Strings.ChrW(13) & Strings.ChrW(10) &
                       "WHERE  GPL.gis_policy_link_id = " & CStr(m_lPolicyLinkId) & Strings.ChrW(13) & Strings.ChrW(10)

                sSQL = sSQL & "AND INSF.insurance_folder_cnt = GPL.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPartyCnt", bStoredProcedure:=bStoredProcedure, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vArray) Then
                m_lPartyCnt = CInt(vArray(0, 0)) ' To store for future purpose
            End If
        Else
            GetBasicClaimInformation()
        End If

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".GetPartyCnt")

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: GetPartyCntFromShortName
    '
    ' Description: Derives PartyCnt from Shortname
    '
    ' History: 21/07/2006 MKW - Created.
    '
    ' ***************************************************************** '
    Public Function GetPartyCntFromShortName(ByRef v_sShortName As Object, ByRef v_lPartyCnt As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT party_cnt from party where Shortname='" & gPMFunctions.ToSafeString(v_sShortName, "") & "'"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPartyCntFromShortName", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vArray) Then


                v_lPartyCnt = vArray(0, 0) ' To store for future purpose
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyCntFromShortName Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="GetPartyCntFromShortName", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name          : GetClaimInformationDetail
    '
    ' Description   : Function to fetch Claims Infomation Details
    '
    ' Author        : Ram Chandrabose
    '
    ' Reference     : NRMA Project Process No : 204
    '
    ' Note          : 1. This function is called at renewal time by Product Builder
    '                    Renewal Script
    '                 2. Uses a new "spu_get_claim_info_detail" stored procedure
    ' Edit History  :
    ' RAM20021120   : Created
    ' ***************************************************************** '
    Public Function GetClaimInformationDetail(ByRef r_oData As Object, Optional ByRef r_oClaimsPerils As Object = Nothing) As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "GetClaimInformationDetail"

        Dim oClaimPerilReserves As Object
        Dim oClaimPerilRecoveries As Object

        Try

            nResult = PMEReturnCode.PMTrue

            m_lReturn = GetInsuranceFileCntForClaim()

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            ' Check if we got a valid Insurance File Cnt i.e > 0.
            If m_lInsuranceFileCnt < 1 Then
                Return PMEReturnCode.PMFalse
            End If



            If Convert.IsDBNull(m_vClaimYearToCheck) Or Informations.IsNothing(m_vClaimYearToCheck) Or Object.Equals(m_vClaimYearToCheck, Nothing) Then
                m_lReturn = GetClaimCnt()

                If m_lReturn <> PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "GetClaimCnt Failed", PMELogLevel.PMLogError)
                End If
            End If

            AddParameterLite(m_oDatabase, "ID", m_lInsuranceFileCnt, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)
            AddParameterLite(m_oDatabase, "claim_cnt", m_lClaimCnt, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "claim_year_to_check", m_vClaimYearToCheck, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

            nResult = m_oDatabase.SQLSelect(sSQL:=ACGetClaimHeaderDetailsSQL, sSQLName:=ACGetClaimHeaderDetailsName, bStoredProcedure:=ACGetClaimHeaderDetailsStored, vResultArray:=m_oClaimDetail_GIS, bKeepNulls:=True)

            If Informations.IsArray(m_oClaimDetail_GIS) Then
                For iClaimCount As Integer = 0 To m_oClaimDetail_GIS.GetUpperBound(1)

                    r_oClaimsPerils = ""
                    'step 3, get the perils
                    AddParameterLite(m_oDatabase, "mode", 3, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)
                    AddParameterLite(m_oDatabase, "ID", m_oClaimDetail_GIS(kCLAIMSDETAILS_ClaimID, iClaimCount), PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
                    AddParameterLite(m_oDatabase, "underwriting", If(m_bUnderwriting, 1, 0), PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

                    nResult = m_oDatabase.SQLSelect(sSQL:=ACGetClaimInfoExSQL, sSQLName:=ACGetClaimInfoExName, bStoredProcedure:=ACGetClaimInfoExStored, vResultArray:=r_oClaimsPerils, bKeepNulls:=True)

                    If Informations.IsArray(r_oClaimsPerils) Then

                        oClaimPerilReserves = ""
                        'WPR 38-39
                        oClaimPerilRecoveries = ""

                        For iPerilCount As Integer = 0 To r_oClaimsPerils.GetUpperBound(1)
                            ' get the reserves for each peril
                            AddParameterLite(m_oDatabase, "claim_peril_id", r_oClaimsPerils(4, CInt(iPerilCount)), PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, PMEReturnCode.PMTrue)
                            AddParameterLite(m_oDatabase, "siriusproduct", "U", PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                            AddParameterLite(m_oDatabase, "insurance_file_cnt", m_lInsuranceFileCnt, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

                            nResult = m_oDatabase.SQLSelect(sSQL:=ACGetReserveAgainstPerilSQL, sSQLName:=ACGetReserveAgainstPerilName, bStoredProcedure:=ACGetReserveAgainstPerilStored, vResultArray:=oClaimPerilReserves, bKeepNulls:=True)

                            If Informations.IsArray(oClaimPerilReserves) Then


                                r_oClaimsPerils.SetValue(oClaimPerilReserves, 3, CInt(iPerilCount))
                            Else


                                r_oClaimsPerils.SetValue(DBNull.Value, 3, CInt(iPerilCount))
                            End If

                            'WPR 38-39
                            AddParameterLite(m_oDatabase, "lPerilID", r_oClaimsPerils(4, CInt(iPerilCount)), PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)

                            GetClaimInformationDetail =
                                m_oDatabase.SQLSelect(sSQL:=kGetClaimRecoveriesSQL,
                                              sSQLName:=kGetClaimRecoveriesName,
                                              bStoredProcedure:=True,
                                              vResultArray:=oClaimPerilRecoveries,
                                              bKeepNulls:=True)

                            If Informations.IsArray(oClaimPerilRecoveries) Then
                                r_oClaimsPerils(6, CInt(iPerilCount)) = oClaimPerilRecoveries
                            Else
                                r_oClaimsPerils(6, CInt(iPerilCount)) = DBNull.Value
                            End If

                        Next

                        m_oClaimDetail_GIS(9, iClaimCount) = r_oClaimsPerils
                    End If
                Next
            End If
            r_oData = DirectCast(m_oClaimDetail_GIS, Object(,)).Clone
            Return nResult

        Catch excep As Exception

            ' We have an error
            nResult = PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="GetClaimInformationDetail Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="GetClaimInformationDetail", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return nResult

        End Try
    End Function
    ''' <summary>
    ''' Function to fetch Claims Payment Infomation Details
    ''' </summary>
    ''' <param name="r_vData"></param>
    ''' <param name="r_vClaimsPerils"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetClaimPaymentAgainstPeril(ByRef r_vData As Object, Optional ByRef r_vClaimsPerils As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimPaymentAgainstPeril"

        Dim vPaymentAgainstPeril As Object

        Try


            result = PMEReturnCode.PMTrue

            m_lReturn = GetInsuranceFileCntForClaim()

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Check if we got a valid Insurance File Cnt i.e > 0.
            If m_lInsuranceFileCnt < 1 Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If



            If Convert.IsDBNull(m_vClaimYearToCheck) Or Informations.IsNothing(m_vClaimYearToCheck) Or Object.Equals(m_vClaimYearToCheck, Nothing) Then
                m_lReturn = GetClaimCnt()

                If m_lReturn <> PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "GetClaimCnt Failed", PMELogLevel.PMLogError)
                End If
            End If

            AddParameterLite(m_oDatabase, "ID", m_lInsuranceFileCnt, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)
            AddParameterLite(m_oDatabase, "claim_cnt", m_lClaimCnt, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "claim_year_to_check", m_vClaimYearToCheck, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

            result = m_oDatabase.SQLSelect(sSQL:=ACGetClaimHeaderDetailsSQL, sSQLName:=ACGetClaimHeaderDetailsName, bStoredProcedure:=ACGetClaimHeaderDetailsStored, vResultArray:=m_oClaimDetail_GIS, bKeepNulls:=True)

            If Informations.IsArray(m_oClaimDetail_GIS) Then
                For iClaimCount As Integer = 0 To m_oClaimDetail_GIS.GetUpperBound(1)

                    r_vClaimsPerils = ""
                    'step 3, get the perils
                    AddParameterLite(m_oDatabase, "mode", 3, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)
                    AddParameterLite(m_oDatabase, "ID", m_oClaimDetail_GIS(kCLAIMSDETAILS_ClaimID, iClaimCount), PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
                    AddParameterLite(m_oDatabase, "underwriting", If(m_bUnderwriting, 1, 0), PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

                    result = m_oDatabase.SQLSelect(sSQL:=ACGetClaimInfoExSQL, sSQLName:=ACGetClaimInfoExName, bStoredProcedure:=ACGetClaimInfoExStored, vResultArray:=r_vClaimsPerils, bKeepNulls:=True)

                    If Informations.IsArray(r_vClaimsPerils) Then

                        vPaymentAgainstPeril = ""


                        For iPerilCount As Integer = 0 To r_vClaimsPerils.GetUpperBound(1)
                            ' get the reserves for each peril with Payment Details
                            AddParameterLite(m_oDatabase, "claim_peril_id", r_vClaimsPerils(4, CInt(iPerilCount)), PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, PMEReturnCode.PMTrue)

                            result = m_oDatabase.SQLSelect(sSQL:=ACGetPaymentAgainstPerilSQL, sSQLName:=ACGetPaymentAgainstPerilName, bStoredProcedure:=ACGetPaymentAgainstPerilStored, vResultArray:=vPaymentAgainstPeril, bKeepNulls:=True)

                            If Informations.IsArray(vPaymentAgainstPeril) Then


                                r_vClaimsPerils.SetValue(vPaymentAgainstPeril, 3, CInt(iPerilCount))
                            Else


                                r_vClaimsPerils.SetValue(DBNull.Value, 3, CInt(iPerilCount))
                            End If
                        Next

                        m_oClaimDetail_GIS(9, iClaimCount) = r_vClaimsPerils
                    End If
                Next
            End If
            r_vData = m_oClaimDetail_GIS.clone()
            Return result
        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=CStr(m_sUsername), v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally


        End Try
        Return result
    End Function

    ''' <summary>
    ''' Function to get insurance_folder_cnt for a give insurance_file_cnt from insurance_file table.
    ''' This function should be called if the Insurance Folder Cnt is less than 1
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetInsuranceFolderCnt(ByVal v_lInsuranceFileCnt As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vData(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        'check if we have already read it
        If m_lInsuranceFolderCnt > 0 Then
            Return result
        End If

        ' Check if we got a valid Insurance File Cnt i.e > 0.

        If CDbl(v_lInsuranceFileCnt) < 1 Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        sSQL = "SELECT insurance_folder_cnt FROM insurance_file WHERE insurance_file_cnt = " & CStr(v_lInsuranceFileCnt)

        ' Execute the SQL
        result = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetInsuranceFolderCnt", bStoredProcedure:=False, vResultArray:=vData)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        If Not Informations.IsArray(vData) Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If


        m_lInsuranceFolderCnt = CInt(vData(0, 0))

        Return result

    End Function



    ' ***************************************************************** '
    ' Name          : ChangeRenewalPremium
    '
    ' Description   : Function to Change the Renewal Premium based on the ReAllowNCD Flag.
    '
    ' Author        : Ram Chandrabose
    '
    ' Reference     : NRMA Project Process No : 204
    '
    ' Note          : 1. This function will be called as a last step in the the Claims Builder
    '                    Validation Script (once all other validation is complete),
    '                    only when the ReAllowNCD is altered.
    '                 2. Uses bSIRRenSelection.dll 's Methods
    '                    a) DeleteRenewalPolicy
    '                    b) GetPolicyForRenewal
    '                 3. Uses bSIRAutomaticRenewalSel.dll 's Method
    '                    a) CreateRenewalPolicy
    '
    ' Edit History  :
    ' RAM20021122   : Created
    ' ***************************************************************** '
    Public Function ChangeRenewalPremium() As Integer

        Dim result As Integer = 0
        Dim lRenewalStatus As Integer
        Dim oSIRRenSelection As bSIRRenSelection.Business
        Dim oSIRAutomaticRenewalSel, vRenewalPolicy As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Step 1 : Fetch the Insurance_File_Cnt
            '          Note : If this is called for Claim call the 'GetInsuranceFileCntForClaim' method
            result = GetInsuranceFileCnt() ' CG20021202 : 1522 Provide alternative to GetBODetails

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Check if we got a valid Insurance File Cnt i.e > 0.
            If m_lInsuranceFileCnt < 1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step 2 : Check whether the Policy is in Renewal Cycle
            result = CheckInRenewal(v_lInsuranceFileCnt:=CInt(m_lInsuranceFileCnt), r_lRenewalStatus:=lRenewalStatus)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Step 3 : Remove the policy from Renewal Cycle, if the policy is in renewal Cycle
            If lRenewalStatus = -1 Then

                ' The Policy is not in Renewal Cycle
                ' So we don't need to do anything
            Else

                ' The policy is in Renewal cycle, so we need to remove it from the
                ' renewal cycle using bSIRRenSelection.dll


                oSIRRenSelection = New bSIRRenSelection.Business
                result = oSIRRenSelection.Initialise(sUsername:=CStr(m_sUsername), sPassword:=CStr(m_sPassword), iUserID:=CInt(m_iUserID), iSourceID:=CInt(m_iSourceID),
                                                     iLanguageID:=CInt(m_iLanguageID), iCurrencyID:=CInt(m_iCurrencyID), iLogLevel:=CInt(m_iLogLevel), sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create bSIRRenSelection.Business Object ", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="ChangeRenewalPremium", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' Check if we have a valid Insurance Folder Cnt
                If m_lInsuranceFolderCnt < 1 Then

                    ' We need to fetch the Insurance Folder Cnt
                    result = GetInsuranceFolderCnt(v_lInsuranceFileCnt:=CInt(m_lInsuranceFileCnt))
                    If result <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Fetch Insurance Folder Cnt ", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="ChangeRenewalPremium", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If
                End If


                result = oSIRRenSelection.DeleteRenewalPolicy(v_lInsuranceFolderCnt:=CInt(m_lInsuranceFileCnt))
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete Renewal Policy ", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="ChangeRenewalPremium", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                'Get correct version of this policy for renewal (This will be returned back as vRenewalPolicy)

                result = oSIRRenSelection.GetPolicyForRenewal(v_lInsuranceFolderCnt:=CInt(m_lInsuranceFileCnt), r_vResultArray:=vRenewalPolicy)

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Get Correct Version Of Policy For Renewal.", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="ChangeRenewalPremium", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' Here after we don't need the oSIRRenSelection Component, so terminate and release it

                oSIRRenSelection.Dispose()

                oSIRRenSelection = Nothing

                ' Create the bSIRAutomaticRenewalSel Component to do the Create Renewal Policy Automatically
                ' Here we assumes that, this bSIRAutomaticRenewalSel.dll will do the renewal Process as the
                '  same way as the iPMURenSelection.dll's frmInterface's CreateRenewalPolicy Method
                result = gPMComponentServices.CreateBusinessObject(oSIRAutomaticRenewalSel, v_sClassName:="bSIRAutomaticRenewalsSel.Business", v_sCallingAppName:=CStr(ACApp), v_sUsername:=CStr(m_sUsername), v_sPassword:=CStr(m_sPassword), v_iUserID:=CInt(m_iUserID), v_iSourceID:=CInt(m_iSourceID), v_iLanguageID:=CInt(m_iLanguageID), v_iCurrencyID:=CInt(m_iCurrencyID), v_iLogLevel:=CInt(m_iLogLevel), v_oDatabase:=m_oDatabase)

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create bSIRRenSelection.Business Object ", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="ChangeRenewalPremium", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' Call the Create Renewal Policy Method


                result = oSIRAutomaticRenewalSel.CreateRenewalPolicy(r_vRenewalList:=vRenewalPolicy, v_lCount:=0)

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Create Renewal Policy.", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="ChangeRenewalPremium", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' Here after we don't need the oSIRAutomaticRenewalSel Component, so terminate and release it

                oSIRAutomaticRenewalSel.Dispose()

                oSIRAutomaticRenewalSel = Nothing

                ' Create an Event of Event Type 6 : Corresponds to event_type table entry
                ' Which is the 'CLACHANGE' -  Change Of Claim Detail

                ' Note : m_lClaimCnt is not set at this point of time, since we don't have it
                '           If we have that information, then it can be set it.
                result = CreateEvent(vPartyCnt:=m_lPartyCnt, vInsuranceFolderCnt:=m_lInsuranceFolderCnt, vInsuranceFileCnt:=CInt(m_lInsuranceFileCnt), vClaimCnt:=m_lClaimCnt, vEventTypeCode:="CLACHANGE", vDescription:="NCD allowed/disallowed/re-allowed")

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Get Create an Event.", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="ChangeRenewalPremium", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

            End If


            Return result

        Catch excep As System.Exception



            ' We have an error
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ChangeRenewalPremium Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="ChangeRenewalPremium", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name          : CheckInRenewal
    '
    ' Description   : Function to Check whether the Policy is in Renewal Cycle
    '
    ' Author        : Ram Chandrabose
    '
    ' Reference     : NRMA Project Process No : 204
    '
    ' Note          : 1. If the Policy is in Renewal Cycle, then the return value
    '                    will NOT BE EQUAL TO -1
    '                 2. This function is taken from bSIRFindInsurance.dll's Form Class
    '
    ' Edit History  :
    ' RAM20021122   : Created
    ' ***************************************************************** '
    Public Function CheckInRenewal(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lRenewalStatus As Integer) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_lRenewalStatus = -1

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckRenewalsSQL, sSQLName:=ACCheckRenewalsName, bStoredProcedure:=ACCheckRenewalsStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vArray) Then
                Return result
            End If


            r_lRenewalStatus = CInt(vArray(0, 0))

            vArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckInRenewal Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="CheckInRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name          : CreateEvent
    '
    ' Description   : Function to add record to event_log Table
    '
    ' Author        : Ram Chandrabose
    '
    ' Reference     : NRMA Project Process No : 204
    '
    ' Note          : This uses bSIREvent.dll
    '
    ' Edit History  :
    ' RAM20021125   : Created
    ' ***************************************************************** '
    Private Function CreateEvent(Optional ByRef vEventCnt As Object = Nothing, Optional ByVal vPartyCnt As Object = Nothing, Optional ByVal vInsuranceFolderCnt As Object = Nothing, Optional ByVal vInsuranceFileCnt As Object = Nothing, Optional ByVal vClaimCnt As Object = Nothing, Optional ByVal vEventTypeID As Object = Nothing, Optional ByVal vEventTypeCode As Object = Nothing, Optional ByVal vDescription As Object = Nothing, Optional ByVal vUserID As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim oEvent As bSIREvent.Business



        result = gPMConstants.PMEReturnCode.PMTrue


        oEvent = New bSIREvent.Business
        result = oEvent.Initialise(sUsername:=CStr(m_sUsername), sPassword:=CStr(m_sPassword), iUserID:=CInt(m_iUserID), iSourceID:=CInt(m_iSourceID), iLanguageID:=CInt(m_iLanguageID), iCurrencyID:=CInt(m_iCurrencyID), iLogLevel:=CInt(m_iLogLevel), sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            'Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSIREvent.Business Component.", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Add an entry in the event log using the DirectAdd method

        result = oEvent.DirectAdd(vEventCnt:=vEventCnt, vPartyCnt:=vPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=vInsuranceFileCnt, vClaimCnt:=vClaimCnt, vEventType:=vEventTypeID, vEventTypeCode:=vEventTypeCode, vDescription:=vDescription, vUserId:=vUserID)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            'Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add an event log.", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        'Terminate the object and clear it up

        oEvent.Dispose()

        oEvent = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name          : GetGISDataModelType
    '
    ' Description   : Function to get the GIS Data Model Type ID for a
    '                   given data model code
    '
    ' Author        : Ram Chandrabose
    '
    ' Reference     : NRMA Project Process No : 204
    '
    ' Edit History  :
    ' RAM20021202   : Created
    ' ***************************************************************** '
    Private Function GetGISDataModelType(ByVal v_sGISDataModelCode As String, ByRef r_lGISDataModelTypeID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vData(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Check if we got a valid Data Model Code
        If v_sGISDataModelCode.Trim().Length = 0 Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        v_sGISDataModelCode = v_sGISDataModelCode.Trim().ToUpper()

        sSQL = "SELECT gis_data_model_type_id From GIS_Data_Model where RTRIM(code) = '" & v_sGISDataModelCode & "'"

        ' Execute the SQL
        result = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetGISDataModelTypeID", bStoredProcedure:=False, vResultArray:=vData)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        If Not Informations.IsArray(vData) Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If


        r_lGISDataModelTypeID = CInt(vData(0, 0))

        Return result

    End Function

    ' ***************************************************************** '
    ' Name          : GetInsuranceFileCntForClaim
    '
    ' Description   : Function to get the Insurance File Cnt for a
    '                   Data Model of Type CLAIM
    '
    ' Author        : Ram Chandrabose
    '
    ' Reference     : NRMA Project Process No : 204
    '
    ' Edit History  :
    ' RAM20021202   : Created
    ' ***************************************************************** '
    Private Function GetInsuranceFileCntForClaim() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue


        m_oDatabase.Parameters.Clear()

        result = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(m_lPolicyLinkId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        result = m_oDatabase.SQLSelect(sSQL:=ACGetInsuranceFileCntForClaimDMSQL, sSQLName:=ACGetInsuranceFileCntForClaimDMName, bStoredProcedure:=ACGetInsuranceFileCntForClaimDMStored, vResultArray:=vResultArray)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        If Not Informations.IsArray(vResultArray) Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If


        m_lInsuranceFileCnt = CInt(vResultArray(0, 0))

        Return result

    End Function

    ''' <summary>
    ''' Calls a stored procedure named from tha passed ICCS code and qualiyfing name
    ''' </summary>
    ''' <param name="v_lIccsCode"></param>
    ''' <param name="v_sSpDescription"></param>
    ''' <param name="r_vResults"></param>
    ''' <param name="v_vExtraParameters"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CallNamedStoredProcedure(ByVal v_lIccsCode As Integer, ByVal v_sSpDescription As String, ByRef r_vResults As Object, Optional ByRef v_vExtraParameters As Object = Nothing) As Integer

        '    a) Insurance_file_cnt,
        '    b) Insurance_folder_cnt
        '    c) risk_cnt

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".CallNamedStoredProcedure")

        Dim sSQL, sAdditionalErrorMessage As String
        Try

            'AK 160403
            'add Insurance File Cnt
            If m_lInsuranceFileCnt = 0 Then
                GetInsuranceFileCnt()
            End If


            m_lReturn = PMEReturnCode.PMTrue

            sSQL = "spu_ICCS_" & v_lIccsCode & "_" & v_sSpDescription & "" 'build sp name

            'add party cnt
            If GetPartyCnt() = gPMConstants.PMEReturnCode.PMFalse Then
                m_lPartyCnt = -1 'signal sp that party cnt is not valid
            End If
            'add Insurance File Cnt
            If Not m_lInsuranceFileCnt > 0 Then
                GetInsuranceFileCnt()
            End If

            'add Insurance Folder Cnt
            If Not m_lInsuranceFolderCnt > 0 Then
                GetInsuranceFolderCnt(m_lInsuranceFileCnt)
            End If

            m_oDatabase.Parameters.Clear()
            AddParameter(m_oDatabase, sSQL, m_lReturn, "lPartyCnt", m_lPartyCnt, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, v_bIgnoreIfBlank:=False)

            'add policylink id
            AddParameter(m_oDatabase, sSQL, m_lReturn, "lPolicyLinkId", m_lPolicyLinkId, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, v_bIgnoreIfBlank:=False)

            'add policy binder id
            AddParameter(m_oDatabase, sSQL, m_lReturn, "lPolicyBinderId", m_lPolicyBinderId, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, v_bIgnoreIfBlank:=False)

            'AK 160403 this code is being moved to biginning of function, as it erased the parameter list
            ''add Insurance File Cnt
            'GetInsuranceFileCnt
            AddParameter(m_oDatabase, sSQL, m_lReturn, "lInsuranceFileCnt", m_lInsuranceFileCnt, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, v_bIgnoreIfBlank:=False)

            AddParameter(m_oDatabase, sSQL, m_lReturn, "lInsuranceFolderCnt", m_lInsuranceFolderCnt, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, v_bIgnoreIfBlank:=False)

            'now add any extra parameters passed to the call

            If Not Informations.IsNothing(v_vExtraParameters) Then
                If Informations.IsArray(v_vExtraParameters) Then
                    For lArrayPointer As Integer = 0 To v_vExtraParameters.GetUpperBound(0)
                        sAdditionalErrorMessage = "Error adding additional parameter " & lArrayPointer / 2 + 1

                        Select Case CStr(v_vExtraParameters(lArrayPointer)).Substring(0, 1).ToLower()
                            Case "s"

                                AddParameter(m_oDatabase, sSQL, m_lReturn, CStr(v_vExtraParameters(lArrayPointer)), v_vExtraParameters(lArrayPointer + 1), PMEParameterDirection.PMParamInput, PMEDataType.PMString, v_bIgnoreIfBlank:=False)
                            Case "l"

                                AddParameter(m_oDatabase, sSQL, m_lReturn, CStr(v_vExtraParameters(lArrayPointer)), v_vExtraParameters(lArrayPointer + 1), PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, v_bIgnoreIfBlank:=False)
                            Case "d"

                                AddParameter(m_oDatabase, sSQL, m_lReturn, CStr(v_vExtraParameters(lArrayPointer)), v_vExtraParameters(lArrayPointer + 1), PMEParameterDirection.PMParamInput, PMEDataType.PMDate, v_bIgnoreIfBlank:=False)
                            Case "b"

                                AddParameter(m_oDatabase, sSQL, m_lReturn, CStr(v_vExtraParameters(lArrayPointer)), v_vExtraParameters(lArrayPointer + 1), PMEParameterDirection.PMParamInput, PMEDataType.PMBoolean, v_bIgnoreIfBlank:=False)
                            Case "f"

                                AddParameter(m_oDatabase, sSQL, m_lReturn, CStr(v_vExtraParameters(lArrayPointer)), v_vExtraParameters(lArrayPointer + 1), PMEParameterDirection.PMParamInput, PMEDataType.PMDouble, v_bIgnoreIfBlank:=False)
                            Case Else

                                Throw New Exception("513, " + +", " + "unknown type (" & CStr(v_vExtraParameters(lArrayPointer)) & ") passed as a parameter")
                                '                        Err.Raise vbObjectError + 1
                        End Select
                        lArrayPointer += 1 'step over the data as we've already used it
                    Next
                End If
            End If
            sAdditionalErrorMessage = ""

            'call the sp
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=sSQL, bStoredProcedure:=True, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResults)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception((Constants.vbObjectError + 1).ToString() + ", " + +", " + "SQL Select returned " & m_lReturn)
            End If

            result = PMEReturnCode.PMTrue

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".CallNamedStoredProcedure")

            Return result

        Catch excep As Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".CallNamedStoredProcedure")

            result = PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="CallNamedStoredProcedure Failed (" & sSQL & ")" & " " & sAdditionalErrorMessage, vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="CallNamedStoredProcedure", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddEvent
    '
    ' Description:
    '
    ' History: 10/12/2002 sj - Created.
    '
    ' ***************************************************************** '
    Public Function AddEvent() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lPartyCnt = 0 Or m_lInsuranceFolderCnt = 0 Or m_lInsuranceFileCnt = 0 Then
                m_lReturn = GetInsuranceFileCnt()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsuranceFileCnt Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="AddEvent")
                    Return result
                End If
            End If

            'Create the event
            m_lReturn = CreateEvent(vPartyCnt:=m_lPartyCnt, vInsuranceFolderCnt:=m_lInsuranceFolderCnt, vInsuranceFileCnt:=CInt(m_lInsuranceFileCnt), vEventTypeCode:=m_sEventTypeCode, vDescription:=m_sEventDescription, vUserID:=CInt(m_iUserID))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Create Event Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="AddEvent")
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddEvent Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="AddEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name          : NoofClaimsLodgedAfterCancelDate
    '
    ' Description   : Function to Return the total no of claims that are lodged
    '                  (for a given insurance_folder_cnt and effective_date)
    '
    ' Author        : Ram Chandrabose
    '
    ' Reference     : NRMA Project Process No : 426
    '
    ' Note          : uses new 'spu_check_claims_lodged'  stored procedure
    '
    ' Edit History  :
    ' RAM20030203   : Created
    ' ***************************************************************** '
    Public Function NoofClaimsLodgedAfterCancelDate() As Integer

        Dim result As Integer = 0
        Dim lNumberRecords As Integer

        Try

            result = -1 ' Initialise

            ' Check if the m_lInsuranceFolderCnt and m_dtCancellationDate are set

            ' First check the insurance folder, if it is not supplied, get it using the
            ' existing method
            If m_lInsuranceFolderCnt < 1 Then
                'get insurance folder count
                m_lReturn = GetInsuranceFileCnt() ' Will fetch the Insurance Folder as well
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If
            End If

            ' Check for the cancellation date
            If Not Informations.IsDate(m_dtCancellationDate) Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Invalid Policy Cancellation Date.", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="NoofClaimsLodgedAfterCancelDate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            With m_oDatabase

                .Parameters.Clear()

                ' Set the Input Parameters

                ' Insurance Folder Cnt
                m_lReturn = .Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(m_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                ' Effective Date or Cancellation Date
                m_lReturn = .Parameters.Add(sName:="effective_date", vValue:=m_dtCancellationDate.ToString, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                ' NoofClaims (Output parameter)

                'developer guide no 85. 
                m_lReturn = .Parameters.Add(sName:="NoofClaims", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                ' Execute the SQL to fetch the data
                m_lReturn = .SQLAction(sSQL:=ACGetNoofClaimsLodgedSQL, sSQLName:=ACGetNoofClaimsLodgedName, bStoredProcedure:=ACGetNoofClaimsLodgedStored, lRecordsAffected:=lNumberRecords)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                ' Return the No of Claims Lodged
                result = .Parameters.Item("NoofClaims").Value

            End With

            Return result

        Catch excep As System.Exception



            ' We have an error
            result = -1

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NoofClaimsLodgedAfterCancelDate Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="NoofClaimsLodgedAfterCancelDate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
            Return result
        End Try
    End Function

    'sj 04/03/2003 - start
    'ISS2644
    ' ***************************************************************** '
    ' Name          : GetPreviousRenewalReasons
    '
    ' Description   : Check to see if work manager task required
    '               ' Only called at renewal
    ' Author        : Steve James
    '
    ' Edit History  :
    ' HG02062003 - Changed function name, and returned data.
    ' ***************************************************************** '
    Private Function GetPreviousRenewalReasons() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        result = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(m_lPolicyLinkId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        result = m_oDatabase.SQLSelect(sSQL:=ACGetRenewalResonsSQL, sSQLName:=ACGetRenewalResonsName, bStoredProcedure:=ACGetRenewalResonsStored, vResultArray:=vResultArray)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If



        m_vRenwalFailureReasons = vResultArray

        Return result

    End Function

    'sj 04/03/2003 - start
    'ISS2644
    ' ***************************************************************** '
    ' Name          : CheckIfWorkManagerTaskRequired
    '
    ' Description   : Check to see if work manager task required
    '               ' Only called at renewal
    ' Author        : Steve James
    '
    ' Edit History  :
    ' HG02062003    : Originally this function checked the return data
    '                 from the stored procedure to check if it was the last
    '                 risk. datetime.now we just back back a flag to derermine if its
    '                 the last risk.
    ' ***************************************************************** '
    Private Function CheckIfWorkManagerTaskRequired() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(m_lPolicyLinkId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.Parameters.Add(sName:="ReturnStatus", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACCheckIfWorkManagerTaskRequiredSQL, sSQLName:=ACCheckIfWorkManagerTaskRequiredName, bStoredProcedure:=ACCheckIfWorkManagerTaskRequiredStored)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            lReturn = CType(m_oDatabase.Parameters.Item("ReturnStatus").Value, gPMConstants.PMEReturnCode)



            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMTrue
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch
        End Try



        ' We have an error
        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckIfWorkManagerTaskRequired Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="CheckIfWorkManagerTaskRequired", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function

    'sj 04/03/2003 - end


    ' ***************************************************************** '
    '
    ' Name: GetRiskTypeDetails
    '
    ' Description:
    '
    ' History: 13/03/2003 Chris Ridgard - Created.
    '
    ' ***************************************************************** '
    Private Function GetRiskTypeDetails() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue


        m_oDatabase.Parameters.Clear()

        result = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(m_lPolicyLinkId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        result = m_oDatabase.Parameters.Add(sName:="transaction_type", vValue:=m_sTransactionType.Trim(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If
        result = m_oDatabase.SQLSelect(sSQL:=ACRiskTypeSQL, sSQLName:=ACRiskTypeName, bStoredProcedure:=ACRiskTypeStored, vResultArray:=vResultArray)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        If Not Informations.IsArray(vResultArray) Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        Else

            m_sRiskTypeCode = CStr(vResultArray(1, 0))

            m_sRiskTypeDescription = CStr(vResultArray(2, 0))
            m_bGotRiskTypeDetails = True
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: AddKeys
    '
    ' Description: Allows the user to define a key array for
    '              work manager tasks
    '
    ' History: 29/05/2003 - HG Created
    '
    ' ***************************************************************** '
    Public Function TaskAddKeys(ByVal sKeyName As String, ByVal vKeyValue As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If sKeyName.Trim() = "" Then
                m_sTaskStatus = "No valid keyname supplied"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(m_vKeyArray) Then
                ReDim Preserve m_vKeyArray(1, m_vKeyArray.GetUpperBound(1) + 1)

                ' Assign Values
                m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, m_vKeyArray.GetUpperBound(1)) = sKeyName

                m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, m_vKeyArray.GetUpperBound(1)) = vKeyValue
            Else
                ' Create a new 2 dimensional array
                ReDim m_vKeyArray(1, 0)

                ' Assign Values
                m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = sKeyName

                m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = vKeyValue

            End If

            Return result

        Catch excep As System.Exception



            ' We have an error
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TaskAddKeys Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="TaskAddKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ClearKeys
    '
    ' Description: Allows the user to clear any keys they have set. Used
    '              to raise work manager tasks with pmwrk_task_inst_key's
    '
    ' History: 29/05/2003 - HG Created
    '
    ' ***************************************************************** '
    Public Function TaskClearKeys() As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Erase m_vKeyArray

            ' Ensure that the variant sub type changes (other wise Informations.IsArray() will still pick it up as an array)
            'developer guide no. 28 (no solution)
            'm_vKeyArray = VB6.CopyArray("")

            Return result

        Catch excep As System.Exception



            ' We have an error
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TaskClearKeys Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="TaskClearKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function CurrentCurrencyISOCode() As String
        m_lReturn = CreatePBRiskPolicyCurrency()

        Return m_oPBRiskPolicyCurrency.CurrentCurrencyISOCode
    End Function

    Public Function CurrentCurrencyName() As String
        m_lReturn = CreatePBRiskPolicyCurrency()

        Return m_oPBRiskPolicyCurrency.CurrentCurrencyName
    End Function

    Public Function CurrentCurrencySymbol() As String
        m_lReturn = CreatePBRiskPolicyCurrency()

        Return m_oPBRiskPolicyCurrency.CurrentCurrencySymbol
    End Function

    Public Function PreviousCurrencyISOCode() As String
        m_lReturn = CreatePBRiskPolicyCurrency()

        Return m_oPBRiskPolicyCurrency.PreviousCurrencyISOCode
    End Function

    Public Function PreviousCurrencyName() As String
        m_lReturn = CreatePBRiskPolicyCurrency()

        Return m_oPBRiskPolicyCurrency.PreviousCurrencyName
    End Function

    Public Function PreviousCurrencySymbol() As String
        m_lReturn = CreatePBRiskPolicyCurrency()

        Return m_oPBRiskPolicyCurrency.PreviousCurrencySymbol
    End Function

    Public Sub ConvertToCurrentPolicyCurrency(ByVal v_vOldAmount As Object, ByRef r_vNewAmount As Object)

        Dim cOldAmount, cNewAmount As Decimal

        m_lReturn = CreatePBRiskPolicyCurrency()


        Dim dbNumericTemp As Double

        If Not (Convert.IsDBNull(v_vOldAmount) Or Informations.IsNothing(v_vOldAmount)) And Double.TryParse(CStr(v_vOldAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

            cOldAmount = CDec(v_vOldAmount)


            m_lReturn = m_oPBRiskPolicyCurrency.ConvertToCurrentPolicyCurrency(v_cOldAmount:=cOldAmount, r_cNewAmount:=cNewAmount)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_vNewAmount = 0
                Exit Sub
            End If

            r_vNewAmount = cNewAmount
        Else
            r_vNewAmount = 0
        End If

    End Sub

    Public Sub ConvertBetweenCurrencies(ByVal v_vOldCurrencyISOCode As Object, ByVal v_vOldAmount As Object, ByVal v_vNewCurrencyISOCode As Object, ByRef r_vNewAmount As Object)

        Dim cOldAmount, cNewAmount As Decimal

        m_lReturn = CreatePBRiskPolicyCurrency()


        Dim dbNumericTemp As Double

        If Not (Convert.IsDBNull(v_vOldAmount) Or Informations.IsNothing(v_vOldAmount)) And Double.TryParse(CStr(v_vOldAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then


            cOldAmount = CDec(v_vOldAmount)




            m_lReturn = m_oPBRiskPolicyCurrency.ConvertBetweenCurrencies(v_sOldCurrencyISOCode:=CStr(v_vOldCurrencyISOCode), v_cOldAmount:=cOldAmount, v_sNewCurrencyISOCode:=CStr(v_vNewCurrencyISOCode), r_cNewAmount:=cNewAmount)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_vNewAmount = 0
                Exit Sub
            End If

            r_vNewAmount = cNewAmount

        Else
            r_vNewAmount = 0
        End If
    End Sub

    Private Function CreatePBRiskPolicyCurrency() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        If m_oPBRiskPolicyCurrency Is Nothing Then


            m_oPBRiskPolicyCurrency = New bGISPMUExtras.PBRiskPolicyCurrency
            m_lReturn = m_oPBRiskPolicyCurrency.Initialise(sUserName:=CStr(m_sUsername), sPassword:=CStr(m_sPassword), iUserID:=CInt(m_iUserID), iSourceID:=CInt(m_iSourceID), iLanguageID:=CInt(m_iLanguageID), iCurrencyID:=CInt(m_iCurrencyID), iLogLevel:=CInt(m_iLogLevel), sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lInsuranceFileCnt < 1 Then
                m_lReturn = GetInsuranceFileCnt()
                If GetInsuranceFileDetails() <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If m_lInsuranceFileCnt < 1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oPBRiskPolicyCurrency.InsuranceFileCnt = m_lInsuranceFileCnt

        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name:         LinkPerilClaim (why not LinkClaimPeril to be consistent with others?)
    '                               oh well Ian, just follow the spec...
    '
    ' Description:  Link claim peril record
    '
    ' Parameters:   ClaimPerilId - Identity int field
    '               Description  - (optional, string)
    '               Comments     - (optional, string)
    '
    ' History:      Created : IRose : 25-06-2003 : 205
    '
    ' ***************************************************************** '
    Public Function LinkPerilClaim(ByRef r_lPerilPartyId As Integer, ByVal v_lClaimId As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lClaimPerilId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "LinkPerilClaim"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Database Parameters again
            m_oDatabase.Parameters.Clear()

            ' Add stored proc input params
            AddClaimPerilParam("ClaimId", gPMConstants.PMEDataType.PMLong, v_lClaimId)
            AddClaimPerilParam("PartyCnt", gPMConstants.PMEDataType.PMLong, v_lPartyCnt)
            AddClaimPerilParam("ClaimPerilId", gPMConstants.PMEDataType.PMLong, v_lClaimPerilId)

            ' Add output parameter to return the new claim debt id
            If m_oDatabase.Parameters.Add(sName:="PerilPartyId", vValue:=CStr(r_lPerilPartyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.LogMessageToFile(sUsername:=CStr(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:=sFunctionName)
            End If
            If m_oDatabase.Parameters.Add(sName:="PerilPartyId", vValue:=CStr(r_lPerilPartyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("r_lPerilPartyId", r_lPerilPartyId)
                oDict.Add("v_lClaimId", v_lClaimId)
                oDict.Add("v_lPartyCnt", v_lPartyCnt)
                oDict.Add("v_lClaimPerilId", v_lClaimPerilId)
                gPMFunctions.LogMessageToFile(sUsername:=CStr(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Informations.Err().Description), oDicParms:=oDict)
            End If

            ' Execute Action Query
            If m_oDatabase.SQLAction(sSQL:=ACLinkPerilClaimSQL, sSQLName:=ACLinkPerilClaimName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("r_lPerilPartyId", r_lPerilPartyId)
                oDict.Add("v_lClaimId", v_lClaimId)
                oDict.Add("v_lPartyCnt", v_lPartyCnt)
                oDict.Add("v_lClaimPerilId", v_lClaimPerilId)
                gPMFunctions.LogMessageToFile(sUsername:=CStr(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to link claim peril", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Informations.Err().Description), oDicParms:=oDict)
            Else
                ' return the peril party id
                r_lPerilPartyId = m_oDatabase.Parameters.Item("PerilPartyId").Value
            End If
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("r_lPerilPartyId", r_lPerilPartyId)
            oDict.Add("v_lClaimId", v_lClaimId)
            oDict.Add("v_lPartyCnt", v_lPartyCnt)
            oDict.Add("v_lClaimPerilId", v_lClaimPerilId)
            gPMFunctions.LogMessageToFile(sUsername:=CStr(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         GetClaimPeril
    ' Description:  Selects a claim peril record and associated data
    '
    ' Parameters:   ClaimId         - ClaimId needed for query
    '               ClaimPerilArray - variant array for returning query results
    '
    ' History:      Created : IRose : 25-06-2003 : 205
    '
    ' ***************************************************************** '
    Public Function GetClaimPeril(ByVal v_lClaimId As Integer, ByRef v_vClaimPerilArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetClaimPeril"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add stored proc input params
            AddClaimPerilParam("ClaimId", gPMConstants.PMEDataType.PMLong, v_lClaimId)

            ' Execute Proc
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClaimPerilDataSQL, sSQLName:=ACGetClaimPerilDataName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=v_vClaimPerilArray)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then result = gPMConstants.PMEReturnCode.PMFalse
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lClaimId", v_lClaimId)
            gPMFunctions.LogMessageToFile(sUsername:=CStr(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         AddClaimPerilParam
    ' Description:  Adds stored proc input parameters for claim peril procs
    '
    ' Parameters:   ParamName   - name of param
    '               PMDataType  - type of param
    '               Value       - value of param
    '
    ' History:      Created : IRose : 24-06-2003 : 205
    '
    ' ***************************************************************** '
    Private Sub AddClaimPerilParam(ByVal v_sParamName As String, ByVal v_lPMEDataType As Integer, ByVal v_vValue As Object)

        Const sFunctionName As String = "AddClaimPerilParam"



        ' Add input parameters for each stored proc paremeter
        If m_oDatabase.Parameters.Add(sName:=v_sParamName, vValue:=CStr(v_vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_lPMEDataType) <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.LogMessageToFile(sUsername:=CStr(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Informations.Err().Description))
        End If

    End Sub
    ' ***************************************************************** '
    ' Name: GetABICodeFromDescription
    '
    ' Description:  Gets the ABI code for a GIS list given the entry description
    '
    ' Return values  PMTrue  r_vABICode contains ABI code
    '                PMFalse Internal non specific error including "invalid list id" etc
    '                PMNotFound  Could not find an entry for list v_sListId matching description v_sDescription
    '
    ' History: CLG 20042306 created
    ' ***************************************************************** '
    Public Function GetABICodeFromDescription(ByVal v_sListId As String, ByVal v_sDescription As String, ByRef r_vABICode As Object) As Integer



        Dim result As Integer = 0
        Static bIGisLoaded As Boolean
        Dim sABICode As String = ""

        result = gPMConstants.PMEReturnCode.PMFalse

        Try


            If Not bIGisLoaded Then
                m_obGISListManager = New bGISListManager.InterfaceNoLogin()
                m_obGISListManager.Initialise()
                result = m_obGISListManager.CheckListVersions(v_sGISDataModelCode:=CStr(m_sGISDataModel))
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_obGISListManager = Nothing
                    Return result
                End If
                bIGisLoaded = True
            End If

            If bIGisLoaded Then
                sABICode = r_vABICode
                result = m_obGISListManager.GetABICodeFromDescription(CStr(v_sListId), CStr(v_sDescription), CStr(sABICode))
                r_vABICode = sABICode
            End If
            Return result

        Catch excep As System.Exception
            ' We have an error
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetABICodeFromDescription Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="GetABICodeFromDescription", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetABIListIdOfProperty
    '
    ' Description:  gets the ABI (GIS) list id for the specified property
    '
    ' Return values  PMTrue  r_vABIListId contains list id
    '                PMFalse Internal non specific error
    '                PMNotFound  object.property is not bound to an ABI list
    '                PMInvalidRequest    object.property name could not be found
    '
    ' History: CLG 20042306 created
    ' ***************************************************************** '

    Public Function GetABIListIdOfProperty(ByRef r_oDataset As Object, ByVal v_sObjectname As String, ByVal v_sPropertyName As String, ByRef r_vABIListId As Object) As Integer


        Dim result As Integer = 0
        Dim lGISListID As Integer


        result = r_oDataset.GetPropertyDefDetails(v_sObjectname:=CStr(v_sObjectname), v_sPropertyName:=CStr(v_sPropertyName), r_lGISListID:=CInt(lGISListID))
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If
        If lGISListID = -1 Then
            result = gPMConstants.PMEReturnCode.PMNotFound
        End If
        r_vABIListId = lGISListID

        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetFullAddress
    '
    ' Description: Get details from DB.
    '
    ' ***************************************************************** '
    Public Function GetFullAddress(ByRef lAddressCnt As Object) As Object


        Dim result() As Object = Nothing
        Try

            Dim aFullAddress, vArray(,) As Object
            Dim sSQL As String = ""


            If String.IsNullOrEmpty(lAddressCnt) Then
                Return result
            End If

            Dim dbNumericTemp As Double
            If Not Double.TryParse(lAddressCnt, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return result
            End If

            sSQL = "select a.address1,a.address2,a.address3,a.address4,a.postal_code, " &
                   "c.country_id,c.code,c.currency_id,c.iso_code,c.telephone_code, " &
                   "pmc.caption,c.postcode_visibility_id " &
                   "from address a " &
                   "inner join country c on a.country_id=c.country_id " &
                   "inner join pmcaption pmc on c.caption_id=pmc.caption_id " &
                   "where a.address_cnt=" & lAddressCnt

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetFullAddress", bStoredProcedure:=gPMConstants.PMEReturnCode.PMFalse, lNumberRecords:=1, vResultArray:=vArray)

            'developer guide no. 28 (no solution)

            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '	Return VB6.CopyArray(gPMConstants.PMEReturnCode.PMFalse)
            'End If

            If Informations.IsArray(vArray) Then



                Dim auxVar_5 As Object = vArray(4, 0)


                Dim auxVar_4 As Object = vArray(3, 0)
                Dim auxVar_3 As Object = vArray(2, 0)
                Dim auxVar_2 As Object = vArray(1, 0)
                Dim auxVar As Object = vArray(0, 0)

                aFullAddress = New Object() {If(Not (Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar)), CStr(vArray(0, 0)).Trim(), ""), If(Not (Convert.IsDBNull(auxVar_2) Or Informations.IsNothing(auxVar_2)), CStr(vArray(1, 0)).Trim(), ""), If(Not (Convert.IsDBNull(auxVar_3) Or Informations.IsNothing(auxVar_3)), CStr(vArray(2, 0)).Trim(), ""), If(Not (Convert.IsDBNull(auxVar_4) Or Informations.IsNothing(auxVar_4)), CStr(vArray(3, 0)).Trim(), ""), If(Not (Convert.IsDBNull(auxVar_5) Or Informations.IsNothing(auxVar_5)), CStr(vArray(4, 0)).Trim(), ""), New Object() {vArray(4 + 1, 0), CStr(vArray(4 + 2, 0)).Trim(), vArray(4 + 3, 0), vArray(4 + 4, 0), vArray(4 + 5, 0), vArray(4 + 6, 0), vArray(4 + 7, 0)}}


                result = aFullAddress
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetFullAddress Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="GetFullAddress", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: LoadRiskDataEngine
    '
    ' Description:  Load a data engine with the policy risk details for this claim
    '               This data is then exposed as Engine.RiskDataEngine
    '               Called from Public Property Get RiskDataEngine
    '
    ' History: CLG 20052005 created
    ' ***************************************************************** '

    Private Function LoadRiskDataEngine() As Integer
        Dim oGIS As bGIS.Application
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sXMLDataset, sXMLDataSetDef, sSQL As String
        Dim vArray(,) As Object
        Dim sMsg As String = ""
        Dim vRiskArray(,) As Object

        Try

            'get insurance_file id and data model code for the risk
            '(work_claim)gis_policy_link -> work_claim -> (claim)insurance_file -> (risk)gis_policy_link* -> gis_data_model*
            sSQL = "select distinct risk_gpl.insurance_file_cnt , risk_gdm.code " &
                   "from gis_policy_link claim_gpl " &
                   "inner join claim wc on wc.claim_id=claim_gpl.claim_id " &
                   "inner join insurance_file claim_insf on claim_insf.insurance_file_cnt=wc.Policy_id " &
                   "inner join gis_policy_link risk_gpl on risk_gpl.insurance_file_cnt=claim_insf.insurance_folder_cnt " &
                   "inner join gis_data_model risk_gdm on risk_gdm.gis_data_model_id= risk_gpl.gis_data_model_id  " &
                   "Where claim_gpl.gis_policy_link_id=" & CStr(m_lPolicyLinkId)

            lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetRiskGplFromClaim", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Informations.IsArray(vArray) Then
                sMsg = "Could not get policy id and/or data model code"
                Throw New Exception()
            End If

            sMsg = "Could not load bGIS.Application"
            oGIS = New bGIS.Application()
            If oGIS Is Nothing Then
                Throw New Exception()
            End If

            sMsg = "Could not initialise bGIS.Application"
            If oGIS.Initialise(sUserName:=CStr(m_sUsername), sPassword:=CStr(m_sPassword), iUserID:=CInt(m_iUserID), iSourceID:=CInt(m_iSourceID), iLanguageID:=CInt(m_iLanguageID), iCurrencyID:=CInt(m_iCurrencyID), iLogLevel:=CInt(m_iLogLevel), sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            lReturn = CType(GetRiskClaim(m_lPolicyLinkId, vRiskArray), gPMConstants.PMEReturnCode)

            sMsg = "Could not LoadRiskFromDB"



            If oGIS.LoadRiskFromDB(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataset, r_sGISDataModelCode:=CStr(vArray(1, 0)).Trim(), v_lInsuranceFileCnt:=CInt(vArray(0, 0)), v_lRiskID:=CInt(vRiskArray(0, 0))) <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            ' Create Data Set Control
            sMsg = "Could not load cGISDataSetControl.Application"
            m_oRiskDataSet = New cGISDataSetControl.Application()
            If m_oRiskDataSet Is Nothing Then
                Throw New Exception()
            End If

            ' Load From XML
            sMsg = "Could not LoadFromXML"
            If m_oRiskDataSet.LoadFromXML(v_sXMLDataSetDef:=CStr(sXMLDataSetDef), v_sXMLDataSet:=CStr(sXMLDataset)) <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            'finished with bGIS
            oGIS.Dispose()
            oGIS = Nothing

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sMsg, vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="LoadRiskDataEngine", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            'tidy up
            If Not (oGIS Is Nothing) Then
                oGIS.Dispose()
            End If


            'nothing more to do

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LoadPartyDataEngine
    '
    ' Description:  Load a data engine with the Party risk details for a Party
    '               This data is then exposed as Engine.PartyDataEngine
    '               Called from Public Property Get PartyDataEngine
    ' PLICO14
    ' ***************************************************************** '

    Private Function LoadPartyDataEngine(ByRef lPartyCnt As Integer) As Integer
        Dim oGIS As bGIS.Application
        Dim sXMLDataset, sXMLDataSetDef, sMsg As String


        Try

            sMsg = "Could not load bGIS.Application"
            oGIS = New bGIS.Application()
            If oGIS Is Nothing Then
                Throw New Exception()
            End If

            sMsg = "Could not initialise bGIS.Application"
            If oGIS.Initialise(sUserName:=CStr(m_sUsername), sPassword:=CStr(m_sPassword), iUserID:=CInt(m_iUserID), iSourceID:=CInt(m_iSourceID), iLanguageID:=CInt(m_iLanguageID), iCurrencyID:=CInt(m_iCurrencyID), iLogLevel:=CInt(m_iLogLevel), sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            sMsg = "Could not LoadPartyFromDB"
            If oGIS.LoadPartyFromDB(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataset, r_sGISDataModelCode:="", v_lPartyCnt:=lPartyCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            ' Create Data Set Control
            sMsg = "Could not load cGISDataSetControl.Application"
            m_oPartyDataSet = New cGISDataSetControl.Application()
            If m_oPartyDataSet Is Nothing Then
                Throw New Exception()
            End If

            ' Load From XML
            sMsg = "Could not LoadFromXML"
            If m_oPartyDataSet.LoadFromXML(v_sXMLDataSetDef:=CStr(sXMLDataSetDef), v_sXMLDataSet:=CStr(sXMLDataset)) <> PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            'finished with bGIS
            oGIS.Dispose()
            oGIS = Nothing

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sMsg, vApp:=CStr(ACApp), vClass:=ACClass, vMethod:=" LoadPartyDataEngine", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            'tidy up
            If Not (oGIS Is Nothing) Then
                oGIS.Dispose()
            End If


            'nothing more to do

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetProductDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 14-10-2005 : True Monthly Policy
    ' ***************************************************************** '
    Private Function GetProductDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetProductDetails"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vProductDetails(,) As Object




        result = gPMConstants.PMEReturnCode.PMTrue

        If m_lInsuranceFileCnt = 0 Then
            If m_lGISDataModelType = GISDMTypeClaim Then
                lReturn = GetInsuranceFileCntForClaim()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetInsuranceFileCntForClaim Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            Else
                lReturn = GetInsuranceFileCnt()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetInsuranceFileCnt Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If
        End If


        ' if the insurance file used to return the product details
        ' has changed then get the new details
        If m_lProductDetailsInsuranceFile <> m_lInsuranceFileCnt Then

            m_lProductDetailsInsuranceFile = m_lInsuranceFileCnt

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", m_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetProductDetailsSQL, sSQLName:=kGetProductDetailsName, bStoredProcedure:=True, vResultArray:=vProductDetails, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetProductDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vProductDetails) Then

                m_bIsTrueMonthlyPolicy = (gPMFunctions.ToSafeLong(CInt(vProductDetails(0, 0)), 0) = 1)
            End If

        End If


        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AddDocumentTemplateReference
    '
    ' Description:  Adds a document template reference to this risk to a property on this risk
    '
    ' History: CLG 20051020 created
    ' ***************************************************************** '

    Public Function AddDocumentTemplateReference(ByVal v_sObjectname As String, ByVal v_sPropertyName As String) As Integer
        Dim vStandardWordingArray, vArray2(,) As Object
        Dim sMsg, sSQL As String
        Dim lReturn As gPMConstants.PMEReturnCode


        Try

            'get any related documents
            sSQL = "select dt.document_template_id,p.gis_object_id,p.gis_property_id from gis_property p " &
                   "inner join gis_object o on p.gis_object_id=o.gis_object_id " &
                   "inner join document_template dt on dt.document_filter=p.specials_type_reference " &
                   "inner join document_type dt2 on dt2.document_type_id=dt.document_type_id " &
                   "Where dt.copy_of_original=0 " &
                   "and dt2.code='Clauses' " &
                   "and o.object_name='" & v_sObjectname & " '" &
                   "and p.property_name = '" & v_sPropertyName & "'"

            lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetDocumentsByFilter", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vStandardWordingArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Informations.IsArray(vStandardWordingArray) Then
                Exit Function
            End If

            'so we have some linked documents
            'get the next free slot in the <datamodel>_standard_wording table
            '(work_claim)gis_policy_link -> work_claim -> (claim)insurance_file -> (risk)gis_policy_link* -> gis_data_model*


            sSQL = "select  ISNULL(Max(sequence_id)+1,1) from " & m_sGISDataModel & "_standard_wording " &
                   "where multipac_policy_binder_id=" & CStr(m_lPolicyBinderId) &
                   "and gis_object_id=" & CStr(vStandardWordingArray(1, 0)) &
                   "and gis_property_id=" & CStr(vStandardWordingArray(2, 0))
            lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetFreeSlotIn <dmc>_standard_wording", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray2)

            'now loop round adding the new entries

            For iCount As Integer = 0 To vStandardWordingArray.GetUpperBound(1)

                bPMAddParameter.AddParameterLite(m_oDatabase, "data_model", m_sGISDataModel, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMTableName, True)
                bPMAddParameter.AddParameterLite(m_oDatabase, "policy_binder_id", m_lPolicyBinderId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "sequence_id", vArray2(0, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "document_template_id", vStandardWordingArray(0, iCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "gis_object_id", vStandardWordingArray(1, iCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "gis_property_id", vStandardWordingArray(2, iCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertStandardWordingSQL, sSQLName:=ACInsertStandardWordingName, bStoredProcedure:=ACInsertStandardWordingStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception()
                    Exit Function
                End If

                'increment slot number


                vArray2(0, 0) = CDbl(vArray2(0, 0)) + 1

            Next iCount

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sMsg, vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="AddDocumentTemplateReference", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)



            'nothing more to do

        End Try

    End Function

    ' ***************************************************************** '
    ' Name: GetSchemeDetails
    '
    ' Description:  Loads an array containing the scheme details.
    '
    ' History: CLG 20060206 created
    ' ***************************************************************** '

    Private Function GetSchemeDetails(ByRef v_lSchemeId As Integer) As Object
        Dim Catch_Renamed As Boolean = False



        Dim sMsg As String = String.Empty
        Try
            Catch_Renamed = True

            Dim vArray(,) As Object : sMsg = "Could not load Scheme Details"

            Dim vSingleDimensionArray As Object
            With m_oDatabase

                .Parameters.Clear()
                .Parameters.Add(sName:="gis_scheme_id", vValue:=CStr(v_lSchemeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect(sSQL:=ACGetSchemeDetails, sSQLName:=ACGetSchemeDetailsName, bStoredProcedure:=ACGetSchemeDetailsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And Informations.IsArray(vArray) Then


                    ReDim vSingleDimensionArray(vArray.GetUpperBound(0))


                    For iCount As Integer = 0 To vArray.GetUpperBound(0)


                        vSingleDimensionArray(iCount) = vArray(iCount, 0)
                    Next
                    Return vSingleDimensionArray
                End If

            End With

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            If Catch_Renamed Then


                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sMsg, vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="GetSchemeDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)


                'nothing more to do

            End If
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetPolicyReinsurers
    '
    ' Parameters: n/a
    '
    ' Description: To retrive list of Reinsurers associated with the policy.
    ' This information won't be available during NB, MTA.. etc but will be for Claims
    ' Created : Vivek and Gaurav: 03-04-2006
    ' ***************************************************************** '
    Public Function GetPolicyReinsurers(ByRef r_vReinsurers(,) As Object) As Integer
        Dim Catch_Renamed As Boolean = False
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sSQL, sSQL2 As String


        Try
            Catch_Renamed = True

            result = gPMConstants.PMEReturnCode.PMNotFound

            m_oDatabase.Parameters.Clear()
            sSQL2 = "GetReinsurerDetails"
            m_lInsuranceFileCnt = GetInsuranceFileCntForClaim()

            If m_lInsuranceFileCnt = gPMConstants.PMEReturnCode.PMNotFound Then
                Return result
            End If
            'm_lInsuranceFileCnt = 0
            sSQL = "SELECT  DISTINCT p.party_cnt,p.shortName,p.Resolved_Name," &
                   "p.Domiciled_for_Tax,p.Tax_Exempt " &
                   "FROM Party p INNER JOIN RI_arrangement_line RL " &
                   "ON P.party_cnt=RL.party_cnt INNER JOIN RI_arrangement RI " &
                   "ON RL.RI_arrangement_id= RI.RI_arrangement_id " &
                   "INNER JOIN Insurance_file_risk_link IFRL " &
                   "ON RI.risk_cnt = IFRL.risk_cnt " &
                   "INNER JOIN Insurance_file IFL " &
                   "ON IFRL.insurance_file_cnt = IFL.Insurance_file_cnt " &
                   "Where ifl.Insurance_file_cnt=" & CStr(m_lInsuranceFileCnt)

            lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=sSQL2, bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vReinsurers)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            If Catch_Renamed Then


                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetPolicyReinsurers Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="GetPolicyReinsurers", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)



                Return result
            End If
        End Try
    End Function


    Public Function GetPolicySections(ByRef r_vSectionArray(,) As Object) As Integer
        Dim Catch_Renamed As Boolean = False
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sSQL, sSQL2 As String

        Dim sMsg As String = ""
        Dim iRetVal As gPMConstants.PMEReturnCode
        Dim sValue As String = ""
        Dim sTaxGroupCode As String = ""
        Dim vSectionArray(,) As Object
        Dim bLinkCommACCToACCExec As Boolean
        Dim lNumberRecords As Integer
        Dim vResultsArray(,) As Object

        Const cPolicySectionSP_CobSectionId As Integer = 0
        Const cPolicySectionSP_CobSectionCode As Integer = 1
        Const cPolicySectionSP_CobDescription As Integer = 2
        Const cPolicySectionSP_IsSelected As Integer = 3
        Const cPolicySectionSP_CanBeDeleted As Integer = 4

        Const cPolicySection_CobSectionId As Integer = 0
        Const cPolicySection_CobSectionCode As Integer = 1
        Const cPolicySection_CobDescription As Integer = 2
        Const cPolicySection_IsSelected As Integer = 3
        Const cPolicySection_CanBeDeleted As Integer = 4
        Const cPolicySection_TaxGroupId As Integer = 5
        Const cPolicySection_TaxRate As Integer = 6
        Const cPolicySection_CommissionCnt As Integer = 7
        'Const cPolicySection_CommissionTaxGroupId As Integer = 8
        Const cPolicySection_CommissionTaxRate As Integer = 9
        Const cPolicySection_Max As Integer = cPolicySection_CommissionTaxRate


        Try
            Catch_Renamed = True

            If m_lInsuranceFileCnt = 0 Then GetInsuranceFileCnt()

            'Getdetails
            GetInsuranceFileDetails()

            m_lReturn = bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTLinkCommACCToACCExec, v_vBranch:=CInt(m_iSourceID), r_vUnderwriting:=sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed ", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="GetPolicySections")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            bLinkCommACCToACCExec = sValue = "1"

            sSQL = "spu_txn_risk_code_section_sel"
            sSQL2 = "spu_txn_risk_code_section_sel"
            bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "risk_code_id", m_lRiskCodeId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "include_all_sections", 0, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=sSQL2, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vSectionArray)

            result = gPMConstants.PMEReturnCode.PMNotFound
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Informations.IsArray(vSectionArray) Then
                Return result
            End If


            ReDim r_vSectionArray(cPolicySection_Max, vSectionArray.GetUpperBound(1))


            For iCount As Integer = 0 To vSectionArray.GetUpperBound(1)

                m_oDatabase.Parameters.Clear()
                sSQL = "spu_txn_section_tax_rate_sel"
                sSQL2 = "spu_txn_section_tax_rate_sel"
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "InsurerSection", 0, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "RiskGroupId", m_lRiskGroupId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "RiskCodeId", m_lRiskCodeId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "CobRatingSectionid", vSectionArray(cPolicySection_CobSectionId, iCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                'Country_id is of party for policy sections
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "CountryId", m_lCountryId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "TransactionType", 0, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "EffectiveDate", m_vCoverStartDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "InsurerId", m_lLeadInsurerCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                'developer guide no. 119 (guide)
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "rTaxGroupId", Nothing, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

                'developer guide no. 119(guide)
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "rTaxGroupCode", Nothing, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMString)

                'developer guide no. 119 (guide)
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "rTaxRate", Nothing, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)

                'developer guide no. 119 (guide)
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "rcalcBasis", Nothing, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
                lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:=sSQL2, bStoredProcedure:=True, lRecordsAffected:=lNumberRecords)



                r_vSectionArray(cPolicySection_CobSectionId, iCount) = vSectionArray(cPolicySectionSP_CobSectionId, iCount)


                r_vSectionArray(cPolicySection_CobSectionCode, iCount) = vSectionArray(cPolicySectionSP_CobSectionCode, iCount)


                r_vSectionArray(cPolicySection_CobDescription, iCount) = vSectionArray(cPolicySectionSP_CobDescription, iCount)


                r_vSectionArray(cPolicySection_IsSelected, iCount) = vSectionArray(cPolicySectionSP_IsSelected, iCount)


                r_vSectionArray(cPolicySection_CanBeDeleted, iCount) = vSectionArray(cPolicySectionSP_CanBeDeleted, iCount)

                r_vSectionArray(cPolicySection_TaxGroupId, iCount) = m_oDatabase.Parameters.Item("rTaxGroupId").Value

                r_vSectionArray(cPolicySection_TaxRate, iCount) = m_oDatabase.Parameters.Item("rTaxRate").Value

                If bLinkCommACCToACCExec Then
                    sSQL = "SELECT ISNULL((SELECT DISTINCT PTY.party_cnt FROM Party PTY"
                    sSQL = sSQL & " JOIN Party_Consultant PTC ON PTC.commission_cnt = PTY.party_cnt"
                    sSQL = sSQL & " JOIN Insurance_file I JOIN Insurance_file I"
                    sSQL = sSQL & " WHERE I.Insurance_file_cnt = " & CStr(m_lInsuranceFileCnt) & "),0)"
                    sSQL2 = ""
                    lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=sSQL2, bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultsArray)
                    If Informations.IsArray(vResultsArray) Then


                        r_vSectionArray(cPolicySection_CommissionCnt, iCount) = vResultsArray(0, 0)
                    End If
                Else
                    sSQL = "SELECT ISNULL((SELECT commission_cnt FROM risk_by_source RBS"
                    sSQL = sSQL & " JOIN Risk_code RC ON RC.risk_group_id = RBS.risk_group_id"
                    sSQL = sSQL & " JOIN Insurance_file I ON I.risk_code_id = RC.risk_code_id"
                    sSQL = sSQL & " AND I.source_id = RBS.source_id"
                    sSQL = sSQL & " WHERE I.Insurance_file_cnt = " & CStr(m_lInsuranceFileCnt) & "),0)"
                    sSQL2 = ""
                    lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=sSQL2, bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultsArray)
                    If Informations.IsArray(vResultsArray) Then


                        r_vSectionArray(cPolicySection_CommissionCnt, iCount) = vResultsArray(0, 0)
                    End If


                    If CDbl(r_vSectionArray(cPolicySection_CommissionCnt, iCount)) = 0 Then
                        sSQL = "SELECT ISNULL((SELECT commission_cnt FROM risk_by_source RBS"
                        sSQL = sSQL & " JOIN Risk_code RC ON RC.risk_group_id = RBS.risk_group_id"
                        sSQL = sSQL & " JOIN Insurance_file I ON I.risk_code_id = RC.risk_code_id"
                        sSQL = sSQL & " AND RBS.source_id = 0"
                        sSQL = sSQL & " WHERE I.Insurance_file_cnt = " & CStr(m_lInsuranceFileCnt) & "),0)"
                        sSQL2 = ""
                        lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=sSQL2, bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultsArray)
                        If Informations.IsArray(vResultsArray) Then


                            r_vSectionArray(cPolicySection_CommissionCnt, iCount) = vResultsArray(0, 0)
                        End If
                    End If
                End If

                'r_vSectionArray(cPolicySection_CommissionTaxGroupId, iCount)
                'r_vSectionArray(cPolicySection_CommissionTaxRate, iCount)
            Next

            'finished with bGIS

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            If Catch_Renamed Then


                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sMsg, vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="GetPolicySections", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
                Return result

                'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

                'nothing more to do

                Return result
            End If
        End Try
    End Function



    ' ***************************************************************** '
    ' Name: GetInsurerTaxRate
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '

    Public Function GetInsurerTaxRate(ByRef r_lPartyCnt As Integer, ByRef r_vTaxGroupId As Object, ByRef r_vTaxGroupCode As Object, ByRef r_vTaxRate As Object) As Integer
        Dim Catch_Renamed As Boolean = False

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sSQL, sSQL2, sMsg As String
        Dim iRetVal As gPMConstants.PMEReturnCode
        Dim vTaxRateArray(,) As Object
        Dim lCountryId As Integer
        Dim oParty As bSIRParty.Services

        Const cTaxRate_TaxGroupId As Integer = 0
        Const cTaxRate_TaxGroupCode As Integer = 1
        Const cTaxRate_TaxRate As Integer = 2


        Try
            Catch_Renamed = True

            'GetCountryId for Insurer

            oParty = New bSIRParty.Services
            m_lReturn = oParty.Initialise(sUsername:=CStr(m_sUsername), sPassword:=CStr(m_sPassword), iUserID:=CInt(m_iUserID), iSourceID:=CInt(m_iSourceID), iLanguageID:=CInt(m_iLanguageID), iCurrencyID:=CInt(m_iCurrencyID), iLogLevel:=CInt(m_iLogLevel), sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            If oParty Is Nothing Then
                Return result
            End If
            With oParty

                .PartyCnt = r_lPartyCnt

                m_lReturn = .GetDetails

                lCountryId = .CountryId
            End With

            oParty.Dispose()
            oParty = Nothing


            sSQL = "spu_TXN_party_tax_rate_sel"
            sSQL2 = "spu_TXN_party_tax_rate_sel"
            bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "PartyCnt", r_lPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "RiskGroupId", m_lRiskGroupId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "RiskCodeId", m_lRiskCodeId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "CountryId", lCountryId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "TransactionType", 0, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "EffectiveDate", m_vCoverStartDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=sSQL2, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vTaxRateArray)

            result = gPMConstants.PMEReturnCode.PMNotFound
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Informations.IsArray(vTaxRateArray) Then
                Return result
            End If
            result = gPMConstants.PMEReturnCode.PMTrue




            r_vTaxGroupId = vTaxRateArray(cTaxRate_TaxGroupId, vTaxRateArray.GetLowerBound(1))



            r_vTaxGroupCode = vTaxRateArray(cTaxRate_TaxGroupCode, vTaxRateArray.GetLowerBound(1))



            r_vTaxRate = vTaxRateArray(cTaxRate_TaxRate, vTaxRateArray.GetLowerBound(1))

            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            If Catch_Renamed Then


                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sMsg, vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="GetInsurerTaxRate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)


                'nothing more to do

                Return result
            End If
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CreatePolicyOutputSections
    '
    ' Description:  Loads an array containing policy section details.
    '               These sections form the basis for the sections a rating section must quote for.
    '
    ' History: CLG 20060125 created
    ' ***************************************************************** '

    Public Function CreatePolicyOutputSections(ByRef r_oDataset As Object) As Integer
        Dim Catch_Renamed As Boolean = False

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sSQL2 As String = ""
        Dim sSQL As New StringBuilder
        Dim vResultArray, vSectionArray(,) As Object
        Dim sOIKey, sChildOIKey As String
        Dim vSectionTaxArray As String = ""
        Dim vSectionInsurerArray(,) As Object
        Dim sMsg As String

        Dim lInsuranceFileCnt As Integer
        Dim sOutputObjectName, sOutputDetailsObjectName, sOutputDetailsInsurerObjectName As String
        Dim vSchemeDetails As Array

        Const cPolicySection_SectionId As Integer = 0
        Const cPolicySection_SectionDescription As Integer = 1
        Const cPolicySection_SectionCode As Integer = 23
        Const cPolicySection_Tax As Integer = 24
        Const cPolicySection_Insurers As Integer = 25

        'Const cPolicySectionTax_tax_group_description As Integer = 2
        Const cPolicySection_TaxGroupId As Integer = 3
        'Const cPolicySectionTax_tax_type_description As Integer = 4
        'Const cPolicySectionTax_tax_band_rate_description As Integer = 6
        'Const cPolicySectionTax_is_value As Integer = 7
        'Const cPolicySectionTax_rate As Integer = 8
        'Const cPolicySectionTax_calc_basis As Integer = 9

        'Insurer
        ' Const cPolicySectionInsurer_type As Integer = 0
        Const cPolicySectionInsurer_party_cnt As Integer = 1
        ' Const cPolicySectionInsurer_Scheme As Integer = 2
        ' Const cPolicySectionInsurer_risk_group_id As Integer = 3
        ' Const cPolicySectionInsurer_risk_code_id As Integer = 4
        ' Const cPolicySectionInsurer_effective_date As Integer = 5
        'Const cPolicySectionInsurer_rate1 As Integer = 6
        'Const cPolicySectionInsurer_value1 As Integer = 7
        'Const cPolicySectionInsurer_minimum_total1 As Integer = 8
        ' Const cPolicySectionInsurer_rate2 As Integer = 9
        'Const cPolicySectionInsurer_value2 As Integer = 10
        ' Const cPolicySectionInsurer_minimum_total2 As Integer = 11
        ' Const cPolicySectionInsurer_rate3 As Integer = 12
        'Const cPolicySectionInsurer_value3 As Integer = 13
        'Const cPolicySectionInsurer_minimum_total3 As Integer = 14
        ' Const cPolicySectionInsurer_is_gemini_transferred As Integer = 15
        ' Const cPolicySectionInsurer_address_cnt As Integer = 16
        'Const cPolicySectionInsurer_Tax_Group_id As Integer = 17
        Const cPolicySectionInsurer_insurer_name As Integer = 18
        Const cPolicySectionInsurer_insurer_code As Integer = 19

        'scheme
        Const cSchemeId As Integer = 0
        Const cSchemeDescription As Integer = 8



        Try
            Catch_Renamed = True

            m_oDatabase.Parameters.Clear()

            lReturn = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(m_lPolicyLinkId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyIDSQL, sSQLName:=ACGetPolicyIDName, bStoredProcedure:=ACGetPolicyIDStored, vResultArray:=vResultArray)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            lInsuranceFileCnt = CInt(vResultArray(0, 0))

            sSQL = New StringBuilder("spu_Policy_Rating_Section_sel")
            sSQL2 = "GetPolicySections"

            With m_oDatabase.Parameters
                .Clear()
                .Add("insuranceFileCnt", CStr(lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Add("TransactionType", CStr(1), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Add("EffectiveDate", DateTime.Now.ToString, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            End With
            lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:=sSQL2, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vSectionArray)

            result = gPMConstants.PMEReturnCode.PMNotFound
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Informations.IsArray(vSectionArray) Then
                Return result
            End If

            'get the scheme details

            vSchemeDetails = GetSchemeDetails(m_lGisSchemeId)

            'Create a XXX_OUTPUT record
            sOutputObjectName = m_sGISDataModel & "_OUTPUT"

            lReturn = r_oDataset.GetAllOIKey(CStr(m_sGISDataModel) & "_POLICY_BINDER", vResultArray)


            lReturn = r_oDataset.NewObjectInstance(CStr(sOutputObjectName), CStr(sOIKey), CStr(vResultArray(0)))

            'populate OUTPUT default properties

            lReturn = r_oDataset.SetPropertyValue(CStr(sOutputObjectName), "scheme_desc", CStr(sOIKey), vSchemeDetails(cSchemeDescription), False)

            lReturn = r_oDataset.SetPropertyValue(CStr(sOutputObjectName), "scheme_id", CStr(sOIKey), vSchemeDetails(cSchemeId), False)

            Dim sInsurerOIKey As String = ""

            For iCount As Integer = 0 To vSectionArray.GetUpperBound(1)

                sOutputDetailsObjectName = sOutputObjectName & "_DETAILS"

                lReturn = r_oDataset.NewObjectInstance(CStr(sOutputDetailsObjectName), CStr(sChildOIKey), CStr(sOIKey))

                lReturn = r_oDataset.SetPropertyValue(CStr(sOutputDetailsObjectName), "description", CStr(sChildOIKey), vSectionArray(cPolicySection_SectionDescription, iCount), False)

                lReturn = r_oDataset.SetPropertyValue(CStr(sOutputDetailsObjectName), "code", CStr(sChildOIKey), vSectionArray(cPolicySection_SectionCode, iCount), False)

                lReturn = r_oDataset.SetPropertyValue(CStr(sOutputDetailsObjectName), "Section_Id", CStr(sChildOIKey), vSectionArray(cPolicySection_SectionId, iCount), False)


                'The following code gets the tax details for each section
                sSQL = New StringBuilder("")
                sSQL.Append("select tg.tax_group_id,tg.code as tax_group_code,pmc1.caption as tax_group_description, ")
                sSQL.Append("tt.code as tax_type_code,pmc2.caption as tax_type_description, ")
                sSQL.Append("tbr.code as tax_band_rate_code ,pmc3.caption as tax_band_rate_description, ")
                sSQL.Append("tbr.is_value,tbr.rate,tbr.Calc_Basis,tbr.Basis_Value,tbr.NB,tbr.AMTA,tbr.RMTA,tbr.CANC,tbr.REN,tbr.Sum_Insured_Rounded, ")
                sSQL.Append("tbr.currency_id,tbr.allow_tax_credit,tbr.country_id,tbr.state_id,tbr.class_of_business_id,tbr.TTRI,tbr.TTRIC,tbr.TTAC,tbr.TTF, ")
                sSQL.Append("tbr.TTCP , tbr.TTCS, tbr.TTCR, tbr.TTIC, tbr.MTA_Threshold_date, tbr.Is_passed_to_insurer ")
                '--we don't need all of the tax_band_rate columns
                '--tbr.tax_band_rate_id,tbr.code,tbr.caption_id,tbr.effective_date,tbr.is_deleted,tbr.tax_band_id
                sSQL.Append("from tax_group tg ")
                sSQL.Append("inner join tax_group_tax_band tgtb on tgtb.tax_group_id=tg.tax_group_id ")
                sSQL.Append("inner join tax_band tb on tb.tax_band_id=tgtb.tax_band_id ")
                sSQL.Append("inner join tax_band_rate tbr on tbr.tax_band_id=tgtb.tax_band_id ")
                sSQL.Append("inner join tax_type tt on tt.tax_type_id=tb.tax_type_id ")
                sSQL.Append("left outer join pmcaption pmc1 on (pmc1.caption_id=tg.caption_id and pmc1.language_id=" & m_iLanguageID & ") ")
                sSQL.Append("left outer join pmcaption pmc2 on (pmc2.caption_id=tt.caption_id and pmc1.language_id=" & m_iLanguageID & ") ")
                sSQL.Append("left outer join pmcaption pmc3 on (pmc3.caption_id=tbr.caption_id and pmc1.language_id=" & m_iLanguageID & ") ")

                sSQL.Append("where tg.tax_group_id=" & CStr(vSectionArray(cPolicySection_TaxGroupId, iCount)) & " and ")
                sSQL.Append("tg.is_deleted=0 and tg.effective_date <='" & DateTime.Now.ToString("yyyyMMdd") & "' and ")
                sSQL.Append("tb.is_deleted=0 and tb.effective_date <='" & DateTime.Now.ToString("yyyyMMdd") & "' and ")
                sSQL.Append("tbr.is_deleted=0 and tbr.effective_date <='" & DateTime.Now.ToString("yyyyMMdd") & "' ")
                sSQL.Append("order by tgtb.tax_group_id,tgtb.sequence")

                sSQL2 = "GetPolicySectionsTaxes"
                lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:=sSQL2, bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vSectionTaxArray)

                If lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    vSectionArray(cPolicySection_Tax, iCount) = vSectionTaxArray
                End If

                'The following code gets the insurers and their commision for each section
                m_oDatabase.Parameters.Clear()
                sSQL = New StringBuilder("spu_policy_section_insurers_sel")
                sSQL2 = "GetPolicySectionInsurers"
                With m_oDatabase.Parameters
                    .Clear()
                    .Add("insuranceFileCnt", CStr(lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    .Add("scheme_id", CStr(vSchemeDetails(cSchemeId)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    .Add("EffectiveDate", DateTime.Now, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)

                    .Add("cob_rating_section_id", CStr(vSectionArray(cPolicySection_SectionId, iCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                End With
                lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:=sSQL2, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vSectionInsurerArray)
                If lReturn = gPMConstants.PMEReturnCode.PMTrue And Informations.IsArray(vSectionInsurerArray) Then

                    vSectionArray(cPolicySection_Insurers, iCount) = vSectionInsurerArray

                    sOutputDetailsInsurerObjectName = sOutputDetailsObjectName & "_INSURER"
                    For iInsurerCount As Integer = 0 To vSectionInsurerArray.GetUpperBound(1)

                        lReturn = r_oDataset.NewObjectInstance(CStr(sOutputDetailsInsurerObjectName), CStr(sInsurerOIKey), CStr(sChildOIKey))

                        lReturn = r_oDataset.SetPropertyValue(CStr(sOutputDetailsInsurerObjectName), "id", CStr(sInsurerOIKey), vSectionInsurerArray(cPolicySectionInsurer_party_cnt, iInsurerCount), False)

                        lReturn = r_oDataset.SetPropertyValue(CStr(sOutputDetailsInsurerObjectName), "code", CStr(sInsurerOIKey), vSectionInsurerArray(cPolicySectionInsurer_insurer_code, iInsurerCount), False)

                        lReturn = r_oDataset.SetPropertyValue(CStr(sOutputDetailsInsurerObjectName), "description", CStr(sInsurerOIKey), vSectionInsurerArray(cPolicySectionInsurer_insurer_name, iInsurerCount), False)
                    Next
                End If
            Next

            'finished with bGIS

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            r_oDataset.SaveXMLToFile(, "c:\temp\ugg1.xml")

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            If Catch_Renamed Then


                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sMsg, vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="CreatePolicyOutputSections", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)


                'nothing more to do

                Return result
            End If
        End Try
    End Function



    ' **************************************************************************** '
    ' Name: GetCatastropheCodeDetails
    '
    ' Description:
    '
    ' Created : Puneet Kukreti : 21-07-2006 : Get Valid ctastrophe Code Details
    ' *************************************************************************** '
    Public Function GetCatastropheCodeDetails(ByVal lCatastropheRegionID As String, ByVal dtLossDate As String, ByVal lPrimaryCauseID As String, ByVal lSecondaryCauseID As String, ByRef vResultArray(,) As Object) As Integer


        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            sSQL = "SELECT catastrophe_code_id, caption_id, code, description, is_deleted,"
            sSQL = sSQL & " effective_date, claim_catastrophe_region_id, primary_cause_id, secondry_cause_id, from_date, to_date"
            sSQL = sSQL & " FROM catastrophe_code"
            sSQL = sSQL & " WHERE catastrophe_code_id > 0"

            If lCatastropheRegionID <> "" Then

                sSQL = sSQL & " claim_catastrophe_region_id ={ICatastropheRegionID}"

                If dtLossDate <> "" Then
                    sSQL = sSQL & " And from_date >= {dtLossDate} and effective_date >= {dtLossDate} and to_date <= {dtLossDate}"
                End If

                If lSecondaryCauseID <> "" Then
                    sSQL = sSQL & " And secondry_cause_id ={lSecondaryCauseID}"
                End If

                If lPrimaryCauseID <> "" Then
                    sSQL = sSQL & " And primary_cause_id ={lPrimaryCauseID}"
                    sSQL = sSQL & " And primary_cause_id in (SELECT pc.primary_cause_id "
                    sSQL = sSQL & " FROM Insurance_File AS ins"
                    sSQL = sSQL & " Inner Join"
                    sSQL = sSQL & " Product AS pro ON ins.product_id = pro.product_id"
                    sSQL = sSQL & " Inner Join"
                    sSQL = sSQL & " Product_Risk_Type_Group AS prtg ON ins.product_id = prtg.product_id"
                    sSQL = sSQL & " Inner Join"
                    sSQL = sSQL & " Risk_Type_Group AS rtg ON prtg.risk_type_group_id = rtg.risk_type_group_id"
                    sSQL = sSQL & " Inner Join"
                    sSQL = sSQL & " Primary_Cause_Risk_Type_Group AS pcrtg ON rtg.risk_type_group_id = pcrtg.risk_type_group_id"
                    sSQL = sSQL & " Inner Join"
                    sSQL = sSQL & " Primary_Cause AS pc ON pcrtg.primary_cause_id = pc.primary_cause_id"
                    sSQL = sSQL & " Where ins.insurance_file_cnt = {m_lInsuranceFileCnt}"
                    sSQL = sSQL & " AND pc.Is_deleted = 0"
                    sSQL = sSQL & " ORDER BY pc.primary_cause_id ASC"

                End If

            End If

            m_oDatabase.Parameters.Clear()

            lReturn = m_oDatabase.Parameters.Add(sName:="lCatastropheRegionID", vValue:=lCatastropheRegionID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            lReturn = m_oDatabase.Parameters.Add(sName:="dtLossDate", vValue:=dtLossDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            lReturn = m_oDatabase.Parameters.Add(sName:="lPrimaryCauseID", vValue:=lPrimaryCauseID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            lReturn = m_oDatabase.Parameters.Add(sName:="lSecondaryCauseID", vValue:=lCatastropheRegionID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            lReturn = m_oDatabase.Parameters.Add(sName:="m_lInsuranceFileCnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetCatastropheCode", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCatastropheCodeDetails Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="GetPartyCnt", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function








    ' ***************************************************************** '
    '
    ' Name: GetResolvedNameFromPartyCnt
    '
    ' Description: Derives Resolved_name from PartyCnt
    '
    ' History: 15/11/2006 MKW - Created.
    '
    ' ***************************************************************** '
    Public Function GetResolvedNameFromPartyCnt(ByRef v_lPartyCnt As Integer, ByRef v_sResolvedName As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "Select resolved_name from party where party_cnt=" & gPMFunctions.ToSafeLong(v_lPartyCnt, 0)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetResolvedNameFromPartyCnt", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vArray) Then


                v_sResolvedName = vArray(0, 0)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetResolvedNameFromPartyCnt Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="GetResolvedNameFromPartyCnt", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
            Return result
        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetInsurerCommissionRate
    '
    ' Description: Derives InsurerCommissionRate for Insurer.
    '
    ' History: 27/01/2007 MKW - Created.
    '
    ' ***************************************************************** '
    Public Function GetInsurerCommissionRate(ByRef v_lInsurerCnt As Object, ByRef v_fCommissionRate As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim oInsurerRate As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oInsurerRate, v_sClassName:="bSirInsurerRate.Business", v_sCallingAppName:=CStr(ACApp), v_sUsername:=CStr(m_sUsername), v_sPassword:=CStr(m_sPassword), v_iUserID:=CInt(m_iUserID), v_iSourceID:=CInt(m_iSourceID), v_iLanguageID:=CInt(m_iLanguageID), v_iCurrencyID:=CInt(m_iCurrencyID), v_iLogLevel:=CInt(m_iLogLevel), v_oDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            If oInsurerRate Is Nothing Then
                Return result
            End If

            m_lReturn = oInsurerRate.GetDetails(vPartyCnt:=CInt(v_lInsurerCnt), vScheme:=CInt(m_lGisSchemeId), vRiskCodeID:=CInt(m_lRiskCodeId), vEffectiveDate:=m_vCoverStartDate)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = oInsurerRate.GetNext(vRate1:=v_fCommissionRate)

                oInsurerRate.Dispose()
                oInsurerRate = Nothing
                Return result
            End If


            m_lReturn = oInsurerRate.GetDetails(vPartyCnt:=CInt(v_lInsurerCnt), vScheme:=CInt(m_lGisSchemeId), vRiskCodeID:=0, vEffectiveDate:=m_vCoverStartDate)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = oInsurerRate.GetNext(vRate1:=v_fCommissionRate)

                oInsurerRate.Dispose()
                oInsurerRate = Nothing
                Return result
            End If


            m_lReturn = oInsurerRate.GetDetails(vPartyCnt:=CInt(v_lInsurerCnt), vScheme:=0, vRiskCodeID:=0, vEffectiveDate:=m_vCoverStartDate)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = oInsurerRate.GetNext(vRate1:=v_fCommissionRate)

                oInsurerRate.Dispose()
                oInsurerRate = Nothing
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsurerCommissionRate Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="GetInsurerCommissionRate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function UpdateClaimReserves(ByVal v_ClaimDetailsArray As Object, Optional ByVal bPostReserves As Boolean = False) As Integer

        Const kMethodName As String = "UpdateClaimReserves"

        Dim bCanChangeReserves As Boolean
        Dim nReserveID As Integer
        Dim crRevised_Reserve As Decimal
        Dim crInitial_Reserve As Decimal
        Dim crPaidtoDate As Decimal
        Dim crAverage As Decimal
        Dim crThis_Payment As Decimal
        Dim crThis_Revision As Decimal
        Dim bRevised_Entered As Boolean
        Dim crSumInsured As Integer
        Dim crReserveSumInsured As Decimal

        Try

            If GetUserCanChangeReserves(v_lUserID:=CInt(m_iUserID), r_bCanChangeReserves:=bCanChangeReserves) <> PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            If Not bCanChangeReserves Then
                bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogInfo,
                        sMsg:="User cannot change reserves.", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:=kMethodName)
                Return PMEReturnCode.PMFalse
            End If

            If m_sTransactionType = kTRANSACTIONTYPE_OpenClaim Or m_sTransactionType = kTRANSACTIONTYPE_MaintainClaim Or
                m_sTransactionType = kTRANSACTIONTYPE_PayClaim Then

                If Informations.IsArray(v_ClaimDetailsArray) Then
                    Dim nClaimCnt As Integer = v_ClaimDetailsArray.GetUpperBound(1)
                    For iClaimIndex As Integer = 0 To nClaimCnt

                        If Informations.IsArray(v_ClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, iClaimIndex)) Then
                            Dim nPerilCnt As Integer = v_ClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, iClaimIndex).GetUpperBound(1)
                            For iPerilIndex As Integer = 0 To nPerilCnt
                                Dim crTotalPerilAmount As Decimal = 0
                                Dim nPerilID As Integer
                                Dim sPerilTypeCode As String
                                nPerilID = ToSafeInteger(v_ClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, iClaimIndex)(kPERIL_PerilID, iPerilIndex))
                                crSumInsured = ToSafeDecimal(v_ClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, iClaimIndex)(kPERIL_SumInsured, iPerilIndex))
                                sPerilTypeCode = ToSafeString(v_ClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, iClaimIndex)(kPERILType_Code, iPerilIndex))
                                If Informations.IsArray(v_ClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, iClaimIndex)(kPERIL_ArrayofReserve, iPerilIndex)) Then
                                    Dim nReserveCnt As Integer = v_ClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, iClaimIndex)(kPERIL_ArrayofReserve, iPerilIndex).GetUpperBound(1)
                                    For iReserveIndex As Integer = 0 To nReserveCnt
                                        If ToSafeInteger(v_ClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, iClaimIndex)(kPERIL_ArrayofReserve, iPerilIndex)(kRESERVE_IsUpdated, iReserveIndex)) = 1 Then
                                            nReserveID = ToSafeInteger(v_ClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, iClaimIndex)(kPERIL_ArrayofReserve, iPerilIndex)(kRESERVE_ReserveID, iReserveIndex))
                                            crInitial_Reserve = ToSafeDecimal(v_ClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, iClaimIndex)(kPERIL_ArrayofReserve, iPerilIndex)(kRESERVE_InitialReserve, iReserveIndex))
                                            crPaidtoDate = ToSafeDecimal(v_ClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, iClaimIndex)(kPERIL_ArrayofReserve, iPerilIndex)(kRESERVE_PaidToDate, iReserveIndex))
                                            crReserveSumInsured = ToSafeDecimal(v_ClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, iClaimIndex)(kPERIL_ArrayofReserve, iPerilIndex)(kRESERVE_SumInsured, iReserveIndex))
                                            If crSumInsured <= 0 Then
                                                crAverage = 0  'No Limit
                                            Else
                                                crAverage = ToSafeDecimal(ToSafeDecimal((crInitial_Reserve / crSumInsured) * 100))
                                            End If

                                            crThis_Payment = ToSafeDecimal(v_ClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, iClaimIndex)(kPERIL_ArrayofReserve, iPerilIndex)(kRESERVE_ThisPayment, iReserveIndex))
                                            crThis_Revision = ToSafeDecimal(v_ClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, iClaimIndex)(kPERIL_ArrayofReserve, iPerilIndex)(kRESERVE_ThisRevision, iReserveIndex))

                                            bRevised_Entered = 0

                                            If TransactionType = kTRANSACTIONTYPE_OpenClaim Then
                                                If crThis_Revision = 0 Then
                                                    crThis_Revision = crInitial_Reserve
                                                Else
                                                    crInitial_Reserve = crThis_Revision
                                                End If
                                            End If

                                            Dim crPreviousReserve As Decimal = 0
                                            If GetPreviousReserve(nReserveID, crPreviousReserve) <> PMEReturnCode.PMTrue Then
                                                Throw New Exception
                                            End If

                                            crRevised_Reserve = crPreviousReserve + crThis_Revision - crInitial_Reserve

                                            If (crInitial_Reserve + crRevised_Reserve) < 0 Then
                                                bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError,
                                                sMsg:="Total of Reserve can never be less than 0", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="UpdateClaimReserves")
                                                Return PMEReturnCode.PMFalse
                                            End If

                                            AddParameterLite(m_oDatabase, "reserveid", nReserveID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
                                            AddParameterLite(m_oDatabase, "revisedreserve", crRevised_Reserve, PMEParameterDirection.PMParamInput, PMEDataType.PMCurrency)
                                            AddParameterLite(m_oDatabase, "initialreserve", crInitial_Reserve, PMEParameterDirection.PMParamInput, PMEDataType.PMCurrency)
                                            AddParameterLite(m_oDatabase, "paidtodate", crPaidtoDate, PMEParameterDirection.PMParamInput, PMEDataType.PMCurrency)
                                            AddParameterLite(m_oDatabase, "average", crAverage, PMEParameterDirection.PMParamInput, PMEDataType.PMCurrency)
                                            AddParameterLite(m_oDatabase, "this_payment", crThis_Payment, PMEParameterDirection.PMParamInput, PMEDataType.PMCurrency)
                                            AddParameterLite(m_oDatabase, "this_revision", crThis_Revision, PMEParameterDirection.PMParamInput, PMEDataType.PMCurrency)
                                            AddParameterLite(m_oDatabase, "revised_entered", bRevised_Entered, PMEParameterDirection.PMParamInput, PMEDataType.PMCurrency)
                                            AddParameterLite(m_oDatabase, "transaction_type", TransactionType, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                                            AddParameterLite(m_oDatabase, "sum_insured", crReserveSumInsured, PMEParameterDirection.PMParamInput, PMEDataType.PMCurrency)
                                            If m_oDatabase.SQLSelect(sSQL:=ACUpdateReserveDetailsSQL,
                                                                     sSQLName:=ACUpdateReserveDetailsName,
                                                                     bStoredProcedure:=ACUpdateReserveDetailsStored) <> PMEReturnCode.PMTrue Then
                                                Throw New Exception
                                            End If

                                            crTotalPerilAmount = crTotalPerilAmount + crThis_Revision
                                        End If
                                    Next
                                End If

                                If bPostReserves AndAlso nPerilID <> 0 Then
                                    If PostReservesToOrion(nPerilID:=nPerilID, sPerilTypeCode:=sPerilTypeCode, crTotalThisReserveTrans:=crTotalPerilAmount) <> PMEReturnCode.PMTrue Then
                                        Throw New Exception
                                    End If
                                End If
                            Next
                        End If
                    Next
                End If
            End If

            Return PMEReturnCode.PMTrue

        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=CStr(m_sUsername),
                         iType:=PMELogLevel.PMLogError,
                         sMsg:="Method Failed!", vClass:=ACClass,
                         vMethod:=kMethodName,
                         excep:=ex)

            Return PMEReturnCode.PMFalse
        End Try
    End Function

    Public Function GetUserCanChangeReserves(ByVal v_lUserID As Integer, ByRef r_bCanChangeReserves As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetUserCanChangeReserves"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add("user_id", vValue:=CStr(v_lUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add("can_change_reserves", vValue:=CStr(r_bCanChangeReserves), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetUserCanChangeReservesSQL, sSQLName:=ACGetUserCanChangeReservesName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the return parameteres
            r_bCanChangeReserves = gPMFunctions.NullToLong(m_oDatabase.Parameters.Item("can_change_reserves").Value)

            m_oDatabase.Parameters.Clear()

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=CStr(m_sUsername), v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    Public Function GetAllowNegativeReserves(ByRef r_bAllowNegativeReserves As Boolean) As Integer

        Dim result As Integer = 0
        Dim r_sValue As String = ""
        Const kMethodName As String = "GetAllowNegativeReserves"

        Dim lReturn As Integer
        Dim oProductBusiness As bSIRProduct.Business
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            oProductBusiness = New bSIRProduct.Business
            m_lReturn = oProductBusiness.Initialise(sUsername:=CStr(m_sUsername), sPassword:=CStr(m_sPassword), iUserID:=CInt(m_iUserID), iSourceID:=CInt(m_iSourceID), iLanguageID:=CInt(m_iLanguageID), iCurrencyID:=CInt(m_iCurrencyID), iLogLevel:=CInt(m_iLogLevel), sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get bSIRProduct.Business")
            End If


            m_lReturn = oProductBusiness.GetProductLevelOptionsForClaim(v_lClaimID:=m_lClaimCnt, r_bAllow_Negative_Reserve:=r_bAllowNegativeReserves)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result
        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=CStr(m_sUsername), v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)


        Finally

            oProductBusiness = Nothing

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetCaseIncurredTotals
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :
    ' ***************************************************************** '
    Public Function GetCaseIncurredTotals(ByVal v_vCaseID As Object, ByRef r_vTotalIndemnity As Object, ByRef r_vTotalExpense As Object, ByRef r_vTotalExcess As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCaseIncurredTotals"
        Const kTotalIndemnity As Integer = 4
        Const kTotalExpense As Integer = 5
        Const kTotalExcess As Integer = 6

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResultArray(,) As Object
        Dim cTotalIndemnity, cTotalExpense, cTotalExcess As Decimal

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "basecaseid", v_vCaseID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCaseIncurredTotalsSQL, sSQLName:=ACGetCaseIncurredTotalsName, bStoredProcedure:=True, vResultArray:=vResultArray, bKeepNulls:=True)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return result
            End If


            For lRow As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)
                cTotalIndemnity += gPMFunctions.ToSafeCurrency(vResultArray(kTotalIndemnity, lRow))
                cTotalExpense += gPMFunctions.ToSafeCurrency(vResultArray(kTotalExpense, lRow))
                cTotalExcess += gPMFunctions.ToSafeCurrency(vResultArray(kTotalExcess, lRow))
            Next lRow

            r_vTotalIndemnity = cTotalIndemnity
            r_vTotalExpense = cTotalExpense
            r_vTotalExcess = cTotalExcess

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=CStr(m_sUsername), v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function GetSchemeMaximumTempDrivers() As Integer

        Dim result As Integer = 0
        Const kMETHOD_NAME As String = "GetSchemeMaximumTempDrivers"
        Try

            Const kFIELD_MAX_TEMP_DRIVERS As Integer = 47

            If m_lGisSchemeId <> m_lCachedGisSchemeId Then

                m_vCachedScheme = GetSchemeDetails(m_lGisSchemeId)
                m_lCachedGisSchemeId = m_lGisSchemeId
            End If

            If Informations.IsArray(m_vCachedScheme) Then
                result = gPMFunctions.ToSafeLong(CInt(m_vCachedScheme(kFIELD_MAX_TEMP_DRIVERS, 0)))
            End If

            Return result

        Catch excep As System.Exception


            bPMFunc.LogMessage(sUsername:=CStr(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Call to method " & kMETHOD_NAME & " failed.", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:=kMETHOD_NAME, vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetSchemeTempVehicleLimitedMaxGroup() As Integer

        Dim result As Integer = 0
        Const kMETHOD_NAME As String = "GetSchemeTempVehicleLimitedMaxGroup"
        Try

            Const kFIELD_MAX_TEMP_VEHICLE_MAX_GROUP As Integer = 48

            If m_lGisSchemeId <> m_lCachedGisSchemeId Then

                m_vCachedScheme = GetSchemeDetails(m_lGisSchemeId)
                m_lCachedGisSchemeId = m_lGisSchemeId
            End If

            If Informations.IsArray(m_vCachedScheme) Then
                result = gPMFunctions.ToSafeLong(CInt(m_vCachedScheme(kFIELD_MAX_TEMP_VEHICLE_MAX_GROUP, 0)))
            End If

            Return result

        Catch excep As System.Exception


            bPMFunc.LogMessage(sUsername:=CStr(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Call to method " & kMETHOD_NAME & " failed.", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:=kMETHOD_NAME, vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetSchemeTadTavCombination() As Integer

        Dim result As Integer = 0
        Const kMETHOD_NAME As String = "GetSchemeTadTavCombination"
        Try

            Const kFIELD_TAD_TAV_COMBINATION As Integer = 49

            If m_lGisSchemeId <> m_lCachedGisSchemeId Then

                m_vCachedScheme = GetSchemeDetails(m_lGisSchemeId)
                m_lCachedGisSchemeId = m_lGisSchemeId
            End If

            If Informations.IsArray(m_vCachedScheme) Then
                result = gPMFunctions.ToSafeLong(CInt(m_vCachedScheme(kFIELD_TAD_TAV_COMBINATION, 0)))
            End If

            Return result

        Catch excep As System.Exception


            bPMFunc.LogMessage(sUsername:=CStr(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Call to method " & kMETHOD_NAME & " failed.", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:=kMETHOD_NAME, vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         GetRiskClaim
    ' Description:
    '
    ' ***************************************************************** '
    Private Function GetRiskClaim(ByVal v_lPolicyId As Integer, ByRef v_vRiskArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetRiskClaim"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add stored proc input params

        m_lReturn = m_oDatabase.Parameters.Add("gis_policy_link_id", CStr(v_lPolicyId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        ' Execute Proc
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskForGisPolicyLinkSQL, sSQLName:=ACGetRiskForGisPolicyLinksName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=v_vRiskArray)
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then result = gPMConstants.PMEReturnCode.PMFalse
        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPartyCode
    '
    ' Description: Derives PartyCode
    ' ***************************************************************** '
    Private Function GetPartyCode(ByRef r_vPartyCode As Object) As Integer

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".GetPartyCode")



        Dim vArray(,) As Object

        result = gPMConstants.PMEReturnCode.PMTrue

        If m_bUnderwriting Then

            bPMAddParameter.AddParameterLite(m_oDatabase, "party_cnt", m_lPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartyCodeSQL, sSQLName:=ACGetPartyCodeName, bStoredProcedure:=ACGetPartyCodeStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

        End If



        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Informations.IsArray(vArray) Then


            r_vPartyCode = vArray(0, 0)
        End If

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".GetPartyCode")

        Return result

    End Function
    'Add the SMS Details to the Event_log table
    Public Function AddExportableMessage(ByVal sMessageType As String, ByVal sDestination As String, ByVal sMessage As String) As Integer

        Dim result As Integer = 0
        Dim sGroupcode As String = ""
        Const kMETHOD_NAME As String = "AddExportableMessage"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = GetClaimCnt()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                bPMFunc.LogError(v_sUsername:=CStr(m_sUsername), v_sClass:=ACClass, v_sMethod:=kMETHOD_NAME, r_lFunctionReturn:=result)
            End If

            m_lReturn = GetInsuranceFileCntForClaim()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                bPMFunc.LogError(v_sUsername:=CStr(m_sUsername), v_sClass:=ACClass, v_sMethod:=kMETHOD_NAME, r_lFunctionReturn:=result)
            End If

            m_lReturn = GetInsuranceFolderCnt(m_lInsuranceFileCnt)
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                bPMFunc.LogError(v_sUsername:=CStr(m_sUsername), v_sClass:=ACClass, v_sMethod:=kMETHOD_NAME, r_lFunctionReturn:=result)
            End If

            m_lReturn = GetInsuranceFileDetails()

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                bPMFunc.LogError(v_sUsername:=CStr(m_sUsername), v_sClass:=ACClass, v_sMethod:=kMETHOD_NAME, r_lFunctionReturn:=result)
            End If

            'Check for sMessageType
            m_lReturn = CheckEventTypeGroup(sMessageType, sGroupcode)
            If sGroupcode = "" Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            m_oDatabase.Parameters.Clear()


            'developer guide no 85. 

            m_lReturn = m_oDatabase.Parameters.Add("event_cnt", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add("party_cnt", CStr(m_lPartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add("insurance_folder_cnt", CStr(m_lInsuranceFolderCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add("insurance_file_cnt", CStr(m_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add("claim_cnt", CStr(m_lClaimCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add("document_cnt", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add("old_address_cnt", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add("new_address_cnt", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add("campaign_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add("document_type_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add("report_type_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add("event_type_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add("user_id", CStr(m_iUserID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add("event_date", DateTime.Now.ToString, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            m_lReturn = m_oDatabase.Parameters.Add("Description", sMessage, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add("old_party_type_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add("event_log_subject_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add("event_type_code", sMessageType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add("account_key", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add("fsa_complaint_folder_cnt", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add("short_description", sDestination, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add("priority_code", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add("is_completed", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add("peril_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add("Batch_ID", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertEventLogSQL, sSQLName:=ACInsertEventLogName, bStoredProcedure:=True)

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                bPMFunc.LogError(v_sUsername:=CStr(m_sUsername), v_sClass:=ACClass, v_sMethod:=kMETHOD_NAME, r_lFunctionReturn:=result)
            End If
            Return result

        Catch excep As System.Exception


            bPMFunc.LogMessage(sUsername:=CStr(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Call to method " & kMETHOD_NAME & " failed.", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:=kMETHOD_NAME, vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function

    Private Function CheckEventTypeGroup(ByVal sMessageType As String, ByRef sGroupcode As String) As Integer

        Dim result As Integer = 0
        Dim vResult(,) As Object
        Const kMETHOD_NAME As String = "CheckEventTypeGroup"



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add("code", sMessageType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetEventTypeGroupSQL, sSQLName:=ACGetEventTypeGroupName, vResultArray:=vResult, bStoredProcedure:=True)

        If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
            bPMFunc.LogError(v_sUsername:=CStr(m_sUsername), v_sClass:=ACClass, v_sMethod:=kMETHOD_NAME, r_lFunctionReturn:=result)
            Return result
        End If

        If Informations.IsArray(vResult) Then

            sGroupcode = CStr(vResult(0, 0))
        End If

        Return result

    End Function



    '1.12 Wr25
    Public Function SetRenewalProductCode(ByVal sProductCode As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetRenewalProductCode"

        Try




            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lGISDataModelType = 2 Then
                m_lReturn = GetInsuranceFileCntForClaim()
            Else
                m_lReturn = GetInsuranceFileCnt()
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_oDatabase.Parameters.Clear()

            m_oDatabase.Parameters.Add("ifileCnt", CStr(m_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            m_oDatabase.Parameters.Add("product_code", sProductCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACSetRenewalProductCodeSQL, sSQLName:=ACSetRenewalProductCodeName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=CStr(m_sUsername), v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function GetRenewalProductCode(ByRef vProductCode As String) As Integer

        Dim result As Integer = 0
        Dim vResult(,) As Object
        Const kMethodName As String = "GetRenewalProductCode"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lGISDataModelType = 2 Then
                m_lReturn = GetInsuranceFileCntForClaim()
            Else
                m_lReturn = GetInsuranceFileCnt()
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_oDatabase.Parameters.Clear()

            m_oDatabase.Parameters.Add("ifileCnt", CStr(m_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRenewalProductCodeSQL, sSQLName:=ACGetRenewalProductCodeName, vResultArray:=vResult, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            ElseIf Informations.IsArray(vResult) Then

                vProductCode = CStr(vResult(0, 0))
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=CStr(m_sUsername), v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    '****************************************************************************
    ' Get values from specified columns (v_vReturnColumn)
    ' if v_sKeyColumn and v_sKeyValue is passed in then use it as criteria otherwise
    ' whole table is selected
    ' if v_vReturnColumn is not an array then single value is returned
    ' Note: v_lColumnType = PMLong, PMString etc
    '
    '****************************************************************************
    Private Function GetValueFromTable(ByVal v_sTableName As String, ByVal v_vReturnColumn As Object, ByVal v_sKeyColumn As String, ByVal v_sKeyValue As String, ByVal v_iDataType As Integer, ByRef r_vResult(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As New StringBuilder
        Dim vResultArray(,) As Object


        result = gPMConstants.PMEReturnCode.PMTrue


        sSQL = New StringBuilder("SELECT DISTINCT ")

        If Informations.IsArray(v_vReturnColumn) Then


            For lCount As Integer = 0 To v_vReturnColumn.GetUpperBound(0)

                sSQL.Append(CStr(v_vReturnColumn(lCount)) & ",")
            Next

            'get rid of last comma
            sSQL = New StringBuilder(sSQL.ToString().Substring(0, sSQL.ToString().Length - 1))

        Else

            sSQL.Append(CStr(v_vReturnColumn))
        End If

        sSQL.Append(Strings.ChrW(13) & Strings.ChrW(10) & "FROM " & v_sTableName & Strings.ChrW(13) & Strings.ChrW(10))

        If v_sKeyColumn <> "" Then
            sSQL.Append("WHERE " & v_sKeyColumn & " = {KeyValue}")

            m_oDatabase.Parameters.Clear()

            Select Case v_iDataType
                Case gPMConstants.PMEDataType.PMString
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=v_sKeyValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)
                Case gPMConstants.PMEDataType.PMLong
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CInt(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)
                Case gPMConstants.PMEDataType.PMInteger
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CInt(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                Case gPMConstants.PMEDataType.PMDouble
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CDbl(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                Case gPMConstants.PMEDataType.PMDate
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CDate(v_sKeyValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                Case gPMConstants.PMEDataType.PMBoolean
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CBool(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                Case gPMConstants.PMEDataType.PMCurrency
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CDec(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

            End Select

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:="Get single value from table", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray, bKeepNulls:=False)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            Return result
        End If

        'are we returning an array or a single value?
        If Informations.IsArray(v_vReturnColumn) Then


            r_vResult = vResultArray
        Else
            If Informations.IsArray(vResultArray) Then


                r_vResult = vResultArray(0, 0)
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If
        End If
        Return result

    End Function
    Private Function CheckForValidUDL(ByVal sTableName As String, ByRef bValid As Boolean) As Long
        Const kMethodName As String = "CheckForValidUDL"
        Dim vResultArray(,) As Object

        CheckForValidUDL = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        ' Add the table name parameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:="table", vValue:=sTableName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetGISUDLDetailSQL, sSQLName:=ACGetGISUDLDetailName, bStoredProcedure:=ACGetGISUDLDetailProc, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Informations.IsArray(vResultArray) Then
            bValid = True
        End If

    End Function

    Private Function GetClaimDetails(ByVal v_lClaimId As Long) As Long

        Const kMethodName As String = "GetClaimDetails"
        Dim vResultArray(,) As Object
        Dim oBusiness As bCLMChangeClaimStatus.Business

        GetClaimDetails = gPMConstants.PMEReturnCode.PMTrue

        oBusiness = New bCLMChangeClaimStatus.Business
        m_lReturn = oBusiness.Initialise(sUsername:=CStr(m_sUsername), sPassword:=CStr(m_sPassword), iUserID:=CInt(m_iUserID), iSourceId:=CInt(m_iSourceID), iLanguageID:=CInt(m_iLanguageID), iCurrencyID:=CInt(m_iCurrencyID), iLogLevel:=CInt(m_iLogLevel), sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)

        ' Get the risk details for the risk pertaining to a policy
        m_lReturn = oBusiness.GetClaimDetails(v_lClaimId:=v_lClaimId, r_vResultArray:=vResultArray)
        ' Check for errors.
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GetClaimDetails = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        If Informations.IsArray(vResultArray) Then
            m_dtClaimLossDate = DateTime.Parse(ToSafeDate(vResultArray(4, 0), DateTime.Now).ToString).ToString("MMM dd yyyy")
        End If

        oBusiness = Nothing
        Exit Function
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sColumnName"></param>
    ''' <param name="sValue"></param>
    ''' <param name="nClaimID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateClaimSearchResultsField(ByVal sColumnName As String, ByVal sValue As String,
                                                  Optional ByVal nClaimID As Integer = 0) As Integer

        Dim nResult As Integer = 0
        Dim sSQL As String = String.Empty
        Dim vArray(,) As Object = Nothing

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            sSQL = " UPDATE CLAIM SET " & sColumnName & " =   '" & sValue & "' WHERE CLAIM_ID = " & nClaimID
            nResult = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Update Claim Search feild ", bStoredProcedure:=False,
                                            lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)
            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateClaimSearchResultsField Failed", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="UpdateClaimSearchResultsField", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult

        End Try
    End Function

    Public Function UpdateClaimRecoveries(ByVal oClaimsPerils As Object) As Integer
        Const kMethodName As String = "UpdateClaimRecoveries"

        Try
            Dim nClaimCnt As Integer = UBound(oClaimsPerils, 2)
            For iClaimIndex As Integer = 0 To nClaimCnt
                Dim nRecoveryCnt As Integer = UBound(oClaimsPerils(kPERIL_ArrayOfRecoveries, iClaimIndex), 2)
                For iRecoveryIndex As Integer = 0 To nRecoveryCnt
                    If ToSafeCurrency(oClaimsPerils(kPERIL_ArrayOfRecoveries, iClaimIndex)(kRECOVERY_ThisReserve, iRecoveryIndex)) <> 0 And
                        ToSafeInteger(oClaimsPerils(kPERIL_ArrayOfRecoveries, iClaimIndex)(kRECOVERY_IsUpdated, iRecoveryIndex)) = 1 Then

                        ' Clear Down Database Parameters
                        m_oDatabase.Parameters.Clear()
                        ' Add Required Stored Procedure Parameters
                        AddParameterLite(m_oDatabase, "lClaimPerilID", ToSafeLong(oClaimsPerils(kPERIL_PerilID, iClaimIndex)), PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
                        AddParameterLite(m_oDatabase, "lRecoveryTypeID", ToSafeLong(oClaimsPerils(kPERIL_ArrayOfRecoveries, iClaimIndex)(kRECOVERY_ReserveType, iRecoveryIndex)), PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
                        AddParameterLite(m_oDatabase, "crReserveAmount", ToSafeCurrency(oClaimsPerils(kPERIL_ArrayOfRecoveries, iClaimIndex)(kRECOVERY_ThisReserve, iRecoveryIndex)), PMEParameterDirection.PMParamInput, PMEDataType.PMCurrency)

                        If oClaimsPerils(kPERIL_ArrayOfRecoveries, iClaimIndex)(kRECOVERY_PartyTypeId, iRecoveryIndex) IsNot Nothing Then
                            AddParameterLite(m_oDatabase, "lRecoveryPartyTypeID", ToSafeLong(oClaimsPerils(kPERIL_ArrayOfRecoveries, iClaimIndex)(kRECOVERY_PartyTypeId, iRecoveryIndex)), PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
                        End If

                        If oClaimsPerils(kPERIL_ArrayOfRecoveries, iClaimIndex)(kRECOVERY_PartyKey, iRecoveryIndex) IsNot Nothing Then
                            AddParameterLite(m_oDatabase, "nRecoveryPartyCnt", ToSafeLong(oClaimsPerils(kPERIL_ArrayOfRecoveries, iClaimIndex)(kRECOVERY_PartyKey, iRecoveryIndex)), PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
                        End If

                        If m_oDatabase.SQLAction(
                            sSQL:=kSaveClaimRecoverySQL,
                            sSQLName:=kSaveClaimRecoveryName,
                            bStoredProcedure:=True) <> PMEReturnCode.PMTrue Then
                            Throw New Exception
                        End If
                    End If
                Next iRecoveryIndex
            Next iClaimIndex

            Return PMEReturnCode.PMTrue
        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=CStr(m_sUsername),
                               iType:=PMELogLevel.PMLogError,
                               sMsg:="Method Failed!", vClass:=ACClass,
                               vMethod:=kMethodName,
                               excep:=ex)
            Return PMEReturnCode.PMFalse
        End Try

    End Function

    ''' <summary>
    ''' New function to be called from rule files to set the renewal status
    ''' If you wish to create a work manager task, use the existing AddTaskToWorkManager function in your rule file
    ''' </summary>
    ''' <param name="nRenewalCnt">This is the version that is under renewal (i.e. not the live version)</param>
    ''' <param name="nRenewalStatus">This is the status to which you wish to set the renewal</param>
    ''' <returns></returns>
    Public Function SetRenewalStatus(ByVal nRenewalCnt As Integer, ByVal nRenewalStatus As Integer) As Integer

        Const kMethodName As String = "SetRenewalStatus"
        Dim oBusiness As bSIRRenewal.Business

        Try

            SetRenewalStatus = PMEReturnCode.PMTrue

            oBusiness = New bSIRRenewal.Business
            m_lReturn = oBusiness.Initialise(sUsername:=CStr(m_sUsername), sPassword:=CStr(m_sPassword), iUserID:=CInt(m_iUserID), iSourceID:=CInt(m_iSourceID), iLanguageID:=CInt(m_iLanguageID), iCurrencyID:=CInt(m_iCurrencyID), iLogLevel:=CInt(m_iLogLevel), sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                SetRenewalStatus = PMEReturnCode.PMFalse
                Return SetRenewalStatus
            End If

            m_lReturn = oBusiness.SetRenewalStatus(nRenewalCnt, nRenewalStatus)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                SetRenewalStatus = PMEReturnCode.PMFalse
                Return SetRenewalStatus
            End If

            Return PMEReturnCode.PMTrue

        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=CStr(m_sUsername),
                               iType:=PMELogLevel.PMLogError,
                               sMsg:="Method Failed!", vClass:=ACClass,
                               vMethod:=kMethodName,
                               excep:=ex)
            Return PMEReturnCode.PMFalse
        Finally
            oBusiness.Dispose()
            oBusiness = Nothing
        End Try

    End Function

    Public Function GetPartyServiceLevel(ByVal r_vInsuranceFileCnt As Integer, ByVal r_vBatchRenewalJobID As Integer, ByRef r_vServiceLevelID As Integer) As Integer
        Dim result As Integer = 0

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "batch_renewal_job_id", r_vBatchRenewalJobID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", r_vInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)

            bPMAddParameter.AddParameterLite(m_oDatabase, "service_level_id", r_vServiceLevelID, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong, False)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartyServiceLevelSQL, sSQLName:=ACGetPartyServiceLevelName, bStoredProcedure:=ACGetPartyServiceLevelStored, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            r_vServiceLevelID = Convert.ToInt32(m_oDatabase.Parameters.Item("service_level_id").Value)

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyCnt", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function


End Class
