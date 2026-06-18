Option Strict Off
Option Explicit On
Imports System

'Developer Guide No.: 129
Imports SharedFiles

Module SQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: SQL
	'
	' Date: 30/08/2000
	'
	' Description: Contains the SQL Statements required by the
	'              CLMRiskType class.
	'
	' Edit History:Pandu
	' ***************************************************************** '
	
	'SQL Statements
	
	
	
	' Add CLMRiskType SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddCLMRiskType"
    Public Const ACAddSQL As String = "spu_risktype_add"
	
	' Update CLMRisktype SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdateCLMRiskType"
    Public Const ACUpdateSQL As String = "spu_risktype_upd"
	
	' Delete CLMRiskType SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeleteCLMRiskType"
    Public Const ACDeleteSQL As String = "spu_risktype_del"
	
	
	
	'******************************************************************
	' Select CLMResvDefn SQL
	Public Const ACSelectSingleStored As Boolean = True
	Public Const ACSelectSingleName As String = "SelectSingleCLMRecovery"
    Public Const ACSelectSingleSQL As String = "spe_recovery_sel"
End Module