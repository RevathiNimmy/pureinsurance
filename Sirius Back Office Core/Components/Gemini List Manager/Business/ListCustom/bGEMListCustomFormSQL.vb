Option Strict Off
Option Explicit On
Imports System
Module FormSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	
	' ***************************************************************** '
	' Class Name: FormSQL
	'
	' Date: 10/02/1999
	'
	' Description: Contains the SQL Statements required by the
	'              bGEMListCustom.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	' Select ListCustom SQL
	Public Const ACGetDetailsStored As Boolean = True
	Public Const ACGetDetailsName As String = "SelectListCustom"
	Public Const ACGetDetailsSQL As String = "{call sp_GEM_Select_List_custom (?)}"
	
	' Select All ListCustom SQL
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "SelectAllListCustom"
	Public Const ACGetAllDetailsSQL As String = "{call sp_GEM_SelAll_List_custom}"
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
	Public Const ACCheckIDName As String = "CheckListCustomID"
	Public Const ACCheckIDSQL As String = "{call sp_GEM_Check_List_custom (?)}"
	
	' Add ListCustom SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddListCustom"
	Public Const ACAddSQL As String = "{call sp_GEM_Add_List_custom (?,?,?,?,?,?,?)}"
	
	' Delete ListCustom SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeleteListCustom"
	Public Const ACDeleteSQL As String = "{call sp_GEM_Delete_List_custom (?)}"
	
	' Update ListCustom SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdateListCustom"
	Public Const ACUpdateSQL As String = "{call sp_GEM_Update_List_custom (?,?,?,?,?,?,?)}"
    'Modified by Vijay Pal on 5/20/2010 11:57:11 AM refer developer guide no.(no solutions)
    'Shared Sub New()
    '    MainModule.JustForInvokeMain()
    'End Sub
End Module