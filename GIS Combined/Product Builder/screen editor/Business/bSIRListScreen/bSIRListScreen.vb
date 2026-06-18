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
	' Date:  27th September 1996
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bSIRListScreen"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	
	
	
	
	
	' Log Level
	
	
	Public Const ACSGISScreenId As Integer = 0
	Public Const ACSGISDataModelId As Integer = 1
	Public Const ACSCaptionId As Integer = 2
	Public Const ACSCode As Integer = 3
	Public Const ACSDescription As Integer = 4
	Public Const ACSIsDeleted As Integer = 5
	Public Const ACSEffectiveDate As Integer = 6
	Public Const ACSParentId As Integer = 7
	Public Const ACSIsMaintainable As Integer = 8
	Public Const ACSDMCode As Integer = 9
	Public Const ACSDMDescription As Integer = 10
	
	Public Const ACArrayMax As Integer = 10
	
	Sub Main_Renamed()
		
		' Main entry point for the component
		
	End Sub
End Module