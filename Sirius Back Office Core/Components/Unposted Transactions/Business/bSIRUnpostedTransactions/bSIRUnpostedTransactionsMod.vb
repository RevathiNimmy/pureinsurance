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
	' Date:  02nd October 2002
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bXXXComponentName"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	
    'developer guide no.39
    Public Const ACGetTranExportDetailsSQL As String = "spu_ACT_Get_Transaction_Export_Details"
	Public Const ACGetTranExportDetailsName As String = "GetTransactionExportDetails"
    Public Const ACUpdateAccountsExportStatusSQL As String = "spu_ACT_Update_Accounts_Export_Status"

	Public Const ACUpdateAccountsExportStatusName As String = "UpdateAccountsExportStatus"
	
	
	'User defined constants for Transaction Export Array
	Public Const ACCnt As Integer = 0
	Public Const ACClientCode As Integer = 1
	Public Const ACAgentCode As Integer = 2
	Public Const ACPolicyNumber As Integer = 3
	Public Const ACCoverStartDate As Integer = 4
	Public Const ACOperator As Integer = 5
	Public Const ACGrossAmount As Integer = 6
	Public Const ACCommission As Integer = 7
	Public Const ACTax As Integer = 8
	Public Const ACCurrency As Integer = 9
	Public Const ACTransactionType As Integer = 10
	Public Const ACExportStatus As Integer = 11
	
	

	Public Sub Main()
		
	End Sub
	'UPGRADE_NOTE: (7013) Constructor is just executed once. Please review if Component contains SingleUse classes because they have a different behaviour. More Information: http://www.vbtonet.com/ewis/ewi7013.aspx
	Sub New()
		Main()
	End Sub
	Sub JustForInvokeMain()
	End Sub
End Module