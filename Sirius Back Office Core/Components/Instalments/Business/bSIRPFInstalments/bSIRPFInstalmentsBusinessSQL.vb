Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: BusinessSQL
    '
    ' Date: 3 Aug 2001
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRPFInstalments.Business class.
    '
    ' Edit History:
    ' PSL 12/11/2002 Added Parameters to Add, Edit, select instalments
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select All PFInstalments SQL
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "Select All PFInstalments"
    Public Const ACGetAllDetailsSQL As String = "spu_PFInstalments_saa"

    ' Select All PFInstalment_Status records SQL
    Public Const ACGetAllStatusStored As Boolean = True
    Public Const ACGetAllStatusName As String = "Select All PFInstalment Status Records"
    Public Const ACGetAllStatusSQL As String = "spe_PFInstalments_Status_sel"

    ' Select All PFInstalment_Transaction records SQL
    Public Const ACGetAllTransCodeStored As Boolean = True
    Public Const ACGetAllTransCodeName As String = "Select All PFInstalment Transaction Records"
    Public Const ACGetAllTransCodeSQL As String = "spe_PFInstalments_Transaction_sel"

    ' Create Batch SQL
    Public Const ACCreateBatchStored As Boolean = True
    Public Const ACCreateBatchName As String = "Create PFInstalments Batch"
    Public Const ACCreateBatchSQL As String = "spe_PFInstalments_createbatch"

    ' Select Batch SQL
    ' similar to spe_PFInstalments_saa but does a lookup on the
    ' status and transaction code tables.
    Public Const ACBatchSelectStored As Boolean = True
    Public Const ACBatchSelectName As String = "Select PFInstalments Batch"
    Public Const ACBatchSelectSQL As String = "spe_PFInstalments_selectbatch"

    ' Post Item SQL
    Public Const ACPostItemStored As Boolean = True
    Public Const ACPostItemName As String = "Post PFInstalment Item"
    Public Const ACPostItemSQL As String = "spe_PFInstalments_postitem"

    ' Recall Item SQL

    Public Const kClearInstalmentsTransactionDetailsName As String = "Recall PFInstalment Item"
    Public Const kClearInstalmentsTransactionDetailsSQL As String = "spe_PFInstalments_recallitem"

    ' Check ID SQL
    Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckPFInstalmentsID"
    Public Const ACCheckIDSQL As String = "spe_PFInstalments_check_id"

    'TF151002 - Get details required for Alternative Payment Method
    Public Const ACGetPolicyDetailsStored As Boolean = True
    Public Const ACGetPolicyDetailsName As String = "GetPolicyDetails"
    Public Const ACGetPolicyDetailsSQL As String = "spu_PFGet_Policy_Details"

    'TF151002 - Update Document Comment with Alternative Payment Method
    Public Const ACUpdateDocumentCommentStored As Boolean = True
    Public Const ACUpdateDocumentCommentName As String = "ACUpdateDocumentComment"
    Public Const ACUpdateDocumentCommentSQL As String = "spu_PFUpdate_Document_Comment"

    ' Select PFInstalments SQL
    Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSinglePFInstalments"
    Public Const ACSelectSingleSQL As String = "spu_PFInstalments_sel"

    ' Add PFInstalments SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddPFInstalments"
    Public Const ACAddSQL As String = "spu_PFInstalments_add"

    ' Delete PFInstalments SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeletePFInstalments"
    Public Const ACDeleteSQL As String = "spe_PFInstalments_del"

    ' Update PFInstalments SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdatePFInstalments"
    Public Const ACUpdateSQL As String = "spu_PFInstalments_upd"

    'SW 22/01/2003 called from PostNonBatchInstalments
    Public Const ACSelectInstalmentPostDetailsSQL As String = "spu_ACT_Select_Instalment_Post_Details"
    Public Const ACSelectInstalmentPostDetailsName As String = "SelectInstalmentPostDetails"

    'sw 23/01/2003 Update instalment record after posting
    Public Const ACUpdateInstalmentAsPostedSQL As String = "spu_ACT_Update_Instalment_As_Posted"
    Public Const ACUpdateInstalmentAsPostedName As String = "UpdateInstalmentAsPosted"

    ' Alix - 31/03/2003 - Select policy start date
    Public Const ACSelectPolicyStartDateStored As Boolean = True
    Public Const ACSelectPolicyStartDateName As String = "SelectPolicyStartDate"
    Public Const ACSelectPolicyStartDateSQL As String = "spu_get_policy_start_date"

    Public Const ACMarkInstalmentStatusStored As Boolean = True
    Public Const ACMarkInstalmentStatusName As String = "Mark Instalment"
    Public Const ACMarkInstalmentStatusSQL As String = "spu_SIR_MarkInstalmentStatus"

    Public Const ACGetPlanPKStored As Boolean = True
    Public Const ACGetPlanPKName As String = "Get Plan Primary Key"
    Public Const ACGetPlanPKSQL As String = "spu_SIR_GetPlanPK"

    Public Const ACGetPlanPaidStored As Boolean = True
    Public Const ACGetPlanPaidName As String = "Get Plan Paid"
    Public Const ACGetPlanPaidSQL As String = "spu_SIR_GetPlanPaid"

    ' Delete CreditControlItem
    Public Const ACDeleteCreditControlItemStored As Boolean = True
    Public Const ACDeleteCreditControlItemName As String = "Delete Credit Control Item Using Instalment ID"
    Public Const ACDeleteCreditControlItemSQL As String = "spu_Delete_Credit_Control_Item_Using_PFID"

    ' AAB - Aug 12, 2003 added to support Partial payments
    Public Const ACPartialPaymentInstalmentsStored As Boolean = True
    Public Const ACPartialPaymentInstalmentsName As String = "spu_ACT_Process_Partial_Payments_For_Instalments"
    Public Const ACPartialPaymentInstalmentsSQL As String = "spu_ACT_Process_Partial_Payments_For_Instalments"

    Public Const ACPartialPaymentCCIStored As Boolean = True
    Public Const ACPartialPaymentCCIName As String = "spu_ACT_Process_Partial_Payments_For_CCI"
    Public Const ACPartialPaymentCCISQL As String = "spu_ACT_Process_Partial_Payments_For_CCI"

    Public Const kGetInstalmentsFromBatchSQL As String = "spe_PFInstalments_selectbatch"
    Public Const kGetInstalmentsFromBatchName As String = "spe_PFInstalments_selectbatch"

    Public Const kRetryInstalmentCollectionSQL As String = "spu_PFInstalments_Retry"
    Public Const kRetryInstalmentCollectionName As String = "spu_PFInstalments_Retry"

    Public Const kCreateInstalmentHistoryItemSQL As String = "spu_ACT_Create_PFInstalment_History_Item"

    Public Const kGetMediaHistoryIdSQL As String = "spu_get_mediahistory_id"
    Public Const kGetMediaHistoryIdName As String = "Get Media History Id"

    Public Const ACSelectProductTypefromPFSQL As String = "spu_PF_GetProductType"
    Public Const ACSelectProductTypefromPFName As String = "SelectProductTypefromPF"

    Public Const ACSelectFirstInstalmentStatusPFSQL As String = "spu_PF_GetFirstInstalmentStatus"
    Public Const ACSelectFirstInstalmentStatusPFName As String = "SelectPFFirstInstalmentStatus"

    Public Const ACGetPlanOutstandingAmountSQL As String = "spu_get_plantransaction_outstanding_amount"
    Public Const ACGetPlanOutstandingAmountName As String = "GetPlanOutstandingAmount"

    Public Const kSelectWriteOffReasonSQL As String = "spu_ACT_Select_Write_Off_Reason"
    Public Const kSelectWriteOffReasonName As String = "SelectWriteOffReasonDescription"

    Public Const kCanInstalmentBeRecalledSQL As String = "spu_PFInstalments_CanInstalmentBeRecalled"
    Public Const kCanInstalmentBeRecalledName As String = "CanInstalmentBeRecalled"

    Public Const kGetInstalmentForRecallSQL As String = "spu_PFInstalments_GetInstalmentForRecall"
    Public Const kGetInstalmentForRecallName As String = "spu_PFInstalments_GetInstalmentForRecall"

    Public Const kGetPfInstalmentsResultIdSQL As String = "spu_PFInstalments_Result_Sel_By_Code"
    Public Const kGetPfInstalmentsResultIdName As String = "Get pfinstalments_result_id"

    Public Const kUpdateInstalmentResultSQL As String = "spu_PfInstalments_Result_Update"
    Public Const kUpdateInstalmentResultName As String = "update the pfinstalments_result_id on the pfinstalments record"

    Public Const kDeleteCashListItemInstalmentsSQL As String = "spu_ACT_DeleteCashlistitem_Instalments"
    Public Const kDeleteCashListItemInstalmentsName As String = "DeleteCashListItemInstalments"

    Public Const kGetFinancePlanDetailsSQL As String = "spu_PF_GetFinancePlanDetails"
    Public Const kGetFinancePlanDetailsSQLName As String = "GetFinancePlanDetailsSQL"

    Public Const ACGetPFFromInstalmentsIDStored As Boolean = True
    Public Const ACGetPFFromInstalmentsIDSQL As String = "spu_ACT_GetPFFromInstalmentsID"
    Public Const ACGetPFFromInstalmentsIDName As String = "Get PF Scheme Proeprties for the Instalment ID"

    Public Const ACPFPremiumFinanceUpdateStatusStored As Boolean = True
    Public Const ACPFPremiumFinanceUpdateStatusName As String = "PFPremiumFinanceUpdateStatus"
    Public Const ACPFPremiumFinanceUpdateStatusSQL As String = "spu_PFPremiumFinance_UpdateStatus"

    Public Const kGetInstalmentPaymentHubDetailsStored As Boolean = True
    Public Const kGetInstalmentPaymentHubDetailsName As String = "Get_Instalment_Payment_Hub_Details"
    Public Const kGetInstalmentPaymentHubDetailsSQL As String = "spu_SIR_Get_Instalment_Payment_Hub_Details"


    Public Const ACGetPFTransactionIDtored As Boolean = True
    Public Const ACGetPFTransactionIDSQL As String = "spu_ACT_GetPFTransactionID"
    Public Const ACGetPFTransactionIDName As String = "Get PF Transaction Id for the Instalment ID"

    Public Const ACGetInstalmentsRemainingStored As Boolean = True
    Public Const ACGetInstalmentsRemainingName As String = "Get Instalments Remaining"
    Public Const ACGetInstalmentsRemainingSQL As String = "spu_Get_InstalmentsAndDeposit_Remaining"
End Module