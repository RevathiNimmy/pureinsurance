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
	' Date:  23/04/1998
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bGEMListManager"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	
	
	
	Public Const GEMRegKeyListManagement As String = "ListManagement"
	Public Const GEMRegKeyListPolaris As String = "Polaris"
	
	' Registry file paths
	Public Const GEMServerListVersion As String = "ServerListVersion"
    Public Const GEMServerListPrefVersion As String = "ServerListPrefVersion"
    Public Const GEMServerListFileCompressed As String = "ServerListFileCompressed"
    Public Const GEMServerListFilePath As String = "ServerListFilePath"
    Public Const GEMServerPolarisFilePath As String = "ServerPolarisFilePath"
    Public Const GEMServerPolarisAppPath As String = "AppPath"
    Public Const GEMCommonPolarisAppVer As String = "AppVer"

	
	Sub Main_Renamed()
		
		
	End Sub
End Module