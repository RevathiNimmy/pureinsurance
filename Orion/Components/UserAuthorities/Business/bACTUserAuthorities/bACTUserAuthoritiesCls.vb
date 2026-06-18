Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
'developer guide no 129. 
Imports SSP.Shared

Friend NotInheritable Class ACTUserAuthorities
    Implements IDisposable
    ' History:
    ' CJB090805 - PN 23035 - New Client Manager Security option for 'Can Delete Policy'.
    '             Changed GetDefaults, DefaultParameters, SetProperties, Validate, GetProperties,
    '             AddInputParam & SetPropertiesFromDB
    '
    ' ***************************************************************** '
    ' Class Name: ACTUserAuthorities
    '
    ' Date: 14/02/2000
    '
    ' Description: Describes the ACTUserAuthorities attributes.
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "ACTUserAuthorities"

    ' ************************************************
    ' Added to replace global variables 24/09/2004
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    'User Authority Variables
    Private m_iUAUserID As Integer
    Private m_iUnrestrictedEnquiry As Integer
    Private m_iUnrestrictedUpdate As Integer
    Private m_nViewbatchProcessStatus As Integer
    Private m_nCanExtractClientData As Integer
    Private m_iCanPerformBrokerTransfer As Integer
    Private m_iOverrideDate As Integer
    Private m_iOverrideRate As Integer
    Private m_iOverridePrePolicyDate As Integer
    Private m_iOverridePrePolicyRate As Integer
    Private m_iDuplicateClaimOverride As Integer

    Private m_iHasRefundAuthority As Integer
    Private m_iHasTransferAuthority As Integer

    Private m_iHasPaymentsAuthority As Integer
    Private m_iPaymentsCurrencyID As Integer
    Private m_dPaymentsAmount As Double

    Private m_iHasWriteOffAuthority As Integer
    Private m_iWriteOffCurrencyID As Integer
    Private m_dWriteOffAmount As Double
    Private m_iLossGainCurrencyID As Integer
    Private m_dCurrencyLossGainLimit As Object
    Private m_iHasTransWriteOffAuthority As Integer
    Private m_iTransWriteOffCurrencyID As Integer
    Private m_dTransWriteOffAmount As Double

    Private m_iHasClaimPaymentsAuthority As Integer
    Private m_iClaimPaymentsCurrencyID As Integer
    Private m_dClaimPaymentsAmount As Double


    Private m_iHasManualJournalAuthority As Integer
    Private m_iManualJournalCurrencyID As Integer
    Private m_dManualJournalAmount As Double

    Private m_lFeeDiscount As Integer

    Private m_iDisplayReinsuranceScreen As Integer
    Private m_iDisplayClaimReinsurance As Integer

    Private m_iEditDefaultCommission As Integer
    Private m_iCanMakeLiveCashDeposit As Integer
    Private m_iCanChangeInstalmentDefaultCurrency As Integer


    Private m_iACUserCanDebugDynamicLogicScripts As Integer
    Private m_iACUserServerScriptsRunInDebug As Integer
    Private m_nEditDefaultCommissionNBRN As Integer
    Private m_nEditDefaultCommissionMTA As Integer
    Private m_nEditDefaultCommissionMTC As Integer
    Private m_nEditDefaultCommissionMTR As Integer
    Private m_nEditAgentDuringMTAMTC As Integer
    Private m_iIsViewClient As Integer
    Private m_iIsEditClient As Integer
    Private m_iIsDeleteClient As Integer
    Private m_iIsEditPolicy As Integer
    Private m_iIsDeletePolicy As Integer
    Private m_iIsEditClaim As Integer
    Private m_iIsEditFinancePlan As Integer
    Private m_iIsRaiseDebit As Integer
    Private m_iIsRaiseCredit As Integer
    Private m_iIsRaiseFee As Integer
    Private m_iIsRaiseCash As Integer
    Private m_iIsReverseTransactions As Integer
    Private m_iIsPerformAllocations As Integer
    Private m_iIsReverseAllocations As Integer
    Private m_iIsRaiseManualDID As Integer
    Private m_iIsEditSchemePolicy As Integer
    'Float Balance and Pre-Payment
    Private m_iCanMakeLiveInvoice As Integer
    Private m_iCanMakeLiveInstalments As Integer
    Private m_iCanMakeLivePaynow As Integer
    Private m_iHasPaynowWriteoffAuthority As Integer
    Private m_iPaynowWriteoffCurrencyID As Integer
    Private m_cPaynowWriteOffAmount As Decimal
    Private m_iPaynowBankAccount As Integer

    'For Reverse and Replace Transaction Option
    Private m_iCanReverseReplaceTransactions As Integer

    'Party View
    Private m_iViewClientManager As Integer
    Private m_iAgentMaintenance As Integer
    Private m_iAccountHandler As Integer
    Private m_iAccountExecutive As Integer
    Private m_iInsurerMaintenance As Integer
    Private m_iOtherPartyMaintenance As Integer

    Private m_iIsRecommender As Integer
    Private m_iRecommenderCurrencyID As Integer
    Private m_cRecommenderCurrencyAmt As Decimal

    Private m_iPostingPeriod As Integer '(RC) IH-UDPP
    Private m_iUserCanChangeReserves As Integer '(RC) QBENZ001

    Private m_iUserCanAddRemoveRatingSections As Integer
    Private m_iUserCanEditExistingRatingSections As Integer

    'Rahul UIIC WR01 – Enhanced Pre Payment (64VB)
    Private m_iCanBackdateCollectionDate As Integer

    'Gaurav UIIC WR06 – Bank Guarantee
    Private m_iCanMakeLiveBankGuarantee As Integer
    ' Database Class
    Private m_oDatabase As dPMDAO.Database
    ' Close Database Flag
    Private m_bCloseDatabase As Boolean

    ' Update Status
    Private m_iDatabaseStatus As gPMConstants.PMEComponentAction

    ' Error Code
    Private m_lReturn As gPMConstants.PMEReturnCode

    'Payment Maintenance
    Private m_iCanReverseAllocations As Integer
    Private m_iTimePeriodForReversal As Integer
    Private m_iMTAAuthority As Integer
    Private m_iOverrideChequeNumber As Integer

    Private m_nAllowReceiptReversal As Integer

    Private m_nInstalmentStatus As Integer
    Private m_nCanEditInstalmentDate As Integer
    Private m_nEditInstalmentDateByNoOfDays As Integer
    Private m_sUniqueId As String
    Private m_sScreenHierarchy As String
    Private m_iModifiedByUserId As Integer
	Private m_VoidTransaction As String

    ' PUBLIC Property Procedures (Begin)
    Public Property DatabaseStatus() As Integer
        Get

            Return m_iDatabaseStatus

        End Get
        Set(ByVal Value As Integer)

            m_iDatabaseStatus = Value

        End Set
    End Property

    Public Property UserID() As Integer
        Get

            Return m_iUAUserID

        End Get
        Set(ByVal Value As Integer)

            m_iUAUserID = Value

        End Set
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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            ' Set Username and Password

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the ACTUserAuthorities.
    '
    ' 19/04/2005 - 2005 Staff Control Added New Client Manager Security fields
    ' 13/10/2005 - New Option to switch of Editing of Schemes Type policies
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vHasWriteOffAuthority As Object = Nothing, Optional ByRef vWriteOffAmount As Object = Nothing, Optional ByRef vHasUnrestrictedEnquiry As Object = Nothing, Optional ByRef vHasUnrestrictedUpdate As Object = Nothing, Optional ByRef vHasTransWriteOffAuthority As Object = Nothing, Optional ByRef vTransWriteOffAmount As Object = Nothing, Optional ByRef vHasRefundAuthority As Object = Nothing, Optional ByRef vHasTransferAuthority As Object = Nothing, Optional ByRef vHasPaymentsAuthority As Object = Nothing, Optional ByRef vPaymentsAmount As Object = Nothing, Optional ByRef vHasClaimPaymentsAuthority As Object = Nothing, Optional ByRef vClaimPaymentsAmount As Object = Nothing, Optional ByRef vOverrideDate As Object = Nothing, Optional ByRef vOverrideRate As Object = Nothing, Optional ByRef vWriteOffCurrencyID As Object = Nothing, Optional ByRef vTransWriteOffCurrencyID As Object = Nothing, Optional ByRef vPaymentsCurrencyID As Object = Nothing, Optional ByRef vClaimPaymentsCurrencyID As Object = Nothing, Optional ByRef vOverridePrePolicyDate As Object = Nothing, Optional ByRef vOverridePrePolicyRate As Object = Nothing, Optional ByRef vIsViewClient As Object = Nothing, Optional ByRef vIsEditClient As Object = Nothing, Optional ByRef vIsEditPolicy As Object = Nothing, Optional ByRef vIsEditClaim As Object = Nothing, Optional ByRef vIsEditFinancePlan As Object = Nothing, Optional ByRef vIsRaiseDebit As Object = Nothing, Optional ByRef vIsRaiseCredit As Object = Nothing, Optional ByRef vIsRaiseFee As Object = Nothing, Optional ByRef vIsRaiseCash As Object = Nothing, Optional ByRef vIsReverseTransactions As Object = Nothing, Optional ByRef vIsReverseAllocations As Object = Nothing, Optional ByRef vIsRaiseManualDID As Object = Nothing, Optional ByRef vIsDeleteClient As Object = Nothing, Optional ByRef vIsPerformAllocations As Object = Nothing, Optional ByRef vCanPerformBrokerTransfer As Object = Nothing, Optional ByRef vIsDeletePolicy As Object = Nothing, Optional ByRef vIsEditSchemePolicy As Object = Nothing, Optional ByRef vCanReverseReplaceTransactions As Integer = 0, Optional ByRef vCurrencyLossGainLimit As Object = Nothing, Optional ByRef vLossGainCurrencyID As Object = Nothing, Optional ByRef vHasManualJournalAuthority As Object = Nothing, Optional ByRef vManualJournalAmount As Double = 0, Optional ByRef vManualJournalCurrencyID As Object = Nothing, Optional ByRef vVoidTransaction As String = "") As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults
            ' 19/04/2005 - 2005 Staff Control Added New Client Manager Security fields





































            'developer guide no. 98
            m_lReturn = CType(DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vUserID:=vUserID, vHasWriteOffAuthority:=vHasWriteOffAuthority, vWriteOffAmount:=vWriteOffAmount, vHasUnrestrictedEnquiry:=vHasUnrestrictedEnquiry, vHasUnrestrictedUpdate:=vHasUnrestrictedUpdate, vHasTransWriteOffAuthority:=vHasTransWriteOffAuthority, vTransWriteOffAmount:=vTransWriteOffAmount, vHasRefundAuthority:=vHasRefundAuthority, vHasTransferAuthority:=vHasTransferAuthority, vHasPaymentsAuthority:=vHasPaymentsAuthority, vPaymentsAmount:=vPaymentsAmount, vHasClaimPaymentsAuthority:=vHasClaimPaymentsAuthority, vClaimPaymentsAmount:=vClaimPaymentsAmount, vOverrideDate:=vOverrideDate, vOverrideRate:=vOverrideRate, vOverridePrePolicyDate:=vOverridePrePolicyDate, vOverridePrePolicyRate:=vOverridePrePolicyRate, vWriteOffCurrencyID:=vWriteOffCurrencyID, vTransWriteOffCurrencyID:=vTransWriteOffCurrencyID, vPaymentsCurrencyID:=vPaymentsCurrencyID, vClaimPaymentsCurrencyID:=vClaimPaymentsCurrencyID, vIsViewClient:=vIsViewClient, vIsEditClient:=vIsEditClient, vIsEditPolicy:=vIsEditPolicy, vIsEditClaim:=vIsEditClaim, vIsEditFinancePlan:=vIsEditFinancePlan, vIsRaiseDebit:=vIsRaiseDebit, vIsRaiseCredit:=vIsRaiseCredit, vIsRaiseFee:=vIsRaiseFee, vIsRaiseCash:=vIsRaiseCash, vIsReverseTransactions:=vIsReverseTransactions, vIsReverseAllocations:=vIsReverseAllocations, vIsRaiseManualDID:=vIsRaiseManualDID, vIsDeleteClient:=vIsDeleteClient, vIsPerformAllocations:=vIsPerformAllocations, vCanPerformBrokerTransfer:=vCanPerformBrokerTransfer, vIsDeletePolicy:=vIsDeletePolicy, vIsEditSchemePolicy:=vIsEditSchemePolicy, vCanReverseReplaceTransactions:=vCanReverseReplaceTransactions, vCurrencyLossGainLimit:=vCurrencyLossGainLimit, vLossGainCurrencyID:=vLossGainCurrencyID, vHasManualJournalAuthority:=vHasManualJournalAuthority,
                                                    vManualJournalAmount:=vManualJournalAmount,
                                                    vManualJournalCurrencyID:=vManualJournalCurrencyID, vVoidTransaction:=vVoidTransaction), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Sets the supplied ACTUserAuthorities property values.
    ''' </summary>
    ''' <param name="iStatus"></param>
    ''' <param name="vUserID"></param>
    ''' <param name="vHasWriteOffAuthority"></param>
    ''' <param name="vWriteOffAmount"></param>
    ''' <param name="vHasUnrestrictedEnquiry"></param>
    ''' <param name="vHasUnrestrictedUpdate"></param>
    ''' <param name="vFeeDiscount"></param>
    ''' <param name="vHasTransWriteOffAuthority"></param>
    ''' <param name="vTransWriteOffAmount"></param>
    ''' <param name="vHasRefundAuthority"></param>
    ''' <param name="vHasTransferAuthority"></param>
    ''' <param name="vHasPaymentsAuthority"></param>
    ''' <param name="vPaymentsAmount"></param>
    ''' <param name="vHasClaimPaymentsAuthority"></param>
    ''' <param name="vClaimPaymentsAmount"></param>
    ''' <param name="vOverrideDate"></param>
    ''' <param name="vOverrideRate"></param>
    ''' <param name="vOverridePrePolicyDate"></param>
    ''' <param name="vOverridePrePolicyRate"></param>
    ''' <param name="vWriteOffCurrencyID"></param>
    ''' <param name="vTransWriteOffCurrencyID"></param>
    ''' <param name="vPaymentsCurrencyID"></param>
    ''' <param name="vClaimPaymentsCurrencyID"></param>
    ''' <param name="vIsViewClient"></param>
    ''' <param name="vIsEditClient"></param>
    ''' <param name="vIsEditPolicy"></param>
    ''' <param name="vIsEditClaim"></param>
    ''' <param name="vIsEditFinancePlan"></param>
    ''' <param name="vIsRaiseDebit"></param>
    ''' <param name="vIsRaiseCredit"></param>
    ''' <param name="vIsRaiseFee"></param>
    ''' <param name="vIsRaiseCash"></param>
    ''' <param name="vIsReverseTransactions"></param>
    ''' <param name="vIsReverseAllocations"></param>
    ''' <param name="vIsRaiseManualDID"></param>
    ''' <param name="vIsDeleteClient"></param>
    ''' <param name="vIsPerformAllocations"></param>
    ''' <param name="vCanPerformBrokerTransfer"></param>
    ''' <param name="vDuplicateClaimOverride"></param>
    ''' <param name="vIsDeletePolicy"></param>
    ''' <param name="vIsEditSchemePolicy"></param>
    ''' <param name="vCanMakeLiveInvoice"></param>
    ''' <param name="vCAnMakeLiveInstalments"></param>
    ''' <param name="vCanMakeLivePayNow"></param>
    ''' <param name="vHasPaynowWriteoffAuthority"></param>
    ''' <param name="vPaynowWriteOffCurrencyID"></param>
    ''' <param name="vPayNowWriteoffAmount"></param>
    ''' <param name="vPaynowBankAccount"></param>
    ''' <param name="vPostingPeriod"></param>
    ''' <param name="vUserCanChangeReserves"></param>
    ''' <param name="vUserCanAddRemoveRatingSections"></param>
    ''' <param name="vUserCanEditExistingRatingSections"></param>
    ''' <param name="vIsViewClientManager"></param>
    ''' <param name="vIsViewAgentMaintenance"></param>
    ''' <param name="vIsViewAccountHandler"></param>
    ''' <param name="vIsViewAccountExecutive"></param>
    ''' <param name="vIsViewInsurerMaintenance"></param>
    ''' <param name="vIsViewOtherParty"></param>
    '''  <param name="vHasManualJournalAuthority"></param>
    '''  <param name="vManualJournalAmount"></param>
    '''  <param name="vManualJournalCurrencyID"></param>
    ''' <param name="vParams"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vUserID As Object = Nothing,
                                  Optional ByRef vHasWriteOffAuthority As Object = Nothing,
                                  Optional ByRef vWriteOffAmount As Double = 0,
                                  Optional ByRef vHasUnrestrictedEnquiry As Object = Nothing,
                                  Optional ByRef vHasUnrestrictedUpdate As Object = Nothing,
                                  Optional ByRef vFeeDiscount As Object = Nothing,
                                  Optional ByRef vHasTransWriteOffAuthority As Object = Nothing,
                                  Optional ByRef vTransWriteOffAmount As Double = 0,
                                  Optional ByRef vHasRefundAuthority As Object = Nothing,
                                  Optional ByRef vHasTransferAuthority As Object = Nothing,
                                  Optional ByRef vHasPaymentsAuthority As Object = Nothing,
                                  Optional ByRef vPaymentsAmount As Double = 0,
                                  Optional ByRef vHasClaimPaymentsAuthority As Object = Nothing,
                                  Optional ByRef vClaimPaymentsAmount As Double = 0,
                                  Optional ByRef vOverrideDate As Object = Nothing,
                                  Optional ByRef vOverrideRate As Object = Nothing,
                                  Optional ByRef vOverridePrePolicyDate As Object = Nothing,
                                  Optional ByRef vOverridePrePolicyRate As Object = Nothing,
                                  Optional ByRef vWriteOffCurrencyID As Object = Nothing,
                                  Optional ByRef vTransWriteOffCurrencyID As Object = Nothing,
                                  Optional ByRef vPaymentsCurrencyID As Object = Nothing,
                                  Optional ByRef vClaimPaymentsCurrencyID As Object = Nothing,
                                  Optional ByRef vIsViewClient As Object = Nothing,
                                  Optional ByRef vIsEditClient As Object = Nothing,
                                  Optional ByRef vIsEditPolicy As Object = Nothing,
                                  Optional ByRef vIsEditClaim As Object = Nothing,
                                  Optional ByRef vIsEditFinancePlan As Object = Nothing,
                                  Optional ByRef vIsRaiseDebit As Object = Nothing,
                                  Optional ByRef vIsRaiseCredit As Object = Nothing,
                                  Optional ByRef vIsRaiseFee As Object = Nothing,
                                  Optional ByRef vIsRaiseCash As Object = Nothing,
                                  Optional ByRef vIsReverseTransactions As Object = Nothing,
                                  Optional ByRef vIsReverseAllocations As Object = Nothing,
                                  Optional ByRef vIsRaiseManualDID As Object = Nothing,
                                  Optional ByRef vIsDeleteClient As Object = Nothing,
                                  Optional ByRef vIsPerformAllocations As Object = Nothing,
                                  Optional ByRef vCanPerformBrokerTransfer As Decimal = 0,
                                  Optional ByRef vDuplicateClaimOverride As Object = Nothing,
                                  Optional ByRef vIsDeletePolicy As Object = Nothing,
                                  Optional ByRef vIsEditSchemePolicy As Object = Nothing,
                                  Optional ByRef vCanMakeLiveInvoice As Object = Nothing,
                                  Optional ByRef vCAnMakeLiveInstalments As Object = Nothing,
                                  Optional ByRef vCanMakeLivePayNow As Object = Nothing,
                                  Optional ByRef vHasPaynowWriteoffAuthority As Object = Nothing,
                                  Optional ByRef vPaynowWriteOffCurrencyID As Object = Nothing,
                                  Optional ByRef vPayNowWriteoffAmount As Decimal = 0,
                                  Optional ByRef vPaynowBankAccount As Object = Nothing,
                                  Optional ByRef vPostingPeriod As Object = Nothing,
                                  Optional ByRef vUserCanChangeReserves As Object = Nothing,
                                  Optional ByRef vUserCanAddRemoveRatingSections As Object = Nothing,
                                  Optional ByRef vUserCanEditExistingRatingSections As Object = Nothing,
                                  Optional ByRef vIsViewClientManager As Object = Nothing,
                                  Optional ByRef vIsViewAgentMaintenance As Object = Nothing,
                                  Optional ByRef vIsViewAccountHandler As Object = Nothing,
                                  Optional ByRef vIsViewAccountExecutive As Object = Nothing,
                                  Optional ByRef vIsViewInsurerMaintenance As Object = Nothing,
                                  Optional ByRef vIsViewOtherParty As Object = Nothing,
                                  Optional ByRef vParams As Object = Nothing,
                                  Optional ByRef vViewBatchProcessStatus As Object = Nothing,
                                  Optional ByRef vCurrencyLossGainLimit As Object = Nothing,
                                  Optional ByRef vLossGainCurrencyID As Object = Nothing,
                                  Optional ByRef vHasManualJournalAuthority As Object = Nothing,
                                  Optional ByRef vManualJournalAmount As Double = 0,
                                  Optional ByRef vManualJournalCurrencyID As Object = Nothing,
                                  Optional ByVal sUniqueId As String = "",
                                  Optional ByVal sScreenHierarchy As String = "",
                                  Optional ByVal iModifiedByUserId As Integer = 0,
                                  Optional ByRef vVoidTransaction As String = "") As Integer

        Dim nResult As Integer
        Dim bDataChanged As Boolean

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then
                'Default Any Missing Parameters
                nResult = CType(DefaultParameters(bDefaultAll:=False, vUserID:=vUserID,
                                                    vHasWriteOffAuthority:=vHasWriteOffAuthority,
                                                    vWriteOffAmount:=vWriteOffAmount,
                                                    vHasUnrestrictedEnquiry:=vHasUnrestrictedEnquiry,
                                                    vHasUnrestrictedUpdate:=vHasUnrestrictedUpdate,
                                                    vFeeDiscount:=vFeeDiscount,
                                                    vHasTransWriteOffAuthority:=vHasTransWriteOffAuthority,
                                                    vTransWriteOffAmount:=vTransWriteOffAmount,
                                                    vHasRefundAuthority:=vHasRefundAuthority,
                                                    vHasTransferAuthority:=vHasTransferAuthority,
                                                    vHasPaymentsAuthority:=vHasPaymentsAuthority,
                                                    vPaymentsAmount:=vPaymentsAmount,
                                                    vHasClaimPaymentsAuthority:=vHasClaimPaymentsAuthority,
                                                    vClaimPaymentsAmount:=vClaimPaymentsAmount,
                                                    vOverrideDate:=vOverrideDate, vOverrideRate:=vOverrideRate,
                                                    vOverridePrePolicyDate:=vOverridePrePolicyDate,
                                                    vOverridePrePolicyRate:=vOverridePrePolicyRate,
                                                    vWriteOffCurrencyID:=vWriteOffCurrencyID,
                                                    vTransWriteOffCurrencyID:=vTransWriteOffCurrencyID,
                                                    vPaymentsCurrencyID:=vPaymentsCurrencyID,
                                                    vClaimPaymentsCurrencyID:=vClaimPaymentsCurrencyID,
                                                    vIsViewClient:=vIsViewClient, vIsEditClient:=vIsEditClient,
                                                    vIsEditPolicy:=vIsEditPolicy, vIsEditClaim:=vIsEditClaim,
                                                    vIsEditFinancePlan:=vIsEditFinancePlan,
                                                    vIsRaiseDebit:=vIsRaiseDebit, vIsRaiseCredit:=vIsRaiseCredit,
                                                    vIsRaiseFee:=vIsRaiseFee, vIsRaiseCash:=vIsRaiseCash,
                                                    vIsReverseTransactions:=vIsReverseTransactions,
                                                    vIsReverseAllocations:=vIsReverseAllocations,
                                                    vIsRaiseManualDID:=vIsRaiseManualDID,
                                                    vIsDeleteClient:=vIsDeleteClient,
                                                    vIsPerformAllocations:=vIsPerformAllocations,
                                                    vCanPerformBrokerTransfer:=vCanPerformBrokerTransfer,
                                                    vDuplicateClaimOverride:=vDuplicateClaimOverride,
                                                    vIsDeletePolicy:=vIsDeletePolicy,
                                                    vIsEditSchemePolicy:=vIsEditSchemePolicy,
                                                    vHasViewBatchProcessStatus:=vViewBatchProcessStatus,
                                                    vCurrencyLossGainLimit:=vCurrencyLossGainLimit,
                                                    vLossGainCurrencyID:=vLossGainCurrencyID,
                                                    vHasManualJournalAuthority:=vHasManualJournalAuthority,
                                                    vManualJournalAmount:=vManualJournalAmount,
                                                    vManualJournalCurrencyID:=vManualJournalCurrencyID,
                                                    vVoidTransaction:=vVoidTransaction),
                                  gPMConstants.PMEReturnCode)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return nResult
                End If
            End If
            nResult = CType(Validate(vUserID:=vUserID, vHasWriteOffAuthority:=vHasWriteOffAuthority,
                                       vWriteOffAmount:=vWriteOffAmount,
                                       vHasUnrestrictedEnquiry:=vHasUnrestrictedEnquiry,
                                       vHasUnrestrictedUpdate:=vHasUnrestrictedUpdate,
                                       vFeeDiscount:=vFeeDiscount,
                                       vHasPaymentsAuthority:=vHasPaymentsAuthority,
                                       vPaymentsAmount:=vPaymentsAmount,
                                       vHasClaimPaymentsAuthority:=vHasClaimPaymentsAuthority,
                                       vClaimPaymentsAmount:=vClaimPaymentsAmount, vOverrideDate:=vOverrideDate,
                                       vOverrideRate:=vOverrideRate, vOverridePrePolicyDate:=vOverridePrePolicyDate,
                                       vOverridePrePolicyRate:=vOverridePrePolicyRate,
                                       vWriteOffCurrencyID:=vWriteOffCurrencyID,
                                       vTransWriteOffCurrencyID:=vTransWriteOffCurrencyID,
                                       vPaymentsCurrencyID:=vPaymentsCurrencyID,
                                       vClaimPaymentsCurrencyID:=vClaimPaymentsCurrencyID,
                                       vIsViewClient:=vIsViewClient, vIsEditClient:=vIsEditClient,
                                       vIsEditPolicy:=vIsEditPolicy, vIsEditClaim:=vIsEditClaim,
                                       vIsEditFinancePlan:=vIsEditFinancePlan, vIsRaiseDebit:=vIsRaiseDebit,
                                       vIsRaiseCredit:=vIsRaiseCredit, vIsRaiseFee:=vIsRaiseFee,
                                       vIsRaiseCash:=vIsRaiseCash, vIsReverseTransactions:=vIsReverseTransactions,
                                       vIsReverseAllocations:=vIsReverseAllocations,
                                       vIsRaiseManualDID:=vIsRaiseManualDID, vIsDeleteClient:=vIsDeleteClient,
                                       vIsPerformAllocations:=vIsPerformAllocations,
                                       vCanPerformBrokerTransfer:=vCanPerformBrokerTransfer,
                                       vIsDeletePolicy:=vIsDeletePolicy, vIsEditSchemePolicy:=vIsEditSchemePolicy,
                                       vPayNowWriteoffAmount:=vPayNowWriteoffAmount,
                                       vIsViewClientManager:=vIsViewClientManager,
                                       vIsViewAgentMaintenance:=vIsViewAgentMaintenance,
                                       vIsViewAccountHandler:=vIsViewAccountHandler,
                                       vIsViewAccountExecutive:=vIsViewAccountExecutive,
                                       vIsViewInsurerMaintenance:=vIsViewInsurerMaintenance,
                                       vIsViewOtherParty:=vIsViewOtherParty, vParams:=vParams,
                                       vHasViewBatchProcessStatus:=vViewBatchProcessStatus,
                                       vCurrencyLossGainLimit:=vCurrencyLossGainLimit,
                                       vLossGainCurrencyID:=vLossGainCurrencyID,
                                        vHasManualJournalAuthority:=vHasManualJournalAuthority,
                                        vManualJournalAmount:=vManualJournalAmount,
                                        vManualJournalCurrencyID:=vManualJournalCurrencyID),
                              gPMConstants.PMEReturnCode)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            'Set Data Changed Flag to False
            bDataChanged = False

            'Set Property values.

            If Not Informations.IsNothing(vUserID) Then
                If m_iUAUserID <> vUserID Then
                    m_iUAUserID = vUserID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vHasWriteOffAuthority) Then
                If m_iHasWriteOffAuthority <> vHasWriteOffAuthority Then
                    m_iHasWriteOffAuthority = vHasWriteOffAuthority
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vWriteOffAmount) Then
                If Math.Abs(m_dWriteOffAmount - vWriteOffAmount) > 0.0 Then
                    m_dWriteOffAmount = vWriteOffAmount
                    bDataChanged = True
                End If
            End If

            If Informations.IsNothing(vCurrencyLossGainLimit) OrElse String.IsNullOrEmpty(vCurrencyLossGainLimit) Then
                m_dCurrencyLossGainLimit = Nothing
                bDataChanged = True
            Else
                If m_dCurrencyLossGainLimit <> vCurrencyLossGainLimit Then
                    m_dCurrencyLossGainLimit = vCurrencyLossGainLimit
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vLossGainCurrencyID) Then
                If m_iLossGainCurrencyID <> vLossGainCurrencyID Then
                    m_iLossGainCurrencyID = vLossGainCurrencyID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vHasUnrestrictedEnquiry) Then
                If m_iUnrestrictedEnquiry <> vHasUnrestrictedEnquiry Then
                    m_iUnrestrictedEnquiry = vHasUnrestrictedEnquiry
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vHasUnrestrictedUpdate) Then
                If m_iUnrestrictedUpdate <> vHasUnrestrictedUpdate Then
                    m_iUnrestrictedUpdate = vHasUnrestrictedUpdate
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vViewBatchProcessStatus) Then
                If m_nViewbatchProcessStatus <> vViewBatchProcessStatus Then
                    m_nViewbatchProcessStatus = vViewBatchProcessStatus
                    bDataChanged = True
                End If
            End If

            If Informations.IsArray(vParams) AndAlso vParams.GetUpperBound(0) >= kACCCanExtractClientData Then
                If m_nCanExtractClientData <> gPMFunctions.ToSafeInteger(vParams(kACCCanExtractClientData)) Then
                    m_nCanExtractClientData = gPMFunctions.ToSafeInteger(vParams(kACCCanExtractClientData))
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vFeeDiscount) Then
                If m_lFeeDiscount <> vFeeDiscount Then
                    m_lFeeDiscount = vFeeDiscount
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vHasTransWriteOffAuthority) Then
                If m_iHasTransWriteOffAuthority <> vHasTransWriteOffAuthority Then
                    m_iHasTransWriteOffAuthority = vHasTransWriteOffAuthority
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vTransWriteOffAmount) Then
                If Math.Abs(m_dTransWriteOffAmount - vTransWriteOffAmount) > 0.0 Then
                    m_dTransWriteOffAmount = vTransWriteOffAmount
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vHasRefundAuthority) Then
                If m_iHasRefundAuthority <> vHasRefundAuthority Then
                    m_iHasRefundAuthority = vHasRefundAuthority
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vHasTransferAuthority) Then
                If m_iHasTransferAuthority <> vHasTransferAuthority Then
                    m_iHasTransferAuthority = vHasTransferAuthority
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vHasPaymentsAuthority) Then
                If m_iHasPaymentsAuthority <> vHasPaymentsAuthority Then
                    m_iHasPaymentsAuthority = vHasPaymentsAuthority
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPaymentsAmount) Then
                If Math.Abs(m_dPaymentsAmount - vPaymentsAmount) > 0.0 Then
                    m_dPaymentsAmount = vPaymentsAmount
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vHasManualJournalAuthority) Then
                If m_iHasManualJournalAuthority <> vHasManualJournalAuthority Then
                    m_iHasManualJournalAuthority = vHasManualJournalAuthority
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vManualJournalAmount) Then
                If Math.Abs(m_dManualJournalAmount - vManualJournalAmount) > 0.0 Then
                    m_dManualJournalAmount = vManualJournalAmount
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vHasClaimPaymentsAuthority) Then
                If m_iHasClaimPaymentsAuthority <> vHasClaimPaymentsAuthority Then
                    m_iHasClaimPaymentsAuthority = vHasClaimPaymentsAuthority
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vClaimPaymentsAmount) Then
                If Math.Abs(m_dClaimPaymentsAmount - vClaimPaymentsAmount) > 0.0 Then
                    m_dClaimPaymentsAmount = vClaimPaymentsAmount
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vOverrideDate) Then
                If m_iOverrideDate <> vOverrideDate Then
                    m_iOverrideDate = vOverrideDate
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vOverrideRate) Then
                If m_iOverrideRate <> vOverrideRate Then
                    m_iOverrideRate = vOverrideRate
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vOverridePrePolicyDate) Then
                If m_iOverridePrePolicyDate <> vOverridePrePolicyDate Then
                    m_iOverridePrePolicyDate = vOverridePrePolicyDate
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vOverridePrePolicyRate) Then
                If m_iOverridePrePolicyRate <> vOverridePrePolicyRate Then
                    m_iOverridePrePolicyRate = vOverridePrePolicyRate
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vWriteOffCurrencyID) AndAlso Not Informations.IsNothing(vHasWriteOffAuthority) AndAlso Not vHasWriteOffAuthority = 0 Then
                If m_iWriteOffCurrencyID <> vWriteOffCurrencyID Then
                    m_iWriteOffCurrencyID = vWriteOffCurrencyID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vTransWriteOffCurrencyID) AndAlso Not Informations.IsNothing(vHasTransWriteOffAuthority) AndAlso vHasTransWriteOffAuthority = 0 Then
                If m_iTransWriteOffCurrencyID <> vTransWriteOffCurrencyID Then
                    m_iTransWriteOffCurrencyID = vTransWriteOffCurrencyID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPaymentsCurrencyID) AndAlso Not Informations.IsNothing(vHasPaymentsAuthority) AndAlso Not vHasPaymentsAuthority = 0 Then
                If m_iPaymentsCurrencyID <> vPaymentsCurrencyID Then
                    m_iPaymentsCurrencyID = vPaymentsCurrencyID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vManualJournalCurrencyID) AndAlso Not Informations.IsNothing(vHasManualJournalAuthority) AndAlso Not vHasManualJournalAuthority = 0 Then
                If m_iManualJournalCurrencyID <> vManualJournalCurrencyID Then
                    m_iManualJournalCurrencyID = vManualJournalCurrencyID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vClaimPaymentsCurrencyID) AndAlso Not Informations.IsNothing(vHasClaimPaymentsAuthority) AndAlso Not vHasClaimPaymentsAuthority = 0 Then
                If m_iClaimPaymentsCurrencyID <> vClaimPaymentsCurrencyID Then
                    m_iClaimPaymentsCurrencyID = vClaimPaymentsCurrencyID
                    bDataChanged = True
                End If
            End If
            ' 19/04/2005 - 2005 Staff Control Added New Client Manager Security fields

            If Not Informations.IsNothing(vIsViewClient) Then
                If m_iIsViewClient <> vIsViewClient Then
                    m_iIsViewClient = vIsViewClient
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vIsEditClient) Then
                If m_iIsEditClient <> vIsEditClient Then
                    m_iIsEditClient = vIsEditClient
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vIsEditPolicy) Then
                If m_iIsEditPolicy <> vIsEditPolicy Then
                    m_iIsEditPolicy = vIsEditPolicy
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vIsEditClaim) Then
                If m_iIsEditClaim <> vIsEditClaim Then
                    m_iIsEditClaim = vIsEditClaim
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vIsEditFinancePlan) Then
                If m_iIsEditFinancePlan <> vIsEditFinancePlan Then
                    m_iIsEditFinancePlan = vIsEditFinancePlan
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vIsRaiseDebit) Then
                If m_iIsRaiseDebit <> vIsRaiseDebit Then
                    m_iIsRaiseDebit = vIsRaiseDebit
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vIsRaiseCredit) Then
                If m_iIsRaiseCredit <> vIsRaiseCredit Then
                    m_iIsRaiseCredit = vIsRaiseCredit
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vIsRaiseFee) Then
                If m_iIsRaiseFee <> vIsRaiseFee Then
                    m_iIsRaiseFee = vIsRaiseFee
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vIsRaiseCash) Then
                If m_iIsRaiseCash <> vIsRaiseCash Then
                    m_iIsRaiseCash = vIsRaiseCash
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vIsReverseTransactions) Then
                If m_iIsReverseTransactions <> vIsReverseTransactions Then
                    m_iIsReverseTransactions = vIsReverseTransactions
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vIsReverseAllocations) Then
                If m_iIsReverseAllocations <> vIsReverseAllocations Then
                    m_iIsReverseAllocations = vIsReverseAllocations
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vIsRaiseManualDID) Then
                If m_iIsRaiseManualDID <> vIsRaiseManualDID Then
                    m_iIsRaiseManualDID = vIsRaiseManualDID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vIsDeleteClient) Then
                If m_iIsDeleteClient <> vIsDeleteClient Then
                    m_iIsDeleteClient = vIsDeleteClient
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vIsPerformAllocations) Then
                If m_iIsPerformAllocations <> vIsPerformAllocations Then
                    m_iIsPerformAllocations = vIsPerformAllocations
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCanPerformBrokerTransfer) Then
                If m_iCanPerformBrokerTransfer <> vCanPerformBrokerTransfer Then
                    m_iCanPerformBrokerTransfer = vCanPerformBrokerTransfer
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vDuplicateClaimOverride) Then
                If m_iDuplicateClaimOverride <> vDuplicateClaimOverride Then
                    m_iDuplicateClaimOverride = vDuplicateClaimOverride
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vIsDeletePolicy) Then
                If m_iIsDeletePolicy <> vIsDeletePolicy Then
                    m_iIsDeletePolicy = vIsDeletePolicy
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vIsEditSchemePolicy) Then
                If m_iIsEditSchemePolicy <> vIsEditSchemePolicy Then
                    m_iIsEditSchemePolicy = vIsEditSchemePolicy
                    bDataChanged = True
                End If
            End If


            'Flaot Balance and Pre-Payment Work
            If Not Informations.IsNothing(vCanMakeLiveInvoice) Then
                If m_iCanMakeLiveInvoice <> vCanMakeLiveInvoice Then
                    m_iCanMakeLiveInvoice = vCanMakeLiveInvoice
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vCAnMakeLiveInstalments) Then
                If m_iCanMakeLiveInstalments <> vCAnMakeLiveInstalments Then
                    m_iCanMakeLiveInstalments = vCAnMakeLiveInstalments
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vCanMakeLivePayNow) Then
                If m_iCanMakeLivePaynow <> vCanMakeLivePayNow Then
                    m_iCanMakeLivePaynow = vCanMakeLivePayNow
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vHasPaynowWriteoffAuthority) Then
                If m_iHasPaynowWriteoffAuthority <> vHasPaynowWriteoffAuthority Then
                    m_iHasPaynowWriteoffAuthority = vHasPaynowWriteoffAuthority
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vPaynowWriteOffCurrencyID) AndAlso Not Informations.IsNothing(vHasPaynowWriteoffAuthority) AndAlso Not vHasPaynowWriteoffAuthority = 0 Then
                If m_iPaynowWriteoffCurrencyID <> vPaynowWriteOffCurrencyID Then
                    m_iPaynowWriteoffCurrencyID = vPaynowWriteOffCurrencyID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vPayNowWriteoffAmount) Then
                If m_cPaynowWriteOffAmount <> vPayNowWriteoffAmount Then
                    m_cPaynowWriteOffAmount = vPayNowWriteoffAmount
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vPaynowBankAccount) Then
                If m_iPaynowBankAccount <> vPaynowBankAccount Then
                    m_iPaynowBankAccount = vPaynowBankAccount
                    bDataChanged = True
                End If
            End If

            '(RC) IH-UDPP
            If Not Informations.IsNothing(vPostingPeriod) Then
                If m_iPostingPeriod <> vPostingPeriod Then
                    m_iPostingPeriod = vPostingPeriod
                    bDataChanged = True
                End If
            End If

            '(RC) QBENZ001
            If Not Informations.IsNothing(vUserCanChangeReserves) Then
                If m_iUserCanChangeReserves <> vUserCanChangeReserves Then
                    m_iUserCanChangeReserves = vUserCanChangeReserves
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vUserCanAddRemoveRatingSections) Then
                If m_iUserCanAddRemoveRatingSections <> vUserCanAddRemoveRatingSections Then
                    m_iUserCanAddRemoveRatingSections = vUserCanAddRemoveRatingSections
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vUserCanEditExistingRatingSections) Then
                If m_iUserCanEditExistingRatingSections <> vUserCanEditExistingRatingSections Then
                    m_iUserCanEditExistingRatingSections = vUserCanEditExistingRatingSections
                    bDataChanged = True
                End If
            End If

            ' If we have changed one of the properties, update the status
            If bDataChanged Then
                m_iDatabaseStatus = iStatus
            End If

            'Party View

            If Not Informations.IsNothing(vIsViewClientManager) Then
                If m_iViewClientManager <> vIsViewClientManager Then
                    m_iViewClientManager = vIsViewClientManager
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vIsViewAgentMaintenance) Then
                If m_iAgentMaintenance <> vIsViewAgentMaintenance Then
                    m_iAgentMaintenance = vIsViewAgentMaintenance
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vIsViewAccountHandler) Then
                If m_iAccountHandler <> vIsViewAccountHandler Then
                    m_iAccountHandler = vIsViewAccountHandler
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vIsViewAccountExecutive) Then
                If m_iAccountExecutive <> vIsViewAccountExecutive Then
                    m_iAccountExecutive = vIsViewAccountExecutive
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vIsViewInsurerMaintenance) Then
                If m_iInsurerMaintenance <> vIsViewInsurerMaintenance Then
                    m_iInsurerMaintenance = vIsViewInsurerMaintenance
                    bDataChanged = True
                End If
            End If
			
			 If Not Information.IsNothing(vVoidTransaction) Then
                If m_VoidTransaction <> vVoidTransaction Then
                    m_VoidTransaction = vVoidTransaction
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vIsViewOtherParty) Then
                If m_iOtherPartyMaintenance <> vIsViewOtherParty Then
                    m_iOtherPartyMaintenance = vIsViewOtherParty
                    bDataChanged = True
                End If
            End If
            If Informations.IsArray(vParams) Then
                If m_iIsRecommender <> gPMFunctions.ToSafeInteger(vParams(ACIsRecommenderArrPos)) Then
                    m_iIsRecommender = gPMFunctions.ToSafeInteger(vParams(ACIsRecommenderArrPos))
                    bDataChanged = True
                End If
                If m_cRecommenderCurrencyAmt <> gPMFunctions.ToSafeCurrency(vParams(ACRecommendationAmountArrPos)) Then
                    m_cRecommenderCurrencyAmt = gPMFunctions.ToSafeCurrency(vParams(ACRecommendationAmountArrPos))
                    bDataChanged = True
                End If
                If m_iRecommenderCurrencyID <> gPMFunctions.ToSafeInteger(vParams(ACRecommendationCurrencyArrPos)) Then
                    m_iRecommenderCurrencyID = gPMFunctions.ToSafeInteger(vParams(ACRecommendationCurrencyArrPos))
                    bDataChanged = True
                End If
                If m_iCanReverseReplaceTransactions <> gPMFunctions.ToSafeInteger(vParams(ACCanReverseReplaceArrPos)) Then
                    m_iCanReverseReplaceTransactions = gPMFunctions.ToSafeInteger(vParams(ACCanReverseReplaceArrPos))
                    bDataChanged = True
                End If

                'Payment Maintenance
                If m_iCanReverseAllocations <> gPMFunctions.ToSafeInteger(vParams(ACCanReverseAllocationArrPos)) Then
                    m_iCanReverseAllocations = gPMFunctions.ToSafeInteger(vParams(ACCanReverseAllocationArrPos))
                    bDataChanged = True
                End If

                If m_iTimePeriodForReversal <> gPMFunctions.ToSafeInteger(vParams(ACTimePeriodForReversalArrPos)) Then
                    m_iTimePeriodForReversal = gPMFunctions.ToSafeInteger(vParams(ACTimePeriodForReversalArrPos))
                    bDataChanged = True
                End If

                If m_iMTAAuthority <> gPMFunctions.ToSafeInteger(vParams(ACMTAAuthorityArrPos)) Then
                    m_iMTAAuthority = gPMFunctions.ToSafeInteger(vParams(ACMTAAuthorityArrPos))
                    bDataChanged = True
                End If

                If m_iOverrideChequeNumber <> gPMFunctions.ToSafeInteger(vParams(ACChequeNumberArrPos)) Then
                    m_iOverrideChequeNumber = gPMFunctions.ToSafeInteger(vParams(ACChequeNumberArrPos))
                    bDataChanged = True
                End If

                'Start(Saurabh Agrawal)- Tech Spec WR3 User Level RI Display Restriction(5.2)
                If m_iDisplayReinsuranceScreen <> gPMFunctions.ToSafeInteger(vParams(ACDisplayReinsuranceScreen)) Then
                    m_iDisplayReinsuranceScreen = gPMFunctions.ToSafeInteger(vParams(ACDisplayReinsuranceScreen))
                    bDataChanged = True
                End If

                If m_iDisplayClaimReinsurance <> gPMFunctions.ToSafeInteger(vParams(ACDisplayClaimReinsurance)) Then
                    m_iDisplayClaimReinsurance = gPMFunctions.ToSafeInteger(vParams(ACDisplayClaimReinsurance))
                    bDataChanged = True
                End If
                'End(Saurabh Agrawal)- Tech Spec WR3 User Level RI Display Restriction(5.2)
                If m_iCanMakeLiveBankGuarantee <> gPMFunctions.ToSafeInteger(vParams(ACMakeLiveBankGuarantee)) Then
                    m_iCanMakeLiveBankGuarantee = gPMFunctions.ToSafeInteger(vParams(ACMakeLiveBankGuarantee))
                    bDataChanged = True
                End If

                If m_iCanBackdateCollectionDate <> gPMFunctions.ToSafeInteger(vParams(ACCanBackdateCollectionDate)) Then
                    m_iCanBackdateCollectionDate = gPMFunctions.ToSafeInteger(vParams(ACCanBackdateCollectionDate))
                    bDataChanged = True
                End If

                If m_iEditDefaultCommission <> gPMFunctions.ToSafeInteger(vParams(ACEditDefaultCommission)) Then
                    m_iEditDefaultCommission = gPMFunctions.ToSafeInteger(vParams(ACEditDefaultCommission))
                    bDataChanged = True
                End If

                If m_iCanChangeInstalmentDefaultCurrency <> gPMFunctions.ToSafeInteger(vParams(ACUserCanChangeInstalmentDefaultCurrency)) Then
                    m_iCanChangeInstalmentDefaultCurrency = gPMFunctions.ToSafeInteger(vParams(ACUserCanChangeInstalmentDefaultCurrency))
                    bDataChanged = True
                End If

                'Start - Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
                If m_iCanMakeLiveCashDeposit <> gPMFunctions.ToSafeInteger(vParams(ACMakeLiveCashDeposit)) Then
                    m_iCanMakeLiveCashDeposit = gPMFunctions.ToSafeInteger(vParams(ACMakeLiveCashDeposit))
                    bDataChanged = True
                End If
                'End - Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
                'Start(Sumit Singla) PN: 75358
                If m_iACUserCanDebugDynamicLogicScripts <> ToSafeInteger(vParams(ACUserCanDebugDynamicLogicScripts)) Then
                    m_iACUserCanDebugDynamicLogicScripts = ToSafeInteger(vParams(ACUserCanDebugDynamicLogicScripts))
                    bDataChanged = True
                End If
                If m_iACUserServerScriptsRunInDebug <> ToSafeInteger(vParams(ACUserServerScriptsRunInDebug)) Then
                    m_iACUserServerScriptsRunInDebug = ToSafeInteger(vParams(ACUserServerScriptsRunInDebug))
                    bDataChanged = True
                End If
                'End(Sumit Singla)
                If m_nEditDefaultCommissionNBRN <> ToSafeInteger(vParams(kEditDefaultCommissionNBRN)) Then
                    m_nEditDefaultCommissionNBRN = ToSafeInteger(vParams(kEditDefaultCommissionNBRN))
                    bDataChanged = True
                End If
                If m_nEditDefaultCommissionMTA <> ToSafeInteger(vParams(kEditDefaultCommissionMTA)) Then
                    m_nEditDefaultCommissionMTA = ToSafeInteger(vParams(kEditDefaultCommissionMTA))
                    bDataChanged = True
                End If
                If m_nEditDefaultCommissionMTC <> ToSafeInteger(vParams(kEditDefaultCommissionMTC)) Then
                    m_nEditDefaultCommissionMTC = ToSafeInteger(vParams(kEditDefaultCommissionMTC))
                    bDataChanged = True
                End If
                If m_nEditDefaultCommissionMTR <> ToSafeInteger(vParams(kEditDefaultCommissionMTR)) Then
                    m_nEditDefaultCommissionMTR = ToSafeInteger(vParams(kEditDefaultCommissionMTR))
                    bDataChanged = True
                End If
                If m_nEditAgentDuringMTAMTC <> ToSafeInteger(vParams(kEditAgentDuringMTAMTC)) Then
                    m_nEditAgentDuringMTAMTC = ToSafeInteger(vParams(kEditAgentDuringMTAMTC))
                    bDataChanged = True
                End If
                ''Receipt Reversal
                If m_nAllowReceiptReversal <> gPMFunctions.ToSafeInteger(vParams(ACCanReverseReceiptArrPos)) Then
                    m_nAllowReceiptReversal = gPMFunctions.ToSafeInteger(vParams(ACCanReverseReceiptArrPos))
                    bDataChanged = True
                End If
            End If

            If m_nInstalmentStatus <> ToSafeInteger(vParams(ACInstalmentStatus)) Then
                m_nInstalmentStatus = ToSafeInteger(vParams(ACInstalmentStatus))
                bDataChanged = True
            End If
            If m_nCanEditInstalmentDate <> ToSafeInteger(vParams(ACCanEditInstalmentDueDate)) Then
                m_nCanEditInstalmentDate = ToSafeInteger(vParams(ACCanEditInstalmentDueDate))
                bDataChanged = True
            End If

            If m_nEditInstalmentDateByNoOfDays <> ToSafeInteger(vParams(ACEditInstalmentDateByNoOfDays)) Then
                m_nEditInstalmentDateByNoOfDays = ToSafeInteger(vParams(ACEditInstalmentDateByNoOfDays))
                bDataChanged = True
            End If

            If Not String.IsNullOrEmpty(sUniqueId) Then
                m_sUniqueId = sUniqueId
                m_sScreenHierarchy = sScreenHierarchy
            End If

            If Not iModifiedByUserId = 0 Then
                m_iModifiedByUserId = iModifiedByUserId
            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ''' <summary>
    ''' Returns the supplied ACTUserAuthorities property values
    ''' </summary>
    ''' <param name="iStatus"></param>
    ''' <param name="vUserID"></param>
    ''' <param name="vHasWriteOffAuthority"></param>
    ''' <param name="vWriteOffAmount"></param>
    ''' <param name="vHasUnrestrictedEnquiry"></param>
    ''' <param name="vHasUnrestrictedUpdate"></param>
    ''' <param name="vFeeDiscount"></param>
    ''' <param name="vHasTransWriteOffAuthority"></param>
    ''' <param name="vTransWriteOffAmount"></param>
    ''' <param name="vHasRefundAuthority"></param>
    ''' <param name="vHasTransferAuthority"></param>
    ''' <param name="vHasPaymentsAuthority"></param>
    ''' <param name="vPaymentsAmount"></param>
    ''' <param name="vHasClaimPaymentsAuthority"></param>
    ''' <param name="vClaimPaymentsAmount"></param>
    ''' <param name="vOverrideDate"></param>
    ''' <param name="vOverrideRate"></param>
    ''' <param name="vOverridePrePolicyDate"></param>
    ''' <param name="vOverridePrePolicyRate"></param>
    ''' <param name="vWriteOffCurrencyID"></param>
    ''' <param name="vTransWriteOffCurrencyID"></param>
    ''' <param name="vPaymentsCurrencyID"></param>
    ''' <param name="vClaimPaymentsCurrencyID"></param>
    ''' <param name="vIsViewClient"></param>
    ''' <param name="vIsEditClient"></param>
    ''' <param name="vIsEditPolicy"></param>
    ''' <param name="vIsEditClaim"></param>
    ''' <param name="vIsEditFinancePlan"></param>
    ''' <param name="vIsRaiseDebit"></param>
    ''' <param name="vIsRaiseCredit"></param>
    ''' <param name="vIsRaiseFee"></param>
    ''' <param name="vIsRaiseCash"></param>
    ''' <param name="vIsReverseTransactions"></param>
    ''' <param name="vIsReverseAllocations"></param>
    ''' <param name="vIsRaiseManualDID"></param>
    ''' <param name="vIsDeleteClient"></param>
    ''' <param name="vIsPerformAllocations"></param>
    ''' <param name="vCanPerformBrokerTransfer"></param>
    ''' <param name="vDuplicateClaimOverride"></param>
    ''' <param name="vIsDeletePolicy"></param>
    ''' <param name="vIsEditSchemePolicy"></param>
    ''' <param name="vCanMakeLiveInvoice"></param>
    ''' <param name="vCAnMakeLiveInstalments"></param>
    ''' <param name="vCanMakeLivePayNow"></param>
    ''' <param name="vHasPaynowWriteoffAuthority"></param>
    ''' <param name="vPaynowWriteOffCurrencyID"></param>
    ''' <param name="vPayNowWriteoffAmount"></param>
    ''' <param name="vPaynowBankAccount"></param>
    ''' <param name="vPostingPeriod"></param>
    ''' <param name="vUserCanChangeReserves"></param>
    ''' <param name="vUserCanAddRemoveRatingSections"></param>
    ''' <param name="vUserCanEditExistingRatingSections"></param>
    ''' <param name="vIsViewClientManager"></param>
    ''' <param name="vIsViewAgentMaintenance"></param>
    ''' <param name="vIsViewAccountHandler"></param>
    ''' <param name="vIsViewAccountExecutive"></param>
    ''' <param name="vIsViewInsurerMaintenance"></param>
    ''' <param name="vIsViewOtherParty"></param>
    ''' <param name="vParams"></param>
    ''' <param name="vCurrencyLossGainLimit"></param>
    ''' <param name="vLossGainCurrencyID"></param>
    ''' <param name="vHasManualJournalAuthority"></param>
    '''  <param name="vManualJournalAmount"></param>
    '''  <param name="vManualJournalCurrencyID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vUserID As Object = Nothing,
                                  Optional ByRef vHasWriteOffAuthority As Object = Nothing,
                                  Optional ByRef vWriteOffAmount As Double = 0,
                                  Optional ByRef vHasUnrestrictedEnquiry As Object = Nothing,
                                  Optional ByRef vHasUnrestrictedUpdate As Object = Nothing,
                                  Optional ByRef vFeeDiscount As Object = Nothing,
                                  Optional ByRef vHasTransWriteOffAuthority As Object = Nothing,
                                  Optional ByRef vTransWriteOffAmount As Double = 0,
                                  Optional ByRef vHasRefundAuthority As Object = Nothing,
                                  Optional ByRef vHasTransferAuthority As Object = Nothing,
                                  Optional ByRef vHasPaymentsAuthority As Object = Nothing,
                                  Optional ByRef vPaymentsAmount As Double = 0,
                                  Optional ByRef vHasClaimPaymentsAuthority As Object = Nothing,
                                  Optional ByRef vClaimPaymentsAmount As Double = 0,
                                  Optional ByRef vOverrideDate As Object = Nothing,
                                  Optional ByRef vOverrideRate As Object = Nothing,
                                  Optional ByRef vOverridePrePolicyDate As Object = Nothing,
                                  Optional ByRef vOverridePrePolicyRate As Object = Nothing,
                                  Optional ByRef vWriteOffCurrencyID As Object = Nothing,
                                  Optional ByRef vTransWriteOffCurrencyID As Object = Nothing,
                                  Optional ByRef vPaymentsCurrencyID As Object = Nothing,
                                  Optional ByRef vClaimPaymentsCurrencyID As Object = Nothing,
                                  Optional ByRef vIsViewClient As Object = Nothing,
                                  Optional ByRef vIsEditClient As Object = Nothing,
                                  Optional ByRef vIsEditPolicy As Object = Nothing,
                                  Optional ByRef vIsEditClaim As Object = Nothing,
                                  Optional ByRef vIsEditFinancePlan As Object = Nothing,
                                  Optional ByRef vIsRaiseDebit As Object = Nothing,
                                  Optional ByRef vIsRaiseCredit As Object = Nothing,
                                  Optional ByRef vIsRaiseFee As Object = Nothing,
                                  Optional ByRef vIsRaiseCash As Object = Nothing,
                                  Optional ByRef vIsReverseTransactions As Object = Nothing,
                                  Optional ByRef vIsReverseAllocations As Object = Nothing,
                                  Optional ByRef vIsRaiseManualDID As Object = Nothing,
                                  Optional ByRef vIsDeleteClient As Object = Nothing,
                                  Optional ByRef vIsPerformAllocations As Object = Nothing,
                                  Optional ByRef vCanPerformBrokerTransfer As Decimal = 0,
                                  Optional ByRef vDuplicateClaimOverride As Object = Nothing,
                                  Optional ByRef vIsDeletePolicy As Object = Nothing,
                                  Optional ByRef vIsEditSchemePolicy As Object = Nothing,
                                  Optional ByRef vCanMakeLiveInvoice As Object = Nothing,
                                  Optional ByRef vCAnMakeLiveInstalments As Object = Nothing,
                                  Optional ByRef vCanMakeLivePayNow As Object = Nothing,
                                  Optional ByRef vHasPaynowWriteoffAuthority As Object = Nothing,
                                  Optional ByRef vPaynowWriteOffCurrencyID As Object = Nothing,
                                  Optional ByRef vPayNowWriteoffAmount As Decimal = 0,
                                  Optional ByRef vPaynowBankAccount As Object = Nothing,
                                  Optional ByRef vPostingPeriod As Object = Nothing,
                                  Optional ByRef vUserCanChangeReserves As Object = Nothing,
                                  Optional ByRef vUserCanAddRemoveRatingSections As Object = Nothing,
                                  Optional ByRef vUserCanEditExistingRatingSections As Object = Nothing,
                                  Optional ByRef vIsViewClientManager As Object = Nothing,
                                  Optional ByRef vIsViewAgentMaintenance As Object = Nothing,
                                  Optional ByRef vIsViewAccountHandler As Object = Nothing,
                                  Optional ByRef vIsViewAccountExecutive As Object = Nothing,
                                  Optional ByRef vIsViewInsurerMaintenance As Object = Nothing,
                                  Optional ByRef vIsViewOtherParty As Object = Nothing,
                                  Optional ByRef vParams As Object = Nothing,
                                  Optional ByRef vCurrencyLossGainLimit As Object = Nothing,
                                  Optional ByRef vLossGainCurrencyID As Object = Nothing,
                                   Optional ByRef vHasManualJournalAuthority As Object = Nothing,
                                  Optional ByRef vManualJournalAmount As Double = 0, Optional ByRef vManualJournalCurrencyID As Object = Nothing,
                                  Optional ByRef vVoidTransaction As String = "") As Integer

        Dim nResult As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.

            vUserID = m_iUAUserID

            vHasWriteOffAuthority = m_iHasWriteOffAuthority

            vWriteOffAmount = m_dWriteOffAmount

            vCurrencyLossGainLimit = m_dCurrencyLossGainLimit
			
			 vVoidTransaction = m_VoidTransaction

            vLossGainCurrencyID = m_iLossGainCurrencyID

            vHasUnrestrictedEnquiry = m_iUnrestrictedEnquiry

            vHasUnrestrictedUpdate = m_iUnrestrictedUpdate

            vFeeDiscount = m_lFeeDiscount

            vHasTransWriteOffAuthority = m_iHasTransWriteOffAuthority

            vTransWriteOffAmount = m_dTransWriteOffAmount

            vHasRefundAuthority = m_iHasRefundAuthority

            vHasTransferAuthority = m_iHasTransferAuthority

            vHasPaymentsAuthority = m_iHasPaymentsAuthority

            vPaymentsAmount = m_dPaymentsAmount

            vHasManualJournalAuthority = m_iHasManualJournalAuthority

            vManualJournalAmount = m_dManualJournalAmount

            vHasClaimPaymentsAuthority = m_iHasClaimPaymentsAuthority

            vClaimPaymentsAmount = m_dClaimPaymentsAmount

            vOverrideDate = m_iOverrideDate

            vOverrideRate = m_iOverrideRate

            vOverridePrePolicyDate = m_iOverridePrePolicyDate

            vOverridePrePolicyRate = m_iOverridePrePolicyRate

            vWriteOffCurrencyID = m_iWriteOffCurrencyID

            vTransWriteOffCurrencyID = m_iTransWriteOffCurrencyID

            vPaymentsCurrencyID = m_iPaymentsCurrencyID

            vManualJournalCurrencyID = m_iManualJournalCurrencyID

            vClaimPaymentsCurrencyID = m_iClaimPaymentsCurrencyID

            vIsViewClient = m_iIsViewClient

            vIsEditClient = m_iIsEditClient

            vIsEditPolicy = m_iIsEditPolicy

            vIsEditClaim = m_iIsEditClaim

            vIsEditFinancePlan = m_iIsEditFinancePlan

            vIsRaiseDebit = m_iIsRaiseDebit

            vIsRaiseCredit = m_iIsRaiseCredit

            vIsRaiseFee = m_iIsRaiseFee

            vIsRaiseCash = m_iIsRaiseCash

            vIsReverseTransactions = m_iIsReverseTransactions

            vIsReverseAllocations = m_iIsReverseAllocations

            vIsRaiseManualDID = m_iIsRaiseManualDID

            vIsDeleteClient = m_iIsDeleteClient

            vIsPerformAllocations = m_iIsPerformAllocations

            vCanPerformBrokerTransfer = m_iCanPerformBrokerTransfer

            vDuplicateClaimOverride = m_iDuplicateClaimOverride

            vIsDeletePolicy = m_iIsDeletePolicy

            vIsEditSchemePolicy = m_iIsEditSchemePolicy

            vCanMakeLiveInvoice = m_iCanMakeLiveInvoice

            vCAnMakeLiveInstalments = m_iCanMakeLiveInstalments

            vCanMakeLivePayNow = m_iCanMakeLivePaynow

            vHasPaynowWriteoffAuthority = m_iHasPaynowWriteoffAuthority

            vPaynowWriteOffCurrencyID = m_iPaynowWriteoffCurrencyID

            vPayNowWriteoffAmount = m_cPaynowWriteOffAmount

            vPaynowBankAccount = m_iPaynowBankAccount

            vPostingPeriod = m_iPostingPeriod

            vUserCanChangeReserves = m_iUserCanChangeReserves


            vUserCanAddRemoveRatingSections = m_iUserCanAddRemoveRatingSections

            vUserCanEditExistingRatingSections = m_iUserCanEditExistingRatingSections

            'Party View

            vIsViewClientManager = m_iViewClientManager

            vIsViewAgentMaintenance = m_iAgentMaintenance

            vIsViewAccountHandler = m_iAccountHandler

            vIsViewAccountExecutive = m_iAccountExecutive

            vIsViewInsurerMaintenance = m_iInsurerMaintenance

            vIsViewOtherParty = m_iOtherPartyMaintenance

            ReDim vParams(kParamsCount)

            vParams(ACIsRecommenderArrPos) = m_iIsRecommender

            vParams(ACRecommendationCurrencyArrPos) = m_iRecommenderCurrencyID

            vParams(ACRecommendationAmountArrPos) = m_cRecommenderCurrencyAmt

            vParams(ACCanReverseReplaceArrPos) = m_iCanReverseReplaceTransactions
            'Payment Maintenance

            vParams(ACCanReverseAllocationArrPos) = m_iCanReverseAllocations

            vParams(ACTimePeriodForReversalArrPos) = m_iTimePeriodForReversal

            vParams(ACMTAAuthorityArrPos) = m_iMTAAuthority

            vParams(ACChequeNumberArrPos) = m_iOverrideChequeNumber

            vParams(ACDisplayReinsuranceScreen) = m_iDisplayReinsuranceScreen

            vParams(ACDisplayClaimReinsurance) = m_iDisplayClaimReinsurance

            vParams(ACMakeLiveBankGuarantee) = m_iCanMakeLiveBankGuarantee

            vParams(ACCanBackdateCollectionDate) = m_iCanBackdateCollectionDate


            vParams(ACEditDefaultCommission) = m_iEditDefaultCommission

            vParams(ACUserCanChangeInstalmentDefaultCurrency) = m_iCanChangeInstalmentDefaultCurrency

            vParams(ACMakeLiveCashDeposit) = m_iCanMakeLiveCashDeposit 'Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
            'End If

            vParams(ACUserCanDebugDynamicLogicScripts) = m_iACUserCanDebugDynamicLogicScripts

            vParams(ACUserServerScriptsRunInDebug) = m_iACUserServerScriptsRunInDebug

            vParams(kEditAgentDuringMTAMTC) = m_nEditAgentDuringMTAMTC

            vParams(kEditDefaultCommissionNBRN) = m_nEditDefaultCommissionNBRN

            vParams(kEditDefaultCommissionMTA) = m_nEditDefaultCommissionMTA

            vParams(kEditDefaultCommissionMTC) = m_nEditDefaultCommissionMTC

            vParams(kEditDefaultCommissionMTR) = m_nEditDefaultCommissionMTR

            vParams(ACCanReverseReceiptArrPos) = m_nAllowReceiptReversal

            vParams(kACCViewBatchProcessStatus) = m_nViewbatchProcessStatus
            vParams(kACCCanExtractClientData) = m_nCanExtractClientData

            iStatus = m_iDatabaseStatus

            vParams(ACInstalmentStatus) = m_nInstalmentStatus
            vParams(ACCanEditInstalmentDueDate) = m_nCanEditInstalmentDate
            vParams(ACEditInstalmentDateByNoOfDays) = m_nEditInstalmentDateByNoOfDays

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectItem (Public)
    '
    ' Description: Reads the Base Details from the Database .
    '
    ' ***************************************************************** '
    Public Function SelectItem() As Integer
        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add PrimaryKey as INPUT parameters
            m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectSingleSQL, sSQLName:=ACSelectSingleName, bStoredProcedure:=ACSelectSingleStored, bKeepNulls:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount = 1 Then
                ' Selected, No action required
            Else
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Set properties
            m_lReturn = CType(SetPropertiesFromDB(oFields:=m_oDatabase.Records.Item(0).Fields()), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Public Function AddItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add PrimaryKey as OUTPUT parameters
            m_lReturn = CType(AddKeyOutputParam(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add the required INPUT parameters
            m_lReturn = CType(AddInputParam(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Primary Key of the record inserted
            m_lReturn = CType(GetNewPrimaryKeyID(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteItem (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    '
    ' ***************************************************************** '
    Public Function DeleteItem() As Integer
        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add PrimaryKey as INPUT parameters
            m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If record wasn't deleted, error
            If lRecordsAffected > 0 Then
                ' Deleted, No action required
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateItem (Public)
    '
    ' Description: UpdateItems a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Public Function UpdateItem() As Integer
        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add PrimaryKey as INPUT parameters
            m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add the required INPUT parameters
            m_lReturn = CType(AddInputParam(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check to see that the record was updated OK
            If lRecordsAffected > 0 Then
                ' Updated No action required
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: DefaultParameters (Private)
    '
    ' Description: Sets the Default Values for a ACTUserAuthorities.
    '
    ' 19/04/2005 - 2005 Staff Control Added New Client Manager Security fields
    ' 13/10/2005 - New Option to switch of Editing of Schemes Type policies
    ' ***************************************************************** '
    'developer guide no. 101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vHasWriteOffAuthority As Object = Nothing, Optional ByRef vWriteOffAmount As Object = Nothing, Optional ByRef vHasUnrestrictedEnquiry As Object = Nothing, Optional ByRef vHasUnrestrictedUpdate As Object = Nothing, Optional ByRef vFeeDiscount As Object = Nothing, Optional ByRef vHasTransWriteOffAuthority As Object = Nothing, Optional ByRef vTransWriteOffAmount As Object = Nothing, Optional ByRef vHasRefundAuthority As Object = Nothing, Optional ByRef vHasTransferAuthority As Object = Nothing, Optional ByRef vHasPaymentsAuthority As Object = Nothing, Optional ByRef vPaymentsAmount As Object = Nothing, Optional ByRef vHasClaimPaymentsAuthority As Object = Nothing, Optional ByRef vClaimPaymentsAmount As Object = Nothing, Optional ByRef vOverrideDate As Object = Nothing, Optional ByRef vOverrideRate As Object = Nothing, Optional ByRef vOverridePrePolicyDate As Object = Nothing, Optional ByRef vOverridePrePolicyRate As Object = Nothing, Optional ByRef vWriteOffCurrencyID As Object = Nothing, Optional ByRef vTransWriteOffCurrencyID As Object = Nothing, Optional ByRef vPaymentsCurrencyID As Object = Nothing, Optional ByRef vClaimPaymentsCurrencyID As Object = Nothing, Optional ByRef vIsViewClient As Object = Nothing, Optional ByRef vIsEditClient As Object = Nothing, Optional ByRef vIsEditPolicy As Object = Nothing, Optional ByRef vIsEditClaim As Object = Nothing, Optional ByRef vIsEditFinancePlan As Object = Nothing, Optional ByRef vIsRaiseDebit As Object = Nothing, Optional ByRef vIsRaiseCredit As Object = Nothing, Optional ByRef vIsRaiseFee As Object = Nothing, Optional ByRef vIsRaiseCash As Object = Nothing, Optional ByRef vIsReverseTransactions As Object = Nothing, Optional ByRef vIsReverseAllocations As Object = Nothing, Optional ByRef vIsRaiseManualDID As Object = Nothing, Optional ByRef vIsDeleteClient As Object = Nothing, Optional ByRef vIsPerformAllocations As Object = Nothing, Optional ByRef vCanPerformBrokerTransfer As Object = Nothing, Optional ByRef vDuplicateClaimOverride As Object = Nothing, Optional ByRef vIsDeletePolicy As Object = Nothing, Optional ByRef vIsEditSchemePolicy As Object = Nothing, Optional ByRef vCanMakeLiveInvoice As Object = Nothing, Optional ByRef vCAnMakeLiveInstalments As Object = Nothing, Optional ByRef vCanMakeLivePayNow As Object = Nothing, Optional ByRef vHasPaynowWriteoffAuthority As Object = Nothing, Optional ByRef vPaynowWriteOffCurrncyID As Object = Nothing, Optional ByRef vPayNowWriteoffAmount As Object = Nothing, Optional ByRef vPaynowBankAccount As Object = Nothing, Optional ByRef vCanReverseReplaceTransactions As Object = Nothing, Optional ByRef vHasViewBatchProcessStatus As Object = Nothing, Optional ByRef vCurrencyLossGainLimit As Object = Nothing, Optional ByRef vLossGainCurrencyID As Object = Nothing, Optional ByRef vHasManualJournalAuthority As Object = Nothing, Optional ByRef vManualJournalAmount As Double = 0, Optional ByRef vManualJournalCurrencyID As Object = Nothing, Optional ByRef vVoidTransaction As String = "") As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            If (Informations.IsNothing(vUserID)) OrElse (vUserID.Equals(0)) Or (bDefaultAll) Then
                vUserID = 0
            End If



            If (Informations.IsNothing(vHasWriteOffAuthority)) OrElse (vHasWriteOffAuthority.Equals(0)) Or (bDefaultAll) Then
                vHasWriteOffAuthority = 0
            End If



            If (Informations.IsNothing(vWriteOffAmount)) OrElse (vWriteOffAmount.Equals(0)) Or (bDefaultAll) Then
                vWriteOffAmount = 0
            End If



            If (Informations.IsNothing(vHasUnrestrictedEnquiry)) OrElse (vHasUnrestrictedEnquiry.Equals(0)) Or (bDefaultAll) Then
                vHasUnrestrictedEnquiry = 0
            End If



            If (Informations.IsNothing(vHasUnrestrictedUpdate)) OrElse (vHasUnrestrictedUpdate.Equals(0)) Or (bDefaultAll) Then
                vHasUnrestrictedUpdate = 0
            End If

            If (Informations.IsNothing(vHasViewBatchProcessStatus)) OrElse (vHasViewBatchProcessStatus.Equals(0)) Or (bDefaultAll) Then
                vHasViewBatchProcessStatus = 0
            End If


            If (Informations.IsNothing(vFeeDiscount)) OrElse (vFeeDiscount.Equals(0)) Or (bDefaultAll) Then
                vFeeDiscount = 0
            End If



            If (Informations.IsNothing(vHasTransWriteOffAuthority)) OrElse (vHasTransWriteOffAuthority.Equals(0)) Or (bDefaultAll) Then
                vHasTransWriteOffAuthority = 0
            End If



            If (Informations.IsNothing(vTransWriteOffAmount)) OrElse (vTransWriteOffAmount.Equals(0)) Or (bDefaultAll) Then
                vTransWriteOffAmount = 0
            End If



            If (Informations.IsNothing(vHasRefundAuthority)) OrElse (vHasRefundAuthority.Equals(0)) Or (bDefaultAll) Then
                vHasRefundAuthority = 0
            End If



            If (Informations.IsNothing(vHasTransferAuthority)) OrElse (vHasTransferAuthority.Equals(0)) Or (bDefaultAll) Then
                vHasTransferAuthority = 0
            End If



            If (Informations.IsNothing(vHasPaymentsAuthority)) OrElse (vHasPaymentsAuthority.Equals(0)) Or (bDefaultAll) Then
                vHasPaymentsAuthority = 0
            End If



            If (Informations.IsNothing(vPaymentsAmount)) OrElse (vPaymentsAmount.Equals(0)) Or (bDefaultAll) Then
                vPaymentsAmount = 0
            End If



            If (Informations.IsNothing(vHasClaimPaymentsAuthority)) OrElse (vHasClaimPaymentsAuthority.Equals(0)) Or (bDefaultAll) Then
                vHasClaimPaymentsAuthority = 0
            End If



            If (Informations.IsNothing(vClaimPaymentsAmount)) OrElse (vClaimPaymentsAmount.Equals(0)) Or (bDefaultAll) Then
                vClaimPaymentsAmount = 0
            End If



            If (Informations.IsNothing(vOverrideDate)) OrElse (vOverrideDate.Equals(0)) Or (bDefaultAll) Then
                vOverrideDate = 0
            End If



            If (Informations.IsNothing(vOverrideRate)) OrElse (vOverrideRate.Equals(0)) Or (bDefaultAll) Then
                vOverrideRate = 0
            End If



            If (Informations.IsNothing(vOverridePrePolicyDate)) OrElse (vOverridePrePolicyDate.Equals(0)) Or (bDefaultAll) Then
                vOverridePrePolicyDate = 0
            End If



            If (Informations.IsNothing(vOverridePrePolicyRate)) OrElse (vOverridePrePolicyRate.Equals(0)) Or (bDefaultAll) Then
                vOverridePrePolicyRate = 0
            End If



            If (Informations.IsNothing(vWriteOffCurrencyID)) OrElse (vWriteOffCurrencyID.Equals(0)) Or (bDefaultAll) Then
                vWriteOffCurrencyID = 0
            End If



            If (Informations.IsNothing(vTransWriteOffCurrencyID)) OrElse (vTransWriteOffCurrencyID.Equals(0)) Or (bDefaultAll) Then
                vTransWriteOffCurrencyID = 0
            End If



            If (Informations.IsNothing(vPaymentsCurrencyID)) OrElse (vPaymentsCurrencyID.Equals(0)) Or (bDefaultAll) Then
                vPaymentsCurrencyID = 0
            End If



            If (Informations.IsNothing(vClaimPaymentsCurrencyID)) OrElse (vClaimPaymentsCurrencyID.Equals(0)) Or (bDefaultAll) Then
                vClaimPaymentsCurrencyID = 0
            End If

            ' 19/04/2005 - 2005 Staff Control Added New Client Manager Security fields



            If (Informations.IsNothing(vIsViewClient)) OrElse (vIsViewClient.Equals(0)) Or (bDefaultAll) Then
                vIsViewClient = 0
            End If



            If (Informations.IsNothing(vIsEditClient)) OrElse (vIsEditClient.Equals(0)) Or (bDefaultAll) Then
                vIsEditClient = 0
            End If



            If (Informations.IsNothing(vIsEditPolicy)) OrElse (vIsEditPolicy.Equals(0)) Or (bDefaultAll) Then
                vIsEditPolicy = 0
            End If



            If (Informations.IsNothing(vIsEditClaim)) OrElse (vIsEditClaim.Equals(0)) Or (bDefaultAll) Then
                vIsEditClaim = 0
            End If



            If (Informations.IsNothing(vIsRaiseDebit)) OrElse (vIsRaiseDebit.Equals(0)) Or (bDefaultAll) Then
                vIsRaiseDebit = 0
            End If



            If (Informations.IsNothing(vIsRaiseCredit)) OrElse (vIsRaiseCredit.Equals(0)) Or (bDefaultAll) Then
                vIsRaiseCredit = 0
            End If



            If (Informations.IsNothing(vIsRaiseFee)) OrElse (vIsRaiseFee.Equals(0)) Or (bDefaultAll) Then
                vIsRaiseFee = 0
            End If



            If (Informations.IsNothing(vIsRaiseCash)) OrElse (vIsRaiseCash.Equals(0)) Or (bDefaultAll) Then
                vIsRaiseCash = 0
            End If



            If (Informations.IsNothing(vIsReverseTransactions)) OrElse (vIsReverseTransactions.Equals(0)) Or (bDefaultAll) Then
                vIsReverseTransactions = 0
            End If


            'If (Not True) OrElse (vCanReverseReplaceTransactions.Equals(0)) Or (bDefaultAll) Then
            If (Informations.IsNothing(vCanReverseReplaceTransactions)) OrElse (vCanReverseReplaceTransactions.Equals(0)) Or (bDefaultAll) Then
                vCanReverseReplaceTransactions = 0
            End If



            If (Informations.IsNothing(vIsReverseAllocations)) OrElse (vIsReverseAllocations.Equals(0)) Or (bDefaultAll) Then
                vIsReverseAllocations = 0
            End If



            If (Informations.IsNothing(vIsRaiseManualDID)) OrElse (vIsRaiseManualDID.Equals(0)) Or (bDefaultAll) Then
                vIsRaiseManualDID = 0
            End If



            If (Informations.IsNothing(vIsDeleteClient)) OrElse (vIsDeleteClient.Equals(0)) Or (bDefaultAll) Then
                vIsDeleteClient = 0
            End If


            If (Informations.IsNothing(vIsPerformAllocations)) OrElse (vIsPerformAllocations.Equals(0)) Or (bDefaultAll) Then
                vIsPerformAllocations = 0
            End If



            If (Informations.IsNothing(vCanPerformBrokerTransfer)) OrElse (vCanPerformBrokerTransfer.Equals(0)) Or (bDefaultAll) Then
                vCanPerformBrokerTransfer = 0
            End If



            If (Informations.IsNothing(vDuplicateClaimOverride)) OrElse (vDuplicateClaimOverride.Equals(0)) Or (bDefaultAll) Then
                vDuplicateClaimOverride = 0
            End If



            If (Informations.IsNothing(vIsDeletePolicy)) OrElse (vIsDeletePolicy.Equals(0)) Or (bDefaultAll) Then
                vIsDeletePolicy = 0
            End If



            If (Informations.IsNothing(vIsEditSchemePolicy)) OrElse (vIsEditSchemePolicy.Equals(0)) Or (bDefaultAll) Then
                vIsEditSchemePolicy = 0
            End If

            'Float Balance and Pre-Payment


            'If (Not True) OrElse (vCanMakeLiveInvoice.Equals(0)) Or (bDefaultAll) Then
            If (Informations.IsNothing(vCanMakeLiveInvoice)) OrElse (vCanMakeLiveInvoice.Equals(0)) Or (bDefaultAll) Then
                vCanMakeLiveInvoice = 1
            End If


            'If (Not True) OrElse (vCAnMakeLiveInstalments.Equals(0)) Or (bDefaultAll) Then
            If (Informations.IsNothing(vCAnMakeLiveInstalments)) OrElse (vCAnMakeLiveInstalments.Equals(0)) Or (bDefaultAll) Then
                vCAnMakeLiveInstalments = 1
            End If


            'If (Not True) OrElse (vCanMakeLivePayNow.Equals(0)) Or (bDefaultAll) Then
            If (Informations.IsNothing(vCanMakeLivePayNow)) OrElse (vCanMakeLivePayNow.Equals(0)) Or (bDefaultAll) Then
                vCanMakeLivePayNow = 1
            End If


            'If (Not True) OrElse (vHasPaynowWriteoffAuthority.Equals(0)) Or (bDefaultAll) Then
            If (Informations.IsNothing(vHasPaynowWriteoffAuthority)) OrElse (vHasPaynowWriteoffAuthority.Equals(0)) Or (bDefaultAll) Then
                vHasPaynowWriteoffAuthority = 0
            End If


            'If (Not True) OrElse (vPayNowWriteoffAmount.Equals(0)) Or (bDefaultAll) Then
            If (Informations.IsNothing(vPayNowWriteoffAmount)) OrElse (vPayNowWriteoffAmount.Equals(0)) Or (bDefaultAll) Then
                vPayNowWriteoffAmount = 0
            End If

            If (Informations.IsNothing(vCurrencyLossGainLimit)) OrElse String.IsNullOrEmpty(vCurrencyLossGainLimit) Or (bDefaultAll) Then
                vCurrencyLossGainLimit = ""
            End If

            If (Informations.IsNothing(vLossGainCurrencyID)) OrElse (vLossGainCurrencyID.Equals(0)) Or (bDefaultAll) Then
                vLossGainCurrencyID = 0
            End If

            If (Informations.IsNothing(vManualJournalCurrencyID)) OrElse (vManualJournalCurrencyID.Equals(0)) Or (bDefaultAll) Then
                vManualJournalCurrencyID = 0
            End If

            If (Informations.IsNothing(vManualJournalAmount)) OrElse (vManualJournalAmount.Equals(0)) Or (bDefaultAll) Then
                vManualJournalAmount = 0
            End If

            If (Informations.IsNothing(vHasManualJournalAuthority)) OrElse (vHasManualJournalAuthority.Equals(0)) Or (bDefaultAll) Then
                vHasManualJournalAuthority = 0
            End If

            If (Information.IsNothing(vVoidTransaction)) OrElse String.IsNullOrEmpty(vVoidTransaction) Or (bDefaultAll) Then
                vVoidTransaction = ""
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DefaultParameters Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DefaultParameters", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the ACTUserAuthorities for Consistency.
    '
    ' 19/04/2005 - 2005 Staff Control Added New Client Manager Security fields
    ' 13/10/2005 - New Option to switch of Editing of Schemes Type policies
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vUserID As Object = Nothing, Optional ByRef vHasWriteOffAuthority As Object = Nothing, Optional ByRef vWriteOffAmount As Object = Nothing, Optional ByRef vHasUnrestrictedEnquiry As Object = Nothing, Optional ByRef vHasUnrestrictedUpdate As Object = Nothing, Optional ByRef vFeeDiscount As Object = Nothing, Optional ByRef vHasTransWriteOffAuthority As Object = Nothing, Optional ByRef vTransWriteOffAmount As Object = Nothing, Optional ByRef vHasRefundAuthority As Object = Nothing, Optional ByRef vHasTransferAuthority As Object = Nothing, Optional ByRef vHasPaymentsAuthority As Object = Nothing, Optional ByRef vPaymentsAmount As Object = Nothing, Optional ByRef vHasClaimPaymentsAuthority As Object = Nothing, Optional ByRef vClaimPaymentsAmount As Object = Nothing, Optional ByRef vOverrideDate As Object = Nothing, Optional ByRef vOverrideRate As Object = Nothing, Optional ByRef vOverridePrePolicyDate As Object = Nothing, Optional ByRef vOverridePrePolicyRate As Object = Nothing, Optional ByRef vWriteOffCurrencyID As Object = Nothing, Optional ByRef vTransWriteOffCurrencyID As Object = Nothing, Optional ByRef vPaymentsCurrencyID As Object = Nothing, Optional ByRef vClaimPaymentsCurrencyID As Object = Nothing, Optional ByRef vIsViewClient As Object = Nothing, Optional ByRef vIsEditClient As Object = Nothing, Optional ByRef vIsEditPolicy As Object = Nothing, Optional ByRef vIsEditClaim As Object = Nothing, Optional ByRef vIsEditFinancePlan As Object = Nothing, Optional ByRef vIsRaiseDebit As Object = Nothing, Optional ByRef vIsRaiseCredit As Object = Nothing, Optional ByRef vIsRaiseFee As Object = Nothing, Optional ByRef vIsRaiseCash As Object = Nothing, Optional ByRef vIsReverseTransactions As Object = Nothing, Optional ByRef vIsReverseAllocations As Object = Nothing, Optional ByRef vIsRaiseManualDID As Object = Nothing, Optional ByRef vIsDeleteClient As Object = Nothing, Optional ByRef vIsPerformAllocations As Object = Nothing, Optional ByRef vCanPerformBrokerTransfer As Object = Nothing, Optional ByRef vDuplicateClaimOverride As Integer = 0, Optional ByRef vIsDeletePolicy As Object = Nothing, Optional ByRef vIsEditSchemePolicy As Object = Nothing, Optional ByRef vPayNowWriteoffAmount As Decimal = 0, Optional ByRef vIsViewClientManager As Object = Nothing, Optional ByRef vIsViewAgentMaintenance As Object = Nothing, Optional ByRef vIsViewAccountHandler As Object = Nothing, Optional ByRef vIsViewAccountExecutive As Object = Nothing, Optional ByRef vIsViewInsurerMaintenance As Object = Nothing, Optional ByRef vIsViewOtherParty As Object = Nothing, Optional ByRef vParams() As Object = Nothing, Optional ByRef vHasViewBatchProcessStatus As Object = Nothing, Optional ByRef vCurrencyLossGainLimit As Object = Nothing, Optional ByRef vLossGainCurrencyID As Object = Nothing, Optional ByRef vHasManualJournalAuthority As Object = Nothing, Optional ByRef vManualJournalAmount As Double = 0, Optional ByRef vManualJournalCurrencyID As Object = Nothing) As Integer

        Dim result As Integer = 0
        ' Dim lVarRow As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Not Informations.IsNothing(vUserID) Then

                Dim dbNumericTemp As Double
                If Not Double.TryParse(CStr(vUserID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vHasWriteOffAuthority) Then

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(vHasWriteOffAuthority), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vWriteOffAmount) Then

                Dim dbNumericTemp3 As Double
                If Not Double.TryParse(CStr(vWriteOffAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If Not Informations.IsNothing(vCurrencyLossGainLimit) And Not String.IsNullOrEmpty(vCurrencyLossGainLimit) Then
                Dim dbNumericTemp3 As Double
                If Not Double.TryParse(CStr(vCurrencyLossGainLimit), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If Not Informations.IsNothing(vLossGainCurrencyID) Then

                Dim dbNumericTemp18 As Double
                If Not Double.TryParse(CStr(vLossGainCurrencyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp18) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vHasUnrestrictedEnquiry) Then

                Dim dbNumericTemp4 As Double
                If Not Double.TryParse(CStr(vHasUnrestrictedEnquiry), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vHasUnrestrictedUpdate) Then

                Dim dbNumericTemp5 As Double
                If Not Double.TryParse(CStr(vHasUnrestrictedUpdate), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If Not Informations.IsNothing(vHasViewBatchProcessStatus) Then

                Dim dbNumericTemp5 As Double
                If Not Double.TryParse(CStr(vHasViewBatchProcessStatus), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If Not Informations.IsNothing(vHasTransWriteOffAuthority) Then

                Dim dbNumericTemp6 As Double
                If Not Double.TryParse(CStr(vHasTransWriteOffAuthority), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vTransWriteOffAmount) Then

                Dim dbNumericTemp7 As Double
                If Not Double.TryParse(CStr(vTransWriteOffAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp7) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vHasRefundAuthority) Then

                Dim dbNumericTemp8 As Double
                If Not Double.TryParse(CStr(vHasRefundAuthority), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp8) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vHasTransferAuthority) Then

                Dim dbNumericTemp9 As Double
                If Not Double.TryParse(CStr(vHasTransferAuthority), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp9) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vHasPaymentsAuthority) Then

                Dim dbNumericTemp10 As Double
                If Not Double.TryParse(CStr(vHasPaymentsAuthority), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp10) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vPaymentsAmount) Then

                Dim dbNumericTemp11 As Double
                If Not Double.TryParse(CStr(vPaymentsAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp11) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vHasClaimPaymentsAuthority) Then

                Dim dbNumericTemp12 As Double
                If Not Double.TryParse(CStr(vHasClaimPaymentsAuthority), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp12) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vClaimPaymentsAmount) Then

                Dim dbNumericTemp13 As Double
                If Not Double.TryParse(CStr(vClaimPaymentsAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp13) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vOverrideDate) Then

                Dim dbNumericTemp14 As Double
                If Not Double.TryParse(CStr(vOverrideDate), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp14) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vOverrideRate) Then

                Dim dbNumericTemp15 As Double
                If Not Double.TryParse(CStr(vOverrideRate), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp15) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vOverridePrePolicyDate) Then

                Dim dbNumericTemp16 As Double
                If Not Double.TryParse(CStr(vOverridePrePolicyDate), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp16) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vOverridePrePolicyRate) Then

                Dim dbNumericTemp17 As Double
                If Not Double.TryParse(CStr(vOverridePrePolicyRate), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp17) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vWriteOffCurrencyID) Then

                Dim dbNumericTemp18 As Double
                If Not Double.TryParse(CStr(vWriteOffCurrencyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp18) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vTransWriteOffCurrencyID) Then

                Dim dbNumericTemp19 As Double
                If Not Double.TryParse(CStr(vTransWriteOffCurrencyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp19) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vPaymentsCurrencyID) Then

                Dim dbNumericTemp20 As Double
                If Not Double.TryParse(CStr(vPaymentsCurrencyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp20) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vClaimPaymentsCurrencyID) Then

                Dim dbNumericTemp21 As Double
                If Not Double.TryParse(CStr(vClaimPaymentsCurrencyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp21) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            ' 19/04/2005 - 2005 Staff Control Added New Client Manager Security fields

            If Not Informations.IsNothing(vIsViewClient) Then

                Dim dbNumericTemp22 As Double
                If Not Double.TryParse(CStr(vIsViewClient), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp22) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vIsEditClient) Then

                Dim dbNumericTemp23 As Double
                If Not Double.TryParse(CStr(vIsEditClient), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp23) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If Not Informations.IsNothing(vIsEditPolicy) Then

                Dim dbNumericTemp24 As Double
                If Not Double.TryParse(CStr(vIsEditPolicy), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp24) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If Not Informations.IsNothing(vIsEditClaim) Then

                Dim dbNumericTemp25 As Double
                If Not Double.TryParse(CStr(vIsEditClaim), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp25) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If Not Informations.IsNothing(vIsRaiseDebit) Then

                Dim dbNumericTemp26 As Double
                If Not Double.TryParse(CStr(vIsRaiseDebit), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp26) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If Not Informations.IsNothing(vIsRaiseCredit) Then

                Dim dbNumericTemp27 As Double
                If Not Double.TryParse(CStr(vIsRaiseCredit), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp27) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If Not Informations.IsNothing(vIsRaiseFee) Then

                Dim dbNumericTemp28 As Double
                If Not Double.TryParse(CStr(vIsRaiseFee), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp28) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If Not Informations.IsNothing(vIsRaiseCash) Then

                Dim dbNumericTemp29 As Double
                If Not Double.TryParse(CStr(vIsRaiseCash), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp29) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vIsReverseTransactions) Then

                Dim dbNumericTemp30 As Double
                If Not Double.TryParse(CStr(vIsReverseTransactions), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp30) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vIsReverseAllocations) Then

                Dim dbNumericTemp31 As Double
                If Not Double.TryParse(CStr(vIsReverseAllocations), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp31) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            Return result


            If Not Informations.IsNothing(vIsRaiseManualDID) Then

                Dim dbNumericTemp32 As Double
                If Not Double.TryParse(CStr(vIsRaiseManualDID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp32) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vIsDeleteClient) Then

                Dim dbNumericTemp33 As Double
                If Not Double.TryParse(CStr(vIsDeleteClient), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp33) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If Not Informations.IsNothing(vIsPerformAllocations) Then

                Dim dbNumericTemp34 As Double
                If Not Double.TryParse(CStr(vIsPerformAllocations), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp34) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vCanPerformBrokerTransfer) Then

                Dim dbNumericTemp35 As Double
                If Not Double.TryParse(CStr(vCanPerformBrokerTransfer), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp35) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If Not False Then
                Dim dbNumericTemp36 As Double
                If Not Double.TryParse(CStr(vDuplicateClaimOverride), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp36) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vIsDeletePolicy) Then

                Dim dbNumericTemp37 As Double
                If Not Double.TryParse(CStr(vIsDeletePolicy), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp37) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vIsEditSchemePolicy) Then

                Dim dbNumericTemp38 As Double
                If Not Double.TryParse(CStr(vIsEditSchemePolicy), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp38) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If Not False Then
                Dim dbNumericTemp39 As Double
                If Not Double.TryParse(CStr(vPayNowWriteoffAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp39) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Party View

            If Not Informations.IsNothing(vIsViewClientManager) Then

                Dim dbNumericTemp40 As Double
                If Not Double.TryParse(CStr(vIsViewClientManager), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp40) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vIsViewAgentMaintenance) Then

                Dim dbNumericTemp41 As Double
                If Not Double.TryParse(CStr(vIsViewAgentMaintenance), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp41) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vIsViewAccountHandler) Then

                Dim dbNumericTemp42 As Double
                If Not Double.TryParse(CStr(vIsViewAccountHandler), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp42) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vIsViewAccountExecutive) Then

                Dim dbNumericTemp43 As Double
                If Not Double.TryParse(CStr(vIsViewAccountExecutive), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp43) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vIsViewInsurerMaintenance) Then

                Dim dbNumericTemp44 As Double
                If Not Double.TryParse(CStr(vIsViewInsurerMaintenance), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp44) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vIsViewOtherParty) Then

                Dim dbNumericTemp45 As Double
                If Not Double.TryParse(CStr(vIsViewOtherParty), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp45) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If Informations.IsArray(vParams) Then

                Dim dbNumericTemp46 As Double
                If Not Double.TryParse(CStr(vParams(ACRecommendationCurrencyArrPos)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp46) And gPMFunctions.ToSafeString(vParams(ACRecommendationCurrencyArrPos)) <> "" Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                Dim dbNumericTemp47 As Double
                If Not Double.TryParse(CStr(vParams(ACRecommendationAmountArrPos)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp47) And gPMFunctions.ToSafeString(vParams(ACRecommendationAmountArrPos)) <> "" Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                Dim dbNumericTemp48 As Double
                If Not Double.TryParse(CStr(vParams(ACCanReverseReplaceArrPos)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp48) And gPMFunctions.ToSafeString(vParams(ACCanReverseReplaceArrPos)) <> "" Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                Dim dbNumericTemp49 As Double
                If Not Double.TryParse(CStr(vParams(ACChequeNumberArrPos)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp49) And gPMFunctions.ToSafeString(vParams(ACChequeNumberArrPos)) <> "" Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Start(Saurabh Agrawal)- Tech Spec WR3 User Level RI Display Restriction(5.2)

                Dim dbNumericTemp50 As Double
                If Not Double.TryParse(CStr(vParams(ACDisplayReinsuranceScreen)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp50) And gPMFunctions.ToSafeString(vParams(ACDisplayReinsuranceScreen)) <> "" Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                Dim dbNumericTemp51 As Double
                If Not Double.TryParse(CStr(vParams(ACDisplayClaimReinsurance)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp51) And gPMFunctions.ToSafeString(vParams(ACDisplayClaimReinsurance)) <> "" Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'End (Saurabh Agrawal)- Tech Spec WR3 User Level RI Display Restriction(5.2)

                Dim dbNumericTemp52 As Double
                If Not Double.TryParse(CStr(vParams(ACEditDefaultCommission)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp52) And gPMFunctions.ToSafeString(vParams(ACEditDefaultCommission)) <> "" Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not Informations.IsNothing(vHasManualJournalAuthority) Then

                    Dim dbNumericTemp55 As Double
                    If Not Double.TryParse(CStr(vHasManualJournalAuthority), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp55) Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
                If Not Informations.IsNothing(vManualJournalAmount) Then

                    Dim dbNumericTemp53 As Double
                    If Not Double.TryParse(CStr(vManualJournalAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp53) Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
                If Not Informations.IsNothing(vManualJournalCurrencyID) Then

                    Dim dbNumericTemp54 As Double
                    If Not Double.TryParse(CStr(vManualJournalCurrencyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp54) Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
                If Informations.IsNumeric(vParams(ACInstalmentStatus)) = False _
                                   And ToSafeString((vParams(ACInstalmentStatus))) <> "" Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
                If Informations.IsNumeric(vParams(ACCanEditInstalmentDueDate)) = False _
                                    And ToSafeString((vParams(ACCanEditInstalmentDueDate))) <> "" Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                If Informations.IsNumeric(vParams(ACEditInstalmentDateByNoOfDays)) = False _
                           And ToSafeString((vParams(ACEditInstalmentDateByNoOfDays))) <> "" Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                If Informations.IsNumeric(vParams(ACEditInstalmentDateByNoOfDays)) = False _
                         And ToSafeString((vParams(ACEditInstalmentDateByNoOfDays))) <> "" Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
                If Informations.IsNumeric(vParams(ACEditInstalmentDateByNoOfDays)) = False _
                         And ToSafeString((vParams(ACEditInstalmentDateByNoOfDays))) <> "" Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
                If Informations.IsNumeric(vParams(ACEditInstalmentDateByNoOfDays)) = False _
                         And ToSafeString((vParams(ACEditInstalmentDateByNoOfDays))) <> "" Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Validate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Validate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: AddKeyOutputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY OUTPUT parameters
    '              required for an Add.
    '
    ' ***************************************************************** '
    Private Function AddKeyOutputParam() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'developer guide no. 85(Guide)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddKeyOutputParam Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddKeyOutputParam", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Adds all of the NON-KEY INPUT parameters required for an Insert or Update
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AddInputParam() As Integer

        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oDatabase.Parameters.Add(sName:="has_write_off_authority",
                                               vValue:=CStr(m_iHasWriteOffAuthority),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="write_off_amount", vValue:=CStr(m_dWriteOffAmount),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="has_unrestricted_enquiry",
                                               vValue:=CStr(m_iUnrestrictedEnquiry),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="has_unrestricted_update",
                                               vValue:=CStr(m_iUnrestrictedUpdate),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="has_ViewBatchProcessStatus",
                                               vValue:=CStr(m_nViewbatchProcessStatus),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="can_extract_client_data",
                                               vValue:=CStr(m_nCanExtractClientData),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="fee_discount", vValue:=CStr(m_lFeeDiscount),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="has_transaction_write_off_authority",
                                               vValue:=CStr(m_iHasTransWriteOffAuthority),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_write_off_amount",
                                               vValue:=CStr(m_dTransWriteOffAmount),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="has_refund_authority",
                                               vValue:=CStr(m_iHasRefundAuthority),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="has_transfer_authority",
                                               vValue:=CStr(m_iHasTransferAuthority),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="has_Payments_authority",
                                               vValue:=CStr(m_iHasPaymentsAuthority),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="payments_amount", vValue:=CStr(m_dPaymentsAmount),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="has_claim_Payments_authority",
                                               vValue:=CStr(m_iHasClaimPaymentsAuthority),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_payments_amount",
                                               vValue:=CStr(m_dClaimPaymentsAmount),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="has_ManualJournal_authority",
                                               vValue:=CStr(m_iHasManualJournalAuthority),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ManualJournal_currency_amount",
                                               vValue:=CStr(m_dManualJournalAmount),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="can_change_exchange_date", vValue:=CStr(m_iOverrideDate),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="can_change_exchange_rate", vValue:=CStr(m_iOverrideRate),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Chek for value as nothing
            If Informations.IsNothing(m_iWriteOffCurrencyID) OrElse Convert.IsDBNull(m_iWriteOffCurrencyID) OrElse m_iWriteOffCurrencyID.Equals(0) Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="write_off_currency_id", vValue:=DBNull.Value,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMInteger)

            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="write_off_currency_id", vValue:=m_iWriteOffCurrencyID,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsNothing(m_iTransWriteOffCurrencyID) OrElse Convert.IsDBNull(m_iTransWriteOffCurrencyID) OrElse m_iTransWriteOffCurrencyID.Equals(0) Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_write_off_currency_id",
                                                   vValue:=DBNull.Value,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_write_off_currency_id",
                                                   vValue:=m_iTransWriteOffCurrencyID,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsNothing(m_iPaymentsCurrencyID) OrElse Convert.IsDBNull(m_iPaymentsCurrencyID) OrElse m_iPaymentsCurrencyID.Equals(0) Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="payments_currency_id", vValue:=DBNull.Value,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="payments_currency_id", vValue:=m_iPaymentsCurrencyID,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsNothing(m_iManualJournalCurrencyID) OrElse Convert.IsDBNull(m_iManualJournalCurrencyID) OrElse m_iPaymentsCurrencyID.Equals(0) Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="ManualJournal_currency_id", vValue:=DBNull.Value,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="ManualJournal_currency_id", vValue:=m_iManualJournalCurrencyID,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsNothing(m_iClaimPaymentsCurrencyID) OrElse Convert.IsDBNull(m_iClaimPaymentsCurrencyID) OrElse m_iClaimPaymentsCurrencyID.Equals(0) Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="claims_payments_currency_id", vValue:=DBNull.Value,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="claims_payments_currency_id",
                                                   vValue:=m_iClaimPaymentsCurrencyID,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="can_change_prepolicy_exchange_date",
                                               vValue:=CStr(m_iOverridePrePolicyDate),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="can_change_prepolicy_exchange_rate",
                                               vValue:=CStr(m_iOverridePrePolicyRate),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_view_client", vValue:=CStr(m_iIsViewClient),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_edit_client", vValue:=CStr(m_iIsEditClient),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_edit_policy", vValue:=CStr(m_iIsEditPolicy),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_edit_claim", vValue:=CStr(m_iIsEditClaim),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_edit_finance_plan", vValue:=CStr(m_iIsEditFinancePlan),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_raise_debit", vValue:=CStr(m_iIsRaiseDebit),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_raise_credit", vValue:=CStr(m_iIsRaiseCredit),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_raise_fee", vValue:=CStr(m_iIsRaiseFee),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_raise_cash", vValue:=CStr(m_iIsRaiseCash),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_reverse_transactions",
                                               vValue:=CStr(m_iIsReverseTransactions),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="can_reverse_and_replace_transactions",
                                               vValue:=CStr(m_iCanReverseReplaceTransactions),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_reverse_allocations",
                                               vValue:=CStr(m_iIsReverseAllocations),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_raise_manual_DID", vValue:=CStr(m_iIsRaiseManualDID),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_delete_client", vValue:=CStr(m_iIsDeleteClient),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_perform_allocations",
                                               vValue:=CStr(m_iIsPerformAllocations),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="can_perform_broker_transfer",
                                               vValue:=CStr(m_iCanPerformBrokerTransfer),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="can_override_duplicate_claims",
                                               vValue:=CStr(m_iDuplicateClaimOverride),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_delete_policy", vValue:=CStr(m_iIsDeletePolicy),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_edit_scheme_policy",
                                               vValue:=CStr(m_iIsEditSchemePolicy),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="can_make_live_invoice",
                                               vValue:=CStr(m_iCanMakeLiveInvoice),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="can_make_live_instalments",
                                               vValue:=CStr(m_iCanMakeLiveInstalments),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="can_make_live_paynow", vValue:=CStr(m_iCanMakeLivePaynow),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="has_paynow_write_off_authority",
                                               vValue:=CStr(m_iHasPaynowWriteoffAuthority),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="paynow_write_off_currency_id",
                                               vValue:=gPMFunctions.ZeroToNull(CStr(m_iPaynowWriteoffCurrencyID)),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="paynow_write_off_amount",
                                               vValue:=CStr(m_cPaynowWriteOffAmount),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="paynow_bankaccount_id",
                                               vValue:=gPMFunctions.ZeroToNull(CStr(m_iPaynowBankAccount)),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="can_override_posting_period",
                                               vValue:=CStr(m_iPostingPeriod),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="can_override_cheque_Numbers",
                                               vValue:=gPMFunctions.ZeroToNull(CStr(m_iOverrideChequeNumber)),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="can_change_reserves_on_claim_payments",
                                               vValue:=CStr(m_iUserCanChangeReserves),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="allow_ratingsection_adddelete",
                                               vValue:=CStr(m_iUserCanAddRemoveRatingSections),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="allow_ratingsection_editing",
                                               vValue:=CStr(m_iUserCanEditExistingRatingSections),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Party View
            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_view_only_client_manager",
                                               vValue:=CStr(m_iViewClientManager),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_view_only_agents_maintenance",
                                               vValue:=CStr(m_iAgentMaintenance),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_view_only_account_handler_maintenance",
                                               vValue:=CStr(m_iAccountHandler),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_view_only_account_executive_maintenance",
                                               vValue:=CStr(m_iAccountExecutive),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_view_only_insurer_maintenance",
                                               vValue:=CStr(m_iInsurerMaintenance),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_view_only_other_party_maintenance",
                                               vValue:=CStr(m_iOtherPartyMaintenance),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_recommender", vValue:=CStr(m_iIsRecommender),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="recommender_currency_id",
                                               vValue:=CStr(m_iRecommenderCurrencyID),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="recommender_currency_amount",
                                               vValue:=CStr(m_cRecommenderCurrencyAmt),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Payment Maintenance
            m_lReturn = m_oDatabase.Parameters.Add(sName:="allow_reverse_allocations",
                                               vValue:=gPMFunctions.ZeroToNull(CStr(m_iCanReverseAllocations)),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="reverse_allocations_days",
                                               vValue:=gPMFunctions.ZeroToNull(CStr(m_iTimePeriodForReversal)),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="MTA_Authority",
                                               vValue:=gPMFunctions.ZeroToNull(CStr(m_iMTAAuthority)),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="display_reinsurance",
                                               vValue:=CStr(m_iDisplayReinsuranceScreen),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="can_change_instalment_default_currency", vValue:=CStr(m_iCanChangeInstalmentDefaultCurrency), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="display_claim_reinsurance",
                                               vValue:=CStr(m_iDisplayClaimReinsurance),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Edit_Default_Commission",
                                               vValue:=CStr(m_iEditDefaultCommission),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="can_backdate_collection_date",
                                               vValue:=CStr(m_iCanBackdateCollectionDate),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="can_make_live_bankguarantee",
                                               vValue:=CStr(m_iCanMakeLiveBankGuarantee),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="can_make_live_CashDeposit",
                                               vValue:=CStr(m_iCanMakeLiveCashDeposit),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="can_user_debug_dynamic_logic_scripts",
                                               vValue:=m_iACUserCanDebugDynamicLogicScripts,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_server_scripts_run_in_debug",
                                               vValue:=m_iACUserServerScriptsRunInDebug,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Edit_Default_Commission_NB_RN",
                                               vValue:=m_nEditDefaultCommissionNBRN,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                AddInputParam = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Edit_Default_Commission_MTA",
                                               vValue:=m_nEditDefaultCommissionMTA,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                AddInputParam = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Edit_Default_Commission_MTC",
                                               vValue:=m_nEditDefaultCommissionMTC,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                AddInputParam = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Edit_Default_Commission_MTR",
                                               vValue:=m_nEditDefaultCommissionMTR,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                AddInputParam = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Agent_Editable_During_MTA_MTC",
                                               vValue:=m_nEditAgentDuringMTAMTC,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                AddInputParam = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ''Receipt Reversal
            m_lReturn = m_oDatabase.Parameters.Add(sName:="allow_receipt_reversal",
                                              vValue:=gPMFunctions.ZeroToNull(CStr(m_nAllowReceiptReversal)),
                                              iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                              iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ''Can update Instalment status
            m_lReturn = m_oDatabase.Parameters.Add(sName:="can_update_instalment_status", vValue:=m_nInstalmentStatus,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="can_edit_instalment_date", vValue:=m_nCanEditInstalmentDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="edit_instalment_by_no_of_days", vValue:=m_nEditInstalmentDateByNoOfDays, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="currency_loss_gain_limit", vValue:=CStr(m_dCurrencyLossGainLimit),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsNothing(m_iLossGainCurrencyID) OrElse Convert.IsDBNull(m_iLossGainCurrencyID) OrElse m_iLossGainCurrencyID.Equals(0) Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="loss_gain_currency_id", vValue:=DBNull.Value,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMInteger)

            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="loss_gain_currency_id", vValue:=m_iLossGainCurrencyID,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="modified_by", vValue:=CInt(m_iModifiedByUserId),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="unique_id", vValue:=CStr(m_sUniqueId),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="screen_hierarchy", vValue:=CStr(m_sScreenHierarchy),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
			
			 m_lReturn = m_oDatabase.Parameters.Add(sName:="void_policy_version", vValue:=ToSafeString(m_VoidTransaction),
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult
        Catch excep As System.Exception



            nResult = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddInputParam Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddInputParam", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return nResult

        End Try


    End Function

    ' ***************************************************************** '
    ' Name: GetNewPrimaryKeyID (Private)
    '
    ' Description: Returns the new PRIMARY KEY values from an Add.
    '
    ' ***************************************************************** '
    Private Function GetNewPrimaryKeyID() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                m_iUAUserID = .Parameters.Item("user_id").Value

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNewPrimaryKeyID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNewPrimaryKeyID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddKeyInputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY INPUT parameters
    '              required for a Select, Delete or Update.
    '
    ' ***************************************************************** '
    Private Function AddKeyInputParam() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(UserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddKeyInputParam Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddKeyInputParam", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Sets the supplied ACTUserAuthorities properties from a database record
    ''' </summary>
    ''' <param name="oFields"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetPropertiesFromDB(ByRef oFields As DataRow) As Integer

        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Populate Base Details
            m_iUAUserID = oFields("user_id")


            If Convert.IsDBNull(oFields("has_write_off_authority")) OrElse Informations.IsNothing(oFields("has_write_off_authority")) Then

                m_iHasWriteOffAuthority = Nothing
            Else
                m_iHasWriteOffAuthority = oFields("has_write_off_authority")
            End If


            If Convert.IsDBNull(oFields("write_off_amount")) OrElse Informations.IsNothing(oFields("write_off_amount")) Then

                m_dWriteOffAmount = Nothing
            Else
                m_dWriteOffAmount = oFields("write_off_amount")
            End If


            If Convert.IsDBNull(oFields("has_unrestricted_enquiry")) OrElse Informations.IsNothing(oFields("has_unrestricted_enquiry")) Then
                m_iUnrestrictedEnquiry = 0
            Else
                m_iUnrestrictedEnquiry = oFields("has_unrestricted_enquiry")
            End If


            If Convert.IsDBNull(oFields("has_unrestricted_update")) OrElse Informations.IsNothing(oFields("has_unrestricted_update")) Then
                m_iUnrestrictedUpdate = 0
            Else
                m_iUnrestrictedUpdate = oFields("has_unrestricted_update")
            End If

            If Convert.IsDBNull(oFields("has_ViewBatchProcessStatus")) OrElse Informations.IsNothing(oFields("has_ViewBatchProcessStatus")) Then
                m_nViewbatchProcessStatus = 0
            Else
                m_nViewbatchProcessStatus = oFields("has_ViewBatchProcessStatus")
            End If

            If oFields.Table.Columns.Contains("can_extract_client_data") AndAlso Not Convert.IsDBNull(oFields("can_extract_client_data")) AndAlso Not Informations.IsNothing(oFields("can_extract_client_data")) Then
                m_nCanExtractClientData = oFields("can_extract_client_data")
            Else
                m_nCanExtractClientData = 0
            End If

            If Convert.IsDBNull(oFields("fee_discount")) OrElse Informations.IsNothing(oFields("fee_discount")) Then
                m_lFeeDiscount = 0
            Else
                m_lFeeDiscount = oFields("fee_discount")
            End If


            If Convert.IsDBNull(oFields("has_transaction_write_off_authority")) OrElse Informations.IsNothing(oFields("has_transaction_write_off_authority")) Then
                m_iHasTransWriteOffAuthority = 0
            Else
                m_iHasTransWriteOffAuthority = oFields("has_transaction_write_off_authority")
            End If


            If Convert.IsDBNull(oFields("transaction_write_off_amount")) OrElse Informations.IsNothing(oFields("transaction_write_off_amount")) Then
                m_dTransWriteOffAmount = 0
            Else
                m_dTransWriteOffAmount = oFields("transaction_write_off_amount")
            End If


            If Convert.IsDBNull(oFields("has_refund_authority")) OrElse Informations.IsNothing(oFields("has_refund_authority")) Then
                m_iHasRefundAuthority = 0
            Else
                m_iHasRefundAuthority = oFields("has_refund_authority")
            End If


            If Convert.IsDBNull(oFields("has_transfer_authority")) OrElse Informations.IsNothing(oFields("has_transfer_authority")) Then
                m_iHasTransferAuthority = 0
            Else
                m_iHasTransferAuthority = oFields("has_transfer_authority")
            End If


            If Convert.IsDBNull(oFields("has_Payments_authority")) OrElse Informations.IsNothing(oFields("has_Payments_authority")) Then
                m_iHasPaymentsAuthority = 0
            Else
                m_iHasPaymentsAuthority = oFields("has_Payments_authority")
            End If


            If Convert.IsDBNull(oFields("payments_amount")) OrElse Informations.IsNothing(oFields("payments_amount")) Then
                m_dPaymentsAmount = 0
            Else
                m_dPaymentsAmount = oFields("payments_amount")
            End If

            If Convert.IsDBNull(oFields("has_ManualJournal_authority")) OrElse Informations.IsNothing(oFields("has_ManualJournal_authority")) Then
                m_iHasManualJournalAuthority = 0
            Else
                m_iHasManualJournalAuthority = oFields("has_ManualJournal_authority")
            End If


            If Convert.IsDBNull(oFields("ManualJournal_currency_amount")) OrElse Informations.IsNothing(oFields("ManualJournal_currency_amount")) Then
                m_dManualJournalAmount = 0
            Else
                m_dManualJournalAmount = oFields("ManualJournal_currency_amount")
            End If



            If Convert.IsDBNull(oFields("has_claim_Payments_authority")) OrElse Informations.IsNothing(oFields("has_claim_Payments_authority")) Then
                m_iHasClaimPaymentsAuthority = 0
            Else
                m_iHasClaimPaymentsAuthority = oFields("has_claim_Payments_authority")
            End If


            If Convert.IsDBNull(oFields("claim_payments_amount")) OrElse Informations.IsNothing(oFields("claim_payments_amount")) Then
                m_dClaimPaymentsAmount = 0
            Else
                m_dClaimPaymentsAmount = oFields("claim_payments_amount")
            End If


            If Convert.IsDBNull(oFields("can_change_exchange_date")) OrElse Informations.IsNothing(oFields("can_change_exchange_date")) Then
                m_iOverrideDate = 0
            Else
                m_iOverrideDate = oFields("can_change_exchange_date")
            End If


            If Convert.IsDBNull(oFields("can_change_exchange_rate")) OrElse Informations.IsNothing(oFields("can_change_exchange_rate")) Then
                m_iOverrideRate = 0
            Else
                m_iOverrideRate = oFields("can_change_exchange_rate")
            End If


            If Convert.IsDBNull(oFields("can_change_prepolicy_exchange_date")) OrElse Informations.IsNothing(oFields("can_change_prepolicy_exchange_date")) Then
                m_iOverridePrePolicyDate = 0
            Else
                m_iOverridePrePolicyDate = oFields("can_change_prepolicy_exchange_date")
            End If


            If Convert.IsDBNull(oFields("can_change_prepolicy_exchange_rate")) OrElse Informations.IsNothing(oFields("can_change_prepolicy_exchange_rate")) Then
                m_iOverridePrePolicyRate = 0
            Else
                m_iOverridePrePolicyRate = oFields("can_change_prepolicy_exchange_rate")
            End If


            If Convert.IsDBNull(oFields("write_off_currency_id")) OrElse Informations.IsNothing(oFields("write_off_currency_id")) Then
                m_iWriteOffCurrencyID = 0
            Else
                m_iWriteOffCurrencyID = oFields("write_off_currency_id")
            End If


            If Convert.IsDBNull(oFields("transaction_write_off_currency_id")) OrElse Informations.IsNothing(oFields("transaction_write_off_currency_id")) Then
                m_iTransWriteOffCurrencyID = 0
            Else
                m_iTransWriteOffCurrencyID = oFields("transaction_write_off_currency_id")
            End If


            If Convert.IsDBNull(oFields("payments_currency_id")) OrElse Informations.IsNothing(oFields("payments_currency_id")) Then
                m_iPaymentsCurrencyID = 0
            Else
                m_iPaymentsCurrencyID = oFields("payments_currency_id")
            End If

            If Convert.IsDBNull(oFields("ManualJournal_currency_id")) OrElse Informations.IsNothing(oFields("ManualJournal_currency_id")) Then
                m_iManualJournalCurrencyID = 0
            Else
                m_iManualJournalCurrencyID = oFields("ManualJournal_currency_id")
            End If

            If Convert.IsDBNull(oFields("claims_payments_currency_id")) OrElse Informations.IsNothing(oFields("claims_payments_currency_id")) Then
                m_iClaimPaymentsCurrencyID = 0
            Else
                m_iClaimPaymentsCurrencyID = oFields("claims_payments_currency_id")
            End If

            If Convert.IsDBNull(oFields("is_view_client")) OrElse Informations.IsNothing(oFields("is_view_client")) Then
                m_iIsViewClient = 0
            Else
                m_iIsViewClient = oFields("is_view_client")
            End If


            If Convert.IsDBNull(oFields("is_edit_client")) OrElse Informations.IsNothing(oFields("is_edit_client")) Then
                m_iIsEditClient = 0
            Else
                m_iIsEditClient = oFields("is_edit_client")
            End If


            If Convert.IsDBNull(oFields("is_edit_policy")) OrElse Informations.IsNothing(oFields("is_edit_policy")) Then
                m_iIsEditPolicy = 0
            Else
                m_iIsEditPolicy = oFields("is_edit_policy")
            End If

            If Convert.IsDBNull(oFields("is_edit_finance_plan")) OrElse Informations.IsNothing(oFields("is_edit_finance_plan")) Then
                m_iIsEditFinancePlan = 0
            Else
                m_iIsEditFinancePlan = oFields("is_edit_finance_plan")
            End If


            If Convert.IsDBNull(oFields("is_edit_claim")) OrElse Informations.IsNothing(oFields("is_edit_claim")) Then
                m_iIsEditClaim = 0
            Else
                m_iIsEditClaim = oFields("is_edit_claim")
            End If


            If Convert.IsDBNull(oFields("is_raise_debit")) OrElse Informations.IsNothing(oFields("is_raise_debit")) Then
                m_iIsRaiseDebit = 0
            Else
                m_iIsRaiseDebit = oFields("is_raise_debit")
            End If


            If Convert.IsDBNull(oFields("is_raise_credit")) OrElse Informations.IsNothing(oFields("is_raise_credit")) Then
                m_iIsRaiseCredit = 0
            Else
                m_iIsRaiseCredit = oFields("is_raise_credit")
            End If


            If Convert.IsDBNull(oFields("is_raise_fee")) OrElse Informations.IsNothing(oFields("is_raise_fee")) Then
                m_iIsRaiseFee = 0
            Else
                m_iIsRaiseFee = oFields("is_raise_fee")
            End If


            If Convert.IsDBNull(oFields("is_raise_cash")) OrElse Informations.IsNothing(oFields("is_raise_cash")) Then
                m_iIsRaiseCash = 0
            Else
                m_iIsRaiseCash = oFields("is_raise_cash")
            End If


            If Convert.IsDBNull(oFields("is_reverse_transactions")) OrElse Informations.IsNothing(oFields("is_reverse_transactions")) Then
                m_iIsReverseTransactions = 0
            Else
                m_iIsReverseTransactions = oFields("is_reverse_transactions")
            End If


            If Convert.IsDBNull(oFields("can_reverse_and_replace_transactions")) OrElse Informations.IsNothing(oFields("can_reverse_and_replace_transactions")) Then
                m_iCanReverseReplaceTransactions = 0
            Else
                m_iCanReverseReplaceTransactions = oFields("can_reverse_and_replace_transactions")
            End If


            If Convert.IsDBNull(oFields("is_reverse_allocations")) OrElse Informations.IsNothing(oFields("is_reverse_allocations")) Then
                m_iIsReverseAllocations = 0
            Else
                m_iIsReverseAllocations = oFields("is_reverse_allocations")
            End If


            If Convert.IsDBNull(oFields("is_raise_manual_DID")) OrElse Informations.IsNothing(oFields("is_raise_manual_DID")) Then
                m_iIsRaiseManualDID = 0
            Else
                m_iIsRaiseManualDID = oFields("is_raise_manual_DID")
            End If

            If Convert.IsDBNull(oFields("is_delete_client")) OrElse Informations.IsNothing(oFields("is_delete_client")) Then
                m_iIsDeleteClient = 0
            Else
                m_iIsDeleteClient = oFields("is_delete_client")
            End If

            If Convert.IsDBNull(oFields("is_perform_allocations")) OrElse Informations.IsNothing(oFields("is_perform_allocations")) Then
                m_iIsPerformAllocations = 0
            Else
                m_iIsPerformAllocations = oFields("is_perform_allocations")
            End If


            If Convert.IsDBNull(oFields("can_override_duplicate_claims")) OrElse Informations.IsNothing(oFields("can_override_duplicate_claims")) Then
                m_iDuplicateClaimOverride = 0
            Else
                m_iDuplicateClaimOverride = oFields("can_override_duplicate_claims")
            End If

            m_iCanPerformBrokerTransfer = gPMFunctions.ToSafeInteger(oFields("can_perform_broker_transfer"), 0)


            If Convert.IsDBNull(oFields("is_delete_policy")) OrElse Informations.IsNothing(oFields("is_delete_policy")) Then
                m_iIsDeletePolicy = 0
            Else
                m_iIsDeletePolicy = oFields("is_delete_policy")
            End If


            If Convert.IsDBNull(oFields("is_edit_scheme_policy")) OrElse Informations.IsNothing(oFields("is_edit_scheme_policy")) Then
                m_iIsEditSchemePolicy = 0
            Else
                m_iIsEditSchemePolicy = oFields("is_edit_scheme_policy")
            End If


            If Convert.IsDBNull(oFields("can_make_live_invoice")) OrElse Informations.IsNothing(oFields("can_make_live_invoice")) Then
                m_iCanMakeLiveInvoice = 1
            Else
                m_iCanMakeLiveInvoice = oFields("can_make_live_invoice")
            End If


            If Convert.IsDBNull(oFields("can_make_live_instalments")) OrElse Informations.IsNothing(oFields("can_make_live_instalments")) Then
                m_iCanMakeLiveInstalments = 1
            Else
                m_iCanMakeLiveInstalments = oFields("can_make_live_instalments")
            End If


            If Convert.IsDBNull(oFields("can_make_live_Paynow")) OrElse Informations.IsNothing(oFields("can_make_live_Paynow")) Then
                m_iCanMakeLivePaynow = 1
            Else
                m_iCanMakeLivePaynow = oFields("can_make_live_Paynow")
            End If


            If Convert.IsDBNull(oFields("Has_Paynow_write_off_Authority")) OrElse Informations.IsNothing(oFields("Has_Paynow_write_off_Authority")) Then
                m_iHasPaynowWriteoffAuthority = 1
            Else
                m_iHasPaynowWriteoffAuthority = oFields("Has_Paynow_write_off_Authority")
            End If


            If Convert.IsDBNull(oFields("Paynow_write_off_Currency_id")) OrElse Informations.IsNothing(oFields("Paynow_write_off_Currency_id")) Then
                m_iPaynowWriteoffCurrencyID = 1
            Else
                m_iPaynowWriteoffCurrencyID = oFields("Paynow_write_off_Currency_id")
            End If


            If Convert.IsDBNull(oFields("Paynow_write_off_amount")) OrElse Informations.IsNothing(oFields("Paynow_write_off_amount")) Then
                m_cPaynowWriteOffAmount = 1
            Else
                m_cPaynowWriteOffAmount = oFields("Paynow_write_off_amount")
            End If


            If Convert.IsDBNull(oFields("Paynow_BankAccount_id")) OrElse Informations.IsNothing(oFields("Paynow_BankAccount_id")) Then
                m_iPaynowBankAccount = 1
            Else
                m_iPaynowBankAccount = oFields("Paynow_BankAccount_id")
            End If

            If Convert.IsDBNull(oFields("can_override_posting_period")) OrElse Informations.IsNothing(oFields("can_override_posting_period")) Then
                m_iPostingPeriod = 0
            Else
                m_iPostingPeriod = oFields("can_override_posting_period")
            End If

            If Convert.IsDBNull(oFields("can_change_reserves_on_claim_payments")) OrElse Informations.IsNothing(oFields("can_change_reserves_on_claim_payments")) Then
                m_iUserCanChangeReserves = 0
            Else
                m_iUserCanChangeReserves = oFields("can_change_reserves_on_claim_payments")
            End If


            If Convert.IsDBNull(oFields("allow_ratingsection_adddelete")) OrElse Informations.IsNothing(oFields("allow_ratingsection_adddelete")) Then
                m_iUserCanAddRemoveRatingSections = 0
            Else
                m_iUserCanAddRemoveRatingSections = oFields("allow_ratingsection_adddelete")
            End If


            If Convert.IsDBNull(oFields("allow_ratingsection_editing")) OrElse Informations.IsNothing(oFields("allow_ratingsection_editing")) Then
                m_iUserCanEditExistingRatingSections = 0
            Else
                m_iUserCanEditExistingRatingSections = oFields("allow_ratingsection_editing")
            End If

            'Party View

            If Convert.IsDBNull(oFields("is_view_only_client_manager")) OrElse Informations.IsNothing(oFields("is_view_only_client_manager")) Then
                m_iViewClientManager = 0
            Else
                m_iViewClientManager = oFields("is_view_only_client_manager")
            End If


            If Convert.IsDBNull(oFields("is_view_only_agents_maintenance")) OrElse Informations.IsNothing(oFields("is_view_only_agents_maintenance")) Then
                m_iAgentMaintenance = 0
            Else
                m_iAgentMaintenance = oFields("is_view_only_agents_maintenance")
            End If


            If Convert.IsDBNull(oFields("is_view_only_account_handler_maintenance")) OrElse Informations.IsNothing(oFields("is_view_only_account_handler_maintenance")) Then
                m_iAccountHandler = 0
            Else
                m_iAccountHandler = oFields("is_view_only_account_handler_maintenance")
            End If


            If Convert.IsDBNull(oFields("is_view_only_account_executive_maintenance")) OrElse Informations.IsNothing(oFields("is_view_only_account_executive_maintenance")) Then
                m_iAccountExecutive = 0
            Else
                m_iAccountExecutive = oFields("is_view_only_account_executive_maintenance")
            End If


            If Convert.IsDBNull(oFields("is_view_only_insurer_maintenance")) OrElse Informations.IsNothing(oFields("is_view_only_insurer_maintenance")) Then
                m_iInsurerMaintenance = 0
            Else
                m_iInsurerMaintenance = oFields("is_view_only_insurer_maintenance")
            End If


            If Convert.IsDBNull(oFields("is_view_only_other_party_maintenance")) Or
                Informations.IsNothing(oFields("is_view_only_other_party_maintenance")) Then
                m_iOtherPartyMaintenance = 0
            Else
                m_iOtherPartyMaintenance = oFields("is_view_only_other_party_maintenance")
            End If


            If Convert.IsDBNull(oFields("is_recommender")) Or Informations.IsNothing(oFields("is_recommender")) Then
                m_iIsRecommender = False
            Else
                m_iIsRecommender = gPMFunctions.ToSafeInteger(oFields("is_recommender"))
            End If


            If Convert.IsDBNull(oFields("recommender_currency_id")) Or Informations.IsNothing(oFields("recommender_currency_id")) Then
                m_iRecommenderCurrencyID = 0
            Else
                m_iRecommenderCurrencyID = gPMFunctions.ToSafeInteger(oFields("recommender_currency_id"))
            End If


            If Convert.IsDBNull(oFields("recommender_currency_amount")) Or
                Informations.IsNothing(oFields("recommender_currency_amount")) Then
                m_cRecommenderCurrencyAmt = False
            Else
                m_cRecommenderCurrencyAmt = gPMFunctions.ToSafeCurrency(oFields("recommender_currency_amount"))
            End If

            'Payment Maintenance

            If Convert.IsDBNull(oFields("allow_reverse_allocations")) Or Informations.IsNothing(oFields("allow_reverse_allocations")) Then
                m_iCanReverseAllocations = 0
            Else
                m_iCanReverseAllocations = gPMFunctions.ToSafeInteger(oFields("allow_reverse_allocations"))
            End If


            If Convert.IsDBNull(oFields("reverse_allocations_days")) Or Informations.IsNothing(oFields("reverse_allocations_days")) Then
                m_iTimePeriodForReversal = 0
            Else
                m_iTimePeriodForReversal = gPMFunctions.ToSafeInteger(oFields("reverse_allocations_days"))
            End If


            If Convert.IsDBNull(oFields("out_of_sequence_mta_authority")) Or
                Informations.IsNothing(oFields("out_of_sequence_mta_authority")) Then
                m_iMTAAuthority = 0
            Else
                m_iMTAAuthority = gPMFunctions.ToSafeInteger(oFields("out_of_sequence_mta_authority"))
            End If


            If Convert.IsDBNull(oFields("can_override_cheque_Numbers")) Or
                Informations.IsNothing(oFields("can_override_cheque_Numbers")) Then
                m_iOverrideChequeNumber = 0
            Else
                m_iOverrideChequeNumber = gPMFunctions.ToSafeInteger(oFields("can_override_cheque_Numbers"))
            End If

            If Convert.IsDBNull(oFields("display_reinsurance")) Or Informations.IsNothing(oFields("display_reinsurance")) Then
                m_iDisplayReinsuranceScreen = 0
            Else
                m_iDisplayReinsuranceScreen = gPMFunctions.ToSafeInteger(oFields("display_reinsurance"))
            End If


            If Convert.IsDBNull(oFields("display_claim_reinsurance")) Or Informations.IsNothing(oFields("display_claim_reinsurance")) Then
                m_iDisplayClaimReinsurance = 0
            Else
                m_iDisplayClaimReinsurance = gPMFunctions.ToSafeInteger(oFields("display_claim_reinsurance"))
            End If

            If Convert.IsDBNull(oFields("Edit_Default_Commission")) Or Informations.IsNothing(oFields("Edit_Default_Commission")) Then
                m_iEditDefaultCommission = 0
            Else
                m_iEditDefaultCommission = gPMFunctions.ToSafeInteger(oFields("Edit_Default_Commission"))
            End If

            If Convert.IsDBNull(oFields("can_change_instalment_default_currency")) Or Informations.IsNothing(oFields("can_change_instalment_default_currency")) Then
                m_iCanChangeInstalmentDefaultCurrency = 0
            Else
                m_iCanChangeInstalmentDefaultCurrency = gPMFunctions.ToSafeInteger(oFields("can_change_instalment_default_currency"))
            End If

            If Convert.IsDBNull(oFields("can_make_live_bankguarantee")) Or
                Informations.IsNothing(oFields("can_make_live_bankguarantee")) Then
                m_iCanMakeLiveBankGuarantee = 1
            Else
                m_iCanMakeLiveBankGuarantee = oFields("can_make_live_bankguarantee")
            End If


            If Convert.IsDBNull(oFields("can_backdate_collection_date")) Or
                Informations.IsNothing(oFields("can_backdate_collection_date")) Then
                m_iCanBackdateCollectionDate = 0
            Else
                m_iCanBackdateCollectionDate = oFields("can_backdate_collection_date")
            End If

            If Convert.IsDBNull(oFields("can_make_live_cashdeposit")) Or Informations.IsNothing(oFields("can_make_live_cashdeposit")) Then
                m_iCanMakeLiveCashDeposit = 0
            Else
                m_iCanMakeLiveCashDeposit = oFields("can_make_live_cashdeposit")
            End If

            If Convert.IsDBNull(oFields("can_user_debug_dynamic_logic_scripts")) Or
                Informations.IsNothing(oFields("can_user_debug_dynamic_logic_scripts")) Then
                m_iACUserCanDebugDynamicLogicScripts = 0
            Else
                m_iACUserCanDebugDynamicLogicScripts = oFields("can_user_debug_dynamic_logic_scripts")
            End If

            If Convert.IsDBNull(oFields("user_server_scripts_run_in_debug")) Or
                Informations.IsNothing(oFields("user_server_scripts_run_in_debug")) Then
                m_iACUserServerScriptsRunInDebug = 0
            Else
                m_iACUserServerScriptsRunInDebug = oFields("user_server_scripts_run_in_debug")
            End If

            If Convert.IsDBNull(oFields("Edit_Default_Commission_NB_RN")) OrElse Informations.IsNothing(oFields("Edit_Default_Commission_NB_RN")) Then
                m_nEditDefaultCommissionNBRN = 0
            Else
                m_nEditDefaultCommissionNBRN = gPMFunctions.ToSafeInteger(oFields("Edit_Default_Commission_NB_RN"))
            End If

            If Convert.IsDBNull(oFields("Edit_Default_Commission_MTA")) OrElse Informations.IsNothing(oFields("Edit_Default_Commission_MTA")) Then
                m_nEditDefaultCommissionMTA = 0
            Else
                m_nEditDefaultCommissionMTA = gPMFunctions.ToSafeInteger(oFields("Edit_Default_Commission_MTA"))
            End If

            If Convert.IsDBNull(oFields("Edit_Default_Commission_MTC")) OrElse Informations.IsNothing(oFields("Edit_Default_Commission_MTC")) Then
                m_nEditDefaultCommissionMTC = 0
            Else
                m_nEditDefaultCommissionMTC = gPMFunctions.ToSafeInteger(oFields("Edit_Default_Commission_MTC"))
            End If

            If Convert.IsDBNull(oFields("Edit_Default_Commission_MTR")) OrElse Informations.IsNothing(oFields("Edit_Default_Commission_MTR")) Then
                m_nEditDefaultCommissionMTR = 0
            Else
                m_nEditDefaultCommissionMTR = gPMFunctions.ToSafeInteger(oFields("Edit_Default_Commission_MTR"))
            End If

            If Convert.IsDBNull(oFields("Agent_Editable_During_MTA_MTC")) OrElse Informations.IsNothing(oFields("Agent_Editable_During_MTA_MTC")) Then
                m_nEditAgentDuringMTAMTC = 0
            Else
                m_nEditAgentDuringMTAMTC = gPMFunctions.ToSafeInteger(oFields("Agent_Editable_During_MTA_MTC"))
            End If

            ''Receipt Reversal
            If Convert.IsDBNull(oFields("allow_receipt_reversal")) Or Informations.IsNothing(oFields("allow_receipt_reversal")) Then
                m_nAllowReceiptReversal = 0
            Else
                m_nAllowReceiptReversal = gPMFunctions.ToSafeInteger(oFields("allow_receipt_reversal"))
            End If

            ''can update instalment status
            If Convert.IsDBNull(oFields("can_update_instalment_status")) Or Informations.IsNothing(oFields("can_update_instalment_status")) Then
                m_nInstalmentStatus = 0
            Else
                m_nInstalmentStatus = oFields("can_update_instalment_status")
            End If
            If Convert.IsDBNull(oFields("can_edit_instalment_date")) Or Informations.IsNothing(oFields("can_edit_instalment_date")) Then
                m_nCanEditInstalmentDate = 0
            Else
                m_nCanEditInstalmentDate = oFields("can_edit_instalment_date")
            End If

            If Convert.IsDBNull(oFields("edit_instalment_by_no_of_days")) Or Informations.IsNothing(oFields("edit_instalment_by_no_of_days")) Then
                m_nEditInstalmentDateByNoOfDays = Nothing
            Else
                m_nEditInstalmentDateByNoOfDays = oFields("edit_instalment_by_no_of_days")
            End If

            If Convert.IsDBNull(oFields("currency_loss_gain_limit")) OrElse Informations.IsNothing(oFields("currency_loss_gain_limit")) Then

                m_dCurrencyLossGainLimit = Nothing
            Else
                m_dCurrencyLossGainLimit = oFields("currency_loss_gain_limit")
            End If

            If Convert.IsDBNull(oFields("loss_gain_currency_id")) OrElse Informations.IsNothing(oFields("loss_gain_currency_id")) Then
                m_iLossGainCurrencyID = 0
            Else
                m_iLossGainCurrencyID = oFields("loss_gain_currency_id")
            End If
			
			 If Convert.IsDBNull(oFields("void_policy_version")) OrElse IsNothing(oFields("void_policy_version")) Then
                m_VoidTransaction = ""
            Else
                m_VoidTransaction = oFields("void_policy_version")
            End If

            Return nResult

        Catch excep As System.Exception


            nResult = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetPropertiesFromDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPropertiesFromDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function
End Class

