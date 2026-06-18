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
	'              bGISUserDefHeader.Business class.
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
	Public Const ACGetLookupHeadersStored As Boolean = True
    Public Const ACGetLookupHeadersName As String = "SelectLookupHeaders"
    'developer guide no. 39

    Public Const ACGetLookupHeadersSQL As String = "spe_GIS_user_def_header_saa"
	
	' Delete All Lookup Headers SQL
	Public Const ACUpdateLookupHeadersStored As Boolean = True
    Public Const ACUpdateLookupHeadersName As String = "DeleteLookupHeaders"
    'developer guide no. 39

    Public Const ACUpdateLookupHeadersSQL As String = "spe_GIS_user_def_header_upd"
	
	' Insert All Lookup Headers SQL
	Public Const ACInsertLookupHeadersStored As Boolean = True
    Public Const ACInsertLookupHeadersName As String = "InsertLookupHeaders"
    'developer guide no. 39

    Public Const ACInsertLookupHeadersSQL As String = "spe_GIS_user_def_header_add"
	
	' Select All Lookup Header Rates SQL
	Public Const ACGetLookupHeaderRatesStored As Boolean = True
    Public Const ACGetLookupHeaderRatesName As String = "SelectLookupHeaderRates"
    'developer guide no. 39

    Public Const ACGetLookupHeaderRatesSQL As String = "spu_GIS_user_def_header_rat_saa"
	
	' Select All Lookup Header Indicators SQL
	Public Const ACGetLookupHeaderIndicatorsStored As Boolean = True
    Public Const ACGetLookupHeaderIndicatorsName As String = "SelectLookupHeaderIndicators"
    'developer guide no. 39

    Public Const ACGetLookupHeaderIndicatorsSQL As String = "spu_GIS_user_def_header_ind_saa"
    'developer guide no. 39

    Public Const ACSQLCaptionReturn As String = "spu_pm_caption_id_return"
	Public Const ACSQLCaptionReturnName As String = "GetCaptionID"
	Public Const ACSQLCaptionReturnStored As Boolean = True
End Module