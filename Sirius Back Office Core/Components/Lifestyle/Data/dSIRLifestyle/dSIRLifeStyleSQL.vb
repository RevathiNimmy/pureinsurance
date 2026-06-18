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
	' Date: 05/05/1999
	'
	' Description: Contains the SQL Statements required by the
	'              PMBLifeStyle class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select PMBLifeStyle SQL
	Public Const ACSelectSingleStored As Boolean = True
	Public Const ACSelectSingleName As String = "SelectSinglePMBLifeStyle"
    'developer guide no. 39
    Public Const ACSelectSingleSQL As String = "spe_party_lifestyle_sel"
	
	' Add PMBLifeStyle SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddPMBLifeStyle"
    'developer guide no. 39
    Public Const ACAddSQL As String = "spe_party_lifestyle_add"
	
	' Delete PMBLifeStyle SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeletePMBLifeStyle"
    'developer guide no. 39
    Public Const ACDeleteSQL As String = "spe_party_lifestyle_del"
	
	' Update PMBLifeStyle SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdatePMBLifeStyle"
    'developer guide no. 39
    Public Const ACUpdateSQL As String = "spe_party_lifestyle_upd"
End Module