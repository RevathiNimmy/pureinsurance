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
	' Date: 07/05/1999
	'
	' Description: Contains the SQL Statements required by the
	'              bSIRDocSpooler.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select All SIRDocSpooler SQL
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "SelectAllSIRDocSpooler"
    'Developer Guide No 39
    Public Const ACGetAllDetailsSQL As String = "spe_document_template_saa"

    ' Check ID SQL
    Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckSIRDocSpoolerID"
    'Developer Guide No 39
    Public Const ACCheckIDSQL As String = "spe_document_template_check_id"

    ' Check Code SQL
    Public Const ACCheckCodeStored As Boolean = False
    Public Const ACCheckCodeName As String = "CheckSIRDocSpoolerCode"
    'Developer Guide No 39
    Public Const ACCheckCodeSQL As String = "select code from document_template where code = {code}"

    ' Update Printed SQL
    Public Const ACUpdatePrintedStored As Boolean = True
    Public Const ACUpdatePrintedName As String = "UpdatePrinted"
    'Developer Guide No 39
    Public Const ACUpdatePrintedSQL As String = "spu_document_spooler_upd_pr"

    ' Update Archived SQL
    Public Const ACUpdateArchivedStored As Boolean = True
    Public Const ACUpdateArchivedName As String = "UpdateArchived"
    'Developer Guide No 39
    Public Const ACUpdateArchivedSQL As String = "spu_document_spooler_upd_ar"

    ' Update Modified SQL
    Public Const ACUpdateModifiedStored As Boolean = True
    Public Const ACUpdateModifiedName As String = "UpdateModified"
    'Developer Guide No 39
    Public Const ACUpdateModifiedSQL As String = "spu_document_spooler_upd_mo"
End Module