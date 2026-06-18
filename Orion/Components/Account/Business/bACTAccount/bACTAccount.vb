Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  23-07-1997
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bACTAccount"
	
	'SD 09/01/2003
	Public Const ACCreditControlOptionNo As String = "4"
	Public Const ACValueWhenCreditControlSet As String = "1"
	Public Const ACAccountActiveID As Integer = 1
	Public Const ACAccountClosedID As Integer = 2
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	'System Options
	Public Const ACCurrencyDifferenceCrebitAccount As Integer = 150
	Public Const ACCurrencyDifferenceDebitAccount As Integer = 151
	
	Public Const ACClientBankAccTypeArrPos As Integer = 0
	Public Const ACMerchantIdArrPos As Integer = 1
	Public Const ACHLastArrayPosition As Integer = 1
	
	' BaseCurrency
	'***********************************************
	' TO DO: GLOBAL VARIABLE IN BAS MODULE
	'Public g_iBaseCurrencyId As Long
	'***********************************************
	
	'***********************************************
	' TO DO: GLOBAL VARIABLE IN BAS MODULE
	'Public g_iCompanyID As Integer
	'***********************************************
	'eck110400
	'MultiBranch
	'***********************************************
	' TO DO: GLOBAL VARIABLE IN BAS MODULE
	'Public g_iPartySourceId As Integer
	'***********************************************
	
	Sub Main_Renamed()
		
	End Sub
End Module