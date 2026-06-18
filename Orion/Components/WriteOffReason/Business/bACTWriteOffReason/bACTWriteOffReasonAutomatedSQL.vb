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
	' Date: 14/08/1998
	'
	' Description: Contains the SQL Statements required by the
	'              bACTWriteOffReason.Automated class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACAutoSelectStored = False
	' Public Const ACAutoSelectName = "SelectRisk"
	' Public Const ACAutoSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select WriteOffReason SQL
	Public Const ACAutoGetDetailsStored As Boolean = True
	Public Const ACAutoGetDetailsName As String = "SelectAllWriteOffReason"
	Public Const ACAutoGetDetailsSQL As String = "{call spu_ACT_select_Write_Off_Reason (?)}"
	
	' Select All WriteOffReason SQL
	Public Const ACAutoGetAllDetailsStored As Boolean = True
	Public Const ACAutoGetAllDetailsName As String = "SelectAllWriteOffReason"
	Public Const ACAutoGetAllDetailsSQL As String = "{call spu_ACT_select_all_Write_Off_Reason}"
	
	' Check ID SQL
	Public Const ACAutoCheckIDStored As Boolean = True
	Public Const ACAutoCheckIDName As String = "CheckWriteOffReasonID"
	Public Const ACAutoCheckIDSQL As String = "{call spu_ACT_check_Write_Off_Reason (?)}"
	
	' Add WriteOffReason SQL
	Public Const ACAutoAddStored As Boolean = True
	Public Const ACAutoAddName As String = "AddWriteOffReason"
	Public Const ACAutoAddSQL As String = "{call spu_ACT_add_Write_Off_Reason (?,?,?,?,?,?)}"
	
	' Delete WriteOffReason SQL
	Public Const ACAutoDeleteStored As Boolean = True
	Public Const ACAutoDeleteName As String = "DeleteWriteOffReason"
	Public Const ACAutoDeleteSQL As String = "{call spu_ACT_delete_Write_Off_Reason (?)}"
	
	' Update WriteOffReason SQL
	Public Const ACAutoUpdateStored As Boolean = True
	Public Const ACAutoUpdateName As String = "UpdateWriteOffReason"
	Public Const ACAutoUpdateSQL As String = "{call spu_ACT_update_Write_Off_Reason (?,?,?,?,?,?)}"
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module