Option Strict Off
Option Explicit On
Imports System
Friend NotInheritable Class Report 
	' ***************************************************************** '
	' Class Name: Report
	'
	' Date: 29/06/2003
	'
	' Description:
	'   Simple class to allow us to display the report description but
	'   still access the real name
	'
	' Edit History:
	' ***************************************************************** '
	
	' This is marked as the default property to we shouldn't break
	' existing code where I miss it.
	Public ReportName As String = ""
	
	' Friendly display name
	Public Description As String = ""
End Class
