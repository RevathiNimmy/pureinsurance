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
	' Date: 10/02/1999
	'
	' Description: Contains the SQL Statements required by the
	'              bGEMLists.Form class.
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
	' Select Lists SQL
	Public Const ACGetDetailsStored As Boolean = True
	Public Const ACGetDetailsName As String = "SelectLists"
    Public Const ACGetDetailsSQL As String = "sp_GEM_Select_Lists"
	
	' Select All Lists SQL
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "SelectAllLists"
    Public Const ACGetAllDetailsSQL As String = "sp_GEM_SelAll_Lists"
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
	Public Const ACCheckIDName As String = "CheckListId"
    Public Const ACCheckIDSQL As String = "sp_GEM_Check_Lists"
	
	' Add Lists SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddLists"
    Public Const ACAddSQL As String = "sp_GEM_Add_Lists"
	
	' Delete Lists SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeleteLists"
    Public Const ACDeleteSQL As String = "sp_GEM_Delete_Lists"
	
	' Update Lists SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdateLists"
    Public Const ACUpdateSQL As String = "sp_GEM_Update_Lists"

    'end
    'Shared Sub New()
    '	MainModule.JustForInvokeMain()
    'End Sub
End Module