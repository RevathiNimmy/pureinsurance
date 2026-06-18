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
	' Date: 08/09/1997
	'
	' Description: Contains the SQL Statements required by the 
	'              bACTBank.Automated class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACAutoSelectStored = False
	' Public Const ACAutoSelectName = "SelectRisk"
	' Public Const ACAutoSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select Bank SQL
	Public Const ACAutoGetDetailsStored As Boolean = True
    Public Const ACAutoGetDetailsName As String = "SelectAllBank"
    'Developer Guide no.39
    Public Const ACAutoGetDetailsSQL As String = "spu_ACT_Select_{NewTable}"
	
	' Select All Bank SQL
	Public Const ACAutoGetAllDetailsStored As Boolean = True
    Public Const ACAutoGetAllDetailsName As String = "SelectAllBank"
    'Developer Guide no.39
    Public Const ACAutoGetAllDetailsSQL As String = "spu_ACT_SelAll_{NewTable}"
	' Check ID SQL
	Public Const ACAutoCheckIDStored As Boolean = True
    Public Const ACAutoCheckIDName As String = "CheckBankID"
    'Developer Guide no.39
    Public Const ACAutoCheckIDSQL As String = "spu_ACT_Check_{NewTable}"
	' Add Bank SQL
	Public Const ACAutoAddStored As Boolean = True
    Public Const ACAutoAddName As String = "AddBank"
    'Developer Guide no.39
    Public Const ACAutoAddSQL As String = "spu_ACT_Add_{NewTable}"
	' Delete Bank SQL
	Public Const ACAutoDeleteStored As Boolean = True
    Public Const ACAutoDeleteName As String = "DeleteBank"
    'Developer Guide no.39
    Public Const ACAutoDeleteSQL As String = "spu_ACT_Delete_{NewTable}"
	' Update Bank SQL
	Public Const ACAutoUpdateStored As Boolean = True
    Public Const ACAutoUpdateName As String = "UpdateBank"
    'Developer Guide no.39
    Public Const ACAutoUpdateSQL As String = "spu_ACT_Update_{NewTable}"

End Module