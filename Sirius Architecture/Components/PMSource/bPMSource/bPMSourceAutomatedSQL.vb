Option Strict Off
Option Explicit On
Imports System
Module AutomatedSQL
	' ***************************************************************** '
	' Class Name: AutomatedSQL
	'
	' Date: 31/07/1997
	'
	' Description: Contains the SQL Statements required by the
	'              bPMSource.Automated class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACAutoSelectStored = False
	' Public Const ACAutoSelectName = "SelectRisk"
	' Public Const ACAutoSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select Source SQL
	Public Const ACAutoGetDetailsStored As Boolean = True
	Public Const ACAutoGetDetailsName As String = "SelectAllSource"
    'Developer Guide No. 39
    Public Const ACAutoGetDetailsSQL As String = "spu_PM_Select_Source"

    ' Select All Source SQL
    Public Const ACAutoGetAllDetailsStored As Boolean = True
    Public Const ACAutoGetAllDetailsName As String = "SelectAllSource"
    'Developer Guide No. 39
    Public Const ACAutoGetAllDetailsSQL As String = "spu_PM_SelAll_Source"

    ' Check ID SQL
    Public Const ACAutoCheckIDStored As Boolean = True
    Public Const ACAutoCheckIDName As String = "CheckSourceID"
    'Developer Guide No. 39
    Public Const ACAutoCheckIDSQL As String = "spu_PM_Check_Source"

    ' Add Source SQL
    Public Const ACAutoAddStored As Boolean = True
    Public Const ACAutoAddName As String = "AddSource"
    ' DC 31/01/00 added new ? for each new field
    'Developer Guide No. 39
    Public Const ACAutoAddSQL As String = "spu_PM_Add_Source"

    ' Delete Source SQL
    Public Const ACAutoDeleteStored As Boolean = True
    Public Const ACAutoDeleteName As String = "DeleteSource"
    'Developer Guide No. 39
    Public Const ACAutoDeleteSQL As String = "spu_PM_Delete_Source"

    ' Update Source SQL
    Public Const ACAutoUpdateStored As Boolean = True
    Public Const ACAutoUpdateName As String = "UpdateSource"
    ' DC 31/01/00 added new ? for each new field
    'Developer Guide No. 39
    Public Const ACAutoUpdateSQL As String = "spu_PM_Update_Source"
    'Developer Guide No. 29
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module