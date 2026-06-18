Option Strict Off
Option Explicit On
Module MainModule
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  26/09/00
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bSIRRenewal"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    'Constants for Renewal Statuses
    Public Const ACAwaitManReview As Integer = 1
    Public Const ACAwaitRenewalPrint As Integer = 2
    Public Const ACAwaitManRatingFail As Integer = 3
    Public Const ACPolicyDetailsChanged As Integer = 4
    Public Const ACAwaitUpdate As Integer = 5
    Public Const ACAwaitManRating As Integer = 6

    'Constants for Customer Document Array
    Public Const ACDocArrProductID As Integer = 0
    Public Const ACDocArrIsRenewable As Integer = 1
    Public Const ACDocArrIsOptionEnabled As Integer = 2
    Public Const ACDocArrRenewalStatusTypeID As Integer = 3
    Public Const ACDocArrInsuranceFileCnt As Integer = 4
    Public Const ACDocArrAwaitManReviewDocTemplateID_Msg As Integer = 5
    Public Const ACDocArrAwaitManReviewDocTemplateID_Att As Integer = 6
    Public Const ACDocArrAwaitInviteDocTemplateID_Msg As Integer = 7
    Public Const ACDocArrAwaitInviteDocTemplateID_Att As Integer = 8
    Public Const ACDocArrAwaitUpdateDocTemplateID_Msg As Integer = 9
    Public Const ACDocArrAwaitUpdateDocTemplateID_Att As Integer = 10
    Public Const ACDocArrAwaitManReviewDocTypeID_Att As Integer = 11
    Public Const ACDocArrAwaitInviteDocTypeID_Att As Integer = 12
    Public Const ACDocArrAwaitUpdateDocTypeID_Att As Integer = 13

    'Constants for Contact Array
    Public Const ACContactFieldContactCnt As Integer = 0
    Public Const ACContactFieldAreaCode As Integer = 1
    Public Const ACContactFieldNumber As Integer = 2
    Public Const ACContactFieldExtension As Integer = 3
    Public Const ACContactFieldDescription As Integer = 4
    Public Const ACContactFieldContactTypeID As Integer = 5
    Public Const ACContactFieldPartyResolvedName As Integer = 6

    ' Constants for the Renewals search data array indexes.
    Public Const ACIRenewalStatusId As Integer = 0
    Public Const ACIRenewalProduct As Integer = 1
    Public Const ACIRenewalInsuranceHolder As Integer = 2
    Public Const ACIRenewalShortname As Integer = 3
    Public Const ACIRenewalPartyType As Integer = 4
    Public Const ACIRenewalLivePolicy As Integer = 5
    Public Const ACIRenewalPolicyCnt As Integer = 6
    Public Const ACIRenewalPolicy As Integer = 7
    Public Const ACIRenewalInsuranceFolder As Integer = 8
    Public Const ACIRenewalInsuranceStructID As Integer = 9
    Public Const ACIRenewalStatusTypeId As Integer = 10
    Public Const ACIRenewalStatusType As Integer = 11
    Public Const ACIRenewalCriticalDate As Integer = 12
    Public Const ACIRenewalLivePolicyCnt As Integer = 13
    Public Const ACIRenewalCoverStartDate As Integer = 14
    Public Const ACIRenewalExpiryDate As Integer = 15
    Public Const ACIRenewalAgentCnt As Integer = 16
    Public Const ACIRenewalProductId As Integer = 17
    Public Const ACIRenewalDate As Integer = 18
    Public Const ACIRenewalLeadAgentCode As Integer = 19
    Public Const ACIRenewalAccHandlerCode As Integer = 20
    Public Const ACIRenewalSourceCode As Integer = 21
    Public Const ACIRenewalClaimsIndicator As Integer = 22
    Public Const ACIRenewalClosedBranch As Integer = 23
    Public Const ACIRenewalIsInTransferMode As Integer = 24
    Public Const ACIRenewalTransferToPartyCnt As Integer = 25
    Public Const ACIRenewalTransferToShortname As Integer = 26
    Public Const ACIRenewalProductIsTrueMonthlyPolicy As Integer = 27
    Public Const ACIRenewalAnniversaryCopy As Integer = 28
    Public Const ACIRenewalExceptionReason As Integer = 31
    Public Const kRenewalPolicyPaymentMethod As Integer = 32
    Public Const kOriginalPaymentMethod As Integer = 33
    Public Const kSystemOptionChaseCycleEnabled As Integer = 5096
    Public Const kSystemOptionPaymentHubEnabled As Integer = 5200
    'DN 21/02/03
    Public Const ACDOCTypeDebitNote As Integer = 3
    Public Const ACDocTypeSchedule As Integer = 4
    Public Const ACDocTypeCertificate As Integer = 5
    Public Const ACDocTypeLapse As Integer = 8

    Public Const ACLeadAgentCnt As Integer = 6
    Public Const ACEmailDocType As Integer = 8
    Public Const ACEMailSubTemplateCode As Integer = 12
    Public Const ACEMailAttachementTemplateCodes As Integer = 13
    Public Const ACCorrespondenceType As Integer = 14
    Public Const ACDefaultPreferredCorrespondenceType As Integer = 15
    Public Const ACIsAgentReceiveCorrespondence As Integer = 16

    Public Const PMKeyNameInsFileCnt As String = "insurance_file_cnt"
    Public Const PMKeyNamePaymentAccountID As String = "Payment Account ID"
    Public Const PMKeyNameDebitAgainst As String = "Debit Against"
    Public Const PMKeyNameCreditTransactions As String = "Credit Transactions"
    Public Const PMKeyNameCashListID As String = "Cash List ID"
    Public Const PMKeyNameCashListItemID As String = "Cash ListItem ID"
    Public Const PMKeyNameTransactionID As String = "TransactionID"
    Public Const PMKeyNameTransactionAmount As String = "TransactionAmount"
    Public Const PMKeyNameInsFolder As String = "insurance_folder_cnt" 'PN35753 --RC
    Public Const PMKeyNameInsFolderCnt As String = "insurance_folder_cnt"

    Public Const PMKeyNamePartyCnt As String = "party_cnt"

    Public Const PMKeyNameProductID As String = "Product_id"
    Public Const PMKeyNameProductCode As String = "product_code"

    ' CTAF 081298 - Orion Constants
    Public Const ACTKeyNameAccountID As String = "account_id"
    Public Const ACTKeyNameLedgerID As String = "ledger_id"
    Public Const ACTKeyNameLedgerTypeID As String = "ledger_type_id"
    Public Const ACTKeyNameShortCode As String = "short_code"
    Public Const ACTKeyNameFullKey As String = "full_key"
    Public Const ACTKeyNameAccountName As String = "account_name"
    Public Const ACTKeyNameDocumentRef As String = "document_ref"
    Public Const ACTKeyNameDocumentID As String = "document_id"
    Public Const ACTKeyNameDocumenttypeID As String = "documenttype_id"
    Public Const ACTKeyNameTransDetailID As String = "trans_detail_id"
    Public Const ACTKeyNameMappingId As String = "mapping_id"
    Public Const ACTKeyNameCashListTypeId As String = "cashlisttype_id"
    Public Const ACTKeyNameCashListId As String = "cashlist_id"
    Public Const ACTKeyNameCashListItemId As String = "cashlistitem_id"

    Public Const kIndexPaymentMethod As Integer = 0
    Public Const kIndexMediaType As Integer = 1
    Public Const kIndexTransactionId As Integer = 2
    Public Const kIndexIntegerationToken As Integer = 3
    Public Const kIndexTokenId As Integer = 4
    Public Const kIndexPremiumAmount As Integer = 5
    Public Const kIndexCurrencyCode As Integer = 6
    Sub Main_Renamed()

    End Sub
End Module
