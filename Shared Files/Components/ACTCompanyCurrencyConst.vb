Option Strict Off
Option Explicit On
Imports System
Public Module CompanyCurrencyConst
	' ***************************************************************** '
	'
	' PMAccounts application general contants module. Contains all of
	' the global that may have a multi-component span, within Orion.
	'
	' ***************************************************************** '
	
	
	' ***************************************************************** '
	' Constants
	'
	' The constants below are grouped by originating application:
	'
	'
	' CompanyCurrency
	' ***************
	'
	Public Const ACTGetCurrenciesInCompany As Integer = 1
	Public Const ACTGetAllCurrencies As Integer = 2
	Public Const ACTGetCurrenciesNotInCompany As Integer = 3
	Public Const ACTGetBaseCurrencies As Integer = 4
	' Field positions within data variant arrays
	Public Const ACTCurrencyId As Integer = 0
	Public Const ACTCurrencyISOCode As Integer = 1
	Public Const ACTCurrencyDescription As Integer = 2
	Public Const ACTCompanyCurrencyId As Integer = 3
	Public Const ACTToInsert As Integer = 4 'returned only
    'Modified by Deepak Sharma on 4/20/2010 4:41:30 PM refer developer guide no. 29(No Solutions)
    'Shared Sub New()
    '	MainModule.JustForInvokeMain()

End Module