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
	Public Const ACApp As String = "bPMLicenceManager"
	
	' The name of the Client Manager that the Licence
	' Manager is using.
	Public Const ACClientManager As String = "bClientManager.ClientManager"
	' RFC231100 - Use InProcess Client Manager to run in COM+
	Public Const ACClientManagerCOMPlus As String = "bPMClientManager.ClientManager"
	
	' More to do here, this needs to be added to gPMLibraries
	' RDC 09042002 moved to gPMLibraries as ACRegKeyClientManagerCOMPlus
	'Public Const PMRegKeyClientManagerInProc As String = "ClientManagerInProcess"
	
	
	' Constants apply to InstanceState.
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	Sub Main_Renamed()
		
	End Sub
End Module