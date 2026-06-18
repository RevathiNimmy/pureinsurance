Option Strict Off
Option Explicit On
Imports System
Imports System.Collections
Imports System.IO
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
	' Date:  23/10/2000
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bSIRPFRF"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	

    Public Const ACMaxInstalments As Integer = 0

    Public Const ACFinanceNetCommission As Integer = 62
    Public Const ACSingleInstalmentPerMonth As Integer = 63
    Public Const ACFirstInstalmentAlignWithDayInMonth As Integer = 64

    Public Const ACParamsCount As Integer = ACFirstInstalmentAlignWithDayInMonth
 
	Sub Main_Renamed()
		
		
	End Sub
End Module