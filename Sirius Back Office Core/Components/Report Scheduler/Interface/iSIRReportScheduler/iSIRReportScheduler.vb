Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	' ***************************************************************** '
	
	' Main public constant for all functions to identify which application this is.
	Public Const ACApp As String = "iSIRReportScheduler"
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "MainModule"
	
	'Get Res Data
	Public Const ACInterfaceCaption As Integer = 100 'Report Scheduled
	
	' Buttons
	Public Const ACViewButton As Integer = 101
	Public Const ACNewButton As Integer = 102
	Public Const ACDeleteButton As Integer = 103
	Public Const ACOKButton As Integer = 104
	Public Const ACHelpButton As Integer = 105
	Public Const ACCancelButton As Integer = 106
	Public Const ACInterfaceCaptionDetail As Integer = 107
	
	'List
	Public Const ACListTitle1 As Integer = 110 'ReportSchedulerId
	Public Const ACListTitle2 As Integer = 111 'Report Name
	Public Const ACListTitle3 As Integer = 112 'Frequency
	
	Public Const ACListDetailTitle1 As Integer = 113 'Automatic
	Public Const ACListDetailTitle2 As Integer = 114 'Parameter ID
	Public Const ACListDetailTitle3 As Integer = 115 'Parameter Name
	Public Const ACListDetailTitle4 As Integer = 116 'Parameter Type
	Public Const ACListDetailTitle5 As Integer = 117 'Parameter Value
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301

    ' SeprateBy Codes
    Public Const ACAgentShortCode As String = "AgentShortCode"
    Public Const ACSubAgentShortCode As String = "SubAgentCode"
    'SeprateBy Descriptions
    Public Const ACAgentShortDesc As String = "Agent Short Name"
    Public Const ACSubAgentShortDesc As String = "Sub Agent Short Name"

    Public Const ACAgentReportCode As String = "Agt_Sta_uy"
    Public Const ACSubAgentReportCode As String = "Sub_Agt_uy"
	''''''''''''''
	
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
        DBMReportPathCSV
        DBMSeprateBy
	End Enum

    Public DBMMax As ReportSchedulerEnum = ReportSchedulerEnum.DBMSeprateBy

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
	
	' ***************************************************************** '
	'                        GLOBAL VARIABLES
	' ***************************************************************** '
    ' Public source and language ID's from the Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
End Module