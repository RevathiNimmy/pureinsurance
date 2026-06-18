Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: BusinessSQL
	'
	' Date: 23/10/2000
	'
	' Description: Contains the SQL Statements required by the
	'              bSIRPFRF.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select All PFRF SQL
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "SelectAllPFRF"
    'Developer Guide No. 39
    Public Const ACGetAllDetailsSQL As String = "spu_PFRF_saa"

    ' Check ID SQL
    Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckPFRFID"
    'Developer Guide No. 39
    Public Const ACCheckIDSQL As String = "spe_PFRF_check_id"

    ' Select PFRF SQL
    Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSinglePFRF"
    'Developer Guide No. 39
    Public Const ACSelectSingleSQL As String = "spu_PFRF_sel"

    ' Add PFRF SQL
    ' RAM20030401   : Added one more Parameter (ExistingDaysDelay)
    '                 Ref. Issue 2915
    ' Alix - Added @statement_frequency_id, @statement_report_id, @advance_instalments
    ' @review_pmuser_group_id, @remainder_threshhold, @remainder_at_end, @maximum_instalments
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddPFRF"
    'Start (Sriram P)Tech Spec - Valiant - P16 - Broker Instalments.doc section(5.2.2)
    'Added one more parameter vFinanceNetCommission
    'End (Sriram P)Tech Spec - Valiant - P16 - Broker Instalments.doc section(5.2.2)
    Public Const ACAddSQL As String = "spu_PFRF_add"

    ' Delete PFRF SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeletePFRF"
    'Developer Guide No. 39
    Public Const ACDeleteSQL As String = "spu_PFRF_del"
	
	' Update PFRF SQL
	' RAM20030401   : Added one more Parameter (ExistingDaysDelay)
	'                 Ref. Issue 2915
	' Alix - Added @statement_frequency_id, @statement_report_id, @advance_instalments
	' @review_pmuser_group_id, @remainder_threshhold, @remainder_at_end, @maximum_instalments
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdatePFRF"
	'Start (Sriram P)Tech Spec - Valiant - P16 - Broker Instalments.doc section(5.2.2)
	'Added one more parameter vFinanceNetCommission
	'End (Sriram P)Tech Spec - Valiant - P16 - Broker Instalments.doc section(5.2.2)
	Public Const ACUpdateSQL As String = "spu_PFRF_upd"
End Module