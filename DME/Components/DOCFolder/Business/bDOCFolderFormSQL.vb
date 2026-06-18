Option Strict Off
Option Explicit On
Imports System
Module FormSQL
	' ***************************************************************** '
	' Class Name: FormSQL
	'
	' Date: 16/01/1998
	'
	' Description: Contains the SQL Statements required by the
	'              dFolder.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Add Folder SQL
	Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddFolder"
    'developer guide no. 39
    Public Const ACAddSQL As String = "spu_DOC_add_folder"
End Module