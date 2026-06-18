Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.Globalization
Imports System.IO
Imports System.Windows.Forms
'developer guide no 129. 
Imports SharedFiles

Module MainModule
	'******************************************************************************
	' Module Name:      MainModule
	' History:          Created 22 Aug 2000
	' Description:      Main module containing public variable/constants.
	'******************************************************************************
	
	' Main public constant for all functions to identify which application this is
	Public Const ACApp As String = "uctCLMPaymentControl"
	
	' Public contants used for the start and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	Public Const ACReserveFrame As Integer = 125
	Public Const ACEditButton As Integer = 205
	
	'Constants used for resizing & positioning the user control
	Public Const ACListViewTop As Integer = 240
	Public Const ACCtrlVerticalSpacing As Integer = 120
	Public Const ACCommandButtonWidth As Integer = 1095
	Public Const ACCommandButtonHeight As Integer = 330
	
	' Public constants declared for the fields in the ReserveType Array
	Public Const g_cIRTADescription As Integer = 0
	
	' Public constants declared for the Reserve Details Array
	Public Const g_cIRDAreserveid As Integer = 0
	Public Const g_cIRDAinitialreserve As Integer = 1
	Public Const g_cIRDApaidtodate As Integer = 2
	Public Const g_cIRDArevisedreserve As Integer = 3
	Public Const g_cIRDAsuminsured As Integer = 4
	Public Const g_cIRDAaverage As Integer = 5
	Public Const g_cIRDArevisioncount As Integer = 6
	Public Const g_cIRDAreservetype As Integer = 7
	Public Const g_cIRDArevisedentered As Integer = 8
	
	' Public constants declared for the Recovery Details
	Public Const g_cIRecoveryDAinitialreserve As Integer = 0
	Public Const g_cIRecoveryDArevisedreserve As Integer = 1
	Public Const g_cIRecoveryDApaidtodate As Integer = 2
	
	' Public constants declared for the Payment Details Array
	Public Const g_cIPDApaymentid As Integer = 0
	Public Const g_cIPDAamount As Integer = 1
	Public Const g_cIPDAPartyID As Integer = 2
	Public Const g_cIPDAComments As Integer = 3
	Public Const g_cIPDATaxAmount As Integer = 4
	Public Const g_cIPDATaxTypeCode As Integer = 5
	Public Const g_cIPDAPayeeName As Integer = 6
	Public Const g_cIPDAPayeeBankName As Integer = 7
	Public Const g_cIPDAPayeeSortCode As Integer = 8
	Public Const g_cIPDAPayeeAccountNo As Integer = 9
	Public Const g_cIPDAPayeeCountry As Integer = 10
	Public Const g_cIPDAPayeeComments As Integer = 11
	Public Const g_cIPDAPaymentCurrencyID As Integer = 12
	Public Const g_cIPDAPaymentLossRate As Integer = 13
	
	' Invalid Data
	Public Const ACInvalidDataTitle As Integer = 310
	Public Const ACInvalidIntegerData As Integer = 311
	Public Const ACInvalidDateData As Integer = 314
	Public Const ACInvalidReserveDataTitle As Integer = 315
	Public Const ACInvalidReserveData As Integer = 316
	
	Public Const ACOptionValueSuspense As Integer = 0
	Public Const ACOptionNumber As Integer = 2002
	
	Public Enum UWDetailScreenMode
		UWPaymentDetails = 0
		UWReserveDetails = 1
	End Enum
	
	'Public variables
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
    Public g_iUserId As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lCurrencyID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lInsurance_file_cnt As Integer
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	' **************************************************
	' **************************************************
	' **********   360 - Taxes on Claims    ************
	' **************************************************
	' **************************************************
	
	' **************************************************
	' claim payment details array position constants
	' **************************************************
	Public Const kClaimPaytDetWorkClaimPaymentId As Integer = 0
	Public Const kClaimPaytDetClaimPerilId As Integer = 1
	Public Const kClaimPaytDetDateOfPayment As Integer = 2
	Public Const kClaimPaytDetAmount As Integer = 3
	Public Const kClaimPaytDetTaxAmount As Integer = 4
	Public Const kClaimPaytDetPartyCnt As Integer = 5
	Public Const kClaimPaytDetComments As Integer = 6
	Public Const kClaimPaytDetIsReferred As Integer = 7
	Public Const kClaimPaytDetCreatedBy As Integer = 8
	Public Const kClaimPaytDetPayeeMediaType As Integer = 9
	Public Const kClaimPaytDetPayeeName As Integer = 10
	Public Const kClaimPaytDetPayeeBankName As Integer = 11
	Public Const kClaimPaytDetPayeeSortCode As Integer = 12
	Public Const kClaimPaytDetPayeeAccountNo As Integer = 13
	Public Const kClaimPaytDetPayeeCountry As Integer = 14
	Public Const kClaimPaytDetPayeeComments As Integer = 15
	Public Const kClaimPaytDetSequenceNo As Integer = 16
	Public Const kClaimPaytDetTreatyId As Integer = 17
	Public Const kClaimPaytDetClaimPaymentToId As Integer = 18
	Public Const kClaimPaytDetPaymentPartyTo As Integer = 19
	Public Const kClaimPaytDetInsuredDomiciled As Integer = 20
	Public Const kClaimPaytDetInsuredPercentage As Integer = 21
	Public Const kClaimPaytDetInsuredTaxNumber As Integer = 22
	Public Const kClaimPaytDetPayeeDomiciled As Integer = 23
	Public Const kClaimPaytDetPayeePercentage As Integer = 24
	Public Const kClaimPaytDetPayeeTaxNumber As Integer = 25
	Public Const kClaimPaytDetSafeHarbourId As Integer = 26
	Public Const kClaimPaytDetSafeHarbourPercentage As Integer = 27
	Public Const kClaimPaytDetIsTaxExempt As Integer = 28
	Public Const kClaimPaytDetIsWHTExempt As Integer = 29
	Public Const kClaimPaytDetIsSettlement As Integer = 30
	Public Const kClaimPaytDetDocumentId As Integer = 31
	Public Const kClaimPaytDetPartyShortname As Integer = 32
	Public Const kClaimPaytDetClaimPayable As Integer = 33
	Public Const kClaimPaytDetParty As Integer = 34
	Public Const kClaimPaytDetAgent As Integer = 35
	Public Const kClaimPaytDetClient As Integer = 36
	Public Const kClaimPaytDetMediaRef As Integer = 37
	Public Const kClaimPaytDetClaimPaymentToDesc As Integer = 38
	Public Const kClaimPaytDetSafeHarbourDesc As Integer = 39
	Public Const kClaimPaytDetMediaTypeDesc As Integer = 40
	Public Const kClaimPaytDetCountryDesc As Integer = 41
	Public Const kClaimPaytDetExcessAmount As Integer = 42
	Public Const kClaimPaytDetPayeeAddress1 As Integer = 43
	Public Const kClaimPaytDetPayeeAddress2 As Integer = 44
	Public Const kClaimPaytDetPayeeAddress3 As Integer = 45
	Public Const kClaimPaytDetPayeeAddress4 As Integer = 46
	Public Const kClaimPaytDetPayeePostalCode As Integer = 47
	Public Const kClaimPaytDetThirdPartyReference As Integer = 48
    Public Const kClaimPaytDetChequeDate As Integer = 49
    Public Const kClaimBankPaymentTypeId As Integer = 50
    Public Const kClaimPaytDetOurReference As Integer = 51
    Public Const kClaimPayDetIsExGratia As Integer = 52
    Public Const kClaimPayBIC As Integer = 53
    Public Const kClaimPayIBAN As Integer = 54

	
	
	' **************************************************
	' option constant values
	' **************************************************
	Public Const kPayeeOptNone As Integer = 0
	Public Const kPayeeOptClaimPayable As Integer = 1
	Public Const kPayeeOptParty As Integer = 2
	Public Const kPayeeOptAgent As Integer = 4
	Public Const kPayeeOptClient As Integer = 8
	
	' **************************************************
	' claim detail array position constants
	' **************************************************
	Public Const kClaimDetailRiskId As Integer = 0
	Public Const kClaimDetailRiskTypeDesc As Integer = 1
	Public Const kClaimDetailLossCurrencyId As Integer = 2
	Public Const kClaimDetailLossCurrencyDesc As Integer = 3
	Public Const kClaimDetailLossFromDate As Integer = 4
	Public Const kClaimDetailLeadAgentCnt As Integer = 5
	Public Const kClaimDetailInsuredCnt As Integer = 6
	Public Const kClaimDetailInsuredName As Integer = 7
	Public Const kClaimDetailProductId As Integer = 8
	Public Const kClaimDetailAgentName As Integer = 9
	Public Const kClaimDetailClaimSourceId As Integer = 10
	Public Const kClaimDetailClientCurrencyId As Integer = 11
	Public Const kClaimDetailAgentCurrencyId As Integer = 12
	Public Const kClaimDetailBaseCurrencyId As Integer = 13
	Public Const kClaimDetailAgentDomiciledForTax As Integer = 14
	Public Const kClaimDetailAgentTaxNumber As Integer = 15
	Public Const kClaimDetailAgentTaxPercentage As Integer = 16
	Public Const kClaimDetailAgentTaxExempt As Integer = 17
	Public Const kClaimDetailClientDomiciledForTax As Integer = 18
	Public Const kClaimDetailClientTaxNumber As Integer = 19
	Public Const kClaimDetailClientTaxPercentage As Integer = 20
	Public Const kClaimDetailClientTaxExempt As Integer = 21
	Public Const kClaimDetailAgentIsInTransferMode As Integer = 22
	Public Const kClaimDetailTransferToBusinessType As Integer = 23
	Public Const kClaimDetailTransferToPartyCnt As Integer = 24
	Public Const kClaimDetailTransferAgentDomiciledForTax As Integer = 25
	Public Const kClaimDetailTransferAgentTaxNumber As Integer = 26
	Public Const kClaimDetailTransferAgentTaxpercentage As Integer = 27
	Public Const kClaimDetailTransferAgentTaxExempt As Integer = 28
	Public Const kClaimDetailTransferAgentCurrencyId As Integer = 29
	Public Const kClaimDetailTransferAgentPartyName As Integer = 30
	Public Const kClaimDetailClassOfBusinessCode As Integer = 31
	Public Const kClaimDetailClassOfBusinessId As Integer = 32
	Public Const kClaimDetailInsuranceFileCnt As Integer = 33
	Public Const kClaimDetailPostClaimsTaxes As Integer = 34
	Public Const kClaimDetailClaimNumber As Integer = 35
	Public Const kClaimDetailClaimPerilDescription As Integer = 36
	Public Const kClaimDetailPreventCancelledAgents As Integer = 37
	Public Const kClaimDetailLeadAgentDateCancelled As Integer = 38
	Public Const kClaimDetailTransferAgentDateCancelled As Integer = 39
	Public Const kClaimDetailProductMediaTypeMandatory As Integer = 40
	
	'**********************************************
	' lookup constants
	'**********************************************
	Public Const kLookupTableNameMediaType As String = "MediaType"
	Public Const kLookupTableNameCountry As String = "Country"
	Public Const kLookupTableNameCurrency As String = "Currency"
	Public Const kLookupTableNameTaxGroup As String = "Tax_Group"
	Public Const kLookupTableNameTaxBand As String = "Tax_Band"
	Public Const kLookupTableNameClassOfBusiness As String = "Class_Of_Business"
	
	Public Const kLookupItemId As Integer = 0
	Public Const kLookupDescription As Integer = 1
	Public Const kLookupCode As Integer = 2
	
	Public Const kLookupClaimPaymentToClaimPayable As Integer = 3
	Public Const kLookupClaimPaymentToParty As Integer = 4
	Public Const kLookupClaimPaymentToAgent As Integer = 5
	Public Const kLookupClaimPaymentToClient As Integer = 6
	
	Public Const kLookupSafeHarbourPercentage As Integer = 3
	
	Public Const kLookupTaxGroupIsWithHoldingTax As Integer = 3
	Public Const kLookupTaxGroupAdvancedTaxScript As Integer = 4
    Public Const kLookupTaxGroupIsTaxAmountEditable As Integer = 5

	Public Const kLookupTGTBDetailsTaxGroupId As Integer = 0
	Public Const kLookupTGTBDetailsTaxBandId As Integer = 1
	Public Const kLookupTGTBDetailsIsWithholdingTax As Integer = 2
	
	Public Const kLookupMediaTypeValidationCode As Integer = 3
	Public Const kLookupMediaTypeValidationCodeBank As String = "BANK"
	
	Public Const kLookupMediaTypeIsValidationEnabled As Integer = 4
	
	'**********************************************
	' screen method constants
	'**********************************************
	Public Const kScreenMethodPayment As Integer = 0
	Public Const kScreenMethodReceipt As Integer = 1
	
	'**********************************************
	' list view taxes on this payment
	'**********************************************
	Public Const kTaxDetColHIndexReserveType As Integer = 1
	Public Const kTaxDetColHIndexTaxGroup As Integer = 2
	Public Const kTaxDetColHIndexTaxBand As Integer = 3
	Public Const kTaxDetColHIndexPercentage As Integer = 4
	Public Const kTaxDetColHIndexTaxAmount As Integer = 5
	
	Public Const kTaxDetColHCodeReserveType As String = "ReserveType"
	Public Const kTaxDetColHCodeTaxGroup As String = "TaxGroup"
	Public Const kTaxDetColHCodeTaxBand As String = "TaxBand"
	Public Const kTaxDetColHCodePercentage As String = "Percentage"
	Public Const kTaxDetColHCodeTaxAmount As String = "Value"
	
	Public Const kTaxDetailsSubItemsTaxGroup As Integer = 1
	Public Const kTaxDetailsSubItemsTaxBand As Integer = 2
	Public Const kTaxDetailsSubItemsPercentage As Integer = 3
	Public Const kTaxDetailsSubItemsTaxAmount As Integer = 4
	
	'**********************************************
	' list view payment detail reserve level constants
	'**********************************************
	
	Public Const kPayDetColHIndexType As Integer = 1
	Public Const kPayDetColHIndexTypeId As Integer = 2
	Public Const kPayDetColHIndexTypeDesc As Integer = 3 ' either reserve or recovery
	Public Const kPayDetColHIndexTotalReserve As Integer = 4
	Public Const kPayDetColHIndexRevisedReserve As Integer = 5 '(RC) QBENZ001
	Public Const kPayDetColHIndexPaidToDate As Integer = 6
	Public Const kPayDetColHIndexPaidToDateTax As Integer = 7
	
	''Start(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
	Public Const kPayDetColHIndexReceivedToDate As Integer = 8
	Public Const kPayDetColHIndexReceivedToDateTax As Integer = 9
	''End(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
	
	Public Const kPayDetColHIndexCurrentReserve As Integer = 10
	Public Const kPayDetColHIndexThisPayment As Integer = 11
	Public Const kPayDetColHIndexThisPaymentTax As Integer = 12
	
	''Start(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
	Public Const kPayDetColHIndexThisReceiptInclTax As Integer = 13
	Public Const kPayDetColHIndexThisReceiptTax As Integer = 14
	''End(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
	Public Const kPayDetColHIndexCostToClaim As Integer = 15
	Public Const kPayDetColHIndexIsExcess As Integer = 16
	Public Const kPayDetColHIndexIsHistory As Integer = 17
	
	Public Const kPayDetColHCodeType As String = "Type"
	Public Const kPayDetColHCodeTypeId As String = "TypeId"
	Public Const kPayDetColHCodeTypeDesc As String = "TypeDesc" ' either reserve or recovery
	Public Const kPayDetColHCodeTotalReserve As String = "TotalReserve"
	Public Const kPayDetColHCodePaidToDate As String = "PaidToDate"
	Public Const kPayDetColHCodePaidToDateTax As String = "PaidToDateTax"
	Public Const kPayDetColHCodeCurrentReserve As String = "CurrentReserve"
	Public Const kPayDetColHCodeThisPayment As String = "ThisPayment"
	Public Const kPayDetColHCodeTax As String = "ThisPaymentTax"
	Public Const kPayDetColHCodeCostToClaim As String = "CostToClaim"
	Public Const kPayDetColHCodeIsExcess As String = "IsExcess"
	Public Const kPayDetColHCodeIsHistory As String = "IsHistory"
	
	''Start(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
	Public Const kPayDetColHCodeReceivedToDate As String = "ReceivedToDate"
	Public Const kPayDetColHCodeReceivedToDateTax As String = "ReceivedToDateTax"
	Public Const kPayDetColHCodeThisReceipt As String = "ThisReceipt"
	Public Const kPayDetColHCodeThisReceiptIncTax As String = "ThisReceiptIncTax"
	''End(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
	
	
	Public Const kPayDetailsSubItemsTypeId As Integer = 1
	Public Const kPayDetailsSubItemsTypeDesc As Integer = 2
	Public Const kPayDetailsSubItemsTotalReserve As Integer = 3
	Public Const kPayDetailsThisReserveRevision As Integer = 4 '(RC) QBENZ001
	Public Const kPayDetailsSubItemsPaidToDate As Integer = 5
	Public Const kPayDetailsSubItemsPaidToDateTax As Integer = 6
	''Start(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
	Public Const kPayDetailsSubItemsReceivedToDate As Integer = 7
	Public Const kPayDetailsSubItemsReceivedToDateTax As Integer = 8
	''End(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
	Public Const kPayDetailsSubItemsCurrentReserve As Integer = 9
	Public Const kPayDetailsSubItemsThisPaymentInclTax As Integer = 10
	Public Const kPayDetailsSubItemsThisPaymentTax As Integer = 11
	''Start(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
	Public Const kPayDetailsSubItemsThisReceipt As Integer = 12
	Public Const kPayDetailsSubItemsThisReceiptTax As Integer = 13
	''End(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
	Public Const kPayDetailsSubItemsCostToClaim As Integer = 14
	Public Const kPayDetailsSubItemsIsExcess As Integer = 15
	Public Const kPayDetailsSubItemsIsHistory As Integer = 16
	'**********************************************
	' list view payment detail item level constants
	'**********************************************
	Public Const kPayDetItemColHIndexReserveId As Integer = 2
	Public Const kPayDetItemColHIndexReserveDesc As Integer = 3
	Public Const kPayDetItemColHIndexThisPayment As Integer = 4
	Public Const kPayDetItemColHIndexThisPaymentTax As Integer = 5
	Public Const kPayDetItemColHIndexTotal As Integer = 6
	Public Const kPayDetItemColHIndexIsExcess As Integer = 7
	
	Public Const kPayDetItemColHCodeReserveId As String = "ReserveId"
	Public Const kPayDetItemColHCodeReserveDesc As String = "TypeDesc"
	Public Const kPayDetItemColHCodeThisPayment As String = "ThisPayment"
	Public Const kPayDetItemColHCodeThisPaymentTax As String = "ThisPaymentTax"
	Public Const kPayDetItemColHCodeTotal As String = "Total"
	Public Const kPayDetItemColHCodeIsExcess As String = "Excess"
	
	Public Const kPayItemDetailsSubItemsReserveId As Integer = 1
	Public Const kPayItemDetailsSubItemsReserveDesc As Integer = 2
	Public Const kPayItemDetailsSubItemsThisPayment As Integer = 3
	Public Const kPayItemDetailsSubItemsThisPaymentTax As Integer = 4
	Public Const kPayItemDetailsSubItemsTotal As Integer = 5
	Public Const kPayItemDetailsSubItemsIsExcess As Integer = 6
	
	'**************************************************
	' resource detail constants
	'**************************************************
	Public Const kResDetailsTotalReserve As Integer = 206
	Public Const kResDetailsPaidToDate As Integer = 207
	Public Const kResDetailsPaidToDateTax As Integer = 208
	Public Const kResDetailsCurrentReserve As Integer = 209
	Public Const kResDetailsThisPaymentIncludingTax As Integer = 210
	Public Const kResDetailsThisPaymentTax As Integer = 211
	Public Const kResDetailsCostToClaim As Integer = 212
	Public Const kResDetailsExcess As Integer = 213
	Public Const kResDetailsTotal As Integer = 214
	Public Const kResDetailsThisPaymentExcess As Integer = 215
	Public Const kResDetailsThisPayment As Integer = 216
	
	''Start(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
	Public Const kResDetailsReceivedToDate As Integer = 217
	Public Const kResDetailsReceivedToDateInclTax As Integer = 218
	Public Const kResDetailsThisReceipt As Integer = 219
	Public Const kResDetailsThisReceiptInclusiveTax As Integer = 220
	''End(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
	
	
	
	
	'**************************************************
	' payment details array position constants
	'**************************************************
	Public Const kPaymentDetailsType As Integer = 0
	Public Const kPaymentDetailsTypeId As Integer = 1
	Public Const kPaymentDetailsReserveDescription As Integer = 2
	Public Const kPaymentDetailsTotalReserve As Integer = 3
	Public Const kPaymentDetailsPaidToDate As Integer = 4
	Public Const kPaymentDetailsPaidToDateTax As Integer = 5
	Public Const kPaymentDetailsPaidToDateTaxWHT As Integer = 6
	Public Const kPaymentDetailsCurrentReserve As Integer = 7
	Public Const kPaymentDetailsThisPayment As Integer = 8
	Public Const kPaymentDetailsThisPaymentTax As Integer = 9
	Public Const kPaymentDetailsThisPaymentTaxWHT As Integer = 10
	Public Const kPaymentDetailsCostToClaim As Integer = 11
	Public Const kPaymentDetailsIsExcess As Integer = 12
	Public Const kPaymentDetailsIsHistory As Integer = 13
	'**************************************************
	' default claim payment system option values
	'**************************************************
	Public Const kDefaultClaimPaymentOptionSuspense As Integer = 0
	Public Const kDefaultClaimPaymentOptionThirdParty As Integer = 1
	Public Const kDefaultClaimPaymentOptionUsersChoice As Integer = 2
	Public Const kDefaultClaimPaymentOptionClient As Integer = 3
	
	'**************************************************
	' Tab Constants
	'**************************************************
	Public Const kTabThisPayment As Integer = 1
	
	'**************************************************
	' system option constants
	'**************************************************
	Public Const kSysOptionDefaultClaimPayment As Integer = 2002
	Public Const kSysOptionClaimAdvancedTaxScripting As Integer = 5007
	Public Const kSysOptionAllowNegativeReserve As Integer = 1016
	Public Const kSysOptionClaimPaymentAuthority As Integer = 2020
	Public Const kSysOptionClaimPaymentIsGross As Integer = 5018
	Public Const kSysOptionQAS As Integer = 13
	Public Const kSysOptionATSSattlement As Integer = 5071
    Public Const kSysOptionPaymentATSSafeHarbour As Integer = 5072
    Public Const kSysOptionPaymentPExGratiaAccount As Integer = 5114
	Public Const kSysOptionCurrentReserveIsGross As Integer = 5239
	
	'**************************************************
	' exemptions constants
	'**************************************************
	Public Const kWHTExempt As Integer = 0
	Public Const kTaxExempt As Integer = 1
	
	'**************************************************
	' payment details type constants
	'**************************************************
	Public Const kTypeReserve As String = "1"
	Public Const kTypeRecovery As String = "2"
	
	'**************************************************
	' account constants
	'**************************************************
	Public Const kAccountCLMPAYABLE As String = "CLMPAYABLE"
	
	'**************************************************
	' claim payment item details array position constants
	'**************************************************
	Public Const kClaimPayItemDetReserveId As Integer = 0
	Public Const kClaimPayItemDetReserveTypeDesc As Integer = 1
	Public Const kClaimPayItemDetThisPayment As Integer = 2
	Public Const kClaimPayItemDetTaxAmount As Integer = 3
	Public Const kClaimPayItemDetTaxAmountWHT As Integer = 4
	Public Const kClaimPayItemDetCurrencyId As Integer = 5
	Public Const kClaimPayItemDetCurrencyBaseXRate As Integer = 6
	Public Const kClaimPayItemDetTaxGroupId As Integer = 7
	Public Const kClaimPayItemDetTotalReserve As Integer = 8
	Public Const kClaimPayItemDetPaidToDate As Integer = 9
	Public Const kClaimPayItemDetBalance As Integer = 10
	Public Const kClaimPayItemDetPaymentToLossXRate As Integer = 11
	Public Const kClaimPayItemDetCurrencyDescription As Integer = 12
	Public Const kClaimPayItemDetTaxGroupDescription As Integer = 13
	Public Const kClaimPayItemDetIsWithHoldingTax As Integer = 14
	Public Const kClaimPayItemDetAdvancedTaxScript As Integer = 15
	Public Const kClaimPayItemDetWorkClaimPaymentItemId As Integer = 16
	
	Public Const kTaxGroupNone As String = "(none)"
	Public Const kTaxGroupNull As String = " " ' PN 45184
	
	Public Const kPaymentDetailsFirstReserveItemRow As Integer = 2
	
	'***********************************************
	' Payment Control Mode
	'***********************************************
	Public Const kModeViewPayment As Integer = 0
	Public Const kModeNewPayment As Integer = 1
	Public Const kModeHistoricPayment As Integer = 2
	
	'***********************************************
	' Tax Type TransCodes
	'***********************************************
	Public Const kTaxTransTypeClaimPayment As String = "TTCP"
	Public Const kTaxTransTypeClaimSalvageReceipt As String = "TTCS"
	Public Const kTaxTransTypeClaimThirdPartyRecoveryReceipt As String = "TTCR"
	
	
	'***********************************************
	' Tax Parameters Array position
	'***********************************************
	Public Const kProcessType As Integer = 0
	Public Const kPayee As Integer = 1
	Public Const kPaymentToCode As Integer = 2
	Public Const kSafeHarbourCode As Integer = 3
	Public Const kSafeHarbourPercentage As Integer = 4
	Public Const kInsuredDomiciled As Integer = 5
	Public Const kInsuredPercentage As Integer = 6
	Public Const kInsuredTaxNumber As Integer = 7
	Public Const kPayeeDomiciled As Integer = 8
	Public Const kPayeePercentage As Integer = 9
	Public Const kPayeeTaxNumber As Integer = 10
	Public Const kIsTaxExempt As Integer = 11
	Public Const kIsWHTExempt As Integer = 12
	Public Const kIsSettlement As Integer = 13
	Public Const kCurrencyCode As Integer = 14
	Public Const kAmount As Integer = 15
	Public Const kExcessAmount As Integer = 16
	Public Const kPaymentAdjustment As Integer = 17
	Public Const kTaxArray As Integer = 18
	Public Const kErrorMessage As Integer = 19
	
	'***********************************************
	' Tax Parameters Tax Array position breakdown
	'***********************************************
	Public Const kTaxArrayTaxGroupId As Integer = 0
	Public Const kTaxArrayTaxBandId As Integer = 1
	Public Const kTaxArrayTaxCurrencyCode As Integer = 2
	Public Const kTaxArrayPercentage As Integer = 3
	Public Const kTaxArrayValue As Integer = 4
	Public Const kTaxArrayIsValue As Integer = 5
	Public Const kTaxArrayClassOfBusinessId As Integer = 6
	Public Const kTaxArraySequence As Integer = 7
	Public Const kTaxArrayIsManuallyChanged As Integer = 8
	Public Const kTaxArrayTaxGroupDescription As Integer = 9
	Public Const kTaxArrayTaxBandDescription As Integer = 10
	
	'***********************************************
	' Is Manually Changed - Constants
	'***********************************************
	Public Const kIsManuallyChangedDefault As Integer = 0
	Public Const kIsManuallyChangedUser As Integer = 1
	Public Const kIsManuallyChangedScript As Integer = 2
	
	Public Const kTaxScriptModePayment As Integer = 1
	Public Const kTaxScriptModeReceipt As Integer = 2
	
	'***********************************************
	' List View Special Characters
	'***********************************************
	Public Const kLVWSpacer As String = "Spacer"
	Public Const kLVWTotal As String = "Total"
	Public Const kLVSCSpacerLineLower As String = "¯¯¯¯¯¯¯"
	Public Const kLVSCSpacerLineUpper As String = "_______"
	Public Const kLVSCExcessNotSet As String = "Not Set"
	Public Const kLVSCNotApplicable As String = "N/A"
	
	' **************************************************
	' Form payment details constants
	' **************************************************
	Public Const kCaptionTotalGrossPayment As String = "Net Payment"
	Public Const kCaptionTotalNetPayment As String = "Payment Total"
	'***********************************************
	
    'Start - (Sankar) - (Tech Spec - QBENZCR007 - Authorise Claim payments.doc)
    'developer guide no. 107
    <ThreadStatic()> _
 Private m_bShowPaymentView As Boolean
	'End - (Sankar) - (Tech Spec - QBENZCR007 - Authorise Claim payments.doc)
	
	'Start - (Sankar) - (Tech Spec - QBENZCR007 - Authorise Claim payments.doc)
	
	Public Property ShowPaymentView() As Boolean
		Get
			Return m_bShowPaymentView
		End Get
		Set(ByVal Value As Boolean)
			m_bShowPaymentView = Value
		End Set
	End Property
	'End - (Sankar) - (Tech Spec - QBENZCR007 - Authorise Claim payments.doc)
	
	
	
	
	Public Function IsValidCurrency(ByRef cValue As String) As Integer
		' Function to test value supplied is a valid currency value
		Dim result As Integer = 0
		Try 
			
			Dim curTemp As Decimal
			
			' set the default return value
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' perform basic checks
			If cValue.Length < 1 Then Return result
			Dim dbNumericTemp As Double
			If Not Double.TryParse(cValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then Return result
			
			' test by converting value to a currency
			curTemp = CDec(cValue)
			
			' worked fine
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch 
			
			
			
			Return result
		End Try
	End Function
End Module