Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 18/06/2007
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:VB
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iCLMCaseHistory"
	
	' String Resources
	' Caption
	Public Const kInterfaceTitle As Integer = 100
	' Buttons
	Public Const kViewButton As Integer = 200
	Public Const kCloseButton As Integer = 201
	
	' Columns Names
	Public Const kLvwColNameDateOfChange As Integer = 101
	Public Const kLvwColNameDescription As Integer = 102
	Public Const kLvwColNameProgressStatus As Integer = 103
	Public Const kLvwColNameUser As Integer = 104
	
	'Lvw Column number
	Public Const kILvwCaseID As Integer = 1
	Public Const kILvwDateOfChange As Integer = 2
	Public Const kILvwDescription As Integer = 3
	Public Const kILvwProgressStatus As Integer = 4
	Public Const kILvwUser As Integer = 5
	
	' ResultArray
	Public Const kICaseID As Integer = 0
	Public Const kIDateOfChange As Integer = 1
	Public Const kIDescription As Integer = 2
	Public Const kIProgressStatus As Integer = 3
	Public Const kIUser As Integer = 4
	
	' Date Formats
	Public Const ACDateConversion As String = "dd/mm/yyyy"
	Public Const ACDateDispaly As String = "long date"
	Public Const ACShortDate As String = "short date"
	
	' Constant for the maxiumum search details.
	Public Const ACMaxSearchDetails As Integer = 500
	
	' Constant for the miniumum search length.
	Public Const ACMinSearchLength As Integer = 1
	
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
 Public g_iUserID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sUserName As String = ""
	
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	
	Sub Main_Renamed()
		
	End Sub
End Module