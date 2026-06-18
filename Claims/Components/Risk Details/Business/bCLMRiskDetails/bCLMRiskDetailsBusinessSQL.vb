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
	' Date: 21st August 2000
	'
	' Description: Contains the SQL Statements required by the
	'              bCLMRiskDetails.Business class.
	'
	' Edit History:
	'               21st Oct DG : Added a functionality. On pressing OK, in Risk Details screen,
	'                             when the Sum(CurrentReserve) = 0 then a message will come up
	'                             asking the User wether the Claim can be closed.
	'                             If the reply is YES the Claim status is set to closed.
	' ***************************************************************** '
	
	'SQL Statements
	
	Public Const ACGetBasicClaimDetailsStored As Boolean = True
	Public Const ACGetBasicClaimDetailsName As String = "Get basic claim details"
	Public Const ACGetBasicClaimDetailsSQL As String = "spu_get_basic_claim_details"
	
	Public Const ACGetCommentsForClaimStored As Boolean = True
	Public Const ACGetCommentsForClaimName As String = "Get comments for claim"
	Public Const ACGetCommentsForClaimSQL As String = "spu_Get_ClaimRisk_Comments"
	
	Public Const ACGetCommentsForClaimStoredBR As Boolean = True
	Public Const ACGetCommentsForClaimNameBR As String = "Get comments for claim"
	Public Const ACGetCommentsForClaimSQLBR As String = "spu_Claim_Comments_Sel"
	
	Public Const ACGetDescFromClaimRiskStored As Boolean = True
	Public Const ACGetDescFromClaimRiskName As String = "Get desc for claim"
	Public Const ACGetDescFromClaimRiskSQL As String = "spu_Get_ClaimRisk_Desc"
	
	Public Const ACGetFieldsForRiskDataDefnStored As Boolean = True
	Public Const ACGetFieldsForRiskDataDefnName As String = "Get fields and their values for risk data definition"
	Public Const ACGetFieldsForRiskDataDefnSQL As String = "spu_getfields_for_rskdatadefn"
	
	Public Const ACGetDataForRiskDataDefnStored As Boolean = True
	Public Const ACGetDataForRiskDataDefnName As String = "Get fields and their values for risk data definition"
	Public Const ACGetDataForRiskDataDefnSQL As String = "spu_getdata_for_rskdatadefn"
	
	Public Const ACGetPartyTypesforRiskTypeStored As Boolean = True
	Public Const ACGetPartyTypesforRiskTypeName As String = "Get all party types for risk type"
	Public Const ACGetPartyTypesforRiskTypeSQL As String = "spu_get_partytypes_for_risk"
	
	Public Const ACGetPartyDetailsForClaimStored As Boolean = True
	Public Const ACGetPartyDetailsforClaimName As String = "Get party details for party type"
	Public Const ACGetPartyDetailsforClaimSQL As String = "spu_partydet_for_party_type"
	
	Public Const ACCheckForExistenceinClaimRiskStored As Boolean = True
	Public Const ACCheckForExistenceinClaimRiskName As String = "Check for existence in Claim_Risk"
	Public Const ACCheckForExistenceinClaimRiskSQL As String = "spu_check_existence_in_clmrsk"
	
	Public Const ACAddClaimRiskStored As Boolean = True
	Public Const ACAddClaimRiskName As String = "Add to Claim_risk"
	Public Const ACAddClaimRiskSQL As String = "spu_add_claim_risk"
	
	Public Const ACAddClaimPartyClaimStored As Boolean = True
	Public Const ACAddClaimPartyClaimName As String = "Add to Claim_Party_Claim"
	Public Const ACAddClaimPartyClaimSQL As String = "spu_add_claim_party_claim"
	
	Public Const ACDeleteClaimPartyClaimStored As Boolean = True
	Public Const ACDeleteClaimPartyClaimName As String = "DeleteClaimPartyClaim"
	Public Const ACDeleteClaimPartyClaimSQL As String = "spu_delete_claim_party_claim"
	
	Public Const ACGetPerilTypeForRiskStored As Boolean = True
	Public Const ACGetPerilTypeForRiskName As String = "GetPerilTypeForRisk"
	Public Const ACGetPerilTypeForRiskSQL As String = "spu_Get_Peril_type_For_Risk"
	
	Public Const ACAddClaimPerilStoredU As Boolean = True
	Public Const ACAddClaimPerilNameU As String = "AddClaimPeril"
	Public Const ACAddClaimPerilSQLU As String = "spu_Add_Claim_Peril"
	
	Public Const ACGetLookupTablesStored As Boolean = True
	Public Const ACGetLookupTablesName As String = "Get lookup tables"
	Public Const ACGetLookupTablesSQL As String = "spu_Get_Lookup_Tables"
	
	Public Const ACAddGeneralDetailStored As Boolean = True
	Public Const ACAddGeneralDetailName As String = "AddGeneralDetail"
	Public Const ACAddGeneralDetailSQL As String = "spu_Add_General_Details"
	
	Public Const ACGetRiskDetailsStored As Boolean = True
	Public Const ACGetRiskDetailsName As String = "GetRiskDetails"
	Public Const ACGetRiskDetailsSQL As String = "spu_GetRiskDetails"
	
	Public Const ACGetPerilForClaimRiskStored As Boolean = True
	Public Const ACGetPerilForClaimRiskName As String = "GetPerilForClaimRisk"
	Public Const ACGetPerilForClaimRiskSQL As String = "spu_Get_Peril_For_claim_Risk"
	
	Public Const ACCheckDeletionForPerilStored As Boolean = True
	Public Const ACCheckDeletionForPerilName As String = "CheckDeletionForPeril"
	Public Const ACCheckDeletionForPerilSQL As String = "spu_check_deletion_for_peril"
	
	Public Const ACDeletePerilStored As Boolean = True
	Public Const ACDeletePerilName As String = "DeletePeril"
	Public Const ACDeletePerilSQL As String = "spu_deleteperil"
	
	Public Const ACAddPerilForClaimRiskStored As Boolean = True
	Public Const ACAddPerilForClaimRiskName As String = "AddPerilForClaimRisk"
	Public Const ACAddPerilForClaimRiskSQL As String = "spu_add_peril_claim_risk"
	
	Public Const ACGetShowRiskDetailsStored As Boolean = True
	Public Const ACGetShowRiskDetailsName As String = "GetShowRiskDetails"
	Public Const ACGetShowRiskDetailsSQL As String = "spu_GetShowRiskDetails"
	
	Public Const ACGetPolicynumberStored As Boolean = True
	Public Const ACGetPolicynumberName As String = "GetPolicynumber"
	Public Const ACGetPolicynumberSQL As String = "spu_GetPolicynumber"
	
	Public Const ACCloseClaimStored As Boolean = True
	Public Const ACCloseClaimName As String = "CloseClaimName"
	Public Const ACCloseClaimSQL As String = "spu_CloseClaim"
	
	Public Const ACGetCurrentReserveRecoveryStored As Boolean = True
	Public Const ACGetCurrentReserveRecoveryName As String = "GetCurrentReserveRecovery"
	Public Const ACGetCurrentReserveRecoverySQL As String = "spu_GetCurrentReserveRecovery"
	
	
	Public Const DataTypeText As Integer = 1
	Public Const DataTypeInteger As Integer = 2
	Public Const DataTypeDate As Integer = 3
	Public Const DataTypeBoolean As Integer = 4
	Public Const DataTypeLookup As Integer = 5
	Public Const DataTypeParty As Integer = 6
	
	Public Const ACSelRiskDetailStored As Boolean = True
	Public Const ACSelRiskDetailName As String = "GetRiskDetails"
	Public Const ACSelRiskDetailSQL As String = "spu_CLM_Get_Claim_Link_Details"
	
	Public Const ACDeleteClaimStored As Boolean = True
	Public Const ACDeleteClaimName As String = "Delete Work Claim"
	Public Const ACDeleteClaimSQL As String = "spu_delete_claim"
	
	Public Const ACGetOriginalClaimIDName As String = "Get Original Claim ID"
	Public Const ACGetOriginalClaimIDSQL As String = "spu_CLM_Get_Base_Claim"
	
	Public Const ACGetInfoOnlyStatusStored As Boolean = True
	Public Const ACGetInfoOnlyStatusName As String = "GetInfoOnlyStatus"
	Public Const ACGetInfoOnlyStatusSQL As String = "spu_get_claim_info_only_status"
	
	Public Const ACGetPolicyTypeStored As Boolean = True
	Public Const ACGetPolicyTypeName As String = "GetPolicyType"
	Public Const ACGetPolicyTypeSQL As String = "spu_Get_Policy_Type"
	
	Public Const ACGetDriversStored As Boolean = True
	Public Const ACGetDriversName As String = "GetDrivers"
	Public Const ACGetDriversSQL As String = "spu_SIRRen_Get_Drivers"
	
	Public Const ACReCloseClaimStored As Boolean = True
	Public Const ACReCloseClaimName As String = "ReCloseClaimName"
	Public Const ACReCloseClaimSQL As String = "spu_ReCloseClaim"
	
	Public Const ACReOpenClaimStored As Boolean = True
	Public Const ACReOpenClaimName As String = "ReOpenClaimName"
	Public Const ACReOpenClaimSQL As String = "spu_ReOpenClaim"
	
	Public Const ACGetClaimCommentsStored As Boolean = True
	Public Const ACGetClaimComments As String = "GetClaimCommemts"
	Public Const ACGetClaimCommentsSQL As String = "spu_claim_comments_sel"
	
	Public Const ACDeleteClaimCommentsStored As Boolean = True
	Public Const ACDeleteClaimCommentsName As String = "DeleteClaimComments"
	Public Const ACDeleteClaimCommentsSQL As String = "spu_claim_comments_del"
	
	Public Const ACAddClaimCommentsStored As Boolean = True
	Public Const ACAddClaimCommentsName As String = "AddClaimComments"
	Public Const ACAddClaimCommentsSQL As String = "spu_claim_comments_add"
	
	Public Const ACGetClientPolicyDetailsStored As Boolean = True
	Public Const ACGetClientPolicyDetailsName As String = "GetClientPolicyDetails"
	Public Const ACGetClientPolicyDetailsSQL As String = "spu_get_client_policy_details"
	
	Public Const ACGetRiskTypeScreenIDStored As Boolean = True
	Public Const ACGetRiskTypeScreenIDName As String = "GetRiskTypeScreenID"
	Public Const ACGetRiskTypeScreenIDSQL As String = "spu_get_risk_type_screenID"
	
	Public Const ACGetProgressStatusCodeStored As Boolean = False
	Public Const ACGetProgressStatusCodeName As String = "Get Progress Status Code"
	Public Const ACGetProgressStatusCodeSQL As String = "spu_CLM_Get_Progress_Status_Code"
	
	
	Public Const ACGetRiskTypeName As String = "Returns the risk_type_id for the specified risk id"
	Public Const ACGetRiskTypeSQL As String = "spu_CLM_Get_Risk_Type"
	
	Public Const ACGetClaimGisDataName As String = " Returns the required gis data for the claim"
	Public Const ACGetClaimGisDataSQL As String = "spu_CLM_Get_Gis_Data"
	
	Public Const ACGetGISScreenIDStored As Boolean = True
	Public Const ACGetGISScreenIDName As String = "GetGISScreenID"
	Public Const ACGetGISScreenIDSQL As String = "spu_claim_get_screen_id"
	
	Public Const ACSaveGISScreenIDStored As Boolean = True
	Public Const ACSaveGISScreenIDName As String = "SaveGISScreenID"
	Public Const ACSaveGISScreenIDSQL As String = "spu_claim_upd_screen_id"
	
	Public Const ACDeleteGISDetailsName As String = "Deletes the dataset and gis policy link record for the specified work claim"
	Public Const ACDeleteGISDetailsSQL As String = "spu_CLM_Delete_GIS_DataSet"
	
	Public Const ACGetGisPolicyLinkDetailsName As String = "Get Gis Policy Link Details for Claim"
	Public Const ACGetGisPolicyLinkDetailsSQL As String = "spu_CLM_Get_GIS_Policy_Link_Details"
	
	Public Const ACBalanceReserveSQL As String = "spu_reserve_balance"
	Public Const ACBalanceReserveName As String = "BalanceReserve"
	
	Public Const ACGetClaimPerilIDSQL As String = "Spu_CLM_Get_Claim_Peril_id"
	Public Const ACGetClaimPerilIDName As String = "Get_Claim_Peril_id"
	
	Public Const kGetClassOfBusinessName As String = "get class of business for peril"
	Public Const kGetClassOfBusinessSQL As String = "spu_CLM_Get_Claim_Peril_Class_Of_Business"

	Public Const ACUpdateWrkManagerForReserveLimitStored As Boolean = True
	Public Const ACUpdateWrkManagerForReserveLimit As String = "UpdateWorkManagerForReserveLimit"
	Public Const ACUpdateWrkManagerForReserveLimitSQL As String = "spu_PMWrk_Task_Instance_upd_For_Reserve_Limit"
End Module
