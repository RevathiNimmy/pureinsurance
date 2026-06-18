Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  20092002
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bCLMAuthorisePayments"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
End Module