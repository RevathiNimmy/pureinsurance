Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' Main public constant for all functions
	' to identify which application this is.
	
	Public Const ACApp As String = "PMLookup"
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
	
	'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.1.3)
	Public Const kUSLangId As Integer = 2
	Public Const kUKLangId As Integer = 1
	'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.1.3)
End Module