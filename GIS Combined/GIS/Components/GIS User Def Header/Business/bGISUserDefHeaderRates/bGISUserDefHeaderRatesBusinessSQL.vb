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
	'              bGISUserDefHeaderRates.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select All Lookup Header Rates SQL
	Public Const ACGetLookupHeaderRatesStored As Boolean = True
    Public Const ACGetLookupHeaderRatesName As String = "SelectLookupHeaderRates"
    'developer guide no. 39

    Public Const ACGetLookupHeaderRatesSQL As String = "spu_GIS_user_def_header_rat_saa"
	
	' Delete All Lookup Header Rates SQL
	Public Const ACDeleteLookupHeaderRatesStored As Boolean = True
    Public Const ACDeleteLookupHeaderRatesName As String = "DeleteLookupHeaderRates"
    'developer guide no. 39

    Public Const ACDeleteLookupHeaderRatesSQL As String = "spu_GIS_user_def_header_rat_del"
	
	' Insert Lookup Header Rates SQL
	Public Const ACInsertLookupHeaderRatesStored As Boolean = True
    Public Const ACInsertLookupHeaderRatesName As String = "InsertLookupHeaderRates"
    'developer guide no. 39

    Public Const ACInsertLookupHeaderRatesSQL As String = "spe_GIS_user_def_header_ra_add"
	
	' Select All Lookup Header Indicators SQL
	Public Const ACGetLookupHeaderIndicatorsStored As Boolean = True
    Public Const ACGetLookupHeaderIndicatorsName As String = "SelectLookupHeaderIndicators"
    'developer guide no. 39

    Public Const ACGetLookupHeaderIndicatorsSQL As String = "spu_GIS_user_def_header_ind_saa"
	
	' Delete All Lookup Header Indicators SQL
	Public Const ACDeleteLookupHeaderIndicatorsStored As Boolean = True
    Public Const ACDeleteLookupHeaderIndicatorsName As String = "DeleteLookupHeaderIndicators"
    'developer guide no. 39

    Public Const ACDeleteLookupHeaderIndicatorsSQL As String = "spu_GIS_user_def_header_ind_del"
	
	' Insert Lookup Header Rates SQL
	Public Const ACInsertLookupHeaderIndicatorsStored As Boolean = True
    Public Const ACInsertLookupHeaderIndicatorsName As String = "InsertLookupHeaderIndicators"
    'developer guide no. 39

    Public Const ACInsertLookupHeaderIndicatorsSQL As String = "spe_GIS_user_def_header_in_add"
End Module