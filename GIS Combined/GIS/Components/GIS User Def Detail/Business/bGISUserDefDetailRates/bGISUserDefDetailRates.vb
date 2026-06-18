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
	' Date:  05/05/1999
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bSIRLookupHeaderRates"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	
	
	
	'Positional constants for the array
	Public Const ACHRecordCnt As Integer = 0
	Public Const ACHCode As Integer = 1
	Public Const ACHDescription As Integer = 2
	Public Const ACHValue As Integer = 3
	
	Sub Main_Renamed()
		
		
	End Sub
End Module