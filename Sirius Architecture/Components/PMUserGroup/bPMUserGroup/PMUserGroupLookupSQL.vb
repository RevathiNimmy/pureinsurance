Option Strict Off
Option Explicit On
Imports System
Module PMUserGroupLookupSQL
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
    Public Const ACAllUsersLookupStored As Boolean = True
    Public Const ACAllUsersLookupName As String = "AllUserLookup"
    'Developer Guide No. 39
    Public Const ACAllUsersLookupSQL As String = "spu_pmuser_all_users_sel"

    ' Group All User Lookup SQL
    Public Const ACGroupUsersLookupStored As Boolean = True
    Public Const ACGroupUsersLookupName As String = "GroupUsersLookup"
    'Developer Guide No. 39
    Public Const ACGroupUsersLookupSQL As String = "spu_pmuser_group_users_sel"

    ' Group All User Lookup SQL
    Public Const ACUsersGroupsLookupStored As Boolean = True
    Public Const ACUsersGroupsLookupName As String = "UsersGroupsLookup"
    'Developer Guide No. 39
    Public Const ACUsersGroupsLookupSQL As String = "spu_pmuser_users_groups_sel"

    ' All Groups Lookup SQL
    Public Const ACAllUserGroupsLookupStored As Boolean = True
    Public Const ACAllUserGroupsLookupName As String = "UserGroupsLookup"
    'Developer Guide No. 39
    Public Const ACAllUserGroupsLookupSQL As String = "spu_pmuser_groups_all_sel"

    ' All User Groups By Task Lookup SQL
    Public Const ACUserGroupsByTaskLookupStored As Boolean = True
    Public Const ACUserGroupsByTaskLookupName As String = "UserGroupsByTaskLookup"
    'Developer Guide No. 39
    Public Const ACUserGroupsByTaskLookupSQL As String = "spu_pmuser_groups_task_sel"

    ' All User Groups By User Lookup SQL
    Public Const ACUserGroupsByUserLookupStored As Boolean = True
    Public Const ACUserGroupsByUserLookupName As String = "UserGroupsByTaskLookup"
    'Developer Guide No. 39
    Public Const ACUserGroupsByUserLookupSQL As String = "spu_pmuser_users_groups_sel"
End Module