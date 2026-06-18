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
	
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  12th July 2000
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	Public Const ACApp As String = "bPMServerRegistry"
	Private Const ACClass As String = "MainModule"
	
	
	Sub Main_Renamed()
		
	End Sub
End Module