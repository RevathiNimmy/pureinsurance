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
	' Class Name:   BusinessSQL
	' Description:  Contains the SQL Statements required by the
	'               bCLMSalvageRecovery.Business class.
	' Author:       SK
	' Date:         06/07/2000
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select All CLMRTInfoChklst SQL
	Public Const ACGetRiskTypeStored As Boolean = True
    Public Const ACGetRiskTypeName As String = "SelectAllRiskType"
    'Developer Guide no. 39
    'start
    Public Const ACGetRiskTypeSQL As String = "spu_Rsk_Type_sel"
	
	' Select All CLMRTInfoChklst SQL
	Public Const ACGetRiskCodeStored As Boolean = True
	Public Const ACGetRiskCodeName As String = "SelectAllRiskCode"
    Public Const ACGetRiskCodeSQL As String = "spu_Rsk_code_sel"
	
	' Select All CLMRTInfoChklst SQL
	Public Const ACGetRiskTypeExpSerStored As Boolean = True
	Public Const ACGetRiskTypeExpSerName As String = "SelectRiskTypeExpSer"
    Public Const ACGetRiskTypeExpSerSQL As String = "spu_Get_RskType_ExpSer"
	
	' Select All CLMRTInfoChklst SQL
	Public Const ACGetExpSerStored As Boolean = True
	Public Const ACGetExpSerName As String = "SelectExpSer"
    Public Const ACGetExpSerSQL As String = "spu_Get_ExpSer"
	
	' Update Show Information Check List SQL
    Public Const ACUpdRskTypeInfoChkLstSQL As String = "Spu_Info_Checklist_upd"
    'end
End Module