Option Strict Off
Option Explicit On
Imports System
Module PMTaskLookupSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: PMTaskLookupSQL
	'
	' Date: 22 October 1998
	'
	' Description: Contains the SQL/Stored Procedures used by the
	'              Lookup class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	'' All Task Lookup SQL
	'Public Const ACAllTaskLookupStored = True
	'Public Const ACAllTaskLookupName = "AllTaskLookup"
	'Public Const ACAllTaskLookupSQL = "{call spu_pmusr_all_Task_lookup (?)}"
	'
	'' Group All Task Lookup SQL
	'Public Const ACGroupAllTaskLookupStored = True
	'Public Const ACGroupAllTaskLookupName = "GroupAllTaskLookup"
	'Public Const ACGroupAllTaskLookupSQL = "{call spu_pmusr_group_lookup (?,?)}"
End Module