Option Strict Off
Option Explicit On
Imports System
Module PFExportSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: PFExportSQL
	'
	' Date: 13-Aug-2001
	'
	' Description: Contains the SQL Statements required by the
	'              bSIRPFExport class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select PFExport SQL
	Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSinglePFPaymentMethod"
    'Developer Guide No 39
    'Public Const ACSelectSingleSQL As String = "{call spe_PFPaymentMethod_sel (?)}"
    Public Const ACSelectSingleSQL As String = "spe_PFPaymentMethod_sel"

    ' Update PFExport SQL
    'MKW160204 PN10300.  Added ExcludeAudis parameter.
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdatePFPaymentMethodSettings"
    'Developer Guide No 39
    'Public Const ACUpdateSQL As String = "{call spe_PFPaymentMethod_upd (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)}"
    Public Const ACUpdateSQL As String = "spe_PFPaymentMethod_upd"
End Module