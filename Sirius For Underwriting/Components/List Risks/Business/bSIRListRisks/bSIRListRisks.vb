Option Strict Off
Option Explicit On
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
	' Date:  30/10/2002
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bSIRListRisks"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"


	Public Const kInsFileDiscountPercentage As Integer = 18
	Public Const kInsFileMatchProductId As Integer = 19
	Public Const kInsFileDiscountedPremium As Integer = 20
	Public Const kInsFileMatchDiscountedPremium As Integer = 21


	Public Const kInsFileTotalPremium As Integer = 0
	
	Public Const kRiskIsNotDiscounted As Integer = 0
	Public Const kRiskIsDiscounted As Integer = 1
	
	Public Const kPolicyDiscountTransactionTypeId As Integer = 0
	Public Const kThisPolicy As Integer = 0
	
	Public Const kPolicyDiscountStatusAllowRollback As Integer = 1
	Public Const kRiskStatusCodeUNQUOTED As String = "UNQUOTED"
    Public Const kUseExistingFeeDetails As Integer = 1

    'WPR 33-75 added
    'Array constants for the Risk
    Public Const ACRRiskId As Integer = 0
    Public Const ACRRiskStatusId As Integer = 1
    Public Const ACRRiskFolderCnt As Integer = 2

    'Array constants for the Risk Folder
    Public Const ACRFRiskFolderCnt As Integer = 0
    Public Const ACRFRiskFolderId As Integer = 1
    Public Const ACRFSourceId As Integer = 2
    Public Const ACRFRiskFolderTypeId As Integer = 3
    Public Const ACRFCode As Integer = 4
    Public Const ACRFDescription As Integer = 5
    Public Const ACRFInsuranceFolderCnt As Integer = 6
    'WPR 33-75 Ends
	
	
	Sub Main_Renamed()
		
		
	End Sub
End Module