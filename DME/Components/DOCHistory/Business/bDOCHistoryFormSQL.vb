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
	'              bDOCHistory.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Add History SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddHistory"
    Public Const ACAddSQL As String = "spu_DOC_add_history"
End Module