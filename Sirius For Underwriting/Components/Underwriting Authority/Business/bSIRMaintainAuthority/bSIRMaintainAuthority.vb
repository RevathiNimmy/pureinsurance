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
	' Date:  07/05/1999
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bSIRMaintainScreenData"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	Public Const ACActionAdd As Integer = 1
	Public Const ACActionUpdate As Integer = 2
	Public Const ACActionDelete As Integer = 3
	
	'Array constants for User Authority Levels.
	Public Const ACUserAuthMaxIndex As Integer = 3
	Public Const ACUserAuthProductId As Integer = 0
	Public Const ACUserAuthProductDesc As Integer = 1
	Public Const ACUserAuthLevelTypeId As Integer = 2
	Public Const ACUserAuthLevelTypeDesc As Integer = 3
	
	
	'Array constants for Rule Inclusions.
	Public Const ACInclusionFieldMaxIndex As Integer = 4
	Public Const ACUARuleDetailId As Integer = 0
	Public Const ACUARuleDetailRuleId As Integer = 1
	Public Const ACUARuleDetailInclude As Integer = 2
	Public Const ACUARuleDetailValue As Integer = 3
	Public Const ACUARuleDetailAction As Integer = 4
	
	
	
	
	
	
	Sub Main_Renamed()
		
		
	End Sub
End Module