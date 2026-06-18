Option Strict Off
Option Explicit On
Imports System
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 02nd October 2002
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "DummyInterface" 'Todo;
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	' General Icons
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	
	' Buttons
	Public Const ACOKCaption As Integer = 200
	Public Const ACCancelCaption As Integer = 201
	Public Const ACHelpCaption As Integer = 202
	Public Const ACNavigateCaption As Integer = 203
	Public Const ACAddCaption As Integer = 204
	Public Const ACDeleteCaption As Integer = 205
	Public Const ACEditCaption As Integer = 206
	
	' Menus
	
	'Form Components
	Public Const ACMainTabTitle0 As Integer = 300
	Public Const ACDummyLabel1 As Integer = 301
	Public Const ACDummyLabel2 As Integer = 302
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 700
	Public Const ACCancelDetails As Integer = 701
	Public Const ACBusinessFailTitle As Integer = 702
	Public Const ACBusinessFail As Integer = 703
	Public Const ACShortNameErrorTitle As Integer = 704
	Public Const ACShortNameError As Integer = 705
	Public Const ACFailedValidationTitle As Integer = 706
	Public Const ACFailedValidationError As Integer = 707
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	

	Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFOrion
	Public Const ScreenHelpID As Integer = 16000 'Todo;
	Public Const ScreenHelpID2 As Integer = 37000 'Todo;
	
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
 Public g_sUserName As String = ""
	
    ' Extra variables for component services
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sPassword As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iCurrencyID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLogLevel As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	Private m_oSystemOption As Object
	Private m_lReturn As Integer
	
	'User defined constants for Transaction Export Array
	Public Const ACCnt As Integer = 0
	Public Const ACClientCode As Integer = 1
	Public Const ACAgentCode As Integer = 2
	Public Const ACPolicyNumber As Integer = 3
	Public Const ACCoverStartDate As Integer = 4
	Public Const ACOperator As Integer = 5
	Public Const ACGrossAmount As Integer = 6
	Public Const ACCommission As Integer = 7
	Public Const ACTax As Integer = 8
	Public Const ACCurrency As Integer = 9
	Public Const ACTransactionType As Integer = 10
	Public Const ACExportStatus As Integer = 11
	
	

	Public Sub Main()
		
	End Sub

	Sub New()
		Main()
	End Sub
	Sub JustForInvokeMain()
	End Sub
End Module