Module bSIRCreateVoidBusinessSQL
	Public Const ACVoidCopyAgentCommissionStored As Boolean = True
	Public Const ACVoidCopyAgentCommissionName As String = "CopyAgentCommission"
	Public Const ACVoidCopyAgentCommissionSQL As String = "spu_copy_void_agent_commission"

	'Public Const kGetMaxPolicyVersionStored As Boolean = True
	'Public Const kGetMaxPolicyVersionName As String = "GetMaxPolicyVersion"
	'Public Const kGetMaxPolicyVersionSQL As String = "spu_get_max_policy_version_no"

	Public Const ACCGetVoidRiskStored As Boolean = True
	Public Const ACGetVoidRiskName As String = "AddVoidRisk"
	Public Const ACGetVoidRiskSQL As String = "spe_Risk_saa"

	Public Const ACGetAutoReinsuredStored As Boolean = False
	Public Const ACGetAutoReinsuredName As String = "Get Auto Reinsured"
	Public Const ACGetAutoReinsuredSQL As String = "SELECT is_auto_reinsured FROM risk_type WHERE risk_type_id = {risk_type_id}"

	Public Const ACAddVoidRiskStored As Boolean = True
	Public Const ACAddVoidRiskName As String = "AddVoidRisk"
	Public Const ACAddVoidRiskSQL As String = "spe_Risk_add"

	Public Const ACCopyRatingSectionForVoidStored As Boolean = True
	Public Const ACCopyRatingSectionForVoidName As String = "CopyRatingSection"
	Public Const ACCopyRatingSectionForVoidSQL As String = "spu_Copy_Rating_Section_For_Void_Transaction"

	Public Const ACCopyRiskTaxStored As Boolean = True
	Public Const ACCopyRiskTaxName As String = "CopyRiskTax"
	Public Const ACCopyRiskTaxSQL As String = "spu_Void_Risk_Tax_Fee_Copy"

	Public Const ACGetRiskStatusStored As Boolean = True
	Public Const ACGetRiskStatusName As String = "GetRiskStatus"
	Public Const ACGetRiskStatusSQL As String = "spu_CLM_risk_status_sel"

	'Add Section and Perils SQL
	Public Const kAddSectionAndPerilsStored As Boolean = True
	Public Const kAddSectionAndPerilsName As String = "AddSectionAndPerils"
	Public Const kAddSectionAndPerilsSQL As String = "spu_Copy_Peril_Reversal"

	Public Const kDelRatingSectionForDeletedRiskStored As Boolean = False
	Public Const kDelRatingSectionForDeletedRiskName As String = "DeleteRatingSectionForDeletedRisk"
	Public Const kDelRatingSectionForDeletedRiskSQL As String = "Delete From Rating_section where risk_cnt = {risk_cnt}"

	Public Const kDelPerilForDeletedRiskStored As Boolean = False
	Public Const kDelPerilForDeletedRiskName As String = "DeletePerilForDeletedRisk"
	Public Const kDelPerilForDeletedRiskSQL As String = " delete from peril where risk_cnt={risk_cnt}"


	Public Const ACGetAllocationDetailStored As Boolean = True
	Public Const ACGetAllocationDetailName As String = "GetAllocationDetail"
	Public Const ACGetAllocationDetailSQL As String = "spu_Get_Remaining_AllocationDetail"

	Public Const ACGetAllocationStored As Boolean = True
	Public Const ACGetAllocationName As String = "GetAllocationID"
	Public Const ACGetAllocationSQL As String = "spu_Get_AllocationID"

	'Public Const ACGetAccountIDStored As Boolean = False
	'Public Const ACGetAccountIDName As String = "GetAccountID_Transdetailid_by_allocationid"
	'Public Const ACGetAccountIDSQL As String = "SELECT TOP 1 TD.transdetail_id, td.account_id from TransDetail as TD " &
	'										   "INNER JOIN document as D on D.document_id = TD.document_id " &
	'										   "AND d.insurance_file_cnt = {insurance_file_cnt} 
	'											AND SPARE = 'GROSS'"

	Public Const ACGetTransDetailIDStored As Boolean = False
	Public Const ACGetTransDetailIDName As String = "GetAccountID_Transdetailid_by_allocationid"
	Public Const ACGetTransDetailIDSQL As String = "SELECT distinct ad.transdetail_id FROM TransDetail as TD inner join document as D on D.document_id = TD.document_id " &
												"inner join Insurance_File as ifi on ifi.insurance_file_cnt = d.insurance_file_cnt " &
												 "inner join AllocationDetail as AD on AD.transdetail_id = TD.transdetail_id " &
												 "Where d.insurance_file_cnt = {insurance_file_cnt}"
	' "LEFT JOIN AllocationDetail As AD On AD.transdetail_id = TD.transdetail_id " &
	'"WHERE ad.allocation_id = {allocation_id} " &


	Public Const ACInsertVoidReversalSQL As String = "spu_void_transaction_log_add"
	Public Const ACInsertVoidReversalName As String = "InsertVoidReversalLog"
	Public Const ACInsertVoidReversalStored As Boolean = True

	Public Const ACUpdateVoidReversalDetailSQL As String = "UPDATE void_reverse_transaction_log_detail SET is_reverse_allocated = 1 , parent_document_ref = {parentDocRef} WHERE " &
															"reverse_transaction_log_id = {reverse_transaction_log_id} AND allocation_id = {allocationid}"
	Public Const ACCUpdateVoidReversalDetailName As String = "UpdateVoidReversalDetail"
	Public Const ACCUpdateVoidReversalDetailStored As Boolean = False


	Public Const ACCheckInstalmentIsCollectedSQL As String = "select isnull(pfprem_finance_cnt,0)  from PFPremiumFinance where insurance_file_cnt = {insurance_file_cnt} "

	Public Const ACCheckInstalmentIsCollectedName As String = "CheckInstalmentIsCollected"
	Public Const ACCheckInstalmentIsCollectedStored As Boolean = False


	Public Const ACUpdPolicyStatusStored As Boolean = True
	Public Const ACUpdPolicyStatusName As String = "UpdatePolicyStatus"
	Public Const ACUpdPolicyStatusSQL As String = "spu_update_void_policy_status"

	Public Const ACCreateVoidPolicyStored As Boolean = True
	Public Const ACCreateVoidPolicyName As String = "CreatePolicy"
	Public Const ACCreateVoidPolicySQL As String = "spu_sir_copy_policy_for_void_transaction"


	Public Const ACResetPolicyStatusStored As Boolean = False
	Public Const ACResetPolicyStatusName As String = "ResetPolicyStatus"
	Public Const ACResetPolicyStatusSQL As String = "UPDATE Insurance_File SET insurance_file_status_id = null, " &
													"insurance_file_type_id = {insurance_file_type_id}, " &
													"original_insurance_file_type_id = null " &
													"WHERE insurance_file_cnt = {insurance_file_cnt}"

	Public Const ACCheckPolicyForVoidStored As Boolean = True
	Public Const ACCheckPolicyForVoidName As String = "CheckPolicyForVoid"
	Public Const ACCheckPolicyForVoidSQL As String = "spu_is_policy_valid_for_void_transaction"

	Public Const ACGetQuotePolicyVersionsStored As Boolean = True
	Public Const ACGetQuotePolicyVersionsName As String = "GETMTAQuotePolicyVersions"
	Public Const ACGetQuotePolicyVersionsSQL As String = "spu_SIR_Get_MTA_RenewalQuotePolicyVersions"

End Module
