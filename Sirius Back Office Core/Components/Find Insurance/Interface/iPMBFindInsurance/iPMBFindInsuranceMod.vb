Option Strict Off
Option Explicit On
Imports System
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 17/02/1997
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iSIRFindInsurance"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACTabTitle2 As Integer = 102
	Public Const ACTabTitle3 As Integer = 103
	
	Public Const ACQuoteInterfaceTitle As Integer = 104
	Public Const ACTabTitle5 As Integer = 105
	
	Public Const ACReference As Integer = 110
	Public Const ACRiskIndex As Integer = 111
	Public Const ACProduct As Integer = 112
	Public Const ACAllTypes As Integer = 113
	Public Const ACQuote As Integer = 114
	Public Const ACMTAQuote As Integer = 115
	Public Const ACPolicy As Integer = 116
	Public Const ACRenewal As Integer = 117
	
	Public Const ACShortName As Integer = 118
	Public Const ACType As Integer = 119
	
	Public Const ACListTitle1 As Integer = 120
	Public Const ACListTitle2 As Integer = 121
	Public Const ACListTitle3 As Integer = 122
	Public Const ACListTitle4 As Integer = 123
	Public Const ACListTitle5 As Integer = 124
	Public Const ACListTitle6 As Integer = 125
	Public Const ACListTitle6a As Integer = 126
	Public Const ACListTitle7 As Integer = 127
	Public Const ACListTitle3a As Integer = 128
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACNewButton As Integer = 204
	Public Const ACEditButton As Integer = 205
	Public Const ACFindNowButton As Integer = 206
	Public Const ACNewSearchButton As Integer = 207
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACClearDetailsTitle As Integer = 304
	Public Const ACClearDetails As Integer = 305
	Public Const ACStatusSearching As Integer = 306
	Public Const ACStatusFound As Integer = 307
	
	'SB 31/03/98 defect 37
	Public Const ACLookupFailTitle As Integer = 308
	Public Const ACLookupFail As Integer = 309
	
	'WPR12- Enhancement Quote Collection Process
	Public Const ACMTAQREINS As Integer = 129
	
	' Menus
	
	
	' Constants for the search data array indexes.
	Public Const ACIInsFileId As Integer = 0
	Public Const ACIInsFileSourceId As Integer = 1
	Public Const ACIInsFileCnt As Integer = 2
	Public Const ACIInsReference As Integer = 3
	Public Const ACIInsFolderName As Integer = 4
	Public Const ACIInsFileType As Integer = 5
	Public Const ACIInsuredLongName As Integer = 6
	Public Const ACIInsuredShortName As Integer = 7
	Public Const ACIInsuredId As Integer = 8
	Public Const ACIInsuredSourceId As Integer = 9
	Public Const ACILastModified As Integer = 10
	Public Const ACIInsHolderCnt As Integer = 11
	Public Const ACIInsFolderCnt As Integer = 12
	Public Const ACIProductID As Integer = 13
	Public Const ACIProductCode As Integer = 14
	Public Const ACIProductName As Integer = 15
	Public Const ACILeadAgentCnt As Integer = 16
	Public Const ACIDateCreated As Integer = 17
	Public Const ACIRegistration As Integer = 18 ' Tom 02/11/98
	Public Const ACIGISProperty As Integer = 18 ' Tom 29/06/00
	Public Const ACIIndexValue As Integer = 19 ' Tom 29/06/00
	Public Const ACIStatus As Integer = 18 ' RWH 25/05/01
	
	Public Const ACIObjectName As Integer = 18 ' Ram 08-01-2001
	Public Const ACIPropertyName As Integer = 19 ' Ram 08-01-2001
    Public Const ACIValue As Integer = 28 ' Ram 08-01-2001
	
	'SJ 19/04/2004 - start
	Public Const ACIPolicyTypeId As Integer = 19
	Public Const ACIAlternateReference As Integer = 20
	Public Const ACIUnderwritingBranchInd As Integer = 21
	'SJ 19/04/2004 - end
	
	Public Const PMKeyNameVehicleRegNo As String = "vehicle_reg"
	Public Const ACViaQuoteCollectionProcess As String = "iPMUQUoteCollectionProcess"
	' {* USER DEFINED CODE (End) *}
	
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
	
	'Constants for Date and Date Sort Column
	Public Const ACDateColumn As Integer = 4
	Public Const ACDateSortColumn As Integer = 6
	
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
	
    ' Public instance of the business object.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As Object
	'Public g_oBusiness As bSIRFindInsurance.Form
	
    'Public do we have a link to Gemini
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_bPMGeminiLink As Boolean
	'---------------------------------------------
    'ED 05082002 : Is Registration Search Activated
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_bRegSearch As Boolean
	'---------------------------------------------
	
	Sub Main_Renamed()
		
    End Sub
End Module