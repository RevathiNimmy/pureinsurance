Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Module MainModule
	
	' This App
	Public Const ACApp As String = "bPMMaintainLookupWrapper"
	
	' This Class
	Private Const ACClass As String = "MainModule"
	
	Public Const ACProductTable As String = "PMProduct"
	
    ' Object Manager
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	
    ' Username.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sUsername As String = ""
	
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
	
    ' Log Level
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLogLevel As Integer
	
    ' Currency ID
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iCurrencyID As Integer
End Module