Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 04/03/1997
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions to identify which application this is.
	Public Const ACApp As String = "iCLMReinsurance"
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	' Form
	Public Const ACRiskInterfaceTitle As Integer = 99
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACRiskInterfaceTitleInsurer As Integer = 117
	Public Const ACInterfaceTitleInsurer As Integer = 118
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACApplyButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACHelpButton As Integer = 204
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	Public Const ACDeleteDetailsTitle As Integer = 304
	Public Const ACDeleteDetails As Integer = 305
	
	'Broker participant
	Public Const ACIBrokerShortName As Integer = 0
	Public Const ACIBrokerLongName As Integer = 1
	Public Const ACIBrokerParticipant_percent As Integer = 2
	Public Const ACIBrokerAssociationPartyCnt As Integer = 3
	Public Const ACIBrokerPartyCnt As Integer = 4
	
	' Public source and language ID's from the Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
    ' If we are an underwriting agency then the wording differs
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_bIsUnderwritingAgency As Boolean
	
    ' Indicate that we are running in balance and close mode
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_bBalanceAndCloseClaim As Boolean
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_vIsRI2007 As String = ""
	
	
	Sub Main_Renamed()
		
	End Sub
End Module