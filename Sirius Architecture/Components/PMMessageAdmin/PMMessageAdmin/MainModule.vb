Option Strict Off
Option Explicit On
Imports System
'developer guide no. 129
Imports SharedFiles
Module MainModule
	'******************************************************************************
	'
	' Name: MainModule
	'
	' Edit History:
	'
	'******************************************************************************
	
	Public Const ACApp As String = "PMMessageAdmin"
	
	'username
	Public Const MAUserName As Integer = 0
	'message_type
	Public Const MAMessageType As Integer = 1
	'text
	Public Const MAText As Integer = 2
	'err_number
	Public Const MAErrNumber As Integer = 3
	'err_description
	Public Const MAErrDescription As Integer = 4
	'calling_app_name
	Public Const MACallingAppName As Integer = 5
	'app_name
	Public Const MAAppName As Integer = 6
	'class_name
	Public Const MAClassName As Integer = 7
	'method_name
	Public Const MAMethodName As Integer = 8
	'log_date
	Public Const MALogDate As Integer = 9
	
	Public Const FilterAll As String = "(All)"
	Public Const AllMessages As gPMConstants.PMELogLevel = 0
	
	Public Const PMRecordsAll As String = "(all)"
	Public Const PMRecords500 As String = "500"
	Public Const PMRecords400 As String = "400"
	Public Const PMRecords300 As String = "300"
	Public Const PMRecords200 As String = "200"
	Public Const PMRecords100 As String = "100"
	Public Const PMRecords10 As String = "10"
	
	' Sub-item
	Public Const ACColumnDate As Integer = 1
End Module