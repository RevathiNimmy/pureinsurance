Option Strict Off
Option Explicit On
Module FormSQL
    ' ***************************************************************** '
    ' Class Name: FormSQL
    '
    ' Date: 08/08/1997
    '
    ' Description: Contains the SQL Statements required by the
    '              bACTCashListItem.Form class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select CashListItem SQL
    Public Const ACGetDetailsStored As Boolean = True
    Public Const ACGetDetailsName As String = "SelectAllCashListItem"
    'developer guide no.39
    Public Const ACGetDetailsSQL As String = "spu_ACT_Select_CashListItem"

    ' Select All CashListItem SQL
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllCashListItem"
    'developer guide no.39
    Public Const ACGetAllDetailsSQL As String = "spu_ACT_SelAll_CashListItem"

    ' Check ID SQL
    Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckCashListItemID"
    'developer guide no.39
    Public Const ACCheckIDSQL As String = "spu_ACT_Check_CashListItem"

    ' Add CashListItem SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddCashListItem"
    Public Const ACAddSQL As String = "spu_ACT_Add_CashListItem"

    ' Delete CashListItem SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteCashListItem"
    'developer guide no.39
    Public Const ACDeleteSQL As String = "spu_ACT_Delete_CashListItem"

    ' Update CashListItem SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateCashListItem"
    Public Const ACUpdateSQL As String = "spu_ACT_Update_CashListItem"

    'eck1001701
    Public Const ACGetLetterDetailsStored As Boolean = False
    Public Const ACGetLetterDetailsName As String = "GetLetterDetails"
    Public Const ACGetLetterDetailsSQL As String = " SELECT p.party_cnt,p.shortname, d.document_ref FROM " & "party p ," & " account a, transdetail t, document d, cashlistitem c" & " WHERE a.account_id = {account_id}" & " AND t.transdetail_id = {transdetail_id}" & " AND c.account_id = a.account_id " & " AND c.transdetail_id = t.transdetail_id " & " AND t.document_id = d.document_id " & " AND a.account_key = P.party_cnt"

    ' Validate CashListItem SQL
    ' DD 16/05/2002
    Public Const ACValidateStored As Boolean = True
    Public Const ACValidateName As String = "ValidateCashListItem"
    'developer guide no.39
    Public Const ACValidateSQL As String = "spu_ACT_Do_CashListItem_Validate"

    'SW Front Office receipting 23-10-2002
    'developer guide no.39
    Public Const ACSelInstalmentsForAccountSQL As String = "spu_ACT_Select_Instalments_For_Account"
    Public Const ACSelInstalmentsForAccountName As String = "SelectInstalmentsForAccount"

    'sw front office receipting
    'developer guide no.39
    Public Const ACSelectCashlistItemInstalmentsSQL As String = "spu_ACT_Select_Instalments_For_CashListItem"
    Public Const ACSelectCashlistItemInstalmentsName As String = "SelectCashListItemInstalments"

    'sw front office receipting
    'developer guide no.39
    Public Const ACCreateCashlistItemInstalmentsSQL As String = "spu_ACT_Add_CashListItem_Instalment"
    Public Const ACCreateCashlistItemInstalmentsName As String = "CreateCashListItemInstalment"

    'sw front office receipting
    'developer guide no.39
    Public Const ACDeleteCashlistItemInstalmentsSQL As String = "spu_ACT_delete_cashlistitem_instalments"
    Public Const ACDeleteCashlistItemInstalmentsName As String = "DeleteCashListItemInstalments"

    'sw Payment maintenance 08-11-2002
    'developer guide no.39
    Public Const ACUpdateMediaRefSQL As String = "spu_ACT_Update_Media_Ref"
    Public Const ACUpdateMediaRefName As String = "UpdateMediaRef"

    'sw payment maintenance 12-11-2002
    'developer guide no.39
    Public Const ACUpdateStopChequeSQL As String = "spu_ACT_update_stop_cheque"
    Public Const ACUpdateStopChequeName As String = "UpdateStopCheque"

    'sw payment maintenance 13-11-2002
    'developer guide no.39
    Public Const ACUpdateStopChequeConfirmSQL As String = "spu_ACT_update_stop_cheque_confirm"
    Public Const ACUpdateStopChequeConfirmName As String = "UpdateStopChequeConfirm"

    'sw payment maintenance 14-11-2002
    'developer guide no.39
    Public Const ACUpdateCancelCashListItemSQL As String = "spu_ACT_Update_Cancel_CashListItem"
    Public Const ACUpdateCancelCashListItemName As String = "UpdateCancelCashListItem"

    'sw FOR 06-12-2002
    'developer guide no.39
    Public Const ACGetAccountAndUserGroupCodeSQL As String = "spu_ACT_Get_AccountAndUserGroupCode"
    Public Const ACGetAccountAndUserGroupCodeName As String = "GetAccountAndUserGroupCode"

    'sw electronic receipting 14/01/2003
    'developer guide no.39
    Public Const ACGetAdditionalFieldsSQL As String = "spu_ACT_Get_Additional_Fields"
    Public Const ACGetAdditionalFieldsName As String = "GetAdditionalFields"

    'sw 28/01/2003
    'developer guide no.39
    Public Const ACGetPaymentStatusIDFromCodeSQL As String = "spu_ACT_Get_Payment_Status_ID"
    Public Const ACGetPaymentStatusIDFromCodeName As String = "GetPaymentStatusIDFromCode"

    'sw 29/01/2003
    'developer guide no.39
    Public Const ACGetPartyCntFromAccountIDSQL As String = "spu_ACT_Get_PartyCnt_From_AccountID"
    Public Const ACGetPartyCntFromAccountIDName As String = "GetPartyCntFromAccountID"

    'PSL 11/04/2003
    Public Const ACGetInstalmentLetterDetailsStored As Boolean = True
    Public Const ACGetInstalmentLetterDetailsName As String = "GetLetterDetailsForInstalment"
    'developer guide no.39
    Public Const ACGetInstalmentLetterDetailsSQL As String = "spu_ACT_GetLetterDetailsForInstalment"
    Public Const ACGetReceiptTypeCodeStored As Boolean = True
    Public Const ACGetReceiptTypeCodeName As String = "ACT_Get_CashListItem_ReceiptType"
    'developer guide no.39
    Public Const ACGetReceiptTypeCodeSQL As String = "spu_ACT_Get_CashListItem_ReceiptType"
    Public Const ACSetLetterPrintedStored As Boolean = True
    Public Const ACSetLetterPrintedName As String = "ACT_CashListItem_SetPrinted"
    'developer guide no.39
    Public Const ACSetLetterPrintedSQL As String = "spu_ACT_CashListItem_SetPrinted"

    ' KG 11/09/2003 - Update status of Reversed Instalments
    Public Const ACUpdStatusOfReversedInstalmentName As String = "ACT_Update_StatusOfReversedInstalment"
    'developer guide no.39
    Public Const ACUpdStatusOfReversedInstalmentSQL As String = "spu_ACT_Update_StatusOfReversedInstalment"


    'to show the positions within the instalment details array
    Public Const ACPFInstalmentsID As Integer = 0
    Public Const ACInstalmentNumber As Integer = 3
    Public Const ACInstalmentFlagElement As Integer = 8
    Public Const ACInstalmentPartialPayment As Integer = 9

    'sw 30/04/2003 instalment receipt reversals
    'developer guide no.39
    Public Const ACGetInstalmentTransdetailIDsSQL As String = "spu_ACT_GetInstalmentTransID_FromCLIID"
    Public Const ACGetInstalmentTransdetailIDsName As String = "GetInstalmentTransIDFromCLIID"
    'developer guide no.39
    Public Const ACGetReceiptTypeCodeAndTransDetailIDSQL As String = "spu_ACT_Get_Receipt_Reversal_Details"
    Public Const ACGetReceiptTypeCodeAndTransDetailIDName As String = "Get_Receipt_Reversal_Details"

    'DC101003 -PN7393 -process payment documentation
    'developer guide no.39
    Public Const ACGetPaymentTypeCodeAndTransDetailIDSQL As String = "spu_ACT_Get_Payment_Reversal_Details"
    Public Const ACGetPaymentTypeCodeAndTransDetailIDName As String = "Get_Payment_Reversal_Details"

    'DD 21/10/2003
    'developer guide no.39
    Public Const ACCreateBatchRecordSQL As String = "spu_ACT_Add_Batch"
    Public Const ACCreateBatchRecordName As String = "AddNewBatchRecord"

    'DD 21/10/2003
    'developer guide no.39
    Public Const ACSelectBatchRecordSQL As String = "spu_ACT_Select_Batch_FromBatchRef"
    Public Const ACSelectBatchRecordName As String = "SelectBatchRecord"

    ' START CHANGES - Changed By: AAB  - Changed On: 25-Nov-2003 09:45
    Public Const ACCheckUserGroupStored As Boolean = True
    Public Const ACCheckUserGroupName As String = "Check Name Group Member"
    'developer guide no.39
    Public Const ACCheckUserGroupSQL As String = "spu_pmuser_is_name_member"

    Public Const ACGetDebtorGroupsStored As Boolean = True
    Public Const ACGetDebtorGroupsName As String = "Get ALL the Debtor groups for perticular type"
    'developer guide no.39
    Public Const ACGetDebtorGroupsSQL As String = "spu_Get_Debtor_User_Groups"

    Public Const ACGetApprovalRecordsStored As Boolean = True
    Public Const ACGetApprovalRecordsName As String = "Get the Approval Records for perticular payment"
    'developer guide no.39
    Public Const ACGetApprovalRecordsSQL As String = "spu_Approval_Records_Sel"

    Public Const ACApprovalStepDetailsStored As Boolean = True
    Public Const ACApprovalStepDetailsName As String = "Get the Approval User Group and Code for perticular Step"
    'developer guide no.39
    Public Const ACApprovalStepDetailsSQL As String = "spu_Get_Approval_Step_Details"

    Public Const ACIsUserUniqueStored As Boolean = True
    Public Const ACIsUserUniqueName As String = "Check if the user has approved a previous step"
    'developer guide no.39
    Public Const ACIsUserUniqueSQL As String = "spu_Check_Is_User_Unique"

    Public Const ACUserLimitsStored As Boolean = True
    Public Const ACUserLimitsName As String = "Get the User Claim & Regular Payments Limits"
    'developer guide no.39
    Public Const ACUserLimitsSQL As String = "spu_Get_User_Authority_Limit"

    Public Const ACAddPaymentApprovalRecordStored As Boolean = True
    Public Const ACAddPaymentApprovalRecordName As String = "Add a Record to the Payment_Approval Table"
    'developer guide no.39
    Public Const ACAddPaymentApprovalRecordSQL As String = "spu_Payment_Approval_Add"

    Public Const ACGetWTMInstanceCntStored As Boolean = True
    Public Const ACGetWTMInstanceCntName As String = "Get Work Task Instance Cnt Using Key name and key value."
    'developer guide no.39
    Public Const ACGetWTMInstanceCntSQL As String = "spu_get_pmwrk_task_instance_cnt"
    'developer guide no.39
    Public Const ACSelMediaTypeIssuerSQL As String = "spu_ACT_Select_MediaType_Issuer"
    Public Const ACSelMediaTypeIssuerName As String = "SelectMediaTypeIssuer"

    Public Const kGetClaimPaymentAccountsDetailsName As String = "Returns the required details for the claim payment made in this session"
    Public Const kGetClaimPaymentAccountsDetailsSQL As String = "spu_CLM_Get_Claim_Payment_Accounts_Details"

    Public Const kGetReceiptTypeDetailsName As String = "Returns details of the specified cashlistitem_receipt_type"
    Public Const kGetReceiptTypeDetailsSQL As String = "spu_ACT_Select_CashListItem_Receipt_Type_Details"

    Public Const kAddCashListItemClaimLinkName As String = "CashListItemClaimLinkAdd"
    Public Const kAddCashListItemClaimLinkSQL As String = "spu_CashListItem_claim_link_add"

    Public Const ACGetSourceBaseCurrencyStored As Boolean = True
    Public Const ACGetSourceBaseCurrencyName As String = "Get Source Base Currency"
    Public Const ACGetSourceBaseCurrencySQL As String = "spu_ACT_Get_Source_Base_Currency"

    Public Const kUpdateCashListItemStatusPendingName As String = "UpdateCashListItemPaymentStatus"
    Public Const kUpdateCashListItemStatusPendingSQL As String = "spu_upd_CLI_PaymentStatus_Pending"

    'Rahul GetCollectionDateOverrideAuthority
    Public Const ACGetCollectionDateOverrideAuthorityStored As Boolean = True
    Public Const ACGetCollectionDateOverrideAuthorityName As String = "GetCollectionDateOverrideAuthority"
    Public Const ACGetCollectionDateOverrideAuthoritySQL As String = "spu_Get_CollectionDate_Override_Authority"

    'Rahul
    'Public Const ACGetCollectionDateOverrideAuthorityStored = True
    'Public Const ACGetCollectionDateOverrideAuthorityName = "GetCollectionDateOverrideAuthority"
    Public Const ACGetDocumentIdFromInsuranceFileSQL As String = "spu_get_document_id_from_insurance_file"

    'Start - Sankar - PN 55288
    Public Const ACGetCashListReceiptTypeFromIDName As String = "GetCashListReceiptTypeFromID"
    Public Const ACGetCashListReceiptTypeFromIDSQL As String = "spu_SAM_Get_And_Validate_Field"
    'End - Sankar - PN 55288

    Public Const ACGetPMNavXMBatchTransactionDetailName As String = "GetPMNavXMBatchTransactionDetail"
    Public Const ACGetPMNavXMBatchTransactionDetailSQL As String = "spu_Get_PMNav_Batch_Transaction_Details"

    Public Const ACTUpdateWriteOffDocumentName As String = "ACUpdateWriteOffDocument"
    Public Const ACTUpdateWriteOffDocumentSQL As String = "spu_ACT_Update_WritOff_Document"

    Public Const kUpdateCashListForSplitReceiptStored As Boolean = True
    Public Const kTUpdaetCashListForSplitReceiptName As String = "UpdateCashListForSplitReceipt"
    Public Const kUpdaetCashListForSplitReceiptSQL As String = "spu_ACT_Update_CashList_For_SplitReceipt"

    Public Const kUpdateTransMatchCashListItemIdStored As Boolean = True
    Public Const kUpdateTransMatchCashListItemIdName As String = "UpdateTransMatchCashListItemId"
    Public Const kUpdateTransMatchCashListItemIdSQL As String = "spu_ACT_Update_TransMatch_CashListID"

    Public Const kUpdateCashListBatchIDStored As Boolean = True
    Public Const kUpdateCashListBatchIDName As String = "ACUpdateCashListBatchIDName"
    Public Const kUpdateCashListBatchIDSQL As String = "spu_ACT_Update_CashList_BatchId"

    Public Const kGetCashListBatchIDStored As Boolean = False
    Public Const kGetCashListBatchIDName As String = "GetCashListBatchID"
    Public Const kGetCashListBatchIDSQL As String = " SELECT pmnav_batch_key FROM " _
                                                     & "cashlist  " _
                                                     & " WHERE cashlist_id = {cashlist_id}"

    Public Const kCheckInsurerPaymentRoadMapStored As Boolean = False
    Public Const kCheckInsurerPaymentRoadMapName As String = "CheckInsurerPaymentRoadMap"
    Public Const kCheckInsurerPaymentRoadMapSQL As String = " SELECT cashlistitem_id FROM " _
                                                              & "TransMatch  " _
                                                              & " WHERE cashlistitem_id = {cashlistitem_id}"

    Public Const kGetandUpdateBatchTransDetailIDStored As Boolean = True
    Public Const kGetandUpdateBatchTransDetailIDName As String = "GetandUpdateBatchTransDetailID"
    Public Const kGetandUpdateBatchTransDetailIDSQL As String = "spu_ACT_Update_Batch_Transdetail_ID"

    Public Const kGetDocumentIDsByBatchStored As Boolean = True
    Public Const kGetDocumentIDsByBatchName As String = "GetandUpdateBatchTransDetailID"
    Public Const kGetDocumentIDsByBatchSQL As String = "spu_ACT_GetDocumentIDsByBatch"

    Public Const kGetAllocationDetailIDsStored As Boolean = True
    Public Const kGetAllocationDetailIDsName As String = "GetAllocationDetailIDs"
    Public Const kGetAllocationDetailIDsSQL As String = "spu_ACT_GetAllocationDetailIDs"

    Public Const kGetReinsurerAndRIPaymentRecoveriesDetailStored As Boolean = True
    Public Const kGetReinsurerAndRIPaymentRecoveriesDetailName As String = "GetReinsurerAndRIPaymentRecoveriesDetail"
    Public Const kGetReinsurerAndRIPaymentRecoveriesDetailSQL As String = "spu_get_details_for_taxes_over_paymentrecoveries"

    Public Const kGetTaxbandDetailForPaymentRecoveriesStored As Boolean = True
    Public Const kGetTaxbandDetailForPaymentRecoveriesName As String = "GetTaxbandDetailForPaymentRecoveries"
    Public Const kGetTaxbandDetailForPaymentRecoveriesSQL As String = "spu_get_tax_band_detail_for_payment_recoveries"

    Public Const kGetCashListDetailsStored As Boolean = True
    Public Const kGetCashListDetailsName As String = "Returns details of the specified cashlist"
    Public Const kGetCashListDetailsSQL As String = "spu_ACT_Select_CashList"
    Public Const kGetClaimPaymentDetailsByCashListItemIdName As String = "Returns claim payment details by cash list item "
    Public Const kGetClaimPaymentDetailsByCashListItemIdSQL As String = "spu_ACT_Get_ClaimPaymentDetails_By_CashListItemId"

    Public Const kCheckWriteOffReasonStored As Boolean = True
    Public Const kCheckWriteOffReason As String = "CheckWriteOffReason"
    Public Const kCheckWriteOffReasonSQL As String = "spu_ACT_SelAll_Write_Off_Reason"

    Public Const ACGetPoliciesForAccountSQL As String = "spu_ACT_Get_Policies_For_Account"
    Public Const ACGetPoliciesForAccountName As String = "GetPoliciesForAccount"

End Module