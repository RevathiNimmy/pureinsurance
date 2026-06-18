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
	' Date: 21/10/1998
	'
	' Description: Contains the SQL Statements required by the
	'              SIRAddress class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
    'developer guide no.39
    'start
	' Select SIRAddress SQL
	Public Const ACSelectSingleStored As Boolean = True
	Public Const ACSelectSingleName As String = "SelectSingleCLMAddress"
    Public Const ACSelectSingleSQL As String = "spu_Claim_Address_sel"
	
	' Add SIRAddress SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddCLMAddress"
	'DJM 07/05/2002 : Added an extra parameter
	'AR20050404 - PN15664 Add update_address parameter
    Public Const ACAddSQL As String = "spu_Claim_Address_add"
	
	' Delete SIRAddress SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeleteCLMAddress"
    Public Const ACDeleteSQL As String = "spu_Claim_Address_del"
	
	' Update SIRAddress SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdateCLMAddress"
	'DJM 07/05/2002 : Added an extra parameter
	'AR20050404 - PN15664 Add update_address parameter
    Public Const ACUpdateSQL As String = "spu_Claim_Address_upd"
	
	' Check Before Add SIRAddress SQL
	Public Const ACCheckStored As Boolean = True
	Public Const ACCheckName As String = "CheckCLMAddress"
	'DJM 07/05/2002 : Added an extra parameter
	'AR20050404 - PN15664 Add update_address parameter (not used)
    Public Const ACCheckSQL As String = "spu_Claim_Address_Check"
End Module