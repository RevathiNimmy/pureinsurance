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
	' Date:  {TodaysDate}
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "dCLMPeril"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	
	
	' Task
	'***********************************************
	' TO DO: GLOBAL VARIABLE IN BAS MODULE
	'Public m_iTask As Integer
	'***********************************************
	
	' constants for ReserveDetailsArray
	Public Const g_ciRDAReserveID As Integer = 0
	Public Const g_ciRDAInitialReserve As Integer = 1
	Public Const g_ciRDARevisedReserve As Integer = 2
	Public Const g_ciRDAAverage As Integer = 3
	Public Const g_ciRDAPaidtoDate As Integer = 4
	
	'TN20010405 Start
	Public Const g_ciRDAThisPayment As Integer = 5
	Public Const g_ciRDAThisRevision As Integer = 6
	'TN20010405 End
	
	'DC111201 -broking only
	Public Const g_ciRDArevisedentered As Integer = 7
	
	' constants declared for PaymentDetailsArray
	Public Const g_ciPDAPaymentID As Integer = 0
	Public Const g_ciPDAPartycnt As Integer = 2
	Public Const g_ciPDAComments As Integer = 3
	Public Const g_ciPDAAmount As Integer = 1
	Public Const g_ciPDATaxAmount As Integer = 4
	Public Const g_ciPDAPayeeMediaType As Integer = 5
	Public Const g_ciPDAPayeeName As Integer = 6
	Public Const g_ciPDAPayeeBankName As Integer = 7
	Public Const g_ciPDAPayeeSortCode As Integer = 8
	Public Const g_ciPDAPayeeAccountNo As Integer = 9
	Public Const g_ciPDAPayeeCountry As Integer = 10
	Public Const g_ciPDAPayeeComments As Integer = 11
	Public Const g_ciPDACurrencyID As Integer = 12
	Public Const g_ciPDAPaymentLossXrate As Integer = 13
	
	' constants declared for General Details Array
	Public Const g_ciGDAUserdefinedperildataid As Integer = 0
	Public Const g_ciGDAValue As Integer = 1
End Module