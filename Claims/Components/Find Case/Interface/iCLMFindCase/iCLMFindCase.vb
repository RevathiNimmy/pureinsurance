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
	Public Const ACApp As String = "iCLMFindCase"
	
	' String Resources
	' Caption
	Public Const kInterfaceTitle As Integer = 100
	Public Const kTabTitle1 As Integer = 101
	Public Const kTabTitle2 As Integer = 102
	Public Const kCaseNumber As Integer = 103
	Public Const kCaseOpenDate As Integer = 104
	Public Const kClaimNumber As Integer = 105
	Public Const kProgressStatus As Integer = 106
	Public Const kRiskType As Integer = 107
	
	' Buttons
	Public Const kFindNowButton As Integer = 200
	Public Const kNewSearchButton As Integer = 201
	Public Const kNewCaseButton As Integer = 202
	Public Const kEditCaseButton As Integer = 203
	Public Const kCloseCaseButton As Integer = 204
	Public Const kCloseButton As Integer = 205
	
	' Columns Names
	Public Const kLvwColNameCaseNumber As Integer = 108
	Public Const kLvwColNameOpenedDate As Integer = 109
	Public Const kLvwColNameAnalyst As Integer = 110
	Public Const kLvwColNameAssistant As Integer = 111
	Public Const kLvwColNameProgressStatus As Integer = 112
	Public Const kLvwColNameTotalIndemnity As Integer = 113
	Public Const kLvwColNameTotalExpense As Integer = 114
	Public Const kLvwColNameTotalExcess As Integer = 115
	
	' Column Number
	Public Const kILvwCaseNumber As Integer = 1
	Public Const kILvwCaseOpenDate As Integer = 2
	Public Const kILvwAnalyst As Integer = 3
	Public Const kILvwAssistant As Integer = 4
	Public Const kILvwProgressStatus As Integer = 5
	Public Const kILvwTotalIndemnity As Integer = 6
	Public Const kILvwTotalExpense As Integer = 7
	Public Const kILvwTotalExcess As Integer = 8
	Public Const kILvwCaseID As Integer = 9
	
	'Column Number
	Public Const kILvwColCaseNumber As Integer = 1
	Public Const kILvwColCaseOpenDate As Integer = 2
	Public Const kILvwColAnalyst As Integer = 3
	Public Const kILvwColAssistant As Integer = 4
	Public Const kILvwColProgressStatus As Integer = 5
	Public Const kILvwColTotalIndemnity As Integer = 6
	Public Const kILvwColTotalExpense As Integer = 7
	Public Const kILvwColTotalExcess As Integer = 8
	Public Const kILvwColCaseID As Integer = 0
	
	' ResultArray
	Public Const kICaseID As Integer = 0
	Public Const kICaseNumber As Integer = 1
	Public Const kICaseOpenDate As Integer = 2
	Public Const kIAnalyst As Integer = 3
	Public Const kIAssistant As Integer = 4
	Public Const kIProgressStatus As Integer = 5
	Public Const kITotalIndemnity As Integer = 6
	Public Const kITotalExpense As Integer = 7
	Public Const kITotalExcess As Integer = 8
	Public Const kIBaseCaseID As Integer = 9
	Public Const KIPartyCnt As Integer = 10
	
	' Date Formats
	Public Const ACDateConversion As String = "dd/mm/yyyy"
	Public Const ACDateDispaly As String = "long date"
	Public Const ACShortDate As String = "short date"
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACClearDetailsTitle As Integer = 302
	Public Const ACClearDetails As Integer = 303
	Public Const ACStatusSearching As Integer = 304
	Public Const ACStatusFound As Integer = 305
	Public Const ACBusinessFail As Integer = 306
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the maxiumum search details.
	Public Const ACMaxSearchDetails As Integer = 500
	
	' Constant for the miniumum search length.
	Public Const ACMinSearchLength As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
    Public g_iLanguageID As Integer
    Public g_sCaseNumber As String
    Public g_bChangeCloseCaption As Boolean
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Public instance of the business object.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As bCLMCase.Business
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBackofficelink As bBackOfficeLink.bBOLink
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oPMUser As Object
	
	Sub Main_Renamed()
		
	End Sub
End Module