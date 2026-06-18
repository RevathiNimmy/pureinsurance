Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("gHUBSpokeConstants_NET.gHUBSpokeConstants")> _
 Public Module gHUBSpokeConstants
	' ****************************************************************************** '
	' Module Name       :   gHUBSpokeConstants
	'
	' Date Created      :   22/11/2002
	'
	' Author            :   Raj Chanian
	'
	' Responsibilities  :   Common Constants for Hub-Spoke Architecture
	'
	' Edit History      :   27/11/2002 - PWC - Added Interface Code constants
	'                   :   28/11/2002 - RAW - Added constants for 'common' columns in Export DetailArray
	'                   :   14/01/2003 - PW  - Add 'CREDITCONTROL' interface code (PS218)
	'                   :   20/02/2003 - DD  - Added 'CREDITBALANCE' interface code (TS220)
	' ****************************************************************************** '
	
	'-------------------------------------------------------------------------------
	'Header / Detail Array Format
	'-------------------------------------------------------------------------------
	'Both arrays are 'jagged' and follow the following format.
	
	'Header Array Format
	'-------------------
	'1 Dimensional Array consisting of 2 elements
	'Element 0 = 1 Dimensional Array consisting of x elements representing names
	'Element 1 = 1 Dimensional Array consisting of x elements representing values
	
	'e.g.
	'Element 0 = (Name 01 , Name 02,  ... Name 0n )
	'Element 1 = (Value 01, Value 02, ... Value 0n)
	
	'Detail Array Format
	'-------------------
	'1 Dimensional Array consisting of 2 elements
	'Element 0 = 1 Dimensional Array consisting of x elements representing names
	'Element 1 = 2 Dimensional Array consisting of x,y elements representing values
	
	'e.g.
	'Element 0 = (Name 01 ,  Name 02, ... Name 0n )
	'Element 1 = (Value 01, Value 02, ... Value 0n)
	'            (Value 11, Value 12, ... Value 1n)
	'            (........, ........, ... ........)
	'            (Value n1, Value n2, ... Value nn)
	'
	'-------------------------------------------------------------------------------
	'############################################################################
	'Used By Hub and Spoke
	'Batch Statuses r_sStatusCode
	'Imports
	Public Const k_STATUS_SIRIUS_BATCH_IMPORT_COMPLETE As String = "SIRIUS_BATCH_IMPORT_COMPLETE"
	Public Const k_STATUS_SIRIUS_BATCH_IMPORT_FAILED As String = "SIRIUS_BATCH_IMPORT_FAILED"
	'Exports
	Public Const k_STATUS_SIRIUS_BATCH_EXPORT_COMPLETE As String = "SIRIUS_BATCH_EXPORT_COMPLETE"
	Public Const k_STATUS_SIRIUS_BATCH_EXPORT_FAILED As String = "SIRIUS_BATCH_EXPORT_FAILED"
	
	'Batch Messages r_sMessage
	'Imports
	Public Const k_MESSAGE_SIRIUS_BATCH_IMPORT_COMPLETE As String = "Batch Has Successfully Been Imported"
	Public Const k_MESSAGE_SIRIUS_BATCH_IMPORT_FAILED As String = "Batch Has Failed Import"
	Public Const k_MESSAGE_SIRIUS_BATCH_INCORRECT_IMPORT_INTERFACE As String = "Batch Failed To Import Successfully by Sirius, Incorrect Interface Passed for Import"
	Public Const k_MESSAGE_SIRIUS_BATCH_CHECK_TOTALS_MISMATCH As String = "Check Totals Mismatch"
	
	'Exports
	Public Const k_MESSAGE_SIRIUS_BATCH_EXPORT_COMPLETE As String = "Batch Has Successfully Been Exported"
	Public Const k_MESSAGE_SIRIUS_BATCH_EXPORT_FAILED As String = "Batch Has Failed Export"
	Public Const k_MESSAGE_SIRIUS_BATCH_INCORRECT_EXPORT_INTERFACE As String = "Batch Failed To Export Successfully by Sirius, Incorrect Interface Passed for Export"
	'Import/Export
	Public Const k_MESSAGE_SIRIUS_BATCH_UNKNOWN_INTERFACE As String = "Batch Failed To Import/Export Successfully by Sirius, Unknown Interface Passed To Spoke"
	
	
	'Record Statuses    r_vDetailData(x,y(Index1)) OptionBase0
	'                    x ColumnNames
	'                    y Column Values
	'Imports
	Public Const k_STATUS_SIRIUS_RECORD_BUSINESS_VALIDATION_FAILED As String = "SIRIUS_RECORD_BUSINESS_VALIDATION_FAILED"
	Public Const k_STATUS_SIRIUS_RECORD_TRANSACTION_COMMIT_FAILED As String = "SIRIUS_RECORD_TRANSACTION_COMMIT_FAILED"
	Public Const k_STATUS_SIRIUS_RECORD_TRANSACTION_COMMIT_SUCCESS As String = "SIRIUS_RECORD_TRANSACTION_COMMIT_SUCCESS"
	'Exports
	Public Const k_STATUS_SIRIUS_RECORD_EXPORT_SUCCESS As String = "SIRIUS_RECORD_EXPORT_SUCCESS"
	Public Const k_STATUS_SIRIUS_RECORD_EXPORT_FAILED As String = "SIRIUS_RECORD_EXPORT_FAILED"
	
	'Record Messages    r_vDetailData(x,y(Index2)) OptionBase0
	'                    x ColumnNames
	'                    y Column Values
	'Imports
	Public Const k_MESSAGE_SIRIUS_RECORD_BUSINESS_VALIDATION_FAILED As String = "Business Logic Has Been Violated for this Record Import"
	Public Const k_MESSAGE_SIRIUS_RECORD_TRANSACTION_COMMIT_FAILED As String = "Record Failed to Commit To Sirius"
	Public Const k_MESSAGE_SIRIUS_RECORD_TRANSACTION_COMMIT_SUCCESS As String = "Record Committed to Sirius Successfully"
	
	'Exports
	Public Const k_MESSAGE_SIRIUS_RECORD_EXPORT_SUCCESS As String = "Record Exported Successfully by Sirius"
	Public Const k_MESSAGE_SIRIUS_RECORD_EXPORT_FAILED As String = "Record Failed To Export Successfully by Sirius"
	'RKC 12/03/2002
	'r_vDetailData(1) Flag To See If Records Exported in This Index
	Public Const k_EXPORTED_RECORDS As Integer = 1
	'r_vDetailData(1) Flag To See If Records Imported in This Index
	Public Const k_IMPORTED_RECORDS As Integer = 1
	
	'Interface Codes
	Public Const ksICRecurring As String = "RECURRING"
	Public Const ksICOneOff As String = "ONEOFF"
	Public Const ksICRejections As String = "REJECTIONS"
	Public Const ksICCloseBatch As String = "CLOSEBATCH"
	Public Const ksICAutoBank As String = "AUTOBANK"
	Public Const ksIC3rdPartyCollect As String = "3RDPARTYCOLLECT"
	Public Const ksICExtractTrans As String = "EXTRACTTRANS"
	Public Const ksICPartyLoyaltyScheme As String = "LOYALTYEXPORT"
	Public Const ksICElectronicReceipting As String = "ELECRECEIPT"
	Public Const ksICPaymentRun As String = "PAYMENT_RUN"
	Public Const ksICCreditControl As String = "CREDITCONTROL"
	Public Const ksICBankStatement As String = "BANK_STMT"
	Public Const ksICSweepBalances As String = "SWEEP_BALANCES"
	Public Const ksICStaleCheques As String = "STALE_CHEQUES"
	Public Const ksICChequeReminder As String = "CHEQUE_REMINDER"
	Public Const ksICCreditBalance As String = "CREDIT_BALANCE"
	Public Const ksICCreditCardExpiryLetters As String = "CREDITCARD"
	Public Const ksICInstalmentsGeneration As String = "INST_GEN"
	Public Const ksICInstalmentsStatements As String = "INST_STMT"
	'sw added this in 01/04/2003
	Public Const ksICGeneralLedger As String = "GLEXPORT"
    Public Const ksICChaseCycle As String = "CHASECYCLE"
	
	
	'VOLUME TESTING INTERFACE CODES
	Public Const k_ZERO_RECORDS As String = "0RECS"
	Public Const k_ONE_RECORD As String = "1REC"
	Public Const k_TEN_RECORDS As String = "10RECS"
	Public Const k_ONE_HUNDRED_RECORDS As String = "100RECS"
	Public Const k_ONE_THOUSAND_RECORDS As String = "1000RECS"
	Public Const k_TEN_THOUSAND_RECORDS As String = "10000RECS"
	Public Const k_FIFTY_THOUSAND_RECORDS As String = "50000RECS"
	Public Const k_ONE_HUNDRED_THOUSAND_RECORDS As String = "100000RECS"
	Public Const k_TWO_HUNDRED_THOUSAND_RECORDS As String = "200000RECS"
	
	'RAW 28/11/2002: PS005: Added
	' position of standard columns in the DetailArray that the HUB expects
	' every export file to include
	Public Const klExportDetailColRecordNo As Integer = 0
	Public Const klExportDetailColStatusCode As Integer = 1
	Public Const klExportDetailColStatusMsg As Integer = 2
	'RKC 12/03/2002
	' position of standard columns in the DetailArray that the HUB expects
	' every Import file to expect
	Public Const klImportDetailColRecordNo As Integer = 0
	Public Const klImportDetailColStatusCode As Integer = 1
	Public Const klImportDetailColStatusMsg As Integer = 2
	
	'Payment run Detail array
	Public Const knPaymentRunDetailColAmount As Integer = 30
	Public Const knPaymentRunDetailColLevel As Integer = 31
	Public Const knPaymentRunDetailColAmalgamated As Integer = 32
	Public Const knPaymentRunDetailColTransDetailID As Integer = 25
	Public Const knPaymentRunDetailColCashListID As Integer = 33
	Public Const knPaymentRunDetailColCashListItemID As Integer = 34
	
	'Export Detail Data Array Elements
	'RECURRING/ONEOFF
	'DD 19/06/2003: Moved here for sharing with PFExport components
	Public Const eddDetailId As Integer = 0
	Public Const eddRecordStatus As Integer = 1
	Public Const eddRecordMessage As Integer = 2
	Public Const eddClientCode As Integer = 3
	Public Const eddClientName As Integer = 4
	Public Const eddAccountName As Integer = 5
	Public Const eddAmount As Integer = 6
	Public Const eddInsuranceFileRef As Integer = 7
	Public Const eddBankAccountNumber As Integer = 8
	Public Const eddBankSortCode As Integer = 9
	Public Const eddBankName As Integer = 10
	Public Const eddCreditCardName As Integer = 11
	Public Const eddCreditCardNumber As Integer = 12
	Public Const eddCreditCardExpiry As Integer = 13
	Public Const eddCreditCardStart As Integer = 14
	Public Const eddCreditCardIssue As Integer = 15
	Public Const eddCreditCardPin As Integer = 16
	Public Const eddTransactionID As Integer = 17
	Public Const eddAlternativeIdentifier As Integer = 18
	Public Const eddAgent As Integer = 19
	Public Const eddPFInstalmentGroupID As Integer = 20
	'DD 19/06/2003: Added for bSIRPFExport support
	Public Const eddPFInstalmentID As Integer = 21
	Public Const eddPFInstalmentNo As Integer = 22
	Public Const eddPFInstalmentStatusID As Integer = 23
	Public Const eddPFInstalmentStatusDescription As Integer = 24
	Public Const eddPFInstalmentTransStatusID As Integer = 25
	Public Const eddPFAutoGeneratedPlanRef As Integer = 26
	Public Const eddPFPlanTransactionID As Integer = 27
    Public Const eddPFInstalmentDueDate As Integer = 28
    Public Const eddPFInstalmentViaPaymentHub As Integer = 28

    '##############################################################################
End Module
