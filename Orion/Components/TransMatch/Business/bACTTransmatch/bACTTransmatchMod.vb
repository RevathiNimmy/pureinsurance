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
	' Date:  04/10/1997
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bACTTransmatch"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	
	
	'***********************************************
	' TO DO: GLOBAL VARIABLE IN BAS MODULE
	'' Public const for Variant Array passsed to AddMatchSet
	'***********************************************
	Public Const ACAllocationdetailID As Integer = 0
	Public Const ACTransdetailID As Integer = 1
	Public Const ACCurrencyID As Integer = 2
	Public Const ACBaseMatchAmount As Integer = 3
	Public Const ACCurrencyMatchAmount As Integer = 4
	Public Const ACCurrencyMatchXrate As Integer = 5
	

	Public Sub Main()
		
		
	End Sub
    Sub New()
        Main()
    End Sub
	Sub JustForInvokeMain()
	End Sub
End Module