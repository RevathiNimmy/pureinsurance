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
	'              bSIRMaintainUserData.Business class.
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
	' Select Data Dictionary SQL
	Public Const ACUpdateDataDictionaryStored As Boolean = True
	Public Const ACUpdateDataDictionaryName As String = "SelectDataDictionary"
	Public Const ACUpdateDataDictionarySQL As String = "spu_full_data_dictionary_sel"
	
	Public Const ACUpdateAllScreenHeaderStored As Boolean = True
	Public Const ACUpdateAllScreenHeaderName As String = "SelectAllScreenHeader"
    Public Const ACUpdateAllScreenHeaderSQL As String = "spe_GIS_screen_sel"
	' Select Screen Details SQL
	Public Const ACUpdateAllScreenDetailsStored As Boolean = True
	Public Const ACUpdateAllScreenDetailsName As String = "SelectAllScreenDetails"
    Public Const ACUpdateAllScreenDetailsSQL As String = "spu_GIS_screen_detail_extra_saa"
	
	' Select Child Screen Details SQL
	Public Const ACUpdateAllChildScreenDetailsStored As Boolean = True
	Public Const ACUpdateAllChildScreenDetailsName As String = "SelectAllChildScreenDetails"
    Public Const ACUpdateAllChildScreenDetailsSQL As String = "spu_GIS_child_screen_detail_saa"
	' Select Data Dictionary SQL
	
	'End (Sriram P)CacheBug
	Public Const ACGetDataDictionaryStored As Boolean = True
    Public Const ACGetDataDictionaryName As String = "SelectDataDictionary"
    'developer guide no. 39
    Public Const ACGetDataDictionarySQL As String = "spu_data_dictionary_sel"

	
	' Select Specific Data Dictionary SQL
	Public Const ACGetSpecificDataDictionaryStored As Boolean = True
    Public Const ACGetSpecificDataDictionaryName As String = "SelectSpecificDataDictionary"
    'developer guide no. 39
    Public Const ACGetSpecificDataDictionarySQL As String = "spu_spec_data_dictionary_sel"
	
	' Delete Screen Header SQL
	Public Const ACUpdateScreenHeaderStored As Boolean = True
	Public Const ACUpdateScreenHeaderName As String = "UpdateScreenHeader"
    Public Const ACUpdateScreenHeaderSQL As String = "spu_GIS_Screen_upd_ex"
	
	' Insert Screen Header SQL
	Public Const ACInsertScreenHeaderStored As Boolean = True
	Public Const ACInsertScreenHeaderName As String = "InsertScreenHeader"
    Public Const ACInsertScreenHeaderSQL As String = "spe_GIS_screen_add" 'parameters added by addParamater
	
	' Select Screen Details SQL
	Public Const ACGetAllScreenDetailsStored As Boolean = True
    Public Const ACGetAllScreenDetailsName As String = "SelectAllScreenDetails"
    'developer guide no. 39
    Public Const ACGetAllScreenDetailsSQL As String = "spu_GIS_screen_detail_saa"
	
	' Delete Screen Details SQL
	Public Const ACDeleteScreenDetailsStored As Boolean = True
	Public Const ACDeleteScreenDetailsName As String = "DeleteScreenDetails"
    Public Const ACDeleteScreenDetailsSQL As String = "spu_GIS_screen_detail_del"
	
	' Insert Screen Details SQL
	Public Const ACInsertScreenDetailsStored As Boolean = True
	Public Const ACInsertScreenDetailsName As String = "InsertScreenDetails"
    Public Const ACInsertScreenDetailsSQL As String = "spe_GIS_screen_detail_add"
	' Select Screen Details SQL
	Public Const ACGetAllChildScreenDetailsStored As Boolean = True
    Public Const ACGetAllChildScreenDetailsName As String = "SelectAllChildScreenDetails"
    'developer guide no. 39
    Public Const ACGetAllChildScreenDetailsSQL As String = "spu_GIS_child_screen_detail_saa"
	
	' Select Children SQL
	Public Const ACGetScreenChildrenStored As Boolean = True
	Public Const ACGetScreenChildrenName As String = "GetScreenChildren"
    Public Const ACGetScreenChildrenSQL As String = "spu_GIS_Screen_children"
	
	' Copy Screen SQL
	Public Const ACCopyScreenStored As Boolean = True
	Public Const ACCopyScreenName As String = "CopyScreen"
    Public Const ACCopyScreenSQL As String = "spu_GIS_Screen_cop"
	
	' Copy Child Screen SQL
	' RAW 08/07/2003 : CQ1596 : removed
	'Public Const ACCopyChildScreenStored = True
	'Public Const ACCopyChildScreenName = "CopyChildScreen"
    'Public Const ACCopyChildScreenSQL = "spu_GIS_Child_Screen_cop (?,?) }"
	
	' Copy Screen Detail SQL
	Public Const ACCopyScreenDetailStored As Boolean = True
	Public Const ACCopyScreenDetailName As String = "CopyScreenDetail"
    Public Const ACCopyScreenDetailSQL As String = "spu_GIS_Screen_detail_cop"
	
	' Set Child SQL
	Public Const ACSetChildStored As Boolean = True
	Public Const ACSetChildName As String = "SetChild"
    Public Const ACSetChildSQL As String = "spu_GIS_Screen_detail_set_child"
	
	' Check The Screen Header Code SQL
	Public Const ACCheckScreenHeaderCodeStored As Boolean = True
	Public Const ACCheckScreenHeaderCodeName As String = "CheckScreenHeaderCode"
    Public Const ACCheckScreenHeaderCodeSQL As String = "spu_GIS_screen_code_check"
	
    Public Const ACSQLCaptionReturn As String = "spu_pm_caption_id_return"
	Public Const ACSQLCaptionReturnName As String = "GetCaptionID"
	Public Const ACSQLCaptionReturnStored As Boolean = True
	
    Public Const ACSQLGetScreensByObjecTypeSQL As String = "spu_GIS_Screen_sel_by_type"
	Public Const ACSQLGetScreensByObjecTypeName As String = "GetScreensByObjecType"
	Public Const ACSQLGetScreensByObjecTypeStored As Boolean = True
	
	Public Const kGetDataModelDetailsName As String = "returns details for the specified gis data model ID"
	Public Const kGetDataModelDetailsSQL As String = "SPU_GIS_GET_DATA_MODEL_DETAILS_FOR_CODE"
End Module