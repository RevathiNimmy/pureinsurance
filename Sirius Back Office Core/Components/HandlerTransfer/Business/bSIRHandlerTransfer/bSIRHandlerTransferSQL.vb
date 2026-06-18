Option Strict Off
Option Explicit On
Imports System
Module FormSQL
	' ***************************************************************** '
	' Class Name: FormSQL
	'
	' Date: 24/03/2005
	'
	' Description: Contains the SQL Statements required by the
	'              bSIRHandlerUpdate.Business class.
	'
	' Edit History:
	' CJB 270605 PN21983 In SelectPoliciesForHandlerSQL & SelectPoliciesForExecutiveSQL also
	'            return I.insurance_folder_cnt as rqd for policy level events
	'
	' ***************************************************************** '
	
	'SQL Statements
	
	
	' Select Clients
	Public Const SelectClientsStored As Boolean = False
	Public Const SelectClientsName As String = "SelectClients"
	Public Const SelectClientsSQL As String = "SELECT party_cnt FROM Party WHERE consultant_cnt = {oldexecutive_cnt}"
	
	' Select Policies for Account Handler
	Public Const SelectPoliciesForHandlerStored As Boolean = False
	Public Const SelectPoliciesForHandlerName As String = "SelectPolicies"
	Public Const SelectPoliciesForHandlerSQL As String = "SELECT I.insurance_file_cnt,I.insured_cnt,I.insurance_folder_cnt FROM Insurance_file I" &  _
	                                                     " WHERE I.account_handler_cnt = {oldhandler_cnt}" &  _
	                                                     " AND I.policy_version = (SELECT max(policy_version) from insurance_file" &  _
	                                                     " WHERE  insurance_ref = i.insurance_ref)" &  _
	                                                     " UNION" &  _
	                                                     " SELECT MAX(I.insurance_file_cnt), I.insured_cnt, I.insurance_folder_cnt from insurance_file I" &  _
	                                                     " INNER JOIN Insurance_File_Type IFT" &  _
	                                                     " ON IFT.insurance_file_type_id = i.insurance_file_type_id" &  _
	                                                     " WHERE I.account_handler_cnt = {oldhandler_cnt} AND ( IFT.CODE='POLICY' OR IFT.CODE='MTA PERM' OR IFT.CODE='MTA TEMP')" &  _
	                                                     " GROUP BY i.insurance_ref, I.insured_cnt, I.insurance_folder_cnt"
	' Select Policies for Account Executive
	Public Const SelectPoliciesForExecutiveStored As Boolean = False
	Public Const SelectPoliciesForExecutiveName As String = "SelectPolicies"
	Public Const SelectPoliciesForExecutiveSQL As String = "SELECT I.insurance_file_cnt,I.insured_cnt,I.insurance_folder_cnt FROM Insurance_file I" &  _
	                                                       " WHERE account_executive_cnt = {oldexecutive_cnt}" &  _
	                                                       " AND I.policy_version = (SELECT max(policy_version) from insurance_file" &  _
	                                                       " WHERE  insurance_ref = i.insurance_ref)"
	
	'Update Client
	Public Const UpdateClientStored As Boolean = False
	Public Const UpdateClientName As String = "UpdateClient"
	Public Const UpdateClientSQL As String = "UPDATE party set consultant_cnt = {newexecutive_cnt} WHERE party_cnt = {party_cnt} "
	
	'Update PolicyHandler
	Public Const UpdatePolicyHandlerStored As Boolean = False
	Public Const UpdatePolicyHandlerName As String = "UpdatePolicy"
	Public Const UpdatePolicyHandlerSQL As String = "UPDATE insurance_file set account_handler_cnt = {newhandler_cnt} WHERE insurance_file_cnt = {insurance_file_cnt} "
	
	'Update PolicyExecutive
	Public Const UpdatePolicyExecutiveStored As Boolean = False
	Public Const UpdatePolicyExecutiveName As String = "UpdatePolicy"
	Public Const UpdatePolicyExecutiveSQL As String = "UPDATE insurance_file set account_executive_cnt = {newexecutive_cnt} WHERE insurance_file_cnt = {insurance_file_cnt} "
	
	'Delete Handler Executive
	Public Const DeleteHandlerExecutiveStored As Boolean = False
	Public Const DeleteHandlerExecutiveName As String = "DeleteHandlerExecutive"
	Public Const DeleteHandlerExecutiveSQL As String = "UPDATE party set is_deleted = 1 WHERE party_cnt = {party_cnt} "

	Public Const InsertConfigurationAuditDetailsStored As Boolean = True
	Public Const InsertConfigurationAuditDetailsName As String = "Insert"
	Public Const InsertConfigurationAuditDetailsSQL As String = "spu_Account_Executive_Handler_Transfer"

End Module