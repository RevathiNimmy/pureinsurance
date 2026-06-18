Option Strict Off
Option Explicit On
Imports System
Module OnlinePartySQL
	' ***************************************************************** '
	' Class Name: PMUserGroupLookupSQL
	'
	' Date: 22 October 1998
	'
	' Description: Contains the SQL/Stored Procedures used by the
	'              Lookup class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' All User Lookup SQL
	' Update Party Onlie Status SQL
	Public Const ACUpdateOnlineAccessStatusStored As Boolean = True
    Public Const ACUpdateOnlineAccessStatusName As String = "UpdateOnlineAccessStatus"
    'Developer Guide No.39
    Public Const ACUpdateOnlineAccessStatusSQL As String = "spu_Party_Update_Online_Access_Status"
End Module