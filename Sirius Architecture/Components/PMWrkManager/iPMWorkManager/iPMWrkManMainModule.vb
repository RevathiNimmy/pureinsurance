Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
Imports SharedFiles
Module MainModule
	' Edit History:
	'
	' DAK130999 - New error message for Favourites Group in
	'             Sheridan active list bar - replacement for
	'             quick start.
	' DAK241299 - Check Box/Menu enabled indicator
	' ***************************************************************** '
	
	Private Declare Function SendMessageLong Lib "user32"  Alias "SendMessageA"(ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
	Private Declare Function FindWindowEx Lib "user32"  Alias "FindWindowExA"(ByVal hWnd1 As Integer, ByVal hWnd2 As Integer, ByVal lpsz1 As String, ByVal lpsz2 As String) As Integer

	Public Const WM_USER As Integer = &H400s
	Public Const TB_SETsStyle As Integer = WM_USER + 56
	Public Const TB_GETsStyle As Integer = WM_USER + 57
	Public Const TBsStyle_FLAT As Integer = &H800s
	Public Const GW_CHILD As Integer = 5
	Public Const GWL_sStyle As Integer = (-16)
    Public Const WS_VSCROLL As Integer = &H200000
    'Modified,add the g_sProductFamily for helpfile
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusArchitecture
	Public Const ACApp As String = "iPMWorkManager"
	
	Public Const ACHelpFileLocation As String = "\..\..\Common\Help\Work.hlp"
	
	Public Const ACSTUrgentCol As Integer = 0
	Public Const ACSTStatusCol As Integer = 1
	Public Const ACSTDueDateCol As Integer = 2
	Public Const ACSTDescriptionCol As Integer = 3
	Public Const ACSTCustomerCol As Integer = 4
	Public Const ACSTTaskTypeCol As Integer = 5
	Public Const ACSTUserGroupCol As Integer = 6
	Public Const ACSTUserCol As Integer = 7
	Public Const ACSTDueDateSortableCol As Integer = 8
    Public Const ACSTCustomerSortableCol As Integer = 9 'mkw100204 PN9978
    Public Const ACSTAgentSortableCol As Integer = 10
	
	Public Const ACTaskGroupPrefix As String = "TG"
	Public Const ACTaskPrefix As String = "T"
	Public Const ACAvailableTaskPrefix As String = "AT"
	Public Const ACSchedTaskPrefix As String = "ST"
	
	Public Const ACTaskGroupImage As String = "TaskGroup"
	Public Const ACTaskMemoImage As String = "Memo"
	Public Const ACTaskSingleComponentImage As String = "SingleComponent"
	Public Const ACTaskNavProcessImage As String = "NavigatorProcess"
	Public Const ACTaskSystemImage As String = "System"
	
	Public Const ACListTaskTypeAll As String = "All"
	Public Const ACListTaskTypeNew As String = "New"
	Public Const ACListTaskTypeInProgress As String = "In Progress"
	Public Const ACListTaskTypeComplete As String = "Complete"
	Public Const ACListTaskTypeInComplete As String = "InComplete"
	Public Const ACListTaskTypeAllButComplete As String = "(Not Complete)"
	
	Public Const ACListShowSystemOnly As String = "System"
	Public Const ACListShowSystemUser As String = "User"
    Public Const ACListShowSystemAll As String = "(All)"
    Public Const ACListShowAgentAll As String = "(All)"
	
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
    Public Const ACAgentIndexAll As Integer = 0

    ' Date Range Combo Constants
    Public Const kACBatchDateRangeDescAll As String = "(All Dates)"
    Public Const kACBatchDateRangeDescToday As String = "Today"
    Public Const kACBatchDateRangeDescPrev1 As String = "Yesterday"
    Public Const kACBatchDateRangeDescPrev2 As String = "Past 2 Days"
    Public Const kACBatchDateRangeDescPrev3 As String = "Past 3 Days"
    Public Const kACBatchDateRangeDescPrev4 As String = "Past 4 Days"
    Public Const kACBatchDateRangeDescPrev5 As String = "Past 5 Days"
    Public Const kACBatchDateRangeDescPrev6 As String = "Past 6 Days"
    Public Const kACBatchDateRangeDescPrev7 As String = "Past 7 Days"
    Public Const kACBatchDateRangeDescPrev14 As String = "Past 14 Days"
    Public Const kACBatchDateRangeDescPrev28 As String = "Past 28 Days"

    Public Const kACBatchDateRangeIndexAll As Integer = 0
    Public Const kACBatchDateRangeIndexToday As Integer = 1
    Public Const kACBatchDateRangeIndexPrev1 As Integer = 2
    Public Const kACBatchDateRangeIndexPrev2 As Integer = 3
    Public Const kACBatchDateRangeIndexPrev3 As Integer = 4
    Public Const kACBatchDateRangeIndexPrev4 As Integer = 5
    Public Const kACBatchDateRangeIndexPrev5 As Integer = 6
    Public Const kACBatchDateRangeIndexPrev6 As Integer = 7
    Public Const kACBatchDateRangeIndexPrev7 As Integer = 8
    Public Const kACBatchDateRangeIndexPrev14 As Integer = 9
    Public Const kACBatchDateRangeIndexPrev28 As Integer = 10

    Public Const ACUserGroupAllGroups As String = "All Groups"
	Public Const ACUserGroupYourGroups As String = "Your Groups"
	
	Public Const ACQuickStartButtonHeight As Double = 780.09
	Public Const ACQuickStartButtonWidth As Double = 1260.28
	Public Const ACQuickStartPlaceHolderWidth As Integer = 200
	
	Public Const ACSystemTasksWithinDays As Integer = 3
	Public Const ACSystemTasksTimerInterval As Integer = 60000
	Public Const ACSystemTasksCheckEveryMins As Integer = 15

    Public Const ACQuickStartErroMsg As String = "A Task on your Quick Start Bar has been" & Strings.Chr(13) & Strings.Chr(10) &  _
	                                             "removed from your Available Tasks." & Strings.Chr(13) & Strings.Chr(10) & "It will automatically removed from the Quick Start Bar."

    'DAK130999 - Name of favourites group on Sheridan Active
    '            List bar
    'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.2.1)
    'Public Const ACFavouritesCaption = "Favourites"
    'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.2.1)

    'DAK130999 - Task unavailable errot message
    Public Const ACFavouritesErrorMsg As String = "A Task on your Favourites Group has been" & Strings.Chr(13) & Strings.Chr(10) &  _
	                                              "removed from your Available Tasks." & Strings.Chr(13) & Strings.Chr(10) & "It will automatically removed from the Favourites Group."
	
	Public Const ACInProgressTitle As String = "Tasks In Progress"
	Public Const ACInProgressWarning As String = "There are Tasks In Progress." & Strings.Chr(13) & Strings.Chr(10) & "Unable to Close WorkManager"
	
	Public Const ACStatusAuthUser As String = "Authority: User"
	Public Const ACStatusAuthSupervisor As String = "Authority: Supervisor"
	Public Const ACStatusAuthSysAdmin As String = "Authority: Sys Admin"
	
	Public Const ACStatusItems As String = " Item(s) Found."
	
	Public Const ACStatusActSearching As String = "Searching..."
	Public Const ACStatusActStartingTask As String = "Starting Task..."
	Public Const ACStatusActCheckingForSystem As String = "Checking for System Tasks which are due..."
	Public Const ACStatusActWebLoading As String = "Loading..........."
	
	'DAK241299
	Public Const ACChkEnabled As Integer = 4
	
	'DAK110700
    'Public Const ACMainFormCaption As String = "Sirius Group: Work Manager"
    Public Const ACMainFormCaption As String = "SSP Pure: Work Manager"
    Public Const ACWebTabCaption As String = "&News"
    Public cboPartySelectedValue As Integer
	
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
	
	'RDC 27042001 new registry settings for task display
	Public Const ACWrkManRegColumnWidths As String = "ColumnWidths"
	Public Const ACWrkManRegComboSettings As String = "ComboSettings"
	Public Const ACWrkManRegSplashAppTitle As String = "SplashAppTitle"
	
	' RES Constants
	Public Const ACFavouritiesCaption As Integer = 100
	
	Private Const ACClass As String = "MainModule"

    Public m_frm As Form
    Public Const kUSLangId As Integer = 2
    Public Const kUKLangId As Integer = 1

    ' ***************************************************************** '
    ' Name: Main
    '
    ' Description: Main Entry Point.
    '
    ' ***************************************************************** '

    Public Sub Main()
		
        ' If there is already a Work Manager running then exit.
        Dim Procesos() As Process = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName)
        'If Procesos.Length > 1 And Process.GetCurrentProcess().StartTime <> Procesos(0).StartTime Then
        Dim iInstances As Integer = 0
        Dim iSessionID As Integer = Process.GetCurrentProcess.SessionId
        If Procesos.Length > 1 Then
            For index As Integer = 0 To Procesos.Length - 1
                If Procesos(index).SessionId = Process.GetCurrentProcess.SessionId Then
                    iInstances += 1
                End If
            Next

            If iInstances > 1 Then
                Exit Sub
            End If
        End If
        ' Create the Control Class
        Dim oControl As PMWorkManager.ControlClass = New PMWorkManager.ControlClass()

        ' Initialise
        'developers guide no. 9
        Dim lReturn As Long
        lReturn = oControl.Initialise()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        ' Start
        lReturn = oControl.Start()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Error Starting Pure Work Manager.", "Pure Insurance")
            Exit Sub
        End If

        ' Do not Terminate the Control Class
        ' as it will Terminate Itself When the Form is Closed

        ' Just release the Reference
        oControl = Nothing
		
	End Sub
	
	Public Sub Coolbar(ByRef tbToolBar As ToolStrip)
		
		
		'get the handle of the toolbar
		Dim shToolbar As Integer = FindWindowEx(tbToolBar.Handle.ToInt32(), 0, "ToolbarWindow32", Nothing)
		
		'retrieve the toolbar sStyles
		Dim style As Integer = SendMessageLong(shToolbar, TB_GETsStyle, 0, 0)
		
		'Set the new sStyle flag
		If style And TBsStyle_FLAT Then
			style = style Xor TBsStyle_FLAT
		Else
			style = style Or TBsStyle_FLAT
		End If
		
		'apply the new sStyle to the toolbar
		Dim lR As Integer = SendMessageLong(shToolbar, TB_SETsStyle, 0, style)
		tbToolBar.Refresh()
		
	End Sub
End Module
