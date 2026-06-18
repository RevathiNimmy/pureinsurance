Option Strict Off
Option Explicit On
Imports System
Module bCLMFinSummSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name    : bCLMFinSummSQL
	' Date          : 15/07/2000
	' Description   : Contains the SQL Statements to (Stored Procedures
	'                 and Enbedded SQL) manipulate an FindClaim
	' Author        : SK
	' Edit History  :
	' ***************************************************************** '
    'developer guide no.39
    'start
	' Retrieve GetResvTypCount SQL
	Public Const ACGetResvTypCountStored As Boolean = True
	Public Const ACGetResvTypCountName As String = "GetResvTypCount"
    Public Const ACGetResvTypCountSQL As String = "spu_get_resv_typ_count"
	
	
	' Select GetPerilsForReserve SQL
	Public Const ACGetPerilsForReserveStored As Boolean = True
	Public Const ACGetPerilsForReserveName As String = "GetPerilsForReserve"
    Public Const ACGetPerilsForReserveSQL As String = "spu_get_perils_for_reserve"
	
	' Select GetPerilsForReserve SQL
	Public Const ACGetPerilsForRecoveryStored As Boolean = True
	Public Const ACGetPerilsForRecoveryName As String = "GetPerilsForRecovery"
    Public Const ACGetPerilsForRecoverySQL As String = "spu_get_perils_for_recovery"
	
	' Select GetPerilTotals SQL
	Public Const ACGetPerilTotalsStored As Boolean = True
	Public Const ACGetPerilTotalsName As String = "GetPerilTotals"
    Public Const ACGetPerilTotalsSQL As String = "spu_get_peril_totals"
    'end
End Module