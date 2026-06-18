Option Strict Off
Option Explicit On
Imports System
Module Main
	
	Public Const ACApp As String = "uctCLMPerilRTControl"
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	Public g_iUserId As Integer
	
    'Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As Object
	
	Public Const ACFormatforNumber As String = "0.00"
	
	'-------------------------------------------------------
	'           Constants for Resource File items
	'-------------------------------------------------------
	' Business failure messages
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACAddPerilTitle As Integer = 305
	Public Const ACAddPerilDetails As Integer = 306
	
	'Delete Peril
	Public Const ACDeletePerilTitle As Integer = 307
	Public Const ACDeletePerilDetails As Integer = 308
	
	'Column Headers
	Public Const ACClaimType As Integer = 212
	Public Const ACRiskDescription As Integer = 213
	Public Const ACPerilDescription As Integer = 214
	Public Const ACSumInsured As Integer = 215
	Public Const ACCurrentReserve As Integer = 216
	
	'Peril data manipulation buttons
	Public Const ACAdd As Integer = 217
	Public Const ACEdit As Integer = 218
	Public Const ACDelete As Integer = 219
	
	'PN 15887 JT 21-10-2004 -Changed the Const value since MKR added 325,326
	'PN13417 JT 21-09-2004
	'Const for Column Headers
	Public Const ACPolicyCurrency As Integer = 232
	Public Const AClossCurrency As Integer = 233
	
	
	Public Const kColHeaderRiskDescription As Integer = 1
	Public Const kolHeaderPerilDescription As Integer = 2
	Public Const kColHeaderSumInsured As Integer = 3
	Public Const kColHeaderIncurred As Integer = 4
	Public Const kColHeaderPaid As Integer = 5
	Public Const kColHeaderRecoveries As Integer = 6
	Public Const kColHeaderSalvage As Integer = 7
	Public Const kColHeaderCurrentReserve As Integer = 8
	Public Const kColHeader9 As Integer = 9
	Public Const kColHeader10 As Integer = 10
	Public Const kColHeader11 As Integer = 11
	Public Const kColHeader12 As Integer = 12
	Public Const kColHeader13 As Integer = 13
	Public Const kColHeaderPolicyCurrency As Integer = 14
	Public Const kColHeaderLossCurrency As Integer = 15
	
	Public Const kSubItemsPerilDescription As Integer = 1
	Public Const kSubItemsSumInsured As Integer = 2
	Public Const kSubItemsIncurred As Integer = 3
	Public Const kSubItemsPaid As Integer = 4
	Public Const kSubItemsRecoveries As Integer = 5
	Public Const kSubItemsSalvage As Integer = 6
	Public Const kSubItemsCurrentReserve As Integer = 7
	Public Const kSubItemsAddMode As Integer = 8
	Public Const kSubItemsGisScreen As Integer = 9
	Public Const kSubItemsOriginalClaimPerilId As Integer = 10
	Public Const kSubItemsLossScheduleTypeId As Integer = 11
	Public Const kSubItemsPerilTypeId As Integer = 12
	Public Const kSubItemsPolicyCurrency As Integer = 13
	Public Const kSubItemsLossCurrency As Integer = 14
	Public Const kSubItemsPerilTypeId_BR As Integer = 9
End Module