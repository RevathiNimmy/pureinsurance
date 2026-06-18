Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 18 July 1996
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main global constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iLicenceAdmin"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
    ' Username and Password
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sUsername As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sPassword As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sCallingAppName As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iCurrencyID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
End Module