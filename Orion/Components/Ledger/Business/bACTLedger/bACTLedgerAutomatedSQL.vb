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
	' Date: 23/07/1997
	'
	' Description: Contains the SQL Statements required by the
	'              bACTLedger.Automated class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACAutoSelectStored = False
	' Public Const ACAutoSelectName = "SelectRisk"
	' Public Const ACAutoSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
    'developer guide no.39
    'start
	' Select Ledger SQL
	Public Const ACAutoGetDetailsStored As Boolean = True
	Public Const ACAutoGetDetailsName As String = "SelectAllLedger"
    Public Const ACAutoGetDetailsSQL As String = "spu_ACT_Select_Ledger"
	
	' Select All Ledger SQL
	Public Const ACAutoGetAllDetailsStored As Boolean = True
	Public Const ACAutoGetAllDetailsName As String = "SelectAllLedger"
    Public Const ACAutoGetAllDetailsSQL As String = "spu_ACT_SelAll_Ledger"
	
	' Check ID SQL
	Public Const ACAutoCheckIDStored As Boolean = True
	Public Const ACAutoCheckIDName As String = "CheckLedgerID"
    Public Const ACAutoCheckIDSQL As String = "spu_ACT_Check_Ledger"
	
	' Add Ledger SQL
	Public Const ACAutoAddStored As Boolean = True
	Public Const ACAutoAddName As String = "AddLedger"
    Public Const ACAutoAddSQL As String = "spu_ACT_Add_Ledger"
	
	' Delete Ledger SQL
	Public Const ACAutoDeleteStored As Boolean = True
	Public Const ACAutoDeleteName As String = "DeleteLedger"
    Public Const ACAutoDeleteSQL As String = "spu_ACT_Delete_Ledger"
	
	' Update Ledger SQL
	Public Const ACAutoUpdateStored As Boolean = True
	Public Const ACAutoUpdateName As String = "UpdateLedger"
    Public Const ACAutoUpdateSQL As String = "spu_ACT_Update_Ledger"
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module