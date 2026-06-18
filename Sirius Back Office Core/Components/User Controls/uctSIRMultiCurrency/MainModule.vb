Option Strict Off
Option Explicit On
Imports System
Module MainModule
	
	Public Const ACApp As String = "uctSIRMultiCurrency"
	
    'Public source and language ID's from the Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iCurrencyID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserId As Integer
	
    'Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	'Caption constants
	Public Const ACfraTransaction As Integer = 100
	Public Const AClblTransactionValue As Integer = 101
	Public Const AClblTransactionCurrency As Integer = 102
	Public Const AClblEffectiveDate As Integer = 103
	Public Const ACfraBaseCurrency As Integer = 104
	Public Const AClblBaseCurrency As Integer = 105
	Public Const AClblBaseCurrencyRate As Integer = 106
	Public Const AClblBaseCurrencyAmount As Integer = 107
	Public Const ACfraAccountCurrency As Integer = 108
	Public Const AClblAccountCurrency As Integer = 109
	Public Const AClblAccountCurrencyRate As Integer = 110
	Public Const AClblAccountCurrencyAmount As Integer = 111
	Public Const ACfraSystemCurrency As Integer = 112
	Public Const AClblSystemCurrency As Integer = 113
	Public Const AClblSystemCurrencyRate As Integer = 114
	Public Const AClblSystemCurrencyAmount As Integer = 115
	Public Const ACfraRateOverrideReason As Integer = 116
	Public Const AClblReason As Integer = 117
End Module