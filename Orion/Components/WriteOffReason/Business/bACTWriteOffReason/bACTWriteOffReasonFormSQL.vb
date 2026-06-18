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
	' Date: 14/08/1998
	'
	' Description: Contains the SQL Statements required by the
	'              bACTWriteOffReason.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
    ' Select WriteOffReason SQL
    'Developer Guide No 39
    'Starts
	Public Const ACGetDetailsStored As Boolean = True
	Public Const ACGetDetailsName As String = "SelectAllWriteOffReason"
    Public Const ACGetDetailsSQL As String = "spu_ACT_select_Write_Off_Reason"
	
	' Select All WriteOffReason SQL
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "SelectAllWriteOffReason"
    Public Const ACGetAllDetailsSQL As String = "spu_ACT_select_all_Write_Off_Reason"
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
	Public Const ACCheckIDName As String = "CheckWriteOffReasonID"
    Public Const ACCheckIDSQL As String = "spu_ACT_check_Write_Off_Reason"
	
	' Add WriteOffReason SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddWriteOffReason"
    Public Const ACAddSQL As String = "spu_ACT_Add_Write_Off_Reason"
	
	' Delete WriteOffReason SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeleteWriteOffReason"
    Public Const ACDeleteSQL As String = "spu_ACT_delete_Write_Off_Reason"
	
	' Update WriteOffReason SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdateWriteOffReason"
    Public Const ACUpdateSQL As String = "spu_ACT_update_Write_Off_Reason"
    'Ends
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module