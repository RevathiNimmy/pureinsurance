Option Strict Off
Option Explicit On
Imports System
Module MainModule
	
	Public Const ACApp As String = "bPMBMaintainRelation"
	Private Const ACClass As String = "MainModule"
	
	' Username.
	'Public g_sUsername As String * 12
	
	' Password.
	'Public g_sPassword As String * 30
	
	' User ID
	'Public g_iUserID As Integer
	
	' Calling Application
	'Public g_sCallingAppName As String
	' Source ID
	'Public g_iSourceID As Integer
	' Language ID
	'Public g_iLanguageID As Integer
	' Currency ID
	'Public g_iCurrencyID As Integer
	' LogLevel
	'Public g_iLogLevel As Integer
	
	Public Const ACArrayRelationShipTypeID As Integer = 0
	Public Const ACArrayCaptionID As Integer = 1
	Public Const ACArrayCode As Integer = 2
	Public Const ACArrayDescription As Integer = 3
	Public Const ACArrayIsDeleted As Integer = 4
	Public Const ACArrayEffectiveDate As Integer = 5
	Public Const ACArrayComplementaryTypeID As Integer = 6
	Public Const ACArrayPartyRelationshipGroupID As Integer = 7
End Module