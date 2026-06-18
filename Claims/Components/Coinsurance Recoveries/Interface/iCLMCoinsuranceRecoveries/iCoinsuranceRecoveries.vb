Option Strict Off
Option Explicit On
Imports System
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
    'Devleoper Guide no. 50
    'developer guide no. 107
    <ThreadStatic()> _
    Public m_ofrmInterface As frmInterface
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iCLMCoinsuranceRecoveries"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' General Icons
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	Public Const ACShareTitle As Integer = 304
	Public Const ACShare As Integer = 305
	Public Const ACADDTitle As Integer = 306
	Public Const ACAdd As Integer = 307
	'Public Const ACEDITTitle = 308
	'Public Const ACEDIT = 309
	Public Const ACDeleteTitle As Integer = 308
	Public Const ACDelete As Integer = 309
	Public Const ACShareTotal As Integer = 310
	' Menus
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
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
	
    ' Public instance of the Business Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As Object
	
	' constants for Index values of the Buttons on the frmInterface
	Public Const g_cIOK As Integer = 0
	Public Const g_cICANCEL As Integer = 1
	Public Const g_cIHELP As Integer = 2
	Public Const g_cIADD As Integer = 3
	Public Const g_cIEDIT As Integer = 4
	Public Const g_cIDELETE As Integer = 5
	
	' constants for the Index values of the Text boxes in the frmDetails
	Public Const g_cISHARE_PERCENTAGE As Integer = 0
	Public Const g_cICURRENT_SHARE_VALUE As Integer = 1
	Public Const g_cINEW_SHARE_VALUE As Integer = 2
	
	' constants declared for the purpose of component testing, comment this part  for integration
	Public m_lInsuranceFileCnt As Integer
	Public m_lClaimID As Integer
	Public m_iTask As Integer
	Public m_sClaimNumber As String = ""
	
	'constants for captions
	Public Const ACDetailstabCaption As Integer = 102
	Public Const ACClaimNumber As Integer = 103
	Public Const ACCoinsuranceTreatment As Integer = 104
	Public Const ACTotalShare As Integer = 105
	Public Const ACTotalCurrentShareValue As Integer = 106
	Public Const ACTotalNewShareValue As Integer = 107
	Public Const ACPartyName As Integer = 108
	Public Const ACSharePercentgae As Integer = 109
	Public Const ACCurrentShareValue As Integer = 110
	Public Const ACNewShareValue As Integer = 111
	Public Const ACCaptionAdd As Integer = 112
	Public Const ACCaptionEdit As Integer = 113
	Public Const ACCaptionDelete As Integer = 114
	
    'RWH(05/03/2001) Flag to avoid adding extra retained record.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_bRetainedRecordAlreadyIncluded As Boolean
End Module