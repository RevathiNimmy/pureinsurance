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
	' Date:  08-June-2000
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bCLMCoInsuranceRecoveries"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
    'RWH(30/08/01) Constants for coinsurer details.
	Public Const ACPartyCnt As Integer = 0
	Public Const ACName As Integer = 1
	Public Const ACSharePercent As Integer = 2
	Public Const ACShareValue As Integer = 3
	
	' Claim ID
	
	'Public Const m_lClaimID As Long = 14
	'Public Const m_lPartyID As Long = 1
	'Public Const m_iTask As Integer = PMAdd
	
	Sub Main_Renamed()
		
		
	End Sub
End Module