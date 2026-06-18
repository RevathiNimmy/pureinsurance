Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' Class Name: BusinessSQL
	'
	' Date: DC060704
	'
	' Description: Contains the SQL Statements
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' create memo task for supervisor to check o/s task of other user
	Public Const ACCreateCheckTaskStored As Boolean = True
    Public Const ACCreateCheckTaskName As String = "CreateCheckTask"
    'developer guide no. 39
    Public Const ACCreateCheckTaskSQL As String = "spu_create_check_task"
End Module