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
	' Date:  {TodaysDate}
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bCLMRiskDetails"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	
	
	
	' Constants for the tables
	Public Const g_iClaim As Integer = 1
	Public Const g_iRiskType As Integer = 2
	Public Const g_iRiskDataDefn As Integer = 3
	
	'Contants for the claim status
	Public Const CLMProvisionalOpenClaim As Integer = 1
	Public Const CLMLiveOpenClaim As Integer = 2
	Public Const CLMClosed As Integer = 3
	Public Const CLMReOpened As Integer = 4
	Public Const CLMReClosed As Integer = 5
End Module