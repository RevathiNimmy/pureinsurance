Option Strict Off
Option Explicit On
Module SIRIUSLinkSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: SiriusLinkSQL
    '
    ' Description: Contains the SQL Statements required by the
    '              bSiriusLink.SiriusLink class.
    '
    ' Edit History:
    '   PWF 06/11/2001 - Created
    ' ***************************************************************** '


    'Add a risk folder
    Public Const ACAddRiskFolderStored As Boolean = True
    Public Const ACAddRiskFolderName As String = "AddRiskFolder"
    'developer guide no 39. 
    Public Const ACAddRiskFolderSQL As String = "spe_risk_folder_add"

    'Add a risk
    Public Const ACAddRiskStored As Boolean = True
    Public Const ACAddRiskName As String = "AddRisk"
    'developer guide no 39.
    Public Const ACAddRiskSQL As String = "spe_risk_add"

    'Add insurance file risk
    Public Const ACInsuranceFileRiskStored As Boolean = True
    Public Const ACInsuranceFileRiskName As String = "AddInsuranceFileRisk"
    'developer guide no 39.
    Public Const ACInsuranceFileRiskSQL As String = "spe_insurance_file_risk_li_add"

    ' TF200202 - Get Percentage & Actual amounts from Fee tables
    Public Const ACGetFeeAmountsStored As Boolean = False
    Public Const ACGetFeeAmountsName As String = "GetFeeAmounts"
    Public Const ACGetFeeAmountsSQL As String = "SELECT fee_percentage, fee_amount" & " FROM Fee_Amounts F, Insurance_File I, Risk_Code R" & " WHERE i.insurance_file_cnt = {insurance_file_cnt}" & " AND R.risk_code_id = I.risk_code_id" & " AND F.risk_group_id = R.risk_group_id" & " AND F.display_on_quotes = 1"

    'eck 090703 PN4760
    Public Const ACGetLapsedReasonStored As Boolean = False
    Public Const ACGetLapsedReasonName As String = "GetLapsedReason"
    Public Const ACGetLapsedReasonSQL As String = "SELECT lapsed_reason_id,lapsed_description" & " FROM insurance_file" & " WHERE insurance_file_cnt = {insurance_file_cnt}"

    ' CJB 110804 PN14031
    Public Const ACGetPolicyReferAtRenewalIndicatorValueStored As Boolean = True
    Public Const ACGetPolicyReferAtRenewalIndicatorValueName As String = "ReferAtRenewalIndicatorValue"
    'developer guide no 39.
    Public Const ACGetPolicyReferAtRenewalIndicatorValueSQL As String = "spu_Get_Policy_Refer_At_Renewal_Indicator_Value"

    Public Const ACGetPartyByShortNameStored As Boolean = True
    Public Const ACGetPartyByShortNameName As String = "GetPartyByShortName"
    Public Const ACGetPartyByShortNameSQL As String = "spu_GetPartyByShortName"

    Public Const ACUpdateRiskDescriptionStored As Boolean = True
    Public Const ACUpdateRiskDescriptionName As String = "UpdateRiskDescription"
    Public Const ACUpdateRiskDescriptionSQL As String = "spu_risk_description_update"

    'Start - Sankar - PN 55108
    Public Const ACGetGracePeriodStored As Boolean = True
    Public Const ACGetGracePeriodName As String = "GetQuoteExpiryDate"
    Public Const ACGetGracePeriodSQL As String = "spu_SAM_Product_sel"
    'End - Sankar - PN 55108
End Module