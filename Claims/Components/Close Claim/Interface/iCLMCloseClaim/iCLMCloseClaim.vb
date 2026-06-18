Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.IO
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 20/09/2000
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iCLMCloseClaim"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	' Form
	' Buttons
	
	' Messages
	Public Const ACInvalidStatusTitle As Integer = 300
	Public Const ACInvalidStatus As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	' Menus
	
	
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
    Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lInsuranceFileCnt As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	Sub Main_Renamed()
		
	End Sub
End Module