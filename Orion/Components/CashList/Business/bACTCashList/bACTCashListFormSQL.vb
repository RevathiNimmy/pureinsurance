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
    ' Date: 03/09/1997
    '
    ' Description: Contains the SQL Statements required by the
    '              bACTCashList.Form class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Select CashList SQL
    Public Const ACGetDetailsStored As Boolean = True
    Public Const ACGetDetailsName As String = "SelectCashList"
    Public Const ACGetDetailsSQL As String = "spu_ACT_Select_CashList"

    ' Select All CashList SQL
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllCashList"
    Public Const ACGetAllDetailsSQL As String = "spu_ACT_SelAll_CashList"

    ' Check ID SQL
    Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckCashListID"
    Public Const ACCheckIDSQL As String = "spu_ACT_Check_CashList"

    ' Add CashList SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddCashList"
    Public Const ACAddSQL As String = "spu_ACT_Add_CashList"

    ' Delete CashList SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteCashList"
    Public Const ACDeleteSQL As String = "spu_ACT_Delete_CashList"

    ' Update CashList SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateCashList"
    Public Const ACUpdateSQL As String = "spu_ACT_Update_CashList"

    ' Select Bank Account Default SQL
    Public Const ACGetBankDefaultStored As Boolean = True
    Public Const ACGetBankDefaultName As String = "SelectBankDefaultCashList"
    Public Const ACGetBankDefaultSQL As String = "spu_ACT_bank_default_Cashlist"

    '
    Public Const ACGetCashListByCashDrawerIdName As String = "GetCashListByCashDrawerId"
    Public Const ACGetCashListByCashDrawerIdSQL As String = "spu_ACT_Select_CashListByCashDrawerId"

    Public Const ACGetAllUserCashListDrawerName As String = "GetAllUserCashListDrawer"
    Public Const ACGetAllUserCashListDrawerSQL As String = "spu_ACT_SelAll_User_CashListDrawer"

    Public Const ACGetLocksName As String = "GetLocks"
    Public Const ACGetLocksSQL As String = "spu_ACT_Select_Locks"

    Public Const ACGetUserBankingAuthorisationName As String = "GetUserBankingAuthorisation"
    Public Const ACGetUserBankingAuthorisationSQL As String = "spu_ACT_Select_UserBankingAuthorisation"

    Public Const ACGetCurrencyDenominationsName As String = "GetCurrencyDenominations"
    Public Const ACGetCurrencyDenominationsSQL As String = "spu_ACT_Get_Cur_Denominations"

    Public Const ACGetBankingItemsName As String = "GetBankingItems"
    Public Const ACGetBankingItemsSQL As String = "spu_ACT_Get_Banking_Items"

    Public Const ACAddAdjustmentName As String = "AddAdjustment"
    Public Const ACAddAdjustmentSQL As String = "spu_ACT_Add_Adjustment"

    Public Const ACCheckCashFloatName As String = "CheckCashFloat"
    Public Const ACCheckCashFloatSQL As String = "spu_ACT_Check_CashFloat"

    Public Const ACGetAllAdjMethodsName As String = "GetAllAdjustmentMethods"
    Public Const ACGetAllAdjMethodsSQL As String = "spu_ACT_SelAll_AdjMethods"

    Public Const ACGetAdjustmentsName As String = "ListAdjustments"
    Public Const ACGetAdjustmentsSQL As String = "spu_ACT_GetAdjustments"

    Public Const ACGetAdjustmentName As String = "GetAdjustment"
    Public Const ACGetAdjustmentSQL As String = "spu_ACT_GetAdjustment"

    Public Const ACGet_BankAccountIdsName As String = "Get_BankAccountIds"
    Public Const ACGet_BankAccountIdsSQL As String = "spu_ACT_Get_BankAccountIds"

    Public Const ACAddCashlistCashName As String = "AddCashlistCash"
    Public Const ACAddCashlistCashSQL As String = "spu_ACT_Add_Cashlist_Cash"

    Public Const ACGetCashlistCashName As String = "GetCashlistCash"
    Public Const ACGetCashlistCashSQL As String = "spu_ACT_Get_Cashlist_Cash"

    Public Const ACGetCashlistStatusName As String = "GetCashListStatus"
    Public Const ACGetCashlistStatusSQL As String = "spu_ACT_Get_Cashlist_Status"

    'SP to Select a single CashListDrawer
    Public Const ACSelectCashListDrawerName As String = "SelectCashListDrawer"
    Public Const ACSelectCashListDrawerSQL As String = "spu_ACT_Select_CashListDrawer"

    'sw front office receipting 25-11-2002
    Public Const ACGetBatchStatusDetailsName As String = "BatchStatusDetails"
    Public Const ACGetBatchStatusDetailsSQL As String = "spu_ACT_Sel_Batch_Status_Details"

    'sw front office receipting 25-11-2002
    Public Const ACGetCashListStatusCodeName As String = "GetCashListStatusCode"
    Public Const ACGetCashListStatusCodeSQL As String = "spu_ACT_Get_CashlistStatusCode"

    Public Const ACGetMatchingCashListDebitsName As String = "GetMatchingCashListDebits"
    Public Const ACGetMatchingCashListDebitsSQL As String = "spu_ACT_Select_Matching_CashList_Debits"

    'TR - TS220 - Filtering MediaTypes based on Payment/Receipts being supported
    Public Const ACSelectFilteredMediaTypesStored As Boolean = True
    Public Const ACSelectFilteredMediaTypesName As String = "SelectFilteredMediaTypes"
    Public Const ACSelectFilteredMediaTypesSQL As String = "spu_ACT_Select_MediaType_filtered"

    'sw 11/07/2003 sp for retrieving autobank total for a cashlist
    Public Const ACGetCashListAutoBankTotalSQL As String = "spu_ACT_Get_CashList_AutoBankTotal"
    Public Const ACGetCashListAutoBankTotalName As String = "GetCashListAutoBankTotal"

    Public Const kGetDocumentDetailsName As String = "Returns details for the specified document / account keys"
    Public Const kGetDocumentDetailsSQL As String = "spu_ACT_Get_Document_Details_For_Account"

    'Chech the currency rate exist or not PN 46054
    Public Const ACCheck_CurrencyRates As String = "Check_CurrencyRates"
    Public Const ACCheck_CurrencyRatesSQL As String = "spu_ACT_Check_Currency_Rate"
    Public Const ACCheckClaimLinkStored As Boolean = True
    Public Const ACCheckClaimLinkName As String = "CheckClaimLink"
    Public Const ACCheckClaimLinkSQL As String = "spu_ACT_Check_Claim_link_For_ClaimPaymentId"

End Module