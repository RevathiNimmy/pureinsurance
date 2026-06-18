Option Strict Off
Option Explicit On
Module FormSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: FormSQL
    '
    ' Date: 09/09/1997
    '
    ' Description: Contains the SQL Statements required by the
    '              bACTBankAccount.Form class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select BankAccount SQL
    Public Const ACGetDetailsStored As Boolean = True
    Public Const ACGetDetailsName As String = "SelectBankAccount"
    'developer guide no.39
    Public Const ACGetDetailsSQL As String = "spu_ACT_Select_BankAccount"

    ' Select All BankAccount SQL
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllBankAccount"
    'developer guide no.39
    Public Const ACGetAllDetailsSQL As String = "spu_ACT_SelAll_BankAccount"

    ' Check ID SQL
    Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckBankAccountID"
    'developer guide no.39
    Public Const ACCheckIDSQL As String = "spu_ACT_Check_BankAccount"

    ' Add BankAccount SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddBankAccount"
    Public Const ACAddSQL As String = "spu_ACT_Add_BankAccount"

    ' Delete BankAccount SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteBankAccount"
    'developer guide no.39
    Public Const ACDeleteSQL As String = "spu_ACT_Delete_BankAccount"

    'PN9167 Check That Its OK to Delete BankAccount SQL
    Public Const ACCheckDeleteOkStored As Boolean = True
    Public Const ACCheckDeleteOkName As String = "DeleteBankAccount"
    'developer guide no.39
    Public Const ACCheckDeleteOkSQL As String = "spu_ACT_Get_BankAccount_Cash"

    'PN4619 If bank account has been used set as deleted
    Public Const ACSetBankAccountIsDeletedStored As Boolean = True
    Public Const ACSetBankAccountIsDeletedName As String = "SetBankAccountIsDeleted"
    'developer guide no.39
    Public Const ACSetBankAccountIsDeletedSQL As String = "spu_ACT_Set_BankAccount_IsDeleted"


    ' Update BankAccount SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateBankAccount"
    Public Const ACUpdateSQL As String = "spu_ACT_Update_BankAccount"


    'PN4619 If bank account has been used set as deleted
    Public Const ACCheckACcountStored As Boolean = True
    Public Const ACCheckACcountName As String = "CheckAccountMapped"
    'developer guide no.39
    Public Const ACCheckACcountSQL As String = "spu_ACT_Check_BankAccount_Mapping"



    ' Get Account Holder details SQL
    Public Const ACGetAccountHolderDetailsStored As Boolean = False
    Public Const ACGetAccountHolderDetailsName As String = "GETACCOUNTHOLDERDETAILS"
    Public Const ACGetAccountHolderDetailsSQL As String = "SELECT account_name FROM account WHERE account_id = "

    'sw bank reconciliation 31/01/2003
    Public Const ACGetBankAccountRulesName As String = "GetBankAccountRules"
    'developer guide no.39
    Public Const ACGetBankAccountRulesSQL As String = "spu_ACT_Get_BankAccount_Rules"

    Public Const ACAddBankAccountRuleName As String = "AddBankAccountRule"
    'developer guide no.39
    Public Const ACAddBankAccountRuleSQL As String = "spu_ACT_Add_BankAccount_Rule"

    Public Const ACUpdateBankAccountRuleName As String = "UpdateBankAccountRule"
    'developer guide no.39
    Public Const ACUpdateBankAccountRuleSQL As String = "spu_ACT_Update_BankAccount_Rule"

    Public Const ACDeleteBankAccountRuleName As String = "DeleteBankAccountRule"
    'developer guide no.39
    Public Const ACDeleteBankAccountRuleSQL As String = "spu_ACT_Delete_BankAccount_Rule"
    'developer guide no.39
    Public Const ACDeleteAllRulesForAccountSQL As String = "spu_ACT_DeleteAll_BankAccount_Rule"
    Public Const ACDeleteAllRulesForAccountName As String = "DeleteAllRulesForAccount"

    'end Sw bank reconciliation 31/01/2003

    Public Const ACGetBankStatementBalanceName As String = "GetBankAccountRules"
    'developer guide no.39
    Public Const ACGetBankStatementBalanceSQL As String = "spu_ACT_Get_Bank_Statement_Balance"

    Public Const ACUpdateBankStatementBalanceName As String = "AddBankAccountRule"
    'developer guide no.39
    Public Const ACUpdateBankStatementBalanceSQL As String = "spu_ACT_Update_Bank_Statement_Balance"


    'Task Account Function & CCY Cash Allocation

    Public Const ACGetAllSourcesLinkedWithBankAcStored As Boolean = True
    Public Const ACGetAllSourcesLinkedWithBankAcName As String = "Get All Sources Linked With BankAc"
    Public Const ACGetAllSourcesLinkedWithBankAcSQL As String = "spu_ACT_Get_All_Sources_Linked_With_BankAc"

    Public Const ACAddSourcesLinkedWithBankAcStored As Boolean = True
    Public Const ACAddSourcesLinkedWithBankAcName As String = "Add Sources Linked With BankAc"
    Public Const ACAddSourcesLinkedWithBankAcSQL As String = "spu_ACT_Add_Sources_Linked_With_BankAc"

    Public Const ACDelSourcesLinkedWithBankAcStored As Boolean = True
    Public Const ACDelSourcesLinkedWithBankAcName As String = "Del Sources Linked With BankAc"
    Public Const ACDelSourcesLinkedWithBankAcSQL As String = "spu_ACT_Del_Sources_Linked_With_BankAc"

    Public Const ACGetAllSourcesLinkedWithBankAcDefaultStored As Boolean = True
    Public Const ACGetAllSourcesLinkedWithBankAcDefaultName As String = "Get All Sources Linked With BankAcDefault"
    Public Const ACGetAllSourcesLinkedWithBankAcDefaultSQL As String = "spu_ACT_Get_All_Sources_Linked_With_BankAcDefault"

    Public Const ACSelAllBankAccountLinkedToSourceStored As Boolean = True
    Public Const ACSelAllBankAccountLinkedToSourceName As String = "Sel All Bank Account Linked To Source"
    Public Const ACSelAllBankAccountLinkedToSourceSQL As String = "spu_ACT_SelAll_BankAccountLinkedToSource"

    Public Const ACGetSourcesListForPickListStored As Boolean = True
    Public Const ACGetSourcesListForPickListName As String = "GetSourcesListForPickList"
    Public Const ACGetSourcesListForPickListSQL As String = "spu_ACT_Get_Sources_List_For_PickList"

    Public Const ACGetChequeForBankAccountStored As Boolean = True
    Public Const ACGetChequeForBankAccountName As String = "Get Cheque For BankAc"
    Public Const ACGetChequeForBankAccountSQL As String = "spu_get_ChequeExistForBankAccount"

End Module