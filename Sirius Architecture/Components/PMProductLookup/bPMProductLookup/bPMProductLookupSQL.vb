Option Strict Off
Option Explicit On
Imports System
Module PMProductLookupSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: PMProductLookupSQL
	'
	' Date: 08 October 1999
	'
	' Description: Contains the SQL Statements to (Stored Procedures
	'              and Enbedded SQL) manipulate a PMProductLookup
	'
	' Edit History:
	' DAK211299 - Sometimes need to get all tables
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectPMUserGroup"
	' Public Const ACSelectSQL = "SELECT * FROM PMUser_Group WHERE PMUser_id = {PMUser_id}"
    'developer guide no.39
    'start
	' Select PMProductLookup SQL
	Public Const ACGetDetailsStored As Boolean = True
	Public Const ACGetDetailsName As String = "SelectPMProductLookup"
    Public Const ACGetDetailsSQL As String = "spu_pmproduct_lookup_sel"
	
	' Select All PMProductLookup SQL
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "SelectAllPMProductLookup"
    Public Const ACGetAllDetailsSQL As String = "spu_pmproduct_lookup_sel_all"
	
	' Add PMProductLookup SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddPMProductLookup"
    Public Const ACAddSQL As String = "spu_pmproduct_lookup_add"
	
	' Delete PMProductLookup SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeletePMProductLookup"
    Public Const ACDeleteSQL As String = "spu_pmproduct_lookup_del"
	
	' Update PMProductLookup SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdatePMProductLookup"
    Public Const ACUpdateSQL As String = "spu_pmproduct_lookup_upd"
	
	'DAK211299
	' Get every PMProduct_Lookup
	Public Const ACGetEveryStored As Boolean = True
	Public Const ACGetEveryName As String = "EveryPMProductLookup"
    Public Const ACGetEverySQL As String = "spu_pmproduct_lookup_all"
    'end
End Module