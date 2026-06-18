Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Module CloseClaimSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: CloseClaimSQL
	'
	' Date: 26/09/2001
	'
	' Description:
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Add StatsFolder SQL
	Public Const ACGetClaimStatusStored As Boolean = True
    Public Const ACGetClaimStatusName As String = "GetClaimStatus"
    'Developer Guide no.39
    Public Const ACGetClaimStatusSQL As String = "spu_get_claim_status"
	
	Public Const ACGetOriginalClaimIDStored As Boolean = False
	Public Const ACGetOriginalClaimIDName As String = "Get Original Claim ID"
	Public Const ACGetOriginalClaimIDSQL As String = "SELECT Original_Claim_id FROM work_claim" & Strings.Chr(13) & Strings.Chr(10) &  _
	                                                 "WHERE claim_id = {claim_id}"
	
	' Delete any Work Claim records
	Public Const ACDeleteWorkClaimStored As Boolean = True
    Public Const ACDeleteWorkClaimName As String = "Delete Work Claim"
    'Developer Guide no.39
    Public Const ACDeleteWorkClaimSQL As String = "spu_delete_work_claim"
	
	Public Const ACGetReserveRecoveryStored As Boolean = True
    Public Const ACGetReserveRecoveryName As String = "Get claim status current reserve and recovery"
    'Developer Guide no.39
    Public Const ACGetReserveRecoverySQL As String = "spu_GetCurrentReserveRecovery_SFU"
	Public Const ACGetReserveFromRIName As String = "Get claim RI arrangement balances from RI lines" 'PN 42887
    Public Const ACGetReserveFromRISQL As String = "spu_GetCurrentReserveFromRI_SFU" 'PN 42887

    Public Const ACUpdClaimStatusName As String = "Update claim status"
    Public Const ACUpdClaimStatusSQL As String = "spu_CLM_Update_Claim_status"
End Module