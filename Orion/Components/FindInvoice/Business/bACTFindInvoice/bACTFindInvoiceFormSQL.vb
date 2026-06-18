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
	' Date: 20/10/1998
	'
	' Description: Contains the SQL Statements required by the
	'              bACTFindInvoice.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    'developer guide no. 39
    'start
	' Select ACTFindInvoice SQL
	Public Const ACGetDetailsStored As Boolean = True
	Public Const ACGetDetailsName As String = "SelectAllACTFindInvoice"
    Public Const ACGetDetailsSQL As String = "spu_ACT_select_{NewTable}"
	
	' Select All ACTFindInvoice SQL
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "SelectAllACTFindInvoice"
    Public Const ACGetAllDetailsSQL As String = "spu_ACT_select_all_{NewTable}"
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
	Public Const ACCheckIDName As String = "CheckACTFindInvoiceID"
    Public Const ACCheckIDSQL As String = "spu_ACT_check_{NewTable}"
	
	' Add ACTFindInvoice SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddACTFindInvoice"
    Public Const ACAddSQL As String = "spu_ACT_add_{NewTable}"
	
	' Delete ACTFindInvoice SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeleteACTFindInvoice"
    Public Const ACDeleteSQL As String = "spu_ACT_delete_{NewTable}"
	
	' Update ACTFindInvoice SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdateACTFindInvoice"
    Public Const ACUpdateSQL As String = "spu_ACT_update_{NewTable}"

End Module