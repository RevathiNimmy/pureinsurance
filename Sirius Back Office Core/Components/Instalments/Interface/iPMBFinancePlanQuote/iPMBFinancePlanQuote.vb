Option Strict Off
Option Explicit On
Imports System
' developer guide no. 129 (guide)
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
 Public Module MainModule
	'*************************************************************************
	' Module Name: MainModule
	' Date: 17/02/1997
	' Description: Main module containing public variable/constants.
	' Edit History:
	' SP130199 - Remove NavigatorV3 class an put in stub so can be called
	' iteratively.
	'*************************************************************************
	
	'================
	'Public Constants
	'================
	' Constant for the functions to identify which class this is.
	Public Const ACApp As String = "iPMBFinancePlanQuote"
    Public g_sProductFamily As String = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	Public Const ScreenHelpID As Integer = 1
	
	'=================
	'Private Constants
	'=================
	Private Const ACClass As String = "MainModule"
	
	'================
	'Public Variables
    '================
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserId As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sUsername As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As Object
	'TR10/03/03 Issue 2873 - Causing a problem and not used anywhere
	'Public g_bGenericConnectionStatus As Boolean
End Module