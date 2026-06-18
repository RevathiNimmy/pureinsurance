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
	' Date: 25/04/1997
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iACTCLIRepeatDecision"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	
	' Buttons
	
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	
	
	' Messages
	
	' Menus
	
	
	' {* USER DEFINED CODE (End) *}
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iCurrencyID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserId As Integer
	
	
	' Constants for the List data array subscripts.
	Public Const ACDescriptionID As Integer = 0
	Public Const ACDescription As Integer = 1
	Public Const ACChosen As Integer = 2
	Public Const ACisOther As Integer = 3
	Public Const ACisDefault As Integer = 4
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	
	
	
	Sub Main_Renamed()
		
	End Sub
End Module