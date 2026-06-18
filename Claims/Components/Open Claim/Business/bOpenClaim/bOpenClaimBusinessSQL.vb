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
	' Date:  14-JUN-00
	'
	' Description: Contains the SQL Statements required by the
	'              bOpenClaim.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	' Select Get Claim Address SQL-Suhel
	Public Const ACGetClmAddStored As Boolean = True
	Public Const ACGetClmAddName As String = "GetClmAdd"
	Public Const ACGetClmAddSQL As String = "spu_get_clm_add"
	
	'***********Start of Change the Same as UserControl Defined Caption is to be Retrieved *******
	
	' Select Get Claim Address SQL-Suhel
	Public Const ACGetUserDefinedCaptionStored As Boolean = True
	Public Const ACGetUserDefinedCaptionName As String = "GetUserDefinedCaption"
	Public Const ACGetUserDefinedCaptionSQL As String = "spu_User_Def_Caption"
	
	'***********End of Change the Same as UserControl Defined Caption is to be Retrieved *******

    ' Select All OpenClaim SQL
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllOpenClaim"

    Public Const ACGetAllDetailsSQL As String = "spe_{SQLTableName}_saa"

    ' Check ID SQL
    Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckClaimID"
    Public Const ACCheckIDSQL As String = "spu_Claim_Check_No"

    ' Select Secondary Cause for Primary Cause SQL
    Public Const ACGetSecondaryCauseStored As Boolean = True
    Public Const ACGetSecondaryCauseName As String = "SecondaryCause"
    Public Const ACGetSecondaryCauseSQL As String = "Spe_Secondary_Cause_Sel"

    ' Add SIRAddress SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddCLMAddress"
    Public Const ACAddSQL As String = "spu_Claim_Address_add"

    'AR20050404 - PN15644
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateCLMAddress"
    Public Const ACUpdateSQL As String = "spu_Claim_Address_upd"

    ' RWH(10/11/2000) Get Insurance File Details
    Public Const ACSelectInsuranceFileStored As Boolean = True
    Public Const ACSelectInsuranceFileName As String = "SelectInsuranceFile"
    Public Const ACSelectInsuranceFileSQL As String = "spe_Insurance_File_sel"

    ' Delete any  Claim records
    Public Const ACDeleteClaimStored As Boolean = True
    Public Const ACDeleteClaimName As String = "Delete  Claim"
    Public Const ACDeleteClaimSQL As String = "spu_delete_claim"

    'TN20010615 start
    Public Const ACGetOriginalClaimIDStored As Boolean = True
    Public Const ACGetOriginalClaimIDName As String = "Get Original Claim ID"
    Public Const ACGetOriginalClaimIDSQL As String = "spu_CLM_Get_Base_Claim"
    'TN20010615 end
    'AK 180401 - To Get Policy Type
    Public Const ACGetPolicyTypeStored As Boolean = True
    Public Const ACGetPolicyTypeName As String = "GetPolicyType"
    Public Const ACGetPolicyTypeSQL As String = "spu_Get_Policy_Type"

    'AK 310701 - To Get Policy Type
    Public Const ACCheckRenewalsStored As Boolean = True
    Public Const ACCheckRenewalsName As String = "CheckRenewals"
    Public Const ACCheckRenewalsSQL As String = "spu_SIRRen_CheckRenewals"

    'AK 030801
    ' Renewal_Control Update
    Public Const ACUpdateRenewalControlStored As Boolean = True
    Public Const ACUpdateRenewalControlName As String = "UpdateRenewalControl"
    Public Const ACUpdateRenewalControlSQL As String = "spu_renewal_control_Update"

    Public Const ACSelectRenewalControlStored As Boolean = True
    Public Const ACSelectRenewalControlName As String = "SelectRenewalControl"
    Public Const ACSelectRenewalControlSQL As String = "spu_Renewal_Control_Sel"

    Public Const ACSetExistRenToRepSQL As String = "spu_SIR_set_exist_ren_to_rep"
    Public Const ACSetExistRenToRepName As String = "SetExistRenRep"
    Public Const ACSetExistRenToRepStored As Boolean = True

    'DC251001 -start -to get details of new insurer selected (BROKING ONLY)
    'DJM 07/05/2002 : Added an extra parameter and changed name to party as it is now used both for Insurer and Client.
    Public Const ACGetClmPartyDtlsStored As Boolean = True
    Public Const ACGetClmPartyDtlsName As String = "Get Claim Party Details"
    Public Const ACGetClmPartyDtlsSQL As String = "spu_get_clm_party_dtls"
    'DC251001 -end

    'CJB 050902 Added the following from the old SourceSafe as part of parallel
    'maintenance that had been missed...
    'DC270502
    Public Const ACGetShowBRRiskDetailsStored As Boolean = True
    Public Const ACGetShowBRRiskDetailsName As String = "GetShowBrokingRiskDetails"
    Public Const ACGetShowBRRiskDetailsSQL As String = "spu_GetShowBrokingRiskDetails"

    'sj 03/10/2002 - start
    Public Const ACGetClientPolicyDetailsStored As Boolean = True
    Public Const ACGetClientPolicyDetailsName As String = "GetClientPolicyDetails"
    Public Const ACGetClientPolicyDetailsSQL As String = "spu_get_client_policy_details"
    'sj 03/10/2002 - end

    ' RDT 08/04/03 - Start - changes for IAG 215 spec
    Public Const ACGetValidPrimaryCausesStored As Boolean = True
    Public Const ACGetValidPrimaryCausesName As String = "GetValidPrimaryCauses"
    Public Const ACGetValidPrimaryCausesSQL As String = "spu_CLM_Get_Valid_Primary_Causes"
    ' RDT 08/04/03 - End - changes for IAG 215 spec

    ' AMB 10-Sep-03: 1.8.6 Deferred Reinsurance development - check risk status of claim's risk
    Public Const ACGetClaimRiskStatusStored As Boolean = True
    Public Const ACGetClaimRiskStatusName As String = "GetClaimRiskStatus"
    Public Const ACGetClaimRiskStatusSQL As String = "spu_CLM_risk_status_sel"

    ' Alix - 17/02/2004
    Public Const ACGetPolicyForClaimDateStored As Boolean = True
    Public Const ACGetPolicyForClaimDateName As String = "Get Policy For Claim Date"
    Public Const ACGetPolicyForClaimDateSQL As String = "spu_Get_Policy_For_Claim_Date"
    ' /Alix

    Public Const ACGetClaimUserTaskGroupStored As Boolean = True
    Public Const ACGetClaimUserTaskGroupName As String = "Get Claim User and Task Group"
    Public Const ACGetClaimUserTaskGroupSQL As String = "spu_get_claim_user_task_group"

    Public Const ACGetClaimNumberStored As Boolean = True
    Public Const ACGetClaimNumberName As String = "Get claim number for the supplied claim id"
    Public Const ACGetClaimNumberSQL As String = "spu_Get_ClaimNumber"

    Public Const kGetPolicyVersionsName As String = "Returns all policy versions for the policy of the specified insurance file"
    Public Const kGetPolicyVersionsSQL As String = "spu_clm_get_all_policy_versions"

    Public Const kGetInsuranceFolderCntSQL As String = "spu_get_Insurance_Folder"
    Public Const kGetInsuranceFolderCntName As String = "Returns InsuranceFolderCnt for the policy of the specified insurancefilecnt"

    Public Const kGetDuplicateClaimsName As String = "Returns possible duplicate claims"
    Public Const kGetDuplicateClaimsSQL As String = "spu_clm_get_duplicate_claim"

    Public Const kGetDuplicateClaimOverrideUsersName As String = "Returns user with duplicate claims override authority"
    Public Const kGetDuplicateClaimOverrideUsersSQL As String = "spu_clm_get_duplicate_claim_override_users"

    Public Const kAddClaimLinkName As String = "add a  claim link entry"
    Public Const kAddClaimLinkSQL As String = "spu_clm_claim_link_add"

    Public Const kGetClaimTransactionSuppressionIndName As String = "Returns the claim transaction suppression indicators for the relevant product and claim"
    Public Const kGetClaimTransactionSuppressionIndSQL As String = "spu_clm_transaction_suppression_ind_sel"

    'S4B Claim Enhancements R&D 2005
    Public Const kCLM_SQL_IS_CLOSED_STATUS_SP As Boolean = True
    Public Const kCLM_SQL_IS_CLOSED_STATUS_NAME As String = "Returns the Closed Check status of a Claim Status"
    Public Const kCLM_SQL_IS_CLOSED_STATUS_SQL As String = "spu_clm_is_progress_status_closed"

    Public Const kCLM_SQL_ZERO_RESERVE_SP As Boolean = True
    Public Const kCLM_SQL_ZERO_RESERVE_NAME As String = "Resets the outstanding reserves of a claim to zero"
    Public Const kCLM_SQL_ZERO_RESERVE_SQL As String = "spu_clm_reset_reserves"

    Public Const kCLM_SQL_ACCOUNT_HANDLER_SP As Boolean = True
    Public Const kCLM_SQL_ACCOUNT_HANDLER_NAME As String = "Gets the account handler names of a policy"
    Public Const kCLM_SQL_ACCOUNT_HANDLER_SQL As String = "spu_clm_get_policy_account_handlers"

    'eck 11-2005
    Public Const kGetCoInsurerDetailsName As String = "Returns CoInsurer Breakdown"
    Public Const kGetCoInsurerDetailsSQL As String = "spu_CLM_Get_CoInsurer_Split"

    ' Get current outstanding reserve and recovery amounts prior to close claim
    Public Const ACGetCurrentReserveRecoverySQL As String = "spu_GetCurrentReserveRecovery"
    Public Const ACGetCurrentReserveRecoveryName As String = "GetCurrentReserveRecovery"

    Public Const kAddPMMessageStored As Boolean = True
    Public Const kAddPMMessageName As String = "AddPMMessage"
    Public Const kAddPMMessageSQL As String = "spu_Add_PMMessage"

    Public Const kGetClaimTypeAndCoverName As String = "Return Claim Type and Cover"
    Public Const kGetClaimTypeAndCoverSQL As String = "spu_Get_Claim_Type_And_Cover"

    Public Const kGetRetroactiveDateGISPropertyName As String = "Return Reactive Date"
    Public Const kGetRetroactiveDateGISPropertySQL As String = "spu_gis_get_retroactivedate_property"

    Public Const ACGetProgressStatusSQL As String = "spu_CLM_Get_Progress_Status"
    Public Const ACGetProgressStatusName As String = "spu_CLM_Get_Progress_Status"

    Public Const ACGetProgressStatusDetailsSQL As String = "spu_CLM_Get_Progress_Status_details"
    Public Const ACGetProgressStatusDetailsName As String = "spu_CLM_Get_Progress_Status_details"

    'PN 45245
    Public Const ACGetShowRiskDetailsUStored As Boolean = True
    Public Const ACGetShowRiskDetailsUName As String = "GetShowRiskDetails_U"
    Public Const ACGetShowRiskDetailsUSQL As String = "spu_GetShowRiskDetails_U"

    Public Const ACSelectUserAuthority As String = "spu_Specific_User_Authority_Sel"
	Public Const ACSelectUserAuthorityScrName As String = "SelectUserAuthority"
	Public Const ACSelectUserAuthorityStored As Boolean = True

    'PN: 73770
    Public Const ACGetClaimHandlerSQL As String = "spu_CLM_Get_Claim_Handler"
    Public Const ACGetClaimHandlerName As String = "spu_CLM_Get_Claim_Handler"

    Public Const ACGetRiskDetailsClaimStored As Boolean = True
    Public Const ACGetRiskDetailsClaimName As String = "Get risk details for claim"
    Public Const ACGetRiskDetailsClaimSQL As String = "spu_get_risk_details_for_claim"

    Public Const ACGetOtherPartyDetailStored As Boolean = True
    Public Const ACGetOtherPartyDetailName As String = "Get other party detail"
    Public Const ACGetOtherPartyDetailSQL As String = "spu_get_otherparty_details"

    Public Const ACGetUserotherpartyStored As Boolean = True
    Public Const ACGetUserotherpartySQL As String = "spu_Get_User_OtherPartyID"
    Public Const ACGetUserotherpartyName As String = "Get user other party detail"

End Module