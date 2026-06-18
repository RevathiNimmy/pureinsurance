Option Strict Off
Option Explicit On
Module bSIRCashDepositSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    ' ***************************************************************** '
    ' Class Name: bSIRBankGuaranteeSQL
    '
    ' Date: 15/08/2007
    '
    ' Description: Contains the SQL Statements to (Stored Procedures)
    '
    ' Edit History: Gaurav Arora
    ' ***************************************************************** '

    'Start - Sankar - (Tech Spec -WPR85_Cash_Deposit_Process Part 1.doc) - (5.1.6.1)
    Public Const ACSELCashDepositDetailsSQL As String = "spu_Get_CashDeposit_For_CD_Maintenance"
    Public Const ACSELCashDepositDetailsName As String = "Select Cash Deposit Details"

    Public Const ACGetNextCashDepositNumberSQL As String = "spu_Get_Next_CashDeposit_Number"
    Public Const ACGetNextCashDepositNumberName As String = "Get Next CashDeposit Number"

    Public Const ACGETPartyDetailsSQL As String = "spe_Party_sel"
    Public Const ACGETPartyDetailsName As String = "Get Party Details"

    Public Const ACGetAllBranchesSQL As String = "spu_Get_Branches"
    Public Const ACGetAllBranchesName As String = "Get All the Branches"

    Public Const ACGetCashDepositBranchesQL As String = "spu_CashDeposit_Branch_Sel"
    Public Const ACGetCashDepositBranchesName As String = "Get All CashDeposit Branches"

    Public Const ACGetAllProductsSQL As String = "spu_Product_saa"
    Public Const ACGetAllProductsName As String = "Get All the Products"

    Public Const ACGetCashDepositProductsSQL As String = "spu_CashDeposit_Product_Sel"
    Public Const ACGetCashDepositProductsName As String = "Get All CashDeposit Products"

    Public Const ACADDCashDepositDetailsSQL As String = "spu_CashDeposit_Add"
    Public Const ACADDCashDepositDetailsName As String = "Add Cash Deposit Details"

    Public Const ACUPDCashDepositDetailsSQL As String = "spu_CashDeposit_SinglePolicy_Upd"
    Public Const ACUPDCashDepositDetailsName As String = "Update CD Details"

    Public Const ACADDCashDepositProductName As String = "Add Product to CD"
    Public Const ACADDCashDepositProductSQL As String = "spu_CashDeposit_Product_Add"

    Public Const ACADDCashDepositBranchSQL As String = "spu_CashDeposit_Branch_Add"
    Public Const ACADDCashDepositBranchName As String = "Add Branch to CD"

    Public Const ACDELCashDepositProductName As String = "Delete Product to CD"
    Public Const ACDELCashDepositProductSQL As String = "spu_CashDeposit_Product_Del"

    Public Const ACDELCashDepositBranchName As String = "Delete Branch to CD"
    Public Const ACDELCashDepositBranchSQL As String = "spu_CashDeposit_Branch_Del"

    Public Const ACSELinkedCashDepositAccountsSQL As String = "spu_Get_Linked_CashDeposit_Accounts"
    Public Const ACSELinkedCashDepositAccountsName As String = "Select CD Linked Accounts"

    Public Const ACGetEventLogSQL As String = "spe_Party_Public_Text_saa"
    Public Const ACGetEventLogName As String = "Get Event Logs Of Party"

    Public Const ACAddEventLogSQL As String = "spe_Party_Public_Text_add"
    Public Const ACAddEventLogName As String = "Add Event Logs For Party"
    'End - Sankar - (Tech Spec -WPR85_Cash_Deposit_Process Part 1.doc) - (5.1.6.1)
    'Start - (Renuka) - Tech Spec -UIIC_WPR85_Cash_Deposit_Process-Part 2
    Public Const ACSELCDsForPolicySQL As String = "spu_Get_CashDeposit_For_Policy"
    Public Const ACSELCDsForPolicyName As String = "Get Cash Deposit Details For Policy"

    Public Const ACSELCDPaymentHistoryForPolicySQL As String = "spu_Get_Policy_CDPayment_History"
    Public Const ACSELCDPaymentHistoryForPolicyName As String = "Get Cash Deposit Details previously used by the same policy"

    Public Const ACSELPolicyDetailsForCashDepositSQL As String = "spu_Get_PolicyDetails_For_CashDeposit"
    Public Const ACSELPolicyDetailsForCashDepositName As String = "Get Policy Details For Cash Deposit"

    Public Const ACSELCDReceiptsForAllocationSQL As String = "spu_Get_CDReceipts_For_Allocation"
    Public Const ACSELCDReceiptsForAllocationName As String = "Get Cash Deposit Receipts for Allocation"
    'Start - Prakash - PN 65554. Code is not needed according to new functionality. It can be removed.
    'Public Const ACSELCDReceiptsForRefundSQL = "spu_Get_CDReceipts_For_Refund"
    'Public Const ACSELCDReceiptsForRefundName = "Get Cash Deposit Receipts for Refund Allocation"
    'End - Prakash - PN 65554
    'Start - Renuka - Changes according to the WPR85 process sheet updation
    Public Const ACSELBalanceForCDSQL As String = "spu_Get_Balance_For_CD"
    Public Const ACSELBalanceForCDName As String = "Get the Total/Running balance for the selected Cash Deposit"
    'End - Renuka - Changes according to the WPR85 process sheet updation
    'Start - Sanakr - Changes according to the WPR85 process sheet updation
    Public Const ACCheckCDUsedForMultiPolicySQL As String = "spu_CheckCDUsedForMultiPolicy"
    Public Const ACCheckCDUsedForMultiPolicyName As String = "Check CD Used For Multi Policy"
    'End - Sankar - Changes according to the WPR85 process sheet updation
    'End - (Renuka) - Tech Spec -UIIC_WPR85_Cash_Deposit_Process-Part 2
    'Start - Prakash - PN 65557
    Public Const ACConvertPolicyAmountToBaseCurrencySQL As String = "spu_Convert_Policy_Amount_To_Base_Currency"
    Public Const ACConvertPolicyAmountToBaseCurrencyName As String = "Convert policy amount to base currency"
    'End - Prakash - PN 65557
End Module