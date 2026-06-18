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
	' Class Name: BusinessSQL
	'
	' Date: 09/06/1999
	'
	' Description: Contains the SQL Statements required by the
	'              bSIRCoinsurance.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
    ' Select COI Arrangement SQL
    'developer guide no. 39
    Public Const ACSelectPerilTypeUsageStored As Boolean = True
	Public Const ACSelectPerilTypeUsageName As String = "SelectPerilTypeUsage"
    Public Const ACSelectPerilTypeUsageSQL As String = "spu_peril_type_usage_saa"
	' Delete COI Arrangement SQL
	Public Const ACDeletePerilTypeUsageStored As Boolean = True
	Public Const ACDeletePerilTypeUsageName As String = "DeletePerilTypeUsage"
    Public Const ACDeletePerilTypeUsageSQL As String = "spu_peril_type_usage_del"
	
	' Insert COI Arrangement SQL
	Public Const ACInsertPerilTypeUsageStored As Boolean = True
	Public Const ACInsertPerilTypeUsageName As String = "InsertPerilTypeUsage"
    Public Const ACInsertPerilTypeUsageSQL As String = "spe_peril_type_usage_add"

	' Insert Earning Pattern Usage SQL
	Public Const ACInsertEarningPatternUsageStored As Boolean = True
	Public Const ACInsertEarningPatternUsageName As String = "InsertEarningPatternUsage"
    Public Const ACInsertEarningPatternUsageSQL As String = "spu_set_earning_pattern_usage"

	' Select Earning Pattern Usage SQL
	Public Const ACSelectEarningPatternUsageStored As Boolean = True
	Public Const ACSelectEarningPatternUsageName As String = "SelectEarningPatternUsage"
    Public Const ACSelectEarningPatternUsageSQL As String = "spu_get_earning_pattern_usage"
    'end
End Module