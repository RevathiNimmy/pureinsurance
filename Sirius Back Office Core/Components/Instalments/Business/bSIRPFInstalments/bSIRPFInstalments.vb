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
	' Date:  {TodaysDate}
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bSIRPFInstalments"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"
	
	'Period constants
	Public Const pfprem_finance_cnt_COL As Integer = 0
	Public Const pfprem_finance_version_COL As Integer = 1
	Public Const InstalmentNumber_COL As Integer = 2
	Public Const DueDate_COL As Integer = 3
	Public Const Fee_COL As Integer = 4
	Public Const Amount_COL As Integer = 5
	Public Const TransactionCode_COL As Integer = 6
	Public Const Status_COL As Integer = 7
	Public Const BatchNumber_COL As Integer = 8
	Public Const BatchExportDate_COL As Integer = 9
	Public Const PostedDate_COL As Integer = 10
	Public Const InstalmentsCount_COL As Integer = 11
	Public Const InstalmentsProcessed_COL As Integer = 12
	Public Const PFTransaction_id_COL As Integer = 13
	Public Const Tax_COL As Integer = 14
	Public Const Commission_COL As Integer = 15
	Public Const PFInstalmentsResult_COL As Integer = 16
	Public Const BatchID_COL As Integer = 17
	
	Public Const ChildDueDate_COL As Integer = 0
	Public Const ChildFee_COL As Integer = 1
	Public Const ChildTax_COL As Integer = 2
	Public Const ChildCommission_COL As Integer = 3
	Public Const ChildAmount_COL As Integer = 4
	
	'SD 31/07/2002 Scalability Changes
	Public Const NEXTBATCHNUMBER As String = "NextBatchNumber"
	
	Public Const kSystemOptionCreditControlEnabled As Integer = 5001
    Public Const kSystemOptionPaymentHubEnabled As Integer = 5200

    Public Const kIndexMediaType As Integer = 0
    Public Const kIndexCurrencyCode As Integer = 1
    Public Const kIndexIntegerationToken As Integer = 2
    Public Const kIndexTokenId As Integer = 3
    Public Const kIndexPartyCnt As Integer = 4

    Sub Main_Renamed()
		
	End Sub
End Module