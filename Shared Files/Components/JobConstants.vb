Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("JobConstants_NET.JobConstants")> _
 Public Module JobConstants
	
	' ***************************************************************** '
	'
	' Name: JobConstants
	'
	' Description: Module containing constants for using the Job Schedule API
	'
	' Author: CTAF 200201
	'
	' Notes: All Network Management API calls are documented at :
	'        http://msdn.microsoft.com/library/psdk/network/ntlmapi_4l83.htm
	'
	' ***************************************************************** '
	
	Private Const ACClass As String = "JobConstants"
	
	Public Structure AT_ENUM
		Dim dw_JobId As Integer
		Dim dw_JobTime As Integer
		Dim dw_DaysOfMonth As Integer
		Dim dw_DaysOfWeek As Byte
		Dim dw_Flags As Byte
		Dim dw_dummy As Integer
		Dim ptr_Command As Integer
	End Structure
	
	Public Structure AT_INFO
		Dim dw_JobTime As Integer
		Dim dw_DaysOfMonth As Integer
		Dim dw_DaysOfWeek As Byte
		Dim dw_Flags As Byte
		Dim dw_dummy As Integer
		Dim ptr_Command As Integer
	End Structure
	
	Public Enum PM_Job_Flags
		JOB_NO_FLAGS = &H0s
		JOB_RUN_PERIODICALLY = &H1s
		JOB_EXEC_ERROR = &H2s
		JOB_RUNS_TODAY = &H4s
		JOB_ADD_CURRENT_DATE = &H8s
		JOB_NONINTERACTIVE = &H10s
	End Enum
	
	Public Enum PM_Job_DaysOfWeek
		Monday = 1
		Tuesday = 2
		Wednesday = 4
		Thursday = 8
		Friday = 16
		Saturday = 32
		Sunday = 64
	End Enum
	
	' Awkward b......s
	Public Enum PM_Sql_Job_DaysOfWeek
		Sunday = 1
		Monday = 2
		Tuesday = 4
		Wednesday = 8
		Thursday = 16
		Friday = 32
		Saturday = 64
	End Enum
	
	' Service Status constants
	Public Const SERVICE_STOPPED As Integer = 1
	Public Const SERVICE_START_PENDING As Integer = 2
	Public Const SERVICE_STOP_PENDING As Integer = 3
	Public Const SERVICE_RUNNING As Integer = 4
	Public Const SERVICE_CONTINUE_PENDING As Integer = 5
	Public Const SERVICE_PAUSE_PENDING As Integer = 6
	Public Const SERVICE_PAUSED As Integer = 7
	
	' Win32 API error messages
	Public Const ERROR_BAD_NETPATH As Integer = 53
	Public Const ERROR_INVALID_PARAMETER As Integer = 87
	Public Const ERROR_MORE_DATA As Integer = 234
	
	' Net API error messages
	Public Const NERR_Success As Integer = 0
	
	' Constants for the positions in the arrays
	Public Const AC_Job_ID As Integer = 0
	Public Const AC_Job_CommandLine As Integer = 1
	Public Const AC_Job_Time As Integer = 2
	Public Const AC_Job_DaysOfMonth As Integer = 3
	Public Const AC_Job_DaysOfWeek As Integer = 4
	Public Const AC_Job_Flags As Integer = 5
	Public Const AC_Job_Name As Integer = 6
	Public Const AC_Job_LastRun As Integer = 7
	
	
	' Occurs constants
	Public Const AC_Job_Occurs_Next As Integer = 0
	Public Const AC_Job_Occurs_Every As Integer = 1
	
	Public Const AC_PreRenSelection As String = "Pre-Renewal Selection"
	Public Const AC_Selection As String = "Selection"
	Public Const AC_Combined_Selection As String = "Combined Selection"
	Public Const AC_Quote_Insurer As String = "Insurer Led Quote"
	Public Const AC_Quote_Broker As String = "Broker Led Quote"
	Public Const AC_Invite As String = "Invite"
	Public Const AC_Reminder As String = "Reminder"
	Public Const AC_Complete As String = "Completion"
	Public Const AC_ProcessEDI As String = "Process EDI"
	Public Const AC_Auto_Renewal As String = "Process Auto Renew"
	
	' CTAF 11062001 - The following aren't used
	Public Const AC_Confirm As String = "Confirm"
	Public Const AC_Lapse As String = "Lapse"
	Public Const AC_Housekeep As String = "Housekeep"
	
	' CTAF 110601 - The following are final file names
	Public Const AC_Job_PreRenSelection As String = "PreRenSel.exe"
	Public Const AC_Job_Selection As String = "RenSelection.exe"
	Public Const AC_Job_Combined_Selection As String = "RenCombSel.exe"
	Public Const AC_Job_Quote_Insurer As String = "QuoteInsLed.exe"
	Public Const AC_Job_Quote_Broker As String = "QuoteBrokerLed.exe"
	Public Const AC_Job_Invite As String = "RenInvitation.exe"
	Public Const AC_Job_Reminder As String = "RenReminder.exe"
	Public Const AC_Job_Complete As String = "RenCompletion.exe"
	'AK 261001 - Process EDI uses a different Executable name
	'Public Const AC_Job_ProcessEDI As String = "ProcessEDI.exe"
	Public Const AC_Job_ProcessEDI As String = "RenEDIIn.exe"
	Public Const AC_Job_Auto_Renewal As String = "RenAutoRenewal.exe"
	
	' CTAF 110601 - The following aren't used at present
	Public Const AC_Job_Confirm As String = "confirm.exe"
	Public Const AC_Job_Lapse As String = "lapse.exe"
	Public Const AC_Job_Housekeep As String = "housekeep.exe"
End Module