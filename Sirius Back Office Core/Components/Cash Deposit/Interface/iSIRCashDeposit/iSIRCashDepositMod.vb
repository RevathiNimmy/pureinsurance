Option Strict Off
Option Explicit On
Imports System
'developer guide no. 129
Imports SharedFiles
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
	Public Const ACApp As String = "iSIRCashDeposit"
	
	' String Resources
	'DC130202
	'Public Const AC_RES_INSURER = 121
	'Public Const AC_RES_ACCOUNTEXEC = 122
	'Public Const AC_RES_CLIENTNAME = 123
	'Public Const AC_RES_VEHICLEREG = 124
	'
	'Public Const AC_RES_LISTTITLE11 = 125
	'Public Const AC_RES_LISTTITLE12 = 126
	'Public Const AC_RES_LISTTITLE13 = 127
	
	'Date Formats
	Public Const ACDateConversion As String = "dd/mm/yyyy"
	Public Const ACDateDispaly As String = "long date"
	Public Const ACShortDate As String = "short date"
	
	
	' Buttons
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACFrameName As Integer = 202
	Public Const ACPartyCode As Integer = 203
	Public Const ACPartyName As Integer = 204
	Public Const ACAdd As Integer = 205
	Public Const ACEdit As Integer = 206
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACClearDetailsTitle As Integer = 302
	Public Const ACClearDetails As Integer = 303
	Public Const ACStatusSearching As Integer = 304
	Public Const ACStatusFound As Integer = 305
	Public Const ACAllTypes As Integer = 306
	Public Const ACBusinessFailTitle As Integer = 307
	Public Const ACBusinessFail As Integer = 308
	
	'Constants for UnderWriting and Broking
	Public Const ACBroking As String = "A"
	Public Const ACUnderWriting As String = "U"
	
	' Constant for the maxiumum search details.
	Public Const ACMaxSearchDetails As Integer = 500
	
	' Constant for the miniumum search length.
	Public Const ACMinSearchLength As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Public instance of the business object.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As bSIRFindBankGuarantee.Business
	
    ' MKW 190503 PN2032 START
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oPMUser As Object
	
	Private m_lReturn As Integer
	' MKW 190503 PN2032 END
	
	
	'**********************************************
	Public Enum ENPMLookups
		Id = 0
		Description = 1
		uboundeNPMLookups = ENPMLookups.Description
	End Enum
	
	
	Sub Main_Renamed()
    End Sub


End Module