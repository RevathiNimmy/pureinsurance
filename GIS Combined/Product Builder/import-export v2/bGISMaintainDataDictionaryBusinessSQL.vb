Option Strict Off
Option Explicit On
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
	' RAW 02/09/2003 : CQ2158 : added DropFK sql
	' ***************************************************************** '
	
	
	'SQL Statements
	
	' Example select using embedded SQL

	' Select GIS Object SQL
	Public Const ACGetAllGISObjectStored As Boolean = True
	Public Const ACGetAllGISObjectName As String = "SelectAllGISObject"
    Public Const ACGetAllGISObjectSQL As String = "spu_GIS_object_saa"
	
	' Insert GIS Object SQL
	Public Const ACInsertGISObjectStored As Boolean = True
	Public Const ACInsertGISObjectName As String = "InsertGISObject"
	'GSD Added 010702
    Public Const ACInsertGISObjectSQL As String = "spu_GIS_object_add"
	
	' Update GIS Object SQL
	Public Const ACUpdateGISObjectStored As Boolean = True
	Public Const ACUpdateGISObjectName As String = "InsertGISObject"
	'GSD Added 010702
    Public Const ACUpdateGISObjectSQL As String = "spe_GIS_object_upd"
	
	' Select GIS Property SQL
	Public Const ACGetAllGISPropertyStored As Boolean = True
	Public Const ACGetAllGISPropertyName As String = "SelectAllGISProperty"
    Public Const ACGetAllGISPropertySQL As String = "spu_GIS_property_saa"
	
	' Insert GIS Property SQL
	Public Const ACInsertGISPropertyStored As Boolean = True
	Public Const ACInsertGISPropertyName As String = "InsertGISProperty"
    Public Const ACInsertGISPropertySQL As String = "spu_GIS_property_add"
	
	' Insert GIS Property SQL
	'TF031002 - parameters not added dynamically on this one
	Public Const ACInsertGISPropertyParamStored As Boolean = True
	Public Const ACInsertGISPropertyParamName As String = "InsertGISProperty"
	'Tomo151002
    Public Const ACInsertGISPropertyParamSQL As String = "spu_GIS_property_add"
	'Public Const ACInsertGISPropertyParamSQL = "{call spu_GIS_property_add (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)}"
	
	' Update GIS Property SQL
	Public Const ACUpdateGISPropertyStored As Boolean = True
	Public Const ACUpdateGISPropertyName As String = "InsertGISProperty"
    Public Const ACUpdateGISPropertySQL As String = "spe_GIS_property_upd"
	
	' Delete GIS Property SQL
	Public Const ACDeleteGISPropertyStored As Boolean = True
	Public Const ACDeleteGISPropertyName As String = "DeteteGISProperty"
    Public Const ACDeleteGISPropertySQL As String = "spu_GIS_property_del"
	
	' Get Caption Id SQL
    Public Const ACGetPMCaptionSQL As String = "spu_pm_caption_id_return"
	Public Const ACGetPMCaptionStored As Boolean = True
	Public Const ACGetPMCaptionName As String = "GetPMCaptionID"
	
	' Select a Lookup SQL
	Public Const ACGetLookupStored As Boolean = False
	Public Const ACGetLookupName As String = "GetLookup"
	Public Const ACGetLookupSQL As String = "SELECT tn.{table_name}_id, cap.caption, tn.code " & "FROM {table_name} tn, pmcaption cap " & "WHERE tn.is_deleted = 0 " & "AND tn.effective_date <= {Effective_Date} " & "AND tn.caption_id = cap.caption_id " & "AND cap.language_id = {Language_ID} " & "ORDER BY 2"
	
	' Insert WP Fields SQL
	Public Const ACInsertWPFieldsStored As Boolean = True
	Public Const ACInsertWPFieldsName As String = "InsertWPFields"
	Public Const ACInsertWPFieldsSQL As String = "spe_wp_fields_add"
	
	' Select GIS QEM Usage SQL
	Public Const ACGetGisQemUsageStored As Boolean = True
	Public Const ACGetGisQemUsageName As String = "SelectGisQemUsage"
    Public Const ACGetGisQemUsageSQL As String = "spu_GIS_QEM_usage_sel"
	
	'RFC26/10/01 - Changed to use spu_gis_qem_usage_add rather than the spe version.
	'RFC26/10/01 - It needed to be amended and there is no point having the two versions.
	' Add GIS QEM Usage SQL
	Public Const ACAddGisQemUsageStored As Boolean = True
	Public Const ACAddGisQemUsageName As String = "AddGisQemUsage"
    Public Const ACAddGisQemUsageSQL As String = "spu_GIS_QEM_usage_add"
	
	' Select WP Fields SQL
	Public Const ACSelectWPFieldsStored As Boolean = True
	Public Const ACSelectWPFieldsName As String = "SelectWPFields"
    Public Const ACSelectWPFieldsSQL As String = "spe_wp_fields_sel"
	
	' Get SQL Server Version SQL
	Public Const ACMSGetVersionStored As Boolean = True
	Public Const ACMSGetVersionName As String = "GetMSSQLServerVersion"
    Public Const ACMSGetVersionSQL As String = "sp_MSgetversion"
	
	'drop procedure
	Public Const ACDropStoredProcedureStored As Boolean = True
	Public Const ACDropStoredProcedureName As String = "DDLDropProcedure"
    Public Const ACDropStoredProcedureSQL As String = "DDLDropProcedure "
	
	'JB Jun 10 drop table
	Public Const ACDropTableStored As Boolean = True
	Public Const ACDropTableName As String = "DDLDropTable"
    Public Const ACDropTableSQL As String = "call DDLDropTable"
	
	' RAW 02/09/2003 : CQ2158 : added
	'drop procedure
	Public Const ACDropFKStored As Boolean = True
	Public Const ACDropFKName As String = "DDLDropForeignKey"
    Public Const ACDropFKSQL As String = "call DDLDropForeignKey"
	' RAW 02/09/2003 : CQ2158 : end
	
	' Get list of column names for supplied table
	Public Const ACMSGetDMTableColumnsStored As Boolean = True
	Public Const ACMSGetDMTableColumnsName As String = "GetDMTableColumnList"
    Public Const ACMSGetDMTableColumnsSQL As String = "sp_mshelpColumns"
	
	' PW280803 - CQ1912
	Public Const ACGetAssociatClientObjectsStored As Boolean = True
	Public Const ACGetAssociatClientObjectsName As String = "GetAssociatedClientObjects"
    Public Const ACGetAssociatClientObjectsSQL As String = "spu_get_associated_client_objects"
	
	' PW280803 - CQ1912
	Public Const ACGetAssociatClientFieldsStored As Boolean = True
	Public Const ACGetAssociatClientFieldsName As String = "GetAssociatedClientFields"
    Public Const ACGetAssociatClientFieldsSQL As String = "spu_get_associated_client_fields"

    Public Const kGenerateUMLHeadersStored As Boolean = True
    Public Const kGenerateUMLHeadersName As String = "Generate UML Headers Script"
    Public Const kGenerateUMLHeadersSQL As String = "spu_APIE_get_UML_Headers_Script"

    Public Const kGetMaxNumberInUMLDetailsStored As Boolean = False
    Public Const kGetMaxNumberInUMLDetailsName As String = "Get Max Number in UML Details"
    Public Const kGetMaxNumberInUMLDetailsSQL As String = "SELECT MAX(gis_user_def_detail_id) FROM GIS_User_Def_Detail"

    Public Const kGenerateUMLDetailsStored As Boolean = True
    Public Const kGenerateUMLDetailsName As String = "Generate UML Details Script"
    Public Const kGenerateUMLDetailsSQL As String = "spu_APIE_get_UML_Details_Script"


End Module