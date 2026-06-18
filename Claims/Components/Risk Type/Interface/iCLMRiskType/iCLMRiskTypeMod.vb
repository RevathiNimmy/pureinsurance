Option Strict Off
Option Explicit On
Imports System
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
	Public Const ACApp As String = "iCLMRisktype"
	
	
	
	' General Icons
	'RESOURCE FILE CONSTANTS
	' Form
	Public Const ACInterfaceTitle As Integer = 100 'Reserve Defination
	Public Const ACTabTitle1 As Integer = 101 '&1-General
	
	Public Const ACListTitle1 As Integer = 109 'Risk Type
	Public Const ACListTitle2 As Integer = 110 'Description
	' Alix
	Public Const ACListTitle3 As Integer = 402 'Selected Screen
	Public Const ACScreenLabel As Integer = 403
	Public Const ACScreenForm As Integer = 404
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACDefineFields As Integer = 203 'Define &fields
	Public Const ACShowScreen As Integer = 204 ' &Show screen
	
	
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACClearDetailsTitle As Integer = 304
	Public Const ACClearDetails As Integer = 305
	Public Const ACStatusSearching As Integer = 306
	
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
 Public g_oBackofficelink As bBackOfficeLink.bBOLink
	' Public instance of the business object.

    'developer guide no. to do list
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