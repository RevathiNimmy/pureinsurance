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
	' Date: 03 July 1996
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main Public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bLicenceManager"
	
	' The name of the Client Manager that the Licence
	' Manager is using.
	Public Const ACClientManager As String = "bClientManager.ClientManager"
	
	' Maximum number of instances allowed to be
	' instatiated at once.
	'***********************************************
	' TO DO: GLOBAL VARIABLE IN BAS MODULE
	'Public g_iLicenceLimit As Integer
	'***********************************************
	
	' The size of the pool of free objects the
	' Licence Manager maintains.
	'***********************************************
	' TO DO: GLOBAL VARIABLE IN BAS MODULE
	'Public g_iPoolSize As Integer
	'***********************************************
	
	' Username and Password
	
	' Language, Source etc
	'***********************************************
	' TO DO: GLOBAL VARIABLE IN BAS MODULE
	'Public g_iHomeCountryID As Integer
	'***********************************************
	
	' Constants apply to InstanceState.
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	Sub Main_Renamed()
		
	End Sub
End Module