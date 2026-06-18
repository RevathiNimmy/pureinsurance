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
	' Date: 23/07/1997
	'
	' Description: Contains the SQL Statements required by the
	'              bACTLedger.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
    ' Select Ledger SQL
    'Developer Guide No 39
    'Starts
	Public Const ACGetDetailsStored As Boolean = True
	Public Const ACGetDetailsName As String = "SelectLedger"
    Public Const ACGetDetailsSQL As String = "spu_ACT_Select_Ledger"
	
	' Select All Ledger SQL
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "SelectAllLedger"
    Public Const ACGetAllDetailsSQL As String = "spu_ACT_SelAll_Ledger"
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
	Public Const ACCheckIDName As String = "CheckLedgerID"
    Public Const ACCheckIDSQL As String = "spu_ACT_Check_Ledger"
	
	' Add Ledger SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddLedger"
    Public Const ACAddSQL As String = "spu_ACT_Add_Ledger"
	
	' Delete Ledger SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeleteLedger"
    Public Const ACDeleteSQL As String = "spu_ACT_Delete_Ledger"
	
	' Update Ledger SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdateLedger"
    Public Const ACUpdateSQL As String = "spu_ACT_Update_Ledger"
	
	' Select LedgerNode Id SQL
	Public Const ACSelectLedgerNodeStored As Boolean = True
	Public Const ACSelectLedgerNodeName As String = "UpdateLedger"
    Public Const ACSelectLedgerNodeSQL As String = "spu_ACT_Select_LedgerNode"
	
	'eck310800
	' Select  NodeFromLedger Id SQL
	Public Const ACSelectNodeFromLedgerStored As Boolean = True
	Public Const ACSelectNodeFromLedgerName As String = "UpdateLedger"
    Public Const ACSelectNodeFromLedgerSQL As String = "spu_ACT_Select_NodeFromLedger"
	
	' Select all Mapping IDs of given Company and Type
	Public Const ACGetAllMappingStored As Boolean = True
	Public Const ACGetAllMappingName As String = "SelectAllMapping"
    Public Const ACGetAllMappingSQL As String = "spu_ACT_SelAll_Mapping"
	
	'EK 11/11/99
	' Select Ledgers for Closure
	Public Const ACGetClosuresStored As Boolean = True
	Public Const ACGetClosuresName As String = "SelectClosures"
    Public Const ACGetClosuresSQL As String = "spu_ACT_Select_Closures"
    'Ends
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module