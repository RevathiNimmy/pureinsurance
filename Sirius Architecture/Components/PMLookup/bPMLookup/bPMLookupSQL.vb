Option Strict Off
Option Explicit On
Imports System
Module PMLookupSQL
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
	' ***************************************************************** '
	
	'SQL Statements
	
	' Select PMLookup By ID SQL
	Public Const ACSelectByIDStored As Boolean = False
	Public Const ACSelectByIDName As String = "SelectPMLookupByID"
	' RAW 15/07/2003 : CQ258 : added is_deleted column
	'Public Const ACSelectByIDSQL = "SELECT tn.{Table_Name}_id, cap.caption, tn.code, tn.is_deleted FROM {Table_Name} tn, pmcaption cap WHERE tn.{Table_Name}_id = {ID} AND tn.caption_id = cap.caption_id and cap.language_id = {Language_ID}"
	' RVH 28/08/2003 : Changed statement to support NULL values as the "caption_id" in a lookup table - in these circumstances, the lookup table description field is used. The reasons this was done was to support the use of SQL Views in place of a physical lookup table.
	Public Const ACSelectByIDSQL As String = "SELECT  tn.{Table_Name}_id, rtrim(isnull(cap.caption, tn.description)) as 'caption', tn.code, tn.is_deleted FROM {Table_Name} tn LEFT OUTER JOIN pmcaption cap on tn.caption_id = cap.caption_id and and (cap.language_id = {Language_ID}) WHERE tn.{Table_Name}_id = {ID}"
	
	' RDC 04102001 START: two versions - non-ordered for broking, ordered for underwriting
	' Select PMLookup All SQL non-ordered
	Public Const ACSelectAllStored As Boolean = False
	Public Const ACSelectAllName As String = "SelectPMLookupAll"
	
	' RAW 15/07/2003 : CQ258 : added is_deleted column and removed where is_deleted test
	'Public Const ACSelectAllSQL = "SELECT tn.{Table_Name}_id, cap.caption, tn.code, tn.is_deleted FROM {Table_Name} tn, pmcaption cap WHERE tn.effective_date <= {Effective_Date} AND tn.caption_id = cap.caption_id and cap.language_id = {Language_ID}"
	' RVH 28/08/2003 : Changed statement to support NULL values as the "caption_id" in a lookup table - in these circumstances, the lookup table description field is used. The reasons this was done was to support the use of SQL Views in place of a physical lookup table.
	Public Const ACSelectAllSQL As String = "SELECT tn.{Table_Name}_id, rtrim(isnull(cap.caption, tn.description)) as 'caption', tn.code, tn.is_deleted FROM {Table_Name} tn LEFT OUTER JOIN pmcaption cap on tn.caption_id = cap.caption_id and (cap.language_id = {Language_ID}) WHERE tn.effective_date <= {Effective_Date}"
    Public Const ACSelectAllUDLSQL As String = "SELECT tn.{Table_Name}_id, rtrim(isnull(cap.caption, tn.description)) as 'caption', tn.code, tn.is_deleted FROM {Table_Name} tn LEFT OUTER JOIN pmcaption cap on tn.caption_id = cap.caption_id and (cap.language_id = {Language_ID}) WHERE 1 = 1 "

	Public Const ACSelectSystemModeStored As Boolean = False
	Public Const ACSelectSystemModeName As String = "SelectSystemMode"
	Public Const ACSelectSystemModeSQL As String = "SELECT value FROM hidden_options WHERE branch_id=1 and option_number=1"
    'developer guide no. 40 (Guide)
	' RDC 15102001 START
	Public Const ACCheckHiddenOptionsStored As Boolean = False
	Public Const ACCheckHiddenOptionsName As String = "CheckHiddenOptions"
	Public Const ACCheckHiddenOptionsSQL As String = "if exists(select name from sysobjects where id = object_id('hidden_options')) select 1 as table_exists else select 0 as table_exists"
	' RDC 15102001 END
	' RDC 0410200 END
	
	' Get Effective ID From Code
	Public Const ACGetFromCodeStored As Boolean = True
	Public Const ACGetFromCodeName As String = "GetFromCode"
    Public Const ACGetFromCodeSQL As String = "spu_pm_get_eff_id_from_code"
	
	' Get Effective ID From ID
	Public Const ACGetFromIDStored As Boolean = True
	Public Const ACGetFromIDName As String = "GetFromID"
  
    Public Const ACGetFromIDSQL As String = "spu_pm_get_eff_id_from_id"
	
	' Get Code From ID - TF200198
	Public Const ACGetCodeFromIDStored As Boolean = True
	Public Const ACGetCodeFromIDName As String = "GetCodeFromID"
 
    Public Const ACGetCodeFromIDSQL As String = "spu_pm_get_code_from_id"
	
	' RAM20040305 : Code changes related to Caching, this SQL Script is similar to
	'               ACSelectAllStored but
	'               1. with out the effective date filter
	'               2. Fetch the effective date field as well
	'               3. Get the Caption Table Caption, if it is null, get tn.description, if that too null, get tn.code
	' RAM20040323 : 4. Cast the ID into String, since dPMDAO returns ID as string before
	'               5. Cast the is_deleted into String
	' RAM20040514 : Changed the embedded SQL into Stored Procedure.
	'               Note: New stored procedure 'spu_Get_PMLookups_For_Caching' is now introduced
	'               Resson being, some lookups may have User Defined fields, and if
	'               those fields are used in the where clause, then the old SQL won't
	'               work, But now with this Stored procedure being it is generic and it will
	'               cater for any number of fields.
	Public Const ACSelectLookupTableToCacheStored As Boolean = True
	Public Const ACSelectLookupTableToCacheName As String = "SelectLookupTableToCache"
	Public Const ACSelectLookupTableToCacheSQL As String = "EXEC spu_Get_PMLookups_For_Caching '{Table_Name}', {Language_ID}, {Is_Underwriting}"
End Module
