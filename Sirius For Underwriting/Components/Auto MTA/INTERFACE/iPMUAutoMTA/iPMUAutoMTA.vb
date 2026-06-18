Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 20/09/2000
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMUAutoMTA"
	
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
	
    ' Username.
    Public g_sUsername As New FixedLengthString(12)
	' Password.
	Public g_sPassword As New FixedLengthString(30)
    ' Calling Application
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sCallingAppName As String = ""
    ' Currency ID
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iCurrencyID As Integer
    ' LogLevel
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLogLevel As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	Public Const ACRiskPosCnt As Integer = 0
	
	Public Const ACIInsFileCnt As Integer = 0
	Public Const ACIRiskId As Integer = 1
	Public Const ACIRiskDescription As Integer = 2
	Public Const ACIRiskTypeDescription As Integer = 3
	Public Const ACIRiskInceptionDate As Integer = 4
	Public Const ACIRiskExpiryDate As Integer = 5
	' AM 061200 Add new column for risk status
	Public Const ACIRiskStatus As Integer = 6
	Public Const ACIRiskTotalSumInsured As Integer = 7
	Public Const ACIRiskTotalAnnualPremium As Integer = 8
	Public Const ACIRiskGisScreen As Integer = 9
	Public Const ACIRiskTypeId As Integer = 10
	Public Const ACIInsuranceFolderCnt As Integer = 11
	Public Const ACIRiskStatusFlag As Integer = 12
	' PW311002 - add new columns for Risk Variations / Quote management
	Public Const ACIRiskNo As Integer = 13
	Public Const ACIVariationNo As Integer = 14
	Public Const ACIIsSelected As Integer = 15
	Public Const ACICoverage As Integer = 16
	Public Const ACIInsuredItem As Integer = 17
	Public Const ACIExtensions As Integer = 18
	' PW221102 - add risk tax
	' PS411
	Public Const ACIRiskTax As Integer = 19
	Public Const ACIRiskFolderCnt As Integer = 24
	
	Sub Main_Renamed()
		
	End Sub
End Module