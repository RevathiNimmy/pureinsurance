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
	'              bSIRLookupDetailRates.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select All Lookup Detail Rates SQL
	Public Const ACGetLookupDetailRatesStored As Boolean = True
	Public Const ACGetLookupDetailRatesName As String = "SelectLookupDetailRates"
    'Public Const ACGetLookupDetailRatesSQL As String = "{call spu_lookup_detail_rates_saa (?)}"
    Public Const ACGetLookupDetailRatesSQL As String = "spu_lookup_detail_rates_saa"

	' Delete All Lookup Detail Rates SQL
	Public Const ACDeleteLookupDetailRatesStored As Boolean = True
	Public Const ACDeleteLookupDetailRatesName As String = "DeleteLookupDetailRates"
    'Public Const ACDeleteLookupDetailRatesSQL As String = "{call spu_lookup_detail_rates_del (?)}"
    Public Const ACDeleteLookupDetailRatesSQL As String = "spu_lookup_detail_rates_del"

	' Insert Lookup Detail Rates SQL
	Public Const ACInsertLookupDetailRatesStored As Boolean = True
	Public Const ACInsertLookupDetailRatesName As String = "InsertLookupDetailRates"
    'Public Const ACInsertLookupDetailRatesSQL As String = "{call spe_lookup_detail_rates_add (?,?,?)}"
    Public Const ACInsertLookupDetailRatesSQL As String = "spe_lookup_detail_rates_add"

	' Select All Lookup Detail Indicators SQL
	Public Const ACGetLookupDetailIndicatorsStored As Boolean = True
	Public Const ACGetLookupDetailIndicatorsName As String = "SelectLookupDetailIndicators"
    'Public Const ACGetLookupDetailIndicatorsSQL As String = "{call spu_lookup_detail_indicator_saa (?)}"
    Public Const ACGetLookupDetailIndicatorsSQL As String = "spu_lookup_detail_indicator_saa"

	' Delete All Lookup Detail Indicators SQL
	Public Const ACDeleteLookupDetailIndicatorsStored As Boolean = True
	Public Const ACDeleteLookupDetailIndicatorsName As String = "DeleteLookupDetailIndicators"
    'Public Const ACDeleteLookupDetailIndicatorsSQL As String = "{call spu_lookup_detail_indicator_del (?)}"
    Public Const ACDeleteLookupDetailIndicatorsSQL As String = "spu_lookup_detail_indicator_del"

	' Insert Lookup Detail Rates SQL
	Public Const ACInsertLookupDetailIndicatorsStored As Boolean = True
	Public Const ACInsertLookupDetailIndicatorsName As String = "InsertLookupDetailIndicators"
    'Public Const ACInsertLookupDetailIndicatorsSQL As String = "{call spe_lookup_detail_indicato_add (?,?,?)}"
    Public Const ACInsertLookupDetailIndicatorsSQL As String = "spe_lookup_detail_indicato_add"
    'end
End Module