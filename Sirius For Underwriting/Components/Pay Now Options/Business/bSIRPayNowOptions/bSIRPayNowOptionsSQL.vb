Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' Class Name: FormSQL
	'
	' Date: 01 Aug 2006
	'
	' Description: Contains the SQL Statements required by the
	'              bSirPayNowOptions.Form class.
	'
	' ***************************************************************** '
	
	'SQL Statements
	
	Public Const ACSelectAgentCodeForInsuranceFileCntStored As Boolean = False
	Public Const ACSelectAgentCodeForInsuranceFileCntName As String = "SelectAgentCode"
	Public Const ACSelectAgentCodeForInsuranceFileCntSQL As String = "Select pat.Code , " &  _
	                                                                 "is_float_balance_account,is_overdraft_account, " &  _
	                                                                 "float_balance_limit , overDraft_limit ,pa.Overdraft_expiry FROM " &  _
	                                                                 "Insurance_file ifl LEFT JOIN party_agent pa ON " &  _
	                                                                 "ifl.lead_agent_cnt=pa.party_cnt LEFT JOIN " &  _
	                                                                 "Party_agent_type pat ON pa.party_agent_type_id=pat.party_agent_type_id " &  _
	                                                                 "Where insurance_file_cnt = {InsuranceFileCnt}"
	Public Const ACSelectUnallocatedCreditForStored As Boolean = True
	Public Const ACSelectUnallocatedCreditForName As String = "GetUnallocatedCredit"
	Public Const ACSelectUnallocatedCreditForSQL As String = "spu_Get_UnAllocated_Credit"
	
	
	
	
	Public Const ACSelectInsuranceFileRefStored As Boolean = True
	Public Const ACSelectInsuranceFileRefName As String = "spe_Insurance_File_sel"
	Public Const ACSelectInsuranceFileRefSQL As String = "spe_Insurance_File_sel"
	
	Public Const ACSelectAccountIDStored As Boolean = False
	Public Const ACSelectAccountIDName As String = "GetAccountID"
	Public Const ACSelectAccountIDSQL As String = "Select Account_id from Account " &  _
	                                              " JOIN Party ON Account.Account_key = party.party_cnt" &  _
	                                              " WHERE party.party_cnt={Party_cnt}"
	
	
	Public Const ACSelectAccountIDFromInsuranceFileStored As Boolean = True
	Public Const ACSelectAccountIDFromInsuranceFileName As String = "GetAccountID"
	Public Const ACSelectAccountIDFromInsuranceFileSQL As String = "spu_GetAccountIDfromInsuranceFileCnt"
	
	
	Public Const ACSelectAgentInformationFromAgentIDStored As Boolean = True
	Public Const ACSelectAgentInformationFromAgentIDName As String = "SelectAgentCode"
	Public Const ACSelectAgentInformationFromAgentIDSQL As String = "spu_Get_AgentInformation"
	
	Public Const ACGetUserWriteOffLimitStored As Boolean = False
	Public Const ACGetUserWriteOffLimitName As String = "User Write Off Limit"
	Public Const ACGetUserWriteOffLimitSQL As String = "SELECT has_paynow_write_off_authority,paynow_write_off_amount FROM User_Authorities WHERE user_id = {User_ID}"
	
	' Start - Sankar - PN 56728
	Public Const ACGetPaymentDetailsOfLivePolicyStored As Boolean = True
	Public Const ACGetPaymentDetailsOfLivePolicyName As String = "GetPaymentDetailsOfLivePolicy"
	Public Const ACGetPaymentDetailsOfLivePolicySQL As String = "spu_GetPaymentDetailsOfLivePolicy"
	' End - Sankar - PN 56728
End Module