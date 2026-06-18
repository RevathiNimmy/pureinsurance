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
	'              bGISUserDefDetail.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	'Start(Sriram P)CacheBug
	Public Const ACSelectGISUserDefDetailToCacheStored As Boolean = False
	Public Const ACSelectGISUserDefDetailToCacheName As String = "SelectLookupTableToCache"
	Public Const ACSelectGISUserDefDetailToCacheSQL As String = "SELECT Cast(GD.GIS_user_def_detail_id as varchar(10)) as GIS_user_def_detail_id, cap.caption, GD.code, " &  _
	                                                            "       GD.is_deleted, " &  _
	                                                            "       GD.effective_date, " &  _
	                                                            "       Cast(GD.parent as varchar(10)) as parent " &  _
	                                                            "FROM GIS_user_def_detail GD, pmcaption cap " &  _
	                                                            "WHERE GD.GIS_user_def_header_id = {table} " &  _
	                                                            "AND GD.caption_id = cap.caption_id " &  _
	                                                            "AND cap.language_id = {Language_ID}"
	'End(Sriram P)CacheBug
	' Select All Lookup Details SQL
	Public Const ACGetLookupDetailsStored As Boolean = True
    Public Const ACGetLookupDetailsName As String = "SelectLookupDetails"
    'developer guide no. 39

    Public Const ACGetLookupDetailsSQL As String = "spu_GIS_user_def_detail_saa"
	
	' Delete All Lookup Details SQL
	Public Const ACUpdateLookupDetailsStored As Boolean = True
    Public Const ACUpdateLookupDetailsName As String = "DeleteLookupDetails"
    'developer guide no. 39

    Public Const ACUpdateLookupDetailsSQL As String = "spe_GIS_user_def_detail_upd"
	
	' Insert All Lookup Details SQL
	Public Const ACInsertLookupDetailsStored As Boolean = True
    Public Const ACInsertLookupDetailsName As String = "InsertLookupDetails"
    'developer guide no. 39

    Public Const ACInsertLookupDetailsSQL As String = "spe_GIS_user_def_detail_add"
    'developer guide no. 39

    Public Const ACSQLCaptionReturn As String = "spu_pm_caption_id_return"
	Public Const ACSQLCaptionReturnName As String = "GetCaptionID"
	Public Const ACSQLCaptionReturnStored As Boolean = True
	
	' Select Header Ind Details SQL
	Public Const ACGetHeaderIndStored As Boolean = True
    Public Const ACGetHeaderIndName As String = "GetHeaderInd"
    'developer guide no. 39

    Public Const ACGetHeaderIndSQL As String = "spu_GIS_user_def_header_ind_saa"
	
	' Check Lookup Details SQL
	Public Const ACCheckLookupDetailsStored As Boolean = True
    Public Const ACCheckLookupDetailsName As String = "CheckLookupDetails"
    'developer guide no. 39

    Public Const ACCheckLookupDetailsSQL As String = "spu_check_GIS_user_def_detail"
End Module