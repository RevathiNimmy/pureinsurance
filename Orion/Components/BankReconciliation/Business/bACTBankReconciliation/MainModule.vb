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
	' Edit History :
	'
	' RAW 13/01/2003 : PS187 : replaced hard-coded sql that deleted from
	'                          TransMatch with stored procedure
	'****************************************************************** '
	
	' This App
	Public Const ACApp As String = "bACTBankReconciliation"
	
	' This Class
	Private Const ACClass As String = "MainModule"
	
	
	
	
	
	
	
	' Log Level
	
	
    ' SQL
    'developer guide no. 39
    Public Const ACBankReconciliationSQL As String = "spu_ACT_Do_BankReconciliation"
	Public Const ACBankReconciliationName As String = "BankReconciliation"
	Public Const ACBankReconciliationStored As Boolean = True

    'developer guide no. 39
    Public Const ACBankReconciliationTotalsSQL As String = "spu_ACT_Do_BankReconciliation_Totals"
	Public Const ACBankReconciliationTotalsName As String = "BankReconciliationTotals"
	Public Const ACBankReconciliationTotalsStored As Boolean = True
	
    ' RAW 13/01/2003: PS187: added
    'developer guide no. 39
    Public Const ACUnMarkTransMatchSQL As String = "spu_ACT_UnMark_TransMatch"
	Public Const ACUnMarkTransMatchName As String = "UnMarkTransaction"
	Public Const ACUnMarkTransMatchStored As Boolean = True
	' RAW 13/01/2003: PS187: end
	
	'sj 28/04/2003 - start
	Public Const g_kLockBankReconciliation As String = "BANK_RECONCILIATION"
	'sj 28/04/2003 - end
End Module