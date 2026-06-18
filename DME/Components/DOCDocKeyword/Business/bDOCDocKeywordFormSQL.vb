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
	'              dKeyword.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Add Keyword SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddKeyword"
    Public Const ACAddSQL As String = "spu_DOC_Add_doc_keyword"
End Module