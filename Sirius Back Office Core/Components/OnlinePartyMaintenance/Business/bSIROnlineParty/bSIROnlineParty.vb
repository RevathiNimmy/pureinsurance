Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  17 October 1996
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bPMUserGroup"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
    ' Username.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_sUsername As String = ""

    ' Password.
    Public g_sPassword As New FixedLengthString(30)

    ' Calling Application
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_sCallingAppName As String = ""
    ' Source ID
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    ' Language ID
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    ' Currency ID
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iCurrencyID As Integer
    ' LogLevel
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLogLevel As Integer
    ' UserID
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
	
	' Constants for ChangeArray
	Public Const ACICAPartyCnt As Byte = 0
	Public Const ACICAPartyShortName As Byte = 1
	Public Const ACICAAccessStatus As Byte = 2
	Public Const ACICAFailureReason As Byte = 3
	
	
	Sub Main_Renamed()
		
		' Main entry point for the component
		
	End Sub
End Module