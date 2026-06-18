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
	' Date: 20/12/1999
	'
	' Description: Contains the SQL Statements required by the
	'              bSIRMaintainAuthority.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select Data Dictionary SQL
	Public Const ACGetDataDictionaryStored As Boolean = True
	Public Const ACGetDataDictionaryName As String = "SelectDataDictionary"
    'Developer Guide No. 39
    Public Const ACGetDataDictionarySQL As String = "spu_data_dictionary_sel"

    ' Select Specific Data Dictionary SQL
    Public Const ACGetSpecificDataDictionaryStored As Boolean = True
    Public Const ACGetSpecificDataDictionaryName As String = "SelectSpecificDataDictionary"
    'Developer Guide No. 39
    Public Const ACGetSpecificDataDictionarySQL As String = "spu_spec_data_dictionary_sel"

    'Developer Guide No. 39
    Public Const ACSQLCaptionReturn As String = "spu_pm_caption_id_return"
    Public Const ACSQLCaptionReturnName As String = "GetCaptionID"
    Public Const ACSQLCaptionReturnStored As Boolean = True


    ' Select PMLookup All SQL
    Public Const ACSelectAllStored As Boolean = False
    Public Const ACSelectAllName As String = "SelectPMLookupAll"
    Public Const ACSelectAllSQL As String = "SELECT tn.{Table_Name}_id, cap.caption, tn.code FROM {Table_Name} tn, pmcaption cap WHERE tn.is_deleted = 0 AND tn.effective_date <= {Effective_Date} AND tn.caption_id = cap.caption_id and cap.language_id = {Language_ID}"

    ' Select GIS lookup stuff
    Public Const ACSelectGISUserDefDetailStored As Boolean = True
    Public Const ACSelectGISUserDefDetailName As String = "SelectGISUserDefDetail"
    'Developer Guide No. 39
    Public Const ACSelectGISUserDefDetailSQL As String = "spu_GIS_user_def_detail_saa"

    ' Select all users on the system.
    Public Const ACSelectPMUsersStored As Boolean = True
    Public Const ACSelectPMUsersName As String = "SelectPMUsers"
    'Developer Guide No. 39
    Public Const ACSelectPMUsersSQL As String = "spu_pmuser_all_users_sel"

    ' Select all products on the system.
    Public Const ACSelectProductsStored As Boolean = True
    Public Const ACSelectProductsName As String = "SelectProducts"
    'Developer Guide No. 39
    Public Const ACSelectProductsSQL As String = "spe_Product_saa"

    ' Authority stuff.
    Public Const ACSelectAuthorityLevelsStored As Boolean = True
    Public Const ACSelectAuthorityLevelsName As String = "SelectAuthorityLevelTypes"
    'Developer Guide No. 39
    Public Const ACSelectAuthorityLevelsSQL As String = "spu_Authority_Level_Type_saa"

    Public Const ACSelectUserAuthorityLevelsStored As Boolean = True
    Public Const ACSelectUserAuthorityLevelsName As String = "SelectUserAuthorityLevels"
    'Developer Guide No. 39
    Public Const ACSelectUserAuthorityLevelsSQL As String = "spe_PMUser_Authority_Level_sel"

    Public Const ACAddUserAuthorityLevelStored As Boolean = True
    Public Const ACAddUserAuthorityLevelName As String = "AddUserAuthorityLevel"
    'Developer Guide No. 39
    Public Const ACAddUserAuthorityLevelSQL As String = "spe_PMUser_Authority_Level_add"

    Public Const ACUpdateUserAuthorityLevelStored As Boolean = True
    Public Const ACUpdateUserAuthorityLevelName As String = "UpdateUserAuthorityLevel"
    'Developer Guide No. 39
    Public Const ACUpdateUserAuthorityLevelSQL As String = "spe_PMUser_Authority_Level_upd"

    Public Const ACDeleteUserAuthorityLevelStored As Boolean = True
    Public Const ACDeleteUserAuthorityLevelName As String = "DeleteUserAuthorityLevel"
    'Developer Guide No. 39
    Public Const ACDeleteUserAuthorityLevelSQL As String = "spe_PMUser_Authority_Level_del"
	
End Module