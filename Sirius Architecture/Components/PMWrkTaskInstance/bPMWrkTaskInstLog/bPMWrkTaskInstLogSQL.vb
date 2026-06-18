Option Strict Off
Option Explicit On
Imports System
Module PMWrkTaskInstLogSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: PMWrkTaskInstLogSQL
	'
	' Date: 26/10/1998
	'
	' Description: SQL and Stored procedures
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Add Log Entry
	Public Const ACAddLogEntryStored As Boolean = True
	Public Const ACAddLogEntryName As String = "AddLogEntry"
    'developers guide no 39
    'Public Const ACAddLogEntrySQL As String = "{call spe_PMWrk_Task_Inst_Log_add (?,?,?,?)}"
    Public Const ACAddLogEntrySQL As String = "spe_PMWrk_Task_Inst_Log_add"
	
	
	' Add Log Entry
	Public Const ACSelAllDescStored As Boolean = True
	Public Const ACSelAllDescName As String = "SelAllDesc"
    'developers guide no 39
    'Public Const ACSelAllDescSQL As String = "{call spu_PMWrk_Task_Inst_Log_sad (?)}"
    Public Const ACSelAllDescSQL As String = "spu_PMWrk_Task_Inst_Log_sad"
End Module