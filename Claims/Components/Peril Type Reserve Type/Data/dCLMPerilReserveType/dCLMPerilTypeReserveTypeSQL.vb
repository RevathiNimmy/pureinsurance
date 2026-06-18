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
	' Date: 30/09/2000
	'
	' Description: Contains the SQL Statements required by the
	'              dCLMPerilTypeReserveType class.
	'
	' Edit History: DG
	' ***************************************************************** '
	
	'SQL Statements
	
	
	' Add CLMPerilTypeReserveType SQL
	Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddCLMPerilTypeReserveType"

    'Developer Guide No:39
    Public Const ACAddSQL As String = "spu_periltype_rsrvtype_add"


	'*************************************************************************
	' Update CLMPerilTypeReserveType SQL
	Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateCLMPerilTypeReserveType"

    'Developer Guide No:39
    Public Const ACUpdateSQL As String = "spu_periltype_rsrvtype_upd"

	
	' Delete CLMPerilTypeReserveType SQL
	Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteCLMPerilTypeReserveType"

    'Developer Guide No:39
    Public Const ACDeleteSQL As String = "spu_periltype_rsrvtype_del"
	
	
	'******************************************************************
	' Select CLMPerilTypeReserveType SQL
	Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSingleCLMPerilTypeReserveType"

    'Developer Guide No:39
    Public Const ACSelectSingleSQL As String = "spu_periltype_rsrvtype_sel"
End Module