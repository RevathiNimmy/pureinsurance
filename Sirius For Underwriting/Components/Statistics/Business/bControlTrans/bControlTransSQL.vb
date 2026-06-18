Option Strict Off
Option Explicit On
Module ControlTransSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: ControlTransSQL
    '
    ' Date: 28/04/1997
    '
    ' Description: Contains the SQL Statements to manipulate the Stats and
    '              Transaction Export tables
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Update Insurance File System SQL
    Public Const ACUpdateSystemStored As Boolean = True
    Public Const ACUpdateSystemName As String = "UpdateInsuranceFileSystem"
    Public Const ACUpdateSystemSQL As String = "spu_upd_ins_file_system" ' PN - 58743

    ' Add StatsFolder SQL
    Public Const ACAddStatsFolderStored As Boolean = True
    Public Const ACAddStatsFolderName As String = "AddStatsFolder"
    'sj 03/04/2003 - start
    'Public Const ACAddStatsFolderSQL = "{call spu_add_stats_folder (?,?,?,?)}"

    Public Const ACAddStatsFolderSQL As String = "spu_add_stats_folder"
    'sj 03/04/2003 - end

    ' Add StatsDetails SQL
    Public Const ACAddStatsDetailsStored As Boolean = True
    Public Const ACAddStatsDetailsName As String = "AddStatsDetails"

    Public Const ACAddStatsDetailsSQL As String = "spu_add_stats_details_control"

    ' Add ExportFolder SQL
    Public Const ACAddExportFolderStored As Boolean = True
    Public Const ACAddExportFolderName As String = "AddExportFolder"

    Public Const ACAddExportFolderSQL As String = "spu_add_trans_export_folder"

    ' Add ExportDetails SQL
    Public Const ACAddExportDetailsStored As Boolean = True
    Public Const ACAddExportDetailsName As String = "AddExportDetails"

    Public Const ACAddExportDetailsSQL As String = "spu_add_trans_details_control"

    ' DD 5-2-2002 : Added
    ' Select PFGetTrans SQL
    Public Const ACGetPFTransactionsStored As Boolean = True
    Public Const ACGetPFTransactionsName As String = "GetPFTransactionsFromInsuranceFile"

    Public Const ACGetPFTransactionsSQL As String = "spu_PFGetTransactionsFromInsuranceFile"

    ' DD 7-2-2002 : Added
    Public Const ACGetPreviousFileStored As Boolean = True
    Public Const ACGetPreviousFileName As String = "GetPreviousInsuranceFileCnt"
    Public Const ACGetPreviousFileSQL As String = "spu_GetPreviousInsuranceFileCnt"

    'Thinh Nguyen 01/03/2002 (start)
    Public Const ACGetPlanInsuranceFileStored As Boolean = True
    Public Const ACGetPlanInsuranceFileName As String = "Get Plan Insurance File Cnt"

    Public Const ACGetPlanInsuranceFileSQL As String = "spu_GetPlanInsuranceFile"
    'Thinh Nguyen 01/03/2002 (end)

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'RAM20030305 -  Issue No. 2687  - Start
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Const ACAddCreditControlItemInsuranceFileStored As Boolean = True
    Public Const ACAddCreditControlItemInsuranceFileName As String = "Add_Credit_Control_Item_InsFile"

    Public Const ACAddCreditControlItemInsuranceFileSQL As String = "spu_ACT_Add_Credit_Control_Item_InsFile"

    Public Const ACDelCreditControlItemInsuranceFileStored As Boolean = True
    Public Const ACDelCreditControlItemInsuranceFileName As String = "Del_Credit_Control_Item_InsFile"

    Public Const ACDelCreditControlItemInsuranceFileSQL As String = "spu_ACT_Del_Credit_Control_Item_InsFile"

    Public Const ACUpdateCreditControlItemInsuranceFileStored As Boolean = True
    Public Const ACUpdateCreditControlItemInsuranceFileName As String = "Update_Credit_Control_Item_InsFile"

    Public Const ACUpdateCreditControlItemInsuranceFileSQL As String = "spu_ACT_Credit_Control_Item_Update_MTC"
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'RAM20030305 -  Issue No. 2687  - End
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'RAM20030313 -  Issue No. 2523  - Start
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Const ACGetTransDetailIDStored As Boolean = True
    Public Const ACGetTransDetailIDName As String = "GetTransDetailID"

    Public Const ACGetTransDetailIDSQL As String = "spu_ACT_Get_TransDetailID_AccountID"

    ' Retrieves payment setting for the current insurance file
    Public Const ACGetPaymentSettingStored As Boolean = True
    Public Const ACGetPaymentSettingName As String = "GetPaymentSetting"

    Public Const ACGetPaymentSettingSQL As String = "spu_get_payment_setting"

    ' Retrives the insurance_folder_cnt for a supplied insurance_file_cnt

    Public Const ACGetInsuranceFolderCntStored As Boolean = True
    Public Const ACGetInsuranceFolderCntName As String = "GetInsuranceFolderCnt"

    Public Const ACGetInsuranceFolderCntSQL As String = "spu_get_insurance_folder_cnt"

    ' Taken from bSIRListRisks
    Public Const ACGetOriginalPolicyCntStored As Boolean = True
    Public Const ACGetOriginalPolicyCntName As String = "GetOriginalPolicyCnt"

    Public Const ACGetOriginalPolicyCntSQL As String = "spu_get_original_pol_cnt"

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'RAM20030313 -  Issue No. 2523  - End
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Const ACGetNextOrionDocRefStored As Boolean = True
    Public Const ACGetNextOrionDocRefName As String = "ACGetNextOrionDocRef"

    Public Const ACGetNextOrionDocRefSQL As String = "spu_SIR_Get_NextOrionDocRef"

    Public Const ACCheckIfInstalmentDepositRequiredStored As Boolean = True
    Public Const ACCheckIfInstalmentDepositRequiredName As String = "ACGetNextOrionDocRef"

    Public Const ACCheckIfInstalmentDepositRequiredSQL As String = "spu_SIR_CheckInstalmentDepositRequired"

    'SMJB 12/09/03
    Public Const ACGetTransDetailForStatsReversalStored As Boolean = True
    Public Const ACGetTransDetailForStatsReversalName As String = "GetTransDetailForStatsReversal"
    Public Const ACGetTransDetailForStatsReversalSQL As String = "spu_ACT_Get_TransDetail_For_Stats_Reversal"

    Public Const ACGetDocumentRefStored As Boolean = False
    Public Const ACGetDocumentRefName As String = "GetDocumentRef"
    Public Const ACGetDocumentRefSQL As String = " Select Document.Document_ref From cashlistitem CLI " &
                                                 " LEFT JOIN TransDetail TD ON " &
                                                 " CLI.Transdetail_id = TD.Transdetail_id " &
                                                 " LEFT JOIN Document on TD.Document_id=Document.Document_id " &
                                                 " Where CLI.CashListItem_id = {CashListItem_id}"

    Public Const ACGetUserWriteOffLimitStored As Boolean = False
    Public Const ACGetUserWriteOffLimitName As String = "User Write Off Limit"
    Public Const ACGetUserWriteOffLimitSQL As String = "SELECT has_paynow_write_off_authority,paynow_write_off_amount FROM User_Authorities WHERE user_id = {User_ID}"

    Public Const ACDoCurrencyConversionStored As Boolean = True
    Public Const ACDoCurrencyConversionName As String = "DoCurrencyConversion"

    Public Const ACDoCurrencyConversionSQL As String = "spu_ACT_Do_Currency_Conversion"

    Public Const ACGetCurrencyIDFromTransDetailStored As Boolean = False
    Public Const ACGetCurrencyIDFromTransDetailName As String = "GetCurrencyIDFromTransDetail"
    Public Const ACGetCurrencyIDFromTransDetailSQL As String = "select currency_id From transdetail where transdetail_id={transdetail_id}"

    Public Const ACMoveSuspendedAgentCommissionName As String = "Move Suspended Agent Commission"
    Public Const ACMoveSuspendedAgentCommissionSQL As String = "spu_ACT_Move_Suspended_Agent_Commission"
    Public Const ACMoveSuspendedAgentCommissionStored As Boolean = True

    Public Const ACGetAccountIdFromShortcodeName As String = "GetAccountIdFromShortcode"
    Public Const ACGetAccountIdFromShortcodeSQL As String = "spu_Get_AccountIdFromShortCode"
    Public Const ACGetAccountIdFromShortcodeStored As Boolean = True

    Public Const ACGetInsuranceFileInformationStored As Boolean = True
    Public Const ACGetInsuranceFileInformationName As String = "GetInsuranceFileInformation"
    Public Const ACGetInsuranceFileInformationSQL As String = "spu_ACT_Get_Insurance_File_Information"

    Public Const ACInsertInsuranceFilePaymentDetailsStored As Boolean = True
    Public Const ACInsertInsuranceFilePaymentDetailsName As String = "InsertInsuranceFilePaymentDetails"
    Public Const ACInsertInsuranceFilePaymentDetailsSQL As String = "spu_ACT_Add_InsuranceFilePaymentDetails"

    Public Const ACGetDocumentFromTransdetailStored As Boolean = True
    Public Const ACGetDocumentFromTransdetailName As String = "GetDocumentFromTransdetail"
    Public Const ACGetDocumentFromTransdetailSQL As String = "spu_ACT_Get_Document_From_Transdetail"

    Public Const ACUpdateCashDepositPolicyLinkSQL As String = "spu_Update_CashDeposit_Policy_Link"
    Public Const ACUpdateCashDepositPolicyLinkName As String = "UpdateCashDepositPolicyLink"
    Public Const ACUpdateCashDepositPolicyLinkStored As Boolean = True

    Public Const ACGetTransDetailByDocStored As Boolean = True
    Public Const ACGetTransDetailByDocName As String = "GetTransDetailByDoc"
    Public Const ACGetTransDetailByDocSQL As String = "spu_ACT_Sel_TransDetail_By_Doc"


    Public Const ACAddChaseCycleItemInsuranceFileStored As Boolean = True
    Public Const ACAddChaseCycleItemInsuranceFileName As String = "Add_Chase_Cycle_Item_InsFile"
    Public Const ACAddChaseCycleItemInsuranceFileSQL As String = "spu_SIR_Add_Chase_Cycle_Item_InsFile"

    Public Const ACDelChaseCycleItemInsuranceFileStored As Boolean = True
    Public Const ACDelChaseCycleItemInsuranceFileName As String = "Del_Chase_Cycle_Item_InsFile"
    Public Const ACDelChaseCycleItemInsuranceFileSQL As String = "spu_SIR_Del_Chase_Cycle_Item_InsFile"

    Public Const ACAddClonedStatsDetailsStored As Boolean = True
    Public Const ACAddClonedStatsDetailsName As String = "AddClonedStatsDetails"
    Public Const ACAddClonedStatsDetailsSQL As String = "spu_Copy_Stats_for_Cloned_Reversal"

    Public Const ACAddPTStatsDetailsStored As Boolean = True
    Public Const ACAddPTStatsDetailsName As String = "AddPTStatsDetails"
    Public Const ACAddPTStatsDetailsSQL As String = "spu_Copy_Stats_for_PT_Reversal"

    Public Const kAddTransDetailExStored As Boolean = True
    Public Const kAddTransDetailExName As String = "AddTransDetailEx"
    Public Const kAddTransDetailExSQL As String = "spu_ACT_Add_TransDetailEx"

    Public Const kCopyClonedExportDetailsStored As Boolean = True
    Public Const kCopyClonedExportDetailsName As String = "CopyClonedExportDetails"
    Public Const kCopyClonedExportDetailsSQL As String = "spu_Copy_TransExportDetail_for_Cloned_Reversal"

    Public Const kGetThisPremiumStored As Boolean = True
    Public Const kGetThisPremiumName As String = "GetThisPremium"
    Public Const kGetThisPremiumSQL As String = "spu_ACT_GetThisPremium"
    Public Const ACGetPolicyIntermediaryAgentAccountStored As Boolean = True
    Public Const ACGetPolicyIntermediaryAgentAccountSQL As String = "spu_Get_Policy_Intermediary_Agent_Account"
    Public Const ACGetPolicyIntermediaryAgentAccountName As String = "GetPolicyIntermediaryAgentAccount"

    Public Const ACCopyStatsDetailsRevStored As Boolean = True
    Public Const ACCopyStatsDetailsRevName As String = "CopyStatsReversal"
    Public Const ACCopyStatsDetailsRevSQL As String = "spu_Copy_Stats_Reversal"

End Module
