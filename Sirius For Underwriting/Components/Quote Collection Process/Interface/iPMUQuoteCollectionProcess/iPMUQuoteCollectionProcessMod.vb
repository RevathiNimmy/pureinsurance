Option Strict Off
Option Explicit On
Imports System
'developer guide no. 129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 23-07-1997
	'
	' Description: Main module containing public variable/constants.
	'
	' ***************************************************************** '
	
	
	' Main public constant for all functions to identify which application this is.
	Public Const ACApp As String = "iPMUQUoteCollectionProcess"
	
	
	' Public interface constants used when retrieving data from the resource file.
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "MainModule"
	
    ' Public source and language ID's from the  Object Manager.
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
	
    ' Public instances of the business objects.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iCurrencyID As Integer
	
	Public Const ScreenHelpID As Integer = 6000
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFOrion
	
	Public Const ACInterfaceCaption As Integer = 100 'Find: Quote Collection Process
	
	' Buttons
	Public Const ACFindQuoteButton As Integer = 101
	Public Const ACFindClientButton As Integer = 102
	Public Const ACFindAgentButton As Integer = 103
	Public Const ACFindProductButton As Integer = 104
	
	Public Const ACQuoteTypeLabel As Integer = 105
	Public Const ACCoverStartDateLabel As Integer = 106
	Public Const ACCoverEndDateLabel As Integer = 107
	Public Const ACRiskIndexLabel As Integer = 108
	Public Const ACDirectBusinessCheck As Integer = 109
	
	Public Const ACFindNowButton As Integer = 110
	Public Const ACNewSearchButton As Integer = 111
	
	Public Const ACOKButton As Integer = 112
	Public Const ACCancelButton As Integer = 113
	Public Const ACHelpButton As Integer = 114
	
	Public Const ACSelectAllButton As Integer = 115
	Public Const ACMakePaymentButton As Integer = 116
	
	'List
	
	Public Const ACListTitle1 As Integer = 117 'Quote Number
	Public Const ACListTitle2 As Integer = 118 'Currency
	Public Const ACListTitle3 As Integer = 119 'Insurance_file_cnt
	Public Const ACListTitle4 As Integer = 120 'Client Name
	Public Const ACListTitle5 As Integer = 121 'Agent
	Public Const ACListTitle6 As Integer = 122 'Product
	Public Const ACListTitle7 As Integer = 123 'Branch
	Public Const ACListTitle8 As Integer = 124 'Amount Due
	
	
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACClearDetailsTitle As Integer = 304
	Public Const ACClearDetails As Integer = 305
	Public Const ACStatusSearching As Integer = 306
	Public Const ACStatusFound As Integer = 307
	
	Public Const ACLookupFailTitle As Integer = 308
	Public Const ACLookupFail As Integer = 309
	
	Public Const ACInvalidDateMsg As Integer = 400 'Invalid Date Entered
	Public Const ACDateDiffError As Integer = 401 'Date Diff Error
	
	' Constants for the search data array indexes.
	Public Const ACIInsuranceFileCnt As Integer = 0
	Public Const ACIInsuranceRef As Integer = 1
	Public Const ACIClientID As Integer = 2
	Public Const ACIClientName As Integer = 3
	Public Const ACIClientResolvedName As Integer = 4
	Public Const ACIAgentID As Integer = 5
	Public Const ACIAgentRef As Integer = 6
	Public Const ACIAgentResolvedName As Integer = 7
	Public Const ACIProductID As Integer = 8
	Public Const ACIProductCode As Integer = 9
	Public Const ACISourceID As Integer = 10
	Public Const ACISource As Integer = 11
	Public Const ACICurrencyID As Integer = 12
	Public Const ACICurrencyCode As Integer = 13
	Public Const ACIPremium As Integer = 14
	Public Const ACIInsuranceFileType As Integer = 15
	Public Const ACIAgentType As Integer = 16
	Public Const ACIAgentCommission As Integer = 17
	Public Const ACIRoundAmount As Integer = 18
	
	Public Const MAXCOL As Integer = ACIAgentCommission
	

    'Developer Guide No 218
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oBackofficelink As bBackOfficeLink.bBOLink


	
	Sub Main_Renamed()
		
	End Sub
End Module