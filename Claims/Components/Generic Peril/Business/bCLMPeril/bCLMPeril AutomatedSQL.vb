Option Strict Off
Option Explicit On
Module AutomatedSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: AutomatedSQL
    '
    ' Date: {TodaysDate}
    '
    ' Description: Contains the SQL Statements required by the
    '              bCLMPeril.Automated class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    Public Const ACGetClaimNumberStored As Boolean = True
    Public Const ACGetClaimNumberName As String = "Get Claim Number"
    Public Const ACGetClaimNumberSQL As String = "spu_get_claimnumber"

    Public Const ACGetClaimClientAndAgentStored As Boolean = True
    Public Const ACGetClaimClientAndAgentName As String = "Get Claim Client and Agent"
    Public Const ACGetClaimClientAndAgentSQL As String = "spu_CLM_Get_Client_And_Agent_Details"

    Public Const ACGetOriginalClaimIDStored As Boolean = True
    Public Const ACGetOriginalClaimIDName As String = "Get Original Claim ID"
    Public Const ACGetOriginalClaimIDSQL As String = "spu_CLM_Get_Base_Claim"

    Public Const ACGetClientPolicyDetailsStored As Boolean = True
    Public Const ACGetClientPolicyDetailsName As String = "GetClientPolicyDetails"
    Public Const ACGetClientPolicyDetailsSQL As String = "spu_get_client_policy_details"

    Public Const ACGetClaimNumberFromClaimStored As Boolean = True
    Public Const ACGetClaimNumberFromClaimName As String = "Get Claim Number"
    Public Const ACGetClaimNumberFromClaimSQL As String = "spu_get_claimnumber"

    Public Const ACGetReferredPaymentStored As Boolean = True
    Public Const ACGetReferredPaymentName As String = "Get Referred Payment"
    Public Const ACGetReferredPaymentSQL As String = "spu_CLM_Get_Referred_Payment_Count"

    Public Const ACGetTaxTypesBandsStored As Boolean = True
    Public Const ACGetTaxTypesBandsName As String = "GetTaxTypesBands"
    Public Const ACGetTaxTypesBandsSQL As String = "spu_Get_Tax_Types_and_Bands"

    Public Const ACGetPolicyTypeStored As Boolean = True
    Public Const ACGetPolicyTypeName As String = "GetPolicyType"
    Public Const ACGetPolicyTypeSQL As String = "spu_Get_Policy_Type"

    Public Const ACGetClaimCurrencyStored As Boolean = True
    Public Const ACGetClaimCurrencyName As String = "GetClaimCurrency"
    Public Const ACGetClaimCurrencySQL As String = "spu_Get_Claim_Currency"

    Public Const kRetrieveCurrenciesForClaimBranchName As String = "Returns the currencies available on the branch associated with the specified claim"
    Public Const kRetrieveCurrenciesForClaimBranchSQL As String = "spu_Get_Claim_Branch_Currency"

    '**********************************************
    ' Changes for 306 - Taxes On Claims
    '**********************************************

    '**********************************************
    ' Payments and Receipts
    '**********************************************

    Public Const kGetSafeHarbourDetailsName As String = "Returns valid safe harbour details"
    Public Const kGetSafeHarbourDetailsSQL As String = "spu_CLM_Get_Safe_Harbour_Details"

    Public Const kGetClaimPaymentToDetailsName As String = "Returns valid claim payment to details"
    Public Const kGetClaimPaymentToDetailsSQL As String = "spu_CLM_Get_Claim_Payment_To"

    Public Const kGetClaimPaymentDetailsName As String = "Get Claim Payment Details"
    Public Const kGetClaimPaymentDetailsSQL As String = "spu_CLM_Get_Claim_Payment_Details"

    Public Const kGetClaimDetailsName As String = "Returns required claim details for specified work_claim"
    Public Const kGetClaimDetailsSQL As String = "spu_CLM_Get_Claim_Details_For_Payment"

    Public Const kGetLookupsByEffectiveDateName As String = "Returns lookups by effective date"
    Public Const kGetLookupsByEffectiveDateSQL As String = "spu_SIR_Get_Lookup_Values_By_Effective_Date"

    Public Const kGetCurrentClaimPaymentReserveDetailsName As String = "Returns the current set of reserves for the specified claim peril and any associated payments"
    Public Const kGetCurrentClaimPaymentReserveDetailsSQL As String = "spu_CLM_Get_Current_Claim_Payments_By_Reserve"

    Public Const kGetOtherPartyDetailsName As String = "Returns the account and other details for the specified other party"
    Public Const kGetOtherPartyDetailsSQL As String = "spu_CLM_Get_Party_Account_Details"

    Public Const kGetAccountDetailsByShortCodeName As String = "Returns the account details for the claim payable account"
    Public Const kGetAccountDetailsByShortCodeSQL As String = "spu_CLM_Get_Account_Details_by_Short_Code"

    Public Const kGetClaimPaymentItemDetailsName As String = "returns the work claim payment item details for the specified work_claim_payment_id"
    Public Const kGetClaimPaymentItemDetailsSQL As String = "spu_CLM_Get_Payment_Item_Details"

    Public Const kGetTaxGroupDetailsName As String = "returns the tax group details"
    Public Const kGetTaxGroupDetailsSQL As String = "spu_SIR_Get_Tax_Group_Details"

    Public Const kCalculateTaxAmountsName As String = "calculate the tax amounts for the specified payment / receipt"
    Public Const kCalculateTaxAmountsSQL As String = "spu_CLM_Calculate_Tax_Amounts"

    Public Const kGetTaxGroupTaxBandDetailsName As String = "returns the tax group tax band links"
    Public Const kGetTaxGroupTaxBandDetailsSQL As String = "spu_CLM_Tax_Group_Tax_Band_Select"

    Public Const kSaveClaimPaymentName As String = "saves the details to the work claim payment table"
    Public Const kSaveClaimPaymentSQL As String = "spu_CLM_Claim_Payment_Add"

    Public Const kSaveClaimPaymentItemName As String = "saves the details to the work claim payment table"
    Public Const kSaveClaimPaymentItemSQL As String = "spu_CLM_Claim_Payment_Item_Add"

    Public Const kSaveTaxCalculationItemName As String = "saves the details to the work claim payment table"
    Public Const kSaveTaxCalculationItemSQL As String = "spu_CLM_Tax_Calculation_Add"

    Public Const kGetClaimPaymentItemTaxName As String = "returns the tax details for the claim payment"
    Public Const kGetClaimPaymentItemTaxSQL As String = "spu_CLM_Tax_Calculation_Select"

    Public Const kUpdateClaimPaymentItemReserveName As String = "updates the reserve associated with the claim payment item based on payment amount"
    Public Const kUpdateClaimPaymentItemReserveSQL As String = "spu_CLM_Claim_Payment_Item_Reserve_Update"

    Public Const kGetClaimPerilRecoveryDetailsName As String = "returns the details from work recovery for the specified claim peril and is salvage flag"
    Public Const kGetClaimPerilRecoveryDetailsSQL As String = "spu_recovery_saa"

    Public Const kSaveClaimReceiptName As String = "adds a new work claim receipt and returns its work id"
    Public Const kSaveClaimReceiptSQL As String = "spu_CLM_Claim_Receipt_Add"

    Public Const kSaveClaimReceiptItemName As String = "adds a new work claim receipt item and returns its work id"
    Public Const kSaveClaimReceiptItemSQL As String = "spu_CLM_Claim_Receipt_Item_Add"

    Public Const kUpdateClaimReceiptItemRecoveryName As String = "updates the recovery specified with the receipt details"
    Public Const kUpdateClaimReceiptItemRecoverySQL As String = "spu_CLM_Claim_Receipt_Item_Recovery_Update"

    Public Const kGetClaimTaxAmountsByTaxTypeName As String = "returns the tax entries by tax type for the specified payment or receipt"
    Public Const kGetClaimTaxAmountsByTaxTypeSQL As String = "spu_CLM_Get_Claim_Tax_Amounts_By_Tax_Type"

    Public Const kCreateClaimReservesName As String = "create the relevant reserve for the claims"
    Public Const kCreateClaimReservesSQL As String = "spu_get_reserve_details"

    Public Const kGetCoinsuranceName As String = "returns the coinsurance details for the specified peril and recovery type"
    Public Const kGetCoinsuranceSQL As String = "spu_claims_recovery_coins_select"

    Public Const kGetReinsuranceName As String = "returns the coinsurance details for the specified peril and recovery type"
    Public Const kGetReinsuranceSQL As String = "spu_claims_recovery_reins_select"

    Public Const kUpdateClaimReinsuranceName As String = "updates the arrangement and arrangement lines with the relevant payment amounts"
    Public Const kUpdateClaimReinsuranceSQL As String = "spu_claims_recovery_reins_allocate"

    Public Const kGetMediaTypesName As String = "returns the media type lookup details"
    Public Const kGetMediaTypesSQL As String = "spu_CLM_Get_MediaTypes"

    Public Const kGetCoInsurerDetailsName As String = "Returns CoInsurer Breakdown"
    Public Const kGetCoInsurerDetailsSQL As String = "spu_CLM_Get_CoInsurer_Split"

    Public Const kUpdateCoInsurerDetailsName As String = "Updates CoInsurer Breakdown"
    Public Const kUpdateCoInsurerDetailsSQL As String = "spu_CLM_Update_CoInsurer_Split"

    Public Const kGetClaimBranchCurrenciesName As String = "saves the details to the claim payment table"
    Public Const kGetClaimBranchCurrenciesSQL As String = "spu_CLM_Get_Branch_Currencies"

    Public Const kGetClassOfBusinessName As String = "get class of business for peril"
    Public Const kGetClassOfBusinessSQL As String = "spu_CLM_Get_Claim_Peril_Class_Of_Business"

    Public Const ACSelRiskDetailName As String = "GetRiskDetails"
    Public Const ACSelRiskDetailSQL As String = "spu_CLM_Get_Risk_Details"

    Public Const ACSelRiskDetailBrokingName As String = "GetRiskDetailsForBroking"
    Public Const ACSelRiskDetailBrokingSQL As String = "spu_GetRiskDetailsForBroking"

    '(RC) QBENZ001
    Public Const ACGetUserCanChangeReservesName As String = "GetUserCanChangeReserves"

    Public Const ACGetUserCanChangeReservesSQL As String = "spu_SIR_Get_UserCanChange_Reserves"

    Public Const ACGetXOLCountName As String = "GetCLMXOLCount"
    Public Const ACGetXOLCountSQL As String = "spu_CLM_Get_XOL_Count"

    Public Const ACGetAllReceiptsForClaimSQL As String = "spu_get_all_receipts_for_claim"
    Public Const ACGetAllReceiptsForClaimName As String = "Get All Receipts For Claim"
    Public Const ACGetAllReceiptsForClaimStoredProcedure As Boolean = True

    Public Const ACGetClaimTaxCalculationForReceiptSQL As String = "spu_CLM_Tax_Calculation_Select_For_Receipt"
    Public Const ACGetClaimTaxCalculationForReceiptName As String = "Claim Tax Calculation select for Receipt"
    Public Const ACGetClaimTaxCalculationForReceiptStoredProcedure As Boolean = True

    Public Const ACGetClaimGetReceiptDetailsSQL As String = "spu_CLM_Get_Claim_Receipt_Details"
    Public Const ACGetClaimGetReceiptDetailsName As String = "Get Claim Receipt Details"
    Public Const ACGetClaimGetReceiptDetailsStoredProcedure As Boolean = True

    Public Const ACGetClaimReceiptItemDetailsName As String = "returns the work claim payment item details for the specified work_claim_payment_id"
    Public Const ACGetClaimReceiptItemDetailsSQL As String = "spu_CLM_Get_Receipt_Item_Details"
    Public Const ACGetClaimReceiptItemDetailsStoredProcedure As Boolean = True

    'WPR022
    Public Const ACUpdateClaimTransactionTypeName As String = "Update Claim Transaction Type"
    Public Const ACUpdateClaimTransactionTypeSQL As String = "spu_CLM_Update_transaction_type"

    Public Const ACGetClaimGetAccountIdFromShortCodeSQL As String = "spu_Get_AccountIdFromShortCode"
    Public Const ACGetClaimGetAccountIdFromShortCodeName As String = "Get Claim Account Id From Short Code"
    Public Const ACGetClaimGetAccountIdFromShortCodeStoredProcedure As Boolean = True

    Public Const ACGetClaimPaymentTotalSQL As String = "spu_CLM_Get_Claim_Payment_Total"
    Public Const ACGetClaimPaymentTotalSQLName As String = "Get Claim Payment Total"
    Public Const ACGetClaimPaymentTotalSQLStoredProcedure As Boolean = True

    Public Const kGetClaimPaymentTotalName As String = "Returns Claim Payment Total"
    Public Const kGetClaimPaymentTotalSQL As String = "spu_CLM_Get_Claim_Payment_Total"

    Public Const ACGetRuleTypeAndFileValueStored As Boolean = True
    Public Const ACGetRuleTypeAndFileValueName As String = "GetRuleTypeAndFileValue"
    Public Const ACGetRuleTypeAndFileValueSQL As String = "spu_Get_Rule_Type_Values"

    Public Const kGetClaimRecoveriesName As String = "Returns required recoveries details for specified peril id"
    Public Const kGetClaimRecoveriesSQL As String = "spu_get_recoveries_by_peril"

    Public Const kSaveClaimRecoveryName As String = "Save Claim recovery"
    Public Const kSaveClaimRecoverySQL As String = "spu_CLM_save_recovery"

    Public Const kUpdateThisClaimPaymentName As String = "Update this claim payment details"
    Public Const kUpdateThisClaimPaymentSQL As String = "spu_update_claim_this_payment_Details"

    Public Const kGetCurrencyRatesToOverrideName = "Returns The current and old Currency Rate When claim is opened"
    Public Const kGetCurrencyRatesToOverrideSQL = "spu_CLM_GetCurrencyRatesToOverride"

    Public Const kOverrideClaimCurrencyRateName = "Override the Claim's Currency Rate to the currency rate When claim is opened"
    Public Const kOverrideClaimCurrencyRateSQL = "spu_CLM_OverrideClaimCurrencyRate"

    Public Const ACGetUserReserveLimitName As String = "Gets the users Reserve Limit"
    Public Const ACGetUserReserveLimitSQL As String = "spu_Get_User_Reserve_Limit_sel"
    Public Const ACGetUserReserveLimitStoredProcedure As Boolean = True


End Module