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
	' Date:  11/03/1998
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	'Public Const ACApp = "bPMPremFinance"
	Public Const ACGetICCSNoStored As Boolean = True
    Public Const ACGetICCSNoName As String = "SelectICCSNo"
    'Developer Guide No 39
    Public Const ACGetICCSNoSQL As String = "spu_pm_iccs"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	
	
	'Constants for Main EDI Array
	
	
	' Database
	
	Sub Main_Renamed()
		
		' Do not put any code in here (It will not be executed).
		
	End Sub
End Module