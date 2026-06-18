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
	' Date: 07/05/1999
	'
	' Description: Contains the SQL Statements required by the
	'              bSIRMailshot.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	'DC270303 -ISS1911
	' Create Tables TO Hold Risks & Group Include Or Exclude For Reports
	'Public Const ACCreateReportTablesStored = True
	'Public Const ACCreateReportTablesName = "CreateReportTables"
	'Public Const ACCreateReportTablesSQL = "{call spu_create_report_risk_and_group_tables}"
    'developer guide no.39
    'start
	' Insert Into Included Risks Table
	Public Const ACInsertIncludedRisksStored As Boolean = True
	Public Const ACInsertIncludedRisksName As String = "InsertIncludedRisks"
    Public Const ACInsertIncludedRisksSQL As String = "spu_insert_included_risks"
	
	' Insert Into Excluded Risks Table
	Public Const ACInsertExcludedRisksStored As Boolean = True
	Public Const ACInsertExcludedRisksName As String = "InsertExcludedRisks"
    Public Const ACInsertExcludedRisksSQL As String = "spu_insert_excluded_risks"
	
	' Insert Into Included Risk Groups Table
	Public Const ACInsertIncludedRiskGroupsStored As Boolean = True
	Public Const ACInsertIncludedRiskGroupsName As String = "InsertIncludedRisks"
    Public Const ACInsertIncludedRiskGroupsSQL As String = "spu_insert_included_risk_groups"
	
	' Insert Into Excluded Risk Groups Table
	Public Const ACInsertExcludedRiskGroupsStored As Boolean = True
	Public Const ACInsertExcludedRiskGroupsName As String = "InsertExcludedRisks"
    Public Const ACInsertExcludedRiskGroupsSQL As String = "spu_insert_excluded_risk_groups"
	
	' Insert Into Excluded Risk Groups Table
	Public Const ACInsertGroupByStored As Boolean = True
	Public Const ACInsertGroupByName As String = "InsertGroupBy"
    Public Const ACInsertGroupBySQL As String = "spu_insert_report_grouping"
	
	'DC270303 -ISS1911
	' Delete From Temporary Report Tables
	Public Const ACDeleteReportTempRecordsStored As Boolean = True
	Public Const ACDeleteReportTempRecordsName As String = "InsertIncludedRisks"
    Public Const ACDeleteReportTempRecordsSQL As String = "spu_delete_temp_report_records"
	
	'DC270303 -ISS1911
	''eck030702
	'' Create Tables To Hold Users For reports
	'Public Const ACCreateReportUserTableStored = True
	'Public Const ACCreateReportUserTableName = "CreateReportUserTable"
	'Public Const ACCreateReportUserTableSQL = "{call spu_create_report_user_table}"
	
	' Insert Into User Table
	Public Const ACInsertReportUserStored As Boolean = True
	Public Const ACInsertReportUserName As String = "InsertReportUser"
    Public Const ACInsertReportUserSQL As String = "spu_insert_report_user"
	
	'DC270303 -ISS1911
	Public Const ACGetNextReportSessionStored As Boolean = True
	Public Const ACGetNextReportSessionName As String = "GetNextSessionId"
    Public Const ACGetNextReportSessionSQL As String = "spu_pm_session_id_alloc"
	
	'DC270303 -ISS1911
	Public Const ACClearReportSessionStored As Boolean = True
	Public Const ACClearReportSessionName As String = "ClearSessionId"
    Public Const ACClearReportSessionSQL As String = "spu_pm_session_id_free"
	
	' Insert Into Temp Report Excluded Table
	Public Const ACInsertExcludedTypeStored As Boolean = True
	Public Const ACInsertExcludedTypeName As String = "InsertExcludedType"
    Public Const ACInsertExcludedTypeSQL As String = "spu_insert_excluded_type"
	
	'Delete From Temp Report Exclude Table
	Public Const ACDeleteTempReportExcludeRecordsStored As Boolean = True
	Public Const ACDeleteTempReportExcludeRecordsName As String = "DeletedExcludedTypes"
    Public Const ACDeleteTempReportExcludeRecordsSQL As String = "spu_delete_temp_report_exclude_records"
    'end
	Public Const ACSelectPartyStored As Boolean = True
	Public Const ACSelectPartyName As String = "ACSelectPartyName"
	Public Const ACSelectPartyNameSQL As String = "spu_Get_Party_Name"
	
	Public Const ACSelectBranchStored As Boolean = True
	Public Const ACSelectBranchName As String = "Select branches for User"
	Public Const ACSelectBranchSQL As String = "spu_pm_get_user_sources"
	
	' Get Third Party Visibility
	Public Const ACGetTPVisibleStored As Boolean = True
	Public Const ACGetTPVisibleName As String = "GetTPVisibility"
    Public Const ACGetTPVisibleSQL As String = "spu_get_TP_visiblity"

    'PM029917
    Public Const ACGetClaimDataStored As Boolean = True
    Public Const ACGetClaimDataName As String = "GetClaimData"
    Public Const ACGetClaimDataSQL As String = "spu_Report_Claim_Payments_Deductible_PLICO"

    Public Const ACGetPMUserSourceStored As Boolean = True
    Public Const ACGetPMUserSourceName As String = "GetPMUserSource"
    Public Const ACGetPMUserSourceSQL As String = "spu_get_pmuser_source_for_report"
End Module