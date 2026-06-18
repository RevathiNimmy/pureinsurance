Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 10/05/2000
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMWrkPartyTasks"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
    ' Object Manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer

    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	Public Const ACSTUrgentCol As Integer = 0
	Public Const ACSTStatusCol As Integer = 1
	Public Const ACSTDueDateCol As Integer = 2
	Public Const ACSTDescriptionCol As Integer = 3
	Public Const ACSTCustomerCol As Integer = 4
	Public Const ACSTTaskTypeCol As Integer = 5
	Public Const ACSTUserGroupCol As Integer = 6
	Public Const ACSTUserCol As Integer = 7
	Public Const ACSTDueDateSortableCol As Integer = 8
	
	Public Const ACSchedTaskPrefix As String = "ST"
	
	Public Const ACListTaskTypeAll As String = "All"
	Public Const ACListTaskTypeNew As String = "New"
	Public Const ACListTaskTypeInProgress As String = "In Progress"
	Public Const ACListTaskTypeComplete As String = "Complete"
	Public Const ACListTaskTypeInComplete As String = "InComplete"
	Public Const ACListTaskTypeAllButComplete As String = "(Not Complete)"
	
	Public Const ACTaskTypeDescSingle As String = "Non-Navigator Function"
	Public Const ACTaskTypeDescNavigator As String = "Navigator Process"
	Public Const ACTaskTypeDescMemo As String = "Memo"
	Public Const ACTaskTypeDescSystem As String = "System Task"
	
	Public Const ACTaskStatusDescNew As String = "New"
	Public Const ACTaskStatusDescInProgress As String = "In Progress"
	Public Const ACTaskStatusDescInComplete As String = "InComplete"
	Public Const ACTaskStatusDescComplete As String = "Complete"
	
	' Date Range Combo Constants
	Public Const ACDateRangeDescAll As String = "(All Dates)"
	Public Const ACDateRangeDescToday As String = "Today"
	Public Const ACDateRangeDescNext1 As String = "Tomorrow"
	Public Const ACDateRangeDescNext2 As String = "Next 2 Days"
	Public Const ACDateRangeDescNext3 As String = "Next 3 Days"
	Public Const ACDateRangeDescNext4 As String = "Next 4 Days"
	Public Const ACDateRangeDescNext5 As String = "Next 5 Days"
	Public Const ACDateRangeDescNext6 As String = "Next 6 Days"
	Public Const ACDateRangeDescNext7 As String = "Next 7 Days"
	Public Const ACDateRangeDescNext14 As String = "Next 14 Days"
	Public Const ACDateRangeDescNext28 As String = "Next 28 Days"
	
	Public Const ACDateRangeIndexAll As Integer = 0
	Public Const ACDateRangeIndexToday As Integer = 1
	Public Const ACDateRangeIndexNext1 As Integer = 2
	Public Const ACDateRangeIndexNext2 As Integer = 3
	Public Const ACDateRangeIndexNext3 As Integer = 4
	Public Const ACDateRangeIndexNext4 As Integer = 5
	Public Const ACDateRangeIndexNext5 As Integer = 6
	Public Const ACDateRangeIndexNext6 As Integer = 7
	Public Const ACDateRangeIndexNext7 As Integer = 8
	Public Const ACDateRangeIndexNext14 As Integer = 9
	Public Const ACDateRangeIndexNext28 As Integer = 10
	
	Public Const ACUserGroupAllGroups As String = "All Groups"
	Public Const ACUserGroupYourGroups As String = "Your Groups"
	
	Public Const ACSystemTasksWithinDays As Integer = 3
	Public Const ACSystemTasksTimerInterval As Integer = 60000
	Public Const ACSystemTasksCheckEveryMins As Integer = 15
	
	Public Const ACStatusAuthUser As String = "Authority: User"
	Public Const ACStatusAuthSupervisor As String = "Authority: Supervisor"
	Public Const ACStatusAuthSysAdmin As String = "Authority: Sys Admin"
	
	Public Const ACStatusItems As String = " Item(s) Found."
	
	Public Const ACStatusActSearching As String = "Searching..."
	Public Const ACStatusActStartingTask As String = "Starting Task..."
	Public Const ACStatusActCheckingForSystem As String = "Checking for System Tasks which are due..."
	Public Const ACStatusActWebLoading As String = "Loading..........."
	
	' Scheduled Task Actions
	Public Enum ACESchedTaskAction
		aceSTAEdit = 1
		aceSTAAssign = 2
		aceSTAView = 3
		aceSTAStart = 4
		aceSTAComplete = 5
		aceSTAIncomplete = 6
		aceSTADelete = 7
		aceSTATaskLog = 8
	End Enum
	
	Sub Main_Renamed()
		
	End Sub
End Module