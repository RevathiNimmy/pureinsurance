Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' Class Name: BusinessSQL
	'
	' Date: 18/12/2000
	'
	' Description: Contains the SQL Statements required by the
	'              bSIRReportGroup.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select All Reports SQL
	Public Const ACSelectAllReportStored As Boolean = True
	Public Const ACSelectAllReportName As String = "SelectAllReports"
    Public Const ACSelectAllReportSQL As String = "spu_report_saa"
	
	' Select Report Group Contents SQL
	Public Const ACSelectReportGroupStored As Boolean = True
	Public Const ACSelectReportGroupName As String = "SelectReportGroup"
    Public Const ACSelectReportGroupSQL As String = "spu_reportgroup_contents_saa"
	
	' Delete Report Group Contents SQL
	Public Const ACDeleteReportGroupStored As Boolean = True
	Public Const ACDeleteReportGroupName As String = "DeleteReportGroup"
    Public Const ACDeleteReportGroupSQL As String = "spu_reportgroup_del"
	'
	' Insert Report Group Contents SQL
	Public Const ACInsertReportGroupStored As Boolean = True
	Public Const ACInsertReportGroupName As String = "InsertReportGroup"
    Public Const ACInsertReportGroupSQL As String = "spu_reportgroup_ins"
	
	' Select All PMUser Groups SQL
	Public Const ACSelectAllPMUserGroupStored As Boolean = True
	Public Const ACSelectAllPMUserGroupName As String = "SelectAllPMUserGroups"
    Public Const ACSelectAllPMUserGroupSQL As String = "spu_PMUserGroup_saa"
	
	' Select Report Group User Groups SQL
	Public Const ACSelectReportGroupUserGroupStored As Boolean = True
	Public Const ACSelectReportGroupUserGroupName As String = "SelectReportGroupUserGroups"
    Public Const ACSelectReportGroupUserGroupSQL As String = "spu_reportgroup_usergroups_saa"
	
	' Delete Report Group User Groups SQL
	Public Const ACDeleteReportGroupUserGroupStored As Boolean = True
	Public Const ACDeleteReportGroupUserGroupName As String = "DeleteReportGroupUserGroups"
    Public Const ACDeleteReportGroupUserGroupSQL As String = "spu_reportgroup_usergroups_del"
	
	' Insert Report Group User Groups SQL
	Public Const ACInsertReportGroupUserGroupStored As Boolean = True
	Public Const ACInsertReportGroupUserGroupName As String = "InsertReportGroupUserGroups"
    Public Const ACInsertReportGroupUserGroupSQL As String = "spu_reportgroup_usergroups_ins"
End Module