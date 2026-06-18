Option Strict Off
Option Explicit On
Imports System
Module FindCashListSql
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	' ***************************************************************** '
	' Class Name: FindCashListSQL
	'
	' Date: 01 April 1997
	'
	' Description: Contains the SQL Statements to (Stored Procedures
	'              and Embedded SQL)
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectEvent"
	' Public Const ACSelectSQL = "SELECT * FROM Event WHERE event_id = {event_id}"
	
	'Select CashList from full query
	Public Const ACCashListFromQueryStored As Boolean = True
	Public Const ACCashListFromQueryName As String = "SelectCashListQuery"
    'developer guide no. 39
    Public Const ACCashListFromQuerySQL As String = "spu_ACT_Do_FindCashList"
	
	''get CashListID from parameters SQL
	'Public Const ACGetCashListIDStored = True
	'Public Const ACGetCashListIDName = "GetCashListID"
	'Public Const ACGetCashListIDSQL = "{call spu_ACT_Do_GetCashListId (?, ?, ?, ?, ?)}"
    
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module