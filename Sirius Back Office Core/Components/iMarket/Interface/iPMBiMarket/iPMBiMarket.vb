Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 06/03/2001
	'
	' Description: Main module containing public variable/constants.
	'
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMBiMarket"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	' Username.
	Public g_sUsername As New FixedLengthString(12)
	
	' Password.
	Public g_sPassword As New FixedLengthString(30)
	
    ' User ID
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
	
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
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	Declare Function ShellExecute Lib "shell32.dll"  Alias "ShellExecuteA"(ByVal hwnd As Integer, ByVal lpOperation As String, ByVal lpFile As String, ByVal lpParameters As String, ByVal lpDirectory As String, ByVal nShowCmd As Integer) As Integer
End Module