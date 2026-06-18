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
	' Date: 11/07/1997
	'
	' Description: Contains the SQL Statements required by the
	'              bGEMLookup.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
    ' Select TypeTable SQL
    'developer guide no. 39
    'start
	Public Const ACGetDetailsStored As Boolean = True
    Public Const ACGetDetailsName As String = "SelectTypeTable"
    'Developer Guide No 101
    'Public Const ACGetDetailsSQL As String = "{call sp_GEM_select_TypeTable (?)}"
    Public Const ACGetDetailsSQL As String = "sp_GEM_select_TypeTable"

	' Select All TypeTable SQL
	Public Const ACGetAllDetailsStored As Boolean = True
    'Developer Guide No 101
    Public Const ACGetAllDetailsName As String = "SelectAllTypeTable"
    'Public Const ACGetAllDetailsSQL As String = "{call sp_GEM_SelAll_TypeTable}"
    Public Const ACGetAllDetailsSQL As String = "sp_GEM_SelAll_TypeTable"
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
    'Developer Guide No 101
    Public Const ACCheckIDName As String = "CheckTypeTableID"
    'Public Const ACCheckIDSQL As String = "{call sp_GEM_check_TypeTable (?)}"
    Public Const ACCheckIDSQL As String = "sp_GEM_check_TypeTable"

	' Add TypeTable SQL
	Public Const ACAddStored As Boolean = True
    'Developer Guide No 101
    Public Const ACAddName As String = "AddTypeTable"
    'Public Const ACAddSQL As String = "{call sp_GEM_add_TypeTable (?,?)}"
    Public Const ACAddSQL As String = "sp_GEM_add_TypeTable"

	' Delete TypeTable SQL
	Public Const ACDeleteStored As Boolean = True
    'Developer Guide No 101
    Public Const ACDeleteName As String = "DeleteTypeTable"
    'Public Const ACDeleteSQL As String = "{call sp_GEM_delete_TypeTable (?)}"
    Public Const ACDeleteSQL As String = "sp_GEM_delete_TypeTable"

	' DeleteAll TypeTable SQL
	Public Const ACDeleteAllStored As Boolean = True
    'Developer Guide No 101
    Public Const ACDeleteAllName As String = "DeleteAllTypeTable"
    'Public Const ACDeleteAllSQL As String = "{call sp_GEM_delAll_TypeTable}"
    Public Const ACDeleteAllSQL As String = "sp_GEM_delAll_TypeTable"

	' Update TypeTable SQL
	Public Const ACUpdateStored As Boolean = True
    'Developer Guide No 101
    Public Const ACUpdateName As String = "UpdateTypeTable"
    'Public Const ACUpdateSQL As String = "{call sp_GEM_update_TypeTable (?,?)}"
    Public Const ACUpdateSQL As String = "sp_GEM_update_TypeTable"
    'MN160799 - Table Prefix COnstants
    'end
	Public Const HKJTablePrefix As String = "HKJ"
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module