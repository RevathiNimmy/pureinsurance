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
    ' Date: 02/09/2000
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRRenInvitePrint.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    Public Const ACUpdPolicyPremiumStored As Boolean = True
    Public Const ACUpdPolicyPremiumName As String = "UpdatePolicyPremium"
    'Developer Guide No 39. 
    Public Const ACUpdPolicyPremiumSQL As String = "spu_Upd_Policy_Premium"

    Public Const ACUpdateInsFileTypeStored As Boolean = False
    Public Const ACUpdateInsFileTypeName As String = "UpdateInsFileType"
    Public Const ACUpdateInsFileTypeSQL As String = "UPDATE event_insurance_file" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                    "SET insurance_file_type_id = {new_id}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                    "WHERE insurance_file_cnt = (" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                    "SELECT MAX(event_cnt)" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                    "FROM event_log" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                    "WHERE insurance_file_cnt = {insurance_file_cnt})" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                    "AND insurance_file_type_id = {old_id}"
    'JMK 03/08/2001
    Public Const ACCancelAllVersionsStored As Boolean = True
    Public Const ACCancelAllVersionsName As String = "CancelAllVersions"
    'Developer Guide No 39. 
    Public Const ACCancelAllVersionsSQL As String = "spu_cancel_all_versions"

    'Tomo240901
    Public Const ACGetRisksByStatusStored As Boolean = True
    Public Const ACGetRisksByStatusName As String = "Get Risks By Status"
    'Developer Guide No 39. 
    Public Const ACGetRisksByStatusSQL As String = "spu_get_risks_by_status"

    'Tomo051101
    Public Const ACCheckProductStored As Boolean = False
    Public Const ACCheckProductName As String = "Check Product"
    Public Const ACCheckProductSQL As String = "SELECT is_policy_number_at_quote FROM product WHERE product_id = {product_id}"

    ' PW311002
    Public Const ACDeleteRiskStored As Boolean = True
    Public Const ACDeleteRiskName As String = "Delete Risk"
    Public Const ACDeleteRiskSQL As String = "spu_delete_risk"

    ' PW311002
    Public Const ACRenumberRisksStored As Boolean = True
    Public Const ACRenumberRisksName As String = "Renumber Risks"
    'Public Const ACRenumberRisksSQL As String = "{call spu_renumber_risks(?)}"
    Public Const ACRenumberRisksSQL As String = "spu_renumber_risks"

    Public Const ACGetPolicySummaryStored As Boolean = True
    Public Const ACGetPolicySummaryName As String = "Get Policy Summary"
    'Public Const ACGetPolicySummarySQL As String = "{call spu_get_policy_summary(?)}"
    Public Const ACGetPolicySummarySQL As String = "spu_get_policy_summary"

    Public Const ACResetAllVersionsStored As Boolean = True
    Public Const ACResetAllVersionsName As String = "ResetAllVersions"
    'Public Const ACResetAllVersionsSQL As String = "{call spu_reset_all_versions(?)}"
    Public Const ACResetAllVersionsSQL As String = "spu_reset_all_versions"
    'Start - Renuka - (WPR87 Paralleling)
    Public Const ACGetNumberingSchemeIdsFromProductSQL As String = "spu_get_prod_auto_num_ids"
    Public Const ACGetNumberingSchemeIdsFromProductName As String = "GetNumberingSchemeIdsFromProduct"
    Public Const ACGetNumberingSchemeFromProductStored As Boolean = True

    Public Const ACGetNumberingSchemeSQL As String = "spu_numbering_scheme_saa"
    Public Const ACGetNumberingSchemeName As String = "GetNumberingScheme"
    Public Const ACGetNumberingSchemeStored As Boolean = True
    'End - Renuka - (WPR87 Paralleling)
End Module