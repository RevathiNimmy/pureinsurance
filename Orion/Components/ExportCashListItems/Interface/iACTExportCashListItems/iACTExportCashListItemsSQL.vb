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
	' Date: 08/09/1997
	'
	' Description: Contains the SQL Statements required by the
	'              bACTBank.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Select Useretails
	Public Const ACSelectUserDetailsStored As Boolean = False
	Public Const ACSelectUserDetailsName As String = "SelectUserId"
	Public Const ACSelectUserDetailsSQL As String = "SELECT user_id,language_id FROM PMUser where username = {username} and password = {password}"
End Module