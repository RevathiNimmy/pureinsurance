Option Strict Off
Option Explicit On
Imports System
'developer guide no. 129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 25/07/1997
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iACTCurrencyRate"
	
	
	'Form Captions
	Public Const ACFormCaption1 As Integer = 100
	Public Const ACFormCaption2Each As Integer = 101
	Public Const ACFormCaption2All As Integer = 102
	Public Const ACFilterFrameCaption As Integer = 103
	Public Const ACBranchCaption As Integer = 104
	Public Const ACEffectiveDateCaption As Integer = 105
	
	'Button Captions
	Public Const ACOKCaption As Integer = 200
	Public Const ACApplyCaption As Integer = 201
	Public Const ACCancelCaption As Integer = 202
	
	'Message Captions
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	Public Const ACMissingRatesTitle As Integer = 304
	Public Const ACMissingRates As Integer = 305
	Public Const ACReplaceRatesTitle As Integer = 306
	Public Const ACReplaceRates As Integer = 307
	Public Const ACCreateRatesTitle As Integer = 308
	Public Const ACCreateRates As Integer = 309
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	Public g_iCompanyID As Integer
    ' Public CompanyCurrencyId which the instance is based on
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lCompanyCurrencyId As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sISOCode As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sCurrencyDescription As String = ""
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	Public g_iCurrencyID As Integer
	'Product Family Name for Help

	Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFOrion
	
	Public Const ScreenHelpID As Integer = 33000
	
	

	Public Sub Main()
		
	End Sub

	Sub New()
		Main()
	End Sub
	Sub JustForInvokeMain()
    End Sub
End Module