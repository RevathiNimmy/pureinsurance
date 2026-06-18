Option Strict Off
Option Explicit On
Module PMBTransactionsSQL
    ' ***************************************************************** '
    ' Class Name: bPMBTransactionsSQL
    '
    ' Date: 09/11/1998
    '
    ' Description: Contains the SQL Statements to populate the Transaction Export tables
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Add ExportFolder SQL
    'EK 26/11/99 Add extra parameter for Debit/credit
    Public Const ACAddPMBExportFolderStored As Boolean = True
    Public Const ACAddPMBExportFolderName As String = "AddExportFolder"
    'eck030500
    'eck060601
    'S4BDAT004

    Public Const ACAddPMBExportFolderSQL As String = "spu_PMB_trans_folder_add"

    ' Get Transaction Options
    Public Const ACSelectPMBTransactionOptionsStored As Boolean = False
    Public Const ACSelectPMBTransactionOptionsName As String = "SelectTransactionOptions"
    Public Const ACSelectPMBTransactionOptionsSQL As String = "SELECT field,comparison_indicator,comparison_value,stored_procedure," &
                                                              "return_value_1,return_value_2,return_value_3,return_value_4,return_value_5,' ' FROM transaction_options"

    'EK 16/11/99 Added extra parameter for sub-agents
    'EK 30/11/99 Added extra parameter for paid direct
    ' Get ExportProperties SQL
    'DC151204 added extra parameter for introducer_cnt
    Public Const ACSelectPMBExportPropertiesStored As Boolean = True
    Public Const ACSelectPMBExportPropertiesName As String = "SelectExportProcedureList"

    Public Const ACSelectPMBExportPropertiesSQL As String = "spu_pmb_trans_get_properties"

    ' Add ExportDetails SQL
    Public Const ACAddPMBExportDetailsControlStored As Boolean = True
    Public Const ACAddPMBExportDetailsControlName As String = "AddExportDetails"

    Public Const ACAddPMBExportDetailsControlSQL As String = "spu_pmb_trans_det_add_control"

    ' Select Transaction Data SQL
    Public Const ACSelectTransDataStored As Boolean = True
    Public Const ACSelectTransDataName As String = "SelectTransactionData"

    Public Const ACSelectTransDataSQL As String = "spu_pmb_select_transaction_data"

    Public Const ACSelectPMBExportNominalsSQL As String = "spu_pmb_trans_det_nominals"
    'EK 13/10/99
    ' Renumber ExportFolder SQL
    Public Const ACPMBExportFolderRenumberStored As Boolean = True
    Public Const ACPMBExportFolderRenumberName As String = "AddExportFolder"

    Public Const ACPMBExportFolderRenumberSQL As String = "spu_PMB_trans_folder_renumber"

    'EK 200300
    'eck080800 extra parameter for manual aj value
    ' Do Paid Direct SQL
    Public Const ACDoPaidDirectStored As Boolean = True
    Public Const ACDoPaidDirectName As String = "PostDirectDebitAJReversal"
    'eck291001 Extra parameter for document ref

    Public Const ACDoPaidDirectSQL As String = "spu_pmb_trans_det_direct"

    'DC260105 : Introducer Transaction Processing
    Public Const ACDoIntroducerStored As Boolean = True
    Public Const ACDoIntroducerName As String = "PostIntroducer"

    Public Const ACDoIntroducerSQL As String = "spu_pmb_trans_det_introducer"

    ' SR 24/03/00 Add ExportDetails SQL for Marsh
    Public Const ACAddMSHExportDetailsControlStored As Boolean = True
    Public Const ACAddMSHExportDetailsControlName As String = "AddExportDetails"

    Public Const ACAddMSHExportDetailsControlSQL As String = "spu_MSH_trans_det_add_control"

    'DC060801 -start -get deposit percentage
    Public Const ACGetIts4MePFDepositPCStored As Boolean = True
    Public Const ACGetIts4MePFDepositPCName As String = "GetIts4MePFDepositPC"

    Public Const ACGetIts4MePFDepositPCSQL As String = "sp_get_its4me_pf_deposit_pc"
    'DC060801 -end
    'DC060801 -start -get deposit bank account id
    Public Const ACGetIts4MeDepositAccountIdStored As Boolean = True
    Public Const ACGetIts4MeDepositAccountIdName As String = "GetIts4MeDepositBankAccountId"

    Public Const ACGetIts4MeDepositAccountIdSQL As String = "sp_get_its4me_deposit_bank_account_id"
    'DC060801 -end
    'DC060801 -start -get deposit bank account id
    Public Const ACGetIts4MePFAccountIdStored As Boolean = True
    Public Const ACGetIts4MePFAccountIdName As String = "GetIts4MePFAccountId"

    Public Const ACGetIts4MePFAccountIdSQL As String = "sp_get_its4me_pf_account_id" 'DC060801 -end
    'DC011104 PN14916 -check certain information so that it can be identified if a transaction will fail to post
    Public Const ACGetPostingCriteriaStored As Boolean = True
    Public Const ACGetPostingCriteriaName As String = "CheckPostingCriteria"

    Public Const ACGetPostingCriteriaSQL As String = "spu_check_posting_criteria"
    'DC261004 -end

    ' Select Terms of payment SQL
    Public Const ACGetTermsOfPaymentStored As Boolean = True
    Public Const ACGetTermsOfPaymentName As String = "GetTermsOfPaymetId"

    Public Const ACGetTermsOfPaymentSQL As String = "spu_terms_of_payment_sel"

    ' Credit Control
    Public Const ACAddCreditControlItemInsuranceFileStored As Boolean = True
    Public Const ACAddCreditControlItemInsuranceFileName As String = "spu_ACT_Add_Credit_Control_Item_InsFile"
    Public Const ACAddCreditControlItemInsuranceFileSQL As String = "spu_ACT_Add_Credit_Control_Item_InsFile(?,?)"

    Public Const AC_SQL_GetEventLogId_Stored As Boolean = True

    Public Const AC_SQL_GetEventLogId_Sql As String = "spu_pmb_event_log_id_sel"
    Public Const AC_SQL_GetEventLogId_Name As String = "GetEventLogId"

    Public Const ACCheckEventInsuranceFileHasZeroValueStored As Boolean = True
    Public Const ACCheckEventInsuranceFileHasZeroValueName As String = "CheckEventInsuranceFileHasZeroValue"
    Public Const ACCheckEventInsuranceFileHasZeroValueSQL As String = "spu_EventInsuranceFile_IsZeroValueEvent" 'Start - Prakash - WPR85_Paralleling
    Public Const ACGetCDAccountDetailsForPolicyStored As Boolean = True
    Public Const ACGetCDAccountDetailsForPolicyName As String = "GetCDAccountDetailsForPolicy"
    Public Const ACGetCDAccountDetailsForPolicySQL As String = "spu_Get_CD_Account_Details_For_Policy"

End Module