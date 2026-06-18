Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' Main public constant for all functions
	' to identify which application this is.
	
	Public Const ACApp As String = "PMTaskLookup"
	' Constant for the functions to identify
	' which class this is.
	
	Private Const ACClass As String = "MainModule"
	
	Private m_lReturn As Integer
	
	' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
End Module