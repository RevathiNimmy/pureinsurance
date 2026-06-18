Option Strict Off
Option Explicit On
Imports System
Module bCLMFindClaimSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	' ***************************************************************** '
	' Class Name: FindClaimSQL
	'
	' Date: 15/07/2000
	'
	' Description: Contains the SQL Statements to (Stored Procedures
	'              and Enbedded SQL) manipulate an FindClaim
	'
	' Edit History:Pandu
	' ***************************************************************** '
	
	' Find Claim from built query.
	Public Const ACClaimFileSearchStored As Boolean = False
	Public Const ACClaimFileSearchName As String = "GetClaimDetails"
	
	'DJM 03/03/2004 : Pass in user_id and source_id to procedure to restrict selection by valid branches.
	'JMK 18/05/2001 put UW search out of script and into SP to prevent date problems
	Public Const ACFindClaimDetailsUWStored As Boolean = True
	Public Const ACFindClaimDetailsUWName As String = "Find Claim Details U/W"
	Public Const ACFindClaimDetailsUWSQL As String = "spu_find_claim_details_u"
	
	' Delete any Work Claim record against this claim
	Public Const ACDeleteClaimStored As Boolean = True
	Public Const ACDeleteClaimName As String = "Delete  Claim"
	Public Const ACDeleteClaimSQL As String = "spu_delete_claims"
	
	' Copy Claim To Work GIS
	Public Const ACGISCopyClaimToWorkStored As Boolean = True
	Public Const ACGISCopyClaimToWorkName As String = "Copy Claim To Work GIS"
	Public Const ACGISCopyClaimToWorkSQLStart As String = "spg_"
	Public Const ACGISCopyClaimToWorkSQLEnd As String = "_copy_claim"
	
	' Get datamodel code for claim
	Public Const ACGetDatamodeCodeforClaimStored As Boolean = True
	Public Const ACGetDatamodeCodeforClaimName As String = "Get Datamodel Code for Claim"
	Public Const ACGetDatamodeCodeforClaimSQL As String = "spu_Get_DataModel_Code_For_Claim"
	
	' returns the gis policy link id etc for the specified claim
	Public Const ACGetGisPolicyLinkDetailsName As String = "Get Gis Policy Link Details for Claim"
	Public Const ACGetGisPolicyLinkDetailsSQL As String = "spu_CLM_Get_GIS_Policy_Link_Details"
	
	Public Const ACGISCopyDatasetStart As String = "spg_"
	Public Const ACGISCopyDatasetEnd As String = "_copy_dataset"
	
	Public Const ACUpdateGisPolicyLinkDetailsName As String = "Get Gis Policy Link Details for Claim"
	Public Const ACUpdateGisPolicyLinkDetailsSQL As String = "spu_CLM_Update_GIS_Policy_Link_Details"
	
	Public Const kCopyClaimName As String = "Copy the Claims Details and return the new claim id"
	Public Const kCopyClaimSQL As String = "spu_CLM_Copy_Claim"
	
	Public Const kGetClaimVersionsName As String = "get all claims version details for the specified claim id"
	Public Const kGetClaimVersionsSQL As String = "spu_CLM_Get_Claim_Version_Details"
	
	Public Const kFindClaimName As String = "returns the claims that match the specified search parameters"
	Public Const kFindClaimSQL As String = "spu_CLM_Find_Claim"
	
	Public Const kGetOriginalClaimIdName As String = "returns the base claim id for the claim specified"
	Public Const kGetOriginalClaimIdSQL As String = "spu_CLM_Get_Base_Claim"
	
	Public Const kCleanUpDirtyClaimsName As String = "cleans up dirty claims"
	Public Const kCleanUpDirtyClaimsSQL As String = "spu_CLM_Clean_Up_Dirty_Claims"
	
	Public Const kIsInfoOnlyClaimName As String = "Checks if a claim is set as information only"
	Public Const kIsInfoOnlyClaimSQL As String = "spu_CLM_Is_InfoOnly_Version"
	
	''''''''PLICO RFC-9 - Amit
	Public Const ACCLMGetOtherClaimsStored As Boolean = True
	Public Const ACCLMGetOtherClaimsName As String = "Get Claims for client added as Risk "
	Public Const ACCLMGetOtherClaimsSQL As String = "spu_claim_GetOtherClaims"
	
	Public Const ACGetReferredPaymentStored As Boolean = True
	Public Const ACGetReferredPaymentName As String = "Get Referred Payment"
	Public Const ACGetReferredPaymentSQL As String = "spu_CLM_Get_Referred_Payment_Count"
	
	Public Const kFindClaimDetailsUWStored As Boolean = True
	Public Const kFindClaimDetailsUWName As String = "Find Claim Details SFU"
	Public Const kFindClaimDetailsUWSQL As String = "spu_find_claim_details_sfu"
	
	'Start - Sankar - (WPRvb64 Media Type Status) - Paralleling
	Public Const kCheckPolicyReceiptMediaTypeStatusStored As Boolean = True
	Public Const kCheckPolicyReceiptMediaTypeStatusName As String = "Check Policy Receipt MediaTypeStatus For Claim Payment"
    Public Const kCheckPolicyReceiptMediaTypeStatusSQL As String = "spu_CLM_Check_Policy_Receipt_MediaType_Status"

    'End - Sankar - (WPRvb64 Media Type Status) - Paralleling

    Public Const ACGetOtherPartyDetailStored As Boolean = True
    Public Const ACGetOtherPartyDetailName As String = "Get other party detail"
    Public Const ACGetOtherPartyDetailSQL As String = "spu_get_otherparty_details"

    Public Const ACGetUserotherpartyStored As Boolean = True
    Public Const ACGetUserotherpartySQL As String = "spu_Get_User_OtherPartyID"
    Public Const ACGetUserotherpartyName As String = "Get user other party detail"

	Public Const kGetLatestClaimIdName As String = "returns the latest claim id for the claim reference specified"
	Public Const kGetLatestClaimIdSQL As String = "spu_CLM_Get_Latest_ClaimID"

End Module
