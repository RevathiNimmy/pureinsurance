Option Strict Off
Option Explicit On
Imports System
Module PMLookupSQL
	
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
	' RAW 04/09/2003 : CQ258 : added extra param to spu_accumulation_by_level_saa
	' ***************************************************************** '
	
	'SQL Statements
	
	' Select PMLookup By ID SQL
	Public Const ACSelectByIDStored As Boolean = False
	Public Const ACSelectByIDName As String = "SelectGISLookupByID"
	Public Const ACSelectByIDSQL As String = "SELECT GD.GIS_user_def_detail_id, cap.caption, GD.code " &  _
	                                         "FROM GIS_user_def_detail GD, pmcaption cap " &  _
	                                         "WHERE GD.GIS_user_def_detail_id = {ID} " &  _
	                                         "AND GD.caption_id = cap.caption_id " &  _
	                                         "AND cap.language_id = {Language_ID}"
	
	' Select PMLookup All SQL
	Public Const ACSelectAllStored As Boolean = True
	Public Const ACSelectAllName As String = "SelectAccumulationByLevelAll"
	' RAW 04/09/2003 : CQ258 : increase params from 3 to 4
	Public Const ACSelectAllSQL As String = "{call spu_accumulation_by_level_saa (?,?,?,?)}"
	
	' Get Effective ID From Code
	Public Const ACGetFromCodeStored As Boolean = True
	Public Const ACGetFromCodeName As String = "GetFromCode"
	Public Const ACGetFromCodeSQL As String = "{? = call spu_pm_get_eff_id_from_code (?,?,?,?)}"
	
	' Get Effective ID From ID
	Public Const ACGetFromIDStored As Boolean = True
	Public Const ACGetFromIDName As String = "GetFromID"
	Public Const ACGetFromIDSQL As String = "{? = call spu_pm_get_eff_id_from_id (?,?,?)}"
	
	' Get Code From ID - TF200198
	Public Const ACGetCodeFromIDStored As Boolean = True
	Public Const ACGetCodeFromIDName As String = "GetCodeFromID"
	Public Const ACGetCodeFromIDSQL As String = "{call spu_pm_get_code_from_id (?,?,?)}"
End Module