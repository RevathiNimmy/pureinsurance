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
	'              bDOCDocTrans.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Add Document SQL
	Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddDocument"
    'developer guide no.39
    Public Const ACAddSQL As String = "spu_DOC_add_No_I_document"
End Module