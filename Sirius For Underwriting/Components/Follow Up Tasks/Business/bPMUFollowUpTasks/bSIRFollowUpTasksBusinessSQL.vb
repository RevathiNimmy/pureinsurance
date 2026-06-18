Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: BusinessSQL
	'
	' Created: PW301002
	'
	' Description: Contains the SQL Statements required by the
	'              bPMUFollowUpTasks.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	Public Const ACGetFollowUpsStored As Boolean = True
	Public Const ACGetFollowUpsName As String = "GetFollowUps"
    'developer guide no 39. 
    Public Const ACGetFollowUpsSQL As String = "spu_get_follow_ups"
	
	Public Const ACDelFollowUpStored As Boolean = True
	Public Const ACDelFollowUpName As String = "DeleteFollowUp"
    'developer guide no 39. 
    Public Const ACDelFollowUpSQL As String = "spu_del_follow_ups"
End Module