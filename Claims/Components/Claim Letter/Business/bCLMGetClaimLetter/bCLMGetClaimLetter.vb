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
	' Date:  06/10/1998
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	'Constants To Identify Table
	Public Const ACRecovery As Integer = 0
	Public Const ACReceipt As Integer = 1
	Public Const ACPayment As Integer = 2
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bCLMGetDocument"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	
	
	
	Sub Main_Renamed()
		
		
	End Sub
End Module