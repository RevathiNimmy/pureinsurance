Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  27th September 1996
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '

    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bACTDocumentPost"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Log Level

    ' Get Ledger_ID From Document_ID
	Public Const ACGetLedgerFromDocumentStored As Boolean = True
    Public Const ACGetLedgerFromDocumentName As String = "GetLedgerFromDocument"
    'Developer guide no 39
    Public Const ACGetLedgerFromDocumentSQL As String = "spu_ACT_Get_LedgerFromDocument"
	
	' Called from PostDocument
	Public Const ACUpdateTransActDateStored As Boolean = True
    Public Const ACUpdateTransActDateName As String = "UpdateTransActDate"
    'Developer Guide no 39
    Public Const ACUpdateTransActDateSQL As String = "spu_ACT_Do_UpdateTransActDate"
End Module