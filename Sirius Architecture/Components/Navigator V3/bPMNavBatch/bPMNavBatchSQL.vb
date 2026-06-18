Option Strict Off
Option Explicit On
Imports System
Module PMNavBatchSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: PMNavBatchSQL
	'
	' Date: 02/10/1997
	'
	' Description: Contains the SQL Statements to manipulate
	'              a PMNavBatch
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select batch nav keys
	Public Const ACGetNavKeysStored As Boolean = True
	Public Const ACGetNavKeysName As String = "GetNavBatchKeys"
    Public Const ACGetNavKeysSQL As String = "spu_pmnav_get_nav_batch_keys"
	
	' Add Navigator Batch Set SQL
	Public Const ACAddBatchSetStored As Boolean = True
	Public Const ACAddBatchSetName As String = "AddNavBatchSet"
    Public Const ACAddBatchSetSQL As String = "spu_pmnav_add_Nav_Batch_Set"
	
	' Add Navigator Batch Record SQL
	Public Const ACAddBatchRecordStored As Boolean = True
	Public Const ACAddBatchRecordName As String = "AddNavBatchRecord"
    Public Const ACAddBatchRecordSQL As String = "spu_pmnav_add_Nav_Batch_Record"
	
	' Select Next Navigator Batch Record SQL
	Public Const ACSelectBatchRecordStored As Boolean = True
	Public Const ACSelectBatchRecordName As String = "SelectNextNavBatchRecord"
    Public Const ACSelectBatchRecordSQL As String = "spu_pmnav_sel_Nav_Batch_Record"
	
	' Delete Navigator Batch Record SQL
	Public Const ACDeleteBatchRecordStored As Boolean = True
	Public Const ACDeleteBatchRecordName As String = "DeleteNavBatchRecord"
    Public Const ACDeleteBatchRecordSQL As String = "spu_pmnav_del_Nav_Batch_Record"
	
	' Delete Navigator Batch Set SQL
	Public Const ACDeleteBatchSetStored As Boolean = True
	Public Const ACDeleteBatchSetName As String = "DeleteNavBatchSet"
    Public Const ACDeleteBatchSetSQL As String = "spu_pmnav_del_Nav_Batch_Set"
	
	' Start Navigator Batch Set SQL
	Public Const ACStartBatchSetStored As Boolean = True
	Public Const ACStartBatchSetName As String = "StartNavBatchSet"
    Public Const ACStartBatchSetSQL As String = "spu_pmnav_start_Nav_Batch_Set"
	
	' Start Navigator Batch Set SQL
	Public Const ACGetBatchSetStored As Boolean = True
	Public Const ACGetBatchSetName As String = "RetrieveNavBatchSet"
    Public Const ACGetBatchSetSQL As String = "spu_pmnav_get_Nav_Batch_Set"
	
	' Start Navigator Batch Set SQL
	Public Const ACStopBatchSetStored As Boolean = True
	Public Const ACStopBatchSetName As String = "StopNavBatchSet"
    Public Const ACStopBatchSetSQL As String = "spu_pmnav_stop_Nav_Batch_Set"
End Module