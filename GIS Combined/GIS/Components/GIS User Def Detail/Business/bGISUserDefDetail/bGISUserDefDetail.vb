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
	Public Const ACApp As String = "bGISUserDefDetail"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	
	
	
	'Positional constants for the array
	Public Const ACHLookupDetailId As Integer = 0
	Public Const ACHLookupHeaderId As Integer = 1
	Public Const ACHCaptionId As Integer = 2
	Public Const ACHCode As Integer = 3
	Public Const ACHDescription As Integer = 4
	Public Const ACHIsDeleted As Integer = 5
	Public Const ACHEffectiveDate As Integer = 6
	Public Const ACHParentId As Integer = 7
	Public Const ACHLookupHeaderIndsId As Integer = 8
	
	Sub Main_Renamed()
		
		
	End Sub
End Module