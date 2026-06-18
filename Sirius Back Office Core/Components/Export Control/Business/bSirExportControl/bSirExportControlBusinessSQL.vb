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
	' Class Name: FormSQL
	'
	' Date: 02/04/1998
	'
	' Description: Contains the SQL Statements required by the
	'              bExportControl.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Get Mapping Structure SQL
	Public Const ACGetMappingStructureStored As Boolean = True
    Public Const ACGetMappingStructureName As String = "SelectRisk"
    'Developer Guide No 39
    Public Const ACGetMappingStructureSQL As String = "spu_sir_get_mapping_structure"
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module