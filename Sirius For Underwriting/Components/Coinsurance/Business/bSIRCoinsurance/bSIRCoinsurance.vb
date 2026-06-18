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
	' Date:  09/06/1999
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bSIRCoinsurance"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	
	
	
	Public Const ACAInsuranceFileCnt As Integer = 0
	Public Const ACAIsRecovered As Integer = 1
	Public Const ACAIsSurcharged As Integer = 2
	Public Const ACACOIDefaultId As Integer = 3
	
	Public Const ACVInsuranceFileCnt As Integer = 0
	Public Const ACVCOIValueId As Integer = 1
	Public Const ACVPartyCnt As Integer = 2
	Public Const ACVArrangementRef As Integer = 3
	Public Const ACVSharePercent As Integer = 4
	Public Const ACVSharePremium As Integer = 5
	Public Const ACVCommissionPercent As Integer = 6
	Public Const ACVCommissionValue As Integer = 7
	Public Const ACVSurchargePercent As Integer = 8
	Public Const ACVSurchargeValue As Integer = 9
	Public Const ACVIsStandardSurcharge As Integer = 10
	Public Const ACVPremiumTaxRecoveryPercent As Integer = 11
	Public Const ACVPremiumTaxRecoveryValue As Integer = 12
	Public Const ACVIsManualPremiumTax As Integer = 13
	
	Sub Main_Renamed()
		
		
	End Sub
End Module