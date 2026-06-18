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
	'              bGEMListUser.Form class.
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
	' Select ListUser SQL
	Public Const ACGetDetailsStored As Boolean = True
	Public Const ACGetDetailsName As String = "SelectListUser"
    Public Const ACGetDetailsSQL As String = "sp_GEM_Select_List_user"
	
	' Select All ListUser SQL
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "SelectAllListUser"
    Public Const ACGetAllDetailsSQL As String = "sp_GEM_SelAll_List_user"
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
	Public Const ACCheckIDName As String = "CheckListUserID"
    Public Const ACCheckIDSQL As String = "sp_GEM_Check_List_user"
	
	' Add ListUser SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddListUser"
    Public Const ACAddSQL As String = "sp_GEM_Add_List_user"
	
	' Delete ListUser SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeleteListUser"
    Public Const ACDeleteSQL As String = "sp_GEM_Delete_List_user"
	
	' Delete ListUser SQL
	Public Const ACDeleteWholeStored As Boolean = True
	Public Const ACDeleteWholeName As String = "DeleteListUser"
    Public Const ACDeleteWholeSQL As String = "sp_GEM_DelByList_List_user"
	
	' Update ListUser SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdateListUser"
    Public Const ACUpdateSQL As String = "sp_GEM_Update_List_user"
    'end
    'Shared Sub New()
    '	MainModule.JustForInvokeMain()
    'End Sub
End Module