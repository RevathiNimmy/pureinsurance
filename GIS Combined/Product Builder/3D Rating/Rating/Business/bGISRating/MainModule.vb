Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	Public Const ACApp As String = "bGISRating"
	Public Const ACClass As String = "MainModule"
    'developer guide no.39
    'start

    Public Const ACGetRateTypeTableSQL As String = "spu_GIS_Get_Rate_Type"
	Public Const ACGetRateTypeTableName As String = "GetGisRateTypeTable"
	Public Const ACGetRateTypeTableStored As Boolean = True
	

    Public Const ACGetListTypes As String = "spu_GIS_Get_Rate_Type_List"

    Public Const ACRateInUse As String = "spu_GIS_Get_Rate_Type_Count" 'PN16559

    Public Const ACGetAxes As String = "spu_GIS_Get_Axis_List"

    Public Const ACGetAxis As String = "spu_GIS_Get_Axis"
	

    Public Const ACAddRiskType As String = "spu_GIS_Add_Rate_type"

    Public Const ACGetMatrix As String = "spu_GIS_Get_Matrix"
    'end
End Module