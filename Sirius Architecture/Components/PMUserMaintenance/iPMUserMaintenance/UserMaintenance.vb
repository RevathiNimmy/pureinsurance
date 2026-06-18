Option Strict Off
Option Explicit On
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Globalization
Imports System.IO
Imports System.Linq
Imports System.ServiceModel
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Artinsoft.VB6.VB
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports SharedFiles
Imports SSP.PureInsuranceRestAPIHandler
Imports SSP.PureInsuranceRestAPIHandler.BaseClasses
Partial Public Class frmUserMaintenance
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Name: frmUserMaintenance
    '
    ' Date:
    '
    ' Description:
    '
    ' Edit History:
    ' AG161204  - Added RefreshListOnSecurityTab method
    ' AG161204  - Added LoadDomains method
    ' AG161204  - Added LoadDomainUsers method
    ' AG161204  - Added PopulateSystemSecurity method
    ' AG161204  - Added ValidateUserPassword method
    ' AG161204  - Added SetProductOption method
    ' AG161204  - Added ValidateUserMapping method
    ' AG161204  - Added ValidateAdminRights method
    ' RKS170105 - Done a few bug fixes
    ' RKS110205 - Done a few bug fixes
    '           - New user can only be added with a valid password, however
    '             password can be change to blank in Mixed or Unifed mode if
    '             user is mapped with a valid domain account
    ' MKR180205 - PN 18859
    ' MKR210205 - PN 18584 : If security model is Unified Login then giving a warning
    '              message if the user is not still matched
    ' CJB270705 - PN 22629 : Changed Save and txtFilter_Change to effectively click listview
    '              item after save (and so load globals with correct data) to prevent corruption
    '              when ok is pressed after pressing Apply. (Bug caused by PN21860). Also only
    '              attempt save if a user is selected (in cmdOK_Click).
    ' CJB280705 - PN 22703 : Enable Duplicate Claim Notification Functionality for Broking in Form_Load.
    ' CJB080805 - PN 23013 : Changed 'Can Edit Claims' to 'Can View Claims' as you could only view them from
    '              client manager anyway! Note that it remains as Edit Claim in the business component and
    '              any SQL.
    ' CJB090805 - PN 23035 : New Client Manager Security option for 'Can Delete Policy'
    ' CJB110805 - PN 23130 : Removed check from cmdOK_Click to exit if no user selected as they may not have
    '              selected one but just changed system security details!
    ' ***************************************************************** '

    Public m_ofrmPassword As frmPassword


    Public m_ofrmUserLink As frmUserLink


    Public m_ofrmUserStatus As frmUserStatus


    Public m_ofrmUser As frmUser
    Private Const ACClass As String = "frmUserMaintenance"

    Private Const s_defFilterText As String = "<All Users>"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_bNewUser As Boolean
    Private m_iUser As Integer
    Private m_sUsername As String = ""
    Private m_dtDateCreated As Date
    Private m_dtLastLogin As Date
    Private m_sPassword As String = ""
    Private m_vOldPassword As String = ""
    Private m_sEncrypt As String = ""
    Private m_bPasswordChanged As Boolean
    Private m_dtPasswordchange As Date
    Private m_dtUserEffectiveDate As Date
    Private m_iIsDeleted As gPMConstants.PMEReturnCode
    Private m_bDeletedUser As Boolean
    Private m_bUndeletedUser As Boolean
    Private m_lDeletedUsers As Integer
    Private m_lFilteredUsers As Integer
    Private m_iLinkToBroking As Integer
    Private m_iIsPrinterChangable As CheckState
    Private m_sFullname As String = ""
    Private m_sInitials As String = ""
    Private m_sTelephoneNumber As String = ""
    Private m_sExtensionNumber As String = ""
    Private m_sMobileNumber As String = ""
    Private m_sSignatureFile As String = ""
    Private m_sFaxNumber As String = ""
    Private m_lPartyHandlerId As Integer
    Private m_lClaimHandlerId As Integer
    Private m_lOtherPartyId As Integer '(RC) WR34
    Private m_lJobTitleId As Integer
    Private m_sEmailAddress As String = ""
    Private m_sSSOPreferredName As String = ""
    Private m_sTitle As String = ""
    Private m_lPartyCnt As Integer
    Private m_sDefPrinter As String = ""
    Private m_lXPos As Integer
    Private m_lYPos As Integer
    Private m_lReturn As Integer
    Private m_vUserArray As Object
    Private m_iNextUserID As Integer
    Private m_bInitialised As Boolean
    Private m_sSysOption As String = ""
    Private m_vSourceArray As Object
    Private m_vUserGroupInfo(,) As Object
    Private m_vUserSourceInfo(,) As Object
    Private m_vUserRiskGroupInfo(,) As Object
    Private m_vUserPartyInfo(,) As Object
    Private m_sSignatureDir As String = ""
    Private m_vJobTitles As Object
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object


    Private m_vEnableFSACompliance As Object
    Private m_sUnderwritingOrAgency As String = ""
    Private m_bFirstDisplay As Boolean
    Private m_vAlternativeIdentifier As String = ""

    'User Authority
    Private m_iCanWriteOff As CheckState
    Private m_cWriteOffAmount As Decimal
    Private m_iUnrestrictedEnquiry As CheckState
    Private m_iUnrestrictedUpdate As CheckState
    Private m_nViewBatchProcessStatus As CheckState
    Private m_nCanExtractClientData As CheckState
    Private m_iCanPerformBrokerTransfer As CheckState
    Private m_iHasTransWriteOffAuthority As Integer
    Private m_cTransWriteOffAmount As Decimal
    Private m_iFeeDiscount As Integer
    Private m_iHasRefundAuthority As Integer
    Private m_iHasTransferAuthority As Integer
    Private m_iHasPaymentsAuthority As CheckState
    Private m_cPaymentsAmount As Decimal
    Private m_iHasClaimPaymentsAuthority As CheckState
    Private m_cClaimPaymentsAmount As Decimal
    Private m_iOverrideDate As CheckState
    Private m_iOverrideRate As CheckState
    Private m_iDuplicateClaimOverride As CheckState
    Private m_iOverridePrePolicyDate As CheckState
    Private m_iOverridePrePolicyRate As CheckState
    Private m_cCurrencyLossGainLimit As Object
    Private m_vLossGainCurrencyID As Object
    Private m_vWriteOffCurrencyID As Object
    Private m_vTransWriteOffCurrencyID As Object
    Private m_vPaymentsCurrencyID As Object
    Private m_vClaimPaymentsCurrencyID As Object

    Private m_iHasManualJournalAuthority As CheckState
    Private m_cManualJournalAmount As Decimal
    Private m_vManualJournalCurrencyID As Object

    Private m_iPostingPeriod As CheckState '(RC) IH-UDPP
    Private m_iUserCanChangeReserves As CheckState '(RC) QBENZ001
    Private m_iUserCanAddRemoveRatingSections As CheckState
    Private m_iUserCanEditExistingRatingSections As CheckState
    Private m_iOverrideChequeNumber As Integer
    'Start(Saurabh Agrawal)- Tech Spec WR3 User Level RI Display Restriction(5.1.1)
    Private m_iDisplayReinsuranceScreen As Integer
    Private m_iDisplayClaimReinsurance As Integer
    'End(Saurabh Agrawal)- Tech Spec WR3 User Level RI Display Restriction(5.1.1)

    Private m_iEditDefaultCommission As Integer
    Private m_nEditDefaultCommissionMTA As Integer
    Private m_nEditDefaultCommissionMTC As Integer
    Private m_nEditDefaultCommissionMTR As Integer
    Private m_nEditAgentDuringMTAMTC As Integer
    Private m_nEditDefaultCommissionNBRN As Integer
    Private m_iCanChangeInstalmentPlanDefaultCurrecny As Integer
    'Party View
    Private m_iViewClientManager As CheckState
    Private m_iAgentMaintenance As CheckState
    Private m_iAccountHandler As CheckState
    Private m_iAccountExecutive As CheckState
    Private m_iInsurerMaintenance As CheckState
    Private m_iOtherPartyMaintenance As CheckState

    '2005 Client Manager Security
    Private m_sEnableClientManagerSecurity As String = ""
    Private m_iIsViewClient As CheckState
    Private m_iIsEditClient As CheckState
    Private m_iIsDeleteClient As CheckState
    Private m_iIsEditPolicy As CheckState
    Private m_iIsDeletePolicy As CheckState
    Private m_iIsViewClaim As CheckState
    Private m_iIsEditFinancePlan As CheckState
    Private m_iIsRaiseDebit As CheckState
    Private m_iIsRaiseCredit As CheckState
    Private m_iIsRaiseFee As CheckState
    Private m_iIsRaiseCash As CheckState
    Private m_iIsReverseTransactions As CheckState
    Private m_iIsPerformAllocations As CheckState
    Private m_iIsReverseAllocations As CheckState
    Private m_iCanReverseReplaceTransactions As Integer
    Private m_iIsRaiseManualDID As CheckState
    Private m_iIsEditSchemePolicy As CheckState
    '2005 Client Manager Security End

    Private m_iCanMakeLiveInvoice As CheckState
    Private m_iCanMakeLiveInstalments As CheckState
    Private m_iCanMakeLivePaynow As CheckState
    Private m_iCanMakeLiveBankGuarantee As Integer
    Private m_iHasPaynowWriteOffAuthority As CheckState
    Private m_iPaynowWriteOffCurrencyID As Integer
    Private m_cPaynowWriteOffAmount As Decimal
    'Private m_iPaynowBankAccount As Integer

    Private m_lJobBasis As Integer
    Private m_dPercentHoursWorked As Double
    Private m_bSiriusUser As Boolean

    Private m_vDateDeleted As Object

    Private m_ctlTabFirstLast(,) As Control
    Private m_bFormLoading As Boolean


    Private m_vSystemSecurityModel As Object

    Private m_iIsRecommender As Integer
    Private m_iRecommenderCurrencyID As Integer
    Private m_cRecommenderCurrencyAmt As Decimal

    'Payment Maintenance
    Private m_iCanReverseAllocations As Integer
    Private m_iTimePeriodForReversal As Integer

    'PN23693
    Private m_sAD_OU_Path As String = ""
    Private m_sAD_OU_Domain As String = ""
    Private m_bLDAPQueryProvided As Boolean

    Private m_colDomainUsersData As Collection
    Private m_ArrDeletedUsers() As Object

    Private m_bPopulateUserParty As Boolean

    Private m_oFormFields As iPMFormControl.FormFields
    Private m_iCanMakeLiveCashDeposit As Integer 'Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling

    Private Const JOB_BASIS_FULLTIME As Integer = 1
    Private Const JOB_BASIS_PARTTIME As Integer = 2

    Private Const ARR_RG_RGID As Integer = 0
    Private Const ARR_RG_RGCODE As Integer = 1
    Private Const ARR_RG_RGDESC As Integer = 2
    Private Const ARR_RG_FSACOMPID As Integer = 3
    Private Const ARR_RG_USERID As Integer = 4
    Private Const ARR_RG_STATUSID As Integer = 5
    Private Const ARR_RG_STATUSDESC As Integer = 6
    Private Const ARR_RG_PASSEDEXAM As Integer = 7
    Private Const ARR_RG_DATEPASSEDEXAM As Integer = 8
    '1.12 PLICO 45
    Private Const MTA_AUTHORITY_WITH_CLAIMS As Integer = 3
    Private Const MTA_AUTHORITY_WITHOUT_CLAIMS As Integer = 2
    Private Const MTA_AUTHORITY_NOT_ALLOWED As Integer = 1
    Private m_iMTAAuthority As Integer
    Private m_bCheckIsRecommendClaimPaymentEnabledatProduct As Boolean
    Private m_iUserCanDebugDynamicLogicScripts As Integer
    Private m_iUserServerScriptsRunInDebug As Integer
    Private m_iCanBackdateCollectionDate As Integer
    Private m_nEnableDefaultCommissionNBRN As Integer
    Private m_nEnableDefaultCommissionMTA As Integer
    Private m_nEnableDefaultCommissionMTC As Integer
    Private m_nEnableDefaultCommissionMTR As Integer
    Private m_bIsEditCommission As Boolean
    Private m_bIsTempPassword As Boolean
    Private m_bIsUserTempPassword As Boolean
    Private m_oListItem As ListViewItem
    Private m_bIsClearCache As Boolean = False

    Private m_nAllowReceiptReversal As Integer
    Private m_nInstalmentStatus As Integer

    Dim m_sOldPassword As String = ""
    Private m_nCanEditInstalmentDate As CheckState
    Private m_nEditInstalmentDateByNoOfDays As Integer
    Private m_sUniqueId As String
    Private m_sScreenHierarchy As String
    Private m_iModifiedUserId As String
    Private m_sVoidTransaction As String
	 Public Property VoidTransaction() As Integer
        Get
            Return m_sVoidTransaction
        End Get
        Set(ByVal Value As Integer)
            m_sVoidTransaction = Value
        End Set
    End Property
	

    Public WriteOnly Property AD_OU_Path() As String
        Set(ByVal Value As String)
            m_sAD_OU_Path = Value
        End Set
    End Property
    Public WriteOnly Property AD_OU_Domain() As String
        Set(ByVal Value As String)
            m_sAD_OU_Domain = Value
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

    Public Property SourceArray() As Object
        Get
            Return m_vSourceArray
        End Get
        Set(ByVal Value As Object)


            m_vSourceArray = Value
        End Set
    End Property

    Public WriteOnly Property SysOption() As String
        Set(ByVal Value As String)
            m_sSysOption = Value
        End Set
    End Property

    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property
    Public Property CanWriteOff() As Integer
        Get
            Return m_iCanWriteOff
        End Get
        Set(ByVal Value As Integer)
            m_iCanWriteOff = Value
        End Set
    End Property
    Public Property UnrestrictedEnquiry() As Integer
        Get
            Return m_iUnrestrictedEnquiry
        End Get
        Set(ByVal Value As Integer)
            m_iUnrestrictedEnquiry = Value
        End Set
    End Property
    Public Property UnrestrictedUpdate() As Integer
        Get
            Return m_iUnrestrictedUpdate
        End Get
        Set(ByVal Value As Integer)
            m_iUnrestrictedUpdate = Value
        End Set
    End Property
    Public Property ViewBatchProcessStatus() As Integer
        Get
            Return m_nViewBatchProcessStatus
        End Get
        Set(ByVal Value As Integer)
            m_nViewBatchProcessStatus = Value
        End Set
    End Property
    Public Property WriteOffAmount() As Decimal
        Get
            Return m_cWriteOffAmount
        End Get
        Set(ByVal Value As Decimal)
            m_cWriteOffAmount = Value
        End Set
    End Property
    Public Property Password() As String
        Get
            Return m_sPassword
        End Get
        Set(ByVal Value As String)
            m_sPassword = Value
        End Set
    End Property
    Public Property OldPassword() As String
        Get
            Return m_vOldPassword
        End Get
        Set(ByVal Value As String)
            m_vOldPassword = Value
        End Set
    End Property
    Public Property DateCreated() As Date
        Get
            Return m_dtDateCreated
        End Get
        Set(ByVal Value As Date)
            m_dtDateCreated = Value
        End Set
    End Property
    Public Property UserName() As String
        Get
            Return m_sUsername
        End Get
        Set(ByVal Value As String)
            m_sUsername = Value
        End Set
    End Property
    Public Property UserEffectiveDate() As Date
        Get
            Return m_dtUserEffectiveDate
        End Get
        Set(ByVal Value As Date)
            m_dtUserEffectiveDate = Value
        End Set
    End Property
    Public Property NewUser() As Boolean
        Get
            Return m_bNewUser
        End Get
        Set(ByVal Value As Boolean)
            m_bNewUser = Value
        End Set
    End Property
    Public Property LastLogin() As Date
        Get
            Return m_dtLastLogin
        End Get
        Set(ByVal Value As Date)
            m_dtLastLogin = Value
        End Set
    End Property
    Public Property IsPrinterChangable() As Integer
        Get
            Return m_iIsPrinterChangable
        End Get
        Set(ByVal Value As Integer)
            m_iIsPrinterChangable = Value
        End Set
    End Property
    Public Property IsDeleted() As Integer
        Get
            Return m_iIsDeleted
        End Get
        Set(ByVal Value As Integer)
            m_iIsDeleted = Value
        End Set
    End Property
    Public Property Fullname() As String
        Get
            Return m_sFullname
        End Get
        Set(ByVal Value As String)
            m_sFullname = Value
        End Set
    End Property
    Public Property Initials() As String
        Get
            Return m_sInitials
        End Get
        Set(ByVal Value As String)
            m_sInitials = Value
        End Set
    End Property
    Public Property TelephoneNumber() As String
        Get
            Return m_sTelephoneNumber
        End Get
        Set(ByVal Value As String)
            m_sTelephoneNumber = Value
        End Set
    End Property
    Public Property ExtensionNumber() As String
        Get
            Return m_sExtensionNumber
        End Get
        Set(ByVal Value As String)
            m_sExtensionNumber = Value
        End Set
    End Property
    Public Property MobileNumber() As String
        Get
            Return m_sMobileNumber
        End Get
        Set(ByVal Value As String)
            m_sMobileNumber = Value
        End Set
    End Property
    Public Property FaxNumber() As String
        Get
            Return m_sFaxNumber
        End Get
        Set(ByVal Value As String)
            m_sFaxNumber = Value
        End Set
    End Property
    Public Property SignatureFile() As String
        Get
            Return m_sSignatureFile
        End Get
        Set(ByVal Value As String)
            m_sSignatureFile = Value
        End Set
    End Property
    Public Property EmailAddress() As String
        Get
            Return m_sEmailAddress
        End Get
        Set(ByVal Value As String)
            m_sEmailAddress = Value
        End Set
    End Property
    Public Property SSOPreferredName() As String
        Get
            Return m_sSSOPreferredName
        End Get
        Set(ByVal Value As String)
            m_sSSOPreferredName = Value
        End Set
    End Property


    Public Property Title() As String
        Get
            Return m_sTitle
        End Get
        Set(ByVal Value As String)
            m_sTitle = Value
        End Set
    End Property
    Public Property DefPrinter() As String
        Get
            Return m_sDefPrinter
        End Get
        Set(ByVal Value As String)
            m_sDefPrinter = Value
        End Set
    End Property
    Public ReadOnly Property PasswordChanged() As Boolean
        Get
            Return m_bPasswordChanged
        End Get
    End Property
    Public WriteOnly Property PasswordChnaged() As Boolean
        Set(ByVal Value As Boolean)
            m_bPasswordChanged = Value
        End Set
    End Property
    Public Property PasswordChange() As Date
        Get
            Return m_dtPasswordchange
        End Get
        Set(ByVal Value As Date)
            m_dtPasswordchange = Value
        End Set
    End Property
    Public Property PartyHandlerId() As Integer
        Get
            Return m_lPartyHandlerId
        End Get
        Set(ByVal Value As Integer)
            m_lPartyHandlerId = Value
        End Set
    End Property
    Public Property ClaimHandlerId() As Integer
        Get
            Return m_lClaimHandlerId
        End Get
        Set(ByVal Value As Integer)
            m_lClaimHandlerId = Value
        End Set
    End Property

    '(RC) WR34
    Public Property OtherPartyId() As Integer
        Get
            Return m_lOtherPartyId
        End Get
        Set(ByVal Value As Integer)
            m_lOtherPartyId = Value
        End Set
    End Property

    Public Property JobTitleId() As Integer
        Get
            Return m_lJobTitleId
        End Get
        Set(ByVal Value As Integer)
            m_lJobTitleId = Value
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

    Public Property JobBasis() As Integer
        Get
            Return m_lJobBasis
        End Get
        Set(ByVal Value As Integer)
            m_lJobBasis = Value
        End Set
    End Property

    Public Property PercentHoursWorked() As Double
        Get
            Return m_dPercentHoursWorked
        End Get
        Set(ByVal Value As Double)
            m_dPercentHoursWorked = Value
        End Set
    End Property


    Public Property DateDeleted() As Object
        Get
            Return m_vDateDeleted
        End Get
        Set(ByVal Value As Object)

            m_vDateDeleted = Value
        End Set
    End Property
    'Ends
    ''Party View
    Public Property ViewClientManager() As Integer
        Get
            Return m_iViewClientManager
        End Get
        Set(ByVal Value As Integer)
            m_iViewClientManager = Value
        End Set
    End Property

    Public Property ViewAgent() As Integer
        Get
            Return m_iAgentMaintenance
        End Get
        Set(ByVal Value As Integer)
            m_iAgentMaintenance = Value
        End Set
    End Property

    Public Property ViewAccountHandle() As Integer
        Get
            Return m_iAccountHandler
        End Get
        Set(ByVal Value As Integer)
            m_iAccountHandler = Value
        End Set
    End Property

    Public Property ViewAccountExecutive() As Integer
        Get
            Return m_iAccountExecutive
        End Get
        Set(ByVal Value As Integer)
            m_iAccountExecutive = Value
        End Set
    End Property

    Public Property ViewInsurer() As Integer
        Get
            Return m_iInsurerMaintenance
        End Get
        Set(ByVal Value As Integer)
            m_iInsurerMaintenance = Value
        End Set
    End Property

    Public Property ViewOtherParty() As Integer
        Get
            Return m_iOtherPartyMaintenance
        End Get
        Set(ByVal Value As Integer)
            m_iOtherPartyMaintenance = Value
        End Set
    End Property

    Public Property AllowReceiptReversal() As Integer
        Get
            Return m_nAllowReceiptReversal
        End Get
        Set(ByVal Value As Integer)
            m_nAllowReceiptReversal = Value
        End Set
    End Property

    Public Property CurrencyLossGainLimit() As Object
        Get
            Return m_cCurrencyLossGainLimit
        End Get
        Set(ByVal Value As Object)
            m_cCurrencyLossGainLimit = Value
        End Set
    End Property

    Public Property ModifiedUserId() As Integer
        Get
            Return m_iModifiedUserId
        End Get
        Set(ByVal Value As Integer)
            m_iModifiedUserId = Value
        End Set
    End Property

    Public Function InitialForm() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_colDomainUsersData = New Collection()

            m_bPasswordChanged = False
            m_lReturn = iPMFunc.GetSystemSecurityModel(m_vSystemSecurityModel)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to retrieve System Security Model", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialForm", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If Convert.ToString(m_vSystemSecurityModel) = "" Then
                m_vSystemSecurityModel = 0
            End If


            Return result

        Catch excep As System.Exception



            'Error Section

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the business", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Refreshes the listview box with data held in the business, not the database
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RefreshList() As Integer

        Dim nResult As Integer
        Dim nRow As Integer
        Dim oListitem As ListViewItem
        Dim bInclude As Boolean
        Dim sfilter As String

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_lDeletedUsers = 0
            m_lFilteredUsers = 0


            g_oBusiness.CurrentRecord = 0

            'Clear the items in the listview box
            lvwUsers.Items.Clear()

            sfilter = txtFilter.Text.Trim().ToUpper() 'Set row count

            nRow = -1
            ReDim m_ArrDeletedUsers(0)
            Do
                'Get id of first record

                m_lReturn = g_oBusiness.GetNext(vUserId:=m_iUser, vUsername:=m_sUsername, vPassword:=m_sPassword,
                                                vPasswordChangeDate:=m_dtPasswordchange, vPartyCnt:=m_lPartyCnt,
                                                vDateCreated:=m_dtDateCreated, vLastLogin:=m_dtLastLogin,
                                                vIsDeleted:=m_iIsDeleted, vEffectiveDate:=m_dtUserEffectiveDate,
                                                vServerPrinter:=m_sDefPrinter,
                                                vIsPrinterChangeable:=m_iIsPrinterChangable,
                                                vEmailAddress:=m_sEmailAddress, vFullName:=m_sFullname,
                                                vTitle:=m_sTitle, vInitials:=m_sInitials,
                                                vSignatureFile:=m_sSignatureFile,
                                                vTelephoneNumber:=m_sTelephoneNumber,
                                                vExtensionNumber:=m_sExtensionNumber, vMobileNumber:=m_sMobileNumber,
                                                vFaxNumber:=m_sFaxNumber, vJobTitleId:=m_lJobTitleId,
                                                vClaimHandlerId:=m_lClaimHandlerId,
                                                vPartyHandlerId:=m_lPartyHandlerId, vOtherPartyId:=m_lOtherPartyId,
                                                vAlternativeIdentifier:=m_vAlternativeIdentifier, vSSOPreferredName:=m_sSSOPreferredName)

                'Exit if you get an invalid response
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Do
                End If

                'DJM 20/02/2004 PN10542 : Add the for each user as this is meant to link to the database.
                nRow += 1

                bInclude = False

                If (m_iIsDeleted = gPMConstants.PMEReturnCode.PMTrue) And
                    (chkHideDeleted.CheckState = CheckState.Unchecked) Then

                    m_lDeletedUsers += 1
                    'PN 27640 08/03/2006
                    ReDim Preserve m_ArrDeletedUsers(m_ArrDeletedUsers.GetUpperBound(0) + 1)

                    m_ArrDeletedUsers(m_ArrDeletedUsers.GetUpperBound(0)) = m_sUsername

                Else
                    If sfilter.ToUpper() = s_defFilterText.ToUpper() Then
                        bInclude = True
                    ElseIf Len(sfilter) > 1 And Strings.Left(UCase(Trim$(m_sUsername)), Len(sfilter)) = sfilter Then
                        bInclude = True
                        'DJM 20/02/2004 PN10542 : Allow filter for first letter.
                    ElseIf sfilter.Length = 1 And m_sUsername.Trim().ToUpper().Substring(0, 1) = sfilter Then
                        bInclude = True
                    End If

                    If bInclude Then
                        nRow = m_iUser
                        oListitem = lvwUsers.Items.Add("L" & nRow, m_sUsername.Trim(), "")
                        oListitem.ImageKey = "user"

                        If m_iIsDeleted = gPMConstants.PMEReturnCode.PMTrue Then


                            oListitem.ForeColor = Color.Gray
                        End If

                    Else

                        m_lFilteredUsers += 1

                    End If

                End If

            Loop

            Return nResult

        Catch excep As System.Exception


            'Error Section

            nResult = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Refresh user details ", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    '****************************************************************************
    '
    ' Function Name: PopListBox
    '
    ' Description: Populates listview box
    '
    '****************************************************************************

    Public Function PopListBox() As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Getdetails from business
            'm_lReturn = g_oBusiness.GetDetails()

            m_lReturn = g_oBusiness.GetAllUsers(r_vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Refresh the List
            'PopListBox = RefreshList

            Return FillList(vResultArray:=vResultArray)

        Catch excep As System.Exception



            'Error Section

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load user details from database", vApp:=ACApp, vClass:=ACClass, vMethod:="PopListBox", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '****************************************************************************
    '
    ' Function Name: ClearUserDetails
    '
    ' Description: Clear All User Details Ready For New User
    '
    '****************************************************************************

    Public Function ClearUserDetails() As Integer

        Dim result As Integer = 0
        Dim r_sOptionValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_iUser = 0

            With Me



                .pnlUsername.Name = ""
                .lblUsername.Text = ""
                .txtFullName.Text = ""
                .txtInitials.Text = ""
                .ddTitle.Text = ""
                .cboJobTitle.SelectedIndex = -1
                .txtEmailAddress.Text = ""
                .txtSSOPreferredName.Text = ""
                .txtTelephoneNumber.Text = ""
                .txtExtensionNumber.Text = ""
                .txtMobileNumber.Text = ""
                .txtFaxNumber.Text = ""


                .pnlEffectiveDate.Name = ""

                .lblEffectiveDatePanel.Text = ""


                .pnlLastLogin.Name = ""

                .lblLastLoginPanel.Text = ""


                .pnlPasswordChange.Name = ""

                .lblPasswordChangePanel.Text = ""


                .pnlDomainUserName.Name = ""

                .lblDomainUserNamePanel.Text = ""
                .cboPrinter.SelectedIndex = -1
                .chkPrinterChangable.CheckState = CheckState.Unchecked



                .pnlAccHandler(0).Name = ""
                .lblAccHandlerPanel.Text = ""
                .chkAccHandler(0).CheckState = CheckState.Unchecked


                .pnlAccHandler(1).Name = ""
                .chkAccHandler(1).CheckState = CheckState.Unchecked
                .lblAccExecutivePanel.Text = ""


                .pnlAccHandler(2).Name = ""
                .chkAccHandler(2).CheckState = CheckState.Unchecked
                .lblClaimsHandlerPanel.Text = ""


                .pnlAgent(0).Name = ""

                lblAgentPanel.Text = ""
                .chkAgent(0).CheckState = CheckState.Unchecked


                .pnlAgent(1).Name = ""

                lblInsurerPanel.Text = ""
                .chkAgent(1).CheckState = CheckState.Unchecked


                .pnlOtherParty.Name = "" '(RC) WR34

                .lblOtherPartypanel.Text = ""

                .chkOtherParty.CheckState = CheckState.Unchecked

                .chkUnrestrictedEnquiry.CheckState = CheckState.Unchecked
                .chkUnrestrictedUpdate.CheckState = CheckState.Unchecked
                .chkViewBatchProcessStatus.CheckState = CheckState.Unchecked
                .chkCanExtractClientData.CheckState = CheckState.Unchecked

                .chkWriteOffs.CheckState = CheckState.Unchecked
                .txtWriteOff.Text = ""
                .chkClaimPayments.CheckState = CheckState.Unchecked
                .txtClaimPayments.Text = ""
                .chkPayments.CheckState = CheckState.Unchecked
                .txtPayments.Text = ""

                .ChkManualJournal.CheckState = CheckState.Unchecked
                .txtjournalAmount.Text = ""

                .txtCurrencyLossGainLimit.Text = ""
                .chkOverrideDate.CheckState = CheckState.Unchecked
                .chkOverrideRate.CheckState = CheckState.Unchecked

                .chkDuplicateClaimOverride.CheckState = CheckState.Unchecked
                .chkPostingPeriod.CheckState = CheckState.Unchecked '(RC) IH-UDPP
                .chkOverrideChequeNumber.CheckState = CheckState.Unchecked
                .chkUserCanChangeReserves.CheckState = CheckState.Unchecked '(RC) QBENZ001

                .chkOverridePrePolicyDate.CheckState = CheckState.Unchecked
                .chkOverridePrePolicyRate.CheckState = CheckState.Unchecked

                .chkCanPerformBrokerTransfer.CheckState = CheckState.Unchecked
                .cboVoidTransaction.SelectedIndex = -1

                .imgSignature.Image = Nothing

                '2005 Client Manager Security
                .chkIsViewClient.CheckState = CheckState.Checked
                .chkIsEditClient.CheckState = CheckState.Checked
                .chkIsDeleteClient.CheckState = CheckState.Checked
                .chkIsEditPolicy.CheckState = CheckState.Checked
                .chkIsDeletePolicy.CheckState = CheckState.Checked
                .chkIsViewClaim.CheckState = CheckState.Checked
                .chkIsEditFinancePlan.CheckState = CheckState.Checked
                .chkIsRaiseDebit.CheckState = CheckState.Checked
                .chkIsRaiseCredit.CheckState = CheckState.Checked
                .chkIsRaiseFee.CheckState = CheckState.Checked
                .chkIsRaiseCash.CheckState = CheckState.Checked
                .chkIsReverseTransactions.CheckState = CheckState.Checked
                .chkIsPerformAllocations.CheckState = CheckState.Checked
                .chkIsReverseAllocations.CheckState = CheckState.Checked
                .chkCanReverseReplaceTransactions.CheckState = CheckState.Checked
                .chkIsRaiseManualDID.CheckState = CheckState.Checked
                .chkSiriusUser.CheckState = CheckState.Unchecked
                m_bSiriusUser = False

                'Party Edit
                .chkIsViewClientManager.CheckState = CheckState.Unchecked
                .chkAgentMaintenance.CheckState = CheckState.Unchecked
                .chkAccountHandler.CheckState = CheckState.Unchecked
                .chkAccountExecutive.CheckState = CheckState.Unchecked
                .chkInsurerMaintenance.CheckState = CheckState.Unchecked
                .chkOtherPartyMaintenance.CheckState = CheckState.Unchecked
                .chkRecommender.CheckState = CheckState.Unchecked
                .txtRecommendAmount.Text = ""

                ' To give priority to System Option over User Authority Option
                m_lReturn = iPMFunc.RetrieveSingleSystemOption(v_iOptionNumber:=5000, r_sOptionValue:=r_sOptionValue, v_iSourceID:=g_iSourceID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Return result
                End If

                If r_sOptionValue <> "1" Then
                    chkIsViewClientManager.CheckState = CheckState.Checked
                    chkIsViewClientManager.Enabled = False
                End If

                'Payment Maintenance
                .chkReverseAllocation.CheckState = CheckState.Unchecked
                .txtTimePeriod.Text = ""

                .chkReceiptReversal.CheckState = CheckState.Unchecked
                .chkInstalmentStatus.CheckState = CheckState.Unchecked
                .chkEditInstalment.CheckState = CheckState.Unchecked
            End With

            'PN20044 Reset Party Handler ID
            m_lPartyHandlerId = 0
            m_lClaimHandlerId = 0
            m_sPassword = ""

            m_ofrmPassword = New frmPassword

            m_ofrmPassword.txtConfirmPassword.Text = ""

            m_ofrmPassword.txtNewPassword.Text = ""
            m_iIsDeleted = gPMConstants.PMEReturnCode.PMFalse
            'm_bUndeletedUser = True
            m_bDeletedUser = False
            m_vAlternativeIdentifier = ""
            m_lOtherPartyId = 0 '(RC) WR34

            m_lReturn = PopulateUserCompanies()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lReturn = PopulateUserGroups()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lReturn = PopulateUserRiskGroups()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            'Flaot Balance and Pre-Payment Work

            chkInvoice.CheckState = CheckState.Checked
            chkPayNow.CheckState = CheckState.Checked
            chkInstalments.CheckState = CheckState.Checked
            chkHasPaynowWriteOffAuthority.CheckState = CheckState.Unchecked
            chkEditAgentDuringMTAMTC.CheckState = CheckState.Checked
            chkEditDefaultCommission.CheckState = CheckState.Checked
            EnablePayNowWriteOff()
            EnableAuthorityReversePayment()
            EnableClaimPayments()
            EnableManualJournalPayments()
            EnableRecommendPayments()
            EnableWriteOff()
            EnablePayments()

            '1.12 PLICO45
            cboMTAAuthority.SelectedIndex = -1

            Return result

        Catch excep As System.Exception



            'Error Section

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear user details", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearUserDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function UpdateAgentStatus(ByRef iAgent As Integer) As Integer

        If chkAgent(iAgent).CheckState = CheckState.Checked Then
            lblAgentYN(iAgent).Text = "Yes"
        ElseIf (chkAgent(iAgent).CheckState = CheckState.Unchecked) Then
            lblAgentYN(iAgent).Text = "No"


            pnlAgent(iAgent).Name = ""


            If (iAgent = 0) Then
                lblAgentPanel.Text = ""
            Else
                lblInsurerPanel.Text = ""
            End If
        End If
        cmdAgent(iAgent).Enabled = (chkAgent(iAgent).CheckState = CheckState.Checked)

        If chkAgent(iAgent).CheckState = CheckState.Checked Then
            If iAgent = 0 Then
                chkAgent(1).Enabled = False
                lblAgentYN(1).Enabled = False
                chkAgent(1).CheckState = CheckState.Unchecked
                lblAgentYN(1).Text = "No"
            Else
                chkAgent(0).Enabled = False
                lblAgentYN(0).Enabled = False
                chkAgent(0).CheckState = CheckState.Unchecked
                lblAgentYN(0).Text = "No"
            End If
        Else
            chkAgent(0).Enabled = True
            chkAgent(1).Enabled = True
            lblAgentYN(0).Enabled = True
            lblAgentYN(1).Enabled = True
        End If
        If chkAgent(0).CheckState = CheckState.Checked Or chkAgent(1).CheckState = CheckState.Checked Then
            chkOtherParty.CheckState = CheckState.Unchecked
            chkOtherParty.Enabled = False
            lblOtherPartyYN.Enabled = False
        Else
            chkOtherParty.Enabled = True
            lblOtherPartyYN.Enabled = True
        End If
    End Function


    ''' <summary>
    ''' EditUser()-Writes edited details of an existing user to the database
    ''' </summary>
    ''' <param name="oListitem"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Private Function EditUser(ByRef oListitem As ListViewItem) As Integer

        Dim nResult As Integer = 0
        Dim sKey As String

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            If oListitem Is Nothing Then
                Return nResult
            End If

            sKey = oListitem.Name
            m_iUser = Conversion.Val(sKey.Substring(sKey.Length - (sKey.Length - 1)))


            g_oBusiness.CurrentRecord = 0

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


            m_lReturn = g_oBusiness.getdetails(vUserId:=m_iUser)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            m_lReturn = g_oBusiness.GetNext(vUserId:=m_iUser, vUsername:=m_sUsername, vPassword:=m_sPassword,
                                            vPasswordChangeDate:=m_dtPasswordchange, vPartyCnt:=m_lPartyCnt,
                                            vDateCreated:=m_dtDateCreated, vLastLogin:=m_dtLastLogin,
                                            vIsDeleted:=m_iIsDeleted, vEffectiveDate:=m_dtUserEffectiveDate,
                                            vServerPrinter:=m_sDefPrinter,
                                            vIsPrinterChangeable:=m_iIsPrinterChangable,
                                            vEmailAddress:=m_sEmailAddress, vFullName:=m_sFullname,
                                            vTitle:=m_sTitle, vInitials:=m_sInitials,
                                            vSignatureFile:=m_sSignatureFile, vTelephoneNumber:=m_sTelephoneNumber,
                                            vExtensionNumber:=m_sExtensionNumber, vMobileNumber:=m_sMobileNumber,
                                            vFaxNumber:=m_sFaxNumber, vJobTitleId:=m_lJobTitleId,
                                            vClaimHandlerId:=m_lClaimHandlerId, vPartyHandlerId:=m_lPartyHandlerId,
                                            vOtherPartyId:=m_lOtherPartyId,
                                            vAlternativeIdentifier:=m_vAlternativeIdentifier, vJobBasis:=m_lJobBasis,
                                            vPercentHoursWorked:=m_dPercentHoursWorked, vSiriusUser:=m_bSiriusUser,
                                            vDateDeleted:=m_vDateDeleted, o_bIsUserTempPassword:=m_bIsUserTempPassword,
                                            vOldPassword:=m_vOldPassword, vSSOPreferredName:=m_sSSOPreferredName)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            'Exit if you get an invalid response
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            PopulateUserFields()

            Return nResult

        Catch excep As System.Exception
            'Error Section

            nResult = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Edit User Details", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUser", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAvailablePrinters
    '
    ' Description: Gets and returns the list of Printers, and the
    '              default printer.
    '
    ' ***************************************************************** '
    Public Function GetAvailablePrinters(ByRef r_vPrinterArray As Object, ByRef r_sDefaultPrinter As String) As Integer

        Dim result As Integer = 0
        Dim lNoOfPrinters As Integer = 0
        Dim printersList As New List(Of String)

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Initialise
        r_sDefaultPrinter = String.Empty 
        r_vPrinterArray = Nothing

        Try
            ' Size the Printer array accordingly
            r_sDefaultPrinter = PrinterHelper.Printer.DeviceName.Trim()

            If String.IsNullOrEmpty(r_sDefaultPrinter) Then
                Return result
            End If

                    lNoOfPrinters = PrinterHelper.Printers.Count
    Catch
    'continue instead of throwing an error.
    End Try
            Try
        If lNoOfPrinters < 1 Then
            r_vPrinterArray = ""
            Return result
        Else


            ' Collect each unique printer name
            For Each oPrinter As Object In PrinterHelper.Printers
                Dim name As String = oPrinter.DeviceName.Trim()

                ' Avoid duplicates
                If Not printersList.Contains(name) Then
                    printersList.Add(name)
                End If
            Next

            ' Assign to output array
            r_vPrinterArray = printersList.ToArray()
        End If

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the List of Printers", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAvailablePrinters", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

        Return result



    End Function

    Private Function PopCombo() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim vPrinterName As Object

        Select Case Information.Err().Number
            Case Is < 0
                Conversion.ErrorToString(5)
            Case 1
                GoTo err_PopCombo
        End Select

        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = GetAvailablePrinters(r_vPrinterArray:=vPrinterName, r_sDefaultPrinter:=m_sDefPrinter)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        cboPrinter.Items.Clear()

        cboPrinter.Items.Add("Default")

        If Information.IsArray(vPrinterName) Then
            ' Loop through the data array

            For lRow As Integer = vPrinterName.GetLowerBound(0) To vPrinterName.GetUpperBound(0)
                'Set the data in the combobox

                cboPrinter.Items.Add(CStr(vPrinterName(lRow)))
            Next lRow
        End If

        '1.12 plico 45
        With cboMTAAuthority
            .Items.Clear()
            Dim listIndex As Integer = .Items.Add(New VB6.ListBoxItem("Not authorised to process MTA/MTC", 1))
            listIndex = .Items.Add(New VB6.ListBoxItem("MTA/MTC with no claims", 2))
            listIndex = .Items.Add(New VB6.ListBoxItem("MTA/MTC with claims", 3))
        End With

        Return result

err_PopCombo:

        'Error Section

        result = gPMConstants.PMEReturnCode.PMError

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to populate the Printer list box", vApp:=ACApp, vClass:=ACClass, vMethod:="PopCombo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: PopulateUserCompanies
    '
    ' Description:
    '
    ' History: 18/09/03 -DC Created
    '
    ' ***************************************************************** '
    Private Function PopulateUserCompanies() As Integer

        Dim result As Integer = 0
        Dim oListitem As ListViewItem
        Dim sSourceCode As String = ""
        Dim sTemp, sKey As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = g_oBusiness.GetUserSourceInfo(r_lUserId:=m_iUser, r_vUserSourceInfo:=m_vUserSourceInfo)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If no Companies then do nothing
            If Not Information.IsArray(m_vUserSourceInfo) Then
                Return result
            End If

            lvwBranches.Items.Clear()
            lvwSelectedBranches.Items.Clear()

            For lRow As Integer = m_vUserSourceInfo.GetLowerBound(1) To m_vUserSourceInfo.GetUpperBound(1)

                sTemp = CStr(m_vUserSourceInfo(2, lRow)).Trim()
                sKey = CStr(m_vUserSourceInfo(0, lRow)).Trim()

                If CStr(m_vUserSourceInfo(3, lRow)) = "0" Then

                    'DC171003 -PN7617 -wrong way round selected/non selected branches
                    oListitem = lvwSelectedBranches.Items.Add("B" & sKey, sTemp, "")

                Else

                    'DC171003 -PN7617 -wrong way round selected/non selected branches
                    oListitem = lvwBranches.Items.Add("B" & sKey, sTemp, "")

                End If

            Next lRow

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateUserCompanies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateUserCompanies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PopulateUserParty
    '
    ' Description:
    '
    ' History: 18/09/03 -DC Created
    '
    ' ***************************************************************** '
    Private Function PopulateUserParty() As Integer

        Dim result As Integer = 0
        Dim lPartyHandlerId As Integer
        Dim lClaimHandlerId As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DC180204 PN10508 to perform click event slightly differently when first displaying party details
            m_bFirstDisplay = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = g_oBusiness.GetUserPartyInfo(r_lUserId:=m_iUser, r_vUserPartyInfo:=m_vUserPartyInfo)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If no party then do nothing
            If Not Information.IsArray(m_vUserPartyInfo) Then
                Return result
            End If
            lPartyHandlerId = m_lPartyHandlerId
            lClaimHandlerId = m_lClaimHandlerId

            chkAccHandler(0).CheckState = CheckState.Unchecked
            chkAccHandler(1).CheckState = CheckState.Unchecked
            chkAccHandler(2).CheckState = CheckState.Unchecked
            chkAgent(0).CheckState = CheckState.Unchecked
            chkAgent(1).CheckState = CheckState.Unchecked

            chkOtherParty.CheckState = CheckState.Unchecked


            pnlAccHandler(0).Name = ""
            lblAccHandlerPanel.Text = ""


            pnlAccHandler(1).Name = ""
            lblAccExecutivePanel.Text = ""


            pnlAccHandler(2).Name = ""
            lblClaimsHandlerPanel.Text = ""


            pnlAgent(0).Name = ""
            lblAgentPanel.Text = ""


            pnlAgent(1).Name = ""
            lblInsurerPanel.Text = ""



            pnlOtherParty.Name = "" '(RC) WR34


            lblOtherPartypanel.Text = ""
            m_lPartyHandlerId = lPartyHandlerId
            If m_lPartyHandlerId <> 0 Then
                If CStr(m_vUserPartyInfo(3, 0)).Trim() = "AH" OrElse CStr(m_vUserPartyInfo(3, 0)).Trim() = "HC" Then
                    chkAccHandler(0).CheckState = CheckState.Checked


                    pnlAccHandler(0).Name = m_vUserPartyInfo(2, 0).Trim()
                    lblAccHandlerPanel.Text = m_vUserPartyInfo(2, 0).Trim()
                End If
                If CStr(m_vUserPartyInfo(3, 0)).Trim() = "CO" OrElse CStr(m_vUserPartyInfo(3, 0)).Trim() = "HC" Then
                    chkAccHandler(1).CheckState = CheckState.Checked


                    pnlAccHandler(1).Name = m_vUserPartyInfo(2, 0).Trim()
                    lblAccExecutivePanel.Text = m_vUserPartyInfo(2, 0).Trim()
                End If
            End If

            If m_lPartyCnt <> 0 Then
                If CStr(m_vUserPartyInfo(1, 0)).Trim() = "IN" Then
                    chkAgent(1).CheckState = CheckState.Checked


                    pnlAgent(1).Name = m_vUserPartyInfo(0, 0).Trim()
                    lblInsurerPanel.Text = m_vUserPartyInfo(0, 0).Trim()
                ElseIf CStr(m_vUserPartyInfo(1, 0)).Trim() = "AG" Then
                    chkAgent(0).CheckState = CheckState.Checked


                    pnlAgent(0).Name = m_vUserPartyInfo(0, 0).Trim()
                    lblAgentPanel.Text = m_vUserPartyInfo(0, 0).Trim()
                Else
                    chkOtherParty.CheckState = CheckState.Checked


                    pnlOtherParty.Name = m_vUserPartyInfo(0, 0).Trim()
                    lblOtherPartypanel.Text = m_vUserPartyInfo(0, 0).Trim()
                End If

            End If

            m_lClaimHandlerId = lClaimHandlerId
            If m_lClaimHandlerId <> 0 Then
                chkAccHandler(2).CheckState = CheckState.Checked


                pnlAccHandler(2).Name = m_vUserPartyInfo(4, 0).Trim()
                lblClaimsHandlerPanel.Text = m_vUserPartyInfo(4, 0).Trim()
            End If

            '(RC) WR34
            If m_lOtherPartyId <> 0 Then

                Me.chkOtherParty.Checked = 1
                Me.pnlOtherParty.Name = Trim$(m_vUserPartyInfo(5, 0))
                Me.lblOtherPartypanel.Text = Trim$(m_vUserPartyInfo(5, 0))
            Else
                Me.chkOtherParty.Checked = 0
            End If

            'DC180204 PN10508 to perform click event slightly differently when first displaying party details
            m_bFirstDisplay = gPMConstants.PMEReturnCode.PMFalse

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateUserParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateUserParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' PopulateUserGroups
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PopulateUserGroups() As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim sTemp As String = ""
        Dim nTemp As Integer
        Dim oListitem As ListViewItem
        Dim sKey As String = ""

        Try

            m_lReturn = g_oBusiness.GetUserGroupInfo(r_lUserId:=m_iUser, r_vUserGroupInfo:=m_vUserGroupInfo)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            ' If no user groups fo nothing
            If Not Information.IsArray(m_vUserGroupInfo) Then
                Return nResult
            End If

            lvwGroups.Items.Clear()
            lvwSelectedGroups.Items.Clear()

            For lRow As Integer = m_vUserGroupInfo.GetLowerBound(1) To m_vUserGroupInfo.GetUpperBound(1)

                sTemp = CStr(m_vUserGroupInfo(2, lRow)).Trim()
                sKey = CStr(m_vUserGroupInfo(0, lRow)).Trim()

                If CStr(m_vUserGroupInfo(3, lRow)) <> "0" Then

                    nTemp = CInt(m_vUserGroupInfo(4, lRow))

                    If nTemp = 1 Then


                        oListitem = lvwSelectedGroups.Items.Add("K" & sKey, sTemp, "")
                        oListitem.ImageKey = "supervisor"
                    Else


                        oListitem = lvwSelectedGroups.Items.Add("K" & sKey, sTemp, "")
                        oListitem.ImageKey = "user"
                    End If

                Else

                    oListitem = lvwGroups.Items.Add("K" & sKey, sTemp, "")
                    oListitem.ImageKey = "user"
                End If

            Next lRow

            Return nResult

        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="PopulateUserGroups Failed",
                               vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateUserGroups",
                               vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PopulateUserRiskGroups
    '
    ' Description:
    '
    ' History: 18/09/2003 DC -Created
    '
    ' ***************************************************************** '
    Private Function PopulateUserRiskGroups() As Integer

        Dim result As Integer = 0
        Dim sTemp1, sTemp2 As String
        Dim oListitem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = g_oBusiness.GetUserRiskGroupInfo(r_lUserId:=m_iUser, r_vUserRiskgroupInfo:=m_vUserRiskGroupInfo)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If no user groups fo nothing
            If Not Information.IsArray(m_vUserRiskGroupInfo) Then
                Return result
            End If

            lvwRiskGroups.Items.Clear()

            For lRow As Integer = m_vUserRiskGroupInfo.GetLowerBound(1) To m_vUserRiskGroupInfo.GetUpperBound(1)

                'Risk Group Code
                '34306
                '"X" is prefixed to handle the numeric risk code as
                'listview do not accept whole numeric values in the key field
                sTemp1 = "X" & CStr(m_vUserRiskGroupInfo(ARR_RG_RGCODE, lRow)).Trim()
                'Risk Group Description
                sTemp2 = CStr(m_vUserRiskGroupInfo(ARR_RG_RGDESC, lRow)).Trim()
                oListitem = lvwRiskGroups.Items.Add(sTemp1, sTemp2, "")
                'Risk Group Id
                oListitem.Tag = CStr(lRow)

                'Status Description
                ListViewHelper.GetListViewSubItem(oListitem, 1).Text = CStr(m_vUserRiskGroupInfo(ARR_RG_STATUSDESC, lRow)).Trim()

                If gPMFunctions.ToSafeString(CStr(m_vUserRiskGroupInfo(ARR_RG_PASSEDEXAM, lRow))) = "" Then
                    ListViewHelper.GetListViewSubItem(oListitem, 2).Text = ""
                Else
                    ListViewHelper.GetListViewSubItem(oListitem, 2).Text = IIf(gPMFunctions.ToSafeBoolean(CStr(m_vUserRiskGroupInfo(ARR_RG_PASSEDEXAM, lRow))), "Yes", "No")
                End If

                If Information.IsDate(m_vUserRiskGroupInfo(ARR_RG_DATEPASSEDEXAM, lRow)) Then
                    ListViewHelper.GetListViewSubItem(oListitem, 3).Text = gPMFunctions.ToSafeString(gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, CStr(m_vUserRiskGroupInfo(ARR_RG_DATEPASSEDEXAM, lRow))))
                Else
                    ListViewHelper.GetListViewSubItem(oListitem, 3).Text = ""
                End If

            Next lRow

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateUserRiskGroups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateUserRiskGroups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' UpdateUserAuthorities
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>New Option to switch off Editing of Schemes Type policies</remarks>
    Private Function UpdateUserAuthorities() As Integer

        Dim nResult As Integer '
        Dim sScreenHierarchy As String = ""
        Try
            Dim vParams As Object

            ReDim vParams(kParamsCount)

            nResult = gPMConstants.PMEReturnCode.PMTrue

            If m_bNewUser Then

                m_lReturn = g_oAuthority.GetDetails(vUserID:=m_iUser)
                'Exit if you get an invalid response
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return nResult
                End If
                'Client Manager Security

                m_lReturn = g_oAuthority.GetNext(vHasWriteOffAuthority:=m_iCanWriteOff,
                                                 vWriteOffAmount:=m_cWriteOffAmount,
                                                 vHasUnrestrictedEnquiry:=m_iUnrestrictedEnquiry,
                                                 vHasUnrestrictedUpdate:=m_iUnrestrictedUpdate,
                                                 vFeeDiscount:=m_iFeeDiscount,
                                                 vHasTransWriteOffAuthority:=m_iHasTransWriteOffAuthority,
                                                 vTransWriteOffAmount:=m_cTransWriteOffAmount,
                                                 vHasRefundAuthority:=m_iHasRefundAuthority,
                                                 vHasTransferAuthority:=m_iHasTransferAuthority,
                                                 vHasPaymentsAuthority:=m_iHasPaymentsAuthority,
                                                 vPaymentsAmount:=m_cPaymentsAmount,
                                                 vHasClaimPaymentsAuthority:=m_iHasClaimPaymentsAuthority,
                                                 vClaimPaymentsAmount:=m_cClaimPaymentsAmount,
                                                 vOverrideDate:=m_iOverrideDate, vOverrideRate:=m_iOverrideRate,
                                                 vOverridePrePolicyDate:=m_iOverridePrePolicyDate,
                                                 vOverridePrePolicyRate:=m_iOverridePrePolicyRate,
                                                 vWriteOffCurrencyID:=m_vWriteOffCurrencyID,
                                                 vTransWriteOffCurrencyID:=m_vTransWriteOffCurrencyID,
                                                 vPaymentsCurrencyID:=m_vPaymentsCurrencyID,
                                                 vClaimPaymentsCurrencyID:=m_vClaimPaymentsCurrencyID,
                                                 vIsViewClient:=m_iIsViewClient, vIsEditClient:=m_iIsEditClient,
                                                 vIsEditPolicy:=m_iIsEditPolicy, vIsEditClaim:=m_iIsViewClaim,
                                                 vIsEditFinancePlan:=m_iIsEditFinancePlan,
                                                 vIsRaiseDebit:=m_iIsRaiseDebit, vIsRaiseCredit:=m_iIsRaiseCredit,
                                                 vIsRaiseFee:=m_iIsRaiseFee, vIsRaiseCash:=m_iIsRaiseCash,
                                                 vIsReverseTransactions:=m_iIsReverseTransactions,
                                                 vIsReverseAllocations:=m_iIsReverseAllocations,
                                                 vIsRaiseManualDID:=m_iIsRaiseManualDID,
                                                 vIsDeleteClient:=m_iIsDeleteClient,
                                                 vIsPerformAllocations:=m_iIsPerformAllocations,
                                                 vCanPerformBrokerTransfer:=m_iCanPerformBrokerTransfer,
                                                 vDuplicateClaimOverride:=m_iDuplicateClaimOverride,
                                                 vIsDeletePolicy:=m_iIsDeletePolicy,
                                                 vIsEditSchemePolicy:=m_iIsEditSchemePolicy,
                                                 vCanMakeLiveInvoice:=m_iCanMakeLiveInvoice,
                                                 vCAnMakeLiveInstalments:=m_iCanMakeLiveInstalments,
                                                 vCanMakeLivePayNow:=m_iCanMakeLivePaynow,
                                                 vHasPaynowWriteoffAuthority:=m_iHasPaynowWriteOffAuthority,
                                                 vPaynowWriteOffCurrencyID:=m_iPaynowWriteOffCurrencyID,
                                                 vPayNowWriteoffAmount:=m_cClaimPaymentsAmount,
                                                 vPostingPeriod:=m_iPostingPeriod,
                                                 vUserCanChangeReserves:=m_iUserCanChangeReserves,
                                                 vUserCanAddRemoveRatingSections:=m_iUserCanAddRemoveRatingSections,
                                                 vUserCanEditExistingRatingSections:=
                                                    m_iUserCanEditExistingRatingSections,
                                                 vIsViewClientManager:=m_iViewClientManager,
                                                 vIsViewAgentMaintenance:=m_iAgentMaintenance,
                                                 vIsViewAccountHandler:=m_iAccountHandler,
                                                 vIsViewAccountExecutive:=m_iAccountExecutive,
                                                 vIsViewInsurerMaintenance:=m_iInsurerMaintenance,
                                                 vIsViewOtherParty:=m_iOtherPartyMaintenance, vParams:=vParams,
                                                 vCurrencyLossGainLimit:=m_cCurrencyLossGainLimit,
                                                 vLossGainCurrencyID:=m_vLossGainCurrencyID,
                                                 vHasManualJournalAuthority:=m_iHasManualJournalAuthority,
                                                    vManualJournalAmount:=m_cManualJournalAmount,
                                                    vManualJournalCurrencyID:=m_vManualJournalCurrencyID,
                                                 vVoidTransaction:=m_sVoidTransaction)


            End If


            'Build the values from the UI
            m_iCanWriteOff = chkWriteOffs.CheckState
            m_cWriteOffAmount = gPMFunctions.ToSafeCurrency(txtWriteOff.Text, 0)
            m_cCurrencyLossGainLimit = gPMFunctions.ToSafeString(txtCurrencyLossGainLimit.Text, Nothing)
            m_vLossGainCurrencyID = cboLossGainCurrency.CurrencyId
            m_sVoidTransaction = cboVoidTransaction.SelectedItem

            m_iUnrestrictedEnquiry = chkUnrestrictedEnquiry.CheckState
            m_iUnrestrictedUpdate = chkUnrestrictedUpdate.CheckState
            m_iCanPerformBrokerTransfer = chkCanPerformBrokerTransfer.CheckState

            m_iHasPaymentsAuthority = chkPayments.CheckState
            m_cPaymentsAmount = gPMFunctions.ToSafeCurrency(txtPayments.Text, 0)

            m_iHasClaimPaymentsAuthority = chkClaimPayments.CheckState
            m_cClaimPaymentsAmount = gPMFunctions.ToSafeCurrency(txtClaimPayments.Text, 0)
            m_iOverrideDate = chkOverrideDate.CheckState
            m_iOverrideRate = chkOverrideRate.CheckState

            m_iDuplicateClaimOverride = chkDuplicateClaimOverride.CheckState

            m_iPostingPeriod = chkPostingPeriod.CheckState
            m_iOverrideChequeNumber = chkOverrideChequeNumber.CheckState

            m_iUserCanChangeReserves = chkUserCanChangeReserves.CheckState

            m_iUserCanAddRemoveRatingSections = chkAddRemoveRatingSections.CheckState
            m_iUserCanEditExistingRatingSections = chkEditRatingSections.CheckState

            m_iOverridePrePolicyDate = chkOverridePrePolicyDate.CheckState
            m_iOverridePrePolicyRate = chkOverridePrePolicyRate.CheckState
            m_vWriteOffCurrencyID = cboWriteOffsCurrency.CurrencyId

            m_vPaymentsCurrencyID = cboPaymentsCurrency.CurrencyId
            m_vClaimPaymentsCurrencyID = cboClaimPaymentsCurrency.CurrencyId
            m_iDisplayReinsuranceScreen = chkDisplayReinsuranceScreen.CheckState
            m_iDisplayClaimReinsurance = chkDisplayClaimReinsurance.CheckState

            m_iEditDefaultCommission = chkEditDefaultCommission.CheckState
            m_nEditDefaultCommissionNBRN = chkEditDefaultCommissionNBRN.CheckState
            m_nEditDefaultCommissionMTA = chkEditDefaultCommissionMTA.CheckState
            m_nEditDefaultCommissionMTC = chkEditDefaultCommissionMTC.CheckState
            m_nEditDefaultCommissionMTR = chkEditDefaultCommissionMTR.CheckState
            m_nEditAgentDuringMTAMTC = chkEditAgentDuringMTAMTC.CheckState
            m_iCanChangeInstalmentPlanDefaultCurrecny = chkCanChangeInstalmentPlanDefaultCurrecny.CheckState

            'Party View
            m_iViewClientManager = chkIsViewClientManager.CheckState
            m_iAgentMaintenance = chkAgentMaintenance.CheckState
            m_iAccountHandler = chkAccountHandler.CheckState
            m_iAccountExecutive = chkAccountExecutive.CheckState
            m_iInsurerMaintenance = chkInsurerMaintenance.CheckState
            m_iOtherPartyMaintenance = chkOtherPartyMaintenance.CheckState
            m_iCanMakeLiveBankGuarantee = chkBankGuarantee.CheckState
            m_iCanMakeLiveCashDeposit = chkCashDeposit.CheckState

            'Client Manager Security
            m_iIsViewClient = chkIsViewClient.CheckState
            m_iIsEditClient = chkIsEditClient.CheckState
            m_iIsDeleteClient = chkIsDeleteClient.CheckState
            m_iIsEditPolicy = chkIsEditPolicy.CheckState
            m_iIsDeletePolicy = chkIsDeletePolicy.CheckState
            m_iIsViewClaim = chkIsViewClaim.CheckState
            m_iIsEditFinancePlan = chkIsEditFinancePlan.CheckState
            m_iIsRaiseDebit = chkIsRaiseDebit.CheckState
            m_iIsRaiseCredit = chkIsRaiseCredit.CheckState
            m_iIsRaiseFee = chkIsRaiseFee.CheckState
            m_iIsRaiseCash = chkIsRaiseCash.CheckState
            m_iIsReverseTransactions = chkIsReverseTransactions.CheckState
            m_iCanReverseReplaceTransactions = chkCanReverseReplaceTransactions.CheckState
            m_iIsPerformAllocations = chkIsPerformAllocations.CheckState
            m_iIsReverseAllocations = chkIsReverseAllocations.CheckState
            m_iIsRaiseManualDID = chkIsRaiseManualDID.CheckState
            m_iIsEditSchemePolicy = chkIsEditSchemePolicy.CheckState

            m_iCanBackdateCollectionDate = chkOverrideCollectionDate.CheckState
            m_iCanMakeLiveBankGuarantee = chkBankGuarantee.CheckState

            'For future expansion
            m_iHasRefundAuthority = 0
            m_iHasTransferAuthority = 0
            m_iHasTransWriteOffAuthority = 0
            m_cTransWriteOffAmount = 0
            m_vTransWriteOffCurrencyID = m_vWriteOffCurrencyID
            m_iFeeDiscount = 0

            m_iCanMakeLiveInvoice = chkInvoice.CheckState
            m_iCanMakeLiveInstalments = chkInstalments.CheckState
            m_iCanMakeLivePaynow = chkPayNow.CheckState
            m_iHasPaynowWriteOffAuthority = chkHasPaynowWriteOffAuthority.CheckState
            m_iPaynowWriteOffCurrencyID = cboMakeLiveCurrency.CurrencyId

            If m_iHasPaynowWriteOffAuthority = CheckState.Checked Then
                m_cPaynowWriteOffAmount = ToSafeCurrency(txtPaynowWriteOffAmount.Text, 0)
            Else
                m_cPaynowWriteOffAmount = 0
            End If

            m_iIsRecommender = gPMFunctions.ToSafeInteger(CStr(chkRecommender.CheckState))
            m_iRecommenderCurrencyID = cboRecommandationCurrency.CurrencyId
            m_cRecommenderCurrencyAmt = gPMFunctions.ToSafeCurrency(txtRecommendAmount.Text, 0)


            vParams(ACIsRecommenderArrPos) = m_iIsRecommender

            vParams(ACRecommendationCurrencyArrPos) = m_iRecommenderCurrencyID

            vParams(ACRecommendationAmountArrPos) = m_cRecommenderCurrencyAmt

            'Payment Maintenance
            m_iCanReverseAllocations = gPMFunctions.ToSafeInteger(CStr(chkReverseAllocation.CheckState))
            If m_iCanReverseAllocations = 1 Then
                m_iTimePeriodForReversal = gPMFunctions.ToSafeInteger(txtTimePeriod.Text)
            Else
                m_iTimePeriodForReversal = 0
            End If

            ''Receipt Reversal
            m_nAllowReceiptReversal = gPMFunctions.ToSafeInteger(CStr(chkReceiptReversal.CheckState))
            m_nViewBatchProcessStatus = chkViewBatchProcessStatus.CheckState
            m_nCanExtractClientData = chkCanExtractClientData.CheckState

            vParams(kACCCanExtractClientData) = m_nCanExtractClientData

            vParams(ACCanReverseAllocationArrPos) = m_iCanReverseAllocations

            vParams(ACTimePeriodForReversalArrPos) = m_iTimePeriodForReversal

            vParams(ACCanReverseReplaceTransactionsArrPos) = m_iCanReverseReplaceTransactions

            vParams(ACMTAAuthorityArrPos) = m_iMTAAuthority

            vParams(ACChequeNumberArrPos) = m_iOverrideChequeNumber

            vParams(ACDisplayReinsuranceScreen) = m_iDisplayReinsuranceScreen

            vParams(ACDisplayClaimReinsurance) = m_iDisplayClaimReinsurance

            vParams(ACCanBackdateCollectionDate) = m_iCanBackdateCollectionDate

            vParams(ACMakeLiveBankGuarantee) = m_iCanMakeLiveBankGuarantee


            vParams(ACEditDefaultCommission) = m_iEditDefaultCommission

            vParams(ACUserCanChangeInstalmentDefaultCurrency) = m_iCanChangeInstalmentPlanDefaultCurrecny '16

            vParams(ACMakeLiveCashDeposit) = m_iCanMakeLiveCashDeposit

            vParams(kEditAgentDuringMTAMTC) = m_nEditAgentDuringMTAMTC

            vParams(kEditDefaultCommissionNBRN) = m_nEditDefaultCommissionNBRN

            vParams(kEditDefaultCommissionMTA) = m_nEditDefaultCommissionMTA

            vParams(kEditDefaultCommissionMTC) = m_nEditDefaultCommissionMTC

            vParams(kEditDefaultCommissionMTR) = m_nEditDefaultCommissionMTR

            vParams(ACCanReverseReceiptArrPos) = m_nAllowReceiptReversal

            vParams(kACCViewBatchProcessStatus) = m_nViewBatchProcessStatus
            'Write the values back
            m_iUserCanDebugDynamicLogicScripts = chkUserCanDebugDynamicLogicScripts.CheckState

            vParams(ACUserCanDebugDynamicLogicScripts) = m_iUserCanDebugDynamicLogicScripts
            m_iUserServerScriptsRunInDebug = chkUserServerScriptsRunInDebug.CheckState

            vParams(ACUserServerScriptsRunInDebug) = m_iUserServerScriptsRunInDebug

            m_nInstalmentStatus = chkInstalmentStatus.CheckState
            vParams(ACInstalmentStatus) = m_nInstalmentStatus

            m_nCanEditInstalmentDate = chkEditInstalment.CheckState
            vParams(ACCanEditInstalmentDueDate) = m_nCanEditInstalmentDate

            m_nEditInstalmentDateByNoOfDays = gPMFunctions.ToSafeInteger(txtEditInstalmentByNoofDays.Text, 0)
            vParams(ACEditInstalmentDateByNoOfDays) = m_nEditInstalmentDateByNoOfDays


            m_iHasManualJournalAuthority = gPMFunctions.ToSafeInteger(CStr(ChkManualJournal.CheckState))
            m_vManualJournalCurrencyID = cboJournalCurrency.CurrencyId
            m_cManualJournalAmount = gPMFunctions.ToSafeCurrency(txtjournalAmount.Text, 0)

            If m_sScreenHierarchy <> "" Then
                sScreenHierarchy = m_sScreenHierarchy
            End If

            m_lReturn = g_oAuthority.EditUpdate(lRow:=1, vUserID:=m_iUser, vHasWriteOffAuthority:=m_iCanWriteOff,
                                                vWriteOffAmount:=m_cWriteOffAmount,
                                                vHasUnrestrictedEnquiry:=m_iUnrestrictedEnquiry,
                                                vHasUnrestrictedUpdate:=m_iUnrestrictedUpdate,
                                                vFeeDiscount:=m_iFeeDiscount,
                                                vHasTransWriteOffAuthority:=m_iHasTransWriteOffAuthority,
                                                vTransWriteOffAmount:=m_cTransWriteOffAmount,
                                                vHasRefundAuthority:=m_iHasRefundAuthority,
                                                vHasTransferAuthority:=m_iHasTransferAuthority,
                                                vHasPaymentsAuthority:=m_iHasPaymentsAuthority,
                                                vPaymentsAmount:=m_cPaymentsAmount,
                                                vHasClaimPaymentsAuthority:=m_iHasClaimPaymentsAuthority,
                                                vClaimPaymentsAmount:=m_cClaimPaymentsAmount,
                                                vOverrideDate:=m_iOverrideDate, vOverrideRate:=m_iOverrideRate,
                                                vOverridePrePolicyDate:=m_iOverridePrePolicyDate,
                                                vOverridePrePolicyRate:=m_iOverridePrePolicyRate,
                                                vWriteOffCurrencyID:=m_vWriteOffCurrencyID,
                                                vTransWriteOffCurrencyID:=m_vTransWriteOffCurrencyID,
                                                vPaymentsCurrencyID:=m_vPaymentsCurrencyID,
                                                vClaimPaymentsCurrencyID:=m_vClaimPaymentsCurrencyID,
                                                vIsViewClient:=m_iIsViewClient, vIsEditClient:=m_iIsEditClient,
                                                vIsEditPolicy:=m_iIsEditPolicy, vIsEditClaim:=m_iIsViewClaim,
                                                vIsEditFinancePlan:=m_iIsEditFinancePlan,
                                                vIsRaiseDebit:=m_iIsRaiseDebit, vIsRaiseCredit:=m_iIsRaiseCredit,
                                                vIsRaiseFee:=m_iIsRaiseFee, vIsRaiseCash:=m_iIsRaiseCash,
                                                vIsReverseTransactions:=m_iIsReverseTransactions,
                                                vIsReverseAllocations:=m_iIsReverseAllocations,
                                                vIsRaiseManualDID:=m_iIsRaiseManualDID,
                                                vIsDeleteClient:=m_iIsDeleteClient,
                                                vIsPerformAllocations:=m_iIsPerformAllocations,
                                                vCanPerformBrokerTransfer:=m_iCanPerformBrokerTransfer,
                                                vDuplicateClaimOverride:=m_iDuplicateClaimOverride,
                                                vIsDeletePolicy:=m_iIsDeletePolicy,
                                                vIsEditSchemePolicy:=m_iIsEditSchemePolicy,
                                                vCanMakeLiveInvoice:=m_iCanMakeLiveInvoice,
                                                vCAnMakeLiveInstalments:=m_iCanMakeLiveInstalments,
                                                vCanMakeLivePayNow:=m_iCanMakeLivePaynow,
                                                vHasPaynowWriteoffAuthority:=m_iHasPaynowWriteOffAuthority,
                                                vPaynowWriteOffCurrencyID:=m_iPaynowWriteOffCurrencyID,
                                                vPayNowWriteoffAmount:=m_cPaynowWriteOffAmount,
                                                vPostingPeriod:=m_iPostingPeriod,
                                                vUserCanChangeReserves:=m_iUserCanChangeReserves,
                                                vUserCanAddRemoveRatingSections:=m_iUserCanAddRemoveRatingSections,
                                                vUserCanEditExistingRatingSections:=
                                                m_iUserCanEditExistingRatingSections,
                                                vIsViewClientManager:=m_iViewClientManager,
                                                vIsViewAgentMaintenance:=m_iAgentMaintenance,
                                                vIsViewAccountHandler:=m_iAccountHandler,
                                                vIsViewAccountExecutive:=m_iAccountExecutive,
                                                vIsViewInsurerMaintenance:=m_iInsurerMaintenance,
                                                vIsViewOtherParty:=m_iOtherPartyMaintenance, vParams:=vParams,
                                                vHasViewbatchProcessStatus:=m_nViewBatchProcessStatus,
                                                vCurrencyLossGainLimit:=m_cCurrencyLossGainLimit,
                                                vLossGainCurrencyID:=m_vLossGainCurrencyID,
                                                vHasManualJournalAuthority:=m_iHasManualJournalAuthority,
                                                vManualJournalAmount:=m_cManualJournalAmount,
                                                vManualJournalCurrencyID:=m_vManualJournalCurrencyID,
                                                sUniqueId:=m_sUniqueId, sScreenHierarchy:=sScreenHierarchy,
                                                iModifiedByUserId:=m_iModifiedUserId,
                                                vVoidTransaction:=m_sVoidTransaction)
         


            m_lReturn = g_oAuthority.Update()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Cannot Update User Authority Details", "User Authority Details")
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception


            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateUserAuthorities Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUserAuthorities", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateRiskGroups
    '
    ' Description:
    '
    ' History: 18/09/2003 DC -Created
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (UpdateUserRiskGroups) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function UpdateUserRiskGroups() As Integer
    '
    'Dim result As Integer = 0
    'Dim lFSAUserCompetencyId As Integer
    '
    'Dim oListitem As ListViewItem
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'For 'i As Integer = m_vUserRiskGroupInfo.GetLowerBound(1) To m_vUserRiskGroupInfo.GetUpperBound(1)
    '
    'If gPMFunctions.ToSafeLong(CStr(m_vUserRiskGroupInfo(ARR_RG_STATUSID, i))) > 0 Then
    '
    'lFSAUserCompetencyId = gPMFunctions.ToSafeLong(CStr(m_vUserRiskGroupInfo(ARR_RG_FSACOMPID, i)))
    '


    'm_lReturn = g_oBusiness.UpdateUserRiskGroupInfo(v_lUserId:=m_iUser, v_lRiskGroupId:=gPMFunctions.ToSafeLong(CStr(m_vUserRiskGroupInfo(ARR_RG_RGID, i))), v_lFSAUserStatusId:=gPMFunctions.ToSafeLong(CStr(m_vUserRiskGroupInfo(ARR_RG_STATUSID, i))), v_bPassedExam:=gPMFunctions.ToSafeBoolean(CStr(m_vUserRiskGroupInfo(ARR_RG_PASSEDEXAM, i))), v_vDatePassedExam:=IIf(Information.IsDate(m_vUserRiskGroupInfo(ARR_RG_DATEPASSEDEXAM, i)), m_vUserRiskGroupInfo(ARR_RG_DATEPASSEDEXAM, i), DBNull.Value), r_lFSAUserCompetencyId:=lFSAUserCompetencyId)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'MessageBox.Show("Cannot Update User Risk Group Details", "User Risk Group Details")
    'End If
    '
    'm_vUserRiskGroupInfo(ARR_RG_FSACOMPID, i) = lFSAUserCompetencyId
    '
    'End If
    '
    'Next i
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateUserRiskGroups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUserRiskGroups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateUserGroups
    '
    ' Description:
    '
    ' History: 18/09/2003 DC -Created
    '
    ' ***************************************************************** '

    Private Function UpdateUserGroups() As Integer

        Dim result As Integer = 0
        Dim oListitem As ListViewItem
        Dim iIsSupervisor As Integer
        Dim sScreenHierarchy As String = ""

        Try
            If m_sScreenHierarchy <> "" Then
                sScreenHierarchy = m_sScreenHierarchy
            End If
            'Remove Any Unselected Groups
            For i As Integer = lvwGroups.Items.Count To 1 Step -1

                For j As Integer = m_vUserGroupInfo.GetLowerBound(1) To m_vUserGroupInfo.GetUpperBound(1)
                    If CInt(CStr(m_vUserGroupInfo(0, j)).Trim()) = CInt(Mid(lvwGroups.Items.Item(i - 1).Name, 2, 5)) And CStr(m_vUserGroupInfo(3, j)).Trim() <> "0" Then

                        m_lReturn = g_oBusiness.UpdateUserGroupInfo(r_lUserId:=m_iUser, r_lPMUserGroupId:=CInt(Mid(lvwGroups.Items.Item(i - 1).Name, 2, 5)), r_iMode:=0, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=sScreenHierarchy)

                    End If

                Next j

            Next i

            'Add/Update Selected Groups
            For i As Integer = lvwSelectedGroups.Items.Count To 1 Step -1

                oListitem = lvwSelectedGroups.Items.Item(i - 1)

                For j As Integer = m_vUserGroupInfo.GetLowerBound(1) To m_vUserGroupInfo.GetUpperBound(1)

                    If CInt(CStr(m_vUserGroupInfo(0, j)).Trim()) = CInt(Mid(oListitem.Name, 2, 5)) Then



                        If oListitem.ImageKey = "supervisor" Then
                            iIsSupervisor = 1
                        Else
                            iIsSupervisor = 0
                        End If

                        If CStr(m_vUserGroupInfo(3, j)).Trim() = "0" Or (iIsSupervisor <> CInt(CStr(m_vUserGroupInfo(4, j)).Trim())) Then

                            'New User Group Selected

                            m_lReturn = g_oBusiness.UpdateUserGroupInfo(r_lUserId:=m_iUser, r_lPMUserGroupId:=CInt(Mid(oListitem.Name, 2, 5)), r_iIsSupervisor:=iIsSupervisor, r_iMode:=1, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=sScreenHierarchy)

                        End If


                    End If

                Next j

            Next i

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateUserGroups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUserGroups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateUserBranches
    '
    ' Description:
    '
    ' History: 18/09/2003 DC -Created
    '
    ' ***************************************************************** '
    Private Function UpdateUserBranches() As Integer

        Dim result As Integer = 0
        Dim oListitem As ListViewItem
        Dim sScreenHierarchy As String = ""

        Try

            'Remove Any Unselected Groups
            If m_sScreenHierarchy <> "" Then
                sScreenHierarchy = m_sScreenHierarchy
            End If
            'MKW051203 PN8934 Changed for Selected Branches.
            For i As Integer = lvwSelectedBranches.Items.Count To 1 Step -1

                For j As Integer = m_vUserSourceInfo.GetLowerBound(1) To m_vUserSourceInfo.GetUpperBound(1)

                    'DC171003 -PN7617 -wrong way round selected/non selected branches
                    If CInt(CStr(m_vUserSourceInfo(0, j)).Trim()) = CInt(Mid(lvwSelectedBranches.Items.Item(i - 1).Name, 2, 5)) And CStr(m_vUserSourceInfo(3, j)).Trim() <> "0" Then


                        m_lReturn = g_oBusiness.UpdateUserSourceInfo(r_lUserId:=m_iUser, r_lSourceId:=CInt(Mid(lvwSelectedBranches.Items.Item(i - 1).Name, 2, 5)), r_iMode:=0, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=sScreenHierarchy)

                    End If

                Next j

            Next i

            'Add Selected Branches
            'DC171003 -PN7617 -wrong way round selected/non selected branches
            For i As Integer = lvwBranches.Items.Count To 1 Step -1

                'DC171003 -PN7617 -wrong way round selected/non selected branches
                oListitem = lvwBranches.Items.Item(i - 1)

                For j As Integer = m_vUserSourceInfo.GetLowerBound(1) To m_vUserSourceInfo.GetUpperBound(1)

                    If CInt(CStr(m_vUserSourceInfo(0, j)).Trim()) = CInt(Mid(oListitem.Name, 2, 5)) Then

                        If CStr(m_vUserSourceInfo(3, j)).Trim() = "0" Then


                            m_lReturn = g_oBusiness.UpdateUserSourceInfo(r_lUserId:=m_iUser, r_lSourceId:=CInt(Mid(oListitem.Name, 2, 5)), r_iMode:=1, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=sScreenHierarchy)


                        End If


                    End If

                Next j

            Next i

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateUserBranches Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUserBranches", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub cboJobBasis_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboJobBasis.SelectedIndexChanged
        If Me.cboJobBasis.SelectedIndex > -1 Then
            If VB6.GetItemData(Me.cboJobBasis, Me.cboJobBasis.SelectedIndex) = JOB_BASIS_FULLTIME Then
                m_oFormFields.FormatControl(txtPercentHoursWorked, 100)
                Me.txtPercentHoursWorked.Enabled = False
            ElseIf VB6.GetItemData(Me.cboJobBasis, Me.cboJobBasis.SelectedIndex) = JOB_BASIS_PARTTIME Then
                If m_bNewUser Then
                    txtPercentHoursWorked.Text = "0.00%"
                Else
                    m_oFormFields.FormatControl(txtPercentHoursWorked, m_dPercentHoursWorked)
                End If
                Me.txtPercentHoursWorked.Enabled = True
            End If
        End If

    End Sub


    Private isInitializingComponent As Boolean
    Private Sub cboMTAAuthority_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboMTAAuthority.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        If cboMTAAuthority.SelectedIndex > -1 Then
            m_iMTAAuthority = VB6.GetItemData(cboMTAAuthority, cboMTAAuthority.SelectedIndex)
        End If
    End Sub

    Private Sub cboMTAAuthority_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboMTAAuthority.SelectedIndexChanged
        If cboMTAAuthority.SelectedIndex > -1 Then
            m_iMTAAuthority = VB6.GetItemData(cboMTAAuthority, cboMTAAuthority.SelectedIndex)
        End If
    End Sub


    'Private Sub cboSystemSecurity_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboSystemSecurity.TextChanged
    '    If isInitializingComponent Then
    '        Exit Sub
    '    End If
    '    cmdApply.Enabled = True
    'End Sub

    Private Sub cboSystemSecurity_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboSystemSecurity.SelectedIndexChanged
        If m_vSystemSecurityModel <> cboSystemSecurity.SelectedIndex Then
            cmdApply.Enabled = True
        End If
    End Sub

    Private Sub chkAccHandler_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _chkAccHandler_0.CheckStateChanged, _chkAccHandler_1.CheckStateChanged, _chkAccHandler_2.CheckStateChanged
        Dim Index As Integer = Array.IndexOf(chkAccHandler, eventSender)

        If chkAccHandler(Index).CheckState = CheckState.Checked Then

            lblAccHandlerYN(Index).Text = "Yes"

            If Index = 2 Then


                pnlAccHandler(Index).Name = ""

                If Index = 0 Then
                    lblAccHandlerPanel.Text = ""
                ElseIf Index = 1 Then
                    lblAccExecutivePanel.Text = ""
                Else
                    lblClaimsHandlerPanel.Text = ""
                End If
            Else

                'DC260204 PN10643 -make sure you check if the array is empty i.e. for new users being added

                If m_vUserPartyInfo Is Nothing Then


                    pnlAccHandler(0).Name = ""
                    lblAccHandlerPanel.Text = ""


                    pnlAccHandler(1).Name = ""
                    lblAccExecutivePanel.Text = ""
                Else
                    'DC180204 PN10508 to perform click event slightly differently when first displaying party details
                    If CStr(m_vUserPartyInfo(3, 0)).Trim() = "" Then


                        pnlAccHandler(0).Name = ""
                        lblAccHandlerPanel.Text = ""


                        pnlAccHandler(1).Name = ""
                        lblAccExecutivePanel.Text = ""
                    Else
                        'DC180204 PN10508 to perform click event slightly differently when first displaying party details
                        If CStr(m_vUserPartyInfo(3, 0)).Trim() = "AH" Or CStr(m_vUserPartyInfo(3, 0)).Trim() = "HC" Then


                            pnlAccHandler(0).Name = m_vUserPartyInfo(2, 0).Trim()
                            lblAccHandlerPanel.Text = m_vUserPartyInfo(2, 0).Trim()
                        End If
                        'DC180204 PN10508 to perform click event slightly differently when first displaying party details
                        If CStr(m_vUserPartyInfo(3, 0)).Trim() = "CO" Or CStr(m_vUserPartyInfo(3, 0)).Trim() = "HC" Then


                            pnlAccHandler(1).Name = m_vUserPartyInfo(2, 0).Trim()
                            lblAccExecutivePanel.Text = m_vUserPartyInfo(2, 0).Trim()
                        End If
                    End If

                End If
            End If

        ElseIf (chkAccHandler(Index).CheckState = CheckState.Unchecked) Then

            lblAccHandlerYN(Index).Text = "No"


            pnlAccHandler(Index).Name = ""

            If Index = 0 Then
                lblAccHandlerPanel.Text = ""
            ElseIf Index = 1 Then
                lblAccExecutivePanel.Text = ""
            Else
                lblClaimsHandlerPanel.Text = ""
            End If

            If Index = 2 Then
                If chkAccHandler(Index).CheckState = CheckState.Unchecked Then
                    m_lClaimHandlerId = 0
                End If
            Else


                pnlAccHandler(0).Name = ""
                lblAccHandlerPanel.Text = ""


                pnlAccHandler(1).Name = ""
                lblAccExecutivePanel.Text = ""
                'DC180204 PN10508 to perform click event slightly differently when first displaying party details
                If m_bFirstDisplay = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lPartyHandlerId = 0
                End If
            End If

        End If

        'If both unchecked m_lPartyHandlerId = 0
        If (chkAccHandler(0).CheckState = CheckState.Unchecked) And (chkAccHandler(1).CheckState = CheckState.Unchecked) Then
            m_lPartyHandlerId = 0
        End If

        cmdAccHandler(Index).Enabled = (chkAccHandler(Index).CheckState = CheckState.Checked)

        'DC180204 PN10508 to perform click event slightly differently when first displaying party details
        '    If (chkAccHandler(0).Value = 1) And (chkAccHandler(1).Value = 1) Then
        '        ' must use the same party
        '        If Len(pnlAccHandler(0).Caption) Then
        '            pnlAccHandler(1).Caption = pnlAccHandler(0).Caption
        '        ElseIf Len(pnlAccHandler(1).Caption) Then
        '            pnlAccHandler(0).Caption = pnlAccHandler(1).Caption
        '        End If
        '    End If

    End Sub

    Private Sub chkAgent_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _chkAgent_1.CheckStateChanged, _chkAgent_0.CheckStateChanged
        Dim Index As Integer = Array.IndexOf(chkAgent, eventSender)
        UpdateAgentStatus(Index)
    End Sub

    Private Sub chkCanReverseReplaceTransactions_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkCanReverseReplaceTransactions.CheckStateChanged
        If chkCanReverseReplaceTransactions.CheckState = CheckState.Checked Then
            lblReverseReplaceTransactions.Text = "Yes"
        Else
            lblReverseReplaceTransactions.Text = "No"
        End If
    End Sub

    Private Sub chkClaimPayments_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkClaimPayments.CheckStateChanged
        EnableClaimPayments()
    End Sub

    Private Sub chkHasPaynowWriteOffAuthority_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkHasPaynowWriteOffAuthority.CheckStateChanged
        EnablePayNowWriteOff()
    End Sub

    Private Sub chkHideDeleted_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkHideDeleted.CheckStateChanged

        Dim lReturn As gPMConstants.PMEReturnCode = CType(PopListBox(), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub chkIsDeleteClient_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsDeleteClient.CheckStateChanged
        If chkIsDeleteClient.CheckState = CheckState.Checked Then
            lblDeleteClient.Text = "Yes"
        Else
            lblDeleteClient.Text = "No"
        End If

    End Sub

    Private Sub chkIsEditSchemePolicy_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsEditSchemePolicy.CheckStateChanged
        If chkIsEditSchemePolicy.CheckState = CheckState.Checked Then
            lblEditSchemePolicy.Text = "Yes"

        Else
            lblEditSchemePolicy.Text = "No"
        End If

    End Sub

    Private Sub chkIsViewClaim_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsViewClaim.CheckStateChanged
        If chkIsViewClaim.CheckState = CheckState.Checked Then
            lblViewClaim.Text = "Yes"
        Else
            lblViewClaim.Text = "No"
        End If
    End Sub

    Private Sub chkIsEditClient_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsEditClient.CheckStateChanged
        If chkIsEditClient.CheckState = CheckState.Checked Then
            lblEditClient.Text = "Yes"
        Else
            lblEditClient.Text = "No"
        End If

    End Sub

    Private Sub chkIsEditFinancePlan_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsEditFinancePlan.CheckStateChanged
        If chkIsEditFinancePlan.CheckState = CheckState.Checked Then
            lblEditFinancePlan.Text = "Yes"
        Else
            lblEditFinancePlan.Text = "No"
        End If

    End Sub

    Private Sub chkIsEditPolicy_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsEditPolicy.CheckStateChanged
        If chkIsEditPolicy.CheckState = CheckState.Checked Then
            lblEditPolicy.Text = "Yes"
            chkIsEditSchemePolicy.CheckState = CheckState.Checked
            lblEditSchemePolicy.Text = "Yes"

        Else
            lblEditPolicy.Text = "No"
            chkIsEditSchemePolicy.CheckState = CheckState.Unchecked
            lblEditSchemePolicy.Text = "No"
        End If

    End Sub

    Private Sub chkIsDeletePolicy_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsDeletePolicy.CheckStateChanged
        If chkIsDeletePolicy.CheckState = CheckState.Checked Then
            lblDeletePolicy.Text = "Yes"
        Else
            lblDeletePolicy.Text = "No"
        End If

    End Sub

    Private Sub chkIsPerformAllocations_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsPerformAllocations.CheckStateChanged
        If chkIsPerformAllocations.CheckState = CheckState.Checked Then
            lblPerformAllocations.Text = "Yes"
        Else
            lblPerformAllocations.Text = "No"
        End If

    End Sub

    Private Sub chkIsRaiseCash_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsRaiseCash.CheckStateChanged
        If chkIsRaiseCash.CheckState = CheckState.Checked Then
            lblRaiseCash.Text = "Yes"
        Else
            lblRaiseCash.Text = "No"
        End If
    End Sub

    Private Sub chkIsRaiseCredit_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsRaiseCredit.CheckStateChanged
        If chkIsRaiseCredit.CheckState = CheckState.Checked Then
            lblRaiseCredit.Text = "Yes"
        Else
            lblRaiseCredit.Text = "No"
        End If

    End Sub

    Private Sub chkIsRaiseDebit_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsRaiseDebit.CheckStateChanged
        If chkIsRaiseDebit.CheckState = CheckState.Checked Then
            lblRaiseDebit.Text = "Yes"
        Else
            lblRaiseDebit.Text = "No"
        End If

    End Sub

    Private Sub chkIsRaiseFee_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsRaiseFee.CheckStateChanged
        If chkIsRaiseFee.CheckState = CheckState.Checked Then
            lblRaiseFee.Text = "Yes"
        Else
            lblRaiseFee.Text = "No"
        End If
    End Sub

    Private Sub chkIsRaiseManualDID_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsRaiseManualDID.CheckStateChanged
        If chkIsRaiseManualDID.CheckState = CheckState.Checked Then
            lblRaiseManualDID.Text = "Yes"
        Else
            lblRaiseManualDID.Text = "No"
        End If

    End Sub

    Private Sub chkIsReverseAllocations_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsReverseAllocations.CheckStateChanged
        If chkIsReverseAllocations.CheckState = CheckState.Checked Then
            lblReverseAllocations.Text = "Yes"
        Else
            lblReverseAllocations.Text = "No"
        End If

    End Sub

    Private Sub chkIsReverseTransactions_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsReverseTransactions.CheckStateChanged
        If chkIsReverseTransactions.CheckState = CheckState.Checked Then
            lblReverseTransactions.Text = "Yes"
        Else
            lblReverseTransactions.Text = "No"
        End If
    End Sub

    Private Sub chkIsViewClient_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsViewClient.CheckStateChanged
        If chkIsViewClient.CheckState = CheckState.Checked Then
            lblViewClient.Text = "Yes"
        Else
            lblViewClient.Text = "No"
        End If

    End Sub

    '(RC) WR34
    Private Sub chkOtherParty_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkOtherParty.CheckStateChanged

        If chkOtherParty.CheckState = CheckState.Checked Then
            lblOtherPartyYN.Text = "Yes"
            chkAgent(0).CheckState = CheckState.Unchecked
            chkAgent(1).CheckState = CheckState.Unchecked
            chkAgent(0).Enabled = False
            chkAgent(1).Enabled = False

        ElseIf (chkOtherParty.CheckState = CheckState.Unchecked) Then
            lblOtherPartyYN.Text = "No"


            pnlOtherParty.Name = ""

            lblOtherPartypanel.Text = ""
            chkAgent(0).Enabled = True
            chkAgent(1).Enabled = True
        End If

        cmdOtherParty.Enabled = (chkOtherParty.CheckState = CheckState.Checked)

    End Sub

    Private Sub chkPayments_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkPayments.CheckStateChanged
        EnablePayments()
    End Sub


    Private Sub chkPayNow_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkPayNow.CheckStateChanged
        EnableUserPayNowWriteOffGrp()
    End Sub

    Private Sub chkRecommender_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkRecommender.CheckStateChanged
        EnableRecommendPayments()
    End Sub

    Private Sub chkReverseAllocation_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkReverseAllocation.CheckStateChanged
        m_lReturn = EnableAuthorityReversePayment()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If
    End Sub

    Private Sub chkUnrestrictedEnquiry_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkUnrestrictedEnquiry.CheckStateChanged
        If chkUnrestrictedEnquiry.CheckState = CheckState.Checked Then
            lblUnrestrictedEnquiry.Text = "Yes"
        Else
            lblUnrestrictedEnquiry.Text = "No"
        End If
    End Sub

    Private Sub chkUnrestrictedUpdate_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkUnrestrictedUpdate.CheckStateChanged
        If chkUnrestrictedUpdate.CheckState = CheckState.Checked Then
            lblUnrestrictedUpdate.Text = "Yes"
        Else
            lblUnrestrictedUpdate.Text = "No"
        End If
    End Sub

    Private Sub chkWriteOffs_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkWriteOffs.CheckStateChanged
        EnableWriteOff()
    End Sub

    Private Sub cmdAccHandler_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdAccHandler_0.Click, _cmdAccHandler_1.Click, _cmdAccHandler_2.Click
        Dim Index As Integer = Array.IndexOf(cmdAccHandler, eventSender)

        Dim sType As String = ""

        Select Case Index
            Case 0 : sType = "AH"
            Case 1 : sType = "CO"
            Case 2 : sType = "CH"
        End Select

        If (sType = "AH" Or sType = "CO") And chkAccHandler(0).CheckState = CheckState.Checked And chkAccHandler(1).CheckState = CheckState.Checked Then

            sType = "HC"

        End If

        If Not (lvwUsers.FocusedItem Is Nothing) Or m_bNewUser Then


            m_ofrmUserLink = New frmUserLink
            With m_ofrmUserLink

                .ClaimHandlerId = m_lClaimHandlerId
                .PartyHandlerId = m_lPartyHandlerId
                .PartyCnt = m_lPartyCnt
                .Initialise(sType)

                VB6.ShowForm(m_ofrmUserLink, FormShowConstants.Modal, Me)

                If m_ofrmUserLink.Cancel = 0 Then

                    If m_ofrmUserLink.Selected = 1 Then

                        Select Case Index
                            Case 0, 1
                                If chkAccHandler(0).CheckState = CheckState.Checked And chkAccHandler(1).CheckState = CheckState.Checked Then


                                    pnlAccHandler(0).Name = .Fullname
                                    lblAccHandlerPanel.Text = .Fullname


                                    pnlAccHandler(1).Name = .Fullname
                                    lblAccExecutivePanel.Text = .Fullname
                                Else


                                    pnlAccHandler(Index).Name = .Fullname

                                    If Index = 0 Then
                                        lblAccHandlerPanel.Text = .Fullname
                                    ElseIf Index = 1 Then
                                        lblAccExecutivePanel.Text = .Fullname
                                    Else
                                        lblClaimsHandlerPanel.Text = .Fullname
                                    End If
                                End If

                                m_lPartyHandlerId = m_ofrmUserLink.PartyHandlerId
                            Case 2


                                pnlAccHandler(Index).Name = .Fullname

                                If Index = 0 Then
                                    lblAccHandlerPanel.Text = .Fullname
                                ElseIf Index = 1 Then
                                    lblAccExecutivePanel.Text = .Fullname
                                Else
                                    lblClaimsHandlerPanel.Text = .Fullname
                                End If

                                m_lClaimHandlerId = m_ofrmUserLink.ClaimHandlerId
                        End Select

                    Else

                        Select Case Index
                            Case 0, 1
                                If chkAccHandler(0).CheckState = CheckState.Checked And chkAccHandler(1).CheckState = CheckState.Checked Then
                                    chkAccHandler(0).CheckState = CheckState.Unchecked


                                    pnlAccHandler(0).Name = ""
                                    lblAccHandlerPanel.Text = ""
                                    chkAccHandler(1).CheckState = CheckState.Unchecked


                                    pnlAccHandler(1).Name = ""
                                    lblAccExecutivePanel.Text = ""
                                Else
                                    chkAccHandler(Index).CheckState = CheckState.Unchecked


                                    pnlAccHandler(Index).Name = ""

                                    If Index = 0 Then
                                        lblAccHandlerPanel.Text = ""
                                    ElseIf Index = 1 Then
                                        lblAccExecutivePanel.Text = ""
                                    Else
                                        lblClaimsHandlerPanel.Text = ""
                                    End If
                                End If
                                m_lPartyHandlerId = 0
                            Case 2
                                chkAccHandler(Index).CheckState = CheckState.Unchecked


                                pnlAccHandler(Index).Name = ""
                                If Index = 0 Then
                                    lblAccHandlerPanel.Text = ""
                                ElseIf Index = 1 Then
                                    lblAccExecutivePanel.Text = ""
                                Else
                                    lblClaimsHandlerPanel.Text = ""
                                End If
                                m_lClaimHandlerId = 0

                        End Select

                    End If

                Else

                    Select Case Index
                        Case 0, 1
                            If m_lPartyHandlerId = 0 Then
                                chkAccHandler(0).CheckState = CheckState.Unchecked


                                pnlAccHandler(1).Name = ""
                                lblAccExecutivePanel.Text = ""
                                chkAccHandler(1).CheckState = CheckState.Unchecked


                                pnlAccHandler(1).Name = ""
                                lblAccExecutivePanel.Text = ""
                            End If
                        Case 2
                            If m_lClaimHandlerId = 0 Then
                                chkAccHandler(2).CheckState = CheckState.Unchecked


                                pnlAccHandler(2).Name = ""
                                lblClaimsHandlerPanel.Text = ""
                            End If
                    End Select

                End If

            End With

            m_ofrmUserLink.Close()

        End If
    End Sub

    Private Sub cmdAddAllBranches_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddAllBranches.Click

        Dim oListitem As ListViewItem

        For i As Integer = lvwBranches.Items.Count To 1 Step -1
            oListitem = lvwSelectedBranches.Items.Add(lvwBranches.Items.Item(i - 1).Name, lvwBranches.Items.Item(i - 1).Text, "")
            lvwBranches.Items.RemoveAt(i - 1)
        Next i

    End Sub

    Private Sub cmdAddAllGroups_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddAllGroups.Click

        Dim oListitem As ListViewItem
        Dim sTemp As String = ""

        For i As Integer = lvwGroups.Items.Count To 1 Step -1

            oListitem = lvwSelectedGroups.Items.Add(lvwGroups.Items.Item(i - 1).Name, lvwGroups.Items.Item(i - 1).Text, "user")
            lvwGroups.Items.RemoveAt(i - 1)
        Next i

    End Sub

    Private Sub cmdAddGroup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddGroup.Click

        Dim oListitem As ListViewItem
        Dim sTemp As String = ""

        If lvwGroups.Items.Count > 0 Then


            If Not Information.IsNothing(lvwGroups.FocusedItem) Then

                oListitem = lvwSelectedGroups.Items.Add(lvwGroups.FocusedItem.Name, lvwGroups.FocusedItem.Text, "user")
                lvwGroups.Items.Remove(lvwGroups.FocusedItem)
            Else
                oListitem = lvwSelectedGroups.Items.Add(lvwGroups.Items.Item(lvwGroups.Items.Count - 1).Name, lvwGroups.Items.Item(lvwGroups.Items.Count - 1).Text, "user")
                lvwGroups.Items.RemoveAt(lvwGroups.Items.Count - 1)
            End If
        End If

    End Sub

    Private Sub cmdAgent_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdAgent_1.Click, _cmdAgent_0.Click
        Dim Index As Integer = Array.IndexOf(cmdAgent, eventSender)
        Dim sType As String = ""
        Dim vCnt As Integer
        Dim vName, vShortName As Object
        Dim vResolvedName As Object

        If Index = 0 Then

            m_lReturn = SelectParty(vPartyCnt:=vCnt, vName:=vName, vShortName:=vShortName, vResolvedName:=vResolvedName, vSpecialParty:="AG", bSuppressSubAgents:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            pnlAgent(Index).Name = vResolvedName
            If (Index = 0) Then
                lblAgentPanel.Text = vResolvedName
            Else
                lblInsurerPanel.Text = vResolvedName
            End If
            m_lPartyCnt = vCnt
            Exit Sub
        ElseIf Index = 1 Then
            sType = "IN"
        ElseIf Index = 2 Then  '(RC) WR34
            sType = "OT"
        End If


        m_ofrmUserLink = New frmUserLink

        With m_ofrmUserLink

            .ClaimHandlerId = m_lClaimHandlerId
            .PartyHandlerId = m_lPartyHandlerId
            .PartyCnt = m_lPartyCnt
            .Initialise(sType)

            VB6.ShowForm(m_ofrmUserLink, FormShowConstants.Modal, Me)

            If m_ofrmUserLink.Cancel = 0 Then

                If m_ofrmUserLink.Selected = 1 Then



                    pnlAgent(Index).Name = .Fullname
                    If (Index = 0) Then
                        lblAgentPanel.Text = .Fullname
                    Else
                        lblInsurerPanel.Text = .Fullname
                    End If

                    m_lPartyCnt = m_ofrmUserLink.PartyCnt

                Else

                    chkAgent(Index).CheckState = CheckState.Unchecked


                    pnlAgent(Index).Name = ""
                    If (Index = 0) Then
                        lblAgentPanel.Text = ""
                    Else
                        lblInsurerPanel.Text = ""
                    End If
                End If

            Else

                If m_lPartyCnt = 0 Then
                    chkAgent(Index).CheckState = CheckState.Unchecked


                    pnlAgent(Index).Name = ""
                    If (Index = 0) Then
                        lblAgentPanel.Text = ""
                    Else
                        lblInsurerPanel.Text = ""
                    End If
                End If

            End If

        End With

        m_ofrmUserLink.Close()

    End Sub

    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click
        m_bIsClearCache = True
        m_lReturn = Save()

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            cmdAddUser.Enabled = True
            cmdDeleteUser.Enabled = True
            lvwUsers.Enabled = True
            cmdApply.Enabled = False

            SSTabHelper.SetSelectedIndex(SSTab1, 0)
            SSTab1.Enabled = False
            tabMain.TabPages(1).Enabled = False
        End If

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        If m_bNewUser Then

            lvwUsers.FocusedItem = Nothing
            'DC141003 -PN7414 -does not need to attempt o remove as not there when adding
            '        lvwUsers.ListItems.Remove Trim$(pnlUsername.Caption)

            ClearUserDetails()

            cmdAddUser.Enabled = True
            cmdDeleteUser.Enabled = True
            lvwUsers.Enabled = True
            cmdApply.Enabled = False
            cmdOK.Enabled = False
            SSTab1.Enabled = False
            tabMain.TabPages(1).Enabled = False
            m_bNewUser = False

        Else

            If m_bDeletedUser Then

                m_bDeletedUser = False
                cmdDeleteUser.Text = "Un&delete"


                lvwUsers.SelectedItems(0).ForeColor = Color.Black
                cmdAddUser.Enabled = True

            Else

                Me.Close()

            End If

        End If

    End Sub


    Private Sub cmdDelAllBranches_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelAllBranches.Click

        Dim oListitem As ListViewItem

        For i As Integer = lvwSelectedBranches.Items.Count To 1 Step -1
            oListitem = lvwBranches.Items.Add(lvwSelectedBranches.Items.Item(i - 1).Name, lvwSelectedBranches.Items.Item(i - 1).Text, "")
            lvwSelectedBranches.Items.RemoveAt(i - 1)
        Next i

    End Sub

    Private Sub cmdDelAllGroups_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelAllGroups.Click

        Dim oListitem As ListViewItem

        For i As Integer = lvwSelectedGroups.Items.Count To 1 Step -1
            oListitem = lvwGroups.Items.Add(lvwSelectedGroups.Items.Item(i - 1).Name, lvwSelectedGroups.Items.Item(i - 1).Text, "")
            lvwSelectedGroups.Items.RemoveAt(i - 1)
        Next
    End Sub

    Private Sub cmdDelBranch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelBranch.Click

        Dim oListitem As ListViewItem

        If lvwSelectedBranches.Items.Count > 0 Then

            If Not Information.IsNothing(lvwSelectedBranches.FocusedItem) Then
                oListitem = lvwBranches.Items.Add(lvwSelectedBranches.FocusedItem.Name, lvwSelectedBranches.FocusedItem.Text, "")
                lvwSelectedBranches.Items.Remove(lvwSelectedBranches.FocusedItem)
            Else
                oListitem = lvwBranches.Items.Add(lvwSelectedBranches.Items(lvwSelectedBranches.Items.Count - 1).Name, lvwSelectedBranches.Items(lvwSelectedBranches.Items.Count - 1).Text, "")
                lvwSelectedBranches.Items.RemoveAt(lvwSelectedBranches.Items.Count - 1)
            End If
        End If
    End Sub

    ''' <summary>
    ''' chkEditDefaultCommission_Click
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub chkEditDefaultCommission_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) _
        Handles chkEditDefaultCommission.Click
        Const kMethodName As String = "chkEditDefaultCommission_Click"
        If m_bIsEditCommission = False Then
            If chkEditDefaultCommission.CheckState = 1 Then
                chkEditDefaultCommissionNBRN.CheckState = 1
                chkEditDefaultCommissionMTA.CheckState = 1
                chkEditDefaultCommissionMTC.CheckState = 1
                chkEditDefaultCommissionMTR.CheckState = 1

                chkEditDefaultCommissionNBRN.Enabled = True
                chkEditDefaultCommissionMTA.Enabled = True
                chkEditDefaultCommissionMTC.Enabled = True
                chkEditDefaultCommissionMTR.Enabled = True
            Else
                chkEditDefaultCommissionNBRN.CheckState = 0
                chkEditDefaultCommissionMTA.CheckState = 0
                chkEditDefaultCommissionMTC.CheckState = 0
                chkEditDefaultCommissionMTR.CheckState = 0

                chkEditDefaultCommissionNBRN.Enabled = False
                chkEditDefaultCommissionMTA.Enabled = False
                chkEditDefaultCommissionMTC.Enabled = False
                chkEditDefaultCommissionMTR.Enabled = False
            End If
            m_lReturn = SetEditCommissionCheckBoxValues(v_bEditCommissionClicked:=True,
                                                        v_bEditDefaultCommissionNBRNClicked:=False,
                                                        v_bEditDefaultCommissionMTAClicked:=False,
                                                        v_bEditDefaultCommissionMTCClicked:=False,
                                                        v_bEditDefaultCommissionMTRClicked:=False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "SetEditCommissionCheckBoxValues Failed")
            End If
            m_bIsEditCommission = False
        End If
    End Sub

    ''' <summary>
    ''' chkEditDefaultCommissionnbrn_Click
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub chkEditDefaultCommissionnbrn_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) _
        Handles chkEditDefaultCommissionNBRN.Click
        Const kMethodName As String = "chkEditDefaultCommissionnbrn_Click"
        m_bIsEditCommission = True
        If chkEditDefaultCommissionNBRN.CheckState = 1 Then
            m_nEnableDefaultCommissionNBRN = 1
        Else
            m_nEnableDefaultCommissionNBRN = 0
        End If
        m_lReturn = SetEditCommissionCheckBoxValues(v_bEditCommissionClicked:=False,
                                                    v_bEditDefaultCommissionNBRNClicked:=True,
                                                    v_bEditDefaultCommissionMTAClicked:=False,
                                                    v_bEditDefaultCommissionMTCClicked:=False,
                                                    v_bEditDefaultCommissionMTRClicked:=False)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "SetEditCommissionCheckBoxValues Failed")
        End If
        m_bIsEditCommission = False
    End Sub

    ''' <summary>
    ''' chkEditDefaultCommissionMTA_Click
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub chkEditDefaultCommissionMTA_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) _
        Handles chkEditDefaultCommissionMTA.Click
        Const kMethodName As String = "chkEditDefaultCommissionMTA_Click"
        m_bIsEditCommission = True
        If chkEditDefaultCommissionMTA.CheckState = 1 Then
            m_nEnableDefaultCommissionMTA = 1
        Else
            m_nEnableDefaultCommissionMTA = 0
        End If
        m_lReturn = SetEditCommissionCheckBoxValues(v_bEditCommissionClicked:=False,
                                                    v_bEditDefaultCommissionNBRNClicked:=False,
                                                    v_bEditDefaultCommissionMTAClicked:=True,
                                                    v_bEditDefaultCommissionMTCClicked:=False,
                                                    v_bEditDefaultCommissionMTRClicked:=False)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "SetEditCommissionCheckBoxValues Failed")
        End If
        m_bIsEditCommission = False
    End Sub

    ''' <summary>
    ''' chkEditDefaultCommissionMTC_Click
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub chkEditDefaultCommissionMTC_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) _
        Handles chkEditDefaultCommissionMTC.Click
        Const kMethodName As String = "chkEditDefaultCommissionMTC_Click"
        m_bIsEditCommission = True
        If chkEditDefaultCommissionMTC.CheckState = 1 Then
            m_nEnableDefaultCommissionMTC = 1
        Else
            m_nEnableDefaultCommissionMTC = 0
        End If
        m_lReturn = SetEditCommissionCheckBoxValues(v_bEditCommissionClicked:=False,
                                                    v_bEditDefaultCommissionNBRNClicked:=False,
                                                    v_bEditDefaultCommissionMTAClicked:=False,
                                                    v_bEditDefaultCommissionMTCClicked:=True,
                                                    v_bEditDefaultCommissionMTRClicked:=False)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "SetEditCommissionCheckBoxValues Failed")
        End If
        m_bIsEditCommission = False
    End Sub

    ''' <summary>
    ''' chkEditDefaultCommissionMTR_Click
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub chkEditDefaultCommissionMTR_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) _
        Handles chkEditDefaultCommissionMTR.Click
        Const kMethodName As String = "chkEditDefaultCommissionMTR_Click"
        m_bIsEditCommission = True
        If chkEditDefaultCommissionMTR.CheckState = 1 Then
            m_nEnableDefaultCommissionMTR = 1
        Else
            m_nEnableDefaultCommissionMTR = 0
        End If
        m_lReturn = SetEditCommissionCheckBoxValues(v_bEditCommissionClicked:=False,
                                                    v_bEditDefaultCommissionNBRNClicked:=False,
                                                    v_bEditDefaultCommissionMTAClicked:=False,
                                                    v_bEditDefaultCommissionMTCClicked:=False,
                                                    v_bEditDefaultCommissionMTRClicked:=True)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "SetEditCommissionCheckBoxValues Failed")
        End If
        m_bIsEditCommission = False
    End Sub


    Private Sub cmdDeleteUser_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteUser.Click
        Dim lStatus, lAdminUserCount As Integer

        If Not (lvwUsers.FocusedItem Is Nothing) Then

            If cmdDeleteUser.Text = "&Delete" Then

                m_lReturn = g_oBusiness.GetUserAdminStatus(r_iUserId:=m_iUser, r_lStatus:=lStatus, r_iSecurityModel:=m_vSystemSecurityModel)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                If lStatus > 0 Then

                    m_lReturn = g_oBusiness.GetAdminUserCount(r_iSecurityModel:=m_vSystemSecurityModel, r_lAdminUserCount:=lAdminUserCount)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                    If lAdminUserCount = 1 Then
                        MessageBox.Show("'" & m_sUsername & "' cannot be deleted. " & "There should be at least one user as " & "system administrator.", "User Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        m_bDeletedUser = False
                        m_bUndeletedUser = True

                        Exit Sub
                    End If
                End If
            End If

            With lvwUsers.FocusedItem


                If .ForeColor = Color.Gray Then
                    .ForeColor = Color.Black
                ElseIf .ForeColor = Color.Black OrElse .ForeColor.Name = "WindowText" Then
                    .ForeColor = Color.Gray
                End If


                If .ForeColor = Color.Gray Then

                    cmdDeleteUser.Text = "Un&delete"
                    SSTab1.Enabled = False
                    tabMain.TabPages(1).Enabled = False
                    m_bDeletedUser = True
                    m_bUndeletedUser = False

                Else

                    cmdDeleteUser.Text = "&Delete"
                    SSTab1.Enabled = True
                    tabMain.TabPages(1).Enabled = True
                    m_bDeletedUser = False
                    m_bUndeletedUser = True

                End If
            End With

            cmdAddUser.Enabled = (m_iIsDeleted = gPMConstants.PMEReturnCode.PMFalse And m_bUndeletedUser) Or (m_iIsDeleted = gPMConstants.PMEReturnCode.PMTrue And m_bDeletedUser)

        End If

    End Sub

    Private Sub cmdDelGroup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelGroup.Click

        Dim oListitem As ListViewItem
        If (lvwSelectedGroups.Items.Count > 0) Then
            If Not (lvwSelectedGroups.FocusedItem Is Nothing) Then
                oListitem = lvwGroups.Items.Add(lvwSelectedGroups.FocusedItem.Name, lvwSelectedGroups.FocusedItem.Text, "")
                lvwSelectedGroups.Items.Remove(lvwSelectedGroups.FocusedItem)
            Else
                oListitem = lvwGroups.Items.Add(lvwSelectedGroups.Items(lvwSelectedGroups.Items.Count - 1).Name, lvwSelectedGroups.Items(lvwSelectedGroups.Items.Count - 1).Text, "")
                lvwSelectedGroups.Items.RemoveAt(lvwSelectedGroups.Items.Count - 1)
            End If
        End If

    End Sub

    Private Sub cmdEditRiskDetails_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditRiskDetails.Click
        Dim sResult As String = ""
        Dim iNumItems As Integer
        Dim lResult, lTag As Integer

        With lvwRiskGroups
            ' how many items have been selected?
            For i As Integer = 1 To .Items.Count
                If .Items.Item(i - 1).Selected Then
                    iNumItems += 1
                End If
            Next i

            If iNumItems = 1 Then

                m_ofrmUserStatus.UserStatus = lvwRiskGroups.FocusedItem.SubItems(1).Text

                m_ofrmUserStatus.RiskGroup = lvwRiskGroups.FocusedItem.Text
                lTag = gPMFunctions.ToSafeLong(Convert.ToString(lvwRiskGroups.FocusedItem.Tag))

                m_ofrmUserStatus.PassedExam = gPMFunctions.ToSafeBoolean(m_vUserRiskGroupInfo(ARR_RG_PASSEDEXAM, lTag))

                m_ofrmUserStatus.DatePassedExam = m_vUserRiskGroupInfo(ARR_RG_DATEPASSEDEXAM, lTag)
            Else

                m_ofrmUserStatus.UserStatus = ""

                m_ofrmUserStatus.PassedExam = False


                m_ofrmUserStatus.DatePassedExam = Nothing

                m_ofrmUserStatus.RiskGroup = "Various"
            End If

            m_ofrmUserStatus.Status = gPMConstants.PMEReturnCode.PMCancel

            VB6.ShowForm(m_ofrmUserStatus, FormShowConstants.Modal, Me)

            If m_ofrmUserStatus.Status <> gPMConstants.PMEReturnCode.PMOK Then
                'The user has cancelled
                Exit Sub
            End If

            sResult = m_ofrmUserStatus.UserStatus
            If sResult.Trim() <> "" Then

                lResult = m_ofrmUserStatus.UserStatusId
            Else
                lResult = 0
            End If

            For i As Integer = 1 To .Items.Count
                If .Items.Item(i - 1).Selected Then

                    lTag = Convert.ToString(.Items.Item(i - 1).Tag)

                    ListViewHelper.GetListViewSubItem(lvwRiskGroups.Items.Item(i - 1), 1).Text = m_ofrmUserStatus.UserStatus

                    ListViewHelper.GetListViewSubItem(lvwRiskGroups.Items.Item(i - 1), 2).Text = IIf(m_ofrmUserStatus.PassedExam, "Yes", "No")

                    ListViewHelper.GetListViewSubItem(lvwRiskGroups.Items.Item(i - 1), 3).Text = IIf(Information.IsDate(m_ofrmUserStatus.DatePassedExam), gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, m_ofrmUserStatus.DatePassedExam), "")

                    m_vUserRiskGroupInfo(ARR_RG_STATUSID, lTag) = IIf(m_ofrmUserStatus.UserStatus.Trim() <> "", CStr(m_ofrmUserStatus.UserStatusId), CStr(0))

                    m_vUserRiskGroupInfo(ARR_RG_STATUSDESC, lTag) = m_ofrmUserStatus.UserStatus

                    m_vUserRiskGroupInfo(ARR_RG_PASSEDEXAM, lTag) = CStr(m_ofrmUserStatus.PassedExam)


                    m_vUserRiskGroupInfo(ARR_RG_DATEPASSEDEXAM, lTag) = IIf(Information.IsDate(m_ofrmUserStatus.DatePassedExam), m_ofrmUserStatus.DatePassedExam, DBNull.Value)
                End If
            Next i

        End With
    End Sub

    Private Sub cmdAddUser_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddUser.Click

        Dim sTemp As String = ""

        cmdAddUser.Enabled = False
        cmdDeleteUser.Enabled = False
        lvwUsers.Enabled = False
        Dim lstSelectedItem As ListViewItem = lvwUsers.FocusedItem
        lvwUsers.FocusedItem = Nothing


        ClearUserDetails()

        'DJM 04/02/2004 : Default effective date to today.


        m_ofrmUser = New frmUser
        m_ofrmUser.EffectiveDate = DateTime.Today

        VB6.ShowForm(m_ofrmUser, FormShowConstants.Modal, Me)

        If m_bNewUser Then

            sTemp = ""


            pnlEffectiveDate.Name = m_ofrmUser.EffectiveDate

            lblEffectiveDatePanel.Text = m_ofrmUser.EffectiveDate


            pnlUsername.Name = m_ofrmUser.UserName


            lblUsername.Text = m_ofrmUser.UserName


            sTemp = pnlUsername.Name

            m_ofrmUser.Close()

            cmdApply.Enabled = True
            cmdOK.Enabled = True
            SSTab1.Enabled = True
            tabMain.TabPages(1).Enabled = True
            'PN16757 Clearing the previous information
            m_vUserPartyInfo = VB6.CopyArray(Nothing)
            'DC141003 -PN7428 -set focus to first field
            ddTitle.Focus()

        Else

            cmdAddUser.Enabled = True
            cmdDeleteUser.Enabled = True
            lvwUsers.Enabled = True
            lvwUsers.FocusedItem = lstSelectedItem

            lvwUsers_ItemClick(lstSelectedItem)

        End If

    End Sub
    'PN7412
    Private Sub cmdNext_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdNext_4.Click, _cmdNext_6.Click, _cmdNext_0.Click, _cmdNext_5.Click, _cmdNext_3.Click, _cmdNext_2.Click, _cmdNext_1.Click
        Dim Index As Integer = Array.IndexOf(cmdNext, eventSender)
        Try

            ' Change to the next tab.
            If SSTabHelper.GetSelectedIndex(SSTab1) < SSTabHelper.GetTabCount(SSTab1) - 1 Then
                '2005 Try to account for Tabs Not being displayed
                If SSTabHelper.GetSelectedIndex(SSTab1) = 4 Then
                    SSTabHelper.SetSelectedIndex(SSTab1, Index + 3)
                Else
                    SSTabHelper.SetSelectedIndex(SSTab1, Index + 1)
                End If


            End If

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(SSTab1) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
            End If

        Catch



            ' Error Section

            Exit Sub
        End Try


    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        m_lReturn = Save()
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            Me.Close()
        End If

    End Sub

    '(RC) WR34
    Private Sub cmdOtherParty_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOtherParty.Click


        m_ofrmUserLink = New frmUserLink

        With m_ofrmUserLink

            .OtherPartyId = m_lOtherPartyId
            .Initialise("OT")

            VB6.ShowForm(m_ofrmUserLink, FormShowConstants.Modal, Me)

            If m_ofrmUserLink.Cancel = 0 Then

                If m_ofrmUserLink.Selected = 1 Then


                    pnlOtherParty.Name = .Fullname

                    lblOtherPartypanel.Text = .Fullname

                    m_lOtherPartyId = m_ofrmUserLink.OtherPartyId '(RC) WR34
                Else
                    chkOtherParty.CheckState = CheckState.Unchecked


                    pnlOtherParty.Name = ""

                    lblOtherPartypanel.Text = ""

                    m_lOtherPartyId = m_ofrmUserLink.OtherPartyId '(RC) WR34
                End If

            End If

        End With

        m_ofrmUserLink.Close()

    End Sub

    'PN7412
    Private Sub cmdPrevious_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdPrevious_3.Click, _cmdPrevious_6.Click, _cmdPrevious_5.Click, _cmdPrevious_4.Click, _cmdPrevious_2.Click, _cmdPrevious_1.Click, _cmdPrevious_0.Click
        Dim Index As Integer = Array.IndexOf(cmdPrevious, eventSender)
        Try


            ' Change to the next tab.
            If SSTabHelper.GetSelectedIndex(SSTab1) > 0 Then
                '2005 Try to account for Tabs Not being displayed
                If SSTabHelper.GetSelectedIndex(SSTab1) = 7 Then
                    SSTabHelper.SetSelectedIndex(SSTab1, Index - 2)
                Else
                    SSTabHelper.SetSelectedIndex(SSTab1, Index)
                End If

            End If

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(SSTab1) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(ACControlStart, Index).Focus()
            End If

        Catch



            ' Error Section

            Exit Sub
        End Try


    End Sub

    Private Sub cmdSignature_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSignature.Click

        Dim sToFile, sFromFile As String

        If m_sSignatureDir = "" Then

            MessageBox.Show("Cannot Obtain Signature Directory", "Signature")

        Else
            With cmdlgSignatureOpen

                .CheckFileExists = True
                .CheckPathExists = True

                .Filter = "Picture Files (*.bmp;*.jpg;*.gif)|*.bmp;*.jpg;*.gif|All Files (*.*)|*.*"

                'Modified by Sumeet Singh on 5/24/2010 4:54:05 PM to do list
                '.CancelError = True
                .InitialDirectory = m_sSignatureDir
                Try


                    .ShowDialog()
                    If .FileName.Equals("") Then
                        Exit Sub
                    End If

                    Dim ioStrm As IO.Stream
                    ioStrm = File.OpenRead(.FileName)
                    imgSignature.Image = Image.FromStream(ioStrm)
                    ioStrm.Close()

                    m_sSignatureDir = .InitialDirectory
                    sFromFile = .FileName
                    sToFile = m_sSignatureDir & m_sUsername
                    File.Copy(sFromFile, sToFile, True)
                    m_sSignatureFile = m_sUsername
                Catch ex As Exception

                End Try
            End With


        End If

    End Sub

    Private Sub cmdChangePassword_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdChangePassword.Click

        g_iSystemSecurityModel = m_vSystemSecurityModel

        g_bValidDomainAccount = False
        For Each oListitem As ListViewItem In lvwMatchedUsers.Items
            If m_iUser = StringsHelper.ToDoubleSafe(Mid(oListitem.Name, 2)) And m_vAlternativeIdentifier.Length > 0 Then
                g_bValidDomainAccount = True
                Exit For
            End If
        Next oListitem


        m_ofrmPassword = New frmPassword

        m_ofrmPassword.txtNewPassword.Text = ""

        m_ofrmPassword.txtConfirmPassword.Text = ""

        VB6.ShowForm(m_ofrmPassword, FormShowConstants.Modal, Me)

        m_bPasswordChanged = m_ofrmPassword.PasswordChanged

        If m_bPasswordChanged Then
            m_bIsTempPassword = True
            cmdApply.Enabled = True
        End If

    End Sub

    Private Sub cmdAddBranch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddBranch.Click

        Dim oListitem As ListViewItem

        If lvwBranches.Items.Count > 0 Then


            If Not Information.IsNothing(lvwBranches.FocusedItem) Then
                oListitem = lvwSelectedBranches.Items.Add(lvwBranches.FocusedItem.Name, lvwBranches.FocusedItem.Text, "")
                lvwBranches.Items.Remove(lvwBranches.FocusedItem)
            Else
                lvwSelectedBranches.Items.Add(lvwBranches.Items(lvwBranches.Items.Count - 1).Name, lvwBranches.Items(lvwBranches.Items.Count - 1).Text, "")
                lvwBranches.Items.RemoveAt(lvwBranches.Items.Count - 1)
            End If
        End If


    End Sub


    Private Sub frmUserMaintenance_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
            If lvwUsers.Enabled Then
                lvwUsers.Focus()
            End If
        End If
    End Sub

    Private Sub Form_Initialize_Renamed()

        ' Initialise the error number value.
        m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

        iPMFunc.ShowFormInTaskBar_Attach()

        'Initialise the form using selected function
        m_lReturn = InitialForm()

    End Sub

    ''' <summary>
    ''' frmUserMaintenance_Load
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub frmUserMaintenance_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        m_bFormLoading = True
        m_bIsEditCommission = False
        If m_sAD_OU_Path.Trim() <> "" And m_sAD_OU_Domain.Trim() <> "" Then
            m_bLDAPQueryProvided = gPMConstants.PMEReturnCode.PMTrue
        Else
            m_bLDAPQueryProvided = gPMConstants.PMEReturnCode.PMFalse
        End If


        SSTabHelper.SetSelectedIndex(tabMain, 0)

        iPMFunc.ShowFormInTaskBar_Detach()

        m_lReturn =
            gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine,
                                         v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions,
                                         v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient,
                                         v_sSettingName:="Signatures", r_sSettingValue:=m_sSignatureDir)

        m_lReturn = GetHiddenOption(v_lSourceId:=g_iSourceID, r_vEnableFSACompliance:=m_vEnableFSACompliance)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Exit Sub
        End If


        m_sUnderwritingOrAgency = g_oBusiness.UnderwritingOrAgency


        fraMakeLive.Visible = True
        fraRatingSections.Visible = True

        fraBrokerAgentPortfolioTransfer.Visible = True

        SSTabHelper.SetTabVisible(SSTab1, 7, False)


        SSTabHelper.SetTabVisible(SSTab1, 5, False)
        SSTabHelper.SetTabVisible(SSTab1, 6, False)
        SSTabHelper.SetTabVisible(SSTab1, 7, True)
        SSTabHelper.SetTabCaption(SSTab1, 7, "6 - Party Edit")

        chkCanPerformBrokerTransfer.Enabled = True


        txtFilter.Text = s_defFilterText
        SSTabHelper.SetSelectedIndex(SSTab1, 0)

        ' Check if we have had an error so far.
        ' Possibly creating the business object.
        If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST exit now.
            Exit Sub
        End If

        m_lReturn = DisplayLookupDetails()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If


        'Populate the listbox using the selected function
        m_lReturn = PopListBox()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Exit Sub
        End If

        m_lReturn = PopCombo()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        m_lReturn = PopulateJobBasisCombo()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        m_lReturn = RefreshListOnSecurityTab()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        m_oFormFields = New iPMFormControl.FormFields()
        m_oFormFields.AddNewFormField(ctlControl:=txtPercentHoursWorked,
                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent,
                                      lFieldType:=gPMConstants.PMEDataType.PMDouble,
                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

        If lvwUsers.Items.Count > 0 Then
            lvwUsers.Items.Item(0).Selected = False
        End If
        CheckIsRecommendClaimPaymentEnabledatProduct()

        If Not m_bCheckIsRecommendClaimPaymentEnabledatProduct Then
            chkRecommender.Enabled = False
            EnableRecommendPayments()
        End If
        ClearUserDetails()
        m_bFormLoading = False
    End Sub


    'UPGRADE_NOTE: (7001) The following declaration (lblEditFinancePlans_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub lblEditFinancePlans_Click()
    '
    'End Sub

    'UPGRADE_NOTE: (7001) The following declaration (lblViewClients_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub lblViewClients_Click()
    '
    'End Sub

    Private Sub frmUserMaintenance_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        If Not (m_oFormFields Is Nothing) Then
            m_oFormFields.Dispose()
            m_oFormFields = Nothing
        End If

    End Sub

    Private Sub lvwBranches_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwBranches.DoubleClick
        cmdAddBranch_Click(cmdAddBranch, New EventArgs())
    End Sub


    Private Sub lvwDomainUsers_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwDomainUsers.DoubleClick
        cmdMatch_Click(cmdMatch, New EventArgs())
    End Sub

    Private Sub lvwDomainUsers_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwDomainUsers.Enter
        lvwDomainUsers.HideSelection = False
    End Sub

    Private Sub lvwGroups_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwGroups.DoubleClick
        cmdAddGroup_Click(cmdAddGroup, New EventArgs())
    End Sub

    Private Sub lvwMatchedUsers_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwMatchedUsers.DoubleClick
        cmdUnmatch_Click(cmdUnmatch, New EventArgs())
    End Sub

    Private Sub lvwMatchedUsers_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwMatchedUsers.Enter
        lvwMatchedUsers.HideSelection = False
        lvwMatchedUsers.MultiSelect = True
    End Sub

    Private Sub lvwRiskGroups_BeforeLabelEdit(ByVal eventSender As Object, ByVal eventArgs As LabelEditEventArgs) Handles lvwRiskGroups.BeforeLabelEdit
        Dim Cancel As Boolean = eventArgs.CancelEdit
        Cancel = 1
    End Sub

    Private Sub lvwSelectedBranches_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSelectedBranches.DoubleClick
        cmdDelBranch_Click(cmdDelBranch, New EventArgs())
    End Sub

    Private Sub lvwSelectedGroups_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSelectedGroups.DoubleClick
        cmdDelGroup_Click(cmdDelGroup, New EventArgs())
    End Sub

    ''' <summary>
    ''' lvwSelectedGroups_MouseDown
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub lvwSelectedGroups_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwSelectedGroups.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000

        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        If Button <> MouseButtons.Right Then
            Exit Sub
        End If

        Dim oListitem As ListViewItem = lvwSelectedGroups.GetItemAt(x, y)
        If oListitem Is Nothing Then
            Exit Sub
        End If

        mnuSuper.Checked = oListitem.ImageKey = "supervisor"
        Ctx_mnuSupervisor.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)

        If mnuSuper.Checked Then
            oListitem.ImageKey = "supervisor"
        Else
            oListitem.ImageKey = "user"
        End If

        m_oListItem = oListitem
        oListitem = Nothing

    End Sub


    'Private Sub lvwRiskGroups_ItemClick(ByVal Item As ListViewItem)

    '    cmdEditRiskDetails.Enabled = Not (Item Is Nothing)

    'End Sub

    Private Sub lvwSiriusUsers_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSiriusUsers.Enter
        lvwSiriusUsers.HideSelection = False

    End Sub

    Private Sub lvwUsers_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwUsers.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000

        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        m_lXPos = CInt(x)
        m_lYPos = CInt(y)

    End Sub


    Private Sub lvwUsers_ItemClick(ByVal Item As ListViewItem)

        Dim oListitem As ListViewItem

        cmdApply.Enabled = True
        cmdOK.Enabled = True
        SSTab1.Enabled = True
        tabMain.TabPages(1).Enabled = True
        m_bNewUser = False



        If Item.ForeColor = Color.Gray Then
            cmdDeleteUser.Text = "Un&delete"
            SSTab1.Enabled = False
            tabMain.TabPages(1).Enabled = False
        Else
            cmdDeleteUser.Text = "&Delete"
            SSTab1.Enabled = True
            tabMain.TabPages(1).Enabled = True
        End If


        If Not Information.IsNothing(lvwUsers.FocusedItem) Then
            ' Edit details of user if doubleclicked
            If lvwUsers.FocusedItem.Index >= 0 Then
                oListitem = lvwUsers.FocusedItem
            Else
                oListitem = lvwUsers.GetItemAt(m_lXPos, m_lYPos)
            End If
        Else
            lvwUsers.FocusedItem = lvwUsers.Items(0)
            oListitem = lvwUsers.Items(0)
        End If
        m_lReturn = EditUser(oListitem)


    End Sub

    Private Sub chkPrinterChangable_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkPrinterChangable.CheckStateChanged
        If chkPrinterChangable.CheckState = CheckState.Checked Then
            lblPrinterYN.Text = "Yes"
        ElseIf (chkPrinterChangable.CheckState = CheckState.Unchecked) Then
            lblPrinterYN.Text = "No"
        End If
    End Sub

    Public Sub mnuSuper_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuSuper.Click
        mnuSuper.Checked = Not (mnuSuper.Checked)
    End Sub

    Private Sub SSTab1_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles SSTab1.SelectedIndexChanged
        Try

            With SSTab1
                ' Set the default button.
                If SSTabHelper.GetSelectedIndex(SSTab1) < cmdNext.Length Then
                    VB6.SetDefault(cmdNext(SSTabHelper.GetSelectedIndex(SSTab1)), True)
                Else
                    '            cmdOK.Default = True
                End If

                ' Now I know this is crap, this goes against
                ' all my principles, but for some reason when
                ' using the mouse to select a tab the setfocus
                ' code below doesn't work. The cursor sticks,
                ' and you can't tab off. Therefore I've used
                ' this to get around the problem.
                Application.DoEvents()

                ' Set focus to the first control on the tab.
                If SSTabHelper.GetSelectedIndex(SSTab1) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                    m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(SSTab1)).Focus()
                End If
            End With

        Catch



            ' Error Section.

            'Modified by Sumeet Singh on 5/24/2010 4:56:28 PM refered to the VB Code
            'SSTab1PreviousTab = SSTab1.SelectedIndex
        End Try

    End Sub


    Private Sub txtClaimPayments_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles txtClaimPayments.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        '''PN68816    m_oFormFields.LostFocus causes screen to Flicker
        If txtClaimPayments.Text.Trim() <> "" Then
            Dim dbNumericTemp As Double
            If Not Double.TryParse(txtClaimPayments.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                MessageBox.Show("Please enter a valid amount", "Invalid amount entered - Amount", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtClaimPayments.Text = ""
                Cancel = True
            End If
        End If
        eventArgs.Cancel = Cancel
    End Sub

    Private Sub txtExtensionNumber_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtExtensionNumber.Leave
        Dim dbNumericTemp As Double
        If (Not Double.TryParse(txtExtensionNumber.Text.Replace(" ", ""), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) And txtExtensionNumber.Text.Trim() <> "" Then
            MessageBox.Show("Please enter a valid number", "Invalid number entered - Extension Number", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtExtensionNumber.Focus()
            Exit Sub
        End If
    End Sub

    Private Sub txtFaxNumber_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFaxNumber.Leave
        Dim dbNumericTemp As Double
        If (Not Double.TryParse(txtFaxNumber.Text.Replace(" ", ""), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) And txtFaxNumber.Text.Trim() <> "" Then
            MessageBox.Show("Please enter a valid number", "Invalid number entered - Fax Number", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtFaxNumber.Focus()
            Exit Sub
        End If
    End Sub

    Private Sub txtFilter_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFilter.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        Dim lReturn As gPMConstants.PMEReturnCode
        If Not m_bFormLoading Then
            If Strings.Len(txtFilter.Text) < 1 Then
                txtFilter.Text = s_defFilterText
                txtFilter_Enter(txtFilter, New EventArgs())
            End If

            RefreshList()
            lReturn = CType(PopListBox(), gPMConstants.PMEReturnCode)
            If Not (lvwUsers.FocusedItem Is Nothing) Then 'PN22629

                lvwUsers_ItemClick(lvwUsers.FocusedItem)
            Else
                ' No users in user list so clear and disable controls!
                ClearUserDetails()
                m_bNewUser = False
                cmdOK.Enabled = False
                cmdApply.Enabled = False
                SSTabHelper.SetSelectedIndex(SSTab1, 0)
                SSTab1.Enabled = False
                tabMain.TabPages(1).Enabled = False
            End If

        End If
    End Sub

    Private Sub txtFilter_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFilter.Enter
        txtFilter.SelectionStart = 0
        txtFilter.SelectionLength = Strings.Len(txtFilter.Text)
    End Sub

    ' ***************************************************************** '
    ' Name: CheckUsername
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function CheckUsername(ByVal v_sUsername As String) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For lRow As Integer = 1 To lvwUsers.Items.Count
                If v_sUsername.Trim() = lvwUsers.Items.Item(lRow - 1).Text.Trim() Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Next
            If Information.IsArray(m_ArrDeletedUsers) Then
                For lRow As Integer = 1 To m_ArrDeletedUsers.GetUpperBound(0)

                    If v_sUsername.Trim() = CStr(m_ArrDeletedUsers(lRow)).Trim() Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Next
            End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckUsernameFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckUsername", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' PopulateUserFields
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>New Option to switch off Editing of Schemes Type policies</remarks>
    Public Function PopulateUserFields() As Integer

        Dim nResult As Integer
        Dim oParams As Object
        Dim sOptionValue As String = ""

        Try

            nResult = PMEReturnCode.PMTrue
            ReDim oParams(kParamsCount)

            pnlUsername.Name = m_sUsername
            lblUsername.Text = m_sUsername
            txtFullName.Text = m_sFullname
            txtInitials.Text = m_sInitials
            txtTelephoneNumber.Text = m_sTelephoneNumber
            txtFaxNumber.Text = m_sFaxNumber
            txtExtensionNumber.Text = m_sExtensionNumber
            txtEmailAddress.Text = m_sEmailAddress
            txtSSOPreferredName.Text = m_sSSOPreferredName
            txtMobileNumber.Text = m_sMobileNumber

            pnlEffectiveDate.Name = m_dtUserEffectiveDate

            lblEffectiveDatePanel.Text = m_dtUserEffectiveDate

            pnlLastLogin.Name = m_dtLastLogin
            lblLastLoginPanel.Text = m_dtLastLogin
            pnlPasswordChange.Name = m_dtPasswordchange
            lblPasswordChangePanel.Text = m_dtPasswordchange
            pnlDomainUserName.Name = m_vAlternativeIdentifier
            lblDomainUserNamePanel.Text = m_vAlternativeIdentifier
            chkPrinterChangable.CheckState = m_iIsPrinterChangable
            ddTitle.Text = m_sTitle
            Me.chkSiriusUser.CheckState = IIf(m_bSiriusUser, CheckState.Checked, CheckState.Unchecked)

            If m_lJobTitleId <> -1 Then
                For iRow As Integer = 0 To cboJobTitle.Items.Count
                    If VB6.GetItemData(cboJobTitle, iRow) = m_lJobTitleId Then
                        cboJobTitle.SelectedIndex = iRow
                        Exit For
                    End If
                Next
            Else : cboJobTitle.SelectedIndex = -1
            End If

            If cboPrinter.Items.Count > 0 Then
                If m_sDefPrinter = "" Then
                    cboPrinter.SelectedIndex = 0
                ElseIf (m_sDefPrinter <> "") Then
                    cboPrinter.SelectedIndex = -1
                    ' Loop through the list
                    For iRow As Integer = 0 To cboPrinter.Items.Count

                        ' Create key which contains the row number

                        If m_sDefPrinter = VB6.GetItemString(cboPrinter, iRow) Then
                            cboPrinter.SelectedIndex = iRow
                        End If

                    Next iRow
                    If cboPrinter.SelectedIndex = -1 Then
                        cboPrinter.SelectedIndex = 0
                    End If
                End If
            End If

            'User Authority Tab

            m_lReturn = g_oAuthority.GetDetails(vUserID:=m_iUser)

            'Exit if you get an invalid response
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            'Client Manager Security
            m_lReturn = g_oAuthority.GetNext(vHasWriteOffAuthority:=m_iCanWriteOff,
                                             vWriteOffAmount:=m_cWriteOffAmount,
                                             vHasUnrestrictedEnquiry:=m_iUnrestrictedEnquiry,
                                             vHasUnrestrictedUpdate:=m_iUnrestrictedUpdate,
                                             vFeeDiscount:=m_iFeeDiscount,
                                             vHasTransWriteOffAuthority:=m_iHasTransWriteOffAuthority,
                                             vTransWriteOffAmount:=m_cTransWriteOffAmount,
                                             vHasRefundAuthority:=m_iHasRefundAuthority,
                                             vHasTransferAuthority:=m_iHasTransferAuthority,
                                             vHasPaymentsAuthority:=m_iHasPaymentsAuthority,
                                             vPaymentsAmount:=m_cPaymentsAmount,
                                             vHasClaimPaymentsAuthority:=m_iHasClaimPaymentsAuthority,
                                             vClaimPaymentsAmount:=m_cClaimPaymentsAmount,
                                             vOverrideDate:=m_iOverrideDate, vOverrideRate:=m_iOverrideRate,
                                             vOverridePrePolicyDate:=m_iOverridePrePolicyDate,
                                             vOverridePrePolicyRate:=m_iOverridePrePolicyRate,
                                             vWriteOffCurrencyID:=m_vWriteOffCurrencyID,
                                             vTransWriteOffCurrencyID:=m_vTransWriteOffCurrencyID,
                                             vPaymentsCurrencyID:=m_vPaymentsCurrencyID,
                                             vClaimPaymentsCurrencyID:=m_vClaimPaymentsCurrencyID,
                                             vIsViewClient:=m_iIsViewClient, vIsEditClient:=m_iIsEditClient,
                                             vIsEditPolicy:=m_iIsEditPolicy, vIsEditClaim:=m_iIsViewClaim,
                                             vIsEditFinancePlan:=m_iIsEditFinancePlan,
                                             vIsRaiseDebit:=m_iIsRaiseDebit, vIsRaiseCredit:=m_iIsRaiseCredit,
                                             vIsRaiseFee:=m_iIsRaiseFee, vIsRaiseCash:=m_iIsRaiseCash,
                                             vIsReverseTransactions:=m_iIsReverseTransactions,
                                             vIsReverseAllocations:=m_iIsReverseAllocations,
                                             vIsRaiseManualDID:=m_iIsRaiseManualDID,
                                             vIsDeleteClient:=m_iIsDeleteClient,
                                             vIsPerformAllocations:=m_iIsPerformAllocations,
                                             vCanPerformBrokerTransfer:=m_iCanPerformBrokerTransfer,
                                             vDuplicateClaimOverride:=m_iDuplicateClaimOverride,
                                             vIsDeletePolicy:=m_iIsDeletePolicy,
                                             vIsEditSchemePolicy:=m_iIsEditSchemePolicy,
                                             vCanMakeLiveInvoice:=m_iCanMakeLiveInvoice,
                                             vCAnMakeLiveInstalments:=m_iCanMakeLiveInstalments,
                                             vCanMakeLivePayNow:=m_iCanMakeLivePaynow,
                                             vHasPaynowWriteoffAuthority:=m_iHasPaynowWriteOffAuthority,
                                             vPaynowWriteOffCurrencyID:=m_iPaynowWriteOffCurrencyID,
                                             vPayNowWriteoffAmount:=m_cPaynowWriteOffAmount,
                                             vPostingPeriod:=m_iPostingPeriod,
                                             vUserCanChangeReserves:=m_iUserCanChangeReserves,
                                             vUserCanAddRemoveRatingSections:=m_iUserCanAddRemoveRatingSections,
                                             vUserCanEditExistingRatingSections:=m_iUserCanEditExistingRatingSections,
                                             vIsViewClientManager:=m_iViewClientManager,
                                             vIsViewAgentMaintenance:=m_iAgentMaintenance,
                                             vIsViewAccountHandler:=m_iAccountHandler,
                                             vIsViewAccountExecutive:=m_iAccountExecutive,
                                             vIsViewInsurerMaintenance:=m_iInsurerMaintenance,
                                             vIsViewOtherParty:=m_iOtherPartyMaintenance, vParams:=oParams,
                                             vCurrencyLossGainLimit:=m_cCurrencyLossGainLimit,
                                             vLossGainCurrencyID:=m_vLossGainCurrencyID,
                                             vHasManualJournalAuthority:=m_iHasManualJournalAuthority,
                                             vManualJournalAmount:=m_cManualJournalAmount,
                                             vManualJournalCurrencyID:=m_vManualJournalCurrencyID,
                                             vVoidTransaction:=m_sVoidTransaction)


            If Not False Then
                If Information.IsArray(oParams) Then

                    m_iIsRecommender = ToSafeInteger(CStr(oParams(ACIsRecommenderArrPos)))

                    m_iRecommenderCurrencyID = ToSafeInteger(CStr(oParams(ACRecommendationCurrencyArrPos)))

                    m_cRecommenderCurrencyAmt = ToSafeCurrency(CStr(oParams(ACRecommendationAmountArrPos)))

                    'Party Maintenance
                    m_iCanReverseAllocations = ToSafeInteger(CStr(oParams(ACCanReverseAllocationArrPos)))

                    m_iTimePeriodForReversal = ToSafeInteger(CStr(oParams(ACTimePeriodForReversalArrPos)))

                    'Find Transaction Changes
                    m_iCanReverseReplaceTransactions = ToSafeInteger(CStr(oParams(ACCanReverseReplaceTransactionsArrPos)))

                    m_iMTAAuthority = ToSafeInteger(CStr(oParams(ACMTAAuthorityArrPos)))

                    m_iOverrideChequeNumber = ToSafeInteger(CStr(oParams(ACChequeNumberArrPos)))

                    m_iDisplayReinsuranceScreen = ToSafeInteger(CStr(oParams(ACDisplayReinsuranceScreen)))

                    m_iDisplayClaimReinsurance = ToSafeInteger(CStr(oParams(ACDisplayClaimReinsurance)))
                    m_iCanMakeLiveBankGuarantee = ToSafeInteger(CStr(oParams(ACMakeLiveBankGuarantee)))

                    m_iCanBackdateCollectionDate = ToSafeInteger(CStr(oParams(ACCanBackdateCollectionDate)))

                    m_iEditDefaultCommission = ToSafeInteger(CStr(oParams(ACEditDefaultCommission)))

                    m_iCanChangeInstalmentPlanDefaultCurrecny = gPMFunctions.ToSafeInteger(CStr(oParams(ACUserCanChangeInstalmentDefaultCurrency)))

                    m_iCanMakeLiveCashDeposit = ToSafeInteger(CStr(oParams(ACMakeLiveCashDeposit)))

                    m_iUserCanDebugDynamicLogicScripts = ToSafeInteger(CStr(oParams(ACUserCanDebugDynamicLogicScripts)))

                    m_iUserServerScriptsRunInDebug = ToSafeInteger(CStr(oParams(ACUserServerScriptsRunInDebug)))

                    m_nEditAgentDuringMTAMTC = ToSafeInteger(CStr(oParams(kEditAgentDuringMTAMTC)))

                    m_nEditDefaultCommissionNBRN = ToSafeInteger(CStr(oParams(kEditDefaultCommissionNBRN)))

                    m_nEditDefaultCommissionMTA = ToSafeInteger(CStr(oParams(kEditDefaultCommissionMTA)))

                    m_nEditDefaultCommissionMTC = ToSafeInteger(CStr(oParams(kEditDefaultCommissionMTC)))

                    m_nEditDefaultCommissionMTR = ToSafeInteger(CStr(oParams(kEditDefaultCommissionMTR)))

                    m_nAllowReceiptReversal = ToSafeInteger(CStr(oParams(ACCanReverseReceiptArrPos)))

                    m_iCanMakeLiveCashDeposit = gPMFunctions.ToSafeInteger(CStr(oParams.GetValue(ACMakeLiveCashDeposit))) 'Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
                    m_iUserCanDebugDynamicLogicScripts = gPMFunctions.ToSafeInteger(CStr(oParams.GetValue(ACUserCanDebugDynamicLogicScripts)))
                    m_iUserServerScriptsRunInDebug = gPMFunctions.ToSafeInteger(CStr(oParams.GetValue(ACUserServerScriptsRunInDebug)))
                    m_nViewBatchProcessStatus = ToSafeInteger(CStr(oParams(kACCViewBatchProcessStatus)))
                    m_nCanExtractClientData = ToSafeInteger(CStr(oParams(kACCCanExtractClientData)))
                    m_nInstalmentStatus = ToSafeInteger(oParams(ACInstalmentStatus))
                    m_nCanEditInstalmentDate = ToSafeInteger(oParams(ACCanEditInstalmentDueDate))
                    m_nEditInstalmentDateByNoOfDays = ToSafeInteger(oParams(ACEditInstalmentDateByNoOfDays))
                End If
            End If

            chkUnrestrictedUpdate.CheckState = m_iUnrestrictedUpdate
            chkUnrestrictedEnquiry.CheckState = m_iUnrestrictedEnquiry
            chkViewBatchProcessStatus.CheckState = m_nViewBatchProcessStatus
            chkCanExtractClientData.CheckState = m_nCanExtractClientData

            chkCanPerformBrokerTransfer.CheckState = m_iCanPerformBrokerTransfer

            chkWriteOffs.CheckState = m_iCanWriteOff
            txtWriteOff.Text = IIf(m_iCanWriteOff = CheckState.Checked, StringsHelper.Format(m_cWriteOffAmount, "0.00"), "")
            If Not IsNothing(m_cCurrencyLossGainLimit) Then
                txtCurrencyLossGainLimit.Text = StringsHelper.Format(m_cCurrencyLossGainLimit, "0.00")
            Else
                txtCurrencyLossGainLimit.Text = ""
            End If
            If m_vLossGainCurrencyID <> 0 Then
                cboLossGainCurrency.CurrencyId = m_vLossGainCurrencyID
            End If
            cboWriteOffsCurrency.CurrencyId = m_vWriteOffCurrencyID
            			
			 If m_sVoidTransaction IsNot Nothing AndAlso m_sVoidTransaction <> "" Then
                ' Check if the ComboBox contains this value first
                If cboVoidTransaction.Items.Contains(m_sVoidTransaction) Then
                    cboVoidTransaction.SelectedItem = m_sVoidTransaction
                End If
            End If
			

            EnableWriteOff()

            chkPayments.CheckState = m_iHasPaymentsAuthority
            txtPayments.Text = IIf(m_iHasPaymentsAuthority = CheckState.Checked,
                                   StringsHelper.Format(m_cPaymentsAmount, "0.00"), "")

            If m_iHasPaymentsAuthority Then
                If m_vPaymentsCurrencyID <> 0 Then
                    cboPaymentsCurrency.CurrencyId = m_vPaymentsCurrencyID
                Else
                    cboPaymentsCurrency.CurrencyId = g_oObjectManager.CurrencyID
                End If
            End If 

            EnablePayments()

            ChkManualJournal.CheckState = m_iHasManualJournalAuthority
            txtjournalAmount.Text = IIf(m_iHasManualJournalAuthority = CheckState.Checked,
                                        StringsHelper.Format(m_cManualJournalAmount, "0.00"), "")
            If m_iHasManualJournalAuthority Then
                If m_vManualJournalCurrencyID <> 0 Then
                    cboJournalCurrency.CurrencyId = m_vManualJournalCurrencyID
                Else
                    cboJournalCurrency.CurrencyId = g_oObjectManager.CurrencyID
                End If
            End If
            EnableManualJournalPayments()

            chkClaimPayments.CheckState = m_iHasClaimPaymentsAuthority
            txtClaimPayments.Text = IIf(m_iHasClaimPaymentsAuthority = CheckState.Checked,
                                        StringsHelper.Format(m_cClaimPaymentsAmount, "0.00"), "")
            If m_iHasClaimPaymentsAuthority Then
                If m_vClaimPaymentsCurrencyID <> 0 Then
                    cboClaimPaymentsCurrency.CurrencyId = m_vClaimPaymentsCurrencyID
                Else
                    cboClaimPaymentsCurrency.CurrencyId = g_oObjectManager.CurrencyID
                End If
            End If
            EnableClaimPayments()

            chkOverrideDate.CheckState = m_iOverrideDate
            chkOverrideRate.CheckState = m_iOverrideRate

            chkDuplicateClaimOverride.CheckState = m_iDuplicateClaimOverride

            chkPostingPeriod.CheckState = m_iPostingPeriod
            chkOverrideChequeNumber.CheckState = m_iOverrideChequeNumber
            chkUserCanChangeReserves.CheckState = m_iUserCanChangeReserves

            chkAddRemoveRatingSections.CheckState = m_iUserCanAddRemoveRatingSections
            chkEditRatingSections.CheckState = m_iUserCanEditExistingRatingSections

            chkOverridePrePolicyDate.CheckState = m_iOverridePrePolicyDate
            chkOverridePrePolicyRate.CheckState = m_iOverridePrePolicyRate

            chkDisplayReinsuranceScreen.CheckState = m_iDisplayReinsuranceScreen
            chkDisplayClaimReinsurance.CheckState = m_iDisplayClaimReinsurance

            chkEditDefaultCommission.CheckState = m_iEditDefaultCommission
            If chkEditDefaultCommission.CheckState Then
                chkEditDefaultCommissionNBRN.Enabled = True
                chkEditDefaultCommissionMTA.Enabled = True
                chkEditDefaultCommissionMTC.Enabled = True
                chkEditDefaultCommissionMTR.Enabled = True
            Else
                chkEditDefaultCommissionNBRN.Enabled = False
                chkEditDefaultCommissionMTA.Enabled = False
                chkEditDefaultCommissionMTC.Enabled = False
                chkEditDefaultCommissionMTR.Enabled = False
            End If

            chkCanChangeInstalmentPlanDefaultCurrecny.CheckState = m_iCanChangeInstalmentPlanDefaultCurrecny
            chkOverrideCollectionDate.CheckState = m_iCanBackdateCollectionDate
            chkBankGuarantee.CheckState = m_iCanMakeLiveBankGuarantee
            chkCashDeposit.CheckState = m_iCanMakeLiveCashDeposit
            chkEditDefaultCommissionNBRN.CheckState = m_nEditDefaultCommissionNBRN
            chkEditAgentDuringMTAMTC.CheckState = m_nEditAgentDuringMTAMTC
            chkEditDefaultCommissionMTA.CheckState = m_nEditDefaultCommissionMTA
            chkEditDefaultCommissionMTC.CheckState = m_nEditDefaultCommissionMTC
            chkEditDefaultCommissionMTR.CheckState = m_nEditDefaultCommissionMTR
            'Client Manager Security
            chkIsViewClient.CheckState = m_iIsViewClient
            chkIsEditClient.CheckState = m_iIsEditClient
            chkIsDeleteClient.CheckState = m_iIsDeleteClient
            chkIsEditPolicy.CheckState = m_iIsEditPolicy
            chkIsDeletePolicy.CheckState = m_iIsDeletePolicy
            chkIsViewClaim.CheckState = m_iIsViewClaim
            chkIsEditFinancePlan.CheckState = m_iIsEditFinancePlan
            chkIsRaiseDebit.CheckState = m_iIsRaiseDebit
            chkIsRaiseCredit.CheckState = m_iIsRaiseCredit
            chkIsRaiseFee.CheckState = m_iIsRaiseFee
            chkIsRaiseCash.CheckState = m_iIsRaiseCash
            chkIsReverseTransactions.CheckState = m_iIsReverseTransactions
            chkCanReverseReplaceTransactions.CheckState = m_iCanReverseReplaceTransactions
            chkIsPerformAllocations.CheckState = m_iIsPerformAllocations
            chkIsReverseAllocations.CheckState = m_iIsReverseAllocations
            chkIsRaiseManualDID.CheckState = m_iIsRaiseManualDID
            chkIsEditSchemePolicy.CheckState = m_iIsEditSchemePolicy
            chkUserCanDebugDynamicLogicScripts.CheckState = m_iUserCanDebugDynamicLogicScripts
            chkUserServerScriptsRunInDebug.CheckState = m_iUserServerScriptsRunInDebug

            'Make Live Group is Visible only to S4I

            chkInvoice.CheckState = m_iCanMakeLiveInvoice
            chkPayNow.CheckState = m_iCanMakeLivePaynow
            chkInstalments.CheckState = m_iCanMakeLiveInstalments
            chkHasPaynowWriteOffAuthority.CheckState = m_iHasPaynowWriteOffAuthority
            cboMakeLiveCurrency.CurrencyId = m_iPaynowWriteOffCurrencyID
            txtPaynowWriteOffAmount.Text = IIf(m_iHasPaynowWriteOffAuthority = CheckState.Checked,
                                               StringsHelper.Format(m_cPaynowWriteOffAmount, "0.00"), "")
            'Party View
            chkIsViewClientManager.CheckState = m_iViewClientManager
            chkAgentMaintenance.CheckState = m_iAgentMaintenance
            chkAccountHandler.CheckState = m_iAccountHandler
            chkAccountExecutive.CheckState = m_iAccountExecutive
            chkInsurerMaintenance.CheckState = m_iInsurerMaintenance
            chkOtherPartyMaintenance.CheckState = m_iOtherPartyMaintenance

            ' To give priority to System Option over User Authority Option
            m_lReturn = iPMFunc.RetrieveSingleSystemOption(v_iOptionNumber:=5000, r_sOptionValue:=sOptionValue,
                                                           v_iSourceID:=g_iSourceID)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                m_lErrorNumber = PMEReturnCode.PMFalse
                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)
                Return nResult
            End If

            If sOptionValue <> "1" Then
                chkIsViewClientManager.CheckState = CheckState.Checked
                chkIsViewClientManager.Enabled = False
            End If

            If m_iCanMakeLivePaynow = CheckState.Unchecked Then EnableUserPayNowWriteOffGrp()
            EnablePayNowWriteOff()
            If m_iMTAAuthority > 0 Then
                For iRow As Integer = 0 To cboMTAAuthority.Items.Count
                    If VB6.GetItemData(cboMTAAuthority, iRow) = m_iMTAAuthority Then
                        cboMTAAuthority.SelectedIndex = iRow
                        Exit For
                    End If
                Next
            Else : cboMTAAuthority.SelectedIndex = -1
            End If

            chkRecommender.CheckState = m_iIsRecommender
            If m_iRecommenderCurrencyID = 0 Then
            Else
                cboRecommandationCurrency.CurrencyId = m_iRecommenderCurrencyID
            End If
            txtRecommendAmount.Text = StringsHelper.Format(m_cRecommenderCurrencyAmt, "0.00")
            EnableRecommendPayments()

            'Payment Maintenance
            chkReverseAllocation.CheckState = m_iCanReverseAllocations
            If m_iCanReverseAllocations = 1 Then
                txtTimePeriod.Text = CStr(m_iTimePeriodForReversal)
            Else
                txtTimePeriod.Text = ""
            End If

            chkEditInstalment.CheckState = m_nCanEditInstalmentDate
            txtEditInstalmentByNoofDays.Text = IIf(m_nCanEditInstalmentDate = CheckState.Checked, m_nEditInstalmentDateByNoOfDays, "")

            chkReceiptReversal.CheckState = m_nAllowReceiptReversal

            chkInstalmentStatus.CheckState = m_nInstalmentStatus

            m_lReturn = EnableAuthorityReversePayment()
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            m_lReturn = PopulateUserCompanies()
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            m_lReturn = PopulateUserGroups()
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            m_lReturn = PopulateUserParty()
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            If m_sSignatureDir <> "" AndAlso m_sSignatureFile <> "" Then
                Dim ioStrm As IO.Stream
                ioStrm = File.OpenRead(m_sSignatureDir & m_sSignatureFile)
                imgSignature.Image = Image.FromStream(ioStrm)
                ioStrm.Close()
            Else
                imgSignature.Image = Nothing
            End If

            Return nResult

        Catch excep As System.Exception


            nResult = PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="PopulateUserFieldsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateUserFields", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

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

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to retrieve all of the lookup
            ' descriptions for a given lookup type.
            ' The GetLookupDetails function will allow you to do this.
            '
            ' Example:-
            '
            '    m_lReturn& = GetLookupDetails( _
            ''        sLookupTable:=PMLookupCodeName, _
            ''        ctlLookup:=cmbCodeName)
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            m_lReturn = GetLookupDetails(sLookupTable:="job_title", ctlLookup:=cboJobTitle)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




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
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Get all of the lookup values.

                    m_lReturn = g_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Get all of the lookup values with the correct
                    ' effective date.

                    m_lReturn = g_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDelete
                    ' Get lookup values for viewing only.

                    m_lReturn = g_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")

                Return result
            End If

            Return result

        Catch excep As System.Exception



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

            If ctlLookup.Name = "cboJobTitle" Then


                Dim listIndex As Integer = ctlLookup.Items.Add(New VB6.ListBoxItem("", 0))
            End If

            bFoundMatch = False

            'RWH(18/09/2000) Check array is not empty first.
            If Information.IsArray(m_vLookupValues) Then
                For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                    ' Check for a match of the table name.
                    If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
                        ' Found a match
                        bFoundMatch = True
                        Exit For
                    End If
                Next lRow
            End If

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


                Dim listIndex As Integer = ctlLookup.Items.Add(New VB6.ListBoxItem(m_vLookupDetails(ACDetailDesc, lCntr), CInt(m_vLookupDetails(ACDetailKey, lCntr))))


                'SP150998 - compare long value not string
                ' Check if this is the selected index.
                If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
                    If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then



                        Dim iIndexVal As Integer
                        iIndexVal = CType(ctlLookup, ComboBox).Items.Add(m_vLookupDetails(ACDetailDesc, lCntr))
                        CType(ctlLookup, ComboBox).SelectedIndex = iIndexVal
                    End If
                End If

            Next lCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to zero.
            If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then


                CType(ctlLookup, ComboBox).SelectedIndex = 0
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'PN7412

    ' ***************************************************************** '
    ' Name: SetFirstLastControls
    '
    ' Description: Sets the first and last data entry controls for
    '              each tab to the control array, for use with the
    '              keyboard navigation.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (SetFirstLastControls) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function SetFirstLastControls() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Initialise the control array with the number of
    ' tabs which contain data entry fields on (Remember
    ' that arrays start from zero, therefore you must
    ' subtract one from the number of tabs).
    ''ReDim m_ctlTabFirstLast(1, 5)
    '
    ' Set the first and last data entry controls for
    ' all of the tabs.
    '
    ' {* USER DEFINED CODE (Begin) *}
    '
    ' ************************************************************
    ' Enter your code here to set the first and last data entry
    ' controls for all of the tabs.
    ''
    ' Example:-
    ''
    '    Set m_ctlTabFirstLast(ACControlStart, 0) = txtName
    '    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtAge
    ''
    ' NOTE: Replace this section with your new code.
    ' ************************************************************
    'm_ctlTabFirstLast(ACControlStart, 0) = ddTitle
    'm_ctlTabFirstLast(ACControlEnd, 0) = cmdNext(0)
    '
    'm_ctlTabFirstLast(ACControlStart, 1) = cboPrinter
    'm_ctlTabFirstLast(ACControlEnd, 1) = cmdNext(1)
    '
    'm_ctlTabFirstLast(ACControlStart, 2) = lvwBranches
    'm_ctlTabFirstLast(ACControlEnd, 2) = cmdNext(2)
    '
    'm_ctlTabFirstLast(ACControlStart, 3) = lvwGroups
    'm_ctlTabFirstLast(ACControlEnd, 3) = cmdNext(3)
    '
    'm_ctlTabFirstLast(ACControlStart, 4) = chkUnrestrictedEnquiry
    'm_ctlTabFirstLast(ACControlEnd, 4) = cmdNext(4)
    '
    'm_ctlTabFirstLast(ACControlStart, 5) = lvwRiskGroups
    'm_ctlTabFirstLast(ACControlEnd, 5) = cmdPrevious(4)
    ''
    ' {* USER DEFINED CODE (End) *}
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    Private Sub txtMobileNumber_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMobileNumber.Leave
        Dim dbNumericTemp As Double
        If (Not Double.TryParse(txtMobileNumber.Text.Replace(" ", ""), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) And txtMobileNumber.Text.Trim() <> "" Then
            MessageBox.Show("Please enter a valid number", "Invalid number entered - Mobile Number", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtMobileNumber.Focus()
            Exit Sub
        End If
    End Sub

    Private Sub txtPayments_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles txtPayments.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        '''PN68816    m_oFormFields.LostFocus causes screen to Flicker
        If txtPayments.Text.Trim() <> "" Then
            Dim dbNumericTemp As Double
            If Not Double.TryParse(txtPayments.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                MessageBox.Show("Please enter a valid amount", "Invalid amount entered - Amount", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtPayments.Text = ""
                Cancel = True
            End If
        End If
        eventArgs.Cancel = Cancel
    End Sub

    Private Sub txtPaynowWriteOffAmount_KeyPress(ByVal KeyAscii As Integer)
        If KeyAscii = 8 Then Exit Sub
        If Len(txtPaynowWriteOffAmount.Text) >= 15 Then
            KeyAscii = 0
        End If
    End Sub

    Private Sub txtPaynowWriteOffAmount_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles txtPaynowWriteOffAmount.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        'PN68816    m_oFormFields.LostFocus causes screen to Flicker
        If txtPaynowWriteOffAmount.Text.Trim() <> "" Then
            Dim dbNumericTemp As Double
            If Not Double.TryParse(txtPaynowWriteOffAmount.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                MessageBox.Show("Please enter a valid amount", "Invalid amount entered - Amount", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtPaynowWriteOffAmount.Text = ""
                Cancel = True
            End If
        End If
        eventArgs.Cancel = Cancel
    End Sub

    Private Sub txtPercentHoursWorked_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPercentHoursWorked.Enter

        m_oFormFields.GotFocus(txtPercentHoursWorked)

    End Sub

    Private Sub txtPercentHoursWorked_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPercentHoursWorked.Leave

        m_oFormFields.LostFocus(txtPercentHoursWorked)

    End Sub

    Private Sub txtTelephoneNumber_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTelephoneNumber.Leave
        Dim dbNumericTemp As Double
        If (Not Double.TryParse(txtTelephoneNumber.Text.Replace(" ", ""), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) And txtTelephoneNumber.Text.Trim() <> "" Then
            MessageBox.Show("Please enter a valid number", "Invalid number entered - Direct Dial Number", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtTelephoneNumber.Focus()
            Exit Sub
        End If
    End Sub

    Private Sub txtTimePeriod_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtTimePeriod.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If (KeyAscii >= 48 And KeyAscii <= 57) Or KeyAscii = 8 Then
            'Fill Time Period
        Else
            KeyAscii = 0
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub


    Private Sub txtWriteOff_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtWriteOff.Leave
        Dim dbNumericTemp As Double
        If (Not Double.TryParse(txtWriteOff.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) And txtWriteOff.Text.Trim() <> "" Then
            MessageBox.Show("Please enter a valid amount", "Invalid amount entered - Writeoff Amount", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtWriteOff.Focus()
            Exit Sub
        End If
    End Sub


    ' ***************************************************************** '
    '
    ' Name: EnableWriteOff
    '
    ' Description: Enables or disables the write off group
    '
    ' ***************************************************************** '
    Private Function EnableWriteOff() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If chkWriteOffs.CheckState = CheckState.Checked Then
                txtWriteOff.Enabled = True
                lblWriteoffAmount.Enabled = True
                lblWriteoffText.Text = "Yes"
                lblWriteOffsCurrency.Enabled = True
                cboWriteOffsCurrency.Enabled = True
            Else
                txtWriteOff.Enabled = False
                lblWriteoffAmount.Enabled = False
                lblWriteoffText.Text = "No"
                lblWriteOffsCurrency.Enabled = False
                cboWriteOffsCurrency.Enabled = False
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnableWriteOff Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnableWriteOff", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: EnablePayNowWriteOff
    '
    ' Description: Enables or disables the write off group
    '
    ' ***************************************************************** '
    Private Function EnablePayNowWriteOff() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If chkHasPaynowWriteOffAuthority.CheckState = CheckState.Checked Then
                txtPaynowWriteOffAmount.Enabled = True
                LblPaynowAmount.Enabled = True
                lblPaynowCurrency.Enabled = True
                cboMakeLiveCurrency.Enabled = True
            Else
                txtPaynowWriteOffAmount.Enabled = False
                LblPaynowAmount.Enabled = False
                lblPaynowCurrency.Enabled = False
                cboMakeLiveCurrency.Enabled = False
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnablePayNowWriteOff Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnablePayNowWriteOff", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: EnableUserPayNowWriteOffGrp
    '
    ' Description: Enables or disables the write off group
    '
    ' ***************************************************************** '
    Private Function EnableUserPayNowWriteOffGrp() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If chkPayNow.CheckState = CheckState.Checked Then
                chkHasPaynowWriteOffAuthority.Enabled = True
                chkHasPaynowWriteOffAuthority.CheckState = CheckState.Unchecked
            Else
                chkHasPaynowWriteOffAuthority.Enabled = False
                chkHasPaynowWriteOffAuthority.CheckState = CheckState.Unchecked
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnableUserPayNowWriteOffGrp Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnableUserPayNowWriteOffGrp", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: EnableClaimPayments
    '
    ' Description: Enables or disables the claim payments group
    '
    ' ***************************************************************** '
    Private Function EnableClaimPayments() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If chkClaimPayments.CheckState = CheckState.Checked Then
                cboClaimPaymentsCurrency.Enabled = True
                lblClaimPaymentsCurrency.Enabled = True
                txtClaimPayments.Enabled = True
                lblClaimPayments.Enabled = True
            Else
                cboClaimPaymentsCurrency.Enabled = False
                lblClaimPaymentsCurrency.Enabled = False
                txtClaimPayments.Enabled = False
                lblClaimPayments.Enabled = False
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnableClaimPayments Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnableClaimPayments", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Function EnableRecommendPayments() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            If Not m_bCheckIsRecommendClaimPaymentEnabledatProduct Then
                chkRecommender.CheckState = CheckState.Unchecked
            End If

            If chkRecommender.CheckState = "1" Then
                txtRecommendAmount.Enabled = True
                cboRecommandationCurrency.Enabled = True
                lblRecommenderAmount.Enabled = True
                lblRecommenderCurrency.Enabled = True
            Else
                txtRecommendAmount.Enabled = False
                cboRecommandationCurrency.Enabled = False
                lblRecommenderAmount.Enabled = False
                lblRecommenderCurrency.Enabled = False
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnableRecommendPayments Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnableRecommendPayments", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: EnablePayments
    '
    ' Description: Enables or disables the payments group
    '
    ' ***************************************************************** '
    Private Function EnablePayments() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If chkPayments.CheckState = CheckState.Checked Then
                cboPaymentsCurrency.Enabled = True
                lblPaymentsCurrency.Enabled = True
                txtPayments.Enabled = True
                lblPaymentsText.Text = "Yes"
                lblPayments.Enabled = True
            Else
                cboPaymentsCurrency.Enabled = False
                lblPaymentsCurrency.Enabled = False
                txtPayments.Enabled = False
                lblPaymentsText.Text = "No"
                lblPayments.Enabled = False
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnablePayments Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnablePayments", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Function EnableManualJournalPayments() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If ChkManualJournal.CheckState = CheckState.Checked Then
                cboJournalCurrency.Enabled = True
                lblJournalCurrency.Enabled = True
                txtjournalAmount.Enabled = True
                lblmanulJournal.Text = "Yes"

                lblJournalAmount.Enabled = True
            Else
                cboJournalCurrency.Enabled = False
                lblJournalCurrency.Enabled = False
                txtjournalAmount.Enabled = False
                lblmanulJournal.Text = "No"
                lblJournalAmount.Enabled = False
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnablePayments Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnablePayments", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    '****************************************************************************
    '
    ' Name: RefreshListOnSecurityTab
    '
    ' Description: Refreshes the listview boxes on system seurity tab with data held in
    '              the business, not the database.
    '****************************************************************************
    Private Function RefreshListOnSecurityTab() As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim oListitem As ListViewItem
        Dim sMappedDomainUser As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


            g_oBusiness.CurrentRecord = 0

            'Clear the items in the lvwSiriusUsers
            lvwSiriusUsers.Items.Clear()

            'Clear the items in the lvwMatchedUsers
            lvwMatchedUsers.Items.Clear()

            lRow = -1


            m_lReturn = g_oBusiness.getdetails

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            Do
                'Get id of first record

                m_lReturn = g_oBusiness.GetNext(vUserId:=m_iUser, vUsername:=m_sUsername, vIsDeleted:=m_iIsDeleted, vAlternativeIdentifier:=m_vAlternativeIdentifier)

                'Exit if you get an invalid response
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Do
                End If

                lRow += 1

                If m_iIsDeleted = gPMConstants.PMEReturnCode.PMFalse Then
                    If m_vAlternativeIdentifier.Length Then

                        oListitem = lvwMatchedUsers.Items.Add("L" & m_iUser, m_sUsername.Trim(), "user")
                        ListViewHelper.GetListViewSubItem(oListitem, 1).Text = m_vAlternativeIdentifier

                        'Delete alternative Identifier from lvwDomainUsers
                        sMappedDomainUser = Mid(ListViewHelper.GetListViewSubItem(oListitem, 1).Text, (ListViewHelper.GetListViewSubItem(oListitem, 1).Text.IndexOf("\"c) + 1) + 1)

                        For nIndex As Integer = lvwDomainUsers.Items.Count To 1 Step -1
                            If lvwDomainUsers.Items.Item(nIndex - 1).Text = sMappedDomainUser Then
                                lvwDomainUsers.Items.RemoveAt(nIndex - 1)
                            End If
                        Next nIndex
                    Else

                        oListitem = lvwSiriusUsers.Items.Add("L" & m_iUser, m_sUsername.Trim(), "")
                        oListitem.ImageKey = "user"
                        'ListViewHelper.GetListViewSubItem(oListitem, 1).Text = CStr(m_iUser)
                    End If
                End If
            Loop

            'lvwUsers_ItemClick lvwUsers.SelectedItem

            If cboSystemSecurity.Items.Count = 0 Then
                cboSystemSecurity.Items.Clear()
                cboSystemSecurity.Items.Add("Standard Sirius Logins Only")
                cboSystemSecurity.Items.Add("Sirius and Unified Logins (Mixed Mode)")
                cboSystemSecurity.Items.Add("Unified (Windows) Logins Only")
                cboSystemSecurity.SelectedIndex = m_vSystemSecurityModel
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Return result

        Catch excep As System.Exception



            'Error Section

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Refresh user details ", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshListOnSecurityTab", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '****************************************************************************
    '
    ' Name: LoadDomains
    '
    ' Description: Load all available domain names into cboDomain
    '
    '****************************************************************************

    Private Function LoadDomains() As Integer
        Dim result As Integer = 0
        Dim vDomain As Object

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            cboDomain.Items.Clear()


            m_lReturn = gPMFunctions.GetAvailableNTDomains(vDomain)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                result = gPMConstants.PMEReturnCode.PMError

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to retrive domain names ", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadDomains", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            'PN - 68720 start added by chandra shehkar on 12 March 2010
            cboDomain.Items.Add("")
            'PN - 68720 end

            If Information.IsArray(vDomain) Then

                For lRow As Integer = 0 To vDomain.GetUpperBound(0)

                    cboDomain.Items.Add(CStr(vDomain(lRow)))
                Next
                cboDomain.SelectedIndex = 0
            End If

            result = gPMConstants.PMEReturnCode.PMTrue
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Return result

        Catch excep As System.Exception



            'Error Section

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load domain names ", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadDomains", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Return result

        End Try
    End Function

    '****************************************************************************
    '
    ' Name: LoadDomainUsers
    '
    ' Description: Load all available domain user names into lvwDomainUsers
    '
    '****************************************************************************
    Private Function LoadDomainUsers(ByVal r_sServerName As String) As Integer
        Dim result As Integer = 0
        Dim vDomainUsers As Object
        Dim oListitem As ListViewItem
        Dim bIsDomainUserMapped As Boolean
        Dim sDomainExt As String = String.Empty

        result = gPMConstants.PMEReturnCode.PMFalse
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
        lvwDomainUsers.Items.Clear()


        'This might raise an error if there is not any item present with this key
        Try


            vDomainUsers = m_colDomainUsersData.Item(r_sServerName)

        Catch excep As System.Exception
            'Control comes here if key not found in collection, Err.Number=5
            'Retrieve the Domain Users
            'PN23693
            'check for m_sAD_OU_Path
            If m_bLDAPQueryProvided = gPMConstants.PMEReturnCode.PMFalse Then
                If r_sServerName = "SSPDEV" Then
                    m_lReturn = gPMFunctions.GetDomainUsers(vDomainUsers, r_sServerName & ".LOCAL")
                Else
                    If r_sServerName <> "" Then
                        m_lReturn = gPMFunctions.GetDomainUsers(vDomainUsers, r_sServerName & sDomainExt)
                        If vDomainUsers Is Nothing Then
                            m_lReturn = gPMFunctions.GetDomainUsers(vDomainUsers, r_sServerName)
                        End If
                    End If
                End If
            Else

                m_lReturn = gPMFunctions.GetUsersFromLDAP(m_sAD_OU_Path, vDomainUsers)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                MessageBox.Show("Not able to fetch users from domain, Domain : " & r_sServerName, "Check domain", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to retrive domain users ", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadDomainUsers", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If
            'Add Domain User List to Collection
            m_colDomainUsersData.Add(vDomainUsers, r_sServerName)
        End Try

        If Information.IsArray(vDomainUsers) Then

            For lRow As Integer = 0 To vDomainUsers.GetUpperBound(0)
                bIsDomainUserMapped = False
                For lCounter As Integer = 1 To lvwMatchedUsers.Items.Count

                    If ListViewHelper.GetListViewSubItem(lvwMatchedUsers.Items.Item(lCounter - 1), 1).Text.ToUpper() = (r_sServerName & "\" & CStr(vDomainUsers(lRow))).ToUpper() Then

                        bIsDomainUserMapped = True
                    End If
                Next

                If Not bIsDomainUserMapped Then


                    oListitem = lvwDomainUsers.Items.Add("R" & lRow, CStr(vDomainUsers(lRow)), "user")
                End If
            Next
            If Not (lvwDomainUsers.FocusedItem Is Nothing) Then
                lvwDomainUsers.Items.Item(0).Selected = True
            End If
        End If

        result = gPMConstants.PMEReturnCode.PMTrue
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        Return result

err_LoadDomainUsers:

        'Error Section

        result = gPMConstants.PMEReturnCode.PMError

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load domain users ", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadDomainUsers", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: PopulateSystemSecurity
    '
    ' Description: Display data to system security tab.
    '
    ' ***************************************************************** '
    Private Function PopulateSystemSecurity() As Integer

        Dim result As Integer = 0
        Dim sDomainName As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue


        Dim oListView As ListView = lvwDomainUsers
        oListView.HideSelection = True
        oListView.FocusedItem = Nothing



        'Select the current user
        oListView = lvwMatchedUsers
        oListView.HideSelection = True
        oListView.MultiSelect = False
        oListView.FocusedItem = Nothing

        For lRow As Integer = 1 To oListView.Items.Count
            If m_sUsername.Trim().ToUpper() = oListView.Items.Item(lRow - 1).Text.Trim().ToUpper() Then
                oListView.HideSelection = False
                oListView.Items.Item(lRow - 1).Selected = True

                oListView.Items.Item(lRow - 1).EnsureVisible()
                Exit For
            End If
        Next


        oListView = lvwSiriusUsers
        oListView.HideSelection = True
        oListView.FocusedItem = Nothing

        'if current user not found in Matched List View
        'select in Sirius User List view
        If lvwMatchedUsers.FocusedItem Is Nothing Then
            For lRow As Integer = 1 To oListView.Items.Count
                If m_sUsername.Trim().ToUpper() = oListView.Items.Item(lRow - 1).Text.Trim().ToUpper() Then
                    oListView.HideSelection = False
                    oListView.Items.Item(lRow - 1).Selected = True

                    oListView.Items.Item(lRow - 1).EnsureVisible()
                    Exit For
                End If
            Next
        End If

        'PN23693
        If m_bLDAPQueryProvided = gPMConstants.PMEReturnCode.PMFalse Then
            cboDomain.Visible = True
            txtLDAPDomain.Visible = False
        Else
            cboDomain.Visible = False
            txtLDAPDomain.Visible = True
            txtLDAPDomain.Text = m_sAD_OU_Domain
        End If



        If cboDomain.Items.Count = 0 Then

            If m_bLDAPQueryProvided = gPMConstants.PMEReturnCode.PMFalse Then
                m_lReturn = LoadDomains()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If
            End If

            If cboDomain.Visible Then
                m_lReturn = LoadDomainUsers(VB6.GetItemString(cboDomain, cboDomain.SelectedIndex))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If
            ElseIf (txtLDAPDomain.Visible) Then
                m_lReturn = LoadDomainUsers(txtLDAPDomain.Text.Trim().ToUpper())
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If
            End If

            'm_lReturn = LoadDomainUsers(cboDomain.List(cboDomain.ListIndex))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

        End If

        If m_vAlternativeIdentifier.Length Then
            sDomainName = Mid(m_vAlternativeIdentifier, 1, m_vAlternativeIdentifier.IndexOf("\"c))
            If sDomainName.Length Then
                For lRow As Integer = 0 To cboDomain.Items.Count - 1
                    If VB6.GetItemString(cboDomain, lRow).Trim().ToUpper() = sDomainName.Trim().ToUpper() Then
                        cboDomain.SelectedIndex = lRow
                        Exit For
                    End If
                Next

            End If
        End If

        '    oListView.SetFocus

        Return result



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateSystemSecurity Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateSystemSecurity", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateUserMapping
    '
    ' Description: Update new mappings among the sirius and domain users.
    '
    ' ***************************************************************** '
    Private Function UpdateUserMapping() As Integer
        Dim result As Integer = 0
        Dim lUserId As Integer
        Dim sAlternativeIdentifier As String = ""
        Dim ArrDeleteIdentifier(,) As Object

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)



            For lRow As Integer = 1 To lvwMatchedUsers.Items.Count
                lUserId = CInt(Conversion.Val(Mid(lvwMatchedUsers.Items.Item(lRow - 1).Name, 2)))
                sAlternativeIdentifier = ListViewHelper.GetListViewSubItem(lvwMatchedUsers.Items.Item(lRow - 1), 1).Text

                ReDim Preserve ArrDeleteIdentifier(1, lRow - 1)

                ArrDeleteIdentifier(0, lRow - 1) = lUserId
                ArrDeleteIdentifier(1, lRow - 1) = sAlternativeIdentifier

            Next


            m_lReturn = g_oBusiness.UpdateUserMapping(r_lUserId:=lUserId, r_sAlternativeIdentifier:=sAlternativeIdentifier, vResultArray:=ArrDeleteIdentifier, sUniqueId:=m_sUniqueId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateUserMapping Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUserMapping", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If


            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateUserMapping Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUserMapping", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ValidateUserPassword
    '
    ' Description: Validation on password according to the system security models
    '
    ' ***************************************************************** '

    Private Function ValidateUserPassword(ByVal iSystemSecurityModel As Integer, ByVal sUserName As String) As Integer

        Dim result As Integer = 0
        Dim vUsers As Object

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If iSystemSecurityModel = 0 Or iSystemSecurityModel = 1 Then

                m_lReturn = g_oBusiness.GetUsersWithNoPassword(r_vUsers:=vUsers, r_iSecurityModel:=iSystemSecurityModel, r_sUserName:=sUserName)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                If Information.IsArray(vUsers) Then

                    MessageBox.Show(CStr(vUsers(1, 0)) & " has blank Password. Please change password.", "User Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    SSTabHelper.SetSelectedIndex(tabMain, 0)
                    SSTabHelper.SetSelectedIndex(SSTab1, 1)

                    For lRow As Integer = 1 To lvwUsers.Items.Count

                        If CStr(vUsers(1, 0)).Trim().ToUpper() = lvwUsers.Items.Item(lRow - 1).Text.Trim().ToUpper() Then

                            lvwUsers.Items.Item(lRow - 1).Selected = True

                            lvwUsers.Items.Item(lRow - 1).EnsureVisible()

                            lvwUsers_ItemClick(lvwUsers.SelectedItems.Item(0))
                            cmdChangePassword.Focus()
                            Return result
                        End If
                    Next
                End If
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateUserPassword Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateUserPassword", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ValidateUserMapping
    '
    ' Description: Validation on User mapping according to the system security models
    '
    ' ***************************************************************** '

    Private Function ValidateUserMapping(ByVal r_iSecurityMode As Integer) As Integer
        Dim result As Integer = 0
        Dim lUserId, lStatus, lAdminUserCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            Select Case r_iSecurityMode
                Case 0
                    result = gPMConstants.PMEReturnCode.PMTrue

                Case 1
                    'in case of mixed mode model first check for standard administrator
                    'if exist then no need to check further

                    m_lReturn = g_oBusiness.GetAdminUserCount(r_iSecurityModel:=0, r_lAdminUserCount:=lAdminUserCount)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMError
                    End If

                    If lAdminUserCount > 0 Then
                        Return gPMConstants.PMEReturnCode.PMTrue
                    End If

                    'control comes here if no standard administrator found
                    'now check for each map user for administrative rights
                    For lRow As Integer = 1 To lvwMatchedUsers.Items.Count
                        lUserId = CInt(Conversion.Val(Mid(lvwMatchedUsers.Items.Item(lRow - 1).Name, 2)))


                        m_lReturn = g_oBusiness.GetUserAdminStatus(r_iUserId:=lUserId, r_lStatus:=lStatus, r_iSecurityModel:=r_iSecurityMode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMError
                        End If

                        If lStatus > 0 Then
                            Return gPMConstants.PMEReturnCode.PMTrue
                        End If
                    Next

                Case 2
                    For lRow As Integer = 1 To lvwMatchedUsers.Items.Count
                        lUserId = CInt(Conversion.Val(Mid(lvwMatchedUsers.Items.Item(lRow - 1).Name, 2)))


                        m_lReturn = g_oBusiness.GetUserAdminStatus(r_iUserId:=lUserId, r_lStatus:=lStatus, r_iSecurityModel:=r_iSecurityMode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMError
                        End If

                        If lStatus > 0 Then
                            Return gPMConstants.PMEReturnCode.PMTrue
                        End If
                    Next

            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateUserMapping Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateUserMapping", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetProductOption
    '
    ' Description: Set the product option value in hidden_option table
    '
    ' ***************************************************************** '
    Private Function SetProductOption(ByVal r_vOption As Object, ByVal r_vBranch As Object, ByVal r_vValue As Object) As Integer
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue


        m_lReturn = g_oBusiness.SetProductOptionValue(r_vOption:=r_vOption, r_vBranch:=r_vBranch, r_vValue:=r_vValue)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        Return result



        result = gPMConstants.PMEReturnCode.PMError

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProductOption Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProductOption", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: ValidateAdminRights
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function ValidateAdminRights() As Integer

        Dim result As Integer = 0
        Dim lGroupId, lStatus, lAdminUserCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            m_lReturn = g_oBusiness.GetUserAdminStatus(r_iUserId:=m_iUser, r_lStatus:=lStatus, r_iSecurityModel:=m_vSystemSecurityModel)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateAdminRights Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateAdminRights", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            If lStatus = 0 Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If



            m_lReturn = g_oBusiness.GetAdminUserCount(r_iSecurityModel:=m_vSystemSecurityModel, r_lAdminUserCount:=lAdminUserCount)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateAdminRights Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateAdminRights", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            If lAdminUserCount >= 1 Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If Not (lvwSelectedGroups.FocusedItem Is Nothing) And Information.IsArray(m_vUserGroupInfo) Then
                For lRow As Integer = 1 To lvwSelectedGroups.Items.Count
                    lGroupId = CInt(Mid(lvwSelectedGroups.Items.Item(lRow - 1).Name, 2, 5))
                    For lRow1 As Integer = m_vUserGroupInfo.GetLowerBound(1) To m_vUserGroupInfo.GetUpperBound(1)
                        If CDbl(m_vUserGroupInfo(0, lRow1)) = lGroupId Then
                            If CDbl(m_vUserGroupInfo(5, lRow1)) = 1 Then
                                Return gPMConstants.PMEReturnCode.PMTrue
                            End If
                        End If
                    Next
                Next
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateAdminRights Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateAdminRights", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ValidateProductOption
    '
    ' Description: Validation on product option according to the system security models
    '
    ' ***************************************************************** '

    Private Function ValidateProductOption(ByVal iSecurityMode As Integer) As Integer
        Dim result As Integer = 0
        Dim lAdminUserCount As Integer

        Try


            m_lReturn = g_oBusiness.GetAdminUserCount(r_iSecurityModel:=iSecurityMode, r_lAdminUserCount:=lAdminUserCount)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            If lAdminUserCount = 0 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateProductOption Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateProductOption", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdMatch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdMatch.Click
        Dim oListitem As ListViewItem

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        If (Not (lvwSiriusUsers.FocusedItem Is Nothing)) And (Not (lvwDomainUsers.FocusedItem Is Nothing)) Then


            oListitem = lvwMatchedUsers.Items.Add(lvwSiriusUsers.FocusedItem.Name, lvwSiriusUsers.FocusedItem.Text, "user")
            'test for LDAP path in PMsystem table
            'PN23693
            If m_bLDAPQueryProvided = gPMConstants.PMEReturnCode.PMFalse Then
                ListViewHelper.GetListViewSubItem(oListitem, 1).Text = VB6.GetItemString(cboDomain, cboDomain.SelectedIndex) & "\" & lvwDomainUsers.FocusedItem.Text
            Else
                ListViewHelper.GetListViewSubItem(oListitem, 1).Text = m_sAD_OU_Domain & "\" & lvwDomainUsers.FocusedItem.Text
            End If

            If m_sUsername = lvwSiriusUsers.FocusedItem.Text Then
                m_vAlternativeIdentifier = ListViewHelper.GetListViewSubItem(oListitem, 1).Text
            End If


            lvwSiriusUsers.Items.RemoveAt(lvwSiriusUsers.FocusedItem.Index)
            lvwDomainUsers.Items.RemoveAt(lvwDomainUsers.FocusedItem.Index)

            'Just to HighLight (PN-21876)
            lvwSiriusUsers.FocusedItem = lvwSiriusUsers.FocusedItem
            lvwDomainUsers.FocusedItem = lvwDomainUsers.FocusedItem

            cmdApply.Enabled = True

        End If

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
    End Sub

    Private Sub cmdUnmatch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdUnmatch.Click
        Dim sMappedDomainName, sMappedDomainUser As String
        Dim oListitem As ListViewItem
        Dim lRow As Integer

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        If Not (lvwMatchedUsers.FocusedItem Is Nothing) Then
            lRow = 1
            While (lRow <= lvwMatchedUsers.Items.Count)
                If lvwMatchedUsers.Items.Item(lRow - 1).Selected Then
                    m_lReturn = CanUnMatch(CInt(Mid(lvwMatchedUsers.Items.Item(lRow - 1).Name, 2)))
                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                        oListitem = lvwSiriusUsers.Items.Add(lvwMatchedUsers.Items.Item(lRow - 1).Name, lvwMatchedUsers.Items.Item(lRow - 1).Text, "user")

                        sMappedDomainName = Mid(ListViewHelper.GetListViewSubItem(lvwMatchedUsers.Items.Item(lRow - 1), 1).Text, 1, ListViewHelper.GetListViewSubItem(lvwMatchedUsers.Items.Item(lRow - 1), 1).Text.IndexOf("\"c))
                        sMappedDomainUser = Mid(ListViewHelper.GetListViewSubItem(lvwMatchedUsers.Items.Item(lRow - 1), 1).Text, (ListViewHelper.GetListViewSubItem(lvwMatchedUsers.Items.Item(lRow - 1), 1).Text.IndexOf("\"c) + 1) + 1)
                        'If UCase(sMappedDomainName) = UCase(cboDomain.List(cboDomain.ListIndex)) Then
                        'PN53672
                        If (sMappedDomainName.ToUpper() = VB6.GetItemString(cboDomain, cboDomain.SelectedIndex).ToUpper() And cboDomain.Visible) Or (sMappedDomainName.ToUpper() = txtLDAPDomain.Text.Trim().ToUpper() And txtLDAPDomain.Visible) Then

                            oListitem = lvwDomainUsers.Items.Add(sMappedDomainUser, "user")
                        End If
                        lvwMatchedUsers.Items.RemoveAt(lvwMatchedUsers.Items.Item(lRow - 1).Index)

                        cmdApply.Enabled = True
                    Else
                        lRow += 1
                    End If
                Else
                    lRow += 1
                End If
            End While
        End If
        'This will set the SeletedItem automatically to next or prev.
        '(this is because of Remove operation has been done (of Listview) so jus setting it again)
        lvwMatchedUsers.FocusedItem = lvwMatchedUsers.FocusedItem
        Try

            lvwDomainUsers.FocusedItem.EnsureVisible()

        Catch
        End Try

        Try

            lvwSiriusUsers.FocusedItem.EnsureVisible()
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch exc As System.Exception

        End Try
    End Sub

    Private Sub cboDomain_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboDomain.SelectedIndexChanged
        Dim sDomainName As String = VB6.GetItemString(cboDomain, cboDomain.SelectedIndex)
        m_lReturn = LoadDomainUsers(sDomainName)
    End Sub

    Private Sub cmdChangeDomainAccount_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdChangeDomainAccount.Click
        SSTabHelper.SetSelectedIndex(tabMain, 1)
    End Sub

    Private Sub tabMain_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMain.SelectedIndexChanged
        If SSTabHelper.GetSelectedIndex(tabMain) = 1 Then

            Application.DoEvents()
            cmdOK.Enabled = True
            m_lReturn = PopulateSystemSecurity()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
        ElseIf SSTabHelper.GetSelectedIndex(tabMain) = 0 Then


            If Strings.Len(pnlUsername.Name) < 1 Then
                cmdOK.Enabled = False
            End If
        End If
        tabMainPreviousTab = tabMain.SelectedIndex
    End Sub

    ''' <summary>
    ''' UpdateUserDetails
    ''' </summary>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Private Function UpdateUserDetails() As Integer

        Dim nResult As Integer = 0
        Dim sEncrypt As String = ""
        Dim nRow As Integer
        Dim sKey As String = ""
        Dim eResult As DialogResult
        Dim dPercentHoursWorked As Double
        Dim sOldPassword As String = ""
        Dim bPasswordChanged As Boolean = False

        Try

            nResult = gPMConstants.PMEReturnCode.PMFalse

            'Only save details if there is a user selected.

            If Strings.Len(pnlUsername.Name) = 0 Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            Select Case cboSystemSecurity.SelectedIndex
                Case 0
                    'User must have a password.
                    If m_sPassword.Trim().Length = 0 Then
                        eResult =
                        MessageBox.Show(
                               "A password will be required if this user is not going to use unified login." &
                               Strings.Chr(13) & Strings.Chr(10) & "Do you wish to enter a password now?", "Password",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
                        If eResult = System.Windows.Forms.DialogResult.Yes Then
                            SSTabHelper.SetSelectedIndex(SSTab1, 1)
                            cmdChangePassword_Click(cmdChangePassword, New EventArgs())
                            Return nResult
                        End If
                    End If
                Case 1
                    'In mixed mode password can be used, but is not required if the user uses unified login for that user.
                    If m_sPassword.Trim().Length = 0 And m_vAlternativeIdentifier.Length = 0 Then
                        eResult =
                            MessageBox.Show(
                                "A password will be required if this user is not going to use unified login." &
                                Strings.Chr(13) & Strings.Chr(10) & "Do you wish to enter a password now?", "Password",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
                        If eResult = System.Windows.Forms.DialogResult.Yes Then
                            SSTabHelper.SetSelectedIndex(SSTab1, 1)
                            cmdChangePassword_Click(cmdChangePassword, New EventArgs())
                            Return nResult
                        End If
                    End If
                Case 2
                    'In unified mode no password is required, all users will use windows login.
            End Select

            If lvwSelectedBranches.Items.Count = 0 Then
                MessageBox.Show("User Must Be Assigned To At Least One Branch", "Branch")
                SSTabHelper.SetSelectedIndex(SSTab1, 2)
                lvwBranches.Focus()
                Return nResult
            End If
            With Me

                If .txtEmailAddress.Text <> "" Then
                    If (.txtEmailAddress.Text.IndexOf("@"c) + 1) = 0 Then
                        MessageBox.Show("Please enter a valid email address.", "Email Address", MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation)
                        txtEmailAddress.Focus()
                        Return nResult
                    End If
                End If

                If m_vEnableFSACompliance And (Not m_bSiriusUser) Then

                    Dim dbNumericTemp As Double
                    If _
                        Not _
                        Double.TryParse(CStr(m_oFormFields.UnformatControl(txtPercentHoursWorked)), NumberStyles.Number,
                                        CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                        MessageBox.Show(
                            "The field Percentage of Normal Hours Worked must have a value between 0 and 100",
                            "User Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        txtPercentHoursWorked.Focus()
                        Return nResult
                    End If
                End If

                If m_vEnableFSACompliance And (Not m_bSiriusUser) Then
                    dPercentHoursWorked = gPMFunctions.ToSafeDouble(m_oFormFields.UnformatControl(txtPercentHoursWorked))

                    Dim dbNumericTemp2 As Double
                    If _
                        (Not _
                         Double.TryParse(CStr(m_oFormFields.UnformatControl(txtPercentHoursWorked)), NumberStyles.Number,
                                         CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) Or
                        dPercentHoursWorked < 0 Or dPercentHoursWorked > 100 Then
                        MessageBox.Show(
                            "The field Percentage of Normal Hours Worked must have a value between 0 and 100",
                            "User Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        txtPercentHoursWorked.Focus()
                        Return nResult
                    End If
                End If

                Dim oPartySources As Object
                Dim bFound As Boolean
                Dim sBranchNames As String = ""

                If chkOtherParty.CheckState = CheckState.Checked And m_lOtherPartyId <> 0 Then
                    m_lReturn = g_oBusiness.GetPartySources(m_lOtherPartyId, "OT", oPartySources)
                ElseIf chkAgent(0).CheckState = CheckState.Checked And m_lPartyCnt <> 0 Then
                    m_lReturn = g_oBusiness.GetPartySources(m_lPartyCnt, "AG", oPartySources)
                End If
                If IsArray(oPartySources) Then
                    For Each oItem As ListViewItem In lvwSelectedBranches.Items
                        bFound = False
                        For iCount As Integer = 0 To UBound(oPartySources, 2)
                            If Mid(oItem.Name, 2) = oPartySources(0, iCount) And oPartySources(2, iCount) = 1 Then
                                bFound = True
                                Exit For
                            End If
                        Next
                        If bFound = False Then
                            sBranchNames = sBranchNames & vbNewLine & oItem.Text
                        End If
                    Next
                    If String.IsNullOrEmpty(sBranchNames) = False Then
                        MessageBox.Show(
                            "Cannot Update User. The Party to which this User is assigned does not have access to Branch" &
                            sBranchNames, "User Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return nResult
                    End If
                End If


                .UserName = pnlUsername.Name
                .Title = ddTitle.Text
                .Initials = txtInitials.Text
                .Fullname = txtFullName.Text
                .TelephoneNumber = txtTelephoneNumber.Text
                .ExtensionNumber = txtExtensionNumber.Text
                .MobileNumber = txtMobileNumber.Text
                .FaxNumber = txtFaxNumber.Text
                .EmailAddress = txtEmailAddress.Text
                .SSOPreferredName = txtSSOPreferredName.Text
                .UserEffectiveDate = CDate(pnlEffectiveDate.Name)


                ' if no job title selected - or blank selected
                ' the job title combo has a "blank" item added with listindex 0
                If cboJobTitle.SelectedIndex = -1 Or cboJobTitle.SelectedIndex = 0 Then
                    .JobTitleId = 0
                Else
                    .JobTitleId = VB6.GetItemData(cboJobTitle, cboJobTitle.SelectedIndex)
                End If


                .IsPrinterChangable = chkPrinterChangable.CheckState
                If .cboPrinter.Text = "Default" Then
                    m_sDefPrinter = "-1"
                ElseIf (.cboPrinter.Text <> "Default") Then
                    m_sDefPrinter = .cboPrinter.Text
                End If
                'DC260304 PN11231 filename no longer includes directory
                '.SignatureFile = Replace(LCase(m_sSignatureFile), LCase(m_sSignatureDir), "", 1, 1)
                .SignatureFile = m_sSignatureFile

                If m_iIsDeleted = gPMConstants.PMEReturnCode.PMFalse And m_bDeletedUser Then
                    .IsDeleted = 1
                    .DateDeleted = DateTime.Now
                End If

                If m_iIsDeleted = gPMConstants.PMEReturnCode.PMTrue And m_bUndeletedUser Then
                    .IsDeleted = 0

                    .DateDeleted = Nothing
                End If


                If m_bPasswordChanged Then
                    bPasswordChanged = True
                    'don't need, already assigned if user click on frmPassword->OK
                    '.Password = frmPassword.txtNewPassword.Text


                    pnlPasswordChange.Name = DateTime.Now


                    lblPasswordChangePanel.Text = DateTime.Now


                    .PasswordChange = CDate(pnlPasswordChange.Name)
                    m_bPasswordChanged = False

                    If m_sPassword.Length = 0 And m_sOldPassword.Length <> 0 Then
                        m_sEncrypt = ""
                    Else
                        m_lReturn = iPMFunc.Encrypt(m_sPassword, m_sEncrypt)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            MessageBox.Show("Failed to Encryt Password", "User Maintenance", MessageBoxButtons.OK,
                                            MessageBoxIcon.Information)
                            Return nResult
                        End If

                        m_sOldPassword = bPMFunc.EncryptPassword(m_sPassword, m_sOldPassword)

                        If String.IsNullOrEmpty(m_sOldPassword) Then
                            MessageBox.Show("Failed to Encryt Password", "User Maintenance", MessageBoxButtons.OK,
                                            MessageBoxIcon.Information)
                            Return nResult
                        End If

                    End If

                    If m_iUser > 0 Then
                        Call g_oBusiness.UpdateIncorrectAttemptsAndLockUnlock(m_iUser, 3, 0, False)
                        'RJ
                        Call ClearSAMCache()
                    End If

                Else
                    m_sEncrypt = m_sPassword
                End If

                'Payment Maintenance
                If .chkReverseAllocation.CheckState = CheckState.Checked Then
                    m_lReturn = CheckTimePeriod()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return nResult
                    End If
                End If

                If .chkEditInstalment.CheckState = CheckState.Checked Then
                    m_lReturn = CheckInstalmentNoOfDays()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return nResult
                    End If
                End If

                If chkOtherParty.Checked = False Then
                    .OtherPartyId = 0
                End If

            End With

            If _
                chkOtherParty.CheckState = CheckState.Checked And chkAgent(0).CheckState = CheckState.Unchecked And
                chkAgent(1).CheckState = CheckState.Unchecked Then m_lPartyCnt = 0
            If _chkAgent_1.CheckState = CheckState.Unchecked And _chkAgent_0.CheckState = CheckState.Unchecked Then _
                m_lPartyCnt = 0



            m_sUniqueId = GetUniqueID()

            m_sScreenHierarchy = $"User({m_sUsername})"

            If m_bNewUser Then

                m_lReturn = g_oBusiness.Add(iUserID:=m_iUser, iLanguageID:=1, sUsername:=m_sUsername,
                                            sPassword:=m_sEncrypt, dtPasswordChangeDate:=m_dtPasswordchange,
                                            lPartyCnt:=m_lPartyCnt, dtDateCreated:=DateTime.Now,
                                            dtLastLogin:=DateTime.Now, iIsDeleted:=0,
                                            dtEffectiveDate:=m_dtUserEffectiveDate, vIsPMBLinkRequired:=0,
                                            vServerPrinter:=m_sDefPrinter,
                                            vIsPrinterChangeable:=m_iIsPrinterChangable,
                                            vEmailAddress:=m_sEmailAddress, vFullName:=m_sFullname,
                                            vSignatureFile:=m_sSignatureFile, vTitle:=m_sTitle,
                                            vTelephoneNumber:=m_sTelephoneNumber,
                                            vExtensionNumber:=m_sExtensionNumber, vFaxNumber:=m_sFaxNumber,
                                            vJobTitleId:=m_lJobTitleId, vClaimHandlerId:=m_lClaimHandlerId,
                                            vPartyHandlerId:=m_lPartyHandlerId, vOtherPartyId:=m_lOtherPartyId,
                                            vInitials:=m_sInitials, vMobileNumber:=m_sMobileNumber,
                                            vJobBasis:=m_lJobBasis, vSiriusUser:=m_bSiriusUser,
                                            vPercentHoursWorked:=m_dPercentHoursWorked, vDateDeleted:=m_vDateDeleted,
                                            vIsTempPassword:=m_bIsTempPassword, sOldPassword:=m_sOldPassword,
                                            sPasswordChanged:=m_sPassword, sUniqueId:=m_sUniqueId,
                                            sScreenHierarchy:=m_sScreenHierarchy, vSSOPreferredName:=m_sSSOPreferredName)

                m_bIsClearCache = False
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return nResult
                End If
            Else

                nRow = 1

                g_oBusiness.CurrentRecord = nRow - 1
                If m_bIsUserTempPassword Then
                    m_bIsTempPassword = m_bIsUserTempPassword
                End If
                Dim sPassword As String = m_sPassword
                If (bPasswordChanged = False) Then
                    sPassword = ""
                End If

                m_lReturn = g_oBusiness.EditUpdate(lRow:=nRow, vLanguageID:=1, vUsername:=m_sUsername,
                                                   vPassword:=m_sEncrypt, vPasswordChangeDate:=m_dtPasswordchange,
                                                   vPartyCnt:=m_lPartyCnt, vDateCreated:=m_dtDateCreated,
                                                   vIsDeleted:=m_iIsDeleted, vEffectiveDate:=m_dtUserEffectiveDate,
                                                   vServerPrinter:=m_sDefPrinter,
                                                   vIsPrinterChangeable:=m_iIsPrinterChangable,
                                                   vEmailAddress:=m_sEmailAddress, vFullName:=m_sFullname,
                                                   vSignatureFile:=m_sSignatureFile, vTitle:=m_sTitle,
                                                   vTelephoneNumber:=m_sTelephoneNumber,
                                                   vExtensionNumber:=m_sExtensionNumber, vFaxNumber:=m_sFaxNumber,
                                                   vJobTitleId:=m_lJobTitleId, vClaimHandlerId:=m_lClaimHandlerId,
                                                   vPartyHandlerId:=m_lPartyHandlerId,
                                                   vOtherPartyId:=m_lOtherPartyId, vInitials:=m_sInitials,
                                                   vMobileNumber:=m_sMobileNumber, vJobBasis:=m_lJobBasis,
                                                   vSiriusUser:=m_bSiriusUser,
                                                   vPercentHoursWorked:=m_dPercentHoursWorked,
                                                   vDateDeleted:=m_vDateDeleted, vIsTempPassword:=m_bIsTempPassword,
                                                   sOldPassword:=m_sOldPassword, sPasswordChanged:=sPassword,
                                                   sUniqueId:=m_sUniqueId, sScreenHierarchy:=m_sScreenHierarchy,
                                                   vSSOPreferredName:=m_sSSOPreferredName)
                m_bIsTempPassword = False
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Cannot Update User Details", "User Details")
                    Return nResult

                End If

                m_lReturn = g_oBusiness.Update()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("User Details Not Updated", "User Details")
                End If

                If m_bIsClearCache And m_bIsTempPassword Then
                    ClearSAMCache()
                    m_bIsClearCache = False
                End If

            End If

            UpdateUserAuthorities()


            'Validating Administrator Rights Removal
            m_lReturn = ValidateAdminRights()
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                UpdateUserGroups()
            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                MessageBox.Show("This user cannot be removed from System Administrative Group" &
                                Strings.Chr(13).ToString() & "There should be at least one user as system administrator" &
                                Strings.Chr(13).ToString() & "Unable to UpdateUserGroups.", "User Maintenance",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return nResult
            Else
                Return nResult
            End If

            UpdateUserBranches()


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception


            nResult = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateUserDetails Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUserDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    Private Function UpdateDomainAccountMappings() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = ValidateUserMapping(m_vSystemSecurityModel)

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                'No administrator found
                MessageBox.Show("At least one user with System Administrative Rights should be mapped " &
                                "with valid Domain Account User." & Strings.Chr(13).ToString() & "Unable to update System Security Model", "User Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lReturn = RefreshListOnSecurityTab()
                Return result
            End If

            m_lReturn = UpdateUserMapping()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateUserMapping Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUserMapping", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateDomainAccountMappings Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDomainAccountMappings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Function SwitchSystemSecurityModel(ByVal vSystemSecurityModel As Integer, ByVal vUserName As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = ValidateUserPassword(vSystemSecurityModel, vUserName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lReturn = ValidateProductOption(vSystemSecurityModel)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Unable to switch System Security Model to '" &
                                cboSystemSecurity.Text & "' mode." & Strings.Chr(13).ToString() &
                                "A System Administrator with a valid Domain Account needs to be matched.", "User Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            End If

            m_lReturn = SetProductOption(r_vOption:=gPMConstants.SIRHiddenOptions.SIROPTAlternativeLogon, r_vBranch:=1, r_vValue:=vSystemSecurityModel)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProductOption Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProductOption", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            If lvwSiriusUsers.Items.Count > 0 And cboSystemSecurity.SelectedIndex = 2 Then
                MessageBox.Show("There are " & lvwSiriusUsers.Items.Count & " Sirius users " &
                                "found which are not mapped with a valid domain user account. So, they will " &
                                "not be able to logon in 'Unified (Windows) Login Only' mode.", "User Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If



            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SwitchSystemSecurityModel Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="SwitchSystemSecurityModel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Function CanUnMatch(ByVal iUser As Integer) As Integer
        Dim result As Integer = 0
        Dim oPMUser As Bpmuser.Business
        Dim sPassword As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            'only in case of SecurityModel 1 and 2
            If m_vSystemSecurityModel = 1 Or m_vSystemSecurityModel = 2 Then
                ' Get an instance of the business object via
                ' the public object manager.
                Dim temp_oPMUser As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oPMUser, "bPMUser.Business", vInstanceManager:="ClientManager")
                oPMUser = temp_oPMUser
                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get an instance of the business object.
                    Return result
                End If


                m_lReturn = oPMUser.GetDetails(vUserId:=iUser)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If


                m_lReturn = oPMUser.GetNext(vPassword:=sPassword)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If


                If m_iUser = iUser Then
                    If sPassword.Trim().Length = 0 Or m_sPassword.Trim().Length = 0 Then
                        'Don't allow to unmatch if the password is blank
                        MessageBox.Show("User password is blank. Please update the user password first " &
                                        "if you wish to unmatch this mapping.", "Unable to unmatch", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return result
                    End If
                Else
                    If sPassword.Trim().Length = 0 Then
                        'Don't allow to unmatch if the password is blank
                        MessageBox.Show("User password is blank. Please update the user password first " &
                                        "if you wish to unmatch this mapping.", "Unable to unmatch", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return result
                    End If
                End If

                oPMUser = Nothing

            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CanUnMatch Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="CanUnMatch", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: Save
    '
    ' Description:
    '
    ' History: 11/02/2005 RKS - Created.
    '
    ' ***************************************************************** '
    Public Function Save() As Integer
        Dim result As Integer = 0
        Dim sNewUser As String = ""
        'Temp var for Parallel PM22900
        Dim bNewUser As Boolean
        Dim sUser As String = String.Empty

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Update Individual User Details
            sNewUser = m_sUsername

            'Parallel PM22900
            bNewUser = m_bNewUser
            If m_sUnderwritingOrAgency = "U" Then
                ' If Not chkInvoice.CheckState And Not chkInstalments.CheckState And Not chkPayNow.CheckState And Not chkHasPaynowWriteOffAuthority.CheckState And fraMakeLive.Visible And Not chkBankGuarantee.CheckState And Not chkCashDeposit.CheckState Then
                If Not chkInvoice.Checked And Not chkInstalments.Checked And Not chkPayNow.Checked And Not chkHasPaynowWriteOffAuthority.Checked And fraMakeLive.Visible And Not chkBankGuarantee.Checked And Not chkCashDeposit.Checked Then
                    MessageBox.Show("You must choose at least one of the Make Live options", "Make Live Option", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return result
                End If
            End If
            m_lReturn = UpdateUserDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            For Each oListitem As ListViewItem In lvwMatchedUsers.Items
                sUser = oListitem.Text

                'Switch System Security Model
                If m_vSystemSecurityModel <> cboSystemSecurity.SelectedIndex Then
                    m_lReturn = SwitchSystemSecurityModel(cboSystemSecurity.SelectedIndex, sUser)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        cboSystemSecurity.SelectedIndex = m_vSystemSecurityModel
                        Return result
                    End If
                    m_vSystemSecurityModel = cboSystemSecurity.SelectedIndex
                End If
            Next

            m_lReturn = SaveSystemSecurityModelValue()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'Update Mappings with Domain Account User
            m_lReturn = UpdateDomainAccountMappings()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lReturn = PopListBox()

            ClearUserDetails()
            m_bNewUser = False

            For i As Integer = 1 To lvwUsers.Items.Count
                If lvwUsers.Items.Item(i - 1).Text.Trim() = sNewUser.Trim() Then
                    lvwUsers.FocusedItem = lvwUsers.Items.Item(i - 1)

                    lvwUsers.FocusedItem.EnsureVisible()

                    If Not Information.IsNothing(lvwUsers.FocusedItem) Then
                        lvwUsers_ItemClick(lvwUsers.FocusedItem)
                        Exit For
                    Else
                        lvwUsers_ItemClick(lvwUsers.Items(0))
                    End If
                End If
            Next i

            m_lReturn = RefreshListOnSecurityTab()

            If Not Information.IsNothing(lvwUsers.FocusedItem) Then
                lvwUsers_ItemClick(lvwUsers.FocusedItem)
            Else
                lvwUsers_ItemClick(lvwUsers.Items(0))
            End If

            'PN 18584
            If m_vSystemSecurityModel = 2 And m_vAlternativeIdentifier.Length = 0 Then
                If MessageBox.Show("You need to match the user to their domain account. Do you want to match now? ", "Match User", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
                    SSTabHelper.SetSelectedIndex(tabMain, 1) ''to show the form
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            m_lReturn = SaveSignatureFileInXMLFormat()
            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Save Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Save", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function FillList(ByVal vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim oListitem As ListViewItem
        Dim sInclude As Boolean
        Dim sfilter As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lDeletedUsers = 0
            m_lFilteredUsers = 0


            'Clear the items in the listview box
            lvwUsers.Items.Clear()

            sfilter = txtFilter.Text.Trim().ToUpper() 'Set row count

            lRow = -1
            ReDim m_ArrDeletedUsers(0)

            For lRow = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)


                m_iUser = CInt(vResultArray(0, lRow))

                m_sUsername = CStr(vResultArray(2, lRow))


                m_iIsDeleted = CInt(vResultArray(12, lRow))
                'DJM 20/02/2004 PN10542 : Add the for each user as this is meant to link to the database.


                sInclude = False

                If (m_iIsDeleted = gPMConstants.PMEReturnCode.PMTrue) And (chkHideDeleted.CheckState = CheckState.Unchecked) Then

                    m_lDeletedUsers += 1
                    ReDim Preserve m_ArrDeletedUsers(m_ArrDeletedUsers.GetUpperBound(0) + 1)

                    m_ArrDeletedUsers(m_ArrDeletedUsers.GetUpperBound(0)) = m_sUsername
                Else
                    If sfilter.ToUpper() = s_defFilterText.ToUpper() Then
                        sInclude = True
                    ElseIf Len(sfilter) > 1 And Strings.Left(UCase(Trim$(m_sUsername)), Len(sfilter)) = sfilter Then
                        sInclude = True
                        'DJM 20/02/2004 PN10542 : Allow filter for first letter.
                    ElseIf sfilter.Length = 1 And m_sUsername.Trim().ToUpper().Substring(0, 1) = sfilter Then
                        sInclude = True
                    End If

                    If sInclude Then


                        oListitem = lvwUsers.Items.Add("L" & m_iUser, m_sUsername.Trim(), "")
                        oListitem.ImageKey = "user"

                        If m_iIsDeleted = gPMConstants.PMEReturnCode.PMTrue Then


                            oListitem.ForeColor = Color.Gray
                        End If

                    Else

                        m_lFilteredUsers += 1

                    End If

                End If

            Next

            Return result

        Catch excep As System.Exception



            'Error Section

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Fill user details ", vApp:=ACApp, vClass:=ACClass, vMethod:="FillList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Function SelectParty(ByRef vPartyCnt As Integer, ByRef vShortName As String, Optional ByRef vName As Object = Nothing, Optional ByRef vSpecialParty As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing, Optional ByRef bSuppressSubAgents As Boolean = False) As Integer 'CT 19/07/00 added vResolvedName parameter


        Dim result As Integer = 0
        Dim oFindParty As iPMBFindParty.Interface_Renamed
        Dim vKeyArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oFindParty = New iPMBFindParty.Interface_Renamed()

            '    oFindParty.BranchID = 1 'Agent Filtering


            m_lErrorNumber = oFindParty.Initialise()

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
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

                ReDim vKeyArray(1, 0)

                vKeyArray(0, 0) = "special_party"

                vKeyArray(1, 0) = vSpecialParty


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


                If Information.IsNothing(vName) Then
                    vName = oFindParty.LongName
                End If
                'TN20000823 - fix CT

                If Information.IsNothing(vResolvedName) Then
                    vResolvedName = oFindParty.ResolvedName 'CT 19/07/00
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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function PopulateJobBasisCombo() As Integer

        Dim result As Integer = 0
        Const AC_METHOD As String = "PopulateJobBasisCombo"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            With Me.cboJobBasis
                .Items.Clear()

                Dim listIndex As Integer = .Items.Add(New VB6.ListBoxItem("Full Time", JOB_BASIS_FULLTIME))
                listIndex = .Items.Add(New VB6.ListBoxItem("Part Time", JOB_BASIS_PARTTIME))
            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Call to method " & AC_METHOD & " failed", vApp:=ACApp, vClass:=ACClass, vMethod:=AC_METHOD, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    Private Function CheckIsRecommendClaimPaymentEnabledatProduct() As Integer

        Dim result As Integer = 0
        Dim vResults As Object = Nothing
        Const kMethodName As String = "CheckIsRecommendClaimPaymentEnabledatProduct"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = g_oAuthority.CheckIsRecommendClaimPaymentEnabledatProduct(r_vResults:=vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CheckIsRecommendClaimPaymentEnabledatProduct Failed")
            End If

            If Information.IsArray(vResults) Then

                m_bCheckIsRecommendClaimPaymentEnabledatProduct = gPMFunctions.ToSafeInteger(CStr(vResults(0, 0))) >= 1
            End If



        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: EnableAuthorityReversePayment
    '
    ' Description: Enables or disables Authority Reverse Payment
    '
    ' ***************************************************************** '
    Private Function EnableAuthorityReversePayment() As Integer

        Dim result As Integer = 0

        Dim bIsChecked As Boolean
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bIsChecked = chkReverseAllocation.CheckState
            'If its checked so enable controls

            lblTimePeriod.Enabled = bIsChecked
            txtTimePeriod.Enabled = bIsChecked



        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnableAuthorityReversePayment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnableAuthorityReversePayment", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

            Return result

        Finally


        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '
    ' Name: CheckTimePeriod
    '
    ' Description: Check Time Period
    '
    ' ***************************************************************** '

    Private Function CheckTimePeriod() As Integer

        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            If chkReverseAllocation.CheckState = CheckState.Checked Then
                If gPMFunctions.ToSafeInteger(txtTimePeriod.Text) <= 0 Or gPMFunctions.ToSafeInteger(txtTimePeriod.Text) > 999 Then
                    MessageBox.Show("Reverse Allocation Time Period data is not an acceptable entry Please enter between 1 and 999 days", "Reverse Allocation", MessageBoxButtons.OK, MessageBoxIcon.Error)

                    Return gPMConstants.PMEReturnCode.PMError
                End If
            End If


        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckTimePeriod Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckTimePeriod", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

            Return result

        Finally


        End Try
        Return result
    End Function


    Private Sub frmUserMaintenance_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown, tabMain.KeyDown

        If e.Alt And e.KeyCode = Keys.U Then
            tabMain.SelectedIndex = 0
        End If

        If e.Alt And e.KeyCode = Keys.S Then
            tabMain.SelectedIndex = 1
        End If

        If tabMain.SelectedIndex = 0 Then
            If e.Alt And e.KeyCode = Keys.D1 Then
                SSTab1.SelectedIndex = 0
            End If
            If e.Alt And e.KeyCode = Keys.D2 Then
                SSTab1.SelectedIndex = 1
            End If
            If e.Alt And e.KeyCode = Keys.D3 Then
                SSTab1.SelectedIndex = 2
            End If
            If e.Alt And e.KeyCode = Keys.D4 Then
                SSTab1.SelectedIndex = 3
            End If
            If e.Alt And e.KeyCode = Keys.D5 Then
                SSTab1.SelectedIndex = 4
            End If
            If e.Alt And e.KeyCode = Keys.D6 Then
                SSTab1.SelectedIndex = 5
            End If
            If e.Alt And e.KeyCode = Keys.D7 Then
                SSTab1.SelectedIndex = 6
            End If
            If e.Alt And e.KeyCode = Keys.D8 Then
                SSTab1.SelectedIndex = 7
            End If
        End If
        If tabMain.SelectedIndex = 0 And SSTab1.SelectedIndex = 4 Then
            If e.Alt And e.KeyCode = Keys.A Then
                SSTab2.SelectedIndex = 0
            End If

            If e.Alt And e.KeyCode = Keys.P Then
                SSTab2.SelectedIndex = 1
            End If
            If e.Alt And e.KeyCode = Keys.L Then
                SSTab2.SelectedIndex = 2
            End If
        End If
    End Sub


    Private Sub lvwUsers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwUsers.SelectedIndexChanged
        Dim oListitem As ListViewItem

        cmdApply.Enabled = True
        cmdOK.Enabled = True
        SSTab1.Enabled = True
        tabMain.TabPages(1).Enabled = True
        m_bNewUser = False
        m_sOldPassword = ""


        If lvwUsers.SelectedItems.Count > 0 Then
            If lvwUsers.SelectedItems.Item(0).ForeColor = Color.Gray Then
                cmdDeleteUser.Text = "Un&delete"
                SSTab1.Enabled = False
                tabMain.TabPages(1).Enabled = False
            Else
                cmdDeleteUser.Text = "&Delete"
                SSTab1.Enabled = True
                tabMain.TabPages(1).Enabled = True
            End If
        End If


        If Not Information.IsNothing(lvwUsers.FocusedItem) Then
            ' Edit details of user if doubleclicked
            If lvwUsers.FocusedItem.Index >= 0 Then
                oListitem = lvwUsers.FocusedItem
            Else
                oListitem = lvwUsers.GetItemAt(m_lXPos, m_lYPos)
            End If
        Else
            lvwUsers.FocusedItem = lvwUsers.Items(0)
            oListitem = lvwUsers.Items(0)
        End If
        m_lReturn = EditUser(oListitem)

    End Sub

    Private Sub lvwRiskGroups_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwRiskGroups.SelectedIndexChanged
        cmdEditRiskDetails.Enabled = Not (lvwRiskGroups.SelectedItems.Item(0) Is Nothing)
    End Sub

    Private Function SaveSignatureFileInXMLFormat() As Integer

        Dim oWord As Object
        Dim sXMLFile As String
        Dim sSignatureFile As String
        Dim sTmpFile As String
        Dim oFSO As Object
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Len(Trim$(m_sSignatureFile)) = 0 Then
                Return result
            End If

            oWord = CreateObject("Word.Application")

            oFSO = CreateObject("Scripting.FileSystemObject")

            sSignatureFile = m_sSignatureDir & m_sSignatureFile

            sTmpFile = m_sSignatureDir & "TmpSign" & ".bmp"

            FileCopy(sSignatureFile, sTmpFile)

            sXMLFile = m_sSignatureDir & m_sSignatureFile & ".xml"

            oWord.Documents.Add(Template:="Normal", NewTemplate:=False, DocumentType:=0)

            oWord.Selection.InlineShapes.AddPicture(FileName:=sTmpFile, LinkToFile:=False, SaveWithDocument:=True)

            oWord.ActiveDocument.SaveAs(FileName:=sXMLFile, FileFormat:=11)

            oWord.Visible = False

            oWord.ActiveDocument.Close()

            oWord.Visible = False

            oWord.Quit()

            oFSO.DeleteFile(sTmpFile, True)

            oWord = Nothing
            Return result

        Catch excep As System.Exception

            'Error Section

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveSignatureFileInXMLFormat", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function GetApiTokendetails() As TokenModel
        Dim apiTokenDetails As TokenModel = New TokenModel()
        Dim clientId As String = Global.iPMUserMaintenance.My.MySettings.Default.ClientId
        Dim tokenUrl As String = Global.iPMUserMaintenance.My.MySettings.Default.TokenUrl
        apiTokenDetails = GenerateToken.GetJwtTokenForBatchProcess(clientId, tokenUrl)
        Dim address As String = Global.iPMUserMaintenance.My.MySettings.Default.ApiEndpoint
        If address.EndsWith("/") Then
            address = address.Substring(0, address.Length - 1)
        End If
        apiTokenDetails.ApiBaseUrl = address
        apiTokenDetails.TokenUrl = tokenUrl
        Return apiTokenDetails
    End Function
    Private Function ClearSAMCache() As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMFalse
        Dim oWcfRequest As ClearCacheCommand = Nothing
        Dim oWcfResponse As ClearCacheCommandResponse = Nothing

        Try

            Try
                oWcfRequest = New ClearCacheCommand
                oWcfResponse = New ClearCacheCommandResponse
                nResult = gPMConstants.PMEReturnCode.PMTrue

                oWcfRequest.LoginUserName = Global.iPMUserMaintenance.My.MySettings.Default.SAMUserName
                oWcfRequest.BranchCode = Global.iPMUserMaintenance.My.MySettings.Default.SamBranchCode
                ApiClient._tokenModel = GetApiTokendetails()
                oWcfResponse = ApiClient.DeserializeJson(Of ClearCacheCommandResponse)(CStr(ApiClient.Post($"/core/clearCache", oWcfRequest)))
            Catch ex As EndpointNotFoundException
                MessageBox.Show(ex.Message)
            Catch ex As System.Net.WebException
                MessageBox.Show("Could not connect to the Pure Api Service.Please contact system administrator ", "User Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try

            Return nResult

        Catch exCep As System.Exception
            'Error Section
            nResult = gPMConstants.PMEReturnCode.PMError
            MessageBox.Show("Could not connect to the Pure Api Service using the clientID/TokenUrl provided in iPMUserMaintenance.config ", "User Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
            'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ResetSAM", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialForm", vErrNo:=Information.Err().Number, vErrDesc:=exCep.Message)
            Return nResult
        Finally
            oWcfRequest = Nothing
            oWcfResponse = Nothing
        End Try

        Return nResult

    End Function

    ''' <summary>
    ''' SetEditCommissionCheckBoxValues
    ''' </summary>
    ''' <param name="v_bEditCommissionClicked"></param>
    ''' <param name="v_bEditDefaultCommissionNBRNClicked"></param>
    ''' <param name="v_bEditDefaultCommissionMTAClicked"></param>
    ''' <param name="v_bEditDefaultCommissionMTCClicked"></param>
    ''' <param name="v_bEditDefaultCommissionMTRClicked"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetEditCommissionCheckBoxValues(ByVal v_bEditCommissionClicked As Boolean,
                                                     ByVal v_bEditDefaultCommissionNBRNClicked As Boolean,
                                                     ByVal v_bEditDefaultCommissionMTAClicked As Boolean,
                                                     ByVal v_bEditDefaultCommissionMTCClicked As Boolean,
                                                     ByVal v_bEditDefaultCommissionMTRClicked As Boolean) As Long
        Dim nResult As Integer = 0

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            If v_bEditDefaultCommissionNBRNClicked Then
                If m_nEnableDefaultCommissionNBRN = 1 Then
                    chkEditDefaultCommission.CheckState = 1
                    chkEditDefaultCommissionNBRN.CheckState = 1
                Else
                    chkEditDefaultCommissionNBRN.CheckState = 0
                End If
            End If
            If v_bEditDefaultCommissionMTAClicked Then
                If m_nEnableDefaultCommissionMTA = 1 Then
                    chkEditDefaultCommission.CheckState = 1
                    chkEditDefaultCommissionMTA.CheckState = 1
                Else
                    chkEditDefaultCommissionMTA.CheckState = 0
                End If
            End If
            If v_bEditDefaultCommissionMTCClicked Then
                If m_nEnableDefaultCommissionMTC = 1 Then
                    chkEditDefaultCommission.CheckState = 1
                    chkEditDefaultCommissionMTC.CheckState = 1
                Else
                    chkEditDefaultCommissionMTC.CheckState = 0
                End If
            End If
            If v_bEditDefaultCommissionMTRClicked Then
                If m_nEnableDefaultCommissionMTR = 1 Then
                    chkEditDefaultCommission.CheckState = 1
                    chkEditDefaultCommissionMTR.CheckState = 1
                Else
                    chkEditDefaultCommissionMTR.CheckState = 0
                End If
            End If
            If chkEditDefaultCommissionNBRN.CheckState = 0 And chkEditDefaultCommissionMTA.CheckState = 0 And
                chkEditDefaultCommissionMTC.CheckState = 0 And chkEditDefaultCommissionMTR.CheckState = 0 And
                chkEditDefaultCommission.CheckState = 1 Then
                chkEditDefaultCommission.CheckState = 0
                chkEditDefaultCommissionNBRN.Enabled = False
                chkEditDefaultCommissionMTA.Enabled = False
                chkEditDefaultCommissionMTC.Enabled = False
                chkEditDefaultCommissionMTR.Enabled = False
            End If
            Return nResult
        Catch
            SetEditCommissionCheckBoxValues = gPMConstants.PMEReturnCode.PMFalse
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError,
                            sMsg:="Failed to SetEditCommissionCheckBoxValues", vApp:=ACApp, vClass:=ACClass,
                            vMethod:="SetEditCommissionCheckBoxValues")
        Finally
        End Try
        Return nResult
    End Function


    ''' <summary>
    ''' Ctx_mnuSupervisor_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Ctx_mnuSupervisor_Click(sender As Object, e As EventArgs) Handles Ctx_mnuSupervisor.Click
        m_lReturn = ToggleSupervisor(m_oListItem)
    End Sub

    ''' <summary>
    ''' ToggleSupervisor
    ''' </summary>
    ''' <param name="oListItem"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ToggleSupervisor(ByRef oListItem As ListViewItem) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue

        Try

            mnuSuper.Checked = Not mnuSuper.Checked
            If oListItem Is Nothing Then
                Return nResult
            End If

            If mnuSuper.Checked Then
                lvwSelectedGroups.Items(oListItem.Index).ImageKey = "supervisor"
                oListItem.ImageKey = "supervisor"
            Else
                lvwSelectedGroups.Items(oListItem.Index).ImageKey = ""
                oListItem.ImageKey = "user"
            End If
            Return nResult

        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                               sMsg:="Failed to Toggle Supervisor", vApp:=ACApp, vClass:=ACClass,
                               vMethod:="ToggleSupervisor", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try

    End Function

    Private Sub cboClaimPaymentsCurrency_Load(sender As Object, e As EventArgs) Handles cboClaimPaymentsCurrency.Load

    End Sub
    Private Sub chkViewbatchProcessStatus_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkViewBatchProcessStatus.CheckStateChanged
        If chkViewBatchProcessStatus.CheckState = CheckState.Checked Then
            lblViewBatchProcessStatus.Text = "Yes"
        Else
            lblViewBatchProcessStatus.Text = "No"
        End If
    End Sub

    Private Sub chkCanExtractClientData_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkCanExtractClientData.CheckStateChanged
        If chkCanExtractClientData.CheckState = CheckState.Checked Then
            lblCanExtractClientData.Text = "Yes"
        Else
            lblCanExtractClientData.Text = "No"
        End If
    End Sub
    ' ***************************************************************** '
    '
    ' Name: SaveSystemSecurityModelValue
    '
    ' Description: Save Hidden field value on the basis of System Security Model
    '
    ' ***************************************************************** '
    Private Function SaveSystemSecurityModelValue() As Integer

        Dim result As Integer = 0
        Dim m_oBusiness As Object
        Dim vResultArray(3, 0) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMFalse
            Dim temp_m_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRProductOptions.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness
            vResultArray(0, 0) = gPMConstants.SIRHiddenOptions.SIROPTAlternativeLogon
            vResultArray(1, 0) = 1 'Branch
            vResultArray(2, 0) = cboSystemSecurity.SelectedIndex
            vResultArray(3, 0) = ""
            m_lReturn = m_oBusiness.updateMasterOptions(vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Save System Security Model Value Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveSystemSecurityModelValue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try

    End Function

    Private Sub chkEditInstalment_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkEditInstalment.CheckStateChanged
        EnableInstalmentNoOfDays()
    End Sub

    Private Function EnableInstalmentNoOfDays() As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            Dim bIsChecked As Boolean = chkEditInstalment.CheckState
            txtEditInstalmentByNoofDays.Enabled = bIsChecked
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnableInstalmentNoOfDays Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnableInstalmentNoOfDays", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Private Function CheckInstalmentNoOfDays() As Integer

        Dim result As Integer = 0

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            If chkEditInstalment.CheckState = CheckState.Checked Then
                If gPMFunctions.ToSafeInteger(txtEditInstalmentByNoofDays.Text) <= 0 Or gPMFunctions.ToSafeInteger(txtEditInstalmentByNoofDays.Text) > 31 Then
                    MessageBox.Show("Number of days entered is not valid, please enter a number between 1-31", "Number of days", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return gPMConstants.PMEReturnCode.PMError
                End If
            End If
            Return result
        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckInstalmentNoOfDays Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckInstalmentNoOfDays", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End Try

    End Function

    Private Sub txtCurrencyAmount_Leave(sender As Object, e As EventArgs) Handles txtCurrencyLossGainLimit.Leave, txtCurrencyAmount.Leave
        Dim dbNumericTemp As Double
        If (Not Double.TryParse(txtCurrencyLossGainLimit.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) And txtCurrencyLossGainLimit.Text.Trim() <> "" Then
            MessageBox.Show("Please enter a valid amount", "Invalid amount entered - Currency Amount", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtCurrencyLossGainLimit.Focus()
            Exit Sub
        End If
    End Sub

    Private Sub ChkManualJournal_CheckStateChanged(sender As Object, e As EventArgs) Handles ChkManualJournal.CheckStateChanged
        EnableManualJournalPayments()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles txtSSOPreferredName.TextChanged

    End Sub
End Class
