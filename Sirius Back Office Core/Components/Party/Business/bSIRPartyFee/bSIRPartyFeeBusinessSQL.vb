Option Strict Off
Option Explicit On
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
    ' Date: 05/05/1999
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRPartyFee.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

#Region "Public Constants"

    ' Select All Risk Codes SQL
    Public Const ACGetRiskGroupsStored As Boolean = False
    Public Const ACGetRiskGRoupsName As String = "SelectRiskGroups"
    Public Const ACGetRiskGroupsSQL As String = "Select risk_group_id, code, description from risk_group"

    ' Select All Expense Accounts SQL
    Public Const ACGetExpenseDetailsStored As Boolean = False
    Public Const ACGetExpenseDetailsName As String = "SelectExpenseAccounts"
    Public Const ACGetExpenseDetailsSQL As String = "Select party_cnt, shortname from party p, party_type pt " &
                                                    "WHERE pt.code = 'Ex' " &
                                                    "AND p.party_type_id = pt.party_type_id"

    ' Select All Discount Accounts SQL
    Public Const ACGetDiscountDetailsStored As Boolean = False
    Public Const ACGetDiscountDetailsName As String = "SelectDiscountAccounts"
    Public Const ACGetDiscountDetailsSQL As String = "Select party_cnt, shortname from party p, party_type pt " &
                                                     "WHERE pt.code = 'DI' " &
                                                     "AND p.party_type_id = pt.party_type_id"

    ' Select All % and commission % SQL
    Public Const ACGetPercentageDetailsStored As Boolean = True
    Public Const ACGetPercentageDetailsName As String = "SelectFeeAmounts"
    Public Const ACGetPercentageDetailsSQL As String = "spu_fee_amounts_sel"

    ' 09/08/2000 PSA
    ' Select Fee Amounts By Risk Group Code and Language Id
    Public Const ACGetFeeByRiskCodeStored As Boolean = True
    Public Const ACGetFeeByRiskCodeName As String = "SelectFeeAmountsByRisk"
    Public Const ACGetFeeByRiskCodeSQL As String = "spu_Fee_amounts_sel_by_risk"
    ' 09/08/2000 PSA

    ' AMB 13-Oct-03: 1.8.6 Accident Management development - get list of Extra schemes
    Public Const ACGetExtraSchemeDetailsStored As Boolean = True
    Public Const ACGetExtraSchemeDetailsName As String = "GetExtraSchemeDetailsSQL"
    Public Const ACGetExtraSchemeDetailsSQL As String = "spu_Extra_Scheme_selall"

    ' Add fee amounts
    ' Datasure Extra Parameter
    Public Const ACAddFeeAmountsStored As Boolean = True
    Public Const ACAddFeeAmountsName As String = "AddFeeAmounts"
    Public Const ACAddFeeAmountsSQL As String = "spe_fee_amounts_add"

    ' Delete fee amounts
    Public Const ACDeleteFeeAmountsStored As Boolean = True
    Public Const ACDeleteFeeAmountsName As String = "DeleteFeeAmounts"
    Public Const ACDeleteFeeAmountsSQL As String = "spe_fee_amounts_del"

    'GW200504 PN013 added extra constants for underwriting DML transactions
    ' Add fee amounts for underwriting
    Public Const ACAddFeeAmountsForUnderwritingStored_u As Boolean = True
    Public Const ACAddFeeAmountsForUnderwritingName As String = "AddFeeAmounts_u"
    Public Const ACAddFeeAmountsForUnderwritingSQL As String = "spu_fee_amounts_u_add"

    ' Delete fee amounts for underwriting
    Public Const ACDeleteFeeAmountsForUnderwrtingStored As Boolean = True
    Public Const ACDeleteFeeAmountsForUnderwritingName As String = "DeleteFeeAmounts_u"
    Public Const ACDeleteFeeAmountsForUnderwritingSQL As String = "spu_Fee_amounts_u_del"

    ' Select All fee amounts for underwriting
    Public Const ACGetAllFeeAmountForUnderwritingDetailsStored As Boolean = True
    Public Const ACGetAllFeeAmountForUnderwritingDetailsName As String = "SelectFeeAmounts_u"
    Public Const ACGetAllFeeAmountForUnderwritingDetailsSQL As String = "spu_fee_amounts_u_sel"

    'Update fee amount for underwriting
    Public Const ACUpdateFeeAmountForUnderwritingDetailsStored As Boolean = True
    Public Const ACUpdateFeeAmountForUnderwritingDetailsName As String = "UpdateFeeAmount_u"
    Public Const ACUpdateFeeAmountForUnderwritingDetailsSQL As String = "spu_fee_amounts_u_upd"

    'Select specific fee amount for underwriting
    Public Const ACGetSpecificFeeAmountForUnderwritingDetailsStored As Boolean = True
    Public Const ACGetSpecificFeeAmountForUnderwritingDetailsName As String = "SelectSpecificFeeAmounts_u"
    Public Const ACGetSpecificFeeAmountForUnderwritingDetailsSQL As String = "spu_fee_amounts_u_sel_specific"

    'Select the road map fees for the interface
    Public Const ACGetRoadMapStepDetailsStored As Boolean = True
    Public Const ACGetRoadMapStepDetailsName As String = "SelectRoadmapFees_u"
    Public Const ACGetRoadMapStepDetailsDetailsSQL As String = "spu_FeeRMStep_sel"

    'Select the effective date
    Public Const ACGetRoadMapStepCheckDateDetailsStored As Boolean = True
    Public Const ACGetRoadMapStepCheckDateDetailsName As String = "SelectRoadmapFeesCheckDate_u"
    Public Const ACGetRoadMapStepCheckDateDetailsDetailsSQL As String = "spu_feesRMStep_pol_date_check_u"

    'check if the fee amount/fee percentage value has changed
    Public Const ACGetRoadMapStepCheckValuesDetailsStored As Boolean = True
    Public Const ACGetRoadMapStepCheckValuesDetailsName As String = "SelectRoadmapFeesCheckValues_u"
    Public Const ACGetRoadMapStepCheckValuesDetailsDetailsSQL As String = "spu_fee_check_ammended_u"

    'update the premium with the Fee charges
    Public Const ACUpdatePremiumDetailsStored As Boolean = True
    Public Const ACUpdatePremiumDetailsName As String = "Update_Premium_u"
    Public Const ACUpdatePremiumDetailsSQL As String = "spu_fee_amounts_u_premium_upd"

    'select the party extra fee charge value
    Public Const ACGetPartyExtraDetailsStored As Boolean = True
    Public Const ACGetPartyExtraDetailsName As String = "Sel_Party_Extra_u"
    Public Const ACGetPartyExtraDetailsSQL As String = "spu_Party_Extra_sel"

    'Insert the actual underwriting policy fee charges
    Public Const ACPolicyFees_u_DetailsStored As Boolean = True
    Public Const ACPolicyFees_u_DetailsName As String = "PolicyFees_u_insert"
    Public Const ACPolicyFees_u_DetailsSQL As String = "spu_fees_insert_u"

    'delete the actual underwriting policy fee charges (before insert)
    Public Const ACDeletePolicyFees_u_DetailsStored As Boolean = True
    Public Const ACDeletePolicyFees_u_DetailsName As String = "PolicyFees_u_delete"
    Public Const ACDeletePolicyFees_u_DetailsSQL As String = "spu_fees_Policy_u_delete"


    '******************************
    '******************************
    ' AUS005 Changes
    Public Const kGetUWFeeAmountName As String = "Returns the fee amount details for the specified fee amount"
    Public Const kGetUWFeeAmountSQL As String = "spu_SIR_Fee_Amounts_UW_Select"

    Public Const kGetTableLookupValuesName As String = "Returns the description, code, id from the specified tables entries"
    Public Const kGetTableLookupValuesSQL As String = "spu_SIR_Get_Lookup_Values"

    Public Const kAddUWFeeAmountName As String = "Inserts a record into the fee amounts table"
    Public Const kAddUWFeeAmountSQL As String = "spu_SIR_Fee_Amounts_UW_Insert"

    Public Const kEditUWFeeAmountName As String = "Updates the specified record in the fee amounts table"
    Public Const kEditUWFeeAmountSQL As String = "spu_SIR_Fee_Amounts_UW_Update"

    Public Const kGetTaxGroupTaxRatesName As String = "Returns all rates that need to be applied and in what order for the specific tax group"
    Public Const kGetTaxGroupTaxRatesSQL As String = "spu_Get_Tax_Types_and_Bands"

    Public Const kGetRiskFeesName As String = "Returns all relevant fee for the specified risk"
    Public Const kGetRiskFeesSQL As String = "spu_SIR_Risk_Fees_Select"

    Public Const kGetPolicyFeesName As String = "Returns all relevant fees for the specified policy"
    Public Const kGetPolicyFeesSQL As String = "spu_SIR_Policy_Fees_Select"

    Public Const kRecalculatePolicyFeesName As String = "recalculates all relevant fees for the specified policy"
    Public Const kRecalculatePolicyFeesSQL As String = "spu_SIR_Recalculate_Policy_Fees"

    Public Const kRecalculateRiskFeesName As String = "recalculates all relevant fees for the specified risk"
    Public Const kRecalculateRiskFeesSQL As String = "spu_SIR_Recalculate_Risk_Fees"

    Public Const kDeletePolicyFeeName As String = "deletes the specified policy_fee_u record"
    Public Const kDeletePolicyFeeSQL As String = "spu_SIR_Delete_Policy_Fee"

    Public Const kUpdatePolicyFeeName As String = "updated the specified policy_fee_u record"
    Public Const kUpdatePolicyFeeSQL As String = "spu_SIR_Update_Policy_Fee"

    Public Const kGetRiskDetailsName As String = "returns details for the specified risk cnt"
    Public Const kGetRiskDetailsSQL As String = "spe_Risk_sel"

    Public Const kGetTotalFeeDetailsName As String = "returns Total Fees not included in instalment"
    Public Const kGetTotalFeeDetailsSQL As String = "spu_SIR_Get_FeeNotIncludedInInstalments"

    '******************************
    '******************************

    Public Const kGetRenewalDetailsName As String = "returns details for the creation of renewal fees"
    Public Const kGetRenewalDetailsSQL As String = "spu_SIR_Get_Renewal_Details"
    ' Copy Risk Fee SQL
    Public Const kCopyRiskFeesStored As Boolean = True
    Public Const kCopyRiskFeesName As String = "Copy Risk Fees"
    Public Const kCopyRiskFeesSQL As String = "spu_Risk_Fees_Copy"

    ' Copy Policy Fee SQL
    Public Const kCopyPolicyFeesStored As Boolean = True
    Public Const kCopyPolicyFeesName As String = "Copy Risk Fees"
    Public Const kCopyPolicyFeesSQL As String = "spu_Policy_Fees_Copy"

    Public Const kGetProRataRateName = "GetProRataRate from fee amounts table"
    Public Const kGetProRataRateSQL = "spu_get_policy_pro_rata_rate"

    Public Const kGetOriginalRiskCntName = "Get_original_risk_cnt_for_risk_cnt"
    Public Const kGetOriginalRiskCntSQL = "spu_Get_original_risk_cnt_for_risk_cnt"

#End Region

End Module