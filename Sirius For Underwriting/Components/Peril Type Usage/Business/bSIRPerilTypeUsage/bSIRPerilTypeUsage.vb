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
	Public Const ACApp As String = "bSIRPerilTypeUsage"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"

	Public Const ACPPerilGroupId As Integer = 0
	Public Const ACPPerilTypeId As Integer = 1
	Public Const ACPAllocatePercent As Integer = 2
	Public Const ACPPerilTypeCode As Integer = 3
	Public Const ACPPerilTypeDescription As Integer = 4
	
	Public Const ACEarningPatternValue As Integer = 0
	Public Const ACEffectiveDateValue As Integer = 1
End Module