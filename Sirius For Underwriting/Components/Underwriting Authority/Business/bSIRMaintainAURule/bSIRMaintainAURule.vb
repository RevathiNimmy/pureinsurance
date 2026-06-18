Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
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
	' Date:  07/05/1999
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bSIRMaintainScreenData"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	'RWH(04/01/2001) Constants for Rule Set Types.
	'Used in conjunction with Rule_Set table.
	Public Const ACRuleSetTypeRating As Integer = 1
	Public Const ACRuleSetTypeUnderwritingAuthority As Integer = 2
	
	'Array constants for Rule Links.
	Public Const ACRuleFieldMaxIndex As Integer = 12
	Public Const ACUARuleId As Integer = 0
	Public Const ACUARuleCaptionId As Integer = 1
	Public Const ACUARuleCode As Integer = 2
	Public Const ACUARuleDescription As Integer = 3
	Public Const ACUARuleEffectiveDate As Integer = 4
	Public Const ACUARuleFileName As Integer = 5
	Public Const ACUARuleLive As Integer = 6
	Public Const ACUARuleTypeId As Integer = 7
	Public Const ACUARuleIsUnderwriter As Integer = 8
	Public Const ACUARuleProductId As Integer = 9
	Public Const ACUARuleTransTypeId As Integer = 10
	Public Const ACUARuleAuthTypeDescription As Integer = 11
	Public Const ACUARuleProductDescription As Integer = 12
	
	
	
	
	
	Sub Main_Renamed()
		
		
	End Sub
End Module