Option Strict Off
Option Explicit On

Imports System
'Developr Guide No: 129
Imports SharedFiles

Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 30/08/2000
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:Pandu
	' ***************************************************************** '

	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iCLMPeriltype"

	' General Icons
	'RESOURCE FILE CONSTANTS
	' Form
	Public Const ACInterfaceTitle As Integer = 100 'Reserve Defination
	Public Const ACTabTitle1 As Integer = 101 '&1-General
	
	Public Const ACListTitle1 As Integer = 109 'Risk Type
	Public Const ACListTitle2 As Integer = 110 'Description
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACDefineFields As Integer = 203 'Define &fields
	Public Const ACShowScreen As Integer = 204 ' &Show screen
	Public Const ACReserve As Integer = 206
	
	'CMG/PB 07112002 Loss Schedule
	Public Const ACInitialisationButton As Integer = 207
	Public Const ACValidateButton As Integer = 208
	Public Const ACFieldLevelButton As Integer = 209
	Public Const ACRowLevelButton As Integer = 210
	Public Const ACPaymentButton As Integer = 211
	
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACClearDetailsTitle As Integer = 304
	Public Const ACClearDetails As Integer = 305
	Public Const ACStatusSearching As Integer = 306
	
	'CMG/PB 07112002 307-310 are in the RES file but const seem to be missing
	Public Const ACRuleEditor As Integer = 311
	Public Const ACRuleEditorFail As Integer = 312
	'End CMG
	
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

    'developer guide no. 243
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oBusiness As Object
	
    'holds the Reserve Type ID globally
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_lRiskTypeID As Integer

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