Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Text
<System.Runtime.InteropServices.ProgId("gACTLibrary_NET.gACTLibrary")> _
 Public Module gACTLibrary
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	' ***************************************************************** '
	' Class Name: gACTLibrary.bas
	'
	' Date: 19/07/2002
	'
	' Description: Public bas module containing core accounting functions
	'
	' Edit History: SJ 19/07/2002  Original created from gACTLibraries.dll
	' RAW 17/12/2002 : PS187 : Added constant for Report lookup
	' RAW 13/01/2003 : PS187 : Added DocType constant for Binding Journal
	'
	' ***************************************************************** '
	
	Private Const ACClass As String = "gACTLibrary"
	
	Public Enum ACTEAccountSign
		acteSignDebit = 1
		acteSignCredit = -1
	End Enum
	Public Const ACTLookup As Integer = 99
	
	Public Const ACTMaxAccounts As Integer = 5000
	
	'********************************************************
	' Type Table Definitions
	Public Const ACTLookupDocumentType As String = "DocumentType"
	Public Const ACTDocTypeJournal As Integer = 1
	Public Const ACTDocTypeDebitNote As Integer = 2
	Public Const ACTDocTypeCreditNote As Integer = 3
	Public Const ACTDocTypeNBDebit As Integer = 4
	Public Const ACTDocTypeNBCredit As Integer = 5
	Public Const ACTDocTypeCashCredit As Integer = 6
	Public Const ACTDocTypeCashDebit As Integer = 7
	Public Const ACTDocTypeAccrual As Integer = 8
	Public Const ACTDocTypePrePayment As Integer = 9
	Public Const ACTDocTypeAdHocReversingJournal As Integer = 10
	Public Const ACTDocTypeDepreciationJournal As Integer = 11
	Public Const ACTDocTypeAdHocRecurringJournal As Integer = 12
	Public Const ACTDocTypePurchaseInvoice As Integer = 13
	Public Const ACTDocTypeWriteOff As Integer = 14
	Public Const ACTDocTypeRenewalDebit As Integer = 15
	Public Const ACTDocTypeRenewalCredit As Integer = 16
	Public Const ACTDocTypeEndorsementDebit As Integer = 17
	Public Const ACTDocTypeEndorsementCredit As Integer = 18
	Public Const ACTDocTypeAgencyTakeover As Integer = 19
	Public Const ACTDocTypeAdjustment As Integer = 20
	Public Const ACTDocTypeBadDebt As Integer = 21
	Public Const ACTDocTypeReceipt As Integer = 22
	Public Const ACTDocTypePayment As Integer = 23
	Public Const ACTDocTypeSalesInvoice As Integer = 24
	'EK 160300
	Public Const ACTDocTypePurchaseCreditNote As Integer = 25
	Public Const ACTDocTypeDiscountAllowed As Integer = 26
	Public Const ACTDocTypeDiscountReceived As Integer = 27
	Public Const ACTDocTypeClaimPayment As Integer = 28
	Public Const ACTDocTypeClaimReceipt As Integer = 29
	Public Const ACTDocTypeFee As Integer = 30
	Public Const ACTDocTypeShortPeriodDebit As Integer = 31
	Public Const ACTDocTypeShortPeriodCredit As Integer = 32
	Public Const ACTDocTypeDirectToInsurerDebit As Integer = 33
	Public Const ACTDocTypeDirectToInsurerCredit As Integer = 34
	Public Const ACTDocTypeTransferredDebit As Integer = 35
	Public Const ACTDocTypetransferredCredit As Integer = 36
	'eck200801
	Public Const ACTDocTypeInstalmentDebit As Integer = 37
	Public Const ACTDocTypeInstalmentCredit As Integer = 38
	Public Const ACTDocTypeInstalmentCash As Integer = 39
	'tomo050302
	Public Const ACTDocTypeClaimOpen As Integer = 40
	Public Const ACTDocTypeClaimAmend As Integer = 41
	'DD 29/11/2002: Instalments enhancements
	Public Const ACTDocTypeInstalmentNBDebit As Integer = 42
	Public Const ACTDocTypeInstalmentNBCredit As Integer = 43
	Public Const ACTDocTypeInstalmentEndorsementDebit As Integer = 44
	Public Const ACTDocTypeInstalmentEndorsementCredit As Integer = 45
	Public Const ACTDocTypeInstalmentRenewalDebit As Integer = 46
	Public Const ACTDocTypeInstalmentRenewalCredit As Integer = 47
	Public Const ACTDocTypeBindingJournal As Integer = 48 ' RAW 13/01/2003 : PS187 : Added
	Public Const ACTDocTypeCurrencyDifferenceCredit As Integer = 49 'KB PN 3036
	Public Const ACTDocTypeClaimCloneReversal = 58
	'ADO #39472: Instalment Claim Recovery document types
	Public Const ACTDocTypeInstalmentClaimDebit As Integer = 59
	Public Const ACTDocTypeInstalmentClaimCredit As Integer = 60

	Public Const ACTLookupDocTypeGroup As String = "DocTypeGroup"
	Public Const ACTDocTypeGroupUKVAT As Integer = 1 'Requires true specification
	Public Const ACTDocTypeGroupUKNOVAT As Integer = 2 '    --- ditto ---
	Public Const ACTDocTypeGroupReverse As Integer = 3
	Public Const ACTDocTypeGroupRecur As Integer = 4
	
	Public Const ACTLookupPostingStatus As String = "PostingStatus"
	Public Const ACTPostStatusRegistered As Integer = 1
	Public Const ACTPostStatusAuthorised As Integer = 2
	Public Const ACTPostStatusPosted As Integer = 3
	
	Public Const ACTLookupLedgerType As String = "LedgerType"
	Public Const ACTLedgerTypeGeneral As Integer = 1
	Public Const ACTLedgerTypeDebtor As Integer = 2
	Public Const ACTLedgerTypeCreditor As Integer = 3
	
	Public Const ACTLookupPurgeFrequency As String = "PurgeFrequency"
	Public Const ACTPurgeFreqNever As Integer = 1
	Public Const ACTPurgeFreqYearly As Integer = 2
	Public Const ACTPurgeFreqPeriodEnd As Integer = 3
	
	Public Const ACTLookupAccountType As String = "AccountType"
	Public Const ACTAccountTypeIncome As Integer = 1
	Public Const ACTAccountTypeExpense As Integer = 2
	Public Const ACTAccountTypeAsset As Integer = 3
	Public Const ACTAccountTypeLiability As Integer = 4
	Public Const ACTAccountTypeGLSuspense As Integer = 5
	
	' CF040399
	Public Const ACTLookupAccountStatus As String = "AccountStatus"
	Public Const ACTAccountStatusActive As Integer = 1
	Public Const ACTAccountStatusStopped As Integer = 2
	
	Public Const ACTLookupBatchStatus As String = "BatchStatus"
	Public Const ACTBatchStatusRegistered As Integer = 1
	Public Const ACTBatchStatusAuthorised As Integer = 2
	Public Const ACTBatchStatusComplete As Integer = 3
	
	Public Const ACTLookupMediaType As String = "MediaType"
	Public Const ACTMediaTypeCheque As Integer = 1
	Public Const ACTMediaTypeCash As Integer = 2
	Public Const ACTMediaTypeElectronic As Integer = 3
	
	'sw front Office receipting
	Public Const ACTLookupReceiptType As String = "CashListItem_Receipt_Type"
	Public Const ACTLookupBankType As String = "CashListItem_Bank"
	Public Const ACTLookupReverseType As String = "CashListItem_Reverse_Reason"
	Public Const ACTLookupPaymentTypeTable As String = "CashListItem_Payment_Type"
	Public Const ACTLookupPaymentStatus As String = "CashListItem_Payment_Status"

	'WPR12- Enhancement Quote Collection Process
	Public Const ACTLookupChequeType As String = "ChequeType"
	Public Const ACTLookupChequeClearingType As String = "Cheque_Clearing_Type"
	
	Public Const ACTLookupPaymentType As String = "PaymentType"
	Public Const ACTPaymentTypeCheque As Integer = 1
	Public Const ACTPaymentTypeCash As Integer = 2
	Public Const ACTPaymentTypeElectronic As Integer = 3
	
	Public Const ACTLookupAllocationStatus As String = "AllocationStatus"
	Public Const ACTAllocationStatusUnallocated As Integer = 1
	Public Const ACTAllocationStatusPosted As Integer = 2
	Public Const ACTAllocationStatusAllocated As Integer = 3
	Public Const ACTAllocationStatusPartial As Integer = 4
	
	Public Const ACTLookupMapType As String = "MapType"
	Public Const ACTMapTypeLedger As Integer = 1
	Public Const ACTMapTypeOther As Integer = 2
	
	Public Const ACTLookupCashListType As String = "CashListType"
	Public Const ACTCashListTypePayments As Integer = 1
	Public Const ACTCashListTypeReceipts As Integer = 2
	Public Const ACTCashListTypeClaimPayments As Integer = 3
	
	Public Const ACTLookupCashListStatus As String = "CashListStatus"
	Public Const ACTCashListStatusEntered As Integer = 1
	Public Const ACTCashListStatusOpened As Integer = 2
	Public Const ACTCashListStatusClosed As Integer = 3
	Public Const ACTCashListStatusInBanking As Integer = 4
	
	'Possible values for AllocationTransType
	Public Const ACTPrimaryForAllocation As Integer = 1
	Public Const ACTSecondaryForAllocation As Integer = 2
	
	'Miscellaneous
	Public Const ACTDefaultSearchAmountTolerance As Integer = 5
	Public Const ACTUseListHidden As Integer = 2
	'sw 06-11-2002
	Public Const ACTEditCheque As String = "editcheque"
	Public Const ACTCancelCheque As String = "cancelcheque"
	Public Const ACTStopCheque As String = "stopcheque"
	Public Const ACTUnderwritingDirect As String = "underwritingdirect"
	Public Const ACTApprove As String = "approve"
	'PSL 03/03/2003
	Public Const ACTViewCheque As String = "viewcashlistitem"
	'SMJB 10/07/2003
	Public Const ACTFindCashList As String = "findcashlist"
	
	' AMB 24/02/2003: PS220 - added for Manage Debtors development
	Public Const ACTRefund As String = "refund"
	'Debit/Credit
	Public Const ACTCredit As Integer = -1
	Public Const ACTDebit As Integer = 1
	
	' Auto Numbering
	Public Const ACTAutoNumberGroupCodeDocumentRef As String = "DOCREF"
	Public Const ACTAutoNumberRangeCodeAll As String = "ALL"
	'EK 220200 New Numbering Ranges
	Public Const ACTAutoNumberGroupCodeDocumentRef1 As String = "DOCREF1"
	Public Const ACTAutoNumberGroupCodeDocumentRef2 As String = "DOCREF2"
	Public Const ACTAutoNumberGroupCodeDocumentRef3 As String = "DOCREF3"
	Public Const ACTAutoNumberGroupCodeDocumentRef4 As String = "DOCREF4"
	Public Const ACTAutoNumberGroupCodeDocumentRef5 As String = "DOCREF5"
	Public Const ACTAutoNumberGroupCodeDocumentRef6 As String = "DOCREF6"
	Public Const ACTAutoNumberGroupCodeDocumentRef7 As String = "DOCREF7"
	Public Const ACTAutoNumberGroupCodeDocumentRef8 As String = "DOCREF8"
	Public Const ACTAutoNumberGroupCodeDocumentRef9 As String = "DOCREF9"
	Public Const ACTAutoNumberGroupCodeDocumentRef10 As String = "DOCREF10"
	Public Const ACTAutoNumberGroupCodeDocumentRef11 As String = "DOCREF11"
	Public Const ACTAutoNumberGroupCodeDocumentRef12 As String = "DOCREF12"
	Public Const ACTAutoNumberGroupCodeDocumentRef13 As String = "DOCREF13"
	Public Const ACTAutoNumberGroupCodeDocumentRef14 As String = "DOCREF14"
	Public Const ACTAutoNumberGroupCodeDocumentRef15 As String = "DOCREF15"
	Public Const ACTAutoNumberGroupCodeDocumentRef16 As String = "DOCREF16"
	Public Const ACTAutoNumberGroupCodeDocumentRef17 As String = "DOCREF17"
	Public Const ACTAutoNumberGroupCodeDocumentRef18 As String = "DOCREF18"
	Public Const ACTAutoNumberGroupCodeDocumentRef19 As String = "DOCREF19"
	Public Const ACTAutoNumberGroupCodeDocumentRef20 As String = "DOCREF20"
	Public Const ACTAutoNumberGroupCodeDocumentRef21 As String = "DOCREF21"
	Public Const ACTAutoNumberGroupCodeDocumentRef22 As String = "DOCREF22"
	Public Const ACTAutoNumberGroupCodeDocumentRef23 As String = "DOCREF23"
	Public Const ACTAutoNumberGroupCodeDocumentRef24 As String = "DOCREF24"
	Public Const ACTAutoNumberGroupCodeDocumentRef25 As String = "DOCREF25"
	Public Const ACTAutoNumberGroupCodeDocumentRef26 As String = "DOCREF26"
	Public Const ACTAutoNumberGroupCodeDocumentRef27 As String = "DOCREF27"
	'100300 EK New Document Type
	Public Const ACTAutoNumberGroupCodeDocumentRef28 As String = "DOCREF28"
	Public Const ACTAutoNumberGroupCodeDocumentRef29 As String = "DOCREF29"
	Public Const ACTAutoNumberGroupCodeDocumentRef30 As String = "DOCREF30"
	Public Const ACTAutoNumberGroupCodeDocumentRef31 As String = "DOCREF31"
	Public Const ACTAutoNumberGroupCodeDocumentRef32 As String = "DOCREF32"
	Public Const ACTAutoNumberGroupCodeDocumentRef33 As String = "DOCREF33"
	Public Const ACTAutoNumberGroupCodeDocumentRef34 As String = "DOCREF34"
	Public Const ACTAutoNumberGroupCodeDocumentRef35 As String = "DOCREF35"
	Public Const ACTAutoNumberGroupCodeDocumentRef36 As String = "DOCREF36"
	'eck200801
	Public Const ACTAutoNumberGroupCodeDocumentRef37 As String = "DOCREF37"
	Public Const ACTAutoNumberGroupCodeDocumentRef38 As String = "DOCREF38"
	Public Const ACTAutoNumberGroupCodeDocumentRef39 As String = "DOCREF39"
	'tomo050302
	Public Const ACTAutoNumberGroupCodeDocumentRef40 As String = "DOCREF40"
	Public Const ACTAutoNumberGroupCodeDocumentRef41 As String = "DOCREF41"
	'DD 29/11/2002: Instalments enhancements
	Public Const ACTAutoNumberGroupCodeDocumentRef42 As String = "DOCREF42"
	Public Const ACTAutoNumberGroupCodeDocumentRef43 As String = "DOCREF43"
	Public Const ACTAutoNumberGroupCodeDocumentRef44 As String = "DOCREF44"
	Public Const ACTAutoNumberGroupCodeDocumentRef45 As String = "DOCREF45"
	Public Const ACTAutoNumberGroupCodeDocumentRef46 As String = "DOCREF46"
	Public Const ACTAutoNumberGroupCodeDocumentRef47 As String = "DOCREF47"
	Public Const ACTAutoNumberGroupCodeDocumentRef49 As String = "DOCREF49" 'KB PN 3036
	Public Const ACTAutoNumberGroupCodeDocumentRef58 As String = "DOCREF58"
	'ADO #39472: Instalment Claim Recovery group codes
	Public Const ACTAutoNumberGroupCodeDocumentRef59 As String = "DOCREF59"
	Public Const ACTAutoNumberGroupCodeDocumentRef60 As String = "DOCREF60"

	'EK 220200 New Numbering Ranges
	Public Const ACTAutoNumberRangeCodeJn As String = "JN"
	Public Const ACTAutoNumberRangeCodeSdn As String = "SDN"
	Public Const ACTAutoNumberRangeCodeScn As String = "SCN"
	Public Const ACTAutoNumberRangeCodeSnd As String = "SND"
	Public Const ACTAutoNumberRangeCodeSnc As String = "SNC"
	Public Const ACTAutoNumberRangeCodeCcr As String = "CCR"
	Public Const ACTAutoNumberRangeCodeCdr As String = "CDR"
	Public Const ACTAutoNumberRangeCodeACc As String = "ACC"
	Public Const ACTAutoNumberRangeCodePpt As String = "PPT"
	Public Const ACTAutoNumberRangeCodeRvj As String = "RVJ"
	Public Const ACTAutoNumberRangeCodeDpj As String = "DPJ"
	Public Const ACTAutoNumberRangeCodeRcj As String = "RCJ"
	Public Const ACTAutoNumberRangeCodePin As String = "PIN"
	Public Const ACTAutoNumberRangeCodeSwd As String = "SWD"
	Public Const ACTAutoNumberRangeCodeSrd As String = "SRD"
	Public Const ACTAutoNumberRangeCodeSrc As String = "SRC"
	Public Const ACTAutoNumberRangeCodeSed As String = "SED"
	Public Const ACTAutoNumberRangeCodeSec As String = "SEC"
	Public Const ACTAutoNumberRangeCodeSat As String = "SAT"
	Public Const ACTAutoNumberRangeCodeSaj As String = "SAJ"
	Public Const ACTAutoNumberRangeCodeSbd As String = "SBD"
	Public Const ACTAutoNumberRangeCodeSrp As String = "SRP"
	Public Const ACTAutoNumberRangeCodeSpy As String = "SPY"
	Public Const ACTAutoNumberRangeCodeSin As String = "SIN"
	Public Const ACTAutoNumberRangeCodePcn As String = "PCN"
	Public Const ACTAutoNumberRangeCodeDia As String = "DIA"
	Public Const ACTAutoNumberRangeCodeDir As String = "DIR"
	'100300 EK New Document Type
	Public Const ACTAutoNumberRangeCodeClp As String = "CLP"
	Public Const ACTAutoNumberRangeCodeClr As String = "CLR"
	Public Const ACTAutoNumberRangeCodeFee As String = "FEE"
	Public Const ACTAutoNumberRangeCodeShd As String = "SHD"
	Public Const ACTAutoNumberRangeCodeShc As String = "SHC"
	Public Const ACTAutoNumberRangeCodeDid As String = "DID"
	Public Const ACTAutoNumberRangeCodeDic As String = "DIC"
	Public Const ACTAutoNumberRangeCodeTrd As String = "TRD"
	Public Const ACTAutoNumberRangeCodeTrc As String = "TRC"
	'eck200801
	Public Const ACTAutoNumberRangeCodeIdr As String = "IDR"
	Public Const ACTAutoNumberRangeCodeIcr As String = "ICR"
	Public Const ACTAutoNumberRangeCodeIca As String = "ICA"
	'tomo050302
	Public Const ACTAutoNumberRangeCodeCla As String = "CLA"
	Public Const ACTAutoNumberRangeCodeClo As String = "CLO"
	'DD 29/11/2002: Instalments enhancements
	Public Const ACTAutoNumberRangeCodeIND As String = "IND"
	Public Const ACTAutoNumberRangeCodeINC As String = "INC"
	Public Const ACTAutoNumberRangeCodeIED As String = "IED"
	Public Const ACTAutoNumberRangeCodeIEC As String = "IEC"
	Public Const ACTAutoNumberRangeCodeIRD As String = "IRD"
	Public Const ACTAutoNumberRangeCodeIRC As String = "IRC"
	'ADO #39472: Instalment Claim Recovery range codes
	Public Const ACTAutoNumberRangeCodeICD As String = "ICD"
	Public Const ACTAutoNumberRangeCodeICC As String = "ICC"
	'KB PN 3036
	Public Const ACTAutoNumberRangeCodeSCD As String = "SCD"
    Public Const ACTAutoNumberRangeCodeCLC As String = "CLC"
	Public Const ACTAutoNumberProductCode As String = "ORION"
	
	' Navigator Batch
	Public Const ACTNavBatchFindTransToAllocation As String = "ACTALLOC"
	
	' Solution configurations
	Public Const ACTOrionSolutionValue As String = "OrionConfig"
	Public Const ACTOrionSolutionMBP As Integer = 1
	Public Const ACTOrionSolutionSFORB As Integer = 2
	Public Const ACTOrionMultiCurrency As String = "EnableMultiCurrency"
	
	'ECK 28/7/99
	Public Const ACTLookupCurrency As String = "currency"
	'EK 220300
	Public Const ACTLookupDepartment As String = "department"
	
	'MKW090603 PN4574.
	Public Const ACTLookupCostCentre As String = "CostCentre"
	
	' RAW 17/12/2002 : PS187 : Added
	Public Const ACTLookupReport As String = "Report"
	' RAW 17/12/2002 : PS187 : End
	
	'DD 19/04/2004
	'The Allocation array
	Public Const k_ACTransDetail_id As Integer = 0
	Public Const k_ACDocument_Ref As Integer = 1
	Public Const k_ACTypeCode As Integer = 2
	Public Const k_ACTaxBandCode As Integer = 3
	Public Const k_ACTransactionCurrencyID As Integer = 4
	Public Const k_ACTransactionCurrency As Integer = 5
	Public Const k_ACTransactionAmount As Integer = 6
	Public Const k_ACBaseCurrencyID As Integer = 7
	Public Const k_ACBaseCurrency As Integer = 8
	Public Const k_ACBaseOutstanding As Integer = 9
	Public Const k_ACBaseAllocated As Integer = 10
	Public Const k_ACWriteOff As Integer = 11
	Public Const k_ACCurrencyDifference As Integer = 12
	Public Const k_ACTTaxGroupCode As Integer = 13
	Public Const k_ACTAllocSequence As Integer = 14
    Public Const k_ACTAllocRule As Integer = 15
    Public Const k_ACTransDetailEx_id = 16

    Public Const ACTAutoNumberRangeCodeSRO = "SRO"
    Public Const ACTAutoNumberGroupCodeDocumentRef56 = "DOCREF56"
    Public Const ACTDocTypeRoundOff = 56

	
    Public Const k_ACAllocationArraySize As Integer = k_ACTransDetailEx_id
	
	'26/02/2003 - PWC - Enum used as structure for v_vTransDetail array in bACTManageDebtor.Approve
	'Referenced from iACTFindTransaction and bACTManageDebtors
	Public Enum eApprove_TransDetail
		TransdetailID
		DocumentId
		DocumentRef
		SourceID
		Amount
		CurrencyAmount
		OSAmount
		Spare
		AuditSetId
		AccountID
		CurrencyID
		CurrencyBaseXrate
	End Enum
	
	' CJB 060704 Folgate Development - Add comments array as a property of the cashlistitem
	' DD 25/03/2004 - Array index structure for CashListItem
	Public Enum eCashListItem
		CashlistitemID
		AllocationstatusID
		MediaTypeID
		MediaTypeIssuerID
		CashlistID
		AccountID
		MediaRef
		OurRef
		TheirRef
		Amount
		TransdetailID
		ContactName
		Address1
		Address2
		Address3
		Address4
		PostalCode
		AddressCountry
		PaymentName
		PaymentAccountCode
		PaymentBranchCode
		PaymentExpiryDate
		PaymentReference1
		PaymentReference2
		Letter
		Batch_id
		pmuser_id
		Transaction_Date
		Original_Amount
		Amount_Tendered
		Change
		CashListItem_receipt_type_id
		CashListItem_receipt_status_id
		CashListItem_bank_id
		Cheque_Date
		CC_Name
		CC_Number
		CC_Expiry_Date
		CC_Start_Date
		CC_Issue
		CC_Pin
		CC_Auth_Code
		CC_Customer
		CC_Manual_Auth_Code
		CC_Transaction_Code
		Receipt_Details
		CashListItem_Reverse_PMUser_id
		CashListItem_Reverse_Reason_id
		CashListItem_Payment_Type_id
		CashListItem_Payment_Status_id
		Date_Presented
		Cheque_in_Possession
		Stop_Requested_Date
		Stop_Printed_Date
		Stop_Confirmation_Date
		Reason
		Replaces_CashListItem_id
		XML_Object
		InstalmentArray
		SalvageArray
		CLMUSRecoveryArray
		CLMRVRecoveryArray
		UnderwritingYearID
		CurrencyBaseDate
		CurrencyBaseXrate
		AccountBaseDate
		AccountBaseXrate
		SystemBaseDate
		SystemBaseXrate
		OverrideReason
		CashListItem_Comments_Array
		PartyBankId
		CollectionDate
		Comments
		BGPolicies
		'WPR12- Enhancement Quote Collection Process
		BankLocation
		BankBranch
		ChequeTypeId
		CCBankId
		CardTypeId
		CardTransSlipNo
		ChequeClearingTypeId
        IsLeadAccount
        SplitTotal
        TaxBandId
        TaxAmount
        PMNavBatchKey
        BIC
		IBAN
		InsuranceRef
		LastItem
	End Enum

    Public Enum eClaimPayment
        kWorkClaimPayment = 0
        kClaimid = 1
        kClaimPerilId = 2
        kPaymentDate = 3
        kAmount = 4
        kTaxAmount = 5
        kTaxAmountWHT = 6
        kPartyCnt = 7
        kComments = 8
        kIsReferred = 9
        kCreatedBy = 10
        kPayeeMediaTypeId = 11
        kPayeeName = 12
        kBankName = 13
        kBankSortCode = 14
        kBankAccountNo = 15
        kPayeeCountryId = 16
        kPayeeComments = 17
        kSequenceNo = 18
        kTreatyId = 19
        kClaimPaymentTo = 20
        kPaymentPartyTo = 21
        kInsuredDomiciled = 22
        kInsuredPercentage = 23
        kInsuredTaxNumber = 24
        kPayeeDomiciled = 25
        kPayeePercentage = 26
        kPayeeTaxNumber = 27
        kSafeHarbourId = 28
        kSafeHarbourPercentage = 29
        kIsTaxExempt = 30
        kIsWHTExempt = 31
        kIsSettlement = 32
        kDocumentId = 33
        kIsLive = 34
        kLiveClaimPaymentId = 35
        kMediaRef = 36
        kCurrencyId = 37
        kExcessAmount = 38
        kPayeeAddress1 = 39
        kPayeeAddress2 = 40
        kPayeeAddress3 = 41
        kPayeeAddress4 = 42
        kPayeePostalCode = 43
        kThirdPartyReference = 44
        kChequeDate = 45
        kBankPaymentTypeId = 46
        kOurReference = 47
        kIsExGratia = 48
        kBIC = 49
        kIBAN = 50
        kLast
    End Enum


	Public Function CompanyBaseCurrency() As Integer
		'DD 24/09/2003: Current hard-coded Base Currency ID
		Return 26
	End Function
	
	' ***************************************************************** '
	' Name: ParseArray
	'
	' Description: Convert a 1-d array to pipe delimited string and vice
	' versa.
	' String Fromat will be "data1|data2|..dataX|"
	'
	' ***************************************************************** '
	Public Function ParseArray(ByRef vArray() As Object, ByRef sString As String, ByRef bArrayToString As Boolean) As Integer
		
		Dim result As Integer = 0
		Dim sTmp As New StringBuilder
		Dim iMax As Integer
		
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If bArrayToString Then
				'convert the array to string
				sString = ""
				If Not Information.IsArray(vArray) Then
					Return result
				End If
				
				For	Each vArray_item As Object In vArray

					sString = sString & CStr(vArray_item) & "|"
				Next vArray_item
				
			Else
				'convert the string to an array
				If sString.Trim() = "" Then
					vArray = Nothing
					Return result
				End If
				
				iMax = 0
				sTmp = New StringBuilder("")
				For i As Integer = 1 To sString.Length
					
					If sString.Substring(i - 1, 1) = "|" Then
						'add to the arrays
						If Information.IsArray(vArray) Then
							ReDim Preserve vArray(iMax)
						Else
							ReDim vArray(iMax)
						End If
						

						vArray(iMax) = sTmp.ToString()
						sTmp = New StringBuilder("")
						iMax += 1
					Else
						sTmp.Append(sString.Substring(i - 1, 1))
					End If
					
				Next i
				
			End If
			
			Return result
		
		Catch 
			
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	' Returns variant decimal signed for debit or credit
	Public Function ACTSigned(ByVal v_vAmount As Double, ByVal v_lDebitOrCredit As ACTEAccountSign) As Double
		Return Math.Abs(v_vAmount) * Math.Sign(v_lDebitOrCredit)
	End Function
End Module
