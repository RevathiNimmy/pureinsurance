Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	
	' ***************************************************************** '
	' Module Name: MainModule
	' ***************************************************************** '
	
	' Main public constant for all functions to identify which application this is.
	Public Const ACApp As String = "bSIRReportScheduler"
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	' ***************************************************************** '
	'                         Report Scheduler ARRAY
	' ***************************************************************** '
	Public Enum ReportSchedulerEnum
		DBMReportSchedulerID
		DBMReportID
		DBMDescription
		DBMFrequency
		DBMExportToPDF
		DBMArchieveToPDF
		DBMExportToCSV
	End Enum

    Public DBMMax As ReportSchedulerEnum = ReportSchedulerEnum.DBMExportToCSV

    ' ***************************************************************** '
    '                      Report Scheduler Detail ARRAY
    ' ***************************************************************** '
    Public Enum ReportSchedulerDetailEnum
        DBMReportSchedulerParameterID
        DBMReportSchedulerDetailID
        DBMParameterName
        DBMDefaultValue
        DBMDataType
        DBMPrompt
        DBMCurrentIDValue
        DBMPartySearch
        DBMEmpty
        DBMIsAutomatic
    End Enum

	Public DBMDetailMax As ReportSchedulerDetailEnum = ReportSchedulerDetailEnum.DBMIsAutomatic
End Module