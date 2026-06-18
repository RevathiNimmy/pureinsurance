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
	' Date: 16/01/1998
	'
	' Description: Contains the SQL Statements required by the
	'              bAnnotation.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Add Annotation SQL
	Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddAnnotation"
    'developer guide no. 39
    Public Const ACAddSQL As String = "spu_DOC_add_annotation"
End Module