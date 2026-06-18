Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports System.Text
<System.Runtime.InteropServices.ProgId("cEventLogs_NET.cEventLogs")> _
Public NotInheritable Class cEventLogs 
	' ************************************************************
	' Copyright © 1995-2001 Slightly Tilted Software
	' All rights reserved
	' You're absolutely free to use these resources within your
	'     own applications, but you may not redistribute them
	'     (as source) in any manner whatsoever, whether for profit
	'     or not.
	' The only legitimate source for the original source code is
	'     at the VBPJ site and my own web site at:
	'     http://www.SlightlyTiltedSoftware.com
	' ************************************************************
	
	' ---------------------------------------------
	' Module    : EventLogs.cls
	' By        : L.J. Johnson       Date: 04-28-2001
	' Comments  : Wrapper for API to read event log entries
	' ---------------------------------------------
	
	' ************************************************
	' Added to replace global variables 18/09/2003
	' Username.
	Private m_sUsername As String = ""
	
	' Password.
	Private m_sPassword As String = ""
	
	' User ID
	Private m_iUserID As Integer
	
	' Calling Application
	Private m_sCallingAppName As String = ""
	' Source ID
	Private m_iSourceID As Integer
	' Language ID
	Private m_iLanguageID As Integer
	' Currency ID
	Private m_iCurrencyID As Integer
	' LogLevel
	Private m_iLogLevel As Integer
	' ************************************************
	
	
	' -------------------------------------------------
	' Type of item to look up in the registry
	' -------------------------------------------------
	Private Const EventMessage As Integer = 0
    Private Const ParameterMessage As Integer = 1
    Private Const iMaxLogCount As Integer = 500
	
	' -------------------------------------------------
	'
	' -------------------------------------------------
	Public Structure FilterData
		Dim EvtInfo As Boolean
		Dim EvtWarning As Boolean
		Dim EvtError As Boolean
		Dim EvtSuccessAudit As Boolean
		Dim EvtFailureAudit As Boolean
		Dim SourceInclude As Boolean
		Dim Source1 As String
		Dim Source2 As String
		Dim Source3 As String
		Dim CategoryInclude As Boolean
		Dim Category1 As String
		Dim Category2 As String
		Dim Category3 As String
		Dim EventIdInclude As Boolean
		Dim EventID As Integer
		Dim EventDateInclude As Boolean
		Dim EventDateFrom As Date
		Dim EventDateTo As Date
		Public Shared Function CreateInstance() As FilterData
			Dim result As New FilterData
			result.Source1 = String.Empty
			result.Source2 = String.Empty
			result.Source3 = String.Empty
			result.Category1 = String.Empty
			result.Category2 = String.Empty
			result.Category3 = String.Empty
			Return result
		End Function
	End Structure
	Private m_typEventFilter As FilterData = FilterData.CreateInstance()
	
	' -------------------------------------------------
	' Save the filter and filter type
	' -------------------------------------------------
	Private m_lngFilterType As Integer
	Private m_vntFilter As Object
	Private m_blnEventReadLogForward As Boolean
	Private m_blnEventDataReturnHex As Boolean
	
	' -------------------------------------------------
	' Share the module-level event log handle
	' -------------------------------------------------
	Private m_lngEventLogHwnd As Integer = &HFFFFs
	
	' -------------------------------------------------
	' Used to save the library names in a static array
	'     in the ResourceString method
	' -------------------------------------------------
	Private Structure Source_Lib_Type
		Dim SourceName As String
		Dim LibName As String
		Public Shared Function CreateInstance() As Source_Lib_Type
			Dim result As New Source_Lib_Type
			result.SourceName = String.Empty
			result.LibName = String.Empty
			Return result
		End Function
	End Structure
	
	' -------------------------------------------------
	' Not an API declaration -- this is the UDT that
	'   stores the info on each event record
	' -------------------------------------------------
	Public Structure EventRecord
		Dim EventTimeWritten As Date
		Dim EventTimeCreated As Date
		Dim EventSourceName As String
		'EventUserName                       As String
		Dim EventUserSID As String
		Dim EventComputerName As String
		Dim EventType As String
		Dim EventDescription As String
		Dim EventData As String
		Dim EventDataText As String
		Dim EventCategory As Integer
		Dim EventCategoryString As String
		Dim EventRecordNum As Integer
		Dim EventID As Integer
		Public Shared Function CreateInstance() As EventRecord
			Dim result As New EventRecord
			result.EventSourceName = String.Empty
			result.EventUserSID = String.Empty
			result.EventComputerName = String.Empty
			result.EventType = String.Empty
			result.EventDescription = String.Empty
			result.EventData = String.Empty
			result.EventDataText = String.Empty
			result.EventCategoryString = String.Empty
			Return result
		End Function
	End Structure
	Private m_atypEventRecords() As EventRecord = Nothing
	
	' --------------------------------------------
	' This is the API type declaration
	' --------------------------------------------
	Public Structure EVENTLOGRECORD
		Dim Length As Integer
		Dim Reserved As Integer
		Dim RecordNumber As Integer
		Dim TimeGenerated As Integer
		Dim TimeWritten As Integer
		Dim EventID As Integer
		Dim EventType As Integer
		Dim NumStrings As Integer
		Dim EventCategory As Integer
		Dim ReservedFlags As Integer
		Dim ClosingRecordNumber As Integer
		Dim StringOffset As Integer
		Dim UserSidLength As Integer
		Dim UserSidOffset As Integer
		Dim DataLength As Integer
		Dim DataOffset As Integer
	End Structure
	
	' -------------------------------------------------
	' Used as part of error string return
	' -------------------------------------------------
	Private Const EVENT_SOURCENAME As String = "ReadEventLogs.EventLogs."
	Private Const FORMAT_SOURCENAME As String = "ReadEventLogs.FormatMsg."
	
	' -------------------------------------------------
	' For keeping track of our own error info
	' -------------------------------------------------
	Private m_lngErrNumber As Integer
	Private m_strErrSource As String = ""
	Private m_strErrDesc As String = ""
	
	' -------------------------------------------------
	' Used to pass info within the class module
	' -------------------------------------------------
	Private m_strTypeEventLog As String = ""
	Private m_lngCount As Integer
	
	' -------------------------------------------------
	' Used to get time/date info from event records
	' -------------------------------------------------
	Private Structure SYSTEMTIME
		Dim wYear As Integer
		Dim wMonth As Integer
		Dim wDayOfWeek As Integer
		Dim wDay As Integer
		Dim wHour As Integer
		Dim wMinute As Integer
		Dim wSecond As Integer
		Dim wMilliseconds As Integer
		Private Function GetDateTime() As DateTime
			Return New DateTime(wYear, wMonth, wDay, wHour, wMinute, wSecond, wMilliseconds)
		End Function
		Public Sub LoadDateTime(ByVal dt As DateTime)
			wYear = dt.Year
			wMonth = dt.Month
			wDayOfWeek = CInt(dt.DayOfWeek)
			wDay = dt.Day
			wHour = dt.Hour
			wMinute = dt.Minute
			wSecond = dt.Second
			wMilliseconds = dt.Millisecond
		End Sub
        Public Sub GetLocalTime()
            LoadDateTime(DateTime.Now)
        End Sub
		Public Sub GetSystemTime()
			LoadDateTime(DateTime.UtcNow)
		End Sub
	End Structure
	
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Private Structure TIME_ZONE_INFORMATION
		Dim Bias As Integer
        '<VBFixedString(32),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=32)> _
        'NIIT - Replaced with the Migrated code 1144 
        'Public StandardName As FixedLengthString
        Private StandardName As String
		Dim StandardDate As SYSTEMTIME
		Dim StandardBias As Integer
        '<VBFixedString(32),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=32)> _
        'NIIT - Replaced with the Migrated code 1144 
        'Public DaylightName As FixedLengthString
        Private DaylightName As String
		Dim DaylightDate As SYSTEMTIME
		Dim DaylightBias As Integer
		Private Function StringToShortArray(ByVal value As String) As Integer()
			Dim result() As Integer
			ReDim result(value.Length())
			Dim i As Integer = 0
			
			While i < value.Length
				result(i) = Strings.AscW(value(i))
				i += 1
			End While
			
			Return result
		End Function
		Public Function GetTimeZoneInformation() As Integer
			Dim tzi As TimeZone = TimeZone.CurrentTimeZone
			
			Bias = DateTime.UtcNow.Subtract(DateTime.Now).Minutes
            'developer guide no.
            StandardName = tzi.StandardName
            StandardDate.LoadDateTime(tzi.GetDaylightChanges(DateTime.Now.Year).End)
            'developer guide no.
            DaylightName = tzi.DaylightName
			DaylightDate.LoadDateTime(tzi.GetDaylightChanges(DateTime.Now.Year).Start)
			If tzi.IsDaylightSavingTime(DateTime.Now) Then
				DaylightBias = DateTime.UtcNow.Subtract(DateTime.Now).Minutes
				StandardBias = DaylightBias - tzi.GetDaylightChanges(DateTime.Now.Year).Delta.Minutes
				Return 2
			Else
				StandardBias = DateTime.UtcNow.Subtract(DateTime.Now).Minutes
				DaylightBias = StandardBias + tzi.GetDaylightChanges(DateTime.Now.Year).Delta.Minutes
				Return 1
			End If
		End Function
	End Structure
	
	' -------------------------------------------------
	' Used with LoadLibraryEx
	' -------------------------------------------------
	Private Const DONT_RESOLVE_DLL_REFERENCES As Integer = 1
	Private Const LOAD_LIBRARY_AS_DATAFILE As Integer = 2
	Private Const LOAD_WITH_ALTERED_SEARCH_PATH As Integer = 8
	
	' -------------------------------------------------
	'
	' -------------------------------------------------
	Private Const FORMAT_MESSAGE_IGNORE_INSERTS As Integer = &H200
	Private Const FORMAT_MESSAGE_FROM_HMODULE As Integer = &H800
	
	Public Enum enmEventType
		EVENTLOG_SUCCESS = &H0
		EVENTLOG_ERROR_TYPE = &H1
		EVENTLOG_WARNING_TYPE = &H2
		EVENTLOG_INFORMATION_TYPE = &H4
		EVENTLOG_AUDIT_SUCCESS = &H8
		EVENTLOG_AUDIT_FAILURE = &H10
	End Enum
	
	' -------------------------------------------------
	' 32-bit API declares
	' -------------------------------------------------
	Private Declare Function OpenEventLog Lib "advapi32"  Alias "OpenEventLogA"(ByVal lpUNCServerName As String, ByVal lpEventSourceName As String) As Integer
    Private Declare Function CloseEventLog Lib "advapi32" Alias "CloseEventLog" (ByVal hEventLog As IntPtr) As Integer
    Private Declare Unicode Function OpenBackupEventLog Lib "advapi32" Alias "OpenBackupEventLogW" (ByVal lpUNCServerName As String, ByVal lpFileName As String) As IntPtr
	Private Declare Function GetNumberOfEventLogRecords Lib "advapi32.dll" (ByVal hEventLog As Integer, ByRef NumberOfRecords As Integer) As Integer
    Private Declare Ansi Function ReadEventLog Lib "advapi32.dll" Alias "ReadEventLogA" (ByVal hEventLog As IntPtr, ByVal dwReadFlags As Integer, ByVal dwRecordOffset As Integer, ByVal lpBuffer As IntPtr, ByVal nNumberOfBytesToRead As Integer, ByRef pnBytesRead As Integer, ByRef pnMinNumberOfBytesNeeded As Integer) As Integer
	Private Declare Function LoadLibraryEx Lib "Kernel32"  Alias "LoadLibraryExA"(ByVal lpLibFileName As String, ByVal hFile As Integer, ByVal dwFlags As Integer) As Integer
	Private Declare Function LoadLibrary Lib "Kernel32"  Alias "LoadLibraryA"(ByVal lpLibFileName As String) As Integer
	Private Declare Function FreeLibrary Lib "Kernel32" (ByVal hLibModule As Integer) As Integer
	Private Declare Function ExpandEnvironmentStrings Lib "Kernel32"  Alias "ExpandEnvironmentStringsA"(ByVal lpSrc As String, ByVal lpDst As String, ByVal nSize As Integer) As Integer
	
	' -------------------------------------------------
	' Defines for the READ flags for Eventlogging
	' -------------------------------------------------
	Private Const EVENTLOG_SEQUENTIAL_READ As Integer = &H1s
	Private Const EVENTLOG_SEEK_READ As Integer = &H2s
	Private Const EVENTLOG_FORWARDS_READ As Integer = &H4s
	Private Const EVENTLOG_BACKWARDS_READ As Integer = &H8s
	
	Public WriteOnly Property EventFilter() As FilterData
		Set(ByVal Value As FilterData)
			m_typEventFilter = Value
		End Set
	End Property
	
	' ***********************************************************************
	' ***********************************************************************
	' ****                     Read/Write Properties                     ****
	' ***********************************************************************
	' ***********************************************************************
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Property Get EventTimeWritten
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 05/29/2001 -- 12:45:48
	' Inputs       : ByVal xi_lngIndex:Long -
	' Outputs      : DATE:
	' Description  : READ-ONLY: Return the Time Written for an event log
	'              :     record (based on passed index value)
	'              : Check for zero values or values greater than
	'              :     the number of log entries in the filtered
	'              :     result set.
	' *******************************************************
	Public ReadOnly Property EventTimeWritten(ByVal xi_lngIndex As Integer) As Date
		Get
			
			Dim result As Date = DateTime.FromOADate(0)
			Select Case xi_lngIndex
				Case 0, Is > m_lngCount
					m_lngErrNumber = (ERR_BAD_INDEX + Constants.vbObjectError)
					m_strErrSource = EVENT_SOURCENAME & "EventTimeWritten"
					m_strErrDesc = "The passed index number is not valid."
					result = CDate(String.Empty)
					Throw New System.Exception(m_lngErrNumber.ToString() + ", " + m_strErrSource + ", " + m_strErrDesc)
			End Select
			
			Return m_atypEventRecords(xi_lngIndex).EventTimeWritten
			
		End Get
	End Property
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Property Get EventTimeCreated
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 05/29/2001 -- 12:44:56
	' Inputs       : ByVal xi_lngIndex:Long -
	' Outputs      : DATE:
	' Description  : READ-ONLY: Return the Time Created an event log
	'              :     record (based on passed index value)
	'              : Check for zero values or values greater than
	'              :     the number of log entries in the filtered
	'              :     result set.
	' *******************************************************
	Public ReadOnly Property EventTimeCreated(ByVal xi_lngIndex As Integer) As Date
		Get
			
			Dim result As Date = DateTime.FromOADate(0)
			Select Case xi_lngIndex
				Case 0, Is > m_lngCount
					m_lngErrNumber = (ERR_BAD_INDEX + Constants.vbObjectError)
					m_strErrSource = EVENT_SOURCENAME & "EventTimeWritten"
					m_strErrDesc = "The passed index number is not valid."
					result = CDate(String.Empty)
					Throw New System.Exception(m_lngErrNumber.ToString() + ", " + m_strErrSource + ", " + m_strErrDesc)
			End Select
			
			Return m_atypEventRecords(xi_lngIndex).EventTimeCreated
			
		End Get
	End Property
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Property Get EventSourceName
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 05/29/2001 -- 12:44:01
	' Inputs       : ByVal xi_lngIndex:Long -
	' Outputs      : STRING:
	' Description  : READ-ONLY: Return the event Source Name of an event log
	'              :     record (based on passed index value)
	'              : Check for zero values or values greater than
	'              :     the number of log entries in the filtered
	'              :     result set.
	' *******************************************************
	Public ReadOnly Property EventSourceName(ByVal xi_lngIndex As Integer) As String
		Get
			
			Dim result As String = String.Empty
			Select Case xi_lngIndex
				Case 0, Is > m_lngCount
					m_lngErrNumber = (ERR_BAD_INDEX + Constants.vbObjectError)
					m_strErrSource = EVENT_SOURCENAME & "EventSourceName"
					m_strErrDesc = "The passed index number is not valid."
					result = Nothing
					Throw New System.Exception(m_lngErrNumber.ToString() + ", " + m_strErrSource + ", " + m_strErrDesc)
			End Select
			
			Return m_atypEventRecords(xi_lngIndex).EventSourceName
			
		End Get
	End Property
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Property Get EventUserSID
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 05/29/2001 -- 12:43:05
	' Inputs       : ByVal xi_lngIndex:Long -
	' Outputs      : STRING:
	' Description  : READ-ONLY: Return the user's SID of an event log
	'              :     record (based on passed index value)
	'              : Check for zero values or values greater than
	'              :     the number of log entries in the filtered
	'              :     result set.
	' *******************************************************
	Public ReadOnly Property EventUserSID(ByVal xi_lngIndex As Integer) As String
		Get
			
			Dim result As String = String.Empty
			Select Case xi_lngIndex
				Case 0, Is > m_lngCount
					m_lngErrNumber = (ERR_BAD_INDEX + Constants.vbObjectError)
					m_strErrSource = EVENT_SOURCENAME & "EventUserName"
					m_strErrDesc = "The passed index number is not valid."
					result = Nothing
					Throw New System.Exception(m_lngErrNumber.ToString() + ", " + m_strErrSource + ", " + m_strErrDesc)
			End Select
			
			Return m_atypEventRecords(xi_lngIndex).EventUserSID
			
		End Get
	End Property
	
	'' -------------------------------------------------
	'' Return the Event User Name of an event log
	''   record (based on passed index value)
	'' Check for zero values or values greater than
	''   the number of log entries in the filtered
	''   result set.
	'' -------------------------------------------------
	'Public Property Get EventUserName(ByVal xi_lngIndex As Long) As String
	'
	'   Select Case xi_lngIndex
	'      Case 0, Is > m_lngCount
	'         m_lngErrNumber = (ERR_BAD_INDEX + vbObjectError)
	'         m_strErrSource = EVENT_SOURCENAME & "EventUserName"
	'         m_strErrDesc = "The passed index number is not valid."
	'         EventUserName = vbNullString
	'         Err.Raise m_lngErrNumber, m_strErrSource, m_strErrDesc
	'   End Select
	'
	'   EventUserName = m_atypEventRecords(xi_lngIndex).EventUserName
	'
	'End Property
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Property Get EventComputerName
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 05/29/2001 -- 12:41:58
	' Inputs       : ByVal xi_lngIndex:Long -
	' Outputs      : STRING:
	' Description  : READ-ONLY: Return the Event Computer Name of an event log
	'              :     record (based on passed index value)
	'              : Check for zero values or values greater than
	'              :     the number of log entries in the filtered
	'              :     result set.
	' *******************************************************
	Public ReadOnly Property EventComputerName(ByVal xi_lngIndex As Integer) As String
		Get
			
			Dim result As String = String.Empty
			Select Case xi_lngIndex
				Case 0, Is > m_lngCount
					m_lngErrNumber = (ERR_BAD_INDEX + Constants.vbObjectError)
					m_strErrSource = EVENT_SOURCENAME & "EventComputerName"
					m_strErrDesc = "The passed index number is not valid."
					result = Nothing
					Throw New System.Exception(m_lngErrNumber.ToString() + ", " + m_strErrSource + ", " + m_strErrDesc)
			End Select
			
			Return m_atypEventRecords(xi_lngIndex).EventComputerName
			
		End Get
	End Property
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Property Get EventType
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 05/29/2001 -- 12:40:43
	' Inputs       : ByVal xi_lngIndex:Long -
	' Outputs      : STRING:
	' Description  : READ-ONLY:  Return the Event Type of an event log
	'              :     record (based on passed index value)
	'              : Check for zero values or values greater than
	'              :     the number of log entries in the filtered
	'              :     result set.
	' *******************************************************
	Public ReadOnly Property EventType(ByVal xi_lngIndex As Integer) As String
		Get
			
			Dim result As String = String.Empty
			Select Case xi_lngIndex
				Case 0, Is > m_lngCount
					m_lngErrNumber = (ERR_BAD_INDEX + Constants.vbObjectError)
					m_strErrSource = EVENT_SOURCENAME & "EventType"
					m_strErrDesc = "The passed index number is not valid."
					result = Nothing
					Throw New System.Exception(m_lngErrNumber.ToString() + ", " + m_strErrSource + ", " + m_strErrDesc)
			End Select
			
			Return m_atypEventRecords(xi_lngIndex).EventType
			
		End Get
	End Property
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Property Get EventDescription
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 05/29/2001 -- 12:39:14
	' Inputs       : ByVal xi_lngIndex:Long -
	' Outputs      : STRING
	' Description  : READ-ONLY:  Return the Event Description of an event log
	'              :     record (based on passed index value)
	'              : Check for zero values or values greater than
	'              :     the number of log entries in the filtered
	'              :     result set.
	' *******************************************************
	Public ReadOnly Property EventDescription(ByVal xi_lngIndex As Integer) As String
		Get
			
			Dim result As String = String.Empty
			Select Case xi_lngIndex
				Case 0, Is > m_lngCount
					m_lngErrNumber = (ERR_BAD_INDEX + Constants.vbObjectError)
					m_strErrSource = EVENT_SOURCENAME & "EventDescription"
					m_strErrDesc = "The passed index number is not valid."
					result = Nothing
					Throw New System.Exception(m_lngErrNumber.ToString() + ", " + m_strErrSource + ", " + m_strErrDesc)
			End Select
			
			Return m_atypEventRecords(xi_lngIndex).EventDescription
			
		End Get
	End Property
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Property Get EventCategoryString
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 05/29/2001 -- 12:38:05
	' Inputs       : ByVal xi_lngIndex:Long -
	' Outputs      : LONG:
	' Description  : READ-ONLY:  Return the Event Category of an event log
	'              :     record (based on passed index value)
	'              : Check for zero values or values greater than
	'              :     the number of log entries in the filtered
	'              :     result set.
	' *******************************************************
	Public ReadOnly Property EventCategoryString(ByVal xi_lngIndex As Integer) As String
		Get
			
			Dim result As String = String.Empty
			Select Case xi_lngIndex
				Case 0, Is > m_lngCount
					m_lngErrNumber = (ERR_BAD_INDEX + Constants.vbObjectError)
					m_strErrSource = EVENT_SOURCENAME & "EventCategoryString"
					m_strErrDesc = "The passed index number is not valid."
					result = CStr(0)
					Throw New System.Exception(m_lngErrNumber.ToString() + ", " + m_strErrSource + ", " + m_strErrDesc)
			End Select
			
			Return m_atypEventRecords(xi_lngIndex).EventCategoryString
			
		End Get
	End Property
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Property Get EventCategory
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 05/29/2001 -- 12:38:05
	' Inputs       : ByVal xi_lngIndex:Long -
	' Outputs      : LONG:
	' Description  : READ-ONLY:  Return the Event Category of an event log
	'              :     record (based on passed index value)
	'              : Check for zero values or values greater than
	'              :     the number of log entries in the filtered
	'              :     result set.
	' *******************************************************
	Public ReadOnly Property EventCategory(ByVal xi_lngIndex As Integer) As Integer
		Get
			
			Dim result As Integer = 0
			Select Case xi_lngIndex
				Case 0, Is > m_lngCount
					m_lngErrNumber = (ERR_BAD_INDEX + Constants.vbObjectError)
					m_strErrSource = EVENT_SOURCENAME & "EventCategory"
					m_strErrDesc = "The passed index number is not valid."
					result = 0
					Throw New System.Exception(m_lngErrNumber.ToString() + ", " + m_strErrSource + ", " + m_strErrDesc)
			End Select
			
			Return m_atypEventRecords(xi_lngIndex).EventCategory
			
		End Get
	End Property
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Property Get EventRecordNum
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 05/29/2001 -- 12:37:14
	' Inputs       : ByVal xi_lngIndex:Long -
	' Outputs      : N/A
	' Description  : READ-ONLY:  Return the Event Record Number of an event log
	'              :     record (based on passed index value)
	'              : Check for zero values or values greater than
	'              :     the number of log entries in the filtered
	'              :     result set.
	' *******************************************************
	Public ReadOnly Property EventRecordNum(ByVal xi_lngIndex As Integer) As Integer
		Get
			
			Dim result As Integer = 0
			Select Case xi_lngIndex
				Case 0, Is > m_lngCount
					m_lngErrNumber = (ERR_BAD_INDEX + Constants.vbObjectError)
					m_strErrSource = EVENT_SOURCENAME & "EventRecordNum"
					m_strErrDesc = "The passed index number is not valid."
					result = 0
					Throw New System.Exception(m_lngErrNumber.ToString() + ", " + m_strErrSource + ", " + m_strErrDesc)
			End Select
			
			Return m_atypEventRecords(xi_lngIndex).EventRecordNum
			
		End Get
	End Property
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Property Get EventID
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 05/29/2001 -- 12:36:04
	' Inputs       : ByVal xi_lngIndex:Long -
	' Outputs      : LONG:
	' Description  : READ-ONLY: Return the Event ID of an event log
	'              :     record (based on passed index value)
	'              : Check for zero values or values greater than
	'              :     the number of log entries in the filtered
	'              :     result set.
	' *******************************************************
	Public ReadOnly Property EventID(ByVal xi_lngIndex As Integer) As Integer
		Get
			
			Dim result As Integer = 0
			Select Case xi_lngIndex
				Case 0, Is > m_lngCount
					m_lngErrNumber = (ERR_BAD_INDEX + Constants.vbObjectError)
					m_strErrSource = EVENT_SOURCENAME & "EventID"
					m_strErrDesc = "The passed index number is not valid."
					result = 0
					Throw New System.Exception(m_lngErrNumber.ToString() + ", " + m_strErrSource + ", " + m_strErrDesc)
			End Select
			
			Return m_atypEventRecords(xi_lngIndex).EventID
			
		End Get
	End Property
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Property Get EventData
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 05/29/2001 -- 12:34:44
	' Inputs       : ByVal xi_lngIndex:Long -
	' Outputs      : LONG:
	' Description  : READ-ONLY: Return the Event DATA of an event log
	'              :     record (based on passed index value)
	'              : Check for zero values or values greater than
	'              :     the number of log entries in the filtered
	'              :     result set.
	' *******************************************************
	Public ReadOnly Property EventData(ByVal xi_lngIndex As Integer) As String
		Get
			
			Dim result As String = String.Empty
			Select Case xi_lngIndex
				Case 0, Is > m_lngCount
					m_lngErrNumber = (ERR_BAD_INDEX + Constants.vbObjectError)
					m_strErrSource = EVENT_SOURCENAME & "EventData"
					m_strErrDesc = "The passed index number is not valid."
					result = Nothing
					Throw New System.Exception(m_lngErrNumber.ToString() + ", " + m_strErrSource + ", " + m_strErrDesc)
			End Select
			
			Return m_atypEventRecords(xi_lngIndex).EventData
			
		End Get
	End Property
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Property Get EventDataText
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 05/29/2001 -- 12:34:44
	' Inputs       : ByVal xi_lngIndex:Long -
	' Outputs      : LONG:
	' Description  : READ-ONLY: Return the Event DATA (text, not HEX) of an event log
	'              :     record (based on passed index value)
	'              : Check for zero values or values greater than
	'              :     the number of log entries in the filtered
	'              :     result set.
	' *******************************************************
	Public ReadOnly Property EventDataText(ByVal xi_lngIndex As Integer) As String
		Get
			
			Dim result As String = String.Empty
			Select Case xi_lngIndex
				Case 0, Is > m_lngCount
					m_lngErrNumber = (ERR_BAD_INDEX + Constants.vbObjectError)
					m_strErrSource = EVENT_SOURCENAME & "EventDataText"
					m_strErrDesc = "The passed index number is not valid."
					result = Nothing
					Throw New System.Exception(m_lngErrNumber.ToString() + ", " + m_strErrSource + ", " + m_strErrDesc)
			End Select
			
			Return m_atypEventRecords(xi_lngIndex).EventDataText
			
		End Get
	End Property
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Property Get EventTypeLog
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 05/29/2001 -- 12:20:42
	' Inputs       : N/A
	' Outputs      :
	' Description  : Get/Let the Event Log Type property
	' *******************************************************
	
	Public Property EventTypeLog() As String
		Get
			Return m_strTypeEventLog
		End Get
		Set(ByVal Value As String)
			m_strTypeEventLog = Value
		End Set
	End Property
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Property Get CountEventRecords
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 05/29/2001 -- 12:18:32
	' Inputs       : N/A
	' Outputs      : N/A
	' Description  : READ-ONLY -- Returns the number of
	'              :     event records
	' *******************************************************
	Public ReadOnly Property CountEventRecords() As Integer
		Get
			Return m_lngCount
		End Get
	End Property
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Property Let EventReadLogForward
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 05/29/2001 -- 12:17:43
	' Inputs       : ByVal xi_blnDirToRead:Boolean -
	' Outputs      : N/A
	' Description  : Set the direction to read the log -- forwards or
	'              :     backwards.  Defaults to backwards, like the
	'              :     NT Event Viewer
	' *******************************************************
	
	Public Property EventReadLogForward() As Boolean
		Get
			Return m_blnEventReadLogForward
		End Get
		Set(ByVal Value As Boolean)
			m_blnEventReadLogForward = Value
		End Set
	End Property
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Property Get EventFilter
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 05/29/2001 -- 12:15:25
	' Inputs       : xi_lngType:Long -
	' Outputs      : VARIANT:
	' Description  : Get/Let the filter for the event log -- if invalid,
	'              :     set to no filter (get back all the event log
	'              :     records)
	' *******************************************************
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Property Let EventFilter
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 05/30/2001 -- 10:48:45
	' Inputs       : ByVal xi_lngType:Long -
	'              : ByVal xi_vntFilter:Variant -
	' Outputs      : N/A
	' Description  :
	'              :
	'              :
	' *******************************************************
	Public Property xEventFilter(ByVal xi_lngType As Integer) As Object
		Get
			xi_lngType = m_lngFilterType
			Return m_vntFilter
		End Get
		Set(ByVal Value As Object)

			'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
			'Try  ' Don't accept an error here
				'
				'   Select Case CLng(xi_lngType)
				'      Case 0
				'         ' ----------------------------------------
				'         ' No filter
				'         ' ----------------------------------------
				'         m_lngFilterType = 0
				'         m_vntFilter = vbNullString
				'      Case Filter_Type_TimeBefore, Filter_Type_TimeAfter
				'         ' ----------------------------------------
				'         ' Passed a date/time parameter
				'         ' ----------------------------------------
				'         If IsDate(xi_vntFilter) = True Then
				'            m_lngFilterType = xi_lngType
				'            m_vntFilter = xi_vntFilter
				'         Else
				'            ' -------------------------------------
				'            ' Passed incorrect date format -- set
				'            '   to no filter
				'            ' -------------------------------------
				'            m_lngFilterType = 0
				'            m_vntFilter = vbNullString
				'         End If
				'      Case Filter_Type_EventType
				'         ' ----------------------------------------
				'         ' Passed an EventType parameter -- make
				'         '   sure it is a valid parameter
				'         ' If not valid, set to no filter
				'         ' ----------------------------------------
				'         Select Case xi_vntFilter
				'            Case EVENTLOG_ERROR_TYPE, _
				''                    EVENTLOG_WARNING_TYPE, _
				''                    EVENTLOG_INFORMATION_TYPE, _
				''                    EVENTLOG_AUDIT_SUCCESS, _
				''                    EVENTLOG_AUDIT_FAILURE
				'               m_lngFilterType = xi_lngType
				'               m_vntFilter = xi_vntFilter
				'            Case Else
				'               ' ----------------------------------
				'               ' Passed incorrect event type -- set
				'               '   to no filter
				'               ' ----------------------------------
				'               m_lngFilterType = 0
				'               m_vntFilter = vbNullString
				'         End Select
				'      Case Filter_Type_Source, Filter_Type_Computer
				'         ' ----------------------------------------
				'         ' Passed a Source or Computer Name (text
				'         '   string)
				'         ' ----------------------------------------
				'         m_lngFilterType = xi_lngType
				'         m_vntFilter = xi_vntFilter
				'      Case Filter_Type_Category
				'         ' ----------------------------------------
				'         ' Passed a Category (Integer)
				'         ' ----------------------------------------
				'         m_lngFilterType = xi_lngType
				'         m_vntFilter = xi_vntFilter
				'      Case Filter_Type_EventID
				'         ' ----------------------------------------
				'         ' Passed an EventID (Long)
				'         ' ----------------------------------------
				'         m_lngFilterType = xi_lngType
				'         m_vntFilter = xi_vntFilter
				'      Case Else
				'         ' ----------------------------------------
				'         ' Passed incorrect filter type -- set to
				'         '   no filter
				'         ' ----------------------------------------
				'         m_lngFilterType = 0
				'         m_vntFilter = vbNullString
				'   End Select
			'
			'Catch exc As System.Exception
				'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
			'End Try
			
		End Set
	End Property
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Property Let EventDataReturnHex
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 05/29/2001 -- 12:09:14
	' Inputs       : ByVal xi_blnDataReturnHex:Boolean -
	' Outputs      : N/A
	' Description  : Set whether or not to return HEX values for the
	'              :     data -- will only effect data over 256 bytes in
	'              :     length (ie, if set to FALSE, the default, then
	'              :     data 256 bytes or less will *still* be returned
	'              :     as hex values separated by spaces)
	' *******************************************************
	
	Public Property EventDataReturnHex() As Boolean
		Get
			Return m_blnEventDataReturnHex
		End Get
		Set(ByVal Value As Boolean)
			m_blnEventDataReturnHex = Value
		End Set
	End Property
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Sub OpenAnyEventLog
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 05/29/2001 -- 12:30:03
	' Inputs       : ByVal xi_strServerName:String -
	' Outputs      : N/A
	' Description  : This method must be called before the
	'              :     ReadEventEntries method.
	'              : It opens the event log type (previously set via
	'              :     the EventTypeLog property) on the NT computer
	'              :     passed to the method
	' *******************************************************
	Public Sub OpenAnyEventLog(ByVal xi_strServerName As String)

		Dim resume2 As Boolean = True
		Try  ' Raise your own errors
			
			' ----------------------------------------------
			' Make sure that the Computer Name is trimmed
			'   and in UNC format
			' ----------------------------------------------
			xi_strServerName = xi_strServerName.Trim()
			
			If xi_strServerName.Length = 0 Then
				' -------------------------------------------
				' Do nothing -- can't put '\\' in front of
				'   a blank string
				' -------------------------------------------
				
			ElseIf xi_strServerName.StartsWith("\\") Then 
				' -------------------------------------------
				' Do nothing -- already has '\\' in front
				'   of server of UNC convention
				' -------------------------------------------
				
			ElseIf xi_strServerName.StartsWith("\") Then 
				xi_strServerName = "\" & xi_strServerName
				
			Else
				xi_strServerName = "\\" & xi_strServerName
			End If
			
			' ----------------------------------------------
			' See if have an invalid EventLog Type
			' ----------------------------------------------
			If m_strTypeEventLog.Length = 0 Or m_strTypeEventLog = CStr(&HFFFFs) Then
				m_lngEventLogHwnd = 0
				
				m_lngErrNumber = (ERR_LOG_NOT_OPENED + Constants.vbObjectError)
				m_strErrSource = EVENT_SOURCENAME & "OpenAnyEventLog"
				m_strErrDesc = "Error opening the event log: " &  _
				               "EventType not has not been set."

				resume2 = False
                MessageBox.Show(m_strErrDesc + m_strErrDesc, "Error Opening Log", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
                'Throw New System.Exception(m_lngErrNumber.ToString() + ", " + m_strErrSource + ", " + m_strErrDesc)
			End If
			
			' ----------------------------------------------
			' Open the event log
			' ----------------------------------------------
			Dim p_lngEventLogHwnd As Integer = OpenEventLog(xi_strServerName, m_strTypeEventLog)
			
			' ----------------------------------------------
			' Check for any errors
			' ----------------------------------------------
			If p_lngEventLogHwnd = 0 Then
				m_lngEventLogHwnd = 0
				
				m_lngErrNumber = (ERR_LOG_NOT_OPENED + Constants.vbObjectError)
				m_strErrSource = EVENT_SOURCENAME & "OpenAnyEventLog"
				m_strErrDesc = "Error opening the event log, " &  _
				               m_strTypeEventLog & ", on the Server, " & xi_strServerName &  _
				               ": " & ReturnApiErrString(Information.Err().LastDllError)

				resume2 = False
                Throw New System.Exception(m_lngErrNumber.ToString() + ", " + m_strErrSource + ", " + m_strErrDesc)

			Else
				m_lngEventLogHwnd = p_lngEventLogHwnd
			End If
		
		Catch exc As System.Exception
            MessageBox.Show(m_strErrDesc, "Error Opening Log", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
			If Not resume2 Then
				Throw exc
			End If
		End Try
		
	End Sub
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Sub ReadEventEntries
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 05/29/2001 -- 12:32:03
	' Inputs       : Optional ByVal xi_blnFilterEvents:Boolean = False -- Whether to filter events in the DLL or not
	'              : Optional ByVal xi_lngNumRecsToRead:Long = -1      -- Number of records to read
	' Outputs      : Long: Number of actual records returned
	' Description  : This is the heart of the OLE server.  It reads
	'              :     the selected event log on the selected NT
	'              :     server, and makes calls to all the private
	'              :     properties and functions
	' *******************************************************
    Public Function ReadEventEntries(Optional ByVal xi_blnFilterEvents As Boolean = False, Optional ByVal xi_lngNumRecsToRead As Integer = -1) As Integer
        Try
            Dim p_lngCount As Integer = 0
            Dim p_lngNumRecords As Integer ' Total number of records in this event log
            Dim eventlog1 As New System.Diagnostics.EventLog("PUREApplicationLog")
            Dim eventlog2 As New System.Diagnostics.EventLog("Application")
            Dim p_typEventRec As EventRecord = EventRecord.CreateInstance()
            'eventlog1.Source = EVENT_SOURCENAME
            Dim entry As EventLogEntry
            Dim nEntryLogLastCount As Integer = 0
            If eventlog1.Entries.Count + eventlog2.Entries.Count > iMaxLogCount Then
                p_lngNumRecords = iMaxLogCount
            Else
                p_lngNumRecords = eventlog1.Entries.Count + eventlog2.Entries.Count
            End If

            m_lngCount = eventlog1.Entries.Count + eventlog2.Entries.Count

            If p_lngNumRecords >= 0 Then
                ReDim m_atypEventRecords(p_lngNumRecords - 1)

                For Each entry In eventlog1.Entries
                    If GetRecordData(xi_typEventRecAPI:=entry, xi_blnFilterEvents:=xi_blnFilterEvents, xio_typEventRec:=p_typEventRec) Then
                        p_lngCount += 1
                        If p_lngCount > p_lngNumRecords Then
                            nEntryLogLastCount = p_lngCount - 1
                            Exit For
                        End If
                        'm_atypEventRecords(p_lngCount) = p_typEventRec
                        m_atypEventRecords(p_lngCount - 1) = p_typEventRec
                    End If
                Next

                If nEntryLogLastCount = 0 Then
                    For Each entry In eventlog2.Entries
                        If GetRecordData(xi_typEventRecAPI:=entry, xi_blnFilterEvents:=xi_blnFilterEvents, xio_typEventRec:=p_typEventRec) Then
                            p_lngCount += 1
                            If p_lngCount > p_lngNumRecords Then
                                Exit For
                            End If

                            m_atypEventRecords(p_lngCount - 1) = p_typEventRec
                        End If
                    Next
                End If

            End If


            Return p_lngNumRecords
        Catch exc As System.Exception
            Throw exc
        End Try
    End Function
    '    Public Function ReadEventEntries(Optional ByVal xi_blnFilterEvents As Boolean = False, Optional ByVal xi_lngNumRecsToRead As Integer = -1) As Integer
    '        Dim result As Integer = 0

    '        Dim resume2 As Boolean = True
    '        Try  ' Raise your own errors
    '            Dim p_lngCount As Integer
    '            Dim p_abytBuffer(30000) As Byte ' Used to read the event log records
    '            Dim p_lngEventLogHwnd As Integer ' Handle to the current open event log
    '            Dim p_lngReadFlags As Integer ' Flags used to read the event log
    '            Dim p_lngNumBytesToRead As Integer ' Number of bytes TO read from event log
    '            Dim p_lngNumBytesRead As Integer ' Number of bytes actually READ from event log
    '            Dim p_lngNumRecords As Integer ' Total number of records in this event log
    '            Dim p_lngMinBytesNeeded As Integer ' Used in ReadEventLog API call
    '            Dim p_lngRtn As Integer ' Return value from ReadEventLog API call
    '            Dim p_typEventRecAPI As New EVENTLOGRECORD
    '            Dim p_typEventRec As EventRecord = EventRecord.CreateInstance()

    '            ' ----------------------------------------------
    '            ' Make sure that the event log has been opened
    '            ' ----------------------------------------------
    '            If m_lngEventLogHwnd <> 0 Then
    '                p_lngEventLogHwnd = m_lngEventLogHwnd

    '            ElseIf m_lngEventLogHwnd = &HFFFFS Then
    '                ' -------------------------------------------
    '                ' Has never been set -- see Initialize event
    '                ' -------------------------------------------
    '                m_lngErrNumber = (ERR_LOG_NOT_OPENED + Constants.vbObjectError)
    '                m_strErrSource = EVENT_SOURCENAME & "ReadEventEntries"
    '                m_strErrDesc = "The Event Log has not been opened yet.  " & _
    '                               "Use the OpenAnyEventLog property."
    '                m_lngCount = 0

    '                resume2 = False
    '                Throw New System.Exception(m_lngErrNumber.ToString() + ", " + m_strErrSource + ", " + m_strErrDesc)

    '            Else
    '                ' -------------------------------------------
    '                ' Error info set when set the Server Name
    '                ' -------------------------------------------
    '                m_lngCount = 0
    '                m_lngErrNumber = (ERR_BAD_SERVER_NAME + Constants.vbObjectError)
    '                m_strErrSource = EVENT_SOURCENAME & "ReadEventEntries"
    '                m_strErrDesc = "The Server name you entered " & _
    '                               "may not be a valid server name."

    '                resume2 = False
    '                Throw New System.Exception(m_lngErrNumber.ToString() + ", " + m_strErrSource + ", " + m_strErrDesc)
    '            End If

    '            ' ----------------------------------------------
    '            ' See how many event records are in this log
    '            ' ----------------------------------------------
    '            If API_NumberOfEventLogRecords(p_lngEventLogHwnd) Then
    '                p_lngNumRecords = CountEventRecords
    '            End If

    '            If p_lngNumRecords <= 0 Then
    '                ' -------------------------------------------
    '                ' No records -- set number of records to zero
    '                '   and exit the function (not necessarily an
    '                '   error)
    '                ' -------------------------------------------
    '                m_lngCount = 0
    '                GoTo Cleanup

    '            Else
    '                ' -------------------------------------------
    '                ' Redimension the type array to total number
    '                '   of records
    '                ' -------------------------------------------
    '                ReDim m_atypEventRecords(p_lngNumRecords - 1)

    '            End If

    '            ' ----------------------------------------------
    '            ' Set the read flags
    '            ' ----------------------------------------------
    '            If m_blnEventReadLogForward Then
    '                p_lngReadFlags = EVENTLOG_SEQUENTIAL_READ Or EVENTLOG_FORWARDS_READ
    '            Else
    '                p_lngReadFlags = EVENTLOG_SEQUENTIAL_READ Or EVENTLOG_BACKWARDS_READ
    '            End If

    '            ' ----------------------------------------------
    '            ' Initialize some variables
    '            ' ----------------------------------------------
    '            For p_lngLoop As Integer = 1 To p_lngNumRecords

    '                ' ---------------------------------------
    '                ' Bail out if only want a fixed number
    '                '     of records
    '                ' ---------------------------------------
    '                If xi_lngNumRecsToRead > 0 Then
    '                    If p_lngLoop > xi_lngNumRecsToRead Then
    '                        Exit For
    '                    End If
    '                End If

    '                Dim handle As GCHandle = GCHandle.Alloc(p_typEventRecAPI, GCHandleType.Pinned)
    '                'Dim noffset As Integer = 0
    '                'Dim handle As GCHandle = GCHandle.Alloc(p_abytBuffer)
    '                Try
    '                    Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()
    '                    'Dim tmpPtr As IntPtr = Marshal.UnsafeAddrOfPinnedArrayElement(p_abytBuffer, 0)

    '                    p_lngRtn = ReadEventLog(p_lngEventLogHwnd, p_lngReadFlags, p_lngLoop, tmpPtr, p_lngNumBytesToRead, p_lngNumBytesRead, p_lngMinBytesNeeded)
    '                    'p_lngRtn = ReadEventLog(p_lngEventLogHwnd, p_lngReadFlags, noffset, tmpPtr, 30000, p_lngNumBytesRead, p_lngMinBytesNeeded)
    '                Finally
    '                    handle.Free()
    '                End Try
    '                If p_lngRtn = 0 And Information.Err().LastDllError <> ERROR_INSUFFICIENT_BUFFER Then
    '                    Debug.WriteLine(modMain.ReturnApiErrString(Information.Err().LastDllError))
    '                ElseIf p_lngRtn = 0 And Information.Err().LastDllError = ERROR_INSUFFICIENT_BUFFER Then

    '                    ' ------------------------------------
    '                    ' DWORD align p_lngMinBytesNeeded
    '                    ' ------------------------------------
    '                    p_lngMinBytesNeeded = ((p_lngMinBytesNeeded + 3) \ 4) * 4
    '                    ReDim p_abytBuffer(p_lngMinBytesNeeded - 1)

    '                    Dim handle4 As GCHandle = GCHandle.Alloc(p_abytBuffer, GCHandleType.Pinned)
    '                    Try
    '                        Dim tmpPtr4 As IntPtr = New IntPtr(handle4.AddrOfPinnedObject().ToInt32() + Marshal.SizeOf(p_abytBuffer(0)) * 0)
    '                        If ReadEventLog(p_lngEventLogHwnd, p_lngReadFlags, p_lngLoop, tmpPtr4, p_lngMinBytesNeeded, p_lngNumBytesRead, p_lngMinBytesNeeded) = 0 Then
    '                            m_lngErrNumber = (ERR_READ_EVENT_RECORD + Constants.vbObjectError)
    '                            m_strErrSource = EVENT_SOURCENAME & "ReadEventEntries"
    '                            m_strErrDesc = "Could not read the EventLog record." & Strings.Chr(13) & Strings.Chr(10) & _
    '                                           "Error: " & modMain.ReturnApiErrString(Information.Err().LastDllError)
    '                            CloseEventLog(p_lngEventLogHwnd)

    '                            resume2 = False
    '                            Throw New System.Exception(m_lngErrNumber.ToString() + ", " + m_strErrSource + ", " + m_strErrDesc)
    '                        Else

    '                            ' ---------------------------------
    '                            ' Move to the structure
    '                            ' ---------------------------------
    '                            Dim handle2 As GCHandle = GCHandle.Alloc(p_abytBuffer, GCHandleType.Pinned)
    '                            Dim handle3 As GCHandle = GCHandle.Alloc(p_typEventRecAPI, GCHandleType.Pinned)
    '                            Try
    '                                Dim tmpPtr3 As IntPtr = handle3.AddrOfPinnedObject()

    '                                Dim tmpPtr2 As IntPtr = New IntPtr(handle2.AddrOfPinnedObject().ToInt32() + Marshal.SizeOf(p_abytBuffer(0)) * 0)

    '                                MoveMem(pTo:=tmpPtr3, uFrom:=tmpPtr2, lSize:=Marshal.SizeOf(p_typEventRecAPI))
    '                            Finally
    '                                handle2.Free()
    '                                handle3.Free()
    '                            End Try

    '                            ' ---------------------------------
    '                            ' Get the data for this record and
    '                            '     add to collection *IF* the
    '                            '     data matches the filter
    '                            ' ---------------------------------
    '                            Information.Err().Clear()
    '                            If GetRecordData(xi_lngLoop:=p_lngLoop, xi_abytBuffer:=p_abytBuffer, xi_typEventRecAPI:=p_typEventRecAPI, xi_blnFilterEvents:=xi_blnFilterEvents, xio_typEventRec:=p_typEventRec) Then
    '                                p_lngCount += 1
    '                                m_atypEventRecords(p_lngCount) = p_typEventRec
    '                            Else
    '                                ' Don't add the record
    '                            End If
    '                            If Information.Err().Number <> 0 Then
    '                                ' Don't raise an error here, just
    '                                '     keep on trucking...
    '                            End If

    '                        End If
    '                    Finally
    '                        handle4.Free()
    '                    End Try
    '                End If

    '            Next p_lngLoop

    '            ' ------------------------------------------
    '            ' Cleanup the unused values in array and
    '            '     set return value
    '            ' ------------------------------------------
    '            If p_lngCount > 0 Then
    '                ReDim Preserve m_atypEventRecords(p_lngCount - 1)
    '            Else
    '                Erase m_atypEventRecords
    '            End If

    '            result = p_lngCount

    'Cleanup:
    '            CloseEventLog(p_lngEventLogHwnd)

    '            Return result

    '        Catch exc As System.Exception
    '            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
    '            If Not resume2 Then
    '                Throw exc
    '            End If
    '        End Try
    '    End Function
	
	' *******************************************************
	' Routine Name : (PRIVATE in CLASS) Sub GetRecordData
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 05/29/2001 -- 13:48:58
	' Inputs       : ByVal xi_lngLoop:Long                  -
	'              : ByRef xi_abytBuffer():Byte             -
	'              : ByRef xi_typEventRecAPI:EVENTLOGRECORD -
	'              : ByRef xio_typEventRec:EventRecord      -
	' Outputs      : N/A
	' Description  : Get the event log record for exactly
	'              :     one record
    ' *******************************************************
    Private Function GetRecordData(ByRef xi_typEventRecAPI As EventLogEntry, ByRef xio_typEventRec As EventRecord, Optional ByVal xi_blnFilterEvents As Boolean = False) As Boolean
        Dim p_lngRecNumber As Integer
        Dim p_dteTimeGenerated, p_dteTimeWritten As Date
        Dim p_lngEventID, p_lngEventIdLong, p_lngEventType As Integer
        Dim p_lngEventCategory As Integer
        Dim p_blnShowRec As Boolean
        Dim p_strSourceName, p_strSID, p_strComputerName, p_strEventType, p_strMsg, p_strData, p_strDataText, p_strEventCategoryName As String


        'Dim p_lngLenLogRec As Integer = Marshal.SizeOf(xi_typEventRecAPI)

        With xi_typEventRecAPI

            ' ---------------------------------
            ' Get the easy stuff
            ' ---------------------------------
            p_lngRecNumber = .CategoryNumber
            p_dteTimeGenerated = .TimeWritten  ' DateToLocalDate((#1/1/1970#).AddSeconds(.TimeGenerated))
            p_dteTimeWritten = .TimeWritten  'DateToLocalDate((#1/1/1970#).AddSeconds(.TimeWritten))
            p_lngEventID = modMain.LoWord(.EventID)
            p_lngEventIdLong = .EventID
            p_lngEventType = .EntryType
            'Debug.Print "Event Type: " & p_lngEventType
            'Todo:
            'p_lngNumStrings = .NumString
            p_lngEventCategory = .CategoryNumber

            ' ---------------------------------
            ' Source is the first text after the structure
            ' ---------------------------------

            'NIIT - Replaced with the Migrated code 1144 
            'p_strSourceName = GetStrFromPtrA(VarPtr(xi_abytBuffer(p_lngLenLogRec)))
            p_strSourceName = .Source  'GetStrFromPtrA(UpgradeStubs.VBA__HiddenModule.VarPtr(xi_abytBuffer(p_lngLenLogRec)))
        End With


        ' ---------------------------------------
        ' Only do this if have a non-zero
        '     Event Category
        ' ---------------------------------------
        If p_lngEventCategory <> 0 Then
            'If p_lngEventCategory <> "(0)" Then
            p_strEventCategoryName = CategoryString(p_strSourceName, p_lngEventCategory)
            p_strEventCategoryName = "(" & p_lngEventCategory & _
                                     "): " & p_strEventCategoryName
        Else
            p_strEventCategoryName = "None"
        End If

        ' ---------------------------------
        ' Get the text string for Event Type
        ' ---------------------------------
        p_strEventType = GetEventTypeText(xi_lngEventType:=p_lngEventType)

        ' ---------------------------------------
        ' Fill in enough info to determine if
        '     we'll save this record
        ' ---------------------------------------
        With xio_typEventRec
            .EventRecordNum = p_lngRecNumber
            .EventTimeCreated = p_dteTimeGenerated
            .EventTimeWritten = p_dteTimeWritten
            .EventID = p_lngEventID
            .EventType = p_strEventType
            .EventSourceName = p_strSourceName
            .EventCategory = p_lngEventCategory
        End With

        If xi_blnFilterEvents Then
            p_blnShowRec = modFilterEvents.FilterRecord(xi_typRecord:=xio_typEventRec, xi_typUserFilterData:=m_typEventFilter)
        Else
            p_blnShowRec = True
        End If

        ' ---------------------------------------
        ' This data is more "expensive" -- no need
        '     to get if we aren't going to show it
        ' ---------------------------------------
        With xi_typEventRecAPI
            If p_blnShowRec Then

                ' ---------------------------------
                ' Computer name is next section
                ' ---------------------------------

                'NIIT - Replaced with the Migrated code 1144 
                'p_strComputerName = GetStrFromPtrA(VarPtr(xi_abytBuffer(p_lngLenLogRec + p_strSourceName.Length + 1)))
                p_strComputerName = .MachineName ' GetStrFromPtrA(UpgradeStubs.VBA__HiddenModule.VarPtr(xi_abytBuffer(p_lngLenLogRec + p_strSourceName.Length + 1)))

                ' ---------------------------------
                ' SID is next
                ' ---------------------------------
                'If .UserSidLength > 0 Then
                'If .UserName.Length > 0 Then

                '    'NIIT - Replaced with the Migrated code 1144
                '    'p_lngSID = VarPtr(xi_abytBuffer(.UserSidOffset))
                '    'p_lngSID = UpgradeStubs.VBA__HiddenModule.VarPtr(xi_abytBuffer(.UserSidOffset))
                '    'p_strSID = GetTextualSid(p_lngSID)
                '    p_strSID = .UserName
                '    'p_strUserID = GetUserNameSID(p_lngSID)
                'Else
                '    p_strSID = "<none>"
                'End If

                ' ---------------------------------
                ' Strings follow -- need to get
                '     the length here, otherwise
                '     function will truncate at
                '     first null
                ' ---------------------------------
                'p_lngStringLen = (.DataOffset - .StringOffset) + 1

                'NIIT - Replaced with the Migrated code 1144 
                'p_strStrings = GetStrFromPtrA(VarPtr(xi_abytBuffer(.StringOffset)), p_lngStringLen)
                'p_strStrings = GetStrFromPtrA(UpgradeStubs.VBA__HiddenModule.VarPtr(xi_abytBuffer(.StringOffset)), p_lngStringLen)
                'p_strMsg = ResourceString(p_strSourceName, p_lngNumStrings, p_strStrings, p_lngEventIdLong, p_lngEventID)
                p_strMsg = .Message

                ' ---------------------------------
                ' Get the data -- ditto for NULLs
                ' ---------------------------------
                p_strData = Nothing
                'p_lngStringLen = (.DataLength)
                'If .DataLength > 0 Then
                If .Data.Length > 0 Then


                    'NIIT - Replaced with the Migrated code 1144 
                    'p_abytData = UnicodeEncoding.Unicode.GetBytes(GetStrFromPtrA(VarPtr(xi_abytBuffer(.DataOffset)), p_lngStringLen))
                    'p_abytData = UnicodeEncoding.Unicode.GetBytes(GetStrFromPtrA(UpgradeStubs.VBA__HiddenModule.VarPtr(xi_abytBuffer(.DataOffset)), p_lngStringLen))
                    'p_strData = ConvertToHex(xi_lngDataLength:=.DataLength, xi_abytData:=p_abytData, xio_strText:=p_strDataText)
                    p_strData = .Data.ToString()
                Else
                    p_strData = "N/A"
                End If

            End If

        End With

        ' ---------------------------------
        ' Fill in the collection for this
        '     record
        ' ---------------------------------
        With xio_typEventRec
            .EventCategoryString = p_strEventCategoryName
            .EventComputerName = p_strComputerName
            .EventUserSID = p_strSID
            .EventDescription = p_strMsg
            .EventData = p_strData
            .EventDataText = p_strDataText
            '.EventUserName = p_strUserID
        End With

        ' ------------------------------------------
        '
        ' ------------------------------------------
        Return p_blnShowRec

    End Function
    'Private Function GetRecordData(ByVal xi_lngLoop As Integer, ByRef xi_abytBuffer() As Byte, ByRef xi_typEventRecAPI As EVENTLOGRECORD, ByRef xio_typEventRec As EventRecord, Optional ByVal xi_blnFilterEvents As Boolean = False) As Boolean
    '    Dim p_lngRecNumber As Integer
    '    Dim p_dteTimeGenerated, p_dteTimeWritten As Date
    '    Dim p_lngEventID, p_lngEventIdLong, p_lngEventType, p_lngNumStrings, p_lngEventCategory, p_lngSID, p_lngStringLen, p_lngCount As Integer
    '    Dim p_blnShowRec As Boolean
    '    Dim p_strSourceName, p_strSID, p_strUserID, p_strComputerName, p_strEventType, p_strStrings, p_strMsg, p_strData, p_strDataText, p_strEventCategoryName As String
    '    Dim p_abytData() As Byte


    '    Dim p_lngLenLogRec As Integer = Marshal.SizeOf(xi_typEventRecAPI)

    '    With xi_typEventRecAPI

    '        ' ---------------------------------
    '        ' Get the easy stuff
    '        ' ---------------------------------
    '        p_lngRecNumber = .RecordNumber
    '        p_dteTimeGenerated = DateToLocalDate((#1/1/1970#).AddSeconds(.TimeGenerated))
    '        p_dteTimeWritten = DateToLocalDate((#1/1/1970#).AddSeconds(.TimeWritten))
    '        p_lngEventID = modMain.LoWord(.EventID)
    '        p_lngEventIdLong = .EventID
    '        p_lngEventType = .EventType
    '        'Debug.Print "Event Type: " & p_lngEventType
    '        p_lngNumStrings = .NumStrings
    '        p_lngEventCategory = .EventCategory

    '        ' ---------------------------------
    '        ' Source is the first text after the structure
    '        ' ---------------------------------

    '        'NIIT - Replaced with the Migrated code 1144 
    '        'p_strSourceName = GetStrFromPtrA(VarPtr(xi_abytBuffer(p_lngLenLogRec)))
    '        p_strSourceName = GetStrFromPtrA(UpgradeStubs.VBA__HiddenModule.VarPtr(xi_abytBuffer(p_lngLenLogRec)))


    '        ' ---------------------------------------
    '        ' Only do this if have a non-zero
    '        '     Event Category
    '        ' ---------------------------------------
    '        If p_lngEventCategory <> 0 Then
    '            p_strEventCategoryName = CategoryString(p_strSourceName, p_lngEventCategory)
    '            p_strEventCategoryName = "(" & p_lngEventCategory & _
    '                                     "): " & p_strEventCategoryName
    '        Else
    '            p_strEventCategoryName = "None"
    '        End If

    '        ' ---------------------------------
    '        ' Get the text string for Event Type
    '        ' ---------------------------------
    '        p_strEventType = GetEventTypeText(xi_lngEventType:=p_lngEventType)

    '        ' ---------------------------------------
    '        ' Fill in enough info to determine if
    '        '     we'll save this record
    '        ' ---------------------------------------
    '        With xio_typEventRec
    '            .EventRecordNum = p_lngRecNumber
    '            .EventTimeCreated = p_dteTimeGenerated
    '            .EventTimeWritten = p_dteTimeWritten
    '            .EventID = p_lngEventID
    '            .EventType = p_strEventType
    '            .EventSourceName = p_strSourceName
    '            .EventCategory = p_lngEventCategory
    '        End With

    '        If xi_blnFilterEvents Then
    '            p_blnShowRec = modFilterEvents.FilterRecord(xi_typRecord:=xio_typEventRec, xi_typUserFilterData:=m_typEventFilter)
    '        Else
    '            p_blnShowRec = True
    '        End If

    '        ' ---------------------------------------
    '        ' This data is more "expensive" -- no need
    '        '     to get if we aren't going to show it
    '        ' ---------------------------------------
    '        If p_blnShowRec Then

    '            ' ---------------------------------
    '            ' Computer name is next section
    '            ' ---------------------------------

    '            'NIIT - Replaced with the Migrated code 1144 
    '            'p_strComputerName = GetStrFromPtrA(VarPtr(xi_abytBuffer(p_lngLenLogRec + p_strSourceName.Length + 1)))
    '            p_strComputerName = GetStrFromPtrA(UpgradeStubs.VBA__HiddenModule.VarPtr(xi_abytBuffer(p_lngLenLogRec + p_strSourceName.Length + 1)))

    '            ' ---------------------------------
    '            ' SID is next
    '            ' ---------------------------------
    '            If .UserSidLength > 0 Then

    '                'NIIT - Replaced with the Migrated code 1144
    '                'p_lngSID = VarPtr(xi_abytBuffer(.UserSidOffset))
    '                p_lngSID = UpgradeStubs.VBA__HiddenModule.VarPtr(xi_abytBuffer(.UserSidOffset))
    '                p_strSID = GetTextualSid(p_lngSID)
    '                'p_strUserID = GetUserNameSID(p_lngSID)
    '            Else
    '                p_strSID = "<none>"
    '            End If

    '            ' ---------------------------------
    '            ' Strings follow -- need to get
    '            '     the length here, otherwise
    '            '     function will truncate at
    '            '     first null
    '            ' ---------------------------------
    '            p_lngStringLen = (.DataOffset - .StringOffset) + 1

    '            'NIIT - Replaced with the Migrated code 1144 
    '            'p_strStrings = GetStrFromPtrA(VarPtr(xi_abytBuffer(.StringOffset)), p_lngStringLen)
    '            p_strStrings = GetStrFromPtrA(UpgradeStubs.VBA__HiddenModule.VarPtr(xi_abytBuffer(.StringOffset)), p_lngStringLen)
    '            p_strMsg = ResourceString(p_strSourceName, p_lngNumStrings, p_strStrings, p_lngEventIdLong, p_lngEventID)

    '            ' ---------------------------------
    '            ' Get the data -- ditto for NULLs
    '            ' ---------------------------------
    '            p_strData = Nothing
    '            p_lngStringLen = (.DataLength)
    '            If .DataLength > 0 Then


    '                'NIIT - Replaced with the Migrated code 1144 
    '                'p_abytData = UnicodeEncoding.Unicode.GetBytes(GetStrFromPtrA(VarPtr(xi_abytBuffer(.DataOffset)), p_lngStringLen))
    '                p_abytData = UnicodeEncoding.Unicode.GetBytes(GetStrFromPtrA(UpgradeStubs.VBA__HiddenModule.VarPtr(xi_abytBuffer(.DataOffset)), p_lngStringLen))
    '                p_strData = ConvertToHex(xi_lngDataLength:=.DataLength, xi_abytData:=p_abytData, xio_strText:=p_strDataText)
    '            Else
    '                p_strData = "N/A"
    '            End If

    '        End If

    '    End With

    '    ' ---------------------------------
    '    ' Fill in the collection for this
    '    '     record
    '    ' ---------------------------------
    '    With xio_typEventRec
    '        .EventCategoryString = p_strEventCategoryName
    '        .EventComputerName = p_strComputerName
    '        .EventUserSID = p_strSID
    '        .EventDescription = p_strMsg
    '        .EventData = p_strData
    '        .EventDataText = p_strDataText
    '        '.EventUserName = p_strUserID
    '    End With

    '    ' ------------------------------------------
    '    '
    '    ' ------------------------------------------
    '    Return p_blnShowRec

    'End Function

    ' *******************************************************
    ' Routine Name : (PRIVATE in CLASS) Function GetEventTypeText
    ' Written By   : L.J  Johnson
    ' Programmer   : L.J  Johnson [Slightly Tilted Software]
    ' Date Writen  : 05/29/2001 -- 13:15:33
    ' Inputs       : ByVal xi_lngEventType:Long -
    ' Outputs      : STRING:
    ' Description  : Return the string for the numeric
    '              :     Event Type
    ' *******************************************************
    Private Function GetEventTypeText(ByVal xi_lngEventType As Integer) As String

        Dim result As String = String.Empty
        Select Case xi_lngEventType
            Case enmEventType.EVENTLOG_SUCCESS
                result = "Success"
            Case enmEventType.EVENTLOG_ERROR_TYPE
                result = "Error"
            Case enmEventType.EVENTLOG_WARNING_TYPE
                result = "Warning"
            Case enmEventType.EVENTLOG_INFORMATION_TYPE
                result = "Information"
            Case enmEventType.EVENTLOG_AUDIT_SUCCESS
                result = "Audit Success"
            Case enmEventType.EVENTLOG_AUDIT_FAILURE
                result = "Audit Failure"
        End Select

        Return result
    End Function

    ' *******************************************************
    ' Routine Name : (PRIVATE in CLASS) Function ConvertToHex
    ' Written By   : L.J  Johnson
    ' Programmer   : L.J  Johnson [Slightly Tilted Software]
    ' Date Writen  : 05/29/2001 -- 13:50:24
    ' Inputs       : ByVal xi_lngDataLength:Long -
    '              : ByRef xi_abytData():Byte -
    ' Outputs      : STRING:
    ' Description  : Convert to hex only if less than
    '              :     256 bytes or if the user wants
    '              :     it that way
    ' *******************************************************
    Private Function ConvertToHex(ByVal xi_lngDataLength As Integer, ByRef xi_abytData() As Byte, ByRef xio_strText As String) As String
        Dim p_strText As String = ""
        Dim p_strData As New StringBuilder

        If Not m_blnEventDataReturnHex Then
            If xi_lngDataLength > 256 Then

                p_strData = New StringBuilder(StringsHelper.ByteArrayToString(xi_abytData))
            Else
                For Each xi_abytData_item As Byte In xi_abytData
                    p_strData.Append(StringsHelper.Format(xi_abytData_item.ToString("X"), "00") & " ")
                Next xi_abytData_item


                xio_strText = StringsHelper.ByteArrayToString(xi_abytData)
                xio_strText = xio_strText.Replace(Strings.Chr(0).ToString(), "")
            End If

        Else
            For Each xi_abytData_item As Byte In xi_abytData
                p_strData.Append(StringsHelper.Format(xi_abytData_item.ToString("X"), "00") & " ")
            Next xi_abytData_item


            xio_strText = StringsHelper.ByteArrayToString(xi_abytData)
            xio_strText = xio_strText.Replace(Strings.Chr(0).ToString(), "")
        End If

        Return p_strData.ToString()

    End Function

    ' *******************************************************
    ' Routine Name : (PRIVATE in CLASS) Function API_NumberOfEventLogRecords
    ' Written By   : L.J  Johnson
    ' Programmer   : L.J  Johnson [Slightly Tilted Software]
    ' Date Writen  : 05/29/2001 -- 12:52:27
    ' Inputs       : ByVal xi_lngEventLogHwnd:Long -
    ' Outputs      : N/A
    ' Description  : Get the number of records in this event log using
    '              :     the API -- note that this is not exposed by
    '              :     the object, since the "actual" number may
    '              :     changed based on the filtering applied
    ' *******************************************************
    Private Function API_NumberOfEventLogRecords(ByVal xi_lngEventLogHwnd As Integer) As Integer
        Dim p_lngNumRecords As Integer

        If GetNumberOfEventLogRecords(xi_lngEventLogHwnd, p_lngNumRecords) Then
            m_lngCount = p_lngNumRecords
            Return True
        Else
            Return False
        End If

    End Function

    ' *******************************************************
    ' Routine Name : (PRIVATE in CLASS) Function CategoryString
    ' Written By   : L.J  Johnson
    ' Programmer   : L.J  Johnson [Slightly Tilted Software]
    ' Date Writen  : 05/31/2001 -- 12:23:11
    ' Inputs       : ByVal xi_strEvtSourceName:String  -
    '              : ByVal xi_lngCategoryID:Long       -
    ' Outputs      : N/A
    ' Description  : Given the category number, return the
    '              :     category string
    ' *******************************************************
    Private Function CategoryString(ByVal xi_strEvtSourceName As String, ByVal xi_lngCategoryID As Integer) As String
        Dim result As String = String.Empty

        ' don't accept an error here
        Dim p_strApiError, p_strLibName, p_strMsgString, p_strNewLibName As String
        Dim p_lngRtn, p_lngRtnErr, p_lngPos, p_lngFoundLib As Integer
        Dim p_objRegistryDB As New cRegistryDB
        Dim p_typCategories As modFormatMsg.FmtMsgArrayType = modFormatMsg.FmtMsgArrayType.CreateInstance()

        Static p_statypLibNames() As Source_Lib_Type

        ' ----------------------------------------------
        ' Exit if passed a blank SourceName
        ' ----------------------------------------------
        If xi_strEvtSourceName.Trim().Length <= 0 Then
            Return Nothing
        End If

        ' ----------------------------------------------
        ' Get the size of the p_statypLibNames array
        ' ----------------------------------------------
        Dim p_lngNumLibNames As Integer = 0

        If p_statypLibNames IsNot Nothing Then
            p_lngNumLibNames = p_statypLibNames.GetUpperBound(0)
        End If

        ' ----------------------------------------------
        ' See if we already have the libray name in the
        '   static array
        ' ----------------------------------------------
        If p_lngNumLibNames > 0 Then
            p_lngFoundLib = False
            For p_lngLoop As Integer = 1 To p_lngNumLibNames
                If p_statypLibNames(p_lngLoop).SourceName = xi_strEvtSourceName Then
                    p_strLibName = p_statypLibNames(p_lngLoop).LibName
                    p_lngFoundLib = True
                    Exit For
                End If
            Next p_lngLoop
        Else
            p_lngFoundLib = False
        End If

        ' ----------------------------------------------
        ' Didn't find the libary name in array,
        '   so get it with the API
        ' ----------------------------------------------
        If Not p_lngFoundLib Then

            ' -------------------------------------------
            ' Get info from the registry
            ' -------------------------------------------
            p_objRegistryDB.LogType = m_strTypeEventLog
            p_objRegistryDB.SourceName = xi_strEvtSourceName

            Try
                p_objRegistryDB.RegistryEntry(xi_enmMsgType:=cRegistryDB.enmMessageLookup.CategoryMessage)
                p_strLibName = p_objRegistryDB.RegistryEntryText
            Catch ex As Exception
                ' ------------------------------------
                ' Failed to find the entry in registry,
                '     so just return a blank string
                ' ------------------------------------
                p_strMsgString = Information.Err().Description
                Return Nothing
            End Try

            ' -------------------------------------------
            ' Expand any environmental strings in the
            '     registry entry
            ' -------------------------------------------
            p_strLibName = p_strLibName.ToUpper()
            p_lngPos = (p_strLibName.IndexOf("%"c) + 1)
            If p_lngPos > 0 Then
                p_strNewLibName = New String(" "c, 256)
                p_lngRtn = ExpandEnvironmentStrings(lpSrc:=p_strLibName, lpDst:=p_strNewLibName, nSize:=p_strNewLibName.Length)
                If p_lngRtn > 0 Then
                    p_strLibName = p_strNewLibName.Substring(0, Math.Min(p_strNewLibName.Length, p_lngRtn))
                End If
            End If

            ' -------------------------------------------
            ' Add to static array
            ' -------------------------------------------
            p_lngNumLibNames += 1
            ReDim Preserve p_statypLibNames(p_lngNumLibNames - 1)
            p_statypLibNames(p_lngNumLibNames).SourceName = xi_strEvtSourceName
            p_statypLibNames(p_lngNumLibNames).LibName = p_strLibName
        End If

        ' ----------------------------------------------
        ' Load the module
        ' Note: If you use LoadLibrary, it will fail
        '       on certain libraries. You will need to use
        '       the LOAD_LIBRARY_AS_DATAFILE flag.
        ' "Use this flag when you want to load a DLL only
        '       to extract messages or resources from it"
        ' ----------------------------------------------
        Dim p_lngModHwnd As Integer = LoadLibraryEx(lpLibFileName:=p_strLibName, hFile:=0, dwFlags:=LOAD_LIBRARY_AS_DATAFILE)

        ' ----------------------------------------------
        ' Get the string from the module
        ' ----------------------------------------------
        If p_lngModHwnd > 0 Then
            p_lngRtnErr = FormatMsgFromResource(p_lngModHwnd, xi_lngCategoryID, p_typCategories, p_strMsgString, p_strApiError, False)
            If p_lngRtnErr <> 0 Then
                m_lngErrNumber = p_lngRtnErr
                m_strErrSource = FORMAT_SOURCENAME & "FormatMsgFromResource"
                m_strErrDesc = p_strApiError

                p_strMsgString = m_strErrDesc
                result = Nothing
                'Err.Raise m_lngErrNumber, m_strErrSource, m_strErrDesc
            Else
                ' ------------------------------------
                ' Got message string, don't need to
                '     do anything
                ' ------------------------------------
            End If
        Else
            p_strMsgString = Nothing
        End If

        ' ----------------------------------------------
        ' Clean up the string
        ' ----------------------------------------------
        p_strMsgString = ClearLastCrLf(p_strMsgString.Trim())

        ' ----------------------------------------------
        ' Free the module
        ' ----------------------------------------------
        FreeLibrary(p_lngModHwnd)

        ' ----------------------------------------------
        ' Set the return value
        ' ----------------------------------------------
        Return p_strMsgString


    End Function

    ' *******************************************************
    ' Routine Name : (PRIVATE in CLASS) Function ResourceString
    ' Written By   : L.J  Johnson
    ' Programmer   : L.J  Johnson [Slightly Tilted Software]
    ' Date Writen  : 05/29/2001 -- 12:28:59
    ' Inputs       : ByVal xi_strEvtSourceName:String -
    '              : ByVal xi_lngNumStrings:Long -
    '              : ByVal xi_strStrings:String -
    '              : ByVal xi_lngEventID:Long -
    '              : ByVal xi_lngEventIdForDisplay:Long -
    ' Outputs      : STRING:
    ' Description  : Private method to read the strings in
    '              :     the resource file
    ' *******************************************************
    Private Function ResourceString(ByVal xi_strEvtSourceName As String, ByVal xi_lngNumStrings As Integer, ByVal xi_strStrings As String, ByVal xi_lngEventID As Integer, ByVal xi_lngEventIdForDisplay As Integer) As String
        Dim result As String = String.Empty

        Dim resume2 As Boolean = True
        ' don't accept an error here
        Dim p_strApiError, p_strLibName, p_strMsgString, p_strNewLibName As String
        Dim p_lngRtn, p_lngRtnErr, p_lngPos, p_lngFoundLib, p_lngTransRtn As Integer
        Dim p_astrMsgArg() As String
        Dim p_objRegistryDB As New cRegistryDB
        Dim p_typMsgs As modFormatMsg.FmtMsgArrayType = modFormatMsg.FmtMsgArrayType.CreateInstance()

        Static p_statypLibNames() As Source_Lib_Type = Nothing

        ' ----------------------------------------------
        ' Exit if passed a blank SourceName
        ' ----------------------------------------------
        If xi_strEvtSourceName.Trim().Length <= 0 Then
            Return Nothing
        End If

        ' ----------------------------------------------
        ' Get the size of the p_statypLibNames array
        ' ----------------------------------------------
        Dim p_lngNumLibNames As Integer = 0
        p_lngNumLibNames = p_statypLibNames.GetUpperBound(0)

        ' ----------------------------------------------
        ' See if we already have the libray name in the
        '   static array
        ' ----------------------------------------------
        If p_lngNumLibNames > 0 Then
            p_lngFoundLib = False
            For p_lngLoop As Integer = 1 To p_lngNumLibNames
                If p_statypLibNames(p_lngLoop).SourceName = xi_strEvtSourceName Then
                    p_strLibName = p_statypLibNames(p_lngLoop).LibName
                    p_lngFoundLib = True
                    Exit For
                End If
            Next p_lngLoop
        Else
            p_lngFoundLib = False
        End If

        ' ----------------------------------------------
        ' Didn't find the libary name in array,
        '   so get it with the API
        ' ----------------------------------------------
        If Not p_lngFoundLib Then

            ' -------------------------------------------
            ' Get info from the registry
            ' -------------------------------------------
            p_objRegistryDB.LogType = m_strTypeEventLog
            p_objRegistryDB.SourceName = xi_strEvtSourceName

            p_objRegistryDB.RegistryEntry(xi_enmMsgType:=cRegistryDB.enmMessageLookup.EventMessage)
            If Information.Err().Number = 0 Then
                p_strLibName = p_objRegistryDB.RegistryEntryText
            Else
                ' ------------------------------------
                ' Failed to find the entry in registry,
                '     but rest of info may be OK
                ' Return the error message as part of
                '     the log item
                ' ------------------------------------
                p_strMsgString = Information.Err().Description

                ' ----------------------------------------
                ' NOTE:  You may want to just return a
                '        blank string, but this is the
                '        same string as is present in the
                '        Event Viewer
                ' ----------------------------------------
                p_strMsgString = p_strMsgString & Strings.Chr(13) & Strings.Chr(10) & _
                                 "The description for Event ID (" & _
                                 xi_lngEventIdForDisplay & ") " & _
                                 "in Source (" & xi_strEvtSourceName.Trim() & _
                                 ") could not be found. The local computer may not " & _
                                 "have the necessary registry information or " & _
                                 "message DLL files to display messages. The following " & _
                                 "information is part of the event: " & Strings.Chr(13) & Strings.Chr(10) & _
                                 xi_strStrings
                Return p_strMsgString
            End If

            ' -------------------------------------------
            ' Replace environmental string(s) if any
            ' -------------------------------------------
            p_strLibName = p_strLibName.ToUpper()
            p_lngPos = (p_strLibName.IndexOf("%"c) + 1)
            If p_lngPos > 0 Then
                p_strNewLibName = New String(" "c, 256)
                p_lngRtn = ExpandEnvironmentStrings(p_strLibName, p_strNewLibName, p_strNewLibName.Length)
                If p_lngRtn > 0 Then
                    p_strLibName = p_strNewLibName.Substring(0, Math.Min(p_strNewLibName.Length, p_lngRtn))
                End If
            End If

            ' -------------------------------------------
            ' Add to static array
            ' -------------------------------------------
            p_lngNumLibNames += 1
            ReDim Preserve p_statypLibNames(p_lngNumLibNames - 1)
            p_statypLibNames(p_lngNumLibNames).SourceName = xi_strEvtSourceName
            p_statypLibNames(p_lngNumLibNames).LibName = p_strLibName
        End If

        ' ----------------------------------------------
        ' If have replacement strings, then parse
        '   them into an array to make them easier
        '   to deal with
        ' ----------------------------------------------
        If xi_lngNumStrings > 0 Then
            ReDim p_astrMsgArg(xi_lngNumStrings - 1)

            p_astrMsgArg = Strings.Split(xi_strStrings, Strings.Chr(0).ToString(), -1, CompareMethod.Binary)

            ' ---------------------------------------
            ' Gets more complicated -- we have some
            '   parameters to replace from the
            '   ParameterMessageFile instead of the
            '   EventMessageFile.
            ' ---------------------------------------
            For p_lngLoop As Integer = 0 To xi_lngNumStrings - 1
                If p_astrMsgArg(p_lngLoop).IndexOf("%%") >= 0 Then
                    p_astrMsgArg(p_lngLoop) = ResourceString2(xi_strEvtSourceName, p_astrMsgArg(p_lngLoop))
                End If
            Next


            resume2 = False

        End If

        ' ----------------------------------------------
        ' Get the string into a format which
        '   FormatMessage can use
        ' ----------------------------------------------
        If xi_lngNumStrings > 0 Then
            p_lngTransRtn = TranslateArray(p_astrMsgArg, p_typMsgs)
        End If

        ' ----------------------------------------------
        ' You can have multiple files listed, separated
        '     by a semicolon. Get just the first item
        ' Actually, you should search the remaining
        '     files, in order, if you can't find the info
        '     in the first one, but I'm leaving that
        '     as an exercise to the reader.
        ' ----------------------------------------------
        p_lngPos = (p_strLibName.IndexOf(";"c) + 1)
        If p_lngPos > 0 Then
            p_strLibName = p_strLibName.Substring(0, Math.Min(p_strLibName.Length, p_lngPos - 1))
        End If

        ' ----------------------------------------------
        ' If path contains spaces, get the short name
        ' ----------------------------------------------
        If p_strLibName.IndexOf(" "c) >= 0 Then
            p_strLibName = modMain.GetShortName(p_strLibName)
        End If

        ' ----------------------------------------------
        ' Load the module
        ' Note: If you use LoadLibrary, it will fail
        '       on certain libraries. You will need to use
        '       the LOAD_LIBRARY_AS_DATAFILE flag.
        ' "Use this flag when you want to load a DLL only
        '       to extract messages or resources from it"
        ' ----------------------------------------------
        Dim p_lngModHwnd As Integer = LoadLibraryEx(lpLibFileName:=p_strLibName, hFile:=0, dwFlags:=LOAD_LIBRARY_AS_DATAFILE)

        ' ----------------------------------------------
        ' Get the string from the module
        ' ----------------------------------------------
        If p_lngModHwnd > 0 Then
            p_lngRtnErr = FormatMsgFromResource(p_lngModHwnd, xi_lngEventID, p_typMsgs, p_strMsgString, p_strApiError, True)
            If p_lngRtnErr <> 0 Then
                m_lngErrNumber = p_lngRtnErr
                m_strErrSource = FORMAT_SOURCENAME & "FormatMsgFromResource"
                m_strErrDesc = p_strApiError

                p_strMsgString = Nothing
                result = Nothing
                'Err.Raise m_lngErrNumber, m_strErrSource, m_strErrDesc
                p_strMsgString = m_strErrDesc
            End If
        Else
            p_strMsgString = "Error loading the library, " & Strings.Chr(13) & Strings.Chr(10) & p_strLibName & Strings.Chr(13) & Strings.Chr(10) & "Error: " & modMain.ReturnApiErrString(Information.Err().LastDllError)
        End If

        ' ----------------------------------------------
        ' Clean up the string
        ' ----------------------------------------------
        p_strMsgString = ClearLastCrLf(p_strMsgString.Trim())

        ' ----------------------------------------------
        ' Free the module
        ' ----------------------------------------------
        FreeLibrary(p_lngModHwnd)

        ' ----------------------------------------------
        ' Set the return value
        ' ----------------------------------------------
        Return p_strMsgString


    End Function

    ' *******************************************************
    ' Routine Name : (PRIVATE in CLASS) Function ResourceString2
    ' Written By   : L.J  Johnson
    ' Programmer   : L.J  Johnson [Slightly Tilted Software]
    ' Date Writen  : 05/29/2001 -- 12:24:54
    ' Inputs       : ByVal xi_strEventSourceName:String -
    '              : ByVal xi_strMsgArg:String -
    ' Outputs      : STRING:
    ' Description  : Private function to translate the embedded "%%"
    '              :     parameters from the resource file with the
    '              :     items returned from parameter file
    ' *******************************************************
    Private Function ResourceString2(ByVal xi_strEventSourceName As String, ByVal xi_strMsgArg As String) As String
        Dim result As String = String.Empty

        ' don't accept an error here
        Dim p_objRegistryDB As New cRegistryDB
        Dim p_strLibName, p_strParam, p_strReplaceable, p_strNewLibName As String
        Dim p_lngItem, p_lngFlags, p_lngSize, p_lngRtn As Integer

        ' -------------------------------------------
        '
        ' -------------------------------------------
        Dim p_strTmp As String = xi_strMsgArg

        ' -------------------------------------------
        ' Get info from the registry
        ' -------------------------------------------
        p_objRegistryDB.LogType = m_strTypeEventLog
        p_objRegistryDB.SourceName = xi_strEventSourceName

        p_objRegistryDB.RegistryEntry(xi_enmMsgType:=cRegistryDB.enmMessageLookup.ParameterMessage)
        If Information.Err().Number = 0 Then
            p_strLibName = p_objRegistryDB.RegistryEntryText

        Else
            ' Just pass on the erro
            result = Nothing
            Throw New System.Exception(m_lngErrNumber.ToString() + ", " + m_strErrSource + ", " + m_strErrDesc)

        End If

        ' -------------------------------------------
        ' Get the fully-replaced library name
        ' -------------------------------------------
        p_strLibName = p_strLibName.ToUpper()
        Dim p_lngPos As Integer = (p_strLibName.IndexOf("%"c) + 1)
        If p_lngPos > 0 Then
            p_strNewLibName = New String(" "c, 256)
            p_lngRtn = ExpandEnvironmentStrings(p_strLibName, p_strNewLibName, p_strNewLibName.Length)
            If p_lngRtn > 0 Then
                p_strLibName = p_strNewLibName.Substring(0, Math.Min(p_strNewLibName.Length, p_lngRtn))
            End If
        End If

        ' -------------------------------------------
        ' Load the library -- if fails, just return
        '     the original string
        ' -------------------------------------------
        Dim p_lngModuleHwnd As Integer = LoadLibrary(p_strLibName)
        If p_lngModuleHwnd = 0 Then
            Return xi_strMsgArg
        End If

        ' -------------------------------------------
        ' Kind of junky, but we need to find the
        '     number of "%%"'s in the string. Using
        '     SPLIT gives up an array of #items + 1,
        '     but it's zero-based, so UBOUND is the
        '     number of items
        ' -------------------------------------------
        Dim p_astrTmpArray() As String = Strings.Split(p_strTmp, "%%", -1, CompareMethod.Binary)
        Dim p_lngNumItems As Integer = p_astrTmpArray.GetUpperBound(0)

        ' -------------------------------------------
        ' Replace each parameter starting with %%
        '   (and get rid of CR-LF pairs)
        ' -------------------------------------------
        For p_lngLoop As Integer = 0 To p_lngNumItems - 1

            p_lngItem = 0
            p_lngItem = CInt(p_astrTmpArray(p_lngLoop + 1))

            p_strReplaceable = "%%" & p_lngItem

            ' ----------------------------------------
            ' Set the flags for the API call
            ' ----------------------------------------
            p_lngFlags = FORMAT_MESSAGE_FROM_HMODULE Or FORMAT_MESSAGE_IGNORE_INSERTS
            p_lngSize = 512
            p_strParam = New String(" "c, p_lngSize)

            ' ----------------------------------------
            ' Get the message string from the file
            ' ----------------------------------------
            Dim handle As GCHandle = GCHandle.Alloc(p_lngModuleHwnd, GCHandleType.Pinned)
            Try
                Dim tmpPtr2 As IntPtr = IntPtr.Zero
                Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()
                p_lngRtn = FormatMessage(p_lngFlags, tmpPtr, p_lngItem, &H0, p_strParam, p_lngSize, tmpPtr2)
            Finally
                handle.Free()
            End Try

            ' ----------------------------------------
            ' Check for errors
            ' ----------------------------------------
            If p_lngRtn = 0 Then
                m_lngErrNumber = (ERR_RESOURCE_DATA_NOT_FOUND + Constants.vbObjectError)
                m_strErrSource = EVENT_SOURCENAME & "ResourceString"
                m_strErrDesc = ReturnApiErrString(Information.Err().LastDllError)
                Throw New System.Exception(m_lngErrNumber.ToString() + ", " + m_strErrSource + ", " + m_strErrDesc)

            Else
                p_strParam = p_strParam.Substring(0, p_lngRtn).Trim()

                If p_strParam.Substring(p_strParam.Length - 2) = Strings.Chr(13) & Strings.Chr(10) Then
                    p_strParam = p_strParam.Substring(0, p_strParam.Length - 2)
                End If
            End If

            p_strTmp = p_strTmp.Replace(p_strReplaceable, p_strParam)

        Next p_lngLoop

        ' -------------------------------------------
        ' Set the return value
        ' -------------------------------------------
        result = p_strTmp

        ' -------------------------------------------
        ' Reset error trapping
        ' -------------------------------------------


        Return result
    End Function

    ' *******************************************************
    ' Routine Name : (PUBLIC in CLASS) Function GetNumRecords
    ' Written By   : L.J  Johnson
    ' Programmer   : L.J  Johnson [Slightly Tilted Software]
    ' Date Writen  : 05/29/2001 -- 14:29:08
    ' Inputs       : ByVal xi_strLogType:String -
    ' Outputs      : LONG:
    ' Description  : Get the number of records (at this instance)
    '              :     for this log
    ' *******************************************************
    Public Function GetNumRecords(ByVal xi_strLogType As String, ByVal xi_strServerName As String) As Integer
        Try
            Dim p_lngNumRecords As Integer

            m_strTypeEventLog = xi_strLogType
            OpenAnyEventLog(xi_strServerName)


            If GetNumberOfEventLogRecords(m_lngEventLogHwnd, p_lngNumRecords) Then
                Return p_lngNumRecords
            Else
                Return -1
            End If

        Catch
        End Try


        m_lngErrNumber = Information.Err().Number
        m_strErrSource = Information.Err().Source
        m_strErrDesc = Information.Err().Description

        CloseEventLog(hEventLog:=m_lngEventLogHwnd)

        Throw New System.Exception(m_lngErrNumber.ToString() + ", " + m_strErrSource + ", " + m_strErrDesc)

    End Function

    ' ***********************************************************************
    ' ***********************************************************************
    ' ****                           CLASS Properties                    ****
    ' ***********************************************************************
    ' ***********************************************************************

    ' *******************************************************
    ' Routine Name : (PRIVATE in CLASS) Sub Class_Initialize
    ' Written By   : L.J  Johnson
    ' Programmer   : L.J  Johnson [Slightly Tilted Software]
    ' Date Writen  : 05/29/2001 -- 12:11:17
    ' Inputs       : N/A
    ' Outputs      : N/A
    ' Description  : Indicates that an event log has never been
    '              :     opened -- used to set correct error info
    ' *******************************************************
End Class
