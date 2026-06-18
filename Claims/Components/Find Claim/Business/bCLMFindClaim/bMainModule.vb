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
	' Date:  14/07/2000
	'
	' Description: Main Module.
	'
	' Edit History:Pandu
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bFindClaim"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	' Constants for the search data array indexes.
	Public Const ACIInsFileId As Integer = 0
	Public Const ACIInsFileSourceId As Integer = 1
	Public Const ACIInsFileCnt As Integer = 2
	Public Const ACIInsReference As Integer = 3
	Public Const ACIInsFolderName As Integer = 4
	Public Const ACIInsFileType As Integer = 5
	Public Const ACIInsuredLongName As Integer = 6
	Public Const ACIInsuredShortName As Integer = 7
	Public Const ACIInsuredId As Integer = 8
	Public Const ACIInsuredSourceId As Integer = 9
	Public Const ACILastModified As Integer = 10
	Public Const ACIInsHolderCnt As Integer = 11
	Public Const ACIInsFolderCnt As Integer = 12
	Public Const ACIProductID As Integer = 13
	Public Const ACIProductCode As Integer = 14
	Public Const ACIProductName As Integer = 15
	Public Const ACILeadAgentCnt As Integer = 16
	Public Const ACIDateCreated As Integer = 17
	Public Const ACIRegistration As Integer = 18
	
	'Constants for Claim Status
	
	Public Const CLMProvisionalOpenClaim As Integer = 1
	Public Const CLMLiveOpenClaim As Integer = 2
	Public Const CLMClosed As Integer = 3
	Public Const CLMReOpened As Integer = 4
	Public Const CLMReClosed As Integer = 5
	
	
	'Date Formats
	Public Const ACDateConversion As String = "dd/mm/yyyy"
	Public Const ACDateDispaly As String = "dddd , mmmm d ,yyyy"
	Public Const ACShortDate As String = "short date"
	Public Const ACDateReverse As String = "yyyy/mm/dd hh:nn:ss"
End Module