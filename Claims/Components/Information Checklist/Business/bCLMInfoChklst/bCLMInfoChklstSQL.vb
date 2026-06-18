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
	' Date: 06/10/1998
	'
	' Description: Contains the SQL Statements required by the
	'              bCLMInfoChklst.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' AM 11/12/00 Get client name from claim for work manager tasks
	Public Const ACGetClientNameStored As Boolean = True
	Public Const ACGetClientName As String = "GetClientName"
	Public Const ACGetClientNameSQL As String = "spu_get_cli_name_claim_chklist"
	
	' Select All GetExpServsAdd SQL
	Public Const ACGetExpServsAddStored As Boolean = True
	Public Const ACGetExpServsAddName As String = "GetExpServsAdd"
	Public Const ACGetExpServsAddSQL As String = "spu_get_exp_servs_add"
	
	' Select All GetExpServsEdit SQL
	Public Const ACGetExpServsEditStored As Boolean = True
	Public Const ACGetExpServsEditName As String = "GetExpServsEdit"
	Public Const ACGetExpServsEditSQL As String = "spu_get_exp_servs_Edit"
	
	'---------------------------------------------------------
	
	' Select CLMCoinsuranceRecoveries SQL
	Public Const ACGetCoInsurerDetailsStored As Boolean = True
	Public Const ACGetCoInsurerDetailsName As String = "SelectCLMCoinsuranceRecoveries"
	Public Const ACGetCoInsurerDetailsSQL As String = "spu_get_sal_coins_details"
	
	' Select CLMReinsuranceRecoveries SQL
	Public Const ACGetReInsurerDetailsStored As Boolean = True
	Public Const ACGetReInsurerDetailsName As String = "SelectCLMReinsuranceRecoveries"
	Public Const ACGetReInsurerDetailsSQL As String = "spu_get_sal_reins_details"
	
	' Select CLMPeril SQL
	Public Const ACGetPerilDetailsStored As Boolean = True
	Public Const ACGetPerilDetailsName As String = "SelectCLMPeril"
	Public Const ACGetPerilDetailsSQL As String = "spu_get_Peril_details"
	
	' Select CLMDefaultCurrencyID SQL
	Public Const ACGetDefaultCurrencyIDStored As Boolean = True
	Public Const ACGetDefaultCurrencyIDName As String = "SelectCLMDefaultCurrencyID"
	Public Const ACGetDefaultCurrencyIDSQL As String = "spu_get_CurrencyID"
	
	' RWH 06/03/2001 Get client name from claim for work manager tasks
	Public Const ACGetClientAndPolicyIDStored As Boolean = True
	Public Const ACGetClientAndPolicyID As String = "GetClaimCliPolID"
	Public Const ACGetClientAndPolicyIDSQL As String = "spu_get_claim_clipol_id"
	
	' Delete any  Claim records
	Public Const ACDeleteClaimStored As Boolean = True
	Public Const ACDeleteClaimName As String = "Delete  Claim"
	Public Const ACDeleteClaimSQL As String = "spu_delete_claim"
	
	' JMK 25/05/2001 Find out whether Claim was previously Info Only status
	Public Const ACGetInfoOnlyStatusStored As Boolean = True
	Public Const ACGetInfoOnlyStatusName As String = "GetInfoOnlyStatus"
	Public Const ACGetInfoOnlyStatusSQL As String = "spu_get_claim_info_only_status"
	
	'get info only flag from _claim table
	Public Const ACGetInfoOnlyFlagStored As Boolean = True
	Public Const ACGetInfoOnlyFlagName As String = "get info only flag for the specified claims"
	Public Const ACGetInfoOnlyFlagSQL As String = "spu_CLM_Get_This_Claim_Info_Only_Status"
	
	'RWH(15/06/01)
	Public Const ACGetOriginalClaimIDStored As Boolean = True
	Public Const ACGetOriginalClaimIDName As String = "Get Original Claim ID"
	Public Const ACGetOriginalClaimIDSQL As String = "spu_CLM_Get_Base_Claim"
	
	Public Const ACAutoShowInfoChkLstStored As Boolean = True
	Public Const ACAutoShowInfoChkLstName As String = "Show_InfoCheckList"
	Public Const ACAutoShowInfoChkLstSQL As String = "spu_CLM_Show_InfoCheckList"
	
	Public Const ACGetClientPolicyDetailsStored As Boolean = True
	Public Const ACGetClientPolicyDetailsName As String = "GetClientPolicyDetails"
	Public Const ACGetClientPolicyDetailsSQL As String = "spu_get_client_policy_details"
End Module