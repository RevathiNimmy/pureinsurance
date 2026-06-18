Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("ACTConst_NET.ACTConst")> _
 Public Module ACTConst
	' ******************************************************************** '
	' Module Name: ACTConst (formerly ExtraConst)
	'
	' Date: 09 April 1997
	'
	' Description: Contains all of the additional global constants
	'               to be included into every WinAccs application.
	'
	' Edit History: 15 October 1998 - CF - Added AutoNumbering constants
	'               10 December 1998 - CF - Removed NAV Constants (again)
	'               05 March 2002 - Tom - Added CLA and CLO
	'
	' ******************************************************************** '
	
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
	
	Public Const ACTLookupCashListStatus As String = "CashListStatus"
	Public Const ACTCashListStatusEntered As Integer = 1
	Public Const ACTCashListStatusOpened As Integer = 2
	Public Const ACTCashListStatusClosed As Integer = 3
	
	'Possible values for AllocationTransType
	Public Const ACTPrimaryForAllocation As Integer = 1
	Public Const ACTSecondaryForAllocation As Integer = 2
	
	'Miscellaneous
	Public Const ACTDefaultSearchAmountTolerance As Integer = 5
	Public Const ACTUseListHidden As Integer = 2
	
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
	
	'MKW PN9003 Return Flag
	Public Const ACInsurerStopped As Integer = 1007
End Module