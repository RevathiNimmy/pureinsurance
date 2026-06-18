Option Strict Off
Option Explicit On
Imports System
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: {TodaysDate}
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iACTFindDocument"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	
	Public Const ACListTitle1 As Integer = 102
	Public Const ACListTitle2 As Integer = 103
	Public Const ACListTitle3 As Integer = 104
	Public Const ACListTitle4 As Integer = 105
	Public Const ACListTitle5 As Integer = 106
	Public Const ACListTitle6 As Integer = 107
	
	Public Const ACDocumentRef As Integer = 110
	Public Const ACDateFrom As Integer = 111
	Public Const ACType As Integer = 112
	Public Const ACStatus As Integer = 113
	Public Const ACDateTo As Integer = 114
	Public Const ACBranch As Integer = 115
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACReverseButton As Integer = 203
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACClearDetailsTitle As Integer = 304
	Public Const ACClearDetails As Integer = 305
	Public Const ACStatusSearching As Integer = 306
	Public Const ACStatusFound As Integer = 307
	
	' Menus
	Public Const ACComboAny As Integer = 400
	
	' Constants for the search data array indexes.
	Public Const ACIDocumentRef As Integer = 0
	Public Const ACIDocumentDate As Integer = 1
	Public Const ACIDocumentType As Integer = 2
	Public Const ACIPostingStatus As Integer = 3
	Public Const ACIComment As Integer = 4
	Public Const ACIDocumentId As Integer = 5
	Public Const ACIDocumentDateSort As Integer = 5
	
	'TN20010702 - start
	Public Const ACIFromSirius As Integer = 6
	'TN20010702 - end
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the maxiumum search details.
	Public Const ACMaxSearchDetails As Integer = 500
	
	' Constant for the miniumum search length.
	Public Const ACMinSearchLength As Integer = 3
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Public instance of the business object.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As Object
	
	Public g_iCurrencyID As Integer
	
	'Product Family Name for Help
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFOrion
	
	Public Const ScreenHelpID As Integer = 19000
	
	Sub Main_Renamed()
		
	End Sub
End Module