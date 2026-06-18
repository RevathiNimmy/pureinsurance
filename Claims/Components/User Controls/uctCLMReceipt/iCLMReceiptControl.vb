Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.Globalization
Imports System.IO
Imports System.Windows.Forms
'Developer Guide no.129
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
	Public Const ACEditButton As Integer = 101
	
	'Constants used for resizing & positioning the user control
	Public Const ACListViewTop As Integer = 240
	Public Const ACCtrlVerticalSpacing As Integer = 120
	Public Const ACCommandButtonWidth As Integer = 1095
	Public Const ACCommandButtonHeight As Integer = 330
	
	' Public constants declared for the fields in the RecoveryType Array
	Public Const g_cIRTADescription As Integer = 0
	
	' Public constants declared for the Reserve Details Array
	Public Const g_cIRDARecoveryId As Integer = 0
	Public Const g_cIRDAinitialreserve As Integer = 1
	Public Const g_cIRDAReceivedToDate As Integer = 2
	Public Const g_cIRDArevisedreserve As Integer = 3
	Public Const g_cIRDAsuminsured As Integer = 4
	Public Const g_cIRDAaverage As Integer = 5
	Public Const g_cIRDArevisioncount As Integer = 6
	Public Const g_cIRDARecoveryType As Integer = 7
	Public Const g_cIRDArevisedentered As Integer = 8
	
	' Public constants declared for the Recovery Details
	Public Const g_cIRecoveryDAinitialreserve As Integer = 0
	Public Const g_cIRecoveryDArevisedreserve As Integer = 1
	Public Const g_cIRecoveryDAReceivedToDate As Integer = 2
	
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
    'S4B Claim Enhancements R&D 2005
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sUnderwritingOrAgency As String = ""
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	' **************************************************
	' **************************************************
	' **********   360 - Taxes on Claims    ************
	' **************************************************
	' **************************************************
	
	' **************************************************
	' option constant values
	' **************************************************
	Public Const kPayeeOptNone As Integer = 0
	Public Const kPayeeOptClaimReceivable As Integer = 1
	Public Const kPayeeOptParty As Integer = 2
	Public Const kPayeeOptAgent As Integer = 4
	Public Const kPayeeOptClient As Integer = 8
	
	'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
	' **************************************************
	' Option corresponding Id's in Database
	' **************************************************
	Public Const kPartyTypeClaimReceivable As Integer = 0
	Public Const kPartyTypeAgent As Integer = 1
	Public Const kPartyTypeClient As Integer = 2
	Public Const kPartyTypeInsurer As Integer = 3
	Public Const kPartyTypeParty As Integer = 4
	'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
	
	' **************************************************
	' claim detail array position constants
	' **************************************************
	Public Const kClaimDetailRiskTypeId As Integer = 0
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
	Public Const kClaimDetailPerilDescription As Integer = 36
	Public Const kClaimDetailInsurer As Integer = 41
	
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
	
	'**********************************************
	' screen method constants
	'**********************************************
	Public Const kScreenMethodPayment As Integer = 0
	Public Const kScreenMethodReceipt As Integer = 1
	
	'**********************************************
	' list view taxes on this payment
	'**********************************************
	Public Const kTaxDetColHIndexRecoveryType As Integer = 1
	Public Const kTaxDetColHIndexTaxGroup As Integer = 2
	Public Const kTaxDetColHIndexTaxBand As Integer = 3
	Public Const kTaxDetColHIndexPercentage As Integer = 4
	Public Const kTaxDetColHIndexTaxAmount As Integer = 5
	
	Public Const kTaxDetColHCodeRecoveryType As String = "RecoveryType"
	Public Const kTaxDetColHCodeTaxGroup As String = "TaxGroup"
	Public Const kTaxDetColHCodeTaxBand As String = "TaxBand"
	Public Const kTaxDetColHCodePercentage As String = "Percentage"
	Public Const kTaxDetColHCodeTaxAmount As String = "Value"
	
	Public Const kTaxDetailsSubItemsTaxGroup As Integer = 1
	Public Const kTaxDetailsSubItemsTaxBand As Integer = 2
	Public Const kTaxDetailsSubItemsPercentage As Integer = 3
	Public Const kTaxDetailsSubItemsTaxAmount As Integer = 4
	
	'**********************************************
	' list view coinsurance / reinsurance on this payment
	'**********************************************
	Public Const kInsuranceColHIndexRecoveryId As Integer = 1
	Public Const kInsuranceColHIndexRecoveryTypeDesc As Integer = 2
	Public Const kInsuranceColHIndexPartyId As Integer = 3
	Public Const kInsuranceColHIndexPartyName As Integer = 4
	Public Const kInsuranceColHIndexSharePercent As Integer = 5
	Public Const kInsuranceColHIndexRecoveryToDate As Integer = 6
	Public Const kInsuranceColHIndexThisRecovery As Integer = 7
	Public Const kInsuranceColHIndexThisRecoverySplit As Integer = 8
	
	Public Const kInsuranceColHCodeRecoveryId As String = "RecoveryId"
	Public Const kInsuranceColHCodeRecoveryTypeDesc As String = "RecoveryDesc"
	Public Const kInsuranceColHCodePartyId As String = "InsurerId"
	Public Const kInsuranceColHCodePartyName As String = "Insurer"
	Public Const kInsuranceColHCodeSharePercent As String = "SharePercent"
	Public Const kInsuranceColHCodeRecoveryToDate As String = "RecoveryToDate"
	Public Const kInsuranceColHCodeThisRecovery As String = "ThisRecovery"
	Public Const kInsuranceColHCodeThisRecoverySplit As String = "ThisRecoverySplit"
	
	Public Const kInsuranceSubItemsRecoveryTypeDesc As Integer = 1
	Public Const kInsuranceSubItemsPartyId As Integer = 2
	Public Const kInsuranceSubItemsPartyName As Integer = 3
	Public Const kInsuranceSubItemsSharePercent As Integer = 4
	Public Const kInsuranceSubItemsRecoveryToDate As Integer = 5
	Public Const kInsuranceSubItemsThisRecovery As Integer = 6
	Public Const kInsuranceSubItemsThisRecoverySplit As Integer = 7
	
	'*************************************************************
	' lcoinsurance / reinsurance details array position constants
	'*************************************************************
	Public Const kInsuranceDetailRecoveryId As Integer = 0
	Public Const kInsuranceDetailRecoveryTypeDesc As Integer = 1
	Public Const kInsuranceDetailPartyCnt As Integer = 2
	Public Const kInsuranceDetailPartyName As Integer = 3
	Public Const kInsuranceDetailPartyShare As Integer = 4
	Public Const kInsuranceDetailRecoveryToDate As Integer = 5
	Public Const kInsuranceDetailIsTaxShared As Integer = 6
	Public Const kInsuranceDetailRIArrangementLineId As Integer = 7
	Public Const kInsuranceDetailTreatyId As Integer = 8
	
	Public Const kInsuranceDetailLowerLimit As Integer = 9
	Public Const kInsuranceDetailUpperLimit As Integer = 10
	Public Const kInsuranceDetailType As Integer = 11
	Public Const kInsuranceDetailPayment As Integer = 12
	
	'**********************************************
	' list view payment detail recovery level constants
	'**********************************************
	Public Const kRecDetColHIndexRecoveryId As Integer = 1
	Public Const kRecDetColHIndexRecoveryTypeDesc As Integer = 2
	'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
	Public Const kRecDetColHIndexRecoveryPartyName As Integer = 3
	Public Const kRecDetColHIndexTotalReserve As Integer = 4
	Public Const kRecDetColHIndexRecoveredTotal As Integer = 5
	Public Const kRecDetColHIndexThisReceipt As Integer = 6
	Public Const kRecDetColHIndexTaxAmount As Integer = 7
	Public Const kRecDetColHIndexThisNet As Integer = 8
	Public Const kRecDetColHIndexBalance As Integer = 9
	Public Const kRecDetColHIndexRecoveryTypeId As Integer = 10
	'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
	
	
	Public Const kRecDetColHCodeRecoveryId As String = ""
	Public Const kRecDetColHCodeRecoveryTypeDesc As String = "Recovery Type"
	'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
	Public Const kRecDetColHCodeRecoveryPartyName As String = "Recovery Party"
	'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
	Public Const kRecDetColHCodeTotalReserve As String = "Total Reserve"
	Public Const kRecDetColHCodeRecoveredTotal As String = "Recovered Total"
	Public Const kRecDetColHCodeThisReceipt As String = "This Receipt"
	Public Const kRecDetColHCodeTaxAmount As String = "This Tax"
	Public Const kRecDetColHCodeThisNet As String = "This Net"
	Public Const kRecDetColHCodeBalance As String = "Balance"
	Public Const kRecDetColHCodeRecoveryTypeId As String = "Recovery Type Id"
	
	Public Const kRecDetSubItemsRecoveryTypeDesc As Integer = 1
	'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc) - (4.3.1)
	Public Const kRecDetSubItemsRecoveryPartyName As Integer = 2
	Public Const kRecDetSubItemsTotalReserve As Integer = 3
	Public Const kRecDetSubItemsRecoveredTotal As Integer = 4
	Public Const kRecDetSubItemsThisReceipt As Integer = 5
	Public Const kRecDetSubItemsTaxAmount As Integer = 6
	Public Const kRecDetSubItemsThisNet As Integer = 7
	Public Const kRecDetSubItemsBalance As Integer = 8
	Public Const kRecDetSubItemsRecoveryTypeId As Integer = 9
	'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc) - (4.3.1)
	
	Public Const kRecDetItemColHIndexRecoveryId As Integer = 2
	Public Const kRecDetItemColHIndexRecoveryDesc As Integer = 3
	Public Const kRecDetItemColHIndexThisReceipt As Integer = 4
	Public Const kRecDetItemColHIndexThisReceiptTax As Integer = 5
	Public Const kRecDetItemColHIndexTotal As Integer = 6
	Public Const kRecDetItemColHIndexIsExcess As Integer = 7
	'**************************************************
	' resource detail constants
	'**************************************************
	Public Const kResDetailsRecoveryType As Integer = 206
	Public Const kResDetailsTotalReserve As Integer = 207
	Public Const kResDetailsRecoveredTotal As Integer = 208
	Public Const kResDetailsThisReceipt As Integer = 209
	Public Const kResDetailsTaxAmount As Integer = 210
	Public Const kResDetailsThisNet As Integer = 211
	Public Const kResDetailsBalance As Integer = 212
	Public Const kResDetailsTotal As Integer = 213
	'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
	Public Const kResDetailsRecoveryParty As Integer = 214
	'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
	
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
	Public Const kTabRecovery As Integer = 0
	Public Const kTabCoinsurance As Integer = 1
	Public Const kTabReinsurance As Integer = 2
	Public Const kTabThisReceipt As Integer = 3
	
	'**************************************************
	' system option constants
	'**************************************************
	Public Const kSysOptionDefaultClaimPayment As Integer = 2002
	Public Const kSysOptionATSSattlement As Integer = 5071
	
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
	Public Const kAccountCLMRECEIVABLE As String = "CLMRECEIVABLE"
	' Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
	Public Const kAccountCLMParty As String = "PARTY"
	Public Const kAccountCLMClient As String = "CLIENT"
	Public Const kAccountCLMAgent As String = "AGENT"
	Public Const kAccountCLMInsurer As String = "INSURER"
	' End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
	
	'**************************************************
	' claim payment item details array position constants
	'**************************************************
	Public Const kRecoveryDetailsRecoveryId As Integer = 0
	Public Const kRecoveryDetailsClaimPerilId As Integer = 1
	Public Const kRecoveryDetailsRecoveryTypeId As Integer = 2
	Public Const kRecoveryDetailsRecoveryTypeDescription As Integer = 3
	Public Const kRecoveryDetailsCurrencyId As Integer = 4
	Public Const kRecoveryDetailsCurrencyDescription As Integer = 5
	Public Const kRecoveryDetailsInitialReserve As Integer = 6
	Public Const kRecoveryDetailsRevisedReserve As Integer = 7
	Public Const kRecoveryDetailsReceivedToDate As Integer = 8
	Public Const kRecoveryDetailsRevisionCount As Integer = 9
	Public Const kRecoveryDetailsTaxAmount As Integer = 10
	Public Const kRecoveryDetailsClaimId As Integer = 11
	Public Const kRecoveryDetailsClaimsIsPostTaxes As Integer = 12
	'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc) - (4.3.1)
	Public Const kRecoveryDetailsClaimsPartyType As Integer = 15
	Public Const kRecoveryDetailsClaimsPartyCnt As Integer = 16
	Public Const kRecoveryDetailsClaimsShortName As Integer = 17
	Public Const kRecoveryDetailsClaimsResolvedName As Integer = 18
	Public Const kRecoveryDetailsClaimsPartyDesc As Integer = 19
	'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc) - (4.3.1)
	
	''Start(Saurabh Agrawal) Tech Spec QBENZCR004 Tech Spec Claim Recovery Reinsurance
	'**************************************************
	' claim Receipt item details array position constants
	'**************************************************
	Public Const kClaimRecItemDetRecoveryId As Integer = 0
	Public Const kClaimRecItemDetRecoveryTypeDesc As Integer = 1
	Public Const kClaimRecItemDetThisReceipt As Integer = 2
	Public Const kClaimRecItemDetTaxAmount As Integer = 3
	Public Const kClaimRecItemDetCurrencyId As Integer = 4
	Public Const kClaimRecItemDetCurrencyBaseXRate As Integer = 5
	Public Const kClaimRecItemDetTaxGroupId As Integer = 6
	Public Const kClaimRecItemDetTotalRecovery As Integer = 7
	Public Const kClaimRecItemDetReceivedToDate As Integer = 8
	Public Const kClaimRecItemDetBalance As Integer = 9
	Public Const kClaimRecItemDetReceiptToLossXRate As Integer = 10
	Public Const kClaimRecItemDetCurrencyDescription As Integer = 11
	Public Const kClaimRecItemDetTaxGroupDescription As Integer = 12
	Public Const kClaimRecItemDetIsWithHoldingTax As Integer = 13
	Public Const kClaimRecItemDetAdvancedTaxScript As Integer = 14
	Public Const kClaimRecItemDetWorkClaimReceiptItemId As Integer = 15
	Public Const kClaimRecItemDetRecoveryTypeCode As Integer = 17
	Public Const kClaimRecItemDetRecoveryPartyTypeId As Integer = 18
	Public Const kClaimRecItemDetRecoveryPartyCnt As Integer = 19
	Public Const kClaimRecItemDetShortName As Integer = 20
	Public Const kClaimRecItemDetResolvedName As Integer = 21
	''End(Saurabh Agrawal) Tech Spec QBENZCR004 Tech Spec Claim Recovery Reinsurance
	
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
	' Receipt Tax Parameters Array position
	'***********************************************
	Public Const kProcessType As Integer = 0
	Public Const kPayee As Integer = 1
	Public Const kPaymentToCode As Integer = 2
	Public Const kInsuredDomiciled As Integer = 3
	Public Const kInsuredPercentage As Integer = 4
	Public Const kInsuredTaxNumber As Integer = 5
	Public Const kIsTaxExempt As Integer = 6
	Public Const kIsSettlement As Integer = 7
	Public Const kCurrencyCode As Integer = 8
	Public Const kAmount As Integer = 9
	Public Const kTaxArray As Integer = 10
	Public Const kReceivablePercentage As Integer = 11
	Public Const kErrorMessage As Integer = 12
	
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
	
	'**************************************
	' uctCLMReceiptControl Component Modes
	'**************************************
	Public Const kRecoveryModeSalvageReserve As Integer = 1
	Public Const kRecoveryModeSalvageReceipt As Integer = 2
	Public Const kRecoveryModeThirdPartyReserve As Integer = 3
	Public Const kRecoveryModeThirdPartyReceipt As Integer = 4
	
	
	'**********************************
	' Custom Interface Constants
	'**********************************
	Public Const kTaxGroupNone As String = "(none)"
	Public Const kReceiptDetailsFirstReserveItemRow As Integer = 2
	
	Public Const kTaxScriptModePayment As Integer = 1
	Public Const kTaxScriptModeReceipt As Integer = 2
	
	Public Const kCoinsuranceFilter As Integer = 0
	Public Const kReinsuranceFilter As Integer = 1
	
	'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
	Public Const kNewPartyPartyID As Integer = 0
	Public Const kNewPartyShortName As Integer = 1
	Public Const kNewPartyLongName As Integer = 2
	Public Const kNewPartyRecoveryId As Integer = 3
	Public Const kNewPartyPartyTypeId As Integer = 4
	'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
	
	
	''Start(Saurabh Agrawal) Tech Spec Claims Recovery Reinsurance
	''ThisReceipt Details Collection Constants
	Public Const kWorkClaimReceiptId As Integer = 0
	Public Const kClaimPerilId As Integer = 1
	Public Const kDateOfReceipt As Integer = 2
	Public Const kRecAmount As Integer = 3
	Public Const kTaxAmount As Integer = 4
	Public Const kPartyID As Integer = 5
	Public Const kComments As Integer = 6
	Public Const kCreatedBy As Integer = 7
	Public Const kPayeeMediaType As Integer = 8
	Public Const kPayeeName As Integer = 9
	Public Const kPayeeBankName As Integer = 10
	Public Const kPayeeSortCode As Integer = 11
	Public Const kPayeeAccountNo As Integer = 12
	Public Const kPayeeCountry As Integer = 13
	Public Const kPayeeComments As Integer = 14
	Public Const kPayeeMediaRef As Integer = 15
	Public Const kRecInsuredDomiciled As Integer = 16
	Public Const kRecInsuredPercentage As Integer = 17
	Public Const kRecInsuredTaxNumber As Integer = 18
	Public Const kRecIsTaxExempt As Integer = 19
	Public Const kRecIsSettlement As Integer = 20
	Public Const kRecDocumentId As Integer = 21
	Public Const kResolvedName As Integer = 22
	Public Const kMediaTypeDesc As Integer = 23
	Public Const kCountryDescription As Integer = 24
	Public Const kThirdPartyReference As Integer = 25
	Public Const kSIROPTReceiptExcludeTax As Integer = 5067
	''End (Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
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