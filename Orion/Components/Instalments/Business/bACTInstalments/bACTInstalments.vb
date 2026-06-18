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
	' Date:  07/04/1998
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '

    Public Const ACApp As String = "bACTInstalments"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    'SD 09/01/2003
    Public Const ACValueWhenCreditControlSet As String = "1"
    Public Const ACCommissionMovementOptionNo As String = "16" 'PN10637
    Public Const ACEarnCommissionOnPartPayments As String = "94" 'FSA Phase IV

    Public Const AsDebited As String = "0"
    Public Const ClientPayment As String = "1"
    Public Const InsurerSetted As String = "2"
    Public Const WhenEffective As String = "3"

    Public Const kDocumentTypeCodeRenewalInvoice As String = "SRD"
    Public Const kSystemOptionCreditControlEnabled As Integer = 5001

    Public Sub Main()

    End Sub
    Sub New()
        Main()
    End Sub
	Sub JustForInvokeMain()
	End Sub
End Module