Option Strict Off
Option Explicit On
Imports System
Module AutomatedSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: AutomatedSQL
	'
	' Date: 04/10/1997
	'
	' Description: Contains the SQL Statements required by the
	'              bACTTransmatch.Automated class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACAutoSelectStored = False
	' Public Const ACAutoSelectName = "SelectRisk"
	' Public Const ACAutoSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    'Developer Guide No39
    'Starts
	' Select Transmatch SQL
	Public Const ACAutoGetDetailsStored As Boolean = True
	Public Const ACAutoGetDetailsName As String = "SelectAllTransmatch"
    Public Const ACAutoGetDetailsSQL As String = "spu_ACT_select_TransMatch"
	
	' Select All Transmatch SQL
	Public Const ACAutoGetAllDetailsStored As Boolean = True
	Public Const ACAutoGetAllDetailsName As String = "SelectAllTransmatch"
    Public Const ACAutoGetAllDetailsSQL As String = "spu_ACT_select_all_TransMatch"
	
	' Check ID SQL
	Public Const ACAutoCheckIDStored As Boolean = True
	Public Const ACAutoCheckIDName As String = "CheckTransmatchID"
    Public Const ACAutoCheckIDSQL As String = "spu_ACT_check_TransMatch"
	
	' Add Transmatch SQL
	Public Const ACAutoAddStored As Boolean = True
	Public Const ACAutoAddName As String = "AddTransmatch"
    Public Const ACAutoAddSQL As String = "spu_ACT_add_TransMatch"
	
	' Delete Transmatch SQL
	Public Const ACAutoDeleteStored As Boolean = True
	Public Const ACAutoDeleteName As String = "DeleteTransmatch"
    Public Const ACAutoDeleteSQL As String = "spu_ACT_delete_TransMatch"
	
	' Update Transmatch SQL
	Public Const ACAutoUpdateStored As Boolean = True
	Public Const ACAutoUpdateName As String = "UpdateTransmatch"
    Public Const ACAutoUpdateSQL As String = "spu_ACT_update_TransMatch"
    'Ends
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module