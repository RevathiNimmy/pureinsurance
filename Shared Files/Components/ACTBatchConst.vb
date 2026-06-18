Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("ACTBatchConst_NET.ACTBatchConst")> _
 Public Module ACTBatchConst
	' ***************************************************************** '
	'
	' PMAccounts application contants module.
	' Contains all of constants relating to Batch which
	' have a multi-component span, within Orion.
	'
	' ***************************************************************** '
	
	
	' ***************************************************************** '
	' Constants
	'
	' The constants below are grouped by originating application:
	'
	'
	' Batch
	' *****
	'
	' Field positions within data variant arrays
	' for Batch.ProcessCompleteBatch ...
	'
	' Inputs
	Public Const ACTBatchRef As Integer = 0
	Public Const ACTBatchCompanyID As Integer = 1
	Public Const ACTBatchCreatedBy As Integer = 2
	Public Const ACTBatchComment As Integer = 3
	Public Const ACTBatchDocuments As Integer = 4
	
	Public Const ACTDocumentRef As Integer = 0
	Public Const ACTDocumentType As Integer = 1
	Public Const ACTDocumentDate As Integer = 2
	Public Const ACTDocumentAccountingDate As Integer = 3
	Public Const ACTDocumentComments As Integer = 4
	Public Const ACTDocumentTransactions As Integer = 5
	Public Const ACTDocumentStatus As Integer = 6
	Public Const ACTDocumentCompany As Integer = 7
	
	Public Const ACTTransDetailAccountID As Integer = 0
	Public Const ACTTransDetailCurrencyID As Integer = 1
	Public Const ACTTransDetailAmount As Integer = 2
	Public Const ACTTransDetailCurrencyAmount As Integer = 3
	Public Const ACTTransDetailCurrencyBaseXrate As Integer = 4
	Public Const ACTTransDetailThirdCurrency As Integer = 5
	Public Const ACTTransDetailThirdCurrencyAmount As Integer = 6
	Public Const ACTTransDetailThirdCurrencyBaseXrate As Integer = 7
	Public Const ACTTransDetailComment As Integer = 8
	Public Const ACTTransDetailInsuranceRef As Integer = 9
	Public Const ACTTransDetailOperatorID As Integer = 10
	Public Const ACTTransDetailPurchaseOrderNo As Integer = 11
	Public Const ACTTransDetailPurchaseInvoiceNo As Integer = 12
	Public Const ACTTransDetailDepartment As Integer = 13
	Public Const ACTTransDetailSpare As Integer = 14
	
	Public Const ACTTransDetailRefDate As Integer = 15
	Public Const ACTTransDetailRefAmount As Integer = 16
	Public Const ACTTransDetailRefQuantity As Integer = 17
	Public Const ACTTransDetailRefUnits As Integer = 18
	Public Const ACTTransDetailTaxesTotal As Integer = 19
	Public Const ACTTransDetailDepartmentId As Integer = 20
	
	'DC020806 add new transdetail type id
	Public Const ACTTransdetailTypeId As Integer = 21
	
	'Outputs
	Public Const ACTBatchId As Integer = 0
	Public Const ACTDocumentId As Integer = 1
	Public Const ACTTransactionId As Integer = 2
	
	' Constants used to define Import Transaction array
	Public Const ACTTransImportParentNode As Integer = 0
	Public Const ACTTransImportLedgerCode As Integer = 1
	Public Const ACTTransImportAccountTypeCode As Integer = 2
	Public Const ACTTransImportCurrencyAmount As Integer = 3
	Public Const ACTTransImportDescription As Integer = 4
	Public Const ACTTransImportRelativeCode As Integer = 5
	Public Const ACTTransImportInsuranceRef As Integer = 6
	Public Const ACTTransImportOperatorID As Integer = 7
	Public Const ACTTransImportPurchaseOrderNo As Integer = 8
	Public Const ACTTransImportPurchaseInvoiceNo As Integer = 9
	Public Const ACTTransImportDepartment As Integer = 10
	Public Const ACTTransImportSpare As Integer = 11
	Public Const ACTTransImportAccountKey As Integer = 12
	Public Const ACTTransImportAccountID As Integer = 13
	Public Const ACTTransImportIPTTotal As Integer = 14
	Public Const ACTTransImportVATTotal As Integer = 15
	Public Const ACTTransImportUWYearID As Integer = 16
	Public Const ACTTransImportSuspended As Integer = 17
	Public Const ACTTransImportReleaseToIncome As Integer = 18
	Public Const ACTTransImportReleaseAccountCode As Integer = 19
	Public Const ACTTransImportTransdetailTypeCode As Integer = 20
	Public Const ACTTransImportTaxGroupID As Integer = 21
	Public Const ACTTransImportTaxBandID As Integer = 22
	'(RC) PLICO 9-10
	Public Const ACTTransImportManuallyReleased As Integer = 23
	Public Const ACTTransImportReleasedOnFullSettlement As Integer = 24
	Public Const ACTTransImportReleasedForWholePosting As Integer = 25
	Public Const ACTTransImportReleasedOnPolicyEffective As Integer = 26
    Public Const kTTransImportFeeType As Integer = 27
    Public Const ACTTransImportArraySize As Integer = 27
	
	' Temporary results array
	Public Const ACCTTempResultsAccountTypeCode As Integer = 0
	Public Const ACCTTempResultsTransactionLedgerCode As Integer = 1
	Public Const ACCTTempResultsTransactionAmount As Integer = 2
	Public Const ACCTTempResultsMapping_code As Integer = 3
	Public Const ACCTTempResultsTransactionAccountKey As Integer = 4
	Public Const ACCTTempResultsSpare As Integer = 5
	Public Const ACCTTempResultsTaxesTotal As Integer = 6
	Public Const ACCTTempResultsChargesTotal As Integer = 7
	Public Const ACCTTempResultsPurchaseOrderNo As Integer = 8
	Public Const ACCTTempResultsPurchaseInvoiceNo As Integer = 9
	Public Const ACCTTempResultsUWYearID As Integer = 10
	Public Const ACCTTempResultsSuspended As Integer = 11
	Public Const ACCTTempResultsReleaseToIncome As Integer = 12
	Public Const ACCTTempResultsReleaseAccountCode As Integer = 13
	Public Const ACCTTempResultsTransdetailTypeCode As Integer = 14
	Public Const ACCTTempResultsTaxGroupID As Integer = 15
	Public Const ACCTTempResultsTaxBandID As Integer = 16
	Public Const ACCTTempResultsManuallyReleased As Integer = 17
	Public Const ACCTTempResultsReleasedOnFullSettlement As Integer = 18
	Public Const ACCTTempResultsReleasedForWholePosting As Integer = 19
    Public Const ACCTTempResultsReleasedOnPolicyEffective As Integer = 20
    Public Const kCTTempResultsFeeType = 21
    Public Const ACCTTempResultsLastItem As Integer = 21
   
End Module