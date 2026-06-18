Option Strict Off
Option Explicit On
Imports System
Module SQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: SQL
	'
	' Date: 07/05/1999
	'
	' Description: Contains the SQL Statements required by the
	'              SIRTextFile class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select SIRTextFile SQL
	Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSingleSIRTextFile"
    'Developer Guide No. 39
    Public Const ACSelectSingleSQL As String = "spe_text_file_sel"
	
	' Add SIRTextFile SQL - This one ain't ERwin generated
	Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddSIRTextFile"
    'Developer Guide No. 39
    Public Const ACAddSQL As String = "spu_text_file_add"
	
	' Delete SIRTextFile SQL
	Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteSIRTextFile"
    'Developer Guide No. 39
    Public Const ACDeleteSQL As String = "spe_text_file_del"
	
	' Update SIRTextFile SQL
	Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateSIRTextFile"
    'Developer Guide No. 39
    Public Const ACUpdateSQL As String = "spe_text_file_upd"
End Module