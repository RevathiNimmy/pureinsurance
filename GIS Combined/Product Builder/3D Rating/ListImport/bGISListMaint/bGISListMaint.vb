Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Module Main
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Module Name: Main
	'
	' Date: 28/06/2002
	'
	' Description:  This contains the main constants
	'
	' Edit History:
	'   28/06/2002 SJP  - Tidied up after merge from Carole Nash
	' ***************************************************************** '
	'***********************************************
	' TO DO: GLOBAL VARIABLE IN BAS MODULE
	'Public m_lReturn As Long
	'***********************************************
	
	'***********************************************
	' TO DO: GLOBAL VARIABLE IN BAS MODULE
	'Public m_oBusiness As Object
	'***********************************************
    'Developer Guide No. 39
	Public Const ACApp As String = "List Maintenance"
	Public Const ACClass As String = "Main"
	
    Public Const ACIsInUseSQL As String = "call spu_isinuse"
	Public Const ACIsInUseName As String = "spu_isinuse"
	Public Const ACIsInUseProc As Boolean = True
	
    Public Const ACListTypeIsUniqueSQL As String = "spu_GIS_List_Type_Is_Unique"
    Public Const ACListTypeIsUniqueName As String = "spu_GIS_List_Type_Is_Unique"
    Public Const ACListTypeIsUniqueProc As Boolean = True

    Public Const ACAddUsageSQL As String = "spu_gis_list_add_usage"
    Public Const ACAddUsageName As String = "spu_gis_list_add_usage"
    Public Const ACAddUsageProc As Boolean = True

    Public Const ACSaveNewListTypeSQL As String = "spu_GIS_List_Type_add"
    Public Const ACSaveNewListTypeName As String = "spu_GIS_List_Type_add"
    Public Const ACSaveNewListTypeProc As Boolean = True

    Public Const ACDeleteListTypeSQL As String = "spu_GIS_List_Type_del"
    Public Const ACDeleteListTypeName As String = "spu_GIS_List_Type_del "
    Public Const ACDeleteListTypeProc As Boolean = True

    Public Const ACGetListTypesSQL As String = "spu_GIS_List_Type_get"
    Public Const ACGetListTypesName As String = "spu_GIS_List_Type_get"
    Public Const ACGetListTypesProc As Boolean = True

    Public Const ACGetUserDefinedCodesSQL As String = "spu_GIS_Get_UDC_items"
    Public Const ACGetUserDefinedCodesName As String = "spu_GIS_Get_UDC_items"
    Public Const ACGetUserDefinedCodesProc As Boolean = True

    Public Const ACGetListVersionsSQL As String = "spu_GIS_List_Type_version"
    Public Const ACGetListVersionsName As String = "spu_GIS_List_Type_version"
    Public Const ACGetListVersionsProc As Boolean = True

    Public Const ACListItemExistsSQL As String = "spu_GIS_List_Type_Item_Exists"
    Public Const ACListItemExistsName As String = "spu_GIS_List_Type_Item_Exists"
    Public Const ACListItemExistsProc As Boolean = True

    Public Const ACListExistsSQL As String = "spu_GIS_List_Type_Exists"
    Public Const ACListExistsName As String = "spu_GIS_List_Type_Exists"
    Public Const ACListExistsProc As Boolean = True

    Public Const ACGetFieldNamesSQL As String = "spu_GIS_Field_Names_Get"
    Public Const ACGetFieldNamesName As String = "spu_GIS_Field_Names_Get"
    Public Const ACGetFieldNamesProc As Boolean = True

    Public Const ACCreatePMLookupSQL As String = "spu_gis_list_pm_caption"
    Public Const ACCreatePMLookupName As String = "spu_gis_list_pm_caption"
    Public Const ACCreatePMLookupProc As Boolean = True

    Public Const ACCreatePMLookupUpdateSQL As String = "spu_gis_list_pm_caption_update"
    Public Const ACCreatePMLookupUpdateName As String = "spu_gis_list_pm_caption_update"
    Public Const ACCreatePMLookupUpdateProc As Boolean = True

    Public Const ACListEntryAddSQL As String = "spu_gis_listentry_add"
    Public Const ACListEntryAddName As String = "spu_gis_listentry_add"
    Public Const ACListEntryAddProc As Boolean = True

    Public Const ACListEntryUpdateSQL As String = "spu_gis_listentry_update"
    Public Const ACListEntryUpdateName As String = "spu_gis_listentry_update"
    Public Const ACListEntryUpdateProc As Boolean = True

    Public Const ACDelListItemSQL As String = "spu_GIS_List_Item_del"
    Public Const ACDelListItemName As String = "spu_GIS_List_Item_del"
    Public Const ACDelListItemProc As Boolean = True

    Public Const ACGetListItemSQL As String = "spu_GIS_List_Item_get"
    Public Const ACGetListItemName As String = "spu_GIS_List_Item_get"
    Public Const ACGetListItemProc As Boolean = True

    Public Const ACUpdateListItemSQL As String = "spu_GIS_List_Item_upd"
    Public Const ACUpdateListItemName As String = "spu_GIS_List_Item_upd"
    Public Const ACUpdateListItemProc As Boolean = True

    Public Const ACUpdatePMLookupSQL As String = "spu_GIS_upd_pm_lookup"
    Public Const ACUpdatePMLookupName As String = "spu_GIS_upd_pm_lookup"
    Public Const ACUpdatePMLookupProc As Boolean = True

    Public Const ACGetUDLDataSQL As String = "spu_UDL_Data_sel"
    Public Const ACGetUDLDataName As String = "spu_UDL_Data_sel"
    Public Const ACGetUDLDataProc As Boolean = True

    Public Const ACUpdateUDLDataSQL As String = "spu_UDL_Data_upd"
	Public Const ACUpdateUDLDataName As String = "spu_UDL_Data_upd"
    Public Const ACUpdateUDLDataProc As Boolean = True

    Public Const ACUpdateUDLVersionSQL As String = "spu_UDL_Version_upd"
    Public Const ACUpdateUDLVersionName As String = "spu_UDL_Version_upd"
    Public Const ACUpdateUDLVersionProc As Boolean = True

    Public Const ACGetMaxUDLVersionSQL As String = "spu_Get_UDL_Max_Version"
    Public Const ACGetMaxUDLVersionName As String = "spu_Get_UDL_Max_Version"
    Public Const ACGetMaxUDLVersionProc As Boolean = True

    Public Const ACGetGISUDLDetailSQL As String = "spu_GIS_List_TypeUDL_Exists"
    Public Const ACGetGISUDLDetailName As String = "spu_GIS_List_TypeUDL_Exists"
    Public Const ACGetGISUDLDetailProc As Boolean = True

	

	
	'   SP 28/06/02 - Commented out as use bFunc logMessage instead
	'Public Sub LogMessage( _
	''    iType As Integer, sMsg As String, Optional vApp As Variant, _
	''    Optional vClass As Variant, Optional vMethod As Variant, _
	''    Optional vErrNo As Variant, Optional vErrDesc As Variant)
	'
	'Dim lReturn As Long
	'Dim oMessage As Object
	'
	'    ' CTAF 270701
	'    On Error Resume Next
	'
	'    ' Create an instance of the message object
	'    Set oMessage = CreateObject("iPMMessage.PMMessageV2")
	'
	'    ' CTAF 270701
	'    On Error GoTo Err_LogMessage
	'
	'    If ((oMessage Is Nothing) = False) Then
	'
	'        ' Log the message
	'        lReturn& = oMessage.LogMessage( _
	''                        iType:=iType, _
	''                        sMsg:=sMsg, _
	''                        vApp:=vApp, _
	''                        vClass:=vClass, _
	''                        vMethod:=vMethod, _
	''                        vErrNo:=vErrNo, _
	''                        vErrDesc:=vErrDesc)
	'        If (lReturn& <> PMTrue) Then
	'            ' If it fails, then
	'            LogMessagePopup _
	''                iType:=iType%, _
	''                sMsg:=sMsg$, _
	''                vApp:=vApp, _
	''                vClass:=vClass, _
	''                vMethod:=vMethod, _
	''                vErrNo:=vErrNo, _
	''                vErrDesc:=vErrDesc
	'        End If
	'
	'        Set oMessage = Nothing
	'
	'    Else
	'
	'        ' CTAF 270701 - Log the message as normal instead
	'
	'        ' Failed to log message, so we must call the
	'        ' function to popup the message instead.
	'        LogMessagePopup _
	''            iType:=iType%, _
	''            sMsg:=sMsg$, _
	''            vApp:=vApp, _
	''            vClass:=vClass, _
	''            vMethod:=vMethod, _
	''            vErrNo:=vErrNo, _
	''            vErrDesc:=vErrDesc
	'
	'    End If
	'
	'
	'    Exit Sub
	'
	'Err_LogMessage:
	'
	'    ' Error Section.
	'
	'    ' Failed to log message, so we must call the
	'    ' function to popup the message instead.
	'    LogMessagePopup _
	''        iType:=iType%, _
	''        sMsg:=sMsg$, _
	''        vApp:=vApp, _
	''        vClass:=vClass, _
	''        vMethod:=vMethod, _
	''        vErrNo:=vErrNo, _
	''        vErrDesc:=vErrDesc
	'
	'    Exit Sub
	'
	'End Sub
	'
End Module