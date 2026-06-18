Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 05/05/1999
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMULookupDetailRates"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACInterfaceTitle2 As Integer = 101
	Public Const ACTabTitle1 As Integer = 102
	Public Const ACHeader As Integer = 103
	Public Const ACCaptionHeader1 As Integer = 104
	Public Const ACCaptionHeader2 As Integer = 105
	Public Const ACCaptionHeader3 As Integer = 106
	
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACAddButton As Integer = 204
	Public Const ACEditButton As Integer = 205
	Public Const ACDeleteButton As Integer = 206
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	' Menus
	
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	'Constants for Date and Date Sort Column
	Public Const ACDateColumn As Integer = 0
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	
	' Public instance of the object manager.
	Public g_oObjectManager As bObjectManager.ObjectManager
	
	'Positional constants for the array
	Public Const ACHRecordCnt As Integer = 0
	Public Const ACHCode As Integer = 1
	Public Const ACHDescription As Integer = 2
	Public Const ACHValue As Integer = 3
	
	
	Sub Main_Renamed()
		
	End Sub
End Module