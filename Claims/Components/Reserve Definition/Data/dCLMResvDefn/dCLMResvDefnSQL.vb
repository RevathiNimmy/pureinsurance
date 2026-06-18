Option Strict Off
Option Explicit On
Imports System
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
	' Date: 06/10/1998
	'
	' Description: Contains the SQL Statements required by the
	'              SIRContact class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Add CLMResvDefn SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddCLMResvDefn"
	Public Const ACAddSQL As String = "spu_resv_defn_add"
	
	' Update CLMResvDefn SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdateCLMResvDefn"
	Public Const ACUpdateSQL As String = "spu_resv_defn_upd"
	
	' Delete CLMResvDefn SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeleteCLMResvDefn"
	Public Const ACDeleteSQL As String = "spu_resv_defn_del"
	
	
	
	'******************************************************************
	' Select CLMResvDefn SQL
	Public Const ACSelectSingleStored As Boolean = True
	Public Const ACSelectSingleName As String = "SelectSingleCLMRecovery"
	Public Const ACSelectSingleSQL As String = "spe_recovery_sel"
End Module