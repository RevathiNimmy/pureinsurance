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
	' Date:  09/06/1999
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bSIRRIModelLine"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	
	
	
	Public Const ACRRIModelId As Integer = 0
	Public Const ACRRIModelLineId As Integer = 1
	Public Const ACRTreatyId As Integer = 2
	Public Const ACRTreatyDescription As Integer = 3
	Public Const ACRMethod As Integer = 4
	Public Const ACRPriority As Integer = 5
	Public Const ACRNumberOfLines As Integer = 6
	Public Const ACRLimitSingleSI As Integer = 7
	Public Const ACRLimitBlockSI As Integer = 8
	Public Const ACRLimitTotalSI As Integer = 9
End Module