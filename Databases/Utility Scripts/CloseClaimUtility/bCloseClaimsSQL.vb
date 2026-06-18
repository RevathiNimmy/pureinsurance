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

    Public Const kGetClaimRollbackTransactionsName As String = "get reserve rollback transactions for the specified claim id"
    Public Const kGetClaimRollbackTransactionsSQL As String = "Spu_CLM_Get_Rollback_Transactions"

    Public Const kGetClaimstoCloseOption1 As String = "select * from (select C.Claim_id, C.Claim_Number as 'Claim Number',inf.insured_name As 'Insured', C.Policy_id, inf.insurance_ref As 'Policy Number', C.Loss_from_date As 'Date of Loss',res3.[description] As 'Class of Business',cs.[description] As 'Claim Status',ps.[description] As 'Claim Progress Status',res2.[Reserve Outstanding] from Claim C " &
        "inner join (select Claim_Number, max(version_id) as 'Version' from claim group by Claim_Number) res on res.Claim_Number =				c.Claim_Number and res.Version = C.version_id " &
        "inner join Insurance_File inf on C.Policy_id = inf.insurance_file_cnt " &
        "inner join (select cpt.Claim_id,(SUM(rt.Initial_reserve+rt.Revised_reserve) - SUM(rt.Paid_to_date)) As 'Reserve Outstanding' from		Claim_Peril cpt inner join Reserve rt on cpt.Claim_Peril_id = rt.claim_Peril_id " &
        "group by Claim_id) res2 on res2.Claim_id = C.Claim_id " &
        "inner join (select distinct cpp.Claim_id, ptp.class_of_business_id, cob.description from Claim_Peril cpp inner join Peril_Type ptp		on cpp.Peril_type_id = ptp.peril_type_id inner join Class_Of_Business cob on cob.class_of_business_id =							ptp.class_of_business_id) res3 on res3.Claim_id = C.Claim_id " &
        "inner join Claim_Status cs on cs.claim_status_id = C.Claim_Status_id " &
        "inner join progress_status ps on ps.progress_status_id = C.Progress_Status_id " &
        "where (C.Claim_Status_id in (2,4) and C.Loss_from_date <= '2014-12-31 00:00:00.000' and C.Progress_Status_id=16)) a"

    Public Const kGetClaimstoCloseOption2 As String = "select * from (select C.Claim_id, C.Claim_Number as 'Claim Number',inf.insured_name As 'Insured', C.Policy_id, inf.insurance_ref As 'Policy Number', C.Loss_from_date As 'Date of Loss',res3.[description] As 'Class of Business',cs.[description] As 'Claim Status',ps.[description] As 'Claim Progress Status',res2.[Reserve Outstanding] from Claim C " &
        "inner join (select Claim_Number, max(version_id) as 'Version' from claim group by Claim_Number) res on res.Claim_Number =				c.Claim_Number and res.Version = C.version_id " &
        "inner join Insurance_File inf on C.Policy_id = inf.insurance_file_cnt " &
        "inner join (select cpt.Claim_id,(SUM(rt.Initial_reserve+rt.Revised_reserve) - SUM(rt.Paid_to_date)) As 'Reserve Outstanding' from		Claim_Peril cpt inner join Reserve rt on cpt.Claim_Peril_id = rt.claim_Peril_id " &
        "group by Claim_id) res2 on res2.Claim_id = C.Claim_id " &
        "inner join (select distinct cpp.Claim_id, ptp.class_of_business_id, cob.description from Claim_Peril cpp inner join Peril_Type ptp		on cpp.Peril_type_id = ptp.peril_type_id inner join Class_Of_Business cob on cob.class_of_business_id =							ptp.class_of_business_id) res3 on res3.Claim_id = C.Claim_id " &
        "inner join Claim_Status cs on cs.claim_status_id = C.Claim_Status_id " &
        "inner join progress_status ps on ps.progress_status_id = C.Progress_Status_id " &
        "where (C.Claim_Status_id in (2,4) and C.Loss_from_date <= '2010-12-31 00:00:00.000' and C.Progress_Status_id not in (18,32,33,29,30,12,34,20,31,28,35))) b"

    Public Const kGetClaimstoCloseOption3 As String = "select * from (select C.Claim_id, C.Claim_Number as 'Claim Number',inf.insured_name As 'Insured', C.Policy_id, inf.insurance_ref As 'Policy Number', C.Loss_from_date As 'Date of Loss',res3.[description] As 'Class of Business',cs.[description] As 'Claim Status',ps.[description] As 'Claim Progress Status',res2.[Reserve Outstanding] from Claim C " &
        "inner join (select Claim_Number, max(version_id) as 'Version' from claim group by Claim_Number) res on res.Claim_Number =				c.Claim_Number and res.Version = C.version_id " &
        "inner join Insurance_File inf on C.Policy_id = inf.insurance_file_cnt " &
        "inner join (select cpt.Claim_id,(SUM(rt.Initial_reserve+rt.Revised_reserve) - SUM(rt.Paid_to_date)) As 'Reserve Outstanding' from		Claim_Peril cpt inner join Reserve rt on cpt.Claim_Peril_id = rt.claim_Peril_id " &
        "group by Claim_id) res2 on res2.Claim_id = C.Claim_id " &
        "inner join (select distinct cpp.Claim_id, ptp.class_of_business_id, cob.description from Claim_Peril cpp inner join Peril_Type ptp		on cpp.Peril_type_id = ptp.peril_type_id inner join Class_Of_Business cob on cob.class_of_business_id =							ptp.class_of_business_id) res3 on res3.Claim_id = C.Claim_id " &
        "inner join Claim_Status cs on cs.claim_status_id = C.Claim_Status_id " &
        "inner join progress_status ps on ps.progress_status_id = C.Progress_Status_id " &
        "where (C.Claim_Status_id in (2,4) and C.Loss_from_date <= '2015-12-31 00:00:00.000' and C.Progress_Status_id not in (18,32,33,29,30,12,34,20,31,28,35) and res3.class_of_business_id=6)) c"

    Public Const kUpdateClaimDetails As String = "UPDATE claim SET Claim_Status_id = {Claim_Status_id},Progress_Status_id = {Progress_Status_id} WHERE Claim_id = {Claim_id}"

    Public Const kGetClaimStatus As String = "select CONVERT(varchar(10), claim_status_id)  + ' - ' + [description] from Claim_Status where is_deleted=0"

    Public Const kGetClassOfBusiness As String = "select CONVERT(varchar(10), class_of_business_id)  + ' - ' + [description] from Class_Of_Business where is_deleted=0"

    Public Const kGetProduct As String = "select CONVERT(varchar(10), product_id)  + ' - ' + [description] from Product where is_deleted=0"

    Public Const kGetProgressStatus As String = "select CONVERT(varchar(10), progress_status_id)  + ' - ' + [description] from progress_status where is_deleted=0"

    'Public Const kGetClaimstoClose As String = "select C.Claim_id, C.Claim_Number as 'Claim Number',inf.insured_name As 'Insured', C.Policy_id, inf.insurance_ref As 'Policy Number', C.Loss_from_date As 'Date of Loss',res3.[description] As 'Class of Business',cs.[description] As 'Claim Status',ps.[description] As 'Claim Progress Status',res2.[Reserve Outstanding] from Claim C " &
    '    "inner join (select Claim_Number, max(version_id) as 'Version' from claim group by Claim_Number) res on res.Claim_Number =				c.Claim_Number and res.Version = C.version_id " &
    '    "inner join Insurance_File inf on C.Policy_id = inf.insurance_file_cnt " &
    '    "inner join (select cpt.Claim_id,(SUM(rt.Initial_reserve+rt.Revised_reserve) - SUM(rt.Paid_to_date)) As 'Reserve Outstanding' from		Claim_Peril cpt inner join Reserve rt on cpt.Claim_Peril_id = rt.claim_Peril_id " &
    '    "group by Claim_id) res2 on res2.Claim_id = C.Claim_id " &
    '    "inner join (select distinct cpp.Claim_id, ptp.class_of_business_id, cob.description from Claim_Peril cpp inner join Peril_Type ptp		on cpp.Peril_type_id = ptp.peril_type_id inner join Class_Of_Business cob on cob.class_of_business_id =							ptp.class_of_business_id) res3 on res3.Claim_id = C.Claim_id " &
    '    "inner join Claim_Status cs on cs.claim_status_id = C.Claim_Status_id " &
    '    "inner join progress_status ps on ps.progress_status_id = C.Progress_Status_id "

    Public Const kGetClaimstoClose As String = " Select c.Claim_id,c.Claim_Number,c.Policy_id,c.Policy_Number,c.Loss_from_date,(Reserve-Payments) balance from ( " & _
                                        " (SELECT   cp.Claim_id, SUM(ISNULL(r.paid_to_date,0)) AS Payments,ISNULL(SUM(ISNULL(r.revised_reserve,0)),0) + ISNULL(SUM(ISNULL(r.initial_reserve,0)),0) Reserve  " & _
           "FROM RESERVE R WITH (NOLOCK) JOIN  claim_peril  cp WITH(NOLOCK) ON R.claim_Peril_id = cp.Claim_Peril_id WHERE Cp.Claim_id IN ( SELECT DISTINCT c.claim_id  " & _
"FROM Claim c WITH (NOLOCK) INNER JOIN insurance_file ifi WITH (NOLOCK) ON ifi.insurance_file_cnt = c.Policy_id " & _
" INNER JOIN product prd WITH (NOLOCK) ON ifi.product_id = prd.product_id INNER JOIN source s WITH (NOLOCK) ON ifi.source_id = s.source_id  " & _
" LEFT JOIN Handler h WITH (NOLOCK) ON c.handler_id = h.handler_id LEFT JOIN primary_cause pc WITH (NOLOCK) ON c.primary_cause_id = pc.primary_cause_id  " & _
"LEFT JOIN secondary_cause sc WITH (NOLOCK) ON c.secondary_cause_id = sc.secondary_cause_id LEFT JOIN progress_status ps WITH (NOLOCK) ON c.progress_status_id = ps.progress_status_id  " & _
"LEFT JOIN currency cu WITH (NOLOCK) ON cu.currency_id = c.currency_id LEFT JOIN catastrophe_code cc WITH(NOLOCK) ON cc.catastrophe_code_id = c.catastrophe_code_id  " & _
"LEFT JOIN party ag WITH(NOLOCK) ON ag.party_cnt = ifi.lead_agent_cnt " & _
" LEFT JOIN claim_status cs WITH(NOLOCK) ON cs.claim_status_id = c.claim_status_id LEFT JOIN party tpa WITH(NOLOCK) ON tpa.party_cnt = c.other_party_id " & _
" INNER JOIN (SELECT MAX(Version_id) as version_id,MAX(Claim_Id) as claim_id, base_claim_id FROM claim WITH (NOLOCK) WHERE " & _
" claim.claim_number like '%%' AND is_dirty = 0 " & _
" and claim.Claim_Status_id=3 GROUP BY base_claim_id ) claim_version ON c.claim_id = claim_version.claim_id LEFT JOIN [case] WITH (NOLOCK)  ON [case].case_id = c.base_case_id WHERE c.claim_number like '%%' AND ifi.source_id NOT IN " & _
" (SELECT source_id FROM pmuser_source WITH (NOLOCK) WHERE user_id = 2) ) GROUP BY cp.claim_id)   ) A  join claim c on a.Claim_id=c.Claim_id   where Payments-Reserve<>0"

End Module
