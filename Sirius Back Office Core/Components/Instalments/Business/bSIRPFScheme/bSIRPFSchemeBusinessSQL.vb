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
	' ***************************************************************** '
	' Class Name: BusinessSQL
	'
	' Date: 23/10/2000
	'
	' Description: Contains the SQL Statements required by the
	'              bSIRPFScheme.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select All PFScheme SQL
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "SelectAllPFScheme"
	Public Const ACGetAllDetailsSQL As String = "spu_PFScheme_saa"
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
	Public Const ACCheckIDName As String = "CheckPFSchemeID"
	Public Const ACCheckIDSQL As String = "spe_PFScheme_check_id"
	
	' Select PFScheme SQL
	Public Const ACSelectSingleStored As Boolean = True
	Public Const ACSelectSingleName As String = "SelectSinglePFScheme"
	Public Const ACSelectSingleSQL As String = "spu_PFScheme_sel"
	
	' Add PFScheme SQL
	'PN12594 Extra parameter
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddPFScheme"
	Public Const ACAddSQL As String = "spu_PFScheme_add"
	
	' Delete PFScheme SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeletePFScheme"
	Public Const ACDeleteSQL As String = "spe_PFScheme_del"
	
	' Update PFScheme SQL
	'PN12594 Extra Parameter
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdatePFScheme"
	Public Const ACUpdateSQL As String = "spu_PFScheme_upd"
	
	'PN-61310 Check Existance of PFScheme rates
	Public Const ACIsPFRateExistStored As Boolean = True
	Public Const ACIsPFRateExistName As String = "ISPFSchemeRateExists"
	Public Const ACIsPFRateExistSQL As String = "spu_PFScheme_IsRateExist"
End Module