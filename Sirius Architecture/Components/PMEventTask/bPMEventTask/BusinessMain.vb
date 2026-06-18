Option Strict Off
Option Explicit On
Imports System
Module BusinessMain
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Module Name: BusinesMain
	'
	' Date:  04-12-2002
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bPMEventTask"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "BusinessMain"
	
	' Initialisation variable stores
End Module