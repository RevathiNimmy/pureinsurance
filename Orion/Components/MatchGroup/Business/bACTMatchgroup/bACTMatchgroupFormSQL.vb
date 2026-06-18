Option Strict Off
Option Explicit On
Imports System
Module FormSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: FormSQL
	'
	' Date: 04/10/1997
	'
	' Description: Contains the SQL Statements required by the
	'              bACTMatchgroup.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
    'Developer Guide No 39
    'Starts
	' Select Matchgroup SQL
	Public Const ACGetDetailsStored As Boolean = True
	Public Const ACGetDetailsName As String = "SelectAllMatchgroup"
    Public Const ACGetDetailsSQL As String = "spu_ACT_select_MatchGroup"
	
	' Select All Matchgroup SQL
	Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllMatchgroup"
    Public Const ACGetAllDetailsSQL As String = "spu_ACT_selall_MatchGroup"
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
	Public Const ACCheckIDName As String = "CheckMatchID"
    Public Const ACCheckIDSQL As String = "spu_ACT_check_MatchGroup"
	
	' Add Matchgroup SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddMatchgroup"
    Public Const ACAddSQL As String = "spu_ACT_add_MatchGroup"
	
	' Delete Matchgroup SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeleteMatchgroup"
    Public Const ACDeleteSQL As String = "spu_ACT_delete_MatchGroup"
	
	' Update Matchgroup SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdateMatchgroup"
    Public Const ACUpdateSQL As String = "spu_ACT_update_MatchGroup"
    'Ends
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module