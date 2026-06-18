Option Strict Off
Option Explicit On
Imports System
Module ListScreenSql
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	' ***************************************************************** '
	' Class Name: ListScreenSQL
	'
	' Date: 14th July 2000
	'
	' Description: Contains the SQL Statements to (Stored Procedures
	'              and Embedded SQL) manipulate an ListScreen
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectEvent"
	' Public Const ACSelectSQL = "SELECT * FROM Event WHERE event_id = {event_id}"
	
	' Select ListScreen by shortname SQL
	Public Const ACListScreensStored As Boolean = True
    Public Const ACListScreensName As String = "ListScreens"
    'developer guide no. 39
    Public Const ACListScreensSQL As String = "spu_GIS_Screen_saa"
    Public Const ACListScreenUpdate As String = "spe_GIS_Screen_upd"
End Module