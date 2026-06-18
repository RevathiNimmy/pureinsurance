Option Strict Off
Option Explicit On
Imports System
'developer guide no.129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 09/06/1999
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
    'developer guide no.50
    'developer guide no. 107
    <ThreadStatic()> _
    Public m_ofrmInterface As frmInterface
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iCLMCheckUnpaidPremium"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form Labels
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACPolicyNumber As Integer = 101
	Public Const ACClaimNumber As Integer = 102
	Public Const ACClaimDate As Integer = 103
	Public Const ACClient As Integer = 104
	Public Const ACOutstandingOnly As Integer = 105
	
	' Form list view headers - Transactions
	Public Const ACTBranch As Integer = 150
	Public Const ACTAccount As Integer = 151
	Public Const ACTDocRef As Integer = 152
	Public Const ACTransDate As Integer = 153
	Public Const ACTAmount As Integer = 154
	Public Const ACTOSAmount As Integer = 155
	Public Const ACTDocType As Integer = 156
	
	' Form list view headers - Instalments
	Public Const ACIBranch As Integer = 170
	Public Const ACIAccount As Integer = 171
	Public Const ACIDocRef As Integer = 172
	Public Const ACIInstalNo As Integer = 173
	Public Const ACIInstalAmount As Integer = 174
	Public Const ACIDueDate As Integer = 175
	Public Const ACITransCode As Integer = 176
	Public Const ACIStatus As Integer = 177
	Public Const ACIPostedDate As Integer = 178
	
	' Buttons
	Public Const ACContinueButton As Integer = 200
	Public Const ACAbortButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	
	' Messages
	Public Const ACBusinessFailTitle As Integer = 300
	Public Const ACBusinessFailMsg As Integer = 301
	Public Const ACAbortTitle As Integer = 302
	Public Const ACAbortMsg As Integer = 303
	
	' Menus
	
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Constants for the search data array indexes.
	Public Const ACAPaymentType As Integer = 0
	Public Const ACAPaidUpToDate As Integer = 1
	Public Const ACAPolicyNum As Integer = 2
	Public Const ACAClaimDate As Integer = 3
	Public Const ACAInstalmentDebt As Integer = 4
	
	Public Const ACATBranch As Integer = 0
	Public Const ACATAccount As Integer = 1
	Public Const ACATDocRef As Integer = 2
	Public Const ACATTransDate As Integer = 3
	Public Const ACATAmount As Integer = 4
	Public Const ACATOSAmount As Integer = 5
	Public Const ACATDocType As Integer = 6
	
	Public Const ACAIBranch As Integer = 0
	Public Const ACAIAccount As Integer = 1
	Public Const ACAIDocRef As Integer = 2
	Public Const ACAIInstalmentNo As Integer = 3
	Public Const ACAIInstalmentAmount As Integer = 4
	Public Const ACAIDueDate As Integer = 5
	Public Const ACAITransactionCode As Integer = 6
	Public Const ACAIStatus As Integer = 7
	Public Const ACAIPostedDate As Integer = 8
	
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
 Public g_nPMMode As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lClaimID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sClaimNo As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sPolicyNo As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lPolicyID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_vClaimDate As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lRiskTypeID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sClientName As String = ""
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' KB 100801 required for help text, screenhelpid will need to be amended
	' when text available
	Public Const ScreenHelpID As Integer = 4001

	Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
	Sub Main_Renamed()
		
	End Sub
End Module