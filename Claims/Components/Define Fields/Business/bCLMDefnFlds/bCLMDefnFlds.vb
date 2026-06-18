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
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bCLMDefnFlds"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	''holds the Reserve Type ID globally
	'***********************************************
	' TO DO: GLOBAL VARIABLE IN BAS MODULE
	''Public g_lDataDefnID As Long
	'***********************************************
	
	'CT 14/11/00 added constants identify mode
	Public Const ACRiskMode As Integer = 0 'when screen is being called from Risk Type screen
	Public Const ACPerilMode As Integer = 1 'when screen is being called from Peril Type screen
	Sub Main_Renamed()
		
		
	End Sub
End Module