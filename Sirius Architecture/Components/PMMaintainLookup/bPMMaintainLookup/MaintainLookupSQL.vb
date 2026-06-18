Option Strict Off
Option Explicit On
Imports System
Module MaintainLookupSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  05/05/1999
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MaintainLookup"
	'Start(Sriram P)CacheBug
	Public Const ACSelectLookupTableToCacheStored As Boolean = True
	Public Const ACSelectLookupTableToCacheName As String = "SelectLookupTableToCache"
	Public Const ACSelectLookupTableToCacheSQL As String = "EXEC spu_Get_PMLookups_For_Caching '{Table_Name}', {Language_ID}, {Is_Underwriting}"
	'End(Sriram P)CacheBug
    ' SQL
    ' developer guide no. 39
    Public Const ACSQLGetColumns As String = "spu_pm_Get_Columns"
	Public Const ACSQLGetColumnsName As String = "GetColumns"
	Public Const ACSQLGetColumnsStored As Boolean = True

    ' developer guide no. 39
    Public Const ACSQLGetKeyColumns As String = "spu_pm_Get_Lookup_Fields"
	Public Const ACSQLGetKeyColumnsName As String = "GetLookupFields"
	Public Const ACSQLGetKeyColumnsStored As Boolean = True
	
    ' developer guide no. 39
    Public Const ACSQLCaptionReturn As String = "spu_pm_caption_id_return"
	Public Const ACSQLCaptionReturnName As String = "GetCaptionID"
	Public Const ACSQLCaptionReturnStored As Boolean = True
	
	Public Const ACSQLSPColumns As String = "exec spu_columns "
	Public Const ACSQLSPColumnsName As String = "SPu_Columns"
	Public Const ACSQLSPColumnsStored As Boolean = True
	
	'SD 08/08/2002 Query table for varchar and nvarchar fields
    ' developer guide no. 39
    Public Const ACSQLGetVarcharFields As String = "spu_pm_Get_Varchar_Fields"
	Public Const ACSQLGetVarcharFieldsName As String = "GetVarcharFields"
	Public Const ACSQLGetVarcharFieldsStored As Boolean = True
	
	
	Public Const ACSQLGetViews As String = "select table_name from INFORMATION_SCHEMA.Views where table_name = {ViewName}"
	Public Const ACSQLGetViewsName As String = "GetViews"
	Public Const ACSQLGetViewsStored As Boolean = False
	
    ' developer guide no. 39
    Public Const ACSQLCheckLinkedDataMandatory As String = "spu_Check_Lookup_Linked_Data_Mandatory"
	Public Const ACSQLCheckLinkedDataMandatoryName As String = "GetCheckLinkedDataMandatory"
	Public Const ACSQLCheckLinkedDataMandatoryStored As Boolean = True
	
	Public Const ACSQLProductMTAEventAdd As String = "spu_SIR_Product_MTA_Event_Add"
	Public Const ACSQLProductMTAEventAddName As String = "ProductMTAEventAdd"
	Public Const ACSQLProductMTAEventAddStored As Boolean = True
	
	Public Const ACSQLProductClaimEventAdd As String = "spu_SIR_Product_Claim_Event_Add"
	Public Const ACSQLProductClaimEventAddName As String = "ProductClaimEventAdd"
    Public Const ACSQLProductClaimEventAddStored As Boolean = True

    ' Updated Market Place Data Model
    Public Const ACUpdateMPDataModelStored As Boolean = True
    Public Const ACUpdateMPDataModelName As String = "Update MPDataModel"
    Public Const ACUpdateMPDataModelSQL As String = "spu_GIS_UpdateMPDataModel"

    ' Export Market Place Data Model
    Public Const ACExportMPDataModelStored As Boolean = True
    Public Const ACExportMPDataModelName As String = "Export MPDataModel"
    Public Const ACExportMPDataModelSQL As String = "spu_GIS_ExportMPDataModel"
End Module