Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 31/11/00
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History: DC Created
	' ***************************************************************** '
	
	
	Public Const ScreenHelpID As Integer = 44000
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "uctListClaimControl"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACTabTitle2 As Integer = 102
	Public Const ACClientCode As Integer = 103
	Public Const ACStatus As Integer = 104
	Public Const ACPolicyCode As Integer = 105
	
	' Listview
	Public Const ACListTitle_ClaimNumber As Integer = 113
	Public Const ACListTitle_PolicyNumber As Integer = 114
	Public Const ACListTitle_ClaimDate As Integer = 115
	Public Const ACListTitle_Description As Integer = 116
	Public Const ACListTitle_Reserve As Integer = 117
	Public Const ACListTitle_Payment As Integer = 118
	Public Const ACListTitle_Handler As Integer = 119
	Public Const ACListTitle_Status As Integer = 120
	Public Const ACListTitle_Currency As Integer = 121
	
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
	Public Const ACLookupFailTitle As Integer = 308
	Public Const ACLookupFail As Integer = 309
	
	' Menus
	
	'Column numbers for Broking
	Public Const ACColumnClaimNumber As Integer = 1
	Public Const ACColumnPolicyNumber As Integer = 2
	Public Const ACColumnClaimDate As Integer = 3
	Public Const ACColumnDescription As Integer = 4
	Public Const ACColumnReserve As Integer = 5
	Public Const ACColumnPayment As Integer = 6
	Public Const ACColumnHandler As Integer = 7
	Public Const ACColumnStatus As Integer = 8
	
	' Constants for the search data array indexes.
	Public Const ACIInsFileCnt As Integer = 2
	Public Const ACIInsReference As Integer = 3
	Public Const ACIInsFolderName As Integer = 4
	Public Const ACIInsFileType As Integer = 5
	Public Const ACIInsuredLongName As Integer = 6
	Public Const ACIInsuredShortName As Integer = 7
	Public Const ACIInsuredId As Integer = 8
	Public Const ACIInsHolderCnt As Integer = 11
	Public Const ACIInsFolderCnt As Integer = 12
	
	'Broking & Underwriting
	Public Const ACIClmPolicyId As Integer = 0
	Public Const ACIClmClaimId As Integer = 1
	Public Const ACIClmDescription As Integer = 2
	Public Const ACIClmClaimNumber As Integer = 3
	Public Const ACIClmPolicyNumber As Integer = 4
	
	'Broking Only
	Public Const ACIClmClaimDate As Integer = 5
	Public Const ACIClmClientName As Integer = 6
	Public Const ACIClmStatusId As Integer = 7
	Public Const ACIClmHandler As Integer = 8
	Public Const ACIClmRiskTypeId As Integer = 9
	Public Const ACIClmPayment As Integer = 18 'MKW170903 PN6326
	Public Const ACIClmReserve As Integer = 19 'MKW170903 PN6326
	
	'Underwriting Only
	Public Const ACIClmRiskTypeIdU As Integer = 5
	Public Const ACIClmShortnameU As Integer = 6
	Public Const ACIClmClaimDateU As Integer = 7
	Public Const ACIClmClientNameU As Integer = 8
	Public Const ACIClmStatusIdU As Integer = 9
	Public Const ACIClmHandlerU As Integer = 10
	Public Const ACICurrencyU As Integer = 20
	'
	Public Const PMKeyNameVehicleRegNo As String = "vehicle_reg"
	
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
	Public Const ACDateColumn As Integer = 2 'MKW230703 PN4360 Changed Sorting Method
	Public Const ACDateSortColumn As Integer = 6
	
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
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
    ' Public instance of the business object.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oPMUser As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sProduct As String = ""
	
	'Constants for Claim Status
	
	Public Const CLMProvisionalOpenClaim As Integer = 1
	Public Const CLMLiveOpenClaim As Integer = 2
	Public Const CLMClosed As Integer = 3
	Public Const CLMReOpened As Integer = 4
	Public Const CLMReClosed As Integer = 5
	
	'Constants for UnderWriting and Broking
	Public Const ACBroking As String = "A"
	Public Const ACUnderWriting As String = "U"
End Module