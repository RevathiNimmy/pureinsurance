Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 15/07/2000
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:SK
	' RKS 01/12/2005 PN25979 Adding IsExcess field
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iCLMResvDefn"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	Public Const ACAdd As Integer = 0 'Constant for ADD button index
	Public Const ACModify As Integer = 1 'Constant for MODIFY button index
	Public Const ACDelete As Integer = 2 'Constant for DELETE button index
	
	
	' General Icons
	'RESOURCE FILE CONSTANTS
	' Form
	Public Const ACInterfaceTitle As Integer = 100 'Reserve Defination
	Public Const ACTabTitle1 As Integer = 101 '&1-General
	'Public Const ACTabTitle2 = 102      '&2-Advanced
	
	Public Const AClblReserveType As Integer = 104 '&Reserve Type :
	Public Const AClblDesc As Integer = 105 '&Description :
	Public Const AClblncludeInTotal As Integer = 106 '&Include in Total :
	Public Const AClbIsExcess As Integer = 107 'Is Excess:
	'Public Const ACLossFromDate = 107 'Loss Date &Start Limit :
	'Public Const ACLossToDate = 108   'Loss Date &End Limit :
	'Public Const ACIncludeClosedClaims = 109 ' &Include Closed Claims :
	Public Const AClblIsIndemnity As Integer = 402 'Is Indemnity
	Public Const AClblIsExpense As Integer = 403 'Is Expense:
	
	Public Const ACListTitle1 As Integer = 109 'Reserve Type
	Public Const ACListTitle2 As Integer = 110 'Description
	Public Const ACListTitle3 As Integer = 111 'Include in Total
	Public Const ACListTitle4 As Integer = 112 'Is Excess
	Public Const ACListTitle5 As Integer = 404 'Is Indemnity
	Public Const ACListTitle6 As Integer = 405 'Is Expense
	''Date Formats
	'Public Const ACDateConversion = "dd/mm/yyyy"
	'Public Const ACDateDispaly = "dddd , mmmm d ,yyyy"
	'Public Const ACShortDate = "short date"
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACAddButton As Integer = 203 '&Add
	Public Const ACModButton As Integer = 204 '&Modify
	Public Const ACDelButton As Integer = 205 '&Delete
	
	' Messages
	Public Const ACResvTypeNameMsg1 As Integer = 307 'The reserve  type
	Public Const ACResvTypeNameMsg2 As Integer = 308 'already exists
	Public Const ACResvTypeNameTitle As Integer = 309 'Duplicate value
	
	
	
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACClearDetailsTitle As Integer = 304
	Public Const ACClearDetails As Integer = 305
	Public Const ACStatusSearching As Integer = 306
	
	Public Const ACInvalidDateMsg As Integer = 400 'Invalid Date Entered
	Public Const ACDateDiffError As Integer = 401 'Date Diff Error
	
	' Constants for the search data array indexes.
	Public Const ACIInsuranceid As Integer = 0
	Public Const ACIClaimCnt As Integer = 1
	Public Const ACIClaimType As Integer = 2
	Public Const ACIClaimRef As Integer = 3
	Public Const ACIInsuranceRef As Integer = 4
	
	
	
	
	' Constants for Underwriting
	Public Const ACIURiskIndex As Integer = 5
	Public Const ACIUProductCode As Integer = 6
	Public Const ACIULossDate As Integer = 7
	Public Const ACIUPolicyHolder As Integer = 8
	
	'Constants for Broking
	Public Const ACIBLossDate As Integer = 5
	Public Const ACIBPolicyHolder As Integer = 6
	
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
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Public instance of the business object.
    'Public g_oBackofficelink As bBackOfficeLink.bBOLink
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBackofficelink As Object
	' Public instance of the business object.

    'developer guide no. 
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oBusiness As Object
	
    'holds the Reserve Type ID globally
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lReserveTypeID As Integer
	
	
	Sub Main_Renamed()
		
		'Dim o As iCLMFindClaim.Interface
		'Dim lReturn As Long
		'
		'    Set o = New Interface
		'    lReturn = o.Initialise()
		'    lReturn = o.SetProcessModes(vTask:=PMView)
		'    lReturn = o.Start
		'    lReturn = o.Terminate
		'
		'    Set o = Nothing
	End Sub
End Module