Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	'* Amendment History
	'*
	'* DAK080999 - Refresh Scheduled Tasks on a regular basis
	'* DAK090999 - Allow drag to "Favourites" group only
	'* DAK090999 - Prevent dragging of non-QuickView tasks
	'* DAK100999 - Prevent dropping onto other tasks
	'* DAK130999 - Function to add a task to the Favourites Group
	'* DAK160999 - Set/unset PMNews enablement when address is changed
	'* DAK210999 - Further amendments for dragging & dropping
	'* DAK220999 - Refresh available tasks
	'* DAK091299 - Prevent crash when leaving Active list Bar
	'* DAK221299 - Add Option to remove graphics
	'* DAK030100 - Reset refresh timer when manual refresh
	'* DAK110100 - Refresh options
	'* DAK210100 - Separate User Options from System Options
	'* DAK240100 - Memo tasks can only create new scheduled task
	'******************************************************************************
	
	Private Declare Function OSWinHelp Lib "user32"  Alias "WinHelpA"(ByVal hwnd As Integer, ByVal HelpFile As String, ByVal wCommand As Short, ByVal dwData As Integer) As Short
	
	Private m_bMoving As Boolean
	Private m_sHGap As Single
	Private m_sVGap As Single
	
	' Parerent Control Class
	Private m_oParent As Interface_Renamed
	
	Private m_bFormDisplayed As Boolean
	
	' ViewToolbar
	Private m_bViewToolbar As Boolean
	' ViewStatusBar
	Private m_bViewStatusBar As Boolean
	' ViewGridLines
	Private m_bViewGridLines As Boolean
	' IsAutoRefresh
	Private m_bIsAutoRefresh As Boolean
	' RefreshRate
	Private m_iRefreshRate As Integer
	' KeyName
	Private m_sKeyName As String = ""
	' KeyValue
	Private m_sKeyValue As String = ""
	
	'Private m_fCurrentYPos As Single
	
	'Private m_lEffect As Long
	
	Private Const ACClass As String = "frmMain"
	
	' Events
	Public Event ScheduledTaskAction(ByVal eAction As MainModule.ACESchedTaskAction)
	Public Event ScheduledTaskClick(ByVal v_sScheduledTaskKey As String)
	Public Event ScheduledTaskRightClick(ByVal v_sScheduledTaskKey As String)
	Public Event RefreshScheduledTasks(ByVal v_bForceRefresh As Boolean)
	Public Event FormClose()
	
	Public Property FormDisplayed() As Boolean
		Get
			Return m_bFormDisplayed
		End Get
		Set(ByVal Value As Boolean)
			m_bFormDisplayed = Value
		End Set
	End Property
	
	Public Property Parent_Renamed() As Interface_Renamed
		Get
			Return m_oParent
		End Get
		Set(ByVal Value As Interface_Renamed)
            'Developer Guide No. 10(no solution)
            'If Microsoft.VisualBasic.Information.IsReference(Value) And Not (TypeOf Value Is String) Then
            '    m_oParent = Value
            'Else
            '    m_oParent = Value
            'End If
            m_oParent = Value
		End Set
	End Property
	
	Public Property ViewToolbar() As Boolean
		Get
			Return m_bViewToolbar
		End Get
		Set(ByVal Value As Boolean)
			m_bViewToolbar = Value
		End Set
	End Property
	
	Public Property ViewStatusBar() As Boolean
		Get
			Return m_bViewStatusBar
		End Get
		Set(ByVal Value As Boolean)
			m_bViewStatusBar = Value
		End Set
	End Property
	
	Public Property ViewGridLines() As Boolean
		Get
			Return m_bViewGridLines
		End Get
		Set(ByVal Value As Boolean)
			m_bViewGridLines = Value
		End Set
	End Property
	
	'DAK110100
	Public Property IsAutoRefresh() As Boolean
		Get
			Return m_bIsAutoRefresh
		End Get
		Set(ByVal Value As Boolean)
			m_bIsAutoRefresh = Value
		End Set
	End Property
	
	Public Property RefreshRate() As Integer
		Get
			Return m_iRefreshRate
		End Get
		Set(ByVal Value As Integer)
			m_iRefreshRate = Value
		End Set
	End Property
	
	Public Property KeyName() As String
		Get
			Return m_sKeyName
		End Get
		Set(ByVal Value As String)
			m_sKeyName = Value
		End Set
	End Property
	
	Public Property KeyValue() As String
		Get
			Return m_sKeyValue
		End Get
		Set(ByVal Value As String)
			m_sKeyValue = Value
		End Set
	End Property
	
	Private Sub cboDateRange_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboDateRange.SelectedIndexChanged
		' Refresh the Scheduled Tasks List
		RaiseEvent RefreshScheduledTasks(False)
	End Sub
	
	Private Sub cboTaskStatus_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTaskStatus.SelectedIndexChanged
		' Refresh the Scheduled Tasks List
		RaiseEvent RefreshScheduledTasks(False)
	End Sub
	
	Private Sub cboUser_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboUser.Click
		' Refresh the Scheduled Tasks List
		RaiseEvent RefreshScheduledTasks(False)
	End Sub
	
	Private Sub cboUserGroup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboUserGroup.Click
		' Refresh the List of Users based on the Group selected.
		RefreshUserList()
		
		' Note we do not need to raise the RefreshSchediledTasks Event
		' as the RefreshUserList call will raise the event for us.
	End Sub
	
	Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			
			' Reset the Control Start Positions and Resize Options
			ResetControlPositions()
			
		End If
	End Sub
	

	Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		With uctPMResizer1
			' Tell the Resizer to only resize ones I tell it to.
			.NoResizeByDefault = True
			.KeepRatio = False
			.FormMinHeight = 7000
			.FormMinWidth = 10980
		End With
		
		' Work Out the Initial Horizontal & Vertical Gaps between
		' the controls.
		m_sHGap = VB6.PixelsToTwipsY(picTitles.Top) + VB6.PixelsToTwipsY(picTitles.Height)
		m_sVGap = VB6.PixelsToTwipsX(lblTitle(2).Left)
		
		
		
	End Sub
	
	Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		' Raise the Form Close Event
		RaiseEvent FormClose()
		
	End Sub
	
	Private Sub lstScheduledTasks_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lstScheduledTasks.ColumnClick
		Dim ColumnHeader As ColumnHeader = lstScheduledTasks.Columns(eventArgs.Column)
		
		If (ColumnHeader.Index + 1 - 1) = ACSTDueDateSortableCol Then
			' We have used a hidden column for a sortable version of the
			' due date. This code resets the column width to zero, if the
			' user has found the column, expanded it and clicked in it.
			lstScheduledTasks.Columns.Item(ACSTDueDateSortableCol).Width = CInt(0)
			mnuViewSortBy_Click(mnuViewSortBy(ACSTDueDateCol + 1), New EventArgs())
		Else
			mnuViewSortBy_Click(mnuViewSortBy((ColumnHeader.Index + 1)), New EventArgs())
		End If
		
	End Sub
	
	Private Sub lstScheduledTasks_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lstScheduledTasks.DoubleClick
		' Raise the Start Task Event
		RaiseEvent ScheduledTaskAction(MainModule.ACESchedTaskAction.aceSTAView)
	End Sub
	
	Private Sub lstScheduledTasks_ItemClick(ByVal Item As ListViewItem)
		
		' Belt and Braces Check
		If Item Is Nothing Then
			Exit Sub
		End If
		
		' Raise the Scheduled Task Click Event
		RaiseEvent ScheduledTaskClick(Item.Name)
		
	End Sub
	
	Private Sub lstScheduledTasks_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lstScheduledTasks.MouseUp
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		Dim oListItem As ListViewItem
		
		' If Right Mouse Click
		If Button = MouseButtonConstants.RightButton Then
			' Check to see if Clicked on a Scheduled Task
			oListItem = lstScheduledTasks.GetItemAt(X, Y)
			If oListItem Is Nothing Then
				' No, so Raise the Right Click Event without a Key
				RaiseEvent ScheduledTaskRightClick("")
			Else
				' Yes, so raise the Right Click Event with the Sched Task Key
				RaiseEvent ScheduledTaskRightClick(oListItem.Name)
			End If
		End If
		
	End Sub
	
	Public Sub mnuFileExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileExit.Click
		
		Parent_Renamed.Status = gPMConstants.PMEReturnCode.PMOK
		Me.Close()
	End Sub
	
	Public Sub mnuTaskAssign_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuTaskAssign.Click
		' Assign/ReAssign Currently Selected Task
		RaiseEvent ScheduledTaskAction(MainModule.ACESchedTaskAction.aceSTAAssign)
	End Sub
	
	Public Sub mnuTaskEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuTaskEdit.Click
		' Edit the Currently Selected Task
		RaiseEvent ScheduledTaskAction(MainModule.ACESchedTaskAction.aceSTAEdit)
	End Sub
	
	Public Sub mnuTaskView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuTaskView.Click
		' View Currently Selected Task
		RaiseEvent ScheduledTaskAction(MainModule.ACESchedTaskAction.aceSTAView)
	End Sub
	
	Public Sub mnuViewSortBy_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _mnuViewSortBy_1.Click, _mnuViewSortBy_2.Click, _mnuViewSortBy_3.Click, _mnuViewSortBy_4.Click, _mnuViewSortBy_5.Click, _mnuViewSortBy_6.Click, _mnuViewSortBy_7.Click, _mnuViewSortBy_8.Click
		Dim Index As Integer = Array.IndexOf(mnuViewSortBy, eventSender)
		
		' Set the List View Sort Column to be the
		' Menu Index. The two must match for this to work.
		
		With lstScheduledTasks
			
			' Turn off sorting so that the list is not sorted twice
			ListViewHelper.SetSortedProperty(lstScheduledTasks, False)
			
			' Reset key so that re-click is recognised see below
			If ListViewHelper.GetSortKeyProperty(lstScheduledTasks) = ACSTDueDateSortableCol Then
				ListViewHelper.SetSortKeyProperty(lstScheduledTasks, ACSTDueDateCol)
			End If
			
			' If we are already sorted by this Column
			If Index - 1 = ListViewHelper.GetSortKeyProperty(lstScheduledTasks) Then
				' Set sort order opposite of current direction
				ListViewHelper.SetSortOrderProperty(lstScheduledTasks, (ListViewHelper.GetSortOrderProperty(lstScheduledTasks) + 1) Mod 2)
			Else
				
				' Uncheck the current Selection
				mnuViewSortBy(ListViewHelper.GetSortKeyProperty(lstScheduledTasks) + 1).Checked = False
				
				' Sort by this column
				ListViewHelper.SetSortKeyProperty(lstScheduledTasks, Index - 1)
				' Ascending
                ListViewHelper.SetSortOrderProperty(lstScheduledTasks, Windows.Forms.SortOrder.Ascending)
				
				' Check the new selection
				mnuViewSortBy(Index).Checked = True
				
			End If
			
			' If we're sorting the date use the hidden yyyymmdd date col
			' so that we get the dates in cronological order
			If ListViewHelper.GetSortKeyProperty(lstScheduledTasks) = ACSTDueDateCol Then
				ListViewHelper.SetSortKeyProperty(lstScheduledTasks, ACSTDueDateSortableCol)
			End If
			
			' Turn on sorting
			ListViewHelper.SetSortedProperty(lstScheduledTasks, True)
			
		End With
		
	End Sub
	
	Private Sub tbToolBar_ButtonClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _tbToolBar_Button1.Click, _tbToolBar_Button2.Click, _tbToolBar_Button3.Click, _tbToolBar_Button4.Click
		Dim Button As ToolStripItem = CType(eventSender, ToolStripItem)
		
		Select Case Button.Name
			Case "Refresh"
				mnuViewRefresh_Click(mnuViewRefresh, New EventArgs())
			Case Else
				'
		End Select
		
	End Sub
	
	Public Sub mnuViewRefresh_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewRefresh.Click
		
		' Raise the Refresh Scheduled Tasks Event
		' Note: As the user has clicked the Refresh Button we will Force
		' the Refresh even if the Selection Criteria is the same as before.
		
		RaiseEvent RefreshScheduledTasks(True)
		
		'DAK040100 - reset timer
		tmrSystemTasks.Enabled = False
		tmrSystemTasks.Enabled = True
		
	End Sub
	
	' ***************************************************************** '
	' Name: ResetControlPositions
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Private Sub ResetControlPositions()
		
		Try 
			
			panMainTab.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(picTitles.Top) + VB6.PixelsToTwipsY(picTitles.Height) + m_sHGap)
			If ViewStatusBar Then
				panMainTab.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(sbStatusBar.Top) - VB6.PixelsToTwipsY(panMainTab.Top) - m_sHGap)
			Else
				panMainTab.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - VB6.PixelsToTwipsY(panMainTab.Top) - m_sHGap)
			End If
			
			panMainTab.Left = 0
			panMainTab.Width = Me.ClientRectangle.Width - panMainTab.Left
			
			lblTitle(2).Left = panMainTab.Left
			lblTitle(2).Width = panMainTab.Width
			
			lstScheduledTasks.Width = panMainTab.Width - VB6.TwipsToPixelsX(150)
			'lstScheduledTasks.Height = tabMain.Height - 600 - 75
			lstScheduledTasks.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(panMainTab.Height) - 600 - 425)
			
			With uctPMResizer1
				.SaveControls()
				.SetControlResizeOption("picTitles", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROWidthOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("panMainTab", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("lstScheduledTasks", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("lblTitle", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROWidthOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight, 2)
				.SetControlResizeOption("picToolbar", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
			End With
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ResetControlPositionsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ResetControlPositions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: AddScheduledTaskToList
	'
	' Description: Builds the Scheduled Task List View from the Array
	'              Supplied.
	' ***************************************************************** '
	Public Function AddScheduledTaskToList(ByVal v_sKey As String, ByVal v_iIsUrgent As Integer, ByVal v_sTaskStatusDesc As String, ByVal v_sTaskTypedesc As String, ByVal v_dtTaskDueDate As Date, ByVal v_sCustomer As String, ByVal v_sDescription As String, ByVal v_sUserGroup As String, ByVal v_sUser As String) As Integer
		
		Dim result As Integer = 0
		Dim oListItem As ListViewItem
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If v_iIsUrgent = gPMConstants.PMEReturnCode.PMTrue Then

				oListItem = lstScheduledTasks.Items.Add(v_sKey, "Yes", "")
			Else
				oListItem = lstScheduledTasks.Items.Add(v_sKey, "No", "")
			End If
			
			ListViewHelper.GetListViewSubItem(oListItem, ACSTStatusCol).Text = v_sTaskStatusDesc.Trim()
			ListViewHelper.GetListViewSubItem(oListItem, ACSTDueDateCol).Text = gPMFunctions.FormatField(iformattype:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vfieldvalue:=v_dtTaskDueDate)
			ListViewHelper.GetListViewSubItem(oListItem, ACSTDescriptionCol).Text = v_sDescription.Trim()
			ListViewHelper.GetListViewSubItem(oListItem, ACSTCustomerCol).Text = v_sCustomer.Trim()
			ListViewHelper.GetListViewSubItem(oListItem, ACSTTaskTypeCol).Text = v_sTaskTypedesc.Trim()
			ListViewHelper.GetListViewSubItem(oListItem, ACSTUserGroupCol).Text = v_sUserGroup.Trim()
			ListViewHelper.GetListViewSubItem(oListItem, ACSTUserCol).Text = v_sUser.Trim()
			ListViewHelper.GetListViewSubItem(oListItem, ACSTDueDateSortableCol).Text = v_dtTaskDueDate.ToString("yyyyMMdd")
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddScheduledTaskToListFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddScheduledTaskToList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: UpdateScheduledTask
	'
	' Description: Updates a Scheduled Task.
	'
	' ***************************************************************** '
	Public Sub UpdateScheduledTask(ByVal v_sKey As String, Optional ByVal v_vTaskStatusDesc As String = "", Optional ByVal v_vTaskDueDate As Object = Nothing, Optional ByVal v_vDescription As String = "", Optional ByVal v_vCustomer As String = "", Optional ByVal v_vTaskTypedesc As String = "", Optional ByVal v_vUserGroup As String = "", Optional ByVal v_vUser As String = "")
		
		Dim oListItem As ListViewItem
		
		Try 
			
			' Get the Scheduled Task From the List
			oListItem = lstScheduledTasks.Items.Item(v_sKey)
			
			' If we have got it, update it
			If oListItem Is Nothing Then
			Else
				' Update the value for each column that we have been supplied.

				If Not Information.IsNothing(v_vTaskStatusDesc) Then
					ListViewHelper.GetListViewSubItem(oListItem, 1).Text = v_vTaskStatusDesc.Trim()
				End If

				If Not Information.IsNothing(v_vTaskDueDate) Then
					ListViewHelper.GetListViewSubItem(oListItem, 2).Text = gPMFunctions.FormatField(iformattype:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vfieldvalue:=v_vTaskDueDate)
				End If

				If Not Information.IsNothing(v_vDescription) Then
					ListViewHelper.GetListViewSubItem(oListItem, 3).Text = v_vDescription.Trim()
				End If

				If Not Information.IsNothing(v_vCustomer) Then
					ListViewHelper.GetListViewSubItem(oListItem, 4).Text = v_vCustomer.Trim()
				End If

				If Not Information.IsNothing(v_vTaskTypedesc) Then
					ListViewHelper.GetListViewSubItem(oListItem, 5).Text = v_vTaskTypedesc.Trim()
				End If

				If Not Information.IsNothing(v_vUserGroup) Then
					ListViewHelper.GetListViewSubItem(oListItem, 6).Text = v_vUserGroup.Trim()
				End If
				' Note: This needs to be a variant so we can use the IsMissing function
				'       as a user of "" is valid.

				If Not Information.IsNothing(v_vUser) Then
					ListViewHelper.GetListViewSubItem(oListItem, 7).Text = v_vUser.Trim()
				End If
			End If
		
		Catch excep As System.Exception
			
			
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateScheduledTaskFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateScheduledTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: DeleteScheduledTask
	'
	' Description: Deletes a Scheduled Task.
	'
	' ***************************************************************** '
	Public Sub DeleteScheduledTask(ByVal v_sKey As String)
		
        Try

            ' Get the Scheduled Task From the List
            lstScheduledTasks.Items.RemoveAt(CInt(v_sKey) - 1)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteScheduledTaskFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteScheduledTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: SetForDisplay
	'
	' Description: Sets the Form defaults for initial display.
	'
	'
	' ***************************************************************** '
	Public Function SetForDisplay() As Integer
		Dim result As Integer = 0
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim lPMAuthorityLevel As gPMConstants.PMEAuthorityLevel
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' If this User a System Administrator or Normal User
			lReturn = CType(Parent_Renamed.GetUserAuthority(r_lPMAuthorityLevel:=lPMAuthorityLevel), gPMConstants.PMEReturnCode)
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If lPMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALSysAdmin Then
				' Add an All Groups Entry in the UserGroups List
				' and Refresh the List of Users
				cboUserGroup.FirstItem = ACUserGroupAllGroups
				RefreshUserList()
			Else
				' Add a Your Groups entry and tell the control
				' to only list the groups that the user is a member of.
				cboUserGroup.FirstItem = ACUserGroupYourGroups
				cboUserGroup.PMUserID = Parent_Renamed.UserID
				RefreshUserList()
			End If
			
			UpdateStatusBar(v_vPMAuthorityLevel:=lPMAuthorityLevel)
			
			FormDisplayed = True
			
			' Initially Sort Scheduled Tasks by Due Date Ascending
			With lstScheduledTasks
				' Initially Sort by Due Date, oldest first.
				ListViewHelper.SetSortedProperty(lstScheduledTasks, True)
				mnuViewSortBy_Click(mnuViewSortBy(ACSTDueDateCol + 1), New EventArgs())
                ListViewHelper.SetSortOrderProperty(lstScheduledTasks, Windows.Forms.SortOrder.Ascending)
				' Set the sortable due date column to zer width
				' so the user cannot see it.
				.Columns.Item(ACSTDueDateSortableCol).Width = CInt(0)
			End With
			
			tbToolBar.Visible = ViewToolbar
			sbStatusBar.Visible = ViewStatusBar
			
			' Are we showing the Grid Lines
			If Not ViewGridLines Then
				lReturn = CType(SetExtraListViewProperties(v_hWndList:=lstScheduledTasks.Handle.ToInt32(), v_vShowGridLines:=False), gPMConstants.PMEReturnCode)
			Else
				lReturn = CType(SetExtraListViewProperties(v_hWndList:=lstScheduledTasks.Handle.ToInt32(), v_vShowGridLines:=True), gPMConstants.PMEReturnCode)
			End If
			
			Me.WindowState = FormWindowState.Normal
			ResetControlPositions()
			
			' Add the Username to the Form Caption
			Me.Text = Me.Text & m_sKeyName & " = " & m_sKeyValue
			
			'RFC150399 - Add Full Row Select & Grid Lines to List View
			'DAK241299 - Set grid lines to ViewGridLines property
			lReturn = CType(SetExtraListViewProperties(v_hWndList:=lstScheduledTasks.Handle.ToInt32(), v_vShowRowSelect:=True, v_vShowGridLines:=ViewGridLines), gPMConstants.PMEReturnCode)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetForDisplayFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetForDisplay", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: RefreshUserList
	'
	' Description: Refreshes the List of Users, based on the selected
	'              User Group.
	'
	' ***************************************************************** '
	Private Sub RefreshUserList()
		Dim lPMUserGroupID As Integer
		Dim lPMAuthorityLevel As gPMConstants.PMEAuthorityLevel
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			' Get the Currently Selected User GroupID
			lPMUserGroupID = cboUserGroup.UserGroupID
			
			' If this User a System Administrator,
			' Group Supervisor or Normal User
			lReturn = CType(Parent_Renamed.GetUserAuthority(r_lPMAuthorityLevel:=lPMAuthorityLevel, v_lUserGroupID:=lPMUserGroupID), gPMConstants.PMEReturnCode)
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			' Set the User List based on the Authority Level
			' and group selection.
			
			Select Case lPMAuthorityLevel
				' Administrators and Group Supervisors can see All Users
				Case gPMConstants.PMEAuthorityLevel.pmeALSysAdmin, gPMConstants.PMEAuthorityLevel.pmeALSupervisor
					
					cboUser.SingleUserID = 0
					cboUser.Enabled = True
					If lPMUserGroupID > 0 Then
						cboUser.FirstItem = "All Group Users"
						cboUser.PMUserGroupID = lPMUserGroupID
					Else
						cboUser.FirstItem = "All Users"
						cboUser.PMUserGroupID = 0
					End If
					
					' Normal Users can see only themselves.
				Case gPMConstants.PMEAuthorityLevel.pmeALUser
					cboUser.SingleUserID = Parent_Renamed.UserID
					cboUser.Enabled = False
					cboUser.FirstItem = ""
					
			End Select
			
			' Refresh the List.
			cboUser.RefreshList()
			
			UpdateStatusBar(v_vPMAuthorityLevel:=lPMAuthorityLevel)
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RefreshUserListFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshUserList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: SetTaskMenuOptions
	'
	' Description: Sets the Task Menu options which are available.
	'
	' ***************************************************************** '
	Public Sub SetTaskMenuOptions(ByVal v_bEditEnabled As Boolean, ByVal v_bAssignEnabled As Boolean, ByVal v_bViewEnabled As Boolean)
		
		Try 
			
			mnuTaskEdit.Enabled = v_bEditEnabled
			mnuTaskAssign.Enabled = v_bAssignEnabled
			mnuTaskView.Enabled = v_bViewEnabled
		
		Catch excep As System.Exception
			
			
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetTaskMenuOptionsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetTaskMenuOptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: DisplayTaskMenu
	'
	' Description: Displays the Task Menu
	'
	'
	' ***************************************************************** '
	Public Sub DisplayTaskMenu()
		
		Try 
			
			Ctx_mnuTask.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayTaskMenuFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayTaskMenu", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: UpdateStatusBar
	'
	' Description: Updates the Status Bar with the Values Supplied.
	' ***************************************************************** '
	Public Sub UpdateStatusBar(Optional ByVal v_vPMAuthorityLevel As gPMConstants.PMEAuthorityLevel = 0, Optional ByVal v_vActivity As String = "", Optional ByVal v_vErrorMsg As String = "")
		
		Try 
			

			If Not Information.IsNothing(v_vPMAuthorityLevel) Then
				
				Select Case v_vPMAuthorityLevel
					Case gPMConstants.PMEAuthorityLevel.pmeALSysAdmin
						sbStatusBar.Items.Item(0).Text = ACStatusAuthSysAdmin
					Case gPMConstants.PMEAuthorityLevel.pmeALSupervisor
						sbStatusBar.Items.Item(0).Text = ACStatusAuthSupervisor
					Case Else
						sbStatusBar.Items.Item(0).Text = ACStatusAuthUser
				End Select
				
			End If
			
			sbStatusBar.Items.Item(1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateTimeShort, DateTime.Now)
			
			sbStatusBar.Items.Item(2).Text = CStr(lstScheduledTasks.Items.Count) & ACStatusItems
			

			If Not Information.IsNothing(v_vActivity) Then
				sbStatusBar.Items.Item(3).Text = v_vActivity
				If CBool(CStr(v_vActivity = "").Trim()) Then
					iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Else
					iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
				End If
			Else
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			End If
			

			If Not Information.IsNothing(v_vErrorMsg) Then
				sbStatusBar.Items.Item(3).Text = v_vErrorMsg
			End If
			
			' Refresh the Status Bar
			sbStatusBar.Refresh()
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateStatusBarFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateStatusBar", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub tmrSystemTasks_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tmrSystemTasks.Tick
		Static iCounter As Integer
		
		
		'DAK110100
		If iCounter = 0 Then
			iCounter = 1
		End If
		
		If iCounter < RefreshRate Then
			iCounter += 1
			Exit Sub
		End If
		
		'DAK080999
		' Refresh Scheduled Tasks on a regular basis
		RaiseEvent RefreshScheduledTasks(False)
		
		' Update the Date/Time on the Status Bar
		UpdateStatusBar()
		
		iCounter = 1
		
	End Sub
End Class
