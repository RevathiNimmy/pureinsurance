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
	
	
	Public Const ACApp As String = "bGISListGrouping"
	Private Const ACClass As String = "MainModule"
	
	
	' SQL Statements
	' CTAF 20020627 - Converted to SPU
	Public Const ACGetSummarySQL As String = "{call spu_GIS_List_Group_Summary(?)}"
	Public Const ACGetSummaryName As String = "GetListSummary"
	Public Const ACGetSummaryStored As Boolean = True
	' CTAF 20020627 - Converted to SPU
	Public Const ACGetItemsSummarySQL As String = "{call spu_GIS_List_Group_Items_Summary(?,?)}"
	Public Const ACGetItemsSummaryName As String = "GetItemsSummary"
	Public Const ACGetItemsSummaryStored As Boolean = True
	' CTAF 20020627 - Converted to SPU
	Public Const ACGetListItemsSQL As String = "{call spu_GIS_Get_List_Group_Items(?,?,?)}"
	Public Const ACGetListItemsName As String = "GetListItems"
	Public Const ACGetListItemsStored As Boolean = True
	' CTAF 20020627 - Converted to SPU
	Public Const ACAutoGroupSQL As String = "{call spu_GIS_Auto_Group_Items(?,?)}"
	Public Const ACAutoGroupName As String = "AutoGroupItems"
	Public Const ACAutoGroupStored As Boolean = True
	' CTAF 20020627 - Converted to SPU
	Public Const ACAddGroupingSQL As String = "{call spu_GIS_Gis_List_Grouping_Add(?,?,?,?,?)}"
	Public Const ACAddGroupingName As String = "AddListGrouping"
	Public Const ACAddGroupingStored As Boolean = True
	
	Public Const ACListArrayID As Integer = 0
	Public Const ACListArrayCode As Integer = 1
	Public Const ACListArrayDescription As Integer = 2
	Public Const ACListArraySelected As Integer = 3
	
	Public Const ACGroupingArrayID As Integer = 0
	Public Const ACGroupingArrayCode As Integer = 1
	Public Const ACGroupingArrayDescription As Integer = 2
	Public Const ACGroupingArrayIsDeleted As Integer = 3
	Public Const ACGroupingArrayItemsCnt As Integer = 4
End Module