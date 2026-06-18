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
	' Date: 05/05/1999
	'
	' Description: Contains the SQL Statements required by the
	'              bSIRNarr.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
    ' Select All Lookup Headers SQL
    'developer guide no. 39
    'Start
	Public Const ACGetLookupHeadersStored As Boolean = True
	Public Const ACGetLookupHeadersName As String = "SelectLookupHeaders"
    'Public Const ACGetLookupHeadersSQL As String = "{call spe_lookup_header_saa}"
    Public Const ACGetLookupHeadersSQL As String = "spe_lookup_header_saa"
	
	' Delete All Lookup Headers SQL
	Public Const ACUpdateLookupHeadersStored As Boolean = True
	Public Const ACUpdateLookupHeadersName As String = "DeleteLookupHeaders"
    'Public Const ACUpdateLookupHeadersSQL As String = "{call spe_lookup_header_upd (?,?,?,?,?,?,?)}"
    Public Const ACUpdateLookupHeadersSQL As String = "spe_lookup_header_upd"

	' Insert All Lookup Headers SQL
	Public Const ACInsertLookupHeadersStored As Boolean = True
	Public Const ACInsertLookupHeadersName As String = "InsertLookupHeaders"
    'Public Const ACInsertLookupHeadersSQL As String = "{call spe_lookup_header_add (?,?,?,?,?,?,?)}"
    Public Const ACInsertLookupHeadersSQL As String = "spe_lookup_header_add"

	' Select All Lookup Header Rates SQL
	Public Const ACGetLookupHeaderRatesStored As Boolean = True
	Public Const ACGetLookupHeaderRatesName As String = "SelectLookupHeaderRates"
    'Public Const ACGetLookupHeaderRatesSQL As String = "{call spu_lookup_header_rate_saa (?)}"
    Public Const ACGetLookupHeaderRatesSQL As String = "spu_lookup_header_rate_saa"

	' Select All Lookup Header Indicators SQL
	Public Const ACGetLookupHeaderIndicatorsStored As Boolean = True
	Public Const ACGetLookupHeaderIndicatorsName As String = "SelectLookupHeaderIndicators"
    'Public Const ACGetLookupHeaderIndicatorsSQL As String = "{call spu_lookup_header_indicator_saa (?)}"
    Public Const ACGetLookupHeaderIndicatorsSQL As String = "spu_lookup_header_indicator_saa"
    'end
End Module