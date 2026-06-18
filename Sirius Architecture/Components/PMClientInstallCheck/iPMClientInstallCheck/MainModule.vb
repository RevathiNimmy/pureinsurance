Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 12/02/1999
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMClientInstallCheck"
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
	
	' Reboot Levels
	Public Const ACNoReboot As Integer = 0
	Public Const ACLogoffOnly As Integer = 1
	Public Const ACFullReboot As Integer = 2
	
	Public Const ACRunOnceRegKey As String = "software\Microsoft\Windows\CurrentVersion\RunOnce"
	Public Const ACRunSettingName As String = "ClientInstall"
	
	Sub Main_Renamed()
		
	End Sub
End Module