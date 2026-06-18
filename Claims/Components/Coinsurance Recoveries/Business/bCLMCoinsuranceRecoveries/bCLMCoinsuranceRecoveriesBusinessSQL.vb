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
	' Date: 08-June-2000
	'
	' Description: Contains the SQL Statements required by the
	'              bCLMCoInsuranceRecoveries.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Delete any  Claim records
	Public Const ACDeleteClaimStored As Boolean = True
	Public Const ACDeleteClaimName As String = "Delete Claim"
	Public Const ACDeleteClaimSQL As String = "spu_delete_claim"
	
	Public Const ACGetSystemOptionStored As Boolean = False
	Public Const ACGetSystemOptionName As String = "Get System Option"
	Public Const ACGetSystemOptionSQL As String = "SELECT value FROM system_options WHERE option_number = {option_number}"
	
	' JMK 25/05/2001 Find out whether Claim was previously Info Only status
	Public Const ACGetInfoOnlyStatusStored As Boolean = True
	Public Const ACGetInfoOnlyStatusName As String = "GetInfoOnlyStatus"
	Public Const ACGetInfoOnlyStatusSQL As String = "spu_get_claim_info_only_status"
	
	'RWH(15/06/01)
	Public Const ACGetOriginalClaimIDName As String = "Get Original Claim ID"
	Public Const ACGetOriginalClaimIDSQL As String = "spu_CLM_Get_Base_Claim"
End Module