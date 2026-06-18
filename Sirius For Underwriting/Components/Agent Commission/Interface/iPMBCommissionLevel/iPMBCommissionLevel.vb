Option Strict Off
Option Explicit On
Imports System
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


	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMBCommissionLevel"


	' Public interface constants used when
	' retrieving data from the resource file.

	' {* USER DEFINED CODE (Begin) *}

	' General Icons

	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACTabTitle2 As Integer = 102
	' PW260702
	Public Const ACInterfaceTitleAgent As Integer = 103
	Public Const ACSearchButtonAgent As Integer = 104

	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACAddButton As Integer = 204
    Public Const ACEditButton As Integer = 205
    Public Const ACDeleteButton As Integer = 206
    Public Const ACUndeleteButton As Integer = 207


    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACIncorrectEffectiveDate As Integer = 302
	Public Const ACNullCommissionLevel As Integer = 309
	Public Const ACBusinessFail As Integer = 303
	Public Const ACStatusSearching As Integer = 306
	Public Const ACStatusFound As Integer = 307
	Public Const ACCommTrans As Integer = 308

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
	'Public Const ACIPartyAgent As Integer = 0
	'Public Const ACIPartyAgentShortname As Integer = 1
	Public Const ACICommissionLevelID As Integer = 0
	Public Const ACICommissionLevelDescription As Integer = 1
	Public Const ACIEffectiveDate As Integer = 2
	Public Const ACIIsDeleted As Integer = 3
    Public Const ACIAgent_Commission_Level_Id As Integer = 4
    Public Const ACRItemStatus As Integer = 5



    ' PW260702 - constant for party type of agent
    Public Const ACPartyAgent As String = "AG"

	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer

	' Public instance of the object manager.
	'developer guide no. 107
	<ThreadStatic()>
	Public g_oObjectManager As bObjectManager.ObjectManager
	'developer guide no. 107
	<ThreadStatic()>
	Public g_oGis As Object
	Public Const ScreenHelpID As Integer = 4093
	Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions



	Sub Main_Renamed()

	End Sub
End Module