Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
Module MainModule
	
	' This Type corresponds to the one in the OLE Server
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_atypEventRecords() As cPMEventLogViewer.cEventLogs.EventRecord = Nothing
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_typUserFilterData As New cPMEventLogViewer.cEventLogs.FilterData

	Public Const ACApp As String = "PMEventLogViewer"
	
	' Constants for different types of events
	Public Const Event_Type_Info As String = "Information"
	Public Const Event_Type_Warning As String = "Warning"
	Public Const Event_Type_Error As String = "Error"
	Public Const Event_Type_Success_Audit As String = "Audit Success"
	Public Const Event_Type_Failure_Audit As String = "Audit Failure"
	
	Public Const EVENTLOG_ERROR_TYPE As Integer = &H1s
	Public Const EVENTLOG_WARNING_TYPE As Integer = &H2s
	Public Const EVENTLOG_INFORMATION_TYPE As Integer = &H4s
	Public Const EVENTLOG_AUDIT_SUCCESS As Integer = &H8s
	Public Const EVENTLOG_AUDIT_FAILURE As Integer = &H10s
	
	' Constants for different types of event filters
	Public Const Filter_Type_None As Integer = 0
	Public Const Filter_Type_TimeBefore As Integer = 1
	Public Const Filter_Type_TimeAfter As Integer = 2
	Public Const Filter_Type_EventType As Integer = 3
	Public Const Filter_Type_Source As Integer = 4
	Public Const Filter_Type_Category As Integer = 5
	Public Const Filter_Type_Computer As Integer = 6
	Public Const Filter_Type_EventID As Integer = 7
	
	' Error numbers possibly returned from EventLog Server
	Public Const ERR_LOGTYPE_NOT_SET As Integer = 1011 + Constants.vbObjectError
	Public Const ERR_SOURCENAME_NOT_SET As Integer = 1012 + Constants.vbObjectError
	Public Const ERR_BAD_INDEX As Integer = 1013 + Constants.vbObjectError
	Public Const ERR_FAILED_OPEN_REGISTRY_KEY As Integer = 1014 + Constants.vbObjectError
	Public Const ERR_FAILED_READ_REGISTRY_KEY As Integer = 1015 + Constants.vbObjectError
	Public Const ERR_RESOURCE_DATA_NOT_FOUND As Integer = 1016 + Constants.vbObjectError
	Public Const ERR_READING_EVENT_LOG As Integer = 1017 + Constants.vbObjectError
	Public Const ERR_LOG_NOT_OPENED As Integer = 1018 + Constants.vbObjectError
	Public Const ERR_FAILED_SET_LOG_TYPE As Integer = 1019 + Constants.vbObjectError
	

	Public Sub Main()
		
		
        Dim oInterface As New frmInterface
        'developer guide no. 68
        oInterface.ShowDialog()
		
		oInterface.Close()
		
		oInterface = Nothing
		
	End Sub
End Module