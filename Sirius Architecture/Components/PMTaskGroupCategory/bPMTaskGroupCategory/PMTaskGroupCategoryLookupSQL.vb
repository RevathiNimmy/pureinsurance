Option Strict Off
Option Explicit On
Imports System
Module PMTaskGroupLookupSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: PMTaskGroupLookupSQL
	'
	' Date: 22 October 1998
	'
	' Description: Contains the SQL/Stored Procedures used by the
	'              Lookup class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Group Category SQL
	Public Const ACTaskGroupCategoryLookupStored As Boolean = True
	Public Const ACTaskGroupCategoryLookupName As String = "TaskGroupCategoryLicence"
	Public Const ACTaskGroupCategoryLookupSQL As String = "{call spu_pmwrk_inst_category_sel (?)}"
End Module