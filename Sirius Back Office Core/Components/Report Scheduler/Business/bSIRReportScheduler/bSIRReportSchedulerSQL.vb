Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' Class Name: BusinessSQL
	' ***************************************************************** '
	
	' Select
	Public Const ACSelectScheduledReportsStored As Boolean = True
	Public Const ACSelectScheduledReportsName As String = "Scheduled Reports Select"
	Public Const ACSelectScheduledReportsSQL As String = "spu_sir_scheduled_reports_sel"
	
	Public Const ACSelectScheduledReportsDigStored As Boolean = True
	Public Const ACSelectScheduledReportsDigName As String = "Scheduled Reports Dig"
	Public Const ACSelectScheduledReportsDigSQL As String = "spu_sir_scheduled_reports_dig"
	
	Public Const ACSelectScheduledReportsDetailStored As Boolean = True
	Public Const ACSelectScheduledReportsDetailName As String = "Scheduled Reports Detail Select"
	Public Const ACSelectScheduledReportsDetailSQL As String = "spu_sir_scheduled_reports_detail_sel"
	
	' Insert
	Public Const ACInsertScheduledReportsStored As Boolean = True
	Public Const ACInsertScheduledReportsName As String = "Insert Scheduled Reports"
	Public Const ACInsertScheduledReportsSQL As String = "spu_sir_scheduled_reports_add"
	
	Public Const ACInsertScheduledReportsDetailsStored As Boolean = True
	Public Const ACInsertScheduledReportsDetailsName As String = "Insert Scheduled Reports Details"
	Public Const ACInsertScheduledReportsDetailsSQL As String = "spu_sir_scheduled_reports_add_details"
	
	' Update
	Public Const ACUpdateScheduledReportStored As Boolean = True
	Public Const ACUpdateScheduledReportName As String = "Update Scheduled Report"
	Public Const ACUpdateScheduledReportSQL As String = "spu_sir_scheduled_reports_update"
	
	' Delete
	Public Const ACDeleteScheduledReportsStored As Boolean = True
	Public Const ACDeleteScheduledReportsName As String = "Delete Scheduled Reports"
    Public Const ACDeleteScheduledReportsSQL As String = "spu_sir_scheduled_reports_delete"

    Public Const ACSelectReportCodeStored As Boolean = True
    Public Const ACSSelectReportCodeName As String = "Report code Select"
    Public Const ACSelectReportCodeSQL As String = "spu_sir_report_code_sel"
End Module