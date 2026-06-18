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
	' Module Name: MainModule
	'
	' Date:  02/02/2000
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bSIRRiskGroup"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Username.
	'Public g_sUsername As String * 12
	
	' Password.
	'Public g_sPassword As String * 30
	
	' User ID
	'Public g_iUserID As Integer
	
	' Calling Application
	'Public g_sCallingAppName As String
	' Source ID
	'Public g_iSourceID As Integer
	' Language ID
	'Public g_iLanguageID As Integer
	' Currency ID
	'Public g_iCurrencyID As Integer
	' LogLevel
	'Public g_iLogLevel As Integer
	
	'CT 02/11/00 Added Constants for the risk group table array
	Public Const ACArrayRiskGroupID As Integer = 0
	Public Const ACArrayCaptionID As Integer = 1
	Public Const ACArrayCode As Integer = 2
	Public Const ACArrayDescription As Integer = 3
	Public Const ACArrayIsDeleted As Integer = 4
	Public Const ACArrayEffectiveDate As Integer = 5
	Public Const ACArrayRiskScreenType As Integer = 6
	Public Const ACArrayABICode As Integer = 7
	' CTAF 050402 ->
	Public Const ACArrayPQRiskScreenType As Integer = 8
	Public Const ACArrayFSAProduct As Integer = 9 'FSA Phase IV
	Public Const ACArrayMidnightRenewal As Integer = 10 '2005 - Midnight Renewal
	Public Const ACArrayCountryId As Integer = 11 'Datasure
	Public Const ACArrayBrokerlinkPolicyTypeId As Integer = 12 'Datasure - Brokerlink
	Public Const ACArrayMax As Integer = 12
	' <- CTAF 050402
	
	
	
	Sub Main_Renamed()
		
		
	End Sub
End Module