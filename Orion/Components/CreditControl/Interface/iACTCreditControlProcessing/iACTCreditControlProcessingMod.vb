Option Strict Off
Option Explicit On
Imports System
' Developer Guide No. 129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 03/26/2003
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions to identify which application this is.
	Public Const ACApp As String = "iACTCreditControlProcessing"
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	Public g_sUserName As String = ""
	
	' Extra variables for component services
	Public g_sPassword As String = ""
	Public g_iUserID As Integer
	Public g_iCurrencyID As Integer
	Public g_iLogLevel As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
    ' Instance of SolutionConfig
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oSirConfig As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSolutionConfig As Integer
	
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFOrion
	Public Const ScreenHelpID As Integer = 16000
	Public Const ScreenHelpID2 As Integer = 37000
	
    Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	
	Public Const ACListTitle1 As Integer = 102
	Public Const ACListTitle2 As Integer = 103
	Public Const ACListTitle3 As Integer = 104
	Public Const ACListTitle4 As Integer = 105
	Public Const ACListTitle5 As Integer = 106
	'eck250800
	Public Const ACTotalLabel As Integer = 107
	
	' Details Form
	Public Const ACDetailsInterfaceTitle As Integer = 120
	Public Const ACDetailsTabTitle1 As Integer = 121
	
	' CR/DR drop down
	Public Const ACDetailsCR As Integer = 130
	Public Const ACDetailsDR As Integer = 131
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACAddButton As Integer = 204
	Public Const ACRemoveButton As Integer = 205
	Public Const ACEditButton As Integer = 206
	
	' START CHANGES - Changed By: AAB  - Changed On: 18-Feb-2004 11:12
	'Labels
	Public Const ACAsOfLabel As Integer = 207
	Public Const ACBranchLabel As Integer = 208
	Public Const ACSpoolCheckBox As Integer = 209
	Public Const ACArchiveCheckBox As Integer = 210
	' END CHANGES - Changed By: AAB  - Changed On: 18-Feb-2004 11:12
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	' Validation
	Public Const ACFailedValidationTitle As Integer = 306
	Public Const ACFailedValidationError As Integer = 307
	
	'Other Controls
	Public Const ACMainFrame As Integer = 401
	
	'Error Message for bACTFinanceSpoke
	Public Const ACFailedSpokeTitle As Integer = 308
	Public Const ACFailedSpokeError As Integer = 309
	Public Const ACFailedNoRecords As Integer = 312
	
	'Status Bar messages
	Public Const ACStatusReady As Integer = 310
	Public Const ACStatusProcessing As Integer = 311
	
	Public Const ACSpokeInterfaceCode As String = "CREDITCONTROL"
	Public Const ACSpokeStatusCode As String = "A"
	Public Const ACSpokeMessage As String = "A"
	Public Const ACSpokeHeaderXML As String = "<XML>"
	Public Const ACSpokeDetailData As String = ""
	Public Const ACSpokeBatch As String = ""
End Module