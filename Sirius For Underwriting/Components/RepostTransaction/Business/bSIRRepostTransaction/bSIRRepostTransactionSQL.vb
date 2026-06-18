Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Module BusinessSQL

    'SQL Statements

    'update transaction export folder
    Public Const ACUpdTransExportFolderStored As Boolean = False
    Public Const ACUpdTransExportFolderName As String = "UpdateTransactionExportFolder"
	Public Const ACUpdTransExportFolderSQL As String = "UPDATE transaction_export_folder" & Strings.Chr(13) & Strings.Chr(10) &  _
	                                                   "SET accounts_export_status = {accounts_export_status}" & Strings.Chr(13) & Strings.Chr(10) &  _
	                                                   "WHERE transaction_export_folder_cnt = {transaction_export_folder_cnt}"
	
	'get failed transaction from transaction export folder (status = 'f')
	Public Const ACGetFailedTransactionsStored As Boolean = True
	Public Const ACGetFailedTransactionsName As String = "GetFailedTransaction"
    Public Const ACGetFailedTransactionsSQL As String = "spu_get_Failed_transaction"
	
	'delete stats folder
	Public Const ACDeleteStatsFolderStored As Boolean = True
	Public Const ACDeleteStatsFolderName As String = "Delete Stats Folder"
    Public Const ACDeleteStatsFolderSQL As String = "spu_DeleteStatsFolder"
	
	'delete stats detail
	Public Const ACDeleteStatsDetailStored As Boolean = True
	Public Const ACDeleteStatsDetailName As String = "Delete Stats Details"
    Public Const ACDeleteStatsDetailSQL As String = "spu_DeleteStatsDetail"

    'delete transaction export detail and folder
    Public Const ACDeleteTransactionExportStored As Boolean = True
    Public Const ACDeleteTransactionExportName As String = "Delete Transaction Export Detail and Folder"
    Public Const ACDeleteTransactionExportSQL As String = "spu_DeleteTransactionExport"

    'get stats folder count
    Public Const ACGetStatsFolderStored As Boolean = False
    Public Const ACGetStatsFolderName As String = "Get Stats Folder Cnt"
    Public Const ACGetStatsFolderSQL As String = "SELECT stats_folder_cnt FROM Stats_Folder WHERE insurance_file_cnt = {InsuranceFileCnt} AND document_ref = {DocumentRef}"

    'get transaction export folder count for this policy
    Public Const ACGetTransExportFolderStored As Boolean = False
    Public Const ACGetTransExportFolderName As String = "Get transaction export folder count"
    Public Const ACGetTransExportFolderSQL As String = "SELECT transaction_export_folder_cnt FROM Transaction_Export_Folder WHERE insurance_file_cnt = {InsuranceFileCnt}"

    ' Add StatsDetails SQL
    Public Const ACAddStatsDetailsReverseStored As Boolean = True
    Public Const ACAddStatsDetailsReverseName As String = "AddStatsDetailsReverse"
    Public Const ACAddStatsDetailsReverseSQL As String = "spu_add_stats_details_Reverse"

    Public Const ACAddStatsDetailsStored As Boolean = True
    Public Const ACAddStatsDetailsName As String = "AddStatsDetails"
    Public Const ACAddStatsDetailsSQL As String = "spu_add_stats_details"

    ' Add ExportFolder SQL
    Public Const ACAddExportFolderStored As Boolean = True
    Public Const ACAddExportFolderName As String = "AddExportFolder"
    Public Const ACAddExportFolderSQL As String = "spu_add_trans_export_folder"

    ' Add ExportDetails SQL
    Public Const ACAddExportDetailsStored As Boolean = True
    Public Const ACAddExportDetailsName As String = "AddExportDetails"
    Public Const ACAddExportDetailsSQL As String = "spu_add_trans_details_control"

    'get document_id from document via document_ref
    Public Const ACGetDocumentIDStored As Boolean = False
    Public Const ACGetDocumentIDName As String = "GetDocumentID"
    Public Const ACGetDocumentIDSQL As String = "SELECT Document_ID FROM Document WHERE document_ref = {DocumentRef}"

    'get all versions of policy
    Public Const ACGetPolicyVersionStored As Boolean = True
    Public Const ACGetPolicyVersionName As String = "GetPolicyVersion"
    Public Const ACGetPolicyVersionSQL As String = "spu_GetAllPolicyVersion"

    'check to see if this version of policy is in account
    Public Const ACGetPolicyDocStored As Boolean = True
    Public Const ACGetPolicyDocName As String = "Get document for this policy version"
    Public Const ACGetPolicyDocSQL As String = "spu_IsPolicyVersionInAccount"

    'get all document for this policy version
    Public Const ACGetPolicyDocumentStored As Boolean = False
    Public Const ACGetPolicyDocumentName As String = "Get document for this policy version"
    Public Const ACGetPolicyDocumentSQL As String = "SELECT document_ref FROM Stats_Folder WHERE insurance_file_cnt = {InsuranceFileCnt}"

    'delete document and all its allocations
    Public Const ACDelDocumentStored As Boolean = True
    Public Const ACDelDocumentName As String = "Delete document"
    Public Const ACDelDocumentSQL As String = "spu_DeleteDocument"

    'delete all allocation for this document
    Public Const ACDelDocAllocationStored As Boolean = True
    Public Const ACDelDocAllocationName As String = "Delete document's allocation"
    Public Const ACDelDocAllocationSQL As String = "spu_DeleteDocumentAllocation"

    'delete this version of policy and its dependancies
    Public Const ACDelPolicyVersionStored As Boolean = True
    Public Const ACDelPolicyVersionName As String = "Delete policy and its dependancies"
    Public Const ACDelPolicyVersionSQL As String = "spu_DeletePolicy"


    'get all claims which have been closed without transactions when these claims were zeroised.
    Public Const ACGetClosedClaimWithNoPostingStored As Boolean = True
    Public Const ACGetClosedClaimWithNoPostingName As String = "GetClosedClaimWithNoPosting"
    Public Const ACGetClosedClaimWithNoPostingSQL As String = "spu_GetClosedClaimWithNoPosting"

    'get claims peril details only interested in peril with either payment or reserve
    Public Const ACGetClaimPerilDetailStored As Boolean = False
    Public Const ACGetClaimPerilDetailName As String = "GetClaimPerilDetail"
    Public Const ACGetClaimPerilDetailSQL As String = "SELECT c.policy_id, c.policy_number, c.claim_number, cp.claim_peril_id, cob.class_of_business_id, cob.code," & Strings.Chr(13) & Strings.Chr(10) & _
                                                      "(SELECT IsNull(SUM(r.this_revision),0) FROM Work_Reserve r WHERE r.claim_peril_id = cp.claim_peril_id) TotalThisRevision," & Strings.Chr(13) & Strings.Chr(10) & _
                                                      "(SELECT IsNull(SUM(r.this_payment),0) FROM Work_Reserve r WHERE r.claim_peril_id = cp.claim_peril_id) TotalThisPayment," & Strings.Chr(13) & Strings.Chr(10) & _
                                                      "(SELECT TOP 1 IsNull(p.party_cnt,0) FROM Work_Payment p WHERE p.claim_id = c.claim_id AND p.claim_peril_id = cp.claim_peril_id ORDER BY p.payment_id DESC) PartyCnt" & Strings.Chr(13) & Strings.Chr(10) & _
                                                      "FROM Work_Claim c" & Strings.Chr(13) & Strings.Chr(10) & _
                                                      "JOIN Work_Claim_Peril cp ON c.claim_id = cp.claim_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                                      "JOIN Peril_Type pt ON pt.peril_type_id = cp.peril_type_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                                      "JOIN Class_Of_Business cob ON cob.class_of_business_id = pt.class_of_business_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                                      "WHERE c.claim_id = {WorkClaimID}" & Strings.Chr(13) & Strings.Chr(10) & _
                                                      "AND ((SELECT IsNull(SUM(r.this_revision),0)FROM Work_Reserve r WHERE r.claim_peril_id = cp.claim_peril_id) <> 0" & Strings.Chr(13) & Strings.Chr(10) & _
                                                      "OR (SELECT IsNull(SUM(r.this_payment),0) FROM Work_Reserve r WHERE r.claim_peril_id = cp.claim_peril_id) <> 0)"

    'get claims peril details with this_revision/this_payment supplied
    Public Const ACGetClaimPerilDetail2Stored As Boolean = False
    Public Const ACGetClaimPerilDetail2Name As String = "GetClaimPerilDetail"
    Public Const ACGetClaimPerilDetail2SQL As String = "SELECT c.policy_id, c.policy_number, c.claim_number, cp.claim_peril_id, cob.class_of_business_id, cob.code," & Strings.Chr(13) & Strings.Chr(10) & _
                                                       "{ThisRevision} TotalThisRevision," & Strings.Chr(13) & Strings.Chr(10) & _
                                                       "{ThisPayment} TotalThisPayment," & Strings.Chr(13) & Strings.Chr(10) & _
                                                       "IsNull((SELECT party_cnt FROM Payment WHERE payment_id = {PaymentID}),0) PartyCnt" & Strings.Chr(13) & Strings.Chr(10) & _
                                                       "FROM Work_Claim c" & Strings.Chr(13) & Strings.Chr(10) & _
                                                       "JOIN Work_Claim_Peril cp ON c.claim_id = cp.claim_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                                       "JOIN Work_Reserve r ON r.claim_peril_id = cp.claim_peril_id AND r.original_reserve_id = {OriginalReserveID}" & Strings.Chr(13) & Strings.Chr(10) & _
                                                       "JOIN Peril_Type pt ON pt.peril_type_id = cp.peril_type_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                                       "JOIN Class_Of_Business cob ON cob.class_of_business_id = pt.class_of_business_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                                       "WHERE c.claim_id = {WorkClaimID}"

    'update the work reserve to make it looks like we are doing a claim maintence
    'otherwise reinsurance won't do anything
    Public Const ACUpdateWorkReserveStored As Boolean = False
    Public Const ACUpdateWorkReserveName As String = "UpdateWorkReserve"
    Public Const ACUpdateWorkReserveSQL As String = "UPDATE Work_Reserve" & Strings.Chr(13) & Strings.Chr(10) & _
                                                    "SET this_revision = r.this_revision," & Strings.Chr(13) & Strings.Chr(10) & _
                                                    "Revised_reserve = r.Revised_reserve - r.this_revision," & Strings.Chr(13) & Strings.Chr(10) & _
                                                    "this_payment = r.this_payment," & Strings.Chr(13) & Strings.Chr(10) & _
                                                    "paid_to_date = r.paid_to_date - r.this_payment" & Strings.Chr(13) & Strings.Chr(10) & _
                                                    "FROM Work_Claim_Peril wcp" & Strings.Chr(13) & Strings.Chr(10) & _
                                                    "JOIN Work_Reserve wr ON wcp.claim_peril_id = wr.claim_peril_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                                    "JOIN Reserve r ON r.reserve_id = wr.original_reserve_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                                    "Where wcp.claim_id = {WorkClaimID}"

    'update work reserve to make it looks like we are doing a claim maintenance or claim payment
    'update value from input rather than from reserve.this_revision or reserve.this_payment
    Public Const ACUpdateWorkReserve2Stored As Boolean = False
    Public Const ACUpdateWorkReserve2Name As String = "UpdateWorkReserve2"
    'Public Const ACUpdateWorkReserve2SQL = "Update Work_Reserve" & vbCrLf & _
    '"SET this_revision = {ThisRevision}," & vbCrLf & _
    '"Revised_reserve = Revised_reserve - {ThisRevision}," & vbCrLf & _
    '"this_payment = {ThisPayment}," & vbCrLf & _
    '"paid_to_date = paid_to_date - {ThisPayment}" & vbCrLf & _
    '"WHERE original_reserve_id = {OriginalReserveID}"

    Public Const ACUpdateWorkReserve2SQL As String = "Update Work_Reserve" & Strings.Chr(13) & Strings.Chr(10) & _
                                                     "SET this_revision = {ThisRevision}," & Strings.Chr(13) & Strings.Chr(10) & _
                                                     "this_payment = {ThisPayment}" & Strings.Chr(13) & Strings.Chr(10) & _
                                                     "WHERE original_reserve_id = {OriginalReserveID}"

    'get all failed claim transactions including those do not have details in transaction export folder
    Public Const ACGetFailedClaimTransactionStored As Boolean = True
    Public Const ACGetFailedClaimTransactionName As String = "GetFailedClaimTransaction"
    Public Const ACGetFailedClaimTransactionSQL As String = "spu_GetFailedClaimTransaction"

    'get imbalanced closed claims
    Public Const ACGetImbalanceClosedClaimStored As Boolean = True
    Public Const ACGetImbalanceClosedClaimName As String = "GetImbalancedClosedClaim"
    Public Const ACGetImbalanceClosedClaimSQL As String = "spu_GetImbalaceClosedClaim"

    'get reserve details
    Public Const ACGetReserveDetailStored As Boolean = False
    Public Const ACGetReserveDetailName As String = "GetReserveDetails"
    Public Const ACGetReserveDetailSQL As String = "SELECT r.reserve_id, r.claim_peril_id, r.reserve_type_id, r.initial_reserve," & Strings.Chr(13) & Strings.Chr(10) & _
                                                   "r.revised_reserve , r.paid_to_date, r.this_revision, this_payment, rt.description, cp.description" & Strings.Chr(13) & Strings.Chr(10) & _
                                                   "FROM Reserve r" & Strings.Chr(13) & Strings.Chr(10) & _
                                                   "JOIN Reserve_Type rt ON r.reserve_type_id = rt.reserve_type_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                                   "JOIN Claim_Peril cp ON r.claim_peril_id = cp.claim_peril_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                                   "JOIN Claim c ON c.claim_id = cp.claim_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                                   "WHERE c.claim_number = {ClaimNumber}"

    'get payment details
    Public Const ACGetPaymentDetailStored As Boolean = False
    Public Const ACGetPaymentDetailName As String = "GetPaymentDetails"
    Public Const ACGetPaymentDetailSQL As String = "SELECT p.payment_id, p.reserve_id, p.claim_peril_id, p.amount, p.date_of_payment, IsNull(pt.shortname,'ClaimPayable')" & Strings.Chr(13) & Strings.Chr(10) & _
                                                   "FROM Payment p JOIN Claim c ON p.claim_id = c.claim_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                                   "LEFT JOIN Party pt ON pt.party_cnt = p.party_cnt" & Strings.Chr(13) & Strings.Chr(10) & _
                                                   "WHERE c.claim_number = {ClaimNumber}" & Strings.Chr(13) & Strings.Chr(10) & _
                                                   "AND IsNull(p.amount,0) <> 0"


    'get all reserve types which are not zero
    Public Const ACGetNoneZeroReserveDetailStored As Boolean = False
    Public Const ACGetNoneZeroReserveDetailName As String = "GetReserveDetails"
    Public Const ACGetNoneZeroReserveDetailSQL As String = "SELECT r.reserve_id, r.claim_peril_id, r.reserve_type_id, r.initial_reserve," & Strings.Chr(13) & Strings.Chr(10) & _
                                                           "r.revised_reserve , r.paid_to_date, r.this_revision, this_payment, rt.description, cp.description" & Strings.Chr(13) & Strings.Chr(10) & _
                                                           "FROM Reserve r" & Strings.Chr(13) & Strings.Chr(10) & _
                                                           "JOIN Reserve_Type rt ON r.reserve_type_id = rt.reserve_type_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                                           "JOIN Claim_Peril cp ON r.claim_peril_id = cp.claim_peril_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                                           "AND (IsNull(r.initial_reserve,0) <> 0 OR IsNull(revised_reserve,0) <> 0 OR IsNull(r.paid_to_date,0) <> 0)" & Strings.Chr(13) & Strings.Chr(10) & _
                                                           "JOIN Claim c ON c.claim_id = cp.claim_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                                           "WHERE c.claim_number = {ClaimNumber}"

    'get claim peril
    Public Const ACGetClaimPerilStored As Boolean = False
    Public Const ACGetClaimPerilName As String = "GetClaimPeril"
    Public Const ACGetClaimPerilSQL As String = "SELECT cp.claim_id, cp.claim_peril_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                                "FROM Claim_Peril cp join Reserve r on cp.claim_peril_id = r.claim_peril_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                                "Where cp.claim_id = {ClaimID}" & Strings.Chr(13) & Strings.Chr(10) & _
                                                "GROUP BY cp.claim_id, cp.claim_peril_id"


    'unique payment party code
    Public Const ACGetUniquePaymentPartyCodeStored As Boolean = False
    Public Const ACGetUniquePaymentPartyCodeName As String = "GetUniquePaymentPartyCode"
    Public Const ACGetUniquePaymentPartyCodeSQL As String = "SELECT IsNull(MAX(p.payment_id),0), p.claim_id, IsNull(pt.shortname,'ClaimPayable')" & Strings.Chr(13) & Strings.Chr(10) & _
                                                            "FROM Payment p JOIN Claim c ON p.claim_id = c.claim_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                                            "LEFT JOIN Party pt ON pt.party_cnt = p.party_cnt" & Strings.Chr(13) & Strings.Chr(10) & _
                                                            "WHERE c.claim_number = {ClaimNumber}" & Strings.Chr(13) & Strings.Chr(10) & _
                                                            "AND IsNull(p.amount,0) <> 0" & Strings.Chr(13) & Strings.Chr(10) & _
                                                            "GROUP BY p.claim_id, IsNull(pt.shortname,'ClaimPayable')"

    'update work_payment.original_payment_id = null so that stats is created with correct currency_code
    Public Const ACUpdateWorkPaymentStored As Boolean = False
    Public Const ACUpdateWorkPaymentName As String = "UpdateWorkPayment"
    Public Const ACUpdateWorkPaymentSQL As String = "UPDATE Work_Payment SET original_payment_id = null WHERE claim_id = {WorkClaimID} AND original_payment_id = {OriginalPaymentID}"

    Public Const ACIsColumnStored As Boolean = False
    Public Const ACIsColumnName As String = "IsOriginalPaymentID"
    Public Const ACIsColumnSQL As String = "SELECT sc.name FROM Syscolumns sc WHERE sc.id = object_id({TableName}) and sc.name = {ColumnName}"

    'get risk details for this policy version
    Public Const ACGetRiskDetailStored As Boolean = False
    Public Const ACGetRiskDetailName As String = "Get risk details"
    Public Const ACGetRiskDetailSQL As String = "SELECT ifrl.insurance_file_cnt, ifrl.risk_cnt, ifrl.status_flag, r.description, IsNull(rs.description,'Null') 'risk status'" & Strings.Chr(13) & Strings.Chr(10) & _
                                                "FROM Insurance_File_Risk_Link ifrl JOIN Risk r ON ifrl.risk_cnt = r.risk_cnt" & Strings.Chr(13) & Strings.Chr(10) & _
                                                "LEFT JOIN Risk_Status rs ON rs.risk_status_id = r.risk_status_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                                "Where ifrl.insurance_file_cnt = {InsuranceFileCnt}"

    'get transaction export for this policy version
    Public Const ACGetTransactionExportStored As Boolean = False
    Public Const ACGetTransactionExportName As String = "Get Transaction Export"
    Public Const ACGetTransactionExportSQL As String = "SELECT transaction_export_folder_cnt, insurance_file_cnt, insurance_ref, document_ref, document_date, accounts_export_status" & Strings.Chr(13) & Strings.Chr(10) & _
                                                       "FROM Transaction_Export_Folder WHERE insurance_file_cnt = {InsuranceFileCnt}"

    'Add RI to policy at risk level
    Public Const ACAddRIToPolicyStored As Boolean = True
    Public Const ACAddRIToPolicyName As String = "Add RI To Policy"
    Public Const ACAddRIToPolicySQL As String = "spu_AddRIToPolicy"

    'change policy status
    Public Const ACUpdPolicyStatusStored As Boolean = False
    Public Const ACUpdPolicyStatusName As String = "Update policy status"
    Public Const ACUpdPolicyStatusSQL As String = "UPDATE Insurance_File SET insurance_file_status_id = {InsuranceFileStatusID} WHERE insurance_file_cnt = {InsuranceFileCnt}"

    'populate policy status
    Public Const ACGetPolicyStatusStored As Boolean = False
    Public Const ACGetPolicyStatusName As String = "Populate Policy Status"
    Public Const ACGetPolicyStatusSQL As String = "SELECT insurance_file_status_id, [description] FROM Insurance_File_Status"

    'delete claim and all associated postings including stats
    Public Const ACDelClaimStored As Boolean = True
    Public Const ACDelClaimName As String = "Delete Claim"
    Public Const ACDelClaimSQL As String = "spu_DeleteClaim"

    'get claim postings
    Public Const ACGetClaimPostingStored As Boolean = True
    Public Const ACGetClaimPostingName As String = "Get claim postings"
    Public Const ACGetClaimPostingSQL As String = "spu_GetClaimPosting"

    Public Const ACAddStatsFolderRevesrseStored As Boolean = True
    Public Const ACAddStatsFolderRevesrseName As String = "AddStatsFolder"
    Public Const ACAddStatsFolderRevesrseSQL As String = "spu_add_stats_folder_reverse"

    'Get claim balance
    Public Const ACGetClaimBalanceStored As Boolean = False
    Public Const ACGetClaimBalanceName As String = "Get claim balance"
    Public Const ACGetClaimBalanceSQL As String = "SELECT ISNULL(SUM(wr.initial_reserve) + SUM(wr.revised_reserve) - SUM(wr.paid_to_date),0)" & Strings.Chr(13) & Strings.Chr(10) & _
                                                  "FROM Work_Claim wc JOIN Work_Claim_Peril wcp ON wc.claim_id = wcp.claim_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                                  "JOIN Work_Reserve wr ON wr.claim_peril_id = wcp.claim_peril_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                                  "WHERE wc.claim_id = {WorkClaimID}"

    'change document date and period_id
    Public Const ACUpdateDocDatePeriodStored As Boolean = True
    Public Const ACUpdateDocDatePeriodName As String = "Change document date and period"
    Public Const ACUpdateDocDatePeriodSQL As String = "spu_ChangeDocDatePeriod"
	
	'reset original claim id to force copy of reinsurance from policy
	Public Const ACUpdateResetOrigClaimIDStored As Boolean = False
	Public Const ACUpdateResetOrigClaimIDName As String = "Reset original claim id"
	Public Const ACUpdateResetOrigClaimIDSQL As String = "UPDATE Work_Claim SET original_claim_id = null WHERE claim_id = {ClaimID}"
	
	'get all claim with no reinsurance ie no claim_risk_ri_band
	Public Const ACGetNoRIClaimStored As Boolean = False
	Public Const ACGetNoRIClaimName As String = "Get claim with no reinsurance"
	Public Const ACGetNoRIClaimSQL As String = "SELECT c.claim_id, c.claim_number" & Strings.Chr(13) & Strings.Chr(10) &  _
	                                           "FROM Claim c LEFT JOIN Claim_Risk_RI_Band crrb ON c.claim_id = crrb.claim_id" & Strings.Chr(13) & Strings.Chr(10) &  _
	                                           "WHERE crrb.claim_id Is Null" & Strings.Chr(13) & Strings.Chr(10) &  _
	                                           "ORDER BY c.claim_id"
	
	'update work claim with original claim id
	Public Const ACUpdateOrigClaimIDStored As Boolean = False
	Public Const ACUpdateOrigClaimIDName As String = "Update Original Claim ID"
	Public Const ACUpdateOrigClaimIDSQL As String = "UPDATE Work_Claim SET Original_Claim_ID = {ClaimID} WHERE claim_id = {WorkClaimID}"
	
	'update this_revision and this_payment with total reserve and total payment
	Public Const ACUpdateThisReservePaymentStored As Boolean = False
	Public Const ACUpdateThisReservePaymentName As String = "Update This Reserve Payment"
	Public Const ACUpdateThisReservePaymentSQL As String = "UPDATE r" & Strings.Chr(13) & Strings.Chr(10) &  _
	                                                       "SET      r.this_revision = r2.reserve," & Strings.Chr(13) & Strings.Chr(10) &  _
	                                                       "r.this_payment = r2.payment" & Strings.Chr(13) & Strings.Chr(10) &  _
	                                                       "FROM     work_reserve r" & Strings.Chr(13) & Strings.Chr(10) &  _
	                                                       "JOIN    (SELECT r.reserve_id, Sum(r.initial_reserve) + Sum(r.revised_reserve) reserve, sum(paid_to_date) payment" & Strings.Chr(13) & Strings.Chr(10) &  _
	                                                       "FROM work_reserve r" & Strings.Chr(13) & Strings.Chr(10) &  _
	                                                       "GROUP BY r.reserve_id" & Strings.Chr(13) & Strings.Chr(10) &  _
	                                                       ") r2 ON r.reserve_id = r2.reserve_id" & Strings.Chr(13) & Strings.Chr(10) &  _
	                                                       "JOIN work_claim_peril cp ON cp.claim_peril_id = r.claim_peril_id" & Strings.Chr(13) & Strings.Chr(10) &  _
	                                                       "Where cp.claim_id = {WorkClaimID}" & Strings.Chr(13) & Strings.Chr(10) &  _
	                                                       "AND (r.initial_reserve <> 0 OR r.revised_reserve <> 0 or r.paid_to_date <> 0)"
	
	
	'update ri_band reserve/payment
	Public Const ACUpdateRIBandReservePaymentStored As Boolean = False
	Public Const ACUpdateRIBandReservePaymentName As String = "Update RI_Band reserve/payment"
	Public Const ACUpdateRIBandReservePaymentSQL As String = "UPDATE Work_Claim_Risk_RI_Band SET reserve = reserve - {ReserveAmount}, payment = payment - {PaymentAmount} WHERE claim_id = {WorkClaimID} AND ri_band = {RIBand}"
	
	'get ri_band for reserve_id
	Public Const ACGetBandIDForReserveStored As Boolean = False
	Public Const ACGetBandIDForReserveName As String = "Get BandID For Reserve"
	Public Const ACGetBandIDForReserveSQL As String = "SELECT cp.ri_band" & Strings.Chr(13) & Strings.Chr(10) &  _
	                                                  "FROM Claim_Peril cp JOIN Reserve r ON cp.claim_peril_id = r.claim_peril_id" & Strings.Chr(13) & Strings.Chr(10) &  _
	                                                  "WHERE r.reserve_id = {ReserveID}"
	
	'update claim_ri_arrangement_line
	Public Const ACUpdateClaimRIArrangementLineStored As Boolean = False
	Public Const ACUpdateClaimRIArrangementLineName As String = "Update Claim RI Arrangement Line"
    Public Const ACUpdateClaimRIArrangementLineSQL As String = "UPDATE cral" & Strings.Chr(13) & Strings.Chr(10) & _
                                                               "SET cral.reserve = cral.reserve - {ReserveAmount}," & Strings.Chr(13) & Strings.Chr(10) & _
                                                               "cral.payment = cral.payment - {PaymentAmount}" & Strings.Chr(13) & Strings.Chr(10) & _
                                                               "FROM Claim_Risk_RI_Arrangement crra" & Strings.Chr(13) & Strings.Chr(10) & _
                                                               "JOIN Claim_RI_Arrangement_Line cral ON crra.claim_id = cral.claim_id AND crra.risk_ri_arrangement_id = cral.risk_ri_arrangement_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                                               "WHERE crra.claim_id = {ClaimID}" & Strings.Chr(13) & Strings.Chr(10) & _
                                                               "AND crra.ri_band = {RIBand}" & Strings.Chr(13) & Strings.Chr(10) & _
                                                               "AND cral.fac_arrangement_summary_id IS NULL"

    Public Const ACGetDocumentDetailsStored As Boolean = False
    Public Const ACGetDocumentDetailsName As String = "Get document_id "
    Public Const ACGetDocumentDetailsSQL As String = "SELECT Document_id FROM Document where document_ref = {DocumentRef}"

    Public Const ACAddTransExportDetailsReverseStored As Boolean = True
    Public Const ACAddTransExportDetailsReverseName As String = "AddTransDetailsReverse"
    Public Const ACAddTransExportDetailsReverseSQL As String = "spu_Copy_transExport_for_Reversal_By_DocumentRef"

    Public Const ACGetRiskCntForInsuranceFileCntStored As Boolean = False
    Public Const ACGetRiskCntForInsuranceFileCntName As String = "Get Risk Cnts "
    Public Const ACGetRiskCntForInsuranceFileCntSQL As String = "SELECT risk_cnt FROM insurance_file_risk_link WHERE insurance_file_cnt= {InsuranceFileCnt}"

    Public Const ACGetTrasanctionExportDetailStored As Boolean = False
    Public Const ACGetTrasanctionExportDetailName As String = "Get Transactions "
    Public Const ACGetTrasanctionExportDetailSQL As String = "SELECT transaction_export_folder_cnt FROM transaction_export_folder WHERE document_ref= {DocumentRef}"


    Public Const ACGetTransactionTypeIDName As String = "Get Transaction Type ID"
    Public Const ACGetTransactionTypeIDSQL As String = "spu_CLM_Get_Transaction_Type_Details"

    Public Const ACFinaliseStatsName As String = "Perform the actions that used to take place in copy work details to live"
    Public Const ACFinaliseStatsSQl As String = "spu_CLM_Finalise_stats_Reversal"

    'get document_id from document via document_ref
    Public Const ACGetDocumentINStatsStored As Boolean = False
    Public Const ACGetDocumentINStatsName As String = "GetDocumentInStats"
    Public Const ACGetDocumentINStatsSQL As String = "SELECT Stats_Folder_cnt FROM Stats_Folder WHERE document_ref = {DocumentRef}"

    Public Const ACAddDocumentStored As Boolean = True
    Public Const ACAddDocumentName As String = "AddDocument"
    Public Const ACAddDocumentSQL As String = "spu_CopyReversalDocument"

    Public Const ACAddReverseTransdetailStored As Boolean = True
    Public Const ACAddReverseTransdetaiName As String = "AddTransdetailReverse"
    Public Const ACAddReverseTransdetaiSQL As String = "spu_CopyReversalTransdetail"


End Module