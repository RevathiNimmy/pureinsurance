Option Strict Off
Option Explicit On
Imports System
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
	Public Const ACApp As String = "iPMUChangePolicyStatus"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	' Form
	Public Const AC_InsuredName As Integer = 100
	Public Const AC_Agent As Integer = 101
	Public Const AC_IncDate As Integer = 102
	Public Const AC_CoverFromDate As Integer = 103
	Public Const AC_ExpiryDate As Integer = 104
	Public Const AC_NetPremium As Integer = 105
	Public Const AC_Tax As Integer = 106
	Public Const AC_Fee As Integer = 107
	Public Const AC_TotalPremium As Integer = 108
	Public Const AC_Currency As Integer = 109
	Public Const AC_FormCaption As Integer = 110
	
	Public Const AC_Button_Requote As Integer = 200
	Public Const AC_Button_Save As Integer = 201
	Public Const AC_Button_MakeLive As Integer = 202
	
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
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	Sub Main_Renamed()
		
	End Sub
End Module