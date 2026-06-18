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
	Public Const ACApp As String = "bSIRRIModelUsage"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	Public Const ACRProductId As Integer = 0
	Public Const ACRRiskTypeId As Integer = 0
	Public Const ACRRIModelBand As Integer = 1
	Public Const ACRRIModelId As Integer = 2
	Public Const ACRDescription As Integer = 3
	Public Const ACRRIModelDescription As Integer = 4
	Public Const ACRIsDeleted As Integer = 5
	Public Const ACREffectiveDate As Integer = 6
	Public Const ACRExpiryDate As Integer = 7
	Public Const ACRPortfolioRIModelId As Integer = 8
	Public Const ACRRiskTypeRIModelUsageCnt As Integer = 9
	Public Const ACRItemStatus As Integer = 10
	Public Const ACRRIBandDescription As Integer = 11
	Public Const ACRMax As Integer = 11
	
	Public Const ACItemStatus_Unchanged As Integer = 0
	Public Const ACItemStatus_Changed As Integer = 1
	Public Const ACItemStatus_Added As Integer = 2
	Public Const ACItemStatus_Deleted As Integer = 3
End Module