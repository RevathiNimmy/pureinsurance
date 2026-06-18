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
	' Date:  27th September 1996
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bCashListPost"
	
	'EK 040200
	Public Const AsDebited As String = "0"
	Public Const ClientPayment As String = "1"
	Public Const InsurerSetted As String = "2"
	'SP080102 - Merge catch up
	'eck261001
	Public Const WhenEffective As String = "3"
	Public Const ClientPaymentincDID As String = "4" 'eck100203
	'eck301001
	Public Const MovePaidDirect As String = "666"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	
	
	
	
	' Log Level
	
	
	
	'***************
	' MEvans : 14-05-2003 : CQ 709
	' Approval Types
	Public Const ACApprovalTypeClaimPayment As Integer = 1
	'***************
	
	Sub Main_Renamed()
		
		' Main entry point for the component
		'Test
		
	End Sub
End Module