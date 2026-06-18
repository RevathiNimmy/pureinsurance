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
	'              bACTTransmatch.Form class.
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
	' Select Transmatch SQL
	Public Const ACGetDetailsStored As Boolean = True
	Public Const ACGetDetailsName As String = "SelectAllTransmatch"
    Public Const ACGetDetailsSQL As String = "spu_ACT_select_TransMatch"
	
	' Select All Transmatch SQL
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "SelectAllTransmatch"
    Public Const ACGetAllDetailsSQL As String = "spu_ACT_select_all_TransMatch"

	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
	Public Const ACCheckIDName As String = "CheckTransmatchID"
    Public Const ACCheckIDSQL As String = "spu_ACT_check_TransMatch"
	
	' Add Transmatch SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddTransmatch"
    Public Const ACAddSQL As String = "spu_ACT_add_TransMatch"
	
	' Delete Transmatch SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeleteTransmatch"
    Public Const ACDeleteSQL As String = "spu_ACT_delete_TransMatch"
	
	' Update Transmatch SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdateTransmatch"
    Public Const ACUpdateSQL As String = "spu_ACT_update_TransMatch"
	
	' Select outstanding amount on transaction
	Public Const ACSelectTransDetailStored As Boolean = True
	Public Const ACSelectTransDetailName As String = "SelectTransDetail"
    Public Const ACSelectTransDetailSQL As String = "spu_act_select_transdetail_OS"
	
	
	' Select the Primary Transaction of a set
	Public Const ACSelectTransPrimaryStored As Boolean = True
	Public Const ACSelectTransPrimaryName As String = "SelectTransPrimary"
    Public Const ACSelectTransPrimarySQL As String = "spu_ACT_select_transdetail_prm"
    'ends
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module