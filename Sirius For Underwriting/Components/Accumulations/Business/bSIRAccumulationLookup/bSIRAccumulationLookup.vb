Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  25 September 1996
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bSIRAccumulationLookup"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Username.
	Public g_sUsername As New FixedLengthString(12)
	
	' Password.
	Public g_sPassword As New FixedLengthString(30)
	
    ' Calling Application
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sCallingAppName As String = ""
    ' Source ID
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    ' Language ID
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    ' Currency ID
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iCurrencyID As Integer
    ' LogLevel
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLogLevel As Integer
    ' UserID
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
	
	Sub main_Renamed()
		
		' Main entry point for the component
		
	End Sub
End Module