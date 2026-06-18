Option Strict Off
Option Explicit On
Module PMLookupSQL
    'Attribute VB_Name = "PMLookupSQL"
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: PMLookupSQL
    '
    ' Date: 25 September 1996
    '
    ' Description: Contains the SQL Statements to Select a single or all
    '              lookup fields from a lookup table.
    '
    ' Edit History: 250996 - Created
    '               TF200198 -GetCodeFromID added
    ' RAW 15/07/2003 : CQ258 : include deleted entries in select but return deleted column to identify them
    ' RAM20040310    : Code changes related to Caching
    ' ***************************************************************** '

    'SQL Statements

    ' Select PMLookup By ID SQL
    Public Const ACSelectByIDStored As Boolean = False
    Public Const ACSelectByIDName As String = "SelectGISLookupByID"
    ' RAW 15/07/2003 : CQ258 : added is_deleted column
    Public Const ACSelectByIDSQL As String = "SELECT GD.GIS_user_def_detail_id, cap.caption, GD.code, " &
                                             "       GD.is_deleted " &
                                             "FROM GIS_user_def_detail GD, pmcaption cap " &
                                             "WHERE GD.parent = {ID} " &
                                             "AND GD.caption_id = cap.caption_id " &
                                             "AND GD.GIS_user_def_header_id = {table} " &
                                             "AND cap.language_id = {Language_ID}"


    ' Select PMLookup All SQL
    Public Const ACSelectAllStored As Boolean = False
    Public Const ACSelectAllName As String = "SelectGISLookupAll"
    ' RAW 15/07/2003 : CQ258 : added is_deleted column and removed where is_deleted test
    Public Const ACSelectAllSQL As String = "SELECT GD.GIS_user_def_detail_id, cap.caption, GD.code, " &
                                            "       GD.is_deleted " &
                                            "FROM GIS_user_def_detail GD, pmcaption cap " &
                                            "WHERE GD.GIS_user_def_header_id = {table} " &
                                            "AND GD.effective_date <= {Effective_Date} " &
                                            "AND GD.caption_id = cap.caption_id " &
                                            "AND cap.language_id = {Language_ID}"

    ' Get Effective ID From Code
    Public Const ACGetFromCodeStored As Boolean = True
    Public Const ACGetFromCodeName As String = "GetFromCode"
    'developer guide no. 39 of Guide
    Public Const ACGetFromCodeSQL As String = "spu_pm_get_eff_id_from_code"

    ' Get Effective ID From ID
    Public Const ACGetFromIDStored As Boolean = True
    Public Const ACGetFromIDName As String = "GetFromID"
    'developer guide no. 39 of Guide
    Public Const ACGetFromIDSQL As String = "spu_pm_get_eff_id_from_id"

    ' Get Code From ID - TF200198
    Public Const ACGetCodeFromIDStored As Boolean = True
    Public Const ACGetCodeFromIDName As String = "GetCodeFromID"
    'developer guide no. 39 of Guide
    Public Const ACGetCodeFromIDSQL As String = "spu_pm_get_code_from_id"

    ' RAM20040310 : Code changes related to Caching, this SQL Script is similar to
    '               ACSelectAllStored but
    '               1. with out the effective date filter
    '               2. Fetch the effective date field as well
    '               3. Fetch the parent field as well
    ' RAM20040323 : 4. Cast the ID into String, since dPMDAO returns ID as string before
    '               5. Cast the parent field into String
    Public Const ACSelectGISUserDefDetailToCacheStored As Boolean = False
    Public Const ACSelectGISUserDefDetailToCacheName As String = "SelectLookupTableToCache"
    Public Const ACSelectGISUserDefDetailToCacheSQL As String = "SELECT Cast(GD.GIS_user_def_detail_id as varchar(10)) as GIS_user_def_detail_id, cap.caption, GD.code, " &
                                                                "       GD.is_deleted, " &
                                                                "       GD.effective_date, " &
                                                                "       Cast(GD.parent as varchar(10)) as parent " &
                                                                "FROM GIS_user_def_detail GD, pmcaption cap " &
                                                                "WHERE GD.GIS_user_def_header_id = {table} " &
                                                                "AND GD.caption_id = cap.caption_id " &
                                                                "AND cap.language_id = {Language_ID}"
End Module