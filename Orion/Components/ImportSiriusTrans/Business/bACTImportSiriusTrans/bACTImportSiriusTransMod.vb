Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  05/10/1997
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '

    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bACTImportSiriusTrans"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    Public Const SIRSalesTransactionLedgerCode As String = "SL"

    Public Const AsDebited As String = "0"
    Public Const ClientPayment As String = "1"
    Public Const InsurerSetted As String = "2"
    'DC260606 Datasure : moved commission tax
    Public Const CashBasis As String = "1"
    Public Const InvoiceBasis As String = "0"

    'System Options
    Public Const ACCurrencyDifferenceCrebitAccount As Integer = 150
    Public Const ACCurrencyDifferenceDebitAccount As Integer = 151
    Public Const ACWriteOffDebtorAccount As Integer = 152
    Public Const ACWriteOffCrebitorAccount As Integer = 153

    Public Sub Main()

    End Sub
    Sub New()
        Main()
    End Sub
	Sub JustForInvokeMain()
	End Sub
End Module