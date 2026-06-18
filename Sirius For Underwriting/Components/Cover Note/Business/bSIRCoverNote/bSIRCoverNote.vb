Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Module MainModule
	
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  27th July 2007
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bSIRCoverNote"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	Public g_sUsername As New FixedLengthString(12)
    Public g_sPassword As New FixedLengthString(30)
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sCallingAppName As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLogLevel As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iCurrencyID As Integer
	
	Public Const PMSearchSirius As Integer = 0
	Public Const PMSearchPMB As Integer = 1
	Public Const PMSearchSiriusPMB As Integer = 2
	
	' Constants for the select book array
	Public Const ACBookNumber As Integer = 0
	Public Const ACStartNumber As Integer = 1
	Public Const ACEndNumber As Integer = 2
	Public Const ACEffectiveDate As Integer = 3
	Public Const ACAgentId As Integer = 4
	Public Const ACAgentName As Integer = 5
	Public Const ACBranch As Integer = 6
	Public Const ACBookStatus As Integer = 7
	Public Const ACUser As Integer = 8
	Public Const ACCreatedDate As Integer = 9
	Public Const ACDateUpdated As Integer = 10
	
	
	Sub Main_Renamed()
		
		' Main entry point for the component
		
	End Sub
End Module