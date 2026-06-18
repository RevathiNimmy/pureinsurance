Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	
	Public Const USER_MODE_USER As Integer = 1
	Public Const USER_MODE_ADMIN As Integer = 2
	
	Public Const MSG_MODE_NEWMAP As Integer = 0
	Public Const MSG_MODE_OLDMAP As Integer = 1
	Public Const MSG_MODE_NEWTASK As Integer = 2
	
	Public Const ID_NO_VALUE As Integer = -1
	
	Public Const FORMAT_DATE As String = "dd mmm yyyy hh:nn:ss"
	
	Public Const ACApp As String = "bPMNavXMEditor"
End Module