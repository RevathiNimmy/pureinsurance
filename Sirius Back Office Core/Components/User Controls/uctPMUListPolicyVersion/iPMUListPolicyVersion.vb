Option Strict Off
Option Explicit On
Imports System
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
	
	
	Public Const ScreenHelpID As Integer = 44000
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "uctListPolicyVersionControl"
	
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
	
	'JMK 12/05/2001 Rename constants
	Public Const ACListTitlePolicyNumber As Integer = 113
	Public Const ACListTitlePolicyType As Integer = 114
	Public Const ACListTitleRiskType As Integer = 115
	Public Const ACListTitleRenewalDate As Integer = 117
	Public Const ACListTitleInsured As Integer = 118
	Public Const ACListTitlePremium As Integer = 119
	Public Const ACListTitlePolicyStatus As Integer = 120
	'JMK Add 2 new constants
	Public Const ACListTitleCoverStart As Integer = 123
	Public Const ACListTitleCoverEnd As Integer = 124
	Public Const ACListTitleMTAReason As Integer = 125
	
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
	
	' Menus
	
	
	' Constants for the search data array indexes.
	Public Const ACIInsuranceFolderCnt As Integer = 0
	Public Const ACIInsuranceFileCnt As Integer = 1
	Public Const ACIInsuranceHolderCnt As Integer = 2
	Public Const ACIPolicyTypeID As Integer = 3
	Public Const ACIInsuranceRef As Integer = 4
	Public Const ACIInsuranceFileType As Integer = 5
	Public Const ACIProduct As Integer = 6
	Public Const ACIRenewalDate As Integer = 7
	Public Const ACIInsurerName As Integer = 8
	Public Const ACIShortName As Integer = 9
	Public Const ACIPremium As Integer = 10
	Public Const ACIInsuranceFileStatus As Integer = 11
	Public Const ACIInsuranceFileTypeID As Integer = 12
	Public Const ACICoverStartDate As Integer = 13
	Public Const ACICoverEndDate As Integer = 14
	Public Const ACIMTAReason As Integer = 15
	Public Const ACITaxAmount As Integer = 16
	Public Const ACIGracePeriod As Integer = 17
	Public Const ACIProductID As Integer = 18
	Public Const ACIAnnualPremium As Integer = 19
	Public Const ACILapsedDate As Integer = 20
	Public Const ACISourceDescription As Integer = 21
	Public Const ACISourceIsDeleted As Integer = 22
	Public Const ACISourceAllowTempMTA As Integer = 23
	Public Const ACISourceAllowPermMTA As Integer = 24
	Public Const ACIEventDescription As Integer = 28 'Gaurav
	'sj 20/09/2002 - end
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
	Public Const ACDateColumn As Integer = 4
	Public Const ACDateSortColumn As Integer = 6
	
	' Public source and language ID's from the
    ' Object Manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'eck220500
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

    ' Public instance of the business object.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oBusiness As Object
    'Public g_oBusiness As bSIRFindInsurance.Form
    'eck190500
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oPMUser As Object
    'Public do we have a link to Gemini
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_bPMGeminiLink As Boolean
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_bPMSwiftLink As Boolean
End Module