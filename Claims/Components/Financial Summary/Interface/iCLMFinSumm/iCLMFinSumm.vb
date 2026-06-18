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
	' Edit History:Pandu
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iCLMFinSumm"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	
	' General Icons
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100 'Financial Details
	Public Const ACTabTitle1 As Integer = 101 '&1-Total
	Public Const ACTabTitle2 As Integer = 102 '&2-Advanced
	
	Public Const ACClaimRef As Integer = 103 '&Claim Reference :
	Public Const ACPolicyRef As Integer = 104 '&Policy :
	Public Const ACPolicyHolder As Integer = 105 'C&lient :
	Public Const ACRegistrationNumber As Integer = 106 '&Registration Number :
	Public Const ACLossFromDate As Integer = 107 'Loss Date &Start Limit :
	Public Const ACLossToDate As Integer = 108 'Loss Date &End Limit :
	Public Const ACIncludeClosedClaims As Integer = 109 ' &Include Closed Claims :
	
	Public Const ACListTitle1 As Integer = 109 'Description
	Public Const ACListTitle2 As Integer = 110 'Initial Reserve
	Public Const ACListTitle3 As Integer = 111 'Paid to date
	Public Const ACListTitle4 As Integer = 112 'Revised Reserve
	Public Const ACListTitle5 As Integer = 113 'Current Reserve
	Public Const ACListTitle6 As Integer = 114 'Sum Insured
	Public Const ACListTitle7 As Integer = 115 'Average(%)
	
	Public Const ACListTitle8 As Integer = 203 'Recieved To Date
	
	Public Const ACTabTitleSal As Integer = 204 'Salvage
	Public Const ACTabTitleRec As Integer = 205 'TP Recovery
	
	'To add new Payment Tab 'Vivek and Gaurav
	Public Const ACTabTitlePay As Integer = 402 'Payment
	
	'Start(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
	Public Const ACTabTitleReceipt As Integer = 403 'Receipt
	'End(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
	
	'Date Formats
	Public Const ACDateConversion As String = "dd/mm/yyyy"
	Public Const ACDateDispaly As String = "dddd , mmmm d ,yyyy"
	Public Const ACShortDate As String = "short date"
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	
	Public Const ACFindNowButton As Integer = 204
	Public Const ACNewSearchButton As Integer = 205
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACClearDetailsTitle As Integer = 304
	Public Const ACClearDetails As Integer = 305
	Public Const ACStatusSearching As Integer = 306
	Public Const ACStatusFound As Integer = 307
	
	Public Const ACInvalidDateMsg As Integer = 400 'Invalid Date Entered
	Public Const ACDateDiffError As Integer = 401 'Date Diff Error
	
	
	Public Const ACLookupFailTitle As Integer = 308
	Public Const ACLookupFail As Integer = 309
	
	
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
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As bCLMFinSumm.Business
	
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